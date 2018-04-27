using System;
using System.Data;
using NUnit.Framework;

using $saferootprojectname$.Helpers.Models;

namespace $safeprojectname$
{
	[TestFixture]
	public class WorkerQueueRecordTests
	{
		[Test]
		public void Constructor_ReceivesTable_Initializes()
		{
			// Arrange
			DataTable table = GetTable();

			// Act
			WorkerQueueRecord record = new WorkerQueueRecord(table.Rows[0]);

			// Assert
			Assert.AreEqual(2345678, record.WorkspaceArtifactID);
			Assert.AreEqual(1, record.RecordID);
			Assert.AreEqual(3456789, record.ArtifactID);
			Assert.AreEqual(2, record.Priority);
		}

		[Test]
		public void Constructor_ReceivesNullTable_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => new WorkerQueueRecord(null));
		}

		private DataTable GetTable()
		{
			DataTable table = new DataTable();
			table.Columns.Add("WorkspaceArtifactID", typeof(Int32));
			table.Columns.Add("ID", typeof(Int32));
			table.Columns.Add("ArtifactID", typeof(Int32));
			table.Columns.Add("Priority", typeof(Int32));
			table.Rows.Add(2345678, 1, 3456789, 2);
			return table;
		}
	}
}
