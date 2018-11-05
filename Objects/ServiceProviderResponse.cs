using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bimbot.Objects
{
   public class ServiceProviderResponse
   {
      public SProvider[] active;
   }

   public class SProvider
   {
      public string name;        // fieldBldr = serviceBldr.AddSimpleField("hostName", typeof(string));
      public string description; // fieldBldr = serviceBldr.AddSimpleField("hostDesc", typeof(string));
      public string listUrl;     // fieldBldr = serviceBldr.AddSimpleField("hostUrl", typeof(string));
   }
}