<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Restaurant | Login</title>
    <%--<link href="css/layout.css" rel="stylesheet" type="text/css" />--%>
    <link rel="stylesheet" href="css/modal.css"  type="text/css" />
    <link rel="stylesheet" href="css/styles_green.css" type="text/css" />
    <%--<link rel="stylesheet" type="text/css" href="css/style.css" media="screen" />--%>
</head>
<body>
    <form id="form1" runat="server">
    <%--=====back ground=========--%>
    <div align="center">
        <img src="Images/Banner.png" width="1000" height="100" alt="RSS" /></div>
    <%--=====Title Bar=========--%>
    <%--<div id="menu">
        <ul class="tabs">
            <li>
                <h4>
                    <a href="#">In the blog &raquo;</a></h4>
            </li>
            <li class="hasmore"><a href="#"><span>DownLoad</span></a>
                <ul class="dropdown">
                    <li><a href="#">Menu item 1</a></li>
                    <li><a href="#">Menu item 2</a></li>
                    <li><a href="#">Menu item 3</a></li>
                    <li class="last"><a href="#">Menu item 6</a></li>
                </ul>
            </li>
            <li class="hasmore"><a href="#"><span>UpLoad</span></a>
                <ul class="dropdown">
                    <li><a href="#">Topic 1</a></li>
                    <li><a href="#">Topic 2</a></li>
                    <li><a href="#">Topic 3</a></li>
                    <li class="last"><a href="#">Topic 4</a></li>
                </ul>
            </li>
            <li><a href="#"><span><strong>
                <img src="images/feed-icon-14x14.png" width="14" height="14" alt="RSS" />
                Subscribe to RSS</strong></span></a></li>
            <li>
                <h4>
                    <a href="#">Elsewhere &raquo;</a></h4>
            </li>
            <li><a href="#"><span>About</span></a></li>
            <li class="hasmore"><a href="/about/#networks"><span>ViewReport</span></a>
                <ul class="dropdown">
                    <li><a href="#">Twitter</a></li>
                    <li><a href="#">posterous</a></li>
                    <li><a href="#">SpeakerSite</a></li>
                    <li><a href="#">LinkedIn</a></li>
                    <li class="last"><a href="#">See more&hellip;</a></li>
                </ul>
            </li>
            <li><a href="#"><span>Bookmarks</span></a></li>
        </ul>
    </div>
    <div id="form">
    </div>
    <script type="text/javascript" src="fancydropdown.js"></script>--%>
    <%--<div id="menu">
        <ul class="tabs">
        </ul>
    </div>
    <script type="text/javascript" src="fancydropdown.js"></script>--%>
    <%--=====Welcome Bar=========--%>
    <div class="panel">
        <h2 align="center">
            Welcome User!</h2>
        <div style="margin-top: -30px;">
            <a href="#login_form" id="login_pop">Log In</a> 
        </div>
    </div>
    
    <%--=====Passwrd picture=========--%>
    <%--<div id="stickynote">
        <br />
        <div id="stickynote-cont">
            Password must
            <ul>
                <li>not contain username</li>
                <li>have min 8 chars</li>
                <li>contain atleast one alphabet, numeric and special char</li>
            </ul>
        </div>
    </div>--%>
    <%--======popup1--%>
    <a href="#x" class="overlay" id="login_form"></a>
    <div class="popup">
        <h2 align="center" style="margin-top: 25px;">
            Sign In Here</h2>
        <div align="center">
        </div>
        <div style="margin-top: 40px; margin-left: 15px;">
            <div>
                <label for="login">
                    Login ID</label>
                <asp:TextBox Width="300px" ID="LoginTxt" BackColor="Transparent" runat="server" />
            </div>
            <div style="margin-top: 18px;">
                <label for="password">
                    Password</label>
                <asp:TextBox Width="300px" ID="PwdTxt" BackColor="Transparent" runat="server" TextMode="Password" />
            </div>
            <asp:Button ID="LoginBtn" Text="Log In" runat="server" BackColor="Transparent"  ForeColor="Black"  OnClick="LoginBtn_Click" Style="margin-top: 10px;
                margin-left: 420px;  border: 0;" />
        </div>
        <a class="close" href="#close"></a>
    </div>
    <%--======popup2--%>
    <a href="#x" class="overlay" id="join_form"></a>
    <div class="popup" style="color: White">
        <h2>
            Sign Up</h2>
        <p>
            Please enter your details here</p>
        <div>
            <label for="email">
                Login (Email)</label>
            <input type="text" id="email" value="" />
        </div>
        <div>
            <label for="pass">
                Password</label>
            <input type="password" id="pass" value="" />
        </div>
        <div>
            <label for="firstname">
                First name</label>
            <input type="text" id="firstname" value="" />
        </div>
        <div>
            <label for="lastname">
                Last name</label>
            <input type="text" id="lastname" value="" />
        </div>
        <input type="button" value="Sign Up" />&nbsp;&nbsp;&nbsp;or&nbsp;&nbsp;&nbsp;<a href="#login_form"
            id="A1">Log In</a> <a class="close" href="#close"></a>
    </div>
    </form>
</body>
</html>
