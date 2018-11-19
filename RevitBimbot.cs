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
using Autodesk.Windows;
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

namespace Bimbot
{
	[Transaction(TransactionMode.Manual)]
	[Regeneration(RegenerationOption.Manual)]
	[Journaling(JournalingMode.NoCommandData)]
	public class RevitBimbot : IExternalApplication
	{
      #region constants
      private static readonly Guid SchemaGuid = new Guid("a54b4c89-0ee7-4fae-8fbd-94443f5020b3");
      private static readonly Guid ServiceGuid = new Guid("a54b4c89-0ee7-4fae-8fbd-94443f5020b4");
      private static readonly Guid ApplicationGuid = new Guid("a694f098-20d5-42c2-a8a0-ac7ae6857c24");

      public static readonly DockablePaneId OutputPaneId = new DockablePaneId(new Guid("{a54b4c89-0ee7-4fae-8fbd-94443f5020b5}"));
      public static readonly DockablePaneId ServicePaneId = new DockablePaneId(new Guid("{a54b4c89-0ee7-4fae-8fbd-94443f5020b6}"));
      private static readonly string VendorId = "nl.tno";
      #endregion

      #region local app
      private static RevitBimbot curApp;

      public static Schema Schema { get; private set; }
      public static Schema ServiceSchema { get; private set; }
      //      public static List<Service> services { get; private set; } = new List<Service>();

      public static adWin.RibbonButton toggleResultPanel;
      public static adWin.RibbonButton toggleServicesPanel;

      #endregion

      #region fields
      UIControlledApplication uiApplication;
      
      ResultsPanel DockableResultPanel = null;
      DockablePane outputPane;
      ServicesPanel DockableServicesPanel = null;
      DockablePane servicePane;

      private Dictionary<Document, BimbotDocument> openedDocuments = new Dictionary<Document, BimbotDocument>();

      #endregion fields

      #region IExternalApplication Members

      public static BimbotDocument GetBimbotDocument(Document doc)
      {
         return curApp.openedDocuments[doc];
      }






      public Result OnStartup(UIControlledApplication application)
		{
         Schema = null;
         curApp = this;

         uiApplication = application;

         // Create an event to perform a change of the view
         var changeViewHandler = new ExtEvntChangeView();
         var changeViewEvent = ExternalEvent.Create(changeViewHandler);

         var changeDocumentHandler = new ExtEvntChangeDocument();
         var changeDocumentEvent = ExternalEvent.Create(changeDocumentHandler);

         var runServicesHandler = new ExtEvntRunServices();
         var runServicesEvent = ExternalEvent.Create(runServicesHandler);

         var ifcImportHandler = new ExtEvntImportIfcSnippet();
         var ifcImportEvent = ExternalEvent.Create(ifcImportHandler);

         // Create The DockablePanels for showing the service results and services
         DockableResultPanel = new ResultsPanel(changeViewEvent, changeViewHandler, ifcImportEvent, ifcImportHandler);
         DockableServicesPanel = new ServicesPanel(changeDocumentEvent, changeDocumentHandler, runServicesEvent, runServicesHandler, ifcImportEvent);

         application.RegisterDockablePane(OutputPaneId, "BIM Bot results", DockableResultPanel as IDockablePaneProvider);
         application.RegisterDockablePane(ServicePaneId, "BIM Bot services", DockableServicesPanel as IDockablePaneProvider);



         try
         {
            // Register events
            application.DockableFrameFocusChanged += new EventHandler<DockableFrameFocusChangedEventArgs>(Application_DockableFrameFocusChanged);
            application.DockableFrameVisibilityChanged += new EventHandler<DockableFrameVisibilityChangedEventArgs>(Application_DockableFrameVisibilityChanged);
            application.ControlledApplication.DocumentOpened += new EventHandler<DocumentOpenedEventArgs>(Application_DocumentOpened);
            application.ControlledApplication.DocumentClosing += new EventHandler<DocumentClosingEventArgs>(Application_DocumentClosing);
            application.ViewActivated += new EventHandler<ViewActivatedEventArgs>(Application_ViewActivated);
         }
         catch (Exception)
         {
            return Result.Failed;
         }

         // Add view buttons for the Dockable panels in Revits UI
         return AddViewToggleButton();
		}

      public Result OnShutdown(UIControlledApplication application)
      {
         // remove in startup added events
         application.DockableFrameFocusChanged      -= Application_DockableFrameFocusChanged;
         application.DockableFrameVisibilityChanged -= Application_DockableFrameVisibilityChanged;
         application.ControlledApplication.DocumentOpened  -= Application_DocumentOpened;
         application.ControlledApplication.DocumentClosing -= Application_DocumentClosing;
         application.ViewActivated -= Application_ViewActivated;

         // Remove the command binding on shutdown
         //return base.OnShutdown(application);
         return Result.Succeeded;
      }

      public static void UpdateServicesPanel()
      {
         curApp.DockableServicesPanel.UpdateView();         
      }

      public static void UpdateResultPanel()
      {
         curApp.DockableResultPanel.UpdateView();
      }


      void Application_DockableFrameFocusChanged(object sender, DockableFrameFocusChangedEventArgs e)
      {
         if (!e.FocusGained)
            return;

         if (e.PaneId == OutputPaneId)
         {
            DockableResultPanel.UpdateView();
         }
         else if (e.PaneId == ServicePaneId)
         {
            DockableServicesPanel.UpdateView();
         }
      }


      void Application_DockableFrameVisibilityChanged(object sender, DockableFrameVisibilityChangedEventArgs e)
      {
         if (e.PaneId == OutputPaneId)
         {
            toggleResultPanel.IsChecked = e.DockableFrameShown;
            DockableResultPanel.UpdateView();
         }
         else if (e.PaneId == ServicePaneId)
         {
            toggleServicesPanel.IsChecked = e.DockableFrameShown;
            DockableServicesPanel.UpdateView();
         }
      }

      private void BimbotDocumentChanged()
      {
         DockableResultPanel.UpdateView();
         DockableServicesPanel.UpdateView();
      }


      public static void EmulateImportIfc()
      {
         TestUIForm form = new TestUIForm();
         form.Setup(adWin.ComponentManager.Ribbon);
         form.Show();
      }


      public Result AddViewToggleButton()
      {
         try
         {
            adWin.RibbonControl ribbon = adWin.ComponentManager.Ribbon;

            // find the view tab
            foreach (adWin.RibbonTab tab in ribbon.Tabs)
            {
               // skip all tabs except the view tab
               if (tab.Id != "View")
                  continue;

               // find the manageviews panel
               foreach (adWin.RibbonPanel panel in tab.Panels)
               {
                  // skip all panels except the manageviews
                  if (panel.Source.Id != "manageviews_shr")
                     continue;

                  // find the toggleviews listbutton
                  foreach (adWin.RibbonItem listBut in panel.Source.Items)
                  {
                     if (listBut.Id == "HID_APPLICATION_ELEMENTS_RibbonListButton")
                     {
                        //Add the bimbot result view activation button
                        toggleResultPanel = new adWin.RibbonToggleButton();
                        toggleResultPanel.Name = "Bimbot output";
                        toggleResultPanel.Id = "ID_RESULT_BUTTON";
                        toggleResultPanel.IsEnabled = false;
                        toggleResultPanel.IsToolTipEnabled = true;
                        toggleResultPanel.IsVisible = true;
                        toggleResultPanel.ShowImage = false; //  true;
                        toggleResultPanel.ShowText = true;
                        toggleResultPanel.ShowToolTipOnDisabled = false;
                        toggleResultPanel.Text = "Bimbot output";
                        toggleResultPanel.ToolTip = "Show the Bimbots output panel";
                        ((adWin.RibbonListButton)listBut).Items.Insert(((adWin.RibbonListButton)listBut).Items.Count - 3, toggleResultPanel);

                        //Add the bimbot service view activation button
                        toggleServicesPanel = new adWin.RibbonToggleButton();
                        toggleServicesPanel.Name = "Bimbot services";
                        toggleServicesPanel.Id = "ID_SERVICE_BUTTON";
                        toggleServicesPanel.IsEnabled = false;
                        toggleServicesPanel.IsToolTipEnabled = true;
                        toggleServicesPanel.IsVisible = true;
                        toggleServicesPanel.ShowImage = false; //  true;
                        toggleServicesPanel.ShowText = true;
                        toggleServicesPanel.ShowToolTipOnDisabled = false;
                        toggleServicesPanel.Text = "Bimbot services";
                        toggleServicesPanel.ToolTip = "Show the Bimbots services panel";
//                        toggleServicesPanel.HostEvent += new System.ComponentModel.PropertyChangedEventHandler(changedServices);
                        ((adWin.RibbonListButton)listBut).Items.Insert(((adWin.RibbonListButton)listBut).Items.Count - 3, toggleServicesPanel);
                        adWin.ComponentManager.UIElementActivated += new EventHandler<adWin.UIElementActivatedEventArgs>(ComponentManager_UIElementActivated);

                        return Result.Succeeded;
                     }
                  }
               }
            }
         }
         catch (Exception e)
         {
            //failed to add button, don't do a thing
         }
         return Result.Failed;
      }


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
      

      void Application_ViewActivated(object sender, ViewActivatedEventArgs args)
      {
         // If document exists it is already opened
         if (openedDocuments.ContainsKey(args.Document))
         {
            // Find or add Schema
            FindOrCreateSchema(args.Document);

            DockableResultPanel.ShowDocument(openedDocuments[args.Document]);
            DockableServicesPanel.ShowDocument(openedDocuments[args.Document]);
         }
      }


      public void Application_DocumentOpened(object sender, DocumentOpenedEventArgs args)
      {
         if (args.Document.IsLinked)
            return;

         try
         {
            // Find or add Schema
            FindOrCreateSchema(args.Document);

            openedDocuments.Add(args.Document, new BimbotDocument(args.Document));
            outputPane = uiApplication.GetDockablePane(OutputPaneId);
            servicePane = uiApplication.GetDockablePane(ServicePaneId);
            toggleResultPanel.IsChecked = true;
            toggleResultPanel.IsEnabled = true;
            toggleServicesPanel.IsChecked = true;
            toggleServicesPanel.IsEnabled = true;

            //Temp insert fixed service
//            openedDocuments[args.Document].AddService(new Service(6, "Limestone Brickalizer", "Generate brick distribution for limestone walls", "ifcanalysis", null, null, null, null, "http://ec2-18-218-56-112.us-east-2.compute.amazonaws.com/"));
//            openedDocuments[args.Document].registeredServices[0].soid = -1;

            DockableResultPanel.ShowDocument(openedDocuments[args.Document]);
            DockableServicesPanel.ShowDocument(openedDocuments[args.Document]);
         }
         catch (Exception e)
         {
            MessageBox.Show("Failed to open bimbot part in document due to:\n" + e.Message);
         }
      }


      public void Application_DocumentClosing(object sender, DocumentClosingEventArgs args)
      {
         // Remove the document from the list of opened documents (holding bimbot data)
         if (args.Document != null)
         {
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


      private void FindOrCreateSchema(Document doc)
      {
         // find schema
         Schema = Schema.Lookup(SchemaGuid);

         try
         {
            // create schema if not found
            if (Schema == null)
            {
               // Start transaction
               Transaction createSchema = new Transaction(doc, "Create Schema");
               createSchema.Start();

               // Build subSchema (services)
               SchemaBuilder serviceBldr = new SchemaBuilder(ServiceGuid);

               // Set read and write access attributes and a name
               serviceBldr.SetReadAccessLevel(AccessLevel.Public);
               serviceBldr.SetWriteAccessLevel(AccessLevel.Application);
               serviceBldr.SetApplicationGUID(ApplicationGuid);
               serviceBldr.SetVendorId(VendorId);
               serviceBldr.SetSchemaName("BimbotService");

               // create fields for relevant attributes of services
               FieldBuilder fieldBldr = serviceBldr.AddSimpleField("hostName", typeof(string));
               fieldBldr.SetDocumentation("Name of the host for this service.");
               fieldBldr = serviceBldr.AddSimpleField("hostUrl", typeof(string));
               fieldBldr.SetDocumentation("Url of the host for this service.");
               fieldBldr = serviceBldr.AddSimpleField("hostDesc", typeof(string));
               fieldBldr.SetDocumentation("Decription of the host for this service.");
               fieldBldr = serviceBldr.AddSimpleField("srvcName", typeof(string));
               fieldBldr.SetDocumentation("Name of the service.");
               fieldBldr = serviceBldr.AddSimpleField("srvcId", typeof(int));
               fieldBldr.SetDocumentation("Id of the service.");
               fieldBldr = serviceBldr.AddSimpleField("srvcDesc", typeof(string));
               fieldBldr.SetDocumentation("Decription of the service.");
               fieldBldr = serviceBldr.AddSimpleField("srvcProvider", typeof(string));
               fieldBldr.SetDocumentation("Provider of the service.");
               fieldBldr = serviceBldr.AddSimpleField("srvcProvIcon", typeof(string));
               fieldBldr.SetDocumentation("Provider Icon of the service.");
               fieldBldr = serviceBldr.AddArrayField("srvcInputs", typeof(string));
               fieldBldr.SetDocumentation("List of input types of the service.");
               fieldBldr = serviceBldr.AddArrayField("srvcOutputs", typeof(string));
               fieldBldr.SetDocumentation("List of output types the service.");
               fieldBldr = serviceBldr.AddSimpleField("srvcUrl", typeof(string));
               fieldBldr.SetDocumentation("Url of the service.");

               /* TODO create user depending service */
               fieldBldr = serviceBldr.AddSimpleField("srvcToken", typeof(string));
               fieldBldr.SetDocumentation("Token needed to run the service.");
               fieldBldr = serviceBldr.AddSimpleField("srvcSoid", typeof(int));
               fieldBldr.SetDocumentation("Soid needed to run the service.");
               fieldBldr = serviceBldr.AddSimpleField("srvcTrigger", typeof(int));
               fieldBldr.SetDocumentation("Trigger that causes the service to execute.");

               fieldBldr = serviceBldr.AddSimpleField("resultType", typeof(string));
               fieldBldr.SetDocumentation("Last result type delivered by the service.");
               fieldBldr = serviceBldr.AddSimpleField("resultData", typeof(string));
               fieldBldr.SetDocumentation("Last results delivered by the service.");
               fieldBldr = serviceBldr.AddSimpleField("resultDate", typeof(string));
               fieldBldr.SetDocumentation("Last date the service was executed.");
               ServiceSchema = serviceBldr.Finish(); // register the subSchema object

               //Build mainSchema (Bimbot Schema)
               SchemaBuilder schemaBldr = new SchemaBuilder(SchemaGuid);

               // Set read and write access attributes and a name
               schemaBldr.SetReadAccessLevel(AccessLevel.Public);
               schemaBldr.SetWriteAccessLevel(AccessLevel.Application);
               schemaBldr.SetApplicationGUID(ApplicationGuid);
               schemaBldr.SetVendorId(VendorId);
               schemaBldr.SetSchemaName("BimbotSchema");

               // create a field to store services
               fieldBldr = schemaBldr.AddArrayField("services", typeof(Entity));
               fieldBldr.SetDocumentation("List of services assigned to this project.");
               fieldBldr.SetSubSchemaGUID(ServiceGuid);

               Schema = schemaBldr.Finish(); // register the Schema object
               createSchema.Commit();
            }
            else
               ServiceSchema = Schema.Lookup(ServiceGuid);
         }
         catch (Exception e)
         {
            Console.WriteLine(e);
         }
      }
   }
}
