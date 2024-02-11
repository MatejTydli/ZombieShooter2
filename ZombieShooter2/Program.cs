using System;
using System.Windows.Forms;

namespace ZombieShooter2
{
	static class Program
	{
		internal static Form1 mainForm;

		[STAThread]
		static void Main()
		{
			Application.SetHighDpiMode(HighDpiMode.SystemAware);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			
			Media.Initialize();
			Game.Initialize();
			mainForm = new Form1();
			Application.Run(mainForm);
		}
	}
}
