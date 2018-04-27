using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TemplateWizard;

namespace RelativityWizard
{
    public class ChildWizard : IWizard
    {

        // Retrieve global replacement parameters
        public void RunStarted(object automationObject,
                Dictionary<string, string> replacementsDictionary,
                WizardRunKind runKind, object[] customParams)
        {
            // Add custom parameters.
            replacementsDictionary.Add("$saferootprojectname$",
                    RootWizard.GlobalDictionary["$saferootprojectname$"]);
            replacementsDictionary.Add("$rootprojectname$",
                    RootWizard.GlobalDictionary["$rootprojectname$"]);
        }

        #region IWizard Members

        public void BeforeOpeningFile(EnvDTE.ProjectItem projectItem)
        {

        }

        public void ProjectFinishedGenerating(EnvDTE.Project project)
        {

        }

        public void ProjectItemFinishedGenerating(EnvDTE.ProjectItem projectItem)
        {

        }

        public void RunFinished()
        {

        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }

        #endregion
    }
}
