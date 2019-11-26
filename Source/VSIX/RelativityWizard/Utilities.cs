using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Relativity.Kepler.Wizard
{
	internal static class Utilities
	{
		private static List<string> blackList = new List<string>() { "Relativity", "kCura" };

		public static bool ValidateService(string service)
		{
			bool isOk = true;

			if (string.IsNullOrWhiteSpace(service))
			{
				isOk = false;
				MessageBox.Show($"Service cannot be empty. The current value is '{service}'");
			}

			return isOk;
		}

		public static bool ValidateModule(string module)
		{
			bool isOk = true;

			if (!string.IsNullOrEmpty(module))
			{
				foreach (string notAllowed in blackList)
				{
					if (module.ToLower().Contains(notAllowed.ToLower()))
					{
						isOk = false;
						MessageBox.Show($"Module cannot contain the string {notAllowed}. The current value is '{module}'");
					}
				}
			}
			else
			{
				isOk = false;
				MessageBox.Show($"Module cannot be empty. The current value is '{module}'");
			}

			return isOk;
		}

		public static string SanitizedText(this TextBox input)
		{
			StringBuilder sb = new StringBuilder();
			foreach (var c in input.Text)
			{
				if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z' || (c >= 'a' && c <= 'z') || c == '_'))
				{
					sb.Append(c);
				}
				else if (c == ' ')
				{
					sb.Append('_');
				}
			}
			return sb.ToString();
		}

	}
}
