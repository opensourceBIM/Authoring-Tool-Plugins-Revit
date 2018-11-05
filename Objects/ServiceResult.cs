using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bimbot.Bcf;

namespace Bimbot.Objects
{
   public class ServiceResult
   {
      public bool isBcf;
      public string data;
      public BcfFile bcf;
      public DateTime lastRun;
   }
}
