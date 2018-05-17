using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections;

namespace DrectSoft.Core.NurseDocument
{
    public class DataPoint
    {

        public string value{get;set;}//实际值
        public string date{get;set;}
        public int temperatureType { get; set; }//0:表示不是体温值，8801表示腋温 ; 8802：表示口温 ; 8803：表示肛温
        public string timeslot{get;set;}

        public DataPoint(){}

        /// <summary>
        /// 根据实际值和坐标获得该点的物理纵坐标
        /// </summary>
        /// <returns></returns>
        public float ToYCoordinate(VerticalCoordinate verticalCoordinate)
        {
            try
            {
                double val;
                if (double.TryParse(value, out val))
                {
                    if (verticalCoordinate.name.ToUpper() == DataLoader.TEMPERATURE || verticalCoordinate.name.ToUpper() == DataLoader.PHYSICALCOOLING || verticalCoordinate.name.ToUpper() == DataLoader.PHYSICALHOTTING)
                    {
                        val = verticalCoordinate.endY - (val * 10 - verticalCoordinate.minValue * 10) * (verticalCoordinate.endY - verticalCoordinate.startY) / (verticalCoordinate.maxValue * 10 - verticalCoordinate.minValue * 10);
                    }
                    else
                    {
                        val = verticalCoordinate.endY - (val - verticalCoordinate.minValue) * (verticalCoordinate.endY - verticalCoordinate.startY) / (verticalCoordinate.maxValue - verticalCoordinate.minValue);
                    }
                    return float.Parse(Math.Round(Math.Round(val, 2, MidpointRounding.AwayFromZero), 1, MidpointRounding.AwayFromZero).ToString());
                }
                else
                    return 0f;

                //double val;
                //if (double.TryParse(value, out val))
                //{
                //    val = verticalCoordinate.endY - (val - verticalCoordinate.minValue) * (verticalCoordinate.endY - verticalCoordinate.startY) / (verticalCoordinate.maxValue - verticalCoordinate.minValue);
                //    return float.Parse(Math.Round(val).ToString());
                //}
                //else
                //    return 0f;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据时间值获得该点的物理横坐标
        /// </summary>
        /// <returns></returns>
        public float ToXCoordinate()
        {
            DateTime curDatetime = Convert.ToDateTime(date+" "+timeslot+":00:00");
            double hours = (curDatetime - DataLoader.m_firstDateOfCunrrentPage).TotalHours;
            return (float)((ConfigInfo.smallGridBound.Width) * hours / (24 * ConfigInfo.m_Days)) + ConfigInfo.smallGridBound.X;
        }

        //距离周首日间隔天数
        public int GetOffsetDays()
        {
             DateTime curDate = Convert.ToDateTime(date+" 00:00:00");
             return (int)(curDate- DataLoader.m_firstDateOfCunrrentPage).TotalDays;
        }

        public int GetOffsetDays_1()
        {
            DateTime curDate = DateTime.Parse(DateTime.Parse(date).ToShortDateString());
            return (int)(curDate - DataLoader.m_firstDateOfCunrrentPage).TotalDays;
        }
    }

    //数据点根据时间的比较器  暂时不用
    public class DataPointCompare : IComparer
    {
        public int Compare(object x, object y)
        {
            DataPoint data1 = x as DataPoint;
            DataPoint data2 = y as DataPoint;
            //if(data1.time.Hour>data2.time.Hour)
                //return -1;
            // if(data1.time.Hour<data2.time.Hour)
                //return 1;
            return 0;
        }
    }
    /// <summary>
    /// 纵坐标轴
    /// </summary>
    public struct VerticalCoordinate
    {
        public string name;//名称
        public float maxValue;//最小值
        public float minValue;//最小值
        public float startY;//起始纵坐标
        public float endY;//结束纵坐标
        public string hide;//是否显示
        


        public VerticalCoordinate(string _name, float _maxValue, float _minValue, float _startY, float _endY,string _hide)
        {
            this.name = _name;
            this.maxValue = _maxValue;
            this.minValue = _minValue;
            this.startY = _startY;
            this.endY = _endY;
            this.hide = _hide;
        }
    }

}
