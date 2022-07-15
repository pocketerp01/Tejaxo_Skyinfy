using QRCoder;
using skyinfyMVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace skyinfyMVC.Controllers
{
    [HandleError(View = "Error")]
    public class EximController : Controller
    {

        System.Globalization.DateTimeFormatInfo monthName = new System.Globalization.DateTimeFormatInfo();
        string mq = "", mq1 = "", vch_num = "", btnval = "", type = "", type_desc = "", ent_date = "", status = "";
        string Master_Type = "", id = "", mid_mst = "", MyGuid = "", master_id, Ac_Year = "", iscfrm = "Y";

        #region 03 June
        Random random = new Random();
        public string name = "", ctype1 = "", ctype2 = "", ctype3 = "", ctype4 = "", ctype5 = "", ctype6 = "", ctype7 = "",
            ctype8 = "", ctype9 = "", ctype10 = "",
            finalpath1 = "", finalpath2 = "", finalpath3 = "", finalpath4 = "", finalpath5 = "", finalpath6 = "", finalpath7 = "", finalpath8 = "",
            finalpath9 = "", finalpath10 = "",
            path1 = "", path2 = "", path3 = "", path4 = "", path5 = "", path6 = "", path7 = "",
            path8 = "", path9 = "", path10 = "",
            fileName1 = "", fileName2 = "", fileName3 = "", fileName4 = "", fileName5 = "", fileName6 = "", fileName7 = "",
            fileName8 = "", fileName9 = "", fileName10 = "",
            encpath1 = "", encpath2 = "", encpath3 = "", encpath4 = "", encpath5 = "", encpath6 = "", encpath7 = "", encpath8 = "",
            encpath9 = "", encpath10 = "",
            reslt = "";

        #endregion

        string userCode = "", userid_mst = "", tab_name = "", where = "", FN_From_Date = "", FN_To_Date = "", Ac_Year_id = "", cg_com_name = "", clientid_mst = "",
            unitid_mst = "", Ac_To_Date = "", Ac_From_Date = "", xprdrange = "", year_from = "", year_to = "", role_mst = "", clientname_mst = "", actionName = "", controllerName = "";
        
        bool isedit = false, res = false, Isvalid = false;
        int cli = 0;
        sgenFun sgen;
        Cmd_Fun cmd_Fun;
        public void FillMst(string Myguid = "")
        {
            MyGuid = Myguid;
            //sgen = new sgenFun(MyGuid);
            if (MyGuid == "") { MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]); }
            //if (MyGuid == "") MyGuid = sgen.GetCookie("", "MyGuid");
            sgen = new sgenFun(MyGuid);
            cmd_Fun = new Cmd_Fun(MyGuid);

            userCode = sgen.GetCookie(MyGuid, "userCode");
            userid_mst = sgen.GetCookie(MyGuid, "userid_mst");
            Ac_Year_id = sgen.GetCookie(MyGuid, "Ac_Year_id");
            cg_com_name = sgen.GetCookie(MyGuid, "cg_com_name");
            clientid_mst = sgen.GetCookie(MyGuid, "clientid_mst");
            unitid_mst = sgen.GetCookie(MyGuid, "unitid_mst");
            FN_From_Date = sgen.GetCookie(MyGuid, "year_from");
            FN_To_Date = sgen.GetCookie(MyGuid, "year_to");
            role_mst = sgen.GetCookie(MyGuid, "role_mst");
            clientname_mst = sgen.GetCookie(MyGuid, "clientname_mst");
            actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            sgen.SetSession(MyGuid, "viewName", actionName);
            sgen.SetSession(MyGuid, "controllerName", controllerName);
            sgen.SetSession(MyGuid, "BackToBack", "");
            year_to = sgen.GetCookie(MyGuid, "year_to");
            year_from = sgen.GetCookie(MyGuid, "year_from");
            xprdrange = "and vch_DATE between " + year_from + " and " + year_to + "";
        }

        //private void pageload_fun(out Tmodelmain tm1, out List<Tmodelmain> model)

        //===============getload==========

        public List<Tmodelmain> getload(List<Tmodelmain> model)
        {
            chkRef();
            Tmodelmain tm1 = new Tmodelmain();
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.vsave = "disabled='disabled'";
            ViewBag.scripCall = "disableForm();";
            ViewBag.vsavenew = "disabled='disabled'";
            tm1.Col100 = "Save & New";
            tm1.Col121 = "S";
            tm1.Col122 = "<u>S</u>ave";
            tm1.Col123 = "Save & Ne<u>w</u>";
            tm1.Col13 = "Save";
            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = MyGuid.Trim();
            model.Add(tm1);
            return model;
        }

        public List<Tmodelmain> getnew(List<Tmodelmain> model)
        {
            sgen.SetSession(MyGuid, "EDMODE", "N");
            ViewBag.vnew = "disabled='disabled'";
            ViewBag.vedit = "disabled='disabled'";
            ViewBag.vsave = "";
            ViewBag.vsavenew = "";
            ViewBag.scripCall = "enableForm();";
            mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
            vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
            model[0].Col16 = vch_num;
            return model;
        }

        public List<Tmodelmain> getedit(List<Tmodelmain> model, DataTable dtt, string param)
        {
            sgen.SetSession(MyGuid, "EDMODE", "Y");
            model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
            model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
            model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
            model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
            model[0].Col7 = dtt.Rows[0]["rec_id"].ToString();
            model[0].Col8 = "client_id||client_unit_id||vch_num|| to_char(vch_date, 'yyyymmdd')|| type = '" + param + "'";
            model[0].Col13 = "Update";
            model[0].Col100 = "Update & New";
            model[0].Col121 = "U";
            model[0].Col122 = "<u>U</u>pdate";
            model[0].Col123 = "Update & Ne<u>w</u>";
            return model;
        }

        private List<Tmodelmain> StartCallback(List<Tmodelmain> model)
        {
            if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
            model = CallbackFun(btnval, "N", actionName, controllerName, model);
            ViewBag.vnew = "disabled='disabled'";
            ViewBag.vedit = "disabled='disabled'";
            ViewBag.vsave = "";
            ViewBag.vsavenew = "";
            return model;
        }

        private ActionResult CancelFun(List<Tmodelmain> model)
        {
            return RedirectToAction(actionName, new { @m_id = EncryptDecrypt.Encrypt(model[0].Col15), @mid = EncryptDecrypt.Encrypt(model[0].Col14) });
        }

        public DataRow getsave(string curdt, DataRow drn, List<Tmodelmain> model, bool edit)
        {
            drn["rec_id"] = "0";
            if (edit)
            {
                //drn["rec_id"] = model[0].Col7;
                drn["ent_by"] = model[0].Col3;
                drn["ent_date"] = model[0].Col4;
                drn["client_id"] = clientid_mst;
                drn["client_unit_id"] = unitid_mst;
                drn["edit_by"] = userid_mst;
                drn["edit_date"] = curdt;
            }
            else
            {
                drn["ent_by"] = userid_mst;
                drn["ent_date"] = curdt;
                drn["edit_by"] = "-";
                drn["edit_date"] = curdt;
                drn["client_id"] = clientid_mst;
                drn["client_unit_id"] = unitid_mst;
            }
            return drn;
        }

        //private void pageload_fun()

        //{
        //    ViewBag.vnew = "";
        //    ViewBag.vedit = "";
        //    ViewBag.vsave = "disabled='disabled'";
        //    ViewBag.scripCall = "disableForm();";
        //    mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
        //    m_id_mst = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
        //    Session["TR_MID"] = mid_mst.Trim();

        //}

        public ActionResult Index()
        {
            return View();
        }

        public void chkRef()
        {
            if (userCode.Equals("")) Response.Redirect(sgenFun.callbackurl);
            if (Request.UrlReferrer == null) { Response.Redirect(sgenFun.callbackurl); }
        }

        //=============callback==============
        #region call back

        public List<Tmodelmain> CallbackFun(string btnval, string edmode, string viewName, string controllerName, List<Tmodelmain> model)
        {
            FillMst();
            string mq = "";
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<SelectListItem> mod2 = new List<SelectListItem>();
            List<SelectListItem> mod3 = new List<SelectListItem>();
            List<SelectListItem> mod4 = new List<SelectListItem>();
            List<SelectListItem> mod5 = new List<SelectListItem>();

            var tm = model.FirstOrDefault();
            //controllerName = Session["controllerName"].ToString();
            //viewName = Session["viewName"].ToString();
            String URL = sgen.GetSession(MyGuid, "SSEEKVAL").ToString();
            btnval = btnval.ToUpper();
            switch (viewName.ToLower())
            {


                #region Export Party Assigned

                case "party_ass":
                    switch (btnval.ToUpper())
                    {

                        case "EDIT":


                            //var tm = model.FirstOrDefault();
                            //if (!Session["TR_MID"].ToString().Trim().Equals(null))
                            //{
                            mq = "select a.group_name, a.client_name,a.rec_id, master_id, a.vch_num, a.master_name,a.master_entby as ent_by," +
                                "a.master_entdate as ent_date,To_Char(a.vch_date, '" + sgen.Getsqldateformat() + "') as vch_date" +
                            ",a.section_Strength,a.classid,a.sectionid,a.Status,a.Inactive_date,a.client_id,a.client_unit_id,a.vch_num,a.type" +
                            " from master_setting a where a.client_id|| a.client_unit_id|| a.master_id|| " +
                            "To_Char(a.vch_date, 'yyyymmdd')|| a.type='" + URL + "' ";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col8 = "client_id|| client_unit_id||master_id|| To_Char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";
                                model[0].Col23 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dt.Rows[0]["vch_date"].ToString();
                                model[0].Col18 = dt.Rows[0]["master_name"].ToString();
                                model[0].Col19 = dt.Rows[0]["classid"].ToString();
                                model[0].Col21 = dt.Rows[0]["sectionid"].ToString();
                                model[0].Col20 = dt.Rows[0]["section_Strength"].ToString();
                                model[0].Col16 = dt.Rows[0]["master_id"].ToString();
                                model[0].Col3 = dt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dt.Rows[0]["ent_date"].ToString();
                                model[0].Col7 = dt.Rows[0]["rec_id"].ToString();
                                model[0].Col22 = dt.Rows[0]["status"].ToString();
                                if ((dt.Rows[0]["status"].ToString() == "")) { model[0].Col17 = "Y"; }
                                model[0].Col13 = "Update";

                            }

                            //}
                            break;
                    }
                    break;

                #endregion

                #region Continents

                case "continents":
                    switch (btnval.ToUpper())
                    {

                        case "EDIT":


                            //var tm = model.FirstOrDefault();
                            //if (!Session["TR_MID"].ToString().Trim().Equals(null))
                            //{
                            mq = "select a.group_name, a.client_name,a.rec_id, master_id, a.vch_num, a.master_name,a.master_entby as ent_by," +
                                "a.master_entdate as ent_date,To_Char(a.vch_date, '" + sgen.Getsqldateformat() + "') as vch_date" +
                            ",a.section_Strength,a.classid,a.sectionid,a.Status,a.Inactive_date,a.client_id,a.client_unit_id,a.vch_num,a.type" +
                            " from master_setting a where a.client_id|| a.client_unit_id|| a.master_id|| " +
                            "To_Char(a.vch_date, 'yyyymmdd')|| a.type='" + URL + "' ";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col8 = "client_id|| client_unit_id||master_id|| To_Char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";
                                model[0].Col23 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dt.Rows[0]["vch_date"].ToString();
                                model[0].Col18 = dt.Rows[0]["master_name"].ToString();
                                model[0].Col19 = dt.Rows[0]["classid"].ToString();
                                model[0].Col21 = dt.Rows[0]["sectionid"].ToString();
                                model[0].Col20 = dt.Rows[0]["section_Strength"].ToString();
                                model[0].Col16 = dt.Rows[0]["master_id"].ToString();
                                model[0].Col3 = dt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dt.Rows[0]["ent_date"].ToString();
                                model[0].Col7 = dt.Rows[0]["rec_id"].ToString();
                                model[0].Col22 = dt.Rows[0]["status"].ToString();
                                if ((dt.Rows[0]["status"].ToString() == "")) { model[0].Col17 = "Y"; }
                                model[0].Col13 = "Update";

                            }

                            //}
                            break;
                    }
                    break;

                #endregion

                #region Bill of Lading

                case "bill_of_lading":
                    switch (btnval.ToUpper())
                    {

                        case "EDIT":


                            //var tm = model.FirstOrDefault();
                            //if (!Session["TR_MID"].ToString().Trim().Equals(null))
                            //{
                            mq = "select a.group_name, a.client_name,a.rec_id, master_id, a.vch_num, a.master_name,a.master_entby as ent_by," +
                                "a.master_entdate as ent_date,To_Char(a.vch_date, '" + sgen.Getsqldateformat() + "') as vch_date" +
                            ",a.section_Strength,a.classid,a.sectionid,a.Status,a.Inactive_date,a.client_id,a.client_unit_id,a.vch_num,a.type" +
                            " from master_setting a where a.client_id|| a.client_unit_id|| a.master_id|| " +
                            "To_Char(a.vch_date, 'yyyymmdd')|| a.type='" + URL + "' ";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col8 = "client_id|| client_unit_id||master_id|| To_Char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";
                                model[0].Col23 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dt.Rows[0]["vch_date"].ToString();
                                model[0].Col18 = dt.Rows[0]["master_name"].ToString();
                                model[0].Col19 = dt.Rows[0]["classid"].ToString();
                                model[0].Col21 = dt.Rows[0]["sectionid"].ToString();
                                model[0].Col20 = dt.Rows[0]["section_Strength"].ToString();
                                model[0].Col16 = dt.Rows[0]["master_id"].ToString();
                                model[0].Col3 = dt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dt.Rows[0]["ent_date"].ToString();
                                model[0].Col7 = dt.Rows[0]["rec_id"].ToString();
                                model[0].Col22 = dt.Rows[0]["status"].ToString();
                                if ((dt.Rows[0]["status"].ToString() == "")) { model[0].Col17 = "Y"; }
                                model[0].Col13 = "Update";

                            }

                            //}
                            break;
                    }
                    break;

                #endregion

                #region Bill of Entry

                case "bill_of_entry":
                    switch (btnval.ToUpper())
                    {

                        case "EDIT":


                            //var tm = model.FirstOrDefault();
                            //if (!Session["TR_MID"].ToString().Trim().Equals(null))
                            //{
                            mq = "select a.group_name, a.client_name,a.rec_id, master_id, a.vch_num, a.master_name,a.master_entby as ent_by," +
                                "a.master_entdate as ent_date,To_Char(a.vch_date, '" + sgen.Getsqldateformat() + "') as vch_date" +
                            ",a.section_Strength,a.classid,a.sectionid,a.Status,a.Inactive_date,a.client_id,a.client_unit_id,a.vch_num,a.type" +
                            " from master_setting a where a.client_id|| a.client_unit_id|| a.master_id|| " +
                            "To_Char(a.vch_date, 'yyyymmdd')|| a.type='" + URL + "' ";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col8 = "client_id|| client_unit_id||master_id|| To_Char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";
                                model[0].Col23 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dt.Rows[0]["vch_date"].ToString();
                                model[0].Col18 = dt.Rows[0]["master_name"].ToString();
                                model[0].Col19 = dt.Rows[0]["classid"].ToString();
                                model[0].Col21 = dt.Rows[0]["sectionid"].ToString();
                                model[0].Col20 = dt.Rows[0]["section_Strength"].ToString();
                                model[0].Col16 = dt.Rows[0]["master_id"].ToString();
                                model[0].Col3 = dt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dt.Rows[0]["ent_date"].ToString();
                                model[0].Col7 = dt.Rows[0]["rec_id"].ToString();
                                model[0].Col22 = dt.Rows[0]["status"].ToString();
                                if ((dt.Rows[0]["status"].ToString() == "")) { model[0].Col17 = "Y"; }
                                model[0].Col13 = "Update";

                            }

                            //}
                            break;
                    }
                    break;

                #endregion

                #region Bulk Comm Invoice
                case "bulk_comm_inv":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            #region Currency
                            TempData[MyGuid + "_Tlist1"] = model[0].TList1 = mod1 = cmd_Fun.curname(userCode, unitid_mst);

                            #endregion

                            mq = "select '-' as fstr, nvl(pi.vch_num,'-') as pi_id,nvl(pi.col10,0) as currate,nvl(pi.col9, 0) as pi_amt_fc," +
                                "nvl(to_char(pi.date1, '" + sgen.Getsqldateformat() + "'), 0) as PI_date,nvl(pi.col7, 0) as docno," +
                                "com.col7,com.ent_by,com.type,com.ent_date,com.rec_id,cm.c_name as name,cm.country,cm.addr,cm.tor," +
                                "com.vch_num,com.col6 as comm_inv_no,com.col5 as Party_id,com.col8 as Comm_Amt_in_FC," +
                                "to_char(convert_tz(com.date1, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as date1, com.col2 as exchange_rate," +
                                "nvl((to_number(pi.col9) * to_number(pi.col10)), 0) as Amt_in_NR," +
                                "nvl((to_number(com.col2) * to_number(com.col8)), 0) as AMT_INR, com.col11 as comm_no," +
                                "to_char(com.date2, '" + sgen.Getsqldateformat() + "') as comm_date,com.col14 as comm_amt_fcdt,com.col12 as comm_type," +
                                "com.col3 as pi_no,com.col17 as pi_bal,nvl(upper(ms.master_name), '-') as pi_curr from enx_tab2 com inner join clients_mst" +
                                " cm on cm.vch_num = com.col5 and cm.type = 'BCD' and cm.client_unit_id=com.client_unit_id" +
                                " left join enx_tab2 pi on pi.col7 = com.col3 and pi.type = 'EPI' and pi.client_unit_id=com.client_unit_id " +
                                "left join master_setting ms on ms.master_id = pi.col6 and ms.type = 'CTM' and ms.client_unit_id = pi.client_unit_id " +
                                "where (com.client_id || com.client_unit_id || com.vch_num || to_char(com.vch_date, 'yyyymmdd') || com.type) = '" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model[0].Col26 = dt.Rows[0]["comm_type"].ToString();
                                if (model[0].Col26 == "91")
                                {
                                    sgen.SetSession(MyGuid, "EDMODE", "Y");
                                    model[0].Col8 = "client_id|| client_unit_id || vch_num ||To_Char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";
                                    model[0].Col16 = dt.Rows[0]["vch_num"].ToString();
                                    model[0].Col22 = dt.Rows[0]["date1"].ToString();
                                    model[0].Col12 = dt.Rows[0]["type"].ToString();
                                    model[0].Col21 = dt.Rows[0]["comm_inv_no"].ToString();
                                    model[0].Col23 = dt.Rows[0]["Comm_Amt_in_FC"].ToString();
                                    model[0].Col24 = dt.Rows[0]["exchange_rate"].ToString();
                                    model[0].Col17 = dt.Rows[0]["Party_id"].ToString();
                                    model[0].Col25 = dt.Rows[0]["AMT_INR"].ToString();
                                    model[0].Col26 = dt.Rows[0]["comm_type"].ToString();
                                    model[0].Col3 = dt.Rows[0]["ent_by"].ToString();
                                    model[0].Col4 = dt.Rows[0]["ent_date"].ToString();
                                    model[0].Col7 = dt.Rows[0]["rec_id"].ToString();
                                    model[0].Col20 = dt.Rows[0]["name"].ToString();
                                    model[0].Col18 = dt.Rows[0]["addr"].ToString();
                                    model[0].Col19 = dt.Rows[0]["country"].ToString();
                                    model[0].Col7 = dt.Rows[0]["rec_id"].ToString();
                                    model[0].TList1 = mod1;
                                    String[] L1 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["col7"].ToString()).Distinct()).Split(',');
                                    model[0].SelectedItems1 = L1;    //Currency
                                    model[0].Col13 = "Update";
                                    model[0].Col100 = "Update & New";
                                    model[0].Col121 = "U";
                                    model[0].Col122 = "<u>U</u>pdate";
                                    model[0].Col123 = "Update & Ne<u>w</u>";

                                }
                                else if (model[0].Col26 == "90")
                                {
                                    sgen.SetSession(MyGuid, "EDMODE", "Y");
                                    model[0].Col8 = "client_id|| client_unit_id|| vch_num ||To_Char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";
                                    model[0].Col16 = dt.Rows[0]["vch_num"].ToString();
                                    model[0].Col22 = dt.Rows[0]["date1"].ToString();
                                    model[0].Col12 = dt.Rows[0]["type"].ToString();
                                    model[0].Col21 = dt.Rows[0]["comm_inv_no"].ToString();
                                    model[0].Col23 = dt.Rows[0]["Comm_Amt_in_FC"].ToString();
                                    model[0].Col24 = dt.Rows[0]["exchange_rate"].ToString();
                                    model[0].Col17 = dt.Rows[0]["Party_id"].ToString();
                                    model[0].Col25 = dt.Rows[0]["AMT_INR"].ToString();
                                    model[0].Col26 = dt.Rows[0]["comm_type"].ToString();
                                    model[0].Col3 = dt.Rows[0]["ent_by"].ToString();
                                    model[0].Col4 = dt.Rows[0]["ent_date"].ToString();
                                    model[0].Col7 = dt.Rows[0]["rec_id"].ToString();
                                    model[0].Col20 = dt.Rows[0]["name"].ToString();
                                    model[0].Col18 = dt.Rows[0]["addr"].ToString();
                                    model[0].Col19 = dt.Rows[0]["country"].ToString();
                                    // model[0].Col52 = dtt.Rows[0]["tor"].ToString();
                                    model[0].TList1 = mod1;
                                    String[] L1 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["col7"].ToString()).Distinct()).Split(',');
                                    model[0].SelectedItems1 = L1;

                                    model[0].Col13 = "Update";
                                    model[0].Col100 = "Update & New";
                                    model[0].Col121 = "U";
                                    model[0].Col122 = "<u>U</u>pdate";
                                    model[0].Col123 = "Update & Ne<u>w</u>";
                                    model[0].dt1 = new DataTable();
                                    model[0].dt1 = ((DataTable)sgen.GetSession(MyGuid, "KMRN_DT")).Clone();

                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        DataRow dr = model[0].dt1.NewRow();
                                        dr["SNo"] = dt.Rows[i]["fstr"].ToString(); ;
                                        dr["PI_No"] = dt.Rows[i]["docno"].ToString();
                                        dr["PI_Date"] = dt.Rows[i]["PI_date"].ToString();
                                        dr["PI_Curr"] = dt.Rows[i]["pi_curr"].ToString();
                                        dr["Curr_rate"] = dt.Rows[i]["currate"].ToString();
                                        dr["pi_amt_fc"] = dt.Rows[i]["pi_amt_fc"].ToString();
                                        dr["pi_amt_lc"] = dt.Rows[i]["Amt_in_NR"].ToString();
                                        dr["comm_amt_fc"] = dt.Rows[i]["comm_amt_fcdt"].ToString();
                                        dr["comm_No"] = dt.Rows[i]["comm_no"].ToString();
                                        dr["comm_Date"] = dt.Rows[i]["comm_date"].ToString();
                                        dr["pi_bal_fc"] = dt.Rows[i]["pi_bal"].ToString();
                                        //dr["pi_amt_lc"] = sgen.Make_decimal( dt.Rows[j]["fcamt"].ToString())* sgen.Make_decimal(dt.Rows[j]["currate"].ToString());

                                        model[0].dt1.Rows.Add(dr);
                                    }
                                    model[0].dt1.AcceptChanges();
                                }
                            }


                            break;

                        case "NEW":

                            mq = "select master_id,master_name,to_char(convert_tz(utc_timestamp(),'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') ci_date " +
                                "from master_Setting where (client_id||client_unit_id||master_id||to_char(vch_date, 'yyyymmdd')||type) = '" + URL + "'";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "N");
                                ViewBag.vnew = "disabled='disabled'";
                                ViewBag.vedit = "disabled='disabled'";
                                ViewBag.vsave = "";
                                ViewBag.vsavenew = "";
                                ViewBag.scripCall = "enableForm();";
                                model[0].Col26 = dtt.Rows[0]["master_id"].ToString();
                                model[0].Col100 = "Save & New";
                                mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12 + "'" + model[0].Col11.Trim() + "";
                                vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                                model[0].Col16 = vch_num;
                                model[0].Col12 = "ECI";
                                sgen.SetSession(MyGuid, "KCITYPE", dtt.Rows[0]["master_id"].ToString());
                                model[0].Col9 = dtt.Rows[0]["master_name"].ToString();
                                model[0].dt1 = new DataTable();
                                model[0].dt1 = ((DataTable)sgen.GetSession(MyGuid, "KMRN_DT"));

                                #region Currency

                                TempData[MyGuid + "_Tlist1"] = model[0].TList1 = mod1 = cmd_Fun.curname(userCode, unitid_mst);

                                model[0].SelectedItems1 = new string[] { "" };
                                #endregion

                            }
                            break;

                        case "PI":
                            mq = "select '-' fstr, pi.vch_num as Doc_Number,pi.col7 as PI_no,pi.col9 as pi_amt_fc,cr.master_name as Currency," +
                                "pi.col10 as exchange_rate,nvl((to_number(pi.col9) * to_number(pi.col10)), 0) as Pi_amt_lc," +
                                "to_char(pi.date1, '" + sgen.Getsqldateformat() + "') as PI_date from enx_tab2 pi " +
                                "inner join master_setting cr on cr.master_id = pi.col6 and cr.type = 'CTM' and cr.client_unit_id = '" + unitid_mst + "' " +
                                "where (pi.client_id || pi.client_unit_id || pi.vch_num || to_char(pi.vch_date, 'yyyymmdd') || pi.type) in ('" + URL + "') ";

                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                if (model[0].dt1.Rows[0]["PI_No"].ToString().Trim().Equals("-")) { model[0].dt1.Rows.RemoveAt(0); }
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["SNo"] = dt.Rows[j]["fstr"].ToString();
                                    dr["PI_No"] = dt.Rows[j]["PI_no"].ToString();
                                    dr["PI_Date"] = dt.Rows[j]["PI_date"].ToString();
                                    dr["PI_Curr"] = dt.Rows[j]["Currency"].ToString();
                                    dr["Curr_rate"] = dt.Rows[j]["exchange_rate"].ToString();
                                    dr["pi_amt_fc"] = dt.Rows[j]["pi_amt_fc"].ToString();
                                    dr["pi_amt_lc"] = dt.Rows[j]["Pi_amt_lc"].ToString();
                                    //dr["pi_amt_lc"] = sgen.Make_decimal( dt.Rows[j]["fcamt"].ToString())* sgen.Make_decimal(dt.Rows[j]["currate"].ToString());

                                    model[0].dt1.Rows.Add(dr);
                                }
                            }
                            break;

                        case "PARTY":

                            mq = "select a.c_name as name,a.c_gstin as gstin,a.country,a.addr,a.vch_num from clients_mst a where a.vch_num||To_Char(a.vch_date, 'yyyymmdd')|| a.type = '" + URL + "'";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                model[0].Col17 = dtt.Rows[0]["vch_num"].ToString();
                                sgen.SetSession(MyGuid, "p_code", dtt.Rows[0]["vch_num"].ToString());
                                model[0].Col20 = dtt.Rows[0]["name"].ToString();
                                model[0].Col18 = dtt.Rows[0]["addr"].ToString();
                                model[0].Col19 = dtt.Rows[0]["country"].ToString();
                                // model[0].Col52 = dtt.Rows[0]["tor"].ToString();
                                sgen.SetSession(MyGuid, "PI_Party", dtt.Rows[0]["vch_num"].ToString());
                            }
                            break;
                    }
                    break;

                #endregion

                #region Export Import Payment Entry

                case "bulk_banking":
                    switch (btnval.ToUpper())
                    {
                        case "NEW":
                            model = getnew(model);
                            if (URL == "001")
                            {
                                model[0].Col9 += "  WITH PI";
                                model[0].Col29 = "001";
                                sgen.SetSession(MyGuid, "inv_type", "001");

                                model[0].dt1 = new DataTable();
                                model[0].dt1 = ((DataTable)sgen.GetSession(MyGuid, "BLBNK_DT"));
                                model[0].dt2 = new DataTable();
                                model[0].dt2 = ((DataTable)sgen.GetSession(MyGuid, "BKFWD_DT"));
                            }
                            else if (URL == "002")
                            {
                                model[0].Col9 += "  WITH CI";
                                model[0].Col29 = "002";
                                sgen.SetSession(MyGuid, "inv_type", "002");

                                model[0].dt1 = new DataTable();
                                model[0].dt1 = ((DataTable)sgen.GetSession(MyGuid, "BLBNK_DT"));
                                model[0].dt2 = new DataTable();
                                model[0].dt2 = ((DataTable)sgen.GetSession(MyGuid, "BKFWD_DT"));

                            }
                            else if (URL == "003")
                            {
                                model[0].Col9 += "  WITH PI AND CI";
                                model[0].Col29 = "003";
                                sgen.SetSession(MyGuid, "inv_type", "003");
                                model[0].dt1 = new DataTable();
                                model[0].dt1 = ((DataTable)sgen.GetSession(MyGuid, "BLBNK_DT"));
                                model[0].dt2 = new DataTable();
                                model[0].dt2 = ((DataTable)sgen.GetSession(MyGuid, "BKFWD_DT"));
                            }
                            model[0].Col41 = "N";
                            break;


                        case "PI":
                            mq = "Select '-' fstr, pi.col10,pi.vch_num,pi.col9 as pi_amt_fc,(case when pi.type = 'EPI' then 'PI' else '-' end) as inv_type," +
                                "to_char(convert_tz(pi.date1, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as doc_dt, pi.col7 as PI_no,upper(ms.master_name) as pi_curr," +
                                "nvl(bb.used_amt, 0) as used_amt,pi.col9 - nvl(bb.used_amt, 0) as bal_amt,to_number(pi.col9) * to_number(pi.col10) as Amt_in_NR " +
                                "from enx_tab2 pi inner join master_setting ms on ms.master_id = pi.col6 and ms.type = 'CTM' and " +
                                "ms.client_unit_id = pi.client_unit_id left join (select sum(col11) as used_amt ,col6 from enx_tab2 where client_unit_id = '" + unitid_mst + "' and " +
                                "type = 'BBK' group by col6) bb on bb.col6 = pi.col7" +
                                " where (pi.client_id || pi.client_unit_id || pi.vch_num || to_char(pi.vch_date, 'yyyymmdd') || pi.type) in ('" + URL + "')";

                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                if (model[0].dt1.Rows[0]["inv_no"].ToString().Trim().Equals("-")) { model[0].dt1.Rows.RemoveAt(0); }
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["SNo"] = dt.Rows[j]["fstr"].ToString();
                                    dr["inv_no"] = dt.Rows[j]["PI_no"].ToString();
                                    dr["inv_date"] = dt.Rows[j]["doc_dt"].ToString();
                                    dr["inv_curr"] = dt.Rows[j]["pi_curr"].ToString();
                                    dr["Curr_rate"] = dt.Rows[j]["col10"].ToString();
                                    dr["inv_type"] = dt.Rows[j]["inv_type"].ToString();
                                    dr["inv_amt_fc"] = dt.Rows[j]["pi_amt_fc"].ToString();
                                    dr["inv_amt_lc"] = dt.Rows[j]["Amt_in_NR"].ToString();

                                    dr["amt_rcpt_fc"] = dt.Rows[j]["used_amt"].ToString();
                                    dr["bal_fc"] = dt.Rows[j]["bal_amt"].ToString();
                                    //dr["tobe_rcpt_fc"] = dt.Rows[j]["to_be_rcpt_fc"].ToString();
                                    model[0].dt1.Rows.Add(dr);
                                }
                                model[0].dt1.AcceptChanges();
                                model[0].Col41 = tm.Col41;
                            }
                            break;

                        case "CI":
                            mq = "select '-' as fstr, com.col6 as comm_inv_no,com.col8 Comm_Amt_in_FC,(case when com.type = 'ECI' then 'CI' else '-' end) as inv_type," +
                                "to_char(com.date1, '" + sgen.Getsqldateformat() + "') as com_inv_date, com.col2 as exchange_rate," +
                                "nvl(bb.used_amt, 0) as used_amt," +
                                "com.col8 - nvl(bb.used_amt, 0) as bal_amt,to_number(com.col8) * to_number(com.col2) as Amt_in_INR,cr.master_name as curr from enx_tab2 com " +
                                "inner join master_setting cr on com.col7 = cr.master_id and cr.type = 'CTM' and cr.client_unit_id = com.client_unit_id " +
                                "left join (select sum(col11) as used_amt ,col6 from enx_tab2 where client_unit_id = '001001' and type = 'BBK' group by col6) bb " +
                                "on bb.col6 = com.col6 where (com.client_id || com.client_unit_id || com.vch_num || to_char(com.vch_date, 'yyyymmdd') || com.type) in ('" + URL + "')";

                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                if (model[0].dt1.Rows[0]["inv_no"].ToString().Trim().Equals("-")) { model[0].dt1.Rows.RemoveAt(0); }
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["SNo"] = dt.Rows[j]["fstr"].ToString();
                                    dr["inv_no"] = dt.Rows[j]["comm_inv_no"].ToString();
                                    dr["inv_date"] = dt.Rows[j]["com_inv_date"].ToString();
                                    dr["inv_curr"] = dt.Rows[j]["curr"].ToString();
                                    dr["Curr_rate"] = dt.Rows[j]["exchange_rate"].ToString();
                                    dr["inv_amt_fc"] = dt.Rows[j]["Comm_Amt_in_FC"].ToString();
                                    dr["inv_type"] = dt.Rows[j]["inv_type"].ToString();
                                    dr["inv_amt_lc"] = dt.Rows[j]["Amt_in_INR"].ToString();
                                    dr["amt_rcpt_fc"] = dt.Rows[j]["used_amt"].ToString();
                                    dr["bal_fc"] = dt.Rows[j]["bal_amt"].ToString();
                                    model[0].dt1.Rows.Add(dr);
                                }
                                model[0].dt1.AcceptChanges();
                                model[0].Col41 = tm.Col41;

                            }
                            break;

                        case "CIPI":

                            dt = ((DataTable)sgen.GetSession(MyGuid, "cipi_dt"));
                            dt = dt.Select("fstr in ('" + URL + "')").CopyToDataTable();
                            if (dt.Rows.Count > 0)
                            {
                                if (model[0].dt1.Rows[0]["inv_no"].ToString().Trim().Equals("-")) { model[0].dt1.Rows.RemoveAt(0); }
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["SNo"] = dt.Rows[j]["fstr"].ToString();
                                    dr["inv_no"] = dt.Rows[j]["Inv_no"].ToString();
                                    dr["inv_date"] = dt.Rows[j]["inv_Date"].ToString();
                                    dr["inv_curr"] = dt.Rows[j]["Currency"].ToString();
                                    dr["Curr_rate"] = dt.Rows[j]["Exchange_Rate"].ToString();
                                    dr["inv_type"] = dt.Rows[j]["inv_type"].ToString();
                                    dr["inv_amt_fc"] = dt.Rows[j]["Amt_in_fc"].ToString();
                                    dr["inv_amt_lc"] = dt.Rows[j]["Amt_in_INR"].ToString();

                                    model[0].dt1.Rows.Add(dr);
                                }
                                model[0].dt1.AcceptChanges();
                                model[0].Col41 = tm.Col41;
                            }
                            break;
                        case "PARTY":

                            if (model[0].Col29 == "001")
                            {
                                mq = "select a.c_name as Cust_Name,a.c_gstin as gstin,a.addr,a.country,a.vch_num as Cust_id,a.tor from clients_mst a where a.vch_num||To_Char(a.vch_date, 'yyyymmdd')|| a.type = '" + URL + "'";

                                dt = sgen.getdata(userCode, mq);
                                if (dt.Rows.Count > 0)
                                {
                                    model[0].Col20 = dt.Rows[0]["Cust_Name"].ToString();
                                    model[0].Col17 = dt.Rows[0]["Cust_id"].ToString();
                                    model[0].Col21 = dt.Rows[0]["Cust_id"].ToString();
                                    model[0].Col23 = dt.Rows[0]["country"].ToString();
                                    sgen.SetSession(MyGuid, "cstmr_id", dt.Rows[0]["Cust_id"].ToString());
                                }
                            }
                            if (model[0].Col29 == "002")
                            {
                                mq = "select a.c_name as Cust_Name,a.c_gstin as gstin,a.addr,a.country,a.vch_num as Cust_id,a.tor from clients_mst a where a.vch_num||To_Char(a.vch_date, 'yyyymmdd')|| a.type = '" + URL + "'";

                                dt = sgen.getdata(userCode, mq);
                                if (dt.Rows.Count > 0)
                                {
                                    model[0].Col20 = dt.Rows[0]["Cust_Name"].ToString();
                                    model[0].Col17 = dt.Rows[0]["Cust_id"].ToString();
                                    model[0].Col21 = dt.Rows[0]["Cust_id"].ToString();
                                    sgen.SetSession(MyGuid, "cstmr_id", dt.Rows[0]["Cust_id"].ToString());
                                    model[0].Col23 = dt.Rows[0]["country"].ToString();
                                }
                            }
                            if (model[0].Col29 == "003")
                            {
                                mq = "select a.c_name as Cust_Name,a.c_gstin as gstin,a.addr,a.country,a.vch_num as Cust_id,a.tor from clients_mst a where a.vch_num||To_Char(a.vch_date, 'yyyymmdd')|| a.type = '" + URL + "'";

                                dt = sgen.getdata(userCode, mq);
                                if (dt.Rows.Count > 0)
                                {
                                    model[0].Col20 = dt.Rows[0]["Cust_Name"].ToString();
                                    model[0].Col17 = dt.Rows[0]["Cust_id"].ToString();
                                    model[0].Col21 = dt.Rows[0]["Cust_id"].ToString();
                                    model[0].Col23 = dt.Rows[0]["country"].ToString();
                                    sgen.SetSession(MyGuid, "cstmr_id", dt.Rows[0]["Cust_id"].ToString());
                                }
                            }

                            model[0].Col41 = "N";

                            break;

                        case "EDIT":

                            mq = "select distinct '-' fstr,cl.country,a.master_name as Bank_name,cm.cpname as Bank_Acc,cm.vch_num as bnk_id,cm.REFERED_BY as Branch_Name,nvl(bb.col11,0) as ci_used_amt," +
                                "nvl((ci.col8 - bb.col11),0) as ci_bal_amt,pi.col9 - nvl(bb.col11, 0) as pi_bal_fc,nvl(bb.col11,0) as pi_used_amt,cm.bnkaddr as Address,bb.rec_id,cl.addr," +
                                "bb.ent_by,nvl(to_char(bb.ent_date, '" + sgen.GetSaveSqlDateFormat() + "'), '0') as ent_date,nvl(to_char(bb.vch_date, '" + sgen.GetSaveSqlDateFormat() + "'), '0') as vch_date,bb.client_id," +
                                "bb.client_unit_id,cl.vch_num Cust_id, bb.vch_num,at.master_name as bnk_Account_type,ct.master_name as bnk_currency_type,bb.col2 as fwd_use,cl.C_name Cust_Name, " +
                                "nvl(pi.col7, 0) as PI_no,nvl(pi.col9, 0) as pi_amt_fc,nvl(ci.col8, 0) C_amt_in_FC,bb.col6 as inv_no,bb.col8 as inv_type,bb.col9 as amc_rcpt_fc,bb.col10 as bal_fc," +
                                "bb.col11 as to_be_rcpt_fc,nvl(to_number(pi.col9) * to_number(pi.col10), 0) as pi_Amt_INR,bb.col7 inv_typ," +
                                "nvl(to_number(ci.col8) * to_number(ci.col2), 0) as ci_Amt_INR,nvl(cr.master_name,0) as ci_curr,pi_cr.master_name as pi_curr," +
                                "nvl(ci.col2, 0) as ci_exchange_rate,nvl(ci.vch_num, 0) as ci_vchnum,nvl(pi.vch_num, 0) pi_vchnum," +
                                "nvl(to_char(ci.date1, '" + sgen.Getsqldateformat() + "'), 0) as com_inv_date,nvl(ci.col6, 0) as comm_inv_no," +
                                "nvl(to_char(convert_tz(pi.date1, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "'), 0) as doc_dt,nvl(pi.col10, 0) as pi_exc_rate from enx_tab2 bb " +
                                "inner join clients_mst cm on cm.vch_num = bb.col17 and cm.client_unit_id = bb.client_unit_id and cm.type = 'BAD' " +
                                "inner join master_setting at on at.master_id = cm.ploc and at.client_unit_id = cm.client_unit_id and at.type = 'BTM' " +
                                "inner join master_setting ct on ct.master_id = cm.panno and ct.client_unit_id = cm.client_unit_id and ct.type = 'CTM' " +
                                "inner join master_setting a on a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id " +
                                "left join enx_tab2 pi on pi.col7 = bb.col6 and pi.type = 'EPI' and pi.client_unit_id = bb.client_unit_id left join enx_tab2 ci on ci.col6 = bb.col6 " +
                                "and ci.type = 'ECI' and ci.client_unit_id = bb.client_unit_id  inner join clients_mst cl on cl.vch_num = bb.col5 and cl.type = 'BCD' " +
                                "and cl.client_unit_id = bb.client_unit_id left join master_setting cr on(cr.master_id = ci.col7) and cr.type = 'CTM' and cr.client_unit_id = ci.client_unit_id  " +
                                "left join master_setting pi_cr on(pi_cr.master_id = pi.col6) and pi_cr.type = 'CTM' and pi_cr.client_unit_id = pi.client_unit_id " +
                                "where (bb.client_id || bb.client_unit_id || bb.vch_num || to_char(bb.vch_date, 'yyyymmdd') || bb.type) = '" + URL + "'";

                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col16 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dt.Rows[0]["Cust_id"].ToString();
                                model[0].Col20 = dt.Rows[0]["Cust_Name"].ToString();
                                model[0].Col21 = dt.Rows[0]["Cust_id"].ToString();
                                model[0].Col23 = dt.Rows[0]["country"].ToString();
                                model[0].Col31 = dt.Rows[0]["Bank_name"].ToString();
                                model[0].Col32 = dt.Rows[0]["bnk_id"].ToString();
                                model[0].Col33 = dt.Rows[0]["Branch_Name"].ToString();
                                model[0].Col34 = dt.Rows[0]["Bank_Acc"].ToString();
                                model[0].Col35 = dt.Rows[0]["bnk_Account_type"].ToString();
                                model[0].Col36 = dt.Rows[0]["bnk_currency_type"].ToString();
                                model[0].Col29 = dt.Rows[0]["inv_typ"].ToString();
                                model[0].Col8 = "client_id|| client_unit_id || vch_num || To_Char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";
                                model[0].Col13 = "Update";
                                model[0].Col100 = "Update & New";
                                model[0].Col121 = "U";
                                model[0].Col122 = "<u>U</u>pdate";
                                model[0].Col123 = "Update & Ne<u>w</u>";
                                if (dt.Rows[0]["inv_typ"].ToString() == "001")
                                {
                                    model[0].Col9 += " WITH PI";
                                    //model[0].Col29 = dt.Rows[0]["inv_typ"].ToString();
                                    model[0].dt1 = new DataTable();
                                    model[0].dt1 = ((DataTable)sgen.GetSession(MyGuid, "BLBNK_DT")).Clone();
                                    for (int j = 0; j < dt.Rows.Count; j++)
                                    {
                                        DataRow dr = model[0].dt1.NewRow();
                                        dr["SNo"] = dt.Rows[j]["fstr"].ToString();
                                        dr["inv_no"] = dt.Rows[j]["PI_no"].ToString();
                                        dr["inv_date"] = dt.Rows[j]["doc_dt"].ToString();
                                        dr["inv_curr"] = dt.Rows[j]["pi_curr"].ToString();
                                        dr["Curr_rate"] = dt.Rows[j]["pi_exc_rate"].ToString();
                                        dr["inv_type"] = dt.Rows[j]["inv_type"].ToString();
                                        dr["inv_amt_fc"] = dt.Rows[j]["pi_amt_fc"].ToString();
                                        dr["inv_amt_lc"] = dt.Rows[j]["pi_Amt_INR"].ToString();
                                        dr["amt_rcpt_fc"] = dt.Rows[j]["pi_used_amt"].ToString();
                                        dr["bal_fc"] = dt.Rows[j]["pi_bal_fc"].ToString();
                                        dr["tobe_rcpt_fc"] = dt.Rows[j]["to_be_rcpt_fc"].ToString();
                                        //dr["pi_amt_lc"] = sgen.Make_decimal( dt.Rows[j]["fcamt"].ToString())* sgen.Make_decimal(dt.Rows[j]["currate"].ToString());

                                        model[0].dt1.Rows.Add(dr);
                                    }
                                    model[0].dt1.AcceptChanges();
                                }

                                else if (dt.Rows[0]["inv_typ"].ToString() == "002")
                                {
                                    model[0].Col9 += " WITH CI";
                                    model[0].Col29 = dt.Rows[0]["inv_typ"].ToString();
                                    model[0].dt1 = new DataTable();
                                    model[0].dt1 = ((DataTable)sgen.GetSession(MyGuid, "BLBNK_DT")).Clone();
                                    for (int j = 0; j < dt.Rows.Count; j++)
                                    {
                                        DataRow dr = model[0].dt1.NewRow();
                                        dr["SNo"] = dt.Rows[j]["fstr"].ToString();
                                        dr["inv_no"] = dt.Rows[j]["comm_inv_no"].ToString();
                                        dr["inv_date"] = dt.Rows[j]["com_inv_date"].ToString();
                                        dr["inv_curr"] = dt.Rows[j]["ci_curr"].ToString();
                                        dr["Curr_rate"] = dt.Rows[j]["ci_exchange_rate"].ToString();
                                        dr["inv_amt_fc"] = dt.Rows[j]["C_amt_in_FC"].ToString();
                                        dr["inv_type"] = dt.Rows[j]["inv_type"].ToString();
                                        dr["inv_amt_lc"] = dt.Rows[j]["ci_Amt_INR"].ToString();
                                        dr["amt_rcpt_fc"] = dt.Rows[j]["ci_used_amt"].ToString();
                                        dr["bal_fc"] = dt.Rows[j]["ci_bal_amt"].ToString();
                                        dr["tobe_rcpt_fc"] = dt.Rows[j]["to_be_rcpt_fc"].ToString();
                                        model[0].dt1.Rows.Add(dr);
                                    }
                                    model[0].dt1.AcceptChanges();
                                }

                                else if (dt.Rows[0]["inv_typ"].ToString() == "003")
                                {
                                    model[0].Col9 += " WITH CI AND PI";
                                    for (int j = 0; j < dt.Rows.Count; j++)
                                    {
                                        if (model[0].dt1.Rows[0]["inv_no"].ToString().Trim().Equals("-")) { model[0].dt1.Rows.RemoveAt(0); }
                                        if (dt.Rows[j]["inv_type"].ToString() == "CI")
                                        {
                                            DataRow dr = model[0].dt1.NewRow();
                                            dr["SNo"] = dt.Rows[j]["fstr"].ToString();
                                            dr["inv_no"] = dt.Rows[j]["comm_inv_no"].ToString();
                                            dr["inv_date"] = dt.Rows[j]["com_inv_date"].ToString();
                                            dr["inv_curr"] = dt.Rows[j]["ci_curr"].ToString();
                                            dr["Curr_rate"] = dt.Rows[j]["ci_exchange_rate"].ToString();
                                            dr["inv_amt_fc"] = dt.Rows[j]["C_amt_in_FC"].ToString();
                                            dr["inv_type"] = dt.Rows[j]["inv_type"].ToString();
                                            dr["inv_amt_lc"] = dt.Rows[j]["ci_Amt_INR"].ToString();
                                            dr["amt_rcpt_fc"] = dt.Rows[j]["ci_used_amt"].ToString();
                                            dr["bal_fc"] = dt.Rows[j]["ci_bal_amt"].ToString();
                                            dr["tobe_rcpt_fc"] = dt.Rows[j]["to_be_rcpt_fc"].ToString();
                                            model[0].dt1.Rows.Add(dr);
                                        }
                                        else if (dt.Rows[j]["inv_type"].ToString() == "PI")
                                        {
                                            DataRow dr = model[0].dt1.NewRow();
                                            dr["SNo"] = dt.Rows[j]["fstr"].ToString();
                                            dr["inv_no"] = dt.Rows[j]["PI_no"].ToString();
                                            dr["inv_date"] = dt.Rows[j]["doc_dt"].ToString();
                                            dr["inv_curr"] = dt.Rows[j]["pi_curr"].ToString();
                                            dr["Curr_rate"] = dt.Rows[j]["pi_exc_rate"].ToString();
                                            dr["inv_type"] = dt.Rows[j]["inv_type"].ToString();
                                            dr["inv_amt_fc"] = dt.Rows[j]["pi_amt_fc"].ToString();
                                            dr["inv_amt_lc"] = dt.Rows[j]["pi_Amt_INR"].ToString();
                                            dr["amt_rcpt_fc"] = dt.Rows[j]["pi_used_amt"].ToString();
                                            dr["bal_fc"] = dt.Rows[j]["pi_bal_fc"].ToString();
                                            dr["tobe_rcpt_fc"] = dt.Rows[j]["to_be_rcpt_fc"].ToString();
                                            model[0].dt1.Rows.Add(dr);
                                        }
                                        model[0].dt1.AcceptChanges();
                                    }
                                }

                                if (dt.Rows[0]["fwd_use"].ToString() == "Y")
                                {

                                    mq = "select distinct '-' as fstr, nvl(bbw.col16,0) as tobeused_amt,nvl(bbw.col16, 0) as used_amt,nvl((bk.col11 - bbw.col16), 0) as Bal_amt," +
                                        "(bbw.client_id||bbw.client_unit_id||bbw.vch_num||to_char(bbw.vch_date, 'yyyymmdd')|| bbw.type) as bbw_fstr," +
                                        "nvl(bk.col11, 0) as Forward_Amount,nvl(bk.vch_num, 0) as fw_doc,nvl(bk.col9, 0) as spot_rate,nvl(bk.col8, 0) as Premium_Discount," +
                                        "nvl(bk.col10, 0) as Forward_no,nvl((bk.col9 + bk.col8), 0) AS FWD_Rate,nvl(((bk.col9 + bk.col8) * bk.col11), 0) AS LOCAL_CUR, nvl(bk.col2, 0)" +
                                        " as nature_of_transaction,nvl(to_char(bk.date2, '" + sgen.Getsqldateformat() + "'), 0) as bk_start," +
                                        "nvl(to_char(bk.date3, '" + sgen.Getsqldateformat() + "'), 0) as bk_end," +
                                        "nvl(to_char(bk.date4, '" + sgen.Getsqldateformat() + "'), 0) as bk_gc_end" +
                                        ",nvl(to_char(bk.date5, '" + sgen.Getsqldateformat() + "'), 0) as bk_date,cur.master_name as fwd_curr from enx_tab2 bb" +
                                        " left join enx_tab2 bbw on bbw.vch_num = bb.vch_num and bbw.client_unit_id = bb.client_unit_id " +
                                        "and bbw.type = 'BBW' left join vehicle_master bk on bk.vch_num = bbw.col13 and bk.type = 'BFW' and bk.client_unit_id = bbw.client_unit_id inner join master_setting cur on cur.master_id = bk.col12 and cur.type = 'CTM' and" +
                                        " cur.client_unit_id = bk.client_unit_id " +
                                        "where (bb.client_id || bb.client_unit_id || bb.vch_num || to_char(bb.vch_date, 'yyyymmdd') || bb.type) = '" + URL + "'";

                                    dtt = sgen.getdata(userCode, mq);
                                    if (dtt.Rows.Count > 0)
                                    {
                                        model[0].Col85 = "(client_id||client_unit_id||vch_num||to_char(vch_date, 'yyyymmdd')|| type) = '" + dtt.Rows[0]["bbw_fstr"].ToString() + "'";
                                        model[0].dt2 = new DataTable();
                                        model[0].dt2 = ((DataTable)sgen.GetSession(MyGuid, "BKFWD_DT")).Clone();
                                        for (int i = 0; i < dtt.Rows.Count; i++)
                                        {
                                            DataRow dr = model[0].dt2.NewRow();
                                            dr["Forward"] = dtt.Rows[i]["fstr"].ToString();
                                            dr["SNo"] = i + 1;
                                            dr["Fwd_no"] = dtt.Rows[i]["Forward_no"].ToString();
                                            dr["Dock_no"] = dtt.Rows[i]["fw_doc"].ToString();
                                            dr["Fwd_date"] = dtt.Rows[i]["bk_date"].ToString();
                                            dr["FWD_Amt"] = dtt.Rows[i]["Forward_Amount"].ToString();
                                            dr["FWD_Curr"] = dtt.Rows[i]["fwd_curr"].ToString();
                                            dr["Fwd_type"] = dtt.Rows[i]["nature_of_transaction"].ToString();
                                            dr["Fwd_start"] = dtt.Rows[i]["bk_start"].ToString();
                                            dr["Fwd_end"] = dtt.Rows[i]["bk_end"].ToString();
                                            dr["Fwd_gc_end"] = dtt.Rows[i]["bk_gc_end"].ToString();
                                            dr["Spot_Rate"] = dtt.Rows[i]["spot_rate"].ToString();
                                            dr["Premium"] = dtt.Rows[i]["Premium_Discount"].ToString();
                                            dr["Fwd_Rate"] = dtt.Rows[i]["FWD_Rate"].ToString();
                                            dr["Fwd_used_Amt"] = dtt.Rows[i]["used_amt"].ToString();
                                            dr["Fwd_Bal_Amt"] = dtt.Rows[i]["bal_amt"].ToString();
                                            dr["Fwd_To_be_use"] = dtt.Rows[i]["tobeused_amt"].ToString();
                                            model[0].dt2.Rows.Add(dr);
                                        }
                                        model[0].dt2.AcceptChanges();
                                        model[0].Col41 = "Y";
                                    }
                                }
                                else
                                {
                                    model[0].Col41 = "N";
                                }
                                model[0].Col16 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col3 = dt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dt.Rows[0]["ent_date"].ToString();
                                model[0].Col7 = dt.Rows[0]["rec_id"].ToString();
                            }
                            break;

                        case "BANK":

                            mq = "select a.master_name as Bank_name,a.master_id as bnk_name_id,cm.bnk,cm.vch_num,cm.cpname as Bank_Acc,cm.REFERED_BY as Branch_Name,at.master_name as Account_type," +
                                "ct.master_name as currency_type,ct.master_id as cur_id,cm.bnkaddr as Address,cm.Addr as Department from clients_mst cm inner join master_setting a on " +
                                "a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id inner join master_setting at on" +
                                " at.master_id = cm.ploc and at.client_unit_id = cm.client_unit_id and at.type = 'BTM' inner join master_setting ct " +
                                "on ct.master_id = cm.panno and ct.client_unit_id = cm.client_unit_id and ct.type = 'CTM'" +
                                " where (cm.client_id || cm.client_unit_id || cm.vch_num || to_char(cm.vch_date, 'yyyymmdd') || cm.type) = '" + URL + "'";

                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model[0].Col31 = dt.Rows[0]["Bank_name"].ToString();
                                model[0].Col32 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col33 = dt.Rows[0]["Branch_Name"].ToString();
                                model[0].Col34 = dt.Rows[0]["Bank_Acc"].ToString();
                                model[0].Col35 = dt.Rows[0]["Account_type"].ToString();
                                model[0].Col36 = dt.Rows[0]["currency_type"].ToString();
                                model[0].Col38 = dt.Rows[0]["cur_id"].ToString();
                                if (dt.Rows[0]["bnk"].ToString() == "Y")
                                {
                                    model[0].Col41 = dt.Rows[0]["bnk"].ToString();
                                }
                                else
                                {
                                    model[0].Col41 = "N";
                                }
                                sgen.SetSession(MyGuid, "cur_type", dt.Rows[0]["cur_id"].ToString());
                                sgen.SetSession(MyGuid, "bnk_name_id", dt.Rows[0]["bnk_name_id"].ToString());
                            }
                            break;

                        case "FRWD":

                            mq = "select '-' as fstr, bk.col11 as Forward_Amount,bk.vch_num,bk.col9 as spot_rate,nvl(bb.used_amt,0) as used_amt,bk.col8 as Premium_Discount," +
                                "bk.col11 - nvl(bb.used_amt, 0) as bal_amt,bk.col10 as Forward_no,(bk.col9 + bk.col8) AS FWD_Rate,((bk.col9 + bk.col8) * bk.col11) AS LOCAL_CUR," +
                                "bk.col2 as nature_of_transaction,to_char(bk.date2, '" + sgen.Getsqldateformat() + "') as Forward_Start_Date," +
                                "to_char(bk.date3, '" + sgen.Getsqldateformat() + "') as Forward_end_Date," +
                                "to_char(bk.date4, '" + sgen.Getsqldateformat() + "') as Forward_grace_end,cur.master_name as fwd_cur," +
                                "to_char(bk.date5, '" + sgen.Getsqldateformat() + "') as fwd_date from vehicle_master bk " +
                                "left join (select sum(col16) as used_amt ,col4 from enx_tab2 where client_unit_id = '" + unitid_mst + "' and type = 'BBW' group by col4) bb" +
                                " on bb.col4 = bk.col10 inner join master_setting cur on cur.master_id = bk.col12 and cur.type = 'CTM' and cur.client_unit_id = bk.client_unit_id" +
                                " where (bk.client_id || bk.client_unit_id || bk.vch_num || to_char(bk.vch_date, 'yyyymmdd') || bk.type) = '" + URL + "'";


                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                if (model[0].dt2.Rows[0]["Fwd_no"].ToString().Trim().Equals("-")) { model[0].dt2.Rows.RemoveAt(0); }
                                model[0].dt2 = new DataTable();
                                model[0].dt2 = ((DataTable)sgen.GetSession(MyGuid, "BKFWD_DT")).Clone();
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    DataRow dr = model[0].dt2.NewRow();
                                    dr["Forward"] = dt.Rows[i]["fstr"].ToString();
                                    dr["SNo"] = i + 1;
                                    dr["Fwd_no"] = dt.Rows[i]["Forward_no"].ToString();
                                    dr["Dock_no"] = dt.Rows[i]["vch_num"].ToString();
                                    dr["Fwd_date"] = dt.Rows[i]["fwd_date"].ToString();
                                    dr["FWD_Amt"] = dt.Rows[i]["Forward_Amount"].ToString();
                                    dr["FWD_Curr"] = dt.Rows[i]["fwd_cur"].ToString();
                                    dr["Fwd_type"] = dt.Rows[i]["nature_of_transaction"].ToString();
                                    dr["Fwd_start"] = dt.Rows[i]["Forward_Start_Date"].ToString();
                                    dr["Fwd_end"] = dt.Rows[i]["Forward_end_Date"].ToString();
                                    dr["Fwd_gc_end"] = dt.Rows[i]["Forward_grace_end"].ToString();
                                    dr["Spot_Rate"] = dt.Rows[i]["spot_rate"].ToString();
                                    dr["Premium"] = dt.Rows[i]["Premium_Discount"].ToString();
                                    dr["Fwd_Rate"] = dt.Rows[i]["FWD_Rate"].ToString();
                                    dr["Fwd_used_Amt"] = dt.Rows[i]["used_amt"].ToString();
                                    dr["Fwd_Bal_Amt"] = dt.Rows[i]["bal_amt"].ToString();
                                    model[0].dt2.Rows.Add(dr);
                                }
                                model[0].dt2.AcceptChanges();
                                model[0].Col41 = tm.Col41;
                            }


                            break;

                    }
                    break;

                #endregion

                #region Export Incentive Master

                case "exp_inc_m":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select to_char(a.date1,'" + sgen.Getsqldateformat() + "') as from_dt,to_char(a.date2,'" + sgen.Getsqldateformat() + "') as to_dt,a.* from enx_tab a where a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type = '" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                #region Inc type
                                TempData[MyGuid + "_Tlist1"] = model[0].TList1 = mod1 = cmd_Fun.incentive_type(userCode, unitid_mst);
                                #endregion
                                #region Country 
                                TempData[MyGuid + "_Tlist2"] = model[0].TList2 = mod2 = cmd_Fun.country(userCode, unitid_mst);
                                #endregion
                                model = getedit(model, dt, URL);
                                String[] L1 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["col7"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems1 = L1;
                                String[] L2 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["col8"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems2 = L2;
                                model[0].Col18 = dt.Rows[0]["col9"].ToString();
                                model[0].Col19 = dt.Rows[0]["from_dt"].ToString();
                                model[0].Col20 = dt.Rows[0]["to_dt"].ToString();
                                model[0].Col16 = dt.Rows[0]["vch_num"].ToString();
                            }
                            break;
                    }
                    break;

                #endregion

                #region Export Incentive Due

                case "due_inc_file":
                    switch (btnval.ToUpper())
                    {

                        case "EDIT":


                            //var tm = model.FirstOrDefault();
                            //if (!Session["TR_MID"].ToString().Trim().Equals(null))
                            //{
                            mq = "select a.group_name, a.client_name,a.rec_id, master_id, a.vch_num, a.master_name,a.master_entby as ent_by," +
                                "a.master_entdate as ent_date,To_Char(a.vch_date, '" + sgen.Getsqldateformat() + "') as vch_date" +
                            ",a.section_Strength,a.classid,a.sectionid,a.Status,a.Inactive_date,a.client_id,a.client_unit_id,a.vch_num,a.type" +
                            " from master_setting a where a.client_id|| a.client_unit_id|| a.master_id|| " +
                            "To_Char(a.vch_date, 'yyyymmdd')|| a.type='" + URL + "' ";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col8 = "client_id|| client_unit_id||master_id|| To_Char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";
                                model[0].Col23 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dt.Rows[0]["vch_date"].ToString();
                                model[0].Col18 = dt.Rows[0]["master_name"].ToString();
                                model[0].Col19 = dt.Rows[0]["classid"].ToString();
                                model[0].Col21 = dt.Rows[0]["sectionid"].ToString();
                                model[0].Col20 = dt.Rows[0]["section_Strength"].ToString();
                                model[0].Col16 = dt.Rows[0]["master_id"].ToString();
                                model[0].Col3 = dt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dt.Rows[0]["ent_date"].ToString();
                                model[0].Col7 = dt.Rows[0]["rec_id"].ToString();
                                model[0].Col22 = dt.Rows[0]["status"].ToString();
                                if ((dt.Rows[0]["status"].ToString() == "")) { model[0].Col17 = "Y"; }
                                model[0].Col13 = "Update";

                            }

                            //}
                            break;
                    }
                    break;

                #endregion

                #region Export Incentive Received

                case "inc_rec":
                    switch (btnval.ToUpper())
                    {

                        case "EDIT":


                            //var tm = model.FirstOrDefault();
                            //if (!Session["TR_MID"].ToString().Trim().Equals(null))
                            //{
                            mq = "select a.group_name, a.client_name,a.rec_id, master_id, a.vch_num, a.master_name,a.master_entby as ent_by," +
                                "a.master_entdate as ent_date,To_Char(a.vch_date, '" + sgen.Getsqldateformat() + "') as vch_date" +
                            ",a.section_Strength,a.classid,a.sectionid,a.Status,a.Inactive_date,a.client_id,a.client_unit_id,a.vch_num,a.type" +
                            " from master_setting a where a.client_id|| a.client_unit_id|| a.master_id|| " +
                            "To_Char(a.vch_date, 'yyyymmdd')|| a.type='" + URL + "' ";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col8 = "client_id|| client_unit_id||master_id|| To_Char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";
                                model[0].Col23 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dt.Rows[0]["vch_date"].ToString();
                                model[0].Col18 = dt.Rows[0]["master_name"].ToString();
                                model[0].Col19 = dt.Rows[0]["classid"].ToString();
                                model[0].Col21 = dt.Rows[0]["sectionid"].ToString();
                                model[0].Col20 = dt.Rows[0]["section_Strength"].ToString();
                                model[0].Col16 = dt.Rows[0]["master_id"].ToString();
                                model[0].Col3 = dt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dt.Rows[0]["ent_date"].ToString();
                                model[0].Col7 = dt.Rows[0]["rec_id"].ToString();
                                model[0].Col22 = dt.Rows[0]["status"].ToString();
                                if ((dt.Rows[0]["status"].ToString() == "")) { model[0].Col17 = "Y"; }
                                model[0].Col13 = "Update";

                            }

                            //}
                            break;
                    }
                    break;

                #endregion

                #region Export Incentive Utilise

                case "inc_use":
                    switch (btnval.ToUpper())
                    {

                        case "EDIT":


                            //var tm = model.FirstOrDefault();
                            //if (!Session["TR_MID"].ToString().Trim().Equals(null))
                            //{
                            mq = "select a.group_name, a.client_name,a.rec_id, master_id, a.vch_num, a.master_name,a.master_entby as ent_by," +
                                "a.master_entdate as ent_date,To_Char(a.vch_date, '" + sgen.Getsqldateformat() + "') as vch_date" +
                            ",a.section_Strength,a.classid,a.sectionid,a.Status,a.Inactive_date,a.client_id,a.client_unit_id,a.vch_num,a.type" +
                            " from master_setting a where a.client_id|| a.client_unit_id|| a.master_id|| " +
                            "To_Char(a.vch_date, 'yyyymmdd')|| a.type='" + URL + "' ";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col8 = "client_id|| client_unit_id||master_id|| To_Char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";
                                model[0].Col23 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dt.Rows[0]["vch_date"].ToString();
                                model[0].Col18 = dt.Rows[0]["master_name"].ToString();
                                model[0].Col19 = dt.Rows[0]["classid"].ToString();
                                model[0].Col21 = dt.Rows[0]["sectionid"].ToString();
                                model[0].Col20 = dt.Rows[0]["section_Strength"].ToString();
                                model[0].Col16 = dt.Rows[0]["master_id"].ToString();
                                model[0].Col3 = dt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dt.Rows[0]["ent_date"].ToString();
                                model[0].Col7 = dt.Rows[0]["rec_id"].ToString();
                                model[0].Col22 = dt.Rows[0]["status"].ToString();
                                if ((dt.Rows[0]["status"].ToString() == "")) { model[0].Col17 = "Y"; }
                                model[0].Col13 = "Update";

                            }

                            //}
                            break;
                    }
                    break;

                #endregion

                #region PI link with Comm

                case "pi_comm":
                    switch (btnval.ToUpper())
                    {

                        case "EDIT":


                            //var tm = model.FirstOrDefault();
                            //if (!Session["TR_MID"].ToString().Trim().Equals(null))
                            //{
                            mq = "select a.group_name, a.client_name,a.rec_id, master_id, a.vch_num, a.master_name,a.master_entby as ent_by," +
                                "a.master_entdate as ent_date,To_Char(a.vch_date, '" + sgen.Getsqldateformat() + "') as vch_date" +
                            ",a.section_Strength,a.classid,a.sectionid,a.Status,a.Inactive_date,a.client_id,a.client_unit_id,a.vch_num,a.type" +
                            " from master_setting a where a.client_id|| a.client_unit_id|| a.master_id|| " +
                            "To_Char(a.vch_date, 'yyyymmdd')|| a.type='" + URL + "' ";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col8 = "client_id|| client_unit_id||master_id|| To_Char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";
                                model[0].Col23 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dt.Rows[0]["vch_date"].ToString();
                                model[0].Col18 = dt.Rows[0]["master_name"].ToString();
                                model[0].Col19 = dt.Rows[0]["classid"].ToString();
                                model[0].Col21 = dt.Rows[0]["sectionid"].ToString();
                                model[0].Col20 = dt.Rows[0]["section_Strength"].ToString();
                                model[0].Col16 = dt.Rows[0]["master_id"].ToString();
                                model[0].Col3 = dt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dt.Rows[0]["ent_date"].ToString();
                                model[0].Col7 = dt.Rows[0]["rec_id"].ToString();
                                model[0].Col22 = dt.Rows[0]["status"].ToString();
                                if ((dt.Rows[0]["status"].ToString() == "")) { model[0].Col17 = "Y"; }
                                model[0].Col13 = "Update";

                            }

                            //}
                            break;
                    }
                    break;

                #endregion

                #region brc

                case "brc":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select '-' as fstr, bb.vch_num bb_doc_no,bc.client_id,bc.client_unit_id," +
                                "bc.col5,bc.col4,to_char(bc.vch_date,'"+sgen.Getsqldateformat()+ "') as doc_date,bc.type,bc.vch_num,bc.col2,bc.col3" +
                                ",bc.ent_by,bc.ent_date,bb.col6 as inv_no,cm.vch_num as bnk_id,cm.REFERED_BY as Branch_Name,cm.bnkaddr as Address," +
                                "cm.cpname as Bank_Acc,a.master_name as Bank_name,bb.col8 as inv_type,bb.col9 as amc_rcpt_fc,bb.col10 as bal_fc," +
                                "bb.col11 as to_be_rcpt_fc,nvl(ci.col6, 0) as comm_inv_no," +
                                "nvl(to_char(ci.date1, '" + sgen.Getsqldateformat() + "'), 0) as com_inv_date,nvl(ci.col8, 0) C_amt_in_FC," +
                                "nvl(to_number(ci.col8) * to_number(ci.col2), 0) as ci_Amt_INR,nvl(ci.col2, 0) as ci_exchange_rate," +
                                "nvl(ci.vch_num, 0) as ci_vchnum,cr.master_name as curr,cl.vch_num c_Cust_id, cl.C_name c_Cust_Name, cl.addr as c_addr,cl.country as c_country," +
                                "bc.col7 as Lc_rec,bc.col9 as brc_no,nvl(to_char(bc.date3, '" + sgen.Getsqldateformat() + "'), 0) as brc_date," +
                                "nvl(to_char(bc.date1, '" + sgen.Getsqldateformat() + "'), 0) as from_dt," +
                                "nvl(to_char(bc.date2, '" + sgen.Getsqldateformat() + "'), 0) as to_date from enx_tab2 bc " +
                                "inner join enx_tab2 bb on bb.vch_num = bc.col17 and bb.client_unit_id = '" + unitid_mst + "' and bb.type = 'BBK' " +
                                "inner join clients_mst cm on cm.vch_num = bb.col17 and cm.client_unit_id = '" + unitid_mst + "' and cm.type = 'BAD' " +
                                "inner join master_setting a on a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = '" + unitid_mst + "' " +
                                "inner join enx_tab2 ci on ci.col6 = bb.col6 and ci.type = 'ECI' and ci.client_unit_id = '" + unitid_mst + "' " +
                                "inner join master_setting cr on(cr.master_id = ci.col7) and cr.type = 'CTM' and cr.client_unit_id = '" + unitid_mst + "' " +
                                "inner join clients_mst cl on cl.vch_num = bb.col5 and cl.type = 'BCD' and cl.client_unit_id = '" + unitid_mst + "' " +
                                "where (bc.client_id || bc.client_unit_id || bc.vch_num || to_char(bc.vch_date, 'yyyymmdd') || bc.type) = '" + URL + "'";
                           
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col12 = dt.Rows[0]["type"].ToString();
                                model[0].Col16 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dt.Rows[0]["doc_date"].ToString();
                                model[0].Col8 = "client_id|| client_unit_id || vch_num ||To_Char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";
                                model[0].Col3 = dt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dt.Rows[0]["ent_date"].ToString();
                                model[0].Col100 = "Update & New";
                                model[0].Col121 = "U";
                                model[0].Col122 = "<u>U</u>pdate";
                                model[0].Col123 = "Update & Ne<u>w</u>";
                                model[0].Col13 = "Update";
                                mq = "select a.master_name as Bank_name,cm.vch_num,cm.cpname as Bank_Acc,cm.REFERED_BY as Branch_Name,at.master_name as Account_type," +
                            "ct.master_name as currency_type,cm.bnkaddr as Address,cm.Addr as Department from clients_mst cm inner join master_setting" +
                            " a on a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id inner join master_setting at" +
                            " on at.master_id = cm.ploc and at.client_unit_id = cm.client_unit_id and at.type = 'BTM' inner join master_setting ct" +
                            " on ct.master_id = cm.panno and ct.client_unit_id = cm.client_unit_id and ct.type = 'CTM'" +
                            " where cm.type = 'BAD' and cm.client_unit_id = '" + unitid_mst + "' and cm.vch_num='" + dt.Rows[0]["col4"].ToString() + "'";

                                dtt = sgen.getdata(userCode, mq);
                                if (dtt.Rows.Count > 0)
                                {
                                    model[0].Col18 = dtt.Rows[0]["Bank_name"].ToString();
                                    model[0].Col19 = dtt.Rows[0]["vch_num"].ToString();
                                    model[0].Col20 = dtt.Rows[0]["Address"].ToString();
                                    model[0].Col21 = dtt.Rows[0]["Bank_Acc"].ToString();
                                    model[0].Col22 = dtt.Rows[0]["Account_type"].ToString();
                                }
                                //model[0].dt1 = new DataTable();
                                //model[0].dt1 = ((DataTable)sgen.GetSession(MyGuid, "BRC_DT")).Clone();
                                model[0].dt1 = model[0].dt1.Clone();
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["SNo"] = dt.Rows[j]["fstr"].ToString();
                                    dr["PartyName"] = dt.Rows[j]["c_Cust_Name"].ToString();
                                    dr["Country"] = dt.Rows[j]["c_country"].ToString();
                                    dr["CINO"] = dt.Rows[j]["comm_inv_no"].ToString();
                                    dr["CIDate"] = dt.Rows[j]["com_inv_date"].ToString();
                                    dr["FC_Amt"] = dt.Rows[j]["C_amt_in_FC"].ToString();
                                    dr["LC_Amt"] = dt.Rows[j]["ci_Amt_INR"].ToString();
                                    dr["Bank_Name"] = dt.Rows[j]["Bank_name"].ToString();
                                    dr["Bank_Acc_No"] = dt.Rows[j]["Bank_Acc"].ToString();
                                    dr["FC_Rate"] = dt.Rows[j]["ci_exchange_rate"].ToString();
                                    dr["Doc_no"] = dt.Rows[j]["bb_doc_no"].ToString();
                                    //dr["Curr_rate"] = dt.Rows[j][""].ToString();
                                    dr["FC_Rec"] = dt.Rows[j]["col2"].ToString();
                                    dr["LC_Rec"] = dt.Rows[j]["col3"].ToString();
                                    dr["brc_no"] = dt.Rows[j]["brc_no"].ToString();
                                    dr["brc_date"] = dt.Rows[j]["brc_date"].ToString();
                                    model[0].dt1.Rows.Add(dr);
                                }
                                model[0].dt1.AcceptChanges();

                            }

                            //}
                            break;

                        case "ITEM":

                            mq = "select '-' fstr,cl.country,nvl(a.master_name,'-') as Bank_name,cm.cpname as Bank_Acc," +
                                "cm.vch_num as bnk_id,cm.REFERED_BY as Branch_Name,cm.bnkaddr as Address,bb.rec_id,cl.addr,cl.vch_num Cust_id," +
                                "bb.vch_num, cl.C_name Cust_Name,nvl(ci.col8, 0) C_amt_in_FC,bb.col6 as inv_no,bb.col8 as inv_type,bb.col9 as amc_rcpt_fc," +
                                "bb.col10 as bal_fc,bb.col11 as to_be_rcpt_fc,nvl(to_number(ci.col8) * to_number(ci.col2), 0) as ci_Amt_INR," +
                                "nvl(cr.master_name,'-') as curr,nvl(ci.col2, 0) as ci_exchange_rate,nvl(ci.vch_num, 0) as ci_vchnum," +
                                "nvl(to_char(ci.date1, '" + sgen.Getsqldateformat() + "'), 0) as com_inv_date,nvl(ci.col6, 0) as comm_inv_no " +
                                "from enx_tab2 bb inner join clients_mst cm on cm.vch_num = bb.col17 and find_in_set(cm.client_unit_id,'" + unitid_mst + "')=1 and cm.type = 'BAD' " +
                                "left join master_setting a on a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = '" + unitid_mst + "' " +
                                "inner join enx_tab2 ci on ci.col6 = bb.col6 and ci.type = 'ECI' and ci.client_unit_id = '" + unitid_mst + "' " +
                                "left join clients_mst cl on cl.vch_num = bb.col5 and cl.type = 'BCD' and find_in_set(cl.client_unit_id, '" + unitid_mst + "')=1 " +
                                "left join master_setting cr on(cr.master_id = ci.col7) and cr.type = 'CTM' and cr.client_unit_id = '" + unitid_mst + "' " +
                                "where (bb.client_id || bb.client_unit_id || bb.vch_num || to_char(bb.vch_date, 'yyyymmdd') || bb.type) in ('" + URL + "')";

                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model[0].dt1 = new DataTable();
                                model[0].dt1 = ((DataTable)sgen.GetSession(MyGuid, "BRC_DT")).Clone();
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["SNo"] = dt.Rows[j]["fstr"].ToString();
                                    dr["PartyName"] = dt.Rows[j]["Cust_Name"].ToString();
                                    dr["Country"] = dt.Rows[j]["country"].ToString();
                                    dr["CINO"] = dt.Rows[j]["comm_inv_no"].ToString();
                                    dr["CIDate"] = dt.Rows[j]["com_inv_date"].ToString();
                                    dr["FC_Amt"] = dt.Rows[j]["C_amt_in_FC"].ToString();
                                    dr["LC_Amt"] = dt.Rows[j]["ci_Amt_INR"].ToString();
                                    dr["Bank_Name"] = dt.Rows[j]["Bank_name"].ToString();
                                    dr["Bank_Acc_No"] = dt.Rows[j]["Bank_Acc"].ToString();
                                    dr["FC_Rate"] = dt.Rows[j]["ci_exchange_rate"].ToString();
                                    dr["Doc_no"] = dt.Rows[j]["vch_num"].ToString();
                                    dr["FC_Rec"] = dt.Rows[j]["amc_rcpt_fc"].ToString();
                                    model[0].dt1.Rows.Add(dr);
                                }
                                model[0].dt1.AcceptChanges();
                            }
                            break;


                        case "BANK":

                            mq = "select a.master_name as Bank_name,a.master_id as bnk_name_id,cm.bnk,cm.vch_num,cm.cpname as Bank_Acc,cm.REFERED_BY as Branch_Name,at.master_name as Account_type," +
                                "ct.master_name as currency_type,ct.master_id as cur_id,cm.bnkaddr as Address,cm.Addr as Department from clients_mst cm inner join master_setting a on " +
                                "a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id inner join master_setting at on" +
                                " at.master_id = cm.ploc and at.client_unit_id = cm.client_unit_id and at.type = 'BTM' inner join master_setting ct " +
                                "on ct.master_id = cm.panno and ct.client_unit_id = cm.client_unit_id and ct.type = 'CTM'" +
                                " where (cm.client_id || cm.client_unit_id || cm.vch_num || to_char(cm.vch_date, 'yyyymmdd') || cm.type) = '" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model[0].Col18 = dt.Rows[0]["Bank_name"].ToString();
                                model[0].Col19 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col20 = dt.Rows[0]["Address"].ToString();
                                model[0].Col21 = dt.Rows[0]["Bank_Acc"].ToString();
                                model[0].Col22 = dt.Rows[0]["Account_type"].ToString();
                            }
                            break;
                    }
                    break;

                #endregion

                #region book_fwd

                case "book_fwd":
                    switch (btnval.ToUpper())
                    {

                        case "EDIT":
                            #region Currency Type 2
                            TempData[MyGuid + "_Tlist1"] = model[0].TList1 = mod1 = cmd_Fun.fwdcurname(userCode, unitid_mst);
                            #endregion
                            #region Nature of Transaction
                            mod2.Add(new SelectListItem { Text = "SALE", Value = "SALE" });
                            mod2.Add(new SelectListItem { Text = "BUY", Value = "BUY" });
                            TempData[MyGuid + "_Tlist2"] = model[0].TList2 = mod2;
                            #endregion
                            mq = "select bk.col11 as f_amt,bk.col13,bk.col9,bk.col41,bk.col8,bk.col10,cm.REFERED_BY as Branch_Name,bk.vch_num,bk.ent_by,bk.ent_date" +
                                ",bk.col2,a.master_name as Bank_name,bk.col12,bk.col1," +
                                "to_char(bk.date2, '" + sgen.Getsqldateformat() + "') as col3," +
                                "to_char(bk.date3, '" + sgen.Getsqldateformat() + "') as col4," +
                                "to_char(bk.date4, '" + sgen.Getsqldateformat() + "') as col7, " +
                                "to_char(bk.date5, '" + sgen.Getsqldateformat() + "') as col11 " +
                                "from vehicle_master bk inner join clients_mst cm on cm.vch_num = bk.col1 and cm.type = 'BAD' and cm.client_unit_id = '" + unitid_mst + "' " +
                                "inner join master_setting a on a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id " +
                                "where(bk.client_id || bk.client_unit_id || bk.vch_num || to_char(bk.vch_date, 'yyyymmdd') || bk.type) = '" + URL + "' ";

                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col16 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col8 = "client_id|| client_unit_id || vch_num ||To_Char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";
                                model[0].Col49 = dt.Rows[0]["col1"].ToString();
                                model[0].Col18 = dt.Rows[0]["col3"].ToString();              //Forward Start Date
                                model[0].Col19 = dt.Rows[0]["col4"].ToString();              //Forward end Date
                                model[0].Col21 = dt.Rows[0]["col7"].ToString();              //Forward Grace End
                                model[0].Col22 = dt.Rows[0]["f_amt"].ToString();            //Forward Amount
                                model[0].Col23 = dt.Rows[0]["col9"].ToString();           //Spot Rate
                                model[0].Col24 = dt.Rows[0]["col8"].ToString();           //Premium (+) / Discount (-)
                                model[0].Col27 = dt.Rows[0]["col10"].ToString();          //Forward No.
                                model[0].Col28 = dt.Rows[0]["col11"].ToString();          //Forward Date
                                model[0].Col30 = dt.Rows[0]["col13"].ToString();          //purpose
                                model[0].Col20 = dt.Rows[0]["Bank_name"].ToString();
                                model[0].Col29 = dt.Rows[0]["Branch_Name"].ToString();
                                model[0].Col3 = dt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dt.Rows[0]["ent_date"].ToString();
                                model[0].Col31 = dt.Rows[0]["col41"].ToString();
                                model[0].TList1 = mod1;
                                model[0].TList2 = mod2;
                                String[] L1 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["col12"].ToString()).Distinct()).Split(',');
                                String[] L2 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["col2"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems1 = L1;    //Currency
                                model[0].SelectedItems2 = L2;     //Nature of Transactio
                                model[0].Col13 = "Update";
                                model[0].Col100 = "Update & New";
                                model[0].Col121 = "U";
                                model[0].Col122 = "<u>U</u>pdate";
                                model[0].Col123 = "Update & Ne<u>w</u>";
                            }
                            break;
                        case "NEW":

                            mq = "select cm.cpname as Bank_Acc,cm.REFERED_BY as Branch_Name,a.master_name as Bank_name,cm.bnkaddr as Address,cm.vch_num from " +
                                  "clients_mst cm inner join master_setting bt on bt.master_id = cm.ploc and bt.type = 'BTM' and bt.client_unit_id = cm.client_unit_id " +
                                  "inner join master_setting a on a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id where " +
                                  "(cm.client_id || cm.client_unit_id || cm.vch_num || to_char(cm.vch_date, 'yyyymmdd') || cm.type) = '" + URL + "'";

                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model[0].Col20 = dt.Rows[0]["Bank_name"].ToString();
                                model[0].Col49 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col29 = dt.Rows[0]["Branch_Name"].ToString();
                            }
                            model = getnew(model);
                            model[0].Col23 = "0";
                            model[0].Col24 = "0";
                            model[0].Col22 = "0";
                            #region Dropdwon
                            mod1 = new List<SelectListItem>();
                            TempData[MyGuid + "_TList1"] = model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
                            if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };

                            mod2 = new List<SelectListItem>();
                            TempData[MyGuid + "_TList2"] = model[0].TList2 = (List<SelectListItem>)TempData[MyGuid + "_TList2"];
                            if (tm.SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };

                            TempData[MyGuid + "_Tlist1"] = model[0].TList1 = mod1 = cmd_Fun.fwdcurname(userCode, unitid_mst);    //Currency Type 2

                            mod2.Add(new SelectListItem { Text = "SALE", Value = "SALE" });    //Nature of Transaction
                            mod2.Add(new SelectListItem { Text = "BUY", Value = "BUY" });
                            TempData[MyGuid + "_TList2"] = model[0].TList2 = mod2;
                            #endregion
                            break;
                    }
                    break;

                #endregion

                #region bulk_pi

                case "bulk_pi":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select pi.vch_num as Doc_Number,pi.col7 as PI_no,cl.c_name as cust_name,cl.addr,pi.col9 as pi_amt_fc," +
                                "to_char(convert_tz(pi.vch_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') vch_date,cr.master_name " +
                                "as Currency,cl.country,pi.col6,to_char(pi.date1, '" + sgen.Getsqldateformat() + "') as PI_date," +
                                "pi.col10 as exchange_rate,pi.col5,pi.rec_id,pi.client_unit_id,pi.client_id, " +
                                "pi.edit_date,pi.edit_by, pi.ent_date,pi.ent_by,cl.vch_num as cl_id from enx_tab2 pi inner join master_setting cr " +
                                "on cr.master_id = pi.col6 and cr.type = 'CTM' and cr.client_unit_id = '" + unitid_mst + "'" +
                                " inner join clients_mst cl on cl.vch_num = pi.col5 and cl.type = 'BCD' and substr(cl.vch_num,0,3)='303' and find_in_set(cl.client_unit_id, '" + unitid_mst + "')=1 " +
                                "where (pi.client_id || pi.client_unit_id || pi.vch_num || to_char(pi.vch_date, 'yyyymmdd') || pi.type) = '" + URL + "'";

                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                model = getedit(model, dtt, URL);
                                #region Currency

                                TempData[MyGuid + "_Tlist1"] = model[0].TList1 = mod1 = cmd_Fun.curname(userCode, unitid_mst);
                                #endregion
                                model[0].Col16 = dtt.Rows[0]["Doc_Number"].ToString();
                                model[0].Col17 = dtt.Rows[0]["col5"].ToString();
                                model[0].Col26 = dtt.Rows[0]["vch_date"].ToString();
                                model[0].Col21 = dtt.Rows[0]["PI_no"].ToString();
                                model[0].Col22 = dtt.Rows[0]["PI_date"].ToString();
                                model[0].Col23 = dtt.Rows[0]["pi_amt_fc"].ToString();
                                model[0].Col24 = dtt.Rows[0]["exchange_rate"].ToString();
                                model[0].Col20 = dtt.Rows[0]["cust_name"].ToString();
                                model[0].Col18 = dtt.Rows[0]["addr"].ToString();
                                model[0].Col19 = dtt.Rows[0]["country"].ToString();
                                model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                                model[0].Col25 = (sgen.Make_decimal(dtt.Rows[0]["pi_amt_fc"].ToString()) * sgen.Make_decimal(dtt.Rows[0]["exchange_rate"].ToString())).ToString();
                                String[] L1 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["col6"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems1 = L1;
                            }
                            break;
                        case "PARTY":
                            mq = "select a.c_name as name,a.c_gstin as gstin,a.addr,a.country,a.vch_num,a.tor from clients_mst a where a.vch_num||To_Char(a.vch_date, 'yyyymmdd')|| a.type = '" + URL + "'";

                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                model[0].Col17 = dtt.Rows[0]["vch_num"].ToString();
                                model[0].Col20 = dtt.Rows[0]["name"].ToString();
                                model[0].Col18 = dtt.Rows[0]["addr"].ToString();
                                model[0].Col19 = dtt.Rows[0]["country"].ToString();
                            }
                            break;
                    }
                    break;

                #endregion

                #region Advance Against PI - PAYMENT RECEIVED

                case "adv_pi":
                    switch (btnval.ToUpper())
                    {

                        case "EDIT":


                            //var tm = model.FirstOrDefault();
                            //if (!Session["TR_MID"].ToString().Trim().Equals(null))
                            //{
                            mq = "select a.group_name, a.client_name,a.rec_id, master_id, a.vch_num, a.master_name,a.master_entby as ent_by," +
                                "a.master_entdate as ent_date,To_Char(a.vch_date, '" + sgen.Getsqldateformat() + "') as vch_date" +
                            ",a.section_Strength,a.classid,a.sectionid,a.Status,a.Inactive_date,a.client_id,a.client_unit_id,a.vch_num,a.type" +
                            " from master_setting a where a.client_id|| a.client_unit_id|| a.master_id|| " +
                            "To_Char(a.vch_date, 'yyyymmdd')|| a.type='" + URL + "' ";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col8 = "client_id|| client_unit_id||master_id|| To_Char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";
                                model[0].Col23 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dt.Rows[0]["vch_date"].ToString();
                                model[0].Col18 = dt.Rows[0]["master_name"].ToString();
                                model[0].Col19 = dt.Rows[0]["classid"].ToString();
                                model[0].Col21 = dt.Rows[0]["sectionid"].ToString();
                                model[0].Col20 = dt.Rows[0]["section_Strength"].ToString();
                                model[0].Col16 = dt.Rows[0]["master_id"].ToString();
                                model[0].Col3 = dt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dt.Rows[0]["ent_date"].ToString();
                                model[0].Col7 = dt.Rows[0]["rec_id"].ToString();
                                model[0].Col22 = dt.Rows[0]["status"].ToString();
                                if ((dt.Rows[0]["status"].ToString() == "")) { model[0].Col17 = "Y"; }
                                model[0].Col13 = "Update";

                            }

                            //}
                            break;
                    }
                    break;

                #endregion

                #region Finance Against CI

                case "dis_ci":
                    switch (btnval.ToUpper())
                    {

                        case "EDIT":


                            //var tm = model.FirstOrDefault();
                            //if (!Session["TR_MID"].ToString().Trim().Equals(null))
                            //{
                            mq = "select a.group_name, a.client_name,a.rec_id, master_id, a.vch_num, a.master_name,a.master_entby as ent_by," +
                                "a.master_entdate as ent_date,To_Char(a.vch_date, '" + sgen.Getsqldateformat() + "') as vch_date" +
                            ",a.section_Strength,a.classid,a.sectionid,a.Status,a.Inactive_date,a.client_id,a.client_unit_id,a.vch_num,a.type" +
                            " from master_setting a where a.client_id|| a.client_unit_id|| a.master_id|| " +
                            "To_Char(a.vch_date, 'yyyymmdd')|| a.type='" + URL + "' ";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col8 = "client_id|| client_unit_id||master_id|| To_Char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";
                                model[0].Col23 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dt.Rows[0]["vch_date"].ToString();
                                model[0].Col18 = dt.Rows[0]["master_name"].ToString();
                                model[0].Col19 = dt.Rows[0]["classid"].ToString();
                                model[0].Col21 = dt.Rows[0]["sectionid"].ToString();
                                model[0].Col20 = dt.Rows[0]["section_Strength"].ToString();
                                model[0].Col16 = dt.Rows[0]["master_id"].ToString();
                                model[0].Col3 = dt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dt.Rows[0]["ent_date"].ToString();
                                model[0].Col7 = dt.Rows[0]["rec_id"].ToString();
                                model[0].Col22 = dt.Rows[0]["status"].ToString();
                                if ((dt.Rows[0]["status"].ToString() == "")) { model[0].Col17 = "Y"; }
                                model[0].Col13 = "Update";

                            }

                            //}
                            break;
                    }
                    break;

                #endregion

                #region SHIPPING BILL FILE WITH BANK

                case "sb_bank":
                    switch (btnval.ToUpper())
                    {

                        case "EDIT":


                            //var tm = model.FirstOrDefault();
                            //if (!Session["TR_MID"].ToString().Trim().Equals(null))
                            //{
                            mq = "select a.group_name, a.client_name,a.rec_id, master_id, a.vch_num, a.master_name,a.master_entby as ent_by," +
                                "a.master_entdate as ent_date,To_Char(a.vch_date, '" + sgen.Getsqldateformat() + "') as vch_date" +
                            ",a.section_Strength,a.classid,a.sectionid,a.Status,a.Inactive_date,a.client_id,a.client_unit_id,a.vch_num,a.type" +
                            " from master_setting a where a.client_id|| a.client_unit_id|| a.master_id|| " +
                            "To_Char(a.vch_date, 'yyyymmdd')|| a.type='" + URL + "' ";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col8 = "client_id|| client_unit_id||master_id|| To_Char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";
                                model[0].Col23 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dt.Rows[0]["vch_date"].ToString();
                                model[0].Col18 = dt.Rows[0]["master_name"].ToString();
                                model[0].Col19 = dt.Rows[0]["classid"].ToString();
                                model[0].Col21 = dt.Rows[0]["sectionid"].ToString();
                                model[0].Col20 = dt.Rows[0]["section_Strength"].ToString();
                                model[0].Col16 = dt.Rows[0]["master_id"].ToString();
                                model[0].Col3 = dt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dt.Rows[0]["ent_date"].ToString();
                                model[0].Col7 = dt.Rows[0]["rec_id"].ToString();
                                model[0].Col22 = dt.Rows[0]["status"].ToString();
                                if ((dt.Rows[0]["status"].ToString() == "")) { model[0].Col17 = "Y"; }
                                model[0].Col13 = "Update";

                            }

                            //}
                            break;
                    }
                    break;

                #endregion

                #region DIRECT COLLECTION FROM CUSTOMER - PAYMENT RECEIVED

                case "dir_coll":
                    switch (btnval.ToUpper())
                    {

                        case "EDIT":


                            //var tm = model.FirstOrDefault();
                            //if (!Session["TR_MID"].ToString().Trim().Equals(null))
                            //{
                            mq = "select a.group_name, a.client_name,a.rec_id, master_id, a.vch_num, a.master_name,a.master_entby as ent_by," +
                                "a.master_entdate as ent_date,To_Char(a.vch_date, '" + sgen.Getsqldateformat() + "') as vch_date" +
                            ",a.section_Strength,a.classid,a.sectionid,a.Status,a.Inactive_date,a.client_id,a.client_unit_id,a.vch_num,a.type" +
                            " from master_setting a where a.client_id|| a.client_unit_id|| a.master_id|| " +
                            "To_Char(a.vch_date, 'yyyymmdd')|| a.type='" + URL + "' ";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col8 = "client_id|| client_unit_id||master_id|| To_Char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";
                                model[0].Col23 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dt.Rows[0]["vch_date"].ToString();
                                model[0].Col18 = dt.Rows[0]["master_name"].ToString();
                                model[0].Col19 = dt.Rows[0]["classid"].ToString();
                                model[0].Col21 = dt.Rows[0]["sectionid"].ToString();
                                model[0].Col20 = dt.Rows[0]["section_Strength"].ToString();
                                model[0].Col16 = dt.Rows[0]["master_id"].ToString();
                                model[0].Col3 = dt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dt.Rows[0]["ent_date"].ToString();
                                model[0].Col7 = dt.Rows[0]["rec_id"].ToString();
                                model[0].Col22 = dt.Rows[0]["status"].ToString();
                                if ((dt.Rows[0]["status"].ToString() == "")) { model[0].Col17 = "Y"; }
                                model[0].Col13 = "Update";

                            }

                            //}
                            break;
                    }
                    break;

                #endregion

                #region DOC SUBMITTED TO BANK FOR COLLECTION FROM CUSTOMER

                case "bank_coll":
                    switch (btnval.ToUpper())
                    {

                        case "EDIT":


                            //var tm = model.FirstOrDefault();
                            //if (!Session["TR_MID"].ToString().Trim().Equals(null))
                            //{
                            mq = "select a.group_name, a.client_name,a.rec_id, master_id, a.vch_num, a.master_name,a.master_entby as ent_by," +
                                "a.master_entdate as ent_date,To_Char(a.vch_date, '" + sgen.Getsqldateformat() + "') as vch_date" +
                            ",a.section_Strength,a.classid,a.sectionid,a.Status,a.Inactive_date,a.client_id,a.client_unit_id,a.vch_num,a.type" +
                            " from master_setting a where a.client_id|| a.client_unit_id|| a.master_id|| " +
                            "To_Char(a.vch_date, 'yyyymmdd')|| a.type='" + URL + "' ";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col8 = "client_id|| client_unit_id||master_id|| To_Char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";
                                model[0].Col23 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dt.Rows[0]["vch_date"].ToString();
                                model[0].Col18 = dt.Rows[0]["master_name"].ToString();
                                model[0].Col19 = dt.Rows[0]["classid"].ToString();
                                model[0].Col21 = dt.Rows[0]["sectionid"].ToString();
                                model[0].Col20 = dt.Rows[0]["section_Strength"].ToString();
                                model[0].Col16 = dt.Rows[0]["master_id"].ToString();
                                model[0].Col3 = dt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dt.Rows[0]["ent_date"].ToString();
                                model[0].Col7 = dt.Rows[0]["rec_id"].ToString();
                                model[0].Col22 = dt.Rows[0]["status"].ToString();
                                if ((dt.Rows[0]["status"].ToString() == "")) { model[0].Col17 = "Y"; }
                                model[0].Col13 = "Update";

                            }

                            //}
                            break;
                    }
                    break;

                #endregion

                #region PAYMENT RECEIVED IN BANK 

                case "bank_rec":
                    switch (btnval.ToUpper())
                    {

                        case "EDIT":


                            //var tm = model.FirstOrDefault();
                            //if (!Session["TR_MID"].ToString().Trim().Equals(null))
                            //{
                            mq = "select a.group_name, a.client_name,a.rec_id, master_id, a.vch_num, a.master_name,a.master_entby as ent_by," +
                                "a.master_entdate as ent_date,To_Char(a.vch_date, '" + sgen.Getsqldateformat() + "') as vch_date" +
                            ",a.section_Strength,a.classid,a.sectionid,a.Status,a.Inactive_date,a.client_id,a.client_unit_id,a.vch_num,a.type" +
                            " from master_setting a where a.client_id|| a.client_unit_id|| a.master_id|| " +
                            "To_Char(a.vch_date, 'yyyymmdd')|| a.type='" + URL + "' ";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col8 = "client_id|| client_unit_id||master_id|| To_Char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";
                                model[0].Col23 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dt.Rows[0]["vch_date"].ToString();
                                model[0].Col18 = dt.Rows[0]["master_name"].ToString();
                                model[0].Col19 = dt.Rows[0]["classid"].ToString();
                                model[0].Col21 = dt.Rows[0]["sectionid"].ToString();
                                model[0].Col20 = dt.Rows[0]["section_Strength"].ToString();
                                model[0].Col16 = dt.Rows[0]["master_id"].ToString();
                                model[0].Col3 = dt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dt.Rows[0]["ent_date"].ToString();
                                model[0].Col7 = dt.Rows[0]["rec_id"].ToString();
                                model[0].Col22 = dt.Rows[0]["status"].ToString();
                                if ((dt.Rows[0]["status"].ToString() == "")) { model[0].Col17 = "Y"; }
                                model[0].Col13 = "Update";

                            }

                            //}
                            break;
                    }
                    break;

                #endregion

                #region Use_fwd

                case "use_fwd":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select '-' as fstr, bk.col11 as Forward_Amount,uf.ent_by,uf.ent_date,uf.type,uf.vch_num,cm.cpname as Bank_Acc,bt.master_name as Acc_Type,a.master_name as " +
                                "Bank_name,cm.REFERED_BY as Branch_Name,nvl((bk.col11-uf.col13),0) as Bal_amt,nvl(uf.col13,0) as used_amt,cm.vch_num as bnk_vchnum,bk.col9 as spot_rate,uf.col1 as bnk_id,uf.col4 as u_fwd_no,bk.col8 as Premium_Discount," +
                                "bk.col10 as Forward_no,uf.col13 as fwd_to_be,to_char(uf.date2, '" + sgen.Getsqldateformat() + "') as use_date," +
                                "(bk.col6 + bk.col8) AS FWD_Rate,((bk.col9 + bk.col8) * bk.col11) AS LOCAL_CUR, bk.col2 as nature_of_transaction," +
                                "to_char(bk.date2, '" + sgen.Getsqldateformat() + "') as Forward_Start_Date," +
                                "to_char(bk.date3, '" + sgen.Getsqldateformat() + "') as Forward_end_Date," +
                                "to_char(bk.date4, '" + sgen.Getsqldateformat() + "') as Forward_grace_end,cur.master_name as fwd_cur," +
                                "to_char(bk.date5, '" + sgen.Getsqldateformat() + "') as fwd_date,uf.col31 as Remark from vehicle_master bk " +
                                "inner join vehicle_master uf on uf.col4 = bk.col10 and uf.type = 'UFW' and " +
                                "uf.client_unit_id = bk.client_unit_id inner join clients_mst cm on cm.vch_num = uf.col1 and cm.type = 'BAD' " +
                                "and cm.client_unit_id = '" + unitid_mst + "' inner join master_setting bt on " +
                                "bt.master_id = cm.ploc and bt.type = 'BTM' and bt.client_unit_id = cm.client_unit_id inner join master_setting a " +
                                "on a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id " +
                                "inner join master_setting cur on cur.master_id = bk.col12 and cur.type = 'FCM' and cur.client_unit_id = bk.client_unit_id where " +
                                "(uf.client_id || uf.client_unit_id || uf.vch_num || to_char(uf.vch_date, 'yyyymmdd') || uf.type) = '" + URL + "'";

                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col12 = dt.Rows[0]["type"].ToString();
                                model[0].Col16 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col8 = "client_id|| client_unit_id || vch_num ||To_Char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";
                                model[0].Col20 = dt.Rows[0]["Bank_name"].ToString();
                                model[0].Col52 = dt.Rows[0]["bnk_vchnum"].ToString();
                                model[0].Col17 = dt.Rows[0]["Branch_Name"].ToString();
                                model[0].Col18 = dt.Rows[0]["Acc_Type"].ToString();
                                model[0].Col19 = dt.Rows[0]["Bank_Acc"].ToString();
                                model[0].Col3 = dt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dt.Rows[0]["ent_date"].ToString();
                                model[0].Col22 = dt.Rows[0]["use_date"].ToString();
                                model[0].Col13 = "Update";
                                model[0].Col100 = "Update & New";
                                model[0].Col121 = "U";
                                model[0].Col122 = "<u>U</u>pdate";
                                model[0].Col123 = "Update & Ne<u>w</u>";
                                model[0].dt1 = new DataTable();
                                model[0].dt1 = ((DataTable)sgen.GetSession(MyGuid, "BUF_DT")).Clone();

                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    //GridViewRows         //TableRows
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["Head"] = dt.Rows[i]["fstr"].ToString();
                                    dr["SNo"] = i + 1;
                                    dr["Fwd_To_be_use"] = dt.Rows[i]["fwd_to_be"].ToString();
                                    dr["Forward_no"] = dt.Rows[i]["Forward_no"].ToString();
                                    dr["Forward_date"] = dt.Rows[i]["Forward_Start_Date"].ToString();
                                    dr["FWD_Book_Amt"] = dt.Rows[i]["Forward_Amount"].ToString();
                                    dr["FWD_Curr"] = dt.Rows[i]["fwd_cur"].ToString();
                                    dr["FWD_Book_Date"] = dt.Rows[i]["fwd_date"].ToString();
                                    dr["FWD_Exp_Date"] = dt.Rows[i]["Forward_end_Date"].ToString();
                                    dr["Spot_Rate"] = dt.Rows[i]["spot_rate"].ToString();
                                    dr["Premium"] = dt.Rows[i]["Premium_Discount"].ToString();
                                    dr["Fwd_Rate"] = dt.Rows[i]["FWD_Rate"].ToString();
                                    dr["Remark"] = dt.Rows[i]["Remark"].ToString();
                                    dr["Fwd_used_Amt"] = dt.Rows[i]["used_amt"].ToString();
                                    dr["Fwd_Bal_Amt"] = dt.Rows[i]["Bal_amt"].ToString();
                                    dr["Fwd_To_be_use"] = dt.Rows[i]["used_amt"].ToString();
                                    model[0].dt1.Rows.Add(dr);
                                }
                                model[0].dt1.AcceptChanges();

                            }

                            break;

                        case "NEW":
                            mq = "select cm.cpname as Bank_Acc,cm.REFERED_BY as Branch_Name,a.master_name as Bank_name,cm.bnkaddr as Address,cm.msmeno as IFSC," +
                                "bt.master_name as Acc_Type,cm.vch_num from clients_mst cm inner join master_setting bt on bt.master_id = cm.ploc and " +
                                "bt.type = 'BTM' and bt.client_unit_id = cm.client_unit_id inner join master_setting a on a.master_id = cm.ptype and " +
                                "a.type = 'ABD' and a.client_unit_id = cm.client_unit_id " +
                                "where (cm.client_id || cm.client_unit_id || cm.vch_num || to_char(cm.vch_date, 'yyyymmdd') || cm.type) = '" + URL + "'";

                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model[0].Col20 = dt.Rows[0]["Bank_name"].ToString();
                                model[0].Col52 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dt.Rows[0]["Branch_Name"].ToString();
                                model[0].Col18 = dt.Rows[0]["Acc_Type"].ToString();
                                model[0].Col19 = dt.Rows[0]["Bank_Acc"].ToString();
                            }
                            model = getnew(model);
                            sgen.SetSession(MyGuid, "Bank_id", model[0].Col52);
                            model[0].dt1 = new DataTable();
                            model[0].dt1 = ((DataTable)sgen.GetSession(MyGuid, "BUF_DT"));

                            Make_query(viewName.ToLower(), "Select Forward", "BOOKFWD", "2", "", "", MyGuid);
                            ViewBag.scripCall = "callFoo('Select Forward');";

                            break;

                        case "BOOKFWD":

                            mq = "select '-' as fstr, bk.col11 as Forward_Amount,bk.col9 as spot_rate,nvl(uf.used_amt,0) as used_amt,bk.col8 as Premium_Discount,(bk.col11-nvl(uf.used_amt,0)) as bal_amt," +
                                "bk.col10 as Forward_no,(bk.col9 + bk.col8) AS FWD_Rate,((bk.col9 + bk.col8) * bk.col11) AS LOCAL_CUR,   bk.col2 as nature_of_transaction," +
                                "to_char(bk.date2, '" + sgen.Getsqldateformat() + "') as Forward_Start_Date," +
                                "to_char(bk.date3, '" + sgen.Getsqldateformat() + "') as Forward_end_Date," +
                                "to_char(bk.date4, '" + sgen.Getsqldateformat() + "') as Forward_grace_end," +
                                "cur.master_name as fwd_cur," +
                                "to_char(bk.date5, '" + sgen.Getsqldateformat() + "') as fwd_date from vehicle_master bk " +
                                "left join (select sum(col13) as used_amt ,col4 from vehicle_master where client_unit_id = '" + unitid_mst + "' and type = 'UFW' group by col4)uf on uf.col4 = bk.col10 left join master_setting cur on " +
                                "cur.master_id = bk.col12 and cur.type = 'FCM' and cur.client_unit_id = bk.client_unit_id " +
                                "where (bk.client_id || bk.client_unit_id || bk.vch_num || to_char(bk.vch_date, 'yyyymmdd') || bk.type) in ('" + URL + "')";

                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                if (model[0].dt1.Rows[0]["Forward_no"].ToString().Trim().Equals("-")) { model[0].dt1.Rows.RemoveAt(0); }
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["Head"] = dt.Rows[i]["fstr"].ToString();
                                    dr["SNo"] = i + 1;
                                    dr["Forward_no"] = dt.Rows[i]["Forward_no"].ToString();
                                    dr["Forward_date"] = dt.Rows[i]["Forward_Start_Date"].ToString();
                                    dr["FWD_Book_Amt"] = dt.Rows[i]["Forward_Amount"].ToString();
                                    dr["FWD_Curr"] = dt.Rows[i]["fwd_cur"].ToString();
                                    dr["FWD_Book_Date"] = dt.Rows[i]["fwd_date"].ToString();
                                    dr["FWD_Exp_Date"] = dt.Rows[i]["Forward_end_Date"].ToString();
                                    dr["Spot_Rate"] = dt.Rows[i]["spot_rate"].ToString();
                                    dr["Premium"] = dt.Rows[i]["Premium_Discount"].ToString();
                                    dr["Fwd_Rate"] = dt.Rows[i]["FWD_Rate"].ToString();
                                    dr["Fwd_used_Amt"] = dt.Rows[i]["used_amt"].ToString();
                                    dr["Fwd_Bal_Amt"] = dt.Rows[i]["bal_amt"].ToString();
                                    model[0].dt1.Rows.Add(dr);
                                }
                                model[0].dt1.AcceptChanges();
                            }
                            break;
                    }
                    break;
                #endregion

                #region Shipping bill

                case "sb":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type as fstr,to_char(vch_date,'" + sgen.Getsqldateformat() + "') " +
                                "as doc_date,to_char(date1,'" + sgen.Getsqldateformat() + "') as sb_date,to_char(date2,'" + sgen.Getsqldateformat() + "') as leo_date,a.*,ci.* from kc_tab a inner join " +
                                "(select * from (select com.client_unit_id,com.col6 as ci_no,com.col8 ci_amt_fc, to_char(com.date1, '" + sgen.Getsqldateformat() + "') as ci_date" +
                                ",cl.vch_num as cust_id,cl.c_name as cust_name,cl.country,com.col2 as exc_rate,to_number(com.col8) * to_number(com.col2) as" +
                                " Amt_in_NR from enx_tab2 com inner join clients_mst cl on cl.vch_num = com.col5 and cl.type = 'BCD' and " +
                                "find_in_set(cl.client_unit_id, '" + unitid_mst + "')= 1 where com.type = 'ECI' and com.client_unit_id = '" + unitid_mst + "')) ci on " +
                                "ci.ci_no = a.col5 and ci.client_unit_id = a.client_unit_id where a.client_id || a.client_unit_id || a.vch_num ||" +
                                " to_char(a.vch_date, 'yyyymmdd') || a.type = '" + URL + "'";

                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model = getedit(model, dt, URL);
                                model[0].Col16 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dt.Rows[0]["doc_date"].ToString();

                                mq = "select a.master_name as Bank_name,cm.vch_num,cm.cpname as Bank_Acc,cm.REFERED_BY as Branch_Name,at.master_name as Account_type," +
                              "ct.master_name as currency_type,cm.bnkaddr as Address,cm.Addr as Department from clients_mst cm inner join master_setting" +
                              " a on a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id inner join master_setting at" +
                              " on at.master_id = cm.ploc and at.client_unit_id = cm.client_unit_id and at.type = 'BTM' inner join master_setting ct" +
                              " on ct.master_id = cm.panno and ct.client_unit_id = cm.client_unit_id and ct.type = 'CTM'" +
                              " where cm.type = 'BAD' and cm.client_unit_id = '" + unitid_mst + "' and cm.vch_num='"+ dt.Rows[0]["col4"].ToString() + "'";

                                dtt = sgen.getdata(userCode, mq);
                                if (dtt.Rows.Count > 0)
                                {
                                model[0].Col18 = dtt.Rows[0]["Bank_name"].ToString();
                                model[0].Col19 = dtt.Rows[0]["vch_num"].ToString();
                                model[0].Col20 = dtt.Rows[0]["Address"].ToString();
                                }
                                model[0].dt1 = model[0].dt1.Clone();
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["SNo"] = j + 1;
                                    dr["Add_Remove"] = dt.Rows[j]["fstr"].ToString();
                                    dr["CI_No"] = dt.Rows[j]["ci_no"].ToString();
                                    dr["CI_Date"] = dt.Rows[j]["ci_date"].ToString();
                                    dr["Custome_name"] = dt.Rows[j]["cust_name"].ToString();
                                    dr["Country"] = dt.Rows[j]["country"].ToString();
                                    dr["CI_FC_Amt"] = dt.Rows[j]["ci_amt_fc"].ToString();
                                    dr["SB_No"] = dt.Rows[j]["col1"].ToString();
                                    dr["SB_Date"] = dt.Rows[j]["sb_date"].ToString();
                                    dr["LEO_Date"] = dt.Rows[j]["leo_date"].ToString();
                                    dr["shipment_terms"] = dt.Rows[j]["col22"].ToString(); 
                                    dr["shipment_country"] = dt.Rows[j]["col21"].ToString();
                                    dr["ship_Port"] = dt.Rows[j]["col23"].ToString();
                                    dr["Remark"] = dt.Rows[j]["col31"].ToString();
                                    model[0].dt1.Rows.Add(dr);
                                }
                              
                            }

                            break;
                        case "BANK":

                            mq = "select a.master_name as Bank_name,a.master_id as bnk_name_id,cm.bnk,cm.vch_num,cm.cpname as Bank_Acc,cm.REFERED_BY as Branch_Name,at.master_name as Account_type," +
                                "ct.master_name as currency_type,ct.master_id as cur_id,cm.bnkaddr as Address,cm.Addr as Department from clients_mst cm inner join master_setting a on " +
                                "a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id inner join master_setting at on" +
                                " at.master_id = cm.ploc and at.client_unit_id = cm.client_unit_id and at.type = 'BTM' inner join master_setting ct " +
                                "on ct.master_id = cm.panno and ct.client_unit_id = cm.client_unit_id and ct.type = 'CTM'" +
                                " where (cm.client_id || cm.client_unit_id || cm.vch_num || to_char(cm.vch_date, 'yyyymmdd') || cm.type) = '" + URL + "'";

                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model[0].Col18 = dt.Rows[0]["Bank_name"].ToString();
                                model[0].Col19 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col20 = dt.Rows[0]["Address"].ToString();
                            }
                            break;
                        case "CI":
                            mq = " select (com.client_id || com.client_unit_id || com.vch_num || to_char(com.vch_date, 'yyyymmdd') || com.type) as fstr" +
                                 ",com.col6 as ci_no,com.col8 ci_amt_fc,to_char(com.date1, '" + sgen.Getsqldateformat() + "') as ci_date,com.col8 - nvl(bb.used_amt, 0) as bal_amt,cl.vch_num as cust_id,cl.c_name as cust_name" +
                                 ",cl.country,com.col2 as exc_rate,to_number(com.col8) * to_number(com.col2) as Amt_in_NR from enx_tab2 com inner join clients_mst" +
                                 " cl on cl.vch_num = com.col5 and cl.type = 'BCD' and find_in_set(cl.client_unit_id, '" + unitid_mst + "')=1 left join (select sum(col11) as used_amt ,col6 from enx_tab2 where " +
                                 "client_unit_id = '" + unitid_mst + "' and type = 'BBK' group by col6) bb on bb.col6 = com.col6  where (com.client_id || com.client_unit_id || com.vch_num || to_char(com.vch_date, 'yyyymmdd') || com.type) in ('" + URL + "')";

                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                if (model[0].dt1.Rows[0]["CI_NO"].ToString().Trim().Equals("-")) { model[0].dt1.Rows.RemoveAt(0); }
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["SNo"] = j + 1;
                                    dr["Add_Remove"] = dt.Rows[j]["fstr"].ToString();
                                    dr["CI_No"] = dt.Rows[j]["ci_no"].ToString();
                                    dr["CI_Date"] = dt.Rows[j]["ci_date"].ToString();
                                    dr["Custome_name"] = dt.Rows[j]["cust_name"].ToString();
                                    dr["Country"] = dt.Rows[j]["country"].ToString();
                                    dr["CI_FC_Amt"] = dt.Rows[j]["ci_amt_fc"].ToString();
                                    dr["shipment_country"] = "-";
                                    dr["shipment_terms"] = "-";
                                    model[0].dt1.Rows.Add(dr);
                                }
                                model[0].dt1.AcceptChanges();
                            }
                            break;
                    }
                    break;
                #endregion

                #region incentive apply
                case "inc_app":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select to_char(ic.vch_date,'" + sgen.Getsqldateformat() + "') as doc_date,ic.*,it.master_name as inc_type,a.master_name as bank_name,sb.* from enx_tab2 ic inner join master_setting it on ic.col21 = it.master_id and it.type = 'ITM' and find_in_set(it.client_unit_id,'" + unitid_mst + "')= 1" +
                                " inner join clients_mst bnk on bnk.vch_num = ic.col5 and bnk.type = 'BAD' and find_in_set(bnk.client_unit_id,'" + unitid_mst + "')= 1 inner join master_setting a on" +
                                " a.master_id = bnk.ptype and a.type = 'ABD' and a.client_unit_id = bnk.client_unit_id inner join(select* from (select (brc.client_id || brc.client_unit_id || brc.vch_num || to_char(brc.vch_date, 'yyyymmdd') || brc.type)  as fstr,cl.c_name as cust_name" +
                                ",sb.col21 as ship_country,sb.col22 as ship_terms,sb.col1 as SB_NO,to_char(sb.date2, '" + sgen.Getsqldateformat() + "') as sb_date,to_char(sb.date2, '" + sgen.Getsqldateformat() + "')" +
                                " as leo_date,brc.client_unit_id,brc.col9 as brc_no,to_char(brc.date3, '" + sgen.Getsqldateformat() + "') as brc_date,brc.col3 as LC_REC from enx_tab2 brc " +
                                "inner join enx_tab2 ci on ci.col6 = brc.col6 and ci.type = 'ECI' and ci.client_unit_id = '" + unitid_mst + "' inner join kc_tab sb on sb.col5 = ci.col6" +
                                " and sb.type = 'ESB' and sb.client_unit_id = ci.client_unit_id inner join clients_mst cl on cl.vch_num = ci.col5 and cl.type = 'BCD' and " +
                                "find_in_set(cl.client_unit_id,'" + unitid_mst + "')= 1 where brc.type = 'BRC' and brc.client_unit_id = '" + unitid_mst + "')) sb on sb.SB_NO = ic.col6 and " +
                                "sb.brc_no = ic.col7 and sb.client_unit_id = ic.client_unit_id where ic.client_id || ic.client_unit_id || ic.vch_num || to_char(ic.vch_date," +
                                " 'yyyymmdd') || ic.type in ('" + URL + "')";

                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model = getedit(model, dt, URL);
                                model[0].Col16 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dt.Rows[0]["col20"].ToString();
                                model[0].Col22 = dt.Rows[0]["col22"].ToString();
                                model[0].Col21 = dt.Rows[0]["doc_date"].ToString();
                                TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1 = cmd_Fun.incentive_type(userCode,unitid_mst);
                                String[] L1 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["col21"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems1 = L1;   
                                mq = "select a.master_name as Bank_name,cm.vch_num,cm.cpname as Bank_Acc,cm.REFERED_BY as Branch_Name,at.master_name as Account_type," +
                              "ct.master_name as currency_type,cm.bnkaddr as Address,cm.Addr as Department from clients_mst cm inner join master_setting" +
                              " a on a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id inner join master_setting at" +
                              " on at.master_id = cm.ploc and at.client_unit_id = cm.client_unit_id and at.type = 'BTM' inner join master_setting ct" +
                              " on ct.master_id = cm.panno and ct.client_unit_id = cm.client_unit_id and ct.type = 'CTM'" +
                              " where cm.type = 'BAD' and cm.client_unit_id = '" + unitid_mst + "' and cm.vch_num='" + dt.Rows[0]["col5"].ToString() + "'";
                                
                                dtt = sgen.getdata(userCode, mq);
                                if (dtt.Rows.Count > 0)
                                {
                                    model[0].Col18 = dtt.Rows[0]["Bank_name"].ToString();
                                    model[0].Col19 = dtt.Rows[0]["vch_num"].ToString();
                                }
                                model[0].dt1 = model[0].dt1.Clone();
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["SNo"] = j + 1;
                                    dr["Add_Remove"] = dt.Rows[j]["fstr"].ToString();
                                    dr["CUST_NAME"] = dt.Rows[j]["cust_name"].ToString();
                                    dr["SHIP_COUNTRY"] = dt.Rows[j]["ship_country"].ToString();
                                    dr["SB_TERM"] = dt.Rows[j]["ship_terms"].ToString();
                                    dr["SB_NO"] = dt.Rows[j]["SB_NO"].ToString();
                                    dr["SB_DATE"] = dt.Rows[j]["sb_date"].ToString();
                                    dr["LEO_DATE"] = dt.Rows[j]["leo_date"].ToString();
                                    dr["BRC_BANK"] = dt.Rows[j]["bank_name"].ToString();
                                    dr["BRC_NO"] = dt.Rows[j]["brc_no"].ToString();
                                    dr["BRC_AMT"] = dt.Rows[j]["LC_REC"].ToString();
                                    dr["INC_DUE"] = dt.Rows[j]["col9"].ToString();
                                    dr["INC_TYPE"] = dt.Rows[j]["col10"].ToString();
                                    dr["INC_APPLY"] = dt.Rows[j]["col19"].ToString();
                                    dr["Remark"] = dt.Rows[j]["col2"].ToString();
                                    model[0].dt1.Rows.Add(dr);
                                }
                            }
                            break;
                        case "BANK":
                            mq = "select a.master_name as Bank_name,a.master_id as bnk_name_id,cm.bnk,cm.vch_num,cm.cpname as Bank_Acc,cm.REFERED_BY as Branch_Name,at.master_name as Account_type," +
                                "ct.master_name as currency_type,ct.master_id as cur_id,cm.bnkaddr as Address,cm.Addr as Department from clients_mst cm inner join master_setting a on " +
                                "a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id inner join master_setting at on" +
                                " at.master_id = cm.ploc and at.client_unit_id = cm.client_unit_id and at.type = 'BTM' inner join master_setting ct " +
                                "on ct.master_id = cm.panno and ct.client_unit_id = cm.client_unit_id and ct.type = 'CTM'" +
                                " where (cm.client_id || cm.client_unit_id || cm.vch_num || to_char(cm.vch_date, 'yyyymmdd') || cm.type) = '" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model[0].Col18 = dt.Rows[0]["Bank_name"].ToString();
                                model[0].Col19 = dt.Rows[0]["vch_num"].ToString();
                                //model[0].Col20 = dt.Rows[0]["Address"].ToString();
                            }
                            break;
                        case "ITEM":
                            mq = "select (brc.client_id || brc.client_unit_id || brc.vch_num || to_char(brc.vch_date, 'yyyymmdd') || brc.type)  as fstr" +
                                ",cl.c_name as cust_name,sb.col21 as ship_country,sb.col22 as ship_terms,sb.col1 as SB_NO,to_char(sb.date2,'" + sgen.Getsqldateformat() + "') " +
                                "as sb_date,to_char(sb.date2, '" + sgen.Getsqldateformat() + "') as leo_date,a.master_name as bank_name,brc.col9 as brc_no," +
                                "to_char(brc.date3, '" + sgen.Getsqldateformat() + "') as brc_date,brc.col3 as LC_REC from enx_tab2 brc inner join enx_tab2 ci " +
                                "on ci.col6 = brc.col6 and ci.type = 'ECI' and ci.client_unit_id = '" + unitid_mst + "' inner join kc_tab sb on sb.col5 = ci.col6" +
                                " and sb.type = 'ESB' and sb.client_unit_id = ci.client_unit_id inner join clients_mst cl on cl.vch_num = ci.col5 and cl.type = 'BCD'" +
                                " and find_in_set(cl.client_unit_id,'" + unitid_mst + "')= 1 inner join clients_mst bnk on bnk.vch_num = brc.col4 and bnk.type = 'BAD'" +
                                " and find_in_set(bnk.client_unit_id,'" + unitid_mst + "')= 1 inner join master_setting a on a.master_id = bnk.ptype and a.type = 'ABD'" +
                                " and a.client_unit_id = bnk.client_unit_id where (brc.client_id || brc.client_unit_id || brc.vch_num || to_char(brc.vch_date, 'yyyymmdd') || brc.type) in ('" + URL + "')";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                if (model[0].dt1.Rows[0]["BRC_NO"].ToString().Trim().Equals("-")) { model[0].dt1.Rows.RemoveAt(0); }
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["SNo"] = j + 1;
                                    dr["Add_Remove"] = dt.Rows[j]["fstr"].ToString();
                                    dr["CUST_NAME"] = dt.Rows[j]["cust_name"].ToString();
                                    dr["SHIP_COUNTRY"] = dt.Rows[j]["ship_country"].ToString();
                                    dr["SB_TERM"] = dt.Rows[j]["ship_terms"].ToString();
                                    dr["SB_NO"] = dt.Rows[j]["SB_NO"].ToString();
                                    dr["SB_DATE"] = dt.Rows[j]["sb_date"].ToString();
                                    dr["LEO_DATE"] = dt.Rows[j]["leo_date"].ToString();
                                    dr["BRC_BANK"] = dt.Rows[j]["bank_name"].ToString();
                                    dr["BRC_NO"] = dt.Rows[j]["brc_no"].ToString();
                                    dr["BRC_AMT"] = dt.Rows[j]["LC_REC"].ToString();
                                    dr["INC_DUE"] = "0";
                                    dr["INC_TYPE"] =  "0";
                                    dr["INC_APPLY"] =  "0";
                                    dr["Remark"] =  "-";
                                    model[0].dt1.Rows.Add(dr);
                                }
                                model[0].dt1.AcceptChanges();
                            }
                            break;
                    }
                    break;
                    #endregion
            }
            return model;
        }
        #endregion


        //=========makequery============

        #region make query
        public void Make_query(string viewname, string title, string btnval, string seektype, string param1, string param2, string Myguid)
        {
            FillMst(Myguid);
            sgen.SetSession(MyGuid, "btnval", btnval);
            String URL = "";
            try
            {
                URL = sgen.GetSession(MyGuid, "SSEEKVAL").ToString();
            }
            catch (Exception err) { }
            string cmd = "";
            switch (viewname.ToLower())
            {
                #region Export Party Assigned

                case "party_ass":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = "SELECT a.client_id||a.client_unit_id||a.master_id||to_char(a.vch_date,'yyyymmdd')||a.type as fstr, a.master_name as Education_Category," +
                        "Section_Strength as Class_Hours,classid as Class_Starting_Time,sectionid as Class_End_Time ,(CASE when a.status='Y' THEN 'Active'  when a.status = '' THEN 'Active' else 'Inactive' end) as Status  " +
                           ",b.First_name ||' ' || b.middle_name || ' '|| b.last_name as Ent_By,to_char( convert_tz( a.master_EntDate,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "')as Ent_Date from master_setting a,user_details b  where a.type in ( 'ECC','DDECC') and  a.master_entby =lpad(b.rec_id,6,'0') and a.client_unit_id='" + unitid_mst + "' order by a.master_name ";
                            break;
                    }

                    break;

                #endregion

                #region Continents Master

                case "continents":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = "SELECT a.client_id||a.client_unit_id||a.master_id||to_char(a.vch_date,'yyyymmdd')||a.type as fstr, a.master_name as Education_Category," +
                        "Section_Strength as Class_Hours,classid as Class_Starting_Time,sectionid as Class_End_Time ,(CASE when a.status='Y' THEN 'Active' when a.status = '' THEN 'Active' else 'Inactive' end) as Status  " +
                           ",b.First_name ||' ' || b.middle_name || ' '|| b.last_name as Ent_By,to_char( convert_tz( a.master_EntDate,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "')as Ent_Date from master_setting a,user_details b  where a.type in ( 'ECC','DDECC') and  a.master_entby =lpad(b.rec_id,6,'0') and a.client_unit_id='" + unitid_mst + "' order by a.master_name ";
                            break;
                    }

                    break;

                #endregion

                #region Bill of Lading

                case "bill_of_lading":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = "SELECT a.client_id||a.client_unit_id||a.master_id||to_char(a.vch_date,'yyyymmdd')||a.type as fstr, a.master_name as Education_Category," +
                        "Section_Strength as Class_Hours,classid as Class_Starting_Time,sectionid as Class_End_Time ,(CASE when a.status='Y' THEN 'Active'  when a.status = '' THEN 'Active' else 'Inactive' end) as Status  " +
                           ",b.First_name ||' ' || b.middle_name || ' '|| b.last_name as Ent_By,to_char( convert_tz( a.master_EntDate,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "')as Ent_Date from master_setting a,user_details b  where a.type in ( 'ECC','DDECC') and  a.master_entby =lpad(b.rec_id,6,'0') and a.client_unit_id='" + unitid_mst + "' order by a.master_name ";
                            break;
                    }

                    break;

                #endregion

                #region Bill of Entry

                case "bill_of_entry":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = "SELECT a.client_id||a.client_unit_id||a.master_id||to_char(a.vch_date,'yyyymmdd')||a.type as fstr, a.master_name as Education_Category," +
                        "Section_Strength as Class_Hours,classid as Class_Starting_Time,sectionid as Class_End_Time ,(CASE when a.status='Y' THEN 'Active' when a.status = '' THEN 'Active' else 'Inactive' end) as Status  " +
                           ",b.First_name ||' ' || b.middle_name || ' '|| b.last_name as Ent_By,to_char( convert_tz( a.master_EntDate,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "')as Ent_Date from master_setting a,user_details b  where a.type in ( 'ECC','DDECC') and  a.master_entby =lpad(b.rec_id,6,'0') and a.client_unit_id='" + unitid_mst + "' order by a.master_name ";
                            break;
                    }

                    break;

                #endregion

                #region Bulk Comm Invoice
                case "bulk_comm_inv":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":

                            cmd = "select (com.client_id || com.client_unit_id || com.vch_num || to_char(com.vch_date, 'yyyymmdd') || " +
                                "com.type) fstr,com.vch_num as doc_no,to_char(com.vch_date, '" + sgen.Getsqldateformat() + "') as doc_date,com.col6 as comm_inv_no" +
                                ",to_char(com.date1, '" + sgen.Getsqldateformat() + "') as invoice_date,(case when com.col12 = '90' " +
                                "then 'With Pi' else 'Without Pi' end) as Comm_type,com.col5 as customer_id,cm.c_name" +
                                " as customer_name,com.col8 as Comm_Amt_in_FC, com.col2 as exchange_rate,nvl((to_number(com.col2) * " +
                                "to_number(com.col8)), 0) as com_AMT_INR,nvl(upper(ms2.master_name), '-') as com_currency,nvl(pi.col7, 0) " +
                                "as pi_no,nvl(to_char(pi.date1, '" + sgen.Getsqldateformat() + "'), 0) as PI_date,nvl((to_number(pi.col9) * to_number(pi.col10)), " +
                                "0) as pi_Amt_in_NR,nvl(pi.col9, 0) as pi_amt_fc,com.col11 as comm_no,to_char(com.date2, '" + sgen.Getsqldateformat() + "') as " +
                                "comm_date,com.col14 as comm_amt_fc_dt,nvl(upper(ms.master_name), '-') as pi_currency from enx_tab2 com inner" +
                                " join clients_mst cm on cm.vch_num = com.col5 and cm.type = 'BCD' and cm.client_unit_id = com.client_unit_id" +
                                " left join enx_tab2 pi on pi.col7 = com.col3 and pi.type = 'EPI' and pi.client_unit_id = com.client_unit_id " +
                                "left join master_setting ms on ms.master_id = pi.col6 and ms.type = 'CTM' and ms.client_unit_id = " +
                                "pi.client_unit_id left join master_setting ms2 on ms2.master_id = com.col7 and ms2.type = 'CTM' and " +
                                "ms2.client_unit_id = com.client_unit_id where com.type = 'ECI' and com.client_unit_id = '" + unitid_mst + "' and to_date(to_char(com.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "')";

                            break;

                        case "NEW":
                            cmd = "select (client_id||client_unit_id||master_id||to_char(vch_date,'yyyymmdd')||type) fstr,master_id Type,master_name Name " +
                                "from master_setting where type='KCI' and client_unit_id='" + unitid_mst + "'";
                            break;

                        case "PARTY":
                            //cmd = "select distinct (a.client_id||a.client_unit_id||a.vch_num|| to_char(a.vch_date,'yyyymmdd')||a.type) as fstr," +
                            //    "a.c_name Name,a.addr address,a.pincode Pincode,a.country,a.isgstr IsGST, a.c_gstin as GSTIN," +
                            //    "nvl((case when a.tor = 'C' then 'Composition' when a.tor = 'R' then 'Regular' END),'Unregistered') GSTType," +
                            //    "a.type_desc as Search_text,a.cpname as PerName,a.cpcont as ContactNo,a.cpaltcont as AltContNo,a.cpemail as Email," +
                            //    "a.cpaddr PerAddress,a.cpdesig Designation from clients_mst a " +
                            //    "inner join enx_tab2 pi on pi.col5 = a.vch_num and pi.type = 'EPI' and pi.client_unit_id = '" + unitid_mst + "' and pi.client_id='" + clientid_mst + "' " +
                            //    "where a.type = 'BCD' and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                            if (sgen.GetSession(MyGuid, "KCITYPE").ToString() == "90")
                            {
                                cmd = "select distinct (a.vch_num|| to_char(a.vch_date,'yyyymmdd')||a.type) as fstr,a.vch_num as Customer_code,a.c_name as Name,a.addr address,a.pincode as Pincode" +
                           ",(case when a.cpcont='0000000000' then '-' else a.cpcont end) ContactNo,(case when a.cpemail='ab@ab.ab' then '-' else a.cpemail end) as Email from clients_mst a " +
                    "inner join enx_tab2 pi on a.vch_num = pi.col5 and pi.type = 'EPI' and find_in_set(a.client_unit_id, pi.client_unit_id)=1 " +
                    "where find_in_set(a.client_unit_id, '" + unitid_mst + "')=1 AND a.type = 'BCD' and substr(a.vch_num,0,3)='303'";

                            }
                            else
                            {
                                cmd = "select distinct (a.vch_num|| to_char(a.vch_date,'yyyymmdd')||a.type) as fstr,a.vch_num as Customer_code,a.c_name as Name,a.addr address,a.pincode as Pincode,a.c_gstin as GSTIN," +
                           "a.type_desc as Search_text,(case when a.cpcont='0000000000' then '-' else a.cpcont end) ContactNo,(case when a.cpemail='ab@ab.ab' then '-' else a.cpemail end) as Email " +
                           "from clients_mst a where find_in_set(a.client_unit_id,'" + unitid_mst + "')=1 AND a.type='BCD' and substr(a.vch_num,0,3)='303'";

                            }

                            break;

                        case "PI":

                            cmd = "select (pi.client_id || pi.client_unit_id || pi.vch_num || to_char(pi.vch_date, 'yyyymmdd') || pi.type) as fstr," +
                                "pi.vch_num as Doc_Number,pi.col7 as PI_no,pi.col9 as pi_amt_fc,cr.master_name as Currency" +
                                ",to_char(pi.date1, '" + sgen.Getsqldateformat() + "') as PI_date,pi.col10 as exchange_rate " +
                                "from enx_tab2 pi inner join master_setting cr on cr.master_id = pi.col6 and cr.type = 'CTM' and " +
                                "cr.client_unit_id = '" + unitid_mst + "' where pi.type = 'EPI' and pi.client_unit_id = '" + unitid_mst + "'";

                            break;

                    }

                    break;

                #endregion

                #region Export Import Payment Entry

                case "bulk_banking":
                    switch (btnval.ToUpper())
                    {
                        case "NEW":
                            cmd = "select '001' as ID ,'WITH PI' as Type_of_Payment from dual union all select '002' as ID ,'WITH CI' as WITH_CI " +
                                "from dual union all select '003' as ID ,'WITH PI and CI' as WITH_PI_and_CI from dual";
                            break;

                        case "EDIT":
                        case "VIEW":
                            cmd = "select distinct (bb.client_id || bb.client_unit_id || bb.vch_num || to_char(bb.vch_date, 'yyyymmdd') || bb.type) fstr,bb.col6 as inv_no," +
                                "bb.col8 as inv_type,bb.col9 as amc_rcpt_fc,(case when bb.col2 = 'Y' then 'YES' when bb.col2 = 'N' then 'NO' else '-' end) as Fwd_Linked," +
                                "bb.col10 as bal_fc,bb.col11 as to_be_rcpt_fc,cl.vch_num Cust_id, bb.vch_num, cl.C_name Cust_Name, nvl(cr.master_name, 0) as pi_curr," +
                                "nvl(ci_cr.master_name, 0) as ci_curr, nvl(pi.col7, 0) as PI_no,nvl(pi.col9, 0) as pi_amt_fc," +
                                "nvl(to_char(bk.date2, '" + sgen.Getsqldateformat() + "'), 0) as Fwd_Start_Date," +
                                "nvl(to_char(bk.date3, '" + sgen.Getsqldateformat() + "'), 0) as Fwd_End_Date" +
                                ",nvl(to_number(pi.col9) * to_number(pi.col10), 0) as pi_Amt_INR,nvl(pi.vch_num, 0) pi_vchnum,nvl(to_char(convert_tz(pi.date1, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "'), 0) as doc_dt" +
                                ",nvl(pi.col10, 0) as pi_exc_rate,nvl(ci.col8, 0) C_amt_in_FC,nvl(to_number(ci.col8) * to_number(ci.col2), 0) as ci_Amt_INR,nvl(bk.col11, 0) as Forward_Amount," +
                                "nvl(bk.vch_num, 0) as fwd_doc,nvl(bk.col9, 0) as spot_rate,nvl(to_char(ci.date1, '" + sgen.Getsqldateformat() + "'), 0) as com_inv_date,nvl(ci.col6, 0) as comm_inv_no from enx_tab2 bb " +
                                "left join enx_tab2 pi on pi.col7 = bb.col6 and pi.type = 'EPI' and pi.client_unit_id = bb.client_unit_id " +
                                "left join enx_tab2 ci on ci.col6 = bb.col6 and ci.type = 'ECI' and ci.client_unit_id = bb.client_unit_id " +
                                "left join enx_tab2 bbw on bbw.vch_num = bb.vch_num and bbw.type = 'BBW' and bbw.client_unit_id = bb.client_unit_id " +
                                "inner join clients_mst cl on cl.vch_num = bb.col5 and cl.type = 'BCD' and cl.client_unit_id = bb.client_unit_id " +
                                "left join master_setting cr on cr.master_id = pi.col6 and cr.type = 'CTM' and cr.client_unit_id = pi.client_unit_id " +
                                "left join master_setting ci_cr on ci_cr.master_id = pi.col6 and ci_cr.type = 'CTM' and ci_cr.client_unit_id = ci.client_unit_id " +
                                "left join vehicle_master bk on bbw.col13 = bk.vch_num and bk.type = 'BFW' and bk.client_unit_id = bbw.client_unit_id" +
                                " where bb.type = 'BBK' and bb.client_unit_id = '" + unitid_mst + "' and to_date(to_char(bb.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "')";

                            break;

                        case "PARTY":
                            if (sgen.GetSession(MyGuid, "inv_type").Equals("001"))
                            {
                                cmd = "select distinct (a.vch_num|| to_char(a.vch_date,'yyyymmdd')||a.type) as fstr,a.vch_num as Customer_code," +
                  "a.c_name as Name,a.addr address,a.pincode as Pincode,(case when a.cpcont='0000000000' then '-' else a.cpcont end) ContactNo,(case when a.cpemail='ab@ab.ab' then '-' else a.cpemail end) as Email from clients_mst a " +
                  "inner join enx_tab2 pi on a.vch_num = pi.col5 and pi.type = 'EPI' and find_in_set(a.client_unit_id, pi.client_unit_id)=1 " +
                  "where find_in_set(a.client_unit_id, '" + unitid_mst + "')=1 AND a.type = 'BCD' and substr(a.vch_num,0,3)='303'";
                            }
                            else if (sgen.GetSession(MyGuid, "inv_type").Equals("002"))
                            {

                                cmd = "select distinct (a.vch_num|| to_char(a.vch_date,'yyyymmdd')||a.type) as fstr,a.vch_num as Customer_code," +
        "a.c_name as Name,a.addr address,a.pincode as Pincode,(case when a.cpcont='0000000000' then '-' else a.cpcont end) ContactNo,(case when a.cpemail='ab@ab.ab' then '-' else a.cpemail end) as Email from clients_mst a " +
        "inner join enx_tab2 pi on a.vch_num = pi.col5 and pi.type = 'ECI' and find_in_set(a.client_unit_id, pi.client_unit_id)=1 " +
        "where find_in_set(a.client_unit_id, '" + unitid_mst + "')=1 AND a.type = 'BCD' and substr(a.vch_num,0,3)='303'";
                            }
                            else if (sgen.GetSession(MyGuid, "inv_type").Equals("003"))
                            {

                                mq = "select distinct (cl.vch_num || to_char(cl.vch_date, 'yyyymmdd') || cl.type) as fstr,cl.c_name as cust_name,cl.vch_num Cust_id, cl.addr,cl.country,(case when pi.type = 'EPI' then 'PI' else '-' end) as Inv_type from clients_mst cl inner join enx_tab2 pi on pi.col5 = cl.vch_num and pi.type = 'EPI' and find_in_set(pi.client_unit_id, cl.client_unit_id)=1 where cl.type = 'BCD' and substr(cl.vch_num,0,3)='303' and find_in_set(cl.client_unit_id, '" + unitid_mst + "')=1";

                                mq1 = "select distinct (cl.vch_num || to_char(cl.vch_date, 'yyyymmdd') || cl.type) as fstr,cl.C_name Cust_Name, cl.vch_num Cust_id, cl.addr,cl.country,(case when ci.type = 'ECI' then 'CI' else '-' end) as Inv_type from clients_mst cl inner join enx_tab2 ci on cl.vch_num = ci.col5 and ci.type = 'ECI'  and find_in_set(ci.client_unit_id, cl.client_unit_id)=1 where cl.type = 'BCD' and substr(cl.vch_num,0,3)='303' and find_in_set(cl.client_unit_id, '" + unitid_mst + "')=1";

                                cmd = mq + " union all " + mq1;
                            }
                            break;

                        case "PI":
                            string cond = " and cl.vch_num = '" + sgen.GetSession(MyGuid, "cstmr_id").ToString() + "'";
                           // string bnkcond = " and pi.col6 = '" + sgen.GetSession(MyGuid, "cur_type") + "'";
                            cmd = "select (pi.client_id || pi.client_unit_id || pi.vch_num || to_char(pi.vch_date, 'yyyymmdd') || pi.type) as fstr,pi.vch_num as Doc_Number" +
                                  ",pi.col7 as PI_no,pi.col9 as pi_amt_fc,cr.master_name as Currency," +
                                  "to_char(pi.date1, '" + sgen.Getsqldateformat() + "') as PI_date,pi.col10 as exchange_rate from enx_tab2 pi " +
                                  "inner join master_setting cr on cr.master_id = pi.col6 and cr.type = 'CTM' and cr.client_unit_id = '" + unitid_mst + "' " +
                                  "inner join clients_mst cl on cl.vch_num = pi.col5 and cl.type = 'BCD' and cl.client_unit_id = '" + unitid_mst + "' " +
                                  "where pi.type = 'EPI' and pi.client_unit_id = '" + unitid_mst + "' " + cond + "";

                            break;

                        case "CI":
                            cond = " and cl.vch_num = '" + sgen.GetSession(MyGuid, "cstmr_id").ToString() + "'";
                           // bnkcond = " and com.col7 = '" + sgen.GetSession(MyGuid, "cur_type").ToString() + "'";
                            cmd = "select (com.client_id || com.client_unit_id || com.vch_num || to_char(com.vch_date, 'yyyymmdd') || com.type) as fstr," +
                                  "com.col6 as comm_inv_no,com.col8 C_amt_in_FC," +
                                  "to_char(com.date1, '" + sgen.Getsqldateformat() + "') as c_inv_date," +
                                  "com.col2 as exc_rate,to_number(com.col8) * to_number(com.col2) as Amt_in_NR from enx_tab2 com " +
                                  "inner join clients_mst cl on cl.vch_num = com.col5 and cl.type = 'BCD' and cl.client_unit_id = '001001' " +
                                  "where com.type = 'ECI' and com.client_unit_id = '" + unitid_mst + "' " + cond + "";
                            break;

                        case "CIPI":
                            cond = " and cl.vch_num = '" + sgen.GetSession(MyGuid, "cstmr_id").ToString() + "'";
                            //bnkcond = " and pi.col6 = '" + sgen.GetSession(MyGuid, "cur_type").ToString() + "'";
                            string bnkcond2 = " and com.col7 = '" + sgen.GetSession(MyGuid, "cur_type").ToString() + "'";
                            mq = "select (pi.client_id || pi.client_unit_id || pi.vch_num || to_char(pi.vch_date, 'yyyymmdd') || pi.type) as fstr,pi.vch_num as Doc_Number," +
                                 "pi.col7 as Inv_no,nvl(bb.used_amt, 0) as used_amt,pi.col9 - nvl(bb.used_amt, 0) as bal_amt,pi.col9 as Amt_in_fc,cr.master_name as Currency," +
                                 "to_number(pi.col9) * to_number(pi.col10) as Amt_in_INR,(case when pi.type = 'EPI' then 'PI' else '-' end) as inv_type," +
                                 "to_char(pi.date1, '" + sgen.Getsqldateformat() + "') as inv_Date,pi.col10 as Exchange_Rate from enx_tab2 pi " +
                                 "inner join master_setting cr on cr.master_id = pi.col6 and cr.type = 'CTM' and cr.client_unit_id = pi.client_unit_id " +
                                 "inner join clients_mst cl on cl.vch_num = pi.col5 and cl.type = 'BCD' and cl.client_unit_id = pi.client_unit_id " +
                                 "left join (select sum(col11) as used_amt ,col6 from enx_tab2 where client_unit_id = '" + unitid_mst + "' and type = 'BBK' group by col6) bb on " +
                                 "bb.col6 = pi.col7 where pi.type = 'EPI' and pi.client_unit_id = '" + unitid_mst + "'  " + cond + "";

                            mq1 = "select (com.client_id || com.client_unit_id || com.vch_num || to_char(com.vch_date, 'yyyymmdd') || com.type) as fstr,com.vch_num as ci_doc_no," +
                                  "com.col6 as comm_inv_no,nvl(bb.used_amt, 0) as used_amt,com.col8 - nvl(bb.used_amt, 0) as bal_amt,com.col8 C_amt_in_FC, cr.master_name as ci_curr," +
                                  "to_number(com.col8) * to_number(com.col2) as C_Amt_in_NR,(case when com.type = 'ECI' then 'CI' else '-' end) as inv_type," +
                                  "to_char(com.date1, '" + sgen.Getsqldateformat() + "') as c_inv_date,com.col2 as c_exc_rate from enx_tab2 com " +
                                  "inner join master_setting cr on cr.master_id = com.col7 and cr.type = 'CTM' and cr.client_unit_id = com.client_unit_id " +
                                  "left join (select sum(col11) as used_amt ,col6 from enx_tab2 where client_unit_id = '" + unitid_mst + "' and type = 'BBK' group by col6) bb on " +
                                  "bb.col6 = com.col6 inner join clients_mst cl on cl.vch_num = com.col5 and cl.type = 'BCD' and cl.client_unit_id = com.client_unit_id" +
                                  " where com.type = 'ECI'  and com.client_unit_id = '" + unitid_mst + "'  " + cond + " " + bnkcond2 + "";

                            cmd = mq + " union all " + mq1;

                            string query = mq + " union all " + mq1;

                            DataTable dtt = sgen.getdata(userCode, query);
                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "cipi_dt", dtt);
                            }
                            break;

                        case "BANK":
                            cmd = "select (cm.client_id||cm.client_unit_id|| cm.vch_num|| to_char(cm.vch_date, 'yyyymmdd')||cm.type )as fstr," +
                                  "a.master_name as Bank_name,cm.cpname as Bank_Acc,cm.REFERED_BY as Branch_Name,at.master_name as Account_type," +
                                  "ct.master_name as currency_type,cm.bnkaddr as Address,cm.Addr as Department from clients_mst cm inner join master_setting" +
                                  " a on a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id inner join master_setting at" +
                                  " on at.master_id = cm.ploc and at.client_unit_id = cm.client_unit_id and at.type = 'BTM' inner join master_setting ct" +
                                  " on ct.master_id = cm.panno and ct.client_unit_id = cm.client_unit_id and ct.type = 'CTM'" +
                                  " where cm.type = 'BAD' and cm.client_unit_id = '" + unitid_mst + "'";
                            break;

                        case "FRWD":

                            string bnkname_cond = " and a.master_id = '" + sgen.GetSession(MyGuid, "bnk_name_id").ToString() + "'";
                            string bnkcond = " and bk.col2='SALE' ";
                            cmd = "select (bk.client_id||bk.client_unit_id|| bk.vch_num|| to_char(bk.vch_date, 'yyyymmdd')||bk.type )as fstr, bk.col10 as Fwd_no,to_char(bk.date5, '" + sgen.Getsqldateformat() + "') as Fwd_date," +
                                  "bk.col11 as Fwd_Amt, bk.col9 as spot_rate,bk.col8 as Premium,bk.col14 as Fwd_rate,bk.col2 as nature_of_trans,a.master_name as Bank_name,cm.REFERED_BY as Branch_Name," +
                                  "to_char(bk.date2, '" + sgen.Getsqldateformat() + "') as Fwd_Start_Date," +
                                  "to_char(bk.date3, '" + sgen.Getsqldateformat() + "') as Fwd_End_Date,to_char(bk.date4, '" + sgen.Getsqldateformat() + "') as Fwd_grace_end,bk.col13 as Purpose from " +
                                  "vehicle_master bk inner join clients_mst cm on cm.vch_num = bk.col1 and cm.type = 'BAD' and cm.client_unit_id = '" + unitid_mst + "' " +
                                  "inner join master_setting a on a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id" +
                                  " where bk.type = 'BFW' and bk.client_unit_id = '" + unitid_mst + "' " + bnkname_cond + " " + bnkcond + "";
                            break;
                    }

                    break;

                #endregion

                #region Export Incentive Master

                case "exp_inc_m":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = "SELECT a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type as fstr,a.vch_num as Doc_no,to_char(a.vch_date,'" + sgen.Getsqldateformat() + "') as doc_date,nvl(b.master_name, '-') as incentve_type,a.col9 as percentage,to_char(a.date1, '" + sgen.Getsqldateformat() + "') as effective_from,to_char(a.date2, '" + sgen.Getsqldateformat() + "') as effective_to from enx_tab a left join master_setting b on a.col7 = b.master_id and b.type = 'ITM' and find_in_set(b.client_unit_id, a.client_unit_id)= 1 where a.type = 'INC' and a.client_unit_id = '" + unitid_mst + "' and to_date(to_char(a.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "')";
                            break;
                    }

                    break;

                #endregion

                #region Export Incentive Due

                case "due_inc_file":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = "SELECT a.client_id||a.client_unit_id||a.master_id||to_char(a.vch_date,'yyyymmdd')||a.type as fstr, a.master_name as Education_Category," +
                        "Section_Strength as Class_Hours,classid as Class_Starting_Time,sectionid as Class_End_Time ,(CASE when a.status='Y' THEN 'Active' when a.status = '' THEN 'Active' else 'Inactive' end) as Status  " +
                           ",b.First_name ||' ' || b.middle_name || ' '|| b.last_name as Ent_By,to_char( convert_tz( a.master_EntDate,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "')as Ent_Date from master_setting a,user_details b  where a.type in ( 'ECC','DDECC') and  a.master_entby =lpad(b.rec_id,6,'0') and a.client_unit_id='" + unitid_mst + "' order by a.master_name ";
                            break;
                    }

                    break;

                #endregion

                #region Export Incentive Received

                case "inc_rec":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = "SELECT a.client_id||a.client_unit_id||a.master_id||to_char(a.vch_date,'yyyymmdd')||a.type as fstr, a.master_name as Education_Category," +
                        "Section_Strength as Class_Hours,classid as Class_Starting_Time,sectionid as Class_End_Time ,(CASE when a.status='Y' THEN 'Active'  when a.status = '' THEN 'Active' else 'Inactive' end) as Status  " +
                           ",b.First_name ||' ' || b.middle_name || ' '|| b.last_name as Ent_By,to_char( convert_tz( a.master_EntDate,'UTC','" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "')as Ent_Date from master_setting a,user_details b  where a.type in ( 'ECC','DDECC') and  a.master_entby =lpad(b.rec_id,6,'0') and a.client_unit_id='" + unitid_mst + "' order by a.master_name ";
                            break;
                    }

                    break;

                #endregion

                #region Export Incentive Utilise

                case "inc_use":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = "SELECT a.client_id||a.client_unit_id||a.master_id||to_char(a.vch_date,'yyyymmdd')||a.type as fstr, a.master_name as Education_Category," +
                        "Section_Strength as Class_Hours,classid as Class_Starting_Time,sectionid as Class_End_Time ,(CASE when a.status='Y' THEN 'Active' when a.status = '' THEN 'Active' else 'Inactive' end) as Status  " +
                           ",b.First_name ||' ' || b.middle_name || ' '|| b.last_name as Ent_By,to_char( convert_tz( a.master_EntDate,'UTC','" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "')as Ent_Date from master_setting a,user_details b  where a.type in ( 'ECC','DDECC') and  a.master_entby =lpad(b.rec_id,6,'0') and a.client_unit_id='" + unitid_mst + "' order by a.master_name ";
                            break;
                    }

                    break;

                #endregion

                #region PI link with Comm

                case "pi_comm":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = "SELECT a.client_id||a.client_unit_id||a.master_id||to_char(a.vch_date,'yyyymmdd')||a.type as fstr, a.master_name as Education_Category," +
                        "Section_Strength as Class_Hours,classid as Class_Starting_Time,sectionid as Class_End_Time ,(CASE when a.status='Y' THEN 'Active' when a.status = '' THEN 'Active' else 'Inactive' end) as Status  " +
                           ",b.First_name ||' ' || b.middle_name || ' '|| b.last_name as Ent_By,to_char( convert_tz( a.master_EntDate,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "')as Ent_Date from master_setting a,user_details b  where a.type in ( 'EPI','DDEPI') and  a.master_entby =lpad(b.rec_id,6,'0') and a.client_unit_id='" + unitid_mst + "' order by a.master_name ";
                            break;
                    }

                    break;

                #endregion

                #region BRC

                case "brc":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = "select (bc.client_id || bc.client_unit_id || bc.vch_num || to_char(bc.vch_date, 'yyyymmdd') || bc.type) fstr,bc.vch_num as doc_num" +
                                ",to_char(bc.vch_date,'" + sgen.Getsqldateformat() + "') as doc_date,bb.vch_num bb_doc_no,bb.col6 as inv_no,bb.col8 as inv_type,bb.col9 as amc_rcpt_fc" +
                                ",bb.col10 as bal_fc,bb.col11 as to_be_rcpt_fc,nvl(ci.col6, 0) as comm_inv_no,nvl(to_char(ci.date1, '" + sgen.Getsqldateformat() + "'), 0) as com_inv_date" +
                                ",nvl(ci.col8, 0) C_amt_in_FC,nvl(to_number(ci.col8) * to_number(ci.col2), 0) as ci_Amt_INR,nvl(ci.col2, 0) as ci_exchange_rate," +
                                "nvl(ci.vch_num, 0) as ci_vchnum,cr.master_name as curr,cl.vch_num Cust_id,cl.C_name Cust_Name, cl.addr,cl.country,bc.col2 as Fc_rec,bc.col3 as Lc_rec" +
                                ",bc.col9 as brc_no,nvl(to_char(bc.date3, '" + sgen.Getsqldateformat() + "'), 0) as brc_date,nvl(to_char(bc.date1, '" + sgen.Getsqldateformat() + "'), 0) as from_dt," +
                                "nvl(to_char(bc.date2, '" + sgen.Getsqldateformat() + "'), 0) as to_date from enx_tab2 bc inner join enx_tab2 bb on bb.vch_num = bc.col17 and " +
                                "bb.client_unit_id = bc.client_unit_id and bb.type = 'BBK' inner join enx_tab2 ci on ci.col6 = bb.col6 and ci.type = 'ECI' and " +
                                "ci.client_unit_id = '" + unitid_mst + "' inner join clients_mst cl on cl.vch_num = bb.col5 and cl.type = 'BCD' and " +
                                "find_in_set(cl.client_unit_id, '" + unitid_mst + "')= 1 inner join master_setting cr on cr.master_id = ci.col7 and cr.type = 'CTM' and " +
                                "find_in_set(cr.client_unit_id , '" + unitid_mst + "')= 1 where bc.type = 'BRC' and bc.client_unit_id = '" + unitid_mst + "' and to_date(to_char(bc.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "')";

                            break;
                        case "BANK":
                            cmd = "select (cm.client_id||cm.client_unit_id|| cm.vch_num|| to_char(cm.vch_date, 'yyyymmdd')||cm.type )as fstr," +
                                "a.master_name as Bank_name,cm.cpname as Bank_Acc,cm.REFERED_BY as Branch_Name,at.master_name as Account_type," +
                                "ct.master_name as currency_type,cm.bnkaddr as Address,cm.Addr as Department from clients_mst cm inner join master_setting" +
                                " a on a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id inner join master_setting at" +
                                " on at.master_id = cm.ploc and at.client_unit_id = cm.client_unit_id and at.type = 'BTM' inner join master_setting ct" +
                                " on ct.master_id = cm.panno and ct.client_unit_id = cm.client_unit_id and ct.type = 'CTM'" +
                                " where cm.type = 'BAD' and cm.client_unit_id = '" + unitid_mst + "'";
                            break;

                        case "ITEM":
                            cmd = "select (bb.client_id || bb.client_unit_id || bb.vch_num || to_char(bb.vch_date, 'yyyymmdd') || bb.type) fstr," +
                                "bb.vch_num as bb_doc_no,nvl(cl.vch_num,'-') Cust_id,nvl(cl.C_name,'-') Cust_Name, nvl(ci.col8, 0) C_amt_in_FC,bb.col6 as inv_no,bb.col8 as inv_type," +
                                "bb.col9 as amc_rcpt_fc,bb.col10 as bal_fc,bb.col11 as to_be_rcpt_fc,nvl(to_number(ci.col8) * to_number(ci.col2), 0) as " +
                                "ci_Amt_INR,nvl(cr.master_name,'-') as curr,nvl(ci.col2, 0) as ci_exchange_rate,nvl(ci.vch_num, 0) as ci_vchnum," +
                                "nvl(to_char(ci.date1, '" + sgen.Getsqldateformat() + "'), 0) as com_inv_date,nvl(ci.col6, 0) as comm_inv_no " +
                                "from enx_tab2 bb inner join enx_tab2 ci on ci.col6 = bb.col6 and ci.type = 'ECI' and ci.client_unit_id = '" + unitid_mst + "' " +
                                "inner join clients_mst cl on cl.vch_num = bb.col5 and cl.type = 'BCD' and find_in_set(cl.client_unit_id,'" + unitid_mst + "')=1 " +
                                "left join master_setting cr on(cr.master_id = ci.col7) and cr.type = 'CTM' and cr.client_unit_id = '" + unitid_mst + "'" +
                                " where bb.type = 'BBK' and bb.client_unit_id = '" + unitid_mst + "' ";
                            break;


                    }

                    break;

                #endregion

                #region book_fwd

                case "book_fwd":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = "select (bk.client_id||bk.client_unit_id|| bk.vch_num|| to_char(bk.vch_date, 'yyyymmdd')||bk.type )as fstr, bk.col10 as Fwd_no,to_char(bk.date5, '" + sgen.Getsqldateformat() + "') as Fwd_date," +
                                "bk.col11 as Fwd_Amt, bk.col9 as spot_rate,bk.col8 as Premium,bk.col14 as Fwd_rate,bk.col2 as nature_of_trans,a.master_name as Bank_name,cm.REFERED_BY as Branch_Name," +
                                "to_char(bk.date2, '" + sgen.Getsqldateformat() + "') as Fwd_Start_Date," +
                                "to_char(bk.date3, '" + sgen.Getsqldateformat() + "') as Fwd_End_Date,to_char(bk.date4, '" + sgen.Getsqldateformat() + "') as Fwd_grace_end,bk.col13 as Purpose from " +
                                "vehicle_master bk inner join clients_mst cm on cm.vch_num = bk.col1 and cm.type = 'BAD' and cm.client_unit_id = '" + unitid_mst + "' " +
                                "inner join master_setting a on a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id" +
                                " where bk.type = 'BFW' and bk.client_unit_id = '" + unitid_mst + "' and to_date(to_char(bk.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "')";

                            break;


                        case "NEW":

                            cmd = "select (cm.client_id || cm.client_unit_id || cm.vch_num || to_char(cm.vch_date, 'yyyymmdd') || cm.type) as fstr,cm.cpname as Bank_Acc," +
                                "cm.REFERED_BY as Branch_Name,a.master_name as Bank_name,cm.bnkaddr as Address from clients_mst cm inner join master_setting bt on bt.master_id = cm.ploc " +
                                "and bt.type = 'BTM' and bt.client_unit_id = cm.client_unit_id inner join master_setting a on a.master_id = cm.ptype and a.type = 'ABD' and " +
                                "a.client_unit_id = '" + unitid_mst + "' where cm.type = 'BAD' and cm.client_unit_id = '" + unitid_mst + "' and cm.bnk = 'Y'";

                            break;
                    }

                    break;

                #endregion

                #region bulk_pi

                case "bulk_pi":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":

                            cmd = "select (pi.client_id || pi.client_unit_id || pi.vch_num || to_char(pi.vch_date, 'yyyymmdd') || pi.type) as fstr," +
                                "pi.vch_num as Doc_Number,pi.col7 as PI_no,cl.c_name as cust_name,cl.addr,pi.col9 as pi_amt_fc,cr.master_name as Currency," +
                                "cl.country,to_char(pi.date1, '" + sgen.Getsqldateformat() + "') as PI_date,pi.col10 as exchange_rate from enx_tab2 pi" +
                                " inner join master_setting cr on cr.master_id = pi.col6 and cr.type = 'CTM' and cr.client_unit_id = '" + unitid_mst + "' " +
                                "inner join clients_mst cl on cl.vch_num = pi.col5 and cl.type = 'BCD' and cl.client_unit_id = '" + unitid_mst + "' " +
                                "where pi.type = 'EPI' and pi.client_unit_id = '" + unitid_mst + "' and to_date(to_char(pi.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "')";

                            break;
                        case "PARTY":
                            cmd = "select distinct (a.vch_num|| to_char(a.vch_date,'yyyymmdd')||a.type) as fstr,a.vch_num as Customer_code,a.c_name as Name,a.addr address,a.pincode as Pincode,a.c_gstin as GSTIN," +
                            "a.type_desc as Search_text,(case when a.cpcont='0000000000' then '-' else a.cpcont end) ContactNo,(case when a.cpemail='ab@ab.ab' then '-' else a.cpemail end) as Email " +
                            "from clients_mst a where find_in_set(a.client_unit_id,'" + unitid_mst + "')=1 AND a.type='BCD' and substr(a.vch_num,0,3)='303'";

                            break;
                    }

                    break;

                #endregion

                #region Use FWD

                case "use_fwd":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = "select distinct (uf.client_id || uf.client_unit_id || uf.vch_num || to_char(uf.vch_date, 'yyyymmdd') || uf.type) as fstr, bk.col11 as Fwd_Amount,cm.cpname as Bank_Acc," +
                                "bt.master_name as Acc_Type,a.master_name as Bank_name,cm.REFERED_BY as Branch_Name,bk.col9 as spot_rate,uf.col1 as bnk_id,uf.col4 as u_fwd_no,bk.col8 as Premium" +
                                ",bk.col10 as Fwd_no,uf.col13 as fwd_to_be,to_char(uf.date2, '" + sgen.Getsqldateformat() + "') as use_date,(bk.col9 + bk.col8) AS FWD_Rate," +
                                "((bk.col9 + bk.col8) * bk.col11) AS LOCAL_CUR, bk.col2 as nature_of_trans,to_char(bk.date2, '" + sgen.Getsqldateformat() + "') as Fwd_Start_Date," +
                                "to_char(bk.date3, '" + sgen.Getsqldateformat() + "') as Fwd_end_Date,to_char(bk.date4, '" + sgen.Getsqldateformat() + "') as Fwd_grace_end," +
                                "cur.master_name as fwd_cur,to_char(bk.date5, '" + sgen.Getsqldateformat() + "') as fwd_date,uf.col31 as Remark from vehicle_master bk inner join vehicle_master" +
                                " uf on uf.col4 = bk.col10 and uf.type = 'UFW' and uf.client_unit_id = bk.client_unit_id inner join clients_mst cm on " +
                                "cm.vch_num = uf.col1 and cm.type = 'BAD' and cm.client_unit_id = '" + unitid_mst + "' inner join master_setting bt on " +
                                "bt.master_id = cm.ploc and bt.type = 'BTM' and bt.client_unit_id = cm.client_unit_id inner join master_setting a on a.master_id = cm.ptype and a.type = 'ABD' and " +
                                "a.client_unit_id = cm.client_unit_id inner join master_setting cur on cur.master_id = bk.col12 and cur.type = 'FCM' and " +
                                "cur.client_unit_id = bk.client_unit_id where uf.type = 'UFW' and uf.client_unit_id = '" + unitid_mst + "' and to_date(to_char(uf.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "')";
                            break;

                        case "NEW":
                            cmd = "select (cm.client_id || cm.client_unit_id || cm.vch_num || to_char(cm.vch_date, 'yyyymmdd') || cm.type) as fstr,cm.cpname as Bank_Acc," +
                                "cm.REFERED_BY as Branch_Name,a.master_name as Bank_name,cm.bnkaddr as Address,cm.msmeno as IFSC,bt.master_name as Acc_Type from clients_mst cm " +
                                "inner join master_setting bt on bt.master_id = cm.ploc and bt.type = 'BTM' and bt.client_unit_id = cm.client_unit_id " +
                                "inner join master_setting a on a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id " +
                                "where cm.type = 'BAD' and cm.client_unit_id = '" + unitid_mst + "' and cm.bnk = 'Y'";

                            break;

                        case "BOOKFWD":
                            mq = "select * from (select (bk.client_id||bk.client_unit_id|| bk.vch_num|| to_char(bk.vch_date, 'yyyymmdd')||bk.type )as fstr," +
                                "bk.col11 as Fwd_Amount,bk.col9 as spot_rate,bk.col8 as Premium,bk.col10 as Forward_no," +
                                "to_char(bk.date2, '" + sgen.Getsqldateformat() + "') as Fwd_Start_Date," +
                                "to_char(bk.date3, '" + sgen.Getsqldateformat() + "') as Fwd_end_Date," +
                                "to_char(bk.date4, '" + sgen.Getsqldateformat() + "') as Fwd_grace_end," +
                                "to_char(bk.date5, '" + sgen.Getsqldateformat() + "') as fwd_date " +
                                "from vehicle_master bk where bk.type = 'BFW' and bk.client_unit_id = '" + unitid_mst + "') a " +
                                "where a.Forward_no not in (select distinct col4 from vehicle_master " +
                                "where type = 'UFW' and client_unit_id = '" + unitid_mst + "'  " +
                                "and col1 in (select distinct col1 from vehicle_master where type = 'UFW' and client_unit_id = '" + unitid_mst + "' ))";

                            mq1 = "select * from (select (bk.client_id||bk.client_unit_id|| bk.vch_num|| to_char(bk.vch_date, 'yyyymmdd')||bk.type )as fstr,bk.col11 as Fwd_Amount," +
                                "bk.col9 as spot_rate,bk.col8 as Premium,bk.col10 as Forward_no,to_char(bk.date2, '" + sgen.Getsqldateformat() + "') as Fwd_Start_Date," +
                                "to_char(bk.date3, '" + sgen.Getsqldateformat() + "') as " +
                                "Fwd_end_Date,to_char(bk.date4, '" + sgen.Getsqldateformat() + "') as Fwd_grace_end," +
                                "to_char(bk.date5, '" + sgen.Getsqldateformat() + "') as fwd_date from vehicle_master bk " +
                                "where bk.type = 'BFW' and bk.client_unit_id = '" + unitid_mst + "') a " +
                                "where a.Forward_no  in (select distinct col4 from vehicle_master where type = 'UFW' and client_unit_id = '" + unitid_mst + "'  and col1 in (select distinct col1 " +
                                "from vehicle_master where type = 'UFW' and client_unit_id = '" + unitid_mst + "' and col1 = '" + sgen.GetSession(MyGuid, "Bank_id").ToString() + "' ))";

                            cmd = mq + " union all " + mq1;
                            break;
                    }
                    break;
                #endregion

                #region shipping bill

                case "sb":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = "select a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type as fstr,a.vch_num as doc_no," +
                                "to_char(vch_date,'" + sgen.Getsqldateformat() + "') as doc_date,a.col1 as SB_No,to_char(date1,'" + sgen.Getsqldateformat() + "') as sb_date,to_char(date2,'" + sgen.Getsqldateformat() + "') as " +
                                "leo_date,a.col21 as shipment_country,a.col22 as shipment_terms,a.col23 as ship_port,a.col31 as remark,ci.* from kc_tab a " +
                                "inner join (select * from (select com.client_unit_id,com.col6 as ci_no,com.col8 ci_amt_fc, to_char(com.date1, '" + sgen.Getsqldateformat() + "') as ci_date" +
                                ",cl.vch_num as cust_id,cl.c_name as cust_name,cl.country,com.col2 as exc_rate,to_number(com.col8) * to_number(com.col2) as Amt_in_NR " +
                                "from enx_tab2 com inner join clients_mst cl on cl.vch_num = com.col5 and cl.type = 'BCD' and find_in_set(cl.client_unit_id, '" + unitid_mst + "')= 1" +
                                " where com.type = 'ECI' and com.client_unit_id = '" + unitid_mst + "')) ci on ci.ci_no = a.col5 and ci.client_unit_id = a.client_unit_id where" +
                                " a.type = 'ESB' and a.client_unit_id='" + unitid_mst + "' and to_date(to_char(a.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "')";
                            break;

                        case "BANK":
                            cmd = "select (cm.client_id||cm.client_unit_id|| cm.vch_num|| to_char(cm.vch_date, 'yyyymmdd')||cm.type )as fstr," +
                                "a.master_name as Bank_name,cm.cpname as Bank_Acc,cm.REFERED_BY as Branch_Name,at.master_name as Account_type," +
                                "ct.master_name as currency_type,cm.bnkaddr as Address,cm.Addr as Department from clients_mst cm inner join master_setting" +
                                " a on a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id inner join master_setting at" +
                                " on at.master_id = cm.ploc and at.client_unit_id = cm.client_unit_id and at.type = 'BTM' inner join master_setting ct" +
                                " on ct.master_id = cm.panno and ct.client_unit_id = cm.client_unit_id and ct.type = 'CTM'" +
                                " where cm.type = 'BAD' and cm.client_unit_id = '" + unitid_mst + "'";
                            break;
                        case "CI":
                            cmd = " select (com.client_id || com.client_unit_id || com.vch_num || to_char(com.vch_date, 'yyyymmdd') || com.type) as fstr" +
                                ",com.col6 as ci_no,com.col8 ci_amt_fc,to_char(com.date1, '" + sgen.Getsqldateformat() + "') as ci_date,cl.vch_num as cust_id,cl.c_name as cust_name" +
                                ",cl.country,com.col2 as exc_rate,to_number(com.col8) * to_number(com.col2) as Amt_in_NR from enx_tab2 com inner join clients_mst" +
                                " cl on cl.vch_num = com.col5 and cl.type = 'BCD' and find_in_set(cl.client_unit_id, '" + unitid_mst + "')=1 where com.type = 'ECI'" +
                                " and com.client_unit_id = '" + unitid_mst + "'";
                            break;
                    }
                    break;
                #endregion

                #region incentive apply

                case "inc_app":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = "select ic.client_id||ic.client_unit_id||ic.vch_num|| to_char(ic.vch_date, 'yyyymmdd')|| ic.type as fstr,ic.vch_num as doc_no,to_char(ic.vch_date,'" + sgen.Getsqldateformat() + "') as doc_date" +
                                ",ic.col20 as inc_no,it.master_name as inc_type,a.master_name as bank_name,ic.col22 as inc_amount,ic.col6 as sb_no,to_char(ic.date1, '" + sgen.Getsqldateformat() + "') as sb_date,ic.col7 as brc_no" +
                                ",ic.col8 as brc_amt,ic.col9 inc_due, ic.col10 as incen_type,ic.col19 as inc_apply,ic.col2 as remark from enx_tab2 ic inner join master_setting it on ic.col21 = it.master_id and " +
                                "it.type = 'ITM' and find_in_set(it.client_unit_id,'" + unitid_mst + "')= 1 inner join clients_mst bnk on bnk.vch_num = ic.col5 and bnk.type = 'BAD' and " +
                                "find_in_set(bnk.client_unit_id,'" + unitid_mst + "')= 1 inner join master_setting a on a.master_id = bnk.ptype and a.type = 'ABD' and a.client_unit_id" +
                                " = bnk.client_unit_id where ic.type = 'INP' and ic.client_unit_id = '" + unitid_mst + "' and to_date(to_char(ic.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "')";
                            break;

                        case "BANK":
                            cmd = "select (cm.client_id||cm.client_unit_id|| cm.vch_num|| to_char(cm.vch_date, 'yyyymmdd')||cm.type )as fstr," +
                                "a.master_name as Bank_name,cm.cpname as Bank_Acc,cm.REFERED_BY as Branch_Name,at.master_name as Account_type," +
                                "ct.master_name as currency_type,cm.bnkaddr as Address,cm.Addr as Department from clients_mst cm inner join master_setting" +
                                " a on a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id inner join master_setting at" +
                                " on at.master_id = cm.ploc and at.client_unit_id = cm.client_unit_id and at.type = 'BTM' inner join master_setting ct" +
                                " on ct.master_id = cm.panno and ct.client_unit_id = cm.client_unit_id and ct.type = 'CTM' " +
                                "inner join enx_tab2 bc on bc.col4 = cm.vch_num and bc.type='BRC' and bc.client_unit_id=cm.client_unit_id" +
                                " where cm.type = 'BAD' and cm.client_unit_id = '" + unitid_mst + "'";
                            break;
                        case "ITEM":
                            cmd = "select (brc.client_id || brc.client_unit_id || brc.vch_num || to_char(brc.vch_date, 'yyyymmdd') || brc.type) as fstr" +
                                ",cl.c_name as cust_name,sb.col21 as ship_country,sb.col22 as ship_terms,sb.col1 as SB_NO,to_char(sb.date2,'" + sgen.Getsqldateformat() + "') " +
                                "as sb_date,to_char(sb.date2, '" + sgen.Getsqldateformat() + "') as leo_date,a.master_name as bank_name,brc.col9 as brc_no,to_char(brc.date3, '" + sgen.Getsqldateformat() + "') " +
                                "as brc_date,brc.col3 as LC_REC from enx_tab2 brc inner join enx_tab2 ci on ci.col6 = brc.col6 and ci.type = 'ECI' and " +
                                "ci.client_unit_id = '" + unitid_mst + "' inner join kc_tab sb on sb.col5 = ci.col6 and sb.type = 'ESB' and sb.client_unit_id " +
                                "= ci.client_unit_id inner join clients_mst cl on cl.vch_num = ci.col5 and cl.type = 'BCD' and find_in_set(cl.client_unit_id," +
                                "'" + unitid_mst + "')= 1 inner join clients_mst bnk on bnk.vch_num = brc.col4 and bnk.type = 'BAD' and " +
                                "find_in_set(bnk.client_unit_id,'" + unitid_mst + "')= 1 inner join master_setting a on a.master_id = bnk.ptype and a.type = 'ABD'" +
                                " and a.client_unit_id = bnk.client_unit_id where brc.type = 'BRC' and brc.client_unit_id = '" + unitid_mst + "'";
                            break;
                    }
                    break;
                    #endregion
            }
            sgen.open_grid(title, cmd, sgen.Make_int(seektype));
            ViewBag.pageurl = sgen.GetSession(MyGuid, "pageurl").ToString();
        }
        #endregion

        //============= Action Method===============

        #region Export Party Assigned
        public ActionResult party_ass()
        {
            FillMst();
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            chkRef();

            ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.vsave = "disabled='disabled'";
            ViewBag.scripCall = "disableForm();";
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);

            sgen.SetSession(MyGuid, "TR_MID", mid_mst.Trim());
            Tmodelmain tm1 = new Tmodelmain();
            List<Tmodelmain> model = new List<Tmodelmain>();
            tm1.Col9 = "EXPORT PARTY ASSIGNED";
            tm1.Col10 = "master_setting"; //TABLE NAME
            tm1.Col13 = "Save";

            tm1.Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            tm1.Col12 = "ECC"; //TYPE
            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = MyGuid.Trim();


            model.Add(tm1);

            return View(model);
        }
        [HttpPost]
        public ActionResult party_ass(List<Tmodelmain> model, string command)
        {
            FillMst();
            Master_Type = "party_ass";
            if (command == "New")
            {
                sgen.SetSession(MyGuid, "EDMODE", "N");
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
                ViewBag.scripCall = "enableForm();";
                mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10 + " where type in  ('DDECC' ,'ECC')" + model[0].Col11 + "";
                master_id = sgen.genNo(userCode, mq, 3, "auto_genid");


                model[0].Col16 = master_id;

                model[0].Col22 = "Y";
            }


            else if (command == "Cancel")
            {
                return CancelFun(model);
            }
            else if (command == "Callback")
            {
                if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
                model = CallbackFun(btnval, "N", "party_ass", "Exim", model);
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
            }
            else if (command.Trim() == "Save" || command.Trim() == "Update")
            {
                try
                {
                    string found = "", inactive_date = "";
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime_local(userCode);
                    ent_date = sgen.Savedate(currdate, true);

                    DataTable dtstr = new DataTable();
                    dtstr = sgen.getdata(userCode, "select * from " + model[0].Col10 + " WHERE 1=2");

                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        mq1 = " and vch_num<>'" + tmodel.Col23.Trim() + "'";
                        isedit = true;
                        vch_num = tmodel.Col23.Trim();
                        master_id = tmodel.Col16.Trim();

                    }
                    else
                    {
                        isedit = false;



                        mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10 + " where type in  ('DDECC','ECC')" + model[0].Col11 + "";
                        master_id = sgen.genNo(userCode, mq, 3, "auto_genid");
                        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where type in   ('DDECC','ECC')" + model[0].Col11 + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");

                    }

                    found = sgen.getstring(userCode, "select master_name from " + model[0].Col10 + " a where upper(master_name)= upper('" + model[0].Col18 + "')" +
                        " and type in  ('DDECC','ECC') " + model[0].Col11 + " " + mq1 + "");
                    if (found != "")
                    {

                        ViewBag.scripCall = "showmsgJS(1, 'Data Already Exists', 2);";
                        goto END;
                    }

                    if (model[0].Col22 == "Y") { status = "Y"; inactive_date = currdate; model[0].Col12 = "ECC"; }
                    else { status = "N"; inactive_date = currdate; model[0].Col12 = "DDECC"; }
                    DataTable dataTable = sgen.getdata(userCode, "select  * from " + model[0].Col10 + " where 1=2");
                    DataRow dr = dataTable.NewRow();

                    dr["vch_num"] = vch_num;
                    dr["vch_date"] = sgen.Savedate(model[0].Col17, false);
                    dr["type"] = model[0].Col12;
                    dr["Master_type"] = Master_Type;
                    dr["Master_Name"] = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(model[0].Col18.ToLower());
                    dr["master_id"] = master_id;
                    dr["Section_Strength"] = model[0].Col20.Trim();
                    dr["classid"] = model[0].Col19.Trim();
                    dr["sectionid"] = model[0].Col21.Trim();
                    dr["Status"] = status;
                    dr["Inactive_date"] = sgen.Savedate(inactive_date, false);


                    dr["client_id"] = clientid_mst;
                    dr["client_unit_id"] = unitid_mst;
                    if (isedit)
                    {
                        dr["master_entby"] = model[0].Col3.Trim();
                        dr["master_entdate"] = model[0].Col4.Trim();
                        dr["master_editby"] = userid_mst;
                        dr["master_editdate"] = currdate;
                        dr["client_id"] = model[0].Col1;
                        dr["client_unit_id"] = model[0].Col2;
                        dr["rec_id"] = model[0].Col7;

                    }
                    else
                    {
                        dr["rec_id"] = "0";
                        dr["master_editby"] = "-";
                        dr["master_editdate"] = currdate;
                        dr["master_entby"] = userid_mst;
                        dr["master_entdate"] = currdate;
                        dr["client_id"] = clientid_mst;
                        dr["client_unit_id"] = unitid_mst;
                    }

                    dataTable.Rows.Add(dr);

                    bool Result = sgen.Update_data(userCode, dataTable, model[0].Col10.Trim(), tmodel.Col8, isedit);
                    if (Result == true)
                    {
                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        //ViewBag.scripCall = "showmsgJS(1, 'Data Saved Successfully', 1);disableForm();";
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";

                        model = new List<Tmodelmain>();
                        tmodel.Col16 = "";
                        tmodel.Col17 = "";
                        tmodel.Col18 = "";
                        tmodel.Col19 = "";
                        tmodel.Col20 = "";
                        tmodel.Col21 = "";
                        tmodel.Col13 = "Save";
                        tmodel.Col9 = "EXPORT PARTY ASSIGNED";
                        model.Add(tmodel);


                    }

                    else
                    {
                        ViewBag.scripCall = "showmsgJS(3, 'Record Not Saved', 1);";
                    }
                END:;

                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1, '" + ex.Message.ToString() + "', 1);"; }

            }



            ModelState.Clear();
            return View(model);

        }
        #endregion

        #region Continents Master
        public ActionResult continents()
        {

            FillMst();
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            chkRef();

            ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.vsave = "disabled='disabled'";
            ViewBag.scripCall = "disableForm();";
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            sgen.SetSession(MyGuid, "TR_MID", mid_mst.Trim());
            Tmodelmain tm1 = new Tmodelmain();
            List<Tmodelmain> model = new List<Tmodelmain>();
            tm1.Col9 = "CONTINENTS MASTER";
            tm1.Col10 = "master_setting"; //TABLE NAME
            tm1.Col13 = "Save";

            tm1.Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            tm1.Col12 = "ECC"; //TYPE
            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = MyGuid.Trim();


            model.Add(tm1);

            return View(model);
        }
        [HttpPost]
        public ActionResult continents(List<Tmodelmain> model, string command)
        {
            FillMst();
            Master_Type = "continents";
            if (command == "New")
            {
                sgen.SetSession(MyGuid, "EDMODE", "N");
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
                ViewBag.scripCall = "enableForm();";
                mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10 + " where type in  ('DDECC' ,'ECC')" + model[0].Col11 + "";
                master_id = sgen.genNo(userCode, mq, 3, "auto_genid");


                model[0].Col16 = master_id;

                model[0].Col22 = "Y";
            }


            else if (command == "Cancel")
            {
                return CancelFun(model);
            }
            else if (command == "Callback")
            {
                if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
                model = CallbackFun(btnval, "N", "party_ass", "Exim", model);
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
            }
            else if (command.Trim() == "Save" || command.Trim() == "Update")
            {
                try
                {
                    string found = "", inactive_date = "";
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime_local(userCode);
                    ent_date = sgen.Savedate(currdate, true);

                    DataTable dtstr = new DataTable();
                    dtstr = sgen.getdata(userCode, "select * from " + model[0].Col10 + " WHERE 1=2");

                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        mq1 = " and vch_num<>'" + tmodel.Col23.Trim() + "'";
                        isedit = true;
                        vch_num = tmodel.Col23.Trim();
                        master_id = tmodel.Col16.Trim();

                    }
                    else
                    {
                        isedit = false;



                        mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10 + " where type in  ('DDECC','ECC')" + model[0].Col11 + "";
                        master_id = sgen.genNo(userCode, mq, 3, "auto_genid");
                        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where type in   ('DDECC','ECC')" + model[0].Col11 + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");

                    }

                    found = sgen.getstring(userCode, "select master_name from " + model[0].Col10 + " a where upper(master_name)= upper('" + model[0].Col18 + "')" +
                        " and type in  ('DDECC','ECC') " + model[0].Col11 + " " + mq1 + "");
                    if (found != "")
                    {

                        ViewBag.scripCall = "showmsgJS(1, 'Data Already Exists', 2);";
                        goto END;
                    }

                    if (model[0].Col22 == "Y") { status = "Y"; inactive_date = currdate; model[0].Col12 = "ECC"; }
                    else { status = "N"; inactive_date = currdate; model[0].Col12 = "DDECC"; }
                    DataTable dataTable = sgen.getdata(userCode, "select  * from " + model[0].Col10 + " where 1=2");
                    DataRow dr = dataTable.NewRow();

                    dr["vch_num"] = vch_num;
                    dr["vch_date"] = sgen.Savedate(model[0].Col17, false);
                    dr["type"] = model[0].Col12;
                    dr["Master_type"] = Master_Type;
                    dr["Master_Name"] = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(model[0].Col18.ToLower());
                    dr["master_id"] = master_id;
                    dr["Section_Strength"] = model[0].Col20.Trim();
                    dr["classid"] = model[0].Col19.Trim();
                    dr["sectionid"] = model[0].Col21.Trim();
                    dr["Status"] = status;
                    dr["Inactive_date"] = sgen.Savedate(inactive_date, false);


                    dr["client_id"] = clientid_mst;
                    dr["client_unit_id"] = unitid_mst;
                    if (isedit)
                    {
                        dr["master_entby"] = model[0].Col3.Trim();
                        dr["master_entdate"] = model[0].Col4.Trim();
                        dr["master_editby"] = userid_mst;
                        dr["master_editdate"] = currdate;
                        dr["client_id"] = model[0].Col1;
                        dr["client_unit_id"] = model[0].Col2;
                        dr["rec_id"] = model[0].Col7;

                    }
                    else
                    {
                        dr["rec_id"] = "0";
                        dr["master_editby"] = "-";
                        dr["master_editdate"] = currdate;
                        dr["master_entby"] = userid_mst;
                        dr["master_entdate"] = currdate;
                        dr["client_id"] = clientid_mst;
                        dr["client_unit_id"] = unitid_mst;
                    }

                    dataTable.Rows.Add(dr);

                    bool Result = sgen.Update_data(userCode, dataTable, model[0].Col10.Trim(), tmodel.Col8, isedit);
                    if (Result == true)
                    {
                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        //ViewBag.scripCall = "showmsgJS(1, 'Data Saved Successfully', 1);disableForm();";
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";

                        model = new List<Tmodelmain>();
                        tmodel.Col16 = "";
                        tmodel.Col17 = "";
                        tmodel.Col18 = "";
                        tmodel.Col19 = "";
                        tmodel.Col20 = "";
                        tmodel.Col21 = "";
                        tmodel.Col13 = "Save";
                        tmodel.Col9 = "CONTINENTS MASTER";
                        model.Add(tmodel);


                    }

                    else
                    {
                        ViewBag.scripCall = "showmsgJS(3, 'Record Not Saved', 1);";
                    }
                END:;

                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1, '" + ex.Message.ToString() + "', 1);"; }

            }



            ModelState.Clear();
            return View(model);

        }
        #endregion

        #region Bill of Lading
        public ActionResult bill_of_lading()
        {

            FillMst();
            chkRef();

            ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.vsave = "disabled='disabled'";
            ViewBag.scripCall = "disableForm();";
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);

            sgen.SetSession(MyGuid, "TR_MID", mid_mst.Trim());
            Tmodelmain tm1 = new Tmodelmain();
            List<Tmodelmain> model = new List<Tmodelmain>();
            tm1.Col9 = "BILL OF LADING";
            tm1.Col10 = "master_setting"; //TABLE NAME
            tm1.Col13 = "Save";

            tm1.Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            tm1.Col12 = "ECC"; //TYPE
            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = MyGuid.Trim();


            model.Add(tm1);

            return View(model);
        }
        [HttpPost]
        public ActionResult bill_of_lading(List<Tmodelmain> model, string command)
        {
            FillMst();
            Master_Type = "continents";
            if (command == "New")
            {
                sgen.SetSession(MyGuid, "EDMODE", "N");
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
                ViewBag.scripCall = "enableForm();";
                mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10 + " where type in  ('DDECC' ,'ECC')" + model[0].Col11 + "";
                master_id = sgen.genNo(userCode, mq, 3, "auto_genid");


                model[0].Col16 = master_id;

                model[0].Col22 = "Y";
            }


            else if (command == "Cancel")
            {
                return CancelFun(model);
            }
            else if (command == "Callback")
            {
                if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
                model = CallbackFun(btnval, "N", "party_ass", "Exim", model);
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
            }
            else if (command.Trim() == "Save" || command.Trim() == "Update")
            {
                try
                {
                    string found = "", inactive_date = "";
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime_local(userCode);
                    ent_date = sgen.Savedate(currdate, true);

                    DataTable dtstr = new DataTable();
                    dtstr = sgen.getdata(userCode, "select * from " + model[0].Col10 + " WHERE 1=2");

                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        mq1 = " and vch_num<>'" + tmodel.Col23.Trim() + "'";
                        isedit = true;
                        vch_num = tmodel.Col23.Trim();
                        master_id = tmodel.Col16.Trim();

                    }
                    else
                    {
                        isedit = false;



                        mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10 + " where type in  ('DDECC','ECC')" + model[0].Col11 + "";
                        master_id = sgen.genNo(userCode, mq, 3, "auto_genid");
                        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where type in   ('DDECC','ECC')" + model[0].Col11 + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");

                    }

                    found = sgen.getstring(userCode, "select master_name from " + model[0].Col10 + " a where upper(master_name)= upper('" + model[0].Col18 + "')" +
                        " and type in  ('DDECC','ECC') " + model[0].Col11 + " " + mq1 + "");
                    if (found != "")
                    {

                        ViewBag.scripCall = "showmsgJS(1, 'Data Already Exists', 2);";
                        goto END;
                    }

                    if (model[0].Col22 == "Y") { status = "Y"; inactive_date = currdate; model[0].Col12 = "ECC"; }
                    else { status = "N"; inactive_date = currdate; model[0].Col12 = "DDECC"; }
                    DataTable dataTable = sgen.getdata(userCode, "select  * from " + model[0].Col10 + " where 1=2");
                    DataRow dr = dataTable.NewRow();

                    dr["vch_num"] = vch_num;
                    dr["vch_date"] = sgen.Savedate(model[0].Col17, false);
                    dr["type"] = model[0].Col12;
                    dr["Master_type"] = Master_Type;
                    dr["Master_Name"] = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(model[0].Col18.ToLower());
                    dr["master_id"] = master_id;
                    dr["Section_Strength"] = model[0].Col20.Trim();
                    dr["classid"] = model[0].Col19.Trim();
                    dr["sectionid"] = model[0].Col21.Trim();
                    dr["Status"] = status;
                    dr["Inactive_date"] = sgen.Savedate(inactive_date, false);


                    dr["client_id"] = clientid_mst;
                    dr["client_unit_id"] = unitid_mst;
                    if (isedit)
                    {
                        dr["master_entby"] = model[0].Col3.Trim();
                        dr["master_entdate"] = model[0].Col4.Trim();
                        dr["master_editby"] = userid_mst;
                        dr["master_editdate"] = currdate;
                        dr["client_id"] = model[0].Col1;
                        dr["client_unit_id"] = model[0].Col2;
                        dr["rec_id"] = model[0].Col7;

                    }
                    else
                    {
                        dr["rec_id"] = "0";
                        dr["master_editby"] = "-";
                        dr["master_editdate"] = currdate;
                        dr["master_entby"] = userid_mst;
                        dr["master_entdate"] = currdate;
                        dr["client_id"] = clientid_mst;
                        dr["client_unit_id"] = unitid_mst;
                    }

                    dataTable.Rows.Add(dr);

                    bool Result = sgen.Update_data(userCode, dataTable, model[0].Col10.Trim(), tmodel.Col8, isedit);
                    if (Result == true)
                    {
                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        //ViewBag.scripCall = "showmsgJS(1, 'Data Saved Successfully', 1);disableForm();";
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";

                        model = new List<Tmodelmain>();
                        tmodel.Col16 = "";
                        tmodel.Col17 = "";
                        tmodel.Col18 = "";
                        tmodel.Col19 = "";
                        tmodel.Col20 = "";
                        tmodel.Col21 = "";
                        tmodel.Col13 = "Save";
                        tmodel.Col9 = "BILL OF LADING";
                        model.Add(tmodel);


                    }

                    else
                    {
                        ViewBag.scripCall = "showmsgJS(3, 'Record Not Saved', 1);";
                    }
                END:;

                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1, '" + ex.Message.ToString() + "', 1);"; }

            }



            ModelState.Clear();
            return View(model);

        }
        #endregion

        #region BIll of Entry
        public ActionResult bill_of_entry()
        {

            FillMst();
            chkRef();

            ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.vsave = "disabled='disabled'";
            ViewBag.scripCall = "disableForm();";
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);

            sgen.SetSession(MyGuid, "TR_MID", mid_mst.Trim());
            Tmodelmain tm1 = new Tmodelmain();
            List<Tmodelmain> model = new List<Tmodelmain>();
            tm1.Col9 = "BILL OF ENTRY";
            tm1.Col10 = "master_setting"; //TABLE NAME
            tm1.Col13 = "Save";

            tm1.Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            tm1.Col12 = "ECC"; //TYPE
            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = MyGuid.Trim();


            model.Add(tm1);

            return View(model);
        }
        [HttpPost]
        public ActionResult bill_of_entry(List<Tmodelmain> model, string command)
        {
            FillMst();
            Master_Type = "continents";
            if (command == "New")
            {
                sgen.SetSession(MyGuid, "EDMODE", "N");
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
                ViewBag.scripCall = "enableForm();";
                mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10 + " where type in  ('DDECC' ,'ECC')" + model[0].Col11 + "";
                master_id = sgen.genNo(userCode, mq, 3, "auto_genid");


                model[0].Col16 = master_id;

                model[0].Col22 = "Y";
            }


            else if (command == "Cancel")
            {
                return CancelFun(model);
            }
            else if (command == "Callback")
            {
                if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
                model = CallbackFun(btnval, "N", "bill_of_entry", "Exim", model);
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
            }
            else if (command.Trim() == "Save" || command.Trim() == "Update")
            {
                try
                {
                    string found = "", inactive_date = "";
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime_local(userCode);
                    ent_date = sgen.Savedate(currdate, true);

                    DataTable dtstr = new DataTable();
                    dtstr = sgen.getdata(userCode, "select * from " + model[0].Col10 + " WHERE 1=2");

                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        mq1 = " and vch_num<>'" + tmodel.Col23.Trim() + "'";
                        isedit = true;
                        vch_num = tmodel.Col23.Trim();
                        master_id = tmodel.Col16.Trim();

                    }
                    else
                    {
                        isedit = false;



                        mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10 + " where type in  ('DDECC','ECC')" + model[0].Col11 + "";
                        master_id = sgen.genNo(userCode, mq, 3, "auto_genid");
                        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where type in   ('DDECC','ECC')" + model[0].Col11 + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");

                    }

                    found = sgen.getstring(userCode, "select master_name from " + model[0].Col10 + " a where upper(master_name)= upper('" + model[0].Col18 + "')" +
                        " and type in  ('DDECC','ECC') " + model[0].Col11 + " " + mq1 + "");
                    if (found != "")
                    {

                        ViewBag.scripCall = "showmsgJS(1, 'Data Already Exists', 2);";
                        goto END;
                    }

                    if (model[0].Col22 == "Y") { status = "Y"; inactive_date = currdate; model[0].Col12 = "ECC"; }
                    else { status = "N"; inactive_date = currdate; model[0].Col12 = "DDECC"; }
                    DataTable dataTable = sgen.getdata(userCode, "select  * from " + model[0].Col10 + " where 1=2");
                    DataRow dr = dataTable.NewRow();

                    dr["vch_num"] = vch_num;
                    dr["vch_date"] = sgen.Savedate(model[0].Col17, false);
                    dr["type"] = model[0].Col12;
                    dr["Master_type"] = Master_Type;
                    dr["Master_Name"] = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(model[0].Col18.ToLower());
                    dr["master_id"] = master_id;
                    dr["Section_Strength"] = model[0].Col20.Trim();
                    dr["classid"] = model[0].Col19.Trim();
                    dr["sectionid"] = model[0].Col21.Trim();
                    dr["Status"] = status;
                    dr["Inactive_date"] = sgen.Savedate(inactive_date, false);


                    dr["client_id"] = clientid_mst;
                    dr["client_unit_id"] = unitid_mst;
                    if (isedit)
                    {
                        dr["master_entby"] = model[0].Col3.Trim();
                        dr["master_entdate"] = model[0].Col4.Trim();
                        dr["master_editby"] = userid_mst;
                        dr["master_editdate"] = currdate;
                        dr["client_id"] = model[0].Col1;
                        dr["client_unit_id"] = model[0].Col2;
                        dr["rec_id"] = model[0].Col7;

                    }
                    else
                    {
                        dr["rec_id"] = "0";
                        dr["master_editby"] = "-";
                        dr["master_editdate"] = currdate;
                        dr["master_entby"] = userid_mst;
                        dr["master_entdate"] = currdate;
                        dr["client_id"] = clientid_mst;
                        dr["client_unit_id"] = unitid_mst;
                    }

                    dataTable.Rows.Add(dr);

                    bool Result = sgen.Update_data(userCode, dataTable, model[0].Col10.Trim(), tmodel.Col8, isedit);
                    if (Result == true)
                    {
                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        //ViewBag.scripCall = "showmsgJS(1, 'Data Saved Successfully', 1);disableForm();";
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";

                        model = new List<Tmodelmain>();
                        tmodel.Col16 = "";
                        tmodel.Col17 = "";
                        tmodel.Col18 = "";
                        tmodel.Col19 = "";
                        tmodel.Col20 = "";
                        tmodel.Col21 = "";
                        tmodel.Col13 = "Save";
                        tmodel.Col9 = "BILL OF ENTRY";
                        model.Add(tmodel);


                    }

                    else
                    {
                        ViewBag.scripCall = "showmsgJS(3, 'Record Not Saved', 1);";
                    }
                END:;

                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1, '" + ex.Message.ToString() + "', 1);"; }

            }



            ModelState.Clear();
            return View(model);

        }
        #endregion

        #region Bulk Comm Invoice
        public ActionResult bulk_comm_inv()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            model = getload(model);
            model[0].Col10 = "enx_tab2"; //TABLE NAME
            model[0].Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            model[0].Col12 = "ECI"; //TYPE
            model[0].Col26 = "90";
            TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
            model[0].SelectedItems1 = new string[] { "" };

            DataTable dt = sgen.getdata(userCode, "select '' PI,'1' as  SNo ,'-' as pi_no,'-' as pi_date,'-' pi_curr,'-'  Curr_rate,'-' as pi_amt_fc,'0' pi_amt_lc,'-' comm_No ,'-' comm_Date,'0' comm_amt_fc,'0' pi_bal_fc from dual");
            model[0].dt1 = dt;
            sgen.SetSession(MyGuid, "KMRN_DT", dt);
            sgen.SetSession(MyGuid, "KCITYPE", null);
            return View(model);
        }
        [HttpPost]
        public ActionResult bulk_comm_inv(List<Tmodelmain> model, string command, string hftable, string hfrow, HttpPostedFileBase upd1)
        {
            FillMst();
            DataTable dtm = sgen.settable(hftable);
            model[0].dt1 = dtm;
            type = model[0].Col12;
            tab_name = model[0].Col10;
            where = model[0].Col11;
            var tm = model.FirstOrDefault();

            #region ddl
            List<SelectListItem> mod1 = new List<SelectListItem>();
            TempData[MyGuid + "_TList1"] = model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];

            if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };

            #endregion

            if (command == "Cancel")
            {
                return CancelFun(model);
            }
            else if (command == "Callback")
            {
                model = StartCallback(model);
            }
            else if (command.Trim() == "Save" || command.Trim() == "Update" || command == "Save & New" || command == "Update & New")
            {
                try
                {
                    string found = "", inactive_date = "";
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime_local(userCode);
                    ent_date = sgen.Savedate(currdate, true);

                    DataTable dtstr = new DataTable();
                    dtstr = sgen.getdata(userCode, "select * from " + model[0].Col10 + " WHERE 1=2");

                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        mq1 = " and vch_num<>'" + tmodel.Col16.Trim() + "'";
                        isedit = true;
                        vch_num = tmodel.Col16.Trim();
                        //master_id = tmodel.Col16.Trim();
                    }
                    else
                    {
                        isedit = false;
                        //mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10 + " where type ='" + model[0].Col12 + "' " + model[0].Col11 + "";
                        //master_id = sgen.genNo(userCode, mq, 3, "auto_genid");
                        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where type ='" + model[0].Col12 + "' " + model[0].Col11 + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");

                        string cond = sgen.seekval(userCode, "select col6 from " + model[0].Col10 + " where type='" + model[0].Col12 + "' and client_id='" + clientid_mst + "'  and " +
                                    "col6='" + model[0].Col21 + "' and client_unit_id='" + unitid_mst + "'" + mq1 + "", "col6");
                        if (!cond.Equals("0"))
                        {
                            //Means Already Exits.... 
                            ViewBag.vnew = "disabled='disabled'";
                            ViewBag.vedit = "disabled='disabled'";
                            ViewBag.scripCall = "showmsgJS(1,'Comm Inv No. Already Exists',2)";
                            goto END;
                        }
                    }

                    DataTable dataTable = sgen.getdata(userCode, "select  * from " + model[0].Col10 + " where 1=2");

                    if (model[0].dt1.Rows.Count > 0)
                    {
                        for (int i = 0; i < model[0].dt1.Rows.Count; i++)
                        {
                            DataRow dr = dataTable.NewRow();
                            dr["rec_id"] = "0";
                            dr["vch_num"] = vch_num;
                            dr["vch_date"] = currdate;
                            dr["date1"] = sgen.Savedate(model[0].Col22, false); //invoice date
                            dr["type"] = model[0].Col12;
                            //dr["Master_type"] = Master_Type;
                            //dr["Master_Name"] = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(model[0].Col18.ToLower());
                            dr["col6"] = model[0].Col21;   //comm inv no
                            dr["col7"] = model[0].SelectedItems1.FirstOrDefault();//Currency
                            dr["col8"] = model[0].Col23;//Comm Inv. Amt in FC
                            dr["col2"] = model[0].Col24;//Exchange Rate
                            dr["col5"] = model[0].Col17;//party code
                            //dr["col9"] = model[0].Col25;
                            dr["col12"] = model[0].Col26;//type

                            //dt====
                            dr["col11"] = model[0].dt1.Rows[i][8].ToString();
                            dr["date2"] = sgen.Savedate(model[i].dt1.Rows[i][9].ToString(), false);   //comm date
                            dr["col14"] = model[0].dt1.Rows[i][10].ToString();    //
                            dr["col3"] = model[0].dt1.Rows[i][2].ToString();    // PI No
                            dr["col17"] = model[0].dt1.Rows[i][11].ToString();    // PI BAL
                            dr = getsave(currdate, dr, model, isedit);
                            dataTable.Rows.Add(dr);
                        }
                    }
                    else
                    {
                        DataRow dr = dataTable.NewRow();

                        dr["rec_id"] = "0";
                        dr["vch_num"] = vch_num;
                        dr["vch_date"] = currdate;
                        dr["date1"] = sgen.Savedate(model[0].Col22, false); //invoice date
                        dr["type"] = model[0].Col12;
                        dr["col6"] = model[0].Col21;   //comm inv no
                        dr["col5"] = model[0].Col17;
                        dr["col7"] = model[0].SelectedItems1.FirstOrDefault();
                        dr["col8"] = model[0].Col23;
                        dr["col2"] = model[0].Col24;
                        //dr["col9"] = model[0].Col25;
                        dr["col12"] = model[0].Col26;
                        dr = getsave(currdate, dr, model, isedit);
                        dataTable.Rows.Add(dr);
                    }

                    bool Result = sgen.Update_data(userCode, dataTable, model[0].Col10.Trim(), tmodel.Col8, isedit);
                    if (Result == true)
                    {

                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col17 = "",
                            Col1 = "",
                            Col18 = "",
                            //Col9 = "EXPORT COMM INVOICE ENTRY",
                            Col10 = tm.Col10,
                            Col11 = tm.Col11,
                            Col12 = tm.Col12,
                            Col13 = tm.Col13,
                            Col14 = tm.Col14,
                            Col15 = tm.Col15,
                            Col100 = tm.Col100,
                            TList1 = mod1,
                            SelectedItems1 = new string[] { "" },
                            dt1 = (DataTable)sgen.GetSession(MyGuid, "KMRN_DT")
                        });
                        if (command.Trim().ToUpper().Equals("SAVE") || command.Trim().ToUpper().Equals("UPDATE"))
                        {
                            ViewBag.vnew = "";
                            ViewBag.vedit = "";
                            ViewBag.vsave = "disabled='disabled'";
                            ViewBag.vsavenew = "disabled='disabled'";
                            ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                        }
                        else if (command.Trim().ToUpper().Equals("SAVE & NEW") || command.Trim().ToUpper().Equals("UPDATE & NEW"))
                        {
                            try
                            {
                                Make_query("bulk_comm_inv", "Select Basic of Comm invoice", "NEW", "1", "", "", MyGuid);
                                ViewBag.scripCall = "callFoo('View');";
                                ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                            }
                            catch (Exception ex)
                            {
                                ViewBag.scripCall += "mytoast('error', 'toast-top-right', '" + ex.Message.ToString().Trim() + "');";

                            }
                        }
                    }

                    else
                    {
                        ViewBag.scripCall = "showmsgJS(3, 'Record Not Saved', 1);";
                    }
                END:;

                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1, '" + ex.Message.ToString() + "', 1);"; }

            }
            else if (command == "-")
            {
                if (model[0].dt1.Rows.Count > 1) model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                else { model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "KMRN_DT"); }
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
            }

            else if (command == "Import")
            {
                HttpPostedFileBase file1 = upd1;
                DataTable dt = new DataTable();
                if (file1.ContentLength > 1)
                {
                    string ext = Path.GetExtension(file1.FileName).ToLower();
                    if (ext == ".csv")
                    {
                        string filesavepath = AppDomain.CurrentDomain.BaseDirectory + "\\Uploads\\" + cg_com_name.Replace(" ", "")
                            + "\\file" + DateTime.Now.ToString("ddMMyyyyhhmmfff") + ext;
                        file1.SaveAs(filesavepath);
                        // Read sample data from CSV file
                        int i = 0;
                        using (CsvFileReader reader = new CsvFileReader(filesavepath))
                        {
                            CsvRow row = new CsvRow();
                            while (reader.ReadRow(row))
                            {
                                DataRow dr = dt.NewRow();
                                for (int c = 0; c < row.Count; c++)
                                {
                                    if (i == 0) { dt.Columns.Add(row[c].ToString()); }
                                    else { dr[c] = row[c].ToString(); }
                                }
                                i++;
                                dt.Rows.Add(dr);
                            }
                        }
                    }
                    try
                    {
                        dt.Rows[0].Delete();
                        string currdate = sgen.server_datetime(userCode);
                        ent_date = currdate;
                        for (int k = 0; k < dt.Rows.Count; k++)
                        {
                            mq = sgen.seekval(userCode, "Select col6 from " + tab_name + " where type = '" + type + "' " + where + " " +
                                "and upper(col6)='" + dt.Rows[k]["Comm_inv_no"].ToString() + "'", "col6");
                            if (!mq.Trim().Equals("0"))
                            {
                                ViewBag.scripCall += "showmsgJS(3, 'You already have " + dt.Rows[k][0].ToString() + " PI No saved', 2);";
                                ViewBag.vnew = "disabled='disabled'";
                                ViewBag.vedit = "disabled='disabled'";
                                ViewBag.vsave = "";
                                ViewBag.vsavenew = "";
                                return View(model);
                            }
                        }
                        DataTable dataTable = cmd_Fun.GetStructure(userCode, tab_name);
                        int inc = 0, inc1 = 0;
                        mq = "select max(to_number(vch_num)) as auto_genid from " + tab_name + " where type = '" + type + "'" + where + "";
                        inc = sgen.Make_int(sgen.genNo(userCode, mq, 6, "auto_genid"));
                        DataTable dtcurr = sgen.getdata(userCode, "SELECT master_id,master_name FROM master_setting where type = 'CTM' and " +
        "find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                        DataTable dtcust = sgen.getdata(userCode, "SELECT vch_num,c_name FROM clients_mst where type = 'BCD' and substr(vch_num,0,3)='303' and " +
        "find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow dr = dataTable.NewRow();
                            if (i == 0)
                            {
                                inc = inc + i;
                            }
                            else
                            {
                                inc = inc + 1;
                            }
                            vch_num = sgen.padlc(inc, 6);
                            dr["vch_num"] = vch_num.Trim();
                            dr["vch_date"] = ent_date;
                            dr["type"] = type.Trim();
                            if (dt.Rows[i]["Customer_code"].ToString() != "")
                            {
                                if (dtcust.Rows.Count > 0)
                                {
                                    try
                                    {
                                        DataTable dtf3 = sgen.seekval_dt(dtcust, "vch_num='" + dt.Rows[i]["Customer_code"].ToString() + "'");
                                        dr["col5"] = dtf3.Rows[0]["vch_num"].ToString();
                                    }
                                    catch (Exception)
                                    {
                                        ViewBag.vnew = "disabled='disabled'";
                                        ViewBag.vedit = "disabled='disabled'";
                                        ViewBag.vsave = "";
                                        ViewBag.vsavenew = "";
                                        ViewBag.scripCall += "showmsgJS(3, 'Customer_code :  " + dt.Rows[i]["Customer_code"].ToString() + " Is Not Valid, Please Check !', 2);";
                                        return View(model);
                                    }
                                }
                            }
                            if (dt.Rows[i]["Currency"].ToString() != "")
                            {
                                if (dtcurr.Rows.Count > 0)
                                {
                                    try
                                    {
                                        DataTable dtf2 = sgen.seekval_dt(dtcurr, "master_id='" + dt.Rows[i]["Currency"].ToString() + "'");
                                        dr["col7"] = dtf2.Rows[0]["master_id"].ToString();
                                    }
                                    catch (Exception)
                                    {
                                        ViewBag.vnew = "disabled='disabled'";
                                        ViewBag.vedit = "disabled='disabled'";
                                        ViewBag.vsave = "";
                                        ViewBag.vsavenew = "";
                                        ViewBag.scripCall = "showmsgJS(1, 'Currency : " + dt.Rows[i]["Currency"].ToString() + " does not belongs to Masters, Please Check ! ', 2);";
                                        return View(model);
                                    }
                                }
                            }
                            dr["col8"] = dt.Rows[i]["Comm_inv_amt_in_fc"].ToString();
                            dr["col6"] = dt.Rows[i]["Comm_inv_no"].ToString();
                            dr["date1"] = sgen.Make_date_S(dt.Rows[i]["Comm_inv_Date"].ToString());
                            dr["col2"] = dt.Rows[i]["Exchange_rate"].ToString();
                            dr["col12"] = "91";
                            dr["rec_id"] = "0";
                            dr["ent_by"] = userid_mst;
                            dr["ent_date"] = ent_date;
                            dr["edit_by"] = "-";
                            dr["edit_date"] = ent_date;
                            dr["client_id"] = clientid_mst;
                            dr["client_unit_id"] = unitid_mst;
                            dataTable.Rows.Add(dr);
                        }
                        res = sgen.Update_data_fast1(userCode, dataTable, tab_name, model[0].Col8, false);
                        if (res)
                        {
                            sgen.SetSession(MyGuid, "CI", null);
                            model = new List<Tmodelmain>();
                            model.Add(new Tmodelmain
                            {
                                Col3 = tm.Col3,
                                Col8 = tm.Col8,
                                Col9 = tm.Col9,
                                Col10 = tm.Col10,
                                Col11 = tm.Col11,
                                Col12 = tm.Col12,
                                Col13 = "Save",
                                Col100 = "Save & New",
                                Col121 = "S",
                                Col122 = "<u>S</u>ave",
                                Col123 = "Save & Ne<u>w</u>",
                                Col14 = tm.Col14,
                                Col15 = tm.Col15,
                            });
                            ViewBag.vnew = "";
                            ViewBag.vedit = "";
                            ViewBag.vsave = "disabled='disabled'";
                            ViewBag.vsavenew = "disabled='disabled'";
                            ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Imported Successfully');disableForm(); ";
                        }
                        else { ViewBag.scripCall = "showmsgJS(3, 'Record Not Saved', 1);"; }
                    }
                    catch (Exception err) { sgen.showmsg(1, err.Message.ToString(), 0); }
                }
            }
            ModelState.Clear();
            return View(model);

        }
        [HttpGet]
        public FileResult bcitemp(List<Tmodelmain> model, string Myguid = "")
        {
            FillMst(Myguid);
            model = (List<Tmodelmain>)sgen.GetSession(Myguid, "model");
            DataTable dtl = new DataTable();
            mq = "SELECT '' Customer_code,'' Comm_inv_no,'' Comm_inv_Date ,'' Currency,'' Comm_inv_amt_in_fc,'' Exchange_rate from Dual";
            dtl = sgen.getdata(userCode, mq);
            string cg_com_name = sgen.GetCookie(MyGuid, "cg_com_name");
            Byte[] fileBytes = sgen.Exp_to_csv_new(dtl, "bcitemp", cg_com_name);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "bcitemp");
        }
        #endregion

        #region Shipping Bill Against Export Commercial Invoice
        public ActionResult sb()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "SHIPPING BILL SUBMISSION";
            model[0].Col10 = "kc_tab";
            model[0].Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            model[0].Col12 = "ESB";

            DataTable dt = sgen.getdata(userCode, "select '' Add_Remove,'1' as  SNo ,'-' as CI_No,'-' as CI_Date,'-' Custome_name,'-'  Country,'-' as CI_FC_Amt,'0' shipment_country,'0' shipment_terms,'0' SB_No,'0' as SB_Date," +
            "'-' ship_Port,'0' LEO_Date, '-' as Remark from dual");
            model[0].dt1 = dt.Copy();
            sgen.SetSession(MyGuid, "BD_SBDT", dt.Copy());

            return View(model);
        }
        public List<Tmodelmain> new_sb(List<Tmodelmain> model)
        {
            model = getnew(model);
            return model;
        }
        [HttpPost]
        public ActionResult sb(List<Tmodelmain> model, string command, string hftable, string hfrow)
        {
            FillMst();
            DataTable dtm = sgen.settable(hftable);
            model[0].dt1 = dtm;
            var tm = model.FirstOrDefault();
            DataTable dt = new DataTable();
            if (command == "New")
            {
                try
                {
                    model = new_sb(model);
                }
                catch (Exception ex) { }
            }
            else if (command == "Cancel")
            {
                return CancelFun(model);
            }
            else if (command == "Callback")
            {
                model = StartCallback(model);
            }
            else if (command.Trim() == "Save" || command.Trim() == "Update" || command == "Save & New" || command == "Update & New")
            {
                try
                {
                    string currdate = sgen.server_datetime(userCode);
                    DataTable dtstr = new DataTable();
                    dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        isedit = true;
                        vch_num = model[0].Col16;
                    }
                    else
                    {
                        isedit = false;
                        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                        model[0].Col16 = vch_num;
                    }
                    for (int i = 0; i < model[0].dt1.Rows.Count; i++)
                    {
                        DataRow dr = dtstr.NewRow();
                        dr["vch_num"] = vch_num;
                        dr["vch_date"] = sgen.Savedate(model[0].Col17, false);
                        dr["type"] = model[0].Col12;
                        dr["col4"] = model[0].Col19;
                        dr["col5"] = model[0].dt1.Rows[i]["CI_No"].ToString();
                        dr["date3"] = sgen.Make_date_S(model[0].dt1.Rows[i]["ci_date"].ToString());
                        dr["col7"] = model[0].dt1.Rows[i]["Custome_name"].ToString();
                        dr["col8"] = model[0].dt1.Rows[i]["Country"].ToString();
                        dr["col9"] = model[0].dt1.Rows[i]["CI_FC_Amt"].ToString();
                        dr["col21"] = model[0].dt1.Rows[i]["shipment_country"].ToString();
                        dr["col22"] = model[0].dt1.Rows[i]["shipment_terms"].ToString();
                        dr["col1"] = model[0].dt1.Rows[i]["SB_No"].ToString();
                        dr["date1"] = sgen.Make_date_S(model[0].dt1.Rows[i]["SB_Date"].ToString());
                        dr["date2"] = sgen.Make_date_S(model[0].dt1.Rows[i]["LEO_Date"].ToString());
                        dr["col23"] = model[0].dt1.Rows[i]["ship_Port"].ToString();
                        dr["col31"] = model[0].dt1.Rows[i]["Remark"].ToString();
                        dr = getsave(currdate, dr, model, isedit);
                        dtstr.Rows.Add(dr);
                    }
                    bool Result = sgen.Update_data_fast1(userCode, dtstr, model[0].Col10.Trim(), tm.Col8, isedit);
                    if (Result == true)
                    {
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col10 = tm.Col10,
                            Col11 = tm.Col11,
                            Col12 = tm.Col12,
                            Col13 = "Save",
                            Col100 = "Save & New",
                            Col121 = "S",
                            Col122 = "<u>S</u>ave",
                            Col123 = "Save & Ne<u>w</u>",
                            Col14 = tm.Col14,
                            Col15 = tm.Col15,
                            Col9 = tm.Col9,
                            SelectedItems1 = new string[] { "" },
                            dt1 = ((DataTable)sgen.GetSession(MyGuid, "BD_SBDT")).Copy()
                        });
                        if (command.Trim().ToUpper().Equals("SAVE") || command.Trim().ToUpper().Equals("UPDATE"))
                        {
                            ViewBag.vnew = "";
                            ViewBag.vedit = "";
                            ViewBag.vsave = "disabled='disabled'";
                            ViewBag.vsavenew = "disabled='disabled'";
                            ViewBag.vsavenew = "disabled='disabled'";
                            ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                        }
                        else if (command.Trim().ToUpper().Equals("SAVE & NEW") || command.Trim().ToUpper().Equals("UPDATE & NEW"))
                        {
                            try
                            {
                                model = new_sb(model);
                                ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                            }
                            catch (Exception ex) { }
                        }
                    }

                    else
                    {
                        ViewBag.scripCall = "showmsgJS(3, 'Record Not Saved', 1);";
                    }
                END:;

                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1, '" + ex.Message.ToString() + "', 1);"; }
            }
            else if (command == "-")
            {
                if (model[0].dt1.Rows.Count > 1) model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                else { model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "BD_SBDT"); }
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
                ViewBag.vsavenew = "";
            }

            ModelState.Clear();
            return View(model);

        }
        #endregion

        #region Export Import Payment Entry
        public ActionResult bulk_banking()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            sgen.SetSession(MyGuid, "TR_MID", mid_mst.Trim());
            model[0].Col9 = "EXPORT PAYMENT";
            model[0].Col10 = "enx_tab2"; //TABLE NAME
            model[0].Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            model[0].Col12 = "BBK"; //Bulk Banking
            DataTable dt = sgen.getdata(userCode, "select '' invoice,'1' as  SNo ,'-' as inv_no,'-' inv_type,'-' as inv_date,'-' inv_curr,'-'  Curr_rate,'-' as inv_amt_fc,'0' inv_amt_lc,'0' amt_rcpt_fc,'0' bal_fc,'0' tobe_rcpt_fc from dual");
            model[0].dt1 = dt;
            sgen.SetSession(MyGuid, "BLBNK_DT", dt);
            dt = sgen.getdata(userCode, "select '' Forward,'1' SNo ,'-' Dock_no ,'-' Fwd_no,'-' Fwd_date,'-' FWD_Amt ,'-' FWD_Curr,'-' Fwd_type,'-' Fwd_start,'-' Fwd_end ,'-' Fwd_gc_end,'-' Spot_Rate,'-' Premium,'-' Fwd_Rate,'0' Fwd_used_Amt,'0' Fwd_Bal_Amt,'0' Fwd_To_be_use from dual");
            model[0].dt2 = dt;
            sgen.SetSession(MyGuid, "BKFWD_DT", dt);
            model[0].Col41 = "N";
            return View(model);
        }
        [HttpPost]
        public ActionResult bulk_banking(List<Tmodelmain> model, string command, string hftable, string hfrow)
        {
            FillMst();
            DataSet ds = new DataSet();
            if (hftable.Trim() != "")
            {
                ds = sgen.setDS(hftable);
                model[0].dt1 = ds.Tables[0];
                model[0].dt2 = ds.Tables[1];
                if (model[0].dt2.Rows.Count == 0) model[0].dt2 = ((DataTable)sgen.GetSession(MyGuid, "BKFWD_DT"));
                if (model[0].dt1.Rows.Count == 0) model[0].dt1 = ((DataTable)sgen.GetSession(MyGuid, "BLBNK_DT"));
            }
            if (command == "Cancel")
            {
                return CancelFun(model);
            }
            else if (command == "Callback")
            {
                model = StartCallback(model);
            }
            else if (command.Trim() == "Save" || command.Trim() == "Update" || command == "Save & New" || command == "Update & New")
            {
                try
                {
                    string found = "", inactive_date = "";
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime_local(userCode);
                    ent_date = sgen.Savedate(currdate, true);

                    DataTable dtstr = new DataTable();
                    dtstr = sgen.getdata(userCode, "select * from " + model[0].Col10 + " WHERE 1=2");

                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        mq1 = " and vch_num<>'" + tmodel.Col16.Trim() + "'";
                        isedit = true;
                        vch_num = tmodel.Col16.Trim();
                    }
                    else
                    {
                        isedit = false;
                        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where type = '" + model[0].Col12 + "' " + model[0].Col11 + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                    }
                    DataTable dataTable = sgen.getdata(userCode, "select  * from " + model[0].Col10 + " where 1=2");
                    for (int i = 0; i < model[0].dt1.Rows.Count; i++)
                    {
                        DataRow dr = dataTable.NewRow();
                        dr["vch_num"] = vch_num;
                        dr["type"] = model[0].Col12;
                        dr["col5"] = model[0].Col17;
                        dr["col7"] = model[0].Col29.Trim(); //inv type
                        dr["col17"] = model[0].Col32.Trim(); //bank id
                        dr["col2"] = model[0].Col41.Trim(); //rbt
                        dr["col6"] = model[0].dt1.Rows[i][2].ToString();   //INVOICE NO
                        dr["col8"] = model[0].dt1.Rows[i][3].ToString();   //INVOICE Type
                        dr["col11"] = model[0].dt1.Rows[i][11].ToString();    //TOBE_RCPT_FC
                        dr = getsave(currdate, dr, model, isedit);
                        dataTable.Rows.Add(dr);
                    }
                    bool Result = sgen.Update_data_fast1(userCode, dataTable, model[0].Col10.Trim(), tmodel.Col8, isedit);
                    if (Result == true)
                    {
                        if (model[0].Col41 == "Y")
                        {
                            #region Book Forward
                            dtstr = new DataTable();
                            dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                            for (int j = 0; j < model[0].dt2.Rows.Count; j++)
                            {
                                DataRow dr = dtstr.NewRow();
                                dr["vch_num"] = model[0].Col16.Trim();
                                dr["vch_date"] = currdate;
                                dr["type"] = "BBW";
                                dr["col2"] = model[0].Col41.Trim(); //rbt
                                dr["col13"] = model[0].dt2.Rows[j][2].ToString();    //fwd vch_num
                                dr["col4"] = model[0].dt2.Rows[j][3].ToString();    //fwd No
                                dr["col16"] = model[0].dt2.Rows[j][16].ToString();    //TOBE_used
                                dr = getsave(currdate, dr, model, isedit);
                                dtstr.Rows.Add(dr);
                            }
                            res = sgen.Update_data_fast1(userCode, dtstr, model[0].Col10, tmodel.Col85, isedit);
                            #endregion
                        }
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col13 = "Save",
                            Col9 = "EXPORT PAYMENT",
                            Col10 = tmodel.Col10,
                            Col11 = tmodel.Col11,
                            Col12 = tmodel.Col12,
                            Col14 = tmodel.Col14,
                            Col15 = tmodel.Col15,
                            Col100 = "Save & New",
                            Col121 = "S",
                            Col122 = "<u>S</u>ave",
                            Col123 = "Save & Ne<u>w</u>",
                            dt1 = (DataTable)sgen.GetSession(MyGuid, "BLBNK_DT"),
                            dt2 = (DataTable)sgen.GetSession(MyGuid, "BKFWD_DT"),
                            Col41 = "N",
                        });
                        if (command.Trim().ToUpper().Equals("SAVE") || command.Trim().ToUpper().Equals("UPDATE"))
                        {
                            ViewBag.vnew = "";
                            ViewBag.vedit = "";
                            ViewBag.vsave = "disabled='disabled'";
                            ViewBag.vsavenew = "disabled='disabled'";
                            ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                        }
                        else if (command.Trim().ToUpper().Equals("SAVE & NEW") || command.Trim().ToUpper().Equals("UPDATE & NEW"))
                        {
                            try
                            {
                                Make_query("bulk_banking", "Select Basis Of Invoice", "NEW", "1", "", "", MyGuid);
                                ViewBag.scripCall = "callFoo('View');";
                                ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                            }
                            catch (Exception ex) { }
                        }
                    }
                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1, '" + ex.Message.ToString() + "', 1);"; }

            }

            else if (command == "-")
            {
                if (model[0].dt1.Rows.Count > 1) model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                else { model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "BLBNK_DT"); }
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
            }
            else if (command == "--")
            {
                if (model[0].dt2.Rows.Count > 1) model[0].dt2.Rows.RemoveAt(sgen.Make_int(hfrow));
                else { model[0].dt2 = (DataTable)sgen.GetSession(MyGuid, "BKFWD_DT"); }
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
            }

            ModelState.Clear();
            return View(model);

        }
        #endregion

        #region Export Incentive Master
        public ActionResult exp_inc_m()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<SelectListItem> mod2 = new List<SelectListItem>();
            model[0].Col9 = "EXPORT INCENTIVE MASTER";
            model[0].Col10 = "enx_tab"; //TABLE NAME
            model[0].Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            model[0].Col12 = "INC"; //TYPE
            model[0].TList1 = mod1;
            model[0].TList2 = mod2;
            model[0].SelectedItems1 = new string[] { "" };
            model[0].SelectedItems2 = new string[] { "" };
            return View(model);
        }
        public List<Tmodelmain> new_exp_inc_m(List<Tmodelmain> model)
        {
            model = getnew(model);
            var tm = model.FirstOrDefault();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<SelectListItem> mod2 = new List<SelectListItem>();
            model = getnew(model);
            #region Inc type
            TempData[MyGuid + "_Tlist1"] = model[0].TList1 = mod1 = cmd_Fun.incentive_type(userCode, unitid_mst);
            #endregion
            #region Country 
            TempData[MyGuid + "_Tlist2"] = model[0].TList2 = mod2 = cmd_Fun.country(userCode, unitid_mst);
            #endregion
            return model;
        }
        [HttpPost]
        public ActionResult exp_inc_m(List<Tmodelmain> model, string command)
        {
            FillMst();
            var tm = model.FirstOrDefault();
            #region ddl
            List<SelectListItem> mod1 = new List<SelectListItem>();
            TempData[MyGuid + "_Tlist1"] = model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_Tlist1"];
            if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };

            List<SelectListItem> mod2 = new List<SelectListItem>();
            TempData[MyGuid + "_Tlist2"] = model[0].TList2 = (List<SelectListItem>)TempData[MyGuid + "_Tlist2"];
            if (tm.SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
            #endregion
            Master_Type = "INCNTIVE MASTER";
            if (command == "New")
            {
                model = new_exp_inc_m(model);
            }
            else if (command == "Cancel")
            {
                return CancelFun(model);
            }
            else if (command == "Callback")
            {
                model = StartCallback(model);
            }
            else if (command.Trim() == "Save" || command.Trim() == "Update" || command == "Save & New" || command == "Update & New")
            {
                try
                {
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime_local(userCode);
                    ent_date = sgen.Savedate(currdate, true);
                    DataTable dtstr = new DataTable();
                    dtstr = sgen.getdata(userCode, "select * from " + model[0].Col10 + " WHERE 1=2");

                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        mq1 = " and vch_num<>'" + tmodel.Col16.Trim() + "'";
                        isedit = true;
                        vch_num = tmodel.Col16.Trim();
                    }
                    else
                    {
                        isedit = false;
                        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where type = '" + model[0].Col12 + "' " + model[0].Col11 + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                    }
                    DataTable dataTable = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    DataRow dr = dataTable.NewRow();
                    dr["vch_num"] = vch_num;
                    dr["vch_date"] = currdate;
                    dr["type"] = model[0].Col12;
                    dr["col7"] = model[0].SelectedItems1.FirstOrDefault();
                    dr["col8"] = string.Join(",", model[0].SelectedItems2);
                    dr["col9"] = model[0].Col18.Trim();
                    dr["date1"] = sgen.Make_date_S(model[0].Col19);
                    dr["date2"] = sgen.Make_date_S(model[0].Col20);
                    dr = getsave(ent_date, dr, model, isedit);
                    dataTable.Rows.Add(dr);
                    bool Result = sgen.Update_data_fast1(userCode, dataTable, model[0].Col10.Trim(), tmodel.Col8, isedit);
                    if (Result == true)
                    {
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col10 = tm.Col10,
                            Col11 = tm.Col11,
                            Col12 = tm.Col12,
                            Col13 = "Save",
                            Col100 = "Save & New",
                            Col121 = "S",
                            Col122 = "<u>S</u>ave",
                            Col123 = "Save & Ne<u>w</u>",
                            Col14 = tm.Col14,
                            Col15 = tm.Col15,
                            Col9 = tm.Col9,
                            TList1 = mod1,
                            SelectedItems1 = new string[] { "" },
                            TList2 = mod2,
                            SelectedItems2 = new string[] { "" },
                        });
                        if (command.Trim().ToUpper().Equals("SAVE") || command.Trim().ToUpper().Equals("UPDATE"))
                        {
                            ViewBag.vnew = "";
                            ViewBag.vedit = "";
                            ViewBag.vsave = "disabled='disabled'";
                            ViewBag.vsavenew = "disabled='disabled'";
                            ViewBag.vsavenew = "disabled='disabled'";
                            ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                        }
                        else if (command.Trim().ToUpper().Equals("SAVE & NEW") || command.Trim().ToUpper().Equals("UPDATE & NEW"))
                        {
                            try
                            {
                                model = new_exp_inc_m(model);
                                ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                            }
                            catch (Exception ex) { }
                        }
                    }
                    else
                    {
                        ViewBag.scripCall = "showmsgJS(3, 'Record Not Saved', 1);";
                    }

                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1, '" + ex.Message.ToString() + "', 1);"; }

            }
            ModelState.Clear();
            return View(model);

        }
        #endregion

        #region Export Incentive Due
        public ActionResult due_inc_file()
        {

            FillMst();
            chkRef();

            ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.vsave = "disabled='disabled'";
            ViewBag.scripCall = "disableForm();";
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);

            sgen.SetSession(MyGuid, "TR_MID", mid_mst.Trim());
            Tmodelmain tm1 = new Tmodelmain();
            List<Tmodelmain> model = new List<Tmodelmain>();

            tm1.Col9 = "EXPORT INCENTIVE DUE";
            tm1.Col10 = "master_setting"; //TABLE NAME
            tm1.Col13 = "Save";

            tm1.Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            tm1.Col12 = "ECC"; //TYPE
            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = MyGuid.Trim();

            List<SelectListItem> mod1 = new List<SelectListItem>();
            tm1.TList1 = mod1;
            tm1.SelectedItems1 = new string[] { "" };

            DataTable dt = sgen.getdata(userCode, "select '' Item,'1' as  SNo ,'-' as Icode,'-' as IName,'-' PARTNO,'-'  HSN,'-' as UOM,'0' TaxRate,'0' Qty_Chl,'0' Qty_In,'0' as IRate," +
               "'-' disc_type,'0' as disc_rate,'0' IAmount,'0' TaxAmount,'0' discAmount,'0' LineAmount,'-' as Remark,'0' PO_Number,'-' PO_Date from dual");
            tm1.dt1 = dt;
            sgen.SetSession(MyGuid, "KMRN_DT", dt);
            sgen.SetSession(MyGuid, "KMRNTYPE", null);
            model.Add(tm1);

            return View(model);
        }
        [HttpPost]
        public ActionResult due_inc_file(List<Tmodelmain> model, string command, string hftable, string hfrow)
        {
            FillMst();
            DataTable dtm = sgen.settable(hftable);
            model[0].dt1 = dtm;

            var tm = model.FirstOrDefault();
            DataTable dt = new DataTable();

            #region ddl
            List<SelectListItem> mod1 = new List<SelectListItem>();

            TempData[MyGuid + "_TList1"] = model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];

            if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };

            #endregion

            if (command == "New")
            {
                sgen.SetSession(MyGuid, "EDMODE", "N");
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
                ViewBag.scripCall = "enableForm();";
                // mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10 + " where type in  ('DDECC' ,'ECC')" + model[0].Col11 + "";
                // master_id = sgen.genNo(userCode, mq, 3, "auto_genid");

                #region Bank Name

                mq1 = "select master_id, master_name from master_setting where  type='CTM' " + model[0].Col11 + "";

                dt = sgen.getdata(userCode, mq1);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        mod1.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                    }
                }
                TempData[MyGuid + "_Tlist1"] = model[0].TList1 = mod1;
                #endregion

            }


            else if (command == "Cancel")
            {
                return CancelFun(model);
            }
            else if (command == "Callback")
            {
                if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
                model = CallbackFun(btnval, "N", "bill_of_entry", "Exim", model);
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
            }
            else if (command.Trim() == "Save" || command.Trim() == "Update")
            {
                try
                {
                    string found = "", inactive_date = "";
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime_local(userCode);
                    ent_date = sgen.Savedate(currdate, true);

                    DataTable dtstr = new DataTable();
                    dtstr = sgen.getdata(userCode, "select * from " + model[0].Col10 + " WHERE 1=2");

                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        mq1 = " and vch_num<>'" + tmodel.Col23.Trim() + "'";
                        isedit = true;
                        vch_num = tmodel.Col23.Trim();
                        master_id = tmodel.Col16.Trim();

                    }
                    else
                    {
                        isedit = false;



                        mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10 + " where type in  ('DDECC','ECC')" + model[0].Col11 + "";
                        master_id = sgen.genNo(userCode, mq, 3, "auto_genid");
                        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where type in   ('DDECC','ECC')" + model[0].Col11 + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");

                    }

                    found = sgen.getstring(userCode, "select master_name from " + model[0].Col10 + " a where upper(master_name)= upper('" + model[0].Col18 + "')" +
                        " and type in  ('DDECC','ECC') " + model[0].Col11 + " " + mq1 + "");
                    if (found != "")
                    {

                        ViewBag.scripCall = "showmsgJS(1, 'Data Already Exists', 2);";
                        goto END;
                    }

                    if (model[0].Col22 == "Y") { status = "Y"; inactive_date = currdate; model[0].Col12 = "ECC"; }
                    else { status = "N"; inactive_date = currdate; model[0].Col12 = "DDECC"; }
                    DataTable dataTable = sgen.getdata(userCode, "select  * from " + model[0].Col10 + " where 1=2");
                    DataRow dr = dataTable.NewRow();

                    dr["vch_num"] = vch_num;
                    dr["vch_date"] = sgen.Savedate(model[0].Col17, false);
                    dr["type"] = model[0].Col12;
                    dr["Master_type"] = Master_Type;
                    dr["Master_Name"] = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(model[0].Col18.ToLower());
                    dr["master_id"] = master_id;
                    dr["Section_Strength"] = model[0].Col20.Trim();
                    dr["classid"] = model[0].Col19.Trim();
                    dr["sectionid"] = model[0].Col21.Trim();
                    dr["Status"] = status;
                    dr["Inactive_date"] = sgen.Savedate(inactive_date, false);


                    dr["client_id"] = clientid_mst;
                    dr["client_unit_id"] = unitid_mst;
                    if (isedit)
                    {
                        dr["master_entby"] = model[0].Col3.Trim();
                        dr["master_entdate"] = model[0].Col4.Trim();
                        dr["master_editby"] = userid_mst;
                        dr["master_editdate"] = currdate;
                        dr["client_id"] = model[0].Col1;
                        dr["client_unit_id"] = model[0].Col2;
                        dr["rec_id"] = model[0].Col7;

                    }
                    else
                    {
                        dr["rec_id"] = "0";
                        dr["master_editby"] = "-";
                        dr["master_editdate"] = currdate;
                        dr["master_entby"] = userid_mst;
                        dr["master_entdate"] = currdate;
                        dr["client_id"] = clientid_mst;
                        dr["client_unit_id"] = unitid_mst;
                    }

                    dataTable.Rows.Add(dr);

                    bool Result = sgen.Update_data(userCode, dataTable, model[0].Col10.Trim(), tmodel.Col8, isedit);
                    if (Result == true)
                    {
                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        //ViewBag.scripCall = "showmsgJS(1, 'Data Saved Successfully', 1);disableForm();";
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";

                        model = new List<Tmodelmain>();
                        tmodel.Col16 = "";
                        tmodel.Col17 = "";
                        tmodel.Col18 = "";
                        tmodel.Col19 = "";
                        tmodel.Col20 = "";
                        tmodel.Col21 = "";
                        tmodel.Col13 = "Save";
                        tmodel.Col9 = "FILLING OF EXPORT INCENTIVE";
                        model.Add(tmodel);


                    }

                    else
                    {
                        ViewBag.scripCall = "showmsgJS(3, 'Record Not Saved', 1);";
                    }
                END:;

                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1, '" + ex.Message.ToString() + "', 1);"; }

            }
            else if (command == "-")
            {
                if (model[0].dt1.Rows.Count > 1) model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                else { model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "KMRN_DT"); }
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
            }


            ModelState.Clear();
            return View(model);

        }
        #endregion

        #region Export Incentive Applied
        public ActionResult inc_app()
        {
            FillMst();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "EXPORT INCENTIVE APPLY";
            model[0].Col10 = "enx_tab2"; //TABLE NAME
            model[0].Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            model[0].Col12 = "INP"; //TYPE
            model[0].TList1 = mod1;
            model[0].SelectedItems1 = new string[] { "" };
            DataTable dt = sgen.getdata(userCode, "select '' ADD_REMOVE,'1' as  SNo ,'-' as CUST_NAME,'-' as SHIP_COUNTRY,'-' SB_TERM,'-' SB_NO,'-'  SB_DATE,'-' as LEO_DATE,'0' BRC_BANK,'-' BRC_NO,'0' BRC_AMT,'0' as INC_DUE," +
               "'-' INC_TYPE,'0' as INC_APPLY,'-' as Remark from dual");
            model[0].dt1 = dt;
            sgen.SetSession(MyGuid, "BD_INAPPDT", dt);
            return View(model);
        }
        List<Tmodelmain> new_inc_app(List<Tmodelmain> model)
        {
            List<SelectListItem> mod1 = new List<SelectListItem>();
            model = getnew(model);
            TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1 = cmd_Fun.incentive_type(userCode, unitid_mst);
            return model;
        }
        [HttpPost]
        public ActionResult inc_app(List<Tmodelmain> model, string command, string hftable, string hfrow)
        {
            FillMst();
            DataTable dtm = sgen.settable(hftable);
            model[0].dt1 = dtm;
            var tm = model.FirstOrDefault();
            DataTable dt = new DataTable();

            #region ddl
            List<SelectListItem> mod1 = new List<SelectListItem>();
            TempData[MyGuid + "_TList1"] = model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
            if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
            #endregion
            if (command == "New")
            {
                model = new_inc_app(model);
            }
            else if (command == "Cancel")
            {
                return CancelFun(model);
            }
            else if (command == "Callback")
            {
                model = StartCallback(model);
            }
            else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
            {
                try
                {
                    string currdate = sgen.server_datetime(userCode);
                    DataTable dtstr = new DataTable();
                    dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        isedit = true;
                        vch_num = model[0].Col16.Trim();
                    }
                    else
                    {
                        isedit = false;
                        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where type = '" + model[0].Col12.Trim() + "' " + model[0].Col11 + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                        model[0].Col16 = vch_num;
                    }
                    for (int i = 0; i < model[0].dt1.Rows.Count; i++)
                    {
                        DataRow dr = dtstr.NewRow();
                        dr["vch_num"] = vch_num;
                        dr["vch_date"] = sgen.Make_date_S(model[0].Col21);
                        dr["type"] = model[0].Col12;
                        dr["col5"] = model[0].Col19;//bankid
                        dr["col20"] = model[0].Col17;//inc number
                        dr["col21"] = model[0].SelectedItems1.FirstOrDefault();//inc type
                        dr["col22"] = model[0].Col22;//inc amount
                        dr["col6"] = model[0].dt1.Rows[i]["SB_NO"].ToString();   //sb_no
                        dr["date1"] = sgen.Make_date_S(model[0].dt1.Rows[i]["sb_date"].ToString());   //sb date
                        dr["col7"] = model[0].dt1.Rows[i]["brc_no"].ToString();   
                        dr["col8"] = model[0].dt1.Rows[i]["BRC_AMT"].ToString();   
                        dr["col9"] = model[0].dt1.Rows[i]["INC_DUE"].ToString();  
                        dr["col10"] = model[0].dt1.Rows[i]["INC_TYPE"].ToString();
                        dr["col19"] = model[0].dt1.Rows[i]["INC_APPLY"].ToString();
                        dr["col2"] = model[0].dt1.Rows[i]["Remark"].ToString();   
                       // dr["col18"] = model[0].dt1.Rows[i]["BRC_NO"].ToString();  
                        dr = getsave(currdate, dr, model, isedit);
                        dtstr.Rows.Add(dr);
                    }
                    bool Result = sgen.Update_data_fast1(userCode, dtstr, model[0].Col10.Trim(), tm.Col8, isedit);
                    if (Result == true)
                    {
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col10 = tm.Col10,
                            Col9 = tm.Col9,
                            Col11 = tm.Col11,
                            Col12 = tm.Col12,
                            Col13 = "Save",
                            Col100 = "Save & New",
                            Col121 = "S",
                            Col122 = "<u>S</u>ave",
                            Col123 = "Save & Ne<u>w</u>",
                            Col14 = tm.Col14,
                            Col15 = tm.Col15,
                            TList1 = mod1,
                            SelectedItems1 = new string[] { "" },
                            dt1 = (DataTable)sgen.GetSession(MyGuid, "BD_INAPPDT")
                        });
                        if (command.Trim().ToUpper().Equals("SAVE") || command.Trim().ToUpper().Equals("UPDATE"))
                        {
                            ViewBag.vnew = "";
                            ViewBag.vedit = "";
                            ViewBag.vsave = "disabled='disabled'";
                            ViewBag.vsavenew = "disabled='disabled'";
                            ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                        }
                        else if (command.Trim().ToUpper().Equals("SAVE & NEW") || command.Trim().ToUpper().Equals("UPDATE & NEW"))
                        {
                            try
                            {
                                model = new_inc_app(model);
                                ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                            }
                            catch (Exception ex) { }
                        }

                    }
                    else
                    {
                        ViewBag.scripCall = "showmsgJS(3, 'Record Not Saved', 1);";
                    }
                END:;

                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1, '" + ex.Message.ToString() + "', 1);"; }

            }
            else if (command == "-")
            {
                if (model[0].dt1.Rows.Count > 1) model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                else { model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "BD_INAPPDT"); }
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
                ViewBag.vsavenew = "";
            }


            ModelState.Clear();
            return View(model);

        }
        #endregion

        #region Export Incentive Received
        public ActionResult inc_rec()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "EXPORT INCENTIVE RECEIVE";
            model[0].Col10 = "master_setting"; //TABLE NAME
            model[0].Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            model[0].Col12 = "ECC"; //TYPE
            List<SelectListItem> mod1 = new List<SelectListItem>();
            model[0].TList1 = mod1;
            model[0].SelectedItems1 = new string[] { "" };
            return View(model);
        }
        [HttpPost]
        public ActionResult inc_rec(List<Tmodelmain> model, string command, string hftable, string hfrow)
        {
            FillMst();
            DataTable dtm = sgen.settable(hftable);
            model[0].dt1 = dtm;

            var tm = model.FirstOrDefault();
            DataTable dt = new DataTable();

            #region ddl
            List<SelectListItem> mod1 = new List<SelectListItem>();

            TempData[MyGuid + "_TList1"] = model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];

            if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };

            #endregion

            if (command == "New")
            {
                model = getnew(model);
                #region Incentive File No.

                mq1 = "select master_id, master_name from master_setting where type='CTM' " + model[0].Col11 + "";

                dt = sgen.getdata(userCode, mq1);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        mod1.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                    }
                }
                TempData[MyGuid + "_Tlist1"] = model[0].TList1 = mod1;
                #endregion
            }
            else if (command == "Cancel")
            {
                return CancelFun(model);
            }
            else if (command == "Callback")
            {
                model = StartCallback(model);
            }
            else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
            {
                try
                {
                    string found = "", inactive_date = "";
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime_local(userCode);
                    ent_date = sgen.Savedate(currdate, true);

                    DataTable dtstr = new DataTable();
                    dtstr = sgen.getdata(userCode, "select * from " + model[0].Col10 + " WHERE 1=2");

                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        mq1 = " and vch_num<>'" + tmodel.Col23.Trim() + "'";
                        isedit = true;
                        vch_num = tmodel.Col23.Trim();
                        master_id = tmodel.Col16.Trim();

                    }
                    else
                    {
                        isedit = false;



                        mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10 + " where type in  ('DDECC','ECC')" + model[0].Col11 + "";
                        master_id = sgen.genNo(userCode, mq, 3, "auto_genid");
                        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where type in   ('DDECC','ECC')" + model[0].Col11 + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");

                    }

                    found = sgen.getstring(userCode, "select master_name from " + model[0].Col10 + " a where upper(master_name)= upper('" + model[0].Col18 + "')" +
                        " and type in  ('DDECC','ECC') " + model[0].Col11 + " " + mq1 + "");
                    if (found != "")
                    {

                        ViewBag.scripCall = "showmsgJS(1, 'Data Already Exists', 2);";
                        goto END;
                    }

                    if (model[0].Col22 == "Y") { status = "Y"; inactive_date = currdate; model[0].Col12 = "ECC"; }
                    else { status = "N"; inactive_date = currdate; model[0].Col12 = "DDECC"; }
                    DataTable dataTable = sgen.getdata(userCode, "select  * from " + model[0].Col10 + " where 1=2");
                    DataRow dr = dataTable.NewRow();

                    dr["vch_num"] = vch_num;
                    dr["vch_date"] = sgen.Savedate(model[0].Col17, false);
                    dr["type"] = model[0].Col12;
                    dr["Master_type"] = Master_Type;
                    dr["Master_Name"] = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(model[0].Col18.ToLower());
                    dr["master_id"] = master_id;
                    dr["Section_Strength"] = model[0].Col20.Trim();
                    dr["classid"] = model[0].Col19.Trim();
                    dr["sectionid"] = model[0].Col21.Trim();
                    dr["Status"] = status;
                    dr["Inactive_date"] = sgen.Savedate(inactive_date, false);


                    dr["client_id"] = clientid_mst;
                    dr["client_unit_id"] = unitid_mst;
                    if (isedit)
                    {
                        dr["master_entby"] = model[0].Col3.Trim();
                        dr["master_entdate"] = model[0].Col4.Trim();
                        dr["master_editby"] = userid_mst;
                        dr["master_editdate"] = currdate;
                        dr["client_id"] = model[0].Col1;
                        dr["client_unit_id"] = model[0].Col2;
                        dr["rec_id"] = model[0].Col7;

                    }
                    else
                    {
                        dr["rec_id"] = "0";
                        dr["master_editby"] = "-";
                        dr["master_editdate"] = currdate;
                        dr["master_entby"] = userid_mst;
                        dr["master_entdate"] = currdate;
                        dr["client_id"] = clientid_mst;
                        dr["client_unit_id"] = unitid_mst;
                    }

                    dataTable.Rows.Add(dr);

                    bool Result = sgen.Update_data(userCode, dataTable, model[0].Col10.Trim(), tmodel.Col8, isedit);
                    if (Result == true)
                    {
                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        //ViewBag.scripCall = "showmsgJS(1, 'Data Saved Successfully', 1);disableForm();";
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";

                        model = new List<Tmodelmain>();
                        tmodel.Col16 = "";
                        tmodel.Col17 = "";
                        tmodel.Col18 = "";
                        tmodel.Col19 = "";
                        tmodel.Col20 = "";
                        tmodel.Col21 = "";
                        tmodel.Col13 = "Save";
                        tmodel.Col9 = "EXPORT INCENTIVE FILLING";
                        model.Add(tmodel);


                    }

                    else
                    {
                        ViewBag.scripCall = "showmsgJS(3, 'Record Not Saved', 1);";
                    }
                END:;

                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1, '" + ex.Message.ToString() + "', 1);"; }

            }
            else if (command == "-")
            {
                if (model[0].dt1.Rows.Count > 1) model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                else { model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "KMRN_DT"); }
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
            }


            ModelState.Clear();
            return View(model);

        }
        #endregion

        #region Export Incentive Utilise
        public ActionResult inc_use()
        {

            FillMst();
            chkRef();

            ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.vsave = "disabled='disabled'";
            ViewBag.scripCall = "disableForm();";
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);

            sgen.SetSession(MyGuid, "TR_MID", mid_mst.Trim());
            Tmodelmain tm1 = new Tmodelmain();
            List<Tmodelmain> model = new List<Tmodelmain>();
            tm1.Col9 = "EXPORT COMM INVOICE ENTRY";
            tm1.Col10 = "master_setting"; //TABLE NAME
            tm1.Col13 = "Save";

            tm1.Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            tm1.Col12 = "ECC"; //TYPE
            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = MyGuid.Trim();


            model.Add(tm1);

            return View(model);
        }
        [HttpPost]
        public ActionResult inc_use(List<Tmodelmain> model, string command, string hfrow, string hftable)
        {
            FillMst();
            DataTable dtm = sgen.settable(hftable);
            model[0].dt1 = dtm;
            Master_Type = "continents";
            if (command == "New")
            {
                sgen.SetSession(MyGuid, "EDMODE", "N");
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
                ViewBag.scripCall = "enableForm();";
                mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10 + " where type in  ('DDECC' ,'ECC')" + model[0].Col11 + "";
                master_id = sgen.genNo(userCode, mq, 3, "auto_genid");


                model[0].Col16 = master_id;

                model[0].Col22 = "Y";
            }


            else if (command == "Cancel")
            {
                return CancelFun(model);
            }
            else if (command == "Callback")
            {
                if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
                model = CallbackFun(btnval, "N", "bill_of_entry", "Exim", model);
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
            }
            else if (command.Trim() == "Save" || command.Trim() == "Update")
            {
                try
                {
                    string found = "", inactive_date = "";
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime_local(userCode);
                    ent_date = sgen.Savedate(currdate, true);

                    DataTable dtstr = new DataTable();
                    dtstr = sgen.getdata(userCode, "select * from " + model[0].Col10 + " WHERE 1=2");

                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        mq1 = " and vch_num<>'" + tmodel.Col23.Trim() + "'";
                        isedit = true;
                        vch_num = tmodel.Col23.Trim();
                        master_id = tmodel.Col16.Trim();

                    }
                    else
                    {
                        isedit = false;



                        mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10 + " where type in  ('DDECC','ECC')" + model[0].Col11 + "";
                        master_id = sgen.genNo(userCode, mq, 3, "auto_genid");
                        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where type in   ('DDECC','ECC')" + model[0].Col11 + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");

                    }

                    found = sgen.getstring(userCode, "select master_name from " + model[0].Col10 + " a where upper(master_name)= upper('" + model[0].Col18 + "')" +
                        " and type in  ('DDECC','ECC') " + model[0].Col11 + " " + mq1 + "");
                    if (found != "")
                    {

                        ViewBag.scripCall = "showmsgJS(1, 'Data Already Exists', 2);";
                        goto END;
                    }

                    if (model[0].Col22 == "Y") { status = "Y"; inactive_date = currdate; model[0].Col12 = "ECC"; }
                    else { status = "N"; inactive_date = currdate; model[0].Col12 = "DDECC"; }
                    DataTable dataTable = sgen.getdata(userCode, "select  * from " + model[0].Col10 + " where 1=2");
                    DataRow dr = dataTable.NewRow();

                    dr["vch_num"] = vch_num;
                    dr["vch_date"] = sgen.Savedate(model[0].Col17, false);
                    dr["type"] = model[0].Col12;
                    dr["Master_type"] = Master_Type;
                    dr["Master_Name"] = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(model[0].Col18.ToLower());
                    dr["master_id"] = master_id;
                    dr["Section_Strength"] = model[0].Col20.Trim();
                    dr["classid"] = model[0].Col19.Trim();
                    dr["sectionid"] = model[0].Col21.Trim();
                    dr["Status"] = status;
                    dr["Inactive_date"] = sgen.Savedate(inactive_date, false);


                    dr["client_id"] = clientid_mst;
                    dr["client_unit_id"] = unitid_mst;
                    if (isedit)
                    {
                        dr["master_entby"] = model[0].Col3.Trim();
                        dr["master_entdate"] = model[0].Col4.Trim();
                        dr["master_editby"] = userid_mst;
                        dr["master_editdate"] = currdate;
                        dr["client_id"] = model[0].Col1;
                        dr["client_unit_id"] = model[0].Col2;
                        dr["rec_id"] = model[0].Col7;

                    }
                    else
                    {
                        dr["rec_id"] = "0";
                        dr["master_editby"] = "-";
                        dr["master_editdate"] = currdate;
                        dr["master_entby"] = userid_mst;
                        dr["master_entdate"] = currdate;
                        dr["client_id"] = clientid_mst;
                        dr["client_unit_id"] = unitid_mst;
                    }

                    dataTable.Rows.Add(dr);

                    bool Result = sgen.Update_data(userCode, dataTable, model[0].Col10.Trim(), tmodel.Col8, isedit);
                    if (Result == true)
                    {
                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        //ViewBag.scripCall = "showmsgJS(1, 'Data Saved Successfully', 1);disableForm();";
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";

                        model = new List<Tmodelmain>();
                        tmodel.Col16 = "";
                        tmodel.Col17 = "";
                        tmodel.Col18 = "";
                        tmodel.Col19 = "";
                        tmodel.Col20 = "";
                        tmodel.Col21 = "";
                        tmodel.Col13 = "Save";
                        tmodel.Col9 = "BILL OF ENTRY";
                        model.Add(tmodel);


                    }

                    else
                    {
                        ViewBag.scripCall = "showmsgJS(3, 'Record Not Saved', 1);";
                    }
                END:;

                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1, '" + ex.Message.ToString() + "', 1);"; }

            }
            else if (command == "-")
            {
                if (model[0].dt1.Rows.Count > 1) model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                else { model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "KMRN_DT"); }
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
            }


            ModelState.Clear();
            return View(model);

        }
        #endregion

        #region PI link with Comm
        public ActionResult pi_comm()
        {

            FillMst();
            chkRef();

            ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.vsave = "disabled='disabled'";
            ViewBag.scripCall = "disableForm();";
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);

            sgen.SetSession(MyGuid, "TR_MID", mid_mst.Trim());
            Tmodelmain tm1 = new Tmodelmain();
            List<Tmodelmain> model = new List<Tmodelmain>();
            tm1.Col9 = "PI LINK WITH COMM.";
            tm1.Col10 = "master_setting"; //TABLE NAME
            tm1.Col13 = "Save";

            tm1.Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            tm1.Col12 = "ECC"; //TYPE
            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = MyGuid.Trim();


            model.Add(tm1);

            return View(model);
        }
        [HttpPost]
        public ActionResult pi_comm(List<Tmodelmain> model, string command)
        {
            FillMst();
            Master_Type = "continents";
            if (command == "New")
            {
                sgen.SetSession(MyGuid, "EDMODE", "N");
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
                ViewBag.scripCall = "enableForm();";
                mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10 + " where type in  ('DDECC' ,'ECC')" + model[0].Col11 + "";
                master_id = sgen.genNo(userCode, mq, 3, "auto_genid");


                model[0].Col16 = master_id;

                model[0].Col22 = "Y";
            }


            else if (command == "Cancel")
            {
                return CancelFun(model);
            }
            else if (command == "Callback")
            {
                if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
                model = CallbackFun(btnval, "N", "bill_of_entry", "Exim", model);
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
            }
            else if (command.Trim() == "Save" || command.Trim() == "Update")
            {
                try
                {
                    string found = "", inactive_date = "";
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime_local(userCode);
                    ent_date = sgen.Savedate(currdate, true);

                    DataTable dtstr = new DataTable();
                    dtstr = sgen.getdata(userCode, "select * from " + model[0].Col10 + " WHERE 1=2");

                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        mq1 = " and vch_num<>'" + tmodel.Col23.Trim() + "'";
                        isedit = true;
                        vch_num = tmodel.Col23.Trim();
                        master_id = tmodel.Col16.Trim();

                    }
                    else
                    {
                        isedit = false;



                        mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10 + " where type in  ('DDECC','ECC')" + model[0].Col11 + "";
                        master_id = sgen.genNo(userCode, mq, 3, "auto_genid");
                        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where type in   ('DDECC','ECC')" + model[0].Col11 + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");

                    }

                    found = sgen.getstring(userCode, "select master_name from " + model[0].Col10 + " a where upper(master_name)= upper('" + model[0].Col18 + "')" +
                        " and type in  ('DDECC','ECC') " + model[0].Col11 + " " + mq1 + "");
                    if (found != "")
                    {

                        ViewBag.scripCall = "showmsgJS(1, 'Data Already Exists', 2);";
                        goto END;
                    }

                    if (model[0].Col22 == "Y") { status = "Y"; inactive_date = currdate; model[0].Col12 = "ECC"; }
                    else { status = "N"; inactive_date = currdate; model[0].Col12 = "DDECC"; }
                    DataTable dataTable = sgen.getdata(userCode, "select  * from " + model[0].Col10 + " where 1=2");
                    DataRow dr = dataTable.NewRow();

                    dr["vch_num"] = vch_num;
                    dr["vch_date"] = sgen.Savedate(model[0].Col17, false);
                    dr["type"] = model[0].Col12;
                    dr["Master_type"] = Master_Type;
                    dr["Master_Name"] = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(model[0].Col18.ToLower());
                    dr["master_id"] = master_id;
                    dr["Section_Strength"] = model[0].Col20.Trim();
                    dr["classid"] = model[0].Col19.Trim();
                    dr["sectionid"] = model[0].Col21.Trim();
                    dr["Status"] = status;
                    dr["Inactive_date"] = sgen.Savedate(inactive_date, false);


                    dr["client_id"] = clientid_mst;
                    dr["client_unit_id"] = unitid_mst;
                    if (isedit)
                    {
                        dr["master_entby"] = model[0].Col3.Trim();
                        dr["master_entdate"] = model[0].Col4.Trim();
                        dr["master_editby"] = userid_mst;
                        dr["master_editdate"] = currdate;
                        dr["client_id"] = model[0].Col1;
                        dr["client_unit_id"] = model[0].Col2;
                        dr["rec_id"] = model[0].Col7;

                    }
                    else
                    {
                        dr["rec_id"] = "0";
                        dr["master_editby"] = "-";
                        dr["master_editdate"] = currdate;
                        dr["master_entby"] = userid_mst;
                        dr["master_entdate"] = currdate;
                        dr["client_id"] = clientid_mst;
                        dr["client_unit_id"] = unitid_mst;
                    }

                    dataTable.Rows.Add(dr);

                    bool Result = sgen.Update_data(userCode, dataTable, model[0].Col10.Trim(), tmodel.Col8, isedit);
                    if (Result == true)
                    {
                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        //ViewBag.scripCall = "showmsgJS(1, 'Data Saved Successfully', 1);disableForm();";
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";

                        model = new List<Tmodelmain>();
                        tmodel.Col16 = "";
                        tmodel.Col17 = "";
                        tmodel.Col18 = "";
                        tmodel.Col19 = "";
                        tmodel.Col20 = "";
                        tmodel.Col21 = "";
                        tmodel.Col13 = "Save";
                        tmodel.Col9 = "BILL OF ENTRY";
                        model.Add(tmodel);


                    }

                    else
                    {
                        ViewBag.scripCall = "showmsgJS(3, 'Record Not Saved', 1);";
                    }
                END:;

                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1, '" + ex.Message.ToString() + "', 1);"; }

            }



            ModelState.Clear();
            return View(model);

        }
        #endregion

        #region BRC
        public ActionResult brc()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            model = getload(model);
            model[0].Col9 = "BRC ENTRY";
            model[0].Col10 = "enx_tab2"; //TABLE NAME - to be check
            model[0].Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            model[0].Col12 = "BRC"; //TYPE
            model[0].TList1 = mod1;
            model[0].SelectedItems1 = new string[] { "" };
            DataTable dt = sgen.getdata(userCode, "select '' Add_Remove,'1' as  SNo ,'-' as Doc_no,'-' as PartyName,'-' as Country,'-' CINO,'-'  CIDate,'-' as FC_Amt,'0' FC_Rate,'0' LC_Amt,'0' Bank_Name,'0' as Bank_Acc_No," +
                "'-' FC_Rec,'0' LC_Rec,'0' as brc_no,'-' as brc_date from dual");
            model[0].dt1 = dt.Copy();
            sgen.SetSession(MyGuid, "BRC_DT", dt.Copy());
            return View(model);
        }
        public List<Tmodelmain> new_brc(List<Tmodelmain> model)
        {
            model = getnew(model);
            return model;
        }
        [HttpPost]
        public ActionResult brc(List<Tmodelmain> model, string command, string hftable, string hfrow)
        {
            FillMst();
            DataTable dtm = sgen.settable(hftable);
            model[0].dt1 = dtm;
            var tm = model.FirstOrDefault();
            DataTable dt = new DataTable();
            if (command == "New")
            {
                try
                {
                    model = new_brc(model);
                }
                catch (Exception ex) { }
            }

            if (command == "Cancel")
            {
                return CancelFun(model);
            }
            else if (command == "Callback")
            {
                model = StartCallback(model);
            }
            else if (command.Trim() == "Save" || command.Trim() == "Update" || command == "Save & New" || command == "Update & New")
            {
                try
                {
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime(userCode);
                    DataTable dtstr = new DataTable();
                    dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        isedit = true;
                        vch_num = tmodel.Col16.Trim();
                    }
                    else
                    {
                        isedit = false;
                        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where type = '" + model[0].Col12 + "'" + model[0].Col11 + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                    }
                    for (int i = 0; i < model[0].dt1.Rows.Count; i++)
                    {
                        DataRow dr = dtstr.NewRow();

                        dr["vch_num"] = vch_num;
                        dr["vch_date"] = sgen.Make_date_S(model[0].Col17);
                        dr["type"] = model[0].Col12;
                        dr["col4"] = model[0].Col19;//bankid
                        dr["col6"] = model[0].dt1.Rows[i]["CINO"].ToString();   //ci_no
                        dr["col17"] = model[0].dt1.Rows[i]["DOC_NO"].ToString();   //bulk banking docn no
                        dr["col2"] = model[0].dt1.Rows[i]["FC_REC"].ToString();   //fc_rec
                        dr["col3"] = model[0].dt1.Rows[i]["LC_REC"].ToString();   //lc_rec
                        dr["col9"] = model[0].dt1.Rows[i]["BRC_NO"].ToString();    //brc no
                        dr["date3"] = sgen.Make_date_S(model[0].dt1.Rows[i]["BRC_DATE"].ToString());    //brc date
                        dr = getsave(currdate, dr, model, isedit);
                        dtstr.Rows.Add(dr);
                    }
                    bool Result = sgen.Update_data_fast1(userCode, dtstr, model[0].Col10.Trim(), tmodel.Col8, isedit);
                    if (Result == true)
                    {
                       
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col10 = tm.Col10,
                            Col9 = tm.Col9,
                            Col11 = tm.Col11,
                            Col12 = tm.Col12,
                            Col13 = "Save",
                            Col100 = "Save & New",
                            Col121 = "S",
                            Col122 = "<u>S</u>ave",
                            Col123 = "Save & Ne<u>w</u>",
                            Col14 = tm.Col14,
                            Col15 = tm.Col15,
                            dt1 = (DataTable)sgen.GetSession(MyGuid, "BRC_DT")
                        });
                        if (command.Trim().ToUpper().Equals("SAVE") || command.Trim().ToUpper().Equals("UPDATE"))
                        {
                            ViewBag.vnew = "";
                            ViewBag.vedit = "";
                            ViewBag.vsave = "disabled='disabled'";
                            ViewBag.vsavenew = "disabled='disabled'";
                            ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                        }
                        else if (command.Trim().ToUpper().Equals("SAVE & NEW") || command.Trim().ToUpper().Equals("UPDATE & NEW"))
                        {
                            try
                            {
                                model = new_brc(model);
                                ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                            }
                            catch (Exception ex) { }
                        }

                    }

                    else
                    {
                        ViewBag.scripCall = "showmsgJS(3, 'Record Not Saved', 1);";
                    }
                END:;

                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1, '" + ex.Message.ToString() + "', 1);"; }

            }
            else if (command == "-")
            {
                if (model[0].dt1.Rows.Count > 1) model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                else { model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "BRC_DT"); }
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
            }
            ModelState.Clear();
            return View(model);

        }
        #endregion

        #region book_fwd
        //brijesh
        public ActionResult book_fwd()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<SelectListItem> mod2 = new List<SelectListItem>();

            model = getload(model);
            model[0].Col9 = "BOOKING OF FORWARD CONTRACT";
            model[0].Col10 = "vehicle_master"; //TABLE NAME
            model[0].TList1 = mod1;
            model[0].SelectedItems1 = new string[] { "" };
            model[0].TList2 = mod2;
            model[0].SelectedItems2 = new string[] { "" };
            model[0].Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            model[0].Col12 = "BFW"; //Book Forward
            return View(model);
        }
        public List<Tmodelmain> newbook_fwd(List<Tmodelmain> model)
        {
            var tm = model.FirstOrDefault();
            model = getnew(model);
            #region Dropdwon
            List<SelectListItem> mod1 = new List<SelectListItem>();
            TempData[MyGuid + "_TList1"] = model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];

            if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };

            List<SelectListItem> mod2 = new List<SelectListItem>();
            TempData[MyGuid + "_TList2"] = model[0].TList2 = (List<SelectListItem>)TempData[MyGuid + "_TList2"];

            if (tm.SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };

            TempData[MyGuid + "_Tlist1"] = model[0].TList1 = mod1 = cmd_Fun.fwdcurname(userCode, unitid_mst);     //Currency Type 2

            mod2.Add(new SelectListItem { Text = "SALE", Value = "SALE" });    //Nature of Transaction
            mod2.Add(new SelectListItem { Text = "BUY", Value = "BUY" });
            TempData[MyGuid + "_Tlist2"] = model[0].TList2 = mod2;
            #endregion
            return model;
        }
        [HttpPost]
        public ActionResult book_fwd(List<Tmodelmain> model, string command)
        {
            FillMst(model[0].Col15);
            var tm = model.FirstOrDefault();
            #region Dropdwon
            List<SelectListItem> mod1 = new List<SelectListItem>();
            TempData[MyGuid + "_Tlist1"] = model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_Tlist1"];
            if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
            List<SelectListItem> mod2 = new List<SelectListItem>();
            TempData[MyGuid + "_Tlist2"] = model[0].TList2 = (List<SelectListItem>)TempData[MyGuid + "_Tlist2"];
            if (tm.SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
            #endregion

            if (command == "Cancel")
            {
                return CancelFun(model);
            }
            else if (command == "Callback")
            {
                model = StartCallback(model);
            }
            else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
            {
                try
                {
                    string found = "", inactive_date = "";
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime_local(userCode);
                    ent_date = sgen.Savedate(currdate, true);

                    DataTable dtstr = new DataTable();
                    dtstr = sgen.getdata(userCode, "select * from " + model[0].Col10 + " WHERE 1=2");

                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        mq1 = " and vch_num<>'" + tmodel.Col16.Trim() + "'";
                        isedit = true;
                        vch_num = tmodel.Col16.Trim();

                    }
                    else
                    {
                        isedit = false;
                        vch_num = sgen.genNo(userCode, "select max(to_number (vch_num)) as max from " + model[0].Col10 + " where client_id='" + clientid_mst + "'" +
                                                       " and client_unit_id='" + unitid_mst + "' and type='" + model[0].Col12 + "'", 6, "max");

                        string cond = sgen.seekval(userCode, "select col10 from " + model[0].Col10 + " where type='" + model[0].Col12 + "' and client_id='" + clientid_mst + "'  and " +
                                    "col10='" + model[0].Col27 + "' and client_unit_id='" + unitid_mst + "'" + mq1 + "", "col10");
                        if (!cond.Equals("0"))
                        {
                            //Means Already Exits....     
                            //ViewBag.scripCall = "showmsgJS(1,'Forward No Already Exists',2)";
                            ViewBag.vnew = "disabled='disabled'";
                            ViewBag.vedit = "disabled='disabled'";
                            ViewBag.scripCall = "showmsgJS(1,'Forward No Already Exists',2)"; return View(model);
                        }
                    }

                    DataRow dr = dtstr.NewRow();
                    dr["rec_id"] = "0";
                    dr["vch_num"] = vch_num;
                    dr["vch_date"] = currdate;
                    dr["type"] = model[0].Col12;
                    dr["col1"] = model[0].Col49.Trim(); //bank id
                    dr["col2"] = model[0].SelectedItems2.FirstOrDefault();  //Nature of Transaction
                    dr["date2"] = sgen.Savedate(model[0].Col18, false);      //Forward Start Date
                    dr["date3"] = sgen.Savedate(model[0].Col19, false);      //Forward end Date
                    dr["date4"] = sgen.Savedate(model[0].Col21, false);      //Forward Grace End
                    dr["col11"] = model[0].Col22.Trim();    //Forward Amount
                    dr["col9"] = model[0].Col23.Trim();   //Spot Rate
                    dr["col8"] = model[0].Col24.Trim();   //Premium (+) / Discount (-)
                    dr["col14"] = model[0].Col25.Trim(); //Forward Rate
                    dr["col10"] = model[0].Col27.Trim();   //Forward No.
                    dr["date5"] = sgen.Savedate(model[0].Col28, false);      //Forward Date
                    dr["col12"] = model[0].SelectedItems1.FirstOrDefault();      //Forward Date
                    dr["col13"] = model[0].Col30;    //Purpose
                    dr["col41"] = model[0].Col31;    //remark
                    dr = getsave(currdate, dr, model, isedit);
                    dtstr.Rows.Add(dr);
                    bool Result = sgen.Update_data(userCode, dtstr, model[0].Col10.Trim(), tmodel.Col8, isedit);
                    if (Result == true)
                    {
                        if (command.Trim().ToUpper().Equals("SAVE") || command.Trim().ToUpper().Equals("UPDATE"))
                        {
                            ViewBag.vnew = "";
                            ViewBag.vedit = "";
                            ViewBag.vsave = "disabled='disabled'";
                            ViewBag.vsavenew = "disabled='disabled'";
                            ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                        }
                        else if (command.Trim().ToUpper().Equals("SAVE & NEW") || command.Trim().ToUpper().Equals("UPDATE & NEW"))
                        {
                            try
                            {
                                Make_query("book_fwd", "Select Bank", "NEW", "1", "", "", MyGuid);
                                ViewBag.scripCall = "callFoo('Select Bank');";
                                ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                            }
                            catch (Exception ex) { }
                        }
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col16 = tm.Col16,
                            Col17 = "",
                            Col1 = "",
                            Col18 = "",
                            Col9 = tm.Col9,
                            Col10 = tm.Col10,
                            Col11 = tm.Col11,
                            Col12 = tm.Col12,
                            Col14 = tm.Col14,
                            Col15 = tm.Col15,
                            Col13 = "Save",
                            Col100 = "Save & New",
                            Col121 = "S",
                            Col122 = "<u>S</u>ave",
                            Col123 = "Save & Ne<u>w</u>",
                            TList1 = mod1,
                            TList2 = mod2,
                            SelectedItems1 = new string[] { "" },
                            SelectedItems2 = new string[] { "" },
                        });
                    }
                    else
                    {
                        ViewBag.scripCall = "showmsgJS(3, 'Record Not Saved', 1);";

                    }
                    //END:;
                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1, '" + ex.Message.ToString() + "', 1);"; }
            }
            ModelState.Clear();
            return View(model);

        }
        #endregion

        #region use_fwd
        //brijesh
        public ActionResult use_fwd()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "DIRECT USE OF FORWARD";
            model[0].Col10 = "vehicle_master"; //TABLE NAME
            model[0].Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            model[0].Col12 = "UFW"; //Use Forward
            DataTable dt = sgen.getdata(userCode, "select '' Head,'1' SNo ,'-' Forward_no,'-' Forward_date,'-' FWD_Book_Amt ,'-' FWD_Curr,'-' FWD_Book_Date,'-' FWD_Exp_Date,'-' Spot_Rate,'-' Premium,'-' Fwd_Rate,'0' Fwd_used_Amt,'0' Fwd_Bal_Amt,'0' Fwd_To_be_use,'-' Remark from dual");
            model[0].dt1 = dt;
            sgen.SetSession(MyGuid, "BUF_DT", dt);
            return View(model);
        }
        [HttpPost]
        public ActionResult use_fwd(List<Tmodelmain> model, string command, string mid, string hfrow, string hftable)
        {
            FillMst(model[0].Col15);
            DataTable dtm = sgen.settable(hftable);
            model[0].dt1 = dtm;
            var tm = model.FirstOrDefault();

            if (command == "Cancel")
            {
                return CancelFun(model);
            }
            else if (command == "Callback")
            {
                model = StartCallback(model);
            }
            else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
            {
                try
                {
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime_local(userCode);
                    ent_date = sgen.Savedate(currdate, true);

                    DataTable dtstr = new DataTable();
                    dtstr = sgen.getdata(userCode, "select * from " + model[0].Col10 + " WHERE 1=2");

                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        mq1 = " and vch_num<>'" + tmodel.Col16.Trim() + "'";
                        isedit = true;
                        vch_num = tmodel.Col16.Trim();
                    }
                    else
                    {
                        isedit = false;
                        vch_num = sgen.genNo(userCode, "select max(to_number (vch_num)) as max from " + model[0].Col10 + " where client_id='" + clientid_mst + "'" +
                                  " and client_unit_id='" + unitid_mst + "' and type='" + model[0].Col12 + "'", 6, "max");
                    }
                    for (int i = 0; i < model[0].dt1.Rows.Count; i++)
                    {
                        DataRow dr = dtstr.NewRow();
                        dr["rec_id"] = "0";
                        dr["type"] = model[0].Col12;
                        dr["vch_num"] = vch_num.Trim();
                        dr["col1"] = model[0].Col52;      //bank id
                        dr["col4"] = model[0].dt1.Rows[i][2].ToString();   //FWD NO
                        dr["col31"] = model[0].dt1.Rows[i][14].ToString();    //Remark
                        //dr["col10"] = model[0].dt1.Rows[i][11].ToString();   //FWD_USED_AMT
                        //dr["col12"] = model[0].dt1.Rows[i][12].ToString();   //FWD_BAL_AMT
                        dr["col13"] = model[0].dt1.Rows[i][13].ToString();   //FWD_TO_BE_USE
                        dr["date2"] = sgen.Savedate(model[0].Col22, false);    //use date
                        dr = getsave(currdate, dr, model, isedit);
                        dtstr.Rows.Add(dr);
                    }
                    bool Result = sgen.Update_data(userCode, dtstr, model[0].Col10.Trim(), tmodel.Col8, isedit);
                    if (Result == true)
                    {
                        if (command.Trim().ToUpper().Equals("SAVE") || command.Trim().ToUpper().Equals("UPDATE"))
                        {
                            ViewBag.vnew = "";
                            ViewBag.vedit = "";
                            ViewBag.vsave = "disabled='disabled'";
                            ViewBag.vsavenew = "disabled='disabled'";
                            ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                        }
                        else if (command.Trim().ToUpper().Equals("SAVE & NEW") || command.Trim().ToUpper().Equals("UPDATE & NEW"))
                        {
                            try
                            {
                                Make_query("use_fwd", "Select Bank", "NEW", "1", "", "", MyGuid);
                                ViewBag.scripCall = "callFoo('Select Bank');";
                                ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                            }
                            catch (Exception ex) { }
                        }
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col16 = tm.Col16,
                            Col17 = "",
                            Col1 = "",
                            Col18 = "",
                            Col9 = tm.Col9,
                            Col10 = tm.Col10,
                            Col11 = tm.Col11,
                            Col12 = tm.Col12,
                            Col13 = "Save",
                            Col100 = "Save & New",
                            Col121 = "S",
                            Col122 = "<u>S</u>ave",
                            Col123 = "Save & Ne<u>w</u>",
                            Col14 = tm.Col14,
                            Col15 = tm.Col15,
                            dt1 = (DataTable)sgen.GetSession(MyGuid, "BUF_DT")

                        });
                    }
                    else
                    {
                        ViewBag.scripCall = "showmsgJS(3, 'Record Not Saved', 1);";
                    }
                END:;
                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1, '" + ex.Message.ToString() + "', 1);"; }
            }

            else if (command == "-")
            {
                if (model[0].dt1.Rows.Count > 1) model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                else { model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "BUF_DT"); }
            }

            ModelState.Clear();
            return View(model);
        }
        #endregion

        #region bulk_pi
        public ActionResult bulk_pi()
        {
            FillMst();

            List<Tmodelmain> model = new List<Tmodelmain>();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            model = getload(model);
            model[0].Col9 = "PERFORMA INVOICE ENTRY";
            model[0].Col10 = "enx_tab2"; //TABLE NAME
            model[0].Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            model[0].Col12 = "EPI"; //TYPE
            model[0].TList1 = mod1;
            model[0].SelectedItems1 = new string[] { "" };
            return View(model);
        }
        public List<Tmodelmain> newbulk_pi(List<Tmodelmain> model)
        {
            var tm = model.FirstOrDefault();
            model = getnew(model);
            List<SelectListItem> mod1 = new List<SelectListItem>();
            #region Currency
            TempData[MyGuid + "_Tlist1"] = model[0].TList1 = mod1 = cmd_Fun.curname(userCode, unitid_mst);

            #endregion
            return model;
        }

        [HttpPost]
        public ActionResult bulk_pi(List<Tmodelmain> model, string command, HttpPostedFileBase upd1)
        {
            FillMst();
            var tm = model.FirstOrDefault();
            DataTable dt = new DataTable();
            type = model[0].Col12;
            tab_name = model[0].Col10;
            where = model[0].Col11;
            #region Dropdwon
            List<SelectListItem> mod1 = new List<SelectListItem>();
            TempData[MyGuid + "_TList1"] = model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];

            if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
            #endregion
            if (command == "New")
            {
                model = newbulk_pi(model);
            }
            else if (command == "Cancel")
            {
                return CancelFun(model);
            }
            else if (command == "Callback")
            {
                model = StartCallback(model);
            }
            else if (command.Trim() == "Save" || command.Trim() == "Update" || command == "Save & New" || command == "Update & New")
            {
                try
                {
                    string found = "";
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime(userCode);
                    ent_date = currdate;

                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        mq1 = " and vch_num<>'" + tmodel.Col16.Trim() + "'";
                        isedit = true;
                        vch_num = tmodel.Col16.Trim();

                    }
                    else
                    {
                        isedit = false;
                        vch_num = sgen.genNo(userCode, "select max(to_number (vch_num)) as max from " + model[0].Col10 + " where client_id='" + clientid_mst + "'" +
                                                       " and client_unit_id='" + unitid_mst + "' and type='" + model[0].Col12 + "'", 6, "max");

                        string cond = sgen.seekval(userCode, "select col7 from " + model[0].Col10 + " where type='" + model[0].Col12 + "' and client_id='" + clientid_mst + "'  and " +
                                    "col7='" + model[0].Col21 + "' and client_unit_id='" + unitid_mst + "'" + mq1 + "", "col7");
                        if (!cond.Equals("0"))
                        {
                            //Means Already Exits.... 
                            ViewBag.vnew = "disabled='disabled'";
                            ViewBag.vedit = "disabled='disabled'";
                            ViewBag.scripCall = "showmsgJS(1,'PI No Already Exists',2)";
                            goto END;
                        }
                    }
                    DataTable dtstr = sgen.getdata(userCode, "select  * from " + model[0].Col10 + " where 1=2");
                    DataRow dr = dtstr.NewRow();
                    dr["type"] = model[0].Col12;
                    dr["col5"] = model[0].Col17;
                    dr["vch_num"] = model[0].Col16;
                    dr["vch_date"] = sgen.Savedate(currdate, false);
                    dr["col6"] = model[0].SelectedItems1.FirstOrDefault();
                    dr["col9"] = model[0].Col23; //PI Amt in FC
                    dr["col7"] = model[0].Col21; //PI No. 
                    dr["date1"] = sgen.Savedate(model[0].Col22, false); //PI Date 
                    dr["col10"] = model[0].Col24; //Exchange Rate
                    dr = getsave(currdate, dr, model, isedit);
                    dtstr.Rows.Add(dr);
                    bool Result = sgen.Update_data(userCode, dtstr, model[0].Col10.Trim(), tmodel.Col8, isedit);
                    if (Result == true)
                    {
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col9 = tm.Col9,
                            Col10 = tm.Col10,
                            Col11 = tm.Col11,
                            Col12 = tm.Col12,
                            Col13 = "Save",
                            Col100 = "Save & New",
                            Col121 = "S",
                            Col122 = "<u>S</u>ave",
                            Col123 = "Save & Ne<u>w</u>",
                            Col14 = tm.Col14,
                            Col15 = tm.Col15,
                            TList1 = mod1,
                            SelectedItems1 = new string[] { "" },
                        });
                        if (command.Trim().ToUpper().Equals("SAVE") || command.Trim().ToUpper().Equals("UPDATE"))
                        {
                            ViewBag.vnew = "";
                            ViewBag.vedit = "";
                            ViewBag.vsave = "disabled='disabled'";
                            ViewBag.vsavenew = "disabled='disabled'";
                            ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                        }
                        else if (command.Trim().ToUpper().Equals("SAVE & NEW") || command.Trim().ToUpper().Equals("UPDATE & NEW"))
                        {
                            try
                            {
                                model = newbulk_pi(model);
                                ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                            }
                            catch (Exception ex) { }
                        }
                    }
                    else
                    {
                        ViewBag.scripCall = "showmsgJS(1, 'Record Not Saved', 2);";
                    }
                END:;
                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1, '" + ex.Message.ToString() + "', 2);"; }
            }
            else if (command == "Import")
            {
                HttpPostedFileBase file1 = upd1;
                dt = new DataTable();
                if (file1.ContentLength > 1)
                {
                    string ext = Path.GetExtension(file1.FileName).ToLower();
                    if (ext == ".csv")
                    {
                        string filesavepath = AppDomain.CurrentDomain.BaseDirectory + "\\Uploads\\" + cg_com_name.Replace(" ", "")
                            + "\\file" + DateTime.Now.ToString("ddMMyyyyhhmmfff") + ext;
                        file1.SaveAs(filesavepath);
                        // Read sample data from CSV file
                        int i = 0;
                        using (CsvFileReader reader = new CsvFileReader(filesavepath))
                        {
                            CsvRow row = new CsvRow();
                            while (reader.ReadRow(row))
                            {
                                DataRow dr = dt.NewRow();
                                for (int c = 0; c < row.Count; c++)
                                {
                                    if (i == 0) { dt.Columns.Add(row[c].ToString()); }
                                    else { dr[c] = row[c].ToString(); }
                                }
                                i++;
                                dt.Rows.Add(dr);
                            }
                        }
                    }
                    try
                    {
                        dt.Rows[0].Delete();
                        string currdate = sgen.server_datetime(userCode);
                        ent_date = currdate;
                        for (int k = 0; k < dt.Rows.Count; k++)
                        {
                            mq = sgen.seekval(userCode, "Select col7 from " + tab_name + " where type = '" + type + "' " + where + " " +
                                "and upper(col7)='" + dt.Rows[k]["Pi_no"].ToString() + "'", "col7");
                            if (!mq.Trim().Equals("0"))
                            {
                                ViewBag.scripCall += "showmsgJS(3, 'You already have " + dt.Rows[k][0].ToString() + " PI No saved', 2);";
                                ViewBag.vnew = "disabled='disabled'";
                                ViewBag.vedit = "disabled='disabled'";
                                ViewBag.vsave = "";
                                ViewBag.vsavenew = "";
                                return View(model);
                            }
                        }
                        DataTable dataTable = cmd_Fun.GetStructure(userCode, tab_name);
                        int inc = 0, inc1 = 0;
                        mq = "select max(to_number(vch_num)) as auto_genid from " + tab_name + " where type = '" + type + "'" + where + "";
                        inc = sgen.Make_int(sgen.genNo(userCode, mq, 6, "auto_genid"));
                        DataTable dtcurr = sgen.getdata(userCode, "SELECT master_id,master_name FROM master_setting where type = 'CTM' and " +
        "find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                        DataTable dtcust = sgen.getdata(userCode, "SELECT vch_num,c_name FROM clients_mst where type = 'BCD' and substr(vch_num,0,3)='303' and " +
        "find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow dr = dataTable.NewRow();
                            if (i == 0)
                            {
                                inc = inc + i;
                            }
                            else
                            {
                                inc = inc + 1;
                            }
                            vch_num = sgen.padlc(inc, 6);
                            dr["vch_num"] = vch_num.Trim();
                            dr["vch_date"] = ent_date;
                            dr["type"] = type.Trim();
                            if (dt.Rows[i]["Customer_code"].ToString() != "")
                            {
                                if (dtcust.Rows.Count > 0)
                                {
                                    try
                                    {
                                        DataTable dtf3 = sgen.seekval_dt(dtcust, "vch_num='" + dt.Rows[i]["Customer_code"].ToString() + "'");
                                        dr["col5"] = dtf3.Rows[0]["vch_num"].ToString();
                                    }
                                    catch (Exception)
                                    {
                                        ViewBag.vnew = "disabled='disabled'";
                                        ViewBag.vedit = "disabled='disabled'";
                                        ViewBag.vsave = "";
                                        ViewBag.vsavenew = "";
                                        ViewBag.scripCall += "showmsgJS(3, 'Customer_code :  " + dt.Rows[i]["Customer_code"].ToString() + " Is Not Valid, Please Check !', 2);";
                                        return View(model);
                                    }
                                }
                            }
                            if (dt.Rows[i]["Currency"].ToString() != "")
                            {
                                if (dtcurr.Rows.Count > 0)
                                {
                                    try
                                    {
                                        DataTable dtf2 = sgen.seekval_dt(dtcurr, "master_id='" + dt.Rows[i]["Currency"].ToString() + "'");
                                        dr["col6"] = dtf2.Rows[0]["master_id"].ToString();
                                    }
                                    catch (Exception)
                                    {
                                        ViewBag.vnew = "disabled='disabled'";
                                        ViewBag.vedit = "disabled='disabled'";
                                        ViewBag.vsave = "";
                                        ViewBag.vsavenew = "";
                                        ViewBag.scripCall = "showmsgJS(1, 'Currency : " + dt.Rows[i]["Currency"].ToString() + " does not belongs to Masters, Please Check ! ', 2);";
                                        return View(model);
                                    }
                                }
                            }
                            dr["col9"] = dt.Rows[i]["Pi_amt_in_fc"].ToString();
                            dr["col7"] = dt.Rows[i]["Pi_no"].ToString();
                            dr["date1"] = sgen.Make_date_S(dt.Rows[i]["Pi_Date"].ToString());
                            dr["col10"] = dt.Rows[i]["Exchange_rate"].ToString();
                            dr["rec_id"] = "0";
                            dr["ent_by"] = userid_mst;
                            dr["ent_date"] = ent_date;
                            dr["edit_by"] = "-";
                            dr["edit_date"] = ent_date;
                            dr["client_id"] = clientid_mst;
                            dr["client_unit_id"] = unitid_mst;
                            dataTable.Rows.Add(dr);
                        }
                        res = sgen.Update_data_fast1(userCode, dataTable, tab_name, model[0].Col8, false);
                        if (res)
                        {
                            sgen.SetSession(MyGuid, "PI", null);
                            model = new List<Tmodelmain>();
                            model.Add(new Tmodelmain
                            {
                                Col3 = tm.Col3,
                                Col8 = tm.Col8,
                                Col9 = tm.Col9,
                                Col10 = tm.Col10,
                                Col11 = tm.Col11,
                                Col12 = tm.Col12,
                                Col13 = "Save",
                                Col100 = "Save & New",
                                Col121 = "S",
                                Col122 = "<u>S</u>ave",
                                Col123 = "Save & Ne<u>w</u>",
                                Col14 = tm.Col14,
                                Col15 = tm.Col15,
                            });
                            ViewBag.vnew = "";
                            ViewBag.vedit = "";
                            ViewBag.vsave = "disabled='disabled'";
                            ViewBag.vsavenew = "disabled='disabled'";
                            ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Imported Successfully');disableForm(); ";
                        }
                        else { ViewBag.scripCall = "showmsgJS(3, 'Record Not Saved', 1);"; }
                    }
                    catch (Exception err) { sgen.showmsg(1, err.Message.ToString(), 0); }
                }
            }
            ModelState.Clear();
            return View(model);
        }
        [HttpGet]
        public FileResult pitemp(List<Tmodelmain> model, string Myguid = "")
        {
            FillMst(Myguid);
            model = (List<Tmodelmain>)sgen.GetSession(Myguid, "model");
            DataTable dtl = new DataTable();
            mq = "SELECT '' Customer_code,'' Pi_no,'' Pi_Date ,'' Currency,'' Pi_amt_in_fc,'' Exchange_rate from Dual";
            dtl = sgen.getdata(userCode, mq);
            string cg_com_name = sgen.GetCookie(MyGuid, "cg_com_name");
            Byte[] fileBytes = sgen.Exp_to_csv_new(dtl, "pitemp", cg_com_name);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "pitemp");
        }
        #endregion

        #region Advance Against PI - PAYMENT RECEIVED
        public ActionResult adv_pi()
        {

            FillMst();
            //DataTable dt = new DataTable();
            chkRef();

            ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.vsave = "disabled='disabled'";
            ViewBag.scripCall = "disableForm();";
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);

            sgen.SetSession(MyGuid, "TR_MID", mid_mst.Trim());
            Tmodelmain tm1 = new Tmodelmain();
            List<Tmodelmain> model = new List<Tmodelmain>();


            tm1.Col9 = "ADVANCE AGAINST PI";
            tm1.Col10 = "sales"; //TABLE NAME
            tm1.Col13 = "Save";

            tm1.Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            tm1.Col12 = "ECI"; //TYPE
            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = MyGuid.Trim();

            List<SelectListItem> mod1 = new List<SelectListItem>();
            tm1.TList1 = mod1;
            tm1.SelectedItems1 = new string[] { "" };

            DataTable dt = sgen.getdata(userCode, "select '' Item,'1' as  SNo ,'-' as Icode,'-' as IName,'-' PARTNO,'-'  HSN,'-' as UOM,'0' TaxRate,'0' Qty_Chl,'0' Qty_In,'0' as IRate," +
                "'-' disc_type,'0' as disc_rate,'0' IAmount,'0' TaxAmount,'0' discAmount,'0' LineAmount,'-' as Remark,'0' PO_Number,'-' PO_Date from dual");
            tm1.dt1 = dt;
            sgen.SetSession(MyGuid, "KMRN_DT", dt);
            sgen.SetSession(MyGuid, "KMRNTYPE", null);

            //DataTable dt = sgen.getdata(userCode, mq);
            model.Add(tm1);

            return View(model);
        }
        [HttpPost]
        public ActionResult adv_pi(List<Tmodelmain> model, string command, string hftable, string hfrow)
        {
            FillMst();
            DataTable dtm = sgen.settable(hftable);
            model[0].dt1 = dtm;

            var tm = model.FirstOrDefault();
            DataTable dt = new DataTable();

            #region ddl
            List<SelectListItem> mod1 = new List<SelectListItem>();

            TempData[MyGuid + "_TList1"] = model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];



            if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };

            #endregion
            if (command == "New")
            {
                try
                {
                    sgen.SetSession(MyGuid, "EDMODE", "N");
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.scripCall = "enableForm();";

                    #region Currency

                    mq1 = "select master_id, master_name from master_setting where  type='CTM' " + model[0].Col11 + "";

                    dt = sgen.getdata(userCode, mq1);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            mod1.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                        }
                    }
                    TempData[MyGuid + "_Tlist1"] = model[0].TList1 = mod1;
                    #endregion


                    //mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10 + " where type in  ('DDECI' ,'ECI')" + model[0].Col11 + "";
                    //master_id = sgen.genNo(userCode, mq, 3, "auto_genid");


                    //model[0].Col16 = master_id;

                    //model[0].Col22 = "Y";


                }
                catch (Exception ex) { }
            }



            else if (command == "Cancel")
            {
                return CancelFun(model);
            }
            else if (command == "Callback")
            {
                if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
                model = CallbackFun(btnval, "N", "adv_pi", "Exim", model);
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
            }
            else if (command.Trim() == "Save" || command.Trim() == "Update")
            {
                try
                {
                    string found = "", inactive_date = "";
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime_local(userCode);
                    ent_date = sgen.Savedate(currdate, true);

                    DataTable dtstr = new DataTable();
                    dtstr = sgen.getdata(userCode, "select * from " + model[0].Col10 + " WHERE 1=2");

                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        mq1 = " and vch_num<>'" + tmodel.Col23.Trim() + "'";
                        isedit = true;
                        vch_num = tmodel.Col23.Trim();
                        master_id = tmodel.Col16.Trim();

                    }
                    else
                    {
                        isedit = false;



                        mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10 + " where type in  ('DDECI','ECI')" + model[0].Col11 + "";
                        master_id = sgen.genNo(userCode, mq, 3, "auto_genid");
                        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where type in   ('DDECI','ECI')" + model[0].Col11 + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");

                    }

                    found = sgen.getstring(userCode, "select master_name from " + model[0].Col10 + " a where upper(master_name)= upper('" + model[0].Col18 + "')" +
                        " and type in  ('DDECI','ECI') " + model[0].Col11 + " " + mq1 + "");
                    if (found != "")
                    {

                        ViewBag.scripCall = "showmsgJS(1, 'Data Already Exists', 2);";
                        goto END;
                    }

                    if (model[0].Col22 == "Y") { status = "Y"; inactive_date = currdate; model[0].Col12 = "ECI"; }
                    else { status = "N"; inactive_date = currdate; model[0].Col12 = "DDECI"; }
                    DataTable dataTable = sgen.getdata(userCode, "select  * from " + model[0].Col10 + " where 1=2");

                    for (int i = 0; i < model[0].dt1.Rows.Count; i++)
                    {
                        DataRow dr = dataTable.NewRow();

                        dr["vch_num"] = vch_num;
                        dr["vch_date"] = sgen.Savedate(model[0].Col17, false);
                        dr["type"] = model[0].Col12;
                        dr["Master_type"] = Master_Type;
                        dr["Master_Name"] = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(model[0].Col18.ToLower());
                        dr["master_id"] = master_id;
                        dr["Section_Strength"] = model[0].Col20.Trim();
                        dr["classid"] = model[0].Col19.Trim();
                        dr["sectionid"] = model[0].Col21.Trim();
                        dr["Status"] = status;
                        dr["Inactive_date"] = sgen.Savedate(inactive_date, false);


                        dr["client_id"] = clientid_mst;
                        dr["client_unit_id"] = unitid_mst;
                        if (isedit)
                        {
                            dr["master_entby"] = model[0].Col3.Trim();
                            dr["master_entdate"] = model[0].Col4.Trim();
                            dr["master_editby"] = userid_mst;
                            dr["master_editdate"] = currdate;
                            dr["client_id"] = model[0].Col1;
                            dr["client_unit_id"] = model[0].Col2;
                            dr["rec_id"] = model[0].Col7;

                        }
                        else
                        {
                            dr["rec_id"] = "0";
                            dr["master_editby"] = "-";
                            dr["master_editdate"] = currdate;
                            dr["master_entby"] = userid_mst;
                            dr["master_entdate"] = currdate;
                            dr["client_id"] = clientid_mst;
                            dr["client_unit_id"] = unitid_mst;
                        }

                        dataTable.Rows.Add(dr);
                    }
                    bool Result = sgen.Update_data(userCode, dataTable, model[0].Col10.Trim(), tmodel.Col8, isedit);
                    if (Result == true)
                    {
                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        //ViewBag.scripCall = "showmsgJS(1, 'Data Saved Successfully', 1);disableForm();";
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                        model = new List<Tmodelmain>();
                        tmodel.Col16 = "";
                        tmodel.Col17 = "";
                        tmodel.Col18 = "";
                        tmodel.Col19 = "";
                        tmodel.Col20 = "";
                        tmodel.Col21 = "";
                        tmodel.Col13 = "Save";
                        tmodel.Col9 = "ADVANCE AGAINST PI";
                        model.Add(tmodel);


                    }

                    else
                    {
                        ViewBag.scripCall = "showmsgJS(3, 'Record Not Saved', 1);";
                    }
                END:;

                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1, '" + ex.Message.ToString() + "', 1);"; }

            }
            else if (command == "-")
            {
                if (model[0].dt1.Rows.Count > 1) model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                else { model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "KMRN_DT"); }
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
            }


            ModelState.Clear();
            return View(model);

        }
        #endregion

        #region Finance Against CI
        public ActionResult dis_ci()
        {

            FillMst();
            //DataTable dt = new DataTable();
            chkRef();

            ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.vsave = "disabled='disabled'";
            ViewBag.scripCall = "disableForm();";
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);

            sgen.SetSession(MyGuid, "TR_MID", mid_mst.Trim());
            Tmodelmain tm1 = new Tmodelmain();
            List<Tmodelmain> model = new List<Tmodelmain>();


            tm1.Col9 = "FINANCE AGAINST CI";
            tm1.Col10 = "sales"; //TABLE NAME
            tm1.Col13 = "Save";

            tm1.Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            tm1.Col12 = "ECI"; //TYPE
            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = MyGuid.Trim();

            List<SelectListItem> mod1 = new List<SelectListItem>();
            tm1.TList1 = mod1;
            tm1.SelectedItems1 = new string[] { "" };

            DataTable dt = sgen.getdata(userCode, "select '' Item,'1' as  SNo ,'-' as Icode,'-' as IName,'-' PARTNO,'-'  HSN,'-' as UOM,'0' TaxRate,'0' Qty_Chl,'0' Qty_In,'0' as IRate," +
                "'-' disc_type,'0' as disc_rate,'0' IAmount,'0' TaxAmount,'0' discAmount,'0' LineAmount,'-' as Remark,'0' PO_Number,'-' PO_Date from dual");
            tm1.dt1 = dt;
            sgen.SetSession(MyGuid, "KMRN_DT", dt);
            sgen.SetSession(MyGuid, "KMRNTYPE", null);

            //DataTable dt = sgen.getdata(userCode, mq);
            model.Add(tm1);

            return View(model);
        }
        [HttpPost]
        public ActionResult dis_ci(List<Tmodelmain> model, string command, string hftable, string hfrow)
        {
            FillMst();
            DataTable dtm = sgen.settable(hftable);
            model[0].dt1 = dtm;

            var tm = model.FirstOrDefault();
            DataTable dt = new DataTable();

            #region ddl
            List<SelectListItem> mod1 = new List<SelectListItem>();

            TempData[MyGuid + "_TList1"] = model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];

            if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };

            #endregion
            if (command == "New")
            {
                try
                {
                    sgen.SetSession(MyGuid, "EDMODE", "N");
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.scripCall = "enableForm();";

                    #region Currency

                    mq1 = "select master_id, master_name from master_setting where  type='CTM' " + model[0].Col11 + "";

                    dt = sgen.getdata(userCode, mq1);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            mod1.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                        }
                    }
                    TempData[MyGuid + "_Tlist1"] = model[0].TList1 = mod1;
                    #endregion

                    //model[0].TList1 = mod1;
                    //mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10 + " where type in  ('DDECI' ,'ECI')" + model[0].Col11 + "";
                    //master_id = sgen.genNo(userCode, mq, 3, "auto_genid");


                    //model[0].Col16 = master_id;

                    //model[0].Col22 = "Y";


                }
                catch (Exception ex) { }
            }



            else if (command == "Cancel")
            {
                return CancelFun(model);
            }
            else if (command == "Callback")
            {
                if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
                model = CallbackFun(btnval, "N", "dis_ci", "Exim", model);
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
            }
            else if (command.Trim() == "Save" || command.Trim() == "Update")
            {
                try
                {
                    string found = "", inactive_date = "";
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime_local(userCode);
                    ent_date = sgen.Savedate(currdate, true);

                    DataTable dtstr = new DataTable();
                    dtstr = sgen.getdata(userCode, "select * from " + model[0].Col10 + " WHERE 1=2");

                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        mq1 = " and vch_num<>'" + tmodel.Col23.Trim() + "'";
                        isedit = true;
                        vch_num = tmodel.Col23.Trim();
                        master_id = tmodel.Col16.Trim();

                    }
                    else
                    {
                        isedit = false;



                        mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10 + " where type in  ('DDECI','ECI')" + model[0].Col11 + "";
                        master_id = sgen.genNo(userCode, mq, 3, "auto_genid");
                        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where type in   ('DDECI','ECI')" + model[0].Col11 + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");

                    }

                    found = sgen.getstring(userCode, "select master_name from " + model[0].Col10 + " a where upper(master_name)= upper('" + model[0].Col18 + "')" +
                        " and type in  ('DDECI','ECI') " + model[0].Col11 + " " + mq1 + "");
                    if (found != "")
                    {

                        ViewBag.scripCall = "showmsgJS(1, 'Data Already Exists', 2);";
                        goto END;
                    }

                    if (model[0].Col22 == "Y") { status = "Y"; inactive_date = currdate; model[0].Col12 = "ECI"; }
                    else { status = "N"; inactive_date = currdate; model[0].Col12 = "DDECI"; }
                    DataTable dataTable = sgen.getdata(userCode, "select  * from " + model[0].Col10 + " where 1=2");

                    for (int i = 0; i < model[0].dt1.Rows.Count; i++)
                    {
                        DataRow dr = dataTable.NewRow();

                        dr["vch_num"] = vch_num;
                        dr["vch_date"] = sgen.Savedate(model[0].Col17, false);
                        dr["type"] = model[0].Col12;
                        dr["Master_type"] = Master_Type;
                        dr["Master_Name"] = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(model[0].Col18.ToLower());
                        dr["master_id"] = master_id;
                        dr["Section_Strength"] = model[0].Col20.Trim();
                        dr["classid"] = model[0].Col19.Trim();
                        dr["sectionid"] = model[0].Col21.Trim();
                        dr["Status"] = status;
                        dr["Inactive_date"] = sgen.Savedate(inactive_date, false);


                        dr["client_id"] = clientid_mst;
                        dr["client_unit_id"] = unitid_mst;
                        if (isedit)
                        {
                            dr["master_entby"] = model[0].Col3.Trim();
                            dr["master_entdate"] = model[0].Col4.Trim();
                            dr["master_editby"] = userid_mst;
                            dr["master_editdate"] = currdate;
                            dr["client_id"] = model[0].Col1;
                            dr["client_unit_id"] = model[0].Col2;
                            dr["rec_id"] = model[0].Col7;

                        }
                        else
                        {
                            dr["rec_id"] = "0";
                            dr["master_editby"] = "-";
                            dr["master_editdate"] = currdate;
                            dr["master_entby"] = userid_mst;
                            dr["master_entdate"] = currdate;
                            dr["client_id"] = clientid_mst;
                            dr["client_unit_id"] = unitid_mst;
                        }

                        dataTable.Rows.Add(dr);
                    }
                    bool Result = sgen.Update_data(userCode, dataTable, model[0].Col10.Trim(), tmodel.Col8, isedit);
                    if (Result == true)
                    {
                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        //ViewBag.scripCall = "showmsgJS(1, 'Data Saved Successfully', 1);disableForm();";
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col10 = tm.Col10,
                            Col11 = tm.Col11,
                            Col13 = "Save",
                            Col14 = tm.Col14,
                            Col15 = tm.Col15,

                        });


                    }

                    else
                    {
                        ViewBag.scripCall = "showmsgJS(3, 'Record Not Saved', 1);";
                    }
                END:;

                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1, '" + ex.Message.ToString() + "', 1);"; }

            }
            else if (command == "-")
            {
                if (model[0].dt1.Rows.Count > 1) model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                else { model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "KMRN_DT"); }
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
            }


            ModelState.Clear();
            return View(model);

        }
        #endregion

        #region SHIPPING BILL FILE WITH BANK
        public ActionResult sb_bank()
        {

            FillMst();
            //DataTable dt = new DataTable();
            chkRef();

            ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.vsave = "disabled='disabled'";
            ViewBag.scripCall = "disableForm();";
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);

            sgen.SetSession(MyGuid, "TR_MID", mid_mst.Trim());
            Tmodelmain tm1 = new Tmodelmain();
            List<Tmodelmain> model = new List<Tmodelmain>();


            tm1.Col9 = "SB SUBMITTED WITH BANK";
            tm1.Col10 = "sales"; //TABLE NAME
            tm1.Col13 = "Save";

            tm1.Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            tm1.Col12 = "ECI"; //TYPE
            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = MyGuid.Trim();

            List<SelectListItem> mod1 = new List<SelectListItem>();
            tm1.TList1 = mod1;
            tm1.SelectedItems1 = new string[] { "" };

            DataTable dt = sgen.getdata(userCode, "select '' Item,'1' as  SNo ,'-' as Icode,'-' as IName,'-' PARTNO,'-'  HSN,'-' as UOM,'0' TaxRate,'0' Qty_Chl,'0' Qty_In,'0' as IRate," +
                "'-' disc_type,'0' as disc_rate,'0' IAmount,'0' TaxAmount,'0' discAmount,'0' LineAmount,'-' as Remark,'0' PO_Number,'-' PO_Date from dual");
            tm1.dt1 = dt;
            sgen.SetSession(MyGuid, "KMRN_DT", dt);
            sgen.SetSession(MyGuid, "KMRNTYPE", null);

            //DataTable dt = sgen.getdata(userCode, mq);
            model.Add(tm1);

            return View(model);
        }
        [HttpPost]
        public ActionResult sb_bank(List<Tmodelmain> model, string command, string hftable, string hfrow)
        {
            FillMst();
            DataTable dtm = sgen.settable(hftable);
            model[0].dt1 = dtm;

            var tm = model.FirstOrDefault();
            DataTable dt = new DataTable();

            #region ddl
            List<SelectListItem> mod1 = new List<SelectListItem>();

            TempData[MyGuid + "_TList1"] = model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];



            if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };

            #endregion
            if (command == "New")
            {
                try
                {
                    sgen.SetSession(MyGuid, "EDMODE", "N");
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.scripCall = "enableForm();";

                    #region Currency

                    mq1 = "select master_id, master_name from master_setting where  type='CTM' " + model[0].Col11 + "";

                    dt = sgen.getdata(userCode, mq1);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            mod1.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                        }
                    }
                    TempData[MyGuid + "_Tlist1"] = model[0].TList1 = mod1;
                    #endregion


                    //mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10 + " where type in  ('DDECI' ,'ECI')" + model[0].Col11 + "";
                    //master_id = sgen.genNo(userCode, mq, 3, "auto_genid");


                    //model[0].Col16 = master_id;

                    //model[0].Col22 = "Y";


                }
                catch (Exception ex) { }
            }



            else if (command == "Cancel")
            {
                return CancelFun(model);
            }
            else if (command == "Callback")
            {
                if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
                model = CallbackFun(btnval, "N", "sb_bank", "Exim", model);
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
            }
            else if (command.Trim() == "Save" || command.Trim() == "Update")
            {
                try
                {
                    string found = "", inactive_date = "";
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime_local(userCode);
                    ent_date = sgen.Savedate(currdate, true);

                    DataTable dtstr = new DataTable();
                    dtstr = sgen.getdata(userCode, "select * from " + model[0].Col10 + " WHERE 1=2");

                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        mq1 = " and vch_num<>'" + tmodel.Col23.Trim() + "'";
                        isedit = true;
                        vch_num = tmodel.Col23.Trim();
                        master_id = tmodel.Col16.Trim();

                    }
                    else
                    {
                        isedit = false;



                        mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10 + " where type in  ('DDECI','ECI')" + model[0].Col11 + "";
                        master_id = sgen.genNo(userCode, mq, 3, "auto_genid");
                        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where type in   ('DDECI','ECI')" + model[0].Col11 + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");

                    }

                    found = sgen.getstring(userCode, "select master_name from " + model[0].Col10 + " a where upper(master_name)= upper('" + model[0].Col18 + "')" +
                        " and type in  ('DDECI','ECI') " + model[0].Col11 + " " + mq1 + "");
                    if (found != "")
                    {

                        ViewBag.scripCall = "showmsgJS(1, 'Data Already Exists', 2);";
                        goto END;
                    }

                    if (model[0].Col22 == "Y") { status = "Y"; inactive_date = currdate; model[0].Col12 = "ECI"; }
                    else { status = "N"; inactive_date = currdate; model[0].Col12 = "DDECI"; }
                    DataTable dataTable = sgen.getdata(userCode, "select  * from " + model[0].Col10 + " where 1=2");

                    for (int i = 0; i < model[0].dt1.Rows.Count; i++)
                    {
                        DataRow dr = dataTable.NewRow();

                        dr["vch_num"] = vch_num;
                        dr["vch_date"] = sgen.Savedate(model[0].Col17, false);
                        dr["type"] = model[0].Col12;
                        dr["Master_type"] = Master_Type;
                        dr["Master_Name"] = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(model[0].Col18.ToLower());
                        dr["master_id"] = master_id;
                        dr["Section_Strength"] = model[0].Col20.Trim();
                        dr["classid"] = model[0].Col19.Trim();
                        dr["sectionid"] = model[0].Col21.Trim();
                        dr["Status"] = status;
                        dr["Inactive_date"] = sgen.Savedate(inactive_date, false);


                        dr["client_id"] = clientid_mst;
                        dr["client_unit_id"] = unitid_mst;
                        if (isedit)
                        {
                            dr["master_entby"] = model[0].Col3.Trim();
                            dr["master_entdate"] = model[0].Col4.Trim();
                            dr["master_editby"] = userid_mst;
                            dr["master_editdate"] = currdate;
                            dr["client_id"] = model[0].Col1;
                            dr["client_unit_id"] = model[0].Col2;
                            dr["rec_id"] = model[0].Col7;

                        }
                        else
                        {
                            dr["rec_id"] = "0";
                            dr["master_editby"] = "-";
                            dr["master_editdate"] = currdate;
                            dr["master_entby"] = userid_mst;
                            dr["master_entdate"] = currdate;
                            dr["client_id"] = clientid_mst;
                            dr["client_unit_id"] = unitid_mst;
                        }

                        dataTable.Rows.Add(dr);
                    }
                    bool Result = sgen.Update_data(userCode, dataTable, model[0].Col10.Trim(), tmodel.Col8, isedit);
                    if (Result == true)
                    {
                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        //ViewBag.scripCall = "showmsgJS(1, 'Data Saved Successfully', 1);disableForm();";
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col10 = tm.Col10,
                            Col11 = tm.Col11,
                            Col13 = "Save",
                            Col14 = tm.Col14,
                            Col15 = tm.Col15,

                        });

                    }

                    else
                    {
                        ViewBag.scripCall = "showmsgJS(3, 'Record Not Saved', 1);";
                    }
                END:;

                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1, '" + ex.Message.ToString() + "', 1);"; }

            }
            else if (command == "-")
            {
                if (model[0].dt1.Rows.Count > 1) model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                else { model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "KMRN_DT"); }
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
            }


            ModelState.Clear();
            return View(model);

        }
        #endregion

        #region DIRECT COLLECTION FROM CUSTOMER - PAYMENT RECEIVED
        public ActionResult dir_coll()
        {

            FillMst();
            //DataTable dt = new DataTable();
            chkRef();

            ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.vsave = "disabled='disabled'";
            ViewBag.scripCall = "disableForm();";
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);

            sgen.SetSession(MyGuid, "TR_MID", mid_mst.Trim());
            Tmodelmain tm1 = new Tmodelmain();
            List<Tmodelmain> model = new List<Tmodelmain>();


            tm1.Col9 = "DIRECT COLLECTION";
            tm1.Col10 = "sales"; //TABLE NAME
            tm1.Col13 = "Save";

            tm1.Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            tm1.Col12 = "ECI"; //TYPE
            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = MyGuid.Trim();

            List<SelectListItem> mod1 = new List<SelectListItem>();
            tm1.TList1 = mod1;
            tm1.SelectedItems1 = new string[] { "" };

            DataTable dt = sgen.getdata(userCode, "select '' Item,'1' as  SNo ,'-' as Icode,'-' as IName,'-' PARTNO,'-'  HSN,'-' as UOM,'0' TaxRate,'0' Qty_Chl,'0' Qty_In,'0' as IRate," +
                "'-' disc_type,'0' as disc_rate,'0' IAmount,'0' TaxAmount,'0' discAmount,'0' LineAmount,'-' as Remark,'0' PO_Number,'-' PO_Date from dual");
            tm1.dt1 = dt;
            sgen.SetSession(MyGuid, "KMRN_DT", dt);
            sgen.SetSession(MyGuid, "KMRNTYPE", null);

            //DataTable dt = sgen.getdata(userCode, mq);
            model.Add(tm1);

            return View(model);
        }
        [HttpPost]
        public ActionResult dir_coll(List<Tmodelmain> model, string command, string hftable, string hfrow)
        {
            FillMst();
            DataTable dtm = sgen.settable(hftable);
            model[0].dt1 = dtm;

            var tm = model.FirstOrDefault();
            DataTable dt = new DataTable();

            #region ddl
            List<SelectListItem> mod1 = new List<SelectListItem>();

            TempData[MyGuid + "_TList1"] = model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];



            if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };

            #endregion
            if (command == "New")
            {
                try
                {
                    sgen.SetSession(MyGuid, "EDMODE", "N");
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.scripCall = "enableForm();";

                    #region Currency

                    mq1 = "select master_id, master_name from master_setting where  type='CTM' " + model[0].Col11 + "";

                    dt = sgen.getdata(userCode, mq1);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            mod1.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                        }
                    }
                    TempData[MyGuid + "_Tlist1"] = model[0].TList1 = mod1;
                    #endregion

                    //mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10 + " where type in  ('DDECI' ,'ECI')" + model[0].Col11 + "";
                    //master_id = sgen.genNo(userCode, mq, 3, "auto_genid");


                    //model[0].Col16 = master_id;

                    //model[0].Col22 = "Y";


                }
                catch (Exception ex) { }
            }



            else if (command == "Cancel")
            {
                return CancelFun(model);
            }
            else if (command == "Callback")
            {
                if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
                model = CallbackFun(btnval, "N", "dir_coll", "Exim", model);
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
            }
            else if (command.Trim() == "Save" || command.Trim() == "Update")
            {
                try
                {
                    string found = "", inactive_date = "";
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime_local(userCode);
                    ent_date = sgen.Savedate(currdate, true);

                    DataTable dtstr = new DataTable();
                    dtstr = sgen.getdata(userCode, "select * from " + model[0].Col10 + " WHERE 1=2");

                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        mq1 = " and vch_num<>'" + tmodel.Col23.Trim() + "'";
                        isedit = true;
                        vch_num = tmodel.Col23.Trim();
                        master_id = tmodel.Col16.Trim();

                    }
                    else
                    {
                        isedit = false;



                        mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10 + " where type in  ('DDECI','ECI')" + model[0].Col11 + "";
                        master_id = sgen.genNo(userCode, mq, 3, "auto_genid");
                        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where type in   ('DDECI','ECI')" + model[0].Col11 + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");

                    }

                    found = sgen.getstring(userCode, "select master_name from " + model[0].Col10 + " a where upper(master_name)= upper('" + model[0].Col18 + "')" +
                        " and type in  ('DDECI','ECI') " + model[0].Col11 + " " + mq1 + "");
                    if (found != "")
                    {

                        ViewBag.scripCall = "showmsgJS(1, 'Data Already Exists', 2);";
                        goto END;
                    }

                    if (model[0].Col22 == "Y") { status = "Y"; inactive_date = currdate; model[0].Col12 = "ECI"; }
                    else { status = "N"; inactive_date = currdate; model[0].Col12 = "DDECI"; }
                    DataTable dataTable = sgen.getdata(userCode, "select  * from " + model[0].Col10 + " where 1=2");

                    for (int i = 0; i < model[0].dt1.Rows.Count; i++)
                    {
                        DataRow dr = dataTable.NewRow();

                        dr["vch_num"] = vch_num;
                        dr["vch_date"] = sgen.Savedate(model[0].Col17, false);
                        dr["type"] = model[0].Col12;
                        dr["Master_type"] = Master_Type;
                        dr["Master_Name"] = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(model[0].Col18.ToLower());
                        dr["master_id"] = master_id;
                        dr["Section_Strength"] = model[0].Col20.Trim();
                        dr["classid"] = model[0].Col19.Trim();
                        dr["sectionid"] = model[0].Col21.Trim();
                        dr["Status"] = status;
                        dr["Inactive_date"] = sgen.Savedate(inactive_date, false);


                        dr["client_id"] = clientid_mst;
                        dr["client_unit_id"] = unitid_mst;
                        if (isedit)
                        {
                            dr["master_entby"] = model[0].Col3.Trim();
                            dr["master_entdate"] = model[0].Col4.Trim();
                            dr["master_editby"] = userid_mst;
                            dr["master_editdate"] = currdate;
                            dr["client_id"] = model[0].Col1;
                            dr["client_unit_id"] = model[0].Col2;
                            dr["rec_id"] = model[0].Col7;

                        }
                        else
                        {
                            dr["rec_id"] = "0";
                            dr["master_editby"] = "-";
                            dr["master_editdate"] = currdate;
                            dr["master_entby"] = userid_mst;
                            dr["master_entdate"] = currdate;
                            dr["client_id"] = clientid_mst;
                            dr["client_unit_id"] = unitid_mst;
                        }

                        dataTable.Rows.Add(dr);
                    }
                    bool Result = sgen.Update_data(userCode, dataTable, model[0].Col10.Trim(), tmodel.Col8, isedit);
                    if (Result == true)
                    {
                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        //ViewBag.scripCall = "showmsgJS(1, 'Data Saved Successfully', 1);disableForm();";
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col10 = tm.Col10,
                            Col11 = tm.Col11,
                            Col13 = "Save",
                            Col14 = tm.Col14,
                            Col15 = tm.Col15,

                        });


                    }

                    else
                    {
                        ViewBag.scripCall = "showmsgJS(3, 'Record Not Saved', 1);";
                    }
                END:;

                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1, '" + ex.Message.ToString() + "', 1);"; }

            }
            else if (command == "-")
            {
                if (model[0].dt1.Rows.Count > 1) model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                else { model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "KMRN_DT"); }
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
            }


            ModelState.Clear();
            return View(model);

        }
        #endregion 

        #region DOC SUBMITTED TO BANK FOR COLLECTION FROM CUSTOMER
        public ActionResult bank_coll()
        {

            FillMst();
            //DataTable dt = new DataTable();
            chkRef();

            ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.vsave = "disabled='disabled'";
            ViewBag.scripCall = "disableForm();";
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);

            sgen.SetSession(MyGuid, "TR_MID", mid_mst.Trim());
            Tmodelmain tm1 = new Tmodelmain();
            List<Tmodelmain> model = new List<Tmodelmain>();


            tm1.Col9 = "DOC SUBMIT FOR COLLECTION TO BANK";
            tm1.Col10 = "sales"; //TABLE NAME
            tm1.Col13 = "Save";

            tm1.Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            tm1.Col12 = "ECI"; //TYPE
            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = MyGuid.Trim();

            List<SelectListItem> mod1 = new List<SelectListItem>();
            tm1.TList1 = mod1;
            tm1.SelectedItems1 = new string[] { "" };

            DataTable dt = sgen.getdata(userCode, "select '' Item,'1' as  SNo ,'-' as Icode,'-' as IName,'-' PARTNO,'-'  HSN,'-' as UOM,'0' TaxRate,'0' Qty_Chl,'0' Qty_In,'0' as IRate," +
                "'-' disc_type,'0' as disc_rate,'0' IAmount,'0' TaxAmount,'0' discAmount,'0' LineAmount,'-' as Remark,'0' PO_Number,'-' PO_Date from dual");
            tm1.dt1 = dt;
            sgen.SetSession(MyGuid, "KMRN_DT", dt);
            sgen.SetSession(MyGuid, "KMRNTYPE", null);

            //DataTable dt = sgen.getdata(userCode, mq);
            model.Add(tm1);

            return View(model);
        }
        [HttpPost]
        public ActionResult bank_coll(List<Tmodelmain> model, string command, string hftable, string hfrow)
        {
            FillMst();
            DataTable dtm = sgen.settable(hftable);
            model[0].dt1 = dtm;

            var tm = model.FirstOrDefault();
            DataTable dt = new DataTable();

            #region ddl
            List<SelectListItem> mod1 = new List<SelectListItem>();

            TempData[MyGuid + "_TList1"] = model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];

            if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };

            #endregion
            if (command == "New")
            {
                try
                {
                    sgen.SetSession(MyGuid, "EDMODE", "N");
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.scripCall = "enableForm();";

                    #region Currency

                    mq1 = "select master_id, master_name from master_setting where  type='CTM' " + model[0].Col11 + "";

                    dt = sgen.getdata(userCode, mq1);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            mod1.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                        }
                    }
                    TempData[MyGuid + "_Tlist1"] = model[0].TList1 = mod1;
                    #endregion


                    //mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10 + " where type in  ('DDECI' ,'ECI')" + model[0].Col11 + "";
                    //master_id = sgen.genNo(userCode, mq, 3, "auto_genid");


                    //model[0].Col16 = master_id;

                    //model[0].Col22 = "Y";


                }
                catch (Exception ex) { }
            }



            else if (command == "Cancel")
            {
                return CancelFun(model);
            }
            else if (command == "Callback")
            {
                if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
                model = CallbackFun(btnval, "N", "bank_coll", "Exim", model);
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
            }
            else if (command.Trim() == "Save" || command.Trim() == "Update")
            {
                try
                {
                    string found = "", inactive_date = "";
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime_local(userCode);
                    ent_date = sgen.Savedate(currdate, true);

                    DataTable dtstr = new DataTable();
                    dtstr = sgen.getdata(userCode, "select * from " + model[0].Col10 + " WHERE 1=2");

                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        mq1 = " and vch_num<>'" + tmodel.Col23.Trim() + "'";
                        isedit = true;
                        vch_num = tmodel.Col23.Trim();
                        master_id = tmodel.Col16.Trim();

                    }
                    else
                    {
                        isedit = false;



                        mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10 + " where type in  ('DDECI','ECI')" + model[0].Col11 + "";
                        master_id = sgen.genNo(userCode, mq, 3, "auto_genid");
                        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where type in   ('DDECI','ECI')" + model[0].Col11 + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");

                    }

                    found = sgen.getstring(userCode, "select master_name from " + model[0].Col10 + " a where upper(master_name)= upper('" + model[0].Col18 + "')" +
                        " and type in  ('DDECI','ECI') " + model[0].Col11 + " " + mq1 + "");
                    if (found != "")
                    {

                        ViewBag.scripCall = "showmsgJS(1, 'Data Already Exists', 2);";
                        goto END;
                    }

                    if (model[0].Col22 == "Y") { status = "Y"; inactive_date = currdate; model[0].Col12 = "ECI"; }
                    else { status = "N"; inactive_date = currdate; model[0].Col12 = "DDECI"; }
                    DataTable dataTable = sgen.getdata(userCode, "select  * from " + model[0].Col10 + " where 1=2");

                    for (int i = 0; i < model[0].dt1.Rows.Count; i++)
                    {
                        DataRow dr = dataTable.NewRow();

                        dr["vch_num"] = vch_num;
                        dr["vch_date"] = sgen.Savedate(model[0].Col17, false);
                        dr["type"] = model[0].Col12;
                        dr["Master_type"] = Master_Type;
                        dr["Master_Name"] = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(model[0].Col18.ToLower());
                        dr["master_id"] = master_id;
                        dr["Section_Strength"] = model[0].Col20.Trim();
                        dr["classid"] = model[0].Col19.Trim();
                        dr["sectionid"] = model[0].Col21.Trim();
                        dr["Status"] = status;
                        dr["Inactive_date"] = sgen.Savedate(inactive_date, false);


                        dr["client_id"] = clientid_mst;
                        dr["client_unit_id"] = unitid_mst;
                        if (isedit)
                        {
                            dr["master_entby"] = model[0].Col3.Trim();
                            dr["master_entdate"] = model[0].Col4.Trim();
                            dr["master_editby"] = userid_mst;
                            dr["master_editdate"] = currdate;
                            dr["client_id"] = model[0].Col1;
                            dr["client_unit_id"] = model[0].Col2;
                            dr["rec_id"] = model[0].Col7;

                        }
                        else
                        {
                            dr["rec_id"] = "0";
                            dr["master_editby"] = "-";
                            dr["master_editdate"] = currdate;
                            dr["master_entby"] = userid_mst;
                            dr["master_entdate"] = currdate;
                            dr["client_id"] = clientid_mst;
                            dr["client_unit_id"] = unitid_mst;
                        }

                        dataTable.Rows.Add(dr);
                    }
                    bool Result = sgen.Update_data(userCode, dataTable, model[0].Col10.Trim(), tmodel.Col8, isedit);
                    if (Result == true)
                    {
                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        //ViewBag.scripCall = "showmsgJS(1, 'Data Saved Successfully', 1);disableForm();";
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                        model = new List<Tmodelmain>();
                        tmodel.Col16 = "";
                        tmodel.Col17 = "";
                        tmodel.Col18 = "";
                        tmodel.Col19 = "";
                        tmodel.Col20 = "";
                        tmodel.Col21 = "";
                        tmodel.Col13 = "Save";
                        tmodel.Col9 = "DOC SUBMIT FOR COLLECTION TO BANK";
                        model.Add(tmodel);


                    }

                    else
                    {
                        ViewBag.scripCall = "showmsgJS(3, 'Record Not Saved', 1);";
                    }
                END:;

                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1, '" + ex.Message.ToString() + "', 1);"; }

            }
            else if (command == "-")
            {
                if (model[0].dt1.Rows.Count > 1) model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                else { model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "KMRN_DT"); }
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
            }


            ModelState.Clear();
            return View(model);

        }
        #endregion

        #region PAYMENT RECEIVED IN BANK 
        public ActionResult bank_rec()
        {

            FillMst();
            //DataTable dt = new DataTable();
            chkRef();

            ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.vsave = "disabled='disabled'";
            ViewBag.scripCall = "disableForm();";
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);

            sgen.SetSession(MyGuid, "TR_MID", mid_mst.Trim());
            Tmodelmain tm1 = new Tmodelmain();
            List<Tmodelmain> model = new List<Tmodelmain>();


            tm1.Col9 = "DOC SUBMIT FOR COLLECTION TO BANK";
            tm1.Col10 = "sales"; //TABLE NAME
            tm1.Col13 = "Save";

            tm1.Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            tm1.Col12 = "ECI"; //TYPE
            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = MyGuid.Trim();

            List<SelectListItem> mod1 = new List<SelectListItem>();
            tm1.TList1 = mod1;
            tm1.SelectedItems1 = new string[] { "" };

            DataTable dt = sgen.getdata(userCode, "select '' Item,'1' as  SNo ,'-' as Icode,'-' as IName,'-' PARTNO,'-'  HSN,'-' as UOM,'0' TaxRate,'0' Qty_Chl,'0' Qty_In,'0' as IRate," +
                "'-' disc_type,'0' as disc_rate,'0' IAmount,'0' TaxAmount,'0' discAmount,'0' LineAmount,'-' as Remark,'0' PO_Number,'-' PO_Date from dual");
            tm1.dt1 = dt;
            sgen.SetSession(MyGuid, "KMRN_DT", dt);
            sgen.SetSession(MyGuid, "KMRNTYPE", null);

            //DataTable dt = sgen.getdata(userCode, mq);
            model.Add(tm1);

            return View(model);
        }
        [HttpPost]
        public ActionResult bank_rec(List<Tmodelmain> model, string command, string hftable, string hfrow)
        {
            FillMst();
            DataTable dtm = sgen.settable(hftable);
            model[0].dt1 = dtm;

            var tm = model.FirstOrDefault();
            DataTable dt = new DataTable();

            #region ddl
            List<SelectListItem> mod1 = new List<SelectListItem>();

            TempData[MyGuid + "_TList1"] = model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];

            if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };

            #endregion
            if (command == "New")
            {
                try
                {
                    sgen.SetSession(MyGuid, "EDMODE", "N");
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.scripCall = "enableForm();";

                    #region Currency

                    mq1 = "select master_id, master_name from master_setting where  type='CTM' " + model[0].Col11 + "";

                    dt = sgen.getdata(userCode, mq1);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            mod1.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                        }
                    }
                    TempData[MyGuid + "_Tlist1"] = model[0].TList1 = mod1;
                    #endregion

                    //mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10 + " where type in  ('DDECI' ,'ECI')" + model[0].Col11 + "";
                    //master_id = sgen.genNo(userCode, mq, 3, "auto_genid");


                    //model[0].Col16 = master_id;

                    //model[0].Col22 = "Y";


                }
                catch (Exception ex) { }
            }



            else if (command == "Cancel")
            {
                return CancelFun(model);
            }
            else if (command == "Callback")
            {
                if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
                model = CallbackFun(btnval, "N", "bank_rec", "Exim", model);
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
            }
            else if (command.Trim() == "Save" || command.Trim() == "Update")
            {
                try
                {
                    string found = "", inactive_date = "";
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime_local(userCode);
                    ent_date = sgen.Savedate(currdate, true);

                    DataTable dtstr = new DataTable();
                    dtstr = sgen.getdata(userCode, "select * from " + model[0].Col10 + " WHERE 1=2");

                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        mq1 = " and vch_num<>'" + tmodel.Col23.Trim() + "'";
                        isedit = true;
                        vch_num = tmodel.Col23.Trim();
                        master_id = tmodel.Col16.Trim();

                    }
                    else
                    {
                        isedit = false;



                        mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10 + " where type in  ('DDECI','ECI')" + model[0].Col11 + "";
                        master_id = sgen.genNo(userCode, mq, 3, "auto_genid");
                        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where type in   ('DDECI','ECI')" + model[0].Col11 + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");

                    }

                    found = sgen.getstring(userCode, "select master_name from " + model[0].Col10 + " a where upper(master_name)= upper('" + model[0].Col18 + "')" +
                        " and type in  ('DDECI','ECI') " + model[0].Col11 + " " + mq1 + "");
                    if (found != "")
                    {

                        ViewBag.scripCall = "showmsgJS(1, 'Data Already Exists', 2);";
                        goto END;
                    }

                    if (model[0].Col22 == "Y") { status = "Y"; inactive_date = currdate; model[0].Col12 = "ECI"; }
                    else { status = "N"; inactive_date = currdate; model[0].Col12 = "DDECI"; }
                    DataTable dataTable = sgen.getdata(userCode, "select  * from " + model[0].Col10 + " where 1=2");

                    for (int i = 0; i < model[0].dt1.Rows.Count; i++)
                    {
                        DataRow dr = dataTable.NewRow();

                        dr["vch_num"] = vch_num;
                        dr["vch_date"] = sgen.Savedate(model[0].Col17, false);
                        dr["type"] = model[0].Col12;
                        dr["Master_type"] = Master_Type;
                        dr["Master_Name"] = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(model[0].Col18.ToLower());
                        dr["master_id"] = master_id;
                        dr["Section_Strength"] = model[0].Col20.Trim();
                        dr["classid"] = model[0].Col19.Trim();
                        dr["sectionid"] = model[0].Col21.Trim();
                        dr["Status"] = status;
                        dr["Inactive_date"] = sgen.Savedate(inactive_date, false);


                        dr["client_id"] = clientid_mst;
                        dr["client_unit_id"] = unitid_mst;
                        if (isedit)
                        {
                            dr["master_entby"] = model[0].Col3.Trim();
                            dr["master_entdate"] = model[0].Col4.Trim();
                            dr["master_editby"] = userid_mst;
                            dr["master_editdate"] = currdate;
                            dr["client_id"] = model[0].Col1;
                            dr["client_unit_id"] = model[0].Col2;
                            dr["rec_id"] = model[0].Col7;

                        }
                        else
                        {
                            dr["rec_id"] = "0";
                            dr["master_editby"] = "-";
                            dr["master_editdate"] = currdate;
                            dr["master_entby"] = userid_mst;
                            dr["master_entdate"] = currdate;
                            dr["client_id"] = clientid_mst;
                            dr["client_unit_id"] = unitid_mst;
                        }

                        dataTable.Rows.Add(dr);
                    }
                    bool Result = sgen.Update_data(userCode, dataTable, model[0].Col10.Trim(), tmodel.Col8, isedit);
                    if (Result == true)
                    {
                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        //ViewBag.scripCall = "showmsgJS(1, 'Data Saved Successfully', 1);disableForm();";
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col10 = tm.Col10,
                            Col11 = tm.Col11,
                            Col13 = "Save",
                            Col14 = tm.Col14,
                            Col15 = tm.Col15,

                        });


                    }

                    else
                    {
                        ViewBag.scripCall = "showmsgJS(3, 'Record Not Saved', 1);";
                    }
                END:;

                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1, '" + ex.Message.ToString() + "', 1);"; }

            }
            else if (command == "-")
            {
                if (model[0].dt1.Rows.Count > 1) model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                else { model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "KMRN_DT"); }
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
            }


            ModelState.Clear();
            return View(model);

        }
        #endregion

        #region Export Incentive Write off
        public ActionResult inc_wrt()
        {

            FillMst();
            chkRef();

            ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.vsave = "disabled='disabled'";
            ViewBag.scripCall = "disableForm();";
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);

            sgen.SetSession(MyGuid, "TR_MID", mid_mst.Trim());
            Tmodelmain tm1 = new Tmodelmain();
            List<Tmodelmain> model = new List<Tmodelmain>();

            tm1.Col9 = "INCENTIVE LICENCE WRITTEN OFF";
            tm1.Col10 = "master_setting"; //TABLE NAME
            tm1.Col13 = "Save";

            tm1.Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            tm1.Col12 = "ECC"; //TYPE
            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = MyGuid.Trim();

            List<SelectListItem> mod1 = new List<SelectListItem>();
            tm1.TList1 = mod1;
            tm1.SelectedItems1 = new string[] { "" };

            DataTable dt = sgen.getdata(userCode, "select '' LICENCE,'1' as  SNo ,'-' as LICENCE_NO,'-' as LICENCE_DATE, '-' LICENCE_EXP_DATE,'-' LICENCE_AMT,'-'  PENDING_AMT,'-' as AMT_WOFF,'-' as Remark from dual");
            tm1.dt1 = dt;
            sgen.SetSession(MyGuid, "KMRN_DT", dt);
            sgen.SetSession(MyGuid, "KMRNTYPE", null);
            model.Add(tm1);

            return View(model);
        }
        [HttpPost]
        public ActionResult inc_wrt(List<Tmodelmain> model, string command, string hftable, string hfrow)
        {
            FillMst();
            DataTable dtm = sgen.settable(hftable);
            model[0].dt1 = dtm;

            var tm = model.FirstOrDefault();
            DataTable dt = new DataTable();

            #region ddl
            List<SelectListItem> mod1 = new List<SelectListItem>();

            TempData[MyGuid + "_TList1"] = model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];

            if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };

            #endregion

            if (command == "New")
            {
                sgen.SetSession(MyGuid, "EDMODE", "N");
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
                ViewBag.scripCall = "enableForm();";
                // mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10 + " where type in  ('DDECC' ,'ECC')" + model[0].Col11 + "";
                // master_id = sgen.genNo(userCode, mq, 3, "auto_genid");

                #region Bank Name

                mq1 = "select master_id, master_name from master_setting where  type='CTM' " + model[0].Col11 + "";

                dt = sgen.getdata(userCode, mq1);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        mod1.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                    }
                }
                TempData[MyGuid + "_Tlist1"] = model[0].TList1 = mod1;
                #endregion

            }


            else if (command == "Cancel")
            {
                return CancelFun(model);
            }
            else if (command == "Callback")
            {
                if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
                model = CallbackFun(btnval, "N", "bill_of_entry", "Exim", model);
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
            }
            else if (command.Trim() == "Save" || command.Trim() == "Update")
            {
                try
                {
                    string found = "", inactive_date = "";
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime_local(userCode);
                    ent_date = sgen.Savedate(currdate, true);

                    DataTable dtstr = new DataTable();
                    dtstr = sgen.getdata(userCode, "select * from " + model[0].Col10 + " WHERE 1=2");

                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        mq1 = " and vch_num<>'" + tmodel.Col23.Trim() + "'";
                        isedit = true;
                        vch_num = tmodel.Col23.Trim();
                        master_id = tmodel.Col16.Trim();

                    }
                    else
                    {
                        isedit = false;



                        mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10 + " where type in  ('DDECC','ECC')" + model[0].Col11 + "";
                        master_id = sgen.genNo(userCode, mq, 3, "auto_genid");
                        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where type in   ('DDECC','ECC')" + model[0].Col11 + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");

                    }

                    found = sgen.getstring(userCode, "select master_name from " + model[0].Col10 + " a where upper(master_name)= upper('" + model[0].Col18 + "')" +
                        " and type in  ('DDECC','ECC') " + model[0].Col11 + " " + mq1 + "");
                    if (found != "")
                    {

                        ViewBag.scripCall = "showmsgJS(1, 'Data Already Exists', 2);";
                        goto END;
                    }

                    if (model[0].Col22 == "Y") { status = "Y"; inactive_date = currdate; model[0].Col12 = "ECC"; }
                    else { status = "N"; inactive_date = currdate; model[0].Col12 = "DDECC"; }
                    DataTable dataTable = sgen.getdata(userCode, "select  * from " + model[0].Col10 + " where 1=2");
                    DataRow dr = dataTable.NewRow();

                    dr["vch_num"] = vch_num;
                    dr["vch_date"] = sgen.Savedate(model[0].Col17, false);
                    dr["type"] = model[0].Col12;
                    dr["Master_type"] = Master_Type;
                    dr["Master_Name"] = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(model[0].Col18.ToLower());
                    dr["master_id"] = master_id;
                    dr["Section_Strength"] = model[0].Col20.Trim();
                    dr["classid"] = model[0].Col19.Trim();
                    dr["sectionid"] = model[0].Col21.Trim();
                    dr["Status"] = status;
                    dr["Inactive_date"] = sgen.Savedate(inactive_date, false);


                    dr["client_id"] = clientid_mst;
                    dr["client_unit_id"] = unitid_mst;
                    if (isedit)
                    {
                        dr["master_entby"] = model[0].Col3.Trim();
                        dr["master_entdate"] = model[0].Col4.Trim();
                        dr["master_editby"] = userid_mst;
                        dr["master_editdate"] = currdate;
                        dr["client_id"] = model[0].Col1;
                        dr["client_unit_id"] = model[0].Col2;
                        dr["rec_id"] = model[0].Col7;

                    }
                    else
                    {
                        dr["rec_id"] = "0";
                        dr["master_editby"] = "-";
                        dr["master_editdate"] = currdate;
                        dr["master_entby"] = userid_mst;
                        dr["master_entdate"] = currdate;
                        dr["client_id"] = clientid_mst;
                        dr["client_unit_id"] = unitid_mst;
                    }

                    dataTable.Rows.Add(dr);

                    bool Result = sgen.Update_data(userCode, dataTable, model[0].Col10.Trim(), tmodel.Col8, isedit);
                    if (Result == true)
                    {
                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        //ViewBag.scripCall = "showmsgJS(1, 'Data Saved Successfully', 1);disableForm();";
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";

                        model = new List<Tmodelmain>();
                        tmodel.Col16 = "";
                        tmodel.Col17 = "";
                        tmodel.Col18 = "";
                        tmodel.Col19 = "";
                        tmodel.Col20 = "";
                        tmodel.Col21 = "";
                        tmodel.Col13 = "Save";
                        tmodel.Col9 = "FILLING OF EXPORT INCENTIVE";
                        model.Add(tmodel);


                    }

                    else
                    {
                        ViewBag.scripCall = "showmsgJS(3, 'Record Not Saved', 1);";
                    }
                END:;

                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1, '" + ex.Message.ToString() + "', 1);"; }

            }
            else if (command == "-")
            {
                if (model[0].dt1.Rows.Count > 1) model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                else { model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "KMRN_DT"); }
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
            }


            ModelState.Clear();
            return View(model);

        }
        #endregion
    }
}

