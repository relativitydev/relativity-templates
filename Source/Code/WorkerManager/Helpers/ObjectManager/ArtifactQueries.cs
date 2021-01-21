using System;
using System.Collections.Generic;
using System.Linq;
using kCura.Relativity.Client;
using kCura.Relativity.Client.DTOs;
using Relativity.API;
using Relativity.Services.Objects;
using Relativity.Services.Objects.DataContracts;
using Field = Relativity.Services.Objects.DataContracts.Field;
using ReadResult = Relativity.Services.Objects.DataContracts.ReadResult;

namespace Helpers.ObjectManager
{
	public class ArtifactQueries
	{
		//Do not convert to async
		public Boolean DoesUserHaveAccessToArtifact(IServicesMgr svcMgr, ExecutionIdentity identity, Int32 workspaceArtifactID, Guid guid, String artifactTypeName)
		{
			Boolean result = DoesUserHaveAccessToRdoByType(svcMgr, identity, workspaceArtifactID, guid, artifactTypeName);
			return result;
		}

		//Do not convert to async
		public Boolean DoesUserHaveAccessToRdoByType(IServicesMgr svcMgr, ExecutionIdentity identity, Int32 workspaceArtifactID, Guid guid, String artifactTypeName)
		{
			bool res = false;
			using (IObjectManager objectManager = svcMgr.CreateProxy<IObjectManager>(identity))
			{
				ReadRequest readRequest = new ReadRequest
				{
					Object = new RelativityObjectRef
					{
						Guid = guid
					},
				};
				try
				{
					ReadResult readResult = objectManager.ReadAsync(workspaceArtifactID, readRequest).Result;
					res = true;
				}
				catch (Exception ex)
				{
					res = false;
				}
			}

			return res;
		}
	}
}
