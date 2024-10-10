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

public partial class MasterModule : System.Web.UI.Page
{
    //public DataTable dtMenuItems = new DataTable();
    //public DataTable dtSubMenuItems = new DataTable();

    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            if (!(string.IsNullOrEmpty(Session["sesUserID"].ToString())))
            {
                //SqlConnection connMenu = BusinessTier.getConnection();
                //connMenu.Open();
                //SqlDataReader readerMenu = BusinessTier.getMenuList(connMenu);
                //dtMenuItems.Load(readerMenu);
                //BusinessTier.DisposeReader(readerMenu);
                //BusinessTier.DisposeConnection(connMenu);
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
                lblStatus.Text = "";
            }
        }
        catch (Exception ex)
        {
            Response.Redirect("Login.aspx");
        }
    }

    protected void RadGrid1_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                GridEditFormItem editedItem = e.Item as GridEditFormItem;

                TextBox txtname = editedItem.FindControl("txtName") as TextBox;
                TextBoxSetting textBoxSetting = (TextBoxSetting)RadInputManager1.GetSettingByBehaviorID("TextBoxBehavior1");
                textBoxSetting.TargetControls.Add(new TargetInput(txtname.UniqueID, true));

                TextBox txtdesc = editedItem.FindControl("txtDesc") as TextBox;
                TextBoxSetting textBoxSetting1 = (TextBoxSetting)RadInputManager1.GetSettingByBehaviorID("TextBoxBehavior1");
                textBoxSetting1.TargetControls.Add(new TargetInput(txtdesc.UniqueID, true));

            }

        }
        catch (Exception ex)
        {
            //ShowMessage(10);
            ShowMessage("Please provide required details which are highlighted","Yellow");
            e.Canceled = true;
            //InsertLogAuditTrial is used to insert the action into MasterAuditTrail table
            //InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterModule", "RadGrid1_ItemCreated", ex.ToString(), "Audit");
        }
    }


    protected void RadGrid1_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            RadGrid1.DataSource = DataSourceHelper();
        }
        catch (Exception ex)
        {
            //ShowMessage(9);
            ShowMessage("Data does not exists","Red");
            //InsertLogAuditTrial is used to insert the action into MasterAuditTrail table
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterModule", "NeedDataSource", ex.ToString(), "Audit");
        }
    }

    public DataTable DataSourceHelper()
    {
        int delval = 0;
        string sql = "";
        SqlConnection conn = BusinessTier.getConnection();
        conn.Open();
        sql = "select ModuleId,ModuleName,CodeFileName,Description,ApprovalNeeded,Createdby FROM MasterModule WHERE Deleted='" + delval + "' order by ModuleName";
        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sql, conn);
        DataTable g_datatable = new DataTable();
        sqlDataAdapter.Fill(g_datatable);
        BusinessTier.DisposeAdapter(sqlDataAdapter);
        BusinessTier.DisposeConnection(conn);
        return g_datatable;
    }

    protected void RadGrid1_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            string ID = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ModuleId"].ToString();
            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();
            int flg = BusinessTier.DeleteModuleGrid(conn, ID.ToString());
            BusinessTier.DisposeConnection(conn);
            if (flg >= 1)
            {
                //ShowMessage(3);
                ShowMessage("Deleted Successfully", "Yellow");
                InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterModule", "Delete", "Success", "Log");
            }
            //InsertLogAuditTrial is used to insert the action into MasterLog table
            else
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterModule", "Delete", "Failed", "Log");
        }
        catch (Exception ex)
        {
            //ShowMessage(7);
            ShowMessage("Unable to delete, Please try again/Contact your administrator", "Red");
            e.Canceled = true;
            //InsertLogAuditTrial is used to insert the action into MasterAuditTrail table
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterModule", "Delete", ex.ToString(), "Audit");
        }
    }


    protected void RadGrid1_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            DataTable m_datatable = DataSourceHelper();

            DataRow newRow = m_datatable.NewRow();

            Hashtable newValues = new Hashtable();
            e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);


            foreach (DictionaryEntry entry in newValues)
            {
                newRow[(string)entry.Key] = entry.Value;
            }

            string strCheckflag = "0";
            SqlConnection connCheck = BusinessTier.getConnection();
            connCheck.Open();
            SqlDataReader reader = BusinessTier.checkModuleName(connCheck, newRow["ModuleName"].ToString());
            if (reader.Read())
            {
                strCheckflag = reader["checkdup"].ToString();

            }
            BusinessTier.DisposeReader(reader);
            BusinessTier.DisposeConnection(connCheck);

            if (strCheckflag.ToString() == "1")
            {
                //ShowMessage(4);
                ShowMessage("Duplicate Entry", "Red");
                return;
            }

            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();
            int flg = BusinessTier.SaveModuleMaster(conn, newRow["ModuleName"].ToString(), newRow["CodeFileName"].ToString(), newRow["Description"].ToString(), newRow["ApprovalNeeded"].ToString(), Session["sesUserID"].ToString(), "N", "0");
            //int flg = BusinessTier.SaveModuleMaster(conn, newRow["ModuleName"].ToString(), newRow["Description"].ToString(), "N", Session["sesUserID"].ToString(), "N", "0");
            BusinessTier.DisposeConnection(conn);
            if (flg >= 1)
            {
                //ShowMessage(1);
                ShowMessage("Inserted Successfully", "Yellow");
            }
            //InsertLogAuditTrial is used to insert the action into MasterLog table
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterModule", "Insert", "Success", "Log");
        }
        catch (Exception ex)
        {
            //ShowMessage(5);
            ShowMessage("Unable to insert, Please try again/Contact your administrator", "Yellow");
            e.Canceled = true;
            //InsertLogAuditTrial is used to insert the action into MasterAuditTrail table
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterModule", "Insert", ex.ToString(), "Audit");
        }

        //m_datatable.Rows.Add(newRow);
        RadGrid1.Rebind();
    }

    protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            DataTable ordersTable = DataSourceHelper();

            DataRow[] changedRows = ordersTable.Select("ModuleId = " + editedItem["ModuleId"].Text);

            string strExistingName = changedRows[0].ItemArray.GetValue(1).ToString();

            //if (changedRows.Length != 1)
            //{
            //    ShowMessage(8);
            //    e.Canceled = true;
            //    return;
            //}

            Hashtable newValues = new Hashtable();
            e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);

            changedRows[0].BeginEdit();

            foreach (DictionaryEntry entry in newValues)
            {
                changedRows[0][(string)entry.Key] = entry.Value;
            }
            changedRows[0].EndEdit();

            string strCheckflag = "0";
            SqlConnection connCheck = BusinessTier.getConnection();
            connCheck.Open();
            SqlDataReader reader = BusinessTier.checkModuleName(connCheck, changedRows[0].ItemArray.GetValue(1).ToString());
            if (reader.Read())
            {
                strCheckflag = reader["checkdup"].ToString();

            }
            BusinessTier.DisposeReader(reader);
            BusinessTier.DisposeConnection(connCheck);

            if (strCheckflag.ToString() == "1")
            {
                if (!(strExistingName.ToString() == changedRows[0].ItemArray.GetValue(1).ToString()))
                {
                    //ShowMessage(4);
                    ShowMessage("Duplicate Entry", "Red");
                    return;
                }
            }

            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();
            int flg = BusinessTier.SaveModuleMaster(conn, changedRows[0].ItemArray.GetValue(1).ToString(), changedRows[0].ItemArray.GetValue(2).ToString(), changedRows[0].ItemArray.GetValue(3).ToString(), changedRows[0].ItemArray.GetValue(4).ToString(), Session["sesUserID"].ToString(), "U", changedRows[0].ItemArray.GetValue(0).ToString());
            BusinessTier.DisposeConnection(conn);

            if (flg >= 1)
            {
                //ShowMessage(2);
                ShowMessage("Updated Successfully", "Red");
            }

            RadGrid1.Rebind();
            changedRows[0].CancelEdit();
            //InsertLogAuditTrial is used to insert the action into MasterLog table
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterModule", "Update", "Success", "Log");
        }
        catch (Exception ex)
        {
            //ShowMessage(6);
            ShowMessage("Unable to update, Please try again/Contact your administrator", "Red");
            e.Canceled = true;
            //InsertLogAuditTrial is used to insert the action into MasterAuditTrail table
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterModule", "Update", ex.ToString(), "Audit");
        }
    }


    //protected void RadGrid1_EditCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    //{
    //    try
    //    {
    //        if (e.Item is GridEditableItem && e.Item.IsInEditMode)
    //        {

    //            DataTable ordersTable = DataSourceHelper();
    //            GridDataItem editedItem = e.Item as GridDataItem;

    //            DataRow[] changedRows = ordersTable.Select("ModuleId = " + editedItem["ModuleId"].Text);

    //            string strExistingFlag = changedRows[0].ItemArray.GetValue(3).ToString();

    //            //if (e.Item is GridDataItem)
    //            //{
    //            //    GridDataItem item = (GridDataItem)e.Item;
    //            //    if (item["Status"].Text == "Approved")
    //            //    {
    //            //        ((CheckBox)item["CheckBoxColumn"].FindControl("CheckBox1")).Enabled = false;
    //            //    }
    //            //} 

    //            CheckBox chkApproval = editedItem.FindControl("chkApproval") as CheckBox;

    //            if (strExistingFlag.ToString() == "Y")
    //            {
    //                chkApproval.Checked = true;
    //            }
    //            if (strExistingFlag.ToString() == "N")
    //            {
    //                chkApproval.Checked = false;
    //            }
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        ShowMessage(10);
    //        e.Canceled = true;
    //        //InsertLogAuditTrial is used to insert the action into MasterAuditTrail table
    //        InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterModule", "RadGrid1_EditCommand", ex.ToString(), "Audit");
    //    }
    //}


    //private void ShowMessage(int errorNo)
    //{
    //    lblStatus.Text = BusinessTier.g_ErrorMessagesDataTable.Rows[errorNo - 1]["Message"].ToString();
    //    System.Drawing.ColorConverter colConvert = new ColorConverter();
    //    string strColor = BusinessTier.g_ErrorMessagesDataTable.Rows[errorNo - 1]["Color"].ToString();
    //    lblStatus.ForeColor = (System.Drawing.Color)colConvert.ConvertFromString(strColor);
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