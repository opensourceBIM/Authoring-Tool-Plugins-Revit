using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Bimbot.Bcf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Security.Cryptography;

namespace Bimbot.Objects
{
   public class Service : INotifyPropertyChanged
   {
      #region Static members and functions
      private const int ZipLeadBytes = 0x04034b50;
      private const ushort GZipLeadBytes = 0x8b1f;
      private const string KalkZandsteenResponse = "F:\\RevitBimbot\\result_final.bcf";
      private const string NameField = "Name";
      private const string DescriptionField = "Description";
      private const string InputsField = "Inputs";
      private const string OutputsField = "Outputs";
      private const string UrlField = "Url";
      private const string AuthorizationField = "Code";
      //      private const string SoidField = "Soid";
      private const string ProviderField = "Provider";
      private const string TriggersField = "Triggers";
      private const string ResultTypeField = "ResultType";
      private const string ResultDataField = "ResultData";
      private const string LastRunField = "LastRun";
      private static readonly byte[] SALT = new byte[] { 0x25, 0xdc, 0x4a, 0x11, 0xda, 0x1d, 0x7b, 0x77, 0x5c, 0x2e, 0x7f, 0xfa, 0x5d, 0x18, 0x5a, 0xc3 };

      private static bool IsCompressedData(byte[] data)
      {
         Debug.Assert(data.Length >= 4);

         return BitConverter.ToInt32(data, 0) == ZipLeadBytes ||
                BitConverter.ToUInt16(data, 0) == GZipLeadBytes;
      }


      public static string Encrypt(string plain, string password)
      {
         if (password == null)
            return plain;

         //convert password to key
         Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, SALT);
         byte[] encrypted;
         byte[] iv;

         //Create instance of the rijndael encryption algorithm        
         using (Rijndael rijndael = Rijndael.Create())
         {
            rijndael.Key = pdb.GetBytes(32);
            rijndael.GenerateIV();
            iv = rijndael.IV;

            using (MemoryStream memoryStream = new MemoryStream())
            {
               using (CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndael.CreateEncryptor(), CryptoStreamMode.Write))
               {
                  using (StreamWriter swEncrypt = new StreamWriter(cryptoStream))
                  {
                     swEncrypt.Write(plain);
                  }
                  encrypted = memoryStream.ToArray();
               }
            }            
         }
         return Convert.ToBase64String(iv.Concat(encrypted).ToArray());
      }

      public static string Decrypt(string encrypted, string password)
      {
         //Convert the string to bytes
         byte[] cypher = Convert.FromBase64String(encrypted);       

         //convert password to key
         Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, SALT);
         string decrypted;

         //Create instance of the rijndael encryption algorithm        
         using (Rijndael rijndael = Rijndael.Create())
         {
            rijndael.Key = pdb.GetBytes(32);
            rijndael.IV = cypher.Take(16).ToArray();

            using (MemoryStream memoryStream = new MemoryStream())
            {
               using (CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndael.CreateDecryptor(), CryptoStreamMode.Write))
               {
                  using (StreamWriter swDecrypt = new StreamWriter(cryptoStream))
                  {
                     swDecrypt.Write(cypher.Skip(16).ToArray());
                  }
                  decrypted = Convert.ToBase64String(memoryStream.ToArray());
               }
            }
         }
         return decrypted;
      }

      private static Schema RvtSchema;
      private static readonly Guid RvtGuid = new Guid("a54b4c89-0ee7-4fae-8fbd-b13b04f01a01"); //b13b04 = bimbot, f01 = service data, a01 = version 0.1

      public static Guid FindOrCreateSchema(Document doc)
      {
         // find schema
         RvtSchema = Schema.Lookup(RvtGuid);

         try
         {
            // Find or read subschemas needed
            Guid provGuid = Provider.FindOrCreateSchema(doc);
            
            // create schema if not found
            if (RvtSchema == null)
            {
               // Start transaction
               Transaction createSchema = new Transaction(doc, "Create Service Schema");
               createSchema.Start();

               // Build schema
               SchemaBuilder serviceBldr = new SchemaBuilder(RvtGuid);

               // Set read and write access attributes and a name
               serviceBldr.SetReadAccessLevel(AccessLevel.Application);
               serviceBldr.SetWriteAccessLevel(AccessLevel.Application);
               serviceBldr.SetApplicationGUID(RevitBimbot.ApplicationGuid);
               serviceBldr.SetVendorId(RevitBimbot.VendorId);
               serviceBldr.SetSchemaName("BimBotService");

               // create fields for relevant attributes of services
               FieldBuilder fieldBldr = serviceBldr.AddSimpleField(NameField, typeof(string));
               fieldBldr.SetDocumentation("Name of the service.");
               fieldBldr = serviceBldr.AddSimpleField(DescriptionField, typeof(string));
               fieldBldr.SetDocumentation("Decription of the service.");
               fieldBldr = serviceBldr.AddArrayField(InputsField, typeof(string));
               fieldBldr.SetDocumentation("List of input types of the service.");
               fieldBldr = serviceBldr.AddArrayField(OutputsField, typeof(string));
               fieldBldr.SetDocumentation("List of output types the service.");
               fieldBldr = serviceBldr.AddSimpleField(UrlField, typeof(string));
               fieldBldr.SetDocumentation("Url of the service.");
               fieldBldr = serviceBldr.AddArrayField(TriggersField, typeof(string));
               fieldBldr.SetDocumentation("List of triggers that activate the service.");
//               fieldBldr = serviceBldr.AddSimpleField(SoidField, typeof(int));
//               fieldBldr.SetDocumentation("Soid of the service.");
               fieldBldr = serviceBldr.AddSimpleField(ProviderField, typeof(string));
               fieldBldr.SetDocumentation("Name of the Provider of the service.");
               fieldBldr = serviceBldr.AddSimpleField(AuthorizationField, typeof(string));
               fieldBldr.SetDocumentation("Code used for authorizing the service.");

               fieldBldr = serviceBldr.AddSimpleField(LastRunField, typeof(string));
               fieldBldr.SetDocumentation("Last date the service was executed.");
               fieldBldr = serviceBldr.AddSimpleField(ResultTypeField, typeof(string));
               fieldBldr.SetDocumentation("Last result type delivered by the service.");
               fieldBldr = serviceBldr.AddSimpleField(ResultDataField, typeof(string));
               fieldBldr.SetDocumentation("Last results delivered by the service.");
               RvtSchema = serviceBldr.Finish(); // register the subSchema object

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
      #region Service Attributes (JSON and internal)
      public string Name { get; } 
      public string Description { get; } 
      public IList<string> Inputs { get; } 
      public IList<string> Outputs { get; } 
      public string Url { get; private set; }
      #endregion

      #region Service only Attributes (not in JSON)
      public Entity RvtEntity { get; private set; }
      public Provider Provider { get; private set; }
      public int? Soid { get; private set; }
      public string AuthorizationCode { get; private set; }

      public List<RevitEvntTrigger> Triggers { get; }
      public ServiceResult Result { get; private set; }

      private string Password;

      public bool IsUpToDate;
      private bool _isRunning;
      public bool IsRunning
      {
         get { return this._isRunning; }
         set
         {
            if (this._isRunning != value)
            {
               this._isRunning = value;
               this.NotifyPropertyChanged("IsRunning");
            }
         }
      }
     
      public event PropertyChangedEventHandler PropertyChanged;

      public void NotifyPropertyChanged(string propName)
      {
         if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propName));
      }

      //TODO IFC export settings (configuration of the exporter)

      // UI visualization getters (conversion)
      public string Icon
      {
         get
         {
            return Url == null || Provider.Icon == null ? "" :
               (Provider.Icon.StartsWith("http") ? Provider.Icon :
               Url.Substring(0, Url.IndexOf('/', 9)) + Provider.Icon);
         }
      }

      private bool UrlIsValid()
      {
         return Url.StartsWith("http://") || Url.StartsWith("https://");
      }

      public Brush ProtectedColor
      {
         get
         {
            return (UrlIsValid()) ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
         }
      }

      public Brush RunningColor
      {
         get
         {
            return IsRunning ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
         }
      }
      #endregion
      #endregion



      #region constructors
      // Copy constructor
      public Service(Service service)
      {
         this.RvtEntity = null;
         this.Name = service.Name;
         this.Description = service.Description;
         this.Inputs = new List<string>(service.Inputs);
         this.Outputs = new List<string>(service.Outputs);
         this.Url = service.Url; // => TODO change to combination of URL and Soid
//         this.Soid = null;       // => not needed when in URL combination
         this.Provider = service.Provider;
         this.AuthorizationCode = service.AuthorizationCode; 

         this.Triggers = new List<RevitEvntTrigger>();
         this.Result = null;
         this.IsUpToDate = false;
         this.IsRunning = false;
      }

      // JSON constructor
      [JsonConstructor]
      public Service(string name, string description, string provider, string providerIcon, 
                     List<string> inputs, List<string> outputs, JsonOauth oauth, string resourceUrl)
      {
         this.RvtEntity = null;
         this.Name = name;
         this.Description = description;
         this.Inputs = inputs;
         this.Outputs = outputs;
         this.Url = resourceUrl;
//         this.Soid = null;
         this.AuthorizationCode = null;
         this.Provider = new Provider(provider, providerIcon, oauth);
         this.Triggers = new List<RevitEvntTrigger>();
         this.Result = null;

         this.IsUpToDate = false;
         this.IsRunning = false;
      }


      // Revit Extensible Storage constructor
      public Service(Entity serviceEnt, Dictionary<string, Provider> providers)
      {
         this.RvtEntity = serviceEnt;
         this.Name = serviceEnt.Get<string>(NameField);
         this.Description = serviceEnt.Get<string>(DescriptionField);
         this.Inputs = serviceEnt.Get<IList<string>>(InputsField);
         this.Outputs = serviceEnt.Get<IList<string>>(OutputsField);
         this.Url = serviceEnt.Get<string>(UrlField);
         this.AuthorizationCode = serviceEnt.Get<string>(AuthorizationField);
//         this.Soid = serviceEnt.Get<int>(SoidField);
         Debug.Assert(providers.ContainsKey(serviceEnt.Get<string>(ProviderField)));
         this.Provider = providers[serviceEnt.Get<string>(ProviderField)];

         //Read the triggers
         this.Triggers = new List<RevitEvntTrigger>();
         foreach (string trigger in serviceEnt.Get<IList<string>>(TriggersField))
            this.Triggers.Add((RevitEvntTrigger)Enum.Parse(typeof(RevitEvntTrigger), trigger, true));
         
         //Read the result data (and in case of BCF parse it)
         this.Result = new ServiceResult();
         this.Result.data = serviceEnt.Get<string>(ResultDataField);
         if (Result.data != "")
         {
            this.Result.isBcf = serviceEnt.Get<string>(ResultTypeField).Equals("bcf");
            this.Result.lastRun = DateTime.Parse(serviceEnt.Get<string>(LastRunField));
            if (this.Result.isBcf)
               this.Result.bcf = new BcfFile(Convert.FromBase64String(this.Result.data));
         }
         else
            this.Result = null;

         this.IsUpToDate = false;
         this.IsRunning = false;
      }
      #endregion

      public void ReAssignProvider(Dictionary<string, Provider> providers)
      {
         Provider = providers[Provider.Name];
      }

      public void SetAutherizationCode(string code)
      {
         this.AuthorizationCode = code;
         IsUpToDate = false;
      }


      public void SetSoid(int soid)
      {         
         this.Soid = soid;
         IsUpToDate = false;
      }

      public void Protect(string password)
      {
         Password = password;
         IsUpToDate = false;
      }

      public void UnProtect(string password)
      {
         // If the url starts with http it is not encrypted
         if (!UrlIsValid())
            return;

         // If the decoded Url does not start with http the password does not match
         string decUrl = Decrypt(Url, password);
         if (!UrlIsValid())
            return;

         Url = decUrl;
         AuthorizationCode = Decrypt(AuthorizationCode, password);
      }

      public void AddTrigger(RevitEvntTrigger trigger)
      {
         Triggers.Add(trigger);
         IsUpToDate = false;
      }


      private void SetServiceResult(byte[] bytes)
      {
         Result = new ServiceResult();
         Result.lastRun = DateTime.Now;
         Result.isBcf = (bytes.Length >= 4 || IsCompressedData(bytes));
         if (Result.isBcf)
         {
            Result.data = Convert.ToBase64String(bytes);
            Result.bcf = new BcfFile(bytes);
         }
         else
         {
            Result.data = Encoding.UTF8.GetString(bytes);
            Result.bcf = null;
         }
         IsUpToDate = false;
      }


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
         RvtEntity.Set<string>(DescriptionField, Description);
         RvtEntity.Set<IList<string>>(InputsField, Inputs);
         RvtEntity.Set<IList<string>>(OutputsField, Outputs);
         RvtEntity.Set<string>(UrlField, Encrypt(Url, Password)); //only encrypted when Password is not null
         RvtEntity.Set<string>(AuthorizationField, Encrypt(AuthorizationCode, Password)); //only encrypted when Password is not null
//         RvtEntity.Set<int>(SoidField, Soid.Value);
         RvtEntity.Set<string>(ProviderField, Provider.Name);

         // Fill the triggers in the entity
         List<string> triggers = new List<string>();
         foreach (RevitEvntTrigger trigger in Triggers)
            triggers.Add(trigger.ToString());
         RvtEntity.Set<IList<String>>(TriggersField, triggers);

         // Fill the result in the entity
         if (Result != null)
         {
            RvtEntity.Set<string>(ResultTypeField, Result.isBcf ? "bcf" : "txt");
            RvtEntity.Set<string>(ResultDataField, Result.data);
            RvtEntity.Set<string>(LastRunField, Result.lastRun.ToString());
         }

         // Set the entity to be up-to-date and return it
         IsUpToDate = true;
         return RvtEntity;
      }


      private HttpWebRequest CreateSeviceRequest(byte[] data)
      {
         HttpWebRequest myHttpWebRequest = (HttpWebRequest) WebRequest.Create(Url + (Soid == 0 ? "" : "/" + Soid.ToString()));
         myHttpWebRequest.Method = "POST";
         myHttpWebRequest.Headers.Add("Input-Type", "IFC_STEP_2X3TC1");
         myHttpWebRequest.Headers.Add("Token", AuthorizationCode); //TO DO use token instead of AuthCode
         myHttpWebRequest.Headers.Add("Accept-Flow", "SYNC");
         myHttpWebRequest.Timeout = 1800000; //half an hour

         Stream requestStream = myHttpWebRequest.GetRequestStream();
         requestStream.Write(data, 0, data.Length);

         return myHttpWebRequest;
      }
            

      public string Run(byte[] data)
      {
         try
         {
            IsRunning = true;
            if (Soid < 0)
            {
               byte[] bytes = File.ReadAllBytes(KalkZandsteenResponse);
               SetServiceResult(bytes);
               IsRunning = false;
               return Result.data;
            }

            HttpWebRequest request = CreateSeviceRequest(data);
            WebResponse response = request.GetResponse() as HttpWebResponse;
            using (Stream responseStream = response.GetResponseStream())
            {
               byte[] bytes;
               using (MemoryStream ms = new MemoryStream())
               {
                  responseStream.CopyTo(ms);
                  bytes = ms.ToArray();
               }
               SetServiceResult(bytes);
               IsRunning = false;
               return Result.data;
            }
         }
         catch (Exception e)
         {
            MessageBox.Show("Service '" + Name + "' fails due to :\n" + e.Message);
         }
         IsRunning = false;
         return "";
      }

      
      public async Task<string> RunAsync(byte[] data)
      {
         try
         {
            IsRunning = true;
            if (Soid < 0)
            {
               byte[] bytes = File.ReadAllBytes(KalkZandsteenResponse);
               SetServiceResult(bytes);
               IsRunning = false;
               return Result.data;
            }
            else
            {
               HttpWebRequest request = CreateSeviceRequest(data);
               WebResponse response = await request.GetResponseAsync() as HttpWebResponse;
               using (Stream responseStream = response.GetResponseStream())
               {
                  byte[] bytes;
                  using (MemoryStream ms = new MemoryStream())
                  {
                     await responseStream.CopyToAsync(ms);
                     bytes = ms.ToArray();
                  }
                  SetServiceResult(bytes);
                  IsRunning = false;
                  return Result.data;
               }
            }
         }
         catch (Exception e)
         {
            MessageBox.Show("Service '" + Name + "' fails due to :\n" + e.Message);
         }
         IsRunning = false;
         return "";
      }
   }
}
