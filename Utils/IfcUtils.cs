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
      public static string ExportProjectToIFC(Document doc, IFCExportOptions ifcOptions)
      {
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
