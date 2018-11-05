using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Bimbot.BcfStructures;
using Bimbot.Utils;
using View = Autodesk.Revit.DB.View;

namespace Bimbot.Forms
{
   public partial class ShowResultsForm : System.Windows.Forms.Form
   {

      private UIDocument uiDoc;
//      private ExternalEvent ExtEvent;
//      private ExtEvntChangeRevitView Handler;


      public ShowResultsForm(UIDocument doc/*, ExternalEvent exEvent, ExtEvntChangeRevitView handler*/)
      {
         InitializeComponent();
         uiDoc = doc;
//         ExtEvent = exEvent;
//         Handler = handler;
      }


      private void ServiceSetupForm_Load(object sender, EventArgs e)
      {
         // Fill active services
         resultList.Items.Clear();
         Entity ent = uiDoc.Document.ProjectInformation.GetEntity(RevitBimbot.Schema);
         if (ent != null)
         {
            IList<Entity> services = ent.Get<IList<Entity>>("services");
            foreach (Entity service in services)
            {
               if (service.Get<String>("resultType").Equals("bcf"))
               {
                  BcfFile bcf = new BcfFile(Convert.FromBase64String(service.Get<String>("resultData")));
                  if (bcf != null)
                  {
                     //Set markups to interface
                     foreach (KeyValuePair<string, Markup> entry in bcf.markups)
                     {
                        ListViewItem item = resultList.Items.Add(entry.Value.Topic.Title);
                        item.SubItems.Add(service.Get<String>("srvcName"));
                        item.SubItems.Add(service.Get<String>("resultDate"));
                        item.SubItems.Add(service.Get<String>("resultType"));
                        item.Tag = entry.Value;
                     }
                  }
               }
               else
               {
                  ListViewItem item = resultList.Items.Add(service.Get<String>("srvcName"));
                  item.SubItems.Add(service.Get<String>("srvcName"));
                  item.SubItems.Add(service.Get<String>("resultDate"));
                  item.SubItems.Add(service.Get<String>("resultType"));
                  item.Tag = service.Get<String>("resultData");
               }
               //resultList.Columns[1].Width = 0;
               //resultList.Columns[2].Width = 0;
               //resultList.Columns[3].Width = 0;
               //resultList.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            }
         }
      }

      private void resultList_SelectedIndexChanged(object sender, EventArgs e)
      {
         topicItems.Items.Clear();
         commentList.Items.Clear();
         viewpointList.Items.Clear();
         viewpointImage.Image = null;

         if (resultList.SelectedItems.Count == 1)
         {
            serviceName.Text = resultList.SelectedItems[0].SubItems[1].Text;
            serviceLastRun.Text = resultList.SelectedItems[0].SubItems[2].Text;

            if (resultList.SelectedItems[0].SubItems[3].Text.Equals("bcf"))
            {
               Markup curMarkup = (Markup)resultList.SelectedItems[0].Tag;

               topicItems.Items.Add("Title").SubItems.Add(curMarkup.Topic.Title);
               topicItems.Items.Add("CreationDate").SubItems.Add(curMarkup.Topic.CreationDate.ToString());
               topicItems.Items.Add("CreationAuthor").SubItems.Add(curMarkup.Topic.CreationAuthor);
               topicItems.Items.Add("ModifiedDate").SubItems.Add(curMarkup.Topic.ModifiedDate.ToString());
               topicItems.Items.Add("ModifiedAuthor").SubItems.Add(curMarkup.Topic.ModifiedAuthor);
               topicItems.Items.Add("Priority").SubItems.Add(curMarkup.Topic.Priority);
               topicItems.Items.Add("DueDate").SubItems.Add(curMarkup.Topic.DueDate.ToString());
               topicItems.Items.Add("AssignedTo").SubItems.Add(curMarkup.Topic.AssignedTo);
               topicItems.Items.Add("Stage").SubItems.Add(curMarkup.Topic.Stage);
               topicItems.Items.Add("Guid").SubItems.Add(curMarkup.Topic.Guid);
               topicItems.Items.Add("TopicType").SubItems.Add(curMarkup.Topic.TopicType);
               topicItems.Items.Add("TopicStatus").SubItems.Add(curMarkup.Topic.TopicStatus);
               topicItems.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

               topicTitle.Text = curMarkup.Topic.Title;
               topicDescription.Text = curMarkup.Topic.Description;

               if (curMarkup.Comment != null)
               {
                  foreach (Comment com in curMarkup.Comment)
                     commentList.Items.Add(com.Date.ToString()).Tag = com;
                  commentList.Items[0].Selected = true;
               }

               if (curMarkup.Viewpoints != null)
               {
                  foreach (ViewPoint vp in curMarkup.Viewpoints)
                     viewpointList.Items.Add(vp.Viewpoint).Tag = vp;
                  viewpointList.Items[0].Selected = true;
               }

            }
            else
            {
               topicDescription.Text = (string)resultList.SelectedItems[0].Tag;
            }
         }
         else
         {
            serviceName.Text = "";
            serviceLastRun.Text = "";
         }
      }

      private void commentList_SelectedIndexChanged(object sender, EventArgs e)
      {
         commentItems.Items.Clear();
         if (commentList.SelectedItems.Count == 1)
         {
            Comment curComment = (Comment)commentList.SelectedItems[0].Tag;
            commentItems.Items.Add("Comment").SubItems.Add(curComment.Comment1);
            commentItems.Items.Add("Date").SubItems.Add(curComment.Date.ToString());
            commentItems.Items.Add("Author").SubItems.Add(curComment.Author);
            if (curComment.Viewpoint != null)
               commentItems.Items.Add("Viewpoint").SubItems.Add(curComment.Viewpoint.Guid);
            commentItems.Items.Add("ModifiedDate").SubItems.Add(curComment.ModifiedDate.ToString());
            commentItems.Items.Add("ModifiedAuthor").SubItems.Add(curComment.ModifiedAuthor);
            commentItems.Items.Add("Guid").SubItems.Add(curComment.Guid);
            commentItems.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
         }
      }

      private void viewpoints_SelectedIndexChanged(object sender, EventArgs e)
      {
         viewpointItems.Items.Clear();

         if (viewpointList.SelectedItems.Count == 1)
         {
            ViewPoint curVp = (ViewPoint)viewpointList.SelectedItems[0].Tag;
            viewpointItems.Items.Add("Index").SubItems.Add(curVp.Index.ToString());
            if (curVp.ViewpointRef.OrthogonalCamera != null)
            {
               viewpointItems.Items.Add("OrthogonalCamera");
               viewpointItems.Items.Add("  Direction");
               viewpointItems.Items.Add("    X").SubItems.Add(curVp.ViewpointRef.OrthogonalCamera.CameraDirection.X.ToString());
               viewpointItems.Items.Add("    Y").SubItems.Add(curVp.ViewpointRef.OrthogonalCamera.CameraDirection.Y.ToString());
               viewpointItems.Items.Add("    Z").SubItems.Add(curVp.ViewpointRef.OrthogonalCamera.CameraDirection.Z.ToString());
               viewpointItems.Items.Add("  Viewpoint");
               viewpointItems.Items.Add("    X").SubItems.Add(curVp.ViewpointRef.OrthogonalCamera.CameraViewPoint.X.ToString());
               viewpointItems.Items.Add("    Y").SubItems.Add(curVp.ViewpointRef.OrthogonalCamera.CameraViewPoint.Y.ToString());
               viewpointItems.Items.Add("    Z").SubItems.Add(curVp.ViewpointRef.OrthogonalCamera.CameraViewPoint.Z.ToString());
               viewpointItems.Items.Add("  UpVector");
               viewpointItems.Items.Add("    X").SubItems.Add(curVp.ViewpointRef.OrthogonalCamera.CameraUpVector.X.ToString());
               viewpointItems.Items.Add("    Y").SubItems.Add(curVp.ViewpointRef.OrthogonalCamera.CameraUpVector.Y.ToString());
               viewpointItems.Items.Add("    Z").SubItems.Add(curVp.ViewpointRef.OrthogonalCamera.CameraUpVector.Z.ToString());
               viewpointItems.Items.Add("  Scale").SubItems.Add(curVp.ViewpointRef.OrthogonalCamera.ViewToWorldScale.ToString());
            }

            if (curVp.ViewpointRef.PerspectiveCamera != null)
            {
               viewpointItems.Items.Add("PerspectiveCamera");
               viewpointItems.Items.Add("  Direction");
               viewpointItems.Items.Add("    X").SubItems.Add(curVp.ViewpointRef.PerspectiveCamera.CameraDirection.X.ToString());
               viewpointItems.Items.Add("    Y").SubItems.Add(curVp.ViewpointRef.PerspectiveCamera.CameraDirection.Y.ToString());
               viewpointItems.Items.Add("    Z").SubItems.Add(curVp.ViewpointRef.PerspectiveCamera.CameraDirection.Z.ToString());
               viewpointItems.Items.Add("  Viewpoint");
               viewpointItems.Items.Add("    X").SubItems.Add(curVp.ViewpointRef.PerspectiveCamera.CameraViewPoint.X.ToString());
               viewpointItems.Items.Add("    Y").SubItems.Add(curVp.ViewpointRef.PerspectiveCamera.CameraViewPoint.Y.ToString());
               viewpointItems.Items.Add("    Z").SubItems.Add(curVp.ViewpointRef.PerspectiveCamera.CameraViewPoint.Z.ToString());
               viewpointItems.Items.Add("  UpVector");
               viewpointItems.Items.Add("    X").SubItems.Add(curVp.ViewpointRef.PerspectiveCamera.CameraUpVector.X.ToString());
               viewpointItems.Items.Add("    Y").SubItems.Add(curVp.ViewpointRef.PerspectiveCamera.CameraUpVector.Y.ToString());
               viewpointItems.Items.Add("    Z").SubItems.Add(curVp.ViewpointRef.PerspectiveCamera.CameraUpVector.Z.ToString());
               viewpointItems.Items.Add("  Scale").SubItems.Add(curVp.ViewpointRef.PerspectiveCamera.FieldOfView.ToString());
            }
            viewpointItems.Items.Add("Guid").SubItems.Add(curVp.Guid);

            viewpointItems.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            viewpointImage.Image = curVp.SnapshotRef;

            ModifyRevitView(curVp);
         }
      }

      private void ModifyRevitView(ViewPoint view)
      {
/*         try
         {
            if (uiDoc.ActiveView.ViewType == ViewType.Schedule)
            {
               MessageBox.Show("RevitBimbot can't take snapshots of schedules.");
               return;
            }

            if (uiDoc.ActiveView.ViewType == ViewType.ThreeD)
            {
               var view3D = (View3D)uiDoc.ActiveView;
               if (view3D.IsPerspective)
               {
                  MessageBox.Show("This operation is not allowed in a Perspective View.\nPlease close the current window(s) and retry.");
                  return;
               }

            }
            Handler.v = view.ViewpointRef;
            ExtEvent.Raise();
         }
         catch (System.Exception ex1)
         {
            TaskDialog.Show("Error opening a View!", "exception: " + ex1);
         }
*/
         var v = view.ViewpointRef;
         try
         {
            UIDocument uidoc = uiDoc;
            Document doc = uidoc.Document;

            // IS ORTHOGONAL
            if (v.OrthogonalCamera != null)
            {
               if (v.OrthogonalCamera.CameraViewPoint == null || v.OrthogonalCamera.CameraUpVector == null || v.OrthogonalCamera.CameraDirection == null)
                  return;
               //type = "OrthogonalCamera";
               var zoom = v.OrthogonalCamera.ViewToWorldScale.ToFeet();
               var cameraDirection = RevitUtils.GetRevitXYZ(v.OrthogonalCamera.CameraDirection);
               var cameraUpVector = RevitUtils.GetRevitXYZ(v.OrthogonalCamera.CameraUpVector);
               var cameraViewPoint = RevitUtils.GetRevitXYZ(v.OrthogonalCamera.CameraViewPoint);
               var orient3D = RevitUtils.ConvertBasePoint(doc, cameraViewPoint, cameraDirection, cameraUpVector, true);

               View3D orthoView = null;
               //if active view is 3d ortho use it
               if (doc.ActiveView.ViewType == ViewType.ThreeD)
               {
                  var activeView3D = doc.ActiveView as View3D;
                  if (!activeView3D.IsPerspective)
                     orthoView = activeView3D;
               }
               if (orthoView == null)
               {
                  //try to use an existing 3D view
                  IEnumerable<View3D> viewcollector3D = get3DViews(doc);
                  if (viewcollector3D.Any(o => o.Name == "{3D}" || o.Name == "BCFortho"))
                     orthoView = viewcollector3D.First(o => o.Name == "{3D}" || o.Name == "BCFortho");
               }
               using (var trans = new Transaction(uidoc.Document))
               {
                  if (trans.Start("Open orthogonal view") == TransactionStatus.Started)
                  {
                     //create a new 3d ortho view 

                     if (orthoView == null)
                     {
                        orthoView = View3D.CreateIsometric(doc, getFamilyViews(doc).First().Id);
                        orthoView.Name = "BCFortho";
                     }
                     else
                     {
                        //reusing an existing view, I net to reset the visibility
                        //placed this here because if set afterwards it doesn't work
                        orthoView.DisableTemporaryViewMode(TemporaryViewMode.TemporaryHideIsolate);
                     }
                     orthoView.SetOrientation(orient3D);
                     trans.Commit();
                  }
               }
               uidoc.ActiveView = orthoView;
               //adjust view rectangle

               // **** CUSTOM VALUE FOR TEKLA **** //
               // double x = zoom / 2.5;
               // **** CUSTOM VALUE FOR TEKLA **** //

               double x = zoom;
               //if(MySettings.Get("optTekla")=="1")
               //    x = zoom / 2.5;

               //set UI view position and zoom
               XYZ m_xyzTl = uidoc.ActiveView.Origin.Add(uidoc.ActiveView.UpDirection.Multiply(x)).Subtract(uidoc.ActiveView.RightDirection.Multiply(x));
               XYZ m_xyzBr = uidoc.ActiveView.Origin.Subtract(uidoc.ActiveView.UpDirection.Multiply(x)).Add(uidoc.ActiveView.RightDirection.Multiply(x));
               uidoc.GetOpenUIViews().First().ZoomAndCenterRectangle(m_xyzTl, m_xyzBr);
            }
            //perspective
            else if (v.PerspectiveCamera != null)
            {
               if (v.PerspectiveCamera.CameraViewPoint == null || v.PerspectiveCamera.CameraUpVector == null || v.PerspectiveCamera.CameraDirection == null)
                  return;

               //not used since the fov cannot be changed in Revit
               var zoom = v.PerspectiveCamera.FieldOfView;
               //FOV - not used
               //double z1 = 18 / Math.Tan(zoom / 2 * Math.PI / 180);
               //double z = 18 / Math.Tan(25 / 2 * Math.PI / 180);
               //double factor = z1 - z;

               var cameraDirection = RevitUtils.GetRevitXYZ(v.PerspectiveCamera.CameraDirection);
               var cameraUpVector = RevitUtils.GetRevitXYZ(v.PerspectiveCamera.CameraUpVector);
               var cameraViewPoint = RevitUtils.GetRevitXYZ(v.PerspectiveCamera.CameraViewPoint);
               var orient3D = RevitUtils.ConvertBasePoint(doc, cameraViewPoint, cameraDirection, cameraUpVector, true);



               View3D perspView = null;
               //try to use an existing 3D view
               IEnumerable<View3D> viewcollector3D = get3DViews(doc);
               if (viewcollector3D.Any(o => o.Name == "BCFpersp"))
                  perspView = viewcollector3D.First(o => o.Name == "BCFpersp");

               using (var trans = new Transaction(uidoc.Document))
               {
                  if (trans.Start("Open perspective view") == TransactionStatus.Started)
                  {
                     if (null == perspView)
                     {
                        perspView = View3D.CreatePerspective(doc, getFamilyViews(doc).First().Id);
                        perspView.Name = "BCFpersp";
                     }
                     else
                     {
                        //reusing an existing view, I net to reset the visibility
                        //placed this here because if set afterwards it doesn't work
                        perspView.DisableTemporaryViewMode(TemporaryViewMode.TemporaryHideIsolate);
                     }

                     perspView.SetOrientation(orient3D);

                     // turn off the far clip plane
                     if (perspView.get_Parameter(BuiltInParameter.VIEWER_BOUND_ACTIVE_FAR).HasValue)
                     {
                        Parameter m_farClip = perspView.get_Parameter(BuiltInParameter.VIEWER_BOUND_ACTIVE_FAR);
                        m_farClip.Set(0);
                     }
                     perspView.CropBoxActive = true;
                     perspView.CropBoxVisible = true;

                     trans.Commit();
                  }
               }
               uidoc.ActiveView = perspView;
            }
            //sheet
            else if (v.SheetCamera != null)
            {
               IEnumerable<View> viewcollectorSheet = getSheets(doc, v.SheetCamera.SheetID, v.SheetCamera.SheetName);
               if (!viewcollectorSheet.Any())
               {

                  System.Windows.Forms.MessageBox.Show("View " + v.SheetCamera.SheetName + " with Id=" + v.SheetCamera.SheetID + " not found.");
                  return;
               }
               uidoc.ActiveView = viewcollectorSheet.First();
               uidoc.RefreshActiveView();

               XYZ m_xyzTl = new XYZ(v.SheetCamera.TopLeft.X, v.SheetCamera.TopLeft.Y, v.SheetCamera.TopLeft.Z);
               XYZ m_xyzBr = new XYZ(v.SheetCamera.BottomRight.X, v.SheetCamera.BottomRight.Y, v.SheetCamera.BottomRight.Z);
               uidoc.GetOpenUIViews().First().ZoomAndCenterRectangle(m_xyzTl, m_xyzBr);

            }
            //no view included
            else
               return;

            if (v.Components == null)
               return;


            var elementsToSelect = new List<ElementId>();
            var elementsToHide = new List<ElementId>();
            var elementsToShow = new List<ElementId>();

            var visibleElems = new FilteredElementCollector(doc, doc.ActiveView.Id)
            .WhereElementIsNotElementType()
            .WhereElementIsViewIndependent()
            .ToElementIds()
            .Where(e => doc.GetElement(e).CanBeHidden(doc.ActiveView)); //might affect performance, but it's necessary


            bool canSetVisibility = (v.Components.Visibility != null &&
              v.Components.Visibility.DefaultVisibilitySpecified &&
              v.Components.Visibility.Exceptions.Any())
              ;
            bool canSetSelection = (v.Components.Selection != null && v.Components.Selection.Any());



            //loop elements
            foreach (var e in visibleElems)
            {
               var guid = IfcGuid.ToIfcGuid(ExportUtils.GetExportId(doc, e));

               if (canSetVisibility)
               {
                  if (v.Components.Visibility.DefaultVisibility)
                  {
                     if (v.Components.Visibility.Exceptions.Any(x => x.IfcGuid == guid))
                        elementsToHide.Add(e);
                  }
                  else
                  {
                     if (v.Components.Visibility.Exceptions.Any(x => x.IfcGuid == guid))
                        elementsToShow.Add(e);
                  }
               }

               if (canSetSelection)
               {
                  if (v.Components.Selection.Any(x => x.IfcGuid == guid))
                     elementsToSelect.Add(e);
               }
            }





            using (var trans = new Transaction(uidoc.Document))
            {
               if (trans.Start("Apply BCF visibility and selection") == TransactionStatus.Started)
               {
                  if (elementsToHide.Any())
                     doc.ActiveView.HideElementsTemporary(elementsToHide);
                  //there are no items to hide, therefore hide everything and just show the visible ones
                  else if (elementsToShow.Any())
                     doc.ActiveView.IsolateElementsTemporary(elementsToShow);

                  if (elementsToSelect.Any())
                     uidoc.Selection.SetElementIds(elementsToSelect);
               }
               trans.Commit();
            }


            uidoc.RefreshActiveView();
         }
         catch (Exception ex)
         {
            TaskDialog.Show("Error!", "exception: " + ex);
         }

      }


      private IEnumerable<ViewFamilyType> getFamilyViews(Document doc)
      {

         return from elem in new FilteredElementCollector(doc).OfClass(typeof(ViewFamilyType))
                let type = elem as ViewFamilyType
                where type.ViewFamily == ViewFamily.ThreeDimensional
                select type;
      }
      private IEnumerable<View3D> get3DViews(Document doc)
      {
         return from elem in new FilteredElementCollector(doc).OfClass(typeof(View3D))
                let view = elem as View3D
                select view;
      }
      private IEnumerable<View> getSheets(Document doc, int id, string stname)
      {
         ElementId eid = new ElementId(id);
         return from elem in new FilteredElementCollector(doc).OfClass(typeof(View))
                let view = elem as View
                //Get the view with the given Id or given name
                where view.Id == eid | view.Name == stname
                select view;

      }




      public string GetName()
      {
         return "Open 3D View";
      }
   }
}
