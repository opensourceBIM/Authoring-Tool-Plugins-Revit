using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;
using Newtonsoft.Json;
using Bimbot.Bcf;
using Bimbot.ExternalEvents;
using Bimbot.Objects;
using Bimbot.Utils;
using System.ComponentModel;
using System.Threading;
using Autodesk.Windows;
using System.Collections.ObjectModel;

namespace Bimbot.BimbotUI
{
   /// <summary>
   /// Interaction logic for UserControl1.xaml
   /// </summary>
   public partial class ServicesPanel : Page, Autodesk.Revit.UI.IDockablePaneProvider
   {
      #region Data
      private ExternalEvent         ExtEvent;
      private ExtEvntChangeDocument Handler;
      private ExternalEvent         RunServiceEvent;
      private ExtEvntRunServices    RunServiceHandler;
      private ExternalEvent         ImportIfcEvent;
      public BimbotDocument         botDoc;
      #endregion

      public ServicesPanel(ExternalEvent exEvent, ExtEvntChangeDocument handler, ExternalEvent runEvent, ExtEvntRunServices runHandler, ExternalEvent importIfcEvent)
      {
         try
         {
            ExtEvent = exEvent;
            Handler = handler;
            RunServiceEvent = runEvent;
            RunServiceHandler = runHandler;
            ImportIfcEvent = importIfcEvent;

            InitializeComponent();
         }
         catch (Exception ex)
         {

         }
      }

      public void ShowDocument(BimbotDocument bimDoc)
      {
         botDoc = bimDoc;
         Handler.curDoc = botDoc.revitDocument;

         UpdateView();
      }

      public void UpdateView()
      {
         servicesList.Items.Clear();
         foreach (Service serv in botDoc.registeredServices)
         {
            servicesList.Items.Add(serv);
         }
      }

      
      public void SetupDockablePane(Autodesk.Revit.UI.DockablePaneProviderData data)
      {
         data.FrameworkElement = this as FrameworkElement;
         data.InitialState = new Autodesk.Revit.UI.DockablePaneState();
         data.InitialState.DockPosition = DockPosition.Tabbed;
         data.InitialState.TabBehind = Autodesk.Revit.UI.DockablePanes.BuiltInDockablePanes.ProjectBrowser;
      }
      

      private void AddService(object sender, RoutedEventArgs e)
      {
         if (botDoc == null)
         {
            System.Windows.MessageBox.Show("The current document is not loaded, so no service can be added!");
            return;
         }

         ServiceAddFormOld form = new ServiceAddFormOld(botDoc.revitDocument);
         if (form.ShowDialog() == DialogResult.OK)
         {
            botDoc.AddService(form.curService);
            servicesList.Items.Add(form.curService);
            System.Windows.MessageBox.Show("Service '" + form.curService.Name + "' is successfully added");
         }
      }


      private void DelService(object sender, RoutedEventArgs e)
      {
         if (servicesList.SelectedItems.Count == 1)
         {
            Service curService = (Service)servicesList.SelectedItem;
            botDoc.DelService(curService);
            servicesList.Items.RemoveAt(servicesList.SelectedIndex);
         }
      }

      private void RegService(object sender, RoutedEventArgs e)
      {
         //        ImportIfcEvent.Raise();
         //Open a bcf from fixed location
      }

      private void ModService(object sender, RoutedEventArgs e)
      {
//         Bimbot.RevitBimbot.EmulateImportIfc();
      }

      private void RunSingle(object sender, RoutedEventArgs e)
      {
         if (servicesList.SelectedItems.Count != 1)
            return;

         RunServiceHandler.services.Clear();
         RunServiceHandler.services.Add((Service)servicesList.SelectedItem);
         RunServiceEvent.Raise();
      }

      private void RunAll(object sender, RoutedEventArgs e)
      {
         // Create list of tasks from services to run
         RunServiceHandler.services.Clear();
         foreach (Service curService in botDoc.registeredServices)
         {
            RunServiceHandler.services.Add(curService);
         }
         RunServiceEvent.Raise();

      }
   }
}
