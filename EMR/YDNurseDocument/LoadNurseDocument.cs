
using DrectSoft.Common.Eop;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Data;
using System.Reflection;
using System.Windows.Forms;
namespace DrectSoft.Core.NurseDocument
{
    public class LoadNurseDocument
    {
        public void MyLoadNurseDocument(IWin32Window wind, IEmrHost m_app, DataRow drInpatient, out DevExpress.XtraEditors.XtraUserControl MymainNursingMeasure)
        {
            try
            {
                Form form = null;
                //获取病人对象
                Inpatient m_NewPat = new Inpatient();
                m_app.ChoosePatient(Convert.ToDecimal((drInpatient["NOOFINPAT"])).ToString(), out m_NewPat);
                DrectSoft.Core.NurseDocument.MainNursingMeasure mainNursingMeasure = new DrectSoft.Core.NurseDocument.MainNursingMeasure(drInpatient["NOOFINPAT"].ToString());
                string version = DrectSoft.Core.NurseDocument.ConfigInfo.GetNurseMeasureVersion(decimal.Parse(drInpatient["NOOFINPAT"].ToString()));
                mainNursingMeasure.CurrentPat = drInpatient["NOOFINPAT"].ToString();
                mainNursingMeasure.eventHandlerXieRu += delegate(object sender1, EventArgs e1)
                {
                    if (form == null)
                    {
                        Assembly a = Assembly.Load("DrectSoft.Core.NurseDocument");
                        Type type = a.GetType(version);
                        form = (Form)Activator.CreateInstance(type, new object[] { m_app, drInpatient["NOOFINPAT"].ToString() });
                        form.Height = DrectSoft.Core.NurseDocument.ConfigInfo.GetNurseRecordSize(decimal.Parse(drInpatient["NOOFINPAT"].ToString()));
                        form.FormClosed += delegate(object sender2, FormClosedEventArgs e2)
                        {
                            form = null;
                            mainNursingMeasure.LoadDataImage(decimal.Parse(drInpatient["NOOFINPAT"].ToString()));
                        };
                        form.Show(wind);

                    }
                    mainNursingMeasure.LoadDataImage(decimal.Parse(drInpatient["NOOFINPAT"].ToString()));
                };
                mainNursingMeasure.Load(m_app, m_NewPat);
                mainNursingMeasure.ReadOnlyControl = true;
                if (form != null)
                {
                    switch (version.Trim())
                    {
                        case "DrectSoft.Core.NurseDocument.Controls.NursingRecordNew":
                            (form as DrectSoft.Core.NurseDocument.Controls.NursingRecordNew).RefreshDate(drInpatient["NOOFINPAT"].ToString());
                            (form as DrectSoft.Core.NurseDocument.Controls.NursingRecordNew).dateEdit_DateTimeChanged(null, null);

                            break;
                        case "DrectSoft.Core.NurseDocument.Controls.NursingRecord":
                            (form as DrectSoft.Core.NurseDocument.Controls.NursingRecord).RefreshDate(drInpatient["NOOFINPAT"].ToString());
                            (form as DrectSoft.Core.NurseDocument.Controls.NursingRecord).dateEdit_DateTimeChanged(null, null);

                            break;
                    }
                    form.FormClosed += delegate(object sender2, FormClosedEventArgs e2)
                    {
                        mainNursingMeasure.LoadDataImage(decimal.Parse(drInpatient["NOOFINPAT"].ToString()));
                    };
                }
                MymainNursingMeasure = mainNursingMeasure;
            }
            catch (Exception ex)
            {
                MymainNursingMeasure = null;
            }

        }
    }
}
