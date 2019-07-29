using System;
using System.Threading.Tasks;
using kCura.EventHandler;
using Relativity.API;
using $saferootprojectname$.Helpers;

namespace $safeprojectname$
{
	[kCura.EventHandler.CustomAttributes.RunOnce(false)]
	[kCura.EventHandler.CustomAttributes.Description("Creates the underlying tables for the application.")]
	[System.Runtime.InteropServices.Guid("$guid1$")]
	class PostInstallSetup : kCura.EventHandler.PostInstallEventHandler
	{
		private IAPILog Logger;

		public override kCura.EventHandler.Response Execute()
		{
			return ExecuteAsync().Result;
		}

		public async Task<kCura.EventHandler.Response> ExecuteAsync()
		{
			return await Task.Run(() =>
			{
				Logger = Helper.GetLoggerFactory().GetLogger();

				Response response = new kCura.EventHandler.Response
				{
					Success = true,
					Message = String.Empty
				};
				Query queryHelper = new Query();

				//Create manager queue table if it doesn't already exist
				Task managerTableTask = queryHelper.CreateManagerQueueTableAsync(Helper.GetDBContext(-1));
				Logger.LogDebug(String.Format("{0} - Created Manager queue table if it doesn't already exist", Constant.Names.ApplicationName));

				//Create worker queue table if it doesn't already exist
				Task workerTableTask = queryHelper.CreateWorkerQueueTableAsync(Helper.GetDBContext(-1));
				Logger.LogDebug(String.Format("{0} - Created Worker queue table if it doesn't already exist", Constant.Names.ApplicationName));

				//Create error log table if it doesn't already exist
				Task errorTableTask = queryHelper.CreateErrorLogTableAsync(Helper.GetDBContext(-1));
				Logger.LogDebug(String.Format("{0} - Created ErrorLog table if it doesn't already exist", Constant.Names.ApplicationName));

				try
				{
					// Waits for all tasks, otherwise exceptions would be lost
					Task.WaitAll(managerTableTask, workerTableTask, errorTableTask);
				}
				catch (AggregateException aex)
				{
					Logger.LogError(aex, String.Format("{0} - Post-Install failed. {1}", Constant.Names.ApplicationName, aex));
					AggregateException ex = aex.Flatten();
					string message = ex.Message + " : " + (ex.InnerException != null ? ex.InnerException.Message : "None");
					response.Success = false;
					response.Message = "Post-Install field rename failed with message: " + message;
				}
				return response;
			});
		}
	}
}
