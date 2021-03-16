using $safeprojectname$.ObjectManager.Interfaces;
using Relativity.API;
using Relativity.Services.Objects;
using Relativity.Services.Objects.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace $safeprojectname$.ObjectManager
{
	public class WorkspaceQueries : IWorkspaceQueries
	{
		public async Task<Int32> GetResourcePool(IServicesMgr svcMgr, ExecutionIdentity identity, int workspaceArtifactId)
		{
			int resourcePoolId = 0;
			using (IObjectManager objectManager = svcMgr.CreateProxy<IObjectManager>(identity))
			{
				// Query for Workspace Resource Pool Name
				QueryRequest workspaceQueryRequest = new QueryRequest()
				{
					ObjectType = new ObjectTypeRef()
					{
						ArtifactTypeID = 8
					},
					Fields = new List<FieldRef>()
					{
						new FieldRef()
						{
							Name = "Resource Pool"
						}
					},
					Condition = $"'ArtifactID' == '{workspaceArtifactId}'"
				};
				QueryResultSlim workspaceQueryResultSlim = await objectManager.QuerySlimAsync(-1, workspaceQueryRequest, 0, 25);
				if (workspaceQueryResultSlim.TotalCount == 0)
				{
					throw new Exception("Failed to Query for Workspace");
				}
				string resourcePoolName = workspaceQueryResultSlim.Objects.First().Values.First().ToString();

				// Query for Resource Pool Artifact Id using Resource Pool Name
				QueryRequest resourcePoolQueryRequest = new QueryRequest()
				{
					ObjectType = new ObjectTypeRef()
					{
						ArtifactTypeID = 31
					},
					Fields = new List<FieldRef>()
					{
						new FieldRef()
						{
							Name = "ArtifactID"
						}
					},
					Condition = $"'Name' == '{resourcePoolName}'"
				};
				QueryResultSlim resourcePoolQueryResultSlim = await objectManager.QuerySlimAsync(-1, resourcePoolQueryRequest, 0, 25);
				if (resourcePoolQueryResultSlim.TotalCount == 0)
				{
					throw new Exception("Failed to Query for Resource Pool");
				}
				resourcePoolId = resourcePoolQueryResultSlim.Objects.First().ArtifactID;
			}

			return resourcePoolId;
		}
	}
}
