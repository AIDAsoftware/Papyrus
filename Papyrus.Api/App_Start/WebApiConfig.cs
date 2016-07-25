using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using Newtonsoft.Json;

namespace Papyrus.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config) {
            var corsConfig = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(corsConfig);

            // Web API routes
            config.MapHttpAttributeRoutes();

            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);

            //No serializar valores nulos
            config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

        }
    }
}
