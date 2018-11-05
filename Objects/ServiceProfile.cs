using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bimbot.Objects
{
   public class ServiceProfile
   {
      public string userName;
      public int soid;
      public List<RevitEvntTrigger> triggers = new List<RevitEvntTrigger>();
      //public string srvcUrl;
      public string srvcToken;


      // fieldBldr = serviceBldr.AddSimpleField("srvcUrl", typeof(string));
      // fieldBldr = serviceBldr.AddSimpleField("srvcToken", typeof(string));
      // fieldBldr = serviceBldr.AddSimpleField("srvcSoid", typeof(int));
      // fieldBldr = serviceBldr.AddSimpleField("srvcTrigger", typeof(int));
   }
}
