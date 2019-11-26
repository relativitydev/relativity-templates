using System;
using System.Threading.Tasks;
using Relativity.API;
using Relativity.API.Context;
using Relativity.Kepler.Logging;
using Relativity.Kepler.Services;

namespace $rootnamespace$
{
    public class $safeitemrootname$ : I$safeitemrootname$
    {
				private IHelper _helper;
				private ILog _logger;

				public $safeitemrootname$(IHelper helper, ILog logger)
        {
            _logger = logger.ForContext <$safeitemrootname$>();
            _helper = helper;
        }

        public Task $safeitemrootname$Async()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        { }
    }
}