<%@ Page Language="C#"  validateRequest="false" AutoEventWireup="true" CodeBehind="NewsAdd.aspx.cs" Inherits="DrectSoft.Emr.Web.Applications.Manage.NewsAdd" %>
<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <link href="../Css/skin.css" rel="stylesheet" type="text/css" />
    <title>新闻添加</title>
    
    <style type="text/css">
 .style1
{
}
.style2
{
    height: 40px;
    width: 5%;
}
 .style5
{
    width: 0px;
}

       
    </style>
    

   
</head>
<body>

    <form id="form1" runat="server">
     <script language="javascript" type="text/javascript">
         function ValidSave() {
             var strtextClass = document.getElementById("ArticleClass_I").value;
             var strtextTitle = document.getElementById("Title_I").value;
             var strtextcontent = document.getElementById("Content").value;
             if (strtextClass.replace(/\s/ig, '') == "") {
                 document.getElementById("Label_Message").innerHTML = "类别名称不能为空";
                 return false;
             }
             if (strtextTitle.replace(/\s/ig, '') == "") {
                 document.getElementById("Label_Message").innerHTML = "标题不能为空";
                 return false;
             }
//             if (strtextcontent.replace(/\s/ig, '') == "") {
//                 document.getElementById("Label_Message").innerHTML = "新闻内容不能为空";
//                 return false;
//             }
         }
         function CancelConfirm() {

             if (confirm("确认取消吗?")) {
                 window.location.href = "NewsList.aspx";
                 
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
        <td height="31"><div class="titlebt">新闻管理</div></td>
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
      <div class="add_title" >增加新闻</div>
         <div>  
      <table width="100%" border="0" cellpadding="3" cellspacing="1"  align="center">
    <tr>
      <td style="padding-left:15px;"><a href="NewsClass_List.aspx" >分类管理</a>&nbsp;┋&nbsp;<a href="NewsClass_Add.aspx" >新增分类</a>&nbsp;┋&nbsp;<a href="NewsList.aspx" >新闻管理</a>&nbsp;┋&nbsp;<a href="NewsDefault.aspx" >新闻首页</a>&nbsp;</td>
    </tr>
  </table></div>
      <div>
      <table cellpadding="0" cellspacing="0" width="100%" class="add_Newstable"  >
      <tr><td class="style5">&nbsp;</td><td></td><td colspan="2"></td></tr>
      <tr><td class="style5"></td><td class="style1">新闻类别</td><td colspan="2">
          <dxe:ASPxComboBox ID="ArticleClass" runat="server" DropDownStyle="DropDown" EnableIncrementalFiltering="true">
          </dxe:ASPxComboBox>
         </td></tr>
          <tr><td class="style5"></td><td colspan="3">&nbsp;</td></tr>
          <tr>
             <td class="style5"></td> <td class="style1">发布对象</td><td>
          <dxe:ASPxComboBox ID="Dept" runat="server" DropDownStyle="DropDown" EnableIncrementalFiltering="true">
          </dxe:ASPxComboBox><span style="float:left; color:Red;">( 不选默认发布对象为【全院所有科室】)</span>
             </td>
              <td>
                  
             </td>
          </tr>
          <tr>
              <td class="style5"></td><td class="style1">&nbsp;</td><td colspan="2">
              &nbsp;</td>
          </tr>
      <tr><td class="style5"></td><td class="style1">新闻标题</td><td colspan="2"> <dxe:ASPxTextBox ID="Title" runat="server" Width="330px"></dxe:ASPxTextBox>
                                                </td></tr>
      <tr><td class="style5"></td><td></td><td class="style1" colspan="2">&nbsp;</td><td>&nbsp;</td></tr>
      <tr><td class="style5"></td><td class="style1">发布人</td><td colspan="2">
          <dxe:ASPxTextBox ID="Author" runat="server"  Width="170px" ReadOnly="true">
          </dxe:ASPxTextBox>
      </td></tr>
      <tr><td class="style5"></td><td class="style1">&nbsp;</td><td colspan="2">&nbsp;</td></tr>
        <tr><td class="style5"></td><td class="style1">新闻内容</td><td colspan="2">    
          <FCKeditorV2:FCKeditor ID="Content" runat="server" Height="400px">
          </FCKeditorV2:FCKeditor>   
            </td></tr>
            <tr>
            <td class="style5"></td><td class="style1"></td>
            <td colspan="2">
                &nbsp;</td>
            </tr>
      <tr><td class="style5"></td><td class="style2"></td><td colspan="2">
              <asp:Button CssClass="op_normal" ID="btnSave" OnClientClick="return ValidSave()" runat="server" Text="提交" onclick="modifybt_Click" />
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

