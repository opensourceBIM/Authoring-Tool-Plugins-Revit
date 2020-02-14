using Bimbot.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Bimbot.BimbotUI
{
   /// <summary>
   /// Interaction logic for ServiceRegisterWindow.xaml
   /// </summary>
   public partial class ProtectServiceWindow : Window
   {
      private Service CurrentService;

      public ProtectServiceWindow()
      {
         InitializeComponent();
         ButtonApply.IsEnabled = true;
         Password.Text = "";
      }

      private void SetButtonState(object sender, TextChangedEventArgs e)
      {
         // enable or disable the add button 
//         ButtonApply.IsEnabled = (!Password.Text.Equals(""));
      }

      private void ButtonApply_Click(object sender, RoutedEventArgs e)
      {
         DialogResult = true;
      }
   }
}
