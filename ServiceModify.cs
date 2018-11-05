using System;
using System.Windows.Forms;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Bimbot.BimbotUI;
// ReSharper disable UnusedMember.Global

namespace Bimbot
{
   [Transaction(TransactionMode.Manual)]
   [Regeneration(RegenerationOption.Manual)]
   [Journaling(JournalingMode.NoCommandData)]

   public class ServiceModify : IExternalCommand
   {
      public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         UIDocument uidoc = commandData.Application.ActiveUIDocument;
         if (uidoc.Document.PathName.EndsWith(".rfa", StringComparison.InvariantCultureIgnoreCase))
         {
            throw new Exception("Revit Families are not supported for Bimbot services");
         }

         try
         {
            ServiceModifyForm form = new ServiceModifyForm(commandData.Application.ActiveUIDocument.Document);
            form.ShowDialog();
//            RevitBimbot.ActivateButtons(commandData.Application.ActiveUIDocument.Document);
         }
         catch (Exception ex)
         {
            string mssg = ex.Message;
            MessageBox.Show(mssg, @"Exception in BIMserver Export");
         }

         // autosucceed for now
         return Result.Succeeded;
      }
   }
}
