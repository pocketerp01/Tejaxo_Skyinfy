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
using System.Drawing.Text;
using System.Drawing;


public partial class erp_schoolReports_Rpts_Filter_Report_XML : System.Web.UI.Page
{

    sgenFun sgen;
    string userCode;
    TextObject txtobj1;
    TextObject txtobj2;
    TextObject txtobj3;
    TextObject txtobj4;
    TextObject txtobj5;
    PictureObject picobj;
    string userid_mst = "", clientid_mst = "", unitid_mst = "", role_mst = "", cg_com_name = "";
    //CrystalDecisions.Web.CrystalImageHandler handler = new CrystalDecisions.Web.CrystalImageHandler();
    ReportDocument ReportDocument = new ReportDocument();
    public static string MyGuid = "";
    protected void Page_Init(object sender, EventArgs e)
    {


        MyGuid = EncryptDecrypt.Decrypt(HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["m_id"]);
        if (MyGuid == "") MyGuid = Request["hfmyguid"].ToString();
        sgen = new sgenFun(MyGuid);

        userCode = sgen.GetCookie(MyGuid, "userCode");
        hfmyguid.Value = MyGuid;
        if (Request.UrlReferrer == null) { Response.Redirect("~/login_main.aspx"); return; }
        if (userCode.Equals("")) { Response.Redirect("~/login_main.aspx"); return; }
        if (!IsPostBack)
        {
            userid_mst = sgen.GetCookie(MyGuid, "userid_mst");
            clientid_mst = sgen.GetCookie(MyGuid, "clientid_mst");
            unitid_mst = sgen.GetCookie(MyGuid, "unitid_mst");
            role_mst = sgen.GetCookie(MyGuid, "role_mst");
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            DataSet ds = new DataSet();
            if (sgen.GetSession(MyGuid, "DataDS") != null)
            {
                ds = (DataSet)sgen.GetSession(MyGuid, "DataDS");
                if (!ds.Tables[0].TableName.ToString().Trim().ToUpper().Equals("prepcur"))
                {
                    ds.Tables[0].TableName = "prepcur";
                }
            }
            else if (sgen.GetSession(MyGuid, "Data") != null)
            {
                //  System.Web.fo.ToolStrip oToolStrip = (System.Windows.Forms.ToolStrip)crystalReportViewer.Controls[4];                    
                dt = (DataTable)sgen.GetSession(MyGuid, "Data");

                dt.TableName = "prepcur";
                try
                {
                    ds.Tables.Add(dt);
                }
                catch (Exception ex) { }
            }


            if (ds.Tables["prepcur"] == null || ds.Tables["prepcur"].Rows.Count < 1) { div_nodata.Visible = true; return; }
            // Report_Dataset Report_Dataset = new Report_Dataset();

            if (ds.Tables.Count > 0)
            {
                if (!ds.Tables.Contains("Company_Detail"))
                {
                    dt1 = sgen.GetCompDetail(userCode, clientid_mst);
                    dt1.TableName = "Company_Detail";
                    dt2 = sgen.GetBranchDetail(userCode, unitid_mst, clientid_mst);
                    dt2.TableName = "Branch_Detail";
                    ds.Tables.Add(dt1);
                    ds.Tables.Add(dt2);
                }
                CrystalReportViewer1.HasCrystalLogo = false;
                CrystalReportViewer1.ToolPanelView = ToolPanelViewType.None;

                string Rpt = sgen.GetSession(MyGuid, "Report").ToString().Trim();
                if (Rpt.ToLower().EndsWith(".rpt")) Rpt = Rpt.Remove(Rpt.Length - 4, 4);
                string rptpath = Server.MapPath("../schoolReports_Rpts/" + Rpt + ".rpt");
                //ReportDocument ReportDocument = new ReportDocument();
                string xmlpath = Server.MapPath("../xmls/" + Rpt + ".xml");

                ds.WriteXml(xmlpath, XmlWriteMode.WriteSchema);
                ReportDocument.Load(rptpath);
                ReportDocument.SetDataSource(ds);

                //ReportDocument.Subreports["Fee_Summary"].SetDataSource(ds);
                //ReportDocument.PrintOptions.PrinterName = PrinterNam
                //ReportDocument.PrintToPrinter(1, false, 0, 0);
                CrystalReportViewer1.ReportSource = ReportDocument;
                CrystalReportViewer1.DataBind();

                sgen.SetSession(MyGuid, "ReportDocument", ReportDocument);
            }
        }
        else
        {
            ReportDocument doc = (ReportDocument)sgen.GetSession(MyGuid, "ReportDocument");
            //doc.PrintToPrinter(1, true, 0, 0);
            CrystalReportViewer1.ReportSource = doc;
            //CrystalReportViewer1.DataBind();
        }
    }


    //private void btnPrint_Click(object sender, EventArgs e)
    //{

    //}
    //protected void CrystalReportViewer1_Load(object sender, EventArgs e)
    //{

    //    System.Windows.Controls.Button button = GenericReportViewer.FindName("btnPrint") as
    //   System.Windows.Controls.Button;
    //    button.Click += MyMethod;
    //}

    //private void MyMethod(object sender, RoutedEventArgs e)
    //{

    //    //Your code here 

    //}
    //void Page_Unload(object sender, EventArgs e)
    //{
    //    ReportDocument.Close();
    //    ReportDocument.Dispose();
    //    GC.Collect();
    //}
}