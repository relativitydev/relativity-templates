using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kCura.Agent;
using kCura.Relativity.Client;
using Relativity.API;

namespace $rootnamespace$
{
	[kCura.Agent.CustomAttributes.Name("Agent")]
	[System.Runtime.InteropServices.Guid("$guid1$")]
	public class $safeitemname$ : kCura.Agent.AgentBase
	{
		public override void Execute()
		{
			//Get the current Agent artifactID
			Int32 agentArtifactID = this.AgentID;
			//Get a dbContext for the EDDS database
      Relativity.API.IDBContext eddsDBContext = this.Helper.GetDBContext(-1);
			
			try
			{

				/*
				
        //Setting up an RSAPI Client
        using (IRSAPIClient proxy =
					Helper.GetServicesManager().CreateProxy<IRSAPIClient>(ExecutionIdentity.System))
        {
          //Add code for working with RSAPIClient
        }

        */

			}
			catch (System.Exception ex)
			{
				//Your Agent caught an exception
				this.RaiseError(ex.Message, ex.Message);
			}
		}
		
		/**
		 * Returns the name of agent
		 */
		public override string Name
		{
			get
			{
				return "Agent";
			}
		}
	}
}
