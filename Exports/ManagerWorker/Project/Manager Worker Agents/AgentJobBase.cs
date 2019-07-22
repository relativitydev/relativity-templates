using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Relativity.API;
using Helpers;

namespace $safeprojectname$
{
	public abstract class AgentJobBase
	{
		public Int32 AgentId { get; set; }
		public IAgentHelper AgentHelper { get; set; }
		public IQuery QueryHelper { get; set; }
		public String QueueTable { get; set; }
		public Int32 WorkspaceArtifactId { get; set; }
		public Int32 RecordId { get; set; }
		public Int32 Priority { get; set; }
		public String OffHoursStartTime { get; set; }
		public String OffHoursEndTime { get; set; }
		public DateTime ProcessedOnDateTime { get; set; }
		public IEnumerable<Int32> AgentResourceGroupIds { get; set; }
		public IAPILog Logger { get; set; }

		public delegate void RaiseMessageEventHandler(object sender, string message);
		public event RaiseMessageEventHandler OnMessage;

		public virtual async Task ResetUnfishedJobsAsync(IDBContext eddsDbContext)
		{
			await QueryHelper.ResetUnfishedJobsAsync(eddsDbContext, AgentId, QueueTable);
		}

		protected virtual void RaiseMessage(String message)
		{
		    RaiseMessageEventHandler handler = OnMessage;
			if (handler != null)
			{
				handler(this, message);
				Logger.LogDebug(String.Format("{0} - {1}", Constant.Names.ApplicationName, message));
			}
		}

		private async Task GetOffHoursTimesAsync()
		{
			DataTable dt = await QueryHelper.RetrieveOffHoursAsync(AgentHelper.GetDBContext(-1));
			if (dt == null || dt.Rows == null || dt.Rows.Count == 0 ||
				String.IsNullOrEmpty(dt.Rows[0]["AgentOffHourStartTime"].ToString()) || String.IsNullOrEmpty(dt.Rows[0]["AgentOffHourEndTime"].ToString()) ||
				dt.Rows[0]["AgentOffHourStartTime"] == null || dt.Rows[0]["AgentOffHourEndTime"] == null)
			{
				throw new Helpers.Exceptions.CustomRelativityAgentException(Constant.Messages.AGENT_OFF_HOURS_NOT_FOUND);
			}
			OffHoursStartTime = dt.Rows[0]["AgentOffHourStartTime"].ToString();
			OffHoursEndTime = dt.Rows[0]["AgentOffHourEndTime"].ToString();
		}

		public async Task<Boolean> IsOffHoursAsync(DateTime? currentTime = null)
		{
			DateTime now = currentTime.GetValueOrDefault(DateTime.Now);
			Boolean isOffHours = false;

			try
			{
				await GetOffHoursTimesAsync();
			    DateTime todayOffHourStart = DateTime.Parse(now.ToString("d") + " " + OffHoursStartTime);
			    DateTime todayOffHourEnd = DateTime.Parse(now.ToString("d") + " " + OffHoursEndTime);

				if (now.Ticks >= todayOffHourStart.Ticks && now.Ticks <= todayOffHourEnd.Ticks)
				{
					isOffHours = true;
				}
			}
			catch (FormatException)
			{
				RaiseMessage(Constant.Messages.AGENT_OFF_HOUR_TIMEFORMAT_INCORRECT);
				throw;
			}

			return isOffHours;
		}

		public String GetCommaDelimitedListOfResourceIds(IEnumerable<Int32> agentResourceGroupIds)
		{
			return String.Join(",", agentResourceGroupIds);
		}

		public abstract Task ExecuteAsync();
	}
}
