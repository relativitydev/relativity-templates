using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$
{
    public class Constants
    {
        public class Guids
        {
            public class Provider
            {
                public const String MY_CUSTOM_PROVIDER = "$guid1$";
            }

            public class Application
            {
				// Replace with the Guid of the Relativity Application in which your RIP Provider will be attached
                public static Guid SMP_RELATIVITY_APPLICATION = new Guid("$guid1$");
            }
        }
    }
}
