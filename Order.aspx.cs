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
using Stimulsoft.Report;
using Stimulsoft.Report.Web;
using Stimulsoft.Report.Viewer;
using Stimulsoft.Report.SaveLoad;
using Stimulsoft.Report.Export;
using Stimulsoft.Report.Print;
using Stimulsoft.Base;
using Stimulsoft.Controls;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Controls;
using System.Net.Mail;
using System.Collections.Generic;
using System.Drawing.Printing;



public partial class Order : System.Web.UI.Page
{

    private string uidSource, uidTarget;

    public static string tbno = "1";

    public static string uid = "0";

    //string appPath = "";
    //public static string ordid = "1";

    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            if (!(string.IsNullOrEmpty(Session["sesUserID"].ToString())))
            {
                uid = Session["sesUserID"].ToString().Trim();
                //DatePickerselect.SelectedDate = System.DateTime.Now;
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

        //Session["sesUserID"] = "1";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(Session["sesUserID"].ToString()))
            {



                //appPath = Request.PhysicalApplicationPath;
                Response.Redirect("Login.aspx");
            }
            else
            {

                //loadtable();
                lblStatus.Text = "";
            }
        }
        catch (Exception ex)
        {
            Response.Redirect("Login.aspx");
        }
    }

    public Dictionary<string, string> drows = new Dictionary<string, string>();

    //Dictionary<string, Dictionary<string,string>> data;

    public class Rows
    {
        public string ID;
        public string name;
        public string quantity;
        public string price;

        
    }



    public struct Rows2
    {

        public string total;
        Rows[] r;
    }


    protected void cboTable_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        //GridItemEventArgs e1 = (GridItemEventArgs)e;
        //       RadComboBoxSelectedIndexChangedEventArgs e1 = RadComboBoxSelectedIndexChangedEventArgs(e);
        //GridEditFormItem editedItem = e1.Item as GridEditFormItem;

        //RadComboBox tbno1 = (RadComboBox)editedItem.FindControl("txtTblNo");
        RadComboBox tbno1 = (RadComboBox)sender;
        tbno = tbno1.SelectedValue.ToString();
    }


    protected void submitOrderServer(object sender, EventArgs e)
    {
       
        RadComboBox tbno1 = (RadComboBox)Page.FindControl("txtTblNo");
        tbno = tbno1.SelectedValue.ToString();
       // postOrder(data.rows, MyMethod_Result);
        tbno1.ClearSelection();
        

    }

    public static decimal round5cen(double tax1)
    {
        double rem = (tax1 % .05);
        if (rem >= .025)
            return (decimal)(tax1 + .05 - rem);
        else
            return (decimal)(tax1 - rem);
    }


    [System.Web.Services.WebMethod(EnableSession = false)]
    //[System.Web.Services.WebMethod]
    //public static string postOrder(Rows[] rdata)
    //public static string postOrder(string rdata)
    public static string postOrder(Rows[] rdata)
    {
        try
        {
            int l = rdata.Length;
            string val;
            string ID1;
            string name;
            int quantity;
            decimal price;
            Rows[] r = new Rows[rdata.Length];
            decimal total = 0;
            decimal orderprice = 0;
            decimal taxorig = 0;
            decimal gstorig = 0;
            decimal tax = 0;
            decimal gst = 0;
            int orderid = 0;
            decimal discount = 0;
            decimal diningcharge = 0;
            decimal totaladj = 0;
            decimal tot = 0;
            decimal totdiningcharge = 0;
            decimal totdiscount = 0;
            Boolean flg = true;
            decimal orderpriceafterded = 0;
            

            if (rdata.Length > 0)
            {
                SqlConnection c1 = BusinessTier.getConnection();
                c1.Open();
                System.Data.DataTable dtItems = new System.Data.DataTable();
                String sql = "select max(OrderID) as mid from OrderMaster";
                dtItems = new System.Data.DataTable();
                dtItems.Clear();

                System.Data.SqlClient.SqlDataAdapter sqlDataAdapter = new System.Data.SqlClient.SqlDataAdapter(sql, c1);
                sqlDataAdapter.Fill(dtItems);
                int rco = dtItems.Rows.Count;
                foreach (System.Data.DataRow dataRow in dtItems.Rows)
                {
                    orderid = (Int32.Parse(dataRow["mid"].ToString())) + 1;
                }
                c1.Close();
                dtItems.Clear();
                sqlDataAdapter.Dispose();
            }

            for (int i = 0; i < rdata.Length; i++)
            {
                l = rdata.Length;
                ID1 = rdata[i].ID;
                name = rdata[i].name;
                quantity = Int32.Parse(rdata[i].quantity);
                price = decimal.Parse(rdata[i].price);
                ///////////////////////////////////////////////////////////////////////////////////////////////
                SqlConnection c3 = BusinessTier.getConnection();
                c3.Open();
                System.Data.DataTable dtItems3 = new System.Data.DataTable();
                String sql3 = "select * from OrderMaster where Table_ID='" + tbno + "' and Status='P' and deleted = 0 and REPLACE(CONVERT(VARCHAR,OrdDate,106),' ','/') ='" + System.DateTime.Now.ToString("dd/MMM/yyyy") + "' ";
                dtItems3 = new System.Data.DataTable();
                dtItems3.Clear();
                System.Data.SqlClient.SqlDataAdapter sqlDataAdapter3 = new System.Data.SqlClient.SqlDataAdapter(sql3, c3);
                sqlDataAdapter3.Fill(dtItems3);
                int rco3 = dtItems3.Rows.Count;
                if (rco3 > 0)
                {
                    foreach (System.Data.DataRow dataRow3 in dtItems3.Rows)
                    {
                        orderid = Int32.Parse(dataRow3["OrderID"].ToString());
                        flg = false;
                    }
                }
                dtItems3.Clear();
                sqlDataAdapter3.Dispose();
                c3.Close();
                ///////////////////////////////////////////////////MENU MASTER DISCOUNT////////////////////////////////////////////////
                SqlConnection c2 = BusinessTier.getConnection();
                c2.Open();
                System.Data.DataTable dtItems2 = new System.Data.DataTable();
                String sql2 = "select discount,diningcharge from MenuMaster where ID='" + ID1 + "'";
                dtItems2 = new System.Data.DataTable();
                dtItems2.Clear();
                System.Data.SqlClient.SqlDataAdapter sqlDataAdapter2 = new System.Data.SqlClient.SqlDataAdapter(sql2, c2);
                sqlDataAdapter2.Fill(dtItems2);
                int rco = dtItems2.Rows.Count;
                foreach (System.Data.DataRow dataRow in dtItems2.Rows)
                {
                    discount = decimal.Parse(dataRow["discount"].ToString());

                    diningcharge = decimal.Parse(dataRow["diningcharge"].ToString());

                    discount = price * quantity * discount / 100;
                    discount = decimal.Round(discount, 2, MidpointRounding.ToEven);
                    //discount = Math.Round((Math.Round(quantity*discount * 20, MidpointRounding.AwayFromZero) / 20), 1);
                    //discount = round5cen((double)discount);
                    totdiscount = totdiscount + discount;

                    diningcharge = price * quantity * diningcharge / 100;
                    taxorig =taxorig + diningcharge;
                    taxorig = decimal.Round(taxorig, 2, MidpointRounding.ToEven);
                    diningcharge = decimal.Round(diningcharge, 2, MidpointRounding.ToEven);
                   // diningcharge = round5cen((double)diningcharge);
                    totdiningcharge = totdiningcharge + diningcharge;
                    //discount = Math.Round((Math.Round(discount * 20, MidpointRounding.AwayFromZero) / 20), 1);
                    //totdiscount = totdiscount + quantity*discount;
                }
                
                dtItems2.Clear();
                sqlDataAdapter2.Dispose();
                c2.Close();

                //price = decimal.Round(price, 2);
                //total = total + (quantity * price);

                price = (decimal)(quantity * price);
                price = decimal.Round(price, 2);
                total = total + price;
                total = Math.Round(total, 2);
                orderprice = total;
                //totdiningcharge = totdiningcharge + diningchargebyitem;
                //if (flg == true)
                BusinessTier.SaveOrderDetail(null, price, Int32.Parse(ID1), discount,diningcharge , "Insert", uid.ToString(), long.Parse(orderid.ToString()), quantity);
                //else
                //{

                //}
                if (i == (rdata.Length - 1))
                {
                    //taxorig = orderprice * 6 / 100;
                    //taxorig = decimal.Round(taxorig, 2, MidpointRounding.ToEven);
                    //tax = Math.Round((Math.Round(taxorig * 20, MidpointRounding.ToEven) / 20), 1);
                    //total = orderprice + (decimal)tax- totdiscount;

                    ///////////////////////////////////////////////////////////////////////////////////////////////
                    if (flg == false)
                    {
                        SqlConnection c4 = BusinessTier.getConnection();
                        c4.Open();
                        System.Data.DataTable dtItems4 = new System.Data.DataTable();
                        String sql4 = "select * from OrderDetail where OrderID='" + orderid + "' and deleted = 0";
                        dtItems4 = new System.Data.DataTable();
                        dtItems4.Clear();

                        System.Data.SqlClient.SqlDataAdapter sqlDataAdapter4 = new System.Data.SqlClient.SqlDataAdapter(sql4, c4);
                        sqlDataAdapter4.Fill(dtItems4);
                        int rco4 = dtItems4.Rows.Count;
                        decimal DedU = 0;
                        decimal TotDedU = 0;
                        decimal PriceU = 0;
                        decimal TaxOrigU = 0;
                        decimal TaxU = 0;
                        decimal OrdPriceU = 0;
                        decimal TotPriceU = 0;
                      
                        if (rco4 > 0)
                        {
                            foreach (System.Data.DataRow dataRow4 in dtItems4.Rows)
                            {
                                PriceU = decimal.Parse(dataRow4["Price"].ToString());
                                DedU = decimal.Parse(dataRow4["Deduction"].ToString());
                                OrdPriceU = OrdPriceU + PriceU;
                                TotDedU = TotDedU + DedU;

                            }
                            orderprice = OrdPriceU;
                            totdiscount = TotDedU;
                           // orderpriceafterded = orderprice - totdiscount;
                        }
                        //dtItems3.Clear();
                        //sqlDataAdapter3.Dispose();
                        //c3.Close();
                        dtItems4.Clear();
                        sqlDataAdapter4.Dispose();
                        c4.Close();
                    }

                    ///////////////////////////////////////////////////////////////////////////////////////////////////
                   // orderpriceafterded = orderprice - totdiscount;
                    //diningcharge = orderpriceafterded * 6 / 100;
                    //diningcharge = decimal.Round(diningcharge, 2, MidpointRounding.ToEven);


                    orderpriceafterded = orderprice - totdiscount;
                    //taxorig = orderpriceafterded * 6 / 100;
                    //taxorig = decimal.Round(taxorig, 2, MidpointRounding.ToEven);
                    //tax = Math.Round((Math.Round(taxorig * 20, MidpointRounding.ToEven) / 20), 1);
                    //tax = round5cen((double)totdiningcharge);
                    tax = totdiningcharge;
                    //GST
                    gstorig = orderpriceafterded * 6 / 100;
                    gstorig = decimal.Round(gstorig, 2, MidpointRounding.ToEven);
                    //gst = round5cen((double)gstorig);
                    gst = gst + gstorig;

                    tot = orderpriceafterded + (decimal)tax + (decimal)gst;
                   
                                   
                    total = round5cen((double)tot);
                    totaladj = tot - total;
                   // total = orderprice + (decimal)tax - totdiscount;

                    if (flg == true)
                        BusinessTier.SaveOrder(null, tbno, taxorig, tax,gst, orderprice,totaladj , total, null, "Insert", uid, null, totdiscount, tax );
                    else
                    {
                        BusinessTier.SaveOrder(null, tbno, taxorig, tax,gst, orderprice, totaladj,total, null, "Update", uid, orderid.ToString(), totdiscount, tax);
                    }
                    Stimulsoft.Report.StiReport stiReport1;
                    
                    DataSet read = new DataSet();
                    PrinterSettings printerSettings = new PrinterSettings();
                    SqlConnection c5 = BusinessTier.getConnection();
                    c5.Open();
                    System.Data.DataTable dtItems5 = new System.Data.DataTable();
                    String sql5 = "SELECT dbo.orderDetail.OrderID, dbo.orderDetail.MenuID, dbo.orderDetail.DetailID, dbo.orderDetail.Qty, dbo.orderDetail.Price, dbo.orderDetail.Deduction, dbo.orderDetail.Created_By, dbo.orderMaster.OrderID AS Expr1, dbo.orderMaster.TotalPrice, dbo.orderMaster.Tax, dbo.orderMaster.OrdDate, dbo.orderMaster.Waiter, dbo.orderMaster.TaxOrg, dbo.orderMaster.TotDed, dbo.orderMaster.Table_ID, dbo.kitchenMaster.Name, dbo.kitchenMaster.ID, dbo.menuMaster.Name AS Expr2, dbo.menuMaster.ID AS Expr3, dbo.categoryMaster.Kitchen, dbo.menuMaster.Category, dbo.categoryMaster.category_ID, dbo.Users_tbl.contact_name FROM 	dbo.orderMaster INNER JOIN dbo.orderDetail ON dbo.orderDetail.OrderID = dbo.orderMaster.OrderID INNER JOIN dbo.menuMaster  ON dbo.menuMaster.ID = dbo.orderDetail.MenuID INNER JOIN dbo.categoryMaster on dbo.menuMaster.Category = dbo.categoryMaster.category_ID INNER JOIN dbo.kitchenMaster ON dbo.categoryMaster.Kitchen = dbo.kitchenMaster.ID INNER JOIN dbo.Users_tbl on dbo.Users_tbl.user_aid=dbo.orderMaster.waiter where dbo.orderMaster.OrderID=" + orderid.ToString().Trim() + " and dbo.kitchenMaster.ID =10";
                    dtItems5 = new System.Data.DataTable();
                    dtItems5.Clear();

                    System.Data.SqlClient.SqlDataAdapter sqlDataAdapter5 = new System.Data.SqlClient.SqlDataAdapter(sql5, c5);
                    sqlDataAdapter5.Fill(dtItems5);
                    int rco5 = dtItems5.Rows.Count;
                    
                    if (rco5 > 0)
                    {
                        
                        stiReport1 = new StiReport();

                        //DataSet read = new DataSet();

                       // stiReport1.Load(appPath + "\\Reports\\KITCHEN.mrt");
                        //stiReport1.Load(appPath + "\\Reports\\KITCHEN.mrt");
                        stiReport1.Load("c:\\inetpub\\wwwroot\\RestaurantGST\\Reports\\KITCHEN.mrt");
                        
                        //stiReport1.Load("c:\\inetpub\\wwwroot\\RestaurantSS\\Reports\\KITCHEN.mrt");
                        ////stiReport1.Dictionary.Databases.Clear();
                        ////stiReport1.Dictionary.Databases.Add(new StiSqlDatabase("Connection", con));
                        ////stiReport1.RegData("DataSource1", dsf);
                        read = new DataSet();
                        stiReport1.RegData(read);
                        ////stiReport1.Dictionary.Synchronize();
                        stiReport1.Compile();

                        stiReport1.CompiledReport.DataSources["DataSource1"].Parameters["@OrdID"].ParameterValue = Convert.ToInt32(orderid.ToString().Trim());
                        stiReport1.CompiledReport.DataSources["DataSource1"].Parameters["@KID"].ParameterValue = 10;
                        dtItems3.Clear();
                        sqlDataAdapter3.Dispose();
                        c3.Close();
                        stiReport1.Render();
                       

                        printerSettings = new PrinterSettings();
                       // printerSettings.PrinterName = "FOOD";
                        //printerSettings.PrinterName = "Canon iR C2880/C3380"; //"Microsoft XPS Document Writer";//  //"iR C3200";
                        printerSettings.PrinterName = ConfigurationManager.AppSettings["Food"].ToString();
                        printerSettings.Copies = 1;
                        // printerSettings.FromPage = 1;
                        printerSettings.ToPage = stiReport1.RenderedPages.Count;
                        stiReport1.Print(false, printerSettings);

                    }

                    SqlConnection c6 = BusinessTier.getConnection();
                    c6.Open();
                    System.Data.DataTable dtItems6 = new System.Data.DataTable();
                    String sql6 = "SELECT dbo.orderDetail.OrderID, dbo.orderDetail.MenuID, dbo.orderDetail.DetailID, dbo.orderDetail.Qty, dbo.orderDetail.Price, dbo.orderDetail.Deduction, dbo.orderDetail.Created_By, dbo.orderMaster.OrderID AS Expr1, dbo.orderMaster.TotalPrice, dbo.orderMaster.Tax, dbo.orderMaster.OrdDate, dbo.orderMaster.Waiter, dbo.orderMaster.TaxOrg, dbo.orderMaster.TotDed, dbo.orderMaster.Table_ID, dbo.kitchenMaster.Name, dbo.kitchenMaster.ID, dbo.menuMaster.Name AS Expr2, dbo.menuMaster.ID AS Expr3, dbo.categoryMaster.Kitchen, dbo.menuMaster.Category, dbo.categoryMaster.category_ID, dbo.Users_tbl.contact_name FROM 	dbo.orderMaster INNER JOIN dbo.orderDetail ON dbo.orderDetail.OrderID = dbo.orderMaster.OrderID INNER JOIN dbo.menuMaster  ON dbo.menuMaster.ID = dbo.orderDetail.MenuID INNER JOIN dbo.categoryMaster on dbo.menuMaster.Category = dbo.categoryMaster.category_ID INNER JOIN dbo.kitchenMaster ON dbo.categoryMaster.Kitchen = dbo.kitchenMaster.ID INNER JOIN dbo.Users_tbl on dbo.Users_tbl.user_aid=dbo.orderMaster.waiter where dbo.orderMaster.OrderID=" + orderid.ToString().Trim() + "and dbo.kitchenMaster.ID =11";
                    dtItems6 = new System.Data.DataTable();
                    dtItems6.Clear();

                    System.Data.SqlClient.SqlDataAdapter sqlDataAdapter6 = new System.Data.SqlClient.SqlDataAdapter(sql6, c6);
                    sqlDataAdapter6.Fill(dtItems6);
                    int rco6 = dtItems6.Rows.Count;
                   
                    if (rco6 > 0)
                    {
                        stiReport1 = new StiReport();
                       // stiReport1.Load(appPath + "\\Reports\\KITCHEN.mrt");
                        stiReport1.Load("c:\\inetpub\\wwwroot\\RestaurantGST\\Reports\\KITCHEN.mrt");

                        //stiReport1.Load("c:\\inetpub\\wwwroot\\RestaurantSS\\Reports\\KITCHEN.mrt");
                        read = new DataSet();
                        stiReport1.RegData(read);
                        stiReport1.Compile();
                        stiReport1.CompiledReport.DataSources["DataSource1"].Parameters["@OrdID"].ParameterValue = orderid.ToString().Trim();
                        stiReport1.CompiledReport.DataSources["DataSource1"].Parameters["@KID"].ParameterValue = "11";
                        dtItems3.Clear();
                        sqlDataAdapter3.Dispose();
                        c3.Close();
                        stiReport1.Render();
                        

                        printerSettings = new PrinterSettings();
                        //printerSettings.PrinterName = "BAR";
                        //printerSettings.PrinterName = "Canon iR C2880/C3380"; //"Microsoft XPS Document Writer";//  //"iR C3200";
                        printerSettings.PrinterName = ConfigurationManager.AppSettings["BAR"].ToString();
                        printerSettings.Copies = 1;
                        printerSettings.FromPage = 1;
                        printerSettings.ToPage = stiReport1.RenderedPages.Count;
                        stiReport1.Print(false, printerSettings);
                    }

                    SqlConnection conn = BusinessTier.getConnection();
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("update orderdetail set Print_Status='True' where OrderID=" + orderid.ToString().Trim(), conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    //stiReport1 = new StiReport();
                    //stiReport1.Load("c:\\inetpub\\wwwroot\\RestaurantSS\\Reports\\RecieptOrder - Copy.mrt");
                    //read = new DataSet();
                    //stiReport1.RegData(read);
                    //stiReport1.Compile();
                    //Int64 oid = Int64.Parse(orderid.ToString().Trim());
                    //stiReport1.CompiledReport.DataSources["DataSource1"].Parameters["@OrderID"].ParameterValue = oid.ToString().Trim();
                    //stiReport1.Render();
                    //printerSettings = new PrinterSettings();
                    //printerSettings.Copies = 1;
                    //printerSettings.ToPage = stiReport1.RenderedPages.Count;
                    //printerSettings.PrinterName = "Code Soft 31xx Series";
                    //stiReport1.Print(false, printerSettings);


                    
                    string sql1 = "";
                    string sql11 = "";
                    string con = @BusinessTier.getConnection1();
                    sql1 = "SELECT     dbo.orderDetail.OrderID,dbo.orderMaster.GST, dbo.orderDetail.MenuID, dbo.orderDetail.DetailID, dbo.orderDetail.Qty, dbo.orderDetail.Price, dbo.orderDetail.Deduction, 	dbo.orderDetail.Created_By, dbo.orderMaster.OrderID AS Expr1, dbo.orderMaster.TotalPrice, dbo.orderMaster.Tax, dbo.orderMaster.OrdDate, dbo.orderMaster.Waiter,dbo.orderMaster.TaxOrg, dbo.orderMaster.TotDed, dbo.orderMaster.Table_ID, dbo.kitchenMaster.Name, dbo.kitchenMaster.ID, dbo.menuMaster.Name AS Expr2, 	dbo.menuMaster.ID AS Expr3,dbo.menuMaster.unitprice, dbo.categoryMaster.Kitchen, dbo.menuMaster.Category, dbo.categoryMaster.category_ID,	dbo.Users_tbl.contact_name,(dbo.orderMaster.Tax-dbo.orderMaster.TaxOrg) as adj,(dbo.orderMaster.TotalPrice+dbo.orderMaster.Tax) as net,(dbo.orderDetail.Qty * dbo.orderDetail.Price) as mprice FROM dbo.orderMaster INNER JOIN dbo.orderDetail ON dbo.orderDetail.OrderID = dbo.orderMaster.OrderID INNER JOIN dbo.menuMaster  ON dbo.menuMaster.ID = dbo.orderDetail.MenuID INNER JOIN dbo.categoryMaster on dbo.menuMaster.Category = dbo.categoryMaster.category_ID INNER JOIN dbo.kitchenMaster ON dbo.categoryMaster.Kitchen = dbo.kitchenMaster.ID INNER JOIN dbo.Users_tbl on dbo.Users_tbl.user_aid=dbo.orderMaster.waiter where dbo.orderMaster.OrderID='" + orderid.ToString().Trim() + "' ";
                    // sql1 = "select * from orderMaster where  deleted=0 and orddate between '" + FromSalesdate + "' and '" + ToSalesDate + "' and Status='R'";
                    sql11 = "select * from restaurantmaster";
                 
                    Stimulsoft.Report.StiReport stiReport2;
                   
                    SqlDataAdapter ad1 = new SqlDataAdapter(sql1, con);
                    SqlDataAdapter ad2 = new SqlDataAdapter(sql11, con);
                 

                    DataSet ds1 = new DataSet();
                    DataSet ds2 = new DataSet();
                    

                    ds1.DataSetName = "DynamicDataSource1";
                    ds1.Tables.Add("DataSource1");
                    ad1.Fill(ds1, "DataSource1");

                    ds2.DataSetName = "DynamicDataSource2";
                    ds2.Tables.Add("DataSource2");
                    ad2.Fill(ds2, "DataSource2");


                    stiReport2 = new StiReport();
                    stiReport2.Dictionary.DataStore.Clear();
                    stiReport2.ClearAllStates();
                   // stiReport1.Load(appPath + "\\Reports\\RecieptOrder.mrt");
                    
                    //stiReport2.Load(appPath + "\\Reports\\ReceiptCashier.mrt");
                    stiReport2.Load("c:\\inetpub\\wwwroot\\RestaurantGST\\Reports\\RecieptOrder.mrt");

                   // stiReport2.Load("c:\\inetpub\\wwwroot\\Restaurantss\\Reports\\RecieptOrder.mrt");
                    stiReport2.Dictionary.Databases.Clear();
                    stiReport2.Dictionary.Databases.Add(new StiSqlDatabase("Connection", con));
                    stiReport2.Dictionary.DataSources.Clear();

                    stiReport2.RegData("DataSource1", ds1);
                    stiReport2.RegData("DataSource2", ds2);


                    stiReport2.Dictionary.Synchronize();
                    stiReport2.Compile();
                    stiReport2.Render();
                    PrinterSettings printerSettings1 = new PrinterSettings();
                    printerSettings1.Copies = 1;
                    //printerSettings.FromPage = 1;
                    printerSettings1.ToPage = stiReport2.RenderedPages.Count;
                    // printerSettings.PrinterName = "Samsung SCX-3400 Series";
                    printerSettings1.PrinterName = ConfigurationManager.AppSettings["Order"].ToString();
                    //printerSettings1.PrinterName = "Canon iR C2880/C3380";
                    // printerSettings.PrinterName
                    stiReport2.Print(false, printerSettings1);
                  //  StiWebViewer1.Report = stiReport2;
                   // StiWebViewer1.ViewMode = StiWebViewMode.WholeReport;
                    stiReport2.Dispose();




                }

            }


        }
        catch (Exception ex)
        {


        }
        return null;
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        string orderprintid = "";
        SqlConnection conn1 = BusinessTier.getConnection();
        string appPath = Request.PhysicalApplicationPath;
        try
        {
            conn1.Open();
            string sql1 = "select OrderID FROM OrderMaster where deleted=0 and status= 'P' and Table_ID = '" + txtTblNo.SelectedValue + "' and REPLACE(CONVERT(VARCHAR,OrdDate,106),' ','/') ='" + System.DateTime.Now.ToString("dd/MMM/yyyy") + "'";
            SqlCommand command1 = new SqlCommand(sql1, conn1);
            SqlDataReader reader1 = command1.ExecuteReader();
            if (reader1.Read())
            {
                orderprintid = reader1["OrderID"].ToString().Trim();
            }
            reader1.Close();
            BusinessTier.DisposeReader(reader1);
            BusinessTier.DisposeConnection(conn1);
            if (string.IsNullOrEmpty(orderprintid))
            {
                lblStatus.Text = "Select Different Table No";
                return;
            }

            else
            {
                Stimulsoft.Report.StiReport stiReport1;
                DataSet read = new DataSet();
                PrinterSettings printerSettings = new PrinterSettings();
                stiReport1 = new StiReport();
                //stiReport1.Load(appPath + "\\Reports\\RecieptOrder.mrt");

                stiReport1.Load("c:\\inetpub\\wwwroot\\RestaurantGST\\Reports\\RecieptOrder.mrt");

                //stiReport1.Load("c:\\inetpub\\wwwroot\\RestaurantSS\\Reports\\RecieptOrder.mrt");
                read = new DataSet();
                stiReport1.RegData(read);
                stiReport1.Compile();
                //  Int64 oid = Int64.Parse(ordid.ToString().Trim());
                stiReport1.CompiledReport.DataSources["DataSource1"].Parameters["@OrderID"].ParameterValue = orderprintid.ToString().Trim();
                stiReport1.Render();
                printerSettings = new PrinterSettings();
                printerSettings.Copies = 1;
                printerSettings.ToPage = stiReport1.RenderedPages.Count;
                //printerSettings.PrinterName = "Code Soft 31xx Series";
                //printerSettings.PrinterName = "Canon iR C2880/C3380";
                printerSettings.PrinterName = ConfigurationManager.AppSettings["Order"].ToString();
                // printerSettings.PrinterName = "Samsung SCX-3400 Series";
                stiReport1.Print(false, printerSettings);
            }
        }
        catch (Exception ex)
        {
            BusinessTier.DisposeConnection(conn1);

        }
    }


}
