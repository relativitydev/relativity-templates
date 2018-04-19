using System;
using System.Linq;
using kCura.Relativity.Client;
using kCura.Relativity.Client.DTOs;
using Relativity.API;

namespace Relativity_Extension.$safeprojectname$.Rsapi
{
	public class ArtifactQueries
	{
		//Do not convert to async
		public Boolean DoesUserHaveAccessToArtifact(IServicesMgr svcMgr, ExecutionIdentity identity, Int32 workspaceArtifactID, Guid guid, String artifactTypeName)
		{
			Response<Boolean> result = DoesUserHaveAccessToRdoByType(svcMgr, identity, workspaceArtifactID, guid, artifactTypeName);
			Boolean hasAccess = result.Success;

			return hasAccess;
		}

		//Do not convert to async
		public Response<Boolean> DoesUserHaveAccessToRdoByType(IServicesMgr svcMgr, ExecutionIdentity identity, Int32 workspaceArtifactID, Guid guid, String artifactTypeName)
		{
			ResultSet<RDO> results = new ResultSet<RDO>();

			using (IRSAPIClient client = svcMgr.CreateProxy<IRSAPIClient>(identity))
			{
				client.APIOptions.WorkspaceID = workspaceArtifactID;
				RDO relApp = new RDO(guid)
				{
					ArtifactTypeName = artifactTypeName
				};

				results = client.Repositories.RDO.Read(relApp);
			}

			Response<bool> res = new Response<Boolean>
			{
				Results = results.Success,
				Success = results.Success,
				Message = MessageFormatter.FormatMessage(results.Results.Select(x => x.Message).ToList(), results.Message, results.Success)
			};

			return res;
		}
	}
}
