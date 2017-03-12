using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Microsoft.ApplicationInsights.Extensibility;
using Swashbuckle.Application;

namespace GitHubRezip
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            InitApplicationInsight();
            //GlobalConfiguration.Configure(WebApiConfig.Register);
            var config = GlobalConfiguration.Configuration;
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            RegisterRoutes(config);
        }
        private void InitApplicationInsight()
        {
            var key = Environment.GetEnvironmentVariable("githubrezipAIKey");
            if (!string.IsNullOrEmpty(key))
            {
                TelemetryConfiguration.Active.InstrumentationKey = key;
                TelemetryConfiguration.Active.DisableTelemetry = false;
            }
            else
            {
                TelemetryConfiguration.Active.DisableTelemetry = true;
            }
        }
        private void RegisterRoutes(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute("rezip", "{user}/{project}/{branch}", new { controller = "ReZip", action = "GetZip", authenticated = false }, new { verb = new HttpMethodConstraint(HttpMethod.Get.ToString()) });
        }
    }
}
