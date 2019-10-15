using System;
using System.Windows.Forms;

namespace Relativity.Kepler.Wizard
{
	public partial class InputFormService : Form
    {
	    public string ServiceName { get; set; }

		public InputFormService()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			ServiceName = textBoxServiceName.SanitizedText();

            if (Utilities.ValidateService(ServiceName))
            {
                Close();
            }
        }
    }
}
