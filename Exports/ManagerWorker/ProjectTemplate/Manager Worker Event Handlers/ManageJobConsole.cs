using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using kCura.EventHandler;
using Relativity.API;
using Relativity_Extension.Helpers;
using Relativity_Extension.Helpers.Rsapi;
using Relativity_Extension.Helpers.Rsapi.Interfaces;
using Console = kCura.EventHandler.Console;

namespace Relativity_Extension.$safeprojectname$
{
	[kCura.EventHandler.CustomAttributes.Description("Allows a user to submit a job or remove a job from the manager queue.")]
	[System.Runtime.InteropServices.Guid("D004F1C2-4D74-4F9A-A017-B69107330456")]
	public class ManageJobConsole : ConsoleEventHandler
	{
		public IQuery QueryHelper = new Query();
		public IWorkspaceQueries WorkspaceQueryHelper = new WorkspaceQueries();

		public override Console GetConsole(PageEvent pageEvent)
		{
			Console console = GetConsoleAsync(pageEvent, GetLogger().Result).Result;
			return console;
		}

		public async Task<Console> GetConsoleAsync(PageEvent pageEvent, IAPILog logger)
		{
			Console console = new Console { Items = new List<IConsoleItem>(), Title = "Manage Job" };

			ConsoleButton addButton = new ConsoleButton
			{
				Name = Constant.Buttons.ADD,
				DisplayText = "Add to Queue",
				ToolTip = "Click here to add this job to the queue",
				RaisesPostBack = true
			};

			ConsoleButton removeButton = new ConsoleButton
			{
				Name = Constant.Buttons.REMOVE,
				DisplayText = "Remove from Queue",
				ToolTip = "Click here to remove this job from the queue",
				RaisesPostBack = true
			};

			if (pageEvent == PageEvent.PreRender)
			{
				bool recordExists = await DoesRecordExistAsync();
				if (recordExists)
				{
					addButton.Enabled = false;
					removeButton.Enabled = true;
				}
				else
				{
					addButton.Enabled = true;
					removeButton.Enabled = false;
				}
			}

			console.Items.Add(addButton);
			console.Items.Add(removeButton);

			return console;
		}

		public override void OnButtonClick(ConsoleButton consoleButton)
		{
			OnButtonClickAsync(consoleButton, GetLogger().Result).Wait();
		}

		public async Task OnButtonClickAsync(ConsoleButton consoleButton, IAPILog logger)
		{
			switch (consoleButton.Name)
			{
				case Constant.Buttons.ADD:
					logger.LogDebug(String.Format("{0} - {1} button clicked.", Constant.Names.ApplicationName, Constant.Buttons.ADD));
					bool recordExists = await DoesRecordExistAsync();
					if (recordExists == false)
					{
						Int32 resourceGroupId = await WorkspaceQueryHelper.GetResourcePool(Helper.GetServicesManager(), ExecutionIdentity.System, Helper.GetActiveCaseID());
						await QueryHelper.InsertRowIntoManagerQueueAsync(Helper.GetDBContext(-1), Helper.GetActiveCaseID(), 1, Helper.GetAuthenticationManager().UserInfo.ArtifactID,
							ActiveArtifact.ArtifactID, resourceGroupId);
					}
					break;
				case Constant.Buttons.REMOVE:
					logger.LogDebug(String.Format("{0} - {1} button clicked.", Constant.Names.ApplicationName, Constant.Buttons.REMOVE));
					int id = await RetrieveIDByArtifactIDAsync();
					if (id > 0)
					{
						await QueryHelper.RemoveRecordFromTableByIDAsync(Helper.GetDBContext(-1), Constant.Tables.ManagerQueue, id);
					}
					break;
			}
		}

		public override FieldCollection RequiredFields
		{
			get
			{
				FieldCollection retVal = new FieldCollection();
				return retVal;
			}
		}

		private async Task<Boolean> DoesRecordExistAsync()
		{
			DataRow dataRow = await QueryHelper.RetrieveSingleInManagerQueueByArtifactIdAsync(Helper.GetDBContext(-1), ActiveArtifact.ArtifactID, Helper.GetActiveCaseID());
			return dataRow != null;
		}

		private async Task<Int32> RetrieveIDByArtifactIDAsync()
		{
			DataRow dataRow = await QueryHelper.RetrieveSingleInManagerQueueByArtifactIdAsync(Helper.GetDBContext(-1), ActiveArtifact.ArtifactID, Helper.GetActiveCaseID());
			if (dataRow != null)
			{
				return Convert.ToInt32(dataRow["ID"]);
			}
			return 0;
		}

		private async Task<IAPILog> GetLogger()
		{
			IAPILog logger = await Task.Run(() => Helper.GetLoggerFactory().GetLogger());
			return logger;
		}
	}
}
