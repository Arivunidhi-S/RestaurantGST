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

public partial class taxmaster : System.Web.UI.Page
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
            //if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            //{
            //    GridEditFormItem editedItem = e.Item as GridEditFormItem;
            //    RadTextBox txttaxname = (RadTextBox)editedItem.FindControl("txttaxname");
            //    RadTextBox txttax = (RadTextBox)editedItem.FindControl("txttax");
            //    RadComboBox txtflag = (RadComboBox)editedItem.FindControl("txtflag");
            //}
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
                GridEditableItem editedItem = e.Item as GridEditableItem;
                Label lblid = (Label)editedItem.FindControl("lblid");
                RadTextBox txttaxName = (RadTextBox)editedItem.FindControl("txttaxName");
                RadNumericTextBox txttax = (RadNumericTextBox)editedItem.FindControl("txttax");
                RadComboBox cboflag = (RadComboBox)editedItem.FindControl("cboflag");
                if (!(string.IsNullOrEmpty(lblid.Text.ToString())))
                {
                    SqlConnection conn = BusinessTier.getConnection();
                    conn.Open();
                    SqlCommand command = new SqlCommand("SELECT flag from taxmaster where id= '" + Convert.ToInt32(lblid.Text.ToString()) + "'", conn);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        //cboflag.SelectedItem.Text = reader["flag"].ToString().Trim();

                        if (reader["flag"].ToString().Trim() == "1")
                        {
                            cboflag.SelectedItem.Text = "yes";
                        }
                        else
                        {
                            cboflag.SelectedItem.Text = "No";
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
        sql = "SELECT * from taxmaster where deleted=0 order by id desc";
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
            int flg = 0;
            string ID = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"].ToString().Trim();
            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();
            flg = BusinessTier.SaveNewTax(conn, "", 0, 0, Convert.ToInt32(Session["sesUserID"].ToString()), "D", Convert.ToInt32(ID.ToString()));
            BusinessTier.DisposeConnection(conn);
            if (flg >= 1)
            {
                lblStatus.Text = "Successfully deleted";
            }
        }
        catch (Exception ex)
        {
            lblStatus.Text = "pls check admin";
            e.Canceled = true;
        }
        RadGrid1.Rebind();
    }
    protected void RadGrid1_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            Label lblid = (Label)editedItem.FindControl("lblid");
            RadTextBox txttaxName = (RadTextBox)editedItem.FindControl("txttaxName");
            RadNumericTextBox txttax = (RadNumericTextBox)editedItem.FindControl("txttax");
            RadComboBox cboflag = (RadComboBox)editedItem.FindControl("cboflag");

            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();
            if (cboflag.SelectedValue.ToString() == "0")
            {
                lblStatus.Text = "Applicable is Mandatory";
                e.Canceled = true;
                return;
            }
            string restid = "0";
            string strqry = "select id FROM taxmaster where name='" + txttaxName.Text.ToString().Trim() + "' and Deleted=0";
            SqlCommand cmd = new SqlCommand(strqry, conn);

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                restid = (reader["id"].ToString().Trim());
            }
            BusinessTier.DisposeReader(reader);

            if (restid == "0")
            {
                int flg = BusinessTier.SaveNewTax(conn, txttaxName.Text.ToString().Trim(), Convert.ToInt32(txttax.Text.ToString().Trim()), Convert.ToInt32(cboflag.SelectedValue.ToString()), Convert.ToInt32(Session["sesUserID"].ToString()), "N", 0);
                if (flg >= 1)
                {
                    lblStatus.Text = "New Tax Name Successfully Saved";
                }
            }
            else
            {
                if (restid != "0")
                {
                    lblStatus.Text = "Tax Name Already Exists ,Try other name";
                    e.Canceled = true;
                    return;
                }
            }
            BusinessTier.DisposeConnection(conn);

        }
        catch (Exception ex)
        {
            lblStatus.Text = "pls check admin";
            e.Canceled = true;
        }
        RadGrid1.Rebind();

    }
    protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            Label lblid = (Label)editedItem.FindControl("lblid");
            Label lbltaxname = (Label)editedItem.FindControl("lbltaxname");
            RadTextBox txttaxName = (RadTextBox)editedItem.FindControl("txttaxName");
            RadNumericTextBox txttax = (RadNumericTextBox)editedItem.FindControl("txttax");
            RadComboBox cboflag = (RadComboBox)editedItem.FindControl("cboflag");

            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();
            if (cboflag.SelectedValue.ToString() == "0")
            {
                lblStatus.Text = "Applicable is Mandatory";
                e.Canceled = true;
                return;
            }

            if (txttaxName.Text.ToString() != lbltaxname.Text.ToString())
            {
                string taxname = "0";
                string strqry = "select id FROM taxmaster where name='" + txttaxName.Text.ToString().Trim() + "' and Deleted=0";
                SqlCommand cmd = new SqlCommand(strqry, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    taxname = (reader["id"].ToString().Trim());
                }
                BusinessTier.DisposeReader(reader);
                if (taxname != "0")
                {
                    lblStatus.Text = "Tax Name Already Exists ,Try other name";
                    e.Canceled = true;
                    return;
                }
            }

            //int applicaid = 0;
            //string strqry1 = "SELECT id from taxmaster where name='" + cboflag.Text.ToString() + "'";
            //SqlCommand cmd1 = new SqlCommand(strqry1, conn);
            //SqlDataReader reader1 = cmd1.ExecuteReader();
            //if (reader1.Read())
            //{
            //    applicaid = Convert.ToInt32(reader1["id"].ToString().Trim());
            //}
            //BusinessTier.DisposeReader(reader1);


            int flg = BusinessTier.SaveNewTax(conn, txttaxName.Text.ToString().Trim(), Convert.ToInt32(txttax.Text.ToString().Trim()), Convert.ToInt32(cboflag.SelectedValue.ToString()), Convert.ToInt32(Session["sesUserID"].ToString()), "U", Convert.ToInt32(lblid.Text.ToString()));
            if (flg >= 1)
            {
                lblStatus.Text = "New Tax Name Successfully Updated";
            }

            BusinessTier.DisposeConnection(conn);

        }
        catch (Exception ex)
        {
            lblStatus.Text = "pls check admin";
            e.Canceled = true;
        }
        RadGrid1.Rebind();

    }
}