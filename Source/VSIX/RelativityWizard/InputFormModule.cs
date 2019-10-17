using Relativity.Kepler.Wizard;
using System;
using System.Windows.Forms;

namespace RelativityWizard
{
	public partial class InputFormModule : Form
	{
		public string ServiceModule { get; set; }

		public InputFormModule()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			ServiceModule = textBoxServiceModule.SanitizedText();

			if (Utilities.ValidateModule(ServiceModule))
			{
				Close();
			}
		}
	}
}
