using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using RelativityWizard;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RelativityWizard
{
	public class ModuleWizard : IWizard
	{
		private InputFormModule inputForm;

		// This method is called before opening any item that
		// has the OpenInEditor attribute.
		public void BeforeOpeningFile(ProjectItem projectItem)
		{
		}

		public void ProjectFinishedGenerating(Project project)
		{
		}

		// This method is only called for item templates,
		// not for project templates.
		public void ProjectItemFinishedGenerating(ProjectItem
				projectItem)
		{
		}

		// This method is called after the project is created.
		public void RunFinished()
		{
		}

		public void RunStarted(object automationObject,
				Dictionary<string, string> replacementsDictionary,
				WizardRunKind runKind, object[] customParams)
		{
			try
			{
				// Display a form to the user. The form collects
				// input for the custom message.
				inputForm = new InputFormModule();
				inputForm.ShowDialog();

				// Add custom parameters.
				replacementsDictionary.Add("$ServiceModule$", inputForm.ServiceModule);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		// This method is only called for item templates,
		// not for project templates.
		public bool ShouldAddProjectItem(string filePath)
		{
			return true;
		}
	}
}
