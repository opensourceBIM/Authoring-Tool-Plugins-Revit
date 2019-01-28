using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;
using Bimbot.BimbotUI;
using Bimbot.Objects;
using Bimbot.Utils;
// ReSharper disable UnusedMember.Global

namespace Bimbot.ExternalEvents
{

   public class ExtEvntRunServices : IExternalEventHandler
   {
      public List<Service> services = new List<Service>();
      private CancellationTokenSource cts;
      private BackgroundWorker bgw;
      

      public void Execute(UIApplication app)
      {
         Document doc = app.ActiveUIDocument.Document;
         try
         {
            // Create ifc-file for service
            string filename = IfcUtils.ExportProjectToIFC(doc);
            byte[] data = File.ReadAllBytes(filename);
            File.Delete(filename);

            BackgroundWorker bgw = new BackgroundWorker();
            bgw.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
            bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_Completed);
            bgw.RunWorkerAsync(new Tuple<Document, byte[]>(doc, data));
         }
         catch (Exception e)
         {
            MessageBox.Show("Failed to create IFC data to send to services.");
         }
      }


      private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
      {
         Tuple<Document, byte[]> args = (Tuple<Document, byte[]>)e.Argument;
         cts = new CancellationTokenSource();

         //RunServices(args.Item1, args.Item2, cts.Token);
         
         Task task = RunServicesAsync(args.Item1, args.Item2, cts.Token);
         task.Wait();
      }


      private void bgWorker_Completed(object sender, RunWorkerCompletedEventArgs e)
      {
         if (e.Error != null)
         {
            MessageBox.Show("A service has failed!");
         }
         else
         {
//            Bimbot.RevitBimbot.UpdateResultPanel();
            MessageBox.Show("Finished all services!");
         }
      }

      private async Task RunServicesAsync(Document doc, byte[] data, CancellationToken ct)
      {
         Dictionary<Task<String>, Service> taskToService = new Dictionary<Task<String>, Service>();
         List<Task<String>> tasks = new List<Task<String>>();

         // Create list of tasks from services to run 
         foreach (Service curService in services)
         {
            Task<string> task = curService.RunAsync(data);
            tasks.Add(task);
            taskToService.Add(task, curService);
         }
//         MessageBox.Show("External services Started!");

         // Excute tasks (services) and handle the first to finish
         while (tasks.Count > 0)
         {
            Task<String> firstFinishedTask = await Task.WhenAny(tasks);
            Service curService = taskToService[firstFinishedTask];
            tasks.Remove(firstFinishedTask);
            taskToService.Remove(firstFinishedTask);
            //String res = await firstFinishedTask;
//            MessageBox.Show("Finished task '" + curService.Name + "' with response: \n" + res.Substring(0, 300));
         }
      }


      private void RunServices(Document doc, byte[] data, CancellationToken ct)
      {
         Dictionary<Task<String>, Service> taskToService = new Dictionary<Task<String>, Service>();
         List<Task<String>> tasks = new List<Task<String>>();

//         MessageBox.Show("External services Started, one by one!");

         // Create list of tasks from services to run
         foreach (Service curService in services)
         {
            string res = curService.Run(data);
//            MessageBox.Show("Finished task '" + curService.Name + "' with response: \n" + res.Substring(0, 300));
         }
      }

      public string GetName()
      {
         return "Run Service";
      }
   }
}
