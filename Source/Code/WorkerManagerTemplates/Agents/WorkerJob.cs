using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Relativity.API;
using Helpers;
using Helpers.Models;

namespace Agents
{
	/// <summary>
	/// This class abstracts the agent logic to allow unit testing without IIS dependencies
	/// </summary>
	public class WorkerJob : AgentJobBase
	{
		public String BatchTableName
		{
			get
			{
				return _batchTableName ?? (_batchTableName = "[" + Constant.Names.TablePrefix + Guid.NewGuid() + "_" + AgentId + "]");
			}
		}
		private String _batchTableName;

		public WorkerJob(Int32 agentId, IAgentHelper agentHelper, IQuery queryHelper, DateTime processedOnDateTime, IEnumerable<Int32> resourceGroupIds, IAPILog logger)
		{
			RecordId = 0;
			WorkspaceArtifactId = -1;
			AgentId = agentId;
			AgentHelper = agentHelper;
			QueryHelper = queryHelper;
			QueueTable = Constant.Tables.WorkerQueue;
			ProcessedOnDateTime = processedOnDateTime;
			AgentResourceGroupIds = resourceGroupIds;
			Logger = logger;
		}

		public override async Task ExecuteAsync()
		{
			if (await IsOffHoursAsync(ProcessedOnDateTime))
			{
				//Check for jobs which stopped unexpectedly on this agent thread
				RaiseMessage(String.Format("Resetting records which failed. [Table = {0}]", QueueTable));
				await ResetUnfishedJobsAsync(AgentHelper.GetDBContext(-1));

				//Retrieve the next record to work on
				RaiseMessage(String.Format("Retrieving next record(s) in the queue. [Table = {0}]", QueueTable));
				string delimiitedListOfResourceGroupIds = GetCommaDelimitedListOfResourceIds(AgentResourceGroupIds);
				if (delimiitedListOfResourceGroupIds != String.Empty)
				{
					DataTable next = await RetrieveNextAsync(delimiitedListOfResourceGroupIds);

					if (TableIsNotEmpty(next))
					{
					    WorkerQueueRecord record = new WorkerQueueRecord(next.Rows[0]);
						RaiseMessage(String.Format("Retrieved record(s) in the queue. [Table = {0}, ID = {1}, Workspace Artifact ID = {2}]", QueueTable, RecordId, WorkspaceArtifactId));

						// Sets the workspaceArtifactID and RecordID so the agent will have access to them in case of an exception
						WorkspaceArtifactId = record.WorkspaceArtifactID;
						RecordId = record.RecordID;

						//Process the record(s)
						RaiseMessage(String.Format("Processing record(s). [Table = {0}, ID = {1}, Workspace Artifact ID = {2}]", QueueTable, RecordId, WorkspaceArtifactId));
						await ProcessRecordsAsync(record);
						RaiseMessage(String.Format("Processed record(s). [Table = {0}, ID = {1}, Workspace Artifact ID = {2}]", QueueTable, RecordId, WorkspaceArtifactId));
					}
					else
					{
						RaiseMessage("No records in the queue for this resource pool.");
					}

					//Remove the record from the worker queue
					RaiseMessage(String.Format("Removing record(s) from the queue. [Table = {0}, ID = {1}, Workspace Artifact ID = {2}]", QueueTable, RecordId, WorkspaceArtifactId));
					await FinishAsync();
					RaiseMessage(String.Format("Removed record(s) from the queue. [Table = {0}, ID = {1}, Workspace Artifact ID = {2}]", QueueTable, RecordId, WorkspaceArtifactId));
				}
				else
				{
					RaiseMessage("This agent server is not part of any resource pools.  Agent execution skipped.");
				}
			}
			else
			{
				RaiseMessage(String.Format("Current time is not between {0} and {1}. Agent execution skipped.", OffHoursStartTime, OffHoursEndTime));
			}
		}

		private Boolean TableIsNotEmpty(DataTable table)
		{
			return (table != null && table.Rows.Count > 0);
		}

		public async Task<DataTable> RetrieveNextAsync(String delimiitedListOfResourceGroupIds)
		{
			DataTable next = await QueryHelper.RetrieveNextBatchInWorkerQueueAsync(AgentHelper.GetDBContext(-1), AgentId, Constant.Sizes.BatchSize, BatchTableName, delimiitedListOfResourceGroupIds);
			return next;
		}

		public async Task ProcessRecordsAsync(WorkerQueueRecord record)
		{
			await Task.Run(() =>
			{
				//logic goes here
			});
		}

		public async Task FinishAsync()
		{
			await QueryHelper.RemoveBatchFromQueueAsync(AgentHelper.GetDBContext(-1), BatchTableName);
			await QueryHelper.DropTableAsync(AgentHelper.GetDBContext(-1), BatchTableName);
		}
	}
}
