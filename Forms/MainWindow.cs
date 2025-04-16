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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace FileSearch
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
            ShowUI();
            ShowFiles();
            ShowDirectories();
        }

        private void ShowFiles()
        {
            Thread loadFiles = new Thread(LoadFiles);
            //loadFiles.Start();
            loadFiles.IsBackground = true;
        }

        private void ShowDirectories()
        {
            Thread showDirectory = new Thread(ShowDirectory);
            showDirectory.Start();
            showDirectory.IsBackground = true;
        }

        private void ShowUI()
        {
            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.HoverSelection = true;
            listView1.Cursor = Cursors.Hand;
        }

        private void LoadFiles()
        {
            List<DriveInfo> drives = DriveInfo.GetDrives().ToList();
            List<DirectoryInfo> directories = new List<DirectoryInfo>();
            List<DirectoryInfo> subDirectories = new List<DirectoryInfo>();
            List<FileInfo> files = new List<FileInfo>();

            string[] sysPaths = { "Config.Msi" };

            try
            {
                foreach (DriveInfo drive in drives)
                {
                    foreach (DirectoryInfo directoryInfo in drive.RootDirectory.GetDirectories())
                    {
                        if (directoryInfo.Attributes.HasFlag(FileAttributes.System) && !sysPaths.Contains(directoryInfo.Name))
                        {
                            continue;
                        }
                        if (sysPaths.Contains(directoryInfo.Name))
                        {
                            continue;
                        }

                        foreach (FileInfo file in directoryInfo.GetFiles())
                        {
                            if (file.Attributes.HasFlag(FileAttributes.System) && !sysPaths.Contains(file.Name))
                            {
                                continue;
                            }
                            if (sysPaths.Contains(file.Name))
                            {
                                continue;
                            }
                            ListViewItem item = new ListViewItem(file.Name);
                            item.SubItems.Add(file.FullName);
                            item.SubItems.Add((file.Length / 1024) + " KB");
                            item.SubItems.Add(file.LastWriteTime.ToString());
                            item.SubItems.Add(file.Extension);
                            listView1.Items.Add(item);
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void ShowDirectory()
        {
            textBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection autoComplete = new AutoCompleteStringCollection();
            textBox1.AutoCompleteCustomSource = autoComplete;

            List<DriveInfo> drives = DriveInfo.GetDrives().ToList();
            List<DirectoryInfo> directories = new List<DirectoryInfo>();
            List<DirectoryInfo> subDirectories = new List<DirectoryInfo>();
            List<FileInfo> files = new List<FileInfo>();

            string[] sysPaths = { "Config.Msi" };

            try
            {
                foreach (DriveInfo drive in drives)
                {
                    foreach (DirectoryInfo directoryInfo in drive.RootDirectory.GetDirectories())
                    {
                        if (directoryInfo.Attributes.HasFlag(FileAttributes.System) && !sysPaths.Contains(directoryInfo.Name))
                        {
                            continue;
                        }
                        if (sysPaths.Contains(directoryInfo.Name))
                        {
                            continue;
                        }
                        directories.Add(directoryInfo);
                        autoComplete.Add(directoryInfo.FullName);
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        public void MainWindow_Load()
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void StartSearch()
        {
            string path = textBox1.Text;

            if (!Directory.Exists(path))
            {
                MessageBox.Show("Указанная директория не существует.");
                return;
            }

            listView1.Items.Clear();

            List<DriveInfo> drives = DriveInfo.GetDrives().ToList();
            List<DirectoryInfo> directories = new List<DirectoryInfo>();
            List<DirectoryInfo> subDirectories = new List<DirectoryInfo>();
            List<FileInfo> files = new List<FileInfo>();

            string[] sysPaths = { "Config.Msi" };

            try
            {
                foreach (DriveInfo drive in drives)
                {
                    foreach (DirectoryInfo directoryInfo in drive.RootDirectory.GetDirectories())
                    {
                        if (directoryInfo.Attributes.HasFlag(FileAttributes.System) && !sysPaths.Contains(directoryInfo.Name))
                        {
                            continue;
                        }
                        if (sysPaths.Contains(directoryInfo.Name))
                        {
                            continue;
                        }
                        directories.Add(directoryInfo);
                    }
                }

                foreach (DirectoryInfo directory in directories)
                {
                    if (directory.FullName == path)
                    {
                        subDirectories.Add(directory);
                    }
                }

                foreach (DirectoryInfo directory in subDirectories)
                {
                    foreach (FileInfo file in directory.GetFiles())
                    {
                        if (file.Attributes.HasFlag(FileAttributes.System) && !sysPaths.Contains(file.Name))
                        {
                            continue;
                        }
                        if (sysPaths.Contains(file.Name))
                        {
                            continue;
                        }
                        ListViewItem item = new ListViewItem(file.Name);
                        item.SubItems.Add(file.FullName);
                        item.SubItems.Add((file.Length / 1024) + " KB");
                        item.SubItems.Add(file.LastWriteTime.ToString());
                        item.SubItems.Add(file.Extension);
                        listView1.Items.Add(item);
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
            MessageBox.Show("Поиск успешно завершен.");
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            StartSearch();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.Multiline = false;
        }
    }
}
