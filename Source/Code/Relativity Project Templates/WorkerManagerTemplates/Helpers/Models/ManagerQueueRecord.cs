using System;
using System.Data;

namespace Relativity_Extension.Helpers.Models
{
	/// <summary>
	/// Represents a single row in the Manager Queue Table 
	/// </summary>
	public class ManagerQueueRecord
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

		/// <summary>
		/// The identifier of the resource group that the workspace belongs to
		/// </summary>
		public Int32 ResourceGroupID;

		#endregion Properties

		public ManagerQueueRecord(DataRow row)
		{
			if (row == null) { throw new ArgumentNullException("row"); }

			WorkspaceArtifactID = (Int32)row["WorkspaceArtifactID"];
			RecordID = (Int32)row["ID"];
			ArtifactID = (Int32)row["ArtifactID"];
			Priority = (Int32)row["Priority"];
			ResourceGroupID = (Int32)row["ResourceGroupID"];
		}
	}
}
