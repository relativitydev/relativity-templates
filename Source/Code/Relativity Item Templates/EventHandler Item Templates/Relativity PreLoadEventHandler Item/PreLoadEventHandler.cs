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
	[kCura.EventHandler.CustomAttributes.Description("Pre Load EventHandler")]
	[System.Runtime.InteropServices.Guid("$guid1$")]
	public class $safeitemname$ : kCura.EventHandler.PreLoadEventHandler
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

		/// <summary>
		/// The RequiredFields property tells Relativity that your event handler needs to have access to specific fields that you return in this collection property
		/// regardless if they are on the current layout or not. These fields will be returned in the ActiveArtifact.Fields collection just like other fields that are on
		/// the current layout when the event handler is executed.
		/// </summary>
		public override kCura.EventHandler.FieldCollection RequiredFields
		{
			get
			{
				kCura.EventHandler.FieldCollection retVal = new kCura.EventHandler.FieldCollection();
				return retVal;
			}
		}
	}
}
