using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace FileSearch
{
    public partial class MainWindow : Form
    {
        private List<DriveInfo> drives = DriveInfo.GetDrives().ToList();
        private List<DirectoryInfo> directories = new List<DirectoryInfo>();
        private List<DirectoryInfo> subDirectories = new List<DirectoryInfo>();
        private List<FileInfo> files = new List<FileInfo>();

        private string[] sysPaths =
        {
            "Config.Msi"
        };

        public MainWindow()
        {
            InitializeComponent();
            ShowUI();
            ShowFiles();
            //ShowDirectories();
        }

        // вызов

        private void ShowFiles()
        {
            Thread loadFiles = new Thread(LoadFiles);
            //loadFiles.Start();
            loadFiles.IsBackground = true;
        }

        private void ShowUI()
        {
            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.HoverSelection = true;
            listView1.Cursor = Cursors.Hand;
        }

        private void ShowDirectories()
        {
            Thread showDirectory = new Thread(ShowDirectory);
            showDirectory.Start();
            showDirectory.IsBackground = true;
        }

        //private void OnSearch()
        //{
        //    Thread search = new Thread(StartSearch);
        //    search.Start();
        //    search.IsBackground = true;
        //}

        //  отображение файлов и при поиске

        private void LoadFiles()
        {
            try
            {
                foreach (var drive in drives)
                {
                    if (drive.IsReady)
                    {
                        foreach (var directory in drive.RootDirectory.GetDirectories())
                        {
                            if (directory != null && !sysPaths.Contains(directory.Name))
                            {
                                RecursionFiles(directory);
                                directories.Add(directory);
                                subDirectories.AddRange(directory.GetDirectories());
                            }
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
            }
        }

        // рекурсивный поиск файлов

        private void RecursionFiles(DirectoryInfo directory)
        {
            try
            {
                foreach (var file in directory.GetFiles())
                {
                    if (!sysPaths.Contains(file.Name))
                    {
                        //files.Add(file);
                        ListViewItem item = new ListViewItem(file.Name);
                        item.SubItems.Add(file.FullName);
                        item.SubItems.Add((file.Length / 1024) + " KB");
                        item.SubItems.Add(file.LastWriteTime.ToString());
                        item.SubItems.Add(file.Extension);
                        listView1.Items.Add(item);
                    }
                }
                foreach (var dir in directory.GetDirectories())
                {
                    //RecursionFiles(dir); // поддиректории показывает
                }
            }
            catch (UnauthorizedAccessException)
            {
            }
        }

        // показать директории при поиске

        private void ShowDirectory()
        {
            textBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection autoComplete = new AutoCompleteStringCollection();
            textBox1.AutoCompleteCustomSource = autoComplete;

            try
            {
                foreach (var drive in drives)
                {
                    if (!drive.IsReady) continue;

                    RecursionDirectories(drive.RootDirectory, autoComplete);
                    autoComplete.Add(drive.RootDirectory.FullName);

                }
            }
            catch (UnauthorizedAccessException)
            {
            }
        }

        private void RecursionDirectories(DirectoryInfo directory, AutoCompleteStringCollection autoComplete)
        {
            try
            {
                if (directory == null || sysPaths.Contains(directory.Name)) return;
                autoComplete.Add(directory.FullName);

                foreach (var subDir in directory.GetDirectories())
                {
                    RecursionDirectories(subDir, autoComplete);
                    autoComplete.Add(subDir.FullName);
                }
            }
            catch (UnauthorizedAccessException)
            {
            }
        }


        public void MainWindow_Load()
        {


        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            //OnSearch();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}