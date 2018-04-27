using System;
using System.Threading.Tasks;
using Relativity.API;

namespace Helpers.Rsapi.Interfaces
{
	public interface IWorkspaceQueries
	{
		Task<Int32> GetResourcePool(IServicesMgr svcMgr, ExecutionIdentity identity, int workspaceArtifactId);
	}
}
