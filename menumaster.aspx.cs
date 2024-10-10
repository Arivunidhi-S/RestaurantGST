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

public partial class menumaster : System.Web.UI.Page
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
        sql = "select * from VW_menumasterDatasource order by id desc";
        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sql, conn);
        DataTable g_datatable = new DataTable();
        sqlDataAdapter.Fill(g_datatable);
        BusinessTier.DisposeAdapter(sqlDataAdapter);
        BusinessTier.DisposeConnection(conn);
        return g_datatable;
    }

    protected void RadGrid1_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                GridEditFormItem editedItem = e.Item as GridEditFormItem;
                RadTextBox txtname = (RadTextBox)editedItem.FindControl("txtname");
                RadTextBox txtdescrip = (RadTextBox)editedItem.FindControl("txtdescrip");
                RadNumericTextBox txtunitprice = (RadNumericTextBox)editedItem.FindControl("txtunitprice");
                RadNumericTextBox txtdiscount = (RadNumericTextBox)editedItem.FindControl("txtdiscount");

                RadNumericTextBox txtdiningcharge = (RadNumericTextBox)editedItem.FindControl("txtdiningcharge");

                RadComboBox cbocategory = (RadComboBox)editedItem.FindControl("cbocategory");
                RadComboBox cboavailable = (RadComboBox)editedItem.FindControl("cboavailable");
                RadAsyncUpload AsyncUpload1 = (RadAsyncUpload)editedItem.FindControl("AsyncUpload1");
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void RadGrid1_ItemDataBound(object source, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                GridEditFormItem editedItem = e.Item as GridEditFormItem;
                int ID = Convert.ToInt32(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["ID"].ToString());
                Label lbl = (Label)editedItem.FindControl("lblID");
                RadTextBox txtname = (RadTextBox)editedItem.FindControl("txtname");
                RadTextBox txtdescrip = (RadTextBox)editedItem.FindControl("txtdescrip");
                RadNumericTextBox txtunitprice = (RadNumericTextBox)editedItem.FindControl("txtunitprice");
                RadNumericTextBox txtdiscount = (RadNumericTextBox)editedItem.FindControl("txtdiscount");

                RadNumericTextBox txtdiningcharge = (RadNumericTextBox)editedItem.FindControl("txtdiningcharge");

                RadComboBox cbocategory = (RadComboBox)editedItem.FindControl("cbocategory");
                RadComboBox cboavailable = (RadComboBox)editedItem.FindControl("cboavailable");
                if (!(string.IsNullOrEmpty(lbl.Text.ToString())))
                {
                    SqlConnection conn = BusinessTier.getConnection();
                    conn.Open();
                    SqlCommand command = new SqlCommand("SELECT categoryMaster.Name,menuMaster.ID,menuMaster.Available FROM menuMaster INNER JOIN categoryMaster ON menuMaster.Category = categoryMaster.category_ID where menuMaster.ID = '" + Convert.ToInt32(lbl.Text.ToString()) + "'", conn);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        cbocategory.SelectedItem.Text = reader["Name"].ToString();
                        cboavailable.SelectedItem.Text = reader["Available"].ToString();
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
    public void RadAsyncUpload1_FileUploaded(object sender, FileUploadedEventArgs e)
    {

    }
    protected void RadGrid1_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        SqlConnection conn = BusinessTier.getConnection();
        try
        {
            //int flg = 0;
            string ID = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"].ToString().Trim();

            conn.Open();
            SqlCommand cmd = new SqlCommand("update menumaster set deleted=1 where id='" + Convert.ToInt32(ID.ToString().Trim()) + "'", conn);
            cmd.ExecuteNonQuery();
            RadGrid1.Rebind();
            BusinessTier.DisposeConnection(conn);
            lblStatus.Text = "Successfully deleted";
            e.Canceled = true;
            return;
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
            RadTextBox txtdescrip = (RadTextBox)editedItem.FindControl("txtdescrip");
            RadNumericTextBox txtunitprice = (RadNumericTextBox)editedItem.FindControl("txtunitprice");
            RadNumericTextBox txtdiscount = (RadNumericTextBox)editedItem.FindControl("txtdiscount");

            RadNumericTextBox txtdiningcharge = (RadNumericTextBox)editedItem.FindControl("txtdiningcharge");

            RadComboBox cbocategory = (RadComboBox)editedItem.FindControl("cbocategory");
            RadComboBox cboavailable = (RadComboBox)editedItem.FindControl("cboavailable");
            RadAsyncUpload RadAsyncUpload1 = (RadAsyncUpload)editedItem.FindControl("RadAsyncUpload1");
            Guid guid = new Guid();
            guid = Guid.NewGuid();
            byte[] fileData;
            int cateid = 0;

            dbcon.Open();
            if (string.IsNullOrEmpty(txtname.Text.ToString()))
            {
                lblStatus.Text = "Name is Mandatory";
                e.Canceled = true;
                return;
            }
            if (string.IsNullOrEmpty(txtunitprice.Text.ToString()))
            {
                lblStatus.Text = "price is Mandatory";
                e.Canceled = true;
                return;
            }
            if (string.IsNullOrEmpty(txtdiscount.Text.ToString()))
            {
                lblStatus.Text = "discount is Mandatory";
                e.Canceled = true;
                return;
            }
            if (string.IsNullOrEmpty(txtdiningcharge.Text.ToString()))
            {
                lblStatus.Text = "dining charge is Mandatory";
                e.Canceled = true;
                return;
            }
            if (cbocategory.SelectedValue.ToString() == "0")
            {
                lblStatus.Text = "Restaurant Name is Mandatory";
                e.Canceled = true;
                return;
            }
            if (cboavailable.SelectedItem.Text.ToString() == "--Select--")
            {
                lblStatus.Text = "Stock Available is Mandatory";
                e.Canceled = true;
                return;
            }
            string strqry = "select id FROM menumaster where name='" + txtname.Text.ToString().Trim() + "' and category='" + cbocategory.SelectedValue.ToString().Trim() + "'   and Deleted=0";
            SqlCommand cmd1 = new SqlCommand(strqry, dbcon);

            SqlDataReader reader = cmd1.ExecuteReader();
            while (reader.Read())
            {
                cateid = Convert.ToInt32(reader["id"].ToString().Trim());
            }
            BusinessTier.DisposeReader(reader);
            if (cateid == 0)
            {
                if (RadAsyncUpload1.UploadedFiles.Count > 0)
                {
                    Session["sesGUid1"] = guid.ToString().Trim();
                    SqlCommand cmd = new SqlCommand("insert into menumaster(name,description,unitprice,Discount,diningcharge,category,available,created_by,guid,Picture)values('" + txtname.Text.ToString().Trim() + "','" + txtdescrip.Text.ToString().Trim() + "','" + Convert.ToDecimal(txtunitprice.Text.ToString().Trim()) + "','" + Convert.ToDecimal(txtdiscount.Text.ToString().Trim()) + "','" + Convert.ToDecimal(txtdiningcharge.Text.ToString().Trim()) + "','" + Convert.ToInt32(cbocategory.SelectedValue.ToString()) + "','" + cboavailable.SelectedItem.Text.ToString() + "','" + Convert.ToInt32(Session["sesUserID"].ToString()) + "','" + guid.ToString().Trim() + "'," + " @Picture)", dbcon);
                    UploadedFile getUploadFile = RadAsyncUpload1.UploadedFiles[0];
                    fileData = new byte[getUploadFile.InputStream.Length];
                    getUploadFile.InputStream.Read(fileData, 0, (int)getUploadFile.InputStream.Length);
                    cmd.Parameters.Add("@Picture", SqlDbType.VarBinary);
                    cmd.Parameters["@Picture"].Value = fileData;
                    cmd.ExecuteScalar();
                    getUploadFile.InputStream.Close();
                    getUploadFile.InputStream.Dispose();
                }
                else
                {
                    SqlCommand cmdi = new SqlCommand("insert into menumaster(name,description,unitprice,Discount,diningcharge,category,available,created_by)values('" + txtname.Text.ToString().Trim() + "','" + txtdescrip.Text.ToString().Trim() + "','" + Convert.ToDecimal(txtunitprice.Text.ToString().Trim()) + "','" + Convert.ToDecimal(txtdiscount.Text.ToString().Trim()) + "','" + Convert.ToDecimal(txtdiningcharge.Text.ToString().Trim()) + "','" + Convert.ToInt32(cbocategory.SelectedValue.ToString()) + "','" + cboavailable.SelectedItem.Text.ToString() + "','" + Convert.ToInt32(Session["sesUserID"].ToString()) + "')", dbcon);
                    cmdi.ExecuteNonQuery();
                }
                BusinessTier.DisposeConnection(dbcon);
                lblStatus.Text = "Menu name Successfully saved";
            }
            else
            {
                if (cateid != 0)
                {
                    lblStatus.Text = "Menu already exists this category,Try other name";
                    e.Canceled = true;
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            BusinessTier.DisposeConnection(dbcon);
            lblStatus.Text = "pls check admin";
            e.Canceled = true;
            return;
        }
        RadGrid1.Rebind();
    }
    protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        SqlConnection dbcon = BusinessTier.getConnection();
        try
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            int ID = Convert.ToInt32(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["ID"].ToString());
            Label lblid = (Label)editedItem.FindControl("lblid");
            Label lblname = (Label)editedItem.FindControl("lblname");
            Label lblcate = (Label)editedItem.FindControl("lblcate");
            RadTextBox txtname = (RadTextBox)editedItem.FindControl("txtname");
            RadTextBox txtdescrip = (RadTextBox)editedItem.FindControl("txtdescrip");
            RadNumericTextBox txtunitprice = (RadNumericTextBox)editedItem.FindControl("txtunitprice");
            RadNumericTextBox txtdiscount = (RadNumericTextBox)editedItem.FindControl("txtdiscount");

            RadNumericTextBox txtdiningcharge = (RadNumericTextBox)editedItem.FindControl("txtdiningcharge");


            RadComboBox cbocategory = (RadComboBox)editedItem.FindControl("cbocategory");
            RadComboBox cboavailable = (RadComboBox)editedItem.FindControl("cboavailable");
            RadAsyncUpload RadAsyncUpload1 = (RadAsyncUpload)editedItem.FindControl("RadAsyncUpload1");
            Guid guid = new Guid();
            guid = Guid.NewGuid();
            byte[] fileData;
            int cateid = 0;
            DateTime getdate = System.DateTime.Now;

            dbcon.Open();
            if (string.IsNullOrEmpty(txtname.Text.ToString()))
            {
                lblStatus.Text = "Name is Mandatory";
                e.Canceled = true;
                return;
            }
            if (string.IsNullOrEmpty(txtunitprice.Text.ToString()))
            {
                lblStatus.Text = "price is Mandatory";
                e.Canceled = true;
                return;
            }
            if (string.IsNullOrEmpty(txtdiscount.Text.ToString()))
            {
                lblStatus.Text = "discount is Mandatory";
                e.Canceled = true;
                return;
            }
            if (string.IsNullOrEmpty(txtdiningcharge.Text.ToString()))
            {
                lblStatus.Text = "dining charge is Mandatory";
                e.Canceled = true;
                return;
            }
            if (cboavailable.SelectedItem.Text.ToString() == "--Select--")
            {
                lblStatus.Text = "Stock Available is Mandatory";
                e.Canceled = true;
                return;
            }
            //if (cbocategory.SelectedValue.ToString() == "0")
            //{
            //    lblStatus.Text = "Category  is Mandatory";
            //    e.Canceled = true;
            //    return;
            //}

            int categorytblid = 0;
            string strqry1 = "select category_ID FROM categorymaster where name='" + cbocategory.Text.ToString().Trim() + "'  and Deleted=0";
            SqlCommand cmd11 = new SqlCommand(strqry1, dbcon);

            SqlDataReader reader1 = cmd11.ExecuteReader();
            while (reader1.Read())
            {
                categorytblid = Convert.ToInt32(reader1["category_ID"].ToString().Trim());
            }
            BusinessTier.DisposeReader(reader1);
            if (txtname.Text.ToString() != lblname.Text.ToString() || cbocategory.Text.ToString() != lblcate.Text.ToString())
            {
                string strqry = "select id FROM menumaster where name='" + txtname.Text.ToString().Trim() + "' and category='" + categorytblid + "'   and Deleted=0";
                SqlCommand cmd1 = new SqlCommand(strqry, dbcon);
                SqlDataReader reader = cmd1.ExecuteReader();
                while (reader.Read())
                {
                    cateid = Convert.ToInt32(reader["id"].ToString().Trim());
                }
                BusinessTier.DisposeReader(reader);
                if (cateid != 0)
                {
                    lblStatus.Text = "Menu already exists this category,Try other name";
                    e.Canceled = true;
                    return;
                }
            }
            if (RadAsyncUpload1.UploadedFiles.Count == 0)
            {
                SqlCommand cmd3 = new SqlCommand("update menumaster set name='" + txtname.Text.ToString().Trim() + "',description='" + txtdescrip.Text.ToString().Trim() + "',unitprice='" + Convert.ToDecimal(txtunitprice.Text.ToString().Trim()) + "',discount='" + Convert.ToDecimal(txtdiscount.Text.ToString().Trim()) + "',diningcharge='" + Convert.ToDecimal(txtdiningcharge.Text.ToString().Trim()) + "',category='" + Convert.ToInt32(categorytblid) + "',available='" + cboavailable.SelectedItem.Text.ToString() + "',modified_by='" + Convert.ToInt32(Session["sesUserID"].ToString()) + "'   where id='" + ID + "'", dbcon);
                cmd3.ExecuteNonQuery();
                lblStatus.Text = "Menu name Successfully Updated";
                return;
            }
            else
            {
                SqlCommand cmd = new SqlCommand("update menumaster set name='" + txtname.Text.ToString().Trim() + "', guid='" + guid.ToString().Trim() + "',description='" + txtdescrip.Text.ToString().Trim() + "',unitprice='" + Convert.ToDecimal(txtunitprice.Text.ToString().Trim()) + "',discount='" + Convert.ToDecimal(txtdiscount.Text.ToString().Trim()) + "',diningcharge='" + Convert.ToDecimal(txtdiningcharge.Text.ToString().Trim()) + "',category='" + Convert.ToInt32(categorytblid) + "',available='" + cboavailable.SelectedItem.Text.ToString() + "',modified_by='" + Convert.ToInt32(Session["sesUserID"].ToString()) + "',picture =" + " @Picture  where id='" + ID + "'", dbcon);
                UploadedFile getUploadFile = RadAsyncUpload1.UploadedFiles[0];
                fileData = new byte[getUploadFile.InputStream.Length];
                getUploadFile.InputStream.Read(fileData, 0, (int)getUploadFile.InputStream.Length);
                cmd.Parameters.Add("@Picture", SqlDbType.VarBinary);
                cmd.Parameters["@Picture"].Value = fileData;
                cmd.ExecuteScalar();
                getUploadFile.InputStream.Close();
                getUploadFile.InputStream.Dispose();
                BusinessTier.DisposeConnection(dbcon);
                lblStatus.Text = "Menu name Successfully saved";
                return;
            }


        }
        catch (Exception ex)
        {
            BusinessTier.DisposeConnection(dbcon);
            lblStatus.Text = "pls check admin";
            e.Canceled = true;
            return;
        }
        RadGrid1.Rebind();

    }
}