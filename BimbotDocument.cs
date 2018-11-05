using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;
using Bimbot.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bimbot
{
   public class BimbotDocument
   {
      public Document      revitDocument;
      public List<Service> registeredServices;
      private List<Task>   runningServices;

      private CancellationTokenSource cancellationToken;

      public BimbotDocument(Document _Doc)
      {
         revitDocument = _Doc;

         registeredServices = new List<Service>();
         runningServices = new List<Task>();

         // Read the Bimbot data in the project
         if (_Doc.ProjectInformation.GetEntity(RevitBimbot.Schema).IsValid())
         {
            IList<Entity> serviceEntities = _Doc.ProjectInformation.GetEntity(RevitBimbot.Schema).Get<IList<Entity>>("services");
            foreach (Entity serviceEnt in serviceEntities)
            {
               AddService(new Service(serviceEnt));
            }
         }
      }


      public void AddService(Service service)
      {
         registeredServices.Add(service);
      }


      public void DelService(Service service)
      {
//         if (runningServices.Contains(service))


         registeredServices.Remove(service);
      }

      public void Close()
      {
         // Set cancellationToken to cancel
         cancellationToken.Cancel();

         registeredServices.Clear();
         runningServices.Clear();

         // Write any (not stored) data?
      }
   }
}
