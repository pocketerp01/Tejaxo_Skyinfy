using DevTrends.MvcDonutCaching;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using skyinfyMVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Dynamic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Oracle.ManagedDataAccess.Client;
//using Oracle.DataAccess.Client;

namespace skyinfyMVC.Controllers
{
    public class HomeController : Controller
    {
        static string mq0 = "", mq = "", mq1 = "", mq2 = "", vch_num = "", btnval = "", tab_name = "", type = "", type_desc = "", tab_name1 = "", mid_mst = "", m_id_mst = "", CUP_id = "", mqm = "";
        //=========== HR =================
        string havetransport = "", isperadd = "N", isdisable = "N", sameas = "", gender = "", ismarried = "N", isspouseworking = "N",
            havechild = "", language = "", ent_date = "", emp_status = "N", emp_code = "", vpf = "N", ispflimit = "N";
        int comp_count = 0, cust_count = 0, unit_count = 0, custunit_count = 0;
        string fileName1 = "", ctype1 = "", finalpath1 = "", year_from = "", year_to = "", xprdrange = "", path1 = "", encpath1 = "";
        public static string MyGuid = "";
        static string reslt = "";
        //============================
        bool res = false, ok = false;
        public static sgenFun sgen;
        Cmd_Fun cmd_Fun;
        public static bool isedit = false, isuser = false;
        public static int cli = 0;
        public static string userCode = "", subdomain_mst = "", userid_mst = "", cg_com_name = "", clientid_mst = "", unitid_mst = "", html = "", Ac_Year_id = "", msg = "", role_mst = "",
            controls_mst = "", actionName = "", controllerName = "", clientname_mst = "", type1 = "", unitname_mst;

        #region main func
        public void FillMst(string Myguid = "")
        {
            MyGuid = Myguid;
            //sgen = new sgenFun(MyGuid);
            if (MyGuid == "")
            {
                MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            }

            Multiton multiton = Multiton.GetInstance(MyGuid);
            sgen = new sgenFun(MyGuid);
            cmd_Fun = new Cmd_Fun(MyGuid);
            userCode = multiton.UserCode;
            userid_mst = sgen.GetCookie(MyGuid, "userid_mst");
            Ac_Year_id = sgen.GetCookie(MyGuid, "Ac_Year_id");
            cg_com_name = sgen.GetCookie(MyGuid, "cg_com_name");
            clientid_mst = sgen.GetCookie(MyGuid, "clientid_mst");
            unitid_mst = sgen.GetCookie(MyGuid, "unitid_mst");
            subdomain_mst = sgen.GetCookie(MyGuid, "subdomain_mst");
            role_mst = sgen.GetCookie(MyGuid, "role_mst");
            controls_mst = sgen.GetCookie(MyGuid, "controls_mst");
            unitname_mst = sgen.GetCookie(MyGuid, "unitname_mst");
            clientname_mst = sgen.GetCookie(MyGuid, "clientname_mst");
            actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            sgen.SetSession(MyGuid, "viewName", actionName);
            sgen.SetSession(MyGuid, "controllerName", controllerName);

            year_to = sgen.GetCookie(MyGuid, "year_to");
            year_from = sgen.GetCookie(MyGuid, "year_from");
            xprdrange = "and vch_DATE between " + year_from + " and " + year_to + "";
        }

        public List<Tmodelmain> getload(List<Tmodelmain> model)
        {
            //chkRef();
            Tmodelmain tm1 = new Tmodelmain();
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            m_id_mst = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.vsave = "disabled='disabled'";
            ViewBag.vsavenew = "disabled='disabled'";
            ViewBag.scripCall = "disableForm();";
            tm1.Col13 = "Save";
            tm1.Col100 = "Save & New";
            tm1.Col121 = "S";
            tm1.Col122 = "<u>S</u>ave";
            tm1.Col123 = "Save & Ne<u>w</u>";
            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = m_id_mst.Trim();
            model.Add(tm1);
            return model;
        }

        public List<Tmodelmain> getnew(List<Tmodelmain> model)
        {
            sgen.SetSession(MyGuid, "EDMODE", "N");
            ViewBag.vnew = "disabled='disabled'";
            ViewBag.vedit = "disabled='disabled'";
            ViewBag.vsave = "";
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

        public DataRow getsave(string curdt, DataRow drn, List<Tmodelmain> model)
        {
            bool edit = false;
            if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
            {
                edit = true;
                vch_num = model[0].Col16.Trim();
            }
            else
            {
                edit = false;
                mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
            }

            drn["vch_num"] = vch_num;
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

        public void chkRef()
        {
            if (userCode.Equals("")) Response.Redirect(sgenFun.callbackurl);
            if (Request.UrlReferrer == null) { Response.Redirect(sgenFun.callbackurl); }
        }

        #endregion

        #region
        public ActionResult Index()
        {

            if (userCode.Equals("")) return RedirectToAction(sgenFun.callbackmvcAct, sgenFun.callbackmvcCtr);

            //string htmls = "";
            //try
            //{
            //    htmls = Mymenu();
            //}
            //catch (Exception err) {
            //    htmls = "<div></div>";
            //}
            //ViewBag.HtmlStr = htmls;
            return View();
        }

        public ActionResult invoice_config()
        {
            if (userCode.Equals("")) return RedirectToAction(sgenFun.callbackmvcAct, sgenFun.callbackmvcCtr);
            //Tmodel10 tm = new Tmodel10();
            string mq = "select 'Y' as vchnum,'7' as vchdate,'7' as name";
            DataTable dt = sgen.getdata("SATECH", mq);
            ViewData["dt1"] = dt;
            //        List<Tmodel10> mod = new List<Tmodel10>();
            //        mod = dt.AsEnumerable().Select(row =>
            //new Tmodel10
            //{

            //    col1 = row.Field<string>("vchnum"),
            //    col2 = row.Field<string>("vchdate"),
            //    Col3 = row.Field<string>("name"),
            //    chk1 = (row.Field<string>("vchnum").Equals("Y")) ? true : false
            //}).ToList();
            return View();

        }
        #endregion

        #region login
        //public ActionResult Login()
        //{
        //    if (userCode.Equals("")) Response.Redirect("~/erp/login_main.aspx");
        //    DataTable dt = new DataTable();
        //    sgen.SetCookie(MyGuid, "val1", "Ram");
        //    Session["val1"] = "rama";
        //    dt = sgen.getdata(userCode, "select m_id,m_name,m_link,m_order from menus limit 10");

        //    //        List<Tmodel> mod = new List<Tmodel>();
        //    //        mod = dt.AsEnumerable().Select(row =>
        //    //new Tmodel
        //    //{
        //    //    col1 = row.Field<string>("m_id"),
        //    //    col2 = row.Field<string>("m_name")
        //    //}).ToList();
        //    //dynamic mymodel = new ExpandoObject();
        //    //mymodel.modules = mod;
        //    //mymodel.makes = new Automobile();

        //    return View();
        //}
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult TermsAccept(List<Tmodelmain> model)
        {

            //var tmodel = model.FirstOrDefault();
            //string name = tmodel.col1;
            //string pass = tmodel.col2;
            //string m_id = sgen.seekval(userCode, "select m_id from menus limit 1", "m_id");
            //tmodel.col2 = m_id;
            //ModelState.Clear();
            return View("login");

        }
        [HttpPost]
        public ContentResult ReceiveWithRequestFormData()
        {
            Tmodel10 tm = new Tmodel10();
            string mq = "select 'Y' as vchnum,'2' as vchdate,'3' as name";

            DataTable dt = sgen.getdata("SATECH", mq);
            ViewData["dt1"] = dt;

            List<Tmodel10> mod = new List<Tmodel10>();
            mod = dt.AsEnumerable().Select(row =>
    new Tmodel10
    {
        Col1 = row.Field<string>("vchnum"),
        Col2 = row.Field<string>("vchdate"),
        Col3 = row.Field<string>("name"),
        Chk1 = (row.Field<string>("vchnum").Equals("Y")) ? true : false
    }).ToList();
            var name = Request.Form["txtram"];
            ViewData["txtram"] = name;

            return Content("");
        }
        [HttpPost]
        public ActionResult Btnview(List<Tmodel10> tmodel)
        {
            return View("invoce_config", tmodel);
        }
        public ContentResult Setsession(string name, string source, string dtname, string view, string controller)
        {

            bool ok = false;
            try
            {
                DataTable dtJS = ConvertJsonToDatatable(source);
                userCode = sgen.GetCookie(MyGuid, "userCode");
                string table_name = EncryptDecrypt.Decrypt_new(dtname);

                DataTable dtm = sgen.getdata(userCode, "select * from " + EncryptDecrypt.Decrypt_new(dtname) + " where 1=2");
                foreach (DataRow drj in dtJS.Rows)
                {
                    DataRow drs = dtm.NewRow();
                    foreach (DataColumn dc in dtJS.Columns)
                    {
                        drs[EncryptDecrypt.Decrypt_new(dc.ColumnName)] = drj[dc.ColumnName].ToString();
                    }
                    dtm.Rows.Add(drs);
                }
                ok = sgen.Update_data(userCode, dtm, table_name, "", false);
            }
            catch (Exception err)
            { }

            return Content(ok.ToString());
        }
        public static DataTable ConvertJsonToDatatable(string jsonString)
        {
            DataTable dt = new DataTable();
            JArray jsonArray1 = JArray.Parse(jsonString);
            int rows = jsonArray1.Descendants().Where(x => x is JArray).Count();
            var linqArray = jsonArray1.Descendants().Where(x => x is JArray).First();
            var jsonArray = new JArray();
            foreach (JObject row in linqArray.Children<JObject>())
            {
                foreach (JProperty column in row.Properties())
                {
                    dt.Columns.Add(column.Name);
                }
            }
            for (int a = 0; a < rows; a++)
            {
                DataRow dr = dt.NewRow();
                linqArray = jsonArray1.Descendants().Where(x => x is JArray).ElementAt(a);
                jsonArray = new JArray();
                foreach (JObject row in linqArray.Children<JObject>())
                {
                    foreach (JProperty column in row.Properties())
                    {
                        if (column.Value is JValue)
                        {
                            dr[column.Name] = column.Value;
                        }
                    }

                }
                dt.Rows.Add(dr);
            }
            return dt;

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TermsDecline()
        {
            //if (ModelState.IsValid)
            //{
            //    user.TermsStatus = "Declined";
            //}
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region popups
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PopUp()
        {
            //sgen.Open_pageMVC(ViewBag, "abcd", "-", "-", 2, "800px", "80%");
            //if (ModelState.IsValid)
            //{
            //    user.TermsStatus = "Declined";
            //}            

            return RedirectToAction("login", "Home");
        }
        public ActionResult PopUp1(string title, string query, int seektype)
        {
            query = "select * from menus";
            sgen.open_grid(title, query, seektype);
            ViewBag.pageurl = sgen.GetSession(MyGuid, "pageurl").ToString();
            return RedirectToAction("login", "Home");
        }
        #endregion

        #region make query
        public void Make_query(string viewname, string title, string btnval, string seektype, string param1, string param2, string Myguid)
        {
            FillMst(Myguid);
            sgen.SetSession(MyGuid, "btnval", btnval);
            clientid_mst = sgen.GetCookie(MyGuid, "clientid_mst");
            unitid_mst = sgen.GetCookie(MyGuid, "unitid_mst");
            string cmd = "";
            switch (viewname.ToLower())
            {
                #region Master_Page
                case "master":
                    switch (btnval)
                    {
                        case "7005.6":

                            //                 cmd = "select (ud.vch_num|| to_char(ud.vch_date, 'yyyymmdd')|| ud.type) as fstr," +
                            //"(ud.FIRST_NAME||' '|| REPLACE(ud.MIDDLE_NAME, '0', '')||' '|| REPLACE(ud.LAST_NAME, '0', '')) as Student_Name," +
                            //"(ud.f_firstname||' '|| REPLACE(ud.f_middlename, '0', '')||' '|| REPLACE(ud.f_lastname, '0', '')) as Father_Name," +
                            //"(ud.m_firstname||' '|| REPLACE(ud.m_middlename, '0', '')||' '|| REPLACE(ud.m_lastname, '0', '')) as Mother_Name," +
                            //"(ud.g_firstname||' '|| REPLACE(ud.g_middlename, '0', '')||' '|| REPLACE(ud.g_lastname, '0', '')) as Gaurdian_Name,ud.Status," +
                            //"(case when to_char(ud.dob,'" + sgen.Getsqldateformat() + "')='01/01/1900' then '-' else to_char(ud.dob,'" + sgen.Getsqldateformat() + "') end) as DOB,ud.blood_grp,ud.gender as Gender,ud.place_of_birth as Birth_Place," +
                            //"ud.main_language as M_Tongue,nvl(cl.class,'-') as Class,nvl(sd.roll_number,'-') as rollno,nvl(sec.master_name,'-') as section," +
                            //"nvl(ad.academic_year,'-') as Acd_Year,ud.adhaar_id as Adhaar_No,nvl(rl.master_name,'-') as Religion,nvl(cc.master_name,'-') as Caste," +
                            //"ud.NATIONALITY,ud.issibling as Is_Sibling,ud.sibling2_rollno as Sibling_Grp_Id,ud.isfamily as Is_Family,ud.sibling1_rollno as Family_Grp_Id," +
                            //"ud.scholarship as Scholar,nvl(ud.role,'-') as Role,ud.prev_school_name as Last_Sch,ud.regnumber as Reg_No,ud.srn_no as SRN_No," +
                            //"ud.withdrawl_no as Withdrawl_No,ud.enrollment_no as Enrol_No," +
                            //"(CASE WHEN ud.f_dob= ud.created_date THEN '0' ELSE (case when to_char(ud.f_dob,'" + sgen.Getsqldateformat() + "')='01/01/1900' then '-' else to_char(ud.f_dob,'" + sgen.Getsqldateformat() + "') end) end) as F_DOB,ud.f_email as F_Email,ud.f_adhaar_id as F_AdhaarNo,nvl(fqa.master_name,'-') as f_qual,ud.f_occupation as F_Occup,ud.f_designation as F_Desig,ud.f_org_name as F_Org,fann.master_name as F_Annl_In,ud.f_con_number as F_ContactNo,ud.f_alumni_type as F_Alumni,ud.f_nationality," +
                            // "(CASE WHEN ud.m_dob=ud.created_date THEN '0' ELSE (case when to_char(ud.m_dob,'" + sgen.Getsqldateformat() + "')='01/01/1900' then '-' else to_char(ud.m_dob,'" + sgen.Getsqldateformat() + "') end) end) as M_DOB,ud.m_email as M_Email,ud.m_adhaar_id as M_AdhaarNo,nvl(mqa.master_name,'-') as m_qual,ud.m_occupation as M_Occup,ud.m_designation as M_Desig,ud.m_org_name as M_Org,mann.master_name as M_Annl_Inc,ud.m_con_number as M_ContactNo,ud.m_alumni_type as M_Alumni,ud.m_nationality," +
                            // "(CASE WHEN ud.g_dob=ud.created_date THEN '0' ELSE (case when to_char(ud.g_dob,'" + sgen.Getsqldateformat() + "')='01/01/1900' then '-' else to_char(ud.g_dob,'" + sgen.Getsqldateformat() + "') end) end) as G_DOB,ud.g_email as G_Email,ud.g_adhaar_id as G_AdhaarNo,nvl(gqa.master_name,'-') as g_qual,ud.g_occupation as G_Occup,ud.g_designation as G_Desig,ud.g_org_name as G_Org,ud.g_con_number as G_ContactNo,ud.g_nationality " +
                            //"from user_details ud inner join student_detail sd on sd.reg_no=ud.RegNumber and sd.type= ud.type and sd.academic_year_id='" + Ac_Year_id + "' " +
                            //"left outer join add_class cl on sd.class_applied=cl.add_class_id and sd.client_unit_id= cl.client_unit_id and cl.type= 'EAC' " +
                            //"left outer join add_academic_year ad on sd.academic_year_id= ad.academic_year_id and sd.client_id= ad.client_id and ad.type= 'ACY' " +
                            //"left outer join master_setting sec on sd.section= sec.master_id and find_in_set(sd.client_unit_id,sec.client_unit_id)=1 and sec.type= 'ECS' " +
                            //"left outer join master_setting fann on ud.f_annual_income= fann.master_id and find_in_set(fann.client_unit_id,'" + unitid_mst + "')=1 and fann.type= 'KAI' " +
                            //"left outer join master_setting mann on ud.m_annual_income= mann.master_id and find_in_set(mann.client_unit_id,'" + unitid_mst + "')=1 and mann.type= 'KAI' " +
                            //"left outer join master_setting fqa on ud.f_qualification= fqa.master_id and find_in_set(ud.client_unit_id, fqa.client_unit_id)=1 and fqa.type= 'EQU' " +
                            //"left outer join master_setting mqa on ud.m_qualification= mqa.master_id and find_in_set(ud.client_unit_id, mqa.client_unit_id)=1 and mqa.type= 'EQU' " +
                            //"left outer join master_setting gqa on ud.g_qualification= gqa.master_id and find_in_set(ud.client_unit_id, gqa.client_unit_id )=1and gqa.type= 'EQU' " +
                            //"left outer join master_setting cc on ud.ucaste= cc.master_id and cc.type= 'ECT' " +
                            //"left outer join master_setting rl on ud.religion= rl.master_id and rl.type= 'ERT' " +
                            //"left outer join(select distinct country_name, alpha_2 from country_state) cs on cs.alpha_2=ud.nationality " +
                            //"left outer join(select distinct country_name, alpha_2 from country_state) fcs on fcs.alpha_2=ud.f_nationality " +
                            //"left outer join(select distinct country_name, alpha_2 from country_state) mcs on mcs.alpha_2=ud.m_nationality " +
                            //"left outer join(select distinct country_name, alpha_2 from country_state) gcs on gcs.alpha_2=ud.g_nationality " +
                            //"WHERE ud.client_unit_id= '" + unitid_mst + "' and ud.type= 'STD' and ud.status='active'   order by ud.vch_num desc";

                            cmd = "select (ud.vch_num|| to_char(ud.vch_date, 'yyyymmdd')|| ud.type) as fstr," +
          "(ud.FIRST_NAME||' '|| REPLACE(ud.MIDDLE_NAME, '0', '')||' '|| REPLACE(ud.LAST_NAME, '0', '')) as Student_Name," +
          "(ud.f_firstname||' '|| REPLACE(ud.f_middlename, '0', '')||' '|| REPLACE(ud.f_lastname, '0', '')) as Father_Name," +
          "(ud.m_firstname||' '|| REPLACE(ud.m_middlename, '0', '')||' '|| REPLACE(ud.m_lastname, '0', '')) as Mother_Name," +
          "(ud.g_firstname||' '|| REPLACE(ud.g_middlename, '0', '')||' '|| REPLACE(ud.g_lastname, '0', '')) as Gaurdian_Name,ud.Status," +
          "(case when to_char(ud.dob,'" + sgen.Getsqldateformat() + "')='01/01/1900' then '-' else to_char(ud.dob,'" + sgen.Getsqldateformat() + "') end) as DOB,ud.blood_grp,ud.gender as Gender,ud.place_of_birth as Birth_Place," +
          "ud.main_language as M_Tongue,nvl(cl.class,'-') as Class,nvl(sd.roll_number,'-') as rollno,nvl(sec.master_name,'-') as section," +
          "nvl(ad.academic_year,'-') as Acd_Year,ud.adhaar_id as Adhaar_No,nvl(rl.master_name,'-') as Religion,nvl(cc.master_name,'-') as Caste," +
          "ud.NATIONALITY,ud.issibling as Is_Sibling,ud.sibling2_rollno as Sibling_Grp_Id,ud.isfamily as Is_Family,ud.sibling1_rollno as Family_Grp_Id," +
          "ud.scholarship as Scholar,nvl(ud.role,'-') as Role,ud.prev_school_name as Last_Sch,ud.regnumber as Reg_No,ud.srn_no as SRN_No," +
          "ud.withdrawl_no as Withdrawl_No,ud.enrollment_no as Enrol_No," +
          "(CASE WHEN ud.f_dob= ud.created_date THEN '0' ELSE (case when to_char(ud.f_dob,'" + sgen.Getsqldateformat() + "')='01/01/1900' then '-' else to_char(ud.f_dob,'" + sgen.Getsqldateformat() + "') end) end) as F_DOB,ud.f_email as F_Email,ud.f_adhaar_id as F_AdhaarNo,nvl(fqa.master_name,'-') as f_qual,ud.f_occupation as F_Occup,ud.f_designation as F_Desig,ud.f_org_name as F_Org,fann.master_name as F_Annl_In,ud.f_con_number as F_ContactNo,ud.f_alumni_type as F_Alumni,ud.f_nationality," +
           "(CASE WHEN ud.m_dob=ud.created_date THEN '0' ELSE (case when to_char(ud.m_dob,'" + sgen.Getsqldateformat() + "')='01/01/1900' then '-' else to_char(ud.m_dob,'" + sgen.Getsqldateformat() + "') end) end) as M_DOB,ud.m_email as M_Email,ud.m_adhaar_id as M_AdhaarNo,nvl(mqa.master_name,'-') as m_qual,ud.m_occupation as M_Occup,ud.m_designation as M_Desig,ud.m_org_name as M_Org,mann.master_name as M_Annl_Inc,ud.m_con_number as M_ContactNo,ud.m_alumni_type as M_Alumni,ud.m_nationality," +
           "(CASE WHEN ud.g_dob=ud.created_date THEN '0' ELSE (case when to_char(ud.g_dob,'" + sgen.Getsqldateformat() + "')='01/01/1900' then '-' else to_char(ud.g_dob,'" + sgen.Getsqldateformat() + "') end) end) as G_DOB,ud.g_email as G_Email,ud.g_adhaar_id as G_AdhaarNo,nvl(gqa.master_name,'-') as g_qual,ud.g_occupation as G_Occup,ud.g_designation as G_Desig,ud.g_org_name as G_Org,ud.g_con_number as G_ContactNo,ud.g_nationality " +
          "from user_details ud inner join student_detail sd on sd.reg_no=ud.RegNumber and ud.vch_num=sd.vch_num and sd.type= ud.type and sd.academic_year_id='" + Ac_Year_id + "' " +
          "left outer join add_class cl on sd.class_applied=cl.add_class_id and cl.client_unit_id='" + unitid_mst + "' and cl.type= 'EAC' " +
          "left outer join add_academic_year ad on sd.academic_year_id= ad.academic_year_id and ad.client_id='" + unitid_mst + "' and ad.type= 'ACY' " +
          "left outer join master_setting sec on sd.section= sec.master_id and find_in_set(sec.client_unit_id,'" + unitid_mst + "')=1 and sec.type= 'ECS' " +
          "left outer join master_setting fann on ud.f_annual_income= fann.master_id and find_in_set(fann.client_unit_id,'" + unitid_mst + "')=1 and fann.type= 'KAI' " +
          "left outer join master_setting mann on ud.m_annual_income= mann.master_id and find_in_set(mann.client_unit_id,'" + unitid_mst + "')=1 and mann.type= 'KAI' " +
          "left outer join master_setting fqa on ud.f_qualification= fqa.master_id and find_in_set(fqa.client_unit_id, '" + unitid_mst + "')=1 and fqa.type= 'EQU' " +
          "left outer join master_setting mqa on ud.m_qualification= mqa.master_id and find_in_set(mqa.client_unit_id, '" + unitid_mst + "')=1 and mqa.type= 'EQU' " +
          "left outer join master_setting gqa on ud.g_qualification= gqa.master_id and find_in_set(gqa.client_unit_id, '" + unitid_mst + "')=1and gqa.type= 'EQU' " +
          "left outer join master_setting cc on ud.ucaste= cc.master_id and cc.type= 'ECT' " +
          "left outer join master_setting rl on ud.religion= rl.master_id and rl.type= 'ERT' " +
           "WHERE ud.client_unit_id= '" + unitid_mst + "' and ud.type= 'STD' and ud.status='active' " +
           "  order by ud.vch_num desc";
                            break;
                        // reena 
                        case "40002.6":
                        case "28005.7":
                        case "21008.2":

                            string condition = "", cond = "";

                            //if (role_mst != "owner")
                            //{

                            //    string view_user = sgen.getstring(userCode, "select param1 from controls where type='CTL' and" +
                            //        " client_unit_id='" + unitid_mst + "' and id='000006' and m_module3='crmvmain' and param5='crm'");

                            //    if (view_user == "Y")
                            //    {

                            //        cond = "and a.ent_by='" + userid_mst + "'";
                            //    }
                            //    else { cond = ""; }
                            //}
                            //cmd = "select (a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr" +
                            //    " ,a.vch_num  Doc_no,replace(nvl(pr.c_name,'-'),0,'-') as Parent_Account," +
                            //            "a.c_name as Name,a.pincode as Pincode,a.isgstr IsGst,a.c_gstin as GSTIN,a.type_desc as Search_text," +
                            //            "a.cpname as PerName, (case when a.cplandno='0000000000' then '' else a.cplandno end)  MOBILE_NO,(case when a.cpcont='0000000000' then '' else a.cpcont end) as PerContact,(case when a.cpaltcont='0000000000' then '' else a.cpaltcont end)  as PerAltContact,(case when (a.cpemail='ab@ab.ab' or a.cpemail='0' ) then '' else a.cpemail end )as PerEmail,a.cpdesig " +
                            //            "as PerDesig,a.file_no, d.First_name ||' ' || Replace(d.middle_name,'0','') || ' '|| Replace(d.last_name,'0','') as Entry_By," +
                            //            " to_char( convert_tz( a.ent_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') as Entry_Date " +
                            //            "from clients_mst a left join clients_mst pr" +
                            //        "  on a.parent_id=pr.vch_num and pr.type='BCD' and  find_in_set(pr.client_unit_id,'" + unitid_mst + "')= 1 inner join user_details d on  a.ent_by =d.rec_id where a.type in ('BCD','BCDDD') and a.client_id='" +
                            //            clientid_mst + "' and a.client_unit_id='" + unitid_mst + "' and substr(a.vch_num,0,3)='303' " + cond + "  order by a.vch_num desc";
                            string cn_type = "", prefix = "", param5 = "";
                            if ((btnval == "40002.6") || (btnval == "21008.2")) { prefix = "303"; param5 = "CUSTOMER DETAIL"; }
                            else { prefix = "203"; param5 = "VENDOR DETAIL"; }
                            type = "BCD";
                            cn_type = "BCD"; ;
                            cond = "fstr,Parent_Account";
                            //string condition = "", cond = "fstr";

                            condition = condition + "and substr(a.vch_num,0,3)='" + prefix + "'";
                            condition = condition + " and (a.mftr is null or a.mftr='0' or a.mftr='N') and (a.pubr is null or a.pubr='0' or a.pubr='N')";

                            DataTable tmp = sgen.getdata(userCode, "select id,param1,param2,upper(param3) param3 ,name from controls where trim(upper(param5))='" + param5 + "' and type='VDC' and client_id='" + clientid_mst + "' order by id");
                            sgen.SetSession(MyGuid, "tmp_client", tmp);
                            if (sgen.GetSession(MyGuid, "tmp_client") != null)
                            {
                                mq = @"select '-' as fstr,vch_num as doc_no , c_name ORGANISATION_NAME,panno PAN_NUMBER ,comp_email ORG_EMAIL_ID ,
                                    cpname cp_first_name,cp_mname CP_MIDDLE_NAME, cp_lname CP_LAST_NAME,cplandno CONTACT_P_MOBILE_NO ,cpcont CONTACT_P_CONTACT_NO,
                                    cpaltcont CONTACT_P_ALTERNATE_CONTACT_NO,DOB CONTACT_P_DATE_OF_BIRTH, DOA CONTACT_P_DATE_OF_ANNIVERSARY , addtype1 TYPE_OF_ADDRESS
                                    ,COUNTRY,STATE,CITY,addr ADDRESS, PINCODE,
                                    tanno TAN_NUMBER,msmeno MSME_NUMBER, WEBSITE ,salesarea SALES_SERVICE_AREA, type_desc SEARCH_TEXT,ptype ORGANISATION_STATUS,ploc PARTY_LOCATION,
                                    client_type TYPE_OF_ACCOUNT, contr LABOUR_CONTRACTOR,SEZ ,isgstr GST, c_gstin GSTIN_NUMBER,tor TYPE_OF_GST_REGISTRATION, addtype1 BILLING_TYPE_OF_ADDRESS,
                                    country2 BILLING_COUNTRY, state2 BILLING_STATE,city2 BILLING_CITY, cpaddr2 BILLING_ADDRESS,pincode_2 BILLING_PINCODE,cpdesig CONTACT_DESIGNATION,
                                       cpdept CONTACT_P_DEPARTMENT,
                                       cpemail CONTACT_P_EMAIL_ID,cpaddr CONTACT_P_ADDRESS, bnk BANK_NAME,
                                    acctno BANK_ACCOUNT_NO, acctype BANK_ACCOUNT_TYPE,curtype BANK_CURRENCY_TYPE, micrno BANK_MICR_NO,swftcd BANK_SWIFT_CODE, ifsc BANK_IFSC_CODE,bnkaddr BANK_ADDRESS, MT_DT MEETING_DATE_TIME,occ_type OCCUPATION, PR_TYPE TYPE_OF_PROPERTY,
                                    file_no FILE_NO,ref_Ext_acc REFERRED_BY_EXISTING_ACCOUNT,ref_user REFERRED_BY_USER, refered_by REFERRED_BY,REMARK ,sch_cat CATEGORY_OF_SCHOOL, sch_med MEDIUM,no_std NO_OF_STUDENTS, no_teach NO_OF_TEACHERS,Aff_type AFFILATION_TYPE,
                                    leadsrc LEAD_SOURCE, CIN_Number,prd_type EXISTING_PRODUCT, pay_term as PAYMENT_TERMS,msme_from MSME_VALID_FROM,msme_upto MSME_VALID_UPTO
                                    ,fix_credit FIX_CREDIT_LIMIT,temp_credit TEMP_CREDIT_LIMIT,valid_credit TEMP_CREDIT_LIMIT_VALID_UPTO, msme_cert MSME_CERTIFICATE_RECD_ON ,'' as PERMANENT_COUNTRY_CODE,
                                    '' as BILLING_COUNTRY_CODE,'' ALIAS,'' CLIENT_RATING,'' RELATION_MANAGER,'' SUB_BROKER,'' PREFERRED_ADD,'' LAST_KYC_DATE,'' UIN_NO ,'' MIN_NO ,
                                    '' DP ,'' as DP_NAME,'' as DP_ID,'' BENEFICIARY_AC_NO , '' ANNUAL_INCOME ,'' QUALIFICATION , '' EMPLOYER ,'' CONTACT_ALIAS,'' CREDIT_RATING ,'' credit_days,
                                '' BANK_TR_DAILY_LIMIT,'' BANK_TR_SINGLE_LIMIT,''CP_OTHER_CONTACT_NO,'' CP_OTHER_EMAIL_ID from clients_mst where 1 = 2";

                                // mq = "select '-' as fstr,vch_num as DOC_NO, c_name ORGANISATION_NAME from clients_mst where 1 = 2";

                                DataTable dtmain = sgen.getdata(userCode, mq);
                                DataTable dtdaily = (DataTable)sgen.GetSession(MyGuid, "tmp_client");
                                dtdaily = sgen.seekval_dt(dtdaily, "param2='Y'");
                                var dtdist = dtdaily.AsEnumerable().Select(row => row.Field<string>("id")).Distinct().ToList();
                                string[] param3 = dtdaily.AsEnumerable().Select(r => r.Field<string>("Param3")).ToArray();
                                string[] disp_name = dtdaily.AsEnumerable().Select(r => r.Field<string>("name")).ToArray();
                                if (param3.Length > 0)
                                {
                                    foreach (DataColumn col in dtmain.Columns)
                                    {
                                        string colname = col.ToString();
                                        for (Int32 i = 0; i < param3.Length; i++)
                                        {
                                            string ctrcol = param3[i].ToString();
                                            string dispcol = disp_name[i].ToString();

                                            if (ctrcol.Contains('.'))
                                            {
                                                ctrcol = ctrcol.Replace('.', ' ');
                                            }
                                            if (ctrcol.Contains(' '))
                                            {
                                                ctrcol = ctrcol.TrimEnd().Replace(' ', '_');
                                            }
                                            if (ctrcol.Contains('/'))
                                            {
                                                ctrcol = ctrcol.TrimEnd().Replace(' ', '_');
                                            }

                                            if (dispcol.Contains('.'))
                                            {
                                                dispcol = dispcol.Replace('.', ' ');
                                            }
                                            if (dispcol.Contains(' '))
                                            {
                                                dispcol = dispcol.TrimEnd().Replace(' ', '_');
                                            }
                                            if (dispcol.Contains('/'))
                                            {
                                                dispcol = dispcol.TrimEnd().Replace(' ', '_');
                                            }
                                            if (colname == ctrcol)
                                            {
                                                cond = cond + ',' + ctrcol + " as " + dispcol;
                                            }
                                        }
                                    }
                                }
                            }
                            cond = cond + ",Entry_BY,Entry_Date";

                            cmd = "select  (a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr" +
                                ",a.vch_num as doc_no ,  a.c_name ORGANISATION_NAME,replace(nvl(pr.c_name,'-'),0,'-') as Parent_Account,replace(nvl(w.c_name,'-'),0,'-') as REFERRED_BY_EXISTING_ACCOUNT,(case when a.panno='AAAAA0000A' then '-' else a.panno end) PAN_NUMBER , a.tanno TAN_NUMBER,a.msmeno MSME_NUMBER," +
                                " a.WEBSITE ,nvl(b.master_name,'-') SALES_SERVICE_AREA, a.type_desc SEARCH_TEXT,nvl(c.master_name,'-') ORGANISATION_STATUS" +
                                ",(case when a.ploc='001' then 'Domestic' when a.ploc='002' then 'Overseas' else '-' end) PARTY_LOCATION," +
                                "nvl(d.master_id, '-') TYPE_OF_ACCOUNT, (case when a.contr = 'N' then 'NO' else 'YES' END) as LABOUR_CONTRACTOR," +
                                "(case when a.SEZ = 'N' then 'NO' else 'YES' END) SEZ  ,(case when a.isgstr = 'Y' then 'YES' else 'NO' END)  GST," +
                                "a.c_gstin GSTIN_NUMBER, a.tor TYPE_OF_GST_REGISTRATION, nvl(f.master_name, '-') TYPE_OF_ADDRESS, " +
                                "nvl(cs.country_name, '-') COUNTRY ,cs.country_code as PERMANENT_COUNTRY_CODE ,nvl(cs.state_name, '-') STATE,nvl(cs.city_name, '-') CITY,a.addr ADDRESS," +
                                " a.PINCODE,nvl(g.master_name, '-') BILLING_TYPE_OF_ADDRESS,nvl(ba.country_name, '-')  BILLING_COUNTRY,ba.country_code as BILLING_COUNTRY_CODE,nvl(ba.state_name," +
                                " '-') BILLING_STATE,nvl(ba.city_name, '-')  BILLING_CITY, a.cpaddr2 BILLING_ADDRESS, a.pincode_2 BILLING_PINCODE, " +
                                "ct.cpname cp_first_name, ct.cp_mname CP_MIDDLE_NAME, ct.cp_lname CP_LAST_NAME, ct.cpdesig CONTACT_DESIGNATION, ct.cpdept" +
                                " CONTACT_P_DEPARTMENT, (case when ct.cplandno='0000000000' then '-' else ct.cplandno end) CONTACT_P_MOBILE_NO, (case when ct.cpemail='ab@ab.ab' then '-' else ct.cpemail end) CONTACT_P_EMAIL_ID, (case when ct.cpcont='0000000000' then '-' else ct.cpcont end) CONTACT_P_CONTACT_NO," +
                                " (case when ct.cpaltcont='0000000000' then '-' else ct.cpaltcont end)  CONTACT_P_ALTERNATE_CONTACT_NO, " +
                                "(case when to_char(ct.DOB, '" + sgen.Getsqldateformat() + "')='01/01/1900' then '-' else to_char(ct.DOB, '" + sgen.Getsqldateformat() + "') end) CONTACT_P_DATE_OF_BIRTH, " +
                                "(case when to_char(ct.DOA,'" + sgen.Getsqldateformat() + "')='01/01/1900' then '-' else to_char(ct.DOA, '" + sgen.Getsqldateformat() + "') end) CONTACT_P_DATE_OF_ANNIVERSARY, " +
                                "ct.cpaddr CONTACT_P_ADDRESS, nvl(h.master_name, '-')" +
                                " BANK_NAME,a.acctno BANK_ACCOUNT_NO, nvl(i.master_name, '-') BANK_ACCOUNT_TYPE, nvl(j.master_name, '') BANK_CURRENCY_TYPE" +
                                ",a.micrno BANK_MICR_NO, a.swftcd BANK_SWIFT_CODE, a.ifsc BANK_IFSC_CODE, a.bnkaddr BANK_ADDRESS, to_char(a.MT_DT,'" + sgen.Getsqldatetimeformat() + "') MEETING_DATE_TIME," +
                                " nvl(k.master_name, '-') OCCUPATION ,nvl(l.master_name, '-') TYPE_OF_PROPERTY,a.file_no FILE_NO, a.refered_by REFERRED_BY" +
                                " ,v.first_name || ' ' || replace(v.middle_name, '0', '') || '' || replace(v.last_name, '0', '') as REFERRED_BY_USER, a.REMARK, " +
                                "a.sch_cat CATEGORY_OF_SCHOOL, a.sch_med MEDIUM, a.no_std NO_OF_STUDENTS, a.no_teach NO_OF_TEACHERS, a.Aff_type AFFILATION_TYPE, " +
                                "nvl(m.master_name, '-') LEAD_SOURCE,a.CIN_Number,(case when a.comp_email='ab@ab.ab' then '-' else a.comp_email end) ORG_EMAIL_ID,t.product_name EXISTING_PRODUCT,nvl(n.master_name, '-') PAYMENT_TERMS,ud.First_name ||' ' || Replace(ud.middle_name,'0','')" +
                                " || ' '|| Replace(ud.last_name,'0','') as Entry_BY,to_char( convert_tz( a.ent_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') as Entry_Date" +
                                " ,(case when to_char(a.msme_from,'" + sgen.Getsqldateformat() + "')='01/01/1900' then '-' else to_char(a.msme_from,'" + sgen.Getsqldateformat() + "') end)as  MSME_VALID_FROM" +
                                " ,(case when to_char(a.msme_upto,'" + sgen.Getsqldateformat() + "')='01/01/1900' then '-' else to_char(a.msme_upto,'" + sgen.Getsqldateformat() + "') end)as  MSME_VALID_UPTO" +
                                " ,(case when to_char(a.valid_credit,'" + sgen.Getsqldateformat() + "') ='01/01/1900' then '-' else to_char(a.valid_credit,'" + sgen.Getsqldateformat() + "') end)as  TEMP_CREDIT_LIMIT_VALID_UPTO," +
                                "to_char( a.msme_cert,'" + sgen.Getsqldateformat() + "') as  MSME_CERTIFICATE_RECD_ON,   a.fix_credit FIX_CREDIT_LIMIT ,a.temp_credit TEMP_CREDIT_LIMIT " +
                                ",o.master_name SUB_BROKER,p.master_name CLIENT_RATING,q.master_name QUALIFICATION ,u.first_name || ' ' || replace(u.middle_name, '0', '') || '' || replace(u.last_name, '0', '') as RELATION_MANAGER ," +
                                "a.dp DP, a.pref_add as PREFERRED_ADD,a.alias_name ALIAS ,a.aadhar_no AADHAR_CARD_NO,a.dp_name DP_NAME,a.dp_id DP_ID,a.ben_acc BENEFICIARY_AC_NO" +
                                ",a.uin_no UIN_NO,a.min_no  MIN_NO,s.master_name as ANNUAL_INCOME,a.employer EMPLOYER,a.cp_alias_name as CONTACT_ALIAS " +
                                ",(case when to_char(  a.KYC_Dt,'" + sgen.Getsqldateformat() + "')='01/01/1900' then '-' else to_char(  a.KYC_Dt,'" + sgen.Getsqldateformat() + "') end) as LAST_KYC_DATE,r.master_name as CREDIT_RATING,a.credit_days," +
                                "a.d_limit BANK_TR_DAILY_LIMIT,a.s_limit BANK_TR_SINGLE_LIMIT, a.pubr,a.trans,a.cpcontother CP_OTHER_CONTACT_NO,a.cpemailother CP_OTHER_EMAIL_ID " +
                                "  from clients_mst a left join (select a.vch_num, group_concat(u.master_name) product_name from clients_mst a left join master_setting u on a.prd_type =u.master_id and u.type='PNM' and find_in_set(u.client_unit_id,'" + unitid_mst + "')= 1 where  find_in_set(a.client_unit_id,'" + unitid_mst + "')=1 and a.type in ('" + type + "','DD" + type + "') " + condition + " group by a.vch_num)t on t.vch_num=a.vch_num  " +
                                " left join master_setting b on a.salesarea = b.master_id and b.type = 'SSA' and find_in_set(b.client_unit_id,'" + unitid_mst + "')= 1" +
                                " left join master_setting c on a.ptype = c.master_id and c.type = 'PT1' and find_in_set(c.client_unit_id,'" + unitid_mst + "')= 1 " +
                                " left join master_setting d on a.client_type = d.master_id and d.type = 'CLI' and find_in_set(d.client_unit_id,'" + unitid_mst + "')= 1 " +
                                " left join master_setting f on a.addtype1 = f.master_id and f.type = 'TOA' and find_in_set(f.client_unit_id,'" + unitid_mst + "')= 1 " +
                                " left join master_setting g on a.addtype2 = g.master_id and g.type = 'TOA' and find_in_set(g.client_unit_id,'" + unitid_mst + "')= 1 " +
                                " left join master_setting h on a.bnk = h.master_id and h.type = 'ABD' and find_in_set(h.client_unit_id,'" + unitid_mst + "')= 1 " +
                                " left join master_setting i on a.acctype = i.master_id and i.type = 'BTM' and find_in_set(i.client_unit_id,'" + unitid_mst + "')= 1 " +
                                " left join master_setting j on a.curtype = j.master_id and j.type = 'CTM' and find_in_set(j.client_unit_id,'" + unitid_mst + "')= 1 " +
                                " left join master_setting k on a.occ_type = k.master_id and k.type = 'OCC' and find_in_set(k.client_unit_id,'" + unitid_mst + "')= 1 " +
                                " left join master_setting l on a.PR_TYPE = l.master_id and l.type = 'PRT' and find_in_set(l.client_unit_id,'" + unitid_mst + "')= 1 " +
                                " left join master_setting m on a.leadsrc = m.master_id and m.type = 'SRC' and find_in_set(m.client_unit_id,'" + unitid_mst + "')= 1 " +
                                 " left join master_setting n on a.pay_term = n.master_id and n.type = 'PTM' and find_in_set(n.client_unit_id,'" + unitid_mst + "')= 1 " +
                                 " left join master_setting o on a.sub_broker = o.master_id and o.type = 'SBM' and find_in_set(o.client_unit_id,'" + unitid_mst + "')= 1 " +
                                 " left join master_setting p on a.client_rating = p.master_id and p.type = 'CRM' and find_in_set(p.client_unit_id,'" + unitid_mst + "')= 1 " +
                                 " left join master_setting s on a.ann_inc = s.master_id and s.type = 'KAI' and find_in_set(s.client_unit_id,'" + unitid_mst + "')= 1 " +
                                 " left join master_setting r on a.credit_rating = r.master_id and r.type = 'CRD' and find_in_set(r.client_unit_id,'" + unitid_mst + "')= 1 " +
                                 " left join master_setting q on a.qualification = q.master_id and q.type = 'EQU' " +
                                " left join country_state cs on cs.rec_id = a.city left join country_state ba on ba.rec_id = a.city2 left join clients_mst pr" +
                                "  on a.parent_id=pr.vch_num and pr.type='BCD' and find_in_set(pr.client_unit_id,'" + unitid_mst + "')= 1 left join clients_mst w" +
                                "  on a.ref_Ext_acc=w.vch_num and w.type='BCD' and find_in_set(w.client_unit_id,'" + unitid_mst + "')= 1 " +
                                //" inner join user_details ud on  a.ent_by =ud.rec_id left join clients_mst ct  on a.vch_num=ct.ref_code  and " +
                                "inner join user_details ud on  a.ent_by =ud.vch_num " +
                                "left join clients_mst ct  on a.vch_num=ct.ref_code  and find_in_set(ct.client_unit_id,'" + unitid_mst + "')= 1 and substr(ct.ref_code,0,3)=" + prefix + " " +
                                //"left join user_details u on u.rec_id = a.rel_mgr" +
                                "left join user_details u on u.vch_num = a.rel_mgr" +
                                //"left join  user_details v on v.rec_id = a.ref_user where find_in_set(a.client_unit_id,'" + unitid_mst + "')=1 and a.type in ('" + type + "','DD" + type + "') " + condition + " order by a.vch_num desc";
                                " left join  user_details v on v.vch_num = a.ref_user where find_in_set(a.client_unit_id,'" + unitid_mst + "')=1 and a.type in ('" + type + "','DD" + type + "') " + condition + " order by a.vch_num desc";



                            cmd = "select  " + cond + " from (" + cmd + ")a";
                            break;
                        case "47002":
                            cmd = "select distinct (p.client_id||p.client_unit_id||p.vch_num||to_char(p.vch_date,'yyyymmdd')||p.type) as fstr,p.vch_num po_number,to_char(p.vch_date, '" + sgen.Getsqldateformat() + "') po_date,c.c_name as Party_Name," +
                              "c.c_gstin as Party_Gstin,c.addr as Party_Address, p.icode ItemCode, p.iname ItemName, p.cpartno PartNo, p.uom UOM, p.hsno HSN, p.indno as po_Mode, p.qtyord Order_Qty, p.irate ItemRate, p.taxrate TaxRate, iamount Item_Amt,p.taxamt taxamt, p.disctype Discount_Type" +
                              ", p.discrate Discount_Rate, p.discamt Discount_Amt, p.lineamount LineAmt,to_char(p.dlv_date, '" + sgen.Getsqldateformat() + "') Delivery_Date" +
                              ",p.gserchrg IGST, p.gloadchrg SGST, p.gamc CGST, p.basicamt BasicAmt, p.ginstlchrg GrossAmt, p.gtaxamt as Net_Amount,nvl(sf.c_name, '-') as From_Name,nvl(sf.c_gstin, '-') as From_Gstin,nvl(sf.addr, '-') FromAddress," +
                              "nvl(st.c_name, '-') as To_Name,nvl(st.c_gstin, '-') as To_Gstin,nvl(st.addr, '-') ToAddress from purchasesc p " +
                              "inner join clients_mst c on p.acode = c.vch_num and find_in_set(c.client_unit_id, p.client_unit_id)=1 and c.type = 'BCD' and substr(c.vch_num,0,3)='303' " +
                              "left join clients_mst sf on p.shipfrom = sf.vch_num and find_in_set(sf.client_unit_id, p.client_unit_id)=1 and sf.type = 'BCD' " +
                              "left join clients_mst st on p.shipto = st.vch_num and find_in_set(st.client_unit_id, p.client_unit_id)=1 and st.type = 'BCD' " +
                              "and substr(st.vch_num,0,3)='303' where p.client_unit_id = '" + unitid_mst + "' and p.type='BDP' and p.pur_type='P' and p.ent_by='" + userid_mst + "' order by p.vch_num desc";

                            break;
                        case "80010":
                            cmd = "select ug.client_id||ug.client_unit_id||ug.vch_num||to_char(ug.vch_date,'yyyymmdd')||ug.type as fstr,ug.vch_num as doc_no," +
                             "to_char(ug.vch_date, 'dd/MM/YYYY') as doc_date,ug.col5 as asset_serial,ug.col6 as asset_name,ug.col8 as expected_value" +
                             ",to_char(ug.date1, 'dd/MM/YYYY') as expected_return_date,ug.col3 as Remark" +
                             ",(emp.FIRST_NAME || ' ' || REPLACE(emp.MIDDLE_NAME, '0', '') || ' ' || REPLACE(emp.LAST_NAME, '0', '')) as Employee_Name," +
                             "emp.emp_code as Employee_Code,nvl(dg.master_name, '-') as designation,nvl(dp.master_name, '-') as department from enx_tab2 ug inner join emp_master emp" +
                             " on emp.emp_code = ug.col2 and emp.client_unit_id = ug.client_unit_id and emp.type = 'EMP' left join master_setting dp on dp.master_id " +
                             "= emp.emp_dept and dp.type = 'MDP' and find_in_set(dp.client_unit_id,'" + unitid_mst + "')= 1 left join master_setting dg on dg.master_id = " +
                             "emp.emp_desig and dg.type = 'MDG' and find_in_set(dg.client_unit_id, '" + unitid_mst + "') = 1 " +
                             "inner join user_details ud on ud.emp_id = emp.vch_num and ud.type='CPR' and ud.client_unit_id=emp.client_unit_id where ug.client_unit_id = '" + unitid_mst + "' and ug.type = 'AAE'" +
                             " and to_date(to_char(ug.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and" +
                             " to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "') and ud.vch_num='" + userid_mst + "'";

                            break;
                        case "40002.7":



                            cmd = "select (a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr,b.c_name as Client_Name," +
                                        "a.c_name as UnitName,a.pincode as Pincode,a.isgstr IsGst,a.c_gstin as GSTIN,a.type_desc as Search_text," +
                                        "a.cpname as PerName,a.cpcont as PerContact,a.cpaltcont as PerAltContact,a.cpemail as PerEmail,a.cpdesig " +
                                        "as PerDesig from clients_mst a inner join clients_mst b on a.panno=b.vch_num and b.type= a.occ_type" +
                                        " and a.client_unit_id=b.client_unit_id where a.type in ('UNT','UNTDD') and a.client_id='" +
                                        clientid_mst + "' and a.client_unit_id='" + unitid_mst + "'  order by a.panno,a.vch_num desc";
                            break;

                        case "40008.1":
                            //cmd = "select (a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr," +
                            //               " a.vch_num as lead_no,to_char(convert_tz(a.vch_date, 'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') as lead_date" +
                            //               ",a.cust_name as Customer_Name, nvl(a.refered_by,'-') refered_by ,c.master_name as Lead_source,d.master_name as lead_status," +
                            //               " ud.first_name || ' ' || replace(ud.middle_name, '0', '') || '' || replace(ud.last_name, '0', '') as lead_owner" +
                            //               ",a.ld_owner,a.ot_source,a.ct_person,a.remark,a.mobile_no from " +
                            //               " lead_master a  left join master_setting c on a.ld_source = c.master_id and c.type = 'SRC'" +
                            //               " and find_in_set(c.client_unit_id, a.client_unit_id)= 1 left join master_setting d on a.ld_status = d.master_id and d.type = 'LST' " +
                            //               "and find_in_set(d.client_unit_id, a.client_unit_id)= 1 left join user_details ud on ud.rec_id = a.ld_owner where a.type = 'LED' and " +
                            //               "a.client_unit_id = '" + unitid_mst + "' and a.lead_con='Y' order by a.vch_num desc";
                            break;

                        case "40008.3":
                            cmd = "select (a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr," +
                                           " a.vch_num as lead_no,to_char(a.vch_date,'" + sgen.Getsqldateformat() + "') as lead_date" +
                                           ",a.cust_name as Customer_Name, nvl(a.refered_by,'-') refered_by ,c.master_name as Lead_source,d.master_name as lead_status," +
                                           " ud.first_name || ' ' || replace(ud.middle_name, '0', '') || '' || replace(ud.last_name, '0', '') as lead_owner" +
                                           ",a.ld_owner,a.ot_source,a.ct_person,a.remark,a.mobile_no from " +
                                           " lead_master a  left join master_setting c on a.ld_source = c.master_id and c.type = 'SRC'" +
                                           " and find_in_set(c.client_unit_id, a.client_unit_id)= 1 left join master_setting d on a.ld_status = d.master_id and d.type = 'LST' " +
                                           //"and find_in_set(d.client_unit_id, a.client_unit_id)= 1 left join user_details ud on ud.rec_id = a.ld_owner where a.type = 'LED' and " +
                                           "and find_in_set(d.client_unit_id, a.client_unit_id)= 1 left join user_details ud on ud.vch_num = a.ld_owner where a.type = 'LED' and " +
                                           "a.client_unit_id = '" + unitid_mst + "' and a.lead_con='Y' order by a.vch_num desc";
                            break;

                            //case "40005.3":
                            //    cmd = "select a.client_id||a.client_unit_id||a.type|| a.ent_by, count(a.vch_num) NO_OF_LEADS,a.ent_by as entry_by_code,u.first_name || ' ' || replace(u.middle_name, '0', '') || '' ||" +
                            //        " replace(u.last_name, '0', '') as Entry_BY_Name from lead_master a inner join user_details u on u.rec_id = a.ent_by" +
                            //        " where a.type = 'LED' and a.client_unit_id = '"+unitid_mst+"' group by a.ent_by,u.first_name || ' ' || replace(u.middle_name, '0', '')" +
                            //        " || '' || replace(u.last_name, '0', ''),a.client_id||a.client_unit_id||a.type|| a.ent_by ";
                            //    break;

                            //case "LEADDETAIL":
                            //    String URL = Session["SSEEKVAL"].ToString();
                            //    cmd = "select (a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr," +
                            //       " a.vch_num as lead_no,to_char(convert_tz(a.vch_date, 'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') as lead_date" +
                            //       ",a.cust_name as Customer_Name, nvl(a.refered_by,'-') refered_by ,c.master_name as Lead_source,d.master_name as lead_status," +
                            //       " ud.first_name || ' ' || replace(ud.middle_name, '0', '') || '' || replace(ud.last_name, '0', '') as lead_owner," +
                            //       "u.first_name || ' ' || replace(u.middle_name, '0', '') || '' || replace(u.last_name, '0', '') as Entry_By," +
                            //       "to_char(convert_tz(u.ent_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Entry_Date from " +
                            //       " lead_master a  left join master_setting c on a.ld_source = c.master_id and c.type = 'SRC'" +
                            //       " and find_in_set(c.client_unit_id, a.client_unit_id)= 1 left join master_setting d on a.ld_status = d.master_id and d.type = 'LST' " +
                            //       "and find_in_set(d.client_unit_id, a.client_unit_id)= 1 left join user_details ud on ud.rec_id = a.ld_owner " +
                            //       "inner join  user_details u on u.rec_id = a.ent_by where a.type = 'LED' and " +
                            //       "a.client_unit_id = '" + unitid_mst + "' and  a.client_id||a.client_unit_id||a.type|| a.ent_by='" + URL + "'";
                            //    break;

                    }
                    break;
                #endregion

                #region INVOICE

                case "invoice_config":

                    switch (btnval)
                    {
                        case "EDIT":
                            cmd = "select * from menus";
                            break;

                    }
                    break;

                #endregion                              

                #region CRM Reports
                case "crm_reports":
                    switch (btnval)
                    {
                        case "11":
                            cmd = @"SELECT concat(vm.client_id, vm.client_unit_id, vm.vch_num, to_char(vm.vch_date, 'yyyymmdd'), vm.type) as fstr
,ud.first_name as Ent_by,to_char(convert_tz(vm.ent_date, '+00:00', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Ent_date , vm.col1 as Client_Type,vm.col39 as Institution_Name," +
"vm.col3 as Affiliation_No,vm.col25 as Postal_Address,vm.col9 as Pincode,vm.col10 as Std_Code,vm.col11 as Office_No,vm.col7 as Resi_No" +
",vm.Col18 As Fax_No,vm.col38 as Email,vm.col40 as Website,vm.col6 as Admin_Exp,vm.col17 as Teaching_Exp," +
"to_char(convert_tz(vm.date1, '+00:00', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') " +
"as Affiliation_From,to_char(convert_tz(vm.date2, '+00:00', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Affiliation_To" +
",vm.col20 as School_Category,vm.col21 as Medium,vm.col19 as Type_oF_School," +
"vm.col28 as Year_of_foundation,to_char(convert_tz(nvl( vm.date3,UTC_TIMESTAMP()), '+00:00', '+05:30'), '%d/%b/%Y') as Year_Of_Opening," +
"vm.col29 as Principal_Name,vm.col30 as Sex,vm.col31 AS " +
"principal_Education,vm.col13 as Status_Of_School,col33 as Affiliation_Type,col34 as Name_of_Trust,nvl(cs.alpha_2, '-') AS Alpha, " +
"nvl(cs.state_name, '-') AS State_name," +
"nvl(cs.district_name, '-') AS District_Name from vehicle_master vm " +
"left outer JOIN country_state cs on cs.rec_id = (select rec_id AS rec_id from country_state WHERE district_id = vm.cur_district AND state_gst_code = vm.cur_state  LIMIT 1) " +
//" left outer join user_details ud on vm.ent_by=lpad(ud.rec_id,6,'0') where vm.type = 'ESD' and vm.client_unit_id = '" + unitid_mst + "' " +
" left outer join user_details ud on vm.ent_by=ud.vch_num where vm.type = 'ESD' and vm.client_unit_id = '" + unitid_mst + "' " +
"order by vm.ent_date desc ";
                            break;

                        case "12":
                            cmd = @"SELECT concat(vm.client_id, vm.client_unit_id, vm.vch_num, to_char(vm.vch_date, 'yyyymmdd'), vm.type) as fstr
,ud.first_name as Ent_by,to_char(convert_tz(vm.ent_date, '+00:00', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Ent_date , vm.col1 as Client_Type,vm.col39 as Institution_Name," +
"vm.col3 as Affiliation_No,vm.col25 as Postal_Address,vm.col9 as Pincode,vm.col10 as Std_Code,vm.col11 as Office_No,vm.col7 as Resi_No" +
",vm.Col18 As Fax_No,vm.col38 as Email,vm.col40 as Website,vm.col6 as Admin_Exp,vm.col17 as Teaching_Exp," +
"to_char(convert_tz(vm.date1, '+00:00', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') " +
"as Affiliation_From,to_char(convert_tz(vm.date2, '+00:00', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Affiliation_To" +
",vm.col20 as School_Category,vm.col21 as Medium,vm.col19 as Type_oF_School," +
"vm.col28 as Year_of_foundation,to_char(convert_tz(nvl( vm.date3,UTC_TIMESTAMP()), '+00:00', '+05:30'), '%d/%b/%Y') as Year_Of_Opening," +
"vm.col29 as Principal_Name,vm.col30 as Sex,vm.col31 AS " +
"principal_Education,vm.col13 as Status_Of_School,col33 as Affiliation_Type,col34 as Name_of_Trust,nvl(cs.alpha_2, '-') AS Alpha, " +
"nvl(cs.state_name, '-') AS State_name," +
"nvl(cs.district_name, '-') AS District_Name from vehicle_master vm " +
"left outer JOIN country_state cs on cs.rec_id = (select rec_id AS rec_id from country_state WHERE district_id = vm.cur_district AND state_gst_code = vm.cur_state  LIMIT 1) " +
//" left outer join user_details ud on vm.ent_by=lpad(ud.rec_id,6,'0') where vm.type = 'SFB' and vm.client_unit_id = '" + unitid_mst + "' " +
" left outer join user_details ud on vm.ent_by=ud.vch_num where vm.type = 'SFB' and vm.client_unit_id = '" + unitid_mst + "' " +
"order by vm.ent_date desc ";
                            break;

                    }

                    break;
                #endregion

                #region ctrl

                case "ctrls":

                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = "select concat(client_id,client_unit_id,vch_num,to_char(date1,'yyyymmdd'),type) as  fstr," +
                                " to_char(convert_tz(date1, '+00:00', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Vch_date,to_char(convert_tz(date2, '+00:00', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as From_Date,to_char(convert_tz(date3, '+00:00', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as To_date,col24 as Name,col1,col2,col3,col4 from vehicle_master where  client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "' and type='CTR'";
                            break;
                    }

                    break;

                #endregion

                #region   create_user
                case "create_user":

                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "DETAIL":
                        case "INACTIVE":

                            cmd = "SELECT ud.vch_num||to_char(ud.vch_date, 'yyyymmdd')|| ud.type as fstr,ud.user_id,ud.first_name,ud.LAST_NAME,GROUP_CONCAT(DISTINCT cp.Company_Name) company_name," +
                                "GROUP_CONCAT(DISTINCT cu.Unit_Name) as unit_name,ud.email,nvl(msdg.master_name, '-') designation,nvl(msdp.master_name, '-') department,ud.status FROM user_details ud " +
                                "left join master_setting msdp on ud.DEPARTMENT = msdp.master_id and msdp.type = 'MDP' and find_in_set(msdp.client_unit_id, ud.client_unit_id)= 1 " +
                                "left join master_setting msdg on ud.designation = msdg.master_id and msdg.type = 'MDG' and find_in_set(msdg.client_unit_id, ud.client_unit_id)= 1 " +
                                "left join company_profile cp on find_in_set(cp.Company_Profile_Id, ud.client_id) = 1 and cp.type = 'CP' " +
                                "left join company_unit_profile cu on find_in_set(cu.cup_id, ud.client_unit_id) = 1 " +
                                "where ud.Ent_By = '" + userid_mst + "' and ud.type = 'CPR' " +
                                "group by ud.vch_num || to_char(ud.vch_date, 'yyyymmdd') || ud.type ,ud.vch_num,ud.user_id,ud.first_name,ud.LAST_NAME,ud.email, " +
                                "msdg.master_name,msdp.master_name,ud.status order by ud.vch_num desc";
                            break;

                        case "EMP":
                            cmd = "select emp.client_id||emp.client_unit_id||emp.vch_num||To_char(emp.vch_date,'yyyymmdd') fstr,emp.type," +
      "emp.FIRST_NAME|| ' '|| REPLACE(emp.MIDDLE_NAME,'0','')|| ' '||REPLACE(emp.LAST_NAME,'0','') as Employee_Name," +
      "emp.gender as Gender, emp.f_name as Father_Name,emp.contact_no as Contact_No,emp.email_id as Email," +
      "TO_CHAR(emp.st_dt,'" + sgen.Getsqldateformat() + "') as Date_Of_Joining, emp.emp_code as Emp_Code from emp_master emp inner join add_academic_year ad " +
      "on ad.type='ACY' and emp.client_id=ad.client_id " +
      "WHERE emp.client_id= '" + clientid_mst + "' and emp.type= 'EMP' and emp.emp_status='Y' order by emp.rec_id";
                            break;

                        case "VND":
                            cmd = "select distinct (a.vch_num|| to_char(a.vch_date,'yyyymmdd')||a.type) fstr,a.vch_num vendor_code,a.c_name vendor_name," +
                                  "a.addr, a.c_gstin as gstin from clients_mst a " +
                                  "where substr(a.vch_num,0,3)='203' and find_in_set(a.client_unit_id,'" + unitid_mst + "')=1 and a.type='BCD'";
                            break;

                        case "CST":
                            cmd = "select distinct (a.vch_num|| to_char(a.vch_date,'yyyymmdd')||a.type) fstr,a.vch_num vendor_code,a.c_name vendor_name," +
                                  "a.addr, a.c_gstin as gstin from clients_mst a " +
                                  "where substr(a.vch_num,0,3)='303' and find_in_set(a.client_unit_id,'" + unitid_mst + "')=1 and a.type='BCD'";
                            break;
                    }

                    break;
                #endregion

                #region mytodo

                case "mytodo":

                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = "select (client_id||client_unit_id||vch_num||to_char(vch_date,'yyyymmdd')||type) as fstr,vch_num DocNo,title Title," +
                                "to_char(convert_tz(fdt, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldatetimeformat() + "') From_Dt," +
                                "to_char(convert_tz(tdt, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldatetimeformat() + "') To_Dt," +
                                "to_char(convert_tz(pdt, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldatetimeformat() + "') as Posted_Dt,pr High_Priority " +
                                "from notifications where type='TDL' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "' and ent_by='" + userid_mst + "'";
                            break;
                    }

                    break;

                #endregion

                #region createnote

                case "createnote":

                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = "select (client_id||client_unit_id||vch_num||to_char(vch_date,'yyyymmdd')||type) as fstr,vch_num DocNo,title Title," +
                                "to_char(convert_tz(fdt, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldatetimeformat() + "') From_Dt," +
                                "to_char(convert_tz(tdt, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldatetimeformat() + "') To_Dt," +
                                "to_char(convert_tz(pdt, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldatetimeformat() + "') as Posted_Dt,Status " +
                                "from notifications where type='NOT' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
                            break;
                    }

                    break;

                #endregion

                #region urights
                case "urights":

                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":

                            cmd = "select distinct (vch_num||modid||type||m_module3) fstr,vch_num DocNo," +
                                "to_char(vch_date,'" + sgen.Getsqldateformat() + "') Doc_Date,m_module3,role,modid from urights " +
                                "where type ='K99' and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
                            break;
                    }

                    break;
                #endregion

                #region mailconfig
                case "mailconfig":
                    switch (btnval.ToUpper())
                    {
                        case "VIEW":
                            cmd = "select (client_unit_id||type) fstr,vch_num DocNo,to_char(vch_date,'" + sgen.Getsqldateformat() + "') Doc_Date," +
                                "module,pgmail,userid,extmail,status from mail_config " +
                                "where type ='MLC' and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
                            break;
                        case "USER":
                            cmd = "select (vch_num||type) fstr,vch_num id,(user_id||' ('||nvl(email,'-')||')') userid,(first_name||replace(middle_name,'0','')||last_name) name " +
                                "FROM user_details WHERE type='CPR'";
                            sgen.SetSession(Myguid, "mc_uids", param1);
                            sgen.SetSession(Myguid, "mc_rval", param2);
                            break;
                        case "TEMP":
                            cmd = "select (client_unit_id||vch_num||type) fstr,vch_num id,col2 name from enx_tab2 WHERE type='MTP' and client_unit_id='" + unitid_mst + "' and col3='" + param1.Trim() + "'";
                            sgen.SetSession(Myguid, "tm_rval", param2);
                            break;
                        case "TEMPPH":
                            cmd = "select (client_unit_id||vch_num||type) fstr,vch_num id,col2 name from enx_tab2 WHERE type='PTP' and client_unit_id='" + unitid_mst + "' and col3='" + param1.Trim() + "'";
                            sgen.SetSession(Myguid, "tm_rph", param2);
                            break;
                    }

                    break;
                #endregion

                #region urights_btn
                case "urights_btn":

                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":

                            cmd = "select distinct (a.type||a.userid) fstr,a.vch_num DocNo,to_char(a.vch_date,'" + sgen.Getsqldateformat() + "') Doc_Date,(b.user_id||' ('||a.userid||')') userid " +
                                //"from urights a inner join user_details b on b.rec_id = to_number(a.userid) " +
                                "from urights a inner join user_details b on b.vch_num = a.userid " +
                                "where a.type = 'K98' and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";

                            break;

                        case "USER":
                            cmd = "select distinct (r.type||r.userid) fstr,(u.user_id||' ('||r.userid||')') ud from urights r " +
                                  //"inner join user_details u on lpad(u.rec_id,6,0)= r.userid and u.type = 'CPR' " +
                                  "inner join user_details u on u.vch_num= r.userid and u.type = 'CPR' " +
                                  "where r.type = 'K98' and r.client_id = '" + clientid_mst + "' and r.client_unit_id = '" + unitid_mst + "'";
                            break;
                    }

                    break;
                #endregion

                #region db_set
                case "db_set":

                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "DELETE":
                        case "DETAIL":
                            cmd = "select ds.client_id||ds.client_unit_id||ds.vch_num||TO_CHAR(ds.vch_date,'YYYYMMDD')||ds.type as fstr,ds.vch_num as db_id,ds.name as Name,m.m_name as Module," +
                                "ds.onmodule as OnModule,ds.drill_level as Drill_Level,ds.chart_type as Chart_Type from DASHBOARD_SETTING ds INNER JOIN menus m on " +
                                "ds.module=m.m_id where ds.type='DBA' and ds.client_id='" + clientid_mst + "' and ds.client_unit_id='" + unitid_mst + "'";
                            //mq = "select concat(ds.client_id,ds.client_unit_id,ds.vch_num,date_format(ds.vch_date,'%Y%m%d'),ds.type) as fstr,ds.name as Name,m.m_name as Module,ds.onmodule as OnModule,ds.drill_level as 'Drill_Level',ds.chart_type as 'Chart_Type',status as Status, user_id as Username  from dashboard_setting ds join menus m on ds.module=m.m_id where ds.type='" + type3 + "' and ds.client_id='" + clientid_mst + "' and ds.client_unit_id='" + unitid_mst + "'";
                            break;
                        case "CHARTS":

                            cmd = "select ds.client_id||ds.client_unit_id||ds.vch_num||TO_CHAR(ds.vch_date,'YYYYMMDD')||ds.type as fstr,ds.name as Name,m.m_name as Module," +
                                "ds.onmodule as OnModule,ds.drill_level as Drill_Level,ds.chart_type as Chart_Type from DASHBOARD_SETTING ds INNER join menus m on " +
                                "ds.module=m.m_id where ds.type='DBA' and ds.client_id='" + clientid_mst + "' and ds.client_unit_id='" + unitid_mst + "' " +
                                "and chart_no not in (select distinct chart_no from DASHBOARD_SETTING where type='DBU' and user_id='" + userid_mst + "')";
                            //mq = "select concat(ds.client_id,ds.client_unit_id,ds.vch_num,date_format(ds.vch_date,'%Y%m%d'),ds.type) as fstr,ds.name as Name,m.m_name as Module,ds.onmodule as OnModule,ds.drill_level as 'Drill_Level',ds.chart_type as 'Chart_Type',status as Status, user_id as Username  from dashboard_setting ds join menus m on ds.module=m.m_id where ds.type='" + type3 + "' and ds.client_id='" + clientid_mst + "' and ds.client_unit_id='" + unitid_mst + "'";
                            break;
                    }

                    break;
                #endregion

                #region client_profile
                case "client_profile":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                            if (role_mst.ToString().Trim().ToUpper() == "OWNER")
                            {
                                cmd = "select (cp.company_profile_id||to_char(cp.vch_date, 'yyyymmdd')|| cp.type) as fstr,cp.client_type org_type,cp.company_profile_id org_id,cp.Company_Name org_Name,replace(cp.Company_Address, '$', ' ') Address," +
                                    "cp.Company_Website as Website,cp.Company_Email_Id as Email,cp.company_contact_no ContNo,cp.company_alternate_contact_no AltContNo, cp.Company_No_Of_Employee as No_of_Emp " +
                                    "from company_profile cp where cp.type = 'CP'";
                            }
                            else
                            {
                                cmd = "select (cp.company_profile_id||to_char(cp.vch_date, 'yyyymmdd')|| cp.type) as fstr,cp.client_type org_type,cp.company_profile_id org_id,cp.Company_Name org_Name,replace(cp.Company_Address, '$', ' ') Address," +
                                   "cp.Company_Website as Website,cp.Company_Email_Id as Email,cp.company_contact_no ContNo,cp.company_alternate_contact_no AltContNo, cp.Company_No_Of_Employee as No_of_Emp " +
                                   "from company_profile cp where cp.type = 'CP' and cp.company_ent_by='" + userid_mst + "'";
                            }
                            break;
                        case "CITY":
                            cmd = "SELECT (client_id||client_unit_id||vch_num||to_char(vch_date,'yyyymmdd')||type||rec_id) as fstr,city_name,state_name,country_name,std_code," +
                                "region FROM country_state";
                            break;
                    }
                    break;
                #endregion

                #region unit_profile
                case "unit_profile":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                            if (role_mst.ToString().Trim().ToUpper() == "OWNER")
                            {
                                //    cmd = "SELECT (cup.cup_id||to_char(cup.vch_date, 'yyyymmdd')) as fstr,cup.cup_id UnitCode,cp.Company_Name as Company_Name,cup.Unit_Name as Unit_Name,REPLACE(cup.unit_address, '$', ' ') as Unit_Address," +
                                //"cup.unit_email as Email,cup.unit_contact_no ContNo, cup.unit_alternate_contact_no AltContNo,cup.unit_website as Website," +
                                //"nvl(to_char(convert_tz(dos, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "'), '') as Start_date,nvl(to_char(convert_tz(doc, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "'), '') as Cease_date " +
                                //"FROM company_unit_profile cup INNER JOIN Company_Profile cp on cp.Company_Profile_Id = cup.Company_Profile_Id order by cup.rec_id desc";

                                cmd = "SELECT (cup.cup_id||to_char(cup.vch_date, 'yyyymmdd')) as fstr,cp.company_profile_id org_id,cup.cup_id Unit_id,cup.Unit_Name as Unit_Name," +
                                    "cp.Company_Name as Org_Name,REPLACE(cup.unit_address, '$', ' ') as Unit_Address,cup.unit_email as Email," +
                                    "cup.unit_contact_no ContNo, cup.unit_alternate_contact_no AltContNo, cup.unit_website as Website," +
                                    "nvl(cup.septr, '-') as seprator,nvl(cr.master_name, '-') as currency,cup.Unit_Contact_Person_Name CP_name," +
                                    " cup.Unit_Person_Email_Id as CP_Email,cup.Unit_Person_Designation as CP_Desig," +
                                    "cup.Unit_Person_Contact_No as CP_Mob,nvl(to_char(dos, '" + sgen.Getsqldateformat() + "'), '') as Start_date,nvl(to_char(doc, '" + sgen.Getsqldateformat() + "'), '') as Cease_date," +
                                    "cup.bank,cup.branch,cup.acctno Account_no,cup.ifsc,(case when cup.unit_status = '0' then 'Active' when cup.unit_status = '1' then 'InActive' end) as status FROM company_unit_profile cup INNER JOIN Company_Profile cp " +
                                    "on cp.Company_Profile_Id = cup.Company_Profile_Id left join master_setting cr on cr.master_id = cup.ll_act and cr.type = 'CTM' order by cup.rec_id desc";
                            }
                            else
                            {
                                //                            cmd = "SELECT (cup.cup_id||to_char(cup.vch_date, 'yyyymmdd')) as fstr,cup.cup_id UnitCode,cp.Company_Name as Company_Name,cup.Unit_Name as Unit_Name,REPLACE(cup.unit_address, '$', ' ') as Unit_Address," +
                                //"cup.unit_email as Email,cup.unit_contact_no ContNo, cup.unit_alternate_contact_no AltContNo,cup.unit_website as Website," +
                                //"nvl(to_char(convert_tz(dos, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "'), '') as Start_date,nvl(to_char(convert_tz(doc, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "'), '') as Cease_date " +
                                //"FROM " + tab_name + " cup INNER JOIN Company_Profile cp on cp.Company_Profile_Id = cup.Company_Profile_Id where cup.unit_ent_by='" + userid_mst + "' order by cup.rec_id desc";

                                cmd = "SELECT (cup.cup_id||to_char(cup.vch_date, 'yyyymmdd')) as fstr,cp.company_profile_id org_id,cup.cup_id Unit_id,cup.Unit_Name as Unit_Name," +
                           "cp.Company_Name as Org_Name,REPLACE(cup.unit_address, '$', ' ') as Unit_Address,cup.unit_email as Email," +
                           "cup.unit_contact_no ContNo, cup.unit_alternate_contact_no AltContNo, cup.unit_website as Website," +
                           "nvl(cup.septr, '-') as seprator,nvl(cr.master_name, '-') as currency,cup.Unit_Contact_Person_Name CP_name," +
                           " cup.Unit_Person_Email_Id as CP_Email,cup.Unit_Person_Designation as CP_Desig," +
                           "cup.Unit_Person_Contact_No as CP_Mob,nvl(to_char(dos, , '" + sgen.Getsqldateformat() + "'), '') as Start_date,nvl(to_char(doc, '" + sgen.Getsqldateformat() + "'), '') as Cease_date," +
                           "cup.bank,cup.branch,cup.acctno Account_no,cup.ifsc,(case when cup.unit_status = '0' then 'Active' when cup.unit_status = '1' then 'InActive' end) as status FROM  " + tab_name + " cup INNER JOIN Company_Profile cp " +
                           "on cp.Company_Profile_Id = cup.Company_Profile_Id left join master_setting cr on cr.master_id = cup.ll_act" +
                           " and cr.type = 'CTM' where cup.unit_ent_by='" + userid_mst + "' order by cup.rec_id desc";

                            }
                            break;

                    }
                    break;
                #endregion

                #region v_schedule
                case "v_schedule":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "DETAIL":
                            cmd = "select distinct a.client_id||a.client_unit_id||a.vch_num||TO_CHAR(a.vch_date,'YYYYMMDD')||a.type as fstr,a.vch_num as meeting_id,a.col13 as title,to_char(a.date1, '" + sgen.Getsqldatetimeformat() + "') as to_date from enx_tab2 a" +
                                " where a.client_unit_id = '" + unitid_mst + "' and a.type = 'SDL'";
                            break;

                    }
                    break;
                #endregion

                #region Activations
                case "Activations":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = "SELECT (vch_num||to_char(vch_date, 'yyyymmdd')||type) as fstr,vch_num DocNo,to_char(vch_date,'dd/MM/yyyy') DocDate," +
                                "compcode CompanyCode,compname CompanyName,hddno NoOfLIC,keycode from activations where type='LIC'";
                            break;
                    }
                    break;
                    #endregion
            }
            sgen.open_grid(title, cmd, sgen.Make_int(seektype));
            //Session["parentname"] = "MVC";
            ViewBag.pageurl = sgen.GetSession(MyGuid, "pageurl").ToString();
            //return Content(cmd);
        }
        #endregion

        #region
        public JsonResult GetDataJson(string mq)
        {

            Dictionary<string, string> lstCategories = new Dictionary<string, string>();
            DataTable dt = sgen.getdata(userCode, EncryptDecrypt.Decrypt_new(mq));
            foreach (DataRow dr in dt.Rows)
            {
                lstCategories.Add(dr[0].ToString(), dr[1].ToString());
            }
            //lstCategories.Add("2", "Condiments");
            //lstCategories.Add("3", "Confections");
            //lstCategories.Add("4", "Dairy Products");
            //lstCategories.Add("5", "Grains/Cereals");
            //lstCategories.Add("6", "Meat/Poultry");
            //lstCategories.Add("7", "Produce");
            //lstCategories.Add("8", "Seafood");
            return Json(lstCategories, JsonRequestBehavior.AllowGet);
        }

        public ContentResult Directcall(string Myguid)
        {


            string url = "";
            string res = "";
            if (sgen.GetSession(Myguid, "SSEEKVAL") != null) url = sgen.GetSession(Myguid, "SSEEKVAL").ToString();
            string MID = sgen.GetSession(Myguid, "TMID").ToString().Trim();
            switch (MID)
            {
                //string res = "../../../../../erp/student/update_student.aspx?fstr=" + EncryptDecrypt.Encrypt(url.Trim());
                case "40002.6":
                case "21008.2":
                case "28005.7":
                    res = "../purchase/party?m_id=" + EncryptDecrypt.Encrypt(Myguid) + "&mid=" + EncryptDecrypt.Encrypt(sgen.GetSession(Myguid, "TMID").ToString())
                        + "&fstr=" + EncryptDecrypt.Encrypt(url.Trim());
                    break;
                case "40002.7":
                    res = "../purchase/party_unit?m_id=" + EncryptDecrypt.Encrypt(Myguid) + "&mid=" + EncryptDecrypt.Encrypt(sgen.GetSession(Myguid, "TMID").ToString()) + "" +
                        "&fstr=" + EncryptDecrypt.Encrypt(url.Trim());
                    break;
                case "21000":
                    res = "../Education/std_registration?m_id=" + EncryptDecrypt.Encrypt(Myguid) + "&mid=" + EncryptDecrypt.Encrypt(sgen.GetSession(Myguid, "TMID").ToString()) + "&fstr=" + EncryptDecrypt.Encrypt(url.Trim());
                    break;

                //case "40005.3":
                //    // res = "../vastu/lead?m_id=" + EncryptDecrypt.Encrypt(Session["TMID"].ToString()) + "&mid=" + EncryptDecrypt.Encrypt(Session["TMID"].ToString()) + "&fstr=" + EncryptDecrypt.Encrypt(url.Trim());
                //    Session["parentname"] = "master";
                //    Make_query("Master", "User Wise Lead Detail", "LEADDETAIL", "1");
                //    mq = "js" + "!~!~!" + "callFoo('User Wise Lead Details');";
                //    break;

                default:
                    res = "../Education/std_registration?m_id=" + EncryptDecrypt.Encrypt(Myguid) + "&mid=" + EncryptDecrypt.Encrypt(sgen.GetSession(Myguid, "TMID").ToString()) + "&fstr=" + EncryptDecrypt.Encrypt(url.Trim());
                    break;

            }
            return Content(res);
        }
        #endregion

        #region call back
        [HttpPost]
        public ActionResult Callback(string btnval, string edmode, string viewName, string controllerName, List<Tmodelmain> model)
        {
            string mq = "";
            DataTable dt = new DataTable();
            controllerName = sgen.GetSession(MyGuid, "controllerName").ToString();
            viewName = sgen.GetSession(MyGuid, "viewName").ToString();
            //String URL = Session["SSEEKVAL"].ToString();


            switch (viewName)
            {
                case "invoice_config":

                    switch (btnval)
                    {
                        case "EDIT":

                            mq = "select 'Y' as vchnum,'2' as vchdate,'3' as name";
                            dt = sgen.getdata("SATECH", mq);
                            ViewData["dt1"] = dt;
                            break;
                    }
                    break;
                case "upload_training":
                    //switch (btnval)
                    //{
                    //case "MODULE":

                    var tmodel = model.FirstOrDefault();
                    //string name = tmodel.col1;
                    //string pass = tmodel.col2;
                    //mq = "select 'Y' as vchnum,'2' as vchdate,'3' as name";
                    //dt = sgen.getdata("SATECH", mq);
                    //Tmodelmain tmodel = new Tmodelmain();
                    //tmodel.col1 = "ram";
                    //tmodel.col2 = "ram";
                    //tmodel.Col3 = "ram";
                    //tmodel.Col4 = "ram";
                    model[0].Col1 = (sgen.Make_int(model[0].Col1) + 1).ToString();
                    model[0].Col2 = "ram";
                    UpdateModel(model);
                    //return PartialView(viewName, model);
                    break;


                    //}
                    //break;
            }
            return RedirectToAction(viewName, model);
        }
        public List<Tmodelmain> CallbackFun(string btnval, string edmode, string viewName, string controllerName, List<Tmodelmain> model)
        {
            var tm = model.FirstOrDefault();
            DataTable dtr = new DataTable();
            DataTable dtm = new DataTable();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();

            String URL = sgen.GetSession(MyGuid, "SSEEKVAL").ToString();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<SelectListItem> mod2 = new List<SelectListItem>();
            List<SelectListItem> mod3 = new List<SelectListItem>();
            List<SelectListItem> mod4 = new List<SelectListItem>();
            List<SelectListItem> mod5 = new List<SelectListItem>();
            List<SelectListItem> mod6 = new List<SelectListItem>();
            List<SelectListItem> mod7 = new List<SelectListItem>();
            List<SelectListItem> mod11 = new List<SelectListItem>();
            switch (viewName.ToLower())
            {
                #region invoice config

                case "invoice_config":

                    switch (btnval)
                    {
                        case "EDIT":

                            mq = "select 'Y' as vchnum,'2' as vchdate,'3' as name";
                            dtm = sgen.getdata("SATECH", mq);
                            ViewData["dt1"] = dtm;
                            break;
                    }
                    break;

                #endregion                                            

                #region ctrls

                case "ctrls":

                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            List<Tmodelmain> rpt_model = new List<Tmodelmain>();
                            mq = "select vch_num,rec_id,ent_by,ent_date,edit_by,edit_date,client_id,client_unit_id,to_char(convert_tz(date1, '+00:00', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Vch_date,to_char(convert_tz(date2, '+00:00', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as From_Date,to_char(convert_tz(date3, '+00:00', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as To_date,col24 as Name,col1,col2,col3,col4,col5,col6,col7,col8,col9,col10,col11,col12,col13,col25 from vehicle_master where concat(client_id, client_unit_id, vch_num, to_char(vch_date, 'yyyymmdd'), type)='" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {

                                #region bindclass


                                DataTable dt1 = new DataTable();
                                mod1 = sgen.BindClass(userCode, unitid_mst);

                                //if (dt1.Rows.Count > 0)
                                //{
                                //    foreach (DataRow dr in dt1.Rows)
                                //    {
                                //        mod1.Add(new SelectListItem { Text = dr["Class"].ToString(), Value = dr["Class_id"].ToString() });
                                //    }
                                TempData[MyGuid + "_Tlist1"] = mod1;

                                //}
                                #endregion
                                #region bindsection


                                mod2 = sgen.BindSection(userCode, unitid_mst); ;
                                //if (dt.Rows.Count > 0)
                                //{
                                //    foreach (DataRow dr in dt1.Rows)
                                //    {
                                //        mod2.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                                //    }
                                TempData[MyGuid + "_Tlist1"] = mod2;
                                //}
                                #endregion

                                foreach (DataRow dr in dt.Rows)
                                {
                                    rpt_model.Add(new Tmodelmain
                                    {

                                        Col1 = dr["client_id"].ToString(),
                                        Col2 = dr["client_unit_id"].ToString(),
                                        Col3 = dr["ent_by"].ToString(),
                                        Col4 = dr["ent_date"].ToString(),
                                        Col5 = dr["edit_by"].ToString(),
                                        Col6 = dr["edit_date"].ToString(),
                                        Col7 = dr["rec_id"].ToString(),
                                        Col8 = "concat(client_id, client_unit_id,master_id, to_char(vch_date, 'yyyymmdd'), type) = '" + URL + "'",
                                        Col9 = tm.Col9,
                                        Col10 = tm.Col10,
                                        Col11 = tm.Col11,
                                        Col12 = tm.Col12,
                                        Col13 = "Update",
                                        Col14 = tm.Col14,
                                        Col15 = tm.Col15,

                                        Col17 = dr["vch_num"].ToString(),
                                        Col18 = dr["Vch_date"].ToString(),
                                        Col19 = dr["From_Date"].ToString(),
                                        Col20 = dr["TO_Date"].ToString(),

                                        Col21 = dr["col25"].ToString(),

                                        Chk1 = Convert.ToBoolean(dr["col2"].ToString()),
                                        Chk2 = Convert.ToBoolean(dr["col3"].ToString()),
                                        Chk3 = Convert.ToBoolean(dr["col4"].ToString()),
                                        Chk4 = Convert.ToBoolean(dr["col5"].ToString()),
                                        Col22 = dr["col6"].ToString(),
                                        Col23 = dr["col7"].ToString(),
                                        Chk5 = Convert.ToBoolean(dr["col8"].ToString()),
                                        Col24 = dr["col9"].ToString(),
                                        Col25 = dr["col10"].ToString(),
                                        Col26 = dr["col11"].ToString(),
                                        Col27 = dr["col13"].ToString(),

                                    });
                                }

                                String[] L1 = System.String.Join(",", dt.Rows.OfType<DataRow>()
                                       .Select(r => r["col1"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems1 = L1;
                                String[] L2 = System.String.Join(",", dt.Rows.OfType<DataRow>()
                                     .Select(r => r["col12"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems2 = L2;


                                rpt_model[0].TList1 = mod1;
                                rpt_model[0].TList2 = mod2;


                                rpt_model[0].SelectedItems1 = L1;
                                rpt_model[0].SelectedItems2 = L2;



                                model = rpt_model;





                            }
                            else { }
                            break;
                    }
                    break;
                #endregion

                #region create_user

                case "create_user":

                    switch (btnval.ToUpper())
                    {
                        case "EDIT":


                            mq = "select vch_num, To_Char(convert_tz(vch_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') as vch_date,ent_by," +
                                "to_char(convert_tz(ent_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') as ent_date," +
                                "user_id,per_id,password,first_name,last_name,cur_country,cur_state,cur_city,email,department,designation,Client_id,Client_Unit_id,mod_id," +
                       "role,trn_approval_admin,email_right,emp_id,to_char(convert_tz(dob,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') as dob," +
                       "to_char(convert_tz(eff_dt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') as eff_dt" +
                       ",to_char(convert_tz(f_dob,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') as Date_of_ann," +
                       "con_number,spouse_name,todo,team,std_type from user_details ud where ud.vch_num||to_char(ud.vch_date, 'yyyymmdd')|| ud.type='" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col1 = dt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dt.Rows[0]["ent_date"].ToString();
                                model[0].Col8 = "vch_num||TO_CHAR(vch_date, 'yyyymmdd')||type = '" + URL + "'";
                                model[0].Col13 = "Update";
                                model[0].Col16 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dt.Rows[0]["vch_date"].ToString();
                                model[0].Col18 = dt.Rows[0]["per_id"].ToString();
                                model[0].Col19 = dt.Rows[0]["emp_id"].ToString();
                                //model[0].Col19 = dt.Rows[0]["first_name"].ToString() + " " + dt.Rows[0]["last_name"].ToString();
                                model[0].Col20 = dt.Rows[0]["user_id"].ToString();
                                model[0].Col21 = EncryptDecrypt.Decrypt(dt.Rows[0]["password"].ToString());
                                model[0].Col22 = EncryptDecrypt.Decrypt(dt.Rows[0]["password"].ToString());
                                model[0].Col23 = dt.Rows[0]["first_name"].ToString();
                                model[0].Col24 = dt.Rows[0]["last_name"].ToString();
                                model[0].Col25 = dt.Rows[0]["dob"].ToString();
                                model[0].Col26 = dt.Rows[0]["Date_of_ann"].ToString();
                                model[0].Col27 = dt.Rows[0]["spouse_name"].ToString();
                                model[0].Col28 = dt.Rows[0]["con_number"].ToString();
                                model[0].Col29 = dt.Rows[0]["email"].ToString();
                                model[0].Col30 = dt.Rows[0]["eff_dt"].ToString();
                                model[0].Col33 = dt.Rows[0]["role"].ToString();

                                model[0].Col100 = "Update & New";
                                model[0].Col121 = "U";
                                model[0].Col122 = "<u>U</u>pdate";
                                model[0].Col123 = "Update & Ne<u>w</u>";

                                model[0].Chk1 = false;
                                if (dt.Rows[0]["email_right"].ToString() == "T") model[0].Chk2 = true;
                                else model[0].Chk2 = false;
                                model[0].Chk3 = dt.Rows[0]["todo"].ToString() == "Y" ? true : false;

                                #region Country

                                DataTable dt1 = new DataTable();

                                mq = "select distinct country_name,alpha_2 from country_state  order by country_name";


                                dt1 = sgen.getdata(userCode, mq);


                                if (dt1.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dt1.Rows)
                                    {
                                        mod1.Add(new SelectListItem { Text = dr["country_name"].ToString(), Value = dr["alpha_2"].ToString() });

                                    }

                                }
                                TempData[MyGuid + "_Tlist1"] = model[0].TList1 = mod1;

                                String[] L1 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["cur_country"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems1 = L1;
                                #endregion
                                #region State                       
                                string country_id = string.Join(",", model[0].SelectedItems1);
                                DataTable dt2 = new DataTable();
                                dt2 = sgen.getdata(userCode, "select distinct state_name,state_gst_code from country_state where alpha_2='" + country_id + "'  and state_name!='-'  order by state_name");

                                if (dt2.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dt2.Rows)
                                    {
                                        mod2.Add(new SelectListItem { Text = dr["state_name"].ToString(), Value = dr["state_gst_code"].ToString() });

                                    }
                                }
                                TempData[MyGuid + "_Tlist2"] = model[0].TList2 = mod2;


                                String[] L2 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["cur_state"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems2 = L2;
                                #endregion
                                #region city
                                mod3 = new List<SelectListItem>();
                                string state_id = string.Join(",", model[0].SelectedItems2);
                                string city_correct_name = dt.Rows[0]["cur_city"].ToString();
                                dt2 = new DataTable();
                                dt2 = sgen.getdata(userCode, "SELECT city_name FROM (SELECT distinct city_name FROM country_state where state_gst_code='" + state_id + "' ) tab union SELECT 'Other' city_name from dual");
                                if (dt2.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dt2.Rows)
                                    {
                                        mod3.Add(new SelectListItem { Text = dr["city_name"].ToString(), Value = dr["city_name"].ToString() });

                                    }
                                }
                                TempData[MyGuid + "_Tlist3"] = model[0].TList3 = mod3;


                                var drs = dt2.AsEnumerable().Where(w => (string)w["city_name"] == city_correct_name).Select(s => s);
                                DataTable dtc = dt2.Clone();
                                foreach (DataRow dr in drs) dtc.ImportRow(dr);

                                if (dtc.Rows.Count > 0)
                                {
                                    String[] L3 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["cur_city"].ToString()).Distinct()).Split(',');
                                    model[0].SelectedItems3 = L3;
                                }
                                else
                                {
                                    //ddl_city.SelectedValue = "Other";
                                    //citydiv.Visible = true;
                                    //txt_city.Text = city_correct_name;
                                    model[0].SelectedItems3 = new string[] { "other" };
                                    model[0].Col31 = city_correct_name;

                                }


                                #endregion                                
                                #region department
                                mod4 = new List<SelectListItem>();
                                dtc = new DataTable();
                                dtc = sgen.getdata(userCode, "select master_id,master_name from master_setting where type='MDP'");
                                if (dtc.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dtc.Rows)
                                    {
                                        mod4.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });

                                    }
                                }
                                String[] L4 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["department"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems4 = L4;
                                TempData[MyGuid + "_Tlist4"] = model[0].TList4 = mod4;

                                #endregion
                                #region designation
                                mod5 = new List<SelectListItem>();
                                dtc = new DataTable();
                                dtc = sgen.getdata(userCode, "select master_id,master_name from master_setting where type='MDG'");
                                if (dtc.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dtc.Rows)
                                    {
                                        mod5.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });

                                    }
                                }

                                String[] L5 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["designation"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems5 = L5;
                                TempData[MyGuid + "_Tlist5"] = model[0].TList5 = mod5;

                                #endregion                                
                                #region company
                                mod6 = new List<SelectListItem>();
                                string where = "";
                                //mq = sgen.seekval(userCode, "select client_id from user_details where rec_id='" + Convert.ToInt32(userid_mst) + "'", "client_id");
                                mq = sgen.seekval(userCode, "select client_id from user_details where vch_num='" + userid_mst + "'", "client_id");
                                if (!role_mst.ToUpper().Equals("OWNER")) where = " and company_profile_id in ('" + mq.Replace(",", "','") + "')";


                                DataTable dtcomp = new DataTable();
                                mq = "SELECT  Company_Profile_Id, company_name|| '('|| company_profile_id||')' as nameid FROM  company_profile where type='CP' " + where + "";
                                dtcomp = sgen.getdata(userCode, mq);
                                if (dtcomp.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dtcomp.Rows)
                                    {
                                        mod6.Add(new SelectListItem { Text = dr["nameid"].ToString(), Value = dr["company_profile_id"].ToString() });

                                    }
                                }

                                String[] L6 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["client_id"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems6 = L6;
                                TempData[MyGuid + "_Tlist6"] = model[0].TList6 = mod6;

                                #endregion
                                #region unit

                                string[] comp = string.Join(",", model[0].SelectedItems6).Split(',');


                                DataTable dtunit = new DataTable();

                                //string mq0 = sgen.seekval(userCode, "select client_unit_id from user_details where rec_id='" + Convert.ToInt32(userid_mst) + "'", "client_unit_id");
                                string mq0 = sgen.seekval(userCode, "select client_unit_id from user_details where vch_num='" + userid_mst + "'", "client_unit_id");
                                if (!role_mst.ToUpper().Equals("OWNER")) where = " and cup_id in ('" + mq0.Replace(",", "','") + "')";

                                for (int i = 0; i < comp.Length; i++)
                                {
                                    mq = "SELECT  cup_id,unit_name ||'('|| cup_Id ||')' as nameid FROM company_unit_profile where company_profile_id='" + comp[i] + "'" + where + "";

                                    dtunit = sgen.getdata(userCode, mq);

                                    if (dtunit.Rows.Count > 0)
                                    {

                                        foreach (DataRow dr in dtunit.Rows)
                                        {
                                            mod7.Add(new SelectListItem { Text = dr["nameid"].ToString(), Value = dr["cup_id"].ToString() });

                                        }

                                    }

                                    TempData[MyGuid + "_Tlist7"] = model[0].TList7 = mod7;

                                    String[] L7 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["Client_Unit_id"].ToString()).Distinct()).Split(',');
                                    model[0].SelectedItems7 = L7;

                                    #endregion
                                    #region module
                                    List<SelectListItem> mod8 = new List<SelectListItem>();
                                    dtc = new DataTable();
                                    where = "";
                                    //mq = sgen.seekval(userCode, "select mod_id from user_details where rec_id='" + Convert.ToInt32(userid_mst) + "'", "mod_id");
                                    mq = sgen.seekval(userCode, "select mod_id from user_details where vch_num='" + userid_mst + "'", "mod_id");
                                    if (!role_mst.ToUpper().Equals("OWNER")) where = " and mod_id in ('" + mq.Replace(",", "','") + "')";

                                    DataTable dtcomp_mod = new DataTable();
                                    mq = "SELECT mod_Id, m_name|| '('||mod_id ||')' as nameid FROM com_module join menus on com_module.mod_id=menus.m_id and UPPER(com_code) ='" + userCode.ToUpper() + "'" + where + "";

                                    dtc = sgen.getdata(userCode, mq);
                                    if (dtc.Rows.Count > 0)
                                    {
                                        foreach (DataRow dr in dtc.Rows)
                                        {
                                            mod8.Add(new SelectListItem { Text = dr["nameid"].ToString(), Value = dr["mod_id"].ToString() });

                                        }
                                    }

                                    String[] L8 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["mod_id"].ToString()).Distinct()).Split(',');
                                    model[0].SelectedItems8 = L8;
                                    TempData[MyGuid + "_Tlist8"] = model[0].TList8 = mod8;

                                    #endregion
                                    #region training
                                    List<SelectListItem> mod9 = new List<SelectListItem>();
                                    DataTable dtuser = new DataTable();
                                    dtuser = sgen.getdata(userCode, "SELECT user_id FROM user_details where status='" + "active" + "' and type='CPR' and find_in_set('1000', mod_id)");
                                    if (dtuser.Rows.Count > 0)
                                    {
                                        foreach (DataRow dr in dt.Rows)
                                        {
                                            mod9.Add(new SelectListItem { Text = dr["user_id"].ToString(), Value = dr["user_id"].ToString() });

                                        }
                                    }

                                    String[] L9 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["trn_approval_admin"].ToString()).Distinct()).Split(',');
                                    model[0].SelectedItems9 = L9;

                                    TempData[MyGuid + "_Tlist9"] = model[0].TList9 = mod9;

                                    #endregion
                                    #region team
                                    List<SelectListItem> mod10 = new List<SelectListItem>();
                                    DataTable dtu = new DataTable();
                                    //dtu = sgen.getdata(userCode, "SELECT lpad(rec_id,'6','0') rec_id,user_id FROM user_details where status='active' and type='CPR'");
                                    dtu = sgen.getdata(userCode, "SELECT vch_num,user_id FROM user_details where status='active' and type='CPR'");
                                    if (dtu.Rows.Count > 0)
                                    {
                                        foreach (DataRow dr in dtu.Rows)
                                        {
                                            mod10.Add(new SelectListItem { Text = dr["user_id"].ToString(), Value = dr["vch_num"].ToString() });
                                        }
                                    }

                                    String[] L10 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["team"].ToString()).Distinct()).Split(',');
                                    model[0].SelectedItems10 = L10;

                                    TempData[MyGuid + "_Tlist10"] = model[0].TList10 = mod10;

                                    #endregion
                                    #region usertype
                                    mod11.Add(new SelectListItem { Text = "Employee", Value = "001" });
                                    mod11.Add(new SelectListItem { Text = "Vendor", Value = "002" });
                                    mod11.Add(new SelectListItem { Text = "Customer", Value = "003" });

                                    String[] L11 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["std_type"].ToString()).Distinct()).Split(',');
                                    model[0].SelectedItems11 = L11;
                                    TempData[MyGuid + "_Tlist11"] = model[0].TList11 = mod11;
                                    #endregion                                   
                                }
                            }
                            break;

                        case "INACTIVE":
                            mq = "SELECT ud.vch_num,ud.user_id,ud.first_name,ud.LAST_NAME,GROUP_CONCAT(DISTINCT cp.Company_Name) as company_name," +
               "GROUP_CONCAT(DISTINCT cu.Unit_Name) as unit_name,ud.email,msdg.master_name as designation,msdp.master_name as department,ud.status," +
               "ud.client_id,ud.client_unit_id FROM user_details ud " +
               "join master_setting msdp on ud.DEPARTMENT = msdp.master_id " +
               "join master_setting msdg on ud.designation = msdg.master_id " +
               "join company_profile cp on find_in_set(cp.Company_Profile_Id, ud.client_id)=1 " +
               "join company_unit_profile cu on find_in_set(cu.cup_id, ud.client_unit_id)=1 " +
               "where ud.type = 'CPR' and msdp.type = 'MDP' and msdg.type = 'MDG' and cp.type = 'CP' and ud.vch_num||to_char(ud.vch_date, 'yyyymmdd')||ud.type in('" + URL + "') " +
               "group by ud.user_id,ud.first_name,ud.LAST_NAME,ud.email,ud.designation,msdg.master_name," +
               "ud.department,msdp.master_name order by ud.vch_num desc";
                            DataTable dtval = sgen.getdata(userCode, mq);
                            if (dtval.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtval.Rows.Count; i++)
                                {
                                    string clientName = dtval.Rows[i]["company_name"].ToString();
                                    string unitName = dtval.Rows[i]["unit_name"].ToString();
                                    string rec_id = dtval.Rows[i]["vch_num"].ToString();
                                    string userId = dtval.Rows[i]["user_id"].ToString();
                                    string email = dtval.Rows[i]["email"].ToString();

                                    DataTable dtmail = new DataTable();
                                    dtmail = sgen.getdata(userCode, "select com_email,com_password,com_smtp,com_port from company_profile where company_profile_id='" + clientid_mst + "' and type='CP'");
                                    if (dtmail.Rows.Count > 0)
                                    {
                                        string smtpvalue = Convert.ToString(dtmail.Rows[0]["com_Smtp"]);
                                        string emailIdvalue = Convert.ToString(dtmail.Rows[0]["com_Email"]);
                                        string passwordvalue = EncryptDecrypt.Decrypt(Convert.ToString(dtmail.Rows[0]["com_password"].ToString()));
                                        int portvalue = Convert.ToInt32(dtmail.Rows[0]["com_Port"]);

                                        if (passwordvalue == "" && smtpvalue == "" && portvalue == 0 && emailIdvalue == "") sgen.showmsg(1, "Please configure your mail first", 2);
                                        else
                                        {
                                            sgen.execute_cmd(userCode, "UPDATE USER_DETAILS SET STATUS='inactive' WHERE vch_num='" + rec_id + "'");

                                            #region

                                            dt = new DataTable();
                                            dt = sgen.getdata(userCode, "select group_id,user_id from usergroup where user_id='" + rec_id + "'");
                                            if (dt.Rows.Count > 0)
                                            {
                                                sgen.execute_cmd(userCode, "Delete from usergroup WHERE USER_ID='" + rec_id + "'  ");
                                                sgen.execute_cmd(userCode, "commit");
                                            }


                                            #endregion

                                            MailMessage msg = new MailMessage();
                                            msg.From = new MailAddress(emailIdvalue);
                                            msg.To.Add(email);
                                            msg.Subject = "Account Information";

                                            string msgbody = "<b style='color: #3caee9; font-size: 20px'>Login Details From Admin</b><br /><hr style='border:1px solid black' /><p>Hi <b>" + userId + "</b>,</p><p>This is to notify that your user login has been deactivated. Kindly contact your admin for any clarification and information. Your login details are as below :-</p><table style='font-weight:600'><tr><td>Company Name </td><td>: " + clientName + " </td></tr><tr><td> Unit Name </td><td>: " + unitName + " </td></tr><tr><td> User id </td><td>: " + userId + " </td></tr></table><br /><p></p><p>Thank you,<br />Administrator<br /></p><br /><p>Note:- Please do not reply to this mail as it is an unmonitered alias.</p>";

                                            msg.Body = msgbody;
                                            msg.IsBodyHtml = true;
                                            SmtpClient smtp = new SmtpClient();
                                            smtp.Host = smtpvalue;
                                            System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                                            NetworkCred.UserName = emailIdvalue;
                                            NetworkCred.Password = passwordvalue;
                                            smtp.UseDefaultCredentials = true;
                                            smtp.Credentials = NetworkCred;
                                            smtp.Port = portvalue;
                                            smtp.EnableSsl = true;
                                            smtp.Send(msg);
                                        }
                                    }
                                }
                                sgen.showmsg(1, "Login is Inactive", 1);
                            }
                            break;

                        case "VND":
                            mq = "select distinct (a.vch_num|| to_char(a.vch_date,'yyyymmdd')||a.type) fstr,a.vch_num vendor_code,a.c_name vendor_name " +
                                "from clients_mst a " +
                                "where substr(a.vch_num,0,3)='203' and find_in_set(a.client_unit_id,'" + unitid_mst + "')=1 and " +
                                "(a.vch_num||to_char(a.vch_date, 'yyyymmdd')|| a.type)='" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model[0].Col19 = dt.Rows[0]["vendor_code"].ToString();
                                model[0].Col23 = dt.Rows[0]["vendor_name"].ToString();
                            }
                            break;

                        case "CST":
                            mq = "select distinct (a.vch_num|| to_char(a.vch_date,'yyyymmdd')||a.type) fstr,a.vch_num cust_code,a.c_name cust_name " +
                                "from clients_mst a " +
                                "where substr(a.vch_num,0,3)='303' and find_in_set(a.client_unit_id,'" + unitid_mst + "')=1 and " +
                                "(a.vch_num||to_char(a.vch_date, 'yyyymmdd')|| a.type)='" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model[0].Col19 = dt.Rows[0]["cust_code"].ToString();
                                model[0].Col23 = dt.Rows[0]["cust_name"].ToString();
                            }
                            break;
                    }
                    break;
                #endregion

                #region mytodo

                case "mytodo":

                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select client_id,client_unit_id,ent_by,ent_date,edit_by,edit_date,rec_id,vch_num,vch_date,title,descp,chk_dt,status," +
                                "to_char(convert_tz(fdt,'UTC', '" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') fdt," +
                                "to_char(convert_tz(tdt,'UTC', '" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') tdt,pr,cdt " +
                                "from notifications where (client_id|| client_unit_id|| vch_num|| to_char(vch_date, 'yyyymmdd')|| type)='" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col1 = dt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dt.Rows[0]["ent_date"].ToString();
                                model[0].Col5 = dt.Rows[0]["edit_by"].ToString();
                                model[0].Col6 = dt.Rows[0]["edit_date"].ToString();
                                model[0].Col7 = dt.Rows[0]["rec_id"].ToString();
                                model[0].Col8 = "(client_id||client_unit_id||vch_num||to_char(vch_date, 'yyyymmdd')||type) = '" + URL + "'";
                                model[0].Col9 = tm.Col9;
                                model[0].Col10 = tm.Col10;
                                model[0].Col11 = tm.Col11;
                                model[0].Col12 = tm.Col12;
                                model[0].Col13 = "Update";
                                model[0].Col14 = tm.Col14;
                                model[0].Col15 = tm.Col15;
                                model[0].Col16 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dt.Rows[0]["title"].ToString();
                                model[0].Col18 = dt.Rows[0]["fdt"].ToString();
                                model[0].Col19 = dt.Rows[0]["tdt"].ToString();
                                model[0].Col20 = dt.Rows[0]["descp"].ToString();
                                model[0].Chk1 = dt.Rows[0]["chk_dt"].ToString() == "Y" ? true : false;
                                model[0].Col21 = dt.Rows[0]["status"].ToString();
                                model[0].Chk2 = dt.Rows[0]["pr"].ToString() == "Y" ? true : false;
                                model[0].Col22 = dt.Rows[0]["cdt"].ToString();
                                model[0].Col100 = "Update & New";
                                model[0].Col122 = "<u>U</u>pdate";
                                model[0].Col123 = "Update & Ne<u>w</u>";
                            }
                            break;
                    }
                    break;
                #endregion

                #region createnote
                case "createnote":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select client_id,client_unit_id,ent_by,ent_date,edit_by,edit_date,rec_id,vch_num,vch_date,title,descp,chk_dt,status," +
                                "to_char(convert_tz(fdt,'UTC', '" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') fdt," +
                                "to_char(convert_tz(tdt,'UTC', '" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') tdt " +
                                "from notifications where (client_id|| client_unit_id|| vch_num|| to_char(vch_date, 'yyyymmdd')|| type)='" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col1 = dt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dt.Rows[0]["ent_date"].ToString();
                                model[0].Col5 = dt.Rows[0]["edit_by"].ToString();
                                model[0].Col6 = dt.Rows[0]["edit_date"].ToString();
                                model[0].Col7 = dt.Rows[0]["rec_id"].ToString();
                                model[0].Col8 = "(client_id||client_unit_id||vch_num||to_char(vch_date, 'yyyymmdd')||type) = '" + URL + "'";
                                model[0].Col9 = tm.Col9;
                                model[0].Col10 = tm.Col10;
                                model[0].Col11 = tm.Col11;
                                model[0].Col12 = tm.Col12;
                                model[0].Col13 = "Update";
                                model[0].Col14 = tm.Col14;
                                model[0].Col15 = tm.Col15;
                                model[0].Col16 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dt.Rows[0]["title"].ToString();
                                model[0].Col18 = dt.Rows[0]["fdt"].ToString();
                                model[0].Col19 = dt.Rows[0]["tdt"].ToString();
                                model[0].Col20 = dt.Rows[0]["descp"].ToString();
                                model[0].Chk1 = dt.Rows[0]["chk_dt"].ToString() == "Y" ? true : false;
                                model[0].Col21 = dt.Rows[0]["status"].ToString();
                                model[0].Col100 = "Update & New";
                                model[0].Col122 = "<u>U</u>pdate";
                                model[0].Col123 = "Update & Ne<u>w</u>";

                                DataTable dtf = new DataTable();
                                dtf = sgen.getdata(userCode, "select rec_id,file_name,file_path,col1 from file_tab where ref_code1='" + dt.Rows[0]["vch_num"].ToString() + "' " +
                                    "and type='" + model[0].Col12 + "' and client_unit_id='" + unitid_mst + "'");
                                List<Tmodelmain> ltmf = new List<Tmodelmain>();
                                foreach (DataRow drf in dtf.Rows)
                                {
                                    Tmodelmain tmf = new Tmodelmain();
                                    tmf.Col4 = drf["file_path"].ToString();
                                    tmf.Col1 = drf["file_name"].ToString();
                                    tmf.Col2 = drf["col1"].ToString();
                                    tmf.Col3 = drf["rec_id"].ToString();
                                    ltmf.Add(tmf);
                                }
                                model[0].LTM1 = ltmf;
                            }
                            break;
                    }
                    break;
                #endregion

                #region todolist               
                case "todolist":

                    switch (btnval.ToUpper())
                    {
                        case "RDEL":
                            string fid = sgen.GetSession(MyGuid, "tododelid").ToString();
                            res = sgen.execute_cmd(userCode, "Delete from notifications WHERE (client_id|| client_unit_id|| vch_num|| to_char(vch_date, 'yyyymmdd')|| type)='" + fid + "'");
                            sgen.SetSession(MyGuid, "tododelid", null);
                            if (res)
                            {
                                var LTM = model[0].dt1;
                                model[0].dt1.Rows.RemoveAt(sgen.Make_int(model[0].Col16));
                            }
                            break;
                    }
                    break;

                #endregion                

                #region urights
                case "urights":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":

                            if (URL.Contains("-")) mq2 = URL.Substring(10, (URL.Length - 10));
                            else mq2 = URL.Split(new string[] { "K99" }, StringSplitOptions.None)[1].Trim();
                            mq0 = URL.Substring(0, 6);
                            mq = "select ex.client_id,ex.client_unit_id,ex.ent_by,to_char(ex.ent_date,'" + sgen.GetSaveSqlDateFormat() + "') ent_date,ex.vch_num,ex.type," +
                                  "to_char(ex.vch_date, 'dd/MM/YYYY') vch_date,ex.m_module3,ex.role,ex.modid,m.m_id,m.m_name,m.m_link,m.m_submenu,ex.btnnew,ex.btnedit," +
                                  "ex.btnview,ex.btnextend from menus m " +
                                  "left join urights ex on ex.m_id = m.m_id and ex.m_module3 = m.m_module3 and ex.type = 'K99' and ex.vch_num='" + mq0 + "'" +
                                  "where m.m_module3 = '" + mq2 + "'";

                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");

                                mq1 = "";
                                #region MOD
                                mq = sgen.seekval(userCode, "select mod_id from user_details where vch_num='" + userid_mst + "'", "mod_id");
                                if (!role_mst.ToUpper().Equals("OWNER")) mq1 = " and mod_id in ('" + mq.Replace(",", "','") + "')";
                                mq = "SELECT m_module3,mod_Id, m_name|| '( '||mod_id ||')' as nameid FROM com_module join menus on com_module.mod_id=menus.m_id and upper(com_code) = upper('" + userCode + "')" + mq1 + " " +
                                    "union all select 'common' m_module3,'-' mod_id,'Common (-)' nameid from dual";
                                dt = sgen.getdata(userCode, mq);
                                if (dt.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        mod1.Add(new SelectListItem { Text = dr["nameid"].ToString(), Value = dr["m_module3"].ToString() });
                                    }
                                }
                                #endregion

                                #region role

                                if (dtt.Rows[0]["modid"].ToString().Equals("-"))
                                {
                                    mq = "select '-' u_id,'-' m_id,'-' r_name,'-' m_name from dual";
                                }
                                else
                                {
                                    mq = "SELECT u_id,m_id,(r_name||'( '||u_id||')') r_name,m_name FROM role_authority where m_id='" + dtt.Rows[0]["modid"].ToString() + "'";
                                }
                                dtr = sgen.getdata(userCode, mq);
                                if (dtr.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dtr.Rows)
                                    {
                                        mod2.Add(new SelectListItem { Text = dr["r_name"].ToString(), Value = dr["u_id"].ToString() });
                                    }
                                }
                                #endregion

                                TempData[MyGuid + "_TList1"] = mod1;
                                TempData[MyGuid + "_TList2"] = mod2;
                                model[0].TList1 = mod1;
                                model[0].TList2 = mod2;
                                model[0].SelectedItems1 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["m_module3"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems2 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["role"].ToString()).Distinct()).Split(',');

                                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                                model[0].Col8 = "(vch_num||modid||type||m_module3||role) = '" + URL + "" + dtt.Rows[0]["role"].ToString() + "'";
                                model[0].Col9 = tm.Col9;
                                model[0].Col10 = tm.Col10;
                                model[0].Col11 = tm.Col11;
                                model[0].Col12 = dtt.Rows[0]["type"].ToString();
                                model[0].Col13 = "Update";
                                model[0].Col14 = tm.Col14;
                                model[0].Col15 = tm.Col15;
                                model[0].Col16 = dtt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dtt.Rows[0]["vch_date"].ToString();

                                model[0].dt1 = new DataTable();
                                mq2 = "select '' Sno,'' m_id,'' m_name,'' m_link,'' m_submenu,'false' Btn_New,'false' Btn_Edit,'false' Btn_View,'false' Btn_Extend from dual where 1=2";
                                model[0].dt1 = sgen.getdata(userCode, mq2);
                                for (int i = 0; i < dtt.Rows.Count; i++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["sno"] = i + 1;
                                    dr["m_id"] = dtt.Rows[i]["m_id"].ToString();
                                    dr["m_name"] = dtt.Rows[i]["m_name"].ToString();
                                    dr["m_link"] = dtt.Rows[i]["m_link"].ToString();
                                    dr["m_submenu"] = dtt.Rows[i]["m_submenu"].ToString();
                                    dr["btn_new"] = dtt.Rows[i]["btnnew"].ToString() == "Y" ? true : false;
                                    dr["btn_edit"] = dtt.Rows[i]["btnedit"].ToString() == "Y" ? true : false;
                                    dr["btn_view"] = dtt.Rows[i]["btnview"].ToString() == "Y" ? true : false;
                                    dr["btn_extend"] = dtt.Rows[i]["btnextend"].ToString() == "Y" ? true : false;
                                    model[0].dt1.Rows.Add(dr);
                                }
                                model[0].dt1.AcceptChanges();
                            }
                            break;
                    }
                    break;

                #endregion

                #region mailconfig
                case "mailconfig":
                    switch (btnval.ToUpper())
                    {
                        case "USER":
                            mq = "select (vch_num||type) fstr,vch_num id,(user_id||' ('||nvl(email,'-')||')') userid,(first_name||replace(middle_name,'0','')||last_name) name " +
                                "FROM user_details WHERE (vch_num||type) in ('" + URL + "')";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                string olduids = sgen.GetSession(MyGuid, "mc_uids").ToString().Trim();
                                int rval = sgen.Make_int(sgen.GetSession(MyGuid, "mc_rval").ToString().Trim());
                                string newuids = "";
                                for (int i = 0; i < dtt.Rows.Count; i++)
                                {
                                    newuids = newuids + dtt.Rows[i]["id"].ToString() + ",";
                                }
                                newuids = olduids + "," + newuids.Trim().TrimEnd(',');
                                model[0].dt1.Rows[rval][3] = newuids;
                            }
                            break;
                        case "TEMP":
                            mq = "select (client_unit_id||vch_num||type) fstr,vch_num from enx_tab2 where (client_unit_id||vch_num||type)='" + URL + "'";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                int rval = sgen.Make_int(sgen.GetSession(MyGuid, "tm_rval").ToString().Trim());
                                model[0].dt1.Rows[rval][6] = dtt.Rows[0]["vch_num"].ToString();
                            }
                            break;
                        case "TEMPPH":
                            mq = "select (client_unit_id||vch_num||type) fstr,vch_num from enx_tab2 where (client_unit_id||vch_num||type)='" + URL + "'";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                int rval = sgen.Make_int(sgen.GetSession(MyGuid, "tm_rph").ToString().Trim());
                                model[0].dt1.Rows[rval][7] = dtt.Rows[0]["vch_num"].ToString();
                            }
                            break;
                    }
                    break;

                #endregion

                #region urights_btn
                case "urights_btn":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":

                            //mq2 = URL.Substring(3, ((URL.Length) - 3));

                            mq = "select ex.client_id,ex.client_unit_id,ex.ent_by,to_char(ex.ent_date,'dd/MM/YYYY') ent_date,ex.vch_num,ex.type," +
                              "to_char(ex.vch_date, 'dd/MM/YYYY') vch_date,m.m_module3,ex.role,ex.userid,ex.modid,m.m_id,m.m_name,m.m_link,m.m_submenu," +
                              "ex.btnnew,ex.btnedit,ex.btnsave,ex.btnsavenew,ex.btnview,ex.btnextend,ex.FAVMENU from menus m " +
                              "left join urights ex on ex.m_id = m.m_id and ex.m_module3 = m.m_module3 and (ex.type||ex.userid)='" + URL + "' order by ex.userid asc";

                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");

                                #region user                    
                                mq1 = "";
                                if (!role_mst.ToUpper().Equals("OWNER")) mq1 = " and ent_by='" + userid_mst + "'";
                                //mq = "select lpad(rec_id,6,0) rec_id,user_id from user_details where type='CPR'" + mq1 + "";
                                mq = "select vch_num,user_id from user_details where type='CPR'" + mq1 + "";

                                dtm = sgen.getdata(userCode, mq);
                                if (dtm.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dtm.Rows)
                                    {
                                        mod1.Add(new SelectListItem { Text = dr["user_id"].ToString(), Value = dr["vch_num"].ToString() });
                                    }
                                }
                                #endregion

                                TempData[MyGuid + "_TList1"] = mod1;
                                model[0].TList1 = mod1;
                                model[0].SelectedItems1 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["userid"].ToString()).Distinct()).Split(',');

                                mq1 = "";
                                #region MOD
                                //dtm = sgen.getdata(userCode, "select mod_id,role from user_details where rec_id='" + sgen.Make_int(dtt.Rows[0]["userid"].ToString()) + "'");
                                dtm = sgen.getdata(userCode, "select mod_id,role from user_details where vch_num='" + dtt.Rows[0]["userid"].ToString() + "'");
                                if (dtm.Rows.Count > 0)
                                {
                                    mq = dtm.Rows[0]["mod_id"].ToString();
                                    if (!dtm.Rows[0]["role"].ToString().Equals("OWNER")) mq1 = " and mod_id in ('" + mq.Replace(",", "','") + "')";
                                    mq = "select group_concat(m_module3) m_module3,group_concat(nameid) nameid from " +
                        "(SELECT m_module3,mod_Id, m_name|| ' ('||mod_id ||')' as nameid FROM com_module join menus on com_module.mod_id=menus.m_id and upper(com_code) = upper('" + userCode + "')" + mq1 + " " +
                        "union all " +
                        "select 'common' m_module3,'-' mod_id,'Common (-)' nameid from dual) a";
                                    dtr = sgen.getdata(userCode, mq);
                                    if (dtr.Rows.Count > 0)
                                    {
                                        model[0].Col17 = dtr.Rows[0]["nameid"].ToString();
                                        model[0].Col18 = dtr.Rows[0]["m_module3"].ToString();
                                        model[0].Col19 = dtm.Rows[0]["role"].ToString();
                                    }
                                }
                                #endregion                                                                

                                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                                model[0].Col8 = "(type||userid) = '" + dtt.Rows[0]["type"].ToString() + dtt.Rows[0]["userid"].ToString() + "'";
                                model[0].Col9 = tm.Col9;
                                model[0].Col10 = tm.Col10;
                                model[0].Col11 = tm.Col11;
                                model[0].Col12 = dtt.Rows[0]["type"].ToString();
                                model[0].Col13 = "Update";
                                model[0].Col14 = tm.Col14;
                                model[0].Col15 = tm.Col15;
                                model[0].Col16 = dtt.Rows[0]["vch_num"].ToString();

                                dtt = dtt.Select("m_module3 in ('" + model[0].Col18.Replace(",", "','") + "')").CopyToDataTable();
                                DataView dv = dtt.AsDataView();
                                dv.Sort = "m_id";
                                dtt = dv.ToTable();

                                model[0].dt1 = new DataTable();
                                mq2 = "select '' Sno,'' m_id,'' m_name,'' m_link,'' m_submenu,'' m_module3,'false' Btn_New,'false' Btn_Edit,'false' Btn_View,'false' Btn_Extend ,'false' FAVMENU from dual where 1=2";
                                model[0].dt1 = sgen.getdata(userCode, mq2);
                                for (int i = 0; i < dtt.Rows.Count; i++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["sno"] = i + 1;
                                    dr["m_id"] = dtt.Rows[i]["m_id"].ToString();
                                    dr["m_name"] = dtt.Rows[i]["m_name"].ToString();
                                    dr["m_link"] = dtt.Rows[i]["m_link"].ToString();
                                    dr["m_submenu"] = dtt.Rows[i]["m_submenu"].ToString();
                                    dr["m_module3"] = dtt.Rows[i]["m_module3"].ToString();
                                    dr["btn_new"] = dtt.Rows[i]["btnnew"].ToString() == "Y" ? true : false;
                                    dr["btn_edit"] = dtt.Rows[i]["btnedit"].ToString() == "Y" ? true : false;
                                    dr["btn_view"] = dtt.Rows[i]["btnview"].ToString() == "Y" ? true : false;
                                    dr["btn_extend"] = dtt.Rows[i]["btnextend"].ToString() == "Y" ? true : false;
                                    dr["FAVMENU"] = dtt.Rows[i]["FAVMENU"].ToString() == "Y" ? true : false;
                                    model[0].dt1.Rows.Add(dr);
                                }
                                model[0].dt1.AcceptChanges();
                            }
                            break;

                        case "USER":
                            mq = "select '' Sno,m_id,m_name,m_link,m_submenu,m_module3,btnnew,btnedit,btnview,btnextend from urights where (r.type||r.userid)='" + URL + "'";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                DataView dv = new DataView(dtt);
                                dtm = dv.ToTable(true, "m_module3");

                                string match = "", vals = "";
                                //mq1 = "select role from user_details where type='CPR' and lpad(rec_id,6,0)='" + model[0].SelectedItems1.FirstOrDefault() + "'";
                                mq1 = "select role from user_details where type='CPR' and vch_num='" + model[0].SelectedItems1.FirstOrDefault() + "'";
                                mq1 = sgen.seekval(userCode, mq1, "role");
                                if (mq1 != "0")
                                {
                                    string[] arr = mq1.Split(',');
                                    foreach (string ar in arr)
                                    {
                                        match = ar.Split('-')[0].Trim();
                                        for (int k = 0; k < dtt.Rows.Count; k++)
                                        {
                                            if (match == dtt.Rows[0]["m_module3"].ToString().Trim())
                                            {
                                                vals = vals + match + ",";
                                            }
                                        }
                                    }

                                    dtr = dtt.Select("m_module3 in ('" + vals.TrimEnd(',').Replace(",", "','").Trim() + "')").CopyToDataTable();
                                    if (dtr.Rows.Count > 0)
                                    {
                                        model[0].dt1 = dtr;
                                        sgen.SetSession(MyGuid, "dturightsbtn", dtr);
                                        ViewBag.vnew = "disabled='disabled'";
                                        ViewBag.vedit = "disabled='disabled'";
                                        ViewBag.vsave = "";
                                        ViewBag.vgetdt = "";
                                    }
                                }
                            }
                            break;
                    }
                    break;

                #endregion

                #region db_set
                case "db_set":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            string mq0 = "";
                            mq = "select * from DASHBOARD_SETTING where client_id||client_unit_id|| vch_num|| to_char(vch_date, 'YYYYMMDD')|| type='" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            for (int k = 0; k < dt.Rows.Count; k++)
                            {
                                #region Drill level
                                mod4.Add(new SelectListItem { Text = "1", Value = "1" });
                                mod4.Add(new SelectListItem { Text = "2", Value = "2" });
                                mod4.Add(new SelectListItem { Text = "3", Value = "3" });
                                mod4.Add(new SelectListItem { Text = "4", Value = "4" });
                                #endregion
                                #region Chart type
                                mod5.Add(new SelectListItem { Text = "Line", Value = "line" });
                                mod5.Add(new SelectListItem { Text = "Pie", Value = "pie" });
                                mod5.Add(new SelectListItem { Text = "Bar", Value = "bar" });
                                mod5.Add(new SelectListItem { Text = "Area", Value = "area" });
                                mod5.Add(new SelectListItem { Text = "Column", Value = "column" });
                                #endregion
                                #region Status
                                mod6.Add(new SelectListItem { Text = "Yes", Value = "Y" });
                                mod6.Add(new SelectListItem { Text = "No", Value = "N" });
                                #endregion
                                #region level
                                mod1.Add(new SelectListItem { Text = "1", Value = "01" });
                                mod1.Add(new SelectListItem { Text = "2", Value = "02" });
                                mod1.Add(new SelectListItem { Text = "3", Value = "03" });
                                mod1.Add(new SelectListItem { Text = "4", Value = "04" });
                                mod1.Add(new SelectListItem { Text = "5", Value = "05" });
                                mod1.Add(new SelectListItem { Text = "6", Value = "06" });
                                #endregion
                                #region module
                                string where = "";
                                mq = sgen.seekval(userCode, "select mod_id from user_details where vch_num='" + userid_mst + "'", "mod_id");
                                if (!role_mst.ToUpper().Equals("OWNER")) where = " and mod_id in ('" + mq.Replace(",", "','") + "')";

                                DataTable dtcomp_mod = new DataTable();
                                mq = "SELECT mod_Id, m_name|| ' ('|| mod_id||')' as nameid FROM com_module, menus where com_module.mod_id=menus.m_id and upper(com_code) ='" + userCode.ToUpper() + "'" + where + "";
                                dtcomp_mod = sgen.getdata(userCode, mq);
                                if (dtcomp_mod.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dtcomp_mod.Rows)
                                    {
                                        mod2.Add(new SelectListItem { Text = dr["nameid"].ToString(), Value = dr["mod_id"].ToString() });
                                        //mod3.Add(new SelectListItem { Text = dr["nameid"].ToString(), Value = dr["mod_id"].ToString() });
                                    }

                                }
                                #endregion
                                #region onmodule

                                mq = sgen.seekval(userCode, "select mod_id from user_details where vch_num='" + userid_mst + "'", "mod_id");
                                if (!role_mst.ToUpper().Equals("OWNER")) where = " and mod_id in ('" + mq.Replace(",", "','") + "')";

                                dtcomp_mod = new DataTable();
                                mq = "SELECT mod_Id, m_name|| ' ('|| mod_id||')' as nameid FROM com_module, menus where com_module.mod_id=menus.m_id and upper(com_code) ='" + userCode.ToUpper() + "'" + where + "";
                                dtcomp_mod = sgen.getdata(userCode, mq);
                                if (dtcomp_mod.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dtcomp_mod.Rows)
                                    {
                                        mod3.Add(new SelectListItem { Text = dr["nameid"].ToString(), Value = dr["mod_id"].ToString() });
                                    }

                                }
                                #endregion
                                #region TEMPDATA
                                TempData[MyGuid + "_Tlist1"] = mod1;
                                TempData[MyGuid + "_Tlist2"] = mod2;
                                TempData[MyGuid + "_Tlist3"] = mod3;
                                TempData[MyGuid + "_Tlist4"] = mod4;
                                TempData[MyGuid + "_TList5"] = mod5;
                                TempData[MyGuid + "_TList6"] = mod6;
                                #endregion
                                #region tlist
                                model[0].TList1 = mod1;
                                model[0].TList2 = mod2;
                                model[0].TList3 = mod3;
                                model[0].TList4 = mod4;
                                model[0].TList5 = mod5;
                                model[0].TList6 = mod6;
                                #endregion

                                if (dt.Rows[k]["type"].ToString().Trim().Equals("DBU"))
                                {
                                    ViewBag.btncond = "Readonly = true";
                                }
                                else
                                {
                                    ViewBag.btncond = "";
                                }

                                String[] L1 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["level_"].ToString()).Distinct()).Split(',');
                                String[] L2 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["module"].ToString()).Distinct()).Split(',');
                                String[] L4 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["drill_level"].ToString()).Distinct()).Split(',');
                                String[] L5 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["chart_type"].ToString()).Distinct()).Split(',');
                                String[] L6 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["status"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems1 = L1;
                                model[0].SelectedItems2 = L2;
                                model[0].SelectedItems4 = L4;
                                model[0].SelectedItems5 = L5;
                                model[0].SelectedItems6 = L6;
                                model = getedit(model, dt, URL);
                                model[0].Col18 = dt.Rows[k]["name"].ToString();
                                model[0].Col16 = dt.Rows[k]["VCH_NUM"].ToString().Trim();
                                model[0].Col17 = dt.Rows[k]["chart_no"].ToString().Trim();
                                if (mq0.Equals("")) mq0 = dt.Rows[k]["onmodule"].ToString().Trim();
                                else mq0 = mq0 + "," + dt.Rows[k]["onmodule"].ToString().Trim();
                            }
                            model[0].Col20 = mq0;
                            model[0].Col13 = "Update";
                            model[0].Col8 = "client_id||client_unit_id||vch_num||TO_CHAR(vch_date, 'YYYYMMDD')|| type='" + URL + "'";
                            sgen.SetSession(MyGuid, "EDMODE", "Y");

                            break;

                        case "CHARTS":
                            mq = "select name,level_,drill_level,module,chart_no,chart_type,status from DASHBOARD_SETTING where " +
                    "client_id||client_unit_id||vch_num||TO_CHAR(vch_date, 'YYYYMMDD')|| type='" + URL + "'";
                            dt = sgen.getdata(userCode, mq);

                            if (dt.Rows.Count > 0)
                            {
                                model[0].Col18 = dt.Rows[0]["name"].ToString().Trim();
                                model[0].Col17 = dt.Rows[0]["chart_no"].ToString().Trim();
                                #region Drill level
                                mod4.Add(new SelectListItem { Text = "1", Value = "01" });
                                mod4.Add(new SelectListItem { Text = "2", Value = "02" });
                                mod4.Add(new SelectListItem { Text = "3", Value = "03" });
                                mod4.Add(new SelectListItem { Text = "4", Value = "04" });
                                #endregion
                                #region Chart type
                                mod5.Add(new SelectListItem { Text = "Line", Value = "line" });
                                mod5.Add(new SelectListItem { Text = "Pie", Value = "pie" });
                                mod5.Add(new SelectListItem { Text = "Bar", Value = "bar" });
                                mod5.Add(new SelectListItem { Text = "Area", Value = "area" });
                                mod5.Add(new SelectListItem { Text = "Column", Value = "column" });
                                #endregion
                                #region Status
                                mod6.Add(new SelectListItem { Text = "Yes", Value = "Y" });
                                mod6.Add(new SelectListItem { Text = "No", Value = "N" });
                                #endregion
                                #region level
                                mod1.Add(new SelectListItem { Text = "1", Value = "01" });
                                mod1.Add(new SelectListItem { Text = "2", Value = "02" });
                                mod1.Add(new SelectListItem { Text = "3", Value = "03" });
                                mod1.Add(new SelectListItem { Text = "4", Value = "04" });
                                mod1.Add(new SelectListItem { Text = "5", Value = "05" });
                                mod1.Add(new SelectListItem { Text = "6", Value = "06" });
                                #endregion
                                #region module
                                string where = "";
                                mq = sgen.seekval(userCode, "select mod_id from user_details where vch_num='" + userid_mst + "'", "mod_id");
                                if (!role_mst.ToUpper().Equals("OWNER")) where = " and mod_id in ('" + mq.Replace(",", "','") + "')";

                                DataTable dtcomp_mod = new DataTable();
                                mq = "SELECT mod_Id, m_name|| ' ('|| mod_id||')' as nameid FROM com_module, menus where com_module.mod_id=menus.m_id and com_code ='" + userCode + "'" + where + "";
                                dtcomp_mod = sgen.getdata(userCode, mq);
                                if (dtcomp_mod.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dtcomp_mod.Rows)
                                    {
                                        mod2.Add(new SelectListItem { Text = dr["nameid"].ToString(), Value = dr["mod_id"].ToString() });
                                        //mod3.Add(new SelectListItem { Text = dr["nameid"].ToString(), Value = dr["mod_id"].ToString() });
                                    }

                                }
                                #endregion
                                #region onmodule

                                mq = sgen.seekval(userCode, "select mod_id from user_details where vch_num='" + userid_mst + "'", "mod_id");
                                if (!role_mst.ToUpper().Equals("OWNER")) where = " and mod_id in ('" + mq.Replace(",", "','") + "')";

                                dtcomp_mod = new DataTable();
                                mq = "SELECT mod_Id, m_name|| ' ('|| mod_id||')' as nameid FROM com_module, menus where com_module.mod_id=menus.m_id and com_code ='" + userCode + "'" + where + "";
                                dtcomp_mod = sgen.getdata(userCode, mq);
                                if (dtcomp_mod.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dtcomp_mod.Rows)
                                    {
                                        mod3.Add(new SelectListItem { Text = dr["nameid"].ToString(), Value = dr["mod_id"].ToString() });
                                    }

                                }
                                #endregion

                                #region TEMPDATA
                                TempData[MyGuid + "_Tlist1"] = mod1;
                                TempData[MyGuid + "_Tlist2"] = mod2;
                                TempData[MyGuid + "_Tlist3"] = mod3;
                                TempData[MyGuid + "_Tlist4"] = mod4;
                                TempData[MyGuid + "_TList5"] = mod5;
                                TempData[MyGuid + "_TList6"] = mod6;
                                #endregion
                                #region tlist
                                model[0].TList1 = mod1;
                                model[0].TList2 = mod2;
                                model[0].TList3 = mod3;
                                model[0].TList4 = mod4;
                                model[0].TList5 = mod5;
                                model[0].TList6 = mod6;
                                #endregion
                                String[] L1 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["level_"].ToString()).Distinct()).Split(',');
                                String[] L2 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["module"].ToString()).Distinct()).Split(',');
                                String[] L4 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["drill_level"].ToString()).Distinct()).Split(',');
                                String[] L5 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["chart_type"].ToString()).Distinct()).Split(',');
                                String[] L6 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["status"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems1 = L1;
                                model[0].SelectedItems2 = L2;
                                model[0].SelectedItems4 = L4;
                                model[0].SelectedItems5 = L5;
                                model[0].SelectedItems6 = L6;
                            }
                            break;

                    }
                    break;

                #endregion

                #region clientProfile
                case "client_profile":
                    switch (btnval)
                    {
                        case "EDIT":
                            mq = "select * from company_profile where (company_profile_id||to_char(vch_date, 'yyyymmdd')||type)='" + URL + "'";
                            DataTable dtval = sgen.getdata(userCode, mq);
                            if (dtval.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col13 = "Update";
                                model[0].Col100 = "Update & New";
                                model[0].Col121 = "U";
                                model[0].Col122 = "<u>U</u>pdate";
                                model[0].Col123 = "Update & Ne<u>w</u>";
                                model[0].Col16 = dtval.Rows[0]["vch_num"].ToString().Trim();
                                model[0].Col17 = Convert.ToString(dtval.Rows[0]["Company_Name"]);
                                model[0].Col50 = dtval.Rows[0]["company_profile_id"].ToString();
                                string vill1 = dtval.Rows[0]["company_city"].ToString().Trim();
                                if (vill1.Trim().Length > 2)
                                {
                                    DataTable dtvill1 = new DataTable();
                                    dtvill1 = sgen.getdata(userCode, "select REC_ID,country_name,state_name,city_name from country_state where city_name = '" + dtval.Rows[0]["company_city"].ToString().Trim() + "'");
                                    if (dtvill1.Rows.Count > 0)
                                    {
                                        model[0].Col20 = dtvill1.Rows[0]["country_name"].ToString().Trim();
                                        model[0].Col19 = dtvill1.Rows[0]["state_name"].ToString().Trim();
                                        model[0].Col18 = dtvill1.Rows[0]["city_name"].ToString().Trim();
                                        model[0].Col51 = dtvill1.Rows[0]["rec_id"].ToString();
                                    }
                                }
                                string address = Convert.ToString(dtval.Rows[0]["Company_Address"]);
                                try
                                {
                                    string[] values = address.Split('$');
                                    model[0].Col21 = values[0];
                                    model[0].Col22 = values[1];
                                    model[0].Col23 = values[2];
                                }
                                catch (Exception err) { }
                                model[0].Col24 = Convert.ToString(dtval.Rows[0]["Company_Pincode"]);
                                model[0].Col25 = Convert.ToString(dtval.Rows[0]["Com_pan_No"]);
                                model[0].Col26 = Convert.ToString(dtval.Rows[0]["Company_Cin_No"]);
                                model[0].Col27 = Convert.ToString(dtval.Rows[0]["Company_gstin_No"]);

                                #region client_type 1

                                mod1.Add(new SelectListItem { Text = "Educational Inst.", Value = "Educational Inst." });
                                mod1.Add(new SelectListItem { Text = "Corporate", Value = "Corporate" });
                                TempData[MyGuid + "_Tlist1"] = model[0].TList1 = mod1;
                                #endregion

                                #region  board 2
                                TempData[MyGuid + "_Tlist2"] = model[0].TList2 = mod2 = cmd_Fun.board(userCode, unitid_mst);
                                #endregion

                                #region  comp_client_type3
                                TempData[MyGuid + "_Tlist3"] = model[0].TList3 = mod3 = cmd_Fun.comp_type(userCode);
                                #endregion

                                #region  nature of activity 3
                                string defval = "";
                                TempData[MyGuid + "_Tlist4"] = model[0].TList4 = mod4 = cmd_Fun.natact(userCode, unitid_mst, out defval);
                                #endregion

                                #region date format 5
                                mod5.Add(new SelectListItem { Text = "dd/MM/yyyy", Value = "dd/MM/yyyy" });
                                mod5.Add(new SelectListItem { Text = "dd/MMM/yyyy", Value = "dd/MMM/yyyy" });
                                mod5.Add(new SelectListItem { Text = "dd/MMMM/yyyy", Value = "dd/MMMM/yyyy" });
                                mod5.Add(new SelectListItem { Text = "MM/dd/yyyy", Value = "MM/dd/yyyy" });
                                mod5.Add(new SelectListItem { Text = "MMM/dd/yyyy", Value = "MMM/dd/yyyy" });
                                mod5.Add(new SelectListItem { Text = "MMMM/dd/yyyy", Value = "MMMM/dd/yyyy" });
                                mod5.Add(new SelectListItem { Text = "yyyy/dd/MM", Value = "yyyy/dd/MM" });
                                mod5.Add(new SelectListItem { Text = "yyyy/dd/MMM", Value = "yyyy/dd/MMM" });
                                mod5.Add(new SelectListItem { Text = "yyyy/dd/MMMM", Value = "yyyy/dd/MMMM" });
                                mod5.Add(new SelectListItem { Text = "yyyy/MM/dd", Value = "yyyy/MM/dd" });
                                mod5.Add(new SelectListItem { Text = "yyyy/MMM/dd", Value = "yyyy/MMM/dd" });
                                mod5.Add(new SelectListItem { Text = "yyyy/MMMM/dd", Value = "yyyy/MMMM/dd" });
                                mod5.Add(new SelectListItem { Text = "yyyy/MMMM/dd", Value = "yyyy/MMMM/dd" });

                                mod5.Add(new SelectListItem { Text = "dd-MM-yyyy", Value = "dd-MM-yyyy" });
                                mod5.Add(new SelectListItem { Text = "dd-MMM-yyyy", Value = "dd-MMM-yyyy" });
                                mod5.Add(new SelectListItem { Text = "dd-MMMM-yyyy", Value = "dd-MMMM-yyyy" });
                                mod5.Add(new SelectListItem { Text = "MM-dd-yyyy", Value = "MM-dd-yyyy" });
                                mod5.Add(new SelectListItem { Text = "MMM-dd-yyyy", Value = "MMM-dd-yyyy" });
                                mod5.Add(new SelectListItem { Text = "MMMM-dd-yyyy", Value = "MMMM-dd-yyyy" });
                                mod5.Add(new SelectListItem { Text = "yyyy-dd-MM", Value = "yyyy-dd-MM" });
                                mod5.Add(new SelectListItem { Text = "yyyy-dd-MMM", Value = "yyyy-dd-MMM" });
                                mod5.Add(new SelectListItem { Text = "yyyy-dd-MMMM", Value = "yyyy-dd-MMMM" });
                                mod5.Add(new SelectListItem { Text = "yyyy-MM-dd", Value = "yyyy-MM-dd" });
                                mod5.Add(new SelectListItem { Text = "yyyy-MMM-dd", Value = "yyyy-MMM-dd" });
                                mod5.Add(new SelectListItem { Text = "yyyy-MMMM-dd", Value = "yyyy-MMMM-dd" });
                                mod5.Add(new SelectListItem { Text = "yyyy-MMMM-dd", Value = "yyyy-MMMM-dd" });

                                mod5.Add(new SelectListItem { Text = "dd.MM.yyyy", Value = "dd.MM.yyyy" });
                                mod5.Add(new SelectListItem { Text = "dd.MMM.yyyy", Value = "dd.MMM.yyyy" });
                                mod5.Add(new SelectListItem { Text = "dd.MMMM.yyyy", Value = "dd.MMMM.yyyy" });
                                mod5.Add(new SelectListItem { Text = "MM.dd.yyyy", Value = "MM.dd.yyyy" });
                                mod5.Add(new SelectListItem { Text = "MMM.dd.yyyy", Value = "MMM.dd.yyyy" });
                                mod5.Add(new SelectListItem { Text = "MMMM.dd.yyyy", Value = "MMMM.dd.yyyy" });
                                mod5.Add(new SelectListItem { Text = "yyyy.dd.MM", Value = "yyyy.dd.MM" });
                                mod5.Add(new SelectListItem { Text = "yyyy.dd.MMM", Value = "yyyy.dd.MMM" });
                                mod5.Add(new SelectListItem { Text = "yyyy.dd.MMMM", Value = "yyyy.dd.MMMM" });
                                mod5.Add(new SelectListItem { Text = "yyyy.MM.dd", Value = "yyyy.MM.dd" });
                                mod5.Add(new SelectListItem { Text = "yyyy.MMM.dd", Value = "yyyy.MMM.dd" });
                                mod5.Add(new SelectListItem { Text = "yyyy.MMMM.dd", Value = "yyyy.MMMM.dd" });
                                mod5.Add(new SelectListItem { Text = "yyyy.MMMM.dd", Value = "yyyy.MMMM.dd" });
                                TempData[MyGuid + "_Tlist5"] = model[0].TList5 = mod5;
                                #endregion

                                #region Time format 6
                                mod6.Add(new SelectListItem { Text = "H:mm:ss", Value = "H:mm:ss" });
                                mod6.Add(new SelectListItem { Text = "h:mm:ss tt", Value = "h:mm:ss tt" });
                                mod6.Add(new SelectListItem { Text = "H:mm", Value = "H:mm" });
                                mod6.Add(new SelectListItem { Text = "h:mm tt", Value = "h:mm tt" });
                                TempData[MyGuid + "_Tlist6"] = model[0].TList6 = mod6;

                                #endregion

                                #region  timezone 7

                                TempData[MyGuid + "_Tlist7"] = model[0].TList7 = mod7 = cmd_Fun.timezone(userCode);
                                #endregion

                                model[0].Col29 = Convert.ToString(dtval.Rows[0]["Company_No_Of_Employee"]);
                                model[0].Col30 = Convert.ToString(dtval.Rows[0]["Company_Email_Id"]);
                                model[0].Col31 = Convert.ToString(dtval.Rows[0]["Company_Contact_No"]);
                                model[0].Col32 = Convert.ToString(dtval.Rows[0]["Company_Alternate_Contact_No"]);
                                model[0].Col33 = Convert.ToString(dtval.Rows[0]["Company_Website"]);
                                model[0].Col42 = Convert.ToString(dtval.Rows[0]["Company_Contact_Person_Name"]);
                                model[0].Col44 = Convert.ToString(dtval.Rows[0]["Company_Person_Email"]);
                                model[0].Col43 = Convert.ToString(dtval.Rows[0]["Company_Person_Designation"]);
                                model[0].Col45 = Convert.ToString(dtval.Rows[0]["Company_Person_contact_No"]);
                                model[0].Col38 = Convert.ToString(dtval.Rows[0]["com_email"]);
                                model[0].Col57 = Convert.ToString(dtval.Rows[0]["logo_path"]);
                                model[0].Col58 = Convert.ToString(dtval.Rows[0]["logo_name"]);
                                string pass = EncryptDecrypt.Decrypt(dtval.Rows[0]["com_password"].ToString());
                                model[0].Col39 = pass;
                                model[0].Col40 = Convert.ToString(dtval.Rows[0]["com_port"]);
                                model[0].Col41 = Convert.ToString(dtval.Rows[0]["com_smtp"]);
                                if (Convert.ToBoolean(dtval.Rows[0]["company_status"]) == true) { model[0].Col46 = "1"; } else { model[0].Col46 = "0"; }


                                String[] L5 = System.String.Join(",", dtval.Rows.OfType<DataRow>().Select(r => r["dateformat"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems5 = L5;

                                String[] L6 = System.String.Join(",", dtval.Rows.OfType<DataRow>().Select(r => r["timeformat"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems6 = L6;

                                model[0].Col55 = Convert.ToString(dtval.Rows[0]["latlng"]);
                                if (model[0].Col55 == null || model[0].Col55 == "0") ViewBag.scripCall = ViewBag.scripCall + "initialize(28.477450,77.034138);";
                                else ViewBag.scripCall = ViewBag.scripCall + "initialize(" + model[0].Col55 + ");";

                                model[0].Col47 = Convert.ToString(dtval.Rows[0]["addr"]);

                                if (dtval.Rows[0]["client_type"].Equals("Educational Inst."))
                                {


                                    ViewBag.scripcall += "$('[id *= divacrow]').show();";
                                    ViewBag.scripcall += "$('[id *= divacyr]').show();";
                                    ViewBag.scripcall += "$('[id *= div_eduboard]').show();";



                                    String[] L2 = System.String.Join(",", dtval.Rows.OfType<DataRow>().Select(r => r["edu_board"].ToString()).Distinct()).Split(',');
                                    model[0].SelectedItems2 = L2;

                                    string url1 = Convert.ToString(dtval.Rows[0]["company_profile_id"]) + Convert.ToString(dtval.Rows[0]["company_profile_id"]) + "001" + "ACY";
                                    mqm = "select to_char(convert_tz(from_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') from_date," +
                                        "to_char(convert_tz(to_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') to_date from add_academic_year " +
                                        "where (client_id||client_unit_id||type)='" + url1 + "'";
                                    DataTable dtac = sgen.getdata(userCode, mqm);
                                    if (dtac.Rows.Count > 0)
                                    {
                                        model[0].Col36 = dtac.Rows[0]["from_date"].ToString();
                                        //txt_acf.ReadOnly = true;
                                        model[0].Col37 = dtac.Rows[0]["to_date"].ToString();
                                        //txt_act.ReadOnly = true;
                                    }
                                }

                                else
                                {
                                    //ViewBag.scripcall += "$('[id *= divacrow]').hide();";
                                    //ViewBag.scripcall += "$('[id *= divacyr]').hide();";
                                    //ViewBag.scripcall += "$('[id *= div_eduboard]').hide();";
                                }

                                string url2 = Convert.ToString(dtval.Rows[0]["company_profile_id"]) + Convert.ToString(dtval.Rows[0]["company_profile_id"]) + "001";
                                mqm = "select to_char(convert_tz(year_from,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') year_from," +
                                    "to_char(convert_tz(year_to,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') year_to from com_year " +
                                    "where (client_id||client_unit_id)='" + url2 + "'";
                                DataTable dtfy = sgen.getdata(userCode, mqm);
                                if (dtfy.Rows.Count > 0)
                                {
                                    model[0].Col34 = dtfy.Rows[0]["year_from"].ToString();
                                    //txt_fyf.ReadOnly = true;
                                    model[0].Col35 = dtfy.Rows[0]["year_to"].ToString();
                                    //txt_fyt.ReadOnly = true;
                                }

                                model[0].Col3 = Convert.ToString(dtval.Rows[0]["Company_Ent_By"]);
                                model[0].Col4 = Convert.ToString(dtval.Rows[0]["Company_Ent_Date"]);
                                model[0].Col7 = Convert.ToString(dtval.Rows[0]["rec_id"]);

                                model[0].Col8 = "(company_profile_id||to_char(vch_date, 'yyyymmdd')||type)='" + URL + "'";
                                //ViewState["URL"] = url;

                                model[0].SelectedItems2 = System.String.Join(",", dtval.Rows.OfType<DataRow>().Select(r => r["edu_board"].ToString()).Distinct()).Split(',');

                                model[0].SelectedItems7 = new string[] { dtval.Rows[0]["timezone"].ToString() };

                                String[] L3 = System.String.Join(",", dtval.Rows.OfType<DataRow>().Select(r => r["Company_Type_Of_Company"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems3 = L3;

                                String[] L4 = System.String.Join(",", dtval.Rows.OfType<DataRow>().Select(r => r["Company_Nature_Of_Activity"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems4 = L4;


                                String[] L1 = System.String.Join(",", dtval.Rows.OfType<DataRow>().Select(r => r["client_type"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems1 = L1;


                                #region Logo
                                try
                                {

                                    List<Tmodelmain> ltmf = new List<Tmodelmain>();
                                    if ((dtval.Rows[0]["logo_name"].ToString() != "") && (dtval.Rows[0]["logo_name"].ToString() != "0"))
                                    {
                                        Tmodelmain tmf = new Tmodelmain();


                                        tmf.Col4 = dtval.Rows[0]["logo_path"].ToString();
                                        tmf.Col1 = dtval.Rows[0]["logo_name"].ToString();
                                        tmf.Col6 = "Company Logo";
                                        tmf.Col2 = "LOGO";
                                        tmf.Col7 = "image";
                                        tmf.Col3 = dtval.Rows[0]["rec_id"].ToString();

                                        ltmf.Add(tmf);
                                        model[0].LTM1 = ltmf;
                                    }

                                }
                                catch { }
                                #endregion

                            }

                            break;

                        case "FDEL":
                            string fid = sgen.GetSession(MyGuid, "delid").ToString();


                            // res = sgen.execute_cmd(userCode, "Delete from file_tab WHERE rec_id='" + fid + "'  and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
                            res = sgen.execute_cmd(userCode, "UPDATE company_profile set logo_name='0'," + "logo_path='0' where rec_id='" + fid + "'");
                            sgen.SetSession(MyGuid, "delid", null);
                            if (res)
                            {
                                var LTM = model[0].LTM1;
                                for (int d = 0; d < LTM.Count; d++)
                                {
                                    var id = LTM[d].Col3;
                                    if (id == fid) { model[0].LTM1.RemoveAt(d); }
                                }
                            }
                            break;
                        case "CITY":
                            mq = "select * from country_state where trim(client_id||client_unit_id||vch_num||to_char(vch_date, 'yyyymmdd')||type||rec_id)='" + URL + "'";
                            dtval = sgen.getdata(userCode, mq);
                            if (dtval.Rows.Count > 0)
                            {
                                model[0].Col20 = dtval.Rows[0]["country_name"].ToString();
                                model[0].Col19 = dtval.Rows[0]["state_name"].ToString();
                                model[0].Col18 = dtval.Rows[0]["city_name"].ToString();
                                model[0].Col51 = dtval.Rows[0]["rec_id"].ToString();
                            }

                            break;
                    }
                    break;
                #endregion

                #region unitProfile
                case "unit_profile":
                    switch (btnval)
                    {
                        case "EDIT":
                            mq = "select rec_id,vch_num,vch_date,type,latlng,addr,CUP_Id,Company_Profile_Id,Company_Code,Unit_Code,Unit_Country,Unit_State,Unit_City,Unit_Address,Unit_Pincode," +
                       "Unit_GSTIN_No,Unit_Email,Unit_Contact_No,Unit_Alternate_Contact_No,Unit_website,Unit_Contact_Person_Name,Unit_Person_Designation,Unit_Person_Email_Id,bank,branch,acctno,ifsc," +
                       "Unit_Person_Contact_No,Unit_Name,Unit_Status,edu_board,nvl(to_char(convert_tz(dos,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "'),'') as dos," +
                       "nvl(to_char(convert_tz(doc,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "'),'') as doc,septr,Unit_Ent_By,Unit_Ent_Date,ll_act from company_unit_profile where " +
                       "(cup_id||to_char(vch_date, 'yyyymmdd'))='" + URL + "'";
                            DataTable dtval = sgen.getdata(userCode, mq);
                            if (dtval.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col13 = "Update";
                                model[0].Col100 = "Update & New";
                                model[0].Col121 = "U";
                                model[0].Col122 = "<u>U</u>pdate";
                                model[0].Col123 = "Update & Ne<u>w</u>";
                                model[0].Col16 = dtval.Rows[0]["vch_num"].ToString().Trim();

                                #region company 1
                                try
                                {
                                    string where = "";
                                    //mq = sgen.seekval(userCode, "select client_id from user_details where rec_id='" + Convert.ToInt32(userid_mst) + "'", "client_id");
                                    mq = sgen.seekval(userCode, "select client_id from user_details where vch_num='" + userid_mst + "'", "client_id");
                                    if (!role_mst.ToUpper().Equals("OWNER")) where = " and company_profile_id in ('" + mq.Replace(",", "','") + "')";

                                    DataTable dtcomp = new DataTable();
                                    mq = "SELECT Company_Profile_Id, (company_name|| ' ('|| company_profile_id||')') as nameid FROM company_profile where type='CP' " + where + "";
                                    dtcomp = sgen.getdata(userCode, mq);
                                    if (dtcomp.Rows.Count > 0)
                                    {

                                        foreach (DataRow dr in dtcomp.Rows)
                                        {
                                            mod1.Add(new SelectListItem { Text = dr["nameid"].ToString(), Value = dr["company_profile_id"].ToString() });

                                        }
                                        TempData[MyGuid + "_Tlist1"] = model[0].TList1 = mod1;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    sgen.showmsg(1, ex.Message.ToString(), 0);
                                }
                                #endregion
                                model[0].SelectedItems1 = System.String.Join(",", dtval.Rows.OfType<DataRow>().Select(r => r["Company_Profile_Id"].ToString()).Distinct()).Split(',');


                                model[0].Col17 = Convert.ToString(dtval.Rows[0]["Unit_Code"]);
                                model[0].Col55 = Convert.ToString(dtval.Rows[0]["cup_id"]);
                                model[0].Col18 = Convert.ToString(dtval.Rows[0]["Unit_Name"]);

                                #region  board 2
                                TempData[MyGuid + "_Tlist2"] = model[0].TList2 = mod2 = cmd_Fun.board(userCode, unitid_mst);
                                #endregion

                                model[0].SelectedItems2 = System.String.Join(",", dtval.Rows.OfType<DataRow>().Select(r => r["edu_board"].ToString()).Distinct()).Split(',');

                                #region  country
                                TempData[MyGuid + "_Tlist3"] = model[0].TList3 = mod3 = cmd_Fun.country(userCode, unitid_mst);
                                #endregion



                                model[0].SelectedItems3 = System.String.Join(",", dtval.Rows.OfType<DataRow>().Select(r => r["Unit_Country"].ToString()).Distinct()).Split(',');

                                #region  currency 6
                                TempData[MyGuid + "_Tlist6"] = model[0].TList6 = mod6 = cmd_Fun.curname(userCode, unitid_mst);
                                #endregion

                                model[0].SelectedItems6 = System.String.Join(",", dtval.Rows.OfType<DataRow>().Select(r => r["ll_act"].ToString()).Distinct()).Split(',');

                                TempData[MyGuid + "_Tlist4"] = model[0].TList4 = mod4 = cmd_Fun.ctrystate(userCode, model[0].SelectedItems3.FirstOrDefault());

                                model[0].SelectedItems4 = System.String.Join(",", dtval.Rows.OfType<DataRow>().Select(r => r["Unit_State"].ToString()).Distinct()).Split(',');

                                try
                                {

                                    DataTable dt2 = new DataTable();
                                    dt2 = sgen.getdata(userCode, "SELECT distinct ord,city_name FROM (SELECT '0' as ord,city_name FROM country_state where state_gst_code='" + model[0].SelectedItems4.FirstOrDefault() + "'" +
                                        ") tab order by 1,2");
                                    if (dt2.Rows.Count > 0)
                                    {
                                        foreach (DataRow dr in dt2.Rows)
                                        {
                                            mod5.Add(new SelectListItem { Text = dr["city_name"].ToString(), Value = dr["city_name"].ToString() });

                                        }

                                        TempData[MyGuid + "_Tlist5"] = model[0].TList5 = mod5;

                                        //if (dt.Rows.Count > 0)
                                        model[0].SelectedItems5 = new string[] { dtval.Rows[0]["unit_City"].ToString() };

                                        //var drs = dt2.AsEnumerable().Where(w => (string)w["city_name"] == Convert.ToString(dtval.Rows[0]["unit_City"]).Trim()).Select(s => s);
                                        //dt = dt2.Clone();
                                        //foreach (DataRow dr in drs) dt.ImportRow(dr);
                                        //if (dt.Rows.Count > 0)
                                        //{
                                        ViewBag.scripcall += "$('[id *= citydiv]').hide();";
                                        //ViewBag.scripcall += "$('[id *= citydiv]').hide();";
                                        //    model[0].SelectedItems5 = System.String.Join(",", dtval.Rows.OfType<DataRow>().Select(r => r["unit_City"].ToString()).Distinct()).Split(',');
                                        //}

                                        //else
                                        //{
                                        //    ViewBag.scripcall += "$('[id *= citydiv]').show();";

                                        //    model[0].Col19 = dtval.Rows[0]["unit_City"].ToString();
                                        //    model[0].SelectedItems5 = System.String.Join(",", "Other").Split(',');
                                        //}



                                    }
                                }
                                catch (Exception ex)
                                {


                                }

                                //city_correct_name = Convert.ToString(dtval.Rows[0]["unit_City"]).Trim();


                                model[0].Col31 = Convert.ToString(dtval.Rows[0]["Unit_website"]).Trim();

                                string address = Convert.ToString(dtval.Rows[0]["Unit_Address"]).Trim();

                                if (address != "")
                                {
                                    try
                                    {
                                        string[] values = address.Split('$');
                                        model[0].Col20 = values[0];
                                        model[0].Col21 = values[1];
                                        model[0].Col22 = values[2];
                                    }
                                    catch (Exception err) { }
                                }
                                model[0].Col23 = Convert.ToString(dtval.Rows[0]["Unit_Pincode"]);
                                model[0].Col27 = Convert.ToString(dtval.Rows[0]["Unit_GSTIN_No"]);
                                model[0].Col28 = Convert.ToString(dtval.Rows[0]["Unit_Email"]);
                                model[0].Col29 = Convert.ToString(dtval.Rows[0]["Unit_Contact_No"]);
                                model[0].Col30 = Convert.ToString(dtval.Rows[0]["Unit_Alternate_Contact_No"]);
                                model[0].Col36 = Convert.ToString(dtval.Rows[0]["Unit_Contact_Person_Name"]);
                                model[0].Col38 = Convert.ToString(dtval.Rows[0]["Unit_Person_Email_Id"]);
                                model[0].Col37 = Convert.ToString(dtval.Rows[0]["Unit_Person_Designation"]);
                                model[0].Col39 = Convert.ToString(dtval.Rows[0]["Unit_Person_Contact_No"]);

                                model[0].Col44 = Convert.ToString(dtval.Rows[0]["bank"]);
                                model[0].Col45 = Convert.ToString(dtval.Rows[0]["branch"]);
                                model[0].Col46 = Convert.ToString(dtval.Rows[0]["acctno"]);
                                model[0].Col47 = Convert.ToString(dtval.Rows[0]["ifsc"]);

                                model[0].Col41 = Convert.ToString(dtval.Rows[0]["latlng"]);
                                if (model[0].Col41 == null || model[0].Col41 == "0") ViewBag.scripCall = ViewBag.scripCall + "initialize(28.477450,77.034138);";
                                else ViewBag.scripCall = ViewBag.scripCall + "initialize(" + model[0].Col55 + ");";

                                model[0].Col42 = Convert.ToString(dtval.Rows[0]["addr"]);

                                if (dtval.Rows[0]["unit_status"].ToString().Trim() == "1") { model[0].Col40 = "1"; } else { model[0].Col40 = "0"; }
                                model[0].Col24 = Convert.ToString(dtval.Rows[0]["dos"]);
                                model[0].Col25 = Convert.ToString(dtval.Rows[0]["doc"]);
                                model[0].Col26 = dtval.Rows[0]["septr"].ToString();




                                mq2 = "select client_type from company_profile where type='CP' and company_profile_id='" + Convert.ToString(dtval.Rows[0]["company_profile_id"]) + "'";
                                mq = sgen.seekval(userCode, mq2, "client_type");
                                model[0].Col43 = mq;
                                if (mq.Trim().Equals("Educational Inst."))
                                {
                                    ViewBag.scripcall += "$('[id *= div_rowacyr]').show();";
                                    ViewBag.scripcall += "$('[id *= div_acyr]').show();";
                                    ViewBag.scripcall += "$('[id *= div_eduboard]').show();";


                                    string url1 = Convert.ToString(dtval.Rows[0]["company_profile_id"]) + Convert.ToString(dtval.Rows[0]["cup_id"]) + "ACY";
                                    mqm = "select to_char(convert_tz(from_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') from_date," +
                                        "to_char(convert_tz(to_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') to_date from add_academic_year " +
                                        "where (client_id||client_unit_id|| type)='" + url1 + "'";
                                    DataTable dtac = sgen.getdata(userCode, mqm);
                                    if (dtac.Rows.Count > 0)
                                    {
                                        model[0].Col34 = dtac.Rows[0]["from_date"].ToString();
                                        //txt_acf.ReadOnly = true;

                                        model[0].Col35 = dtac.Rows[0]["to_date"].ToString();
                                        //txt_act.ReadOnly = true;
                                    }
                                }

                                else
                                {
                                    ViewBag.scripcall += "$('[id *= div_rowacyr]').hide();";
                                    ViewBag.scripcall += "$('[id *= div_acyr]').hide();";
                                    ViewBag.scripcall += "$('[id *= div_eduboard]').hide();";
                                }

                                string url2 = Convert.ToString(dtval.Rows[0]["company_profile_id"]) + Convert.ToString(dtval.Rows[0]["cup_id"]);
                                mqm = "select to_char(convert_tz(year_from,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') year_from," +
                                    "to_char(convert_tz(year_to,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') year_to from com_year " +
                                    "where (client_id||client_unit_id)='" + url2 + "'";
                                DataTable dtfy = sgen.getdata(userCode, mqm);
                                if (dtfy.Rows.Count > 0)
                                {
                                    model[0].Col32 = dtfy.Rows[0]["year_from"].ToString();
                                    //txt_fyf.ReadOnly = true;
                                    model[0].Col33 = dtfy.Rows[0]["year_to"].ToString();
                                    //txt_fyt.ReadOnly = true;
                                }

                                model[0].Col3 = Convert.ToString(dtval.Rows[0]["Unit_Ent_By"]);
                                model[0].Col4 = Convert.ToString(dtval.Rows[0]["Unit_Ent_Date"]);
                                model[0].Col7 = Convert.ToString(dtval.Rows[0]["rec_id"]);
                                model[0].Col8 = "(cup_id||to_char(vch_date, 'yyyymmdd'))='" + URL + "'";
                                //ViewState["URL"] = url;
                            }
                            break;
                    }
                    break;
                #endregion

                #region v_schedule
                case "v_schedule":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select a.*,to_char(a.date1, '" + sgen.Getsqldatetimeformat() + "') as to_date from enx_tab2 a where a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type = '" + URL + "'";

                            DataTable dtreg = new DataTable();
                            dtreg = sgen.getdata(userCode, mq);
                            if (dtreg.Rows.Count > 0)
                            {
                                model = getedit(model, dtreg, URL);
                                TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1 = cmd_Fun.mailcc(userCode);
                                model[0].Col16 = dtreg.Rows[0]["vch_num"].ToString();
                                String[] L1 = System.String.Join(",", dtreg.Rows.OfType<DataRow>().Select(r => r["col4"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems1 = L1;
                                model[0].Col17 = dtreg.Rows[0]["col13"].ToString();
                                model[0].Col18 = dtreg.Rows[0]["date1"].ToString();
                            }
                            else
                            {
                                sgen.showmsg(1, "No Data Found", 1);
                            }
                            break;
                    }
                    break;
                #endregion

                #region activations
                case "activations":
                    switch (btnval)
                    {
                        case "EDIT":
                            mq = "select rec_id,vch_num,vch_date,type,compcode,compname,hddno,keycode,ent_by,ent_date from activations " +
                                "where (vch_num||to_char(vch_date, 'yyyymmdd')||type)='" + URL + "'";
                            DataTable dtval = sgen.getdata(userCode, mq);
                            if (dtval.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col13 = "Update";
                                model[0].Col100 = "Update & New";
                                model[0].Col121 = "U";
                                model[0].Col122 = "<u>U</u>pdate";
                                model[0].Col123 = "Update & Ne<u>w</u>";
                                model[0].Col16 = dtval.Rows[0]["vch_num"].ToString().Trim();
                                model[0].Col17 = Convert.ToString(dtval.Rows[0]["compcode"]);
                                model[0].Col18 = Convert.ToString(dtval.Rows[0]["compname"]);
                                model[0].Col19 = Convert.ToString(dtval.Rows[0]["hddno"]);
                                model[0].Col20 = Convert.ToString(dtval.Rows[0]["keycode"]);
                                model[0].Col3 = Convert.ToString(dtval.Rows[0]["ent_by"]);
                                model[0].Col4 = Convert.ToString(dtval.Rows[0]["ent_date"]);
                                model[0].Col7 = Convert.ToString(dtval.Rows[0]["rec_id"]);
                                model[0].Col8 = "(vch_num||to_char(vch_date, 'yyyymmdd')||type)='" + URL + "'";
                            }
                            break;
                    }
                    break;
                    #endregion
            }
            return model;
        }
        #endregion

        #region menus list
        private IEnumerable<SelectItem> _makes = new List<SelectItem>
        {
            new SelectItem { id = 1, text = "Ford" },
            new SelectItem { id = 2, text = "Chevy" },
            new SelectItem { id = 3, text = "Chrysler" },
            new SelectItem { id = 4, text = "Honda" },
            new SelectItem { id = 5, text = "Toyota" }
        };
        [HttpGet]
        public IEnumerable<SelectItem> SearchMake(string id)
        {
            var query = _makes.Where(m => m.text.ToLower().Contains(id.ToLower()));

            return query;
        }
        [HttpGet]
        public IEnumerable<SelectItem> GetMake(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;

            var items = new List<SelectItem>();

            string[] idList = id.Split(new char[] { ',' });
            foreach (var idStr in idList)
            {
                int idInt;
                if (int.TryParse(idStr, out idInt))
                {
                    items.Add(_makes.FirstOrDefault(m => m.id == idInt));
                }
            }

            return items;
        }
        public string Mymenu()
        {
            sgenFun sgen = new sgenFun(MyGuid);
            string m_module1 = sgen.GetCookie(MyGuid, "m_module1").ToString();
            string m_module2 = sgen.GetCookie(MyGuid, "m_module2").ToString();
            string m_module3 = sgen.GetCookie(MyGuid, "m_module3").ToString();
            string ulevel_mst = sgen.GetCookie(MyGuid, "ulevel_mst").ToString();
            string m_id = sgen.GetCookie(MyGuid, "m_id").ToString();
            string utype_mst = sgen.GetCookie(MyGuid, "utype_mst").ToString();

            //string mq0 = "select m_id,m_level,m_name,m_order,m_submenu,m_module1,m_module2,m_module3 from menus where m_module3='tmain' order by m_id,m_level, m_order";
            string mq = "";
            if (utype_mst.Equals("STP") || utype_mst.Equals("STD")) mq = "select * from menus where  m_module3 = '" + m_module3 + "' and u_id>= '" + ulevel_mst + "' order by m_id,m_level, m_order";

            else mq = "select * from menus where  (m_module3 = '" + m_module3 + "' or m_module3 = 'common') and m_module3 not in ('stpmain','stdmain')  and u_id>= '" + ulevel_mst + "' order by m_order";

            DataTable dtparent = new DataTable();

            if (sgen.GetSession(MyGuid, "menudt") != null)
            {
                dtparent = (DataTable)sgen.GetSession(MyGuid, "menudt");
            }
            else
            {
                dtparent = sgen.getdata(userCode, mq);
                sgen.SetSession(MyGuid, "menudt", dtparent);
            }

            foreach (DataRow drk in dtparent.Rows)
            {
                if (drk["m_id"].ToString().Equals("7005.6"))
                {

                }
                if (drk["m_submenu"].ToString().Trim().ToUpper().Equals("FALSE") && drk["m_link"].ToString().Trim().Length > 3)
                {
                    drk["m_link"] = drk["m_link"].ToString().Trim() + ".aspx";

                }
                if (drk["m_submenu"].ToString().Trim().ToUpper().Equals("FALSE"))
                {
                    if (drk["attributes"].ToString().Trim().Contains("onclick=$"))
                    {
                        var bval = drk["attributes"].ToString().Trim();
                        bval = bval.Replace("onclick=$", "");
                        drk["attributes"] = "onclick=$showwait();" + bval;
                    }
                    else drk["attributes"] = "onclick=$showwait();menuclick(this);$";
                }

            }

            dtparent.AcceptChanges();

            sgen.SetSession(MyGuid, "dtparent", dtparent);

            DataTable dt0 = dtparent.AsEnumerable().Where(w => (Int32)w["m_level"] == 2).Select(s => s).CopyToDataTable();
            html = "<ul id='myMenu' class='nav side-menu'>";
            foreach (DataRow dr in dt0.Rows)
            {
                if (dr["m_id"].ToString().Equals("7005.6"))
                {

                }
                if (dr["m_submenu"].ToString().Equals("False"))
                {
                    cli++;
                    if (dr["m_link"].ToString().Trim().Length > 4) html = html + "<li id='l" + cli + "' onclick='givemyid(this)'> <a runat='server' id='m" + dr["m_id"].ToString() + "' " + dr["attributes"].ToString().Replace('$', '\'') + "'><i class='" + dr["m_icon"].ToString() + "'></i>" + dr["m_name"].ToString() + "</a></li>";
                    else html = html + "<li id='l" + cli + "' onclick='givemyid(this)'> <a runat='server' id='m" + dr["m_id"].ToString() + "' " + dr["attributes"].ToString().Replace('$', '\'') + "><i class='" + dr["m_icon"].ToString() + "'></i>" + dr["m_name"].ToString() + "</a></li>";
                }
                else
                {
                    cli++;

                    html = html + "<li id='l" + cli + "' onclick='liclick(this,event);'> <a id='a" + cli + "'><i class='" + dr["m_icon"].ToString() + "'></i>" + dr["m_name"].ToString() + "<span class='fa fa-chevron-down'></span></a>";
                    html = html + "<ul id='u" + cli + "' class='nav child_menu'>";
                    makemenu(dtparent, dr["m_module3"].ToString(), dr["m_module1"].ToString(), Convert.ToInt32(dr["m_level"].ToString()) + 1);
                    html = html + " </ul>";
                }
            }
            html = html + " </ul>";
            return html;
        }
        private void makemenu(DataTable dtparent, string module3, string module1, int level)
        {
            string m_id = sgen.GetCookie(MyGuid, "m_id").ToString();
            try
            {
                DataTable dtstatuswise = dtparent.AsEnumerable().Where(w => (string)w["m_module3"] == module3 &&
                                (string)w["m_module2"] == module1 && (Int32)w["m_level"] == level)
                                                    .Select(s => s).CopyToDataTable();

                foreach (DataRow dr in dtstatuswise.Rows)
                {
                    if (dr["m_id"].ToString().Equals("7005.6"))

                    {

                    }
                    if (dr["m_submenu"].ToString().Equals("False"))
                    {
                        cli++;
                        if (dr["m_link"].ToString().Trim().Length > 4) html = html + "<li id='l" + cli + "' onclick='givemyid(this)'> <a runat='server' id='m" + dr["m_id"].ToString() + "' " + dr["attributes"].ToString().Replace('$', '\'') + " ><i class='" + dr["m_icon"].ToString() + "'></i>" + dr["m_name"].ToString() + "</a></li>";
                        else html = html + "<li id='l" + cli + "' onclick='givemyid(this)'> <a runat='server' id='m" + dr["m_id"].ToString() + "' " + dr["attributes"].ToString().Replace('$', '\'') + "><i class='" + dr["m_icon"].ToString() + "'></i>" + dr["m_name"].ToString() + "</a></li>";
                    }
                    else
                    {
                        cli++;
                        html = html + "<li id='l" + cli + "' onclick='liclick(this,event);' > <a id='a" + cli + "'><i class='" + dr["m_icon"].ToString() + "'></i>" + dr["m_name"].ToString() + "<span class='fa fa-chevron-down'></span></a>";
                        html = html + "<ul id='u" + cli + "' class='nav child_menu'>";
                        makemenu(dtparent, dr["m_module3"].ToString(), dr["m_module1"].ToString(), Convert.ToInt32(dr["m_level"].ToString()) + 1);
                        html = html + " </ul>";
                    }
                }
            }
            catch { }
        }


        [HttpPost]
        public ContentResult Mainsrch(string mid, string Myguid, string srchval)
        {
            FillMst(Myguid);
            sgen.SetSession(MyGuid, "btnval", btnval);
            clientid_mst = sgen.GetCookie(MyGuid, "clientid_mst");
            unitid_mst = sgen.GetCookie(MyGuid, "unitid_mst");
            string cmd = "", title = "", seektype = "1";



            switch (mid)
            {
                case "21000":
                    var cacheManager = new OutputCacheManager();
                    cacheManager.RemoveItem("Education", "std_registration");
                    sgen.SetSession(Myguid, "parentname", "master");

                    title = "List of Invoices";
                    seektype = "1";
                    cmd = "select (p.client_id||p.client_unit_id||p.vch_num||to_char(p.vch_date,'yyyymmdd')||p.type) as fstr,p.vch_num Invoice_Number," +
                               "to_char(convert_tz(p.vch_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') Invoice_Date," +
                               "c.c_name as Party_Name,c.c_gstin as Party_Gstin,c.addr as Party_Address from itransaction p " +
                               "inner join clients_mst c on p.acode = c.vch_num and find_in_set(c.client_unit_id,p.client_unit_id)=1 and c.type = 'BCD' and substr(c.vch_num,0,3)='303' " +
                               "where p.client_unit_id = '" + unitid_mst + "' and SUBSTR(p.type, 1, 1) = '4' and SUBSTR(p.potype, 1, 1)='5' and to_date(to_char(p.vch_DATE,'ddMMyyyy'),'ddMMyyyy') " +
                               "between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "') order by p.vch_num desc";
                    break;

                default:
                    break;
            }


            mq = "js" + "!~!~!" + "callFoo('" + title + "');";
            sgen.open_grid(title, cmd, sgen.Make_int(seektype), srchval, true);
            //Session["parentname"] = "MVC";
            ViewBag.pageurl = sgen.GetSession(MyGuid, "pageurl").ToString();
            //return Content(cmd);            
            return Content(mq);

        }

        [HttpPost]
        public ContentResult MenuClick(string mid, string Myguid)
        {
            sgen = new sgenFun(Myguid);
            userCode = sgen.GetCookie(Myguid, "userCode");
            mid = mid.Substring(1).Trim();
            DataTable dtl = new DataTable();
            try
            {
                dtl = sgen.seekval_dt((DataTable)sgen.GetSession(Myguid, "dtparent"), "m_id='" + mid + "'");
            }
            catch (Exception err)
            {
                mq0 = "select m.m_id,m.m_name,m.m_link,m.m_level,m.m_icon,m.m_css,m.m_submenu,m.m_module1,m.m_module2,m.u_id,m.m_order,m.m_default," +
                             "m.and_type,m.and_link,m.i_link,m.attributes,m.activity_type from menus m" +
                             " where m.m_id='" + mid + "'";

                dtl = sgen.getdata(userCode, mq0);



            }
            string mq = "";
            DataRow dr = dtl.Rows[0];
            if (!dr["m_link"].ToString().Trim().Equals(""))
            {
                mq = dr["m_link"].ToString() + "?m_id=" + EncryptDecrypt.Encrypt(Myguid) + "&mid=" + EncryptDecrypt.Encrypt(mid);
                do
                {
                    mq = mq.Replace("../", "");
                }
                while (mq.Contains("../"));
                mq = "/" + mq;
                var fname = mq.Split('?')[0].ToString();

                if (fname.Trim().ToLower().EndsWith("dashboard.aspx")) mq = "/home/dashboard";

            }
            string menuid = mid.Substring(1);
            if (mq.Trim().Length > 4)
            {
                //Response.Redirect(mq);

            }

            sgen.SetSession(Myguid, "TMID", mid);
            switch (mid)
            {

                case "1008.14":
                    //bool ok = sgen.Export_Backup(userCode, userid_mst, clientid_mst, unitid_mst);
                    try
                    {
                        string batchfile = HttpRuntime.AppDomainAppPath + "\\backups\\batchfile.bat";
                        string dmpfile = HttpRuntime.AppDomainAppPath + "\\backups\\" + userCode + "" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".dmp";
                        string logfile = HttpRuntime.AppDomainAppPath + "\\backups\\" + userCode + "" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".log";
                        string exp_cmd = "exp " + userCode + "/SATECH file='" + dmpfile + "' GRANTS=Y log=" + logfile + "";

                        try
                        {
                            if (System.IO.File.Exists(batchfile))
                            {
                                System.IO.File.Delete(batchfile);
                            }
                            StreamWriter w = new StreamWriter(batchfile, true);
                            w.WriteLine(exp_cmd);
                            w.Flush();
                            w.Close();
                        }
                        catch { }
                        // Get the full file path

                        System.Diagnostics.Process p = new System.Diagnostics.Process();
                        p.StartInfo = new System.Diagnostics.ProcessStartInfo(batchfile);
                        p.Start();

                    }
                    catch (Exception err)
                    {

                    }
                    mq = "js" + "!~!~!" + "showmsgJS(1, 'Data Exported Successfully at Server', 1);";

                    break;
                case "999999":
                    sgen.Expireall();
                    Session.RemoveAll();
                    Session.Abandon();
                    mq = "link" + "!~!~!" + "/home/login";

                    break;
                case "7005.6":
                    var cacheManager = new OutputCacheManager();
                    cacheManager.RemoveItem("Education", "std_registration");
                    sgen.SetSession(Myguid, "parentname", "master");
                    Make_query("Master", "List Of Student", "7005.6", "1", "", "", Myguid);
                    mq = "js" + "!~!~!" + "callFoo('List Of Student');";
                    break;
                case "47002":
                    //var cacheManager = new OutputCacheManager();
                    //cacheManager.RemoveItem("Education", "std_registration");
                    sgen.SetSession(Myguid, "parentname", "master");
                    Make_query("Master", "List Of Pending Orders", "47002", "1", "", "", Myguid);
                    mq = "js" + "!~!~!" + "callFoo('List Of Pending Orders');";
                    break;
                case "80010":
                    sgen.SetSession(Myguid, "parentname", "master");
                    Make_query("Master", "List Of Assets With Me", "80010", "1", "", "", Myguid);
                    mq = "js" + "!~!~!" + "callFoo('List Of Assets With Me');";
                    break;
                case "19000.2":
                    //sgen.open_grid("Daily Work Report", MakeQuery("DETAIL"), 0);
                    break;
                case "4008":
                    if ((clientid_mst != "") && (unitid_mst != "") && (userid_mst != ""))
                    {
                        sgen.Alter_Table(userCode, clientid_mst, unitid_mst, userid_mst);
                        mq = "js" + "!~!~!" + "showmsgJS(1, 'Database Refreshed', 1);$('#ctl00_logout').click();";

                    }
                    break;
                //case "18000.1":
                //    sgen.open_grid("List Of User Login", MakeQuery("USERLOGIN"), 0);
                //    break;
                //case "7005.5":
                //    sgen.open_grid("List Of Pending Applicants", MakeQuery("PENDSTD"), 0);
                //    break;

                case "40002.6":
                    sgen.SetSession(Myguid, "parentname", "master");
                    Make_query("Master", "List Of Accounts", "40002.6", "1", "", "", Myguid);
                    mq = "js" + "!~!~!" + "callFoo('List Of Accounts');";
                    //mq = "js" + "!~!~!" + "callFoo('List Of Accounts');";
                    break;

                case "40002.7":
                    sgen.SetSession(Myguid, "parentname", "master");
                    Make_query("Master", "List Of Units", "40002.7", "1", "", "", Myguid);
                    mq = "js" + "!~!~!" + "callFoo('List Of Units');";
                    break;

                case "40008.1":
                    //Session["parentname"] = "master";
                    //Make_query("Master", "List Of Units", "40008.1", "0");
                    //mq = "js" + "!~!~!" + "callFoo('List Of Orders');";
                    break;

                case "40008.3":
                    sgen.SetSession(Myguid, "parentname", "master");
                    Make_query("Master", "List Of Pending Orders For Assign", "40008.3", "0", "", "", MyGuid);
                    mq = "js" + "!~!~!" + "callFoo('List Of Pending Orders For Assign');";
                    break;

                //case "40005.3":
                //    Session["parentname"] = "master";
                //    Make_query("Master", "List Of Units", "40005.3", "1");
                //    mq = "js" + "!~!~!" + "callFoo('User Wise Leads');";
                //    break;

                case "28005.7":
                    sgen.SetSession(Myguid, "parentname", "master");
                    Make_query("Master", "List Of Vendors", "28005.7", "1", "", "", Myguid);
                    mq = "js" + "!~!~!" + "callFoo('List Of Vendors');";

                    break;

                case "21008.2":
                    sgen.SetSession(Myguid, "parentname", "master");
                    Make_query("Master", "List Of Customers", "21008.2", "1", "", "", Myguid);
                    mq = "js" + "!~!~!" + "callFoo('List Of Customers');";
                    //mq = "js" + "!~!~!" + "callFoo('List Of Accounts');";
                    break;



                default:
                    mq = "link" + "!~!~!" + mq;
                    break;
            }
            return Content(mq);
        }

        public ActionResult Signout(string Myguid)
        {

            return Redirect("/home/login");
        }
        public ContentResult Backup_database()
        {
            try
            {
                string batchfile = HttpRuntime.AppDomainAppPath + "\\backups\\batchfile.bat";
                string dmpfile = HttpRuntime.AppDomainAppPath + "\\backups\\" + userCode + "" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".dmp";
                string logfile = HttpRuntime.AppDomainAppPath + "\\backups\\" + userCode + "" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".log";
                string exp_cmd = "exp " + userCode + "/SATECH file='" + dmpfile + "' GRANTS=Y log=" + logfile + "";

                try
                {
                    if (System.IO.File.Exists(batchfile))
                    {
                        System.IO.File.Delete(batchfile);
                    }
                    StreamWriter w = new StreamWriter(batchfile, true);
                    w.WriteLine(exp_cmd);
                    w.Flush();
                    w.Close();
                }
                catch { }
                // Get the full file path

                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo = new System.Diagnostics.ProcessStartInfo(batchfile);
                p.Start();

            }
            catch (Exception err)
            {

            }
            mq = "Data Exported Successfully at Server";
            return Content(mq);
        }
        #endregion

        #region For Dashboard

        public ActionResult Dashboard()
        {
            FillMst();
            //sgen.IsDuplicate();
            chkRef();
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            m_id_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);

            var model = new List<Tmodelmain>();
            Tmodelmain tm1 = new Tmodelmain();
            tm1.Col14 = mid_mst;
            tm1.Col15 = MyGuid;
            model.Add(tm1);
            //mq = "select datediff(UTC_TIMESTAMP,vch_date)+1 todaydt from history where type='HIS' order by rec_id desc";
            //mq = sgen.seekval(userCode, mq, "todaydt");

            //if (sgen.Make_int(mq) == 0 || sgen.Make_int(mq) > 10) { ViewBag.scripCall = "showmsgJS(2,'Do you want to take database backup',0);"; }
            return View(model);
        }


        [HttpPost]
        public ContentResult ChartList(String MyGuid)
        {

            sgenFun sgen = new sgenFun(MyGuid);
            string modulename_mst = sgen.GetCookie(MyGuid, "m_name").ToString();
            string moduleid_mst = sgen.GetCookie(MyGuid, "m_id").ToString();
            string userCode = sgen.GetCookie(MyGuid, "userCode");
            string userid_mst = sgen.GetCookie(MyGuid, "userid_mst");
            DataTable dtm = new DataTable();
            if (sgen.GetSession(MyGuid, "_CHARTROWS") != null) dtm = (DataTable)sgen.GetSession(MyGuid, "_CHARTROWS");
            if (dtm.Rows.Count < 1)
            {

                string mq = "select ds.vch_num||ds.user_id||ds.type as fstr, ds.vch_num,ds.name,ds.level_ as level1,'-' as link,ds.module,m.m_name,ds.drill_level,ds.chart_type,ds.chart_no," +
                        "ds.data_type,'chartdiv' as divname,'btnchart' as btnid,'-'  as chartjs,'-' as pvt,'-' d3 from " +
                       "dashboard_setting ds inner join menus m on ds.module=m.m_id where ds.type='DBU' and ds.status='Y' and ds.user_id='" + userid_mst + "' and (onmodule='" + moduleid_mst + "' or " +
                       "module='" + moduleid_mst + "') union all select ds.vch_num||ds.user_id||ds.type as fstr, ds.vch_num,ds.name,ds.level_ as level1,'-' as link,ds.module,m.m_name,ds.drill_level," +
                       "ds.chart_type,ds.chart_no,ds.data_type ,'chartdiv' as divname,'btnchart' as btnid,'-'  as chartjs,'-' as pvt,'-' d3 from dashboard_setting ds inner join menus m" +
                       " on ds.module=m.m_id where ds.type='DBA' and ds.status='Y' and ds.module='" + moduleid_mst + "' and " +
                       "chart_no not in (select chart_no from dashboard_setting where type='DBU' and user_id='" + userid_mst + "' and (onmodule='" + moduleid_mst + "' or module='" + moduleid_mst + "'))" +
                       "";

                //mq = "select * from (" + mq + ") a where chart_no='000020'";
                dtm = sgen.getdata(userCode, mq);

                for (int r = 0; r < dtm.Rows.Count; r++)
                {

                    dtm.Rows[r]["divname"] = dtm.Rows[r]["divname"].ToString() + r;
                    dtm.Rows[r]["btnid"] = dtm.Rows[r]["btnid"].ToString() + r;
                }
                dtm.AcceptChanges();
                dtm.TableName = "MainData";
                sgen.SetSession(MyGuid, "_CHARTROWS", dtm);
            }

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = dtm.Copy();
            ds.Tables.Add(dt);

            sgen.SetSession(MyGuid, "_CHARTROWSXML", ds.GetXml());
            return Content(ds.GetXml());

        }
        [HttpPost]
        public ContentResult Populate(string chartno, string divname, string btnid, string type_str, string d3_str, string pivot_str, string refresh, string Myguid)
        {
            MyGuid = Myguid;
            DataTable dt, dtm = (DataTable)sgen.GetSession(MyGuid, "_CHARTROWS");
            try
            {
                if (refresh.Trim().Equals("Y"))
                {
                    sgen.SetSession(MyGuid, "_CHART_" + chartno, null);
                }
                string charttype = "", pvttype = "", d3type = "", module = "", headername = "", hdatatype = "", chartjs = "";
                bool d3 = true, pivot = false;
                DataRow dr = dtm.Select("chart_no=" + chartno + "")[0];
                mq = "";
                //foreach (DataRow drm in dtm.Rows)
                //{
                string cno = dr["chart_no"].ToString();
                if (cno.Trim().Equals(chartno.Trim()))
                {
                    module = dr["module"].ToString();
                    headername = dr["m_name"].ToString();
                    hdatatype = "NOS";
                    charttype = dr["chart_type"].ToString().ToLower();
                    chartjs = dr["chartjs"].ToString();
                    pvttype = dr["pvt"].ToString().ToLower();
                    d3type = dr["d3"].ToString().ToLower();
                }
                if (pivot_str.Trim().ToLower().Contains("pivot")) pivot = true;
                if (d3_str.Trim().ToLower().Contains("2d")) d3 = false;
                mq = GenerateChart(type_str, module, headername, hdatatype, chartno, divname, btnid, d3, pivot);
                dr["chartjs"] = mq;
                dr["chart_type"] = type_str;
                dr["pvt"] = pivot_str;
                dr["d3"] = d3_str;
                //}
                dtm.AcceptChanges();
                sgen.SetSession(MyGuid, "CHARTROWS", dtm);
            }
            catch (Exception err) { }
            return Content(mq);

        }

        public string GenerateChart(string charttype, string module, string headername, string hdatatype, string chartno, string divname, string btnid, bool d3, bool pivot)
        {
            userCode = sgen.GetCookie(MyGuid, "userCode");
            string chartscript = "";
            switch (charttype.ToLower())
            {
                case "pie":
                    chartscript = sgen.FillChart_pie(userCode, "", "pie", makechartquery1(chartno), hdatatype, divname, btnid, headername, d3, chartno, pivot);
                    break;
                case "column":
                    chartscript = sgen.FillChart_stacked(userCode, module, "column", makechartquery1(chartno), "Nos", divname, btnid, headername, d3, "My Trainings", false, pivot, chartno);
                    break;
                case "columnstacked":
                    chartscript = sgen.FillChart_stacked(userCode, module, "column", makechartquery1(chartno), "Nos", divname, btnid, headername, d3, "My Trainings", true, pivot, chartno);
                    break;
                case "meter":
                    chartscript = sgen.FillChart_meter(userCode, module, makechartquery1(chartno), 0, 150, 0, 80, 80, 120, 120, 150, "K/h", divname, btnid, chartno);
                    break;
                case "line":
                    chartscript = sgen.FillChart_line(userCode, module, "line", makechartquery1(chartno), "Nos", divname, btnid, headername, d3, "My Trainings", pivot, chartno);
                    break;
                case "bar":
                    chartscript = sgen.FillChart_stacked(userCode, module, "bar", makechartquery1(chartno), "Nos", divname, btnid, headername, d3, "My Trainings", false, pivot, chartno);
                    break;
                case "barstacked":
                    chartscript = sgen.FillChart_stacked(userCode, module, "bar", makechartquery1(chartno), "Nos", divname, btnid, headername, d3, "My Trainings", true, pivot, chartno);
                    break;

            }
            //string chartscript = sgen.FillChartwithlegend(userCode, module, charttype, headername, "", makechartquery(chartno), "", divname, "bt", "lt");

            try
            {
                chartscript = Regex.Split(chartscript, "Highcharts.chart")[1].ToString();
                chartscript = Regex.Replace(chartscript, "script", "");
                //chartscript = chartscript.Substring(1, chartscript.Length - 7);
                chartscript = "Highcharts.chart" + chartscript.Substring(0, chartscript.Length - 6);
                //System.Web.UI.Page page = HttpContext.Current.Handler as System.Web.UI.Page;
                //ScriptManager.RegisterStartupScript(page, page.GetType(), divname, chartscript, false);
            }
            catch (Exception err) { }

            return divname + "~!~!~" + chartscript + "~!~!~" + btnid;
            //DataTable dtm = (DataTable)context.Session["CHARTROWS"];
            //foreach (DataRow drm in dtm.Rows)
            //{
            //    if (drm["chart_no"].ToString().Trim().Equals(chartno))
            //    {
            //        drm["chartjs"] = divname + "~!~!~" + chartscript;

            //    }
            //}
            //dtm.AcceptChanges();
            //context.Session["CHARTROWS"] = dtm;

        }

        public static string makechartquery1(string chartid)
        {

            string res = "";
            string Ac_Year_id = sgen.GetCookie(MyGuid, "Ac_Year_id");
            string Ac_Year = sgen.GetCookie(MyGuid, "Ac_Year");
            string year_to = sgen.GetCookie(MyGuid, "year_to");
            string year_from = sgen.GetCookie(MyGuid, "year_from"); clientid_mst = sgen.GetCookie(MyGuid, "clientid_mst");

            unitid_mst = sgen.GetCookie(MyGuid, "unitid_mst");
            string curdate = sgen.server_datetime_dt_local(userCode).ToString(sgen.Getdateformat(), CultureInfo.InvariantCulture);
            string mq3 = "";
            switch (chartid)
            {
                case "000001":
                    res = "SELECT 'completed' as QUESPAPER_STATUS ,count(QUESPAPER_STATUS) as Ctn FROM  user_quespaper WHERE QUESPAPER_STATUS='completed' union SELECT 'not completed' as QUESPAPER_STATUS ,count(QUESPAPER_STATUS) as Ctn  FROM  user_quespaper WHERE QUESPAPER_STATUS='not completed'";
                    //                    res = @"select 'January' as Month,'31' as Visits UNION select 'February' as Month,'27' as Visits UNION select 'March' as Month,'34' as Visits UNION select 'April' as Month,'39' as Visits
                    //UNION select 'May' as Month,'21' as Visits UNION select 'June' as Month,'26' as Visits UNION select 'July' as Month,'41' as Visits UNION select 'August' as Month,'26' as Visits
                    //UNION select 'September' as Month,'19' as Visits UNION select 'October' as Month,'52' as Visits UNION select 'November' as Month,'29' as Visits UNION select 'December' as Month,'65' as Visits";
                    break;

                case "000002":
                    res = "select MOD_NAME,count(training_status) as ctn from usercourse group by MOD_NAME";
                    //                    res = @"select 'Ram Rattan' as Employee,'56' as Visits UNION select 'Umed Singh' as Employee,'64' as Visits UNION 
                    //select 'Akash Kumar' as Employee,'67' as Visits UNION select 'Rahul Sharma' as Employee,'56' as Visits UNION
                    //select 'Avinash Shetty' as Employee,'46' as Visits";
                    break;

                case "000003":
                    res = "select MOD_NAME,count(QUESPAPER_STATUS) as ctn from user_quespaper group by MOD_NAME";
                    //                    res = @"select 'January' as Month,'26' as Visits UNION select 'February' as Month,'23' as Visits UNION select 'March' as Month,'27' as Visits UNION select 'April' as Month,'25' as Visits
                    //UNION select 'May' as Month,'21' as Visits UNION select 'June' as Month,'24' as Visits UNION select 'July' as Month,'27' as Visits UNION select 'August' as Month,'27' as Visits
                    //UNION select 'September' as Month,'22' as Visits UNION select 'October' as Month,'26' as Visits UNION select 'November' as Month,'26' as Visits UNION select 'December' as Month,'28' as Visits";

                    break;

                case "000004":
                    res = "SELECT 'Opened' as training_status ,count(training_status) as Ctn FROM  usercourse WHERE training_status='Opened' union SELECT 'Unopened' as training_status ,count(training_status) as Ctn  FROM  usercourse WHERE training_status='Unopened'";
                    //                    res = @"select 'January' as Month,'4800' as Conveyance UNION select 'February' as Month,'2300' as Conveyance UNION select 'March' as Month,'2700' as Conveyance UNION select 'April' as Month,'2500' as Conveyance
                    //UNION select 'May' as Month,'2100' as Conveyance UNION select 'June' as Month,'2400' as Conveyance UNION select 'July' as Month,'2700' as Conveyance UNION select 'August' as Month,'2700' as Conveyance
                    //UNION select 'September' as Month,'2200' as Conveyance UNION select 'October' as Month,'2600' as Conveyance UNION select 'November' as Month,'2600' as Conveyance UNION select 'December' as Month,'2800' as Conveyance";

                    break;

                case "000005":
                    res = "select insm.ins_name as Insurance_Company,count(ins.policy_no) as Policy_Count from Insurance ins join insurance_master insm on ins.ins_comp_id = insm.ins_id where ins.type = 'INP' and insm.ins_type = 'ins_cmp' and ins.client_id='" + clientid_mst + "' and ins.client_unit_id='" + unitid_mst + "' group BY ins.ins_comp_id";
                    break;

                case "000006":
                    res = "select insm.ins_name as Insurance_Company,sum(ins.total_prem) as Revenue from Insurance ins join insurance_master insm on ins.ins_comp_id = insm.ins_id where ins.type = 'INP' and insm.ins_type = 'ins_cmp' and ins.client_id='" + clientid_mst + "' and ins.client_unit_id='" + unitid_mst + "' group BY ins.ins_comp_id";
                    break;

                case "000007":
                    res = "select m.master_name as Customer_Type,count(ins.policy_no) as policy from Insurance ins join master_setting m on ins.cust_type_id=m.master_id where ins.type='INP' and m.master_type='cust_type' and ins.client_id='" + clientid_mst + "' and ins.client_unit_id='" + unitid_mst + "' GROUP by ins.cust_type_id";
                    break;

                case "000008":
                    res = "select m.master_name as Customer_Type,sum(ins.total_prem) as policy from Insurance ins join master_setting m on ins.cust_type_id=m.master_id where ins.type='INP' and m.master_type='cust_type' and ins.client_id='" + clientid_mst + "' and ins.client_unit_id='" + unitid_mst + "' GROUP by ins.cust_type_id";
                    break;

                case "000009":
                    res = "select insm.ins_name as Policy_Type,count(ins.policy_no) as policy from Insurance ins join insurance_master insm on ins.type_of_ins_id = insm.ins_id where ins.type = 'INP' and insm.ins_type = 'typeofins' and ins.client_id='" + clientid_mst + "' and ins.client_unit_id='" + unitid_mst + "' GROUP by ins.type_of_ins_id";
                    break;

                case "000010":
                    res = "SELECT type_of_ins,count(policy_no) from insurance where expiry_date< CURRENT_DATE and type='INP' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "' GROUP BY type_of_ins_id";
                    break;

                case "000011":
                    res = "select insm.ins_name as Insurance_Company,count(DISTINCT ins.clientid) as client_Count from Insurance ins join insurance_master insm on ins.ins_comp_id = insm.ins_id where ins.type = 'INP' and insm.ins_type = 'ins_cmp' and ins.client_id='" + clientid_mst + "' and ins.client_unit_id='" + unitid_mst + "' group BY ins.ins_comp_id";
                    break;

                case "000012":
                    res = "select a.mod_name,sum(a.open) as open,sum(a.unopen) as unopen from (select mod_name,count(training_status) as open,0 as unopen from usercourse where upper(training_status)='OPENED' group by mod_name union all select mod_name,0 as open,count(training_status) as unopen from usercourse where upper(training_status)='UNOPENED' group by mod_name) a group by a.mod_name";
                    break;

                case "000013":
                    res = "select insm.ins_name as Policy_Type,SUM(ins.total_prem) as Revenue from Insurance ins join insurance_master insm on ins.type_of_ins_id = insm.ins_id where ins.type = 'INP' and insm.ins_type = 'typeofins' and ins.client_id='" + clientid_mst + "' and ins.client_unit_id='" + unitid_mst + "' GROUP by ins.type_of_ins_id";
                    break;

                case "000014":
                    res = "select concat('Class ',classname)as classname,sum(totalmount) as totalmount from(select main.*,'Pending' as feestatus,cs.CategoryType,cs.class as classname,cs.CategoryType as class_cat,msc.master_name as cat_name,ms.master_name as sectionnme from (select concat(to_char(fq.due_dt,'%Y%m'),'01') as feedate, concat('01/',to_char(fq.due_dt,'%m/%Y')) as Fee_Month,to_char(fq.due_dt,'%d/%m/%Y') as Due_Date,to_char(Last_Day(fq.due_dt) ,'%d/%m/%Y')as  HideDate,to_char(fq.due_dt,'%m') as Month_Id, date_format (fq.due_dt,'%M') as month_name, fq.grp, fq.due_dt,fq.overdue_dt, cast.*, ac.From_Date,ac.To_Date,ac.academic_year,cast.Fee_Amount as Receipt_Amount,'0.00' as PaidAmount,'Y' as IntialStatus,Round((nvl( (case when sm.IsAmount = 1 THEN ((sm.Amount*cast.Fee_Amount)/100 )ELSE sm.Amount end),0)) ,2)AS scholarship,sm.ScholarshipName,sm.vch_num as Scholarship_id from(select tab.*,ud.RegNumber,ud.Caste_Category,ud.GENDER,ud.FIRST_NAME, ud.MIDDLE_NAME,ud.LAST_NAME,ud.f_firstname,ud.f_middlename,ud.f_lastname,ud.m_firstname,ud.m_middlename,ud.m_lastname from (select sd.vch_num as student_id, sd.reg_no,sd.section,sd.roll_number,sd.RollNo ,cl1.* from student_detail sd inner join (select  CONCAT( sf.Due_Month , ' / ' , sf.Due_Date) as Duedate ,cl.*  from (select school_fee_structure.*,school_frequency.FrequencyName,school_fee_type.FeeTypeName,school_fee_head.HeadName from school_fee_structure,school_frequency,school_fee_type,school_fee_head WHERE  school_frequency.Frequency_Id=school_fee_structure.Frequency_Id and school_fee_type.FeeType_Id=school_fee_structure.FeeType_Id and school_fee_structure.FeeHead_Id=school_fee_head.FeeHead_Id order by Frequency_Id) cl inner join (select * from school_frequency) sf on cl.frequency_id=sf.Frequency_Id) cl1 on sd.CLASS_APPLIED=cl1.class_id ) tab INNER join user_details ud on tab.reg_no=ud.RegNo ) cast left outer join scholarship_master sm on cast.class_id=sm.class_id and cast.Caste_Category=sm.Caste_category_id  and cast.Gender=sm.Gender and cast.feeHead_id=sm.Fee_HeadId INNER join add_academic_year ac ON  cast.academic_year_id=ac.academic_year_id inner join (select tab.* ,date_add(due_dt, interval tab.overdue_date-due_date day ) as overdue_dt from (select m.frequency_id, m.frequencyname,m.From_Date,m.no_trans,m.months,m.due_month,m.due_date,m.overdue_date,@ndt:=(case when @grp<>m.frequencyname or   @ndt=0 then m.from_date + INTERVAL due_month MONTH + INTERVAL due_date-1 DAY else  INTERVAL months MONTH +@ndt end)   as due_dt, @rownum:= @rownum + 1 as row_number,@grp:=m.frequencyname AS grp from (select b.from_date,a.* from school_frequency a inner join (select From_Date from add_academic_year where academic_year_id='" + Ac_Year_id + "' and client_id='" + clientid_mst + "') b on 1=1 inner join (select 1 as num union select 2 as num union select 3 as num union select 4 as num union select 5 as num union select 6 as num union select 7 as num union select 8 as num union select 9 as num union select 10 as num union select 11 as num union select 12 as num) c on c.num<=a.no_trans inner join (SELECT @mnt:=0) e on 1=1  order by frequencyname) m inner join (SELECT @ndt:=0) d on 1=1 inner join (SELECT @grp:=0) g on 1=1   inner join (SELECT @rownum:= 0) r on 1=1 order by frequencyname) tab) fq on fq.frequency_id=cast.frequency_id order by fq.due_dt, fq.grp)main inner join school_fee_type ft on ft.FeeType_Id=main.feetype_id INNER join school_fee_head fh on main.feehead_id=fh.FeeHead_Id INNER join add_class cs on cs.add_class_id=main.class_id INNER JOIN master_setting ms on ms.master_id=main.section and ms.master_type='section' inner join master_setting msc on msc.master_id=cs.CategoryType)main_table group by classname order by classname";
                    break;

                case "000015":
                    res = "select a.class,count(rollno) from (select usession,academic_year ,cat_name, class_cat ,section,section_name , sa_id,first_name ,first_name,fathersname,student_status,Date_of_Apply,Date_of_confirm,class_applied, (class || '-' ||section_name) as class,class_applied,reg_no, roll_number,rollno from(select sd.reg_no, sd.roll_number, sd.rollno, sd.section, nvl(s.master_name,'-') as section_name ,a.client_id,a.client_unit_id,a.type,a.sa_id,a.first_name|| ' '|| replace(a.middle_name, '0', '')|| ' '|| replace(a.last_name, '0', '') AS first_name, a.f_firstname|| ' '||replace(a.f_middlename, '0', '')|| ' '|| replace(a.f_lastname, '0', '') AS fathersname,c.academic_year,b.class,a.student_status,b.CategoryType as class_cat, d.master_name as cat_name,a.class_applied,a.usession,to_char(convert_tz(a.vch_date,'UTC','+05:30'),'dd/MM/YYYY') as Date_of_Apply,to_date(convert_tz(sd.vch_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') as Date_of_confirm from student_applicant_details a inner join add_class b on a.class_applied=b.add_class_id and b.type= 'EAC' and a.client_id= b.client_id and a.client_unit_id= b.client_unit_id inner join add_academic_year c on a.usession= c.academic_year_id and a.client_id= c.client_id   and c.type= 'ACY' inner join master_setting d on b.categorytype=d.master_id and d.type= 'ECC'  and a.client_id= d.client_id and a.client_unit_id = d.client_unit_id left join student_detail sd ON sd.Reg_no= a.regnumber and sd.type= 'STD' AND sd.client_id= a.client_id and sd.client_unit_id= a.client_unit_id left join master_setting s on sd.section= s.master_id and s.type= 'ECS' and sd.client_unit_id= s.client_unit_id  where a.type= 'STD' and a.client_unit_id= '" + unitid_mst + "')tab where client_id='" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "' and type = 'STD' group by usession ,class_applied ,section,class_cat,section,academic_year ,cat_name, first_name,first_name,fathersname,student_status,Date_of_Apply,Date_of_confirm,section_name, (class || '-' ||section_name),reg_no, roll_number,rollno,sa_id order by rollno) a group by a.class ";
                    //mq3 = "(b.client_id||b.client_unit_id||b.vch_num||to_char(b.vch_date,'yyyymmdd')||b.type)";
                    //mq = sgen.Fee_AllData(clientid_mst, unitid_mst, Ac_Year_id, mq3);

                    //res = "select a.student_name,sum(a.Due_Bal_Amt) as Due_Amount from (" + mq + ") a where a.Due_Bal_Amt>0 group by a.student_name";

                    //res = "select concat('Class ',classname)as classname,sum(receipt_amount) as receipt_amount from(SELECT b.class as classname,c.Receipt_no,c.Receipt_Date,d.headname,f.academic_year,g.feetypename,c.receipt_amount,c.vch_date,e.master_name as sectionnme,c.class_id,c.section_id as section, c.frequency_id,c.fee_type_id as feetype_id,c.fee_head_id as feeHead_Id,f.academic_year_id,b.categorytype as class_cat,h.master_name as cat_name from add_class b,student_receipt_detail c,school_fee_head d,master_setting e,add_academic_year f,school_fee_type g,master_setting h where b.add_class_id=c.class_id and c.Type='erd' and d.feehead_id=c.fee_head_id and f.academic_year_id=c.academic_year_id and c.client_id='" + clientid_mst + "' and c.client_unit_id='" + unitid_mst + "' and g.feetype_id=c.fee_type_id and c.section_id=e.master_id and b.categorytype=h.master_id and h.master_type='Edu_Category' and c.academic_year_id='" + Ac_Year_id + "' )main_table group by classname order by classname";
                    break;
                case "000016":
                    mq3 = "(b.client_id||b.client_unit_id||b.vch_num||to_char(b.vch_date,'yyyymmdd')||b.type)";
                    mq = sgen.Fee_AllData(clientid_mst, unitid_mst, Ac_Year_id, mq3);

                    res = "select a.Class Classname,sum(a.Due_Bal_Amt) as Due_Amount from (" + mq + ") a where a.Due_Bal_Amt>0 group by a.Class";

                    //  res = "select concat('Class ',classname)as classname,sum(fee_amount) as fee_amount from (select class_cat,cat_name ,academic_year ,classname,class_id ,sectionnme,section ,feetypename,feetype_id ,headname,feeHead_Id,client_id,client_unit_id,academic_year_id,RegNumber,due_dt,fee_amount,0 as Receipt_Date,0 as receipt_amount from(select main.*,cs.class as classname,cs.CategoryType as class_cat,msc.master_name as cat_name,ms.master_name as sectionnme from (select  fq.grp,fq.due_dt,fq.overdue_dt, cast.*,ac.From_Date,ac.academic_year,ac.To_Date, (case when sm.IsAmount = 1 THEN ((sm.Amount*cast.Fee_Amount)/100 )ELSE sm.Amount end) AS scholarship from(select tab.*,ud.RegNumber,ud.Caste_Category,ud.GENDER,ud.FIRST_NAME,ud.MIDDLE_NAME,ud.LAST_NAME,ud.f_firstname,ud.f_middlename,ud.f_lastname,ud.m_firstname,ud.m_middlename,ud.m_lastname from (select sd.vch_num as student_id, sd.reg_no,sd.section,sd.roll_number,sd.RollNo ,cl1.* from student_detail sd inner join (select  CONCAT( sf.Due_Month , ' / ' , sf.Due_Date) as Duedate ,cl.*  from (select school_fee_structure.*,school_frequency.FrequencyName,school_fee_type.FeeTypeName,school_fee_head.HeadName from school_fee_structure,school_frequency,school_fee_type,school_fee_head WHERE  school_frequency.Frequency_Id=school_fee_structure.Frequency_Id and school_fee_type.FeeType_Id=school_fee_structure.FeeType_Id and school_fee_structure.FeeHead_Id=school_fee_head.FeeHead_Id order by Frequency_Id) cl inner join (select * from school_frequency) sf on cl.frequency_id=sf.Frequency_Id) cl1 on sd.CLASS_APPLIED=cl1.class_id ) tab INNER join user_details ud on tab.reg_no=ud.RegNo ) cast left outer join scholarship_master sm on cast.class_id=sm.class_id and cast.Caste_Category=sm.Caste_category_id  and cast.Gender=sm.Gender and cast.feeHead_id=sm.Fee_HeadId INNER join add_academic_year ac ON cast.academic_year_id=ac.academic_year_id inner join (select tab.* ,date_add(due_dt, interval tab.overdue_date-due_date day ) as overdue_dt from (select m.frequency_id,m.frequencyname,m.From_Date,m.no_trans,m.months,m.due_month,m.due_date,m.overdue_date,@ndt:=(case when @grp<>m.frequencyname or   @ndt=0 then m.from_date + INTERVAL due_month MONTH + INTERVAL due_date-1 DAY else  INTERVAL months MONTH +@ndt end)   as due_dt, @rownum:= @rownum + 1 as row_number,@grp:=m.frequencyname AS grp from (select b.from_date,a.* from school_frequency a inner join (select From_Date from add_academic_year where academic_year_id='" + Ac_Year_id + "') b on 1=1 inner join (select 1 as num union select 2 as num union select 3 as num union select 4 as num union select 5 as num union select 6 as num union select 7 as num union select 8 as num union select 9 as num union select 10 as num union select 11 as num union select 12 as num) c on c.num<=a.no_trans inner join (SELECT @mnt:=0) e on 1=1  order by frequencyname) m inner join (SELECT @ndt:=0) d on 1=1 inner join (SELECT @grp:=0) g on 1=1   inner join (SELECT @rownum:= 0) r on 1=1 order by frequencyname) tab) fq on fq.frequency_id=cast.frequency_id order by fq.due_dt, fq.grp)main inner join school_fee_type ft on ft.FeeType_Id=main.feetype_id INNER join school_fee_head fh on main.feehead_id=fh.FeeHead_Id INNER join add_class cs on cs.add_class_id=main.class_id INNER JOIN master_setting ms on ms.master_id=main.section and ms.master_type='section' inner join master_setting msc on msc.master_id=cs.CategoryType)tbb UNION ALL select class_cat,cat_name ,academic_year ,classname,class_id ,sectionnme,section ,feetypename,feetype_id ,headname,feeHead_Id,client_id,client_unit_id,academic_year_id,RegNumber,Receipt_Date,0 as due_dt,receipt_amount,0 as fee_amount from (SELECT b.class as classname, c.Receipt_no,c.Receipt_Date,d.headname,f.academic_year,g.feetypename, c.receipt_amount,c.vch_date,e.master_name as sectionnme,c.class_id,c.section_id as section, c.frequency_id,c.fee_type_id as feetype_id,c.fee_head_id as feeHead_Id,f.academic_year_id,e.master_name,c.client_id,c.client_unit_id,c.regno as regnumber,b.categorytype as class_cat,h.master_name as cat_name from add_class b,student_receipt_detail c,school_fee_head d,master_setting e, add_academic_year f,school_fee_type g,master_setting h where b.add_class_id=c.class_id and c.Type='erd' and d.feehead_id=c.fee_head_id and f.academic_year_id=c.academic_year_id and c.client_id='" + clientid_mst + "' and c.client_unit_id='" + unitid_mst + "' and g.feetype_id=c.fee_type_id and c.section_id=e.master_id and b.categorytype=h.master_id and h.master_type='Edu_Category') tbb2)main_table group by classname order by classname";
                    break;
                case "000017":
                    res = "select FeetypeName,sum(Fee_amount) as fee_amount from(select main.*,'Pending' as feestatus,cs.CategoryType,cs.class as classname,cs.CategoryType as class_cat,msc.master_name as cat_name,ms.master_name as sectionnme from (select concat(to_char(fq.due_dt,'%Y%m'),'01') as feedate, concat('01/',to_char(fq.due_dt,'%m/%Y')) as Fee_Month,to_char(fq.due_dt,'%d/%m/%Y') as Due_Date,to_char(Last_Day(fq.due_dt) ,'%d/%m/%Y')as  HideDate,to_char(fq.due_dt,'%m') as Month_Id, date_format (fq.due_dt,'%M') as month_name, fq.grp, fq.due_dt,fq.overdue_dt, cast.*, ac.From_Date,ac.To_Date,ac.academic_year,cast.Fee_Amount as Receipt_Amount,'0.00' as PaidAmount,'Y' as IntialStatus,Round((nvl( (case when sm.IsAmount = 1 THEN ((sm.Amount*cast.Fee_Amount)/100 )ELSE sm.Amount end),0)) ,2)AS scholarship,sm.ScholarshipName,sm.vch_num as Scholarship_id from(select tab.*,ud.RegNumber,ud.Caste_Category,ud.GENDER,ud.FIRST_NAME, ud.MIDDLE_NAME,ud.LAST_NAME,ud.f_firstname,ud.f_middlename,ud.f_lastname,ud.m_firstname,ud.m_middlename,ud.m_lastname from (select sd.vch_num as student_id, sd.reg_no,sd.section,sd.roll_number,sd.RollNo ,cl1.* from student_detail sd inner join (select  CONCAT( sf.Due_Month , ' / ' , sf.Due_Date) as Duedate ,cl.*  from (select school_fee_structure.*,school_frequency.FrequencyName,school_fee_type.FeeTypeName,school_fee_head.HeadName from school_fee_structure,school_frequency,school_fee_type,school_fee_head WHERE  school_frequency.Frequency_Id=school_fee_structure.Frequency_Id and school_fee_type.FeeType_Id=school_fee_structure.FeeType_Id and school_fee_structure.FeeHead_Id=school_fee_head.FeeHead_Id order by Frequency_Id) cl inner join (select * from school_frequency) sf on cl.frequency_id=sf.Frequency_Id) cl1 on sd.CLASS_APPLIED=cl1.class_id ) tab INNER join user_details ud on tab.reg_no=ud.RegNo ) cast left outer join scholarship_master sm on cast.class_id=sm.class_id and cast.Caste_Category=sm.Caste_category_id  and cast.Gender=sm.Gender and cast.feeHead_id=sm.Fee_HeadId INNER join add_academic_year ac ON  cast.academic_year_id=ac.academic_year_id  and ac.client_id ='" + clientid_mst + "' and ac.type='ACY' inner join (select tab.* ,date_add(due_dt, interval tab.overdue_date-due_date day ) as overdue_dt from (select m.frequency_id, m.frequencyname,m.From_Date,m.no_trans,m.months,m.due_month,m.due_date,m.overdue_date,@ndt:=(case when @grp<>m.frequencyname or   @ndt=0 then m.from_date + INTERVAL due_month MONTH + INTERVAL due_date-1 DAY else  INTERVAL months MONTH +@ndt end)   as due_dt, @rownum:= @rownum + 1 as row_number,@grp:=m.frequencyname AS grp from (select b.from_date,a.* from school_frequency a inner join (select From_Date from add_academic_year where academic_year_id='" + Ac_Year_id + "' and client_id='" + clientid_mst + "' and type='ACY') b on 1=1 inner join (select 1 as num union select 2 as num union select 3 as num union select 4 as num union select 5 as num union select 6 as num union select 7 as num union select 8 as num union select 9 as num union select 10 as num union select 11 as num union select 12 as num) c on c.num<=a.no_trans inner join (SELECT @mnt:=0) e on 1=1  order by frequencyname) m inner join (SELECT @ndt:=0) d on 1=1 inner join (SELECT @grp:=0) g on 1=1   inner join (SELECT @rownum:= 0) r on 1=1 order by frequencyname) tab) fq on fq.frequency_id=cast.frequency_id order by fq.due_dt, fq.grp)main inner join school_fee_type ft on ft.FeeType_Id=main.feetype_id INNER join school_fee_head fh on main.feehead_id=fh.FeeHead_Id INNER join add_class cs on cs.add_class_id=main.class_id INNER JOIN master_setting ms on ms.master_id=main.section and ms.master_type='section' inner join master_setting msc on msc.master_id=cs.CategoryType)main_table group by feetypename order by feetypename";
                    break;
                case "000018":
                    //     res = "select feetypename,sum(Receipt_Amount) as Receipt_Amount from(SELECT b.class as classname,c.Receipt_no,c.Receipt_Date,d.headname,f.academic_year,g.feetypename,c.receipt_amount,c.vch_date,e.master_name as sectionnme,c.class_id,c.section_id as section, c.frequency_id,c.fee_type_id as feetype_id,c.fee_head_id as feeHead_Id,f.academic_year_id,b.categorytype as class_cat,h.master_name as cat_name from add_class b,student_receipt_detail c,school_fee_head d,master_setting e,add_academic_year f,school_fee_type g,master_setting h where b.add_class_id=c.class_id and c.Type='erd' and d.feehead_id=c.fee_head_id and f.academic_year_id=c.academic_year_id and c.client_id='" + clientid_mst + "' and c.client_unit_id='" + unitid_mst + "' and g.feetype_id=c.fee_type_id and c.section_id=e.master_id and b.categorytype=h.master_id and h.master_type='Edu_Category' and c.academic_year_id='" + Ac_Year_id + "')main_table group by feetypename order by feetypename";
                    res = "select a.FeeTypeName,sum(a.Receipt_Amount) from (SELECT (case when a.type='ERD' and sum(a.receipt_amount)>0 then 'Regular Fees' when a.type='ERD' and sum(a.receipt_amount)<0 then 'Reversible Fees' when a.type = 'ERC' then 'Concession Fees' when a.type = 'ERW' then 'Mid Term Concession' End) as Status ,ft.FeeTypeName,h.master_name as HeadName, a.Class_Id,cl.class as classname,a.section_id,cs.master_name as Sectionname,a.Receipt_No,a.vch_date as Receipt_Date,sum(a.Receipt_Amount) as Receipt_Amount from student_receipt_detail a   inner join add_class cl on a.Class_Id=cl.add_class_id and cl.type= 'EAC' and a.client_id= cl.client_id and a.client_unit_id= cl.client_unit_id inner join master_setting cs on a.Section_Id= cs.master_id and cs.type= 'ECS' and a.client_id= cs.client_id and a.client_unit_id= cs.client_unit_id inner join school_fee_type ft on a.Fee_type_id= ft.FeeType_Id and a.client_id= ft.client_id and a.client_unit_id= ft.client_unit_id and (ft.master_type= '0' or ft.master_type= '')  INNER join master_setting h on a.Fee_Head_id=h.master_id and h.type= 'EFH' and h.master_type= 'Education Fees Head' and a.client_id= h.client_id and a.client_unit_id= h.client_unit_id WHERE a.client_id= '" + clientid_mst + "' and a.client_unit_id= '" + unitid_mst + "' and a.type in ('ERD','ERC','ERW') GROUP by ft.feetypename,h.master_name,  a.Receipt_No,a.vch_date ,a.type,ft.FeeTypeName,h.master_name , a.Class_Id,cl.class, a.section_id,cs.master_name order  by cast(a.class_id as int),a.vch_date) a group by a.FeeTypeName";
                    break;
                case "000019":
                    //to be changed
                    mq3 = "(b.client_id||b.client_unit_id||b.vch_num||to_char(b.vch_date,'yyyymmdd')||b.type)";
                    mq = sgen.Fee_AllData(clientid_mst, unitid_mst, Ac_Year_id, mq3);

                    res = "select a.Fee_Heads,sum(a.Due_Bal_Amt) as Due_Amount from (" + mq + ") a where a.Due_Bal_Amt>0 group by a.Fee_Heads";

                    //res = "select feetypename,sum(fee_amount) as fee_amount from (select class_cat,cat_name ,academic_year ,classname,class_id ,sectionnme,section ,feetypename,feetype_id ,headname,feeHead_Id,client_id,client_unit_id,academic_year_id,RegNumber,due_dt,fee_amount,0 as Receipt_Date,0 as receipt_amount from(select main.*,cs.class as classname,cs.CategoryType as class_cat,msc.master_name as cat_name,ms.master_name as sectionnme from (select  fq.grp,fq.due_dt,fq.overdue_dt, cast.*,ac.From_Date,ac.academic_year,ac.To_Date, (case when sm.IsAmount = 1 THEN ((sm.Amount*cast.Fee_Amount)/100 )ELSE sm.Amount end) AS scholarship from(select tab.*,ud.RegNumber,ud.Caste_Category,ud.GENDER,ud.FIRST_NAME,ud.MIDDLE_NAME,ud.LAST_NAME,ud.f_firstname,ud.f_middlename,ud.f_lastname,ud.m_firstname,ud.m_middlename,ud.m_lastname from (select sd.vch_num as student_id, sd.reg_no,sd.section,sd.roll_number,sd.RollNo ,cl1.* from student_detail sd inner join (select  CONCAT( sf.Due_Month , ' / ' , sf.Due_Date) as Duedate ,cl.*  from (select school_fee_structure.*,school_frequency.FrequencyName,school_fee_type.FeeTypeName,school_fee_head.HeadName from school_fee_structure,school_frequency,school_fee_type,school_fee_head WHERE  school_frequency.Frequency_Id=school_fee_structure.Frequency_Id and school_fee_type.FeeType_Id=school_fee_structure.FeeType_Id and school_fee_structure.FeeHead_Id=school_fee_head.FeeHead_Id order by Frequency_Id) cl inner join (select * from school_frequency) sf on cl.frequency_id=sf.Frequency_Id) cl1 on sd.CLASS_APPLIED=cl1.class_id ) tab INNER join user_details ud on tab.reg_no=ud.RegNo ) cast left outer join scholarship_master sm on cast.class_id=sm.class_id and cast.Caste_Category=sm.Caste_category_id  and cast.Gender=sm.Gender and cast.feeHead_id=sm.Fee_HeadId INNER join add_academic_year ac ON cast.academic_year_id=ac.academic_year_id inner join (select tab.* ,date_add(due_dt, interval tab.overdue_date-due_date day ) as overdue_dt from (select m.frequency_id,m.frequencyname,m.From_Date,m.no_trans,m.months,m.due_month,m.due_date,m.overdue_date,@ndt:=(case when @grp<>m.frequencyname or   @ndt=0 then m.from_date + INTERVAL due_month MONTH + INTERVAL due_date-1 DAY else  INTERVAL months MONTH +@ndt end)   as due_dt, @rownum:= @rownum + 1 as row_number,@grp:=m.frequencyname AS grp from (select b.from_date,a.* from school_frequency a inner join (select From_Date from add_academic_year where academic_year_id='" + Ac_Year_id + "') b on 1=1 inner join (select 1 as num union select 2 as num union select 3 as num union select 4 as num union select 5 as num union select 6 as num union select 7 as num union select 8 as num union select 9 as num union select 10 as num union select 11 as num union select 12 as num) c on c.num<=a.no_trans inner join (SELECT @mnt:=0) e on 1=1  order by frequencyname) m inner join (SELECT @ndt:=0) d on 1=1 inner join (SELECT @grp:=0) g on 1=1   inner join (SELECT @rownum:= 0) r on 1=1 order by frequencyname) tab) fq on fq.frequency_id=cast.frequency_id order by fq.due_dt, fq.grp)main inner join school_fee_type ft on ft.FeeType_Id=main.feetype_id INNER join school_fee_head fh on main.feehead_id=fh.FeeHead_Id INNER join add_class cs on cs.add_class_id=main.class_id INNER JOIN master_setting ms on ms.master_id=main.section and ms.master_type='section' inner join master_setting msc on msc.master_id=cs.CategoryType)tbb UNION ALL select class_cat,cat_name ,academic_year ,classname,class_id ,sectionnme,section ,feetypename,feetype_id ,headname,feeHead_Id,client_id,client_unit_id,academic_year_id,RegNumber,Receipt_Date,0 as due_dt,receipt_amount,0 as fee_amount from (SELECT b.class as classname, c.Receipt_no,c.Receipt_Date,d.headname,f.academic_year,g.feetypename, c.receipt_amount,c.vch_date,e.master_name as sectionnme,c.class_id,c.section_id as section, c.frequency_id,c.fee_type_id as feetype_id,c.fee_head_id as feeHead_Id,f.academic_year_id,e.master_name,c.client_id,c.client_unit_id,c.regno as regnumber,b.categorytype as class_cat,h.master_name as cat_name from add_class b,student_receipt_detail c,school_fee_head d,master_setting e, add_academic_year f,school_fee_type g,master_setting h where b.add_class_id=c.class_id and c.Type='erd' and d.feehead_id=c.fee_head_id and f.academic_year_id=c.academic_year_id and c.client_id='" + clientid_mst + "' and c.client_unit_id='" + unitid_mst + "' and g.feetype_id=c.fee_type_id and c.section_id=e.master_id and b.categorytype=h.master_id and h.master_type='Edu_Category') tbb2)main_table GROUP by feetypename order by feetypename";
                    break;
                case "000020":
                    res = "select 'Class '||a.class,Count(vw.Presents) from vw_student_attendance vw,add_class a where a.add_class_id=vw.class_id and presents != 0.0 and Atten_Date<utc_timestamp and vw.client_unit_id='" + unitid_mst + "' group by a.class order by a.class";
                    //res = "select concat('Class ',a.class),Count(vw.Presents) from vw_student_attendance vw,add_class a where a.add_class_id=vw.class_id and presents!=0.0 and Atten_Date<utc_timestamp group by a.class order by a.class";
                    break;
                case "000021":
                    res = "SELECT concat('Class ',c.class,',',tt.subject_name,',Lecture ',tt.period),1 as devidation from time_table tt,add_class c where tt.day=to_char(UTC_TIMESTAMP(),'%a') and lpad(tt.Teacher_Id,6,'0')='" + userid_mst + "' and tt.class_id=c.add_class_id";
                    break;
                case "000022":
                    // res = "select concat(Day,' Lecture '),count(teacher_id) from time_table where lpad(Teacher_Id,6,'0')='" + userid_mst + "' and type!='STD' group by Day order by day";
                    res = "select Day||' Lecture ',count(teacher_id) from time_table where type!='STD' and client_unit_id='" + unitid_mst + "' group by Day order by day";
                    break;
                case "000023":
                    //res = "SELECT (tt.subject_name||',Lecture '||tt.period),1 as devidation from time_table tt,student_detail a where tt.day=" +
                    //    " to_char(UTC_TIMESTAMP(),'%a') and tt.class_id=a.class_applied and tt.section_id=a.section and a.type='STD' and" +
                    //    " lpad(a.vch_num,6,'0')='000076'";

                    res = "SELECT (tt.subject_name||',Lecture '||tt.period),1 as devidation from time_table tt,student_detail a where upper(tt.day) = substr(to_char(UTC_TIMESTAMP(), 'DAY'), 0, 3) and tt.class_id = a.class_applied and tt.section_id = a.section and a.type = 'STD'";
                    break;
                case "000024":
                    //res = "SELECT tt.Day||' Lecture ',count(tt.Day) from time_table tt,student_detail a where tt.class_id=a.class_applied and" +
                    //    " tt.section_id=a.section and a.type='STD' and lpad(a.vch_num,6,'0')='000076' group by tt.day order by tt.day";
                    res = "SELECT tt.Day||' Lecture ',count(tt.Day) from time_table tt,student_detail a where tt.class_id=a.class_applied and tt.section_id = a.section and a.type = 'STD' and tt.client_unit_id='" + unitid_mst + "' group by tt.day order by tt.day";
                    break;
                case "000025":
                    //to be changed
                    res = "select concat('Section ',Sectionnme)as sectionnme,sum(totalmount) as totalmount from(select main.*,'Pending' as feestatus,cs.CategoryType,cs.class as classname,cs.CategoryType as class_cat,msc.master_name as cat_name,ms.master_name as sectionnme from (select concat(to_char(fq.due_dt,'%Y%m'),'01') as feedate, concat('01/',to_char(fq.due_dt,'%m/%Y')) as Fee_Month,to_char(fq.due_dt,'%d/%m/%Y') as Due_Date,to_char(Last_Day(fq.due_dt) ,'%d/%m/%Y')as  HideDate,to_char(fq.due_dt,'%m') as Month_Id, date_format (fq.due_dt,'%M') as month_name, fq.grp, fq.due_dt,fq.overdue_dt, cast.*, ac.From_Date,ac.To_Date,ac.academic_year,cast.Fee_Amount as Receipt_Amount,'0.00' as PaidAmount,'Y' as IntialStatus,Round((nvl( (case when sm.IsAmount = 1 THEN ((sm.Amount*cast.Fee_Amount)/100 )ELSE sm.Amount end),0)) ,2)AS scholarship,sm.ScholarshipName,sm.vch_num as Scholarship_id from(select tab.*,ud.RegNumber,ud.Caste_Category,ud.GENDER,ud.FIRST_NAME, ud.MIDDLE_NAME,ud.LAST_NAME,ud.f_firstname,ud.f_middlename,ud.f_lastname,ud.m_firstname,ud.m_middlename,ud.m_lastname from (select sd.vch_num as student_id, sd.reg_no,sd.section,sd.roll_number,sd.RollNo ,cl1.* from student_detail sd inner join (select  CONCAT( sf.Due_Month , ' / ' , sf.Due_Date) as Duedate ,cl.*  from (select school_fee_structure.*,school_frequency.FrequencyName,school_fee_type.FeeTypeName,school_fee_head.HeadName from school_fee_structure,school_frequency,school_fee_type,school_fee_head WHERE  school_frequency.Frequency_Id=school_fee_structure.Frequency_Id and school_fee_type.FeeType_Id=school_fee_structure.FeeType_Id and school_fee_structure.FeeHead_Id=school_fee_head.FeeHead_Id order by Frequency_Id) cl inner join (select * from school_frequency) sf on cl.frequency_id=sf.Frequency_Id) cl1 on sd.CLASS_APPLIED=cl1.class_id ) tab INNER join user_details ud on tab.reg_no=ud.RegNo ) cast left outer join scholarship_master sm on cast.class_id=sm.class_id and cast.Caste_Category=sm.Caste_category_id  and cast.Gender=sm.Gender and cast.feeHead_id=sm.Fee_HeadId INNER join add_academic_year ac ON  cast.academic_year_id=ac.academic_year_id and  ac.client_id='" + clientid_mst + "' and type = 'ACY' inner join (select tab.* ,date_add(due_dt, interval tab.overdue_date-due_date day ) as overdue_dt from (select m.frequency_id, m.frequencyname,m.From_Date,m.no_trans,m.months,m.due_month,m.due_date,m.overdue_date,@ndt:=(case when @grp<>m.frequencyname or   @ndt=0 then m.from_date + INTERVAL due_month MONTH + INTERVAL due_date-1 DAY else  INTERVAL months MONTH +@ndt end)   as due_dt, @rownum:= @rownum + 1 as row_number,@grp:=m.frequencyname AS grp from (select b.from_date,a.* from school_frequency a inner join (select From_Date from add_academic_year where academic_year_id='" + Ac_Year_id + "') b on 1=1 inner join (select 1 as num union select 2 as num union select 3 as num union select 4 as num union select 5 as num union select 6 as num union select 7 as num union select 8 as num union select 9 as num union select 10 as num union select 11 as num union select 12 as num) c on c.num<=a.no_trans inner join (SELECT @mnt:=0) e on 1=1  order by frequencyname) m inner join (SELECT @ndt:=0) d on 1=1 inner join (SELECT @grp:=0) g on 1=1   inner join (SELECT @rownum:= 0) r on 1=1 order by frequencyname) tab) fq on fq.frequency_id=cast.frequency_id order by fq.due_dt, fq.grp)main inner join school_fee_type ft on ft.FeeType_Id=main.feetype_id INNER join school_fee_head fh on main.feehead_id=fh.FeeHead_Id INNER join add_class cs on cs.add_class_id=main.class_id INNER JOIN master_setting ms on ms.master_id=main.section and ms.master_type='section' inner join master_setting msc on msc.master_id=cs.CategoryType)main_table where classname='1' group by sectionnme order by sectionnme";
                    break;
                case "000026":
                    res = "select 'Section '||sectionnme as sectionnme,sum(receipt_amount) as receipt_amount " +
                        "FROM(SELECT b.class as classname,c.Receipt_no,c.Receipt_Date,d.headname,f.academic_year," +
                        "g.feetypename,c.receipt_amount,c.vch_date,e.master_name as sectionnme,c.class_id,c.section_id as " +
                        "section, c.frequency_id,c.fee_type_id as feetype_id,c.fee_head_id as feeHead_Id,f.academic_year_id" +
                        ",b.categorytype as class_cat,h.master_name as cat_name from add_class b,student_receipt_detail c," +
                        "school_fee_head d,master_setting e,add_academic_year f,school_fee_type g,master_setting h where" +
                        " b.add_class_id=c.class_id and c.Type='erd' and d.feehead_id=c.fee_head_id and f.academic_year_id" +
                        "=c.academic_year_id and f.client_id='" + clientid_mst + "' and c.client_id='" + clientid_mst + "' and c.client_unit_id='" + unitid_mst + "' and g.feetype_id=" +
                        "c.fee_type_id and c.section_id=e.master_id and b.categorytype=h.master_id and h.master_type=" +
                        "'Edu_Category' and c.academic_year_id='" + Ac_Year_id + "')main_table where classname='1' group" +
                        " by sectionnme order by sectionnme";
                    break;
                case "000027":
                    //to be changed
                    res = "select concat('Section ',sectionnme)as classname,sum(fee_amount) as fee_amount from (select class_cat,cat_name ,academic_year ,classname,class_id ,sectionnme,section ,feetypename,feetype_id ,headname,feeHead_Id,client_id,client_unit_id,academic_year_id,RegNumber,due_dt,fee_amount,0 as Receipt_Date,0 as receipt_amount from(select main.*,cs.class as classname,cs.CategoryType as class_cat,msc.master_name as cat_name,ms.master_name as sectionnme from (select  fq.grp,fq.due_dt,fq.overdue_dt, cast.*,ac.From_Date,ac.academic_year,ac.To_Date, (case when sm.IsAmount = 1 THEN ((sm.Amount*cast.Fee_Amount)/100 )ELSE sm.Amount end) AS scholarship from(select tab.*,ud.RegNumber,ud.Caste_Category,ud.GENDER,ud.FIRST_NAME,ud.MIDDLE_NAME,ud.LAST_NAME,ud.f_firstname,ud.f_middlename,ud.f_lastname,ud.m_firstname,ud.m_middlename,ud.m_lastname from (select sd.vch_num as student_id, sd.reg_no,sd.section,sd.roll_number,sd.RollNo ,cl1.* from student_detail sd inner join (select  CONCAT( sf.Due_Month , ' / ' , sf.Due_Date) as Duedate ,cl.*  from (select school_fee_structure.*,school_frequency.FrequencyName,school_fee_type.FeeTypeName,school_fee_head.HeadName from school_fee_structure,school_frequency,school_fee_type,school_fee_head WHERE  school_frequency.Frequency_Id=school_fee_structure.Frequency_Id and school_fee_type.FeeType_Id=school_fee_structure.FeeType_Id and school_fee_structure.FeeHead_Id=school_fee_head.FeeHead_Id order by Frequency_Id) cl inner join (select * from school_frequency) sf on cl.frequency_id=sf.Frequency_Id) cl1 on sd.CLASS_APPLIED=cl1.class_id ) tab INNER join user_details ud on tab.reg_no=ud.RegNo ) cast left outer join scholarship_master sm on cast.class_id=sm.class_id and cast.Caste_Category=sm.Caste_category_id  and cast.Gender=sm.Gender and cast.feeHead_id=sm.Fee_HeadId INNER join add_academic_year ac ON cast.academic_year_id=ac.academic_year_id and ac.client_id='" + clientid_mst + "' and type='ACY' inner join (select tab.* ,date_add(due_dt, interval tab.overdue_date-due_date day ) as overdue_dt from (select m.frequency_id,m.frequencyname,m.From_Date,m.no_trans,m.months,m.due_month,m.due_date,m.overdue_date,@ndt:=(case when @grp<>m.frequencyname or   @ndt=0 then m.from_date + INTERVAL due_month MONTH + INTERVAL due_date-1 DAY else  INTERVAL months MONTH +@ndt end)   as due_dt, @rownum:= @rownum + 1 as row_number,@grp:=m.frequencyname AS grp from (select b.from_date,a.* from school_frequency a inner join (select From_Date from add_academic_year where academic_year_id='" + Ac_Year_id + "' and client_id='" + clientid_mst + "' and type ='ACY') b on 1=1 inner join (select 1 as num union select 2 as num union select 3 as num union select 4 as num union select 5 as num union select 6 as num union select 7 as num union select 8 as num union select 9 as num union select 10 as num union select 11 as num union select 12 as num) c on c.num<=a.no_trans inner join (SELECT @mnt:=0) e on 1=1  order by frequencyname) m inner join (SELECT @ndt:=0) d on 1=1 inner join (SELECT @grp:=0) g on 1=1   inner join (SELECT @rownum:= 0) r on 1=1 order by frequencyname) tab) fq on fq.frequency_id=cast.frequency_id order by fq.due_dt, fq.grp)main inner join school_fee_type ft on ft.FeeType_Id=main.feetype_id INNER join school_fee_head fh on main.feehead_id=fh.FeeHead_Id INNER join add_class cs on cs.add_class_id=main.class_id INNER JOIN master_setting ms on ms.master_id=main.section and ms.master_type='section' inner join master_setting msc on msc.master_id=cs.CategoryType)tbb UNION ALL select class_cat,cat_name ,academic_year ,classname,class_id ,sectionnme,section ,feetypename,feetype_id ,headname,feeHead_Id,client_id,client_unit_id,academic_year_id,RegNumber,Receipt_Date,0 as due_dt,receipt_amount,0 as fee_amount from (SELECT b.class as classname, c.Receipt_no,c.Receipt_Date,d.headname,f.academic_year,g.feetypename, c.receipt_amount,c.vch_date,e.master_name as sectionnme,c.class_id,c.section_id as section, c.frequency_id,c.fee_type_id as feetype_id,c.fee_head_id as feeHead_Id,f.academic_year_id,e.master_name,c.client_id,c.client_unit_id,c.regno as regnumber,b.categorytype as class_cat,h.master_name as cat_name from add_class b,student_receipt_detail c,school_fee_head d,master_setting e, add_academic_year f,school_fee_type g,master_setting h where b.add_class_id=c.class_id and c.Type='erd' and d.feehead_id=c.fee_head_id and f.academic_year_id=c.academic_year_id and c.client_id='" + clientid_mst + "' and c.client_unit_id='" + unitid_mst + "' and g.feetype_id=c.fee_type_id and c.section_id=e.master_id and b.categorytype=h.master_id and h.master_type='Edu_Category') tbb2)main_table where classname='1' GROUP by sectionnme order by sectionnme";
                    break;
                case "000028":
                    //to be chenged
                    res = "select headname,sum(Fee_amount) as fee_amount from(select main.*,'Pending' as feestatus,cs.CategoryType,cs.class as classname,cs.CategoryType as class_cat,msc.master_name as cat_name,ms.master_name as sectionnme from (select concat(to_char(fq.due_dt,'%Y%m'),'01') as feedate, concat('01/',to_char(fq.due_dt,'%m/%Y')) as Fee_Month,to_char(fq.due_dt,'%d/%m/%Y') as Due_Date,to_char(Last_Day(fq.due_dt) ,'%d/%m/%Y')as  HideDate,to_char(fq.due_dt,'%m') as Month_Id, date_format (fq.due_dt,'%M') as month_name, fq.grp, fq.due_dt,fq.overdue_dt, cast.*, ac.From_Date,ac.To_Date,ac.academic_year,cast.Fee_Amount as Receipt_Amount,'0.00' as PaidAmount,'Y' as IntialStatus,Round((nvl( (case when sm.IsAmount = 1 THEN ((sm.Amount*cast.Fee_Amount)/100 )ELSE sm.Amount end),0)) ,2)AS scholarship,sm.ScholarshipName,sm.vch_num as Scholarship_id from(select tab.*,ud.RegNumber,ud.Caste_Category,ud.GENDER,ud.FIRST_NAME, ud.MIDDLE_NAME,ud.LAST_NAME,ud.f_firstname,ud.f_middlename,ud.f_lastname,ud.m_firstname,ud.m_middlename,ud.m_lastname from (select sd.vch_num as student_id, sd.reg_no,sd.section,sd.roll_number,sd.RollNo ,cl1.* from student_detail sd inner join (select  CONCAT( sf.Due_Month , ' / ' , sf.Due_Date) as Duedate ,cl.*  from (select school_fee_structure.*,school_frequency.FrequencyName,school_fee_type.FeeTypeName,school_fee_head.HeadName from school_fee_structure,school_frequency,school_fee_type,school_fee_head WHERE  school_frequency.Frequency_Id=school_fee_structure.Frequency_Id and school_fee_type.FeeType_Id=school_fee_structure.FeeType_Id and school_fee_structure.FeeHead_Id=school_fee_head.FeeHead_Id order by Frequency_Id) cl inner join (select * from school_frequency) sf on cl.frequency_id=sf.Frequency_Id) cl1 on sd.CLASS_APPLIED=cl1.class_id ) tab INNER join user_details ud on tab.reg_no=ud.RegNo ) cast left outer join scholarship_master sm on cast.class_id=sm.class_id and cast.Caste_Category=sm.Caste_category_id  and cast.Gender=sm.Gender and cast.feeHead_id=sm.Fee_HeadId INNER join add_academic_year ac ON  cast.academic_year_id=ac.academic_year_id inner join (select tab.* ,date_add(due_dt, interval tab.overdue_date-due_date day ) as overdue_dt from (select m.frequency_id, m.frequencyname,m.From_Date,m.no_trans,m.months,m.due_month,m.due_date,m.overdue_date,@ndt:=(case when @grp<>m.frequencyname or   @ndt=0 then m.from_date + INTERVAL due_month MONTH + INTERVAL due_date-1 DAY else  INTERVAL months MONTH +@ndt end)   as due_dt, @rownum:= @rownum + 1 as row_number,@grp:=m.frequencyname AS grp from (select b.from_date,a.* from school_frequency a inner join (select From_Date from add_academic_year where academic_year_id='" + Ac_Year_id + "') b on 1=1 inner join (select 1 as num union select 2 as num union select 3 as num union select 4 as num union select 5 as num union select 6 as num union select 7 as num union select 8 as num union select 9 as num union select 10 as num union select 11 as num union select 12 as num) c on c.num<=a.no_trans inner join (SELECT @mnt:=0) e on 1=1  order by frequencyname) m inner join (SELECT @ndt:=0) d on 1=1 inner join (SELECT @grp:=0) g on 1=1   inner join (SELECT @rownum:= 0) r on 1=1 order by frequencyname) tab) fq on fq.frequency_id=cast.frequency_id order by fq.due_dt, fq.grp)main inner join school_fee_type ft on ft.FeeType_Id=main.feetype_id INNER join school_fee_head fh on main.feehead_id=fh.FeeHead_Id INNER join add_class cs on cs.add_class_id=main.class_id INNER JOIN master_setting ms on ms.master_id=main.section and ms.master_type='section' inner join master_setting msc on msc.master_id=cs.CategoryType)main_table where feetypename='Sports fees' group by headname order by headname";
                    break;
                case "000029":
                    res = "select headname,sum(Receipt_Amount) as Receipt_Amount from(SELECT b.class as classname,c.Receipt_no,c.Receipt_Date,d.headname,f.academic_year,g.feetypename,c.receipt_amount,c.vch_date,e.master_name as sectionnme,c.class_id,c.section_id as section, c.frequency_id,c.fee_type_id as feetype_id,c.fee_head_id as feeHead_Id,f.academic_year_id,b.categorytype as class_cat,h.master_name as cat_name from add_class b,student_receipt_detail c,school_fee_head d,master_setting e,add_academic_year f,school_fee_type g,master_setting h where b.add_class_id=c.class_id and c.Type='erd' and d.feehead_id=c.fee_head_id and f.academic_year_id=c.academic_year_id and c.client_id='" + clientid_mst + "' and c.client_unit_id='" + unitid_mst + "' and g.feetype_id=c.fee_type_id and c.section_id=e.master_id and b.categorytype=h.master_id and h.master_type='Edu_Category' and c.academic_year_id='" + Ac_Year_id + "')main_table where feetypename='Academic Fees' group by headname order by headname";
                    break;
                case "000030":
                    //to be changed
                    res = "select headname,sum(fee_amount) as fee_amount from (select class_cat,cat_name ,academic_year ,classname,class_id ,sectionnme,section ,feetypename,feetype_id ,headname,feeHead_Id,client_id,client_unit_id,academic_year_id,RegNumber,due_dt,fee_amount,0 as Receipt_Date,0 as receipt_amount from(select main.*,cs.class as classname,cs.CategoryType as class_cat,msc.master_name as cat_name,ms.master_name as sectionnme from (select  fq.grp,fq.due_dt,fq.overdue_dt, cast.*,ac.From_Date,ac.academic_year,ac.To_Date, (case when sm.IsAmount = 1 THEN ((sm.Amount*cast.Fee_Amount)/100 )ELSE sm.Amount end) AS scholarship from(select tab.*,ud.RegNumber,ud.Caste_Category,ud.GENDER,ud.FIRST_NAME,ud.MIDDLE_NAME,ud.LAST_NAME,ud.f_firstname,ud.f_middlename,ud.f_lastname,ud.m_firstname,ud.m_middlename,ud.m_lastname from (select sd.vch_num as student_id, sd.reg_no,sd.section,sd.roll_number,sd.RollNo ,cl1.* from student_detail sd inner join (select  CONCAT( sf.Due_Month , ' / ' , sf.Due_Date) as Duedate ,cl.*  from (select school_fee_structure.*,school_frequency.FrequencyName,school_fee_type.FeeTypeName,school_fee_head.HeadName from school_fee_structure,school_frequency,school_fee_type,school_fee_head WHERE  school_frequency.Frequency_Id=school_fee_structure.Frequency_Id and school_fee_type.FeeType_Id=school_fee_structure.FeeType_Id and school_fee_structure.FeeHead_Id=school_fee_head.FeeHead_Id order by Frequency_Id) cl inner join (select * from school_frequency) sf on cl.frequency_id=sf.Frequency_Id) cl1 on sd.CLASS_APPLIED=cl1.class_id ) tab INNER join user_details ud on tab.reg_no=ud.RegNo ) cast left outer join scholarship_master sm on cast.class_id=sm.class_id and cast.Caste_Category=sm.Caste_category_id  and cast.Gender=sm.Gender and cast.feeHead_id=sm.Fee_HeadId INNER join add_academic_year ac ON cast.academic_year_id=ac.academic_year_id and ac.client_id='" + clientid_mst + "' and type='ACY' inner join (select tab.* ,date_add(due_dt, interval tab.overdue_date-due_date day ) as overdue_dt from (select m.frequency_id,m.frequencyname,m.From_Date,m.no_trans,m.months,m.due_month,m.due_date,m.overdue_date,@ndt:=(case when @grp<>m.frequencyname or   @ndt=0 then m.from_date + INTERVAL due_month MONTH + INTERVAL due_date-1 DAY else  INTERVAL months MONTH +@ndt end)   as due_dt, @rownum:= @rownum + 1 as row_number,@grp:=m.frequencyname AS grp from (select b.from_date,a.* from school_frequency a inner join (select From_Date from add_academic_year where academic_year_id='" + Ac_Year_id + "' and client_id='" + clientid_mst + "' and type='ACY') b on 1=1 inner join (select 1 as num union select 2 as num union select 3 as num union select 4 as num union select 5 as num union select 6 as num union select 7 as num union select 8 as num union select 9 as num union select 10 as num union select 11 as num union select 12 as num) c on c.num<=a.no_trans inner join (SELECT @mnt:=0) e on 1=1  order by frequencyname) m inner join (SELECT @ndt:=0) d on 1=1 inner join (SELECT @grp:=0) g on 1=1   inner join (SELECT @rownum:= 0) r on 1=1 order by frequencyname) tab) fq on fq.frequency_id=cast.frequency_id order by fq.due_dt, fq.grp)main inner join school_fee_type ft on ft.FeeType_Id=main.feetype_id INNER join school_fee_head fh on main.feehead_id=fh.FeeHead_Id INNER join add_class cs on cs.add_class_id=main.class_id INNER JOIN master_setting ms on ms.master_id=main.section and ms.master_type='section' inner join master_setting msc on msc.master_id=cs.CategoryType)tbb UNION ALL select class_cat,cat_name ,academic_year ,classname,class_id ,sectionnme,section ,feetypename,feetype_id ,headname,feeHead_Id,client_id,client_unit_id,academic_year_id,RegNumber,Receipt_Date,0 as due_dt,receipt_amount,0 as fee_amount from (SELECT b.class as classname, c.Receipt_no,c.Receipt_Date,d.headname,f.academic_year,g.feetypename, c.receipt_amount,c.vch_date,e.master_name as sectionnme,c.class_id,c.section_id as section, c.frequency_id,c.fee_type_id as feetype_id,c.fee_head_id as feeHead_Id,f.academic_year_id,e.master_name,c.client_id,c.client_unit_id,c.regno as regnumber,b.categorytype as class_cat,h.master_name as cat_name from add_class b,student_receipt_detail c,school_fee_head d,master_setting e, add_academic_year f,school_fee_type g,master_setting h where b.add_class_id=c.class_id and c.Type='erd' and d.feehead_id=c.fee_head_id and f.academic_year_id=c.academic_year_id and c.client_id='" + clientid_mst + "' and c.client_unit_id='" + unitid_mst + "' and g.feetype_id=c.fee_type_id and c.section_id=e.master_id and b.categorytype=h.master_id and h.master_type='Edu_Category') tbb2)main_table where feetypename='Academic Fees' GROUP by headname order by headname";
                    break;
                case "000031":
                    // emp dept wise (Payroll)

                    //                res = "select mdp.master_name,COUNT(em.first_name) as Count from emp_master em left outer JOIN master_setting mdp on em.emp_dept=mdp.master_id " +
                    //"and mdp.type='MDP' where em.type='EMP' and em.client_id='" + clientid_mst + "' and em.client_unit_id='" + unitid_mst + "' group by em.emp_dept";

                    res = "select department,count(vch_num) employees from (select emp.vch_num, dp.master_name as department " +
                        "from emp_master emp inner join master_setting dp on dp.master_id = emp.emp_dept and find_in_set(dp.client_id, emp.client_id) = 1 " +
                        "and find_in_set(dp.client_unit_id, emp.client_unit_id) = 1 and dp.type = 'MDP'" +
                        " WHERE emp.client_id = '" + clientid_mst + "' and emp.client_unit_id = '" + unitid_mst + "' and emp.type = 'EMP' and emp.emp_status != 'R' and" +
                        " (case when to_char(emp.ex_dt, 'yyyy') = '1900' then 1 when emp.ex_dt > to_date('" + curdate + "', 'dd/MM/yyyy') then 1 else 0 end)= 1 )" +
                        " group by department";

                    break;
                case "000032":
                    //emp desig wise (Payroll)
                    res = "select designation,count(vch_num) employees from (select emp.vch_num,dg.master_name as designation from emp_master emp inner" +
                        " join master_setting dg on dg.master_id = emp.emp_desig and find_in_set(dg.client_id, emp.client_id) = 1" +
                        " and find_in_set(dg.client_unit_id, emp.client_unit_id)= 1 and dg.type = 'MDG' WHERE emp.client_id = '" + clientid_mst + "' and emp.client_unit_id =" +
                        " '" + unitid_mst + "' and emp.type = 'EMP' and emp.emp_status != 'R' and(case when to_char(emp.ex_dt, 'yyyy') = '1900' then 1 " +
                        "when emp.ex_dt > to_date('" + curdate + "', 'dd/MM/yyyy') then 1 else 0 end)= 1) group by designation";
                    //res = "select mdg.master_name,COUNT(em.first_name) as Count from emp_master em left outer JOIN master_setting mdg on em.emp_dept=mdg.master_id " +
                    //    "and mdg.type='MDG' where em.type='EMP' and em.client_id='" + clientid_mst + "' and em.client_unit_id='" + unitid_mst + "' group by em.emp_desig";
                    break;
                case "000033":
                    //emp grade wise (Payroll)
                    //res = "select msg.master_name grade,count(emp.first_name) count from emp_master emp inner join master_setting msdp on " +
                    //    "emp.emp_dept = msdp.master_id and msdp.type = 'MDP' inner join master_setting msdg on emp.emp_desig = msdg.master_id and msdg.type = 'MDG' " +
                    //    "INNER join enx_tab en on en.col1 = emp.emp_dept and en.col3 = emp.emp_desig and en.type = 'KSG' and en.client_id = '" + clientid_mst + "' and " +
                    //    "en.client_unit_id = '" + unitid_mst + "' INNER join master_setting msg on msg.master_id = en.col2 and msg.type = 'KGM' and msg.client_id = '" + clientid_mst + "' " +
                    //    "and msg.client_unit_id = '" + unitid_mst + "' where emp.client_id = '" + clientid_mst + "' and emp.client_unit_id = '" + unitid_mst + "' and emp.type = 'EMP' group by en.col2";
                    res = "select sg.master_name grade,count(emp.first_name) count from emp_master emp INNER join enx_tab en on en.col1 = emp.emp_dept and en.col3 = emp.emp_desig" +
                        " and en.type = 'KSG' and en.client_id = '" + clientid_mst + "' and en.client_unit_id = '" + unitid_mst + "' INNER join master_setting sg on sg.master_id = en.col2 and sg.type = 'KGM'" +
                        " and sg.client_id = '" + clientid_mst + "' and sg.client_unit_id = '" + unitid_mst + "' where emp.client_id = '" + clientid_mst + "' and emp.client_unit_id = '" + unitid_mst + "' and emp.type = 'EMP' and " +
                        "emp.emp_status != 'R' and(case when to_char(emp.ex_dt, 'yyyy') = '1900' then 1 when emp.ex_dt > to_date('" + curdate + "', 'dd/MM/yyyy') then 1 else 0 end)= 1" +
                        " group by en.col2,sg.master_name";

                    break;
                //no. of users in group(Training)
                case "000034":
                    res = "select gd.Group_Name,count(ug.user_id) as User_Count from group_details gd LEFT OUTER join usergroup ug on " +
                        "ug.group_id = gd.group_id and ug.type = 'TGP' and ug.status = 'assigned' and ug.client_id = gd.client_id where gd.client_id = '" + clientid_mst + "' " +
                        "and gd.type = 'TGP' GROUP by gd.GROUP_ID,gd.group_name";
                    break;
                //no. of users in dept (Training) - Online
                case "000035":
                    res = " select ms.master_name as Department,count(ud.user_id) as User_in_Dept from master_setting ms left outer join " +
                        "user_details ud on ud.department = ms.master_id and ud.type = 'CPR' and ud.status = 'active' and find_in_set('" + clientid_mst + "',ud.client_id) " +
                        "and find_in_set('1000',ud.mod_id)  WHERE ms.type = 'MDP'  group by ms.master_id";
                    break;
                //no. of users in desig (Training) - Online
                case "000036":
                    res = "select ms.master_name as Designation,count(ud.user_id) as User_in_Dept from master_setting ms left outer join " +
                        "user_details ud on ud.designation = ms.master_id and ud.type = 'CPR' and ud.status = 'active' and find_in_set('" + clientid_mst + "',ud.client_id) " +
                        "and find_in_set('1000',ud.mod_id)  WHERE ms.type = 'MDG'  group by ms.master_id";
                    break;
                case "000037":
                    //res = "select mg,sum(stock) as Qty_stock from (select lc.icode, mg.master_name mg, mg.master_id as mg_code,i.iname,nvl(s.closing, '0') as stock," +
                    //    "nvl(lc.qtyin, 0) qtyin, nvl(lc.qtyout, 0) qtyout,(case when lc.pono = '0' then 'C' else lc.pono end) store,nvl(lc.irate, 0) irate,lc.type," +
                    //    "lc.client_id,lc.client_unit_id from btchtrans lc inner join item i on i.icode = lc.icode and i.type = 'IT' and i.client_id = lc.client_id and" +
                    //    " find_in_set(lc.client_unit_id, i.client_unit_id) = 1 inner join master_setting mg on mg.master_id = substr(lc.icode, 1, 3) and mg.type = 'KIG' " +
                    //    "and mg.client_id = lc.client_id and mg.client_unit_id = lc.client_unit_id inner join(select t.client_unit_id, t.icode, sum(nvl(t.qtyin,0)) as " +
                    //    "Received,sum(nvl(t.qtyout, 0)) as Issued,  sum(nvl(t.qtyin, 0)) - sum(nvl(t.qtyout, 0)) as closing from itransaction t where trim(t.store) = 'Y' " +
                    //    "  group by t.icode,t.client_unit_id ) s on i.icode = s.icode and find_in_set(s.client_unit_id, i.client_unit_id)= 1 and s.closing > 0 " +
                    //    " where length(lc.icode)> 5 and lc.store = 'Y' and lc.client_id = '001' and lc.client_unit_id = '001001') a group by mg";


                    //res = "select mg,sum(to_number(iamount)) iamount from (select it.icode, mg.master_name mg, mg.master_id as mg_code," +
                    //    " sg.iname sg, sg.icode as sg_code, i.iname,um.master_name uom, nvl(it.qtyin, 0) qtyin, nvl(it.qtyout, 0) qtyout, it.store," +
                    //    " nvl(it.irate, 0) irate, nvl(it.iamount, 0) iamount, it.type, it.client_id,it.client_unit_id from itransaction it inner " +
                    //    "join item i on i.icode = it.icode and i.type = 'IT' and i.client_id = it.client_id and find_in_set(it.client_unit_id, i.client_unit_id) = 1" +
                    //    " inner join master_setting um on um.master_id = i.uom and um.type = 'UMM' and find_in_set(um.client_id, i.client_id) = 1 and" +
                    //    " find_in_set(um.client_unit_id, i.client_unit_id) = 1 inner join master_setting mg on mg.master_id = substr(it.icode, 1, 3) " +
                    //    "and mg.type = 'KIG' and mg.client_id = it.client_id and mg.client_unit_id = it.client_unit_id inner join item sg on sg.icode " +
                    //    "= substr(it.icode, 1, 5) and sg.type = 'SG' and sg.client_id = it.client_id and find_in_set(sg.client_unit_id, it.client_unit_id)=1" +
                    //    " where length(it.icode) > 5 and it.store = 'Y' and it.client_id = '" + clientid_mst + "' and it.client_unit_id = '" + unitid_mst + "')" +
                    //    " a group by mg";


                    res = "select mg,sum(to_number(iamount)) iamount " +
                        "from (select it.icode, mg.master_name mg, mg.master_id as mg_code, nvl(it.qtyin, 0) qtyin, nvl(it.qtyout, 0) qtyout, it.store," +
                       " nvl(it.irate, 0) irate, nvl(it.iamount, 0) iamount, it.type, it.client_unit_id from itransaction it " +
                       "inner join master_setting mg on mg.master_id = substr(it.icode, 1, 3) and mg.type = 'KIG' and mg.client_unit_id = it.client_unit_id " +
                       " where length(it.icode) > 5 and it.store = 'Y' and it.client_unit_id = '" + unitid_mst + "') a group by mg";

                    break;

                case "000039":
                    //billing prty wise sale
                    res = "select cl.c_name as costomer,iv.netamt from clients_mst cl inner join(select acode, client_id, client_unit_id, max(netamt) as netamt" +
                        " from itransaction where substr(type, 1, 1) = '4' and substr(potype,1,1)= '5' and rownum <= '10' and to_date(vch_date) " +
                    "between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "') " +
                    "group by acode,client_id,client_unit_id) iv on iv.acode = cl.vch_num and cl.client_id = iv.client_id  and cl.client_unit_id = iv.client_unit_id" +
                        " where cl.type = 'BCD' and substr(cl.vch_num,0,3)= '303' and cl.client_id = '" + clientid_mst + "' and cl.client_unit_id = '" + unitid_mst + "'";
                    break;

                case "000040":
                    //billing item wise sale
                    res = "select it.iname,iv.netamt from item it inner join(select icode, client_id, client_unit_id, max(netamt) as netamt from " +
                        "itransaction where substr(type, 1, 1) = '4' and substr(potype,1,1)= '5' and rownum <= '10' and to_date(vch_date) " +
                    "between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "') " +
                        "group by icode,client_unit_id,client_id) iv on iv.icode = it.icode and find_in_set(it.client_id, iv.client_id) = 1 where it.type = 'IT' " +
                        "and it.sale = 'Y' and find_in_set(it.client_id,'" + clientid_mst + "')= 1 and find_in_set(it.client_unit_id,'" + unitid_mst + "')= 1";
                    break;
                case "000041":
                    //(crm) lead_owner wise
                    res = "select lead_owner,count(lead_no) leads from (select a.vch_num as lead_no,ud.first_name || ' ' || replace(ud.middle_name, '0', '') || '' || replace(ud.last_name, '0', '') as lead_owner " +
                        //"from lead_master a inner join user_details ud on ud.rec_id = a.ld_owner where a.type = 'LED' and a.client_unit_id = '" + unitid_mst + "'  and a.lead_con = 'N') group by lead_owner";
                        "from lead_master a inner join user_details ud on ud.vch_num = a.ld_owner where a.type = 'LED' and a.client_unit_id = '" + unitid_mst + "'  and a.lead_con = 'N') group by lead_owner";
                    break;
                case "000042":
                    //(crm) Lead_source
                    res = "select Lead_source,count(lead_no) leads from (select a.vch_num as lead_no,c.master_name as Lead_source from lead_master a inner join master_setting c on " +
                        "a.ld_source = c.master_id and c.type = 'SRC' and find_in_set(c.client_unit_id, a.client_unit_id)= 1 where a.type = 'LED' and a.client_unit_id = '" + unitid_mst + "'  and a.lead_con = 'N') group by Lead_source ";
                    break;
                case "000043":
                    //(crm) lead_status
                    res = "select lead_status,count(lead_no) leads from (select a.vch_num as lead_no,d.master_name as lead_status from lead_master a inner join master_setting d on a.ld_source = d.master_id and d.type = 'LST' and find_in_set(d.client_unit_id, a.client_unit_id)= 1 where a.type = 'LED' " +
                        "and a.client_unit_id = '" + unitid_mst + "'  and a.lead_con = 'N') group by lead_status";
                    break;
                case "000044":
                    //lead product wise(crm)
                    res = "select product_name,count(lead_no) leads from (select a.vch_num as lead_no,d.master_name as " +
                        "product_name from lead_master a inner join master_setting d on find_in_set(a.ld_product, d.master_id)=1 and d.type = 'PNM' and " +
                        "find_in_set(d.client_unit_id, a.client_unit_id)= 1 where a.type = 'LED' and " +
                        "a.client_unit_id = '" + unitid_mst + "'  and a.lead_con='N') group by product_name";
                    break;
                case "000045":
                    //lead assign to(crm)
                    res = "select assign_to,count(lead_no) leads from (select a.vch_num as lead_no,d.first_name||' '||replace(d.middle_name,'0','')" +
                        //"||''||replace(d.last_name,'0','') as assign_to from lead_master a inner join user_details d on a.ct_person = lpad(d.rec_id, 6, 0)" +
                        "||''||replace(d.last_name,'0','') as assign_to from lead_master a inner join user_details d on a.ct_person = d.vch_num" +
                        " and d.type = 'CPR' where a.type = 'LED' and a.client_unit_id = '" + unitid_mst + "'  and a.lead_con = 'N') group by assign_to";
                    break;
                case "000046":
                    //lead business type(crm)
                    res = "select business_type,count(lead_no) leads from (select a.vch_num as lead_no,d.master_name as " +
                        "business_type from lead_master a inner join master_setting d on find_in_set(a.bsn_type, d.master_id)=1 and d.type = 'CMN' and " +
                        "find_in_set(d.client_unit_id, a.client_unit_id)= 1 where a.type = 'LED' and a.client_unit_id = '" + unitid_mst + "'  and a.lead_con='N') group by business_type";
                    break;
                case "000047":
                    //lead account type(crm)
                    res = "select account_type,count(lead_no) leads from (select a.vch_num as lead_no,d.master_name as account_type " +
                        "from lead_master a inner join master_setting d on a.client_type = d.master_id and d.type = 'CLI' and " +
                        "find_in_set(d.client_unit_id, a.client_unit_id)= 1 where a.type = 'LED' and a.client_unit_id = '" + unitid_mst + "'  and a.lead_con = 'N') group by account_type";
                    break;
                case "000048":
                    //lead business type(crm)
                    res = "select location,count(lead_no) leads from (select a.vch_num as lead_no,a.ploc as location " +
                        "from lead_master a where a.type = 'LED' and a.client_unit_id = '" + unitid_mst + "'  and a.lead_con = 'N') group by location";
                    break;
                case "000049":
                    //party location(crm)
                    res = "select (case when cl.ploc='001' then 'Domestic' when cl.ploc='002' then 'Overseas' else 'Not Mentioned' end) party_location," +
                        "count(cl.vch_num) as doc from clients_mst cl where cl.type = 'BCD' and substr(vch_num,0,3)= '303' and " +
                        "cl.client_unit_id = '" + unitid_mst + "' group by(case when cl.ploc = '001' then 'Domestic' when cl.ploc = '002' then" +
                        " 'Overseas' else 'Not Mentioned' end)";
                    break;
                case "000050":
                    //Type of account(crm)
                    res = "select ct.master_name as type_of_accounts,count(cl.vch_num) as doc from clients_mst cl inner join master_setting ct on " +
                        "ct.master_id = cl.client_type and ct.type='CLI' and find_in_set(ct.client_unit_id,'" + unitid_mst + "')=1 where" +
                        " cl.type='BCD' and substr(cl.vch_num,0,3)='303' and cl.client_unit_id='" + unitid_mst + "' group by ct.master_name";
                    break;
                case "000051":
                    //business type (crm)
                    res = "select ct.master_name as business_type,count(cl.vch_num) as doc from clients_mst cl inner join " +
                        "master_setting ct on ct.master_id = cl.bsn_type and ct.type='CMN' and find_in_set(ct.client_unit_id,'" + unitid_mst + "')=1 where " +
                        "cl.type='BCD' and substr(cl.vch_num,0,3)='303' and cl.client_unit_id='" + unitid_mst + "' group by ct.master_name";
                    break;
                case "000052":
                    //gender wise employee (payroll)
                    res = "select emp.gender,count(emp.vch_num) as Gender_ratio from emp_master emp where emp.type='EMP' and " +
                        "emp.client_unit_id='" + unitid_mst + "' and to_char(emp.ex_dt,'yyyy')= '1900' and emp.emp_status = 'Y'" +
                        " group by emp.gender";
                    break;
                case "000053":
                    //age wise employee (payroll)
                    res = "select (to_number(to_char(utc_timestamp(), 'yyyy')) - to_number(to_char(emp.dob, 'yyyy'))) as age,count(emp.vch_num) as Gender_ratio from emp_master emp where emp.type='EMP' and emp.client_unit_id='" + unitid_mst + "' and to_char(emp.ex_dt,'yyyy')= '1900' and emp.emp_status = 'Y' group by(to_number(to_char(utc_timestamp(), 'yyyy')) - to_number(to_char(emp.dob, 'yyyy')))";
                    break;
                case "000054":
                    //Dept wise pending indents
                    res = "select dpt.master_name department,count(b.vch_num) indents from master_setting dpt " +
                        "inner join(select ind.vch_num,ind.deptno,ind.client_id,ind.client_unit_id from purchases ind where " +
                        "ind.type = '66' and ind.client_unit_id = '" + unitid_mst + "' and ind.client_unit_id || ind.vch_num not in " +
                        "(select client_unit_id || indno from purchases where type in ('50', '52', '54') and client_unit_id = '" + unitid_mst + "' " +
                        "and pur_type = '001')) b on b.deptno = dpt.master_id and find_in_set(b.client_unit_id, dpt.client_unit_id)= 1 " +
                        "where dpt.type = 'MDP' and find_in_set(dpt.client_unit_id, '" + unitid_mst + "') = 1 group by dpt.master_name";
                    break;
                case "000055":
                    //pending po party wise
                    res = "select cl.c_name as party_name,count(b.vch_num) as po_numbers from clients_mst cl inner join(select acode, " +
                        "vch_num, client_unit_id from purchases where type in ('50','52','54') and client_unit_id = '" + unitid_mst + "' and " +
                        "client_unit_id|| vch_num not in (select client_unit_id || pono from itransaction where type in ('02') and " +
                        "client_unit_id = '" + unitid_mst + "')) b on b.acode = cl.vch_num and find_in_set(cl.client_unit_id, b.client_unit_id)= 1 " +
                        "and substr(cl.vch_num,0,3)= '203' group by cl.c_name";
                    break;
                case "000056":
                    //item wise pending indents
                    res = "select it.iname,count(b.vch_num) indents from item it inner join(select ind.vch_num,ind.client_id," +
                        "ind.client_unit_id,ind.icode from purchases ind where ind.type = '66' and ind.client_unit_id = '" + unitid_mst + "' " +
                        "and ind.client_unit_id || ind.vch_num not in (select client_unit_id || indno from purchases where type in " +
                        "('50', '52', '54') and client_unit_id = '" + unitid_mst + "' and pur_type='001')) b on b.icode = it.icode and " +
                        "find_in_set_one(b.client_unit_id, it.client_unit_id)= 1 where it.type = 'IT' and find_in_set_one('" + unitid_mst + "',it.client_unit_id)= 1" +
                        " group by it.iname";
                    break;
            }




            //res = "select month_name,row_number from (select concat(to_char(clf.due_dt,'%Y%m'),'01') as feedate, concat('01/', to_char(clf.due_dt, '%m/%Y')) as Fee_Month,to_char(clf.due_dt, '%d/%m/%Y') as Due_Date1," +
            //    "to_char(Last_Day(clf.due_dt), '%d/%m/%Y') as HideDate,to_char(clf.due_dt, '%m') as Month_Id, to_char(clf.due_dt, '%M') as month_name, sd.reg_no, sd.class_applied," +
            //    " sd.section, sd.vch_num as student_id, sd.roll_number, sd.RollNo ,sd.havetransport,sd.pick_point,sd.drop_point,clf.*,ud.RegNumber, ud.Caste_Category, ud.GENDER, ud.FIRST_NAME, " +
            //    "ud.MIDDLE_NAME, ud.LAST_NAME, ud.f_firstname, ud.f_middlename, ud.f_lastname, ud.m_firstname, ud.m_middlename, ud.m_lastname,'0.00' as Receipt_Amount,'0.00' as PaidAmount," +
            //    "'Y' as IntialStatus,Round((nvl((case when sm.IsAmount = 1 THEN((sm.Amount * clf.Fee_Amount) / 100)ELSE sm.Amount end), 0)) ,2)AS scholarship, sm.ScholarshipName," +
            //    "sm.vch_num as Scholarship_id,'Pending' as feestatus, st.FeeTypeName, ms.master_name as HeadName,cs.CategoryType,cs.class as classname,cs.CategoryType as class_cat," +
            //    "msc.master_name as cat_name,ms1.master_name as sectionnme from student_detail sd inner join user_details ud on sd.reg_no=ud.regnumber and ud.type= 'STD' inner join " +
            //    "(select * from Fees_detail where client_id='004' and client_unit_id='004001' and academic_year_id='005' ) clf on sd.academic_year_id=clf.academic_year_id and " +
            //    "clf.client_id=sd.client_id and sd.CLASS_APPLIED=clf.class_id and clf.client_unit_id=sd.client_unit_id left outer join  scholarship_master sm on clf.class_id = sm.class_id and" +
            //    " ud.Caste_Category = sm.Caste_category_id  and ud.Gender = sm.Gender and clf.feeHead_id = sm.Fee_HeadId and clf.client_id = sm.client_id and clf.client_unit_id and sm.client_unit_id " +
            //    "and sm.academic_year_id = clf.academic_year_id  inner join school_fee_type st on st.FeeType_Id = clf.FeeType_Id  and st.client_id= clf.client_id and" +
            //    " st.client_unit_id= clf.client_unit_id inner join master_setting ms  on clf.FeeHead_Id = ms.master_id and ms.type = 'EFH' and ms.client_id= clf.client_id and " +
            //    "clf.client_unit_id= ms.client_unit_id INNER join add_class cs on cs.add_class_id = clf.class_id and clf.client_id = cs.client_id and clf.client_unit_id and cs.client_unit_id " +
            //    "INNER JOIN master_setting ms1 on ms1.master_id = sd.section and ms1.type = 'ECS' and clf.client_id = ms1.client_id and clf.client_unit_id = ms1.client_unit_id inner " +
            //    "join master_setting msc on msc.master_id = cs.CategoryType  and msc.client_id = cs.client_id and msc.client_unit_id = cs.client_unit_id and msc.type = 'ECC' where" +
            //    " RegNumber='Aggarwal P/Blb/004001/2018 2019/1' order by due_dt) a ";
            return res;
        }

        #endregion

        #region
        public class Automobile
        {
            [Display(Name = "Makes")]
            public string Makes { get; set; }
        }
        public class SelectItem
        {
            public int id { get; set; }
            public string text { get; set; }
        }
        public static class DAL
        {

            public static List<T> CreateListFromTable<T>(DataTable tbl) where T : new()
            {
                // define return list
                List<T> lst = new List<T>();

                // go through each row
                foreach (DataRow r in tbl.Rows)
                {
                    // add to the list
                    lst.Add(CreateItemFromRow<T>(r));
                }

                // return the list
                return lst;
            }
            // function that creates an object from the given data row
            public static T CreateItemFromRow<T>(DataRow row) where T : new()
            {
                // create a new object
                T item = new T();

                // set the item
                SetItemFromRow(item, row);

                // return 
                return item;
            }

            public static void SetItemFromRow<T>(T item, DataRow row) where T : new()
            {
                // go through each column
                foreach (DataColumn c in row.Table.Columns)
                {
                    // find the property for the column
                    PropertyInfo p = item.GetType().GetProperty(c.ColumnName);

                    // if exists, set the value
                    if (p != null && row[c] != DBNull.Value)
                    {
                        p.SetValue(item, row[c], null);
                    }
                }
            }

        }
        #endregion

        //================ frm config===============

        #region frm config

        public ActionResult Frm_Config()
        {
            if (userCode.Equals("")) Response.Redirect("~/home/login");
            ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.vsave = "disabled='disabled'";
            Tmodelmain tmodel = new Tmodelmain();
            List<Tmodelmain> cards = new List<Tmodelmain>
            {
                //new Tmodelmain{ Col1="",Col2="",TList1=GetAllTeaTypes(),Col18="Ram Rattan",Col15="1"},
                //new Tmodelmain{ Col1="",Col2="",TList1=GetAllTeaTypes(),Col18="Ram Rattan",Col15="2"},
                //new Tmodelmain{ Col1="",Col2="",TList1=GetAllTeaTypes(),Col18="Ram Rattan",Col15="3"}

            };

            return View(cards);

        }
        [HttpPost]
        public ActionResult Frm_Config(List<Tmodelmain> model, string command, HttpPostedFileBase postedFile, string mytxt, string mytxt1)
        {
            try
            {

                userCode = sgen.GetCookie(MyGuid, "userCode");
                if (command == "New")
                {
                    sgen.SetSession(MyGuid, "EDMODE", "N");
                    model = new List<Tmodelmain>();
                    model = new List<Tmodelmain>
            {
                new Tmodelmain{ Col1="",Col2="",SelectedItems1=new string[]{ "01","00" }

                }

            };
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                }
                else if (command == "Edit")
                {
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";

                }

                else if (command == "Save")
                {

                    if (postedFile != null)
                    {

                        if (sgen.GetSession(MyGuid, "EDMODE") != null)
                        {
                            if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                            {
                                isedit = true;
                            }
                            else
                            {
                                isedit = false;
                            }
                        }
                        else
                        {


                        }
                        #region save
                        var tmodel = model.FirstOrDefault();
                        cg_com_name = sgen.GetCookie(MyGuid, "cg_com_name");
                        string serverpath = Server.MapPath("~/Uploads/" + cg_com_name.Replace(" ", "") + "/");
                        string currdate = sgen.server_datetime_dt(userCode).ToString("yyyy-MM-dd HH:mm:ss");
                        string where = " and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
                        string cou_cont_id = "select max(to_number(cou_content_id)) as auto_genid from course_content where type='TCR'" + where + "";
                        cou_cont_id = sgen.genNo(userCode, cou_cont_id, 6, "auto_genid");
                        string vch_num = "select max(to_number(vch_num)) as auto_genid from course_content where type='TCR' " + where + "";
                        vch_num = sgen.genNo(userCode, vch_num, 6, "auto_genid");
                        string fileName2 = "";
                        string path2 = "";
                        string encpath2 = "";
                        string finalpath2 = "";
                        if (postedFile != null)
                        {
                            var file = postedFile;
                            fileName2 = file.FileName;
                            path2 = clientid_mst + unitid_mst + userid_mst + currdate + "_" + fileName2;
                            encpath2 = sgen.Convert_Stringto64(path2).ToString();
                            finalpath2 = serverpath + encpath2;
                            file.SaveAs(finalpath2);
                        }

                        //bool data = sgen.execute_cmd(userCode, "insert into course_content (cou_content_id,course_id,mod_id,cat_id,cou_title,ref_code,file_name,file_path,cou_content_ent_by," +
                        //               "cou_content_ent_dt,trn_duration,client_id,client_unit_id,training_level,type,vch_num)values('" + cou_cont_id + "','" + tmodel.Col5
                        //               + "','" + tmodel.Col1 + "','" + tmodel.Col3 + "','" + tmodel.Col5 + "','"
                        //               + tmodel.Col7 + "','" + fileName2 + "','" + encpath2 + "','" + userid_mst + "'," +
                        //               "'" + currdate + "','" + tmodel.Col8 + "','" + clientid_mst + "','" + unitid_mst + "','" + tmodel.Col9 + "','TCR','" + vch_num + "')");




                        DataTable dtagnt = new DataTable();
                        dtagnt = sgen.getdata(userCode, "select * from course_content where 1=2");
                        DataRow dr = dtagnt.NewRow();
                        dr["type"] = "TCR";
                        dr["vch_num"] = vch_num.Trim();
                        dr["cou_content_id"] = cou_cont_id.Trim();
                        dr["course_id"] = tmodel.Col5;
                        dr["mod_id"] = tmodel.Col1;
                        dr["cat_id"] = tmodel.Col3;
                        dr["cou_title"] = tmodel.Col5;
                        dr["ref_code"] = tmodel.Col7;
                        dr["file_name"] = fileName2;
                        dr["file_path"] = encpath2;
                        dr["trn_duration"] = tmodel.Col8;
                        dr["client_id"] = clientid_mst;
                        dr["client_unit_id"] = unitid_mst;
                        dr["training_level"] = tmodel.SelectedItems1.FirstOrDefault().ToString();
                        if (isedit)
                        {
                            dr["rec_id"] = tmodel.Col20;
                            dr["cou_content_ent_by"] = tmodel.Col19;
                        }
                        else
                        {
                            dr["rec_id"] = "0";
                            dr["cou_content_ent_by"] = userid_mst;

                        }
                        dtagnt.Rows.Add(dr);
                        sgen.Update_data(userCode, dtagnt, "course_content", tmodel.Col18, isedit);
                        #endregion
                        ViewBag.scripCall = "showmsgJS(3, 'Record Saved Successfully', 1);";
                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        model = new List<Tmodelmain>();
                        model = new List<Tmodelmain>
                {
                    new Tmodelmain{ Col1="",Col2=""}

                };
                    }
                    else
                    {
                        ViewBag.scripCall = "showmsgJS(3, 'Record Saved Successfully', 1);";
                    }


                }
                else if (command == "Cancel")
                {
                    return CancelFun(model);
                }
                else if (command == "Callback")
                {

                    if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
                    model = CallbackFun(btnval, "N", "upload_training", "HOME", model);
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                }

                var tmodel1 = model.FirstOrDefault();
                //tmodel1.TList1 = GetAllTeaTypes();
                if (tmodel1.SelectedItems1 != null && tmodel1.SelectedItems1.Length > 0 && tmodel1.TList1.Count() > 0)
                {

                    foreach (SelectListItem item in tmodel1.TList1)
                    {

                        if (tmodel1.SelectedItems1.Contains(item.Value))
                        {
                            item.Selected = true;
                        }
                    }

                    //List<SelectListItem> selectedItems = tmodel.TList.Where(p => tmodel.SelectedItems1.Contains(p.Value)).ToList();
                    //foreach (var T in selectedItems)
                    //{
                    //    T.Selected = true;
                    //    //ViewBag.Message += T.Text + " | ";
                    //}

                }
            }
            catch (Exception err)
            {
                var LineNumber = new StackTrace(err, true).GetFrame(0).GetFileLineNumber();
                ViewBag.scripCall += "showmsgJS(1, '" + err.Message.Replace("'", "") + " At Line number " + LineNumber + "', 1);";
            }
            ModelState.Clear();

            return View(model);
        }

        #endregion


        //================ vedio conference & Broadcast
        #region
        public ActionResult v_schedule()
        {
            FillMst("");
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            List<SelectListItem> mod1 = new List<SelectListItem>();
            TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
            model[0].SelectedItems1 = new string[] { "" };
            model[0].Col9 = "SCHEDULE";
            model[0].Col10 = "enx_tab2";
            model[0].Col12 = "SDL";
            model[0].Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            ModelState.Clear();
            return View(model);
        }

        [HttpPost]
        public ActionResult v_schedule(List<Tmodelmain> model, string command)
        {
            FillMst(model[0].Col15);
            var tm = model.FirstOrDefault();
            IList<HttpPostedFileBase> fileCollection1 = new List<HttpPostedFileBase>();
            List<SelectListItem> mod1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
            TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
            if (model[0].SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
            if (command == "Cancel")
            {
                return CancelFun(model);
            }
            else if (command == "New")
            {
                model = getnew(model);
                TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1 = cmd_Fun.mailcc(userCode);
                model[0].SelectedItems1 = new string[] { "" };
            }
            else if (command == "Callback")
            {
                model = StartCallback(model);
            }
            else if (command == "Send")
            {
                try
                {
                    string Random_no = "", Random_no2 = "";
                    int rrno;
                    string currdate = sgen.server_datetime(userCode);
                    string emailidcc = "";
                    string ncc = "", tto = "", tcc = "";
                    string cdt = "";
                    if ((model[0].Col18 != "") && (model[0].Col18 != "0") && (model[0].Col18 != null))
                    {
                        DateTime dt = sgen.Make_datetime(model[0].Col18);
                        cdt = dt.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    #region SEND MAIL
                    string mailfrom = "";
                    DataTable datatable = new DataTable();
                    datatable = sgen.getdata(userCode, "select com_email from company_profile where Company_Profile_Id='" + clientid_mst + "' and type='CP'");
                    if (datatable.Rows.Count > 0)
                    {
                        mailfrom = Convert.ToString(datatable.Rows[0]["com_email"]);
                    }
                    string mailcc = string.Join(",", model[0].SelectedItems1);
                    mailcc = mailcc.Replace("'", "");
                    string[] itar = mailcc.Trim().Split(',');
                    if ((model[0].SelectedItems1.FirstOrDefault() != null) && (model[0].SelectedItems1.FirstOrDefault() != ""))
                    {
                        if (itar.Length > 0)
                        {
                            for (Int32 y = 0; y < itar.Length; y++)
                            {
                                string[] Emialar = itar[y].Replace("(", "").Replace(")", "").Trim().Split(' ');
                                if (sgen.ValidateEmail(Emialar[1].ToString()))
                                {
                                    if (emailidcc.Equals("")) emailidcc = Emialar[1].ToString();
                                    else
                                    {
                                        emailidcc = emailidcc + "," + Emialar[1].ToString();
                                    }
                                }
                            }
                        }
                        Random rnd = new Random();
                        //rnd.Next(1000000, 9999999);
                        //rnd.Next();
                        rrno = rnd.Next(1000000, 9999999);
                        Random_no = rrno.ToString();
                        Random_no2 = rrno.ToString();
                        #region for participant
                        Random_no = "https://" + userCode + ".skyinfy.com/home/v_conf_lnk?m_id=qZrJvpRdx/a1u7IWpkxw9A==&mid=I5HxeUiiEdA=&rid=skyinfy-" + Random_no;
                        //Random_no = "https://" + userCode + ".skyinfy.com/home/v_conf_lnk?m_id=qZrJvpRdx/a1u7IWpkxw9A==&mid=I5HxeUiiEdA=&rid=skyinfy-" + Random_no + "&ishost=1";
                        var body = new StringBuilder();
                        body.AppendLine(@"<b style='color:#3caee9; font-size: 20px'>Link For Join Conference Meeting </b><br /><hr style='border:1px solid black' />" +
                             "<p>Hi <b>Dear: Team Member</b>,</p><table style='font-weight:600'>");
                        body.AppendLine(@"</table><br /><p>" + model[0].Col17 + "<br />" + Random_no + "</p><br />" +
                            "Meeting Is Scheduled on " + cdt + "<br /><p>Thank You,<br />" + clientname_mst + "<br /></p><br />");
                        sgen.Send_mail_SA1(userCode, clientid_mst, unitid_mst, mailcc, emailidcc, "", body.ToString(), "V Schedule", "");
                        #endregion

                        #region For Host
                        Random_no2 = "https://" + userCode + ".skyinfy.com/home/v_conf_lnk?m_id=qZrJvpRdx/a1u7IWpkxw9A==&mid=I5HxeUiiEdA=&rid=skyinfy-" + Random_no2 + "&ishost=1";
                        body = new StringBuilder();
                        body.AppendLine(@"<b style='color:#3caee9; font-size: 20px'>Link For Join Conference Meeting </b><br /><hr style='border:1px solid black' />" +
                             "<p>Hi <b>Dear: Meeting Host</b>,</p><table style='font-weight:600'>");
                        body.AppendLine(@"</table><br /><p>" + model[0].Col17 + "<br />" + Random_no2 + "</p><br />" +
                            "Meeting Is Scheduled on " + cdt + "<br /><p>Thank You,<br />" + clientname_mst + "<br /></p><br />");
                        sgen.Send_mail_SA1(userCode, clientid_mst, unitid_mst, mailfrom, "", "", body.ToString(), "V Schedule", "");
                        #endregion
                        #region saving
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
                        DataTable dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                        DataRow dr = dtstr.NewRow();
                        dr["rec_id"] = "0";
                        dr["type"] = model[0].Col12;
                        dr["vch_num"] = vch_num.Trim();
                        dr["vch_date"] = currdate;
                        //dr["col1"] = Random_no;
                        //dr["col3"] = Random_no2;
                        dr["col13"] = model[0].Col17; //title
                        dr["col4"] = string.Join(",", model[0].SelectedItems1);//users
                        dr["date1"] = sgen.Savedate(model[0].Col18, true);
                        dr = getsave(currdate, dr, model);
                        dtstr.Rows.Add(dr);
                        bool Result = sgen.Update_data_fast1(userCode, dtstr, model[0].Col10.Trim(), tm.Col8, isedit);
                        #endregion
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
                        });
                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        ViewBag.vsavenew = "disabled='disabled'";
                        ViewBag.vsavenew = "disabled='disabled'";
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Mail Send Successfully');disableForm();";
                    }
                    #endregion


                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
            }
            ModelState.Clear();
            return View(model);
        }
        public ActionResult v_conf_lnk()
        {

            FillMst();
            var rid = Request.QueryString["rid"];
            var ishost = Request.QueryString["ishost"];
            if (rid != null) { ViewBag.rid = rid; }
            else { ViewBag.rid = "non"; }
            var userid_temp = "";
            var cl_id = "";
            var unit_id = "";
            var ac_year = "";
            var ttype = "";
            userid_temp = Request.QueryString["uid"];
            cl_id = Request.QueryString["cl_id"];
            unit_id = Request.QueryString["ut_id"];
            ac_year = Request.QueryString["ac_year"];
            if (userid_temp == null || userid_temp == "") userid_temp = userid_mst;
            if (cl_id == null || cl_id == "") cl_id = clientid_mst;
            if (unit_id == null || unit_id == "") unit_id = unitid_mst;
            if (ac_year == null || ac_year == "") ac_year = Ac_Year_id;
            ttype = Request.QueryString["ttype"];
            if (ttype == null || ttype == "") ttype = sgen.GetCookie(MyGuid, "utype_mst");
            ViewBag.vnew = "";
            ViewBag.ishost = ishost;
            ViewBag.vedit = "";
            ViewBag.vsave = "disabled='disabled'";
            List<Tmodelmain> model = new List<Tmodelmain>();
            List<SelectListItem> md1 = new List<SelectListItem>();
            List<SelectListItem> md2 = new List<SelectListItem>();
            Tmodelmain tm1 = new Tmodelmain();
            model = getload(model);
            DataTable dtd = new DataTable();
            DataTable dt = new DataTable();
            if (ttype == "CPR")
            {
                mq = "SELECT distinct ud.vch_num||to_char(ud.vch_date, 'yyyymmdd')|| ud.type as fstr,ud.user_id,ud.first_name||' '||replace(ud.middle_name,'0','')||''||replace(ud.last_name,'0','') as name," +
                                "ud.email,nvl(msdg.master_name, '-') section,ud.client_id,ud.client_unit_id,ud.type,ud.vch_num as u_id,nvl(msdp.master_name, '-') Class,ud.status FROM user_details ud " +
                                "left join master_setting msdp on ud.DEPARTMENT = msdp.master_id and msdp.type = 'MDP' and find_in_set(msdp.client_unit_id, ud.client_unit_id)= 1 " +
                                "left join master_setting msdg on ud.designation = msdg.master_id and msdg.type = 'MDG' and find_in_set(msdg.client_unit_id, ud.client_unit_id)= 1 " +
                                "where ud.type = 'CPR' and ud.vch_num='" + userid_temp + "'";
            }
            else
            {
                //mq = "select distinct (ud.vch_num|| to_char(ud.vch_date, 'yyyymmdd')|| ud.type) as fstr,(ud.FIRST_NAME||' '|| REPLACE(ud.MIDDLE_NAME, '0', '')||' '|| REPLACE(ud.LAST_NAME, '0', '')) as name," +
                //    "(ud.f_firstname||' '|| REPLACE(ud.f_middlename, '0', '')||' '|| REPLACE(ud.f_lastname, '0', '')) as Father_Name,sec.master_id secid,cl.add_class_id,ud.Status," +
                //    "(case when to_char(ud.dob,'dd/MM/YYYY')='01/01/1900' then '-' else to_char(ud.dob,'dd/MM/YYYY') end) as DOB,ud.gender as Gender,nvl(cl.class,'-') as Class,ud.client_unit_id," +
                //    "ud.client_id,nvl(sd.roll_number,'-') as u_id,ud.type,nvl(sec.master_name,'-') as section,ud.adhaar_id as Adhaar_No,ud.isfamily as Is_Family,ud.regnumber as Reg_No from user_details" +
                //    " ud inner join student_detail sd on sd.reg_no=ud.RegNumber and sd.type= ud.type inner join add_class cl on sd.class_applied=cl.add_class_id and cl.type= 'EAC' and " +
                //    "cl.client_unit_id=ud.client_unit_id inner join master_setting sec on sd.section= sec.master_id and find_in_set(sec.client_unit_id,sd.client_unit_id)=1 and " +
                //    "sec.type= 'ECS' WHERE ud.type= 'STD' and ud.status='active' AND ud.vch_num='" + userid_temp + "' order by ud.vch_num desc";

                mq = "select (ud.vch_num|| to_char(ud.vch_date, 'yyyymmdd')|| ud.type) as fstr,(ud.FIRST_NAME||' '|| REPLACE(ud.MIDDLE_NAME, '0', '')||' '|| REPLACE(ud.LAST_NAME, '0', '')) as name" +
    ",(ud.f_firstname||' '|| REPLACE(ud.f_middlename, '0', '')||' '|| REPLACE(ud.f_lastname, '0', '')) as Father_Name,sec.master_id secid,cl.add_class_id,ud.Status,(case when to_char(ud.dob,'dd/MM/YYYY')='01/01/1900' then '-' else to_char(ud.dob,'dd/MM/YYYY') end) as DOB" +
    ",ud.gender as Gender,nvl(cl.class,'-') as Class,nvl(sd.roll_number,'-') as u_id,ud.type,nvl(sec.master_name,'-') as section,nvl(ad.academic_year,'-') as Acd_Year,ud.adhaar_id as Adhaar_No,ud.isfamily as Is_Family,ud.regnumber as Reg_No" +
    ",ud.srn_no as SRN_No,ud.withdrawl_no as Withdrawl_No,ud.enrollment_no as Enrol_No from user_details ud LEFT join student_detail sd on sd.reg_no=ud.RegNumber and sd.type= ud.type and sd.academic_year_id='" + ac_year + "' " +
    "left outer join add_class cl on sd.class_applied=cl.add_class_id and cl.client_unit_id='" + unit_id + "' and cl.type= 'EAC' left outer join add_academic_year ad on sd.academic_year_id= ad.academic_year_id and ad.client_id='" + unit_id + "'" +
    " and ad.type= 'ACY' left outer join master_setting sec on sd.section= sec.master_id and find_in_set(sec.client_unit_id,'" + unit_id + "')=1 and sec.type= 'ECS' WHERE ud.client_unit_id= '" + unit_id + "' and ud.type= 'STD' and " +
    "ud.status='active' AND ud.vch_num='" + userid_temp + "' order by ud.vch_num desc";
            }
            dtd = sgen.getdata(MyGuid, mq);

            if (dtd.Rows.Count > 0)
            {
                model[0].Col17 = dtd.Rows[0]["name"].ToString();
                model[0].Col18 = dtd.Rows[0]["u_id"].ToString();
                model[0].Col19 = dtd.Rows[0]["Class"].ToString();
                model[0].Col20 = dtd.Rows[0]["section"].ToString();
                model[0].Col23 = dtd.Rows[0]["type"].ToString();
                model[0].Col21 = cl_id;
                model[0].Col22 = unit_id;
                if (model[0].Col23 == "CPR")
                {
                    TempData[MyGuid + "_TList1"] = model[0].TList1 = md1 = cmd_Fun.lib_class(userCode, unit_id);
                    DataTable dtsec = sgen.getdata(userCode, "SELECT  master_id,master_name FROM master_setting where type='ECS' and find_in_set(client_unit_id,'" + unit_id + "')=1 ");
                    if (dtsec.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtsec.Rows)
                        {
                            md2.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                        }
                    }
                    TempData[MyGuid + "_TList2"] = model[0].TList2 = md2;
                    model[0].Col24 = "";
                    model[0].Col25 = "";
                }
                else if (model[0].Col23 == "STD")
                {
                    model[0].Col24 = dtd.Rows[0]["add_class_id"].ToString();
                    model[0].Col25 = dtd.Rows[0]["secid"].ToString();
                }
                tm1.SelectedItems1 = new string[] { "" };
                tm1.SelectedItems2 = new string[] { "" };
            }
            else
            {
                model[0].Col17 = "abc";
                model[0].Col18 = "xxxx";
                model[0].Col19 = "xx";
                model[0].Col20 = "xx";
                model[0].Col23 = "xx";
                model[0].Col24 = "";
                model[0].Col25 = "";
                model[0].Col21 = clientid_mst;
                model[0].Col22 = unitid_mst;
            }
            return View(model);

        }
        public ActionResult V_Conference()
        {

            FillMst();
            var userid_temp = "";
            var cl_id = "";
            var unit_id = "";
            var ac_year = "";
            var ttype = "";
            userid_temp = Request.QueryString["uid"];
            cl_id = Request.QueryString["cl_id"];
            unit_id = Request.QueryString["ut_id"];
            ac_year = Request.QueryString["ac_year"];
            if (userid_temp == null || userid_temp == "") userid_temp = userid_mst;
            if (cl_id == null || cl_id == "") cl_id = clientid_mst;
            if (unit_id == null || unit_id == "") unit_id = unitid_mst;
            if (ac_year == null || ac_year == "") ac_year = Ac_Year_id;
            ttype = Request.QueryString["ttype"];
            if (ttype == null || ttype == "") ttype = sgen.GetCookie(MyGuid, "utype_mst");
            ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.vsave = "disabled='disabled'";
            List<Tmodelmain> model = new List<Tmodelmain>();
            List<SelectListItem> md1 = new List<SelectListItem>();
            List<SelectListItem> md2 = new List<SelectListItem>();
            Tmodelmain tm1 = new Tmodelmain();
            model = getload(model);
            DataTable dtd = new DataTable();
            DataTable dt = new DataTable();
            if (ttype == "CPR")
            {
                mq = "SELECT distinct ud.vch_num||to_char(ud.vch_date, 'yyyymmdd')|| ud.type as fstr,ud.user_id,ud.first_name||' '||replace(ud.middle_name,'0','')||''||replace(ud.last_name,'0','') as name," +
                                "ud.email,nvl(msdg.master_name, '-') section,ud.client_id,ud.client_unit_id,ud.type,ud.vch_num as u_id,nvl(msdp.master_name, '-') Class,ud.status FROM user_details ud " +
                                "left join master_setting msdp on ud.DEPARTMENT = msdp.master_id and msdp.type = 'MDP' and find_in_set(msdp.client_unit_id, ud.client_unit_id)= 1 " +
                                "left join master_setting msdg on ud.designation = msdg.master_id and msdg.type = 'MDG' and find_in_set(msdg.client_unit_id, ud.client_unit_id)= 1 " +
                                "where ud.type = 'CPR' and ud.vch_num='" + userid_temp + "'";
            }
            else
            {
                //mq = "select distinct (ud.vch_num|| to_char(ud.vch_date, 'yyyymmdd')|| ud.type) as fstr,(ud.FIRST_NAME||' '|| REPLACE(ud.MIDDLE_NAME, '0', '')||' '|| REPLACE(ud.LAST_NAME, '0', '')) as name," +
                //    "(ud.f_firstname||' '|| REPLACE(ud.f_middlename, '0', '')||' '|| REPLACE(ud.f_lastname, '0', '')) as Father_Name,sec.master_id secid,cl.add_class_id,ud.Status," +
                //    "(case when to_char(ud.dob,'dd/MM/YYYY')='01/01/1900' then '-' else to_char(ud.dob,'dd/MM/YYYY') end) as DOB,ud.gender as Gender,nvl(cl.class,'-') as Class,ud.client_unit_id," +
                //    "ud.client_id,nvl(sd.roll_number,'-') as u_id,ud.type,nvl(sec.master_name,'-') as section,ud.adhaar_id as Adhaar_No,ud.isfamily as Is_Family,ud.regnumber as Reg_No from user_details" +
                //    " ud inner join student_detail sd on sd.reg_no=ud.RegNumber and sd.type= ud.type inner join add_class cl on sd.class_applied=cl.add_class_id and cl.type= 'EAC' and " +
                //    "cl.client_unit_id=ud.client_unit_id inner join master_setting sec on sd.section= sec.master_id and find_in_set(sec.client_unit_id,sd.client_unit_id)=1 and " +
                //    "sec.type= 'ECS' WHERE ud.type= 'STD' and ud.status='active' AND ud.vch_num='" + userid_temp + "' order by ud.vch_num desc";

                mq = "select (ud.vch_num|| to_char(ud.vch_date, 'yyyymmdd')|| ud.type) as fstr,(ud.FIRST_NAME||' '|| REPLACE(ud.MIDDLE_NAME, '0', '')||' '|| REPLACE(ud.LAST_NAME, '0', '')) as name" +
    ",(ud.f_firstname||' '|| REPLACE(ud.f_middlename, '0', '')||' '|| REPLACE(ud.f_lastname, '0', '')) as Father_Name,sec.master_id secid,cl.add_class_id,ud.Status,(case when to_char(ud.dob,'dd/MM/YYYY')='01/01/1900' then '-' else to_char(ud.dob,'dd/MM/YYYY') end) as DOB" +
    ",ud.gender as Gender,nvl(cl.class,'-') as Class,nvl(sd.roll_number,'-') as u_id,ud.type,nvl(sec.master_name,'-') as section,nvl(ad.academic_year,'-') as Acd_Year,ud.adhaar_id as Adhaar_No,ud.isfamily as Is_Family,ud.regnumber as Reg_No" +
    ",ud.srn_no as SRN_No,ud.withdrawl_no as Withdrawl_No,ud.enrollment_no as Enrol_No from user_details ud LEFT join student_detail sd on sd.reg_no=ud.RegNumber and sd.type= ud.type and sd.academic_year_id='" + ac_year + "' " +
    "left outer join add_class cl on sd.class_applied=cl.add_class_id and cl.client_unit_id='" + unit_id + "' and cl.type= 'EAC' left outer join add_academic_year ad on sd.academic_year_id= ad.academic_year_id and ad.client_id='" + unit_id + "'" +
    " and ad.type= 'ACY' left outer join master_setting sec on sd.section= sec.master_id and find_in_set(sec.client_unit_id,'" + unit_id + "')=1 and sec.type= 'ECS' WHERE ud.client_unit_id= '" + unit_id + "' and ud.type= 'STD' and " +
    "ud.status='active' AND ud.vch_num='" + userid_temp + "' order by ud.vch_num desc";
            }
            dtd = sgen.getdata(MyGuid, mq);

            if (dtd.Rows.Count > 0)
            {
                model[0].Col17 = dtd.Rows[0]["name"].ToString();
                model[0].Col18 = dtd.Rows[0]["u_id"].ToString();
                model[0].Col19 = dtd.Rows[0]["Class"].ToString();
                model[0].Col20 = dtd.Rows[0]["section"].ToString();
                model[0].Col23 = dtd.Rows[0]["type"].ToString();
                model[0].Col21 = cl_id;
                model[0].Col22 = unit_id;
                if (model[0].Col23 == "CPR")
                {
                    TempData[MyGuid + "_TList1"] = model[0].TList1 = md1 = cmd_Fun.lib_class(userCode, unit_id);
                    DataTable dtsec = sgen.getdata(userCode, "SELECT  master_id,master_name FROM master_setting where type='ECS' and find_in_set(client_unit_id,'" + unit_id + "')=1 ");
                    if (dtsec.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtsec.Rows)
                        {
                            md2.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                        }
                    }
                    TempData[MyGuid + "_TList2"] = model[0].TList2 = md2;
                    model[0].Col24 = "";
                    model[0].Col25 = "";
                }
                else if (model[0].Col23 == "STD")
                {
                    model[0].Col24 = dtd.Rows[0]["add_class_id"].ToString();
                    model[0].Col25 = dtd.Rows[0]["secid"].ToString();
                }
                tm1.SelectedItems1 = new string[] { "" };
                tm1.SelectedItems2 = new string[] { "" };
            }
            else
            {
                model[0].Col17 = "abc";
                model[0].Col18 = "xxxx";
                model[0].Col19 = "xx";
                model[0].Col20 = "xx";
                model[0].Col23 = "xx";
                model[0].Col24 = "";
                model[0].Col25 = "";
                model[0].Col21 = clientid_mst;
                model[0].Col22 = unitid_mst;
            }
            return View(model);

        }

        public ActionResult V_Broadcast()
        {

            ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.vsave = "disabled='disabled'";
            Tmodelmain tmodel = new Tmodelmain();
            List<Tmodelmain> cards = new List<Tmodelmain>
            {
                //new Tmodelmain{ Col1="",Col2="",TList1=GetAllTeaTypes(),Col18="Ram Rattan",Col15="1"},
                //new Tmodelmain{ Col1="",Col2="",TList1=GetAllTeaTypes(),Col18="Ram Rattan",Col15="2"},
                //new Tmodelmain{ Col1="",Col2="",TList1=GetAllTeaTypes(),Col18="Ram Rattan",Col15="3"}

            };

            return View(cards);

        }
        public ActionResult V_Classes()
        {
            FillMst();
            ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.vsave = "disabled='disabled'";
            List<Tmodelmain> model = new List<Tmodelmain>();
            List<SelectListItem> md1 = new List<SelectListItem>();
            List<SelectListItem> md2 = new List<SelectListItem>();
            Tmodelmain tm1 = new Tmodelmain();
            model = getload(model);
            mq0 = "SELECT ud.vch_num||to_char(ud.vch_date, 'yyyymmdd')|| ud.type as fstr,ud.user_id,ud.first_name||' '||replace(ud.middle_name,'0','')||''||replace(ud.last_name,'0','') as teachername," +
                            "ud.email,nvl(msdg.master_name, '-') designation,ud.type,ud.vch_num as u_id,nvl(msdp.master_name, '-') department,ud.status FROM user_details ud " +
                            "left join master_setting msdp on ud.DEPARTMENT = msdp.master_id and msdp.type = 'MDP' and find_in_set(msdp.client_unit_id, ud.client_unit_id)= 1 " +
                            "left join master_setting msdg on ud.designation = msdg.master_id and msdg.type = 'MDG' and find_in_set(msdg.client_unit_id, ud.client_unit_id)= 1 " +
                            "where ud.type = 'CPR' and ud.vch_num='" + userid_mst + "'";
            DataTable dtd = sgen.getdata(MyGuid, mq0);
            mq = "select (ud.vch_num|| to_char(ud.vch_date, 'yyyymmdd')|| ud.type) as fstr,(ud.FIRST_NAME||' '|| REPLACE(ud.MIDDLE_NAME, '0', '')||' '|| REPLACE(ud.LAST_NAME, '0', '')) as Student_Name" +
       ",(ud.f_firstname||' '|| REPLACE(ud.f_middlename, '0', '')||' '|| REPLACE(ud.f_lastname, '0', '')) as Father_Name,sec.master_id secid,cl.add_class_id,ud.Status,(case when to_char(ud.dob,'dd/MM/YYYY')='01/01/1900' then '-' else to_char(ud.dob,'dd/MM/YYYY') end) as DOB" +
       ",ud.gender as Gender,nvl(cl.class,'-') as Class,nvl(sd.roll_number,'-') as rollno,ud.type,nvl(sec.master_name,'-') as section,nvl(ad.academic_year,'-') as Acd_Year,ud.adhaar_id as Adhaar_No,ud.isfamily as Is_Family,ud.regnumber as Reg_No" +
       ",ud.srn_no as SRN_No,ud.withdrawl_no as Withdrawl_No,ud.enrollment_no as Enrol_No from user_details ud LEFT join student_detail sd on sd.reg_no=ud.RegNumber and sd.type= ud.type and sd.academic_year_id='" + Ac_Year_id + "' " +
       "left outer join add_class cl on sd.class_applied=cl.add_class_id and cl.client_unit_id='" + unitid_mst + "' and cl.type= 'EAC' left outer join add_academic_year ad on sd.academic_year_id= ad.academic_year_id and ad.client_id='" + unitid_mst + "'" +
       " and ad.type= 'ACY' left outer join master_setting sec on sd.section= sec.master_id and find_in_set(sec.client_unit_id,'" + unitid_mst + "')=1 and sec.type= 'ECS' WHERE ud.client_unit_id= '" + unitid_mst + "' and ud.type= 'STD' and " +
       "ud.status='active' AND ud.vch_num='" + userid_mst + "' order by ud.vch_num desc";
            DataTable dt = sgen.getdata(MyGuid, mq);
            if (dtd.Rows.Count > 0)
            {
                model[0].Col17 = dtd.Rows[0]["teachername"].ToString();
                model[0].Col18 = dtd.Rows[0]["u_id"].ToString();
                model[0].Col19 = dtd.Rows[0]["department"].ToString();
                model[0].Col20 = dtd.Rows[0]["designation"].ToString();
                model[0].Col23 = dtd.Rows[0]["type"].ToString();

                TempData[MyGuid + "_TList1"] = model[0].TList1 = md1 = cmd_Fun.lib_class(userCode, unitid_mst);

                DataTable dtsec = sgen.getdata(userCode, "SELECT  master_id,master_name FROM master_setting where type='ECS' and find_in_set(client_unit_id,'" + unitid_mst + "')=1 ");
                if (dtsec.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtsec.Rows)
                    {
                        md2.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                    }
                }
                TempData[MyGuid + "_TList2"] = model[0].TList2 = md2;
                tm1.SelectedItems1 = new string[] { "" };
                tm1.SelectedItems2 = new string[] { "" };
                model[0].Col24 = "";
                model[0].Col25 = "";
            }
            else if (dt.Rows.Count > 0)
            {
                model[0].Col17 = dt.Rows[0]["Student_Name"].ToString();
                model[0].Col18 = dt.Rows[0]["rollno"].ToString();
                model[0].Col19 = dt.Rows[0]["Class"].ToString();
                model[0].Col20 = dt.Rows[0]["section"].ToString();
                model[0].Col23 = dt.Rows[0]["type"].ToString();
                model[0].Col24 = dt.Rows[0]["add_class_id"].ToString();
                model[0].Col25 = dt.Rows[0]["secid"].ToString();
            }
            else
            {
                model[0].Col17 = "abc";
                model[0].Col18 = "xxxx";
                model[0].Col19 = "xx";
                model[0].Col20 = "xx";
                model[0].Col23 = "xx";
                model[0].Col24 = "";
                model[0].Col25 = "";
            }
            model[0].Col21 = clientid_mst;
            model[0].Col22 = unitid_mst;
            return View(model);
        }
        public ActionResult V_Classes2()
        {
            FillMst();
            var userid_temp = "";
            var cl_id = "";
            var unit_id = "";
            var ac_year = "";
            var ttype = "";
            userid_temp = Request.QueryString["uid"];
            cl_id = Request.QueryString["cl_id"];
            unit_id = Request.QueryString["ut_id"];
            ac_year = Request.QueryString["ac_year"];
            if (userid_temp == null || userid_temp == "") userid_temp = userid_mst;
            if (cl_id == null || cl_id == "") cl_id = clientid_mst;
            if (unit_id == null || unit_id == "") unit_id = unitid_mst;
            if (ac_year == null || ac_year == "") ac_year = Ac_Year_id;
            ttype = Request.QueryString["ttype"];
            if (ttype == null || ttype == "") ttype = sgen.GetCookie(MyGuid, "utype_mst");
            ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.vsave = "disabled='disabled'";
            List<Tmodelmain> model = new List<Tmodelmain>();
            List<SelectListItem> md1 = new List<SelectListItem>();
            List<SelectListItem> md2 = new List<SelectListItem>();
            Tmodelmain tm1 = new Tmodelmain();
            model = getload(model);
            DataTable dtd = new DataTable();
            DataTable dt = new DataTable();
            if (ttype == "CPR")
            {
                mq = "SELECT distinct ud.vch_num||to_char(ud.vch_date, 'yyyymmdd')|| ud.type as fstr,ud.user_id,ud.first_name||' '||replace(ud.middle_name,'0','')||''||replace(ud.last_name,'0','') as name," +
                                "ud.email,nvl(msdg.master_name, '-') section,ud.client_id,ud.client_unit_id,ud.type,ud.vch_num as u_id,nvl(msdp.master_name, '-') Class,ud.status FROM user_details ud " +
                                "left join master_setting msdp on ud.DEPARTMENT = msdp.master_id and msdp.type = 'MDP' and find_in_set(msdp.client_unit_id, ud.client_unit_id)= 1 " +
                                "left join master_setting msdg on ud.designation = msdg.master_id and msdg.type = 'MDG' and find_in_set(msdg.client_unit_id, ud.client_unit_id)= 1 " +
                                "where ud.type = 'CPR' and ud.vch_num='" + userid_temp + "'";
            }
            else
            {
                //mq = "select distinct (ud.vch_num|| to_char(ud.vch_date, 'yyyymmdd')|| ud.type) as fstr,(ud.FIRST_NAME||' '|| REPLACE(ud.MIDDLE_NAME, '0', '')||' '|| REPLACE(ud.LAST_NAME, '0', '')) as name," +
                //    "(ud.f_firstname||' '|| REPLACE(ud.f_middlename, '0', '')||' '|| REPLACE(ud.f_lastname, '0', '')) as Father_Name,sec.master_id secid,cl.add_class_id,ud.Status," +
                //    "(case when to_char(ud.dob,'dd/MM/YYYY')='01/01/1900' then '-' else to_char(ud.dob,'dd/MM/YYYY') end) as DOB,ud.gender as Gender,nvl(cl.class,'-') as Class,ud.client_unit_id," +
                //    "ud.client_id,nvl(sd.roll_number,'-') as u_id,ud.type,nvl(sec.master_name,'-') as section,ud.adhaar_id as Adhaar_No,ud.isfamily as Is_Family,ud.regnumber as Reg_No from user_details" +
                //    " ud inner join student_detail sd on sd.reg_no=ud.RegNumber and sd.type= ud.type inner join add_class cl on sd.class_applied=cl.add_class_id and cl.type= 'EAC' and " +
                //    "cl.client_unit_id=ud.client_unit_id inner join master_setting sec on sd.section= sec.master_id and find_in_set(sec.client_unit_id,sd.client_unit_id)=1 and " +
                //    "sec.type= 'ECS' WHERE ud.type= 'STD' and ud.status='active' AND ud.vch_num='" + userid_temp + "' order by ud.vch_num desc";

                mq = "select (ud.vch_num|| to_char(ud.vch_date, 'yyyymmdd')|| ud.type) as fstr,(ud.FIRST_NAME||' '|| REPLACE(ud.MIDDLE_NAME, '0', '')||' '|| REPLACE(ud.LAST_NAME, '0', '')) as name" +
    ",(ud.f_firstname||' '|| REPLACE(ud.f_middlename, '0', '')||' '|| REPLACE(ud.f_lastname, '0', '')) as Father_Name,sec.master_id secid,cl.add_class_id,ud.Status,(case when to_char(ud.dob,'dd/MM/YYYY')='01/01/1900' then '-' else to_char(ud.dob,'dd/MM/YYYY') end) as DOB" +
    ",ud.gender as Gender,nvl(cl.class,'-') as Class,nvl(sd.roll_number,'-') as u_id,ud.type,nvl(sec.master_name,'-') as section,nvl(ad.academic_year,'-') as Acd_Year,ud.adhaar_id as Adhaar_No,ud.isfamily as Is_Family,ud.regnumber as Reg_No" +
    ",ud.srn_no as SRN_No,ud.withdrawl_no as Withdrawl_No,ud.enrollment_no as Enrol_No from user_details ud LEFT join student_detail sd on sd.reg_no=ud.RegNumber and sd.type= ud.type and sd.academic_year_id='" + ac_year + "' " +
    "left outer join add_class cl on sd.class_applied=cl.add_class_id and cl.client_unit_id='" + unit_id + "' and cl.type= 'EAC' left outer join add_academic_year ad on sd.academic_year_id= ad.academic_year_id and ad.client_id='" + unit_id + "'" +
    " and ad.type= 'ACY' left outer join master_setting sec on sd.section= sec.master_id and find_in_set(sec.client_unit_id,'" + unit_id + "')=1 and sec.type= 'ECS' WHERE ud.client_unit_id= '" + unit_id + "' and ud.type= 'STD' and " +
    "ud.status='active' AND ud.vch_num='" + userid_temp + "' order by ud.vch_num desc";
            }
            dtd = sgen.getdata(MyGuid, mq);

            if (dtd.Rows.Count > 0)
            {
                model[0].Col17 = dtd.Rows[0]["name"].ToString();
                model[0].Col18 = dtd.Rows[0]["u_id"].ToString();
                model[0].Col19 = dtd.Rows[0]["Class"].ToString();
                model[0].Col20 = dtd.Rows[0]["section"].ToString();
                model[0].Col23 = dtd.Rows[0]["type"].ToString();
                model[0].Col21 = cl_id;
                model[0].Col22 = unit_id;
                if (model[0].Col23 == "CPR")
                {
                    TempData[MyGuid + "_TList1"] = model[0].TList1 = md1 = cmd_Fun.lib_class(userCode, unit_id);
                    DataTable dtsec = sgen.getdata(userCode, "SELECT  master_id,master_name FROM master_setting where type='ECS' and find_in_set(client_unit_id,'" + unit_id + "')=1 ");
                    if (dtsec.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtsec.Rows)
                        {
                            md2.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                        }
                    }
                    TempData[MyGuid + "_TList2"] = model[0].TList2 = md2;
                    model[0].Col24 = "";
                    model[0].Col25 = "";
                }
                else if (model[0].Col23 == "STD")
                {
                    model[0].Col24 = dtd.Rows[0]["add_class_id"].ToString();
                    model[0].Col25 = dtd.Rows[0]["secid"].ToString();
                }
                tm1.SelectedItems1 = new string[] { "" };
                tm1.SelectedItems2 = new string[] { "" };
            }
            else
            {
                model[0].Col17 = "abc";
                model[0].Col18 = "xxxx";
                model[0].Col19 = "xx";
                model[0].Col20 = "xx";
                model[0].Col23 = "xx";
                model[0].Col24 = "";
                model[0].Col25 = "";
                model[0].Col21 = clientid_mst;
                model[0].Col22 = unitid_mst;
            }
            return View(model);
        }
        public ActionResult skype()
        {
            ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.vsave = "disabled='disabled'";
            Tmodelmain tmodel = new Tmodelmain();
            List<Tmodelmain> cards = new List<Tmodelmain>
            {
                //new Tmodelmain{ Col1="",Col2="",TList1=GetAllTeaTypes(),Col18="Ram Rattan",Col15="1"},
                //new Tmodelmain{ Col1="",Col2="",TList1=GetAllTeaTypes(),Col18="Ram Rattan",Col15="2"},
                //new Tmodelmain{ Col1="",Col2="",TList1=GetAllTeaTypes(),Col18="Ram Rattan",Col15="3"}

            };

            return View(cards);

        }
        public ActionResult V_admin()
        {

            ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.vsave = "disabled='disabled'";
            Tmodelmain tmodel = new Tmodelmain();
            List<Tmodelmain> cards = new List<Tmodelmain>
            {
                //new Tmodelmain{ Col1="",Col2="",TList1=GetAllTeaTypes(),Col18="Ram Rattan",Col15="1"},
                //new Tmodelmain{ Col1="",Col2="",TList1=GetAllTeaTypes(),Col18="Ram Rattan",Col15="2"},
                //new Tmodelmain{ Col1="",Col2="",TList1=GetAllTeaTypes(),Col18="Ram Rattan",Col15="3"}

            };

            return View(cards);

        }

        #endregion

        public ActionResult crm_reports()
        {
            ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.vsave = "disabled='disabled'";
            Tmodelmain tmodel = new Tmodelmain();
            List<Tmodelmain> cards = new List<Tmodelmain>
            {
                //new Tmodelmain{ Col1="",Col2="",TList1=GetAllTeaTypes(),Col18="Ram Rattan",Col15="1"},
                //new Tmodelmain{ Col1="",Col2="",TList1=GetAllTeaTypes(),Col18="Ram Rattan",Col15="2"},
                //new Tmodelmain{ Col1="",Col2="",TList1=GetAllTeaTypes(),Col18="Ram Rattan",Col15="3"}

            };
            return View(cards);
        }

        #region report templates (school) 
        public ActionResult temp_Report()
        {
            userCode = sgen.GetCookie(MyGuid, "userCode");
            if (userCode.Equals("")) Response.Redirect("~/home/login");

            List<Tmodelmain> model = new List<Tmodelmain>();
            Tmodelmain tm1 = new Tmodelmain();

            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            m_id_mst = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            if (mid_mst.Trim().Equals("7020.8")) { tm1.Col9 = "STUDENT ICARD TEMPLATE"; }
            if (mid_mst.Trim().Equals("21001.8")) { tm1.Col9 = "INVOICE TEMPLATE"; }
            if (mid_mst.Trim().Equals("22001.4")) { tm1.Col9 = "PT REPORT"; }
            if (mid_mst.Trim().Equals("22001.4")) { tm1.Col9 = "PT REPORT"; }


            tm1.Col10 = mid_mst.Trim();
            tm1.Col11 = m_id_mst.Trim();
            sgen.SetSession(MyGuid, "TR_MID", mid_mst.Trim());
            model.Add(tm1);

            return View(model);
        }
        [HttpPost]
        public ActionResult temp_Report(List<Tmodelmain> model, string command)
        {
            return View(model);
        }
        #endregion

        #region adminora
        public ActionResult adminora()
        {

            List<Tmodelmain> model = new List<Tmodelmain>();
            Tmodelmain tm = new Tmodelmain();

            try
            {

                sgen.SetSession(MyGuid, "cocd_mst", "-");
                userCode = sgen.getUserCode();
                mq1 = "select company_name from company_profile where type='CG'";
                tm.Col11 = (sgen.getstring(userCode, mq1)).ToUpper();
                tm.Col16 = HttpRuntime.AppDomainAppPath;
                tm.Col16 = "";
                tm.Col21 = userCode;
                tm.Col22 = "";
                tm.Col25 = "";
                sgen.SetCookie(MyGuid, "userCode", userCode);
                ViewBag.vnew = "";
                ViewBag.vedit = "";
                ViewBag.vsave = "disabled='disabled'";
                ViewBag.scripCall = "disableForm();";
            }
            catch (Exception err)
            {
                tm.Col11 = err.Message;
                tm.Col16 = err.Message;
            }
            model.Add(tm);

            return View(model);
        }
        [HttpPost]
        public ActionResult adminora(List<Tmodelmain> model, string command, HttpPostedFileBase upd1)
        {
            userCode = sgen.getUserCode();
            if (command == "Export")
            {
                try
                {
                    string batchfile = HttpRuntime.AppDomainAppPath + "\\backups\\batchfile.bat";
                    string dmpfile = HttpRuntime.AppDomainAppPath + "\\backups\\" + userCode + "" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".dmp";
                    string logfile = HttpRuntime.AppDomainAppPath + "\\backups\\" + userCode + "" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".log";
                    string exp_cmd = "exp " + userCode + "/SATECH file='" + dmpfile + "' GRANTS=Y log=" + logfile + "";


                    try
                    {
                        if (System.IO.File.Exists(batchfile))
                        {
                            System.IO.File.Delete(batchfile);
                        }
                        StreamWriter w = new StreamWriter(batchfile, true);
                        w.WriteLine(exp_cmd);
                        w.Flush();
                        w.Close();
                    }
                    catch { }
                    // Get the full file path

                    System.Diagnostics.Process p = new System.Diagnostics.Process();
                    p.StartInfo = new System.Diagnostics.ProcessStartInfo(batchfile);
                    p.Start();

                }
                catch (Exception err)
                {

                }
            }
            if (command == "Import")
            {
                //try
                //{
                //    string batchfile = HttpRuntime.AppDomainAppPath + "\\backups\\batchfile.bat";
                //    string dmpfile = HttpRuntime.AppDomainAppPath + "\\backups\\" + upd1.FileName;
                //    string exp_cmd = "imp " + userCode + "/SATECH file='" + dmpfile + "' GRANTS=Y full=y log=a.log";
                //    DataTable dt = sgen.getdata_DR(userCode, "select username from dba_users where " +
                //        "trim(upper(username))='" + userCode.ToUpper().Trim() + "'", sgen.connString("SYSTEM", "SKYINFY", "LOCALHOST", "XE"));
                //    if (dt.Rows.Count > 0)
                //    {
                //        model[0].Col25 = "Database Already Exists! Can not import Again";
                //    }
                //    else
                //    {

                //        sgen.execute_cmd(userCode, "CREATE USER " + userCode + " IDENTIFIED BY SATECH", sgen.connString("SYSTEM", "SKYINFY", "LOCALHOST", "XE"));
                //        sgen.execute_cmd(userCode, "GRANT DBA,CONNECT,RESOURCE TO " + userCode + "", sgen.connString("SYSTEM", "SKYINFY", "LOCALHOST", "XE"));



                //        model[0].Col22 = upd1.FileName;
                //        model[0].Col25 = "Database Created";

                //        try
                //        {
                //            if (System.IO.File.Exists(batchfile))
                //            {
                //                System.IO.File.Delete(batchfile);
                //            }
                //            StreamWriter w = new StreamWriter(batchfile, true);
                //            w.WriteLine(exp_cmd);
                //            w.Flush();
                //            w.Close();
                //        }
                //        catch { }
                //        // Get the full file path

                //        System.Diagnostics.Process p = new System.Diagnostics.Process();
                //        p.StartInfo = new System.Diagnostics.ProcessStartInfo(batchfile);
                //        p.Start();
                //    }

                //}
                //catch (Exception err)
                //{

                //}
            }
            else if (command == "SMS")
            {
                FillMst();

            }
            ModelState.Clear();
            return View(model);
        }
        #endregion

        #region login
        [GZipOrDeflate]
        public ActionResult login()
        {
            string NewVersion = "2.1.2";
            Session["NewVersion"] = NewVersion;
            List<Tmodelmain> model = new List<Tmodelmain>();
            Tmodelmain tm = new Tmodelmain();
            try
            {
                var sid = Session.SessionID;
                userCode = Multiton.getUserCode();
                byte[] gb = Guid.NewGuid().ToByteArray();
                int i = BitConverter.ToInt32(gb, 0);
                MyGuid = i.ToString();

                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
                Response.Cache.SetNoStore();
                var cacheManager = new OutputCacheManager();
                cacheManager.RemoveItems();

                Multiton.SetCookie(MyGuid, "userCode", userCode);
                Multiton.SetSession(MyGuid, "userCode", userCode);
                FillMst(MyGuid);

                Multiton.SetSession(MyGuid, "cocd_mst", "-");
                mq1 = "select company_name,version from company_profile where type='CG'";

                var dtc = sgen.getdata(userCode, mq1);

                try
                {
                    tm.Col11 = dtc.Rows[0]["company_name"].ToString().Trim().ToUpper();
                    tm.Col12 = dtc.Rows[0]["version"].ToString().Trim().ToUpper();

                }
                catch (Exception err)
                {
                    tm.Col11 = "Connection to Server Failed.Please Check your Network Connection..";
                }
                tm.Col13 = NewVersion;
                //if (userCode.Trim().ToUpper().Equals("REAL"))
                //{
                //    tm.Col11 = "WELCOME TO";
                //}
                //tm.Col16 = HttpRuntime.AppDomainAppPath;
                tm.Col16 = "";
                tm.Col15 = MyGuid;

                //ViewBag.vnew = "";
                //ViewBag.vedit = "";
                //ViewBag.vsave = "disabled='disabled'";
                //ViewBag.scripCall = "disableForm();";
            }
            catch (Exception err)
            {
                tm.Col11 = err.Message;
                tm.Col16 = err.Message;
            }
            //tm.Col16 = Session.SessionID.ToString();

            model.Add(tm);

            return View(model);
        }
        [HttpPost]
        public ActionResult login(List<Tmodelmain> model, string command)
        {
            try
            {

                if (command == "Login")
                {
                    try
                    {
                        FillMst(model[0].Col15);

                        //bool isHuman = captchaBox.Validate(txtCaptcha.Text);
                        //txtCaptcha.Text = null;
                        //if (!ctrlGoogleReCaptcha.Validate())
                        //{
                        //    lbl_msg.Visible = true;
                        //    lbl_msg.Text = "invalid captcha";//The Captcha entered by user is Invalid.
                        //}
                        //else
                        {


                            string mq = "select company_status,company_name,Company_No_Of_User,subdomain,version,ver_name,company_code,client_type from company_profile where type='CG' and company_profile_id='000'";
                            DataTable dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count == 0)
                            {
                                model[0].Col16 = "No company found";
                                return View(model);
                            }
                            if (dt.Rows[0]["company_status"].ToString().Equals("0"))
                            {
                                model[0].Col16 = "No activated company found";
                                return View(model);
                            }
                            //if (dt.Rows[0]["ver_name"].ToString().Equals("") || dt.Rows[0]["version"].ToString().Equals(""))
                            //{
                            //    sgen.execute_cmd(userCode, "update company_profile set version='" + version.Trim() + "',ver_name='" + ver_name.Trim() + "' where type='CG'");
                            //}

                            sgen.SetCookie(MyGuid, "controls_mst", EncryptDecrypt.Decrypt(dt.Rows[0]["Company_No_Of_User"].ToString()));
                            sgen.SetCookie(MyGuid, "subdomain_mst", dt.Rows[0]["subdomain"].ToString());
                            sgen.SetCookie(MyGuid, "cg_com_name", dt.Rows[0]["company_code"].ToString());
                            sgen.SetCookie(MyGuid, "rrb", "home");
                            sgen.SetSession(MyGuid, "rrb", dt);

                            string pwd = EncryptDecrypt.Encrypt(model[0].Col22);
                            string textuser = model[0].Col21;
                            string txtpasswrd = pwd;
                            string client_unitid = "";
                            string client_id = "";

                            if (textuser == null && pwd == "")
                            {
                                model[0].Col16 = "Please Check your Username OR Password"; return View(model);
                            }
                            dt = new DataTable();
                            mq = "SELECT vch_num,user_id,password,client_id,status,role,client_unit_id,ent_by,profilepic,profilepic_path,type " +
                                "FROM user_details WHERE UPPER(trim(user_id))='" + textuser.Trim().ToUpper() + "' and password='" + pwd + "'";
                            dt = sgen.getdata(userCode, mq);
                            //if (dt.Rows.Count < 1) { model[0].Col16 = " No record found of this user id"; return View(model); }
                            if (dt.Rows.Count < 1) { model[0].Col16 = "Please Check your Username OR Password"; return View(model); }

                            //if (textuser != dt.Rows[0]["user_id"].ToString() && txtpasswrd != dt.Rows[0]["password"].ToString())
                            //{
                            //    model[0].Col16 = "Please Check your User name OR Password";
                            //}

                            if (dt.Rows[0]["status"].ToString() == "inactive")
                            {
                                model[0].Col21 = "";
                                model[0].Col16 = "Your Account Is Not Active Please Contact Your Admin";
                                return View(model);
                            }

                            string rola = dt.Rows[0]["role"].ToString();
                            sgen.SetCookie(MyGuid, "utype_mst", dt.Rows[0]["type"].ToString());
                            if (dt.Rows[0]["type"].ToString().Trim().ToUpper().Equals("STP")) sgen.SetCookie(MyGuid, "role_mst", "stpmain-1");
                            else if (dt.Rows[0]["type"].ToString().Trim().ToUpper().Equals("STD")) sgen.SetCookie(MyGuid, "role_mst", "stdmain-1");
                            else sgen.SetCookie(MyGuid, "role_mst", dt.Rows[0]["role"].ToString());
                            sgen.SetCookie(MyGuid, "username_mst", dt.Rows[0]["user_id"].ToString());
                            //sgen.SetCookie(MyGuid, "userid_mst", sgen.padlc(Convert.ToInt16(dt.Rows[0]["rec_id"].ToString()), 6));
                            sgen.SetCookie(MyGuid, "userid_mst", dt.Rows[0]["vch_num"].ToString());
                            userid_mst = dt.Rows[0]["vch_num"].ToString();

                            client_id = Convert.ToString(dt.Rows[0]["client_id"]);
                            //unitid_mst = Convert.ToString(dt.Rows[0]["client_unit_id"]);
                            client_unitid = Convert.ToString(dt.Rows[0]["client_unit_id"]);

                            sgen.SetCookie(MyGuid, "pp_filename", dt.Rows[0]["profilepic"].ToString());
                            sgen.SetCookie(MyGuid, "pp_filepath", dt.Rows[0]["profilepic_path"].ToString());

                            if (client_id.ToString().Trim().ToUpper() == "OWNER" || client_unitid.ToString().Trim().ToUpper() == "OWNER")
                            {
                                clientid_mst = "001";
                                unitid_mst = "001001";
                                sgen.SetCookie(MyGuid, "clientid_mst", "001");
                                sgen.SetCookie(MyGuid, "unitid_mst", "001001");
                            }
                            else
                            {
                                List<string> mylist1 = client_unitid.Split(',').Distinct().OrderBy(x => x).ToList();
                                int ct1 = mylist1.Count;
                                sgen.SetCookie(MyGuid, "unitid_mst", mylist1[0].ToString());

                                unitid_mst = mylist1[0].ToString();
                                clientid_mst = unitid_mst.Substring(0, 3);
                                sgen.SetCookie(MyGuid, "clientid_mst", clientid_mst);
                            }

                            if (rola.ToString().Trim().ToUpper() == "OWNER") mq = "select (case when max(company_status)='1' then 'active' else 'inactive' end) mval from company_profile ";
                            else mq = "select (case when max(company_status)='1' then 'active' else 'inactive' end) mval from company_profile where find_in_set(company_profile_id,'" + client_id + "')=1";
                            mq = sgen.seekval(userCode, mq, "mval");
                            if (mq == "inactive")
                            {
                                model[0].Col16 = "Your all Assigned Companies are Deactivated or Disabled, Please contact your admin to activate";
                                return View(model);
                            }

                            //string currdate = myyear;
                            DataTable dtc = sgen.getdata(userCode, "select company_name,logo_name,logo_path,client_type from company_profile where company_profile_id='" + clientid_mst + "'");
                            sgen.SetCookie(MyGuid, "clientname_mst", dtc.Rows[0]["company_name"].ToString());

                            //sgen.SetSession(MyGuid, "clientname_mst", dtc.Rows[0]["company_name"].ToString());


                            sgen.SetCookie(MyGuid, "cp_filename", dtc.Rows[0]["logo_name"].ToString());
                            sgen.SetCookie(MyGuid, "cp_filepath", dtc.Rows[0]["logo_path"].ToString());
                            string client_type = dtc.Rows[0]["client_type"].ToString();//Educational Inst.
                            string unitname_mst = sgen.getstring(userCode, "select unit_name from company_unit_profile where cup_id='" + unitid_mst + "'");
                            sgen.SetSession(MyGuid, "unitname_mst", unitname_mst);

                            mq = sgen.seekval(userCode, "SELECT COUNT(*) AS CNT FROM company_unit_profile where 1=1 and unit_status = '1' and company_profile_id='" + clientid_mst + "'", "cnt");
                            sgen.SetSession(MyGuid, "unitcnt", mq);

                            sgen.generateformats(userCode, clientid_mst);
                            string myyear = sgen.server_datetime_dt_local(userCode).ToString(sgen.Getdateformat(), CultureInfo.InvariantCulture);

                            //mq = "select TO_CHAR(to_date( to_char(year_to,'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldatetimeformat() + "') year_to ," +
                            //    "TO_CHAR(to_date( to_char(year_from,'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldatetimeformat() + "') year_from," +
                            //  "com_year,com_code  from com_year where to_date('" + myyear + "','" + sgen.Getsqldateformat() + "') between year_from and year_to and  client_unit_id='" + unitid_mst + "'";
                            //DataTable ytab = sgen.getdata(userCode, mq);
                            //if (ytab.Rows.Count == 0)
                            //{
                            //    mq = "select TO_CHAR(to_date( to_char(year_to,'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldatetimeformat() + "') year_to ," +
                            //    "TO_CHAR(to_date( to_char(year_from,'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldatetimeformat() + "') year_from," +
                            //                           "com_year,com_code  from com_year where  client_unit_id='" + unitid_mst + "' order by year_to DESC";
                            //    ytab = sgen.getdata(userCode, mq);
                            //    if (ytab.Rows.Count == 0)
                            //    {
                            //        model[0].Col16 = "Your Id is not Operatable in this Financial Year";
                            //        return View(model);
                            //    }
                            //}
                            //sgen.SetCookie(MyGuid, "year_to", ytab.Rows[0]["year_to"].ToString());
                            //sgen.SetCookie(MyGuid, "year_from", ytab.Rows[0]["year_from"].ToString());
                            //sgen.SetCookie(MyGuid, "com_yr", ytab.Rows[0]["com_year"].ToString());
                            //sgen.SetCookie(MyGuid, "com_codeyear", ytab.Rows[0]["com_code"].ToString());

                            //if (client_type.ToLower().Contains("educational"))
                            //{
                            DataTable dtyr = sgen.FindCurrentACYear(userCode, unitid_mst, clientid_mst);
                            if (dtyr.Rows.Count == 0)
                            {
                                dtyr = sgen.getdata(userCode, "SELECT academic_year_id,academic_year, " +
                                    "TO_CHAR(to_date( to_char(from_date,'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldatetimeformat() + "') from_date ," +
                                "TO_CHAR(to_date( to_char(to_date,'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldatetimeformat() + "') to_date FROM add_academic_year  " +
                  "WHERE type='ACY' and client_id='" + clientid_mst + "' order by to_date desc");

                                if (dtyr.Rows.Count > 0)
                                {
                                    sgen.SetCookie(MyGuid, "Ac_Year_id", dtyr.Rows[0]["academic_year_id"].ToString());
                                    sgen.SetCookie(MyGuid, "Ac_Year", dtyr.Rows[0]["academic_year"].ToString());
                                    sgen.SetCookie(MyGuid, "Ac_From_Date", dtyr.Rows[0]["From_Date"].ToString());
                                    sgen.SetCookie(MyGuid, "Ac_To_Date", dtyr.Rows[0]["To_Date"].ToString());

                                    sgen.SetCookie(MyGuid, "year_to", dtyr.Rows[0]["To_Date"].ToString());
                                    sgen.SetCookie(MyGuid, "year_from", dtyr.Rows[0]["From_Date"].ToString());
                                    sgen.SetCookie(MyGuid, "com_yr", dtyr.Rows[0]["academic_year"].ToString());
                                    //sgen.SetCookie(MyGuid, "com_codeyear", ytab.Rows[0]["com_code"].ToString());
                                }
                                else
                                {
                                    model[0].Col16 = "Your Id is not Operatable in this Academic Year";
                                    return View(model);
                                }
                            }
                            else
                            {
                                sgen.SetCookie(MyGuid, "Ac_Year_id", dtyr.Rows[0]["academic_year_id"].ToString());
                                sgen.SetCookie(MyGuid, "Ac_Year", dtyr.Rows[0]["academic_year"].ToString());
                                sgen.SetCookie(MyGuid, "Ac_From_Date", dtyr.Rows[0]["From_Date"].ToString());
                                sgen.SetCookie(MyGuid, "Ac_To_Date", dtyr.Rows[0]["To_Date"].ToString());

                                sgen.SetCookie(MyGuid, "year_to", dtyr.Rows[0]["To_Date"].ToString());
                                sgen.SetCookie(MyGuid, "year_from", dtyr.Rows[0]["From_Date"].ToString());
                                sgen.SetCookie(MyGuid, "com_yr", dtyr.Rows[0]["academic_year"].ToString());
                            }
                            //}
                            //else
                            //{
                            //    sgen.SetCookie(MyGuid, "Ac_Year_id", ytab.Rows[0]["com_code"].ToString());
                            //    sgen.SetCookie(MyGuid, "Ac_Year", ytab.Rows[0]["com_year"].ToString());
                            //    sgen.SetCookie(MyGuid, "Ac_From_Date", ytab.Rows[0]["year_from"].ToString());
                            //    sgen.SetCookie(MyGuid, "Ac_To_Date", ytab.Rows[0]["year_to"].ToString());

                            //}
                            //else
                            //{
                            //    dtyr = sgen.getdata(userCode, "select vch_num,com_year,year_from,year_to from com_year where client_id='" + clientid_mst + "' " +
                            //        "and client_unit_id='" + unitid_mst + "'");
                            //    if (dtyr.Rows.Count > 0)
                            //    {
                            //        sgen.SetCookie(MyGuid, "Ac_Year_id", dtyr.Rows[0]["vch_num"].ToString());
                            //        sgen.SetCookie(MyGuid, "Ac_Year", dtyr.Rows[0]["com_year"].ToString());
                            //        sgen.SetCookie(MyGuid, "Ac_From_Date", dtyr.Rows[0]["year_from"].ToString());
                            //        sgen.SetCookie(MyGuid, "Ac_To_Date", dtyr.Rows[0]["year_to"].ToString());
                            //    }
                            //}



                            //sgen.execute_cmd(userCode, "Update User_Details set User_Last_Login='" + currdate + "',sys_name='" + sysname + "',sys_ip_add='" + userip + "' where User_Id='" + textuser + "'");
                            //sgen.execute_cmd(userCode, "commit");

                            //Session[gid + "_gid"] = gid;          




                            try
                            {
                                var NewVersion = new Version(model[0].Col13);
                                var OldVersion = new Version(model[0].Col12);
                                var result = NewVersion.CompareTo(OldVersion);
                                if (result > 0)
                                {
                                    sgen.Alter_Table(userCode, client_id, unitid_mst, userid_mst);
                                    sgen.execute_cmd(userCode, "update company_profile set version='" + NewVersion + "',ver_name='" + NewVersion + "' where type='CG'");
                                }

                            }
                            catch (Exception err) { }


                            return RedirectToAction("landing", new { m_id = EncryptDecrypt.Encrypt(MyGuid) });
                            //Response.Redirect("/home/landing", true);
                        }
                    }
                    catch (Exception ex)
                    {
                        model[0].Col16 = ex.Message;
                    }
                }
                else if (command == "clear")
                {
                    sgen.Expireall();
                    Session.RemoveAll();
                    Session.Abandon();
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
                    Response.Cache.SetNoStore();
                    var cacheManager = new OutputCacheManager();
                    cacheManager.RemoveItems();
                    return Redirect(Request.UrlReferrer.ToString());
                }
                else if (command == "SMS")
                {
                    FillMst();
                    //Mailer mailer = new Mailer();
                    //List<MailObjs> mails = new List<MailObjs>();

                    //for (int a = 0; a < 1000; a++)
                    //{
                    //    MailObjs mail = new MailObjs();

                    //    mail.fromAddress = "gsthelp001@gmail.com";
                    //    mail.smtp = "smtp.gmail.com";
                    //    mail.port = 25;
                    //    mail.ssl = false;
                    //    mail.toAddress = "ramrattan71@gmail.com";
                    //    mail.body = "Mail Number" + a;
                    //    mail.subject = "Mail Number" + a;
                    //    mails.Add(mail);

                    //}

                    //mailer.StartEmailRun(mails);
                    //string SmsUser = "", SmsPass = "", SmsSender = "", SmsRoute = "";

                    //clientid_mst = "001";
                    //unitid_mst = "001001";
                    //try
                    //{
                    //    sgenFun sgen = new sgenFun();


                    //    mq = "select a.rec_id,a.client_name,a.master_id,a.master_name,a.master_entby as ent_by,a.master_entdate as ent_date,a.vch_date,a.status,a.master_editby edit_by,a.master_editdate as edit_date," +
                    //           "a.client_id,a.client_unit_id,a.cont_person_name,a.cont_person_email,a.group_refrence_number,a.group_id,to_char(convert_tz(Inactive_date,'UTC','+05:30'),'%d/%m/%Y %T') " +
                    //           "from master_setting a where type='API' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
                    //    DataTable dt = sgen.getdata(userCode, mq);
                    //    if (dt.Rows.Count > 0)
                    //    {
                    //        SmsUser = dt.Rows[0]["cont_person_name"].ToString();
                    //        SmsPass = dt.Rows[0]["cont_person_email"].ToString();
                    //        SmsSender = dt.Rows[0]["group_refrence_number"].ToString();
                    //        SmsRoute = dt.Rows[0]["group_id"].ToString();

                    //    }

                    //}
                    //catch (Exception ex)
                    //{
                    //    //Log Exception
                    //}


                    //BulkSMS bulk = new BulkSMS();
                    //List<SMSObjs> SmSs = new List<SMSObjs>();


                    //SMSObjs sms = new SMSObjs();
                    //sms.MobileNos = "9999565112";
                    //sms.SmsText = "Hello Ram";
                    //string baseurl = "http://login.smsmantra.online/api/mt/SendSMS?user=" + Uri.EscapeUriString(SmsUser) +
                    //      "&password=" + Uri.EscapeUriString(SmsPass) + "&senderid=" + Uri.EscapeUriString(SmsSender) +
                    //      "&channel=Trans&DCS=8&flashsms=0&number=" + Uri.EscapeUriString(sms.MobileNos) +
                    //      "&text=" + System.Uri.EscapeUriString(sms.SmsText) + "&route=" + Uri.EscapeUriString(SmsRoute) + "";
                    //sms.ApiUrl = baseurl;
                    //SmSs.Add(sms);


                    //sms = new SMSObjs();
                    //sms.MobileNos = "9034904743";
                    //sms.SmsText = "Hello Rattan";
                    //baseurl = "http://login.smsmantra.online/api/mt/SendSMS?user=" + Uri.EscapeUriString(SmsUser) +
                    //     "&password=" + Uri.EscapeUriString(SmsPass) + "&senderid=" + Uri.EscapeUriString(SmsSender) +
                    //     "&channel=Trans&DCS=8&flashsms=0&number=" + Uri.EscapeUriString(sms.MobileNos) +
                    //     "&text=" + System.Uri.EscapeUriString(sms.SmsText) + "&route=" + Uri.EscapeUriString(SmsRoute) + "";
                    //sms.ApiUrl = baseurl;

                    //sms = new SMSObjs();
                    //sms.MobileNos = "9034904743";
                    //sms.SmsText = "Hello Baghel";
                    //baseurl = "http://login.smsmantra.online/api/mt/SendSMS?user=" + Uri.EscapeUriString(SmsUser) +
                    //     "&password=" + Uri.EscapeUriString(SmsPass) + "&senderid=" + Uri.EscapeUriString(SmsSender) +
                    //     "&channel=Trans&DCS=8&flashsms=0&number=" + Uri.EscapeUriString(sms.MobileNos) +
                    //     "&text=" + System.Uri.EscapeUriString(sms.SmsText) + "&route=" + Uri.EscapeUriString(SmsRoute) + "";
                    //sms.ApiUrl = baseurl;
                    //SmSs.Add(sms);



                    //bulk.GetUsersInParallelFixed(SmSs);
                    //sgen.Send_Sms(userCode, clientid_mst, unitid_mst, "9999565112", "Jai Hind");
                    //sgen.get_config(userCode, "000001");
                }
                else if (command == "forget")
                {
                    return RedirectToAction("forgot_pswd", new { m_id = EncryptDecrypt.Encrypt(MyGuid) });
                }
            }
            catch (Exception err)
            {
                var LineNumber = new StackTrace(err, true).GetFrame(0).GetFileLineNumber();
                ViewBag.scripCall += "showmsgJS(1, '" + err.Message.Replace("'", "") + " At Line number " + LineNumber + "', 1);";
            }
            return View(model);
        }
        #endregion

        #region ftsignup
        [GZipOrDeflate]
        public ActionResult ftsignup()
        {
            List<Tmodelmain> model = new List<Tmodelmain>();
            Tmodelmain tm = new Tmodelmain();
            try
            {
                var sid = Session.SessionID;
                userCode = Multiton.getUserCode();
                byte[] gb = Guid.NewGuid().ToByteArray();
                int i = BitConverter.ToInt32(gb, 0);
                MyGuid = i.ToString();

                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
                Response.Cache.SetNoStore();
                var cacheManager = new OutputCacheManager();
                cacheManager.RemoveItems();

                Multiton.SetCookie(MyGuid, "userCode", userCode);
                Multiton.SetSession(MyGuid, "userCode", userCode);
                FillMst(MyGuid);
                Multiton.SetSession(MyGuid, "cocd_mst", "-");
                tm.Col15 = MyGuid;
            }
            catch (Exception err) { }
            model.Add(tm);
            return View(model);
        }
        [HttpPost]
        public ActionResult ftsignup(List<Tmodelmain> model, string command)
        {
            try
            {
                if (command == "Login")
                {
                    try
                    {
                        FillMst(model[0].Col15);
                        {
                            string mq = "select company_status,company_name,Company_No_Of_User,subdomain,version,ver_name,company_code,client_type from company_profile where type='CG' and company_profile_id='000'";
                            DataTable dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count == 0)
                            {
                                model[0].Col16 = "No company found";
                                return View(model);
                            }
                            if (dt.Rows[0]["company_status"].ToString().Equals("0"))
                            {
                                model[0].Col16 = "No activated company found";
                                return View(model);
                            }
                            sgen.SetCookie(MyGuid, "controls_mst", EncryptDecrypt.Decrypt(dt.Rows[0]["Company_No_Of_User"].ToString()));
                            sgen.SetCookie(MyGuid, "subdomain_mst", dt.Rows[0]["subdomain"].ToString());
                            sgen.SetCookie(MyGuid, "cg_com_name", dt.Rows[0]["company_code"].ToString());
                            sgen.SetCookie(MyGuid, "rrb", "home");
                            sgen.SetSession(MyGuid, "rrb", dt);

                            string pwd = EncryptDecrypt.Encrypt(model[0].Col22);
                            string textuser = model[0].Col21;
                            string txtpasswrd = pwd;
                            string client_unitid = "";
                            string client_id = "";

                            if (textuser == null && pwd == "")
                            {
                                model[0].Col16 = "Please Check your Username OR Password"; return View(model);
                            }
                            dt = new DataTable();
                            mq = "SELECT vch_num,user_id,password,client_id,status,role,client_unit_id,ent_by,profilepic,profilepic_path,type " +
                                "FROM user_details WHERE UPPER(trim(user_id))='" + textuser.Trim().ToUpper() + "' and password='" + pwd + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count < 1) { model[0].Col16 = "Please Check your Username OR Password"; return View(model); }
                            if (dt.Rows[0]["status"].ToString() == "inactive")
                            {
                                model[0].Col21 = "";
                                model[0].Col16 = "Your Account Is Not Active Please Contact Your Admin";
                                return View(model);
                            }

                            string rola = dt.Rows[0]["role"].ToString();
                            sgen.SetCookie(MyGuid, "utype_mst", dt.Rows[0]["type"].ToString());
                            if (dt.Rows[0]["type"].ToString().Trim().ToUpper().Equals("STP")) sgen.SetCookie(MyGuid, "role_mst", "stpmain-1");
                            else if (dt.Rows[0]["type"].ToString().Trim().ToUpper().Equals("STD")) sgen.SetCookie(MyGuid, "role_mst", "stdmain-1");
                            else sgen.SetCookie(MyGuid, "role_mst", dt.Rows[0]["role"].ToString());
                            sgen.SetCookie(MyGuid, "username_mst", dt.Rows[0]["user_id"].ToString());
                            sgen.SetCookie(MyGuid, "userid_mst", dt.Rows[0]["vch_num"].ToString());

                            client_id = Convert.ToString(dt.Rows[0]["client_id"]);
                            client_unitid = Convert.ToString(dt.Rows[0]["client_unit_id"]);

                            sgen.SetCookie(MyGuid, "pp_filename", dt.Rows[0]["profilepic"].ToString());
                            sgen.SetCookie(MyGuid, "pp_filepath", dt.Rows[0]["profilepic_path"].ToString());

                            if (client_id.ToString().Trim().ToUpper() == "OWNER" || client_unitid.ToString().Trim().ToUpper() == "OWNER")
                            {
                                clientid_mst = "001";
                                unitid_mst = "001001";
                                sgen.SetCookie(MyGuid, "clientid_mst", "001");
                                sgen.SetCookie(MyGuid, "unitid_mst", "001001");
                            }
                            else
                            {
                                List<string> mylist1 = client_unitid.Split(',').Distinct().OrderBy(x => x).ToList();
                                int ct1 = mylist1.Count;
                                sgen.SetCookie(MyGuid, "unitid_mst", mylist1[0].ToString());

                                unitid_mst = mylist1[0].ToString();
                                clientid_mst = unitid_mst.Substring(0, 3);
                                sgen.SetCookie(MyGuid, "clientid_mst", clientid_mst);
                            }

                            if (rola.ToString().Trim().ToUpper() == "OWNER") mq = "select (case when max(company_status)='1' then 'active' else 'inactive' end) mval from company_profile ";
                            else mq = "select (case when max(company_status)='1' then 'active' else 'inactive' end) mval from company_profile where find_in_set(company_profile_id,'" + client_id + "')";
                            mq = sgen.seekval(userCode, mq, "mval");
                            if (mq == "inactive")
                            {
                                model[0].Col16 = "Your all Assigned Companies are Deactivated or Disabled, Please contact your admin to activate";
                                return View(model);
                            }

                            //string currdate = myyear;
                            DataTable dtc = sgen.getdata(userCode, "select company_name,logo_name,logo_path,client_type from company_profile where company_profile_id='" + clientid_mst + "'");
                            sgen.SetCookie(MyGuid, "clientname_mst", dtc.Rows[0]["company_name"].ToString());
                            sgen.SetCookie(MyGuid, "cp_filename", dtc.Rows[0]["logo_name"].ToString());
                            sgen.SetCookie(MyGuid, "cp_filepath", dtc.Rows[0]["logo_path"].ToString());
                            string client_type = dtc.Rows[0]["client_type"].ToString();//Educational Inst.
                            string unitname_mst = sgen.getstring(userCode, "select unit_name from company_unit_profile where cup_id='" + unitid_mst + "'");
                            sgen.SetSession(MyGuid, "unitname_mst", unitname_mst);
                            mq = sgen.seekval(userCode, "SELECT COUNT(*) AS CNT FROM company_unit_profile where 1=1 and unit_status = '1' and company_profile_id='" + clientid_mst + "'", "cnt");
                            sgen.SetSession(MyGuid, "unitcnt", mq);

                            sgen.generateformats(userCode, clientid_mst);
                            string myyear = sgen.server_datetime_dt_local(userCode).ToString(sgen.Getdateformat(), CultureInfo.InvariantCulture);
                            DataTable dtyr = sgen.FindCurrentACYear(userCode, unitid_mst, clientid_mst);
                            if (dtyr.Rows.Count == 0)
                            {
                                dtyr = sgen.getdata(userCode, "SELECT academic_year_id,academic_year,TO_CHAR(to_date( to_char(from_date,'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldatetimeformat() + "') from_date," +
                                "TO_CHAR(to_date( to_char(to_date,'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldatetimeformat() + "') to_date FROM add_academic_year " +
                                "WHERE type='ACY' and client_id='" + clientid_mst + "' order by to_date desc");
                                if (dtyr.Rows.Count > 0)
                                {
                                    sgen.SetCookie(MyGuid, "Ac_Year_id", dtyr.Rows[0]["academic_year_id"].ToString());
                                    sgen.SetCookie(MyGuid, "Ac_Year", dtyr.Rows[0]["academic_year"].ToString());
                                    sgen.SetCookie(MyGuid, "Ac_From_Date", dtyr.Rows[0]["From_Date"].ToString());
                                    sgen.SetCookie(MyGuid, "Ac_To_Date", dtyr.Rows[0]["To_Date"].ToString());

                                    sgen.SetCookie(MyGuid, "year_to", dtyr.Rows[0]["To_Date"].ToString());
                                    sgen.SetCookie(MyGuid, "year_from", dtyr.Rows[0]["From_Date"].ToString());
                                    sgen.SetCookie(MyGuid, "com_yr", dtyr.Rows[0]["academic_year"].ToString());
                                    //sgen.SetCookie(MyGuid, "com_codeyear", ytab.Rows[0]["com_code"].ToString());
                                }
                                else
                                {
                                    model[0].Col16 = "Your Id is not Operatable in this Academic Year";
                                    return View(model);
                                }
                            }
                            else
                            {
                                sgen.SetCookie(MyGuid, "Ac_Year_id", dtyr.Rows[0]["academic_year_id"].ToString());
                                sgen.SetCookie(MyGuid, "Ac_Year", dtyr.Rows[0]["academic_year"].ToString());
                                sgen.SetCookie(MyGuid, "Ac_From_Date", dtyr.Rows[0]["From_Date"].ToString());
                                sgen.SetCookie(MyGuid, "Ac_To_Date", dtyr.Rows[0]["To_Date"].ToString());

                                sgen.SetCookie(MyGuid, "year_to", dtyr.Rows[0]["To_Date"].ToString());
                                sgen.SetCookie(MyGuid, "year_from", dtyr.Rows[0]["From_Date"].ToString());
                                sgen.SetCookie(MyGuid, "com_yr", dtyr.Rows[0]["academic_year"].ToString());
                            }
                            return RedirectToAction("landing", new { m_id = EncryptDecrypt.Encrypt(MyGuid) });
                        }
                    }
                    catch (Exception ex)
                    {
                        model[0].Col16 = ex.Message;
                    }
                }
                else if (command == "clear")
                {
                    sgen.Expireall();
                    Session.RemoveAll();
                    Session.Abandon();
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
                    Response.Cache.SetNoStore();
                    var cacheManager = new OutputCacheManager();
                    cacheManager.RemoveItems();
                    return Redirect(Request.UrlReferrer.ToString());
                }
                else if (command == "SMS")
                {
                    FillMst();
                }
                else if (command == "forget")
                {
                    return RedirectToAction("forgot_pswd", new { m_id = EncryptDecrypt.Encrypt(MyGuid) });
                }
            }
            catch (Exception err)
            {
                var LineNumber = new StackTrace(err, true).GetFrame(0).GetFileLineNumber();
                ViewBag.scripCall += "showmsgJS(1, '" + err.Message.Replace("'", "") + " At Line number " + LineNumber + "', 1);";
            }
            return View(model);
        }
        #endregion

        #region landing
        //[OutputCache(CacheProfile = "Cache30sec")]
        public ActionResult landing()
        {
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            FillMst(MyGuid);
            string m_module3 = "", ulevel_mst = "";
            if (userCode.Equals("")) return RedirectToAction(sgenFun.callbackmvcAct, sgenFun.callbackmvcCtr);
            if (Request.UrlReferrer == null) { return RedirectToAction(sgenFun.callbackmvcAct, sgenFun.callbackmvcCtr); }
            Icon_class ic = new Icon_class(MyGuid);
            //ic.insert_icons();
            role_class rc = new role_class();
            //rc.insert_roles();
            com_module_class cm = new com_module_class();
            //cm.insert_com_modules();



            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);


            userid_mst = sgen.GetCookie(MyGuid, "userid_mst");
            string utype_mst = sgen.GetCookie(MyGuid, "utype_mst");
            string username_mst = sgen.GetCookie(MyGuid, "username_mst");
            string clientid_mst = sgen.GetCookie(MyGuid, "clientid_mst");
            string unitid_mst = sgen.GetCookie(MyGuid, "unitid_mst");
            string role_mst = sgen.GetCookie(MyGuid, "role_mst");
            cg_com_name = sgen.GetCookie(MyGuid, "cg_com_name");
            clientname_mst = sgen.GetCookie(MyGuid, "clientname_mst");
            string com_yr = sgen.GetCookie(MyGuid, "com_yr");
            //mq = "select company_name from company_profile where type='CG' ";
            //clientname = sgen.getstring(userCode, mq);
            var namse = sgen.GetSession(MyGuid, "rrb");
            List<Tmodelmain> model = new List<Tmodelmain>();


            try
            {
                string c = "";
                string where = "";

                string[] aa = role_mst.Split(',');
                foreach (string b in aa)
                {
                    if (b != "")
                    {
                        c += b.Split('-')[0] + ",";
                    }
                }

                //Rama
                where = " and m_module3 in ('" + c.Replace(",", "','") + "')";

                if (role_mst.ToString().Trim().ToUpper() == "OWNER")
                {

                    //if (userCode == "satech" || userCode == "test" || userCode == "satechdemo" || userCode == "name")
                    //{ mq = "SELECT m.m_module1,m.m_module2,m.m_module3,m.m_id,m_name,m.m_link,m.m_textfront,m.m_textback," +
                    //        " (CASE WHEN cm.mod_to<sysdate then 0 ELSE 1 END) status" +
                    //        " FROM menus m inner join com_module cm on cm.mod_id=m.m_id where m_level='1' " +
                    //        "and  trim(upper(cm.com_code))='" + userCode.ToUpper().Trim() + "'  ";
                    //}

                    //else
                    {
                        mq = "select distinct m.m_module1,m.m_module2,m.m_module3, m.m_id,m.m_link ,m.m_name ,m.m_textfront,m.m_textback" +
                            " ,(CASE WHEN cm.mod_to<sysdate then 0 ELSE (case when ra.r_id is null then 2 else 1 end) END) status from menus m " +
                            "join com_module cm on cm.mod_id=m.m_id inner join (select * from role_authority ra) ra on m.m_id=ra.m_id where m.m_level='1'" +
                            " and  trim(upper(cm.com_code))='" + userCode.ToUpper().Trim() + "'";
                    }
                }
                else
                {

                    //if (userCode == "satech" || userCode == "test" || userCode == "satechdemo" || userCode == "name")
                    //{
                    //   mq = "SELECT m.m_module1,m.m_module2,m.m_module3,m.m_id,m_name,m.m_link,m.m_textfront,m.m_textback," +
                    //        " (CASE WHEN cm.mod_to<sysdate then 0 ELSE 1 END) status as status " +
                    //        " FROM menus m inner join com_module cm on cm.mod_id=m.m_id where m_level='1' " +
                    //        "and  trim(upper(cm.com_code))='" + userCode.ToUpper().Trim() + "'" + where + "";
                    //}
                    ////else mq2 = "select distinct ra.m_id,ra.m_name,m.m_link,(CASE WHEN cm.mod_to<UTC_TIMESTAMP() then 0 ELSE 1 END) status from role_authority ra join menus m on m.m_id=ra.m_id join com_module cm on cm.mod_id=ra.m_id where ra.u_id in ('" + role_mst.Replace(",", "','") + "')";
                    //else
                    {
                        mq = "select distinct m.m_module1,m.m_module2,m.m_module3, m.m_id,m.m_link ,m.m_name ,m.m_textfront,m.m_textback " +
                            ",(CASE WHEN cm.mod_to<sysdate then 0 ELSE (case when ra.r_id is null then 2 else 1 end) END) status" +
                            " from menus m join com_module cm on cm.mod_id=m.m_id inner join (select * from role_authority ra where ra.u_id" +
                            " in ('" + role_mst.Replace(",", "','") + "')) ra on m.m_id=ra.m_id  and  trim(upper(cm.com_code))='" + userCode.ToUpper().Trim() + "'";
                    }
                }

                DataTable dtmod3 = sgen.getdata(userCode, mq);

                if (dtmod3.Rows.Count == 1)
                {
                    if (dtmod3.Rows[0]["status"].ToString() == "0")
                    {
                        ViewBag.scripCall += "mytoast('warning', 'toast-top-right', 'Module is not Active for You! Please Contact Administrator');disableForm(); ";
                    }
                    else
                    {
                        var modname = dtmod3.Rows[0]["m_module3"].ToString();
                        string m_id = dtmod3.Rows[0]["m_id"].ToString();
                        sgen.SetCookie(MyGuid, "m_id", dtmod3.Rows[0]["m_id"].ToString());
                        sgen.SetCookie(MyGuid, "m_name", dtmod3.Rows[0]["m_name"].ToString());
                        sgen.SetCookie(MyGuid, "m_module1", dtmod3.Rows[0]["m_module1"].ToString());
                        sgen.SetCookie(MyGuid, "m_module2", dtmod3.Rows[0]["m_module2"].ToString());
                        sgen.SetCookie(MyGuid, "m_module3", dtmod3.Rows[0]["m_module3"].ToString());
                        mq = dtmod3.Rows[0]["m_module3"].ToString();

                        if (role_mst.ToLower().Trim().Equals("owner"))
                        {
                            sgen.SetCookie(MyGuid, "ulevel_mst", "0");
                        }
                        else
                        {
                            List<string> rols = role_mst.Split(',').ToList();
                            sgen.SetCookie(MyGuid, "ulevel_mst", rols.Where(w => w.Split('-')[0].ToLower().Equals(modname.ToLower())).FirstOrDefault().ToString().Split('-')[1]);
                        }

                        m_module3 = sgen.GetCookie(MyGuid, "m_module3");
                        ulevel_mst = sgen.GetCookie(MyGuid, "ulevel_mst");

                        mq = "select userid from urights where type='K98' and userid='" + userid_mst + "'";
                        mq = sgen.seekval(userCode, mq, "userid");
                        sgen.SetCookie(MyGuid, "urights_mst", mq);
                        if (role_mst.ToLower().Trim().Equals("owner"))
                        {
                            sgen.SetCookie(MyGuid, "urights_mst", "owner");
                        }
                        return RedirectToAction("dashboard", new { @mid = EncryptDecrypt.Encrypt(m_id).ToString(), @m_id = EncryptDecrypt.Encrypt(MyGuid).ToString() });

                    }
                }


                if (dtmod3.Rows.Count > 0)
                {
                    foreach (DataRow dr0 in dtmod3.Rows)
                    {


                        Tmodelmain tm = new Tmodelmain();
                        tm.Col11 = clientname_mst.Trim().ToUpper();
                        tm.Col12 = username_mst.Trim().ToUpper();
                        tm.Col13 = com_yr.Trim().ToUpper();
                        tm.Col20 = dr0["m_id"].ToString();
                        tm.Col21 = dr0["status"].ToString();
                        tm.Col22 = dr0["m_name"].ToString();
                        tm.Col23 = dr0["m_textfront"].ToString();
                        tm.Col24 = EncryptDecrypt.Encrypt(dr0["m_id"].ToString());
                        tm.Col25 = dr0["m_name"].ToString();
                        tm.Col26 = dr0["m_textback"].ToString();
                        tm.Col15 = MyGuid;
                        model.Add(tm);
                    }
                }
                else
                {

                    sgen.SetSession(MyGuid, "loginmsg", "Module is not Active for You! Please Contact Administrator");
                    return RedirectToAction("login");
                }

                m_module3 = sgen.GetCookie(MyGuid, "m_module3");
                ulevel_mst = sgen.GetCookie(MyGuid, "ulevel_mst");
                mq = "select userid from urights where type='K98' and userid='" + userid_mst + "'";
                mq = sgen.seekval(userCode, mq, "userid");
                sgen.SetCookie(MyGuid, "urights_mst", mq);
                if (role_mst.ToLower().Trim().Equals("owner"))
                {
                    sgen.SetCookie(MyGuid, "urights_mst", "owner");
                }

            }
            catch (Exception ex)
            {
                sgen.showmsg(1, ex.Message.ToString(), 0);
            }
            model[0].Col15 = MyGuid;
            ModelState.Clear();
            return View(model);

        }
        [HttpPost]
        public ActionResult landing(List<Tmodelmain> model, string command, string mid)
        {
            try
            {
                FillMst();
                if (model == null) model = new List<Tmodelmain>();
                model.Add(new Tmodelmain { Col15 = MyGuid });
                //if (mid != null && mid.Trim() != "")
                //{
                //    FillMst(mid); model.Add(new Tmodelmain { Col15 = mid });
                //}
                //else FillMst(model[0].Col15);
                //try
                //{
                //    MyGuid = model[0].Col15;
                //}
                //catch (Exception err)
                //{
                //    MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);

                //}

                string m_id = "";
                userid_mst = sgen.GetCookie(MyGuid, "userid_mst");
                string utype_mst = sgen.GetCookie(MyGuid, "utype_mst");
                string username_mst = sgen.GetCookie(MyGuid, "username_mst");
                string clientid_mst = sgen.GetCookie(MyGuid, "clientid_mst");
                string unitid_mst = sgen.GetCookie(MyGuid, "unitid_mst");
                string role_mst = sgen.GetCookie(MyGuid, "role_mst");
                cg_com_name = sgen.GetCookie(MyGuid, "cg_com_name");
                string com_yr = sgen.GetCookie(MyGuid, "com_yr");
                List<string> charts = new List<string>();


                foreach (var key in Session.Keys)
                {
                    if (key.ToString().ToUpper().Contains("_CHART")) charts.Add(key.ToString());
                }
                foreach (var ch in charts)
                {
                    Session[ch.ToString()] = null;
                }

                if (command == "Logout")
                {
                    return RedirectToAction("login", "home");
                }
                else
                {
                    if (mid != null && !mid.Trim().Equals(""))
                    {
                        m_id = mid;
                    }
                    else
                    {
                        m_id = model[0].Col31;

                    }
                    //string m_id = command.Split('$')[1].ToString(); ;
                    if (role_mst.ToString().Trim().ToUpper() == "OWNER")
                    {
                        mq = "select m.m_id,m.m_name,m.m_module1,m.m_module2,m.m_module3,(CASE WHEN cm.mod_to<UTC_TIMESTAMP() then 0 ELSE (case when ra.r_id is null then 2 else 1 end) END) as status from menus m join " +
                        "com_module cm on cm.mod_id=m.m_id left join (select * from role_authority ) ra on m.m_id=ra.m_id where m.m_id='" + m_id + "' and  trim(upper(cm.com_code))='" + userCode.ToUpper().Trim() + "'";
                    }
                    else
                    {
                        mq = "select m.m_id,m.m_name,m.m_module1,m.m_module2,m.m_module3,(CASE WHEN cm.mod_to<UTC_TIMESTAMP() then 0 ELSE (case when ra.r_id is null then 2 else 1 end) END) as status from menus m join " +
                      "com_module cm on cm.mod_id=m.m_id left join (select * from role_authority ra where ra.u_id" +
                              " in ('" + role_mst.Replace(",", "','") + "')) ra on m.m_id=ra.m_id where m.m_id='" + m_id + "' and  trim(upper(cm.com_code))='" + userCode.ToUpper().Trim() + "'";
                    }
                    DataTable dtmenu = sgen.getdata(userCode, mq);

                    if (dtmenu.Rows.Count > 0)
                    {

                        if (dtmenu.Rows[0]["status"].ToString() == "0" || dtmenu.Rows[0]["status"].ToString() == "2")
                        {
                            ViewBag.scripCall += "mytoast('warning', 'toast-top-right', 'Module is not Active for You! Please Contact Administrator');disableForm(); ";
                        }
                        else
                        {
                            var modname = dtmenu.Rows[0]["m_module3"].ToString();
                            sgen.SetCookie(MyGuid, "m_id", dtmenu.Rows[0]["m_id"].ToString());
                            sgen.SetCookie(MyGuid, "m_name", dtmenu.Rows[0]["m_name"].ToString());
                            sgen.SetCookie(MyGuid, "m_module1", dtmenu.Rows[0]["m_module1"].ToString());
                            sgen.SetCookie(MyGuid, "m_module2", dtmenu.Rows[0]["m_module2"].ToString());
                            sgen.SetCookie(MyGuid, "m_module3", modname);
                            if (role_mst.ToLower().Trim().Equals("owner"))
                            {
                                sgen.SetCookie(MyGuid, "ulevel_mst", "0");
                            }
                            else
                            {
                                List<string> rols = role_mst.Split(',').ToList();
                                sgen.SetCookie(MyGuid, "ulevel_mst", rols.Where(w => w.Split('-')[0].ToLower().Equals(modname.ToLower())).FirstOrDefault().ToString().Split('-')[1]);
                            }
                            mq = dtmenu.Rows[0]["m_module3"].ToString();

                            ViewBag.mid = EncryptDecrypt.Encrypt(m_id).ToString();
                            return RedirectToAction("dashboard", new { @mid = EncryptDecrypt.Encrypt(m_id).ToString(), @m_id = EncryptDecrypt.Encrypt(MyGuid).ToString() });
                        }
                    }

                }
            }
            catch (Exception err)
            {
                var LineNumber = new StackTrace(err, true).GetFrame(0).GetFileLineNumber();
                ViewBag.scripCall += "showmsgJS(1, '" + err.Message.Replace("'", "") + " At Line number " + LineNumber + "', 1);";
            }
            return View(model);
        }

        #endregion

        #region chng_unit

        public ActionResult chng_unit()
        {
            string where = "";
            if (userCode.Equals("")) return RedirectToAction(sgenFun.callbackmvcAct, sgenFun.callbackmvcCtr);
            if (Request.UrlReferrer == null) { return RedirectToAction(sgenFun.callbackmvcAct, sgenFun.callbackmvcCtr); }
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            m_id_mst = EncryptDecrypt.Decrypt(Request.QueryString["m_id"].ToString());

            model.Add(new Tmodelmain
            {
                Col1 = "",
                Col15 = m_id_mst
            });
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<SelectListItem> mod2 = new List<SelectListItem>();
            List<SelectListItem> mod3 = new List<SelectListItem>();
            List<SelectListItem> mod4 = new List<SelectListItem>();

            //mod1.Add(new SelectListItem { Text = "Active", Value = "1" });
            //mod1.Add(new SelectListItem { Text = "Inactive", Value = "0" });
            //DataTable dtcomp = new DataTable();
            //try
            //{
            //    mq = sgen.seekval(userCode, "select client_id from user_details where rec_id='" + Convert.ToInt32(userid_mst) + "'", "client_id");
            //    if (!role_mst.ToUpper().Equals("OWNER")) where = " and company_profile_id in ('" + mq.Replace(",", "','") + "')";


            //    mq = "select '------Select------' as id,'------Select------' as txt from dual union all SELECT " +
            //        "Company_Profile_Id, (company_name|| ' ('|| company_profile_id||')') as nameid FROM company_profile " +
            //        "where type='CP' " + where + " ORDER BY 2";
            //    dtcomp = sgen.getdata(userCode, mq);
            //    mod2 = sgen.dt_to_selectlist(dtcomp);

            //}
            //catch (Exception ex) { }



            model[0].TList1 = mod1;
            model[0].TList2 = mod2;
            model[0].TList3 = mod3;
            model[0].TList4 = mod1;
            model[0].SelectedItems1 = new string[] { "" };
            model[0].SelectedItems2 = new string[] { "" };
            model[0].SelectedItems3 = new string[] { "" };
            model[0].SelectedItems4 = new string[] { "" };
            ModelState.Clear();
            return View(model);

        }
        [HttpPost]
        public ContentResult chng_unit(List<Tmodelmain> model, string command, string mid)
        {


            FillMst(model[0].Col15);

            List<string> charts = new List<string>();


            foreach (var key in Session.Keys)
            {
                if (key.ToString().ToUpper().Contains("_CHART")) charts.Add(key.ToString());
            }
            foreach (var ch in charts)
            {
                Session[ch.ToString()] = null;
            }
            string[] txtx = mid.Split(new string[] { "###" }, StringSplitOptions.None);

            sgen.SetSession(MyGuid, "clientid_mst", model[0].SelectedItems2.FirstOrDefault());
            sgen.SetSession(MyGuid, "unitid_mst", model[0].SelectedItems3.FirstOrDefault());
            sgen.SetSession(MyGuid, "clientname_mst", txtx[1].ToString());
            sgen.SetSession(MyGuid, "unitname_mst", txtx[2].Substring(0, txtx[2].Length - 8));
            sgen.SetSession(MyGuid, "yr_mst", txtx[3].ToString());
            sgen.SetSession(MyGuid, "Ac_Year_id", model[0].SelectedItems4.FirstOrDefault());
            sgen.SetSession(MyGuid, "Ac_Year", txtx[3].ToString());

            sgen.SetCookie(MyGuid, "clientid_mst", model[0].SelectedItems2.FirstOrDefault());
            sgen.SetCookie(MyGuid, "unitid_mst", model[0].SelectedItems3.FirstOrDefault());
            //            sgen.SetCookie(MyGuid, "yr_mst", ddl_year.SelectedItem.Text);
            sgen.SetCookie(MyGuid, "Ac_Year_id", model[0].SelectedItems4.FirstOrDefault());
            sgen.SetCookie(MyGuid, "Ac_Year", txtx[3].ToString());
            sgen.SetCookie(MyGuid, "clientname_mst", txtx[1].ToString());
            sgen.SetCookie(MyGuid, "unitname_mst", txtx[2].Substring(0, txtx[2].Length - 8));
            sgen.SetCookie(MyGuid, "yr_mst", txtx[3].ToString());

            //mq = sgen.seekval(userCode, "select m_module3 from menus where m_module3='edumain'", "m_module3");
            //if (mq.Trim() != "0")
            //{

            //DataTable dtyr = sgen.getdata(userCode, "SELECT academic_year_id,academic_year,TO_CHAR(to_date( to_char(from_date,'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldatetimeformat() + "') from_date," +
            //"TO_CHAR(to_date( to_char(to_date,'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldatetimeformat() + "') to_date FROM add_academic_year WHERE type='ACY' " +
            //"and client_id='" + model[0].SelectedItems2.FirstOrDefault() + "' and client_unit_id='" + model[0].SelectedItems3.FirstOrDefault() + "' and academic_year_id='" + model[0].SelectedItems4.FirstOrDefault() + "' " +
            //"and rownum=1 order by to_date desc");

            DataTable dtyr = sgen.getdata(userCode, "SELECT academic_year_id,academic_year,TO_CHAR(to_date( to_char(from_date,'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldatetimeformat() + "') from_date," +
                "TO_CHAR(to_date( to_char(to_date,'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldatetimeformat() + "') to_date FROM add_academic_year WHERE type='ACY' " +
                "and client_id='" + model[0].SelectedItems2.FirstOrDefault() + "' and academic_year_id='" + model[0].SelectedItems4.FirstOrDefault() + "' " +
                "and rownum=1 order by to_date desc");
            if (dtyr.Rows.Count > 0)
            {
                sgen.SetCookie(MyGuid, "Ac_Year_id", dtyr.Rows[0]["academic_year_id"].ToString());
                sgen.SetCookie(MyGuid, "Ac_Year", dtyr.Rows[0]["academic_year"].ToString());
                sgen.SetCookie(MyGuid, "Ac_From_Date", dtyr.Rows[0]["From_Date"].ToString());
                sgen.SetCookie(MyGuid, "Ac_To_Date", dtyr.Rows[0]["To_Date"].ToString());
                sgen.SetCookie(MyGuid, "year_to", dtyr.Rows[0]["To_Date"].ToString());
                sgen.SetCookie(MyGuid, "year_from", dtyr.Rows[0]["From_Date"].ToString());

                //fyr
                sgen.SetCookie(MyGuid, "year_to", dtyr.Rows[0]["To_Date"].ToString());
                sgen.SetCookie(MyGuid, "year_from", dtyr.Rows[0]["From_Date"].ToString());
                sgen.SetCookie(MyGuid, "com_yr", dtyr.Rows[0]["academic_year"].ToString());
                //sgen.SetCookie(MyGuid, "com_codeyear", ytab.Rows[0]["com_code"].ToString());
            }

            //}
            //else
            //{

            //mq = "select TO_CHAR(to_date( to_char(year_to,'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldatetimeformat() + "') year_to ," +
            //"TO_CHAR(to_date( to_char(year_from,'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldatetimeformat() + "') year_from," +
            //                       "com_year,com_code  from com_year where  client_unit_id='" + model[0].SelectedItems3.FirstOrDefault() + "' AND vch_num='" + model[0].SelectedItems4.FirstOrDefault() + "' " +
            //                       "and rownum=1 order by year_to DESC";
            //DataTable ytab = sgen.getdata(userCode, mq);

            //sgen.SetCookie(MyGuid, "year_to", ytab.Rows[0]["year_to"].ToString());
            //sgen.SetCookie(MyGuid, "year_from", ytab.Rows[0]["year_from"].ToString());
            //sgen.SetCookie(MyGuid, "Ac_From_Date", ytab.Rows[0]["year_from"].ToString());
            //sgen.SetCookie(MyGuid, "Ac_To_Date", ytab.Rows[0]["year_to"].ToString());
            //sgen.SetCookie(MyGuid, "com_yr", ytab.Rows[0]["com_year"].ToString());
            //sgen.SetCookie(MyGuid, "com_codeyear", ytab.Rows[0]["com_code"].ToString());

            //}

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
            var cacheManager = new OutputCacheManager();
            cacheManager.RemoveItems();
            return Content("");

            //ModelState.Clear();
            //return View(model);
        }

        public JsonResult getdll_Data(string modelstr, string ddlid, string Myguid) // its a GET, not a POST
        {
            FillMst(Myguid);
            string com_code = "", status = "", where = "";
            List<Tmodelmain> model = new List<Tmodelmain>();
            DataTable dt = new DataTable();
            model = sgen.Make_Model(modelstr);

            List<SelectListItem> mod1 = new List<SelectListItem>();




            switch (ddlid)
            {
                case "0":
                    try
                    {
                        //mq = sgen.seekval(userCode, "select client_id from user_details where rec_id='" + Convert.ToInt32(userid_mst) + "'", "client_id");
                        mq = sgen.seekval(userCode, "select client_id from user_details where vch_num='" + userid_mst + "'", "client_id");
                        if (!role_mst.ToUpper().Equals("OWNER")) where = " and company_profile_id in ('" + mq.Replace(",", "','") + "')";


                        mq = "Select '------Select------' as id,'------Select------' as txt from dual union all SELECT " +
                            "Company_Profile_Id, (company_name|| ' ('|| company_profile_id||')') as nameid FROM company_profile " +
                            "where type='CP' " + where + " ORDER BY 2";
                        dt = sgen.getdata(userCode, mq);
                        mod1 = sgen.dt_to_selectlist(dt);

                    }
                    catch (Exception ex) { }
                    break;
                case "1":
                    try
                    {
                        status = model[0].SelectedItems1.FirstOrDefault();
                        //string mq0 = sgen.seekval(userCode, "select client_unit_id from user_details where rec_id='" + Convert.ToInt32(userid_mst) + "'", "client_unit_id");
                        string mq0 = sgen.seekval(userCode, "select client_unit_id from user_details where vch_num='" + userid_mst + "'", "client_unit_id");
                        if (!role_mst.ToUpper().Equals("OWNER")) where = " and cup_id in ('" + mq0.Replace(",", "','") + "')";

                        string comp_id = model[0].SelectedItems2.FirstOrDefault();

                        mq = "SELECT '------Select------' as id,'------Select------' as txt from dual union all select cup_id,upper(unit_name||' ('||Cup_Id||')') as nameid FROM company_unit_profile where " +
                            "company_profile_id='" + comp_id + "'" + where + " and unit_status=" + Convert.ToInt32(status.Trim()) + " order by 2";
                        dt = sgen.getdata(userCode, mq);
                        mod1 = sgen.dt_to_selectlist(dt);

                    }
                    catch (Exception ex) { }
                    break;
                case "2":
                    try
                    {
                        mq = sgen.seekval(userCode, "select m_module3 from menus where m_module3='edumain'", "m_module3");
                        //if (mq.Trim() != "0")
                        //{
                        //mq1 = "select '0' as academic_year_id,'--Select--' as academic_year from dual union ALL select academic_year_id," +
                        //    "academic_year from add_academic_year where type='ACY' and client_id = '" + model[0].SelectedItems2.FirstOrDefault() + "' and " +
                        //    "client_unit_id = '" + model[0].SelectedItems3.FirstOrDefault() + "'";
                        mq1 = "select '0' as academic_year_id,'--select--' as academic_year from dual union all select academic_year_id," +
                            "academic_year from add_academic_year where type='ACY' and client_id = '" + model[0].SelectedItems2.FirstOrDefault() + "'";
                        dt = sgen.getdata(userCode, mq1);
                        //if (dt.Rows.Count <= 1)
                        //{
                        //    mq1 = "select '0' as vch_num,'--Select--' as com_year from dual union ALL select vch_num,com_year from com_year " +
                        //    //"where client_id='" + model[0].SelectedItems2.FirstOrDefault() + "' and client_unit_id='" + model[0].SelectedItems3.FirstOrDefault() + "'";
                        //    "where client_id='" + model[0].SelectedItems2.FirstOrDefault() + "'";
                        //    dt = sgen.getdata(userCode, mq1);
                        //}
                        //}
                        //else
                        //{
                        //    mq1 = "select '0' as vch_num,'--Select--' as com_year from dual union ALL select vch_num,com_year from com_year " +
                        //        "where client_id='" + model[0].SelectedItems2.FirstOrDefault() + "' and client_unit_id='" + model[0].SelectedItems3.FirstOrDefault() + "'";
                        //    dt = sgen.getdata(userCode, mq1);
                        //}
                        mod1 = sgen.dt_to_selectlist(dt);

                    }
                    catch (Exception ex) { }
                    break;
            }

            return Json(mod1, JsonRequestBehavior.AllowGet);
        }
        #endregion
        ///////===================mvcctrls=====================
        #region ctrls
        public ActionResult ctrls()
        {
            FillMst();
            if (userCode.Equals("")) return RedirectToAction(sgenFun.callbackmvcAct, sgenFun.callbackmvcCtr);
            if (Request.UrlReferrer == null) { return RedirectToAction(sgenFun.callbackmvcAct, sgenFun.callbackmvcCtr); }
            ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.vsave = "disabled='disabled'";
            ViewBag.scripCall = "disableForm();";

            List<Tmodelmain> model = new List<Tmodelmain>();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<SelectListItem> mod2 = new List<SelectListItem>();
            Tmodelmain tm1 = new Tmodelmain();

            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            m_id_mst = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);

            tm1.Col9 = "MVC CONTROLS"; tm1.Col12 = "CTR";
            tm1.Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";


            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = m_id_mst.Trim();
            tm1.Col10 = "vehicle_master";
            tm1.Col13 = "Save";
            sgen.SetSession(MyGuid, "TR_MID", mid_mst.Trim());
            sgen.SetSession(MyGuid, "TYPE_MST", tm1.Col12.Trim());
            sgen.SetSession(MyGuid, "COND_MST", tm1.Col11.Trim());
            sgen.SetSession(MyGuid, "TBL_MST", tm1.Col10.Trim());

            tm1.TList1 = mod1;
            tm1.TList2 = mod2;
            tm1.SelectedItems1 = new string[] { "" };
            tm1.SelectedItems2 = new string[] { "" };
            model.Add(tm1);

            return View(model);
        }
        [HttpPost]
        public ActionResult ctrls(List<Tmodelmain> model, string command)
        {
            FillMst();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<SelectListItem> mod2 = new List<SelectListItem>();
            List<Tmodelmain> rpt_model = new List<Tmodelmain>();

            if (command == "New")
            {
                try
                {
                    sgen.SetSession(MyGuid, "EDMODE", "N");
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.scripCall = "enableForm();";
                    mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                    vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");

                    #region bindclass
                    DataTable dt = new DataTable();

                    mod1 = sgen.BindClass(userCode, unitid_mst);

                    //if (dt.Rows.Count > 0)
                    //{
                    //    foreach (DataRow dr in dt.Rows)
                    //    {
                    //        mod1.Add(new SelectListItem { Text = dr["Class"].ToString(), Value = dr["Class_id"].ToString() });
                    //    }
                    TempData[MyGuid + "_Tlist1"] = mod1;

                    //}
                    #endregion
                    #region bindsection


                    mod2 = sgen.BindSection(userCode, unitid_mst); ;
                    //if (dt.Rows.Count > 0)
                    //{
                    //    foreach (DataRow dr in dt.Rows)
                    //    {
                    //        mod2.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                    //    }
                    TempData[MyGuid + "_Tlist2"] = mod2;
                    //}
                    #endregion

                    model[0].TList1 = mod1;
                    model[0].TList2 = mod2;
                    model[0].SelectedItems1 = new string[] { "" };
                    model[0].SelectedItems2 = new string[] { "" };
                    model[0].Col17 = vch_num;
                    model[0].Col22 = "G";
                    model[0].Col13 = "Save";
                    var tm = model.FirstOrDefault();


                    DataTable fillgrid = new DataTable();
                    mq = @"select to_char(convert_tz(date1, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as date1,to_char(convert_tz(date2, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as date2,to_char(convert_tz(date3, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as date3,col1,col2,col3,col4,col5,col6,col7,col8,col9,col10,col11,col12,col13  from vehicle_master where client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "' and type='" + model[0].Col12 + "'";
                    fillgrid = sgen.getdata(userCode, mq);
                    if (fillgrid.Rows.Count > 0)
                    {
                        foreach (DataRow dr in fillgrid.Rows)
                        {
                            rpt_model.Add(new Tmodelmain
                            {

                                //Col18 = dr["date1"].ToString(),
                                //Col19 = dr["date2"].ToString(),
                                //Col20 = dr["date3"].ToString(),
                                Col21 = dr["col12"].ToString(),
                                // Col21 = dr["col1"].ToString(),


                                Col24 = dr["col9"].ToString(),
                                Col26 = dr["col11"].ToString(),
                                Col25 = dr["col10"].ToString(),
                                //Chk5= dr["col8"],
                                Col22 = dr["col6"].ToString(),
                                Col23 = dr["col7"].ToString(),

                                Col16 = tm.Col16,
                                TList1 = tm.TList1,
                                TList2 = tm.TList2,
                                Col9 = tm.Col9,
                                Col10 = tm.Col10,
                                Col11 = tm.Col11,
                                Col12 = tm.Col12,
                                Col13 = tm.Col13,
                                Col14 = tm.Col14,
                                Col15 = tm.Col15,

                                Col17 = tm.Col17.Trim(),
                                SelectedItems1 = tm.SelectedItems1,
                                SelectedItems2 = tm.SelectedItems2,





                            });

                        }
                        model = rpt_model;


                    }
                    else
                    {

                    }
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
                model = CallbackFun(btnval, "N", "ctrls", "Mst", model);
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
            }
            else if (command == "Save" || command == "Update")
            {
                try
                {

                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime_local(userCode);
                    currdate = sgen.Savedate(currdate, true);
                    string vch_num = "", master_id = "";
                    string cond = "";
                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {

                        isedit = true;
                        vch_num = model[0].Col17;


                    }

                    else
                    {
                        cond = "";
                        isedit = false;
                        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");

                    }

                    DataTable dataTable = sgen.getdata(userCode, "select  * from " + model[0].Col10.Trim() + "  where  1=2");
                    for (int i = 0; i < model.Count; i++)
                    {
                        DataRow dr = dataTable.NewRow();
                        dr["rec_id"] = "0";
                        dr["type"] = model[i].Col12;
                        dr["date1"] = currdate;
                        dr["client_id"] = clientid_mst;
                        dr["client_unit_id"] = unitid_mst;
                        dr["vch_num"] = vch_num.Trim();
                        dr["date2"] = sgen.Savedate(model[i].Col19, false);
                        dr["date3"] = sgen.Savedate(model[i].Col20, false);
                        dr["col1"] = string.Join(",", model[i].SelectedItems1);
                        dr["col12"] = string.Join(",", model[i].SelectedItems2);
                        dr["col25"] = model[i].Col21.Trim();
                        dr["col2"] = model[i].Chk1;
                        dr["col3"] = model[i].Chk2;
                        dr["col4"] = model[i].Chk3;
                        dr["col5"] = model[i].Chk4;
                        dr["col6"] = model[i].Col22.Trim();
                        dr["col7"] = model[i].Col23.Trim();

                        dr["col8"] = model[i].Chk5;
                        dr["col9"] = model[i].Col24;
                        dr["col10"] = model[i].Col25;

                        dr["col11"] = model[i].Col26;
                        dr["col13"] = model[i].Col27;

                        if (isedit)
                        {

                            dr["ent_by"] = model[0].Col3;
                            dr["ent_date"] = model[0].Col4;
                            dr["edit_by"] = model[0].Col5;
                            dr["edit_date"] = model[0].Col6;
                        }
                        else
                        {

                            dr["ent_by"] = userid_mst;
                            dr["ent_date"] = currdate;
                            dr["edit_by"] = "-";
                            dr["edit_date"] = currdate;

                        }
                        dataTable.Rows.Add(dr);
                    }

                    bool Result = sgen.Update_data(userCode, dataTable, model[0].Col10.Trim(), tmodel.Col8, isedit);
                    ViewBag.vnew = "";
                    ViewBag.vedit = "";
                    ViewBag.vsave = "disabled='disabled'";
                    if (Result == true)
                    {


                        ViewBag.scripCall = "showmsgJS(1, 'Record Saved Successfully', 1);disableForm();";

                        TempData[MyGuid + "_Tlist1"] = mod1;
                        TempData[MyGuid + "_Tlist2"] = mod2;
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col17 = "",
                            Col1 = "",
                            Col13 = "Save",
                            Col9 = "MVC CONTROLS",
                            TList1 = mod1,
                            TList2 = mod2,

                            SelectedItems1 = new string[] { "" },
                            SelectedItems2 = new string[] { "" },


                        }
                            );

                    }
                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
            }

            ModelState.Clear();
            return View(model);
        }
        #endregion

        #region mytodo
        public ActionResult mytodo()
        {
            FillMst();
            chkRef();
            List<Tmodelmain> model = new List<Tmodelmain>();
            Tmodelmain tm = new Tmodelmain();
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            m_id_mst = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);

            tm.Col9 = "MY TO DO";
            tm.Col10 = "Notifications";
            tm.Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            tm.Col12 = "TDL";
            tm.Col13 = "Save";
            tm.Col14 = mid_mst.Trim();
            tm.Col15 = m_id_mst.Trim();
            tm.Col100 = "Save & New";
            tm.Col122 = "<u>S</u>ave";
            tm.Col123 = "Save & Ne<u>w</u>";

            if (Request.QueryString["fstr"] != null)
            {
                mq0 = (Request.QueryString["fstr"]).Trim();
                if (mq0.ToUpper().Equals("NEW"))
                {
                    sgen.SetSession(MyGuid, "EDMODE", "N");
                    //ViewBag.vnew = "disabled='disabled'";
                    //ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    //ViewBag.scripCall += "enableForm();";
                    //mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + "  where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                    mq = "select max(to_number(vch_num)) as auto_genid from " + tm.Col10.Trim() + "  where type='" + tm.Col12.Trim() + "'" + tm.Col11.Trim() + "";
                    vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                    //model[0].Col16 = vch_num;
                    //model[0].Col21 = "P";//active or pending state
                    tm.Col16 = vch_num;
                    tm.Col21 = "P";//active or pending state
                    //ModelState.Clear();
                    //return View(model);
                }
                else
                {
                    sgen.SetSession(MyGuid, "SSEEKVAL", mq0);
                    model.Add(tm);
                    CallbackFun("EDIT", "Y", "mytodo", "Home", model);
                    //ViewBag.vnew = "disabled='disabled'";
                    //ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.scripCall += "enableForm();";
                    ModelState.Clear();
                    return View(model);
                }
            }

            model.Add(tm);
            return View(model);
        }

        [HttpPost]
        public ActionResult mytodo(List<Tmodelmain> model, string command)
        {
            try
            {
                string chk_dt = "N", fdt = "", tdt = "", bval = "";
                FillMst();
                command = command.Trim();
                if (command == "Cancel")
                {
                    return RedirectToAction("task_list2", new { @m_id = EncryptDecrypt.Encrypt(MyGuid), @mid = EncryptDecrypt.Encrypt("1004.2") });
                }
                else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
                {
                    try
                    {
                        var tmodel = model.FirstOrDefault();
                        string currdate = sgen.server_datetime(userCode);
                        ent_date = currdate;
                        DataTable dtstr = new DataTable();
                        dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                        if (model[0].Chk1 == true) chk_dt = "Y";
                        if (chk_dt.Trim().Equals("Y"))
                        {
                            if ((model[0].Col18 == "" || model[0].Col18 == null) && (model[0].Col19 == "" || model[0].Col19 == null))
                            {
                                ViewBag.scripCall += "showmsgJS(1,'Please fill both date',2);";
                                return View(model);
                            }
                            else if ((model[0].Col18 == "" || model[0].Col18 == null) && (model[0].Col19 == "" || model[0].Col19 == null))
                            {
                                ViewBag.scripCall += "showmsgJS(1,'Please fill both date',2);";
                                return View(model);
                            }
                            else
                            {
                                fdt = sgen.Savedate(model[0].Col18, true);
                                tdt = sgen.Savedate(model[0].Col19, true);
                            }
                        }
                        else
                        {
                            //fdt = "01/01/1900";
                            //tdt = "01/01/1900";
                            fdt = ent_date;
                            tdt = ent_date;
                        }
                        if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                        {
                            mq1 = " and vch_num<>'" + model[0].Col16.Trim() + "'";
                            isedit = true;
                            vch_num = model[0].Col16.Trim();
                        }
                        else
                        {
                            mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where type='" + model[0].Col12 + "' " + model[0].Col11 + "  ";
                            vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                            model[0].Col16 = vch_num;
                            isedit = false;
                        }
                        #region dtstr
                        DataRow dr = dtstr.NewRow();
                        dr["vch_num"] = vch_num.Trim();
                        dr["vch_date"] = ent_date;
                        dr["type"] = model[0].Col12.Trim();
                        dr["title"] = model[0].Col17;
                        dr["fdt"] = fdt;//from dt
                        dr["tdt"] = tdt;//to date
                        dr["descp"] = model[0].Col20;
                        dr["pdt"] = ent_date;//posted date
                        dr["chk_dt"] = chk_dt;//chk_val
                        dr["pr"] = model[0].Chk2 == true ? "Y" : "N";//priority
                        dr["status"] = model[0].Col21;//status

                        if (isedit)
                        {
                            dr["rec_id"] = model[0].Col7;
                            dr["ent_by"] = model[0].Col3;
                            dr["ent_date"] = model[0].Col4.Trim();
                            dr["client_id"] = model[0].Col1.Trim();
                            dr["client_unit_id"] = model[0].Col2.Trim();
                            dr["edit_by"] = userid_mst.Trim();
                            dr["edit_date"] = ent_date;
                            dr["cdt"] = model[0].Col22;
                        }
                        else
                        {
                            dr["rec_id"] = "0";
                            dr["ent_by"] = userid_mst;
                            dr["ent_date"] = ent_date;
                            dr["client_id"] = clientid_mst;
                            dr["client_unit_id"] = unitid_mst;
                            dr["edit_by"] = "-";
                            dr["edit_date"] = ent_date;
                            dr["cdt"] = ent_date;
                        }
                        #endregion
                        dtstr.Rows.Add(dr);
                        ok = sgen.Update_data(userCode, dtstr, model[0].Col10, model[0].Col8, isedit);
                        if (ok)
                        {
                            if (command == "Save" || command == "Update")
                            {
                                if (isedit) bval = "EDIT";
                                else bval = "NEW";
                                return RedirectToAction("task_list2", new { @m_id = EncryptDecrypt.Encrypt(MyGuid), @mid = EncryptDecrypt.Encrypt("1004.2"), bval = bval });
                            }
                            else if (command == "Save & New" || command == "Update & New")
                            {
                                return RedirectToAction("mytodo", new { @m_id = EncryptDecrypt.Encrypt(MyGuid), @mid = EncryptDecrypt.Encrypt(tmodel.Col14), fstr = "NEW" });
                            }

                            //ViewBag.vnew = "";
                            //ViewBag.vedit = "";
                            //ViewBag.vsave = "disabled='disabled'";
                            //ViewBag.scripCall += "showmsgJS(1, 'Data Saved Successfully', 1);disableForm();";

                            //model = new List<Tmodelmain>();
                            //model.Add(new Tmodelmain
                            //{
                            //    Col9 = tmodel.Col9,
                            //    Col10 = tmodel.Col10,
                            //    Col11 = tmodel.Col11,
                            //    Col12 = tmodel.Col12,
                            //    Col13 = "Save",
                            //    Col14 = tmodel.Col14,
                            //    Col15 = tmodel.Col15,
                            //});
                        }
                        else { ViewBag.scripCall = "showmsgJS(1, 'Data Not Saved', 0);"; }
                    }
                    catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString().Replace("'", "") + "', 0);"; }
                }
            }
            catch (Exception err)
            {
                var LineNumber = new StackTrace(err, true).GetFrame(0).GetFileLineNumber();
                ViewBag.scripCall += "showmsgJS(1, '" + err.Message.Replace("'", "") + " At Line number " + LineNumber + "', 1);";
            }
            ModelState.Clear();
            return View(model);
        }
        #endregion

        #region createnote
        public ActionResult createnote()
        {
            FillMst();
            chkRef();
            List<Tmodelmain> model = new List<Tmodelmain>();
            Tmodelmain tm = new Tmodelmain();
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            m_id_mst = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);

            tm.Col9 = "NOTIFICATIONS";
            tm.Col10 = "Notifications";
            tm.Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            tm.Col12 = "NOT";
            tm.Col13 = "Save";
            tm.Col14 = mid_mst.Trim();
            tm.Col15 = m_id_mst.Trim();
            tm.Col22 = "Choose File";
            tm.Col100 = "Save & New";
            tm.Col122 = "<u>S</u>ave";
            tm.Col123 = "Save & Ne<u>w</u>";

            if (Request.QueryString["fstr"] != null)
            {
                mq0 = (Request.QueryString["fstr"]).Trim();
                if (mq0.ToUpper().Equals("NEW"))
                {
                    sgen.SetSession(MyGuid, "EDMODE", "N");
                    ViewBag.vsave = "";
                    mq = "select max(to_number(vch_num)) as auto_genid from " + tm.Col10.Trim() + "  where type='" + tm.Col12.Trim() + "'" + tm.Col11.Trim() + "";
                    vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                    tm.Col16 = vch_num;
                    tm.Col21 = "Y";//active or pending state
                }
                else
                {
                    sgen.SetSession(MyGuid, "SSEEKVAL", mq0);
                    model.Add(tm);
                    CallbackFun("EDIT", "Y", "createnote", "Home", model);
                    ViewBag.vsave = "";
                    ViewBag.scripCall += "enableForm();";
                    ModelState.Clear();
                    return View(model);
                }
            }

            model.Add(tm);
            return View(model);
        }
        [HttpPost]
        public ActionResult createnote(List<Tmodelmain> model, string command, HttpPostedFileBase[] upd1)
        {
            try
            {
                string chk_dt = "N", fdt = "", tdt = "", bval = "", att = "N";
                FillMst();
                command = command.Trim();
                if (command == "Cancel")//correction
                {
                    return RedirectToAction("notelist", new { @m_id = EncryptDecrypt.Encrypt(MyGuid), @mid = EncryptDecrypt.Encrypt("1005") });
                }
                else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
                {
                    try
                    {
                        Satransaction sat = new Satransaction(userCode, MyGuid);
                        var tmodel = model.FirstOrDefault();
                        string currdate = sgen.server_datetime(userCode);
                        ent_date = currdate;
                        DataTable dtstr = new DataTable();
                        dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                        if (model[0].Chk1 == true) chk_dt = "Y";
                        if (chk_dt.Trim().Equals("Y"))
                        {
                            if ((model[0].Col18 == "" || model[0].Col18 == null) && (model[0].Col19 == "" || model[0].Col19 == null))
                            {
                                ViewBag.scripCall += "showmsgJS(1,'Please fill both date',2);";
                                return View(model);
                            }
                            else if ((model[0].Col18 == "" || model[0].Col18 == null) && (model[0].Col19 == "" || model[0].Col19 == null))
                            {
                                ViewBag.scripCall += "showmsgJS(1,'Please fill both date',2);";
                                return View(model);
                            }
                            else
                            {
                                fdt = sgen.Savedate(model[0].Col18, true);
                                tdt = sgen.Savedate(model[0].Col19, true);
                            }
                        }
                        else
                        {
                            fdt = ent_date;
                            tdt = ent_date;
                        }
                        if (upd1.Length > 1) att = "Y";
                        if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                        {
                            mq1 = sgen.seekval(userCode, "select file_name from file_tab where ref_code='" + model[0].Col16 + "' and type='NOT' and client_unit_id='" + unitid_mst + "'", "file_name");
                            if (mq1.Trim() != "0") att = "Y";
                            mq1 = " and vch_num<>'" + model[0].Col16.Trim() + "'";
                            isedit = true;
                            vch_num = model[0].Col16.Trim();
                        }
                        else
                        {
                            mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where type='" + model[0].Col12 + "' " + model[0].Col11 + "  ";
                            vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                            model[0].Col16 = vch_num;
                            isedit = false;
                        }

                        #region dtstr
                        DataRow dr = dtstr.NewRow();
                        dr["vch_num"] = vch_num.Trim();
                        dr["vch_date"] = ent_date;
                        dr["type"] = model[0].Col12.Trim();
                        dr["title"] = model[0].Col17;
                        dr["fdt"] = fdt;//from dt
                        dr["tdt"] = tdt;//to date
                        dr["descp"] = model[0].Col20;
                        dr["pdt"] = ent_date;//posted date
                        dr["chk_dt"] = chk_dt;//chk_val                        
                        dr["attch"] = att;//attachment
                        dr["status"] = model[0].Col21;//status
                        if (isedit)
                        {
                            dr["rec_id"] = model[0].Col7;
                            dr["ent_by"] = model[0].Col3;
                            dr["ent_date"] = model[0].Col4.Trim();
                            dr["client_id"] = model[0].Col1.Trim();
                            dr["client_unit_id"] = model[0].Col2.Trim();
                            dr["edit_by"] = userid_mst.Trim();
                            dr["edit_date"] = ent_date;
                        }
                        else
                        {
                            dr["rec_id"] = "0";
                            dr["ent_by"] = userid_mst;
                            dr["ent_date"] = ent_date;
                            dr["client_id"] = clientid_mst;
                            dr["client_unit_id"] = unitid_mst;
                            dr["edit_by"] = "-";
                            dr["edit_date"] = ent_date;
                        }
                        #endregion
                        dtstr.Rows.Add(dr);
                        ok = sgen.Update_data_uncommit2(userCode, dtstr, model[0].Col10, model[0].Col8, isedit, sat);
                        if (ok)
                        {
                            try
                            {
                                DataTable dtfile = new DataTable();
                                dtfile = cmd_Fun.GetStructure(userCode, "file_tab");
                                string serverpath = Server.MapPath("~/Uploads/" + cg_com_name.Replace(" ", "") + "/");
                                foreach (HttpPostedFileBase file in upd1)
                                {
                                    HttpPostedFileBase file1 = file;
                                    ctype1 = file1.ContentType;
                                    fileName1 = file1.FileName;
                                    path1 = clientid_mst + unitid_mst + userid_mst + currdate + "_" + fileName1;
                                    encpath1 = sgen.Convert_Stringto64(path1).ToString();
                                    finalpath1 = serverpath + encpath1;
                                    file1.SaveAs(finalpath1);
                                    filesave(model, currdate, dtfile, fileName1, encpath1, "Notice", ctype1, "-");
                                }
                                res = sgen.Update_data_uncommit2(userCode, dtfile, "file_tab", "", false, sat);
                                if (!res)
                                {
                                    sat.Rollback();
                                    ViewBag.vsave = "";
                                    ViewBag.vsavenew = "";
                                    ViewBag.scripCall += "showmsgJS(1, 'Image data have error', 0);";
                                    ModelState.Clear();
                                    return View(model);
                                }
                            }
                            catch (Exception ex) { }
                            sat.Commit();
                            if (command == "Save" || command == "Update")
                            {
                                if (isedit) bval = "EDIT";
                                else bval = "NEW";
                                return RedirectToAction("notelist", new { @m_id = EncryptDecrypt.Encrypt(MyGuid), @mid = EncryptDecrypt.Encrypt("1005"), bval = bval });
                            }
                            else if (command == "Save & New" || command == "Update & New")
                            {
                                return RedirectToAction("createnote", new { @m_id = EncryptDecrypt.Encrypt(MyGuid), @mid = EncryptDecrypt.Encrypt(tmodel.Col14), fstr = "NEW" });
                            }
                        }
                        else { ViewBag.scripCall = "showmsgJS(1, 'Data Not Saved', 0);"; }
                    }
                    catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString().Replace("'", "") + "', 0);"; }
                }
            }
            catch (Exception err)
            {
                var LineNumber = new StackTrace(err, true).GetFrame(0).GetFileLineNumber();
                ViewBag.scripCall += "showmsgJS(1, '" + err.Message.Replace("'", "") + " At Line number " + LineNumber + "', 1);";
            }
            ModelState.Clear();
            return View(model);
        }
        private void filesave(List<Tmodelmain> model, string currdate, DataTable dtfile, string filename, string filepath, string filetitle, string content_type, string description)
        {
            DataRow drf = dtfile.NewRow();
            drf["rec_id"] = "0";
            drf["vch_num"] = model[0].Col16;
            drf["vch_date"] = currdate;
            drf["type"] = model[0].Col12;
            drf["ref_code"] = model[0].Col16;
            drf["ref_code1"] = model[0].Col16;
            drf["col3"] = "-";
            drf["col1"] = filetitle;
            drf["description"] = description;
            drf["file_name"] = filename;
            drf["file_path"] = filepath;
            drf["col2"] = content_type;
            drf["rec_id"] = "0";
            drf["ent_by"] = userid_mst;
            drf["ent_date"] = currdate;
            drf["client_id"] = clientid_mst;
            drf["client_unit_id"] = unitid_mst;
            drf["edit_by"] = "-";
            drf["edit_date"] = currdate;
            dtfile.Rows.Add(drf);
        }
        #endregion

        #region todolist
        public ActionResult todolist()
        {
            string bval = "";
            FillMst();
            chkRef();

            List<Tmodelmain> model = new List<Tmodelmain>();
            Tmodelmain tm1 = new Tmodelmain();
            DataTable dt = new DataTable();
            DataTable dtn = new DataTable();
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            m_id_mst = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            try { bval = EncryptDecrypt.Decrypt(Request.QueryString["bval"]).Trim(); }
            catch (Exception er) { }

            tm1.Col9 = "TO DO LIST";
            tm1.Col14 = mid_mst;
            tm1.Col15 = m_id_mst;

            mq = "select (client_id||client_unit_id||vch_num||to_char(vch_date,'yyyymmdd')||type) Sno,title," +
                "to_char(convert_tz(fdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') From_dt," +
                "to_char(convert_tz(tdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') To_dt," +
                "to_char(convert_tz(pdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') Posted_dt,descp Description,status," +
                "to_char(convert_tz(cdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') Complete_dt," +
                "(case when pr='Y' then pr else '' end) Priority from notifications " +
                "where type='TDL' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "' and ent_by='" + userid_mst + "'";
            dt = sgen.getdata(userCode, mq);
            if (dt.Rows.Count > 0)
            {
                for (int k = 0; k < dt.Rows.Count; k++)
                {
                    if (dt.Rows[k]["Description"].ToString().Length > 25)
                    {
                        dt.Rows[k]["Description"] = dt.Rows[k]["Description"].ToString().Substring(0, 25).Trim() + ".....";
                    }
                }
                dt.AcceptChanges();
                sgen.SetSession(MyGuid, actionName, dt);
                dtn = dt.Select("status<>'C'").CopyToDataTable();
                tm1.dt1 = dtn;
            }

            model.Add(tm1);
            if (bval.ToUpper().Equals("NEW")) ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
            else if (bval.ToUpper().Equals("EDIT")) ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Updated Successfully');";
            ModelState.Clear();
            return View(model);
        }

        [HttpPost]
        public ActionResult todolist(List<Tmodelmain> model, string command, String layout, string hfrow, string hftable)
        {
            try
            {

                FillMst();
                DataTable dtn = new DataTable();
                DataTable dt = (DataTable)sgen.GetSession(MyGuid, actionName);
                DataTable dt2 = (DataTable)sgen.GetSession(MyGuid, actionName + "_2");
                DataSet ds = sgen.setDS(hftable);
                if (ds.Tables.Count > 0)
                {
                    //model[0].dt1 = ds.Tables[0];
                    model[0].dt1 = dt.Select("status<>'C'").CopyToDataTable();
                    try { model[0].dt2 = dt.Select("status='C'").CopyToDataTable(); }
                    catch (Exception er) { }
                    try { model[0].dt3 = dt2.Select("status<>'C'").CopyToDataTable(); }
                    catch (Exception er) { }
                    try { model[0].dt4 = dt2.Select("status='C'").CopyToDataTable(); }
                    catch (Exception er) { }
                }

                model[0].Col16 = hfrow;
                model[0].Col17 = layout;

                if (command == "sct")
                {
                    try
                    {
                        //model[0].dt2 = dt.Select("status='C'").CopyToDataTable();
                        model[0].Col18 = "Y";
                    }
                    catch (Exception ex) { ViewBag.scripCall += "showmsgJS(1, 'No data found, 2);"; }
                }
                else if (command == "opt")
                {
                    mq1 = "select (n.client_id||n.client_unit_id||n.vch_num||to_char(n.vch_date,'yyyymmdd')||n.type) Sno,n.title," +
                   "to_char(convert_tz(n.fdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') From_dt," +
                   "to_char(convert_tz(n.tdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') To_dt," +
                   "to_char(convert_tz(n.pdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') Posted_dt,n.descp Description," +
                   "n.status,(case when n.status='C' then to_char(convert_tz(n.cdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') else '-' end) Complete_dt," +
                   "(case when n.pr='Y' then n.pr else '' end) Priority,u.user_id Ent_by from notifications n " +
                   "inner join user_details u on u.vch_num=n.ent_by and u.type='CPR' and u.team='" + userid_mst + "' " +
                   "where n.type='TDL' and n.ent_by<>'" + userid_mst + "'";
                    dtn = sgen.getdata(userCode, mq1);
                    if (dtn.Rows.Count > 0)
                    {
                        for (int k = 0; k < dtn.Rows.Count; k++)
                        {
                            if (dtn.Rows[k]["Description"].ToString().Length > 25)
                            {
                                dtn.Rows[k]["Description"] = dtn.Rows[k]["Description"].ToString().Substring(0, 25).Trim() + ".....";
                            }
                        }
                        dtn.AcceptChanges();
                        sgen.SetSession(MyGuid, actionName + "_2", dtn);
                        dtn = dtn.Select("status<>'C'").CopyToDataTable();
                        model[0].dt3 = dtn;
                        model[0].Col19 = "Y";
                    }
                }
                else if (command == "oct")
                {
                    try
                    {
                        //dtn = (DataTable)Session[actionName + "_2"];
                        //dtn = dtn.Select("status='C'").CopyToDataTable();
                        //model[0].dt4 = dtn;
                        model[0].Col20 = "Y";
                    }
                    catch (Exception ex) { ViewBag.scripCall += "showmsgJS(1, 'No data found, 2);"; }
                }
                else if (command == "Callback")
                {
                    if (sgen.GetSession(MyGuid, "tododelid") != null) { btnval = "RDEL"; }
                    else if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
                    model = CallbackFun(btnval, "N", "todolist", "Home", model);
                }
            }
            catch (Exception err)
            {
                var LineNumber = new StackTrace(err, true).GetFrame(0).GetFileLineNumber();
                ViewBag.scripCall += "showmsgJS(1, '" + err.Message.Replace("'", "") + " At Line number " + LineNumber + "', 1);";
            }
            ModelState.Clear();
            return View(model);
        }

        [HttpPost]
        public ContentResult rdel(string value)
        {
            FillMst();
            res = false;
            try
            {
                if (value.Trim() != "")
                {
                    res = sgen.execute_cmd(userCode, "Delete from notifications WHERE (client_id|| client_unit_id|| vch_num|| to_char(vch_date, 'yyyymmdd')|| type)='" + value.Trim() + "'");
                    if (value.Substring(23, 3).Trim().Equals("NOT"))
                    {
                        mq1 = sgen.seekval(userCode, "select file_name from notifications where (client_id|| client_unit_id|| vch_num|| to_char(vch_date, 'yyyymmdd')|| type)='" + value.Trim() + "'", "file_name");
                        if (!mq1.Trim().Equals("0"))
                        {
                            res = sgen.execute_cmd(userCode, "Delete from file_tab WHERE (client_id|| client_unit_id|| vch_num|| to_char(vch_date, 'yyyymmdd')|| type)='" + value.Trim() + "'");
                        }
                    }
                }
                return Content(res.ToString());
            }
            catch (Exception ex) { return Content(res.ToString()); }
        }

        //[HttpPost]
        //public ContentResult setst(string val, string vchnum)
        //{
        //    string status = "", cdt = "";
        //    try
        //    {
        //        if (val.Trim() != null && vchnum.Trim() != null)
        //        {
        //            string currdate = sgen.server_datetime(userCode);
        //            mq1 = sgen.seekval(userCode, "select status from notifications where (client_id||client_unit_id||vch_num||to_char(vch_date,'yyyymmdd')||type)='" + vchnum + "'", "status");
        //            if (mq1.Trim().ToUpper() == "P")
        //            {
        //                status = "C";
        //                cdt = ",cdt=" + "to_date('" + currdate + "','YYYY-MM-DD HH24:MI:SS') " + "";
        //            }
        //            else status = "P";
        //            mq = "update notifications set status='" + status + "'" + cdt + " where (client_id||client_unit_id||vch_num||to_char(vch_date,'yyyymmdd')||type)='" + vchnum + "'";
        //            res = sgen.execute_cmd(userCode, mq);
        //            if (!res) { return Content("N"); }
        //            else return Content("Y");
        //        }
        //        else { return Content("N"); }
        //    }
        //    catch (Exception err) { return Content("N"); }
        //}

        [HttpPost]
        public ContentResult setst(string vchnum)
        {
            string status = "", cdt = "";
            try
            {
                if (vchnum.Trim() != null)
                {
                    string currdate = sgen.server_datetime(userCode);
                    mq1 = sgen.seekval(userCode, "select status from notifications where (client_id||client_unit_id||vch_num||to_char(vch_date,'yyyymmdd')||type)='" + vchnum + "'", "status");
                    if (mq1.Trim().ToUpper() == "P")
                    {
                        status = "C";
                        cdt = ",cdt=" + "to_date('" + currdate + "','YYYY-MM-DD HH24:MI:SS') " + "";
                    }
                    else status = "P";
                    mq = "update notifications set status='" + status + "'" + cdt + " where (client_id||client_unit_id||vch_num||to_char(vch_date,'yyyymmdd')||type)='" + vchnum + "'";
                    res = sgen.execute_cmd(userCode, mq);
                    if (!res) { return Content("N"); }
                    else return Content("Y");
                }
                else { return Content("N"); }
            }
            catch (Exception err) { return Content("N"); }
        }

        [HttpPost]
        public ActionResult getdetail(string val, string vchnum)
        {
            try
            {
                if (val.Trim() != null && vchnum.Trim() != null)
                {
                    mq = "select title Title,descp Description,to_char(convert_tz(fdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') From_Date," +
                         "to_char(convert_tz(tdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') To_Date," +
                         "to_char(convert_tz(pdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') Posted_Date " +
                         "from notifications where (client_id||client_unit_id||vch_num||to_char(vch_date,'yyyymmdd')||type)='" + vchnum + "'";
                    DataTable dt = sgen.getdata(userCode, mq);
                    if (dt.Rows.Count > 0)
                    {
                        List<string> td = new List<string>();
                        td.Add(dt.Rows[0]["Title"].ToString());
                        td.Add(dt.Rows[0]["Description"].ToString());
                        td.Add(dt.Rows[0]["From_Date"].ToString());
                        td.Add(dt.Rows[0]["To_Date"].ToString());
                        td.Add(dt.Rows[0]["Posted_Date"].ToString());

                        if (vchnum.Trim().Substring(23, 3).Trim().Equals("NOT"))
                        {
                            mq1 = "select rec_id,file_name,file_path,col1 from file_tab where (client_id||client_unit_id||vch_num||type)='" + vchnum.Substring(0, 15).Trim() + "NOT'";
                            DataTable dtf = sgen.getdata(userCode, mq1);
                            if (dtf.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtf.Rows.Count; i++)
                                {
                                    td.Add(dtf.Rows[i]["rec_id"].ToString() + "~" + dtf.Rows[i]["file_name"].ToString());
                                }
                            }
                        }
                        return Json(td);
                    }
                    else return Json("N");
                }
                else { return Json("N"); }
            }
            catch (Exception err) { return Json("N"); }
        }

        [HttpGet]
        public FileResult fdown(string value, string type, string Myguid = "")
        {
            FillMst(Myguid);
            if (!value.Trim().Equals(""))
            {
                DataTable dt2 = new DataTable();
                mq = "select file_name,file_path from file_tab where rec_id='" + value.Trim() + "' and type='" + type + "' and client_unit_id='" + unitid_mst + "'";
                dt2 = sgen.getdata(userCode, mq);
                if (dt2.Rows.Count > 0)
                {
                    path1 = Convert.ToString(dt2.Rows[0]["file_path"]);
                    fileName1 = Convert.ToString(dt2.Rows[0]["file_name"]);
                }
            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Uploads/" + cg_com_name.Replace(" ", "") + "/" + path1));
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName1);
        }

        #endregion

        #region todolist
        public ActionResult task_list2(string m_id, string mid)
        {
            string bval = "", colsord = "";
            m_id = EncryptDecrypt.Decrypt(m_id);
            FillMst(m_id);
            chkRef();

            List<Tmodelmain> model = new List<Tmodelmain>();
            Tmodelmain tm1 = new Tmodelmain();
            DataTable dt = new DataTable();
            DataTable dtn = new DataTable();
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            m_id_mst = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            try { bval = Request.QueryString["bval"].Trim(); }
            catch (Exception er) { }

            tm1.Col9 = "TO DO LIST";
            tm1.Col14 = mid_mst;
            tm1.Col15 = m_id_mst;

            mq = "SELECT param1,param2,param3,param5 FROM CONTROLS WHERE TYPE='CTL' AND ID='000005' AND M_MODULE3='-'";
            dt = sgen.getdata(userCode, mq);
            if (dt.Rows.Count > 0)
            {
                colsord = colsord + "fdt " + dt.Rows[0]["param1"].ToString() + ",";
                colsord = colsord + "tdt " + dt.Rows[0]["param2"].ToString() + ",";
                colsord = colsord + "pdt " + dt.Rows[0]["param3"].ToString() + ",";
                colsord = colsord + "pr " + dt.Rows[0]["param5"].ToString() + ",";
                colsord = "order by " + colsord.TrimEnd(',').Trim();
            }

            mq = "select (client_id||client_unit_id||vch_num||to_char(vch_date,'yyyymmdd')||type) Sno,title," +
               "to_char(convert_tz(fdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') From_dt," +
               "to_char(convert_tz(tdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') To_dt," +
               "to_char(convert_tz(pdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') Posted_dt,descp Description,status," +
               "to_char(convert_tz(cdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') Complete_dt," +
               "(case when pr='Y' then pr else '' end) Priority from notifications " +
               "where type='TDL' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "' and ent_by='" + userid_mst + "' and status<>'C' " + colsord + "";

            mq0 = "select (client_id||client_unit_id||vch_num||to_char(vch_date,'yyyymmdd')||type) Sno,title," +
   "to_char(convert_tz(fdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') From_dt," +
   "to_char(convert_tz(tdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') To_dt," +
   "to_char(convert_tz(pdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') Posted_dt,descp Description,status," +
   "to_char(convert_tz(cdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') Complete_dt," +
   "(case when pr='Y' then pr else '' end) Priority from notifications " +
   "where type='TDL' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "' and ent_by='" + userid_mst + "' and status='C' " + colsord + "";

            mq1 = "select (n.client_id||n.client_unit_id||n.vch_num||to_char(n.vch_date,'yyyymmdd')||n.type) Sno,n.title," +
"to_char(convert_tz(n.fdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') From_dt," +
"to_char(convert_tz(n.tdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') To_dt," +
"to_char(convert_tz(n.pdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') Posted_dt,n.descp Description," +
"n.status,(case when n.status='C' then to_char(convert_tz(n.cdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') else '-' end) Complete_dt," +
"(case when n.pr='Y' then n.pr else '' end) Priority,u.user_id Ent_by from notifications n " +
"inner join user_details u on u.vch_num=n.ent_by and u.type='CPR' and u.team='" + userid_mst + "' " +
"where n.type='TDL' and n.ent_by<>'" + userid_mst + "' and n.status<>'C' " + colsord + "";

            mq2 = "select (n.client_id||n.client_unit_id||n.vch_num||to_char(n.vch_date,'yyyymmdd')||n.type) Sno,n.title," +
"to_char(convert_tz(n.fdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') From_dt," +
"to_char(convert_tz(n.tdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') To_dt," +
"to_char(convert_tz(n.pdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') Posted_dt,n.descp Description," +
"n.status,(case when n.status='C' then to_char(convert_tz(n.cdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') else '-' end) Complete_dt," +
"(case when n.pr='Y' then n.pr else '' end) Priority,u.user_id Ent_by from notifications n " +
"inner join user_details u on u.vch_num=n.ent_by and u.type='CPR' and u.team='" + userid_mst + "' " +
"where n.type='TDL' and n.ent_by<>'" + userid_mst + "' and n.status='C' " + colsord + "";

            sgen.SetSession(MyGuid, "gridq_demogrid", mq);
            sgen.SetSession(MyGuid, "gridq_grd2", mq0);
            sgen.SetSession(MyGuid, "gridq_grd3", mq1);
            sgen.SetSession(MyGuid, "gridq_grd4", mq2);

            //dt = sgen.getdata(userCode, mq);
            //if (dt.Rows.Count > 0)
            //{
            //    for (int k = 0; k < dt.Rows.Count; k++)
            //    {
            //        if (dt.Rows[k]["Description"].ToString().Length > 25)
            //        {
            //            dt.Rows[k]["Description"] = dt.Rows[k]["Description"].ToString().Substring(0, 25).Trim() + ".....";
            //        }
            //    }
            //    dt.AcceptChanges();
            //    sgen.SetSession(MyGuid, actionName, dt);
            //    dtn = dt.Select("status<>'C'").CopyToDataTable();
            //    tm1.dt1 = dtn;
            //}

            model.Add(tm1);
            if (bval.ToUpper().Equals("NEW")) ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
            else if (bval.ToUpper().Equals("EDIT")) ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Updated Successfully');";
            ModelState.Clear();
            return View(model);
        }

        [HttpPost]
        public ActionResult task_list2(List<Tmodelmain> model, string command, String layout, string hfrow, string hftable)
        {
            try
            {

                FillMst();
                //DataTable dtn = new DataTable();
                //DataTable dt = (DataTable)sgen.GetSession(MyGuid, actionName);
                //DataTable dt2 = (DataTable)sgen.GetSession(MyGuid, actionName + "_2");
                //DataSet ds = sgen.setDS(hftable);
                //if (ds.Tables.Count > 0)
                //{
                //    //model[0].dt1 = ds.Tables[0];
                //    model[0].dt1 = dt.Select("status<>'C'").CopyToDataTable();
                //    try { model[0].dt2 = dt.Select("status='C'").CopyToDataTable(); }
                //    catch (Exception er) { }
                //    try { model[0].dt3 = dt2.Select("status<>'C'").CopyToDataTable(); }
                //    catch (Exception er) { }
                //    try { model[0].dt4 = dt2.Select("status='C'").CopyToDataTable(); }
                //    catch (Exception er) { }
                //}

                //model[0].Col16 = hfrow;
                //model[0].Col17 = layout;

                //if (command == "sct")
                //{
                //    try
                //    {
                //        //model[0].dt2 = dt.Select("status='C'").CopyToDataTable();
                //        model[0].Col18 = "Y";
                //    }
                //    catch (Exception ex) { ViewBag.scripCall += "showmsgJS(1, 'No data found, 2);"; }
                //}
                //else if (command == "opt")
                //{
                //    mq1 = "select (n.client_id||n.client_unit_id||n.vch_num||to_char(n.vch_date,'yyyymmdd')||n.type) Sno,n.title," +
                //   "to_char(convert_tz(n.fdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') From_dt," +
                //   "to_char(convert_tz(n.tdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') To_dt," +
                //   "to_char(convert_tz(n.pdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') Posted_dt,n.descp Description," +
                //   "n.status,(case when n.status='C' then to_char(convert_tz(n.cdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') else '-' end) Complete_dt," +
                //   "(case when n.pr='Y' then n.pr else '' end) Priority,u.user_id Ent_by from notifications n " +
                //   "inner join user_details u on lpad(u.rec_id,'6','0')=n.ent_by and u.type='CPR' and u.team='" + userid_mst + "' " +
                //   "where n.type='TDL' and n.ent_by<>'" + userid_mst + "' and n.status<>'C'";
                //    dtn = sgen.getdata(userCode, mq1);
                //    if (dtn.Rows.Count > 0)
                //    {
                //        for (int k = 0; k < dtn.Rows.Count; k++)
                //        {
                //            if (dtn.Rows[k]["Description"].ToString().Length > 25)
                //            {
                //                dtn.Rows[k]["Description"] = dtn.Rows[k]["Description"].ToString().Substring(0, 25).Trim() + ".....";
                //            }
                //        }
                //        dtn.AcceptChanges();
                //        sgen.SetSession(MyGuid, actionName + "_2", dtn);
                //        dtn = dtn.Select("status<>'C'").CopyToDataTable();
                //        model[0].dt3 = dtn;
                //        model[0].Col19 = "Y";
                //    }
                //}
                //else if (command == "oct")
                //{
                //    try
                //    {
                //        //dtn = (DataTable)Session[actionName + "_2"];
                //        //dtn = dtn.Select("status='C'").CopyToDataTable();
                //        //model[0].dt4 = dtn;
                //        model[0].Col20 = "Y";
                //    }
                //    catch (Exception ex) { ViewBag.scripCall += "showmsgJS(1, 'No data found, 2);"; }
                //}
                //if (command == "Callback")
                //{
                //if (sgen.GetSession(MyGuid, "tododelid") != null)
                //{
                //    btnval = "RDEL";
                //    sgen.SetSession(MyGuid, "SSEEKVAL", sgen.GetSession(MyGuid, "tododelid").ToString());
                //}
                //if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
                //model = CallbackFun(btnval, "N", actionName, controllerName, model);
                //if (model[0].Col16 == "Y")
                //{
                //    //return RedirectToAction(actionName, new { @m_id = EncryptDecrypt.Encrypt(model[0].Col15), @mid = EncryptDecrypt.Encrypt(model[0].Col14) });
                //}
                //else { model[0].Col16 = "N"; }
                //}
            }
            catch (Exception err)
            {
                var LineNumber = new StackTrace(err, true).GetFrame(0).GetFileLineNumber();
                ViewBag.scripCall += "showmsgJS(1, '" + err.Message.Replace("'", "") + " At Line number " + LineNumber + "', 1);";
            }
            ModelState.Clear();
            return View(model);
        }
        #endregion

        #region notelist
        public ActionResult notelist(string m_id, string mid)
        {
            string bval = "";
            m_id = EncryptDecrypt.Decrypt(m_id);
            FillMst(m_id);
            chkRef();
            List<Tmodelmain> model = new List<Tmodelmain>();
            Tmodelmain tm1 = new Tmodelmain();
            DataTable dt = new DataTable();
            DataTable dtn = new DataTable();
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            m_id_mst = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            try { bval = Request.QueryString["bval"].Trim(); }
            catch (Exception er) { }

            tm1.Col9 = "NOTIFICATIONS";
            tm1.Col14 = mid_mst;
            tm1.Col15 = m_id_mst;

            mq = "select (client_id||client_unit_id||vch_num||to_char(vch_date,'yyyymmdd')||type) Sno,title," +
               "to_char(convert_tz(fdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') From_dt," +
               "to_char(convert_tz(tdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') To_dt," +
               "to_char(convert_tz(pdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') Posted_dt,descp Description,status," +
               "attch attachment from notifications " +
               "where type='NOT' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            sgen.SetSession(MyGuid, "gridq_demogrid", mq);
            model.Add(tm1);
            if (bval.ToUpper().Equals("NEW")) ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
            else if (bval.ToUpper().Equals("EDIT")) ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Updated Successfully');";
            ModelState.Clear();
            return View(model);
        }
        #endregion

        #region menueditor
        public ActionResult menueditor()
        {
            FillMst();
            chkRef();
            List<Tmodelmain> model = new List<Tmodelmain>();
            Tmodelmain tm1 = new Tmodelmain();
            DataTable dt = new DataTable();
            DataTable dtn = new DataTable();
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            m_id_mst = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            tm1.Col9 = "MENU EDITOR";
            tm1.Col14 = mid_mst;
            tm1.Col15 = m_id_mst;
            mq = "select (rec_id||m_id||m_module3) Sno,rec_id id,m_id menu_id,m_name,m_submenu,u_id,m_order,m_link,m_module1,m_module2,m_module3,m_level from menus";
            sgen.SetSession(MyGuid, "gridq_demogrid", mq);
            model.Add(tm1);
            ModelState.Clear();
            return View(model);
        }

        [HttpPost]
        public ContentResult updaterec(string val, string colval, string col)
        {
            try
            {
                if (val.Trim() != null)
                {
                    mq = "update menus set " + col + "='" + colval + "' where (rec_id||m_id||m_module3)='" + val.Trim() + "'";
                    res = sgen.execute_cmd(userCode, mq);
                    if (!res) { return Content("N"); }
                    else return Content("Y");
                }
                else { return Content("N"); }
            }
            catch (Exception err) { return Content("N"); }
        }
        #endregion

        #region   create_user
        public ActionResult create_user()
        {
            List<Tmodelmain> model = new List<Tmodelmain>();
            try
            {
                FillMst();
                chkRef();
                ViewBag.vnew = "";
                ViewBag.vedit = "";
                ViewBag.vsave = "disabled='disabled'";
                ViewBag.vsavenew = "disabled='disabled'";

                ViewBag.scripCall = "disableForm();";
                mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
                m_id_mst = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
                List<SelectListItem> mod1 = new List<SelectListItem>();
                List<SelectListItem> mod2 = new List<SelectListItem>();
                List<SelectListItem> mod3 = new List<SelectListItem>();
                List<SelectListItem> mod4 = new List<SelectListItem>();
                List<SelectListItem> mod5 = new List<SelectListItem>();
                List<SelectListItem> mod6 = new List<SelectListItem>();
                List<SelectListItem> mod7 = new List<SelectListItem>();
                List<SelectListItem> mod8 = new List<SelectListItem>();
                List<SelectListItem> mod9 = new List<SelectListItem>();
                List<SelectListItem> mod11 = new List<SelectListItem>();

                tab_name = "user_details";
                type = "CPR";
                type_desc = "";
                string title = "";
                title = "CREATION OF USER";

                model.Add(new Tmodelmain
                {
                    TList1 = mod1,
                    TList2 = mod2,
                    TList3 = mod3,
                    TList4 = mod4,
                    TList5 = mod5,
                    TList6 = mod6,
                    TList7 = mod7,
                    TList8 = mod8,
                    TList9 = mod9,
                    TList10 = mod9,
                    TList11 = mod11,
                    SelectedItems1 = new string[] { "" },
                    SelectedItems2 = new string[] { "" },
                    SelectedItems3 = new string[] { "" },
                    SelectedItems4 = new string[] { "" },
                    SelectedItems5 = new string[] { "" },
                    SelectedItems6 = new string[] { "" },
                    SelectedItems7 = new string[] { "" },
                    SelectedItems8 = new string[] { "" },
                    SelectedItems9 = new string[] { "" },
                    SelectedItems10 = new string[] { "" },
                    SelectedItems11 = new string[] { "" },
                    Col9 = title,
                    Col1 = clientid_mst,
                    Col2 = unitid_mst,
                    Col10 = tab_name,
                    Col12 = type,
                    Col13 = "Save",
                    Col14 = mid_mst,
                    Col15 = m_id_mst,
                    Col100 = "Save & New",
                    Col121 = "S",
                    Col122 = "<u>S</u>ave",
                    Col123 = "Save & Ne<u>w</u>",
                    Col17 = "",
                    Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'"
                });
            }
            catch (Exception ex)
            {
                msg = Regex.Replace(ex.Message.ToString().Replace("'", ""), @"\t|\n|\r", "");
                ViewBag.scripCall = "showmsgJS(1, '" + msg + "', 1);";
            }
            return View(model);
        }
        public List<Tmodelmain> new_create_user(List<Tmodelmain> model)
        {
            try
            {
                model[0].Col13 = "Save";
                model[0].Col100 = "Save & New";
                model[0].Col121 = "S";
                model[0].Col122 = "<u>S</u>ave";
                model[0].Col123 = "Save & Ne<u>w</u>";
                try
                {
                    sgen.SetSession(MyGuid, "SSEEKVAL", null);
                    sgen.SetSession(MyGuid, "EDMODE", "N");
                    ViewBag.scripCall = "enableForm();";
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vsavenew = "";

                    List<SelectListItem> mod1 = new List<SelectListItem>();
                    List<SelectListItem> mod2 = new List<SelectListItem>();
                    List<SelectListItem> mod3 = new List<SelectListItem>();
                    List<SelectListItem> mod4 = new List<SelectListItem>();
                    List<SelectListItem> mod5 = new List<SelectListItem>();
                    List<SelectListItem> mod6 = new List<SelectListItem>();
                    List<SelectListItem> mod7 = new List<SelectListItem>();
                    List<SelectListItem> mod8 = new List<SelectListItem>();
                    List<SelectListItem> mod9 = new List<SelectListItem>();
                    List<SelectListItem> mod10 = new List<SelectListItem>();
                    List<SelectListItem> mod11 = new List<SelectListItem>();
                    var tm = model.FirstOrDefault();

                    #region Country
                    DataTable dt1 = new DataTable();
                    mod1 = cmd_Fun.country(userCode, unitid_mst);
                    //mq = "select distinct country_name,alpha_2 from country_state  order by country_name";


                    //dt1 = sgen.getdata(userCode, mq);


                    //if (dt1.Rows.Count > 0)
                    //{
                    //    foreach (DataRow dr in dt1.Rows)
                    //    {
                    //        mod1.Add(new SelectListItem { Text = dr["country_name"].ToString(), Value = dr["alpha_2"].ToString() });

                    //    }

                    //}
                    #endregion

                    #region department
                    mod4 = cmd_Fun.dept(userCode, clientid_mst, unitid_mst);
                    DataTable dt = new DataTable();
                    //dt = sgen.getdata(userCode, "select master_id,master_name from master_setting where type='MDP'");
                    //if (dt.Rows.Count > 0)
                    //{
                    //    foreach (DataRow dr in dt.Rows)
                    //    {
                    //        mod4.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });

                    //    }
                    //}
                    #endregion

                    #region designation
                    mod5 = cmd_Fun.desig(userCode, unitid_mst);

                    #endregion

                    #region company
                    string where = "";
                    //mq = sgen.seekval(userCode, "select client_id from user_details where rec_id='" + Convert.ToInt32(userid_mst) + "'", "client_id");
                    mq = sgen.seekval(userCode, "select client_id from user_details where vch_num='" + userid_mst + "'", "client_id");
                    if (!role_mst.ToUpper().Equals("OWNER")) where = " and company_profile_id in ('" + mq.Replace(",", "','") + "')";


                    DataTable dtcomp = new DataTable();
                    mq = "SELECT  Company_Profile_Id, company_name|| '('|| company_profile_id||')' as nameid FROM  company_profile where type='CP' " + where + "";
                    dtcomp = sgen.getdata(userCode, mq);
                    if (dtcomp.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtcomp.Rows)
                        {
                            mod6.Add(new SelectListItem { Text = dr["nameid"].ToString(), Value = dr["company_profile_id"].ToString() });

                        }
                    }
                    #endregion

                    #region module
                    dt = new DataTable();
                    where = "";
                    //mq = sgen.seekval(userCode, "select mod_id from user_details where rec_id='" + Convert.ToInt32(userid_mst) + "'", "mod_id");
                    mq = sgen.seekval(userCode, "select mod_id from user_details where vch_num='" + userid_mst + "'", "mod_id");
                    if (!role_mst.ToUpper().Equals("OWNER")) where = " and mod_id in ('" + mq.Replace(",", "','") + "')";

                    DataTable dtcomp_mod = new DataTable();

                    mq = "SELECT mod_Id, m_name|| '('||mod_id ||')' as nameid FROM com_module join menus on com_module.mod_id=menus.m_id and upper(com_code) = upper('" + userCode + "')" + where + "";

                    dt = sgen.getdata(userCode, mq);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            mod8.Add(new SelectListItem { Text = dr["nameid"].ToString(), Value = dr["mod_id"].ToString() });

                        }
                    }
                    #endregion

                    #region training
                    DataTable dtuser = new DataTable();
                    dtuser = sgen.getdata(userCode, "SELECT user_id FROM user_details where status='active' and type='CPR' and find_in_set('1000', mod_id)=1");
                    if (dtuser.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtuser.Rows)
                        {
                            mod9.Add(new SelectListItem { Text = dr["user_id"].ToString(), Value = dr["user_id"].ToString() });

                        }
                    }
                    #endregion

                    #region TEAM
                    DataTable dtt = new DataTable();
                    //dtt = sgen.getdata(userCode, "SELECT lpad(rec_id,'6','0') rec_id,user_id FROM user_details where status='active' and " +
                    dtt = sgen.getdata(userCode, "SELECT vch_num,user_id FROM user_details where status='active' and " +
                        "type='CPR'");
                    if (dtt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtt.Rows)
                        {
                            mod10.Add(new SelectListItem { Text = dr["user_id"].ToString(), Value = dr["vch_num"].ToString() });
                        }
                    }
                    #endregion

                    #region usertype
                    mod11.Add(new SelectListItem { Text = "Employee", Value = "001" });
                    mod11.Add(new SelectListItem { Text = "Vendor", Value = "002" });
                    mod11.Add(new SelectListItem { Text = "Customer", Value = "003" });
                    #endregion

                    #region city
                    DataTable dt2 = new DataTable();
                    dt2 = sgen.getdata(userCode, "SELECT distinct city_name FROM country_state union all SELECT 'Other' city_name from dual");
                    if (dt2.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt2.Rows)
                        {
                            mod3.Add(new SelectListItem { Text = dr["city_name"].ToString(), Value = dr["city_name"].ToString() });

                        }
                    }
                    #endregion                    

                    TempData[MyGuid + "_TList1"] = mod1;
                    TempData[MyGuid + "_TList2"] = mod2;
                    TempData[MyGuid + "_TList3"] = mod3;
                    TempData[MyGuid + "_TList4"] = mod4;
                    TempData[MyGuid + "_TList5"] = mod5;
                    TempData[MyGuid + "_TList6"] = mod6;
                    TempData[MyGuid + "_TList7"] = mod7;
                    TempData[MyGuid + "_TList8"] = mod8;
                    TempData[MyGuid + "_TList9"] = mod9;
                    TempData[MyGuid + "_Tlist10"] = mod10;
                    TempData[MyGuid + "_Tlist11"] = mod11;

                    model[0].TList1 = mod1;
                    model[0].TList2 = mod2;
                    model[0].TList3 = mod3;
                    model[0].TList4 = mod4;
                    model[0].TList5 = mod5;
                    model[0].TList6 = mod6;
                    model[0].TList7 = mod7;
                    model[0].TList8 = mod8;
                    model[0].TList9 = mod9;
                    model[0].TList10 = mod10;
                    model[0].TList11 = mod11;

                    // mq = "select max(to_number(per_id))  as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'";
                    //mq = "select max(to_number(substr(vch_num,2,5))) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "' and substr(vch_num,1,1)='9'";
                    mq = "select max(to_number((substr(lpad(vch_num,6,'0'),2,5)))) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "' and substr(lpad(vch_num,6,'0'),1,1)='9'";
                    vch_num = "9" + sgen.genNo(userCode, mq, 5, "auto_genid");
                    model[0].Col18 = vch_num;
                }
                catch (Exception ex)
                { ViewBag.scripCall = "showmsgJS(1, '" + ex.Message.ToString().Replace("'", "") + "', 1);"; }
            }
            catch (Exception ex) { }
            return model;
        }
        [HttpPost]
        public ActionResult create_user(List<Tmodelmain> model, string command, string[] rbt_mod)
        {
            try
            {
                #region 
                FillMst();
                List<SelectListItem> mod1 = (List<SelectListItem>)TempData[MyGuid + "_Tlist1"];
                List<SelectListItem> mod2 = (List<SelectListItem>)TempData[MyGuid + "_Tlist2"];
                List<SelectListItem> mod3 = (List<SelectListItem>)TempData[MyGuid + "_Tlist3"];
                List<SelectListItem> mod4 = (List<SelectListItem>)TempData[MyGuid + "_Tlist4"];
                List<SelectListItem> mod5 = (List<SelectListItem>)TempData[MyGuid + "_Tlist5"];
                List<SelectListItem> mod6 = (List<SelectListItem>)TempData[MyGuid + "_Tlist6"];
                List<SelectListItem> mod7 = (List<SelectListItem>)TempData[MyGuid + "_Tlist7"];
                List<SelectListItem> mod8 = (List<SelectListItem>)TempData[MyGuid + "_Tlist8"];
                List<SelectListItem> mod9 = (List<SelectListItem>)TempData[MyGuid + "_Tlist9"];
                List<SelectListItem> mod10 = (List<SelectListItem>)TempData[MyGuid + "_Tlist10"];
                List<SelectListItem> mod11 = (List<SelectListItem>)TempData[MyGuid + "_Tlist11"];

                TempData[MyGuid + "_Tlist1"] = mod1;
                TempData[MyGuid + "_Tlist2"] = mod2;
                TempData[MyGuid + "_Tlist3"] = mod3;
                TempData[MyGuid + "_Tlist4"] = mod4;
                TempData[MyGuid + "_Tlist5"] = mod5;
                TempData[MyGuid + "_Tlist6"] = mod6;
                TempData[MyGuid + "_Tlist7"] = mod7;
                TempData[MyGuid + "_Tlist8"] = mod8;
                TempData[MyGuid + "_Tlist9"] = mod9;
                TempData[MyGuid + "_Tlist10"] = mod10;
                TempData[MyGuid + "_Tlist11"] = mod11;

                model[0].TList1 = mod1;
                model[0].TList2 = mod2;
                model[0].TList3 = mod3;
                model[0].TList4 = mod4;
                model[0].TList5 = mod5;
                model[0].TList6 = mod6;
                model[0].TList7 = mod7;
                model[0].TList8 = mod8;
                model[0].TList9 = mod9;
                model[0].TList10 = mod10;
                model[0].TList11 = mod11;

                if (model[0].SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
                if (model[0].SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
                if (model[0].SelectedItems3 == null) model[0].SelectedItems3 = new string[] { "" };
                if (model[0].SelectedItems4 == null) model[0].SelectedItems4 = new string[] { "" };
                if (model[0].SelectedItems5 == null) model[0].SelectedItems5 = new string[] { "" };
                if (model[0].SelectedItems6 == null) model[0].SelectedItems6 = new string[] { "" };
                if (model[0].SelectedItems7 == null) model[0].SelectedItems7 = new string[] { "" };
                if (model[0].SelectedItems8 == null) model[0].SelectedItems8 = new string[] { "" };
                if (model[0].SelectedItems9 == null) model[0].SelectedItems9 = new string[] { "" };
                if (model[0].SelectedItems10 == null) model[0].SelectedItems10 = new string[] { "" };
                if (model[0].SelectedItems11 == null) model[0].SelectedItems11 = new string[] { "" };
                #endregion

                if (command == "New")
                {
                    model = new_create_user(model);
                }
                else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
                {
                    try
                    {
                        string outside_permission = "F", mannual_city = "", city_name = "", clientname = "", role_mst = "", unit_name = "";
                        Int32 usercount = Convert.ToInt32(sgen.getstring(userCode, "select Count(User_Id) as User_Count from " + model[0].Col10.Trim() + " where status='" + "active" + "'"));
                        Int32 comp_usercount = Convert.ToInt32(controls_mst.Split(',')[0]);
                        if (usercount >= comp_usercount)
                        {

                            ViewBag.scripCall = "showmsgJS(1, 'You Cannot Create a New Account. Your No. of User Is Limited', 2);";
                            //goto END;
                            return View(model);

                        }

                        string cond = "";
                        if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                        {
                            isedit = true;
                            vch_num = model[0].Col16;
                            cond = "and vch_num<>'" + vch_num + "'";

                        }
                        else
                        {
                            isedit = false;

                            // mq = "select max(to_number(per_id)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                            //mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a  where role!='owner'";
                            //mq = "select max(to_number(substr(vch_num,2,5))) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "' and substr(vch_num,1,1)='9'";
                            mq = "select max(to_number((substr(lpad(vch_num,6,'0'),2,5)))) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "' and substr(lpad(vch_num,6,'0'),1,1)='9'";
                            vch_num = "9" + sgen.genNo(userCode, mq, 5, "auto_genid");
                        }

                        string chk = sgen.getstring(userCode, "select user_id from user_details where user_id='" + model[0].Col20 + "' " + cond + "");
                        if (chk != "")
                        {
                            ViewBag.vnew = "disabled='disabled'";
                            ViewBag.vedit = "disabled='disabled'";
                            ViewBag.vsave = "";
                            ViewBag.vsavenew = "";
                            ViewBag.scripCall = "showmsgJS(1, 'User Already Exist', 2);";
                            //goto END;
                            return View(model);
                        }
                        DataTable dataTable = sgen.getdata(userCode, "select  * from " + model[0].Col10.Trim() + "  where  1=2");
                        string currdate = sgen.server_datetime(userCode);
                        string pwd = EncryptDecrypt.Encrypt(model[0].Col21.Trim());
                        if (model[0].Chk2 == true) outside_permission = "T";

                        try
                        {
                            city_name = ((List<SelectListItem>)TempData[MyGuid + "_Tlist3"]).SingleOrDefault(item => item.Value == model[0].SelectedItems3.FirstOrDefault().ToString()).Text.ToString();
                        }
                        catch { }
                        if (city_name == "Other") mannual_city = model[0].Col31.Trim();

                        else mannual_city = model[0].SelectedItems3.FirstOrDefault();
                        string[] clientid = string.Join(",", model[0].SelectedItems6).Split(',');
                        string client_id = string.Join(",", model[0].SelectedItems6);

                        try
                        {
                            for (Int32 x = 0; x < clientid.Length; x++)
                            {
                                clientname = clientname + "," + ((List<SelectListItem>)TempData[MyGuid + "_Tlist6"]).SingleOrDefault(item => item.Value == clientid[x]).Text.ToString();
                            }
                        }
                        catch (Exception ex) { }

                        #region 
                        List<string> mylist = client_id.Split(',').Distinct().OrderBy(x => x).ToList();
                        int ct = mylist.Count;
                        string cl_id = mylist[0].ToString();
                        DataTable dtmail = new DataTable();
                        dtmail = sgen.getdata(userCode, "select com_email,com_password,com_smtp,com_port from company_profile where company_profile_id='" + cl_id + "' and type='CP'");
                        if (dtmail.Rows.Count > 0)
                        {
                            string smtpvalue = Convert.ToString(dtmail.Rows[0]["com_Smtp"]);
                            string emailIdvalue = Convert.ToString(dtmail.Rows[0]["com_Email"]);
                            string passwordvalue = EncryptDecrypt.Decrypt(Convert.ToString(dtmail.Rows[0]["com_password"].ToString()));
                            int portvalue = Convert.ToInt32(dtmail.Rows[0]["com_Port"]);
                            if (passwordvalue == "" && smtpvalue == "" && portvalue == 0 && emailIdvalue == "")
                            {
                                ViewBag.vnew = "";
                                ViewBag.vedit = "";
                                ViewBag.vsave = "disabled='disabled'";
                                ViewBag.vsavenew = "disabled='disabled'";
                                ViewBag.scripCall = "showmsgJS(1, 'Please Configure Your Mail First', 2);";
                                //goto END;
                                return View(model);
                            }
                            else
                            {
                                string[] unitid = string.Join(",", model[0].SelectedItems7).Split(',');
                                try
                                {
                                    for (Int32 x = 0; x < unitid.Length; x++)
                                    {
                                        unit_name = unit_name + "," + ((List<SelectListItem>)TempData[MyGuid + "_Tlist7"]).SingleOrDefault(item => item.Value == unitid[x]).Text.ToString();
                                    }
                                }
                                catch (Exception ex) { }

                                string module_id = string.Join(",", model[0].SelectedItems8).ToString();
                                role_mst = model[0].Col33;
                                if (role_mst == null) { role_mst = ""; }
                                string teamid = string.Join(",", model[0].SelectedItems10).ToString();

                                for (int i = 0; i < model.Count; i++)
                                {
                                    DataRow dr = dataTable.NewRow();
                                    dr["rec_id"] = "0";
                                    dr["type"] = model[0].Col12;
                                    dr["vch_date"] = currdate;
                                    dr["vch_num"] = vch_num.Trim();
                                    dr["per_id"] = vch_num.Trim();
                                    dr["user_id"] = model[0].Col20;
                                    dr["password"] = pwd;
                                    dr["email"] = model[0].Col29;
                                    dr["department"] = model[0].SelectedItems4.FirstOrDefault();

                                    dr["Unit_Code"] = "-";
                                    dr["role"] = role_mst.TrimStart(',');
                                    dr["status"] = "active";
                                    dr["email_right"] = outside_permission;
                                    dr["first_name"] = model[0].Col23;
                                    dr["last_name"] = model[0].Col24;
                                    dr["cur_country"] = model[0].SelectedItems1.FirstOrDefault();
                                    dr["cur_state"] = model[0].SelectedItems2.FirstOrDefault(); ;
                                    dr["cur_city"] = mannual_city;
                                    dr["trn_approval_admin"] = model[0].SelectedItems9.FirstOrDefault();
                                    dr["designation"] = model[0].SelectedItems5.FirstOrDefault();
                                    dr["mod_id"] = module_id;

                                    dr["std_type"] = model[0].SelectedItems11.FirstOrDefault();
                                    dr["emp_id"] = model[0].Col19;

                                    dr["con_number"] = model[0].Col28;
                                    dr["dob"] = sgen.Savedate(model[0].Col25, false);
                                    dr["todo"] = model[0].Chk10 == true ? "Y" : "N";//show todo
                                    dr["team"] = teamid;

                                    if (isedit)
                                    {
                                        dr["client_id"] = model[0].Col1.Trim();
                                        dr["client_unit_id"] = model[0].Col2.Trim();
                                        dr["ent_by"] = model[0].Col3;
                                        dr["ent_date"] = currdate;
                                        dr["edit_by"] = userid_mst;
                                        dr["edit_date"] = currdate;
                                    }
                                    else
                                    {
                                        dr["client_id"] = client_id;
                                        dr["client_unit_id"] = string.Join(",", model[0].SelectedItems7);
                                        dr["ent_by"] = userid_mst;
                                        dr["ent_date"] = currdate;
                                        dr["edit_by"] = "-";
                                        dr["edit_date"] = currdate;
                                    }

                                    dataTable.Rows.Add(dr);
                                }

                                bool Result = sgen.Update_data(userCode, dataTable, model[0].Col10.Trim(), model[0].Col8, isedit);

                                if (Result == true)
                                {
                                    string msgbody = "<b style='color: #3caee9; font-size: 20px'>Login Details From Admin</b><br /><hr style='border:1px solid black' /><p>Hi " +
                                    "<b>" + model[0].Col20 + "</b>,</p><p>Congratulations, This is to notify that your user login has been created. your login credentials " +
                                    "are as below :-</p><table style='font-weight:600'><tr><td>Company Name </td><td>: " + clientname.TrimEnd(',') + " </td></tr><tr><td> " +
                                    "Unit Name </td><td>: " + unit_name.TrimStart(',') + " </td></tr><tr><td> User id </td><td>: " + model[0].Col20 + " </td></tr><tr><td> " +
                                    "Password </td><td>: " + model[0].Col21 + " </td> </tr><tr><td> URL </td><td>: " + subdomain_mst + " </td></tr></table><br /><p>Please " +
                                    "login and change Your Password. Kindly do not share your login credentials</p><p>Thank you,<br />Administrator<br /></p><br /><p>Note:- " +
                                    "Please do not reply to this mail as it is an unmonitered alias.</p>";


                                    try
                                    {
                                        MailMessage msg = new MailMessage();
                                        msg.From = new MailAddress(emailIdvalue);

                                        msg.Bcc.Add("gsthelp001@gmail.com");
                                        msg.To.Add(model[0].Col29);
                                        msg.Subject = "Login_Detail";
                                        msg.Body = msgbody;
                                        msg.IsBodyHtml = true;
                                        SmtpClient smtp = new SmtpClient();
                                        smtp.Host = smtpvalue;
                                        System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                                        NetworkCred.UserName = emailIdvalue;
                                        NetworkCred.Password = passwordvalue;
                                        smtp.UseDefaultCredentials = true;
                                        smtp.Credentials = NetworkCred;
                                        smtp.Port = portvalue;
                                        smtp.EnableSsl = true;
                                        smtp.Send(msg);
                                        msg.Dispose();
                                    }
                                    catch (Exception ex)
                                    {
                                        ViewBag.scripCall = "showmsgJS(1, 'User Created Successfully." + System.Environment.NewLine + "Mail setting have problem." + System.Environment.NewLine + " (" + ex.Message.ToString() + ")', 0);";
                                    }
                                    //ViewBag.scripCall = "showmsgJS(1, 'User created, please check ur mail for detail', 1);";

                                    TempData[MyGuid + "_Tlist1"] = mod1;
                                    TempData[MyGuid + "_Tlist2"] = mod2;
                                    TempData[MyGuid + "_Tlist3"] = mod3;
                                    TempData[MyGuid + "_Tlist4"] = mod4;
                                    TempData[MyGuid + "_Tlist5"] = mod5;
                                    TempData[MyGuid + "_Tlist6"] = mod6;
                                    TempData[MyGuid + "_Tlist7"] = mod7;
                                    TempData[MyGuid + "_Tlist8"] = mod8;
                                    TempData[MyGuid + "_Tlist9"] = mod9;
                                    TempData[MyGuid + "_Tlist10"] = mod10;
                                    TempData[MyGuid + "_Tlist11"] = mod11;
                                    var tmodel1 = model.FirstOrDefault();
                                    model = new List<Tmodelmain>();
                                    model.Add(new Tmodelmain
                                    {
                                        Col17 = "",
                                        Col1 = "",
                                        Col13 = "Save",
                                        Col100 = "Save & New",
                                        Col121 = "S",
                                        Col122 = "<u>S</u>ave",
                                        Col123 = "Save & Ne<u>w</u>",
                                        Col9 = "CREATION OF USER",
                                        Col10 = tmodel1.Col10,
                                        Col11 = tmodel1.Col11,
                                        Col12 = tmodel1.Col12,
                                        Col14 = tmodel1.Col14,
                                        Col15 = tmodel1.Col15,
                                        TList1 = mod1,
                                        TList2 = mod2,
                                        TList3 = mod3,
                                        TList4 = mod4,
                                        TList5 = mod5,
                                        TList6 = mod6,
                                        TList7 = mod7,
                                        TList8 = mod8,
                                        TList9 = mod9,
                                        TList10 = mod10,
                                        TList11 = mod11,

                                        SelectedItems1 = new string[] { "" },
                                        SelectedItems2 = new string[] { "" },
                                        SelectedItems3 = new string[] { "" },
                                        SelectedItems4 = new string[] { "" },
                                        SelectedItems5 = new string[] { "" },
                                        SelectedItems6 = new string[] { "" },
                                        SelectedItems7 = new string[] { "" },
                                        SelectedItems8 = new string[] { "" },
                                        SelectedItems9 = new string[] { "" },
                                        SelectedItems10 = new string[] { "" },
                                        SelectedItems11 = new string[] { "" }
                                    });
                                    if (command.Trim().ToUpper().Equals("SAVE") || command.Trim().ToUpper().Equals("UPDATE"))
                                    {
                                        ViewBag.vnew = "";
                                        ViewBag.vedit = "";
                                        ViewBag.vsave = "disabled='disabled'";
                                        ViewBag.vsavenew = "disabled='disabled'";
                                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'User created, please check ur mail for detail');disableForm();";
                                    }
                                    else if (command.Trim().ToUpper().Equals("SAVE & NEW") || command.Trim().ToUpper().Equals("UPDATE & NEW"))
                                    {
                                        try
                                        {
                                            model = new_create_user(model);
                                            ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'User created, please check ur mail for detail');";
                                        }
                                        catch (Exception ex) { }
                                    }
                                }
                                else
                                {
                                    ViewBag.scripCall = "showmsgJS(1, 'Error in Saving Record, Please Check Each Value Again', 0);";
                                }
                            }
                        }
                        #endregion
                        //END:;
                    }
                    catch (Exception ex) { }
                }
                else if (command == "country")
                {
                    try
                    {
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                        ViewBag.scripCall = "enableForm();";
                        string country_id = string.Join(",", model[0].SelectedItems1);
                        DataTable dt2 = new DataTable();
                        dt2 = sgen.getdata(userCode, "select distinct state_name,state_gst_code from country_state where alpha_2='" + country_id + "'  and state_name!='-'  order by state_name");
                        if (dt2.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt2.Rows)
                            {
                                mod2.Add(new SelectListItem { Text = dr["state_name"].ToString(), Value = dr["state_gst_code"].ToString() });

                            }
                        }
                        TempData[MyGuid + "_Tlist2"] = mod2;

                        model[0].TList2 = mod2;
                    }
                    catch (Exception ex)
                    {
                        msg = Regex.Replace(ex.Message.ToString().Replace("'", ""), @"\t|\n|\r", "");
                        ViewBag.scripCall = "showmsgJS(1, '" + msg + "', 1);";
                    }
                }
                else if (command == "state")
                {
                    try
                    {
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                        ViewBag.scripCall = "enableForm();";
                        string state_id = string.Join(",", model[0].SelectedItems2);
                        DataTable dt2 = new DataTable();
                        dt2 = sgen.getdata(userCode, "SELECT city_name FROM (SELECT distinct city_name FROM country_state where state_gst_code='" + state_id + "' ) tab union SELECT 'Other' city_name from dual");
                        if (dt2.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt2.Rows)
                            {
                                mod3.Add(new SelectListItem { Text = dr["city_name"].ToString(), Value = dr["city_name"].ToString() });

                            }
                        }
                        TempData[MyGuid + "_Tlist3"] = mod3;

                        model[0].TList3 = mod3;
                    }
                    catch (Exception ex)
                    {
                        msg = Regex.Replace(ex.Message.ToString().Replace("'", ""), @"\t|\n|\r", "");
                        ViewBag.scripCall = "showmsgJS(1, '" + msg + "', 1);";
                    }
                }
                else if (command == "company")
                {
                    string where = "";
                    try
                    {
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                        ViewBag.scripCall = "enableForm();";
                        string[] comp = string.Join(",", model[0].SelectedItems6).Split(',');


                        DataTable dtunit = new DataTable();
                        mod7 = new List<SelectListItem>();
                        //string mq0 = sgen.seekval(userCode, "select client_unit_id from user_details where rec_id='" + Convert.ToInt32(userid_mst) + "'", "client_unit_id");
                        string mq0 = sgen.seekval(userCode, "select client_unit_id from user_details where vch_num='" + userid_mst + "'", "client_unit_id");
                        if (!role_mst.ToUpper().Equals("OWNER")) where = " and cup_id in ('" + mq0.Replace(",", "','") + "')";

                        for (int i = 0; i < comp.Length; i++)
                        {
                            mq = "SELECT  cup_id,unit_name ||'('|| cup_Id ||')' as nameid FROM company_unit_profile where company_profile_id='" + comp[i] + "'" + where + "";
                            dtunit = sgen.getdata(userCode, mq);

                            if (dtunit.Rows.Count > 0)
                            {

                                foreach (DataRow dr in dtunit.Rows)
                                {
                                    mod7.Add(new SelectListItem { Text = dr["nameid"].ToString(), Value = dr["cup_id"].ToString() });

                                }

                            }

                            TempData[MyGuid + "_Tlist7"] = mod7;

                            model[0].TList7 = mod7;
                        }

                    }
                    catch (Exception ex)
                    {
                        msg = Regex.Replace(ex.Message.ToString().Replace("'", ""), @"\t|\n|\r", "");
                        ViewBag.scripCall = "showmsgJS(1, '" + msg + "', 1);";
                    }
                }
                else if (command == "module")
                {

                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vsavenew = "";
                    ViewBag.scripCall = "enableForm();";

                }
                else if (command == "Cancel")
                {
                    return CancelFun(model);
                }
                else if (command == "Callback")
                {
                    try
                    {
                        if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
                        model = CallbackFun(btnval, "N", "create_user", "Home", model);
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";

                    }
                    catch (Exception ex)
                    {
                        msg = Regex.Replace(ex.Message.ToString().Replace("'", ""), @"\t|\n|\r", "");
                        ViewBag.scripCall = "showmsgJS(1, '" + msg + "', 1);";
                    }
                }
            }
            catch (Exception ex)
            {
                msg = Regex.Replace(ex.Message.ToString().Replace("'", ""), @"\t|\n|\r", "");
                ViewBag.scripCall = "showmsgJS(1, '" + msg + "', 1);";
            }
            ModelState.Clear();
            return View(model);
        }
        #endregion

        #region rnd

        public DataTable Mymenu2()
        {
            FillMst();
            sgenFun sgen = new sgenFun(MyGuid);


            string m_module1 = sgen.GetCookie(MyGuid, "m_module1").ToString();
            string m_module2 = sgen.GetCookie(MyGuid, "m_module2").ToString();
            string m_module3 = sgen.GetCookie(MyGuid, "m_module3").ToString();
            string ulevel_mst = sgen.GetCookie(MyGuid, "ulevel_mst").ToString();
            string m_id = sgen.GetCookie(MyGuid, "m_id").ToString();
            string utype_mst = sgen.GetCookie(MyGuid, "utype_mst").ToString();
            string mq = "";

            mq = "select b.* from menus b where b.m_module3='invtmain' order by m_id, m_order";

            DataTable dtparent = sgen.getdata(userCode, mq);
            DataTable dtcopy1 = dtparent.Clone();

            DataTable dt0 = dtparent.AsEnumerable().Where(w => (decimal)w["m_level"] == 1).Select(s => s).CopyToDataTable();
            html = "<ul id='myMenu' class='nav side-menu'>";
            foreach (DataRow dr in dt0.Rows)
            {
                dtcopy1.ImportRow(dr);
                if (dr["m_submenu"].ToString().Equals("False"))
                {
                    cli++;
                }
                else
                {
                    cli++;
                    makemenu2(dtparent, dr["m_module3"].ToString(), dr["m_module3"].ToString(), Convert.ToInt32(dr["m_level"].ToString()) + 1, dtcopy1);
                }
            }
            sgen.SetSession(MyGuid, "dtcopy", dtcopy1);
            return dtcopy1;

        }
        private void makemenu2(DataTable dtparent, string module3, string module1, int level, DataTable dtcopy1)
        {
            string m_id = sgen.GetCookie(MyGuid, "m_id").ToString();
            try
            {
                DataTable dtstatuswise = dtparent.AsEnumerable().Where(w => (string)w["m_module3"] == module3 &&
                                (string)w["m_module2"] == module1 && (decimal)w["m_level"] == level)
                                                    .Select(s => s).CopyToDataTable();

                foreach (DataRow dr in dtstatuswise.Rows)
                {
                    dtcopy1.ImportRow(dr);
                    if (dr["m_submenu"].ToString().Equals("False"))
                    {
                        cli++;
                    }
                    else
                    {
                        cli++;
                        makemenu2(dtparent, dr["m_module3"].ToString(), dr["m_module1"].ToString(), Convert.ToInt32(dr["m_level"].ToString()) + 1, dtcopy1);
                    }
                }
            }
            catch { }
        }

        public ActionResult rnd()
        {
            //var dtt = Mymenu2();
            List<Tmodelmain> model = new List<Tmodelmain>();
            Tmodelmain tm = new Tmodelmain();
            model = new List<Tmodelmain>();
            model.Add(new Tmodelmain { Col10 = "master_setting" });
            return View(model);
        }
        [HttpPost]
        public ActionResult rnd(List<Tmodelmain> model, string command, string hfrow, string hftable)
        {
            FillMst();
            ent_date = sgen.server_datetime(userCode);



            model = new List<Tmodelmain>();
            model.Add(new Tmodelmain { Col10 = "master_setting" });
            //update master_setting set classid=nvl(substr(master_name,0,2),'0'),sectionid=nvl(substr(master_name,4,2),'0'),subjectid=nvl(substr(master_name,7,1),'0'),   client_name = nvl(substr(master_name, 9, 1), '0'),optional_subject = nvl(substr(master_name, 10, 1), 'Z')  where type = 'IIL'

            Tmodelmain tm = new Tmodelmain();
            if (command == "run")
            {


                DataTable dt1 = sgen.getdata(userCode, "select distinct classid from master_setting where type='IIL'");
                isedit = false;

                string type1 = "HBM", type2 = "IN0", type3 = "IN1", type4 = "IN2", type5 = "IN3";

                foreach (DataRow dr1 in dt1.Rows)
                {
                    string col1 = "0", col2 = "0", col3 = "0", col4 = "0";

                    type = type1;
                    string name = dr1[0].ToString().Trim();
                    DataTable dtstr = new DataTable();
                    //dtstr = sgen.getdata(userCode, "select * from " + model[0].Col10.Trim() + " WHERE 1=2");
                    dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10.Trim() + " where type='" + type + "' and client_unit_id='001001'";
                    vch_num = sgen.genNo(userCode, mq, 3, "auto_genid");


                    #region dtstr module

                    DataRow dr = dtstr.NewRow();
                    dr["master_id"] = vch_num.Trim();
                    dr["vch_date"] = ent_date;
                    dr["type"] = type.Trim();
                    dr["master_name"] = sgen.ProperCase(name);
                    dr["classid"] = col1;
                    dr["sectionid"] = col2;
                    dr["client_name"] = col3;
                    dr["subjectid"] = col4;

                    dr["Status"] = "Y";
                    dr["Inactive_date"] = ent_date;


                    {
                        dr["rec_id"] = "0";
                        dr["master_entby"] = userid_mst;
                        dr["master_entdate"] = ent_date;
                        dr["master_editby"] = "-";
                        dr["master_editdate"] = ent_date;
                        dr["client_id"] = clientid_mst;
                        dr["client_unit_id"] = unitid_mst;
                    }
                    dtstr.Rows.Add(dr);
                    col1 = vch_num;
                    #endregion

                    bool Result = sgen.Update_data(userCode, dtstr, model[0].Col10.Trim(), model[0].Col8, isedit);



                    DataTable dt2 = sgen.getdata(userCode, "select distinct sectionid from master_setting where type='IIL' and trim(classid)='" + dr1[0].ToString().Trim() + "' ");
                    foreach (DataRow dr2 in dt2.Rows)
                    {

                        string val = dr1[0].ToString();

                        type = type2;
                        name = dr2[0].ToString().Trim();
                        dtstr = new DataTable();
                        //dtstr = sgen.getdata(userCode, "select * from " + model[0].Col10.Trim() + " WHERE 1=2");
                        dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                        mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10.Trim() + " where type='" + type + "' and client_unit_id='001001'";
                        vch_num = sgen.genNo(userCode, mq, 3, "auto_genid");

                        #region dtstr module

                        dr = dtstr.NewRow();
                        dr["master_id"] = vch_num.Trim();
                        dr["vch_date"] = ent_date;
                        dr["type"] = type.Trim();
                        dr["master_name"] = sgen.ProperCase(name);
                        dr["classid"] = col1;
                        dr["sectionid"] = col2;
                        dr["client_name"] = col3;
                        dr["subjectid"] = col4;

                        dr["Status"] = "Y";
                        dr["Inactive_date"] = ent_date;


                        {
                            dr["rec_id"] = "0";
                            dr["master_entby"] = userid_mst;
                            dr["master_entdate"] = ent_date;
                            dr["master_editby"] = "-";
                            dr["master_editdate"] = ent_date;
                            dr["client_id"] = clientid_mst;
                            dr["client_unit_id"] = unitid_mst;
                        }
                        dtstr.Rows.Add(dr);
                        col2 = vch_num;
                        #endregion

                        Result = sgen.Update_data(userCode, dtstr, model[0].Col10.Trim(), model[0].Col8, isedit);

                        DataTable dt3 = sgen.getdata(userCode, "select distinct subjectid from master_setting where type='IIL' and " +
                            "trim(classid)='" + dr1[0].ToString().Trim() + "' and  trim(sectionid)='" + dr2[0].ToString().Trim() + "'");
                        foreach (DataRow dr3 in dt3.Rows)
                        {
                            type = type3;
                            name = dr3[0].ToString().Trim();
                            dtstr = new DataTable();
                            //dtstr = sgen.getdata(userCode, "select * from " + model[0].Col10.Trim() + " WHERE 1=2");
                            dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());

                            mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10.Trim() + " where type='" + type + "' and client_unit_id='001001'";
                            vch_num = sgen.genNo(userCode, mq, 3, "auto_genid");

                            #region dtstr module

                            dr = dtstr.NewRow();
                            dr["master_id"] = vch_num.Trim();
                            dr["vch_date"] = ent_date;
                            dr["type"] = type.Trim();
                            dr["master_name"] = sgen.ProperCase(name);
                            dr["classid"] = col1;
                            dr["sectionid"] = col2;
                            dr["client_name"] = col3;
                            dr["subjectid"] = col4;

                            dr["Status"] = "Y";
                            dr["Inactive_date"] = ent_date;


                            {
                                dr["rec_id"] = "0";
                                dr["master_entby"] = userid_mst;
                                dr["master_entdate"] = ent_date;
                                dr["master_editby"] = "-";
                                dr["master_editdate"] = ent_date;
                                dr["client_id"] = clientid_mst;
                                dr["client_unit_id"] = unitid_mst;
                            }
                            dtstr.Rows.Add(dr);
                            col3 = vch_num;
                            #endregion

                            Result = sgen.Update_data(userCode, dtstr, model[0].Col10.Trim(), model[0].Col8, isedit);

                            DataTable dt4 = sgen.getdata(userCode, "select distinct client_name from master_setting where type='IIL' and " +
                     "trim(classid)='" + dr1[0].ToString().Trim() + "' and  trim(sectionid)='" + dr2[0].ToString().Trim() + "' and trim(subjectid)='" + dr3[0].ToString().Trim() + "'");
                            foreach (DataRow dr4 in dt4.Rows)
                            {


                                type = type4;
                                name = dr4[0].ToString().Trim();
                                dtstr = new DataTable();
                                //dtstr = sgen.getdata(userCode, "select * from " + model[0].Col10.Trim() + " WHERE 1=2");
                                dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());

                                mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10.Trim() + " where type='" + type + "' and client_unit_id='001001'";
                                vch_num = sgen.genNo(userCode, mq, 3, "auto_genid");

                                #region dtstr module

                                dr = dtstr.NewRow();
                                dr["master_id"] = vch_num.Trim();
                                dr["vch_date"] = ent_date;
                                dr["type"] = type.Trim();
                                dr["master_name"] = sgen.ProperCase(name);
                                dr["classid"] = col1;
                                dr["sectionid"] = col2;
                                dr["client_name"] = col3;
                                dr["subjectid"] = col4;

                                dr["Status"] = "Y";
                                dr["Inactive_date"] = ent_date;


                                {
                                    dr["rec_id"] = "0";
                                    dr["master_entby"] = userid_mst;
                                    dr["master_entdate"] = ent_date;
                                    dr["master_editby"] = "-";
                                    dr["master_editdate"] = ent_date;
                                    dr["client_id"] = clientid_mst;
                                    dr["client_unit_id"] = unitid_mst;
                                }
                                dtstr.Rows.Add(dr);
                                col4 = vch_num;
                                #endregion

                                Result = sgen.Update_data(userCode, dtstr, model[0].Col10.Trim(), model[0].Col8, isedit);

                                DataTable dt5 = sgen.getdata(userCode, "select distinct optional_subject from master_setting where type='IIL' and " +
      "trim(classid)='" + dr1[0].ToString().Trim() + "' and  trim(sectionid)='" + dr2[0].ToString().Trim() + "' and trim(subjectid)='" + dr3[0].ToString().Trim() + "'" +
      " and trim(client_name)='" + dr4[0].ToString() + "'");
                                foreach (DataRow dr5 in dt5.Rows)
                                {

                                    type = type5;
                                    name = dr5[0].ToString().Trim();
                                    dtstr = new DataTable();
                                    //dtstr = sgen.getdata(userCode, "select * from " + model[0].Col10.Trim() + " WHERE 1=2");
                                    dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());

                                    mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10.Trim() + " where type='" + type + "' and client_unit_id='001001'";
                                    vch_num = sgen.genNo(userCode, mq, 3, "auto_genid");

                                    #region dtstr module

                                    dr = dtstr.NewRow();
                                    dr["master_id"] = vch_num.Trim();
                                    dr["vch_date"] = ent_date;
                                    dr["type"] = type.Trim();
                                    dr["master_name"] = sgen.ProperCase(name);
                                    dr["classid"] = col1;
                                    dr["sectionid"] = col2;
                                    dr["client_name"] = col3;
                                    dr["subjectid"] = col4;

                                    dr["Status"] = "Y";
                                    dr["Inactive_date"] = ent_date;


                                    {
                                        dr["rec_id"] = "0";
                                        dr["master_entby"] = userid_mst;
                                        dr["master_entdate"] = ent_date;
                                        dr["master_editby"] = "-";
                                        dr["master_editdate"] = ent_date;
                                        dr["client_id"] = clientid_mst;
                                        dr["client_unit_id"] = unitid_mst;
                                    }
                                    dtstr.Rows.Add(dr);

                                    #endregion

                                    Result = sgen.Update_data(userCode, dtstr, model[0].Col10.Trim(), model[0].Col8, isedit);

                                    String FSTR = clientid_mst + unitid_mst + col1 + col2 + col3 + col4 + vch_num;

                                    string whr = "client_id||client_unit_id||trim(classid)||trim(sectionid)||trim(subjectid)||trim(client_name)||trim(optional_subject)" +
                                        "='" + clientid_mst + unitid_mst + dr1[0].ToString().Trim() + dr2[0].ToString().Trim() + dr3[0].ToString().Trim() + dr4[0].ToString().Trim() + dr5[0].ToString().Trim() + "'";
                                    mq1 = "update master_setting set cont_person_name='" + FSTR + "'," +
                                        "cont_person_email='" + dr1[0].ToString().Trim() + "-" + dr2[0].ToString().Trim() + "-" + dr3[0].ToString().Trim() + "-"
                                        + dr4[0].ToString().Trim() + dr5[0].ToString().Trim() + "' where " + whr + "";
                                    sgen.execute_cmd(userCode, mq1);
                                    string tid = sgen.seekval(userCode, "select master_id from master_setting where " + whr + "", "master_id");

                                    mq1 = "update item set loc='" + FSTR + "' where trim(loc)='" + tid.Trim() + "'";
                                    sgen.execute_cmd(userCode, mq1);

                                }
                            }
                        }
                    }
                }
            }
            else if (command == "Tally Connect")
            {
                string XMLData = CreateXMLParty("abc", "VENDOR 6", "-", "-", "RAM@GMAIL.COM", "00000", "0.4", "10", "Sundry Creditors", "-", "0", "0");
                //string XMLData = PurchaseVoucher("1-Apr-2019", "00002", "00120", "VENDOR 3", "004", "100", "-100");
                XMLData = JV("VENDOR 6", "Wages", "1-Apr-2019", "004", "525", "009", "24-04-2019", "My Naration from Code");

                sgen.Tally_Sync(XMLData);
                //XMLData = JV("Customer 1", "Salary and Wages", "1-Apr-2019", "004", "525", "009", "24-04-2019", "My Naration from Code");
                HttpWebRequest TallyRequest;
                string RequestXML = "";
                RequestXML = XMLData; // Called XML Function
                TallyRequest = (HttpWebRequest)WebRequest.Create("http://localhost:9000");
                TallyRequest.UserAgent = ".NET Framework Example Client";
                TallyRequest.Method = "POST";
                string postData = RequestXML;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                TallyRequest.ContentType = "application/x-www-form-urlencoded";
                TallyRequest.ContentLength = byteArray.Length;
                try
                {
                    Stream dataStream = TallyRequest.GetRequestStream();
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();
                    HttpWebResponse response1 = (HttpWebResponse)TallyRequest.GetResponse();
                    string Response = response1.StatusDescription.ToString();
                    dataStream = response1.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    string responseFromTallyServer = reader.ReadToEnd().ToString();
                    bool ErrorCheck = false;
                    ErrorCheck = responseFromTallyServer.Contains("<LINEERROR>");
                    bool BA;
                    string qry = responseFromTallyServer;
                    BA = qry.Contains("<ERRORS>1</ERRORS>");
                    if (!ErrorCheck & !BA)
                    {
                        DataSet TallyResponseDataSet = new DataSet();
                        TallyResponseDataSet.ReadXml(new StringReader(responseFromTallyServer));
                        string MasterId = "";
                        qry = "";
                        qry = responseFromTallyServer;
                        var Tem = qry.IndexOf("<LASTVCHID>") + "<LASTVCHID>".Length;
                        var Tem1 = qry.IndexOf("</LASTVCHID>");
                        MasterId = qry.Substring(System.Convert.ToInt32(Tem), System.Convert.ToInt32(Tem1 - Tem)).Trim();
                        reader.Close();
                        dataStream.Close();
                        response1.Close();
                        byteArray = null;
                        Response = null;
                        responseFromTallyServer = null;
                        Response = null;
                        dataStream = null;
                    }
                    else
                    {
                        //MsgBox("Failed to POST " + responseFromTallyServer + " ", MsgBoxStyle.Critical);
                    }
                }
                catch (Exception ex)
                {
                    //Interaction.MsgBox(ex.Message.ToString(), MsgBoxStyle.Critical);
                }
            }
            else if (command == "IndiaMart")
            {

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string json = (new WebClient()).DownloadString("https://mapi.indiamart.com/wservce/enquiry/listing/GLUSR_MOBILE/8860009141/GLUSR_MOBILE_KEY/MTU4MzQ4MTAzNi43MDA5IzIyNjAxNDg4/Start_Time/01-MAR-2020/End_Time/6-MAR-2020/");
                var data = JsonConvert.DeserializeObject<DataTable>(json);

            }
            else if (command == "99Acres")
            {
                Mailer.Kaamkar("", "");


            }
            else if (command == "sess")
            {

                string vv = "";
                try
                {
                    vv = HttpContext.Application["myvalue"] as string;
                }
                catch (Exception err) { }
                HttpContext.Application["myvalue"] = vv + DateTime.Now;
                try
                {
                    vv = HttpContext.Application["myvalue"] as string;
                }
                catch (Exception err) { }

                model[0].Col21 = vv;
            }
            return View(model);
        }


        public ActionResult rndgrid()
        {
            List<UserModel> users = UserModel.getUsers();
            return View(users);
        }
        [HttpPost]
        public ActionResult rndgrid(List<UserModel> model)
        {
            List<UserModel> users = UserModel.getUsers();
            return View(users);
        }
        public ActionResult gridag()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model.Add(new Tmodelmain { Col1 = "" });
            DataTable dt = sgen.getdata(userCode, "select vch_num,ispflimit,isesilimit,f_name from emp_master where client_unit_id='" + unitid_mst + "'");
            model[0].dt1 = dt;

            List<UserModel> users = UserModel.getUsers();
            return View(model);
        }
        [HttpPost]
        public ActionResult gridag(List<UserModel> model)
        {
            List<UserModel> users = UserModel.getUsers();

            return View(users);
        }
        public JsonResult GetStudents()
        {

            var students = new List<Students>();
            DataTable dt = new DataTable();
            dt.Columns.Add("iD");
            dt.Columns.Add("firstName");
            dt.Columns.Add("lastName");
            dt.Columns.Add("feesPaid");
            dt.Columns.Add("gender");
            dt.Columns.Add("emailId");
            dt.Columns.Add("telephoneNumber");
            dt.Columns.Add("dateOfBirth");
            for (int a = 0; a < 100; a++)
            {
                DataRow student = dt.NewRow();

                student["iD"] = a + 1;
                student["firstName"] = "Name " + a;
                student["lastName"] = "Last Name " + a;
                student["feesPaid"] = a * 10;
                student["gender"] = "M";
                student["emailId"] = "Ram@ram.com";
                student["telephoneNumber"] = "10101010010";
                student["dateOfBirth"] = DateTime.Now;

                dt.Rows.Add(student);
            }

            var js = new JavaScriptSerializer();
            return Json(JsonConvert.SerializeObject(dt), JsonRequestBehavior.AllowGet);
        }
        class Students
        {
            public int iD { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public int feesPaid { get; set; }
            public string gender { get; set; }
            public string emailId { get; set; }
            public string telephoneNumber { get; set; }
            public DateTime dateOfBirth { get; set; }
        }
        public JsonResult GetPerson()
        {
            List<Person> persons = new List<Person>();
            for (int a = 0; a < 100; a++)
            {
                Person p = new Person();
                p.ID = a + 1;
                p.Name = "Name " + a;
                p.Age = a;
                p.Country = 6;
                p.Address = "address " + a;
                p.Married = true;
                persons.Add(p);
            }
            return Json(JsonConvert.SerializeObject(persons), JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadData()
        {
            try
            {
                //Creating instance of DatabaseContext class  
                //using (DatabaseContext _context = new DatabaseContext())
                //{
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();


                //Paging Size (10,20,50,100)    
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;


                DataTable dt = new DataTable();
                dt.Columns.Add("CustomerID");
                dt.Columns.Add("CompanyName");
                dt.Columns.Add("ContactName");
                dt.Columns.Add("ContactTitle");
                dt.Columns.Add("City");
                dt.Columns.Add("PostalCode");
                dt.Columns.Add("Country");
                dt.Columns.Add("Phone");
                for (int a = 0; a < 100; a++)
                {
                    DataRow student = dt.NewRow();

                    student[0] = a + 1;
                    student[1] = "Name " + a;
                    student[2] = "Last Name " + a;
                    student[3] = a * 10;
                    student[4] = "M";
                    student[5] = "Ram@ram.com";
                    student[6] = "10101010010";
                    student[7] = "10101010010";

                    dt.Rows.Add(student);
                }
                //// Getting all Customer data    
                //var customerData = (from tempcustomer in _context.Customers
                //                    select tempcustomer);

                ////Sorting    
                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                //{
                //    customerData = customerData.OrderBy(sortColumn + " " + sortColumnDir);
                //}
                ////Search    
                //if (!string.IsNullOrEmpty(searchValue))
                //{
                //    customerData = customerData.Where(m => m.CompanyName == searchValue);
                //}

                ////total number of rows count     
                //recordsTotal = customerData.Count();
                ////Paging     
                //var data = customerData.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data    
                return Json(JsonConvert.SerializeObject(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = dt.AsEnumerable().ToList() })
                    , JsonRequestBehavior.AllowGet);
                //}
            }
            catch (Exception)
            {
                throw;
            }

        }
        public JsonResult GetStudent()
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();


            //Paging Size (10,20,50,100)    
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;


            DataTable dt = new DataTable();
            dt.Columns.Add("CustomerID");
            dt.Columns.Add("CompanyName");
            dt.Columns.Add("ContactName");
            dt.Columns.Add("ContactTitle");
            dt.Columns.Add("City");
            dt.Columns.Add("PostalCode");
            dt.Columns.Add("Country");
            dt.Columns.Add("Phone");
            for (int a = 0; a < 100; a++)
            {
                DataRow student = dt.NewRow();

                student[0] = a + 1;
                student[1] = "Name " + a;
                student[2] = "Last Name " + a;
                student[3] = a * 10;
                student[4] = "M";
                student[5] = "Ram@ram.com";
                student[6] = "10101010010";
                student[7] = "10101010010";

                dt.Rows.Add(student);
            }
            //// Getting all Customer data    
            //var customerData = (from tempcustomer in _context.Customers
            //                    select tempcustomer);

            ////Sorting    
            //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            //{
            //    customerData = customerData.OrderBy(sortColumn + " " + sortColumnDir);
            //}
            ////Search    
            //if (!string.IsNullOrEmpty(searchValue))
            //{
            //    customerData = customerData.Where(m => m.CompanyName == searchValue);
            //}

            ////total number of rows count     
            //recordsTotal = customerData.Count();
            ////Paging     
            //var data = customerData.Skip(skip).Take(pageSize).ToList();
            //Returning Json Data    
            return Json(JsonConvert.SerializeObject(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = dt.AsEnumerable().ToList() })
                , JsonRequestBehavior.AllowGet);

            //DataTable dt = new DataTable();
            //dt.Columns.Add("Name");
            //dt.Columns.Add("Surname");
            //dt.Columns.Add("Add");
            //dt.Columns.Add("Mobile");
            //DataRow dr = dt.NewRow();
            //dr[0] = "John";
            //dr[1] = "Doe";
            //dr[2] = "Street 34, California";
            //dr[3] = "180565823487";
            //dt.Rows.Add(dr);
            //dr = dt.NewRow();
            //dr[0] = "John";
            //dr[1] = "Doe";
            //dr[2] = "Street 34, California";
            //dr[3] = "180565823487";
            //dt.Rows.Add(dr);
            //dr = dt.NewRow();
            //dr[0] = "John";
            //dr[1] = "Doe";
            //dr[2] = "Street 34, California";
            //dr[3] = "180565823487";
            //dt.Rows.Add(dr);
            //dr = dt.NewRow();
            //dr[0] = "John";
            //dr[1] = "Doe";
            //dr[2] = "Street 34, California";
            //dr[3] = "180565823487";
            //dt.Rows.Add(dr);
            //dr = dt.NewRow();
            //dr[0] = "John";
            //dr[1] = "Doe";
            //dr[2] = "Street 34, California";
            //dr[3] = "180565823487";
            //dt.Rows.Add(dr);

            //return Json(JsonConvert.SerializeObject(dt), JsonRequestBehavior.AllowGet);
        }
        public class Student
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Address { get; set; }
            public string Mobile { get; set; }
        }
        public class Person
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
            public int Country { get; set; }
            public string Address { get; set; }
            public bool Married { get; set; }
        }
        #endregion

        #region 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CompName"> ##SVCURRENTCOMPANY is defaul</param>
        /// <param name="LedgerName"></param>
        /// <param name="MailingName"></param>
        /// <param name="Address"></param>
        /// <param name="Email"></param>
        /// <param name="Pincode"></param>
        /// <param name="Opening"></param>
        /// <param name="CreditDebit"></param>
        /// <param name="Group"></param>
        /// <param name="Phoneno"></param>
        /// <param name="TinNo"></param>
        /// <param name="SalesTaxNo"></param>
        /// <returns></returns>
        private string CreateXMLParty(string CompName, string LedgerName, string MailingName, string Address, string Email, string Pincode, string Opening,
            string CreditDebit, string Group, string Phoneno, string TinNo, string SalesTaxNo)
        {
            string x = "";
            x = "<ENVELOPE>";
            x = (x + "<HEADER>");
            x = (x + "<TALLYREQUEST>Import Data</TALLYREQUEST>");
            x = (x + "</HEADER>");
            x = (x + "<BODY>");
            x = (x + "<IMPORTDATA>");
            x = (x + " <REQUESTDESC>");
            x = (x + "<STATICVARIABLES>");
            x = (x + "<SVCURRENTCOMPANY>" + CompName + "</SVCURRENTCOMPANY>");
            x = (x + "</STATICVARIABLES>");
            x = (x + "<REPORTNAME>All Masters</REPORTNAME>");
            x = (x + "</REQUESTDESC>");
            x = (x + "<REQUESTDATA>");
            x = (x + "<TALLYMESSAGE xmlns:UDF='TallyUDF'>");
            x = (x + " <LEDGER NAME='" + LedgerName + "' ACTION='Create'>");
            x = (x + "<MAILINGNAME.LIST>");
            x = (x + "<MAILINGNAME>" + MailingName + "</MAILINGNAME>");
            x = (x + "</MAILINGNAME.LIST>");
            x = (x + " <ADDRESS>" + Address + " </ADDRESS>");
            x = (x + "<STATENAME></STATENAME>");
            x = (x + "<PINCODE>" + Pincode + "</PINCODE>");
            x = (x + "<LEDGERCONTACT></LEDGERCONTACT>");
            x = (x + "<LEDGERPHONE>" + Phoneno + "</LEDGERPHONE>");
            x = (x + "<LEDGERFAX></LEDGERFAX>");
            x = (x + " <LEDGERMOBILE>" + Phoneno + "</LEDGERMOBILE>");
            x = (x + "<EMAIL>" + Email + "</EMAIL>");
            x = (x + "<INCOMETAXNUMBER >" + TinNo + "</INCOMETAXNUMBER>"); // IT/PAN Number
            x = (x + "<VATTINNUMBER></VATTINNUMBER>");
            x = (x + "<CSTNUMBER></CSTNUMBER>");
            x = (x + "<SALESTAXNUMBER>" + SalesTaxNo + "</SALESTAXNUMBER>");
            x = (x + " <OPENINGBALANCE>" + Opening + "" + " " + CreditDebit + " </OPENINGBALANCE>");
            x = (x + " <NAME.LIST><NAME> " + LedgerName + "</NAME></NAME.LIST>");
            x = (x + "  <PARENT>" + Group + "</PARENT>");
            x = (x + " <ISSUBLEDGER>No</ISSUBLEDGER>");
            x = (x + "  <ISBILLWISEON>No</ISBILLWISEON>");
            x = (x + " <ISCOSTCENTRESON>No</ISCOSTCENTRESON>");
            x = (x + "</LEDGER>");
            x = (x + "</TALLYMESSAGE>");
            x = (x + "</REQUESTDATA>");
            x = (x + "</IMPORTDATA>");
            x = (x + "</BODY>");
            x = (x + "</ENVELOPE>");
            return x;
        }
        private string DeleteXMLParty(string LedgerName)
        {
            string x = "";
            x = (x + "<ENVELOPE>");
            x = (x + "<HEADER>");
            x = (x + "<TALLYREQUEST>Import Data</TALLYREQUEST>");
            x = (x + "</HEADER>");
            x = (x + "<BODY>");
            x = (x + "<IMPORTDATA>");
            x = (x + "<REQUESTDESC>");
            x = (x + "<REPORTNAME>All Masters</REPORTNAME>");
            x = (x + "</REQUESTDESC>");
            x = (x + "<REQUESTDATA>");
            x = (x + "<TALLYMESSAGE xmlns:UDF='TallyUDF'>");
            x = (x + "<LEDGER NAME='" + LedgerName + "' ACTION='Delete'>");
            x = (x + "<NAME.LIST>");
            x = (x + "<NAME>" + LedgerName + "</NAME>");
            x = (x + "</NAME.LIST>");
            x = (x + "</LEDGER>");
            x = (x + "</TALLYMESSAGE>");
            x = (x + "</REQUESTDATA>");
            x = (x + "</IMPORTDATA>");
            x = (x + "</BODY>");
            x = (x + "</ENVELOPE>");
            return x;
        }
        private string PurchaseVoucher(string strGRNDate, string strVoucherNo, string strGRNNo, string strSupplierName, string strPurchaseOrder,
            string strGRN, string strGRNValueNtv)
        {
            string xmlstc = "<ENVELOPE>" + "\r\n";
            xmlstc = xmlstc + "<HEADER>" + "\r\n";
            xmlstc = xmlstc + "<TALLYREQUEST>Import Data</TALLYREQUEST>" + "\r\n";
            xmlstc = xmlstc + "</HEADER>" + "\r\n";
            xmlstc = xmlstc + "<BODY>" + "\r\n";
            xmlstc = xmlstc + "<IMPORTDATA>" + "\r\n";
            xmlstc = xmlstc + "<REQUESTDESC>" + "\r\n";
            xmlstc = xmlstc + "<REPORTNAME>Vouchers</REPORTNAME>" + "\r\n";
            xmlstc = xmlstc + "<STATICVARIABLES>" + "\r\n";
            xmlstc = xmlstc + "<SVCURRENTCOMPANY>##SVCURRENTCOMPANY</SVCURRENTCOMPANY>" + "\r\n";
            xmlstc = xmlstc + "</STATICVARIABLES>" + "\r\n";
            xmlstc = xmlstc + "</REQUESTDESC>" + "\r\n";
            xmlstc = xmlstc + "<REQUESTDATA>" + "\r\n";
            xmlstc = xmlstc + "<TALLYMESSAGE xmlns:UDF=" + "\"" + "TallyUDF" + "\" >" + "\r\n";
            xmlstc = xmlstc + "<VOUCHER VCHTYPE=" + "\"" + "Purchase" + "\" >" + "\r\n";
            xmlstc = xmlstc + "<DATE>" + strGRNDate + "</DATE>" + "\r\n";
            xmlstc = xmlstc + "<VOUCHERTYPENAME>Purchase</VOUCHERTYPENAME>" + "\r\n";
            xmlstc = xmlstc + "<VOUCHERNUMBER>" + strVoucherNo + "</VOUCHERNUMBER>" + "\r\n";
            xmlstc = xmlstc + "<REFERENCE>" + strGRNNo + "</REFERENCE>" + "\r\n";
            xmlstc = xmlstc + "<PARTYLEDGERNAME>" + strSupplierName + "</PARTYLEDGERNAME>" + "\r\n";
            xmlstc = xmlstc + "<PARTYNAME>" + strSupplierName + "</PARTYNAME>" + "\r\n";
            xmlstc = xmlstc + "<EFFECTIVEDATE>" + strGRNDate + "</EFFECTIVEDATE>" + "\r\n";
            xmlstc = xmlstc + "<ISINVOICE>Yes</ISINVOICE>" + "\r\n";
            xmlstc = xmlstc + "<INVOICEORDERLIST.LIST>" + "\r\n";
            xmlstc = xmlstc + "<BASICORDERDATE>" + strGRNDate + "</BASICORDERDATE>" + "\r\n";
            xmlstc = xmlstc + "<BASICPURCHASEORDERNO>" + strPurchaseOrder + "</BASICPURCHASEORDERNO>" + "\r\n";
            xmlstc = xmlstc + "</INVOICEORDERLIST.LIST>" + "\r\n";
            xmlstc = xmlstc + "<ALLLEDGERENTRIES.LIST>" + "\r\n";
            xmlstc = xmlstc + "<LEDGERNAME>" + strSupplierName + "</LEDGERNAME>" + "\r\n";
            xmlstc = xmlstc + "<GSTCLASS/>" + "\r\n";
            xmlstc = xmlstc + "<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>" + "\r\n";
            xmlstc = xmlstc + "<LEDGERFROMITEM>No</LEDGERFROMITEM>" + "\r\n";
            xmlstc = xmlstc + "<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>" + "\r\n";
            xmlstc = xmlstc + "<ISPARTYLEDGER>Yes</ISPARTYLEDGER>" + "\r\n";
            xmlstc = xmlstc + "<AMOUNT>" + strGRN + "</AMOUNT>" + "\r\n";
            xmlstc = xmlstc + "<BILLALLOCATIONS.LIST>" + "\r\n";
            xmlstc = xmlstc + "<NAME>" + strGRNNo + "</NAME>" + "\r\n";
            xmlstc = xmlstc + "<BILLCREDITPERIOD>30 Days</BILLCREDITPERIOD>" + "\r\n";
            xmlstc = xmlstc + "<BILLTYPE>New Ref</BILLTYPE>" + "\r\n";
            xmlstc = xmlstc + "<AMOUNT>" + strGRN + "</AMOUNT>" + "\r\n";
            xmlstc = xmlstc + "</BILLALLOCATIONS.LIST>" + "\r\n";
            xmlstc = xmlstc + "</ALLLEDGERENTRIES.LIST>" + "\r\n";
            xmlstc = xmlstc + "<ALLLEDGERENTRIES.LIST>" + "\r\n";
            xmlstc = xmlstc + "<LEDGERNAME>RM</LEDGERNAME>" + "\r\n";
            xmlstc = xmlstc + "<GSTCLASS/>" + "\r\n";
            xmlstc = xmlstc + "<ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>" + "\r\n";
            xmlstc = xmlstc + "<LEDGERFROMITEM>Yes</LEDGERFROMITEM>" + "\r\n";
            xmlstc = xmlstc + "<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>" + "\r\n";
            xmlstc = xmlstc + "<ISPARTYLEDGER>No</ISPARTYLEDGER>" + "\r\n";
            xmlstc = xmlstc + "<AMOUNT>" + strGRNValueNtv + "</AMOUNT>" + "\r\n";
            xmlstc = xmlstc + "</ALLLEDGERENTRIES.LIST>" + "\r\n";
            xmlstc = xmlstc + "</VOUCHER>" + "\r\n";
            xmlstc = xmlstc + "</TALLYMESSAGE>" + "\r\n";
            xmlstc = xmlstc + "</REQUESTDATA>" + "\r\n";
            xmlstc = xmlstc + "</IMPORTDATA>" + "\r\n";
            xmlstc = xmlstc + "</BODY>" + "\r\n";
            xmlstc = xmlstc + "</ENVELOPE>" + "\r\n";
            return xmlstc;
        }
        private string Invoice(string custName, string InvDate, string InvNo, string InvAmt, string SalesOrderNo, string Discount,
            string Surcharge1, string Surcharge2,
            string Surcharge3, double NetAmount, string Discountldgr, string Srchargldgr1, string Srchargldgr2, string Srchargldgr3)
        {
            string xmlstc = "<ENVELOPE>" + "\r\n";
            try
            {
                xmlstc = xmlstc + "<HEADER>" + "\r\n";
                xmlstc = xmlstc + "<TALLYREQUEST>Import Data</TALLYREQUEST>" + "\r\n";
                xmlstc = xmlstc + "</HEADER>" + "\r\n";
                xmlstc = xmlstc + "<BODY>" + "\r\n";
                xmlstc = xmlstc + "<IMPORTDATA>" + "\r\n";
                xmlstc = xmlstc + "<REQUESTDESC>" + "\r\n";
                xmlstc = xmlstc + "<REPORTNAME>Vouchers</REPORTNAME>" + "\r\n";
                xmlstc = xmlstc + "<STATICVARIABLES>" + "\r\n";
                xmlstc = xmlstc + "<SVCURRENTCOMPANY>##SVCURRENTCOMPANY</SVCURRENTCOMPANY>" + "\r\n";
                xmlstc = xmlstc + "</STATICVARIABLES>" + "\r\n";
                xmlstc = xmlstc + "</REQUESTDESC>" + "\r\n";
                xmlstc = xmlstc + "<REQUESTDATA>" + "\r\n";
                xmlstc = xmlstc + "<TALLYMESSAGE xmlns:UDF=" + "\"" + "TallyUDF" + "\" >" + "\r\n";
                xmlstc = xmlstc + "<VOUCHER VCHTYPE=" + "\"" + "Sales" + "\" ACTION=" + "\"" + "Create" + "\" >" + "\r\n";
                xmlstc = xmlstc + "<DATE>" + InvDate + "</DATE>" + "\r\n";
                xmlstc = xmlstc + "<VOUCHERTYPENAME>Sales</VOUCHERTYPENAME>" + "\r\n";
                xmlstc = xmlstc + "<VOUCHERNUMBER>" + InvNo + "</VOUCHERNUMBER>" + "\r\n";
                xmlstc = xmlstc + "<REFERENCE>Ref</REFERENCE>" + "\r\n";
                xmlstc = xmlstc + "<PARTYLEDGERNAME>" + custName + "</PARTYLEDGERNAME>" + "\r\n";
                xmlstc = xmlstc + "<EFFECTIVEDATE>" + InvDate + "</EFFECTIVEDATE>" + "\r\n";
                xmlstc = xmlstc + "<ISINVOICE>Yes</ISINVOICE>" + "\r\n";
                xmlstc = xmlstc + "<INVOICEORDERLIST.LIST>" + "\r\n";
                xmlstc = xmlstc + "<BASICORDERDATE>" + InvDate + "</BASICORDERDATE>" + "\r\n";
                xmlstc = xmlstc + "<BASICPURCHASEORDERNO>" + SalesOrderNo + "</BASICPURCHASEORDERNO>" + "\r\n";
                xmlstc = xmlstc + "</INVOICEORDERLIST.LIST>" + "\r\n";
                xmlstc = xmlstc + "<ALLLEDGERENTRIES.LIST>" + "\r\n";
                xmlstc = xmlstc + "<LEDGERNAME>" + custName + "</LEDGERNAME>" + "\r\n";
                xmlstc = xmlstc + "<GSTCLASS/>" + "\r\n";
                xmlstc = xmlstc + "<ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>" + "\r\n";
                xmlstc = xmlstc + "<LEDGERFROMITEM>No</LEDGERFROMITEM>" + "\r\n";
                xmlstc = xmlstc + "<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>" + "\r\n";
                xmlstc = xmlstc + "<ISPARTYLEDGER>Yes</ISPARTYLEDGER>" + "\r\n";
                xmlstc = xmlstc + "<AMOUNT>-" + NetAmount + "</AMOUNT>" + "\r\n";
                xmlstc = xmlstc + "<BILLALLOCATIONS.LIST>" + "\r\n";
                xmlstc = xmlstc + "<NAME>" + InvNo + "</NAME>" + "\r\n";
                xmlstc = xmlstc + "<BILLCREDITPERIOD>30 Days</BILLCREDITPERIOD>" + "\r\n";
                xmlstc = xmlstc + "<BILLTYPE>New Ref</BILLTYPE>" + "\r\n";
                xmlstc = xmlstc + "<AMOUNT>-" + NetAmount + "</AMOUNT>" + "\r\n";
                xmlstc = xmlstc + "</BILLALLOCATIONS.LIST>" + "\r\n";
                xmlstc = xmlstc + "</ALLLEDGERENTRIES.LIST>" + "\r\n";
                xmlstc = xmlstc + "<ALLLEDGERENTRIES.LIST>" + "\r\n";
                xmlstc = xmlstc + "<LEDGERNAME>Sales</LEDGERNAME>" + "\r\n";
                xmlstc = xmlstc + "<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>" + "\r\n";
                xmlstc = xmlstc + "<LEDGERFROMITEM>No</LEDGERFROMITEM>" + "\r\n";
                xmlstc = xmlstc + "<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>" + "\r\n";
                xmlstc = xmlstc + "<ISPARTYLEDGER>No</ISPARTYLEDGER>" + "\r\n";
                xmlstc = xmlstc + "<AMOUNT>" + InvAmt + "</AMOUNT>" + "\r\n";
                xmlstc = xmlstc + "</ALLLEDGERENTRIES.LIST>" + "\r\n";
                if (Discount != "0.00")
                {
                    xmlstc = xmlstc + "<ALLLEDGERENTRIES.LIST>" + "\r\n";
                    xmlstc = xmlstc + "<LEDGERNAME>" + Discountldgr + "</LEDGERNAME>" + "\r\n";
                    xmlstc = xmlstc + "<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>" + "\r\n";
                    xmlstc = xmlstc + "<LEDGERFROMITEM>No</LEDGERFROMITEM>" + "\r\n";
                    xmlstc = xmlstc + "<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>" + "\r\n";
                    xmlstc = xmlstc + "<ISPARTYLEDGER>No</ISPARTYLEDGER>" + "\r\n";
                    xmlstc = xmlstc + "<AMOUNT>-" + Discount + "</AMOUNT>" + "\r\n";
                    xmlstc = xmlstc + "</ALLLEDGERENTRIES.LIST>" + "\r\n";
                }
                if (Surcharge1 != "0.00")
                {
                    xmlstc = xmlstc + "<ALLLEDGERENTRIES.LIST>" + "\r\n";
                    xmlstc = xmlstc + "<LEDGERNAME>" + Srchargldgr1 + "</LEDGERNAME>" + "\r\n";
                    xmlstc = xmlstc + "<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>" + "\r\n";
                    xmlstc = xmlstc + "<LEDGERFROMITEM>No</LEDGERFROMITEM>" + "\r\n";
                    xmlstc = xmlstc + "<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>" + "\r\n";
                    xmlstc = xmlstc + "<ISPARTYLEDGER>No</ISPARTYLEDGER>" + "\r\n";
                    xmlstc = xmlstc + "<AMOUNT>" + Surcharge1 + "</AMOUNT>" + "\r\n";
                    xmlstc = xmlstc + "</ALLLEDGERENTRIES.LIST>" + "\r\n";
                }
                if (Surcharge2 != "0.00")
                {
                    xmlstc = xmlstc + "<ALLLEDGERENTRIES.LIST>" + "\r\n";
                    xmlstc = xmlstc + "<LEDGERNAME>" + Srchargldgr2 + "</LEDGERNAME>" + "\r\n";
                    xmlstc = xmlstc + "<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>" + "\r\n";
                    xmlstc = xmlstc + "<LEDGERFROMITEM>No</LEDGERFROMITEM>" + "\r\n";
                    xmlstc = xmlstc + "<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>" + "\r\n";
                    xmlstc = xmlstc + "<ISPARTYLEDGER>No</ISPARTYLEDGER>" + "\r\n";
                    xmlstc = xmlstc + "<AMOUNT>" + Surcharge2 + "</AMOUNT>" + "\r\n";
                    xmlstc = xmlstc + "</ALLLEDGERENTRIES.LIST>" + "\r\n";
                }
                if (Surcharge3 != "0.00")
                {
                    xmlstc = xmlstc + "<ALLLEDGERENTRIES.LIST>" + "\r\n";
                    xmlstc = xmlstc + "<LEDGERNAME>" + Srchargldgr3 + "</LEDGERNAME>" + "\r\n";
                    xmlstc = xmlstc + "<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>" + "\r\n";
                    xmlstc = xmlstc + "<LEDGERFROMITEM>No</LEDGERFROMITEM>" + "\r\n";
                    xmlstc = xmlstc + "<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>" + "\r\n";
                    xmlstc = xmlstc + "<ISPARTYLEDGER>No</ISPARTYLEDGER>" + "\r\n";
                    xmlstc = xmlstc + "<AMOUNT>" + Surcharge3 + "</AMOUNT>" + "\r\n";
                    xmlstc = xmlstc + "</ALLLEDGERENTRIES.LIST>" + "\r\n";
                }
                xmlstc = xmlstc + "</VOUCHER>" + "\r\n";
                xmlstc = xmlstc + "</TALLYMESSAGE>" + "\r\n";
                xmlstc = xmlstc + "</REQUESTDATA>" + "\r\n";
                xmlstc = xmlstc + "</IMPORTDATA>" + "\r\n";
                xmlstc = xmlstc + "</BODY>" + "\r\n";
                xmlstc = xmlstc + "</ENVELOPE>" + "\r\n";



            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, ex.StackTrace);
            }
            return xmlstc;
        }

        private string CreditNote(string custName, string InvDate, string InvNo, string InvAmt, string SalesOrderNo)
        {
            string xmlstc = "<ENVELOPE>" + "\r\n";
            try
            {

                xmlstc = xmlstc + "<HEADER>" + "\r\n";
                xmlstc = xmlstc + "<TALLYREQUEST>Import Data</TALLYREQUEST>" + "\r\n";
                xmlstc = xmlstc + "</HEADER>" + "\r\n";
                xmlstc = xmlstc + "<BODY>" + "\r\n";
                xmlstc = xmlstc + "<IMPORTDATA>" + "\r\n";
                xmlstc = xmlstc + "<REQUESTDESC>" + "\r\n";
                xmlstc = xmlstc + "<REPORTNAME>Vouchers</REPORTNAME>" + "\r\n";
                xmlstc = xmlstc + "<STATICVARIABLES>" + "\r\n";
                xmlstc = xmlstc + "<SVCURRENTCOMPANY>##SVCURRENTCOMPANY</SVCURRENTCOMPANY>" + "\r\n";
                xmlstc = xmlstc + "</STATICVARIABLES>" + "\r\n";
                xmlstc = xmlstc + "</REQUESTDESC>" + "\r\n";
                xmlstc = xmlstc + "<REQUESTDATA>" + "\r\n";
                xmlstc = xmlstc + "<TALLYMESSAGE xmlns:UDF=" + "\"" + "TallyUDF" + "\" >" + "\r\n";
                xmlstc = xmlstc + "<VOUCHER VCHTYPE=" + "\"" + "Sales" + "\" ACTION=" + "\"" + "Create" + "\" >" + "\r\n";
                xmlstc = xmlstc + "<DATE>" + InvDate + "</DATE>" + "\r\n";
                xmlstc = xmlstc + "<VOUCHERTYPENAME>Credit Note</VOUCHERTYPENAME>" + "\r\n";
                xmlstc = xmlstc + "<VOUCHERNUMBER>" + InvNo + "</VOUCHERNUMBER>" + "\r\n";
                xmlstc = xmlstc + "<REFERENCE>Ref</REFERENCE>" + "\r\n";
                xmlstc = xmlstc + "<PARTYLEDGERNAME>" + custName + "</PARTYLEDGERNAME>" + "\r\n";
                xmlstc = xmlstc + "<EFFECTIVEDATE>" + InvDate + "</EFFECTIVEDATE>" + "\r\n";
                xmlstc = xmlstc + "<ISINVOICE>Yes</ISINVOICE>" + "\r\n";
                xmlstc = xmlstc + "<INVOICEORDERLIST.LIST>" + "\r\n";
                xmlstc = xmlstc + "<BASICORDERDATE>" + InvDate + "</BASICORDERDATE>" + "\r\n";
                xmlstc = xmlstc + "<BASICPURCHASEORDERNO>" + SalesOrderNo + "</BASICPURCHASEORDERNO>" + "\r\n";
                xmlstc = xmlstc + "</INVOICEORDERLIST.LIST>" + "\r\n";
                xmlstc = xmlstc + "<ALLLEDGERENTRIES.LIST>" + "\r\n";
                xmlstc = xmlstc + "<LEDGERNAME>" + custName + "</LEDGERNAME>" + "\r\n";
                xmlstc = xmlstc + "<GSTCLASS/>" + "\r\n";
                xmlstc = xmlstc + "<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>" + "\r\n";
                xmlstc = xmlstc + "<LEDGERFROMITEM>No</LEDGERFROMITEM>" + "\r\n";
                xmlstc = xmlstc + "<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>" + "\r\n";
                xmlstc = xmlstc + "<ISPARTYLEDGER>Yes</ISPARTYLEDGER>" + "\r\n";
                xmlstc = xmlstc + "<AMOUNT>" + InvAmt + "</AMOUNT>" + "\r\n";
                xmlstc = xmlstc + "<BILLALLOCATIONS.LIST>" + "\r\n";
                xmlstc = xmlstc + "<NAME>" + InvNo + "</NAME>" + "\r\n";
                xmlstc = xmlstc + "<BILLCREDITPERIOD>30 Days</BILLCREDITPERIOD>" + "\r\n";
                xmlstc = xmlstc + "<BILLTYPE>New Ref</BILLTYPE>" + "\r\n";
                xmlstc = xmlstc + "<AMOUNT>" + InvAmt + "</AMOUNT>" + "\r\n";
                xmlstc = xmlstc + "</BILLALLOCATIONS.LIST>" + "\r\n";
                xmlstc = xmlstc + "</ALLLEDGERENTRIES.LIST>" + "\r\n";
                xmlstc = xmlstc + "<ALLLEDGERENTRIES.LIST>" + "\r\n";
                xmlstc = xmlstc + "<LEDGERNAME>Sales</LEDGERNAME>" + "\r\n";
                xmlstc = xmlstc + "<ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>" + "\r\n";
                xmlstc = xmlstc + "<LEDGERFROMITEM>No</LEDGERFROMITEM>" + "\r\n";
                xmlstc = xmlstc + "<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>" + "\r\n";
                xmlstc = xmlstc + "<ISPARTYLEDGER>No</ISPARTYLEDGER>" + "\r\n";
                xmlstc = xmlstc + "<AMOUNT>-" + InvAmt + "</AMOUNT>" + "\r\n";
                xmlstc = xmlstc + "</ALLLEDGERENTRIES.LIST>" + "\r\n";
                xmlstc = xmlstc + "</VOUCHER>" + "\r\n";
                xmlstc = xmlstc + "</TALLYMESSAGE>" + "\r\n";
                xmlstc = xmlstc + "</REQUESTDATA>" + "\r\n";
                xmlstc = xmlstc + "</IMPORTDATA>" + "\r\n";
                xmlstc = xmlstc + "</BODY>" + "\r\n";
                xmlstc = xmlstc + "</ENVELOPE>" + "\r\n";



            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, ex.StackTrace);
            }
            return xmlstc;
        }

        private string JV(string Crparty, string Drparty, string InvDate, string InvNo, string InvAmt, string refno, string refdate, string Narration)
        {
            string xmlstc = "<ENVELOPE>" + "\r\n";
            try
            {
                xmlstc = xmlstc + "<HEADER>" + "\r\n";
                xmlstc = xmlstc + "<TALLYREQUEST>Import Data</TALLYREQUEST>" + "\r\n";
                xmlstc = xmlstc + "</HEADER>" + "\r\n";
                xmlstc = xmlstc + "<BODY>" + "\r\n";
                xmlstc = xmlstc + "<IMPORTDATA>" + "\r\n";
                xmlstc = xmlstc + "<REQUESTDESC>" + "\r\n";
                xmlstc = xmlstc + "<REPORTNAME>Vouchers</REPORTNAME>" + "\r\n";
                xmlstc = xmlstc + "<STATICVARIABLES>" + "\r\n";
                xmlstc = xmlstc + "<SVCURRENTCOMPANY>##SVCURRENTCOMPANY</SVCURRENTCOMPANY>" + "\r\n";
                xmlstc = xmlstc + "</STATICVARIABLES>" + "\r\n";
                xmlstc = xmlstc + "</REQUESTDESC>" + "\r\n";
                xmlstc = xmlstc + "<REQUESTDATA>" + "\r\n";
                xmlstc = xmlstc + "<TALLYMESSAGE xmlns:UDF=" + "\"" + "TallyUDF" + "\" >" + "\r\n";
                xmlstc = xmlstc + "<VOUCHER VCHTYPE=" + "\"" + "Journal" + "\" >" + "\r\n";
                xmlstc = xmlstc + "<DATE>" + InvDate + "</DATE>" + "\r\n";
                xmlstc = xmlstc + "<VOUCHERTYPENAME>Journal</VOUCHERTYPENAME>" + "\r\n";
                xmlstc = xmlstc + "<NARRATION> " + Narration + "</NARRATION> " + "\r\n";
                xmlstc = xmlstc + "<VOUCHERNUMBER>" + InvNo + "</VOUCHERNUMBER>" + "\r\n";
                xmlstc = xmlstc + "<REFERENCE>" + InvNo + "</REFERENCE>" + "\r\n";
                xmlstc = xmlstc + "<PARTYLEDGERNAME>" + Crparty + "</PARTYLEDGERNAME>" + "\r\n";
                xmlstc = xmlstc + "<EFFECTIVEDATE>" + InvDate + "</EFFECTIVEDATE>" + "\r\n";
                xmlstc = xmlstc + "<ISINVOICE>Yes</ISINVOICE>" + "\r\n";

                xmlstc = xmlstc + "<INVOICEORDERLIST.LIST>" + "\r\n";
                xmlstc = xmlstc + "<BASICORDERDATE>" + InvDate + "</BASICORDERDATE>" + "\r\n";
                xmlstc = xmlstc + "<BASICPURCHASEORDERNO>" + refno + "</BASICPURCHASEORDERNO>" + "\r\n";
                xmlstc = xmlstc + "</INVOICEORDERLIST.LIST>" + "\r\n";



                xmlstc = xmlstc + "<ALLLEDGERENTRIES.LIST>" + "\r\n";
                xmlstc = xmlstc + "<LEDGERNAME>" + Crparty + "</LEDGERNAME>" + "\r\n";
                xmlstc = xmlstc + "<GSTCLASS/>" + "\r\n";
                xmlstc = xmlstc + "<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>" + "\r\n";
                xmlstc = xmlstc + "<LEDGERFROMITEM>No</LEDGERFROMITEM>" + "\r\n";
                xmlstc = xmlstc + "<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>" + "\r\n";
                xmlstc = xmlstc + "<ISPARTYLEDGER>Yes</ISPARTYLEDGER>" + "\r\n";
                xmlstc = xmlstc + "<AMOUNT>" + InvAmt + "</AMOUNT>" + "\r\n";

                xmlstc = xmlstc + "<BILLALLOCATIONS.LIST>" + "\r\n";
                xmlstc = xmlstc + "<NAME>" + InvNo + "</NAME>" + "\r\n";
                xmlstc = xmlstc + "<BILLCREDITPERIOD>30 Days</BILLCREDITPERIOD>" + "\r\n";
                xmlstc = xmlstc + "<BILLTYPE>New Ref</BILLTYPE>" + "\r\n";
                xmlstc = xmlstc + "<AMOUNT>" + InvAmt + "</AMOUNT>" + "\r\n";
                xmlstc = xmlstc + "</BILLALLOCATIONS.LIST>" + "\r\n";

                xmlstc = xmlstc + "</ALLLEDGERENTRIES.LIST>" + "\r\n";


                xmlstc = xmlstc + "<ALLLEDGERENTRIES.LIST>" + "\r\n";
                xmlstc = xmlstc + "<LEDGERNAME>" + Drparty + "</LEDGERNAME>" + "\r\n";
                xmlstc = xmlstc + "<ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>" + "\r\n";
                xmlstc = xmlstc + "<LEDGERFROMITEM>No</LEDGERFROMITEM>" + "\r\n";
                xmlstc = xmlstc + "<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>" + "\r\n";
                xmlstc = xmlstc + "<ISPARTYLEDGER>No</ISPARTYLEDGER>" + "\r\n";
                xmlstc = xmlstc + "<AMOUNT>-" + InvAmt + "</AMOUNT>" + "\r\n";
                xmlstc = xmlstc + "</ALLLEDGERENTRIES.LIST>" + "\r\n";



                xmlstc = xmlstc + "</VOUCHER>" + "\r\n";
                xmlstc = xmlstc + "</TALLYMESSAGE>" + "\r\n";
                xmlstc = xmlstc + "</REQUESTDATA>" + "\r\n";
                xmlstc = xmlstc + "</IMPORTDATA>" + "\r\n";
                xmlstc = xmlstc + "</BODY>" + "\r\n";
                xmlstc = xmlstc + "</ENVELOPE>" + "\r\n";

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, ex.StackTrace);
            }
            return xmlstc;
        }

        private string JV_Multi(string Crparty, string Drparty, string InvDate, string InvNo, string InvAmt, string refno, string refdate, string Narration)
        {
            string xmlstc = "<ENVELOPE>" + "\r\n";
            try
            {

                xmlstc = xmlstc + "<HEADER>" + "\r\n";
                xmlstc = xmlstc + "<TALLYREQUEST>Import Data</TALLYREQUEST>" + "\r\n";
                xmlstc = xmlstc + "</HEADER>" + "\r\n";
                xmlstc = xmlstc + "<BODY>" + "\r\n";
                xmlstc = xmlstc + "<IMPORTDATA>" + "\r\n";
                xmlstc = xmlstc + "<REQUESTDESC>" + "\r\n";
                xmlstc = xmlstc + "<REPORTNAME>Vouchers</REPORTNAME>" + "\r\n";
                xmlstc = xmlstc + "<STATICVARIABLES>" + "\r\n";
                xmlstc = xmlstc + "<SVCURRENTCOMPANY>##SVCURRENTCOMPANY</SVCURRENTCOMPANY>" + "\r\n";
                xmlstc = xmlstc + "</STATICVARIABLES>" + "\r\n";
                xmlstc = xmlstc + "</REQUESTDESC>" + "\r\n";
                xmlstc = xmlstc + "<REQUESTDATA>" + "\r\n";
                xmlstc = xmlstc + "<TALLYMESSAGE xmlns:UDF=" + "\"" + "TallyUDF" + "\" >" + "\r\n";
                xmlstc = xmlstc + "<VOUCHER VCHTYPE=" + "\"" + "Journal" + "\" >" + "\r\n";
                xmlstc = xmlstc + "<DATE>" + InvDate + "</DATE>" + "\r\n";
                xmlstc = xmlstc + "<VOUCHERTYPENAME>Journal</VOUCHERTYPENAME>" + "\r\n";
                xmlstc = xmlstc + "<NARRATION> " + Narration + "</NARRATION> " + "\r\n";
                xmlstc = xmlstc + "<VOUCHERNUMBER>" + InvNo + "</VOUCHERNUMBER>" + "\r\n";
                xmlstc = xmlstc + "<REFERENCE>" + InvNo + "</REFERENCE>" + "\r\n";
                xmlstc = xmlstc + "<PARTYLEDGERNAME>" + Crparty + "</PARTYLEDGERNAME>" + "\r\n";
                xmlstc = xmlstc + "<EFFECTIVEDATE>" + InvDate + "</EFFECTIVEDATE>" + "\r\n";
                xmlstc = xmlstc + "<ISINVOICE>Yes</ISINVOICE>" + "\r\n";

                xmlstc = xmlstc + "<INVOICEORDERLIST.LIST>" + "\r\n";
                xmlstc = xmlstc + "<BASICORDERDATE>" + InvDate + "</BASICORDERDATE>" + "\r\n";
                xmlstc = xmlstc + "<BASICPURCHASEORDERNO>" + refno + "</BASICPURCHASEORDERNO>" + "\r\n";
                xmlstc = xmlstc + "</INVOICEORDERLIST.LIST>" + "\r\n";

                xmlstc = xmlstc + "<ALLLEDGERENTRIES.LIST>" + "\r\n";
                xmlstc = xmlstc + "<LEDGERNAME>" + Crparty + "</LEDGERNAME>" + "\r\n";
                xmlstc = xmlstc + "<GSTCLASS/>" + "\r\n";
                xmlstc = xmlstc + "<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>" + "\r\n";
                xmlstc = xmlstc + "<LEDGERFROMITEM>No</LEDGERFROMITEM>" + "\r\n";
                xmlstc = xmlstc + "<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>" + "\r\n";
                xmlstc = xmlstc + "<ISPARTYLEDGER>Yes</ISPARTYLEDGER>" + "\r\n";
                xmlstc = xmlstc + "<AMOUNT>" + InvAmt + "</AMOUNT>" + "\r\n";

                xmlstc = xmlstc + "<BILLALLOCATIONS.LIST>" + "\r\n";
                xmlstc = xmlstc + "<NAME>" + InvNo + "</NAME>" + "\r\n";
                xmlstc = xmlstc + "<BILLCREDITPERIOD>30 Days</BILLCREDITPERIOD>" + "\r\n";
                xmlstc = xmlstc + "<BILLTYPE>New Ref</BILLTYPE>" + "\r\n";
                xmlstc = xmlstc + "<AMOUNT>" + InvAmt + "</AMOUNT>" + "\r\n";
                xmlstc = xmlstc + "</BILLALLOCATIONS.LIST>" + "\r\n";

                xmlstc = xmlstc + "</ALLLEDGERENTRIES.LIST>" + "\r\n";


                xmlstc = xmlstc + "<ALLLEDGERENTRIES.LIST>" + "\r\n";
                xmlstc = xmlstc + "<LEDGERNAME>" + Drparty + "</LEDGERNAME>" + "\r\n";
                xmlstc = xmlstc + "<ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>" + "\r\n";
                xmlstc = xmlstc + "<LEDGERFROMITEM>No</LEDGERFROMITEM>" + "\r\n";
                xmlstc = xmlstc + "<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>" + "\r\n";
                xmlstc = xmlstc + "<ISPARTYLEDGER>No</ISPARTYLEDGER>" + "\r\n";
                xmlstc = xmlstc + "<AMOUNT>-" + InvAmt + "</AMOUNT>" + "\r\n";
                xmlstc = xmlstc + "</ALLLEDGERENTRIES.LIST>" + "\r\n";



                xmlstc = xmlstc + "</VOUCHER>" + "\r\n";
                xmlstc = xmlstc + "</TALLYMESSAGE>" + "\r\n";
                xmlstc = xmlstc + "</REQUESTDATA>" + "\r\n";
                xmlstc = xmlstc + "</IMPORTDATA>" + "\r\n";
                xmlstc = xmlstc + "</BODY>" + "\r\n";
                xmlstc = xmlstc + "</ENVELOPE>" + "\r\n";

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, ex.StackTrace);
            }
            return xmlstc;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Company"></param>
        /// <param name="Vch_Type">Journal</param>
        /// <param name="dtV"></param>
        /// <returns></returns>
        private string JV_Multi_2(string Company, string Vch_Type, DataTable dtV)
        {
            string xmlstc = "<ENVELOPE>" + "\r\n";
            try
            {

                xmlstc = xmlstc + "<HEADER>" + "\r\n";
                xmlstc = xmlstc + "<TALLYREQUEST>Import Data</TALLYREQUEST>" + "\r\n";
                xmlstc = xmlstc + "</HEADER>" + "\r\n";
                xmlstc = xmlstc + "<BODY>" + "\r\n";
                xmlstc = xmlstc + "<IMPORTDATA>" + "\r\n";
                xmlstc = xmlstc + "<REQUESTDESC>" + "\r\n";
                xmlstc = xmlstc + "<REPORTNAME>Vouchers</REPORTNAME>" + "\r\n";
                xmlstc = xmlstc + "<STATICVARIABLES>" + "\r\n";
                xmlstc = xmlstc + "<SVCURRENTCOMPANY>" + Company + "</SVCURRENTCOMPANY>" + "\r\n";
                xmlstc = xmlstc + "</STATICVARIABLES>" + "\r\n";
                xmlstc = xmlstc + "</REQUESTDESC>" + "\r\n";
                xmlstc = xmlstc + "<REQUESTDATA>" + "\r\n";
                xmlstc = xmlstc + "<TALLYMESSAGE xmlns:UDF=" + "\"" + "TallyUDF" + "\" >" + "\r\n";
                xmlstc = xmlstc + "<VOUCHER VCHTYPE=" + "\"" + "" + Vch_Type + "" + "\" >" + "\r\n";
                xmlstc = xmlstc + "<DATE>" + ((DateTime)dtV.Rows[0]["vch_date"]).ToString("dd-MMM-YYYY") + "</DATE>" + "\r\n";
                xmlstc = xmlstc + "<VOUCHERTYPENAME>" + Vch_Type + "</VOUCHERTYPENAME>" + "\r\n";
                xmlstc = xmlstc + "<NARRATION> " + dtV.Rows[0]["totremark"].ToString() + "</NARRATION> " + "\r\n";
                xmlstc = xmlstc + "<VOUCHERNUMBER>" + dtV.Rows[0]["vch_num"].ToString() + "</VOUCHERNUMBER>" + "\r\n";
                xmlstc = xmlstc + "<REFERENCE>" + dtV.Rows[0]["invno"].ToString() + "</REFERENCE>" + "\r\n";


                foreach (DataRow dr in dtV.Rows)
                {

                    if (sgen.Make_decimal(dtV.Rows[0]["dramt"].ToString()) > 0)
                    {
                        xmlstc = xmlstc + "<ALLLEDGERENTRIES.LIST>" + "\r\n";
                        xmlstc = xmlstc + "<LEDGERNAME>" + dtV.Rows[0]["tally_name"].ToString() + "</LEDGERNAME>" + "\r\n";
                        xmlstc = xmlstc + "<ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>" + "\r\n";
                        xmlstc = xmlstc + "<LEDGERFROMITEM>No</LEDGERFROMITEM>" + "\r\n";
                        xmlstc = xmlstc + "<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>" + "\r\n";
                        xmlstc = xmlstc + "<ISPARTYLEDGER>No</ISPARTYLEDGER>" + "\r\n";
                        xmlstc = xmlstc + "<AMOUNT>-" + dtV.Rows[0]["dramt"].ToString() + "</AMOUNT>" + "\r\n";
                        xmlstc = xmlstc + "</ALLLEDGERENTRIES.LIST>" + "\r\n";
                    }
                    else
                    {

                        xmlstc = xmlstc + "<ALLLEDGERENTRIES.LIST>" + "\r\n";
                        xmlstc = xmlstc + "<LEDGERNAME>" + dtV.Rows[0]["tally_name"].ToString() + "</LEDGERNAME>" + "\r\n";
                        xmlstc = xmlstc + "<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>" + "\r\n";
                        xmlstc = xmlstc + "<LEDGERFROMITEM>No</LEDGERFROMITEM>" + "\r\n";
                        xmlstc = xmlstc + "<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>" + "\r\n";
                        xmlstc = xmlstc + "<ISPARTYLEDGER>No</ISPARTYLEDGER>" + "\r\n";
                        xmlstc = xmlstc + "<AMOUNT>-" + dtV.Rows[0]["cramt"].ToString() + "</AMOUNT>" + "\r\n";
                        xmlstc = xmlstc + "</ALLLEDGERENTRIES.LIST>" + "\r\n";
                    }
                }



                xmlstc = xmlstc + "</VOUCHER>" + "\r\n";
                xmlstc = xmlstc + "</TALLYMESSAGE>" + "\r\n";
                xmlstc = xmlstc + "</REQUESTDATA>" + "\r\n";
                xmlstc = xmlstc + "</IMPORTDATA>" + "\r\n";
                xmlstc = xmlstc + "</BODY>" + "\r\n";
                xmlstc = xmlstc + "</ENVELOPE>" + "\r\n";

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, ex.StackTrace);
            }
            return xmlstc;
        }
        #endregion

        #region rpt_vwr
        public ActionResult rpt_vwr()
        {
            List<Tmodelmain> model = new List<Tmodelmain>();
            return View(model);
        }
        [HttpPost]
        public ActionResult rpt_vwr(List<Tmodelmain> model)
        {

            return View(model);
        }
        #endregion

        #region event_cal
        public ActionResult event_cal()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            getload(model);
            string where = "";
            //mq = sgen.seekval(userCode, "select mod_id from user_details where rec_id='" + Convert.ToInt32(userid_mst) + "'", "mod_id");
            mq = sgen.seekval(userCode, "select mod_id from user_details where vch_num='" + userid_mst + "'", "mod_id");
            if (!role_mst.ToUpper().Equals("OWNER")) where = " and a.mod_id in ('" + mq.Replace(",", "','") + "')";

            DataTable dtcomp_mod = new DataTable();
            mq = "select  a.mod_Id,b.m_name from com_module a inner join  menus b on   a.mod_id=b.m_id and a.com_code='" + userCode + "' and m_name='CRM'"
                + where + "";
            dtcomp_mod = sgen.getdata(userCode, mq);
            if (dtcomp_mod.Rows.Count > 0)
            {
                model[0].Col17 = "Y";
                model[0].Col18 = "Y";
                //if (!dtcomp_mod.Rows[0]["m_name"].ToString().Trim().Equals("Training"))
                //{
                //    model[0].Col17 = "N";
                //}
                if (!dtcomp_mod.Rows[0]["m_name"].ToString().Trim().Equals("CRM"))
                {
                    model[0].Col17 = "N";
                }
            }
            ModelState.Clear();
            return View(model);
        }
        [HttpPost]
        public ActionResult event_cal(List<Tmodelmain> model)
        {
            return View(model);
        }

        #endregion

        #region event_cal_day
        public ActionResult event_cal_day()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            getload(model);
            string where = "";
            //mq = sgen.seekval(userCode, "select mod_id from user_details where rec_id='" + Convert.ToInt32(userid_mst) + "'", "mod_id");
            mq = sgen.seekval(userCode, "select mod_id from user_details where vch_num='" + userid_mst + "'", "mod_id");
            if (!role_mst.ToUpper().Equals("OWNER")) where = " and a.mod_id in ('" + mq.Replace(",", "','") + "')";

            DataTable dtcomp_mod = new DataTable();
            mq = "select  a.mod_Id,b.m_name from com_module a inner join  menus b on   a.mod_id=b.m_id and a.com_code='" + userCode + "' and m_name='CRM'"
                + where + "";
            dtcomp_mod = sgen.getdata(userCode, mq);
            if (dtcomp_mod.Rows.Count > 0)
            {
                model[0].Col17 = "Y";
                model[0].Col18 = "Y";
                //if (!dtcomp_mod.Rows[0]["m_name"].ToString().Trim().Equals("Training"))
                //{
                //    model[0].Col17 = "N";
                //}
                if (!dtcomp_mod.Rows[0]["m_name"].ToString().Trim().Equals("CRM"))
                {
                    model[0].Col17 = "N";
                }
            }

            ModelState.Clear();
            return View(model);
        }
        [HttpPost]
        public ActionResult event_cal_day(List<Tmodelmain> model)
        {
            return View(model);
        }

        #endregion

        #region btnauth


        public static void makemenu(DataTable dtparent, string module3, string module1, long level)
        {
            string MyGuid = "";
            string m_id = sgen.GetCookie(MyGuid, "m_id").ToString();
            try
            {
                DataTable dtstatuswise = dtparent.AsEnumerable().Where(w => (string)w["m_module3"] == module3 &&
                                (string)w["m_module2"] == module1 && Convert.ToInt64(w["m_level"]) == level)
                                                    .Select(s => s).CopyToDataTable();

                foreach (DataRow dr in dtstatuswise.Rows)
                {
                    if (dr["m_id"].ToString().Equals("7005.6"))
                    {

                    }
                    if (dr["m_submenu"].ToString().Equals("0"))
                    {
                        cli++;
                        if (dr["m_link"].ToString().Trim().Length > 4) html = html + "<li id='l" + cli + "' onclick='givemyid(this)'> <a runat='server' id='m" + dr["m_id"].ToString() + "' " + dr["attributes"].ToString().Replace('$', '\'') + " ><i class='" + dr["m_icon"].ToString() + "'></i>" + dr["m_name"].ToString() + "</a></li>";
                        else html = html + "<li id='l" + cli + "' onclick='givemyid(this)'> <a runat='server' id='m" + dr["m_id"].ToString() + "' " + dr["attributes"].ToString().Replace('$', '\'') + "><i class='" + dr["m_icon"].ToString() + "'></i>" + dr["m_name"].ToString() + "</a></li>";

                    }
                    else
                    {
                        cli++;

                        html = html + "<li id='l" + cli + "' onclick='liclick(this,event);' > <a id='a" + cli + "'><i class='" + dr["m_icon"].ToString() + "'></i>" + dr["m_name"].ToString() + "<span class='fa fa-chevron-down'></span></a>";
                        html = html + "<ul id='u" + cli + "' class='nav child_menu'>";
                        makemenu(dtparent, dr["m_module3"].ToString(), dr["m_module1"].ToString(), Convert.ToInt64(dr["m_level"].ToString()) + 1);
                        html = html + " </ul>";
                    }
                }
            }
            catch { }
        }


        public class TreeViewNode
        {
            public string id { get; set; }
            public string parent { get; set; }
            public string text { get; set; }
        }

        public ActionResult btnauth()
        {
            FillMst();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "BUTTON AUTHORITY";
            model[0].Col10 = "uright";
            model[0].Col11 = " and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            model[0].Col12 = "K99";
            model[0].TList1 = mod1;
            model[0].TList2 = mod1;
            model[0].SelectedItems2 = new string[] { "" };
            model[0].SelectedItems3 = new string[] { "" };
            ViewBag.vgetdt = "disabled='disabled'";
            return View(model);
        }

        [HttpPost]
        public ActionResult btnauth(List<Tmodelmain> model, string command, string hfrow, string hftable)
        {
            FillMst();

            #region
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<SelectListItem> mod2 = new List<SelectListItem>();
            model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
            model[0].TList2 = (List<SelectListItem>)TempData[MyGuid + "_TList2"];
            TempData[MyGuid + "_TList1"] = model[0].TList1;
            TempData[MyGuid + "_TList2"] = model[0].TList2;
            if (model[0].SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
            if (model[0].SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
            #endregion

            if (command == "New")
            {
                try
                {
                    model = getnew(model);
                    mod1 = new List<SelectListItem>();
                    mod2 = new List<SelectListItem>();

                    DataTable dtmod = new DataTable();
                    //module                   
                    mq1 = "";
                    //mq = sgen.seekval(userCode, "select mod_id from user_details where rec_id='" + Convert.ToInt32(userid_mst) + "'", "mod_id");
                    mq = sgen.seekval(userCode, "select mod_id from user_details where vch_num='" + userid_mst + "'", "mod_id");
                    if (!role_mst.ToUpper().Equals("OWNER")) mq1 = " and mod_id in ('" + mq.Replace(",", "','") + "')";
                    mq = "SELECT m_module3,mod_Id, m_name|| '( '||mod_id ||')' as nameid FROM com_module join menus on com_module.mod_id=menus.m_id and upper(com_code) = upper('" + userCode + "')" + mq1 + "";

                    dtmod = sgen.getdata(userCode, mq);
                    if (dtmod.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtmod.Rows)
                        {
                            mod1.Add(new SelectListItem { Text = dr["nameid"].ToString(), Value = dr["m_module3"].ToString() });
                        }
                    }

                    TempData[MyGuid + "_TList1"] = mod1;
                    model[0].TList1 = mod1;
                    model[0].SelectedItems1 = new string[] { "" };

                    //role                   
                    TempData[MyGuid + "_TList2"] = mod2;
                    model[0].TList2 = mod2;
                    model[0].SelectedItems2 = new string[] { "" };
                    ViewBag.vgetdt = "";
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
            else if (command == "ddl_role")
            {
                var modid = model[0].TList1.SingleOrDefault(w => w.Value == model[0].SelectedItems1.FirstOrDefault()).Text.Split('(')[1].Trim().Split(')')[0].Trim();
                mq = "SELECT u_id,m_id,(r_name||'( '||u_id||')') r_name,m_name FROM role_authority where m_id='" + modid + "'";
                DataTable dtr = sgen.getdata(userCode, mq);
                if (dtr.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtr.Rows)
                    {
                        mod2.Add(new SelectListItem { Text = dr["r_name"].ToString(), Value = dr["u_id"].ToString() });
                    }
                }

                TempData[MyGuid + "_TList2"] = mod2;
                model[0].TList2 = mod2;
                model[0].SelectedItems2 = new string[] { "" };
                ViewBag.vnew = "disabled='disabled'";
            }
            else if (command == "Get Data")
            {
                try
                {
                    string modid = string.Join(",", model[0].SelectedItems1);
                    List<TreeViewNode> nodes = new List<TreeViewNode>();
                    mq = "select m_id,m_name,m_module3 as m_module1,m_module2,m_module3 from menus where m_level='1' and m_module3 in ('" + modid.Replace(",", "','") + "') order by to_number(m_id) asc";
                    DataTable dt = sgen.getdata(userCode, mq);
                    if (dt.Rows.Count > 0)
                    {
                        //Loop and add the Parent Nodes.
                        foreach (DataRow dr in dt.Rows)
                        {
                            nodes.Add(new TreeViewNode { id = dr["m_id"].ToString(), parent = "#", text = dr["m_name"].ToString() });
                        }

                        mq1 = "select m_id,m_name,m_module1,m_module2,m_module3 from menus where to_number(m_level)=2 and m_module2 in ('" + modid.Replace(",", "','") + "')";
                        DataTable dtm = sgen.getdata(userCode, mq1);
                        if (dtm.Rows.Count > 0)
                        {
                            //Loop and add the Child Nodes.
                            foreach (DataRow drm in dtm.Rows)
                            {
                                nodes.Add(new TreeViewNode { id = drm["m_module3"].ToString() + "-" + drm["m_id"].ToString(), parent = drm["m_module2"].ToString(), text = drm["m_name"].ToString() });
                            }
                        }

                        string mq2 = "select m_id,m_name,m_module1,m_module2,m_module3 from menus where to_number(m_level)=3 and m_module3 in ('" + modid.Replace(",", "','") + "')";
                        DataTable dt3 = sgen.getdata(userCode, mq2);
                        if (dt3.Rows.Count > 0)
                        {
                            //Loop and add the Child Nodes.
                            foreach (DataRow dr3 in dt3.Rows)
                            {
                                nodes.Add(new TreeViewNode { id = dr3["m_module2"].ToString() + "-" + dr3["m_id"].ToString(), parent = dr3["m_module2"].ToString(), text = dr3["m_name"].ToString() });
                            }
                        }

                        //Serialize to JSON string.
                        ViewBag.Json = (new JavaScriptSerializer()).Serialize(nodes);
                    }


                    mq = "select * from menus where  (m_module3 = '" + modid + "' or m_module3 = 'common') and m_module3 not in ('stpmain','stdmain') order by m_order";
                    DataTable dtparent = new DataTable();
                    dtparent = sgen.getdata(userCode, mq);

                    DataTable dt0 = dtparent.AsEnumerable().Where(w => Convert.ToInt64(w["m_level"]) == 2).Select(s => s).CopyToDataTable();
                    foreach (DataRow dr in dt0.Rows)
                    {
                        if (dr["m_submenu"].ToString().Equals("0"))
                        {
                            cli++;
                            if (dr["m_link"].ToString().Trim().Length > 4) html = html + "<li id='l" + cli + "' onclick='givemyid(this)'> <a runat='server' id='m" + dr["m_id"].ToString() + "' " + dr["attributes"].ToString().Replace('$', '\'') + "'><i class='" + dr["m_icon"].ToString() + "'></i>" + dr["m_name"].ToString() + "</a></li>";
                            else html = html + "<li id='l" + cli + "' onclick='givemyid(this)'> <a runat='server' id='m" + dr["m_id"].ToString() + "' " + dr["attributes"].ToString().Replace('$', '\'') + "><i class='" + dr["m_icon"].ToString() + "'></i>" + dr["m_name"].ToString() + "</a></li>";
                        }
                        else
                        {
                            cli++;
                            html = html + "<li id='l" + cli + "' onclick='liclick(this,event);'> <a id='a" + cli + "'><i class='" + dr["m_icon"].ToString() + "'></i>" + dr["m_name"].ToString() + "<span class='fa fa-chevron-down'></span></a>";
                            html = html + "<ul id='u" + cli + "' class='nav child_menu'>";
                            makemenu(dtparent, dr["m_module3"].ToString(), dr["m_module1"].ToString(), Convert.ToInt64(dr["m_level"].ToString()) + 1);
                            html = html + " </ul>";
                        }
                    }




                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
            }
            else if (command == "Save" || command == "Update")
            {
                try
                {
                    Satransaction sat = new Satransaction(userCode, MyGuid);
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime(userCode);
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

                    DataTable dataTable = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    for (int i = 0; i < model[0].dt1.Rows.Count; i++)
                    {
                        DataRow dr = dataTable.NewRow();
                        dr["type"] = model[0].Col12;
                        dr["vch_num"] = vch_num.Trim();
                        dr["vch_date"] = sgen.Make_date_S(model[0].Col17);
                        dr["acode"] = model[0].Col19;//main icode
                        dr["mstage"] = model[0].SelectedItems2.FirstOrDefault();//main stage                        
                        dr["stage1"] = model[0].Col98;//to stage next stage
                        dr["opcode"] = model[0].SelectedItems3.FirstOrDefault();//op
                        dr["mccode"] = model[0].SelectedItems4.FirstOrDefault();//machine
                        dr["mouldcode"] = model[0].SelectedItems5.FirstOrDefault();//mould
                        dr["shftno"] = model[0].SelectedItems6.FirstOrDefault();//shft
                        dr["stime"] = model[0].Col25;//tool starttime
                        dr["etime"] = model[0].Col26;//tool endtime
                        dr["qtyin"] = sgen.Make_decimal(model[0].Col23);//produce qty
                        dr["qtyrej"] = sgen.Make_decimal(model[0].Col30);//rejection qty
                        dr["qtyrw"] = sgen.Make_decimal(model[0].Col31);//rework qty
                        dr["totremark"] = model[0].Col24;//remark
                        dr["cstk"] = sgen.Make_decimal(model[0].Col29);//currentstk
                        dr["rno"] = model[0].Col99;//check stk or not
                        dr["pflag"] = model[0].Col101;//check final pd or not                       
                        dr["pdono"] = model[0].Col32;//pdo no
                        dr["pdodt"] = sgen.Make_date_S(model[0].Col33);//pdo dt 
                        dr["qtypdo"] = sgen.Make_decimal(model[0].Col34);//pdo qty

                        dr["col1"] = model[0].Col35;
                        dr["col2"] = model[0].Col36;
                        dr["col3"] = model[0].Col37;
                        dr["date1"] = sgen.Make_date_S(model[0].Col38);
                        dr["date2"] = sgen.Make_date_S(model[0].Col39);
                        dr["date3"] = sgen.Make_date_S(model[0].Col40);
                        dr["col4"] = model[0].Col41;
                        dr["col5"] = model[0].Col42;
                        dr["col6"] = model[0].Col43;

                        //dt====
                        dr["icode"] = model[0].dt1.Rows[i][1].ToString();
                        dr["iname"] = model[0].dt1.Rows[i][2].ToString();
                        dr["qtystk"] = sgen.Make_decimal(model[0].dt1.Rows[i][4].ToString().Trim());//qtystk
                        dr["stage"] = model[0].dt1.Rows[i][5].ToString();//item stage                      
                        dr["qtychl"] = sgen.Make_decimal(model[0].dt1.Rows[i][7].ToString().Trim());//est con qty
                        dr["qtyout"] = sgen.Make_decimal(model[0].dt1.Rows[i][8].ToString().Trim());//act con qty               
                        dr["qtyplanned"] = sgen.Make_decimal(model[0].dt1.Rows[i][9].ToString().Trim());//short_excess
                        dr["alt"] = model[0].dt1.Rows[i][10].ToString().Trim();//alternate
                        dr = getsave(currdate, dr, model);
                        dataTable.Rows.Add(dr);
                    }
                    res = sgen.Update_data_uncommit2(userCode, dataTable, model[0].Col10.Trim(), tmodel.Col8, isedit, sat);
                    if (res == true)
                    {
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col9 = tmodel.Col9,
                            Col10 = tmodel.Col10,
                            Col11 = tmodel.Col11,
                            Col12 = tmodel.Col12,
                            Col13 = "Save",
                            Col14 = tmodel.Col14,
                            Col15 = tmodel.Col15,
                            TList1 = mod1,
                            TList2 = mod2,
                            SelectedItems1 = new string[] { "" },
                            SelectedItems2 = new string[] { "" }
                        });
                        sat.Commit();
                        ViewBag.vnew = "";
                        ViewBag.vsave = "disabled='disabled'";
                        ViewBag.vgetdt = "disabled='disabled'";
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                    }
                    else
                    {
                        sat.Rollback();
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vgetdt = "";
                        ViewBag.scripCall += "showmsgJS(1, 'Something went wrong', 0);";
                        ModelState.Clear();
                        return View(model);
                    }
                }
                catch (Exception ex) { ViewBag.scripCall += "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
            }
            ModelState.Clear();
            return View(model);
        }

        #endregion

        #region sys_update

        public ActionResult sys_update()
        {
            FillMst();


            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            m_id_mst = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);

            List<Tmodelmain> model = new List<Tmodelmain>();
            Tmodelmain tm = new Tmodelmain();

            tm.Col14 = mid_mst;
            tm.Col15 = m_id_mst;

            model.Add(tm);
            return View(model);
        }

        [HttpPost]
        public ActionResult sys_update(List<Tmodelmain> model, string command)
        {
            FillMst();
            ViewBag.msg += "1-";
            if (command == "down")
            {
                ViewBag.msg += "2-";
                DriveService service = GetService();
                ViewBag.msg += "3-";
                string FolderPath = System.Web.HttpContext.Current.Server.MapPath("/GoogleDriveFiles/");
                FilesResource.ListRequest FileListRequest = service.Files.List();
                FileListRequest.Fields = "nextPageToken, files(id, name, size, version, trashed, createdTime)";
                IList<Google.Apis.Drive.v3.Data.File> files = FileListRequest.Execute().Files.Where(w => w.Name.ToLower().Equals("saweb_latest.zip")).ToList();
                files = files.GroupBy(c => c.Name).
                    Select(g => g.OrderByDescending(c => c.ModifiedByMeTime).First()).ToList();
                ViewBag.msg += "4-";
                //string FilePath = FolderPath + "saweb_rama.zip";
                sgen.SetSession(MyGuid, "fstatus", "2/" + files[0].Size + "/0");
                sgen.SetSession(MyGuid, "zstatus", "z/0/0");
                string FilePath = DownloadGoogleFile(files[0].Id, files[0].Size.ToString());
                //string tpath = HttpRuntime.AppDomainAppPath;
                string tpath = "c:\\development";
                ViewBag.msg += "5-";

                if (!Directory.Exists(tpath)) Directory.CreateDirectory(tpath);

                ViewBag.msg += "9-";
                //int i = 1;
                //using (ZipArchive za = ZipFile.OpenRead(FilePath))
                //{
                //    ViewBag.msg += "10-";
                //    foreach (ZipArchiveEntry zaItem in za.Entries)
                //    {
                //        Session["zstatus"] = "z/" + i + "/" + za.Entries.Count;
                //        i++;
                //        if (zaItem.FullName.EndsWith("/"))
                //        {
                //            if (!Directory.Exists("" + tpath + "\\" + zaItem.FullName.TrimEnd('/') + ""))
                //            {
                //                Directory.CreateDirectory("" + tpath + "\\" + zaItem.FullName.TrimEnd('/') + "");
                //            }
                //        }
                //        else
                //        {
                //            zaItem.ExtractToFile(tpath + "\\" + zaItem.FullName, true);
                //        }
                //    }
                //}
                //sgen.Alter_Table(userCode, clientid_mst, unitid_mst, userid_mst);
            }
            ModelState.Clear();
            ViewBag.msg = "File Donloaded ";
            //return Content("");
            return View(model);
        }
        [HttpPost]
        public ContentResult Prog(string value)
        {
            string reslt = "";
            try
            {
                reslt = sgen.GetSession(MyGuid, "fstatus").ToString() + "#" + sgen.GetSession(MyGuid, "zstatus").ToString();
            }
            catch (Exception ex) { reslt = "Err"; }
            return Content(reslt);
        }
        /// <returns>The inserted permission, null is returned if an API error occurred</returns>
        public Permission InsertPermission(DriveService service, String fileId, String who, String type, String role)
        {
            Permission newPermission = new Permission();
            newPermission.EmailAddress = who;
            newPermission.Type = type;
            newPermission.Role = role;
            try
            {
                return service.Permissions.Create(newPermission, fileId).Execute();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }
            return null;
        }
        public void createFolder()
        {
            Google.Apis.Drive.v3.Data.File NewDirectory = null;

            // Create metaData for a new Directory
            Google.Apis.Drive.v3.Data.File body = new Google.Apis.Drive.v3.Data.File();
            body.Name = "NAMe";
            body.Description = "descfff";
            body.MimeType = "application/vnd.google-apps.folder";

            try
            {
                FilesResource.CreateRequest request = GetService().Files.Create(body);
                NewDirectory = request.Execute();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }

            //var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            //{
            //    Name = "Invoices",
            //    MimeType = "application/vnd.google-apps.folder"
            //};
            //Google.Apis.Drive.v3.Data.File body = new Google.Apis.Drive.v3.Data.File();
            //body.Name = "document title";
            //body.Description = "document description";
            //body.MimeType = "application/vnd.google-apps.folder";

            //// service is an authorized Drive API service instance
            //Google.Apis.Drive.v3.Data.File file = GetService().Files.Create(body).Execute();
        }
        public string[] Scopes = { DriveService.Scope.Drive };

        public DriveService GetService()
        {
            UserCredential credential;
            string path = System.Web.HttpContext.Current.Server.MapPath("~/credentials.json");
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                String FolderPath = @"D:\";
                //String FilePath = Path.Combine(FolderPath, "DriveServiceCredentials.json");
                string credPath = System.Web.HttpContext.Current.Server.MapPath("~/token.json");
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "admin",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }
            //Create Drive API service.
            DriveService service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "GoogleDriveRestAPI-v3",
            });

            return service;
        }

        public List<GoogleDriveFiles> GetDriveFiles()
        {
            DriveService service = GetService();

            // Define parameters of request.
            FilesResource.ListRequest FileListRequest = service.Files.List();

            //listRequest.PageSize = 10;
            //listRequest.PageToken = 10;
            FileListRequest.Fields = "nextPageToken, files(id, name, size, version, trashed, createdTime)";

            // List files.
            IList<Google.Apis.Drive.v3.Data.File> files = FileListRequest.Execute().Files;
            files = files.Where(w => w.Name.ToLower().Equals("saweb5.rar")).GroupBy(c => c.Name).Select(g => g.OrderByDescending(c => c.ModifiedByMeTime).First()).ToList();

            List<GoogleDriveFiles> FileList = new List<GoogleDriveFiles>();
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    GoogleDriveFiles File = new GoogleDriveFiles
                    {
                        Id = file.Id,
                        Name = file.Name,
                        Size = file.Size,
                        Version = file.Version,
                        CreatedTime = file.CreatedTime,

                    };

                    FileList.Add(File);

                }
            }
            return FileList;
        }

        public void FileUpload(HttpPostedFile file)
        {

            if (file != null && file.ContentLength > 0)
            {
                DriveService service = GetService();

                string path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/GoogleDriveFiles"),
                Path.GetFileName(file.FileName));
                file.SaveAs(path);

                var FileMetaData = new Google.Apis.Drive.v3.Data.File();
                FileMetaData.Name = Path.GetFileName(file.FileName);
                FileMetaData.MimeType = MimeMapping.GetMimeMapping(path);
                //FileMetaData.Parents(Arrays.asList(new ParentReference().setId(folderId)));

                FilesResource.CreateMediaUpload request;

                using (var stream = new System.IO.FileStream(path, System.IO.FileMode.Open))
                {
                    request = service.Files.Create(FileMetaData, stream, FileMetaData.MimeType);
                    request.Fields = "id";
                    request.Upload();
                }
            }
        }
        //public static ParentReference insertFileIntoFolder(DriveService service, String folderId,
        // String fileId)
        //{
        //    ParentReference newParent = new ParentReference();
        //    newParent.Id = folderId;
        //    try
        //    {
        //        return service.Parents.Insert(newParent, fileId).Execute();
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("An error occurred: " + e.Message);
        //    }
        //    return null;
        //}

        public string DownloadGoogleFile(string fileId, string size)
        {
            DriveService service = GetService();
            ViewBag.msg += "6-";
            string FolderPath = System.Web.HttpContext.Current.Server.MapPath("/GoogleDriveFiles/");
            FilesResource.GetRequest request = service.Files.Get(fileId);
            var filesize = size;
            string FileName = request.Execute().Name;
            string FilePath = System.IO.Path.Combine(FolderPath, FileName);
            ViewBag.msg += "7-";
            MemoryStream stream1 = new MemoryStream();

            request.MediaDownloader.ProgressChanged += (Google.Apis.Download.IDownloadProgress progress) =>
            {
                switch (progress.Status)
                {
                    case DownloadStatus.Downloading:
                        {
                            //Console.WriteLine(progress.BytesDownloaded);
                            sgen.SetSession(MyGuid, "fstatus", "2/" + filesize + "/" + progress.BytesDownloaded);
                            sgen.SetSession(MyGuid, "zstatus", "z/0/100");
                            break;
                        }
                    case DownloadStatus.Completed:
                        {
                            //Console.WriteLine("Download complete.");
                            SaveStream(stream1, FilePath);
                            sgen.SetSession(MyGuid, "fstatus", "1/" + filesize + "/" + progress.BytesDownloaded);
                            sgen.SetSession(MyGuid, "zstatus", "z/0/100");

                            break;
                        }
                    case DownloadStatus.Failed:
                        {
                            ViewBag.msg += progress.Exception.Message.ToString();
                            //Console.WriteLine("Download failed.");
                            sgen.SetSession(MyGuid, "fstatus", "0/" + filesize + "/" + progress.BytesDownloaded);
                            sgen.SetSession(MyGuid, "zstatus", "z/0/100");
                            break;
                        }
                }
            };
            ViewBag.msg += "8-";
            request.Download(stream1);

            return FilePath;
        }

        private static void SaveStream(MemoryStream stream, string FilePath)
        {
            using (System.IO.FileStream file = new FileStream(FilePath, FileMode.Create, FileAccess.ReadWrite))
            {
                stream.WriteTo(file);
            }
        }

        public void DeleteFile(GoogleDriveFiles files)
        {
            DriveService service = GetService();
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");

                if (files == null)
                    throw new ArgumentNullException(files.Id);

                // Make the request.
                service.Files.Delete(files.Id).Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Files.Delete failed.", ex);
            }
        }

        public class GoogleDriveFiles
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public long? Size { get; set; }
            public long? Version { get; set; }
            public DateTime? CreatedTime { get; set; }

        }

        #endregion

        #region urights

        public ActionResult urights()
        {
            FillMst();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "USER ROLE RIGHTS";
            model[0].Col12 = "K99";
            model[0].Col10 = "urights";
            model[0].Col11 = " and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            model[0].TList1 = mod1;
            model[0].TList2 = mod1;
            model[0].SelectedItems1 = new string[] { "" };
            model[0].SelectedItems2 = new string[] { "" };
            ViewBag.vgetdt = "disabled='disabled'";
            return View(model);
        }

        [HttpPost]
        public ActionResult urights(List<Tmodelmain> model, string command, string hfrow, string hftable)
        {
            FillMst();
            DataSet ds = new DataSet();
            if (hftable != null)
            {
                ds = sgen.setDS(hftable);
                model[0].dt1 = ds.Tables[0];
            }
            var tm = model.FirstOrDefault();
            #region
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<SelectListItem> mod2 = new List<SelectListItem>();
            model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
            model[0].TList2 = (List<SelectListItem>)TempData[MyGuid + "_TList2"];
            TempData[MyGuid + "_TList1"] = model[0].TList1;
            TempData[MyGuid + "_TList2"] = model[0].TList2;
            if (model[0].SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
            if (model[0].SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
            #endregion

            if (command == "New")
            {
                try
                {
                    model = getnew(model);
                    mod1 = new List<SelectListItem>();
                    mod2 = new List<SelectListItem>();

                    DataTable dtmod = new DataTable();
                    //module                   
                    mq1 = "";

                    #region MOD
                    //mq = sgen.seekval(userCode, "select mod_id from user_details where rec_id='" + Convert.ToInt32(userid_mst) + "'", "mod_id");
                    mq = sgen.seekval(userCode, "select mod_id from user_details where vch_num='" + userid_mst + "'", "mod_id");
                    if (!role_mst.ToUpper().Equals("OWNER")) mq1 = " and mod_id in ('" + mq.Replace(",", "','") + "')";
                    mq = "SELECT m_module3,mod_Id, m_name|| ' ('||mod_id ||')' as nameid FROM com_module join menus on com_module.mod_id=menus.m_id and upper(com_code) = upper('" + userCode + "')" + mq1 + " " +
                        "union all select 'common' m_module3,'-' mod_id,'Common (-)' nameid from dual";

                    dtmod = sgen.getdata(userCode, mq);
                    if (dtmod.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtmod.Rows)
                        {
                            mod1.Add(new SelectListItem { Text = dr["nameid"].ToString(), Value = dr["m_module3"].ToString() });
                        }
                    }
                    #endregion

                    TempData[MyGuid + "_TList1"] = mod1;
                    model[0].TList1 = mod1;
                    model[0].SelectedItems1 = new string[] { "" };

                    //role                   
                    TempData[MyGuid + "_TList2"] = mod2;
                    model[0].TList2 = mod2;
                    model[0].SelectedItems2 = new string[] { "" };

                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vgetdt = "";
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
            else if (command == "ddl_role")
            {
                var modid = "";
                try
                {
                    modid = model[0].TList1.SingleOrDefault(w => w.Value == model[0].SelectedItems1.FirstOrDefault()).Text.Split('(')[1].Trim().Split(')')[0].Trim();
                }
                catch (Exception ex) { }
                if (modid.Equals("-"))
                {
                    mq = "select '-' u_id,'-' m_id,'-' r_name,'-' m_name from dual";
                }
                else
                {
                    mq = "SELECT u_id,m_id,(r_name||' ('||u_id||')') r_name,m_name FROM role_authority where m_id='" + modid + "'";
                }
                DataTable dtr = sgen.getdata(userCode, mq);
                if (dtr.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtr.Rows)
                    {
                        mod2.Add(new SelectListItem { Text = dr["r_name"].ToString(), Value = dr["u_id"].ToString() });
                    }
                }

                TempData[MyGuid + "_TList2"] = mod2;
                model[0].TList2 = mod2;
                model[0].SelectedItems2 = new string[] { "" };
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
            }
            else if (command == "Get Data")
            {
                try
                {
                    string modid = model[0].SelectedItems1.FirstOrDefault();
                    mq = "select * from (select '' Sno,(trim(m_id)) m_id,m_name,m_link,m_submenu,'false' Btn_New,'false' Btn_Edit,'false' Btn_View,'false' Btn_Extend " +
                        "from menus where m_module3 in ('" + modid.Replace(",", "','") + "')) a order by m_id asc";
                    DataTable dt = sgen.getdata(userCode, mq);
                    if (dt.Rows.Count > 0)
                    {
                        model[0].dt1 = dt;
                    }
                    sgen.SetSession(MyGuid, "dturights", dt);
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vgetdt = "";
                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
            }
            else if (command == "Save" || command == "Update")
            {
                try
                {
                    Satransaction sat = new Satransaction(userCode, MyGuid);
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime(userCode);
                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        mq1 = " and vch_num<>'" + model[0].Col16.Trim() + "'";
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

                    string cond = sgen.seekval(userCode, "select distinct role from urights where type='" + model[0].Col12 + "'" + model[0].Col11 + " and " +
                        "m_module3='" + model[0].SelectedItems1.FirstOrDefault() + "' and role='" + model[0].SelectedItems2.FirstOrDefault() + "'" + mq1 + "", "role");
                    if (!cond.Equals("0"))
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'Data Already exist for this role', 2);";
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        return View(model);
                    }

                    DataTable dataTable = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    for (int i = 0; i < model[0].dt1.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(model[0].dt1.Rows[i][5].ToString()) == true || Convert.ToBoolean(model[0].dt1.Rows[i][6].ToString()) == true ||
                            Convert.ToBoolean(model[0].dt1.Rows[i][7].ToString()) == true || Convert.ToBoolean(model[0].dt1.Rows[i][8].ToString()) == true)
                        {
                            DataRow dr = dataTable.NewRow();
                            dr["rec_id"] = "0";
                            dr["vch_num"] = vch_num.Trim();
                            dr["vch_date"] = currdate;
                            dr["type"] = model[0].Col12;
                            dr["m_module3"] = model[0].SelectedItems1.FirstOrDefault();
                            dr["role"] = model[0].SelectedItems2.FirstOrDefault();
                            dr["modid"] = model[0].TList1.SingleOrDefault(w => w.Value == model[0].SelectedItems1.FirstOrDefault()).Text.Split('(')[1].Trim().Split(')')[0].Trim();

                            //dt====
                            dr["m_id"] = model[0].dt1.Rows[i][1].ToString();
                            dr["btnnew"] = Convert.ToBoolean(model[0].dt1.Rows[i][5].ToString()) == true ? "Y" : "N";
                            dr["btnedit"] = Convert.ToBoolean(model[0].dt1.Rows[i][6].ToString()) == true ? "Y" : "N";
                            dr["btnview"] = Convert.ToBoolean(model[0].dt1.Rows[i][7].ToString()) == true ? "Y" : "N"; ;
                            dr["btnextend"] = Convert.ToBoolean(model[0].dt1.Rows[i][8].ToString()) == true ? "Y" : "N";

                            if (isedit)
                            {
                                dr["ent_by"] = model[0].Col3;
                                dr["ent_date"] = model[0].Col4;
                                dr["client_id"] = clientid_mst;
                                dr["client_unit_id"] = unitid_mst;
                                dr["edit_by"] = userid_mst;
                                dr["edit_date"] = currdate;
                            }
                            else
                            {

                                dr["ent_by"] = userid_mst;
                                dr["ent_date"] = currdate;
                                dr["edit_by"] = "-";
                                dr["edit_date"] = currdate;
                                dr["client_id"] = clientid_mst;
                                dr["client_unit_id"] = unitid_mst;
                            }
                            dataTable.Rows.Add(dr);
                        }
                    }
                    res = sgen.Update_data_uncommit2(userCode, dataTable, model[0].Col10.Trim(), tmodel.Col8, isedit, sat);
                    if (res == true)
                    {
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col9 = tmodel.Col9,
                            Col10 = tmodel.Col10,
                            Col11 = tmodel.Col11,
                            Col12 = tmodel.Col12,
                            Col13 = "Save",
                            Col14 = tmodel.Col14,
                            Col15 = tmodel.Col15,
                            TList1 = mod1,
                            TList2 = mod2,
                            SelectedItems1 = new string[] { "" },
                            SelectedItems2 = new string[] { "" }
                        });
                        sat.Commit();
                        ViewBag.vnew = "";
                        ViewBag.vsave = "disabled='disabled'";
                        ViewBag.vgetdt = "disabled='disabled'";
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                    }
                    else
                    {
                        sat.Rollback();
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vgetdt = "";
                        ViewBag.scripCall += "showmsgJS(1, 'Something went wrong', 0);";
                        ModelState.Clear();
                        return View(model);
                    }
                }
                catch (Exception ex) { ViewBag.scripCall += "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
            }
            ModelState.Clear();
            return View(model);
        }

        #endregion

        #region urights_btn

        public ActionResult urights_btn()
        {
            FillMst();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "USER RIGHTS";
            model[0].Col12 = "K98";
            model[0].Col10 = "urights";
            model[0].Col11 = " and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            model[0].TList1 = mod1;
            model[0].SelectedItems1 = new string[] { "" };
            ViewBag.vgetdt = "disabled='disabled'";
            return View(model);
        }

        [HttpPost]
        public ActionResult urights_btn(List<Tmodelmain> model, string command, string hfrow, string hftable)
        {
            FillMst();
            DataTable dtm = new DataTable();
            DataSet ds = new DataSet();
            if (hftable != null)
            {
                ds = sgen.setDS(hftable);
                model[0].dt1 = ds.Tables[0];
            }
            var tm = model.FirstOrDefault();
            #region
            List<SelectListItem> mod1 = new List<SelectListItem>();
            model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
            TempData[MyGuid + "_TList1"] = model[0].TList1;
            if (model[0].SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
            #endregion

            if (command == "New")
            {
                try
                {
                    model = getnew(model);
                    mod1 = new List<SelectListItem>();
                    //mod2 = new List<SelectListItem>();

                    DataTable dtmod = new DataTable();
                    //module                   
                    mq1 = "";

                    #region user                    
                    if (!role_mst.ToUpper().Equals("OWNER")) mq1 = " and ent_by='" + userid_mst + "'";
                    //mq = "select lpad(rec_id,6,0) rec_id,user_id from user_details where type='CPR'" + mq1 + "";
                    mq = "select vch_num,user_id from user_details where type='CPR'" + mq1 + "";


                    dtmod = sgen.getdata(userCode, mq);
                    if (dtmod.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtmod.Rows)
                        {
                            mod1.Add(new SelectListItem { Text = dr["user_id"].ToString(), Value = dr["vch_num"].ToString() });
                        }
                    }
                    #endregion

                    TempData[MyGuid + "_TList1"] = mod1;
                    model[0].TList1 = mod1;
                    model[0].SelectedItems1 = new string[] { "" };

                    ////role                   
                    //TempData[MyGuid + "_TList2"] = mod2;
                    //model[0].TList2 = mod2;
                    //model[0].SelectedItems2 = new string[] { "" };

                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vgetdt = "";
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
            else if (command == "ddl_role")
            {
                DataTable dtr = new DataTable();
                mq1 = "";
                model[0].Col17 = "";
                model[0].Col18 = "";
                model[0].Col19 = "";

                #region MOD

                dtm = sgen.getdata(userCode, "select mod_id,role from user_details where vch_num='" + model[0].SelectedItems1.FirstOrDefault() + "'");
                if (dtm.Rows.Count > 0)
                {
                    mq = dtm.Rows[0]["mod_id"].ToString();
                    if (!dtm.Rows[0]["role"].ToString().Equals("OWNER")) mq1 = " and mod_id in ('" + mq.Replace(",", "','") + "')";
                    mq = "select group_concat(m_module3) m_module3,group_concat(nameid) nameid from " +
                        "(SELECT m_module3,mod_Id, m_name|| ' ('||mod_id ||')' as nameid FROM com_module " +
                        "join menus on com_module.mod_id=menus.m_id and upper(com_code) = upper('" + userCode + "')" + mq1 + " " +
                        "union all " +
                        "select 'common' m_module3,'-' mod_id,'Common (-)' nameid from dual) a";
                    dtr = sgen.getdata(userCode, mq);
                    if (dtr.Rows.Count > 0)
                    {
                        model[0].Col17 = dtr.Rows[0]["nameid"].ToString();
                        model[0].Col18 = dtr.Rows[0]["m_module3"].ToString();
                        model[0].Col19 = dtm.Rows[0]["role"].ToString();
                    }
                }
                #endregion
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
            }
            else if (command == "lbl_role")
            {
                //var modid = "";
                //try
                //{
                //    modid = model[0].TList2.SingleOrDefault(w => w.Value == model[0].SelectedItems2.FirstOrDefault()).Text.Split('(')[1].Trim().Split(')')[0].Trim();
                //}
                //catch (Exception ex) { }
                //if (modid.Equals("-"))
                //{
                //    model[0].Col17 = "-";
                //}
                //else
                //{
                //    mq = "select role from user_details where lpad(rec_id,6,0)='" + model[0].SelectedItems1.FirstOrDefault() + "' and find_in_set(mod_id,'" + modid + "')=1";

                //    DataTable dtr = sgen.getdata(userCode, mq);
                //    if (dtr.Rows.Count > 0)
                //    {
                //        string[] rolearr = dtr.Rows[0]["role"].ToString().Split(',');
                //        foreach (string role in rolearr)
                //        {
                //            if (role.Split('-')[0].Trim() == model[0].SelectedItems2.FirstOrDefault())
                //            {
                //                model[0].Col17 = role;
                //            }
                //        }
                //    }
                //}

                //ViewBag.vnew = "disabled='disabled'";
                //ViewBag.vedit = "disabled='disabled'";
            }
            else if (command == "Get Data")
            {
                try
                {
                    //string modid = model[0].SelectedItems2.FirstOrDefault();
                    //mq = "select * from (select '' Sno,(trim(m_id)) m_id,m_name,m_link,m_submenu,'false' Btn_New,'false' Btn_Edit,'false' Btn_Save,'false' Btn_Save_and_New,'false' Btn_View,'false' Btn_Extend " +
                    //    "from menus where m_module3 in ('" + modid + "')) a order by m_id asc";

                    mq = "select DISTINCT '' Sno,m.m_id,m.m_name,m.m_link,m.m_submenu,m.m_module3," +
                         "(case when ex.btnnew='Y' then 'true' else 'false' end) btnnew," +
                         "(case when ex.btnedit='Y' then 'true' else 'false' end) btnedit," +
                         "(case when ex.btnview='Y' then 'true' else 'false' end) btnview," +
                         "(case when ex.btnextend='Y' then 'true' else 'false' end) btnextend," +
                         "(case when ex.FAVMENU='Y' then 'true' else 'false' end) FAVMENU from menus m " +
                         "left join urights ex on ex.m_id = m.m_id and ex.m_module3 = m.m_module3 and ex.type = 'K99' and role in ('" + model[0].Col19.Replace(",", "','").Trim() + "')" +
                         "where m.m_module3 in ('" + model[0].Col18.Replace(",", "','") + "') order by m.m_id asc";



                    DataTable dt = sgen.getdata(userCode, mq);
                    if (dt.Rows.Count > 0) model[0].dt1 = dt;
                    sgen.SetSession(MyGuid, "dturightsbtn", dt);
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vgetdt = "";
                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
            }
            else if (command == "Save" || command == "Update")
            {
                try
                {
                    Satransaction sat = new Satransaction(userCode, MyGuid);
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime(userCode);
                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        mq1 = " and vch_num<>'" + model[0].Col16.Trim() + "'";
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

                    string cond = sgen.seekval(userCode, "select distinct userid from urights where type='" + model[0].Col12 + "'" + model[0].Col11 + " and " +
                        "userid='" + model[0].SelectedItems1.FirstOrDefault() + "'" + mq1 + "", "userid");
                    if (!cond.Equals("0"))
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'Data Already exist for this User', 2);";
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        return View(model);
                    }

                    DataTable dataTable = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    for (int i = 0; i < model[0].dt1.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(model[0].dt1.Rows[i][6].ToString()) == true || Convert.ToBoolean(model[0].dt1.Rows[i][7].ToString()) == true ||
                            Convert.ToBoolean(model[0].dt1.Rows[i][8].ToString()) == true || Convert.ToBoolean(model[0].dt1.Rows[i][9].ToString()) == true)
                        {
                            DataRow dr = dataTable.NewRow();
                            dr["rec_id"] = "0";
                            dr["vch_num"] = vch_num.Trim();
                            dr["vch_date"] = currdate;
                            dr["type"] = model[0].Col12;
                            //dr["role"] = model[0].Col17;
                            dr["userid"] = model[0].SelectedItems1.FirstOrDefault();
                            //dr["modid"] = model[0].TList2.SingleOrDefault(w => w.Value == model[0].SelectedItems2.FirstOrDefault()).Text.Split('(')[1].Trim().Split(')')[0].Trim();

                            //dt====
                            dr["m_id"] = model[0].dt1.Rows[i][1].ToString();
                            dr["m_module3"] = model[0].dt1.Rows[i][5].ToString();
                            dr["btnnew"] = Convert.ToBoolean(model[0].dt1.Rows[i][6].ToString()) == true ? "Y" : "N";
                            dr["btnedit"] = Convert.ToBoolean(model[0].dt1.Rows[i][7].ToString()) == true ? "Y" : "N";
                            dr["btnview"] = Convert.ToBoolean(model[0].dt1.Rows[i][8].ToString()) == true ? "Y" : "N";
                            dr["btnextend"] = Convert.ToBoolean(model[0].dt1.Rows[i][9].ToString()) == true ? "Y" : "N";
                            dr["FAVMENU"] = Convert.ToBoolean(model[0].dt1.Rows[i][10].ToString()) == true ? "Y" : "N";
                            if (isedit)
                            {
                                dr["ent_by"] = model[0].Col3;
                                dr["ent_date"] = model[0].Col4;
                                dr["client_id"] = clientid_mst;
                                dr["client_unit_id"] = unitid_mst;
                                dr["edit_by"] = userid_mst;
                                dr["edit_date"] = currdate;
                            }
                            else
                            {

                                dr["ent_by"] = userid_mst;
                                dr["ent_date"] = currdate;
                                dr["edit_by"] = "-";
                                dr["edit_date"] = currdate;
                                dr["client_id"] = clientid_mst;
                                dr["client_unit_id"] = unitid_mst;
                            }
                            dataTable.Rows.Add(dr);
                        }
                    }
                    res = sgen.Update_data_uncommit2(userCode, dataTable, model[0].Col10.Trim(), tmodel.Col8, isedit, sat);
                    if (res == true)
                    {
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col9 = tmodel.Col9,
                            Col10 = tmodel.Col10,
                            Col11 = tmodel.Col11,
                            Col12 = tmodel.Col12,
                            Col13 = "Save",
                            Col14 = tmodel.Col14,
                            Col15 = tmodel.Col15,
                            TList1 = mod1,
                            //TList2 = mod2,
                            SelectedItems1 = new string[] { "" },
                            //SelectedItems2 = new string[] { "" }
                        });
                        sgen.SetSession(MyGuid, "dturightsbtn", null);
                        sat.Commit();
                        ViewBag.vnew = "";
                        ViewBag.vsave = "disabled='disabled'";
                        ViewBag.vgetdt = "disabled='disabled'";
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                    }
                    else
                    {
                        sat.Rollback();
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vgetdt = "";
                        ViewBag.scripCall += "showmsgJS(1, 'Something went wrong', 0);";
                        ModelState.Clear();
                        return View(model);
                    }
                }
                catch (Exception ex) { ViewBag.scripCall += "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
            }
            ModelState.Clear();
            return View(model);
        }

        #endregion

        #region Forgot Password
        public ActionResult forgot_pswd(string m_id)
        {
            FillMst(EncryptDecrypt.Decrypt(m_id));
            userCode = sgen.getUserCode();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            //model.Add(new Tmodelmain { Col1 = "" });
            ViewBag.scripCall = "enableForm();";
            model[0].Col9 = "FORGOT PASSWORD";
            model[0].Col10 = m_id;
            model[0].Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";



            return View(model);
        }

        [HttpPost]
        public ActionResult forgot_pswd(List<Tmodelmain> model, string command, string mid)
        {
            MyGuid = EncryptDecrypt.Decrypt(model[0].Col10);
            FillMst(MyGuid);

            //Session["viewName"] = actionName;
            //Session["controllerName"] = controllerName;
            #region
            var tm = model.FirstOrDefault();

            #endregion
            if (command == "Submit")
            {
                DataTable dt = new DataTable();
                dt = sgen.getdata(userCode, "SELECT user_id,email,password,first_name,client_id FROM user_details where lower(user_id) ='" + sgen.LowerCase(model[0].Col17) + "'");
                Int32 RowCount = dt.Rows.Count;
                if (dt.Rows.Count > 0)
                {
                    string textuser;
                    string txtpasswrd;
                    string emailid;
                    string fname;
                    int i = 0;

                    textuser = dt.Rows[i]["user_id"].ToString();
                    emailid = dt.Rows[i]["email"].ToString();
                    txtpasswrd = dt.Rows[i]["password"].ToString();
                    string pwd = EncryptDecrypt.Decrypt(txtpasswrd);
                    fname = dt.Rows[i]["first_name"].ToString();
                    sgen.SetSession(MyGuid, "paswrd", txtpasswrd);
                    sgen.SetSession(MyGuid, "user_id", textuser);

                    string client_id = "";

                    string clientid = Convert.ToString(dt.Rows[0]["client_id"]);

                    #region
                    if (clientid == "owner")
                    {
                        client_id = "001";
                    }
                    else
                    {
                        string s = clientid;
                        string[] values = s.Split(',');
                        for (int j = 0; j < values.Length; j++)
                        {
                            values[j] = values[j].Trim();
                            client_id = values[0];
                        }
                    }
                    #endregion

                    DataTable datatable = new DataTable();
                    // datatable = sgen.getdata(userCode, "select * from user_details where User_Id='" + userid + "'");
                    datatable = sgen.getdata(userCode, "select com_email,com_password,com_port,com_smtp,Company_Profile_Id,type from company_profile where Company_Profile_Id='" + client_id + "' and type='CP'");
                    if (datatable.Rows.Count > 0)
                    {
                        string mailfrom = Convert.ToString(datatable.Rows[0]["com_email"]);
                        string msg_txt = "Hi" + "\r\n" + fname + "<br/>" + "Plz Find Your Login Credentials" + "<br/>" + "<br/>" + "Your User Id Is :" + textuser + "<br/>" + " Your Password Is :" + "\r\n" + pwd + "<br/>" + "<br/>" + "Regards" + "<br/>" + "Administration";

                        var body = new StringBuilder();
                        body.AppendLine(@"<b style='color:#3caee9; font-size: 20px'> " + model[0].Col32 + " </b><br /><hr style='border:1px solid black' />" +
                             "<p>Hi <b> : </b>,</p><table style='font-weight:600'>");


                        body.AppendLine(@"</table><br /><p>" + msg_txt + "</p>");
                        sgen.Send_mail_SA1(userCode, client_id, unitid_mst, emailid, "", "", body.ToString(), "Recovery Of Your Password", "");

                        string result = fname + "******";
                        model[0].Col18 = "Username and Password sent to " + result + " successfully";
                    }
                }
                else { model[0].Col18 = "UserId Is Wrong"; }
                model[0].Col14 = tm.Col14;
                model[0].Col15 = tm.Col15;


            }




            ModelState.Clear();
            return View(model);




        }
        #endregion

        # region Reset Password
        public ActionResult reset_pswd(string mid)
        {
            FillMst();


            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            ViewBag.scripCall = "enableForm();";
            model[0].Col9 = "RESET PASSWORD";
            model[0].Col10 = "";
            model[0].Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            //model[0].Col16 = userid_mst;

            DataTable dt = new DataTable();
            //mq = "select user_id from user_details where lpad(rec_id,6,'0')='" + userid_mst + "'";
            mq = "select user_id from user_details where vch_num='" + userid_mst + "'";
            dt = sgen.getdata(userCode, mq);
            if (dt.Rows.Count > 0)
            {
                model[0].Col17 = dt.Rows[0]["user_id"].ToString().Trim();
                model[0].Col18 = "";
            }



            return View(model);
        }

        [HttpPost]
        public ActionResult reset_pswd(List<Tmodelmain> model, string command, string mid)
        {
            FillMst();



            var tm = model.FirstOrDefault();


            if (command == "Submit")
            {

                string cur_pwd = EncryptDecrypt.Encrypt(model[0].Col18.Trim());
                string new_pwd = EncryptDecrypt.Encrypt(model[0].Col19.Trim());
                string con_pwd = EncryptDecrypt.Encrypt(model[0].Col20.Trim());
                string decrpt_pwd = EncryptDecrypt.Decrypt(new_pwd);

                if (model[0].Col19 != model[0].Col20)
                {
                    ViewBag.scripCall += "showmsgJS(1, 'New And Confirm Password Should Be Equal ', 2);";

                }

                else
                {
                    DataTable dt = new DataTable();
                    dt = sgen.getdata(userCode, "SELECT user_id,password,email,first_name FROM user_details where user_id='" + model[0].Col17 + "'");
                    int RowCount = dt.Rows.Count;
                    if (dt.Rows.Count > 0)
                    {
                        string textuser;
                        string txtpasswrd;
                        string txtemail;
                        string name;
                        int i = 0;
                        //string clientName;
                        //string clientUnitName;
                        textuser = dt.Rows[i]["user_id"].ToString();
                        txtpasswrd = dt.Rows[i]["password"].ToString();
                        txtemail = dt.Rows[i]["email"].ToString();
                        name = dt.Rows[i]["first_name"].ToString();


                        sgen.SetSession(MyGuid, "paswrd", txtpasswrd);
                        ////Session["user_id"] = textuser;

                        if (textuser == model[0].Col17 && txtpasswrd == cur_pwd)
                        {
                            //sgen.execute_cmd(userCode, "UPDATE LOGIN_DETAILS SET PASSWORD='" + new_pwd + "' WHERE USER_ID='" + txtuser.Text + "'");

                            string mq = "UPDATE USER_DETAILS SET PASSWORD='" + new_pwd + "' WHERE USER_ID='" + model[0].Col17 + "'";
                            bool data1 = sgen.execute_cmd(userCode, mq);
                            bool data2 = sgen.execute_cmd(userCode, "commit");

                            // sending mail
                            DataTable datatable = new DataTable();
                            // datatable = sgen.getdata(userCode, "select * from user_details where User_Id='" + txtuser.Text + "'");
                            datatable = sgen.getdata(userCode, "select com_email,com_password,com_port,com_smtp,Company_Profile_Id,type from company_profile where Company_Profile_Id='" + clientid_mst + "' and type='CP'");
                            if (datatable.Rows.Count > 0)
                            {



                                string smtpvalue = Convert.ToString(datatable.Rows[0]["com_smtp"]);
                                string emailIdvalue = Convert.ToString(datatable.Rows[0]["com_email"]);
                                string passwordvalue = EncryptDecrypt.Decrypt(datatable.Rows[0]["com_password"].ToString());
                                int portvalue = Convert.ToInt32(datatable.Rows[0]["com_port"]);


                                DataTable dtable = new DataTable();
                                dtable = sgen.getdata(userCode, "select subdomain from company_profile where company_profile_id='" + clientid_mst + "'");
                                if (dtable.Rows.Count > 0)
                                {
                                    string subdomain = Convert.ToString(dtable.Rows[0]["subdomain"]);

                                    try
                                    {
                                        MailMessage msg = new MailMessage();
                                        msg.From = new MailAddress(emailIdvalue);
                                        msg.CC.Add(emailIdvalue);
                                        msg.To.Add(txtemail);
                                        msg.Subject = "Reset Password";

                                        //msg.Body = "Hi" + "\r\n" + name + "<br/>" + "Congratulation!!! " + "<br/>" + "<br/>" + " Your Password is Succesfully Changed" + "<br/>" + "User Id :" + "\r\n" + textuser + "<br/>" + "Password :" + "\r\n" + decrpt_pwd + "<br/>" + "Now You Can Login:" + "\r\n" + subdomain + "<br/>" + "<br/>" + "Regards" + "<br/>" + "Administration";
                                        msg.Body = "<b style='color: #3caee9; font-size: 20px'>Mail from admin</b><br /><hr style='border:1px solid black' /><p>Hi <b>" + name + "</b>,</p><p>Congratulations, This is to notify that your That your Password is Succesfully Changed your login credentials are as below :-</p><table style='font-weight:600'><tr><td>Company Name </td><td>: " + clientname_mst + " </td></tr><tr><td> Unit Name </td><td>: " + unitname_mst + " </td></tr><tr><td> User Id </td><td>: " + textuser + " </td></tr><tr><td>Password </td><td>: " + decrpt_pwd + " </td></tr></table><br /><p>Please login and check.</p><p>Thank you,<br />Administrator<br /></p><br /><p>Note:- Please do not reply to this mail as it is an unmonitered alias.</p>";
                                        msg.IsBodyHtml = true;
                                        SmtpClient smtp = new SmtpClient();
                                        smtp.Host = smtpvalue;
                                        System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                                        NetworkCred.UserName = emailIdvalue;
                                        NetworkCred.Password = passwordvalue;
                                        smtp.UseDefaultCredentials = true;
                                        smtp.Credentials = NetworkCred;
                                        smtp.Port = portvalue;
                                        smtp.EnableSsl = true;
                                        smtp.Send(msg);



                                    }
                                    catch (Exception ex)
                                    {
                                        sgen.showmsg(1, ex.Message.ToString(), 0);
                                    }


                                    ViewBag.scripCall += "showmsgJS(1,'Password Changed Successfully ',1)";

                                    model[0].Col18 = "";
                                    model[0].Col19 = "";
                                    model[0].Col20 = "";

                                }


                                else
                                {

                                    ViewBag.scripCall = "showmsgJS(1,'Please Configre Your Mail',2)";


                                }
                            }
                        }
                        else
                        {


                            model[0].Col21 = "Your Current Password Is Wrong !";
                        }

                    }
                }



            }




            ModelState.Clear();
            return View(model);




        }
        #endregion

        # region Profile
        public ActionResult profile(string mid)
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            ViewBag.scripCall = "enableForm();";
            model[0].Col9 = "PROFILE";
            model[0].Col10 = "";
            model[0].Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            List<SelectListItem> md1 = new List<SelectListItem>();
            List<SelectListItem> md2 = new List<SelectListItem>();
            List<SelectListItem> md3 = new List<SelectListItem>();

            md1.Add(new SelectListItem { Text = "Male", Value = "Male" });
            md1.Add(new SelectListItem { Text = "Female", Value = "Female" });

            md2 = cmd_Fun.country(userCode, unitid_mst);

            model[0].TList1 = md1;
            model[0].TList2 = md2;
            model[0].TList3 = md3;
            model[0].SelectedItems1 = new string[] { "" };
            model[0].SelectedItems2 = new string[] { "" };
            model[0].SelectedItems3 = new string[] { "" };
            TempData[MyGuid + "_TList1"] = model[0].TList1;
            TempData[MyGuid + "_TList2"] = model[0].TList2;
            TempData[MyGuid + "_TList3"] = model[0].TList3;

            mq = "select first_name,middle_name,last_name,email,to_char(dob,'" + sgen.Getsqldateformat() + "')  dob,gender,cur_country,cur_state,cur_city," +
                "cur_address,cur_pincode,con_number,alt_number,profilepic,profilepic_path from user_details" +
                //" where rec_id='" + Convert.ToInt32(userid_mst) + "' and type='CPR'";
                " where vch_num='" + userid_mst + "' and type='CPR'";

            DataTable dt = new DataTable();
            dt = sgen.getdata(userCode, mq);
            if (dt.Rows.Count > 0)
            {
                model[0].Col17 = dt.Rows[0]["first_name"].ToString();
                model[0].Col18 = dt.Rows[0]["middle_name"].ToString();
                model[0].Col19 = dt.Rows[0]["last_name"].ToString();
                model[0].Col20 = dt.Rows[0]["email"].ToString();
                model[0].Col21 = dt.Rows[0]["dob"].ToString();
                model[0].Col22 = dt.Rows[0]["cur_city"].ToString();
                model[0].Col23 = dt.Rows[0]["cur_address"].ToString();
                model[0].Col24 = dt.Rows[0]["cur_pincode"].ToString();
                model[0].Col25 = dt.Rows[0]["con_number"].ToString();
                model[0].Col26 = dt.Rows[0]["alt_number"].ToString();
                model[0].Col27 = dt.Rows[0]["profilepic"].ToString();
                model[0].Col28 = dt.Rows[0]["profilepic_path"].ToString();

                String[] L1 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["gender"].ToString()).Distinct()).Split(',');
                model[0].SelectedItems1 = L1;

                String[] L2 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["cur_country"].ToString()).Distinct()).Split(',');
                model[0].SelectedItems2 = L2;

                TempData[MyGuid + "_TList3"] = model[0].TList3 = md3 = cmd_Fun.ctrystate(userCode, model[0].SelectedItems2.FirstOrDefault());


            }



            return View(model);
        }

        [HttpPost]
        public ActionResult profile(List<Tmodelmain> model, string command, string mid, HttpPostedFileBase upd1)
        {
            FillMst();



            var tm = model.FirstOrDefault();
            List<SelectListItem> mod1 = (List<SelectListItem>)TempData[MyGuid + "_Tlist1"];
            List<SelectListItem> mod2 = (List<SelectListItem>)TempData[MyGuid + "_Tlist2"];
            List<SelectListItem> mod3 = (List<SelectListItem>)TempData[MyGuid + "_Tlist3"];
            model[0].TList1 = mod1;
            model[0].TList2 = mod2;
            model[0].TList3 = mod3;
            TempData[MyGuid + "_Tlist1"] = model[0].TList1;
            TempData[MyGuid + "_Tlist2"] = model[0].TList2;
            TempData[MyGuid + "_Tlist3"] = model[0].TList3;
            if (model[0].SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
            if (model[0].SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
            if (model[0].SelectedItems3 == null) model[0].SelectedItems3 = new string[] { "" };


            if (command == "Update")
            {
                if (model[0].Col17 == null) { model[0].Col17 = "0"; }
                if (model[0].Col18 == null) { model[0].Col18 = "0"; }
                if (model[0].Col19 == null) { model[0].Col19 = "0"; }
                if (model[0].Col20 == null) { model[0].Col20 = "0"; }
                if (model[0].Col21 == null) { model[0].Col21 = "01/01/1900"; }
                if (model[0].Col22 == null) { model[0].Col22 = "0"; }
                if (model[0].Col23 == null) { model[0].Col23 = "0"; }
                if (model[0].Col24 == null) { model[0].Col24 = "0"; }
                if (model[0].Col25 == null) { model[0].Col25 = "0"; }
                if (model[0].Col26 == null) { model[0].Col26 = "0"; }

                if (model[0].SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
                if (model[0].SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
                if (model[0].SelectedItems3 == null) model[0].SelectedItems3 = new string[] { "" };

                string currdate = sgen.server_datetime(userCode);
                string profile_img = "";
                if (upd1 != null)
                {
                    HttpPostedFileBase file = upd1;
                    string serverpath = Server.MapPath("~/Uploads/" + cg_com_name.Replace(" ", "") + "/");
                    string fileName2 = Path.GetFileName(file.FileName);
                    string path2 = clientid_mst + unitid_mst + userid_mst + currdate + "_" + fileName2;
                    string encpath2 = sgen.Convert_Stringto64(path2).ToString();
                    string finalpath2 = serverpath + encpath2;
                    file.SaveAs(finalpath2);
                    profile_img = "profilepic = '" + fileName2 + "',profilepic_path = '" + encpath2 + "',";

                }

                if ((model[0].Col18 == null) || (model[0].Col18 == " ")) { model[0].Col18 = "0"; }

                sgen.execute_cmd(userCode, "UPDATE user_details SET " + profile_img + " first_name='" + model[0].Col17.Trim() + "',middle_name = '" + model[0].Col18.Trim() + "',last_name = '" + model[0].Col19.Trim() + "'," +
                    "email= '" + model[0].Col20.Trim() + "',dob = to_date('" + model[0].Col21 + "','dd/mm/yyyy'),gender = '" + model[0].SelectedItems1.FirstOrDefault() + "'," +
                    "cur_country = '" + model[0].SelectedItems2.FirstOrDefault() + "',cur_state = '" + model[0].SelectedItems3.FirstOrDefault() + "',cur_city = '" + model[0].Col22.Trim() + "'," +
                    "cur_address = '" + model[0].Col23.Trim() + "',cur_pincode = '" + model[0].Col24 + "',con_number = '" + model[0].Col25 + "',alt_number = '" + model[0].Col26 + "' " +
                    //"WHERE rec_id = '" + Convert.ToInt32(userid_mst) + "'  and type='CPR'");
                    "WHERE vch_num = '" + userid_mst + "'  and type='CPR'");

                sgen.execute_cmd(userCode, "commit");



            }

            else if (command == "state")
            {
                TempData[MyGuid + "_TList3"] = model[0].TList3 = mod3 = cmd_Fun.ctrystate(userCode, model[0].SelectedItems2.FirstOrDefault());
            }

            ModelState.Clear();
            return View(model);




        }
        #endregion

        #region dashboard_setting

        public ActionResult db_set()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            List<SelectListItem> mod1 = new List<SelectListItem>();
            model[0].Col9 = "DASHBOARD SETTING";
            type1 = "1";
            ViewBag.btncond = "";
            //mid_mst = "4006";
            //if (mid_mst == "4006")
            //{
            model[0].Col12 = "DBA";
            model[0].Col19 = "Dashoard Admin";
            //div_onmodule.Visible = false;
            //div_search.Visible = false;
            //}

            //else if (mid_mst == "1011.2")
            //{
            //    model[0].Col12 = "DBU";
            //    model[0].Col19 = "Dashboard User";
            //    isuser = true;
            //}
            model[0].Col10 = "DASHBOARD_SETTING";
            model[0].Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            model[0].TList1 = mod1;
            model[0].TList2 = mod1;
            model[0].TList3 = mod1;
            model[0].TList4 = mod1;
            model[0].TList5 = mod1;
            model[0].TList6 = mod1;
            model[0].SelectedItems1 = new string[] { "" };
            model[0].SelectedItems2 = new string[] { "" };
            model[0].SelectedItems3 = new string[] { "" };
            model[0].SelectedItems4 = new string[] { "" };
            model[0].SelectedItems5 = new string[] { "" };
            model[0].SelectedItems6 = new string[] { "" };
            return View(model);
        }
        [HttpPost]
        public ActionResult db_set(List<Tmodelmain> model, string command)
        {
            FillMst();
            string where = "";
            var tm = model.FirstOrDefault();
            #region DDL
            List<SelectListItem> mod1 = (List<SelectListItem>)TempData[MyGuid + "_Tlist1"];
            List<SelectListItem> mod2 = (List<SelectListItem>)TempData[MyGuid + "_Tlist2"];
            List<SelectListItem> mod3 = (List<SelectListItem>)TempData[MyGuid + "_Tlist3"];
            List<SelectListItem> mod4 = (List<SelectListItem>)TempData[MyGuid + "_Tlist4"];
            List<SelectListItem> mod5 = (List<SelectListItem>)TempData[MyGuid + "_Tlist5"];
            List<SelectListItem> mod6 = (List<SelectListItem>)TempData[MyGuid + "_Tlist6"];
            TempData[MyGuid + "_Tlist1"] = mod1;
            TempData[MyGuid + "_Tlist2"] = mod2;
            TempData[MyGuid + "_Tlist3"] = mod3;
            TempData[MyGuid + "_Tlist4"] = mod4;
            TempData[MyGuid + "_Tlist5"] = mod5;
            TempData[MyGuid + "_Tlist6"] = mod6;
            model[0].TList1 = mod1;
            model[0].TList2 = mod2;
            model[0].TList3 = mod3;
            model[0].TList4 = mod4;
            model[0].TList5 = mod5;
            model[0].TList6 = mod6;
            if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
            if (tm.SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
            if (tm.SelectedItems3 == null) model[0].SelectedItems3 = new string[] { "" };
            if (tm.SelectedItems4 == null) model[0].SelectedItems4 = new string[] { "" };
            if (tm.SelectedItems5 == null) model[0].SelectedItems5 = new string[] { "" };
            if (tm.SelectedItems6 == null) model[0].SelectedItems6 = new string[] { "" };
            #endregion

            if (command == "New")
            {
                try
                {
                    //model = getnew(model)
                    sgen.SetSession(MyGuid, "EDMODE", "N");
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                    vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                    model[0].Col16 = vch_num;
                    model[0].Col17 = model[0].Col16;
                    model[0].Col20 = "";
                    model[0].Col13 = "Save";
                    //ViewBag.scripCall = "disableForm();";
                    if (isuser == true)
                    {
                        model[0].Col17 = "";
                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        ViewBag.btncond = "Readonly = true";

                    }
                    else
                    {
                        //ViewBag.scripCall = "enableForm();";

                    }
                    #region DDL
                    mod1 = new List<SelectListItem>();
                    mod2 = new List<SelectListItem>();
                    mod3 = new List<SelectListItem>();
                    mod4 = new List<SelectListItem>();
                    mod5 = new List<SelectListItem>();
                    mod6 = new List<SelectListItem>();

                    #region Drill level
                    mod4.Add(new SelectListItem { Text = "1", Value = "1" });
                    mod4.Add(new SelectListItem { Text = "2", Value = "2" });
                    mod4.Add(new SelectListItem { Text = "3", Value = "3" });
                    mod4.Add(new SelectListItem { Text = "4", Value = "4" });
                    #endregion
                    #region Chart type
                    mod5.Add(new SelectListItem { Text = "Line", Value = "line" });
                    mod5.Add(new SelectListItem { Text = "Pie", Value = "pie" });
                    mod5.Add(new SelectListItem { Text = "Bar", Value = "bar" });
                    mod5.Add(new SelectListItem { Text = "Area", Value = "area" });
                    mod5.Add(new SelectListItem { Text = "Column", Value = "column" });
                    #endregion   
                    #region Status
                    mod6.Add(new SelectListItem { Text = "Yes", Value = "Y" });
                    mod6.Add(new SelectListItem { Text = "No", Value = "N" });
                    #endregion
                    #region level
                    mod1.Add(new SelectListItem { Text = "1", Value = "01" });
                    mod1.Add(new SelectListItem { Text = "2", Value = "02" });
                    mod1.Add(new SelectListItem { Text = "3", Value = "03" });
                    mod1.Add(new SelectListItem { Text = "4", Value = "04" });
                    mod1.Add(new SelectListItem { Text = "5", Value = "05" });
                    mod1.Add(new SelectListItem { Text = "6", Value = "06" });
                    #endregion
                    #region module
                    mq = sgen.seekval(userCode, "select mod_id from user_details where vch_num='" + userid_mst + "'", "mod_id");
                    if (!role_mst.ToUpper().Equals("OWNER")) where = " and mod_id in ('" + mq.Replace(",", "','") + "')";

                    DataTable dtcomp_mod = new DataTable();
                    mq = "SELECT mod_Id, m_name|| ' ('|| mod_id||')' as nameid FROM com_module, menus where com_module.mod_id=menus.m_id and upper(com_code) ='" + userCode + "'" + where + "";
                    dtcomp_mod = sgen.getdata(userCode, mq);
                    if (dtcomp_mod.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtcomp_mod.Rows)
                        {
                            mod2.Add(new SelectListItem { Text = dr["nameid"].ToString(), Value = dr["mod_id"].ToString() });
                            //mod3.Add(new SelectListItem { Text = dr["nameid"].ToString(), Value = dr["mod_id"].ToString() });
                        }

                    }
                    #endregion
                    #region onmodule
                    where = "";
                    mq = sgen.seekval(userCode, "select mod_id from user_details where vch_num='" + userid_mst + "'", "mod_id");
                    if (!role_mst.ToUpper().Equals("OWNER")) where = " and mod_id in ('" + mq.Replace(",", "','") + "')";

                    dtcomp_mod = new DataTable();
                    mq = "SELECT mod_Id, m_name|| ' ('|| mod_id||')' as nameid FROM com_module, menus where com_module.mod_id=menus.m_id and upper(com_code) ='" + userCode + "'" + where + "";
                    dtcomp_mod = sgen.getdata(userCode, mq);
                    if (dtcomp_mod.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtcomp_mod.Rows)
                        {
                            mod3.Add(new SelectListItem { Text = dr["nameid"].ToString(), Value = dr["mod_id"].ToString() });
                        }

                    }
                    #endregion

                    #region TEMPDATA
                    TempData[MyGuid + "_Tlist1"] = mod1;
                    TempData[MyGuid + "_Tlist2"] = mod2;
                    TempData[MyGuid + "_Tlist3"] = mod3;
                    TempData[MyGuid + "_Tlist4"] = mod4;
                    TempData[MyGuid + "_TList5"] = mod5;
                    TempData[MyGuid + "_TList6"] = mod6;
                    #endregion
                    #region tlist
                    model[0].TList1 = mod1;
                    model[0].TList2 = mod2;
                    model[0].TList3 = mod3;
                    model[0].TList4 = mod4;
                    model[0].TList5 = mod5;
                    model[0].TList6 = mod6;
                    #endregion 
                    #endregion
                }
                catch (Exception ex) { }
            }
            else if (command == "Cancel") { return CancelFun(model); }
            else if (command == "Callback")
            {
                //if (model[0].Col134 == "N")
                //{
                //    ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for edit Gate In, please contact your admin', 2);disableForm();";
                //    return View(model);
                //}
                model = StartCallback(model);
            }

            else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
            {
                DataTable dataTable = new DataTable();
                bool Result = false;
                model[0].Col20 = tm.Col20;
                string level = "", drill_level = "";
                try
                {
                    #region Conditions
                    if (model[0].SelectedItems2.FirstOrDefault() == "")
                    {
                        ViewBag.scripCall = "showmsgJS(1,'Please Select Module', 2);";
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        return View(model);
                    }
                    if (model[0].SelectedItems4.FirstOrDefault() == "00")
                    {
                        ViewBag.scripCall = "showmsgJS(1,'Please Select Drilllevel', 2);";
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        return View(model);
                    }
                    if (model[0].SelectedItems5.FirstOrDefault() == "0")
                    {
                        ViewBag.scripCall = "showmsgJS(1,'Please Select Charttype', 2);";
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        return View(model);
                    }
                    if (model[0].SelectedItems6.FirstOrDefault() == "0")
                    {
                        ViewBag.scripCall = "showmsgJS(1,'Please Select Status', 2);";
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        return View(model);
                    }
                    if (model[0].Col18.Trim() == "")
                    {
                        ViewBag.scripCall = "showmsgJS(1,'Please Fill Chart Name', 2);";
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        return View(model);
                    }
                    #endregion

                    string currdate = sgen.server_datetime(userCode);
                    DataTable dtf = new DataTable();
                    dtf = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
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
                    level = model[0].SelectedItems1.FirstOrDefault().Trim();
                    drill_level = model[0].SelectedItems4.FirstOrDefault().Trim();
                    if (model[0].Col20 == "" || model[0].Col20 == null) model[0].Col20 = "-";
                    int len = model[0].Col20.Split(',').Length;
                    for (int i = 0; i < len; i++)
                    {
                        DataRow dr = dtf.NewRow();
                        #region DR
                        dr["rec_id"] = "0";
                        dr["vch_num"] = vch_num;
                        dr["chart_no"] = model[0].Col17.Trim();
                        dr["vch_date"] = currdate;
                        dr["type"] = model[0].Col12;
                        dr["type_desc"] = model[0].Col19;
                        dr["name"] = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(model[0].Col18.Trim().ToLower());
                        dr["level_"] = level;
                        dr["drill_level"] = drill_level;
                        dr["user_id"] = userid_mst;
                        dr["module"] = model[0].SelectedItems2.FirstOrDefault();
                        dr["onmodule"] = model[0].SelectedItems3.FirstOrDefault().Split(',')[i].ToString().Trim();
                        dr["chart_type"] = model[0].SelectedItems5.FirstOrDefault();
                        dr["client_id"] = clientid_mst;
                        dr["client_unit_id"] = unitid_mst;
                        dr["status"] = model[0].SelectedItems6.FirstOrDefault();
                        dr["rec_id"] = "0";
                        if (isedit)
                        {
                            //drn["rec_id"] = model[0].Col7;
                            dr["ent_by"] = model[0].Col3;
                            dr["ent_date"] = model[0].Col4;
                            dr["edit_by"] = userid_mst;
                            dr["edit_date"] = currdate;
                        }
                        else
                        {

                            dr["ent_by"] = userid_mst;
                            dr["ent_date"] = currdate;
                        }
                        #endregion
                        dr = getsave(currdate, dr, model);
                        dtf.Rows.Add(dr);
                    }
                    Result = sgen.Update_data_fast1(userCode, dtf, model[0].Col10.Trim(), model[0].Col8, isedit);
                    if (Result == true)
                    {
                        if (command.Trim().ToUpper().Equals("SAVE") || command.Trim().ToUpper().Equals("UPDATE"))
                        {
                            ViewBag.vnew = "";
                            ViewBag.vedit = "";
                            ViewBag.vsave = "disabled='disabled'";
                            ViewBag.vsavenew = "disabled='disabled'";
                            if (command.Trim().ToUpper().Equals("UPDATE"))
                            {
                                ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Chart Updated');disableForm();";
                            }
                            else
                            {
                                ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Chart Added');disableForm();";

                            }
                        }
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col10 = tm.Col10,
                            Col9 = tm.Col9,
                            Col11 = tm.Col11,
                            Col13 = "Save",
                            Col100 = "Save & New",
                            Col121 = "S",
                            Col122 = "<u>S</u>ave",
                            Col123 = "Save & Ne<u>w</u>",
                            Col14 = tm.Col14,
                            Col15 = tm.Col15,
                            Col12 = tm.Col12,
                            TList1 = mod1,
                            TList2 = mod2,
                            TList3 = mod3,
                            TList4 = mod4,
                            TList5 = mod5,
                            TList6 = mod6,
                            SelectedItems1 = new string[] { "" },
                            SelectedItems2 = new string[] { "" },
                            SelectedItems3 = new string[] { "" },
                            SelectedItems4 = new string[] { "" },
                            SelectedItems5 = new string[] { "" },
                            SelectedItems6 = new string[] { "" },
                            Col20 = ""
                        });
                    }
                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
            }
            ModelState.Clear();
            return View(model);
        }
        #endregion

        #region company profile
        public ActionResult client_profile(string mid)
        {
            FillMst();

            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            sgen.SetSession(MyGuid, "cplogo", null);
            List<SelectListItem> md1 = new List<SelectListItem>();

            TempData[MyGuid + "_TList1"] = model[0].TList1 = md1;
            TempData[MyGuid + "_TList2"] = model[0].TList2 = md1;
            TempData[MyGuid + "_TList3"] = model[0].TList3 = md1;
            TempData[MyGuid + "_TList4"] = model[0].TList4 = md1;
            TempData[MyGuid + "_TList5"] = model[0].TList5 = md1;
            TempData[MyGuid + "_TList6"] = model[0].TList6 = md1;
            TempData[MyGuid + "_TList7"] = model[0].TList7 = md1;
            TempData[MyGuid + "_TList8"] = model[0].TList8 = md1;
            TempData[MyGuid + "_TList9"] = model[0].TList9 = md1;
            model[0].SelectedItems1 = new string[] { "" };
            model[0].SelectedItems2 = new string[] { "" };
            model[0].SelectedItems3 = new string[] { "" };
            model[0].SelectedItems4 = new string[] { "" };
            model[0].SelectedItems5 = new string[] { "" };
            model[0].SelectedItems6 = new string[] { "" };
            model[0].SelectedItems7 = new string[] { "" };
            model[0].SelectedItems8 = new string[] { "" };
            model[0].SelectedItems9 = new string[] { "" };




            model[0].Col9 = "COMPANY PROFILE";

            model[0].Col10 = "company_profile";
            model[0].Col52 = "Company_Unit_Profile";
            model[0].Col53 = "com_year";
            model[0].Col54 = "add_academic_year";
            model[0].Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            model[0].Col12 = "CP";
            model[0].Col13 = "Save";
            model[0].Col100 = "Save & New";
            model[0].Col121 = "S";
            model[0].Col122 = "<u>S</u>ave";
            model[0].Col123 = "Save & Ne<u>w</u>";


            if (model[0].Col55 == null) ViewBag.scripCall = ViewBag.scripCall + "initialize(28.477450,77.034138);";
            else ViewBag.scripCall = ViewBag.scripCall + "initialize(" + model[0].Col55 + ");";


            DataTable dtb = (DataTable)sgen.GetSession(MyGuid, "URIGHTS");
            try
            {
                dtb = dtb.Select("m_id='" + model[0].Col14 + "'").CopyToDataTable();
                if (dtb.Rows.Count > 0)
                {
                    model[0].Col133 = dtb.Rows[0]["btnnew"].ToString();
                    model[0].Col134 = dtb.Rows[0]["btnedit"].ToString();
                    model[0].Col135 = dtb.Rows[0]["btnview"].ToString();
                }
            }
            catch (Exception ex) { }



            return View(model);
        }
        public List<Tmodelmain> new_client_profile(List<Tmodelmain> model)
        {
            try
            {
                model[0].Col46 = "1";
                model[0].Col38 = "";
                model[0].Col39 = "";

                sgen.SetSession(MyGuid, "EDMODE", "N");
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
                ViewBag.vsavenew = "";
                ViewBag.scripCall = "enableForm();";

                mq = "select max(to_number(Company_Profile_Id)) as auto_genid from " + model[0].Col10 + " where type='" + model[0].Col12 + "'";
                model[0].Col50 = sgen.genNo(userCode, mq, 3, "auto_genid");

                mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where type='" + model[0].Col12 + "'";
                vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");


                model[0].Col16 = vch_num.Trim();
                List<SelectListItem> mod1 = new List<SelectListItem>();
                List<SelectListItem> mod2 = new List<SelectListItem>();
                List<SelectListItem> mod3 = new List<SelectListItem>();
                List<SelectListItem> mod4 = new List<SelectListItem>();
                List<SelectListItem> mod5 = new List<SelectListItem>();
                List<SelectListItem> mod6 = new List<SelectListItem>();
                List<SelectListItem> mod7 = new List<SelectListItem>();

                if (model[0].Col133 == "N")
                {
                    ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for new " + model[0].Col9 + ", please contact your admin', 2);";
                    return model;
                }
                if (model[0].Col55 == null) ViewBag.scripCall = ViewBag.scripCall + "initialize(28.477450,77.034138);";
                else ViewBag.scripCall = ViewBag.scripCall + "initialize(" + model[0].Col55 + ");";

                #region client_type 1

                mod1.Add(new SelectListItem { Text = "Educational Inst.", Value = "Educational Inst." });
                mod1.Add(new SelectListItem { Text = "Corporate", Value = "Corporate" });
                TempData[MyGuid + "_Tlist1"] = model[0].TList1 = mod1;
                #endregion

                #region  board 2
                TempData[MyGuid + "_Tlist2"] = model[0].TList2 = mod2 = cmd_Fun.board(userCode, unitid_mst);
                #endregion

                #region  comp_client_type3
                TempData[MyGuid + "_Tlist3"] = model[0].TList3 = mod3 = cmd_Fun.comp_type(userCode);
                #endregion

                #region  nature of activity 4
                string defval = "";
                TempData[MyGuid + "_Tlist4"] = model[0].TList4 = mod4 = cmd_Fun.natact(userCode, unitid_mst, out defval);
                #endregion

                #region date format 5
                mod5.Add(new SelectListItem { Text = "dd/MM/yyyy", Value = "dd/MM/yyyy" });
                mod5.Add(new SelectListItem { Text = "dd/MMM/yyyy", Value = "dd/MMM/yyyy" });
                mod5.Add(new SelectListItem { Text = "dd/MMMM/yyyy", Value = "dd/MMMM/yyyy" });
                mod5.Add(new SelectListItem { Text = "MM/dd/yyyy", Value = "MM/dd/yyyy" });
                mod5.Add(new SelectListItem { Text = "MMM/dd/yyyy", Value = "MMM/dd/yyyy" });
                mod5.Add(new SelectListItem { Text = "MMMM/dd/yyyy", Value = "MMMM/dd/yyyy" });
                mod5.Add(new SelectListItem { Text = "yyyy/dd/MM", Value = "yyyy/dd/MM" });
                mod5.Add(new SelectListItem { Text = "yyyy/dd/MMM", Value = "yyyy/dd/MMM" });
                mod5.Add(new SelectListItem { Text = "yyyy/dd/MMMM", Value = "yyyy/dd/MMMM" });
                mod5.Add(new SelectListItem { Text = "yyyy/MM/dd", Value = "yyyy/MM/dd" });
                mod5.Add(new SelectListItem { Text = "yyyy/MMM/dd", Value = "yyyy/MMM/dd" });
                mod5.Add(new SelectListItem { Text = "yyyy/MMMM/dd", Value = "yyyy/MMMM/dd" });
                mod5.Add(new SelectListItem { Text = "yyyy/MMMM/dd", Value = "yyyy/MMMM/dd" });

                mod5.Add(new SelectListItem { Text = "dd-MM-yyyy", Value = "dd-MM-yyyy" });
                mod5.Add(new SelectListItem { Text = "dd-MMM-yyyy", Value = "dd-MMM-yyyy" });
                mod5.Add(new SelectListItem { Text = "dd-MMMM-yyyy", Value = "dd-MMMM-yyyy" });
                mod5.Add(new SelectListItem { Text = "MM-dd-yyyy", Value = "MM-dd-yyyy" });
                mod5.Add(new SelectListItem { Text = "MMM-dd-yyyy", Value = "MMM-dd-yyyy" });
                mod5.Add(new SelectListItem { Text = "MMMM-dd-yyyy", Value = "MMMM-dd-yyyy" });
                mod5.Add(new SelectListItem { Text = "yyyy-dd-MM", Value = "yyyy-dd-MM" });
                mod5.Add(new SelectListItem { Text = "yyyy-dd-MMM", Value = "yyyy-dd-MMM" });
                mod5.Add(new SelectListItem { Text = "yyyy-dd-MMMM", Value = "yyyy-dd-MMMM" });
                mod5.Add(new SelectListItem { Text = "yyyy-MM-dd", Value = "yyyy-MM-dd" });
                mod5.Add(new SelectListItem { Text = "yyyy-MMM-dd", Value = "yyyy-MMM-dd" });
                mod5.Add(new SelectListItem { Text = "yyyy-MMMM-dd", Value = "yyyy-MMMM-dd" });
                mod5.Add(new SelectListItem { Text = "yyyy-MMMM-dd", Value = "yyyy-MMMM-dd" });

                mod5.Add(new SelectListItem { Text = "dd.MM.yyyy", Value = "dd.MM.yyyy" });
                mod5.Add(new SelectListItem { Text = "dd.MMM.yyyy", Value = "dd.MMM.yyyy" });
                mod5.Add(new SelectListItem { Text = "dd.MMMM.yyyy", Value = "dd.MMMM.yyyy" });
                mod5.Add(new SelectListItem { Text = "MM.dd.yyyy", Value = "MM.dd.yyyy" });
                mod5.Add(new SelectListItem { Text = "MMM.dd.yyyy", Value = "MMM.dd.yyyy" });
                mod5.Add(new SelectListItem { Text = "MMMM.dd.yyyy", Value = "MMMM.dd.yyyy" });
                mod5.Add(new SelectListItem { Text = "yyyy.dd.MM", Value = "yyyy.dd.MM" });
                mod5.Add(new SelectListItem { Text = "yyyy.dd.MMM", Value = "yyyy.dd.MMM" });
                mod5.Add(new SelectListItem { Text = "yyyy.dd.MMMM", Value = "yyyy.dd.MMMM" });
                mod5.Add(new SelectListItem { Text = "yyyy.MM.dd", Value = "yyyy.MM.dd" });
                mod5.Add(new SelectListItem { Text = "yyyy.MMM.dd", Value = "yyyy.MMM.dd" });
                mod5.Add(new SelectListItem { Text = "yyyy.MMMM.dd", Value = "yyyy.MMMM.dd" });
                mod5.Add(new SelectListItem { Text = "yyyy.MMMM.dd", Value = "yyyy.MMMM.dd" });
                TempData[MyGuid + "_Tlist5"] = model[0].TList5 = mod5;
                #endregion

                #region Time format 6
                mod6.Add(new SelectListItem { Text = "H:mm:ss", Value = "H:mm:ss" });
                mod6.Add(new SelectListItem { Text = "h:mm:ss tt", Value = "h:mm:ss tt" });
                mod6.Add(new SelectListItem { Text = "H:mm", Value = "H:mm" });
                mod6.Add(new SelectListItem { Text = "h:mm tt", Value = "h:mm tt" });
                TempData[MyGuid + "_Tlist6"] = model[0].TList6 = mod6;

                #endregion

                #region  timezone 7

                TempData[MyGuid + "_Tlist7"] = model[0].TList7 = mod7 = cmd_Fun.timezone(userCode);
                #endregion




            }
            catch (Exception ex) { }
            return model;
        }
        [HttpPost]
        public ActionResult client_profile(List<Tmodelmain> model, string command, string mid, HttpPostedFileBase upd1)
        {
            FillMst(model[0].Col15);


            #region
            var tm = model.FirstOrDefault();
            List<SelectListItem> mod1 = (List<SelectListItem>)TempData[MyGuid + "_Tlist1"];
            List<SelectListItem> mod2 = (List<SelectListItem>)TempData[MyGuid + "_Tlist2"];
            List<SelectListItem> mod3 = (List<SelectListItem>)TempData[MyGuid + "_Tlist3"];
            List<SelectListItem> mod4 = (List<SelectListItem>)TempData[MyGuid + "_Tlist4"];
            List<SelectListItem> mod5 = (List<SelectListItem>)TempData[MyGuid + "_Tlist5"];
            List<SelectListItem> mod6 = (List<SelectListItem>)TempData[MyGuid + "_Tlist6"];
            List<SelectListItem> mod7 = (List<SelectListItem>)TempData[MyGuid + "_Tlist7"];

            TempData[MyGuid + "_Tlist1"] = model[0].TList1 = mod1;
            TempData[MyGuid + "_Tlist2"] = model[0].TList2 = mod2;
            TempData[MyGuid + "_Tlist3"] = model[0].TList3 = mod3;
            TempData[MyGuid + "_Tlist4"] = model[0].TList4 = mod4;
            TempData[MyGuid + "_Tlist5"] = model[0].TList5 = mod5;
            TempData[MyGuid + "_Tlist6"] = model[0].TList6 = mod6;
            TempData[MyGuid + "_Tlist7"] = model[0].TList7 = mod7;

            if (model[0].SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
            if (model[0].SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
            if (model[0].SelectedItems3 == null) model[0].SelectedItems3 = new string[] { "" };
            if (model[0].SelectedItems4 == null) model[0].SelectedItems4 = new string[] { "" };
            if (model[0].SelectedItems5 == null) model[0].SelectedItems5 = new string[] { "" };
            if (model[0].SelectedItems6 == null) model[0].SelectedItems6 = new string[] { "" };
            if (model[0].SelectedItems7 == null) model[0].SelectedItems7 = new string[] { "" };

            #endregion
            if (command == "New")
            {
                model = new_client_profile(model);
            }
            else if (command == "Cancel")
            {
                return CancelFun(model);

            }
            else if (command == "Callback")
            {
                if (model[0].Col134 == "N" && btnval.Trim().Equals("EDIT"))
                {
                    ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for Edit " + model[0].Col9 + ", please contact your admin', 2);disableForm();";
                    return View(model);
                }
                if (sgen.GetSession(MyGuid, "delid") != null) { btnval = "FDEL"; sgen.SetSession(MyGuid, "btnval", btnval); }
                else if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();

                model = StartCallback(model);
                if (model[0].Col55 == null) ViewBag.scripCall = ViewBag.scripCall + "initialize(28.477450,77.034138);";
                else ViewBag.scripCall = ViewBag.scripCall + "initialize(" + model[0].Col55 + ");";

            }
            else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
            {
                try
                {
                    if (model[0].SelectedItems1.FirstOrDefault().Equals("Educational Inst."))
                    {

                        //if (model[0].Col36 == null)
                        //{
                        //    ViewBag.scripCall = "showmsgJS(1, 'Please Enter From Academic Year', 2);";
                        //    ViewBag.vnew = "";
                        //    ViewBag.vedit = "";
                        //    ViewBag.vsave = "disabled='disabled'";
                        //    ViewBag.vsavenew = "disabled='disabled'"; goto END;
                        //}
                        //if (model[0].Col37 == null)
                        //{
                        //    ViewBag.scripCall = "showmsgJS(1, 'Please Enter To Academic Year', 2);";
                        //    ViewBag.vnew = "";
                        //    ViewBag.vedit = "";
                        //    ViewBag.vsave = "disabled='disabled'";
                        //    ViewBag.vsavenew = "disabled='disabled'"; goto END;
                        //}
                        if (model[0].SelectedItems2.FirstOrDefault().Equals(""))
                        {
                            ViewBag.scripCall = "showmsgJS(1, 'Please Select Board', 2);";
                            ViewBag.vnew = "disabled='disabled'";
                            ViewBag.vedit = "disabled='disabled'";
                            ViewBag.vsave = "";
                            ViewBag.vsavenew = "";
                            goto END;
                        }
                    }
                    bool res = false, res1 = false, res2 = false, ok = false;
                    type = model[0].Col12;
                    string currdate = sgen.server_datetime(userCode);
                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("N"))
                    {
                        comp_count = Convert.ToInt32(sgen.getstring(userCode, "select Count(company_profile_id) as comp_count from " + model[0].Col10 + " where type='" + model[0].Col12 + "'"));
                        cust_count = Convert.ToInt32(controls_mst.Split(',')[1]);
                        if (comp_count >= cust_count)
                        {
                            ViewBag.vnew = "disabled='disabled'";
                            ViewBag.vedit = "disabled='disabled'";
                            ViewBag.vsave = "";
                            ViewBag.vsavenew = "";
                            ViewBag.scripCall = "showmsgJS(1, 'you cannot create a new company, your no. of company is limited', 2);";

                            goto END;
                        }
                        unit_count = Convert.ToInt32(sgen.getstring(userCode, "select Count(cup_id) as unit_count from company_unit_profile"));
                        custunit_count = Convert.ToInt32(controls_mst.Split(',')[2]);
                        if (unit_count >= custunit_count)
                        {
                            ViewBag.vnew = "disabled='disabled'";
                            ViewBag.vedit = "disabled='disabled'";
                            ViewBag.vsave = "";
                            ViewBag.vsavenew = "";
                            ViewBag.scripCall = "showmsgJS(1, 'you cannot create a new company, your no. of unit is limited', 2);";

                            goto END;

                        }
                    }
                    Boolean status = true;
                    if (model[0].Col46.Trim() == "0")
                    {
                        type = "DD" + type;
                        status = false;
                    }
                    else status = true;

                    DataTable dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10);

                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        mq1 = " and vch_num<>'" + model[0].Col16 + "'";
                        isedit = true;
                        vch_num = model[0].Col16;
                        fileName1 = model[0].Col57;
                        encpath1 = model[0].Col58;
                    }
                    else
                    {
                        mq = "select max(to_number(Company_Profile_Id)) as auto_genid from " + model[0].Col10 + " where type='" + type + "'";
                        model[0].Col50 = sgen.genNo(userCode, mq, 3, "auto_genid");
                        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where type='" + type + "'";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");

                        isedit = false;
                    }
                    #region attachment
                    DataTable dtfile = new DataTable();
                    dtfile = cmd_Fun.GetStructure(userCode, "file_tab");
                    string serverpath = Server.MapPath("~/Uploads/" + cg_com_name.Replace(" ", "") + "/");
                    try
                    {
                        fileName1 = "";
                        HttpPostedFileBase file1 = upd1;
                        ctype1 = file1.ContentType;
                        fileName1 = file1.FileName;
                        path1 = clientid_mst + unitid_mst + userid_mst + currdate + "_" + fileName1;
                        encpath1 = sgen.Convert_Stringto64(path1).ToString();
                        finalpath1 = serverpath + encpath1;
                        file1.SaveAs(finalpath1);

                    }
                    catch (Exception ex) { }

                    #endregion
                    #region dt compnay

                    DataRow dr = dtstr.NewRow();
                    dr["vch_num"] = vch_num.Trim();
                    dr["vch_date"] = currdate;
                    dr["type"] = type.Trim();
                    dr["latlng"] = model[0].Col55;
                    dr["addr"] = model[0].Col21;
                    dr["Company_Profile_Id"] = model[0].Col50;
                    dr["Company_Name"] = model[0].Col17;
                    dr["Company_Address"] = model[0].Col21 + "$" + model[0].Col22 + "$" + model[0].Col23;

                    try
                    {
                        DataTable dtadd = new DataTable();
                        mq2 = "select * from country_state where rec_id='" + model[0].Col51 + "'";
                        dtadd = sgen.getdata(userCode, mq2);
                        if (dtadd.Rows.Count > 0)
                        {
                            dr["Company_Country"] = dtadd.Rows[0]["alpha_2"].ToString().Trim();
                            dr["Company_State"] = dtadd.Rows[0]["state_gst_code"].ToString().Trim();
                            dr["Company_City"] = dtadd.Rows[0]["city_name"].ToString().Trim();
                        }
                    }
                    catch (Exception ex) { }


                    dr["Company_Pincode"] = model[0].Col24;
                    dr["com_pan_no"] = model[0].Col25;
                    dr["Company_Cin_No"] = model[0].Col26;
                    dr["logo_name"] = fileName1;
                    dr["logo_path"] = encpath1;
                    try
                    {
                        if (!model[0].Col27.Equals(null)) { dr["Company_gstin_No"] = model[0].Col27.ToUpper().Trim(); }
                    }
                    catch
                    {
                        dr["Company_gstin_No"] = model[0].Col27;
                    }

                    dr["Company_Type_Of_Company"] = model[0].SelectedItems3.FirstOrDefault();
                    dr["Company_Nature_Of_Activity"] = model[0].SelectedItems4.FirstOrDefault();
                    try
                    {
                        if (!model[0].Col29.Equals(null)) dr["Company_No_Of_Employee"] = Convert.ToInt32(model[0].Col29);
                    }
                    catch
                    {
                        dr["Company_No_Of_Employee"] = 0;
                    }
                    dr["Company_Email_Id"] = model[0].Col30;
                    try
                    {
                        if (model[0].Col31 != null) dr["Company_Contact_No"] = Convert.ToInt64(model[0].Col31);
                    }
                    catch { dr["Company_Contact_No"] = 0; }

                    dr["Company_Alternate_Contact_No"] = model[0].Col32;
                    dr["Company_Website"] = model[0].Col33;
                    dr["Company_Contact_Person_Name"] = model[0].Col42;
                    dr["Company_Person_Designation"] = model[0].Col43;
                    dr["Company_Person_Email"] = model[0].Col44;
                    try
                    {
                        if (!model[0].Col44.Equals(null)) { dr["Company_Person_contact_No"] = Convert.ToInt64(model[0].Col44); }
                    }
                    catch
                    {
                        dr["Company_Person_contact_No"] = 0;
                    }
                    dr["Company_Status"] = status;
                    dr["com_email"] = model[0].Col38;
                    dr["com_password"] = EncryptDecrypt.Encrypt(model[0].Col39);
                    try
                    {
                        if (!model[0].Col40.Equals(null)) dr["com_port"] = Convert.ToInt32(model[0].Col40);
                    }
                    catch
                    {
                        dr["com_port"] = 0;
                    }
                    dr["com_smtp"] = model[0].Col41;
                    dr["dateformat"] = model[0].SelectedItems5.FirstOrDefault();
                    dr["timeformat"] = model[0].SelectedItems6.FirstOrDefault();
                    dr["timezone"] = model[0].SelectedItems7.FirstOrDefault();
                    dr["client_type"] = model[0].SelectedItems1.FirstOrDefault();
                    dr["edu_board"] = model[0].SelectedItems2.FirstOrDefault();

                    if (isedit)
                    {
                        dr["Company_Ent_By"] = model[0].Col3;
                        dr["Company_Ent_Date"] = model[0].Col4;
                        dr["rec_id"] = model[0].Col7;
                    }
                    else
                    {
                        dr["Company_Ent_By"] = userid_mst;
                        dr["Company_Ent_Date"] = currdate;
                        dr["rec_id"] = "0";
                    }

                    dr["Company_Edit_By"] = userid_mst;
                    dr["Company_Edit_Date"] = currdate;
                    dtstr.Rows.Add(dr);

                    #endregion

                    Satransaction sat1 = new Satransaction(userCode, MyGuid);
                    Satransaction sat2 = new Satransaction(userCode, MyGuid);
                    //Satransaction sat = new Satransaction(userCode, MyGuid);
                    Satransaction sat4 = new Satransaction(userCode, MyGuid);

                    ok = sgen.Update_data_fast1_uncommit(userCode, dtstr, model[0].Col10, model[0].Col8, isedit, sat1);

                    if (ok)
                    {
                        if (!isedit)
                        {
                            #region unit
                            DataTable dtu = new DataTable();
                            dtu = sgen.getdata(userCode, "select * from " + model[0].Col52 + " WHERE 1=2");

                            mq1 = sgen.genNo(userCode, "Select max(to_number(Unit_Code)) as Gen_Unit_Code from " + model[0].Col52 + " where Company_Profile_Id="
                                + model[0].Col50, 3, "Gen_Unit_Code");

                            mqm = sgen.genNo(userCode, "Select max(to_number(vch_num)) as vch_num from " + model[0].Col52 + " where Company_Profile_Id="
                               + model[0].Col50, 6, "vch_num");
                            CUP_id = model[0].Col50 + mq1;


                            #region  dtunit

                            DataRow dru = dtu.NewRow();
                            dru["vch_num"] = mqm.Trim();
                            dru["vch_date"] = currdate.Trim();
                            dru["type"] = "";
                            dru["latlng"] = model[0].Col55;
                            dru["addr"] = model[0].Col21;
                            dru["CUP_Id"] = CUP_id.Trim();
                            dru["Company_Profile_Id"] = model[0].Col50.Trim();
                            dru["Company_Code"] = "";
                            dru["Unit_Code"] = mq1.Trim();

                            try
                            {
                                DataTable dtadd = new DataTable();
                                mq2 = "select * from country_state where rec_id='" + model[0].Col51 + "'";
                                dtadd = sgen.getdata(userCode, mq2);
                                if (dtadd.Rows.Count > 0)
                                {
                                    dr["Unit_Country"] = dtadd.Rows[0]["alpha_2"].ToString().Trim();
                                    dr["Unit_State"] = dtadd.Rows[0]["state_gst_code"].ToString().Trim();
                                    dr["Unit_City"] = dtadd.Rows[0]["city_name"].ToString().Trim();
                                }
                            }
                            catch (Exception ex) { }

                            dru["Unit_Address"] = model[0].Col21 + "$" + model[0].Col22 + "$" + model[0].Col23;
                            dru["Unit_Pincode"] = model[0].Col24;
                            try
                            {
                                if (!model[0].Col27.Equals(null)) { dru["Unit_GSTIN_No"] = model[0].Col27.ToUpper().Trim(); }
                            }
                            catch { }

                            dru["Unit_Email"] = model[0].Col28;
                            try
                            {
                                if (model[0].Col31 != null) dru["Unit_Contact_No"] = Convert.ToInt64(model[0].Col31);
                            }
                            catch { dru["Unit_Contact_No"] = "0"; }
                            dru["Unit_Alternate_Contact_No"] = model[0].Col32;
                            dru["Unit_website"] = model[0].Col33;
                            dru["Unit_Contact_Person_Name"] = model[0].Col42;
                            dru["Unit_Person_Designation"] = model[0].Col43;
                            dru["Unit_Person_Email_Id"] = model[0].Col44;
                            try
                            {
                                if (!model[0].Col45.Equals(null)) dru["Unit_Person_Contact_No"] = Convert.ToInt64(model[0].Col45);
                            }
                            catch
                            {
                                dru["Unit_Person_Contact_No"] = "0";
                            }
                            dru["Unit_Name"] = "Unit";
                            dru["Unit_Status"] = status;
                            dru["Unit_Ent_By"] = userid_mst;
                            dru["Unit_Ent_Date"] = currdate;
                            dru["rec_id"] = "0";
                            dru["Unit_Edit_By"] = userid_mst;
                            dru["Unit_Edit_Date"] = currdate;
                            dru["edu_board"] = model[0].SelectedItems2.FirstOrDefault();
                            dtu.Rows.Add(dru);

                            #endregion

                            res = sgen.Update_data_fast1_uncommit(userCode, dtu, model[0].Col52, "", false, sat2);

                            #endregion

                            if (res)
                            {
                                #region fy

                                //DataTable dtfy = new DataTable();
                                //dtfy = sgen.getdata(userCode, "select * from " + model[0].Col53 + " WHERE 1=2");
                                //mq2 = sgen.genNo(userCode, "Select max(to_number(vch_num)) as Gen_Unit_Code from " + model[0].Col53 + " where " +
                                //    "client_id='" + model[0].Col50 + "' and client_unit_id='" + CUP_id + "'", 6, "Gen_Unit_Code");

                                //string fyf = DateTime.ParseExact(model[0].Col34, sgen.Getdateformat(), CultureInfo.InvariantCulture).ToString("yyyy");
                                //string com_code = userCode + "-" + fyf;
                                //string fyt = DateTime.ParseExact(model[0].Col35, sgen.Getdateformat(), CultureInfo.InvariantCulture).ToString("yyyy");

                                //#region  dt fy

                                //DataRow drf = dtfy.NewRow();
                                //drf["vch_num"] = mq2.Trim();
                                //drf["vch_date"] = currdate.Trim();
                                //drf["type"] = "";
                                //drf["com_code"] = com_code;
                                //drf["com_year"] = fyf + " " + fyt;
                                //drf["year_from"] = sgen.Make_date_S(model[0].Col34);
                                //drf["year_to"] = sgen.Make_date_S(model[0].Col35);
                                //drf["ent_by"] = userid_mst;
                                //drf["ent_date"] = currdate.Trim();
                                //drf["rec_id"] = "0";
                                //drf["edit_By"] = userid_mst;
                                //drf["edit_Date"] = currdate.Trim();
                                //drf["client_id"] = model[0].Col50;
                                //drf["client_unit_id"] = CUP_id.Trim();
                                //dtfy.Rows.Add(drf);

                                //#endregion

                                //res1 = sgen.Update_data_uncommit(userCode, dtfy, model[0].Col53, "", false, out sat3);

                                #endregion

                                //if (res1)
                                //{
                                //    if (model[0].SelectedItems1.FirstOrDefault().Equals("Educational Inst."))
                                //    {
                                #region ac
                                if (model[0].Col36 == "" || model[0].Col36 == null)
                                {
                                    ViewBag.vnew = "disabled='disabled'";
                                    ViewBag.vedit = "disabled='disabled'";
                                    ViewBag.vsave = "";
                                    ViewBag.vsavenew = "";
                                    ViewBag.scripCall = "showmsgJS(1, 'Please select Year of Operation', 2);";
                                    return View(model);
                                }
                                if (model[0].Col37 == "" || model[0].Col37 == null)
                                {
                                    ViewBag.vnew = "disabled='disabled'";
                                    ViewBag.vedit = "disabled='disabled'";
                                    ViewBag.vsave = "";
                                    ViewBag.vsavenew = "";
                                    ViewBag.scripCall = "showmsgJS(1, 'Please select Year of Operation', 2);";
                                    return View(model);
                                }
                                DataTable dtac = new DataTable();
                                dtac = sgen.getdata(userCode, "select * from " + model[0].Col54 + " WHERE 1=2");
                                string acf = DateTime.ParseExact(model[0].Col36, sgen.Getdateformat(), CultureInfo.InvariantCulture).ToString("yyyy");
                                string act = DateTime.ParseExact(model[0].Col37, sgen.Getdateformat(), CultureInfo.InvariantCulture).ToString("yyyy");
                                string acfm = DateTime.ParseExact(model[0].Col36, sgen.Getdateformat(), CultureInfo.InvariantCulture).ToString("MMMM");
                                string actm = DateTime.ParseExact(model[0].Col37, sgen.Getdateformat(), CultureInfo.InvariantCulture).ToString("MMMM");
                                int acfm_id = DateTime.ParseExact(model[0].Col36, sgen.Getdateformat(), CultureInfo.InvariantCulture).Month;
                                int actm_id = DateTime.ParseExact(model[0].Col37, sgen.Getdateformat(), CultureInfo.InvariantCulture).Month;
                                string vchac = "", acid = "";
                                try
                                {
                                    type1 = "ACY";
                                    vchac = sgen.genNo(userCode, "Select max(to_number(vch_num)) as maxno from " + model[0].Col54 + " where " +
                                       "type='" + type1 + "' and client_id='" + model[0].Col50 + "' and client_unit_id='" + CUP_id + "'", 6, "maxno");
                                    acid = sgen.genNo(userCode, "Select max(to_number(academic_year_id)) as maxno from " + model[0].Col54 + " " +
                                       "where type='ACY' and client_id='" + model[0].Col50 + "' and client_unit_id='" + CUP_id + "'", 3, "maxno");
                                }
                                catch { }

                                #region  dt ac

                                DataRow dra = dtac.NewRow();
                                dra["vch_num"] = vchac.Trim();
                                dra["vch_date"] = currdate.Trim();
                                dra["type"] = type1;
                                dra["academic_year_id"] = acid;
                                dra["academic_year"] = acf + " " + act;
                                dra["fmonthid"] = acfm_id;
                                dra["tmonthid"] = actm_id;
                                dra["from_month"] = acfm;
                                dra["to_month"] = actm;
                                dra["from_date"] = sgen.Make_date_S(model[0].Col36);
                                dra["to_date"] = sgen.Make_date_S(model[0].Col37);
                                dra["ent_by"] = userid_mst;
                                dra["ent_date"] = currdate.Trim();
                                dra["rec_id"] = "0";
                                dra["edit_By"] = userid_mst;
                                dra["edit_Date"] = currdate.Trim();
                                dra["client_id"] = model[0].Col50;
                                dra["client_unit_id"] = CUP_id.Trim();
                                dtac.Rows.Add(dra);

                                #endregion

                                res2 = sgen.Update_data_fast1_uncommit(userCode, dtac, model[0].Col54, "", false, sat4);
                                #endregion

                                if (res2)
                                {
                                    mq0 = "0";
                                    mq = "INSERT INTO " + userCode + ".MASTER_SETTING (REC_ID, MASTER_ID, MASTER_NAME, MASTER_TYPE, MASTER_ENTBY, MASTER_ENTDATE, MASTER_EDITBY," +
                                        "MASTER_EDITDATE, MASTER_STATUS, ISROLLNOREGWISE, ISREGNOCHANGE, ISRECEIPTNOCHANGE, ISROLLNOCHANGE, ISSCHOOLBASED, ISCLASSBASED," +
                                        "ISSECTIONBASED, ISADMISSION_WISE, ISALPHABETIC_WISE, ISREGFEESBEFOREADM, CLASSID, SECTIONID, SECTION_STRENGTH, SUBJECTID, SUBJECT_STRENGTH," +
                                        "OPTIONAL_SUBJECT, TEACHER_CATEGORY, CLIENT_NAME, GROUP_NAME, UNIT_NAME, CONT_PERSON_NAME, CONT_PERSON_NUMBER, CONT_PERSON_ALTNUMBER," +
                                        "CONT_PERSON_EMAIL, GROUP_REFRENCE_NUMBER, GROUP_ID, CLIENT_GSTIN, HEADING, CONTENT, DATE_OF_ISSUE, AUTH_SIGN_FILENAME, AUTH_SIGN_FILEPATH," +
                                        "CREATED_DATE, VCH_NUM, VCH_DATE, TYPE, client_id, client_unit_id, QUALIFICATION_TYPE, SECTIONTYPE, FEMALERATIO, MALERATIO, STATUS, INACTIVE_DATE) " +
                                        "select rec_id, MASTER_ID, MASTER_NAME, MASTER_TYPE, MASTER_ENTBY, MASTER_ENTDATE, MASTER_EDITBY, MASTER_EDITDATE,MASTER_STATUS," +
                                        "ISROLLNOREGWISE, ISREGNOCHANGE, ISRECEIPTNOCHANGE, ISROLLNOCHANGE, ISSCHOOLBASED, ISCLASSBASED, ISSECTIONBASED, ISADMISSION_WISE," +
                                        "ISALPHABETIC_WISE, ISREGFEESBEFOREADM, CLASSID, SECTIONID, SECTION_STRENGTH, SUBJECTID, SUBJECT_STRENGTH, OPTIONAL_SUBJECT, TEACHER_CATEGORY," +
                                        "CLIENT_NAME, GROUP_NAME, UNIT_NAME, CONT_PERSON_NAME, CONT_PERSON_NUMBER, CONT_PERSON_ALTNUMBER, CONT_PERSON_EMAIL, GROUP_REFRENCE_NUMBER, GROUP_ID," +
                                        "CLIENT_GSTIN, HEADING, CONTENT, DATE_OF_ISSUE, AUTH_SIGN_FILENAME, AUTH_SIGN_FILEPATH, CREATED_DATE, VCH_NUM, VCH_DATE, TYPE, '" + model[0].Col50 + "'," +
                                        "'" + CUP_id.Trim() + "', QUALIFICATION_TYPE, SECTIONTYPE, FEMALERATIO, MALERATIO, STATUS, INACTIVE_DATE from " + userCode + ".master_setting where " +
                                        "type in ('KMR', 'DDKMR', 'KIS', 'DDKIS', 'KSR', 'DDKSR', 'KPO', 'DDKPO', 'KRQ', 'DDKRQ', 'KPD', 'DDKPD','KRG','DDKRG','CLS','DDCLS') and find_in_set(client_id,'001')= 1 " +
                                        "and find_in_set(client_unit_id, '001001') = 1";
                                    res = sgen.execute_cmd(userCode, mq);
                                    if (res) { mq0 = "1"; }
                                    else mq = "0";

                                    mq = "Insert into " + userCode + ".controls (REC_ID,TYPE,ID,NAME,PARAM1,PARAM2,PARAM3,PARAM4,CLIENT_ID,CLIENT_UNIT_ID,M_MODULE3,PARAM5,PARAM6,PARAM7,PARAM8,PARAM9,ID_DUP) " +
                                        "select REC_ID,TYPE,ID,NAME,PARAM1,PARAM2,PARAM3,PARAM4,'" + model[0].Col50 + "','" + CUP_id.Trim() + "',M_MODULE3,PARAM5,PARAM6,PARAM7,PARAM8,PARAM9,ID_DUP from " + userCode + ".controls " +
                                        "where client_id='001' and upper(param5)='VENDOR DETAIL'";
                                    res = sgen.execute_cmd(userCode, mq);
                                    if (res) { mq0 = mq0 + "!~!~!~!" + "2"; }
                                    else mq = "0";

                                    mq = "Insert into " + userCode + ".controls (REC_ID,TYPE,ID,NAME,PARAM1,PARAM2,PARAM3,PARAM4,CLIENT_ID,CLIENT_UNIT_ID,M_MODULE3,PARAM5,PARAM6,PARAM7,PARAM8,PARAM9,ID_DUP) " +
                                        "select REC_ID,TYPE,ID,NAME,PARAM1,PARAM2,PARAM3,PARAM4,'" + model[0].Col50 + "','" + CUP_id.Trim() + "',M_MODULE3,PARAM5,PARAM6,PARAM7,PARAM8,PARAM9,ID_DUP from " + userCode + ".controls " +
                                        "where client_id='001' and upper(param5)='CUSTOMER DETAIL'";
                                    res = sgen.execute_cmd(userCode, mq);
                                    if (res) { mq0 = mq0 + "!~!~!~!" + "3"; }
                                    else mq = "0";

                                    mq = "Insert into " + userCode + ".controls (REC_ID,TYPE,ID,NAME,PARAM1,PARAM2,PARAM3,PARAM4,CLIENT_ID,CLIENT_UNIT_ID,M_MODULE3,PARAM5,PARAM6,PARAM7,PARAM8,PARAM9,ID_DUP) " +
                                       "select REC_ID,TYPE,ID,NAME,PARAM1,PARAM2,PARAM3,PARAM4,'" + model[0].Col50 + "','" + CUP_id.Trim() + "',M_MODULE3,PARAM5,PARAM6,PARAM7,PARAM8,PARAM9,ID_DUP from " + userCode + ".controls " +
                                       "where client_id='001' and upper(param5)='PROSPECT DATA'";
                                    res = sgen.execute_cmd(userCode, mq);
                                    if (res) { mq0 = mq0 + "!~!~!~!" + "4"; }
                                    else mq = "0";

                                    mq = "Insert into " + userCode + ".reports (rec_id,vch_num,vch_date,rpt_name,type,cat_id,cat_name,menu_id,module_id,module,username,role,ent_by,ent_date,created_date,edit_by,edit_date,client_id,client_unit_id,com_id,rpt_newname,fun_name) " +
                                        "select rec_id,vch_num,vch_date,rpt_name,type,cat_id,cat_name,menu_id,module_id,module,username,role,ent_by,ent_date,created_date,edit_by,edit_date,'" + model[0].Col50 + "','" + CUP_id.Trim() + "'," +
                                        "com_id,rpt_newname,fun_name from " + userCode + ".reports where type = 'RPT' and client_id = '001' and client_unit_id = '001001'";
                                    res = sgen.execute_cmd(userCode, mq);
                                    if (res) { mq0 = mq0 + "!~!~!~!" + "5"; }
                                    else mq = "0";

                                    mq = "Insert into " + userCode + ".controls (REC_ID,TYPE,ID,NAME,PARAM1,PARAM2,PARAM3,PARAM4,CLIENT_ID,CLIENT_UNIT_ID,M_MODULE3,PARAM5,PARAM6,PARAM7,PARAM8,PARAM9,ID_DUP) " +
                                      "select REC_ID,TYPE,ID,NAME,PARAM1,PARAM2,PARAM3,PARAM4,'" + model[0].Col50 + "','" + CUP_id.Trim() + "',M_MODULE3,PARAM5,PARAM6,PARAM7,PARAM8,PARAM9,ID_DUP from " + userCode + ".controls " +
                                      "where client_id='001' and upper(param5)='SITE VISIT'";
                                    res = sgen.execute_cmd(userCode, mq);
                                    if (res) { mq0 = mq0 + "!~!~!~!" + "6"; }

                                    mq = "Insert into " + userCode + ".controls (REC_ID,TYPE,ID,NAME,PARAM1,PARAM2,PARAM3,PARAM4,CLIENT_ID,CLIENT_UNIT_ID,M_MODULE3,PARAM5,PARAM6,PARAM7,PARAM8,PARAM9,ID_DUP) " +
                                      "select REC_ID,TYPE,ID,NAME,PARAM1,PARAM2,PARAM3,PARAM4,'" + model[0].Col50 + "','" + CUP_id.Trim() + "',M_MODULE3,PARAM5,PARAM6,PARAM7,PARAM8,PARAM9,ID_DUP from " + userCode + ".controls " +
                                      "where client_id='001' and upper(param5)='LEAD MASTER'";
                                    res = sgen.execute_cmd(userCode, mq);

                                    mq = "Insert into " + userCode + ".controls (REC_ID,TYPE,ID,NAME,PARAM1,PARAM2,PARAM3,PARAM4,CLIENT_ID,CLIENT_UNIT_ID,M_MODULE3,PARAM5,PARAM6,PARAM7,PARAM8,PARAM9,ID_DUP) " +
                                     "select REC_ID,TYPE,ID,NAME,PARAM1,PARAM2,PARAM3,PARAM4,'" + model[0].Col50 + "','" + CUP_id.Trim() + "',M_MODULE3,PARAM5,PARAM6,PARAM7,PARAM8,PARAM9,ID_DUP from " + userCode + ".controls " +
                                     "where client_unit_id='001001' and upper(type)='SMS'";
                                    res = sgen.execute_cmd(userCode, mq);
                                    if (res) { mq0 = mq0 + "!~!~!~!" + "7"; }

                                    if (mq0.Split(new string[] { "!~!~!~!" }, StringSplitOptions.None)[0].Trim().Equals("0")) ViewBag.scripCall += "showmsgJS(1, 'Type not created for this Company & Unit', 2);";
                                    else if (mq0.Split(new string[] { "!~!~!~!" }, StringSplitOptions.None)[1].Trim().Equals("0")) ViewBag.scripCall += "showmsgJS(1, 'Vendor form controls not created for this Company', 2);";
                                    else if (mq0.Split(new string[] { "!~!~!~!" }, StringSplitOptions.None)[2].Trim().Equals("0")) ViewBag.scripCall += "showmsgJS(1, 'Customer form controls not created for this Company', 2);";
                                    else if (mq0.Split(new string[] { "!~!~!~!" }, StringSplitOptions.None)[3].Trim().Equals("0")) ViewBag.scripCall += "showmsgJS(1, 'Prospect form controls not created for this Company', 2);";
                                    else if (mq0.Split(new string[] { "!~!~!~!" }, StringSplitOptions.None)[4].Trim().Equals("0")) ViewBag.scripCall += "showmsgJS(1, 'Report Type not created for this Company', 2);";
                                    else if (mq0.Split(new string[] { "!~!~!~!" }, StringSplitOptions.None)[5].Trim().Equals("0")) ViewBag.scripCall += "showmsgJS(1, 'Site Visit form controls not created for this Company', 2);";
                                    else if (mq0.Split(new string[] { "!~!~!~!" }, StringSplitOptions.None)[6].Trim().Equals("0")) ViewBag.scripCall += "showmsgJS(1, 'Lead Master form controls not created for this Company', 2);";
                                }
                                else
                                {
                                    sat1.Rollback();
                                    sat2.Rollback();
                                    //sat3.Rollback();
                                    sat4.Rollback();
                                }
                            }
                            else
                            {
                                sat1.Rollback();
                                sat2.Rollback();
                                //sat3.Rollback();
                                sat4.Rollback();

                            }
                        }

                        sat1.Commit();
                        sat2.Commit();
                        //sat3.Commit();
                        sat4.Commit();



                        //if (isedit) sgen.showmsg(1, "Detail Updated", 1);
                        //else sgen.showmsg(1, "Detail Added", 1);

                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col13 = "Save",
                            Col100 = "Save & New",
                            Col121 = "S",
                            Col122 = "<u>S</u>ave",
                            Col123 = "Save & Ne<u>w</u>",
                            Col9 = tm.Col9,
                            Col10 = tm.Col10,
                            Col11 = tm.Col11,
                            Col12 = tm.Col12,
                            Col14 = tm.Col14,
                            Col15 = tm.Col15,
                            Col18 = tm.Col18,
                            Col20 = tm.Col20,
                            TList1 = mod1,
                            TList2 = mod2,
                            TList3 = mod3,
                            TList4 = mod4,
                            TList5 = mod5,
                            TList6 = mod6,
                            TList7 = mod7,
                            SelectedItems1 = new string[] { "" },
                            SelectedItems2 = new string[] { "" },
                            SelectedItems3 = new string[] { "" },
                            SelectedItems4 = new string[] { "" },
                            SelectedItems5 = new string[] { "" },
                            SelectedItems6 = new string[] { "" },
                            SelectedItems7 = new string[] { "" },


                        });
                        if (command.Trim().ToUpper().Equals("SAVE") || command.Trim().ToUpper().Equals("UPDATE"))
                        {
                            ViewBag.vnew = "";
                            ViewBag.vedit = "";
                            ViewBag.vsave = "disabled='disabled'";
                            ViewBag.vsavenew = "disabled='disabled'";
                            ViewBag.scripCall += "disableForm();";
                            //ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully'');disableForm();";
                        }
                        else if (command.Trim().ToUpper().Equals("SAVE & NEW") || command.Trim().ToUpper().Equals("UPDATE & NEW"))
                        {
                            try
                            {
                                model = new_client_profile(model);
                                // ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully'');";
                            }
                            catch (Exception ex) { }
                        }
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";


                    }
                    else
                    {
                        sat1.Rollback();
                        sat2.Rollback();
                        //sat3.Rollback();
                        sat4.Rollback();

                    }
                END:;
                }
                catch (Exception ex) { }
            }

            ModelState.Clear();
            return View(model);




        }
        #endregion

        #region unit profile
        public ActionResult unit_profile(string mid)
        {
            FillMst();

            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);

            List<SelectListItem> md1 = new List<SelectListItem>();

            TempData[MyGuid + "_TList1"] = model[0].TList1 = md1;
            TempData[MyGuid + "_TList2"] = model[0].TList2 = md1;
            TempData[MyGuid + "_TList3"] = model[0].TList3 = md1;
            TempData[MyGuid + "_TList4"] = model[0].TList4 = md1;
            TempData[MyGuid + "_TList5"] = model[0].TList5 = md1;
            TempData[MyGuid + "_TList6"] = model[0].TList6 = md1;
            TempData[MyGuid + "_TList7"] = model[0].TList7 = md1;

            model[0].SelectedItems1 = new string[] { "" };
            model[0].SelectedItems2 = new string[] { "" };
            model[0].SelectedItems3 = new string[] { "" };
            model[0].SelectedItems4 = new string[] { "" };
            model[0].SelectedItems5 = new string[] { "" };
            model[0].SelectedItems6 = new string[] { "" };
            model[0].SelectedItems7 = new string[] { "" };





            model[0].Col9 = "COMPANY UNIT PROFILE";

            model[0].Col10 = "company_unit_profile";
            model[0].Col52 = "com_year";
            model[0].Col53 = "add_academic_year";

            model[0].Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            model[0].Col12 = "CP";
            model[0].Col13 = "Save";
            model[0].Col100 = "Save & New";
            model[0].Col121 = "S";
            model[0].Col122 = "<u>S</u>ave";
            model[0].Col123 = "Save & Ne<u>w</u>";


            if (model[0].Col41 == null) ViewBag.scripCall = ViewBag.scripCall + "initialize(28.477450,77.034138);";
            else ViewBag.scripCall = ViewBag.scripCall + "initialize(" + model[0].Col41 + ");";


            DataTable dtb = (DataTable)sgen.GetSession(MyGuid, "URIGHTS");
            try
            {
                dtb = dtb.Select("m_id='" + model[0].Col14 + "'").CopyToDataTable();
                if (dtb.Rows.Count > 0)
                {
                    model[0].Col133 = dtb.Rows[0]["btnnew"].ToString();
                    model[0].Col134 = dtb.Rows[0]["btnedit"].ToString();
                    model[0].Col135 = dtb.Rows[0]["btnview"].ToString();
                }
            }
            catch (Exception ex) { }



            return View(model);
        }
        public List<Tmodelmain> new_unit_profile(List<Tmodelmain> model)
        {
            try
            {
                sgen.SetSession(MyGuid, "EDMODE", "N");
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
                ViewBag.vsavenew = "";
                ViewBag.scripCall = "enableForm();";
                model[0].Col40 = "1";
                mq = "select max(to_number(Company_Profile_Id)) as auto_genid from " + model[0].Col10 + " where type='" + model[0].Col12 + "'";
                model[0].Col50 = sgen.genNo(userCode, mq, 3, "auto_genid");
                mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where type='" + model[0].Col12 + "'";
                vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                model[0].Col16 = vch_num.Trim();
                List<SelectListItem> mod1 = new List<SelectListItem>();
                List<SelectListItem> mod2 = new List<SelectListItem>();
                List<SelectListItem> mod3 = new List<SelectListItem>();
                List<SelectListItem> mod4 = new List<SelectListItem>();
                List<SelectListItem> mod5 = new List<SelectListItem>();
                List<SelectListItem> mod6 = new List<SelectListItem>();
                List<SelectListItem> mod7 = new List<SelectListItem>();

                if (model[0].Col133 == "N")
                {
                    ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for new " + model[0].Col9 + ", please contact your admin', 2);";
                    return model;
                }
                #region company 1
                try
                {
                    string where = "";
                    //mq = sgen.seekval(userCode, "select client_id from user_details where rec_id='" + Convert.ToInt32(userid_mst) + "'", "client_id");
                    mq = sgen.seekval(userCode, "select client_id from user_details where vch_num='" + userid_mst + "'", "client_id");
                    if (!role_mst.ToUpper().Equals("OWNER")) where = " and company_profile_id in ('" + mq.Replace(",", "','") + "')";

                    DataTable dtcomp = new DataTable();
                    mq = "SELECT Company_Profile_Id, (company_name|| ' ('|| company_profile_id||')') as nameid FROM company_profile where type='CP' " + where + "";
                    dtcomp = sgen.getdata(userCode, mq);
                    if (dtcomp.Rows.Count > 0)
                    {

                        foreach (DataRow dr in dtcomp.Rows)
                        {
                            mod1.Add(new SelectListItem { Text = dr["nameid"].ToString(), Value = dr["company_profile_id"].ToString() });

                        }

                    }
                    TempData[MyGuid + "_Tlist1"] = model[0].TList1 = mod1;
                }
                catch (Exception ex)
                {
                    sgen.showmsg(1, ex.Message.ToString(), 0);
                }
                #endregion
                #region  board 2
                TempData[MyGuid + "_Tlist2"] = model[0].TList2 = mod2 = cmd_Fun.board(userCode, unitid_mst);
                #endregion
                #region  country3
                TempData[MyGuid + "_Tlist3"] = model[0].TList3 = mod3 = cmd_Fun.country(userCode, unitid_mst);
                #endregion
                #region  currency 6
                TempData[MyGuid + "_Tlist6"] = model[0].TList6 = mod6 = cmd_Fun.curname(userCode, unitid_mst);
                #endregion
                if (model[0].Col41 == null) ViewBag.scripCall = ViewBag.scripCall + "initialize(28.477450,77.034138);";
                else ViewBag.scripCall = ViewBag.scripCall + "initialize(" + model[0].Col41 + ");";

            }
            catch (Exception ex) { }
            return model;
        }
        [HttpPost]
        public ActionResult unit_profile(List<Tmodelmain> model, string command, string mid, HttpPostedFileBase upd1)
        {
            FillMst(model[0].Col15);
            #region
            var tm = model.FirstOrDefault();
            List<SelectListItem> mod1 = (List<SelectListItem>)TempData[MyGuid + "_Tlist1"];
            List<SelectListItem> mod2 = (List<SelectListItem>)TempData[MyGuid + "_Tlist2"];
            List<SelectListItem> mod3 = (List<SelectListItem>)TempData[MyGuid + "_Tlist3"];
            List<SelectListItem> mod4 = (List<SelectListItem>)TempData[MyGuid + "_Tlist4"];
            List<SelectListItem> mod5 = (List<SelectListItem>)TempData[MyGuid + "_Tlist5"];
            List<SelectListItem> mod6 = (List<SelectListItem>)TempData[MyGuid + "_Tlist6"];
            List<SelectListItem> mod7 = (List<SelectListItem>)TempData[MyGuid + "_Tlist7"];

            TempData[MyGuid + "_Tlist1"] = model[0].TList1 = mod1;
            TempData[MyGuid + "_Tlist2"] = model[0].TList2 = mod2;
            TempData[MyGuid + "_Tlist3"] = model[0].TList3 = mod3;
            TempData[MyGuid + "_Tlist4"] = model[0].TList4 = mod4;
            TempData[MyGuid + "_Tlist5"] = model[0].TList5 = mod5;
            TempData[MyGuid + "_Tlist6"] = model[0].TList6 = mod6;
            TempData[MyGuid + "_Tlist7"] = model[0].TList7 = mod7;

            if (model[0].SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
            if (model[0].SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
            if (model[0].SelectedItems3 == null) model[0].SelectedItems3 = new string[] { "" };
            if (model[0].SelectedItems4 == null) model[0].SelectedItems4 = new string[] { "" };
            if (model[0].SelectedItems5 == null) model[0].SelectedItems5 = new string[] { "" };
            if (model[0].SelectedItems6 == null) model[0].SelectedItems6 = new string[] { "" };
            if (model[0].SelectedItems7 == null) model[0].SelectedItems7 = new string[] { "" };

            #endregion
            if (command == "New")
            {
                model = new_unit_profile(model);
            }
            else if (command == "Company")
            {
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
                ViewBag.vsavenew = "";
                ViewBag.scripCall = "enableForm();";
                try
                {
                    mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where " +
                        "Company_Profile_Id='" + model[0].SelectedItems1.FirstOrDefault() + "'";
                    vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                    model[0].Col16 = vch_num.Trim();

                    mq1 = "Select max(to_number(Unit_Code)) as Gen_Unit_Code from " + model[0].Col10 + " where Company_Profile_Id='" +
                        model[0].SelectedItems1.FirstOrDefault() + "'";
                    model[0].Col17 = sgen.genNo(userCode, mq1, 3, "Gen_Unit_Code");


                    mq2 = "select client_type from company_profile where type='CP' and company_profile_id='" + model[0].SelectedItems1.FirstOrDefault() + "'";
                    mq = sgen.seekval(userCode, mq2, "client_type");
                    if (mq.Trim().Equals("Educational Inst."))
                    {
                        //ViewBag.scripcall += "$('[id *= div_rowacyr]').show();";
                        //ViewBag.scripcall += "$('[id *= div_acyr]').show();";
                        ViewBag.scripcall += "$('[id *= div_eduboard]').show();";
                        model[0].Col43 = mq;
                    }
                    else
                    {
                        //ViewBag.scripcall += "$('[id *= div_rowacyr]').hide();";
                        //ViewBag.scripcall += "$('[id *= div_acyr]').hide();";
                        ViewBag.scripcall += "$('[id *= div_eduboard]').hide();";

                        model[0].Col43 = mq;

                    }
                }
                catch (Exception ex) { sgen.showmsg(1, ex.Message.ToString(), 0); }

            }
            else if (command == "Country")
            {
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
                ViewBag.vsavenew = "";
                ViewBag.scripCall = "enableForm();";
                List<SelectListItem> modempty = new List<SelectListItem>();
                model[0].TList4 = modempty;
                model[0].TList5 = modempty;
                model[0].SelectedItems4 = new string[] { "" };
                model[0].SelectedItems5 = new string[] { "" };

                DataTable dtc = sgen.getdata(userCode, "select distinct state_gst_code,state_name from country_state where alpha_2='" + model[0].SelectedItems3.FirstOrDefault() + "'");
                if (dtc.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtc.Rows)
                    {
                        mod4.Add(new SelectListItem { Text = dr["state_name"].ToString(), Value = dr["state_gst_code"].ToString() });
                    }
                    TempData[MyGuid + "_Tlist4"] = model[0].TList4 = mod4;
                }


                if (model[0].Col43.Equals("Educational Inst."))
                {
                    //ViewBag.scripcall += "$('[id *= div_rowacyr]').show();";
                    //ViewBag.scripcall += "$('[id *= div_acyr]').show();";
                    ViewBag.scripcall += "$('[id *= div_eduboard]').show();";
                }
                else
                {
                    //ViewBag.scripcall += "$('[id *= div_rowacyr]').hide();";
                    //ViewBag.scripcall += "$('[id *= div_acyr]').hide();";
                    ViewBag.scripcall += "$('[id *= div_eduboard]').hide();";
                }
            }
            else if (command == "State")
            {
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
                ViewBag.vsavenew = "";
                ViewBag.scripCall = "enableForm();";
                mod5 = new List<SelectListItem>();
                model[0].TList5 = mod5;
                model[0].SelectedItems5 = new string[] { "" };

                try
                {
                    DataTable dt = new DataTable();
                    DataTable dt2 = new DataTable();
                    //               dt2 = sgen.getdata(userCode, "SELECT distinct ord,city_name FROM (SELECT '0' as ord,city_name FROM country_state where state_gst_code='" + model[0].SelectedItems4.FirstOrDefault() + "'" +
                    //" union SELECT '1' as ord, 'Other' city_name from dual ) tab order by 1,2");
                    dt2 = sgen.getdata(userCode, "SELECT distinct ord,city_name FROM (SELECT '0' as ord,city_name FROM country_state where state_gst_code='" + model[0].SelectedItems4.FirstOrDefault() + "') tab order by 1,2");
                    if (dt2.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt2.Rows)
                        {
                            mod5.Add(new SelectListItem { Text = dr["city_name"].ToString(), Value = dr["city_name"].ToString() });
                        }
                        TempData[MyGuid + "_Tlist5"] = model[0].TList5 = mod5;
                    }



                    if (model[0].Col43.Equals("Educational Inst."))
                    {
                        //ViewBag.scripcall += "$('[id *= div_rowacyr]').show();";
                        //ViewBag.scripcall += "$('[id *= div_acyr]').show();";
                        ViewBag.scripcall += "$('[id *= div_eduboard]').show();";
                    }
                    else
                    {
                        //ViewBag.scripcall += "$('[id *= div_rowacyr]').hide();";
                        //ViewBag.scripcall += "$('[id *= div_acyr]').hide();";
                        ViewBag.scripcall += "$('[id *= div_eduboard]').hide();";
                    }
                }
                catch (Exception ex)
                {
                    //ddl_city.SelectedValue = "Other";                  
                    //txt_city.Text = city_correct_name;
                    //txt_city.Visible = true;

                }

            }

            else if (command == "Cancel")
            {
                return CancelFun(model);

            }
            else if (command == "Callback")
            {
                if (model[0].Col134 == "N" && btnval.Trim().Equals("EDIT"))
                {
                    ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for Edit " + model[0].Col9 + ", please contact your admin', 2);disableForm();";
                    return View(model);
                }
                model = StartCallback(model);
                if (model[0].Col41 == null) ViewBag.scripCall = ViewBag.scripCall + "initialize(28.477450,77.034138);";
                else ViewBag.scripCall = ViewBag.scripCall + "initialize(" + model[0].Col41 + ");";

                if (model[0].Col43.Equals("Educational Inst."))
                {
                    ViewBag.scripcall += "$('[id *= div_rowacyr]').show();";
                    ViewBag.scripcall += "$('[id *= div_acyr]').show();";
                    ViewBag.scripcall += "$('[id *= div_eduboard]').show();";

                }
                else
                {
                    ViewBag.scripcall += "$('[id *= div_rowacyr]').hide();";
                    ViewBag.scripcall += "$('[id *= div_acyr]').hide();";
                    ViewBag.scripcall += "$('[id *= div_eduboard]').hide();";

                }

            }
            else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
            {
                try
                {
                    bool res2 = false;
                    if (model[0].SelectedItems1.FirstOrDefault().Equals(0) || model[0].SelectedItems1.FirstOrDefault().Equals("")) { ViewBag.scripCall = "showmsgJS(1, 'Please select company', 2);"; goto END; }
                    unit_count = Convert.ToInt32(sgen.getstring(userCode, "select Count(cup_id) as unit_count from " + model[0].Col10 + ""));
                    custunit_count = Convert.ToInt32(controls_mst.Split(',')[2]);

                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("N"))
                    {
                        if (unit_count >= custunit_count)
                        {
                            ViewBag.vnew = "disabled='disabled'";
                            ViewBag.vedit = "disabled='disabled'";
                            ViewBag.vsave = "";
                            ViewBag.vsavenew = "";
                            sgen.showmsg(1, "you cannot create a new unit, ur no. of unit is limited", 2); goto END;
                        }
                    }

                    if (model[0].Col43.Equals("Educational Inst."))
                    {

                        //if (model[0].Col34 == null) { ViewBag.scripCall = "showmsgJS(1, 'Please Enter From Academic Year', 2);"; goto END; }
                        //if (model[0].Col35 == null) { ViewBag.scripCall = "showmsgJS(1, 'Please Enter To Academic Year', 2);"; goto END; }
                        if (model[0].SelectedItems2.FirstOrDefault().Equals(""))
                        {
                            ViewBag.vnew = "disabled='disabled'";
                            ViewBag.vedit = "disabled='disabled'";
                            ViewBag.vsave = "";
                            ViewBag.vsavenew = "";
                            ViewBag.scripCall = "showmsgJS(1, 'Please Select Board', 2);";
                            ViewBag.scripcall += "$('[id *= div_eduboard]').show();";

                            goto END;
                        }
                    }

                    string address = model[0].Col20 + "$" + model[0].Col21 + "$" + model[0].Col22;
                    Boolean status = true;
                    if (model[0].Col40.Trim() == "0")
                    {
                        type = "DD" + type;
                        status = false;
                    }
                    else status = true;
                    string city_data = "";
                    if (model[0].SelectedItems5.FirstOrDefault() == "Other") city_data = model[0].Col19;
                    else city_data = model[0].SelectedItems5.FirstOrDefault();

                    DataTable dtu = new DataTable();
                    dtu = sgen.getdata(userCode, "select * from " + model[0].Col10 + " WHERE 1=2");

                    string Unit_Code = "";
                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        mq1 = " and vch_num<>'" + model[0].Col16 + "'";
                        isedit = true;
                        vch_num = model[0].Col16;
                        Unit_Code = model[0].Col17;
                        CUP_id = model[0].Col55;
                    }
                    else
                    {
                        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where Company_Profile_Id='"
                            + model[0].SelectedItems1.FirstOrDefault() + "'";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");

                        Unit_Code = sgen.genNo(userCode, "Select max(to_number(Unit_Code)) as Gen_Unit_Code from " + model[0].Col10 + " where Company_Profile_Id="
                            + model[0].SelectedItems1.FirstOrDefault(), 3, "Gen_Unit_Code");
                        CUP_id = model[0].SelectedItems1.FirstOrDefault() + Unit_Code;

                        isedit = false;
                    }

                    string currdate = sgen.server_datetime(userCode);
                    string ent_dt = currdate;

                    #region dt unit

                    DataRow dru = dtu.NewRow();
                    dru["vch_num"] = vch_num.Trim();
                    dru["vch_date"] = ent_dt.Trim();
                    dru["type"] = "";
                    dru["latlng"] = model[0].Col41;
                    dru["addr"] = model[0].Col42;
                    dru["CUP_Id"] = CUP_id.Trim();
                    dru["Company_Profile_Id"] = model[0].SelectedItems1.FirstOrDefault();
                    dru["Company_Code"] = "";
                    dru["Unit_Code"] = Unit_Code;
                    dru["Unit_Country"] = model[0].SelectedItems3.FirstOrDefault();
                    dru["Unit_State"] = model[0].SelectedItems4.FirstOrDefault();
                    dru["Unit_City"] = city_data;
                    dru["Unit_Address"] = model[0].Col20 + "$" + model[0].Col21 + "$" + model[0].Col22;
                    dru["Unit_Pincode"] = model[0].Col23;
                    try { dru["Unit_GSTIN_No"] = model[0].Col27.ToUpper().Trim(); } catch { }
                    dru["Unit_Email"] = model[0].Col28;
                    dru["Unit_Contact_No"] = model[0].Col29 == null ? "0" : model[0].Col29;

                    dru["Unit_Alternate_Contact_No"] = model[0].Col30 == null ? "0" : model[0].Col30;
                    dru["Unit_website"] = model[0].Col31;
                    dru["Unit_Contact_Person_Name"] = model[0].Col36;
                    dru["Unit_Person_Designation"] = model[0].Col37;
                    dru["Unit_Person_Email_Id"] = model[0].Col38;
                    dru["Unit_Person_Contact_No"] = model[0].Col39 == null ? "0" : model[0].Col39;

                    dru["Unit_Name"] = model[0].Col18;
                    dru["Unit_Status"] = status;
                    dru["edu_board"] = model[0].SelectedItems2.FirstOrDefault();

                    dru["dos"] = sgen.Make_date_S(model[0].Col24);
                    dru["doc"] = sgen.Make_date_S(model[0].Col25);
                    dru["septr"] = model[0].Col26;
                    dru["ll_act"] = model[0].SelectedItems6.FirstOrDefault();

                    dru["bank"] = model[0].Col44;
                    dru["branch"] = model[0].Col45;
                    dru["acctno"] = model[0].Col46;
                    dru["ifsc"] = model[0].Col47;

                    if (isedit)
                    {
                        dru["Unit_Ent_By"] = model[0].Col3;
                        dru["Unit_Ent_Date"] = sgen.Make_date_S(model[0].Col4);
                        dru["rec_id"] = model[0].Col7;
                    }
                    else
                    {
                        dru["rec_id"] = "0";
                        dru["Unit_Ent_By"] = userid_mst;
                        dru["unit_Ent_Date"] = ent_dt;
                    }

                    dru["Unit_Edit_By"] = userid_mst;
                    dru["Unit_Edit_Date"] = ent_dt;
                    dtu.Rows.Add(dru);

                    #endregion

                    Satransaction sat1 = new Satransaction(userCode, MyGuid);
                    //Satransaction sat2 = new Satransaction(userCode, MyGuid);
                    //Satransaction sat3 = new Satransaction(userCode, MyGuid);

                    res = sgen.Update_data_fast1_uncommit(userCode, dtu, model[0].Col10, model[0].Col8, isedit, sat1);
                    if (res)
                    {
                        if (!isedit)
                        {
                            //                        mq = "Insert into " + userCode + ".reports (rec_id,vch_num,vch_date,rpt_name,type,cat_id,cat_name,menu_id,module_id,module,username,role,ent_by,ent_date,created_date,edit_by,edit_date,client_id,client_unit_id,com_id,rpt_newname,fun_name) " +
                            //"select rec_id,vch_num,vch_date,rpt_name,type,cat_id,cat_name,menu_id,module_id,module,username,role,ent_by,ent_date,created_date,edit_by,edit_date,'" + model[0].SelectedItems1.FirstOrDefault() +"','" + CUP_id.Trim() + "'," +
                            //"com_id,rpt_newname,fun_name from " + userCode + ".reports where type = 'RPT' and client_id = '001' and client_unit_id = '001001'";
                            //                        res = sgen.execute_cmd(userCode, mq);
                            //                        if (!res) ViewBag.scripCall += "showmsgJS(1, 'Report Type not created for this Company & Unit', 2);";

                            #region fy

                            //DataTable dtfy = new DataTable();
                            //dtfy = sgen.getdata(userCode, "select * from " + model[0].Col52 + " WHERE 1=2");
                            //mq2 = sgen.genNo(userCode, "Select max(to_number(vch_num)) as Gen_Unit_Code from " + model[0].Col52 + " where " +
                            //         "client_id='" + model[0].SelectedItems1.FirstOrDefault() + "'", 6, "Gen_Unit_Code");

                            //string fyf = DateTime.ParseExact(model[0].Col32, sgen.Getdateformat(), CultureInfo.InvariantCulture).ToString("yyyy");
                            //string com_code = userCode + "-" + fyf;
                            //string fyt = DateTime.ParseExact(model[0].Col33, sgen.Getdateformat(), CultureInfo.InvariantCulture).ToString("yyyy");

                            //#region  dt fy

                            //DataRow drf = dtfy.NewRow();
                            //drf["vch_num"] = mq2.Trim();
                            //drf["vch_date"] = currdate.Trim();
                            //drf["type"] = "";
                            //drf["com_code"] = com_code;
                            //drf["com_year"] = fyf + " " + fyt;
                            //drf["year_from"] = sgen.Make_date_S(model[0].Col32);
                            //drf["year_to"] = sgen.Make_date_S(model[0].Col33);
                            //drf["ent_by"] = userid_mst;
                            //drf["ent_date"] = currdate.Trim();
                            //drf["rec_id"] = "0";
                            //drf["edit_By"] = userid_mst;
                            //drf["edit_Date"] = currdate.Trim();
                            //drf["client_id"] = model[0].SelectedItems1.FirstOrDefault();
                            //drf["client_unit_id"] = CUP_id.Trim();
                            //dtfy.Rows.Add(drf);

                            //#endregion

                            //bool res1 = sgen.Update_data_uncommit(userCode, dtfy, model[0].Col52, "", false, out sat2);
                            #endregion

                            //if (res1)
                            //{

                            //    try
                            //    {
                            //        if (!model[0].Col34.Equals(null) && !model[0].Col35.Equals(null))
                            //        {
                            //            #region ac
                            //            DataTable dtac = new DataTable();
                            //            dtac = sgen.getdata(userCode, "select * from " + model[0].Col53 + " WHERE 1=2");
                            //            string acf = DateTime.ParseExact(model[0].Col34, sgen.Getdateformat(), CultureInfo.InvariantCulture).ToString("yyyy");
                            //            string act = DateTime.ParseExact(model[0].Col35, sgen.Getdateformat(), CultureInfo.InvariantCulture).ToString("yyyy");
                            //            string acfm = DateTime.ParseExact(model[0].Col34, sgen.Getdateformat(), CultureInfo.InvariantCulture).ToString("MMMM");
                            //            string actm = DateTime.ParseExact(model[0].Col35, sgen.Getdateformat(), CultureInfo.InvariantCulture).ToString("MMMM");
                            //            int acfm_id = DateTime.ParseExact(model[0].Col34, sgen.Getdateformat(), CultureInfo.InvariantCulture).Month;
                            //            int actm_id = DateTime.ParseExact(model[0].Col35, sgen.Getdateformat(), CultureInfo.InvariantCulture).Month;

                            //            string vchac = sgen.genNo(userCode, "Select max(to_number(vch_num)) as maxno from " + model[0].Col53 + " where " +
                            //                "type='ACY' and client_id='" + model[0].SelectedItems1.FirstOrDefault() + "'", 6, "maxno");
                            //            string acid = sgen.genNo(userCode, "Select max(to_number(academic_year_id)) as maxno from " + model[0].Col53 + " " +
                            //                "where type='ACY' and client_id='" + model[0].SelectedItems1.FirstOrDefault() + "'", 3, "maxno");

                            //            #region  dt ac

                            //            DataRow dra = dtac.NewRow();
                            //            dra["vch_num"] = vchac.Trim();
                            //            dra["vch_date"] = currdate.Trim();
                            //            dra["type"] = "ACY";
                            //            dra["academic_year_id"] = acid;
                            //            dra["academic_year"] = acf + " " + act;
                            //            dra["fmonthid"] = acfm_id;
                            //            dra["tmonthid"] = actm_id;
                            //            dra["from_month"] = acfm;
                            //            dra["to_month"] = actm;
                            //            dra["from_date"] = sgen.Make_date_S(model[0].Col34);
                            //            dra["to_date"] = sgen.Make_date_S(model[0].Col35);
                            //            dra["ent_by"] = userid_mst;
                            //            dra["ent_date"] = currdate.Trim();
                            //            dra["rec_id"] = "0";
                            //            dra["edit_By"] = userid_mst;
                            //            dra["edit_Date"] = currdate.Trim();
                            //            dra["client_id"] = model[0].SelectedItems1.FirstOrDefault();
                            //            dra["client_unit_id"] = CUP_id.Trim();
                            //            dtac.Rows.Add(dra);

                            //            #endregion

                            //            res2 = sgen.Update_data_uncommit(userCode, dtac, model[0].Col53, "", false, out sat3);
                            //            #endregion
                            //            if (!res2)
                            //            {
                            //                sat1.Rollback();
                            //                sat2.Rollback();
                            //                sat3.Rollback();
                            //                goto END;
                            //            }

                            //        }
                            //    }   
                            //    catch
                            //    {
                            //        if (model[0].Col43.Equals("Educational Inst."))
                            //        {
                            //            res2 = true;
                            //        }
                            //    }

                            //}
                            //else
                            //{
                            //    ViewBag.scripCall = "showmsgJS(1, 'Something Went Wrong,Not Added', 0);";
                            //    sat1.Rollback();
                            //    sat2.Rollback();
                            //    sat3.Rollback();
                            //    goto END;
                            //}
                        }
                        sat1.Commit();
                        //sat2.Commit();
                        //sat3.Commit();

                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col13 = "Save",
                            Col100 = "Save & New",
                            Col121 = "S",
                            Col122 = "<u>S</u>ave",
                            Col123 = "Save & Ne<u>w</u>",
                            Col9 = tm.Col9,
                            Col10 = tm.Col10,
                            Col11 = tm.Col11,
                            Col12 = tm.Col12,
                            Col14 = tm.Col14,
                            Col15 = tm.Col15,
                            Col20 = tm.Col20,
                            TList1 = mod1,
                            TList2 = mod2,
                            TList3 = mod3,
                            TList4 = mod4,
                            TList5 = mod5,
                            TList6 = mod6,
                            TList7 = mod7,
                            SelectedItems1 = new string[] { "" },
                            SelectedItems2 = new string[] { "" },
                            SelectedItems3 = new string[] { "" },
                            SelectedItems4 = new string[] { "" },
                            SelectedItems5 = new string[] { "" },
                            SelectedItems6 = new string[] { "" },
                            SelectedItems7 = new string[] { "" },


                        });
                        if (command.Trim().ToUpper().Equals("SAVE") || command.Trim().ToUpper().Equals("UPDATE"))
                        {
                            ViewBag.vnew = "";
                            ViewBag.vedit = "";
                            ViewBag.vsave = "disabled='disabled'";
                            ViewBag.vsavenew = "disabled='disabled'";
                            if (isedit) ViewBag.scripCall = "showmsgJS(1, 'Your Unit Updated successfully', 1);disableForm();";
                            else ViewBag.scripCall = "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";

                            //ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully'');disableForm();";
                        }
                        else if (command.Trim().ToUpper().Equals("SAVE & NEW") || command.Trim().ToUpper().Equals("UPDATE & NEW"))
                        {
                            try
                            {
                                model = new_unit_profile(model);
                                ViewBag.scripCall = "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                            }
                            catch (Exception ex) { }
                        }
                    }
                    else
                    {
                        sgen.showmsg(1, "Something Went Wrong,Not Added", 0);
                        ViewBag.scripCall = "showmsgJS(1, 'Something Went Wrong,Not Added', 0);";
                        sat1.Rollback();
                        //sat2.Rollback();
                        //sat3.Rollback();
                        goto END;
                    }
                END:;
                }

                catch (Exception ex) { sgen.showmsg(1, ex.Message.ToString(), 0); }
            }

            ModelState.Clear();
            return View(model);

        }
        #endregion

        [HttpPost]
        public void fdelete(string value)
        {
            if (!value.Trim().Equals(""))
            {
                sgen.SetSession(MyGuid, "delid", value);
            }
        }
        [HttpPost]
        public ContentResult CheckConn(string Myguid)
        {
            Multiton multiton = Multiton.GetInstance(MyGuid);
            OracleConnection fCon;
            try
            {
                fCon = new OracleConnection(connString(multiton.UserCode));
                fCon.Open();
            }
            catch (Exception err)
            {
                fCon = null;
            }
            if (fCon == null) return Content("null");
            else return Content("ok");
        }
        public string connString(string UserCode)
        {

            //string path = @"C:\skyinfy\mytns2.txt";
            string path = HttpRuntime.AppDomainAppPath + "\\mytns2.txt";

            string str = "", srv = "", PWD = "", constr = "", IP = "";
            try
            {
                if (System.IO.File.Exists(path))
                {
                    StreamReader sr = new StreamReader(path);
                    str = sr.ReadToEnd().Trim();
                    if (str.Contains("\r")) str = str.Replace("\r", ",");
                    if (str.Contains("\n")) str = str.Replace("\n", ",");
                    str = str.Replace(",,", ",");
                    IP = str.Split(',')[1]; srv = str.Split(',')[2];
                    sr.Close();
                    PWD = "SATECH";
                    constr = "Data Source=(DESCRIPTION="
                    + "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST= " + IP.Trim() + ")(PORT=1521)))"
                    + "(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=" + srv + ")));"
                    + "User Id=" + UserCode.ToUpper() + "; Password=" + PWD + ";Pooling=true;";
                    //constr = constr + ";Connection Timeout=900;pooling='true';Max Pool Size=900";
                }
            }
            catch { }
            return constr;
        }

        #region activations
        public ActionResult Activations(string mid)
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "ACTIVATIONS";
            model[0].Col10 = "activations";
            model[0].Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            model[0].Col12 = "LIC";
            model[0].Col13 = "Save";
            model[0].Col100 = "Save & New";
            model[0].Col121 = "S";
            model[0].Col122 = "<u>S</u>ave";
            model[0].Col123 = "Save & Ne<u>w</u>";
            return View(model);
        }
        public List<Tmodelmain> new_Activations(List<Tmodelmain> model)
        {
            try
            {
                sgen.SetSession(MyGuid, "EDMODE", "N");
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
                ViewBag.vsavenew = "";
                ViewBag.scripCall = "enableForm();";
                mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where type='" + model[0].Col12 + "'";
                vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                model[0].Col16 = vch_num.Trim();
            }
            catch (Exception ex) { }
            return model;
        }
        [HttpPost]
        public ActionResult Activations(List<Tmodelmain> model, string command, string mid, HttpPostedFileBase upd1)
        {
            FillMst(model[0].Col15);
            var tm = model.FirstOrDefault();
            if (command == "New") { model = new_Activations(model); }
            else if (command == "Cancel") { return CancelFun(model); }
            else if (command == "Callback")
            {
                if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
                model = StartCallback(model);
            }
            else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
            {
                try
                {
                    type = model[0].Col12;
                    string currdate = sgen.server_datetime(userCode);
                    DataTable dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10);
                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        mq1 = " and vch_num<>'" + model[0].Col16 + "'";
                        isedit = true;
                        vch_num = model[0].Col16;
                    }
                    else
                    {
                        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where type='" + type + "'";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                        isedit = false;
                    }

                    #region dt compnay
                    DataRow dr = dtstr.NewRow();
                    dr["vch_num"] = vch_num.Trim();
                    dr["vch_date"] = currdate;
                    dr["type"] = type.Trim();
                    dr["Compcode"] = model[0].Col17;
                    dr["CompName"] = model[0].Col18;
                    dr["hddno"] = model[0].Col19;
                    dr["keycode"] = EncryptDecrypt.Encrypt(model[0].Col17 + model[0].Col18);
                    if (isedit)
                    {
                        dr["client_id"] = model[0].Col1.Trim();
                        dr["client_unit_id"] = model[0].Col2.Trim();
                        dr["ent_by"] = model[0].Col3.Trim();
                        dr["ent_date"] = model[0].Col4.Trim();
                        dr["edit_by"] = userid_mst;
                        dr["edit_date"] = currdate;
                        dr["rec_id"] = model[0].Col7;
                    }
                    else
                    {
                        dr["client_id"] = clientid_mst;
                        dr["client_unit_id"] = unitid_mst;
                        dr["ent_by"] = userid_mst;
                        dr["ent_date"] = currdate;
                        dr["edit_by"] = "-";
                        dr["edit_date"] = currdate;
                        dr["rec_id"] = "0";
                    }
                    dtstr.Rows.Add(dr);

                    #endregion                 
                    ok = sgen.Update_data(userCode, dtstr, model[0].Col10, model[0].Col8, isedit);
                    if (ok)
                    {
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col9 = tm.Col9,
                            Col10 = tm.Col10,
                            Col11 = tm.Col11,
                            Col12 = tm.Col12,
                            Col13 = "Save",
                            Col14 = tm.Col14,
                            Col15 = tm.Col15,
                            Col100 = "Save & New",
                            Col121 = "S",
                            Col122 = "<u>S</u>ave",
                            Col123 = "Save & Ne<u>w</u>"
                        });
                        if (command.Trim().ToUpper().Equals("SAVE") || command.Trim().ToUpper().Equals("UPDATE"))
                        {
                            ViewBag.vnew = "";
                            ViewBag.vedit = "";
                            ViewBag.vsave = "disabled='disabled'";
                            ViewBag.vsavenew = "disabled='disabled'";
                            ViewBag.scripCall += "disableForm();";
                            //ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully'');disableForm();";
                        }
                        else if (command.Trim().ToUpper().Equals("SAVE & NEW") || command.Trim().ToUpper().Equals("UPDATE & NEW"))
                        {
                            try
                            {
                                model = new_Activations(model);
                                // ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully'');";
                            }
                            catch (Exception ex) { }
                        }
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                    }
                }
                catch (Exception ex) { }
            }
            ModelState.Clear();
            return View(model);
        }
        #endregion

        #region cpanal
        public ActionResult cpanal()
        {
            FillMst();
            chkRef();
            string m_module3 = "";
            List<Tmodelmain> model = new List<Tmodelmain>();
            Tmodelmain tm1 = new Tmodelmain();
            DataTable dt = new DataTable();
            DataTable dtn = new DataTable();
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            m_id_mst = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            m_module3 = sgen.GetCookie(MyGuid, "m_module3");
            tm1.Col9 = "CPANEL";
            tm1.Col14 = mid_mst;
            tm1.Col15 = m_id_mst;
            mq = "select rec_id SNO,id ,name ,param1,param2,param3,param4,param5,param6,param7,param8,param9 from controls where type='CTL' and m_module3='" + m_module3 + "' or m_module3='-'";
            sgen.SetSession(MyGuid, "gridq_demogrid", mq);
            model.Add(tm1);
            ModelState.Clear();
            return View(model);
        }
        [HttpPost]
        public ContentResult updateval(string val, string colval, string col)
        {
            try
            {
                if (val.Trim() != null)
                {
                    mq = "update Controls set " + col + "='" + colval + "' where rec_id=" + val.Trim() + " and type='CTL'";
                    res = sgen.execute_cmd(userCode, mq);
                    if (!res) { return Content("N"); }
                    else return Content("Y");
                }
                else { return Content("N"); }

            }
            catch (Exception err) { return Content("N"); }
        }
        #endregion

        #region mailConfig
        public ActionResult mailConfig()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "MAIL CONFIGURATION";
            model[0].Col12 = "MLC";
            model[0].Col10 = "mail_config";
            model[0].Col11 = " and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            return View(model);
        }
        [HttpPost]
        public ActionResult mailConfig(List<Tmodelmain> model, string command, string hfrow, string hftable)
        {
            FillMst();
            DataSet ds = new DataSet();
            if (hftable != null)
            {
                ds = sgen.setDS(hftable);
                model[0].dt1 = ds.Tables[0];
            }
            var tm = model.FirstOrDefault();

            if (command == "New")
            {
                try
                {
                    model = getnew(model);
                    DataTable dt = new DataTable();
                    mq = "select (vch_num||to_char(vch_date,'yyyyMMdd')||type||client_unit_id) fstr,module,pgmail,userid,extmail,extphone,temp MailTemp," +
                        "phntemp PhoneTemp,(case when status='Y' then 'True' else 'False' end) status,(case when phnstatus='Y' then 'True' else 'False' end) Whatsappstatus from mail_config where type='MLC' and client_unit_id='" + unitid_mst + "'";
                    dt = sgen.getdata(userCode, mq);
                    if (dt.Rows.Count > 0)
                    {
                        model[0].dt1 = dt;
                    }
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
            else if (command == "Save" || command == "Update")
            {
                try
                {
                    Satransaction sat = new Satransaction(userCode, MyGuid);
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime(userCode);
                    //if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    //{
                    //    mq1 = " and vch_num<>'" + model[0].Col16.Trim() + "'";
                    //    isedit = true;
                    //    vch_num = model[0].Col16;
                    //}
                    //else
                    //{
                    isedit = true;
                    mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                    vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                    model[0].Col16 = vch_num;
                    type = model[0].Col12;
                    //}                    

                    DataTable dataTable = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    for (int i = 0; i < model[0].dt1.Rows.Count; i++)
                    {
                        DataRow dr = dataTable.NewRow();
                        dr["rec_id"] = "0";
                        dr["vch_num"] = vch_num.Trim();
                        dr["vch_date"] = currdate;
                        dr["type"] = model[0].Col12;
                        dr["module"] = model[0].dt1.Rows[i][1].ToString();
                        dr["pgmail"] = model[0].dt1.Rows[i][2].ToString();
                        dr["userid"] = model[0].dt1.Rows[i][3].ToString();
                        dr["extmail"] = model[0].dt1.Rows[i][4].ToString();
                        dr["extphone"] = model[0].dt1.Rows[i][5].ToString();
                        dr["temp"] = model[0].dt1.Rows[i][6].ToString();
                        dr["phntemp"] = model[0].dt1.Rows[i][7].ToString();
                        dr["status"] = Convert.ToBoolean(model[0].dt1.Rows[i][8].ToString()) == true ? "Y" : "N";
                        dr["phnstatus"] = Convert.ToBoolean(model[0].dt1.Rows[i][9].ToString()) == true ? "Y" : "N";

                        //if (isedit)
                        //{
                        //    dr["ent_by"] = model[0].Col3;
                        //    dr["ent_date"] = model[0].Col4;
                        //    dr["client_id"] = clientid_mst;
                        //    dr["client_unit_id"] = unitid_mst;
                        //    dr["edit_by"] = userid_mst;
                        //    dr["edit_date"] = currdate;
                        //}
                        //else
                        //{
                        dr["ent_by"] = userid_mst;
                        dr["ent_date"] = currdate;
                        dr["edit_by"] = "-";
                        dr["edit_date"] = currdate;
                        dr["client_id"] = clientid_mst;
                        dr["client_unit_id"] = unitid_mst;
                        //}
                        dataTable.Rows.Add(dr);
                    }
                    res = sgen.Update_data_fast1_uncommit(userCode, dataTable, model[0].Col10.Trim(), "(client_unit_id||type)='" + unitid_mst + type + "'", true, sat);
                    if (res == true)
                    {
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col9 = tmodel.Col9,
                            Col10 = tmodel.Col10,
                            Col11 = tmodel.Col11,
                            Col12 = tmodel.Col12,
                            Col13 = "Save",
                            Col14 = tmodel.Col14,
                            Col15 = tmodel.Col15
                        });
                        sat.Commit();
                        ViewBag.vnew = "";
                        ViewBag.vsave = "disabled='disabled'";
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                    }
                    else
                    {
                        sat.Rollback();
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.scripCall += "showmsgJS(1, 'Something went wrong', 0);";
                        ModelState.Clear();
                        return View(model);
                    }
                }
                catch (Exception ex) { ViewBag.scripCall += "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
            }
            ModelState.Clear();
            return View(model);
        }

        #endregion

        #region  manualMail
        public ActionResult manualMail()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            ViewBag.scripCall = "enableForm();";
            ViewBag.vsend = "disabled='disabled'";
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            model[0].Col9 = "MANUAL MAIL CONFIG";
            model[0].Col10 = "";
            model[0].Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            model[0].Col12 = ""; //TYPE                        
            return View(model);
        }
        [HttpPost]
        public ActionResult manualMail(List<Tmodelmain> model, string command)
        {
            try
            {
                FillMst(model[0].Col15);
                var tm = model.FirstOrDefault();
                DataTable dt = new DataTable();

                if (command == "Cancel")
                {
                    return CancelFun(model);
                }
                else if (command == "Callback")
                {
                    model = StartCallback(model);
                }
                else if (command == "Fill Data")
                {
                    string FromDt = model[0].Col17.Trim();
                    string todt = model[0].Col18.Trim();

                    mq = "select (a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr,a.vch_num," +
                         "to_char(convert_tz(a.dof, 'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') as dof,a.occasion," +
                         "a.foodtype,m.master_name menu,v.master_name venue,a.servtxt1,a.servtxt2,a.servtxt3,a.servtxt4,a.servtxt5,a.servtxt6,a.servtxt7," +
                         "a.custname,to_char(a.dob,'ddMMyyyy') dob,a.custadd,a.contno,a.email,a.remark,a.tot,a.adv,a.bal from func a " +
                         "inner join master_setting m on m.master_id=a.menu and m.type='MEN' and find_in_set(m.client_unit_id,a.client_unit_id)=1 " +
                         "inner join master_setting v on v.master_id=a.menu and v.type='VEN' and find_in_set(v.client_unit_id,a.client_unit_id)=1 " +
                         "where a.type = 'BOK' and a.client_unit_id = '" + unitid_mst + "' and a.vch_date between to_date('" + FromDt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')";
                    dt = sgen.getdata(userCode, mq);
                    model = new List<Tmodelmain>();
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            Tmodelmain tmm = new Tmodelmain();
                            Boolean chk1 = true;
                            tmm.Col19 = dt.Rows[i]["vch_num"].ToString();
                            tmm.Col20 = dt.Rows[i]["occasion"].ToString();
                            tmm.Col21 = dt.Rows[i]["custname"].ToString();
                            tmm.Col22 = dt.Rows[i]["contno"].ToString();
                            tmm.Col23 = dt.Rows[i]["email"].ToString();
                            tmm.Col24 = dt.Rows[i]["dob"].ToString();
                            tmm.Col25 = dt.Rows[i]["custadd"].ToString();
                            tmm.Col26 = dt.Rows[i]["dof"].ToString();
                            tmm.Col27 = dt.Rows[i]["foodtype"].ToString();
                            tmm.Col28 = dt.Rows[i]["menu"].ToString();
                            tmm.Col29 = dt.Rows[i]["venue"].ToString();
                            tmm.Col30 = dt.Rows[i]["servtxt1"].ToString();
                            tmm.Col31 = dt.Rows[i]["servtxt2"].ToString();
                            tmm.Col32 = dt.Rows[i]["servtxt3"].ToString();
                            tmm.Col33 = dt.Rows[i]["servtxt4"].ToString();
                            tmm.Col34 = dt.Rows[i]["servtxt5"].ToString();
                            tmm.Col35 = dt.Rows[i]["servtxt6"].ToString();
                            tmm.Col36 = dt.Rows[i]["servtxt7"].ToString();
                            tmm.Col37 = dt.Rows[i]["tot"].ToString();
                            tmm.Col38 = dt.Rows[i]["adv"].ToString();
                            tmm.Col39 = dt.Rows[i]["bal"].ToString();
                            tmm.Col40 = dt.Rows[i]["remark"].ToString();

                            tmm.Chk1 = chk1;
                            tmm.Col14 = tm.Col14;
                            tmm.Col15 = tm.Col15;
                            tmm.Col9 = tm.Col9;
                            tmm.Col10 = tm.Col10;
                            tmm.Col11 = tm.Col11;
                            tmm.Col12 = tm.Col12;
                            tmm.Col17 = tm.Col17;
                            tmm.Col18 = tm.Col18;
                            tmm.Chk4 = tm.Chk4;
                            model.Add(tmm);
                        }
                    }
                    else
                    {
                        ViewBag.scripCall += "mytoast('warning', 'toast-top-right', 'No Data Exist');";
                        ViewBag.vsend = "disabled='disabled'";
                        model = new List<Tmodelmain>();
                        tm.Col16 = "";
                        tm.Col17 = tm.Col17;
                        tm.Col18 = tm.Col18;
                        tm.Col19 = "";
                        model.Add(tm);
                    }
                }
                else if (command == "Send")
                {
                    string pg = "", uid = "", temp = "", extmail = "", sub = "";
                    pg = "MAIL OF FEED BACK";
                    mq1 = "004";

                    mq = "select m.userid,nvl(group_concat(u.email),'0') umail,m.extmail,m.temp,t.col4 msub,to_char(t.col23) mbody from mail_config m " +
                         "inner join user_details u on find_in_set(u.vch_num, m.userid)= 1 and u.type = 'CPR' " +
                         "inner join enx_tab2 t on t.vch_num = m.temp and t.type = 'MTP' and t.col3 = '" + mq1 + "' and t.client_unit_id = m.client_unit_id " +
                         "where m.type = 'MLC' and m.client_unit_id = '" + unitid_mst + "' and m.status = 'Y' and m.module = '40000' and upper(m.pgmail)= '" + pg + "' " +
                         "group by m.userid,m.extmail,m.temp,t.col4,to_char(t.col23)";
                    DataTable dtm = sgen.getdata(userCode, mq);
                    if (dtm.Rows.Count > 0)
                    {
                        List<KCFile> fileCollection = new List<KCFile>();
                        uid = dtm.Rows[0]["umail"].ToString();
                        extmail = dtm.Rows[0]["extmail"].ToString();
                        temp = dtm.Rows[0]["mbody"].ToString();
                        sub = dtm.Rows[0]["msub"].ToString();

                        for (int i = 0; i < model.Count; i++)
                        {
                            if (model[i].Chk1 == true)
                            {
                                string newsub = "", newtemp = "";
                                List<Tmodel5> vars = new List<Tmodel5>();
                                vars.Add(new Tmodel5 { Col1 = "[occasion]", Col2 = model[i].Col20 });
                                vars.Add(new Tmodel5 { Col1 = "[foodtype]", Col2 = model[i].Col27 });
                                vars.Add(new Tmodel5 { Col1 = "[dof]", Col2 = sgen.Savedate(model[i].Col26, false) });
                                vars.Add(new Tmodel5 { Col1 = "[menu]", Col2 = model[i].Col28 });
                                vars.Add(new Tmodel5 { Col1 = "[venue]", Col2 = model[i].Col29 });
                                vars.Add(new Tmodel5 { Col1 = "[servtxt1]", Col2 = model[i].Col30 });
                                vars.Add(new Tmodel5 { Col1 = "[servtxt2]", Col2 = model[i].Col31 });
                                vars.Add(new Tmodel5 { Col1 = "[servtxt3]", Col2 = model[i].Col32 });
                                vars.Add(new Tmodel5 { Col1 = "[servtxt4]", Col2 = model[i].Col33 });
                                vars.Add(new Tmodel5 { Col1 = "[servtxt5]", Col2 = model[i].Col34 });
                                vars.Add(new Tmodel5 { Col1 = "[servtxt6]", Col2 = model[i].Col35 });
                                vars.Add(new Tmodel5 { Col1 = "[servtxt7]", Col2 = model[i].Col36 });
                                vars.Add(new Tmodel5 { Col1 = "[custname]", Col2 = model[i].Col21 });
                                vars.Add(new Tmodel5 { Col1 = "[tot]", Col2 = model[i].Col37 });
                                vars.Add(new Tmodel5 { Col1 = "[adv]", Col2 = model[i].Col38 });
                                vars.Add(new Tmodel5 { Col1 = "[bal]", Col2 = model[i].Col39 });
                                vars.Add(new Tmodel5 { Col1 = "[remark]", Col2 = model[i].Col40 });

                                for (int a = 0; a < 2; a++)
                                {
                                    string mdata = "";
                                    if (a == 0) mdata = sub;
                                    else if (a == 1) mdata = temp;
                                    foreach (var t in vars)
                                    {
                                        //mdata = Regex.Replace(mdata, @"^" + t.Col1 + "$", t.Col2);                                        
                                        mdata = Regex.Replace(mdata, "[^-+<>/#!@$%*()_={};[]:',.?&^ 0-9 a-z A-Z]+", "");
                                        mdata = mdata.Replace(t.Col1, t.Col2);
                                    }
                                    if (a == 0) newsub = mdata;
                                    else if (a == 1) newtemp = mdata;
                                }

                                string allmail = uid + "," + extmail + "," + model[i].Col23;
                                sgen.Send_mail_kcfile(userCode, clientid_mst, unitid_mst, allmail, "", "", newtemp, newsub, userid_mst, fileCollection);
                            }
                        }
                    }

                    model = new List<Tmodelmain>();
                    model.Add(new Tmodelmain
                    {
                        Col9 = tm.Col9,
                        Col10 = tm.Col10,
                        Col11 = tm.Col11,
                        Col12 = tm.Col12,
                        Col14 = tm.Col14,
                        Col15 = tm.Col15,
                    });
                }
            }
            catch (Exception err)
            {
                var LineNumber = new StackTrace(err, true).GetFrame(0).GetFileLineNumber();
                ViewBag.scripCall += "showmsgJS(1, '" + err.Message.Replace("'", "") + " At Line number " + LineNumber + "', 1);";
            }
            ModelState.Clear();
            return View(model);
        }
        #endregion

        #region  manualPhn
        public ActionResult manualPhn()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            ViewBag.scripCall = "enableForm();";
            ViewBag.vsend = "disabled='disabled'";
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            model[0].Col9 = "MANUAL PHONE CONFIG";
            model[0].Col10 = "";
            model[0].Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            model[0].Col12 = ""; //TYPE                        
            return View(model);
        }
        [HttpPost]
        public ActionResult manualPhn(List<Tmodelmain> model, string command)
        {
            try
            {
                FillMst(model[0].Col15);
                var tm = model.FirstOrDefault();
                DataTable dt = new DataTable();

                if (command == "Cancel")
                {
                    return CancelFun(model);
                }
                else if (command == "Callback")
                {
                    model = StartCallback(model);
                }
                else if (command == "Fill Data")
                {
                    string FromDt = model[0].Col17.Trim();
                    string todt = model[0].Col18.Trim();

                    mq = "select (a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr,a.vch_num," +
                         "to_char(convert_tz(a.dof, 'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') as dof,a.occasion," +
                         "a.foodtype,m.master_name menu,v.master_name venue,a.servtxt1,a.servtxt2,a.servtxt3,a.servtxt4,a.servtxt5,a.servtxt6,a.servtxt7," +
                         "a.custname,to_char(a.dob,'ddMMyyyy') dob,a.custadd,a.contno,a.email,a.remark,a.tot,a.adv,a.bal from func a " +
                         "inner join master_setting m on m.master_id=a.menu and m.type='MEN' and find_in_set(m.client_unit_id,a.client_unit_id)=1 " +
                         "inner join master_setting v on v.master_id=a.menu and v.type='VEN' and find_in_set(v.client_unit_id,a.client_unit_id)=1 " +
                         "where a.type = 'BOK' and a.client_unit_id = '" + unitid_mst + "' and a.vch_date between to_date('" + FromDt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')";
                    dt = sgen.getdata(userCode, mq);
                    model = new List<Tmodelmain>();
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            Tmodelmain tmm = new Tmodelmain();
                            Boolean chk1 = true;
                            tmm.Col19 = dt.Rows[i]["vch_num"].ToString();
                            tmm.Col20 = dt.Rows[i]["occasion"].ToString();
                            tmm.Col21 = dt.Rows[i]["custname"].ToString();
                            tmm.Col22 = dt.Rows[i]["contno"].ToString();
                            tmm.Col23 = dt.Rows[i]["email"].ToString();
                            tmm.Col24 = dt.Rows[i]["dob"].ToString();
                            tmm.Col25 = dt.Rows[i]["custadd"].ToString();
                            tmm.Col26 = dt.Rows[i]["dof"].ToString();
                            tmm.Col27 = dt.Rows[i]["foodtype"].ToString();
                            tmm.Col28 = dt.Rows[i]["menu"].ToString();
                            tmm.Col29 = dt.Rows[i]["venue"].ToString();
                            tmm.Col30 = dt.Rows[i]["servtxt1"].ToString();
                            tmm.Col31 = dt.Rows[i]["servtxt2"].ToString();
                            tmm.Col32 = dt.Rows[i]["servtxt3"].ToString();
                            tmm.Col33 = dt.Rows[i]["servtxt4"].ToString();
                            tmm.Col34 = dt.Rows[i]["servtxt5"].ToString();
                            tmm.Col35 = dt.Rows[i]["servtxt6"].ToString();
                            tmm.Col36 = dt.Rows[i]["servtxt7"].ToString();
                            tmm.Col37 = dt.Rows[i]["tot"].ToString();
                            tmm.Col38 = dt.Rows[i]["adv"].ToString();
                            tmm.Col39 = dt.Rows[i]["bal"].ToString();
                            tmm.Col40 = dt.Rows[i]["remark"].ToString();

                            tmm.Chk1 = chk1;
                            tmm.Col14 = tm.Col14;
                            tmm.Col15 = tm.Col15;
                            tmm.Col9 = tm.Col9;
                            tmm.Col10 = tm.Col10;
                            tmm.Col11 = tm.Col11;
                            tmm.Col12 = tm.Col12;
                            tmm.Col17 = tm.Col17;
                            tmm.Col18 = tm.Col18;
                            tmm.Chk4 = tm.Chk4;
                            model.Add(tmm);
                        }
                    }
                    else
                    {
                        ViewBag.scripCall += "mytoast('warning', 'toast-top-right', 'No Data Exist');";
                        ViewBag.vsend = "disabled='disabled'";
                        model = new List<Tmodelmain>();
                        tm.Col16 = "";
                        tm.Col17 = tm.Col17;
                        tm.Col18 = tm.Col18;
                        tm.Col19 = "";
                        model.Add(tm);
                    }
                }
                else if (command == "Send")
                {
                    string pg = "", uid = "", temp = "", extmail = "", sub = "";
                    pg = "MAIL OF FEED BACK";
                    mq1 = "004";

                    mq = "select m.userid,nvl(group_concat(u.email),'0') umail,m.extphone,m.phntemp,to_char(t.col23) mbody from mail_config m " +
                         "inner join user_details u on find_in_set(u.vch_num, m.userid)= 1 and u.type = 'CPR' " +
                         "inner join enx_tab2 t on t.vch_num = m.temp and t.type = 'PTP' and t.col3 = '" + mq1 + "' and t.client_unit_id = m.client_unit_id " +
                         "where m.type = 'MLC' and m.client_unit_id = '" + unitid_mst + "' and m.status = 'Y' and m.module = '40000' and upper(m.pgmail)= '" + pg + "' " +
                         "group by m.userid,m.extmail,m.temp,t.col4,to_char(t.col23)";
                    DataTable dtm = sgen.getdata(userCode, mq);
                    if (dtm.Rows.Count > 0)
                    {
                        List<KCFile> fileCollection = new List<KCFile>();
                        uid = dtm.Rows[0]["umail"].ToString();
                        extmail = dtm.Rows[0]["extphone"].ToString();
                        temp = dtm.Rows[0]["mbody"].ToString();

                        for (int i = 0; i < model.Count; i++)
                        {
                            if (model[i].Chk1 == true)
                            {
                                string newsub = "", newtemp = "";
                                List<Tmodel5> vars = new List<Tmodel5>();
                                vars.Add(new Tmodel5 { Col1 = "[occasion]", Col2 = model[i].Col20 });
                                vars.Add(new Tmodel5 { Col1 = "[foodtype]", Col2 = model[i].Col27 });
                                vars.Add(new Tmodel5 { Col1 = "[dof]", Col2 = sgen.Savedate(model[i].Col26, false) });
                                vars.Add(new Tmodel5 { Col1 = "[menu]", Col2 = model[i].Col28 });
                                vars.Add(new Tmodel5 { Col1 = "[venue]", Col2 = model[i].Col29 });
                                vars.Add(new Tmodel5 { Col1 = "[servtxt1]", Col2 = model[i].Col30 });
                                vars.Add(new Tmodel5 { Col1 = "[servtxt2]", Col2 = model[i].Col31 });
                                vars.Add(new Tmodel5 { Col1 = "[servtxt3]", Col2 = model[i].Col32 });
                                vars.Add(new Tmodel5 { Col1 = "[servtxt4]", Col2 = model[i].Col33 });
                                vars.Add(new Tmodel5 { Col1 = "[servtxt5]", Col2 = model[i].Col34 });
                                vars.Add(new Tmodel5 { Col1 = "[servtxt6]", Col2 = model[i].Col35 });
                                vars.Add(new Tmodel5 { Col1 = "[servtxt7]", Col2 = model[i].Col36 });
                                vars.Add(new Tmodel5 { Col1 = "[custname]", Col2 = model[i].Col21 });
                                vars.Add(new Tmodel5 { Col1 = "[tot]", Col2 = model[i].Col37 });
                                vars.Add(new Tmodel5 { Col1 = "[adv]", Col2 = model[i].Col38 });
                                vars.Add(new Tmodel5 { Col1 = "[bal]", Col2 = model[i].Col39 });
                                vars.Add(new Tmodel5 { Col1 = "[remark]", Col2 = model[i].Col40 });

                                for (int a = 0; a < 2; a++)
                                {
                                    string mdata = "";
                                    if (a == 0) mdata = sub;
                                    else if (a == 1) mdata = temp;
                                    foreach (var t in vars)
                                    {
                                        //mdata = Regex.Replace(mdata, @"^" + t.Col1 + "$", t.Col2);                                        
                                        mdata = Regex.Replace(mdata, "[^-+<>/#!@$%*()_={};[]:',.?&^ 0-9 a-z A-Z]+", "");
                                        mdata = mdata.Replace(t.Col1, t.Col2);
                                    }
                                    if (a == 0) newsub = mdata;
                                    else if (a == 1) newtemp = mdata;
                                }

                                string allmail = uid + "," + extmail + "," + model[i].Col23;
                                //sgen.Send_mail_kcfile(userCode, clientid_mst, unitid_mst, allmail, "", "", newtemp, newsub, userid_mst, fileCollection);
                            }
                        }
                    }

                    model = new List<Tmodelmain>();
                    model.Add(new Tmodelmain
                    {
                        Col9 = tm.Col9,
                        Col10 = tm.Col10,
                        Col11 = tm.Col11,
                        Col12 = tm.Col12,
                        Col14 = tm.Col14,
                        Col15 = tm.Col15,
                    });
                }
            }
            catch (Exception err)
            {
                var LineNumber = new StackTrace(err, true).GetFrame(0).GetFileLineNumber();
                ViewBag.scripCall += "showmsgJS(1, '" + err.Message.Replace("'", "") + " At Line number " + LineNumber + "', 1);";
            }
            ModelState.Clear();
            return View(model);
        }
        #endregion
    }
}


