using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Core;
using DrectSoft.FrameWork.WinForm.Plugin;
namespace DrectSoft.Common.SystemAppManager
{
    public partial class StandardAppConfigDesign : UserControl, IAppConfigDesign
    {
        Dictionary<string, EmrAppConfig> _configs;
        Dictionary<string, EmrAppConfig> _updconfigs = new Dictionary<string, EmrAppConfig>();
        const int cstInitLeft = 10;
        const int cstInitTop = 10;
        const int cstInitInterval = 3;

        public StandardAppConfigDesign()
        {
            InitializeComponent();
        }

        #region IAppConfigDesign Members

        public Dictionary<string, EmrAppConfig> ChangedConfigs
        {
            get { return _updconfigs; }
        }

        public Control DesignUI
        {
            get { return this; }
        }

        public new void Load(IEmrHost app, Dictionary<string, EmrAppConfig> configs)
        {
            _configs = configs;
            GenDesignInterface(this, configs);
        }

        private void GenDesignInterface(Control parCtrl, Dictionary<string, EmrAppConfig> configs)
        {
            parCtrl.Controls.Clear();
            int lastTop = cstInitTop;
            int lastHeight = 0;
            foreach (string key in configs.Keys)
            {
                EmrAppConfig eac = configs[key];
                Control ctrl = CreateDesignControl(eac);
                if (ctrl == null) continue;
                ctrl.Tag = eac;
                ctrl.Left = cstInitLeft;
                ctrl.Top = lastTop + lastHeight + cstInitInterval;
                lastTop = ctrl.Top;
                lastHeight = ctrl.Height;
                if (ctrl.Dock == DockStyle.None)
                    ctrl.Dock = DockStyle.Top;
                //ctrl.Anchor = (AnchorStyles)((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right);

                parCtrl.Controls.Add(ctrl);
            }
        }

        public void Save()
        {
        }

        private Control CreateDesignControl(EmrAppConfig eac)
        {
            switch (eac.Ptype)
            {
                case ConfigParamType.Boolean:
                    return GenCheckBox(eac);
                case ConfigParamType.Set:
                    return GenGroupControl(eac);
                case ConfigParamType.Color:
                case ConfigParamType.Double:
                case ConfigParamType.Integer:
                case ConfigParamType.String:
                    return GenCaptionTextBox(eac);
                case ConfigParamType.Var:
                case ConfigParamType.Xml:
                    return GenRichTextBox(eac);
                default:
                    return null;
            }
        }

        public object ConfigObj
        {
            get { return null; }
        }

        #endregion

        #region Create Control By Type

        CheckBox GenCheckBox(EmrAppConfig eac)
        {
            CheckBox cb = new CheckBox();
            cb.Text = eac.Descript;
            cb.AutoSize = true;
            cb.Checked = eac.Config == bool.TrueString;
            cb.CheckStateChanged += new EventHandler(cb_CheckStateChanged);
            return cb;
        }

        void cb_CheckStateChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb == null) return;
            EmrAppConfig eac = (sender as Control).Tag as EmrAppConfig;
            if (eac == null) return;
            eac.Config = cb.Checked.ToString();
            ValidateUpdateConfigs(eac);
        }

        GroupControl GenGroupControl(EmrAppConfig eac)
        {
            GroupControl gctrl = new GroupControl();
            GenDesignInterface(gctrl, eac.SubConfigs);
            return gctrl;
        }

        CaptionEditBox GenCaptionTextBox(EmrAppConfig eac)
        {
            CaptionEditBox ctb = null;
            switch (eac.Ptype)
            {
                case ConfigParamType.Double:
                    ctb = new CaptionDoubleBox();
                    break;
                case ConfigParamType.Integer:
                    ctb = new CaptionIntBox();
                    break;
                case ConfigParamType.Color:
                    ctb = new CaptionColorBox();
                    break;
                default:
                    ctb = new CaptionTextBox();
                    break;
            }
            ctb.Caption = eac.Descript;
            ctb.Config = eac.Config;
            ctb.ConfigChanged += new EventHandler(ctb_ConfigChanged);
            return ctb;
        }

        void ctb_ConfigChanged(object sender, EventArgs e)
        {
            CaptionEditBox ctb = sender as CaptionEditBox;
            if (ctb == null) return;
            EmrAppConfig eac = ctb.Tag as EmrAppConfig;
            if (eac == null) return;
            eac.Config = ctb.Config;
            ValidateUpdateConfigs(eac);
        }

        RichTextBox GenRichTextBox(EmrAppConfig eac)
        {
            RichTextBox rtb = new RichTextBox();
            rtb.SelectedText = eac.Config;
            rtb.TextChanged += new EventHandler(rtb_TextChanged);
            rtb.Dock = DockStyle.Fill;
            return rtb;
        }

        void rtb_TextChanged(object sender, EventArgs e)
        {
            RichTextBox rtb = sender as RichTextBox;
            if (rtb == null)
                return;
            EmrAppConfig eac = rtb.Tag as EmrAppConfig;
            if (eac == null)
                return;
            eac.Config = rtb.Text;
            ValidateUpdateConfigs(eac);
        }

        void ValidateUpdateConfigs(EmrAppConfig eac)
        {
            if (_updconfigs.ContainsKey(eac.Key)) _updconfigs.Remove(eac.Key);
            _updconfigs.Add(eac.Key, eac);
        }

        #endregion
    }

}
