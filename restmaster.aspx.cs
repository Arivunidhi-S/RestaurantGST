using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
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

public partial class restmaster : System.Web.UI.Page
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

    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                GridEditFormItem editedItem = e.Item as GridEditFormItem;
                RadTextBox txtname = (RadTextBox)editedItem.FindControl("txtname");
                RadTextBox txtaddress1 = (RadTextBox)editedItem.FindControl("txtaddress1");
                RadTextBox txtaddress2 = (RadTextBox)editedItem.FindControl("txtaddress2");
                RadTextBox txtcity = (RadTextBox)editedItem.FindControl("txtcity");
                RadTextBox txtstate = (RadTextBox)editedItem.FindControl("txtstate");
                RadTextBox txtcountry = (RadTextBox)editedItem.FindControl("txtcountry");
                RadMaskedTextBox txttelno = (RadMaskedTextBox)editedItem.FindControl("txttelno");
                RadAsyncUpload AsyncUpload1 = (RadAsyncUpload)editedItem.FindControl("AsyncUpload1");
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void RadGrid1_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                GridEditFormItem editedItem = e.Item as GridEditFormItem;
                Label lbl = (Label)editedItem.FindControl("lblID");
                RadTextBox txtname = (RadTextBox)editedItem.FindControl("txtname");
                RadTextBox txtaddress1 = (RadTextBox)editedItem.FindControl("txtaddress1");
                RadTextBox txtaddress2 = (RadTextBox)editedItem.FindControl("txtaddress2");
                RadTextBox txtcity = (RadTextBox)editedItem.FindControl("txtcity");
                RadTextBox txtstate = (RadTextBox)editedItem.FindControl("txtstate");
                RadTextBox txtcountry = (RadTextBox)editedItem.FindControl("txtcountry");
                RadMaskedTextBox txttelno = (RadMaskedTextBox)editedItem.FindControl("txttelno");
                RadAsyncUpload AsyncUpload1 = (RadAsyncUpload)editedItem.FindControl("AsyncUpload1");
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void RadGrid1_ItemCommand(object sender, GridItemEventArgs e)
    {
        //if (e.Item is GridEditableItem && e.Item.IsInEditMode)
        //{
        //    GridEditFormItem item = (GridEditFormItem)e.Item;
        //    RadTextBox txbName = (RadTextBox)item.FindControl("txbName");
        //    RadTextBox txtcity = (RadTextBox)item.FindControl("txtcity");
        //    RadTextBox txtadd1 = (RadTextBox)item.FindControl("txtadd1");
        //    RadTextBox txtadd2 = (RadTextBox)item.FindControl("txtadd2");
        //    RadTextBox txtstate = (RadTextBox)item.FindControl("txtstate");
        //    RadTextBox txtcountry = (RadTextBox)item.FindControl("txtcountry");
        //    RadTextBox txttelno = (RadTextBox)item.FindControl("txttelno");
        //}
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
        sql = "select * from restaurantmaster where deleted=0 order by id desc";
        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sql, conn);
        DataTable g_datatable = new DataTable();
        sqlDataAdapter.Fill(g_datatable);
        BusinessTier.DisposeAdapter(sqlDataAdapter);
        BusinessTier.DisposeConnection(conn);
        return g_datatable;
    }
    public void RadAsyncUpload1_FileUploaded(object sender, FileUploadedEventArgs e)
    {

    }
    protected void RadGrid1_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            string ID = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"].ToString().Trim();
            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("update restaurantmaster set deleted=1 where id='" + Convert.ToInt32(ID.ToString().Trim()) + "'", conn);
            cmd.ExecuteNonQuery();
            RadGrid1.Rebind();
            BusinessTier.DisposeConnection(conn);
            lblStatus.Text = "Restaurant Name Successfully deleted";
            e.Canceled = true;
            return;
        }


        catch (Exception ex)
        {
            lblStatus.Text = "Please Check Admin";
            e.Canceled = true;
            return;
        }
    }
    protected void RadGrid1_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            RadTextBox txtname = (RadTextBox)editedItem.FindControl("txtname");
            RadTextBox txtaddress1 = (RadTextBox)editedItem.FindControl("txtaddress1");
            RadTextBox txtaddress2 = (RadTextBox)editedItem.FindControl("txtaddress2");
            RadTextBox txtcity = (RadTextBox)editedItem.FindControl("txtcity");
            RadTextBox txtstate = (RadTextBox)editedItem.FindControl("txtstate");
            RadTextBox txtcountry = (RadTextBox)editedItem.FindControl("txtcountry");
            RadMaskedTextBox txttelno = (RadMaskedTextBox)editedItem.FindControl("txttelno");
            RadAsyncUpload RadAsyncUpload1 = (RadAsyncUpload)editedItem.FindControl("RadAsyncUpload1");
            Guid guid = new Guid();
            guid = Guid.NewGuid();
            byte[] fileData;
            int restid = 0;
            SqlConnection dbcon = BusinessTier.getConnection();
            dbcon.Open();
            if (string.IsNullOrEmpty(txtname.Text.ToString()))
            {
                lblStatus.Text = "Name is Mandatory";
                e.Canceled = true;
                return;
            }
            if (string.IsNullOrEmpty(txtaddress1.Text.ToString()))
            {
                lblStatus.Text = "Address1 is Mandatory";
                e.Canceled = true;
                return;
            }
            if (string.IsNullOrEmpty(txttelno.Text.ToString()))
            {
                lblStatus.Text = "contact is Mandatory";
                e.Canceled = true;
                return;
            }
            string strqry = "select id FROM restaurantmaster where name='" + txtname.Text.ToString().Trim() + "' and address1='" + txtaddress1.Text.ToString().Trim() + "'   and Deleted=0";
            SqlCommand cmd1 = new SqlCommand(strqry, dbcon);
            SqlDataReader reader = cmd1.ExecuteReader();
            while (reader.Read())
            {
                restid = Convert.ToInt32(reader["id"].ToString().Trim());
            }
            BusinessTier.DisposeReader(reader);

            if (restid == 0)
            {
                if (RadAsyncUpload1.UploadedFiles.Count > 0)
                {
                    SqlCommand cmd = new SqlCommand("insert into restaurantmaster(name,address1,address2,city,country,state,telno,created_by,giud,Picture)values('" + txtname.Text.ToString().Trim() + "','" + txtaddress1.Text.ToString().Trim() + "','" + txtaddress2.Text.ToString().Trim() + "','" + txtcity.Text.ToString().Trim() + "','" + txtstate.Text.ToString().Trim() + "','" + txtcountry.Text.ToString().Trim() + "','" + txttelno.Text.ToString().Trim() + "','" + Convert.ToInt32(Session["sesUserID"].ToString()) + "','" + guid.ToString().Trim() + "'," + " @Picture)", dbcon);
                    UploadedFile getUploadFile = RadAsyncUpload1.UploadedFiles[0];
                    fileData = new byte[getUploadFile.InputStream.Length];
                    getUploadFile.InputStream.Read(fileData, 0, (int)getUploadFile.InputStream.Length);
                    cmd.Parameters.Add("@Picture", SqlDbType.VarBinary);
                    cmd.Parameters["@Picture"].Value = fileData;
                    cmd.ExecuteScalar();
                    getUploadFile.InputStream.Close();
                    getUploadFile.InputStream.Dispose();
                    BusinessTier.DisposeConnection(dbcon);

                }
                else
                {
                    SqlCommand cmd5 = new SqlCommand("insert into restaurantmaster(name,address1,address2,city,country,state,telno,created_by)values('" + txtname.Text.ToString().Trim() + "','" + txtaddress1.Text.ToString().Trim() + "','" + txtaddress2.Text.ToString().Trim() + "','" + txtcity.Text.ToString().Trim() + "','" + txtstate.Text.ToString().Trim() + "','" + txtcountry.Text.ToString().Trim() + "','" +txttelno.Text.ToString().Trim() + "','" + Session["sesUserID"].ToString() + "')", dbcon);
                    //SqlCommand cmd5 = new SqlCommand("insert into restaurantmaster(name,address1,address2,city,state,country,telno)values('" + txtname.Text.ToString().Trim() + "','" + txtaddress1.Text.ToString().Trim() + "','" + txtaddress2.Text.ToString().Trim() + "','" + txtcity.Text.ToString().Trim() + "','" + txtstate.Text.ToString().Trim() + "','" + txtcountry.Text.ToString().Trim() + "','" + txttelno.Text.ToString().Trim() + "')", dbcon);
                    cmd5.ExecuteNonQuery();
                }
                lblStatus.Text = "Restaurant Name Successfully saved";
            }
            else
            {
                if (restid != 0)
                {
                    lblStatus.Text = "Menu already Already Exists this category,Try other name";
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
            int ID = Convert.ToInt32(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["ID"].ToString());
            Label lblid = (Label)editedItem.FindControl("lblid");
            Label lblname = (Label)editedItem.FindControl("lblname");
            RadTextBox txtname = (RadTextBox)editedItem.FindControl("txtname");
            RadTextBox txtaddress1 = (RadTextBox)editedItem.FindControl("txtaddress1");
            RadTextBox txtaddress2 = (RadTextBox)editedItem.FindControl("txtaddress2");
            RadTextBox txtcity = (RadTextBox)editedItem.FindControl("txtcity");
            RadTextBox txtstate = (RadTextBox)editedItem.FindControl("txtstate");
            RadTextBox txtcountry = (RadTextBox)editedItem.FindControl("txtcountry");
            RadMaskedTextBox txttelno = (RadMaskedTextBox)editedItem.FindControl("txttelno");
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
            if (string.IsNullOrEmpty(txtaddress1.Text.ToString()))
            {
                lblStatus.Text = "price is Mandatory";
                e.Canceled = true;
                return;
            }
            if (string.IsNullOrEmpty(txttelno.Text.ToString()))
            {
                lblStatus.Text = "contact is Mandatory";
                e.Canceled = true;
                return;
            }
            if (txtname.Text.ToString() != lblname.Text.ToString())
            {
                string strqry = "select id FROM restaurantmaster where name='" + txtname.Text.ToString().Trim() + "' and address1='" + txtaddress1.Text.ToString().Trim() + "' and Deleted=0";
                SqlCommand cmd1 = new SqlCommand(strqry, dbcon);
                SqlDataReader reader = cmd1.ExecuteReader();
                while (reader.Read())
                {
                    cateid = Convert.ToInt32(reader["id"].ToString().Trim());
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
                SqlCommand cmd3 = new SqlCommand("update restaurantmaster set name='" + txtname.Text.ToString().Trim() + "',address1='" + txtaddress1.Text.ToString().Trim() + "',address2='" + txtaddress2.Text.ToString().Trim() + "',city='" + txtcity.Text.ToString() + "',state='" + txtstate.Text.ToString() + "',country='" + txtcountry.Text.ToString() + "',modified_by='" + Convert.ToInt32(Session["sesUserID"].ToString()) + "' where id='" + Convert.ToInt32(lblid.Text.ToString()) + "'", dbcon);
                cmd3.ExecuteNonQuery();
                lblStatus.Text = "Restaurant Name Successfully Updated";
               // return;
            }
            else
            {
                SqlCommand cmd = new SqlCommand("update restaurantmaster set name='" + txtname.Text.ToString().Trim() + "',address1='" + txtaddress1.Text.ToString().Trim() + "',address2='" + txtaddress2.Text.ToString().Trim() + "',city='" + txtcity.Text.ToString() + "',state='" + txtstate.Text.ToString() + "',country='" + txtcountry.Text.ToString() + "',modified_by='" + Convert.ToInt32(Session["sesUserID"].ToString()) + "',picture =" + " @Picture  where id='" + Convert.ToInt32(lblid.Text.ToString()) + "'", dbcon);
                UploadedFile getUploadFile = RadAsyncUpload1.UploadedFiles[0];
                fileData = new byte[getUploadFile.InputStream.Length];
                getUploadFile.InputStream.Read(fileData, 0, (int)getUploadFile.InputStream.Length);
                cmd.Parameters.Add("@Picture", SqlDbType.VarBinary);
                cmd.Parameters["@Picture"].Value = fileData;
                cmd.ExecuteScalar();
                getUploadFile.InputStream.Close();
                getUploadFile.InputStream.Dispose();
                BusinessTier.DisposeConnection(dbcon);
                lblStatus.Text = "Restaurant Name Successfully Updated";
               // return;
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
}