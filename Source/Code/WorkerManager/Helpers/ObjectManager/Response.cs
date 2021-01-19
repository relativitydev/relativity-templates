using System;
using System.Collections.Generic;
using System.Linq;
using kCura.Relativity.Client;
using kCura.Relativity.Client.DTOs;

namespace Helpers.ObjectManager
{
	public class Response<TResultType>
	{
		public String Message { get; set; }
		public Boolean Success { get; set; }
		public TResultType Results { get; set; }


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
