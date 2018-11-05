using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bimbot.Objects
{
   public enum RevitEvntTrigger
   {
      manualButton,
      documentSaved,
      documentClosed,
      documentOpened,
      documentModified,
      transactionCommitted,
      all
   };

}
