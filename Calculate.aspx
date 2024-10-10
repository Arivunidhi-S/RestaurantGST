<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Calculate.aspx.cs" Inherits="Calculate" %>

<%@ Register Assembly="Stimulsoft.Report.Web, Version=2011.2.1100.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a"
    Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<script language="javascript" type="text/javascript">
    history.go(1); /* undo user navigation (ex: IE Back Button) */

    function checkPasswordMatch()
     {
        var text1 = $find("<%=txtNet.ClientID %>").get_textBoxValue();
        var text2 = $find("<%=txtCash.ClientID %>").get_textBoxValue();
        // var text3 = $find("<%=txtBal.ClientID %>");
        //  var total9 = text1 - text2;
        document.getElementById('<%=txtBal.ClientID %>').value = text2 - text1;
    }
    function numberOnlyExample() {
        if ((event.keyCode < 46) || (event.keyCode > 57))
            return false;
    }



    function GetRadWindow() 
    {
        var Window1 = null;
        if (window.radWindow)
            Window1 = window.RadWindow; //Will work in Moz in all cases, including clasic dialog      
        else if (window.frameElement.radWindow)
            Window1 = window.frameElement.radWindow; //IE (and Moz as well)      
        return Window1;
    }
    function Close() {
        GetRadWindow().Close();
       top.location.href = top.location.href;
    } 

</script>
<body background="images/BG.jpg">
    <form id="form1" runat="server">
    <div>
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
                </td>
            </tr>
        </table>
        <%--        <div id="DivHeader" runat="server" style="height: 100%; width: 60%; background-image: url(Images/Untitled.jpg); color: Navy;">--%>
        <table border="0" cellpadding="2" cellspacing="5" width="100%">
            <tr>
                <td style="border-right: blue thin solid; border-top: blue thin solid; border-left: blue thin solid; background-color:Red;
                    border-bottom: blue thin solid; border-width: 0" align="center">
                    <table id="maitable" runat="server" align="left" border="0" style="width: 45%">
                        <tr>
                            <td align="left" style="width: 20%; height: auto">
                                <asp:Label ID="choose1" runat="server" Font-Bold="true" ForeColor="white" EnableTheming="true"
                                    Text="Table No" />
                            </td>
                            <td style="width: 25%" align="left">
                                <%--   <telerik:RadComboBox ID="cmbTable" runat="server" Height="90px" Width="150px" DataValueField="Station_ID"
                                    EnableLoadOnDemand="true" AppendDataBoundItems="True">
                                    <%--                        OnItemsRequested="cboStationName_OnItemsRequested" OnSelectedIndexChanged="OnSelectedIndexChangedHandler1"
                                  
                                    <HeaderTemplate>
                                        <table style="width: 300px" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td style="width: 160px;">
                                                    Table No
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
                                                    <%# DataBinder.Eval(Container, "Attributes['Station_Type']")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </telerik:RadComboBox>--%>
                                <telerik:RadTextBox ID="cmbTable" TabIndex="1" ReadOnly="true" runat="server" Width="150px"
                                    Style="margin-left: 0px" />
                            </td>
                        </tr>
                        <tr style="width: 20%; height: 10%">
                            <td align="left" style="width: 20%; height: 58%">
                                <asp:Label runat="server" ID="orderid" Font-Bold="true" Font-Size="10" EnableTheming="true"
                                    ForeColor="White" Text="Order IDs"></asp:Label>
                            </td>
                            <td align="left">
                                <telerik:RadTextBox ID="txtOrdID" TabIndex="1" ReadOnly="true" runat="server" Width="150px"
                                    Style="margin-left: 0px" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label runat="server" ID="Amount" Font-Bold="true" Font-Size="10" EnableTheming="true"
                                    ForeColor="White" Height="10" Text="Amount" />
                            </td>
                            <td align="left">
                                <telerik:RadTextBox ID="txtOrdAmt" TabIndex="1" ReadOnly="true" runat="server" Width="150px"
                                    Height="22" Style="margin-left: 0px" />
                            </td>
                        </tr>

                        <tr>
                            <td align="left">
                                <asp:Label runat="server" ID="Discount" Font-Bold="true" Font-Size="10" EnableTheming="true"
                                    ForeColor="White" Height="10" Text="Discount" />
                            </td>
                            <td align="left">
                                <telerik:RadTextBox ID="txtDis" TabIndex="1" ReadOnly="true" Font-Names="Poor Richard"
                                    runat="server" Width="150px" Height="22" Style="margin-left: 0px" />
                            </td>
                        </tr>

                        <tr>
                            <td align="left">
                                <asp:Label runat="server" ID="Tax" Font-Bold="true" Font-Size="10" EnableTheming="true"
                                    ForeColor="White" Height="10" Text="Dining Chrg" />
                            </td>
                            <td align="left">
                                <telerik:RadTextBox ID="txtTax" TabIndex="1" ReadOnly="true" runat="server" Width="150px"
                                    Height="22" Style="margin-left: 0px" />
                            </td>
                        </tr>

                        <tr>
                            <td align="left">
                                <asp:Label runat="server" ID="lblgst" Font-Bold="true" Font-Size="10" EnableTheming="true"
                                    ForeColor="White" Height="10" Text="GST @6%" />
                            </td>
                            <td align="left">
                                <telerik:RadTextBox ID="txtgst" TabIndex="1" ReadOnly="true" runat="server" Width="150px"
                                    Height="22" Style="margin-left: 0px" />
                            </td>
                        </tr>


                        
                        <tr>
                            <td align="left">
                                <asp:Label runat="server" ID="NetAmount" Font-Bold="true" Font-Size="10" EnableTheming="true"
                                    ForeColor="White" Height="10" Text="NetAmount" />
                            </td>
                            <td align="left">
                                <%--      
                              <telerik:RadNumericTextBox Visible="true" runat="server" Width="150px" ID="txtNet"
                                                                    NumberFormat-DecimalDigits="2"  Type="Number"
                                                                    ToolTip="Numeric, Enter Only Numbers"   Height="22" Style="margin-left: 0px"/>--%>
                                <telerik:RadTextBox ID="txtNet" TabIndex="1" ReadOnly="true" runat="server" Width="150px"
                                    Height="22" Style="margin-left: 0px" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label runat="server" ID="CashReceived" Font-Bold="true" Font-Size="10" EnableTheming="true"
                                    ForeColor="White" Height="10" Text="Cash Received" />
                            </td>
                            <td align="left">
                                <%--<telerik:RadNumericTextBox DefaultinsertValue="0" Type="Currency" ID="txtCash" TabIndex="1"
                                    ReadOnly="false" runat="server" Width="150px" Height="25" Style="margin-left: 0px">
                                    <NumberFormat ZeroPattern="RMn"></NumberFormat>  return numberOnlyExample()
                                </telerik:RadNumericTextBox>--%>
                                      <telerik:RadTextBox ID="txtCash" TabIndex="1" Font-Size="Large" ReadOnly="false" OnTextChanged="txtCash_TextChanged"
                                    AutoPostBack="true" runat="server" Width="150px" Height="40" Style="margin-left: 0px"
                                    onKeyPress="return numberOnlyExample();" onkeyup="checkPasswordMatch()" GotFocus="txt_remove_GotFocus" />
                             <%--   <telerik:RadTextBox ID="txtCash" TabIndex="1" Font-Size="Large" ReadOnly="false"
                                    OnTextChanged="txtCash_TextChanged" AutoPostBack="true" runat="server" Width="150px"
                                    Height="40" Style="margin-left: 0px" onKeyPress="return numberOnlyExample();"
                                  />--%>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtCash"
                                    ErrorMessage="Please Enter Only Numbers" Style="z-index: 101; left: 424px; position: absolute;
                                    top: 285px" ValidationExpression="^\d+$" ValidationGroup="check"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label runat="server" ID="Label1" Font-Bold="true" Font-Size="10" EnableTheming="true"
                                    ForeColor="White" Height="10" Text="Balance" />
                            </td>
                            <td align="left">
                                <telerik:RadTextBox ID="txtBal" TabIndex="1" Font-Size="Large" ReadOnly="true" runat="server"
                                    Width="150px" Height="40" Style="margin-left: 0px" />
                            </td>
                        </tr>
                    </table>
                    <table id="Table1" runat="server" align="Center" border="0" style="width: 55%">
                        <tr>
                            <td align="left" style="width: 5%">
                                <%--  <asp:Button ID="LoginBtn" Text="Log In" runat="server" BackColor="Transparent"  ForeColor="Black"  OnClick="LoginBtn_Click" Style="margin-top: 10px;
                margin-left: 420px;  border: 0;" />--%>
                                <asp:Button runat="server" Text="1" Height="50" Width="80" Margin="30,10,10,10" FontWeight="Bold"
                                    FontSize="20" Style="{dynamicresource roundedbutton}" BorderBrush="#83FFFFFF"
                                    OnClick="btnOne_Click" />
                            </td>
                            <td align="left" style="width: 5%">
                                <asp:Button runat="server" Text="2" Height="50" Width="80" Margin="30,10,10,10" FontWeight="Bold"
                                    FontSize="20" Style="{dynamicresource roundedbutton}" BorderBrush="#83FFFFFF"
                                    OnClick="btnTwo_Click" />
                            </td>
                            <td align="left" style="width: 5%">
                                <asp:Button ID="Button2" runat="server" Text="3" Height="50" Width="80" Margin="30,10,10,10"
                                    FontWeight="Bold" FontSize="16" Style="{dynamicresource roundedbutton}" BorderBrush="#83FFFFFF"
                                    OnClick="btnThree_Click" />
                            </td>
                            <td align="left" style="width: 5%">
                                <asp:Button ID="Button3" runat="server" Text="Clear" Height="50" Width="80" Margin="30,10,10,10"
                                    OnClick="rbtn_remove_Click" FontWeight="Bold" FontSize="16" Style="{dynamicresource roundedbutton}"
                                    BorderBrush="#83FFFFFF" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 5%">
                                <asp:Button ID="Button4" runat="server" Text="4" Height="50" Width="80" Margin="30,10,10,10"
                                    FontWeight="Bold" FontSize="16" Style="{dynamicresource roundedbutton}" BorderBrush="#83FFFFFF"
                                    OnClick="btnFour_Click" />
                            </td>
                            <td align="left" style="width: 5%">
                                <asp:Button ID="Button5" runat="server" Text="5" Height="50" Width="80" Margin="30,10,10,10"
                                    FontWeight="Bold" FontSize="16" Style="{dynamicresource roundedbutton}" BorderBrush="#83FFFFFF"
                                    OnClick="btnFive_Click" />
                            </td>
                            <td align="left" style="width: 5%">
                                <asp:Button ID="Button6" runat="server" Text="6" Height="50" Width="80" Margin="30,10,10,10"
                                    FontWeight="Bold" FontSize="16" Style="{dynamicresource roundedbutton}" BorderBrush="#83FFFFFF"
                                    OnClick="btnSix_Click" />
                            </td>
                            <td align="left" style="width: 5%">
                                <asp:Button ID="Button7" runat="server" Text=".." Height="50" Width="80" Margin="30,10,10,10"
                                    FontWeight="Bold" FontSize="16" Style="{dynamicresource roundedbutton}" BorderBrush="#83FFFFFF" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 5%">
                                <asp:Button ID="Button8" runat="server" Text="7" Height="50" Width="80" Margin="30,10,10,10"
                                    FontWeight="Bold" FontSize="16" Style="{dynamicresource roundedbutton}" BorderBrush="#83FFFFFF"
                                    OnClick="btnSeven_Click" />
                            </td>
                            <td align="left" style="width: 5%">
                                <asp:Button ID="Button9" runat="server" Text="8" Height="50" Width="80" Margin="30,10,10,10"
                                    FontWeight="Bold" FontSize="16" Style="{dynamicresource roundedbutton}" BorderBrush="#83FFFFFF"
                                    OnClick="btnEight_Click" />
                            </td>
                            <td align="left" style="width: 5%">
                                <asp:Button ID="Button10" runat="server" Text="9" Height="50" Width="80" Margin="30,10,10,10"
                                    FontWeight="Bold" FontSize="16" Style="{dynamicresource roundedbutton}" BorderBrush="#83FFFFFF"
                                    OnClick="btnNine_Click" />
                            </td>
                            <td align="left" style="width: 5%">
                                <asp:Button ID="Button11" runat="server" Text=".." Height="50" Width="80" Margin="30,10,10,10"
                                    FontWeight="Bold" FontSize="16" Style="{dynamicresource roundedbutton}" BorderBrush="#83FFFFFF" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 5%">
                                <asp:Button ID="Button12" runat="server" Text="0" Height="50" Width="80" Margin="30,10,10,10"
                                    FontWeight="Bold" FontSize="16" Style="{dynamicresource roundedbutton}" BorderBrush="#83FFFFFF"
                                    OnClick="btnZero_Click" />
                            </td>
                            <td align="left" style="width: 5%">
                                <asp:Button ID="Button13" runat="server" Text="." Height="50" Width="80" Margin="30,10,10,10"
                                    FontWeight="Bold" FontSize="16" Style="{dynamicresource roundedbutton}" BorderBrush="#83FFFFFF"
                                    OnClick="btnDot_Click" />
                            </td>
                            <td align="left" style="width: 5%">
                                <asp:Button ID="Button14" runat="server" Text="Ent" Height="50" Width="80" Margin="30,10,10,10"
                                    FontWeight="Bold" FontSize="16" Style="{dynamicresource roundedbutton}" BorderBrush="#83FFFFFF" />
                            </td>
                            <td align="left" style="width: 5%">
                                <asp:Button ID="Button15" runat="server" Text=".." Height="50" Width="80" Margin="30,10,10,10"
                                    FontWeight="Bold" FontSize="16" Style="{dynamicresource roundedbutton}" BorderBrush="#83FFFFFF" />
                            </td>
                        </tr>
                    </table>
                    <table align="left">
                        <tr>
                            <td align="left"  >
                                <asp:Button ID="btnRecieve" runat="server" Text="Recieve Cash Only" Height="50" Width="185"
                                    Margin="10,10,10,10" OnClick="btnRecieve_Click" FontWeight="Bold" FontSize="16"
                                    BorderBrush="#83FFFFFF" />
                            </td>
                            <td align="left"  >
                                <asp:Button ID="btnPrint" runat="server" Text="Recieve Cash Print Reciept" Height="50"
                                    Width="185" Margin="10,10,10,10" OnClick="btnPrint_Click" FontWeight="Bold" FontSize="16"
                                    BorderBrush="#83FFFFFF" />
                            </td>
                        </tr>
                        <tr>
                          <%--  <cc1:StiWebViewer ID="StiWebViewer1" runat="server" RenderMode="AjaxWithCache" Width="40px" Visible="false" />--%>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="border-right: blue thin solid; border-top: blue thin solid; border-left: blue thin solid;
                    border-bottom: blue thin solid; border-width: 0" align="left">
                      <cc1:StiWebViewer ID="StiWebViewer1" runat="server" RenderMode="AjaxWithCache" Width="40px" Visible="false"  />
                    </td>
                    </tr>
        </table>
        <%--   </div>
         <div id="Div1" runat="server" style="height: 17px; width: 50%; border:2 background-image: url(Images/Untitled.jpg); color: Navy;">
         </div>--%>
    </div>
    </form>
</body>
</html>
