using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileUpdater.Util;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.ComponentModel;

namespace FileUpdater.Libs
{
    class FileUpdate
    {
        public static readonly string CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;

        private readonly Uri uri = new Uri("http://shyn.bugs3.com/Update/");
        private const string OnlineList = "http://shyn.bugs3.com/list.txt";
        private const string LocalList = @".\list.txt";

        //ToRead the online and local version. 
        private string onlineVersion = string.Empty;
        private string localVersion = string.Empty;
        int index = 0;
        
        public List<string> files = new List<string>();   //List of checked files.


        private List<string> CreateList(string fileToCheck)
        {
            List<string> lines = new List<string>();
            using (FileStream file = new FileStream(fileToCheck, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                string[] arrayLines = File.ReadAllLines(fileToCheck);
                foreach (string ln in arrayLines)
                {
                    lines.Add(ln);
                }
                file.Dispose();
            
            }
            return lines; 
        }



        public void CheckForFiles()
        {
            try
            {
                WebClient client = new WebClient();
                client.DownloadFile(OnlineList, LocalList);

                List<string> OnlineListTxt = CreateList(LocalList);

                foreach (string ln in OnlineListTxt)
                {
                    string[] fileInfo = ln.Split(new string[] { "|" }, 4, 0);
                    string _file = fileInfo[0];
                    string _path = fileInfo[2];
                    string md5 = fileInfo[3];

                    if (_path == "")
                    {
                        
                        string md5check = Util.Util.Md5Checksum(_file);
                        if (md5check != md5 || md5check == "-1")
                        {
                            files.Add(ln);
                            PrintInfo(_file, md5check);
                        }
                    }
                    else
                    {
                        string drc = Path.Combine(CurrentDirectory, fileInfo[2] + "\\");
                        string pathh = _path + _file;
                        string md5check = Util.Util.Md5Checksum(drc + _file);
                        if (md5check != md5 || md5check == "-1")
                        {
                            files.Add(ln);
                            PrintInfo(_file, md5check);
                        }
                    }
                }
                    
                } 

            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

        } 


        public void ParseFiles()
        {
            foreach (string ln in files)
            {
                string[] fileInfo = ln.Split(new string[] { "|" }, 4, 0);
                string drc = Path.Combine(CurrentDirectory, fileInfo[2] + "\\");

                if (fileInfo[2] == "")
                
                    DownloadFile(fileInfo[0], "");
                else
                    DownloadFile(fileInfo[0], drc);
            }
        }

        private void PrintInfo(string _file, string _md5)
        {
            Console.WriteLine("File: " + _file + " Md5: " + _md5);
        }


        public void DownloadFile(string _file, string _path)
        {
            try
            {
                    WebClient file = new WebClient();
                    file.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                    file.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadCompleted);

                    if (!Directory.Exists(_path) && (_path != ""))
                    {
                        Directory.CreateDirectory(_path);
                    }

                if (_path == "")
                        file.DownloadFileAsync(new Uri(uri + _file), _file); 
                else

                       file.DownloadFileAsync(new Uri(uri + _file), _path + _file);
                    

                
                   
                    var currItem = UI.lvItems.Items[index];
                    currItem.SubItems[3].Text = "Downloading";
                  


            }

            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

        }

        private void DownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
           
            ListViewItem current = UI.lvItems.Items[index];
            current.SubItems[3].Text = "Installed";
            index += 1;
            UI.progressBar1.Increment(100);
            

         }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            
            UI.progressBar1.Value = e.ProgressPercentage;
            
        }

    }
}
