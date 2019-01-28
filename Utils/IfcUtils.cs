using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.ApplicationServices; 
using Autodesk.Revit.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bimbot.Utils
{
   static class IfcUtils
   {
      public static string ExportProjectToIFC(Document doc)
      {
         IFCExportOptions ifcOptions = new IFCExportOptions();

/*
         //Get an instance of IFCExportConfiguration
         IFCExportConfiguration selectedConfig = modelSelection.Configuration;

         //Get the current view Id, or -1 if you want to export the entire model
         ElementId activeViewId = GenerateActiveViewIdFromDocument(doc);
         selectedConfig.ActiveViewId =
                 selectedConfig.UseActiveViewGeometry ? activeViewId.IntegerValue : -1;

         //Update the IFCExportOptions
         selectedConfig.UpdateOptions(IFCOptions, activeViewId);
*/
         {
            ifcOptions.FileVersion = IFCVersion.IFC2x3;
            ifcOptions.WallAndColumnSplitting = false;
            ifcOptions.SpaceBoundaryLevel = 1;
            ifcOptions.ExportBaseQuantities = false;

            ifcOptions.AddOption("ExportInternalRevitPropertySets", "false");
            ifcOptions.AddOption("ExportIFCCommonPropertySets", "true");
            ifcOptions.AddOption("ExportAnnotations", "false");
            ifcOptions.AddOption("Use2DRoomBoundaryForVolume", "false");
            ifcOptions.AddOption("UseFamilyAndTypeNameForReference", "false");
            ifcOptions.AddOption("ExportVisibleElementsInView", "false");
            ifcOptions.AddOption("ExportPartsAsBuildingElements", "false");
            ifcOptions.AddOption("UseActiveViewGeometry", "false");
            ifcOptions.AddOption("ExportSpecificSchedules", "false");
            ifcOptions.AddOption("ExportBoundingBox", "false");
            ifcOptions.AddOption("ExportSolidModelRep", "false");
            ifcOptions.AddOption("ExportSchedulesAsPsets", "false");
            ifcOptions.AddOption("ExportUserDefinedPsets", "false");
            ifcOptions.AddOption("ExportUserDefinedParameterMapping", "false");
            ifcOptions.AddOption("ExportLinkedFiles", "false");
            ifcOptions.AddOption("IncludeSiteElevation", "false");
            ifcOptions.AddOption("TessellationLevelOfDetail", "0.5");
            ifcOptions.AddOption("StoreIFCGUID", "true");
         }
         /*            // get the revit form and set its cursor to busy
            System.Windows.Forms.Control form = System.Windows.Forms.Control.FromHandle(Process.GetCurrentProcess().MainWindowHandle);
            if (null != form) form.Cursor = Cursors.WaitCursor;

            //RevitStatusText.Set("Exporting Revit Project to IFC");
            Application.DoEvents();
         */

         Transaction trans = new Transaction(doc, "export model");
         trans.Start();

         string folder = Path.GetTempPath();
         string name = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Path.GetFileNameWithoutExtension(doc.PathName) + ".ifc";

         // revit doesn't allow the export to run in a different thread
         doc.Export(folder, name, ifcOptions);

         trans.Commit();
         return Path.Combine(folder, name);
      }
   }
}
