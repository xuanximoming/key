<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewsDefault.aspx.cs" Inherits="DrectSoft.Emr.Web.Applications.Manage.NewsDefault" %>

<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dxtc" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dxw" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>新闻首页</title>
    <script language="JavaScript" type="text/javascript" src="../Javascript/public.js"></script>
    <link href="../Css/skin.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="17" valign="top" background="../Images/mail_leftbg.gif">
                        <img src="../Images/left-top-right.gif" width="17" height="29" /></td>
                    <td valign="top" background="../Images/content-bg.gif">
                        <table width="100%" height="31" border="0" cellpadding="0" cellspacing="0" class="left_topbg" id="table2">
                            <tr>
                                <td height="31">
                                    <div class="titlebt">新闻首页</div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td width="16" valign="top" background="../Images/mail_rightbg.gif">
                        <img src="../Images/nav-right-bg.gif" width="16" height="29" /></td>
                </tr>
                <!--放置主体内容开始-->
                <tr>
                    <td valign="middle" background="../Images/mail_leftbg.gif">&nbsp;</td>
                    <td valign="top" bgcolor="#F7F8F9">
                        <table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <div class="add_title">新闻分类查看</div>
                                    <div>
                                        <table width="100%" border="0" cellpadding="3" cellspacing="1" align="center">
                                            <tr>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div>
                                        <dxtc:ASPxPageControl ID="ASPxPageControl1" runat="server" OnInit="ASPxPageControl1_Init" Width="100%" Theme="Youthful">
                                            <ContentStyle>
                                                <Border BorderColor="#7C7C94" BorderStyle="Solid" BorderWidth="1px"></Border>
                                            </ContentStyle>
                                            <LoadingPanelStyle ImageSpacing="6px">
                                            </LoadingPanelStyle>
                                        </dxtc:ASPxPageControl>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td background="../Images/mail_rightbg.gif">&nbsp;</td>
                </tr>
                <!--放置主体内容结束-->
                <tr>
                    <td valign="bottom" background="../Images/mail_leftbg.gif">
                        <img src="../Images/buttom_left2.gif" width="17" height="17" /></td>
                    <td background="../Images/buttom_bgs.gif">
                        <img src="../Images/buttom_bgs.gif" width="17" height="17"></td>
                    <td valign="bottom" background="../Images/mail_rightbg.gif">
                        <img src="../Images/buttom_right2.gif" width="16" height="17" /></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
