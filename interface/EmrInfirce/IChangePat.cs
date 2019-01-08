
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace EmrInfirce
{
    [Guid("68D21CE6-0D67-462C-ADBA-6BF88D8ED732")]
    public interface IChangePat
    {
        [DispId(1)]
        int InitEmr(string UserId, string Name, string DeptId, string WardId, string Cate);
        [DispId(2)]
        void ChangePatient(string WinHandle, string PatNoOfHis);
        [DispId(3)]
        void ChangePatientOut(string WinHandle, string PatNoOfHis);
        [DispId(4)]
        UserControl ChangePatient(string PatNoOfHis);
        [DispId(5)]
        UserControl ChangePatientout(string PatNoOfHis);
    }
}
