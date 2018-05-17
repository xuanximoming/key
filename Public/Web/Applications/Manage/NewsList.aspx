<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewsList.aspx.cs" Inherits="DrectSoft.Emr.Web.Applications.Manage.NewsList" %>

<%@ Import Namespace="System.Data" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>新闻管理列表</title>
    <script language="JavaScript" type="text/javascript" src="../Javascript/public.js"></script>
    <link href="../Css/skin.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <script language="javascript" type="text/javascript">
                function ConfirmDel() {
                    var chk = document.getElementsByName("define_checkbox");
                    for (var i = 0; i < chk.length; i++) {
                        if (chk[i].checked) {
                            if (confirm('确认删除所选新闻吗?')) {
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
                function ConfirmValid() {
                    var chk = document.getElementsByName("define_checkbox");
                    for (var i = 0; i < chk.length; i++) {
                        if (chk[i].checked) {
                            if (confirm('确认对所选信息做过期吗?')) {
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
                function SearchValid() {
                    var strtextName = document.getElementById("NewsTitle").value;
                    if (strtextName == "请输入新闻标题" || strtextName.replace(/\s/ig, '') == "") {
                        alert("请输入新闻标题");
                        document.getElementById("NewsTitle").focus();
                        return false;
                    }
                    document.getElementById("NewsTitle").value = document.getElementById("NewsTitle").value.replace(/\s/ig, '');
                }
            </script>
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="17" valign="top" background="../Images/mail_leftbg.gif">
                        <img src="../Images/left-top-right.gif" width="17" height="29" /></td>
                    <td valign="top" background="../Images/content-bg.gif">
                        <table width="100%" height="31" border="0" cellpadding="0" cellspacing="0" class="left_topbg" id="table2">
                            <tr>
                                <td height="31">
                                    <div class="titlebt">新闻管理</div>
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
                                    <!--放置主体内容开始-->
                                    <div class="add_title">新闻管理</div>
                                    <div>
                                        <table width="100%" border="0" cellpadding="3" cellspacing="1" align="center">
                                            <tr>
                                                <td style="padding-left: 15px;">

                                                    <a href="NewsClass_List.aspx">分类管理</a>&nbsp;┋&nbsp;<a href="NewsClass_Add.aspx">新增分类</a>&nbsp;┋&nbsp;<a href="NewsAdd.aspx">新增新闻</a>&nbsp;┋&nbsp;<asp:LinkButton ID="DelP" runat="server" CssClass="topnavichar" OnClick="DelSelect_Click" OnClientClick="return ConfirmDel();">删除所选</asp:LinkButton>&nbsp;┋&nbsp;<asp:LinkButton ID="ValidP" runat="server" CssClass="topnavichar" OnClick="ValidSelect_Click" OnClientClick="return ConfirmValid();">过期所选</asp:LinkButton>&nbsp;┋&nbsp;<a href="NewsDefault.aspx">新闻首页</a>&nbsp;
                                                </td>
                                                <td>
                                                    <div style="float: right; margin-top: 0px; margin-left: 0px; margin-right: 0px; padding: 0px">
                                                        <asp:Button ID="SearchNews" Text="搜索" CssClass="op_normal" runat="server" OnClientClick="return SearchValid()" OnClick="SearchNews_Click" />
                                                    </div>
                                                    <div style="float: right; margin-top: 5px; margin-left: 0px; margin-right: 0px; padding: 0px">
                                                        <input name="NewsTitle" runat="server" style="width: 200px;" id="NewsTitle" value="请输入新闻标题" onclick="if (this.value == '请输入新闻标题') { this.value = ''; }" onblur="if (this.value == '') { this.value = '请输入新闻标题'; }" type="text">
                                                    </div>

                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div>
                                        <asp:Repeater ID="Repeater_NewsClass_List" runat="server">
                                            <HeaderTemplate>
                                                <table width="100%" border="0" align="center" cellpadding="2" cellspacing="0" class="sys_list_table" bgcolor="#FFFFFF">
                                                    <tr class="TR_BG">
                                                        <td width="30%" align="middle" class="sys_topBg">新闻标题</td>
                                                        <td align="middle" class="sys_topBg">新闻类别</td>
                                                        <td align="middle" class="sys_topBg">添加时间</td>
                                                        <td align="middle" class="sys_topBg">添加人</td>
                                                        <td align="middle" class="sys_topBg">发布对象</td>
                                                        <td align="center" valign="middle" class="sys_topBg">属性
            <input type="checkbox" id="define_checkbox2" value="-1" name="define_checkbox2" onclick="javascript: return selectAll(this.form, this.checked);" /></td>
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
                                    <div>
                                        <webdiyer:AspNetPager ID="AspNetPager1" runat="server" HorizontalAlign="Center" PageSize="30" OnPageChanged="AspNetPager1_PageChanged"
                                            ShowCustomInfoSection="Left" Width="100%" meta:resourceKey="AspNetPager1" Style="font-size: 12px" InputBoxStyle="width:19px"
                                            CustomInfoHTML="一共有<b><font color='red'>%RecordCount%</font></b>条记录 当前页<font color='red'><b>%CurrentPageIndex%/%PageCount%</b></font>   次序 %StartRecordIndex%-%EndRecordIndex%" AlwaysShow="True" FirstPageText="首页" LastPageText="尾页" NextPageText="下一页" PrevPageText="上一页" CustomInfoStyle="FONT-SIZE: 12px">
                                        </webdiyer:AspNetPager>
                                    </div>

                                    <!--放置主体内容结束-->
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
