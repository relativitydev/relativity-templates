using System.Threading.Tasks;
using System.Web.Mvc;
using Relativity.$safeprojectname$;
using Relativity_Extension.$safeprojectname$.Models;

namespace Relativity_Extension.$safeprojectname$.Controllers
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
