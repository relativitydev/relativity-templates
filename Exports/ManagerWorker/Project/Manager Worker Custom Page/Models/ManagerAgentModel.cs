using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Relativity.API;
using Helpers;

namespace $safeprojectname$.Models
{
	public class ManagerAgentModel
	{
		public List<ManagerQueueRecordModel> Records { get; set; }
		public IQuery QueryHelper;

		public ManagerAgentModel(IQuery queryModel)
		{
			QueryHelper = queryModel;
			Records = new List<ManagerQueueRecordModel>();
		}

		public ManagerAgentModel()
		{
			QueryHelper = new Query();
			Records = new List<ManagerQueueRecordModel>();
		}

		public async Task GetAllAsync(IDBContext eddsDbContext)
		{
			DataTable dt = await QueryHelper.RetrieveAllInManagerQueueAsync(eddsDbContext);

			foreach (DataRow thisRow in dt.Rows)
			{
				Records.Add(new ManagerQueueRecordModel(thisRow, QueryHelper));
			}
		}
	}
}
