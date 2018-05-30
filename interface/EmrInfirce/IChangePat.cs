
using System.Windows.Forms;
namespace EmrInfirce
{
    public interface IChangePat
    {
        int InitEmr(string UserId);
        UserControl ChangePatient(string PatNoOfHis);
    }
}
