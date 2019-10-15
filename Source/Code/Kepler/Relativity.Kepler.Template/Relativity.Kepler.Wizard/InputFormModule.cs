using System;
using System.Windows.Forms;

namespace Relativity.Kepler.Wizard
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
