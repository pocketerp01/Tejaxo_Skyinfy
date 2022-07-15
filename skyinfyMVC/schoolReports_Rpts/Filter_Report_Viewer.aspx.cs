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


public partial class erp_schoolReports_Rpts_Filter_Report_Viewer2 : System.Web.UI.Page
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
    ReportDocument ReportDocument = new ReportDocument();
    public static string MyGuid = "";
    protected void Page_Init(object sender, EventArgs e)
    {
        MyGuid = EncryptDecrypt.Decrypt(HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["m_id"]);
        if (MyGuid == "") MyGuid = Request["hfmyguid"].ToString();
        sgen = new sgenFun(MyGuid);

        userCode = sgen.GetCookie(MyGuid, "userCode");
        hfmyguid.Value = MyGuid;
        if (Request.UrlReferrer == null) { Response.Redirect("~/erp/login_main.aspx"); return; }
        if (userCode.Equals("")) { Response.Redirect("~/erp/login_main.aspx"); return; }


        userid_mst = sgen.GetCookie(MyGuid, "userid_mst");
        clientid_mst = sgen.GetCookie(MyGuid, "clientid_mst");
        unitid_mst = sgen.GetCookie(MyGuid, "unitid_mst");
        role_mst = sgen.GetCookie(MyGuid, "role_mst");


        if (sgen.GetSession(MyGuid, "Data") != null)
        {
            //  System.Web.fo.ToolStrip oToolStrip = (System.Windows.Forms.ToolStrip)crystalReportViewer.Controls[4];
            CrystalReportViewer1.HasCrystalLogo = false;


            CrystalReportViewer1.ToolPanelView = ToolPanelViewType.None;
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            DataSet ds = new DataSet();
            DataTable dtmain = new DataTable();
            DataTable dtcompdt = new DataTable();
            DataTable dtbranchdt = new DataTable();
            DataTable Dt_summary = new DataTable();
            dt = (DataTable)sgen.GetSession(MyGuid, "Data");
            if (sgen.GetSession(MyGuid, "fee_summary") != null)
            {
                dt3 = (DataTable)sgen.GetSession(MyGuid, "fee_summary");

                dt.TableName = "dt_std_details";

                dt1 = sgen.GetCompDetail(userCode, clientid_mst);
                dt1.TableName = "Company_Detail";


                dt2 = sgen.GetBranchDetail(userCode, unitid_mst, clientid_mst);
                dt2.TableName = "Branch_Detail";

                dt3.TableName = "Dt_summary";
                Dt_summary = dt3.Copy();
                ds.Tables.Add(Dt_summary);
            }
            else
            {
                dt.TableName = "dt_std_details";
                dt1 = sgen.GetCompDetail(userCode, clientid_mst);
                dt1.TableName = "Company_Detail";
                dt2 = sgen.GetBranchDetail(userCode, unitid_mst, clientid_mst);
                dt2.TableName = "Branch_Detail";
            }

            dtmain = dt.Copy();
            dtcompdt = dt1.Copy();
            dtbranchdt = dt2.Copy();

            //dt3 = sgen.getdata(userCode, @"select school_fee_structure.Frequency_Id,school_frequency.FrequencyName, sum(school_fee_structure.totalmount) as totalmount   from school_fee_structure ,school_frequency  GROUP by school_fee_structure.Frequency_Id");



            string Rpt = (string)sgen.GetSession(MyGuid, "Report");
            //ReportDocument ReportDocument = new ReportDocument();
            ReportDocument.Load(Server.MapPath(Rpt));

            if (sgen.GetSession(MyGuid, "chq_Print") != null)
            {
                DataTable chq_print = (DataTable)sgen.GetSession(MyGuid, "chq_Print");
                FieldObject txtamtword = (FieldObject)ReportDocument.ReportDefinition.ReportObjects["Amtwords1"];


                DataTable dtfilter = sgen.seekval_dt(chq_print, "parameter='Amount In Word'");
                txtamtword.Top = sgen.Make_int(dtfilter.Rows[0]["topalign"].ToString());
                txtamtword.Left = sgen.Make_int(dtfilter.Rows[0]["leftalign"].ToString());


                FieldObject txtamt = (FieldObject)ReportDocument.ReportDefinition.ReportObjects["Amount1"];
                dtfilter = sgen.seekval_dt(chq_print, "parameter='Amount In Figure'");
                txtamt.Top = sgen.Make_int(dtfilter.Rows[0]["topalign"].ToString());
                txtamt.Left = sgen.Make_int(dtfilter.Rows[0]["leftalign"].ToString());



                FieldObject Chqdate1 = (FieldObject)ReportDocument.ReportDefinition.ReportObjects["Chqdate1"];
                dtfilter = sgen.seekval_dt(chq_print, "parameter='Cheque date'");
                Chqdate1.Top = sgen.Make_int(dtfilter.Rows[0]["topalign"].ToString());
                Chqdate1.Left = sgen.Make_int(dtfilter.Rows[0]["leftalign"].ToString());



                FieldObject accpay1 = (FieldObject)ReportDocument.ReportDefinition.ReportObjects["accpay1"];
                dtfilter = sgen.seekval_dt(chq_print, "parameter='Account Pay'");
                accpay1.Top = sgen.Make_int(dtfilter.Rows[0]["topalign"].ToString());
                accpay1.Left = sgen.Make_int(dtfilter.Rows[0]["leftalign"].ToString());

                FieldObject Infavourof1 = (FieldObject)ReportDocument.ReportDefinition.ReportObjects["Infavourof1"];
                dtfilter = sgen.seekval_dt(chq_print, "parameter='In Favour Of'");
                Infavourof1.Top = sgen.Make_int(dtfilter.Rows[0]["topalign"].ToString());
                Infavourof1.Left = sgen.Make_int(dtfilter.Rows[0]["leftalign"].ToString());

            }




            ds.Tables.Add(dtmain);
            ds.Tables.Add(dtcompdt);
            ds.Tables.Add(dtbranchdt);
            if (ds.Tables.Contains("table1")) ds.Tables.Remove("table1");


            // Report_Dataset Report_Dataset = new Report_Dataset();
            ReportDocument.SetDataSource(ds);

            // ReportDocument.Subreports["Fee_Summary"].SetDataSource(ds);
            CrystalReportViewer1.ReportSource = ReportDocument;
            CrystalReportViewer1.DataBind();
            CrystalReportViewer1.RefreshReport();



        }
    }

    private void chqprint()
    {

        //PictureObject lnObj;

        // 1440 twips = 2.5 cm whereas 576 = 1 cm.

        //txtobj.ObjectFormat.EnableSuppress=true;
        //Font fp = new Font(“Tahoma”, 10);
        //txtobj.ApplyFont(fp);
        //txtobj.Font(10);

        //After fliping LineStyle must be set:


        //PictureObject lnObj;

        //// 1440 twips = 2.5 cm whereas 576 = 1 cm.

        //int fr1cm = 576;

        //lnObj = (PictureObject)ReportDocument.ReportDefinition.ReportObjects["Picture1"];

        //int top;

        ////set the Top is Equal To The Bottom Size to avoid getting Error positioning line on Crystal

        //top = lnObj.Top;

        ////lnObj.Top = lnObj.Left

        //lnObj.Left = (int)((70 / 10.0) * fr1cm);

        //lnObj.Top = top;

        // After fliping LineStyle must be set:
        //if (Session["shivam"] != null)
        //{
        //    if (dt.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {


        //            //lnObj = (PictureObject)ReportDocument.ReportDefinition.ReportObjects["Text1"];

        //            //if (dt.Rows[0]["Name2"].ToString().Equals("school"))
        //            //{



        //            if (dt.Rows[i]["Col11"].ToString() == "F")

        //            { txtobj1.ObjectFormat.EnableSuppress = true; }
        //            else
        //            {
        //                Font font1 = new Font(dt.Rows[i]["Col10"].ToString(), sgen.Make_int(dt.Rows[i]["Col9"].ToString()), FontStyle.Bold);


        //                //int ttop = txtobj.Top;
        //                int topside1 = sgen.Make_int(dt.Rows[i]["Col7"].ToString());
        //                txtobj1.Top = (int)((topside1 / 10.0) * 270);
        //                //lnObj.Top = lnObj.Left
        //                int leftside1 = sgen.Make_int(dt.Rows[i]["Col8"].ToString());
        //                txtobj1.Left = (int)((leftside1 / 10.0) * 576);
        //                txtobj1.Height = (int)(Convert.ToInt16(dt.Rows[i]["Col9"]));
        //                txtobj1.ApplyFont(font1);
        //            }

        //txtobj2 = (TextObject)ReportDocument.ReportDefinition.ReportObjects["txtcontact"];
        //            if (dt.Rows[i]["Col29"].ToString() == "F")

        //            {
        //                txtobj2.ObjectFormat.EnableSuppress = true;
        //            }
        //            else {
        //                Font font2 = new Font(dt.Rows[i]["Col28"].ToString(), sgen.Make_int(dt.Rows[i]["Col27"].ToString()), FontStyle.Bold);

        //                //int ttop = txtobj.Top;
        //                int topside2 = sgen.Make_int(dt.Rows[i]["Col25"].ToString());
        //                txtobj2.Top = (int)((topside2 / 10.0) * 270);
        //                //lnObj.Top = lnObj.Left
        //                int leftside2 = sgen.Make_int(dt.Rows[i]["Col26"].ToString());
        //                txtobj2.Left = (int)((leftside2 / 10.0) * 576);
        //                txtobj2.Height = (int)(Convert.ToInt16(dt.Rows[i]["Col27"]));
        //                txtobj2.ApplyFont(font2);
        //            }

        //            txtobj3 = (TextObject)ReportDocument.ReportDefinition.ReportObjects["txtaddress"];
        //            if (dt.Rows[i]["Col17"].ToString() == "F")

        //            {
        //                txtobj3.ObjectFormat.EnableSuppress = true;
        //            }
        //            else
        //            {
        //                //int ttop = txtobj.Top;
        //                Font font3 = new Font(dt.Rows[i]["Col16"].ToString(), sgen.Make_int(dt.Rows[i]["Col15"].ToString()), FontStyle.Bold);
        //                int topside3 = sgen.Make_int(dt.Rows[i]["Col13"].ToString());
        //                txtobj3.Top = (int)((topside3 / 10.0) * 270);
        //                //lnObj.Top = lnObj.Left
        //                int leftside3 = sgen.Make_int(dt.Rows[i]["Col14"].ToString());
        //                txtobj3.Left = (int)((leftside3 / 10.0) * 576);
        //                txtobj3.Height = (int)(Convert.ToInt16(dt.Rows[i]["Col15"]));
        //                txtobj3.ApplyFont(font3);
        //            }
        //                //}
        //                //else if ("Name4".Equals("email")) {
        //                txtobj4 = (TextObject)ReportDocument.ReportDefinition.ReportObjects["txtemail"];

        //            if (dt.Rows[i]["Col23"].ToString() == "F")

        //            {
        //                txtobj4.ObjectFormat.EnableSuppress = true;
        //            }
        //            else
        //            {

        //                //int ttop = txtobj.Top;
        //                Font font4 = new Font(dt.Rows[i]["Col22"].ToString(), sgen.Make_int(dt.Rows[i]["Col21"].ToString()), FontStyle.Bold);
        //                int topside4 = sgen.Make_int(dt.Rows[i]["Col19"].ToString());
        //                txtobj4.Top = (int)((topside4 / 10.0) * 270);
        //                //lnObj.Top = lnObj.Left
        //                int leftside4 = sgen.Make_int(dt.Rows[i]["Col20"].ToString());
        //                txtobj4.Left = (int)((leftside4 / 10.0) * 576);
        //                txtobj4.Height = (int)(Convert.ToInt16(dt.Rows[i]["Col21"]));
        //                txtobj4.ApplyFont(font4);
        //            }
        //                txtobj5 = (TextObject)ReportDocument.ReportDefinition.ReportObjects["txtwebsite"];
        //            if (dt.Rows[i]["Col35"].ToString() == "F")

        //            {
        //                txtobj5.ObjectFormat.EnableSuppress = true;
        //            }
        //            else
        //            {
        //                //int ttop = txtobj.Top;
        //                Font font5 = new Font(dt.Rows[i]["Col34"].ToString(), sgen.Make_int(dt.Rows[i]["Col33"].ToString()), FontStyle.Bold);
        //                int topside5 = sgen.Make_int(dt.Rows[i]["Col31"].ToString());
        //                txtobj5.Top = (int)((topside5 / 10.0) * 270);
        //                //lnObj.Top = lnObj.Left
        //                int leftside5 = sgen.Make_int(dt.Rows[i]["Col32"].ToString());
        //                txtobj5.Left = (int)((leftside5 / 10.0) * 576);
        //                txtobj5.Height = (int)(Convert.ToInt16(dt.Rows[i]["Col33"]));
        //                txtobj5.ApplyFont(font5);
        //            }
        //                picobj = (PictureObject)ReportDocument.ReportDefinition.ReportObjects["logo"];
        //            if (dt.Rows[i]["Col41"].ToString() == "F")

        //            {
        //                picobj.ObjectFormat.EnableSuppress = true;
        //            }
        //            else
        //            {
        //                int topside6 = sgen.Make_int(dt.Rows[i]["Col37"].ToString());
        //                picobj.Top = (int)((topside6 / 10.0) * 270);
        //                //lnObj.Top = lnObj.Left
        //                int leftside6 = sgen.Make_int(dt.Rows[i]["Col38"].ToString());
        //                picobj.Left = (int)((leftside6 / 10.0) * 576);
        //                //}
        //            }
        //                //set the Top is Equal To The Bottom Size to avoid getting Error positioning line on Crystal


        //        }
        //    }
        //}

    }

    //void Page_Unload(object sender, EventArgs e)
    //{
    //    ReportDocument.Close();
    //    ReportDocument.Dispose();
    //    GC.Collect();
    //}
}