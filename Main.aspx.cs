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

public partial class Main : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {

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
    protected void Page_Init(object sender, EventArgs e)
    {
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
    
}