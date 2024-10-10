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
public partial class kitchenmaster : System.Web.UI.Page
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
                RadTextBox txtkitchenName = (RadTextBox)editedItem.FindControl("txtkitchenName");
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
                RadTextBox txtkitchenName = (RadTextBox)editedItem.FindControl("txtkitchenName");
                RadTextBox txtdescrip = (RadTextBox)editedItem.FindControl("txtdescrip");
                RadComboBox cborestname = (RadComboBox)editedItem.FindControl("cborestname");
                if (!(string.IsNullOrEmpty(lbl.Text.ToString())))
                {
                    SqlConnection conn = BusinessTier.getConnection();
                    conn.Open();
                    SqlCommand command = new SqlCommand("SELECT kitchenMaster.ID,restaurantMaster.Name AS Restname FROM kitchenMaster INNER JOIN restaurantMaster ON kitchenMaster.RestaurantID = restaurantMaster.ID where  kitchenMaster.ID = '" + Convert.ToInt32(lbl.Text.ToString()) + "'", conn);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        cborestname.SelectedItem.Text = reader["restname"].ToString();
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
        sql = "SELECT kitchenMaster.Name,kitchenMaster.deleted,kitchenMaster.Description,kitchenMaster.ID,restaurantMaster.Name AS Restname FROM kitchenMaster INNER JOIN restaurantMaster ON kitchenMaster.RestaurantID = restaurantMaster.ID where kitchenMaster.deleted=0 order by id desc";
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
            flg = BusinessTier.SaveNewKitchen(conn, "", "", 0, Convert.ToInt32(Session["sesUserID"].ToString()), "D", Convert.ToInt32(ID.ToString().Trim()));
            BusinessTier.DisposeConnection(conn);
            if (flg >= 1)
            {
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
            RadTextBox txtkitchenName = (RadTextBox)editedItem.FindControl("txtkitchenName");
            RadTextBox txtdescrip = (RadTextBox)editedItem.FindControl("txtdescrip");
            RadComboBox cborestname = (RadComboBox)editedItem.FindControl("cborestname");
            if (string.IsNullOrEmpty(txtkitchenName.Text.ToString()))
            {
                lblStatus.Text = "Kitchen Name is Mandatory";
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
            string strqry = "select id FROM kitchenmaster where name='" + txtkitchenName.Text.ToString().Trim() + "' and restaurantid='" + cborestname.SelectedValue.ToString().Trim() + "'   and Deleted=0";
            SqlCommand cmd = new SqlCommand(strqry, conn);

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                restid = (reader["id"].ToString().Trim());
            }
            BusinessTier.DisposeReader(reader);

            if (restid == "0")
            {
                int flg = BusinessTier.SaveNewKitchen(conn, txtkitchenName.Text.ToString().Trim(), txtdescrip.Text.ToString().Trim(), Convert.ToInt32(cborestname.SelectedValue.ToString()), Convert.ToInt32(Session["sesUserID"].ToString()), "N", 0);
                if (flg >= 1)
                {
                    lblStatus.Text = "Kitchen Name Successfully Saved";
                }
            }
            else
            {
                if (restid != "0")
                {
                    lblStatus.Text = "Kitchen Name Already Exists this restaurant,Try other name";
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
            Label lbl = (Label)editedItem.FindControl("lblID");
            Label lblkname = (Label)editedItem.FindControl("lblkname");
           Label lblrname = (Label)editedItem.FindControl("lblrname");
            RadTextBox txtkitchenName = (RadTextBox)editedItem.FindControl("txtkitchenName");
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
            if (txtkitchenName.Text.ToString() != lblkname.Text.ToString() && cborestname.Text.ToString() != lblrname.Text.ToString())
            {
            string dubid = "0";
            string strqry1 = "select id FROM kitchenmaster where name='" + txtkitchenName.Text.ToString().Trim() + "' and restaurantid='" + restid + "' and Deleted=0";
            SqlCommand cmd1 = new SqlCommand(strqry1, conn);

            SqlDataReader reader1 = cmd1.ExecuteReader();
            while (reader1.Read())
            {
                dubid = (reader1["id"].ToString().Trim());
            }
            BusinessTier.DisposeReader(reader1);
            if (dubid != "0")
            {
                lblStatus.Text = "Kitchen Name Already Exists this restaurant,Try other name";
                e.Canceled = true;
                return;
            }
            }
            int flg = BusinessTier.SaveNewKitchen(conn, txtkitchenName.Text.ToString().Trim(), txtdescrip.Text.ToString().Trim(), restid, Convert.ToInt32(Session["sesUserID"].ToString()), "U", Convert.ToInt32(lbl.Text.ToString()));
            if (flg >= 1)
            {
                lblStatus.Text = "Kitchen Name Successfully Updated";
            }
            BusinessTier.DisposeConnection(conn);

        }
        catch (Exception ex)
        {
            lblStatus.Text = "pls check admin";
            e.Canceled = true;
        }
    }
}