using System.Web.Mvc;
using Relativity.API;
using Helpers;
using Relativity.CustomPages;
using System;

namespace CustomPages
{
	public class MyCustomErrorHandler : HandleErrorAttribute
	{
		public override void OnException(ExceptionContext filterContext)
		{
			base.OnException(filterContext);
		}
	}
}
