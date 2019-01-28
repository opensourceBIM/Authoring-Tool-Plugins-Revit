using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
//using Autodesk.Windows;
using System.Collections.Generic;
using Bimbot.BimbotUI;
using Autodesk.Revit.UI.Events;
using adWin = Autodesk.Windows;
using Bimbot.Objects;
using Bimbot.Bcf;
using Bimbot.ExternalEvents;
using System.Linq;
using System.Windows;
using System.ComponentModel;
using System.Runtime.InteropServices;

using Rvt = Autodesk.Revit.UI;


namespace Bimbot
{
	[Transaction(TransactionMode.Manual)]
	[Regeneration(RegenerationOption.Manual)]
	[Journaling(JournalingMode.NoCommandData)]
	public class RevitBimbot : IExternalApplication
	{
      #region constants
      public static readonly Guid ApplicationGuid = new Guid("a694f098-20d5-42c2-a8a0-ac7ae6857c24");
      public static readonly string VendorId = "nl.tno";

      public static readonly DockablePaneId OutputPaneId = new DockablePaneId(new Guid("{a54b4c89-0ee7-4fae-8fbd-94443f5020b5}"));
      public static readonly DockablePaneId ServicePaneId = new DockablePaneId(new Guid("{a54b4c89-0ee7-4fae-8fbd-94443f5020b6}"));
      #endregion

      #region local app
      private static RevitBimbot curApp;

      public static adWin.RibbonButton toggleButtonResults;
      public static adWin.RibbonButton toggleButtonServices;

      #endregion

      #region fields
      UIControlledApplication uiApplication;
      public ExternalEventsContainer ExtEvents { get; private set; }
      
      ResultsPanel DockableResultPanel = null;
      DockablePane outputPane;
      ServicesPanel DockableServicesPanel = null;
      DockablePane servicePane;

      private static Dictionary<Document, BimbotDocument> openedDocuments = new Dictionary<Document, BimbotDocument>();
      #endregion fields

      #region IExternalApplication Members


      public Rvt.Result OnStartup(UIControlledApplication application)
		{
//         Schema = null;
         curApp = this;
         uiApplication = application;
         ExtEvents = new ExternalEventsContainer();

         try
         {
            // Add Panel to Add_Ins tab
            RibbonPanel panel = application.CreateRibbonPanel("BIM Bot"); 

            // Add Button to the BIM Bot panel
            PushButton pushButton = panel.AddItem(new PushButtonData("BIMBot", "About BIM Bot", Assembly.GetExecutingAssembly().Location, "Bimbot.AboutAddin")) as PushButton;
            SetImage(pushButton, Properties.Resources.BIMserver_BimBot);


            // Create The DockablePanels for showing the service results and services
            DockableResultPanel = new ResultsPanel(ExtEvents);
            application.RegisterDockablePane(OutputPaneId, "BIM Bot results", DockableResultPanel as IDockablePaneProvider);

            DockableServicesPanel = new ServicesPanel(ExtEvents);
            application.RegisterDockablePane(ServicePaneId, "BIM Bot services", DockableServicesPanel as IDockablePaneProvider);

            // Get the listbutton that manages the dockable views
            adWin.RibbonListButton listBut = (adWin.RibbonListButton)adWin.ComponentManager.Ribbon.Tabs.Where(t => t.Id.Equals("View")).SelectMany(t => t.Panels).
                                                    Where(p => p.Source.Id.Equals("manageviews_shr")).SelectMany(p => p.Source.Items).
                                                    Where(b => b.Id.Equals("HID_APPLICATION_ELEMENTS_RibbonListButton")).First();

            // Add the bimbot result view activation button
            toggleButtonResults = new adWin.RibbonToggleButton();
            toggleButtonResults.Name = "Bimbot output";
//            toggleButtonResults.Id = "ID_RESULT_BUTTON";
            toggleButtonResults.IsEnabled = false;
            toggleButtonResults.ToolTip = "Show the Bimbots output panel";
            toggleButtonResults.PropertyChanged += new PropertyChangedEventHandler(toggleButtonResult_PropertyChanged);
            listBut.Items.Insert(listBut.Items.Count - 3, toggleButtonResults);

            // Add the bimbot service view activation button
            toggleButtonServices = new adWin.RibbonToggleButton();
            toggleButtonServices.Name = "Bimbot services";
//            toggleButtonServices.Id = "ID_SERVICE_BUTTON";
            toggleButtonServices.IsEnabled = false;
            toggleButtonServices.ToolTip = "Show the Bimbots services panel";
            toggleButtonServices.PropertyChanged += new PropertyChangedEventHandler(toggleButtonService_PropertyChanged);
            listBut.Items.Insert(listBut.Items.Count - 3, toggleButtonServices);

            // Set the event handler to handle te view activation buttons
//            adWin.ComponentManager.UIElementActivated += new EventHandler<adWin.UIElementActivatedEventArgs>(ComponentManager_UIElementActivated);


            // Register events
            application.DockableFrameVisibilityChanged += new EventHandler<DockableFrameVisibilityChangedEventArgs>(Application_DockableFrameVisibilityChanged);
            application.ControlledApplication.DocumentOpened += new EventHandler<DocumentOpenedEventArgs>(Application_DocumentOpened);
            application.ControlledApplication.DocumentClosing += new EventHandler<DocumentClosingEventArgs>(Application_DocumentClosing);
            application.ViewActivated += new EventHandler<ViewActivatedEventArgs>(Application_ViewActivated);
         }
         catch (Exception)
         {
            return Autodesk.Revit.UI.Result.Failed;
         }

         // Add view buttons for the Dockable panels in Revits UI
         return Rvt.Result.Succeeded;
		}


      public Rvt.Result OnShutdown(UIControlledApplication application)
      {
         // remove in startup added events
         application.DockableFrameVisibilityChanged -= Application_DockableFrameVisibilityChanged;
         application.ControlledApplication.DocumentOpened  -= Application_DocumentOpened;
         application.ControlledApplication.DocumentClosing -= Application_DocumentClosing;
         application.ViewActivated -= Application_ViewActivated;

//         adWin.ComponentManager.UIElementActivated -= ComponentManager_UIElementActivated;

         // Remove the command binding on shutdown
         //return base.OnShutdown(application);
         return Rvt.Result.Succeeded;
      }

/*
      public static void UpdateServicesPanel()
      {
  //       curApp.DockableServicesPanel.UpdateView();         
      }


      public static void UpdateResultPanel()
      {
//         curApp.DockableResultPanel.UpdateView();
      }
*/

      void toggleButtonResult_PropertyChanged(object sender, PropertyChangedEventArgs e)
      {
         if (e.PropertyName == "IsChecked")
         {
            if (((adWin.RibbonToggleButton)sender).IsChecked)
               outputPane.Show();
            else
               outputPane.Hide();
         }
      }


      void toggleButtonService_PropertyChanged(object sender, PropertyChangedEventArgs e)
      {
         if (e.PropertyName == "IsChecked")
         {
            if (((adWin.RibbonToggleButton)sender).IsChecked)
               servicePane.Show();
            else
               servicePane.Hide();
         }
      }

      void Application_DockableFrameVisibilityChanged(object sender, DockableFrameVisibilityChangedEventArgs e)
      {
         if (e.PaneId == OutputPaneId)
            toggleButtonResults.IsChecked = e.DockableFrameShown;
         else if (e.PaneId == ServicePaneId)
            toggleButtonServices.IsChecked = e.DockableFrameShown;
      }

/*
      void ComponentManager_UIElementActivated(object sender, adWin.UIElementActivatedEventArgs e)
      {
         if (e != null && e.Item != null && e.Item.Id != null)
         {
            if (e.Item.Id == "ID_RESULT_BUTTON")
            {
               // Perform the button action
               if (((adWin.RibbonToggleButton)(e.Item)).IsChecked)
                  outputPane.Show();
               else
                  outputPane.Hide();
            }
            if (e.Item.Id == "ID_SERVICE_BUTTON")
            {
               // Perform the button action
               if (((adWin.RibbonToggleButton)(e.Item)).IsChecked)
                  servicePane.Show();
               else
                  servicePane.Hide();
            }
         }
      }
     */ 

      void Application_ViewActivated(object sender, ViewActivatedEventArgs args)
      {
         if (args.Document == null)
            return;

         // If document exists it is already opened
         if (openedDocuments.ContainsKey(args.Document))
         {
            BimbotDocument curDoc = openedDocuments[args.Document];

            // Change the data context of viewing panels to the current document
            DockableResultPanel.DataContext = curDoc;
            DockableServicesPanel.DataContext = curDoc;
            ExtEvents.ChangeDocumentHandler.documentToUpdate = curDoc;
         }
      }


      public void Application_DocumentOpened(object sender, DocumentOpenedEventArgs args)
      {
         if (args.Document == null ||
             (Path.GetTempPath().StartsWith(Path.GetDirectoryName(args.Document.PathName)) &&
              args.Document.PathName.EndsWith(".ifc.RVT")))
            return;

         try
         {
            openedDocuments.Add(args.Document, new BimbotDocument(args.Document));
            BimbotDocument curDoc = openedDocuments[args.Document];

            DockableResultPanel.DataContext = curDoc;
            DockableServicesPanel.DataContext = curDoc;
            ExtEvents.ChangeDocumentHandler.documentToUpdate = curDoc;

            //            ActiveDocument = curApp.openedDocuments[args.Document];
            outputPane = uiApplication.GetDockablePane(OutputPaneId);
            servicePane = uiApplication.GetDockablePane(ServicePaneId);
            toggleButtonResults.IsChecked = true;
            toggleButtonResults.IsEnabled = true;
            toggleButtonServices.IsChecked = true;
            toggleButtonServices.IsEnabled = true;

            //Temp insert fixed service
//            openedDocuments[args.Document].AddService(new Service(6, "Limestone Brickalizer", "Generate brick distribution for limestone walls", "ifcanalysis", null, null, null, null, "http://ec2-18-218-56-112.us-east-2.compute.amazonaws.com/"));
//            openedDocuments[args.Document].registeredServices[0].soid = -1;

//            DockableResultPanel.ShowDocument(openedDocuments[args.Document]);
//            DockableServicesPanel.ShowDocument(openedDocuments[args.Document]);
         }
         catch (Exception e)
         {
            MessageBox.Show("Failed to open bimbot part in document due to:\n" + e.Message);
         }
      }


      public void Application_DocumentClosing(object sender, DocumentClosingEventArgs args)
      {
         if (args.Document == null ||
             (Path.GetTempPath().StartsWith(Path.GetDirectoryName(args.Document.PathName)) &&
              args.Document.PathName.EndsWith(".ifc.RVT")))
            return;

         // Remove the document from the list of opened documents (holding bimbot data)
         if (openedDocuments.ContainsKey(args.Document))
         {
//            ActiveDocument = null;
            openedDocuments[args.Document].Close();
            openedDocuments.Remove(args.Document);
         }
      }
      #endregion IExternalApplication Members


      private void SetImage(PushButton btn, Bitmap bitmap)
		{
			ImageSourceConverter conv = new ImageSourceConverter();
			Bitmap bmp = new Bitmap(bitmap);
			using (MemoryStream memory = new MemoryStream())
			{
				bmp.Save(memory, ImageFormat.Png);
				memory.Position = 0;
				BitmapImage img = new BitmapImage();
				img.BeginInit();
				img.StreamSource = memory;
				img.CacheOption = BitmapCacheOption.OnLoad;
				img.EndInit();
				btn.LargeImage = img;
            btn.Image = img;
			}
		}

   }
}
