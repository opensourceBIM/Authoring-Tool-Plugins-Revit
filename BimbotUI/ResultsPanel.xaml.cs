using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Autodesk.Revit.UI;
using Bimbot.Bcf;
using Bimbot.ExternalEvents;
using Bimbot.Objects;

namespace Bimbot.BimbotUI
{
   /// <summary>
   /// Interaction logic for ResultPanel.xaml
   /// </summary>
   public partial class ResultsPanel : Page, Autodesk.Revit.UI.IDockablePaneProvider
   {
      #region Data
      private ExternalEvent ExtEvent;
      private ExtEvntChangeView Handler;
      private BimbotDocument botDoc;
      #endregion

      public ResultsPanel(ExternalEvent exEvent, ExtEvntChangeView handler)
      {
         try
         {
            ExtEvent = exEvent;
            Handler = handler;
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
    

      public void ShowDocument(BimbotDocument _botDoc)
      {
         botDoc = _botDoc;
         UpdateView();
      }

      public void UpdateView()
      {
         issuesList.Items.Clear();
         foreach (Service service in botDoc.registeredServices)
         {
            if (service.result == null)
               continue;

            if (service.result.isBcf)
            {
               if (service.result.bcf != null)
               {
                  //Set markups to interface
                  foreach (KeyValuePair<string, Markup> entry in service.result.bcf.markups)
                  {
                     issuesList.Items.Add(new ServiceItem
                     {
                        title = entry.Value.Topic.Title,
                        service = service.Name,
                        date = service.result.lastRun.ToString(),
                        type = "bcf",
                        issueData = entry.Value
                     });
                  }
               }
            }
            else
            {
               issuesList.Items.Add(new ServiceItem
               {
                  title = service.Name,
                  service = service.Name,
                  date = service.result.lastRun.ToString(),
                  type = "text",
                  textData = service.result.data
               });
            }
         }
      }

      
      public void SetupDockablePane(Autodesk.Revit.UI.DockablePaneProviderData data)
      {
         data.FrameworkElement = this as FrameworkElement;
         data.InitialState = new Autodesk.Revit.UI.DockablePaneState();
         data.InitialState.DockPosition = DockPosition.Tabbed;
         data.InitialState.TabBehind = Autodesk.Revit.UI.DockablePanes.BuiltInDockablePanes.ProjectBrowser;
      }
      
      /*
      private void DockableDialogs_Loaded(object sender, RoutedEventArgs e)
      {

      }
      */

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
               Handler.v = curVp.ViewpointRef;
               ExtEvent.Raise();
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
            Handler.v = curVp.ViewpointRef;
            ExtEvent.Raise();
         }
         else
         {
            viewpointItems.SelectedObject = null;
         }
      }     
   }
}
