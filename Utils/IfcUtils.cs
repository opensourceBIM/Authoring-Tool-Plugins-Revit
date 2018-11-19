using Autodesk.Revit.DB;
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
