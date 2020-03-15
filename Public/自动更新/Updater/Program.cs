using System;
using System.Windows.Forms;

namespace AutoUpdate
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new FrmUpdate(args[0].Trim()));
                }
            }
        }
    }
}
