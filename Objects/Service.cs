using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;
using Bimbot.Bcf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Bimbot.Objects
{
   public class ServiceAuthorization
   {
   }


   public class Service
   {
      public class SOauth
      {
         public string registerUrl;
         public string authorizationUrl;
         public string tokenUrl;
      }

      private const int ZIP_LEAD_BYTES = 0x04034b50;
      private const ushort GZIP_LEAD_BYTES = 0x8b1f;


      #region JSON Attributes (do not change)
      public int Id { get; } // fieldBldr = serviceBldr.AddSimpleField("srvcId", typeof(int));
      public string Name { get; } // fieldBldr = serviceBldr.AddSimpleField("srvcName", typeof(string));
      public string Description { get; } // fieldBldr = serviceBldr.AddSimpleField("srvcDesc", typeof(string));
      public string Provider { get; } // fieldBldr = serviceBldr.AddSimpleField("srvcProvider", typeof(string));
      public string ProviderIcon { get; } // fieldBldr = serviceBldr.AddSimpleField("srvcProvIcon", typeof(string));
      public IList<string> Inputs { get; } // fieldBldr = serviceBldr.AddArrayField("srvcInputs", typeof(string));
      public IList<string> Outputs { get; } // fieldBldr = serviceBldr.AddArrayField("srvcOutputs", typeof(string));
      public SOauth Oauth { get; }
      public string ResourceUrl { get; } // fieldBldr = serviceBldr.AddSimpleField("srvcUrl", typeof(string));????????

      // Service profile rights for users and special settings. 
      public List<ServiceProfile> profile { get; }
      public int soid;
      public RevitEvntTrigger Trigger;
      //public string srvcUrl;
      public string srvcToken;

      // property not in json, but added for ease of use in application
      public SProvider host;
      #endregion



      public ServiceResult result { get; set; }

      public Entity serviceEntity;

      public string ProviderIconUrl
      {
         get {
            return ResourceUrl == null ? "" : 
               (ResourceUrl.StartsWith("http") ? ResourceUrl : 
               ResourceUrl.Substring(0, ResourceUrl.IndexOf('/',9)) + ProviderIcon);
         }
      }

      #region constructors
      // JSON constructor
      [JsonConstructor]
      public Service(int id, string name, string description, string provider, string providerIcon, 
                     List<string> inputs, List<string> outputs, SOauth oauth, string resourceUrl)
      {
         Id = id;        
         //Temporary fix for demo
         Name = /*name.Equals("FixedFileService") ? "Validate Model Service" :*/ name;
         Description = description;
         Provider = provider;
         ProviderIcon = providerIcon;
         Inputs = inputs;
         Outputs = outputs;
         Oauth = oauth;
         ResourceUrl = resourceUrl;
      }


      // Revit Extensible Storage constructor
      public Service(Entity serviceEnt)
      {
         serviceEntity = serviceEnt;

         Id = serviceEnt.Get<int>("srvcId");
         Name = serviceEnt.Get<string>("srvcName");

         //Temporary fix for demo
//         Name = Name.Equals("FixedFileService") ? "Validate Model Service" : Name;
         
         Description = serviceEnt.Get<string>("srvcDesc");
         Provider = serviceEnt.Get<string>("srvcProvider");
         ProviderIcon = serviceEnt.Get<string>("srvcProvIcon");
         Inputs = serviceEnt.Get<IList<string>>("srvcInputs");
         Outputs = serviceEnt.Get<IList<string>>("srvcOutputs");
         ResourceUrl = serviceEnt.Get<string>("srvcUrl");
         Trigger = (RevitEvntTrigger)serviceEnt.Get<int>("srvcTrigger");
         soid = serviceEnt.Get<int>("srvcSoid");
         srvcToken = serviceEnt.Get<string>("srvcToken");

         //         curService.serviceEntity.Set<string>("hostName", curService.host.name);
         //         curService.serviceEntity.Set<string>("hostUrl", curService.host.listUrl);
         //         curService.serviceEntity.Set<string>("hostDesc", curService.host.description);

         result = new ServiceResult();
         result.isBcf = serviceEnt.Get<string>("resultType").Equals("bcf");
         result.data = serviceEnt.Get<string>("resultData");
         result.lastRun = DateTime.Parse(serviceEnt.Get<string>("resultDate"));
         if (result.isBcf)
            result.bcf = new BcfFile(Convert.FromBase64String(result.data));

         /* temp solution to fix previous stored OAuth data */
/*         if (Name.Equals("Validate Model Service"))
         {
            if (ResourceUrl.Equals("https://ifcanalysis.bimserver.services/services"))
            {
               soid = 327758;
               srvcToken = "895f555dbe081dbec5b8e4222678bf778b39c59793af7e40d4b7a1acae5d67429fbfdc68f3725742f07c1e5f4435f614511f9a3126250c0b634edf4b2b0ed613";
            }
         }
 */
      }
      #endregion


      public bool ToExtensibleStorage()
      {
         /*
         fieldBldr = serviceBldr.AddSimpleField("srvcUrl", typeof(string));
         fieldBldr = serviceBldr.AddSimpleField("srvcToken", typeof(string));
         fieldBldr = serviceBldr.AddSimpleField("srvcSoid", typeof(int));
         fieldBldr = serviceBldr.AddSimpleField("resultType", typeof(string));
         fieldBldr = serviceBldr.AddSimpleField("resultData", typeof(string));
         fieldBldr = serviceBldr.AddSimpleField("resultDate", typeof(string));
         */
         return false; //failed to write
      }


      internal static bool IsPkZipCompressedData(byte[] data)
      {
         Debug.Assert(data != null && data.Length >= 4);
         // if the first 4 bytes of the array are the ZIP signature then it is compressed data
         return (BitConverter.ToInt32(data, 0) == ZIP_LEAD_BYTES);
      }

      internal static bool IsGZipCompressedData(byte[] data)
      {
         Debug.Assert(data != null && data.Length >= 2);
         // if the first 2 bytes of the array are theG ZIP signature then it is compressed data;
         return (BitConverter.ToUInt16(data, 0) == GZIP_LEAD_BYTES);
      }

      public static bool IsCompressedData(byte[] data)
      {
         return IsPkZipCompressedData(data) || IsGZipCompressedData(data);
      }
      
      public string Run(byte[] data)
      {
         if (soid <= 0)
         {
            // Special for Thomas Brick Bot not to run online
            byte[] fileBytes = File.ReadAllBytes("C:\\DataLocal\\RunningProjects\\DemoLeon\\result_final.bcf");

            result = new ServiceResult();
            result.isBcf = true;
            result.data = Convert.ToBase64String(fileBytes);
            result.bcf = new BcfFile(fileBytes);
            result.lastRun = DateTime.Now;
         }
         else
         {
            WebRequest myWebRequest;
            myWebRequest = WebRequest.Create(ResourceUrl + (soid == -1 ? "" : "/" + soid.ToString()));

            HttpWebRequest myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.Method = "POST";
            myHttpWebRequest.Headers.Add("Input-Type", "IFC_STEP_2X3TC1");
            myHttpWebRequest.Headers.Add("Token", srvcToken);
            myHttpWebRequest.Headers.Add("Accept-Flow", "SYNC");

            Stream requestStream = myHttpWebRequest.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            //         Thread.Sleep(1000);
            WebResponse response = myHttpWebRequest.GetResponse() as HttpWebResponse;
            if (response != null)
            {
               using (Stream responseStream = response.GetResponseStream())
               {
                  if (responseStream != null)
                  {
                     //                  BinaryReader binReader = new BinaryReader(responseStream);
                     byte[] bytes;

                     using (MemoryStream ms = new MemoryStream())
                     {
                        responseStream.CopyTo(ms);
                        bytes = ms.ToArray();
                     }

                     result = new ServiceResult();

                     if (bytes.Length == 0)
                     {
                        result.isBcf = false;
                        result.data = "-Empty response-";
                     }
                     else
                     {
                        result.isBcf = IsCompressedData(bytes);

                        if (result.isBcf)
                           result.data = Convert.ToBase64String(bytes);
                        else
                           result.data = Encoding.UTF8.GetString(bytes);
                     }
                     result.lastRun = DateTime.Now;

                     //InsertOutput();
                  }
               }
            }
         }
         return result.data != null ? result.data : "Service Failed";
      }


      public async Task<string> RunAsync(byte[] data)
      {
         WebRequest myWebRequest;
         myWebRequest = WebRequest.Create(ResourceUrl + "/" + soid.ToString());

         HttpWebRequest myHttpWebRequest = (HttpWebRequest)myWebRequest;
         myHttpWebRequest.Method = "POST";
         myHttpWebRequest.Headers.Add("Input-Type", "IFC_STEP_2X3TC1");
         myHttpWebRequest.Headers.Add("Token", srvcToken);
         myHttpWebRequest.Headers.Add("Accept-Flow", "SYNC");

         Stream requestStream = myHttpWebRequest.GetRequestStream();
         requestStream.Write(data, 0, data.Length);
//         Thread.Sleep(1000);
         WebResponse response = await myHttpWebRequest.GetResponseAsync() as HttpWebResponse;
         if (response != null)
         {
            using (Stream responseStream = response.GetResponseStream())
            {
               if (responseStream != null)
               {
                  //                  BinaryReader binReader = new BinaryReader(responseStream);
                  byte[] bytes;

                  using (MemoryStream ms = new MemoryStream())
                  {
                     await responseStream.CopyToAsync(ms);
                     bytes = ms.ToArray();
                  }

                  result = new ServiceResult();
                  if (bytes.Length == 0)
                  {
                     result.isBcf = false;
                     result.data = "-Empty response-";
                  }
                  else
                  {
                     result.isBcf = IsCompressedData(bytes);

                     if (result.isBcf)
                        result.data = Convert.ToBase64String(bytes);
                     else
                        result.data = Encoding.UTF8.GetString(bytes);
                  }
                  result.lastRun = DateTime.Now;

                  //InsertOutput();
               }
            }
         }
         return result.data != null ? result.data : "Service Failed";
      }



      private void ExportProjectToIFC(Document doc, string path)
      {
         IFCExportOptions ifcOptions = new IFCExportOptions();
         {
            ifcOptions.FileVersion = IFCVersion.IFC2x3;
            ifcOptions.WallAndColumnSplitting = false;
            ifcOptions.SpaceBoundaryLevel = 1;
            ifcOptions.ExportBaseQuantities = false;

            ifcOptions.AddOption("ExportInternalRevitPropertySets", "false");
            ifcOptions.AddOption("ExportIFCCommonPropertySets", "true");
            ifcOptions.AddOption("ExportAnnotations", "false");
            ifcOptions.AddOption("Use2DRoomBoundaryForVolume", "false");
            ifcOptions.AddOption("UseFamilyAndTypeNameForReference", "false");
            ifcOptions.AddOption("ExportVisibleElementsInView", "false");
            ifcOptions.AddOption("ExportPartsAsBuildingElements", "false");
            ifcOptions.AddOption("UseActiveViewGeometry", "false");
            ifcOptions.AddOption("ExportSpecificSchedules", "false");
            ifcOptions.AddOption("ExportBoundingBox", "false");
            ifcOptions.AddOption("ExportSolidModelRep", "false");
            ifcOptions.AddOption("ExportSchedulesAsPsets", "false");
            ifcOptions.AddOption("ExportUserDefinedPsets", "false");
            ifcOptions.AddOption("ExportUserDefinedParameterMapping", "false");
            ifcOptions.AddOption("ExportLinkedFiles", "false");
            ifcOptions.AddOption("IncludeSiteElevation", "false");
            ifcOptions.AddOption("TessellationLevelOfDetail", "0.5");
            ifcOptions.AddOption("StoreIFCGUID", "true");
         }
         /*            // get the revit form and set its cursor to busy
                     System.Windows.Forms.Control form = System.Windows.Forms.Control.FromHandle(Process.GetCurrentProcess().MainWindowHandle);
                     if (null != form) form.Cursor = Cursors.WaitCursor;

                     //RevitStatusText.Set("Exporting Revit Project to IFC");
                     Application.DoEvents();
         */
         Transaction trans = new Transaction(doc, "export model");
         trans.Start();

         // revit doesn't allow the export to run in a different thread
         doc.Export(path, "tmp.ifc", ifcOptions);

         trans.Commit();
      }

   }
}
