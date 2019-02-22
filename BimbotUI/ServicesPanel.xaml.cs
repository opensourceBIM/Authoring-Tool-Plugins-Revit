using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;
using Newtonsoft.Json;
using Bimbot.Bcf;
using Bimbot.ExternalEvents;
using Bimbot.Objects;
using Bimbot.Utils;
using System.ComponentModel;
using System.Threading;
using Autodesk.Windows;
using System.Collections.ObjectModel;

namespace Bimbot.BimbotUI
{
   /// <summary>
   /// Interaction logic for UserControl1.xaml
   /// </summary>
   public partial class ServicesPanel : Page, Autodesk.Revit.UI.IDockablePaneProvider
   {
      #region Data
      private ExternalEventsContainer         ExtEvents;
      private Guid m_targetGuid; 
      private DockPosition m_position = DockPosition.Bottom; 
      private int m_left = 1; 
      private int m_right = 1; 
      private int m_top = 1; 
      private int m_bottom = 1;

//      private ServiceAddWindow addWindow;
      #endregion

      public ServicesPanel(ExternalEventsContainer extEvents)
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


      public void SetupDockablePane(Autodesk.Revit.UI.DockablePaneProviderData data)
      { 
        data.FrameworkElement = this as FrameworkElement; 
        data.InitialState = new Autodesk.Revit.UI.DockablePaneState(); 
        data.InitialState.DockPosition = DockPosition.Tabbed; 
        //DockablePaneId targetPane; 
        //if (m_targetGuid == Guid.Empty) 
        //    targetPane = null; 
        //else targetPane = new DockablePaneId(m_targetGuid); 
        //if (m_position == DockPosition.Tabbed) 
        data.InitialState.TabBehind = Autodesk.Revit.UI.DockablePanes.BuiltInDockablePanes.ProjectBrowser; 
       //if (m_position == DockPosition.Floating) 
        //{ 
       //data.InitialState.SetFloatingRectangle(new Autodesk.Revit.UI.Rectangle(10, 710, 10, 710)); 
        //data.InitialState.DockPosition = DockPosition.Tabbed; 
        //} 
        //Log.Message("***Intial docking parameters***"); 
        //Log.Message(APIUtility.GetDockStateSummary(data.InitialState)); 
      }

      public void SetInitialDockingParameters(int left, int right, int top, int bottom, DockPosition position, Guid targetGuid)
      { 
        m_position = position; 
        m_left = left; 
        m_right = right; 
        m_top = top; 
        m_bottom = bottom; 
        m_targetGuid = targetGuid; 
      } 
   

      private void AddService(object sender, RoutedEventArgs e)
      {
         ServiceAddWindow addWindow = new ServiceAddWindow((BimbotDocument) DataContext);
         if (addWindow.ShowDialog() == true)
         {
            ((BimbotDocument)DataContext).AddService(addWindow.CurrentService);
            ExtEvents.ChangeDocumentEvent.Raise();
         }
      }


      private void DelService(object sender, RoutedEventArgs e)
      {
         if (servicesList.SelectedItems.Count == 1)
         {
           ((BimbotDocument)DataContext).DelService((Service)servicesList.SelectedItem);
            ExtEvents.ChangeDocumentEvent.Raise();
         }
      }


      private void ModService(object sender, RoutedEventArgs e)
      {

      }


      private void ProtectService(object sender, RoutedEventArgs e)
      {
         if (servicesList.SelectedItems.Count == 1)
         {
            Service CurrentService = (Service)servicesList.SelectedItem;
            ProtectServiceWindow ProtectWindow = new ProtectServiceWindow();
            ProtectWindow.Title = "(Un)protect service '" + CurrentService.Name + "'";
            if (ProtectWindow.ShowDialog() == true)
            {
               CurrentService.Protect(ProtectWindow.Password.Text);
               ExtEvents.ChangeDocumentEvent.Raise();
            }
         }
      }

      private void UnlockServices(object sender, RoutedEventArgs e)
      {
         ProtectServiceWindow UnlockWindow = new ProtectServiceWindow();
         UnlockWindow.Title = "Unlock protected Services";
         if (UnlockWindow.ShowDialog() == true)
         {
            foreach (Service currentService in servicesList.Items)
            {
               currentService.UnProtect(UnlockWindow.Password.Text);
            }
         }
      }




      private void RunSelected(object sender, RoutedEventArgs e)
      {
         ExtEvents.RunServicesHandler.services.Clear();
         foreach (Service curService in servicesList.SelectedItems)
         {
            ExtEvents.RunServicesHandler.services.Add(curService);
         }
         ExtEvents.RunServicesEvent.Raise();
      }


      private void RunAll(object sender, RoutedEventArgs e)
      {
         // Create list of tasks from services to run
         ExtEvents.RunServicesHandler.services.Clear();
         foreach (Service curService in servicesList.Items)
         {
            ExtEvents.RunServicesHandler.services.Add(curService);
         }
         ExtEvents.RunServicesEvent.Raise();
      }
   }
}
