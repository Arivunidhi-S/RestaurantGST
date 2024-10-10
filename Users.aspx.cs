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

public partial class Users : System.Web.UI.Page
{
    String a = "", b = "";
    protected void Page_Init(object sender, EventArgs e)
    {
        lblStatus.Text = "";
        try
        {
            a = Session["user_name"].ToString();
           // b = Session["user_group"].ToString();
            b = Session["user_group"].ToString();

            if (string.IsNullOrEmpty(Session["sesUserID"].ToString()))
            {
                Response.Redirect("Login.aspx");
            }

        }
        catch (Exception ex1)
        {
            Response.Redirect("Login.aspx");

        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        lblStatus.Text = "";
        try
        {

            if ((string.IsNullOrEmpty(Session["user_name"].ToString())))
            {
                Response.Redirect("Login.aspx");
            }



        }
        catch (Exception ex1)
        {
            Response.Redirect("Login.aspx");

        }
    }


    protected void RadGrid1_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            string ID = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["user_aid"].ToString();
            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();
            int flg = BusinessTier.SaveUserDetails(conn, ID.ToString(), "", "", "", "", "", "Delete", "","","","","","");
            BusinessTier.DisposeConnection(conn);
            if (flg >= 1)
            {
                //lblStatus.Text = "User Data deleted successfully";
                ShowMessage("Deleted Successfully", "Yellow");
                System.Drawing.ColorConverter colConvert = new ColorConverter();
                lblStatus.ForeColor = (System.Drawing.Color)colConvert.ConvertFromString("Yellow");
            }
        }
        catch (Exception ex)
        {
           // lblStatus.Text = ex.ToString();
            ShowMessage("Can't Perform Deletion", "Red");
            System.Drawing.ColorConverter colConvert = new ColorConverter();
            lblStatus.ForeColor = (System.Drawing.Color)colConvert.ConvertFromString("Red");
            e.Canceled = true;
            //InsertLogAuditTrial is used to insert the action into MasterAuditTrail table
            InsertLogAuditTrail("", "Users_tbl", "Delete", ex.ToString(), "Audit");
        }
    }


    protected void cboGroup_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        SqlConnection conn = BusinessTier.getConnection();
        conn.Open();
        //string sql1 = " select group_id,group_aid from UserGroup_tbl where DELETED=0  and  [group_id] LIKE @text + '%' order by group_id";
        string sql1 = " select group_id,group_aid from UserGroup_tbl where DELETED=0 order by group_id";
        SqlDataAdapter adapter1 = new SqlDataAdapter(sql1, conn);
        adapter1.SelectCommand.Parameters.AddWithValue("@Text", e.Text);
        DataTable dataTable1 = new DataTable();
        adapter1.Fill(dataTable1);
        RadComboBox comboBox = (RadComboBox)sender;
        comboBox.Items.Clear();
        foreach (DataRow row in dataTable1.Rows)
        {
            RadComboBoxItem item = new RadComboBoxItem();
            // item.Text = row["city"].ToString();
            //item.Text = row["CompanyName"].ToString();
            item.Text = row["group_id"].ToString();
            item.Value = row["group_aid"].ToString();

            // item.Attributes.Add("city", row["city"].ToString());
            item.Attributes.Add("group_aid", row["group_aid"].ToString());
            item.Attributes.Add("group_id", row["group_id"].ToString());

            //item.Attributes.Add("City", row["City"].ToString());
            comboBox.Items.Add(item);
            item.DataBind();
        }
        adapter1.Dispose();
        BusinessTier.DisposeConnection(conn);
    }


    protected void RadGrid1_ItemDataBound(object source, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                GridEditFormItem editedItem = e.Item as GridEditFormItem;
                Label lbl = (Label)editedItem.FindControl("lblID");
                RadComboBox cboGroup = (RadComboBox)editedItem.FindControl("cboGroup");
                RadComboBox cboStatus = (RadComboBox)editedItem.FindControl("cboStatus");
                RadComboBox cboContact = (RadComboBox)editedItem.FindControl("cboContact");
                CheckBox ChkSelect = (CheckBox)editedItem.FindControl("ChkSelect");

                if (!(string.IsNullOrEmpty(b.ToString())))
                {
                    string strGroup = "";
                    string strStatus = "";
                    string strContact = "";
                    int Rptflag = 0;

                    SqlConnection conn = BusinessTier.getConnection();
                    conn.Open();
                    string sql = "select user_aid,user_name,user_active,contact_name,Rpt_Flag FROM users_tbl WHERE user_aid = '" + b.ToString() + "'";
                    SqlCommand command = new SqlCommand(sql, conn);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        strGroup = reader["user_name"].ToString().Trim();
                        strStatus = reader["user_active"].ToString().Trim();
                        strContact = reader["contact_name"].ToString().Trim();
                        Rptflag = Convert.ToInt32(reader["Rpt_Flag"].ToString().Trim());
                    }
                    BusinessTier.DisposeReader(reader);
                    BusinessTier.DisposeConnection(conn);

                    cboGroup.SelectedValue = strGroup.ToString().Trim();
                    cboStatus.SelectedValue = strStatus.ToString().Trim();
                    cboContact.SelectedValue = strContact.ToString().Trim();
                    //if (Rptflag == 1)
                    //{
                    //    ChkSelect.Checked = true;
                    //}
                }

            }
        }
        catch (Exception ex)
        {
            //lblStatus.Text = ex.ToString();

            System.Drawing.ColorConverter colConvert = new ColorConverter();
            lblStatus.ForeColor = (System.Drawing.Color)colConvert.ConvertFromString("Red");
            e.Canceled = true;
            //InsertLogAuditTrial is used to insert the action into MasterAuditTrail table
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "Users_tbl", "RadGrid1_ItemDataBound", ex.ToString(), "Audit");
        }
    }


    protected void RadGrid1_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;

            RadTextBox txtName = (RadTextBox)editedItem.FindControl("txtName");
            RadTextBox txtPass = (RadTextBox)editedItem.FindControl("txtPass");
            RadComboBox cboGroup = (RadComboBox)editedItem.FindControl("cboGroup");
            RadTextBox txtEmail = (RadTextBox)editedItem.FindControl("txtEmail");
            RadComboBox cboStatus = (RadComboBox)editedItem.FindControl("cboStatus");
            RadComboBox cboContact = (RadComboBox)editedItem.FindControl("cboContact");
            RadTextBox contact_name = (RadTextBox)editedItem.FindControl("contact_name");
            RadTextBox contact_no = (RadTextBox)editedItem.FindControl("contact_no");
            RadTextBox designation = (RadTextBox)editedItem.FindControl("designation");
            RadTextBox department = (RadTextBox)editedItem.FindControl("department");

            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();
            //int flg = BusinessTier.SaveUserDetails(conn, "0", txtName.Text.ToString(), txtPass.Text.ToString(), cboGroup.SelectedItem.Text.ToString(), txtEmail.Text.ToString(), "", "Insert", cboStatus.SelectedItem.Text.ToString(), cboContact.SelectedItem.Text.ToString(), contact_name.Text.ToString(), contact_no.Text.ToString(), designation.Text.ToString(), department.Text.ToString());
            int flg = BusinessTier.SaveUserDetails(conn, "0", txtName.Text.ToString(), txtPass.Text.ToString(), cboGroup.SelectedValue.ToString(), txtEmail.Text.ToString(), "", "Insert", cboStatus.SelectedItem.Text.ToString(), cboContact.SelectedItem.Text.ToString(), contact_name.Text.ToString(), contact_no.Text.ToString(), designation.Text.ToString(), department.Text.ToString());
            BusinessTier.DisposeConnection(conn);

            //lblStatus.Text = "User Data inserted successfully";
            ShowMessage("Inserted Successfully", "Yellow");
            System.Drawing.ColorConverter colConvert = new ColorConverter();
            lblStatus.ForeColor = (System.Drawing.Color)colConvert.ConvertFromString("Yellow");
        }
        catch (Exception ex)
        {
            //lblStatus.Text = ex.ToString();
            ShowMessage("Can't Perform Insertion", "Red");
            System.Drawing.ColorConverter colConvert = new ColorConverter();
            lblStatus.ForeColor = (System.Drawing.Color)colConvert.ConvertFromString("Red");
            e.Canceled = true;
            //InsertLogAuditTrial is used to insert the action into MasterAuditTrail table
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "Users_tbl", "Insert", ex.ToString(), "Audit");
        }

        //m_datatable.Rows.Add(newRow);
        RadGrid1.Rebind();
    }


    protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            int ID = Convert.ToInt32(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["user_aid"].ToString());

            RadTextBox txtName = (RadTextBox)editedItem.FindControl("txtName");
            RadTextBox txtPass = (RadTextBox)editedItem.FindControl("txtPass");
            RadComboBox cboGroup = (RadComboBox)editedItem.FindControl("cboGroup");
            RadTextBox txtEmail = (RadTextBox)editedItem.FindControl("txtEmail");
            RadComboBox cboStatus = (RadComboBox)editedItem.FindControl("cboStatus");
            RadComboBox cboContact = (RadComboBox)editedItem.FindControl("cboContact");
            RadTextBox contact_name = (RadTextBox)editedItem.FindControl("contact_name");
            RadTextBox contact_no = (RadTextBox)editedItem.FindControl("contact_no");
            RadTextBox designation = (RadTextBox)editedItem.FindControl("designation");
            RadTextBox department = (RadTextBox)editedItem.FindControl("department");

            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();
            //int flg = BusinessTier.SaveUserDetails(conn, ID.ToString(), txtName.Text.ToString(), txtPass.Text.ToString(), cboGroup.SelectedItem.Text.ToString(), txtEmail.Text.ToString(), "", "Update", cboStatus.SelectedItem.Text.ToString(), cboContact.SelectedItem.Text.ToString(), contact_name.Text.ToString(), contact_no.Text.ToString(), designation.Text.ToString(), department.Text.ToString());
            int flg = BusinessTier.SaveUserDetails(conn, ID.ToString(), txtName.Text.ToString(), txtPass.Text.ToString(), cboGroup.SelectedValue.ToString(), txtEmail.Text.ToString(), "", "Update", cboStatus.SelectedItem.Text.ToString(), cboContact.SelectedItem.Text.ToString(), contact_name.Text.ToString(), contact_no.Text.ToString(), designation.Text.ToString(), department.Text.ToString());
            BusinessTier.DisposeConnection(conn);

            //lblStatus.Text = "User data updated successfully";
            ShowMessage("Updated Successfully", "Yellow");
            System.Drawing.ColorConverter colConvert = new ColorConverter();
            lblStatus.ForeColor = (System.Drawing.Color)colConvert.ConvertFromString("Yellow");
        }
        catch (Exception ex)
        {
            //lblStatus.Text = ex.ToString();
            ShowMessage("Can't Perform Updation. Choose the Group and fill the required values", "Red");
            System.Drawing.ColorConverter colConvert = new ColorConverter();
            lblStatus.ForeColor = (System.Drawing.Color)colConvert.ConvertFromString("Red");
            e.Canceled = true;
            //InsertLogAuditTrial is used to insert the action into MasterAuditTrail table
           InsertLogAuditTrail(Session["sesUserID"].ToString(), "Users_tbl", "Update", ex.ToString(), "Audit");
        }
}

    public DataTable DataSourceHelper()
    {
        SqlConnection conn = BusinessTier.getConnection();
        conn.Open();
       // SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("select user_aid,user_name,user_password,group_aid,user_email,user_active,contact,contact_name,contact_no,designation,department FROM users_tbl WHERE user_active='TRUE' and Deleted=0 order by user_name", conn);
        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("select UserGroup_tbl.group_id,Users_tbl.user_aid,user_name,user_password,Users_tbl.group_aid,user_email,user_active,contact,contact_name,contact_no,designation,department,Rpt_flag FROM users_tbl inner join UserGroup_tbl on UserGroup_tbl.group_aid=Users_tbl.group_aid WHERE user_active='TRUE' and Users_tbl.Deleted=0 and UserGroup_tbl.Deleted=0 order by user_name", conn);

        
        DataTable g_datatable = new DataTable();
        sqlDataAdapter.Fill(g_datatable);
        BusinessTier.DisposeAdapter(sqlDataAdapter);
        BusinessTier.DisposeConnection(conn);
        return g_datatable;
    }

    //private void InsertLogAuditTrail(string userid, string module, string activity, string result, string flag)
    //{
    //    SqlConnection connLog = BusinessTier.getConnection();
    //    connLog.Open();
    //    //BusinessTier.InsertLogAuditTrial(connLog, userid, module, activity, result, flag);
    //    BusinessTier.DisposeConnection(connLog);
    //}

    protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            RadGrid1.DataSource = DataSourceHelper();
        }
        catch (Exception ex)
        {
            //InsertLogAuditTrial is used to insert the action into MasterAuditTrail table
            InsertLogAuditTrail(Session["sesUserID"].ToString(), "Users_tbl", "NeedDataSource", ex.ToString(), "Audit");
        }
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

    protected void btnDelivered_Click(object sender, EventArgs e)
    {
        try
        {
           

            foreach (GridDataItem Item in RadGrid1.Items)
            {
                CheckBox ChkSelect = (CheckBox)Item.FindControl("ChkSelect");

                if (ChkSelect.Checked)
                {
                   // strSectorDKey = item.GetDataKeyValue("Sector_ID").ToString();
                    Label lbluserid = Item.FindControl("lbluserid") as Label;
                    string struserid = lbluserid.Text.ToString().Trim();

                    SqlConnection conn = BusinessTier.getConnection();
                    conn.Open();

                    string strupdate = "UPDATE [Users_tbl] SET [Rpt_flag]=1 WHERE [user_aid] = '" + struserid + "'";
                    SqlCommand cmd = new SqlCommand(strupdate, conn);
                    cmd.ExecuteNonQuery();
                    BusinessTier.DisposeConnection(conn);
                    ShowMessage("Updated Successfully", "Yellow");
                   // RadGrid1.Rebind();
                }
                else
                {
                    Label lbluserid = Item.FindControl("lbluserid") as Label;
                    string struserid = lbluserid.Text.ToString().Trim();

                    SqlConnection conn = BusinessTier.getConnection();
                    conn.Open();

                    string strupdate = "UPDATE [Users_tbl] SET [Rpt_flag]=0 WHERE [user_aid] = '" + struserid + "'";
                    SqlCommand cmd = new SqlCommand(strupdate, conn);
                    cmd.ExecuteNonQuery();
                    BusinessTier.DisposeConnection(conn);
                    ShowMessage("Updated Successfully", "Yellow");
                   // RadGrid1.Rebind();
                   // lblStatus.Text = "Please Select the CheckBox";
                }
            }

            RadGrid1.Rebind();
        }

        catch (Exception ex)
        {
            //InsertLogAuditTrail(Session["sesUserID"].ToString(), "ScheWaste_Approve", "btnSchWasteApprove_Click", ex.ToString(), "Audit");
           // ShowMessage(8);
        }
    }

}