using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FileUpdater.Libs;

namespace FileUpdater
{
    public partial class UI : MetroFramework.Forms.MetroForm
    {

        FileUpdate fileupd1 = new FileUpdate(); // ---> FileUpdate Main Object!
        public UI()
        {
            InitializeComponent();
        }

        private void btnInstall_Click(object sender, EventArgs e)
        {
            
            try {
   
            lvItems.Items.Clear();
            fileupd1.files.Clear();

            fileupd1.CheckForFiles();

                 foreach (string line in fileupd1.files)
                {
                    string[] fileInfo = line.Split(new string[] { "|" }, 4, 0);
                    ListViewItem lvItem = new ListViewItem(new string[] { fileInfo[0], fileInfo[1], fileInfo[2], "New version" });
                    lvItems.Items.Add(lvItem);
                    Console.WriteLine(line);
                }

                buttonInstall.Enabled = true;
                buttonCheck.Enabled = false;
            }

            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvItems.Items)
            {
                Console.WriteLine(item.ToString());
            }

            fileupd1.ParseFiles();
            buttonInstall.Enabled = false;

        }

        private void lvItems_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void UI_Load(object sender, EventArgs e)
        {

        }

    }
}
