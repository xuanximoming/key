using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.EMR_NursingDocument.PublicMethod
{
    public class PrintRecordModel
    {
        private List<NurseRecordEntity> nurseRecordEntityList;
        private PrintHeaderModel printHeaderModel;

        public PrintHeaderModel PrintHeaderModel
        {
            get { return printHeaderModel; }
            set { printHeaderModel = value; }
        }
        public List<NurseRecordEntity> NurseRecordEntityList
        {
            get { return nurseRecordEntityList; }
            set { nurseRecordEntityList = value; }
        }

        private List<NurseRecordEntity> nurseRecordEntityListInOut1;
        private List<NurseRecordEntity> nurseRecordEntityListInOut2;
        private List<NurseRecordEntity> nurseRecordEntityListInOut3;

        public List<NurseRecordEntity> NurseRecordEntityListInOut3
        {
            get { return nurseRecordEntityListInOut3; }
            set { nurseRecordEntityListInOut3 = value; }
        }

        public List<NurseRecordEntity> NurseRecordEntityListInOut2
        {
            get { return nurseRecordEntityListInOut2; }
            set { nurseRecordEntityListInOut2 = value; }
        }

        public List<NurseRecordEntity> NurseRecordEntityListInOut1
        {
            get { return nurseRecordEntityListInOut1; }
            set { nurseRecordEntityListInOut1 = value; }
        }
    }
}
