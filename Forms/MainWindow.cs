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
        private List<DriveInfo> drives = DriveInfo.GetDrives().ToList();
        private List<DirectoryInfo> directories = new List<DirectoryInfo>();
        private List<DirectoryInfo> subDirectories = new List<DirectoryInfo>();

        private string[] sysPaths = { "Config.Msi" };

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
            listView1.Items.Clear();
            string path = textBox1.Text.Trim();
            string mask = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(path) && string.IsNullOrEmpty(mask))
            {
                MessageBox.Show("Введите путь или маску для поиска.");
                return;
            }

            if (string.IsNullOrWhiteSpace(mask))
            {
                mask = "*";
            }
            else
            {
                if (!mask.StartsWith("*"))
                {
                    if (!mask.StartsWith("."))
                    {
                        mask = "*." + mask;
                    }
                    else
                    {
                        mask = "*" + mask;
                    }
                }
            }

            if (string.IsNullOrEmpty(path))
            {
                foreach (DriveInfo drive in DriveInfo.GetDrives())
                {
                    try
                    {
                        directories.AddRange(drive.RootDirectory.GetDirectories());
                    }
                    catch 
                    { 
                        continue; 
                    }
                }
            }
            else if (Directory.Exists(path))
            {
                directories.Add(new DirectoryInfo(path));
            }
            else
            {
                MessageBox.Show("Указанный путь не существует.");
                return;
            }

            foreach (DirectoryInfo directory in directories)
            {
                try
                {
                    if (directory.Attributes.HasFlag(FileAttributes.System) || sysPaths.Contains(directory.Name))
                    {
                        continue;
                    }

                    FileInfo[] files = directory.GetFiles(mask, SearchOption.TopDirectoryOnly);

                    foreach (FileInfo file in files)
                    {
                        if (file.Attributes.HasFlag(FileAttributes.System) || sysPaths.Contains(file.Name))
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
                catch 
                { 
                    continue; 
                }
            }
            MessageBox.Show("Поиск завершён.");
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            StartSearch();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.Multiline = false;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
