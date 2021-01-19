using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Relativity.API;
using Relativity.CustomPages;
using Relativity.Services.Objects;
using RIP.Provider;

namespace RIP.Web.Controllers
{
	public class MyCustomProviderController : Controller
	{
		// GET: Provider
		public ActionResult Index()
		{
			// Example demonstrates the creation of the Object Manager client and use of the logger
			IAPILog logger = ConnectionHelper.Helper().GetLoggerFactory().GetLogger();
			try
			{
				using (IObjectManager objectManager = ConnectionHelper.Helper().GetServicesManager()
					.CreateProxy<IObjectManager>(ExecutionIdentity.System))
				{

				}
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "An error occurred while loaded the custom page");
			}

			return View();
		}

		[HttpPost]
		public ActionResult GetViewFields()
		{
			/*
			 *  The JSON model from Script/myCustomProvider.js is retrieved from the input stream.  Here you have the option
			 *  to de-serialize the model and add items to the returned KeyValuePair, everything added will be displayed on the Job Status Page
			 */
			IAPILog logger = ConnectionHelper.Helper().GetLoggerFactory().GetLogger();
			List<KeyValuePair<string, string>> settings = new List<KeyValuePair<string, string>>();
			try
			{
				if (Request.InputStream != null && Request.InputStream.Length > 0)
				{
					string data = new StreamReader(Request.InputStream).ReadToEnd();
					JToken token = JToken.Parse(data);
					ExampleConfigurationModel jobConfiguration = JsonConvert.DeserializeObject<ExampleConfigurationModel>(token.ToString());
					settings.Add(new KeyValuePair<string, string>("Job Setting 1", jobConfiguration.ConfigSetting1));
					settings.Add(new KeyValuePair<string, string>("Job Setting 2", jobConfiguration.ConfigSetting2));
					settings.Add(new KeyValuePair<string, string>("Job Setting 3", jobConfiguration.ConfigSetting3));
					settings.Add(new KeyValuePair<string, string>("Example Static Value", "Example, Example, Example"));
				}
					;
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Unable to parse JSON Input");
			}

			return Json(settings);
		}
	}
}