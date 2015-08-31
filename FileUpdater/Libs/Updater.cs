using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;

namespace FileUpdater.Libs
{
    
    class Updater
    {
        public const string VersionCheckURL = "http://shyn.bugs3.com/version.txt";
        public const string LoaderDownloadURL = "http://shyn.bugs3.com/FileUpdater.exe";
        public const string LocalFile = @".\version.txt";
        public const string LocalLoader = @".\FileUpdater.exe";

        public bool UpdatesChecked = false;
        string onlineVersion = string.Empty;
        public static  string localVersion = string.Empty;


        public void checkVersion()
        {
            try
            { 

            WebClient client = new WebClient();
            client.DownloadFile(VersionCheckURL, LocalFile);

            client.Dispose();
                
              if (File.Exists(LocalFile))
                {
                    using (FileStream file = new FileStream("version.txt", FileMode.Open, FileAccess.Read, FileShare.Read))
                    {

                        using (StreamReader reader = new StreamReader(file))
                        {
                            onlineVersion = (string)reader.ReadLine().Trim();
                            reader.Close();
                        }
                        file.Close();
                        

                    } //EndofStream

                   localVersion = Assembly.GetEntryAssembly().GetName().Version.ToString();

                    if (localVersion != onlineVersion)
                    {
                        MessageBox.Show("A different version has been found!\n New Version: " + onlineVersion + "\n Current Version: " + localVersion, "Updater", MessageBoxButtons.OK);
                       DownloadNewLoader();
                 
                    }
              
                    else
                    {
                        MessageBox.Show("Loader lastest version is already installed.", "Updater", MessageBoxButtons.OK);
                    }
                }

                UI ui1 = new UI();
                ui1.ShowDialog();
            }

            catch (Exception)
            {
                MessageBox.Show("Connection has failed. \n Please try again later.","File Updater");
            }

        }

        public void DownloadNewLoader()
        {
            try
            {
                if (File.Exists(LocalLoader))
                {
                    if (File.Exists(LocalLoader + ".old"))
                    {
                        File.Delete(LocalLoader + ".old");
                    }

                    File.Move(LocalLoader, LocalLoader + ".old");
                    WebClient downloadclient = new WebClient();
                    downloadclient.DownloadFile(LoaderDownloadURL, LocalLoader);
                    RestartApp();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());  
            }

        }

        private void RestartApp()
        {
            Process.Start(Application.StartupPath + "\\FileUpdater.exe");
            Process.GetCurrentProcess().Kill();
        }






    }
}
