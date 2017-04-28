using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kCura.EventHandler;
using kCura.Relativity.Client;
using Relativity.API;

namespace $safeprojectname$
{
	[kCura.EventHandler.CustomAttributes.Description("Page Interaction EventHandler")]
	[System.Runtime.InteropServices.Guid("$guid1$")]
	public class PageInteractionEventhandler : kCura.EventHandler.PageInteractionEventHandler
	{
		public override Response PopulateScriptBlocks()
		{
			//Create a response object with default values
      kCura.EventHandler.Response retVal = new kCura.EventHandler.Response();
      retVal.Success = true;
      retVal.Message = String.Empty;

			/*

			//Let's get the url to our custom pages so we can pull in script/css pages from there
      String applicationPath = getApplicationPath(this.Application.ApplicationUrl);

      // Before the elements are loaded on a page, register the JavaScript file. 
      // You can load a JavaScript file into Relativity via a custom page.
      this.RegisterLinkedClientScript(applicationPath + "javascript/myjavascriptfunctions.js");

      // You can also register functions directly.
      String alertFunction = "<script type=\"text/javascript\"> function alertWithText(text){alert(text);}</script>";
      this.RegisterClientScriptBlock(new kCura.EventHandler.ScriptBlock() { Key = "alertFunc", Script = alertFunction });

      // After the elements are loaded on the page, register the JavaScript.
      this.RegisterLinkedStartupScript(applicationPath + "functionCall.js");

      // You can also call functions directly
      String alert = "<script type=\"text/javascript\"> alertWithText('Successfully called a function registered earlier!');</script>";
      this.RegisterStartupScriptBlock(new kCura.EventHandler.ScriptBlock() { Key = "alertKey", Script = alert });

      // Your custom page can include a .css file for loading into a page.
      this.RegisterLinkedCss(applicationPath + "styles/loadedCSS.css");
			  
			 */
			return retVal;
		}
	}
}
