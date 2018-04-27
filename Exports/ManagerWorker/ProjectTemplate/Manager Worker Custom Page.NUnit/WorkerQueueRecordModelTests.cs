using System;
using System.Data;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using $saferootprojectname$.CustomPages.Models;
using $saferootprojectname$.Helpers;

namespace $safeprojectname$
{
	[TestFixture]
	class WorkerQueueRecordModelTests
	{
		#region Tests

		[Test]
		public async Task NewWorkerQueueRecordTest_NoAgentIdAsync()
		{
			//Arrange
			Mock<IQuery> queryMock = new Mock<IQuery>();
			DataTable dt = await DataHelpers.WorkerAgentData.BuildDataTableAsync();

			const Int32 workspaceArtifactId = 12345;
			const String workspaceName = "Test Workspace";

			const Int32 row1Id = 1;
			DateTime row1AddedOn = DateTime.UtcNow.AddTicks(-(DateTime.UtcNow.Ticks % TimeSpan.TicksPerSecond));
			const String row1Status = "Waiting";
			const Int32 row1Priority = 10;
			const Int32 row1RecordsRemaining = 25;
			const Int32 row1ArtifactId = 88888;

			DataRow dataRow = await DataHelpers.WorkerAgentData.BuildDataRowAsync(dt, row1Id, row1AddedOn, workspaceArtifactId, workspaceName, row1Status, null, row1Priority, row1RecordsRemaining, row1ArtifactId);

			//Act 
			WorkerQueueRecordModel record = new WorkerQueueRecordModel(dataRow, queryMock.Object);

			//Assert
			Assert.AreEqual(row1Id, record.JobId);
			Assert.AreEqual(row1AddedOn, record.AddedOn);
			Assert.AreEqual(workspaceArtifactId, record.WorkspaceArtifactId);
			Assert.AreEqual(workspaceName, record.WorkspaceName);
			Assert.AreEqual(row1Status, record.Status);
			Assert.AreEqual(null, record.AgentId);
			Assert.AreEqual(row1Priority, record.Priority);
			Assert.AreEqual(row1RecordsRemaining, record.RemainingRecordCount);
			Assert.AreEqual(row1ArtifactId, record.ParentRecordArtifactId);
		}

		[Test]
		public async Task NewWorkerQueueRecordTest_AgentIdAsync()
		{
			//Arrange
			Mock<IQuery> queryMock = new Mock<IQuery>();
			DataTable dt = await DataHelpers.WorkerAgentData.BuildDataTableAsync();

			const Int32 workspaceArtifactId = 12345;
			const String workspaceName = "Test Workspace";

			const Int32 row1Id = 1;
			DateTime row1AddedOn = DateTime.UtcNow.AddTicks(-(DateTime.UtcNow.Ticks % TimeSpan.TicksPerSecond));
			const String row1Status = "Waiting";
			const Int32 row1Priority = 10;
			const Int32 row1RecordsRemaining = 25;
			const Int32 row1ArtifactId = 88888;
			const Int32 row1AgentId = 99999;

			DataRow dataRow = await DataHelpers.WorkerAgentData.BuildDataRowAsync(dt, row1Id, row1AddedOn, workspaceArtifactId, workspaceName, row1Status, row1AgentId, row1Priority, row1RecordsRemaining, row1ArtifactId);

			//Act 
			WorkerQueueRecordModel record = new WorkerQueueRecordModel(dataRow, queryMock.Object);

			//Assert
			Assert.AreEqual(row1Id, record.JobId);
			Assert.AreEqual(row1AddedOn, record.AddedOn);
			Assert.AreEqual(workspaceArtifactId, record.WorkspaceArtifactId);
			Assert.AreEqual(workspaceName, record.WorkspaceName);
			Assert.AreEqual(row1Status, record.Status);
			Assert.AreEqual(row1AgentId, record.AgentId);
			Assert.AreEqual(row1Priority, record.Priority);
			Assert.AreEqual(row1RecordsRemaining, record.RemainingRecordCount);
			Assert.AreEqual(row1ArtifactId, record.ParentRecordArtifactId);
		}

		[Test]
		public async Task NewWorkerQueueRecordTest_NoValuesAsync()
		{
			//Arrange
			Mock<IQuery> queryMock = new Mock<IQuery>();
			DataTable dt = await DataHelpers.WorkerAgentData.BuildDataTableAsync();
			DataRow dataRow = await DataHelpers.WorkerAgentData.BuildEmptyDataRowAsync(dt);

			//Act 
			WorkerQueueRecordModel record = new WorkerQueueRecordModel(dataRow, queryMock.Object);

			//Assert
			Assert.AreEqual(0, record.JobId);
			Assert.AreEqual(new DateTime(), record.AddedOn);
			Assert.AreEqual(0, record.WorkspaceArtifactId);
			Assert.AreEqual(String.Empty, record.WorkspaceName);
			Assert.AreEqual(String.Empty, record.Status);
			Assert.AreEqual(null, record.AgentId);
			Assert.AreEqual(0, record.Priority);
			Assert.AreEqual(0, record.RemainingRecordCount);
			Assert.AreEqual(0, record.ParentRecordArtifactId);
		}

		#endregion
	}
}
