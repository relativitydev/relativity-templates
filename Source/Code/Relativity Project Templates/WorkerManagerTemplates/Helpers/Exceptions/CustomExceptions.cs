
namespace Helpers.Exceptions
{
	[System.Serializable]
	public class CustomRelativityAgentException : System.Exception
	{
		public CustomRelativityAgentException()
		{
		}

		public CustomRelativityAgentException(string message)
			: base(message)
		{
		}

		public CustomRelativityAgentException(string message, System.Exception inner)
			: base(message, inner)
		{
		}

		// A constructor is needed for serialization when an
		// exception propagates from a remoting server to the client. 
		protected CustomRelativityAgentException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
		{
		}
	}
}
