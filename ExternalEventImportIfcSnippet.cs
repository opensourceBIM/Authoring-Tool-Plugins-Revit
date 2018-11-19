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
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading;

namespace Bimbot.ExternalEvents
{
   /// <summary>
   /// Obfuscation Ignore for External Interface
   /// </summary>
   public class ExtEvntImportIfcSnippet : IExternalEventHandler
   {
      public String filePath;

      /// <summary>
      /// External Event Implementation
      /// </summary>
      /// <param name="app"></param>
      public void Execute(UIApplication app)
      {
         try
         {
            UIDocument uiDoc = app.ActiveUIDocument;
            Document doc = uiDoc.Document;
            Autodesk.Revit.DB.View curView = null;

            View3D usableView = null;
            //if the current view is 3d (future correction => valid for IFC import)
            if (doc.ActiveView.ViewType != ViewType.ThreeD)
            {
               //try to use an existing 3D view
               IEnumerable<View3D> viewcollector3D = get3DViews(doc);
               if (viewcollector3D.Any(o => o.Name == "{3D}" || o.Name == "BCFortho" || o.Name == "BCFpersp"))
                  usableView = viewcollector3D.First(o => o.Name == "{3D}" || o.Name == "BCFortho" || o.Name == "BCFpersp");
               
               // No valid view to use for importing
               if (usableView == null)
               {
                  System.Windows.MessageBox.Show("Bim snippet can't be imported since\n the project contains no valid view.");
                  return;
               }

               // set the view to a usableView for importing the IFC snippet
               uiDoc.ActiveView = usableView;
            }

            // Show the import screen
            app.PostCommand(RevitCommandId.LookupCommandId("ID_IFC_LINK"));

            //Run a background worker to insert the name of the file to import 
            BackgroundWorker bgw = new BackgroundWorker();
            bgw.DoWork += new DoWorkEventHandler(DoWork);
            bgw.RunWorkerCompleted += (_, __) =>
            {
               if (curView != null)
                  uiDoc.ActiveView = curView;
            };
            bgw.RunWorkerAsync();
         }
         catch (Exception e)
         {
            System.Windows.MessageBox.Show("A service has failed.");
         }
      }


      private IEnumerable<View3D> get3DViews(Document doc)
      {
         return from elem in new FilteredElementCollector(doc).OfClass(typeof(View3D))
                let view = elem as View3D
                select view;
      }

      public void DoWork(object sender, DoWorkEventArgs e)
      {
         Thread.Sleep(1000);
         SendKeys.SendWait(filePath + "~");
      }


      public string GetName()
      {
         return "Import BCF ifc snippet";
      }
   }
}