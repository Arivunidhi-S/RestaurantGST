<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Cashier.aspx.cs" Inherits="Cashier" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Restaurant | Cashier</title>
    <telerik:RadStyleSheetManager ID="RadStyleSheetManager1" runat="server" />
    <style type="text/css">
        body { font-family:Arial, Helvetica, Sans-Serif; font-size:12px; margin:0px 20px;}
        /* menu */
    #menu{ margin:0px; padding:0px; list-style:none; color:#fff; line-height:45px; display:inline-block; float:left; z-index:1000; }
        #menu a { color:#fff; text-decoration:none; }
        #menu > li {background:#172322 none repeat scroll 0 0; cursor:pointer; float:left; position:relative;padding:0px 10px;z-index:1000;}
        #menu > li a:hover {color:#B0D730;}
        #menu .logo {background:transparent none repeat scroll 0% 0%; padding:0px; background-color:Transparent;}
        /* sub-menus*/
        #menu ul { padding:0px; margin:0px; display:block; display:inline;}
        #menu li ul { position:absolute; left:-10px; top:0px; margin-top:45px; width:150px; line-height:16px; background-color:#172322; color:#0395CC; /* for IE */ display:none; }
        #menu li:hover ul { display:block;}
        #menu li ul li{ display:block; margin:5px 20px; padding: 5px 0px;  border-top: dotted 1px #606060; list-style-type:none; }
        #menu li ul li:first-child { border-top: none; }
        #menu li ul li a { display:block; color:#0395CC; }
        #menu li ul li a:hover { color:#7FCDFE; }
        /* main submenu */
        <%--#menu #main { left:0px; top:-20px; padding-top:20px; background-color:#7cb7e3; color:#fff; z-index:999;}--%>
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
    <style type="text/css">
        .exampleWrapper
        {
            width: 739px;
            height: 404px;
            background: transparent url(images/background.png) no-repeat top left;
            position: relative;
        }
        .tabStrip
        {
            position: absolute;
            top: 190px;
            left: 180px;
            height: 150px;
        }
        .multiPage
        {
            position: absolute;
            top: 250px;
            left: 180px;
            color: Black;
        }
        
        .multiPage p
        {
            margin-left: 40px;
        }
        
        .multiPage ul
        {
            list-style: none;
            width: 520px;
            border-bottom: 1px solid #8d9396;
        }
        .multiPage label
        {
            float: left;
            width: 120px;
            padding-left: 16px;
        }
        
        .multiPage li
        {
            border-top: 1px solid #8d9396;
            line-height: 23px;
        }
        .multiPage .pageViewEducation label
        {
            width: 220px;
        }
        .tabStrip .rtsLI, .tabStrip .rtsLink
        {
            line-height: 50px !important;
            text-decoration: none !important;
            background-color: Black;
        }
    </style>
</head>
<script language="javascript" type="text/javascript">
    history.go(1); /* undo user navigation (ex: IE Back Button) */
    function ShowEditForm(rowIndex) {
        window.radopen("Calculate.aspx", "UserListDialog");
        return false;
    }

    function OnClientPageLoad(sender, eventArgs) {
        setTimeout(function () { sender.set_status(""); }, 0);
    }

    function RefreshParentPage()//function in parent page
    {

        document.location.reload();

    }
</script>
<body background="images/BG.jpg">
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
    <div align="center" style="margin-top: -31px;">
        <form id="form1" runat="server">
        <table border="0" cellpadding="2" cellspacing="2" width="1000px">
            <tr>
                <td style="border-right: blue thin solid; border-top: blue thin solid; border-left: blue thin solid;
                    border-bottom: blue thin solid; border-width: 0px" align="center">
                    <hr style="visibility: hidden;" />
                    <hr style="visibility: hidden;" />
                    <table border="0" width="100%">
                        <tr>
                            <td id="Td1" align="left" runat="server" colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <div style="height: 20px;">
                                    <asp:Label class="labelstyle" ID="lblStatus" runat="server" ForeColor="Red" Font-Bold="true" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 80%;">
                                <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
                                    <Scripts>
                                        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
                                        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
                                        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js" />
                                    </Scripts>
                                </telerik:RadScriptManager>
                                <script type="text/javascript">
                                </script>
                                <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1">
                                    <AjaxSettings>
                                        <telerik:AjaxSetting AjaxControlID="RadGrid1">
                                            <UpdatedControls>
                                                <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                                                <telerik:AjaxUpdatedControl ControlID="RadInputManager1" />
                                                <telerik:AjaxUpdatedControl ControlID="lblStatus" />
                                            </UpdatedControls>
                                        </telerik:AjaxSetting>
                                    </AjaxSettings>
                                </telerik:RadAjaxManager>
                                <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
                                <div id="Div10" runat="server" style="width: 1000px; height: 400px; border: 5 background-color: White; 
                                    overflow: auto;">
                                    <telerik:RadInputManager ID="RadInputManager1" runat="server">
                                        <telerik:TextBoxSetting BehaviorID="TextBoxBehavior1" InitializeOnClient="false"
                                            Validation-IsRequired="true">
                                        </telerik:TextBoxSetting>
                                        <telerik:RegExpTextBoxSetting BehaviorID="RagExpBehavior1" ValidationExpression="[Y,y,N,n]{1}"
                                            IsRequiredFields="true" Validation-IsRequired="true">
                                            <TargetControls>
                                                <telerik:TargetInput ControlID="txtApprovalflag" />
                                            </TargetControls>
                                        </telerik:RegExpTextBoxSetting>
                                    </telerik:RadInputManager>
                                    <div id="DivHeader" runat="server" style="background-color: White; font-size: medium;
                                        text-align: center; font-weight: bold; height: 15px; background-image: url(Images/Untitled.jpg);">
                                        Date
                                        <telerik:RadDatePicker ID="DatePickerselect" runat="server" AutoPostBack="true" OnSelectedDateChanged="DatePickerselect_SelectedDateChanged"
                                            MinDate="01/01/1000" MaxDate="01/01/3000" DateInput-DateFormat="dd/MMM/yyyy">
                                            <Calendar ID="Calendar1" runat="server">
                                                <SpecialDays>
                                                    <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday" />
                                                </SpecialDays>
                                            </Calendar>
                                        </telerik:RadDatePicker>
                                        <asp:Panel ID="Panel1" runat="server">
                                            <asp:Timer ID="Timer1" runat="server" Interval="15000" OnTick="Timer1_Tick" />
                                            <%--       <telerik:RadAjaxTimer ID="Timer1" runat="server" OnTick="Timer1_Tick" Interval="1000"/> --%>
                                        </asp:Panel>
                                    </div>
                                    <div id="Div2" runat="server" style="background-color: White; font-size: medium;
                                        text-align: center; font-weight: bold; height: 10px; background-image: url(Images/Untitled.jpg);
                                        color: Black;">
                                        <telerik:RadTabStrip ID="RadTabStrip1" runat="server" Skin="Black" MultiPageID="RadMultiPage1"
                                            Height="50PX" SelectedIndex="0" CssClass="tabStrip">
                                            <Tabs>
                                                <telerik:RadTab Text="Receive Receipt" Width="200PX" Font-Size="Large">
                                                </telerik:RadTab>
                                                <telerik:RadTab Text="Void Bill" Width="200PX" Font-Size="Large">
                                                </telerik:RadTab>
                                                <telerik:RadTab Text="Change Table" Width="200PX" Font-Size="Large">
                                                </telerik:RadTab>
                                                <telerik:RadTab Text="Combine Table" Width="200PX" Font-Size="Large">
                                                </telerik:RadTab>
                                                <telerik:RadTab Text="" Width="1PX" Visible="true">
                                                </telerik:RadTab>
                                            </Tabs>
                                        </telerik:RadTabStrip>
                                    </div>
                                    <telerik:RadMultiPage ID="RadMultiPage1" runat="server" SelectedIndex="0" CssClass="multiPage">
                                        <telerik:RadPageView ID="RadPageView1" runat="server" align="Center" BackColor="Red">
                                            <table cellpadding="0" width="100%" cellspacing="0" border="0" align="center">
                                                <tr>
                                                    <td align="left" style="width: 25%;">
                                                        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="false"
                                                            DestroyOnClose="false" EnableViewState="True" OffsetElementID="Div2" AutoSize="false">
                                                            <Windows>
                                                                <telerik:RadWindow ID="UserListDialog" Behaviors="Close" runat="server" Title="Calculate"
                                                                    NavigateUrl="Calculate.aspx" Height="450px" Width="800px" ReloadOnShow="false"
                                                                    ShowContentDuringLoad="false" Modal="true" AutoSize="false" DestroyOnClose="false" />
                                                            </Windows>
                                                        </telerik:RadWindowManager>
                                                        <telerik:GridDropDownListColumnEditor ID="ddl1" runat="server" />
                                                    </td>
                                                    <td align="left" style="width: 25%;">
                                                        <asp:Panel ID="Tablepanel" runat="server" Height="350px" EnableTheming="true">
                                                        </asp:Panel>
                                                    </td>
                                                    <td align="left" style="width: 25%;">
                                                        <asp:Panel ID="Tablepanel1" runat="server" Height="350px" EnableTheming="true">
                                                        </asp:Panel>
                                                    </td>
                                                    <td align="left" style="width: 25%;">
                                                        <asp:Panel ID="Tablepanel2" runat="server" Height="350px" EnableTheming="true">
                                                        </asp:Panel>
                                                    </td>
                                                    <td align="left" style="width: 25%;">
                                                        <asp:Panel ID="Tablepanel3" runat="server" Height="350px" EnableTheming="true">
                                                        </asp:Panel>
                                                    </td>
                                                    <td align="left" style="width: 25%;">
                                                        <asp:Panel ID="Tablepanel4" runat="server" Height="350px" EnableTheming="true">
                                                        </asp:Panel>
                                                    </td>
                                                    <td align="left" style="width: 25%;">
                                                        <asp:Panel ID="Tablepanel5" runat="server" Height="350px" EnableTheming="true">
                                                        </asp:Panel>
                                                    </td>
                                                    <td align="left" style="width: 25%;">
                                                        <asp:Panel ID="Tablepanel6" runat="server" Height="350px" EnableTheming="true">
                                                        </asp:Panel>
                                                    </td>
                                                    <td align="left" style="width: 25%;">
                                                        <asp:Panel ID="Tablepanel7" runat="server" Height="350px" EnableTheming="true">
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </telerik:RadPageView>
                                        <telerik:RadPageView ID="RadPageView2" runat="server" CssClass="pageViewEducation">
                                            <asp:Label ID="Label5" Text="SELECT TABLE :" runat="server" Font-Size="Large"></asp:Label>
                                            <%--        <telerik:RadComboBox ID="cmbTable" runat="server" DataValueField="Table_ID" DataSourceID="MyDataSource1"
                                                AutoPostBack="true" DataTextField="TableNo" DataField="Table_ID" OnSelectedIndexChanged="cmbTable_OnSelectedIndexChanged">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="--Select--" ForeColor="Silver" />
                                                </Items>
                                            </telerik:RadComboBox>--%>
                                            <telerik:RadComboBox ID="cmbTable" runat="server" DataValueField="Table_ID" AutoPostBack="true"
                                                DataTextField="TableNo" OnItemsRequested="cmbSourceTable_OnItemsRequested" OnSelectedIndexChanged="cmbTable_OnSelectedIndexChanged"
                                                Height="200" Text='<%# Bind("TableNo") %>' EnableLoadOnDemand="true" AppendDataBoundItems="True">
                                                <HeaderTemplate>
                                                    <table style="width: 300px" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td style="width: 160px;">
                                                            </td>
                                                            <td style="width: 140px;">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table style="width: 300px" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td style="width: 160px;" align="left">
                                                                <%# DataBinder.Eval(Container, "Text")%>
                                                            </td>
                                                            <td style="width: 140px;" align="left">
                                                                <%# DataBinder.Eval(Container, "Attributes['TableNo']")%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </telerik:RadComboBox>
                                            <asp:Label ID="Label6" Text="QTY :" runat="server" Font-Size="Large"></asp:Label>
                                            <%--      <telerik:RadComboBox ID="cmbQuantity" runat="server" AutoPostBack="true"   OnSelectedIndexChanged="cmbQuantity_SelectionChanged">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="--Select--" ForeColor="Silver" />
                                                </Items>
                                                </telerik:RadComboBox>--%>
                                            <telerik:RadTextBox runat="server" ID="txtQty">
                                            </telerik:RadTextBox>
                                            <telerik:RadGrid ID="RadGridvoidbill" runat="server" AllowMultiRowEdit="false" OnNeedDataSource="RadGridvoidbill_NeedDataSource"
                                                GridLines="Vertical" AllowPaging="True" PagerStyle-AlwaysVisible="true" PagerStyle-Position="Bottom"
                                                Skin="Black" OnDeleteCommand="RadGridvoidbill_DeleteCommand" AllowAutomaticUpdates="true"
                                                AllowAutomaticInserts="true" PagerStyle-Mode="NextPrevNumericAndAdvanced" AllowAutomaticDeletes="true"
                                                AllowSorting="true" AllowFilteringByColumn="true" EditMode="Popup">
                                                <MasterTableView AutoGenerateColumns="false" DataKeyNames="DetailID" CommandItemDisplay="Top"
                                                    EditMode="EditForms" InsertItemPageIndexAction="ShowItemOnFirstPage">
                                                    <PagerStyle Mode="NextPrevNumericAndAdvanced" />
                                                    <Columns>
                                                        <telerik:GridBoundColumn DataField="DetailID" DataType="System.Int64" HeaderText="ID"
                                                            ReadOnly="True" SortExpression="DetailID" UniqueName="DetailID" AllowSorting="false"
                                                            AllowFiltering="false" Visible="false">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Name" DataType="System.String" HeaderText="Name"
                                                            SortExpression="Name" UniqueName="Name">
                                                            <HeaderStyle Width="18%" />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Qty" DataType="System.String" HeaderText="Qty"
                                                            SortExpression="Qty" UniqueName="Qty">
                                                            <HeaderStyle Width="15%" />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Price" DataType="System.String" HeaderText="Price"
                                                            AllowFiltering="false" SortExpression="Price" UniqueName="Price">
                                                            <HeaderStyle Width="10%" />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridButtonColumn CommandName="Delete" UniqueName="DeleteColumn" ButtonType="ImageButton"
                                                            ConfirmText="Are you sure want to delete?">
                                                            <HeaderStyle Width="5%" />
                                                        </telerik:GridButtonColumn>
                                                    </Columns>
                                                    <EditFormSettings EditFormType="Template">
                                                        <EditColumn UniqueName="EditCommandColumn1">
                                                        </EditColumn>
                                                    </EditFormSettings>
                                                </MasterTableView>
                                                <PagerStyle Mode="NumericPages"></PagerStyle>
                                            </telerik:RadGrid>
                                            <%--    <asp:Button runat="server" Height="50" Width="100" margin="10,10,10,10" OnClick="btnVoid_Click"
                                                Text="VOID" BorderBrush="#83FFFFFF" />--%>
                                            <%--                                            <Button x:Name="btnVoid" Height="50" Width="100" Margin="10,10,10,10" Visibility="Visible"  FontWeight="Bold" FontSize="16" Style="{DynamicResource RoundedButton}" BorderBrush="#83FFFFFF" onclick="btnVoid_Click">
                                            --%>
                                            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$  ConnectionStrings:connString %>"
                                                SelectCommand="select DetailID,Name,Qty,Price FROM VW_OrderDetailMenu where Status='P'">
                                            </asp:SqlDataSource>
                                            <asp:SqlDataSource ID="MyDataSource1" runat="server" ConnectionString="<%$  ConnectionStrings:connString %>"
                                                SelectCommand="select Table_ID,TableNO FROM VW_TableOrderMaster where Status='P' and deleted=0 group by Table_ID,TableNO ">
                                            </asp:SqlDataSource>
                                        </telerik:RadPageView>
                                        <telerik:RadPageView ID="RadPageView3" runat="server" BackColor="Red">
                                            <table id="maitable" runat="server" align="left" border="0" style="width: 100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label2" Text=" Scource Table :" runat="server" Font-Size="Large"></asp:Label>
                                                        <telerik:RadComboBox ID="cmbSourceTable" runat="server" DataValueField="Table_ID"
                                                            AutoPostBack="true" DataTextField="TableNo" OnItemsRequested="cmbSourceTable_OnItemsRequested"
                                                            OnSelectedIndexChanged="cmbSourceTable_SelectionChanged" Height="200" Text='<%# Bind("TableNo") %>'
                                                            EnableLoadOnDemand="true" AppendDataBoundItems="True">
                                                            <HeaderTemplate>
                                                                <table style="width: 300px" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td style="width: 160px;">
                                                                        </td>
                                                                        <td style="width: 140px;">
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table style="width: 300px" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td style="width: 160px;" align="left">
                                                                            <%# DataBinder.Eval(Container, "Text")%>
                                                                        </td>
                                                                        <td style="width: 140px;" align="left">
                                                                            <%# DataBinder.Eval(Container, "Attributes['TableNo']")%>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </telerik:RadComboBox>
                                                        <%--      <telerik:RadComboBox ID="cmbSourceTable" runat="server" DataValueField="Table_ID"
                                                            DataSourceID="MyDataSource1" AutoPostBack="true" DataTextField="TableNo" DataField="Table_ID"
                                                            OnSelectedIndexChanged="cmbSourceTable_SelectionChanged">
                                                            <Items>
                                                                <telerik:RadComboBoxItem Text="--Select--" ForeColor="Silver" />
                                                            </Items>
                                                        </telerik:RadComboBox>--%>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label1" Text="Target Table :" runat="server" Font-Size="Large"></asp:Label>
                                                        <telerik:RadComboBox ID="cmbTargetTable" runat="server" DataValueField="Table_ID"
                                                            DataSourceID="SqlDataSourcelist" AutoPostBack="true" DataTextField="TableNo"
                                                            DataField="Table_ID" OnSelectedIndexChanged="cmbTargetTable_SelectionChanged">
                                                            <Items>
                                                                <telerik:RadComboBoxItem Text="--Select--" ForeColor="Silver" />
                                                            </Items>
                                                        </telerik:RadComboBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="width: 50%">
                                                        <asp:ListBox runat="server" ID="lstSource" Width="400" Height="270" Margin="10,10,10,0"
                                                            SelectionMode="Multiple"></asp:ListBox>
                                                    </td>
                                                    <td style="width: 50%" align="left">
                                                        <asp:ListBox runat="server" ID="lstTarget" Width="400" Height="270" Margin="10,10,10,0"
                                                            SelectionMode="Multiple"></asp:ListBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnChange" runat="server" Height="50" Width="100" margin="10,10,10,10"
                                                            OnClick="btnChange_Click" Text="Change" BorderBrush="#83FFFFFF" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:SqlDataSource ID="SqlDataSourcelist" runat="server" ConnectionString="<%$  ConnectionStrings:connString %>"
                                                SelectCommand="select Table_ID,TableNO FROM TableMaster where deleted=0"></asp:SqlDataSource>
                                        </telerik:RadPageView>
                                        <telerik:RadPageView ID="RadPageView4" runat="server" BackColor="Red">
                                            <table id="Table1" runat="server" align="left" border="0" style="width: 100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label3" Text=" Scource Table :" runat="server" Font-Size="Large"></asp:Label>
                                                        <telerik:RadComboBox ID="cmbSourceTablecombine" runat="server" DataValueField="Table_ID"
                                                            AutoPostBack="true" DataTextField="TableNo" OnItemsRequested="cmbSourceTable_OnItemsRequested"
                                                            OnSelectedIndexChanged="cmbSourceTablecombine_SelectionChanged" Height="200"
                                                            Text='<%# Bind("TableNo") %>' EnableLoadOnDemand="true" AppendDataBoundItems="True">
                                                            <HeaderTemplate>
                                                                <table style="width: 300px" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td style="width: 160px;">
                                                                        </td>
                                                                        <td style="width: 140px;">
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table style="width: 300px" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td style="width: 160px;" align="left">
                                                                            <%# DataBinder.Eval(Container, "Text")%>
                                                                        </td>
                                                                        <td style="width: 140px;" align="left">
                                                                            <%# DataBinder.Eval(Container, "Attributes['TableNo']")%>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </telerik:RadComboBox>
                                                        <%--     <telerik:RadComboBox ID="cmbSourceTablecombine" runat="server" DataValueField="Table_ID"
                                                            Height="50" Width="100" margin="10,10,10,10" Font-Size="Large" DataSourceID="MyDataSource1"
                                                            AutoPostBack="true" DataTextField="TableNo" DataField="Table_ID" dropdownHeight="200px"
                                                            OnSelectedIndexChanged="cmbSourceTablecombine_SelectionChanged">
                                                            <Items>
                                                                <telerik:RadComboBoxItem Text="--Select--" ForeColor="Silver" />
                                                            </Items>
                                                        </telerik:RadComboBox>--%>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label4" Text="Target Table :" runat="server" Font-Size="Large"></asp:Label>
                                                        <telerik:RadComboBox ID="cmbTargetTablecombine" runat="server" DataValueField="Table_ID"
                                                            DataSourceID="SqlDataSourcelist" AutoPostBack="true" DataTextField="TableNo"
                                                            DataField="Table_ID" OnSelectedIndexChanged="cmbTargetTablecombine_SelectionChanged">
                                                            <Items>
                                                                <telerik:RadComboBoxItem Text="--Select--" ForeColor="Silver" />
                                                            </Items>
                                                        </telerik:RadComboBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="width: 50%">
                                                        <asp:ListBox runat="server" ID="lstSourcecombine" Width="400" Height="270" Margin="10,10,10,0"
                                                            SelectionMode="Multiple"></asp:ListBox>
                                                    </td>
                                                    <td style="width: 50%" align="left">
                                                        <asp:ListBox runat="server" ID="lstTargetcombine" Width="400" Height="270" Margin="10,10,10,0"
                                                            SelectionMode="Multiple"></asp:ListBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnCombine" runat="server" Height="50" Width="100" margin="10,10,10,10"
                                                            OnClick="btnCombine_Click" Text="Combine" BorderBrush="#83FFFFFF" />
                                                    <%--        <asp:Button ID="btnEmail1" runat="server" Class="buttonstyle" Text="Click" Width="70px"
                                            OnClick="btnEmail1_Click" Visible="true" />--%>
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$  ConnectionStrings:connString %>"
                                                SelectCommand="select Table_ID,TableNO FROM TableMaster where deleted=0"></asp:SqlDataSource>
                                        </telerik:RadPageView>
                                    </telerik:RadMultiPage>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        </form>
    </div>
</body>
</html>
