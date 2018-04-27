using System;

namespace Helpers
{
	public class Constant
	{
		public class Tables
		{
			public static readonly String WorkerQueue = "SampleWorkerQueue";
			public static readonly String ManagerQueue = "SampleManagerQueue";
			public static readonly String ErrorLog = "SampleErrorLog";
		}

		public class Names
		{
			public static readonly String ApplicationName = "Sample Application";
			public static readonly String TablePrefix = "SampleApp_";
		}

		public class Guids
		{
			public static readonly Guid ApplicationGuid = new Guid("8DF88F3C-5795-4C02-9CDF-5E826D603B13");
			public static readonly Guid ManagerQueueTab = new Guid("1CC1A982-6F61-42F6-9F65-DE2C522EC0F2");
			public static readonly Guid WorkerQueueTab = new Guid("96D453C3-A512-4B1F-BE95-BEE52AE6176E");
		}

		public class Sizes
		{
			public static readonly Int32 BatchSize = 1000;
		}

		public class QueueStatus
		{
			public static readonly Int32 NotStarted = 0;
			public static readonly Int32 InProgress = 1;
			public static readonly Int32 Error = -1;
		}

		public class Messages
		{
			public const String PRIORITY_REQUIRED = "Please enter a priority";
			public const String ARTIFACT_ID_REQUIRED = "Please enter an artifact ID";
			public const String AGENT_OFF_HOURS_NOT_FOUND = "No agent off-hours found in the configuration table.";
			public const String AGENT_OFF_HOUR_TIMEFORMAT_INCORRECT = "Please verify that the EDDS.Configuration AgentOffHourStartTime & AgentOffHourEndTime is in the following format HH:MM:SS";
		}

		public class Buttons
		{
			public const String ADD = "add";
			public const String REMOVE = "remove";
		}
	}
}
