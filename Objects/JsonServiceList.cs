using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;


namespace Bimbot.Objects
{
   public class JsonServiceList
   {
      public IList<Service> Services { get; }
      public JsonServiceList(IList<Service> services)
      {
         this.Services = services;
      }
   }

   public class JsonOauth
   {
      public string RegisterUrl { get; }
      public string AuthorizationUrl { get; }
      public string TokenUrl { get; }

      public JsonOauth(string registerUrl, string authorizationUrl, string tokenUrl)
      {
         this.RegisterUrl = registerUrl;
         this.AuthorizationUrl = authorizationUrl;
         this.TokenUrl = tokenUrl;
      }
   }
}
