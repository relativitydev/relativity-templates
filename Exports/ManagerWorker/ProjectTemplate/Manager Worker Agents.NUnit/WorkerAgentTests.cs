using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Relativity.API;
using Relativity_Extension.Helpers;
using Relativity_Extension.Agents;

namespace Relativity_Extension.$safeprojectname$
{
	[TestFixture]
	public class WorkerAgentTests
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
			WorkerJob workerJob = new WorkerJob(AgentId, MockAgentHelper.Object, MockQuery.Object, new DateTime(2016, 01, 25, 01, 00, 00), _resourceGroupIdList, MockLogger.Object);

			// Act
			await workerJob.ExecuteAsync();

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
			WorkerJob workerJob = new WorkerJob(AgentId, MockAgentHelper.Object, MockQuery.Object, new DateTime(2016, 01, 25, 01, 00, 00), _resourceGroupIdList, MockLogger.Object);

			// Act
			await workerJob.ExecuteAsync();

			// Assert
			AssertRecordWasSkippedAndFinishAsyncIsCalled();
		}

		[Description("When it's not during configured off-hours, record is not processed")]
		[Test]
		public async Task ExecuteAsync_QueueHasARecord_NotDuringOffHours()
		{
			// Arrange
			WhenQueueReturnsARecord();
			SetUpExpectations("00:00:00", "02:00:00");
			WorkerJob workerJob = new WorkerJob(AgentId, MockAgentHelper.Object, MockQuery.Object, new DateTime(2016, 01, 25, 03, 00, 00), _resourceGroupIdList, MockLogger.Object);

			// Act
			await workerJob.ExecuteAsync();

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
			WorkerJob workerJob = new WorkerJob(AgentId, MockAgentHelper.Object, MockQuery.Object, new DateTime(2016, 01, 25, 03, 00, 00), _resourceGroupIdList, MockLogger.Object);

			//Act
			string observedResult = workerJob.GetCommaDelimitedListOfResourceIds(resourceIdsList);

			//Assert
			Assert.AreEqual(expectedResult, observedResult);
		}

		#endregion Tests

		#region Test Helpers

		private void WhenQueueReturnsNull()
		{
			MockQuery.Setup(
				x =>
					x.RetrieveNextBatchInWorkerQueueAsync(
					It.IsAny<IDBContext>(),
					It.IsAny<Int32>(),
					It.IsAny<Int32>(),
					It.IsAny<String>(),
					It.IsAny<String>()))
					.ReturnsAsync(null);
		}

		private void WhenQueueReturnsARecord()
		{
			DataTable table = GetWorkerTable();

			MockQuery.Setup(
				x =>
					x.RetrieveNextBatchInWorkerQueueAsync(
					It.IsAny<IDBContext>(),
					It.IsAny<Int32>(),
					It.IsAny<Int32>(),
					It.IsAny<String>(),
					It.IsAny<String>()))
					.ReturnsAsync(table);
		}

		private void SetUpExpectations(String offHoursStartTime, String offHoursEndTime)
		{
			// Removes the batch of work from the Worker Queue
			MockQuery.Setup(
				x =>
					x.RemoveBatchFromQueueAsync(
					It.IsAny<IDBContext>(),
					It.IsAny<String>()))
					.Returns(Task.FromResult(false))
					.Verifiable();

			// Drops the temporary table created for this batch of work
			MockQuery.Setup(
				x =>
					x.DropTableAsync(
					It.IsAny<IDBContext>(),
					It.IsAny<String>()))
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
			// Remove batch was never called
			MockQuery.Verify(
				x =>
					x.RemoveBatchFromQueueAsync(
					It.IsAny<IDBContext>(),
					It.IsAny<String>()),
					Times.Never);

			// Drop table was never called
			MockQuery.Verify(
				x =>
					x.DropTableAsync(
					It.IsAny<IDBContext>(),
					It.IsAny<String>()),
					Times.Never);
		}

		private void AssertRecordWasSkippedAndFinishAsyncIsCalled()
		{
			// Remove batch was never called
			MockQuery.Verify(
				x =>
					x.RemoveBatchFromQueueAsync(
					It.IsAny<IDBContext>(),
					It.IsAny<String>()),
					Times.Once);

			// Drop table was never called
			MockQuery.Verify(
				x =>
					x.DropTableAsync(
					It.IsAny<IDBContext>(),
					It.IsAny<String>()),
					Times.Once);
		}

		private void AssertRecordWasProcessed()
		{
			// Remove batch was called once
			MockQuery.Verify(
				x =>
					x.RemoveBatchFromQueueAsync(
					It.IsAny<IDBContext>(),
					It.IsAny<String>()),
					Times.Once);

			// Drop table was called once
			MockQuery.Verify(
				x =>
					x.DropTableAsync(
					It.IsAny<IDBContext>(),
					It.IsAny<String>()),
					Times.Once);
		}

		private DataTable GetWorkerTable()
		{
			DataTable table = new DataTable("Test Worker Table");
			table.Columns.Add("WorkspaceArtifactID", typeof(Int32));
			table.Columns.Add("ID", typeof(Int32));
			table.Columns.Add("ArtifactID", typeof(Int32));
			table.Columns.Add("Priority", typeof(Int32));
			table.Rows.Add(2345678, 1, 3456789, 3);
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
