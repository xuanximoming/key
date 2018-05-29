using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using EmrInfirce;
using EmrInsert;

namespace EMRTESTWINDOW
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        EmrDataHelper emr = new EmrDataHelper();

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                //    UCEmr ucEmr = new UCEmr();
                //    ucEmr.Dock = DockStyle.Fill;
                //    DataTable dt = new DataTable();
                //    dt.Columns.Add("PatientNum");
                //    dt.Columns.Add("Name");
                //    DataRow newRow = dt.NewRow();
                //    newRow["PatientNum"] = "7938";
                //    newRow["Name"] = "ÈË×ß²èÁ¹";
                //    dt.Rows.Add(newRow);

                //    ucEmr.Shuaxin("2401", newRow);
                //    panel1.Controls.Add(ucEmr);t
                
                DataTable dtbed = emr.SelectDataBase("select * from Bed ");
                DataTable dtinp = emr.SelectDataBase("select * from InPatient  where name='ÖÜæÂæÂ'");
                DataTable dtinp1 = emr.SelectDataBase("select * from InPatient ");
                dgBed.DataSource = dtbed;
                dgInPatient.DataSource = dtinp;
                dataGridView1.DataSource = dtinp1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

    }
}