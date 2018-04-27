using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using Moq;

using NUnit.Framework;
using Relativity.API;
using $saferootprojectname$.CustomPages.Models;
using $saferootprojectname$.Helpers;

namespace $safeprojectname$
{
	[TestFixture]
	public class WorkerAgentModelTests
	{

		#region Tests

		[Test]
		public async Task GetAllWorkerRecordsTestAsync()
		{
			//Arrange
			Mock<IQuery> queryMock = new Mock<IQuery>();
			WorkerAgentModel workerAgentModel = new WorkerAgentModel(queryMock.Object);
			Mock<IDBContext> dbContextMock = new Mock<IDBContext>();
			DataTable dt = await DataHelpers.WorkerAgentData.BuildDataTableAsync();

			const Int32 workspaceArtifactId = 12345;
			const String workspaceName = "Test Workspace";

			const Int32 row1Id = 1;
			DateTime row1AddedOn = DateTime.UtcNow.AddTicks(-(DateTime.UtcNow.Ticks % TimeSpan.TicksPerSecond));
			const String row1Status = "Waiting";
			const Int32 row1Priority = 10;
			const Int32 row1RecordsRemaining = 100;
			const Int32 row1ArtifactId = 88888;

			const Int32 row2Id = 2;
			DateTime row2AddedOn = DateTime.UtcNow.AddTicks(-(DateTime.UtcNow.Ticks % TimeSpan.TicksPerSecond));
			const String row2Status = "In Progress";
			const Int32 row2AgentId = 11111;
			const Int32 row2Priority = 5;
			const Int32 row2RecordsRemaining = 200;
			const Int32 row2ArtifactId = 9999999;

			DataRow row1 = await DataHelpers.WorkerAgentData.BuildDataRowAsync(dt, row1Id, row1AddedOn, workspaceArtifactId, workspaceName, row1Status, null, row1Priority, row1RecordsRemaining, row1ArtifactId);
			DataRow row2 = await DataHelpers.WorkerAgentData.BuildDataRowAsync(dt, row2Id, row2AddedOn, workspaceArtifactId, workspaceName, row2Status, row2AgentId, row2Priority, row2RecordsRemaining, row2ArtifactId);
			dt.Rows.Add(row1);
			dt.Rows.Add(row2);
			queryMock.Setup(x => x.RetrieveAllInWorkerQueueAsync(It.IsAny<IDBContext>())).ReturnsAsync(dt);

			//Act
			await workerAgentModel.GetAllAsync(dbContextMock.Object);

			//Assert
			//There are 2 records
			Assert.AreEqual(2, workerAgentModel.Records.Count);

			//The first row is set correctly
			Assert.AreEqual(workspaceArtifactId, workerAgentModel.Records.ElementAt(0).WorkspaceArtifactId);
			Assert.AreEqual(workspaceName, workerAgentModel.Records.ElementAt(0).WorkspaceName);
			Assert.AreEqual(row1Id, workerAgentModel.Records.ElementAt(0).JobId);
			Assert.AreEqual(row1AddedOn, workerAgentModel.Records.ElementAt(0).AddedOn);
			Assert.AreEqual(row1Status, workerAgentModel.Records.ElementAt(0).Status);
			Assert.AreEqual(null, workerAgentModel.Records.ElementAt(0).AgentId);
			Assert.AreEqual(row1Priority, workerAgentModel.Records.ElementAt(0).Priority);
			Assert.AreEqual(row1RecordsRemaining, workerAgentModel.Records.ElementAt(0).RemainingRecordCount);
			Assert.AreEqual(row1ArtifactId, workerAgentModel.Records.ElementAt(0).ParentRecordArtifactId);

			//The second row is set correctly
			Assert.AreEqual(workspaceArtifactId, workerAgentModel.Records.ElementAt(1).WorkspaceArtifactId);
			Assert.AreEqual(workspaceName, workerAgentModel.Records.ElementAt(1).WorkspaceName);
			Assert.AreEqual(row2Id, workerAgentModel.Records.ElementAt(1).JobId);
			Assert.AreEqual(row2AddedOn, workerAgentModel.Records.ElementAt(1).AddedOn);
			Assert.AreEqual(row2Status, workerAgentModel.Records.ElementAt(1).Status);
			Assert.AreEqual(row2AgentId, workerAgentModel.Records.ElementAt(1).AgentId);
			Assert.AreEqual(row2Priority, workerAgentModel.Records.ElementAt(1).Priority);
			Assert.AreEqual(row2RecordsRemaining, workerAgentModel.Records.ElementAt(1).RemainingRecordCount);
			Assert.AreEqual(row2ArtifactId, workerAgentModel.Records.ElementAt(1).ParentRecordArtifactId);
		}

		#endregion

	}
}
