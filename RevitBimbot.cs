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
   /// <summary> 
   /// Show dockable dialog for Results (output)
   /// </summary> 
   [Transaction(TransactionMode.ReadOnly)]
   public class ToggleResults : IExternalCommand
   {
      public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         DockablePane dp = commandData.Application.GetDockablePane(RevitBimbot.OutputPaneId);
         if (RevitBimbot.PluginToggleButtonResults.ItemText.Equals("Hide Results"))
         {
            RevitBimbot.PluginToggleButtonResults.ItemText = "Show Results";
            dp.Hide();
         }
         else
         {
            RevitBimbot.PluginToggleButtonResults.ItemText =  "Hide Results";
            dp.Show();
         }
         return Result.Succeeded;
      }
   }



   /// <summary> 
   /// Show dockable dialog for services 
   /// </summary> 
   [Transaction(TransactionMode.ReadOnly)]
   public class ToggleServices : IExternalCommand
   {
      public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         DockablePane dp = commandData.Application.GetDockablePane(RevitBimbot.ServicePaneId);
         if (RevitBimbot.PluginToggleButtonServices.ItemText.Equals("Hide Services"))
         {
            RevitBimbot.PluginToggleButtonServices.ItemText = "Show Services";
            dp.Hide();
         }
         else
         {
            RevitBimbot.PluginToggleButtonServices.ItemText = "Hide Services";
            dp.Show();
         }
         return Result.Succeeded;
      }
   }



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

      public static adWin.RibbonButton ViewToggleButtonResults;
      public static adWin.RibbonButton ViewToggleButtonServices;
      public static RibbonItem PluginToggleButtonResults;
      public static RibbonItem PluginToggleButtonServices;

      public static RevitBimbot Instance
      {
         get { return curApp; }
      }

      #endregion

      #region fields
      UIControlledApplication uiApplication;
      public ExternalEventsContainer ExtEvents { get; private set; }

      private ResultsPanel DockableResultPanel = null;
      private DockablePane outputPane;
      private ServicesPanel DockableServicesPanel = null;
      private DockablePane servicePane;




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

            PluginToggleButtonResults = panel.AddItem(new PushButtonData("ToggleResults", "Hide Results", Assembly.GetExecutingAssembly().Location, "Bimbot.ToggleResults"));
            SetImage(PluginToggleButtonResults as PushButton, Properties.Resources.BIMserver_BimBot);
            PluginToggleButtonResults.ToolTip = "Toggles the window results";
            PluginToggleButtonResults.LongDescription = "Showing or hiding the last responses of the BIM Bot services performed";

            PluginToggleButtonServices = panel.AddItem(new PushButtonData("ToggleServices", "Hide Services", Assembly.GetExecutingAssembly().Location, "Bimbot.ToggleServices"));
            SetImage(PluginToggleButtonServices as PushButton, Properties.Resources.BIMserver_BimBot);
            PluginToggleButtonServices.ToolTip = "Toggles the window services";
            PluginToggleButtonServices.LongDescription = "Showing or hiding the BIM Bot services assigned to this Revit project";



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
            ViewToggleButtonResults = new adWin.RibbonToggleButton();
            ViewToggleButtonResults.Name = "Bimbot output";
//            ViewToggleButtonResults.Id = "ID_RESULT_BUTTON";
            ViewToggleButtonResults.IsEnabled = false;
            ViewToggleButtonResults.ToolTip = "Show the Bimbots output panel";
            ViewToggleButtonResults.PropertyChanged += new PropertyChangedEventHandler(toggleButtonResult_PropertyChanged);
            listBut.Items.Insert(listBut.Items.Count - 3, ViewToggleButtonResults);

            // Add the bimbot service view activation button
            ViewToggleButtonServices = new adWin.RibbonToggleButton();
            ViewToggleButtonServices.Name = "Bimbot services";
//            ViewToggleButtonServices.Id = "ID_SERVICE_BUTTON";
            ViewToggleButtonServices.IsEnabled = false;
            ViewToggleButtonServices.ToolTip = "Show the Bimbots services panel";
            ViewToggleButtonServices.PropertyChanged += new PropertyChangedEventHandler(toggleButtonService_PropertyChanged);
            listBut.Items.Insert(listBut.Items.Count - 3, ViewToggleButtonServices);

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
            ViewToggleButtonResults.IsChecked = e.DockableFrameShown;
         else if (e.PaneId == ServicePaneId)
            ViewToggleButtonServices.IsChecked = e.DockableFrameShown;
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

      private void Application_ViewActivated(object sender, ViewActivatedEventArgs args)
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


      private void Application_DocumentOpened(object sender, DocumentOpenedEventArgs args)
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
            ViewToggleButtonResults.IsChecked = true;
            ViewToggleButtonResults.IsEnabled = true;
            ViewToggleButtonServices.IsChecked = true;
            ViewToggleButtonServices.IsEnabled = true;
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

      public static void UpdateDocument(Document doc)
      {
         // If document exists it is already opened
         if (openedDocuments.ContainsKey(doc))
         {
            BimbotDocument curDoc = openedDocuments[doc];

            curApp.ExtEvents.ChangeDocumentHandler.documentToUpdate = curDoc;
            curApp.ExtEvents.ChangeDocumentEvent.Raise();
         }
      }

   }
}
