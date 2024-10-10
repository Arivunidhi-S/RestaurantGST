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

public partial class Vessel : System.Web.UI.Page
{


    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            if (!(string.IsNullOrEmpty(Session["sesUserID"].ToString())))
            {
          
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

                TextBox txtVesselName = editedItem.FindControl("txtVesselName") as TextBox;
                TextBoxSetting textBoxSetting = (TextBoxSetting)RadInputManager1.GetSettingByBehaviorID("TextBoxBehavior1");
                textBoxSetting.TargetControls.Add(new TargetInput(txtVesselName.UniqueID, true));



            }
        }
        catch (Exception ex)
        {
            ShowMessage("Please provide required details which are highlighted", "Yellow");
            e.Canceled = true;
            //InsertLogAuditTrial is used to insert the action into MasterAuditTrail table
            //InsertLogAuditTrail(Session["sesUserID"].ToString(), "MasterCategory", "RadGrid1_ItemCreated", ex.ToString(), "Audit");
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
            ShowMessage("Data does not exists", "Red");
            //InsertLogAuditTrial is used to insert the action into MasterAuditTrail table
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "VesselName", "NeedDataSource", ex.ToString(), "Audit");
        }
    }

    public DataTable DataSourceHelper()
    {
        SqlConnection conn = BusinessTier.getConnection();
        conn.Close();
        //string sql = "select Vessel_ID,Vessel_NAME,Description,Vessel_Type,Nature_Of_Activity,CREATED_BY FROM Vessel WHERE Deleted=0 order by Vessel_Name";
        string sql = "select * FROM Vessel WHERE Deleted=0 order by Vessel_Id Desc";

        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sql, conn);
        DataTable g_datatable = new DataTable();
        sqlDataAdapter.Fill(g_datatable);
        BusinessTier.DisposeAdapter(sqlDataAdapter);
        BusinessTier.DisposeConnection(conn);
        return g_datatable;
    }
    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {

        try
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                GridEditFormItem editedItem = e.Item as GridEditFormItem;
                Label lbl = (Label)editedItem.FindControl("lblVesselID");
                RadComboBox cboVesselType = (RadComboBox)editedItem.FindControl("cboVesselType");
              RadComboBox cbovesselcategory = (RadComboBox)editedItem.FindControl("cbovesselcategory");
              RadComboBox cboStationName = (RadComboBox)editedItem.FindControl("cboStationName");
                if (!(string.IsNullOrEmpty(lbl.Text.ToString())))
                {

                    string strVesselType = "";
                    string strVesselCategory = "";
                    string strLocation = "";

                    SqlConnection conn = BusinessTier.getConnection();
                    conn.Open();
                    string sql = "select Vessel_Type, Vessel_Category,Vessel_Location FROM Vessel WHERE Deleted = 0 and Vessel_ID = '" + lbl.Text.ToString() + "'";
                    SqlCommand command = new SqlCommand(sql, conn);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        strVesselType = reader["Vessel_Type"].ToString();
                        strVesselCategory = reader["Vessel_Category"].ToString();
                        strLocation = reader["Vessel_Location"].ToString();
                       
                    }
                    BusinessTier.DisposeReader(reader);
                    BusinessTier.DisposeConnection(conn);

                    cboVesselType.SelectedItem.Text = strVesselType.ToString();
                    cbovesselcategory.SelectedItem.Text = strVesselCategory.ToString();
                    cboStationName.Text = strLocation;
                   
                }

            }
        }
        catch (Exception ex)

        { 
        }
    //    if (e.Item is GridEditableItem && e.Item is GridEditFormInsertItem)
    //    {
    //        GridEditableItem item = (GridEditableItem)e.Item;

        
    //    }
    //    if (e.Item is GridEditFormItem && e.Item.IsInEditMode)
    //    {
    //        GridEditFormItem item = (GridEditFormItem)e.Item;
    //        RadComboBox cboVesselType = (RadComboBox)item.FindControl("cboVesselType");
    //        cboVesselType.SelectedValue = (string)DataBinder.Eval(e.Item.DataItem, "Vessel_Type").ToString();

    //    }
    }

    protected void RadGrid1_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            string ID = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Vessel_ID"].ToString().Trim();
            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();
            int flg = BusinessTier.SaveVessel(conn, Convert.ToInt32(ID), "","", "", "", "","", "","", "", "", "","","", Session["sesUserID"].ToString(), "D");
            BusinessTier.DisposeConnection(conn);
            if (flg >= 1)
            {
                ShowMessage("Deleted Successfully", "Red");
            }
            //InsertLogAuditTrial is used to insert the action into MasterLog table
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "VesselName", "Delete", "Success", "Log");
        }
        catch (Exception ex)
        {
            ShowMessage("Unable to delete, Please try again/Contact your administrator", "Red");
            e.Canceled = true;
            //InsertLogAuditTrial is used to insert the action into MasterAuditTrail table
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "VesselName", "Delete", ex.ToString(), "Audit");
        }
    }

    protected void RadGrid1_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            TextBox txtVesselName = (TextBox)editedItem.FindControl("txtVesselName");
            TextBox txtDescription = (TextBox)editedItem.FindControl("txtDescription");

            RadComboBox cboVesselType = (RadComboBox)editedItem.FindControl("cboVesselType");
            TextBox txtVesselSize = (TextBox)editedItem.FindControl("txtVesselSize");
            TextBox txtNatureofActivity = (TextBox)editedItem.FindControl("txtNatureofActivity");
            TextBox txtOwnerName = (TextBox)editedItem.FindControl("txtOwnerName");
            TextBox txtCompanyName = (TextBox)editedItem.FindControl("txtCompanyName");
            TextBox txtDesignation = (TextBox)editedItem.FindControl("txtDesignation");
            TextBox txtContactNumber = (TextBox)editedItem.FindControl("txtContactNumber");

            TextBox txtMaterialman = (TextBox)editedItem.FindControl("txtMaterialman");
            TextBox txtMaterialcontno = (TextBox)editedItem.FindControl("txtMaterialcontno");
            RadComboBox cbovesselcategory = (RadComboBox)editedItem.FindControl("cbovesselcategory");
            RadComboBox cboStationName = (RadComboBox)editedItem.FindControl("cboStationName");

            string strCheckflag = "0";
            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();
            string strsql = "select count(*) as checkdup from Vessel where Vessel_Name = '" + txtVesselName.Text.ToString().Trim() + "' and deleted = 0 ";
             SqlCommand cmd = new SqlCommand(strsql, conn);
             SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                strCheckflag = reader["checkdup"].ToString();

            }
            BusinessTier.DisposeReader(reader);
            BusinessTier.DisposeConnection(conn);

            if (strCheckflag.ToString() == "1")
            {
                //ShowMessage(4);
                ShowMessage("Unable to insert,Vessel Name Inserted Already", "Yellow");
                return;                
              
            }
            else
            {
                SqlConnection conn1 = BusinessTier.getConnection();
                conn1.Open();
                int flg = BusinessTier.SaveVessel(conn1, 1, txtVesselName.Text.ToString().Trim(),txtVesselSize.Text.ToString().Trim(), txtDescription.Text.ToString().Trim(),
                    cboVesselType.SelectedItem.Text.ToString().Trim(), cbovesselcategory.SelectedItem.Text.ToString().Trim(), cboStationName.Text.ToString().Trim(), txtNatureofActivity.Text.ToString().Trim(), 
                    txtOwnerName.Text.ToString().Trim(), txtCompanyName.Text.ToString().Trim(), txtDesignation.Text.ToString().Trim(),
                    txtContactNumber.Text.ToString().Trim(), txtMaterialman.Text.ToString().Trim(),txtMaterialcontno.Text.ToString().Trim(), Session["sesUserID"].ToString(), "N");
                BusinessTier.DisposeConnection(conn1);
                if (flg >= 1)
                {
                    ShowMessage("Inserted Successfully", "Yellow");
                }
                //InsertLogAuditTrial is used to insert the action into MasterLog table
                InsertLogAuditTrail(Session["sesUserID"].ToString(), "VesselName", "Insert", "Success", "Log");
            }
        }
        catch (Exception ex)
        {
            ShowMessage("Unable to insert, Please try again/Contact your administrator", "Red");
            e.Canceled = true;
            //InsertLogAuditTrial is used to insert the action into MasterAuditTrail table
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "VesselName", "Insert", ex.ToString(), "Audit");
        }

        //m_datatable.Rows.Add(newRow);
        RadGrid1.Rebind();
    }

    protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            string ID = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Vessel_ID"].ToString().Trim();
            TextBox txtVesselName = (TextBox)editedItem.FindControl("txtVesselName");
            TextBox txtDescription = (TextBox)editedItem.FindControl("txtDescription");

            RadComboBox cboVesselType = (RadComboBox)editedItem.FindControl("cboVesselType");
            TextBox txtVesselSize = (TextBox)editedItem.FindControl("txtVesselSize");
            TextBox txtNatureofActivity = (TextBox)editedItem.FindControl("txtNatureofActivity");
            TextBox txtOwnerName = (TextBox)editedItem.FindControl("txtOwnerName");
            TextBox txtCompanyName = (TextBox)editedItem.FindControl("txtCompanyName");
            TextBox txtDesignation = (TextBox)editedItem.FindControl("txtDesignation");
            TextBox txtContactNumber = (TextBox)editedItem.FindControl("txtContactNumber");
            TextBox txtMaterialman = (TextBox)editedItem.FindControl("txtMaterialman");
            TextBox txtMaterialcontno = (TextBox)editedItem.FindControl("txtMaterialcontno");
            RadComboBox cbovesselcategory = (RadComboBox)editedItem.FindControl("cbovesselcategory");
            RadComboBox cboStationName = (RadComboBox)editedItem.FindControl("cboStationName");

            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();
            int flg = BusinessTier.SaveVessel(conn, Convert.ToInt32(ID), txtVesselName.Text.ToString().Trim(),txtVesselSize.Text.ToString().Trim(), txtDescription.Text.ToString().Trim(),
                cboVesselType.SelectedItem.Text.ToString().Trim(), cbovesselcategory.SelectedItem.Text.ToString().Trim(), cboStationName.Text.ToString().Trim(), txtNatureofActivity.Text.ToString().Trim(), txtOwnerName.Text.ToString().Trim(),
                txtCompanyName.Text.ToString().Trim(), txtDesignation.Text.ToString().Trim(), txtContactNumber.Text.ToString().Trim(),
                txtMaterialman.Text.ToString().Trim(), txtMaterialcontno.Text.ToString().Trim(), Session["sesUserID"].ToString(), "U");
            BusinessTier.DisposeConnection(conn);

            if (flg >= 1)
            {
                ShowMessage("Updated Successfully", "yellow");
            }

            RadGrid1.Rebind();
            //InsertLogAuditTrial is used to insert the action into MasterLog table
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "VesselName", "Update", "Success", "Log");
        }
        catch (Exception ex)
        {
            ShowMessage("Unable to update, Please try again/Contact your administrator", "Red");
            e.Canceled = true;
            //InsertLogAuditTrial is used to insert the action into MasterAuditTrail table
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "VesselName", "Update", ex.ToString(), "Audit");
        }

    }
    protected void cboStationName_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        try
        {
            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();
            string sql1 = " select Station_ID,Station_Name,Station_type from Station where DELETED=0  and [Station_Name] LIKE @text + '%' order by Station_Name";

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
                item.Text = row["Station_Name"].ToString();
                item.Value = row["Station_ID"].ToString();
                item.Attributes.Add("Station_Name", row["Station_Name"].ToString());
                item.Attributes.Add("Station_Type", row["Station_Type"].ToString());
                comboBox.Items.Add(item);
                item.DataBind();
            }
            adapter1.Dispose();
            BusinessTier.DisposeConnection(conn);
            // Session["rdobutton"] = "";
        }


        catch (Exception ex)
        {

            ShowMessage("Please Select the Installation Name", "Red");
        }


    }
    protected void OnSelectedIndexChangedHandler1(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        Session["Station_ID"] = e.Value;
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
