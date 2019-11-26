using System;
using System.Threading.Tasks;
using Relativity.Kepler.Services;

namespace $rootnamespace$
{
	[WebService("$ServiceName$ Service")]
	[ServiceAudience(Audience.Public)]
	[RoutePrefix("$ServiceName$")]
	public interface $safeitemrootname$ : IDisposable
	{
		[HttpPost]
		[Route("")]
		Task $safeitemrootname$Async();
	}
}