<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Users.aspx.cs" Inherits="Users" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
   <title>VIS System | User Master</title>
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
</head>
<script language="javascript" type="text/javascript">
    history.go(1); /* undo user navigation (ex: IE Back Button) */
</script>
<body background="images/BG.jpg">
 <div align="center" style="margin-top:-0px;"><img alt="" src="bar.png" width="100%" height="45px" /></div>
<div align="center" style="background-image:url('bar.png'); width:1000px; margin-top:-47px;">


<% 
        System.Data.DataTable dtItems = new System.Data.DataTable();
        System.Data.DataTable dtItems2 = new System.Data.DataTable();
        String user_group = Session["user_group"].ToString().Trim();
        System.Data.SqlClient.SqlConnection conn = BusinessTier.getConnection();
        System.Data.SqlClient.SqlConnection conn2 = BusinessTier.getConnection();
        conn.Open();
        conn2.Open();
        //System.Data.SqlClient.SqlDataReader reader;
        String sql = "select * from vw_ModulePermissionGroup where group_aid="+user_group.ToString().Trim()+" order by TabOrder";
        //System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql, conn);
        //reader = cmd.ExecuteReader();
        dtItems = new System.Data.DataTable();
        dtItems.Clear();
        
        System.Data.SqlClient.SqlDataAdapter sqlDataAdapter = new System.Data.SqlClient.SqlDataAdapter(sql, conn);
        sqlDataAdapter.Fill(dtItems);
        int rco = dtItems.Rows.Count;
       
 %>
         <ul id="menu">
            <li class="logo">
                <a href="http://www.e-serbadk.com/IT/contactus.html" target="_blank" style="border:none"><img style="float:left;" alt="" border="0" src="images/vimeo/menu_left.png"/></a>
                <ul id="main">
                    <li><a href="http://www.e-serbadk.com/IT/contactus.html"><font color="white"><b>Enquiry?</b></font></a></li>
                    <li class="last">
                        <img class="corner_left" alt="" src="images/vimeo/corner_blue_left.png"/>
                        <img class="middle" alt="" src="images/vimeo/dot_blue.png"/>
                        <img class="corner_right" alt="" src="images/vimeo/corner_blue_right.png"/>
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
 <li><a href="<%=dataRow["CodeFileName"].ToString()%>"><span><%= dataRow["ModuleName"].ToString()%></span></a>
                <ul id="Ul1">
 <%
                            foreach (System.Data.DataRow dataRow2 in dtItems2.Rows)
                        {
                            if ((dataRow2["ModuleName"].ToString() == "UserMap") || (dataRow2["ModuleName"].ToString() == "AdminMap"))
                            {%>
                             <li><a href="#" onclick="window.open('<%=dataRow2["CodeFileName"].ToString()%>'); return false;" target="_blank"><span><%= dataRow2["ModuleName"].ToString()%></span></a></li>


                               <%}
                            else
                            {%>

                                <li><a href="<%=dataRow2["CodeFileName"].ToString()%>"><span><%= dataRow2["ModuleName"].ToString()%></span></a></li>
 
 <%
     }
                        }
                        
 %>
 <li class="last">
                        <img class="corner_left" alt="" src="images/vimeo/corner_left.png"/>
                        <img class="middle" alt="" src="images/vimeo/dot.gif"/>
                        <img class="corner_right" alt="" src="images/vimeo/corner_right.png"/>
                    </li>
                </ul>
            </li>
 <%
                    }
                    else
                    {
 %>
 <li><a href="<%=dataRow["CodeFileName"].ToString()%>"><span><%= dataRow["ModuleName"].ToString()%></span></a></li>
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
           
            <li><a href="Login.aspx">Logout</a>
            </li>
			<li><a href="#">Help</a>
                <ul id="help">
                    <li>
                        <img class="corner_inset_left" alt="" src="images/vimeo/corner_inset_left.png"/>
                        <a href="#">Download Manual</a>
                        <img class="corner_inset_right" alt="" src="images/vimeo/corner_inset_right.png"/>
                    </li>
                    <li><a href="#">About</a></li>
                    <li class="last">
                        <img class="corner_left" alt="" src="images/vimeo/corner_left.png"/>
                        <img class="middle" alt="" src="images/vimeo/dot.gif"/>
                        <img class="corner_right" alt="" src="images/vimeo/corner_right.png"/>
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

   <br /><br /><br /><br />
<div align="center" style="margin-top:-50px;">
        <img src="Images/Banner.png" width="1000px" height="100px" alt="RSS"/>
</div>

    <div align="center" style="margin-top:-31px;">
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
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

                                <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1">
                                    <AjaxSettings>
                                        <telerik:AjaxSetting AjaxControlID="RadGrid1">
                                            <UpdatedControls>
                                                <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                                                <telerik:AjaxUpdatedControl ControlID="lblStatus" />
                                            </UpdatedControls>
                                        </telerik:AjaxSetting>
                                    </AjaxSettings>
                                </telerik:RadAjaxManager>
                                <telerik:RadInputManager ID="RadInputManager1" runat="server">
                                    <telerik:TextBoxSetting BehaviorID="TextBoxBehavior1" InitializeOnClient="false"
                                        Validation-IsRequired="true">
                                    </telerik:TextBoxSetting>
                                </telerik:RadInputManager>
                                <telerik:RadGrid ID="RadGrid1" runat="server" AllowMultiRowEdit="false" OnNeedDataSource="RadGrid1_NeedDataSource"
                                    GridLines="Vertical" AllowPaging="True" PagerStyle-AlwaysVisible="true" PagerStyle-Position="Bottom"
                                    OnItemDataBound="RadGrid1_ItemDataBound" OnDeleteCommand="RadGrid1_DeleteCommand"
                                    AllowAutomaticUpdates="true" AllowAutomaticInserts="true" PagerStyle-Mode="NextPrevNumericAndAdvanced"
                                    AllowAutomaticDeletes="true" OnInsertCommand="RadGrid1_InsertCommand" AllowSorting="true"
                                    AllowFilteringByColumn="true" OnUpdateCommand="RadGrid1_UpdateCommand">
                                  
                                    <MasterTableView AutoGenerateColumns="false" DataKeyNames="user_aid" CommandItemDisplay="Top" ClientDataKeyNames="user_aid" 
                                        CommandItemSettings-AddNewRecordText="Add New User Details" InsertItemPageIndexAction="ShowItemOnFirstPage">
                                        <PagerStyle Mode="NextPrevNumericAndAdvanced" />
                                        <Columns>
                                            <telerik:GridEditCommandColumn ButtonType="ImageButton" />
                                            <telerik:GridTemplateColumn DataField="user_aid" DataType="System.Int32" HeaderText="user id"
                                                                        SortExpression="user_aid" UniqueName="user_aid" Resizable="true" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" Visible="true" ID="lbluserid" Text='<%# Eval("user_aid") %>' />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Width="7%" />
                                                                    </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn DataField="user_aid" DataType="System.Int64" HeaderText="User Id"
                                                ReadOnly="True" SortExpression="user_aid" UniqueName="user_aid" AllowSorting="false"
                                                AllowFiltering="false" Visible="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="user_name" DataType="System.String" HeaderText="User Name"
                                                ReadOnly="True" SortExpression="user_name" UniqueName="user_name" AllowSorting="true"
                                                AllowFiltering="true" Visible="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="contact_name" DataType="System.String" HeaderText="Contact Name"
                                             ReadOnly="True" SortExpression="contact_name" UniqueName="contact_name" Visible="true">
                                            </telerik:GridBoundColumn>
                                             <telerik:GridBoundColumn DataField="department" DataType="System.String" HeaderText="Department"
                                                ReadOnly="True" SortExpression="department" UniqueName="department" AllowSorting="true"
                                                AllowFiltering="true" Visible="true">
                                            </telerik:GridBoundColumn>
                                             <telerik:GridBoundColumn DataField="designation" DataType="System.String" HeaderText="Designation"
                                                ReadOnly="True" SortExpression="designation" UniqueName="designation" AllowSorting="true"
                                                AllowFiltering="true" Visible="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="contact_no" DataType="System.String" HeaderText="Contact No"
                                             ReadOnly="True" SortExpression="contact_no" UniqueName="contact_no" Visible="true">
                                            </telerik:GridBoundColumn>
                                          
                                             <telerik:GridTemplateColumn HeaderText="Report Flag" AllowFiltering="false">
                                                                        <HeaderStyle Width="5%" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                        <ItemTemplate>
<%--                                                                            <asp:CheckBox runat="server" ID="ChkSelect" ForeColor="white" Font-Size="1" Checked='<%#Convert.ToBoolean(Eval("status")) %>' ToolTip='<%# Eval("Rpt_Flag") %>' />--%>  
                                                                           <asp:CheckBox runat="server" ID="ChkSelect" ForeColor="white" Font-Size="1" Checked='<%#Convert.ToBoolean(Eval("Rpt_Flag")) %>'  />

                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                            <%--<telerik:GridBoundColumn DataField="description" DataType="System.String" HeaderText="description"
                                                SortExpression="description" UniqueName="description">
                                            </telerik:GridBoundColumn>--%>
                                            <telerik:GridButtonColumn CommandName="Delete" UniqueName="DeleteColumn" ButtonType="ImageButton"
                                                ConfirmText="Are you sure want to delete?" />
                                        </Columns>
                                        <EditFormSettings EditFormType="Template">
                                            <EditColumn UniqueName="EditCommandColumn1">
                                            </EditColumn>
                                            <FormTemplate>
                                                <table cellspacing="2" cellpadding="1" width="100%" border="0">
                                                    <tr>
                                                        <td colspan="4" align="left">
                                                            <b><%--ID:--%>
                                                                <asp:Label ID="lblID" Visible="true" runat="server" Width="20px" Text='<%# Bind("user_aid")%>' />
                                                            </b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            Login ID:
                                                        </td>
                                                        <td align="left">
                                                            <telerik:RadTextBox Width="200px" ID="txtName" MaxLength="30" ToolTip="Maximum Length: 30"
                                                                runat="server" Text='<%# Bind("user_name") %>'>
                                                            </telerik:RadTextBox>
                                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtName"
                                                                ErrorMessage="Name is required" ForeColor="Red" Text="*"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td align="left">
                                                            Password:
                                                        </td>
                                                        <td align="left">
                                                            <telerik:RadTextBox Width="200px" ID="txtPass"  MaxLength="30"
                                                                ToolTip="Maximum Length: 30" runat="server" Text='<%# Bind("user_password") %>'>
                                                            </telerik:RadTextBox>
                                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtPass"
                                                                ErrorMessage="Password is required" ForeColor="Red" Text="*"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    
                                                    <tr>
                                                        <td align="left">
                                                            Group:
                                                        </td>
                                                        <td align="left">
                                                           <%-- <telerik:RadComboBox ID="cboGroup" runat="server" AppendDataBoundItems="True" Width="100px"
                                                                DataSource='<%# (new string[] { "Admin", "Purchase","Users"}) %>'>
                                                            </telerik:RadComboBox>--%>

                                                            <%--<telerik:RadComboBox ID="cboGroup" runat="server" Height="80px" Width="300px"
                                                DataSourceID="SqlDataSourceGroup"
                                                DataTextField="group_id" AutoPostBack="true" AppendDataBoundItems="True" DataValueField="group_aid" 
                                                Visible="true">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="---Select---" Value="0" ForeColor="Silver" />
                                                </Items>
                                            </telerik:RadComboBox>--%>


                                            <telerik:RadComboBox ID="cboGroup" runat="server" Height="160px" Width="300px"
                                                                            EnableLoadOnDemand="true" OnItemsRequested="cboGroup_OnItemsRequested" AppendDataBoundItems="True" HighlightTemplatedItems="true" 
                                                                            Text='<%# Bind("group_id") %>'>
                                                                            <HeaderTemplate>
                                                                                <table style="width: 300px" cellspacing="0" cellpadding="0">
                                                                                    <tr>
                                                                                        <td style="width: 140px;">
                                                                                            User Group
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
                                                                                       
                                                                                    </tr>
                                                                                </table>
                                                                            </ItemTemplate>
                                                                        </telerik:RadComboBox>

                                             <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="cboGroup"
                                                                ErrorMessage="GroupName is required" ForeColor="Red" Text="*"></asp:RequiredFieldValidator>

                                                        </td>
                                                        <td align="left">
                                                            Email:
                                                        </td>
                                                        <td align="left">
                                                            <telerik:RadTextBox Width="200px" ID="txtEmail" MaxLength="50" ToolTip="Maximum Length: 50"
                                                                runat="server" Text='<%# Bind("user_email") %>'>
                                                            </telerik:RadTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            Status:
                                                        </td>
                                                        <td align="left">
                                                            <telerik:RadComboBox ID="cboStatus" runat="server" AppendDataBoundItems="True" Width="100px"
                                                                DataSource='<%# (new string[] { "True", "False"}) %>'>
                                                            </telerik:RadComboBox>
                                                        </td>
                                                        <td align="left">
                                                            Contact:
                                                        </td>
                                                        <td align="left">
                                                            <telerik:RadComboBox ID="cboContact" runat="server" AppendDataBoundItems="True" Width="100px"
                                                                DataSource='<%# (new string[] { "False", "True"}) %>'>
                                                            </telerik:RadComboBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                           Contact Name:
                                                        </td>
                                                        <td align="left">
                                                            <telerik:RadTextBox Width="200px" ID="contact_name" MaxLength="50" ToolTip="Maximum Length: 50"
                                                                runat="server" Text='<%# Bind("contact_name") %>'>
                                                            </telerik:RadTextBox>
                                                                                                   <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ControlToValidate="contact_name"
                                                                ErrorMessage="GroupName is required" ForeColor="Red" Text="*"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td align="left">
                                                            Contact No:
                                                        </td>
                                                        <td align="left">
                                                            <telerik:RadTextBox Width="200px" ID="contact_no" MaxLength="50" ToolTip="Maximum Length: 50"
                                                                runat="server" Text='<%# Bind("contact_no") %>'>
                                                            </telerik:RadTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            Designation:
                                                        </td>
                                                        <td align="left">
                                                            <telerik:RadTextBox Width="200px" ID="designation" MaxLength="50" ToolTip="Maximum Length: 50"
                                                                runat="server" Text='<%# Bind("designation") %>'>
                                                            </telerik:RadTextBox>
                                                        </td>
                                                        
                                                        <td align="left">
                                                            Department:
                                                        </td>
                                                       <td align="left">
                                                            <telerik:RadTextBox Width="200px" ID="department" MaxLength="50" ToolTip="Maximum Length: 50"
                                                                runat="server" Text='<%# Bind("department") %>'>
                                                            </telerik:RadTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4" align="left">
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
                                <asp:Button runat="server" ID="btnDelivered" CssClass="buttonstyle" Text="Update" Visible="false"
                                                                    OnClick="btnDelivered_Click" />
                                <asp:SqlDataSource ID="SqlDataSourceGroup" runat="server" ConnectionString="<%$ ConnectionStrings:connString %>"
                                                SelectCommand="select group_aid,group_id from UserGroup_tbl where DELETED=0 order by group_id">
                                            </asp:SqlDataSource>
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
