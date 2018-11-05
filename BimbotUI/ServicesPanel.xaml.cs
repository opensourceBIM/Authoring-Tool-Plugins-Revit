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
      public BimbotDocument         botDoc;
      #endregion

      public ServicesPanel(ExternalEvent exEvent, ExtEvntChangeDocument handler)
      {
         try
         {
            ExtEvent = exEvent;
            Handler = handler;
            InitializeComponent();
         }
         catch (Exception ex)
         {

         }
      }

      public void ShowDocument(BimbotDocument bimDoc)
      {
         botDoc = bimDoc;
         Handler.curDoc = bimDoc.revitDocument;

         servicesList.Items.Clear();
         foreach (Service serv in bimDoc.registeredServices)
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
         ServiceAddFormOld form = new ServiceAddFormOld(botDoc.revitDocument);
         if (form.ShowDialog() == DialogResult.OK)
         {
            if (botDoc != null)
               botDoc.AddService(form.curService);

//            Bimbot.RevitBimbot.services.Add(curService);
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

      private void RefreshView(object sender, RoutedEventArgs e)
      {

      }
      private void RegService(object sender, RoutedEventArgs e)
      {

      }
      private void ModService(object sender, RoutedEventArgs e)
      {

      }

      private void RunSingle(object sender, RoutedEventArgs e)
      {
         if (servicesList.SelectedItems.Count != 1)
            return;

         // Create ifc-file for service
         string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
         string filename = IfcUtils.ExportProjectToIFC(botDoc.revitDocument, path);
         byte[] data = File.ReadAllBytes(Path.Combine(path, filename));
         File.Delete(Path.Combine(path, filename));

         BackgroundWorker bgw = new BackgroundWorker();
         bgw.DoWork += new DoWorkEventHandler(DoWork);
         bgw.RunWorkerCompleted += (_, __) =>
         {
            System.Windows.MessageBox.Show("Finished selected service!");
         };
         bgw.RunWorkerAsync(new Tuple<Service, byte[]>((Service)servicesList.SelectedItem, data));
      }

      private void RunAll(object sender, RoutedEventArgs e)
      {
         try
         {
            // Create ifc-file for service
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string filename = IfcUtils.ExportProjectToIFC(botDoc.revitDocument, path);
            byte[] data = File.ReadAllBytes(Path.Combine(path, filename));
            File.Delete(Path.Combine(path, filename));

            BackgroundWorker bgw = new BackgroundWorker();
            bgw.DoWork += new DoWorkEventHandler(DoWork);
            bgw.RunWorkerCompleted += (_, __) =>
            {
               System.Windows.MessageBox.Show("Finished all services!");
            };
            bgw.RunWorkerAsync(data);
         }
         catch (OperationCanceledException ex)
         {
            System.Windows.MessageBox.Show("Not finished services have been cancelled.");
         }
         catch (Exception ex)
         {
            System.Windows.MessageBox.Show("A service has failed.");
         }
      }

      public void DoWork(object sender, DoWorkEventArgs e)
      {
         CancellationTokenSource cts = new CancellationTokenSource();

         RunServicesAsync((byte[])e.Argument, cts.Token).Wait();

      }

      public void DoWorkSingle(object sender, DoWorkEventArgs e)
      {
         CancellationTokenSource cts = new CancellationTokenSource();
         ((Tuple<Service, byte[]>)e.Argument).Item1.Run(((Tuple<Service, byte[]>)e.Argument).Item2).Wait();

         lock (Handler._IsFree)
         {
            Handler.partsToUpdate = ExtEvntChangeDocument.ServiceParts.result;
            Handler.serviceToUpdate = ((Tuple<Service, byte[]>)e.Argument).Item1;
            ExtEvent.Raise();
         }
      }


      private async Task RunServicesAsync(byte[] data, CancellationToken ct)
      {
         Dictionary<Task<String>, Service> taskToService = new Dictionary<Task<String>, Service>();
         List<Task<String>> tasks = new List<Task<String>>();

         // Create list of tasks from services to run
         foreach (Service curService in botDoc.registeredServices)
         {
            if (curService.Trigger == RevitEvntTrigger.manualButton)
            {
               Task<string> task = RunService(data, curService, ct);
               tasks.Add(task);
               taskToService.Add(task, curService);
            }
         }
         System.Windows.MessageBox.Show("Services Started!");

         // Excute tasks (services) and handle the first to finish
         while (tasks.Count > 0)
         {
            Task<String> firstFinishedTask = await Task.WhenAny(tasks);

            lock (Handler._IsFree)
            {
               Handler.partsToUpdate = ExtEvntChangeDocument.ServiceParts.result;
               Handler.serviceToUpdate = taskToService[firstFinishedTask];
               ExtEvent.Raise();
            }
            tasks.Remove(firstFinishedTask);
            taskToService.Remove(firstFinishedTask);

            String res = await firstFinishedTask;

            System.Windows.MessageBox.Show("Finished task '" + taskToService[firstFinishedTask].Name + "' with response: \n" + res.Substring(0, 300));
         }
      }


      async Task<string> RunService(byte[] data, Service curService, CancellationToken ct)
      {
         // GetAsync returns a Task<HttpResponseMessage>. 
         return await curService.Run(data);
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


   }
}
