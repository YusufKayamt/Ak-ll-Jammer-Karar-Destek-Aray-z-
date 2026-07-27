using System;
using System.Windows.Forms;

namespace Akıllı_Jammer_Karar_Destek_Arayüzü
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}