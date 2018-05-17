<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewsClass_List.aspx.cs" Inherits="DrectSoft.Emr.Web.Applications.Manage.NewsClass_List" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
<script language="JavaScript" type="text/javascript" src="../Javascript/public.js"></script>
<link href="../Css/skin.css" rel="stylesheet" type="text/css" />
    <title>新闻分类列表</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <script language="javascript" type="text/javascript">
        function ConfirmDel() {
            var chk = document.getElementsByName("define_checkbox");
            for(var i=0;i<chk.length;i++)
            {
              if(chk[i].checked)
              {
                  if (confirm('确认删除所选信息吗?')) {
                      return true;
                  }
                  else {
                      return false;
                  } 
              }
            }
             alert("没有选中任何项!");
             return false; 
        }
    </script>
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
      <div class="add_title" >新闻类别管理</div>
      <div>  
      <table width="100%" border="0" cellpadding="3" cellspacing="1"  align="center">
    <tr>
      <td style="padding-left:15px;"><a href="NewsClass_List.aspx" >分类管理</a>&nbsp;┋&nbsp;<a href="NewsClass_Add.aspx" >新增分类</a>&nbsp;┋&nbsp;<a href="NewsList.aspx" >新闻管理</a>&nbsp;┋&nbsp;<a href="NewsAdd.aspx" >新增新闻</a>&nbsp;┋&nbsp;<asp:LinkButton ID="DelP" runat="server" CssClass="topnavichar" OnClientClick="return ConfirmDel();" OnClick=" DelSelect_Click" >删除所选</asp:LinkButton>&nbsp;┋&nbsp;<a href="NewsDefault.aspx" >新闻首页</a>&nbsp;
      </td>
    </tr>
  </table></div>
      <div>
     <asp:Repeater ID="Repeater_NewsClass_List" runat="server">
                  <HeaderTemplate>
          <table width="100%" border="0" align="center" cellpadding="2" cellspacing="0" class="sys_list_table" bgcolor="#FFFFFF" >
        <tr class="TR_BG">
          <td width="30%" valign="middle" class="sys_topBg">类别名称</td>
          <td align="center" valign="middle" class="sys_topBg">类别说明</td>
          <td align="center" valign="middle" class="sys_topBg">属性
            <input type="checkbox" id="define_checkbox2" value="-1" name="define_checkbox2" onclick="javascript:return selectAll(this.form,this.checked);"/></td>
        </tr>
          </HeaderTemplate>
          <ItemTemplate>
         <%#((System.Data.DataRowView)Container.DataItem)["Colum"]%>
          </ItemTemplate>
            <FooterTemplate>
        </table>
      </FooterTemplate>
          </asp:Repeater>
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


