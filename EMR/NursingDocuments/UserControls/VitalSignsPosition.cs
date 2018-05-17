using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections;

namespace DrectSoft.Core.NursingDocuments.UserControls
{

    /// <summary>
    /// 点的位置类
    /// </summary>
    public class VitalSignsPosition
    {
        #region Property && Field

        private PointF m_PointF;
        private string m_Type = string.Empty; //1:体温 2:脉搏 3:呼吸 4:心率 5:物理降温
        private string m_SubType = string.Empty; //温度的类型 1: 口温  2: 腋温 3: 肛温
        private string m_IsSpecial = string.Empty; //是否特殊 对于呼吸类别有呼吸机R 对于心率类别有起搏器H

        //唯一标示这个点，用于同过此ID找到与该点有关的线段中的起始点或终止点，即通过m_ArrayListPoint中的VitalSigns关联到m_ArrayListPointLine中的VitalSignsLine
        //在调整m_ArrayListPoint中的VitalSigns中的点的坐标后，找到m_ArrayListPointLine中的VitalSignsLine中的点的坐标
        private int m_ID;//唯一表示这个点
        private DateTime m_Date = new DateTime();//日期
        //private int m_indx = 0;//时间点
        private string m_TimePoint = string.Empty;//时间点
        private string m_Value = string.Empty;//值
        private bool m_IsDraw = true;//表示是否要绘到界面上，用于重合点

        public PointF PointF
        {
            get
            {
                return m_PointF;
            }
            set
            {
                m_PointF = value;
            }
        }

        public string Type
        {
            get
            {
                return m_Type;
            }
            set
            {
                m_Type = value;
            }
        }

        public string SubType
        {
            get
            {
                return m_SubType;
            }
            set
            {
                m_SubType = value;
            }
        }

        public int ID
        {
            get
            {
                return m_ID;
            }
            set
            {
                m_ID = value;
            }
        }

        public string IsSpecial
        {
            get
            {
                return m_IsSpecial;
            }
            set
            {
                m_IsSpecial = value;
            }
        }

        public DateTime Date
        {
            get
            {
                return m_Date;
            }
            set
            {
                m_Date = value;
            }
        }

        //public int Indx
        //{
        //    get
        //    {
        //        return m_indx;
        //    }
        //    set
        //    {
        //        m_indx = value;
        //    }
        //}

        public string TimePoint
        {
            get
            {
                return m_TimePoint;
            }
            set
            {
                m_TimePoint = value;
            }
        }

        public string Value
        {
            get
            {
                return m_Value;
            }
            set
            {
                m_Value = value;
            }
        }

        public bool IsDraw
        {
            get
            {
                return m_IsDraw;
            }
            set
            {
                m_IsDraw = value;
            }
        }
        #endregion

        #region Enum
        public enum ConflictPointType
        {
            MaiBoTiWenKou = 1,  //脉搏与体温(口腔)重叠
            MaiBoTiWenGang = 2, //脉搏与体温(肛门)重叠
            MaiBoTiWenYe = 3,   //脉搏与体温(腋下)重叠
            HuXiMaiBo = 4,      //呼吸与脉搏相遇
            TiWenHuXiMaiBo = 5 //体温、呼吸、脉搏均在一个点上
        }
        #endregion

        public string GetConflictPointType(string currentType, string currentSubType, string nextType, string nextSubType)
        {
            string returnValue = string.Empty;

            //脉搏与体温
            if (currentType == UCThreeMeasureTable.VitalSignsType.TiWen.ToString() && nextType == UCThreeMeasureTable.VitalSignsType.MaiBo.ToString()
                || nextType == UCThreeMeasureTable.VitalSignsType.TiWen.ToString() && currentType == UCThreeMeasureTable.VitalSignsType.MaiBo.ToString())
            {
                if (currentSubType == Convert.ToInt32(UCThreeMeasureTable.VitalSignsTiWenType.KouWen).ToString()
                    || nextSubType == Convert.ToInt32(UCThreeMeasureTable.VitalSignsTiWenType.KouWen).ToString())
                {
                    returnValue = ConflictPointType.MaiBoTiWenKou.ToString();
                }
                else if (currentSubType == Convert.ToInt32(UCThreeMeasureTable.VitalSignsTiWenType.GangWen).ToString()
                    || nextSubType == Convert.ToInt32(UCThreeMeasureTable.VitalSignsTiWenType.GangWen).ToString())
                {
                    returnValue = ConflictPointType.MaiBoTiWenGang.ToString();
                }
                else if (currentSubType == Convert.ToInt32(UCThreeMeasureTable.VitalSignsTiWenType.YeWen).ToString()
                    || nextSubType == Convert.ToInt32(UCThreeMeasureTable.VitalSignsTiWenType.YeWen).ToString())
                {
                    returnValue = ConflictPointType.MaiBoTiWenYe.ToString();
                }
            }
            else if (currentType == UCThreeMeasureTable.VitalSignsType.HuXi.ToString() && nextType == UCThreeMeasureTable.VitalSignsType.MaiBo.ToString()
                || nextType == UCThreeMeasureTable.VitalSignsType.HuXi.ToString() && currentType == UCThreeMeasureTable.VitalSignsType.MaiBo.ToString())
            {
                returnValue = ConflictPointType.HuXiMaiBo.ToString();
            }

            return returnValue;
        }

        /// <summary>
        /// 处理曲线图中有重合的点，并返回所有的重合点
        /// </summary>
        /// <param name="arrayListPoint"></param>
        /// <param name="arrayListPointLine"></param>
        /// <param name="cellHeight"></param>
        /// <param name="conflictDistance">两点相距指定的像素点认为是有重合的点</param>
        /// <returns></returns>
        public ArrayList GetmConflictPoint(ArrayList arrayListPoint, ArrayList arrayListPointLine, int cellHeight, int conflictDistance)
        {
            ArrayList arrayListConflictPoint = new ArrayList();
            for (int i = 0; i < arrayListPoint.Count; i++)
            {
                VitalSignsPosition vitalSignsPosition = arrayListPoint[i] as VitalSignsPosition;

                if (vitalSignsPosition != null)
                {
                    PointF currentPoint = vitalSignsPosition.PointF;
                    int currentID = vitalSignsPosition.ID;
                    string currentType = vitalSignsPosition.Type;
                    string currentSubType = vitalSignsPosition.SubType;
                    string currentDateTime = vitalSignsPosition.Date.ToString("yyyy-MM-dd");
                    string currentTimePoint = vitalSignsPosition.TimePoint;
                    //int currentIndx = vitalSignsPosition.Indx;

                    //排除心率和物理降温   加上排除物理升温 add by ywk 
                    if (currentType == UCThreeMeasureTable.VitalSignsType.XinLv.ToString()
                        || currentType == UCThreeMeasureTable.VitalSignsType.WuLiJiangWen.ToString()
                        ||currentType==UCThreeMeasureTable.VitalSignsType.WuLiShengWen.ToString()
                        //edit by cyq 2013-01-25 修复重合点图片错误问题
                        //||currentType==UCThreeMeasureTable.VitalSignsType.TiWen.ToString() //zyx eidt 2013-1-15
                        )
                    {
                        continue;
                    }

                    int count = 0; //计算与几个点相同，如果与2个点相同，则表示：体温、呼吸、脉搏均在一个点上
                    float yPoint = 0f;
                    for (int j = i + 1; j < arrayListPoint.Count; j++)
                    {
                        VitalSignsPosition vitalSignsPositionNext = arrayListPoint[j] as VitalSignsPosition;

                        if (vitalSignsPositionNext != null)
                        {
                            PointF nextPoint = vitalSignsPositionNext.PointF;
                            int nextID = vitalSignsPositionNext.ID;
                            string nextType = vitalSignsPositionNext.Type;
                            string nextSubType = vitalSignsPositionNext.SubType;
                            string nextDateTime = vitalSignsPositionNext.Date.ToString("yyyy-MM-dd");
                            string nextTimePoint = vitalSignsPositionNext.TimePoint;
                            //int nextIndx = vitalSignsPositionNext.Indx;

                            //找到同一天同一个时段的点
                            if (currentDateTime == nextDateTime && currentTimePoint == nextTimePoint)
                            {
                                if (Math.Abs(currentPoint.Y - nextPoint.Y) <= conflictDistance) //上下差距10个像素之间的认为是重合的
                                {
                                    count++;

                                    float abs = Math.Abs(currentPoint.Y - nextPoint.Y);
                                    if (yPoint == 0f)
                                    {
                                        yPoint = Math.Min(currentPoint.Y, nextPoint.Y) + abs / 2;
                                    }
                                    vitalSignsPosition.PointF = new PointF(vitalSignsPosition.PointF.X, yPoint);
                                    vitalSignsPositionNext.PointF = new PointF(vitalSignsPosition.PointF.X, yPoint);

                                    for (int m = 0; m < arrayListPointLine.Count; m++)
                                    {
                                        VitalSignsLine vitalSignsLine = arrayListPointLine[m] as VitalSignsLine;

                                        if (vitalSignsLine.StartPointID == currentID || vitalSignsLine.StartPointID == nextID)
                                        {
                                            vitalSignsLine.StartPointF = new PointF(vitalSignsLine.StartPointF.X, yPoint + cellHeight / 2);
                                        }
                                        if (vitalSignsLine.EndPointID == currentID || vitalSignsLine.EndPointID == nextID)
                                        {
                                            vitalSignsLine.EndPointF = new PointF(vitalSignsLine.EndPointF.X, yPoint + cellHeight / 2);
                                        }
                                    }

                                    VitalSignsPosition vsp = new VitalSignsPosition();
                                    vsp.PointF = new PointF(vitalSignsPosition.PointF.X, yPoint);

                                    if (count == 1)
                                    {
                                        vsp.Type = GetConflictPointType(currentType, currentSubType, nextType, nextSubType);
                                    }
                                    else if (count > 1)
                                    {
                                        vsp.Type = ConflictPointType.TiWenHuXiMaiBo.ToString();
                                    }

                                    arrayListConflictPoint.Add(vsp);
                                }
                            }


                        }
                    }
                }
            }

            for (int i = 0; i < arrayListConflictPoint.Count; i++)
            {
                VitalSignsPosition vsp1 = arrayListConflictPoint[i] as VitalSignsPosition;
                if (vsp1 != null)
                {
                    if (vsp1.Type == ConflictPointType.TiWenHuXiMaiBo.ToString())//当为三点重合的时候才进入判断
                    {
                        PointF p1 = vsp1.PointF;
                        for (int m = 0; m < arrayListConflictPoint.Count; m++)
                        {
                            if (i == m)
                            {
                                continue;
                            }

                            VitalSignsPosition vsp2 = arrayListConflictPoint[m] as VitalSignsPosition;

                            if (vsp2 != null)
                            {
                                PointF p2 = vsp2.PointF;

                                if (p1.X == p2.X && p1.Y == p2.Y)
                                {
                                    vsp2.m_IsDraw = false;
                                }
                            }
                        }
                    }
                }
            }

            return arrayListConflictPoint;
        }
    }
}
