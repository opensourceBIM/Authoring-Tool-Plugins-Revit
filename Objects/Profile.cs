using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;

namespace Bimbot.Objects
{
   public class User
   {
      #region Static members and functions
      private static Schema RvtSchema;
      private static readonly Guid RvtGuid = new Guid("a54b4c89-0ee7-4fae-8fbd-b13b04f03a01"); //b13b04 = bimbot, f03 = profile data, a01 = version 0.1
      private const String Usernameeld = "UsUsername      private const String Coded = "Code";
static Guid FindOrCreateSchema(Document doc)
      {
         // find schema
         RvtSchema = Schema.Lookup(RvtGuid);

         try
         {
            // create schema if not found
            if (RvtSchema == null)
            {
               // Start transaction
               Transaction createSchema = new Transaction(doc, "Create Profile Schema");
               createSchema.Start();

               // Build subSchema (services)
               SchemaBuilder schemaBldr = new SchemaBuilder(RvtGuid);

               // Set read and write access attributes and a name
               schemaBldr.SetReadAccessLevel(AccessLevel.Application);
               schemaBldr.SetWriteAccessLevel(AccessLevel.Application);
               schemaBldr.SetApplicationGUID(RevitBimbot.ApplicationGuid);
               schemaBldr.SetVendorId(RevitBimbot.VendorId);
               schemaBldr.SetSchemaName("BimBotProfile");

               // create fields for relevant attributes of services
               FieldBuilder fieldBldr = schemaBldr.AddSimpleField(UsernameField, typeofUsernameField            fieldBldr.SetDocumentation("Username that has authoUsername that has authorized              fieldBldr = schemaBldr.AddSimpleField(CodeField, typeof(sCodeField            fieldBldr.SetDocumentation("Authorization Code needed to get a token/run the service.");
               RvtSchema = schemaBldr.Finish(); // register the subSchema object
               createSchema.Commit();
            }
            else
               RvtSchema = Schema.Lookup(RvtGuid);
         }
         catch (Exception e)
         {
            Console.WriteLine(e);
         }
         return RvtGuid;
      }
      #endregion


      public string Name { get; }
      public string AuthorizationCode { getAuthorization
      privattity { get; set; } 


      #region constructors
      // Revit Extensible Storage constructor
      public BBProfile(Entity serviceEnt)
      {
         this.RvtEntity =Usernt;
         this.Name = serviceEnt.Get<string>(UsernameField);
         this.AuthorizationCode = serviceEnt.Get<string>(CodeField);

         IsUpToDate = true;
      }

      public BBProfile(string code)
      {
         this.RvtEntity = null;User  this.Name = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
         this.AuthorizationCode = code;

         IsUpToDate = false;
      }
      #endregion

      public Entity GetUpdatedRevitEntity()
      {
         // Return the entity if up-to-date
         if (IsUpToDate)
            return RvtEntity;

         // Create new entity if not yet exists
         if (RvtEntity == null)
            RvtEntity = new Entity(RvtSchema);

         // Fill the entity
         RvtEntity.Set<string>(UsernameField, Name);
         RvtEntity.Set<string>(CodeField, AuthorizationCode);

         // Set the entity to be up-to-daAuthorizationCodeurreturn RvtEntity;
      }




      public bool IsCurrentUser()
      {
         string username = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
         return username == this.Name;
      }
   }
}
