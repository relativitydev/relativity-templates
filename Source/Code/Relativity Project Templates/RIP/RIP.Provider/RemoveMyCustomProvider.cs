using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using kCura.EventHandler;
using kCura.IntegrationPoints.SourceProviderInstaller;

namespace RIP.Provider
{
    [kCura.EventHandler.CustomAttributes.Description("My Custom Provider - Uninstall")]
    [kCura.EventHandler.CustomAttributes.RunOnce(false)]
    [Guid("3113E395-E8C9-4CF8-883D-8E3BF35C0A19")]
    public class RemoveMyCustomProvider : kCura.IntegrationPoints.SourceProviderInstaller.IntegrationPointSourceProviderUninstaller
    {
        public RemoveMyCustomProvider()
        {
            // Subscribe to Pre and Post UnInstall Events to cleanup environment after your provider is removed
            RaisePreUninstallPreExecuteEvent += PreUninstall;
            RaisePreUninstallPostExecuteEvent += PostUninstall;
            
        }

        public void PreUninstall()
        {
            // Execute a command before your provider is Uninstalled
            //this.Helper.GetDBContext(Helper.GetActiveCaseID()).ExecuteNonQuerySQLStatement("DROP TABLE [EDDSDBO].MyScratchTableForMyCustomProvider");
        }

        public void PostUninstall(Boolean isUninstalled, Exception ex)
        {
            // Execute a command after your provider is Uninstalled
            //this.Helper.GetDBContext(Helper.GetActiveCaseID()).ExecuteNonQuerySQLStatement("DROP TABLE [EDDSDBO].MyJobTableForMyCustomProvider");
        }
    }
}
