using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.QCReport
{
    public interface IControlDataInit
    {
        string Value { get; }
        void InitControlBindData();
        void Reset();
    }
}
