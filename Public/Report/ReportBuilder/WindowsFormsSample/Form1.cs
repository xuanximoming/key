using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using YidanSoft.Common.Report;

namespace WindowsFormsSample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection cn = new System.Data.SqlClient.SqlConnection
                (@"Database=EMRNET;Server=192.168.2.202\two;user id=sa;password=sa");
//            SqlConnection cn = new System.Data.SqlClient.SqlConnection
//            (@"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)
//            (HOST=yidan-ser)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=yidanemr)));User Id=yidandba;Password=sa;");
            SqlCommand cmd = new SqlCommand("select  * from YY_ZGDMK",cn);
//            SqlCommand cmd = new SqlCommand(@"
//              select
//                 d.noofinpat,
//                 a.id,a.childname,
//                 b.cname,
//                 c.problem_desc as koufenliyou,
//                 c.reducepoint as redpoint,
//                  d.name as patname,
//                  e.name as deptname,
//                  f.name as indocname,
//                  f1.name as updocname
//                   from emr_configpoint a 
//                join dict_catalog b on a.ccode=b.ccode
//                left join  emr_point  c on   a.ccode=c.sortid 
//                join  inpatient  d on d.noofinpat=c.noofinpat
//                left join  department e on d.outhosdept=e.id
//                left join users f on d.resident=f.id
//                left join users f1 on f1.id=d.chief 
//                where  a.valid='1' and d.noofinpat='4724' ",cn);
            DataTable data = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(data);

            string file =  @"D:\YidanSoft.Net\OracleRun\users.repx";
            XReport xreport = new XReport(data, file);
            xreport.ShowPreview();
            
        }
    }
}
