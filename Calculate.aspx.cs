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
public partial class Calculate : System.Web.UI.Page
{
    public virtual int SelectionLength { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        lblStatus.Text = "";
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        if (!(IsPostBack))
        {
            cmbTable.Text = Session["tableName"].ToString().Trim();
            string tableid = Session["tableid"].ToString().Trim();
            string selectdate = Session["selectDate"].ToString().Trim();
            billtot(tableid, selectdate);
            txtCash.Focus();
        }
    }
    ArrayList orderIDs;
    protected void billtot(string tableid, string selectdate)
    {
        int k = 0;
        //if (cmbTable.SelectedIndex >= 0)
        //{
        string orderNumbers = ""; double ordAmt = 0; double taxAmt = 0; double gstAmt = 0; double gst = 0; double disAmt = 0; double totAmt = 0;
        //string tableNo = cmbTable.SelectedItem.ToString();
        // string dtpckrselect = Convert.ToString(DatePickerselect.SelectedDate);
        orderIDs = BusinessTier.getPendingOrderIDsUsingUID(tableid, selectdate);
     
        for (int i = 0; i < orderIDs.Count; i++)
        {
            //DataSet orderItems = DataLayer.getOrderDetailsUsingOrderID(orderIDs[i].ToString());
            //for (int k = 0; k < orderItems.Tables["ORDERITEMS"].Rows.Count; k++)
            //{
            //string ordID = orderItems.Tables["ORDERITEMS"].Rows[k]["ID"].ToString();
          
            if (i == 0)
            {
                orderNumbers = orderIDs[i].ToString();
                DataSet ds = BusinessTier.getOrderRecieptDetails(orderIDs[i].ToString());
                Double Ded = BusinessTier.GetDeductionFromOrderMaster(orderIDs[i].ToString());
                ordAmt = Convert.ToDouble(ds.Tables["ORDER"].Rows[0]["OrdPrice"].ToString());
                taxAmt = Convert.ToDouble(ds.Tables["ORDER"].Rows[0]["Tax"].ToString());

                gstAmt = Convert.ToDouble(ds.Tables["ORDER"].Rows[0]["gst"].ToString());

                totAmt = Convert.ToDouble(ds.Tables["ORDER"].Rows[0]["TotalPrice".ToString()]);
                disAmt = Ded;
                k = 1;
            }
            else
            {
                orderNumbers = orderNumbers + " , " + orderIDs[i].ToString();
                DataSet ds = BusinessTier.getOrderRecieptDetails(orderIDs[i].ToString());
                Double Ded = BusinessTier.GetDeductionFromOrderMaster(orderIDs[i].ToString());
                ordAmt = ordAmt + Convert.ToDouble(ds.Tables["ORDER"].Rows[0]["OrdPrice"].ToString());
                taxAmt = taxAmt + Convert.ToDouble(ds.Tables["ORDER"].Rows[0]["Tax"].ToString());

                gstAmt = gstAmt+ Convert.ToDouble(ds.Tables["ORDER"].Rows[0]["gst"].ToString());

                totAmt = totAmt + Convert.ToDouble(ds.Tables["ORDER"].Rows[0]["TotalPrice".ToString()]);
                disAmt = disAmt + Ded;
                k = 2;
            }
            //}
        }
        txtOrdID.Text = orderNumbers.ToString();
        txtOrdAmt.Text = ordAmt.ToString();
        txtDis.Text = disAmt.ToString();
        txtTax.Text = taxAmt.ToString();
        
        txtNet.Text = totAmt.ToString();
        txtgst.Text = gstAmt.ToString();
        //FOR GST
        //gst = (ordAmt) / 100 * 6;
        //gst = Math.Round(gst / 5, 2) * 5;
        //txtgst.Text = Convert.ToString(gst);
        txtNet.Text = totAmt.ToString();


        if (k == 2)
        {
            double txttotnet = totAmt - disAmt;
            txtNet.Text = Convert.ToString(txttotnet);
        }

        //double txttotnet = totAmt - disAmt;
        //double txttotnet = totAmt;
        //txtNet.Text = Convert.ToString(txttotnet);
        //(totAmt + taxAmt - disAmt).ToString();
        // }
        
    }


    protected void btnRecieve_Click(object sender, EventArgs e)
    {
        updatemaster();
        //if (txtCash.Text.ToString() != "" && (Convert.ToDouble(txtCash.Text.ToString()) >= Convert.ToDouble(txtNet.Text.ToString())))
        //{
        //    string tableid = Session["tableid"].ToString().Trim();
        //    string selectdate = Session["selectDate"].ToString().Trim();
        //    //printRciept(Environment.CurrentDirectory + "\\RecieptBlank.rpt", cmbTable.SelectedItem.ToString());
        //    orderIDs = BusinessTier.getPendingOrderIDsUsingUID(tableid, selectdate);
        //    for (int i = 0; i < orderIDs.Count; i++)
        //    {
        //        string sqlMaster = "Update dbo.orderMaster Set Status = 'R', Cashier = '" + Session["sesUserID"].ToString() + "', Modified_Date = '" + DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss") + "' Where OrderID = " + orderIDs[i].ToString();
        //        BusinessTier.ExecuteInsertUpdateQry(sqlMaster);
        //       // Response.Redirect("cashier.aspx");
        //    }
        //}
        //else
        //{
        //    lblStatus.Text = "Please Enter the Cash Recieved...";

        //    txtCash.Focus();
        //}
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        report();
        updatemaster();
        lblStatus.Text = "<script type='text/javascript'>Close()</" + "script>";      
    
    }

    public void updatemaster()
    {
        try
        {
            if (txtCash.Text.ToString() != "" && (Convert.ToDouble(txtCash.Text.ToString()) >= Convert.ToDouble(txtNet.Text.ToString())))
            {
                string tableid = Session["tableid"].ToString().Trim();
                string selectdate = Session["selectDate"].ToString().Trim();
                //printRciept(Environment.CurrentDirectory + "\\RecieptBlank.rpt", cmbTable.SelectedItem.ToString());
                orderIDs = BusinessTier.getPendingOrderIDsUsingUID(tableid, selectdate);
                for (int i = 0; i < orderIDs.Count; i++)
                {
                    string sqlMaster = "Update dbo.orderMaster Set Status = 'R', Cashier = '" + Session["sesUserID"].ToString() + "', Modified_Date = '" + DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss") + "' Where OrderID = " + orderIDs[i].ToString();
                    BusinessTier.ExecuteInsertUpdateQry(sqlMaster);
                    // Response.Redirect("cashier.aspx");
                }
            }
            else
            {
                lblStatus.Text = "Please Enter the Cash Recieved...";

                txtCash.Focus();
            }
        }

        catch (Exception ex)
        {
            lblStatus.Text = "An error occured while trying to generate report, Please try again/Contact your administrator";
            // lblStatus.Text = "An error occured while trying to generate report.";
        }
    }

    public void report()
    {
        try
        {
           // DateTime RptDte = Convert.ToDateTime(Session["selectDate"].ToString().Trim());
           // string con = BusinessTier.getConnection1();
           // Stimulsoft.Report.StiReport stiReport1;
           // string strsqlOrderDetailTable = "Select * from VW_OrderDetailTable where deleted=0 and orderid='" + txtOrdID.Text + "' and  REPLACE(CONVERT(VARCHAR,OrdDate,106),' ','/') = '" + RptDte.ToString("dd/MMM/yyyy") + "' ";
           // string strsqlrestmaster = "select * from restaurantmaster where deleted=0 ";
           // SqlDataAdapter OrderDetail = new SqlDataAdapter(strsqlOrderDetailTable, con);
           // DataSet ds9 = new DataSet();
           // ds9.DataSetName = "DynamicDataSource9";
           // ds9.Tables.Add("VW_OrderDetailTable");
           // OrderDetail.Fill(ds9, "VW_OrderDetailTable");
           // SqlDataAdapter restmaster = new SqlDataAdapter(strsqlrestmaster, con);
           // DataSet ds = new DataSet();
           // ds.DataSetName = "DynamicDataSource";
           // ds.Tables.Add("restaurantMaster");
           // restmaster.Fill(ds, "restaurantMaster");
           // stiReport1 = new StiReport();
           // stiReport1.Dictionary.DataStore.Clear();
           // //stiReport1.Load(Environment.CurrentDirectory + "\\Receipt.rpt");
           //  stiReport1.Load("c:\\inetpub\\wwwroot\\Restaurant\\Reports\\Receipt.mrt");
           // stiReport1.Dictionary.Databases.Clear();
           // stiReport1.Dictionary.Databases.Add(new StiSqlDatabase("Connection", con));
           // stiReport1.Dictionary.DataSources.Clear();
           // stiReport1.RegData("VW_OrderDetailTable", ds9);
           // stiReport1.RegData("restaurantMaster", ds);
           // stiReport1.Dictionary.Synchronize();


            // stiReport1.CompiledReport.DataSources["VW_OrderDetailTable"].Parameters["@Cash"].ParameterValue = txtCash.Text.ToString();
            //stiReport1.Compile();
            //stiReport1["@Cash"] = txtCash.Text.ToString();
            //stiReport1.CompiledReport.DataSources["VW_OrderDetailTable"].Parameters["@Cash"].ParameterValue = txtCash.Text.ToString();
            //stiReport1.Render();
            //StiWebViewer1.Report = stiReport1;
            //StiWebViewer1.ViewMode = StiWebViewMode.WholeReport;

            StiWebViewer1.Visible = true;
            string strReport = Request.QueryString.Get("rpt");
            StiReport report = new StiReport();
            DataSet read = new DataSet();
            string appPath = Request.PhysicalApplicationPath;
           // report.Load(appPath + "\\Reports\\ReceiptCashier.mrt");
            report.Load("c:\\inetpub\\wwwroot\\RestaurantGST\\Reports\\ReceiptCashier.mrt");
           
                    
            //report.Load("c:\\inetpub\\wwwroot\\RestaurantJey\\Reports\\ReceiptCashier.mrt");

            //report.Load("c:\\inetpub\\wwwroot\\RestaurantSS\\Reports\\ReceiptCashier.mrt");
            report.RegData(read);
            report.Compile();          
            string strorid = txtOrdID.Text.ToString();
            string[] strnam = strorid.Split(',');
            int i = 0;
            string strnam1 = "";
            string strnam2 = "1";
            string strnam3 = "1";
            foreach (string word in strnam)
            {                
                i++;
            }
            if (i == 1)
            {
                strnam1 = strnam[0].ToString().Trim();
            }
            else if (i == 2)
            {
                strnam2 = strnam[1].ToString().Trim();
            }
            else if (i == 3)
            {
                strnam2 = strnam[1].ToString().Trim();
                strnam3 = strnam[2].ToString().Trim();
            }


            DateTime toDate = Convert.ToDateTime(Session["selectDate"].ToString().Trim());
       

           
            string ToSalesDate1 = toDate.Month + "/" + toDate.Day + "/" + toDate.Year;
            string ToSalesDate2 = toDate.Month + "/" + toDate.Day + "/" + toDate.Year + " " + "23:59:59";

            report.CompiledReport.DataSources["VW_OrderDetailTable"].Parameters["@OrderID"].ParameterValue = strnam[0].ToString();// txtOrdID.Text.ToString();
            report.CompiledReport.DataSources["VW_OrderDetailTable"].Parameters["@OrderID12"].ParameterValue = strnam2;
            report.CompiledReport.DataSources["VW_OrderDetailTable"].Parameters["@OrderID123"].ParameterValue = strnam3;// txtOrdID.Text.ToString();
            txtCash.Text = Convert.ToDouble(txtCash.Text.ToString()).ToString("0.00");
            txtNet.Text = Convert.ToDouble(txtNet.Text.ToString()).ToString("0.00");
            txtTax.Text = Convert.ToDouble(txtTax.Text.ToString()).ToString("0.00");
            txtgst.Text = Convert.ToDouble(txtgst.Text.ToString()).ToString("0.00");
            txtDis.Text = Convert.ToDouble(txtDis.Text.ToString()).ToString("0.00");
            report.CompiledReport.DataSources["VW_OrderDetailTable"].Parameters["@Cash"].ParameterValue = txtCash.Text.ToString();
            report.CompiledReport.DataSources["VW_OrderDetailTable"].Parameters["@Change"].ParameterValue = txtBal.Text.ToString();
            report.CompiledReport.DataSources["VW_OrderDetailTable"].Parameters["@ParamTax"].ParameterValue = txtTax.Text.ToString();
            //for GST
            report.CompiledReport.DataSources["VW_OrderDetailTable"].Parameters["@ParamGst"].ParameterValue = txtgst.Text.ToString();


            report.CompiledReport.DataSources["VW_OrderDetailTable"].Parameters["@ParamDed"].ParameterValue = txtDis.Text.ToString();
            report.CompiledReport.DataSources["VW_OrderDetailTable"].Parameters["@ParamNet"].ParameterValue = txtNet.Text.ToString();
            report.CompiledReport.DataSources["VW_OrderDetailTable"].Parameters["@ParamDate"].ParameterValue = ToSalesDate1;
            report.CompiledReport.DataSources["VW_OrderDetailTable"].Parameters["@ParamDate1"].ParameterValue = ToSalesDate2;
            report.Render();
            PrinterSettings printerSettings = new PrinterSettings();
            printerSettings.Copies = 1;
            //printerSettings.FromPage = 1;
            printerSettings.ToPage = report.RenderedPages.Count;
           //  printerSettings.PrinterName = "Samsung SCX-3400 Series";
           // printerSettings.PrinterName = "Canon iR C2880/C3380";
            //printerSettings.PrinterName = "Code Soft 31xx Series";
            printerSettings.PrinterName = ConfigurationManager.AppSettings["CodeSoft"].ToString();
           // report.Print(false, printerSettings);
            report.Print(false, printerSettings);
            StiWebViewer1.Report = report;
            StiWebViewer1.ViewMode = StiWebViewMode.WholeReport;           
        }

        catch (Exception ex)
        {
            lblStatus.Text = "An error occured while trying to generate report, Please try again/Contact your administrator";
            return;
        }

    }

    protected void txtCash_TextChanged(object sender, EventArgs e)
    {        
       
        calculate();
    }

    protected void calculate()
    {
        try
        {
            if (txtCash.Text.ToString() != "" && (Convert.ToDouble(txtCash.Text.ToString()) >= Convert.ToDouble(txtNet.Text.ToString())))
            {
                txtBal.Text = (Convert.ToDouble(txtCash.Text.ToString()) - Convert.ToDouble(txtNet.Text.ToString())).ToString("0.00");
                txtCash.Focus();
            }
            else
                txtBal.Text = "";
        }
        catch (Exception ex)
        {
            // MessageBox.Show(ex.Message + ",  Please try again...");
        }
    }

    protected void btnOne_Click(object sender, EventArgs e)
    {
        txtCash.Text = txtCash.Text + "1";
        //  calculate();
        //if ((txtCash.Text).Length==0)         
        //     txtCash.Text = txtCash.Text + "1";
        // else if((txtCash.Text).Length==1)
        //    txtCash.Text = txtCash.Text + "1";
        //else if ((txtCash.Text).Length > 1)
        //    txtCash.Text = "1";

        //if (txtCash.SelectionLength == 0)
        //    txtCash.Text = txtCash.Text + "1";
        //else if (txtCash.SelectionLength == 1)
        //    txtCash.Text = txtCash.Text + "1";
        //else if (txtCash.SelectionLength > 1)
        //    txtCash.Text = "1";
        calculate();
    }

    protected void btnTwo_Click(object sender, EventArgs e)
    {
        txtCash.Text = txtCash.Text + "2";

        //if ((txtCash.Text).Length == 0)  

        //    txtCash.Text = txtCash.Text + "2";
        //else if ((txtCash.Text).Length == 2)
        //txtCash.Text = txtCash.Text + "2";
        //else if ((txtCash.Text).Length > 1)
        //    txtCash.Text = "2";
        //if (txtCash.SelectionLength == 0)
        //    txtCash.Text = txtCash.Text + "2";
        //else if (txtCash.SelectionLength == 1)
        //    txtCash.Text = txtCash.Text + "2";
        //else if (txtCash.SelectionLength > 1)
        //    txtCash.SelectedText = "2";
        calculate();
    }

    protected void btnThree_Click(object sender, EventArgs e)
    {
        txtCash.Text = txtCash.Text + "3";
        //if (txtCash.SelectionLength == 0)
        //    txtCash.Text = txtCash.Text + "3";
        //else if (txtCash.SelectionLength == 1)
        //    txtCash.Text = txtCash.Text + "3";
        //else if (txtCash.SelectionLength > 1)
        //    txtCash.SelectedText = "3";
        calculate();
    }

    protected void btnFour_Click(object sender, EventArgs e)
    {

        //if (txtCash.SelectionLength == 0)
        txtCash.Text = txtCash.Text + "4";
        //else if (txtCash.SelectionLength == 1)
        //    txtCash.Text = txtCash.Text + "4";
        //else if (txtCash.SelectionLength > 0)
        //    txtCash.SelectedText = "4";
        calculate();
    }

    protected void btnFive_Click(object sender, EventArgs e)
    {
        //if (txtCash.SelectionLength == 0)
        txtCash.Text = txtCash.Text + "5";
        //else if (txtCash.SelectionLength == 1)
        //    txtCash.Text = txtCash.Text + "5";
        //else if (txtCash.SelectionLength > 0)
        //    txtCash.SelectedText = "5";
        calculate();
    }

    protected void btnSix_Click(object sender, EventArgs e)
    {
        //if (txtCash.SelectionLength == 0)
        txtCash.Text = txtCash.Text + "6";
        //if (txtCash.SelectionLength == 1)
        //    txtCash.Text = txtCash.Text + "6";
        //else if (txtCash.SelectionLength > 0)
        //    txtCash.SelectedText = "6";
        calculate();
    }

    protected void btnSeven_Click(object sender, EventArgs e)
    {
        //if (txtCash.SelectionLength == 0)
        txtCash.Text = txtCash.Text + "7";
        //else if (txtCash.SelectionLength == 1)
        //    txtCash.Text = txtCash.Text + "7";
        //else if (txtCash.SelectionLength > 0)
        //    txtCash.SelectedText = "7";
        calculate();
    }

    protected void btnEight_Click(object sender, EventArgs e)
    {
        //if (txtCash.SelectionLength == 0)
        txtCash.Text = txtCash.Text + "8";
        //else if (txtCash.SelectionLength == 1)
        //    txtCash.Text = txtCash.Text + "8";
        //else if (txtCash.SelectionLength > 0)
        //    txtCash.SelectedText = "8";
        calculate();
    }

    protected void btnNine_Click(object sender, EventArgs e)
    {
        //if (txtCash.SelectionLength == 0)
        txtCash.Text = txtCash.Text + "9";
        //else if (txtCash.SelectionLength == 1)
        //    txtCash.Text = txtCash.Text + "9";
        //else if (txtCash.SelectionLength > 0)
        //    txtCash.SelectedText = "9";
        calculate();
    }

    protected void btnZero_Click(object sender, EventArgs e)
    {
        //if (txtCash.SelectionLength == 0)
        txtCash.Text = txtCash.Text + "0";
        //else if (txtCash.SelectionLength == 1)
        //    txtCash.Text = txtCash.Text + "0";
        //else if (txtCash.SelectionLength > 0)
        //    txtCash.SelectedText = "0";
        calculate();
    }

    protected void btnDot_Click(object sender, EventArgs e)
    {
        txtCash.Text = txtCash.Text + ".";
        //if (txtCash.Text.ToString().IndexOf(".") < 0)
        //{
        //    if (txtCash.SelectionLength == 0)
        //        txtCash.Text = txtCash.Text + ".";
        //    if (txtCash.SelectionLength == 1)
        //        txtCash.Text = txtCash.Text + ".";
        //    else if (txtCash.SelectionLength > 0)
        //        txtCash.SelectedText = ".";
        //}
        calculate();
    }

    protected void rbtn_remove_Click(object sender, EventArgs e)

    {
      
        if (!(string.IsNullOrEmpty(txtCash.Text.ToString())))
      
        {
            txtCash.Text = txtCash.Text.Remove(txtCash.Text.Length - 1, 1);
            calculate();
            txtCash.Focus();
        }
    }

    protected void txt_remove_GotFocus(object sender, EventArgs e)
    {
        //GetTextbox = (Control)sender;
    }
   

}