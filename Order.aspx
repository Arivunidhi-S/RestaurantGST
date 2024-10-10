<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Order.aspx.cs" Inherits="Order" %>
<%@ Import Namespace="System.IO" %>
<%@ Register Assembly="Stimulsoft.Report.Web, Version=2011.2.1100.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a"
    Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<%--<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">--%>
<html>
<head>
    <title>Order</title>
   <%-- <telerik:RadStyleSheetManager ID="RadStyleSheetManager1" runat="server" />--%>
    <link rel="stylesheet" type="text/css" href="themes/default/easyui.css"/>
	<%--<link rel="stylesheet" type="text/css" href="themes/icon.css"/>--%>
	<script type="text/javascript" src="js/jquery.min.js"></script>
	<script type="text/javascript" src="js/jquery.easyui.min.js"></script>
   <%-- <script type="text/javascript" src="js/jquery.js"></script>--%>
    <%--<script type="text/javascript" src="js/new_jquery.galleriffic.js"></script>--%>
    <%--<link rel="stylesheet" href="css/galleriffic-5.css" type="text/css" />--%>
    <%--<script type="text/javascript" src="js/gallerificPlus.js"></script>
    
    <link rel="stylesheet" href="css/gallerificPlus.css" type="text/css" />--%>
    <script type="text/javascript">
        var data = { "total": 0, "rows": [] };
        var totalCost = 0;
        var totalCostRoundOff = 0;
        
        function roundNumber(num, dec) {
            var result = Math.round(num * Math.pow(10, dec)) / Math.pow(10, dec);
            return result;
        }
//        function roundToFiveCents(v) {

//            // Turn dollars into cents and convert to number
//            var len;
//            v = v * 100;

//            // Get residual cents
//            var r = v % 5;

//            // Round to 5c
//            if (r) {
//                v -= (r == 1 || r == 2) ? r : r - 5;
//            }

//            // Convert to string
//            v = v + '';
//            len = v.length - 2;

//            // Return formatted string
//            return v.substring(0, len) + '.' + v.substring(len);
//        }

        function submitOrder() {

            PageMethods.postOrder(data.rows, MyMethod_Result);
            //PageMethods.postOrder(data.rows, MyMethod_Result);
            //PageMethods.postOrder(data.rows[0].name, MyMethod_Result);

            //alert("submitting order");
        }

//        function AddQty(rid) {
//            alert(rid+" 1");
//        }
//        function ReduceQty(rid) {
//            alert(rid+" 2");
//        }

        function AddQty(ID) {
            var price = 0;
            function add() {
                
                for (var i = 0; i < data.total; i++) {
                    var row = data.rows[i];
                    if (row.ID == ID) {
                        row.quantity += 1;
                        price = row.price;
                        return;
                    }
                }
                data.total += 1;
                //                data.rows.push({
                //                    ID: ID,
                //                    name: name,
                //                    quantity: 1,
                //                    price: price
                //                });
            }
            add();
            totalCost += price;
            totalCost = roundNumber(totalCost, 2);
            //            $('div.cart .total').html('Total: RM ' + totalCost) = roundToFiveCents(totalCost);
            $('#cartcontent').datagrid('loadData', data);
            $('div.cart .total').html('Total: RM ' + totalCost);
            //            $('div.cart .totalro').html('Total: RM ' + totalCostRoundOff);
        }

        function ReduceQty(ID) {
            var price = 0;
            function remove() {
                for (var i = 0; i < data.total; i++) {
                    var row = data.rows[i];
                    if (row.ID == ID) {

                        if (row.quantity == 1) {
                            //row.quantity -= 1;
                            data.total -= 1;
                            data.rows.pop();
                        }
                        else {
                            row.quantity -= 1;
                        }
                        price = row.price;
                        return;
                    }
                }
                //data.total -= 1;
                //data.rows.pop();

            }
            remove();
            totalCost -= price;
            totalCost = roundNumber(totalCost, 2);
            $('#cartcontent').datagrid('loadData', data);
            $('div.cart .total').html('Total: RM ' + totalCost);
        }

        function MyMethod_Result(ResultString) {
            //alert("Order has been submitted");
            //sval = ResultString;
            //return ResultString;
        }

        $(function () {
            //            $('#cartcontent').datagrid({
            //                singleSelect: true
            //            });

            $('#cartcontent').datagrid({
                singleSelect: true,
                remoteSort: false,
                fitcolumns: true,
                nowrap: false,
                columns: [[
                        { field: 'ID', title: 'ID', sortable: true },
                        { field: 'name', title: 'Name', sortable: true },
                        { field: 'quantity', title: 'Qty', sortable: true },
                        { field: 'price', title: 'Price', sortable: true },
                        
                        { field: 'action2', title: '-', formatter: function (value, row, index) {
                            var col2;
                            if (row.ID != null) {
                                col2 = '<input type="button" id="RedBtn' + row.ID + '"  onclick="ReduceQty(' + row.ID + ')" value="-" class="GridButton"/>';
                                btnid = "RedBtn" + row.ID; //set to a global for getting out grid
                            }
                            return col2;
                        }
                        },
                        { field: 'action1', title: '+', formatter: function (value, row, index) {
                            var col1;
                            if (row.ID != null) {
                                col1 = '<input type="button" id="AddBtn' + row.ID + '"  onclick="AddQty(' + row.ID + ')" value="+" class="GridButton"/>';
                                btnid = "AddBtn" + row.ID; //set to a global for getting out grid
                            }
                            return col1;
                        } 
                        }
                    ]]
            });

            $('.item').click(function (e, source) {
                var name = $(this).find('p:eq(1)').html();
                var price = $(this).find('p:eq(2)').html();
                var ID = $(this).find('p:eq(0)').html();
                addProduct(name, parseFloat(price.split('RM')[1]), ID);
            });





            //            $('.item').draggable({
            //                revert: true,
            //                proxy: 'clone',
            //                onStartDrag: function () {
            //                    $(this).draggable('options').cursor = 'not-allowed';
            //                    $(this).draggable('proxy').css('z-index', 10);
            //                },
            //                onStopDrag: function () {
            //                    $(this).draggable('options').cursor = 'move';
            //                }
            //            });
            //            $('.cart').droppable({
            //                onDragEnter: function (e, source) {
            //                    $(source).draggable('options').cursor = 'auto';
            //                },
            //                onDragLeave: function (e, source) {
            //                    $(source).draggable('options').cursor = 'not-allowed';

            //                },
            //                onDrop: function (e, source) {
            //                    var name = $(source).find('p:eq(1)').html();
            //                    var price = $(source).find('p:eq(2)').html();
            //                    var ID = $(source).find('p:eq(0)').html();
            //                    addProduct(name, parseFloat(price.split('RM')[1]), ID);
            //                }

            //            });
        });

        function addProduct(name, price,ID) {
            function add() {
                for (var i = 0; i < data.total; i++) {
                    var row = data.rows[i];
                    if (row.name == name) {
                        row.ID = ID;
                        row.quantity += 1;
                        return;
                    }
                }
                data.total += 1;
                data.rows.push({
                    ID:ID,
                    name: name,
                    quantity: 1,
                    price: price
                });
            }
            add();
            totalCost += price;
            totalCost = roundNumber(totalCost, 2);
//            $('div.cart .total').html('Total: RM ' + totalCost) = roundToFiveCents(totalCost);
            $('#cartcontent').datagrid('loadData', data);
            $('div.cart .total').html('Total: RM ' + totalCost);
//            $('div.cart .totalro').html('Total: RM ' + totalCostRoundOff);
        }

        
        
	</script>


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
		.products{
			list-style:none;
			margin-right:300px;
			padding:0px;
			width:800px;
			
		}
		.products li{
			display:inline;
			float:left;
			margin:10px;
			color:White;
			width:140px;
		}
		.item{
			display:block;
			text-decoration:none;
			width:140px;
		}
		.item img{
			border:1px solid #333;
			width:140px;
		}
		.item p{
			margin:0;
			font-weight:bold;
			text-align:center;
			width:140px;
			color:#CC0000 ;/*White	#CC0000		color:#c3c3c3;*/

		}
		.cart{
			position:fixed;/*relative fixed*/
			right:0;
			top:0;
			width:300px;
			height:100%;
			/*background:#ccc;*/
			 
			/*background: Orange;*/
			background:Orange;
			padding:0px 0px;
		}
		/*h1{
			text-align:center;
			color:#555;
		}
		h2{
			position:absolute;
			font-size:16px;
			left:10px;
			bottom:20px;
			color:#555;
		}*/
		.total{
			margin:0;
			text-align:right;
			padding-right:23px;
			color:White;
		}
	</style>
<%--    <style type="text/css">
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
    </style>--%>
</head>
<%--<script language="javascript" type="text/javascript">
    history.go(1); /* undo user navigation (ex: IE Back Button) */
    function ShowEditForm(rowIndex) {
        window.radopen("Calculate.aspx", "UserListDialog");
        return false;
    }

    function OnClientPageLoad(sender, eventArgs) {
        setTimeout(function () { sender.set_status(""); }, 0);
    }
</script>--%>
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
                    }//parent==0..rec>0

                }//parent==0
 
            %>
            <%
                else
                {
                //parent!=0..no direct sub menus
                }
            }//for


        }//rec>0
                conn.Close();
                conn2.Close();
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
   <%-- <div align="center" style="margin-top: -50px;">
        <img src="Images/Banner.png" width="1000px" height="100px" alt="RSS" />
    </div>--%>
    <div align="center" style="margin-top: -31px;">
        <form id="form1" runat="server">
                                    <telerik:RadScriptManager ID="RadScriptManager1" runat="server" EnablePageMethods="true">
                                        <Scripts>
                                            <%--<asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
                                            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
                                            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js" />--%>
                                        </Scripts>
                                    </telerik:RadScriptManager>
                                    <script type="text/javascript">
                                    </script>
                                    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1">
                                        <AjaxSettings>
                                            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                                                <UpdatedControls>
                                                    <%--<telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                                                    <telerik:AjaxUpdatedControl ControlID="RadInputManager1" />
                                                    <telerik:AjaxUpdatedControl ControlID="lblStatus" />
                                                    <telerik:AjaxUpdatedControl ControlID="placeholder" />
                                                    <telerik:AjaxUpdatedControl ControlID="RadToolTipManager1" />
                                                    <telerik:AjaxUpdatedControl ControlID="SqlDataSource1" />
                                                    <telerik:AjaxUpdatedControl ControlID="rbOSector" />--%>
                                                </UpdatedControls>
                                            </telerik:AjaxSetting>
                                        </AjaxSettings>
                                    </telerik:RadAjaxManager>
        <table border="0" cellpadding="2" cellspacing="2" width="1000px">
            <tr>
                <td style="border-right: blue thin solid; border-top: blue thin solid; border-left: blue thin solid;
                    border-bottom: blue thin solid; border-width: 0px" align="center">
                    <hr style="visibility: hidden;" />
                    <hr style="visibility: hidden;" />
                    <table border="0" width="100%">
                        <%--<tr>
                            <td id="Td1" align="left" runat="server" colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <div style="height: 20px;">
                                    <asp:Label class="labelstyle" ID="lblStatus" runat="server" ForeColor="Red" Font-Bold="true" />
                                </div>
                            </td>
                        </tr>--%>
                        <tr>
                        <td>
         
        <%--<div id="gallery" class="content">
            <div id="controls" class="controls"></div>
            <div id="slideshow" class="slideshow"></div>
            <div id="details" class="embox">
                <div id="download" class="download"><a id="download-link">Download Original</a></div>
                <div id="image-title" class="image-title"></div>
                <div id="image-desc" class="image-desc"></div>
            </div>
        </div>--%>

      <div  style="width:850px; position:fixed; right:400px; top:10px; height:500px;">
        <div class="easyui-tabs" align="center" style="width:850px; position:absolute; top:50px; background-color:White">
        <%      
            System.Data.DataTable dtItems3 = new System.Data.DataTable();
            System.Data.SqlClient.SqlConnection conn3 = BusinessTier.getConnection();
            conn3.Open();
            String sql3 = "select category_ID,kitchen,name,description,image from categorymaster where deleted ='False' and parent='0'";
            dtItems3 = new System.Data.DataTable();
            dtItems3.Clear();

            System.Data.SqlClient.SqlDataAdapter sqlDataAdapter3 = new System.Data.SqlClient.SqlDataAdapter(sql3, conn3);
            sqlDataAdapter3.Fill(dtItems3);
            int rco3 = dtItems3.Rows.Count;
            conn3.Close();
            if (rco3 > 0)
            {
                
                foreach (System.Data.DataRow dataRow3 in dtItems3.Rows)
                {
                %>

                <div title="<%= dataRow3["name"].ToString()%>" iconCls="icon-reload" closable="true" style="padding:10px;">
		    	
		        
                <%
                    int category_ID = Int32.Parse(dataRow3["category_ID"].ToString().Trim());
                    
                    System.Data.DataTable dtItems4 = new System.Data.DataTable();
                    System.Data.SqlClient.SqlConnection conn4 = BusinessTier.getConnection();
                    conn4.Open();
                    String sql4 = "select category_ID,name,kitchen,description,image from categorymaster where deleted ='False' and parent=" + category_ID;
                    dtItems4 = new System.Data.DataTable();
                    dtItems4.Clear();

                    System.Data.SqlClient.SqlDataAdapter sqlDataAdapter4 = new System.Data.SqlClient.SqlDataAdapter(sql4, conn4);
                    sqlDataAdapter4.Fill(dtItems4);
                    int rco4 = dtItems4.Rows.Count;
                    conn4.Close();
                    if (rco4 > 0)
                    {
                        %>
                        <div class="easyui-tabs" style="width:830px;height:500px;background-color:transparent">
                        <%
                        foreach (System.Data.DataRow dataRow4 in dtItems4.Rows)
                        {
                        %>
<%--                        <div class="easyui-tabs" style="width:700px;height:490px;">
                            <div title="Second Tab" closable="true" style="padding:10px;">
			                    Second Tab
		                    </div>
                        </div>--%>
                         
                         <%--<div class="easyui-tabs" style="width:830px;height:500px;">--%>
                            <div title="<%= dataRow4["name"].ToString()%>" iconCls="icon-reload" closable="true" style="padding:10px;">
		                	    <%--<%= dataRow4["name"].ToString()%>--%>
                                <% 
                                System.Data.DataTable dtItems5 = new System.Data.DataTable();
                                System.Data.SqlClient.SqlConnection conn5 = BusinessTier.getConnection();
                                int category_ID2 = Int32.Parse(dataRow4["category_ID"].ToString().Trim());
                                conn5.Open();
                                String sql5 = "select ID,category,name,unitprice,Picture,Description,Discount from menumaster where deleted ='False' and Category=" + category_ID2;
                                dtItems5 = new System.Data.DataTable();
                                dtItems5.Clear();

                                System.Data.SqlClient.SqlDataAdapter sqlDataAdapter5 = new System.Data.SqlClient.SqlDataAdapter(sql5, conn5);
                                sqlDataAdapter5.Fill(dtItems5);
                                int rco5 = dtItems5.Rows.Count;
                                conn5.Close();
                                if (rco5 > 0)
                                {
                                %>
                                <ul class="products">
                                <%
                                foreach (System.Data.DataRow dataRow5 in dtItems5.Rows)
                                {
                                %>
                                <%--<li>
			                        <a href="#" class="item">
				                        <img src="images/cake/KekDalamKek.jpg" alt=""/>
				                        <div>
					                        <p>Kek Dalam Kek</p>
					                        <p>Price:$25</p>
				                        </div>
			                        </a>
		                        </li>--%>
		                            <li>
		                            	<a href="#" class="item">
				                       
                                        <%--<img src="ShowImage.ashx?autoid=<%= dataRow5["ID"].ToString().Trim() %>" alt=""/>--%>
                                        <%--<img src="ShowImage.ashx?autoid=<%= dataRow5["ID"].ToString().Trim() %>" alt=""/>--%>
				                        <div style=" background-color:red; border-width:medium; height:50px; overflow:hidden">
                                            <p style="color:red; font-size:xx-small"><%=dataRow5["ID"].ToString().Trim()%></p>
					                        <p style="color:White;"><%=dataRow5["name"].ToString().Trim()%></p>
					                        <p style="color:red; font-size:xx-small">Price:RM<%= dataRow5["unitprice"].ToString()%></p>
				                        </div>
			                            </a>
		                            </li>
                                <%
                                }
                                %>
                                </ul>
                                 <%
                                }
                                %>

		                    </div>
                      
                        <%
}%>
                       
                         </div>
                        <%
                    }
                    else
                    {
                    %>
                    <%= dataRow3["name"].ToString()%>
                    <%
                    }
                     %>
                     
                    </div>
                    <%
                }
            }
                        conn3.Close();
                        //conn3.Close();
                            %>
		<%--<div title="First Tab" style="padding:10px;">
			<ul class="products">
		<li>
			<a href="#" class="item">
				<img src="images/cake/KekDalamKek.jpg" alt=""/>
				<div>
					<p>Kek Dalam Kek</p>
					<p>Price:$25</p>
				</div>
			</a>
		</li>
		<li>
			<a href="#" class="item">
				<img src="images/cake/LapisCoklatPutih.jpg" alt=""/>
				<div>
					<p>Lapis Coklat Putih</p>
					<p>Price:$40</p>
				</div>
			</a>
		</li>
        <li>
			<a href="#" class="item">
				<img src="images/cake/LapisBatikKenyalang.jpg" alt=""/>
				<div>
					<p>Lapis Batik Kenyalang</p>
					<p>Price:$30</p>
				</div>
			</a>
		</li>
        <li>
			<a href="#" class="item">
				<img src="images/cake/LapisCoklatWalnut.jpg" alt=""/>
				<div>
					<p>Lapis Coklat Walnut</p>
					<p>Price:$35</p>
				</div>
			</a>
		</li>

	</ul>
		</div>--%>
		<%--<div title="Second Tab" closable="true" style="padding:10px;">
			<%--<div id="navigation" class="navigation">
              <ul class="thumbs noscript">
                  <li><a href="http://farm1.static.flickr.com/100/253177978_7609879637_o_d.jpg" original="http://farm1.static.flickr.com/100/253177978_7609879637_o_d.jpg" title="There's a train a comin'" description="Description"><img src="http://farm1.static.flickr.com/100/253177978_7609879637_s.jpg" alt="There's a train a comin'" /></a></li>
	            	<li><a href="http://farm1.static.flickr.com/112/253177973_0710df33a7_d.jpg" original="http://farm1.static.flickr.com/112/253177973_0710df33a7_d.jpg" title="P P P Pick Up A Penguin" description="Description"><img src="http://farm1.static.flickr.com/112/253177973_0710df33a7_s.jpg" alt="P P P Pick Up A Penguin" /></a></li>
		            <li><a href="http://farm1.static.flickr.com/90/253177962_4a0056155e_o_d.jpg" original="http://farm1.static.flickr.com/90/253177962_4a0056155e_o_d.jpg" title="Ban The Flag" description="Description"><img src="http://farm1.static.flickr.com/90/253177962_4a0056155e_s_d.jpg" alt="Ban The Flag" /></a></li>
		            <li><a href="http://farm1.static.flickr.com/103/253177996_4d56eb8a92_d.jpg" original="http://farm1.static.flickr.com/103/253177996_4d56eb8a92_d.jpg" title="Our Man Charlie" description="Description"><img src="http://farm1.static.flickr.com/103/253177996_4d56eb8a92_s_d.jpg" alt="Our Man Charlie" /></a></li>
                    <li><a href="http://farm4.static.flickr.com/3153/2538167690_c812461b7b.jpg" original="http://farm4.static.flickr.com/3153/2538167690_c812461b7b_b.jpg" title="Title #3" description="Description"><img src="http://farm4.static.flickr.com/3153/2538167690_c812461b7b_s.jpg" alt="Title #3" /></a></li>
		            <li><a href="http://farm4.static.flickr.com/3150/2538167224_0a6075dd18.jpg" original="http://farm4.static.flickr.com/3150/2538167224_0a6075dd18_b.jpg" title="Title #4" description="Description"><img src="http://farm4.static.flickr.com/3150/2538167224_0a6075dd18_s.jpg" alt="Title #4" /></a></li>
		            <li><a href="http://farm4.static.flickr.com/3204/2537348699_bfd38bd9fd.jpg" original="http://farm4.static.flickr.com/3204/2537348699_bfd38bd9fd_b.jpg" title="Title #5" description="Description"><img src="http://farm4.static.flickr.com/3204/2537348699_bfd38bd9fd_s.jpg" alt="Title #5" /></a></li>
		            <li><a href="http://farm4.static.flickr.com/3124/2538164582_b9d18f9d1b.jpg" original="http://farm4.static.flickr.com/3124/2538164582_b9d18f9d1b_b.jpg" title="Title #6" description="Description"><img src="http://farm4.static.flickr.com/3124/2538164582_b9d18f9d1b_s.jpg" alt="Title #6" /></a></li>
		            <li><a href="http://farm4.static.flickr.com/3205/2538164270_4369bbdd23.jpg" original="http://farm4.static.flickr.com/3205/2538164270_c7d1646ecf_o.jpg" title="Title #7" description="Description"><img src="http://farm4.static.flickr.com/3205/2538164270_4369bbdd23_s.jpg" alt="Title #7" /></a></li>
		            <li><a href="http://farm4.static.flickr.com/3211/2538163540_c2026243d2.jpg" original="http://farm4.static.flickr.com/3211/2538163540_c2026243d2_b.jpg" title="Title #8" description="Description"><img src="http://farm4.static.flickr.com/3211/2538163540_c2026243d2_s.jpg" alt="Title #8" /></a></li>
		            <li><a href="http://farm3.static.flickr.com/2315/2537343449_f933be8036.jpg" original="http://farm3.static.flickr.com/2315/2537343449_f933be8036_b.jpg" title="Title #9" description="Description"><img src="http://farm3.static.flickr.com/2315/2537343449_f933be8036_s.jpg" alt="Title #9" /></a></li>
		            <li><a href="http://farm3.static.flickr.com/2167/2082738157_436d1eb280.jpg" original="http://farm3.static.flickr.com/2167/2082738157_436d1eb280_b.jpg" title="Title #10" description="Description"><img src="http://farm3.static.flickr.com/2167/2082738157_436d1eb280_s.jpg" alt="Title #10" /></a></li>
		            <li><a href="http://farm3.static.flickr.com/2342/2083508720_fa906f685e.jpg" original="http://farm3.static.flickr.com/2342/2083508720_fa906f685e_b.jpg" title="Title #11" description="Description"><img src="http://farm3.static.flickr.com/2342/2083508720_fa906f685e_s.jpg" alt="Title #11" /></a></li>
		            <li><a href="http://farm3.static.flickr.com/2132/2082721339_4b06f6abba.jpg" original="http://farm3.static.flickr.com/2132/2082721339_4b06f6abba_b.jpg" title="Title #12" description="Description"><img src="http://farm3.static.flickr.com/2132/2082721339_4b06f6abba_s.jpg" alt="Title #12" /></a></li>
		            <li><a href="http://farm3.static.flickr.com/2139/2083503622_5b17f16a60.jpg" original="http://farm3.static.flickr.com/2139/2083503622_5b17f16a60_b.jpg" title="Title #13" description="Description"><img src="http://farm3.static.flickr.com/2139/2083503622_5b17f16a60_s.jpg" alt="Title #13" /></a></li>
		            <li><a href="http://farm3.static.flickr.com/2041/2083498578_114e117aab.jpg" original="http://farm3.static.flickr.com/2041/2083498578_114e117aab_b.jpg" title="Title #14" description="Description"><img src="http://farm3.static.flickr.com/2041/2083498578_114e117aab_s.jpg" alt="Title #14" /></a></li>
             </ul>
            </div>--%>
            <%--<div class="easyui-tabs" style="width:700px;height:490px;">
                <div title="Second Tab" closable="true" style="padding:10px;">
			    Second Tab
		        </div>
            </div> --%>
		

	</div>
            </div>            
    <div class="cart">
		<h1>Order Cart</h1>
		<div style="background:#fff">
		<table id="cartcontent" fitcolumns="true" style="width:300px;height:auto;">
			<%--<thead>
				<tr>
                    <th field="ID" width="1">ID</th>
					<th field="name" width="210">Name</th>
					<th field="quantity" width="39" align="right">Qty</th>
					<th field="price" width="50" align="right">Price</th>
                    <th field="add" width="25" align="right">+</th>
                    <th field="reduce" width="25" align="right">-</th>
				</tr>
                </thead>--%>
                <%--<tr>
					<td>Kek Dalam Kek</td>
					<td>2</td>
					<td align="right">50</td>
				</tr>
                <tr>
					<td>Lapis Coklat Putih</td>
					<td>1</td>
					<td align="right">40</td>
				</tr>--%>
			
		</table>
		</div>
<%--        <p class="totalro"><b>TotalRoundOff: RM 0</b><br />
          
        </p>--%>
		<p class="total"><b>Total: RM 0</b><br />
          <h2>Drop here to add to cart</h2>
        </p>
        <%--<asp:ListBox ID="txtTblNo" TabIndex="5" runat="server" Rows="1"> 
           <asp:ListItem>Choose Table Number</asp:ListItem>
        </asp:ListBox>--%>
        <telerik:RadComboBox ID="txtTblNo" runat="server" Height="250px" Width="300px" DataTextField="TableNo" DataValueField="Table_ID" AutoPostBack="true"
        OnSelectedIndexChanged="cboTable_SelectedIndexChanged" DataSourceID="SqlDataSourceTable" AppendDataBoundItems="true" EmptyMessage="Select">
        </telerik:RadComboBox>
        <asp:SqlDataSource ID="SqlDataSourceTable" runat="server" ConnectionString="<%$ ConnectionStrings:connString %>"
                                        SelectCommand="select Table_ID,TableNo from TableMaster where DELETED=0 order by Table_ID">
                                    </asp:SqlDataSource>
	<%--	<button id="order" name="order"   type="submit" onclick="submitOrder();">Submit Order</button>--%>

        <asp:Button ID="btnSubmit" OnClientClick="submitOrder();" runat="server" Text="Submit Oder" 
         onclick="submitOrderServer" />

           <asp:Button ID="btnPrint" runat="server" Text="Print Order" Height="23"
                                    Width="115" Margin="10,10,10,10" OnClick="btnPrint_Click" FontWeight="Bold" FontSize="16"
                                    BorderBrush="#83FFFFFF" />

       <%-- <button id="Button1" name="order" type="submit" onclick="submitOrder();">Print Order</button>--%>
        <asp:Label ID="lblStatus" Width="80%" runat="server" ForeColor="Yellow" Font-Bold="true" />
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
