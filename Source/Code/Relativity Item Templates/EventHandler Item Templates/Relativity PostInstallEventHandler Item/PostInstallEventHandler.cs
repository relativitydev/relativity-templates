using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kCura.EventHandler;
using kCura.Relativity.Client;
using Relativity.API;

namespace $rootnamespace$
{
	[kCura.EventHandler.CustomAttributes.Description("Post Install EventHandler")]
	[System.Runtime.InteropServices.Guid("$guid1$")]
	public class $safeitemname$ : kCura.EventHandler.PostInstallEventHandler
	{
		public override kCura.EventHandler.Response Execute()
		{
			//Construct a response object with default values.
			kCura.EventHandler.Response retVal = new kCura.EventHandler.Response();
			retVal.Success = true;
			retVal.Message = String.Empty;
			try
			{

				/*
        
				Int32 currentWorkspaceArtifactID = this.Helper.GetActiveCaseID();
				//Setting up an RSAPI Client
				using (IRSAPIClient proxy =
					Helper.GetServicesManager().CreateProxy<IRSAPIClient>(ExecutionIdentity.System))
				{
					//Set the proxy to use the current workspace
					proxy.APIOptions.WorkspaceID = currentWorkspaceArtifactID;
					//Add code for working with RSAPIClient
				}

                           
				Relativity.API.IDBContext workspaceContext = this.Helper.GetDBContext(currentWorkspaceArtifactID);
                                  
				//Get a dbContext for the EDDS database
				Relativity.API.IDBContext eddsDBContext = this.Helper.GetDBContext(-1);
				
				 
				//Use version properties to alter your workflow
				if (this.CurrentVersion != null && this.CurrentVersion < new System.Version("2.0.0.0")
				{  
				 
				}
				 
				//Dirty flag indicates that the application has been unlocked since the previous install, thus the validity of the application can't be determined
				if (this.Dirty == true) 
				{
				
				}
				 
        */

			}
			catch (System.Exception ex)
			{
				//Change the response Success property to false to let the user know an error occurred
				retVal.Success = false;
				retVal.Message = ex.ToString();
			}

			return retVal;
		}
	}
}
