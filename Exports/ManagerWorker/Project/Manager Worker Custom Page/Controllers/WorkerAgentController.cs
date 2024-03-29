﻿using System.Threading.Tasks;
using System.Web.Mvc;
using Relativity.CustomPages;
using $saferootprojectname$.CustomPages.Models;

namespace $safeprojectname$.Controllers
{
	[MyWorkerQueueAuthorize]
	public class WorkerAgentController : Controller
	{
		[HttpGet]
		public async Task<ActionResult> Index()
		{
		    WorkerAgentModel model = new WorkerAgentModel();
			await model.GetAllAsync(ConnectionHelper.Helper().GetDBContext(-1));
			return View(model);
		}
	}
}
