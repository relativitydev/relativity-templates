using System;
using System.Net;
using System.Runtime.InteropServices;
using kCura.EventHandler;
using kCura.EventHandler.CustomAttributes;
using kCura.Relativity.Client;
using Relativity.API;
using Relativity.Services.Objects;

namespace Relativity.PageInteractionEventhandler
{
	[kCura.EventHandler.CustomAttributes.Description("Page Interaction EventHandler")]
	[System.Runtime.InteropServices.Guid("2a23d7a1-6d3e-458b-a9f4-36e2e155bf5a")]
	public class PageInteractionEventhandler : kCura.EventHandler.PageInteractionEventHandler
	{
		public override Response PopulateScriptBlocks()
		{
			// Update Security Protocol
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

			//Create a response object with default values
			kCura.EventHandler.Response retVal = new kCura.EventHandler.Response();
			retVal.Success = true;
			retVal.Message = string.Empty;

			Int32 currentWorkspaceArtifactID = Helper.GetActiveCaseID();

			//The Object Manager is the newest and preferred way to interact with Relativity instead of the Relativity Services API(RSAPI). 
			//The RSAPI will be scheduled for depreciation after the Object Manager reaches feature party with it.
			using (IObjectManager objectManager = this.Helper.GetServicesManager().CreateProxy<IObjectManager>(ExecutionIdentity.System))
			{

			}

			//Setting up an RSAPI Client
			using (IRSAPIClient proxy =
					Helper.GetServicesManager().CreateProxy<IRSAPIClient>(ExecutionIdentity.System))
			{
				//Set the proxy to use the current workspace
				proxy.APIOptions.WorkspaceID = currentWorkspaceArtifactID;
				//Add code for working with RSAPIClient
			}

			//Register Javascript functions directly.
			String alertFunction = "<script type=\"text/javascript\"> function alertWithText(text){alert(text);}</script>";
			this.RegisterClientScriptBlock(new kCura.EventHandler.ScriptBlock() { Key = "alertFunc", Script = alertFunction });

			//Can also call functions directly
			String alert = "<script type=\"text/javascript\"> alertWithText('Successfully called a function registered earlier!');</script>";
			this.RegisterStartupScriptBlock(new kCura.EventHandler.ScriptBlock() { Key = "alertKey", Script = alert });

			//Load Javascript and CSS from an existing Relativity Custom Page

			//Let's get the url to our custom pages so we can pull in script/css pages from there
			String applicationPath = getApplicationPath(this.Application.ApplicationUrl);

			// Before the elements are loaded on a page, register the JavaScript file. 
			// You can load a JavaScript file into Relativity via a custom page.
			this.RegisterLinkedClientScript(applicationPath + "javascript/myjavascriptfunctions.js");

			// After the elements are loaded on the page, register the JavaScript.
			this.RegisterLinkedStartupScript(applicationPath + "functionCall.js");

			// Your custom page can include a .css file for loading into a page.
			this.RegisterLinkedCss(applicationPath + "styles/loadedCSS.css");

			IAPILog logger = Helper.GetLoggerFactory().GetLogger();
			logger.LogVerbose("Log information throughout execution.");

			return retVal;
		}

		private string getApplicationPath(string currentURL)
		{
			string retVal = "";
			string[] split = currentURL.Substring(0, currentURL.IndexOf("/Case")).Split('/');
			retVal = "/" + split[split.Length - 1] + "/CustomPages/45A52DF1-41E1-4E71-8119-35C5AA014E62/";

			return retVal;
		}
	}
}