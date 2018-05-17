using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DrectSoft.Core.NurseDocument
{

    /// <summary>
    /// 表字段所在表中行的配置信息，非体征数据
    /// </summary>
    public class DataFieldRowConfigInfo
    {
        public string FieldName { get; set;} //关联的数据库表字段名称
        public PointF PointStartPosition { get; set;}//起始填写数据的物理位置
        public int height { get; set; }//区域高度
        public int DataShowType { get; set; }//showType :0:表示是时段显示 1：表示是天显示
        public int VerAlign { get; set; }//verAlign（垂直对其方式） :0:表示 居中 1：表示上下交替
        public int FontSize { get; set; }//字号
        public string textColor { get; set; }// 文本颜色
        public bool updown { get; set; }//如果文本需要交替显示true第一个数值居上
        private RectangleF startBound=new RectangleF();
        //根据字段获得起始区域
        public RectangleF StartBound
        {
            get
            {
                startBound.Location = PointStartPosition;
                startBound.Width = 0;
                startBound.Height = height;
                return startBound;
            }
        }

    }

    /// <summary>
    /// 特殊行数据的显示设置
    /// </summary>
    public class SpecialRowCellDisplay
    {
        public string FieldName { get; set;} //关联的数据库表字段名称
        public string ValueKey { get; set; } //需要特殊显示的值内容
        public string TextColor { get; set; } //需要特殊显示的值颜色
        public float TextSize { get; set; } //需要特殊显示的字号 处理内容过多时 文本将缩放
    }

    /// <summary>
    /// 带有占位符的文本
    /// </summary>
    public class VocateText
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; } 
        public string DataFiled { get; set; } 
        public string Caption { get; set; } 
    }
}
