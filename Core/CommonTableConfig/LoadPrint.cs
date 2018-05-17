using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.FrameWork.WinForm.Plugin;
using System.IO;

namespace DrectSoft.Core.CommonTableConfig
{
    /// <summary>
    /// <title>用于上传护理文档的打印模板  暂未完成</title>
    /// <auth>xuliangliang</auth>
    /// <date>2012-10-29</date>
    /// </summary>
    public partial class LoadPrint : DevBaseForm
    {
        IEmrHost m_app;
        LoadPrintBiz loadbiz;
        public LoadPrint(IEmrHost m_app)
        {
            try
            {
                InitializeComponent();
                this.m_app = m_app;
                loadbiz = new LoadPrintBiz(this.m_app);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        private void BindDate()
        {
            try
            {
                List<EmrPrintTempEntity> EmrPrintTempEntityList = loadbiz.GetPrintTempList();
                gridControl1Print.DataSource = EmrPrintTempEntityList;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddDate();
        }


        private void AddDate()
        {
            List<EmrPrintTempEntity> emrPrintTempEntityList = gridControl1Print.DataSource as List<EmrPrintTempEntity>;
            if (emrPrintTempEntityList == null)
            {
                emrPrintTempEntityList = new List<EmrPrintTempEntity>();
            }
            emrPrintTempEntityList.Add(new EmrPrintTempEntity());
            gridControl1Print.DataSource = new List<EmrPrintTempEntity>(emrPrintTempEntityList);
            gridView1.MoveBy(emrPrintTempEntityList.Count);
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
           EmrPrintTempEntity emrPrintTempEntity= gridView1.GetFocusedRow() as EmrPrintTempEntity;
           if (emrPrintTempEntity == null)
           {
               btnLiulang.Enabled = btnSave.Enabled = false;
               return;
           }
           else
           {
               btnLiulang.Enabled = btnSave.Enabled = true;
           }

           if (string.IsNullOrEmpty(emrPrintTempEntity.PrintFlow))
           {
               txtPrintFileName.Properties.ReadOnly = false;
           }
           else
           {
               txtPrintFileName.Properties.ReadOnly = true;
           }
           txtPrintFileName.Text = emrPrintTempEntity.PrintFileName;
           txtPrintName.Text = emrPrintTempEntity.PrintName;
        }

        private void btnLiulang_Click(object sender, EventArgs e)
        {
            EmrPrintTempEntity emrPrintTempEntity = gridView1.GetFocusedRow() as EmrPrintTempEntity;
            if (emrPrintTempEntity == null)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请先点击新增按钮");
                return;
            }
            openFileDialog.Filter = "模板文件(*.xrp)|*.xrp";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                 
                  string fullNames=  openFileDialog.FileName;
                  txtFilestr.Text = fullNames;
                  string fileName = fullNames.Substring(fullNames.LastIndexOf("\\") + 1, fullNames.LastIndexOf(".")-fullNames.LastIndexOf("\\")-1);
                  if (string.IsNullOrEmpty(txtPrintFileName.Text))
                  {
                     txtPrintFileName.Text=fileName;
                  }
                  FileStream fileStream = new FileStream(fullNames, FileMode.Open, FileAccess.Read);
                  byte[] bytes = new byte[fileStream.Length];
                  fileStream.Read(bytes, 0, (int)fileStream.Length);
                  fileStream.Flush();
                  fileStream.Close();
                 
                  emrPrintTempEntity.PrintContent ="";
                  emrPrintTempEntity.PrintContentbyte = bytes;
            }

        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            EmrPrintTempEntity emrPrintTempEntity = gridView1.GetFocusedRow() as EmrPrintTempEntity;
            if (string.IsNullOrEmpty(emrPrintTempEntity.PrintFlow))
                emrPrintTempEntity.PrintFlow = Guid.NewGuid().ToString();
            emrPrintTempEntity.PrintName =  txtPrintName.Text;
            emrPrintTempEntity.PrintFileName = txtPrintFileName.Text;
            if (string.IsNullOrEmpty(emrPrintTempEntity.CreateDoctorName))
            {
                emrPrintTempEntity.CreateDoctorID = m_app.User.DoctorId;
                emrPrintTempEntity.CreateDoctorName = m_app.User.DoctorName;
                emrPrintTempEntity.IsValide = "1";
            }
            
           bool result= loadbiz.AddOrModelPrintTempEntity(emrPrintTempEntity);
           if (result)
           {
               BindDate();
           }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            EmrPrintTempEntity emrPrintTempEntity = gridView1.GetFocusedRow() as EmrPrintTempEntity;
            if (emrPrintTempEntity == null)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选中要删除的数据");
                return;
            }
            emrPrintTempEntity.IsValide = "0";
            bool result = loadbiz.AddOrModelPrintTempEntity(emrPrintTempEntity);
            if (result)
            {
                BindDate();
            }
        }

      
    }
}