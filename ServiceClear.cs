using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;
using Bimbot.BimbotUI;
using Bimbot.Objects;
// ReSharper disable UnusedMember.Global

namespace Bimbot
{
   [Transaction(TransactionMode.Manual)]
   [Regeneration(RegenerationOption.Manual)]
   [Journaling(JournalingMode.NoCommandData)]

   public class ServiceClear : IExternalCommand
   {
      public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         try
         {
            Schema schema = Schema.Lookup(new Guid("a54b4c89-0ee7-4fae-8fbd-94443f5020b3"));
            if (schema != null)
               Schema.EraseSchemaAndAllEntities(schema, true);

            schema = Schema.Lookup(new Guid("a54b4c89-0ee7-4fae-8fbd-94443f5020b4"));
            if (schema != null)
               Schema.EraseSchemaAndAllEntities(schema, true);
         }
         catch (Exception e)
         {
            Console.WriteLine(e);  
         }
         // autosucceed for now
         return Result.Succeeded;
      }
   }
}
