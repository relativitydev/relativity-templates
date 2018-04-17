using System;
using System.Threading.Tasks;
using kCura.Relativity.Client;
using Relativity.API;
using Relativity_Extension.Helpers.Rsapi.Interfaces;

namespace Relativity_Extension.Helpers.Rsapi
{
	public class WorkspaceQueries : IWorkspaceQueries
	{
		public async Task<Int32> GetResourcePool(IServicesMgr svcMgr, ExecutionIdentity identity, int workspaceArtifactId)
		{
			return await Task.Run(() =>
			{
				Int32 resourcePoolId = 0;
				using (IRSAPIClient proxy = svcMgr.CreateProxy<IRSAPIClient>(identity))
				{
					int? result = proxy.Repositories.Workspace.ReadSingle(workspaceArtifactId).ResourcePoolID;
					if (result.HasValue)
					{
						resourcePoolId = result.Value;
					}

					return resourcePoolId;
				}
			});
		}
	}
}
