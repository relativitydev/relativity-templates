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
                public const String MY_CUSTOM_PROVIDER = "D7A00988-9F1D-4440-83B2-E6C41F126690";
            }

            public class Application
            {
                // Replace with the Guid of the Relativity Application in which your RIP Provider will be attached
                public static Guid SMP_RELATIVITY_APPLICATION = new Guid("125D8DE8-85F7-47BA-BE16-546A6BC15432");
            }
        }
    }
}
