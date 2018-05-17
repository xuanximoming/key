<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Search3.aspx.cs" Inherits="DrectWeb.Search2.yaopin.Search3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .cell1
        {
            text-align: right;width:80px;vertical-align:top
        }
        .cell2
        {
            text-align: left; 
        }
        #search_logo
        {
            width: 270px;
            height: 75px;
            background: url(../images/logo_search.jpg) no-repeat center center;
            margin: 0 auto;
        }
        td{padding-top:20px;}
    </style>
    <link rel="stylesheet" href="../css/style_search_subpage.css" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align: center">
    <div style="text-align: left"><a style="cursor:pointer" href="../default.aspx?type=yp"><div id="search_logo">
        </div></a></div>
    
        <div style="width: 800px; border: 1px solid #ACC4E0; padding: 5px">
            <div class="posi_nav" style="background-color:#E1EEFF;width:770px">
                <p>
                    您所在的位置：</p>
                <a href="../default.aspx?type=yp">医药搜索</a> <font style="float: left"> &nbsp;&nbsp;药品 </font>
            </div>
              <br />
            <br />
            <br />
            <div>
                <h1>
                    <asp:Label runat="server" ID="lbTitle"></asp:Label></h1>
            </div>
            <div>
                <table class="table_Contain" style="width:770px">
                    <tr>
                        <td class="cell1" >
                            分类：
                        </td>
                        <td class="cell2">
                            <asp:Label ID="lb_CATEGORYONE" runat="server" Text=""></asp:Label>/
                            <asp:Label ID="lb_CATEGORYTWO" runat="server" Text=""></asp:Label>/
                            <asp:Label ID="lb_CATEGORYTHREE" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="cell1">
                            规格：
                        </td>
                        <td class="cell2">
                            <asp:Label ID="lb_SPECIFICATION" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="cell1">
                            适用症：
                        </td>
                        <td class="cell2">
                            <asp:Label ID="lb_APPLYTO" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="cell1">
                            用法：
                        </td>
                        <td class="cell2">
                            <asp:Label ID="lb_REFERENCEUSAGE" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="cell1">
                            注意事项：
                        </td>
                        <td class="cell2">
                            <asp:Label ID="lb_MENO" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
