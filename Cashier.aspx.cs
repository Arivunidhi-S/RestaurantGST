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
using System.Globalization;
using System.Collections;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.OleDb;
using System.Drawing;
using System.Diagnostics;
public partial class Cashier : System.Web.UI.Page
{

    public string uidSource, uidTarget;
    string itemID; string OrderID; string menuID; string TotQty;
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            if (!(string.IsNullOrEmpty(Session["sesUserID"].ToString())))
            {
                DatePickerselect.SelectedDate = System.DateTime.Now;
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



                // LoadQuantity(0);

                Response.Redirect("Login.aspx");
            }
            else
            {

                loadtable();
                lblStatus.Text = "";
            }
        }
        catch (Exception ex)
        {
            Response.Redirect("Login.aspx");
        }
    }


    //protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    Timer1.Interval = int.Parse(DropDownList1.SelectedValue);
    //    lblInterval.Text = Timer1.Interval.ToString();
    //}
    public void Timer1_Tick(object sender, EventArgs e)
    {

        loadtable();
    }


    protected void DatePickerselect_SelectedDateChanged(object sender, EventArgs e)
    {
        loadtable();
    }

    private void loadtable()
    {
        try
        {

            Tablepanel.Controls.Clear();
            Tablepanel1.Controls.Clear();
            Tablepanel2.Controls.Clear();
            Tablepanel3.Controls.Clear();
            Tablepanel4.Controls.Clear();
            Tablepanel5.Controls.Clear();
            Tablepanel6.Controls.Clear();
            Tablepanel7.Controls.Clear();
            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();
            string sql = "select Table_ID,TableNo FROM TableMaster where deleted=0";
            SqlCommand command = new SqlCommand(sql, conn);
            int i = 1;
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {

                Button TableButton = new Button();
                TableButton.Text = (reader["TableNo"].ToString());
                TableButton.ID = (reader["Table_ID"].ToString());
                string dtpckrselect = Convert.ToString(DatePickerselect.SelectedDate);
                DateTime selectdt = Convert.ToDateTime(dtpckrselect);
                TableButton.Click += new EventHandler(TableButton_Click);
                TableButton.Width = 100;
                TableButton.Height = 60;
                TableButton.EnableTheming = true;
                TableButton.Font.Size = 20;
                TableButton.ForeColor = System.Drawing.Color.White;
                TableButton.BackColor = System.Drawing.Color.Red;
                TableButton.BorderColor = System.Drawing.Color.Red;
                TableButton.Font.Bold = true;
                SqlConnection conn1 = BusinessTier.getConnection();
                conn1.Open();

                string sql1 = "select Table_ID FROM VW_TableOrderMaster where deleted=0 and Table_ID = '" + reader["Table_ID"].ToString() + "' and REPLACE(CONVERT(VARCHAR,OrdDate,106),' ','/') ='" + selectdt.ToString("dd/MMM/yyyy") + "'";
                SqlCommand command1 = new SqlCommand(sql1, conn1);

                SqlDataReader reader1 = command1.ExecuteReader();
                if (reader1.Read())
                {
                    TableButton.BackColor = System.Drawing.Color.Green;
                }
                reader1.Close();
                BusinessTier.DisposeReader(reader1);
                BusinessTier.DisposeConnection(conn1);
                Tablepanel.HorizontalAlign = HorizontalAlign.Center;

                if (i <= 5)
                {
                    Tablepanel.Controls.Add(TableButton);
                    Tablepanel.Controls.Add(new LiteralControl("<br />"));
                    Tablepanel.Controls.Add(new LiteralControl("<br />"));
                }
                if (i > 5 && i <= 10)
                {
                    Tablepanel1.Controls.Add(TableButton);
                    Tablepanel1.Controls.Add(new LiteralControl("<br />"));
                    Tablepanel1.Controls.Add(new LiteralControl("<br />"));
                }
                if (i > 10 && i <= 15)
                {
                    Tablepanel2.Controls.Add(TableButton);
                    Tablepanel2.Controls.Add(new LiteralControl("<br />"));
                    Tablepanel2.Controls.Add(new LiteralControl("<br />"));
                }
                if (i > 15 && i <= 20)
                {
                    Tablepanel3.Controls.Add(TableButton);
                    Tablepanel3.Controls.Add(new LiteralControl("<br />"));
                    Tablepanel3.Controls.Add(new LiteralControl("<br />"));
                }
                if (i > 20 && i <= 25)
                {
                    Tablepanel4.Controls.Add(TableButton);
                    Tablepanel4.Controls.Add(new LiteralControl("<br />"));
                    Tablepanel4.Controls.Add(new LiteralControl("<br />"));
                }
                if (i > 25 && i <= 30)
                {
                    Tablepanel5.Controls.Add(TableButton);
                    Tablepanel5.Controls.Add(new LiteralControl("<br />"));
                    Tablepanel5.Controls.Add(new LiteralControl("<br />"));
                }
                if (i > 30 && i <= 35)
                {
                    Tablepanel6.Controls.Add(TableButton);
                    Tablepanel6.Controls.Add(new LiteralControl("<br />"));
                    Tablepanel6.Controls.Add(new LiteralControl("<br />"));
                }
                if (i > 35 && i <= 40)
                {
                    Tablepanel7.Controls.Add(TableButton);
                    Tablepanel7.Controls.Add(new LiteralControl("<br />"));
                    Tablepanel7.Controls.Add(new LiteralControl("<br />"));
                }
                i++;
            }
            reader.Close();
            BusinessTier.DisposeReader(reader);
            BusinessTier.DisposeConnection(conn);
        }
        catch (Exception ex)
        {

            lblStatus.Text = "Exception:Table Load" + ex.Message.ToString();
        }
    }

    protected void TableButton_Click(object sender, EventArgs e)
    {
        try
        {
            Timer1.Enabled = false;


            string script = "function f(){$find(\"" + UserListDialog.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            lblStatus.Text = "";
            string tableorderid = ((Button)sender).ID;
            Session["tableName"] = ((Button)sender).Text.ToString().Trim();
            Session["tableid"] = ((Button)sender).ID.ToString().Trim();
            Session["selectDate"] = DatePickerselect.SelectedDate;

        }
        catch (Exception ex)
        {
            lblStatus.Text = "Exception:Table No click Error" + ex.Message.ToString();
            return;
        }
    }



    //===============================change Tbale==============================================

    protected void cmbSourceTable_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        string dtpckrselect = Convert.ToString(DatePickerselect.SelectedDate);
        DateTime selectdt = Convert.ToDateTime(dtpckrselect);
        SqlConnection conn = BusinessTier.getConnection();
        conn.Open();
        //select Table_ID,TableNO FROM VW_TableOrderMaster where Status='P' and deleted=0 group by Table_ID,TableNO
        // string sql1 = " select Table_ID,TableNO  from VW_TableOrderMaster where DELETED=0 and Status='P' and [TableNO] LIKE @text + '%' order by TableNO ASC";
        string sql1 = " select Distinct Table_ID,TableNO  from VW_TableOrderMaster where DELETED=0 and Status='P' and REPLACE(CONVERT(VARCHAR,OrdDate,106),' ','/') ='" + selectdt.ToString("dd/MMM/yyyy") + "' order by TableNO ASC";
        // string sql1 = " select CONTACT_ID,CONTACT_NAME,COMPANY_NAME from CONTACTS where DELETED=0  and [COMPANY_NAME] LIKE @text + '%' group by CONTACT_ID,CONTACT_NAME,COMPANY_NAME order by COMPANY_NAME ";
        SqlDataAdapter adapter1 = new SqlDataAdapter(sql1, conn);
        adapter1.SelectCommand.Parameters.AddWithValue("@Text", e.Text);
        DataTable dataTable1 = new DataTable();
        adapter1.Fill(dataTable1);
        RadComboBox comboBox = (RadComboBox)sender;
        comboBox.Items.Clear();
        foreach (DataRow row in dataTable1.Rows)
        {
            RadComboBoxItem item = new RadComboBoxItem();
            item.Text = row["TableNO"].ToString();
            item.Value = row["Table_ID"].ToString();

            item.Attributes.Add("TableNO", row["TableNO"].ToString());
            item.Attributes.Add("TableNO", row["TableNO"].ToString());
            comboBox.Items.Add(item);
            item.DataBind();
        }
        adapter1.Dispose();
        BusinessTier.DisposeConnection(conn);
    }



    protected void cmbSourceTable_SelectionChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        lstSource.Items.Clear();
        //if (cmbSourceTable.SelectedIndex != -1)
        //{
        //string UID = BusinessTier.getTableIDUsingName(cmbSourceTable.SelectedValue.ToString());
        string UID = cmbSourceTable.SelectedValue.ToString();
        uidSource = UID;
        string dtpckrselect = Convert.ToString(DatePickerselect.SelectedDate);
        ArrayList orderIDs = BusinessTier.getPendingOrderIDsUsingUID(UID, dtpckrselect);
        for (int i = 0; i < orderIDs.Count; i++)
        {
            DataSet orderItems = BusinessTier.getOrderDetailsUsingOrderID(orderIDs[i].ToString());
            for (int k = 0; k < orderItems.Tables["ORDERITEMS"].Rows.Count; k++)
            {
                lstSource.Items.Add(orderItems.Tables["ORDERITEMS"].Rows[k]["DetailID"].ToString() + "-" + orderItems.Tables["ORDERITEMS"].Rows[k]["Item"].ToString() + " - " + orderItems.Tables["ORDERITEMS"].Rows[k]["Descr"].ToString() + " : " + orderItems.Tables["ORDERITEMS"].Rows[k]["Qty"].ToString());
            }
        }
        cmbTargetTable.SelectedIndex = -1;
    }
    // }
    protected void cmbTargetTable_SelectionChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        lstTarget.Items.Clear();
        if (cmbTargetTable.SelectedIndex != -1)
        {
            //string UID = BusinessTier.getTableIDUsingName(cmbTargetTable.SelectedValue.ToString());
            string UID = cmbTargetTable.SelectedValue.ToString();
            uidTarget = UID;
            string dtpckrselect = Convert.ToString(DatePickerselect.SelectedDate);
            ArrayList orderIDs = BusinessTier.getPendingOrderIDsUsingUID(UID, dtpckrselect);

            for (int i = 0; i < orderIDs.Count; i++)
            {
                DataSet orderItems = BusinessTier.getOrderDetailsUsingOrderID(orderIDs[i].ToString());
                for (int k = 0; k < orderItems.Tables["ORDERITEMS"].Rows.Count; k++)
                {
                    lstTarget.Items.Add(orderItems.Tables["ORDERITEMS"].Rows[k]["DetailID"].ToString() + "-" + orderItems.Tables["ORDERITEMS"].Rows[k]["Item"].ToString() + " - " + orderItems.Tables["ORDERITEMS"].Rows[k]["Descr"].ToString() + " : " + orderItems.Tables["ORDERITEMS"].Rows[k]["Qty"].ToString());
                }
            }
        }
    }

    protected void btnChange_Click(object sender, EventArgs e)
    {
        try
        {
            string UID = cmbSourceTable.SelectedValue.ToString();
            uidSource = UID;
            string UID1 = cmbTargetTable.SelectedValue.ToString();
            uidTarget = UID1;
            if (uidSource == uidTarget)
            {
                lblStatus.Text = "Source Table and Target Table cannot be same....";
                return;
            }
            if (string.IsNullOrEmpty(uidTarget))
            {
                lblStatus.Text = "Choose the Target Table....";
                return;
            }
            else
            {
                string flg = "";
                string dtpckrselect = Convert.ToString(DatePickerselect.SelectedDate);
                //string OrderID = BusinessTier.GetOrderIDUsingUID(uidSource);                
                ArrayList orderIDs = BusinessTier.getPendingOrderIDsUsingUID(uidSource, dtpckrselect);
                for (int i = 0; i < orderIDs.Count; i++)
                {
                    string sql = "Update dbo.orderMaster set Table_ID = " + uidTarget + " Where OrderID = " + orderIDs[i].ToString() + " and Status = 'P'";
                    if (BusinessTier.ExecuteInsertUpdateQry(sql) == true)
                        flg = "Y";
                    else
                        flg = "N";
                }
                if (flg == "Y")
                {
                    lblStatus.Text = "Table NO " + cmbSourceTable.SelectedValue.ToString() + " has been changed to " + "Table NO " + cmbTargetTable.SelectedValue.ToString();
                    Clear();
                }
                else
                    lblStatus.Text = "Unable to Change.. Please Try later...";
                //  MessageBox.Show("Unable to Change.. Please Try later...");
            }
            //lstSource.Items.Clear();
            //cmbSourceTable.SelectedItem.Text = "";
            loadtable();
        }

        catch (Exception ex)
        {
            lblStatus.Text = ex.Message;
        }

    }
    private void Clear()
    {
        lstSource.Items.Clear();
        lstTarget.Items.Clear();
        cmbSourceTable.SelectedIndex = -1;
        cmbTargetTable.SelectedIndex = -1;
        uidSource = "";
        uidTarget = "";
    }

    //====================================CombineTable===========================================


    protected void cmbSourceTablecombine_SelectionChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        lstSourcecombine.Items.Clear();
        //if (cmbSourceTablecombine.SelectedIndex != -1)
        //{
        //string UID = BusinessTier.getTableIDUsingName(cmbSourceTable.SelectedValue.ToString());
        string UID = cmbSourceTablecombine.SelectedValue.ToString();
        uidSource = UID;
        string dtpckrselect = Convert.ToString(DatePickerselect.SelectedDate);
        ArrayList orderIDs = BusinessTier.getPendingOrderIDsUsingUID(UID, dtpckrselect);
        for (int i = 0; i < orderIDs.Count; i++)
        {
            DataSet orderItems = BusinessTier.getOrderDetailsUsingOrderID(orderIDs[i].ToString());
            for (int k = 0; k < orderItems.Tables["ORDERITEMS"].Rows.Count; k++)
            {
                lstSourcecombine.Items.Add(orderItems.Tables["ORDERITEMS"].Rows[k]["DetailID"].ToString() + "-" + orderItems.Tables["ORDERITEMS"].Rows[k]["Item"].ToString() + " - " + orderItems.Tables["ORDERITEMS"].Rows[k]["Descr"].ToString() + " : " + orderItems.Tables["ORDERITEMS"].Rows[k]["Qty"].ToString());
            }
        }
        cmbTargetTablecombine.SelectedIndex = -1;
        //}
    }
    protected void cmbTargetTablecombine_SelectionChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        lstTargetcombine.Items.Clear();
        if (cmbTargetTablecombine.SelectedIndex != -1)
        {
            //string UID = BusinessTier.getTableIDUsingName(cmbTargetTable.SelectedValue.ToString());
            string UID = cmbTargetTablecombine.SelectedValue.ToString();
            uidTarget = UID;
            string dtpckrselect = Convert.ToString(DatePickerselect.SelectedDate);
            ArrayList orderIDs = BusinessTier.getPendingOrderIDsUsingUID(UID, dtpckrselect);

            for (int i = 0; i < orderIDs.Count; i++)
            {
                DataSet orderItems = BusinessTier.getOrderDetailsUsingOrderID(orderIDs[i].ToString());
                for (int k = 0; k < orderItems.Tables["ORDERITEMS"].Rows.Count; k++)
                {
                    lstTargetcombine.Items.Add(orderItems.Tables["ORDERITEMS"].Rows[k]["DetailID"].ToString() + "-" + orderItems.Tables["ORDERITEMS"].Rows[k]["Item"].ToString() + " - " + orderItems.Tables["ORDERITEMS"].Rows[k]["Descr"].ToString() + " : " + orderItems.Tables["ORDERITEMS"].Rows[k]["Qty"].ToString());
                }
            }
        }
    }
    protected void btnCombine_Click(object sender, EventArgs e)
    {
        try
        {
            string UID = cmbSourceTablecombine.SelectedValue.ToString();
            uidSource = UID;
            string UID1 = cmbTargetTablecombine.SelectedValue.ToString();
            uidTarget = UID1;
            if (uidSource == uidTarget)
            {
                lblStatus.Text = "Source Table and Target Table cannot be same....";
                return;
            }
            else
            {
                string flg = "";
                //string sourceOrderID = BusinessTier.GetOrderIDUsingUID(uidSource);
                //string targetOrderID = BusinessTier.GetOrderIDUsingUID(uidTarget);
                string dtpckrselect = Convert.ToString(DatePickerselect.SelectedDate);
                //string OrderID = BusinessTier.GetOrderIDUsingUID(uidSource);                
                ArrayList orderIDs = BusinessTier.getPendingOrderIDsUsingUID(uidSource, dtpckrselect);
                for (int i = 0; i < orderIDs.Count; i++)
                {
                    string sql = "Update dbo.orderMaster set Table_ID = " + uidTarget + " , Modified_Date='" + DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss") + "',Modified_By='" + Session["sesUserID"].ToString() + "' , Status = 'P' Where OrderID = " + orderIDs[i].ToString() + " "; //and REPLACE(CONVERT(VARCHAR,OrdDate,106),' ','/') = '" + DateTime.Now.ToString("dd/MMM/yyyy") + "'";
                    if (BusinessTier.ExecuteInsertUpdateQry(sql) == true)
                        flg = "Y";
                    else
                        flg = "N";
                }
                if (flg == "Y")
                {
                    lblStatus.Text = "Table NO " + cmbSourceTablecombine.SelectedValue.ToString() + " has been combined to " + "Table NO " + cmbTargetTablecombine.SelectedValue.ToString();
                    Clear1();
                }
                else
                    lblStatus.Text = "Unable to Change.. Please Try later...";
            }
            loadtable();
        }

        catch (Exception ex)
        {
            lblStatus.Text = ex.Message;
        }
    }

    private void Clear1()
    {
        lstSourcecombine.Items.Clear();
        lstTargetcombine.Items.Clear();
        cmbSourceTablecombine.SelectedIndex = -1;
        cmbTargetTablecombine.SelectedIndex = -1;
        uidSource = "";
        uidTarget = "";
    }
    //=======================================VOID BILL ===========================================================

    //private void LoadQuantity(int qty)
    //{
    //    if (cmbQuantity.Items.Count > 0)
    //        cmbQuantity.Items.Clear();

    //    for (int i = 0; i < qty; i++)
    //    {
    //        //cmbQuantity.Items.Add();
    //    }
    //    cmbQuantity.SelectedIndex = 0;
    //}
    protected void cmbTable_OnSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {

        if (!(cmbTable.Text.ToString() == "--Select--"))
        {
            if (string.IsNullOrEmpty(cmbTable.SelectedValue.ToString().Trim()))
            {

            }
            else
            {
                RadGridvoidbill.DataSource = DataSourceHelper();
                RadGridvoidbill.Rebind();
            }
        }
    }

    protected void RadGridvoidbill_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            if (cmbTable.SelectedValue == "")
            {
            }
            else
            {
                RadGridvoidbill.DataSource = DataSourceHelper();
            }

        }
        catch (Exception ex)
        {
            ShowMessage("Data does not exists", "Red");
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "VOID", "NeedDataSource", ex.ToString(), "Audit");
        }
    }
    public DataTable DataSourceHelper()
    {
        SqlConnection conn = BusinessTier.getConnection();
        // conn.Close();
        conn.Open();

        DateTime dtpckrselect1 = Convert.ToDateTime(DatePickerselect.SelectedDate);
        string sql = "select  DetailID,Name,Qty,Price, Table_ID,TableNO FROM VW_OrderDetailTable where deleted=0 and status='P' and table_ID='" + cmbTable.SelectedValue + "' and REPLACE(CONVERT(VARCHAR,OrdDate,106),' ','/') = '" + dtpckrselect1.ToString("dd/MMM/yyyy ") + "'";
        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sql, conn);
        DataTable g_datatable = new DataTable();
        sqlDataAdapter.Fill(g_datatable);
        BusinessTier.DisposeAdapter(sqlDataAdapter);
        BusinessTier.DisposeConnection(conn);
        return g_datatable;
    }
    public static decimal round5cen(double tax1)
    {
        double rem = (tax1 % .05);
        if (rem >= .025)
            return (decimal)(tax1 + .05 - rem);
        else
            return (decimal)(tax1 - rem);
    }
    protected void RadGridvoidbill_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        int selctqty = 0;
        Double ordPrice, tax, totPrice, tottax, gst, totaladj, tot,menutax;
        SqlConnection conn = BusinessTier.getConnection();
        try
        {
            if (Session["user_group"].ToString().Trim() == "4")
            {

                string ID = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["DetailID"].ToString();
                //  selctqty = Convert.ToInt32(BusinessTier.GettotQty(ID));
                conn.Open();
                string sql1 = "select Qty, OrderID,menuID from dbo.ORDERDETAIL  where deleted=0 and DetailID= '" + ID + "'";
                SqlCommand command = new SqlCommand(sql1, conn);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    selctqty = Convert.ToInt32(reader["Qty"].ToString());
                    OrderID = reader["OrderID"].ToString();
                    menuID = reader["menuID"].ToString();
                    //strVesselCategory = reader["Vessel_Category"].ToString();
                    //strLocation = reader["Vessel_Location"].ToString();
                }
                reader.Close();
                // BusinessTier.DisposeReader(reader);
                // BusinessTier.DisposeConnection(conn);
                if ((string.IsNullOrEmpty(txtQty.Text.ToString())))
                {
                    lblStatus.Text = "Please Select the Qty";
                    return;
                }
                else if (selctqty == Convert.ToInt32(txtQty.Text.ToString()))
                {
                    ordPrice = BusinessTier.GetOrderPrice(OrderID) - Convert.ToDouble(BusinessTier.GetOrderDetailPrice(ID));
                    tottax = BusinessTier.GetTotTax(OrderID);
                    tax = BusinessTier.GetTax(ID);

                    tax = tottax - tax;


                    // double taxOrg = tax;
                    // tax = BusinessTier.RoundUp(tax, 2);
                    // string taxAdj = (tax - taxOrg).ToString("0.00");
                    // BusinessTier.UpdateTaxAdjustment(taxAdj, taxOrg.ToString("0.00"), OrderID);
                    //  totPrice = Convert.ToDouble(ordPrice) + Convert.ToDouble(tax);
                    double Ded = BusinessTier.GetDeductionFromOrderDetail(ID);
                    double TotDed = BusinessTier.GetDeductionFromOrderMaster(OrderID);
                    TotDed = TotDed - Ded;
                    gst = Convert.ToDouble(ordPrice) - TotDed;
                    gst = gst * 6 / 100;
                    //gst = decimal.Round(gst, 2, MidpointRounding.ToEven);
                    totPrice = Convert.ToDouble(ordPrice) + gst + tax;
                    tot = totPrice;

                    double rem = (totPrice % .05);
                    if (rem >= .025)
                        totPrice = (totPrice + .05 - rem);
                    else
                        totPrice = (totPrice - rem);


                    //totPrice = round5cen(totPrice));
                    totaladj = tot - totPrice;
                    BusinessTier.UpdateDeductionInOrderMaster(OrderID, TotDed.ToString("0.00"));
                    if (totPrice != 0)
                    {
                        string sql = "Update dbo.orderMaster Set OrdPrice = " + ordPrice.ToString() + ", Tax = " + tax.ToString() + ",GST=" + gst.ToString() + ",totaladj="+ totaladj.ToString() +", TotalPrice = " + totPrice.ToString() + ", Status = 'P',Modified_Date='" + DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss") + "',Modified_By='" + Session["sesUserID"].ToString() + "' where OrderID = " + OrderID;
                        BusinessTier.ExecuteInsertUpdateQry("Update dbo.orderDetail Set Deleted='1',Modified_Date='" + DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss") + "',Modified_By='" + Session["sesUserID"].ToString() + "' Where DetailID = " + ID);
                        BusinessTier.ExecuteInsertUpdateQry(sql);
                    }
                    if (totPrice == 0)
                    {
                        // BusinessTier.ExecuteInsertUpdateQry("Update dbo.orderMaster Set OrdPrice = " + ordPrice.ToString() + ", Tax = " + tax.ToString() + ", TotalPrice = " + totPrice.ToString() + ", Status = 'D', OrdDate = '" + DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss") + "' where OrderID = " + OrderID);
                        //sql = "Update dbo.orderMaster Set OrdPrice = " + ordPrice.ToString() + ", Tax = " + tax.ToString() + ", TotalPrice = " + totPrice.ToString() + ", Status = 'D', OrdDate = '" + DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss") + "' where OrderID = " + OrderID;
                        BusinessTier.ExecuteInsertUpdateQry("Update dbo.orderDetail Set Deleted='1',Modified_Date='" + DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss") + "',Modified_By='" + Session["sesUserID"].ToString() + "' Where DetailID = " + ID);
                        BusinessTier.ExecuteInsertUpdateQry("Update dbo.orderMaster Set Deleted='1',Modified_Date='" + DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss") + "',Modified_By='" + Session["sesUserID"].ToString() + "' Where OrderID = " + OrderID);
                    }
                }
                else if (selctqty >= Convert.ToInt32(txtQty.Text.ToString()))
                {
                    // conn.Open();
                    Double Qty = Convert.ToDouble(txtQty.Text);
                    double orgQty = Convert.ToDouble(selctqty) - Qty;
                    Double unitPrice = Convert.ToDouble(BusinessTier.GetUnitPrice(menuID));
                    double deduction = BusinessTier.GetDiscount(menuID, (unitPrice * Qty));
                    deduction = BusinessTier.RoundUp(deduction, 2);
                    Double TotDed = deduction;
                    //  ordPrice = BusinessTier.GetOrderPrice(OrderID) - (unitPrice * orgQty);
                    ordPrice = BusinessTier.GetOrderPrice(OrderID);
                    ordPrice = ordPrice - (unitPrice * Qty);
                    // tax = BusinessTier.GetTax(OrderID, ordPrice);
                    
                    menutax = BusinessTier.GetMenuTax(menuID, orgQty);
                    string sqlD = "Update dbo.orderDetail Set Price = " + (unitPrice * orgQty) + ",diningcharge="+ menutax +" ,Qty = " + orgQty.ToString() + ", Deduction = " + deduction.ToString() + ",Modified_Date='" + DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss") + "',Modified_By='" + Session["sesUserID"].ToString() + "' Where DetailID = " + ID;
                    BusinessTier.ExecuteInsertUpdateQry(sqlD);
                    tottax = BusinessTier.GetSumTax(ID);
                    //tax = BusinessTier.GetTax(ID);
                    
                    //tax = tottax - (tax * Qty);

                    gst = Convert.ToDouble(ordPrice) - TotDed;
                    gst = gst * 6 / 100;
                    
                    totPrice = Convert.ToDouble(ordPrice) + gst + tottax;
                    tot = totPrice;

                    double rem = (totPrice % .05);
                    if (rem >= .025)
                        totPrice = (totPrice + .05 - rem);
                    else
                        totPrice = (totPrice - rem);


                    
                    totaladj = tot - totPrice;


                    BusinessTier.UpdateDeductionInOrderMaster(OrderID, TotDed.ToString("0.00"));
                    //double taxOrg = tax;
                    //tax = BusinessTier.RoundUp(tax, 2);
                    //string taxAdj = (tax - taxOrg).ToString("0.00");
                    //BusinessTier.UpdateTaxAdjustment(taxAdj, taxOrg.ToString("0.00"), OrderID);
                    //totPrice = Convert.ToDouble(ordPrice) + Convert.ToDouble(tax) - Convert.ToDouble(TotDed);
                    string sql = "Update dbo.orderMaster Set OrdPrice = " + ordPrice.ToString() + ", Tax = " + tottax.ToString() + ",GST=" + gst.ToString() + ",totaladj=" + totaladj.ToString() + ", TotalPrice = " + totPrice.ToString() + ", Status = 'P', Modified_Date='" + DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss") + "',Modified_By='" + Session["sesUserID"].ToString() + "' where OrderID = " + OrderID;
               ////     string sqlD = "Update dbo.orderDetail Set Price = " + (unitPrice * orgQty) + ", Qty = " + orgQty.ToString() + ", Deduction = " + deduction.ToString() + ",Modified_Date='" + DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss") + "',Modified_By='" + Session["sesUserID"].ToString() + "' Where DetailID = " + ID;
                    BusinessTier.ExecuteInsertUpdateQry(sql);
                    
                }
                //SqlConnection conn = BusinessTier.getConnection();
                //conn.Open();
                //string strupdate = "UPDATE [ORDERDETAIL] SET [Deleted]=1,[Modified_By] ='" + Session["sesUserID"].ToString().Trim() + "', [Modified_date]='" + System.DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss ") + "' WHERE [DetailID] = '" + ID + "'";
                //SqlCommand cmd = new SqlCommand(strupdate, conn);
                //cmd.ExecuteNonQuery();
                //BusinessTier.DisposeConnection(conn);
                //ShowMessage("Deleted Successfully", "Yellow");
                lblStatus.Text = "Deleted Successfully";
                RadGridvoidbill.Rebind();
                loadtable();
            }
            else
            {
                BusinessTier.DisposeConnection(conn);
                lblStatus.Text = "Please Contact Your Supervisor";
            }
        }

        catch (Exception ex)
        {
            ShowMessage("Unable to delete, Please try again/Contact your administrator", "Red");
            e.Canceled = true;
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "VOID", "Delete", ex.ToString(), "Audit");
        }
    }
       
    protected void btnVoid_Click(object sender, EventArgs e)
    {
        if (cmbTable.SelectedIndex >= 0)
        {
            //  VoidItem();
        }
        else
        {
            lblStatus.Text = "Please Selct the Table to Void....";
            cmbTable.Focus();
        }
    }
    //protected void btnEmail1_Click(object sender, EventArgs e)
    //{


    //    try
    //    {

    //        Process p = new Process();
    //        //// p.StartInfo.FileName = "D:\\yourapplication.exe";
    //        //p.StartInfo.FileName = "C:\\Windows\\System32\\notepad.exe";
    //        //p.Start();

    //        //p.StartInfo.WorkingDirectory = Request.MapPath("~/");
    //        //// Set the filename name of the file you want to open
    //        //p.StartInfo.FileName = Request.MapPath("WindowsMediaPlayer.exe");
    //        //p.Start(); 
    //        ProcessStartInfo processStartInfo = new ProcessStartInfo();
    //        processStartInfo.FileName = @"c:\WINDOWS\system32\notepad.exe";
    //        processStartInfo.Arguments = @"c:\Download.txt";
    //        Process.Start(processStartInfo);



    //    }
    //    catch (Exception ex)
    //    {
    //        ShowMessage("An error occured while trying to generate report, Please try again/Contact your administrator", "Red");
    //        // lblStatus.Text = "An error occured while trying to generate report.";
    //    }

    //}

    private void ShowMessage(string message, string color)
    {
        lblStatus.Text = message.ToString();
        System.Drawing.ColorConverter colConvert = new ColorConverter();
        string strColor = color.ToString();
        lblStatus.ForeColor = (System.Drawing.Color)colConvert.ConvertFromString(strColor);
    }


    private void InsertLogAuditTrail(string userid, string module, string activity, string result, string flag)
    {
        SqlConnection connLog = BusinessTier.getConnection();
        connLog.Open();
        BusinessTier.InsertLogAuditTrial(connLog, userid, module, activity, result, flag);
        BusinessTier.DisposeConnection(connLog);
    }
}
