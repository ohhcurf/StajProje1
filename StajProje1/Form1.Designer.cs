﻿namespace StajProje1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            isimBox = new TextBox();
            soyadBox = new TextBox();
            numaraBox = new TextBox();
            button1 = new Button();
            dataGridView1 = new DataGridView();
            isim = new DataGridViewTextBoxColumn();
            soyad = new DataGridViewTextBoxColumn();
            numara = new DataGridViewTextBoxColumn();
            ID = new DataGridViewTextBoxColumn();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(71, 31);
            label1.Name = "label1";
            label1.Size = new Size(29, 15);
            label1.TabIndex = 0;
            label1.Text = "İsim";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(71, 84);
            label2.Name = "label2";
            label2.Size = new Size(39, 15);
            label2.TabIndex = 1;
            label2.Text = "Soyad";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(71, 134);
            label3.Name = "label3";
            label3.Size = new Size(50, 15);
            label3.TabIndex = 2;
            label3.Text = "Numara";
            // 
            // isimBox
            // 
            isimBox.Location = new Point(164, 31);
            isimBox.Name = "isimBox";
            isimBox.Size = new Size(100, 23);
            isimBox.TabIndex = 3;
            isimBox.KeyPress += isimBox_KeyPress;
            // 
            // soyadBox
            // 
            soyadBox.Location = new Point(164, 81);
            soyadBox.Name = "soyadBox";
            soyadBox.Size = new Size(100, 23);
            soyadBox.TabIndex = 4;
            soyadBox.KeyPress += isimBox_KeyPress;
            // 
            // numaraBox
            // 
            numaraBox.Location = new Point(164, 134);
            numaraBox.Name = "numaraBox";
            numaraBox.Size = new Size(100, 23);
            numaraBox.TabIndex = 5;
            numaraBox.KeyPress += numaraBox_KeyPress;
            // 
            // button1
            // 
            button1.Location = new Point(71, 224);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 6;
            button1.Text = "Ekle";
            button1.UseVisualStyleBackColor = true;
            button1.Click += ekleButton_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { isim, soyad, numara, ID });
            dataGridView1.Location = new Point(348, 31);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.Size = new Size(420, 216);
            dataGridView1.TabIndex = 7;
            dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;
            // 
            // isim
            // 
            isim.HeaderText = "İsim";
            isim.Name = "isim";
            // 
            // soyad
            // 
            soyad.HeaderText = "Soyad";
            soyad.Name = "soyad";
            // 
            // numara
            // 
            numara.HeaderText = "Numara";
            numara.Name = "numara";
            // 
            // ID
            // 
            ID.HeaderText = "ID";
            ID.Name = "ID";
            // 
            // button2
            // 
            button2.Location = new Point(189, 224);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 8;
            button2.Text = "Listele";
            button2.UseVisualStyleBackColor = true;
            button2.Click += listeleButton_Click;
            // 
            // button3
            // 
            button3.Location = new Point(693, 270);
            button3.Name = "button3";
            button3.Size = new Size(75, 23);
            button3.TabIndex = 9;
            button3.Text = "Sil";
            button3.UseVisualStyleBackColor = true;
            button3.Click += silButton_Click;
            // 
            // button4
            // 
            button4.Location = new Point(348, 270);
            button4.Name = "button4";
            button4.Size = new Size(75, 23);
            button4.TabIndex = 10;
            button4.Text = "Arama";
            button4.UseVisualStyleBackColor = true;
            button4.Click += aramaButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(dataGridView1);
            Controls.Add(button1);
            Controls.Add(numaraBox);
            Controls.Add(soyadBox);
            Controls.Add(isimBox);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox isimBox;
        private TextBox soyadBox;
        private TextBox numaraBox;
        private Button button1;
        private DataGridView dataGridView1;
        private Button button2;
        private Button button3;
        private Button button4;
        private DataGridViewTextBoxColumn isim;
        private DataGridViewTextBoxColumn soyad;
        private DataGridViewTextBoxColumn numara;
        private DataGridViewTextBoxColumn ID;
    }
}