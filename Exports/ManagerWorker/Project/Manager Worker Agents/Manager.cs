﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Relativity.API;
using $saferootprojectname$.Helpers;

namespace $safeprojectname$
{
	// Change the name and guid for your application
	[kCura.Agent.CustomAttributes.Name("Manager Agent Template")]
	[System.Runtime.InteropServices.Guid("7E42DCAB-2828-4CAC-8CDF-2DD18F172A6D")]
	public class Manager : kCura.Agent.AgentBase
	{
		private IAPILog Logger;

		public override void Execute()
		{
			ExecuteAsync().Wait();
		}

		public async Task ExecuteAsync()
		{
		    Helpers.IQuery queryHelper = new Query();
			IEnumerable<Int32> resourceGroupIds = GetResourceGroupIDs();
			Logger = Helper.GetLoggerFactory().GetLogger();
		    ManagerJob job = new ManagerJob(AgentID, Helper, queryHelper, DateTime.Now, resourceGroupIds, Logger);
			job.OnMessage += MessageRaised;

			try
			{
				RaiseMessage("Enter Agent", 10);
				await job.ExecuteAsync();
				RaiseMessage("Exit Agent", 10);
			}
			catch (Exception ex)
			{
				//Raise an error on the agents tab and event viewer
				RaiseError(ex.ToString(), ex.ToString());
				Logger.LogError(ex, String.Format("{0} - {1}", Constant.Names.ApplicationName, ex));

				//Add the error to our custom Errors table
				queryHelper.InsertRowIntoErrorLogAsync(Helper.GetDBContext(-1), job.WorkspaceArtifactId, Constant.Tables.ManagerQueue, job.RecordId, job.AgentId, ex.ToString()).Wait();

				//Add the error to the Relativity Errors tab
				//this second try catch is in case we have a problem connecting to the RSAPI
				try
				{
					Helpers.Rsapi.ErrorQueries.WriteError(Helper.GetServicesManager(), ExecutionIdentity.System, job.WorkspaceArtifactId, ex);
				}
				catch (Exception rsapiException)
				{
					RaiseError(rsapiException.ToString(), rsapiException.ToString());
					Logger.LogError(rsapiException, String.Format("{0} - {1}", Constant.Names.ApplicationName, rsapiException));
				}

				//Set the status in the queue to error
				queryHelper.UpdateStatusInManagerQueueAsync(Helper.GetDBContext(-1), Constant.QueueStatus.Error, job.RecordId).Wait();
			}
		}

		public override String Name
		{
			get { return "Manager Agent Template"; }
		}

		private void MessageRaised(Object sender, String message)
		{
			RaiseMessage(message, 10);
		}
	}
}