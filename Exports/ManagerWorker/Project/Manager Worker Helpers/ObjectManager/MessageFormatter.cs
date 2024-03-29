﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace $safeprojectname$.ObjectManager
{
	public class MessageFormatter
	{
		//Do not convert to async
		public static String FormatMessage(List<String> results, String message, Boolean success)
		{
			String messageList = "";

			if (!success)
			{
				messageList = message;
				results.ToList().ForEach(w => messageList += (w));
			}

			return messageList;
		}
	}
}
