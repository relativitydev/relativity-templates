using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Threading.Tasks;
using Relativity_Extension.Helpers;

namespace Relativity_Extension.$safeprojectname$.Models
{
	public class ManagerQueueRecordModel
	{
		[DisplayName("Priority")]
		[Required(ErrorMessage = Constant.Messages.PRIORITY_REQUIRED)]
		public Int32? Priority { get; set; }

		[DisplayName("Workspace Artifact ID")]
		public Int32 WorkspaceArtifactId { get; set; }

		[DisplayName("ID")]
		public Int32 ID { get; set; }

		[DisplayName("Added On")]
		public DateTime AddedOn { get; set; }

		[DisplayName("Workspace Name")]
		public String WorkspaceName { get; set; }

		[DisplayName("Status")]
		public String Status { get; set; }

		[DisplayName("Agent ID")]
		public Int32? AgentId { get; set; }

		[DisplayName("Added By")]
		public String AddedBy { get; set; }

		[DisplayName("Record Artifact ID")]
		[Required(ErrorMessage = Constant.Messages.ARTIFACT_ID_REQUIRED)]
		public Int32? RecordArtifactId { get; set; }

		public IQuery QueryHelper { get; set; }

		public ManagerQueueRecordModel()
		{
			QueryHelper = new Query();
		}

		public ManagerQueueRecordModel(DataRow row, IQuery queryHelper)
		{
			SetPropertiesFromDataRow(row, queryHelper);
		}

		private void SetPropertiesFromDataRow(DataRow row, IQuery queryHelper)
		{
			ID = row["ID"] != DBNull.Value ? Convert.ToInt32(row["ID"]) : 0;
			AddedOn = row["Added On"] != DBNull.Value ? Convert.ToDateTime(row["Added On"]) : new DateTime();
			WorkspaceArtifactId = row["Workspace Artifact ID"] != DBNull.Value ? Convert.ToInt32(row["Workspace Artifact ID"]) : 0;
			WorkspaceName = row["Workspace Name"] != DBNull.Value ? Convert.ToString(row["Workspace Name"]) : String.Empty;
			Status = row["Status"] != DBNull.Value ? Convert.ToString(row["Status"]) : String.Empty;
			AgentId = (row["Agent Artifact ID"] != DBNull.Value) ? Convert.ToInt32(row["Agent Artifact ID"]) : new Int32?();
			Priority = row["Priority"] != DBNull.Value ? Convert.ToInt32(row["Priority"]) : 0;
			AddedBy = row["Added By"] != DBNull.Value ? Convert.ToString(row["Added By"]) : String.Empty;
			RecordArtifactId = row["Record Artifact ID"] != DBNull.Value ? Convert.ToInt32(row["Record Artifact ID"]) : 0;

			QueryHelper = queryHelper;
		}

	}
}
