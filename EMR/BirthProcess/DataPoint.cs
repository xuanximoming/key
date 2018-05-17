using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.BirthProcess
{
    public class DataPoint
    {
        public string value { get; set; }//实际值
        public string date { get; set; }
        public int temperatureType { get; set; }//0:表示不是体温值，8801表示口温 ; 8802：表示腋温 ; 8803：表示肛温
        public string timeslot { get; set; }

        public DataPoint() { }

        /// <summary>
        /// 根据实际值和坐标获得该点的物理纵坐标
        /// </summary>
        /// <returns></returns>
        public float ToYCoordinate(VerticalCoordinate verticalCoordinate)
        {
            float val;
            if (float.TryParse(value, out val))
                return verticalCoordinate.endY - (val - verticalCoordinate.minValue) * (verticalCoordinate.endY - verticalCoordinate.startY) / (verticalCoordinate.maxValue - verticalCoordinate.minValue);
            else
                return val;
        }

        /// <summary>
        /// 根据时间值获得该点的物理横坐标
        /// </summary>
        /// <returns></returns>
        public float ToXCoordinate()
        {
            DateTime curDatetime = Convert.ToDateTime(date + " " + timeslot + ":00:00");
            double hours = 0;
            return (float)((DrawHepler.smallGridBound.Width) * hours / (24 * DrawHepler.m_Days)) + DrawHepler.smallGridBound.X;
        }
    }
}
