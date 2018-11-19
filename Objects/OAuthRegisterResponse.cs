using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bimbot.Objects
{
   class OAuthRegisterResponse
   {
      public string client_id { get; set; }
      public string client_secret { get; set; }
      public string issued_at { get; set; }
      public string expires_in { get; set; }
   }
}
