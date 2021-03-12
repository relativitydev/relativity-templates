using kCura.EventHandler;
using Relativity.API;
using Relativity.Services.Objects;
using System;
using System.Net;

namespace $rootnamespace$
{
	[kCura.EventHandler.CustomAttributes.Description("Page Interaction EventHandler")]
	[System.Runtime.InteropServices.Guid("$guid1$")]
	public class PageInteractionEventhandler : kCura.EventHandler.$safeitemname$
	{
		public override Response PopulateScriptBlocks()
		{
			// Update Security Protocol
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

			//Create a response object with default values
			kCura.EventHandler.Response retVal = new kCura.EventHandler.Response();
			retVal.Success = true;
			retVal.Message = string.Empty;

			Int32 currentWorkspaceArtifactID = Helper.GetActiveCaseID();

			//The Object Manager is the newest and preferred way to interact with Relativity instead of the Relativity Services API(RSAPI).
			using (IObjectManager objectManager = this.Helper.GetServicesManager().CreateProxy<IObjectManager>(ExecutionIdentity.System))
			{

			}

			//Register Javascript functions directly.
			String alertFunction = "<script type=\"text/javascript\"> function alertWithText(text){alert(text);}</script>";
			this.RegisterClientScriptBlock(new kCura.EventHandler.ScriptBlock() { Key = "alertFunc", Script = alertFunction });

			//Can also call functions directly
			String alert = "<script type=\"text/javascript\"> alertWithText('Successfully called a function registered earlier!');</script>";
			this.RegisterStartupScriptBlock(new kCura.EventHandler.ScriptBlock() { Key = "alertKey", Script = alert });

			//Load Javascript and CSS from an existing Relativity Custom Page

			//Retrieve the URL to the custom page where your external files are stored.  Use your custom page application's GUID.
			Guid applicationGuid = new Guid("439e0ca3-cfbf-4940-a868-c9cd70d0368d");
			string applicationPath = this.Helper.GetUrlHelper().GetRelativePathToCustomPages(applicationGuid);

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
	}
}