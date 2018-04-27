using System;
using System.Runtime.InteropServices;
using kCura.EventHandler.CustomAttributes;
using kCura.IntegrationPoints.SourceProviderInstaller;

namespace RIP.Provider
{
    [Description("My Custom Provider - Uninstall")]
    [RunOnce(false)]
    [Guid("3113E395-E8C9-4CF8-883D-8E3BF35C0A19")]
    public class RemoveMyCustomProvider : IntegrationPointSourceProviderUninstaller
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

        public void PostUninstall(bool isUninstalled, Exception ex)
        {
            // Execute a command after your provider is Uninstalled
            //this.Helper.GetDBContext(Helper.GetActiveCaseID()).ExecuteNonQuerySQLStatement("DROP TABLE [EDDSDBO].MyJobTableForMyCustomProvider");
        }
    }
}