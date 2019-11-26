using Relativity.Kepler.Wizard;
using System;
using System.Windows.Forms;

namespace RelativityWizard
{
	public partial class InputFormProject : Form
	{
		public string ServiceName { get; set; }
		public string ServiceModule { get; set; }

		public InputFormProject()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			ServiceModule = textBoxServiceModule.SanitizedText();
			ServiceName = textBoxServiceName.SanitizedText();

			if (Utilities.ValidateModule(ServiceModule)
					&& Utilities.ValidateService(ServiceName))
			{
				Close();
			}
		}
	}
}
