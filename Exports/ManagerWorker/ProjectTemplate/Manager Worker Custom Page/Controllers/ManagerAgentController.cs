using System.Threading.Tasks;
using System.Web.Mvc;
using Relativity.$safeprojectname$;
using Relativity_Extension.$safeprojectname$.Models;

namespace Relativity_Extension.$safeprojectname$.Controllers
{
	[MyManagerQueueAuthorize]
	public class ManagerAgentController : Controller
	{
		[HttpGet]
		public async Task<ActionResult> Index()
		{
		    ManagerAgentModel model = new ManagerAgentModel();
			await model.GetAllAsync(ConnectionHelper.Helper().GetDBContext(-1));
			return View(model);
		}
	}
}
