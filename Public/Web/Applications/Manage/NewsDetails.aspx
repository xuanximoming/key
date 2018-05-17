<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewsDetails.aspx.cs" Inherits="DrectSoft.Emr.Web.Applications.Manage.NewsDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>新闻内容页面</title>
<script language="JavaScript" type="text/javascript" src="../Javascript/public.js"></script>
<link href="../Css/skin.css" rel="stylesheet" type="text/css" />
 <link href="../Css/style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table width="100%" border="0" cellpadding="0" cellspacing="0">
  <tr>
    <td width="17" valign="top" background="../Images/mail_leftbg.gif"><img src="../Images/left-top-right.gif" width="17" height="29" /></td>
    <td valign="top" background="../Images/content-bg.gif">
    <table width="100%" height="31" border="0" cellpadding="0" cellspacing="0" class="left_topbg" id="table2">
      <tr>
        <td height="31"><div class="titlebt">新闻内容</div></td>
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
      <div class="add_title" >新闻内容查看</div>
      <div>  
      <table width="100%" border="0" cellpadding="3" cellspacing="1"  align="center">
    <tr>
      <td>
      <div class="post">
         
          <div class="postcontent"> 
            <div class="newhead1"><h2><br /><asp:Label ID="NewsTitle" runat="server"></asp:Label></h2></div>
            <div class="newhead2" style="text-align:center;">发布者：<asp:Label runat="server" ID="NewsAuthor"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 发布时间：<asp:Label ID="NewsAddTime" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;发布对象：<asp:Label ID="NewsDepartMentName" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;新闻类别：<asp:Label ID="NewsClass" runat="server"></asp:Label>&nbsp;&nbsp;</div>
            <div id="Content" runat="server"></div>
          </div>
          <div class="postbottom" style="float:right;">
          <a href="NewsDefault.aspx" style="text-decoration:none;">返回新闻列表</a>
          </div>
        </div>
      </td>
    </tr>
  </table></div>
  <div> 
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
