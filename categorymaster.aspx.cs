using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Telerik.Charting;
using Telerik.Web.UI;
using System.Data.SqlClient;



public partial class categorymaster : System.Web.UI.Page
{

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
    }
    protected void RadGrid1_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                GridEditFormItem editedItem = e.Item as GridEditFormItem;
                Label lblid = (Label)editedItem.FindControl("lblid");
                RadTextBox txtname = (RadTextBox)editedItem.FindControl("txtname");
                RadTextBox txtdesc = (RadTextBox)editedItem.FindControl("txtdesc");
                RadComboBox cbokitchen = (RadComboBox)editedItem.FindControl("cbokitchen");
                RadComboBox cbocatparent = (RadComboBox)editedItem.FindControl("cbocatparent");
                RadAsyncUpload RadAsyncUpload1 = (RadAsyncUpload)editedItem.FindControl("RadAsyncUpload1");
            }
        }
        catch (Exception ex)
        {
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

        }
    }
    public DataTable DataSourceHelper()
    {
        string sql = "";
        SqlConnection conn = BusinessTier.getConnection();
        conn.Open();
        sql = "SELECT categoryMaster.category_ID,kitchenMaster.Name AS kitchenname,categoryMaster.Name AS categoryname,categoryMaster.Description,categoryMaster.Picture,categoryMaster.Deleted FROM categoryMaster INNER JOIN kitchenMaster ON categoryMaster.Kitchen = kitchenMaster.ID where categoryMaster.deleted=0 order by category_id desc";
        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sql, conn);
        DataTable g_datatable = new DataTable();
        sqlDataAdapter.Fill(g_datatable);
        BusinessTier.DisposeAdapter(sqlDataAdapter);
        BusinessTier.DisposeConnection(conn);
        return g_datatable;
    }
    protected void RadGrid1_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        SqlConnection conn = BusinessTier.getConnection();
        try
        {
            string ID = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["category_ID"].ToString().Trim();
            conn.Open();
            SqlCommand cmd = new SqlCommand("update categorymaster set deleted=1 where category_ID='" + Convert.ToInt32(ID.ToString().Trim()) + "'", conn);
            cmd.ExecuteNonQuery();
            RadGrid1.Rebind();
            BusinessTier.DisposeConnection(conn);
            lblStatus.Text = "Successfully deleted";
        }
        catch (Exception ex)
        {
            BusinessTier.DisposeConnection(conn);
            lblStatus.Text = "Please Check Admin";
            e.Canceled = true;
            return;
        }
    }
    protected void RadGrid1_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        SqlConnection dbcon = BusinessTier.getConnection();
        try
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            RadTextBox txtname = (RadTextBox)editedItem.FindControl("txtname");
            RadTextBox txtdesc = (RadTextBox)editedItem.FindControl("txtdesc");
            RadComboBox cbokitchen = (RadComboBox)editedItem.FindControl("cbokitchen");
            RadComboBox cbocatparent = (RadComboBox)editedItem.FindControl("cbocatparent");
            RadAsyncUpload RadAsyncUpload1 = (RadAsyncUpload)editedItem.FindControl("RadAsyncUpload1");
            Guid guid = new Guid();
            guid = Guid.NewGuid();
            byte[] fileData;
            int cateid = 0;
            dbcon.Open();
            if (string.IsNullOrEmpty(txtname.Text.ToString()))
            {
                lblStatus.Text = "Category Name is Mandatory";
                e.Canceled = true;
                return;
            }

            if (cbokitchen.SelectedValue.ToString() == "0")
            {
                lblStatus.Text = "Kitchen Name is Mandatory,Please Select";
                e.Canceled = true;
                return;
            }

            string strqry = "select category_id FROM categorymaster where name='" + txtname.Text.ToString().Trim() + "' and kitchen='" + cbokitchen.SelectedValue.ToString().Trim() + "'   and Deleted=0";
            SqlCommand cmd1 = new SqlCommand(strqry, dbcon);

            SqlDataReader reader = cmd1.ExecuteReader();
            while (reader.Read())
            {
                cateid = Convert.ToInt32(reader["category_id"].ToString().Trim());
            }
            BusinessTier.DisposeReader(reader);
            if (cateid == 0)
            {
                if (RadAsyncUpload1.UploadedFiles.Count > 0)
                {
                    Session["sesGUid1"] = guid.ToString().Trim();
                    SqlCommand cmd = new SqlCommand("insert into categorymaster(name,description,kitchen,parent,created_by,guid,Picture)values('" + txtname.Text.ToString().Trim() + "','" + txtdesc.Text.ToString().Trim() + "','" + Convert.ToInt32(cbokitchen.SelectedValue.ToString()) + "','" + Convert.ToInt32(cbocatparent.SelectedValue.ToString()) + "','" + Convert.ToInt32(Session["sesUserID"].ToString()) + "','" + guid.ToString().Trim() + "'," + " @Picture)", dbcon);
                    UploadedFile getUploadFile = RadAsyncUpload1.UploadedFiles[0];
                    fileData = new byte[getUploadFile.InputStream.Length];
                    getUploadFile.InputStream.Read(fileData, 0, (int)getUploadFile.InputStream.Length);
                    cmd.Parameters.Add("@Picture", SqlDbType.VarBinary);
                    cmd.Parameters["@Picture"].Value = fileData;
                    cmd.ExecuteScalar();
                    getUploadFile.InputStream.Close();
                    getUploadFile.InputStream.Dispose();
                    BusinessTier.DisposeConnection(dbcon);
                    lblStatus.Text = "Category Name Successfully Saved";
                }
                else
                {
                    SqlCommand cmdnoninmage = new SqlCommand("insert into categorymaster(name,description,kitchen,parent,created_by)values('" + txtname.Text.ToString().Trim() + "','" + txtdesc.Text.ToString().Trim() + "','" + Convert.ToInt32(cbokitchen.SelectedValue.ToString()) + "','" + Convert.ToInt32(cbocatparent.SelectedValue.ToString()) + "','" + Convert.ToInt32(Session["sesUserID"].ToString()) + "')", dbcon);
                    cmdnoninmage.ExecuteNonQuery();
                }
            }
            else
            {
                if (cateid != 0)
                {
                    lblStatus.Text = "Category Name already Already Exists this kitchen,Try other name";
                    e.Canceled = true;
                    return;
                }
            }

        }
        catch (Exception ex)
        {
            lblStatus.Text = "pls check admin";
            e.Canceled = true;
            return;
        }
        RadGrid1.Rebind();
    }


    protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            int ID = Convert.ToInt32(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["category_ID"].ToString());
            Label lblid = (Label)editedItem.FindControl("lblid");

            Label lblcatname = (Label)editedItem.FindControl("lblcatname");
            Label lblkitchen = (Label)editedItem.FindControl("lblkitchen");
            Label lblparent = (Label)editedItem.FindControl("lblparent");
            RadTextBox txtname = (RadTextBox)editedItem.FindControl("txtname");
            RadTextBox txtdesc = (RadTextBox)editedItem.FindControl("txtdesc");
            RadComboBox cbokitchen = (RadComboBox)editedItem.FindControl("cbokitchen");
            RadComboBox cbocatparent = (RadComboBox)editedItem.FindControl("cbocatparent");
            RadAsyncUpload RadAsyncUpload1 = (RadAsyncUpload)editedItem.FindControl("RadAsyncUpload1");
            Guid guid = new Guid();
            guid = Guid.NewGuid();
            byte[] fileData;
            int cateid = 0;
            DateTime getdate = System.DateTime.Now;
            SqlConnection dbcon = BusinessTier.getConnection();
            dbcon.Open();
            if (string.IsNullOrEmpty(txtname.Text.ToString()))
            {
                lblStatus.Text = "Name is Mandatory";
                e.Canceled = true;
                return;
            }

            //if (cbokitchen.SelectedValue.ToString() == "0")
            //{
            //    lblStatus.Text = "kitchen Name is Mandatory";
            //    e.Canceled = true;
            //    return;
            //}

            int kitchenid = 0;
            string strqry1 = "select id FROM kitchenmaster where name='" + cbokitchen.Text.ToString().Trim() + "'  and Deleted=0";
            SqlCommand cmd11 = new SqlCommand(strqry1, dbcon);

            SqlDataReader reader1 = cmd11.ExecuteReader();
            while (reader1.Read())
            {
                kitchenid = Convert.ToInt32(reader1["id"].ToString().Trim());
            }
            BusinessTier.DisposeReader(reader1);

            string strqry2 = "select category_id FROM categorymaster where name='" + cbocatparent.Text.ToString().Trim() + "'  and Deleted=0";
            SqlCommand cmd2 = new SqlCommand(strqry2, dbcon);

            int cateparentid = 0;
            SqlDataReader reader2 = cmd2.ExecuteReader();
            while (reader2.Read())
            {
                cateparentid = Convert.ToInt32(reader2["category_id"].ToString().Trim());
            }
            BusinessTier.DisposeReader(reader2);

            if (txtname.Text.ToString() != lblcatname.Text.ToString())//&& cbokitchen.Text.ToString() != lblkitchen.Text.ToString() && cbocatparent.Text.ToString() != lblparent.Text.ToString()
            {
                string strqry = "select category_id FROM categorymaster where name='" + txtname.Text.ToString().Trim() + "' and kitchen='" + kitchenid + "'  and Deleted=0";
                SqlCommand cmd1 = new SqlCommand(strqry, dbcon);
                SqlDataReader reader = cmd1.ExecuteReader();
                while (reader.Read())
                {
                    cateid = Convert.ToInt32(reader["category_id"].ToString().Trim());
                }
                BusinessTier.DisposeReader(reader);
                if (cateid != 0)
                {
                    lblStatus.Text = "Menu already Already Exists this category,Try other name";
                    e.Canceled = true;
                    return;
                }

            }

            if (RadAsyncUpload1.UploadedFiles.Count == 0)
            {
                SqlCommand cmd3 = new SqlCommand("update categorymaster set name='" + txtname.Text.ToString().Trim() + "',description='" + txtdesc.Text.ToString().Trim() + "',kitchen='" + Convert.ToInt32(kitchenid) + "',parent='" + Convert.ToInt32(cateparentid) + "',modified_by='" + Convert.ToInt32(Session["sesUserID"].ToString()) + "'   where category_id='" + Convert.ToInt32(ID.ToString()) + "'", dbcon);
                cmd3.ExecuteNonQuery();
                lblStatus.Text = "Category name Successfully Updated";

            }
            else
            {
                SqlCommand cmd = new SqlCommand("update categorymaster set name='" + txtname.Text.ToString().Trim() + "', guid='" + guid.ToString().Trim() + "',description='" + txtdesc.Text.ToString().Trim() + "',kitchen='" + Convert.ToInt32(kitchenid) + "',parent='" + Convert.ToInt32(cateparentid) + "',modified_by='" + Convert.ToInt32(Session["sesUserID"].ToString()) + "',picture =" + " @Picture  where category_id='" + Convert.ToInt32(ID.ToString()) + "'", dbcon);
                UploadedFile getUploadFile = RadAsyncUpload1.UploadedFiles[0];
                fileData = new byte[getUploadFile.InputStream.Length];
                getUploadFile.InputStream.Read(fileData, 0, (int)getUploadFile.InputStream.Length);
                cmd.Parameters.Add("@Picture", SqlDbType.VarBinary);
                cmd.Parameters["@Picture"].Value = fileData;
                cmd.ExecuteScalar();
                getUploadFile.InputStream.Close();
                getUploadFile.InputStream.Dispose();
                lblStatus.Text = "Category Name Successfully Updated";
            }
            BusinessTier.DisposeConnection(dbcon);
        }
        catch (Exception ex)
        {
            lblStatus.Text = "pls check admin";
            e.Canceled = true;
            return;
        }
        RadGrid1.Rebind();
    }
    public void RadAsyncUpload1_FileUploaded(object sender, FileUploadedEventArgs e)
    {

    }
    protected void RadGrid1_ItemDataBound(object source, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                GridEditFormItem editedItem = e.Item as GridEditFormItem;
                int ID = Convert.ToInt32(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["category_ID"].ToString());
                Label lblid = (Label)editedItem.FindControl("lblid");
                RadTextBox txtname = (RadTextBox)editedItem.FindControl("txtname");
                RadTextBox txtdesc = (RadTextBox)editedItem.FindControl("txtdesc");
                RadComboBox cbokitchen = (RadComboBox)editedItem.FindControl("cbokitchen");
                RadComboBox cbocatparent = (RadComboBox)editedItem.FindControl("cbocatparent");
                RadAsyncUpload RadAsyncUpload1 = (RadAsyncUpload)editedItem.FindControl("RadAsyncUpload1");
                if (!(string.IsNullOrEmpty(lblid.Text.ToString())))
                {
                    SqlConnection conn = BusinessTier.getConnection();
                    conn.Open();
                    SqlCommand command = new SqlCommand("SELECT categoryMaster.category_ID,categoryMaster.Deleted,categoryMaster_1.Name as parentname,kitchenMaster.Name AS kitchenname FROM categoryMaster LEFT OUTER JOIN kitchenMaster ON  categoryMaster.Kitchen =  kitchenMaster.ID LEFT OUTER JOIN  categoryMaster AS categoryMaster_1 ON  categoryMaster.parent = categoryMaster_1.category_ID WHERE categoryMaster.Deleted = 0 and categoryMaster.category_ID = '" + Convert.ToInt32(ID.ToString()) + "'", conn);
                    //SqlCommand command1 = new SqlCommand("SELECT categoryMaster.category_ID,  categoryMaster.Deleted,kitchenMaster.Name AS kitchenname FROM categoryMaster INNER JOIN kitchenMaster ON  categoryMaster.Kitchen =  kitchenMaster.ID WHERE categoryMaster.Deleted = 0 and categoryMaster.category_ID = '" + Convert.ToInt32(ID.ToString()) + "'", conn);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        cbokitchen.SelectedItem.Text = reader["kitchenname"].ToString();
                        cbocatparent.SelectedItem.Text = reader["parentname"].ToString();
                        if (reader["parentname"].ToString() == "")
                        {
                            cbocatparent.SelectedItem.Text = "Top Level";
                        }
                    }
                    BusinessTier.DisposeReader(reader);
                    BusinessTier.DisposeConnection(conn);
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    //---------------------------------------------------------
    protected void RadGrid1_ItemCommand(object source, GridCommandEventArgs e)
    {
        if (e.CommandName == RadGrid.EditCommandName)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetEditMode", "isEditMode = true;", true);
        }
    }
}