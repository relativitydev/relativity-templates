using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Relativity.API;
using Relativity.CustomPages;
using kCura.Relativity.Client;

namespace $rootnamespace$
{
	public class $safeitemname$ : Controller
	{
		//
		// GET: /Default1/

		public ActionResult Index()
		{
			//Available Session variables
			var userID = Session["UserID"];
			var email = Session["Email"];
			var workspaceID = Session["WorkspaceID"];
			var firstName = Session["FirstName"];
			var lastName = Session["LastName"];
			try
			{

				/*
          
        //Setting up an RSAPI Client
        using (IRSAPIClient proxy =
					Relativity.CustomPages.ConnectionHelper.Helper().GetServicesManager().CreateProxy<kCura.Relativity.Client.IRSAPIClient>(Relativity.API.ExecutionIdentity.System))
        {
          //Add code for working with RSAPIClient
        }
                                  
        //Get a dbContext for the EDDS database
        Relativity.API.IDBContext eddsDBContext = Relativity.CustomPages.ConnectionHelper.Helper().GetDBContext(-1);
				
        */

			}
			catch (System.Exception ex)
			{
			}


			return View();
		}

	}
}
