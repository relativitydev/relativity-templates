using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Relativity.API;
using $saferootprojectname$.Helpers;
using $saferootprojectname$.Agents;

namespace $safeprojectname$
{
	[TestFixture]
	public class ManagerAgentTests
	{
		public Int32 AgentId;
		public Mock<IQuery> MockQuery;
		public Mock<IAgentHelper> MockAgentHelper;
		public Mock<IAPILog> MockLogger;
		private List<Int32> _resourceGroupIdList;

		[SetUp]
		public void Setup()
		{
			AgentId = 1234567;
			MockQuery = new Mock<IQuery>();
			MockAgentHelper = new Mock<IAgentHelper>();
			MockLogger = new Mock<IAPILog>();
			_resourceGroupIdList = new List<int> { 10000, 20000 };
		}

		#region " Dynamic Test Case Data "

		public IEnumerable<TestCaseData> TestCasesForGetCommaDelimitedListOfResourceIds
		{
			get
			{
				Setup();
				yield return new TestCaseData(new List<Int32> { 1000001, 1000002, 1000003 }, "1000001,1000002,1000003");
				yield return new TestCaseData(new List<Int32>(), "");
				yield return new TestCaseData(new List<Int32> { 1000001 }, "1000001");
			}
		}

		#endregion

		#region Tests

		[Description("When a record is picked up by the agent, should complete execution process")]
		[Test]
		public async Task ExecuteAsync_QueueHasARecord_ExecuteAll()
		{
			// Arrange
			WhenQueueReturnsARecord();
			SetUpExpectations("00:00:00", "23:59:59");
		    ManagerJob managerJob = new ManagerJob(AgentId, MockAgentHelper.Object, MockQuery.Object, new DateTime(2016, 01, 25, 01, 00, 00), _resourceGroupIdList, MockLogger.Object);

			// Act
			await managerJob.ExecuteAsync();

			// Assert
			AssertRecordWasProcessed();
		}

		[Description("When no record is picked up by the agent, should not process")]
		[Test]
		public async Task ExecuteAsync_QueueHasNoRecord_DoNotExecute()
		{
			// Arrange
			WhenQueueReturnsNull();
			SetUpExpectations("00:00:00", "23:59:59");
		    ManagerJob managerJob = new ManagerJob(AgentId, MockAgentHelper.Object, MockQuery.Object, new DateTime(2016, 01, 25, 01, 00, 00), _resourceGroupIdList, MockLogger.Object);

			// Act
			await managerJob.ExecuteAsync();

			// Assert
			AssertRecordWasSkipped();
		}

		[Description("When it's not during configured off-hours, record is not processed")]
		[Test]
		public async Task ExecuteAsync_QueueHasARecord_NotDuringOffHours()
		{
			// Arrange
			WhenQueueReturnsARecord();
			SetUpExpectations("00:00:00", "02:00:00");
			ManagerJob managerJob = new ManagerJob(AgentId, MockAgentHelper.Object, MockQuery.Object, new DateTime(2016, 01, 25, 03, 00, 00), _resourceGroupIdList, MockLogger.Object);

			// Act
			await managerJob.ExecuteAsync();

			// Assert
			AssertRecordWasSkipped();
		}

		[TestCaseSource("TestCasesForGetCommaDelimitedListOfResourceIds")]
		[Test]
		[Description("This will test getting a comma delimited list of resource IDs from a list of integers.")]
		public void GetCommaDelimitedListOfResourceIds(IEnumerable<Int32> resourceIdsList, String expectedResult)
		{
			// Arrange
			WhenQueueReturnsARecord();
			SetUpExpectations("00:00:00", "02:00:00");
		    ManagerJob managerJob = new ManagerJob(AgentId, MockAgentHelper.Object, MockQuery.Object, new DateTime(2016, 01, 25, 03, 00, 00), _resourceGroupIdList, MockLogger.Object);

			//Act
			string observedResult = managerJob.GetCommaDelimitedListOfResourceIds(resourceIdsList);

			//Assert
			Assert.AreEqual(expectedResult, observedResult);
		}

		#endregion Tests

		#region Test Helpers

		private void WhenQueueReturnsNull()
		{
			MockQuery.Setup(x => x.RetrieveNextInManagerQueueAsync(It.IsAny<IDBContext>(), It.IsAny<Int32>(), It.IsAny<String>())).ReturnsAsync(null);
		}

		private void WhenQueueReturnsARecord()
		{
			DataTable table = GetManagerTable();
			MockQuery.Setup(x => x.RetrieveNextInManagerQueueAsync(It.IsAny<IDBContext>(), It.IsAny<Int32>(), It.IsAny<String>())).ReturnsAsync(table);
		}

		private void SetUpExpectations(String offHoursStartTime, String offHoursEndTime)
		{
			// Inserts rows in the Worker Queue
			MockQuery.Setup(
				x =>
					x.InsertRowsIntoWorkerQueueAsync(
					It.IsAny<IDBContext>(),
					It.IsAny<Int32>(),
					It.IsAny<Int32>(),
					It.IsAny<Int32>(),
					It.IsAny<Int32>(),
					It.IsAny<Int32>()))
					.Returns(Task.FromResult(false))
					.Verifiable();

			// Deletes the completed job from the Manager Queue
			MockQuery.Setup(
				x =>
					x.RemoveRecordFromTableByIDAsync(
					It.IsAny<IDBContext>(),
					It.IsAny<String>(),
					It.IsAny<Int32>()))
					.Returns(Task.FromResult(false))
					.Verifiable();

			//Returns off-hours from configuration table
			DataTable offHoursDt = GetOffHoursTable(offHoursStartTime, offHoursEndTime);
			MockQuery.Setup(
				x =>
				x.RetrieveOffHoursAsync(
				It.IsAny<IDBContext>()))
				.Returns(Task.FromResult(offHoursDt))
				.Verifiable();
		}

		private void AssertRecordWasSkipped()
		{
			// Insert was never called
			MockQuery.Verify(
				x =>
					x.InsertRowsIntoWorkerQueueAsync(
					It.IsAny<IDBContext>(),
					It.IsAny<Int32>(),
					It.IsAny<Int32>(),
					It.IsAny<Int32>(),
					It.IsAny<Int32>(),
					It.IsAny<Int32>()),
					Times.Never);

			// Remove record was never called
			MockQuery.Verify(
				x =>
					x.RemoveRecordFromTableByIDAsync(
					It.IsAny<IDBContext>(),
					It.IsAny<String>(),
					It.IsAny<Int32>()),
					Times.Never);
		}

		private void AssertRecordWasProcessed()
		{
			// Insert was called once
			MockQuery.Verify(
				x =>
					x.InsertRowsIntoWorkerQueueAsync(
					It.IsAny<IDBContext>(),
					It.IsAny<Int32>(),
					It.IsAny<Int32>(),
					It.IsAny<Int32>(),
					It.IsAny<Int32>(),
					It.IsAny<Int32>()),
					Times.Once);

			// Remove record was called once
			MockQuery.Verify(
				x =>
					x.RemoveRecordFromTableByIDAsync(
					It.IsAny<IDBContext>(),
					It.IsAny<String>(),
					It.IsAny<Int32>()),
					Times.Once);
		}

		private DataTable GetManagerTable()
		{
			DataTable table = new DataTable("Test Manager Table");
			table.Columns.Add("WorkspaceArtifactID", typeof(Int32));
			table.Columns.Add("ID", typeof(Int32));
			table.Columns.Add("ArtifactID", typeof(Int32));
			table.Columns.Add("Priority", typeof(Int32));
			table.Columns.Add("ResourceGroupID", typeof(Int32));
			table.Rows.Add(2345678, 1, 3456789, 3, 1000001);
			return table;
		}

		private DataTable GetOffHoursTable(String startTime, String endTime)
		{
			DataTable table = new DataTable();
			table.Columns.Add("AgentOffHourStartTime", typeof(String));
			table.Columns.Add("AgentOffHourEndTime", typeof(String));
			table.Rows.Add(startTime, endTime);
			return table;
		}

		#endregion Test Helpers

	}
}
