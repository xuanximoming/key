<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Master.aspx.cs" Inherits="DrectSoft.Emr.Web.Master1" %>

<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxNavBar" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script src="Applications/Javascript/LunarCalendar.js" type="text/javascript"></script>

    <style type="text/css">
        body
        {
            margin: 0;
            padding: 0;
            font: 12px "宋体";
            background: #d9e2ec;
            min-width: 900px;
            /*width: expression((documentElement.clientWidth < 1000) ? "1000px" : "auto" );*/
            width:100%;
        }
        .hidden
        {
            display: none;
        }
        body p
        {
            margin: 0;
            padding: 0;
            line-height: 14px;
        }
        body a
        {
            color: #000;
            text-decoration: none;
        }
        body a:hover
        {
            color: #f00;
            text-decoration: underline;
        }
        body ul
        {
            margin: 0;
            padding: 0;
        }
        body ul li
        {
            list-style: none;
        }
        .main_outer
        {
            margin: 0 auto;
            padding: 0;
            width: 100%;
        }
        .top_frame
        {
            width: 100%;
            height: 80px;
            background: url(Applications/Images/2010-2-10-02_03.png) repeat-x center center;
            position: relative;
        }
        .top_logo
        {
            width: 324px;
            height: 65px;
            position: absolute;
            top: 5px;
            left: 30px;
            background: url(Applications/Images/2010-2-10-02_09_2_2.jpg) no-repeat center center;
        }
        .top_mainnav
        {
            width: 782px;
            height: 32px;
            position: absolute;
            bottom: 0;
            left: 0;
            background: url(Applications/Images/2010-2-10-02_13.png) no-repeat left center;
        }
        .top_mainnav li
        {
            float: left;
            width: 93px;
            height: 14px;
            padding: 9px 0 9px 0;
            text-align: center;
            line-height: 14px;
        }
        .sd02 a
        {
            color: #fff;
            font-weight: bold;
        }
        .sd01
        {
            background: url(Applications/Images/2010-2-10-02_09_03.png) no-repeat top center;
        }
        .sd01 a
        {
            color: #069;
            font-weight: bold;
        }
        .task_center
        {
            position: absolute;
            top: 0;
            right: 0;
            width: 200px;
            height: 108px;
            padding: 5px 10px 5px 10px;
            border-left: 1px solid #6f82c4;
        }
        .task_center_title
        {
            width: 180px;
            height: 14px;
            padding: 0 0 4px 20px;
            background: url(Applications/Images/2010-2-10-02_06.png) no-repeat left center;
            line-height: 14px;
            float: left;
            font-weight: bold;
            color: #069;
        }
        .task_center_list
        {
            width: 180px;
            float: left;
        }
        .task_center_list li
        {
            width: 180px;
            float: left;
            height: 14px;
            padding: 2px 0 2px 0;
        }
        .top_user_inf
        {
            width: 100%;
            height: 27px;
            border-bottom: 1px solid #6f82c4;
            background: #d9e2ec;
            margin: 0 auto;
            position: relative;
        }
        .top_user_inf_text
        {
            float: left;
            height: 14px;
            padding: 7px 10px 6px 10px;
        }
        .top_user_inf_text span
        {
            color: #f00;
        }
        .right_func
        {
            width: 170px;
            height: 27px;
            position: absolute;
            right: 0;
            top: 0;
        }
        .func_button
        {
            margin: 4px 5px 0 0;
            height: 20px;
            padding: 3px 19px 2px 0;
            color: #069;
            font-weight: bold;
            cursor: pointer;
            border: none;
            float: left;
        }
        #botton_01
        {
            background: url(Applications/Images/2010-2-10-02_18.png) no-repeat right center;
        }
        #botton_02
        {
            background: url(Applications/Images/2010-2-10-02_20.png) no-repeat right center;
        }
    </style>

    <script language="Javascript" type="text/javascript">
        var DispClose = true;
        function CloseEvent() {
            if (DispClose) {
                return "是否离开当前页面?";
            }
        }
        window.onbeforeunload = CloseEvent;

        function SetDispState() {
            DispClose = false;
        }

        function ShowStatPage() {
            document.getElementById("MainFrame").src = "Applications/QualityControl/QCStatic.aspx";
        }
        function showTime() {
            var systemTime = document.getElementById("SystemTime");
            if (systemTime != null) {
                systemTime.innerHTML = YYMMDD() + " " + weekday() + " " + CurentTime() + "(" + solarDay2() + ")";
                setTimeout("showTime()", 1000);
            }
        }
        function showTimeDelay() {
            setTimeout("showTime()", 1000);
        }
    </script>

</head>
<body onload="showTime()" scrolling="Yes">
    <form id="form1" runat="server">
    <div class="main_outer">
        <div class="top_frame">
            <div class="top_logo">
            </div>
            <div class="hidden">
                <div id="c01">
                </div>
                <div id="c02">
                </div>
                <div id="c03">
                </div>
                <div id="c04">
                </div>
                <div id="c05">
                </div>
                <div id="c06">
                </div>
                <div id="c07">
                </div>
                <div id="c08">
                </div>
            </div>
        </div>
        <div class="top_user_inf">
            <p class="top_user_inf_text">
                当前用户：<asp:Label ID="LabelUser" runat="server"></asp:Label></p>
            <p class="top_user_inf_text">
                当前时间：<asp:Label ID="SystemTime" runat="server"></asp:Label></p>
            <td class="text_content" id="tdDate" align="left" />
            <div class="right_func">
                <input type="button" class="func_button" id="botton_01" value="首页" onclick="javascript:ShowStatPage()" />
                <input type="button" onclick="javascript:window.location.href='Default.aspx';" class="func_button"
                    id="botton_02" value="退出" /></div>
        </div>
        <div class="center_frame">
        </div>
    </div>
    <table width="100%">
        <tr>
            <td valign="top" >
                <div>
                    <dx:ASPxNavBar ID="ASPxNavBarTree" runat="server" Width="100%" CssFilePath="~/App_Themes/Plastic Blue/{0}/styles.css"
                        CssPostfix="PlasticBlue" GroupSpacing="1px" ImageFolder="~/App_Themes/Plastic Blue/{0}/">
                        <LoadingPanelImage Url="~/App_Themes/Plastic Blue/Web/nbLoading.gif" />
                        <ItemStyle ImageSpacing="8px" />
                        <CollapseImage Url="~/App_Themes/Plastic Blue/Web/nbCollapse.gif" />
                        <ExpandImage Url="~/App_Themes/Plastic Blue/Web/nbExpand.gif" />
                        <ItemImage Url="~/App_Themes/Plastic Blue/Web/nbItemBullet.gif" />
                    </dx:ASPxNavBar>
                </div>
            </td>
            <td style="width: 87%;" align="left" valign="top">
                <div>
                    <iframe id="MainFrame" class="iframe" style="background-color: transparent;" name="MainFrame"
                        height="590px" src="Applications/QualityControl/QCStatic.aspx" frameborder="0" 
                        width="100%" marginheight="0" marginwidth="0" scrolling="auto"></iframe>
                </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
