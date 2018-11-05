using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bimbot.Forms
{
   public partial class ServiceRunning : Form
   {
      public ServiceRunning()
      {
         InitializeComponent();
      }

      public void SetLabel(string str)
      {
         label1.Text = str;
      }
   }
}
