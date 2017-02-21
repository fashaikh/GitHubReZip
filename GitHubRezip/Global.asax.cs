using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Swashbuckle.Application;

namespace GitHubRezip
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //GlobalConfiguration.Configure(WebApiConfig.Register);
            var config = GlobalConfiguration.Configuration;
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            RegisterRoutes(config);
        }

        private void RegisterRoutes(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute("rezip", "{user}/{project}/{branch}", new { controller = "ReZip", action = "GetZip", authenticated = false }, new { verb = new HttpMethodConstraint(HttpMethod.Get.ToString()) });
        }///{*gitHubZipUrl2}/{*gitHubZipUrl3}/{*gitHubZipUrl4}/{*gitHubZipUrl5}/{*gitHubZipUrl6}/{*gitHubZipUrl7}/{*gitHubZipUrl8}/{*gitHubZipUrl9}/{*gitHubZipUrl10}
    }
}
