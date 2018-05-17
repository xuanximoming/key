using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace XWriterDemo
{
	/// <summary>
	/// frmResource 的摘要说明。
	/// </summary>
	public class frmResource : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label _TEXTDEMO;
		private System.Windows.Forms.Label _SAVEFILTER;
		private System.Windows.Forms.Label _EXEERR;
		private System.Windows.Forms.Label _ERROR;
		private System.Windows.Forms.Label _OPENING;
		private System.Windows.Forms.Label _OPENED;
		private System.Windows.Forms.Label _OPENERR;
		private System.Windows.Forms.Label _IMGFILTER;
		private System.Windows.Forms.Label _SAVING;
		private System.Windows.Forms.Label _SAVED;
		private System.Windows.Forms.Label _QUERYSAVE;
		private System.Windows.Forms.Label _LINE;
		private System.Windows.Forms.Label _SELECTELEMENTS;
		private System.Windows.Forms.Label _QUERYHTMLINDENT;
		private System.Windows.Forms.Label _BOOKMARKEXIST;
		private System.Windows.Forms.TextBox _ABOUT;
		private System.Windows.Forms.Label _SAVEERR;
		private System.Windows.Forms.TextBox _WORDCOUNT;
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmResource()
		{
			//
			// Windows 窗体设计器支持所必需的
			//
			InitializeComponent();

			//
			// TODO: 在 InitializeComponent 调用后添加任何构造函数代码
			//
		}

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows 窗体设计器生成的代码
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmResource));
            this._TEXTDEMO = new System.Windows.Forms.Label();
            this._SAVEFILTER = new System.Windows.Forms.Label();
            this._EXEERR = new System.Windows.Forms.Label();
            this._ERROR = new System.Windows.Forms.Label();
            this._OPENING = new System.Windows.Forms.Label();
            this._OPENED = new System.Windows.Forms.Label();
            this._OPENERR = new System.Windows.Forms.Label();
            this._IMGFILTER = new System.Windows.Forms.Label();
            this._SAVING = new System.Windows.Forms.Label();
            this._SAVED = new System.Windows.Forms.Label();
            this._QUERYSAVE = new System.Windows.Forms.Label();
            this._LINE = new System.Windows.Forms.Label();
            this._SELECTELEMENTS = new System.Windows.Forms.Label();
            this._QUERYHTMLINDENT = new System.Windows.Forms.Label();
            this._BOOKMARKEXIST = new System.Windows.Forms.Label();
            this._ABOUT = new System.Windows.Forms.TextBox();
            this._SAVEERR = new System.Windows.Forms.Label();
            this._WORDCOUNT = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // _TEXTDEMO
            // 
            resources.ApplyResources(this._TEXTDEMO, "_TEXTDEMO");
            this._TEXTDEMO.Name = "_TEXTDEMO";
            // 
            // _SAVEFILTER
            // 
            resources.ApplyResources(this._SAVEFILTER, "_SAVEFILTER");
            this._SAVEFILTER.Name = "_SAVEFILTER";
            // 
            // _EXEERR
            // 
            resources.ApplyResources(this._EXEERR, "_EXEERR");
            this._EXEERR.Name = "_EXEERR";
            // 
            // _ERROR
            // 
            resources.ApplyResources(this._ERROR, "_ERROR");
            this._ERROR.Name = "_ERROR";
            // 
            // _OPENING
            // 
            resources.ApplyResources(this._OPENING, "_OPENING");
            this._OPENING.Name = "_OPENING";
            // 
            // _OPENED
            // 
            resources.ApplyResources(this._OPENED, "_OPENED");
            this._OPENED.Name = "_OPENED";
            // 
            // _OPENERR
            // 
            resources.ApplyResources(this._OPENERR, "_OPENERR");
            this._OPENERR.Name = "_OPENERR";
            // 
            // _IMGFILTER
            // 
            resources.ApplyResources(this._IMGFILTER, "_IMGFILTER");
            this._IMGFILTER.Name = "_IMGFILTER";
            // 
            // _SAVING
            // 
            resources.ApplyResources(this._SAVING, "_SAVING");
            this._SAVING.Name = "_SAVING";
            // 
            // _SAVED
            // 
            resources.ApplyResources(this._SAVED, "_SAVED");
            this._SAVED.Name = "_SAVED";
            // 
            // _QUERYSAVE
            // 
            resources.ApplyResources(this._QUERYSAVE, "_QUERYSAVE");
            this._QUERYSAVE.Name = "_QUERYSAVE";
            // 
            // _LINE
            // 
            resources.ApplyResources(this._LINE, "_LINE");
            this._LINE.Name = "_LINE";
            // 
            // _SELECTELEMENTS
            // 
            resources.ApplyResources(this._SELECTELEMENTS, "_SELECTELEMENTS");
            this._SELECTELEMENTS.Name = "_SELECTELEMENTS";
            // 
            // _QUERYHTMLINDENT
            // 
            resources.ApplyResources(this._QUERYHTMLINDENT, "_QUERYHTMLINDENT");
            this._QUERYHTMLINDENT.Name = "_QUERYHTMLINDENT";
            // 
            // _BOOKMARKEXIST
            // 
            resources.ApplyResources(this._BOOKMARKEXIST, "_BOOKMARKEXIST");
            this._BOOKMARKEXIST.Name = "_BOOKMARKEXIST";
            // 
            // _ABOUT
            // 
            resources.ApplyResources(this._ABOUT, "_ABOUT");
            this._ABOUT.Name = "_ABOUT";
            // 
            // _SAVEERR
            // 
            resources.ApplyResources(this._SAVEERR, "_SAVEERR");
            this._SAVEERR.Name = "_SAVEERR";
            // 
            // _WORDCOUNT
            // 
            resources.ApplyResources(this._WORDCOUNT, "_WORDCOUNT");
            this._WORDCOUNT.Name = "_WORDCOUNT";
            // 
            // frmResource
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this._WORDCOUNT);
            this.Controls.Add(this._ABOUT);
            this.Controls.Add(this._SAVEFILTER);
            this.Controls.Add(this._TEXTDEMO);
            this.Controls.Add(this._EXEERR);
            this.Controls.Add(this._ERROR);
            this.Controls.Add(this._OPENING);
            this.Controls.Add(this._OPENED);
            this.Controls.Add(this._OPENERR);
            this.Controls.Add(this._IMGFILTER);
            this.Controls.Add(this._SAVING);
            this.Controls.Add(this._SAVED);
            this.Controls.Add(this._QUERYSAVE);
            this.Controls.Add(this._LINE);
            this.Controls.Add(this._SELECTELEMENTS);
            this.Controls.Add(this._QUERYHTMLINDENT);
            this.Controls.Add(this._BOOKMARKEXIST);
            this.Controls.Add(this._SAVEERR);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmResource";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
	}
}
