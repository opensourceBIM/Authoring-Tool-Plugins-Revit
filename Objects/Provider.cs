using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Bimbot.Objects
{
   public class Provider // : INotifyPropertyChanged
   {
      #region Static members and functions
      private static Schema RvtSchema;
      private static readonly Guid RvtGuid = new Guid("a54b4c89-0ee7-4fae-8fbd-b13b04f02a01"); //b13b04 = bimbot, f02 = provider data, a01 = version 0.1
      private const String NameField = "Name";
      private const String IconField = "Icon";
      private const String RegisterUrlField = "RegisterUrl";
      private const String AuthorizeUrlField = "AuthorizeUrl";
      private const String TokenUrlField = "TokenUrl";
      private const String ClientIdField = "ClientId";
      private const String ClientSecretField = "ClientSecret";

      public static Guid FindOrCreateSchema(Document doc)
      {
         // find schema
         RvtSchema = Schema.Lookup(RvtGuid);

         try
         {
            // create schema if not found
            if (RvtSchema == null)
            {
               // Start transaction
               Transaction createSchema = new Transaction(doc, "Create Provider Schema");
               createSchema.Start();

               // Build schema
               SchemaBuilder schemaBldr = new SchemaBuilder(RvtGuid);

               // Set read and write access attributes and a name
               schemaBldr.SetReadAccessLevel(AccessLevel.Application);
               schemaBldr.SetWriteAccessLevel(AccessLevel.Application);
               schemaBldr.SetApplicationGUID(RevitBimbot.ApplicationGuid);
               schemaBldr.SetVendorId(RevitBimbot.VendorId);
               schemaBldr.SetSchemaName("BimBotProvider");

               // create fields for relevant attributes of services
               FieldBuilder fieldBldr = schemaBldr.AddSimpleField(NameField, typeof(string));
               fieldBldr.SetDocumentation("Name of the provider");
               fieldBldr = schemaBldr.AddSimpleField(IconField, typeof(string));
               fieldBldr.SetDocumentation("Icon of the provider");
               fieldBldr = schemaBldr.AddSimpleField(ClientIdField, typeof(string));
               fieldBldr.SetDocumentation("Id of this document at provider");
               fieldBldr = schemaBldr.AddSimpleField(ClientSecretField, typeof(string));
               fieldBldr.SetDocumentation("Secret of this document at provider");
               fieldBldr = schemaBldr.AddSimpleField(RegisterUrlField, typeof(string));
               fieldBldr.SetDocumentation("OAuth URL for registering at provider");
               fieldBldr = schemaBldr.AddSimpleField(AuthorizeUrlField, typeof(string));
               fieldBldr.SetDocumentation("OAuth URL for authorizing at provider");
               fieldBldr = schemaBldr.AddSimpleField(TokenUrlField, typeof(string));
               fieldBldr.SetDocumentation("OAuth URL for token request at provider");

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


      #region members
      public string Name { get; set; }
      public string Icon { get; set; }

      public string ClientId { get; set; }
      public string ClientSecret { get; set; }

      public string RegisterUrl { get; set; }
      public string AuthorizeUrl { get; set; }
      public string TokenUrl { get; set; }

      public Entity RvtEntity { get; private set; }

      public List<Service> Services { get; }

      public Brush RegisteredColor
      {
         get
         {
            return ClientId == "" ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.Green);
         }
      }

      public bool IsUpToDate;

      #endregion


      #region Constructor
      // Copy constructor
      public Provider(Provider provider)
      {
         RvtEntity = null;
         this.Name = provider.Name;
         this.Icon = provider.Icon;
         this.RegisterUrl = provider.RegisterUrl;
         this.AuthorizeUrl = provider.AuthorizeUrl;
         this.TokenUrl = provider.TokenUrl;
         this.ClientId = "";
         this.ClientSecret = "";
         IsUpToDate = false;
      }

      // Revit Extensible Storage constructor
      public Provider(Entity entity)
      {
         RvtEntity = entity;
         Name = RvtEntity.Get<string>(NameField);
         Icon = RvtEntity.Get<string>(IconField);
         RegisterUrl = RvtEntity.Get<string>(RegisterUrlField);
         AuthorizeUrl = RvtEntity.Get<string>(AuthorizeUrlField);
         TokenUrl = RvtEntity.Get<string>(TokenUrlField);
         ClientId = RvtEntity.Get<string>(ClientIdField);
         ClientSecret = RvtEntity.Get<string>(ClientSecretField);
         IsUpToDate = true;
      }

      public Provider(string name, string icon, JsonOauth oauth)
      {
         this.RvtEntity = null;
         this.Name = name;
         this.Icon = icon;
         if (oauth != null)
         {
            this.RegisterUrl = oauth.RegisterUrl;
            this.AuthorizeUrl = oauth.AuthorizationUrl;
            this.TokenUrl = oauth.TokenUrl;
         }
         else
         {
            this.RegisterUrl = "";
            this.AuthorizeUrl = "";
            this.TokenUrl = "";
         }
         this.ClientId = "";
         this.ClientSecret = "";
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
         RvtEntity.Set<string>(NameField, Name);
         RvtEntity.Set<string>(IconField, Icon);
         RvtEntity.Set<string>(RegisterUrlField, RegisterUrl);
         RvtEntity.Set<string>(AuthorizeUrlField, AuthorizeUrl);
         RvtEntity.Set<string>(TokenUrlField, TokenUrl);
         RvtEntity.Set<string>(ClientIdField, ClientId);
         RvtEntity.Set<string>(ClientSecretField, ClientSecret);

         // Set the entity to be up-to-date and return it
         IsUpToDate = true;
         return RvtEntity;
      }


      public void RegisterForDocument(BimbotDocument document)
      {
         byte[] data = Encoding.ASCII.GetBytes("{\n" +
            "\"type\": \"pull\",\n" +
            "\"client_name\": \"Revit file " + document.Name + "\",\n" +
            "\"client_url\": \"-no url-\",\n" +
            "\"client_description\": \"A project file opened in Revit\",\n" +
            "\"client_icon\": \"https://www.itannex.com/wp-content/uploads/2016/12/revit-icon-128px-hd.png \",\n" +
            "\"redirect_url\": \"\"\n" +
            "}");

         HttpWebRequest request = (HttpWebRequest)WebRequest.Create(RegisterUrl);
         request.Method = "POST";
         request.ContentType = "application/json";
         request.ContentLength = data.Length;
         try
         {
            using (Stream requestStream = request.GetRequestStream())
            {
               requestStream.Write(data, 0, data.Length);
            }

            WebResponse response = request.GetResponse() as HttpWebResponse;
            Debug.Assert(response != null);
            using (Stream responseStream = response.GetResponseStream())
            {
               StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
               string str = reader.ReadToEnd();
               OAuthRegisterResponse regData = JsonConvert.DeserializeObject<OAuthRegisterResponse>(str);

               ClientId = regData.client_id;
               ClientSecret = regData.client_secret;

               document.RegisteredProviders.Add(Name, this);
            }
         }
         catch (Exception ex)
         {
            Console.Write(ex);
         }
      }


      public string GetToken(string code)
      {
         return "";
      }
   }
}