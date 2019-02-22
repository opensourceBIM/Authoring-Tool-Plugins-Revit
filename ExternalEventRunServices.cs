extern alias IFCExportUIOverride;

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
using IFCExportUIOverride::BIM.IFC.Export.UI;
// ReSharper disable UnusedMember.Global

namespace Bimbot.ExternalEvents
{

   public class ExtEvntRunServices : IExternalEventHandler
   {
      public List<Service> services = new List<Service>();
      public ExternalEventsContainer extEvents;

      private CancellationTokenSource cts;
      private BackgroundWorker bgw;
      private Document doc;
      

      public void Execute(UIApplication app)
      {
         doc = app.ActiveUIDocument.Document;
         try
         {
            Dictionary<string, byte[]> ifcData = new Dictionary<string, byte[]>();
            foreach (Service service in services)
            {
               // Skip creation of ifcData generation when this already exists
               // (checked by ifcexportconfiguration name of the service)
               // 
               if (ifcData.ContainsKey(service.IfcExportConfiguration))
                  continue;

               // Export to IFC according to all used Ifc export configurations
               IFCExportConfigurationsMapCustom configurationsMap = new IFCExportConfigurationsMapCustom();
               foreach (IFCExportConfigurationCustom config in configurationsMap.Values)
               {
                  if (config.Name.Equals(service.IfcExportConfiguration))
                  {
                     IFCExportOptions IFCOptions = new IFCExportOptions();
                     
                     //Get the current view Id, or -1 if you want to export the entire model
                     config.ActiveViewId = -1;

                     //Update the IFCExportOptions
                     config.UpdateOptions(IFCOptions, null);

                     string filename = IfcUtils.ExportProjectToIFC(doc, IFCOptions);
                     ifcData.Add(config.Name, File.ReadAllBytes(filename));
                     File.Delete(filename);
                  }
               }
            }

            BackgroundWorker bgw = new BackgroundWorker();
            bgw.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
            bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_Completed);
            bgw.RunWorkerAsync(new Tuple<Document, Dictionary<string, byte[]>>(doc, ifcData));
         }
         catch (Exception e)
         {
            MessageBox.Show("Failed to create IFC data to send to services.");
         }
      }


      private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
      {
         Tuple<Document, Dictionary<string,byte[]>> args = (Tuple<Document, Dictionary<string, byte[]>>)e.Argument;
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
            Bimbot.RevitBimbot.UpdateDocument(doc);
            MessageBox.Show("Finished all services!");
         }
      }

      private async Task RunServicesAsync(Document doc, Dictionary<string,byte[]> data, CancellationToken ct)
      {
         Dictionary<Task<String>, Service> taskToService = new Dictionary<Task<String>, Service>();
         List<Task<String>> tasks = new List<Task<String>>();

         // Create list of tasks from services to run 
         foreach (Service curService in services)
         {
            // 
            if (!data.ContainsKey(curService.IfcExportConfiguration))
            {
               MessageBox.Show(curService.Name + " not executed due to unavailable \n" +
                               "ifc export configuration (" + curService.IfcExportConfiguration + ")");
               break;
            }

            Task<string> task = curService.RunAsync(data[curService.IfcExportConfiguration]);
            tasks.Add(task);
            taskToService.Add(task, curService);
         }

         // Excute tasks (services) and handle the first to finish
         while (tasks.Count > 0)
         {
            Task<String> firstFinishedTask = await Task.WhenAny(tasks);
            Service curService = taskToService[firstFinishedTask];

            tasks.Remove(firstFinishedTask);
            taskToService.Remove(firstFinishedTask);
//            MessageBox.Show("Finished task '" + curService.Name + "' with response: \n" + res.Substring(0, 300));
         }
      }


      private void RunServices(Document doc, byte[] data, CancellationToken ct)
      {
         Dictionary<Task<String>, Service> taskToService = new Dictionary<Task<String>, Service>();
         List<Task<String>> tasks = new List<Task<String>>();

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
