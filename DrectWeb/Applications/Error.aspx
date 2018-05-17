<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="DrectSoft.Emr.Web.Applications.Error" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>系统执行错误消息</title>
</head>
<body>
    <form id="Form1" runat="server">
    <script language="javascript" type="text/javascript">
        function QueryError() {
            if (document.getElementById("ErrorInfo").style.display == "block") {
                document.getElementById("ErrorInfo").style.display = "none";
            }
            else {
                document.getElementById("ErrorInfo").style.display = "block";
            }
        }
    </script>
        <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td align="center" valign="middle">
                    <table width="594" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td height="82"  background="Images/error_r1_c1_Long.gif">
                                <table width="60%" border="0" align="center" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td align="center">
                                            <br>
                                            <br>
                                            <img src="Images/errortext.gif" width="123" height="23"></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td height="113" align="center" valign="middle" background="Images/error_r2_c1_long.gif">
                                <div id="Error" style="width:96%; font-size: 14px;color:#08769b;">
                                    <asp:Label ID="lblError" runat="server" Width="90%"></asp:Label>
                                    </div></td>
                        </tr>
                         <tr>
                            <td height="54" align="center" valign="top" background="Images/error_r3_c1_long.gif">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <div id="ErrorInfo" runat="server" style="display:none;"></div>
    </form>
</body>
</html>
