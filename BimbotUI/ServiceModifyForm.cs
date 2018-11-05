using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB;

namespace Bimbot.BimbotUI
{
   public partial class ServiceModifyForm : System.Windows.Forms.Form
   {
//      private Service curService = new Service();
      private Document curDoc;

      public object textToTrigger { get; private set; }

      public ServiceModifyForm(Document doc)
      {
         InitializeComponent();
         curDoc = doc;
      }


      private void ServiceSetupForm_Load(object sender, EventArgs e)
      {
         // Set Trigger comboboxes
         serviceTrigger.Items.Clear();
         foreach (string key in ServiceAddForm.textToTrigger.Keys)
         {
            serviceTrigger.Items.Add(key);
         }

         // Fill active services
         listActiveServices.Items.Clear();
         Entity ent = curDoc.ProjectInformation.GetEntity(RevitBimbot.Schema);
         if (ent != null)
         {
            IList<Entity> services = ent.Get<IList<Entity>>("services");
            foreach(Entity service in services)
            {
               ListViewItem item = listActiveServices.Items.Add(service.Get<String>("srvcName"));
               item.SubItems.Add(service.Get<string>("resultDate"));
               item.Tag = service;
            }
         }
         changeButton.Enabled = false;
         deleteButton.Enabled = false;
      }

      private void ServiceSoid_TextChanged(object sender, EventArgs e)
      {

      }

      private void ServiceTrigger_SelectedIndexChanged(object sender, EventArgs e)
      {

      }

      private void ServiceToken_TextChanged(object sender, EventArgs e)
      {

      }

      private void ChangeButton_Click(object sender, EventArgs e)
      {

      }

      private void DeleteButton_Click(object sender, EventArgs e)
      {

      }

      private void ListActive_SelectedIndexChanged(object sender, EventArgs e)
      {
         if (listActiveServices.SelectedItems.Count == 1)
         {
            // Show fields for selected item
            Entity service = (Entity)listActiveServices.SelectedItems[0].Tag;

            serviceName.Text = service.Get<string>("srvcName");
            serviceDescription.Text = service.Get<string>("srvcDesc");
            serviceUrl.Text = service.Get<string>("srvcUrl");
            serviceToken.Text = service.Get<string>("srvcToken");
            serviceSoid.Text = service.Get<int>("srvcSoid").ToString();
            serviceTrigger.SelectedIndex = service.Get<int>("srvcTrigger");

            changeButton.Enabled = true;
            deleteButton.Enabled = true;
         }
         else
         {
            // Clear fields
            serviceName.Text = "";
            serviceDescription.Text = "";
            serviceName.Text = "";
            serviceUrl.Text = "";
            serviceToken.Text = "";
            serviceSoid.Text = "";
            serviceTrigger.Text = "";

            changeButton.Enabled = false;
            deleteButton.Enabled = false;
         }
      }
   }
}
