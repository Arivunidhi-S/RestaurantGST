<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Report.aspx.cs" Inherits="Report" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Stimulsoft.Report.Web, Version=2011.2.1100.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a"
    Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <<title>RESTAURANT | Report</title>
    <telerik:RadStyleSheetManager ID="RadStyleSheetManager1" runat="server" />
    <style type="text/css">
        body { font-family:Arial, Helvetica, Sans-Serif; font-size:12px; margin:0px 20px;}
        /* menu */
        #menu{ margin:0px; padding:0px; list-style:none; color:#fff; line-height:45px; display:inline-block; float:left; z-index:1000; }
        #menu a { color:#fff; text-decoration:none; }
        #menu > li {background:#172322 none repeat scroll 0 0; cursor:pointer; float:left; position:relative;padding:0px 10px;}
        #menu > li a:hover {color:#B0D730;}
        #menu .logo {background:padding:0px; }
        /* sub-menus*/
        #menu ul { padding:0px; margin:0px; display:block; display:inline;}
        #menu li ul { position:absolute; left:-10px; top:0px; margin-top:45px; width:150px; line-height:16px; background-color:#172322; color:#0395CC; /* for IE */ display:none; }
        #menu li:hover ul { display:block;}
        #menu li ul li{ display:block; margin:5px 20px; padding: 5px 0px;  border-top: dotted 1px #606060; list-style-type:none; }
        #menu li ul li:first-child { border-top: none; }
        #menu li ul li a { display:block; color:#0395CC; }
        #menu li ul li a:hover { color:#7FCDFE; }
        /* main submenu */
        #menu #main { left:0px; top:-20px; padding-top:20px; background-color:#7cb7e3; color:#fff; z-index:999;}--%>
        #menu #main { left:0px; top:-20px; padding-top:20px; color:#fff; z-index:999; background:none;}
        /* search */
        .searchContainer div { background-color:#fff; display:inline; padding:5px;}
        .searchContainer input[type="text"] {border:none;}
        .searchContainer img { vertical-align:middle;}
        /* corners*/
        #menu .corner_inset_left { position:absolute; top:0px; left:-12px;}
        #menu .corner_inset_right { position:absolute; top:0px; left:150px;}
        #menu .last { background:transparent none repeat scroll 0% 0%; margin:0px; padding:0px; border:none; position:relative; border:none; height:0px;}
        #menu .corner_left { position:absolute; left:0px; top:0px;}
        #menu .corner_right { position:absolute; left:132px; top:0px;}
        #menu .middle { position:absolute; left:18px; height: 20px; width: 115px; top:0px;}
    </style>
</head>
<body style="background-image: url('images/BG.jpg'); margin: 10px 0px 0px 0px;">
    <div align="center" style="margin-top: -0px;">
        <img alt="" src="bar.png" width="100%" height="45px" /></div>
    <div align="center" style="background-image: url('bar.png'); width: 1000px; margin-top: -47px;">
        <% 
            System.Data.DataTable dtItems = new System.Data.DataTable();
            System.Data.DataTable dtItems2 = new System.Data.DataTable();
            String user_group = Session["user_group"].ToString().Trim();
            System.Data.SqlClient.SqlConnection conn = BusinessTier.getConnection();
            System.Data.SqlClient.SqlConnection conn2 = BusinessTier.getConnection();
            conn.Open();
            conn2.Open();
            //System.Data.SqlClient.SqlDataReader reader;
            String sql = "select * from vw_ModulePermissionGroup where group_aid=" + user_group.ToString().Trim() + " order by TabOrder";
            //System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql, conn);
            //reader = cmd.ExecuteReader();
            dtItems = new System.Data.DataTable();
            dtItems.Clear();

            System.Data.SqlClient.SqlDataAdapter sqlDataAdapter = new System.Data.SqlClient.SqlDataAdapter(sql, conn);
            sqlDataAdapter.Fill(dtItems);
            int rco = dtItems.Rows.Count;
       
        %>
        <ul id="menu">
            <li class="logo"><a href="http://www.e-serbadk.com/IT/contactus.html" target="_blank"
                style="border: none">
                <img style="float: left;" alt="" border="0" src="images/vimeo/menu_left.png" /></a>
                <ul id="main">
                    <li><a href="http://www.e-serbadk.com/IT/contactus.html"><font color="white"><b>Enquiry?</b></font></a></li>
                    <li class="last">
                        <img class="corner_left" alt="" src="images/vimeo/corner_blue_left.png" />
                        <img class="middle" alt="" src="images/vimeo/dot_blue.png" />
                        <img class="corner_right" alt="" src="images/vimeo/corner_blue_right.png" />
                    </li>
                </ul>
            </li>
            <%         
                if (rco > 0)
                {
                    String mn = "", cfn = "";
                    foreach (System.Data.DataRow dataRow in dtItems.Rows)
                    {
                        if (dataRow["Parent"].ToString() == "0")
                        {

                            String sql2 = "select * from vw_ModulePermissionGroup where group_aid=" + user_group.ToString().Trim() + " and Parent=" + dataRow["ModuleId"].ToString().Trim() + " order by TabOrder";
                            //System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql, conn);
                            //reader = cmd.ExecuteReader();
                            dtItems2 = new System.Data.DataTable();
                            dtItems2.Clear();
                            System.Data.SqlClient.SqlDataAdapter sqlDataAdapter2 = new System.Data.SqlClient.SqlDataAdapter(sql2, conn2);
                            sqlDataAdapter2.Fill(dtItems2);
                            int rco2 = dtItems2.Rows.Count;

                            if (rco2 > 0)
                            {
            %>
            <li><a href="<%=dataRow["CodeFileName"].ToString()%>"><span>
                <%= dataRow["ModuleName"].ToString()%></span></a>
                <ul id="Ul1">
                    <%
                        foreach (System.Data.DataRow dataRow2 in dtItems2.Rows)
                        {
                            if ((dataRow2["ModuleName"].ToString() == "UserMap") || (dataRow2["ModuleName"].ToString() == "AdminMap"))
                            {%>
                    <li><a href="#" onclick="window.open('<%=dataRow2["CodeFileName"].ToString()%>'); return false;"
                        target="_blank"><span>
                            <%= dataRow2["ModuleName"].ToString()%></span></a></li>
                    <%}
                            else
                            {%>
                    <li><a href="<%=dataRow2["CodeFileName"].ToString()%>"><span>
                        <%= dataRow2["ModuleName"].ToString()%></span></a></li>
                    <%
                        }
                        }
                        
                    %>
                    <li class="last">
                        <img class="corner_left" alt="" src="images/vimeo/corner_left.png" />
                        <img class="middle" alt="" src="images/vimeo/dot.gif" />
                        <img class="corner_right" alt="" src="images/vimeo/corner_right.png" />
                    </li>
                </ul>
            </li>
            <%
}
                            else
                            {
            %>
            <li><a href="<%=dataRow["CodeFileName"].ToString()%>"><span>
                <%= dataRow["ModuleName"].ToString()%></span></a></li>
            <%   
}

                        }
 
            %>
            <%
                else
                {

                }
                    }


                }
            
            %>
            <li><a href="Login.aspx">Logout</a> </li>
            <li><a href="#">Help</a>
                <ul id="help">
                    <li>
                        <img class="corner_inset_left" alt="" src="images/vimeo/corner_inset_left.png" />
                        <a href="#">Download Manual</a>
                        <img class="corner_inset_right" alt="" src="images/vimeo/corner_inset_right.png" />
                    </li>
                    <li><a href="#">About</a></li>
                    <li class="last">
                        <img class="corner_left" alt="" src="images/vimeo/corner_left.png" />
                        <img class="middle" alt="" src="images/vimeo/dot.gif" />
                        <img class="corner_right" alt="" src="images/vimeo/corner_right.png" />
                    </li>
                </ul>
            </li>
            <li class="searchContainer">
                <div>
                    <input type="text" id="searchField" />
                    <img src="images/vimeo/magnifier.png" alt="Search" onclick="alert('Sorry Content Search Is Not Available Now!')" /></div>
            </li>
        </ul>
    </div>
    <br />
    <br />
    <br />
    <br />
    <div align="center" style="margin-top: -50px;">
        <img src="Images/Banner.png" width="1000px" height="100px" alt="RSS" />
    </div>
    <form id="form1" runat="server">
    <div align="center">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" align="center">
            <tr>
                <td style="border-right: blue thin solid; border-top: blue thin solid; border-left: blue thin solid;
                    border-bottom: blue thin solid; border-width: 0px" align="center">
                    <table border="0" width="100%" align="center">
                        <tr>
                            <td align="center">
                                <div style="height: 20px;">
                                    <asp:Label ID="lblStatus" Width="80%" runat="server" ForeColor="Yellow" Font-Bold="true" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <table id="Table1" runat="server" align="center">
                                    <tr>
                                        <td>
                                            <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
                                            </telerik:RadScriptManager>
                                            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1">
                                                <AjaxSettings>
                                                    <telerik:AjaxSetting AjaxControlID="RadGrid1">
                                                        <UpdatedControls>
                                                            <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                                                            <telerik:AjaxUpdatedControl ControlID="RadInputManager1" />
                                                            <telerik:AjaxUpdatedControl ControlID="lblStatus" />
                                                            <telerik:AjaxUpdatedControl ControlID="cbostaffno" />
                                                            <telerik:AjaxUpdatedControl ControlID="RadToolTipManager1" />
                                                        </UpdatedControls>
                                                    </telerik:AjaxSetting>
                                                </AjaxSettings>
                                            </telerik:RadAjaxManager>
                                            <telerik:RadToolTipManager ID="RadToolTipManager1" OffsetY="-1" HideEvent="ManualClose"
                                                Width="300" Height="305" runat="server" RelativeTo="Element" Position="MiddleRight">
                                            </telerik:RadToolTipManager>
                                            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style1" align="left">
                                            <font style="font-size: 1.0em; color: White"><span class="style1">Report *</span></font><span
                                                class="style1"> </span>
                                        </td>
                                        <td align="left">
                                            <telerik:RadComboBox ID="txtReport" runat="server" Height="160px" Width="200px" EnableLoadOnDemand="false"
                                                AutoPostBack="true" Filter="None" AppendDataBoundItems="false" OnSelectedIndexChanged="OnSelectedCompo"
                                                EmptyMessage="--Select--">
                                                <Items>
                                                    <telerik:RadComboBoxItem runat="server" Text="Today" Value="Today" />
                                                    <telerik:RadComboBoxItem runat="server" Text="Sales Report DateTime" Value="SalesReportDateTime" />
                                                    <telerik:RadComboBoxItem runat="server" Text="Itemwise Report" Value="ItemwiseReport" />
                                                    <telerik:RadComboBoxItem runat="server" Text="Summary Report" Value="SummaryReport" />
                                                    <telerik:RadComboBoxItem runat="server" Text="Dining Charge Report" Value="Dining Charge Report" />

                                                    <telerik:RadComboBoxItem runat="server" Text="GST Report" Value="GST Report" />

                                                    <telerik:RadComboBoxItem runat="server" Text="PLU Report" Value="PLUReport" />
                                                    <telerik:RadComboBoxItem runat="server" Text="VOID Report" Value="VOIDReport" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                        <td align="left">
                                            <font style="font-size: 1.0em; color: White"><span class="style1">Category:</span></font><span
                                                class="style1"></span>
                                        </td>
                                        <td align="left">
                                            <telerik:RadComboBox ID="cbocagtegory" runat="server" Height="80px" Width="200px"
                                                DataSourceID="SqlDataSourcecategory" DataTextField="name" AutoPostBack="false"
                                                AppendDataBoundItems="True" DataValueField="category_id" Visible="true">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="---All---" Value="0" ForeColor="Silver" />
                                                </Items>
                                            </telerik:RadComboBox>
                                            <asp:SqlDataSource ID="SqlDataSourcecategory" runat="server" ConnectionString="<%$ ConnectionStrings:connString %>"
                                                SelectCommand="select category_id,name from categorymaster where deleted=0 order by category_id">
                                            </asp:SqlDataSource>
                                        </td>
                                        <td class="style1" align="left">
                                            <font style="font-size: 1.0em; color: White"><span class="style1">From</span></font><span
                                                class="style1"> </span>
                                        </td>
                                        <td>
                                            <telerik:RadDateTimePicker ID="txtFromDate" runat="server" Width="200px" MinDate="01/01/1000"
                                                MaxDate="01/01/3000" DateInput-DateFormat="d/MMM/yyyy hh:mm tt" PopupDirection="BottomRight"
                                                EnableTheming="true">
                                                <Calendar ID="Calendar2" runat="server" ShowRowHeaders="true">
                                                    <SpecialDays>
                                                        <telerik:RadCalendarDay Repeatable="Today" />
                                                    </SpecialDays>
                                                </Calendar>
                                            </telerik:RadDateTimePicker>
                                            <%-- <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtFromDate"
                                                ErrorMessage="Select a date!"></asp:RequiredFieldValidator>--%>
                                        </td>
                                        <td class="style1" align="left">
                                            <font style="font-size: 1.0em; color: White"><span class="style1">To</span></font><span
                                                class="style1"> </span>
                                        </td>
                                        <td>
                                            <telerik:RadDateTimePicker ID="txtToDate" runat="server" Width="200px" MinDate="01/01/1000"
                                                MaxDate="01/01/3000" DateInput-DateFormat="d/MMM/yyyy hh:mm tt" PopupDirection="BottomRight"
                                                EnableTheming="true">
                                                <Calendar ID="Calendar1" runat="server" ShowRowHeaders="true">
                                                    <SpecialDays>
                                                        <telerik:RadCalendarDay Repeatable="Today" />
                                                    </SpecialDays>
                                                </Calendar>
                                            </telerik:RadDateTimePicker>
                                            <%-- <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtToDate"
                                                ErrorMessage="Select a date!"></asp:RequiredFieldValidator>--%>
                                        </td>
                                    </tr>
                                    <tr align="center">
                                        <td align="center">
                                            <asp:Button ID="btnsubmit" runat="server" Class="buttonstyle" Text="Submit" Width="70px"
                                                OnClick="btnsubmit_Click" />
                                                 <asp:Button ID="Print" runat="server" Class="buttonstyle" Text="Print" Width="70px"
                                                OnClick="btnprint_Click" />
                                        </td>
                                        <td colspan="4">
                                            <asp:Label ID="lblMsg" runat="server" Text="" Width="300px" class="style2"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <table id="Table2" border="0" runat="server" align="center">
                                    <tr>
                                        <td>
                                            <div align="center">
                                                <cc1:StiWebViewer ID="StiWebViewer1" runat="server" RenderMode="AjaxWithCache" ScrollBarsMode="true"
                                                    Width="1000px" Height="500px" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
