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

namespace Bimbot
{
   [Transaction(TransactionMode.Manual)]
   [Regeneration(RegenerationOption.Manual)]
   [Journaling(JournalingMode.NoCommandData)]

   public class ServiceRunAll : IExternalCommand
   {
      private CancellationTokenSource cts;
      private BackgroundWorker bgw;


      public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         try
         {
            // Create ifc-file for service
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string filename = IfcUtils.ExportProjectToIFC(commandData.Application.ActiveUIDocument.Document, path);
            byte[] data = File.ReadAllBytes(Path.Combine(path, filename));
            File.Delete(Path.Combine(path, filename));

            BackgroundWorker bgw = new BackgroundWorker();
            bgw.DoWork += new DoWorkEventHandler(DoWork);
            bgw.RunWorkerCompleted += (_, __) =>
            {
               MessageBox.Show("Finished all services!");
            };
            bgw.RunWorkerAsync(new Tuple<Document, byte[]>(commandData.Application.ActiveUIDocument.Document, data));
         }
         catch (OperationCanceledException e)
         {
            MessageBox.Show("Not finished services have been cancelled.");
            return Result.Cancelled;
         }
         catch (Exception e)
         {
            MessageBox.Show("A service has failed.");
            return Result.Failed;
         }
//         RevitBimbot.ActivateButtons(commandData.Application.ActiveUIDocument.Document);
         return Result.Succeeded;
      }
      /*
      public Task ExecuteAsync(Document doc, byte[] data)
      {
         cts = new CancellationTokenSource();

      }
      */
      public void DoWork(object sender, DoWorkEventArgs e)
      {
         Tuple<Document, byte[]> args = (Tuple<Document, byte[]>)e.Argument;
         cts = new CancellationTokenSource();

         RunServicesAsync(args.Item1, args.Item2, cts.Token).Wait();
      }


      private async Task RunServicesAsync(Document doc, byte[] data, CancellationToken ct)
      {
         Dictionary<Task<String>, Service> taskToService = new Dictionary<Task<String>, Service>();
         List<Task<String>> tasks = new List<Task<String>>();

         // Create list of tasks from services to run
         foreach (Service curService in RevitBimbot.services)
         {
            if (curService.Trigger == RevitEvntTrigger.manualButton)
            {
               Task<string> task = RunService(data, curService, ct);
               tasks.Add(task);
               taskToService.Add(task, curService);
            }
         }
         MessageBox.Show("Services Started!");

         // Excute tasks (services) and handle the first to finish
         while (tasks.Count > 0)
         {
            Task<String> firstFinishedTask = await Task.WhenAny(tasks);

            tasks.Remove(firstFinishedTask);
            String res = await firstFinishedTask;

            MessageBox.Show("Finished task '" + taskToService[firstFinishedTask].Name + "' with response: \n" + res.Substring(0, 300));
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
