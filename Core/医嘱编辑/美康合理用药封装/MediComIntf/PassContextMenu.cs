using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DrectSoft.Common.MediComIntf
{
   /// <summary>
   ///
   /// </summary>
   public partial class PassMenu : System.Windows.Forms.ContextMenuStrip
    {
        private System.Windows.Forms.ToolStripMenuItem 临床药物信息参考ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 病人用药教育ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 药品说明书ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 国家药典ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 检验值ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 专项信息ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 药物药物相互作用ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 药物食物相互作用ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 国内注射剂体外配伍ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 国外注射剂体外配伍ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 禁忌症ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 副作用ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 老年人用药ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 儿童用药ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 妊娠期用药ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 哺乳期用药ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 药物信息中心ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 药品配对信息ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 给药途径配对信息ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 医院药品信息ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 系统设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 用药研究ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 单药警告ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 审查ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 过敏史病生状态管理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;

        private PassCheckHelper _passCheckhlp;

        /// <summary>
        ///
        /// </summary>
        public PassCheckHelper PassCheckhlp
        {
            get { return _passCheckhlp; }
            set { _passCheckhlp = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public PassMenu()
            : this(null)
        { }

        /// <summary>
        ///
        /// </summary>
        public PassMenu(PassCheckHelper passCheckHelper)
        {
            Init();
            _passCheckhlp = passCheckHelper;
        }

        void Init()
        {
            this.临床药物信息参考ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.病人用药教育ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.药品说明书ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.国家药典ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.检验值ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.专项信息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.药物药物相互作用ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.药物食物相互作用ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.国内注射剂体外配伍ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.国外注射剂体外配伍ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.禁忌症ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.副作用ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.老年人用药ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.儿童用药ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.妊娠期用药ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.哺乳期用药ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.药物信息中心ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.药品配对信息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.给药途径配对信息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.医院药品信息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.系统设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.用药研究ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.单药警告ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.审查ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.过敏史病生状态管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();

            // 
            // contextMenuStrip1
            // 
            this.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.临床药物信息参考ToolStripMenuItem,
            this.病人用药教育ToolStripMenuItem,
            this.药品说明书ToolStripMenuItem,
            this.国家药典ToolStripMenuItem,
            this.检验值ToolStripMenuItem,
            this.toolStripSeparator1,
            this.专项信息ToolStripMenuItem,
            this.toolStripSeparator2,
            this.药物信息中心ToolStripMenuItem,
            this.toolStripSeparator3,
            this.药品配对信息ToolStripMenuItem,
            this.给药途径配对信息ToolStripMenuItem,
            this.医院药品信息ToolStripMenuItem,
            this.toolStripSeparator4,
            this.系统设置ToolStripMenuItem,
            this.用药研究ToolStripMenuItem,
            this.单药警告ToolStripMenuItem,
            this.审查ToolStripMenuItem,
            this.toolStripSeparator5,
            this.过敏史病生状态管理ToolStripMenuItem,
            this.toolStripSeparator6,
            this.帮助ToolStripMenuItem});
            this.Name = "passContextMenuTrip";
            this.Size = new System.Drawing.Size(229, 446);
            // 
            // 临床药物信息参考ToolStripMenuItem
            // 
            this.临床药物信息参考ToolStripMenuItem.Name = "临床药物信息参考ToolStripMenuItem";
            this.临床药物信息参考ToolStripMenuItem.Size = new System.Drawing.Size(228, 24);
            this.临床药物信息参考ToolStripMenuItem.Text = "临床药物信息参考";
            this.临床药物信息参考ToolStripMenuItem.Tag = 101;
            this.临床药物信息参考ToolStripMenuItem.Click += new EventHandler(TagToolStripMenuItem_Click);
            // 
            // 病人用药教育ToolStripMenuItem
            // 
            this.病人用药教育ToolStripMenuItem.Name = "病人用药教育ToolStripMenuItem";
            this.病人用药教育ToolStripMenuItem.Size = new System.Drawing.Size(228, 24);
            this.病人用药教育ToolStripMenuItem.Text = "病人用药教育";
            this.病人用药教育ToolStripMenuItem.Tag = 103;
            this.病人用药教育ToolStripMenuItem.Click +=new EventHandler(TagToolStripMenuItem_Click);
            // 
            // 药品说明书ToolStripMenuItem
            // 
            this.药品说明书ToolStripMenuItem.Name = "药品说明书ToolStripMenuItem";
            this.药品说明书ToolStripMenuItem.Size = new System.Drawing.Size(228, 24);
            this.药品说明书ToolStripMenuItem.Text = "药品说明书";
            this.药品说明书ToolStripMenuItem.Tag = 102;
            this.药品说明书ToolStripMenuItem.Click += new EventHandler(TagToolStripMenuItem_Click);
            // 
            // 国家药典ToolStripMenuItem
            // 
            this.国家药典ToolStripMenuItem.Name = "国家药典ToolStripMenuItem";
            this.国家药典ToolStripMenuItem.Size = new System.Drawing.Size(228, 24);
            this.国家药典ToolStripMenuItem.Text = "国家药典";
            this.国家药典ToolStripMenuItem.Tag = 107;
            this.国家药典ToolStripMenuItem.Click += new EventHandler(TagToolStripMenuItem_Click);
            // 
            // 检验值ToolStripMenuItem
            // 
            this.检验值ToolStripMenuItem.Name = "检验值ToolStripMenuItem";
            this.检验值ToolStripMenuItem.Size = new System.Drawing.Size(228, 24);
            this.检验值ToolStripMenuItem.Text = "检验值";
            this.检验值ToolStripMenuItem.Tag = 104;
            this.检验值ToolStripMenuItem.Click += new EventHandler(TagToolStripMenuItem_Click);
            // 
            // 专项信息ToolStripMenuItem
            // 
            this.专项信息ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.药物药物相互作用ToolStripMenuItem,
            this.药物食物相互作用ToolStripMenuItem,
            this.toolStripSeparator7,
            this.国内注射剂体外配伍ToolStripMenuItem,
            this.国外注射剂体外配伍ToolStripMenuItem,
            this.toolStripSeparator8,
            this.禁忌症ToolStripMenuItem,
            this.副作用ToolStripMenuItem,
            this.toolStripSeparator9,
            this.老年人用药ToolStripMenuItem,
            this.儿童用药ToolStripMenuItem,
            this.妊娠期用药ToolStripMenuItem,
            this.哺乳期用药ToolStripMenuItem});
            this.专项信息ToolStripMenuItem.Name = "专项信息ToolStripMenuItem";
            this.专项信息ToolStripMenuItem.Size = new System.Drawing.Size(228, 24);
            this.专项信息ToolStripMenuItem.Text = "专项信息";
            // 
            // 药物药物相互作用ToolStripMenuItem
            // 
            this.药物药物相互作用ToolStripMenuItem.Name = "药物药物相互作用ToolStripMenuItem";
            this.药物药物相互作用ToolStripMenuItem.Size = new System.Drawing.Size(222, 24);
            this.药物药物相互作用ToolStripMenuItem.Text = "药物-药物相互作用";
            this.药物药物相互作用ToolStripMenuItem.Tag = 201;
            this.药物药物相互作用ToolStripMenuItem.Click += new EventHandler(TagToolStripMenuItem_Click);
            // 
            // 药物食物相互作用ToolStripMenuItem
            // 
            this.药物食物相互作用ToolStripMenuItem.Name = "药物食物相互作用ToolStripMenuItem";
            this.药物食物相互作用ToolStripMenuItem.Size = new System.Drawing.Size(222, 24);
            this.药物食物相互作用ToolStripMenuItem.Text = "药物-食物相互作用";
            this.药物食物相互作用ToolStripMenuItem.Tag = 202;
            this.药物食物相互作用ToolStripMenuItem.Click += new EventHandler(TagToolStripMenuItem_Click);
            // 
            // 国内注射剂体外配伍ToolStripMenuItem
            // 
            this.国内注射剂体外配伍ToolStripMenuItem.Name = "国内注射剂体外配伍ToolStripMenuItem";
            this.国内注射剂体外配伍ToolStripMenuItem.Size = new System.Drawing.Size(222, 24);
            this.国内注射剂体外配伍ToolStripMenuItem.Text = "国内注射剂体外配伍";
            this.国内注射剂体外配伍ToolStripMenuItem.Tag = 203;
            this.国内注射剂体外配伍ToolStripMenuItem.Click += new EventHandler(TagToolStripMenuItem_Click);
            // 
            // 国外注射剂体外配伍ToolStripMenuItem
            // 
            this.国外注射剂体外配伍ToolStripMenuItem.Name = "国外注射剂体外配伍ToolStripMenuItem";
            this.国外注射剂体外配伍ToolStripMenuItem.Size = new System.Drawing.Size(222, 24);
            this.国外注射剂体外配伍ToolStripMenuItem.Text = "国外注射剂体外配伍";
            this.国外注射剂体外配伍ToolStripMenuItem.Tag = 204;
            this.国外注射剂体外配伍ToolStripMenuItem.Click += new EventHandler(TagToolStripMenuItem_Click);
            // 
            // 禁忌症ToolStripMenuItem
            // 
            this.禁忌症ToolStripMenuItem.Name = "禁忌症ToolStripMenuItem";
            this.禁忌症ToolStripMenuItem.Size = new System.Drawing.Size(222, 24);
            this.禁忌症ToolStripMenuItem.Text = "禁忌症";
            this.禁忌症ToolStripMenuItem.Tag = 205;
            this.禁忌症ToolStripMenuItem.Click += new EventHandler(TagToolStripMenuItem_Click);
            // 
            // 副作用ToolStripMenuItem
            // 
            this.副作用ToolStripMenuItem.Name = "副作用ToolStripMenuItem";
            this.副作用ToolStripMenuItem.Size = new System.Drawing.Size(222, 24);
            this.副作用ToolStripMenuItem.Text = "副作用";
            this.副作用ToolStripMenuItem.Tag = 206;
            this.副作用ToolStripMenuItem.Click += new EventHandler(TagToolStripMenuItem_Click);
            // 
            // 老年人用药ToolStripMenuItem
            // 
            this.老年人用药ToolStripMenuItem.Name = "老年人用药ToolStripMenuItem";
            this.老年人用药ToolStripMenuItem.Size = new System.Drawing.Size(222, 24);
            this.老年人用药ToolStripMenuItem.Text = "老年人用药";
            this.老年人用药ToolStripMenuItem.Tag = 207;
            this.老年人用药ToolStripMenuItem.Click += new EventHandler(TagToolStripMenuItem_Click);
            // 
            // 儿童用药ToolStripMenuItem
            // 
            this.儿童用药ToolStripMenuItem.Name = "儿童用药ToolStripMenuItem";
            this.儿童用药ToolStripMenuItem.Size = new System.Drawing.Size(222, 24);
            this.儿童用药ToolStripMenuItem.Text = "儿童用药";
            this.儿童用药ToolStripMenuItem.Tag = 208;
            this.儿童用药ToolStripMenuItem.Click += new EventHandler(TagToolStripMenuItem_Click);
            // 
            // 妊娠期用药ToolStripMenuItem
            // 
            this.妊娠期用药ToolStripMenuItem.Name = "妊娠期用药ToolStripMenuItem";
            this.妊娠期用药ToolStripMenuItem.Size = new System.Drawing.Size(222, 24);
            this.妊娠期用药ToolStripMenuItem.Text = "妊娠期用药";
            this.妊娠期用药ToolStripMenuItem.Tag = 209;
            this.妊娠期用药ToolStripMenuItem.Click += new EventHandler(TagToolStripMenuItem_Click);
            // 
            // 哺乳期用药ToolStripMenuItem
            // 
            this.哺乳期用药ToolStripMenuItem.Name = "哺乳期用药ToolStripMenuItem";
            this.哺乳期用药ToolStripMenuItem.Size = new System.Drawing.Size(222, 24);
            this.哺乳期用药ToolStripMenuItem.Text = "哺乳期用药";
            this.哺乳期用药ToolStripMenuItem.Tag = 210;
            this.哺乳期用药ToolStripMenuItem.Click += new EventHandler(TagToolStripMenuItem_Click);
            // 
            // 药物信息中心ToolStripMenuItem
            // 
            this.药物信息中心ToolStripMenuItem.Name = "药物信息中心ToolStripMenuItem";
            this.药物信息中心ToolStripMenuItem.Size = new System.Drawing.Size(228, 24);
            this.药物信息中心ToolStripMenuItem.Text = "药物信息中心";
            this.药物信息中心ToolStripMenuItem.Tag = 106;
            this.药物信息中心ToolStripMenuItem.Click += new EventHandler(TagToolStripMenuItem_Click);
            // 
            // 药品配对信息ToolStripMenuItem
            // 
            this.药品配对信息ToolStripMenuItem.Name = "药品配对信息ToolStripMenuItem";
            this.药品配对信息ToolStripMenuItem.Size = new System.Drawing.Size(228, 24);
            this.药品配对信息ToolStripMenuItem.Text = "药品配对信息";
            this.药品配对信息ToolStripMenuItem.Tag = 13;
            this.药品配对信息ToolStripMenuItem.Click += new EventHandler(TagToolStripMenuItem_Click);
            // 
            // 给药途径配对信息ToolStripMenuItem
            // 
            this.给药途径配对信息ToolStripMenuItem.Name = "给药途径配对信息ToolStripMenuItem";
            this.给药途径配对信息ToolStripMenuItem.Size = new System.Drawing.Size(228, 24);
            this.给药途径配对信息ToolStripMenuItem.Text = "给药途径配对信息";
            this.给药途径配对信息ToolStripMenuItem.Tag = 14;
            this.给药途径配对信息ToolStripMenuItem.Click += new EventHandler(TagToolStripMenuItem_Click);
            // 
            // 医院药品信息ToolStripMenuItem
            // 
            this.医院药品信息ToolStripMenuItem.Name = "医院药品信息ToolStripMenuItem";
            this.医院药品信息ToolStripMenuItem.Size = new System.Drawing.Size(228, 24);
            this.医院药品信息ToolStripMenuItem.Text = "医院药品信息";
            this.医院药品信息ToolStripMenuItem.Tag = 105;
            this.医院药品信息ToolStripMenuItem.Click += new EventHandler(TagToolStripMenuItem_Click);
            // 
            // 系统设置ToolStripMenuItem
            // 
            this.系统设置ToolStripMenuItem.Name = "系统设置ToolStripMenuItem";
            this.系统设置ToolStripMenuItem.Size = new System.Drawing.Size(228, 24);
            this.系统设置ToolStripMenuItem.Text = "系统设置";
            this.系统设置ToolStripMenuItem.Tag = 11;
            this.系统设置ToolStripMenuItem.Click += new EventHandler(TagToolStripMenuItem_Click);
            // 
            // 用药研究ToolStripMenuItem
            // 
            this.用药研究ToolStripMenuItem.Name = "用药研究ToolStripMenuItem";
            this.用药研究ToolStripMenuItem.Size = new System.Drawing.Size(228, 24);
            this.用药研究ToolStripMenuItem.Text = "用药研究";
            this.用药研究ToolStripMenuItem.Tag = 12;
            this.用药研究ToolStripMenuItem.Click += new EventHandler(TagToolStripMenuItem_Click);
            // 
            // 单药警告ToolStripMenuItem
            // 
            this.单药警告ToolStripMenuItem.Name = "单药警告ToolStripMenuItem";
            this.单药警告ToolStripMenuItem.Size = new System.Drawing.Size(228, 24);
            this.单药警告ToolStripMenuItem.Text = "单药警告";
            this.单药警告ToolStripMenuItem.Tag = 6;
            this.单药警告ToolStripMenuItem.Click += new EventHandler(单药警告ToolStripMenuItem_Click);
            // 
            // 审查ToolStripMenuItem
            // 
            this.审查ToolStripMenuItem.Name = "审查ToolStripMenuItem";
            this.审查ToolStripMenuItem.Size = new System.Drawing.Size(228, 24);
            this.审查ToolStripMenuItem.Text = "审查";
            this.审查ToolStripMenuItem.Tag = 3;
            this.审查ToolStripMenuItem.Click += new EventHandler(审查ToolStripMenuItem_Click);
            // 
            // 过敏史病生状态管理ToolStripMenuItem
            // 
            this.过敏史病生状态管理ToolStripMenuItem.Name = "过敏史病生状态管理ToolStripMenuItem";
            this.过敏史病生状态管理ToolStripMenuItem.Size = new System.Drawing.Size(228, 24);
            this.过敏史病生状态管理ToolStripMenuItem.Text = "过敏史/病生状态管理";
            this.过敏史病生状态管理ToolStripMenuItem.Tag = 21;
            this.过敏史病生状态管理ToolStripMenuItem.Click += new EventHandler(过敏史病生状态管理ToolStripMenuItem_Click);
            // 
            // 帮助ToolStripMenuItem
            // 
            this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            this.帮助ToolStripMenuItem.Size = new System.Drawing.Size(228, 24);
            this.帮助ToolStripMenuItem.Text = "帮助";
            this.帮助ToolStripMenuItem.Tag = 301;
            this.帮助ToolStripMenuItem.Click += new EventHandler(TagToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(225, 6);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(225, 6);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(225, 6);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(225, 6);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(225, 6);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(225, 6);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(219, 6);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(219, 6);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(219, 6);
        }

        void 审查ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int commandId = Convert.ToInt32((sender as System.Windows.Forms.ToolStripMenuItem).Tag);
            if (_passCheckhlp != null && _passCheckhlp.HasPatient()
                && _passCheckhlp.CurrentDrugInfos != null
                && _passCheckhlp.CurrentDrugInfos.Count > 0)
            {
                _passCheckhlp.DoCommand(commandId);
            }
        }

        void 单药警告ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int commandId = Convert.ToInt32((sender as System.Windows.Forms.ToolStripMenuItem).Tag);
            if (_passCheckhlp != null && _passCheckhlp.HasPatient()
                && !string.IsNullOrEmpty(_passCheckhlp.CurrentDrugIndex))
            {
                _passCheckhlp.PassSetWarnDrug(_passCheckhlp.CurrentDrugIndex);
                _passCheckhlp.DoCommand(commandId);
            }
        }

        void 过敏史病生状态管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int commandId = Convert.ToInt32((sender as System.Windows.Forms.ToolStripMenuItem).Tag);
            if (_passCheckhlp != null && _passCheckhlp.HasPatient())
                _passCheckhlp.DoCommand(commandId);
        }

        void TagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int commandId = Convert.ToInt32((sender as System.Windows.Forms.ToolStripMenuItem).Tag);
            if (_passCheckhlp != null)
            {
                if (_passCheckhlp.PassPopMenuEnable(commandId.ToString()))
                    _passCheckhlp.DoCommand(commandId);
            }
        }

    }
}
