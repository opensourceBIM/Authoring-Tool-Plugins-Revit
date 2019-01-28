using Bimbot.Bcf;
using Bimbot.Bcf.Bcf2;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfBcfPanelTester
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      public MainWindow()
      {
         InitializeComponent();
         this.DataContext = App.test;
      }

      private void Browse1_Click(object sender, RoutedEventArgs e)
      {
         // Displays an OpenFileDialog so the user can select a Cursor.  
         OpenFileDialog dlg = new OpenFileDialog();
         dlg.Filter = "BCF Files|*.bcf|BCF Files|*.bcfzip";
         dlg.Title = "Open Bcf file";


         if (dlg.ShowDialog() == true)
         {
            byte[] fileBytes = File.ReadAllBytes(dlg.FileName);
            fileName.Text = dlg.FileName;

            DataContext = (new BcfFile(fileBytes)).markups;
/*
            if (bcfFile != null)
            {
               //Set markups to interface
               foreach (KeyValuePair<string, Markup> entry in bcfFile.markups)
               {
                  issuesList.Items.Add(entry.Value);
               }
            }
*/
         }
      }

      private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         if (issuesList.SelectedItems.Count == 1)
         {
            Markup curMarkup = (Markup)issuesList.SelectedItem;
            topicItems.SelectedObject = curMarkup.Topic;


            commentCombo.Items.Clear();
            foreach (Comment com in curMarkup.Comment)
            {
               commentCombo.Items.Add(com);
            }
            if (commentCombo.Items.Count > 0)
               commentCombo.SelectedIndex = 0;


            viewpointCombo.Items.Clear();
            foreach (ViewPoint vp in curMarkup.Viewpoints)
            {
               viewpointCombo.Items.Add(vp);
            }
            if (viewpointCombo.Items.Count > 0)
               viewpointCombo.SelectedIndex = 0;
         }
         
      }

      private void commentCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         if (commentCombo.SelectedItem != null)
         {
            Comment curComment = (Comment)commentCombo.SelectedItem;
            commentItems.SelectedObject = curComment;
         }
         else
         {
            commentItems.SelectedObject = null;
         }
      }

      private void viewpointCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         if (viewpointCombo.SelectedItem != null)
         {
            ViewPoint curVp = (ViewPoint)viewpointCombo.SelectedItem;
            viewpointItems.SelectedObject = curVp.ViewpointRef;
         }
         else
         {
            viewpointItems.SelectedObject = null;
         }
      }

      /*
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
               }
            }
            */

   }
}
