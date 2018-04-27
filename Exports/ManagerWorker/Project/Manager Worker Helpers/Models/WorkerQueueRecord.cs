using System;
using System.Data;

namespace $safeprojectname$.Models
{
	/// <summary>
	/// Represents a single row in the Worker Queue Table 
	/// </summary>
	public class WorkerQueueRecord
	{
		#region Properties

		/// <summary>
		/// The primary key identifier of the table 
		/// </summary>
		public Int32 RecordID;

		/// <summary>
		/// The workspace to be processed 
		/// </summary>
		public Int32 WorkspaceArtifactID;

		/// <summary>
		/// An identifier for any use within the workspace 
		/// </summary>
		public Int32 ArtifactID;

		/// <summary>
		/// The priority of this record against all records in the table 
		/// </summary>
		public Int32 Priority;

		#endregion Properties

		public WorkerQueueRecord(DataRow row)
		{
			if (row == null) { throw new ArgumentNullException("row"); }

			WorkspaceArtifactID = (Int32)row["WorkspaceArtifactID"];
			RecordID = (Int32)row["ID"];
			ArtifactID = (Int32)row["ArtifactID"];
			Priority = (Int32)row["Priority"];
		}
	}
}
