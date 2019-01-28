using Autodesk.Revit.UI;
using Bimbot.ExternalEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bimbot
{
   // Container holding all external events and handlers to share them among Services

   public class ExternalEventsContainer
   {
      public ExtEvntSetViewpoint ChangeViewHandler { get; private set; }
      public ExternalEvent ChangeViewEvent { get; private set; }

      public ExtEvntSetBimbotData ChangeDocumentHandler { get; private set; }
      public ExternalEvent ChangeDocumentEvent { get; private set; }

      public ExtEvntRunServices RunServicesHandler { get; private set; }
      public ExternalEvent RunServicesEvent { get; private set; }

      public ExtEvntImportIfcSnippet IfcImportHandler { get; private set; }
      public ExternalEvent IfcImportEvent { get; private set; }

      public ExternalEventsContainer()
      {
         ChangeViewHandler = new ExtEvntSetViewpoint();
         ChangeViewEvent = ExternalEvent.Create(ChangeViewHandler);

         ChangeDocumentHandler = new ExtEvntSetBimbotData();
         ChangeDocumentEvent = ExternalEvent.Create(ChangeDocumentHandler);

         RunServicesHandler = new ExtEvntRunServices();
         RunServicesEvent = ExternalEvent.Create(RunServicesHandler);

         IfcImportHandler = new ExtEvntImportIfcSnippet();
         IfcImportEvent = ExternalEvent.Create(IfcImportHandler);
      }
   }
}
