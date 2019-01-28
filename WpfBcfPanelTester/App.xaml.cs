using Bimbot.Bcf.Bcf2;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WpfBcfPanelTester
{
   /// <summary>
   /// Interaction logic for App.xaml
   /// </summary>
   public partial class App : Application
   {
      public static Dictionary<string, Markup> test = new Dictionary<string, Markup>{ { "My", null} , { "Test", null }, { "Window", null } }; 
   }
}
