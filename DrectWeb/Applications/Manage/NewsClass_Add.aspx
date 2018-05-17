<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewsClass_Add.aspx.cs" Inherits="DrectSoft.Emr.Web.Applications.Manage.NewsClass_Add" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>新闻类别添加</title>
    <link href="../Css/skin.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <script language="javascript" type="text/javascript">
        function ValidSave() {
            var strtextName=document.getElementById("TextBox_ClassName").value;
            var strtextSummary = document.getElementById("TextBox_Summary").value;
            if (strtextName.replace(/\s/ig, '') == "") {
                document.getElementById("Label_Message").innerHTML= "类别名称不能为空";
                return false;
            }
            if (strtextSummary.replace(/\s/ig, '')== "") {
                document.getElementById("Label_Message").innerHTML = "简要填写类别说明";
                return false;
            }
        }
        function CancelConfirm() {

            if (confirm("确认取消吗?")) {
                window.location.href = "NewsClass_List.aspx";

            }
            else {
                return false;
            }
        }
    </script>
    <div>
    <table width="100%" border="0" cellpadding="0" cellspacing="0">
  <tr>
    <td width="17" valign="top" background="../Images/mail_leftbg.gif"><img src="../Images/left-top-right.gif" width="17" height="29" /></td>
    <td valign="top" background="../Images/content-bg.gif">
    <table width="100%" height="31" border="0" cellpadding="0" cellspacing="0" class="left_topbg" id="table2">
      <tr>
        <td height="31"><div class="titlebt">类别管理</div></td>
      </tr>
    </table></td>
    <td width="16" valign="top" background="../Images/mail_rightbg.gif"><img src="../Images/nav-right-bg.gif" width="16" height="29" /></td>
  </tr>
<!--放置主体内容开始-->
<tr>
  <td valign="middle" background="../Images/mail_leftbg.gif">&nbsp;</td>
    <td valign="top" bgcolor="#F7F8F9">
    <table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
      <tr><td>
      <!--放置主体内容开始-->
      <div class="add_title" >增加新闻类别</div>
         <div>  
      <table width="100%" border="0" cellpadding="3" cellspacing="1"  align="center">
    <tr>
      <td style="padding-left:15px;"><a href="NewsClass_List.aspx" >分类管理</a>&nbsp;┋&nbsp;<a href="NewsClass_Add.aspx" >新增分类</a>&nbsp;┋&nbsp;<a href="NewsList.aspx" >新闻管理</a>&nbsp;┋&nbsp;<a href="NewsAdd.aspx" >新增新闻</a>&nbsp;┋&nbsp;<a href="NewsDefault.aspx" >新闻首页</a>&nbsp;
      </td>
    </tr>
  </table></div>
      <div>
      <table cellpadding="0" cellspacing="0" width="100%" class="add_table" >
      <tr><td style="width:10%">类别名称</td><td><asp:TextBox ID="TextBox_ClassName" runat="server"></asp:TextBox>
                                                </td></tr>
      <tr><td style="width:10%">类别说明</td><td><asp:TextBox ID="TextBox_Summary" TextMode="MultiLine" Height="40px" runat="server"></asp:TextBox>
      </td></tr>
      <tr><td style="width:10%; height:40px;"></td><td>
          <asp:Button runat="server" OnClick="BtnSave_Click" Text="保存类别" ID="BtnSave" CssClass="op_normal" OnClientClick="return ValidSave()" />
          <input id="Cancel" class="op_normal" type="reset" value="取 消" onclick=" return CancelConfirm()" />
          <asp:Label ID="Label_Message" runat="server" Text="" ForeColor="Red"></asp:Label>
          </td></tr>
      </table>
      </div>
     
      
      <!--放置主体内容结束-->
      </td></tr>
      </table>
      </td>
       <td background="../Images/mail_rightbg.gif">&nbsp;</td>
       </tr>
<!--放置主体内容结束-->
  <tr>
    <td valign="bottom" background="../Images/mail_leftbg.gif"><img src="../Images/buttom_left2.gif" width="17" height="17" /></td>
    <td background="../Images/buttom_bgs.gif"><img src="../Images/buttom_bgs.gif" width="17" height="17"></td>
    <td valign="bottom" background="../Images/mail_rightbg.gif"><img src="../Images/buttom_right2.gif" width="16" height="17" /></td>
  </tr>
</table>
    </div>
    </form>
</body>
</html>

