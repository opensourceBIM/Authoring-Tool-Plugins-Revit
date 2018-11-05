using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;
using Bimbot.Forms;
// ReSharper disable UnusedMember.Global

namespace Bimbot
{
   [Transaction(TransactionMode.Manual)]
   [Regeneration(RegenerationOption.Manual)]
   [Journaling(JournalingMode.NoCommandData)]

   public class ServiceResult : IExternalCommand
   {
//      public static readonly Guid mySchemaGuid = new Guid("a54b4c89-0ee7-4fae-8fbd-94443f5020b3");
//      public static readonly Guid myApplicationGuid = new Guid("a694f098-20d5-42c2-a8a0-ac7ae6857c24");
//      public static readonly string myVendorId = "nl.tno";

      
      public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {

         UIDocument uidoc = commandData.Application.ActiveUIDocument;

         // A new handler to handle request posting by the dialog  
//         var handler = new ExtEvntChangeRevitView();

         // External Event for the dialog to use (to post requests)  
//         var extEvent = ExternalEvent.Create(handler);

         if (uidoc.Document.PathName.EndsWith(".rfa", StringComparison.InvariantCultureIgnoreCase))
         {
            throw new Exception("Revit Families are not supported for Bimbot services");
         }

         try
         {
            ShowResultsForm form = new ShowResultsForm(commandData.Application.ActiveUIDocument/*, extEvent, handler*/);
            form.ShowDialog();
         }
         catch (Exception ex)
         {
            string mssg = ex.Message;
            MessageBox.Show(mssg, @"Exception in BIMserver Export");
         }

         // autosucceed for now
         return Result.Succeeded;

         /*
         try
         {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            // Find the Schema
            Schema mySchema = Schema.Lookup(mySchemaGuid);
            IList<Schema> anySchemas = Schema.ListSchemas();

            // Create Schema if not found
            if (mySchema == null)
            {
               Transaction createSchema = new Transaction(doc, "Create Schema");
               createSchema.Start();

               SchemaBuilder schemaBuilder = new SchemaBuilder(mySchemaGuid);

               schemaBuilder.SetReadAccessLevel(AccessLevel.Public);
               schemaBuilder.SetWriteAccessLevel(AccessLevel.Application);
               schemaBuilder.SetApplicationGUID(myApplicationGuid);
               schemaBuilder.SetVendorId(myVendorId);
               schemaBuilder.SetSchemaName("TestSchema");

               // create a field to store services
               FieldBuilder fieldBuilder = schemaBuilder.AddSimpleField("name", typeof(String));
               fieldBuilder.SetDocumentation("Just a name for testing");

               mySchema = schemaBuilder.Finish();
               createSchema.Commit();
            }

            // Find the DataStorage
            FilteredElementCollector collector = new FilteredElementCollector(doc).OfClass(typeof(DataStorage));

            DataStorage myDataStorage = null;
            foreach (DataStorage ds in collector)
            {
               if (mySchema != null)
               {
                  Entity ent = ds.GetEntity(mySchema);
                  if (ent.IsValid())
                  {
                     myDataStorage = ds;
                     break;
                  }
               }
            }

            // Create DataStorage and add mySchema data to it
            if (myDataStorage == null)
            {
               Transaction createDataStorage = new Transaction(doc, "Create DataStorage");
               createDataStorage.Start();

               // Create data storage in new document
               myDataStorage = DataStorage.Create(doc);

               // Create an entity matching mySchema and assing it to myDataStorage
               Entity entity = new Entity(mySchema);
               Field name = mySchema.GetField("name");
               entity.Set<string>(name, "Dit is een Test");

               if (myDataStorage != null)
                  myDataStorage.SetEntity(entity);

               doc.ProjectInformation.SetEntity(entity);

               createDataStorage.Commit();
            }



            // Find the Schema after adding
            Schema mySchemaAfterAdding = Schema.Lookup(mySchemaGuid);
            IList<Schema> anySchemasAfterAdding = Schema.ListSchemas();


            // Find the DataStorage after adding
            FilteredElementCollector collectorAfterAdding = new FilteredElementCollector(doc).OfClass(typeof(DataStorage));

            DataStorage myDataStorageAfterAdding = null;
            foreach (DataStorage ds in collectorAfterAdding)
            {
               if (mySchemaAfterAdding != null)
               {
                  Entity ent = ds.GetEntity(mySchemaAfterAdding);
                  if (ent.IsValid())
                  {
                     myDataStorageAfterAdding = ds;
                     break;
                  }
               }
            }
         }
         catch (Exception ex)
         {
            string mssg = ex.Message;
            MessageBox.Show(mssg, @"Exception occured");
         }

         // autosucceed for now
         return Result.Succeeded;
        */
      }
   }
}
