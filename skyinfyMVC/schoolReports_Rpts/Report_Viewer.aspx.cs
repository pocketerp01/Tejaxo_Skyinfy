using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;

public partial class erp_Reports_Report_Viewer : System.Web.UI.Page
{
    sgenFun sgen = new sgenFun();
    string userCode;
    string userid_mst = "", clientid_mst = "", unitid_mst = "", role_mst = "", cg_com_name = "";
    
    protected void Page_Load(object sender, EventArgs e)
    {
        userCode = sgen.getcookie("userCode");

        if (Request.UrlReferrer == null)
        {
            Response.Redirect("~/erp/login_main.aspx");
            return;
        }

        if (userCode.Equals(""))
        {
            Response.Redirect("~/erp/login_main.aspx");
            return;
        
        }


        userid_mst = sgen.getcookie("userid_mst");
        clientid_mst = sgen.getcookie("clientid_mst");
        unitid_mst = sgen.getcookie("unitid_mst");
        role_mst = sgen.getcookie("role_mst");
      

        if (Session["Data"] != null)
        {
            //  System.Web.fo.ToolStrip oToolStrip = (System.Windows.Forms.ToolStrip)crystalReportViewer.Controls[4];
                CrystalReportViewer1.HasCrystalLogo = false;
            CrystalReportViewer1.ToolPanelView = ToolPanelViewType.None;
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            dt = (DataTable)Session["RptData"];
            dt.TableName = "Prepcur";
            dt1 = sgen.GetCompDetail(userCode, clientid_mst);
            dt1.TableName = "CP_detail";
            dt2 = sgen.GetBranchDetail(userCode, unitid_mst, clientid_mst);
            dt2.TableName = "BR_Detail";
            
            //dt3 = sgen.getdata(userCode, @"select school_fee_structure.Frequency_Id,school_frequency.FrequencyName, sum(school_fee_structure.totalmount) as totalmount   from school_fee_structure ,school_frequency  GROUP by school_fee_structure.Frequency_Id");
       
            string Rpt = (string)Session["RptReport"];
            ReportDocument ReportDocument = new ReportDocument();

            ReportDocument.Load(Server.MapPath(Rpt));
            DataSet ds = new DataSet();

            //DataTable dtmain = new DataTable();
            //DataTable dtcompdt = new DataTable();
            //DataTable dtbranchdt = new DataTable();
            //DataTable Dt_summary = new DataTable();
         
            //dtmain = dt.Copy();
            //dtcompdt = dt1.Copy();
            //dtbranchdt = dt2.Copy();
            //Dt_summary = dt3.Copy();
            //ds.Tables.Add(dtmain);
            //ds.Tables.Add(dtcompdt);
            //ds.Tables.Add(dtbranchdt);
            //ds.Tables.Add(Dt_summary);

          
            // Report_Dataset Report_Dataset = new Report_Dataset();
            ReportDocument.SetDataSource(ds);

           // ReportDocument.Subreports["Fee_Summary"].SetDataSource(ds);
          
          
            CrystalReportViewer1.ReportSource = ReportDocument;
            CrystalReportViewer1.DataBind();

        }
    }

 
}