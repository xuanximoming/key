using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.CommonTableConfig
{


    /// <summary>
    /// 简要对象
    /// </summary>
    public class BaseDictory
    {
        private string _Id;

        public string Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
