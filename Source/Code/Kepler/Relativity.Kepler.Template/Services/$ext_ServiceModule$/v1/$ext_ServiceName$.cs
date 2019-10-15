using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Relativity.API;
using Relativity.API.Context;
using Relativity.Kepler.Logging;
using Relativity.Services.Exceptions;
using $ext_safeprojectname$.Interfaces.$ext_ServiceModule$.v1;
using $ext_safeprojectname$.Interfaces.$ext_ServiceModule$.v1.Exceptions;
using $ext_safeprojectname$.Interfaces.$ext_ServiceModule$.v1.Models;

namespace $ext_safeprojectname$.Services.$ext_ServiceModule$.v1
{
	public class $safeitemrootname$ : I$safeitemrootname$
	{
		private IHelper _helper;
		private ILog _logger;

		// Note: IHelper and ILog are dependency injected into the constructor every time the service is called.
		public $safeitemrootname$(IHelper helper, ILog logger)
		{
			// Note: Set the logging context to the current class.
			_logger = logger.ForContext<$safeitemrootname$>();
			_helper = helper;
		}

		public async Task<$ext_ServiceName$Model> GetWorkspaceNameAsync(int workspaceID)
{
			$ext_ServiceName$Model model;

			try
			{
				// Use the dependency injected IHelper to get a database connection.
				// In this example a query is being made for the name of a workspace from the workspaceID.
				// Note: async/await and ConfigureAwait(false) is used when making calls external to the service.
				//       See https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/
				//       See also https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.task.configureawait
				//       See also https://blogs.msdn.microsoft.com/benwilli/2017/02/09/an-alternative-to-configureawaitfalse-everywhere/
				//       See also https://blog.stephencleary.com/2012/07/dont-block-on-async-code.html
				//       Warning: Improper use of the tasks can cause deadlocks and performance issues within an application.
				string workspaceName = await _helper.GetDBContext(workspaceID).ExecuteScalarAsync<string>(new ContextQuery()
				{
					SqlStatement = "SELECT [TextIdentifier] FROM [EDDSDBO].[Artifact] WHERE [ArtifactTypeID] = 8"
				}).ConfigureAwait(false);

				model = new $ext_ServiceName$Model
				{
					Name = workspaceName
				};
			}
			catch (Exception exception)
			{
				// Note: logging templates should never use interpolation! Doing so will cause memory leaks. 
				_logger.LogWarning(exception, "Could not read workspace {WorkspaceID}.", workspaceID);
				
				// Throwing a user defined exception with a 404 status code with an additional custom FaultSafe object.
				throw new $ext_ServiceName$Exception($"Workspace {workspaceID} not found.")
				{
					FaultSafeObject = new $ext_ServiceName$Exception.FaultSafeInfo()
					{
						Information = $"Workspace {workspaceID}",
						Time = DateTime.Now
					}
				};
			}

			return model;
		}

		public async Task<List<$ext_ServiceName$Model>> QueryWorkspaceByNameAsync(string queryString, int limit)
		{
			var models = new List<$ext_ServiceName$Model>();

			// Create a Kepler service proxy to interact with other Kepler services.
			// Use the dependency injected IHelper to create a proxy to an external service.
			// This proxy will execute as the currently logged in user. (ExecutionIdentity.CurrentUser)
			// Note: If calling methods within the same service the proxy is not needed. It is doing so
			//       in this example only as a demonstration of how to call other services.
			var proxy = _helper.GetServicesManager().CreateProxy<I$ext_ServiceName$>(ExecutionIdentity.CurrentUser);

			// Validate queryString and throw a ValidationException (HttpStatusCode 400) if the string does not meet the validation requirements.
			if (string.IsNullOrEmpty(queryString) || queryString.Length > 50)
			{
				// ValidationException is in the namespace Relativity.Services.Exceptions and found in the Relativity.Kepler.dll.
				throw new ValidationException($"{nameof(queryString)} cannot be empty or grater than 50 characters.");
			}

			try
			{
				// Use the dependency injected IHelper to get a database connection.
				// In this example a query is made for all workspaces that are like the query string.
				// Note: async/await and ConfigureAwait(false) is used when making calls external to the service.
				//       See https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/
				//       See also https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.task.configureawait
				//       See also https://blogs.msdn.microsoft.com/benwilli/2017/02/09/an-alternative-to-configureawaitfalse-everywhere/
				//       See also https://blog.stephencleary.com/2012/07/dont-block-on-async-code.html
				//       Warning: Improper use of the tasks can cause deadlocks and performance issues within an application.
				var workspaceIDs = await _helper.GetDBContext(-1).ExecuteEnumerableAsync(
					new ContextQuery
					{
						SqlStatement = @"SELECT TOP (@limit) [ArtifactID] FROM [Case] WHERE [ArtifactID] > 0 AND [Name] LIKE '%'+@workspaceName+'%'",
						Parameters = new[]
						{
							new SqlParameter("@limit", limit), 
							new SqlParameter("@workspaceName", queryString)
						}
					}, (record, cancel) => Task.FromResult(record.GetInt32(0))).ConfigureAwait(false);

				foreach (int workspaceID in workspaceIDs)
		{
					// Loop through the results and use the proxy to call another service for more information.
					// Note: async/await and ConfigureAwait(false) is used when making calls external to the service.
					//       See https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/
					//       See also https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.task.configureawait
					//       See also https://blogs.msdn.microsoft.com/benwilli/2017/02/09/an-alternative-to-configureawaitfalse-everywhere/
					//       See also https://blog.stephencleary.com/2012/07/dont-block-on-async-code.html
					//       Warning: Improper use of the tasks can cause deadlocks and performance issues within an application.
					$ext_ServiceName$Model wsModel = await proxy.GetWorkspaceNameAsync(workspaceID).ConfigureAwait(false);
					if (wsModel != null)
					{
						models.Add(wsModel);
					}
				}
			}
			catch (Exception exception)
			{
				// Note: logging templates should never use interpolation! Doing so will cause memory leaks. 
				_logger.LogWarning(exception, "An exception occured during query for workspace(s) containing {QueryString}.", queryString);

				// Throwing a user defined exception with a 404 status code.
				throw new $ext_ServiceName$Exception($"An exception occured during query for workspace(s) containing {queryString}.");
			}

			return models;
		}

		/// <summary>
		/// All Kepler services must inherit from IDisposable.
		/// Use this dispose method to dispose of any unmanaged memory at this point.
		/// See https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose for examples of how to properly use the dispose pattern.
		/// </summary>
		public void Dispose()
		{ }
	}
}
