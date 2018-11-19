using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using adWin = Autodesk.Windows;

namespace Bimbot.BimbotUI
{
   public partial class TestUIForm : Form
   {
      public TestUIForm()
      {
         InitializeComponent();

      }


      public void Setup(adWin.RibbonControl ribbon)
      {
         try
         {
            // find the view tab
            listing1.Items.Clear();
            foreach (adWin.RibbonTab tab in ribbon.Tabs)
            {
               ListViewItem item = listing1.Items.Add(tab.Id);
               item.Tag = tab;
            }
         }
         catch (Exception e)
         {
            //failed to add button, don't do a thing
         }
      }

      private void listing1_SelectedIndexChanged(object sender, EventArgs e)
      {
         listing2.Items.Clear();
         if (listing1.SelectedItems.Count == 1)
         {
            foreach (adWin.RibbonPanel panel in ((adWin.RibbonTab)listing1.SelectedItems[0].Tag).Panels)
            {
               ListViewItem item = listing2.Items.Add(panel.Source.Id);
               item.Tag = panel.Source;
            }
         }
      }

 
      private void listing2_SelectedIndexChanged(object sender, EventArgs e)
      {
         listing3.Items.Clear();
         if (listing2.SelectedItems.Count == 1)
         {
            foreach (adWin.RibbonItem control in ((adWin.RibbonPanelSource)listing2.SelectedItems[0].Tag).Items)
            {
               ListViewItem item = listing3.Items.Add(control.Id);
               item.Tag = control;
            }
         }
      }

      private void listing3_SelectedIndexChanged(object sender, EventArgs e)
      {
         if (listing3.SelectedItems.Count == 1)
         {
            adWin.RibbonItem item = (adWin.RibbonItem) listing3.SelectedItems[0].Tag;
            if (item.Id == "ID_IFC_LINK")
               ((adWin.RibbonButton) item).CommandHandler.Execute(null);
         }
      }
   }
}
