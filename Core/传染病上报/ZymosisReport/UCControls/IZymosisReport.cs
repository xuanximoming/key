using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.ZymosisReport.UCControls
{
    /// <summary>
    /// 副卡各控件接口
    /// </summary>
    interface IZymosisReport
    {
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="strValues"></param>
        void InitValue(string strValues);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        string GetValue();

        /// <summary>
        /// 设置焦点
        /// </summary>
        void SetFocus();

    }
    }
