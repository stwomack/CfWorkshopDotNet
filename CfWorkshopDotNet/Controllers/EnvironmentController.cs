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
            CloudFoundryServicesOptions servicesOptions = ProviderConfig.ServicesOptions;
         
            ViewData["ApplicationName"] = applicationOptions.ApplicationName;
            ViewData["ApplicationUris"] = applicationOptions.ApplicationUris == null ? "" : String.Join(", ", applicationOptions.ApplicationUris);
            ViewData["InstanceIndex"] = applicationOptions.InstanceIndex;
            ViewData["InstanceId"] = applicationOptions.InstanceId;

            string serviceList = "";
            foreach (var service in servicesOptions.Services)
            {
                if (serviceList.Length > 0)
                {
                    serviceList += ", ";
                }
                serviceList += service.Name;
            }
            ViewData["BoundServices"] = serviceList;

            return View();
        }

        [HttpGet]
        public void Kill()
        {
            Process.GetCurrentProcess().Kill();
        }
    }
}