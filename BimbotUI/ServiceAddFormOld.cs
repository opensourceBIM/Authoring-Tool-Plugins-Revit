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


      public String documentName { get; set; }
      public Service curService { get; private set; }

      private Dictionary<string, Provider> ExistingProviders;
      

      public ServiceAddFormOld()
      {
         InitializeComponent();
         this.Text = "Add service to project '" + documentName + "'";

         // Fill the list of services (http request)
         GetAvailableServices();

         // Fill the combobox of triggers
         newTrigger.Items.Clear();
         foreach (string key in textToTrigger.Keys)
         {
            newTrigger.Items.Add(key);
         }
      }
      

      private void ServiceSetupForm_Load(object sender, EventArgs e)
      {
         //GetAvailableServices();
         listAvailableServices.SelectedItems.Clear();
         newSoid.Text = "";
         newToken.Text = "";
         newTrigger.SelectionLength = 0;
         SetButtonState(sender, e);

         // Update list of Providers

      }

      private string ResponseOfGetRequest(string url)
      {
         string resstr = null;
         // Create a get request
         HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
         myHttpWebRequest.Method = "GET";
         try
         {
            // Get the response of the request
            WebResponse response = myHttpWebRequest.GetResponse();
            Debug.Assert(response != null);
            using (Stream responseStream = response.GetResponseStream())
            {
               StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
               resstr = reader.ReadToEnd();
            }
         }
         catch (Exception e)
         {
            Console.Write(e);
         }
         return resstr;
      }



      private void GetAvailableServices()
      {
         try
         {
            // Clear the list
            listAvailableServices.Items.Clear();

            // Get list of known providers of BIM Bot services from GitHub 
            string resstr = ResponseOfGetRequest("https://raw.githubusercontent.com/opensourceBIM/BIMserver-Repository/master/serviceproviders.json");
            if (resstr != null)
            {
               // modify the response to add the ifcanalysis provider for now (TODO remove in future)
               resstr = resstr.Insert(resstr.LastIndexOf(']'), ", {\n" +
                         "\"name\": \"ifcanalysis.bimserver.services\", \n" +
                         "\"description\": \"Experimental services provider\", \n" +
                         "\"listUrl\": \"https://ifcanalysis.bimserver.services/servicelist\"\n" +
                         "}");

               // Deserialize the JSON response
               JsonProviderList gitProviders = JsonConvert.DeserializeObject<JsonProviderList>(resstr);

               // Insert services of each provider
               foreach (JsonProvider provider in gitProviders.active)
               {
                  JsonServiceList serviceList = provider.GetJsonServices();
                  if (serviceList != null)
                  {
                     // Add each service in the list from the JSON response
                     foreach (Service service in serviceList.Services)
                     {
                        // Add the service to the service list
                        ListViewItem item = listAvailableServices.Items.Add(service.Name);
                        item.SubItems.Add(service.Url);
                        item.Tag = service;
                        if (ExistingProviders != null)
                           service.AssignProvider(ExistingProviders);
                     }
                  }
               }
            }

            // add the provider of Kalkzandsteen bot (Thomas) for now (TODO remove in future)
            InsertFixedBotThomas();
         }
         catch (Exception e)
         {
            Console.Write(e);
         }
      }
      


      private void InsertFixedBotThomas()
      {
         // Create a Service object for the brick bot        
         Service service = new Service("Bricker BIM Bot", "Convert limestone walls to bricks", 
                                       "Bricker", "http://www.clker.com/cliparts/a/g/Z/8/6/G/blue-ibm-wall-md.png", 
                                       new List<string> { "IFC_STEP_2X3TC1" },
                                       new List<string> { "BCF_ZIP_2_0" },
                                       null, 
                                       "http://ec2-18-218-56-112.us-east-2.compute.amazonaws.com/");

         // Add the service/bot to the list of services
         ListViewItem item = listAvailableServices.Items.Add(service.Name);
         item.SubItems.Add(service.Url);
         item.Tag = service;
      }


      private void RefreshButton_Click(object sender, EventArgs e)
      {
         GetAvailableServices();
      }



      private void SetButtonState(object sender, EventArgs e)
      {
         // enable or disable the add button 
         buttonAdd.Enabled = ((listAvailableServices.SelectedItems.Count == 1) &&
                              !newTrigger.Text.Equals("") &&
                              ((!newToken.Text.Equals("") && !newSoid.Text.Equals("")) || 
                               listAvailableServices.SelectedItems[0].Text.Equals("Bricker BIM Bot"))); // Special for the Bricker BIM BOT

         // enable or disable the registrate button
         buttonRegister.Enabled = ((listAvailableServices.SelectedItems.Count == 1) &&
                                   !listAvailableServices.SelectedItems[0].Text.Equals("Bricker BIM Bot"));
      }



      private void listAvailableServices_SelectedIndexChanged(object sender, EventArgs e)
      {
         if (listAvailableServices.SelectedItems.Count != 1)
         {
            newSoid.Text = "";
            newToken.Text = "";
         }
         SetButtonState(sender, e);
      }


      private void buttonRegister_Click(object sender, EventArgs e)
      {
         Service regService = (Service)listAvailableServices.SelectedItems[0].Tag;
         byte[] data = Encoding.ASCII.GetBytes("{\n" +
            "\"type\": \"pull\",\n" +
            "\"client_name\": \"Revit file " + documentName + "\",\n" +
            "\"client_url\": \"-no url-\",\n"+
            "\"client_description\": \"A project file opened in Revit\",\n"+
            "\"client_icon\": \"https://www.itannex.com/wp-content/uploads/2016/12/revit-icon-128px-hd.png \",\n"+
            "\"redirect_url\": \"\"\n"+
            "}");

         OAuthRegisterResponse regData = null;

         HttpWebRequest request = (HttpWebRequest)WebRequest.Create(regService.Provider.RegisterUrl);
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
            Debug.Assert(response != null);
            using (Stream responseStream = response.GetResponseStream())
            {
               StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
               string str = reader.ReadToEnd();
               regData = JsonConvert.DeserializeObject<OAuthRegisterResponse>(str);
            }

            if (regData != null)
            {
               string url = regService.Provider.AuthorizeUrl +"?redirect_uri=SHOW_CODE&auth_type=service&response_type=code";
               url += "&client_id=" + regData.client_id + "&state=%7B%22_serviceName%22%3A%22" + regService.Name + "%22%7D";

               System.Diagnostics.Process.Start(url);
            }
         }
         catch (Exception ex)
         {
            Console.Write(ex);
         }
      }



      private void AddServiceClick(object sender, EventArgs e)
      {
         curService = new Service((Service)listAvailableServices.SelectedItems[0].Tag);
         curService.SetSoid(newSoid.Text.Equals("") ? -1 : Convert.ToInt32(newSoid.Text));
         curService.AddTrigger(textToTrigger[newTrigger.Text]);
         curService.AddProfile(new Bimbot.Objects.Profile(newToken.Text));

         //RevitBimbot.GetBimbotDocument(curDoc).AddService(curService);
      }

   }
   
}
