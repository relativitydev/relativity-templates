using System.Web.Mvc;
using Relativity.API;
using $saferootprojectname$.Helpers;
using Relativity.CustomPages;
using System;

namespace $safeprojectname$
{
	public class MyCustomErrorHandler : HandleErrorAttribute
	{
		public override void OnException(ExceptionContext filterContext)
		{
			base.OnException(filterContext);
		}
	}
}
