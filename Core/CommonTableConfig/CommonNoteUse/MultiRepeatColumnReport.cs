using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Data;
using System.Reflection;

namespace DrectSoft.Core.CommonTableConfig.CommonNoteUse
{
    public class MultiRepeatColumnReport : IPrintNurse
    {
        public int PageIndex = 0;
        public List<object> entitiesList = new List<object>();
        public MultiRepeatColumnReport()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 计算ICU护理记录单的总量 并消除相同时间
        /// </summary>
        /// <param name="printInCommonView"></param>
        /// <returns></returns>
        public List<PrintInCommonItemView> JiSuanZongLiang(PrintInCommonView printInCommonView)
        {

            if (commonNoteCountEntity == null || commonNoteCountEntity.Valide != "1" || string.IsNullOrEmpty(commonNoteCountEntity.ItemCount))
            {
                return printInCommonView.PrintInCommonTabViewList1.PrintInCommonItemViewList;
            }
            List<string> JiSuanColList = new List<string>();
            string[] strList = commonNoteCountEntity.ItemCount.Split(',', '，');
            foreach (var item in strList)
            {
                //避免统计列格式问题
                //XLB 2013-07-10
                int result = 0;
                if (!int.TryParse(item, out result))
                {
                    strList = strList.Where(p => p != item).ToArray();
                }
                else
                {
                    var colIndex = from jiSuanIndex in JiSuanColList where jiSuanIndex.Equals("Value" + item) select jiSuanIndex;
                    if (colIndex == null || colIndex.Count() <= 0)
                    {
                        JiSuanColList.Add("Value" + result);
                    }

                }
            }
            DateTime datetime1 = Convert.ToDateTime(commonNoteCountEntity.Hour12Time);
            DateTime datetime2 = Convert.ToDateTime(commonNoteCountEntity.Hour24Time);

            int time1 = datetime1.Hour * 60 + datetime1.Minute;  //12小时统计
            int time2 = datetime2.Hour * 60 + datetime2.Minute;//24小时统计


            List<PrintInCommonItemView> printInCommonItemViewList = printInCommonView.PrintInCommonTabViewList1.PrintInCommonItemViewList;
            List<PrintInCommonItemView> printInCommonItemViewListNew = new List<PrintInCommonItemView>();
            if (printInCommonItemViewList == null || printInCommonItemViewList.Count == 0) return printInCommonItemViewListNew;




            Dictionary<string, decimal> ZL12DicList = new Dictionary<string, decimal>();
            Dictionary<string, decimal> ZL24DicList = new Dictionary<string, decimal>();
            foreach (var item in JiSuanColList)
            {
                ZL12DicList.Add(item, 0);
                ZL24DicList.Add(item, 0);
            }


            //int LastTongJi = 0;  //标记上一条统计是何种统计 1早七点 2是晚七点 0为刚起步
            for (int i = 0; i < printInCommonItemViewList.Count; i++)
            {
                #region
                var item = printInCommonItemViewList[i];


                foreach (var JiSuanCol in JiSuanColList)
                {
                    PropertyInfo property = item.GetType().GetProperty(JiSuanCol);
                    if (property != null)
                    {
                        decimal ValueDec = 0;
                        string ValueStr = property.GetValue(item, null).ToString();
                        decimal.TryParse(ValueStr, out ValueDec);
                        ZL12DicList[JiSuanCol] += ValueDec;
                        ZL24DicList[JiSuanCol] += ValueDec;
                    }
                }

                string datetimeStr = item.DateTimeShow;


                printInCommonItemViewListNew.Add(item);

                if (!string.IsNullOrEmpty(datetimeStr) && datetimeStr.Length == 16 && i < printInCommonItemViewList.Count - 1)
                {
                    datetimeStr = datetimeStr + ":00";
                    DateTime dtime = Convert.ToDateTime(datetimeStr);
                    string datetimeNextStr = printInCommonItemViewList[i + 1].DateTimeShow + ":00";
                    DateTime dtNext = Convert.ToDateTime(datetimeNextStr);

                    int dTimeMin = dtime.Hour * 60 + dtime.Minute;
                    int dTimeMinNext = dtNext.Hour * 60 + dtNext.Minute;

                    if (i < printInCommonItemViewList.Count - 1
                        && printInCommonItemViewList[i + 1].DateTimeShow.Length == 16)
                    {

                        if ((dTimeMin <= time1 && dTimeMinNext > time1 && dtNext.Date == dtime.Date)  //同一天  12小时统计
                            || (dTimeMin <= time1 && dtNext.Date > dtime.Date)
                             || (dTimeMin > time2 && dTimeMinNext > time1 && dtNext.Date > dtime.Date)
                            || (dTimeMin > time2 && dtNext.Date.AddDays(-1) > dtime.Date))
                        {
                            PrintInCommonItemView printInCommonItemView = new CommonNoteUse.PrintInCommonItemView();
                            printInCommonItemView.IsZongLiang = 1;

                            if (commonNoteCountEntity.Hour12Name.Length >= 6)
                            {
                                printInCommonItemView.Date = commonNoteCountEntity.Hour12Name.Substring(0, 6);
                                printInCommonItemView.Time = commonNoteCountEntity.Hour12Name.Substring(6);
                            }
                            else
                            {
                                printInCommonItemView.Date = commonNoteCountEntity.Hour12Name;
                            }
                            printInCommonItemView.DateTimeShow = commonNoteCountEntity.Hour12Name;

                            bool Is12HasCount = false;
                            foreach (var jisuancol in JiSuanColList)
                            {
                                PropertyInfo property = printInCommonItemView.GetType().GetProperty(jisuancol);
                                property.SetValue(printInCommonItemView, ZL12DicList[jisuancol].ToString(), null);
                                if (ZL12DicList[jisuancol] > 0)
                                {
                                    Is12HasCount = true;
                                }
                                ZL12DicList[jisuancol] = 0;
                                //视情况清空24小时统计
                                if (dtNext.Date.AddDays(-1) > dtime.Date)
                                {
                                    ZL24DicList[jisuancol] = 0;
                                }
                            }
                            if (Is12HasCount)  //如果需要计算的列 有一个不等于0 就显示 否则不显示统计列
                            {
                                printInCommonItemViewListNew.Add(printInCommonItemView);
                            }


                        }
                        else if ((dTimeMin <= time2 && dTimeMinNext > time2 && dtNext.Date == dtime.Date)
                          || (dTimeMin <= time2 && dtNext.Date > dtime.Date))
                        {
                            PrintInCommonItemView printInCommonItemView = new CommonNoteUse.PrintInCommonItemView();
                            printInCommonItemView.IsZongLiang = 2;

                            if (commonNoteCountEntity.Hour24Name.Length >= 4)
                            {
                                printInCommonItemView.Date = commonNoteCountEntity.Hour24Name.Substring(0, 6);
                                printInCommonItemView.Time = commonNoteCountEntity.Hour24Name.Substring(6);
                            }
                            else
                            {
                                printInCommonItemView.Date = commonNoteCountEntity.Hour24Name;
                            }
                            printInCommonItemView.DateTimeShow = commonNoteCountEntity.Hour24Name;

                            bool IsJisuan24 = false;
                            foreach (var jisuancol in JiSuanColList)
                            {
                                PropertyInfo property = printInCommonItemView.GetType().GetProperty(jisuancol);
                                property.SetValue(printInCommonItemView, ZL24DicList[jisuancol].ToString(), null);
                                if (ZL24DicList[jisuancol] > 0)
                                {
                                    IsJisuan24 = true;
                                }
                                ZL12DicList[jisuancol] = 0;
                                ZL24DicList[jisuancol] = 0;
                            }
                            if (IsJisuan24)
                            {
                                printInCommonItemViewListNew.Add(printInCommonItemView);
                            }

                        }

                    }


                }
                #endregion
                //InCommonNoteBiz.ConvertForImgRec(printInCommonItemViewList[i]);

                //添加最后一条记录
                if (i == printInCommonItemViewList.Count - 1)
                {
                    datetimeStr = printInCommonItemViewList[printInCommonItemViewList.Count - 1].DateTimeShow + ":00";
                    DateTime dtime = Convert.ToDateTime(datetimeStr);
                    int dTimeMin = dtime.Hour * 60 + dtime.Minute;
                    PrintInCommonItemView printInCommonItemView = new CommonNoteUse.PrintInCommonItemView();
                    if (dTimeMin <= time1 || dTimeMin > time2)
                    {
                        printInCommonItemView.IsZongLiang = 1;

                        if (commonNoteCountEntity.Hour12Name.Length >= 6)
                        {
                            printInCommonItemView.Date = commonNoteCountEntity.Hour12Name.Substring(0, 6);
                            printInCommonItemView.Time = commonNoteCountEntity.Hour12Name.Substring(6);
                        }
                        else
                        {
                            printInCommonItemView.Date = commonNoteCountEntity.Hour12Name;
                        }
                        printInCommonItemView.DateTimeShow = commonNoteCountEntity.Hour12Name;
                        bool IsJisuan12 = false;
                        foreach (var jisuancol in JiSuanColList)
                        {
                            PropertyInfo property = printInCommonItemView.GetType().GetProperty(jisuancol);
                            property.SetValue(printInCommonItemView, ZL12DicList[jisuancol].ToString(), null);
                            if (ZL12DicList[jisuancol] > 0)
                            {
                                IsJisuan12 = true;
                            }
                            ZL12DicList[jisuancol] = 0;
                        }
                        if (IsJisuan12)
                        {
                            printInCommonItemViewListNew.Add(printInCommonItemView);
                        }

                    }
                    else
                    {
                        printInCommonItemView.IsZongLiang = 2;

                        if (commonNoteCountEntity.Hour24Name.Length >= 6)
                        {
                            printInCommonItemView.Date = commonNoteCountEntity.Hour24Name.Substring(0, 6);
                            printInCommonItemView.Time = commonNoteCountEntity.Hour24Name.Substring(6);
                        }
                        else
                        {
                            printInCommonItemView.Date = commonNoteCountEntity.Hour24Name;
                        }
                        printInCommonItemView.DateTimeShow = commonNoteCountEntity.Hour24Name;

                        bool Isjisuan24 = false;
                        foreach (var jisuancol in JiSuanColList)
                        {
                            PropertyInfo property = printInCommonItemView.GetType().GetProperty(jisuancol);
                            property.SetValue(printInCommonItemView, ZL24DicList[jisuancol].ToString(), null);
                            if (ZL24DicList[jisuancol] > 0)
                            {
                                Isjisuan24 = true;
                            }
                            ZL24DicList[jisuancol] = 0;
                        }
                        if (Isjisuan24)
                        {
                            printInCommonItemViewListNew.Add(printInCommonItemView);
                        }
                    }

                }

            }
            //InCommonNoteBiz.ConvertForDateTime(printInCommonItemViewListNew);
            return printInCommonItemViewListNew;
        }




        /// <summary>
        /// ICU单据的横向计算
        /// </summary>
        /// <param name="inCommonNoteItemEntityList"></param>
        /// <param name="dr"></param>
        public void SetDataRowZongLiang(List<InCommonNoteItemEntity> inCommonNoteItemEntityList, DataRow datarow, DataTable dt)
        {

        }

        public void GetPreview(RecordPrintView recordPrintView, Graphics g)
        {
            try
            {
                MultiRepeatColumnDrawer drawer = new MultiRepeatColumnDrawer(g);
                drawer.Draw(recordPrintView);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void GetPrintimage(RecordPrintView recordPrintView, Graphics g)
        {
            throw new NotImplementedException();
        }

        public bool IsCustomPrint
        {
            get
            {
                return true;
            }
        }

        public int PageRecordCount
        {
            get;
            set;
        }





        public CommonNoteCountEntity commonNoteCountEntity
        {
            get;
            set;
        }
    }
}
