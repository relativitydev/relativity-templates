using System;
using System.Data;
using System.Threading.Tasks;
using kCura.EventHandler;
using Moq;
using NUnit.Framework;
using Relativity.API;
using $saferootprojectname$.Helpers;
using $saferootprojectname$.Helpers.Rsapi.Interfaces;
using PageEvent = kCura.EventHandler.ConsoleEventHandler.PageEvent;
using $saferootprojectname$.EventHandlers;
using Console = kCura.EventHandler.Console;

namespace $safeprojectname$
{
	[TestFixture]
	public class ManageJobConsoleTests
	{
		public Mock<IEHHelper> MockEHHelper;
		public Mock<IQuery> MockQuery;
		public Mock<IWorkspaceQueries> MockWorkspaceQuery;
		public Mock<IAPILog> MockLogger;

		[SetUp]
		public void SetUp()
		{
			MockEHHelper = new Mock<IEHHelper>();
			MockQuery = new Mock<IQuery>();
			MockWorkspaceQuery = new Mock<IWorkspaceQueries>();
			MockLogger = new Mock<IAPILog>();
		}

		[Test]
		public async Task GetConsoleAsync_RecordExists_ReturnsConsoleWithRemoveNotAdd()
		{
			// Arrange 
			WhenARecordExists();
			ManageJobConsole console = GetManageJobConsole();

			// Act 
			Console actual = await console.GetConsoleAsync(PageEvent.PreRender, MockLogger.Object);

			// Assert 
			Assert.IsNotNull(actual);
			Assert.IsNotNull(actual.Items);
			AssertHasButtons(actual);
			AssertAddIsDisabledAndRemoveIsEnabled(actual);
		}

		[Test]
		public async Task GetConsoleAsync_NoRecordExists_ReturnsConsoleWithAddNotRemove()
		{
			// Arrange 
			WhenNoRecordExists();
			ManageJobConsole console = GetManageJobConsole();

			// Act 
			Console actual = await console.GetConsoleAsync(PageEvent.PreRender, MockLogger.Object);

			// Assert 
			Assert.IsNotNull(actual);
			Assert.IsNotNull(actual.Items);
			AssertHasButtons(actual);
			AssertAddIsEnabledAndRemoveIsDisabled(actual);
		}

		[Test]
		public async Task OnButtonClick_ClickAddButton_AddsRecordInManagerQueue()
		{
			// Arrange 
			WhenNoRecordExists();
			ManageJobConsole console = GetManageJobConsole();
			ConsoleButton button = new ConsoleButton() { Name = "add" };

			// Act 
			await console.OnButtonClickAsync(button, MockLogger.Object);

			// Assert 
			ExpectRecordWasAddedToTheManagerQueue();
		}

		[Test]
		public async Task OnButtonClick_ClickRemoveButton_RemovesRecordFromManagerQueue()
		{
			// Arrange 
			WhenARecordExists();
			ManageJobConsole console = GetManageJobConsole();
			ConsoleButton button = new ConsoleButton() { Name = "remove" };

			// Act 
			await console.OnButtonClickAsync(button, MockLogger.Object);

			// Assert 
			ExpectRecordWasRemovedFromManagerQueue();
		}

		#region Helper Methods

		private ManageJobConsole GetManageJobConsole()
		{
			ManageJobConsole console = new ManageJobConsole();
			console = AddActiveArtifacts(console);
			console = AddMockHelper(console);
			console = AddMockQuery(console);
			console = AddMockWorkspaceQuery(console);
			return console;
		}

		private ManageJobConsole AddActiveArtifacts(ManageJobConsole console)
		{
			console.ActiveArtifact = new Artifact(1234567, 2345678, 16, "Console", true, new FieldCollection() { new Field(3456789) });
			console.ActiveLayout = new Layout(1234567, "Test Console Layout");
			return console;
		}

		private ManageJobConsole AddMockHelper(ManageJobConsole console)
		{
			Mock<IAuthenticationMgr> authMgr = new Mock<IAuthenticationMgr>();
			authMgr.Setup(
				x =>
				x.UserInfo)
				.Returns(new Mock<IUserInfo>().Object);

			MockEHHelper.Setup(
				x =>
				x.GetAuthenticationManager())
				.Returns(authMgr.Object);

			console.Helper = MockEHHelper.Object;
			return console;
		}

		private ManageJobConsole AddMockQuery(ManageJobConsole console)
		{
			// Makes these two calls verifiable 
			MockQuery.Setup(
				x =>
				x.InsertRowIntoManagerQueueAsync(
					It.IsAny<IDBContext>(),
					It.IsAny<Int32>(),
					It.IsAny<Int32>(),
					It.IsAny<Int32>(),
					It.IsAny<Int32>(),
					It.IsAny<Int32>()))
					.Returns(Task.FromResult(false))
					.Verifiable();

			MockQuery.Setup(
				x =>
				x.RemoveRecordFromTableByIDAsync(
					It.IsAny<IDBContext>(),
					It.IsAny<String>(),
					It.IsAny<Int32>()))
					.Returns(Task.FromResult(false))
					.Verifiable();

			console.QueryHelper = MockQuery.Object;
			return console;
		}

		private ManageJobConsole AddMockWorkspaceQuery(ManageJobConsole console)
		{
			MockWorkspaceQuery.Setup(
				x =>
				x.GetResourcePool(
				It.IsAny<IServicesMgr>(),
				It.IsAny<ExecutionIdentity>(),
				It.IsAny<Int32>()))
				.ReturnsAsync(It.IsAny<Int32>())
				.Verifiable();

			console.WorkspaceQueryHelper = MockWorkspaceQuery.Object;
			return console;
		}

		private Boolean HasAddButton(IConsoleItem item)
		{
			ConsoleButton button = (ConsoleButton)item;
			return (button.Name == "add");
		}

		private Boolean HasRemoveButton(IConsoleItem item)
		{
			ConsoleButton button = (ConsoleButton)item;
			return (button.Name == "remove");
		}

		private DataRow GetDataRow()
		{
			DataTable table = new DataTable("Test Table");
			table.Columns.Add("ID");
			table.Rows.Add(1);
			return table.Rows[0];
		}

		#endregion Helper Methods

		#region When Methods

		private void WhenARecordExists()
		{
			MockQuery.Setup(
				x => x.RetrieveSingleInManagerQueueByArtifactIdAsync(
					It.IsAny<IDBContext>(),
					It.IsAny<Int32>(),
					It.IsAny<Int32>()))
				.ReturnsAsync(GetDataRow());
		}

		private void WhenNoRecordExists()
		{
			MockQuery.Setup(
				x => x.RetrieveSingleInManagerQueueByArtifactIdAsync(
					It.IsAny<IDBContext>(),
					It.IsAny<Int32>(),
					It.IsAny<Int32>()))
				.ReturnsAsync(null);
		}

		#endregion When Methods

		#region Assert Methods

		private void AssertHasButtons(kCura.EventHandler.Console console)
		{
			Assert.IsTrue(console.Items.Exists(HasAddButton));
			Assert.IsTrue(console.Items.Exists(HasRemoveButton));
		}

		private void AssertAddIsDisabledAndRemoveIsEnabled(kCura.EventHandler.Console console)
		{
			ConsoleButton addButton = (ConsoleButton)console.Items.Find(HasAddButton);
			ConsoleButton removeButton = (ConsoleButton)console.Items.Find(HasRemoveButton);

			Assert.IsFalse(addButton.Enabled);
			Assert.IsTrue(removeButton.Enabled);
		}

		private void AssertAddIsEnabledAndRemoveIsDisabled(kCura.EventHandler.Console console)
		{
			ConsoleButton addButton = (ConsoleButton)console.Items.Find(HasAddButton);
			ConsoleButton removeButton = (ConsoleButton)console.Items.Find(HasRemoveButton);

			Assert.IsTrue(addButton.Enabled);
			Assert.IsFalse(removeButton.Enabled);
		}

		#endregion Assert Methods

		#region Expect Methods

		private void ExpectRecordWasAddedToTheManagerQueue()
		{
			MockQuery.Verify(
				x =>
				x.InsertRowIntoManagerQueueAsync(
					It.IsAny<IDBContext>(),
					It.IsAny<Int32>(),
					It.IsAny<Int32>(),
					It.IsAny<Int32>(),
					It.IsAny<Int32>(),
					It.IsAny<Int32>()),
					Times.Once);
		}

		private void ExpectRecordWasRemovedFromManagerQueue()
		{
			MockQuery.Verify(
				x =>
				x.RemoveRecordFromTableByIDAsync(
					It.IsAny<IDBContext>(),
					It.IsAny<String>(),
					It.IsAny<Int32>()),
					Times.Once);
		}

		#endregion Expect Methods
	}
}
