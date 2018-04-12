using System;

namespace Relativity_Extension.Helpers
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
			public static readonly Guid ApplicationGuid = new Guid("EEC7752D-EC38-42A2-91D4-CCEE6C7744B2");
			public static readonly Guid ManagerQueueTab = new Guid("03910743-44A0-4A3C-9DFD-5DE9B7BED9FE");
			public static readonly Guid WorkerQueueTab = new Guid("CB2946FB-0B67-4785-A782-43356A9EA562");
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
