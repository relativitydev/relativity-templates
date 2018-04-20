using System;
using System.IO;
using System.Reflection;
using kCura.Relativity.Client;
using NUnit.Framework;
using Relativity.API;
using Relativity.Test.Helpers;
using Relativity.Test.Helpers.ArtifactHelpers;
using Relativity.Test.Helpers.GroupHelpers;
using Relativity.Test.Helpers.ServiceFactory.Extentions;
using Relativity.Test.Helpers.SharedTestHelpers;
using Relativity.Test.Helpers.UserHelpers;
using Relativity.Test.Helpers.WorkspaceHelpers;

namespace $rootnamespace$
{
    /// <summary>
    ///     Relativity Integration Test Helpers to assist you with writing good Integration Tests for your application. You can
    ///     use this framework to test event handlers, agents and any workflow that combines agents and frameworks.
    ///     Before you get Started, fill out details for the following in the app.config file
    ///     "WorkspaceID", "RSAPIServerAddress", "RESTServerAddress",	"AdminUsername","AdminPassword", "SQLServerAddress"
    ///     ,"SQLUsername","SQLPassword" "TestWorkspaceName"
    /// </summary>
    [TestFixture]
    [Description("Fixture description here")]
    public class $safeitemname$
    {
        private IRSAPIClient _client;
        private int _workspaceId;
        private int _rootFolderArtifactID;
        private readonly string _workspaceName = $"IntTest_{Guid.NewGuid()}";
        private const ExecutionIdentity EXECUTION_IDENTITY = ExecutionIdentity.CurrentUser;
        private IDBContext dbContext;
        private IServicesMgr servicesManager;
        private IDBContext _eddsDbContext;
        private readonly int _numberOfDocuments = 5;
        private readonly string _foldername = "Test Folder";
        private readonly string _groupName = "Test Group";
        private int _userArtifactId;
        private int _groupArtifactId;
        private int _fixedLengthArtId;
        private int _longtextartid;
        private int _yesnoartid;
        private int _wholeNumberArtId;

        [TestFixtureSetUp]
        public void Execute_TestFixtureSetup()
        {
            //Setup for testing		
            TestHelper helper = new TestHelper();
            servicesManager = helper.GetServicesManager();
            _eddsDbContext = helper.GetDBContext(-1);

            // implement_IHelper
            //create client
            _client = helper.GetServicesManager().GetProxy<IRSAPIClient>(ConfigurationHelper.ADMIN_USERNAME, ConfigurationHelper.DEFAULT_PASSWORD);

            //Create new user 
            _userArtifactId = CreateUser.CreateNewUser(_client);

            //Create new group
            CreateGroup.Create_Group(_client, _groupName);


            //Create workspace
            _workspaceId = CreateWorkspace.CreateWorkspaceAsync(_workspaceName, ConfigurationHelper.TEST_WORKSPACE_TEMPLATE_NAME, servicesManager, ConfigurationHelper.ADMIN_USERNAME, ConfigurationHelper.DEFAULT_PASSWORD).Result;
            dbContext = helper.GetDBContext(_workspaceId);
            _client.APIOptions.WorkspaceID = _workspaceId;
            string executableLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string nativeFilePath = "";
            string nativeName = @"\\\\FakeFilePath\Natives\SampleTextFile.txt";
            ;
            if (executableLocation != null)
                nativeFilePath = Path.Combine(executableLocation, nativeName);
            //Create Documents with a given folder name
            Test.Helpers.ImportAPIHelper.ImportAPIHelper.CreateDocumentswithFolderName(_workspaceId, _numberOfDocuments, _foldername, nativeFilePath);

            //Create Documents with a given folder artifact id
            string folderName = Folders.GetFolderName(_rootFolderArtifactID, dbContext);
            Test.Helpers.ImportAPIHelper.ImportAPIHelper.CreateDocumentswithFolderName(_workspaceId, _numberOfDocuments, folderName, nativeFilePath);

            //Create Fixed Length field
            _fixedLengthArtId = Fields.CreateField_FixedLengthText(_client, _workspaceId);

            //Create Long Text Field
            _longtextartid = Fields.CreateField_LongText(_client, _workspaceId);

            //Create Whole number field
            _wholeNumberArtId = Fields.CreateField_WholeNumber(_client, _workspaceId);

            //Create Yes/no field

            //Create Single Choice fields

            //Create Multiple Choice fields
        }


        [TestFixtureTearDown]
        public void Execute_TestFixtureTeardown()
        {
            //Delete Workspace
            DeleteWorkspace.DeleteTestWorkspace(_workspaceId, servicesManager, ConfigurationHelper.ADMIN_USERNAME, ConfigurationHelper.DEFAULT_PASSWORD);

            //Delete User
            DeleteUser.Delete_User(_client, _userArtifactId);

            //Delete Group
            DeleteGroup.Delete_Group(_client, _groupArtifactId);
        }

        [Test]
        [Description("Test description here")]
        public void Integration_Test_Golden_Flow_Valid()
        {
            //Example for Arrange, Act, Assert

            //Arrange
            //CreateEmptyFolders();
            //countBeforeExecute = SQLHelper.GetFolderCount(_workspaceId);

            ////Act
            //_Executeresult = TestHelpers.Execute.ExecuteRelativityScriptByNameThroughRsapi("Delete Empty Case Folders", _workspaceId);

            ////Assert
            //Assert.AreEqual(_Executeresult, "Empty folders deleted with no errors.");
            //countAfterExecute = SQLHelper.GetFolderCount(_workspaceId);
            //Assert.AreEqual(countAfterExecute, 0);
            //Assert.AreNotEqual(countBeforeExecute, countAfterExecute);
        }
    }
}