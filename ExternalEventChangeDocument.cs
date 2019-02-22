using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
//using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Bimbot.Bcf;
using Bimbot.Utils;
using Bimbot.Objects;
using Autodesk.Revit.DB.ExtensibleStorage;

namespace Bimbot.ExternalEvents
{
   /// <summary>
   /// Obfuscation Ignore for External Interface
   /// </summary>
   public class ExtEvntSetBimbotData : IExternalEventHandler
   {
      public BimbotDocument documentToUpdate;

      /// <summary>
      /// External Event Implementation
      /// </summary>
      /// <param name="app"></param>
      public void Execute(UIApplication app)
      {
/*         foreach (Service serv in documentToUpdate.AssignedServices)
         {
            serv.UpdateResults();
         }
*/
         documentToUpdate.WriteToRevit();
      }
 

      public string GetName()
      {
         return "Update/Create Service";
      }
   }
}