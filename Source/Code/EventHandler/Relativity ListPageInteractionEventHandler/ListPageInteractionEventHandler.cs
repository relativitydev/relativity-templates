using kCura.EventHandler;
using Relativity.API;
using Relativity.Services.Objects;
using System;
using System.Net;

namespace Relativity_ListPageInteractionEventHandler
{
	[kCura.EventHandler.CustomAttributes.Description("List Page Interaction EventHandler")]
	[System.Runtime.InteropServices.Guid("C206B189-43A9-4D05-9FF3-6ABABB9DB32D")]
	public class ListPageInteractionEventHandler : kCura.EventHandler.ListPageInteractionEventHandler
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

			IAPILog logger = Helper.GetLoggerFactory().GetLogger();
			logger.LogVerbose("Log information throughout execution.");

			return retVal;
		}

		public override string[] ScriptFileNames => new string[] { "lpieh.js" };

		public override string[] AdditionalHostedFileNames => new string[] { "cat.jpg", "dog.jpg" };
	}
}
