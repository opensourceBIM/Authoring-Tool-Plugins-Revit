using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using Bimbot.Objects;
using Newtonsoft.Json;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB;
using System.Diagnostics;

namespace Bimbot.BimbotUI
{
   public partial class ServiceAddFormOld : System.Windows.Forms.Form
   {
      public static readonly Dictionary<string, RevitEvntTrigger> textToTrigger = new Dictionary<string, RevitEvntTrigger>
      {
         {"button",                   RevitEvntTrigger.manualButton },
         {"structural element changed",RevitEvntTrigger.manualButton },
         {"saved document",           RevitEvntTrigger.documentSaved },
         {"closed document",          RevitEvntTrigger.documentClosed },
         {"opened document",          RevitEvntTrigger.documentOpened },
         {"modified document",        RevitEvntTrigger.documentModified },
         {"committed transaction",    RevitEvntTrigger.transactionCommitted },
         {"all",                      RevitEvntTrigger.all }
      };


      public Service curService { get; private set; }
      private Document curDoc;


      public ServiceAddFormOld(Document doc)
      {
         InitializeComponent();
         curDoc = doc;

         // Set Trigger comboboxes
         newTrigger.Items.Clear();
         foreach (string key in textToTrigger.Keys)
         {
            newTrigger.Items.Add(key);
         }
      }
      

      private void ServiceSetupForm_Load(object sender, EventArgs e)
      {
         // Fill available services
         listAvailableServices.Items.Clear();

         // Get servers 
         string resstr = ResponseOfGetRequest("https://raw.githubusercontent.com/opensourceBIM/BIMserver-Repository/master/serviceproviders.json");
         if (resstr != null)
         {
            resstr = resstr.Insert(resstr.LastIndexOf(']'), ", {\n" +
                      "\"name\": \"ifcanalysis.bimserver.services\", \n" +
                      "\"description\": \"Experimental services provider\", \n" +
                      "\"listUrl\": \"https://ifcanalysis.bimserver.services/servicelist\"\n" +
                      "}");

            ServiceProviderResponse serviceProviderResponse = JsonConvert.DeserializeObject<ServiceProviderResponse>(resstr);
            foreach (SProvider provider in serviceProviderResponse.active)
            {
               InsertServicesFromProvider(provider);
            }
         }
         buttonAdd.Enabled = false;
      }


      private string ResponseOfGetRequest(string url)
      {
         string resstr = null;

         WebRequest myWebRequest = WebRequest.Create(url);
         HttpWebRequest myHttpWebRequest = (HttpWebRequest)myWebRequest;
         myHttpWebRequest.Method = "GET";
         try
         {
            WebResponse response = myHttpWebRequest.GetResponse();
            if (response != null)
            {
               Stream responseStream = response.GetResponseStream();
               if (responseStream != null)
               {
                  StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                  resstr = reader.ReadToEnd();
                  responseStream.Close();
               }
            }
         }
         catch (Exception e)
         {
            Console.Write(e);
         }
         return resstr;
      }

      private void InsertServicesFromProvider(SProvider provider)
      {
         string resstr = ResponseOfGetRequest(provider.listUrl);
         if (resstr != null)
         {
            ServiceListResponse serviceListResponse = JsonConvert.DeserializeObject<ServiceListResponse>(resstr);
            foreach (Service service in serviceListResponse.services)
            {
               service.host = provider;
               ListViewItem item = listAvailableServices.Items.Add(service.Name.Equals("FixedFileService") ? "Validate Model Service" : service.Name);
               item.SubItems.Add(service.Provider);
               item.Tag = service;
            }
         }
      }

      private void EnableDisableAddButton(object sender, EventArgs e)
      {
         buttonAdd.Enabled = ((listAvailableServices.SelectedItems.Count == 1) &&
                              !newTrigger.Text.Equals("") &&
                              !newToken.Text.Equals("") &&
                              !newSoid.Text.Equals(""));
      }


      private void AddServiceClick(object sender, EventArgs e)
      {
         Service curService = (Service)listAvailableServices.SelectedItems[0].Tag;
         curService.soid = Convert.ToInt32(newSoid.Text);
         curService.srvcToken = newToken.Text;
         curService.Trigger = textToTrigger[newTrigger.Text];


         // Add the configuration to the revit file as well (make triggering work on reopening the document)
         Transaction transaction = new Transaction(curDoc, "tAddEntity");
         transaction.Start();

         // create an entity (object) for this schema (class)
         curService.serviceEntity = new Entity(RevitBimbot.ServiceSchema);


         Entity entity = curDoc.ProjectInformation.GetEntity(RevitBimbot.Schema);
         if (!entity.IsValid()) entity = new Entity(RevitBimbot.Schema);

         // get the field from the schema
         curService.serviceEntity.Set<string>("hostName", curService.host.name);
         curService.serviceEntity.Set<string>("hostUrl", curService.host.listUrl);
         curService.serviceEntity.Set<string>("hostDesc", curService.host.description);

         curService.serviceEntity.Set<string>("srvcName", curService.Name);
         curService.serviceEntity.Set<int>("srvcId", curService.Id);
         curService.serviceEntity.Set<string>("srvcDesc", curService.Description);
         curService.serviceEntity.Set<string>("srvcProvider", curService.Provider);
         curService.serviceEntity.Set<string>("srvcProvIcon", curService.ProviderIcon);
         curService.serviceEntity.Set<IList<string>>("srvcInputs", curService.Inputs);
         curService.serviceEntity.Set<IList<string>>("srvcOutputs", curService.Outputs);
         curService.serviceEntity.Set<string>("srvcUrl", curService.ResourceUrl);
         curService.serviceEntity.Set<int>("srvcSoid", curService.soid);
         curService.serviceEntity.Set<string>("srvcToken", curService.srvcToken);
         curService.serviceEntity.Set<int>("srvcTrigger", (int)curService.Trigger);

         //Get values (services) currently in the schema and add the new service
         IList<Entity> orgServices = entity.Get<IList<Entity>>("services");

         orgServices.Add(curService.serviceEntity);
         entity.Set<IList<Entity>>("services", orgServices); // set the value for this entity

         curDoc.ProjectInformation.SetEntity(entity); // store the entity in the element
         transaction.Commit();
         MessageBox.Show("Service '" + curService.Name + "' is successfully added");
      }

      private void listAvailableServices_SelectedIndexChanged(object sender, EventArgs e)
      {
         if (listAvailableServices.SelectedItems.Count == 1)
         {
            Service curService = (Service)listAvailableServices.SelectedItems[0].Tag;

            if (curService.Name.Equals("Simple Analyses Service"))
            {
               if (curService.host.listUrl.Equals("https://ifcanalysis.bimserver.services/servicelist"))
               {
                  newSoid.Text = "720974";
                  //                  newToken.Text = "70a379ff10141931ee62e598c2f645e942d658e8a9ec98151dc31065ce60478abe5fe392cedc2c92233015176c6198bb00fab77a88bdd1c939576ee512d7f059";
                  newToken.Text = "4e620dd2b4af8e702e41ad082fcb9552368331a8853e3cc5ace6504e453214e3be5fe392cedc2c92233015176c6198bb00fab77a88bdd1c939576ee512d7f059";
               }
               else if (curService.host.listUrl.Equals("http://localhost:8082/servicelist"))
               {
                  newSoid.Text = "262222";
                  newToken.Text = "2f7c4a24267f95729de33ad384a0a10bcf2390d64011201077402a2dde3255d2079360b3e4655e74924e57608bdddca413870da2ff7fb081e3ee88ed176ad0c0";
               }
            }
            else if (curService.Name.Equals("FixedFileService"))
            {
               if (curService.host.listUrl.Equals("https://ifcanalysis.bimserver.services/servicelist"))
               {
                  newSoid.Text = "327758";
                  newToken.Text = "895f555dbe081dbec5b8e4222678bf778b39c59793af7e40d4b7a1acae5d67429fbfdc68f3725742f07c1e5f4435f614511f9a3126250c0b634edf4b2b0ed613";
               }
            }
         }
         else
         {
            newSoid.Text = "";
            newToken.Text = "";
         }
         EnableDisableAddButton(sender, e);
      }

      private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
      {

      }

      private void buttonRegister_Click(object sender, EventArgs e)
      {
         Service curService = (Service)listAvailableServices.SelectedItems[0].Tag;
         byte[] data = Encoding.ASCII.GetBytes("{\n" +
            "\"type\": \"no idea what it's for\",\n" +
            "\"client_name\": \"Revit file " + curDoc.PathName.Substring(curDoc.PathName.LastIndexOf('\\')) + " \",\n" +
            "\"client_url\": \"-No URL-\",\n"+
            "\"client_description\": \"A project file opened in Revit\",\n"+
            "\"client_icon\": \"\",\n"+
            "\"redirect_url\": \"\"\n"+
            "}");


         HttpWebRequest requist = (HttpWebRequest) WebRequest.Create(curService.Oauth.registerUrl);
         requist.Method = "POST";
         requist.ContentType = "application/json";
         requist.ContentLength = data.Length;
         try
         {
            using (Stream requestStream = requist.GetRequestStream())
            {
               requestStream.Write(data, 0, data.Length);
            }

            WebResponse response = requist.GetResponse() as HttpWebResponse;
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
                        string str = bytes.ToString();
                     }
                     //InsertOutput();

                  }
               }
            }

         }
         catch (Exception ex)
         {
            Console.Write(ex);
         }





         System.Diagnostics.Process.Start("http://google.com");
      }
   }
   
}
