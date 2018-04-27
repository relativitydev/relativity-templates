using System.Threading.Tasks;
using System.Web.Mvc;
using Relativity.CustomPages;
using CustomPages.Models;

namespace CustomPages.Controllers
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
