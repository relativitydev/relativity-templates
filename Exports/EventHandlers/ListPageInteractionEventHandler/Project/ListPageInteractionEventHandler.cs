using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using kCura.EventHandler;
using kCura.Relativity.Client;
using Relativity.API;
using Relativity.Services.Objects;

namespace $safeprojectname$
{
	[kCura.EventHandler.CustomAttributes.Description("Page Interaction EventHandler")]
	[System.Runtime.InteropServices.Guid("$guid1$")]
	public class $safeitemname$ : kCura.EventHandler.ListPageInteractionEventHandler
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

			IAPILog logger = Helper.GetLoggerFactory().GetLogger();
			logger.LogVerbose("Log information throughout execution.");

			return retVal;
		}

		public override string[] ScriptFileNames => new string[] { "lpieh.js" };

		public override string[] AdditionalHostedFileNames => new string[] { "cat.jpg", "dog.jpg" };
	}
}
