using System;
using System.Net;
using System.Runtime.Serialization;
using Relativity.Services.Exceptions;

namespace $rootnamespace$
{
	[ExceptionIdentifier("$guid1$")]
	[FaultCode(HttpStatusCode.NotFound)]
	public class $safeitemrootname$ : ServiceException
	{
		public $safeitemrootname$()
			: base()
		{ }

		public $safeitemrootname$(string message)
			: base(message)
		{ }

		public $safeitemrootname$(string message, Exception innerException)
			: base(message, innerException)
		{ }
	}
}