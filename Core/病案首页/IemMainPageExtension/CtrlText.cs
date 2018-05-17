using System;
using System.Collections.Generic;
using DrectSoft.Common.Ctrs.OTHER;

namespace IemMainPageExtension
{
    class CtrlText : DSTextBox, IIemMainPageExcept
    {
        IemMainPageExcept iemExcept;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit fProperties;/*维护对象实体*/
        IemMainPageExceptUse myIemMainPageExceptUse; //使用对象

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iem"></param>
        /// <param name="iemMainUse"></param>
        public CtrlText(IemMainPageExcept iem, IemMainPageExceptUse iemMainUse)
        {
            try
            {
                if (iem == null || iemMainUse == null)
                {
                    return;
                }
                this.iemExcept = iem;
                myIemMainPageExceptUse = iemMainUse;
                InitData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化数据
        /// 有默认值则填充默认值
        /// </summary>
        private void InitData()
        {
            try
            {
                if (!string.IsNullOrEmpty(myIemMainPageExceptUse.IemexUseId))
                {
                    this.Text = myIemMainPageExceptUse.Value;
                }
                else
                {
                    Dictionary<string, string> dicStr = DateElementEntity.GetDataSource(iemExcept.DateElement);
                    if (dicStr == null)
                    {
                        return;
                    }
                    //默认值
                    string defaultValue = dicStr["DefaultValue"];
                    this.Text = defaultValue;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 接口方法
        /// 返回使用对象
        /// </summary>
        /// <returns></returns>
        public IemMainPageExceptUse GetIemMainPageExceptUse()
        {
            try
            {
                myIemMainPageExceptUse.Value = this.Text;
                myIemMainPageExceptUse.IemexId = iemExcept.IemExId;
                return myIemMainPageExceptUse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitializeComponent()
        {
            this.fProperties = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.fProperties)).BeginInit();
            this.SuspendLayout();
            // 
            // fProperties
            // 
            this.fProperties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.fProperties.Name = "fProperties";
            // 
            // CtrlText
            // 
            this.Size = new System.Drawing.Size(100, 19);
            ((System.ComponentModel.ISupportInitialize)(this.fProperties)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
