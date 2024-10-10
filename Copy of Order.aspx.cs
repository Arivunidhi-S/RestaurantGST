using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using System.ComponentModel;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.OleDb;
using System.Drawing;
using System.Web.SessionState;
using System.Runtime.Remoting.Contexts;
using System.Web.Script.Serialization;
using System.Net.Mail;

public partial class Cashier : System.Web.UI.Page
{

    private string uidSource, uidTarget;
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            if (!(string.IsNullOrEmpty(Session["sesUserID"].ToString())))
            {
                //DatePickerselect.SelectedDate = System.DateTime.Now;
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }
        catch (Exception ex)
        {
            Response.Redirect("Login.aspx");
        }

        //Session["sesUserID"] = "1";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(Session["sesUserID"].ToString()))
            {




                Response.Redirect("Login.aspx");
            }
            else
            {

                //loadtable();
                //lblStatus.Text = "";
            }
        }
        catch (Exception ex)
        {
            Response.Redirect("Login.aspx");
        }
    }

   public Dictionary<string, string> drows =new Dictionary<string, string>();
    
    //Dictionary<string, Dictionary<string,string>> data;

    public class Rows
    {
       public string name;
       public string quantity;
       public string price;
    }

    

   public struct Rows2
    {
        
        public string total;
        Rows[] r;
    }
    [System.Web.Services.WebMethod(EnableSession = false)]
    //[System.Web.Services.WebMethod]
    //public static string postOrder(Rows[] rdata)
    //public static string postOrder(string rdata)
   public static string postOrder(Rows[] rdata)
    {
        int l = rdata.Length;
        string val;
        string name;
        string quantity;
        string price;
        Rows[] r = new Rows[rdata.Length];
        for (int i = 0; i < rdata.Length; i++)
        {
            l = rdata.Length;
            name = rdata[i].name;
            quantity = rdata[i].quantity;
            price = rdata[i].price;
        }
            //r = (Rows2)rdata[0];
        //string name = rdata[0].ToString();
        //Rows[] r = new Rows[l];
        //rdata.CopyTo(r, 0);
        return null;
    }
    ////private void loadtable()
    ////{
    ////    try
    ////    {

    ////        //Tablepanel.Controls.Clear();
    ////        //Tablepanel1.Controls.Clear();
    ////        //Tablepanel2.Controls.Clear();
    ////        //Tablepanel3.Controls.Clear();
    ////        //Tablepanel4.Controls.Clear();
    ////        //Tablepanel5.Controls.Clear();
    ////        //Tablepanel6.Controls.Clear();
    ////        SqlConnection conn = BusinessTier.getConnection();
    ////        conn.Open();
    ////        string sql = "select Table_ID,TableNo FROM TableMaster";
    ////        SqlCommand command = new SqlCommand(sql, conn);
    ////        int i = 1;
    ////        SqlDataReader reader = command.ExecuteReader();

    ////        while (reader.Read())
    ////        {

    ////            Button TableButton = new Button();
    ////            TableButton.Text = (reader["TableNo"].ToString());
    ////            TableButton.ID = (reader["Table_ID"].ToString());


    ////            TableButton.Click += new EventHandler(TableButton_Click);
    ////            TableButton.Width = 100;
    ////            TableButton.Height = 60;
    ////            TableButton.EnableTheming = true;
    ////            TableButton.Font.Size = 10;
    ////            TableButton.ForeColor = System.Drawing.Color.Brown;
    ////            TableButton.Font.Bold = true;
    ////            SqlConnection conn1 = BusinessTier.getConnection();
    ////            conn1.Open();
    ////            string sql1 = "select Table_ID FROM VW_TableOrderMaster where Table_ID = '" + reader["Table_ID"].ToString() + "'";
    ////            SqlCommand command1 = new SqlCommand(sql1, conn1);

    ////            SqlDataReader reader1 = command1.ExecuteReader();
    ////            if (reader1.Read())
    ////            {
    ////                TableButton.BackColor = System.Drawing.Color.Green;
    ////            }
    ////            reader1.Close();
    ////            BusinessTier.DisposeReader(reader1);
    ////            BusinessTier.DisposeConnection(conn1);
    ////            //Tablepanel.HorizontalAlign = HorizontalAlign.Center;

    ////            //if (i <= 5)
    ////            //{
    ////            //    Tablepanel.Controls.Add(TableButton);
    ////            //    Tablepanel.Controls.Add(new LiteralControl("<br />"));
    ////            //    Tablepanel.Controls.Add(new LiteralControl("<br />"));
    ////            //}
    ////            //if (i > 5 && i <= 10)
    ////            //{
    ////            //    Tablepanel1.Controls.Add(TableButton);
    ////            //    Tablepanel1.Controls.Add(new LiteralControl("<br />"));
    ////            //    Tablepanel1.Controls.Add(new LiteralControl("<br />"));
    ////            //}
    ////            //if (i > 10 && i <= 15)
    ////            //{
    ////            //    Tablepanel2.Controls.Add(TableButton);
    ////            //    Tablepanel2.Controls.Add(new LiteralControl("<br />"));
    ////            //    Tablepanel2.Controls.Add(new LiteralControl("<br />"));
    ////            //}
    ////            //if (i > 15 && i <= 20)
    ////            //{
    ////            //    Tablepanel3.Controls.Add(TableButton);
    ////            //    Tablepanel3.Controls.Add(new LiteralControl("<br />"));
    ////            //    Tablepanel3.Controls.Add(new LiteralControl("<br />"));
    ////            //}
    ////            //if (i > 20 && i <= 25)
    ////            //{
    ////            //    Tablepanel4.Controls.Add(TableButton);
    ////            //    Tablepanel4.Controls.Add(new LiteralControl("<br />"));
    ////            //    Tablepanel4.Controls.Add(new LiteralControl("<br />"));
    ////            //}
    ////            //if (i > 25 && i <= 30)
    ////            //{
    ////            //    Tablepanel5.Controls.Add(TableButton);
    ////            //    Tablepanel5.Controls.Add(new LiteralControl("<br />"));
    ////            //    Tablepanel5.Controls.Add(new LiteralControl("<br />"));
    ////            //}
    ////            //if (i > 30 && i <= 35)
    ////            //{
    ////            //    Tablepanel6.Controls.Add(TableButton);
    ////            //    Tablepanel6.Controls.Add(new LiteralControl("<br />"));
    ////            //    Tablepanel6.Controls.Add(new LiteralControl("<br />"));
    ////            //}

    ////            i++;
    ////        }
    ////        reader.Close();
    ////        BusinessTier.DisposeReader(reader);
    ////        BusinessTier.DisposeConnection(conn);
    ////    }
    ////    catch (Exception ex)
    ////    {

    ////        lblStatus.Text = "Exception:Table Load" + ex.Message.ToString();
    ////    }
    ////}

    ////protected void TableButton_Click(object sender, EventArgs e)
    ////{
    ////    try
    ////    {

    ////        string script = "function f(){$find(\"" + UserListDialog.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
    ////        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);

    ////        lblStatus.Text = "";
    ////        string tableorderid = ((Button)sender).ID;
    ////        Session["tableorderid"] = ((Button)sender).Text.ToString().Trim();
    ////        //Lblskid.Text = ((Button)sender).Text.ToString().Trim();
    ////        //Session["sesskidID"] = skidid.ToString().Trim();
    ////        //if (!(Session["sesskidID"].ToString() == "0"))
    ////        //{

    ////        //}


    ////        //lblStatus.Text = "";
    ////        //string Tableid = ((Button)sender).ID;
    ////        //Session["sesmacname"] = ((Button)sender).Text.ToString().Trim();
    ////        //Lblmachine.Text = ((Button)sender).Text.ToString().Trim();
    ////        //Session["sesMachineID"] = machineid.ToString().Trim();
    ////        //RadGrid1.DataSource = DataSourceHelper(Session["sesMachineID"].ToString().Trim());
    ////        //RadGrid1.Rebind();
    ////        //string strTagTypeID = "";
    ////        //foreach (GridDataItem grdItem in RadGrid1.Items)
    ////        //{
    ////        //    RadNumericTextBox txtParaValue = (RadNumericTextBox)grdItem.FindControl("txtParaValue");
    ////        //    RadComboBox cboParaValues_Discrete = (RadComboBox)grdItem.FindControl("cboParaValues_Discrete");
    ////        //    RadComboBox cboParaValues_String = (RadComboBox)grdItem.FindControl("cboParaValues_String");
    ////        //    //AjaxControlToolkit.ComboBox cboParaValues_Discrete = (AjaxControlToolkit.ComboBox)grdItem.FindControl("cboParaValues_Discrete");
    ////        //    //AjaxControlToolkit.ComboBox cboParaValues_String = (AjaxControlToolkit.ComboBox)grdItem.FindControl("cboParaValues_String");
    ////        //    Label lblTagTypeID = (Label)grdItem.FindControl("lblTagTypeID");
    ////        //    Label lblminrange = (Label)grdItem.FindControl("lblMinEu");
    ////        //    Label lblmaxrange = (Label)grdItem.FindControl("lblmaxEu");
    ////        //    Label lblunit = (Label)grdItem.FindControl("lblunitval");
    ////        //    strTagTypeID = lblTagTypeID.Text.ToString().Trim();
    ////        //    txtParaValue.Visible = false;
    ////        //    cboParaValues_String.Visible = false;
    ////        //    cboParaValues_Discrete.Visible = false;
    ////        //    lblminrange.Visible = true;
    ////        //    lblmaxrange.Visible = true;
    ////        //    lblunit.Visible = true;
    ////        //    if (strTagTypeID.ToString().Trim() == "1")
    ////        //    {
    ////        //        txtParaValue.Visible = true;
    ////        //    }

    ////        //    if (strTagTypeID.ToString().Trim() == "2")
    ////        //    {
    ////        //        cboParaValues_Discrete.Visible = true;
    ////        //        lblminrange.Visible = false;
    ////        //        lblmaxrange.Visible = false;
    ////        //        lblunit.Visible = false;
    ////        //        LoadDiscreetstringcombo(cboParaValues_Discrete, "Discreet");
    ////        //    }

    ////        //    if (strTagTypeID.ToString().Trim() == "3")
    ////        //    {
    ////        //        cboParaValues_String.Visible = true;
    ////        //        LoadDiscreetstringcombo(cboParaValues_String, "String");
    ////        //        lblminrange.Visible = false;
    ////        //        lblmaxrange.Visible = false;
    ////        //        lblunit.Visible = false;
    ////        //    }
    ////        //}
    ////    }
    ////    catch (Exception ex)
    ////    {
    ////        lblStatus.Text = "Exception:Table No click Error" + ex.Message.ToString();
    ////        return;
    ////    }
    ////}

    //protected void cboTableno_OnSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    //{

    //    //if (!(cboTableno.Text.ToString() == "--Select--"))
    //    //{
    //    //    if (string.IsNullOrEmpty(cboTableno.SelectedValue.ToString().Trim()))
    //    //    {

    //    //    }
    //    //    else
    //    //    {
    //    //        //RadGridCargoOut.DataSource = DataSourceHelper();
    //    //        //RadGridCargoOut.Rebind();
    //    //    }
    //    //}
    //}
    ////protected void RadGrid1_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    ////{
    ////    try
    ////    {
    ////        RadGrid1.DataSource = DataSourceHelper();
    ////    }
    ////    catch (Exception ex)
    ////    {
    ////        ShowMessage("Data does not exists", "Red");
    ////        //InsertLogAuditTrial is used to insert the action into MasterAuditTrail table
    ////        InsertLogAuditTrail(Session["sesUserID"].ToString(), "VOID", "NeedDataSource", ex.ToString(), "Audit");
    ////    }
    ////}

    ////public DataTable DataSourceHelper()
    ////{
    ////    SqlConnection conn = BusinessTier.getConnection();
    ////    conn.Close();
    ////    //string sql = "select Vessel_ID,Vessel_NAME,Description,Vessel_Type,Nature_Of_Activity,CREATED_BY FROM Vessel WHERE Deleted=0 order by Vessel_Name";
    ////    string sql = "select DetailID,Name,Qty,Price FROM VW_OrderDetailMenu where Deleted=0 and Status='P'";
    ////    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sql, conn);
    ////    DataTable g_datatable = new DataTable();
    ////    sqlDataAdapter.Fill(g_datatable);
    ////    BusinessTier.DisposeAdapter(sqlDataAdapter);
    ////    BusinessTier.DisposeConnection(conn);
    ////    return g_datatable;
    ////}

    ////protected void RadGrid1_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    ////{
    ////    try
    ////    {
    ////        //if (Session["sesUserType"].ToString() == "1")
    ////        //{

    ////        string ID = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["DetailID"].ToString();
    ////        SqlConnection conn = BusinessTier.getConnection();
    ////        conn.Open();
    ////        string strupdate = "UPDATE [ORDERDETAIL] SET [Deleted]=1,[Modified_By] ='" + Session["sesUserID"].ToString().Trim() + "', [Modified_date]='" + System.DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss ") + "' WHERE [DetailID] = '" + ID + "'";
    ////        SqlCommand cmd = new SqlCommand(strupdate, conn);
    ////        cmd.ExecuteNonQuery();
    ////        BusinessTier.DisposeConnection(conn);
    ////        ShowMessage("Deleted Successfully", "Yellow");
    ////        RadGrid1.Rebind();
    ////        //}
    ////        //else
    ////        //{
    ////        //    lblStatus.Text = "YOU DON'T HAVE PERMISSION TO DELETE";
    ////        //}
    ////    }
    ////    catch (Exception ex)
    ////    {
    ////        ShowMessage("Unable to delete, Please try again/Contact your administrator", "Red");
    ////        e.Canceled = true;
    ////        //InsertLogAuditTrial is used to insert the action into MasterAuditTrail table
    ////        InsertLogAuditTrail(Session["sesUserID"].ToString(), "VOID", "Delete", ex.ToString(), "Audit");
    ////    }
    ////}

    ////===============================change Tbale==============================================
    ////protected void cmbSourceTable_SelectionChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    ////{
    ////    //lstSource.Items.Clear();
    ////    //if (cmbSourceTable.SelectedIndex != -1)
    ////    //{
    ////    //    //string UID = DataLayer.getTableIDUsingName(cmbSourceTable.SelectedValue.ToString());
    ////    //    string UID = cmbSourceTable.SelectedValue.ToString();
    ////    //    uidSource = UID;
    ////    //    string dtpckrselect = Convert.ToString(DatePickerselect.SelectedDate);
    ////    //    ArrayList orderIDs = BusinessTier.getPendingOrderIDsUsingUID(UID, dtpckrselect);
    ////    //    for (int i = 0; i < orderIDs.Count; i++)
    ////    //    {
    ////    //        DataSet orderItems = BusinessTier.getOrderDetailsUsingOrderID(orderIDs[i].ToString());
    ////    //        for (int k = 0; k < orderItems.Tables["ORDERITEMS"].Rows.Count; k++)
    ////    //        {
    ////    //            lstSource.Items.Add(orderItems.Tables["ORDERITEMS"].Rows[k]["OrderID"].ToString() + "-" + orderItems.Tables["ORDERITEMS"].Rows[k]["Item"].ToString() + " - " + orderItems.Tables["ORDERITEMS"].Rows[k]["Descr"].ToString() + " : " + orderItems.Tables["ORDERITEMS"].Rows[k]["Qty"].ToString());
    ////    //        }
    ////    //    }
    ////    //    cmbTargetTable.SelectedIndex = -1;
    ////    //}
    ////}
    //protected void cmbTargetTable_SelectionChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    //{
    //    //lstTarget.Items.Clear();
    //    //if (cmbTargetTable.SelectedIndex != -1)
    //    //{
    //    //    //string UID = DataLayer.getTableIDUsingName(cmbTargetTable.SelectedValue.ToString());
    //    //    string UID = cmbTargetTable.SelectedValue.ToString();
    //    //    uidTarget = UID;
    //    //    string dtpckrselect = Convert.ToString(DatePickerselect.SelectedDate);
    //    //    ArrayList orderIDs = BusinessTier.getPendingOrderIDsUsingUID(UID, dtpckrselect);

    //    //    for (int i = 0; i < orderIDs.Count; i++)
    //    //    {
    //    //        DataSet orderItems = BusinessTier.getOrderDetailsUsingOrderID(orderIDs[i].ToString());
    //    //        for (int k = 0; k < orderItems.Tables["ORDERITEMS"].Rows.Count; k++)
    //    //        {
    //    //            lstTarget.Items.Add(orderItems.Tables["ORDERITEMS"].Rows[k]["OrderID"].ToString() + "-" + orderItems.Tables["ORDERITEMS"].Rows[k]["Item"].ToString() + " - " + orderItems.Tables["ORDERITEMS"].Rows[k]["Descr"].ToString() + " : " + orderItems.Tables["ORDERITEMS"].Rows[k]["Qty"].ToString());
    //    //        }
    //    //    }
    //    //}
    //}

    //protected void btnChange_Click(object sender, EventArgs e)
    //{
    //    //if (uidSource == uidTarget)
    //    //{
    //    //    lblStatus.Text = "Source Table and Target Table cannot be same....";

    //    //}
    //    //if (string.IsNullOrEmpty(uidTarget))
    //    //{
    //    //    lblStatus.Text = "Choose the Target Table....";

    //    //}
    //    //else
    //    //{
    //    //    string flg = "";
    //    //    string dtpckrselect = Convert.ToString(DatePickerselect.SelectedDate);
    //    //    //string OrderID = DataLayer.GetOrderIDUsingUID(uidSource);                
    //    //    ArrayList orderIDs = BusinessTier.getPendingOrderIDsUsingUID(uidSource, dtpckrselect);
    //    //    for (int i = 0; i < orderIDs.Count; i++)
    //    //    {
    //    //        string sql = "Update dbo.orderMaster set Table_ID = " + uidTarget + " Where OrderID = " + orderIDs[i].ToString() + " and Status = 'P'";
    //    //        if (BusinessTier.ExecuteInsertUpdateQry(sql) == true)
    //    //            flg = "Y";
    //    //        else
    //    //            flg = "N";
    //    //    }
    //    //    if (flg == "Y")
    //    //    {
    //    //        lblStatus.Text = cmbSourceTable.SelectedValue.ToString() + "Choose the Target Table...." + cmbTargetTable.SelectedValue.ToString();
    //    //        Clear();
    //    //    }
    //    //    else
    //    //        lblStatus.Text = "Unable to Change.. Please Try later...";
    //    //    //  MessageBox.Show("Unable to Change.. Please Try later...");
    //    //}
    //}
    //private void Clear()
    //{
    //    lstSource.Items.Clear();
    //    lstTarget.Items.Clear();
    //    cmbSourceTable.SelectedIndex = -1;
    //    cmbTargetTable.SelectedIndex = -1;
    //    uidSource = "";
    //    uidTarget = "";
    //}
    //private void ShowMessage(string message, string color)
    //{
    //    lblStatus.Text = message.ToString();
    //    System.Drawing.ColorConverter colConvert = new ColorConverter();
    //    string strColor = color.ToString();
    //    lblStatus.ForeColor = (System.Drawing.Color)colConvert.ConvertFromString(strColor);
    //}


    //private void InsertLogAuditTrail(string userid, string module, string activity, string result, string flag)
    //{
    //    SqlConnection connLog = BusinessTier.getConnection();
    //    connLog.Open();
    //    BusinessTier.InsertLogAuditTrial(connLog, userid, module, activity, result, flag);
    //    BusinessTier.DisposeConnection(connLog);
    //}
}
