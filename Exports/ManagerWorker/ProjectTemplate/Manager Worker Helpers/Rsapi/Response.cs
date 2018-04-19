using System;
using System.Collections.Generic;
using System.Linq;
using kCura.Relativity.Client;
using kCura.Relativity.Client.DTOs;

namespace Relativity_Extension.$safeprojectname$.Rsapi
{
	public class Response<TResultType>
	{
		public String Message { get; set; }
		public Boolean Success { get; set; }
		public TResultType Results { get; set; }

		//Do not convert to async
		public static Response<IEnumerable<RDO>> CompileQuerySubsets(IRSAPIClient proxy, QueryResultSet<RDO> theseResults)
		{
			Boolean success = true;
			String message = "";
			List<Result<RDO>> resultList = new List<Result<RDO>>();
			Int32 iterator = 0;

			message += theseResults.Message;
			if (!theseResults.Success)
			{
				success = false;
			}

			resultList.AddRange(theseResults.Results);
			if (!String.IsNullOrEmpty(theseResults.QueryToken))
			{
				String queryToken = theseResults.QueryToken;
				int batchSize = theseResults.Results.Count();
				iterator += batchSize;
				do
				{
					theseResults = proxy.Repositories.RDO.QuerySubset(queryToken, iterator + 1, batchSize);
					resultList.AddRange(theseResults.Results);
					message += theseResults.Message;
					if (!theseResults.Success)
					{
						success = false;
					}
					iterator += batchSize;
				} while (iterator < theseResults.TotalCount);
			}

			Response<IEnumerable<RDO>> res = new Response<IEnumerable<RDO>>
			{
				Results = resultList.Select(x => x.Artifact),
				Success = success,
				Message = MessageFormatter.FormatMessage(resultList.Select(x => x.Message).ToList(), message, success)
			};
			return res;
		}

		//Do not convert to async
		public static Response<IEnumerable<RDO>> CompileWriteResults(WriteResultSet<RDO> theseResults)
		{
			Boolean success = true;
			String message = "";

			message += theseResults.Message;
			if (!theseResults.Success)
			{
				success = false;
			}

			Response<IEnumerable<RDO>> res = new Response<IEnumerable<RDO>>
			{
				Results = theseResults.Results.Select(x => x.Artifact),
				Success = success,
				Message = MessageFormatter.FormatMessage(theseResults.Results.Select(x => x.Message).ToList(), message, success)
			};
			return res;
		}

		//Do not convert to async
		public static Response<IEnumerable<Error>> CompileWriteResults(WriteResultSet<Error> theseResults)
		{
			Boolean success = true;
			String message = "";

			message += theseResults.Message;
			if (!theseResults.Success)
			{
				success = false;
			}

			Response<IEnumerable<Error>> res = new Response<IEnumerable<Error>>
			{
				Results = theseResults.Results.Select(x => x.Artifact),
				Success = success,
				Message = MessageFormatter.FormatMessage(theseResults.Results.Select(x => x.Message).ToList(), message, success)
			};
			return res;
		}
	}
}
