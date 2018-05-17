<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InpPat1.aspx.cs" Inherits="DrectWeb.Applications.inquire.InpPat1" %>

<%@ Register Assembly="DevExpress.XtraCharts.v14.1.Web, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <dxchartsui:WebChartControl ID="WebChartControl1" runat="server" Visible="false" 
            Style="font-family: Arial;font-size: 11px;" Height="400px" Width="800px">
        </dxchartsui:WebChartControl>
    </div>
    </form>
</body>
</html>
