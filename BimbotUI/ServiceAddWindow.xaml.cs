using Bimbot.Objects;
using Bimbot.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bimbot.BimbotUI
{
   /// <summary>
   /// Interaction logic for ServiceAddWindow.xaml
   /// </summary>
   public partial class ServiceAddWindow : Window, INotifyPropertyChanged
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

      public event PropertyChangedEventHandler PropertyChanged;

      public BimbotDocument CurrentBimbotDocument { get; private set; }

      public static ObservableCollection<Service> AvailableServices { get; set; }
     

      public RevitEvntTrigger CurrentTrigger { get; set; }

      public Service CurrentService { get; set; }
      /*public Service CurrentService
      {
         get
         {
            return _CurrentService;
         }
         set
         {
            _CurrentService = value;
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("CurrentService"));
         }
      }
      */


      public ServiceAddWindow(BimbotDocument document)
      {
         InitializeComponent();
         DataContext = this;
         CurrentBimbotDocument = document;

         // Get services
         if (AvailableServices == null)
         {
            AvailableServices = new ObservableCollection<Service>();
            GetAvailableServices();
         }
         ReUseAuthorizedProviders(document);
                 
         this.Title = "Add service to project '" + CurrentBimbotDocument.Name + "'";

         // Fill the combobox of triggers
         newTrigger.Items.Clear();
         foreach (string key in textToTrigger.Keys)
         {
            newTrigger.Items.Add(key);
         }

         // Fill the combobox of configurations
         IFCExportConfigurationsMapCustom configurationsMap = new IFCExportConfigurationsMapCustom();
         foreach (var config in configurationsMap.Values)
         {
            newConfiguration.Items.Add(config);
         }
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



      public void GetAvailableServices()
      {
         try
         {
            // Clear the list
            AvailableServices.Clear();

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
                        AvailableServices.Add(service);
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


      public void ReUseAuthorizedProviders(BimbotDocument document)
      {
         foreach (Service service in AvailableServices)
         {
            if (document.RegisteredProviders.ContainsKey(service.Provider.Name))
               service.ReAssignProvider(document.RegisteredProviders);
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
         AvailableServices.Add(service);
      }


      private void RefreshButton_Click(object sender, RoutedEventArgs e)
      {
         GetAvailableServices();
         ReUseAuthorizedProviders(CurrentBimbotDocument);
      }



      private void SetButtonState(object sender, EventArgs e)
      {
         // enable or disable the add button 
         ButtonAdd.IsEnabled = (CurrentService != null &&
                                !newTrigger.Text.Equals("") &&
                                (!newToken.Text.Equals("") ||
                                 CurrentService.Name.Equals("Bricker BIM Bot"))); // Special for the Bricker BIM BOT

         // enable or disable the registrate button
         ButtonAuthorize.IsEnabled = (CurrentService != null &&
                                      !CurrentService.Name.Equals("Bricker BIM Bot"));
      }


      /*
            private void listAvailableServices_SelectedIndexChanged(object sender, EventArgs e)
            {
               if (ListServices.SelectedItems.Count != 1)
               {
                  newSoid.Text = "";
                  newToken.Text = "";
               }
               SetButtonState(sender, e);
            }
      */


      private void buttonAuthorize_Click(object sender, RoutedEventArgs e)
      {
         try
         {
            if (CurrentService.Provider.ClientId == "")
            {
               CurrentService.Provider.RegisterForDocument(CurrentBimbotDocument);
            }

            string url = CurrentService.Provider.AuthorizeUrl + "?redirect_uri=SHOW_CODE&auth_type=service&response_type=code";
            url += "&client_id=" + CurrentService.Provider.ClientId + "&state=%7B%22_serviceName%22%3A%22" + CurrentService.Name + "%22%7D";

            System.Diagnostics.Process.Start(url);
         }
         catch (Exception ex)
         {
            Console.Write(ex);
         }
      }



      private void AddServiceClick(object sender, RoutedEventArgs e)
      {
//         CurrentService.SetSoid(newSoid.Text.Equals("") ? -1 : Convert.ToInt32(newSoid.Text));
         CurrentService.AddTrigger(textToTrigger[newTrigger.Text]);
         CurrentService.SetAutherizationCode(newToken.Text);
//         CurrentService.AddUser(new Bimbot.Objects.User(newToken.Text));

         DialogResult = true;
      }
   }
}
