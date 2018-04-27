using System;
using System.ComponentModel;
using System.Data;
using $saferootprojectname$.Helpers;

namespace $safeprojectname$.Models
{
	public class WorkerQueueRecordModel
	{
		[DisplayName("Priority")]
		public Int32 Priority { get; set; }

		[DisplayName("Workspace Artifact ID")]
		public Int32 WorkspaceArtifactId { get; set; }

		[DisplayName("Job ID")]
		public Int32 JobId { get; set; }

		[DisplayName("Added On")]
		public DateTime AddedOn { get; set; }

		[DisplayName("Workspace Name")]
		public String WorkspaceName { get; set; }

		[DisplayName("Status")]
		public String Status { get; set; }

		[DisplayName("Agent ID")]
		public Int32? AgentId { get; set; }

		[DisplayName("# Records Remaining")]
		public Int32 RemainingRecordCount { get; set; }

		[DisplayName("Parent Record Artifact ID")]
		public Int32 ParentRecordArtifactId { get; set; }

		public IQuery QueryHelper { get; set; }

		public WorkerQueueRecordModel()
		{
			QueryHelper = new Query();
		}

		public WorkerQueueRecordModel(DataRow row, IQuery queryHelper)
		{
			SetPropertiesFromDataRowAsync(row, queryHelper);
		}

		private void SetPropertiesFromDataRowAsync(DataRow row, IQuery queryHelper)
		{
			JobId = row["ID"] != DBNull.Value ? Convert.ToInt32(row["ID"]) : 0;
			AddedOn = row["Added On"] != DBNull.Value ? Convert.ToDateTime(row["Added On"]) : new DateTime();
			WorkspaceArtifactId = row["Workspace Artifact ID"] != DBNull.Value ? Convert.ToInt32(row["Workspace Artifact ID"]) : 0;
			WorkspaceName = row["Workspace Name"] != DBNull.Value ? Convert.ToString(row["Workspace Name"]) : String.Empty;
			Status = row["Status"] != DBNull.Value ? Convert.ToString(row["Status"]) : String.Empty;
			AgentId = (row["Agent Artifact ID"] != DBNull.Value) ? Convert.ToInt32(row["Agent Artifact ID"]) : new Int32?();
			Priority = row["Priority"] != DBNull.Value ? Convert.ToInt32(row["Priority"]) : 0;
			RemainingRecordCount = row["# Records Remaining"] != DBNull.Value ? Convert.ToInt32(row["# Records Remaining"]) : 0;
			ParentRecordArtifactId = row["Parent Record Artifact ID"] != DBNull.Value ? Convert.ToInt32(row["Parent Record Artifact ID"]) : 0;
			QueryHelper = queryHelper;
		}

	}
}
