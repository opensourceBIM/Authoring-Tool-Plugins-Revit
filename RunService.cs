using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace Bimbot
{
   public static class RunService
   {
      private const int ZIP_LEAD_BYTES = 0x04034b50;
      private const ushort GZIP_LEAD_BYTES = 0x8b1f;

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




      public static Entity Start(Document doc, Entity service)
      {
         WebRequest myWebRequest;
         myWebRequest = WebRequest.Create(service.Get<string>("srvcUrl") + "/" + service.Get<int>("srvcSoid").ToString());

         HttpWebRequest myHttpWebRequest = (HttpWebRequest)myWebRequest;
         myHttpWebRequest.Method = "POST";
         myHttpWebRequest.Headers.Add("Input-Type", "IFC_STEP_2X3TC1");
         myHttpWebRequest.Headers.Add("Token", service.Get<string>("srvcToken"));

         Stream requestStream = myHttpWebRequest.GetRequestStream();
         {
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            ExportProjectToIFC(doc, path);

            byte[] data = File.ReadAllBytes(Path.Combine(path, "tmp.ifc"));
            requestStream.Write(data, 0, data.Length);
         }

         WebResponse response = myHttpWebRequest.GetResponse() as HttpWebResponse;
         if (response != null)
         {
            using (Stream responseStream = response.GetResponseStream())
            {
               if (responseStream != null)
               {
                  BinaryReader binReader = new BinaryReader(responseStream);
                  byte[] bytes;

                  using (MemoryStream ms = new MemoryStream())
                  {
                     int read;
                     byte[] buffer = new byte[16 * 1024];
                     while ((read = binReader.Read(buffer, 0, buffer.Length)) > 0)
                     {
                        ms.Write(buffer, 0, read);
                     }
                     bytes = ms.ToArray();
                  }

                  bool isBcf = IsCompressedData(bytes);
                  string resstr;
                  if (isBcf)
                     resstr = Convert.ToBase64String(bytes);
                  else
                     resstr = Encoding.UTF8.GetString(bytes);

                  service = InsertOutput(doc, service, resstr, isBcf ? "bcf" : "unknown");
                  
//                  MessageBox.Show(resstr);
               }
            }
         }
         return service;
      }

      private static Entity InsertOutput(Document doc, Entity service, string result, string type)
      {
         Transaction trans = new Transaction(doc, "Insert results");
         trans.Start();


         service.Set<string>("resultType", type);
         service.Set<string>("resultData", result);
         service.Set<string>("resultDate", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));

         trans.Commit();
         return service;
      }


      private static void ExportProjectToIFC(Document doc, string path)
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

