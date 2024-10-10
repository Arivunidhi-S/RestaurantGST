using System;
using System.Collections.Generic;
using System.Linq;
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
using System.IO;
using System.Web.SessionState;
using System.Runtime;
using System.Drawing.Printing;

public partial class Report : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        String To = txtToDate.SelectedDate.ToString();
        To = System.DateTime.Now.ToString();
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
    protected void OnSelectedCompo(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (txtReport.SelectedValue.ToString() == "Today")
        {
            hidden();
            cbocagtegory.Enabled = true;
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            txtFromDate.SelectedDate = System.DateTime.Now;
            txtToDate.SelectedDate = System.DateTime.Now;
        }
        if (txtReport.SelectedValue.ToString() == "SalesReportDateTime")
        {
            hidden();
            cbocagtegory.Enabled = true;
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;
            txtToDate.SelectedDate = System.DateTime.Now;
            txtFromDate.SelectedDate = System.DateTime.Now;
        }
        if (txtReport.SelectedValue.ToString() == "ItemwiseReport")
        {
            hidden();
            cbocagtegory.Enabled = true;
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;
            txtToDate.SelectedDate = System.DateTime.Now;

        }
        if (txtReport.SelectedValue.ToString() == "SummaryReport")
        {
            hidden();
            cbocagtegory.Enabled = true;
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;
            txtToDate.SelectedDate = System.DateTime.Now;

        }
        if (txtReport.SelectedValue.ToString() == "Dining Charge Report")
        {
            hidden();
            cbocagtegory.Enabled = false;
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;
            txtToDate.SelectedDate = System.DateTime.Now;
        }

        //Gst report
        if (txtReport.SelectedValue.ToString() == "Gst Report")
        {
            hidden();
            cbocagtegory.Enabled = false;
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;
            txtToDate.SelectedDate = System.DateTime.Now;
        }
        if (txtReport.SelectedValue.ToString() == "PLUReport")
        {
            hidden();
            cbocagtegory.Enabled = true;
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;
            txtToDate.SelectedDate = System.DateTime.Now;
        }
        if (txtReport.SelectedValue.ToString() == "VOIDReport")
        {
            hidden();
            cbocagtegory.Enabled = false;
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;
            txtToDate.SelectedDate = System.DateTime.Now;

        }
    }
    protected void hidden()
    {
        Stimulsoft.Report.StiReport stiReport1;
        stiReport1 = new StiReport();
        stiReport1.Dictionary.DataStore.Clear();
        cbocagtegory.Enabled = false;
        txtFromDate.Enabled = false;
        txtToDate.Enabled = false;

    }
    //protected void btnprint_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        StiReport report = new StiReport();
    //           //  Stimulsoft.Report.StiReport stiReport1;
    //          StiWebViewer1.Report = report;
    //         StiWebViewer1.ViewMode = StiWebViewMode.WholeReport;
    //           // stiReport1.Dispose();          
    //        PrinterSettings printerSettings = new PrinterSettings();
    //        printerSettings.Copies = 1;
    //        //printerSettings.FromPage = 1;
    //        printerSettings.ToPage = report.RenderedPages.Count;
    //        printerSettings.PrinterName = "Samsung SCX-3400 Series";
    //       // printerSettings.PrinterName = ConfigurationManager.AppSettings["CodeSoft"].ToString();
    //        // printerSettings.PrinterName
    //        report.Print(false, printerSettings);
    //    }

    //    catch (Exception ex)
    //    {
    //        lblStatus.Text = "" + ex.Message.ToString();
    //    }
    //}
    protected void btnprint_Click(object sender, EventArgs e)
    {
        try
        {
            lblMsg.Text = "";
            string sql1 = "";
            string sql2 = "";
            string sql3 = "";
            String Report = txtReport.Text.ToString();
            string appPath = Request.PhysicalApplicationPath;

            if (Report == "")
            {
                lblStatus.Text = "Please Select Report";
                return;
            }
            string con = @BusinessTier.getConnection1();
            DateTime fromDate = Convert.ToDateTime(txtFromDate.SelectedDate.ToString());
            if ((string.IsNullOrEmpty(fromDate.ToString())))
            {
                lblStatus.Text = "Select From date";
                return;
            }
            string FromSalesdate = fromDate.Month + "/" + fromDate.Day + "/" + fromDate.Year;

            DateTime toDate = Convert.ToDateTime(txtToDate.SelectedDate.ToString());
            if ((string.IsNullOrEmpty(toDate.ToString())))
            {
                lblStatus.Text = "Select To date";
                return;
            }

            string ToSalesDate = toDate.Month + "/" + toDate.Day + "/" + toDate.Year + " " + "23:59:59";
            string ToSalesDate1 = toDate.Month + "/" + toDate.Day + "/" + toDate.Year;

            if (Report.Equals("Today"))//===================>today
            {
                DateTime dtTooDate = System.DateTime.Now;
                string todaydate = dtTooDate.Month + "/" + dtTooDate.Day + "/" + dtTooDate.Year;
                string todaydate1 = dtTooDate.Month + "/" + dtTooDate.Day + "/" + dtTooDate.Year + " " + "23:59:59";
                if (cbocagtegory.Text != "---All---")
                {
                    sql1 = "SELECT * from VW_salesreportone where orddate between '" + todaydate + "' and'" + todaydate1 + "' and category_id='" + cbocagtegory.SelectedValue + "' and  status='R' and deleted=0";
                }
                else
                {
                    sql1 = "SELECT * from VW_salesreportone where orddate between '" + todaydate + "' and'" + todaydate1 + "'  and  status='R' and deleted=0";
                }
                // sql1 = "SELECT * from VW_salesreportone where  orddate between '2010-07-16' and'2010-07-16 23:59:59' and category_id=28 and status='R' and deleted=0";
                Stimulsoft.Report.StiReport stiReport1;
                SqlDataAdapter ad1 = new SqlDataAdapter(sql1, con);
                DataSet ds1 = new DataSet();
                ds1.DataSetName = "DynamicDataSource1";
                ds1.Tables.Add("VW_salesreportone");
                ad1.Fill(ds1, "VW_salesreportone");
                stiReport1 = new StiReport();
                stiReport1.Dictionary.DataStore.Clear();
                stiReport1.ClearAllStates();
                stiReport1.Load(appPath + "\\Reports\\today.mrt");
                //stiReport1.Load("c:\\inetpub\\wwwroot\\RestaurantJey\\Reports\\today.mrt");

                //stiReport1.Load("c:\\inetpub\\wwwroot\\Restaurantss\\Reports\\today.mrt");
                stiReport1.Dictionary.Databases.Clear();
                stiReport1.Dictionary.Databases.Add(new StiSqlDatabase("Connection", con));
                stiReport1.Dictionary.DataSources.Clear();
                stiReport1.RegData("VW_salesreportone", ds1);



                stiReport1.Dictionary.Synchronize();
                stiReport1.Compile();
                stiReport1.Render();
                PrinterSettings printerSettings = new PrinterSettings();
                printerSettings.Copies = 1;
                //printerSettings.FromPage = 1;
                printerSettings.ToPage = stiReport1.RenderedPages.Count;
                // printerSettings.PrinterName = "Samsung SCX-3400 Series";
                printerSettings.PrinterName = ConfigurationManager.AppSettings["CodeSoft"].ToString();
                // printerSettings.PrinterName
                stiReport1.Print(false, printerSettings);
                StiWebViewer1.Report = stiReport1;
                StiWebViewer1.ViewMode = StiWebViewMode.WholeReport;
                stiReport1.Dispose();




                ////////stiReport1.Dictionary.Synchronize();
                ////////stiReport1.Compile();
                ////////stiReport1.Render();
                ////////StiWebViewer1.Report = stiReport1;
                ////////StiWebViewer1.ViewMode = StiWebViewMode.WholeReport;
                ////////stiReport1.Dispose();
                ////////PrinterSettings printerSettings = new PrinterSettings();
                ////////printerSettings.Copies = 1;
                //////////printerSettings.FromPage = 1;
                ////////printerSettings.ToPage = stiReport1.RenderedPages.Count;
                ////////// printerSettings.PrinterName = "Samsung SCX-3400 Series";
                ////////printerSettings.PrinterName = ConfigurationManager.AppSettings["CodeSoft"].ToString();
                ////////// printerSettings.PrinterName
                ////////stiReport1.Print(false, printerSettings);


            }
            //----------------------------------------------------------------
            else if (Report.Equals("Sales Report DateTime"))//Sales Report DateTime
            {
                if ((string.IsNullOrEmpty(fromDate.ToString())))
                {
                    lblStatus.Text = "Select From date";
                    return;
                }
                StiWebViewer1.Visible = true;
                string strReport = Request.QueryString.Get("rpt");
                StiReport report = new StiReport();
                DataSet read = new DataSet();
                if (cbocagtegory.Text != "---All---")
                {
                    // sql1 = "SELECT * from VW_salesreportone where category_id='" + cbocagtegory.SelectedValue + "' and orddate between '" + FromSalesdate + "' and '" + ToSalesDate + "' and deleted=0 and status='R'";
                    report.Load(appPath + "\\Reports\\SalesReportAll - Copy.mrt");
                    //report.Load("c:\\inetpub\\wwwroot\\RestaurantJey\\Reports\\SalesReportAll - Copy.mrt");

                    //report.Load("c:\\inetpub\\wwwroot\\RestaurantSS\\Reports\\SalesReportAll - Copy.mrt");
                    report.RegData(read);
                    report.Compile();
                    report.CompiledReport.DataSources["VW_salesreportone"].Parameters["@Ordfrmdate"].ParameterValue = FromSalesdate;
                    report.CompiledReport.DataSources["VW_salesreportone"].Parameters["@OrdTodate"].ParameterValue = ToSalesDate;
                    report.CompiledReport.DataSources["VW_salesreportone"].Parameters["@OrdTodate1"].ParameterValue = ToSalesDate1;
                    report.CompiledReport.DataSources["VW_salesreportone"].Parameters["@Ordcat"].ParameterValue = cbocagtegory.SelectedValue;
                }
                else
                {
                    // sql1 = "SELECT * from VW_salesreportone where deleted=0 and orddate between '" + FromSalesdate + "' and '" + ToSalesDate + "' and status='R'";
                    report.Load(appPath + "\\Reports\\SalesReportAll.mrt");
                    //report.Load("c:\\inetpub\\wwwroot\\RestaurantJey\\Reports\\SalesReportAll.mrt");

                    //report.Load("c:\\inetpub\\wwwroot\\RestaurantSS\\Reports\\SalesReportAll.mrt");
                    report.RegData(read);
                    report.Compile();
                    report.CompiledReport.DataSources["VW_salesreportone"].Parameters["@Ordfrmdate"].ParameterValue = FromSalesdate;
                    report.CompiledReport.DataSources["VW_salesreportone"].Parameters["@OrdTodate"].ParameterValue = ToSalesDate;
                    report.CompiledReport.DataSources["VW_salesreportone"].Parameters["@OrdTodate1"].ParameterValue = ToSalesDate1;
                    // report.CompiledReport.DataSources["VW_salesreportone"].Parameters["@Ordcat"].ParameterValue = cbocagtegory.SelectedValue;  
                }
                // report.Load("c:\\inetpub\\wwwroot\\RestaurantSS\\Reports\\ReceiptCashier.mrt");



                report.Dictionary.Synchronize();
                report.Render();
                PrinterSettings printerSettings = new PrinterSettings();
                printerSettings.Copies = 1;
                //printerSettings.FromPage = 1;
                printerSettings.ToPage = report.RenderedPages.Count;
                // printerSettings.PrinterName = "Samsung SCX-3400 Series";
                printerSettings.PrinterName = ConfigurationManager.AppSettings["CodeSoft"].ToString();
                // printerSettings.PrinterName
                report.Print(false, printerSettings);
                StiWebViewer1.Report = report;
                StiWebViewer1.ViewMode = StiWebViewMode.WholeReport;
                report.Dispose();

                ////////report.Render();
                ////////StiWebViewer1.Report = report;
                ////////StiWebViewer1.ViewMode = StiWebViewMode.WholeReport;
                ////////report.Dispose();
                ////////PrinterSettings printerSettings = new PrinterSettings();
                ////////printerSettings.Copies = 1;
                //////////printerSettings.FromPage = 1;
                ////////printerSettings.ToPage = report.RenderedPages.Count;
                ////////// printerSettings.PrinterName = "Samsung SCX-3400 Series";
                ////////printerSettings.PrinterName = ConfigurationManager.AppSettings["CodeSoft"].ToString();
                ////////// printerSettings.PrinterName
                ////////report.Print(false, printerSettings);
                ////////StiWebViewer1.Report = report;
                ////////StiWebViewer1.ViewMode = StiWebViewMode.WholeReport;
                ////////report.Dispose();
            }
            else if (Report.Equals("Itemwise Report"))
            {
                if ((string.IsNullOrEmpty(fromDate.ToString())))
                {
                    lblStatus.Text = "Select From date";
                    return;
                }
                StiWebViewer1.Visible = true;
                string strReport = Request.QueryString.Get("rpt");
                StiReport report = new StiReport();
                DataSet read = new DataSet();
                if (cbocagtegory.Text != "---All---")
                {
                    // sql1 = "SELECT * from VW_salesreportone where category_id='" + cbocagtegory.SelectedValue + "' and orddate between '" + FromSalesdate + "' and '" + ToSalesDate + "' and deleted=0 and status='R'";
                    report.Load(appPath + "\\Reports\\SalesReportAll - Copy.mrt");
                    //report.Load("c:\\inetpub\\wwwroot\\RestaurantJey\\Reports\\SalesReportAll - Copy.mrt");

                    //report.Load("c:\\inetpub\\wwwroot\\RestaurantSS\\Reports\\SalesReportAll - Copy.mrt");
                    report.RegData(read);
                    report.Compile();
                    report.CompiledReport.DataSources["VW_salesreportone"].Parameters["@Ordfrmdate"].ParameterValue = FromSalesdate;
                    report.CompiledReport.DataSources["VW_salesreportone"].Parameters["@OrdTodate"].ParameterValue = ToSalesDate;
                    report.CompiledReport.DataSources["VW_salesreportone"].Parameters["@OrdTodate1"].ParameterValue = ToSalesDate1;
                    report.CompiledReport.DataSources["VW_salesreportone"].Parameters["@Ordcat"].ParameterValue = cbocagtegory.SelectedValue;
                }
                else
                {
                    // sql1 = "SELECT * from VW_salesreportone where deleted=0 and orddate between '" + FromSalesdate + "' and '" + ToSalesDate + "' and status='R'";
                    report.Load(appPath + "\\Reports\\SalesReportAll.mrt");
                    //report.Load("c:\\inetpub\\wwwroot\\RestaurantJey\\Reports\\SalesReportAll.mrt");

                    //report.Load("c:\\inetpub\\wwwroot\\RestaurantSS\\Reports\\SalesReportAll.mrt");
                    report.RegData(read);
                    report.Compile();
                    report.CompiledReport.DataSources["VW_salesreportone"].Parameters["@Ordfrmdate"].ParameterValue = FromSalesdate;
                    report.CompiledReport.DataSources["VW_salesreportone"].Parameters["@OrdTodate"].ParameterValue = ToSalesDate;
                    report.CompiledReport.DataSources["VW_salesreportone"].Parameters["@OrdTodate1"].ParameterValue = ToSalesDate1;
                    // report.CompiledReport.DataSources["VW_salesreportone"].Parameters["@Ordcat"].ParameterValue = cbocagtegory.SelectedValue;  
                }
                // report.Load("c:\\inetpub\\wwwroot\\RestaurantSS\\Reports\\ReceiptCashier.mrt");

                report.Render();
                PrinterSettings printerSettings = new PrinterSettings();
                printerSettings.Copies = 1;
                //printerSettings.FromPage = 1;
                printerSettings.ToPage = report.RenderedPages.Count;
                // printerSettings.PrinterName = "Samsung SCX-3400 Series";
                printerSettings.PrinterName = ConfigurationManager.AppSettings["CodeSoft"].ToString();
                // printerSettings.PrinterName
                report.Print(false, printerSettings);
                StiWebViewer1.Report = report;
                StiWebViewer1.ViewMode = StiWebViewMode.WholeReport;
                report.Dispose();

            }
            else if (Report.Equals("Summary Report"))
            {
                if ((string.IsNullOrEmpty(fromDate.ToString())))
                {
                    lblStatus.Text = "Select From date";
                    return;
                }
                StiWebViewer1.Visible = true;
                string strReport = Request.QueryString.Get("rpt");
                StiReport report = new StiReport();
                DataSet read = new DataSet();
                if (cbocagtegory.Text != "---All---")
                {
                    // sql1 = "SELECT * from VW_salesreportone where category_id='" + cbocagtegory.SelectedValue + "' and orddate between '" + FromSalesdate + "' and '" + ToSalesDate + "' and deleted=0 and status='R'";
                    report.Load(appPath + "\\Reports\\SummaryReport.mrt");
                    //report.Load("c:\\inetpub\\wwwroot\\RestaurantJey\\Reports\\SummaryReport.mrt");

                    //report.Load("c:\\inetpub\\wwwroot\\RestaurantSS\\Reports\\SummaryReport.mrt");
                    report.RegData(read);
                    report.Compile();
                    report.CompiledReport.DataSources["VW_Summary"].Parameters["@Ordfrmdate"].ParameterValue = FromSalesdate;
                    report.CompiledReport.DataSources["VW_Summary"].Parameters["@OrdTodate"].ParameterValue = ToSalesDate;
                    report.CompiledReport.DataSources["VW_Summary"].Parameters["@OrdTodate1"].ParameterValue = ToSalesDate1;
                    report.CompiledReport.DataSources["VW_Summary"].Parameters["@Ordcat"].ParameterValue = cbocagtegory.SelectedValue;
                }
                else
                {
                    // sql1 = "SELECT * from VW_salesreportone where deleted=0 and orddate between '" + FromSalesdate + "' and '" + ToSalesDate + "' and status='R'";

                    report.Load(appPath + "\\Reports\\SummaryReportitem.mrt");
                    //report.Load("c:\\inetpub\\wwwroot\\RestaurantJey\\Reports\\SummaryReportitem.mrt");

                    //report.Load("c:\\inetpub\\wwwroot\\RestaurantSS\\Reports\\SummaryReportitem.mrt");
                    report.RegData(read);
                    report.Compile();
                    report.CompiledReport.DataSources["VW_Summary"].Parameters["@Ordfrmdate"].ParameterValue = FromSalesdate;
                    report.CompiledReport.DataSources["VW_Summary"].Parameters["@OrdTodate"].ParameterValue = ToSalesDate;
                    report.CompiledReport.DataSources["VW_Summary"].Parameters["@OrdTodate1"].ParameterValue = ToSalesDate1;
                    // report.CompiledReport.DataSources["VW_salesreportone"].Parameters["@Ordcat"].ParameterValue = cbocagtegory.SelectedValue;  
                }
                // report.Load("c:\\inetpub\\wwwroot\\RestaurantSS\\Reports\\ReceiptCashier.mrt");

                report.Render();
                PrinterSettings printerSettings = new PrinterSettings();
                printerSettings.Copies = 1;
                //printerSettings.FromPage = 1;
                printerSettings.ToPage = report.RenderedPages.Count;
                // printerSettings.PrinterName = "Samsung SCX-3400 Series";
                printerSettings.PrinterName = ConfigurationManager.AppSettings["CodeSoft"].ToString();
                // printerSettings.PrinterName
                report.Print(false, printerSettings);
                StiWebViewer1.Report = report;
                StiWebViewer1.ViewMode = StiWebViewMode.WholeReport;
                report.Dispose();

            }
            //else if (Report.Equals("Sales Report DateTime"))//Sales Report DateTime
            //{
            //    if ((string.IsNullOrEmpty(fromDate.ToString())))
            //    {
            //        lblStatus.Text = "Select From date";
            //        return;
            //    }

            //    if (cbocagtegory.Text != "---All---")
            //    {
            //        sql1 = "SELECT * from VW_salesreportone where category_id='" + cbocagtegory.SelectedValue + "' and orddate between '" + FromSalesdate + "' and '" + ToSalesDate + "' and deleted=0 and status='R'";
            //    }
            //    else
            //    {
            //        sql1 = "SELECT * from VW_salesreportone where deleted=0 and orddate between '" + FromSalesdate + "' and '" + ToSalesDate + "' and status='R'";
            //    }
            //    Stimulsoft.Report.StiReport stiReport1;
            //    SqlDataAdapter ad1 = new SqlDataAdapter(sql1, con);
            //    DataSet ds1 = new DataSet();
            //    ds1.DataSetName = "DynamicDataSource1";
            //    ds1.Tables.Add("VW_salesreportone");
            //    ad1.Fill(ds1, "VW_salesreportone");
            //    stiReport1 = new StiReport();
            //    stiReport1.Dictionary.DataStore.Clear();
            //    stiReport1.ClearAllStates();
            //    stiReport1.Load("c:\\inetpub\\wwwroot\\Restaurantss\\Reports\\SalesReportAll.mrt");
            //    stiReport1.Dictionary.Databases.Clear();
            //    stiReport1.Dictionary.Databases.Add(new StiSqlDatabase("Connection", con));
            //    stiReport1.Dictionary.DataSources.Clear();
            //    stiReport1.RegData("VW_salesreportone", ds1);

            //    stiReport1.Dictionary.Synchronize();
            //    stiReport1.Compile();
            //    stiReport1.Render();
            //    StiWebViewer1.Report = stiReport1;
            //    StiWebViewer1.ViewMode = StiWebViewMode.WholeReport;
            //    stiReport1.Dispose();
            //}
            //---------------------------------------------------------------
            //else if (Report.Equals("Itemwise Report"))
            //{
            //    if ((string.IsNullOrEmpty(fromDate.ToString())))
            //    {
            //        lblStatus.Text = "Select From date";
            //        return;
            //    }
            //    if (cbocagtegory.Text != "---All---")
            //    {
            //        sql1 = "SELECT * from VW_salesreportone where category_id='" + cbocagtegory.SelectedValue + "' and orddate between '" + FromSalesdate + "' and '" + ToSalesDate + "' and deleted=0 and status='R'";
            //    }
            //    else
            //    {
            //        sql1 = "SELECT * from VW_salesreportone where deleted=0 and orddate between '" + FromSalesdate + "' and '" + ToSalesDate + "' and status='R'";
            //    }
            //    Stimulsoft.Report.StiReport stiReport1;
            //    SqlDataAdapter ad1 = new SqlDataAdapter(sql1, con);
            //    DataSet ds1 = new DataSet();
            //    ds1.DataSetName = "DynamicDataSource1";
            //    ds1.Tables.Add("VW_salesreportone");
            //    ad1.Fill(ds1, "VW_salesreportone");
            //    stiReport1 = new StiReport();
            //    stiReport1.Dictionary.DataStore.Clear();
            //    stiReport1.ClearAllStates();
            //    stiReport1.Load("c:\\inetpub\\wwwroot\\Restaurantss\\Reports\\ItemWiseSalesReport.mrt");
            //    stiReport1.Dictionary.Databases.Clear();
            //    stiReport1.Dictionary.Databases.Add(new StiSqlDatabase("Connection", con));
            //    stiReport1.Dictionary.DataSources.Clear();
            //    stiReport1.RegData("VW_salesreportone", ds1);

            //    stiReport1.Dictionary.Synchronize();
            //    stiReport1.Compile();
            //    stiReport1.Render();
            //    StiWebViewer1.Report = stiReport1;
            //    StiWebViewer1.ViewMode = StiWebViewMode.WholeReport;
            //    stiReport1.Dispose();
            //}
            //else if (Report.Equals("Summary Report"))
            //{
            //    if ((string.IsNullOrEmpty(fromDate.ToString())))
            //    {
            //        lblStatus.Text = "Select From date";
            //        return;
            //    }
            //    if (cbocagtegory.Text != "---All---")
            //    {
            //        sql1 = "SELECT * from VW_Summary where category_id='" + cbocagtegory.SelectedValue + "' and deleted=0 and orddate between '" + FromSalesdate + "' and '" + ToSalesDate + "' and status='R'";
            //    }
            //    else
            //    {
            //        sql1 = "SELECT * from VW_Summary where deleted=0 and orddate between '" + FromSalesdate + "' and '" + ToSalesDate + "' and status='R'";
            //    }
            //    Stimulsoft.Report.StiReport stiReport1;
            //    SqlDataAdapter ad1 = new SqlDataAdapter(sql1, con);
            //    DataSet ds1 = new DataSet();
            //    ds1.DataSetName = "DynamicDataSource1";
            //    ds1.Tables.Add("VW_Summary");
            //    ad1.Fill(ds1, "VW_Summary");
            //    stiReport1 = new StiReport();
            //    stiReport1.Dictionary.DataStore.Clear();
            //    stiReport1.ClearAllStates();
            //    stiReport1.Load("c:\\inetpub\\wwwroot\\Restaurantss\\Reports\\SummaryReport.mrt");
            //    stiReport1.Dictionary.Databases.Clear();
            //    stiReport1.Dictionary.Databases.Add(new StiSqlDatabase("Connection", con));
            //    stiReport1.Dictionary.DataSources.Clear();
            //    stiReport1.RegData("VW_Summary", ds1);

            //    stiReport1.Dictionary.Synchronize();
            //    stiReport1.Compile();
            //    stiReport1.Render();
            //    StiWebViewer1.Report = stiReport1;
            //    StiWebViewer1.ViewMode = StiWebViewMode.WholeReport;
            //    stiReport1.Dispose();
            //}
            else if (Report.Equals("Dining Charge Report")) //============================>TaxMaster
            {
                if ((string.IsNullOrEmpty(fromDate.ToString())))
                {
                    lblStatus.Text = "Select From date";
                    return;
                }


                sql1 = "Select OM.*,UM.* from dbo.orderMaster OM, dbo.users_tbl UM Where cast(UM.user_aid as varchar(10)) = OM.Cashier and OM.Status = 'R' and  OM.orddate between '" + FromSalesdate + "' and '" + ToSalesDate + "'";
                // sql1 = "select * from orderMaster where  deleted=0 and orddate between '" + FromSalesdate + "' and '" + ToSalesDate + "' and Status='R'";
                sql2 = "Select * from menuMaster";

                sql3 = "Select * from restaurantMaster";
                Stimulsoft.Report.StiReport stiReport1;

                SqlDataAdapter ad1 = new SqlDataAdapter(sql1, con);
                SqlDataAdapter ad2 = new SqlDataAdapter(sql2, con);
                SqlDataAdapter ad3 = new SqlDataAdapter(sql3, con);

                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet ds3 = new DataSet();

                ds1.DataSetName = "DynamicDataSource1";
                ds1.Tables.Add("orderMaster");
                ad1.Fill(ds1, "orderMaster");

                ds2.DataSetName = "DynamicDataSource2";
                ds2.Tables.Add("menuMaster");
                ad2.Fill(ds2, "menuMaster");

                ds3.DataSetName = "DynamicDataSource3";
                ds3.Tables.Add("restaurantMaster");
                ad3.Fill(ds3, "restaurantMaster");

                stiReport1 = new StiReport();
                stiReport1.Dictionary.DataStore.Clear();
                stiReport1.ClearAllStates();
                stiReport1.Load(appPath + "\\Reports\\TaxReport.mrt");
                //stiReport1.Load("c:\\inetpub\\wwwroot\\RestaurantJey\\Reports\\TaxReport.mrt");
                // stiReport1.Load("c:\\inetpub\\wwwroot\\Restaurantss\\Reports\\TaxReport.mrt");
                stiReport1.Dictionary.Databases.Clear();
                stiReport1.Dictionary.Databases.Add(new StiSqlDatabase("Connection", con));
                stiReport1.Dictionary.DataSources.Clear();

                stiReport1.RegData("orderMaster", ds1);
                stiReport1.RegData("menuMaster", ds2);
                stiReport1.RegData("restaurantMaster", ds3);

                stiReport1.Dictionary.Synchronize();
                stiReport1.Compile();
                stiReport1.Render();
                PrinterSettings printerSettings = new PrinterSettings();
                printerSettings.Copies = 1;
                //printerSettings.FromPage = 1;
                printerSettings.ToPage = stiReport1.RenderedPages.Count;
                // printerSettings.PrinterName = "Samsung SCX-3400 Series";
                printerSettings.PrinterName = ConfigurationManager.AppSettings["CodeSoft"].ToString();
                // printerSettings.PrinterName
                stiReport1.Print(false, printerSettings);
                StiWebViewer1.Report = stiReport1;
                StiWebViewer1.ViewMode = StiWebViewMode.WholeReport;
                stiReport1.Dispose();

            }
            //Gst report
            else if (Report.Equals("GST Report")) //============================>TaxMaster
            {
                if ((string.IsNullOrEmpty(fromDate.ToString())))
                {
                    lblStatus.Text = "Select From date";
                    return;
                }


                //sql1 = "Select OM.*,UM.* from dbo.orderMaster OM, dbo.users_tbl UM Where cast(UM.user_aid as varchar(10)) = OM.Cashier and OM.Status = 'R' and  OM.orddate between '" + FromSalesdate + "' and '" + ToSalesDate + "'";
                 sql2 = "select * from orderMaster where  deleted=0 and orddate between '" + FromSalesdate + "' and '" + ToSalesDate + "' and Status='R'";
                 //sql1 = "Select * from users_tbl";
                sql3 = "Select * from restaurantMaster";
                Stimulsoft.Report.StiReport stiReport1;

                SqlDataAdapter ad1 = new SqlDataAdapter(sql1, con);
                SqlDataAdapter ad2 = new SqlDataAdapter(sql2, con);
                SqlDataAdapter ad3 = new SqlDataAdapter(sql3, con);

                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet ds3 = new DataSet();

                //ds1.DataSetName = "DynamicDataSource1";
                //ds1.Tables.Add("users_tbl");
                //ad1.Fill(ds1, "users_tbl");

                ds2.DataSetName = "DynamicDataSource2";
                ds2.Tables.Add("orderMaster");
                ad2.Fill(ds2, "orderMaster");

                ds3.DataSetName = "DynamicDataSource3";
                ds3.Tables.Add("restaurantMaster");
                ad3.Fill(ds3, "restaurantMaster");

                stiReport1 = new StiReport();
                stiReport1.Dictionary.DataStore.Clear();
                stiReport1.ClearAllStates();
                stiReport1.Load(appPath + "\\Reports\\GSTReport.mrt");

                //stiReport1.Load("c:\\inetpub\\wwwroot\\RestaurantJey\\Reports\\GstReport.mrt");

                //stiReport1.Load("c:\\inetpub\\wwwroot\\Restaurantss\\Reports\\GstReport.mrt");
                stiReport1.Dictionary.Databases.Clear();
                stiReport1.Dictionary.Databases.Add(new StiSqlDatabase("Connection", con));
                stiReport1.Dictionary.DataSources.Clear();

                //stiReport1.RegData("users_tbl", ds1);
                stiReport1.RegData("orderMaster", ds2);
                stiReport1.RegData("restaurantMaster", ds3);

                stiReport1.Dictionary.Synchronize();
                stiReport1.Compile();
                stiReport1.Render();
                PrinterSettings printerSettings = new PrinterSettings();
                printerSettings.Copies = 1;
                //printerSettings.FromPage = 1;
                printerSettings.ToPage = stiReport1.RenderedPages.Count;
                // printerSettings.PrinterName = "Samsung SCX-3400 Series";
                printerSettings.PrinterName = ConfigurationManager.AppSettings["CodeSoft"].ToString();
                // printerSettings.PrinterName
                stiReport1.Print(false, printerSettings);
                StiWebViewer1.Report = stiReport1;
                StiWebViewer1.ViewMode = StiWebViewMode.WholeReport;
                stiReport1.Dispose();

            }
            else if (Report.Equals("PLU Report"))
            {
                if ((string.IsNullOrEmpty(fromDate.ToString())))
                {
                    lblStatus.Text = "Select From date";
                    return;
                }
                if (cbocagtegory.Text != "---All---")
                {
                    sql1 = "SELECT * from VW_PLU where category_id='" + cbocagtegory.SelectedValue + "' and deleted=0 and orddate between '" + FromSalesdate + "' and '" + ToSalesDate + "' and Status='R'";
                }
                else
                {
                    sql1 = "SELECT * from VW_PLU where deleted=0 and orddate between '" + FromSalesdate + "' and '" + ToSalesDate + "' and Status='R'";
                }
                Stimulsoft.Report.StiReport stiReport1;
                SqlDataAdapter ad1 = new SqlDataAdapter(sql1, con);
                DataSet ds1 = new DataSet();
                ds1.DataSetName = "DynamicDataSource1";
                ds1.Tables.Add("VW_PLU");
                ad1.Fill(ds1, "VW_PLU");
                stiReport1 = new StiReport();
                stiReport1.Dictionary.DataStore.Clear();
                stiReport1.ClearAllStates();
                stiReport1.Load(appPath + "\\Reports\\PLUNEW.mrt");
                //stiReport1.Load("c:\\inetpub\\wwwroot\\RestaurantJey\\Reports\\PLUNEW.mrt");

                //stiReport1.Load("c:\\inetpub\\wwwroot\\Restaurantss\\Reports\\PLUNEW.mrt");
                stiReport1.Dictionary.Databases.Clear();
                stiReport1.Dictionary.Databases.Add(new StiSqlDatabase("Connection", con));
                stiReport1.Dictionary.DataSources.Clear();
                stiReport1.RegData("VW_PLU", ds1);
                stiReport1.Dictionary.Synchronize();
                stiReport1.Compile();

                stiReport1.Render();
                PrinterSettings printerSettings = new PrinterSettings();
                printerSettings.Copies = 1;
                //printerSettings.FromPage = 1;
                printerSettings.ToPage = stiReport1.RenderedPages.Count;
                // printerSettings.PrinterName = "Samsung SCX-3400 Series";
                printerSettings.PrinterName = ConfigurationManager.AppSettings["CodeSoft"].ToString();
                // printerSettings.PrinterName
                stiReport1.Print(false, printerSettings);
                StiWebViewer1.Report = stiReport1;
                //StiWebViewer1.ViewMode = StiWebViewMode.WholeReport;
                stiReport1.Dispose();



                //report.Render();
                //PrinterSettings printerSettings = new PrinterSettings();
                //printerSettings.Copies = 1;
                ////printerSettings.FromPage = 1;
                //printerSettings.ToPage = report.RenderedPages.Count;
                ////  printerSettings.PrinterName = "Samsung SCX-3400 Series";
                //printerSettings.PrinterName = ConfigurationManager.AppSettings["CodeSoft"].ToString();
                //// printerSettings.PrinterName
                //report.Print(false, printerSettings);
                //StiWebViewer1.Report = report;
                //StiWebViewer1.ViewMode = StiWebViewMode.WholeReport; 
            }
            else if (Report.Equals("PLU Report"))
            {
                if ((string.IsNullOrEmpty(fromDate.ToString())))
                {
                    lblStatus.Text = "Select From date";
                    return;
                }
                if (cbocagtegory.Text != "---All---")
                {
                    sql1 = "SELECT * from VW_PLU where category_id='" + cbocagtegory.SelectedValue + "' and deleted=0 and orddate between '" + FromSalesdate + "' and '" + ToSalesDate + "' and Status='R'";
                }
                else
                {
                    sql1 = "SELECT * from VW_PLU where deleted=0 and orddate between '" + FromSalesdate + "' and '" + ToSalesDate + "' and Status='R'";
                }
                Stimulsoft.Report.StiReport stiReport1;
                SqlDataAdapter ad1 = new SqlDataAdapter(sql1, con);
                DataSet ds1 = new DataSet();
                ds1.DataSetName = "DynamicDataSource1";
                ds1.Tables.Add("VW_PLU");
                ad1.Fill(ds1, "VW_PLU");
                stiReport1 = new StiReport();
                stiReport1.Dictionary.DataStore.Clear();
                stiReport1.ClearAllStates();
                stiReport1.Load(appPath + "\\Reports\\PLUNEW.mrt");
                //stiReport1.Load("c:\\inetpub\\wwwroot\\RestaurantJey\\Reports\\PLUNEW.mrt");

                //stiReport1.Load("c:\\inetpub\\wwwroot\\Restaurantss\\Reports\\PLUNEW.mrt");
                stiReport1.Dictionary.Databases.Clear();
                stiReport1.Dictionary.Databases.Add(new StiSqlDatabase("Connection", con));
                stiReport1.Dictionary.DataSources.Clear();
                stiReport1.RegData("VW_PLU", ds1);
                stiReport1.Dictionary.Synchronize();
                stiReport1.Compile();

                stiReport1.Render();
                PrinterSettings printerSettings = new PrinterSettings();
                printerSettings.Copies = 1;
                //printerSettings.FromPage = 1;
                printerSettings.ToPage = stiReport1.RenderedPages.Count;
                // printerSettings.PrinterName = "Samsung SCX-3400 Series";
                printerSettings.PrinterName = ConfigurationManager.AppSettings["CodeSoft"].ToString();
                // printerSettings.PrinterName
                stiReport1.Print(false, printerSettings);
                StiWebViewer1.Report = stiReport1;
                //StiWebViewer1.ViewMode = StiWebViewMode.WholeReport;
                stiReport1.Dispose();



                //report.Render();
                //PrinterSettings printerSettings = new PrinterSettings();
                //printerSettings.Copies = 1;
                ////printerSettings.FromPage = 1;
                //printerSettings.ToPage = report.RenderedPages.Count;
                ////  printerSettings.PrinterName = "Samsung SCX-3400 Series";
                //printerSettings.PrinterName = ConfigurationManager.AppSettings["CodeSoft"].ToString();
                //// printerSettings.PrinterName
                //report.Print(false, printerSettings);
                //StiWebViewer1.Report = report;
                //StiWebViewer1.ViewMode = StiWebViewMode.WholeReport; 
            }
            else if (Report.Equals("VOID Report"))
            {
                if ((string.IsNullOrEmpty(fromDate.ToString())))
                {
                    lblStatus.Text = "Select From date";
                    return;
                }
                if ((string.IsNullOrEmpty(toDate.ToString())))
                {
                    lblStatus.Text = "Select To date";
                    return;
                }
                StiWebViewer1.Visible = true;
                string strReport = Request.QueryString.Get("rpt");
                StiReport report = new StiReport();
                DataSet read = new DataSet();
                report.Load(appPath + "\\Reports\\report.mrt");

                //report.Load("c:\\inetpub\\wwwroot\\RestaurantJey\\Reports\\report.mrt");

                //report.Load("c:\\inetpub\\wwwroot\\RestaurantSS\\Reports\\report.mrt");
                report.RegData(read);
                report.Compile();

                report.CompiledReport.DataSources["VW_VoidBillReport"].Parameters["@Ordfrmdate"].ParameterValue = FromSalesdate;
                report.CompiledReport.DataSources["VW_VoidBillReport"].Parameters["@OrdTodate"].ParameterValue = ToSalesDate;
                report.Render();
                PrinterSettings printerSettings = new PrinterSettings();
                printerSettings.Copies = 1;
                //printerSettings.FromPage = 1;
                printerSettings.ToPage = report.RenderedPages.Count;
                // printerSettings.PrinterName = "Samsung SCX-3400 Series";
                printerSettings.PrinterName = ConfigurationManager.AppSettings["CodeSoft"].ToString();
                // printerSettings.PrinterName
                report.Print(false, printerSettings);
                StiWebViewer1.Report = report;
                StiWebViewer1.ViewMode = StiWebViewMode.WholeReport;
                report.Dispose();
            }

        }
        catch (Exception ex)
        {
            lblStatus.Text = "" + ex.Message.ToString();
        }
    }

    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            lblMsg.Text = "";
            string sql1 = "";
            string sql2 = "";
            string sql3 = "";
            String Report = txtReport.Text.ToString();

            string appPath = Request.PhysicalApplicationPath;


            if (Report == "")
            {
                lblStatus.Text = "Please Select Report";
                return;
            }
            string con = @BusinessTier.getConnection1();
            DateTime fromDate = Convert.ToDateTime(txtFromDate.SelectedDate.ToString());
            if ((string.IsNullOrEmpty(fromDate.ToString())))
            {
                lblStatus.Text = "Select From date";
                return;
            }
            string FromSalesdate = fromDate.Month + "/" + fromDate.Day + "/" + fromDate.Year;

            DateTime toDate = Convert.ToDateTime(txtToDate.SelectedDate.ToString());
            if ((string.IsNullOrEmpty(toDate.ToString())))
            {
                lblStatus.Text = "Select To date";
                return;
            }

            string ToSalesDate = toDate.Month + "/" + toDate.Day + "/" + toDate.Year + " " + "23:59:59";
            string ToSalesDate1 = toDate.Month + "/" + toDate.Day + "/" + toDate.Year;

            if (Report.Equals("Today"))//===================>today
            {
                DateTime dtTooDate = System.DateTime.Now;
                string todaydate = dtTooDate.Month + "/" + dtTooDate.Day + "/" + dtTooDate.Year;
                string todaydate1 = dtTooDate.Month + "/" + dtTooDate.Day + "/" + dtTooDate.Year + " " + "23:59:59";
                if (cbocagtegory.Text != "---All---")
                {
                    sql1 = "SELECT * from VW_salesreportone where orddate between '" + todaydate + "' and'" + todaydate1 + "' and category_id='" + cbocagtegory.SelectedValue + "' and  status='R' and deleted=0 and detail_Deleted = 0";
                }
                else
                {
                    sql1 = "SELECT * from VW_salesreportone where orddate between '" + todaydate + "' and'" + todaydate1 + "'  and  status='R' and deleted=0 and detail_Deleted = 0";
                }
                // sql1 = "SELECT * from VW_salesreportone where  orddate between '2010-07-16' and'2010-07-16 23:59:59' and category_id=28 and status='R' and deleted=0";
                Stimulsoft.Report.StiReport stiReport1;
                SqlDataAdapter ad1 = new SqlDataAdapter(sql1, con);
                DataSet ds1 = new DataSet();
                ds1.DataSetName = "DynamicDataSource1";
                ds1.Tables.Add("VW_salesreportone");
                ad1.Fill(ds1, "VW_salesreportone");
                stiReport1 = new StiReport();
                stiReport1.Dictionary.DataStore.Clear();
                stiReport1.ClearAllStates();
                stiReport1.Load(appPath + "\\Reports\\today.mrt");
                //stiReport1.Load("c:\\inetpub\\wwwroot\\RestaurantJey\\Reports\\today.mrt");

                //stiReport1.Load("c:\\inetpub\\wwwroot\\Restaurantss\\Reports\\today.mrt");
                stiReport1.Dictionary.Databases.Clear();
                stiReport1.Dictionary.Databases.Add(new StiSqlDatabase("Connection", con));
                stiReport1.Dictionary.DataSources.Clear();
                stiReport1.RegData("VW_salesreportone", ds1);
                stiReport1.Dictionary.Synchronize();
                stiReport1.Compile();
                stiReport1.Render();
                StiWebViewer1.Report = stiReport1;
                StiWebViewer1.ViewMode = StiWebViewMode.WholeReport;
                stiReport1.Dispose();
            }
            //----------------------------------------------------------------
            else if (Report.Equals("Sales Report DateTime"))//Sales Report DateTime
            {
                if ((string.IsNullOrEmpty(fromDate.ToString())))
                {
                    lblStatus.Text = "Select From date";
                    return;
                }
                StiWebViewer1.Visible = true;
                string strReport = Request.QueryString.Get("rpt");
                StiReport report = new StiReport();
                DataSet read = new DataSet();
                if (cbocagtegory.Text != "---All---")
                {
                    // sql1 = "SELECT * from VW_salesreportone where category_id='" + cbocagtegory.SelectedValue + "' and orddate between '" + FromSalesdate + "' and '" + ToSalesDate + "' and deleted=0 and status='R'";

                    report.Load(appPath + "\\Reports\\SalesReportAll - Copy.mrt");

                    //report.Load("c:\\inetpub\\wwwroot\\RestaurantJey\\Reports\\SalesReportAll - Copy.mrt");

                    //report.Load("c:\\inetpub\\wwwroot\\RestaurantSS\\Reports\\SalesReportAll - Copy.mrt");
                    report.RegData(read);
                    report.Compile();
                    report.CompiledReport.DataSources["VW_salesreportone"].Parameters["@Ordfrmdate"].ParameterValue = FromSalesdate;
                    report.CompiledReport.DataSources["VW_salesreportone"].Parameters["@OrdTodate"].ParameterValue = ToSalesDate;
                    report.CompiledReport.DataSources["VW_salesreportone"].Parameters["@OrdTodate1"].ParameterValue = ToSalesDate1;
                    report.CompiledReport.DataSources["VW_salesreportone"].Parameters["@Ordcat"].ParameterValue = cbocagtegory.SelectedValue;
                }
                else
                {
                    // sql1 = "SELECT * from VW_salesreportone where deleted=0 and orddate between '" + FromSalesdate + "' and '" + ToSalesDate + "' and status='R'";

                    report.Load(appPath + "\\Reports\\SalesReportAll.mrt");
                    //report.Load("c:\\inetpub\\wwwroot\\RestaurantJey\\Reports\\SalesReportAll.mrt");

                    //report.Load("c:\\inetpub\\wwwroot\\RestaurantSS\\Reports\\SalesReportAll.mrt");
                    report.RegData(read);
                    report.Compile();
                    report.CompiledReport.DataSources["VW_salesreportone"].Parameters["@Ordfrmdate"].ParameterValue = FromSalesdate;
                    report.CompiledReport.DataSources["VW_salesreportone"].Parameters["@OrdTodate"].ParameterValue = ToSalesDate;
                    report.CompiledReport.DataSources["VW_salesreportone"].Parameters["@OrdTodate1"].ParameterValue = ToSalesDate1;
                    // report.CompiledReport.DataSources["VW_salesreportone"].Parameters["@Ordcat"].ParameterValue = cbocagtegory.SelectedValue;  
                }
                // report.Load("c:\\inetpub\\wwwroot\\RestaurantSS\\Reports\\ReceiptCashier.mrt");

                report.Render();

                StiWebViewer1.Report = report;
                StiWebViewer1.ViewMode = StiWebViewMode.WholeReport;
                report.Dispose();

            }
            else if (Report.Equals("Itemwise Report"))
            {
                if ((string.IsNullOrEmpty(fromDate.ToString())))
                {
                    lblStatus.Text = "Select From date";
                    return;
                }
                StiWebViewer1.Visible = true;
                string strReport = Request.QueryString.Get("rpt");
                StiReport report = new StiReport();
                DataSet read = new DataSet();
                if (cbocagtegory.Text != "---All---")
                {
                    // sql1 = "SELECT * from VW_salesreportone where category_id='" + cbocagtegory.SelectedValue + "' and orddate between '" + FromSalesdate + "' and '" + ToSalesDate + "' and deleted=0 and status='R'";
                    report.Load(appPath + "\\Reports\\SalesReportAll - Copy.mrt");

                    //report.Load("c:\\inetpub\\wwwroot\\RestaurantJey\\Reports\\SalesReportAll - Copy.mrt");

                    //report.Load("c:\\inetpub\\wwwroot\\RestaurantSS\\Reports\\SalesReportAll - Copy.mrt");
                    report.RegData(read);
                    report.Compile();
                    report.CompiledReport.DataSources["VW_salesreportone"].Parameters["@Ordfrmdate"].ParameterValue = FromSalesdate;
                    report.CompiledReport.DataSources["VW_salesreportone"].Parameters["@OrdTodate"].ParameterValue = ToSalesDate;
                    report.CompiledReport.DataSources["VW_salesreportone"].Parameters["@OrdTodate1"].ParameterValue = ToSalesDate1;
                    report.CompiledReport.DataSources["VW_salesreportone"].Parameters["@Ordcat"].ParameterValue = cbocagtegory.SelectedValue;
                }
                else
                {
                    // sql1 = "SELECT * from VW_salesreportone where deleted=0 and orddate between '" + FromSalesdate + "' and '" + ToSalesDate + "' and status='R'";
                    report.Load(appPath + "\\Reports\\SalesReportAll.mrt");

                    //report.Load("c:\\inetpub\\wwwroot\\RestaurantJey\\Reports\\SalesReportAll.mrt");

                    //report.Load("c:\\inetpub\\wwwroot\\RestaurantSS\\Reports\\SalesReportAll.mrt");
                    report.RegData(read);
                    report.Compile();
                    report.CompiledReport.DataSources["VW_salesreportone"].Parameters["@Ordfrmdate"].ParameterValue = FromSalesdate;
                    report.CompiledReport.DataSources["VW_salesreportone"].Parameters["@OrdTodate"].ParameterValue = ToSalesDate;
                    report.CompiledReport.DataSources["VW_salesreportone"].Parameters["@OrdTodate1"].ParameterValue = ToSalesDate1;
                    // report.CompiledReport.DataSources["VW_salesreportone"].Parameters["@Ordcat"].ParameterValue = cbocagtegory.SelectedValue;  
                }
                // report.Load("c:\\inetpub\\wwwroot\\RestaurantSS\\Reports\\ReceiptCashier.mrt");

                report.Render();
                StiWebViewer1.Report = report;
                StiWebViewer1.ViewMode = StiWebViewMode.WholeReport;
                report.Dispose();
            }
            else if (Report.Equals("Summary Report"))
            {
                if ((string.IsNullOrEmpty(fromDate.ToString())))
                {
                    lblStatus.Text = "Select From date";
                    return;
                }
                StiWebViewer1.Visible = true;
                string strReport = Request.QueryString.Get("rpt");
                StiReport report = new StiReport();
                DataSet read = new DataSet();
                if (cbocagtegory.Text != "---All---")
                {
                    // sql1 = "SELECT * from VW_salesreportone where category_id='" + cbocagtegory.SelectedValue + "' and orddate between '" + FromSalesdate + "' and '" + ToSalesDate + "' and deleted=0 and status='R'";
                    report.Load(appPath + "\\Reports\\SummaryReport.mrt");

                    //report.Load("c:\\inetpub\\wwwroot\\RestaurantJey\\Reports\\SummaryReport.mrt");

                    //report.Load("c:\\inetpub\\wwwroot\\RestaurantSS\\Reports\\SummaryReport.mrt");
                    report.RegData(read);
                    report.Compile();
                    report.CompiledReport.DataSources["VW_Summary"].Parameters["@Ordfrmdate"].ParameterValue = FromSalesdate;
                    report.CompiledReport.DataSources["VW_Summary"].Parameters["@OrdTodate"].ParameterValue = ToSalesDate;
                    report.CompiledReport.DataSources["VW_Summary"].Parameters["@OrdTodate1"].ParameterValue = ToSalesDate1;
                    report.CompiledReport.DataSources["VW_Summary"].Parameters["@Ordcat"].ParameterValue = cbocagtegory.SelectedValue;
                }
                else
                {
                    // sql1 = "SELECT * from VW_salesreportone where deleted=0 and orddate between '" + FromSalesdate + "' and '" + ToSalesDate + "' and status='R'";
                    report.Load(appPath + "\\Reports\\SummaryReportitem.mrt");

                    //report.Load("c:\\inetpub\\wwwroot\\RestaurantJey\\Reports\\SummaryReportitem.mrt");

                    //report.Load("c:\\inetpub\\wwwroot\\RestaurantSS\\Reports\\SummaryReportitem.mrt");
                    report.RegData(read);
                    report.Compile();
                    report.CompiledReport.DataSources["VW_Summary"].Parameters["@Ordfrmdate"].ParameterValue = FromSalesdate;
                    report.CompiledReport.DataSources["VW_Summary"].Parameters["@OrdTodate"].ParameterValue = ToSalesDate;
                    report.CompiledReport.DataSources["VW_Summary"].Parameters["@OrdTodate1"].ParameterValue = ToSalesDate1;
                    // report.CompiledReport.DataSources["VW_salesreportone"].Parameters["@Ordcat"].ParameterValue = cbocagtegory.SelectedValue;  
                }
                // report.Load("c:\\inetpub\\wwwroot\\RestaurantSS\\Reports\\ReceiptCashier.mrt");

                report.Render();
                StiWebViewer1.Report = report;
                StiWebViewer1.ViewMode = StiWebViewMode.WholeReport;
                report.Dispose();
            }

          
            //////////////////////////////////////////////////////////////////////////////////

            else if (Report.Equals("Dining Charge Report")) //============================>TaxMaster
            {
                if ((string.IsNullOrEmpty(fromDate.ToString())))
                {
                    lblStatus.Text = "Select From date";
                    return;
                }


                sql1 = "Select OM.*,UM.* from dbo.orderMaster OM, dbo.users_tbl UM Where cast(UM.user_aid as varchar(10)) = OM.Cashier and OM.Status = 'R' and  OM.orddate between '" + FromSalesdate + "' and '" + ToSalesDate + "'";
                // sql1 = "select * from orderMaster where  deleted=0 and orddate between '" + FromSalesdate + "' and '" + ToSalesDate + "' and Status='R'";
                sql2 = "Select * from menuMaster";

                sql3 = "Select * from restaurantMaster";
                Stimulsoft.Report.StiReport stiReport1;

                SqlDataAdapter ad1 = new SqlDataAdapter(sql1, con);
                SqlDataAdapter ad2 = new SqlDataAdapter(sql2, con);
                SqlDataAdapter ad3 = new SqlDataAdapter(sql3, con);

                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet ds3 = new DataSet();

                ds1.DataSetName = "DynamicDataSource1";
                ds1.Tables.Add("orderMaster");
                ad1.Fill(ds1, "orderMaster");

                ds2.DataSetName = "DynamicDataSource2";
                ds2.Tables.Add("menuMaster");
                ad2.Fill(ds2, "menuMaster");

                ds3.DataSetName = "DynamicDataSource3";
                ds3.Tables.Add("restaurantMaster");
                ad3.Fill(ds3, "restaurantMaster");

                stiReport1 = new StiReport();
                stiReport1.Dictionary.DataStore.Clear();
                stiReport1.ClearAllStates();
                stiReport1.Load(appPath + "\\Reports\\TaxReport.mrt");
                //stiReport1.Load("c:\\inetpub\\wwwroot\\RestaurantJey\\Reports\\TaxReport.mrt");
                // stiReport1.Load("c:\\inetpub\\wwwroot\\Restaurantss\\Reports\\TaxReport.mrt");
                stiReport1.Dictionary.Databases.Clear();
                stiReport1.Dictionary.Databases.Add(new StiSqlDatabase("Connection", con));
                stiReport1.Dictionary.DataSources.Clear();

                stiReport1.RegData("orderMaster", ds1);
                stiReport1.RegData("menuMaster", ds2);
                stiReport1.RegData("restaurantMaster", ds3);

                stiReport1.Dictionary.Synchronize();
                stiReport1.Compile();
                stiReport1.Render();

                //PrinterSettings printerSettings = new PrinterSettings();
                //printerSettings.Copies = 1;
                //printerSettings.ToPage = stiReport1.RenderedPages.Count;
                //printerSettings.PrinterName = ConfigurationManager.AppSettings["CodeSoft"].ToString();
                //stiReport1.Print(false, printerSettings);

                StiWebViewer1.Report = stiReport1;
                StiWebViewer1.ViewMode = StiWebViewMode.WholeReport;
                stiReport1.Dispose();

            }
            
            else if (Report.ToString().Trim() == "GST Report")
            //    Report
            //else if (Report.Equals("Gst Report")) //============================>TaxMaster
            {
                if ((string.IsNullOrEmpty(fromDate.ToString())))
                {
                    lblStatus.Text = "Select From date";
                    return;
                }


                //sql1 = "Select * from VW_OrderDetailTable where orddate between '" + FromSalesdate + "' and '" + ToSalesDate + "'";
                 sql1 = "select * from orderMaster where  deleted=0 and orddate between '" + FromSalesdate + "' and '" + ToSalesDate + "' and Status='R'";
               // sql1 = "Select * from orderMaster";
                sql3 = "Select * from restaurantMaster";
                Stimulsoft.Report.StiReport stiReport1;

                SqlDataAdapter ad1 = new SqlDataAdapter(sql1, con);
                //SqlDataAdapter ad2 = new SqlDataAdapter(sql2, con);
                SqlDataAdapter ad3 = new SqlDataAdapter(sql3, con);

                DataSet ds1 = new DataSet();
                //DataSet ds2 = new DataSet();
                DataSet ds3 = new DataSet();

                ds1.DataSetName = "DynamicDataSource1";
                ds1.Tables.Add("orderMaster");
                ad1.Fill(ds1, "orderMaster");

                //ds2.DataSetName = "DynamicDataSource2";
                //ds2.Tables.Add("orderMaster");
                //ad2.Fill(ds2, "orderMaster");

                ds3.DataSetName = "DynamicDataSource3";
                ds3.Tables.Add("restaurantMaster");
                ad3.Fill(ds3, "restaurantMaster");

                stiReport1 = new StiReport();
                stiReport1.Dictionary.DataStore.Clear();
                stiReport1.ClearAllStates();
                stiReport1.Load(appPath + "\\Reports\\GSTReport.mrt");
                //stiReport1.Load("c:\\inetpub\\wwwroot\\RestaurantJey\\Reports\\GstReport.mrt");

                //stiReport1.Load("c:\\inetpub\\wwwroot\\Restaurantss\\Reports\\GstReport.mrt");
                stiReport1.Dictionary.Databases.Clear();
                stiReport1.Dictionary.Databases.Add(new StiSqlDatabase("Connection", con));
                stiReport1.Dictionary.DataSources.Clear();

                stiReport1.RegData("VW_OrderDetailTable", ds1);
                //stiReport1.RegData("orderMaster", ds2);
                stiReport1.RegData("restaurantMaster", ds3);

                stiReport1.Dictionary.Synchronize();
                stiReport1.Compile();
                stiReport1.Render();

                //PrinterSettings printerSettings = new PrinterSettings();
                //printerSettings.Copies = 1;
                //printerSettings.ToPage = stiReport1.RenderedPages.Count;
                //printerSettings.PrinterName = ConfigurationManager.AppSettings["CodeSoft"].ToString();
                //stiReport1.Print(false, printerSettings);

                StiWebViewer1.Report = stiReport1;
                StiWebViewer1.ViewMode = StiWebViewMode.WholeReport;
                stiReport1.Dispose();

            }
            else if (Report.Equals("PLU Report"))
            {
                if ((string.IsNullOrEmpty(fromDate.ToString())))
                {
                    lblStatus.Text = "Select From date";
                    return;
                }
                if (cbocagtegory.Text != "---All---")
                {
                    sql1 = "SELECT * from VW_PLU where category_id='" + cbocagtegory.SelectedValue + "' and deleted=0 and detail_deleted=0 and orddate between '" + FromSalesdate + "' and '" + ToSalesDate + "' and Status='R'";
                }
                else
                {
                    sql1 = "SELECT * from VW_PLU where deleted=0 and detail_deleted=0 and orddate between '" + FromSalesdate + "' and '" + ToSalesDate + "' and Status='R'";
                }
                Stimulsoft.Report.StiReport stiReport1;
                SqlDataAdapter ad1 = new SqlDataAdapter(sql1, con);
                DataSet ds1 = new DataSet();
                ds1.DataSetName = "DynamicDataSource1";
                ds1.Tables.Add("VW_PLU");
                ad1.Fill(ds1, "VW_PLU");
                stiReport1 = new StiReport();
                stiReport1.Dictionary.DataStore.Clear();
                stiReport1.ClearAllStates();
                stiReport1.Load(appPath + "\\Reports\\PLUNEW.mrt");
                //stiReport1.Load("c:\\inetpub\\wwwroot\\RestaurantJey\\Reports\\PLUNEW.mrt");

                //stiReport1.Load("c:\\inetpub\\wwwroot\\Restaurantss\\Reports\\PLUNEW.mrt");
                stiReport1.Dictionary.Databases.Clear();
                stiReport1.Dictionary.Databases.Add(new StiSqlDatabase("Connection", con));
                stiReport1.Dictionary.DataSources.Clear();
                stiReport1.RegData("VW_PLU", ds1);
                stiReport1.Dictionary.Synchronize();
                stiReport1.Compile();
                stiReport1.Render();
                StiWebViewer1.Report = stiReport1;
                StiWebViewer1.ViewMode = StiWebViewMode.WholeReport;
                stiReport1.Dispose();
            }
            else if (Report.Equals("VOID Report"))
            {
                if ((string.IsNullOrEmpty(fromDate.ToString())))
                {
                    lblStatus.Text = "Select From date";
                    return;
                }
                if ((string.IsNullOrEmpty(toDate.ToString())))
                {
                    lblStatus.Text = "Select To date";
                    return;
                }
                StiWebViewer1.Visible = true;
                string strReport = Request.QueryString.Get("rpt");
                StiReport report = new StiReport();
                DataSet read = new DataSet();
                report.Load(appPath + "\\Reports\\VoidReport.mrt");

                //report.Load("c:\\inetpub\\wwwroot\\RestaurantJey\\Reports\\VoidReport.mrt");

                // report.Load("c:\\inetpub\\wwwroot\\RestaurantSS\\Reports\\VoidReport.mrt");
                report.RegData(read);
                report.Compile();

                report.CompiledReport.DataSources["VW_VoidBillReport"].Parameters["@Ordfrmdate"].ParameterValue = FromSalesdate;
                report.CompiledReport.DataSources["VW_VoidBillReport"].Parameters["@OrdTodate"].ParameterValue = ToSalesDate;

                report.Render();
                StiWebViewer1.Report = report;
                StiWebViewer1.ViewMode = StiWebViewMode.WholeReport;
                report.Dispose();


            }
        }
        catch (Exception ex)
        {
            lblStatus.Text = "" + ex.Message.ToString();
        }

    }
}