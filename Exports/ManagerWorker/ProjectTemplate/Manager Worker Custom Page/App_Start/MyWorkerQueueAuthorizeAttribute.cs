using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Relativity.API;
using System;
using Relativity.$safeprojectname$;
using Relativity_Extension.Helpers.Rsapi;

namespace Relativity_Extension.$safeprojectname$
{
	public class MyWorkerQueueAuthorizeAttribute : AuthorizeAttribute
	{
		protected override bool AuthorizeCore(HttpContextBase httpContext)
		{
			bool isAuthorized = false;

			if (httpContext.Session != null)
			{
				Int32 caseArtifactId = Relativity.$safeprojectname$.ConnectionHelper.Helper().GetActiveCaseID();

                ArtifactQueries query = new ArtifactQueries();
				bool res = query.DoesUserHaveAccessToArtifact(
				ConnectionHelper.Helper().GetServicesManager(),
				ExecutionIdentity.CurrentUser,
				caseArtifactId,
				Helpers.Constant.Guids.WorkerQueueTab,
				"Tab");
				isAuthorized = res;
			}

			return isAuthorized;
		}

		protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
		{
			filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
            {
                {"action", "AccessDenied"},
                {"controller", "Error"}
            });
		}
	}
}
