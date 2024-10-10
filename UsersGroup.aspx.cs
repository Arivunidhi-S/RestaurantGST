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
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.OleDb;
using System.Drawing;

public partial class UsersGroup : System.Web.UI.Page
{
    public static string g_strSaveFlag;
    public static string g_strUserGroupID;
    public static string g_strUserGroupAID;
    

    //public DataTable dtMenuItems = new DataTable();
    //public DataTable dtSubMenuItems = new DataTable();

    protected void Page_Init(object sender, EventArgs e)
    {
        Session["sesUserID"] = "1";
        Session["user_group"] = "1";
        try
        {
            if (!(string.IsNullOrEmpty(Session["sesUserID"].ToString())))
            {
                if (!(IsPostBack))
                {
                    clearFields();
                    g_strSaveFlag = "Insert";
                    g_strUserGroupID = "0";
                    g_strUserGroupAID = "0";
                    FillListBox_Module_InsertMode();
                    lblStatus.Text = "";
                }

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
    }

    protected void Page_Load(object sender, EventArgs e)
    {
       // Session["sesUserID"] = "1";
        try
        {

            if ((string.IsNullOrEmpty(Session["sesUserID"].ToString())))
            {
                Response.Redirect("Login.aspx");
            }



        }
        catch (Exception ex1)
        {
            Response.Redirect("Login.aspx");

        }
    }

    //protected void cboUserType_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (string.IsNullOrEmpty(g_strSaveFlag.ToString()))
    //    {
    //        g_strSaveFlag = "Insert";
    //    }
    //    if (g_strSaveFlag.ToString() == "Insert")
    //    {
    //        //ValidateUserType();
    //        TabContainer1.Tabs[1].Enabled = false;
    //    }
    //}

    //protected void cboPlatform_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (string.IsNullOrEmpty(g_strSaveFlag.ToString()))
    //    {
    //        g_strSaveFlag = "Insert";
    //    }

    //    if (g_strSaveFlag.ToString() == "Insert")
    //    {
    //        string strUserType = cboUserType.Text.ToString();

    //        if (strUserType.ToString() == "Platform")
    //        {
    //            if (chkApprovalRqrd.Checked)
    //            {
    //                if (!(cboPlatform.Text.ToString() == "Choose"))
    //                {
    //                    FillListBox_Approver_InsertMode(cboUserType.Text.ToString(), cboPlatform.SelectedValue.ToString());
    //                }
    //                TabContainer1.Tabs[1].Enabled = true;
    //            }
    //            else
    //            {
    //                TabContainer1.Tabs[1].Enabled = false;
    //            }
    //        }
    //    }
    //}

    //protected void chkAppRqrd_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (string.IsNullOrEmpty(g_strSaveFlag.ToString()))
    //    {
    //        g_strSaveFlag = "Insert";
    //    }
    //    string strUserType = cboUserType.Text.ToString();

    //    if (strUserType.ToString() == "Platform")
    //    {
    //        if (chkApprovalRqrd.Checked)
    //        {
    //            if (!(cboPlatform.Text.ToString() == "Choose"))
    //            {
    //                FillListBox_Approver_InsertMode(cboUserType.Text.ToString(), cboPlatform.SelectedValue.ToString());
    //            }
    //            TabContainer1.Tabs[1].Enabled = true;
    //        }
    //        else
    //        {
    //            TabContainer1.Tabs[1].Enabled = false;
    //        }
    //    }
    //}

    //protected void ValidateUserType()
    //{
    //    string strUserType = cboUserType.Text.ToString();

    //    if (strUserType.ToString() == "Platform")
    //    {
    //        cboPlatform.Visible = true;
    //        chkApprovalRqrd.Checked = false;
    //        chkApprovalRqrd.Visible = true;
    //        lblChkAppRqrd.Visible = true;
    //        if (chkApprovalRqrd.Checked)
    //        {
    //            TabContainer1.Tabs[1].Enabled = true;
    //            FillListBox_Approver_InsertMode(cboUserType.Text.ToString(), cboPlatform.SelectedValue.ToString());
    //        }
    //        else
    //        {
    //            TabContainer1.Tabs[1].Enabled = false;
    //        }

    //    }
    //    if (strUserType.ToString() == "KSB")
    //    {
    //        cboPlatform.Visible = false;
    //        chkApprovalRqrd.Checked = false;
    //        chkApprovalRqrd.Visible = false;
    //        lblChkAppRqrd.Visible = false;

    //        TabContainer1.Tabs[1].Enabled = false;
    //    }

    //}


    protected void clearFields()
    {

        txtUserGroupID.Text = "";
        txtDesc.Text = "";
       
         TabContainer1.Tabs[1].Enabled = true;
       
    }

    protected void btnClearItem_Click(object source, EventArgs e)
    {
        clearFields();
        g_strSaveFlag = "Insert";
        g_strUserGroupID = "0";
        //g_strUserID = "0";
        //g_strLoginID = "0";
        //g_strIsApproverAdded = "N";
        FillListBox_Module_InsertMode();
        lblStatus.Text = "";
    }

    protected void btnRegister_Click(object sender, EventArgs e)
    {
        try
        {
            int intSaveValidationFlag = 0;
            if (!(string.IsNullOrEmpty(g_strSaveFlag.ToString())))
            {
                if (g_strSaveFlag.ToString() == "Update")
                {
                    if (g_strUserGroupID.ToString() == txtUserGroupID.Text.ToString())
                    {
                        intSaveValidationFlag = 2;
                    }
                    else
                    {
                        intSaveValidationFlag = 1;
                    }
                }
                if (g_strSaveFlag.ToString() == "Insert")
                {
                    intSaveValidationFlag = 1;
                }

                if (intSaveValidationFlag == 1)
                {
                    string strCheckflag = "0";
                    SqlConnection connCheck = BusinessTier.getConnection();
                    connCheck.Open();
                    SqlDataReader reader = BusinessTier.checkUserGroupId(connCheck, txtUserGroupID.Text.ToString());
                    if (reader.Read())
                    {
                        strCheckflag = reader["checkdup"].ToString();
                    }
                    BusinessTier.DisposeReader(reader);
                    BusinessTier.DisposeConnection(connCheck);

                    if (strCheckflag.ToString() == "1")
                    {
                        //ShowMessage(13);
                        return;
                    }
                }

                //string strPlatformid = "0";
                //string strAppRqrd = "N";
                //string strNotifyrqrd = "N";
                //if (g_strSaveFlag.ToString() == "Insert")
                //{
                //    if (cboUserType.Text.ToString() == "Platform")
                //    {
                //        if (cboPlatform.Text.ToString() == "Choose")
                //        {
                //          //  ShowMessage(12);
                //            return;
                //        }
                //        else
                //        {
                //            strPlatformid = cboPlatform.SelectedValue.ToString();
                //        }
                //    }

                //    if (chkApprovalRqrd.Checked)
                //    {
                //        strAppRqrd = "Y";
                //    }
                //    if (chkNotifyRqrd.Checked)
                //    {
                //        strNotifyrqrd = "Y";
                //    }

                //    if (string.IsNullOrEmpty(strPlatformid.ToString()))
                //    {
                //        strPlatformid = "0";
                //    }
                //}
                SqlConnection connSave = BusinessTier.getConnection();
                connSave.Open();
                int intSavedStatusflag = 0;
                //Int64 intplatformId = Int64.Parse(strPlatformid.ToString());
                //intSavedStatusflag = BusinessTier.SaveUserMaster(connSave, txtUserID.Text.ToString(), txtPass.Text.ToString(), txtDesignation.Text.ToString(), txtName.Text.ToString(), txtDept.Text.ToString(), txtEmail.Text.ToString(), cboUserType.Text.ToString(), intplatformId, strAppRqrd.ToString(), strNotifyrqrd.ToString(), Session["sesUserID"].ToString(), g_strSaveFlag, g_strUserID.ToString());

                intSavedStatusflag = BusinessTier.SaveUserGroup(connSave, txtUserGroupID.Text.ToString(), txtDesc.Text.ToString(), g_strSaveFlag, g_strUserGroupAID.ToString());
                
                BusinessTier.DisposeConnection(connSave);
                if (intSavedStatusflag == 1)
                {
                    if (g_strSaveFlag.ToString() == "Insert")
                    {
                        //ShowMessage(48);
                    }

                    if (g_strSaveFlag.ToString() == "Update")
                    {
                        //ShowMessage(50);
                    }

                    //if (TabContainer1.Tabs[1].Enabled == true)
                    //{
                    //    SaveApproverList();
                    //}
                    if (TabContainer1.Tabs[1].Enabled == true)
                    {
                        SaveMudulePermissionList();
                    }

                    RadGrid1.DataSource = DataSourceHelper();

                    clearFields();
                    g_strSaveFlag = "Insert";
                    g_strUserGroupID = "0";
                    g_strUserGroupAID = "0";
                    //g_strLoginID = "0";
                    //g_strIsApproverAdded = "N";
                    FillListBox_Module_InsertMode();
                }
            }

            //InsertLogAuditTrial is used to insert the action into MasterLog table
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "UserGroup_tbl", "btnRegister", "Success", "Log");
        }
        catch (Exception ex)
        {
            //ShowMessage(15);
            //InsertLogAuditTrial is used to insert the action into MasterAuditTrail table
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "UserGroup_tbl", "btnRegister", ex.ToString(), "Audit");
        }

    }

    
    protected void SaveMudulePermissionList()
    {
        string strUserGroupid = txtUserGroupID.Text.ToString();
        string strgetId = "0";
        string strgetAId = "0";
        
        SqlConnection connCheck = BusinessTier.getConnection();
        connCheck.Open();
        SqlDataReader reader = BusinessTier.getUserGroupById(connCheck, strUserGroupid.ToString());
        if (reader.Read())
        {
            strgetId = reader["group_id"].ToString();
            strgetAId = reader["group_aid"].ToString();
        }
        BusinessTier.DisposeReader(reader);
        BusinessTier.DisposeConnection(connCheck);

        if (string.IsNullOrEmpty(strgetId.ToString()))
        {
            if (strgetId.ToString() == "0")
            {
                //ShowMessage(14);
                return;
            }
        }
        
        
        if (g_strSaveFlag.ToString() == "Update")
        {
            if (strUserGroupid.ToString() == strgetId.ToString())
            {
                DeleteMasterModulePermission(strgetAId.ToString());
            }
            else
            {
                //ShowMessage(14);
                return;
            }
        }

        Int64 intgroupid = Int64.Parse(strgetAId.ToString());
        
        int intListbox2_count = listboxModule2.Items.Count;
        SqlConnection connect = BusinessTier.getConnection();
        connect.Open();
        for (int t = 0; t < intListbox2_count; t++)
        {
            Int64 intlinebyline = Convert.ToInt64(listboxModule2.Items[t].Value);

            SqlDataReader reader1 = BusinessTier.getMasterModuleById(connect, listboxModule2.Items[t].Value.ToString());
            //if (reader1.Read())
            //{
            //    strAppRqrd_Module = reader1["ApprovalNeeded"].ToString();
            //}
            BusinessTier.DisposeReader(reader1);

        
            //if (strAppRqrd_ModuleValidateFlag1.ToString() == "1")
            //{
                SqlConnection connSave = BusinessTier.getConnection();
                connSave.Open();
                int intSaveflag = BusinessTier.SaveUserGroupModulePermission(connSave, intgroupid, intlinebyline, Session["sesUserID"].ToString());
                BusinessTier.DisposeConnection(connSave);
                //strAppRqrd_ModuleValidateFlag1 = "0";
            //}
        }
        BusinessTier.DisposeConnection(connect);
        //if (strAppRqrd_ModuleValidateFlag.ToString() == "1")
        //{
        //    ShowMessage(15);
        //    return;
        //}
    }

    protected void DeleteMasterModulePermission(string strGAId)
    {
        SqlConnection dbConn = BusinessTier.getConnection();
        dbConn.Open();
        SqlDataAdapter sqlDelete = new SqlDataAdapter();
        sqlDelete.DeleteCommand = dbConn.CreateCommand();
        sqlDelete.DeleteCommand.CommandText = "Delete from UserGroupModulePermission where group_aid = '" + strGAId.ToString() + "'";
        sqlDelete.DeleteCommand.ExecuteNonQuery();
    }

    protected void RadGrid1_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName == "RowClick" && e.Item is GridDataItem)
        {
            e.Item.Selected = true;
            string strTakenId = (e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["group_aid"]).ToString();
            try
            {
                clearFields();
                int intflagTakenUserType = 0;
                //string strPlatformid = "0";
                //string strUserType = "";
                //string strAppRqrd = "";
                g_strUserGroupAID = strTakenId.ToString();   //Assign userID to class variable for update.
                SqlConnection conn = BusinessTier.getConnection();
                conn.Open();
                SqlDataReader reader = BusinessTier.getUserGroupByAID(conn, g_strUserGroupAID);
                while (reader.Read())
                {
                    txtUserGroupID.Text = (reader["group_id"].ToString());
                    txtDesc.Text = (reader["Description"].ToString());

                    g_strUserGroupID = (reader["group_id"]).ToString();  //Assign to Class variable to edit the LoginID
                    //string strNotifyrqrd = (reader["IsNotifyRqrd"].ToString());

                    
                }
                BusinessTier.DisposeReader(reader);
                BusinessTier.DisposeConnection(conn);

                //listboxModule1.Visible = false;
                //listboxModule2.Visible = false;

              

                FillListBox_Module_UpdateMode(g_strUserGroupAID);
                g_strSaveFlag = "Update"; //Assign to Class Variable
               // TabContainer1.ActiveTab = TabContainer1.Tabs[3];
            }
            catch (Exception ex)
            {
                //InsertLogAuditTrial is used to insert the action into MasterAuditTrail table
                InsertLogAuditTrail(Session["sesUserID"].ToString(), "UserGroup_tbl", "LoadValuesFromGrid", ex.ToString(), "Audit");
                //ShowMessage(8);
            }
        }
    }

   
    protected void FillListBox_Module_InsertMode()
    {
        listboxModule1.Items.Clear();
        listboxModule2.Items.Clear();

        SqlConnection conn = BusinessTier.getConnection();
        conn.Open();
        SqlDataReader reader = BusinessTier.getMasterModule(conn);
        while (reader.Read())
        {
            string strname = (reader["ModuleName"].ToString());
            string strid = (reader["ModuleId"].ToString());
            listboxModule1.Items.Add(new RadListBoxItem(strname.ToString(), strid.ToString()));

        }
        BusinessTier.DisposeReader(reader);
        BusinessTier.DisposeConnection(conn);
    }

    protected void FillListBox_Module_UpdateMode(string strUserGroupAId)
    {
        listboxModule1.Items.Clear();
        listboxModule2.Items.Clear();

        SqlConnection connModulePermission = BusinessTier.getConnection();
        connModulePermission.Open();
        SqlDataReader readerModulePermission = BusinessTier.getMasterModulePermisnByUserGroupId(connModulePermission, strUserGroupAId);
        while (readerModulePermission.Read())
        {
            string strname = (readerModulePermission["ModuleName"].ToString());
            string strid = (readerModulePermission["ModuleId"].ToString());
            listboxModule2.Items.Add(new RadListBoxItem(strname.ToString(), strid.ToString()));
        }
        BusinessTier.DisposeReader(readerModulePermission);
        BusinessTier.DisposeConnection(connModulePermission);

        SqlConnection conn = BusinessTier.getConnection();
        conn.Open();
        SqlDataReader reader = BusinessTier.getMasterModule(conn);
        while (reader.Read())
        {
            string strname = (reader["ModuleName"].ToString());
            string strid = (reader["ModuleId"].ToString());
            if (listboxModule2.Items.Count == 0)
            {
                listboxModule1.Items.Add(new RadListBoxItem(strname.ToString(), strid.ToString()));
            }
            else
            {
                int flagValidate = 0;
                for (int t = 0; t < listboxModule2.Items.Count; t++)
                {
                    string strModule2Id = (listboxModule2.Items[t].Value).ToString();
                    if ((strModule2Id.ToString() == strid.ToString()))
                    {
                        flagValidate = 1;
                    }
                }
                if (flagValidate == 0)
                {
                    listboxModule1.Items.Add(new RadListBoxItem(strname.ToString(), strid.ToString()));
                }

            }

        }
        BusinessTier.DisposeReader(reader);
        BusinessTier.DisposeConnection(conn);
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

            
            //InsertLogAuditTrial is used to insert the action into MasterAuditTrail table
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "UserGroup_tbl", "NeedDataSource", ex.ToString(), "Audit");
        }
    }

    protected void RadGrid1_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            string ID = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["group_aid"].ToString();
            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();
            int flg = BusinessTier.DeleteUserGroupGrid(conn, ID.ToString());
            BusinessTier.DisposeConnection(conn);
            if (flg >= 1)
            {
                //ShowMessage(3);
                ShowMessage("Deleted Successfully", "Yellow");

            }
            //InsertLogAuditTrial is used to insert the action into MasterLog table
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "UserGroup_tbl", "Delete", "Success", "Log");
        }
        catch (Exception ex)
        {
            //ShowMessage(7);
            ShowMessage("Can't Perform Deletion", "Red");
            e.Canceled = true;
            //InsertLogAuditTrial is used to insert the action into MasterAuditTrail table
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "UserGroup_tbl", "Delete", ex.ToString(), "Audit");
        }
    }

    public DataTable DataSourceHelper()
    {
        SqlConnection conn = BusinessTier.getConnection();
        conn.Open();
        int delval = 0;
        string sql = "select group_aid, group_id, access_level, description FROM UserGroup_tbl WHERE Deleted='" + delval + "' order by group_id";
        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sql, conn);
        DataTable g_datatable = new DataTable();
        sqlDataAdapter.Fill(g_datatable);
        BusinessTier.DisposeAdapter(sqlDataAdapter);
        BusinessTier.DisposeConnection(conn);
        return g_datatable;
    }

    
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