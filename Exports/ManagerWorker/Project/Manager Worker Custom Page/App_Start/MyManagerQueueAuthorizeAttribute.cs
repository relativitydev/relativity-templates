﻿using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Relativity.API;
using System;
using $saferootprojectname$.Helpers.ObjectManager;
using Relativity.CustomPages;

namespace $safeprojectname$
{
	public class MyManagerQueueAuthorizeAttribute : AuthorizeAttribute
	{
		protected override bool AuthorizeCore(HttpContextBase httpContext)
		{
			bool isAuthorized = false;

			if (httpContext.Session != null)
			{
				Int32 caseArtifactId = Relativity.CustomPages.ConnectionHelper.Helper().GetActiveCaseID();
				ArtifactQueries query = new ArtifactQueries();
				bool res = query.DoesUserHaveAccessToArtifact(
				ConnectionHelper.Helper().GetServicesManager(),
				ExecutionIdentity.CurrentUser,
				caseArtifactId,
				Helpers.Constant.Guids.ManagerQueueTab,
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
