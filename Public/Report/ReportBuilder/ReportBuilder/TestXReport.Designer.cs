namespace YidanSoft.Common.Report
{
   partial class TestXReport
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
         this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
         this.button1 = new System.Windows.Forms.Button();
         this.textBox1 = new System.Windows.Forms.TextBox();
         this.button2 = new System.Windows.Forms.Button();
         this.label1 = new System.Windows.Forms.Label();
         this.button3 = new System.Windows.Forms.Button();
         this.button4 = new System.Windows.Forms.Button();
         this.button5 = new System.Windows.Forms.Button();
         this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
         this.label2 = new System.Windows.Forms.Label();
         this.SuspendLayout();
         // 
         // openFileDialog1
         // 
         this.openFileDialog1.FileName = "openFileDialog1";
         // 
         // button1
         // 
         this.button1.Location = new System.Drawing.Point(279, 97);
         this.button1.Name = "button1";
         this.button1.Size = new System.Drawing.Size(75, 23);
         this.button1.TabIndex = 0;
         this.button1.Text = "设计";
         this.button1.Click += new System.EventHandler(this.button1_Click);
         // 
         // textBox1
         // 
         this.textBox1.Location = new System.Drawing.Point(10, 52);
         this.textBox1.Name = "textBox1";
         this.textBox1.Size = new System.Drawing.Size(344, 21);
         this.textBox1.TabIndex = 1;
         this.textBox1.Text = "D:\\Winning.NET\\SourceCode\\Common\\XReport\\test1.rpt";
         // 
         // button2
         // 
         this.button2.Location = new System.Drawing.Point(180, 97);
         this.button2.Name = "button2";
         this.button2.Size = new System.Drawing.Size(75, 23);
         this.button2.TabIndex = 2;
         this.button2.Text = "预览";
         this.button2.Click += new System.EventHandler(this.button2_Click);
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(8, 32);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(76, 12);
         this.label1.TabIndex = 3;
         this.label1.Text = "报表文件位置";
         // 
         // button3
         // 
         this.button3.Location = new System.Drawing.Point(12, 97);
         this.button3.Name = "button3";
         this.button3.Size = new System.Drawing.Size(91, 23);
         this.button3.TabIndex = 4;
         this.button3.Text = "生成随机数据";
         this.button3.Click += new System.EventHandler(this.button3_Click);
         // 
         // button4
         // 
         this.button4.Location = new System.Drawing.Point(359, 37);
         this.button4.Name = "button4";
         this.button4.Size = new System.Drawing.Size(75, 23);
         this.button4.TabIndex = 5;
         this.button4.Text = "查看文件";
         this.button4.Click += new System.EventHandler(this.button4_Click);
         // 
         // button5
         // 
         this.button5.Location = new System.Drawing.Point(360, 67);
         this.button5.Name = "button5";
         this.button5.Size = new System.Drawing.Size(75, 23);
         this.button5.TabIndex = 6;
         this.button5.Text = "保存文件";
         this.button5.Click += new System.EventHandler(this.button5_Click);
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(4, 9);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(358, 12);
         this.label2.TabIndex = 7;
         this.label2.Text = "1、设置一个正确的文件位置，2、在生成数据，3、点击预览或设计";
         // 
         // TestXReport
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(453, 146);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.button5);
         this.Controls.Add(this.button4);
         this.Controls.Add(this.button3);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.button2);
         this.Controls.Add(this.textBox1);
         this.Controls.Add(this.button1);
         this.Name = "TestXReport";
         this.Text = "TestXReport";
         this.Load += new System.EventHandler(this.TestXReport_Load);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.OpenFileDialog openFileDialog1;
      private System.Windows.Forms.Button button1;
      private System.Windows.Forms.TextBox textBox1;
      private System.Windows.Forms.Button button2;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Button button3;
      private System.Windows.Forms.Button button4;
      private System.Windows.Forms.Button button5;
      private System.Windows.Forms.SaveFileDialog saveFileDialog1;
      private System.Windows.Forms.Label label2;
   }
}