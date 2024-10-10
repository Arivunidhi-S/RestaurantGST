<%@ WebHandler Language="C#" Class="ShowImage" %>

using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public class ShowImage : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {

        if (context.Request.QueryString["autoId"] == null) return;



        //string connStr = ConfigurationManager.AppSettings["connString"].ToString();

        string pictureId = context.Request.QueryString["autoId"];

        //using (SqlConnection conn = new SqlConnection(connStr))
        using (SqlConnection conn = BusinessTier.getConnection())
        {

            using (SqlCommand cmd = new SqlCommand("SELECT Picture from menumaster WHERE ID = @autoId", conn))
            {

                cmd.Parameters.Add(new SqlParameter("@autoId", pictureId));

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    reader.Read();
                    context.Response.BinaryWrite((Byte[])reader[reader.GetOrdinal("Picture")]);
                    reader.Close();
                }

            }

        }

    }



    public bool IsReusable
    {

        get
        {

            return true;

        }

    }
}