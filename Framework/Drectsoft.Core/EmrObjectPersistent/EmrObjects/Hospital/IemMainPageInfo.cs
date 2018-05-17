using System.Collections.Generic;

namespace DrectSoft.Core
{
    public class IemMainPageInfo
    {

        private Iem_Mainpage_Basicinfo m_IemBasicInfo = new Iem_Mainpage_Basicinfo();
        /// <summary>
        /// 病案首页基本信息
        /// </summary>
        public Iem_Mainpage_Basicinfo IemBasicInfo
        {
            get { return m_IemBasicInfo; }
            set { m_IemBasicInfo = value; }
        }

        private List<Iem_Mainpage_Diagnosis> m_IemDiagInfo = new List<Iem_Mainpage_Diagnosis>();
        /// <summary>
        /// 病案首页诊断信息
        /// </summary>
        public List<Iem_Mainpage_Diagnosis> IemDiagInfo
        {
            get { return m_IemDiagInfo; }
            set { m_IemDiagInfo = value; }
        }


        private List<Iem_MainPage_Operation> m_IemOperInfo = new List<Iem_MainPage_Operation>();
        /// <summary>
        /// 病案首页手术信息
        /// </summary>
        public List<Iem_MainPage_Operation> IemOperInfo
        {
            get { return m_IemOperInfo; }
            set { m_IemOperInfo = value; }
        }

    }
}
