using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using kCura.EventHandler.CustomAttributes;
using kCura.IntegrationPoints.Contracts;
using kCura.IntegrationPoints.SourceProviderInstaller;

namespace $rootnamespace$
{
    [Description("My Custom Provider - Installer")]
    [RunOnce(false)]
    [Guid("$guid1$")]
    public class $safeitemname$ : IntegrationPointSourceProviderInstaller
    {
        public $safeitemname$()
        {
            RaisePostInstallPreExecuteEvent += PreInstall;
            RaisePostInstallPostExecuteEvent += PostInstall;
        }

        public override IDictionary<Guid, SourceProvider> GetSourceProviders()
        {
            Dictionary<Guid, SourceProvider> sourceProviders = new Dictionary<Guid, SourceProvider>();

            // Register the name, custom page location and configuration location of your provider
            SourceProvider myCustomProvider = new SourceProvider
            {
                Name = "My Custom Provider",
                Url = $"/%applicationpath%/CustomPages/{Constants.Guids.Application.SMP_RELATIVITY_APPLICATION}/MyCustomProvider/Index/",
                ViewDataUrl = $"/%applicationpath%/CustomPages/{Constants.Guids.Application.SMP_RELATIVITY_APPLICATION}/MyCustomProvider/GetViewFields/"
            };

            sourceProviders.Add(new Guid(Constants.Guids.Provider.MY_CUSTOM_PROVIDER), myCustomProvider);

            return sourceProviders;
        }

        public void PreInstall()
        {
            // Execute a command before your provider is installed
            //this.Helper.GetDBContext(Helper.GetActiveCaseID()).ExecuteNonQuerySQLStatement("CREATE TABLE [EDDSDBO].MyScratchTableForMyCustomProvider ([ID] INT)");
            //this.Helper.GetDBContext(Helper.GetActiveCaseID()).ExecuteNonQuerySQLStatement("CREATE TABLE [EDDSDBO].MyJobTableForMyCustomProvider ([ID] INT)");
        }

        public void PostInstall(bool isUninstalled, Exception ex)
        {
            // Execute a command after your provider is installed
            //this.Helper.GetDBContext(Helper.GetActiveCaseID()).ExecuteNonQuerySQLStatement("INSERT INTO [EDDSDBO].MyScratchTableForMyCustomProvider(ID)VALUES(1)");
        }
    }
}