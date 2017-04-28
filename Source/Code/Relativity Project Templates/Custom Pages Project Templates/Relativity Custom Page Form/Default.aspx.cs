using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using kCura.Relativity.Client;
using Relativity.API;
using Relativity.CustomPages;

namespace $safeprojectname$
{
	public partial class Default : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
				//Available Session variables
				var userID = Session["UserID"];
				var email = Session["Email"];
				var workspaceID = Session["WorkspaceID"];
				var firstName = Session["FirstName"];
				var lastName = Session["LastName"];
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
	}
}