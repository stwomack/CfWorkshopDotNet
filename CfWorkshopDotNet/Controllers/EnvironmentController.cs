using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Web.Mvc;
using Microsoft.Extensions.Logging;
using Steeltoe.Extensions.Configuration.CloudFoundry;

namespace CfWorkshopDotNet.Controllers
{
    public class EnvironmentController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            CloudFoundryApplicationOptions applicationOptions = ProviderConfig.ApplicationOptions;
            Console.WriteLine(applicationOptions.ToString());
         
            ViewData["ApplicationName"] = applicationOptions.ApplicationName;
            ViewData["ApplicationUris"] = applicationOptions.ApplicationUris == null ? "" : applicationOptions.ApplicationUris[0];
            ViewData["InstanceIndex"] = applicationOptions.InstanceIndex;
            ViewData["InstanceId"] = applicationOptions.InstanceId;
            ViewData["BoundServices"] = "please fix me";
            return View();
        }

        [HttpGet]
        public void Kill()
        {
            Process.GetCurrentProcess().Kill();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}