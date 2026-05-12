using System;
using System.Windows.Forms;
using TixNova_Final;   

namespace TixNova__Final
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            
            using (LoginForm login = new LoginForm())
            {
                if (login.ShowDialog() == DialogResult.OK)
                {
                    
                    Application.Run(new MainDashBoard());
                }
                
            }
        }
    }
}