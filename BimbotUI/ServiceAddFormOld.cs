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
         //Temp brickbot Thomas add
         InsertFixedBotThomas();
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
         if (resstr != null && !resstr.Equals(""))
         {
            ServiceListResponse serviceListResponse = JsonConvert.DeserializeObject<ServiceListResponse>(resstr);
            foreach (Service service in serviceListResponse.services)
            {
               service.host = provider;
               ListViewItem item = listAvailableServices.Items.Add(/*service.Name.Equals("FixedFileService") ? "Validate Model Service" :*/ service.Name);
               item.SubItems.Add(service.ResourceUrl);
               item.Tag = service;
            }
         }
      }

      private void InsertFixedBotThomas()
      {
         //Open a bcf from fixed location
        
         Service service = new Service(7654321, "Bricker BIM Bot", "Convert limestone walls to bricks", 
                                       "Bricker", "http://www.clker.com/cliparts/a/g/Z/8/6/G/blue-ibm-wall-md.png", 
                                       new List<string> { "IFC_STEP_2X3TC1" },
                                       new List<string> { "BCF_ZIP_2_0" },
                                       null, 
                                       "http://ec2-18-218-56-112.us-east-2.compute.amazonaws.com/");

         ListViewItem item = listAvailableServices.Items.Add(service.Name);
         item.SubItems.Add(service.ResourceUrl);
         item.Tag = service;
      }




      private void EnableDisableAddButton(object sender, EventArgs e)
      {
         buttonAdd.Enabled = ((listAvailableServices.SelectedItems.Count == 1) &&
                              !newTrigger.Text.Equals("") &&
                              ((!newToken.Text.Equals("") && !newSoid.Text.Equals("")) || 
                               listAvailableServices.SelectedItems[0].Text.Equals("Bricker BIM Bot"))); // Special for the Bricker BIM BOT
      }


      private void AddServiceClick(object sender, EventArgs e)
      {
         curService = (Service)listAvailableServices.SelectedItems[0].Tag;
         curService.soid = newSoid.Text.Equals("") ? -1 : Convert.ToInt32(newSoid.Text);
         curService.srvcToken = newToken.Text;
         curService.Trigger = textToTrigger[newTrigger.Text];

         //RevitBimbot.GetBimbotDocument(curDoc).AddService(curService);
      }

      private void listAvailableServices_SelectedIndexChanged(object sender, EventArgs e)
      {
         if (listAvailableServices.SelectedItems.Count == 1)
         {
            Service curService = (Service)listAvailableServices.SelectedItems[0].Tag;

/*            if (curService.Name.Equals("Simple Analyses Service"))
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
*/
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
            "\"type\": \"pull\",\n" +
            "\"client_name\": \"Revit file " + curDoc.PathName.Substring(curDoc.PathName.LastIndexOf('\\')+ 1) + "\",\n" +
            "\"client_url\": \"-no url-\",\n"+
            "\"client_description\": \"A project file opened in Revit\",\n"+
            "\"client_icon\": \"https://www.itannex.com/wp-content/uploads/2016/12/revit-icon-128px-hd.png\",\n"+
            "\"redirect_url\": \"\"\n"+
            "}");

         OAuthRegisterResponse regData = null;

         HttpWebRequest request = (HttpWebRequest)WebRequest.Create(curService.Oauth.registerUrl);
         request.Method = "POST";
         request.ContentType = "application/json";
         request.ContentLength = data.Length;
         try
         {
            using (Stream requestStream = request.GetRequestStream())
            {
               requestStream.Write(data, 0, data.Length);
            }

            WebResponse response = request.GetResponse() as HttpWebResponse;
            if (response != null)
            {
               using (Stream responseStream = response.GetResponseStream())
               {
                  if (responseStream != null)
                  {
                     StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                     string str = reader.ReadToEnd();
                     regData = JsonConvert.DeserializeObject<OAuthRegisterResponse>(str);
                  }
               }
            }

            if (regData != null)
            {
               string url = curService.Oauth.authorizationUrl +"?redirect_uri=SHOW_CODE&auth_type=service&response_type=code";
               url += "&client_id=" + regData.client_id + "&state=%7B%22_serviceName%22%3A%22" + curService.Name + "%22%7D";

               System.Diagnostics.Process.Start(url);
            }
         }
         catch (Exception ex)
         {
            Console.Write(ex);
         }
      }
   }
   
}
