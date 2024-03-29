﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors.Repository;

namespace DrectSoft.EMR.ThreeRecordAll.Controls
{
    public partial class SplitTextBox : TextBox
    {
        TextBox aTextBox = new TextBox();
        TextBox bTextBox = new TextBox();
        private void initControls()
        {
            try
            {
                bTextBox = new TextBox();
                bTextBox.Width = (this.Width - 10) / 2;
                bTextBox.Dock = DockStyle.Left;
                bTextBox.BorderStyle = BorderStyle.None;
                bTextBox.TextAlign = HorizontalAlignment.Center;
                bTextBox.TextChanged += new EventHandler(aTextBox_TextChanged);
                this.Controls.Add(bTextBox);
                Label aLabel = new Label();
                aLabel.AutoSize = false;
                aLabel.Text = "|";
                aLabel.Width = 10;
                aLabel.Dock = DockStyle.Left;
                aLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

                this.Controls.Add(aLabel);
                aTextBox = new TextBox();
                aTextBox.Width = (this.Width - 10) / 2;
                aTextBox.Dock = DockStyle.Left;
                aTextBox.BorderStyle = BorderStyle.None;
                aTextBox.TextAlign = HorizontalAlignment.Center;
                aTextBox.TextChanged += new EventHandler(aTextBox_TextChanged);
                this.Controls.Add(aTextBox);
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        //public SplitTextBox()
        //{
        //    InitializeComponent();
        //    initControls();
        //}

        public SplitTextBox()
        {

            InitializeComponent();
            initControls();
        }

        private void aTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.Text = aTextBox.Text.Trim() + "|" + bTextBox.Text.Trim();
            }
            catch (Exception ce)
            {
                MessageBox.Show(ce.Message.ToString());
            }
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                try
                {
                    base.Text = value;
                    if (value == null || value == "")
                    {
                        return;
                    }
                    string[] arr = value.Split('|');
                    if (arr.Length == 0)
                    {
                        return;
                    }
                    aTextBox.Text = arr[0] == null ? "" : arr[0];
                    bTextBox.Text = arr[1] == null ? "" : arr[1];
                }
                catch(Exception ex)
                {
                    return;
                }
            }
        }
    }
}
