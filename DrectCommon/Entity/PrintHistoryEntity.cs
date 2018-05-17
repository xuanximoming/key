using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Common
{
    public class PrintHistoryEntity
    {
        private string _PHFlow;

        public string PHFlow
        {
            get { return _PHFlow; }
            set { _PHFlow = value; }
        }
        private string _PrintRecordFlow;

        public string PrintRecordFlow
        {
            get { return _PrintRecordFlow; }
            set { _PrintRecordFlow = value; }
        }
        private int _StartPage;

        public int StartPage
        {
            get { return _StartPage; }
            set { _StartPage = value; }
        }
        private int _EndPage;

        public int EndPage
        {
            get { return _EndPage; }
            set { _EndPage = value; }
        }
        private int _PrintPages;

        public int PrintPages
        {
            get { return _PrintPages; }
            set { _PrintPages = value; }
        }
        private string _PrintDocId;

        public string PrintDocId
        {
            get { return _PrintDocId; }
            set { _PrintDocId = value; }
        }
        private string _PrintDateTime;

        public string PrintDateTime
        {
            get { return _PrintDateTime; }
            set { _PrintDateTime = value; }
        }
        private string _PrintType;

        public string PrintType
        {
            get { return _PrintType; }
            set { _PrintType = value; }
        }
    }
}
