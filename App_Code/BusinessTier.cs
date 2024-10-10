using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Configuration;
using System.Net.Mail;
using System.IO;
using System.Net;

using System.Globalization;
using System.Collections;
using System.Data.OleDb;
using System.Drawing;

/// <summary>
/// Summary description for Class1
/// </summary>
public class BusinessTier
{
    public BusinessTier()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static DataTable g_ErrorMessagesDataTable;

    public static SqlConnection getConnection()
    {
        string conString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        SqlConnection conn = new SqlConnection(conString);
        return conn;
    }

    public static string getConnection1()
    {
        string conString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        //SqlConnection conn = new SqlConnection(conString);
        return conString;
    }


    public static void DisposeConnection(SqlConnection conn)
    {
        conn.Close();
        conn.Dispose();
    }

    public static void DisposeReader(SqlDataReader reader)
    {
        reader.Close();
        reader.Dispose();
    }

    public static void DisposeAdapter(SqlDataAdapter adapter)
    {
        adapter.Dispose();
    }

    public static int SaveOrderDetail(SqlConnection connSave, decimal price, Int32 menuid, decimal deduction,decimal diningcharge, string strSaveFlag, string CurrUserId, Int64 orderid, Int32 qty)
    {
        if (connSave == null)
            connSave = getConnection();
        connSave.Open();
        string sp_Name;
        if (strSaveFlag.ToString() == "Insert")
        {
            sp_Name = "[sp_OrderDetail_Insert]";
        }
        else
        {
            sp_Name = "[sp_OrderDetail_Update]";
        }
        SqlCommand dCmd = new SqlCommand(sp_Name, connSave);
        dCmd.CommandType = CommandType.StoredProcedure;
        if (strSaveFlag.ToString() == "Update")
        {
            dCmd.Parameters.AddWithValue("@orderid", orderid);
        }
        dCmd.Parameters.AddWithValue("@price", price);
        dCmd.Parameters.AddWithValue("@menuid", menuid);
        dCmd.Parameters.AddWithValue("@orderid", orderid);
        dCmd.Parameters.AddWithValue("@deduction", deduction);
        dCmd.Parameters.AddWithValue("@diningcharge", diningcharge);
        dCmd.Parameters.AddWithValue("@qty", qty);
        dCmd.Parameters.AddWithValue("@CurrUserId", CurrUserId);
        return dCmd.ExecuteNonQuery();
    }
    public static int SaveOrder(SqlConnection connSave, string tblno, decimal taxorig, decimal tax,decimal gst, decimal orderprice, decimal totaladj,decimal strtotal, string orderdate, string strSaveFlag, string CurrUserId, string orderid, decimal totdisc, decimal taxadj)
    {
        if (connSave == null)
            connSave = getConnection();
        connSave.Open();
        string sp_Name;
        if (strSaveFlag.ToString() == "Insert")
        {
            sp_Name = "[sp_Order_Insert]";
        }
        else
        {
            sp_Name = "[sp_Order_Update]";
        }
        SqlCommand dCmd = new SqlCommand(sp_Name, connSave);
        dCmd.CommandType = CommandType.StoredProcedure;
        if (strSaveFlag.ToString() == "Update")
        {
            dCmd.Parameters.AddWithValue("@orderid", orderid);
        }
        dCmd.Parameters.AddWithValue("@tblno", tblno);
        dCmd.Parameters.AddWithValue("@taxorig", taxorig);
        dCmd.Parameters.AddWithValue("@tax", tax);
        
        dCmd.Parameters.AddWithValue("@gst", gst);
        dCmd.Parameters.AddWithValue("@taxadj", taxadj);
        dCmd.Parameters.AddWithValue("@orderprice", orderprice);
        dCmd.Parameters.AddWithValue("@totaladj", totaladj);
        dCmd.Parameters.AddWithValue("@total", strtotal);
        dCmd.Parameters.AddWithValue("@totdisc", totdisc);
        dCmd.Parameters.AddWithValue("@CurrUserId", CurrUserId);
        return dCmd.ExecuteNonQuery();
    }



    public static int SaveInstallation(SqlConnection connSave, string strInstId, string strInstCode, string strInstName, string strInstType, string strXC, string strYC, string strMC, string strDesc, string strUserid, string strSaveFlag, string pid1)
    {
        string sp_Name;
        if (strSaveFlag.ToString() == "Insert")
        {
            sp_Name = "[sp_Installation_Insert]";
        }
        else
        {
            sp_Name = "[sp_Installation_Update]";
        }
        SqlCommand dCmd = new SqlCommand(sp_Name, connSave);
        dCmd.CommandType = CommandType.StoredProcedure;
        if (strSaveFlag.ToString() == "Update")
        {
            dCmd.Parameters.AddWithValue("@instid_p", strInstId);
        }
        dCmd.Parameters.AddWithValue("@instcode_p", strInstCode);
        dCmd.Parameters.AddWithValue("@instname_p", strInstName);
        dCmd.Parameters.AddWithValue("@insttype_p", strInstType);
        dCmd.Parameters.AddWithValue("@xc_p", strXC);
        dCmd.Parameters.AddWithValue("@yc_p", strYC);
        dCmd.Parameters.AddWithValue("@mc_p", strMC);
        dCmd.Parameters.AddWithValue("@descriptionp", strDesc);
        dCmd.Parameters.AddWithValue("@useridp", strUserid);
        dCmd.Parameters.AddWithValue("@pidp", pid1);
        return dCmd.ExecuteNonQuery();
    }


    public static int DeleteInstallation(SqlConnection conn, string id)
    {
        SqlCommand dCmd = new SqlCommand("[sp_Installation_Delete]", conn);
        dCmd.CommandType = CommandType.StoredProcedure;
        dCmd.Parameters.AddWithValue("@instid_p", id);
        return dCmd.ExecuteNonQuery();
    }

    public static SqlDataReader VaildateUserLogin(SqlConnection connec, string Logind, string Password)
    {
        SqlCommand cmd = new SqlCommand("sp_Validate_UserLogin", connec);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Useridp", Logind);
        cmd.Parameters.AddWithValue("@Passp", Password);
        SqlDataReader reader1 = cmd.ExecuteReader();
        return reader1;
    }

    public static SqlDataReader checkUserGroupId(SqlConnection connCheck, string strGroupId)
    {
        SqlCommand cmd = new SqlCommand("[sp_UserGroup_IsDuplicate]", connCheck);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@group_idp", strGroupId);
        SqlDataReader reader = cmd.ExecuteReader();
        return reader;
    }


    public static int DeleteUserGroupGrid(SqlConnection conn, string id)
    {
        SqlCommand dCmd = new SqlCommand("[sp_UserGroup_Delete]", conn);
        dCmd.CommandType = CommandType.StoredProcedure;
        dCmd.Parameters.AddWithValue("@group_aidp", id);
        return dCmd.ExecuteNonQuery();
    }

    public static int SaveUserGroup(SqlConnection connSave, string strgroupid, string strdesc, string strSaveFlag, string strCurrUserId)
    {
        string sp_Name;
        if (strSaveFlag.ToString() == "Insert")
        {
            sp_Name = "[sp_UserGroup_Insert]";
        }
        else
        {
            sp_Name = "[sp_UserGroup_Update]";
        }
        SqlCommand dCmd = new SqlCommand(sp_Name, connSave);
        dCmd.CommandType = CommandType.StoredProcedure;
        if (strSaveFlag.ToString() == "Update")
        {
            dCmd.Parameters.AddWithValue("@group_aidp", strCurrUserId);
        }
        dCmd.Parameters.AddWithValue("@group_idp", strgroupid);
        dCmd.Parameters.AddWithValue("@descriptionp", strdesc);
        return dCmd.ExecuteNonQuery();
    }

    public static SqlDataReader getUserGroupById(SqlConnection conn, string strUserGroupId)
    {
        int delval = 0;
        string sql = "select * FROM UserGroup_tbl WHERE Deleted='" + delval + "' and group_id='" + strUserGroupId + "' ORDER BY group_id";
        SqlCommand cmd = new SqlCommand(sql, conn);
        SqlDataReader reader = cmd.ExecuteReader();
        return reader;
    }

    public static SqlDataReader getUserGroupByAID(SqlConnection conn, string strUserGroupAId)
    {
        int delval = 0;
        string sql = "select * FROM UserGroup_tbl WHERE Deleted='" + delval + "' and group_aid='" + strUserGroupAId + "' ORDER BY group_id";
        SqlCommand cmd = new SqlCommand(sql, conn);
        SqlDataReader reader = cmd.ExecuteReader();
        return reader;
    }

    public static int SaveUserGroupModulePermission(SqlConnection connSave, long intgroupid, long intlinebyline, string struserid)
    {
        SqlCommand dCmd = new SqlCommand("[sp_UserGroupModulePermission_Save]", connSave);
        dCmd.CommandType = CommandType.StoredProcedure;

        dCmd.Parameters.AddWithValue("@usergroup_aidp", intgroupid);
        dCmd.Parameters.AddWithValue("@moduleidp", intlinebyline);
        dCmd.Parameters.AddWithValue("@useridp", struserid);
        return dCmd.ExecuteNonQuery();
    }

    public static SqlDataReader getMasterModulePermisnByUserGroupId(SqlConnection connModulePermission, string strUserGroupId)
    {
        int delval = 0;
        string sql = "select * FROM vw_MasterModulePermission_MasterModuleByModuleID WHERE Deleted='" + delval + "' and group_aid='" + strUserGroupId.ToString() + "'";
        SqlCommand cmd = new SqlCommand(sql, connModulePermission);
        SqlDataReader readerModulePermission = cmd.ExecuteReader();
        return readerModulePermission;
    }

    public static int SaveUserDetails(SqlConnection conn, string strID, string strName, string strPass, string strGroup, string strEmail, string strUserid, string strSaveFlag, string strStatus, string strContact, string contact_name, string contact_no, string designation, string department)
    {
        string sp_Name = "";
        if (strSaveFlag.ToString() == "Insert")
            sp_Name = "sp_user_tbl_insert";

        if (strSaveFlag.ToString() == "Update")
            sp_Name = "sp_user_tbl_update";

        if (strSaveFlag.ToString() == "Delete")
            sp_Name = "[sp_user_tbl_delete]";

        SqlCommand dCmd = new SqlCommand(sp_Name, conn);
        dCmd.CommandType = CommandType.StoredProcedure;
        if ((strSaveFlag.ToString() == "Update") || (strSaveFlag.ToString() == "Delete"))
        {
            dCmd.Parameters.AddWithValue("@user_aid", strID);
        }
        if ((strSaveFlag.ToString() == "Update") || (strSaveFlag.ToString() == "Insert"))
        {
            dCmd.Parameters.AddWithValue("@user_name", strName.Trim());
            dCmd.Parameters.AddWithValue("@user_password", strPass.Trim());
            dCmd.Parameters.AddWithValue("@user_group", strGroup.Trim());
            dCmd.Parameters.AddWithValue("@user_email", strEmail.Trim());
            dCmd.Parameters.AddWithValue("@user_active", strStatus.Trim());
            dCmd.Parameters.AddWithValue("@contact", strContact.Trim());
            dCmd.Parameters.AddWithValue("@contact_name", contact_name.Trim());
            dCmd.Parameters.AddWithValue("@contact_no", contact_no.Trim());
            dCmd.Parameters.AddWithValue("@designation", designation.Trim());
            dCmd.Parameters.AddWithValue("@department", department.Trim());
        }
        return dCmd.ExecuteNonQuery();
    }
    public static void InsertLogAuditTrial(SqlConnection connLog, string userid, string module, string activity, string result, string flag)
    {
        string sp_Name;
        if (flag.ToString() == "Log")
        {
            sp_Name = "[sp_Master_Insert_Log]";
        }
        else
        {
            sp_Name = "[sp_Master_Insert_AuditTrail]";
        }

        SqlCommand dCmd = new SqlCommand(sp_Name, connLog);

        dCmd.CommandType = CommandType.StoredProcedure;
        dCmd.Parameters.AddWithValue("@useridp", userid);
        dCmd.Parameters.AddWithValue("@modulep", module);
        dCmd.Parameters.AddWithValue("@activityp", activity);
        dCmd.Parameters.AddWithValue("@resultp", result);
        dCmd.ExecuteNonQuery();
    }

    //---------------------------------------------------------------------------------------------
    //--------------------------< Methods For Master Module >--------------------------------------

    public static SqlDataReader getMasterModule(SqlConnection conn)
    {
        int delval = 0;
        string sql = "select * FROM MasterModule WHERE Deleted='" + delval + "' ORDER BY ModuleName";
        SqlCommand cmd = new SqlCommand(sql, conn);
        SqlDataReader reader = cmd.ExecuteReader();
        return reader;
    }

    public static SqlDataReader getMasterModuleById(SqlConnection connect, string strModuleId)
    {
        int delval = 0;
        string sql = "select * FROM MasterModule WHERE Deleted='" + delval + "' and ModuleId='" + strModuleId + "' ORDER BY ModuleName";
        SqlCommand cmd = new SqlCommand(sql, connect);
        SqlDataReader reader1 = cmd.ExecuteReader();
        return reader1;

    }

    public static int DeleteModuleGrid(SqlConnection conn, string id)
    {
        SqlCommand dCmd = new SqlCommand("sp_MasterModule_Delete", conn);
        dCmd.CommandType = CommandType.StoredProcedure;
        dCmd.Parameters.AddWithValue("@moduleidp", id);
        return dCmd.ExecuteNonQuery();
    }


    public static SqlDataReader checkModuleName(SqlConnection connCheck, string name)
    {
        SqlCommand cmd = new SqlCommand("sp_MasterModule_IsDuplicate", connCheck);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@modulenamep", name);
        SqlDataReader reader = cmd.ExecuteReader();
        return reader;
    }


    public static int SaveModuleMaster(SqlConnection conn, string name, string cfname, string desc, string appflag, string userid, string saveflag, string modid)
    {
        string sp_Name;
        string RowValue = "0";
        if (saveflag.ToString() == "N")
        {
            sp_Name = "[sp_MasterModule_Insert]";
        }
        else
        {
            sp_Name = "[sp_MasterModule_Update]";
        }
        SqlCommand dCmd = new SqlCommand(sp_Name, conn);
        dCmd.CommandType = CommandType.StoredProcedure;
        if (saveflag.ToString() == "U")
        {
            dCmd.Parameters.AddWithValue("@idp", modid);
            dCmd.Parameters.AddWithValue("@Rowp", RowValue);
        }
        dCmd.Parameters.AddWithValue("@namep", name);
        dCmd.Parameters.AddWithValue("@cfnamep", cfname);
        dCmd.Parameters.AddWithValue("@descriptionp", desc);
        dCmd.Parameters.AddWithValue("@approvalflag", appflag);
        dCmd.Parameters.AddWithValue("@useridp", userid);
        return dCmd.ExecuteNonQuery();
    }

    
    public static int SaveNewKitchen(SqlConnection conn, string kitchname, string descrp, int restid, int userid, string saveflag, int kitchid)
    {
        SqlCommand dCmd = new SqlCommand("sp_Kitchen_Save", conn);
        dCmd.CommandType = CommandType.StoredProcedure;
        dCmd.Parameters.AddWithValue("@kitchnamep", kitchname);
        dCmd.Parameters.AddWithValue("@descrpp", descrp);
        dCmd.Parameters.AddWithValue("@restidp", restid);
        dCmd.Parameters.AddWithValue("@useridp", userid);
        dCmd.Parameters.AddWithValue("@Flagp", saveflag);
        dCmd.Parameters.AddWithValue("@kitchidp", kitchid);
        return dCmd.ExecuteNonQuery();
    }

    public static int SaveNewtable(SqlConnection conn, string tblno, string descrp, int restid, int userid, string saveflag, int tblmstrid)
    {
        SqlCommand dCmd = new SqlCommand("sp_Table_Save", conn);
        dCmd.CommandType = CommandType.StoredProcedure;
        dCmd.Parameters.AddWithValue("@tblnoep", tblno);
        dCmd.Parameters.AddWithValue("@descrpp", descrp);
        dCmd.Parameters.AddWithValue("@restidp", restid);
        dCmd.Parameters.AddWithValue("@useridp", userid);
        dCmd.Parameters.AddWithValue("@Flagp", saveflag);
        dCmd.Parameters.AddWithValue("@tblmstridp", tblmstrid);
        return dCmd.ExecuteNonQuery();
    }

   

    public static int Savemenutable(SqlConnection conn, string name, string descrp, int price, int discount,int diningcharge, int category, string aval, int userid, string saveflag, int tblmenuid, string guid, byte pic)
    {
        SqlCommand dCmd = new SqlCommand("sp_Menu_Save", conn);
        dCmd.CommandType = CommandType.StoredProcedure;
        dCmd.Parameters.AddWithValue("@menunamep", name);
        dCmd.Parameters.AddWithValue("@descrp", descrp);
        dCmd.Parameters.AddWithValue("@pricep", price);
        dCmd.Parameters.AddWithValue("@discountp", discount);

        dCmd.Parameters.AddWithValue("@diningchargep", diningcharge);

        dCmd.Parameters.AddWithValue("@categoryp", category);
        dCmd.Parameters.AddWithValue("@avalp", aval);
        dCmd.Parameters.AddWithValue("@useridp", userid);
        dCmd.Parameters.AddWithValue("@Flagp", saveflag);
        dCmd.Parameters.AddWithValue("@menumstridp", tblmenuid);
        dCmd.Parameters.AddWithValue("@guidp", guid);
        //dCmd.Parameters.AddWithValue("@picp", pic);
        dCmd.Parameters.Add("@Picture", SqlDbType.VarBinary);
        dCmd.Parameters["@Picture"].Value = pic;
        dCmd.Parameters.AddWithValue("@picp", pic);
        return dCmd.ExecuteNonQuery();
    }

    public static int SaveNewTax(SqlConnection conn, string taxname, int perct, int aplicab, int userid, string saveflag, int taxtblid)
    {
        SqlCommand dCmd = new SqlCommand("sp_Tax_Save", conn);
        dCmd.CommandType = CommandType.StoredProcedure;
        dCmd.Parameters.AddWithValue("@taxnamep", taxname);
        dCmd.Parameters.AddWithValue("@perctp", perct);
        dCmd.Parameters.AddWithValue("@aplicabp", aplicab);
        dCmd.Parameters.AddWithValue("@useridp", userid);
        dCmd.Parameters.AddWithValue("@Flagp", saveflag);
        dCmd.Parameters.AddWithValue("@taxtblidp", taxtblid);
        return dCmd.ExecuteNonQuery();
    }

 

    public static string getCCMailID(string strModule)
    {
        string strEmailFile = ConfigurationManager.AppSettings["Email_CC_FilePath"].ToString();
        //   string strMailCC = "Default@petronas.com.my";
        string strMailCC = "sara@e-serbadk.com";

        if (File.Exists(strEmailFile))
        {
            string strLine = "";
            string[] strLine1 = new string[1];
            int counter = 0;
            StreamReader reader = new StreamReader(strEmailFile);
            while ((strLine = reader.ReadLine()) != null)
            {
                if (counter == 0)
                {
                    strLine1 = strLine.Split(':');

                    if (strLine1[0].ToString().Trim() == strModule.ToString().Trim())
                    {
                        strMailCC = strLine1[1].ToString().Trim();
                        counter = 1;
                    }
                }
            }
            reader.Close();
            reader.Dispose();
        }
        return strMailCC.ToString().Trim();
    }

    public static void SendMail(string strSubject, string strBody, string strToAddress, string strApprovarMail, string strAttachmentFilename)
    {
        SmtpClient smtpClient = new SmtpClient();
        MailMessage message = new MailMessage();
        if (!(strAttachmentFilename.ToString().Trim() == "NoAttach"))
        {
            Attachment attachment = new Attachment(strAttachmentFilename.ToString().Trim());
            message.Attachments.Add(attachment);
        }
        MailAddress fromAddress = new MailAddress(ConfigurationManager.AppSettings["FromAddress"].ToString(), "Vessel Information System");
        smtpClient.Host = ConfigurationManager.AppSettings["ExchangeServer"].ToString();
        smtpClient.Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"].ToString());

        message.Priority = MailPriority.High;
        message.From = fromAddress;
        message.Subject = strSubject.ToString();
        message.To.Add(strToAddress.ToString());
        message.CC.Add(strApprovarMail.ToString());
        message.Body = strBody;
        //smtpClient.EnableSsl = true;
        //smtpClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["FromAddress"].ToString(), ConfigurationManager.AppSettings["Password"].ToString().Trim());
        //smtpClient.UseDefaultCredentials = true;
        smtpClient.Send(message);
        message.Dispose();
        smtpClient.Dispose();
        File.Delete(strAttachmentFilename.ToString().Trim());
    }

    public static SqlDataReader getCustMailbyID(SqlConnection conn1, int intCustID)
    {
        SqlCommand cmd = new SqlCommand("[sp_CustomerMail_CustID]", conn1);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@custidp", intCustID);
        SqlDataReader reader = cmd.ExecuteReader();
        return reader;
    }

    //-----------

    public static Boolean ExecuteInsertUpdateQry(string sql)
    {
        SqlConnection conn = BusinessTier.getConnection();
        conn.Open();
        SqlCommand comm = new SqlCommand(sql, conn);
        int ret = comm.ExecuteNonQuery();
        conn.Close();
        if (ret == 1)
        {
            return true;
        }
        {
            return false;
        }
    }
    public static Boolean ExecuteDeleteQry(string sql)
    {
        SqlConnection conn = BusinessTier.getConnection();
        conn.Open();
        SqlCommand comm = new SqlCommand(sql, conn);
        int ret = comm.ExecuteNonQuery();
        conn.Close();
        if (ret == 1)
        {
            return true;
        }
        {
            return false;
        }
    }
    public static ArrayList getPendingOrderIDsUsingUID(string UID, string dtpckrselect)
    {
        DateTime selectdt = Convert.ToDateTime(dtpckrselect);
        ArrayList al = new ArrayList();
        SqlConnection conn = BusinessTier.getConnection();
        conn.Open();
        string sql = "Select OrderID from dbo.OrderMaster where Table_ID = " + UID + " and Status = 'P' and Deleted='0' and REPLACE(CONVERT(VARCHAR,OrdDate,106),' ','/') = '" + selectdt.ToString("dd/MMM/yyyy") + "'";
        SqlCommand comm = new SqlCommand(sql, conn);
        SqlDataReader dr = comm.ExecuteReader();
        while (dr.Read())
        {
            al.Add(dr["OrderID"].ToString());
        }
        return al;
    }

    public static DataSet getOrderDetailsUsingOrderID(string OID)
    {
        SqlConnection conn = BusinessTier.getConnection();
        conn.Open();
        String sql = "Select MM.Name as Item, MM.Description as Descr, OD.Qty as Qty, OD.Price, OD.DetailID as DetailID, OD.Split as Split ";
        sql = sql + "from dbo.orderDetail OD, dbo.menuMaster MM ";
        sql = sql + "Where OD.OrderID = " + OID + " and OD.MenuID = MM.ID and OD.Deleted=0";
        SqlCommand comm = new SqlCommand(sql, conn);
        SqlDataAdapter da = new SqlDataAdapter(comm);
        DataSet ds = new DataSet();
        da.Fill(ds, "ORDERITEMS");
        conn.Close();
        return ds;
    }
    public static SqlCommand GetCommand(SqlConnection conn, String sql)
    {
        SqlCommand comm = new SqlCommand(sql, conn);
        return comm;
    }
    public static String GettotQty(string id)
    {
        SqlConnection conn = getConnection();
        conn.Open();
        SqlCommand comm = GetCommand(conn, "select Qty, OrderID from dbo.ORDERDETAIL  where deleted=0 and DetailID = " + id);
        SqlDataReader dr = comm.ExecuteReader();
        dr.Read();
        return dr["Qty"].ToString();
    }
    public static String GetUnitPrice(string id)
    {
        SqlConnection conn = getConnection();
        conn.Open();
        SqlCommand comm = GetCommand(conn, "Select UnitPrice from dbo.menuMaster where ID = " + id);
        SqlDataReader dr = comm.ExecuteReader();
        dr.Read();
        return dr["UnitPrice"].ToString();
    }
    public static double RoundUp(double figure, int precision)
    {
        double newFigure = Math.Round(figure / 5, precision) * 5;
        return newFigure;
    }
    public static String GetOrderDetailPrice(string id)
    {
        SqlConnection conn = getConnection();
        conn.Open();
        SqlCommand comm = GetCommand(conn, "Select Price from orderDetail where  deleted=0 and DetailID = " + id);
        SqlDataReader dr = comm.ExecuteReader();
        dr.Read();
        return dr["Price"].ToString();
    }

    public static Double GetTotTax(string id)
    {
        Double ret;
        SqlConnection conn = getConnection();
        conn.Open();
        SqlCommand comm = GetCommand(conn, "Select tax from ordermaster where orderid='" + id + "' and deleted=0");
        SqlDataReader dr = comm.ExecuteReader();
        dr.Read();
        ret = Convert.ToDouble(dr["tax"].ToString());
        return ret;
    }
    public static Double GetTax(string id)
    {
        Double ret;
        SqlConnection conn = getConnection();
        conn.Open();
        SqlCommand comm = GetCommand(conn, "Select diningcharge from ORDERDETAIL where DETAILID='"+ id +"' and deleted=0");
        SqlDataReader dr = comm.ExecuteReader();
        dr.Read();
        ret = Convert.ToDouble(dr["diningcharge"].ToString());
        return ret;
        //Decimal rnd = 2;
        //ret = Convert.ToDouble(RoundUp(Convert.ToDecimal(ret), rnd));
        
        ////SqlCommand comm = GetCommand(conn, "Select diningcharge from dbo.menuMaster where deleted=0 and Flag='Y'");
        //SqlCommand comm = GetCommand(conn, "Select diningcharge from dbo.menuMaster where deleted=0");
        
    }
    public static Double GetMenuTax(string menuID,double  orgQty)
    {
        Double ret,up,cal;
        SqlConnection conn = getConnection();
        conn.Open();
        SqlCommand comm = GetCommand(conn, "Select diningcharge,UnitPrice from menuMaster where ID='" + menuID + "' and deleted=0");
        SqlDataReader dr = comm.ExecuteReader();
        dr.Read();
        ret = Convert.ToDouble(dr["diningcharge"].ToString());
        up = Convert.ToDouble(dr["UnitPrice"].ToString());
        cal = orgQty * up;
        ret = cal * ret / 100;
        return ret;
    }
    public static Double GetSumTax(string id)
    {
        Double  per;
        SqlConnection conn = getConnection();
        conn.Open();
        SqlCommand comm = GetCommand(conn, "Select sum(diningcharge) as diningcharge from dbo.orderdetail where deleted=0 and DETAILID='" + id + "' ");
        SqlDataReader dr = comm.ExecuteReader();
        dr.Read();
        per = Convert.ToDouble(dr["diningcharge"].ToString());
        //ret = (ordPrice * (per / 100));
        //Decimal rnd = 2;
        //ret = Convert.ToDouble(RoundUp(Convert.ToDecimal(ret), rnd));
        return per;
    }
    //public static Double GetTax(string ordid, double ordPrice)
    //{
    //    Double ret, per;
    //    SqlConnection conn = getConnection();
    //    conn.Open();
    //    SqlCommand comm = GetCommand(conn, "Select sum(Percentage) as perc from dbo.taxMaster where deleted=0 and Flag = 'Y'");
    //    SqlDataReader dr = comm.ExecuteReader();
    //    dr.Read();
    //    per = Convert.ToDouble(dr["perc"].ToString());
    //    ret = (ordPrice * (per / 100));
    //    //Decimal rnd = 2;
    //    //ret = Convert.ToDouble(RoundUp(Convert.ToDecimal(ret), rnd));
    //    return ret;
    //}

    public static Double GetDiscount(string menuID, double ordPrice)
    {
        Double ret, dis;
        SqlConnection conn = getConnection();
        conn.Open();
        SqlCommand comm = GetCommand(conn, "Select Discount from dbo.menuMaster where deleted=0 and ID = " + menuID);
        SqlDataReader dr = comm.ExecuteReader();
        dr.Read();
        if (dr["Discount"].ToString() != "")
            dis = Convert.ToDouble(dr["Discount"]);
        else
            dis = 0;
        ret = (ordPrice * (dis / 100));
        return ret;
    }

    public static Double GetOrderPrice(string id)
    {
        SqlConnection conn = getConnection();
        conn.Open();
        SqlCommand comm = GetCommand(conn, "Select sum(Price) as Price from dbo.orderDetail where deleted=0 and OrderID = " + id);
        SqlDataReader dr = comm.ExecuteReader();
        dr.Read();
        return Convert.ToDouble(dr["Price"]);
    }

    public static Double GetDeductionFromOrderDetail(string id)
    {
        SqlConnection conn = getConnection();
        conn.Open();
        SqlCommand comm = GetCommand(conn, "Select Deduction from dbo.orderDetail where deleted=0 and DetailID = " + id);
        SqlDataReader dr = comm.ExecuteReader();
        dr.Read();
        return Convert.ToDouble(dr["Deduction"]);
    }

    public static void UpdateTaxAdjustment(string taxAdj, string taxOrg, string OID)
    {
        SqlConnection conn = getConnection();
        conn.Open();
        string sql = "Update dbo.OrderMaster set TaxAdj = '" + taxAdj + "', TaxOrg = '" + taxOrg + "' Where OrderID = " + OID;
        SqlCommand comm = GetCommand(conn, sql);
        int ret = comm.ExecuteNonQuery();
        conn.Close();
    }
    public static void UpdateDeductionInOrderMaster(string OID, string TotDed)
    {
        SqlConnection conn = getConnection();
        conn.Open();
        string sql = "Update dbo.OrderMaster set TotDed = " + TotDed + " Where OrderID = " + OID;
        SqlCommand comm = GetCommand(conn, sql);
        int ret = comm.ExecuteNonQuery();
        conn.Close();
    }

    public static DataSet getOrderRecieptDetails(string OID)
    {
        SqlConnection conn = getConnection();
        //String sql = "Select OM.*, UM.UID ";
        //sql = sql + "from dbo.orderMaster OM, dbo.userMaster UM ";    
        //sql = sql + "Where OM.OrderID = " + OID + " and cast(OM.UID as varchar) = UM.UID ";

        String sql = "Select OM.*, UM.user_aid ";
        sql = sql + "from dbo.orderMaster OM, dbo.Users_tbl UM ";
        sql = sql + "Where OM.OrderID = " + OID + " and OM.status='P' and OM.Deleted='0'";

        SqlCommand comm = GetCommand(conn, sql);
        SqlDataAdapter da = new SqlDataAdapter(comm);
        DataSet ds = new DataSet();
        da.Fill(ds, "ORDER");
        conn.Close();
        return ds;
    }
    public static Double GetDeductionFromOrderMaster(string id)
    {
        SqlConnection conn = getConnection();
        conn.Open();
        SqlCommand comm = GetCommand(conn, "Select TotDed from dbo.orderMaster where deleted=0 and OrderID = " + id);
        SqlDataReader dr = comm.ExecuteReader();
        if (dr.Read())
        {
            // dr.Read();
            if (dr["TotDed"] == "")
            {
                return Convert.ToDouble(0);
            }
            else
            {
                return Convert.ToDouble(dr["TotDed"].ToString());
            }
        }
        else
        {
            return Convert.ToDouble(0);
        }
    

    }

}