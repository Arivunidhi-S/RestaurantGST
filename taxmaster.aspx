<%@ Page Language="C#" AutoEventWireup="true" CodeFile="taxmaster.aspx.cs" Inherits="taxmaster" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>RESTAURANT | Tax Master</title>
    <telerik:RadStyleSheetManager ID="RadStyleSheetManager1" runat="server" />
    <style type="text/css">
        body { font-family:Arial, Helvetica, Sans-Serif; font-size:12px; margin:0px 20px;}
        /* menu */
        #menu{ margin:0px; padding:0px; list-style:none; color:#fff; line-height:45px; display:inline-block; float:left; z-index:1000; }
        #menu a { color:#fff; text-decoration:none; }
        #menu > li {background:#172322 none repeat scroll 0 0; cursor:pointer; float:left; position:relative;padding:0px 10px;}
        #menu > li a:hover {color:#B0D730;}
        #menu .logo {background:; 
padding:0px; }
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
</head>
<script language="javascript" type="text/javascript">
    history.go(1); /* undo user navigation (ex: IE Back Button) */
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
    <form id="form1" runat="server">
    <div>
        <table border="0" cellpadding="2" cellspacing="2" align="center" width="1000px">
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
                                    <asp:Label class="labelstyle" ID="lblStatus" runat="server" ForeColor="Yellow" Font-Bold="true" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" style="width: 80%;">
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
                                <div id="Div10" runat="server" align="center" style="width: 1000px; height: 500px;
                                    background-color: White; overflow: auto;">
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
                                        text-align: center; font-weight: bold; height: 17px; background-image: url(Images/Untitled.jpg);
                                        color: Navy;">
                                        Tax Master
                                    </div>
                                    <telerik:RadGrid ID="RadGrid1" runat="server" AllowMultiRowEdit="false" OnNeedDataSource="RadGrid1_NeedDataSource"
                                        GridLines="Vertical" AllowPaging="True" OnItemCreated="RadGrid1_ItemCreated"
                                        OnItemDataBound="RadGrid1_ItemDataBound" PagerStyle-AlwaysVisible="true" PagerStyle-Position="Bottom"
                                        OnDeleteCommand="RadGrid1_DeleteCommand" AllowAutomaticUpdates="false" AllowAutomaticInserts="false"
                                        PagerStyle-Mode="NextPrevNumericAndAdvanced" AllowAutomaticDeletes="false" OnInsertCommand="RadGrid1_InsertCommand"
                                        AllowSorting="true" AllowFilteringByColumn="true" OnUpdateCommand="RadGrid1_UpdateCommand">
                                        <MasterTableView AutoGenerateColumns="false" DataKeyNames="ID" CommandItemDisplay="Top"
                                            CommandItemSettings-AddNewRecordText="Add New Tax Master">
                                            <PagerStyle Mode="NextPrevNumericAndAdvanced" />
                                            <Columns>
                                                <telerik:GridEditCommandColumn ButtonType="ImageButton">
                                                    <HeaderStyle Width="5%" />
                                                </telerik:GridEditCommandColumn>
                                                <telerik:GridBoundColumn DataField="ID" DataType="System.Int64" HeaderText="ID" ReadOnly="True"
                                                    SortExpression="ID" UniqueName="ID" AllowSorting="false" AllowFiltering="false"
                                                    Visible="false">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="name" DataType="System.String" HeaderText="Tax Name"
                                                    SortExpression="name" UniqueName="name">
                                                    <HeaderStyle Width="25%" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Percentage" DataType="System.String" HeaderText="Percentage"
                                                    SortExpression="Percentage" UniqueName="Percentage">
                                                    <HeaderStyle Width="25%" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridButtonColumn CommandName="Delete" UniqueName="DeleteColumn" ButtonType="ImageButton"
                                                    ConfirmText="Are you sure want to delete?">
                                                    <HeaderStyle Width="5%" />
                                                </telerik:GridButtonColumn>
                                            </Columns>
                                            <EditFormSettings EditFormType="Template">
                                                <EditColumn UniqueName="EditCommandColumn1">
                                                </EditColumn>
                                                <FormTemplate>
                                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                        <tr>
                                                            <td style="width: 20%;" align="left">
                                                                <b>
                                                                    <asp:Label ID="lblid" runat="server" Text='<%# Bind("id") %>' Visible="false" />
                                                                </b>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="height: 15px;">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 20%" align="left">
                                                                Tax Name:
                                                                <asp:Label ID="Label1" runat="server" Text="*" ForeColor="Red" />
                                                            </td>
                                                            <td align="left">
                                                                <telerik:RadTextBox Width="200px" ID="txttaxName" MaxLength="40" EmptyMessage="Enter Kitchen Name"
                                                                    ToolTip="Maximum Length: 40" runat="server" Text='<%# Bind("Name") %>' />
                                                                    <asp:Label ID="lbltaxname" Text='<%# Bind("Name") %>' runat="server" Visible="false" />
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ForeColor="Red"
                                                                    ControlToValidate="txttaxName" ErrorMessage="Mandatory" SetFocusOnError="True" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="height: 15px;">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                Tax Percentage:
                                                                <asp:Label ID="Label3" runat="server" Text="*" ForeColor="Red" />
                                                            </td>
                                                            <td align="left">
                                                                <telerik:RadNumericTextBox ID="txttax" Text='<%# Bind("Percentage") %>' MaxLength="10"
                                                                    Width="200px" ToolTip="Maximum Length: 10" MinValue="0" MaxValue="99999" Type="Number"
                                                                    runat="server">
                                                                    <NumberFormat GroupSeparator="" DecimalDigits="1" />
                                                                </telerik:RadNumericTextBox>
                                                                <asp:RequiredFieldValidator ID="RFmin" runat="server" ForeColor="Red" ControlToValidate="txttax"
                                                                    ErrorMessage="Mandatory" SetFocusOnError="True" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="height: 15px;">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                Applicable
                                                                <asp:Label ID="Label2" runat="server" Text="*" ForeColor="Red" />
                                                            </td>
                                                            <td align="left">
                                                                <telerik:RadComboBox ID="cboflag" runat="server" Height="60px" Width="200px"                                                                      DropDownWidth="300px" AppendDataBoundItems="True"
                                                                    Text='<%# Bind("flag") %>'>
                                                                    <Items>
                                                                        <telerik:RadComboBoxItem Text="--Select--" Value="0" ForeColor="Silver" />
                                                                        <telerik:RadComboBoxItem Text="Yes" Value="1" ForeColor="Black" />
                                                                        <telerik:RadComboBoxItem Text="No" Value="2" ForeColor="Black" />
                                                                    </Items>
                                                                </telerik:RadComboBox>
                                                                <asp:RequiredFieldValidator ID="RFcpy" runat="server" ControlToValidate="cboflag"
                                                                    ErrorMessage="Enter Y/N" SetFocusOnError="True"> 
                                                                </asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="height: 20px;">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Button ID="Button1" runat="server" Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>'
                                                                    CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>'>
                                                                </asp:Button>
                                                                <asp:Button ID="Button2" runat="server" Text="Cancel" CausesValidation="false" CommandName="Cancel">
                                                                </asp:Button>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </FormTemplate>
                                            </EditFormSettings>
                                        </MasterTableView>
                                        <PagerStyle Mode="NumericPages"></PagerStyle>
                                    </telerik:RadGrid>
                                    <asp:SqlDataSource ID="SqlDatarestname" runat="server" ConnectionString="<%$ ConnectionStrings:connString %>"
                                        SelectCommand="SELECT  [id], [name] FROM [restaurantmaster] where deleted=0 ORDER BY [id]">
                                    </asp:SqlDataSource>
                                </div>
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
