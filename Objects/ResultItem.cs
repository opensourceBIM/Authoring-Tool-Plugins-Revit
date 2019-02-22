using Bimbot.Bcf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Bimbot.Objects
{
   public class ResultItem
   {
      public Service OfService { get; set; }
      public string  Name { get; set; }
      public string Type { get; set; }
      public string TextData { get; set; }
      public Markup IssueData { get; set; }
      public DateTime LastRun { get; set; }

      public bool IsBcf { get { return Type.Equals("bcf"); } }
      public bool IsTxt { get { return !Type.Equals("bcf"); } }

      public Visibility BcfVisible { get { return IsBcf ? Visibility.Visible : Visibility.Hidden; } }


      public ResultItem(Service service, Markup markup)
      {
         Name = markup.Topic.Title;
         OfService = service;
         IssueData = markup;
         LastRun = service.Result.lastRun;
         Type = "bcf";
      }

      public ResultItem(Service service)
      {
         Name = service.Name + " result";
         OfService = service;
         TextData = service.Result.data;
         LastRun = service.Result.lastRun;
         Type = "txt";
      }


   }
}
