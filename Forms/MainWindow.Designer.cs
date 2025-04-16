namespace FileSearch
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            listView1 = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            columnHeader4 = new ColumnHeader();
            columnHeader5 = new ColumnHeader();
            textBox1 = new TextBox();
            button1 = new Button();
            SuspendLayout();
            // 
            // listView1
            // 
            listView1.BackColor = SystemColors.Window;
            listView1.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3, columnHeader4, columnHeader5 });
            listView1.GridLines = true;
            listView1.HoverSelection = true;
            listView1.ImeMode = ImeMode.Alpha;
            listView1.Location = new Point(12, 46);
            listView1.Name = "listView1";
            listView1.Size = new Size(965, 247);
            listView1.TabIndex = 3;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.Details;
            listView1.SelectedIndexChanged += listView1_SelectedIndexChanged;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Имя";
            columnHeader1.Width = 300;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "Путь";
            columnHeader2.Width = 350;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "Размер";
            columnHeader3.Width = 110;
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "Изменен";
            columnHeader4.Width = 120;
            // 
            // columnHeader5
            // 
            columnHeader5.Text = "Тип";
            columnHeader5.Width = 64;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(12, 12);
            textBox1.Name = "textBox1";
            textBox1.PlaceholderText = "Введите";
            textBox1.Size = new Size(869, 23);
            textBox1.TabIndex = 4;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // button1
            // 
            button1.Location = new Point(887, 11);
            button1.Name = "button1";
            button1.Size = new Size(90, 23);
            button1.TabIndex = 5;
            button1.Text = "Поиск";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click_2;
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(989, 305);
            Controls.Add(button1);
            Controls.Add(textBox1);
            Controls.Add(listView1);
            Name = "MainWindow";
            Text = "Поиск файлов";
            Load += MainWindow_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            
        }

        #endregion

        private ListView listView1;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
        private TextBox textBox1;
        private ColumnHeader columnHeader5;
        private Button button1;
    }
}