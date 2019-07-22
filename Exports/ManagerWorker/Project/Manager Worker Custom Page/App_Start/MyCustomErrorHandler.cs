using System.Web.Mvc;
using Relativity.API;
using Helpers;
using Relativity.$safeprojectname$;
using System;

namespace $safeprojectname$
{
	public class MyCustomErrorHandler : HandleErrorAttribute
	{
		public override void OnException(ExceptionContext filterContext)
		{
			base.OnException(filterContext);
			Int32 caseArtifactId = Relativity.$safeprojectname$.ConnectionHelper.Helper().GetActiveCaseID();

            Helpers.IQuery queryHelper = new Query();

			if (filterContext.Exception != null)
			{
				try
				{
					//try to log the error to the errors tab in Relativity
					Helpers.Rsapi.ErrorQueries.WriteError(ConnectionHelper.Helper().GetServicesManager(),
					ExecutionIdentity.CurrentUser, caseArtifactId, filterContext.Exception);
				}
				catch
				{
					//if the error cannot be logged, add the error to our custom Errors table
					queryHelper.InsertRowIntoErrorLogAsync(ConnectionHelper.Helper().GetDBContext(-1), caseArtifactId, Constant.Tables.WorkerQueue, 0, 0, filterContext.Exception.ToString()).Wait();
				}
			}
		}
	}
}
