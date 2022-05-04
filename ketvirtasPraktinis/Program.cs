using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ketvirtasPraktinis
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FormLogin formLogin = new FormLogin();
            Application.Run(formLogin);
            if (formLogin.Login == true)
            {
                FormHome formHome = new FormHome();
                Application.Run(formHome);
            }
        }
    }
}
