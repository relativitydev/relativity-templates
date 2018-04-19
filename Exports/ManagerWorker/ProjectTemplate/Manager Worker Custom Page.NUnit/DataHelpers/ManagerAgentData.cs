using System;
using System.Data;
using System.Threading.Tasks;

namespace Relativity_Extension.$safeprojectname$.DataHelpers
{
	public class ManagerAgentData
	{
		public static async Task<DataTable> BuildDataTableAsync()
		{
			return await Task.Run(() =>
			{
				DataTable dt = new DataTable();
				dt.Columns.Add(new DataColumn("ID"));
				dt.Columns.Add(new DataColumn("Added On"));
				dt.Columns.Add(new DataColumn("Workspace Artifact ID"));
				dt.Columns.Add(new DataColumn("Workspace Name"));
				dt.Columns.Add(new DataColumn("Status"));
				dt.Columns.Add(new DataColumn("Agent Artifact ID"));
				dt.Columns.Add(new DataColumn("Priority"));
				dt.Columns.Add(new DataColumn("Added By"));
				dt.Columns.Add(new DataColumn("Record Artifact ID"));

				return dt;
			});
		}

		public static async Task<DataRow> BuildDataRowAsync(DataTable dt, Int32 id, DateTime addedOn, Int32 workspaceArtifactId, String workspaceName,
			String status, Int32? agentArtifactId, Int32? priority, String addedBy, Int32 recordArtifactId)
		{
			return await Task.Run(() =>
			{
				DataRow newRow = dt.NewRow();
				newRow["ID"] = id;
				newRow["Added On"] = addedOn;
				newRow["Workspace Artifact ID"] = workspaceArtifactId;
				newRow["Workspace Name"] = workspaceName;
				newRow["Status"] = status;

				if (agentArtifactId.HasValue)
				{
					newRow["Agent Artifact ID"] = agentArtifactId;
				}
				newRow["Priority"] = priority;
				newRow["Added By"] = addedBy;
				newRow["Record Artifact ID"] = recordArtifactId;

				return newRow;
			});
		}

		public static async Task<DataRow> BuildEmptyDataRowAsync(DataTable dt)
		{
			return await Task.Run(() =>
			{
				DataRow newRow = dt.NewRow();
				newRow["ID"] = DBNull.Value;
				newRow["Added On"] = DBNull.Value;
				newRow["Workspace Artifact ID"] = DBNull.Value;
				newRow["Workspace Name"] = DBNull.Value;
				newRow["Status"] = DBNull.Value;
				newRow["Agent Artifact ID"] = DBNull.Value;
				newRow["Priority"] = DBNull.Value;
				newRow["Added By"] = DBNull.Value;
				newRow["Record Artifact ID"] = DBNull.Value;

				return newRow;
			});
		}
	}
}
