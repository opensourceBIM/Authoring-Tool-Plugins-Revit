using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace Bimbot.Objects
{
   public class JsonProviderList
   {
      public JsonProvider[] active;
   }


   public class JsonProvider
   {

      public string Name { get; set; }        // fieldBldr = serviceBldr.AddSimpleField("hostName", typeof(string));
      public string Description { get; set; } // fieldBldr = serviceBldr.AddSimpleField("hostDesc", typeof(string));
      public string ListUrl { get; set; }     // fieldBldr = serviceBldr.AddSimpleField("hostUrl", typeof(string));


      public JsonProvider(string name, string description, string listUrl)
      {
         this.Name = name;
         this.Description = description;
         this.ListUrl = listUrl;
      }

      public JsonServiceList GetJsonServices()
      {
         try
         {
            // Get the list of services of the provider
            string resstr = null;
            // Create a get request
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(ListUrl);
            myHttpWebRequest.Method = "GET";

            // Get the response of the request
            WebResponse response = myHttpWebRequest.GetResponse();
            Debug.Assert(response != null);
            using (Stream responseStream = response.GetResponseStream())
            {
               StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
               resstr = reader.ReadToEnd();
            }

            if (resstr != null && !resstr.Equals(""))
            {
               // Create a list service objects from the JSON response
               return JsonConvert.DeserializeObject<JsonServiceList>(resstr);
            }
         }
         catch (Exception e)
         {
            Console.Write(e);
         }
         return null;
      }
   }
}
