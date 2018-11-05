using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using Bimbot.Objects;
using Newtonsoft.Json;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB;
using System.Diagnostics;

namespace Bimbot.BimbotUI
{
   public partial class ServiceAddForm : System.Windows.Forms.Form
   {
      public static readonly Dictionary<string, RevitEvntTrigger> textToTrigger = new Dictionary<string, RevitEvntTrigger>
      {
         {"button",                   RevitEvntTrigger.manualButton },
         {"structural element changed",RevitEvntTrigger.manualButton },
         {"saved document",           RevitEvntTrigger.documentSaved },
         {"closed document",          RevitEvntTrigger.documentClosed },
         {"opened document",          RevitEvntTrigger.documentOpened },
         {"modified document",        RevitEvntTrigger.documentModified },
         {"committed transaction",    RevitEvntTrigger.transactionCommitted },
         {"all",                      RevitEvntTrigger.all }
      };


      //      private Service curService = new Service();
      private Service curService;


      public ServiceAddForm(Service service)
      {
         InitializeComponent();
         curService = service;

         serviceName.Text = curService.Name;
         serviceProvider.Text = curService.Provider;
         userName.Text = System.Security.Principal.WindowsIdentity.GetCurrent().Name;


         /* temp solution to overcome OAuth */
         if (curService.Name.Equals("Simple Analyses Service"))
         {
            if (curService.host.listUrl.Equals("https://ifcanalysis.bimserver.services/servicelist"))
            {
               newSoid.Text = "720974";
               //                  newToken.Text = "70a379ff10141931ee62e598c2f645e942d658e8a9ec98151dc31065ce60478abe5fe392cedc2c92233015176c6198bb00fab77a88bdd1c939576ee512d7f059";
               newToken.Text = "4e620dd2b4af8e702e41ad082fcb9552368331a8853e3cc5ace6504e453214e3be5fe392cedc2c92233015176c6198bb00fab77a88bdd1c939576ee512d7f059";
            }
            else if (curService.host.listUrl.Equals("http://localhost:8082/servicelist"))
            {
               newSoid.Text = "262222";
               newToken.Text = "2f7c4a24267f95729de33ad384a0a10bcf2390d64011201077402a2dde3255d2079360b3e4655e74924e57608bdddca413870da2ff7fb081e3ee88ed176ad0c0";
            }
         }
         else if (curService.Name.Equals("Validate Model Service"))
         {
            if (curService.host.listUrl.Equals("https://ifcanalysis.bimserver.services/servicelist"))
            {
               newSoid.Text = "327758";
               newToken.Text = "895f555dbe081dbec5b8e4222678bf778b39c59793af7e40d4b7a1acae5d67429fbfdc68f3725742f07c1e5f4435f614511f9a3126250c0b634edf4b2b0ed613";
            }
         }

         // Set Trigger comboboxes
         newTrigger.Items.Clear();
         foreach (string key in textToTrigger.Keys)
         {
            newTrigger.Items.Add(key);
         }

         // Get servers 
         buttonAdd.Enabled = false;
      }


      private void EnableDisableAddButton(object sender, EventArgs e)
      {
         buttonAdd.Enabled = (!newTrigger.Text.Equals("") &&
                              !newToken.Text.Equals("") &&
                              !newSoid.Text.Equals(""));
      }


      private void AddServiceClick(object sender, EventArgs e)
      {
         curService.soid = Convert.ToInt32(newSoid.Text);
         curService.srvcToken = newToken.Text;
         curService.Trigger = textToTrigger[newTrigger.Text];

         ServiceProfile prof = new ServiceProfile();
         prof.userName = userName.Text;
         prof.soid = curService.soid;
         prof.srvcToken = curService.srvcToken;
         prof.triggers.Add(curService.Trigger);

         curService.profile.Add(prof);
      }


      private void buttonRegister_Click_1(object sender, EventArgs e)
      {
         System.Diagnostics.Process.Start("http://google.com");
      }
   }
   
}
