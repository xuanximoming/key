using DevExpress.Utils;
using DevExpress.XtraTab;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace DrectSoft.Core.CommonTableConfig.CommonNoteUse
{
    public partial class UCInCommonNote : DevExpress.XtraEditors.XtraUserControl
    {
        IEmrHost m_app;
        CommonNoteEntity m_commonNoteEntity;
        public InCommonNoteEnmtity m_inCommonNote;//edit by xlb 
        InCommonNoteBiz inCommonNoteBiz;
        UCPrintRecord ucPrintRecord; //打印界面
        bool m_canEdit;
        IPrintNurse iPrintNurse;
        List<string> DanweiList;
        List<char> BiaoDianlist;
        public UCInCommonNote(IEmrHost app, CommonNoteEntity commonNoteEntity, InCommonNoteEnmtity inCommonNote, bool canEdit)
        {
            try
            {
                m_app = app;
                m_commonNoteEntity = commonNoteEntity;
                m_inCommonNote = inCommonNote;
                m_canEdit = canEdit;
                //Test();
                InitializeComponent();
                GetBiaoDianAndFuHao();
                InitForm();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.StackTrace);
            }
        }

        /// <summary>
        /// 测试方法
        /// </summary>
        private void Test()
        {
            string message = "";
            CommonNoteBiz cbiz = new CommonNoteBiz(m_app);
            m_commonNoteEntity = cbiz.GetDetailCommonNote("f5d1c558-df2a-4ac5-8d1d-6ef3ccaef5e7");
            InCommonNoteBiz icombiz = new InCommonNoteBiz(m_app);
            List<InCommonNoteEnmtity> InCommonNoteList = icombiz.GetSimInCommonNote("1333");
            if (InCommonNoteList == null || InCommonNoteList.Count == 0)
            {
                m_inCommonNote = InCommonNoteBiz.ConvertCommonToInCommon(m_commonNoteEntity);
                DataTable inpatientDt = icombiz.GetInpatient("1333");
                m_inCommonNote.CurrDepartID = inpatientDt.Rows[0]["OUTHOSDEPT"].ToString();
                m_inCommonNote.CurrDepartName = inpatientDt.Rows[0]["DEPARTNAME"].ToString();
                m_inCommonNote.CurrWardID = inpatientDt.Rows[0]["OUTHOSWARD"].ToString();
                m_inCommonNote.CurrWardName = inpatientDt.Rows[0]["WARDNAME"].ToString();
                m_inCommonNote.NoofInpatient = "1333";
                m_inCommonNote.InPatientName = inpatientDt.Rows[0]["NAME"].ToString();
                bool saveResult = icombiz.SaveInCommomNoteAll(m_inCommonNote, ref message);
            }
            else
            {
                m_inCommonNote = InCommonNoteList[0];
            }
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        private void InitForm()
        {
            WaitDialogForm waitDialog = new WaitDialogForm("正在获取数据……", "请稍等。");
            try
            {
                if (inCommonNoteBiz == null)
                    inCommonNoteBiz = new InCommonNoteBiz(m_app);
                //xll 在初始化列表界面的时候获取值
                //inCommonNoteBiz.GetDetaliInCommonNote(ref m_inCommonNote);
                inCommonNoteBiz.GetIncommTab(ref m_inCommonNote);
                waitDialog.Hide();
                waitDialog.Close();
                //判断选择节点是否被删除，删除则给出提示
                if (m_inCommonNote == null || m_inCommonNote.InCommonNoteFlow == null)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该单据已被他人删除");
                    return;
                }
                //int countItem=  inCommonNoteBiz.GetCommonItemCount(m_inCommonNote.InCommonNoteFlow);
                foreach (var item in m_inCommonNote.InCommonNoteTabList)
                {
                    CommonNote_TabEntity commonNote_TabEntity = null;
                    foreach (var itemTab in m_commonNoteEntity.CommonNote_TabList)
                    {
                        if (itemTab.CommonNote_Tab_Flow == item.CommonNote_Tab_Flow)
                        {
                            commonNote_TabEntity = itemTab;
                            break;
                        }
                    }
                    if (commonNote_TabEntity == null)
                        continue;
                    XtraTabPage tabPage = new XtraTabPage();
                    tabPage.Tag = item;
                    tabPage.Text = item.CommonNoteTabName;
                    if (item.ShowType == "表格")
                    {
                        UCIncommonNoteTab UCIncommonNoteTab = new UCIncommonNoteTab(item, commonNote_TabEntity, m_inCommonNote, m_app, m_canEdit);
                        tabPage.Controls.Add(UCIncommonNoteTab);
                        UCIncommonNoteTab.Dock = DockStyle.Fill;
                    }
                    else
                    {
                        UCInCommonTabSingle ucInCommonTabSingle = new UCInCommonTabSingle(item, commonNote_TabEntity, m_inCommonNote, m_app, m_canEdit);
                        tabPage.Controls.Add(ucInCommonTabSingle);
                        ucInCommonTabSingle.Dock = DockStyle.Fill;
                    }
                    tabcontrol.TabPages.Add(tabPage);
                }
                tabcontrol.SelectedTabPageIndex = 1;
            }
            catch (Exception ex)
            {
                waitDialog.Hide();
                waitDialog.Close();
                throw ex;
            }
        }

        /// <summary>
        /// 切换tab触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabcontrol_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
        {
            if (tabcontrol.SelectedTabPage != tabPagePrint) return;
            WaitDialogForm waitDialog = new WaitDialogForm("正在组织打印数据……", "请稍等。");
            try
            {

                //xll 2013-03-26 获取全部数据 点击全部了 就无需再次查找 点击其他的 需要查找
                foreach (XtraTabPage item in tabcontrol.TabPages)
                {
                    if (item.Controls.Count > 0)
                    {
                        UCIncommonNoteTab uCIncommonNoteTab = item.Controls[0] as UCIncommonNoteTab;
                        if (uCIncommonNoteTab != null)
                        {
                            m_inCommonNote = uCIncommonNoteTab.GetAllDetailDate();
                            break;
                        }
                    }
                }
                PrintInCommonView printInCommonView = ConvertPrintEntity();

                iPrintNurse = AbstractorFactry.GetNurseRecord(printInCommonView.PrintFileName);
                SetWaitDialogCaption("正在绘制预览界面……", waitDialog);
                if (iPrintNurse != null && iPrintNurse.IsCustomPrint == true)
                {
                    if (tabPagePrint.Controls == null || tabPagePrint.Controls.Count <= 0)
                    {
                        PrintForm1 printForm1 = new PrintForm1(printInCommonView);
                        printForm1.Dock = DockStyle.Fill;
                        printForm1.TopLevel = false;
                        printForm1.FormBorderStyle = FormBorderStyle.None;
                        printForm1.Show();
                        tabPagePrint.Controls.Add(printForm1);
                    }
                    else
                    {
                        PrintForm1 printForm1 = tabPagePrint.Controls[0] as PrintForm1;
                        if (printForm1 != null)
                        {
                            printForm1.m_printInCommonView = printInCommonView;
                            printForm1.RefreshView();
                        }
                    }
                }
                else
                {

                    if (ucPrintRecord == null)
                    {
                        ucPrintRecord = new UCPrintRecord();
                        tabPagePrint.Controls.Add(ucPrintRecord);
                        ucPrintRecord.Dock = DockStyle.Fill;
                    }
                    ucPrintRecord.LoadPrint(printInCommonView);
                }
                waitDialog.Hide();
                waitDialog.Close();
            }
            catch (Exception ex)
            {
                waitDialog.Hide();
                waitDialog.Close();
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 设置等待框
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="waitDialog"></param>
        public void SetWaitDialogCaption(string caption, WaitDialogForm waitDialog)
        {
            try
            {
                if (waitDialog != null)
                {
                    if (!waitDialog.Visible)
                        waitDialog.Visible = true;
                    waitDialog.Caption = caption;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 将数据对象转诊打印对象 用于绑定到控件进行打印预览
        /// </summary>
        /// <returns></returns>
        private PrintInCommonView ConvertPrintEntity()
        {
            //inCommonNoteBiz.GetDetaliInCommonNote(ref m_inCommonNote);
            PrintInCommonView printInCommonView = new CommonNoteUse.PrintInCommonView();
            printInCommonView.PrintFileName = m_inCommonNote.PrinteModelName;
            printInCommonView.IncommonNoteflow = m_inCommonNote.InCommonNoteFlow;
            printInCommonView.RecordName = m_commonNoteEntity.CommonNoteName;
            //病人的基本信息构造
            if (inCommonNoteBiz == null)
                inCommonNoteBiz = new InCommonNoteBiz(m_app);
            DataTable inpatientDt = inCommonNoteBiz.GetInpatient(m_inCommonNote.NoofInpatient);
            PrintInpatientView printInpatientView = new CommonNoteUse.PrintInpatientView();
            printInpatientView.HospitalName = m_app.CurrentHospitalInfo.Name;
            printInpatientView.SubHospitalName = m_app.CurrentHospitalInfo.Subname;
            printInpatientView.AdmitDiagnosis = inpatientDt.Rows[0]["DIAGNOSISNAME"].ToString();
            printInpatientView.Depart = m_inCommonNote.CurrDepartName;
            printInpatientView.InBedNo = inpatientDt.Rows[0]["OUTBED"].ToString();
            printInpatientView.RecordName = m_inCommonNote.InCommonNoteName;
            if (inpatientDt.Rows[0]["INWARDDATE"].ToString().Length >= 10)
            {
                printInpatientView.InDateTime = inpatientDt.Rows[0]["INWARDDATE"].ToString().Substring(0, 10);
            }
            printInpatientView.InNo = inpatientDt.Rows[0]["PATID"].ToString();
            printInpatientView.OutNo = inpatientDt.Rows[0]["NOOFCLINIC"].ToString();
            printInpatientView.PatId = inpatientDt.Rows[0]["PATNOOFHIS"].ToString();
            printInpatientView.InpatientAge = inpatientDt.Rows[0]["AGESTR"].ToString();
            printInpatientView.InpatientName = inpatientDt.Rows[0]["NAME"].ToString();
            printInpatientView.Sex = inpatientDt.Rows[0]["SEX"].ToString();
            printInpatientView.Ward = m_inCommonNote.CurrWardName;
            printInCommonView.PrintInpatientView = printInpatientView;

            if (m_inCommonNote.InCommonNoteTabList == null) return printInCommonView;
            for (int i = 0; i < m_inCommonNote.InCommonNoteTabList.Count; i++)
            {
                CommonNote_TabEntity commonNote_TabEntity = null;
                foreach (var itemTab in m_commonNoteEntity.CommonNote_TabList)
                {
                    if (itemTab.CommonNote_Tab_Flow == m_inCommonNote.InCommonNoteTabList[i].CommonNote_Tab_Flow)
                    {
                        commonNote_TabEntity = itemTab;
                        break;
                    }
                }
                PrintInCommonTabView printInCommonTabView = new CommonNoteUse.PrintInCommonTabView();  //主要目的是给这个对象复制
                Dictionary<string, List<InCommonNoteItemEntity>> dicitemList;
                List<string> strNames = InCommonNoteBiz.ConvertInCommonTabToPrint(m_inCommonNote.InCommonNoteTabList[i], printInCommonTabView, out dicitemList, commonNote_TabEntity, m_app, m_inCommonNote);

                string proName = "PrintInCommonTabViewList";
                proName = proName + (i + 1);
                PropertyInfo property = printInCommonView.GetType().GetProperty(proName);
                if (property != null)
                {
                    property.SetValue(printInCommonView, printInCommonTabView, null);
                }

                if (i == 0 && m_inCommonNote.InCommonNoteTabList[i] != null)
                {

                    ConverToDuoRow(printInCommonView);
                    //ConvertForDuoLie(printInCommonView);


                    IPrintNurse iPrintNurse = AbstractorFactry.GetNurseRecord(printInCommonView.PrintFileName);
                    if (iPrintNurse != null)
                    {
                        iPrintNurse.commonNoteCountEntity = CommonNoteBiz.GetCommonNoteCount(m_commonNoteEntity.CommonNoteFlow);

                        printInCommonView.PrintInCommonTabViewList1.PrintInCommonItemViewList = iPrintNurse.JiSuanZongLiang(printInCommonView);
                    }
                    else
                    {
                        if (printInCommonView != null
                            && printInCommonView.PrintInCommonTabViewList1 != null
                            && printInCommonView.PrintInCommonTabViewList1.PrintInCommonItemViewList != null)
                        {
                            InCommonNoteBiz.ConvertForDateTime(printInCommonView.PrintInCommonTabViewList1.PrintInCommonItemViewList);
                            InCommonNoteBiz.SetRowEnd(printInCommonView.PrintInCommonTabViewList1.PrintInCommonItemViewList, printInCommonView.PrintFileName);
                        }
                    }



                    //xll 2013-01-12 处理相同时间数据
                    //ConvertForDateTime(printInCommonView);
                }
            }
            return printInCommonView;
        }

        /// <summary>
        /// 将一列变成可绑定的多列  最大只有5大列
        /// </summary> 
        /// <param name="printInCommonView"></param>
        private void ConvertForDuoLie(PrintInCommonView printInCommonView)
        {
            List<PrintInCommonItemView> printInCommonItemViewListOld = printInCommonView.PrintInCommonTabViewList1.PrintInCommonItemViewList;
            if (printInCommonItemViewListOld == null) return;
            Dictionary<string, string> dicstr = InCommonNoteBiz.GetReportxmlCol(printInCommonView);
            if (!dicstr.ContainsKey("maxCols")) return;
            string maxcols = dicstr["maxCols"]; //大列数
            int maxcolsint; //大列
            bool ismax = int.TryParse(maxcols, out maxcolsint);
            if (maxcolsint <= 1 || maxcolsint > 5) return;
            List<PrintInCommonItemView> printInCommonItemViewListNew = new List<PrintInCommonItemView>();
            for (int i = 0; i < printInCommonItemViewListOld.Count; i++)
            {
                int yushu = i % maxcolsint;
                if (yushu == 0)
                {
                    PrintInCommonItemView printInCommonItemView = new PrintInCommonItemView();
                    if (maxcolsint >= 5
                        && (i + 4 < printInCommonItemViewListOld.Count))
                    {
                        printInCommonItemView.PrintInCommonItemView5 = printInCommonItemViewListOld[i + 4];
                    }
                    if (maxcolsint >= 4
                        && (i + 3 < printInCommonItemViewListOld.Count))
                    {
                        printInCommonItemView.PrintInCommonItemView4 = printInCommonItemViewListOld[i + 3];
                    }
                    if (maxcolsint >= 3
                      && (i + 2 < printInCommonItemViewListOld.Count))
                    {
                        printInCommonItemView.PrintInCommonItemView3 = printInCommonItemViewListOld[i + 2];
                    }
                    if (maxcolsint >= 2
                     && (i + 1 < printInCommonItemViewListOld.Count))
                    {
                        printInCommonItemView.PrintInCommonItemView2 = printInCommonItemViewListOld[i + 1];
                    }
                    printInCommonItemView.PrintInCommonItemView1 = printInCommonItemViewListOld[i];

                    printInCommonItemViewListNew.Add(printInCommonItemView);  //添加整理好的一行
                }
            }

            printInCommonView.PrintInCommonTabViewList1.PrintInCommonItemViewList = printInCommonItemViewListNew;

        }

        /// <summary>
        /// 打印表格时一行不过换行显示
        /// </summary>
        /// <param name="printInCommonView"></param>
        private void ConvertForChangeRow(PrintInCommonView printInCommonView)
        {
            List<PrintInCommonItemView> printInCommonItemViewListOld = printInCommonView.PrintInCommonTabViewList1.PrintInCommonItemViewList;
            if (printInCommonItemViewListOld == null) return;
            List<PrintInCommonItemView> printInCommonItemViewListNew = new List<PrintInCommonItemView>();
            Dictionary<string, ColmonXMLEntity> dicwordcounts = InCommonNoteBiz.GetReportxmlwords(printInCommonView);  //xml中属性 和最大字符数
            if (dicwordcounts == null || dicwordcounts.Count == 0) return;
            foreach (var inCommonItem in printInCommonItemViewListOld)
            {
                int maxrow = -1;  //用于获取一个对象中被分成的最大行数
                Dictionary<string, string> dicNeedRows = new Dictionary<string, string>(); //需要换行的列 值
                foreach (var item in dicwordcounts)
                {
                    if (item.Value == null) break;
                    string proName = item.Key;  //属性名
                    PropertyInfo property = inCommonItem.GetType().GetProperty(proName); //获取该对象的该属性
                    if (property != null)
                    {
                        string valuestr = "";
                        object valueobj = property.GetValue(inCommonItem, null);
                        if (valueobj != null)
                        {
                            valuestr = valueobj.ToString();
                        }
                        dicNeedRows.Add(proName, valuestr);
                        int zheshu = GetNeedRows(valuestr, item.Value).Count;
                        if (zheshu > maxrow)
                        {
                            maxrow = zheshu;
                        }
                    }
                }
                if (maxrow == -1) continue;
                if (maxrow == 1 || maxrow == 0)
                {
                    PrintInCommonItemView printInCommonItemView = inCommonItem;
                    printInCommonItemViewListNew.Add(printInCommonItemView);
                }
                else
                {
                    List<PrintInCommonItemView> printInCommonItemOnemore = new List<PrintInCommonItemView>();  //一个正常行最终变成的多行
                    for (int i = 0; i < maxrow; i++)
                    {
                        PrintInCommonItemView printInCommonItemView;
                        if (i == 0)
                        {
                            printInCommonItemView = inCommonItem;
                        }
                        else
                        {
                            printInCommonItemView = new PrintInCommonItemView();
                        }
                        foreach (var itemneed in dicNeedRows)  //对各列进行截取
                        {
                            string proName = itemneed.Key;
                            PropertyInfo property = printInCommonItemView.GetType().GetProperty(proName); //获取该对象的该属性
                            string rowindexvalue = GetIndexStr(i, GetNeedRows(itemneed.Value, dicwordcounts[proName]));
                            property.SetValue(printInCommonItemView, rowindexvalue, null);
                        }
                        printInCommonItemOnemore.Add(printInCommonItemView);
                    }
                    printInCommonItemOnemore[printInCommonItemOnemore.Count - 1].RecordDoctorName = printInCommonItemOnemore[0].RecordDoctorName;
                    printInCommonItemOnemore[printInCommonItemOnemore.Count - 1].RecordDoctorId = printInCommonItemOnemore[0].RecordDoctorId;
                    printInCommonItemOnemore[printInCommonItemOnemore.Count - 1].RecordDoctorImg = printInCommonItemOnemore[0].RecordDoctorImg;
                    printInCommonItemOnemore[0].RecordDoctorName = "";
                    printInCommonItemOnemore[0].RecordDoctorId = "";
                    printInCommonItemOnemore[0].RecordDoctorImg = "";
                    printInCommonItemViewListNew.AddRange(printInCommonItemOnemore);
                }
            }
            printInCommonView.PrintInCommonTabViewList1.PrintInCommonItemViewList = printInCommonItemViewListNew;

        }

        /// <summary>
        ///根据条件获取该行的字符个数 
        /// </summary>
        /// <param name="allStr"></param>
        /// <param name="intIndex"></param>
        /// <param name="wordCounts"></param>
        /// <returns></returns>
        private string getRowStr(string allStr, int intIndex, int wordCounts)
        {
            Graphics graphics = this.CreateGraphics();
            float width = graphics.MeasureString("", new Font("宋体", 15)).Width;

            List<string> strList = new List<string>();
            do
            {
                strList.Add(allStr.Substring(0, wordCounts));
                allStr = allStr.Substring(wordCounts);
            } while (allStr.Length > wordCounts);
            strList.Add(allStr);
            if (intIndex >= strList.Count) return "";
            return strList[intIndex];
        }

        public void GetBiaoDianAndFuHao()
        {
            try
            {
                string biaodianstr = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("BiaoDianList");
                string danweistr = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("DanWeiList");
                DanweiList = new List<string>();
                BiaoDianlist = new List<char>();
                if (!string.IsNullOrEmpty(biaodianstr))
                {
                    for (int i = 0; i < biaodianstr.Length; i++)
                    {
                        BiaoDianlist.Add(biaodianstr[i]);
                    }
                }
                if (!string.IsNullOrEmpty(danweistr))
                {
                    DanweiList = danweistr.Split('&').ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 截成打印时长度相等的字符
        /// </summary>
        /// <param name="allStr"></param>
        /// <param name="colmonXMLEntity"></param>
        /// <returns></returns>
        private List<string> GetNeedRows(string allStr, ColmonXMLEntity colmonXMLEntity)
        {

            try
            {
                Graphics graphics = this.CreateGraphics();
                float maxfontsize = (float)(Convert.ToDecimal(colmonXMLEntity.Maxpix));
                List<string> strList = DrectSoft.Common.DS_Common.GetStrEquallong(graphics, allStr, maxfontsize, DanweiList, BiaoDianlist);
                return strList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// 获取对应行的字符串
        /// </summary>
        /// <param name="index"></param>
        /// <param name="strList"></param>
        /// <returns></returns>
        private string GetIndexStr(int index, List<string> strList)
        {
            if (index >= strList.Count) { return ""; }
            return strList[index];
        }


        /// <summary>
        /// 存在数据未保存
        /// </summary>
        /// <returns></returns>
        public bool HasInfoSave()
        {
            bool HasSave = false;
            foreach (XtraTabPage item in tabcontrol.TabPages)
            {
                if (item == null || item.Controls == null || item.Controls.Count == 0)
                {
                    continue;
                }
                UCIncommonNoteTab uCIncommonNoteTab = item.Controls[0] as UCIncommonNoteTab;
                if (uCIncommonNoteTab == null)
                {
                    continue;
                }
                HasSave = uCIncommonNoteTab.HasSave();
                if (HasSave)
                {
                    return HasSave;
                }
            }
            return HasSave;

        }

        /// <summary>
        /// 保存数据
        /// </summary>
        public void SaveAllDate()
        {
            try
            {
                foreach (XtraTabPage item in tabcontrol.TabPages)
                {
                    if (item == null || item.Controls == null || item.Controls.Count == 0) continue;
                    UCIncommonNoteTab uCIncommonNoteTab = item.Controls[0] as UCIncommonNoteTab;
                    UCInCommonTabSingle uCInCommonTabSingle = item.Controls[0] as UCInCommonTabSingle;
                    if (uCIncommonNoteTab != null)
                    {
                        uCIncommonNoteTab.btnSave_Click(null, null);
                    }
                    else if (uCInCommonTabSingle != null)
                    {
                        uCInCommonTabSingle.btnSave_Click(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ConverToDuoRow(PrintInCommonView printInCommonView)
        {
            try
            {
                List<PrintInCommonItemView> printInCommonItemViewListOld = printInCommonView.PrintInCommonTabViewList1.PrintInCommonItemViewList;
                if (printInCommonItemViewListOld == null) return;
                Dictionary<string, ColmonXMLEntity> dicwordcounts = InCommonNoteBiz.GetReportxmlwords(printInCommonView);  //xml中属性 和最大字符数
                if (dicwordcounts == null || dicwordcounts.Count == 0) return;
                List<PrintInCommonItemView> printInCommonItemViewListOut = new List<PrintInCommonItemView>(); //存放处理好的对象
                List<PrintInCommonItemView> printInCommonItemViewListNew = new List<PrintInCommonItemView>();

                Dictionary<string, List<PrintInCommonItemView>> dicIncommList = new Dictionary<string, List<PrintInCommonItemView>>();

                //将datetime时间相同的分组
                for (int i = 0; i < printInCommonItemViewListOld.Count; i++)
                {
                    if (!dicIncommList.ContainsKey(printInCommonItemViewListOld[i].DateTimeShow))
                    {
                        dicIncommList.Add(printInCommonItemViewListOld[i].DateTimeShow, new List<PrintInCommonItemView>());
                    }
                    dicIncommList[printInCommonItemViewListOld[i].DateTimeShow].Add(printInCommonItemViewListOld[i]);
                }


                foreach (List<PrintInCommonItemView> incommItemList in dicIncommList.Values)
                {
                    #region 处理数据
                    printInCommonItemViewListNew = incommItemList;
                    int maxrow = -1;  //用于获取一个对象中被分成的最大行数
                    Dictionary<string, string> dicNeedRows = new Dictionary<string, string>(); //需要换行的列 值
                    foreach (var item in dicwordcounts)
                    {
                        string valueStr = "";
                        if (item.Value == null) break;
                        string proName = item.Key;  //属性名

                        foreach (var inCommonItem in printInCommonItemViewListNew)
                        {
                            PropertyInfo property = inCommonItem.GetType().GetProperty(proName); //获取该对象的该属性
                            if (property != null)
                            {
                                object valueobj = property.GetValue(inCommonItem, null);
                                if (valueobj != null)
                                {
                                    valueStr += valueobj.ToString();
                                }
                                property.SetValue(inCommonItem, "", null);
                            }
                        }
                        //xll 20130228 伤处字符中的/r 回车键
                        //valueStr = valueStr.Replace("\n", "");

                        dicNeedRows.Add(proName, valueStr);
                        int zheshu = GetNeedRows(valueStr, item.Value).Count;
                        if (zheshu > maxrow)
                        {
                            maxrow = zheshu;
                        }
                    }   //获得需要换行的属性和值的集合 并获得需要的最大行数


                    //处理数据 最终的分行处理
                    for (int j = 0; j < maxrow; j++)
                    {
                        foreach (var itemneed in dicNeedRows)  //对各列进行截取
                        {
                            string proName = itemneed.Key;
                            if (j >= printInCommonItemViewListNew.Count)
                            {
                                PrintInCommonItemView printInCommonItemView = new CommonNoteUse.PrintInCommonItemView();
                                printInCommonItemView.Date = printInCommonItemViewListNew[0].Date;
                                printInCommonItemView.Time = printInCommonItemViewListNew[0].Time;
                                printInCommonItemView.DateTimeShow = printInCommonItemViewListNew[0].DateTimeShow;
                                printInCommonItemView.RecordDoctorId = printInCommonItemViewListNew[0].RecordDoctorId;
                                printInCommonItemView.RecordDoctorName = printInCommonItemViewListNew[0].RecordDoctorName;
                                printInCommonItemView.RecordDoctorImg = printInCommonItemViewListNew[0].RecordDoctorImg;
                                printInCommonItemView.RecordDoctorImgbyte = printInCommonItemViewListNew[0].RecordDoctorImgbyte;
                                printInCommonItemViewListNew.Add(printInCommonItemView);
                            }

                            PropertyInfo property = printInCommonItemViewListNew[j].GetType().GetProperty(proName); //获取该对象的该属性
                            string rowindexvalue = GetIndexStr(j, GetNeedRows(itemneed.Value, dicwordcounts[proName]));
                            property.SetValue(printInCommonItemViewListNew[j], rowindexvalue, null);
                        }
                    }
                    #endregion
                    //将处理好的对象集合存放到最新的数据集中
                    printInCommonItemViewListOut.AddRange(printInCommonItemViewListNew);
                }
                printInCommonView.PrintInCommonTabViewList1.PrintInCommonItemViewList = printInCommonItemViewListOut;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
