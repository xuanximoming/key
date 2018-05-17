<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="exmple.aspx.cs" Inherits="DrectWeb.Applications.inquire.exmple" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="http://localhost/DrectWeb/Applications/Javascript/echarts.min.js" type="text/javascript"></script>
    <style>
        div {float:left}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <%=this.GetHtml()%>
        <script type="text/javascript">
            <%=this.GetJson()%>
        </script>
    </form>
</body>
</html>
