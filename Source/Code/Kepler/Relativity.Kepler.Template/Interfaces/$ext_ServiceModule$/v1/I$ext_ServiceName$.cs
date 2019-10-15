using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Relativity.Kepler.Services;
using $ext_safeprojectname$.Interfaces.$ext_ServiceModule$.v1.Models;

namespace $ext_safeprojectname$.Interfaces.$ext_ServiceModule$.v1
{
	/// <summary>
	/// MyService Service Interface.
	/// </summary>
	[WebService("$ext_ServiceName$ Service")]
	[ServiceAudience(Audience.Public)]
	[RoutePrefix("$ext_ServiceName$")]
	public interface $safeitemrootname$ : IDisposable
	{
		/// <summary>
		/// Get workspace name.
		/// </summary>
		/// <param name="workspaceID">Workspace ArtifactID.</param>
		/// <returns><see cref="$ext_ServiceName$Model"/> with the name of the workspace.</returns>
		/// <remarks>
		/// Example REST request:
		///   [GET] /Relativity.REST/api/$ext_ServiceModule$/v1/$ext_ServiceName$/workspace/1015024
		/// Example REST response:
		///   {"Name":"Relativity Starter Template"}
		/// </remarks>
		[HttpGet]
		[Route("workspace/{workspaceID:int}")]
		Task<$ext_ServiceName$Model> GetWorkspaceNameAsync(int workspaceID);

		/// <summary>
		/// Query for a workspace by name
		/// </summary>
		/// <param name="queryString">Partial name of a workspace to query for.</param>
		/// <param name="limit">Limit the number of results via a query string parameter. (Default 10)</param>
		/// <returns>Collection of <see cref="$ext_ServiceName$Model"/> containing workspace names that match the query string.</returns>
		/// <remarks>
		/// Example REST request:
		///   [POST] /Relativity.REST/api/$ext_ServiceModule$/v1/$ext_ServiceName$/workspace?limit=2
		///   { "queryString":"a" }
		/// Example REST response:
		///   [{"Name":"New Case Template"},{"Name":"Relativity Starter Template"}]
		/// </remarks>
		[HttpPost]
		[Route("workspace?{limit}")]
		Task<List<$ext_ServiceName$Model>> QueryWorkspaceByNameAsync(string queryString, int limit = 10);
	}
}
