using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;
using Bimbot.Bcf;
using Bimbot.Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bimbot
{
   public class BimbotDocument: INotifyPropertyChanged
   {
      #region Static members and functions
      private static Schema RvtSchema;
      private static readonly Guid RvtGuid = new Guid("a54b4c89-0ee7-4fae-8fbd-b13b04f00a01"); //b13b04 = bimbot, f00 = bimbot data, a01 = version 0.1
      private const String ProvidersField = "Providers";
      private const String ServicesField = "Services";

      private static Guid FindOrCreateSchema(Document doc)
      {
         // find schema
         RvtSchema = Schema.Lookup(RvtGuid);

         try
         {
            // Find or read subschemas needed
            Guid provGuid = Provider.FindOrCreateSchema(doc);
            Guid servGuid = Service.FindOrCreateSchema(doc);
            
            // create schema if not found
            if (RvtSchema == null)
            {
               // Start transaction
               Transaction createSchema = new Transaction(doc, "Create Bimbot Schema");
               createSchema.Start();

               // Build schema
               SchemaBuilder schemaBldr = new SchemaBuilder(RvtGuid);

               // Set read and write access attributes and a name
               schemaBldr.SetReadAccessLevel(AccessLevel.Application);
               schemaBldr.SetWriteAccessLevel(AccessLevel.Application);
               schemaBldr.SetApplicationGUID(RevitBimbot.ApplicationGuid);
               schemaBldr.SetVendorId(RevitBimbot.VendorId);
               schemaBldr.SetSchemaName("BimBotDocument");

               // create a field to store services
               FieldBuilder fieldBldr = schemaBldr.AddMapField(ProvidersField, typeof(string), typeof(Entity));
               fieldBldr.SetDocumentation("Dictionary of providers by url of service(s) in this project.");
               fieldBldr.SetSubSchemaGUID(provGuid);

               fieldBldr = schemaBldr.AddArrayField(ServicesField, typeof(Entity));
               fieldBldr.SetDocumentation("List of services assigned to this project.");
               fieldBldr.SetSubSchemaGUID(servGuid);

               RvtSchema = schemaBldr.Finish(); // register the Schema object
               createSchema.Commit();
            }
            else
            {
               RvtSchema = Schema.Lookup(RvtGuid);
            }
         }
         catch (Exception e)
         {
            Console.WriteLine(e);
         }
         return RvtGuid;
      }
      #endregion



      #region INotifyPropertyChanged 
      public event PropertyChangedEventHandler PropertyChanged;

      // Create the OnPropertyChanged method to raise the event
      protected void OnPropertyChanged(string name)
      {
         PropertyChangedEventHandler handler = this.PropertyChanged;
         if (handler != null)
         {
            handler(this, new PropertyChangedEventArgs(name));
         }
      }
      #endregion


      private Document RvtDocument { get; }
      public string Name { get; }

      private Entity RvtEntity { get; set; }
      public  bool IsUpToDate;


      public Dictionary<string, Provider> RegisteredProviders { get; }
      public ObservableCollection<Service> AssignedServices { get; set; }
      public ObservableCollection<Service> AvailableServices { get; set; }
      public ObservableCollection<ResultItem> ResultItems { get; set; }
      

      private List<Task> Tasks { get; }
      private CancellationTokenSource cancellationToken;

      public BimbotDocument(Document document)
      {
         RvtDocument = document;
         Name = RvtDocument.PathName.Substring(RvtDocument.PathName.LastIndexOf('\\') + 1);

         RegisteredProviders = new Dictionary<string, Provider>();
         AssignedServices = new ObservableCollection<Service>();
         ResultItems = new ObservableCollection<ResultItem>();

         // Find or add Schema
         FindOrCreateSchema(RvtDocument);

         // Read the Bimbot data in the project
         RvtEntity = RvtDocument.ProjectInformation.GetEntity(RvtSchema);
         if (RvtEntity.IsValid())
         {
            IDictionary<string, Entity> providers = RvtEntity.Get<IDictionary<string, Entity>>(ProvidersField);
            foreach (KeyValuePair<string, Entity> pair in providers)
            {
               Provider readProvider = new Provider(pair.Value);
               RegisteredProviders.Add(pair.Key, readProvider);
            }
            IList<Entity> services = RvtEntity.Get<IList<Entity>>(ServicesField);
            foreach (Entity serviceEnt in services)
            {
               Service readService = new Service(serviceEnt, RegisteredProviders);
               readService.InitResults(ResultItems);
               AssignedServices.Add(readService);
            }
         }
         else
            RvtEntity = null;
         
         IsUpToDate = true;
      }


      public void AddService(Service service)
      {
         try
         {
            // Add the provider and service in memory
            AssignedServices.Add(service);
            service.InitResults(ResultItems);
//            service.AssignOrCreateProvider(RegisteredProviders); 
         }
         catch (Exception e)
         {
            Console.WriteLine(e.Message);
         }
      }


      public void DelService(Service service)
      {
//         if (runningServices.Contains(service))
         AssignedServices.Remove(service);
         service.CleanResults();
         //Provider is not removed since the registration remains valid even without services using it
      }


      public void WriteToRevit()
      {
         try
         {
            // Add the provider and service to Revit document
            Transaction transaction = new Transaction(RvtDocument, "Write BimbotDocument to revit");
            transaction.Start();
            
            // Create the bimbot entity in the document if not exists
            if (RvtEntity == null)
               RvtEntity = new Entity(RvtSchema);

            Dictionary<string, Entity> ProvidersMap = new Dictionary<string, Entity>();
            foreach (KeyValuePair<string, Provider> pair in RegisteredProviders)
               ProvidersMap.Add(pair.Key, pair.Value.GetUpdatedRevitEntity());
            RvtEntity.Set<IDictionary<string, Entity>>(ProvidersField, ProvidersMap); // set the value for this entity

            List<Entity> ServicesEntity = new List<Entity>();
            foreach (Service service in AssignedServices)
               ServicesEntity.Add(service.GetUpdatedRevitEntity());
            RvtEntity.Set<IList<Entity>>(ServicesField, ServicesEntity);

            RvtDocument.ProjectInformation.SetEntity(RvtEntity); // store the entity in the element
            transaction.Commit();
         }
         catch (Exception e)
         {
            Console.WriteLine(e.Message);
         }
      }


      public void Close()
      {
         // Set cancellationToken to cancel
         if (cancellationToken != null)
            cancellationToken.Cancel();

         if (AssignedServices != null && AssignedServices.Count > 0)
            AssignedServices.Clear();

         if (Tasks != null && Tasks.Count > 0)
            Tasks.Clear();

         // Write any (not stored) data?
      }
   }
}
