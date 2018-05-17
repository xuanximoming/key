<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" CodeBehind="Default.aspx.cs"
    Inherits="DrectSoft.Emr.Web.Default" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>欢迎访问一丹EMR质量管理平台！</title>
    <style type="text/css">
        body
        {
            margin: 0;
            padding: 0;
            font: 12px "宋体";
            background: #eff1f0;
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
        .main_login_frame
        {
            width: 761px;
            height: 432px;
            margin: 100px auto;
            background: url(Applications/Images/login_bkg_03.png) no-repeat center center;
            position: relative;
        }
        .login_input_div
        {
            position: absolute;
            left: 304px;
            top: 164px;
        }
        .login_input_content
        {
            float: left;
            width: 440px;
            padding: 5px 0 5px 0;
        }
        .login_input_content p
        {
            width: 58px;
            float: left;
            height: 14px;
            padding: 4px 0 4px 0;
        }
        .login_input
        {
            width: 197px;
            height: 22px;
            padding: 2px 4px 2px 4px;
            line-height: 22px;
            background: url(Applications/Images/login_img_03.png) no-repeat center center;
            border: none;
        }
        .login_submit_button
        {
            width: 210px;
            float: left;
            padding: 10px 0 0 58px;
            _padding: 10px 0 0 61px;
        }
        .submit_boutton
        {
            width: 85px;
            height: 27px;
            vertical-align: middle;
            color: #fff;
            border: none;
            background: url(Applications/Images/login_img_06.png) no-repeat center center;
            margin: 0;
            padding: 0;
            text-align: center;
            line-height: 14px;
            cursor: pointer;
            float: left;
        }
        .bottom_inf
        {
            width: 430px;
            float: left;
            color: #f00;
            padding: 10px 0 5px 10px;
            height: 14px;
            line-height: 14px;
        }
        .bottm_link_content
        {
            width: 440px;
            float: left;
            padding: 10px 0 4px 0;
        }
        .bottm_link_content a
        {
            display: block;
            float: left;
            height: 14px;
            border-right: 1px solid #069;
            color: #069;
            padding: 0 6px 0 6px;
        }
    </style>

    <script type="text/javascript">
        function GetLockInfo() {
            if (charge()) {
                if (document.getElementById('_txtLoginName').value != "") {
                    __doPostBack('lbtn_Hidden');
                }
                else {
                }
            }
        }

        function charge() {
            ischarge = true;
            return ischarge;
        }

        window.onload = function() { document.form1._txtLoginName.focus(); }
    
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div class="main_login_frame">
        <div class="login_input_div">
            <div class="login_input_content">
                <p>
                    用户名：</p>
                <input type="text" name="_txtLoginName" id="_txtLoginName" runat="server" class="login_input" />
            </div>
            <div class="login_input_content">
                <p>
                    密&nbsp;&nbsp;码：</p>
                <input type="password" name="_txtPassword" id="_txtPassword" runat="server" class="login_input" />
            </div>
            <div class="login_submit_button">
                <input type="button" class="submit_boutton" value="登录" style="margin: 0 35px 0 0;"
                    runat="server" onclick="javascript:GetLockInfo();" />
                <input type="reset" class="submit_boutton" value="取消" />
            </div>
            <div class="bottom_inf">
                <td id="tdMsg" runat="server">
                    <font face="宋体" size="1pt" color="red">
                        <asp:Label ID="lMsg" runat="server" Font-Bold="True" Font-Size="12px"></asp:Label>
                    </font>
                </td>
            </div>
            <div class="bottm_link_content" style="display:none">
                <a href="http://www.ydyyw.com/">一丹医药网</a> <a href="http://www.yidansoft.com/">一丹软件</a>
                <a href="http://www.yidansoft.com/soft_about.html">关于我们</a> <a href="http://www.yidansoft.com/soft_contract.html"
                    style="border: none;">联系我们</a>
            </div>
        </div>
        <tr style="display: none">
            <td>
                <asp:LinkButton Style="display: none" ID="lbtn_Hidden" runat="server" OnClick="lbtn_Hidden_Click"></asp:LinkButton>
                <input type="hidden" id="lockID" runat="server" />
                <input type="hidden" id="_Hidden1" runat="server" />
            </td>
        </tr>
    </div>

    <script language="javascript" type="text/javascript">
        document.all["_txtPassword"].onkeydown = function() { if (event.keyCode == 13) GetLockInfo(); }
        document.all["_txtLoginName"].onkeydown = function() { if (event.keyCode == 13) document.form1._txtPassword.focus(); }
    </script>

    </form>
</body>
</html>
