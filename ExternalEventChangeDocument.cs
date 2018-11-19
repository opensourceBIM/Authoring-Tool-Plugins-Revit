using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Bimbot.Bcf;
using Bimbot.Utils;
using Bimbot.Objects;
using Autodesk.Revit.DB.ExtensibleStorage;

namespace Bimbot.ExternalEvents
{
   /// <summary>
   /// Obfuscation Ignore for External Interface
   /// </summary>
   public class ExtEvntChangeDocument : IExternalEventHandler
   {
      public enum ServiceParts
      {
         service,
         token, 
         result
      };

      public Service      serviceToUpdate;
      public ServiceParts partsToUpdate;
      public Document curDoc;
      public readonly object _IsFree = new object();


      /// <summary>
      /// External Event Implementation
      /// </summary>
      /// <param name="app"></param>
      public void Execute(UIApplication app)
      {
         switch (partsToUpdate)
         {
            case ServiceParts.service:
               UpdateService();
               break;
            case ServiceParts.token:
               UpdateToken();
               break;
            case ServiceParts.result:
               UpdateResult();
               break;
         }
      }

      private void UpdateService()
      {
         // Add the configuration to the revit file as well (make triggering work on reopening the document)
         Transaction transaction = new Transaction(curDoc, "tAddEntity");
         transaction.Start();

         // create an entity (object) for this schema (class)
         Entity service = new Entity(RevitBimbot.ServiceSchema);

         Entity entity = curDoc.ProjectInformation.GetEntity(RevitBimbot.Schema);
         if (!entity.IsValid()) entity = new Entity(RevitBimbot.Schema);

         // get the field from the schema
         service.Set<string>("hostName", serviceToUpdate.host.name);
         service.Set<string>("hostUrl", serviceToUpdate.host.listUrl);
         service.Set<string>("hostDesc", serviceToUpdate.host.description);

         service.Set<string>("srvcName", serviceToUpdate.Name);
         service.Set<int>("srvcId", serviceToUpdate.Id);
         service.Set<string>("srvcDesc", serviceToUpdate.Description);
         service.Set<string>("srvcProvider", serviceToUpdate.Provider);
         service.Set<string>("srvcProvIcon", serviceToUpdate.ProviderIcon);
         service.Set<IList<string>>("srvcInputs", serviceToUpdate.Inputs);
         service.Set<IList<string>>("srvcOutputs", serviceToUpdate.Outputs);
         service.Set<string>("srvcUrl", serviceToUpdate.ResourceUrl);
         service.Set<int>("srvcSoid", serviceToUpdate.soid);
         service.Set<string>("srvcToken", serviceToUpdate.srvcToken);
         service.Set<int>("srvcTrigger", (int)serviceToUpdate.Trigger);

         //Get values (services) currently in the schema and add the new service
         IList<Entity> orgServices = entity.Get<IList<Entity>>("services");

         orgServices.Add(service);
         entity.Set<IList<Entity>>("services", orgServices); // set the value for this entity

         curDoc.ProjectInformation.SetEntity(entity); // store the entity in the element
         transaction.Commit();
      }

      private void UpdateToken()
      {

         // Add the configuration to the revit file as well (make triggering work on reopening the document)
         Transaction transaction = new Transaction(curDoc, "tUpdateToken");
         transaction.Start();

         Entity entity = curDoc.ProjectInformation.GetEntity(RevitBimbot.Schema);

         //Get values (services) currently in the schema and add the new service
         IList<Entity> orgServices = entity.Get<IList<Entity>>("services");
         foreach (Entity service in orgServices)
         {
            orgServices.Add(service);
            entity.Set<IList<Entity>>("services", orgServices); // set the value for this entity
         }
         curDoc.ProjectInformation.SetEntity(entity); // store the entity in the element
         transaction.Commit();
      }



      private void UpdateResult()
      {
         // Add the configuration to the revit file as well (make triggering work on reopening the document)
         Transaction transaction = new Transaction(curDoc, "tAddEntity");
         transaction.Start();

         // create an entity (object) for this schema (class)
         Entity service = new Entity(RevitBimbot.ServiceSchema);

         Entity entity = curDoc.ProjectInformation.GetEntity(RevitBimbot.Schema);
         if (!entity.IsValid()) entity = new Entity(RevitBimbot.Schema);

         // get the field from the schema
         service.Set<string>("hostName", serviceToUpdate.host.name);
         service.Set<string>("hostUrl", serviceToUpdate.host.listUrl);
         service.Set<string>("hostDesc", serviceToUpdate.host.description);

         service.Set<string>("srvcName", serviceToUpdate.Name);
         service.Set<int>("srvcId", serviceToUpdate.Id);
         service.Set<string>("srvcDesc", serviceToUpdate.Description);
         service.Set<string>("srvcProvider", serviceToUpdate.Provider);
         service.Set<string>("srvcProvIcon", serviceToUpdate.ProviderIcon);
         service.Set<IList<string>>("srvcInputs", serviceToUpdate.Inputs);
         service.Set<IList<string>>("srvcOutputs", serviceToUpdate.Outputs);
         service.Set<string>("srvcUrl", serviceToUpdate.ResourceUrl);
         service.Set<int>("srvcSoid", serviceToUpdate.soid);
         service.Set<string>("srvcToken", serviceToUpdate.srvcToken);
         service.Set<int>("srvcTrigger", (int)serviceToUpdate.Trigger);

         //Get values (services) currently in the schema and add the new service
         IList<Entity> orgServices = entity.Get<IList<Entity>>("services");

         orgServices.Add(service);
         entity.Set<IList<Entity>>("services", orgServices); // set the value for this entity

         curDoc.ProjectInformation.SetEntity(entity); // store the entity in the element
         transaction.Commit();
      }




      public string GetName()
      {
         return "Update/Create Service";
      }
   }
}