<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UsersGroup.aspx.cs" Inherits="UsersGroup" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
   <title>VIS | UserGroup Master</title>
   <style type="text/css">
        body { font-family:Arial, Helvetica, Sans-Serif; font-size:12px; margin:0px 20px;}
        /* menu */
        #menu{ margin:0px; padding:0px; list-style:none; color:#fff; line-height:45px; display:inline-block; float:left; z-index:1000; }
        #menu a { color:#fff; text-decoration:none; }
        #menu > li {background:#172322 none repeat scroll 0 0; cursor:pointer; float:left; position:relative;padding:0px 10px;z-index:1000;}
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


    
    <div align="center" style="margin-top:-75px;">
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
                            <%--<ul id="myMenu" class="nfMain nfPure">
                                <% for (int a = 0; a < dtMenuItems.Rows.Count; a++)
                                   { %>
                                <li class="nfItem"><a class="nfLink" href="#">
                                    <%= dtMenuItems.Rows[a][0].ToString()  %></a>
                                    <div class="nfSubC nfSubS">
                                        <% dtSubMenuItems = BusinessTier.getSubMenuItems(dtMenuItems.Rows[a][0].ToString(), Session["sesUserID"].ToString().Trim(), Session["sesUserType"].ToString().Trim());
                                           int aa;
                                           for (aa = 0; aa < dtSubMenuItems.Rows.Count; aa++)
                                           { %>
                                        <div class="nfItem">
                                            <a class="nfLink" id='<%= dtSubMenuItems.Rows[aa][0].ToString() %>' href='<%= dtSubMenuItems.Rows[aa][1].ToString() %>'>
                                                <%= dtSubMenuItems.Rows[aa][2].ToString()%></a>
                                        </div>
                                        <% } %>
                                    </div>
                                </li>
                                <% } %>
                                <li class="nfItem"><a class="nfLink" href="Login.aspx" style="border-right-width: 1px;">
                                    LOGOUT</a></li>
                            </ul>--%>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2" id="tdStatus" runat="server">
                            <div style="height: 20px;">
                                <asp:Label class="labelstyle" ID="lblStatus" runat="server" ForeColor="Red" Font-Bold="true" />
                            </div>
                        </td>
                    </tr>
                    <tr style="width: 100%; height: 400px;">
                        <td align="left" style="width: 100%; height: 500px;">
                            <telerik:RadScriptManager ID="ScriptManager" runat="server">
                                <%--<Scripts>
                                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
                                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
                                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js" />
                                </Scripts>--%>
                            </telerik:RadScriptManager>
                            <div id="DivHeader" runat="server" style="background-color: White; font-size: medium;
                                text-align: center; font-weight: bold; height: 17px; background-image: url(Images/Untitled.png);
                                color: Navy;">
                                USER GROUP DETAILS
                            </div>
                            <ajaxToolkit:TabContainer runat="server" ID="TabContainer1" Height="140px" ActiveTabIndex="1"
                                BackColor="#C0D7E8">
                                <ajaxToolkit:TabPanel BackColor="#C0D7E8" runat="server" ID="TabPanel1" HeaderText="Step 1: User Group Information">
                                    <ContentTemplate>
                                        <div class="text" style="background-color: #edf9fe; height: 140px; overflow: auto;">
                                            <table cellpadding="2" cellspacing="2" border="0" width="100%">
                                                <tr>
                                                    <td align="left" style="width: 20%">
                                                        <asp:Label runat="server" CssClass="labelstyle_1" ID="lblUserGroupId" AssociatedControlID="txtUserGroupID">UserGroup ID:</asp:Label>
                                                    </td>
                                                    <td align="left" style="width: 30%">
                                                        <telerik:RadTextBox ID="txtUserGroupID" CssClass="textInput" runat="server" Width="220px"
                                                            MaxLength="50" ToolTip="Max Length:50" LabelCssClass="" LabelWidth="64px">
                                                        </telerik:RadTextBox>
                                                        <asp:RequiredFieldValidator runat="server" ID="UserGroupValidator" ControlToValidate="txtUserGroupID"
                                                            ErrorMessage="UserGroupID is required" ForeColor="Red" Text="*"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td align="left" style="width: 15%">
                                                        <asp:Label runat="server" CssClass="labelstyle_1" ID="lblDesc" AssociatedControlID="txtDesc">Description:</asp:Label>
                                                    </td>
                                                    <td align="left" style="width: 35%">
                                                        <telerik:RadTextBox ID="txtDesc" CssClass="textInput" runat="server" Width="220px"
                                                            MaxLength="200" TextMode="MultiLine" Height="100px" LabelCssClass="" 
                                                            LabelWidth="64px" />
                                                        <asp:RequiredFieldValidator runat="server" ID="DescValidator" ControlToValidate="txtDesc"
                                                            ErrorMessage="Description Needed" ForeColor="Red" Text="*"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                            </table>
                                            <br />
                                        </div>
                                    </ContentTemplate>
                                </ajaxToolkit:TabPanel>
                                <%--<ajaxToolkit:TabPanel runat="server" BackColor="#C0D7E8" ID="TabPanel2" HeaderText="Step 2: Add Approvar"
                                    Enabled="True">
                                    <ContentTemplate>
                                        <div class="text" style="background-color: #edf9fe; overflow: auto;">
                                            <ul class="formList" id="newsletterOptions">
                                                <li class="lastListItem">
                                                    <telerik:RadListBox ID="listboxApprovar1" runat="server" Width="300px" Height="100px"
                                                        AllowTransfer="true" ButtonSettings-ShowTransferAll="false" TransferToID="listboxApprovar2"
                                                        AutoPostBackOnTransfer="true" AllowReorder="true" AutoPostBackOnReorder="true"
                                                        EnableDragAndDrop="true">
                                                    </telerik:RadListBox>
                                                    <telerik:RadListBox ID="listboxApprovar2" runat="server" Width="300px" Height="100px"
                                                        SelectionMode="Multiple" AllowReorder="true" AutoPostBackOnReorder="true" EnableDragAndDrop="true">
                                                    </telerik:RadListBox>
                                                </li>
                                            </ul>
                                        </div>
                                    </ContentTemplate>
                                </ajaxToolkit:TabPanel>--%>
                                <ajaxToolkit:TabPanel runat="server" BackColor="#C0D7E8" ID="TabPanel3" HeaderText="Step 2: Add Module Permission"
                                    Enabled="True">
                                    <ContentTemplate>
                                        <div class="text" style="background-color: #edf9fe; overflow:auto;">
                                            <ul class="formList" id="termsOfUse">
                                                <li class="lastListItem">
                                                    <telerik:RadListBox ID="listboxModule1" runat="server" Width="300px" Height="100px"
                                                        AllowTransfer="true" TransferToID="listboxModule2" AutoPostBackOnTransfer="true"
                                                        ButtonSettings-ShowTransferAll="false" AllowReorder="true" AutoPostBackOnReorder="true"
                                                        EnableDragAndDrop="true" DataTextField="ModuleName" DataValueField="ModuleId">
                                                    </telerik:RadListBox>
                                                    <telerik:RadListBox ID="listboxModule2" runat="server" Width="300px" Height="100px"
                                                        SelectionMode="Multiple" AllowReorder="true" AutoPostBackOnReorder="true" EnableDragAndDrop="true">
                                                    </telerik:RadListBox>
                                                </li>
                                            </ul>
                                        </div>
                                    </ContentTemplate>
                                </ajaxToolkit:TabPanel>
                            </ajaxToolkit:TabContainer>
                            <div class="text" style="background-color: #edf9fe; text-align: right;">
                                <asp:Button runat="server" ID="btnRegister" CssClass="buttonstyle" OnClick="btnRegister_Click"
                                    Text="Save" />
                                <asp:Button runat="server" ID="btnClearItem" CssClass="buttonstyle" Text="Clear"
                                    OnClick="btnClearItem_Click" />
                            </div>
                            <telerik:RadAjaxManager ID="RadAjaxManager10" runat="server">
                                <AjaxSettings>
                                    <telerik:AjaxSetting AjaxControlID="RadGrid1">
                                        <UpdatedControls>
                                            <telerik:AjaxUpdatedControl ControlID="lblStatus" />
                                            <telerik:AjaxUpdatedControl ControlID="tdStatus" />
                                        </UpdatedControls>
                                    </telerik:AjaxSetting>
                                    <telerik:AjaxSetting AjaxControlID="RadGrid1">
                                        <UpdatedControls>
                                            <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                                            <telerik:AjaxUpdatedControl ControlID="TabContainer1" />
                                            <telerik:AjaxUpdatedControl ControlID="lblStatus" />
                                        </UpdatedControls>
                                    </telerik:AjaxSetting>
                                </AjaxSettings>
                            </telerik:RadAjaxManager>
                            <telerik:RadInputManager ID="RadInputManager1" runat="server">
                                <telerik:RegExpTextBoxSetting BehaviorID="RagExpBehavior1" Validation-IsRequired="true"
                                    ValidationExpression="^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$" ErrorMessage="Invalid Email">
                                    <TargetControls>
                                        <telerik:TargetInput ControlID="txtEmail" />
                                    </TargetControls>
                                </telerik:RegExpTextBoxSetting>
                            </telerik:RadInputManager>
                            <div id="Div3" runat="server" style="color: Blue; background-image: url(Images/Untitled.png);
                                height: 190px; font-size: medium; text-align: left">
                                <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
                                <!-- content start -->
                                <div id="Div10" runat="server" style="width: 100%; height: 195px; background-color: White;
                                    overflow: auto;">
                                    <telerik:RadGrid ID="RadGrid1" runat="server" AllowMultiRowEdit="false" OnNeedDataSource="RadGrid1_NeedDataSource"
                                        GridLines="Vertical" AllowPaging="True" PagerStyle-AlwaysVisible="true" PagerStyle-Position="Bottom"
                                        OnDeleteCommand="RadGrid1_DeleteCommand" PagerStyle-Mode="NextPrevNumericAndAdvanced"
                                        AllowAutomaticDeletes="true" AllowSorting="true" OnItemCommand="RadGrid1_ItemCommand" Skin="Forest"
                                        AllowFilteringByColumn="true">
                                        <ClientSettings EnableRowHoverStyle="true" EnablePostBackOnRowClick="true">
                                            <Selecting AllowRowSelect="true" />
                                        </ClientSettings>
                                        <MasterTableView AutoGenerateColumns="false" AllowAutomaticUpdates="false" DataKeyNames="group_aid" InsertItemPageIndexAction="ShowItemOnFirstPage"
                                            CommandItemDisplay="Top" CommandItemSettings-AddNewRecordText="Add New UserGroup Details">
                                            <CommandItemSettings ShowAddNewRecordButton="false" />
                                            <PagerStyle Mode="NextPrevNumericAndAdvanced" />
                                            <Columns>
                                                <%--                                                <telerik:GridEditCommandColumn ButtonType="ImageButton" InsertText="Text"/>
                                                --%>
                                                <%--<telerik:GridBoundColumn DataField="group_aid" DataType="System.Int64" HeaderText="ID" ReadOnly="True"
                                                    SortExpression="group_aid" UniqueName="group_aid" AllowFiltering="false" AllowSorting="false"
                                                    Visible="false">
                                                </telerik:GridBoundColumn>--%>
                                                <telerik:GridHyperLinkColumn DataTextField="group_id" DataType="System.String" HeaderText="GroupID"
                                                    SortExpression="group_id" UniqueName="group_id">
                                                    <HeaderStyle Width="30%" />
                                                </telerik:GridHyperLinkColumn>
                                                <telerik:GridBoundColumn DataField="description" DataType="System.String" HeaderText="Description"
                                                    SortExpression="description" UniqueName="description">
                                                    <HeaderStyle Width="60%" />
                                                </telerik:GridBoundColumn>
                                                
                                                <telerik:GridButtonColumn CommandName="Delete" UniqueName="DeleteColumn" ButtonType="ImageButton"
                                                    ConfirmText="Are you sure want to delete?">
                                                    <HeaderStyle Width="10%" />
                                                </telerik:GridButtonColumn>
                                            </Columns>
                                            <EditFormSettings EditFormType="Template">
                                                <EditColumn UniqueName="EditCommandColumn1">
                                                </EditColumn>
                                            </EditFormSettings>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </div>
                            </div>
                        </td>
                         <%--<td align="center" valign="top" style="width: 20%; height: 400px">
                           <div id="Div2" runat="server" style="color: Navy; background-color: White; font-size: large;
                                font-weight: bold; height: 400px; background-image: url(Images/Untitled.png)">
                                HELP
                                <div id="Div1" runat="server" style="color: Navy; width: 270px; font-weight: lighter;
                                    font-size: small; text-align: left">
                                    <ul style="text-align: left;">
                                        <li>Click "Refresh" button to refresh the Database.</li><hr style="border-color: transparent;
                                            height: 1px;" />
                                        <li>Double Click the link to change existing details.</li><hr style="border-color: transparent" />
                                        <li>Click "Delete" button to delete existing details.</li><hr style="border-color: transparent" />
                                        <li>Click on "Filter" provided under each columns, to filter the information.</li><hr
                                            style="border-color: transparent" />
                                        <li>Click on "Next/Previous" or "First/Last" button to move to other pages.</li><hr
                                            style="border-color: transparent" />
                                        <li>Enter Page No and Click on "Go" button to goto particular page directly.</li><hr
                                            style="border-color: transparent" />
                                        <li>Enter the Number and Click on "Change" to change the number of items shown.</li>
                                    </ul>
                                </div>
                            </div>
                            <div id="Div4" runat="server" style="color: Navy; background-color: White; font-size: large;
                                font-weight: bold; height: 15px; background-image: url(Images/Untitled.png)">
                            </div>
                        </td>--%>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>