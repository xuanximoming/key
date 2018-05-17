using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DrectSoft.Common.Library;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Wordbook;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
namespace DrectSoft.Core.TimeLimitQC
{
    /// <summary>
    /// 时限规则设置窗口
    /// </summary>
    public partial class FormTimeQCConfig : NoCaptionBarForm, IStartPlugIn
    {
        #region fields

        IList<QCRule> _rules;
        IList<QCCondition> _conditions;
        IList<QCResult> _results;
        IList<QCRuleGroup> _rulegroups;
        IEmrHost app;
        QCRule _currentRule;
        bool _needSave = false;
        LookUpWindow _showlistwindow;
        SqlWordbook _rulewordbook;
        IDataAccess _sqlHelper;
        Hashtable _proptrees = new Hashtable();
        IQCDataDal _qcObjdal;
        QcObject _qcObj;

        private enum EditState
        {
            None = 0,
            Rule = 1,
            Condition = 2,
            Result = 3,
        }
        EditState _editState = EditState.None;

        #endregion

        #region ctor

        /// <summary>
        /// 构造
        /// </summary>
        public FormTimeQCConfig()
        {
            InitializeComponent();
            _sqlHelper = DataAccessFactory.GetSqlDataAccess();
            _showlistwindow = new LookUpWindow();
            _showlistwindow.MaxCount = 9999;
            _showlistwindow.SqlHelper = _sqlHelper;
            _rulewordbook = new SqlWordbook(
                ConstRes.cstTableNameQcRule, ConstRes.cstSqlSelectQcRule,
                "RuleCode", "Description", ConstRes.cstShowlistColsQcRule, "RuleCode//Description");

            //默认会设置为AutoFilter,赋值后会显示下拉框
            textBoxConditionId.Properties.SearchMode = SearchMode.OnlyInPopup;
            textBoxResultId.Properties.SearchMode = SearchMode.OnlyInPopup;

            xtraTabControlEdit.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;

            InitConditionAndResultType();
            ReloadRules();

            #region add events

            buttonLoadRule.Click += new EventHandler(buttonLoadRule_Click);
            buttonNewRule.Click += new EventHandler(buttonNewRule_Click);
            buttonEditRule.Click += new EventHandler(buttonEditRule_Click);
            buttonSaveRule.Click += new EventHandler(buttonSaveRule_Click);
            buttonDelRule.Click += new EventHandler(buttonDelRule_Click);
            buttonNewQcObj.Click += new EventHandler(buttonNewQcObj_Click);
            buttonSaveQcObj.Click += new EventHandler(buttonSaveQcObj_Click);
            gridViewRules.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(gridViewRules_FocusedRowChanged);
            gridViewRules.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(gridViewRules_CustomDrawCell);
            gridViewRules.DoubleClick += new EventHandler(gridViewRules_DoubleClick);
            textBoxConditionId.EditValueChanged += new EventHandler(textBoxConditionId_EditValueChanged);
            textBoxResultId.EditValueChanged += new EventHandler(textBoxResultId_EditValueChanged);
            checkEditValid.CheckedChanged += new EventHandler(checkEditValid_CheckedChanged);
            textEditRelateRules.KeyDown += new KeyEventHandler(listBoxRelateRules_KeyDown);
            comboBoxOpMode.SelectedIndexChanged += new EventHandler(comboBoxOpMode_SelectedIndexChanged);
            lookUpEditRuleGroup.EditValueChanged += new EventHandler(lookUpEditRuleGroup_EditValueChanged);
            buttonConditionEdit.Click += new EventHandler(buttonConditionEdit_Click);
            buttonResultEdit.Click += new EventHandler(buttonResultEdit_Click);
            gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(gridView1_FocusedRowChanged);
            comboBoxTypeEdit.SelectedIndexChanged += new EventHandler(comboBoxTypeEdit_SelectedIndexChanged);

            #endregion
        }

        #endregion

        #region event handler

        void comboBoxTypeEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxTypeEdit.SelectedIndex < 0) return;
            propertyInfoTree1.SetClassParams((Enum2Chinese.ChineseEnum)comboBoxTypeEdit.SelectedItem);
        }

        void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            int frh = e.FocusedRowHandle;
            if (frh < 0) return;
            _qcObj = gridView1.GetRow(frh) as QcObject;
            ShowQcObject();
            textEditIdEdit.Enabled = false;
        }

        void buttonNewQcObj_Click(object sender, EventArgs e)
        {
            _qcObj = NewQcObject();
            ClearQcObjectEditor();
            textEditIdEdit.Enabled = true;
        }

        void buttonSaveQcObj_Click(object sender, EventArgs e)
        {
            SaveQcObject();
            RefreshQcObjectGrid();
            if (_editState == EditState.Condition) ReloadConditions();
            if (_editState == EditState.Result) ReloadResults();
        }

        void buttonConditionEdit_Click(object sender, EventArgs e)
        {
            if (_editState == EditState.Condition)
            {
                buttonConditionEdit.Text = "维护条件 &C";
                SetEditState(EditState.None);
            }
            else
            {
                buttonConditionEdit.Text = "返回";
                SetEditState(EditState.Condition);
            }
        }

        void buttonResultEdit_Click(object sender, EventArgs e)
        {
            if (_editState == EditState.Result)
            {
                buttonResultEdit.Text = "维护操作 &W";
                SetEditState(EditState.None);
            }
            else
            {
                buttonResultEdit.Text = "返回";
                SetEditState(EditState.Result);
            }
        }

        void gridViewRules_DoubleClick(object sender, EventArgs e)
        {
            if (gridViewRules.FocusedRowHandle < 0) return;
            SetEditState(EditState.Rule);
        }

        void gridViewRules_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                QCRule rule = gridViewRules.GetRow(e.RowHandle) as QCRule;
                if (rule != null)
                {
                    if (rule.Invalid)
                        e.Appearance.ForeColor = Color.Red;
                }
            }
        }

        void gridViewRules_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0)
            {
                _currentRule = null;   //防止清空编辑控件时改动对象相关属性
                ClearEditInterface();
                _currentRule = gridViewRules.GetRow(e.FocusedRowHandle) as QCRule;
                ShowARule(_currentRule);
            }
        }

        void lookUpEditRuleGroup_EditValueChanged(object sender, EventArgs e)
        {
            QCRuleGroup qrg = lookUpEditRuleGroup.Properties.GetDataSourceRowByKeyValue(lookUpEditRuleGroup.EditValue) as QCRuleGroup;
            if (_currentRule != null)
                _currentRule.Group = qrg;
        }

        void comboBoxOpMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RuleDealType.Loop ==
                (RuleDealType)((Enum2Chinese.ChineseEnum)comboBoxOpMode.SelectedItem).Value)
            {
                textEditLooptimes.Enabled = true;
                textEditLoopinterval.Enabled = true;
                if (!string.IsNullOrEmpty(textBoxRuleTime.Text))
                    textEditLoopinterval.Text = textBoxRuleTime.Text;
            }
            else
            {
                textEditLooptimes.Enabled = false;
                textEditLoopinterval.Enabled = false;
            }
        }

        void listBoxRelateRules_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Insert)
            {
                string codevalue = _currentRule.RelateRuleIds;
                _showlistwindow.CallLookUpWindow(_rulewordbook, WordbookKind.Sql, codevalue,
                     ShowListFormMode.Full, textEditRelateRules.PointToScreen(textEditRelateRules.Location),
                     new Size(100, 25), this.Bounds);
                _currentRule.ClearRelateRules();
                if (!string.IsNullOrEmpty(_showlistwindow.CodeValue))
                {
                    string[] ruleids = _showlistwindow.CodeValue.Split(',');
                    foreach (string ruleid in ruleids)
                    {
                        QCRule qcr = QCRule.SelectQCRule(ruleid).Clone();
                        _currentRule.AddRelateRule(qcr);
                    }
                }
                textEditRelateRules.Text = _currentRule.RelateRuleIds;
            }
        }

        void checkEditValid_CheckedChanged(object sender, EventArgs e)
        {
            if (_currentRule == null) return;
            _currentRule.Invalid = !checkEditValid.Checked;
        }

        void textBoxResultId_EditValueChanged(object sender, EventArgs e)
        {
            if (textBoxResultId.EditValue == null) return;
            QCResult qcr = QCResult.SelectQCResult(textBoxResultId.EditValue.ToString());
            FillResult2Editor(qcr);
            if (_currentRule != null) _currentRule.Result = qcr;
        }

        void textBoxConditionId_EditValueChanged(object sender, EventArgs e)
        {
            if (textBoxConditionId.EditValue == null) return;
            QCCondition qcc = QCCondition.SelectQCCondition(textBoxConditionId.EditValue.ToString());
            FillCondition2Editor(qcc);
            if (_currentRule != null) _currentRule.Condition = qcc;
        }

        /// <summary>
        /// 保存规则（写入XML规则）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void buttonSaveRule_Click(object sender, EventArgs e)
        {
            SaveCurrentRule();
        }

        void buttonLoadRule_Click(object sender, EventArgs e)
        {
            ReloadRules();
        }

        void buttonNewRule_Click(object sender, EventArgs e)
        {
            ClearEditInterface();
            NewRule();
            SetEditState(EditState.Rule);
        }

        void buttonEditRule_Click(object sender, EventArgs e)
        {
            SetEditState(EditState.Rule);
        }

        void buttonDelRule_Click(object sender, EventArgs e)
        {
            if (_currentRule == null) return;
            DialogResult dlgr = app.CustomMessageBox.MessageShow(ConstRes.cstConfirmDelete, CustomMessageBoxKind.QuestionYesNo);
            if (dlgr == DialogResult.Yes)
            {
                QCRule.DeleteQCRule(_currentRule);
                ReloadRules();
            }
        }

        #endregion

        #region procedures

        #region init

        Enum2Chinese e2cConditionType = new Enum2Chinese(typeof(QCConditionType));
        Enum2Chinese e2cResultType = new Enum2Chinese(typeof(QCResultType));
        Enum2Chinese e2cOpMode = new Enum2Chinese(typeof(RuleDealType));
        Enum2Chinese e2cRelateOpMode = new Enum2Chinese(typeof(RelateRuleDealType));
        void InitConditionAndResultType()
        {
            comboBoxConditionType.Properties.Items.Clear();
            comboBoxConditionType.Properties.Items.AddRange(e2cConditionType.EnumNames);

            comboBoxResultType.Properties.Items.Clear();
            comboBoxResultType.Properties.Items.AddRange(e2cResultType.EnumNames);

            comboBoxOpMode.Properties.Items.Clear();
            comboBoxOpMode.Properties.Items.AddRange(e2cOpMode.EnumNames);

            comboBoxRelateRuleOpMode.Properties.Items.Clear();
            comboBoxRelateRuleOpMode.Properties.Items.AddRange(e2cRelateOpMode.EnumNames);
        }

        void ReloadRules()
        {
            ReloadConditions();
            ReloadResults();
            ReloadRuleGroups();
            _rules = QCRule.GetAllRules(_conditions, _results);

            if (_conditions == null || _results == null || _rules == null) return;

            gridRules.DataSource = _rules;
            SetGridViewRule();
            SetEditState(EditState.None);
        }

        void SetGridViewRule()
        {
            gridViewRules.Columns["Id"].Width = 80;
            gridViewRules.Columns["Group"].Visible = false;
            gridViewRules.Columns["Group"].Group();
            gridViewRules.ExpandAllGroups();
        }

        void ReloadConditions()
        {
            _conditions = QCCondition.AllConditions;
            textBoxConditionId.Properties.DataSource = _conditions;
            AddLookupColumnInfo(textBoxConditionId, new KeyValuePair<string, string>[]{
                new KeyValuePair<string,string>("Id", ConstRes.cstFieldCaptionId), 
                new KeyValuePair<string,string>("Name", ConstRes.cstFieldCaptionDescript)
            });
            textBoxConditionId.Properties.DisplayMember = "Id";
            textBoxConditionId.Properties.ValueMember = "Id";
        }

        void ReloadResults()
        {
            _results = QCResult.AllResults;
            textBoxResultId.Properties.DataSource = _results;
            AddLookupColumnInfo(textBoxResultId, new KeyValuePair<string, string>[]{
                new KeyValuePair<string,string>("Id", ConstRes.cstFieldCaptionId), 
                new KeyValuePair<string,string>("Name", ConstRes.cstFieldCaptionDescript)
            });
            textBoxResultId.Properties.DisplayMember = "Id";
            textBoxResultId.Properties.ValueMember = "Id";
        }

        void ReloadRuleGroups()
        {
            _rulegroups = QCRuleGroup.AllRuleGroups;
            lookUpEditRuleGroup.Properties.DataSource = _rulegroups;
            AddLookupColumnInfo(lookUpEditRuleGroup, new KeyValuePair<string, string>[]{
                new KeyValuePair<string,string>("Id", ConstRes.cstFieldCaptionId), 
                new KeyValuePair<string,string>("Name", ConstRes.cstFieldCaptionDescript)
            });
            lookUpEditRuleGroup.Properties.DisplayMember = "Name";
            lookUpEditRuleGroup.Properties.ValueMember = "Id";
        }

        #endregion

        #region edit rule

        void NewRule()
        {
            if (_currentRule != null && _needSave)
            {
                // tip for save
            }
            _currentRule = new QCRule();
        }

        void ShowARule(QCRule rule)
        {
            if (rule == null) return;

            textBoxConditionId.Text = rule.Condition.Id;
            textBoxConditionName.Text = rule.Condition.Name;
            textBoxConditionParams.Text = rule.Condition.JudgeSetting;
            comboBoxConditionType.SelectedIndex = ((int)rule.Condition.ConditionType) - 1;

            textBoxResultId.Text = rule.Result.Id;
            textBoxResultName.Text = rule.Result.Name;
            textBoxResultParams.Text = rule.Result.JudgeSetting;
            comboBoxResultType.SelectedIndex = ((int)rule.Result.ResultType) - 1;

            textBoxRuleId.Text = rule.Id;
            textBoxRuleName.Text = rule.Name;
            comboBoxDutyLevel.SelectedIndex = (int)rule.Dutylevel;
            textBoxRuleTime.Text = QCRule.TimeSpan2LimitString(rule.Timelimit);
            textBoxTipInfo.Text = rule.TipInfo;
            textBoxWarningInfo.Text = rule.WarnInfo;
            checkEditValid.Checked = !rule.Invalid;
            comboBoxOpMode.SelectedIndex = (int)rule.DealType;
            if (rule.DealType == RuleDealType.Loop)
            {
                textEditLooptimes.Text = rule.LoopTimes.ToString();
                textEditLoopinterval.Text = QCRule.TimeSpan2LimitString(rule.LoopTimeInterVal);
            }
            comboBoxRelateRuleOpMode.SelectedIndex = (int)rule.RelateDealType;
            textEditRelateRules.Text = rule.RelateRuleIds;

            if (_currentRule.Group != null)
                lookUpEditRuleGroup.EditValue = _currentRule.Group.Id;
        }

        void SetEditState(EditState state)
        {
            _editState = state;
            switch (state)
            {
                case EditState.None:
                    SetRuleEditState(false, false);
                    xtraTabControlEdit.SelectedTabPage = xtraTabPageRule;
                    break;
                case EditState.Rule:
                    SetRuleEditState(true, _currentRule.IsNew);
                    xtraTabControlEdit.SelectedTabPage = xtraTabPageRule;
                    break;
                case EditState.Condition:
                    _qcObjdal = QCCondition.Dal;
                    gridView1.Columns.Clear();
                    gridControlEdit.DataSource = QCCondition.AllConditions;
                    xtraTabControlEdit.SelectedTabPage = xtraTabPageQcObj;
                    InitQcObjectInnerKind();
                    break;
                case EditState.Result:
                    _qcObjdal = QCResult.Dal;
                    gridView1.Columns.Clear();
                    gridControlEdit.DataSource = QCResult.AllResults;
                    xtraTabControlEdit.SelectedTabPage = xtraTabPageQcObj;
                    InitQcObjectInnerKind();
                    break;
                default:
                    break;
            }
            SetButtonState();
        }

        void SetButtonState()
        {
            switch (_editState)
            {
                case EditState.None:
                case EditState.Rule:
                    buttonConditionEdit.Enabled = true;
                    buttonResultEdit.Enabled = true;
                    panelRuleButtons.Enabled = true;
                    this.splitContainerControl1.PanelVisibility = SplitPanelVisibility.Both;
                    break;
                case EditState.Condition:
                    panelRuleButtons.Enabled = false;
                    buttonResultEdit.Enabled = false;
                    this.splitContainerControl1.PanelVisibility = SplitPanelVisibility.Panel2;
                    break;
                case EditState.Result:
                    panelRuleButtons.Enabled = false;
                    buttonConditionEdit.Enabled = false;
                    this.splitContainerControl1.PanelVisibility = SplitPanelVisibility.Panel2;
                    break;
                default:
                    break;
            }
        }

        void SetRuleEditState(bool isEdit, bool isNew)
        {
            groupCondition.Enabled = isEdit;
            groupResult.Enabled = isEdit;
            groupRule.Enabled = isEdit;
            buttonDelRule.Enabled = !isNew;

            if (!isEdit)
                labelEditInfo.Text = ConstRes.cstEditStatusBrowse;
            else
            {
                if (isNew)
                    labelEditInfo.Text = ConstRes.cstEditStatusNew;
                else
                    labelEditInfo.Text = ConstRes.cstEditStatusModify;
            }
        }

        void ClearEditInterface()
        {
            ClearConditionEditInterface();
            ClearResultEditInterface();

            textBoxRuleId.Text = string.Empty;
            textBoxRuleName.Text = string.Empty;
            comboBoxDutyLevel.SelectedIndex = -1;
            textBoxRuleTime.Text = string.Empty;
            textBoxTipInfo.Text = string.Empty;
            textBoxWarningInfo.Text = string.Empty;
            checkEditValid.Checked = true;
            textEditLooptimes.Text = "0";
            textEditLoopinterval.Text = string.Empty;
            lookUpEditRuleGroup.EditValue = null;
        }

        void ClearConditionEditInterface()
        {
            textBoxConditionId.EditValue = null;
            textBoxConditionName.Text = string.Empty;
            textBoxConditionParams.Text = string.Empty;
            comboBoxConditionType.SelectedIndex = -1;
        }

        void ClearResultEditInterface()
        {
            textBoxResultId.EditValue = null;
            textBoxResultName.Text = string.Empty;
            textBoxResultParams.Text = string.Empty;
            comboBoxResultType.SelectedIndex = -1;
        }

        void SaveCurrentRule()
        {
            if (_currentRule == null) return;

            string newId = textBoxRuleId.Text;
            string newName = textBoxRuleName.Text;
            if (string.IsNullOrEmpty(newId))
            {
                app.CustomMessageBox.MessageShow(ConstRes.cstSaveCheckNeedQcRuleId);
                textBoxRuleId.Focus();
                return;
            }
            if (string.IsNullOrEmpty(newName))
            {
                app.CustomMessageBox.MessageShow(ConstRes.cstSaveCheckNeedQcRuleName);
                textBoxRuleName.Focus();
                return;
            }
            if (_currentRule.IsNew)
            {
                if (QCRule.IdIsExisted(newId))
                {
                    app.CustomMessageBox.MessageShow(ConstRes.cstSaveCheckExistSameRuleId);
                    textBoxRuleId.Focus();
                    return;
                }
                _currentRule.Id = newId;
                _currentRule.Name = newName;
            }

            if (comboBoxDutyLevel.SelectedIndex >= 0)
                _currentRule.Dutylevel = (DutyLevel)(comboBoxDutyLevel.SelectedIndex);
            else
                _currentRule.Dutylevel = DutyLevel.All;
            _currentRule.Timelimit = QCRule.LimitString2TimeSpan(textBoxRuleTime.Text);
            _currentRule.TipInfo = textBoxTipInfo.Text;
            _currentRule.WarnInfo = textBoxWarningInfo.Text;
            if (comboBoxOpMode.SelectedIndex >= 0)
                _currentRule.DealType = (RuleDealType)((Enum2Chinese.ChineseEnum)comboBoxOpMode.SelectedItem).Value;
            if (_currentRule.DealType == RuleDealType.Loop)
            {
                _currentRule.LoopTimes = int.Parse(textEditLooptimes.Text);
                _currentRule.LoopTimeInterVal = QCRule.LimitString2TimeSpan(textEditLoopinterval.Text);
            }
            if (comboBoxRelateRuleOpMode.SelectedIndex >= 0)
                _currentRule.RelateDealType = (RelateRuleDealType)((Enum2Chinese.ChineseEnum)comboBoxRelateRuleOpMode.SelectedItem).Value;

            QCRule.SaveQCRule(_currentRule);
            app.CustomMessageBox.MessageShow(ConstRes.cstSaveSuccess);
            ReloadRules();
        }

        #endregion

        #region edit Conds & Results

        void InitQcObjectInnerKind()
        {
            comboBoxTypeEdit.Properties.Items.Clear();
            switch (_editState)
            {
                case EditState.Condition:
                    comboBoxTypeEdit.Properties.Items.AddRange(e2cConditionType.EnumNames);
                    break;
                case EditState.Result:
                    comboBoxTypeEdit.Properties.Items.AddRange(e2cResultType.EnumNames);
                    break;
                default:
                    break;
            }
        }

        void RefreshQcObjectGrid()
        {
            //gridControlEdit.DataSource = null;
            gridView1.Columns.Clear();
            switch (_editState)
            {
                case EditState.Condition:
                    gridControlEdit.DataSource = QCCondition.AllConditions;
                    break;
                case EditState.Result:
                    gridControlEdit.DataSource = QCResult.AllResults;
                    break;
                default:
                    break;
            }
        }

        void ClearQcObjectEditor()
        {
            textEditIdEdit.Text = string.Empty;
            textEditDescriptEdit.Text = string.Empty;
            propertyInfoTree1.SetClassParams(string.Empty);
        }

        void ShowQcObject()
        {
            if (_qcObj == null) return;
            textEditIdEdit.Text = _qcObj.Id;
            textEditDescriptEdit.Text = _qcObj.Name;
            switch (_editState)
            {
                case EditState.Condition:
                    comboBoxTypeEdit.SelectedItem = e2cConditionType.FindChineseEnum(_qcObj.GetQcObjInnerKind() as Enum);
                    break;
                case EditState.Result:
                    comboBoxTypeEdit.SelectedItem = e2cResultType.FindChineseEnum(_qcObj.GetQcObjInnerKind() as Enum);
                    break;
                default:
                    break;
            }
            propertyInfoTree1.SetClassParams(_qcObj.JudgeSetting);
        }

        QcObject NewQcObject()
        {
            switch (_editState)
            {
                case EditState.Condition:
                    return QcObjectFactory.CreateQcObjectByType(QcObjType.Condition);
                case EditState.Result:
                    return QcObjectFactory.CreateQcObjectByType(QcObjType.Result);
                default:
                    return null;
            }
        }

        bool SetQcObjectFromEditor()
        {
            if (_qcObj == null) return false;
            string newId = textEditIdEdit.Text;
            if (_qcObj.IsNew)
            {
                if (string.IsNullOrEmpty(newId) || QCCondition.IdIsExisted(newId))
                {
                    string msg = ConstRes.cstSaveCheckExistSameId;
                    switch (_editState)
                    {
                        case EditState.Condition:
                            msg = ConstRes.cstSaveCheckExistSameCondId;
                            break;
                        case EditState.Result:
                            msg = ConstRes.cstSaveCheckExistSameResultId;
                            break;
                        default:
                            break;
                    }
                    app.CustomMessageBox.MessageShow(msg);
                    textBoxConditionId.Focus();
                    return false;
                }
                else
                {
                    _qcObj.Id = newId;
                    _qcObj.Name = textEditDescriptEdit.Text;
                }
            }
            if (comboBoxTypeEdit.SelectedItem == null)
            {
                comboBoxTypeEdit.Focus();
                return false;
            }
            if (propertyInfoTree1.QcParams.Settings == null || propertyInfoTree1.QcParams.Settings.Count == 0)
            {
                propertyInfoTree1.Focus();
                return false;
            }
            _qcObj.SetQcObjInnerKind((QCConditionType)((Enum2Chinese.ChineseEnum)comboBoxTypeEdit.SelectedItem).Value);
            _qcObj.JudgeSetting = propertyInfoTree1.QcParams.ToString();
            return true;
        }

        void SaveQcObject()
        {
            if (_qcObj == null) return;
            if (!SetQcObjectFromEditor()) return;
            try
            {
                _qcObjdal.SaveRecord(_qcObj);
                switch (_editState)
                {
                    case EditState.Condition:
                        QCCondition.AllConditions.Add(_qcObj as QCCondition);
                        break;
                    case EditState.Result:
                        QCResult.AllResults.Add(_qcObj as QCResult);
                        break;
                    default:
                        break;
                }
            }
            catch
            {
            }
        }

        #endregion

        /// <summary>
        /// 生成DevLookUpEdit的显示下拉列表
        /// </summary>
        /// <param name="lookupEdit"></param>
        /// <param name="table"></param>
        static void AddLookupColumnInfo(LookUpEdit lookupEdit, KeyValuePair<string, string>[] fields)
        {
            lookupEdit.Properties.Columns.Clear();
            for (int i = 0; i < fields.Length; i++)
            {
                KeyValuePair<string, string> field = fields[i];
                LookUpColumnInfo luci = new LookUpColumnInfo(field.Key, field.Value, 100);
                lookupEdit.Properties.Columns.Add(luci);
            }
        }

        void FillCondition2Editor(QCCondition condition)
        {
            if (condition == null) return;
            textBoxConditionName.Text = condition.Name;
            comboBoxConditionType.SelectedItem = e2cConditionType.FindChineseEnum(condition.ConditionType as Enum);
            textBoxConditionParams.Text = condition.JudgeSetting;
        }

        void FillResult2Editor(QCResult result)
        {
            if (result == null) return;
            textBoxResultName.Text = result.Name;
            comboBoxResultType.SelectedItem = e2cResultType.FindChineseEnum(result.ResultType as Enum);
            textBoxResultParams.Text = result.JudgeSetting;
        }

        void GenPropertyInfoTree(string typestring, string value, PopupContainerControl popcontainer)
        {
            if (string.IsNullOrEmpty(typestring)) return;
            PropertyInfoTree ppit = GetCachePropTree(typestring, popcontainer.Name);
            popcontainer.Controls.Clear();
            popcontainer.Controls.Add(ppit);
        }

        void FillPropertyInfoTree(string value, PopupContainerControl popcontainer)
        {
            if (popcontainer.Controls.Count > 0)
            {
                PropertyInfoTree ppit = popcontainer.Controls[0] as PropertyInfoTree;
                if (ppit != null) ppit.SetClassParams(value);
            }
        }

        PropertyInfoTree GetCachePropTree(string typestring, string tag)
        {
            if (!_proptrees.ContainsKey(typestring + tag))
            {
                Type at = Type.GetType(typestring);
                PropertyInfoTree ppit = new PropertyInfoTree();
                ppit.Dock = DockStyle.Fill;
                ppit.SetClassParams(at);
                _proptrees.Add(typestring + tag, ppit);
                return ppit;
            }
            else
                return _proptrees[typestring + tag] as PropertyInfoTree;
        }

        #endregion

        #region IStartup Members

        public virtual IPlugIn Run(IEmrHost application)
        {

            app = application;
            return new PlugIn(this.GetType().ToString(), this);
        }

        #endregion
    }
}