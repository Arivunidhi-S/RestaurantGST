using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using MySql.Data.MySqlClient;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Runtime;
using System.Drawing;
using System.Data.SqlClient;
//using MySql.Data.MySqlClient;
using System.Data;
using System.IO;
using System.Web.SessionState;

public partial class Login : System.Web.UI.Page
{
    String txtName = "", txtPass = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        LoginTxt.Focus();


        if (!(IsPostBack))
        {
        }
    }
    protected void Page_Init(object sender, EventArgs e)
    {
        LoginTxt.Focus();
        if (!(IsPostBack))
        {

            Session.Contents.Clear();

        }

    }
    protected void LoginBtn_Click(object sender, EventArgs e)
    {
        SqlConnection connec = BusinessTier.getConnection();
        try
        {
            if (string.IsNullOrEmpty(LoginTxt.Text.ToString()))
            {
                MessageBox("Enter your Login ID");
                return;
            }
            if (string.IsNullOrEmpty(PwdTxt.Text.ToString()))
            {
                MessageBox("Enter your Password");
                return;
            }

            SqlConnection conn = BusinessTier.getConnection();
            conn.Open();
            //  BusinessTier.BindErrorMessageDetails(conn);
            BusinessTier.DisposeConnection(conn);

            int flag = 0;
            string strId = "0";
            int intValidation = 0;

            connec.Open();

            txtName = LoginTxt.Text.ToString();//Request["LoginTxt"];
            txtPass = PwdTxt.Text.ToString();//Request["txtPass"];
            String a1 = "", a2 = "";
            SqlDataReader reader1 = BusinessTier.VaildateUserLogin(connec, txtName, txtPass);
            if (reader1.Read())
            {
                if (!(string.IsNullOrEmpty(reader1["user_aid"].ToString())))
                {
                    flag = 1;
                    strId = (reader1["user_aid"].ToString());
                    a1 = (reader1["user_name"].ToString());
                    Session["user_name"] = a1;
                    //a2 = (reader1["user_group"].ToString());
                    a2 = (reader1["group_aid"].ToString());
                    Session["user_group"] = a2;
                    HttpSessionState ss = HttpContext.Current.Session;
                    DateTime dt = DateTime.Now;
                    String strDt = dt.ToString();
                    String sid = ss.SessionID + "-" + strDt;
                    Session["sid"] = sid;

                    Session["sesUserID"] = strId.ToString();


                    Response.Redirect("Main.aspx", true);
                }
                else
                {
                    intValidation = 1;
                }
            }
            BusinessTier.DisposeReader(reader1);
            BusinessTier.DisposeConnection(connec);

            if (intValidation == 1)
            {
                //ShowMessage("Invalid Authentication");
                MessageBox("Check Your LoginID & Password");
                return;
            }
        }
        catch (Exception ex)
        {
            BusinessTier.DisposeConnection(connec);
            MessageBox("Invalid Authentication");
        }
    }
    private void MessageBox(string msg)
    {
        Label lbl = new Label();
        lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
        Page.Controls.Add(lbl);
    }
}