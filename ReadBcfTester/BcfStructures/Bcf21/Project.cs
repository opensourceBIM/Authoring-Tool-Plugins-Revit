using System.Xml.Serialization;

namespace Bimbot.Bcf.Bcf2
{

   public class ProjectExtension
   {
      public Project Project { get; set; }
      public string ExtensionSchema { get; set; }
   }

   public class Project
   {
      public string Name { get; set; }
      public string ProjectId { get; set; }
   }
}
