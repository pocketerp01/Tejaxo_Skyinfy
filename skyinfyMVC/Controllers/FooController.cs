using iTextSharp.text;
using iTextSharp.text.pdf;
using skyinfyMVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.text.html.simpleparser;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Text;

namespace skyinfyMVC.Controllers
{
    public class FooController : Controller
    {
        public static sgenFun sgen;
        public static string userCode;

        // GET: Foo
        static string basequey = "", mq = "", mq0 = "", mq1 = "", cond = "", query = "",
            query1 = "", query2 = "", query3 = "", cols = "", colswhere = "", colsorder = "",
            orderby = "", filename = "", menuid = "", year_to = "", year_from = "";
        static int iPageNo = 0, iPageRecords = 0, seektype = 0, CHECKTYPE = 0;
        public static string MyGuid = "";

        public void FillMst(string Myguid = "")
        {
            MyGuid = Myguid;
            if (MyGuid == "") MyGuid = EncryptDecrypt.Decrypt(HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["m_id"]);
            if (menuid == "") menuid = EncryptDecrypt.Decrypt(HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["mid"]);
            sgen = new sgenFun(MyGuid);
            if (MyGuid == "") { MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]); }
            if (MyGuid == "") MyGuid = sgen.GetCookie("", "MyGuid");
            sgen = new sgenFun(MyGuid);
            userCode = sgen.GetCookie(MyGuid, "userCode");
        }
        #region foov2

        [HttpGet]
        public ActionResult Footable_v2(string Myguid)
        {
            FillMst(Myguid);
            TableResultModel model = new TableResultModel();
            try
            {
                model.error = "N";
                userCode = sgen.GetCookie(MyGuid, "userCode");
                //Session["filename"] = "All Menus";
                //Session["basedtquery"] = "select * from menus";
                //Session["SEEKLIMIT"] = 9999999999;
                //Session["SHOWSAVE"] = false;
                //Session["TEMPID"] = "-";
                //Session["SEEKTYPE"] = 0;
                //else Session["SEEKTYPE"] = 2;
                //Session["CHECKTYPE"] = 0;
                sgen.SetSession(MyGuid, "ALLVALUES", "");
                userCode = sgen.GetCookie(MyGuid, "userCode");
                model.Myguid = MyGuid;
                model.txtsearch = (string)sgen.GetSession(MyGuid, "SRCHVAL");
                sgen.SetSession(MyGuid, "SRCHVAL", "");
                //sgen.execute_cmd(userCode, "create or replace procedure SP_FOO (   mq1 in varchar,  mq2 in varchar,  cursor1 out SYS_REFCURSOR,  cursor2 out SYS_REFCURSOR ) is begin open cursor1 FOR mq1;  open cursor2 FOR mq2; end SP_FOO; ");
                try
                {

                    iPageRecords = Convert.ToInt32(model.iPageRecords);
                    iPageRecords = Convert.ToInt32(sgen.seekval(userCode, "select param1 from controls where type='CTL' and id='000002' and trim(param5)='common' ", "param1"));
                }
                catch (Exception) { iPageRecords = 10; }
                sgen.SetSession(MyGuid, "iPageRecords", iPageRecords);
                seektype = Convert.ToInt16(sgen.GetSession(MyGuid, "SEEKTYPE").ToString());
                CHECKTYPE = Convert.ToInt16(sgen.GetSession(MyGuid, "CHECKTYPE").ToString());
                model.divtempl = false;
                if (sgen.GetSession(MyGuid, "SHOWSAVE") != null)
                {
                    bool showsave = Boolean.Parse(sgen.GetSession(MyGuid, "SHOWSAVE").ToString());
                    if (showsave) { model.divtempl = true; }
                }
                basequey = (string)sgen.GetSession(MyGuid, "basedtquery");
                DataTable dtsession = (DataTable)sgen.GetSession(MyGuid, "basedatatable");
                sgen.SetSession(MyGuid, "ALLDATADT", null);
                sgen.SetSession(MyGuid, "searchVal", null);
                sgen.SetSession(MyGuid, "filename", "File");
                sgen.SetSession(MyGuid, "basedtcolswhere", null);
                if (!basequey.Trim().Equals(""))
                {
                    sgen.SetSession(MyGuid, "basedatatable", null);
                    BindOperators(model);
                }
                else if (dtsession != null)
                {
                    ViewBag.basequey = "";
                    model = BindOperators(model);
                }
            }
            catch (Exception err)
            {
                model.error = "Y";
                var LineNumber = new StackTrace(err, true).GetFrame(0).GetFileLineNumber();
                ViewBag.scripCall += "showmsgJS(1, '" + err.Message.Replace("'", "") + " At Line number " + LineNumber + "', 1);";
            }

            return PartialView("footable_v2", model);
        }

        [HttpPost]
        public ActionResult Footable_v2(TableResultModel model, string command, string rows, string sortc)
        {
            MyGuid = model.Myguid;
            ViewBag.basequey = "-";
            command = command.ToLower().Trim();
            if (command.Equals("word"))
            {
                return expword(model);

            }
            else if (command.Equals("selected"))
            {
                GetValue(rows);
            }
            else if (command.Equals("search"))
            {

                if (model.showtot != null)
                {
                    if (model.showtot.Trim() == "Y")
                    {
                    Multiton.SetSession(MyGuid, "SHOWTOT", "Y");
                    }
                }
                userCode = sgen.GetCookie(MyGuid, "userCode");
                try { iPageRecords = Convert.ToInt32(model.iPageRecords); }
                catch (Exception) { iPageRecords = 10; }
                sgen.SetSession(MyGuid, "iPageRecords", iPageRecords);
                seektype = Convert.ToInt16(sgen.GetSession(MyGuid, "SEEKTYPE").ToString());
                CHECKTYPE = Convert.ToInt16(sgen.GetSession(MyGuid, "CHECKTYPE").ToString());
                model.divtempl = false;
                if (sgen.GetSession(MyGuid, "SHOWSAVE") != null)
                {
                    bool showsave = Boolean.Parse(sgen.GetSession(MyGuid, "SHOWSAVE").ToString());
                    if (showsave) { model.divtempl = true; }
                }
                sgen.SetSession(MyGuid, "ALLVALUES", model.selectedrows);
                //sgen.SetSession(MyGuid, "ALLDATADT", null);
                //sgen.SetSession(MyGuid, "searchVal", null);
                sgen.SetSession(MyGuid, "filename", "File");
                model = BindOperators(model);

            }
            return PartialView("footable_v2", model);
        }

        #endregion
        [HttpGet]
        public ActionResult Footable_v3(string Myguid)
        {
            FillMst(Myguid);
            TableResultModel model = new TableResultModel();
            try
            {
                model.error = "N";
                userCode = sgen.GetCookie(MyGuid, "userCode");
                //Session["filename"] = "All Menus";
                //Session["basedtquery"] = "select * from menus";
                //Session["SEEKLIMIT"] = 9999999999;
                //Session["SHOWSAVE"] = false;
                //Session["TEMPID"] = "-";
                //Session["SEEKTYPE"] = 0;
                //else Session["SEEKTYPE"] = 2;
                //Session["CHECKTYPE"] = 0;
                sgen.SetSession(MyGuid, "ALLVALUES", "");
                userCode = sgen.GetCookie(MyGuid, "userCode");
                model.Myguid = MyGuid;
                model.txtsearch = (string)sgen.GetSession(MyGuid, "SRCHVAL");
                sgen.SetSession(MyGuid, "SRCHVAL", "");
                //sgen.execute_cmd(userCode, "create or replace procedure SP_FOO (   mq1 in varchar,  mq2 in varchar,  cursor1 out SYS_REFCURSOR,  cursor2 out SYS_REFCURSOR ) is begin open cursor1 FOR mq1;  open cursor2 FOR mq2; end SP_FOO; ");
                try
                {

                    iPageRecords = Convert.ToInt32(model.iPageRecords);
                    iPageRecords = Convert.ToInt32(sgen.seekval(userCode, "select param1 from controls where type='CTL' and id='000002' and trim(param5)='common' ", "param1"));
                }
                catch (Exception) { iPageRecords = 10; }
                sgen.SetSession(MyGuid, "iPageRecords", iPageRecords);
                seektype = Convert.ToInt16(sgen.GetSession(MyGuid, "SEEKTYPE").ToString());
                CHECKTYPE = Convert.ToInt16(sgen.GetSession(MyGuid, "CHECKTYPE").ToString());
                model.divtempl = false;
                if (sgen.GetSession(MyGuid, "SHOWSAVE") != null)
                {
                    bool showsave = Boolean.Parse(sgen.GetSession(MyGuid, "SHOWSAVE").ToString());
                    if (showsave) { model.divtempl = true; }
                }
                basequey = (string)sgen.GetSession(MyGuid, "basedtquery");
                DataTable dtsession = (DataTable)sgen.GetSession(MyGuid, "basedatatable");
                sgen.SetSession(MyGuid, "ALLDATADT", null);
                sgen.SetSession(MyGuid, "searchVal", null);
                sgen.SetSession(MyGuid, "filename", "File");
                sgen.SetSession(MyGuid, "basedtcolswhere", null);
                if (!basequey.Trim().Equals(""))
                {
                    sgen.SetSession(MyGuid, "basedatatable", null);
                    BindOperators(model);
                }
                else if (dtsession != null)
                {
                    ViewBag.basequey = "";
                    model = BindOperators(model);
                }
            }
            catch (Exception err)
            {
                model.error = "Y";
                var LineNumber = new StackTrace(err, true).GetFrame(0).GetFileLineNumber();
                ViewBag.scripCall += "showmsgJS(1, '" + err.Message.Replace("'", "") + " At Line number " + LineNumber + "', 1);";
            }

            return PartialView("footable_v3", model);
        }

        [HttpPost]
        public ActionResult Footable_v3(TableResultModel model, string command, string rows, string sortc)
        {
            MyGuid = model.Myguid;
            ViewBag.basequey = "-";
            command = command.ToLower().Trim();
            if (command.Equals("word"))
            {
                return expword(model);

            }
            else if (command.Equals("selected"))
            {
                GetValue(rows);
            }
            else if (command.Equals("search"))
            {
                if (model.showtot != null)
                {
                    if (model.showtot.Trim() == "Y")
                    {
                        Multiton.SetSession(MyGuid, "SHOWTOT", "Y");
                    }
                }
                userCode = sgen.GetCookie(MyGuid, "userCode");
                try { iPageRecords = Convert.ToInt32(model.iPageRecords); }
                catch (Exception) { iPageRecords = 10; }
                sgen.SetSession(MyGuid, "iPageRecords", iPageRecords);
                seektype = Convert.ToInt16(sgen.GetSession(MyGuid, "SEEKTYPE").ToString());
                CHECKTYPE = Convert.ToInt16(sgen.GetSession(MyGuid, "CHECKTYPE").ToString());
                model.divtempl = false;
                if (sgen.GetSession(MyGuid, "SHOWSAVE") != null)
                {
                    bool showsave = Boolean.Parse(sgen.GetSession(MyGuid, "SHOWSAVE").ToString());
                    if (showsave) { model.divtempl = true; }
                }
                sgen.SetSession(MyGuid, "ALLVALUES", model.selectedrows);
                //sgen.SetSession(MyGuid, "ALLDATADT", null);
                //sgen.SetSession(MyGuid, "searchVal", null);
                sgen.SetSession(MyGuid, "filename", "File");
                model = BindOperators(model);

            }
            return PartialView("footable_v3", model);
        }

        [HttpPost]
        private ContentResult GetValue(string selectedvalues)
        {

            DataTable dtold = new DataTable();
            int olds = 0;
            DataTable alldata = new DataTable();
            string values = "", allkeys = "";
            try
            {
                if (selectedvalues != null)
                {
                    alldata = (DataTable)sgen.GetSession(MyGuid, "ALLDATADT");
                    allkeys = selectedvalues;
                    int limit = sgen.Make_int(sgen.GetSession(MyGuid, "SEEKLIMIT").ToString());
                    if (limit == 0) limit = 99999;


                    if (!allkeys.Trim().Equals(""))
                    {
                        string[] keys = allkeys.Split(',');
                        foreach (var str in keys)
                        {
                            olds++;
                            if (values.Trim().Equals("")) values = alldata.Rows[Convert.ToInt32(str)][2].ToString().Trim();
                            else values = values + "','" + alldata.Rows[Convert.ToInt32(str)][2].ToString().Trim();
                        }
                        sgen.SetSession(MyGuid, "SSEEKVAL", values);
                    }
                }
            }
            catch (Exception err)
            {
                sgen.SetSession(MyGuid, "SSEEKVAL", "");
            }
            sgen.SetSession(MyGuid, "parentname", "");
            ViewBag.ScripCall = "close_pop();";
            return Content("");
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "SessionAlert", "close_pop();", true);
        }

        [HttpPost]
        public ContentResult delsession(string sname, string Myguid)
        {
            try
            {
                sgen = new sgenFun(Myguid);
                sgen.SetSession(Myguid, sname, "");
                sgen.SetSession(Myguid, "basedtquery", "");
                sgen.SetSession(Myguid, "basedatatable", null);

            }
            catch (Exception ex) { }
            return Content("");
        }

        private TableResultModel BindOperators(TableResultModel model)
        {

            DataTable dtops = new DataTable();
            dtops.Columns.Add("Name", typeof(string));
            DataRow dr = dtops.NewRow();
            dr[0] = "Equals To";
            dtops.Rows.Add(dr);
            dr = dtops.NewRow();
            dr[0] = "Smaller Than";
            dtops.Rows.Add(dr);
            dr = dtops.NewRow();
            dr[0] = "Greater Than";
            dtops.Rows.Add(dr);
            dr = dtops.NewRow();
            dr[0] = "Contains";
            dtops.Rows.Add(dr);
            if (model.filters.Count < 1)
            {
                model.filters = new List<Filters>();
                model.filters.Add(new Filters { ddoperators = sgen.dt_to_selectlist(dtops) });
            }
            else
            {
                for (int k = 0; k < model.filters.Count; k++)
                {

                    model.filters[k].ddoperators = sgen.dt_to_selectlist(dtops);
                }
            }
            sgen.SetSession(MyGuid, "DDOPERATORS", sgen.dt_to_selectlist(dtops));
            ///
            // shiv
            //DataTable dtpages = sgen.getdata(userCode, "select '10' as name from dual union select '20' as name from dual union select '25' as name from dual union " +
            //    "select '50' as name from dual union select '75' as name  from dual union select '100' as name from dual union select '500' as name from dual");

            DataTable dtpages = new DataTable();
            dtpages.Columns.Add("name");
            DataRow drp = dtpages.NewRow();
            drp[0] = "10";
            dtpages.Rows.Add(drp);
            drp = dtpages.NewRow();
            drp[0] = "20";
            dtpages.Rows.Add(drp);
            drp = dtpages.NewRow();
            drp[0] = "25";
            dtpages.Rows.Add(drp);
            drp = dtpages.NewRow();
            drp[0] = "50";
            dtpages.Rows.Add(drp);
            drp = dtpages.NewRow();
            drp[0] = "75";
            dtpages.Rows.Add(drp);
            drp = dtpages.NewRow();
            drp[0] = "100";
            dtpages.Rows.Add(drp);
            drp = dtpages.NewRow();
            drp[0] = "500";
            dtpages.Rows.Add(drp);

            model.ddpages = sgen.dt_to_selectlist(dtpages);
            basequey = (string)sgen.GetSession(MyGuid, "basedtquery");
            DataTable dt = new DataTable();
            if (basequey.Trim().Equals(""))
            {
                //dt = (DataTable)sgen.GetSession(MyGuid, "basedatatable");
            }
            // shiv
            else
            {
                dt = sgen.getdata(userCode, "select * from (" + basequey + ") tab where 1=2");
                sgen.SetSession(MyGuid, "basedatatable", dt);
            }
            model = ColswhereFun(model);
            model = BindGrid(model, 1);
            return model;

        }

        private TableResultModel ColswhereFun(TableResultModel model)
        {
            DataTable dt = new DataTable();
            if (sgen.GetSession(MyGuid, "basedatatable") != null) dt = (DataTable)sgen.GetSession(MyGuid, "basedatatable");
            try
            {
                dt.Columns.Remove("chk");
                dt.Columns.Remove("Sr_no");
            }
            catch (Exception err) { }
            cols = ""; colswhere = "";
            DataTable dtcols = new DataTable();
            DataTable dtcolsorder = new DataTable();
            dtcols.Columns.Add("Name", typeof(string));
            dtcolsorder = dtcols.Clone();

            int i = 0;
            foreach (DataColumn dc in dt.Columns)
            {

                if (i > 0)
                {

                    DataRow dr0 = dtcols.NewRow();
                    dr0[0] = dc.ColumnName.Trim().ToUpper();
                    dtcols.Rows.Add(dr0);

                    DataRow dr1 = dtcolsorder.NewRow();
                    dr1[0] = dc.ColumnName.Trim().ToUpper() + " ASC";
                    dtcolsorder.Rows.Add(dr1);
                    dr1 = dtcolsorder.NewRow();
                    dr1[0] = dc.ColumnName.Trim().ToUpper() + " DESC";
                    dtcolsorder.Rows.Add(dr1);
                    if (cols.Equals(""))
                    {
                        cols = dc.ColumnName;
                        colswhere = "NVL(" + dc.ColumnName + ",'0')";
                    }
                    else
                    {
                        cols = cols + "," + dc.ColumnName;
                        colswhere = colswhere + "||" + "NVL(" + dc.ColumnName + ",'0') ";
                    }
                }
                else sgen.SetSession(MyGuid, "firstcol", dc.ColumnName.Trim().ToUpper());
                i++;

            }
            sgen.SetSession(MyGuid, "basedtcols", cols);
            sgen.SetSession(MyGuid, "basedtcolswhere", colswhere);

            sgen.SetSession(MyGuid, "DDNAMES", sgen.dt_to_selectlist(dtcols));
            for (int k = 0; k < model.filters.Count; k++)
            {
                model.filters[k].ddnames = sgen.dt_to_selectlist(dtcols);
            }

            model.ddcolumns = sgen.dt_to_selectlist(dtcols);
            model.ddshowcols = sgen.dt_to_selectlist(dtcols);
            model.ddorderby = sgen.dt_to_selectlist(dtcolsorder);

            if (sgen.GetSession(MyGuid, "ordr") != null)
            {
                model.hforderby = sgen.GetSession(MyGuid, "ordr").ToString();
                model.hfddcolumns = sgen.GetSession(MyGuid, "colexp").ToString();
                model.filters[0].ddnamesSelected = new string[] { sgen.GetSession(MyGuid, "colfil").ToString() };
                model.txttname = sgen.GetSession(MyGuid, "tempname").ToString();
                model.filters[0].ddoperatorsSelected = new string[] { sgen.GetSession(MyGuid, "oper").ToString() };
                model.hfddpages = sgen.GetSession(MyGuid, "records").ToString();
                model.filters[0].txtsval = sgen.GetSession(MyGuid, "filtstr").ToString();
                model.hfddshowcols = sgen.GetSession(MyGuid, "colsshow").ToString();
                sgen.SetSession(MyGuid, "ordr", null);
            }
            return model;
        }

        public TableResultModel BindGrid(TableResultModel model, int pageindex)
        {
            string where = Create_where(model);
            orderby = "";
            if (model.hforderby != null && !model.hforderby.Trim().Equals(""))
            {
                var vals = model.hforderby.Split(',');
                List<string> orders = new List<string>();
                foreach (var vv in vals)
                {
                    var tt = vv.Split(' ');
                    var cc = "lower(" + tt[0] + ") " + tt[1];
                    orders.Add(cc);
                }
                orderby = "order by " + string.Join(",", orders.ToArray());
            }
            else if (sgen.GetSession(MyGuid, "ordr") != null)
            {
                orderby = "order by " + sgen.GetSession(MyGuid, "ordr").ToString();
            }
            else
            {
                //try
                //{
                //    basequey = (string)sgen.GetSession(MyGuid, "basedtquery");
                //    IEnumerable<string> Ewords = new List<string>();
                //    IEnumerable<string> Nwords = new List<string>();
                //    IEnumerable<string> okwords = new List<string>();
                //    IEnumerable<string> allcols = new List<string>();
                //    Ewords = basequey.Split(' ').ToList();
                //    mq1 = "";
                //    mq = "";
                //    bool hasorder = false;
                //    foreach (string line in Ewords.Reverse())
                //    {
                //        string val = line.ToString();
                //        if (val.ToUpper().Trim().Equals("WHERE") || val.ToUpper().Trim().Equals("GROUP") || val.ToUpper().Trim().Equals(")")) { break; }
                //        else
                //        {
                //            if (val.ToUpper().Trim().Equals("ORDER")) { hasorder = true; break; }
                //            if (val.Contains(".")) val = val.Split('.')[1].ToString();
                //            if (mq.Trim().Equals("")) mq = val;
                //            else mq = mq + "#" + val;
                //            if (val.Trim().ToUpper().Equals("ASC") || val.Trim().ToUpper().Equals("DESC") || val.Trim().ToUpper().Equals("BY")) { }
                //            else
                //            {
                //                if (mq1.Trim().Equals("")) mq1 = "order " + val;
                //                else mq1 = mq1 + "#" + val;
                //            }
                //        }
                //    }
                //    allcols = sgen.GetSession(MyGuid, "basedtcols").ToString().ToUpper().Split(',');
                //    okwords = mq1.Split('#');
                //    foreach (var kw in okwords)
                //    {
                //        if (!allcols.Contains(kw.ToUpper())) hasorder = false;

                //    }
                //    string mqw = "";
                //    if (hasorder)
                //    {
                //        Nwords = mq.Split('#').ToList();
                //        foreach (var line in Nwords.Reverse())
                //        {
                //            if (mqw.Trim().Equals("")) mqw = "order " + line;
                //            else mqw = mqw + " " + line;
                //        }
                //        orderby = mqw;
                //    }
                //}
                //catch (Exception err) { orderby = ""; }
            }
            string showcols = "*";
            if (model.hfddshowcols != null && !model.hfddshowcols.Trim().Equals("")) showcols = sgen.GetSession(MyGuid, "firstcol").ToString() + "," + model.hfddshowcols;
            DataSet ds = getpagedate(pageindex, where, orderby, showcols, false, model.txtsearch);
            model = ColswhereFun(model);
            try
            {
                model.sumofclosing = ds.Tables["sumtab"].Rows[0]["closing"].ToString();
                model.sumofdramt = ds.Tables["sumtab"].Rows[0]["dramt"].ToString();
                model.sumofcramt = ds.Tables["sumtab"].Rows[0]["cramt"].ToString();
                model.sumofopening = ds.Tables["sumtab"].Rows[0]["opbal"].ToString();
                model.showtot = "Y";
            }
            catch (Exception ex)
            {
            }
            iPageRecords = (Int32)sgen.GetSession(MyGuid, "iPageRecords");
            try
            {
                model.lbltotrecords = "Showing " + ds.Tables["Maindata"].Rows.Count + " of Total " + ds.Tables["Totals"].Rows[0][0].ToString();
            }
            catch (Exception err)
            {
                model.lbltotrecords = "No Records Found";
            }
            try
            {
                var ddt = ds.Tables["MainData"];
                if (ds.Tables[0].Rows.Count < 1)
                {
                    ViewBag.scripCall = "showmsgJS(3, 'No Data Found', 1);disableForm();";

                }

                model.CampaignDataTable = ds.Tables["MainData"];
                sgen.SetSession(MyGuid, "ALLDATADT", ds.Tables["MainData"]);

            }
            catch (Exception er)
            {
                ViewBag.scripCall = "showmsgJS(3, 'No Data Found', 1);disableForm();";
                //sgen.showmsg(1, "No Data Found", 1);
            }
            //grdviewnew.HeaderRow.Cells[1].Attributes["data-class"] = "expand";
            //grdviewnew.HeaderRow.Cells[3].Attributes["data-class"] = "tablet,phone";
            //grdviewnew.HeaderRow.Cells[2].Attributes["data-class"] = "tablet,phone";
            //grdviewnew.HeaderRow.Cells[4].Attributes["data-class"] = "tablet,phone";

            //int dcc = ds.Tables["MainData"].Columns.Count;
            //for (int k = 0; k < dcc; k++)
            //{
            //    if (k > 4) grdviewnew.HeaderRow.Cells[k].Attributes["data-hide"] = "phone";

            //}
            //grdviewnew.HeaderRow.TableSection = TableRowSection.TableHeader;
            return model;
        }

        public string Create_where(TableResultModel model)
        {
            //foreach (PropertyInfo propertyInfo in model.GetType().GetProperties())
            //{
            //    // do stuff here
            //}

            sgen.SetSession(MyGuid, "filtermodel", model.filters);

            cond = "";
            string opname = "", colname = "", sval = "", searchval = "";

            for (int k = 0; k < model.filters.Count; k++)
            {
                try
                {
                    opname = model.filters[k].ddoperatorsSelected.FirstOrDefault();
                    colname = model.filters[k].ddnamesSelected.FirstOrDefault();
                    sval = model.filters[k].txtsval.Trim().ToLower(); ;
                }
                catch (Exception err)
                { }
                try
                {
                    searchval = model.txtsearch.Trim();
                }
                catch (Exception err) { searchval = ""; }
                if (!sval.Equals(""))
                {
                    if (opname.Equals("Equals To")) cond += " and lower(" + colname + ") = '" + sval.ToString().ToLower() + "'";
                    else if (opname.Equals("Contains")) cond += " and lower(NVL(" + colname + ",'0')) LIKE '%" + sval + "%'";
                    else if (opname.Equals("Smaller Than")) cond += " and " + colname + " < '" + sval + "'";
                    else if (opname.Equals("Greater Than")) cond += " and " + colname + " > '" + sval + "'";
                }
            }
            colswhere = sgen.GetSession(MyGuid, "basedtcolswhere").ToString();
            string where = "";
            if (!colswhere.Trim().Equals("")) where = " where lower(" + colswhere + ") LIKE '%" + searchval.ToLower() + "%'  " + cond;
            return where;
        }

        public DataSet getpagedate(int pageno, string where, string orderby, string showcols, bool loadmore, string searchval)
        {
            userCode = sgen.GetCookie(MyGuid, "userCode");
            DataSet ds = new DataSet();
            string searchVal = "";
            DataTable dtold = new DataTable();
            int olds = 0;
            DataTable alldata = new DataTable();
            string values = "", allkeys = "";
            if (basequey.Trim().Equals(""))
            {
                if (loadmore) return ds;

                if (CHECKTYPE == 2)
                {
                    if (sgen.GetSession(MyGuid, "ALLVALUES") != null)
                    {
                        alldata = (DataTable)sgen.GetSession(MyGuid, "ALLDATADT");
                        if (alldata != null)
                        {
                            dtold = alldata.Clone();
                            allkeys = sgen.GetSession(MyGuid, "ALLVALUES").ToString();
                            if (!allkeys.Trim().Equals(""))
                            {
                                string[] keys = allkeys.Split(',');

                                foreach (var str in keys)
                                {
                                    olds++;
                                    dtold.ImportRow(alldata.Rows[Convert.ToInt32(str)]);

                                    if (values.Trim().Equals("")) values = alldata.Rows[Convert.ToInt32(str)][2].ToString().Trim();
                                    else values = values + "','" + alldata.Rows[Convert.ToInt32(str)][2].ToString().Trim();
                                }
                                dtold.Columns.RemoveAt(0);
                                DataColumn Col = new DataColumn("chk", typeof(Int32));
                                Col.DefaultValue = "1";
                                dtold.Columns.Add(Col);
                                Col.SetOrdinal(0);
                            }
                        }

                    }
                }

                DataTable dataTable = new DataTable();
                DataTable dtsession = (DataTable)sgen.GetSession(MyGuid, "basedatatable");
                dataTable = dtsession.Copy();
                DataColumn newColumn = new DataColumn("chk", typeof(System.Int32));
                newColumn.DefaultValue = "0";
                dataTable.Columns.Add(newColumn);

                newColumn.SetOrdinal(0);
                DataColumn dtsr = dataTable.Columns.Add("Sr_No", typeof(Int32));
                dtsr.SetOrdinal(1);
                //try
                //{
                //    searchVal = Session["searchVal"].ToString();
                //}
                //catch (Exception err) { }


                dataTable = sgen.searchDataTable(searchval, dataTable);

                //var whs = where.Split(new string[] { "and" }, StringSplitOptions.None).Skip(1).ToArray();
                //foreach (var w in whs)
                //{


                //}
                List<Filters> filters = new List<Filters>();
                if (sgen.GetSession(MyGuid, "filtermodel") != null) { filters = (List<Filters>)sgen.GetSession(MyGuid, "filtermodel"); }
                for (int k = 0; k < filters.Count; k++)
                {
                    try
                    {
                        var opname = filters[k].ddoperatorsSelected.FirstOrDefault();
                        var colname = filters[k].ddnamesSelected.FirstOrDefault();
                        var sval = filters[k].txtsval.Trim().ToLower();
                        if (!sval.Trim().Equals(""))
                        {
                            dataTable = sgen.searchDataTable(dataTable, colname, sval, opname);
                        }
                    }
                    catch (Exception err)
                    { }

                }

                if (dtold.Rows.Count > 0)
                {
                    DataView dv1 = dataTable.DefaultView;
                    dv1.RowFilter = "fstr not in ('" + values + "')";
                    dataTable = dv1.ToTable();
                    dataTable.Merge(dtold);
                    DataView dv = dataTable.DefaultView;
                    if (orderby.Equals("order by 1")) dv.Sort = " chk desc";
                    else dv.Sort = orderby;
                    dataTable = dv.ToTable();

                }

                for (int r = 0; r < dataTable.Rows.Count; r++)
                {
                    dataTable.Rows[r]["Sr_No"] = r + 1;

                }
                if (dataTable.Rows.Count > 0)
                {
                    DataView dv = dataTable.DefaultView;
                    if (orderby.Equals("order by 1")) dv.Sort = " Sr_No asc";
                    else dv.Sort = orderby.Replace("order by", "").ToLower();
                    dataTable = dv.ToTable();
                }
                string rows = dataTable.Rows.Count.ToString();

                dataTable.TableName = "MainData";
                mq1 = "SELECT '1' as PageCount";
                DataTable DT2 = sgen.getdata(userCode, mq1);
                DT2.TableName = "PageCount";
                if (basequey.Trim().Equals(""))
                    ds.Tables.Add(dataTable);
                ds.Tables.Add(DT2);
                DT2 = new DataTable();
                DT2.TableName = "Totals";
                DT2.Columns.Add("Total", typeof(string));
                DataRow dataRow = DT2.NewRow();
                dataRow[0] = Convert.ToInt32(rows);
                DT2.Rows.Add(dataRow);
                ds.Tables.Add(DT2);
            }
            else ds = DataByQuery(pageno, where, orderby, showcols, loadmore);

            return ds;
        }

        public DataSet DataByQuery(int pageno, string where, string orderby, string showcols, bool loadmore)
        {
            sgen = new sgenFun(MyGuid);
            userCode = sgen.GetCookie(MyGuid, "userCode");

            DataSet ds = new DataSet();
            DataTable dtold = new DataTable();
            int olds = 0;
            DataTable alldata = new DataTable();
            string values = "", allkeys = "";
            try
            {
                ds.Tables.RemoveAt(0);
            }
            catch { }
            iPageRecords = (Int32)sgen.GetSession(MyGuid, "iPageRecords");
            if (iPageRecords == 0)
            {
                sgen.SetSession(MyGuid, "iPageRecords", 10);
                iPageRecords = 10;
            }
            try
            {
                basequey = (string)sgen.GetSession(MyGuid, "basedtquery");
                if (CHECKTYPE == 2)
                {
                    if (sgen.GetSession(MyGuid, "ALLVALUES") != null)
                    {
                        alldata = (DataTable)sgen.GetSession(MyGuid, "ALLDATADT");
                        allkeys = sgen.GetSession(MyGuid, "ALLVALUES").ToString();
                        if (!allkeys.Trim().Equals(""))
                        {
                            try
                            {
                                string[] keys = allkeys.Split(',');
                                foreach (var str in keys)
                                {

                                    if (values.Trim().Equals("")) values = alldata.Rows[Convert.ToInt32(str)][2].ToString().Trim();
                                    else values = values + "','" + alldata.Rows[Convert.ToInt32(str)][2].ToString().Trim();
                                    olds++;
                                }
                            }
                            catch (Exception err)
                            {

                                if (sgen.GetSession(MyGuid, "ALLVALUES") != null)
                                {

                                    values = sgen.GetSession(MyGuid, "ALLVALUES").ToString();
                                }
                            }

                            if (!loadmore)
                            {
                                // shiv
                                string q1 = "SELECT *  from (" + basequey + " ) tab where fstr in ('" + values + "')";
                                if (showcols == "*") showcols = "t.*";
                                string queryold = "SELECT '1' as chk, tab.* FROM( SELECT  rownum as Sr_No,  " + showcols + " fROM(" + q1 + ") t  " + orderby + ") TAB";
                                dtold = sgen.getdata(userCode, queryold);
                                olds = dtold.Rows.Count;
                                // shiv
                                basequey = "SELECT *  from (" + basequey + " ) tab where fstr not in ('" + values + "')";
                            }
                        }
                    }
                }


                cols = (string)sgen.GetSession(MyGuid, "basedtcols");
                // shiv
                query = "SELECT *  from (" + basequey + " ) tab " + where;
                query2 = query;
                // shiv
                mq0 = "SELECT Count(*) as cnt from (" + query2 + " ) tab";
                //string rows = sgen.seekval(userCode, mq0, "cnt");
                //query3 = "SELECT * FROM( SELECT @rownum:= @rownum + 1 as Sr_No,  t.* fROM(" + query + ") t ,(SELECT @rownum:= " + olds + ") r order by " + orderby + ") TAB WHERE Sr_No BETWEEN " + iPageRecords + " * (" + pageno + " - 1) + 1 AND " + pageno + " *" + iPageRecords;
                //if (seektype == 2)
                if (iPageRecords == 0) iPageRecords = 1;
                if (showcols == "*") showcols = "t.*";
                query3 = "SELECT * FROM ( SELECT '0' as chk,ROWNUM+" + olds + " as Sr_No,tab.* FROM ( SELECT " + showcols + " fROM (" + query + ") t   " + orderby + ") TAB ) TAB WHERE Sr_No BETWEEN " + iPageRecords + " * (" + pageno + " - 1) + 1+" + olds + " AND (" + pageno + " *" + iPageRecords + ")+" + olds;
                DataTable dataTable = new DataTable();
                //dataTable = sgen.getdata(userCode, query3);
                DataSet dsm = sgen.Get_SP2Q(userCode, mq0, query3);
                #region showsumofamounts
                if (Multiton.GetSession(MyGuid, "SHOWTOT") != null)
                {
                    string cond2 = "";
                    if (Multiton.GetSession(MyGuid, "SHOWTOT").ToString().Equals("Y"))
                    {
                        mq = "select distinct LedgerCode from (" + query2 + ") ab";
                        //DataTable dtc = new DataTable();
                        //dtc = sgen.getdata(userCode, mq);
                        //if (dtc.Rows.Count > 0)
                        //{
                           cond2 = " WHERE ACODE in (" + mq + ")";
                        //}
                        string cdt1 = sgen.GetCookie(MyGuid, "year_from");
                        cond = ""; var level = 0; string fdt = "", tdt = "";
                        year_to = sgen.GetCookie(MyGuid, "year_to");
                        year_from = sgen.GetCookie(MyGuid, "year_from");
                        fdt = cdt1.Split(' ')[0].Trim();
                        tdt = year_to.Split(' ')[0].Trim();
                        if (sgen.GetSession(MyGuid, "KPDFDT") != null) fdt = sgen.GetSession(MyGuid, "KPDFDT").ToString();
                        if (sgen.GetSession(MyGuid, "KPDTDT") != null) tdt = sgen.GetSession(MyGuid, "KPDTDT").ToString();
                        string year = year_from.Substring(6, 4);
                        //string currdate = sgen.server_datetime_dt(userCode).ToString("dd/MM/yyyy");
                        string pfstr = "";
                        level = sgen.Make_int(Multiton.GetSession(MyGuid, "LEVEL").ToString());
                        if (level == 1)
                        {
                            if (sgen.GetSession(MyGuid, "l2fstr") != null) cond = " AND (p.client_unit_id || p.acode) in ('" + sgen.GetSession(MyGuid, "l2fstr").ToString() + "')";
                        }
                        if (level == 2)
                        {
                            if (sgen.GetSession(MyGuid, "l3fstr") != null) cond = " AND trim(p.acode) in ('" + sgen.GetSession(MyGuid, "l3fstr").ToString() + "')";
                        }
                        if (level == 4)
                        {
                            if (sgen.GetSession(MyGuid, "l4fstr") != null) cond = " AND substr(p.acode,1,3) in ('" + sgen.GetSession(MyGuid, "l4fstr").ToString() + "')";
                        }
                        DataTable dtsums = new DataTable();
                        mq = "select sum(p.closing) as closing,sum(p.dramt) as dramt,sum(p.cramt) as cramt,sum(p.OPBAL) as opbal from (SELECT ACODE,CLIENT_UNIT_ID,SUM(OPBAL) AS OPBAL,SUM(DRAMOUNT) as dramt,SUM(CRAMOUNT) as cramt,SUM(OPBAL)+SUM(DRAMOUNT)-SUM(CRAMOUNT) as closing from " +
                                                             "(select acode,CLIENT_UNIT_ID, OP_" + year + " as opbal,0 as cramount,0 as dramount,0 as cl from acbal union all select acode,CLIENT_UNIT_ID,sum(dramount)-sum(cramount) as opbal" +
                             ",0 as cramount,0 as dramount,0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + cdt1.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and " +
                                                               "to_date('" + fdt + "','" + sgen.Getsqldateformat() + "')-1 group by acode,CLIENT_UNIT_ID union all select acode,CLIENT_UNIT_ID,0 as opbal,TO_NUMBER(cramount) cramount,TO_NUMBER(dramount) dramount,0 cl from vouchers " +
                                                               "where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                                                               "to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') and to_date('" + tdt + "','" + sgen.Getsqldateformat() + "')) group by acode,CLIENT_UNIT_ID) p " + cond2 + " " + cond + "";
                        dtsums = sgen.getdata(userCode, mq);
                        if (dtsums.Rows.Count > 0)
                        {
                            dtsums.TableName = "sumtab";
                            ds.Tables.Add(dtsums);
                        }
                        Multiton.SetSession(MyGuid, "SHOWTOT", "");
                    }
                } 
                #endregion
                string rows = dsm.Tables[0].Rows[0][0].ToString();
                dataTable = dsm.Tables[1].Copy();
                sgen.SetSession(MyGuid, "basedatatable", dataTable.Clone());
                if (dtold.Rows.Count > 0)
                {
                    dataTable.Merge(dtold);
                    DataView dv = dataTable.DefaultView;
                    dv.Sort = "sr_no asc";
                    dataTable = dv.ToTable();
                }

                dataTable.TableName = "MainData";
                //mq1 = "SELECT (" + rows + "- MOD(" + rows + ", " + iPageRecords + ") ) / " + iPageRecords + " + (CASE WHEN MOD(" + rows + ", " + iPageRecords + ") = 0 THEN 0 ELSE 1 END)  as PageCount FROM DUAL";
                //DataTable DT2 = sgen.getdata(userCode, mq1);
                //SELECT (5- MOD(5, 10) ) / 10 + (CASE WHEN MOD(5, 10) = 0 THEN 0 ELSE 1 END)  as PageCount FROM DUAL

                DataTable DT2 = new DataTable();
                int mod = sgen.Make_int(rows) % iPageRecords;
                int t = 0;
                if (mod > 0) t = 1;
                int a = (sgen.Make_int(rows) - mod / (iPageRecords + t));

                DT2 = new DataTable();
                DT2.Columns.Add("PageCount", typeof(string));
                DataRow dataRow = DT2.NewRow();
                dataRow[0] = a;
                DT2.Rows.Add(dataRow);

                DT2.TableName = "PageCount";
                if (!basequey.Trim().Equals(""))
                    ds.Tables.Add(dataTable);
                ds.Tables.Add(DT2);
                DT2 = new DataTable();
                DT2.TableName = "Totals";
                DT2.Columns.Add("Total", typeof(string));
                dataRow = DT2.NewRow();
                dataRow[0] = Convert.ToInt32(rows) + olds;
                DT2.Rows.Add(dataRow);
                ds.Tables.Add(DT2);

            }
            catch (Exception err) { sgen.showmsg(1, "No Data Found", 2); }

            return ds;
        }

        private DataTable getexpdata(TableResultModel model)
        {
            DataTable dt1 = new DataTable();
            string where = Create_where(model);
            if (model.hforderby != null && !model.hforderby.Trim().Equals("")) orderby = "order by " + model.hforderby;
            else orderby = "order by 1";
            string showcols = "*";
            if (model.hfddshowcols != null && !model.hfddshowcols.Trim().Equals("")) showcols = "Sr_No" + "," + model.hfddshowcols;


            basequey = (string)sgen.GetSession(MyGuid, "basedtquery");

            if (basequey.ToString().Trim().Equals(""))
            {
                dt1 = (DataTable)sgen.GetSession(MyGuid, "basedatatable");
            }
            else
            {
                cols = (string)sgen.GetSession(MyGuid, "basedtcols");

                basequey = "select * from(" + basequey + ") tab " + where;
                query3 = "SELECT tab.* FROM( SELECT ROWNUM as Sr_No,  t.* fROM(" + basequey + ") t  " + orderby + ") TAB";


                if (model.hfddcolumns != null && !model.hfddcolumns.Trim().Equals(""))
                {
                    query = "SELECT Sr_No," + model.hfddcolumns + "  from (" + query3 + " ) tab " + where;
                }
                else
                {
                    query = "SELECT " + showcols + "  from (" + query3 + " ) tab " + where;
                }
                dt1 = sgen.getdata(userCode, query);
            }

            if (dt1.Rows.Count == 0)
            {
                DataRow dr = dt1.NewRow();
                dr[0] = "0";
                dt1.Rows.Add(dr);
            }
            return dt1;

        }

        //protected void grdviewnew_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Row.RowType == DataControlRowType.DataRow)
        //        {
        //            //CheckBox checkBox = (CheckBox)e.Row.FindControl("cbSelect");

        //            //DataTable DTALL = (DataTable)Session["ALLDATATDT"];
        //            //string KK = DTALL.Rows[e.Row.RowIndex]["CHK"].ToString();
        //            //if (KK.Equals("1")) checkBox.Checked = true;
        //            TableCellCollection cell = e.Row.Cells;
        //            e.Row.Attributes["onmouseover"] = string.Format("RowMouseOver({0});", e.Row.RowIndex);
        //            e.Row.Attributes["onmouseout"] = string.Format("RowMouseOut({0});", e.Row.RowIndex);
        //            //if (seektype == 2)
        //            e.Row.Attributes["ondblclick"] = string.Format("btnokclick({0});", e.Row.RowIndex);
        //            //else 
        //            e.Row.Attributes["onclick"] = string.Format("RowSelect({0});", e.Row.RowIndex);
        //            e.Row.Attributes["data-id"] = e.Row.RowIndex.ToString();

        //            //for (int i = 0; i < e.Row.Cells.Count; i++) e.Row.Cells[i].Attributes.Add("onclick", "javascript: GridCellClick('" + e.Row.RowIndex + "','" + i + "');");
        //            for (int i = 1; i < e.Row.Cells.Count; i++)
        //            {
        //                e.Row.Cells[i].Text = HttpUtility.HtmlDecode(e.Row.Cells[i].Text);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    e.Row.Cells[0].Visible = false;
        //    e.Row.Cells[1].Visible = false;
        //    e.Row.Cells[3].Visible = false;
        //    if (seektype == 2)
        //        e.Row.Cells[0].Visible = true;
        //}

        [HttpPost]
        public ContentResult getdata(int pageIndex, string where, string orderby, string showcols, string Myguid)
        {

            FillMst(Myguid);
            userCode = sgen.GetCookie(MyGuid, "userCode");
            DataTable alldata = (DataTable)sgen.GetSession(MyGuid, "ALLDATADT");
            if (!showcols.Trim().Equals("*")) showcols = sgen.GetSession(MyGuid, "firstcol").ToString() + "," + showcols;
            DataSet newds = getpagedate(pageIndex, where, orderby, showcols, true, "");
            alldata.Merge(newds.Tables[0]);
            sgen.SetSession(MyGuid, "ALLDATADT", alldata);
            return Content("loadmore!~!~!" + newds.GetXml());

        }
        [HttpPost]
        public ContentResult Setvalues(string idxs, string Myguid)
        {
            FillMst();
            sgen.SetSession(Myguid, "ALLVALUES", idxs);
            return Content("");

        }
        [HttpPost]
        public ContentResult SetSession(TableResultModel Val, string Name)
        {
            FillMst();
            sgen.SetSession(MyGuid, Name, Val);
            return Content("");
        }
        [HttpPost]
        public ContentResult Save_template(string colfil, string oper, string filtstr, string ordr, string records, string colexp, string tempname, string colsshow)
        {

            try
            {
                FillMst();
                sgenFun sgen = new sgenFun(MyGuid);
                string basequery = sgen.GetSession(MyGuid, "basedtquery").ToString();
                string tempid = sgen.GetSession(MyGuid, "TEMPID").ToString();
                string tab_name = "enx_tab2";
                DataTable dtf = new DataTable();
                dtf = sgen.getdata(userCode, "select * from enx_tab2 WHERE 1=2");
                string userid_mst = sgen.GetCookie(MyGuid, "userid_mst");
                string clientid_mst = sgen.GetCookie(MyGuid, "clientid_mst");
                string unitid_mst = sgen.GetCookie(MyGuid, "unitid_mst");
                userCode = sgen.GetCookie(MyGuid, "userCode");
                string currdate = sgen.server_datetime_local(userCode);
                string vch_date = sgen.Savedate(currdate, true);
                string type = "TML";
                string vch_num = sgen.genNo(userCode, "select to_number(max(vch_num)) as auto_genid from " + tab_name + " where type='" + type + "' " +
                    "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'", 6, "auto_genid");

                mq1 = sgen.seekval(userCode, "select col4 from " + tab_name + " where type='" + type + "' and upper(trim(col4))='" + tempname.Trim().ToUpper() + "' ", "col4");

                if (mq1.Trim().ToUpper().Equals(tempname.Trim().ToUpper()))
                {
                    return Content("0");
                }
                DataRow dr = dtf.NewRow();
                dr["rec_id"] = "0";
                dr["vch_num"] = vch_num;
                dr["vch_date"] = vch_date;
                dr["type"] = type;
                dr["col1"] = colexp;
                dr["col2"] = tempid;
                dr["col3"] = ordr;
                dr["col4"] = tempname;
                dr["col5"] = colfil;
                dr["col6"] = oper;
                dr["col7"] = records;
                dr["col8"] = basequery;
                dr["col9"] = filtstr;
                dr["col13"] = colsshow;
                dr["ent_by"] = userid_mst;
                dr["ent_date"] = vch_date;
                dr["client_id"] = clientid_mst;
                dr["client_unit_id"] = unitid_mst;
                dr["edit_by"] = userid_mst;
                dr["edit_date"] = vch_date;
                dtf.Rows.Add(dr);
                var OK = sgen.Update_data_fast1(userCode, dtf, tab_name, "", false);
                return Content(OK.ToString());
            }
            catch (Exception err)
            {
                return Content(false.ToString());
            }

        }
        //[HttpPost]
        //public ContentResult SearchClick(TableResultModel model, string idxs, int pageIndex, string where, string orderby, string showcols)
        //{
        //    userCode = sgen.GetCookie(MyGuid, "userCode");
        //    sgen.SetSession(MyGuid, "ALLVALUES", idxs);
        //    DataTable alldata = (DataTable)sgen.GetSession(MyGuid, "ALLDATADT");
        //    if (!showcols.Trim().Equals("*")) showcols = sgen.GetSession(MyGuid, "firstcol").ToString() + "," + showcols;
        //    DataSet newds = getpagedate(pageIndex, where, "order by " + orderby, showcols, true, model.txtsearch);
        //    sgen.SetSession(MyGuid, "ALLDATADT", newds.Tables[0]);
        //    return Content("searchclick!~!~!" + newds.GetXml());
        //}


        //protected void btnsrch_Click(object sender, EventArgs e)
        //{
        //    BindGrid(1);
        //}

        //protected void grdviewnew_RowCreated(object sender, GridViewRowEventArgs e)
        //{

        //}

        public FileResult Download()
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(@"c:\folder\myfile.ext");
            string fileName = "myfile.ext";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [HttpGet]
        public FileResult expword(TableResultModel model)
        {
            FillMst();
            model = (TableResultModel)sgen.GetSession(MyGuid, "model");
            string filename = sgen.GetSession(MyGuid, "filename").ToString();
            string GridHtml = sgen.exp_to_word(getexpdata(model), filename);

            //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                StringReader sr = new StringReader(GridHtml);
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                pdfDoc.Close();
                return File(stream.ToArray(), "application/pdf", "Grid.pdf");
            }
        }
        [HttpGet]
        public FileResult expxls(TableResultModel model)
        {
            FillMst();

            model = (TableResultModel)sgen.GetSession(MyGuid, "model");
            //var model = new TableResultModel();
            //model.ddoperatorsSelected = new string[] { opname };
            //model.ddcolumnsSelected = new string[] { colname };
            //model.txtsval = sval;
            //model.txtsearch = searchval;

            string filename = sgen.GetSession(MyGuid, "filename").ToString();
            string GridHtml = sgen.exp_to_xls(getexpdata(model), filename);

            //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                StringReader sr = new StringReader(GridHtml);
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                pdfDoc.Close();
                return File(stream.ToArray(), "application/pdf", "Grid.pdf");
            }
        }
        [HttpGet]
        public FileResult expcsv(TableResultModel model)
        {
            FillMst();
            model = (TableResultModel)sgen.GetSession(MyGuid, "model");
            string cg_com_name = sgen.GetCookie(MyGuid, "cg_com_name");
            string filename = sgen.GetSession(MyGuid, "filename").ToString();
            Byte[] fileBytes = sgen.Exp_to_csv_new(getexpdata(model), filename, cg_com_name);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
            //using (MemoryStream stream = new System.IO.MemoryStream())
            //{
            //    StringReader sr = new StringReader(GridHtml);
            //    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
            //    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
            //    pdfDoc.Open();
            //    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
            //    pdfDoc.Close();
            //    return File(stream.ToArray(), "application/pdf", "Grid.pdf");
            //}
        }
        [HttpGet]
        public FileResult exppdf(TableResultModel model)
        {
            FillMst();

            model = (TableResultModel)sgen.GetSession(MyGuid, "model");
            string cg_com_name = sgen.GetCookie(MyGuid, "cg_com_name");
            string filename = sgen.GetSession(MyGuid, "filename").ToString();
            MemoryStream stream = sgen.exp_to_pdf(getexpdata(model), filename);

            //return File(System.Net.Mime.MediaTypeNames.Application.Octet, filename);
            return File(stream.ToArray(), "application/pdf", "Grid.pdf");

        }
        [HttpGet]
        public ActionResult _partfilters(string filters, string name)
        {
            try
            {
                FillMst();
                TableResultModel model = new TableResultModel();

                string[] obj = filters.Replace("%5B", "[").Replace("%5D", "]").Replace('+', ' ').Split('&');
                List<Filters> Filters = new List<Filters>();

                Filters filter = new Filters();
                int grps = obj.Length / 3;
                int i = 0;
                for (int j = 0; j < grps; j++)
                {
                    filter = new Filters();
                    filter.ddnames = (List<SelectListItem>)sgen.GetSession(MyGuid, "DDNAMES");
                    filter.ddoperators = (List<SelectListItem>)sgen.GetSession(MyGuid, "DDOPERATORS");
                    for (int a = 0; a < 3; a++)
                    {

                        var cname = obj[i].Replace("filters[" + j + "].", "").Replace("[" + j + "].", "").Split('=')[0].ToString();
                        var cval = obj[i].Replace("filters[" + j + "].", "").Replace("[" + j + "].", "").Split('=')[1].ToString();
                        if (cname.Trim().Equals("ddnamesSelected")) filter.ddnamesSelected = new string[] { cval };
                        if (cname.Trim().Equals("ddoperatorsSelected")) filter.ddoperatorsSelected = new string[] { cval };
                        if (cname.Trim().Equals("txtsval")) filter.txtsval = cval;
                        i++;
                    }

                    Filters.Add(filter);
                }
                filter = new Filters();
                filter.ddnames = (List<SelectListItem>)sgen.GetSession(MyGuid, "DDNAMES");
                filter.ddoperators = (List<SelectListItem>)sgen.GetSession(MyGuid, "DDOPERATORS");

                filter.ddnamesSelected = new string[] { "" };
                filter.ddoperatorsSelected = new string[] { "" };
                filter.txtsval = "";

                Filters.Add(filter);

                model.filters = Filters;

                return PartialView("_partfilters", model);
            }
            catch (Exception ex)
            {

                return RedirectToAction("Index");
            }
        }

        //public override void VerifyRenderingInServerForm(Control control)
        //{
        //    /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
        //       server control at run time. */
        //}

        //protected void expxls_ServerClick(object sender, EventArgs e)
        //{

        //    sgen.exp_to_xls(getexpdata(), Session["filename"].ToString());
        //}

        //protected void expcsv_ServerClick(object sender, EventArgs e)
        //{
        //    sgen.Exp_to_csv(getexpdata(), Session["filename"].ToString());
        //}

        //protected void cbSelecteds_CheckedChanged(object sender, EventArgs e)
        //{
        //    CheckBox cbSelects = (CheckBox)sender;

        //    foreach (GridViewRow gridViewRow in this.grdviewnew.Rows)
        //    {
        //        CheckBox cbSelected = (CheckBox)gridViewRow.FindControl("cbSelected");
        //        cbSelected.Checked = cbSelects.Checked;
        //    }
        //}

        //protected void cbSelecteds2_CheckedChanged(object sender, EventArgs e)
        //{
        //    CheckBox cbSelects2 = (CheckBox)sender;

        //    foreach (GridViewRow gridViewRow in this.grdviewnew.Rows)
        //    {
        //        CheckBox cbSelected2 = (CheckBox)gridViewRow.FindControl("cbSelected2");
        //        cbSelected2.Checked = cbSelects2.Checked;
        //    }
        //}



        //protected void callback1_Click(object sender, EventArgs e)
        //{

        //}

        //        public ActionResult Pops()
        //    {
        //        //string val = Session["val1"].ToString();
        //        DataTable dt = new DataTable();
        //        sgen.SetCookie(MyGuid, "val1", "Ram");
        //        Session["val1"] = "rama";
        //        dt = sgen.getdata(userCode, "select m_id,m_name,m_link,m_order from menus limit 10");

        //        List<Tmodel> mod = new List<Tmodel>();
        //        mod = dt.AsEnumerable().Select(row =>
        //        new Tmodel
        //        {
        //            Col1 = row.Field<string>("m_id"),
        //            Col2 = row.Field<string>("m_name")
        //        }).ToList();


        //        var tableResultModel = new TableResultModel();
        //        tableResultModel.CampaignDataTable = dt;
        //        tableResultModel.SortColumn = "";
        //        tableResultModel.SortDirection = "";
        //        return PartialView(tableResultModel);
        //    }
        //    public ActionResult Footab()
        //    {
        //        string val = Session["val1"].ToString();
        //        DataTable dt = new DataTable();
        //        sgen.SetCookie(MyGuid, "val1", "Ram");
        //        Session["val1"] = "rama";
        //        dt = sgen.getdata(userCode, "select m_id,m_name from menus limit 10");

        //        List<Tmodel> mod = new List<Tmodel>();
        //        mod = dt.AsEnumerable().Select(row =>
        //        new Tmodel
        //        {
        //            Col1 = row.Field<string>("m_id"),
        //            Col2 = row.Field<string>("m_name")
        //        }).ToList();

        //        return PartialView(mod);
        //    }
        //    //     private List<CampaignModel> GetCampaigns()
        //    //     {
        //    //         var campaigns = new List<CampaignModel>()
        //    //{
        //    //  new CampaignModel(){Code="C001", Name="Campaign 1", Category="Cat 1", Status="In Progress", DateCreated=DateTime.Now.ToShortDateString()},
        //    //  new CampaignModel(){Code="C002", Name="Campaign 2", Category="Cat 1", Status="In Progress", DateCreated=DateTime.Now.ToShortDateString()},
        //    //  new CampaignModel(){Code="C003", Name="Campaign 3", Category="Cat 1", Status="Completed", DateCreated=DateTime.Now.ToShortDateString()},
        //    //  new CampaignModel(){Code="C004", Name="Campaign 4", Category="Cat 1", Status="In Progress", DateCreated=DateTime.Now.ToShortDateString()},
        //    //  new CampaignModel(){Code="C005", Name="Campaign 5", Category="Cat 1", Status="In Progress", DateCreated=DateTime.Now.ToShortDateString()},
        //    //  new CampaignModel(){Code="C006", Name="Campaign 6", Category="Cat 2", Status="In Progress", DateCreated=DateTime.Now.ToShortDateString()},
        //    //  new CampaignModel(){Code="C007", Name="Campaign 7", Category="Cat 2", Status="Completed", DateCreated=DateTime.Now.ToShortDateString()},
        //    //  new CampaignModel(){Code="C008", Name="Campaign 8", Category="Cat 2", Status="In Progress", DateCreated=DateTime.Now.ToShortDateString()},
        //    //  new CampaignModel(){Code="C009", Name="Campaign 9", Category="Cat 1", Status="In Progress", DateCreated=DateTime.Now.ToShortDateString()},
        //    //  new CampaignModel(){Code="C0010", Name="Campaign 10", Category="Cat 1", Status="In Progress", DateCreated=DateTime.Now.ToShortDateString()},
        //    //  new CampaignModel(){Code="C0011", Name="Campaign 11", Category="Cat 3", Status="In Progress", DateCreated=DateTime.Now.ToShortDateString()},
        //    //  new CampaignModel(){Code="C0012", Name="Campaign 12", Category="Cat 3", Status="Completed", DateCreated=DateTime.Now.ToShortDateString()},
        //    //  new CampaignModel(){Code="C0013", Name="Campaign 13", Category="Cat 4", Status="In Progress", DateCreated=DateTime.Now.ToShortDateString()},
        //    //  new CampaignModel(){Code="C0014", Name="Campaign 14", Category="Cat 2", Status="In Progress", DateCreated=DateTime.Now.ToShortDateString()},
        //    //  new CampaignModel(){Code="C0015", Name="Campaign 15", Category="Cat 1", Status="Cancelled", DateCreated=DateTime.Now.ToShortDateString()}
        //    //};
        //    //         return campaigns;
        //    //     }

        //    public ActionResult Index()
        //    {
        //        //PropertyInfo[] propertyInfos = typeof(CampaignModel).GetProperties();
        //        //var columns = propertyInfos.Select(s => s.Name).ToArray();
        //        //ViewBag.Columns = columns;
        //        //var campaigns = GetCampaigns();
        //        //if (campaigns.Count() % 10 == 0)
        //        //    ViewBag.GridViewPageCount = campaigns.Count() / 10;
        //        //else
        //        //    ViewBag.GridViewPageCount = (campaigns.Count() / 10) + 1;
        //        DataTable dt = sgen.getdata(userCode, "select m_id,m_name from menus limit 10");

        //        List<SelectListItem> mod = new List<SelectListItem>();

        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            var newItem = new SelectListItem { Text =dr["m_name"].ToString(), Value = dr["m_id"].ToString() };
        //            mod.Add(newItem);
        //        }
        //        var newItem2 = new MultiSelectList(mod,"value", "text", mod);
        //        ViewBag.Skills = newItem2;
        //        return View();
        //    }
        //    [HttpPost]
        //    public ActionResult GetCampaignsByAjax(string[] filterColumns, int skip, int take, string sortByColum, string sortDirection)
        //    {
        //        try
        //        {
        //            //List<CampaignModel> campaingns = new List<CampaignModel>();
        //            //if (!string.IsNullOrEmpty(sortByColum))
        //            //{
        //            //    if (string.IsNullOrEmpty(sortDirection))
        //            //        campaingns = GetCampaigns().OrderBy(x => x.GetType().GetProperty(sortByColum).GetValue(x, null)).ToList();
        //            //    else if (sortDirection == "ASC")
        //            //        campaingns = GetCampaigns().OrderBy(x => x.GetType().GetProperty(sortByColum).GetValue(x, null)).ToList();
        //            //    else
        //            //        campaingns = GetCampaigns().OrderByDescending(x => x.GetType().GetProperty(sortByColum).GetValue(x, null)).ToList();
        //            //}
        //            //else
        //            //    campaingns = GetCampaigns().ToList();
        //            //// convert the list to a DataTable.  
        //            //DataTable campaignDataTable = campaingns.Skip(skip).Take(take).ToList().ToDataTable();
        //            //// select only requestd columns from the DataTable.  
        //            //DataTable selectedCampaignDataTable = new DataView(campaignDataTable).ToTable(false, filterColumns);

        //            DataTable dt = new DataTable();
        //            sgen.SetCookie(MyGuid, "val1", "Ram");
        //            Session["val1"] = "rama";
        //            dt = sgen.getdata(userCode, "select m_id,m_name from menus limit 10");

        //            var tableResultModel = new TableResultModel();
        //            tableResultModel.CampaignDataTable = dt;
        //            tableResultModel.SortColumn = sortByColum;
        //            tableResultModel.SortDirection = sortDirection;
        //            return PartialView("_TableResult", tableResultModel);
        //        }
        //        catch (Exception ex)
        //        {
        //        }
        //        return PartialView("_Campaign", new List<CampaignModel>());
        //    }

        //}
        //public class CampaignModel
        //{
        //    public string Code { get; set; }
        //    public string Name { get; set; }
        //    public string Category { get; set; }
        //    public string Status { get; set; }
        //    public string DateCreated { get; set; }
        //}

        //public static class Extension
        //{
        //    public static DataTable ToDataTable<T>(this IList<T> data)
        //    {
        //        PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
        //        DataTable table = new DataTable();
        //        foreach (PropertyDescriptor prop in properties)
        //        {
        //            table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
        //        }
        //        foreach (T item in data)
        //        {
        //            DataRow row = table.NewRow();
        //            foreach (PropertyDescriptor prop in properties)
        //            {
        //                row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
        //            }
        //            table.Rows.Add(row);
        //        }
        //        return table;
        //    }
        //}

        public ActionResult CrystalDoc(string Myguid)
        {
            List<Tmodelmain> model = new List<Tmodelmain>();
            Tmodelmain tm = new Tmodelmain();
            tm.Col14 = Myguid;
            tm.Col15 = Myguid;
            model.Add(tm);
            return View(model);
        }
        public ActionResult ReportView(string Myguid)
        {
            List<Tmodelmain> model = new List<Tmodelmain>();
            Tmodelmain tm = new Tmodelmain();
            tm.Col14 = Myguid;
            tm.Col15 = Myguid;
            model.Add(tm);
            return View(model);
        }
      
        

        [HttpGet]
        public ActionResult ShowReport(string Myguid, string ok)
        {
            List<Tmodelmain> model = new List<Tmodelmain>();
            Tmodelmain tm = new Tmodelmain();
            tm.Col14 = Myguid;
            tm.Col15 = Myguid;

            model.Add(tm);
            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = "generic";
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("mm/dd/yyyy");
            string strToDate = DateTime.Now.ToString("mm/dd/yyyy");

            #region regular
            ReportDocument rd = new ReportDocument();
            MyGuid = Myguid;
            sgen = new sgenFun(MyGuid);
            try
            {
                userCode = sgen.GetCookie(MyGuid, "userCode");
                //if (!IsPostBack)
                {
                    string userid_mst = "", clientid_mst = "", unitid_mst = "", role_mst = "", cg_com_name = "";
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
                            ds.Tables.Add(dt.Copy());
                        }
                        catch (Exception ex) { }
                    }


                    if (ok == null)
                    {
                        jsonErrorCode = "ram";
                        goto end;
                    }

                    if (ds.Tables["prepcur"] == null || ds.Tables["prepcur"].Rows.Count < 1)
                    {
                        string pdfpath = System.Web.HttpContext.Current.Server.MapPath("~/") + "images/nodata.pdf";
                        byte[] bytfile = sgenFun.ReadFile(pdfpath);
                        Response.BinaryWrite(bytfile);
                        jsonErrorCode = "-2";
                        goto end;
                    }

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

                        string Rpt = (string)sgen.GetSession(MyGuid, "Report");
                        if (Rpt.Trim().ToLower().EndsWith(".rpt")) Rpt = Rpt.Substring(0, Rpt.Length - 4);
                        string rptpath = System.Web.HttpContext.Current.Server.MapPath("~/") + "schoolReports_Rpts/" + Rpt + ".rpt";
                        var fname = Path.GetFileName(rptpath);
                        fname = fname.Substring(0, fname.Length - 4);
                        rptpath = System.Web.HttpContext.Current.Server.MapPath("~/") + "schoolReports_Rpts/" + fname + ".rpt";
                        //ReportDocument ReportDocument = new ReportDocument();
                        string xmlpath = System.Web.HttpContext.Current.Server.MapPath("~/") + "xmls/" + fname + ".xml";

                        ds.WriteXml(xmlpath, XmlWriteMode.WriteSchema);
                        rd.Load(rptpath);
                        rd.SetDataSource(ds);
                        //if (!string.IsNullOrEmpty(strFromDate))
                        //    rd.SetParameterValue("fromDate", strFromDate);
                        //if (!string.IsNullOrEmpty(strToDate))
                        //    rd.SetParameterValue("toDate", strFromDate);
                        rd.Refresh();
                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");

                        Multiton.SetSession(Myguid, "rd", rd);
                    }
                }
            }
            catch (Exception err)
            {
                jsonErrorCode = "B";msg = err.Message.ToString();
            }

        #endregion

        end:
            return Json(new { result = jsonErrorCode, err = msg }, JsonRequestBehavior.AllowGet);
        }

        #region open_frames
        public ActionResult open_frame(string Myguid)
        {
            List<Tmodelmain> model = new List<Tmodelmain>();
            Tmodelmain tm = new Tmodelmain();
            tm.Col14 = Myguid;
            tm.Col15 = Myguid;
            model.Add(tm);
            return View(model);
        }
        #endregion

        public ActionResult RefreshReport(string Myguid)
        {
            List<Tmodelmain> model = new List<Tmodelmain>();
            Tmodelmain tm = new Tmodelmain();
            tm.Col14 = Myguid;
            tm.Col15 = Myguid;
            model.Add(tm);
            return PartialView("_ReportsDisplay", model);
        }
        [HttpGet]
        public FileResult pdfonly(string Myguid)
        {
            try
            {
                string title = Multiton.GetSession(Myguid, "title").ToString();
                ReportDocument rd = (ReportDocument)Multiton.GetSession(Myguid, "rd");
                if (rd == null)
                {
                    byte[] byteArray = Encoding.ASCII.GetBytes("No Data...");
                    MemoryStream streaminput = new MemoryStream(byteArray);
                    return File(streaminput, "application/pdf", title + ".pdf");
                }
                else
                {
                    var streaminput = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                    //Stream stream = sgen.PassPdf(streaminput, "Pass@123");
                    return File(streaminput, "application/pdf", title + ".pdf");
                }
            }
            catch (Exception err)
            {
                byte[] byteArray = Encoding.ASCII.GetBytes("No Data...");
                MemoryStream streaminput = new MemoryStream(byteArray);
                return File(streaminput, "application/pdf", "No Data.pdf");

            }
        }
        [HttpGet]
        public ContentResult pdfprint(string Myguid)
        {


            List<Tmodelmain> model = new List<Tmodelmain>();
            Tmodelmain tm = new Tmodelmain();
            tm.Col14 = Myguid;
            tm.Col15 = Myguid;

            model.Add(tm);

            string strFromDate = DateTime.Now.AddDays(-30).ToString("mm/dd/yyyy");
            string strToDate = DateTime.Now.ToString("mm/dd/yyyy");


            ReportDocument rd = new ReportDocument();
            MyGuid = Myguid;
            sgen = new sgenFun(MyGuid);

            userCode = sgen.GetCookie(MyGuid, "userCode");
            string base44 = "";
            try
            {
                string userid_mst = "", clientid_mst = "", unitid_mst = "", role_mst = "", cg_com_name = "";
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
                        ds.Tables.Add(dt.Copy());
                    }
                    catch (Exception ex) { }
                }

                string title = Multiton.GetSession(Myguid, "title").ToString();

                if (ds.Tables["prepcur"] == null || ds.Tables["prepcur"].Rows.Count < 1)
                {
                    string pdfpath = System.Web.HttpContext.Current.Server.MapPath("~/") + "images/nodata.pdf";
                    byte[] bytfile = sgenFun.ReadFile(pdfpath);
                    MemoryStream streaminput = new MemoryStream(bytfile);
                    //return File(streaminput, "application/pdf", title + ".pdf");
                }
                else if (ds.Tables.Count > 0)
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
                    string Rpt = (string)sgen.GetSession(MyGuid, "Report");
                    if (Rpt.Trim().ToLower().EndsWith(".rpt")) Rpt = Rpt.Substring(0, Rpt.Length - 4);
                    string rptpath = System.Web.HttpContext.Current.Server.MapPath("~/") + "schoolReports_Rpts/" + Rpt + ".rpt";
                    var fname = Path.GetFileName(rptpath);
                    fname = fname.Substring(0, fname.Length - 4);
                    rptpath = System.Web.HttpContext.Current.Server.MapPath("~/") + "schoolReports_Rpts/" + fname + ".rpt";
                    //ReportDocument ReportDocument = new ReportDocument();
                    string xmlpath = System.Web.HttpContext.Current.Server.MapPath("~/") + "xmls/" + fname + ".xml";
                    ds.WriteXml(xmlpath, XmlWriteMode.WriteSchema);
                    rd.Load(rptpath);
                    rd.SetDataSource(ds);
                    rd.Refresh();
                }
                try
                {
                    //ReportDocument rd = (ReportDocument)Multiton.GetSession(Myguid, "rd");
                    if (rd == null)
                    {
                        byte[] byteArray = Encoding.ASCII.GetBytes("No Data...");
                        MemoryStream streaminput = new MemoryStream(byteArray);
                        base44 = Convert.ToBase64String(ReadFully(streaminput));
                        //return File(streaminput, "application/pdf", title + ".pdf");
                    }
                    else
                    {
                        var streaminput = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                        //Stream stream = sgen.PassPdf(streaminput, "Pass@123");
                        base44 = Convert.ToBase64String(ReadFully(streaminput));
                        //return File(streaminput, "application/pdf", title + ".pdf");
                    }
                }
                catch (Exception err)
                {
                    byte[] byteArray = Encoding.ASCII.GetBytes("No Data...");
                    MemoryStream streaminput = new MemoryStream(byteArray);
                    //return File(streaminput, "application/pdf", "No Data.pdf");

                }
            }
            catch (Exception err)
            {
                base44 = "BERR__" + err.Message.ToString().Replace("'", ""); ;
            }
            return Content(base44);
        }
        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        private static Stream CopyStream(Stream inputStream)
        {
            const int readSize = 256;
            byte[] buffer = new byte[readSize];
            MemoryStream ms = new MemoryStream();

            int count = inputStream.Read(buffer, 0, readSize);
            while (count > 0)
            {
                ms.Write(buffer, 0, count);
                count = inputStream.Read(buffer, 0, readSize);
            }
            ms.Seek(0, SeekOrigin.Begin);
            inputStream.Seek(0, SeekOrigin.Begin);
            return ms;
        }
        [HttpGet]
        public FileResult wordonly(string Myguid)
        {
            string title = Multiton.GetSession(Myguid, "title").ToString();
            ReportDocument rd = (ReportDocument)Multiton.GetSession(Myguid, "rd");
            if (rd == null)
            {
                byte[] byteArray = Encoding.ASCII.GetBytes("No Data...");
                MemoryStream streaminput = new MemoryStream(byteArray);
                return File(streaminput, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", title + ".doc");
            }
            else
            {
                var streaminput = rd.ExportToStream(ExportFormatType.WordForWindows);
                return File(streaminput, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", title + ".doc");

            }
        }

        [HttpGet]
        public FileResult expcsvonly(string Myguid)
        {
            string title = Multiton.GetSession(Myguid, "title").ToString();
            ReportDocument rd = (ReportDocument)Multiton.GetSession(Myguid, "rd");
            if (rd == null)
            {
                byte[] byteArray = Encoding.ASCII.GetBytes("No Data...");
                MemoryStream streaminput = new MemoryStream(byteArray);
                return File(streaminput, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", title + ".xls");
            }
            else
            {
                var streaminput = rd.ExportToStream(ExportFormatType.CharacterSeparatedValues);
                return File(streaminput, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", title + ".csv");
            }

        }

        [HttpGet]
        public FileResult expformat(string Myguid)
        {
            string title = Multiton.GetSession(Myguid, "title").ToString();
            ReportDocument rd = (ReportDocument)Multiton.GetSession(Myguid, "rd");
            if (rd == null)
            {
                byte[] byteArray = Encoding.ASCII.GetBytes("No Data...");
                MemoryStream streaminput = new MemoryStream(byteArray);
                return File(streaminput, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", title + ".xls");
            }
            else
            {
                var streaminput = rd.ExportToStream(ExportFormatType.Excel);
                return File(streaminput, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", title + ".xls");
            }
        }
        [HttpGet]
        public FileResult expdata(string Myguid)
        {
            string title = Multiton.GetSession(Myguid, "title").ToString();
            ReportDocument rd = (ReportDocument)Multiton.GetSession(Myguid, "rd");
            if (rd == null)
            {
                byte[] byteArray = Encoding.ASCII.GetBytes("No Data...");
                MemoryStream streaminput = new MemoryStream(byteArray);
                return File(streaminput, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", title + ".xlsx");
            }
            else
            {
                var streaminput = rd.ExportToStream(ExportFormatType.ExcelWorkbook);
                return File(streaminput, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", title + ".xlsx");
            }
        }
    }
}
