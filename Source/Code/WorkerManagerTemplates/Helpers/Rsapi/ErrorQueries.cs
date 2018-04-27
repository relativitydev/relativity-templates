using System;
using System.Collections.Generic;
using kCura.Relativity.Client;
using kCura.Relativity.Client.DTOs;
using Relativity.API;

namespace Helpers.Rsapi
{
	public class ErrorQueries
	{
		#region Public Methods
		//Do not convert to async
		public static void WriteError(IServicesMgr svcMgr, ExecutionIdentity identity, Int32 workspaceArtifactID, Exception ex)
		{
			using (IRSAPIClient client = svcMgr.CreateProxy<IRSAPIClient>(identity))
			{
				client.APIOptions.WorkspaceID = workspaceArtifactID;

				Response<IEnumerable<Error>> res = WriteError(client, workspaceArtifactID, ex);
				if (!res.Success)
				{
					throw new Exception(res.Message);
				}
			}
		}
		#endregion

		#region Private Methods
		//Do not convert to async
		private static Response<IEnumerable<Error>> WriteError(IRSAPIClient proxy, int workspaceArtifactId, Exception ex)
		{
			Error artifact = new Error();
			artifact.FullError = ex.StackTrace;
			artifact.Message = ex.Message;
			artifact.SendNotification = false;
			artifact.Server = Environment.MachineName;
			artifact.Source = String.Format("{0} [Guid={1}]", Constant.Names.ApplicationName, Constant.Guids.ApplicationGuid);
			artifact.Workspace = new Workspace(workspaceArtifactId);
			WriteResultSet<Error> theseResults = proxy.Repositories.Error.Create(artifact);
			return Response<Error>.CompileWriteResults(theseResults);
		}
		#endregion Private Methods
	}
}
