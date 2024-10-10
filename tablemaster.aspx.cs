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

public partial class tablemaster : System.Web.UI.Page
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
        sql = "SELECT restaurantMaster.Name as restid,TableMaster.table_ID,TableMaster.created_date,TableMaster.TableNo,TableMaster.Description,TableMaster.deleted FROM TableMaster INNER JOIN restaurantMaster ON TableMaster.Restaurantid = restaurantMaster.ID where TableMaster.deleted =0 order by table_ID desc";
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
                RadTextBox txttblno = (RadTextBox)editedItem.FindControl("txttblno");
                RadTextBox txtdescrip = (RadTextBox)editedItem.FindControl("txtdescrip");
                RadComboBox cborestname = (RadComboBox)editedItem.FindControl("cborestname");
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
                Label lbl = (Label)editedItem.FindControl("lblID");
                RadTextBox txttblno = (RadTextBox)editedItem.FindControl("txttblno");
                RadTextBox txtdescrip = (RadTextBox)editedItem.FindControl("txtdescrip");
                RadComboBox cborestname = (RadComboBox)editedItem.FindControl("cborestname");
                if (!(string.IsNullOrEmpty(lbl.Text.ToString())))
                {
                    SqlConnection conn = BusinessTier.getConnection();
                    conn.Open();
                    SqlCommand command = new SqlCommand("SELECT restaurantMaster.Name,TableMaster.Table_ID,TableMaster.deleted FROM TableMaster INNER JOIN restaurantMaster ON TableMaster.Restaurantid = restaurantMaster.ID where TableMaster.Table_ID = '" + Convert.ToInt32(lbl.Text.ToString()) + "'", conn);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        cborestname.SelectedItem.Text = reader["Name"].ToString();
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
    protected void RadGrid1_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            int flg = 0;
            string ID = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["table_ID"].ToString().Trim();
            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();
            flg = BusinessTier.SaveNewtable(conn, "", "", 0, Convert.ToInt32(Session["sesUserID"].ToString()), "D", Convert.ToInt32(ID.ToString().Trim()));
            BusinessTier.DisposeConnection(conn);
            if (flg >= 1)
            {
                RadGrid1.Rebind();
                lblStatus.Text = "Successfully deleted";
            }

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
            Label lbl = (Label)editedItem.FindControl("lblID");
            RadTextBox txttblno = (RadTextBox)editedItem.FindControl("txttblno");
            RadTextBox txtdescrip = (RadTextBox)editedItem.FindControl("txtdescrip");
            RadComboBox cborestname = (RadComboBox)editedItem.FindControl("cborestname");
            if (string.IsNullOrEmpty(txttblno.Text.ToString()))
            {
                lblStatus.Text = "Table No is Mandatory";
                e.Canceled = true;
                return;
            }
            if (cborestname.SelectedValue.ToString() == "0")
            {
                lblStatus.Text = "Restaurant Name is Mandatory";
                e.Canceled = true;
                return;
            }
            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();
            string restid = "0";
            string strqry = "select table_id FROM tablemaster where tableno='" + txttblno.Text.ToString().Trim() + "' and restaurantid='" + cborestname.SelectedValue.ToString().Trim() + "'   and Deleted=0";
            SqlCommand cmd = new SqlCommand(strqry, conn);

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                restid = (reader["table_id"].ToString().Trim());
            }
            BusinessTier.DisposeReader(reader);

            if (restid == "0")
            {
                int flg = BusinessTier.SaveNewtable(conn, txttblno.Text.ToString().Trim(), txtdescrip.Text.ToString().Trim(), Convert.ToInt32(cborestname.SelectedValue.ToString()), Convert.ToInt32(Session["sesUserID"].ToString()), "N", 0);
                if (flg >= 1)
                {
                    lblStatus.Text = "Tbl No Successfully Saved";
                }
            }
            else
            {
                if (restid != "0")
                {
                    lblStatus.Text = "Table Number Already Exists this restaurant,Try other name";
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
            return;
        }
        RadGrid1.Rebind();

    }
    protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            Label lbl = (Label)editedItem.FindControl("lblID");
            Label lbltblno = (Label)editedItem.FindControl("lbltblno");
            Label lblrestname = (Label)editedItem.FindControl("lblrestname");
            RadTextBox txttblno = (RadTextBox)editedItem.FindControl("txttblno");
            RadTextBox txtdescrip = (RadTextBox)editedItem.FindControl("txtdescrip");
            RadComboBox cborestname = (RadComboBox)editedItem.FindControl("cborestname");
            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();

            int restid = 0;
            string strqry = "SELECT id from restaurantmaster where name='" + cborestname.Text.ToString() + "'";
            SqlCommand cmd = new SqlCommand(strqry, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                restid = Convert.ToInt32(reader["id"].ToString().Trim());
            }
            BusinessTier.DisposeReader(reader);
            if (txttblno.Text.ToString() != lbltblno.Text.ToString() || cborestname.SelectedItem.Text.ToString() != lblrestname.Text.ToString())
            {
                string dubid = "0";
                string strqry1 = "select table_id FROM tablemaster where tableno='" + txttblno.Text.ToString().Trim() + "' and restaurantid='" + restid + "' and Deleted=0";
                SqlCommand cmd1 = new SqlCommand(strqry1, conn);
                SqlDataReader reader1 = cmd1.ExecuteReader();
                while (reader1.Read())
                {
                    dubid = (reader1["table_id"].ToString().Trim());
                }
                BusinessTier.DisposeReader(reader1);
                if (dubid != "0")
                {
                    lblStatus.Text = "Table Number Already Exists this restaurant,Try other name";
                    e.Canceled = true;
                    return;
                }
            }
            int flg = BusinessTier.SaveNewtable(conn, txttblno.Text.ToString().Trim(), txtdescrip.Text.ToString().Trim(), restid, Convert.ToInt32(Session["sesUserID"].ToString()), "U", Convert.ToInt32(lbl.Text.ToString()));
            if (flg >= 1)
            {
                lblStatus.Text = "Table Number Successfully Updated";
            }
            BusinessTier.DisposeConnection(conn);

        }
        catch (Exception ex)
        {
            lblStatus.Text = "pls check admin";
            e.Canceled = true;
            return;
        }
    }
}