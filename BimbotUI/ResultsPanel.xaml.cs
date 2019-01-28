using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Autodesk.Revit.UI;
using Bimbot.Bcf;
using Bimbot.ExternalEvents;
using Bimbot.Objects;
using Xceed.Wpf.Toolkit.PropertyGrid;

namespace Bimbot.BimbotUI
{
   /// <summary>
   /// Interaction logic for ResultPanel.xaml
   /// </summary>
   public partial class ResultsPanel : Page, Autodesk.Revit.UI.IDockablePaneProvider
   {
      #region Data
//      private BimbotDocument botDoc;
      private ExternalEventsContainer ExtEvents;
      #endregion

      public ResultsPanel(ExternalEventsContainer extEvents)
      {
         try
         {
            ExtEvents = extEvents;
            InitializeComponent();
         }
         catch (Exception ex)
         {

         }
      }


      public class ServiceItem
      {
         public string title { get; set; }
         public string service { get; set; }
         public string date { get; set; }
         public string type { get; set; }
         public string textData { get; set; }
         public Markup issueData { get; set; }
      }
    

/*      public void UpdateView()
      {
         issuesList.Items.Clear();

         foreach (Service service in ((BimbotDocument)DataContext).AssignedServices)
         {
            if (service.Result == null)
               continue;

            if (service.Result.isBcf)
            {
//               if (service.Result.bcf == null)
//                  service.Result.bcf = new BcfFile(Convert.FromBase64String(service.Result.data));

               //Set markups to interface
               foreach (KeyValuePair<string, Markup> entry in service.Result.bcf.markups)
               {
                  issuesList.Items.Add(new ServiceItem
                  {
                     title = entry.Value.Topic.Title,
                     service = service.Name,
                     date = service.Result.lastRun.ToString(),
                     type = "bcf",
                     issueData = entry.Value
                  });
               }
            }
            else
            {
               issuesList.Items.Add(new ServiceItem
               {
                  title = service.Name,
                  service = service.Name,
                  date = service.Result.lastRun.ToString(),
                  type = "text",
                  textData = service.Result.data
               });
            }
         }
      }
*/
      
      public void SetupDockablePane(Autodesk.Revit.UI.DockablePaneProviderData data)
      {
         data.FrameworkElement = this as FrameworkElement;
         data.InitialState = new Autodesk.Revit.UI.DockablePaneState();
         data.InitialState.DockPosition = DockPosition.Tabbed;
         data.InitialState.TabBehind = Autodesk.Revit.UI.DockablePanes.BuiltInDockablePanes.ProjectBrowser;
      }
      


      private void IssuesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         topicItems.SelectedObject = null;
         commentCombo.Items.Clear();
         viewpointCombo.Items.Clear();

         if (issuesList.SelectedItems.Count == 1)
         {
            if (((ServiceItem)issuesList.SelectedItems[0]).type.Equals("bcf"))
            {
               bcfIssue.Visibility = System.Windows.Visibility.Visible;
               textIssue.Visibility = System.Windows.Visibility.Hidden;
               Markup curMarkup = (Markup)((ServiceItem)issuesList.SelectedItems[0]).issueData;
               topicItems.SelectedObject = curMarkup.Topic;

               commentCombo.Items.Clear();
               if (curMarkup.Comment != null)
               {
                  foreach (Comment com in curMarkup.Comment)
                     commentCombo.Items.Add(com);
                  commentCombo.SelectedIndex = 0;
               }

               viewpointCombo.Items.Clear();
               if (curMarkup.Viewpoints != null)
               {
                  foreach (ViewPoint vp in curMarkup.Viewpoints)
                     viewpointCombo.Items.Add(vp);
                  viewpointCombo.SelectedIndex = 0;
               }
            }
            else
            {
               bcfIssue.Visibility = System.Windows.Visibility.Hidden;
               textIssue.Visibility = System.Windows.Visibility.Visible;
               textIssue.Text = (string)((ServiceItem)issuesList.SelectedItems[0]).textData;
            }
         }
      }

      private void IssuesList_DoubleClick(object sender, MouseButtonEventArgs e)
      {
         HitTestResult r = VisualTreeHelper.HitTest(this, e.GetPosition(this));
         if (r.VisualHit.GetType() == typeof(Image))
         {
            ViewPoint curVp = (ViewPoint)viewpointCombo.SelectedItem;

            if (curVp != null)
            {
               ExtEvents.ChangeViewHandler.v = curVp.ViewpointRef;
               ExtEvents.ChangeViewEvent.Raise();
            }
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

            //ModifyRevitView(curVp);
            ExtEvents.ChangeViewHandler.v = curVp.ViewpointRef;
            ExtEvents.ChangeViewEvent.Raise();
         }
         else
         {
            viewpointItems.SelectedObject = null;
         }
      }


      private void topicItems_SelectedPropertyItemChanged(object sender, RoutedPropertyChangedEventArgs<PropertyItemBase> e)
      {
         PropertyItem propItem = topicItems.SelectedPropertyItem as PropertyItem;
         if (propItem != null && propItem.PropertyName.Equals("BimSnippet") && propItem.Value != null)
         {
            BimSnippet snippet = (BimSnippet)propItem.Value;
            Markup curMarkup = ((ServiceItem)issuesList.SelectedItems[0]).issueData;
            ExtEvents.IfcImportHandler.filePath = Path.Combine(Path.GetTempPath(), curMarkup.Topic.Guid + ".ifc");

            if (snippet.isExternal)
            {
               // Read data from URL
               using (var client = new WebClient())
               {
                  client.DownloadFile(snippet.Reference, ExtEvents.IfcImportHandler.filePath);
               }
            }
            else
            {
               // Read data from zip
               using (var fileStream = File.Create(ExtEvents.IfcImportHandler.filePath))
               {
                  snippet.RefData.CopyTo(fileStream);
               }
            }

            // Get the data and create local file 
            ExtEvents.IfcImportEvent.Raise();
         }
      }
   }
}
