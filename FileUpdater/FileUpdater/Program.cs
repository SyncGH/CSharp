using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using FileUpdater.Libs;
using System.IO;

namespace FileUpdater
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
            //  Application.Run(new Loader());

            Updater obj1 = new Updater();
            obj1.checkVersion();

            if (File.Exists("FileUpdater.exe.old"))
            {
                File.Delete("FileUpdater.exe.old");
            }


        }
    }
}
