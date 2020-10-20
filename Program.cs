using System;
using System.Windows.Forms;

namespace Tachi
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TachiApp());
            Tachi.TachiMain();
        }
    }
}
