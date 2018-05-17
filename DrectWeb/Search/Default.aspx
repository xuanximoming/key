<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DrectWeb.Search2.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <style type="text/css">
        body
        {
            margin: 0;
            padding: 0;
            font-size: 12px;
            color: #000;
            text-align: center;
            width: 100%;
        }
        #main_search_area
        {
            width: 1000px;
            margin: 0px auto;
            padding: 4% 0 0 0;
        }
        #search_logo
        {
            width: 270px;
            height: 75px;
            background: url(images/logo_search.jpg) no-repeat center center;
            margin: 0 auto;
        }
        #search_nav
        {
            margin: 50px 0 0 300px;
            padding: 0 20px 0 20px;
            float: left;
            height: 25px;
            background: url(images/slide_title_bkg_02.gif) repeat-x bottom center;
            display: inline;
        }
        #search_nav li
        {
            list-style: none;
            font-weight: bold;
            float: left;
            margin: 0 3px 0 0;
        }
        #search_nav li a
        {
            display: block;
            padding: 0 0 0 10px;
            text-decoration: none;
        }
        #search_nav li a span
        {
            display: block;
            padding: 6px 10px 5px 0;
            cursor: pointer;
        }
        .sd01 a
        {
            background: url(images/slide_nav_bkg_2.gif) no-repeat top left;
            color: #069;
        }
        .sd01 a span
        {
            background: url(images/slide_nav_bkg_2.gif) no-repeat top right;
        }
        .sd02 a
        {
            background: url(images/slide_nav_bkg_2_02.gif) no-repeat top left;
            color: #666;
        }
        .sd02 a span
        {
            background: url(images/slide_nav_bkg_2_02.gif) no-repeat top right;
        }
        #search_content
        {
            width: 1000px;
            float: left;
        }
        #zs_search_div
        {
            padding: 10px 0 0 0;
            text-align: center;
            width: 1000px;
            float: left;
        }
        #zs_search_hot_tags
        {
            width: 1000px;
            float: left;
            padding: 6px 0 0 0;
            height: 19px;
            margin: 10px 0 0 0;
            text-align: center;
        }
        #zs_search_hot_tags a
        {
            color: #f60;
            text-decoration: none;
        }
        #zs_search_hot_tags a:hover
        {
            color: #f00;
            text-decoration: underline;
        }
        #zs_link_box
        {
            width: 1000px;
            float: left;
            padding: 6px 0 0 0;
            height: 19px;
            margin: 10px 0 0 0;
            text-align: center;
        }
        #zs_link_box a
        {
            color: #060;
            text-decoration: none;
        }
        #zs_link_box a:hover
        {
            color: #f00;
            text-decoration: underline;
        }
        #zs_premier_search
        {
            width: 1000px;
            float: left;
            padding: 6px 0 0 0;
            height: 19px;
            margin: 10px 0 0 0;
            text-align: center;
        }
        #zs_premier_search a
        {
            color: #060;
            font-weight: bold;
            text-decoration: none;
        }
        .zs_class_linklist
        {
            width: 580px;
            float: left;
            border: 1px solid #069;
            margin: 0;
            padding: 10px;
            margin: 20px 0 0 200px;
            display: inline;
        }
        .zs_class_linklist li
        {
            list-style: none;
            float: left;
            width: 580px;
            padding: 6px 0 5px 0;
        }
        .zs_class_linklist li a
        {
            display: block;
            padding: 5px 0 4px 5px;
            float: left;
            text-decoration: none;
            color: #069;
        }
        .zs_class_linklist li a:hover
        {
            color: #f00;
            text-decoration: underline;
        }
        #bottom
        {
            width: 1000px;
            float: left;
            padding: 30px 0 15px 0;
            text-align: center;
            line-height: 1.8em;
        }
        #bottom a
        {
            color: #069;
            text-decoration: none;
        }
        .main_searchbox
        {
            width: 394px;
            height: 19px;
            border: none;
            background: none;
            padding: 4px 3px 0 3px;
            position: absolute;
            top: 31px;
            left: 23px;
        }
        #top_user_nav
        {
            float: right;
            padding: 0 15px 0 0;
        }
        #top_user_nav a
        {
            display: block;
            padding: 6px 0 5px 10px;
            float: left;
            color: #000;
            text-decoration: none;
        }
        #top_user_nav a:hover
        {
            color: #f00;
            text-decoration: underline;
        }
        body
        {
            margin: 0;
            padding: 0;
            font-size: 12px;
            color: #000;
            text-align: center;
            width: 100%;
        }
        #search_top_nav
        {
            float: left;
            padding: 0 0 0 15px;
        }
        #search_top_nav a
        {
            display: block;
            float: left;
            padding: 6px 10px 5px 0;
            color: #000;
            text-decoration: none;
        }
        #search_top_nav a:hover
        {
            color: #f00;
            text-decoration: underline;
        }
    </style>
    <link href="css/autocomplete.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="css/tabtrip.css" type="text/css" />
    <script language="javascript" type="text/javascript">
 
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
    </telerik:RadWindowManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" BackgroundPosition="Center"
        Skin="Web20">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="cbxClientCode">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="cbxClientCode"  />

                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="tbs">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tbs"  />
                    <telerik:AjaxUpdatedControl ControlID="cbxClientCode"  />


                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <%--<div style="width: 100%; border-bottom: 1px solid #ccc;">
        <div id="search_top_nav">
        </div>
    </div>--%>
    <div id="main_search_area">
        <div id="search_logo">
        </div>
        <br /><br />
        <div style="margin: 0 auto; text-align: center" id="main_search">
            <telerik:RadTabStrip ID="tbs" runat="server" Skin="" SelectedIndex="2" EnableEmbeddedSkins="False"  
                Width="270px" ontabclick="tbs_TabClick"  >
                <Tabs>
                    <telerik:RadTab Text="说明书" Value="sms" CssClass="fourthTab" Width="89"  Selected="true"
                        SelectedCssClass="fourthTabSelected">
                    </telerik:RadTab>
                    <telerik:RadTab Text="药品" Value="yp" CssClass="fourthTab" Width="89" 
                        SelectedCssClass="fourthTabSelected"
                         >
                    </telerik:RadTab>
                    <telerik:RadTab Text="药典" Value="yd" CssClass="fourthTab" Width="89" 
                        SelectedCssClass="fourthTabSelected"  
                         >
                    </telerik:RadTab>
                </Tabs>
            </telerik:RadTabStrip> 
            
        </div>
        <div id="search_content">
            <div id="zs_search_div">
                <div style="width: 800px; display: inline" class="searchGoogleIpt">
                    <div style='margin: 0 0 0 200; padding: 0; width: 500px; font-size: 14px;'>
<table>
<tr><td>
                        <telerik:RadComboBox Width="200" ID="cbxClientCode" runat="server" AutoPostBack="true"
                            EnableLoadOnDemand="true" OnItemsRequested="cbxClientCode_ItemsRequested" DropDownWidth="300px"
                            ShowMoreResultsBox="true" EnableVirtualScrolling="true" 
                            ShowToggleImage="False"  onselectedindexchanged="cbxClientCode_SelectedIndexChanged"
                            
                            >
                        </telerik:RadComboBox></td>
                        <td><input id="search" type="button" runat="server"  class="srh_onesearch" style="margin: 0;
                            padding-top: 5 !important;width:80px" value="搜索" onserverclick="search_Click" />
                            <a href="yaodian/yaodiansearch3.aspx">药典浏览</a>
                            </td>
                            </tr>
                            </table>
                    </div>
                </div>
                <div style="width: 200px; display: inline; margin: 0 0 0 10px; float: left">
                    <%-- <asp:Button runat="server" ID="submit" OnClick="submit_Click" />--%>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
