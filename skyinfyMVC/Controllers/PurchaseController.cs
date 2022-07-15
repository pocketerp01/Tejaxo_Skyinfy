using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using QRCoder;
using skyinfyMVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
namespace skyinfyMVC.Controllers
{
    public class PurchaseController : Controller
    {
        static string mq0 = "", mq = "", mq1 = "", vch_num = "", btnval = "", tab_name = "", cmd = "", type = "", where = "", type_desc = "", tab_name1 = "", mid_mst = "", m_id_mst = "", Ac_Year_id = "", status = "", isgstr = "N", commval = "";
        //============================
        bool res = false, ok = false;
        static string MyGuid = "";
        sgenFun sgen;
        Cmd_Fun cmd_Fun;
        public static bool isedit = false;
        public static int cli = 0;
        string year_from = "", year_to = "";
        public static string userCode = "", userid_mst = "", cg_com_name = "", clientid_mst = "", unitid_mst = "", actionName = "", controllerName = "", role_mst = "", m_module3 = "", xprdrange = "";
        public void FillMst(string Myguid = "")
        {
            MyGuid = Myguid;
            //sgen = new sgenFun(MyGuid);
            if (MyGuid == "") { MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]); }
            //if (MyGuid == "") MyGuid = sgen.GetCookie("", "MyGuid");
            sgen = new sgenFun(MyGuid);
            cmd_Fun = new Cmd_Fun(MyGuid);
            userCode = sgen.GetCookie(MyGuid, "userCode");
            role_mst = sgen.GetCookie(MyGuid, "role_mst");
            userid_mst = sgen.GetCookie(MyGuid, "userid_mst");
            Ac_Year_id = sgen.GetCookie(MyGuid, "Ac_Year_id");
            cg_com_name = sgen.GetCookie(MyGuid, "cg_com_name");
            clientid_mst = sgen.GetCookie(MyGuid, "clientid_mst");
            unitid_mst = sgen.GetCookie(MyGuid, "unitid_mst");
            m_module3 = sgen.GetCookie(MyGuid, "m_module3");
            actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            sgen.SetSession(MyGuid, "viewName", actionName);
            sgen.SetSession(MyGuid, "controllerName", controllerName);
            year_to = sgen.GetCookie(MyGuid, "year_to");
            year_from = sgen.GetCookie(MyGuid, "year_from");
            xprdrange = "and vch_DATE between " + year_from + " and " + year_to + "";
        }
        #region MAIN FUNC
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
            tm1.Col13 = "Save";
            tm1.Col121 = "S";
            tm1.Col122 = "<u>S</u>ave";
            tm1.Col123 = "Save & Ne<u>w</u>";
            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = MyGuid.Trim();
            DataTable dtb = (DataTable)sgen.GetSession(MyGuid, "URIGHTS");
            try
            {
                dtb = dtb.Select("m_id='" + tm1.Col14 + "'").CopyToDataTable();
                if (dtb.Rows.Count > 0)
                {
                    tm1.Col133 = dtb.Rows[0]["btnnew"].ToString();
                    tm1.Col134 = dtb.Rows[0]["btnedit"].ToString();
                    tm1.Col135 = dtb.Rows[0]["btnview"].ToString();
                    tm1.Col136 = dtb.Rows[0]["btnextend"].ToString();
                }
            }
            catch (Exception ex) { }
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
            if (actionName == "party")
            {
                mq = "select max(to_number(substr(vch_num,4,8))) as auto_genid from " + model[0].Col10.Trim() + " a where  type in ('" + model[0].Col12.Trim() + "' ,'DD" + model[0].Col12 + "')" + model[0].Col11.Trim() + " and substr(vch_num,0,3)='" + model[0].Col131 + "'";
                vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                vch_num = model[0].Col131 + vch_num;
            }
            else
            {
                mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type in ('" + model[0].Col12.Trim() + "' ,'DD" + model[0].Col12 + "')" + model[0].Col11.Trim() + "";
                vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
            }
            model[0].Col16 = vch_num;
            if (model[0].Col133 == "N")
            {
                ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for new entry, please contact your admin', 2);";
                return (model);
            }
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
            if (sgen.GetSession(MyGuid, "podelrno") != null) btnval = "CHRGDEL";
            else if (sgen.GetSession(MyGuid, "delid") != null) { btnval = "FDEL"; }

            else if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
            //if (Session["btnval"] != null) btnval = Session["btnval"].ToString();
            model = CallbackFun(btnval, "N", actionName, controllerName, model);
            ViewBag.vnew = "disabled='disabled'";
            ViewBag.vedit = "disabled='disabled'";
            if (btnval != "VIEW")
            {
                ViewBag.vsave = "";
                ViewBag.vsavenew = "";
            }
            if (model[0].Col136 == "N" && btnval.Trim().Equals("EXT"))
            {
                ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for extend " + model[0].Col9 + ", please contact your admin', 2);disableForm();";
                return model;
            }
            else if (model[0].Col134 == "N" && btnval.Trim().Equals("EDIT"))
            {
                ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for edit " + model[0].Col9 + ", please contact your admin', 2);disableForm();";
                return model;
            }
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
        #endregion
        public ActionResult Index()
        {
            return View();
        }
        public void chkRef()
        {
            if (userCode.Equals("")) Response.Redirect(sgenFun.callbackurl);
            if (Request.UrlReferrer == null) { Response.Redirect(sgenFun.callbackurl); }
        }
        #region call back
        public List<Tmodelmain> CallbackFun(string btnval, string edmode, string viewName, string controllerName, List<Tmodelmain> model)
        {
            string mq = "";
            FillMst();
            //string def = "";
            string defcall = "";
            string taxrt1 = "", taxamt1 = "", taxrt2 = "", taxamt2 = "", taxrt3 = "", taxamt3 = "", rtcond = "", amtcond = "";
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();
            DataTable dtp = new DataTable();
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
            List<SelectListItem> mod12 = new List<SelectListItem>();
            List<SelectListItem> mod13 = new List<SelectListItem>();
            List<SelectListItem> mod14 = new List<SelectListItem>();
            List<SelectListItem> mod15 = new List<SelectListItem>();
            List<SelectListItem> mod16 = new List<SelectListItem>();
            List<SelectListItem> mod17 = new List<SelectListItem>();
            List<SelectListItem> mod18 = new List<SelectListItem>();
            List<SelectListItem> mod19 = new List<SelectListItem>();
            List<SelectListItem> mod20 = new List<SelectListItem>();
            List<SelectListItem> mod21 = new List<SelectListItem>();
            List<SelectListItem> mod22 = new List<SelectListItem>();
            List<SelectListItem> mod23 = new List<SelectListItem>();
            List<SelectListItem> mod24 = new List<SelectListItem>();
            List<SelectListItem> mod25 = new List<SelectListItem>();
            List<SelectListItem> mod26 = new List<SelectListItem>();
            List<SelectListItem> mod27 = new List<SelectListItem>();
            List<SelectListItem> mod28 = new List<SelectListItem>();
            List<SelectListItem> mod29 = new List<SelectListItem>();
            List<SelectListItem> mod30 = new List<SelectListItem>();
            List<SelectListItem> mod31 = new List<SelectListItem>();
            List<SelectListItem> mod32 = new List<SelectListItem>();
            List<SelectListItem> mod33 = new List<SelectListItem>();
            List<SelectListItem> mod34 = new List<SelectListItem>();
            List<SelectListItem> mod35 = new List<SelectListItem>();
            List<Tmodelmain> rpt_model = new List<Tmodelmain>();
            var tm = model.FirstOrDefault();
            String URL = sgen.GetSession(MyGuid, "SSEEKVAL").ToString();
            switch (viewName.ToLower())
            {
                #region vendor_detail
                //case "vendor_detail":
                //    switch (btnval.ToUpper())
                //    {
                //        case "EDIT":
                //            // shiv = why select *
                //            mq = "select * from clients_mst where (client_id||client_unit_id||vch_num||to_char(vch_date, 'yyyymmdd')|| type) = '" + URL + "'";
                //            dtt = sgen.getdata(userCode, mq);
                //            if (dtt.Rows.Count > 0)
                //            {
                //                sgen.SetSession(MyGuid, "EDMODE", "Y");
                //                #region MASTERS
                //                #region country
                //                mq = "select distinct country_name,alpha_2 from country_state order by country_name";
                //                dt = sgen.getdata(userCode, mq);
                //                if (dt.Rows.Count > 0)
                //                {
                //                    foreach (DataRow dr1 in dt.Rows)
                //                    {
                //                        mod1.Add(new SelectListItem { Text = dr1["country_name"].ToString(), Value = dr1["alpha_2"].ToString() });
                //                    }
                //                }
                //                TempData[MyGuid + "_Tlist1"] = model[0].TList1 = mod1;
                //                #endregion
                //                #region state                   
                //                mq = "select distinct state_name,state_gst_code from country_state where alpha_2='" + dtt.Rows[0]["country"] + "' and state_name!='-' order by state_name";
                //                dt = sgen.getdata(userCode, mq);
                //                if (dt.Rows.Count > 0)
                //                {
                //                    foreach (DataRow dr1 in dt.Rows)
                //                    {
                //                        mod2.Add(new SelectListItem { Text = dr1["state_name"].ToString(), Value = dr1["state_gst_code"].ToString() });
                //                    }
                //                }
                //                TempData[MyGuid + "_Tlist2"] = model[0].TList2 = mod2;
                //                #endregion
                //                #region party type
                //                TempData[MyGuid + "_Tlist3"] = model[0].TList3 = mod3 = cmd_Fun.partytype(userCode, unitid_mst);
                //                #endregion
                //                #region party location - fix
                //                mod4.Add(new SelectListItem { Text = "Domestic", Value = "001" });
                //                mod4.Add(new SelectListItem { Text = "Overseas", Value = "002" });
                //                TempData[MyGuid + "_Tlist4"] = model[0].TList4 = mod4;
                //                #endregion
                //                #region acc type
                //                TempData[MyGuid + "_Tlist5"] = model[0].TList5 = mod5 = cmd_Fun.acctypevdm(userCode, unitid_mst);
                //                #endregion
                //                #region currency type
                //                TempData[MyGuid + "_Tlist6"] = model[0].TList6 = mod6 = cmd_Fun.curname(userCode, unitid_mst);
                //                #endregion
                //                #region bank name
                //                TempData[MyGuid + "_Tlist7"] = model[0].TList7 = mod7 = cmd_Fun.bank(userCode, unitid_mst);
                //                #endregion
                //                #endregion

                //                String[] L1 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["country"].ToString()).Distinct()).Split(',');
                //                model[0].SelectedItems1 = L1;
                //                String[] L2 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["state"].ToString()).Distinct()).Split(',');
                //                model[0].SelectedItems2 = L2;
                //                String[] L3 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["ptype"].ToString()).Distinct()).Split(',');
                //                model[0].SelectedItems3 = L3;
                //                String[] L4 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["ploc"].ToString()).Distinct()).Split(',');
                //                model[0].SelectedItems4 = L4;
                //                String[] L5 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["acctype"].ToString()).Distinct()).Split(',');
                //                model[0].SelectedItems5 = L5;
                //                String[] L6 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["curtype"].ToString()).Distinct()).Split(',');
                //                model[0].SelectedItems6 = L6;
                //                String[] L7 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["bnk"].ToString()).Distinct()).Split(',');
                //                model[0].SelectedItems7 = L7;
                //                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                //                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                //                model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                //                model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                //                model[0].Col7 = dtt.Rows[0]["rec_id"].ToString();
                //                model[0].Col8 = "(client_id||client_unit_id||vch_num||to_char(vch_date, 'yyyymmdd')|| type) = '" + URL + "'";
                //                model[0].Col9 = tm.Col9;
                //                model[0].Col10 = tm.Col10;
                //                model[0].Col11 = tm.Col11;
                //                model[0].Col12 = tm.Col12;
                //                model[0].Col13 = "Update";
                //                model[0].Col121 = "U";
                //                model[0].Col122 = "<u>U</u>pdate";
                //                model[0].Col123 = "Update & Ne<u>w</u>";
                //                model[0].Col14 = tm.Col14;
                //                model[0].Col15 = tm.Col15;
                //                model[0].Col17 = dtt.Rows[0]["vch_num"].ToString();
                //                model[0].Col18 = dtt.Rows[0]["c_name"].ToString();
                //                model[0].Col22 = dtt.Rows[0]["addr"].ToString();
                //                model[0].Col23 = dtt.Rows[0]["pincode"].ToString();
                //                model[0].Col24 = dtt.Rows[0]["c_gstin"].ToString();
                //                if (dtt.Rows[0]["isgstr"].ToString() == "Y") { model[0].Chk1 = true; }
                //                model[0].Col25 = dtt.Rows[0]["type_desc"].ToString();
                //                model[0].Col26 = dtt.Rows[0]["cpname"].ToString();
                //                model[0].Col27 = dtt.Rows[0]["cpcont"].ToString();
                //                model[0].Col28 = dtt.Rows[0]["cpaltcont"].ToString();
                //                model[0].Col29 = dtt.Rows[0]["cpemail"].ToString();
                //                model[0].Col30 = dtt.Rows[0]["cpaddr"].ToString();
                //                model[0].Col31 = dtt.Rows[0]["cpdesig"].ToString();
                //                model[0].Col33 = dtt.Rows[0]["addr"].ToString();
                //                model[0].Col34 = dtt.Rows[0]["micrno"].ToString();
                //                model[0].Col35 = dtt.Rows[0]["tor"].ToString();
                //                model[0].Col36 = dtt.Rows[0]["panno"].ToString();
                //                model[0].Col37 = dtt.Rows[0]["msmeno"].ToString();
                //                model[0].Col38 = dtt.Rows[0]["tanno"].ToString();
                //                model[0].Col44 = dtt.Rows[0]["status"].ToString() == "Y" ? "Active" : "Inactive";
                //                model[0].Col45 = tm.Col45;//chkacct
                //                model[0].Col50 = dtt.Rows[0]["swftcd"].ToString();
                //                model[0].Col51 = dtt.Rows[0]["acctno"].ToString();
                //                model[0].Col52 = dtt.Rows[0]["ifsc"].ToString();
                //                if (dtt.Rows[0]["contr"].ToString().Trim() == "Y") model[0].Chk2 = true;
                //                DataTable dtf = new DataTable();
                //                dtf = sgen.getdata(userCode, "select rec_id,file_name,file_path,col1 from file_tab where ref_code1='" + dtt.Rows[0]["vch_num"].ToString() + "' and type in " +
                //                    "('" + tm.Col12 + "','DD" + model[0].Col12 + "') and client_unit_id='" + unitid_mst + "'");
                //                List<Tmodelmain> ltmf = new List<Tmodelmain>();
                //                foreach (DataRow drf in dtf.Rows)
                //                {
                //                    Tmodelmain tmf = new Tmodelmain();
                //                    tmf.Col4 = drf["file_path"].ToString();
                //                    tmf.Col1 = drf["file_name"].ToString();
                //                    tmf.Col2 = drf["col1"].ToString();
                //                    tmf.Col3 = drf["rec_id"].ToString();
                //                    ltmf.Add(tmf);
                //                }
                //                model[0].LTM1 = ltmf;
                //            }
                //            break;
                //        case "FDEL":
                //            string fid = sgen.GetSession(MyGuid, "delid").ToString();
                //            res = sgen.execute_cmd(userCode, "Delete from file_tab WHERE rec_id='" + fid + "' and type='BCD' and substr(vch_num,0,3)='203' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
                //            sgen.SetSession(MyGuid, "delid", null);
                //            if (res)
                //            {
                //                var LTM = model[0].LTM1;
                //                for (int d = 0; d < LTM.Count; d++)
                //                {
                //                    var id = LTM[d].Col3;
                //                    if (id == fid) { model[0].LTM1.RemoveAt(d); }
                //                }
                //            }
                //            break;
                //        case "LSR":
                //            mq = "select from file_tab WHERE client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "' and type='BCD' and substr(vch_num,0,3)='203'";
                //            dtt = sgen.getdata(userCode, mq);
                //            if (dtt.Rows.Count > 0)
                //            {
                //                model[0].Col32 = dtt.Rows[0]["lsrno"].ToString();
                //            }
                //            break;
                //    }
                //    break;
                #endregion
                #region indent_req
                case "indent_req":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "COPYIND":
                            mq = @"select a.*,to_char(convert_tz(vch_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') vch_date1,to_char(convert_tz(dlv_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') dlv_date1" +
                                " from purchases a where (client_id||client_unit_id||vch_num||to_char(vch_date, 'yyyymmdd')||type)='" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                #region getdept
                                TempData[MyGuid + "_Tlist1"] = model[0].TList1 = mod1 = cmd_Fun.dept(userCode, clientid_mst, unitid_mst);
                                #endregion
                                #region getdesig
                                TempData[MyGuid + "_Tlist2"] = model[0].TList2 = mod2 = cmd_Fun.desig(userCode, unitid_mst);
                                #endregion

                                model[0].SelectedItems1 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["deptno"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems2 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["desig"].ToString()).Distinct()).Split(',');
                                if (btnval.ToUpper().Equals("EDIT") || btnval.ToUpper().Equals("VIEW"))
                                {
                                    sgen.SetSession(MyGuid, "EDMODE", "Y");
                                    DataTable dt2 = new DataTable();
                                    model[0].Col1 = dt.Rows[0]["client_id"].ToString();
                                    model[0].Col2 = dt.Rows[0]["client_unit_id"].ToString();
                                    model[0].Col3 = dt.Rows[0]["ent_by"].ToString();
                                    model[0].Col4 = dt.Rows[0]["ent_date"].ToString();
                                    model[0].Col5 = dt.Rows[0]["edit_by"].ToString();
                                    model[0].Col6 = dt.Rows[0]["edit_date"].ToString();
                                    model[0].Col8 = "(client_id||client_unit_id||vch_num||to_char(vch_date, 'yyyymmdd')||type) = '" + URL + "'";
                                    model[0].Col9 = tm.Col9;
                                    model[0].Col10 = tm.Col10;
                                    model[0].Col11 = tm.Col11;
                                    model[0].Col13 = "Update";
                                    model[0].Col100 = "Update & New";
                                    model[0].Col121 = "U";
                                    model[0].Col122 = "<u>U</u>pdate";
                                    model[0].Col123 = "Update & Ne<u>w</u>";
                                    model[0].Col14 = tm.Col14;
                                    model[0].Col15 = tm.Col15;
                                    model[0].Col18 = dt.Rows[0]["vch_num"].ToString();
                                    model[0].Col20 = dt.Rows[0]["vch_date1"].ToString();
                                    model[0].Col21 = dt.Rows[0]["totremark"].ToString();
                                    model[0].Col22 = dt.Rows[0]["reqby"].ToString();
                                    int ver = sgen.Make_int(dt.Rows[0]["ver"].ToString() == "" ? "1" : dt.Rows[0]["ver"].ToString()) + 1;
                                    model[0].Col26 = ver.ToString();
                                }
                                else
                                {
                                    sgen.SetSession(MyGuid, "EDMODE", "N");
                                }
                                model[0].dt1 = model[0].dt1.Clone();
                                if (sgen.GetSession(MyGuid, "dtbase") == null)
                                {
                                    DataTable dttemp = sgen.getdata(userCode, "select '0' as  SNo ,'-' as Icode,'-' as IName,'-' PartNo,'-'  HSN,'-' as UOM,'0' as  Qty_in_stock,'0' pn_ind_qty,'0' pn_po_qty,'0' as Qty_Req,'0' as Exp_Val_Perunit,'-' as Remark from dual");
                                    sgen.SetSession(MyGuid, "dtbase", dttemp);
                                }
                                //else
                                //{
                                //    model[0].dt1 = ((DataTable)Session["dtbase"]).Clone();
                                //}
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["sno"] = i.ToString();
                                    dr["IName"] = dt.Rows[i]["iname"].ToString();
                                    dr["Icode"] = dt.Rows[i]["icode"].ToString();
                                    dr["HSN"] = dt.Rows[i]["hsno"].ToString();
                                    dr["UOM"] = dt.Rows[i]["uom"].ToString();
                                    dr["partno"] = dt.Rows[i]["cpartno"].ToString();
                                    dr["Qty_in_stock"] = dt.Rows[i]["qtystk"].ToString();
                                    dr["pn_ind_qty"] = dt.Rows[i]["TCREMARK"].ToString();
                                    dr["pn_po_qty"] = dt.Rows[i]["TNAME"].ToString();
                                    dr["Qty_Req"] = dt.Rows[i]["qtyord"].ToString();
                                    dr["EXP_VAL_PERUNIT"] = dt.Rows[i]["iamount"].ToString();
                                    dr["Required_Date"] = dt.Rows[i]["dlv_date1"].ToString();
                                    dr["Remark"] = dt.Rows[i]["iremark"].ToString();
                                    model[0].dt1.Rows.Add(dr);
                                }
                            }
                            break;
                        case "PRINT":
                            ViewBag.vnew = "";
                            ViewBag.vedit = "";
                            ViewBag.vsave = "disabled='disabled'";
                            ViewBag.vsavenew = "disabled='disabled'";
                            ViewBag.scripCall = "disableForm();";
                            mq = "select REPLACE(ud.FIRST_NAME|| ' '|| ud.MIDDLE_NAME|| ' '|| ud.last_name,'0','') as created_by, it.cpartno  ,it.iname ,it.partname ," +
                                "um.master_name as UOM,hsn.master_name as hsn,i.TCREMARK as pn_ind_qty,i.tname as pn_po_qty,i.iamount as EXP_VAL_PERUNIT, nvl(lp.vch_num,'-') as lastpo, nvl(lp.qtyord,'-') as poqty, nvl(lp.vch_date,'-') as last_podt," +
                                " i.vch_num as Indent_no,dep.master_name as Dept,des.master_name as desg,to_char" +
                                "(i.vch_date, '" + sgen.Getsqldateformat() + "') as Indent_date,to_char(i.dlv_date,'" + sgen.Getsqldateformat() + "') as Required_Date,i.icode, " +
                                "i.qtystk as Qty_Stock,i.qtyord as Qty_Req,i.totremark from  purchases i " +
                                "inner join item it on i.icode=it.icode  and it.type='IT' and find_in_set(i.client_unit_id,it.client_unit_id)=1 " +
                                "inner join master_setting um on um.master_id=it.uom and um.type='UMM' and find_in_set(um.client_unit_id,'" + unitid_mst + "')=1 " +
                                "inner join master_setting hsn on hsn.master_id=it.hsno and hsn.type='HSN' and find_in_set(hsn.client_unit_id,'" + unitid_mst + "')=1 " +
                                "inner join master_setting dep on dep.master_id = i.deptno and dep.type = 'MDP' and find_in_set(dep.client_unit_id,i.client_unit_id)=1 " +
                                "inner join master_setting des on des.master_id = i.desig and des.type = 'MDG' and find_in_set(des.client_unit_id,i.client_unit_id)=1 " +
                                "left join (select icode ,vch_num,qtyord,to_char(convert_tz(vch_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') vch_date from purchases " +
                                "where client_unit_id='" + unitid_mst + "' and type in ('50', '52') and vch_num in " +
                                "(select max(vch_num) as vch_num from purchases where client_unit_id = '" + unitid_mst + "' and type in ('50', '52') " +
                                "and icode in (select icode from purchases i where i.client_id || i.client_unit_id || i.vch_num || to_char(i.vch_date, 'yyyymmdd') ||" +
                                " i.type = '" + URL + "')))lp on lp.icode = i.icode inner join user_details ud on i.ent_by = ud.vch_num and ud.type = 'CPR'" +
                                " where(i.client_id || i.client_unit_id || i.vch_num || to_char(i.vch_date, 'yyyymmdd') || i.type) = '" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                DataTable qrdt = new DataTable();
                                DataSet ds = new DataSet();
                                qrdt = dt.Copy();
                                qrdt.Columns.Add("qrimg", typeof(Byte[]));
                                foreach (DataRow dr in qrdt.Rows)
                                {
                                    string code = dr["icode"].ToString() + "\n" + dr["iname"].ToString();
                                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                                    QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);
                                    System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
                                    imgBarCode.Height = 150;
                                    imgBarCode.Width = 150;
                                    using (Bitmap bitMap = qrCode.GetGraphic(20))
                                    {
                                        using (MemoryStream ms = new MemoryStream())
                                        {
                                            bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                                            byte[] byteImage = ms.ToArray();
                                            dr["qrimg"] = byteImage;
                                            //imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                                        }
                                        //plBarCode.Controls.Add(imgBarCode);
                                    }
                                }
                                qrdt.AcceptChanges();
                                qrdt.TableName = "prepcur";
                                ds.Tables.Add(qrdt);
                                sgen.open_report_byDs_xml(userCode, ds, "Indent_2", "Indent");
                                ViewBag.scripCall += "showRptnew('Indent Detail');disableForm();";

                                //ViewBag.scripCall = "OpenSingle('../../../../../erp/schoolReports_Rpts/Filter_Report_xml.aspx','95%','900px','Indent');disableForm();";
                            }
                            else
                            {
                                ViewBag.scripCall = "showmsgJS(1, 'Data Not Exist', 2);";
                            }
                            break;
                        case "ITEM":
                            //mq = "select it.icode,it.iname,it.cpartno as partno,um.master_name as UOM,hsn.master_name as hsn,s.closing qty_in_stock,'0' Qty_Req," +
                            //    "'0' Exp_Value,'-' remark from item it " +
                            //    "inner join master_setting um on um.master_id=it.uom and um.type='UMM' and find_in_set(um.client_id,'" + clientid_mst + "')=1 and find_in_set(um.client_unit_id,'" + unitid_mst + "')=1 " +
                            //    "inner join master_setting hsn on hsn.master_id=it.hsno and hsn.type='HSN' and find_in_set(hsn.client_id,'" + clientid_mst + "')=1 and find_in_set(hsn.client_unit_id,'" + unitid_mst + "')=1 " +
                            //    "left join (select t.client_id, t.client_unit_id,t.icode,sum(nvl(t.qtyin,0)) as Received,sum(nvl(t.qtyout,0)) as Issued," +
                            //    "sum(nvl(t.qtyin,0))-sum(nvl(t.qtyout,0)) as closing from itransaction t where trim(t.store)='Y' group by t.icode," +
                            //    "t.client_unit_id,t.client_id ) s on it.icode=s.icode and s.client_id='" + clientid_mst + "' and s.client_unit_id='" + unitid_mst + "' " +
                            //    " where (it.client_id||it.client_unit_id||it.icode||to_char(it.vch_date,'yyyymmdd')||it.type) in ('" + URL + "')";
                            //mq = "select distinct (it.client_id||it.client_unit_id||it.icode||to_char(it.vch_date,'yyyymmdd')||it.type) fstr,'0' Qty_Req,it.icode as Icode,it.iname as Iname,s.closing as qty_in_stock,it.cpartno Partno,um.master_name as UOM,hsn.master_name as hsn,hsn.group_name taxrate,nvl(pc.ind_qty,'0') ind_qty,nvl(pc.Bal_qty,'0') ind_Bal_qty,'0' Exp_Value,'-' remark,nvl(b.po_qty,'0') po_qty,nvl(b.Bal_qty,'0') po_Bal_qty from item it left join(select a.icode, a.client_id, a.client_unit_id, a.ind_qty, sum(a.po_qty) as used,(a.ind_qty - nvl(sum(a.po_qty), '0')) as Bal_qty  from(select nd.icode,nd.client_id,nd.client_unit_id, nvl(nd.qtyord,'0') as ind_qty,nvl(b.qtyord, '0') as po_qty from purchases nd left join purchases b on b.icode = nd.icode and b.type not in ('66') and b.pur_type = '001' and nd.client_id = b.client_id and nd.client_unit_id = b.client_unit_id where nd.type = '66' and nd.client_id = '" + clientid_mst + "' and nd.client_unit_id = " + unitid_mst + " union all select nd.icode, nd.client_id, nd.client_unit_id,nvl(nd.qtyord,'0') as ind_qty, nvl(b.col11, '0') as close_qty from purchases nd left join enx_tab b on b.col7 = nd.icode and b.type = 'CPI' and nd.client_id = b.client_id and nd.client_unit_id = b.client_unit_id where nd.type = '66' and nd.client_id = '" + clientid_mst + "' and nd.client_unit_id = " + unitid_mst + ") a group by(a.icode, a.client_id, a.client_unit_id, a.ind_qty)) pc on pc.icode = it.icode and pc.client_id=it.client_id and pc.client_unit_id=it.client_unit_id left join (select a.icode,a.client_id,a.client_unit_id ,a.qtyord po_qty,(a.qtyord - nvl(sum(a.inv_qty), '0')) as bal_qty from(select po.icode,po.client_id,po.client_unit_id,po.qtyord,nvl(iv.qtyrcvd,'0') as inv_qty from purchases po left join itransaction iv on iv.icode=po.icode and iv.client_id = po.client_id and iv.type='02' and iv.client_unit_id = po.client_unit_id where po.type in ('50','54','52') and po.client_id = '001' and po.client_unit_id = " + unitid_mst + " union all select po.icode,po.client_id,po.client_unit_id,po.qtyord,nvl(to_number(iv.col11),'0') as close_qty from purchases po left join enx_tab iv on iv.col7=po.icode and iv.client_id = po.client_id and iv.type='POR' and iv.client_unit_id = po.client_unit_id where po.type in ('50','54','52') and po.client_id = '001' and po.client_unit_id = " + unitid_mst + ") a group by(a.icode,a.client_id,a.client_unit_id,a.qtyord)) b on b.icode = it.icode and b.client_id=it.client_id and b.client_unit_id = it.client_unit_id inner join master_setting um on um.master_id = it.uom and um.type = 'UMM' and find_in_set(um.client_id,'001')= 1 and find_in_set(um.client_unit_id," + unitid_mst + ")= 1 inner join master_setting hsn on hsn.master_id = it.hsno and hsn.type = 'HSN' and find_in_set(hsn.client_id,'" + clientid_mst + "')= 1 and find_in_set(hsn.client_unit_id, " + unitid_mst + ") = 1" +
                            //    " left join (select t.client_id, t.client_unit_id,t.icode,sum(nvl(t.qtyin,0)) as Received,sum(nvl(t.qtyout,0)) as Issued,sum(nvl(t.qtyin, 0)) - sum(nvl(t.qtyout, 0)) as closing from itransaction t where trim(t.store) = 'Y' group by t.icode,t.client_unit_id,t.client_id ) s on it.icode = s.icode and s.client_id = '" + clientid_mst + "' and s.client_unit_id = '" + unitid_mst + "' " +
                            //    " where(it.client_id || it.client_unit_id || it.icode || to_char(it.vch_date, 'yyyymmdd') || it.type) in ('" + URL + "')";
                            mq = "select (it.icode||to_char(it.vch_date,'yyyymmdd')||it.type) fstr,'0' Qty_Req,'0' Exp_Value,'-' remark,it.icode as " +
                                "Icode,nvl(s.closing,'0') as qty_in_stock,it.iname as Iname,it.cpartno Partno, um.master_name as UOM,hsn.master_name as hsn," +
                                "hsn.group_name taxrate, nvl(pc.ind_bal_qty, '0') ind_bal_qty,nvl(pc.po_bal_qty, '0') po_bal_qty from item it " +
                                "left join(select aa.icode, aa.client_id, aa.client_unit_id, sum(aa.Bal_qty) as ind_bal_qty,sum(aa.po_bal_qty) " +
                                "as po_bal_qty from(select icode, to_number(ind_qty) ind_qty, to_number(Bal_qty) Bal_qty, 0 po_qty, 0 po_bal_qty, " +
                                "client_id, client_unit_id from(select a.icode, a.client_id, a.client_unit_id, nvl(a.ind_qty, '0') as ind_qty," +
                                " (a.ind_qty - nvl(sum(a.po_qty), '0')) as Bal_qty  from(select nd.icode, nd.client_id, nd.client_unit_id, nvl(nd.qtyord, '0') " +
                                "as ind_qty, nvl(b.qtyord, '0') as po_qty from purchases nd left join purchases b on b.icode = nd.icode and b.type not in ('66') " +
                                "and b.pur_type = '001' and nd.client_unit_id = b.client_unit_id " +
                                "where nd.client_unit_id = '" + unitid_mst + "' and nd.type = '66' " +
                                "union all select nd.icode, nd.client_id, nd.client_unit_id, nvl(nd.qtyord, '0') as ind_qty, nvl(b.col11, '0') " +
                                "as close_qty from purchases nd left join enx_tab b on b.col7 = nd.icode and b.type = 'CPI' " +
                                "and nd.client_unit_id = b.client_unit_id " +
                                "where nd.client_unit_id = '" + unitid_mst + "' and nd.type = '66') " +
                                "a group by(a.icode, a.client_id, a.client_unit_id, a.ind_qty)) a union all select icode, 0 ind_qty, 0 Bal_qty, " +
                                "to_number(po_qty), to_number(bal_qty), client_id, client_unit_id from(select a.icode, a.client_id, " +
                                "a.client_unit_id, a.qtyord po_qty, (a.qtyord - nvl(sum(a.inv_qty), '0')) as bal_qty from(select po.icode, " +
                                "po.client_id, po.client_unit_id, po.qtyord, nvl(iv.qtyrcvd, '0') as inv_qty from purchases po " +
                                "left join itransaction iv on iv.icode = po.icode and iv.type = '02' " +
                                "and iv.client_unit_id = po.client_unit_id where" +
                                " po.client_unit_id = '" + unitid_mst + "' and po.type in ('50', '54', '52') union all select po.icode, " +
                                "po.client_id, po.client_unit_id, po.qtyord, nvl(to_number(iv.col11), '0') as close_qty from purchases po " +
                                "left join enx_tab iv on iv.col7 = po.icode and iv.type = 'POR' and " +
                                "iv.client_unit_id = po.client_unit_id where " +
                                "po.client_unit_id = '" + unitid_mst + "' and po.type in ('50', '54', '52')) a group " +
                                "by(a.icode, a.client_id, a.client_unit_id, a.qtyord)) b) aa group by(aa.icode, aa.client_id, aa.client_unit_id)) pc" +
                                " on pc.icode = it.icode and find_in_set(pc.client_id, it.client_id)=1 and find_in_set(pc.client_unit_id, it.client_unit_id)=1 " +
                                "inner join master_setting um on um.master_id = it.uom and um.type = 'UMM' " +
                                "and find_in_set(um.client_unit_id, '" + unitid_mst + "') = 1 inner join master_setting hsn on hsn.master_id = it.hsno and" +
                                " hsn.type = 'HSN' " +
                                "and find_in_set(hsn.client_unit_id, '" + unitid_mst + "') = 1 left join(select icode,client_unit_id,sum(op) op,sum(pktop) pktop,sum(qtyin) qtyin,sum(qtyout) out," +
                                      "sum(op)+sum(qtyin)-sum(qtyout) closing from (select icode, client_unit_id, nvl(op_2019, 0) op," +
                                      " nvl(pkt_2019, 0) pktop, 0 qtyin, 0 qtyout, 0 pktno from itbal union all select icode, client_unit_id, " +
                                      "sum(to_number(qtyin)) - sum(to_number(qtyout)) op, sum((case when substr(type, 1, 1) in ('0', '1') then" +
                                      " pktno when substr(type,1,1) in ('2', '3', '4') then - pktno else pktno end)) pktop,sum(to_number(qtyin)) " +
                                      "qtyin,sum(to_number(qtyout)) qtyout,0 pktno from itransaction where trim(store) = 'Y' and " +
                                      "to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and " +
                                      "to_date('" + year_from.Split(' ')[0].Trim() + "','" + sgen.Getsqldateformat() + "')-1 group by icode,client_unit_id union all select icode,client_unit_id," +
                                      "0 as op,0 as pktop,to_number(qtyin) qtyin,to_number(qtyout) qtyout,sum((case when substr(type, 1, 1) in " +
                                      "('0', '1') then pktno when substr(type,1,1) in ('2', '3', '4') then - pktno else pktno end)) pktno from " +
                                      "itransaction where trim(store) = 'Y' and to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                                      "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + model[0].Col20.Trim() + "','" + sgen.Getsqldateformat() + "') group by icode,client_unit_id," +
                                      "to_number(qtyin),to_number(qtyout)) a group by icode, client_unit_id) s on it.icode = s.icode " +
                                "and s.client_unit_id = '" + unitid_mst + "' where" +
                                " (it.icode || to_char(it.vch_date, 'yyyymmdd') || it.type) in ('" + URL + "') ";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                if (model[0].dt1.Rows[0]["icode"].ToString().Trim().Equals("-")) { model[0].dt1.Rows.RemoveAt(0); }
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["SNo"] = i + 1;
                                    dr["Icode"] = dt.Rows[i]["icode"].ToString();
                                    dr["IName"] = dt.Rows[i]["iname"].ToString();
                                    dr["PARTNO"] = dt.Rows[i]["partno"].ToString();
                                    dr["HSN"] = dt.Rows[i]["hsn"].ToString();
                                    dr["UOM"] = dt.Rows[i]["uom"].ToString();
                                    dr["Qty_in_stock"] = dt.Rows[i]["qty_in_stock"].ToString();
                                    dr["pn_ind_qty"] = dt.Rows[i]["ind_bal_qty"].ToString();
                                    dr["pn_po_qty"] = dt.Rows[i]["po_bal_qty"].ToString();
                                    dr["Qty_Req"] = dt.Rows[i]["Qty_Req"].ToString();
                                    dr["Exp_Val_Perunit"] = dt.Rows[i]["Exp_Value"].ToString();
                                    dr["remark"] = dt.Rows[i]["remark"].ToString();
                                    dr["Required_Date"] = sgen.server_datetime_local(userCode);
                                    model[0].dt1.Rows.Add(dr);
                                }
                            }
                            break;
                    }
                    break;
                #endregion
                #region store_issue
                case "store_issue":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = @"select (i.client_id||i.client_unit_id||i.vch_num,to_char(i.vch_date,'yyyymmdd')||i.type) as fstr,
i.vch_num as Indent_No,to_char(convert_tz(i.date1, 'UTC', '" + sgen.Gettimezone() + "'), " + "'" + sgen.Getsqldateformat() + "') as date1," +
"i.department,d.master_name as Dept_name,i.designation,dg.master_name as desig,i.remark,i.i_type,i.iname,i_descrption,i.i_remark,i.iqty_chl," +
"qty_bal,iqty_out,iqty_in,iamount as Exp_value,i.i_remark,i.uom,i.icode,i.client_id,i.client_unit_id,i.ent_by,i.ent_date,i.rec_id,i.vch_num,i.edit_by,i.edit_date,i.hsn from itransaction i " +
"inner join master_setting d on d.master_id=i.department and d.type='MDP' " +
"inner join master_setting dg on  dg.master_id  = i.designation and dg.type='MDG' " +
"where (i.client_id|| i.client_unit_id|| i.vch_num|| to_char(i.vch_date, 'yyyymmdd')|| i.type)='" + URL + "' ";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                //DataTable dtm = sgen.settable(hftable);
                                //model[0].dt1 = dtm;
                                DataTable dt2 = new DataTable();
                                #region getdept
                                TempData[MyGuid + "_Tlist1"] = model[0].TList1 = mod1 = cmd_Fun.dept(userCode, clientid_mst, unitid_mst);
                                #endregion
                                #region getdesig
                                TempData[MyGuid + "_Tlist2"] = model[0].TList2 = mod2 = cmd_Fun.desig(userCode, unitid_mst);
                                #endregion
                                model[0].Col1 = dt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dt.Rows[0]["ent_date"].ToString();
                                model[0].Col5 = dt.Rows[0]["edit_by"].ToString();
                                model[0].Col6 = dt.Rows[0]["edit_date"].ToString();
                                model[0].Col7 = dt.Rows[0]["rec_id"].ToString();
                                model[0].Col8 = "(i.client_id|| i.client_unit_id|| i.vch_num|| to_char(i.vch_date, 'yyyymmdd')|| i.type) = '" + URL + "'";
                                model[0].Col9 = tm.Col9;
                                model[0].Col10 = tm.Col10;
                                model[0].Col11 = tm.Col11;
                                model[0].Col13 = "Update";
                                model[0].Col121 = "U";
                                model[0].Col122 = "<u>U</u>pdate";
                                model[0].Col123 = "Update & Ne<u>w</u>";
                                model[0].Col14 = tm.Col14;
                                model[0].Col15 = tm.Col15;
                                model[0].Col18 = dt.Rows[0]["vch_num"].ToString();
                                //model[0].Col19 = dt.Rows[0]["indent_no"].ToString();
                                model[0].Col20 = dt.Rows[0]["date1"].ToString();
                                model[0].Col21 = dt.Rows[0]["remark"].ToString();
                                model[0].SelectedItems1 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["department"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems2 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["designation"].ToString()).Distinct()).Split(',');
                                model[0].dt1 = new DataTable();
                                model[0].dt1 = ((DataTable)sgen.GetSession(MyGuid, "dtbase")).Clone();
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["sno"] = i.ToString();
                                    dr["I_Name"] = dt.Rows[i]["iname"].ToString();
                                    dr["Icode"] = dt.Rows[i]["icode"].ToString();
                                    dr["HSN"] = dt.Rows[i]["hsn"].ToString();
                                    dr["Qty_in_stock"] = dt.Rows[i]["iqty_chl"].ToString();
                                    dr["Qty_Req"] = dt.Rows[i]["iqty_in"].ToString();
                                    dr["Exp_Value"] = dt.Rows[i]["Exp_Value"].ToString();
                                    dr["Remark"] = dt.Rows[i]["i_remark"].ToString();
                                    dr["UOM"] = dt.Rows[i]["uom"].ToString();
                                    dr["Qty_issue"] = dt.Rows[i]["iqty_out"].ToString();
                                    model[0].dt1.Rows.Add(dr);
                                }
                                //model[0].dt1 = dt;
                            }
                            break;
                    }
                    break;
                #endregion
                #region po_withindent
                case "po_withindent":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                        case "COPYPO":
                            mq = "select a.*,to_char(convert_tz(vch_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') vch_date1," +
                                "to_char(dlv_date,'" + sgen.Getsqldateformat() + "') dlv_date1," +
                                "to_char(convert_tz(inddate,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') inddate1," +
                                "to_char(convert_tz(WRTDATE,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') WRTDATE1," +
                                "to_char(convert_tz(vch_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') date11," +
                                "to_char(convert_tz(vch_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') date21," +
                                "to_char(convert_tz(vch_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') date31 from purchases a " +
                                "where (client_id||client_unit_id||vch_num||to_char(vch_date, 'yyyymmdd')|| type) = '" + URL + "'";
                            dtp = sgen.getdata(userCode, mq);
                            if (dtp.Rows.Count > 0)
                            {
                                #region ddl
                                TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1 = cmd_Fun.payterm(userCode, unitid_mst, out defcall);              //payterm
                                TempData[MyGuid + "_TList2"] = model[0].TList2 = mod2 = cmd_Fun.priceterm(userCode, unitid_mst);           //priceterm
                                TempData[MyGuid + "_TList3"] = model[0].TList3 = mod3 = cmd_Fun.transportmode(userCode, unitid_mst);       //transport 
                                TempData[MyGuid + "_TList4"] = model[0].TList4 = mod4 = cmd_Fun.Modeofpayment(userCode, unitid_mst);       //payment mode 
                                TempData[MyGuid + "_TList5"] = model[0].TList5 = mod5 = cmd_Fun.insby(userCode, unitid_mst);               //insby 
                                TempData[MyGuid + "_TList6"] = model[0].TList6 = mod6 = cmd_Fun.delterm(userCode, unitid_mst);             //delterm 
                                TempData[MyGuid + "_TList7"] = model[0].TList7 = mod7 = cmd_Fun.curname(userCode, unitid_mst);             //curname

                                String[] L1 = System.String.Join(",", dtp.Rows.OfType<DataRow>().Select(r => r["pay_term"].ToString()).Distinct()).Split(',');
                                String[] L2 = System.String.Join(",", dtp.Rows.OfType<DataRow>().Select(r => r["price_term"].ToString()).Distinct()).Split(',');
                                String[] L3 = System.String.Join(",", dtp.Rows.OfType<DataRow>().Select(r => r["tmode"].ToString()).Distinct()).Split(',');
                                String[] L4 = System.String.Join(",", dtp.Rows.OfType<DataRow>().Select(r => r["paymode"].ToString()).Distinct()).Split(',');
                                String[] L5 = System.String.Join(",", dtp.Rows.OfType<DataRow>().Select(r => r["insby"].ToString()).Distinct()).Split(',');
                                String[] L6 = System.String.Join(",", dtp.Rows.OfType<DataRow>().Select(r => r["delterm"].ToString()).Distinct()).Split(',');
                                String[] L7 = System.String.Join(",", dtp.Rows.OfType<DataRow>().Select(r => r["doccur"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems1 = L1;
                                model[0].SelectedItems2 = L2;
                                model[0].SelectedItems3 = L3;
                                model[0].SelectedItems4 = L4;
                                model[0].SelectedItems5 = L5;
                                model[0].SelectedItems6 = L6;
                                model[0].SelectedItems7 = L7;
                                #endregion
                                model[0].Col56 = dtp.Rows[0]["tname"].ToString();
                                model[0].Col57 = dtp.Rows[0]["pkins"].ToString();
                                model[0].Col58 = dtp.Rows[0]["tcstatus"].ToString();
                                model[0].Col84 = dtp.Rows[0]["IS_WARNT"].ToString();
                                if (model[0].Col84.Trim().Equals("Y")) model[0].Col85 = dtp.Rows[0]["WRTDATE1"].ToString();
                                else model[0].Col85 = "";
                                model[0].Col89 = dtp.Rows[0]["tcremark"].ToString();
                                model[0].Col67 = dtp.Rows[0]["gstcr"].ToString();
                                model[0].Col68 = dtp.Rows[0]["gstcrremark"].ToString();
                                model[0].Col70 = dtp.Rows[0]["col1"].ToString();
                                model[0].Col71 = dtp.Rows[0]["col2"].ToString();
                                model[0].Col72 = dtp.Rows[0]["col3"].ToString();
                                model[0].Col73 = dtp.Rows[0]["date11"].ToString();
                                model[0].Col74 = dtp.Rows[0]["date21"].ToString();
                                model[0].Col75 = dtp.Rows[0]["date31"].ToString();
                                model[0].Col76 = dtp.Rows[0]["col4"].ToString();
                                model[0].Col77 = dtp.Rows[0]["col5"].ToString();
                                model[0].Col78 = dtp.Rows[0]["col6"].ToString();
                                model[0].Col60 = dtp.Rows[0]["pref"].ToString();
                                model[0].Col61 = dtp.Rows[0]["taxre"].ToString();
                                model[0].Col62 = dtp.Rows[0]["shpfval"].ToString();
                                model[0].Col63 = dtp.Rows[0]["shptval"].ToString();
                                model[0].Col79 = sgen.seekval(userCode, "select upper(master_name) master_name from (select '001' fstr,'001' master_id,'With Indent' master_name from dual union all " +
                   "select '002' fstr,'002' master_id,'Without Indent' master_name from dual) a where fstr='" + dtp.Rows[0]["pur_type"].ToString().Trim() + "'", "master_name");
                                model[0].Col31 = dtp.Rows[0]["totremark"].ToString();
                                model[0].Col32 = dtp.Rows[0]["cond"].ToString();
                                model[0].Col33 = dtp.Rows[0]["basicamt"].ToString();
                                model[0].Col34 = dtp.Rows[0]["igst"].ToString();
                                model[0].Col35 = dtp.Rows[0]["cgst"].ToString();
                                model[0].Col36 = dtp.Rows[0]["sgst"].ToString();
                                model[0].Col37 = dtp.Rows[0]["gamt"].ToString();
                                model[0].Col38 = dtp.Rows[0]["gigst"].ToString();
                                model[0].Col39 = dtp.Rows[0]["gcgst"].ToString();
                                model[0].Col40 = dtp.Rows[0]["gsgst"].ToString();
                                model[0].Col69 = dtp.Rows[0]["netamt"].ToString();
                                model[0].Col82 = dtp.Rows[0]["vdst"].ToString();                               

                                int ver = sgen.Make_int(dtp.Rows[0]["ver"].ToString() == "" ? "1" : dtp.Rows[0]["ver"].ToString()) + 1;
                                model[0].Col41 = ver.ToString();
                                DataTable dte = new DataTable();
                                mq1 = "select a.c_name as name,a.c_gstin as gstin,a.addr,a.vch_num,a.tor,a.comp_email,a.trcom from clients_mst a where find_in_set(a.client_unit_id,'" + unitid_mst + "')=1 and a.type='BCD' and a.vch_num='" + dtp.Rows[0]["acode"].ToString() + "' and substr(a.vch_num,0,3)='203'";
                                dte = sgen.getdata(userCode, mq1);
                                if (dte.Rows.Count > 0)
                                {
                                    model[0].Col49 = dte.Rows[0]["vch_num"].ToString();
                                    model[0].Col20 = dte.Rows[0]["name"].ToString();
                                    model[0].Col21 = dte.Rows[0]["addr"].ToString();
                                    model[0].Col22 = dte.Rows[0]["gstin"].ToString();
                                    model[0].Col52 = dte.Rows[0]["tor"].ToString();
                                    model[0].Col86 = dte.Rows[0]["comp_email"].ToString();
                                    model[0].Col87 = dte.Rows[0]["trcom"].ToString();
                                }
                                mq1 = "select a.c_name as name,a.c_gstin as gstin,a.addr,a.vch_num from clients_mst a where " +
                                    "find_in_set(a.client_unit_id,'" + unitid_mst + "')=1 and a.type='BCD' and a.vch_num='" + dtp.Rows[0]["shipfrom"].ToString() + "' " +
                                  " and substr(a.vch_num,0,3)='203'";
                                dte = sgen.getdata(userCode, mq1);
                                if (dte.Rows.Count > 0)
                                {
                                    model[0].Col50 = dte.Rows[0]["vch_num"].ToString();
                                    model[0].Col23 = dte.Rows[0]["name"].ToString();
                                    model[0].Col24 = dte.Rows[0]["addr"].ToString();
                                    model[0].Col25 = dte.Rows[0]["gstin"].ToString();
                                }
                                mq1 = "select a.c_name as name,a.c_gstin as gstin,a.addr,a.vch_num from clients_mst a where find_in_set(a.client_unit_id,'" + unitid_mst + "')=1 and a.type='BCD' and a.vch_num='" + dtp.Rows[0]["shipto"].ToString() + "' and substr(a.vch_num,0,3)='203'";
                                dte = sgen.getdata(userCode, mq1);
                                if (dte.Rows.Count > 0)
                                {
                                    model[0].Col51 = dte.Rows[0]["vch_num"].ToString();
                                    model[0].Col26 = dte.Rows[0]["name"].ToString();
                                    model[0].Col27 = dte.Rows[0]["addr"].ToString();
                                    model[0].Col28 = dte.Rows[0]["gstin"].ToString();
                                }
                                if (btnval.ToUpper().Equals("EDIT") || btnval.ToUpper().Equals("VIEW"))
                                {
                                    sgen.SetSession(MyGuid, "EDMODE", "Y");
                                    model[0].Col1 = dtp.Rows[0]["client_id"].ToString();
                                    model[0].Col2 = dtp.Rows[0]["client_unit_id"].ToString();
                                    model[0].Col3 = dtp.Rows[0]["ent_by"].ToString();
                                    model[0].Col4 = dtp.Rows[0]["ent_date"].ToString();
                                    model[0].Col5 = dtp.Rows[0]["edit_by"].ToString();
                                    model[0].Col6 = dtp.Rows[0]["edit_date"].ToString();
                                    model[0].Col8 = "(client_id||client_unit_id||vch_num||to_char(vch_date, 'yyyymmdd')|| type) = '" + URL + "'";
                                    model[0].Col9 = sgen.seekval(userCode, "select upper(master_name) master_name from master_setting where master_id='" + dtp.Rows[0]["type"].ToString() + "' and type='KPO'", "master_name");
                                    model[0].Col10 = model[0].Col10;
                                    model[0].Col11 = model[0].Col11;
                                    model[0].Col12 = dtp.Rows[0]["type"].ToString();
                                    model[0].Col13 = "Update";
                                    model[0].Col100 = "Update & New";
                                    model[0].Col121 = "U";
                                    model[0].Col122 = "<u>U</u>pdate";
                                    model[0].Col123 = "Update & Ne<u>w</u>";
                                    model[0].Col14 = model[0].Col14;
                                    model[0].Col15 = model[0].Col15;
                                    model[0].Col16 = dtp.Rows[0]["vch_num"].ToString();
                                    model[0].Col47 = dtp.Rows[0]["vch_num"].ToString();
                                    model[0].Col48 = dtp.Rows[0]["vch_date1"].ToString();
                                    model[0].Col29 = dtp.Rows[0]["pur_type"].ToString();
                                    if (btnval.ToUpper().Equals("VIEW"))
                                    {
                                        ViewBag.vnew = "disabled='disabled'";
                                        ViewBag.vedit = "disabled='disabled'";
                                        ViewBag.vsave = "disabled='disabled'";
                                        ViewBag.vsavenew = "disabled='disabled'";
                                        ViewBag.scripCall += "disableForm();";
                                    }
                                }
                                else
                                {
                                    sgen.SetSession(MyGuid, "EDMODE", "N");
                                }
                                DataTable dtf = new DataTable();
                                if (dtp.Rows[0]["type"].ToString().Trim().Equals("50") || dtp.Rows[0]["type"].ToString().Trim().Equals("52") || dtp.Rows[0]["type"].ToString().Trim().Equals("54"))
                                {
                                    if (dtp.Rows[0]["pur_type"].ToString().Equals("002"))
                                    {
                                        dtt = sgen.getdata(userCode, "select '' Item,'1' as  SNo ,'-' as Icode,'-' as IName,'-' PARTNO,'-'  HSN,'-' as UOM,'0' as TaxRate," +
                                            "'0' Qty_in_stock,'0' Qty_Ord,'0' IRate,'0' disc_rate,'0' IAmount,'0' TaxAmount,'0' discAmount,'0' LineAmount,'-' delivery_date," +
                                            "'-' as Remark from dual");
                                        model[0].dt1 = dtt;
                                        dtf = model[0].dt1.Clone();
                                        sgen.SetSession(MyGuid, "KPO_DT", dtt);
                                    }
                                    else
                                    {
                                        dtt = sgen.getdata(userCode, "select '' Item,'1' SNo ,'-' as Icode,'-' as IName,'-' PARTNO,'-'  HSN,'-' as UOM,'0' as TaxRate," +
                                            "'0' as Qty_in_stock,'0' as Qty_Ord,'0' Qty_Req,'0' Qty_bal,'0' as IRate,'0' as disc_rate,'0' IAmount,'0' TaxAmount,'0' discAmount," +
                                            "'0' LineAmount,'-' as delivery_date,'-' as Remark,'-' Indentno,'-' Indent_Date from dual");
                                        model[0].dt2 = dtt;
                                        dtf = model[0].dt2.Clone();
                                        sgen.SetSession(MyGuid, "KPO_DT", dtt);
                                    }
                                }
                                //else
                                //{
                                //    dtt = sgen.getdata(userCode, "select '' Item,'1' as  SNo ,'-' as Icode,'-' as IName,'-' PARTNO,'-'  HSN,'-' as UOM,'0' as TaxRate," +
                                //           "'0' Qty_in_stock,'0' Qty_Ord,'0' IRate,'0' disc_rate,'0' IAmount,'0' TaxAmount,'0' discAmount,'0' LineAmount,'-' delivery_date," +
                                //           "'-' as Remark from dual");
                                //    model[0].dt1 = dtt;
                                //    dtf = model[0].dt2.Clone();
                                //    sgen.SetSession(MyGuid, "KPO_DT", dtt);
                                //}
                                for (int i = 0; i < dtp.Rows.Count; i++)
                                {
                                    DataRow dr = dtf.NewRow();
                                    dr["SNo"] = i + 1;
                                    dr["Icode"] = dtp.Rows[i]["icode"].ToString();
                                    dr["IName"] = dtp.Rows[i]["iname"].ToString();
                                    dr["PARTNO"] = dtp.Rows[i]["cpartno"].ToString();
                                    dr["HSN"] = dtp.Rows[i]["hsno"].ToString();
                                    dr["UOM"] = dtp.Rows[i]["uom"].ToString();
                                    dr["TaxRate"] = dtp.Rows[i]["taxrate"].ToString() + "%";
                                    dr["Qty_in_stock"] = dtp.Rows[i]["qtystk"].ToString();
                                    if (dtp.Rows[0]["pur_type"].ToString().Equals("001"))
                                    {
                                        dr["Qty_ord"] = dtp.Rows[i]["qtyind"].ToString();
                                        dr["Qty_Req"] = dtp.Rows[i]["qtyord"].ToString();
                                        dr["Qty_Bal"] = dtp.Rows[i]["qtybal"].ToString();
                                        dr["IndentNo"] = dtp.Rows[i]["indno"].ToString();
                                        dr["Indent_Date"] = dtp.Rows[i]["inddate1"].ToString();
                                    }
                                    else
                                    {
                                        dr["Qty_Ord"] = dtp.Rows[i]["qtyord"].ToString();
                                    }
                                    dr["IRate"] = dtp.Rows[i]["irate"].ToString();
                                    dr["disc_rate"] = dtp.Rows[i]["discrate"].ToString();
                                    dr["Iamount"] = dtp.Rows[i]["Iamount"].ToString();
                                    dr["taxamount"] = dtp.Rows[i]["taxamt"].ToString();
                                    dr["discamount"] = dtp.Rows[i]["discamt"].ToString();
                                    dr["lineamount"] = dtp.Rows[i]["lineamount"].ToString();
                                    dr["delivery_date"] = dtp.Rows[i]["dlv_date1"].ToString();
                                    dr["Remark"] = dtp.Rows[i]["iremark"].ToString();
                                    dtf.Rows.Add(dr);
                                }
                                dtf.AcceptChanges();
                                if (dtp.Rows[0]["type"].ToString().Trim().Equals("50") || dtp.Rows[0]["type"].ToString().Trim().Equals("52") || dtp.Rows[0]["type"].ToString().Trim().Equals("54"))
                                {
                                    if (dtp.Rows[0]["pur_type"].ToString().Equals("002")) { model[0].dt1 = dtf; }
                                    else { model[0].dt2 = dtf; }
                                }
                                //else { model[0].dt1 = dtf; }
                                //other charges LTM
                                mq1 = "select ch.master_name chrgname,en.col1,en.col2,en.col3,en.col4,en.col5,en.col6,en.col7,en.col8,en.col9,en.col10,en.r_no from enx_tab en " +
                                    "inner join master_setting ch on en.col1=ch.master_id and ch.type='MR0' and find_in_set(ch.client_unit_id,en.client_unit_id)=1 " +
                                    "where (en.client_id||en.client_unit_id||en.vch_num||to_char(en.vch_date, 'yyyymmdd')|| en.type) = '" + URL + "'";
                                dt = sgen.getdata(userCode, mq1);
                                if (dt.Rows.Count > 0)
                                {
                                    model[0].LTM1 = new List<Tmodelmain>();
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        model[0].LTM1.Add(new Tmodelmain
                                        {
                                            Col13 = dr["col3"].ToString(),//chrgrtid
                                            Col14 = dr["r_no"].ToString(),//rno
                                            Col15 = dr["col1"].ToString(),//chrgid
                                            Col16 = dr["col2"].ToString(),//chrgamt
                                            Col17 = dr["col4"].ToString(),//igstrt
                                            Col18 = dr["col5"].ToString(),//igstamt
                                            Col19 = dr["col6"].ToString(),//cgstrt
                                            Col20 = dr["col7"].ToString(),//cgstamt
                                            Col21 = dr["col8"].ToString(),//sgstrt
                                            Col22 = dr["col9"].ToString(),//sgstamt
                                            Col23 = dr["chrgname"].ToString(),//chrgname
                                            Col25 = dr["col10"].ToString(),//chrgrate
                                        });
                                    }
                                }
                            }
                            break;
                        case "NEW":
                            mq = "select * from (select '001' fstr,'001' master_id,'With Indent' master_name from dual union all select '002' fstr,'002' master_id," +
                                "'Without Indent' master_name from dual) a where fstr='" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model[0].Col29 = dt.Rows[0]["fstr"].ToString();
                                model[0].Col79 = dt.Rows[0]["master_name"].ToString().ToUpper();
                                Make_query(viewName.ToLower(), "Select PO Type", "POTYPE", "1", "", "", MyGuid);
                                ViewBag.scripCall = "callFoo('Select PO Type');";
                            }
                            break;
                        case "POTYPE":
                            mq = "select master_id,upper(master_name) master_name,to_char(convert_tz(utc_timestamp(),'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') po_date " +
                                "from master_Setting where (master_id||to_char(vch_date, 'yyyymmdd')||type) = '" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "N");
                                ViewBag.vnew = "disabled='disabled'";
                                ViewBag.vedit = "disabled='disabled'";
                                ViewBag.vsave = "";
                                ViewBag.vcalc = "";
                                ViewBag.scripCall = "enableForm();";
                                mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + dt.Rows[0]["master_id"].ToString() + "'" + model[0].Col11.Trim() + "";
                                vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                                model[0].Col47 = vch_num;
                                model[0].Col16 = vch_num;
                                model[0].Col48 = dt.Rows[0]["po_date"].ToString();
                                model[0].Col12 = dt.Rows[0]["master_id"].ToString();
                                model[0].Col17 = "1";
                                model[0].Col9 = dt.Rows[0]["master_name"].ToString();
                                model[0].Col58 = "N";
                                model[0].Col67 = "Y";
                                var loccur = sgen.GetSession(MyGuid, "LOCALCUR").ToString();
                                #region ddl
                                TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1 = cmd_Fun.payterm(userCode, unitid_mst, out defcall);
                                TempData[MyGuid + "_TList2"] = model[0].TList2 = mod2 = cmd_Fun.priceterm(userCode, unitid_mst);
                                TempData[MyGuid + "_TList3"] = model[0].TList3 = mod3 = cmd_Fun.transportmode(userCode, unitid_mst);
                                TempData[MyGuid + "_TList4"] = model[0].TList4 = mod4 = cmd_Fun.Modeofpayment(userCode, unitid_mst);
                                TempData[MyGuid + "_TList5"] = model[0].TList5 = mod5 = cmd_Fun.insby(userCode, unitid_mst);
                                TempData[MyGuid + "_TList6"] = model[0].TList6 = mod6 = cmd_Fun.delterm(userCode, unitid_mst);
                                TempData[MyGuid + "_TList7"] = model[0].TList7 = mod7 = cmd_Fun.curname(userCode, unitid_mst);

                                mod8.Add(new SelectListItem { Text = "Fixed", Value = "001" });
                                mod8.Add(new SelectListItem { Text = "Fixed Qty Open Rate", Value = "002" });
                                mod8.Add(new SelectListItem { Text = "Open Qty Fixed Rate", Value = "003" });

                                TempData[MyGuid + "_TList8"] = model[0].TList8 = mod8;
                                model[0].SelectedItems1 = new string[] { "" };
                                model[0].SelectedItems2 = new string[] { "" };
                                model[0].SelectedItems3 = new string[] { "" };
                                model[0].SelectedItems4 = new string[] { "" };
                                model[0].SelectedItems5 = new string[] { "" };
                                model[0].SelectedItems6 = new string[] { "" };
                                model[0].SelectedItems7 = new string[] { loccur };
                                model[0].SelectedItems8 = new string[] { "" };
                                #endregion
                                if (dt.Rows[0]["master_id"].ToString().Trim().Equals("50") || dt.Rows[0]["master_id"].ToString().Trim().Equals("52") || dt.Rows[0]["master_id"].ToString().Trim().Equals("54"))
                                //if (dt.Rows[0]["master_id"].ToString().Trim().Equals("51") || dt.Rows[0]["master_id"].ToString().Trim().Equals("53"))
                                {
                                    if (model[0].Col29 == "002")
                                    {
                                        dtt = sgen.getdata(userCode, "select '' Item,'1' as  SNo ,'-' as Icode,'-' as IName,'-' PARTNO,'-'  HSN,'-' as UOM,'0' as TaxRate," +
                                            "'0' Qty_in_stock,'0' Qty_Ord,'0' IRate,'0' disc_rate,'0' IAmount,'0' TaxAmount,'0' discAmount,'0' LineAmount,'-' delivery_date," +
                                            "'-' as Remark from dual");
                                        model[0].dt1 = dtt;
                                        sgen.SetSession(MyGuid, "KPO_DT", dtt);
                                    }
                                    else
                                    {
                                        dtt = sgen.getdata(userCode, "select '' Item,'1' SNo ,'-' as Icode,'-' as IName,'-' PARTNO,'-'  HSN,'-' as UOM,'0' as TaxRate," +
                                            "'0' as Qty_in_stock,'0' as Qty_Ord,'0' Qty_Req,'0' Qty_bal,'0' as IRate,'0' as disc_rate,'0' IAmount,'0' TaxAmount,'0' discAmount," +
                                            "'0' LineAmount,'-' as delivery_date,'-' as Remark,'-' Indentno,'-' Indent_Date from dual");
                                        model[0].dt2 = dtt;
                                        sgen.SetSession(MyGuid, "KPO_DT", dtt);
                                    }
                                }
                                //else
                                //{
                                //    dtt = sgen.getdata(userCode, "select '' Item,'1' as  SNo ,'-' as Icode,'-' as IName,'-' PARTNO,'-'  HSN,'-' as UOM,'0' as TaxRate," +
                                //           "'0' Qty_in_stock,'0' Qty_Ord,'0' IRate,'0' disc_rate,'0' IAmount,'0' TaxAmount,'0' discAmount,'0' LineAmount,'-' delivery_date," +
                                //           "'-' as Remark from dual");
                                //    model[0].dt1 = dtt;
                                //    sgen.SetSession(MyGuid, "KPO_DT", dtt);
                                //}
                            }
                            break;
                        case "PARTY":
                            mq = "select a.c_name name,a.c_gstin gstin,a.addr,a.vch_num,a.tor,a.pay_term2,a.pay_mode2,a.trcom,a.comp_email from clients_mst a " +
                                "where (a.vch_num||to_char(a.vch_date, 'yyyymmdd')|| a.type) = '" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model[0].Col49 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col20 = dt.Rows[0]["name"].ToString();
                                model[0].Col21 = dt.Rows[0]["addr"].ToString();
                                model[0].Col22 = dt.Rows[0]["gstin"].ToString();
                                if (dt.Rows[0]["gstin"].ToString().Trim().ToUpper().Equals("NOT REGISTERED"))
                                {
                                    model[0].Col67 = "N";
                                }
                                model[0].Col52 = dt.Rows[0]["tor"].ToString();
                                model[0].Col86 = dt.Rows[0]["comp_email"].ToString();
                                model[0].Col87 = dt.Rows[0]["trcom"].ToString();
                                model[0].SelectedItems1 = new string[] { dt.Rows[0]["pay_term2"].ToString() };
                                model[0].SelectedItems4 = new string[] { dt.Rows[0]["pay_mode2"].ToString() };
                            }
                            break;
                        case "PARTYFRM":
                            mq = "select a.c_name as name,a.c_gstin as gstin,a.addr,a.vch_num from clients_mst a " +
                                "where (a.vch_num||to_char(a.vch_date, 'yyyymmdd')|| a.type) = '" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model[0].Col50 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col23 = dt.Rows[0]["name"].ToString();
                                model[0].Col24 = dt.Rows[0]["addr"].ToString();
                                model[0].Col25 = dt.Rows[0]["gstin"].ToString();
                            }
                            break;
                        case "PARTYTO":
                            mq = "select a.c_name as name,a.c_gstin as gstin,a.addr,a.vch_num from clients_mst a " +
                                "where (a.vch_num||to_char(a.vch_date, 'yyyymmdd')|| a.type) = '" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model[0].Col51 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col26 = dt.Rows[0]["name"].ToString();
                                model[0].Col27 = dt.Rows[0]["addr"].ToString();
                                model[0].Col28 = dt.Rows[0]["gstin"].ToString();
                            }
                            break;
                        case "ITEM":
                            if (sgen.GetSession(MyGuid, "Ptype") != null)
                            {
                                if (sgen.GetSession(MyGuid, "Ptype").ToString() == "002")
                                {
                                    mq = "select '-' fstr,it.icode,it.iname,it.cpartno as partno,TRIM(dbms_lob.substr(it.description,4000,1)) description,um.master_name as UOM,hsn.master_name as hsn,hsn.group_name taxrate" +
                                        ",s.closing as qtystk,'0' qtyord from item it " +
                                    "inner join master_setting um on um.master_id=it.uom and um.type='UMM' and find_in_set(um.client_unit_id,'" + unitid_mst + "')=1 " +
                                    "inner join master_setting hsn on hsn.master_id=it.hsno and hsn.type='HSN' and find_in_set(hsn.client_unit_id,'" + unitid_mst + "')=1 " +
                                    "left join(select icode,client_unit_id,sum(op) op,sum(pktop) pktop,sum(qtyin) qtyin,sum(qtyout) out," +
                                      "sum(nvl(op,0))+sum(nvl(qtyin,0))-sum(nvl(qtyout,0)) closing from (select icode, client_unit_id, nvl(op_2019, 0) op," +
                                      " nvl(pkt_2019, 0) pktop, 0 qtyin, 0 qtyout, 0 pktno from itbal union all select icode, client_unit_id, " +
                                      "sum(to_number(qtyin)) - sum(to_number(qtyout)) op, sum((case when substr(type, 1, 1) in ('0', '1') then" +
                                      " pktno when substr(type,1,1) in ('2', '3', '4') then - pktno else pktno end)) pktop,sum(to_number(qtyin)) " +
                                      "qtyin,sum(to_number(qtyout)) qtyout,0 pktno from itransaction where trim(store) = 'Y' and " +
                                      "to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and " +
                                      "to_date('" + year_from.Split(' ')[0].Trim() + "','" + sgen.Getsqldateformat() + "')-1 group by icode,client_unit_id union all select icode,client_unit_id," +
                                      "0 as op,0 as pktop,to_number(qtyin) qtyin,to_number(qtyout) qtyout,sum((case when substr(type, 1, 1) in " +
                                      "('0', '1') then pktno when substr(type,1,1) in ('2', '3', '4') then - pktno else pktno end)) pktno from " +
                                      "itransaction where trim(store) = 'Y' and to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                                      "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + model[0].Col48.Trim() + "','" + sgen.Getsqldateformat() + "') group by icode,client_unit_id," +
                                      "to_number(qtyin),to_number(qtyout)) a group by icode, client_unit_id) s on it.icode = s.icode and find_in_set(s.client_unit_id, it.client_unit_id)=1 where (it.icode||to_char(it.vch_date,'yyyymmdd')||it.type) in ('" + URL + "')";
                                }
                                else if (sgen.GetSession(MyGuid, "Ptype").ToString() == "001")
                                {
                                    // mq = "select (it.client_id||it.client_unit_id||it.icode||to_char(it.vch_date,'yyyymmdd')||it.type||ind.ind_no||to_char(ind.ind_date,'yyyymmdd')) fstr,it.icode as Icode," +
                                    // "it.iname as Iname,it.cpartno Partno,um.master_name as UOM,hsn.master_name as hsn,hsn.group_name taxrate,'0' qtystk,ind.qtyord,ind.ind_no," +
                                    // "to_char(convert_tz(ind.ind_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') ind_date from item it " +
                                    //"inner join master_setting um on um.master_id=it.uom and um.type='UMM' and find_in_set(um.client_id,'" + clientid_mst + "')=1 and find_in_set(um.client_unit_id,'" + unitid_mst + "')=1 " +
                                    //"inner join master_setting hsn on hsn.master_id=it.hsno and hsn.type='HSN' and find_in_set(hsn.client_id,'" + clientid_mst + "')=1 and find_in_set(hsn.client_unit_id,'" + unitid_mst + "')=1 " +
                                    //"inner join (select sum(a.qtyord)-nvl(sum(b.qtyord),0) as qtyord,a.icode,a.vch_date ind_date,a.vch_num as ind_no,a.client_id,a.client_unit_id from purchases a " +
                                    //"left join purchases b on a.icode=b.icode and b.type not in ('66') and a.client_id=b.client_id and a.client_unit_id=b.client_unit_id where a.type='66' and a.client_id='" + clientid_mst + "' " +
                                    //"and a.client_unit_id='" + unitid_mst + "' group by a.icode,a.vch_date,a.vch_num,a.client_id,a.client_unit_id) ind on ind.icode=it.icode and ind.client_id=it.client_id " +
                                    //"and ind.client_unit_id=it.client_unit_id " +
                                    //"where (it.client_id||it.client_unit_id||it.icode||to_char(it.vch_date,'yyyymmdd')||it.type||ind.ind_no||to_char(ind.ind_date,'yyyymmdd')) in ('" + URL + "')";
                                    //brijesh
                                    //mq = "select (it.icode||to_char(it.vch_date,'yyyymmdd')||it.type||pc.ind_no||to_char(pc.vch_date,'yyyymmdd')) fstr ,pc.ind_no,to_char(pc.vch_date,'dd/mm/yyyy') as ind_date,pc.icode as Icode,it.iname as Iname,it.cpartno Partno,um.master_name as UOM,hsn.master_name as hsn,hsn.group_name taxrate,pc.type" +
                                    //    ",s.closing as qtystk,pc.ind_qty,pc.Bal_qty qtyord from item it inner join(select a.ind_no,a.vch_date,a.type, a.icode,a.client_id,a.client_unit_id,a.ind_qty,sum(a.po_qty) as used,(a.ind_qty - nvl(sum(a.po_qty), '0')) as Bal_qty from(select nd.vch_num as ind_no,nd.vch_date,nd.type,nd.icode,nd.client_id,nd.client_unit_id,nd.qtyord as ind_qty,nvl(b.qtyord,'0') as po_qty from purchases nd left join purchases b on b.icode=nd.icode and b.indno=nd.vch_num and b.type not in ('66') and b.pur_type='001' and nd.client_unit_id=b.client_unit_id where nd.client_unit_id='" + unitid_mst + "' and nd.type='66' union all select nd.vch_num as ind_no,nd.vch_date,nd.type,nd.icode,nd.client_id,nd.client_unit_id, nd.qtyord as ind_qty,nvl(b.col11,'0') as close_qty from purchases nd left join enx_tab b on b.col7=nd.icode and b.col12=nd.vch_num and b.type = 'CPI' and nd.client_unit_id=b.client_unit_id where nd.client_unit_id='" + unitid_mst + "' and nd.type='66') a group by(a.ind_no,a.vch_date,a.type, a.icode,a.client_id,a.client_unit_id,a.ind_qty)) pc on pc.icode=it.icode and find_in_set(it.client_id,pc.client_id)=1 and find_in_set(pc.client_unit_id,it.client_unit_id)=1 inner join master_setting um on um.master_id=it.uom and um.type='UMM' and find_in_set(um.client_unit_id,'" + unitid_mst + "')=1 inner join master_setting hsn on hsn.master_id=it.hsno and hsn.type='HSN' and find_in_set(hsn.client_unit_id,'" + unitid_mst + "')=1 " +
                                    //    "left join(select t.client_unit_id, t.icode, sum(nvl(t.qtyin,0)) as Received,sum(nvl(t.qtyout, 0)) as Issued,sum(nvl(t.qtyin, 0)) - sum(nvl(t.qtyout, 0)) as closing from itransaction t where trim(t.store) = 'Y' group by t.icode,t.client_unit_id ) s on it.icode = s.icode and find_in_set(s.client_unit_id, it.client_unit_id)=1 where (it.icode||to_char(it.vch_date,'yyyymmdd')||it.type||pc.ind_no||to_char(pc.vch_date,'yyyymmdd')) in ('" + URL + "')";
                                    mq = "select (it.icode||to_char(it.vch_date,'yyyymmdd')||it.type||pc.ind_no||to_char(pc.vch_date,'yyyymmdd')) fstr,TRIM(dbms_lob.substr(it.description,4000,1)) description,pc.ind_no,to_char(pc.vch_date,'" + sgen.Getsqldateformat() + "') as ind_date,pc.icode as Icode,it.iname as Iname,it.cpartno Partno,um.master_name as UOM,hsn.master_name as hsn,hsn.group_name taxrate,pc.type" +
                                       ",s.closing as qtystk,pc.ind_qty,pc.Bal_qty qtyord from item it inner join(select a.ind_no,a.vch_date,a.type, a.icode,a.client_id,a.client_unit_id,a.ind_qty,sum(a.po_qty) as used,(a.ind_qty - nvl(sum(a.po_qty), '0')) as Bal_qty from(select nd.vch_num as ind_no,nd.vch_date,nd.type,nd.icode,nd.client_id,nd.client_unit_id,nd.qtyord as ind_qty,nvl(b.qtyord,'0') as po_qty from purchases nd left join purchases b on b.icode=nd.icode and b.indno=nd.vch_num and b.type not in ('66') and b.pur_type='001' and nd.client_unit_id=b.client_unit_id where nd.client_unit_id='" + unitid_mst + "' and nd.type='66' union all select nd.vch_num as ind_no,nd.vch_date,nd.type,nd.icode,nd.client_id,nd.client_unit_id, nd.qtyord as ind_qty,nvl(b.col11,'0') as close_qty from purchases nd left join enx_tab b on b.col7=nd.icode and b.col12=nd.vch_num and b.type = 'CPI' and nd.client_unit_id=b.client_unit_id where nd.client_unit_id='" + unitid_mst + "' and nd.type='66') a group by(a.ind_no,a.vch_date,a.type, a.icode,a.client_id,a.client_unit_id,a.ind_qty)) pc on pc.icode=it.icode and find_in_set(it.client_id,pc.client_id)=1 and find_in_set(pc.client_unit_id,it.client_unit_id)=1 inner join master_setting um on um.master_id=it.uom and um.type='UMM' and find_in_set(um.client_unit_id,'" + unitid_mst + "')=1 inner join master_setting hsn on hsn.master_id=it.hsno and hsn.type='HSN' and find_in_set(hsn.client_unit_id,'" + unitid_mst + "')=1 " +
                                       "left join(select icode,client_unit_id,sum(op) op,sum(pktop) pktop,sum(qtyin) qtyin,sum(qtyout) out," +
                                     "sum(nvl(op,0))+sum(nvl(qtyin,0))-sum(nvl(qtyout,0)) closing from (select icode, client_unit_id, nvl(op_2019, 0) op," +
                                     " nvl(pkt_2019, 0) pktop, 0 qtyin, 0 qtyout, 0 pktno from itbal union all select icode, client_unit_id, " +
                                     "sum(to_number(qtyin)) - sum(to_number(qtyout)) op, sum((case when substr(type, 1, 1) in ('0', '1') then" +
                                     " pktno when substr(type,1,1) in ('2', '3', '4') then - pktno else pktno end)) pktop,sum(to_number(qtyin)) " +
                                     "qtyin,sum(to_number(qtyout)) qtyout,0 pktno from itransaction where trim(store) = 'Y' and " +
                                     "to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and " +
                                     "to_date('" + year_from.Split(' ')[0].Trim() + "','" + sgen.Getsqldateformat() + "')-1 group by icode,client_unit_id union all select icode,client_unit_id," +
                                     "0 as op,0 as pktop,to_number(qtyin) qtyin,to_number(qtyout) qtyout,sum((case when substr(type, 1, 1) in " +
                                     "('0', '1') then pktno when substr(type,1,1) in ('2', '3', '4') then - pktno else pktno end)) pktno from " +
                                     "itransaction where trim(store) = 'Y' and to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                                     "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + model[0].Col48.Trim() + "','" + sgen.Getsqldateformat() + "') group by icode,client_unit_id," +
                                     "to_number(qtyin),to_number(qtyout)) a group by icode, client_unit_id) s on it.icode = s.icode and find_in_set(s.client_unit_id, it.client_unit_id)=1 where (it.icode||to_char(it.vch_date,'yyyymmdd')||it.type||pc.ind_no||to_char(pc.vch_date,'yyyymmdd')) in ('" + URL + "')";
                                }
                                dt = sgen.getdata(userCode, mq);
                                if (dt.Rows.Count > 0)
                                {
                                    if (model[0].Col12 == "50" || model[0].Col12 == "52" || model[0].Col12 == "54")
                                    {
                                        if (model[0].Col29 == "002")
                                        {
                                            if (model[0].dt1.Rows[0]["icode"].ToString().Trim().Equals("-")) { model[0].dt1.Rows.RemoveAt(0); }
                                        }
                                        else
                                        {
                                            if (model[0].dt2.Rows[0]["icode"].ToString().Trim().Equals("-")) { model[0].dt2.Rows.RemoveAt(0); }
                                        }
                                    }
                                    //else
                                    //{
                                    //    if (model[0].dt1.Rows[0]["icode"].ToString().Trim().Equals("-")) { model[0].dt1.Rows.RemoveAt(0); }
                                    //}
                                    DataRow dr;
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        if (model[0].Col12 == "50" || model[0].Col12 == "52" || model[0].Col12 == "54")
                                        {
                                            if (model[0].Col29 == "002") { dr = model[0].dt1.NewRow(); }
                                            else { dr = model[0].dt2.NewRow(); }
                                        }
                                        else { dr = model[0].dt1.NewRow(); }
                                        dr["Item"] = dt.Rows[i]["fstr"].ToString();
                                        dr["SNo"] = i + 1;
                                        dr["Icode"] = dt.Rows[i]["icode"].ToString();
                                        dr["IName"] = dt.Rows[i]["iname"].ToString();
                                        dr["PARTNO"] = dt.Rows[i]["partno"].ToString();
                                        dr["HSN"] = dt.Rows[i]["hsn"].ToString();
                                        dr["UOM"] = dt.Rows[i]["uom"].ToString();
                                        dr["taxrate"] = dt.Rows[i]["taxrate"].ToString();
                                        dr["Qty_In_Stock"] = dt.Rows[i]["qtystk"].ToString();
                                        dr["Qty_ord"] = dt.Rows[i]["qtyord"].ToString();
                                        dr["Remark"] = dt.Rows[i]["description"].ToString();
                                        try
                                        {
                                            dr["delivery_date"] = sgen.server_datetime_local(userCode);
                                        }
                                        catch (Exception ex)
                                        { }
                                        if (sgen.GetSession(MyGuid, "Ptype").ToString() == "001")
                                        {
                                            dr["Qty_req"] = dt.Rows[i]["qtyord"].ToString();
                                            dr["IndentNo"] = dt.Rows[i]["ind_no"].ToString();
                                            dr["Indent_Date"] = dt.Rows[i]["ind_date"].ToString();
                                        }
                                        if (model[0].Col12 == "50" || model[0].Col12 == "52" || model[0].Col12 == "54")
                                        {
                                            if (model[0].Col29 == "002") { model[0].dt1.Rows.Add(dr); }
                                            else { model[0].dt2.Rows.Add(dr); }
                                        }
                                        //else { model[0].dt1.Rows.Add(dr); }
                                    }
                                }
                            }
                            break;
                        case "PRINT":
                            string term = "", doc_contrl_no = "", invtype = "";
                            invtype = sgen.getstring(userCode, "select distinct p.type as master_id from purchases p where (p.client_id||p.client_unit_id||p.vch_num||to_char(p.vch_date,'yyyymmdd')||p.type) = '" + URL + "'");
                            DataTable dtterm = sgen.getdata(userCode, "select k.col73 tandc,k.col1 doc_control_no,k.col5 rptid,fm.CLIENT_NAME rptname from kc_tab k " +
                                "inner join master_setting fm on fm.master_id = k.col5 and fm.type = 'IRF' and fm.classid = '84' and fm.sectionid = '" + invtype + "' " +
                                //"inner join master_setting fm on fm.master_id = k.col5 and fm.type = 'IRF' and fm.client_unit_id = '" + unitid_mst + "' and fm.classid = '83' and fm.sectionid = '" + invtype + "' " +
                                "where k.type='PRN' and k.client_unit_id = '" + unitid_mst + "'");
                            if (dtterm.Rows.Count > 0)
                            {
                                term = dtterm.Rows[0]["tandc"].ToString();
                                model[0].Col30 = dtterm.Rows[0]["rptname"].ToString();
                                //model[0].Col30 = "po6";
                                doc_contrl_no = dtterm.Rows[0]["doc_control_no"].ToString();
                            }
                            else
                            {
                                if (invtype == "45") model[0].Col30 = "po5";
                                else model[0].Col30 = "po5";
                            }
                            mq = "select a.acode,nvl(um.master_name,'0') UOM,nvl(hsn.master_name,'0') hsn,a.cond,a.vch_num,a.icode,a.taxrate,to_number(a.qtystk ) qtystk," +
                                "to_number(a.qtyord) qtyord,to_number(a.irate) irate,to_number(a.discrate) discrate,to_number(a.iamount) iamount,to_number(a.taxamt) taxamt," +
                                "to_number(a.discamt) discamt,to_number(a.lineamount) lineamount,a.iremark,a.indno,a.totremark,to_number(a.basicamt) basicamt,to_number(a.gdisc) gdisc," +
                                "to_number(a.gfreight) gfreight,to_number(a.insurance) insurance,to_number(a.ginstlchrg) ginstlchrg,to_number(a.gserchrg) gserchrg,to_number(a.gamc) gamc," +
                                "to_number(a.gloadchrg) gloadchrg,to_number(a.gothrchrg) gothrchrg,to_number(a.gtaxamt) gtaxamt,to_number(a.igst) igst,to_number(a.cgst) cgst," +
                                "to_number(a.sgst) sgst,to_number(a.gamt) gamt,to_number(a.netamt) netamt,to_char(convert_tz(a.vch_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') vch_date1," +
                                "to_char(convert_tz(a.dlv_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') dlv_date1,to_char(convert_tz(a.inddate, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') inddate1," +
                                "c.c_name as name,c.cpname as cont_per,c.cpcont,c.cpemail,c.c_gstin as gstin,c.addr,c.tor,c.org_mobile,nvl(s.c_name, '0') sf_name,nvl(s.c_gstin, '0') sf_gstin,nvl(s.addr, '0') as sf_addr," +
                                "nvl(s.tor, '0') as sf_tor,nvl(s.cpcont, '0') sf_contact,nvl(t.c_name, '0') st_name,nvl(t.c_gstin, '0') tf_gstin,nvl(t.addr, '0') as st_addr," +
                                "nvl(t.tor, '0') as st_tor,nvl(t.cpcont, '0') st_contact,nvl(py.master_name, '0') py_term,nvl(pr.master_name, '0') pr_term,nvl(mt.master_name, '0') trans," +
                                "i.cpartno as partno,i.iname as item_name,'" + term + "' terms_cond,'" + doc_contrl_no + "' doc_control_no" +
                                ",nvl(ct.master_name,'-') as currency,nvl(ins.master_name,'-') as ins_by,a.shptval,a.shpfval,dl.master_name as delivery_term,nvl(a.tcremark,'-')" +
                                ",(case when a.IS_WARNT='Y' then to_char(a.WRTDATE,'" + sgen.Getsqldateformat() + "') when a.IS_WARNT='N' then 'No Warranty' else '-' end) warranty from purchases a " +
                                "inner join item i on a.icode = i.icode  and i.type = 'IT' and find_in_set(a.client_unit_id, i.client_unit_id)=1 " +
                                "left join clients_mst c on c.vch_num = a.acode and c.type = 'BCD' and substr(c.vch_num,0,3)='203' and find_in_set(c.client_unit_id, a.client_unit_id)=1 " +
                                "left join clients_mst s on s.vch_num = a.shipfrom and s.type = 'BCD' and substr(s.vch_num,0,3)='203' and find_in_set(s.client_unit_id, a.client_unit_id)=1 " +
                                "left join clients_mst t on t.vch_num = a.shipto and t.type = 'BCD' and substr(t.vch_num,0,3)='203' and find_in_set(t.client_unit_id, a.client_unit_id)=1 " +
                                "left join master_setting py on a.pay_term = py.master_id and py.type = 'PTM' and find_in_set(a.client_unit_id, py.client_unit_id)= 1 " +
                                "left join master_setting pr on a.price_term = pr.master_id and pr.type = 'PRI' and find_in_set(a.client_unit_id, pr.client_unit_id)= 1 " +
                                "left join master_setting dl on a.delterm = dl.master_id and dl.type = 'DEL' and find_in_set(a.client_unit_id, dl.client_unit_id)= 1 " +
                                "left join master_setting mt on a.tmode = mt.master_id and mt.type = 'AMT' and find_in_set(a.client_unit_id, mt.client_unit_id)= 1 " +
                                "left join master_setting um on um.master_id = i.uom and um.type = 'UMM' and find_in_set(um.client_unit_id,'" + unitid_mst + "')= 1 " +
                                "left join master_setting hsn on hsn.master_id = i.hsno and hsn.type = 'HSN' and find_in_set(hsn.client_unit_id,'" + unitid_mst + "')= 1 " +
                                "left join master_setting ins on ins.master_id = a.insby and ins.type = 'K01' and find_in_set(ins.client_unit_id,'" + unitid_mst + "')= 1 " +
                                "left join master_setting ct on ct.master_id = a.doccur and ct.type = 'CTM' and find_in_set(ct.client_unit_id,'" + unitid_mst + "')= 1 " +
                                "where (a.client_id || a.client_unit_id || a.vch_num || to_char(a.vch_date, 'yyyymmdd') || a.type) = '" + URL + "' and to_number(a.qtyord) > 0";
                            //for other charges"select OTH.col2 as othr_chrgamt,OTH.col4 as othr_igstrt,OTH.col5 AS othr_igst,OTH.col6 as othr_cgstrt,OTH.col7 AS othr_cgst,OTH.col8 as othr_sgstrt,OTH.col9 AS othr_sgst
                            //left join (select ch.master_name chrgname,en.col1,en.col2,en.col3,en.col4,en.col5,en.col6,en.col7,en.col8,en.col9,en.r_no from enx_tab en inner join master_setting ch on en.col1=ch.master_id and ch.type='MR0' and find_in_set(ch.client_unit_id,en.client_unit_id)=1 where (en.client_id||en.client_unit_id||en.vch_num||to_char(en.vch_date, 'yyyymmdd')|| en.type) = '" + URL + "') OTH on 1=1";
                            dtp = sgen.getdata(userCode, mq);
                            if (dtp.Rows.Count > 0)
                            {
                                dtp.TableName = "prepcur";
                                mq1 = "select ch.master_name chrgname,en.col1,to_number(en.col2) col2,en.col3,en.col4,to_number(en.col5) col5,en.col6,to_number(en.col7) col7,en.col8,to_number(en.col9) col9,en.col10" +
                                    ",en.r_no from enx_tab en inner join master_setting ch on en.col1=ch.master_id and ch.type='MR0' and " +
                                    "find_in_set(ch.client_unit_id,en.client_unit_id)=1 where (en.client_id||en.client_unit_id||en.vch_num||to_char(en.vch_date, 'yyyymmdd')|| en.type) = '" + URL + "'";
                                var dthsn = sgen.getdata(userCode, mq1);
                                DataSet dsh = new DataSet();
                                dthsn.TableName = "prepcur2";
                                dsh.Tables.Add(dtp);
                                dsh.Tables.Add(dthsn);
                                //string xmlpath = System.Web.HttpContext.Current.Server.MapPath("~/") + "xmls/po_othrchrg.xml";
                                //dsh.WriteXml(xmlpath, XmlWriteMode.WriteSchema);
                                sgen.open_report_byDs_xml(userCode, dsh, model[0].Col30, "Purchase Order");
                                ViewBag.scripCall += "showRptnew('Report');disableForm();";
                            }
                            break;
                        case "CHRGS":
                            mq = "select master_id,master_name from master_setting where (client_id||client_unit_id||master_id||to_char(vch_date,'yyyymmdd')||type) in ('" + URL + "')";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                int i = 0;
                                if (model[0].LTM1 == null) { model[0].LTM1 = new List<Tmodelmain>(); }
                                if (model[0].LTM1.Count == 0) { model[0].LTM1 = new List<Tmodelmain>(); }
                                else i = model[0].LTM1.Count;
                                foreach (DataRow dr in dtt.Rows)
                                {
                                    i++;
                                    model[0].LTM1.Add(new Tmodelmain
                                    {
                                        Col14 = i.ToString(),
                                        Col15 = dr["master_id"].ToString(),
                                        Col23 = dr["master_name"].ToString(),
                                        Col16 = "0",
                                        Col25 = "0",
                                        Col17 = "0",
                                        Col18 = "0",
                                        Col19 = "0",
                                        Col20 = "0",
                                        Col21 = "0",
                                        Col22 = "0",
                                    });
                                }
                            }
                            break;
                        case "CHRGDEL":
                            if (sgen.GetSession(MyGuid, "podelrno") != null)
                            {
                                var mrno = sgen.GetSession(MyGuid, "podelrno").ToString();
                                sgen.SetSession(MyGuid, "podelrno", null);
                                var LTM = model[0].LTM1;
                                for (int d = 0; d < LTM.Count; d++)
                                {
                                    string chrgno = LTM[d].Col14;
                                    if (chrgno.Trim() == mrno.Trim()) { model[0].LTM1.RemoveAt(d); }
                                }
                                sgen.SetSession(MyGuid, "podelrno", null);
                            }
                            break;
                        case "RATE":
                            if (sgen.GetSession(MyGuid, "rtrno") == null)
                            {
                                ViewBag.scripCall += "showmsgJS(1,'Rno not defined', 0);";
                                return model;
                            }
                            var rno = sgen.Make_int(sgen.GetSession(MyGuid, "rtrno").ToString());
                            mq = "select master_id DocNo,group_name TaxRate from master_setting where (client_id||client_unit_id||master_id||to_char(vch_date,'yyyymmdd')||type)='" + URL + "'";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                var utgst = "";
                                model[0].LTM1[rno - 1].Col13 = dtt.Rows[0]["DocNo"].ToString().Trim();
                                try { utgst = model[0].Col18.Substring(0, 2).Trim(); }
                                catch (Exception ex) { utgst = model[0].Col18; }
                                var pgst = model[0].Col22.Substring(0, 2).Trim();
                                if (utgst == pgst)
                                {
                                    var amt = sgen.Make_decimal(model[0].LTM1[rno - 1].Col16);
                                    var rt = sgen.Make_decimal(dtt.Rows[0]["TaxRate"].ToString().Trim().Split('%')[0].Trim());
                                    var amtrt = (amt * rt) / 100;
                                    model[0].LTM1[rno - 1].Col19 = (rt / 2).ToString() + "%";
                                    model[0].LTM1[rno - 1].Col20 = (amtrt / 2).ToString();
                                    model[0].LTM1[rno - 1].Col21 = (rt / 2).ToString() + "%";
                                    model[0].LTM1[rno - 1].Col22 = (amtrt / 2).ToString();
                                }
                                else
                                {
                                    var amt = sgen.Make_decimal(model[0].LTM1[rno - 1].Col16);
                                    var rt = sgen.Make_decimal(dtt.Rows[0]["TaxRate"].ToString().Trim().Split('%')[0].Trim());
                                    var amtrt = (amt * rt) / 100;
                                    model[0].LTM1[rno - 1].Col17 = rt.ToString() + "%";
                                    model[0].LTM1[rno - 1].Col18 = amtrt.ToString();
                                }
                                decimal bamt = sgen.Make_decimal(model[0].Col33);//basicamt
                                decimal bigst = sgen.Make_decimal(model[0].Col34);//basicigst
                                decimal bcgst = sgen.Make_decimal(model[0].Col35);//basiccgst
                                decimal bsgst = sgen.Make_decimal(model[0].Col36);//basicsgst
                                decimal tamt = 0, tigst = 0, tcgst = 0, tsgst = 0;
                                for (int i = 0; i < model[0].LTM1.Count; i++)
                                {
                                    tamt = tamt + sgen.Make_decimal(model[0].LTM1[i].Col16);//chrgamt
                                    tigst = tigst + sgen.Make_decimal(model[0].LTM1[i].Col18);//chrgigst
                                    tcgst = tcgst + sgen.Make_decimal(model[0].LTM1[i].Col20);//chrgcgst
                                    tsgst = tsgst + sgen.Make_decimal(model[0].LTM1[i].Col22);//chrgsgst
                                }
                                model[0].Col37 = (bamt + tamt).ToString();//totamount
                                model[0].Col38 = (bigst + tigst).ToString();//totigst
                                model[0].Col39 = (bcgst + tcgst).ToString();//totcgst
                                model[0].Col40 = (bsgst + tsgst).ToString();//totsgst
                                model[0].Col69 = (bamt + tamt + bigst + tigst + bcgst + tcgst + bsgst + tsgst).ToString();//netamt
                            }
                            break;
                    }
                    break;
                #endregion
                #region po_imp                    
                case "po_imp":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                        case "COPYPO":
                            mq = "select a.*,to_char(convert_tz(vch_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') vch_date1," +
                                "to_char(convert_tz(dlv_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') dlv_date1," +
                                "to_char(convert_tz(inddate,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') inddate1," +
                                "to_char(convert_tz(vch_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') date11," +
                                "to_char(convert_tz(vch_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') date21," +
                                "to_char(convert_tz(vch_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') date31 from purchases a " +
                                "where (client_id||client_unit_id||vch_num||to_char(vch_date, 'yyyymmdd')|| type) = '" + URL + "'";
                            dtp = sgen.getdata(userCode, mq);
                            if (dtp.Rows.Count > 0)
                            {
                                #region ddl
                                TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1 = cmd_Fun.payterm(userCode, unitid_mst, out defcall);              //payterm
                                TempData[MyGuid + "_TList2"] = model[0].TList2 = mod2 = cmd_Fun.priceterm(userCode, unitid_mst);           //priceterm
                                TempData[MyGuid + "_TList3"] = model[0].TList3 = mod3 = cmd_Fun.transportmode(userCode, unitid_mst);       //transport 
                                TempData[MyGuid + "_TList4"] = model[0].TList4 = mod4 = cmd_Fun.Modeofpayment(userCode, unitid_mst);       //payment mode 
                                TempData[MyGuid + "_TList5"] = model[0].TList5 = mod5 = cmd_Fun.insby(userCode, unitid_mst);               //insby 
                                TempData[MyGuid + "_TList6"] = model[0].TList6 = mod6 = cmd_Fun.delterm(userCode, unitid_mst);             //delterm 
                                TempData[MyGuid + "_TList7"] = model[0].TList7 = mod7 = cmd_Fun.curname(userCode, unitid_mst);             //curname
                                String[] L1 = System.String.Join(",", dtp.Rows.OfType<DataRow>().Select(r => r["pay_term"].ToString()).Distinct()).Split(',');
                                String[] L2 = System.String.Join(",", dtp.Rows.OfType<DataRow>().Select(r => r["price_term"].ToString()).Distinct()).Split(',');
                                String[] L3 = System.String.Join(",", dtp.Rows.OfType<DataRow>().Select(r => r["tmode"].ToString()).Distinct()).Split(',');
                                String[] L4 = System.String.Join(",", dtp.Rows.OfType<DataRow>().Select(r => r["paymode"].ToString()).Distinct()).Split(',');
                                String[] L5 = System.String.Join(",", dtp.Rows.OfType<DataRow>().Select(r => r["insby"].ToString()).Distinct()).Split(',');
                                String[] L6 = System.String.Join(",", dtp.Rows.OfType<DataRow>().Select(r => r["delterm"].ToString()).Distinct()).Split(',');
                                String[] L7 = System.String.Join(",", dtp.Rows.OfType<DataRow>().Select(r => r["doccur"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems1 = L1;
                                model[0].SelectedItems2 = L2;
                                model[0].SelectedItems3 = L3;
                                model[0].SelectedItems4 = L4;
                                model[0].SelectedItems5 = L5;
                                model[0].SelectedItems6 = L6;
                                model[0].SelectedItems7 = L7;
                                #endregion
                                model[0].Col56 = dtp.Rows[0]["tname"].ToString();
                                model[0].Col57 = dtp.Rows[0]["pkins"].ToString();
                                model[0].Col58 = dtp.Rows[0]["tcstatus"].ToString();
                                model[0].Col59 = dtp.Rows[0]["tcremark"].ToString();
                                model[0].Col67 = dtp.Rows[0]["gstcr"].ToString();
                                model[0].Col68 = dtp.Rows[0]["gstcrremark"].ToString();
                                model[0].Col70 = dtp.Rows[0]["col1"].ToString();
                                model[0].Col71 = dtp.Rows[0]["col2"].ToString();
                                model[0].Col72 = dtp.Rows[0]["col3"].ToString();
                                model[0].Col73 = dtp.Rows[0]["date11"].ToString();
                                model[0].Col74 = dtp.Rows[0]["date21"].ToString();
                                model[0].Col75 = dtp.Rows[0]["date31"].ToString();
                                model[0].Col76 = dtp.Rows[0]["col4"].ToString();
                                model[0].Col77 = dtp.Rows[0]["col5"].ToString();
                                model[0].Col78 = dtp.Rows[0]["col6"].ToString();
                                model[0].Col60 = dtp.Rows[0]["pref"].ToString();
                                model[0].Col61 = dtp.Rows[0]["taxre"].ToString();
                                model[0].Col62 = dtp.Rows[0]["shpfval"].ToString();
                                model[0].Col63 = dtp.Rows[0]["shptval"].ToString();
                                model[0].Col79 = sgen.seekval(userCode, "select upper(master_name) master_name from (select '001' fstr,'001' master_id,'With Indent' master_name from dual union all " +
                                    "select '002' fstr,'002' master_id,'Without Indent' master_name from dual) a where fstr='" + dtp.Rows[0]["pur_type"].ToString().Trim() + "'", "master_name");
                                model[0].Col31 = dtp.Rows[0]["totremark"].ToString();
                                model[0].Col32 = dtp.Rows[0]["cond"].ToString();
                                model[0].Col33 = dtp.Rows[0]["basicamt"].ToString();
                                model[0].Col34 = dtp.Rows[0]["igst"].ToString();
                                model[0].Col35 = dtp.Rows[0]["cgst"].ToString();
                                model[0].Col36 = dtp.Rows[0]["sgst"].ToString();
                                model[0].Col37 = dtp.Rows[0]["gamt"].ToString();
                                model[0].Col38 = dtp.Rows[0]["gigst"].ToString();
                                model[0].Col39 = dtp.Rows[0]["gcgst"].ToString();
                                model[0].Col40 = dtp.Rows[0]["gsgst"].ToString();
                                model[0].Col69 = dtp.Rows[0]["netamt"].ToString();
                                DataTable dte = new DataTable();
                                mq1 = "select a.c_name as name,a.c_gstin as gstin,a.addr,a.vch_num,a.tor from clients_mst a where find_in_set(a.client_unit_id,'" + unitid_mst + "')=1 and a.type='BCD' and a.vch_num='" + dtp.Rows[0]["acode"].ToString() + "' and substr(a.vch_num,0,3)='203'";
                                dte = sgen.getdata(userCode, mq1);
                                if (dte.Rows.Count > 0)
                                {
                                    model[0].Col49 = dte.Rows[0]["vch_num"].ToString();
                                    model[0].Col20 = dte.Rows[0]["name"].ToString();
                                    model[0].Col21 = dte.Rows[0]["addr"].ToString();
                                    model[0].Col22 = dte.Rows[0]["gstin"].ToString();
                                    model[0].Col52 = dte.Rows[0]["tor"].ToString();
                                }
                                mq1 = "select a.c_name as name,a.c_gstin as gstin,a.addr,a.vch_num from clients_mst a where " +
                                    "find_in_set(a.client_unit_id,'" + unitid_mst + "')=1 and a.type='BCD' and a.vch_num='" + dtp.Rows[0]["shipfrom"].ToString() + "' " +
                                  " and substr(a.vch_num,0,3)='203'";
                                dte = sgen.getdata(userCode, mq1);
                                if (dte.Rows.Count > 0)
                                {
                                    model[0].Col50 = dte.Rows[0]["vch_num"].ToString();
                                    model[0].Col23 = dte.Rows[0]["name"].ToString();
                                    model[0].Col24 = dte.Rows[0]["addr"].ToString();
                                    model[0].Col25 = dte.Rows[0]["gstin"].ToString();
                                }
                                mq1 = "select a.c_name as name,a.c_gstin as gstin,a.addr,a.vch_num from clients_mst a where find_in_set(a.client_unit_id,'" + unitid_mst + "')=1 and a.type='BCD' and a.vch_num='" + dtp.Rows[0]["shipto"].ToString() + "' and substr(a.vch_num,0,3)='203'";
                                dte = sgen.getdata(userCode, mq1);
                                if (dte.Rows.Count > 0)
                                {
                                    model[0].Col51 = dte.Rows[0]["vch_num"].ToString();
                                    model[0].Col26 = dte.Rows[0]["name"].ToString();
                                    model[0].Col27 = dte.Rows[0]["addr"].ToString();
                                    model[0].Col28 = dte.Rows[0]["gstin"].ToString();
                                }
                                if (btnval.ToUpper().Equals("EDIT") || btnval.ToUpper().Equals("VIEW"))
                                {
                                    sgen.SetSession(MyGuid, "EDMODE", "Y");
                                    model[0].Col1 = dtp.Rows[0]["client_id"].ToString();
                                    model[0].Col2 = dtp.Rows[0]["client_unit_id"].ToString();
                                    model[0].Col3 = dtp.Rows[0]["ent_by"].ToString();
                                    model[0].Col4 = dtp.Rows[0]["ent_date"].ToString();
                                    model[0].Col5 = dtp.Rows[0]["edit_by"].ToString();
                                    model[0].Col6 = dtp.Rows[0]["edit_date"].ToString();
                                    model[0].Col8 = "(client_id||client_unit_id||vch_num||to_char(vch_date, 'yyyymmdd')|| type) = '" + URL + "'";
                                    model[0].Col9 = sgen.seekval(userCode, "select upper(master_name) master_name from master_setting where master_id='" + dtp.Rows[0]["type"].ToString() + "' and type='KPO'", "master_name");
                                    model[0].Col10 = model[0].Col10;
                                    model[0].Col11 = model[0].Col11;
                                    model[0].Col12 = dtp.Rows[0]["type"].ToString();
                                    model[0].Col13 = "Update";
                                    model[0].Col100 = "Update & New";
                                    model[0].Col121 = "U";
                                    model[0].Col122 = "<u>U</u>pdate";
                                    model[0].Col123 = "Update & Ne<u>w</u>";
                                    model[0].Col14 = model[0].Col14;
                                    model[0].Col15 = model[0].Col15;
                                    model[0].Col16 = dtp.Rows[0]["vch_num"].ToString();
                                    model[0].Col47 = dtp.Rows[0]["vch_num"].ToString();
                                    model[0].Col48 = dtp.Rows[0]["vch_date1"].ToString();
                                    model[0].Col29 = dtp.Rows[0]["pur_type"].ToString();
                                    if (btnval.ToUpper().Equals("VIEW"))
                                    {
                                        ViewBag.vnew = "disabled='disabled'";
                                        ViewBag.vedit = "disabled='disabled'";
                                        ViewBag.vsave = "disabled='disabled'";
                                        ViewBag.vsavenew = "disabled='disabled'";
                                        ViewBag.scripCall += "disableForm();";
                                    }
                                }
                                else
                                {
                                    sgen.SetSession(MyGuid, "EDMODE", "N");
                                }
                                DataTable dtf = new DataTable();
                                //if (dtp.Rows[0]["type"].ToString().Trim().Equals("50") || dtp.Rows[0]["type"].ToString().Trim().Equals("52") || dtp.Rows[0]["type"].ToString().Trim().Equals("54"))
                                //{
                                //    if (dtp.Rows[0]["pur_type"].ToString().Equals("002"))
                                //    {
                                //        dtt = sgen.getdata(userCode, "select '' Item,'1' as  SNo ,'-' as Icode,'-' as IName,'-' PARTNO,'-'  HSN,'-' as UOM,'0' as TaxRate," +
                                //            "'0' Qty_in_stock,'0' Qty_Ord,'0' IRate,'0' disc_rate,'0' IAmount,'0' TaxAmount,'0' discAmount,'0' LineAmount,'-' delivery_date," +
                                //            "'-' as Remark from dual");
                                //        model[0].dt1 = dtt;
                                //        dtf = model[0].dt1.Clone();
                                //        sgen.SetSession(MyGuid, "KPO_DT", dtt);
                                //    }
                                //    else
                                //    {
                                //        dtt = sgen.getdata(userCode, "select '' Item,'1' SNo ,'-' as Icode,'-' as IName,'-' PARTNO,'-'  HSN,'-' as UOM,'0' as TaxRate," +
                                //            "'0' as Qty_in_stock,'0' as Qty_Ord,'0' Qty_Req,'0' Qty_bal,'0' as IRate,'0' as disc_rate,'0' IAmount,'0' TaxAmount,'0' discAmount," +
                                //            "'0' LineAmount,'-' as delivery_date,'-' as Remark,'-' Indentno,'-' Indent_Date from dual");
                                //        model[0].dt2 = dtt;
                                //        dtf = model[0].dt2.Clone();
                                //        sgen.SetSession(MyGuid, "KPO_DT", dtt);
                                //    }
                                //}
                                //else
                                //{
                                dtt = sgen.getdata(userCode, "select '' Item,'1' as  SNo ,'-' as Icode,'-' as IName,'-' PARTNO,'-'  HSN,'-' as UOM,'0' as TaxRate," +
                                       "'0' Qty_in_stock,'0' Qty_Ord,'0' IRate,'0' disc_rate,'0' IAmount,'0' TaxAmount,'0' discAmount,'0' LineAmount,'-' delivery_date," +
                                       "'-' as Remark from dual");
                                model[0].dt1 = dtt;
                                dtf = model[0].dt2.Clone();
                                sgen.SetSession(MyGuid, "KPO_DT", dtt);
                                //}
                                for (int i = 0; i < dtp.Rows.Count; i++)
                                {
                                    DataRow dr = dtf.NewRow();
                                    dr["SNo"] = i + 1;
                                    dr["Icode"] = dtp.Rows[i]["icode"].ToString();
                                    dr["IName"] = dtp.Rows[i]["iname"].ToString();
                                    dr["PARTNO"] = dtp.Rows[i]["cpartno"].ToString();
                                    dr["HSN"] = dtp.Rows[i]["hsno"].ToString();
                                    dr["UOM"] = dtp.Rows[i]["uom"].ToString();
                                    dr["TaxRate"] = dtp.Rows[i]["taxrate"].ToString() + "%";
                                    dr["Qty_in_stock"] = dtp.Rows[i]["qtystk"].ToString();
                                    if (dtp.Rows[0]["pur_type"].ToString().Equals("001"))
                                    {
                                        dr["Qty_ord"] = dtp.Rows[i]["qtyind"].ToString();
                                        dr["Qty_Req"] = dtp.Rows[i]["qtyord"].ToString();
                                        dr["Qty_Bal"] = dtp.Rows[i]["qtybal"].ToString();
                                        dr["IndentNo"] = dtp.Rows[i]["indno"].ToString();
                                        dr["Indent_Date"] = dtp.Rows[i]["inddate1"].ToString();
                                    }
                                    else
                                    {
                                        dr["Qty_Ord"] = dtp.Rows[i]["qtyord"].ToString();
                                    }
                                    dr["IRate"] = dtp.Rows[i]["irate"].ToString();
                                    dr["disc_rate"] = dtp.Rows[i]["discrate"].ToString();
                                    dr["Iamount"] = dtp.Rows[i]["Iamount"].ToString();
                                    dr["taxamount"] = dtp.Rows[i]["taxamt"].ToString();
                                    dr["discamount"] = dtp.Rows[i]["discamt"].ToString();
                                    dr["lineamount"] = dtp.Rows[i]["lineamount"].ToString();
                                    dr["delivery_date"] = dtp.Rows[i]["dlv_date1"].ToString();
                                    dr["Remark"] = dtp.Rows[i]["iremark"].ToString();
                                    dtf.Rows.Add(dr);
                                }
                                dtf.AcceptChanges();
                                //if (dtp.Rows[0]["type"].ToString().Trim().Equals("50") || dtp.Rows[0]["type"].ToString().Trim().Equals("52") || dtp.Rows[0]["type"].ToString().Trim().Equals("54"))
                                //{
                                //    if (dtp.Rows[0]["pur_type"].ToString().Equals("002")) { model[0].dt1 = dtf; }
                                //    else { model[0].dt2 = dtf; }
                                //}
                                //else { model[0].dt1 = dtf; }
                                model[0].dt1 = dtf;

                                //other charges LTM
                                mq1 = "select ch.master_name chrgname,en.col1,en.col2,en.col3,en.col4,en.col5,en.col6,en.col7,en.col8,en.col9,en.r_no from enx_tab en " +
                                    "inner join master_setting ch on en.col1=ch.master_id and ch.type='MR0' and find_in_set(ch.client_unit_id,en.client_unit_id)=1 " +
                                    "where (en.client_id||en.client_unit_id||en.vch_num||to_char(en.vch_date, 'yyyymmdd')|| en.type) = '" + URL + "'";
                                dt = sgen.getdata(userCode, mq1);
                                if (dt.Rows.Count > 0)
                                {
                                    model[0].LTM1 = new List<Tmodelmain>();
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        model[0].LTM1.Add(new Tmodelmain
                                        {
                                            Col13 = dr["col3"].ToString(),//chrgrtid
                                            Col14 = dr["r_no"].ToString(),//rno
                                            Col15 = dr["col1"].ToString(),//chrgid
                                            Col16 = dr["col2"].ToString(),//chrgamt
                                            Col17 = dr["col4"].ToString(),//igstrt
                                            Col18 = dr["col5"].ToString(),//igstamt
                                            Col19 = dr["col6"].ToString(),//cgstrt
                                            Col20 = dr["col7"].ToString(),//cgstamt
                                            Col21 = dr["col8"].ToString(),//sgstrt
                                            Col22 = dr["col9"].ToString(),//sgstamt
                                            Col23 = dr["chrgname"].ToString(),//chrgname
                                        });
                                    }
                                }
                            }
                            break;
                        case "NEW":
                            mq = "select * from (select '001' fstr,'001' master_id,'With Indent' master_name from dual union all select '002' fstr,'002' master_id," +
                                "'Without Indent' master_name from dual) a where fstr='" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model[0].Col29 = dt.Rows[0]["fstr"].ToString();
                                model[0].Col79 = dt.Rows[0]["master_name"].ToString().ToUpper();
                                Make_query(viewName.ToLower(), "Select PO Type", "POTYPE", "1", "", "", MyGuid);
                                ViewBag.scripCall = "callFoo('Select PO Type');";
                            }
                            break;
                        case "POTYPE":
                            mq = "select master_id,upper(master_name) master_name,to_char(convert_tz(utc_timestamp(),'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') po_date " +
                                "from master_Setting where (master_id||to_char(vch_date, 'yyyymmdd')||type) = '" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "N");
                                ViewBag.vnew = "disabled='disabled'";
                                ViewBag.vedit = "disabled='disabled'";
                                ViewBag.vsave = "";
                                ViewBag.vcalc = "";
                                ViewBag.scripCall = "enableForm();";
                                mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + dt.Rows[0]["master_id"].ToString() + "'" + model[0].Col11.Trim() + "";
                                vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                                model[0].Col47 = vch_num;
                                model[0].Col16 = vch_num;
                                model[0].Col48 = dt.Rows[0]["po_date"].ToString();
                                model[0].Col12 = dt.Rows[0]["master_id"].ToString();
                                model[0].Col17 = "1";
                                model[0].Col9 = dt.Rows[0]["master_name"].ToString();
                                model[0].Col58 = "N";
                                model[0].Col67 = "Y";
                                var loccur = sgen.GetSession(MyGuid, "LOCALCUR").ToString();
                                #region ddl
                                TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1 = cmd_Fun.payterm(userCode, unitid_mst, out defcall);
                                TempData[MyGuid + "_TList2"] = model[0].TList2 = mod2 = cmd_Fun.priceterm(userCode, unitid_mst);
                                TempData[MyGuid + "_TList3"] = model[0].TList3 = mod3 = cmd_Fun.transportmode(userCode, unitid_mst);
                                TempData[MyGuid + "_TList4"] = model[0].TList4 = mod4 = cmd_Fun.Modeofpayment(userCode, unitid_mst);
                                TempData[MyGuid + "_TList5"] = model[0].TList5 = mod5 = cmd_Fun.insby(userCode, unitid_mst);
                                TempData[MyGuid + "_TList6"] = model[0].TList6 = mod6 = cmd_Fun.delterm(userCode, unitid_mst);
                                TempData[MyGuid + "_TList7"] = model[0].TList7 = mod7 = cmd_Fun.curname(userCode, unitid_mst);
                                model[0].SelectedItems1 = new string[] { "" };
                                model[0].SelectedItems2 = new string[] { "" };
                                model[0].SelectedItems3 = new string[] { "" };
                                model[0].SelectedItems4 = new string[] { "" };
                                model[0].SelectedItems5 = new string[] { "" };
                                model[0].SelectedItems6 = new string[] { "" };
                                model[0].SelectedItems7 = new string[] { loccur };
                                #endregion
                                //if (dt.Rows[0]["master_id"].ToString().Trim().Equals("50") || dt.Rows[0]["master_id"].ToString().Trim().Equals("52") || dt.Rows[0]["master_id"].ToString().Trim().Equals("54"))
                                ////if (dt.Rows[0]["master_id"].ToString().Trim().Equals("51") || dt.Rows[0]["master_id"].ToString().Trim().Equals("53"))
                                //{
                                //    if (model[0].Col29 == "002")
                                //    {
                                //        dtt = sgen.getdata(userCode, "select '' Item,'1' as  SNo ,'-' as Icode,'-' as IName,'-' PARTNO,'-'  HSN,'-' as UOM,'0' as TaxRate," +
                                //            "'0' Qty_in_stock,'0' Qty_Ord,'0' IRate,'0' disc_rate,'0' IAmount,'0' TaxAmount,'0' discAmount,'0' LineAmount,'-' delivery_date," +
                                //            "'-' as Remark from dual");
                                //        model[0].dt1 = dtt;
                                //        sgen.SetSession(MyGuid, "KPO_DT", dtt);
                                //    }
                                //    else
                                //    {
                                //        dtt = sgen.getdata(userCode, "select '' Item,'1' SNo ,'-' as Icode,'-' as IName,'-' PARTNO,'-'  HSN,'-' as UOM,'0' as TaxRate," +
                                //            "'0' as Qty_in_stock,'0' as Qty_Ord,'0' Qty_Req,'0' Qty_bal,'0' as IRate,'0' as disc_rate,'0' IAmount,'0' TaxAmount,'0' discAmount," +
                                //            "'0' LineAmount,'-' as delivery_date,'-' as Remark,'-' Indentno,'-' Indent_Date from dual");
                                //        model[0].dt2 = dtt;
                                //        sgen.SetSession(MyGuid, "KPO_DT", dtt);
                                //    }
                                //}
                                //else
                                //{                                
                                mq = "select param1 from controls where type='CTL' and m_module3='-' and id in ('000008','000009','000010')";
                                DataTable dtm = sgen.getdata(userCode, mq);
                                if (dtm.Rows.Count > 0)
                                {
                                    taxrt1 = dtm.Rows[0]["param1"].ToString();//Social Welfare Surcharge
                                    taxrt2 = dtm.Rows[1]["param1"].ToString();//Anti-dumping Duty
                                    taxrt3 = dtm.Rows[2]["param1"].ToString();//Safeguard Duty
                                    if (taxrt1.Trim().Equals("Y"))
                                    {
                                        rtcond = rtcond + "'0%' Soc_Welf_Sur,";
                                        amtcond = amtcond + "'0' Soc_Welf_Sur_Amt,";
                                    }
                                    if (taxrt2.Trim().Equals("Y"))
                                    {
                                        rtcond = rtcond + "'0%' Anti_dumping_Duty,";
                                        amtcond = amtcond + "'0' Anty_Dumping_Duty_Amt,";
                                    }
                                    if (taxrt3.Trim().Equals("Y"))
                                    {
                                        rtcond = rtcond + "'0%' Safeguard_Duty,";
                                        amtcond = amtcond + "'0' Safeguard_Duty_Amt,";
                                    }
                                }
                                dtt = sgen.getdata(userCode, "select '' Item,'1' as SNo ,'-' as Icode,'-' as IName,'-' PARTNO,'-' as UOM,'-'  HSN,'0%' Bcdrate," + rtcond + "'0' Bcdamt," + amtcond + "" +
                                    "'0' Qty_in_stock,'0' Qty_Ord,'0' IRate,'0' RptAmt,'0' DocAmt,'' DocCurrency,'0' LineAmount,'-' as Remark from dual");
                                model[0].dt1 = dtt;
                                sgen.SetSession(MyGuid, "KPO_DT", dtt);
                                //}
                            }
                            break;
                        case "PARTY":
                            mq = "select a.c_name name,a.c_gstin gstin,a.addr,a.vch_num,a.tor,a.pay_term2,a.pay_mode2 from clients_mst a " +
                                "where (a.vch_num||to_char(a.vch_date, 'yyyymmdd')|| a.type) = '" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model[0].Col49 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col20 = dt.Rows[0]["name"].ToString();
                                model[0].Col21 = dt.Rows[0]["addr"].ToString();
                                model[0].Col22 = dt.Rows[0]["gstin"].ToString();
                                if (dt.Rows[0]["gstin"].ToString().Trim().ToUpper().Equals("NOT REGISTERED"))
                                {
                                    model[0].Col67 = "N";
                                }
                                model[0].Col52 = dt.Rows[0]["tor"].ToString();
                                model[0].SelectedItems1 = new string[] { dt.Rows[0]["pay_term2"].ToString() };
                                model[0].SelectedItems4 = new string[] { dt.Rows[0]["pay_mode2"].ToString() };
                            }
                            break;
                        case "PARTYFRM":
                            mq = "select a.c_name as name,a.c_gstin as gstin,a.addr,a.vch_num from clients_mst a " +
                                "where (a.vch_num||to_char(a.vch_date, 'yyyymmdd')|| a.type) = '" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model[0].Col50 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col23 = dt.Rows[0]["name"].ToString();
                                model[0].Col24 = dt.Rows[0]["addr"].ToString();
                                model[0].Col25 = dt.Rows[0]["gstin"].ToString();
                            }
                            break;
                        case "PARTYTO":
                            mq = "select a.c_name as name,a.c_gstin as gstin,a.addr,a.vch_num from clients_mst a " +
                                "where (a.vch_num||to_char(a.vch_date, 'yyyymmdd')|| a.type) = '" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model[0].Col51 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col26 = dt.Rows[0]["name"].ToString();
                                model[0].Col27 = dt.Rows[0]["addr"].ToString();
                                model[0].Col28 = dt.Rows[0]["gstin"].ToString();
                            }
                            break;
                        case "ITEM":
                            if (sgen.GetSession(MyGuid, "Ptype") != null)
                            {
                                mq = "select param1 from controls where type='CTL' and m_module3='-' and id in ('000008','000009','000010')";
                                DataTable dtm = sgen.getdata(userCode, mq);
                                if (dtm.Rows.Count > 0)
                                {
                                    taxrt1 = dtm.Rows[0]["param1"].ToString();//Social Welfare Surcharge
                                    taxrt2 = dtm.Rows[1]["param1"].ToString();//Anti-dumping Duty
                                    taxrt3 = dtm.Rows[2]["param1"].ToString();//Safeguard Duty
                                    if (taxrt1.Trim().Equals("Y")) { rtcond = rtcond + "hsn.QUALIFICATION_TYPE SocWelSur,"; }
                                    if (taxrt2.Trim().Equals("Y")) { rtcond = rtcond + "hsn.SECTIONTYPE Anti_dumping_Duty,"; }
                                    if (taxrt3.Trim().Equals("Y")) { rtcond = rtcond + "hsn.GROUP_REFRENCE_NUMBER Safegrd_Duty,"; }
                                }

                                if (sgen.GetSession(MyGuid, "Ptype").ToString() == "002")
                                {
                                    mq = "select '-' fstr,it.icode,it.iname,it.cpartno partno,um.master_name UOM,hsn.master_name hsn,hsn.group_id bcdrate," +
                                        "" + rtcond + "" +
                                        "'0' qtystk,'0' qtyord from item it " +
                                        "inner join master_setting um on um.master_id=it.uom and um.type='UMM' and find_in_set(um.client_unit_id,'" + unitid_mst + "')=1 " +
                                        "inner join master_setting hsn on hsn.master_id=it.hsno and hsn.type='HSN' and find_in_set(hsn.client_unit_id,'" + unitid_mst + "')=1 " +
                                        "where (it.icode||to_char(it.vch_date,'yyyymmdd')||it.type) in ('" + URL + "')";
                                }
                                else if (sgen.GetSession(MyGuid, "Ptype").ToString() == "001")
                                {
                                    // mq = "select (it.client_id||it.client_unit_id||it.icode||to_char(it.vch_date,'yyyymmdd')||it.type||ind.ind_no||to_char(ind.ind_date,'yyyymmdd')) fstr,it.icode as Icode," +
                                    // "it.iname as Iname,it.cpartno Partno,um.master_name as UOM,hsn.master_name as hsn,hsn.group_name taxrate,'0' qtystk,ind.qtyord,ind.ind_no," +
                                    // "to_char(convert_tz(ind.ind_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') ind_date from item it " +
                                    //"inner join master_setting um on um.master_id=it.uom and um.type='UMM' and find_in_set(um.client_id,'" + clientid_mst + "')=1 and find_in_set(um.client_unit_id,'" + unitid_mst + "')=1 " +
                                    //"inner join master_setting hsn on hsn.master_id=it.hsno and hsn.type='HSN' and find_in_set(hsn.client_id,'" + clientid_mst + "')=1 and find_in_set(hsn.client_unit_id,'" + unitid_mst + "')=1 " +
                                    //"inner join (select sum(a.qtyord)-nvl(sum(b.qtyord),0) as qtyord,a.icode,a.vch_date ind_date,a.vch_num as ind_no,a.client_id,a.client_unit_id from purchases a " +
                                    //"left join purchases b on a.icode=b.icode and b.type not in ('66') and a.client_id=b.client_id and a.client_unit_id=b.client_unit_id where a.type='66' and a.client_id='" + clientid_mst + "' " +
                                    //"and a.client_unit_id='" + unitid_mst + "' group by a.icode,a.vch_date,a.vch_num,a.client_id,a.client_unit_id) ind on ind.icode=it.icode and ind.client_id=it.client_id " +
                                    //"and ind.client_unit_id=it.client_unit_id " +
                                    //"where (it.client_id||it.client_unit_id||it.icode||to_char(it.vch_date,'yyyymmdd')||it.type||ind.ind_no||to_char(ind.ind_date,'yyyymmdd')) in ('" + URL + "')";
                                    //brijesh
                                    mq = "select (it.icode||to_char(it.vch_date,'yyyymmdd')||it.type||pc.ind_no||to_char(pc.vch_date,'yyyymmdd')) fstr," +
                                        "pc.ind_no,to_char(pc.vch_date,'" + sgen.Getsqldateformat() + "') as ind_date,pc.icode as Icode,it.iname as Iname,it.cpartno Partno," +
                                        "um.master_name UOM,hsn.master_name hsn,hsn.group_id bcdrate,hsn.QUALIFICATION_TYPE SocWelSur,hsn.SECTIONTYPE Anti_dumping_Duty," +
                                        "hsn.GROUP_REFRENCE_NUMBER Safegrd_Duty,pc.type,s.closing as qtystk,pc.ind_qty,pc.Bal_qty qtyord from item it " +
                                        "inner join (select a.ind_no,a.vch_date,a.type, a.icode,a.client_id,a.client_unit_id,a.ind_qty,sum(a.po_qty) used," +
                                        "(a.ind_qty - nvl(sum(a.po_qty), '0')) Bal_qty from(select nd.vch_num as ind_no,nd.vch_date,nd.type,nd.icode,nd.client_id," +
                                        "nd.client_unit_id,nd.qtyord as ind_qty,nvl(b.qtyord,'0') po_qty from purchases nd " +
                                        "left join purchases b on b.icode=nd.icode and b.indno=nd.vch_num and b.type not in ('66') and b.pur_type='001' and " +
                                        "nd.client_unit_id=b.client_unit_id where nd.client_unit_id='" + unitid_mst + "' and nd.type='66' " +
                                        "union all " +
                                        "select nd.vch_num as ind_no,nd.vch_date,nd.type,nd.icode,nd.client_id,nd.client_unit_id,nd.qtyord ind_qty,nvl(b.col11,'0') close_qty from purchases nd " +
                                        "left join enx_tab b on b.col7=nd.icode and b.col12=nd.vch_num and b.type = 'CPI' and nd.client_unit_id=b.client_unit_id " +
                                        "where nd.client_unit_id='" + unitid_mst + "' and nd.type='66') a " +
                                        "group by(a.ind_no,a.vch_date,a.type, a.icode,a.client_id,a.client_unit_id,a.ind_qty)) pc on pc.icode=it.icode and " +
                                        "find_in_set(it.client_id,pc.client_id)=1 and find_in_set(pc.client_unit_id,it.client_unit_id)=1 " +
                                        "inner join master_setting um on um.master_id=it.uom and um.type='UMM' and find_in_set(um.client_unit_id,'" + unitid_mst + "')=1 " +
                                        "inner join master_setting hsn on hsn.master_id=it.hsno and hsn.type='HSN' and find_in_set(hsn.client_unit_id,'" + unitid_mst + "')=1 " +
                                        "left join(select icode,client_unit_id,sum(op) op,sum(pktop) pktop,sum(qtyin) qtyin,sum(qtyout) out," +
                                      "sum(op)+sum(qtyin)-sum(qtyout) closing from (select icode, client_unit_id, nvl(op_2019, 0) op," +
                                      " nvl(pkt_2019, 0) pktop, 0 qtyin, 0 qtyout, 0 pktno from itbal union all select icode, client_unit_id, " +
                                      "sum(to_number(qtyin)) - sum(to_number(qtyout)) op, sum((case when substr(type, 1, 1) in ('0', '1') then" +
                                      " pktno when substr(type,1,1) in ('2', '3', '4') then - pktno else pktno end)) pktop,sum(to_number(qtyin)) " +
                                      "qtyin,sum(to_number(qtyout)) qtyout,0 pktno from itransaction where trim(store) = 'Y' and " +
                                      "to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and " +
                                      "to_date('" + year_from.Split(' ')[0].Trim() + "','" + sgen.Getsqldateformat() + "')-1 group by icode,client_unit_id union all select icode,client_unit_id," +
                                      "0 as op,0 as pktop,to_number(qtyin) qtyin,to_number(qtyout) qtyout,sum((case when substr(type, 1, 1) in " +
                                      "('0', '1') then pktno when substr(type,1,1) in ('2', '3', '4') then - pktno else pktno end)) pktno from " +
                                      "itransaction where trim(store) = 'Y' and to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                                      "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + model[0].Col48.Trim() + "','" + sgen.Getsqldateformat() + "') group by icode,client_unit_id," +
                                      "to_number(qtyin),to_number(qtyout)) a group by icode, client_unit_id) s on it.icode = s.icode and find_in_set(s.client_unit_id, it.client_unit_id)=1 " +
                                        "where (it.icode||to_char(it.vch_date,'yyyymmdd')||it.type||pc.ind_no||to_char(pc.vch_date,'yyyymmdd')) in ('" + URL + "')";
                                }
                                dt = sgen.getdata(userCode, mq);
                                if (dt.Rows.Count > 0)
                                {
                                    //if (model[0].Col12 == "50" || model[0].Col12 == "52" || model[0].Col12 == "54")
                                    //{
                                    //    if (model[0].Col29 == "002")
                                    //    {
                                    //        if (model[0].dt1.Rows[0]["icode"].ToString().Trim().Equals("-")) { model[0].dt1.Rows.RemoveAt(0); }
                                    //    }
                                    //    else
                                    //    {
                                    //        if (model[0].dt2.Rows[0]["icode"].ToString().Trim().Equals("-")) { model[0].dt2.Rows.RemoveAt(0); }
                                    //    }
                                    //}
                                    //else
                                    //{
                                    if (model[0].dt1.Rows[0]["icode"].ToString().Trim().Equals("-")) { model[0].dt1.Rows.RemoveAt(0); }
                                    //}
                                    DataRow dr;
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        //if (model[0].Col12 == "50" || model[0].Col12 == "52" || model[0].Col12 == "54")
                                        //{
                                        //    if (model[0].Col29 == "002") { dr = model[0].dt1.NewRow(); }
                                        //    else { dr = model[0].dt2.NewRow(); }
                                        //}
                                        //else { dr = model[0].dt1.NewRow(); }
                                        dr = model[0].dt1.NewRow();
                                        dr["Item"] = dt.Rows[i]["fstr"].ToString();
                                        dr["SNo"] = i + 1;
                                        dr["Icode"] = dt.Rows[i]["icode"].ToString();
                                        dr["IName"] = dt.Rows[i]["iname"].ToString();
                                        dr["PARTNO"] = dt.Rows[i]["partno"].ToString();
                                        dr["UOM"] = dt.Rows[i]["uom"].ToString();
                                        dr["HSN"] = dt.Rows[i]["hsn"].ToString();
                                        dr["Bcdrate"] = dt.Rows[i]["bcdrate"].ToString();
                                        try { dr["Soc_Welf_Sur"] = dt.Rows[i]["SocWelSur"].ToString(); }
                                        catch (Exception er) { }
                                        try { dr["Anti_dumping_Duty"] = dt.Rows[i]["Anti_dumping_Duty"].ToString(); }
                                        catch (Exception er) { }
                                        try { dr["Safeguard_Duty"] = dt.Rows[i]["Safegrd_Duty"].ToString(); }
                                        catch (Exception er) { }
                                        dr["Qty_In_Stock"] = dt.Rows[i]["qtystk"].ToString();
                                        dr["Qty_ord"] = dt.Rows[i]["qtyord"].ToString();
                                        if (sgen.GetSession(MyGuid, "Ptype").ToString() == "001")
                                        {
                                            dr["Qty_req"] = dt.Rows[i]["qtyord"].ToString();
                                            dr["IndentNo"] = dt.Rows[i]["ind_no"].ToString();
                                            dr["Indent_Date"] = dt.Rows[i]["ind_date"].ToString();
                                        }
                                        dr["DocCurrency"] = model[0].TList7.SingleOrDefault(w => w.Value == model[0].SelectedItems7.FirstOrDefault()).Text;
                                        //if (model[0].Col12 == "50" || model[0].Col12 == "52" || model[0].Col12 == "54")
                                        //{
                                        //    if (model[0].Col29 == "002") { model[0].dt1.Rows.Add(dr); }
                                        //    else { model[0].dt2.Rows.Add(dr); }
                                        //}
                                        //else { model[0].dt1.Rows.Add(dr); }
                                        model[0].dt1.Rows.Add(dr);

                                        mq = "select master_name from master_setting where type='FCR' and classid='" + model[0].SelectedItems7.FirstOrDefault() + "' " +
                                            "and client_unit_id='" + unitid_mst + "'";
                                        mq = sgen.seekval(userCode, mq, "master_name");
                                        model[0].Col30 = mq;
                                    }
                                }
                            }
                            break;
                        case "PRINT":
                            ViewBag.vnew = "";
                            ViewBag.vedit = "";
                            ViewBag.vsave = "disabled='disabled'";
                            ViewBag.scripCall = "disableForm();";
                            mq = "select a.acode,nvl(um.master_name,'0') UOM,nvl(hsn.master_name,'0') hsn,a.cond,a.vch_num,a.icode,a.taxrate,to_number(a.qtystk ) qtystk," +
                                "to_number(a.qtyord) qtyord,to_number(a.irate) irate,to_number(a.discrate) discrate,to_number(a.iamount) iamount,to_number(a.taxamt) taxamt," +
                                "to_number(a.discamt) discamt,to_number(a.lineamount) lineamount,a.iremark,a.indno,a.totremark,to_number(a.basicamt) basicamt,to_number(a.gdisc) gdisc," +
                                "to_number(a.gfreight) gfreight,to_number(a.insurance) insurance,to_number(a.ginstlchrg) ginstlchrg,to_number(a.gserchrg) gserchrg,to_number(a.gamc) gamc," +
                                "to_number(a.gloadchrg) gloadchrg,to_number(a.gothrchrg) gothrchrg,to_number(a.gtaxamt) gtaxamt,to_number(a.igst) igst,to_number(a.cgst) cgst," +
                                "to_number(a.sgst) sgst,to_number(a.gamt) gamt,to_char(convert_tz(a.vch_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') vch_date1," +
                                "to_char(convert_tz(a.dlv_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') dlv_date1,to_char(convert_tz(a.inddate, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') inddate1," +
                                "c.c_name as name,c.c_gstin as gstin,c.addr,c.tor,c.cpcont,nvl(s.c_name, '0') sf_name,nvl(s.c_gstin, '0') sf_gstin,nvl(s.addr, '0') as sf_addr," +
                                "nvl(s.tor, '0') as sf_tor,nvl(s.cpcont, '0') sf_contact,nvl(t.c_name, '0') st_name,nvl(t.c_gstin, '0') tf_gstin,nvl(t.addr, '0') as st_addr," +
                                "nvl(t.tor, '0') as st_tor,nvl(t.cpcont, '0') st_contact,nvl(py.master_name, '0') py_term,nvl(pr.master_name, '0') pr_term,nvl(mt.master_name, '0') trans," +
                                "i.cpartno as partno,i.iname as item_name from purchases a " +
                                "inner join item i on a.icode = i.icode  and i.type = 'IT' and find_in_set(a.client_unit_id, i.client_unit_id)=1 " +
                                "left join clients_mst c on c.vch_num = a.acode and c.type = 'BCD' and substr(c.vch_num,0,3)='203' and find_in_set(c.client_unit_id, a.client_unit_id)=1 " +
                                "left join clients_mst s on s.vch_num = a.shipfrom and s.type = 'BCD' and substr(s.vch_num,0,3)='203' and find_in_set(s.client_unit_id, a.client_unit_id)=1 " +
                                "left join clients_mst t on t.vch_num = a.shipto and t.type = 'BCD' and substr(t.vch_num,0,3)='203' and find_in_set(t.client_unit_id, a.client_unit_id)=1 " +
                                "left join master_setting py on a.pay_term = py.master_id and py.type = 'PTM' and find_in_set(a.client_unit_id, py.client_unit_id)= 1 " +
                                "left join master_setting pr on a.price_term = pr.master_id and pr.type = 'PRI' and find_in_set(a.client_unit_id, pr.client_unit_id)= 1 " +
                                "left join master_setting mt on a.tmode = mt.master_id and mt.type = 'AMT' and find_in_set(a.client_unit_id, mt.client_unit_id)= 1 " +
                                "left join master_setting um on um.master_id = i.uom and um.type = 'UMM' and find_in_set(um.client_unit_id,'" + unitid_mst + "')= 1 " +
                                "left join master_setting hsn on hsn.master_id = i.hsno and hsn.type = 'HSN' and find_in_set(hsn.client_unit_id,'" + unitid_mst + "')= 1 " +
                                "where(a.client_id || a.client_unit_id || a.vch_num || to_char(a.vch_date, 'yyyymmdd') || a.type) = '" + URL + "'";
                            dtp = sgen.getdata(userCode, mq);
                            if (dtp.Rows.Count > 0)
                            {
                                sgen.open_report_bydt_xml(userCode, dtp, "PO3", "Purchase Order");
                                ViewBag.scripCall += "showRptnew('Purchase Order Detail');disableForm();";

                                //ViewBag.scripCall = "OpenSingle('../../../../../erp/schoolReports_Rpts/Filter_Report_xml.aspx','95%','900px','Reports');";
                            }
                            break;
                        case "CHRGS":
                            mq = "select master_id,master_name from master_setting where (client_id||client_unit_id||master_id||to_char(vch_date,'yyyymmdd')||type) in ('" + URL + "')";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                int i = 0;
                                if (model[0].LTM1 == null) { model[0].LTM1 = new List<Tmodelmain>(); }
                                if (model[0].LTM1.Count == 0) { model[0].LTM1 = new List<Tmodelmain>(); }
                                else i = model[0].LTM1.Count;
                                foreach (DataRow dr in dtt.Rows)
                                {
                                    i++;
                                    model[0].LTM1.Add(new Tmodelmain
                                    {
                                        Col14 = i.ToString(),
                                        Col15 = dr["master_id"].ToString(),
                                        Col23 = dr["master_name"].ToString(),
                                        Col16 = "0",
                                        Col17 = "0",
                                        Col18 = "0",
                                        Col19 = "0",
                                        Col20 = "0",
                                        Col21 = "0",
                                        Col22 = "0",
                                    });
                                }
                            }
                            break;
                        case "CHRGDEL":
                            if (sgen.GetSession(MyGuid, "podelrno") != null)
                            {
                                var mrno = sgen.GetSession(MyGuid, "podelrno").ToString();
                                sgen.SetSession(MyGuid, "podelrno", null);
                                var LTM = model[0].LTM1;
                                for (int d = 0; d < LTM.Count; d++)
                                {
                                    string chrgno = LTM[d].Col14;
                                    if (chrgno.Trim() == mrno.Trim()) { model[0].LTM1.RemoveAt(d); }
                                }
                                sgen.SetSession(MyGuid, "podelrno", null);
                            }
                            break;
                        case "RATE":
                            if (sgen.GetSession(MyGuid, "rtrno") == null)
                            {
                                ViewBag.scripCall += "showmsgJS(1,'Rno not defined', 0);";
                                return model;
                            }
                            var rno = sgen.Make_int(sgen.GetSession(MyGuid, "rtrno").ToString());
                            mq = "select master_id DocNo,group_name TaxRate from master_setting where (client_id||client_unit_id||master_id||to_char(vch_date,'yyyymmdd')||type)='" + URL + "'";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                var utgst = "";
                                model[0].LTM1[rno - 1].Col13 = dtt.Rows[0]["DocNo"].ToString().Trim();
                                try { utgst = model[0].Col18.Substring(0, 2).Trim(); }
                                catch (Exception ex) { utgst = model[0].Col18; }
                                var pgst = model[0].Col22.Substring(0, 2).Trim();
                                if (utgst == pgst)
                                {
                                    var amt = sgen.Make_decimal(model[0].LTM1[rno - 1].Col16);
                                    var rt = sgen.Make_decimal(dtt.Rows[0]["TaxRate"].ToString().Trim().Split('%')[0].Trim());
                                    var amtrt = (amt * rt) / 100;
                                    model[0].LTM1[rno - 1].Col19 = (rt / 2).ToString() + "%";
                                    model[0].LTM1[rno - 1].Col20 = (amtrt / 2).ToString();
                                    model[0].LTM1[rno - 1].Col21 = (rt / 2).ToString() + "%";
                                    model[0].LTM1[rno - 1].Col22 = (amtrt / 2).ToString();
                                }
                                else
                                {
                                    var amt = sgen.Make_decimal(model[0].LTM1[rno - 1].Col16);
                                    var rt = sgen.Make_decimal(dtt.Rows[0]["TaxRate"].ToString().Trim().Split('%')[0].Trim());
                                    var amtrt = (amt * rt) / 100;
                                    model[0].LTM1[rno - 1].Col17 = rt.ToString() + "%";
                                    model[0].LTM1[rno - 1].Col18 = amtrt.ToString();
                                }
                                decimal bamt = sgen.Make_decimal(model[0].Col33);//basicamt
                                decimal bigst = sgen.Make_decimal(model[0].Col34);//basicigst
                                decimal bcgst = sgen.Make_decimal(model[0].Col35);//basiccgst
                                decimal bsgst = sgen.Make_decimal(model[0].Col36);//basicsgst
                                decimal tamt = 0, tigst = 0, tcgst = 0, tsgst = 0;
                                for (int i = 0; i < model[0].LTM1.Count; i++)
                                {
                                    tamt = tamt + sgen.Make_decimal(model[0].LTM1[i].Col16);//chrgamt
                                    tigst = tigst + sgen.Make_decimal(model[0].LTM1[i].Col18);//chrgigst
                                    tcgst = tcgst + sgen.Make_decimal(model[0].LTM1[i].Col20);//chrgcgst
                                    tsgst = tsgst + sgen.Make_decimal(model[0].LTM1[i].Col22);//chrgsgst
                                }
                                model[0].Col37 = (bamt + tamt).ToString();//totamount
                                model[0].Col38 = (bigst + tigst).ToString();//totigst
                                model[0].Col39 = (bcgst + tcgst).ToString();//totcgst
                                model[0].Col40 = (bsgst + tsgst).ToString();//totsgst
                                model[0].Col69 = (bamt + tamt + bigst + tigst + bcgst + tcgst + bsgst + tsgst).ToString();//netamt
                            }
                            break;
                    }
                    break;
                #endregion
                #region Vendor Approval
                case "vendor_app":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select a.*,i.iname,b.c_name as vname ,to_char(convert_tz(a.vch_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') vch_date1" +
                                ",to_char(convert_tz(a.date1,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') eff_from," +
                                "to_char(convert_tz(a.date2,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') eff_to,i.cpartno as partno" +
                                " ,(s.state_name|| ',' ||cs.country_name ||  ','||b.addr || ',' || b.pincode) as Full_Address" +
                                " from ivendor a inner join item i on a.icode=i.icode  and i.type='IT'" +
                               " and a.client_unit_id=i.client_unit_id inner join clients_mst b on a.acode=b.vch_num and b.type='BCD' and substr(b.vch_num,0,3)='203' and a.client_unit_id=b.client_unit_id" +
                               " left join (select distinct alpha_2,country_name from country_state) cs on b.country = cs.alpha_2 left join" +
                               "(select distinct state_gst_code, state_name from country_state ) s on b.state = s.state_gst_code  where" +
                               " (a.client_id||a.client_unit_id||a.vch_num|| to_char(a.vch_date,'yyyymmdd')||a.type)='" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col1 = dt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dt.Rows[0]["ent_date"].ToString();
                                model[0].Col12 = "VAP";
                                model[0].Col16 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dt.Rows[0]["vch_date1"].ToString();
                                model[0].Col8 = "(client_id||client_unit_id||vch_num|| to_char(vch_date,'yyyymmdd')||type) = '" + URL + "'";
                                model[0].Col13 = "Update";
                                model[0].Col18 = dt.Rows[0]["vname"].ToString();
                                model[0].Col21 = dt.Rows[0]["acode"].ToString();
                                model[0].Col19 = dt.Rows[0]["eff_from"].ToString();
                                model[0].Col20 = dt.Rows[0]["eff_to"].ToString();
                                model[0].Col22 = dt.Rows[0]["Full_Address"].ToString();
                                model[0].dt1 = new DataTable();
                                model[0].dt1 = ((DataTable)sgen.GetSession(MyGuid, "RVA_DT")).Clone();
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["SNo"] = i + 1;
                                    dr["Icode"] = dt.Rows[i]["icode"].ToString();
                                    dr["IName"] = dt.Rows[i]["iname"].ToString();
                                    dr["P_unit"] = dt.Rows[i]["uom"].ToString();
                                    dr["S_Unit"] = dt.Rows[i]["uom2"].ToString();
                                    dr["Part_No"] = dt.Rows[i]["partno"].ToString();
                                    dr["PartName"] = dt.Rows[i]["partname"].ToString();
                                    dr["IRate"] = dt.Rows[i]["IRate"].ToString();
                                    dr["DiscAmt"] = dt.Rows[i]["DiscAmt"].ToString();
                                    dr["DiscRate"] = dt.Rows[i]["DiscRate"].ToString();
                                    model[0].dt1.Rows.Add(dr);
                                }
                                model[0].dt1.AcceptChanges();
                            }
                            break;
                        case "PARTY":
                            mq = "select a.c_name as name,a.c_gstin as gstin,a.addr as Address,a.vch_num,a.tor ," +
                                "(s.state_name|| ',' ||cs.country_name ||  ','||a.addr || ',' || a.pincode) as Full_Address from" +
                                " clients_mst a left join (select distinct alpha_2,country_name from country_state) cs on a.country = cs.alpha_2 left join" +
                                "(select distinct state_gst_code, state_name from country_state ) s on a.state = s.state_gst_code " +
                                " where (a.vch_num ||to_char(a.vch_date, 'yyyymmdd') || a.type) = '" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model[0].Col18 = dt.Rows[0]["name"].ToString();
                                model[0].Col21 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col22 = dt.Rows[0]["Full_Address"].ToString();
                            }
                            break;
                        case "ITEM":
                            mq = "select '-' fstr,it.icode,it.iname,it.cpartno as partno,it.partname,um.master_name as UOM,um.master_name as UOM2 " +
                                "from item it " +
                                "inner join master_setting um on um.master_id=it.uom and um.type='UMM' and find_in_set(um.client_unit_id,'" + unitid_mst + "')=1 " +
                                "inner join master_setting um2 on um2.master_id=it.uom2 and um2.type='UMM' and find_in_set(um2.client_unit_id,'" + unitid_mst + "')=1 " +
                                "where (it.icode||to_char(it.vch_date,'yyyymmdd')||it.type) in ('" + URL + "')";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                if (model[0].dt1.Rows[0]["icode"].ToString().Trim().Equals("-")) { model[0].dt1.Rows.RemoveAt(0); }
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["Item"] = dt.Rows[i]["fstr"].ToString();
                                    dr["SNo"] = i + 1;
                                    dr["Icode"] = dt.Rows[i]["icode"].ToString();
                                    dr["IName"] = dt.Rows[i]["iname"].ToString();
                                    dr["P_unit"] = dt.Rows[i]["uom"].ToString();
                                    dr["S_Unit"] = dt.Rows[i]["uom2"].ToString();
                                    dr["Part_No"] = dt.Rows[i]["partno"].ToString();
                                    dr["PartName"] = dt.Rows[i]["partname"].ToString();
                                    model[0].dt1.Rows.Add(dr);
                                }
                                model[0].dt1.AcceptChanges();
                            }
                            break;
                    }
                    break;
                #endregion
                #region pr_app
                case "pr_app":
                    switch (btnval.ToUpper())
                    {
                        case "SHOW":
                            cmd = "SELECT  a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type||a.icode  as fstr," +
                                "a.vch_num as Indent_No,to_char(a.vch_date, '" + sgen.Getsqldateformat() + "') Indent_Date,a.icode,b.iname ," +
                                "b.uom,um.master_name as UOM_name,a.qtyord as Indent_Qty,a.qtyord as App_Qty,hsn.master_name as hsn,hsn.group_name taxrate," +
                                "b.cpartno FROM purchases a INNER join item b on a.icode = b.icode and b.type = 'IT' and find_in_set(b.client_unit_id, a.client_unit_id)=1 inner join" +
                                " master_setting um on um.master_id = b.uom and um.type = 'UMM' and a.client_unit_id = um.client_unit_id " +
                                "inner join master_setting hsn on hsn.master_id = b.hsno and hsn.type = 'HSN' and a.client_unit_id = hsn.client_unit_id " +
                                "where a.client_unit_id = '" + unitid_mst + "' and  a.type = '66' and a.client_unit_id || a.vch_num || to_char(a.vch_date, 'yyyymmdd') || a.type || a.icode in  " +
                                "('" + URL + "')  order by a.vch_num";
                            DataTable dtm = sgen.getdata(userCode, cmd);
                            if (dtm.Rows.Count < 1) { return model; }
                            model[0].dt1 = new DataTable();
                            model[0].dt1 = ((DataTable)sgen.GetSession(MyGuid, "RPR_DT")).Clone();
                            for (int i = 0; i < dtm.Rows.Count; i++)
                            {
                                DataRow dr = model[0].dt1.NewRow();
                                dr["SNo"] = i + 1;
                                dr["Icode"] = dtm.Rows[i]["icode"].ToString();
                                dr["IName"] = dtm.Rows[i]["iname"].ToString();
                                dr["UOM"] = dtm.Rows[i]["UOM_name"].ToString();
                                dr["Indent_No"] = dtm.Rows[i]["Indent_No"].ToString();
                                dr["Indent_Date"] = dtm.Rows[i]["Indent_Date"].ToString();
                                dr["Indent_Qty"] = dtm.Rows[i]["Indent_Qty"].ToString();
                                dr["Order_Qty"] = dtm.Rows[i]["App_Qty"].ToString();
                                dr["HSN_NO"] = dtm.Rows[i]["hsn"].ToString();
                                dr["Tax_Rate"] = dtm.Rows[i]["taxrate"].ToString();
                                dr["Part_No"] = dtm.Rows[i]["cpartno"].ToString();
                                model[0].dt1.Rows.Add(dr);
                            }
                            model[0].dt1.AcceptChanges();
                            break;
                        case "LSR":
                            cmd = "";
                            break;
                    }
                    break;
                #endregion
                #region party_detail
                case "party":
                    switch (btnval.ToUpper())
                    {
                        case "PARENT":
                            mq = "select a.vch_num, a.c_name from clients_mst a where a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type ='" + URL + "' order by a.vch_num desc";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model[0].Col94 = dt.Rows[0]["c_name"].ToString();
                                model[0].Col95 = dt.Rows[0]["vch_num"].ToString();
                            }
                            break;
                        case "EXISTING":
                            mq = "select a.vch_num, a.c_name from clients_mst a where a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type ='" + URL + "' order by a.vch_num desc";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model[0].Col137 = dt.Rows[0]["c_name"].ToString();
                                model[0].Col138 = dt.Rows[0]["vch_num"].ToString();
                            }
                            break;
                        case "EDIT":
                        case "VIEW":
                        case "PROSPECT":
                        case "40002.6"://list of account and crm summary
                        case "28005.7":// list of vendors
                        case "21008.2":// List  of customers
                            string contact_type = "";
                            //if (model[0].Col15 == "28005.1")
                            //{ contact_type = "BCN"; }
                            contact_type = sgen.GetSession(MyGuid, "VD_CONT").ToString().Trim();
                            string prefix = sgen.GetSession(MyGuid, "VD_PREFIX").ToString().Trim();
                            if ((actionName == "party") && (btnval == "PROSPECT"))
                            {
                                contact_type = "PON";
                            }
                            mq = "select  cp.addtype3,cp.ship_ADD,cp.country3,cp.state3,cp.city3,cp.cpaddr3,cp.pincode_3," +
                                "cp.istds,cp.d_name,cp.d_pan,cp.d_type,cp.isnri,cp.nri_tax,cp.nri_country,cp.d_add,cp.d_address," +
                                "cp.d_pincode,cp.d_city,cp.d_state, cp.d_country,cp.tds_d,cp.tds_rate,cp.tds_limit," +
                                "cp.d_lower,cp.d_cert,to_char(cp.d_valid, '" + sgen.Getsqldateformat() + "')d_valid ,sa.country_name as country_name3" +
                                ",sa.state_name as state_name3,sa.city_name as city_name3,sa.std_code std_code3,da.country_name as country_named," +
                                "da.state_name as state_named,da.city_name as city_named,cp.country country_mn " +
                                ",cp.state state_mn ,cp.city_mn, ct.cp_gender, cp.cpcontother,cp.cpemailother, cp.taxable,cp.ghsub,cp.s_citizen, " +
                                "(case when cp.org_mobile='0' then '' else cp.org_mobile end) org_mobile," +
                                "(case when cp.org_contact='0' then '' else cp.org_contact end) org_contact," +
                                "cp.Isorg_email,cp.ISorg_sms,cp.Iscp_email,cp.iscp_sms, cp.mftr,cp.trans," +
                                " cp.risk_ctg, cp.pubr, cp.d_limit,cp.s_limit, cp.credit_days, cp.pay_mode,cp.pay_mode2,cp.cheq_print, cp.interunit," +
                                "(case when length(cp.tin_no)=1  and cp.tin_no='0' then '-' else cp.tin_no end ) tin_no," +
                                " (case when length(w.c_name)=1  and w.c_name='0' then '-' else w.c_name end )  as ref_name, " +
                                "(case when length(cp.ref_Ext_acc)=1  and cp.ref_Ext_acc='0' then '-' else cp.ref_Ext_acc end ) ref_Ext_acc," +
                                "(case when length(cp.ref_user)=1  and cp.ref_user='0' then '-' else cp.ref_user end ) ref_user," +
                                " (case when length(ct.vch_num)=1  and ct.vch_num='0' then '-' else ct.vch_num end )  ct_vch_num, " +
                                "(case when length(ct.ref_code)=1  and ct.ref_code='0' then '-' else ct.ref_code end ) ref_code, cp.credit_rating, " +
                                "(case when length(ct.cp_alias_name)=1  and ct.cp_alias_name='0' then '-' else ct.cp_alias_name end ) cp_alias_name," +
                                " (case when length(cp.rel_mgr)=1  and cp.rel_mgr='0' then '-' else cp.rel_mgr end ) rel_mgr,cp.sub_broker,cp.client_rating,cp.qualification,cp.dp,cp.pref_add," +
                                "(case when length(cp.alias_name)=1  and cp.alias_name='0' then '-' else cp.alias_name end ) alias_name," +
                                "(case when length(cp.aadhar_no)=1  and cp.aadhar_no='0' then '-' else cp.aadhar_no end ) aadhar_no," +
                                "(case when length(cp.dp_name)=1  and cp.dp_name='0' then '-' else cp.dp_name end ) dp_name," +
                                "(case when length(cp.dp_id)=1  and cp.dp_id='0' then '-' else cp.dp_id end ) dp_id," +
                                "(case when length(cp.ben_acc)=1  and cp.ben_acc='0' then '-' else cp.ben_acc end ) ben_acc," +
                                "(case when length(cp.uin_no)=1  and cp.uin_no='0' then '-' else cp.uin_no end ) uin_no," +
                                "(case when length(cp.min_no)=1  and cp.min_no='0' then '-' else cp.min_no end ) min_no,cp.ann_inc," +
                                "(case when length(cp.employer)=1  and cp.employer='0' then '-' else cp.employer end ) employer ," +
                                "(case when length(cp.infavour)=1  and cp.infavour='0' then '-' else cp.infavour end ) infavour ," +
                                "(case when to_char(cp.kyc_dt, '" + sgen.Getsqldateformat() + "') = '01/01/1900' then '' else to_char(cp.kyc_dt, '" + sgen.Getsqldateformat() + "') end )  kyc_dt, " +
                                "(case when to_char(cp.msme_cert, '" + sgen.Getsqldateformat() + "') = '01/01/1900' then '' else to_char(cp.msme_cert, '" + sgen.Getsqldateformat() + "') end )  msme_cert,   " +
                                "(case when to_char(cp.msme_upto, '" + sgen.Getsqldateformat() + "') = '01/01/1900' then '' else to_char(cp.msme_upto, '" + sgen.Getsqldateformat() + "') end )  msme_upto," +
                                "(case when to_char(cp.msme_from, '" + sgen.Getsqldateformat() + "') = '01/01/1900' then '' else to_char(cp.msme_from, '" + sgen.Getsqldateformat() + "') end )  msme_from," +
                                "cp.fix_credit,cp.temp_credit,cp.valid_credit, cp.pay_term,cp.pay_term2, " +
                                "(case when length(cp.cfrm)=1  and cp.cfrm='0' then '-' else cp.cfrm end ) cfrm, cp.parent_id," +
                                "(case when length(pr.c_name)=1  and pr.c_name='0' then '-' else pr.c_name end ) parent_name, " +
                                "cp.prd_type, cp.bsn_type," +
                                " (case when length(cp.CIN_Number)=1  and cp.CIN_Number='0' then '-' else cp.CIN_Number end ) CIN_Number," +
                                "(case when  cp.comp_email='ab@ab.ab' then '' else cp.comp_email end ) comp_email, " +
                                "(case when length(cp.cp_mname)=1  and cp.cp_mname='0' then '-' else cp.cp_mname end )  cp_mname," +
                                "(case when length(cp.cp_lname)=1  and cp.cp_lname='0' then '-' else cp.cp_lname end ) cp_lname," +
                                "(case when length(ct.cp_mname)=1  and ct.cp_mname='0' then '-' else ct.cp_mname end ) ct_cp_mname," +
                                "(case when length(ct.cp_lname)=1  and ct.cp_lname='0' then '-' else ct.cp_lname end )  as ct_cp_lname, cp.leadsrc," +
                                " (case when length(cp.website)=1  and cp.website='0' then '-' else cp.website end )  website," +
                                "(case when length(cp.addtype1)=1  and cp.addtype1='0' then '-' else cp.addtype1 end ) addtype1," +
                                "(case when length(cp.addtype2)=1  and cp.addtype2='0' then '-' else cp.addtype2 end ) addtype2, cp.salesarea, ct.rec_id " +
                                "as ct_rec_id, ct.ref_code||to_char(ct.vch_date, 'yyyymmdd')|| ct.type as ct_fstr, ct.cpname as c_cpname" +
                                ",(case when ct.cpcont='0000000000' then '' else ct.cpcont end) as c_cpcont," +
                                "(case when ct.cpaltcont='0000000000' then '' else ct.cpaltcont end) as c_cpaltcont," +
                                "(case when  ct.cpemail='ab@ab.ab' then '' else ct.cpemail end )  c_cpemail," +
                                "(case when length(ct.cpaddr)=1  and ct.cpaddr='0' then '-' else ct.cpaddr end )  c_cpaddr," +
                                "(case when length(ct.cpdesig)=1  and ct.cpdesig='0' then '-' else ct.cpdesig end )  c_cpdesig," +
                                "(case when ct.cplandno='0000000000' then '' else ct.cplandno end) as c_cplandno," +
                                "(case when length(ct.cpdept)=1  and ct.cpdept='0' then '-' else ct.cpdept end )  c_cpdept," +
                                "(case when to_char(ct.DOB, '" + sgen.Getsqldateformat() + "') = '01/01/1900' then '' else to_char(ct.DOB, '" + sgen.Getsqldateformat() + "') end )  c_DOB," +
                                "(case when to_char(ct.DOA, '" + sgen.Getsqldateformat() + "') = '01/01/1900' then '' else to_char(ct.DOA, '" + sgen.Getsqldateformat() + "') end )  c_doa," +
                                "(case when length(cp.pincode_2)=1  and cp.pincode_2='0' then '' else cp.pincode_2 end )  pincode_2," +
                                "cp.sch_cat,cp.sch_med,cp.no_std,cp.no_teach,cp.Aff_type," +
                                "(case when length(cp.cpdept)=1  and cp.cpdept='0' then '-' else cp.cpdept end ) cpdept," +
                                "(case when length(cp.cpname)=1  and cp.cpname='0' then '-' else cp.cpname end ) cpname," +
                                "(case when length(cp.type_desc)=1  and cp.type_desc='0' then '-' else cp.type_desc end ) type_desc," +
                                "(case when length(cp.isgstr)=1  and cp.isgstr='0' then '-' else cp.isgstr end ) isgstr," +
                                "(case when length(cp.c_gstin)=1  and cp.c_gstin='0' then '-' else cp.c_gstin end ) c_gstin, " +
                                "(case when length(cp.micrno)=1  and cp.micrno='0' then '-' else cp.micrno end ) micrno, " +
                                "(case when length(cp.tor)=1  and cp.tor='0' then '-' else cp.tor end ) tor, " +
                                "(case when length(cp.status)=1  and cp.status='0' then '-' else cp.status end ) status," +
                                "(case when length(cp.pincode)=1  and cp.pincode='0' then '-' else cp.pincode end ) pincode," +
                                "(case when length(cp.addr)=1  and cp.addr='0' then '-' else cp.addr end ) addr, " +
                                "(case when length(cp.ifsc)=1  and cp.ifsc='0' then '-' else cp.ifsc end ) ifsc, " +
                                " (case when length(cp.acctno)=1  and cp.acctno='0' then '-' else cp.acctno end ) acctno, cp.contr, " +
                                "(case when length(cp.swftcd)=1  and cp.swftcd='0' then '-' else cp.swftcd end ) swftcd, cp.bnk, cp.curtype, cp.acctype,  cp.ploc, cp.ptype," +
                                "(case when length(cp.tanno)=1  and cp.tanno='0' then '-' else cp.tanno end ) tanno," +
                                " (case when length(cp.msmeno)=1  and cp.msmeno='0' then '-' else cp.msmeno end ) msmeno, " +
                                "(case when length(cp.micrno)=1  and cp.micrno='0' then '-' else cp.msmeno end ) micrno," +
                                "(case when length(cp.bnkaddr)=1  and cp.bnkaddr='0' then '-' else cp.bnkaddr end ) bnkaddr, " +
                                "(case when length(cp.cpdesig)=1  and cp.cpdesig='0' then '-' else cp.cpdesig end ) cpdesig," +
                                "(case when length(cp.lsrno)=1  and cp.lsrno='0' then '-' else cp.lsrno end ) lsrno," +
                                " cp.sez, cp.PR_TYPE ," +
                                "(case when length(cp.refered_by)=1  and cp.refered_by='0' then '-' else cp.refered_by end )  refered_by, " +
                                "(case when length(cp.file_no)=1  and cp.file_no='0' then '-' else cp.file_no end ) file_no,cp.occ_type," +
                                "(case when length(cp.panno)=1  and cp.panno='0' then '' when cp.panno='AAAAA0000A' then '' else cp.panno end ) panno," +
                                "(case when length(cp.BILL_ADD)=1  and cp.BILL_ADD='0' then '-' else cp.BILL_ADD end ) BILL_ADD,cp.rec_id," +
                                "(case when length(cp.c_name)=1  and cp.c_name='0' then '-' else cp.c_name end ) c_name ," +
                                " (case when to_char(cp.dob, '" + sgen.Getsqldateformat() + "') = '01/01/1900' then '' else to_char(cp.dob, '" + sgen.Getsqldateformat() + "') end ) dob," +
                                " (case when to_char(cp.doa, '" + sgen.Getsqldateformat() + "') = '01/01/1900' then '' else to_char(cp.doa, '" + sgen.Getsqldateformat() + "') end )  doa," +
                                "( case when to_char(convert_tz(cp.mt_dt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "')= '01/01/1900' then '' else to_char(convert_tz(cp.mt_dt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') end) mt_dt," +
                        "(case when  cp.cpemail='ab@ab.ab' then '' else cp.cpemail end ) cpemail ,cs.alpha_2 as country_id,cs.country_name,cs.std_code,cs.state_gst_code as state_id ,cs.state_name," +
                       "cs.district_name,cs.city_name,cs.teh_name,cs.v_name,cp.city" +
                           ",ba.alpha_2 as country_id2,ba.country_name as country_name2,ba.std_code std_code2, ba.state_gst_code as state_id2 ," +
                             "ba.state_name as state_name2 ,ba.district_name as district_name2,ba.city_name as city_name2," +
                             "ba.teh_name as teh_name2,ba.v_name as v_name2 ,cp.city2," +
                             "(case when length(cp.cpaddr2)=1  and cp.cpaddr2='0' then '-' else cp.cpaddr2 end ) cpaddr2," +
                              "(case when length(cp.c_gstin)=1  and cp.c_gstin='0' then '-' else cp.c_gstin end )  cust_gstin_no, (case when cp.CPLANDNO='0000000000' then '' else cp.CPLANDNO end) CPLANDNO, " +
                              "(case when cp.cpcont='0000000000' then '' else cp.cpcont end) cpcont, (case when cp.cpaltcont='0000000000' then '' else cp.cpaltcont end) cpaltcont ," +
                              "(case when length(cp.cpaddr)=1  and cp.cpaddr='0' then '-' else cp.cpaddr end )  cpaddr,ms.master_name as prop_type," +
                              "(case when length(cp.remark)=1  and cp.remark='0' then '-' else cp.remark end )  remark,cp.client_id," +
"cp.client_unit_id,cp.vch_num,cp.ent_by,cp.ent_date,cp.latlong,cp.google_add,cp.client_type,cp.trcom " +
"from clients_mst cp left join clients_mst ct" +
" on cp.vch_num=ct.ref_code and find_in_set(cp.client_unit_id,ct.client_unit_id)=1 and ct.type='" + contact_type + "' " +
" left join country_state cs on cs.rec_id = cp.city " +
" left join country_state ba on ba.rec_id = cp.city2" +
" left join country_state sa on sa.rec_id = cp.city3 " +
" left join country_state da on da.rec_id = cp.d_city " +
" left join master_setting ms on ms.master_id = cp.PR_TYPE and ms.type='PRT' and ms.client_unit_id='" + unitid_mst + "'" +
" left join clients_mst pr  on cp.parent_id=pr.vch_num and pr.type='BCD' and  find_in_set(pr.client_unit_id,'" + unitid_mst + "')= 1  " +
" left join clients_mst w  on cp.ref_Ext_acc=w.vch_num and w.type='BCD'  and  find_in_set(w.client_unit_id,'" + unitid_mst + "')= 1 where cp.vch_num||to_char(cp.vch_date, 'yyyymmdd')|| cp.type='" + URL + "' ";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                #region 
                                TempData[MyGuid + "_Tlist3"] = model[0].TList3 = mod3 = cmd_Fun.partytype(userCode, unitid_mst);   //party type

                                mod4.Add(new SelectListItem { Text = "Domestic", Value = "001" });   //party location - fix
                                mod4.Add(new SelectListItem { Text = "Overseas", Value = "002" });
                                TempData[MyGuid + "_Tlist4"] = model[0].TList4 = mod4;

                                TempData[MyGuid + "_Tlist5"] = model[0].TList5 = mod5 = cmd_Fun.acctypevdm(userCode, unitid_mst);   //acc type

                                TempData[MyGuid + "_Tlist6"] = model[0].TList6 = mod6 = cmd_Fun.curname(userCode, unitid_mst);   //currency type

                                TempData[MyGuid + "_Tlist7"] = model[0].TList7 = mod7 = cmd_Fun.bank(userCode, unitid_mst);   //bank name

                                TempData[MyGuid + "_TList9"] = model[0].TList9 = mod9 = cmd_Fun.protype(userCode, unitid_mst);   //bindpro_type

                                TempData[MyGuid + "_TList10"] = model[0].TList10 = mod10 = cmd_Fun.occtype(userCode, unitid_mst, out defcall);  //occupation type

                                TempData[MyGuid + "_TList8"] = model[0].TList8 = mod8 = cmd_Fun.salearea(userCode, unitid_mst, out defcall);  //SALES/SERVICE AREA

                                TempData[MyGuid + "_TList11"] = model[0].TList11 = mod11 = cmd_Fun.clienttype(userCode, unitid_mst, out defcall);  //typeofclient

                                TempData[MyGuid + "_TList12"] = model[0].TList12 = mod12 = cmd_Fun.addresstype(userCode, unitid_mst, out defcall);  //typeofaddress
                                TempData[MyGuid + "_TList13"] = model[0].TList13 = mod13 = mod12;

                                TempData[MyGuid + "_TList14"] = model[0].TList14 = mod14 = cmd_Fun.leadsource(userCode, unitid_mst, out defcall);   // Lead source

                                TempData[MyGuid + "_TList15"] = model[0].TList15 = mod15 = cmd_Fun.businesstype(userCode, unitid_mst, out defcall);   //Business type

                                TempData[MyGuid + "_TList16"] = model[0].TList16 = mod16 = cmd_Fun.product(userCode, unitid_mst, out defcall);  //Producttype

                                TempData[MyGuid + "_TList17"] = model[0].TList17 = mod17 = cmd_Fun.payterm(userCode, unitid_mst, out defcall);  //Payment term

                                TempData[MyGuid + "_TList30"] = model[0].TList30 = mod30 = mod17;  //secondary Payment term

                                TempData[MyGuid + "_TList25"] = model[0].TList25 = mod25 = cmd_Fun.username(userCode);  //Refered By User
                                                                                                                        //  model[0].SelectedItems25 = new string[] { "" };

                                TempData[MyGuid + "_TList26"] = model[0].TList26 = mod26 = cmd_Fun.compunit(userCode, clientid_mst);  //comp_unit
                                                                                                                                      //  model[0].SelectedItems26 = new string[] { "" };

                                TempData[MyGuid + "_TList27"] = model[0].TList27 = mod27 = cmd_Fun.Modeofpayment(userCode, unitid_mst);  //Payment Mode

                                mod31 = mod27;
                                TempData[MyGuid + "_TList31"] = model[0].TList31 = mod31;  //secondary Payment Mode

                                TempData[MyGuid + "_TList28"] = model[0].TList28 = mod28 = cmd_Fun.cheqprint(userCode, unitid_mst);    //Cheque Print Location
                                                                                                                                       // model[0].SelectedItems28 = new string[] { "" };

                                mod29.Add(new SelectListItem { Text = "Low", Value = "001" });    //Risk Category
                                mod29.Add(new SelectListItem { Text = "Medium", Value = "002" });
                                mod29.Add(new SelectListItem { Text = "High", Value = "003" });

                                TempData[MyGuid + "_TList29"] = model[0].TList29 = mod29;

                                TempData[MyGuid + "_TList33"] = model[0].TList33 = mod33 = mod12;  //typeofaddress

                                mod34.Add(new SelectListItem { Text = "Company", Value = "Company" });
                                mod34.Add(new SelectListItem { Text = "Not a Company", Value = "Not a Company" });
                                TempData[MyGuid + "_TList34"] = model[0].TList34 = mod34;  //Deductee Type
                                TempData[MyGuid + "_TList35"] = model[0].TList35 = mod35 = cmd_Fun.country(userCode, unitid_mst);  //country(FOR NRI)
                                #endregion
                                #region DDL

                                String[] L3 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["ptype"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems3 = L3;
                                String[] L4 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["ploc"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems4 = L4;
                                String[] L5 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["acctype"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems5 = L5;
                                String[] L6 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["curtype"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems6 = L6;
                                String[] L7 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["bnk"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems7 = L7;
                                String[] L8 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["salesarea"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems8 = L8;
                                String[] L9 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["PR_TYPE"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems9 = L9;
                                String[] L10 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["occ_type"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems10 = L10;
                                String[] L11 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["client_type"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems11 = L11;
                                String[] L12 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["addtype1"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems12 = L12;
                                String[] L13 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["addtype2"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems13 = L13;
                                String[] L14 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["leadsrc"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems14 = L14;
                                String[] L15 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["bsn_type"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems15 = L15;
                                #region existing product for account
                                string ex_product = sgen.getstring(userCode, "select ex_product from ( select group_concat(distinct ld_product) as ex_product from lead_master" +
                                    " where client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "' and type = 'LED' and " +
                                    "cust_id = '" + dtt.Rows[0]["vch_num"].ToString() + "' and lead_con = 'Y' union " +
                                    "select group_concat(distinct prd_type) as ex_product from clients_mst where " +
                                    "client_unit_id='" + unitid_mst + "' and type = 'BCD' and vch_num = '" + dtt.Rows[0]["vch_num"].ToString() + "')a where ex_product != '0' ");
                                if (tm.Col14 == "40002.1")
                                {
                                    String[] L16 = System.String.Join(",", ex_product).Split(',');
                                    model[0].SelectedItems16 = L16;
                                }
                                else
                                {
                                    String[] L16 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["prd_type"].ToString()).Distinct()).Split(',');
                                    model[0].SelectedItems16 = L16;
                                }
                                #endregion
                                String[] L17 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["pay_term"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems17 = L17;
                                String[] L18 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["rel_mgr"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems18 = L18;
                                String[] L19 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["sub_broker"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems19 = L19;
                                String[] L20 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["client_rating"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems20 = L20;
                                String[] L21 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["qualification"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems21 = L21;
                                String[] L22 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["dp"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems22 = L22;
                                String[] L23 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["ann_inc"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems23 = L23;
                                String[] L24 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["credit_rating"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems24 = L24;
                                String[] L25 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["ref_user"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems25 = L25;
                                String[] L26 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["interunit"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems26 = L26;
                                String[] L27 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["pay_mode"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems27 = L27;
                                String[] L28 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["cheq_print"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems28 = L28;
                                String[] L29 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["risk_ctg"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems29 = L29;
                                String[] L30 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["pay_term2"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems30 = L30;
                                String[] L31 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["pay_mode2"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems31 = L31;


                                #endregion
                                if (sgen.GetSession(MyGuid, "tmp_client") != null)
                                {
                                    DataTable tmp = (DataTable)sgen.GetSession(MyGuid, "tmp_client");
                                    if (tmp.Rows.Count > 0)
                                    {
                                        if ((tmp.Select("param3='RELATION MANAGER'")[0]["param2"].ToString() == "Y") || (tmp.Select("param3='SUB BROKER'")[0]["param2"].ToString() == "Y") || (tmp.Select("param3='CLIENT RATING'")[0]["param2"].ToString() == "Y") || (tmp.Select("param3='CREDIT RATING'")[0]["param2"].ToString() == "Y") || (tmp.Select("param3='QUALIFICATION'")[0]["param2"].ToString() == "Y") || (tmp.Select("param3='DP'")[0]["param2"].ToString() == "Y") || (tmp.Select("param3='ANNUAL INCOME'")[0]["param2"].ToString() == "Y"))
                                        {
                                            #region  Relation Manger
                                            TempData[MyGuid + "_TList18"] = model[0].TList18 = mod18 = cmd_Fun.username(userCode);
                                            //   model[0].SelectedItems18 = new string[] { "" };
                                            #endregion
                                            #region  sub broker
                                            string defval = "";
                                            TempData[MyGuid + "_TList19"] = model[0].TList19 = mod19 = cmd_Fun.subbroker(userCode, unitid_mst, out defval);
                                            //  model[0].SelectedItems19 = new string[] { defval };
                                            #endregion
                                            #region  Client rating
                                            defval = "";
                                            TempData[MyGuid + "_TList20"] = model[0].TList20 = mod20 = cmd_Fun.clientrating(userCode, unitid_mst, out defval);
                                            //   model[0].SelectedItems20 = new string[] { defval };
                                            #endregion
                                            #region  Qualification
                                            TempData[MyGuid + "_TList21"] = model[0].TList21 = mod21 = cmd_Fun.qualification(userCode, unitid_mst, out defval);
                                            //   model[0].SelectedItems21 = new string[] { "" };
                                            #endregion
                                            #region  DP
                                            mod22.Add(new SelectListItem { Text = "NSDL", Value = "NSDL" });
                                            mod22.Add(new SelectListItem { Text = "CDSL", Value = "CDSL" });
                                            //  model[0].SelectedItems22 = new string[] { "" };
                                            TempData[MyGuid + "_TList22"] = model[0].TList22 = mod22;
                                            #endregion
                                            #region  annual income
                                            TempData[MyGuid + "_TList23"] = model[0].TList23 = mod23 = cmd_Fun.annincome(userCode, unitid_mst);
                                            //  model[0].SelectedItems23 = new string[] { "" };
                                            #endregion
                                            #region  Credit Rating
                                            TempData[MyGuid + "_TList24"] = model[0].TList24 = mod24 = cmd_Fun.creditrating(userCode, unitid_mst, out defval);
                                            // model[0].SelectedItems24 = new string[] { "" };
                                            #endregion
                                        }
                                        else
                                        {
                                            TempData[MyGuid + "_TList18"] = model[0].TList18 = mod18;
                                            //  model[0].SelectedItems18 = new string[] { "" };
                                            TempData[MyGuid + "_TList19"] = model[0].TList19 = mod19;
                                            //   model[0].SelectedItems19 = new string[] { "" };
                                            TempData[MyGuid + "_TList20"] = model[0].TList20 = mod20;
                                            //  model[0].SelectedItems20 = new string[] { "" };
                                            TempData[MyGuid + "_TList21"] = model[0].TList21 = mod21;
                                            //  model[0].SelectedItems21 = new string[] { "" };
                                            TempData[MyGuid + "_TList22"] = model[0].TList22 = mod22;
                                            //   model[0].SelectedItems22 = new string[] { "" };
                                            TempData[MyGuid + "_TList23"] = model[0].TList23 = mod23;
                                            //  model[0].SelectedItems23 = new string[] { "" };
                                            TempData[MyGuid + "_TList24"] = model[0].TList24 = mod24;
                                            //   model[0].SelectedItems24 = new string[] { "" };
                                        }
                                    }
                                }
                                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                                model[0].Col7 = dtt.Rows[0]["rec_id"].ToString();
                                model[0].Col8 = "(vch_num||to_char(vch_date, 'yyyymmdd')|| type) = '" + URL + "'";
                                model[0].Col85 = "(ref_code||to_char(vch_date, 'yyyymmdd')|| type) = '" + dtt.Rows[0]["ct_fstr"].ToString() + "'";
                                model[0].Col9 = tm.Col9;
                                model[0].Col10 = tm.Col10;
                                model[0].Col11 = tm.Col11;
                                model[0].Col12 = tm.Col12;
                                model[0].Col13 = "Update";
                                model[0].Col100 = "Update & New";
                                model[0].Col121 = "U";
                                model[0].Col122 = "<u>U</u>pdate";
                                model[0].Col123 = "Update & Ne<u>w</u>";
                                model[0].Col14 = tm.Col14;
                                model[0].Col15 = tm.Col15;
                                model[0].Col18 = dtt.Rows[0]["c_name"].ToString();
                                model[0].Col22 = dtt.Rows[0]["addr"].ToString();
                                model[0].Col23 = dtt.Rows[0]["pincode"].ToString();
                                model[0].Col24 = dtt.Rows[0]["c_gstin"].ToString();
                                if (dtt.Rows[0]["isgstr"].ToString() == "Y") { model[0].Chk1 = true; }
                                model[0].Col25 = dtt.Rows[0]["type_desc"].ToString();
                                model[0].Col32 = dtt.Rows[0]["lsrno"].ToString();
                                model[0].Col33 = dtt.Rows[0]["bnkaddr"].ToString();
                                model[0].Col34 = dtt.Rows[0]["micrno"].ToString();
                                model[0].Col35 = dtt.Rows[0]["tor"].ToString();
                                model[0].Col36 = dtt.Rows[0]["panno"].ToString();
                                model[0].Col37 = dtt.Rows[0]["msmeno"].ToString();
                                model[0].Col38 = dtt.Rows[0]["tanno"].ToString();
                                model[0].Col44 = dtt.Rows[0]["status"].ToString() == "Y" ? "Active" : "Inactive";
                                model[0].Col45 = tm.Col45;//chkacct
                                model[0].Col50 = dtt.Rows[0]["swftcd"].ToString();
                                model[0].Col51 = dtt.Rows[0]["acctno"].ToString();
                                model[0].Col52 = dtt.Rows[0]["ifsc"].ToString();
                                model[0].Col88 = dtt.Rows[0]["website"].ToString();
                                model[0].Col49 = dtt.Rows[0]["country_id"].ToString();
                                model[0].Col59 = dtt.Rows[0]["country_name"].ToString();
                                model[0].Col71 = dtt.Rows[0]["state_id"].ToString();
                                model[0].Col60 = dtt.Rows[0]["state_name"].ToString();
                                model[0].Col61 = dtt.Rows[0]["city_name"].ToString();
                                model[0].Col64 = dtt.Rows[0]["city"].ToString();
                                model[0].Col105 = dtt.Rows[0]["std_code"].ToString();
                                model[0].Col72 = dtt.Rows[0]["country_id2"].ToString();
                                model[0].Col69 = dtt.Rows[0]["country_name2"].ToString();
                                model[0].Col73 = dtt.Rows[0]["state_id2"].ToString();
                                model[0].Col68 = dtt.Rows[0]["state_name2"].ToString();
                                model[0].Col67 = dtt.Rows[0]["city_name2"].ToString();
                                model[0].Col106 = dtt.Rows[0]["std_code"].ToString();
                                //model[0].Col68 = dtt.Rows[0]["state_name2"].ToString();
                                //model[0].Col69 = dtt.Rows[0]["country_name2"].ToString();
                                model[0].Col70 = dtt.Rows[0]["cpaddr2"].ToString();
                                model[0].Col48 = dtt.Rows[0]["city2"].ToString();//village id billing
                                model[0].Col76 = dtt.Rows[0]["latlong"].ToString();
                                model[0].Col77 = dtt.Rows[0]["google_add"].ToString();
                                model[0].Col54 = dtt.Rows[0]["remark"].ToString();
                                model[0].Col55 = dtt.Rows[0]["refered_by"].ToString();
                                model[0].Col56 = dtt.Rows[0]["file_no"].ToString();
                                model[0].Col75 = dtt.Rows[0]["MT_DT"].ToString();
                                model[0].Col79 = dtt.Rows[0]["pincode_2"].ToString();
                                model[0].Col80 = dtt.Rows[0]["sch_cat"].ToString();
                                model[0].Col81 = dtt.Rows[0]["sch_med"].ToString();
                                model[0].Col82 = dtt.Rows[0]["no_std"].ToString();
                                model[0].Col83 = dtt.Rows[0]["no_teach"].ToString();
                                model[0].Col84 = dtt.Rows[0]["Aff_type"].ToString();
                                model[0].Col92 = dtt.Rows[0]["CIN_Number"].ToString();
                                model[0].Col93 = (dtt.Rows[0]["comp_email"].ToString() == "0") ? "" : dtt.Rows[0]["comp_email"].ToString();
                                model[0].Col94 = dtt.Rows[0]["parent_name"].ToString();
                                model[0].Col95 = dtt.Rows[0]["parent_id"].ToString();
                                model[0].Col97 = dtt.Rows[0]["cfrm"].ToString();
                                model[0].Col98 = dtt.Rows[0]["fix_credit"].ToString();
                                model[0].Col99 = dtt.Rows[0]["temp_credit"].ToString();
                                model[0].Col101 = dtt.Rows[0]["valid_credit"].ToString();
                                model[0].Col107 = dtt.Rows[0]["pref_add"].ToString();
                                model[0].Col108 = dtt.Rows[0]["alias_name"].ToString();
                                model[0].Col109 = dtt.Rows[0]["aadhar_no"].ToString();
                                model[0].Col110 = dtt.Rows[0]["dp_name"].ToString();
                                model[0].Col111 = dtt.Rows[0]["dp_id"].ToString();
                                model[0].Col112 = dtt.Rows[0]["ben_acc"].ToString();
                                model[0].Col113 = dtt.Rows[0]["uin_no"].ToString();
                                model[0].Col114 = dtt.Rows[0]["min_no"].ToString();
                                model[0].Col115 = dtt.Rows[0]["tin_no"].ToString();
                                model[0].Col116 = dtt.Rows[0]["employer"].ToString();
                                model[0].Col137 = dtt.Rows[0]["ref_name"].ToString();
                                model[0].Col138 = dtt.Rows[0]["ref_Ext_acc"].ToString();
                                model[0].Col139 = dtt.Rows[0]["infavour"].ToString();
                                model[0].Col140 = sgen.server_datetime_local(userCode);
                                model[0].Col141 = sgen.getstring(userCode, "select  a.first_name||' '||Replace(a.MIDDLE_NAME,'0','')||' '||Replace(a.last_name,'0','') from user_details a where a.vch_num ='" + userid_mst + "'");
                                model[0].Col144 = dtt.Rows[0]["credit_days"].ToString();
                                model[0].Col145 = dtt.Rows[0]["d_limit"].ToString();
                                model[0].Col146 = dtt.Rows[0]["s_limit"].ToString();
                                model[0].Col147 = dtt.Rows[0]["org_mobile"].ToString();
                                model[0].Col148 = dtt.Rows[0]["org_contact"].ToString();
                                model[0].Col149 = dtt.Rows[0]["taxable"].ToString();
                                model[0].Col150 = dtt.Rows[0]["ghsub"].ToString();
                                if ((actionName == "party") && (btnval == "PROSPECT"))
                                {
                                    model[0].Col95 = "0";
                                    model[0].Col13 = "Save";
                                    model[0].Col100 = "Save & New";
                                    model[0].Col66 = dtt.Rows[0]["vch_num"].ToString();
                                    model[0].Col65 = dtt.Rows[0]["c_name"].ToString();
                                    model[0].Col13 = "Save";
                                    model[0].Col12 = "BCD";
                                    //shiv
                                    //mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" +
                                    //    "" + model[0].Col11.Trim() + "  and substr(vch_num,0,2)=" + prefix + " ";
                                    //model[0].Col17 = sgen.genNo(userCode, mq, 6, "auto_genid");
                                    model[0].Col121 = "S";
                                    model[0].Col122 = "<u>S</u>ave";
                                    model[0].Col123 = "Save & Ne<u>w</u>";
                                    sgen.SetSession(MyGuid, "EDMODE", "N");
                                }
                                else { model[0].Col17 = dtt.Rows[0]["vch_num"].ToString(); }
                                model[0].LTM2 = new List<Tmodelmain>();
                                mod32.Add(new SelectListItem { Text = "Male", Value = "Male" });
                                mod32.Add(new SelectListItem { Text = "Female", Value = "Female" });
                                for (int i = 0; i < dtt.Rows.Count; i++)
                                {
                                    Tmodelmain ltm2 = new Tmodelmain();

                                    TempData[MyGuid + "_Tlist32"] = ltm2.TList32 = mod32;


                                    if (i == 0)
                                    {
                                        ltm2.Col26 = dtt.Rows[0]["cpname"].ToString();
                                        ltm2.Col27 = dtt.Rows[0]["cpcont"].ToString() == "0" ? "" : dtt.Rows[0]["cpcont"].ToString();
                                        ltm2.Col28 = dtt.Rows[0]["cpaltcont"].ToString() == "0" ? "" : dtt.Rows[0]["cpaltcont"].ToString();
                                        ltm2.Col29 = dtt.Rows[0]["cpemail"].ToString() == "0" ? "" : dtt.Rows[0]["cpemail"].ToString();
                                        ltm2.Col30 = dtt.Rows[0]["cpaddr"].ToString();
                                        ltm2.Col20 = dtt.Rows[0]["dob"].ToString();
                                        ltm2.Col21 = dtt.Rows[0]["doa"].ToString();
                                        ltm2.Col53 = dtt.Rows[0]["cplandno"].ToString() == "0" ? "" : dtt.Rows[0]["cplandno"].ToString();
                                        ltm2.Col78 = dtt.Rows[0]["cpdept"].ToString();
                                        ltm2.Col31 = dtt.Rows[0]["cpdesig"].ToString();
                                        ltm2.Col86 = dtt.Rows[0]["ct_rec_id"].ToString();
                                        ltm2.Col89 = dtt.Rows[0]["cp_mname"].ToString();
                                        ltm2.Col90 = dtt.Rows[0]["cp_Lname"].ToString();
                                        ltm2.Col126 = dtt.Rows[0]["cp_alias_name"].ToString();
                                        ltm2.Col132 = dtt.Rows[0]["ct_vch_num"].ToString();
                                        ltm2.Col110 = dtt.Rows[0]["cpcontother"].ToString();
                                        ltm2.Col111 = dtt.Rows[0]["cpemailother"].ToString();
                                        if (dtt.Rows[0]["iscp_sms"].ToString().Trim() == "Y") ltm2.Chk1 = true;
                                        if (dtt.Rows[0]["Iscp_email"].ToString().Trim() == "Y") ltm2.Chk2 = true;
                                        ltm2.SelectedItems32 = new string[] { dtt.Rows[0]["cp_gender"].ToString() };

                                    }
                                    else
                                    {
                                        ltm2.Col26 = dtt.Rows[i]["c_cpname"].ToString();
                                        ltm2.Col27 = dtt.Rows[i]["c_cpcont"].ToString() == "0" ? "" : dtt.Rows[0]["c_cpcont"].ToString(); ;
                                        ltm2.Col28 = dtt.Rows[i]["c_cpaltcont"].ToString() == "0" ? "" : dtt.Rows[0]["c_cpaltcont"].ToString(); ;
                                        ltm2.Col29 = dtt.Rows[i]["c_cpemail"].ToString() == "0" ? "" : dtt.Rows[0]["c_cpemail"].ToString();
                                        ltm2.Col30 = dtt.Rows[i]["c_cpaddr"].ToString();
                                        ltm2.Col20 = dtt.Rows[i]["c_dob"].ToString();
                                        ltm2.Col21 = dtt.Rows[i]["c_doa"].ToString();
                                        ltm2.Col53 = dtt.Rows[i]["c_cplandno"].ToString() == "0" ? "" : dtt.Rows[0]["c_cplandno"].ToString(); ;
                                        ltm2.Col78 = dtt.Rows[i]["c_cpdept"].ToString();
                                        ltm2.Col31 = dtt.Rows[i]["cpdesig"].ToString();
                                        ltm2.Col86 = dtt.Rows[i]["ct_rec_id"].ToString();
                                        ltm2.Col89 = dtt.Rows[i]["ct_cp_mname"].ToString();
                                        ltm2.Col90 = dtt.Rows[i]["ct_cp_Lname"].ToString();
                                        ltm2.Col126 = dtt.Rows[i]["cp_alias_name"].ToString();
                                        ltm2.Col132 = dtt.Rows[i]["ct_vch_num"].ToString();
                                        ltm2.Col110 = dtt.Rows[i]["cpcontother"].ToString();
                                        ltm2.Col111 = dtt.Rows[i]["cpemailother"].ToString();
                                        if (dtt.Rows[i]["iscp_sms"].ToString().Trim() == "Y") ltm2.Chk1 = true;
                                        if (dtt.Rows[i]["Iscp_email"].ToString().Trim() == "Y") ltm2.Chk2 = true;
                                        ltm2.SelectedItems32 = new string[] { dtt.Rows[i]["cp_gender"].ToString() };
                                    }
                                    model[0].LTM2.Add(ltm2);
                                }
                                if (dtt.Rows[0]["contr"].ToString().Trim() == "Y") model[0].Chk2 = true;
                                if (dtt.Rows[0]["sez"].ToString() == "Y") { model[0].Chk3 = true; }
                                if (dtt.Rows[0]["BILL_ADD"].ToString() == "Y") { model[0].Chk4 = true; }
                                if (dtt.Rows[0]["pubr"].ToString().Trim() == "Y") model[0].Chk5 = true;
                                if (dtt.Rows[0]["mftr"].ToString().Trim() == "Y") model[0].Chk6 = true;
                                if (dtt.Rows[0]["trans"].ToString().Trim() == "Y") model[0].Chk7 = true;
                                if (dtt.Rows[0]["Isorg_email"].ToString().Trim() == "Y") model[0].Chk8 = true;
                                if (dtt.Rows[0]["ISorg_sms"].ToString().Trim() == "Y") model[0].Chk9 = true;
                                if (dtt.Rows[0]["s_citizen"].ToString().Trim() == "Y") model[0].Chk10 = true;
                                DataTable dtf = new DataTable();
                                dtf = sgen.getdata(userCode, "select rec_id,file_name,file_path,col1,col2 from file_tab where client_unit_id='" + unitid_mst + "' and ref_code1='" + dtt.Rows[0]["vch_num"].ToString() + "' " +
                                    "and type in ('" + tm.Col12 + "','DD" + model[0].Col12 + "')");
                                List<Tmodelmain> ltmf = new List<Tmodelmain>();
                                foreach (DataRow drf in dtf.Rows)
                                {
                                    Tmodelmain tmf = new Tmodelmain();
                                    tmf.Col4 = drf["file_path"].ToString();
                                    tmf.Col1 = drf["file_name"].ToString();
                                    tmf.Col2 = drf["col1"].ToString();
                                    tmf.Col3 = drf["rec_id"].ToString();
                                    tmf.Col5 = drf["col2"].ToString();
                                    ltmf.Add(tmf);
                                }
                                #region Remark
                                string remark = "", rmk = "";
                                DataTable dtr = sgen.getdata(userCode, "select vch_num, to_char(convert_tz(vch_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "')  as vch_date,col48 from kc_tab where client_unit_id='" + unitid_mst + "' and type='RMK' and" +
                                    " col1='" + model[0].Col17 + "' order by vch_num desc");
                                if (dtr.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dtr.Rows)
                                    {
                                        if (!remark.Contains(dr["vch_num"].ToString()))
                                        {
                                            rmk = Environment.NewLine + "Doc No : " + dr["vch_num"] + "" + "  Date : " + dr["vch_date"] + "" + Environment.NewLine;
                                        }
                                        remark = remark + rmk + "          remark : " + " " + dr["col48"] + " " + Environment.NewLine + "";
                                    }
                                    model[0].Col143 = remark;
                                }
                                #endregion
                                model[0].LTM1 = ltmf;

                                #region manual country,state,city
                                if (dtt.Rows[0]["country_mn"].ToString() != "")
                                {
                                    dt = new DataTable();
                                    dt = sgen.getdata(userCode, "select distinct country_name,state_name from country_state where alpha_2='" + dtt.Rows[0]["country_mn"].ToString() + "' and state_gst_code='" + dtt.Rows[0]["state_mn"].ToString() + "'");

                                    if (dt.Rows.Count > 0)
                                    {
                                        model[0].Col59 = dt.Rows[0]["country_name"].ToString();
                                        model[0].Col60 = dt.Rows[0]["state_name"].ToString();
                                    }
                                    model[0].Col49 = dtt.Rows[0]["country_mn"].ToString();
                                    model[0].Col71 = dtt.Rows[0]["state_mn"].ToString();
                                    model[0].Col61 = dtt.Rows[0]["city_mn"].ToString();


                                }


                                #endregion

                                model[0].SelectedItems33 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["addtype3"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems34 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["d_type"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems35 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["nri_country"].ToString()).Distinct()).Split(',');
                                model[0].Col176 = dtt.Rows[0]["country3"].ToString();
                                model[0].Col175 = dtt.Rows[0]["state3"].ToString();
                                model[0].Col174 = dtt.Rows[0]["city3"].ToString();
                                model[0].Col154 = dtt.Rows[0]["cpaddr3"].ToString();
                                model[0].Col155 = dtt.Rows[0]["pincode_3"].ToString();
                                model[0].Col157 = dtt.Rows[0]["d_name"].ToString();
                                model[0].Col158 = dtt.Rows[0]["d_pan"].ToString();
                                model[0].Col159 = dtt.Rows[0]["nri_tax"].ToString();
                                model[0].Col170 = dtt.Rows[0]["d_add"].ToString();
                                model[0].Col160 = dtt.Rows[0]["d_address"].ToString();
                                model[0].Col161 = dtt.Rows[0]["d_pincode"].ToString();
                                model[0].Col173 = dtt.Rows[0]["d_city"].ToString();
                                model[0].Col172 = dtt.Rows[0]["d_state"].ToString();
                                model[0].Col171 = dtt.Rows[0]["d_country"].ToString();
                                model[0].Col165 = dtt.Rows[0]["tds_d"].ToString();
                                model[0].Col166 = dtt.Rows[0]["tds_rate"].ToString();
                                model[0].Col167 = dtt.Rows[0]["tds_limit"].ToString();
                                model[0].Col168 = dtt.Rows[0]["d_cert"].ToString();
                                model[0].Col169 = dtt.Rows[0]["d_valid"].ToString();
                                model[0].Col151 = dtt.Rows[0]["country_name3"].ToString();
                                model[0].Col152 = dtt.Rows[0]["state_name3"].ToString();
                                model[0].Col153 = dtt.Rows[0]["city_name3"].ToString();
                                model[0].Col156 = dtt.Rows[0]["std_code3"].ToString();
                                model[0].Col164 = dtt.Rows[0]["country_named"].ToString();
                                model[0].Col163 = dtt.Rows[0]["state_named"].ToString();
                                model[0].Col162 = dtt.Rows[0]["city_named"].ToString();

                                model[0].Chk11 = dtt.Rows[0]["ship_ADD"].ToString() == "Y" ? true : false;
                                model[0].Chk12 = dtt.Rows[0]["istds"].ToString() == "Y" ? true : false;
                                model[0].Chk13 = dtt.Rows[0]["isnri"].ToString() == "Y" ? true : false;
                                model[0].Chk14 = dtt.Rows[0]["d_lower"].ToString() == "Y" ? true : false;
                                model[0].Chk15 = dtt.Rows[0]["trcom"].ToString() == "Y" ? true : false;

                                if (model[0].SelectedItems13.FirstOrDefault() == "") { model[0].SelectedItems13 = model[0].SelectedItems12; }
                                if ((model[0].Col72 == "") || (model[0].Col72 == "-") || (model[0].Col72 == "0")) { model[0].Col72 = model[0].Col49; }

                                if (model[0].Col69 == "" || model[0].Col69 == "-" || model[0].Col69 == "0") { model[0].Col69 = model[0].Col59; }
                                if (model[0].Col73 == "" || model[0].Col73 == "-" || model[0].Col73 == "0") { model[0].Col73 = model[0].Col71; }
                                if (model[0].Col68 == "" || model[0].Col68 == "-" || model[0].Col68 == "0") { model[0].Col68 = model[0].Col60; }
                                if (model[0].Col48 == "" || model[0].Col48 == "-" || model[0].Col48 == "0") { model[0].Col48 = model[0].Col64; }
                                if (model[0].Col67 == "" || model[0].Col67 == "-" || model[0].Col67 == "0") { model[0].Col67 = model[0].Col61; }


                                if (model[0].SelectedItems33.FirstOrDefault() == "") { model[0].SelectedItems33 = model[0].SelectedItems12; }
                                if (model[0].Col176 == "" || model[0].Col176 == "-" || model[0].Col176 == "0") { model[0].Col176 = model[0].Col49; }
                                if (model[0].Col151 == "" || model[0].Col151 == "-" || model[0].Col151 == "0") { model[0].Col151 = model[0].Col59; }
                                if (model[0].Col175 == "" || model[0].Col175 == "-" || model[0].Col175 == "0") { model[0].Col175 = model[0].Col71; }
                                if (model[0].Col152 == "" || model[0].Col152 == "-" || model[0].Col152 == "0") { model[0].Col152 = model[0].Col60; }
                                if (model[0].Col174 == "" || model[0].Col174 == "-" || model[0].Col174 == "0") { model[0].Col174 = model[0].Col64; }
                                if (model[0].Col153 == "" || model[0].Col153 == "-" || model[0].Col153 == "0") { model[0].Col153 = model[0].Col61; }
                            }
                            break;
                        case "FDEL":
                            string fid = sgen.GetSession(MyGuid, "delid").ToString();
                            res = sgen.execute_cmd(userCode, "Delete from file_tab WHERE client_unit_id='" + unitid_mst + "'and rec_id='" + fid + "'");
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
                        case "LSR":
                            mq = "Select Financial_Category_Code from (SELECT client_id||client_unit_id||vch_num||TO_CHAR(vch_date,'YYYYMMDD')|| type as fstr,master_type as Financial_Category_Code , master_name AS Financial_Category,'-' Main_Head,'-' as Sub_Head,'-' as Sub_Sub_Head FROM master_setting where type = 'AGM' AND TRIM(CLIENT_ID)= '-' AND TRIM(CLIENT_UNIT_ID)= '-' union   SELECT  (a.client_id||a.client_unit_id|| a.vch_num|| TO_CHAR(a.vch_date, 'YYYYMMDD')|| a.type) as fstr,a.master_type as Financial_Category_Code ,b.master_name as Financial_Category,a.master_name as Main_Head,'-' as Sub_Head,'-' as Sub_Sub_Head FROM master_setting a INNER JOIN master_setting b ON SUBSTR(a.master_type, 1, 1) = SUBSTR(b.master_type, 1, 1)  AND TO_NUMBER(SUBSTR(b.master_type,2,7))= 0   and b.type = 'AGM'  where TO_NUMBER(SUBSTR(a.master_type,2,3))> 0 AND TO_NUMBER(SUBSTR(a.master_type,4,7))= 0 and a.type = 'AGM' and a.client_unit_id='" + unitid_mst + "' union SELECT (a.client_id|| a.client_unit_id|| a.vch_num|| TO_CHAR(a.vch_date, 'YYYYMMDD')|| a.type) as fstr,a.master_type as Financial_Category_Code ,b.master_name as Financial_Category ,c.master_name as Main_Head,a.master_name as Sub_Head,'-' as Sub_Sub_Head  FROM master_setting a INNER JOIN master_setting b ON SUBSTR(a.master_type, 1, 1)= SUBSTR(b.master_type, 1, 1)  AND TO_NUMBER(SUBSTR(b.master_type,2,7))= 0   and b.type = 'AGM'  INNER JOIN master_setting c ON SUBSTR(a.master_type,1,3)= SUBSTR(c.master_type, 1, 3)  AND TO_NUMBER(SUBSTR(c.master_type,4,7))= 0   and c.type = 'AGM' where TO_NUMBER(SUBSTR(a.master_type,4,5))> 0 AND TO_NUMBER(SUBSTR(a.master_type,6,7))= 0 and a.type = 'AGM' and a.client_id='" + clientid_mst + "' and a.client_unit_id='" + unitid_mst + "'  and a.client_unit_id = c.client_unit_id union SELECT (a.client_id|| a.client_unit_id|| a.vch_num|| TO_CHAR(a.vch_date, 'YYYYMMDD')|| a.type) as fstr,a.master_type as Financial_Category_Code ,b.master_name AS Financial_Category,c.master_name as Main_Head,d.master_name as Sub_Head,a.master_name as Sub_Sub_Head  FROM master_setting a INNER JOIN master_setting b ON SUBSTR(a.master_type, 1, 1) = SUBSTR(b.master_type, 1, 1)  AND TO_NUMBER(SUBSTR(b.master_type,2,7))= 0   and b.type = 'AGM' INNER JOIN master_setting c ON SUBSTR(a.master_type,1,3)= SUBSTR(c.master_type, 1, 3)  AND TO_NUMBER(SUBSTR(c.master_type,4,7))= 0 and c.type = 'AGM'  INNER JOIN master_setting d ON SUBSTR(a.master_type,1,5)= SUBSTR(d.master_type, 1, 5) AND TO_NUMBER(SUBSTR(d.master_type,6,7))= 0   and d.type = 'AGM'  where TO_NUMBER(SUBSTR(a.master_type,6,7))> 0  and a.type = 'AGM' and a.client_id='" + clientid_mst + "' and a.client_unit_id='" + unitid_mst + "' and a.client_unit_id= c.client_unit_id and a.client_unit_id=d.client_unit_id) where fstr = '" + URL + "'";
                            // model[0].Col32 = sgen.getstring(userCode, "select ledger_code from ledger_master a  where a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type ='" + URL + "'");
                            model[0].Col32 = sgen.getstring(userCode, mq);
                            break;
                        case "ADD1":
                            // mq = "select * from country_state where client_id||client_unit_id||vch_num|| to_char(vch_date, 'yyyymmdd')||type='" + URL + "'";
                            mq = "select * from country_state where alpha_2||state_gst_code ||city_name='" + URL + "'";
                            DataTable dt1 = new DataTable();
                            dt1 = sgen.getdata(userCode, mq);
                            if (dt1.Rows.Count > 0)
                            {
                                model[0].Col59 = dt1.Rows[0]["country_name"].ToString();
                                model[0].Col60 = dt1.Rows[0]["state_name"].ToString();
                                model[0].Col61 = dt1.Rows[0]["city_name"].ToString();
                                //model[0].Col26 = dt1.Rows[0]["city_name"].ToString();
                                //model[0].Col62 = dt1.Rows[0]["teh_name"].ToString();
                                //model[0].Col63 = dt1.Rows[0]["v_name"].ToString();
                                model[0].Col64 = dt1.Rows[0]["rec_id"].ToString();
                                model[0].Col49 = dt1.Rows[0]["alpha_2"].ToString();
                                model[0].Col71 = dt1.Rows[0]["state_gst_code"].ToString();
                                model[0].Col105 = dt1.Rows[0]["std_code"].ToString();
                                if (model[0].Chk4 == true)
                                {
                                    //model[0].Col65 = dt1.Rows[0]["v_name"].ToString();
                                    //model[0].Col66 = dt1.Rows[0]["teh_name"].ToString();
                                    model[0].Col67 = dt1.Rows[0]["city_name"].ToString();
                                    model[0].Col68 = dt1.Rows[0]["state_name"].ToString();
                                    model[0].Col69 = dt1.Rows[0]["country_name"].ToString();
                                    model[0].Col48 = dt1.Rows[0]["rec_id"].ToString();
                                    model[0].Col72 = dt1.Rows[0]["alpha_2"].ToString();
                                    model[0].Col73 = dt1.Rows[0]["state_gst_code"].ToString();
                                    model[0].Col106 = dt1.Rows[0]["std_code"].ToString();
                                }
                            }
                            break;
                        case "ADD2":

                            mq = "select * from country_state where alpha_2||state_gst_code ||city_name='" + URL + "'";
                            dt1 = new DataTable();
                            dt1 = sgen.getdata(userCode, mq);
                            if (dt1.Rows.Count > 0)
                            {
                                //model[0].Col65 = dt1.Rows[0]["v_name"].ToString();
                                //model[0].Col66 = dt1.Rows[0]["teh_name"].ToString();
                                model[0].Col67 = dt1.Rows[0]["city_name"].ToString();
                                model[0].Col68 = dt1.Rows[0]["state_name"].ToString();
                                model[0].Col69 = dt1.Rows[0]["country_name"].ToString();
                                model[0].Col48 = dt1.Rows[0]["rec_id"].ToString();
                                model[0].Col72 = dt1.Rows[0]["alpha_2"].ToString();
                                model[0].Col73 = dt1.Rows[0]["state_gst_code"].ToString();
                                model[0].Col106 = dt1.Rows[0]["std_code"].ToString();
                            }
                            break;

                        case "ADD3":

                            mq = "select * from country_state where alpha_2||state_gst_code ||city_name='" + URL + "'";
                            dt1 = new DataTable();
                            dt1 = sgen.getdata(userCode, mq);
                            if (dt1.Rows.Count > 0)
                            {

                                model[0].Col153 = dt1.Rows[0]["city_name"].ToString();
                                model[0].Col152 = dt1.Rows[0]["state_name"].ToString();
                                model[0].Col151 = dt1.Rows[0]["country_name"].ToString();
                                model[0].Col174 = dt1.Rows[0]["rec_id"].ToString();
                                model[0].Col176 = dt1.Rows[0]["alpha_2"].ToString();
                                model[0].Col175 = dt1.Rows[0]["state_gst_code"].ToString();
                                model[0].Col156 = dt1.Rows[0]["std_code"].ToString();
                            }
                            break;
                        case "PRINT":
                            mq = "select sp.master_name as service_area, (select file_path from file_tab where client_unit_id='" + unitid_mst + "' and type='BCD' and col1='Client' and ref_code1=cp.vch_num and rownum<2) as cl_file_path ," +
                                "(select file_path from file_tab where type='BCD' and client_unit_id='" + unitid_mst + "'" +
                                " and col1='Property' and ref_code1=cp.vch_num and rownum<2) as pr_file_path,oc.master_name as occ_name, " +
                                "cp.PR_TYPE ,cp.refered_by, cp.file_no,cp.occ_type,cp.panno,cp.BILL_ADD,cp.rec_id,cp.c_name as cust_name, " +
                                "to_char(cp.dob, '" + sgen.Getsqldateformat() + "') as dob," +
                                "to_char(cp.doa, '" + sgen.Getsqldateformat() + "') as doa," +
                                "to_char(cp.MT_DT, '" + sgen.Getsqldatetimeformat() + "') as MT_DT," +
                                "cp.cpemail as email,cs.alpha_2 as country_id,cs.country_name,cs.state_gst_code as state_id ,cs.state_name," +
                                "cs.district_name,cs.city_name,cs.teh_name,cs.v_name,cp.city" + ",ba.alpha_2 as country_id2,ba.country_name as " +
                                "country_name2, ba.state_gst_code as state_id2 ,ba.state_name as state_name2 ,ba.district_name as district_name2," +
                                "ba.city_name as city_name, ba.teh_name as teh_name2,ba.v_name as v_name2 ,cp.city2,cp.cpaddr2,cp.c_gstin as" +
                                "cust_gstin_no,cp.CPLANDNO landline,cp.cpcont mob,cp.cpaltcont altmob,cp.cpaddr,ms.master_name as prop_type," +
                                "cp.remark,cp.client_id,cp.client_unit_id,cp.vch_num,cp.ent_by,cp.ent_date,cp.latlong,cp.google_add," +
                                "cp.client_type from clients_mst cp  inner join country_state cs on cs.rec_id = cp.city  " +
                                " left join country_state ba  on ba.rec_id = cp.city2   left join master_setting ms on ms.master_id = cp.PR_TYPE and ms.type='PRT' " +
                                " and ms.client_unit_id='" + unitid_mst + "' left join master_setting oc " +
                                "on cp.occ_type= oc.master_id and cp.client_unit_id=oc.client_unit_id and oc.type='OCC' left join master_setting sp " +
                                "on cp.occ_type= sp.master_id and cp.client_unit_id=sp.client_unit_id and sp.type='SSA' where cp.client_id||cp.client_unit_id||" +
                                " cp.vch_num||to_char(cp.vch_date, 'yyyymmdd')|| cp.type='" + URL + "' ";
                            DataTable dtm = new DataTable();
                            dtm = sgen.getdata(userCode, mq);
                            if (dtm.Rows.Count > 0)
                            {
                                dtm.Columns.Add("clientimage", typeof(Byte[]));
                                dtm.Columns.Add("propimage", typeof(Byte[]));
                                foreach (DataRow dr in dtm.Rows)
                                {
                                    FileStream fs;
                                    BinaryReader br;
                                    string cl_file_path = Server.MapPath("~/Uploads/") + cg_com_name.Replace(" ", "") + "/" + dr["cl_file_path"].ToString();
                                    string pr_file_path = Server.MapPath("~/Uploads/") + cg_com_name.Replace(" ", "") + "/" + dr["pr_file_path"].ToString();
                                    try
                                    {
                                        fs = new FileStream(cl_file_path, FileMode.Open);
                                        br = new BinaryReader(fs);
                                        byte[] Image = new byte[fs.Length + 1];
                                        Image = br.ReadBytes(Convert.ToInt32((fs.Length)));
                                        dr["clientimage"] = Image;
                                        br.Close();
                                        fs.Close();
                                    }
                                    catch
                                    {
                                        cl_file_path = Server.MapPath("~/Uploads/") + "person-icon.png";
                                        fs = new FileStream(cl_file_path, FileMode.Open);
                                        br = new BinaryReader(fs);
                                        byte[] Image = new byte[fs.Length + 1];
                                        Image = br.ReadBytes(Convert.ToInt32((fs.Length)));
                                        dr["clientimage"] = Image;
                                        br.Close();
                                        fs.Close();
                                    }
                                    try
                                    {
                                        fs = new FileStream(pr_file_path, FileMode.Open);
                                        br = new BinaryReader(fs);
                                        byte[] Image = new byte[fs.Length + 1];
                                        Image = br.ReadBytes(Convert.ToInt32((fs.Length)));
                                        dr["propimage"] = Image;
                                        br.Close();
                                        fs.Close();
                                    }
                                    catch
                                    {
                                        pr_file_path = Server.MapPath("~/Uploads/") + "person-icon.png";
                                        fs = new FileStream(pr_file_path, FileMode.Open);
                                        br = new BinaryReader(fs);
                                        byte[] Image = new byte[fs.Length + 1];
                                        Image = br.ReadBytes(Convert.ToInt32((fs.Length)));
                                        dr["propimage"] = Image;
                                        br.Close();
                                        fs.Close();
                                    }
                                }
                                dtm.AcceptChanges();
                                sgen.open_report_bydt(userCode, dtm, "new_client", "New Client Detail");
                                ViewBag.scripCall += "showRptnew('New Client Detail');disableForm();";

                                //ViewBag.scripCall = "OpenSingle('../../../../../erp/schoolReports_Rpts/Filter_Report_xml.aspx','95%','900px','Reports');";
                            }
                            break;
                        case "EXT":
                            mq = @"select cp.cp_mname,cp.cp_lname,ct.cp_mname as ct_cp_mname,ct.cp_lname as ct_cp_lname, cp.leadsrc, cp.website,cp.addtype1,cp.addtype2, cp.salesarea, ct.rec_id as ct_rec_id, ct.client_id || ct.client_unit_id || ct.vch_num || to_char(ct.vch_date, 'yyyymmdd') || ct.type as ct_fstr, ct.cpname as c_cpname,ct.cpcont as c_cpcont,ct.cpaltcont as c_cpaltcont,ct.cpemail as c_cpemail,ct.cpaddr as c_cpaddr,ct.cpdesig as c_cpdesig,ct.cplandno as c_cplandno,ct.cpdept as c_cpdept,ct.DOB as c_DOB,ct.doa as c_doa, cp.pincode_2,cp.sch_cat,cp.sch_med,cp.no_std,cp.no_teach,cp.Aff_type,
                              cp.cpdept, cp.cpname, cp.type_desc, cp.isgstr, cp.c_gstin, cp.micrno, cp.tor, cp.status, cp.pincode, cp.addr, cp.ifsc,
                                   cp.acctno, cp.contr, cp.swftcd, cp.bnk, cp.curtype, cp.acctype,  cp.ploc, cp.ptype, cp.tanno, 
                              cp.msmeno, cp.micrno,cp.bnkaddr, cp.cpdesig,cp.lsrno, cp.sez, cp.PR_TYPE ,cp.refered_by,
                            cp.file_no,cp.occ_type,cp.panno,cp.BILL_ADD,cp.rec_id,cp.c_name ,
                          to_char(cp.dob,  '" + sgen.Getsqldateformat() + "') as dob," +
                         "to_char(cp.doa,  '" + sgen.Getsqldateformat() + "') as doa," +
                         "to_char(cp.MT_DT, '" + sgen.Getsqldatetimeformat() + "') as MT_DT," +
                        "cp.cpemail ,cs.alpha_2 as country_id,cs.country_name,cs.state_gst_code as state_id ,cs.state_name," +
                       "cs.district_name,cs.city_name,cs.teh_name,cs.v_name,cp.city" +
                           ",ba.alpha_2 as country_id2,ba.country_name as country_name2, ba.state_gst_code as state_id2 ," +
                             "ba.state_name as state_name2 ,ba.district_name as district_name2,ba.city_name as city_name2," +
                             "ba.teh_name as teh_name2,ba.v_name as v_name2 ,cp.city2,cp.cpaddr2," +
                              "cp.c_gstin as cust_gstin_no,cp.CPLANDNO ,cp.cpcont ,cp.cpaltcont ,cp.cpaddr,ms.master_name as prop_type,cp.remark,cp.client_id," +
"cp.client_unit_id,cp.vch_num,cp.ent_by,cp.ent_date,cp.latlong,cp.google_add,cp.client_type from clients_mst cp left join clients_mst ct" +
" on cp.vch_num=ct.vch_num and cp.client_unit_id=ct.client_unit_id and ct.type='CON' " +
"left join country_state cs on cs.rec_id = cp.city " +
"left join country_state ba on ba.rec_id = cp.city2 " +
"left join master_setting ms on ms.master_id = cp.PR_TYPE and ms.type='PRT' and ms.client_unit_id='" + unitid_mst + "' " +
"where (cp.vch_num||to_char(cp.vch_date, 'yyyymmdd')|| cp.type) in ('" + URL + "') ";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "PVDDATA", URL.Trim());
                                Make_query(viewName.ToLower(), "Select Unit", "UNIT", "2", "", "", MyGuid);
                                ViewBag.scripCall += "callFoo('Select Unit');";
                            }
                            break;
                        case "UNIT":
                            try
                            {
                                string currdate = sgen.server_datetime(userCode);
                                string[] uts = URL.Replace("','", ",").Split(',');
                                string[] mst_data = sgen.GetSession(MyGuid, "PVDDATA").ToString().Replace("','", ",").Split(',');
                                #region dtstr                                
                                try
                                {
                                    foreach (string mst in mst_data)
                                    {
                                        List<string> cp = new List<string>();
                                        List<string> up = new List<string>();
                                        foreach (string ut in uts)
                                        {
                                            cp.Add(ut.Substring(6, 3).Trim());
                                            up.Add(ut.Substring(0, 6).Trim());
                                        }
                                        cp.Distinct();
                                        up.Distinct();
                                        mq = "update clients_mst set edit_by='" + userid_mst + "',edit_date=TO_DATE('" + currdate + "', 'YYYY-MM-DD HH24:MI:SS')," +
                                            "client_id='" + String.Join(",", cp) + "',client_unit_id='" + String.Join(",", up) + "' " +
                                            "where (vch_num||to_char(vch_date, 'yyyymmdd')||type)='" + mst + "'";
                                        res = sgen.execute_cmd(userCode, mq);
                                    }
                                }
                                catch (Exception ex) { res = false; }
                                #endregion
                                if (res)
                                {
                                    ViewBag.vnew = "";
                                    ViewBag.vedit = "";
                                    ViewBag.vsave = "disabled='disabled'";
                                    ViewBag.vsavenew = "disabled='disabled'";
                                    ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Extended Successfully');disableForm(); ";
                                }
                                else { ViewBag.scripCall = "showmsgJS(1, 'Record Not Extended', 0);"; }
                            }
                            catch (Exception ex) { sgen.showmsg(1, ex.Message.ToString(), 0); }
                            break;
                    }
                    break;
                #endregion
                #region party_unit_detail
                //                case "party_unit":
                //                    switch (btnval.ToUpper())
                //                    {
                //                        case "EDIT":
                //                        case "40002.7":
                //                            mq = @"select cp.cp_mname,cp.cp_lname,ct.cp_mname as ct_cp_mname,ct.cp_lname as ct_cp_lname,cp.occ_type,cp.panno, b.c_name as clientName, cp.website,cp.addtype1,cp.addtype2, cp.salesarea, ct.rec_id as ct_rec_id, ct.client_id||ct.client_unit_id||ct.vch_num||to_char(ct.vch_date, 'yyyymmdd')|| ct.type as ct_fstr, ct.cpname as c_cpname,ct.cpcont as c_cpcont,ct.cpaltcont as c_cpaltcont,ct.cpemail as c_cpemail,ct.cpaddr as c_cpaddr,ct.cpdesig as c_cpdesig,ct.cplandno as c_cplandno,ct.cpdept as c_cpdept,ct.DOB as c_DOB,ct.doa as c_doa, cp.pincode_2,cp.sch_cat,cp.sch_med,cp.no_std,cp.no_teach,cp.Aff_type,
                //                              cp.cpdept, cp.cpname, cp.type_desc, cp.isgstr, cp.c_gstin, cp.micrno, cp.tor, cp.status, cp.pincode, cp.addr, cp.ifsc,
                //                                   cp.acctno, cp.contr, cp.swftcd, cp.bnk, cp.curtype, cp.acctype,  cp.ploc, cp.ptype, cp.tanno, 
                //                              cp.msmeno, cp.micrno,cp.bnkaddr, cp.cpdesig,cp.lsrno, cp.sez, cp.PR_TYPE ,cp.refered_by,
                //                            cp.file_no,cp.BILL_ADD,cp.rec_id,cp.c_name ,
                //                         to_char(convert_tz(cp.dob, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as dob," +
                //                         "to_char(convert_tz(cp.doa, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as doa," +
                //                        "to_char(convert_tz(cp.MT_DT, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldatetimeformat() + "') as MT_DT," +
                //                        "cp.cpemail ,cs.alpha_2 as country_id,cs.country_name,cs.state_gst_code as state_id ,cs.state_name," +
                //                       "cs.district_name,cs.city_name,cs.teh_name,cs.v_name,cp.city" +
                //                           ",ba.alpha_2 as country_id2,ba.country_name as country_name2, ba.state_gst_code as state_id2 ," +
                //                             "ba.state_name as state_name2 ,ba.district_name as district_name2,ba.city_name as city_name2," +
                //                             "ba.teh_name as teh_name2,ba.v_name as v_name2 ,cp.city2,cp.cpaddr2," +
                //                              "cp.c_gstin as cust_gstin_no,cp.CPLANDNO ,cp.cpcont ,cp.cpaltcont ,cp.cpaddr,ms.master_name as prop_type,cp.remark,cp.client_id," +
                //"cp.client_unit_id,cp.vch_num,cp.ent_by,cp.ent_date,cp.latlong,cp.google_add,cp.client_type from clients_mst cp left join clients_mst ct" +
                //" on cp.vch_num=ct.vch_num and cp.client_unit_id=ct.client_unit_id and ct.type='UNC' " +
                //"left join country_state cs on cs.rec_id = cp.city " +
                //"left join country_state ba on ba.rec_id = cp.city2 " +
                //"left join master_setting ms on ms.master_id = cp.PR_TYPE and ms.type='PRT' and ms.client_unit_id='" + unitid_mst + "' inner join clients_mst b on cp.panno=b.vch_num and b.type= cp.occ_type" +
                //                                          " and cp.client_unit_id=b.client_unit_id " +
                //"where cp.client_id||cp.client_unit_id|| cp.vch_num||to_char(cp.vch_date, 'yyyymmdd')|| cp.type='" + URL + "' ";
                //                            dtt = sgen.getdata(userCode, mq);
                //                            if (dtt.Rows.Count > 0)
                //                            {
                //                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                //                                #region party type
                //                                TempData[MyGuid + "_Tlist3"] = model[0].TList3 = mod3 = cmd_Fun.partytype(userCode, unitid_mst);
                //                                #endregion
                //                                #region party location - fix
                //                                mod4.Add(new SelectListItem { Text = "Domestic", Value = "001" });
                //                                mod4.Add(new SelectListItem { Text = "Overseas", Value = "002" });
                //                                TempData[MyGuid + "_Tlist4"] = model[0].TList4 = mod4;
                //                                #endregion
                //                                #region acc type
                //                                mq = "select master_id,master_name from master_setting where type='BTM'" + model[0].Col11 + "";
                //                                dt = sgen.getdata(userCode, mq);
                //                                if (dt.Rows.Count > 0)
                //                                {
                //                                    foreach (DataRow dr in dt.Rows)
                //                                    {
                //                                        mod5.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                //                                    }
                //                                }
                //                                TempData[MyGuid + "_Tlist5"] = mod5;
                //                                #endregion
                //                                #region currency type
                //                                mod6 = cmd_Fun.curname(userCode, unitid_mst);
                //                                TempData[MyGuid + "_Tlist6"] = mod6;
                //                                #endregion
                //                                #region bank name
                //                                TempData[MyGuid + "_Tlist7"] = model[0].TList7 = mod7 = cmd_Fun.bank(userCode, unitid_mst);
                //                                #endregion
                //                                #region bindpro_type
                //                                TempData[MyGuid + "_TList9"] = model[0].TList9 = mod9 = cmd_Fun.protype(userCode, unitid_mst);
                //                                #endregion
                //                                #region occupation type
                //                                TempData[MyGuid + "_TList10"] = model[0].TList10 = mod10 = cmd_Fun.occtype(userCode, unitid_mst);
                //                                #endregion
                //                                #region  SALES/SERVICE AREA
                //                                mod8 = cmd_Fun.salearea(userCode, unitid_mst);
                //                                TempData[MyGuid + "_TList8"] = mod8;
                //                                #endregion
                //                                #region  typeofclient
                //                                mod11 = cmd_Fun.clienttype(userCode, unitid_mst);
                //                                TempData[MyGuid + "_TList11"] = mod11;
                //                                #endregion
                //                                #region  typeofaddress
                //                                mod12 = cmd_Fun.addresstype(userCode, unitid_mst);
                //                                mod13 = cmd_Fun.addresstype(userCode, unitid_mst);
                //                                TempData[MyGuid + "_TList12"] = mod12;
                //                                TempData[MyGuid + "_TList13"] = mod13;
                //                                #endregion
                //                                model[0].TList1 = mod1;
                //                                model[0].TList2 = mod2;

                //                                model[0].TList5 = mod5;
                //                                model[0].TList6 = mod6;
                //                                model[0].TList8 = mod8;
                //                                model[0].TList11 = mod11;
                //                                model[0].TList12 = mod12;
                //                                model[0].TList13 = mod13;
                //                                String[] L3 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["ptype"].ToString()).Distinct()).Split(',');
                //                                model[0].SelectedItems3 = L3;
                //                                String[] L4 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["ploc"].ToString()).Distinct()).Split(',');
                //                                model[0].SelectedItems4 = L4;
                //                                String[] L5 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["acctype"].ToString()).Distinct()).Split(',');
                //                                model[0].SelectedItems5 = L5;
                //                                String[] L6 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["curtype"].ToString()).Distinct()).Split(',');
                //                                model[0].SelectedItems6 = L6;
                //                                String[] L7 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["bnk"].ToString()).Distinct()).Split(',');
                //                                model[0].SelectedItems7 = L7;
                //                                String[] L8 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["salesarea"].ToString()).Distinct()).Split(',');
                //                                model[0].SelectedItems8 = L8;
                //                                String[] L9 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["PR_TYPE"].ToString()).Distinct()).Split(',');
                //                                model[0].SelectedItems9 = L9;
                //                                //String[] L10 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["occ_type"].ToString()).Distinct()).Split(',');
                //                                //model[0].SelectedItems10 = L10;
                //                                String[] L11 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["client_type"].ToString()).Distinct()).Split(',');
                //                                model[0].SelectedItems11 = L11;
                //                                String[] L12 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["addtype1"].ToString()).Distinct()).Split(',');
                //                                model[0].SelectedItems12 = L12;
                //                                String[] L13 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["addtype2"].ToString()).Distinct()).Split(',');
                //                                model[0].SelectedItems12 = L13;
                //                                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                //                                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                //                                model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                //                                model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                //                                model[0].Col7 = dtt.Rows[0]["rec_id"].ToString();
                //                                model[0].Col8 = "(client_id||client_unit_id||vch_num||to_char(vch_date, 'yyyymmdd')|| type) = '" + URL + "'";
                //                                model[0].Col85 = "(client_id||client_unit_id||vch_num||to_char(vch_date, 'yyyymmdd')|| type) = '" + dtt.Rows[0]["ct_fstr"].ToString() + "'";
                //                                model[0].Col9 = tm.Col9;
                //                                model[0].Col10 = tm.Col10;
                //                                model[0].Col11 = tm.Col11;
                //                                model[0].Col12 = tm.Col12;
                //                                model[0].Col13 = "Update";
                //                                model[0].Col100 = "Update & New";
                //                                model[0].Col14 = tm.Col14;
                //                                model[0].Col15 = tm.Col15;
                //                                model[0].Col16 = dtt.Rows[0]["vch_num"].ToString();
                //                                model[0].Col17 = dtt.Rows[0]["panno"].ToString();
                //                                model[0].Col22 = dtt.Rows[0]["addr"].ToString();
                //                                model[0].Col23 = dtt.Rows[0]["pincode"].ToString();
                //                                model[0].Col24 = dtt.Rows[0]["c_gstin"].ToString();
                //                                if (dtt.Rows[0]["isgstr"].ToString() == "Y") { model[0].Chk1 = true; }
                //                                model[0].Col25 = dtt.Rows[0]["type_desc"].ToString();
                //                                model[0].Col33 = dtt.Rows[0]["bnkaddr"].ToString();
                //                                model[0].Col34 = dtt.Rows[0]["micrno"].ToString();
                //                                model[0].Col35 = dtt.Rows[0]["tor"].ToString();
                //                                //model[0].Col36 = dtt.Rows[0]["panno"].ToString();
                //                                model[0].Col37 = dtt.Rows[0]["msmeno"].ToString();
                //                                model[0].Col38 = dtt.Rows[0]["tanno"].ToString();
                //                                model[0].Col44 = dtt.Rows[0]["status"].ToString() == "Y" ? "Active" : "Inactive";
                //                                model[0].Col45 = tm.Col45;//chkacct
                //                                model[0].Col50 = dtt.Rows[0]["swftcd"].ToString();
                //                                model[0].Col51 = dtt.Rows[0]["acctno"].ToString();
                //                                model[0].Col52 = dtt.Rows[0]["ifsc"].ToString();
                //                                model[0].Col88 = dtt.Rows[0]["clientName"].ToString();
                //                                model[0].Col89 = dtt.Rows[0]["website"].ToString();
                //                                model[0].Col18 = dtt.Rows[0]["c_name"].ToString();
                //                                model[0].Col49 = dtt.Rows[0]["country_id"].ToString();
                //                                model[0].Col59 = dtt.Rows[0]["country_name"].ToString();
                //                                model[0].Col71 = dtt.Rows[0]["state_id"].ToString();
                //                                model[0].Col60 = dtt.Rows[0]["state_name"].ToString();
                //                                //model[0].Col63 = dtt.Rows[0]["v_name"].ToString();
                //                                //model[0].Col62 = dtt.Rows[0]["teh_name"].ToString();
                //                                model[0].Col61 = dtt.Rows[0]["city_name"].ToString();
                //                                model[0].Col72 = dtt.Rows[0]["country_id2"].ToString();
                //                                model[0].Col69 = dtt.Rows[0]["country_name2"].ToString();
                //                                model[0].Col73 = dtt.Rows[0]["state_id2"].ToString();
                //                                model[0].Col68 = dtt.Rows[0]["state_name2"].ToString();
                //                                //model[0].Col65 = dtt.Rows[0]["v_name2"].ToString();
                //                                //model[0].Col66 = dtt.Rows[0]["teh_name2"].ToString();
                //                                model[0].Col67 = dtt.Rows[0]["city_name2"].ToString();
                //                                model[0].Col68 = dtt.Rows[0]["state_name2"].ToString();
                //                                model[0].Col69 = dtt.Rows[0]["country_name2"].ToString();
                //                                model[0].Col70 = dtt.Rows[0]["cpaddr2"].ToString();
                //                                model[0].Col48 = dtt.Rows[0]["city2"].ToString();//village id billing
                //                                model[0].Col76 = dtt.Rows[0]["latlong"].ToString();
                //                                model[0].Col77 = dtt.Rows[0]["google_add"].ToString();
                //                                model[0].Col54 = dtt.Rows[0]["remark"].ToString();
                //                                model[0].Col55 = dtt.Rows[0]["refered_by"].ToString();
                //                                model[0].Col56 = dtt.Rows[0]["file_no"].ToString();
                //                                model[0].Col75 = dtt.Rows[0]["MT_DT"].ToString();
                //                                //model[0].Col20 = dtt.Rows[0]["dob"].ToString();
                //                                //model[0].Col21 = dtt.Rows[0]["doa"].ToString();
                //                                //model[0].Col53 = dtt.Rows[0]["cplandno"].ToString();
                //                                //model[0].Col78 = dtt.Rows[0]["cpdept"].ToString();
                //                                model[0].Col79 = dtt.Rows[0]["pincode_2"].ToString();
                //                                model[0].Col80 = dtt.Rows[0]["sch_cat"].ToString();
                //                                model[0].Col81 = dtt.Rows[0]["sch_med"].ToString();
                //                                model[0].Col82 = dtt.Rows[0]["no_std"].ToString();
                //                                model[0].Col83 = dtt.Rows[0]["no_teach"].ToString();
                //                                model[0].Col84 = dtt.Rows[0]["Aff_type"].ToString();
                //                                model[0].LTM2 = new List<Tmodelmain>();
                //                                for (int i = 0; i < dtt.Rows.Count; i++)
                //                                {
                //                                    Tmodelmain ltm2 = new Tmodelmain();
                //                                    //ltm.Col38 = dtt.Rows[i]["property_name"].ToString();
                //                                    //ltm.SelectedItems2 = new string[] { dtt.Rows[i]["prop_type"].ToString() };
                //                                    //ltm.Col39 = dtt.Rows[i]["remark"].ToString();
                //                                    //ltm.Col47 = dtt.Rows[i]["Col4"].ToString();
                //                                    //ltm.TList2 = mod2;
                //                                    //model[0].LTM2.Add(ltm);
                //                                    if (i == 0)
                //                                    {
                //                                        ltm2.Col26 = dtt.Rows[0]["cpname"].ToString();
                //                                        ltm2.Col27 = dtt.Rows[0]["cpcont"].ToString();
                //                                        ltm2.Col28 = dtt.Rows[0]["cpaltcont"].ToString();
                //                                        ltm2.Col29 = dtt.Rows[0]["cpemail"].ToString();
                //                                        ltm2.Col30 = dtt.Rows[0]["cpaddr"].ToString();
                //                                        ltm2.Col20 = dtt.Rows[0]["dob"].ToString();
                //                                        ltm2.Col21 = dtt.Rows[0]["doa"].ToString();
                //                                        ltm2.Col53 = dtt.Rows[0]["cplandno"].ToString();
                //                                        ltm2.Col78 = dtt.Rows[0]["cpdept"].ToString();
                //                                        ltm2.Col86 = dtt.Rows[0]["ct_rec_id"].ToString();
                //                                        ltm2.Col90 = dtt.Rows[0]["cp_mname"].ToString();
                //                                        ltm2.Col91 = dtt.Rows[0]["cp_Lname"].ToString();
                //                                    }
                //                                    else
                //                                    {
                //                                        ltm2.Col26 = dtt.Rows[i]["c_cpname"].ToString();
                //                                        ltm2.Col27 = dtt.Rows[i]["c_cpcont"].ToString();
                //                                        ltm2.Col28 = dtt.Rows[i]["c_cpaltcont"].ToString();
                //                                        ltm2.Col29 = dtt.Rows[i]["c_cpemail"].ToString();
                //                                        ltm2.Col30 = dtt.Rows[i]["c_cpaddr"].ToString();
                //                                        ltm2.Col20 = dtt.Rows[i]["c_dob"].ToString();
                //                                        ltm2.Col21 = dtt.Rows[i]["c_doa"].ToString();
                //                                        ltm2.Col53 = dtt.Rows[i]["c_cplandno"].ToString();
                //                                        ltm2.Col78 = dtt.Rows[i]["c_cpdept"].ToString();
                //                                        ltm2.Col86 = dtt.Rows[0]["ct_rec_id"].ToString();
                //                                        ltm2.Col90 = dtt.Rows[i]["ct_cp_mname"].ToString();
                //                                        ltm2.Col91 = dtt.Rows[i]["ct_cp_Lname"].ToString();
                //                                    }
                //                                    model[0].LTM2.Add(ltm2);
                //                                }
                //                                if (dtt.Rows[0]["contr"].ToString().Trim() == "Y") model[0].Chk2 = true;
                //                                if (dtt.Rows[0]["sez"].ToString() == "Y") { model[0].Chk3 = true; }
                //                                if (dtt.Rows[0]["BILL_ADD"].ToString() == "Y") { model[0].Chk4 = true; }
                //                                DataTable dtf = new DataTable();
                //                                dtf = sgen.getdata(userCode, "select rec_id,file_name,file_path,col1 from file_tab where client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "' and ref_code1='" + dtt.Rows[0]["vch_num"].ToString() + "' and type in " +
                //                                    "('" + tm.Col12 + "','DD" + model[0].Col12 + "')");
                //                                List<Tmodelmain> ltmf = new List<Tmodelmain>();
                //                                foreach (DataRow drf in dtf.Rows)
                //                                {
                //                                    Tmodelmain tmf = new Tmodelmain();
                //                                    tmf.Col4 = drf["file_path"].ToString();
                //                                    tmf.Col1 = drf["file_name"].ToString();
                //                                    tmf.Col2 = drf["col1"].ToString();
                //                                    tmf.Col3 = drf["rec_id"].ToString();
                //                                    ltmf.Add(tmf);
                //                                }
                //                                model[0].LTM1 = ltmf;
                //                            }
                //                            break;
                //                        case "CLIENT":
                //                            mq = @"select cp.addtype1,cp.addtype2, cp.salesarea, ct.rec_id as ct_rec_id, ct.client_id||ct.client_unit_id||ct.vch_num||
                //                                to_char(ct.vch_date, 'yyyymmdd')|| ct.type as ct_fstr, ct.cpname as c_cpname,ct.cpcont as c_cpcont,ct.cpaltcont as 
                //                               c_cpaltcont,ct.cpemail as c_cpemail,ct.cpaddr as c_cpaddr,ct.cpdesig as c_cpdesig,ct.cplandno as c_cplandno,
                //                         ct.cpdept as c_cpdept,ct.DOB as c_DOB,ct.doa as c_doa, cp.pincode_2,cp.sch_cat,cp.sch_med,cp.no_std,cp.no_teach,cp.Aff_type,
                //                              cp.cpdept, cp.cpname, cp.type_desc, cp.isgstr, cp.c_gstin, cp.micrno, cp.tor, cp.status, cp.pincode, cp.addr, cp.ifsc,
                //                                   cp.acctno, cp.contr, cp.swftcd, cp.bnk, cp.curtype, cp.acctype,  cp.ploc, cp.ptype, cp.tanno, 
                //                              cp.msmeno, cp.micrno,cp.bnkaddr, cp.cpdesig,cp.lsrno, cp.sez, cp.PR_TYPE ,cp.refered_by,
                //                            cp.file_no,cp.occ_type,cp.panno,cp.BILL_ADD,cp.rec_id,cp.c_name ,
                //                         to_char(convert_tz(cp.dob, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as dob," +
                //                         "to_char(convert_tz(cp.doa, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as doa," +
                //                        "to_char(convert_tz(cp.MT_DT, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldatetimeformat() + "') as MT_DT," +
                //                        "cp.cpemail ,cs.alpha_2 as country_id,cs.country_name,cs.state_gst_code as state_id ,cs.state_name," +
                //                       "cs.district_name,cs.city_name,cs.teh_name,cs.v_name,cp.city" +
                //                           ",ba.alpha_2 as country_id2,ba.country_name as country_name2, ba.state_gst_code as state_id2 ," +
                //                             "ba.state_name as state_name2 ,ba.district_name as district_name2,ba.city_name as city_name2," +
                //                             "ba.teh_name as teh_name2,ba.v_name as v_name2 ,cp.city2,cp.cpaddr2," +
                //                              "cp.c_gstin as cust_gstin_no,cp.CPLANDNO ,cp.cpcont ,cp.cpaltcont ,cp.cpaddr,ms.master_name as prop_type,cp.remark,cp.client_id," +
                //"cp.client_unit_id,cp.vch_num,cp.ent_by,cp.ent_date,cp.latlong,cp.google_add,cp.client_type from clients_mst cp left join clients_mst ct" +
                //" on cp.vch_num=ct.vch_num and cp.client_unit_id=ct.client_unit_id and ct.type='CON' " +
                //"left join country_state cs on cs.rec_id = cp.city " +
                //"left join country_state ba on ba.rec_id = cp.city2 " +
                //"left join master_setting ms on ms.master_id = cp.PR_TYPE and ms.type='PRT' and ms.client_unit_id='" + unitid_mst + "' " +
                //"where cp.client_id||cp.client_unit_id|| cp.vch_num||to_char(cp.vch_date, 'yyyymmdd')|| cp.type='" + URL + "' ";
                //                            dtt = new DataTable();
                //                            dtt = sgen.getdata(userCode, mq);
                //                            if (dtt.Rows.Count > 0)
                //                            {
                //                                #region party type
                //                                mod3 = cmd_Fun.partytype(userCode, unitid_mst);
                //                                TempData[MyGuid + "_Tlist3"] = mod3;
                //                                #endregion
                //                                #region party location - fix
                //                                mod4.Add(new SelectListItem { Text = "Domestic", Value = "001" });
                //                                mod4.Add(new SelectListItem { Text = "Overseas", Value = "002" });
                //                                TempData[MyGuid + "_Tlist4"] = mod4;
                //                                #endregion
                //                                #region acc type
                //                                mq = "select master_id,master_name from master_setting where type='BTM'" + model[0].Col11 + "";
                //                                dt = sgen.getdata(userCode, mq);
                //                                if (dt.Rows.Count > 0)
                //                                {
                //                                    foreach (DataRow dr in dt.Rows)
                //                                    {
                //                                        mod5.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                //                                    }
                //                                }
                //                                TempData[MyGuid + "_Tlist5"] = mod5;
                //                                #endregion
                //                                #region currency type
                //                                mod6 = cmd_Fun.curname(userCode, unitid_mst);
                //                                TempData[MyGuid + "_Tlist6"] = mod6;
                //                                #endregion
                //                                #region bank name
                //                                mod7 = cmd_Fun.bank(userCode, unitid_mst);
                //                                TempData[MyGuid + "_Tlist7"] = mod7;
                //                                #endregion
                //                                #region bindpro_type
                //                                mod9 = cmd_Fun.protype(userCode, unitid_mst);
                //                                TempData[MyGuid + "_TList9"] = mod9;
                //                                #endregion
                //                                #region occupation type
                //                                mod10 = cmd_Fun.occtype(userCode, unitid_mst);
                //                                TempData[MyGuid + "_TList10"] = mod10;
                //                                #endregion
                //                                #region  SALES/SERVICE AREA
                //                                mod8 = cmd_Fun.salearea(userCode, unitid_mst);
                //                                TempData[MyGuid + "_TList8"] = mod8;
                //                                #endregion
                //                                #region  typeofclient
                //                                mod11 = cmd_Fun.clienttype(userCode, unitid_mst);
                //                                TempData[MyGuid + "_TList11"] = mod11;
                //                                #endregion
                //                                #region  typeofaddress
                //                                mod12 = cmd_Fun.addresstype(userCode, unitid_mst);
                //                                mod13 = cmd_Fun.addresstype(userCode, unitid_mst);
                //                                TempData[MyGuid + "_TList12"] = mod12;
                //                                TempData[MyGuid + "_TList13"] = mod13;
                //                                #endregion
                //                                model[0].TList1 = mod1;
                //                                model[0].TList2 = mod2;
                //                                model[0].TList3 = mod3;
                //                                model[0].TList4 = mod4;
                //                                model[0].TList5 = mod5;
                //                                model[0].TList6 = mod6;
                //                                model[0].TList7 = mod7;
                //                                model[0].TList8 = mod8;
                //                                model[0].TList9 = mod9;
                //                                model[0].TList10 = mod10;
                //                                model[0].TList11 = mod11;
                //                                model[0].TList12 = mod12;
                //                                model[0].TList13 = mod13;
                //                                String[] L3 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["ptype"].ToString()).Distinct()).Split(',');
                //                                model[0].SelectedItems3 = L3;
                //                                String[] L4 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["ploc"].ToString()).Distinct()).Split(',');
                //                                model[0].SelectedItems4 = L4;
                //                                String[] L5 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["acctype"].ToString()).Distinct()).Split(',');
                //                                model[0].SelectedItems5 = L5;
                //                                String[] L6 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["curtype"].ToString()).Distinct()).Split(',');
                //                                model[0].SelectedItems6 = L6;
                //                                String[] L7 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["bnk"].ToString()).Distinct()).Split(',');
                //                                model[0].SelectedItems7 = L7;
                //                                String[] L8 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["salesarea"].ToString()).Distinct()).Split(',');
                //                                model[0].SelectedItems8 = L8;
                //                                String[] L9 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["PR_TYPE"].ToString()).Distinct()).Split(',');
                //                                model[0].SelectedItems9 = L9;
                //                                String[] L10 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["occ_type"].ToString()).Distinct()).Split(',');
                //                                model[0].SelectedItems10 = L10;
                //                                String[] L11 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["client_type"].ToString()).Distinct()).Split(',');
                //                                model[0].SelectedItems11 = L11;
                //                                String[] L12 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["addtype1"].ToString()).Distinct()).Split(',');
                //                                model[0].SelectedItems12 = L12;
                //                                String[] L13 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["addtype2"].ToString()).Distinct()).Split(',');
                //                                model[0].SelectedItems12 = L13;
                //                                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                //                                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                //                                model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                //                                model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                //                                model[0].Col7 = dtt.Rows[0]["rec_id"].ToString();
                //                                //model[0].Col8 = "(client_id||client_unit_id||vch_num||to_char(vch_date, 'yyyymmdd')|| type) = '" + URL + "'";
                //                                //model[0].Col85 = "(client_id||client_unit_id||vch_num||to_char(vch_date, 'yyyymmdd')|| type) = '" + dtt.Rows[0]["ct_fstr"].ToString() + "'";
                //                                model[0].Col9 = tm.Col9;
                //                                model[0].Col10 = tm.Col10;
                //                                model[0].Col11 = tm.Col11;
                //                                model[0].Col12 = tm.Col12;
                //                                model[0].Col13 = "Save";
                //                                model[0].Col100 = "Save & New";
                //                                model[0].Col14 = tm.Col14;
                //                                model[0].Col15 = tm.Col15;
                //                                model[0].Col17 = dtt.Rows[0]["vch_num"].ToString();
                //                                model[0].Col88 = dtt.Rows[0]["c_name"].ToString();
                //                                model[0].Col22 = dtt.Rows[0]["addr"].ToString();
                //                                model[0].Col23 = dtt.Rows[0]["pincode"].ToString();
                //                                model[0].Col24 = dtt.Rows[0]["c_gstin"].ToString();
                //                                if (dtt.Rows[0]["isgstr"].ToString() == "Y") { model[0].Chk1 = true; }
                //                                model[0].Col25 = dtt.Rows[0]["type_desc"].ToString();
                //                                model[0].Col33 = dtt.Rows[0]["bnkaddr"].ToString();
                //                                model[0].Col34 = dtt.Rows[0]["micrno"].ToString();
                //                                model[0].Col35 = dtt.Rows[0]["tor"].ToString();
                //                                model[0].Col36 = dtt.Rows[0]["panno"].ToString();
                //                                model[0].Col37 = dtt.Rows[0]["msmeno"].ToString();
                //                                model[0].Col38 = dtt.Rows[0]["tanno"].ToString();
                //                                model[0].Col44 = dtt.Rows[0]["status"].ToString() == "Y" ? "Active" : "Inactive";
                //                                model[0].Col45 = tm.Col45;//chkacct
                //                                model[0].Col50 = dtt.Rows[0]["swftcd"].ToString();
                //                                model[0].Col51 = dtt.Rows[0]["acctno"].ToString();
                //                                model[0].Col52 = dtt.Rows[0]["ifsc"].ToString();
                //                                model[0].Col49 = dtt.Rows[0]["country_id"].ToString();
                //                                model[0].Col59 = dtt.Rows[0]["country_name"].ToString();
                //                                model[0].Col71 = dtt.Rows[0]["state_id"].ToString();
                //                                model[0].Col60 = dtt.Rows[0]["state_name"].ToString();
                //                                model[0].Col61 = dtt.Rows[0]["city_name"].ToString();
                //                                model[0].Col72 = dtt.Rows[0]["country_id2"].ToString();
                //                                model[0].Col69 = dtt.Rows[0]["country_name2"].ToString();
                //                                model[0].Col73 = dtt.Rows[0]["state_id2"].ToString();
                //                                model[0].Col68 = dtt.Rows[0]["state_name2"].ToString();
                //                                model[0].Col67 = dtt.Rows[0]["city_name2"].ToString();
                //                                model[0].Col68 = dtt.Rows[0]["state_name2"].ToString();
                //                                model[0].Col69 = dtt.Rows[0]["country_name2"].ToString();
                //                                model[0].Col70 = dtt.Rows[0]["cpaddr2"].ToString();
                //                                model[0].Col48 = dtt.Rows[0]["city2"].ToString();//village id billing
                //                                model[0].Col76 = dtt.Rows[0]["latlong"].ToString();
                //                                model[0].Col77 = dtt.Rows[0]["google_add"].ToString();
                //                                model[0].Col54 = dtt.Rows[0]["remark"].ToString();
                //                                model[0].Col55 = dtt.Rows[0]["refered_by"].ToString();
                //                                model[0].Col56 = dtt.Rows[0]["file_no"].ToString();
                //                                model[0].Col75 = dtt.Rows[0]["MT_DT"].ToString();
                //                                model[0].Col79 = dtt.Rows[0]["pincode_2"].ToString();
                //                                model[0].Col80 = dtt.Rows[0]["sch_cat"].ToString();
                //                                model[0].Col81 = dtt.Rows[0]["sch_med"].ToString();
                //                                model[0].Col82 = dtt.Rows[0]["no_std"].ToString();
                //                                model[0].Col83 = dtt.Rows[0]["no_teach"].ToString();
                //                                model[0].Col84 = dtt.Rows[0]["Aff_type"].ToString();
                //                                model[0].LTM2 = new List<Tmodelmain>();
                //                                for (int i = 0; i < dtt.Rows.Count; i++)
                //                                {
                //                                    Tmodelmain ltm2 = new Tmodelmain();
                //                                    if (i == 0)
                //                                    {
                //                                        ltm2.Col26 = dtt.Rows[0]["cpname"].ToString();
                //                                        ltm2.Col27 = dtt.Rows[0]["cpcont"].ToString();
                //                                        ltm2.Col28 = dtt.Rows[0]["cpaltcont"].ToString();
                //                                        ltm2.Col29 = dtt.Rows[0]["cpemail"].ToString();
                //                                        ltm2.Col30 = dtt.Rows[0]["cpaddr"].ToString();
                //                                        ltm2.Col20 = dtt.Rows[0]["dob"].ToString();
                //                                        ltm2.Col21 = dtt.Rows[0]["doa"].ToString();
                //                                        ltm2.Col53 = dtt.Rows[0]["cplandno"].ToString();
                //                                        ltm2.Col78 = dtt.Rows[0]["cpdept"].ToString();
                //                                        ltm2.Col86 = dtt.Rows[0]["ct_rec_id"].ToString();
                //                                    }
                //                                    else
                //                                    {
                //                                        ltm2.Col26 = dtt.Rows[i]["c_cpname"].ToString();
                //                                        ltm2.Col27 = dtt.Rows[i]["c_cpcont"].ToString();
                //                                        ltm2.Col28 = dtt.Rows[i]["c_cpaltcont"].ToString();
                //                                        ltm2.Col29 = dtt.Rows[i]["c_cpemail"].ToString();
                //                                        ltm2.Col30 = dtt.Rows[i]["c_cpaddr"].ToString();
                //                                        ltm2.Col20 = dtt.Rows[i]["c_dob"].ToString();
                //                                        ltm2.Col21 = dtt.Rows[i]["c_doa"].ToString();
                //                                        ltm2.Col53 = dtt.Rows[i]["c_cplandno"].ToString();
                //                                        ltm2.Col78 = dtt.Rows[i]["c_cpdept"].ToString();
                //                                        ltm2.Col86 = dtt.Rows[0]["ct_rec_id"].ToString();
                //                                    }
                //                                    model[0].LTM2.Add(ltm2);
                //                                }
                //                                if (dtt.Rows[0]["contr"].ToString().Trim() == "Y") model[0].Chk2 = true;
                //                                if (dtt.Rows[0]["sez"].ToString() == "Y") { model[0].Chk3 = true; }
                //                                if (dtt.Rows[0]["BILL_ADD"].ToString() == "Y") { model[0].Chk4 = true; }
                //                                DataTable dtf = new DataTable();
                //                                dtf = sgen.getdata(userCode, "select rec_id,file_name,file_path,col1 from file_tab where client_unit_id='" + unitid_mst + "'" +
                //                                    " and ref_code1='" + dtt.Rows[0]["vch_num"].ToString() + "' and type in ('" + tm.Col12 + "','DD" + model[0].Col12 + "')");
                //                                List<Tmodelmain> ltmf = new List<Tmodelmain>();
                //                                foreach (DataRow drf in dtf.Rows)
                //                                {
                //                                    Tmodelmain tmf = new Tmodelmain();
                //                                    tmf.Col4 = drf["file_path"].ToString();
                //                                    tmf.Col1 = drf["file_name"].ToString();
                //                                    tmf.Col2 = drf["col1"].ToString();
                //                                    tmf.Col3 = drf["rec_id"].ToString();
                //                                    ltmf.Add(tmf);
                //                                }
                //                                model[0].LTM1 = ltmf;
                //                            }
                //                            break;
                //                        case "FDEL":
                //                            string fid = sgen.GetSession(MyGuid, "delid").ToString();
                //                            res = sgen.execute_cmd(userCode, "Delete from file_tab WHERE client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "' and rec_id='" + fid + "' and type='BCD' and substr(vch_num,0,3)='203'");
                //                            sgen.SetSession(MyGuid, "delid", null);
                //                            if (res)
                //                            {
                //                                var LTM = model[0].LTM1;
                //                                for (int d = 0; d < LTM.Count; d++)
                //                                {
                //                                    var id = LTM[d].Col3;
                //                                    if (id == fid) { model[0].LTM1.RemoveAt(d); }
                //                                }
                //                            }
                //                            break;
                //                        case "LSR":
                //                            mq = "select from file_tab WHERE client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "' and type='BCD' and substr(vch_num,0,3)='203' and";
                //                            dtt = sgen.getdata(userCode, mq);
                //                            if (dtt.Rows.Count > 0)
                //                            {
                //                                model[0].Col32 = dtt.Rows[0]["lsrno"].ToString();
                //                            }
                //                            break;
                //                        case "ADD1":
                //                            // mq = "select * from country_state where client_id||client_unit_id||vch_num|| to_char(vch_date, 'yyyymmdd')||type='" + URL + "'";
                //                            mq = "select * from country_state where alpha_2||state_gst_code ||city_name='" + URL + "'";
                //                            DataTable dt1 = new DataTable();
                //                            dt1 = sgen.getdata(userCode, mq);
                //                            if (dt1.Rows.Count > 0)
                //                            {
                //                                model[0].Col59 = dt1.Rows[0]["country_name"].ToString();
                //                                model[0].Col60 = dt1.Rows[0]["state_name"].ToString();
                //                                model[0].Col61 = dt1.Rows[0]["city_name"].ToString();
                //                                //model[0].Col26 = dt1.Rows[0]["city_name"].ToString();
                //                                //model[0].Col62 = dt1.Rows[0]["teh_name"].ToString();
                //                                //model[0].Col63 = dt1.Rows[0]["v_name"].ToString();
                //                                model[0].Col64 = dt1.Rows[0]["rec_id"].ToString();
                //                                model[0].Col49 = dt1.Rows[0]["alpha_2"].ToString();
                //                                model[0].Col71 = dt1.Rows[0]["state_gst_code"].ToString();
                //                                if (model[0].Chk4 == true)
                //                                {
                //                                    //model[0].Col65 = dt1.Rows[0]["v_name"].ToString();
                //                                    //model[0].Col66 = dt1.Rows[0]["teh_name"].ToString();
                //                                    model[0].Col67 = dt1.Rows[0]["city_name"].ToString();
                //                                    model[0].Col68 = dt1.Rows[0]["state_name"].ToString();
                //                                    model[0].Col69 = dt1.Rows[0]["country_name"].ToString();
                //                                    model[0].Col48 = dt1.Rows[0]["rec_id"].ToString();
                //                                    model[0].Col72 = dt1.Rows[0]["alpha_2"].ToString();
                //                                    model[0].Col73 = dt1.Rows[0]["state_gst_code"].ToString();
                //                                }
                //                            }
                //                            break;
                //                        case "ADD2":
                //                            // mq = "select * from country_state where client_id||client_unit_id||vch_num|| to_char(vch_date, 'yyyymmdd')||type='" + URL + "'";
                //                            mq = "select * from country_state where alpha_2||state_gst_code ||city_name='" + URL + "'";
                //                            dt1 = new DataTable();
                //                            dt1 = sgen.getdata(userCode, mq);
                //                            if (dt1.Rows.Count > 0)
                //                            {
                //                                //model[0].Col65 = dt1.Rows[0]["v_name"].ToString();
                //                                //model[0].Col66 = dt1.Rows[0]["teh_name"].ToString();
                //                                model[0].Col67 = dt1.Rows[0]["city_name"].ToString();
                //                                model[0].Col68 = dt1.Rows[0]["state_name"].ToString();
                //                                model[0].Col69 = dt1.Rows[0]["country_name"].ToString();
                //                                model[0].Col48 = dt1.Rows[0]["rec_id"].ToString();
                //                                model[0].Col72 = dt1.Rows[0]["alpha_2"].ToString();
                //                                model[0].Col73 = dt1.Rows[0]["state_gst_code"].ToString();
                //                            }
                //                            break;
                //                    }
                //                    break;
                //                #endregion
                //               #region cust_enquiry
                //               case "enq":
                //                   switch (btnval.ToUpper())
                //                   {
                //                       case "EDIT":
                //                           mq = "select ba.state_name as state_name2 , d.cpcont,d.cpemail, a.client_id, a.client_unit_id, a.ent_by, a.ent_date, a.rec_id, c.pincode_2, ba.city_name as city_name2," +
                //                               " ba.state_name as state_name, ba.country_name as country_name2, c.cpaddr2, b.c_name as Organisation,c.c_name as Unit_Name," +
                //                               " a.vch_num,a.vch_date,a.type,a.c_name as unit_id,a.panno , a.COUNTRY,a.COUNTRY2,a.PTYPE,a.cpname as" +
                //                               " cont_id,a.MT_DT as action_dt , ta.master_name as addtype2 from clients_mst a inner join clients_mst b on " +
                //                               " a.panno = b.vch_num and b.type = 'BCD' and a.client_unit_id = b.client_unit_id inner join clients_mst c on a.c_name " +
                //                               "= c.vch_num and c.type = 'UNT' and a.client_unit_id = c.client_unit_id left join master_setting ta on ta.master_id " +
                //                               "= c.addtype2  and ta.type='TOA' and ta.client_unit_id=c.client_unit_id left join country_state ba on ba.rec_id =c.city2 " +
                //                               " left join clients_mst d on d.rec_id = a.cpname and d.type='UNC'" +
                //                               " where a.client_id||a.client_unit_id|| a.vch_num||to_char(a.vch_date, 'yyyymmdd')|| a.type='" + URL + "' ";
                //                           dtt = sgen.getdata(userCode, mq);
                //                           if (dtt.Rows.Count > 0)
                //                           {
                //                               sgen.SetSession(MyGuid, "EDMODE", "Y");
                //                               model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                //                               model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                //                               model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                //                               model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                //                               model[0].Col7 = dtt.Rows[0]["rec_id"].ToString();
                //                               model[0].Col8 = "(client_id||client_unit_id||vch_num||to_char(vch_date, 'yyyymmdd')|| type) = '" + URL + "'";
                //                               model[0].Col16 = dtt.Rows[0]["vch_num"].ToString();
                //                               model[0].Col17 = dtt.Rows[0]["Organisation"].ToString();
                //                               model[0].Col18 = dtt.Rows[0]["panno"].ToString();
                //                               model[0].Col19 = dtt.Rows[0]["Unit_Name"].ToString();
                //                               model[0].Col20 = dtt.Rows[0]["unit_id"].ToString();
                //                               model[0].Col26 = dtt.Rows[0]["pincode_2"].ToString();
                //                               model[0].Col25 = dtt.Rows[0]["city_name2"].ToString();
                //                               model[0].Col24 = dtt.Rows[0]["state_name2"].ToString();
                //                               model[0].Col23 = dtt.Rows[0]["country_name2"].ToString();
                //                               model[0].Col27 = dtt.Rows[0]["cpaddr2"].ToString();
                //                               model[0].Col31 = dtt.Rows[0]["addtype2"].ToString();
                //                               model[0].Col32 = dtt.Rows[0]["cont_id"].ToString();
                //                               model[0].Col21 = dtt.Rows[0]["cpcont"].ToString();
                //                               model[0].Col22 = dtt.Rows[0]["cpemail"].ToString();
                //                               model[0].Col29 = dtt.Rows[0]["COUNTRY2"].ToString();
                //                               model[0].Col28 = dtt.Rows[0]["COUNTRY"].ToString();
                //                               mq = "select rec_id,cpname from clients_mst where type='UNC' and vch_num='" + model[0].Col20 + "' and client_unit_id='" + unitid_mst + "'";
                //                               DataTable dtcont = new DataTable();
                //                               dtcont = sgen.getdata(userCode, mq);
                //                               if (dtcont.Rows.Count > 0)
                //                               {
                //                                   mod1 = sgen.dt_to_selectlist(dtcont);
                //                               }
                //                               TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
                //                               #region  next act
                //                               TempData[MyGuid + "_TList2"] = model[0].TList2 = mod2 = cmd_Fun.nextact(userCode, unitid_mst);
                //                               #endregion


                //                               String[] L1 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["cont_id"].ToString()).Distinct()).Split(',');
                //                               model[0].SelectedItems2 = L1;
                //                               String[] L2 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["PTYPE"].ToString()).Distinct()).Split(',');
                //                               model[0].SelectedItems2 = L2;
                //                           }
                //                           break;
                //                       case "CLIENT":
                //                           mq = @"select ct.rec_id as ct_id, cp.cp_mname,cp.cp_lname,ct.cp_mname as ct_cp_mname,ct.cp_lname as ct_cp_lname,cp.occ_type,cp.panno, b.c_name as clientName, cp.website,cp.addtype1, ta.master_name as addtype2, cp.salesarea, ct.rec_id as ct_rec_id, ct.client_id||ct.client_unit_id||ct.vch_num||to_char(ct.vch_date, 'yyyymmdd')|| ct.type as ct_fstr, ct.cpname as c_cpname,ct.cpcont as c_cpcont,ct.cpaltcont as c_cpaltcont,ct.cpemail as c_cpemail,ct.cpaddr as c_cpaddr,ct.cpdesig as c_cpdesig,ct.cplandno as c_cplandno,ct.cpdept as c_cpdept,ct.DOB as c_DOB,ct.doa as c_doa, cp.pincode_2,cp.sch_cat,cp.sch_med,cp.no_std,cp.no_teach,cp.Aff_type,
                //                             cp.cpdept, cp.cpname, cp.type_desc, cp.isgstr, cp.c_gstin, cp.micrno, cp.tor, cp.status, cp.pincode, cp.addr, cp.ifsc,
                //                                  cp.acctno, cp.contr, cp.swftcd, cp.bnk, cp.curtype, cp.acctype,  cp.ploc, cp.ptype, cp.tanno, 
                //                             cp.msmeno, cp.micrno,cp.bnkaddr, cp.cpdesig,cp.lsrno, cp.sez, cp.PR_TYPE ,cp.refered_by,
                //                           cp.file_no,cp.BILL_ADD,cp.rec_id,cp.c_name ,
                //                        to_char(convert_tz(cp.dob, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as dob," +
                //                         "to_char(convert_tz(cp.doa, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as doa," +
                //                        "to_char(convert_tz(cp.MT_DT, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldatetimeformat() + "') as MT_DT," +
                //                        "cp.cpemail ,cs.alpha_2 as country_id,cs.country_name,cs.state_gst_code as state_id ,cs.state_name," +
                //                       "cs.district_name,cs.city_name,cs.teh_name,cs.v_name,cp.city" +
                //                           ",ba.alpha_2 as country_id2,ba.country_name as country_name2, ba.state_gst_code as state_id2 ," +
                //                             "ba.state_name as state_name2 ,ba.district_name as district_name2,ba.city_name as city_name2," +
                //                             "ba.teh_name as teh_name2,ba.v_name as v_name2 ,cp.city2,cp.cpaddr2," +
                //                              "cp.c_gstin as cust_gstin_no,cp.CPLANDNO ,cp.cpcont ,cp.cpaltcont ,cp.cpaddr,ms.master_name as prop_type,cp.remark,cp.client_id," +
                //"cp.client_unit_id,cp.vch_num,cp.ent_by,cp.ent_date,cp.latlong,cp.google_add,cp.client_type from clients_mst cp left join clients_mst ct" +
                //" on cp.vch_num=ct.vch_num and cp.client_unit_id=ct.client_unit_id and ct.type='UNC' " +
                //"left join country_state cs on cs.rec_id = cp.city " +
                //"left join country_state ba on ba.rec_id = cp.city2 " +
                //"left join master_setting ms on ms.master_id = cp.PR_TYPE and ms.type='PRT' and ms.client_unit_id='" + unitid_mst + "' " +
                //"left join master_setting ta on ta.master_id = cp.addtype2  and ta.type='TOA' and ta.client_unit_id=cp.client_unit_id inner join clients_mst b on cp.panno=b.vch_num and b.type= cp.occ_type" +
                //                                          " and cp.client_unit_id=b.client_unit_id " +
                //"where cp.client_id||cp.client_unit_id|| cp.vch_num||to_char(cp.vch_date, 'yyyymmdd')|| cp.type='" + URL + "' ";
                //                           dtt = new DataTable();
                //                           dtt = sgen.getdata(userCode, mq);
                //                           if (dtt.Rows.Count > 0)
                //                           {
                //                               #region contact_person
                //                               foreach (DataRow dr in dtt.Rows)
                //                               {
                //                                   mod1.Add(new SelectListItem { Text = dr["c_cpname"].ToString(), Value = dr["ct_id"].ToString() });
                //                               }
                //                               TempData[MyGuid + "_Tlist1"] = model[0].TList1 = mod1;
                //                               #endregion
                //                               //         #region  typeofaddress
                //                               //         mod2 = cmd_Fun.addresstype(userCode, clientid_mst, unitid_mst);
                //                               //         TempData[MyGuid + "_TList2"] = mod2;
                //                               //;
                //                               //         #endregion

                //                               model[0].TList2 = mod2;
                //                               //String[] L2 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["addtype2"].ToString()).Distinct()).Split(',');
                //                               //model[0].SelectedItems2 = L2;
                //                               model[0].Col17 = dtt.Rows[0]["clientName"].ToString();
                //                               model[0].Col18 = dtt.Rows[0]["panno"].ToString();
                //                               model[0].Col19 = dtt.Rows[0]["c_name"].ToString();
                //                               model[0].Col20 = dtt.Rows[0]["vch_num"].ToString();
                //                               model[0].Col26 = dtt.Rows[0]["pincode_2"].ToString();
                //                               model[0].Col25 = dtt.Rows[0]["city_name2"].ToString();
                //                               model[0].Col24 = dtt.Rows[0]["state_name2"].ToString();
                //                               model[0].Col23 = dtt.Rows[0]["country_name2"].ToString();
                //                               model[0].Col27 = dtt.Rows[0]["cpaddr2"].ToString();
                //                               model[0].Col31 = dtt.Rows[0]["addtype2"].ToString();
                //                               model[0].Col32 = dtt.Rows[0]["ct_id"].ToString();
                //                           }
                //                           break;
                //                       case "FDEL":
                //                           string fid = sgen.GetSession(MyGuid, "delid").ToString();
                //                           res = sgen.execute_cmd(userCode, "Delete from file_tab WHERE rec_id='" + fid + "' and type='BCD' and substr(vch_num,0,3)='203' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                //                           sgen.SetSession(MyGuid, "delid", null);
                //                           if (res)
                //                           {
                //                               var LTM = model[0].LTM1;
                //                               for (int d = 0; d < LTM.Count; d++)
                //                               {
                //                                   var id = LTM[d].Col3;
                //                                   if (id == fid) { model[0].LTM1.RemoveAt(d); }
                //                               }
                //                           }
                //                           break;
                //                       case "LSR":
                //                           mq = "select from file_tab WHERE type='BCD' and substr(vch_num,0,3)='203' and find_in_set(client_unit_id,'" + unitid_mst + "')=1";
                //                           dtt = sgen.getdata(userCode, mq);
                //                           if (dtt.Rows.Count > 0)
                //                           {
                //                               model[0].Col32 = dtt.Rows[0]["lsrno"].ToString();
                //                           }
                //                           break;
                //                       case "ADD1":
                //                           // mq = "select * from country_state where client_id||client_unit_id||vch_num|| to_char(vch_date, 'yyyymmdd')||type='" + URL + "'";
                //                           mq = "select * from country_state where alpha_2||state_gst_code ||city_name='" + URL + "'";
                //                           DataTable dt1 = new DataTable();
                //                           dt1 = sgen.getdata(userCode, mq);
                //                           if (dt1.Rows.Count > 0)
                //                           {
                //                               model[0].Col59 = dt1.Rows[0]["country_name"].ToString();
                //                               model[0].Col60 = dt1.Rows[0]["state_name"].ToString();
                //                               model[0].Col61 = dt1.Rows[0]["city_name"].ToString();
                //                               //model[0].Col26 = dt1.Rows[0]["city_name"].ToString();
                //                               //model[0].Col62 = dt1.Rows[0]["teh_name"].ToString();
                //                               //model[0].Col63 = dt1.Rows[0]["v_name"].ToString();
                //                               model[0].Col64 = dt1.Rows[0]["rec_id"].ToString();
                //                               model[0].Col49 = dt1.Rows[0]["alpha_2"].ToString();
                //                               model[0].Col71 = dt1.Rows[0]["state_gst_code"].ToString();
                //                               if (model[0].Chk4 == true)
                //                               {
                //                                   //model[0].Col65 = dt1.Rows[0]["v_name"].ToString();
                //                                   //model[0].Col66 = dt1.Rows[0]["teh_name"].ToString();
                //                                   model[0].Col67 = dt1.Rows[0]["city_name"].ToString();
                //                                   model[0].Col68 = dt1.Rows[0]["state_name"].ToString();
                //                                   model[0].Col69 = dt1.Rows[0]["country_name"].ToString();
                //                                   model[0].Col48 = dt1.Rows[0]["rec_id"].ToString();
                //                                   model[0].Col72 = dt1.Rows[0]["alpha_2"].ToString();
                //                                   model[0].Col73 = dt1.Rows[0]["state_gst_code"].ToString();
                //                               }
                //                           }
                //                           break;
                //                       case "ADD2":
                //                           // mq = "select * from country_state where client_id||client_unit_id||vch_num|| to_char(vch_date, 'yyyymmdd')||type='" + URL + "'";
                //                           mq = "select * from country_state where alpha_2||state_gst_code ||city_name='" + URL + "'";
                //                           dt1 = new DataTable();
                //                           dt1 = sgen.getdata(userCode, mq);
                //                           if (dt1.Rows.Count > 0)
                //                           {
                //                               //model[0].Col65 = dt1.Rows[0]["v_name"].ToString();
                //                               //model[0].Col66 = dt1.Rows[0]["teh_name"].ToString();
                //                               model[0].Col67 = dt1.Rows[0]["city_name"].ToString();
                //                               model[0].Col68 = dt1.Rows[0]["state_name"].ToString();
                //                               model[0].Col69 = dt1.Rows[0]["country_name"].ToString();
                //                               model[0].Col48 = dt1.Rows[0]["rec_id"].ToString();
                //                               model[0].Col72 = dt1.Rows[0]["alpha_2"].ToString();
                //                               model[0].Col73 = dt1.Rows[0]["state_gst_code"].ToString();
                //                           }
                //                           break;
                //                   }
                //                   break;
                #endregion
                #region pur report
                case "pur_rpt":
                    switch (btnval.ToUpper())
                    {
                        case "POPARTYSUM":
                            Make_query(viewName, "Detailed PO Party Wise", "POPARTYDET", "0", URL, "", MyGuid);
                            ViewBag.scripCall = "callFoo('Detailed PO Party Wise');";
                            sgen.SetSession(MyGuid, "PURRPTBTN", null);
                            break;
                        case "POITSUM":
                            Make_query(viewName, "Detailed PO ITEM Wise", "POITDET", "0", URL, "", MyGuid);
                            ViewBag.scripCall = "callFoo('Detailed PO Item Wise');";
                            sgen.SetSession(MyGuid, "PURRPTBTN", null);
                            break;
                    }
                    break;
                #endregion
                #region pur report2
                case "pur_rpt2":
                    switch (btnval.ToUpper())
                    {
                        case "POPARTYSUM":
                            Make_query(viewName, "Detailed PO Party Wise", "POPARTYDET", "0", URL, "", MyGuid);
                            ViewBag.scripCall = "callFoo('Detailed PO Party Wise');";
                            sgen.SetSession(MyGuid, "PURRPTBTN", null);
                            break;
                        case "POITSUM":
                            Make_query(viewName, "Detailed PO ITEM Wise", "POITDET", "0", URL, "", MyGuid);
                            ViewBag.scripCall = "callFoo('Detailed PO Item Wise');";
                            sgen.SetSession(MyGuid, "PURRPTBTN", null);
                            break;
                    }
                    break;
                #endregion
                #region PO shedule
                case "po_sch":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = " select sd.rec_id,sd.client_id,sd.col20,sd.client_unit_id,sd.ent_by,sd.ent_date,sd.edit_by,sd.edit_date,sd.col1 as pt_code,sd.type,cl.c_name as name,cl.c_gstin as gstin,cl.addr," +
                                "to_char(convert_tz(sd.vch_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') as Doc_date,sd.vch_num as Sdl_no," +
                                "sd.col4 as po_no,sd.col21 as Remark1,to_char(sd.date2,'" + sgen.Getsqldateformat() + "') as sdl_st_date," +
                                "to_char(sd.date3,'" + sgen.Getsqldateformat() + "') as sdl_end_date," +
                                "to_char(sd.date6 ,'" + sgen.Getsqldateformat() + "') as PO_dt2" +
                                ",to_char(sd.date7,'" + sgen.Getsqldateformat() + "') as PO_dt1," +
                                "to_char(sd.date8 ,'" + sgen.Getsqldateformat() + "') as Sdl_st_dt," +
                                "to_char(sd.date9 ,'" + sgen.Getsqldateformat() + "') as Sdl_en_dt,sd.col5 as Icode," +
                                "sd.col27 as IName,sd.col7 as PartNo, sd.col8 as UOM, sd.col9 as Order_qty,sd.col10 as Plan_qty," +
                                "sd.col12 as remark2,sd.col13 as po_no2 from kc_tab sd " +
                                "inner join clients_mst cl on cl.vch_num = sd.col1 and cl.type = 'BCD' and find_in_set(cl.client_unit_id,sd.client_unit_id)=1 " +
                                "where (sd.client_id || sd.client_unit_id || sd.vch_num || to_char(sd.vch_date, 'yyyymmdd') || sd.type) = '" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model = getedit(model, dt, URL);
                                model[0].Col12 = dt.Rows[0]["type"].ToString();
                                sgen.SetSession(MyGuid, "BDPOTYPE", dt.Rows[0]["type"].ToString());
                                model[0].Col16 = dt.Rows[0]["Sdl_no"].ToString();
                                model[0].Col49 = dt.Rows[0]["pt_code"].ToString();
                                sgen.SetSession(MyGuid, "PRTY_ID", dt.Rows[0]["pt_code"].ToString());
                                model[0].Col19 = dt.Rows[0]["name"].ToString();
                                model[0].Col20 = dt.Rows[0]["gstin"].ToString();
                                model[0].Col21 = dt.Rows[0]["addr"].ToString();
                                model[0].Col22 = dt.Rows[0]["Doc_date"].ToString();
                                model[0].Col29 = dt.Rows[0]["PO_no"].ToString();
                                model[0].Col31 = dt.Rows[0]["PO_dt1"].ToString();
                                sgen.SetSession(MyGuid, "BDPONO", dt.Rows[0]["PO_no"].ToString());
                                model[0].Col28 = dt.Rows[0]["Remark1"].ToString();
                                model[0].Col40 = dt.Rows[0]["col20"].ToString();
                                model[0].Col24 = dt.Rows[0]["sdl_st_date"].ToString();
                                model[0].Col25 = dt.Rows[0]["sdl_end_date"].ToString();
                                model[0].dt1 = model[0].dt1.Clone();
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["SNo"] = i + 1;
                                    dr["Icode"] = dt.Rows[i]["Icode"].ToString();
                                    dr["IName"] = dt.Rows[i]["IName"].ToString();
                                    dr["PartNo"] = dt.Rows[i]["PartNo"].ToString();
                                    dr["UOM"] = dt.Rows[i]["UOM"].ToString();
                                    dr["Order_qty"] = dt.Rows[i]["Order_qty"].ToString();
                                    dr["Plan_qty"] = dt.Rows[i]["Plan_qty"].ToString();
                                    dr["Sdl_start_dt"] = dt.Rows[i]["Sdl_st_dt"].ToString();
                                    dr["Sdl_end_dt"] = dt.Rows[i]["Sdl_en_dt"].ToString();
                                    dr["PO_No"] = dt.Rows[i]["PO_No2"].ToString();
                                    dr["PO_Date"] = dt.Rows[i]["PO_dt2"].ToString();
                                    dr["Remark"] = dt.Rows[i]["remark2"].ToString();
                                    model[0].dt1.Rows.Add(dr);
                                }
                                model[0].dt1.AcceptChanges();
                            }
                            break;
                        case "NEW":
                            mq = "select master_id,upper(master_name) master_name,to_char(convert_tz(utc_timestamp(),'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') po_date " +
                                   "from master_Setting where master_id||To_Char(vch_date, 'yyyymmdd')||type = '" + URL + "' ";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model[0].Col12 = dt.Rows[0]["master_id"].ToString();
                                sgen.SetSession(MyGuid, "BDPOTYPE", dt.Rows[0]["master_id"].ToString());
                                model = getnew(model);
                                model[0].Col13 = "Save";
                                model[0].Col100 = "Save & New";
                                model[0].Col121 = "S";
                                model[0].Col122 = "<u>S</u>ave";
                                model[0].Col123 = "Save & Ne<u>w</u>";
                                model[0].Col16 = vch_num;
                                model[0].Col9 = dt.Rows[0]["master_name"].ToString();
                                model[0].Col22 = sgen.server_datetime_local(userCode);
                                Make_query(viewName.ToLower(), "Select Party", "PARTY", "1", "", "", MyGuid);
                                ViewBag.scripCall = "callFoo('Select Party');";
                            }
                            break;
                        case "GETSO":
                            mq = "select group_concat(distinct a.vch_num) as po_no from purchases a where (a.client_id || a.client_unit_id || a.vch_num || to_char(a.vch_date, 'yyyymmdd') || a.type) in ('" + URL + "') ";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model[0].Col29 = dt.Rows[0]["po_no"].ToString();
                                sgen.SetSession(MyGuid, "BDPONO", dt.Rows[0]["po_no"].ToString());
                                ViewBag.vimport = "";
                            }
                            string[] xfstr = URL.Split(',');
                            string xfstrnew = xfstr[0].Substring(0, 25);
                            mq1 = "select distinct to_char(convert_tz(a.vch_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') as so_date from purchases a where " +
                                "(a.client_id || a.client_unit_id || a.vch_num || to_char(a.vch_date, 'yyyymmdd')|| a.type) in ('" + xfstrnew + "') ";
                            dt = sgen.getdata(userCode, mq1);
                            if (dt.Rows.Count > 0)
                            {
                                model[0].Col31 = dt.Rows[0]["so_date"].ToString();
                            }
                            break;
                        case "PARTY":
                            mq = "select a.c_name as name,a.c_gstin as gstin,a.addr,a.vch_num,a.tor from clients_mst a " +
                                "where (a.vch_num||to_char(a.vch_date, 'yyyymmdd')|| a.type) = '" + URL + "'";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                model[0].Col49 = dtt.Rows[0]["vch_num"].ToString();
                                model[0].Col19 = dtt.Rows[0]["name"].ToString();
                                model[0].Col20 = dtt.Rows[0]["gstin"].ToString();
                                model[0].Col21 = dtt.Rows[0]["addr"].ToString();
                                sgen.SetSession(MyGuid, "PRTY_ID", dtt.Rows[0]["vch_num"].ToString());
                                Make_query(viewName.ToLower(), "Select Purchase Order No.", "GETSO", "2", "", "", MyGuid);
                                ViewBag.scripCall = "callFoo('Select Purchase Order No.');";
                            }
                            break;
                        case "ITEM":
                            mq = "select (it.icode || pc.type || pc.po_no || to_char(it.vch_date, 'yyyymmdd') || it.type) as fstr,it.icode as Icode,it.iname as Iname,it.cpartno Partno, hsn.master_name as HSN,nvl(pc.bal_qty, '0') as Order_qty" +
                                ",um.master_name as UOM,pc.po_no as po_num,to_char(pc.po_date, '" + sgen.Getsqldateformat() + "') as podate from item it inner join (select a.vch_num as po_no,a.vch_date as po_date,a.icode,a.type, a.po_qty,a.acode," +
                                "a.client_unit_id,nvl(sum(a.mrn_qty), '0') as used_qty,a.po_qty - nvl(sum(a.mrn_qty), '0') as bal_qty from(select pc.vch_num, pc.vch_date, pc.icode, pc.type, pc.qtyord as po_qty,nvl(to_number(iv.qtyrcvd),'0') as mrn_qty," +
                                " pc.acode, pc.client_unit_id from purchases pc left join itransaction iv on iv.pono = pc.vch_num and iv.type IN('02', '03') and pc.icode = iv.icode and iv.client_unit_id = pc.client_unit_id where " +
                                "substr(pc.type, 1, 1) = '5' and pc.client_unit_id = '" + unitid_mst + "' union all select pc.vch_num, pc.vch_date, pc.icode, pc.type, pc.qtyord as po_qty," +
                                " nvl(to_number(ds.col10),'0') as sdl_qty, pc.acode, pc.client_unit_id from purchases pc left join kc_tab ds on ds.col13 = pc.vch_num and pc.icode = ds.col5 " +
                                "and ds.col20 = 'BPS' and ds.client_unit_id = pc.client_unit_id where substr(pc.type, 1, 1) = '5' and pc.client_unit_id = '" + unitid_mst + "' " +
                                "union all select pc.vch_num, pc.vch_date, pc.icode, pc.type, pc.qtyord as po_qty,nvl(to_number(iv.col11), '0') as mrn_qty,pc.acode, " +
                                "pc.client_unit_id from purchases pc left join enx_tab iv on iv.col12 = pc.vch_num and iv.col7 = pc.icode and " +
                                "iv.type = 'POR' and iv.client_unit_id = pc.client_unit_id where substr(pc.type,1,1)= '5' and pc.client_unit_id = '" + unitid_mst + "') a group by " +
                                "a.vch_num, a.icode, a.type,a.po_qty, a.vch_date,a.acode,a.client_unit_id) pc on pc.icode = it.icode inner join clients_mst cl on cl.vch_num = pc.acode and cl.type = 'BCD' and substr(cl.vch_num,0,3)= '203' and" +
                                " cl.client_unit_id = pc.client_unit_id left join master_setting um on um.master_id = it.uom and um.type = 'UMM' and find_in_set(um.client_unit_id, it.client_unit_id) = 1" +
                                " left join master_setting hsn on hsn.master_id = it.hsno and hsn.type = 'HSN' and find_in_set(hsn.client_unit_id, it.client_unit_id)= 1 where " +
                                " (it.icode || pc.type || pc.po_no || to_char(it.vch_date, 'yyyymmdd') || it.type) in ('" + URL + "')";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                if (model[0].dt1.Rows[0]["icode"].ToString().Trim().Equals("-") || model[0].dt1.Rows[0]["icode"].ToString().Trim().Equals("")) { model[0].dt1.Rows.RemoveAt(0); }
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["Item"] = dt.Rows[i]["fstr"].ToString();
                                    dr["SNo"] = i + 1;
                                    dr["Icode"] = dt.Rows[i]["icode"].ToString();
                                    dr["IName"] = dt.Rows[i]["iname"].ToString();
                                    dr["PartNo"] = dt.Rows[i]["Partno"].ToString();
                                    //dr["HSN"] = dt.Rows[i]["HSN"].ToString();
                                    dr["UOM"] = dt.Rows[i]["UOM"].ToString();
                                    dr["Order_qty"] = dt.Rows[i]["Order_qty"].ToString();
                                    dr["PO_No"] = dt.Rows[i]["po_num"].ToString();
                                    dr["PO_Date"] = dt.Rows[i]["podate"].ToString();
                                    model[0].dt1.Rows.Add(dr);
                                }
                                model[0].dt1.AcceptChanges();
                            }
                            break;
                        case "PRINT":
                            ViewBag.vnew = "";
                            ViewBag.vedit = "";
                            ViewBag.vsave = "disabled='disabled'";
                            ViewBag.vsavenew = "disabled='disabled'";
                            ViewBag.scripCall = "disableForm();";
                            mq = "select a.totremark, REPLACE(ud.FIRST_NAME|| ' '|| ud.MIDDLE_NAME|| ' '|| ud.last_name,'0','') as created_by ,it.partname, d.master_name as deptname, lp.loc, a.vch_num, a.icode, it.iname,it.cpartno ,um.master_name as UOM,um.master_name as UOM2,hsn.master_name as hsn," +
                                " to_char(convert_tz(a.vch_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') vch_date1,a.pono,it.cuom, " +
                                " a.qtychl,a.qtyout,a.qtybal " +
                                "from itransaction a inner join item it on a.icode=it.icode  and it.type='IT' and find_in_set(a.client_unit_id,it.client_unit_id)=1 " +
                                "inner join master_setting um on um.master_id=it.uom and um.type='UMM' and um.client_unit_id='" + unitid_mst + "' " +
                                "inner join master_setting um2 on um2.master_id=it.uom2 and um2.type='UMM' and um2.client_unit_id='" + unitid_mst + "' " +
                                "inner join master_setting hsn on hsn.master_id=it.hsno and hsn.type='HSN' and hsn.client_unit_id='" + unitid_mst + "'" +
                                "inner join (select (a.client_id||a.client_unit_id||b.master_id||f.master_id||rm.master_id||rk.master_id||a.master_id) as fstr," +
                        "(b.master_name || ct.param1 || f.master_name || ct.param1 || rm.master_name || ct.param1 || rk.master_name || ct.param1 || a.master_name) Loc from master_setting a " +
                        "inner join master_setting b on b.master_id = a.classid and b.type = 'HBM' and a.client_unit_id = b.client_unit_id " +
                        "inner join master_setting f on f.master_id = a.sectionid and f.type = 'IN0' and a.client_unit_id = f.client_unit_id " +
                        "inner join master_setting rm on rm.master_id = a.client_name and rm.type = 'IN1' and a.client_unit_id = rm.client_unit_id " +
                        "inner join master_setting rk on rk.master_id = a.subjectid and rk.type = 'IN2' and a.client_unit_id = rk.client_unit_id " +
                        "inner join controls ct on 1 = 1 and ct.id = '000010' " +
                        "where a.type='IN3' and a.client_unit_id = '" + unitid_mst + "')lp on lp.fstr=it.loc" +
                        " inner join master_setting d on d.master_id=a.deptno and d.type='MDP' inner join user_details ud on a.ent_by = ud.vch_num and ud.type = 'CPR' " +
                                "where (a.client_id||a.client_unit_id||a.vch_num|| to_char(a.vch_date, 'yyyymmdd')||a.type)='" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                DataTable dt2 = new DataTable();
                                DataTable qrdt = new DataTable();
                                qrdt = dt.Copy();
                                qrdt.Columns.Add("qrimg", typeof(Byte[]));
                                foreach (DataRow dr in qrdt.Rows)
                                {
                                    string code = dr["icode"].ToString() + "\n" + dr["iname"].ToString();
                                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                                    QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);
                                    System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
                                    imgBarCode.Height = 150;
                                    imgBarCode.Width = 150;
                                    using (Bitmap bitMap = qrCode.GetGraphic(20))
                                    {
                                        using (MemoryStream ms = new MemoryStream())
                                        {
                                            bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                                            byte[] byteImage = ms.ToArray();
                                            dr["qrimg"] = byteImage;
                                        }
                                    }
                                }
                                qrdt.AcceptChanges();
                                qrdt.TableName = "prepcur";
                                sgen.open_report_bydt_xml(userCode, qrdt, "dispatch", "Dispatch");
                                ViewBag.scripCall += "showRptnew('Dispatch Detail');disableForm();";

                                //ViewBag.scripCall = "OpenSingle('../../../../../erp/schoolReports_Rpts/Filter_Report_xml.aspx','95%','900px','Reports');";
                            }
                            break;
                    }
                    break;
                #endregion
                #region vd_po
                case "vd_po":
                    switch (btnval.ToUpper())
                    {
                        case "INPO":
                        case "ACPO":
                        case "REJPO":
                            mq = "select a.*,to_char(convert_tz(vch_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') vch_date1," +
                                "to_char(dlv_date,'" + sgen.Getsqldateformat() + "') dlv_date1," +
                                "to_char(convert_tz(inddate,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') inddate1," +
                                "to_char(convert_tz(vch_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') date11," +
                                "to_char(convert_tz(vch_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') date21," +
                                "to_char(convert_tz(vch_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') date31 from purchases a " +
                                "where (client_id||client_unit_id||vch_num||to_char(vch_date, 'yyyymmdd')|| type) = '" + URL + "'";
                            dtp = sgen.getdata(userCode, mq);
                            if (dtp.Rows.Count > 0)
                            {
                                #region ddl
                                TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1 = cmd_Fun.payterm(userCode, unitid_mst, out defcall);              //payterm
                                TempData[MyGuid + "_TList2"] = model[0].TList2 = mod2 = cmd_Fun.priceterm(userCode, unitid_mst);           //priceterm
                                TempData[MyGuid + "_TList3"] = model[0].TList3 = mod3 = cmd_Fun.transportmode(userCode, unitid_mst);       //transport 
                                TempData[MyGuid + "_TList4"] = model[0].TList4 = mod4 = cmd_Fun.Modeofpayment(userCode, unitid_mst);       //payment mode 
                                TempData[MyGuid + "_TList5"] = model[0].TList5 = mod5 = cmd_Fun.insby(userCode, unitid_mst);               //insby 
                                TempData[MyGuid + "_TList6"] = model[0].TList6 = mod6 = cmd_Fun.delterm(userCode, unitid_mst);             //delterm 
                                TempData[MyGuid + "_TList7"] = model[0].TList7 = mod7 = cmd_Fun.curname(userCode, unitid_mst);             //curname

                                String[] L1 = System.String.Join(",", dtp.Rows.OfType<DataRow>().Select(r => r["pay_term"].ToString()).Distinct()).Split(',');
                                String[] L2 = System.String.Join(",", dtp.Rows.OfType<DataRow>().Select(r => r["price_term"].ToString()).Distinct()).Split(',');
                                String[] L3 = System.String.Join(",", dtp.Rows.OfType<DataRow>().Select(r => r["tmode"].ToString()).Distinct()).Split(',');
                                String[] L4 = System.String.Join(",", dtp.Rows.OfType<DataRow>().Select(r => r["paymode"].ToString()).Distinct()).Split(',');
                                String[] L5 = System.String.Join(",", dtp.Rows.OfType<DataRow>().Select(r => r["insby"].ToString()).Distinct()).Split(',');
                                String[] L6 = System.String.Join(",", dtp.Rows.OfType<DataRow>().Select(r => r["delterm"].ToString()).Distinct()).Split(',');
                                String[] L7 = System.String.Join(",", dtp.Rows.OfType<DataRow>().Select(r => r["doccur"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems1 = L1;
                                model[0].SelectedItems2 = L2;
                                model[0].SelectedItems3 = L3;
                                model[0].SelectedItems4 = L4;
                                model[0].SelectedItems5 = L5;
                                model[0].SelectedItems6 = L6;
                                model[0].SelectedItems7 = L7;
                                #endregion
                                model[0].Col56 = dtp.Rows[0]["tname"].ToString();
                                model[0].Col57 = dtp.Rows[0]["pkins"].ToString();
                                model[0].Col58 = dtp.Rows[0]["tcstatus"].ToString();
                                model[0].Col59 = dtp.Rows[0]["tcremark"].ToString();
                                model[0].Col67 = dtp.Rows[0]["gstcr"].ToString();
                                model[0].Col68 = dtp.Rows[0]["gstcrremark"].ToString();
                                model[0].Col70 = dtp.Rows[0]["col1"].ToString();
                                model[0].Col71 = dtp.Rows[0]["col2"].ToString();
                                model[0].Col72 = dtp.Rows[0]["col3"].ToString();
                                model[0].Col73 = dtp.Rows[0]["date11"].ToString();
                                model[0].Col74 = dtp.Rows[0]["date21"].ToString();
                                model[0].Col75 = dtp.Rows[0]["date31"].ToString();
                                model[0].Col76 = dtp.Rows[0]["col4"].ToString();
                                model[0].Col77 = dtp.Rows[0]["col5"].ToString();
                                model[0].Col78 = dtp.Rows[0]["col6"].ToString();
                                model[0].Col60 = dtp.Rows[0]["pref"].ToString();
                                model[0].Col61 = dtp.Rows[0]["taxre"].ToString();
                                model[0].Col62 = dtp.Rows[0]["shpfval"].ToString();
                                model[0].Col63 = dtp.Rows[0]["shptval"].ToString();
                                model[0].Col79 = sgen.seekval(userCode, "select upper(master_name) master_name from (select '001' fstr,'001' master_id,'With Indent' master_name from dual union all " +
                   "select '002' fstr,'002' master_id,'Without Indent' master_name from dual) a where fstr='" + dtp.Rows[0]["pur_type"].ToString().Trim() + "'", "master_name");
                                model[0].Col31 = dtp.Rows[0]["totremark"].ToString();
                                model[0].Col32 = dtp.Rows[0]["cond"].ToString();
                                model[0].Col33 = dtp.Rows[0]["basicamt"].ToString();
                                model[0].Col34 = dtp.Rows[0]["igst"].ToString();
                                model[0].Col35 = dtp.Rows[0]["cgst"].ToString();
                                model[0].Col36 = dtp.Rows[0]["sgst"].ToString();
                                model[0].Col37 = dtp.Rows[0]["gamt"].ToString();
                                model[0].Col38 = dtp.Rows[0]["gigst"].ToString();
                                model[0].Col39 = dtp.Rows[0]["gcgst"].ToString();
                                model[0].Col40 = dtp.Rows[0]["gsgst"].ToString();
                                model[0].Col69 = dtp.Rows[0]["netamt"].ToString();
                                model[0].Col82 = dtp.Rows[0]["vdst"].ToString();
                                int ver = sgen.Make_int(dtp.Rows[0]["ver"].ToString() == "" ? "1" : dtp.Rows[0]["ver"].ToString()) + 1;
                                model[0].Col41 = ver.ToString();
                                DataTable dte = new DataTable();
                                mq1 = "select a.c_name as name,a.c_gstin as gstin,a.addr,a.vch_num,a.tor from clients_mst a where find_in_set(a.client_unit_id,'" + unitid_mst + "')=1 and a.type='BCD' and a.vch_num='" + dtp.Rows[0]["acode"].ToString() + "' and substr(a.vch_num,0,3)='203'";
                                dte = sgen.getdata(userCode, mq1);
                                if (dte.Rows.Count > 0)
                                {
                                    model[0].Col49 = dte.Rows[0]["vch_num"].ToString();
                                    model[0].Col20 = dte.Rows[0]["name"].ToString();
                                    model[0].Col21 = dte.Rows[0]["addr"].ToString();
                                    model[0].Col22 = dte.Rows[0]["gstin"].ToString();
                                    model[0].Col52 = dte.Rows[0]["tor"].ToString();
                                }
                                mq1 = "select a.c_name as name,a.c_gstin as gstin,a.addr,a.vch_num from clients_mst a where " +
                                    "find_in_set(a.client_unit_id,'" + unitid_mst + "')=1 and a.type='BCD' and a.vch_num='" + dtp.Rows[0]["shipfrom"].ToString() + "' " +
                                  " and substr(a.vch_num,0,3)='203'";
                                dte = sgen.getdata(userCode, mq1);
                                if (dte.Rows.Count > 0)
                                {
                                    model[0].Col50 = dte.Rows[0]["vch_num"].ToString();
                                    model[0].Col23 = dte.Rows[0]["name"].ToString();
                                    model[0].Col24 = dte.Rows[0]["addr"].ToString();
                                    model[0].Col25 = dte.Rows[0]["gstin"].ToString();
                                }
                                mq1 = "select a.c_name as name,a.c_gstin as gstin,a.addr,a.vch_num from clients_mst a where find_in_set(a.client_unit_id,'" + unitid_mst + "')=1 and a.type='BCD' and a.vch_num='" + dtp.Rows[0]["shipto"].ToString() + "' and substr(a.vch_num,0,3)='203'";
                                dte = sgen.getdata(userCode, mq1);
                                if (dte.Rows.Count > 0)
                                {
                                    model[0].Col51 = dte.Rows[0]["vch_num"].ToString();
                                    model[0].Col26 = dte.Rows[0]["name"].ToString();
                                    model[0].Col27 = dte.Rows[0]["addr"].ToString();
                                    model[0].Col28 = dte.Rows[0]["gstin"].ToString();
                                }
                                if (btnval.ToUpper().Equals("EDIT") || btnval.ToUpper().Equals("VIEW"))
                                {
                                    sgen.SetSession(MyGuid, "EDMODE", "Y");
                                    model[0].Col1 = dtp.Rows[0]["client_id"].ToString();
                                    model[0].Col2 = dtp.Rows[0]["client_unit_id"].ToString();
                                    model[0].Col3 = dtp.Rows[0]["ent_by"].ToString();
                                    model[0].Col4 = dtp.Rows[0]["ent_date"].ToString();
                                    model[0].Col5 = dtp.Rows[0]["edit_by"].ToString();
                                    model[0].Col6 = dtp.Rows[0]["edit_date"].ToString();
                                    model[0].Col8 = "(client_id||client_unit_id||vch_num||to_char(vch_date, 'yyyymmdd')|| type) = '" + URL + "'";
                                    model[0].Col9 = sgen.seekval(userCode, "select upper(master_name) master_name from master_setting where master_id='" + dtp.Rows[0]["type"].ToString() + "' and type='KPO'", "master_name");
                                    model[0].Col10 = model[0].Col10;
                                    model[0].Col11 = model[0].Col11;
                                    model[0].Col12 = dtp.Rows[0]["type"].ToString();
                                    model[0].Col13 = "Update";
                                    model[0].Col100 = "Update & New";
                                    model[0].Col121 = "U";
                                    model[0].Col122 = "<u>U</u>pdate";
                                    model[0].Col123 = "Update & Ne<u>w</u>";
                                    model[0].Col14 = model[0].Col14;
                                    model[0].Col15 = model[0].Col15;
                                    model[0].Col16 = dtp.Rows[0]["vch_num"].ToString();
                                    model[0].Col47 = dtp.Rows[0]["vch_num"].ToString();
                                    model[0].Col48 = dtp.Rows[0]["vch_date1"].ToString();
                                    model[0].Col29 = dtp.Rows[0]["pur_type"].ToString();
                                    if (btnval.ToUpper().Equals("VIEW"))
                                    {
                                        ViewBag.vnew = "disabled='disabled'";
                                        ViewBag.vedit = "disabled='disabled'";
                                        ViewBag.vsave = "disabled='disabled'";
                                        ViewBag.vsavenew = "disabled='disabled'";
                                        ViewBag.scripCall += "disableForm();";
                                    }
                                }
                                else
                                {
                                    sgen.SetSession(MyGuid, "EDMODE", "N");
                                    model[0].Col1 = dtp.Rows[0]["client_id"].ToString();
                                    model[0].Col2 = dtp.Rows[0]["client_unit_id"].ToString();
                                    model[0].Col3 = dtp.Rows[0]["ent_by"].ToString();
                                    model[0].Col4 = dtp.Rows[0]["ent_date"].ToString();
                                    model[0].Col5 = dtp.Rows[0]["edit_by"].ToString();
                                    model[0].Col6 = dtp.Rows[0]["edit_date"].ToString();
                                    model[0].Col8 = "(client_id||client_unit_id||vch_num||to_char(vch_date, 'yyyymmdd')|| type) = '" + URL + "'";
                                    model[0].Col9 = sgen.seekval(userCode, "select upper(master_name) master_name from master_setting where master_id='" + dtp.Rows[0]["type"].ToString() + "' and type='KPO'", "master_name");
                                    model[0].Col10 = model[0].Col10;
                                    model[0].Col11 = model[0].Col11;
                                    model[0].Col12 = dtp.Rows[0]["type"].ToString();
                                    model[0].Col13 = "Update";
                                    model[0].Col100 = "Update & New";
                                    model[0].Col121 = "U";
                                    model[0].Col122 = "<u>U</u>pdate";
                                    model[0].Col123 = "Update & Ne<u>w</u>";
                                    model[0].Col14 = model[0].Col14;
                                    model[0].Col15 = model[0].Col15;
                                    model[0].Col16 = dtp.Rows[0]["vch_num"].ToString();
                                    model[0].Col47 = dtp.Rows[0]["vch_num"].ToString();
                                    model[0].Col48 = dtp.Rows[0]["vch_date1"].ToString();
                                    model[0].Col29 = dtp.Rows[0]["pur_type"].ToString();
                                }
                                DataTable dtf = new DataTable();
                                if (dtp.Rows[0]["type"].ToString().Trim().Equals("50") || dtp.Rows[0]["type"].ToString().Trim().Equals("52") || dtp.Rows[0]["type"].ToString().Trim().Equals("54"))
                                {
                                    if (dtp.Rows[0]["pur_type"].ToString().Equals("002"))
                                    {
                                        dtt = sgen.getdata(userCode, "select '' Item,'1' as  SNo ,'-' as Icode,'-' as IName,'-' PARTNO,'-'  HSN,'-' as UOM,'0' as TaxRate," +
                                            "'0' Qty_in_stock,'0' Qty_Ord,'0' IRate,'0' disc_rate,'0' IAmount,'0' TaxAmount,'0' discAmount,'0' LineAmount,'-' delivery_date," +
                                            "'-' as Remark from dual");
                                        model[0].dt1 = dtt;
                                        dtf = model[0].dt1.Clone();
                                        sgen.SetSession(MyGuid, "KPO_DT", dtt);
                                    }
                                    else
                                    {
                                        dtt = sgen.getdata(userCode, "select '' Item,'1' SNo ,'-' as Icode,'-' as IName,'-' PARTNO,'-'  HSN,'-' as UOM,'0' as TaxRate," +
                                            "'0' as Qty_in_stock,'0' as Qty_Ord,'0' Qty_Req,'0' Qty_bal,'0' as IRate,'0' as disc_rate,'0' IAmount,'0' TaxAmount,'0' discAmount," +
                                            "'0' LineAmount,'-' as delivery_date,'-' as Remark,'-' Indentno,'-' Indent_Date from dual");
                                        model[0].dt2 = dtt;
                                        dtf = model[0].dt2.Clone();
                                        sgen.SetSession(MyGuid, "KPO_DT", dtt);
                                    }
                                }
                                for (int i = 0; i < dtp.Rows.Count; i++)
                                {
                                    DataRow dr = dtf.NewRow();
                                    dr["SNo"] = i + 1;
                                    dr["Icode"] = dtp.Rows[i]["icode"].ToString();
                                    dr["IName"] = dtp.Rows[i]["iname"].ToString();
                                    dr["PARTNO"] = dtp.Rows[i]["cpartno"].ToString();
                                    dr["HSN"] = dtp.Rows[i]["hsno"].ToString();
                                    dr["UOM"] = dtp.Rows[i]["uom"].ToString();
                                    dr["TaxRate"] = dtp.Rows[i]["taxrate"].ToString() + "%";
                                    dr["Qty_in_stock"] = dtp.Rows[i]["qtystk"].ToString();
                                    if (dtp.Rows[0]["pur_type"].ToString().Equals("001"))
                                    {
                                        dr["Qty_ord"] = dtp.Rows[i]["qtyind"].ToString();
                                        dr["Qty_Req"] = dtp.Rows[i]["qtyord"].ToString();
                                        dr["Qty_Bal"] = dtp.Rows[i]["qtybal"].ToString();
                                        dr["IndentNo"] = dtp.Rows[i]["indno"].ToString();
                                        dr["Indent_Date"] = dtp.Rows[i]["inddate1"].ToString();
                                    }
                                    else
                                    {
                                        dr["Qty_Ord"] = dtp.Rows[i]["qtyord"].ToString();
                                    }
                                    dr["IRate"] = dtp.Rows[i]["irate"].ToString();
                                    dr["disc_rate"] = dtp.Rows[i]["discrate"].ToString();
                                    dr["Iamount"] = dtp.Rows[i]["Iamount"].ToString();
                                    dr["taxamount"] = dtp.Rows[i]["taxamt"].ToString();
                                    dr["discamount"] = dtp.Rows[i]["discamt"].ToString();
                                    dr["lineamount"] = dtp.Rows[i]["lineamount"].ToString();
                                    dr["delivery_date"] = dtp.Rows[i]["dlv_date1"].ToString();
                                    dr["Remark"] = dtp.Rows[i]["iremark"].ToString();
                                    dtf.Rows.Add(dr);
                                }
                                dtf.AcceptChanges();
                                if (dtp.Rows[0]["type"].ToString().Trim().Equals("50") || dtp.Rows[0]["type"].ToString().Trim().Equals("52") || dtp.Rows[0]["type"].ToString().Trim().Equals("54"))
                                {
                                    if (dtp.Rows[0]["pur_type"].ToString().Equals("002")) { model[0].dt1 = dtf; }
                                    else { model[0].dt2 = dtf; }
                                }
                                //other charges LTM
                                mq1 = "select ch.master_name chrgname,en.col1,en.col2,en.col3,en.col4,en.col5,en.col6,en.col7,en.col8,en.col9,en.r_no from enx_tab en " +
                                    "inner join master_setting ch on en.col1=ch.master_id and ch.type='MR0' and find_in_set(ch.client_unit_id,en.client_unit_id)=1 " +
                                    "where (en.client_id||en.client_unit_id||en.vch_num||to_char(en.vch_date, 'yyyymmdd')|| en.type) = '" + URL + "'";
                                dt = sgen.getdata(userCode, mq1);
                                if (dt.Rows.Count > 0)
                                {
                                    model[0].LTM1 = new List<Tmodelmain>();
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        model[0].LTM1.Add(new Tmodelmain
                                        {
                                            Col13 = dr["col3"].ToString(),//chrgrtid
                                            Col14 = dr["r_no"].ToString(),//rno
                                            Col15 = dr["col1"].ToString(),//chrgid
                                            Col16 = dr["col2"].ToString(),//chrgamt
                                            Col17 = dr["col4"].ToString(),//igstrt
                                            Col18 = dr["col5"].ToString(),//igstamt
                                            Col19 = dr["col6"].ToString(),//cgstrt
                                            Col20 = dr["col7"].ToString(),//cgstamt
                                            Col21 = dr["col8"].ToString(),//sgstrt
                                            Col22 = dr["col9"].ToString(),//sgstamt
                                            Col23 = dr["chrgname"].ToString(),//chrgname
                                        });
                                    }
                                }
                            }
                            break;
                    }
                    break;
                #endregion
                #region vd_dis
                case "vd_dis":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select (p.client_id||p.client_unit_id||p.vch_num||to_char(p.vch_date,'yyyymmdd')||p.type) fstr,p.vch_num," +
                            "to_char(p.vch_date,'" + sgen.Getsqldateformat() + "') vch_date,p.acode Vendor_Code,p.price_term Vehicle_No,p.tmode Driver_Name," +
                            "p.tname Driver_Contact,p.indno Billno,to_char(p.inddate,'" + sgen.Getsqldateformat() + "') Billdate,p.col1 Dispatch_No," +
                            "to_char(p.date1,'" + sgen.Getsqldateformat() + "') Dispatch_Date,p.icode,p.iname,p.cpartno PartNo,p.uom UOM,p.qtyord,p.qtyind,p.qtybal," +
                            "p.irate,p.iamount,p.iremark,p.col2 pono,to_char(p.date2,'" + sgen.Getsqldateformat() + "') podate,p.client_id,p.client_unit_id,p.type," +
                            "p.ent_by,to_char(p.ent_date,'" + sgen.Getsqldateformat() + "') ent_date,p.edit_by,to_char(p.edit_date,'" + sgen.Getsqldateformat() + "') edit_date " +
                            "from purchases p where (p.client_id||p.client_unit_id||p.vch_num||to_char(p.vch_date,'yyyymmdd')||p.type)='" + URL + "'";
                            dtp = sgen.getdata(userCode, mq);
                            if (dtp.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col1 = dtp.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dtp.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dtp.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dtp.Rows[0]["ent_date"].ToString();
                                model[0].Col5 = dtp.Rows[0]["edit_by"].ToString();
                                model[0].Col6 = dtp.Rows[0]["edit_date"].ToString();
                                model[0].Col8 = "(client_id||client_unit_id||vch_num||to_char(vch_date, 'yyyymmdd')|| type) = '" + URL + "'";
                                model[0].Col9 = model[0].Col9;
                                model[0].Col10 = model[0].Col10;
                                model[0].Col11 = model[0].Col11;
                                model[0].Col12 = model[0].Col12;
                                model[0].Col13 = "Update";
                                model[0].Col14 = model[0].Col14;
                                model[0].Col15 = model[0].Col15;
                                model[0].Col16 = dtp.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dtp.Rows[0]["vch_date"].ToString();
                                model[0].Col18 = dtp.Rows[0]["Vehicle_No"].ToString();
                                model[0].Col19 = dtp.Rows[0]["Driver_Name"].ToString();
                                model[0].Col20 = dtp.Rows[0]["Driver_Contact"].ToString();
                                model[0].Col21 = dtp.Rows[0]["Billno"].ToString();
                                model[0].Col22 = dtp.Rows[0]["Billdate"].ToString();
                                model[0].Col23 = dtp.Rows[0]["Dispatch_No"].ToString();
                                model[0].Col24 = dtp.Rows[0]["Dispatch_Date"].ToString();
                                model[0].Col30 = dtp.Rows[0]["Vendor_Code"].ToString();

                                dtt = sgen.getdata(userCode, "select '0' SNo ,'-' Icode,'-' IName,'-' PartNo,'-' UOM,'0' Order_Qty,'0' Dispatch_Qty,'0' Bal_Qty,'0' Irate," +
                                    "'0' Iamount,'-' Iremark,'0' Pono,'-' Podate from dual");
                                model[0].dt1 = dtt;
                                dtt = model[0].dt1.Clone();
                                sgen.SetSession(MyGuid, "dtvd_dis", dtt);
                                for (int i = 0; i < dtt.Rows.Count; i++)
                                {
                                    DataRow dr = dtt.NewRow();
                                    dr["SNo"] = i + 1;
                                    dr["Icode"] = dtp.Rows[i]["icode"].ToString();
                                    dr["IName"] = dtp.Rows[i]["iname"].ToString();
                                    dr["PARTNO"] = dtp.Rows[i]["PartNo"].ToString();
                                    dr["UOM"] = dtp.Rows[i]["UOM"].ToString();
                                    dr["Po_Qty"] = dtp.Rows[i]["qtyord"].ToString();
                                    dr["Bal_Qty"] = dtp.Rows[i]["qtybal"].ToString();
                                    dr["Dispatch_Qty"] = dtp.Rows[i]["qtyind"].ToString();
                                    dr["Iremark"] = dtp.Rows[i]["iremark"].ToString();
                                    dr["Pono"] = dtp.Rows[i]["pono"].ToString();
                                    dr["Podate"] = dtp.Rows[i]["podate"].ToString();
                                    dtt.Rows.Add(dr);
                                }
                                dtt.AcceptChanges();
                                model[0].dt1 = dtt;
                            }
                            break;
                        case "ITEM":
                            //mq = "select (it.icode||to_char(it.vch_date,'yyyymmdd')||it.type) fstr,pc.icode Icode,it.iname Iname,it.cpartno Partno,um.master_name UOM," +
                            //   "from item it " +
                            //   "inner join master_setting um on um.master_id=it.uom and um.type='UMM' and find_in_set(um.client_unit_id,'" + unitid_mst + "')=1 " +
                            //   "where (it.icode||to_char(it.vch_date,'yyyymmdd')||it.type||pc.ind_no||to_char(pc.vch_date,'yyyymmdd')) in ('" + URL + "')";

                            mq = "select (p.client_id || p.client_unit_id || p.vch_num || to_char(p.vch_date, 'yyyymmdd') || p.type || p.icode) fstr,p.vch_num pono," +
                            "to_char(p.vch_date,'" + sgen.Getsqldateformat() + "') podate,'0' AS qtybal,p.icode,p.iname,p.cpartno partno,p.uom uom,p.hsno HSN," +
                            "p.qtystk Stock_Qty,p.qtyind,p.qtyord,to_char(p.dlv_date,'" + sgen.Getsqldateformat() + "') Delivery_Date," +
                            "p.indno IndentNo,to_char(p.inddate,'" + sgen.Getsqldateformat() + "') Indent_Date from purchases p " +
                            "where (p.client_id || p.client_unit_id || p.vch_num || to_char(p.vch_date, 'yyyymmdd') || p.type || p.icode) in ('" + URL + "')";

                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                if (model[0].dt1.Rows[0]["Icode"].ToString().Trim().Equals("-")) { model[0].dt1.Rows.RemoveAt(0); }
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["SNo"] = i + 1;
                                    dr["Icode"] = dt.Rows[i]["icode"].ToString();
                                    dr["IName"] = dt.Rows[i]["iname"].ToString();
                                    dr["PARTNO"] = dt.Rows[i]["partno"].ToString();
                                    dr["UOM"] = dt.Rows[i]["uom"].ToString();
                                    dr["Po_Qty"] = dt.Rows[i]["qtyord"].ToString();
                                    dr["Bal_Qty"] = dt.Rows[i]["qtybal"].ToString();
                                    dr["Dispatch_Qty"] = dt.Rows[i]["qtyind"].ToString();
                                    dr["Pono"] = dt.Rows[i]["pono"].ToString();
                                    dr["Podate"] = dt.Rows[i]["podate"].ToString();
                                    model[0].dt1.Rows.Add(dr);
                                }
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
            FillMst(Myguid); where = "";
            sgen.SetSession(MyGuid, "btnval", btnval);
            //DateTime year_from = DateTime.ParseExact(year_from.ToString(), sgen.Getdateformat(), CultureInfo.InvariantCulture);
            //DateTime year_to = DateTime.ParseExact(year_to, sgen.Getdateformat(), CultureInfo.InvariantCulture);
            switch (viewname.ToLower())
            {
                #region vendor_detail
                //case "vendor_detail":
                //    if (sgen.GetSession(MyGuid, "VD_TYPEMST") != null)
                //    {
                //        type = sgen.GetSession(MyGuid, "VD_TYPEMST").ToString().Trim();
                //        switch (btnval.ToUpper())
                //        {
                //            case "EDIT":
                //            case "VIEW":
                //                //cmd = "select (a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr,a.c_name as Name,a.addr as Address,a.pincode as Pincode,a.isgstr IsGst," +
                //                //    "a.c_gstin as GSTIN,(case when a.tor='C' then 'Composition' when a.tor='R' then 'Regular' end) GstType,a.type_desc as Search_text,a.cpname as PerName,a.cpcont as PerContact," +
                //                //    "a.cpaltcont as PerAltContact,a.cpemail as PerEmail,a.cpaddr as PerAddr,a.cpdesig as PerDesig from clients_mst a where a.type in ('" + type + "','DD" + type + "') and " +
                //                //    "a.client_id='" + clientid_mst + "' and a.client_unit_id='" + unitid_mst + "'";
                //                cmd = "select (a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr," +
                //                    "a.c_name as Name,a.pincode as Pincode,a.isgstr IsGst,a.c_gstin as GSTIN,a.type_desc as Search_text," +
                //                    "a.cpname as PerName,a.cpcont as PerContact,a.cpaltcont as PerAltContact,a.cpemail as PerEmail,a.cpdesig " +
                //                    "as PerDesig from clients_mst a where find_in_set(a.client_unit_id,'" + unitid_mst + "')=1 and a.type in ('" + type + "','DD" + type + "')";
                //                break;
                //            case "LSR":
                //                cmd = "";
                //                break;
                //        }
                //    }
                //    break;
                #endregion
                #region indent_req
                case "indent_req":
                    switch (btnval.ToUpper())
                    {
                        case "COPYIND":
                        case "EDIT":
                        case "PRINT":
                            cmd = "select (i.client_id||i.client_unit_id||i.vch_num||to_char(i.vch_date,'yyyymmdd')||i.type) as fstr,i.vch_num as indent_no," +
                                "to_char(convert_tz(i.vch_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Indent_date," +
                                "i.reqby Requested_By, i.icode as ItemCode,i.iname as ItemName,i.cpartno PartNo, i.qtyord as QtyReq,i.qtystk as Qty_In_Stock," +
                                "i.iamount as Exp_value from purchases i " +
                                " where i.client_unit_id = '" + unitid_mst + "' and i.type = '66' and to_date(to_char(i.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "')";
                            break;
                        case "VIEW":
                            cmd = "select (i.client_id||i.client_unit_id||i.vch_num||to_char(i.vch_date,'yyyymmdd')||i.type) as fstr,i.vch_num as indent_no," +
                                "to_char(convert_tz(i.vch_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Indent_date," +
                                "to_char(i.dlv_date,'" + sgen.Getsqldateformat() + "') as Required_Date,d.master_name as Department,dg.master_name as Designation,i.reqby Requested_By," +
                                "mg.master_name ItemGrp, sg.iname ItemSubgrp, i.icode as ItemCode,i.iname as ItemName,i.cpartno PartNo, i.qtyord as QtyReq,i.qtystk as Qty_In_Stock," +
                                "i.iamount as Exp_value from purchases i " +
                                "inner join master_setting d on d.master_id = i.deptno and d.type = 'MDP' and find_in_set(d.client_unit_id,i.client_unit_id)=1 " +
                                "inner join master_setting dg on dg.master_id = i.desig and dg.type = 'MDG' and find_in_set(dg.client_unit_id,i.client_unit_id)=1 " +
                                "inner join master_setting mg on mg.master_id = substr(i.icode, 1, 3) and mg.type = 'KIG' and find_in_set(mg.client_unit_id, i.client_unit_id)=1 " +
                                "inner join item sg on sg.icode = substr(i.icode, 1, 5) and sg.type = 'SG' and find_in_set(sg.client_unit_id, i.client_unit_id)=1" +
                                " where i.client_unit_id = '" + unitid_mst + "' and i.type = '66' and to_date(to_char(i.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "')";
                            break;
                        case "INDENT":
                            cmd = @"select (i.client_id||i.client_unit_id||i.vch_num||to_char(i.vch_date,'yyyymmdd')||i.type) as fstr,i.vch_num as indent_no,
to_char(convert_tz(i.date1, 'UTC', '" + sgen.Gettimezone() + "'), " + "'" + sgen.Getsqldateformat() + "') as Indent_date,d.master_name as Dept_name,dg.master_name as desig," +
"i.icode as Icode,i.iname as Item_Name,iqty_chl as qty_req,qty_bal as qty_store,iamount as Exp_value,(b.First_name|| ' '|| nvl(b.middle_name, '')|| ' '|| b.last_name) as Ent_By," +
"to_char(convert_tz(i.ent_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Ent_Date from itransaction i  " +
"inner join master_setting d on d.master_id=i.department and d.type='MDP' and find_in_set(d.client_unit_id,i.client_unit_id)=1 " +
"inner join master_setting dg on  dg.master_id  = i.designation and dg.type='MDG' and find_in_set(dg.client_unit_id,i.client_unit_id)=1 " +
"inner join user_details b on i.ent_by = b.vch_num and b.type = 'CPR' where i.client_unit_id='" + unitid_mst + "' and i.type='AIR' and to_date(to_char(i.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "')";
                            break;
                        case "ITEM":
                            where = "";
                            if (param1 != "") where = " and it.icode not in ('" + param1.Trim().TrimEnd(',') + "')";
                            //cmd = "select (it.client_id||it.client_unit_id||it.icode||to_char(it.vch_date,'yyyymmdd')||it.type) fstr,it.icode as Icode,it.iname as Iname,it.cpartno Partno," +
                            //    "um.master_name as UOM,hsn.master_name as hsn,hsn.group_name taxrate from item it " +
                            //    "inner join master_setting um on um.master_id=it.uom and um.type='UMM' and find_in_set(um.client_id,'" + clientid_mst + "')=1 and find_in_set(um.client_unit_id,'" + unitid_mst + "')=1 " +
                            //    "inner join master_setting hsn on hsn.master_id=it.hsno and hsn.type='HSN' and find_in_set(hsn.client_id,'" + clientid_mst + "')=1 and find_in_set(hsn.client_unit_id,'" + unitid_mst + "')=1 " +
                            //    "where it.type='IT' and it.client_id='" + clientid_mst + "' and it.client_unit_id='" + unitid_mst + "'" + where + "";
                            //  cmd = "select distinct (it.client_id||it.client_unit_id||it.icode||to_char(it.vch_date,'yyyymmdd')||it.type) fstr,it.icode as Icode,it.iname as Iname,it.cpartno Partno, um.master_name as UOM,hsn.master_name as hsn,hsn.group_name taxrate, nvl(pc.ind_qty, '0') ind_qty,nvl(pc.Bal_qty, '0') ind_Bal_qty,nvl(b.po_qty, '0') po_qty,nvl(b.Bal_qty, '0') po_Bal_qty from item it left join(select a.icode, a.client_id, a.client_unit_id, a.ind_qty, sum(a.po_qty) as used,(a.ind_qty - nvl(sum(a.po_qty), '0')) as Bal_qty  from(select nd.icode, nd.client_id, nd.client_unit_id, nvl(nd.qtyord, '0') as ind_qty, nvl(b.qtyord, '0') as po_qty from purchases nd left join purchases b on b.icode = nd.icode and b.type not in ('66') and b.pur_type = '001' and nd.client_id = b.client_id and nd.client_unit_id = b.client_unit_id where nd.type = '66' and nd.client_id = '" + clientid_mst + "' and nd.client_unit_id = " + unitid_mst + " union all select nd.icode, nd.client_id, nd.client_unit_id, nvl(nd.qtyord, '0') as ind_qty, nvl(b.col11, '0') as close_qty from purchases nd left join enx_tab b on b.col7 = nd.icode and b.type = 'CPI' and nd.client_id = b.client_id and nd.client_unit_id = b.client_unit_id where nd.type = '66' and nd.client_id = '" + clientid_mst + "' and nd.client_unit_id = " + unitid_mst + ") a group by(a.icode, a.client_id, a.client_unit_id, a.ind_qty)) pc on pc.icode = it.icode and pc.client_id = it.client_id and pc.client_unit_id = it.client_unit_id left join(select a.icode, a.client_id, a.client_unit_id , a.qtyord po_qty, (a.qtyord -nvl(sum(a.inv_qty), '0')) as bal_qty from(select po.icode, po.client_id, po.client_unit_id, po.qtyord, nvl(iv.qtyrcvd, '0') as inv_qty from purchases po left join itransaction iv on iv.icode = po.icode and iv.client_id = po.client_id and iv.type = '02' and iv.client_unit_id = po.client_unit_id where po.type in ('50', '54', '52') and po.client_id = '" + clientid_mst + "' and po.client_unit_id = '" + unitid_mst + "' union all select po.icode, po.client_id, po.client_unit_id, po.qtyord, nvl(to_number(iv.col11), '0') as close_qty from purchases po left join enx_tab iv on iv.col7 = po.icode and iv.client_id = po.client_id and iv.type = 'POR' and iv.client_unit_id = po.client_unit_id where po.type in ('50', '54', '52') and po.client_id = '" + clientid_mst + "' and po.client_unit_id = '" + unitid_mst + "') a group by(a.icode, a.client_id, a.client_unit_id, a.qtyord)) b on b.icode = it.icode and b.client_id = it.client_id and b.client_unit_id = it.client_unit_id inner join master_setting um on um.master_id = it.uom and um.type = 'UMM' and find_in_set(um.client_id,'" + clientid_mst + "')= 1 and find_in_set(um.client_unit_id, '" + unitid_mst + "') = 1 inner join master_setting hsn on hsn.master_id = it.hsno and hsn.type = 'HSN' and find_in_set(hsn.client_id,'" + clientid_mst + "')= 1 and find_in_set(hsn.client_unit_id, '" + unitid_mst + "') = 1 where it.type = 'IT' and it.client_id = '" + clientid_mst + "' and it.client_unit_id = '" + unitid_mst + "' " + where + "";

                            //cmd = "select (it.icode||to_char(it.vch_date,'yyyymmdd')||it.type) fstr,it.icode as Icode,it.iname as Iname," +
                            //    "it.cpartno Partno, um.master_name as UOM,hsn.master_name as hsn,hsn.group_name taxrate ,nvl(pc.ind_bal_qty,'0')" +
                            //    " ind_bal_qty,nvl(pc.po_bal_qty,'0') po_bal_qty from item it left join (select aa.icode,aa.client_id,aa.client_unit_id" +
                            //    ",sum(aa.Bal_qty) as ind_bal_qty,sum(aa.po_bal_qty) as po_bal_qty from (select icode,to_number(ind_qty) ind_qty" +
                            //    ",to_number(Bal_qty) Bal_qty,0 po_qty,0 po_bal_qty,client_id,client_unit_id from (select a.icode, a.client_id," +
                            //    " a.client_unit_id,nvl(a.ind_qty,'0') as ind_qty,(a.ind_qty - nvl(sum(a.po_qty), '0')) as Bal_qty " +
                            //    "from(select nd.icode, nd.client_id, nd.client_unit_id, nvl(nd.qtyord, '0') as ind_qty, nvl(b.qtyord, '0') as" +
                            //    " po_qty from purchases nd left join purchases b on b.icode = nd.icode and b.type not in ('66') and b.pur_type = '001' " +
                            //    "and nd.client_unit_id = b.client_unit_id where nd.client_unit_id = '" + unitid_mst + "' and nd.type = '66' union all select nd.icode, nd.client_id," +
                            //    "nd.client_unit_id, nvl(nd.qtyord, '0') as ind_qty, nvl(b.col11, '0') as close_qty from purchases nd" +
                            //    " left join enx_tab b on b.col7 = nd.icode and b.type = 'CPI' and nd.client_unit_id" +
                            //    " = b.client_unit_id where nd.client_unit_id = '" + unitid_mst + "' and nd.type = '66') a group by(a.icode, a.client_id, a.client_unit_id, a.ind_qty))" +
                            //    " a union all select icode,0 ind_qty,0 Bal_qty,to_number(po_qty),to_number(bal_qty),client_id,client_unit_id " +
                            //    "from (select a.icode, a.client_id, a.client_unit_id , a.qtyord po_qty,(a.qtyord -nvl(sum(a.inv_qty), '0')) as " +
                            //    "bal_qty from(select po.icode, po.client_id, po.client_unit_id, po.qtyord,nvl(iv.qtyrcvd, '0') as inv_qty from" +
                            //    " purchases po left join itransaction iv on iv.icode = po.icode and iv.type = '02'" +
                            //    " and iv.client_unit_id = po.client_unit_id where po.client_unit_id = '" + unitid_mst + "' and po.type in ('50', '54', '52') union all select" +
                            //    " po.icode, po.client_id, po.client_unit_id,po.qtyord, nvl(to_number(iv.col11), '0') as close_qty from" +
                            //    " purchases po left join enx_tab iv on iv.col7 = po.icode and iv.type = 'POR'" +
                            //    " and iv.client_unit_id = po.client_unit_id where po.client_unit_id = '" + unitid_mst + "' and po.type in ('50', '54', '52')) a " +
                            //    "group by(a.icode, a.client_id,a.client_unit_id, a.qtyord)) b) aa group by(aa.icode,aa.client_id,aa.client_unit_id)) " +
                            //    "pc on pc.icode=it.icode and find_in_set(pc.client_unit_id,it.client_unit_id)=1 " +
                            //    "inner join master_setting um on um.master_id = it.uom and um.type = 'UMM' and " +
                            //    "find_in_set(um.client_unit_id,'" + unitid_mst + "') = 1 " +
                            //    "inner join master_setting hsn on hsn.master_id = it.hsno and hsn.type = 'HSN' and " +
                            //    "find_in_set(hsn.client_unit_id, '" + unitid_mst + "') = 1 " +
                            //    "where find_in_set(it.client_unit_id, '" + unitid_mst + "')=1 and it.type = 'IT'" + where + "";

                            // kashish this query removes client id and HSN no. and tax rate but itwm not matched = 19.04.2020

                            //cmd = "select (it.icode||to_char(it.vch_date,'yyyymmdd')||it.type) fstr,it.icode as Icode,it.iname as Iname," +
                            //  "it.cpartno Partno, um.master_name as UOM,nvl(pc.ind_bal_qty,'0')" +
                            //  " ind_bal_qty,nvl(pc.po_bal_qty,'0') po_bal_qty from item it left join (select aa.icode,aa.client_unit_id" +
                            //  ",sum(aa.Bal_qty) as ind_bal_qty,sum(aa.po_bal_qty) as po_bal_qty from (select icode,to_number(ind_qty) ind_qty" +
                            //  ",to_number(Bal_qty) Bal_qty,0 po_qty,0 po_bal_qty,client_unit_id from (select a.icode," +
                            //  " a.client_unit_id,nvl(a.ind_qty,'0') as ind_qty,(a.ind_qty - nvl(sum(a.po_qty), '0')) as Bal_qty " +
                            //  "from(select nd.icode, nd.client_unit_id, nvl(nd.qtyord, '0') as ind_qty, nvl(b.qtyord, '0') as" +
                            //  " po_qty from purchases nd left join purchases b on b.icode = nd.icode and b.type not in ('66') and b.pur_type = '001' " +
                            //  "and nd.client_unit_id = b.client_unit_id where nd.client_unit_id = '" + unitid_mst + "' and nd.type = '66' union all select nd.icode," +
                            //  "nd.client_unit_id, nvl(nd.qtyord, '0') as ind_qty, nvl(b.col11, '0') as close_qty from purchases nd" +
                            //  " left join enx_tab b on b.col7 = nd.icode and b.type = 'CPI' and nd.client_unit_id" +
                            //  " = b.client_unit_id where nd.client_unit_id = '" + unitid_mst + "' and nd.type = '66') a group by(a.icode, a.client_unit_id, a.ind_qty))" +
                            //  " a union all select icode,0 ind_qty,0 Bal_qty,to_number(po_qty),to_number(bal_qty),client_unit_id " +
                            //  "from (select a.icode, a.client_unit_id , a.qtyord po_qty,(a.qtyord -nvl(sum(a.inv_qty), '0')) as " +
                            //  "bal_qty from(select po.icode, po.client_unit_id, po.qtyord,nvl(iv.qtyrcvd, '0') as inv_qty from" +
                            //  " purchases po left join itransaction iv on iv.icode = po.icode and iv.type = '02'" +
                            //  " and iv.client_unit_id = po.client_unit_id where po.client_unit_id = '" + unitid_mst + "' and po.type in ('50', '54', '52') union all select" +
                            //  " po.icode, po.client_unit_id,po.qtyord, nvl(to_number(iv.col11), '0') as close_qty from" +
                            //  " purchases po left join enx_tab iv on iv.col7 = po.icode and iv.type = 'POR'" +
                            //  " and iv.client_unit_id = po.client_unit_id where po.client_unit_id = '" + unitid_mst + "' and po.type in ('50', '54', '52')) a " +
                            //  "group by(a.icode, a.client_unit_id, a.qtyord)) b) aa group by(aa.icode,aa.client_unit_id)) " +
                            //  "pc on pc.icode=it.icode and find_in_set(pc.client_unit_id,it.client_unit_id)=1 " +
                            //  "inner join master_setting um on um.master_id = it.uom and um.type = 'UMM' and " +
                            //  "find_in_set(um.client_unit_id,'" + unitid_mst + "') = 1 " +
                            //  "where find_in_set(it.client_unit_id, '" + unitid_mst + "')=1 and it.type = 'IT'" + where + "";

                            //cmd = "select (it.icode||to_char(it.vch_date,'yyyymmdd')||it.type) fstr,it.icode as Icode,it.iname as Iname,it.cpartno Partno, um.master_name as UOM," +
                            //    "nvl(pc.ind_bal_qty, 0) ind_bal_qty,nvl(pc.po_bal_qty, 0) po_bal_qty from item it " +
                            //    "left join(select aa.icode, aa.client_unit_id, sum(aa.Bal_qty) as ind_bal_qty,sum(aa.po_bal_qty) as po_bal_qty from " +
                            //    "(select icode, to_number(ind_qty) ind_qty, to_number(Bal_qty) Bal_qty, 0 po_qty, 0 po_bal_qty, client_unit_id from " +
                            //    "(select a.icode, a.client_unit_id, sum(a.ind_qty) as ind_qty, (sum(a.ind_qty) - sum(a.po_qty)) as Bal_qty from " +
                            //    "(select nd.icode, nd.client_unit_id, nvl(to_number(nd.qtyord), 0) as ind_qty, nvl(to_number(b.qtyord), 0) as po_qty from purchases nd " +
                            //    "left join purchases b on b.icode = nd.icode and b.type not in ('66') and b.pur_type = '001' and nd.client_unit_id = b.client_unit_id " +
                            //    "where nd.client_unit_id = '"+unitid_mst+"' and nd.type = '66' " +
                            //    "union all " +
                            //    "select nd.icode, nd.client_unit_id, 0 as ind_qty, nvl(to_number(b.col11), 0) as close_qty from purchases nd " +
                            //    "left join enx_tab b on b.col7 = nd.icode and b.type = 'CPI' and nd.client_unit_id = b.client_unit_id " +
                            //    "where nd.client_unit_id = '"+unitid_mst+"' and nd.type = '66') a group by a.icode, a.client_unit_id) " +
                            //    "union all " +
                            //    "select icode, 0 ind_qty, 0 Bal_qty, to_number(po_qty) po_qty, to_number(bal_qty) po_bal_qty, client_unit_id from " +
                            //    "(select a.icode, a.client_unit_id, sum(a.qtyord) po_qty, (sum(a.qtyord) - sum(a.inv_qty)) as bal_qty from " +
                            //    "(select po.icode, po.client_unit_id, to_number(po.qtyord) qtyord, nvl(to_number(iv.qtyrcvd), 0) as inv_qty from purchases po " +
                            //    "left join itransaction iv on iv.icode = po.icode and iv.type = '02' and iv.client_unit_id = po.client_unit_id " +
                            //    "where po.client_unit_id = '"+unitid_mst+"' and po.type in ('50', '54', '52') " +
                            //    "union all " +
                            //    "select po.icode, po.client_unit_id, 0 qtyord, nvl(to_number(iv.col11), 0) as close_qty from purchases po " +
                            //    "left join enx_tab iv on iv.col7 = po.icode and iv.type = 'POR' and iv.client_unit_id = po.client_unit_id " +
                            //    "where po.client_unit_id = '"+unitid_mst+"' and po.type in ('50', '54', '52')) a group by a.icode, a.client_unit_id) b) aa " +
                            //    "group by aa.icode,aa.client_unit_id) pc on pc.icode = it.icode and find_in_set(pc.client_unit_id, it.client_unit_id)= 1 " +
                            //    "inner join master_setting um on um.master_id = it.uom and um.type = 'UMM' and find_in_set(um.client_unit_id,'"+unitid_mst+"') = 1 " +
                            //    "where find_in_set(it.client_unit_id, '"+unitid_mst+"')= 1 and it.type = 'IT' " + where + "";

                            // remove pending qty of PO and Indent

                            //cmd = "select (it.icode||to_char(it.vch_date,'yyyymmdd')||it.type) fstr,it.icode as Icode,it.iname as Iname,it.cpartno Partno, um.master_name UOM," +
                            //    "nvl(pc.ind_bal_qty, 0) ind_bal_qty,nvl(pc.po_bal_qty, 0) po_bal_qty from item it " +
                            //    "left join(select aa.icode, aa.client_unit_id, sum(aa.Bal_qty) ind_bal_qty, sum(aa.po_bal_qty) po_bal_qty from " +
                            //    "(select icode, client_unit_id, ind_qty, Bal_qty,0 po_qty,0 po_bal_qty from " +
                            //    "(select a.icode, a.client_unit_id, sum(a.ind_qty) ind_qty, (sum(a.ind_qty) -sum(a.po_qty)) Bal_qty from " +
                            //    "(select icode, client_unit_id, nvl(to_number(qtyord), 0) ind_qty,0 po_qty from purchases " +
                            //    "where client_unit_id = '" + unitid_mst + "' and type = '66' " +
                            //    "union all " +
                            //    "select icode, client_unit_id,0 ind_qty,nvl(to_number(qtyord), 0) po_qty from purchases " +
                            //    "where client_unit_id = '" + unitid_mst + "' and type<>'66' and pur_type = '001' " +
                            //    "union all " +
                            //    "select col7 icode,client_unit_id,0 ind_qty,nvl(to_number(col11), 0) po_qty from enx_tab " +
                            //    "where client_unit_id = '" + unitid_mst + "' and type = 'CPI') a group by a.icode, a.client_unit_id)  " +
                            //    "union all " +
                            //    "select icode, client_unit_id,0 ind_qty,0 Bal_qty,po_qty,bal_qty po_bal_qty from " +
                            //    "(select a.icode, a.client_unit_id, sum(a.qtyord) po_qty, (sum(a.qtyord) - sum(a.inv_qty)) bal_qty from " +
                            //    "(select icode, client_unit_id, nvl(to_number(qtyord), 0) qtyord, 0 inv_qty from purchases " +
                            //    "where client_unit_id = '" + unitid_mst + "' and type in ('50', '54', '52') " +
                            //    "union all " +
                            //    "select icode, client_unit_id, 0 qtyord, nvl(to_number(qtyrcvd), 0) inv_qty from itransaction " +
                            //    "where client_unit_id = '" + unitid_mst + "' and type = '02' " +
                            //    "union all " +
                            //    "select col7 icode, client_unit_id, 0 qtyord, nvl(to_number(col11), 0) inv_qty from enx_tab " +
                            //    "where client_unit_id = '" + unitid_mst + "' and type = 'POR') a group by a.icode, a.client_unit_id) b) aa " +
                            //    "group by aa.icode,aa.client_unit_id) pc on pc.icode = it.icode and find_in_set(pc.client_unit_id, it.client_unit_id)= 1 " +
                            //    "inner join master_setting um on um.master_id = it.uom and um.type = 'UMM' and find_in_set(um.client_unit_id,'" + unitid_mst + "') = 1 " +
                            //    "where find_in_set(it.client_unit_id, '" + unitid_mst + "')= 1 and it.type = 'IT' " + where + "";

                            cmd = "select (it.icode||to_char(it.vch_date,'yyyymmdd')||it.type) fstr,it.icode as Icode,it.iname as Iname,it.cpartno Partno from item it " +
                                  "where find_in_set(it.client_unit_id, '" + unitid_mst + "')= 1 and it.type = 'IT' and((substr(it.icode, 1, 1) <> '9') or(substr(it.icode, 1, 1) = '9' and it.dbuy = 'Y')) " + where + "";

                            break;
                    }
                    break;
                #endregion
                #region store_issue
                case "store_issue":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            cmd = @"select (i.client_id||i.client_unit_id||i.vch_num||to_char(i.vch_date,'yyyymmdd')||i.type) as fstr,i.vch_num as Doc_no,
to_char(convert_tz(i.date1, 'UTC', '" + sgen.Gettimezone() + "'), " + "'" + sgen.Getsqldateformat() + "') as Doc_date,i.icode as Icode,i.iname as Item_Name," +
"i.iqty_chl as Qty_in_stock,i.iqty_in as qty_req,i.iamount as Exp_value,i.iqty_out as qty_issue from itransaction i " +
"where i.type='STI' and i.client_unit_id='" + unitid_mst + "' and to_date(to_char(i,vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "')";
                            break;
                        case "VIEW":
                            cmd = @"select (i.client_id||i.client_unit_id||i.vch_num||to_char(i.vch_date,'yyyymmdd')||i.type) as fstr,i.vch_num as Doc_no,
to_char(convert_tz(i.date1, 'UTC', '" + sgen.Gettimezone() + "'), " + "'" + sgen.Getsqldateformat() + "') as Doc_date,d.master_name as Dept_name,dg.master_name as desig,i.icode as Icode,i.iname as Item_Name," +
"i.iqty_chl as Qty_in_stock,i.iqty_in as qty_req,i.iamount as Exp_value,i.iqty_out as qty_issue,(b.First_name|| ' '|| nvl(b.middle_name|| '')|| ' '|| b.last_name) as Ent_By," +
"to_char(convert_tz(i.ent_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Ent_Date from itransaction i " +
"inner join master_setting d on d.master_id=i.department and d.type='MDP' and find_in_set(d.client_unit_id,i.client_unit_id)=1 " +
"inner join master_setting dg on  dg.master_id  = i.designation and dg.type='MDG' and find_in_set(dg.client_unit_id,i.client_unit_id)=1 " +
"inner join user_details b on i.ent_by = b.vch_num and b.type = 'CPR' where i.type='STI' and i.client_unit_id='" + unitid_mst + "' and to_date(to_char(i,vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "')";
                            break;
                    }
                    break;
                #endregion
                #region po_withindent
                case "po_withindent":
                    switch (btnval.ToUpper())
                    {
                        case "VIEW":
                            cmd = "select (p.client_id||p.client_unit_id||p.vch_num||to_char(p.vch_date,'yyyymmdd')||p.type) as fstr,p.vch_num PO_Number," +
                           "to_char(p.vch_date,'" + sgen.Getsqldateformat() + "') PO_Date," +
                           "c.c_name as Party_Name,c.c_gstin as Party_Gstin,c.addr Address,NVL(sf.c_name,'-') as From_Name," +
                           "NVL(sf.c_gstin,'-') as From_Gstin,nvl(sf.addr,'-') FromAddress,NVL(st.c_name,'-') as To_Name,NVL(st.c_gstin,'-') as To_Gstin,NVL(st.addr,'-') ToAddress," +
                           "p.icode ItemCode,p.iname ItemName,p.cpartno PartNo,p.uom UOM,p.hsno HSN,p.qtystk Stock_Qty,p.qtyind Indent_Qty,p.qtyord Order_Qty,p.irate ItemRate,p.taxrate TaxRate," +
                           "iamount Item_Amt,p.taxamt taxamt,p.discrate Discount_Rate,p.discamt Discount_Amt,p.lineamount LineAmt," +
                           "to_char(p.dlv_date,'" + sgen.Getsqldateformat() + "') Delivery_Date,p.indno IndentNo," +
                           "to_char(p.inddate,'" + sgen.Getsqldateformat() + "') Indent_Date," +
                           "p.basicamt BasicAmt from purchases p " +
                           "inner join clients_mst c on p.acode = c.vch_num and find_in_set(c.client_unit_id,p.client_unit_id)=1 and c.type = 'BCD' and substr(c.vch_num,0,3)='203' " +
                           "left join clients_mst sf on p.shipfrom = sf.vch_num and find_in_set(sf.client_unit_id,p.client_unit_id)=1 and sf.type = 'BCD' and substr(sf.vch_num,0,3)='203' " +
                           "left join clients_mst st on p.shipto = st.vch_num and find_in_set(st.client_unit_id,p.client_unit_id)=1 and st.type = 'BCD' and substr(st.vch_num,0,3)='203' " +
                           "where p.client_unit_id = '" + unitid_mst + "' and p.type in ('50','52','54') and to_date(to_char(p.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "') order by p.vch_num desc";
                            break;
                        case "EDIT":
                        case "PRINT":
                            //cmd = "select concat(p.client_id,p.client_unit_id,p.vch_num,to_char(p.vch_date,'yyyymmdd'),p.type) as fstr,p.vch_num PO_Number," +
                            //    "to_char(convert_tz(p.vch_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') PO_Date," +
                            //    "c.c_name as Party_Name,c.c_gstin as Party_Gstin,c.c_country as Party_Address,pay_term PayTerm,price_term PriceTerm,tmode Transport,ifnull(sf.c_name,'-') as From_Name," +
                            //    "ifnull(sf.c_gstin,'-') as From_Gstin,ifnull(sf.c_country,'-') FromAddress,ifnull(st.c_name,'-') as To_Name,ifnull(st.c_gstin,'-') as To_Gstin,ifnull(st.c_country,'-') ToAddress," +
                            //    "p.pur_type Purchase_Type,p.icode ItemCode,p.iname ItemName,p.cpartno PartNo,p.uom UOM,p.hsno HSN,p.qtystk Stock_Qty,p.qtyind Indent_Qty,p.qtyord Order_Qty,p.irate ItemRate,p.taxrate TaxRate," +
                            //    "iamount Item_Amt,p.taxamt taxamt,p.disctype Discount_Type,p.discrate Discount_Rate,p.discamt Discount_Amt,p.lineamount LineAmt," +
                            //    "to_char(convert_tz(p.dlv_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') Delivery_Date,p.indno IndentNo," +
                            //    "to_char(convert_tz(p.inddate,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') Indent_Date," +
                            //    "p.basicamt BasicAmt,p.gdisc Gross_Discount,p.gfreight GrossFreight,p.insurance Insurance,p.ginstlchrg Installation_Charge,p.gserchrg Service_Charge,p.gamc AMC," +
                            //    "p.gloadchrg Loading_Charge,p.gothrchrg Other_Charge,p.gtaxamt Gross_TamAmt,p.igst IGST,p.sgst SGST,p.cgst CGST,p.gamt GrossAmt from purchases p " +
                            //    "inner join clients_mst c on p.acode = c.vch_num and c.client_id = p.client_id and c.client_unit_id = p.client_unit_id and c.type = 'PVD' " +
                            //    "left join clients_mst sf on p.shipfrom = sf.vch_num and sf.client_id = p.client_id and sf.client_unit_id = p.client_unit_id and sf.type = 'PVD' " +
                            //    "left join clients_mst st on p.shipto = st.vch_num and st.client_id = p.client_id and st.client_unit_id = p.client_unit_id and st.type = 'PVD' " +
                            //    "where p.client_id = '" + clientid_mst + "' and p.client_unit_id = '" + unitid_mst + "' and p.type in ('50','52')";
                            cmd = "select distinct (p.client_id||p.client_unit_id||p.vch_num||to_char(p.vch_date,'yyyymmdd')||p.type) as fstr,p.vch_num PO_Number," +
                            "to_char(p.vch_date,'" + sgen.Getsqldateformat() + "') PO_Date, c.c_name as Party_Name,c.addr Address from purchases p " +
                            "inner join clients_mst c on p.acode = c.vch_num and find_in_set(c.client_unit_id,p.client_unit_id)=1 and c.type = 'BCD' and substr(c.vch_num,0,3)='203' " +
                            "where p.client_unit_id = '" + unitid_mst + "' and p.type in ('50','52','54') and to_date(to_char(p.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "') order by p.vch_num desc";
                            break;
                        case "COPYPO":
                            //cmd = "select (p.client_id||p.client_unit_id||p.vch_num||to_char(p.vch_date,'yyyymmdd')||p.type) as fstr,p.vch_num PO_Number," +
                            //"to_char(p.vch_date,'" + sgen.Getsqldateformat() + "') PO_Date," +
                            //"c.c_name as Party_Name,c.c_gstin as Party_Gstin,c.addr Address,NVL(sf.c_name,'-') as From_Name," +
                            //"NVL(sf.c_gstin,'-') as From_Gstin,nvl(sf.addr,'-') FromAddress,NVL(st.c_name,'-') as To_Name,NVL(st.c_gstin,'-') as To_Gstin,NVL(st.addr,'-') ToAddress," +
                            //"p.icode ItemCode,p.iname ItemName,p.cpartno PartNo,p.uom UOM,p.hsno HSN,p.qtystk Stock_Qty,p.qtyind Indent_Qty,p.qtyord Order_Qty,p.irate ItemRate,p.taxrate TaxRate," +
                            //"iamount Item_Amt,p.taxamt taxamt,p.discrate Discount_Rate,p.discamt Discount_Amt,p.lineamount LineAmt," +
                            //"to_char(p.dlv_date,'" + sgen.Getsqldateformat() + "') Delivery_Date,p.indno IndentNo," +
                            //"to_char(p.inddate,'" + sgen.Getsqldateformat() + "') Indent_Date," +
                            //"p.basicamt BasicAmt from purchases p " +
                            //"inner join clients_mst c on p.acode = c.vch_num and find_in_set(c.client_unit_id,p.client_unit_id)=1 and c.type = 'BCD' and substr(c.vch_num,0,3)='203' " +
                            //"left join clients_mst sf on p.shipfrom = sf.vch_num and find_in_set(sf.client_unit_id,p.client_unit_id)=1 and sf.type = 'BCD' and substr(sf.vch_num,0,3)='203' " +
                            //"left join clients_mst st on p.shipto = st.vch_num and find_in_set(st.client_unit_id,p.client_unit_id)=1 and st.type = 'BCD' and substr(st.vch_num,0,3)='203' " +
                            //"where p.client_unit_id = '" + unitid_mst + "' and p.type = '50' and p.pur_type='002' and to_date(to_char(p.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "')";
                            cmd = "select distinct (p.client_id||p.client_unit_id||p.vch_num||to_char(p.vch_date,'yyyymmdd')||p.type) as fstr,p.vch_num PO_Number," +
                            "to_char(p.vch_date,'" + sgen.Getsqldateformat() + "') PO_Date," +
                            "c.c_name as Party_Name from purchases p " +
                            "inner join clients_mst c on p.acode = c.vch_num and find_in_set(c.client_unit_id,p.client_unit_id)=1 and c.type = 'BCD' and substr(c.vch_num,0,3)='203' " +
                            "where p.client_unit_id = '" + unitid_mst + "' and p.type = '50' and p.pur_type='002' order by p.vch_num desc";
                            break;
                        case "NEW":
                            cmd = "select '001' fstr,'001' master_id,'With Indent' master_name from dual union all select '002' fstr,'002' master_id,'Without Indent' master_name from dual";
                            break;
                        case "POTYPE":
                            cmd = "select (master_id||to_char(vch_date,'yyyymmdd')||type) fstr,master_id as Type,master_name Name " +
                                "from master_setting where find_in_set(client_unit_id,'" + unitid_mst + "')=1 and type='KPO' and substr(master_id,1,2) in ('50','52','54')";
                            break;
                        case "PARTY":
                        case "PARTYFRM":
                        case "PARTYTO":
                            cmd = @"select (a.vch_num|| to_char(a.vch_date,'yyyymmdd')||a.type) as fstr,a.c_name as vendor_Name,a.addr address,
a.c_gstin as GSTIN,a.type_desc as Search_text,a.cpname as Per_Name,(case when a.cpcont='0000000000' then '-' else a.cpcont end) Contact_No from clients_mst a 
where find_in_set(a.client_unit_id,'" + unitid_mst + "')=1 and a.type='BCD' and substr(a.vch_num,0,3)='203'";
                            break;
                        case "ITEM":
                            sgen.SetSession(MyGuid, "Ptype", param2.Trim());
                            if (param2 == "002") // without Indent
                            {
                                if (param1 != "") where = " and it.icode not in ('" + param1.Trim().TrimEnd(',').Replace(",", "','").Trim().ToString() + "')";
                                // UM and tax rate not required
                                cmd = "select (it.icode||to_char(it.vch_date,'yyyymmdd')||it.type) fstr,it.icode as Icode,it.iname," +
                                    "it.cpartno Partno,um.master_name as UOM,hsn.master_name as hsn,hsn.group_name taxrate from item it " +
                                    "inner join master_setting um on um.master_id=it.uom and um.type='UMM' and find_in_set(um.client_unit_id,'" + unitid_mst + "')=1 " +
                                    "inner join master_setting hsn on hsn.master_id=it.hsno and hsn.type='HSN' and find_in_set(hsn.client_unit_id,'" + unitid_mst + "')=1 " +
                                    "where find_in_set(it.client_unit_id,'" + unitid_mst + "')=1 and it.type='IT' and ((substr(it.icode,1,1)<>'9') or (substr(it.icode,1,1)='9' and it.dbuy='Y')) " + where + "";



                            }
                            else if (param2 == "001") // with indent
                            {
                                if (param1 != "") where = " and (it.icode||to_char(it.vch_date,'yyyymmdd')||it.type||pc.ind_no||to_char(pc.vch_date,'yyyymmdd')) not in " +
                                        "('" + param1.Trim().TrimEnd(',').Replace(",", "','").Trim().ToString() + "')";

                                //cmd = "select distinct (it.icode||to_char(it.vch_date,'yyyymmdd')||it.type||pc.ind_no||to_char(pc.vch_date,'yyyymmdd')) fstr" +
                                //   ",pc.ind_no,to_char(pc.vch_date,'" + sgen.Getsqldateformat() + "') as ind_date,pc.icode as Icode,it.iname as Iname,it.cpartno Partno,um.master_name as UOM" +
                                //   ",hsn.master_name as hsn,hsn.group_name taxrate,pc.ind_qty,pc.Bal_qty from item it inner join(select a.ind_no,a.vch_date,a.type," +
                                //   " a.icode,a.client_unit_id,a.ind_qty,sum(a.po_qty) as used,(a.ind_qty - nvl(sum(a.po_qty), '0')) as Bal_qty " +
                                //   "from(select nd.vch_num as ind_no,nd.vch_date,nd.type,nd.icode,nd.client_unit_id,nd.qtyord as ind_qty,nvl(b.qtyord,'0') as po_qty" +
                                //   " from purchases nd left join purchases b on b.icode=nd.icode and b.indno=nd.vch_num and b.type not in ('66') and b.pur_type='001' and " +
                                //   "nd.client_unit_id=b.client_unit_id where nd.client_unit_id='" + unitid_mst + "' and nd.type='66' " +
                                //   "union all select nd.vch_num as ind_no,nd.vch_date,nd.type,nd.icode,nd.client_unit_id,nd.qtyord as ind_qty,nvl(b.col11,'0') as" +
                                //   " close_qty from purchases nd left join enx_tab b on b.col7=nd.icode and b.col12=nd.vch_num and b.type = 'CPI' " +
                                //   " and nd.client_unit_id=b.client_unit_id where nd.client_unit_id='" + unitid_mst + "' and nd.type='66') a " +
                                //   "group by(a.ind_no,a.vch_date,a.type, a.icode,a.client_unit_id,a.ind_qty)) pc on pc.icode=it.icode and " +
                                //   " find_in_set(pc.client_unit_id,it.client_unit_id)=1 and pc.bal_qty > 0 inner join master_setting um on um.master_id=it.uom and um.type='UMM'" +
                                //   " and find_in_set(um.client_unit_id,'" + unitid_mst + "')=1 inner join master_setting hsn on hsn.master_id=it.hsno and hsn.type='HSN' and " +
                                //   "find_in_set(hsn.client_unit_id,'" + unitid_mst + "')=1 where find_in_set(it.client_unit_id,'" + unitid_mst + "')=1 and it.type='IT' " + where + "";

                                cmd = "select distinct (it.icode||to_char(it.vch_date,'yyyymmdd')||it.type||pc.ind_no||to_char(pc.vch_date,'yyyymmdd')) fstr,pc.ind_no," +
                                    "to_char(pc.vch_date, '" + sgen.Getsqldateformat() + "') as ind_date,pc.icode as Icode,it.iname as Iname,it.cpartno Partno, um.master_name UOM, hsn.master_name hsn," +
                                    "hsn.group_name taxrate, pc.ind_qty,pc.Bal_qty from item it " +
                                    "inner join(select a.ind_no, a.vch_date, a.type, a.icode, a.client_unit_id, sum(a.ind_qty) ind_qty, sum(a.po_qty) used, (sum(a.ind_qty) -sum(a.po_qty)) Bal_qty from " +
                                    "(select nd.vch_num ind_no, nd.vch_date, nd.type, nd.icode, nd.client_unit_id, to_number(nd.qtyord) ind_qty, nvl(to_number(b.qtyord),0) po_qty from purchases nd " +
                                    "left join purchases b on b.icode = nd.icode and b.indno = nd.vch_num and b.type not in ('66') and b.pur_type = '001' and nd.client_unit_id = b.client_unit_id " +
                                    "where nd.client_unit_id = '" + unitid_mst + "' and nd.type = '66' " +
                                    "union all " +
                                    "select nd.vch_num ind_no, nd.vch_date,nd.type,nd.icode,nd.client_unit_id,0 ind_qty,nvl(to_number(b.col11), 0) close_qty from purchases nd " +
                                    "left join enx_tab b on b.col7 = nd.icode and b.col12 = nd.vch_num and b.type = 'CPI' and nd.client_unit_id = b.client_unit_id " +
                                    "where nd.client_unit_id = '" + unitid_mst + "' and nd.type = '66') a group by a.ind_no,a.vch_date,a.type,a.icode,a.client_unit_id) pc " +
                                    "on pc.icode = it.icode and find_in_set(pc.client_unit_id, it.client_unit_id)= 1 and pc.bal_qty > 0 " +
                                    "inner join master_setting um on um.master_id = it.uom and um.type = 'UMM' and find_in_set(um.client_unit_id,'" + unitid_mst + "')= 1 " +
                                    "inner join master_setting hsn on hsn.master_id = it.hsno and hsn.type = 'HSN' and find_in_set(hsn.client_unit_id,'" + unitid_mst + "')= 1 " +
                                    "where find_in_set(it.client_unit_id,'" + unitid_mst + "')= 1 and it.type = 'IT' " + where + "";

                                //cmd = "select distinct (it.icode||to_char(it.vch_date,'yyyymmdd')||it.type||pc.ind_no||to_char(pc.vch_date,'yyyymmdd')) fstr" +
                                //    ",pc.ind_no,to_char(pc.vch_date,'dd/mm/yyyy') as ind_date,pc.icode as Icode,it.iname as Iname,it.cpartno Partno,um.master_name as UOM" +
                                //    ",hsn.master_name as hsn,hsn.group_name taxrate,pc.ind_qty,pc.Bal_qty from item it inner join(select a.ind_no,a.vch_date,a.type," +
                                //    " a.icode,a.client_id,a.client_unit_id,a.ind_qty,sum(a.po_qty) as used,(a.ind_qty - nvl(sum(a.po_qty), '0')) as Bal_qty " +
                                //    "from(select nd.vch_num as ind_no,nd.vch_date,nd.type,nd.icode,nd.client_id,nd.client_unit_id,nd.qtyord as ind_qty,nvl(b.qtyord,'0') as po_qty" +
                                //    " from purchases nd left join purchases b on b.icode=nd.icode and b.indno=nd.vch_num and b.type not in ('66') and b.pur_type='001' and " +
                                //    "nd.client_id=b.client_id and nd.client_unit_id=b.client_unit_id where nd.client_id='" + clientid_mst + "' and nd.client_unit_id='" + unitid_mst + "' and nd.type='66' " +
                                //    "union all select nd.vch_num as ind_no,nd.vch_date,nd.type,nd.icode,nd.client_id,nd.client_unit_id,nd.qtyord as ind_qty,nvl(b.col11,'0') as" +
                                //    " close_qty from purchases nd left join enx_tab b on b.col7=nd.icode and b.col12=nd.vch_num and b.type = 'CPI' and nd.client_id=b.client_id" +
                                //    " and nd.client_unit_id=b.client_unit_id where nd.client_id='" + clientid_mst + "' and nd.client_unit_id='" + unitid_mst + "' and nd.type='66') a " +
                                //    "group by(a.ind_no,a.vch_date,a.type, a.icode,a.client_id,a.client_unit_id,a.ind_qty)) pc on pc.icode=it.icode and it.client_id=pc.client_id and " +
                                //    " find_in_set(pc.client_unit_id,it.client_unit_id)=1 and pc.bal_qty > 0 inner join master_setting um on um.master_id=it.uom and um.type='UMM' and find_in_set(um.client_id,'" + clientid_mst + "')=1" +
                                //    " and find_in_set(um.client_unit_id,'" + unitid_mst + "')=1 inner join master_setting hsn on hsn.master_id=it.hsno and hsn.type='HSN' and " +
                                //    "find_in_set(hsn.client_id,'" + clientid_mst + "')=1 and find_in_set(hsn.client_unit_id,'" + unitid_mst + "')=1 where find_in_set(it.client_id,'" + clientid_mst + "')=1 and find_in_set(it.client_unit_id,'" + unitid_mst + "')=1 and it.type='IT' " + where + "";
                                //cmd = "select (it.client_id||it.client_unit_id||it.icode||to_char(it.vch_date,'yyyymmdd')||it.type||ind.ind_no||to_char(ind.ind_date,'yyyymmdd')) fstr,it.icode as Icode," +
                                //    "it.iname as Iname,it.cpartno Partno," +
                                //    "um.master_name as UOM,hsn.master_name as hsn,hsn.group_name taxrate,ind.qtyord,ind.ind_no,to_char(convert_tz(ind.ind_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') ind_date from item it " +
                                //    "inner join master_setting um on um.master_id=it.uom and um.type='UMM' and find_in_set(um.client_id,'" + clientid_mst + "')=1 and find_in_set(um.client_unit_id,'" + unitid_mst + "')=1 " +
                                //    "inner join master_setting hsn on hsn.master_id=it.hsno and hsn.type='HSN' and find_in_set(hsn.client_id,'" + clientid_mst + "')=1 and find_in_set(hsn.client_unit_id,'" + unitid_mst + "')=1 " +
                                //    "inner join (select sum(a.qtyord)-NVL(sum(b.qtyord),0) as qtyord,a.icode,a.vch_date ind_date,a.vch_num as ind_no,a.client_id,a.client_unit_id from purchases a " +
                                //    "left join purchases b on a.icode=b.icode and b.type not in ('66') and a.client_id=b.client_id and a.client_unit_id=b.client_unit_id where a.type='66' and " +
                                //    "a.client_id='" + clientid_mst + "' and a.client_unit_id='" + unitid_mst + "' group by a.icode,a.vch_date,a.vch_num,a.client_id,a.client_unit_id having " +
                                //    "sum(a.qtyord)-NVL(sum(b.qtyord),0)<>0) ind on ind.icode=it.icode and ind.client_id=it.client_id and ind.client_unit_id=it.client_unit_id " +
                                //    "where it.type='IT' and it.client_id='" + clientid_mst + "' and it.client_unit_id='" + unitid_mst + "'" + where + "";
                            }
                            break;

                        case "CHRGS":
                            cmd = "select (client_id||client_unit_id||master_id||to_char(vch_date,'yyyymmdd')||type) fstr,master_id Doc_No,master_name Charge_name " +
                                "from master_setting where find_in_set(client_unit_id,'" + unitid_mst + "')=1 and type='MR0'";
                            break;

                        case "RATE":
                            sgen.SetSession(MyGuid, "rtrno", param1.Trim());
                            cmd = "select (client_id||client_unit_id||master_id||to_char(vch_date,'yyyymmdd')||type) fstr,master_id Doc_No,group_name GST_Tax_Rate " +
                                "from master_setting where find_in_set(client_unit_id,'" + unitid_mst + "')=1 and type='HSN'";
                            break;
                        case "PORATE":
                            cmd = "select (client_id||client_unit_id||vch_num||to_char(vch_date,'yyyymmdd')||type) fstr,vch_num Doc_No," +
                                "to_char(convert_tz(vch_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') Doc_Date,type,irate " +
                                "from purchases where icode='" + param1 + "' and type in ('50','52','54') and client_unit_id='" + unitid_mst + "'";
                            break;
                    }
                    break;
                #endregion
                #region po_imp
                case "po_imp":
                    switch (btnval.ToUpper())
                    {
                        case "VIEW":
                            cmd = "select (p.client_id||p.client_unit_id||p.vch_num||to_char(p.vch_date,'yyyymmdd')||p.type) as fstr,p.vch_num PO_Number," +
                           "to_char(p.vch_date,'" + sgen.Getsqldateformat() + "') PO_Date," +
                           "c.c_name as Party_Name,c.c_gstin as Party_Gstin,c.addr Address,NVL(sf.c_name,'-') as From_Name," +
                           "NVL(sf.c_gstin,'-') as From_Gstin,nvl(sf.addr,'-') FromAddress,NVL(st.c_name,'-') as To_Name,NVL(st.c_gstin,'-') as To_Gstin,NVL(st.addr,'-') ToAddress," +
                           "p.icode ItemCode,p.iname ItemName,p.cpartno PartNo,p.uom UOM,p.hsno HSN,p.qtystk Stock_Qty,p.qtyind Indent_Qty,p.qtyord Order_Qty,p.irate ItemRate,p.taxrate TaxRate," +
                           "iamount Item_Amt,p.taxamt taxamt,p.discrate Discount_Rate,p.discamt Discount_Amt,p.lineamount LineAmt," +
                           "to_char(p.dlv_date,'" + sgen.Getsqldateformat() + "') Delivery_Date,p.indno IndentNo," +
                           "to_char(p.inddate,'" + sgen.Getsqldateformat() + "') Indent_Date," +
                           "p.basicamt BasicAmt from purchases p " +
                           "inner join clients_mst c on p.acode = c.vch_num and find_in_set(c.client_unit_id,p.client_unit_id)=1 and c.type = 'BCD' and substr(c.vch_num,0,3)='203' " +
                           "left join clients_mst sf on p.shipfrom = sf.vch_num and find_in_set(sf.client_unit_id,p.client_unit_id)=1 and sf.type = 'BCD' and substr(sf.vch_num,0,3)='203' " +
                           "left join clients_mst st on p.shipto = st.vch_num and find_in_set(st.client_unit_id,p.client_unit_id)=1 and st.type = 'BCD' and substr(st.vch_num,0,3)='203' " +
                           "where p.client_unit_id = '" + unitid_mst + "' and p.type in ('51','53') and to_date(to_char(p.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "')";
                            break;
                        case "EDIT":
                        case "PRINT":
                            cmd = "select (p.client_id||p.client_unit_id||p.vch_num||to_char(p.vch_date,'yyyymmdd')||p.type) as fstr,p.vch_num PO_Number," +
                            "to_char(p.vch_date,'" + sgen.Getsqldateformat() + "') PO_Date," +
                            "c.c_name as Party_Name from purchases p " +
                            "inner join clients_mst c on p.acode = c.vch_num and find_in_set(c.client_unit_id,p.client_unit_id)=1 and c.type = 'BCD' and substr(c.vch_num,0,3)='203' " +
                            "where p.client_unit_id = '" + unitid_mst + "' and p.type in ('51','53') and to_date(to_char(p.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "')";
                            break;
                        case "COPYPO":
                            cmd = "select (p.client_id||p.client_unit_id||p.vch_num||to_char(p.vch_date,'yyyymmdd')||p.type) as fstr,p.vch_num PO_Number," +
                            "to_char(p.vch_date,'" + sgen.Getsqldateformat() + "') PO_Date," +
                            "c.c_name as Party_Name,c.c_gstin as Party_Gstin,c.addr Address from purchases p " +
                            "inner join clients_mst c on p.acode = c.vch_num and find_in_set(c.client_unit_id,p.client_unit_id)=1 and c.type = 'BCD' and substr(c.vch_num,0,3)='203' " +
                           "where p.client_unit_id = '" + unitid_mst + "' and p.type in ('51','53') and p.pur_type='002' ";
                            break;
                        case "NEW":
                            cmd = "select '001' fstr,'001' master_id,'With Indent' master_name from dual union all " +
                                "select '002' fstr,'002' master_id,'Without Indent' master_name from dual";
                            break;
                        case "POTYPE":
                            cmd = "select (master_id||to_char(vch_date,'yyyymmdd')||type) fstr,master_id as Type,master_name Name " +
                                "from master_setting where find_in_set(client_unit_id,'" + unitid_mst + "')=1 and type='KPO' and master_id in ('51','53')";
                            break;
                        case "PARTY":
                        case "PARTYFRM":
                        case "PARTYTO":
                            cmd = @"select (a.vch_num|| to_char(a.vch_date,'yyyymmdd')||a.type) as fstr,a.c_name as vendor_Name,a.addr address,a.pincode as Pincode,
a.c_gstin as GSTIN,a.type_desc as Search_text,a.cpname as Per_Name,(case when a.cpcont='0000000000' then '-' else a.cpcont end) Contact_No from clients_mst a 
where find_in_set(a.client_unit_id,'" + unitid_mst + "')=1 and a.type='BCD' and substr(a.vch_num,0,3)='203'";
                            break;
                        case "ITEM":
                            sgen.SetSession(MyGuid, "Ptype", param2.Trim());
                            if (param2 == "002") // without Indent
                            {
                                if (param1 != "") where = " and it.icode not in ('" + param1.Trim().TrimEnd(',').Replace(",", "','").Trim().ToString() + "')";
                                cmd = "select (it.icode||to_char(it.vch_date,'yyyymmdd')||it.type) fstr,it.icode as Icode,it.iname," +
                                    "it.cpartno Partno,um.master_name as UOM,hsn.master_name as hsn from item it " +
                                    "inner join master_setting um on um.master_id=it.uom and um.type='UMM' and find_in_set(um.client_unit_id,'" + unitid_mst + "')=1 " +
                                    "inner join master_setting hsn on hsn.master_id=it.hsno and hsn.type='HSN' and find_in_set(hsn.client_unit_id,'" + unitid_mst + "')=1 " +
                                    "where find_in_set(it.client_unit_id,'" + unitid_mst + "')=1 and it.type='IT' " + where + "";
                            }
                            else if (param2 == "001")  // with Indent
                            {
                                if (param1 != "") where = " and (it.icode||to_char(it.vch_date,'yyyymmdd')||it.type||pc.ind_no||to_char(pc.vch_date,'yyyymmdd')) not in " +
                                        "('" + param1.Trim().TrimEnd(',').Replace(",", "','").Trim().ToString() + "')";
                                //brijesh
                                cmd = "select distinct (it.icode||to_char(it.vch_date,'yyyymmdd')||it.type||pc.ind_no||to_char(pc.vch_date,'yyyymmdd')) fstr" +
                                   ",pc.ind_no,to_char(pc.vch_date,'" + sgen.Getsqldateformat() + "') as ind_date,pc.icode as Icode,it.iname as Iname,it.cpartno Partno,um.master_name as UOM" +
                                   ",hsn.master_name as hsn,pc.ind_qty,pc.Bal_qty from item it inner join(select a.ind_no,a.vch_date,a.type," +
                                   " a.icode,a.client_unit_id,a.ind_qty,sum(a.po_qty) as used,(a.ind_qty - nvl(sum(a.po_qty), '0')) as Bal_qty " +
                                   "from(select nd.vch_num as ind_no,nd.vch_date,nd.type,nd.icode,nd.client_unit_id,nd.qtyord as ind_qty,nvl(b.qtyord,'0') as po_qty" +
                                   " from purchases nd left join purchases b on b.icode=nd.icode and b.indno=nd.vch_num and b.type not in ('66') and b.pur_type='001' and " +
                                   "nd.client_unit_id=b.client_unit_id where nd.client_unit_id='" + unitid_mst + "' and nd.type='66' " +
                                   "union all select nd.vch_num as ind_no,nd.vch_date,nd.type,nd.icode,nd.client_unit_id,nd.qtyord as ind_qty,nvl(b.col11,'0') as" +
                                   " close_qty from purchases nd left join enx_tab b on b.col7=nd.icode and b.col12=nd.vch_num and b.type = 'CPI' " +
                                   " and nd.client_unit_id=b.client_unit_id where nd.client_unit_id='" + unitid_mst + "' and nd.type='66') a " +
                                   "group by(a.ind_no,a.vch_date,a.type, a.icode,a.client_unit_id,a.ind_qty)) pc on pc.icode=it.icode and " +
                                   " find_in_set(pc.client_unit_id,it.client_unit_id)=1 and pc.bal_qty > 0 inner join master_setting um on um.master_id=it.uom and um.type='UMM'" +
                                   " and find_in_set(um.client_unit_id,'" + unitid_mst + "')=1 inner join master_setting hsn on hsn.master_id=it.hsno and hsn.type='HSN' and " +
                                   "find_in_set(hsn.client_unit_id,'" + unitid_mst + "')=1 where find_in_set(it.client_unit_id,'" + unitid_mst + "')=1 and it.type='IT' " + where + "";
                            }
                            break;

                        case "CHRGS":
                            cmd = "select (client_id||client_unit_id||master_id||to_char(vch_date,'yyyymmdd')||type) fstr,master_id Doc_No,master_name Charge_name " +
                                "from master_setting where find_in_set(client_unit_id,'" + unitid_mst + "')=1 and type='MR0'";
                            break;

                        case "RATE":
                            sgen.SetSession(MyGuid, "rtrno", param1.Trim());
                            cmd = "select (client_id||client_unit_id||master_id||to_char(vch_date,'yyyymmdd')||type) fstr,master_id Doc_No,group_name GST_Tax_Rate " +
                                "from master_setting where find_in_set(client_unit_id,'" + unitid_mst + "')=1 and type='HSN'";
                            break;
                        case "PORATE":
                            cmd = "select (client_id||client_unit_id||vch_num||to_char(vch_date,'yyyymmdd')||type) fstr,vch_num Doc_No," +
                                "to_char(convert_tz(vch_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') Doc_Date,type,irate " +
                                "from purchases where icode='" + param1 + "' and type in ('51','53') and client_unit_id='" + unitid_mst + "'";
                            break;
                    }
                    break;
                #endregion                
                #region Vendor Approval
                case "vendor_app":
                    switch (btnval.ToUpper())
                    {
                        case "VIEW":
                        case "EDIT":
                            cmd = "select distinct  a.client_id||a.client_unit_id||a.vch_num|| to_char(a.vch_date,'yyyymmdd')||a.type as fstr,b.c_name as Vendor_Name," +
                                "a.vch_num as Doc_No,to_char( convert_tz( a.Vch_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') as Doc_Date " +
                                " from ivendor a inner join clients_mst b on a.acode=b.vch_num and b.type='BCD' and substr(b.vch_num,0,3)='203' and find_in_set(a.client_unit_id,b.client_unit_id)=1  WHERE a.client_unit_id='" + unitid_mst + "' and a.type='VAP' and to_date(to_char(a.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "')";
                            break;
                        case "PARTY":
                            cmd = "select distinct a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type as fstr," +
                                "a.c_name as Client_name,a.addr as address,a.pincode as Pincode,a.c_gstin as GSTIN," +
                                "a.type_desc as Search_text,a.cpname as Contact_person_name,a.cpcont as contact_no,a.cpaltcont as Alter_contactno," +
                                "a.cpemail as Email from clients_mst a  where find_in_set(a.client_unit_id, '" + unitid_mst + "')=1 and a.type = 'BCD' and substr(a.vch_num,0,3)='203'";
                            break;
                        case "ITEM":
                            if (param1 != "") where = " and it.icode not in ('" + param1.Trim().TrimEnd(',').Replace(",", "','").Trim().ToString() + "')";
                            cmd = "select it.icode||to_char(it.vch_date,'yyyymmdd')||it.type fstr,it.icode as Icode,it.iname as Iname,it.cpartno Partno," +
                                "um.master_name as UOM,hsn.master_name as hsn,hsn.group_name taxrate from item it " +
                                "inner join master_setting um on um.master_id=it.uom and um.type='UMM'  and find_in_set(um.client_unit_id, it.client_unit_id)= 1 " +
                                "inner join master_setting hsn on hsn.master_id=it.hsno and hsn.type='HSN' and find_in_set(hsn.client_unit_id, it.client_unit_id)= 1 " +
                                "where find_in_set(it.client_unit_id,'" + unitid_mst + "')=1 and it.type='IT' " + where + "";
                            break;
                    }
                    break;
                #endregion
                #region pr_app
                case "pr_app":
                    switch (btnval.ToUpper())
                    {
                        case "SHOW":
                            string FromDt = sgen.Make_date(param1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                            string ToDt = sgen.Make_date(param2).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                            //cmd = "SELECT  a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type||a.icode  as fstr,a.vch_num as Indent_No,to_char(convert_tz(a.vch_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "')" +
                            //    " Indent_Date,a.icode,b.iname ,b.uom,um.master_name as UOM_name,a.qtyord as Indent_Qty,a.qtyord" +
                            //    " as App_Qty,hsn.master_name as hsn,hsn.group_name taxrate,b.cpartno FROM purchases a INNER join item b on a.icode=b.icode and b.type='IT' and b.client_unit_id=a.client_unit_id " +
                            //    "inner join master_setting um on um.master_id=b.uom and um.type='UMM' inner join master_setting hsn on hsn.master_id=b.hsno and hsn.type='HSN'" +
                            //    " where a.type='66' and a.client_unit_id='" + unitid_mst + "' and " +
                            //    " convert_tz(a.vch_date, 'UTC', '" + sgen.Gettimezone() + "') between to_date('" + FromDt + "','yyyy-MM-dd') and to_date('" + ToDt + "','yyyy-MM-dd') order by a.vch_num";
                            cmd = "SELECT  a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type||a.icode  as fstr," +
                                "a.vch_num as Indent_No,to_char(a.vch_date, '" + sgen.Getsqldateformat() + "') Indent_Date,a.icode,b.iname ," +
                                "b.uom,um.master_name as UOM_name,a.qtyord as Indent_Qty,a.qtyord as App_Qty,hsn.master_name as hsn,hsn.group_name taxrate," +
                                "b.cpartno FROM purchases a INNER join item b on a.icode = b.icode and b.type = 'IT' and find_in_set(b.client_unit_id, a.client_unit_id)=1 inner join" +
                                " master_setting um on um.master_id = b.uom and um.type = 'UMM' and a.client_unit_id = um.client_unit_id " +
                                "inner join master_setting hsn on hsn.master_id = b.hsno and hsn.type = 'HSN' and a.client_unit_id = hsn.client_unit_id " +
                                " where a.client_unit_id = '" + unitid_mst + "' and a.type = '66' and convert_tz(a.vch_date, 'UTC', '" + sgen.Gettimezone() + "') between to_date('" + FromDt + "','yyyy-MM-dd') and to_date('" + ToDt + "','yyyy-MM-dd') order by a.vch_num";
                            break;
                        case "LSR":
                            cmd = "";
                            break;
                    }
                    break;
                #endregion
                #region party_detail - party
                case "party":
                    if (sgen.GetSession(MyGuid, "VD_TYPEMST") != null)
                    {
                        type = sgen.GetSession(MyGuid, "VD_TYPEMST").ToString().Trim();
                        string cn_type = sgen.GetSession(MyGuid, "VD_CONT").ToString().Trim();
                        string condition = "", cond = "fstr,Parent_Account";
                        //string condition = "", cond = "fstr";
                        string prefix = sgen.GetSession(MyGuid, "VD_PREFIX").ToString().Trim();
                        if (mid_mst == "40002.9")
                        {
                            condition = "and (a.Pros ='0' or a.Pros is null or a.Pros= 'N')";
                        }
                        //if ((role_mst != "owner") && (m_module3 == "crmvmain"))
                        //{
                        //    condition = "and a.ent_by='" + userid_mst + "'";
                        //}
                        if ((role_mst != "owner") && (m_module3 == "crmvmain"))
                        {
                            string view_user = sgen.getstring(userCode, "select param1 from controls where client_unit_id='" + unitid_mst + "' and " +
                                "type='CTL' and id='000006' and m_module3='" + m_module3 + "' and param5='crm'");
                            if (view_user == "Y")
                            {
                                condition = "and a.ent_by='" + userid_mst + "'";
                            }
                            else { condition = ""; }
                        }
                        condition = condition + "and substr(a.vch_num,0,3)='" + prefix + "'";
                        if (mid_mst == "16000.2.2")
                        {
                            condition = condition + " and a.pubr='Y'";
                        }
                        else if (mid_mst == "35004.3")
                        {
                            condition = condition + " and a.mftr='Y'";
                        }
                        else { condition = condition + " and (a.mftr is null or a.mftr='0' or a.mftr='N' or a.mftr='-') and (a.pubr is null or a.pubr='0' or a.pubr='N' or a.pubr='-')"; }
                        switch (btnval.ToUpper())
                        {
                            #region Edit
                            case "EDIT":
                            case "PRINT":
                            case "EXT":
                                if (sgen.GetSession(MyGuid, "tmp_client") != null)
                                {
                                    mq = @"select '-' as fstr,vch_num as doc_no , c_name ORGANISATION_NAME , cpname cp_first_name,cp_mname CP_MIDDLE_NAME, cp_lname CP_LAST_NAME,cplandno CONTACT_P_MOBILE_NO,
                                    type_desc SEARCH_TEXT from clients_mst where 1 = 2";

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
                                cond = cond + "";

                                cmd = "select distinct (a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr" +
                                     ",a.vch_num as doc_no,  a.c_name ORGANISATION_NAME,(case when length(pr.c_name)=1  and pr.c_name='0' then '-' else pr.c_name end ) as Parent_Account," +
                                     " a.type_desc SEARCH_TEXT, replace(ct.cpname,'0','-') cp_first_name, replace(nvl(ct.cp_mname,'-'),'0','-') CP_MIDDLE_NAME, replace(nvl(ct.cp_lname,'-'),'0','-') CP_LAST_NAME, (case when ct.cplandno='0000000000' then '-' else ct.cplandno end) CONTACT_P_MOBILE_NO " +
                                     "from clients_mst a " +
                                       "left join clients_mst pr on a.parent_id=pr.vch_num and pr.type='BCD' and find_in_set(pr.client_unit_id,'" + unitid_mst + "')= 1 " +
                                     "left join clients_mst ct  on a.vch_num=ct.ref_code  and find_in_set(ct.client_unit_id,'" + unitid_mst + "')= 1 and substr(ct.ref_code,0,3)='" + prefix + "' " +
                                     "where find_in_set(a.client_unit_id,'" + unitid_mst + "')=1 and a.type in ('" + type + "','DD" + type + "') " + condition + " order by a.vch_num desc";

                                //cmd = "select  (a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr" +
                                //    ",a.vch_num as DOC_NO ,  a.c_name ORGANISATION_NAME from clients_mst a where find_in_set(a.client_unit_id,'" + unitid_mst + "')=1 and a.type in ('" + type + "','DD" + type + "') " + condition + " order by a.vch_num desc";

                                cmd = "select  " + cond + " from (" + cmd + ")a";

                                break;
                            #endregion

                            #region View
                            case "VIEW":
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

                                cmd = "select distinct (a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr" +
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
                                    "  on a.ref_Ext_acc=w.vch_num and w.type='BCD' and find_in_set(w.client_unit_id,'" + unitid_mst + "')= 1" +
                                    " inner join user_details ud on  a.ent_by =ud.vch_num left join clients_mst ct  on a.vch_num=ct.ref_code  and " +
                                    " find_in_set(ct.client_unit_id,'" + unitid_mst + "')= 1 and substr(ct.ref_code,0,3)='" + prefix + "' left join  user_details u on u.vch_num = a.rel_mgr" +
                                    " left join  user_details v on v.vch_num = a.ref_user where find_in_set(a.client_unit_id,'" + unitid_mst + "')=1 and a.type in ('" + type + "','DD" + type + "') " + condition + " order by a.vch_num desc";

                                //cmd = "select  (a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr" +
                                //    ",a.vch_num as DOC_NO ,  a.c_name ORGANISATION_NAME from clients_mst a where find_in_set(a.client_unit_id,'" + unitid_mst + "')=1 and a.type in ('" + type + "','DD" + type + "') " + condition + " order by a.vch_num desc";

                                cmd = "select  " + cond + " from (" + cmd + ")a";

                                break;
                            #endregion
                            case "ADD1":
                            case "ADD2":
                            case "ADD3":
                                cmd = "select distinct alpha_2||state_gst_code ||city_name as fstr," +
                                "country_name,state_name ,city_name from country_state   order by state_name desc ";
                                break;
                            case "UNIT":
                                cmd = "select (cup_id||company_profile_id||to_char(vch_date,'yyyymmdd')) fstr,company_profile_id Company_d,(unit_name||' ('||cup_id||')') " +
                                    "Unit_Name from company_unit_profile where unit_status='1' order by unit_name asc";
                                break;
                            case "PROSPECT":
                                cmd = "select (a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr," +
                                          "a.c_name as Name,a.type_desc as Search_text," +
                                          "a.cpname as PerName," +
                                          "(case when a.cpcont='0000000000' then '-' else a.cpcont end) as PerContact," +
                                          "(case when a.cpemail='ab@ab.ab' then '-' else a.cpemail end) as PerEmail from clients_mst a where a.client_unit_id='" + unitid_mst + "' " +
                                          "and a.type in ('PRO') and (Pros is null or Pros= 'N')  and substr(a.vch_num,0,3)='909' order by a.vch_num desc";
                                break;
                            case "PARENT":
                                cmd = "select (a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr," +
                                          "a.vch_num doc_no,a.c_name as Name," +
                                          "a.cpname as PerName,(case when a.cpcont='0000000000' then '-' else a.cpcont end) as PerContact,(case when a.cpaltcont='0000000000' then '-' else a.cpaltcont end) as PerAltContact " +
                                          "from clients_mst a where a.client_unit_id='" + unitid_mst + "' and a.type in ('" + type + "') and (Parent_id is null " +
                                          " or parent_id=0 or vch_num=parent_id)  and substr(a.vch_num,0,3)='" + prefix + "'" +
                                          " order by a.vch_num desc";
                                break;
                            case "EXISTING":
                                cmd = "select (a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr,b.c_name as Parent_Account," +
                                     "a.c_name as Account_Name,a.type_desc as Search_text," +
                                     "a.cpname as PerName,(case when (a.cplandno='0000000000' or a.cplandno='0000000000000' or a.cplandno='0') then '-' else a.cplandno end) as PerContact, (case when (a.cpemail='ab@ab.ab' or a.cpemail='AB@AB.AB' or a.cpemail='0') then '-' else a.cpemail end) as PerEmail, 'Account Detail' as Data_Source from clients_mst a left join clients_mst b on a.parent_id=b.vch_num " +
                                     " and a.client_unit_id=b.client_unit_id and b.type='" + type + "' where a.client_unit_id='" + unitid_mst + "' and a.type in ('" + type + "') and substr(a.vch_num,0,3)='" + prefix + "'";
                                break;
                            case "LSR":
                                string q1 = "SELECT client_id||client_unit_id||vch_num||TO_CHAR(vch_date, 'YYYYMMDD')|| type as fstr,master_type as Financial_Category_Code , master_name AS Financial_Category,'-' Main_Head,'-' as Sub_Head,'-' as Sub_Sub_Head,COL1 as recon FROM master_setting where type = 'AGM' AND TRIM(CLIENT_ID)= '-' AND TRIM(CLIENT_UNIT_ID)= '-'";
                                string q2 = "SELECT  (a.client_id||a.client_unit_id|| a.vch_num|| TO_CHAR(a.vch_date, 'YYYYMMDD')|| a.type) as fstr,a.master_type as Financial_Category_Code ,b.master_name as Financial_Category,a.master_name as Main_Head,'-' as Sub_Head,'-' as Sub_Sub_Head,a.COL1 as recon FROM master_setting a INNER JOIN master_setting b ON SUBSTR(a.master_type, 1, 1) = SUBSTR(b.master_type, 1, 1)  AND TO_NUMBER(SUBSTR(b.master_type,2,7))= 0   and b.type = 'AGM'  where TO_NUMBER(SUBSTR(a.master_type,2,3))> 0 AND TO_NUMBER(SUBSTR(a.master_type,4,7))= 0 and a.type = 'AGM' and a.client_id='" + clientid_mst + "' and a.client_unit_id='" + unitid_mst + "'";
                                string q3 = "SELECT (a.client_id|| a.client_unit_id|| a.vch_num|| TO_CHAR(a.vch_date, 'YYYYMMDD')|| a.type) as fstr,a.master_type as Financial_Category_Code ,b.master_name as Financial_Category ,c.master_name as Main_Head,a.master_name as Sub_Head,'-' as Sub_Sub_Head,a.COL1 as recon  FROM master_setting a INNER JOIN master_setting b ON SUBSTR(a.master_type, 1, 1) = SUBSTR(b.master_type, 1, 1)  AND TO_NUMBER(SUBSTR(b.master_type,2,7))= 0   and b.type = 'AGM'  INNER JOIN master_setting c ON SUBSTR(a.master_type,1,3)= SUBSTR(c.master_type, 1, 3)  AND TO_NUMBER(SUBSTR(c.master_type,4,7))= 0   and c.type = 'AGM'  where TO_NUMBER(SUBSTR(a.master_type,4,5))> 0 AND TO_NUMBER(SUBSTR(a.master_type,6,7))= 0 and a.type = 'AGM' and a.client_id='" + clientid_mst + "' and a.client_unit_id='" + unitid_mst + "'  and a.client_id=c.client_id and  a.client_unit_id = c.client_unit_id";
                                string q4 = "SELECT  (a.client_id|| a.client_unit_id|| a.vch_num|| TO_CHAR(a.vch_date, 'YYYYMMDD')|| a.type) as fstr,a.master_type as Financial_Category_Code ,b.master_name AS Financial_Category,c.master_name as Main_Head,d.master_name as Sub_Head,a.master_name as Sub_Sub_Head,a.COL1 as recon  FROM master_setting a INNER JOIN master_setting b ON SUBSTR(a.master_type, 1, 1) = SUBSTR(b.master_type, 1, 1)  AND TO_NUMBER(SUBSTR(b.master_type,2,7))= 0   and b.type = 'AGM'  INNER JOIN master_setting c ON SUBSTR(a.master_type,1,3)= SUBSTR(c.master_type, 1, 3)  AND TO_NUMBER(SUBSTR(c.master_type,4,7))= 0   and c.type = 'AGM'  INNER JOIN master_setting d ON SUBSTR(a.master_type,1,5)= SUBSTR(d.master_type, 1, 5)  AND TO_NUMBER(SUBSTR(d.master_type,6,7))= 0 and d.type = 'AGM'  where TO_NUMBER(SUBSTR(a.master_type,6,7))> 0  and a.type = 'AGM' and a.client_id='" + clientid_mst + "' and a.client_unit_id='" + unitid_mst + "'  and a.client_id=c.client_id and a.client_unit_id= c.client_unit_id and a.client_id=d.client_id and a.client_unit_id=d.client_unit_id";
                                mq = q1 + " union   " + q2 + " union " + q3 + " union " + q4;
                                if (mid_mst == "21008.1")//customer
                                {
                                    //condition = "and  substr(a.ledger_code,1,1)=3";
                                    condition = " substr(Financial_Category_Code,1,1)=3 and recon = 'Y'";
                                }
                                else if ((mid_mst == "28005.1") || (mid_mst == "9008.6") || (mid_mst == "41005.8"))//vendor
                                {
                                    condition = " substr(Financial_Category_Code,1,1)=2 and recon = 'Y'";
                                }
                                cmd = "select * from (" + mq + ") where " + condition + "";
                                //cmd = "select (a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr , group_code,b.Financial_Category as Group_Name,b.Main_Head,b.Sub_Head,b.Sub_Sub_Head," +
                                //  " ledger_code,ledger_name, ledger_abb from ledger_master a inner join (" + mq + ") b on a.group_code=b. Financial_Category_Code   where a.type='ALM'" +
                                //  " and a.client_id='" + clientid_mst + "' and a.client_unit_id='" + unitid_mst + "' and a.acc_type='Y' " + condition + " order by a.vch_num";
                                break;
                            case "DUPLICATE":
                                //Account
                                mq1 = "select (a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr, a.vch_num as doc_no,b.c_name as Parent_Account," +
                                "upper(a.c_name) as Account_Name,a.cpname as PerName," +
                                "(case when a.cpcont='0000000000' then '-' else a.cpcont end) as PerContact," +
                                "(case when a.cpemail='ab@ab.ab' then '-' else a.cpemail end) as PerEmail " +
                                ", 'Account Detail' as Data_Source from clients_mst a left join clients_mst b on a.parent_id=b.vch_num " +
                                " and a.client_unit_id=b.client_unit_id and b.type='BCD' where a.client_unit_id='" + unitid_mst + "'" +
                                " and a.type in ('BCD') and substr(a.vch_num,0,3)='303'";
                                //PROPECT
                                string mq2 = "select (a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr , a.vch_num as doc_no,a.c_name" +
                                        " as Parent_Account,upper(a.c_name) as Account_Name ,a.cpname as PerName," +
                                        "(case when a.cpcont='0000000000' then '-' else a.cpcont end) as PerContact ," +
                                        "(case when a.cpemail='ab@ab.ab' then '-' else a.cpemail end) as PerEmail" +
                                        ", 'Prospect Data' as Data_Source  from clients_mst a  where a.client_unit_id = '" + unitid_mst + "' and a.type in ('PRO') and (a.Pros is null or a.Pros = 'N') and substr(a.vch_num,0,3)='909' ";
                                //LEAD
                                string mq3 = "select (vch_num||to_char(vch_date,'yyyymmdd')||type) as fstr , vch_num as doc_no," +
                                    " cust_name as Parent_Account,upper(cust_name) Account_Name,cp_fname PerName," +
                                    "(case when mobile_no='0000000000' then '-' else mobile_no end) PerContact," +
                                    "(case when emailid='ab@ab.ab' then '-' else emailid end) PerEmail" +
                                    ",'LEAD' Data_Source  from lead_master where client_unit_id = '" + unitid_mst + "' and type='LED' and cust_id='0' and " +
                                    "to_date(to_char(vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and " +
                                    "to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "')";

                               
                                cmd = mq1 + " union " + mq2 + " union " + mq3;
                                cmd = "select * from (" + cmd + ")a where upper(Account_Name) like upper('%" + param1.ToUpper() + "%')";
                                break;
                        }
                    }
                    break;
                #endregion
                #region party_unit_detail - commented code
                //case "party_unit":
                //    if (sgen.GetSession(MyGuid, "UNT_TYPEMST").ToString() != null)
                //    {
                //        type = sgen.GetSession(MyGuid, "UNT_TYPEMST").ToString().Trim();
                //        switch (btnval.ToUpper())
                //        {
                //            case "EDIT":
                //            case "VIEW":
                //            case "PRINT":
                //                cmd = "select (a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr,b.c_name as Client_Name," +
                //                          "a.c_name as UnitName,a.pincode as Pincode,a.isgstr IsGst,a.c_gstin as GSTIN,a.type_desc as Search_text," +
                //                          "a.cpname as PerName,a.cpcont as PerContact,a.cpaltcont as PerAltContact,a.cpemail as PerEmail,a.cpdesig " +
                //                          "as PerDesig from clients_mst a inner join clients_mst b on a.panno=b.vch_num and b.type= a.occ_type" +
                //                          " and a.client_unit_id=b.client_unit_id where a.client_unit_id='" + unitid_mst + "'" +
                //                          " and a.type in ('" + type + "','DD" + type + "')  order by a.panno,a.vch_num desc";
                //                break;
                //            case "CLIENT":
                //                cmd = "select (a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr," +
                //                          "a.c_name as Name,a.pincode as Pincode,a.isgstr IsGst,a.c_gstin as GSTIN,a.type_desc as Search_text," +
                //                          "a.cpname as PerName,a.cpcont as PerContact,a.cpaltcont as PerAltContact,a.cpemail as PerEmail,a.cpdesig " +
                //                          "as PerDesig from clients_mst a where a.client_unit_id='" + unitid_mst + "' and a.type in ('" + sgen.GetSession(MyGuid, "NC_TYPEMST").ToString() + "') and a.cfrm='Y' order by a.vch_num desc";
                //                break;
                //            case "ADD1":
                //            case "ADD2":
                //                cmd = "select distinct alpha_2||state_gst_code ||city_name as fstr," +
                //                "alpha_2,country_name,state_gst_code,state_name ,city_name from country_state   order by state_name desc ";
                //                break;
                //        }
                //    }
                //    break;
                //#endregion
                //#region cust_enquiry
                //case "enq":
                //    switch (btnval.ToUpper())
                //    {
                //        case "EDIT":
                //        case "VIEW":
                //            cmd = "select (a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr," +
                //                "    b.c_name as Organisation,c.c_name as Unit_Name,a.COUNTRY as remark,a.COUNTRY2 as follow_remark  from clients_mst a " +
                //                "inner join clients_mst b  on a.panno = b.vch_num and b.type = 'BCD' and a.client_unit_id = b.client_unit_id " +
                //                "       inner join clients_mst c on a.c_name = c.vch_num and c.type = 'UNT' and a.client_unit_id = c.client_unit_id" +
                //                "  where a.client_unit_id = '" + unitid_mst + "' and a.type = 'ENQ'";
                //            break;
                //        case "CLIENT":
                //            cmd = "select (a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr,b.c_name as Client_Name," +
                //                      "a.c_name as UnitName,a.pincode as Pincode,a.isgstr IsGst,a.c_gstin as GSTIN,a.type_desc as Search_text," +
                //                      "a.cpname as PerName,a.cpcont as PerContact,a.cpaltcont as PerAltContact,a.cpemail as PerEmail,a.cpdesig " +
                //                      "as PerDesig from clients_mst a inner join clients_mst b on a.panno=b.vch_num and b.type= a.occ_type" +
                //                      " and a.client_unit_id=b.client_unit_id where a.client_unit_id='" + unitid_mst + "' and a.type in ('UNT') order by a.panno,a.vch_num desc";
                //            break;
                //        case "ADD1":
                //            cmd = "select distinct alpha_2||state_gst_code ||city_name as fstr," +
                //            "alpha_2,country_name,state_gst_code,state_name ,city_name from country_state   order by state_name desc ";
                //            break;
                //    }
                //    break;
                #endregion
                #region pur_report
                case "pur_rpt":
                    switch (btnval.ToUpper())
                    {
                        case "POPARTYSUM":
                            cmd = "select p.acode as fstr, p.acode as party_code,c.C_name as Party_name,c.c_gstin as Gst_no,sum(p.igst) as IGST,sum(p.sgst) as SGST," +
                                "sum(p.cgst) as CGST,sum(p.basicamt) as basic_amt,sum(p.netamt) as net_amount from purchases p inner join clients_mst c on" +
                                " p.acode = c.vch_num and find_in_set(c.client_id, p.client_id)=1 and find_in_set(c.client_unit_id, p.client_unit_id)=1 and c.type = 'BCD' and " +
                                "substr(c.vch_num,0,3)='203' where p.client_unit_id = '" + unitid_mst + "' and p.type in ('50', '52') group by" +
                                " p.acode,c.C_name,c.c_gstin";
                            break;
                        case "POPARTYDET":
                            cmd = "select (p.client_id||p.client_unit_id||p.vch_num||to_char(p.vch_date,'yyyymmdd')||p.type) as fstr,p.vch_num PO_Number" +
                                ",to_char(p.vch_date, '" + sgen.Getsqldateformat() + "') PO_Date,c.vch_num as party_code,c.c_name as Party_Name,c.c_gstin as Party_Gstin,c.addr Address," +
                                "p.qtyord Order_Qty, p.irate ItemRate, p.taxrate TaxRate, iamount Item_Amt,p.taxamt taxamt, p.discrate Discount_Rate,p.discamt " +
                                "Discount_Amt, p.basicamt BasicAmt, p.lineamount LineAmt, p.netamt,p.indno IndentNo,to_char(p.inddate, '" + sgen.Getsqldateformat() + "') Indent_Date,p.qtyind Indent_Qty,to_char(p.dlv_date, '" + sgen.Getsqldateformat() + "') Delivery_Date" +
                                " from purchases p inner join clients_mst c on p.acode = c.vch_num and find_in_set(c.client_unit_id, p.client_unit_id)=1 and c.type = 'BCD' and substr(c.vch_num,0,3)='203' left join clients_mst sf " +
                                "on p.shipfrom = sf.vch_num and find_in_set(sf.client_unit_id, p.client_unit_id)=1 and sf.type = 'BCD' and " +
                                "substr(sf.vch_num,0,3)='203' left join clients_mst st on p.shipto = st.vch_num and " +
                                "find_in_set(st.client_unit_id, p.client_unit_id)=1 and st.type = 'BCD' and substr(st.vch_num,0,3)='203' where p.client_unit_id = '" + unitid_mst + "' and p.type in ('50', '52') and p.acode = '" + param1 + "'";
                            break;
                        case "POITSUM":
                            cmd = "select p.icode as fstr, p.icode as item_code,it.iname as item_name,sum(p.igst) as IGST,sum(p.sgst) as SGST,sum(p.cgst) as CGST" +
                                ",sum(p.basicamt) as basic_amt,sum(p.netamt) as net_amount from purchases p inner join item it on p.icode = it.icode" +
                                " and find_in_set(p.client_unit_id, it.client_unit_id)= 1 and it.type = 'IT' " +
                                "where p.client_unit_id = '" + unitid_mst + "' and p.type in ('50', '52')" +
                                "group by p.icode,it.iname";
                            break;
                        case "POITDET":
                            cmd = "select (p.client_id||p.client_unit_id||p.vch_num||to_char(p.vch_date,'yyyymmdd')||p.type) as fstr,p.vch_num PO_Number" +
                                ",to_char(p.vch_date, '" + sgen.Getsqldateformat() + "') PO_Date,it.icode,it.iname, p.qtyord Order_Qty, p.irate ItemRate," +
                                "p.taxrate TaxRate, iamount Item_Amt,p.taxamt taxamt, p.discrate Discount_Rate,p.discamt Discount_Amt, p.basicamt BasicAmt," +
                                "p.lineamount LineAmt, p.netamt,p.indno IndentNo,to_char(p.inddate, '" + sgen.Getsqldateformat() + "') Indent_Date,p.qtyind Indent_Qty," +
                                "to_char(p.dlv_date, '" + sgen.Getsqldateformat() + "') Delivery_Date from purchases p inner join item it on p.icode = it.icode and " +
                                "find_in_set(p.client_unit_id, it.client_unit_id)= 1 and it.type = 'IT' where p.client_unit_id = " +
                                "'" + unitid_mst + "' and p.type in ('50', '52') and it.icode = '" + param1 + "'";
                            break;
                    }
                    break;
                #endregion
                #region pur_report2
                case "pur_rpt2":
                    switch (btnval.ToUpper())
                    {
                        case "POPARTYSUM":
                            cmd = "select p.acode as fstr, p.acode as party_code,c.C_name as Party_name,c.c_gstin as Gst_no,sum(p.igst) as IGST,sum(p.sgst) as SGST," +
                                "sum(p.cgst) as CGST,sum(p.basicamt) as basic_amt,sum(p.netamt) as net_amount from purchases p inner join clients_mst c on" +
                                " p.acode = c.vch_num and find_in_set(c.client_unit_id, p.client_unit_id)=1 and c.type = 'BCD' and " +
                                "substr(c.vch_num,0,3)='203' where p.client_unit_id = '" + unitid_mst + "' and p.type in ('50', '52') group by" +
                                " p.acode,c.C_name,c.c_gstin";
                            break;
                        case "POPARTYDET":
                            cmd = "select (p.client_id||p.client_unit_id||p.vch_num||to_char(p.vch_date,'yyyymmdd')||p.type) as fstr,p.vch_num PO_Number" +
                                ",to_char(p.vch_date, '" + sgen.Getsqldateformat() + "') PO_Date,c.vch_num as party_code,c.c_name as Party_Name,c.c_gstin as Party_Gstin,c.addr Address," +
                                "p.qtyord Order_Qty, p.irate ItemRate, p.taxrate TaxRate, iamount Item_Amt,p.taxamt taxamt, p.discrate Discount_Rate,p.discamt " +
                                "Discount_Amt, p.basicamt BasicAmt, p.lineamount LineAmt, p.netamt,p.indno IndentNo,to_char(p.inddate, '" + sgen.Getsqldateformat() + "') Indent_Date,p.qtyind Indent_Qty,to_char(p.dlv_date, '" + sgen.Getsqldateformat() + "') Delivery_Date" +
                                " from purchases p inner join clients_mst c on p.acode = c.vch_num and find_in_set(c.client_unit_id, p.client_unit_id)=1 and c.type = 'BCD' and substr(c.vch_num,0,3)='203' left join clients_mst sf " +
                                "on p.shipfrom = sf.vch_num and find_in_set(sf.client_unit_id, p.client_unit_id)=1 and sf.type = 'BCD' and " +
                                "substr(sf.vch_num,0,3)='203' left join clients_mst st on p.shipto = st.vch_num and " +
                                "find_in_set(st.client_unit_id, p.client_unit_id)=1 and st.type = 'BCD' and substr(st.vch_num,0,3)='203' where p.client_unit_id = '" + unitid_mst + "' and p.type in ('50', '52') and p.acode = '" + param1 + "'";
                            break;
                        case "POITSUM":
                            cmd = "select p.icode as fstr, p.icode as item_code,it.iname as item_name,sum(p.igst) as IGST,sum(p.sgst) as SGST,sum(p.cgst) as CGST" +
                                ",sum(p.basicamt) as basic_amt,sum(p.netamt) as net_amount from purchases p inner join item it on p.icode = it.icode" +
                                " and find_in_set(p.client_unit_id, it.client_unit_id)= 1 and it.type = 'IT' " +
                                "where p.client_unit_id = '" + unitid_mst + "' and p.type in ('50', '52') group by p.icode,it.iname";
                            break;
                        case "POITDET":
                            cmd = "select (p.client_id||p.client_unit_id||p.vch_num||to_char(p.vch_date,'yyyymmdd')||p.type) as fstr,p.vch_num PO_Number" +
                                ",to_char(p.vch_date, '" + sgen.Getsqldateformat() + "') PO_Date,it.icode,it.iname, p.qtyord Order_Qty, p.irate ItemRate," +
                                "p.taxrate TaxRate, iamount Item_Amt,p.taxamt taxamt, p.discrate Discount_Rate,p.discamt Discount_Amt, p.basicamt BasicAmt," +
                                "p.lineamount LineAmt, p.netamt,p.indno IndentNo,to_char(p.inddate, '" + sgen.Getsqldateformat() + "') Indent_Date,p.qtyind Indent_Qty," +
                                "to_char(p.dlv_date, '" + sgen.Getsqldateformat() + "') Delivery_Date from purchases p inner join item it on p.icode = it.icode and " +
                                "find_in_set(p.client_unit_id, it.client_unit_id)= 1 and it.type = 'IT' where p.client_unit_id = " +
                                "'" + unitid_mst + "' and p.type in ('50', '52') and it.icode = '" + param1 + "'";
                            break;
                    }
                    break;
                #endregion
                #region PO shedule
                case "po_sch":
                    switch (btnval.ToUpper())
                    {
                        case "NEW":
                            cmd = "select master_id||to_char(vch_date,'yyyymmdd')||type fstr,master_id as Type,master_name Name " +
                                "from master_setting where type = 'KPO' and find_in_set(client_unit_id,'" + unitid_mst + "')=1" +
                                " and substr(master_id,1,1)='5'";
                            break;
                        case "GETSO":
                            string cond = " and a.type='" + sgen.GetSession(MyGuid, "BDPOTYPE").ToString() + "' and a.acode = '" + sgen.GetSession(MyGuid, "PRTY_ID").ToString() + "'";
                            cmd = "select distinct (a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr,a.vch_num as po_number,to_char(a.vch_date, 'yyyymmdd') as po_date from purchases a where a.client_unit_id = '" + unitid_mst + "' " + cond + "";
                            break;
                        case "PRINT":
                        case "EDIT":
                            cmd = " select (sd.client_id||sd.client_unit_id||sd.vch_num||to_char(sd.vch_date,'yyyymmdd')||sd.type) as fstr, sd.vch_num as Doc_no ," +
                               "cl.c_name as name,to_char(sd.vch_date,'" + sgen.Getsqldateformat() + "') as Doc_date from kc_tab sd " +
                               "inner join clients_mst cl on cl.vch_num = sd.col1 and cl.type = 'BCD' and substr(cl.vch_num,0,3)='203' and find_in_set(cl.client_unit_id,sd.client_unit_id)=1 " +
                               "where sd.col20 = 'BPS' and substr(sd.type,1,1)='5' and sd.client_unit_id = '" + unitid_mst + "' and to_date(to_char(sd.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "')";
                            break;
                        case "VIEW":
                            cmd = " select (sd.client_id||sd.client_unit_id||sd.vch_num||to_char(sd.vch_date,'yyyymmdd')||sd.type) as fstr, sd.vch_num as Doc_no ," +
                                "cl.c_name as name,cl.c_gstin as gstin,cl.addr,to_char(sd.vch_date,'" + sgen.Getsqldateformat() + "') as Doc_date," +
                                "sd.vch_num as Sdl_no,sd.col3 as PO_no,sd.col4 as So_no,sd.col21 as Remark," +
                                "to_char(sd.date2,'" + sgen.Getsqldateformat() + "') as sdl_st_date,to_char(sd.date3,'" + sgen.Getsqldateformat() + "') as sdl_end_date," +
                                "to_char(sd.date7,'" + sgen.Getsqldateformat() + "') as Po_dt1,to_char(sd.date6,'" + sgen.Getsqldateformat() + "') as Po_dt2,sd.col5 as Icode,sd.col27 as IName," +
                                "sd.col7 as PartNo, sd.col8 as UOM, sd.col9 as Order_qty,sd.col10 as Plan_qty,sd.col12 as remark2,sd.col13 as So_no2 from kc_tab" +
                                " sd inner join clients_mst cl on cl.vch_num = sd.col1 and cl.type = 'BCD' and substr(cl.vch_num,0,3)='203' and find_in_set(cl.client_unit_id,sd.client_unit_id)=1 where sd.col20 = 'BPS' and substr(sd.type,1,1)='5' and sd.client_unit_id = '" + unitid_mst + "' and to_date(to_char(sd.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "')";
                            break;
                        case "PARTY":
                            string condi = " and pc.type='" + sgen.GetSession(MyGuid, "BDPOTYPE").ToString() + "'";
                            cmd = "select distinct (a.vch_num|| to_char(a.vch_date,'yyyymmdd')||a.type) as fstr,a.vch_num party_code,a.c_name name," +
                            "a.c_gstin as gstin,a.addr,a.vch_num,a.tor from clients_mst a " +
                            "inner join purchases pc on pc.acode=a.vch_num and find_in_set(pc.client_unit_id,a.client_unit_id)=1 " + condi + "" +
                            " where substr(a.vch_num,0,3)='203' and find_in_set(a.client_unit_id,'" + unitid_mst + "')=1 and a.type='BCD'";
                            break;
                        case "ITEM":
                            if (param1 != null) { if (param1 != "") where = " and (it.icode || pc.type || pc.po_no || to_char(it.vch_date, 'yyyymmdd') || it.type) not in ('" + param1.Trim().TrimEnd(',').Replace(",", "','").Trim().ToString() + "')"; }
                            condi = " and pc.type='" + sgen.GetSession(MyGuid, "BDPOTYPE").ToString() + "' and cl.vch_num = '" + sgen.GetSession(MyGuid, "PRTY_ID").ToString() + "' and pc.po_no in ('" + sgen.GetSession(MyGuid, "BDPONO").ToString().Trim().TrimEnd(',').Replace(",", "','").Trim().ToString() + "')";
                            cmd = "select (it.icode || pc.type || pc.po_no || to_char(it.vch_date, 'yyyymmdd') || it.type) as fstr,it.icode as Icode," +
                                "it.iname as Iname,it.cpartno Partno, hsn.master_name as HSN,nvl(pc.bal_qty, '0') as Order_qty,um.master_name as UOM,pc.po_no as po_num," +
                                "to_char(pc.po_date, '" + sgen.Getsqldateformat() + "') as podate from item it inner join (select a.vch_num as po_no,a.vch_date as po_date,a.icode,a.type, a.po_qty,a.acode," +
                                "a.client_unit_id,nvl(sum(a.mrn_qty), '0') as used_qty,a.po_qty - nvl(sum(a.mrn_qty), '0') as bal_qty from(select pc.vch_num, pc.vch_date, pc.icode, " +
                                "pc.type, pc.qtyord as po_qty,nvl(to_number(iv.qtyrcvd),'0') as mrn_qty, pc.acode, pc.client_unit_id from purchases pc left join itransaction iv on iv.pono = pc.vch_num" +
                                " and iv.type IN('02', '03') and pc.icode = iv.icode and iv.client_unit_id = pc.client_unit_id where substr(pc.type, 1, 1) = '5' and pc.client_unit_id = '" + unitid_mst + "' " +
                                "union all select pc.vch_num, pc.vch_date, pc.icode, pc.type, pc.qtyord as po_qty,nvl(to_number(ds.col10),'0') as sdl_qty, pc.acode, pc.client_unit_id from purchases pc " +
                                "left join kc_tab ds on ds.col13 = pc.vch_num and pc.icode = ds.col5 and ds.col20 = 'BPS' and ds.client_unit_id = pc.client_unit_id where substr(pc.type, 1, 1) = '5'" +
                                " and pc.client_unit_id = '" + unitid_mst + "' union all select pc.vch_num, pc.vch_date, pc.icode, pc.type, pc.qtyord as po_qty,nvl(to_number(iv.col11), '0') as mrn_qty,pc.acode, " +
                                "pc.client_unit_id from purchases pc left join enx_tab iv on iv.col12 = pc.vch_num and iv.col7 = pc.icode and " +
                                "iv.type = 'POR' and iv.client_unit_id = pc.client_unit_id where substr(pc.type,1,1)= '5' and pc.client_unit_id = '" + unitid_mst + "') a group by a.vch_num, a.icode, a.type,a.po_qty, a.vch_date,a.acode,a.client_unit_id) pc on pc.icode = it.icode inner join" +
                                " clients_mst cl on cl.vch_num = pc.acode and cl.type = 'BCD' and substr(cl.vch_num,0,3)= '203' and cl.client_unit_id = pc.client_unit_id left join master_setting um" +
                                " on um.master_id = it.uom and um.type = 'UMM' and find_in_set(um.client_unit_id, it.client_unit_id) = 1" +
                                " left join master_setting hsn on hsn.master_id = it.hsno and hsn.type = 'HSN' and " +
                                "find_in_set(hsn.client_unit_id, it.client_unit_id)= 1 where it.type = 'IT' and " +
                                "find_in_set(it.client_unit_id,'" + unitid_mst + "')= 1 " + condi + "";
                            break;
                    }
                    break;
                #endregion
                #region vd_po
                case "vd_po":
                    switch (btnval.ToUpper())
                    {
                        case "INPO":
                            cmd = "select distinct (p.client_id||p.client_unit_id||p.vch_num||to_char(p.vch_date,'yyyymmdd')||p.type) as fstr,p.vch_num PO_Number," +
                            "to_char(p.vch_date,'" + sgen.Getsqldateformat() + "') PO_Date,c.c_name as Party_Name,c.c_gstin as Party_Gstin,c.addr Address from purchases p " +
                            "inner join clients_mst c on p.acode = c.vch_num and find_in_set(c.client_unit_id,p.client_unit_id)=1 and c.type = 'BCD' and substr(c.vch_num,0,3)='203' " +
                            "inner join user_details u on u.emp_id=c.vch_num and u.type='CPR' and u.std_type='002' and u.vch_num='" + userid_mst + "' " +
                            "where p.client_unit_id = '" + unitid_mst + "' and p.type in ('50','52','54') and p.vdst='P' and to_date(to_char(p.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between " +
                            "to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "')";
                            break;
                        case "ACPO":
                            cmd = "select distinct (p.client_id||p.client_unit_id||p.vch_num||to_char(p.vch_date,'yyyymmdd')||p.type) as fstr,p.vch_num PO_Number," +
                            "to_char(p.vch_date,'" + sgen.Getsqldateformat() + "') PO_Date,c.c_name as Party_Name,c.c_gstin as Party_Gstin,c.addr Address from purchases p " +
                            "inner join clients_mst c on p.acode = c.vch_num and find_in_set(c.client_unit_id,p.client_unit_id)=1 and c.type = 'BCD' and substr(c.vch_num,0,3)='203' " +
                            "inner join user_details u on u.emp_id=c.vch_num and u.type='CPR' and u.std_type='002' and u.vch_num='" + userid_mst + "' " +
                            "where p.client_unit_id = '" + unitid_mst + "' and p.type in ('50','52','54') and p.vdst='A' and to_date(to_char(p.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between " +
                            "to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "')";
                            break;
                        case "REJPO":
                            cmd = "select distinct (p.client_id||p.client_unit_id||p.vch_num||to_char(p.vch_date,'yyyymmdd')||p.type) as fstr,p.vch_num PO_Number," +
                            "to_char(p.vch_date,'" + sgen.Getsqldateformat() + "') PO_Date,c.c_name as Party_Name,c.c_gstin as Party_Gstin,c.addr Address from purchases p " +
                            "inner join clients_mst c on p.acode = c.vch_num and find_in_set(c.client_unit_id,p.client_unit_id)=1 and c.type = 'BCD' and substr(c.vch_num,0,3)='203' " +
                            "inner join user_details u on u.emp_id=c.vch_num and u.type='CPR' and u.std_type='002' and u.vch_num='" + userid_mst + "' " +
                            "where p.client_unit_id = '" + unitid_mst + "' and p.type in ('50','52','54') and p.vdst='R' and to_date(to_char(p.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between " +
                            "to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "')";
                            break;
                        case "PORATE":
                            cmd = "select (client_id||client_unit_id||vch_num||to_char(vch_date,'yyyymmdd')||type) fstr,vch_num Doc_No," +
                                "to_char(convert_tz(vch_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') Doc_Date,type,irate " +
                                "from purchases where icode='" + param1 + "' and type in ('50','52','54') and client_unit_id='" + unitid_mst + "'";
                            break;
                    }
                    break;
                #endregion
                #region vd_dis
                case "vd_dis":
                    switch (btnval.ToUpper())
                    {
                        case "VIEW":
                        case "EDIT":
                            cmd = "select (p.client_id||p.client_unit_id||p.vch_num||to_char(p.vch_date,'yyyymmdd')||p.type) as fstr,p.vch_num," +
                           "to_char(p.vch_date,'" + sgen.Getsqldateformat() + "') vch_date,p.acode Vendor_Code,p.price_term Vehicle_No,p.tmode Driver_Name," +
                           "p.tname Driver_Contact,p.indno Billno,to_char(p.inddate,'" + sgen.Getsqldateformat() + "') Billdate,p.col1 Dispatch_No," +
                           "to_char(p.date1,'" + sgen.Getsqldateformat() + "') Dispatch_Date from purchases p " +
                           "where p.client_unit_id = '" + unitid_mst + "' and p.type='VDS'";
                            break;
                        case "ITEM":
                            if (param1 != "") where = " and (p.client_id||p.client_unit_id||p.vch_num||to_char(p.vch_date,'yyyymmdd')||p.type||p.icode) not in ('" + param1.Trim().TrimEnd(',').Replace(",", "','").Trim().ToString() + "')";
                            cmd = "select (p.client_id||p.client_unit_id||p.vch_num||to_char(p.vch_date,'yyyymmdd')||p.type||p.icode) fstr,p.vch_num PO_Number," +
                            "to_char(p.vch_date,'" + sgen.Getsqldateformat() + "') PO_Date,p.icode ItemCode,p.iname ItemName,p.cpartno PartNo,p.uom UOM," +
                            "p.qtystk Stock_Qty,p.qtyind Indent_Qty,p.qtyord Order_Qty,to_char(p.dlv_date,'" + sgen.Getsqldateformat() + "') Delivery_Date," +
                            "p.indno IndentNo,to_char(p.inddate,'" + sgen.Getsqldateformat() + "') Indent_Date from purchases p " +
                            "where p.client_unit_id = '" + unitid_mst + "' and p.type in ('50','52','54') and to_date(to_char(p.vch_date,'ddMMyyyy'),'ddMMyyyy') between " +
                            "to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "') and " +
                            "p.acode='" + param2.Trim() + "'";
                            break;
                    }
                    break;
                    #endregion
            }
            sgen.open_grid(title, cmd, sgen.Make_int(seektype));
            ViewBag.pageurl = sgen.GetSession(MyGuid, "pageurl").ToString();
        }
        #endregion
        //================== purchase order============
        #region
        public ActionResult purchase_order()
        {
            FillMst();
            chkRef();
            List<Tmodelmain> model = new List<Tmodelmain>();
            Tmodelmain tm1 = new Tmodelmain();
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            if (mid_mst.Trim().Equals("28002.1")) { ViewBag.Title = "PURCHASE ORDER"; }
            if (mid_mst.Trim().Equals("22001.4")) { tm1.Col9 = "VOUCHER TEMPLATE"; }
            if (mid_mst.Trim().Equals("7020.8")) { tm1.Col9 = "ICARD TEMPLATE"; }
            tm1.Col10 = mid_mst.Trim();
            tm1.Col11 = MyGuid.Trim();
            sgen.SetSession(MyGuid, "TR_MID", mid_mst.Trim());
            model.Add(tm1);
            return View(model);
        }
        //[HttpPost]
        //public ActionResult purchase_order(List<Tmodelmain> model, string command)
        //{
        //    userCode = sgen.GetCookie(MyGuid, "userCode");
        //    tab_name = "emp_master";
        //    tab_name1 = "file_tab";
        //    type_desc = "Employee Detail";
        //    type = "EMP";
        //    DataTable dtr = new DataTable();
        //    if (dtr.Rows.Count < 1)
        //    {
        //        DataRow dr = dtr.NewRow();
        //        dtr.Columns.Add("temp", typeof(string));
        //        dr["temp"] = "temp";
        //        dtr.Rows.Add(dr);
        //    }
        //    if (model[0].Col10.Trim().Equals("28002.1"))
        //    {
        //        if (command == "PO-1") { sgen.open_report_bydt_xml(userCode, dtr, "po1", "Purchase Order 1"); }
        //        if (command == "PO-2") { sgen.open_report_bydt_xml(userCode, dtr, "po2", "Purchase Order 2"); }
        //    }
        //    if (model[0].Col10.Trim().Equals("22001.4"))
        //    {
        //        if (command == "General Voucher") { sgen.open_report_bydt_xml(userCode, dtr, "gen_vou", "General Voucher"); }
        //        if (command == "Receipt Voucher") { sgen.open_report_bydt_xml(userCode, dtr, "rec_vou", "Receipt Voucher"); }
        //    }
        //    if (model[0].Col10.Trim().Equals("7020.8"))
        //    {
        //        if (command == "ICard 1") { sgen.open_report_bydt_xml(userCode, dtr, "Student_IC1", "ICard 1"); }
        //        if (command == "ICard 2") { sgen.open_report_bydt_xml(userCode, dtr, "Student_IC2", "ICard 2"); }
        //        if (command == "ICard 3") { sgen.open_report_bydt_xml(userCode, dtr, "Student_IC3", "ICard 3"); }
        //        if (command == "ICard 4") { sgen.open_report_bydt_xml(userCode, dtr, "Student_IC4", "ICard 4"); }
        //    }
        //    ViewBag.scripCall = "OpenSingle('../../../../../erp/schoolReports_Rpts/Filter_Report_xml.aspx','95%','900px','Reports');";
        //    return View(model);
        //}
        #endregion
        //=================indent===============================
        #region indent_required
        public ActionResult indent_req()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<SelectListItem> mod2 = new List<SelectListItem>();
            model = getload(model);
            model[0].Col9 = "INDENT";
            model[0].Col10 = "purchases";
            model[0].Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            model[0].Col12 = "66";
            model[0].Col14 = mid_mst.Trim();
            model[0].Col15 = MyGuid.Trim();
            model[0].TList1 = mod1;
            model[0].SelectedItems1 = new string[] { "" };
            model[0].TList2 = mod2;
            model[0].SelectedItems2 = new string[] { "" };
            DataTable dt = sgen.getdata(userCode, "select '0' as  SNo ,'-' as Icode,'-' as IName,'-' PartNo,'-'  HSN,'-' as UOM,'0' as  Qty_in_stock,'0' pn_ind_qty,'0' pn_po_qty,'0' as Qty_Req,'0' as Exp_Val_Perunit,'-' Required_date,'-' as Remark from dual");
            model[0].dt1 = dt;
            sgen.SetSession(MyGuid, "dtbase", dt);
            //model.Add(tm1);
            return View(model);
        }
        public List<Tmodelmain> newindent_req(List<Tmodelmain> model)
        {
            var tm = model.FirstOrDefault();
            //model = getnew(model);
            sgen.SetSession(MyGuid, "EDMODE", "N");
            ViewBag.vnew = "disabled='disabled'";
            ViewBag.vedit = "disabled='disabled'";
            ViewBag.vsave = "";
            ViewBag.vsavenew = "";
            ViewBag.scripCall = "enableForm();";
            mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
            vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
            model[0].Col17 = "1";
            model[0].Col18 = vch_num;
            model[0].Col13 = "Save";
            model[0].Col100 = "Save & New";
            model[0].Col121 = "S";
            model[0].Col122 = "<u>S</u>ave";
            model[0].Col123 = "Save & Ne<u>w</u>";
            model[0].Col20 = sgen.server_datetime_local(userCode);
            model[0].Col26 = "1";
            #region Dropdwon
            List<SelectListItem> mod1 = new List<SelectListItem>();
            model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
            TempData[MyGuid + "_TList1"] = model[0].TList1;
            if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
            List<SelectListItem> mod2 = new List<SelectListItem>();
            model[0].TList2 = (List<SelectListItem>)TempData[MyGuid + "_TList2"];
            TempData[MyGuid + "_TList2"] = model[0].TList2;
            if (tm.SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
            #endregion
            #region getdept
            TempData[MyGuid + "_Tlist1"] = model[0].TList1 = mod1 = cmd_Fun.dept(userCode, clientid_mst, unitid_mst);
            #endregion
            #region getdesig
            TempData[MyGuid + "_Tlist2"] = model[0].TList2 = mod2 = cmd_Fun.desig(userCode, unitid_mst);
            #endregion

            //model[0].Col13 = "Save";
            //model[0].Col100 = "Save & New";
            return model;
        }
        [HttpPost]
        public ActionResult indent_req(List<Tmodelmain> model, string command, string hfrow, string hftable)
        {
            try
            {
                FillMst(model[0].Col15);
                DataTable dtm = sgen.settable(hftable);
                model[0].dt1 = dtm;
                var tm = model.FirstOrDefault();
                List<SelectListItem> mod1 = new List<SelectListItem>();
                List<SelectListItem> mod2 = new List<SelectListItem>();
                model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
                model[0].TList2 = (List<SelectListItem>)TempData[MyGuid + "_TList2"];
                TempData[MyGuid + "_TList1"] = model[0].TList1;
                TempData[MyGuid + "_TList2"] = model[0].TList2;
                if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
                if (tm.SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
                DataTable dt = new DataTable();
                if (command == "New")
                {
                    model = newindent_req(model);
                }
                else if (command == "Cancel")
                {
                    return CancelFun(model);
                }
                else if (command == "Callback")
                {
                    if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
                    model = CallbackFun(btnval, "N", "indent_req", "Purchase", model);
                    if (btnval.Equals("PRINT"))
                    {
                        //ViewBag.scripCall = "disableForm()'";
                        dt = (DataTable)sgen.GetSession(MyGuid, "dtbase");
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col13 = "Save",
                            Col15 = tm.Col15,
                            Col14 = tm.Col14,
                            Col100 = "Save & New",
                            Col121 = "S",
                            Col122 = "<u>S</u>ave",
                            Col123 = "Save & Ne<u>w</u>",
                            Col9 = "CREATE INDENT",
                            TList1 = mod1,
                            SelectedItems1 = new string[] { "" },
                            TList2 = mod2,
                            SelectedItems2 = new string[] { "" },
                            dt1 = dt
                        });
                    }
                    else
                    {
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                    }
                }
                else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
                {
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime(userCode);
                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        isedit = true;
                        vch_num = model[0].Col18;
                    }
                    else
                    {
                        isedit = false;
                        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                        model[0].Col26 = "1";
                    }
                    //DataTable dataTable = sgen.getdata(userCode, "select  * from " + model[0].Col10.Trim() + "  where  1=2");
                    DataTable dataTable = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    for (int i = 0; i < model[0].dt1.Rows.Count; i++)
                    {
                        DataRow dr = dataTable.NewRow();
                        dr["vch_num"] = vch_num;
                        dr["type"] = model[0].Col12;
                        dr["vch_date"] = sgen.Savedate(model[0].Col20, false);
                        dr["deptno"] = model[0].SelectedItems1.FirstOrDefault();
                        dr["deptname"] = model[0].TList1.SingleOrDefault(w => w.Value == model[0].SelectedItems1.FirstOrDefault()).Text;
                        dr["desig"] = model[0].SelectedItems2.FirstOrDefault();
                        dr["reqby"] = model[0].Col22;
                        dr["totremark"] = model[0].Col21;
                        dr["icode"] = model[0].dt1.Rows[i]["Icode"].ToString();
                        dr["iname"] = model[0].dt1.Rows[i]["IName"].ToString();
                        dr["cpartno"] = model[0].dt1.Rows[i]["partno"].ToString();
                        dr["hsno"] = model[0].dt1.Rows[i]["HSN"].ToString();
                        dr["uom"] = model[0].dt1.Rows[i]["UOM"].ToString();
                        dr["qtystk"] = model[0].dt1.Rows[i]["Qty_in_stock"].ToString();
                        dr["qtyord"] = model[0].dt1.Rows[i]["Qty_Req"].ToString();
                        dr["iamount"] = model[0].dt1.Rows[i]["EXP_VAL_PERUNIT"].ToString();
                        dr["TCREMARK"] = model[0].dt1.Rows[i]["pn_ind_qty"].ToString();
                        dr["TNAME"] = model[0].dt1.Rows[i]["pn_po_qty"].ToString();
                        dr["iremark"] = model[0].dt1.Rows[i]["Remark"].ToString();
                        dr["dlv_date"] = sgen.Make_date_S(model[0].dt1.Rows[i]["Required_date"].ToString());
                        dr["ver"] = model[0].Col26;
                        dr["approved"] = "N";
                        dr["rec_id"] = "0";
                        if (isedit)
                        {
                            dr["client_id"] = model[0].Col1.Trim();
                            dr["client_unit_id"] = model[0].Col2.Trim();
                            dr["ent_by"] = model[0].Col3;
                            dr["ent_date"] = model[0].Col4;
                            dr["edit_by"] = userid_mst;
                            dr["edit_date"] = currdate;
                        }
                        else
                        {
                            dr["client_id"] = clientid_mst;
                            dr["client_unit_id"] = unitid_mst;
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
                    ViewBag.vsavenew = "disabled='disabled'";
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
                            model = newindent_req(model);
                            ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                        }
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col13 = "Save",
                            Col100 = "Save & New",
                            Col121 = "S",
                            Col122 = "<u>S</u>ave",
                            Col123 = "Save & Ne<u>w</u>",
                            Col9 = tmodel.Col9,
                            Col10 = tmodel.Col10,
                            Col11 = tmodel.Col11,
                            Col12 = tmodel.Col12,
                            Col14 = tmodel.Col14,
                            Col15 = tmodel.Col15,
                            Col18 = tmodel.Col18,
                            Col20 = tmodel.Col20,
                            TList1 = mod1,
                            SelectedItems1 = new string[] { "" },
                            TList2 = mod2,
                            SelectedItems2 = new string[] { "" },
                            dt1 = (DataTable)sgen.GetSession(MyGuid, "dtbase")
                        });
                    }
                    //if (Result == true)
                    //{
                    //    dt = (DataTable)Session["dtbase"];
                    //    model = new List<Tmodelmain>();
                    //    model = new List<Tmodelmain>();comman
                    //    model.Add(new Tmodelmain
                    //    {
                    //        Col13 = "Save",
                    //        Col100 = "Save & New",
                    //        Col9 = tmodel.Col9,
                    //        Col10 = tmodel.Col10,
                    //        Col11 = tmodel.Col11,
                    //        Col12 = tmodel.Col12,
                    //        Col14 = tmodel.Col14,
                    //        Col15 = tmodel.Col15,
                    //        TList1 = mod1,
                    //        SelectedItems1 = new string[] { "" },
                    //        TList2 = mod2,
                    //        SelectedItems2 = new string[] { "" },
                    //        dt1 = dt
                    //    });
                    //    //ViewBag.scripCall = "showmsgJS(1, 'Record Saved Successfully', 1);disableForm();";
                    //    ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                    //}
                }
                else if (command == "-")
                {
                    if (model[0].dt1.Rows.Count > 1) model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                    else model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "dtbase");
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vsavenew = "";
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
        //===================purchase order with indent==============================
        #region Purchase_order_wid_indent
        public ActionResult po_withindent()
        {
            FillMst();
            chkRef();
            ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.vsave = "disabled='disabled'";
            ViewBag.vsavenew = "disabled='disabled'";
            ViewBag.vcalc = "disabled='disabled'";
            ViewBag.scripCall = "disableForm();";
            List<Tmodelmain> model = new List<Tmodelmain>();
            Tmodelmain tm1 = new Tmodelmain();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<SelectListItem> mod2 = new List<SelectListItem>();
            List<SelectListItem> mod3 = new List<SelectListItem>();
            List<SelectListItem> mod4 = new List<SelectListItem>();
            List<SelectListItem> mod5 = new List<SelectListItem>();
            List<SelectListItem> mod6 = new List<SelectListItem>();
            List<SelectListItem> mod7 = new List<SelectListItem>();
            List<SelectListItem> mod8 = new List<SelectListItem>();
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            tm1.Col10 = "purchases";
            tm1.Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            tm1.Col13 = "Save";
            tm1.Col100 = "Save & New";
            tm1.Col121 = "S";
            tm1.Col122 = "<u>S</u>ave";
            tm1.Col123 = "Save & Ne<u>w</u>";
            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = MyGuid.Trim();
            tm1.TList1 = mod1;
            tm1.SelectedItems1 = new string[] { "" };
            tm1.TList2 = mod2;
            tm1.SelectedItems2 = new string[] { "" };
            tm1.TList3 = mod3;
            tm1.SelectedItems3 = new string[] { "" };
            tm1.TList4 = mod4;
            tm1.SelectedItems4 = new string[] { "" };
            tm1.TList5 = mod5;
            tm1.SelectedItems5 = new string[] { "" };
            tm1.TList6 = mod6;
            tm1.SelectedItems6 = new string[] { "" };
            tm1.TList7 = mod7;
            tm1.SelectedItems7 = new string[] { "" };
            tm1.TList8 = mod8;
            tm1.SelectedItems8 = new string[] { "" };
            TempData[MyGuid + "_TList1"] = tm1.TList1;
            TempData[MyGuid + "_TList2"] = tm1.TList2;
            TempData[MyGuid + "_TList3"] = tm1.TList3;
            TempData[MyGuid + "_TList4"] = tm1.TList4;
            TempData[MyGuid + "_TList5"] = tm1.TList5;
            TempData[MyGuid + "_TList6"] = tm1.TList6;
            TempData[MyGuid + "_TList7"] = tm1.TList7;
            TempData[MyGuid + "_TList8"] = tm1.TList8;
            tm1.Col18 = sgen.seekval(userCode, "select unit_gstin_no from company_unit_profile where cup_id='" + unitid_mst + "'", "unit_gstin_no");
            tm1.Col99 = sgen.seekval(userCode, "select ll_act from company_unit_profile where company_profile_id='" + clientid_mst + "' and cup_id='" + unitid_mst + "'", "ll_act");//local_currency
            sgen.SetSession(MyGuid, "LOCALCUR", tm1.Col99);
            tm1.Col65 = sgen.seekval(userCode, "select param1 from controls where type='CTL' and m_module3='-' and id='000017'", "param1");            
            model.Add(tm1);
            ModelState.Clear();
            return View(model);
        }
        [HttpPost]
        public ActionResult po_withindent(List<Tmodelmain> model, string command, string hfrow, string hftable, HttpPostedFileBase upd1)
        {
            try
            {
                FillMst(model[0].Col15);
                DataTable dtm = sgen.settable(hftable);
                if (dtm.Rows.Count > 0)
                {
                    if (model[0].Col12 != null)
                    {
                        if (model[0].Col12.Trim().Equals("50") || model[0].Col12.Trim().Equals("52") || model[0].Col12.Trim().Equals("54"))
                        {
                            if (model[0].Col29 == "002") model[0].dt1 = dtm;
                            else model[0].dt2 = dtm;
                        }
                    }
                    //else model[0].dt1 = dtm;
                }
                var tm = model.FirstOrDefault();
                #region ddl          
                List<SelectListItem> mod1 = new List<SelectListItem>();
                List<SelectListItem> mod2 = new List<SelectListItem>();
                List<SelectListItem> mod3 = new List<SelectListItem>();
                List<SelectListItem> mod4 = new List<SelectListItem>();
                List<SelectListItem> mod5 = new List<SelectListItem>();
                List<SelectListItem> mod6 = new List<SelectListItem>();
                List<SelectListItem> mod7 = new List<SelectListItem>();
                List<SelectListItem> mod8 = new List<SelectListItem>();
                TempData[MyGuid + "_TList1"] = model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
                TempData[MyGuid + "_TList2"] = model[0].TList2 = (List<SelectListItem>)TempData[MyGuid + "_TList2"];
                TempData[MyGuid + "_TList3"] = model[0].TList3 = (List<SelectListItem>)TempData[MyGuid + "_TList3"];
                TempData[MyGuid + "_TList4"] = model[0].TList4 = (List<SelectListItem>)TempData[MyGuid + "_TList4"];
                TempData[MyGuid + "_TList5"] = model[0].TList5 = (List<SelectListItem>)TempData[MyGuid + "_TList5"];
                TempData[MyGuid + "_TList6"] = model[0].TList6 = (List<SelectListItem>)TempData[MyGuid + "_TList6"];
                TempData[MyGuid + "_TList7"] = model[0].TList7 = (List<SelectListItem>)TempData[MyGuid + "_TList7"];
                TempData[MyGuid + "_TList8"] = model[0].TList8 = (List<SelectListItem>)TempData[MyGuid + "_TList8"];
                if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
                if (tm.SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
                if (tm.SelectedItems3 == null) model[0].SelectedItems3 = new string[] { "" };
                if (tm.SelectedItems4 == null) model[0].SelectedItems4 = new string[] { "" };
                if (tm.SelectedItems5 == null) model[0].SelectedItems5 = new string[] { "" };
                if (tm.SelectedItems6 == null) model[0].SelectedItems6 = new string[] { "" };
                if (tm.SelectedItems7 == null) model[0].SelectedItems7 = new string[] { "" };
                if (tm.SelectedItems8 == null) model[0].SelectedItems8 = new string[] { "" };
                #endregion
                if (command == "Cancel")
                {
                    return CancelFun(model);
                }
                else if (command == "Callback")
                {
                    model = StartCallback(model);
                    ViewBag.vcalc = "";
                    if (btnval == "PRINT")
                    {
                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        ViewBag.vsavenew = "disabled='disabled'";
                    }
                }
                else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
                {
                    int cnt = 0;
                    DataTable dtf = new DataTable();
                    Satransaction sat1 = new Satransaction(userCode, MyGuid);
                    string currdate = sgen.server_datetime(userCode);
                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        isedit = true;
                        vch_num = model[0].Col16;
                        vch_num = model[0].Col47;
                    }
                    else
                    {
                        isedit = false;
                        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                        model[0].Col41 = "1";
                        model[0].Col82 = "P";
                    }
                    if (model[0].dt1.Rows.Count > 0)
                    {
                        if (model[0].dt1.Rows[0]["icode"].ToString().Trim().Equals("-") || model[0].dt1.Rows[0]["icode"].ToString().Trim().Equals(""))
                        {
                            ViewBag.scripCall += "showmsgJS(1,'Please Select item in grid-1', 2);";
                            ViewBag.vnew = "disabled='disabled'";
                            ViewBag.vedit = "disabled='disabled'";
                            ViewBag.vsave = "";
                            ViewBag.vsavenew = "";
                            return View(model);
                        }
                    }
                    else if (model[0].dt2.Rows.Count > 0)
                    {
                        if (model[0].dt2.Rows[0]["icode"].ToString().Trim().Equals("-") || model[0].dt2.Rows[0]["icode"].ToString().Trim().Equals(""))
                        {
                            ViewBag.scripCall += "showmsgJS(1,'Please Select item in grid-1', 2);";
                            ViewBag.vnew = "disabled='disabled'";
                            ViewBag.vedit = "disabled='disabled'";
                            ViewBag.vsave = "";
                            ViewBag.vsavenew = "";
                            return View(model);
                        }
                    }
                    DataTable dataTable = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    string vch_date = sgen.Savedate(model[0].Col48, true);
                    var ptype = model[0].Col29;
                    if (ptype.Trim().Equals("002"))
                    {
                        cnt = model[0].dt1.Rows.Count;
                        dtf = model[0].dt1;
                    }
                    else
                    {
                        cnt = model[0].dt2.Rows.Count;
                        dtf = model[0].dt2;
                    }
                    for (int i = 0; i < cnt; i++)
                    {
                        DataRow dr = dataTable.NewRow();
                        dr["rec_id"] = "0";
                        dr["type"] = model[0].Col12;
                        dr["vch_num"] = vch_num.Trim();
                        dr["vch_date"] = vch_date;
                        dr["pur_type"] = model[0].Col29;
                        dr["acode"] = model[0].Col49;
                        dr["pay_term"] = model[0].SelectedItems1.FirstOrDefault();
                        dr["price_term"] = model[0].SelectedItems2.FirstOrDefault();
                        dr["tmode"] = model[0].SelectedItems3.FirstOrDefault();
                        dr["tname"] = model[0].Col56;
                        dr["pkins"] = model[0].Col57;
                        dr["tcstatus"] = model[0].Col58;
                        dr["tcremark"] = model[0].Col59;
                        dr["gstcr"] = model[0].Col67;
                        dr["gstcrremark"] = model[0].Col68;
                        dr["col1"] = model[0].Col70;//no1                     
                        dr["col2"] = model[0].Col71;//no2                   
                        dr["col3"] = model[0].Col72;//no3                   
                        dr["date1"] = sgen.Make_date_S(model[0].Col73);
                        dr["date2"] = sgen.Make_date_S(model[0].Col74);
                        dr["date3"] = sgen.Make_date_S(model[0].Col75);
                        dr["col4"] = model[0].Col76;//txt1                       
                        dr["col5"] = model[0].Col77;//txt2                  
                        dr["col6"] = model[0].Col78;//txt3                  
                        dr["shipfrom"] = model[0].Col50;
                        dr["shipto"] = model[0].Col51;
                        dr["pref"] = model[0].Col60;
                        dr["taxre"] = model[0].Col61;
                        dr["paymode"] = model[0].SelectedItems4.FirstOrDefault();
                        dr["insby"] = model[0].SelectedItems5.FirstOrDefault();
                        dr["delterm"] = model[0].SelectedItems6.FirstOrDefault();
                        dr["doccur"] = model[0].SelectedItems7.FirstOrDefault();
                        dr["shpfval"] = model[0].Col62;
                        dr["shptval"] = model[0].Col63;
                        //dt====
                        dr["icode"] = dtf.Rows[i][2].ToString();
                        dr["iname"] = dtf.Rows[i][3].ToString();
                        dr["cpartno"] = dtf.Rows[i][4].ToString();
                        dr["hsno"] = dtf.Rows[i][5].ToString();
                        dr["uom"] = dtf.Rows[i][6].ToString();
                        dr["taxrate"] = dtf.Rows[i][7].ToString().Split('%')[0];
                        dr["qtystk"] = dtf.Rows[i][8].ToString();
                        if (ptype.Trim().Equals("002"))
                        {
                            dr["qtyord"] = dtf.Rows[i][9].ToString();
                            dr["irate"] = dtf.Rows[i][10].ToString();
                            dr["discrate"] = dtf.Rows[i][11].ToString();
                            dr["iamount"] = dtf.Rows[i][12].ToString();
                            dr["taxamt"] = dtf.Rows[i][13].ToString();
                            dr["discamt"] = dtf.Rows[i][14].ToString();
                            dr["lineamount"] = dtf.Rows[i][15].ToString();
                            dr["dlv_date"] = sgen.Make_date_S(dtf.Rows[i][16].ToString());
                            dr["iremark"] = dtf.Rows[i][17].ToString();
                        }
                        else
                        {
                            dr["qtyind"] = dtf.Rows[i][9].ToString();
                            dr["qtyord"] = dtf.Rows[i][10].ToString();
                            dr["qtybal"] = dtf.Rows[i][11].ToString();
                            dr["irate"] = dtf.Rows[i][12].ToString();
                            dr["discrate"] = dtf.Rows[i][13].ToString();
                            dr["iamount"] = dtf.Rows[i][14].ToString();
                            dr["taxamt"] = dtf.Rows[i][15].ToString();
                            dr["discamt"] = dtf.Rows[i][16].ToString();
                            dr["lineamount"] = dtf.Rows[i][17].ToString();
                            dr["dlv_date"] = sgen.Make_date_S(dtf.Rows[i][18].ToString());
                            dr["iremark"] = dtf.Rows[i][19].ToString();
                            dr["indno"] = dtf.Rows[i][20].ToString();
                            dr["inddate"] = sgen.Make_date_S(dtf.Rows[i][21].ToString());
                        }
                        //======
                        dr["totremark"] = model[0].Col31;
                        dr["cond"] = model[0].Col32;
                        dr["basicamt"] = model[0].Col33;
                        dr["igst"] = model[0].Col34;
                        dr["cgst"] = model[0].Col35;
                        dr["sgst"] = model[0].Col36;
                        dr["gamt"] = model[0].Col37;
                        dr["gigst"] = model[0].Col38;
                        dr["gcgst"] = model[0].Col39;
                        dr["gsgst"] = model[0].Col40;
                        dr["IS_WARNT"] = model[0].Col84;
                        dr["WRTDATE"] = sgen.Make_date_S(model[0].Col85);
                        dr["gsgst"] = model[0].Col40;
                        dr["netamt"] = model[0].Col69;
                        dr["ver"] = model[0].Col41;
                        if (model[0].Col65 == "Y") dr["approved"] = "Y";
                        else dr["approved"] = "N";
                        dr["vdst"] = model[0].Col82;
                        if (isedit)
                        {
                            dr["client_id"] = model[0].Col1.Trim();
                            dr["client_unit_id"] = model[0].Col2.Trim();
                            dr["ent_by"] = model[0].Col3;
                            dr["ent_date"] = model[0].Col4;
                            dr["edit_by"] = userid_mst;
                            dr["edit_date"] = currdate;
                        }
                        else
                        {
                            dr["client_id"] = clientid_mst;
                            dr["client_unit_id"] = unitid_mst;
                            dr["ent_by"] = userid_mst;
                            dr["ent_date"] = currdate;
                            dr["edit_by"] = "-";
                            dr["edit_date"] = currdate;
                        }
                        dataTable.Rows.Add(dr);
                    }
                    bool Result = sgen.Update_data_uncommit2(userCode, dataTable, model[0].Col10.Trim(), model[0].Col8, isedit, sat1);
                    if (Result == true)
                    {                       
                        DataTable dten = cmd_Fun.GetStructure(userCode, "enx_tab");
                        if (model[0].LTM1 != null && model[0].LTM1.Count > 0)
                        {
                            res = false;
                            #region dr
                            for (int k = 0; k < model[0].LTM1.Count; k++)
                            {
                                DataRow dr = dten.NewRow();
                                dr["type"] = model[0].Col12;
                                dr["vch_num"] = vch_num.Trim();
                                dr["vch_date"] = sgen.Savedate(model[0].Col48, true);
                                //dt====
                                dr["r_no"] = model[0].LTM1[k].Col14;
                                dr["col1"] = model[0].LTM1[k].Col15;//chrgid
                                dr["col2"] = model[0].LTM1[k].Col16;//chrgamt
                                dr["col3"] = model[0].LTM1[k].Col13;//chrgrtid
                                dr["col4"] = model[0].LTM1[k].Col17;//igstrt
                                dr["col5"] = model[0].LTM1[k].Col18;//igstamt
                                dr["col6"] = model[0].LTM1[k].Col19;//cgstrt
                                dr["col7"] = model[0].LTM1[k].Col20;//cgstamt
                                dr["col8"] = model[0].LTM1[k].Col21;//sgstrt
                                dr["col9"] = model[0].LTM1[k].Col22;//sgstamt
                                dr["col10"] = model[0].LTM1[k].Col25;//per
                                                                     //======                            
                                dr["rec_id"] = "0";
                                // shiv
                                if (isedit)
                                {
                                    dr["client_id"] = model[0].Col1.Trim();
                                    dr["client_unit_id"] = model[0].Col2.Trim();
                                    dr["ent_by"] = model[0].Col3;
                                    dr["ent_date"] = model[0].Col4;
                                    dr["edit_by"] = userid_mst;
                                    dr["edit_date"] = currdate;
                                }
                                else
                                {
                                    dr["client_id"] = clientid_mst;
                                    dr["client_unit_id"] = unitid_mst;
                                    dr["ent_by"] = userid_mst;
                                    dr["ent_date"] = currdate;
                                    dr["edit_by"] = "-";
                                    dr["edit_date"] = currdate;
                                }
                                dten.Rows.Add(dr);
                            }
                            #endregion
                            res = sgen.Update_data_uncommit2(userCode, dten, "enx_tab", model[0].Col8, isedit, sat1);
                            if (!res)
                            {
                                sat1.Rollback();
                                ViewBag.vnew = "disabled='disabled'";
                                ViewBag.vedit = "disabled='disabled'";
                                ViewBag.vsave = "";
                                ViewBag.vsavenew = "";
                                ViewBag.scripCall += "showmsgJS(1, 'Calc error', 0);";
                                ModelState.Clear();
                                return View(model);
                            }
                        }
                        sat1.Commit();

                        if (model[0].Col87 == "Y")
                        {
                            #region
                            string vdt = sgen.Make_date(model[0].Col48).ToString("yyyyMMdd");
                            string url = clientid_mst + unitid_mst + vch_num + vdt + model[0].Col12;
                            string term = "", doc_contrl_no = "", invtype = "", rptname = "";
                            invtype = sgen.getstring(userCode, "select distinct p.type as master_id from purchases p where (p.client_id||p.client_unit_id||p.vch_num||to_char(p.vch_date,'yyyymmdd')||p.type) = '" + url + "'");
                            DataTable dtterm = sgen.getdata(userCode, "select k.col73 tandc,k.col1 doc_control_no,k.col5 rptid,fm.CLIENT_NAME rptname from kc_tab k " +
                                "inner join master_setting fm on fm.master_id = k.col5 and fm.type = 'IRF' and fm.classid = '84' and fm.sectionid = '" + invtype + "' " +
                                //"inner join master_setting fm on fm.master_id = k.col5 and fm.type = 'IRF' and fm.client_unit_id = '" + unitid_mst + "' and fm.classid = '83' and fm.sectionid = '" + invtype + "' " +
                                "where k.type='PRN' and k.client_unit_id = '" + unitid_mst + "'");
                            if (dtterm.Rows.Count > 0)
                            {
                                term = dtterm.Rows[0]["tandc"].ToString();
                                rptname = dtterm.Rows[0]["rptname"].ToString();
                                //model[0].Col30 = "po6";
                                doc_contrl_no = dtterm.Rows[0]["doc_control_no"].ToString();
                            }
                            else
                            {
                                if (invtype == "45") rptname = "po5";
                                else rptname = "po5";
                            }
                            mq = "select a.acode,nvl(um.master_name,'0') UOM,nvl(hsn.master_name,'0') hsn,a.cond,a.vch_num,a.icode,a.taxrate,to_number(a.qtystk ) qtystk," +
                                "to_number(a.qtyord) qtyord,to_number(a.irate) irate,to_number(a.discrate) discrate,to_number(a.iamount) iamount,to_number(a.taxamt) taxamt," +
                                "to_number(a.discamt) discamt,to_number(a.lineamount) lineamount,a.iremark,a.indno,a.totremark,to_number(a.basicamt) basicamt,to_number(a.gdisc) gdisc," +
                                "to_number(a.gfreight) gfreight,to_number(a.insurance) insurance,to_number(a.ginstlchrg) ginstlchrg,to_number(a.gserchrg) gserchrg,to_number(a.gamc) gamc," +
                                "to_number(a.gloadchrg) gloadchrg,to_number(a.gothrchrg) gothrchrg,to_number(a.gtaxamt) gtaxamt,to_number(a.igst) igst,to_number(a.cgst) cgst," +
                                "to_number(a.sgst) sgst,to_number(a.gamt) gamt,to_number(a.netamt) netamt,to_char(convert_tz(a.vch_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') vch_date1," +
                                "to_char(convert_tz(a.dlv_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') dlv_date1,to_char(convert_tz(a.inddate, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') inddate1," +
                                "c.c_name as name,c.cpname as cont_per,c.cpcont,c.cpemail,c.c_gstin as gstin,c.addr,c.tor,c.org_mobile,nvl(s.c_name, '0') sf_name,nvl(s.c_gstin, '0') sf_gstin,nvl(s.addr, '0') as sf_addr," +
                                "nvl(s.tor, '0') as sf_tor,nvl(s.cpcont, '0') sf_contact,nvl(t.c_name, '0') st_name,nvl(t.c_gstin, '0') tf_gstin,nvl(t.addr, '0') as st_addr," +
                                "nvl(t.tor, '0') as st_tor,nvl(t.cpcont, '0') st_contact,nvl(py.master_name, '0') py_term,nvl(pr.master_name, '0') pr_term,nvl(mt.master_name, '0') trans," +
                                "i.cpartno as partno,i.iname as item_name,'" + term + "' terms_cond,'" + doc_contrl_no + "' doc_control_no" +
                                ",nvl(ct.master_name,'-') as currency,nvl(ins.master_name,'-') as ins_by,a.shptval,a.shpfval,dl.master_name as delivery_term,nvl(a.tcremark,'-')" +
                                ",(case when a.IS_WARNT='Y' then to_char(a.WRTDATE,'" + sgen.Getsqldateformat() + "') when a.IS_WARNT='N' then 'No Warranty' else '-' end) warranty from purchases a " +
                                "inner join item i on a.icode = i.icode  and i.type = 'IT' and find_in_set(a.client_unit_id, i.client_unit_id)=1 " +
                                "left join clients_mst c on c.vch_num = a.acode and c.type = 'BCD' and substr(c.vch_num,0,3)='203' and find_in_set(c.client_unit_id, a.client_unit_id)=1 " +
                                "left join clients_mst s on s.vch_num = a.shipfrom and s.type = 'BCD' and substr(s.vch_num,0,3)='203' and find_in_set(s.client_unit_id, a.client_unit_id)=1 " +
                                "left join clients_mst t on t.vch_num = a.shipto and t.type = 'BCD' and substr(t.vch_num,0,3)='203' and find_in_set(t.client_unit_id, a.client_unit_id)=1 " +
                                "left join master_setting py on a.pay_term = py.master_id and py.type = 'PTM' and find_in_set(a.client_unit_id, py.client_unit_id)= 1 " +
                                "left join master_setting pr on a.price_term = pr.master_id and pr.type = 'PRI' and find_in_set(a.client_unit_id, pr.client_unit_id)= 1 " +
                                "left join master_setting dl on a.delterm = dl.master_id and dl.type = 'DEL' and find_in_set(a.client_unit_id, dl.client_unit_id)= 1 " +
                                "left join master_setting mt on a.tmode = mt.master_id and mt.type = 'AMT' and find_in_set(a.client_unit_id, mt.client_unit_id)= 1 " +
                                "left join master_setting um on um.master_id = i.uom and um.type = 'UMM' and find_in_set(um.client_unit_id,'" + unitid_mst + "')= 1 " +
                                "left join master_setting hsn on hsn.master_id = i.hsno and hsn.type = 'HSN' and find_in_set(hsn.client_unit_id,'" + unitid_mst + "')= 1 " +
                                "left join master_setting ins on ins.master_id = a.insby and ins.type = 'K01' and find_in_set(ins.client_unit_id,'" + unitid_mst + "')= 1 " +
                                "left join master_setting ct on ct.master_id = a.doccur and ct.type = 'CTM' and find_in_set(ct.client_unit_id,'" + unitid_mst + "')= 1 " +
                                "where (a.client_id || a.client_unit_id || a.vch_num || to_char(a.vch_date, 'yyyymmdd') || a.type) = '" + url + "' and to_number(a.qtyord) > 0";
                            //for other charges"select OTH.col2 as othr_chrgamt,OTH.col4 as othr_igstrt,OTH.col5 AS othr_igst,OTH.col6 as othr_cgstrt,OTH.col7 AS othr_cgst,OTH.col8 as othr_sgstrt,OTH.col9 AS othr_sgst
                            //left join (select ch.master_name chrgname,en.col1,en.col2,en.col3,en.col4,en.col5,en.col6,en.col7,en.col8,en.col9,en.r_no from enx_tab en inner join master_setting ch on en.col1=ch.master_id and ch.type='MR0' and find_in_set(ch.client_unit_id,en.client_unit_id)=1 where (en.client_id||en.client_unit_id||en.vch_num||to_char(en.vch_date, 'yyyymmdd')|| en.type) = '" + URL + "') OTH on 1=1";
                            DataTable dtp = sgen.getdata(userCode, mq);
                            if (dtp.Rows.Count > 0)
                            {
                                mq1 = "select ch.master_name chrgname,en.col1,to_number(en.col2) col2,en.col3,en.col4,to_number(en.col5) col5,en.col6,to_number(en.col7) col7,en.col8,to_number(en.col9) col9,en.col10" +
                                    ",en.r_no from enx_tab en inner join master_setting ch on en.col1=ch.master_id and ch.type='MR0' and " +
                                    "find_in_set(ch.client_unit_id,en.client_unit_id)=1 where (en.client_id||en.client_unit_id||en.vch_num||to_char(en.vch_date, 'yyyymmdd')|| en.type) = '" + url + "'";
                                var dthsn = sgen.getdata(userCode, mq1);
                                DataSet dsh = new DataSet();
                                dthsn.TableName = "prepcur2";
                                dsh.Tables.Add(dthsn);
                                string xmlpath = System.Web.HttpContext.Current.Server.MapPath("~/") + "xmls/po_othrchrg.xml";
                                dsh.WriteXml(xmlpath, XmlWriteMode.WriteSchema);
                            }
                            #endregion
                            #region regular
                            ReportDocument rd = new ReportDocument();
                            {
                                DataTable dt = new DataTable();
                                DataTable dt1 = new DataTable();
                                DataTable dt2 = new DataTable();
                                DataTable dt3 = new DataTable();
                                DataSet ds = new DataSet();

                                dtp.TableName = "prepcur";
                                try { ds.Tables.Add(dtp.Copy()); }
                                catch (Exception ex) { }

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

                                    //string Rpt = (string)sgen.GetSession(MyGuid, "Report");///seek lgana h
                                    string Rpt = rptname;
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
                                    //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                    var streaminput = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                                    //Stream stream = sgen.PassPdf(streaminput, "Pass@123");                                

                                    try
                                    {
                                        string msgbody = "<b style='color: #3caee9; font-size: 20px'>PURCHASE ORDER</b><br /><hr style='border:1px solid black' /><p>Hi " +
                                        "<b>" + model[0].Col20.Trim() + "</b>,</p><p>Please check the below attached file :-</p>" +
                                        "<table style='font-weight:600'><tr><td>Email </td><td>: " + "demo" + " </td></tr>" +
                                        "<tr><td>Contact </td><td>: " + "demo" + " </td></tr></table><br />" +
                                        "<p>Thank you,<br />Administrator<br /></p><br /><p>Note:- Please do not reply to this mail as it is an unmonitered alias.</p>";
                                        MailMessage msg = new MailMessage();
                                        msg.From = new MailAddress("gsthelp001@gmail.com");
                                        msg.Bcc.Add("gsthelp001@gmail.com");
                                        msg.To.Add(model[0].Col86);
                                        msg.Subject = "Purchase Order";
                                        msg.Body = msgbody;
                                        msg.Attachments.Add(new Attachment(streaminput, "PO.pdf"));
                                        msg.IsBodyHtml = true;
                                        SmtpClient smtp = new SmtpClient();
                                        smtp.Host = "smtp.gmail.com";
                                        System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                                        NetworkCred.UserName = "gsthelp001@gmail.com";
                                        NetworkCred.Password = "srsoffice2";
                                        smtp.UseDefaultCredentials = true;
                                        smtp.Credentials = NetworkCred;
                                        smtp.Port = 587;
                                        smtp.EnableSsl = true;
                                        smtp.Send(msg);
                                        msg.Dispose();
                                    }
                                    catch (Exception ex) { }
                                }
                            }
                            #endregion
                        }

                        if (command.Trim().ToUpper().Equals("SAVE") || command.Trim().ToUpper().Equals("UPDATE"))
                        {
                            ViewBag.vnew = "";
                            ViewBag.vedit = "";
                            ViewBag.vsave = "disabled='disabled'";
                            ViewBag.vsavenew = "disabled='disabled'";
                            ViewBag.vcalc = "disabled='disabled'";
                            ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                        }
                        else if (command.Trim().ToUpper().Equals("SAVE & NEW") || command.Trim().ToUpper().Equals("UPDATE & NEW"))
                        {
                            ViewBag.vcalc = "disabled='disabled'";
                            Make_query("po_withindent", "Select Indent Type", "NEW", "1", "", "", MyGuid);
                            ViewBag.scripCall = "callFoo('Select Indent Type');";
                            ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";

                        }
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col10 = tm.Col10,
                            Col11 = tm.Col11,
                            Col13 = "Save",
                            Col100 = "Save & New",
                            Col121 = "S",
                            Col122 = "<u>S</u>ave",
                            Col123 = "Save & Ne<u>w</u>",
                            Col14 = tm.Col14,
                            Col15 = tm.Col15,
                            Col65 = tm.Col65,
                            TList1 = mod1,
                            TList2 = mod2,
                            TList3 = mod3,
                            TList4 = mod4,
                            TList5 = mod5,
                            TList6 = mod6,
                            TList7 = mod7,
                            TList8 = mod8,
                            SelectedItems1 = new string[] { "" },
                            SelectedItems2 = new string[] { "" },
                            SelectedItems3 = new string[] { "" },
                            SelectedItems4 = new string[] { "" },
                            SelectedItems5 = new string[] { "" },
                            SelectedItems6 = new string[] { "" },
                            SelectedItems7 = new string[] { "" },
                            SelectedItems8 = new string[] { "" },
                            dt1 = (DataTable)sgen.GetSession(MyGuid, "KPO_DT")
                        });
                    }
                    else
                    {
                        sat1.Rollback();
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                        ViewBag.scripCall += "mytoast('error', 'toast-top-right', 'Data Not Saved');";
                        ModelState.Clear();
                        return View(model);
                    }
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
                                        //if (c > dt.Columns.Count) break;                                                                 
                                    }
                                    i++;
                                    dt.Rows.Add(dr);
                                }
                            }
                        }
                        dt.Rows[0].Delete();
                        for (int k = 0; k < dt.Rows.Count; k++)
                        {
                            mq = sgen.seekval(userCode, "select icode from item where icode='" + dt.Rows[k]["icode"].ToString() + "' and type='IT' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'", "icode");
                            if (mq.Trim() == "0")
                            {
                                ViewBag.vnew = "disabled='disabled'";
                                ViewBag.vedit = "disabled='disabled'";
                                ViewBag.vsave = "";
                                ViewBag.vsavenew = "";
                                ViewBag.scripCall += "showmsgJS(1,'Icode: '" + dt.Rows[k]["icode"].ToString() + "' not found,please check.', 0);";
                                ModelState.Clear();
                                return View(model);
                            }
                        }
                        mq1 = "select '-' item,'0' Sno,i.icode,i.iname,i.cpartno partno,u.master_name uom,h.master_name hsn,h.group_name taxrate,'0' qty_in_stock," +
                            "'0' iamount,'0' taxamount,'0' lineamount from item i " +
                            "inner join  master_setting u on u.master_id = i.uom and u.type = 'UMM' and find_in_set(u.client_unit_id, i.client_unit_id)= 1 " +
                            "inner join  master_setting h on h.master_id = i.hsno and h.type = 'HSN' and find_in_set(h.client_unit_id, i.client_unit_id)= 1 " +
                            "where i.client_unit_id = '" + unitid_mst + "' and i.type = 'IT'";
                        DataTable dt2 = sgen.getdata(userCode, mq1);
                        if (dt2.Rows.Count > 0)
                        {
                            var results = from table1 in dt.AsEnumerable()
                                          join table2 in dt2.AsEnumerable() on table1["icode"] equals table2["icode"]
                                          select new
                                          {
                                              item = table2["item"],
                                              sno = table2["sno"],
                                              icode = table1["icode"],
                                              iname = table2["iname"],
                                              partno = table2["partno"],
                                              hsn = table2["hsn"],
                                              uom = table2["uom"],
                                              taxrate = table2["taxrate"],
                                              qty_in_stock = table2["qty_in_stock"],
                                              qty_ord = table1["qty_ord"],
                                              irate = table1["irate"],
                                              disc_rate = table1["disc_rate"],
                                              iamount = table2["iamount"],
                                              taxamount = table2["taxamount"],
                                              discamount = table1["discamount"],
                                              lineamount = table2["lineamount"],
                                              delivery_date = table1["delivery_date"],
                                              remark = table1["remark"],
                                          };
                            results.ToList();
                            DataTable dtk = sgen.ToDataTable(results.ToList());
                            model[0].dt1 = dtk;
                            ViewBag.vsave = "";
                            ViewBag.vimport = "";
                        }
                    }
                }
                else if (command == "-")
                {
                    mq0 = "select icode from itransaction where substr(type,1,1)='0' and pono='" + model[0].Col47 + "' and to_char(podate.'yyyyMMdd')='" + sgen.Make_date(model[0].Col48).ToString("yyyyMMdd") + "' " +
                        "and acode='" + model[0].Col49 + "' and icode='" + model[0].dt1.Rows[sgen.Make_int(hfrow)]["icode"].ToString() + "'";
                    mq0 = sgen.seekval(userCode, mq0, "icode");
                    if (mq0 != "0")
                    {
                        ViewBag.scripCall += "showmsgJS(3, 'This item cannot be deleted,mrn created already', 2);";
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                        ViewBag.vcalc = "";
                        return View(model);
                    }

                    //if (model[0].dt1 == null && model[0].dt2.Rows.Count > 1)
                    if (model[0].dt2.Rows.Count > 1)
                    {
                        model[0].dt2.Rows.RemoveAt(sgen.Make_int(hfrow));
                    }
                    else { model[0].dt2 = (DataTable)sgen.GetSession(MyGuid, "KPO_DT"); }

                    //if (model[0].dt2 == null && model[0].dt1.Rows.Count > 1)
                    if (model[0].dt1.Rows.Count > 1)
                    {
                        model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                    }
                    else { model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "KPO_DT"); }

                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vsavenew = "";
                    ViewBag.vcalc = "";
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
        public void podelrno(string value, string guid)
        {
            sgen = new sgenFun(guid);
            sgen.SetSession(guid, "podelrno", value);
        }
        [HttpPost]
        public ContentResult SetSession(List<Tmodelmain> Val, string Name)
        {
            sgen = new sgenFun(Val[0].Col15);
            sgen.SetSession(Val[0].Col15, Name, Val);
            return Content("");
        }
        [HttpGet]
        public FileResult potemp(List<Tmodelmain> model, string Myguid = "")
        {
            FillMst(Myguid);
            model = (List<Tmodelmain>)sgen.GetSession(Myguid, "model");
            DataTable dtl = new DataTable();
            mq = "SELECT '' Icode,'' Qty_ord,'' Irate,'' Disc_Rate,'' DiscAmount,'' Delivery_date,'' Remark from Dual";
            dtl = sgen.getdata(userCode, mq);
            string cg_com_name = sgen.GetCookie(MyGuid, "cg_com_name");
            Byte[] fileBytes = sgen.Exp_to_csv_new(dtl, "poitem", cg_com_name);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "poitem");
        }
        #endregion
        #region po_imp
        public ActionResult po_imp()
        {
            FillMst();
            chkRef();
            ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.vsave = "disabled='disabled'";
            ViewBag.vsavenew = "disabled='disabled'";
            ViewBag.vcalc = "disabled='disabled'";
            ViewBag.scripCall = "disableForm();";
            List<Tmodelmain> model = new List<Tmodelmain>();
            Tmodelmain tm1 = new Tmodelmain();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<SelectListItem> mod2 = new List<SelectListItem>();
            List<SelectListItem> mod3 = new List<SelectListItem>();
            List<SelectListItem> mod4 = new List<SelectListItem>();
            List<SelectListItem> mod5 = new List<SelectListItem>();
            List<SelectListItem> mod6 = new List<SelectListItem>();
            List<SelectListItem> mod7 = new List<SelectListItem>();
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            tm1.Col10 = "purchases";
            tm1.Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            tm1.Col13 = "Save";
            tm1.Col100 = "Save & New";
            tm1.Col121 = "S";
            tm1.Col122 = "<u>S</u>ave";
            tm1.Col123 = "Save & Ne<u>w</u>";
            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = MyGuid.Trim();
            tm1.TList1 = mod1;
            tm1.SelectedItems1 = new string[] { "" };
            tm1.TList2 = mod2;
            tm1.SelectedItems2 = new string[] { "" };
            tm1.TList3 = mod3;
            tm1.SelectedItems3 = new string[] { "" };
            tm1.TList4 = mod4;
            tm1.SelectedItems4 = new string[] { "" };
            tm1.TList5 = mod5;
            tm1.SelectedItems5 = new string[] { "" };
            tm1.TList6 = mod6;
            tm1.SelectedItems6 = new string[] { "" };
            tm1.TList7 = mod7;
            tm1.SelectedItems7 = new string[] { "" };
            TempData[MyGuid + "_TList1"] = tm1.TList1;
            TempData[MyGuid + "_TList2"] = tm1.TList2;
            TempData[MyGuid + "_TList3"] = tm1.TList3;
            TempData[MyGuid + "_TList4"] = tm1.TList4;
            TempData[MyGuid + "_TList5"] = tm1.TList5;
            TempData[MyGuid + "_TList6"] = tm1.TList6;
            TempData[MyGuid + "_TList7"] = tm1.TList7;
            tm1.Col18 = sgen.seekval(userCode, "select unit_gstin_no from company_unit_profile where cup_id='" + unitid_mst + "'", "unit_gstin_no");
            tm1.Col99 = sgen.seekval(userCode, "select ll_act from company_unit_profile where company_profile_id='" + clientid_mst + "' and cup_id='" + unitid_mst + "'", "ll_act");//local_currency
            sgen.SetSession(MyGuid, "LOCALCUR", tm1.Col99);
            model.Add(tm1);
            ModelState.Clear();
            return View(model);
        }
        [HttpPost]
        public ActionResult po_imp(List<Tmodelmain> model, string command, string hfrow, string hftable, HttpPostedFileBase upd1)
        {
            try
            {
                FillMst(model[0].Col15);
                DataTable dtm = sgen.settable(hftable);
                if (dtm.Rows.Count > 0)
                {
                    //if (model[0].Col12 != null)
                    //{
                    //    if (model[0].Col12.Trim().Equals("50") || model[0].Col12.Trim().Equals("52") || model[0].Col12.Trim().Equals("54"))
                    //    {
                    //        if (model[0].Col29 == "002") model[0].dt1 = dtm;
                    //        else model[0].dt2 = dtm;
                    //    }
                    //}
                    //else
                    model[0].dt1 = dtm;
                }
                var tm = model.FirstOrDefault();
                #region ddl          
                List<SelectListItem> mod1 = new List<SelectListItem>();
                List<SelectListItem> mod2 = new List<SelectListItem>();
                List<SelectListItem> mod3 = new List<SelectListItem>();
                List<SelectListItem> mod4 = new List<SelectListItem>();
                List<SelectListItem> mod5 = new List<SelectListItem>();
                List<SelectListItem> mod6 = new List<SelectListItem>();
                List<SelectListItem> mod7 = new List<SelectListItem>();
                TempData[MyGuid + "_TList1"] = model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
                TempData[MyGuid + "_TList2"] = model[0].TList2 = (List<SelectListItem>)TempData[MyGuid + "_TList2"];
                TempData[MyGuid + "_TList3"] = model[0].TList3 = (List<SelectListItem>)TempData[MyGuid + "_TList3"];
                TempData[MyGuid + "_TList4"] = model[0].TList4 = (List<SelectListItem>)TempData[MyGuid + "_TList4"];
                TempData[MyGuid + "_TList5"] = model[0].TList5 = (List<SelectListItem>)TempData[MyGuid + "_TList5"];
                TempData[MyGuid + "_TList6"] = model[0].TList6 = (List<SelectListItem>)TempData[MyGuid + "_TList6"];
                TempData[MyGuid + "_TList7"] = model[0].TList7 = (List<SelectListItem>)TempData[MyGuid + "_TList7"];
                if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
                if (tm.SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
                if (tm.SelectedItems3 == null) model[0].SelectedItems3 = new string[] { "" };
                if (tm.SelectedItems4 == null) model[0].SelectedItems4 = new string[] { "" };
                if (tm.SelectedItems5 == null) model[0].SelectedItems5 = new string[] { "" };
                if (tm.SelectedItems6 == null) model[0].SelectedItems6 = new string[] { "" };
                if (tm.SelectedItems7 == null) model[0].SelectedItems7 = new string[] { "" };
                #endregion
                if (command == "Cancel")
                {
                    return CancelFun(model);
                }
                else if (command == "Callback")
                {
                    model = StartCallback(model);
                    ViewBag.vcalc = "";
                }
                else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
                {
                    int cnt = 0;
                    DataTable dtf = new DataTable();
                    Satransaction sat1 = new Satransaction(userCode, MyGuid);
                    string currdate = sgen.server_datetime(userCode);
                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        isedit = true;
                        vch_num = model[0].Col16;
                        vch_num = model[0].Col47;
                    }
                    else
                    {
                        isedit = false;
                        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                    }
                    DataTable dataTable = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    string vch_date = sgen.Savedate(model[0].Col48, true);
                    var ptype = model[0].Col29;
                    if (ptype.Trim().Equals("002"))
                    {
                        cnt = model[0].dt1.Rows.Count;
                        dtf = model[0].dt1;
                    }
                    else
                    {
                        cnt = model[0].dt2.Rows.Count;
                        dtf = model[0].dt2;
                    }
                    for (int i = 0; i < cnt; i++)
                    {
                        DataRow dr = dataTable.NewRow();
                        dr["rec_id"] = "0";
                        dr["type"] = model[0].Col12;
                        dr["vch_num"] = vch_num.Trim();
                        dr["vch_date"] = vch_date;
                        dr["pur_type"] = model[0].Col29;
                        dr["acode"] = model[0].Col49;
                        dr["pay_term"] = model[0].SelectedItems1.FirstOrDefault();
                        dr["price_term"] = model[0].SelectedItems2.FirstOrDefault();
                        dr["tmode"] = model[0].SelectedItems3.FirstOrDefault();
                        dr["tname"] = model[0].Col56;
                        dr["pkins"] = model[0].Col57;
                        dr["tcstatus"] = model[0].Col58;
                        dr["tcremark"] = model[0].Col59;
                        dr["gstcr"] = model[0].Col67;
                        dr["gstcrremark"] = model[0].Col68;
                        dr["col1"] = model[0].Col70;//no1                     
                        dr["col2"] = model[0].Col71;//no2                   
                        dr["col3"] = model[0].Col72;//no3                   
                        dr["date1"] = sgen.Make_date_S(model[0].Col73);
                        dr["date2"] = sgen.Make_date_S(model[0].Col74);
                        dr["date3"] = sgen.Make_date_S(model[0].Col75);
                        dr["col4"] = model[0].Col76;//txt1                       
                        dr["col5"] = model[0].Col77;//txt2                  
                        dr["col6"] = model[0].Col78;//txt3                  
                        dr["shipfrom"] = model[0].Col50;
                        dr["shipto"] = model[0].Col51;
                        dr["pref"] = model[0].Col60;
                        dr["taxre"] = model[0].Col61;
                        dr["paymode"] = model[0].SelectedItems4.FirstOrDefault();
                        dr["insby"] = model[0].SelectedItems5.FirstOrDefault();
                        dr["delterm"] = model[0].SelectedItems6.FirstOrDefault();
                        dr["doccur"] = model[0].SelectedItems7.FirstOrDefault();
                        dr["shpfval"] = model[0].Col62;
                        dr["shptval"] = model[0].Col63;
                        //dt====
                        dr["icode"] = dtf.Rows[i][2].ToString();
                        dr["iname"] = dtf.Rows[i][3].ToString();
                        dr["cpartno"] = dtf.Rows[i][4].ToString();
                        dr["hsno"] = dtf.Rows[i][5].ToString();
                        dr["uom"] = dtf.Rows[i][6].ToString();
                        dr["taxrate"] = dtf.Rows[i][7].ToString().Split('%')[0];
                        dr["qtystk"] = dtf.Rows[i][8].ToString();
                        if (ptype.Trim().Equals("002"))
                        {
                            dr["qtyord"] = dtf.Rows[i][9].ToString();
                            dr["irate"] = dtf.Rows[i][10].ToString();
                            dr["discrate"] = dtf.Rows[i][11].ToString();
                            dr["iamount"] = dtf.Rows[i][12].ToString();
                            dr["taxamt"] = dtf.Rows[i][13].ToString();
                            dr["discamt"] = dtf.Rows[i][14].ToString();
                            dr["lineamount"] = dtf.Rows[i][15].ToString();
                            dr["dlv_date"] = sgen.Make_date_S(dtf.Rows[i][16].ToString());
                            dr["iremark"] = dtf.Rows[i][17].ToString();
                        }
                        else
                        {
                            dr["qtyind"] = dtf.Rows[i][9].ToString();
                            dr["qtyord"] = dtf.Rows[i][10].ToString();
                            dr["qtybal"] = dtf.Rows[i][11].ToString();
                            dr["irate"] = dtf.Rows[i][12].ToString();
                            dr["discrate"] = dtf.Rows[i][13].ToString();
                            dr["iamount"] = dtf.Rows[i][14].ToString();
                            dr["taxamt"] = dtf.Rows[i][15].ToString();
                            dr["discamt"] = dtf.Rows[i][16].ToString();
                            dr["lineamount"] = dtf.Rows[i][17].ToString();
                            dr["dlv_date"] = sgen.Make_date_S(dtf.Rows[i][18].ToString());
                            dr["iremark"] = dtf.Rows[i][19].ToString();
                            dr["indno"] = dtf.Rows[i][20].ToString();
                            dr["inddate"] = sgen.Make_date_S(dtf.Rows[i][21].ToString());
                        }
                        //======
                        dr["totremark"] = model[0].Col31;
                        dr["cond"] = model[0].Col32;
                        dr["basicamt"] = model[0].Col33;
                        dr["igst"] = model[0].Col34;
                        dr["cgst"] = model[0].Col35;
                        dr["sgst"] = model[0].Col36;
                        dr["gamt"] = model[0].Col37;
                        dr["gigst"] = model[0].Col38;
                        dr["gcgst"] = model[0].Col39;
                        dr["gsgst"] = model[0].Col40;
                        dr["netamt"] = model[0].Col69;
                        if (isedit)
                        {
                            dr["client_id"] = model[0].Col1.Trim();
                            dr["client_unit_id"] = model[0].Col2.Trim();
                            dr["ent_by"] = model[0].Col3;
                            dr["ent_date"] = model[0].Col4;
                            dr["edit_by"] = userid_mst;
                            dr["edit_date"] = currdate;
                        }
                        else
                        {
                            dr["client_id"] = clientid_mst;
                            dr["client_unit_id"] = unitid_mst;
                            dr["ent_by"] = userid_mst;
                            dr["ent_date"] = currdate;
                            dr["edit_by"] = "-";
                            dr["edit_date"] = currdate;
                        }
                        dataTable.Rows.Add(dr);
                    }
                    bool Result = sgen.Update_data_uncommit2(userCode, dataTable, model[0].Col10.Trim(), model[0].Col8, isedit, sat1);
                    if (Result == true)
                    {
                        DataTable dten = cmd_Fun.GetStructure(userCode, "enx_tab");
                        if (model[0].LTM1 != null && model[0].LTM1.Count > 0)
                        {
                            res = false;
                            #region dr
                            for (int k = 0; k < model[0].LTM1.Count; k++)
                            {
                                DataRow dr = dten.NewRow();
                                dr["type"] = model[0].Col12;
                                dr["vch_num"] = vch_num.Trim();
                                dr["vch_date"] = sgen.Savedate(model[0].Col48, true);
                                //dt====
                                dr["r_no"] = model[0].LTM1[k].Col14;
                                dr["col1"] = model[0].LTM1[k].Col15;//chrgid
                                dr["col2"] = model[0].LTM1[k].Col16;//chrgamt
                                dr["col3"] = model[0].LTM1[k].Col13;//chrgrtid
                                dr["col4"] = model[0].LTM1[k].Col17;//igstrt
                                dr["col5"] = model[0].LTM1[k].Col18;//igstamt
                                dr["col6"] = model[0].LTM1[k].Col19;//cgstrt
                                dr["col7"] = model[0].LTM1[k].Col20;//cgstamt
                                dr["col8"] = model[0].LTM1[k].Col21;//sgstrt
                                dr["col9"] = model[0].LTM1[k].Col22;//sgstamt
                                                                    //======                            
                                dr["rec_id"] = "0";
                                // shiv
                                if (isedit)
                                {
                                    dr["client_id"] = model[0].Col1.Trim();
                                    dr["client_unit_id"] = model[0].Col2.Trim();
                                    dr["ent_by"] = model[0].Col3;
                                    dr["ent_date"] = model[0].Col4;
                                    dr["edit_by"] = userid_mst;
                                    dr["edit_date"] = currdate;
                                }
                                else
                                {
                                    dr["client_id"] = clientid_mst;
                                    dr["client_unit_id"] = unitid_mst;
                                    dr["ent_by"] = userid_mst;
                                    dr["ent_date"] = currdate;
                                    dr["edit_by"] = "-";
                                    dr["edit_date"] = currdate;
                                }
                                dten.Rows.Add(dr);
                            }
                            #endregion
                            res = sgen.Update_data_uncommit2(userCode, dten, "enx_tab", model[0].Col8, isedit, sat1);
                            if (!res)
                            {
                                sat1.Rollback();
                                ViewBag.vnew = "disabled='disabled'";
                                ViewBag.vedit = "disabled='disabled'";
                                ViewBag.vsave = "";
                                ViewBag.vsavenew = "";
                                ViewBag.scripCall += "showmsgJS(1, 'Calc error', 0);";
                                ModelState.Clear();
                                return View(model);
                            }
                        }
                        sat1.Commit();
                        if (command.Trim().ToUpper().Equals("SAVE") || command.Trim().ToUpper().Equals("UPDATE"))
                        {
                            ViewBag.vnew = "";
                            ViewBag.vedit = "";
                            ViewBag.vsave = "disabled='disabled'";
                            ViewBag.vsavenew = "disabled='disabled'";
                            ViewBag.vcalc = "disabled='disabled'";
                            ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                        }
                        else if (command.Trim().ToUpper().Equals("SAVE & NEW") || command.Trim().ToUpper().Equals("UPDATE & NEW"))
                        {
                            ViewBag.vcalc = "disabled='disabled'";
                            Make_query("po_withindent", "Select Indent Type", "NEW", "1", "", "", MyGuid);
                            ViewBag.scripCall = "callFoo('Select Indent Type');";
                            ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";

                        }
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col10 = tm.Col10,
                            Col11 = tm.Col11,
                            Col13 = "Save",
                            Col100 = "Save & New",
                            Col121 = "S",
                            Col122 = "<u>S</u>ave",
                            Col123 = "Save & Ne<u>w</u>",
                            Col14 = tm.Col14,
                            Col15 = tm.Col15,
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
                            dt1 = (DataTable)sgen.GetSession(MyGuid, "KPO_DT")
                        });
                    }
                    else
                    {
                        sat1.Rollback();
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                        ViewBag.scripCall += "mytoast('error', 'toast-top-right', 'Data Not Saved');";
                        ModelState.Clear();
                        return View(model);
                    }
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
                                        //if (c > dt.Columns.Count) break;                                                                 
                                    }
                                    i++;
                                    dt.Rows.Add(dr);
                                }
                            }
                        }
                        dt.Rows[0].Delete();
                        for (int k = 0; k < dt.Rows.Count; k++)
                        {
                            mq = sgen.seekval(userCode, "select icode from item where icode='" + dt.Rows[k]["icode"].ToString() + "' and type='IT' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'", "icode");
                            if (mq.Trim() == "0")
                            {
                                ViewBag.vnew = "disabled='disabled'";
                                ViewBag.vedit = "disabled='disabled'";
                                ViewBag.vsave = "";
                                ViewBag.vsavenew = "";
                                ViewBag.scripCall += "showmsgJS(1,'Icode: '" + dt.Rows[k]["icode"].ToString() + "' not found,please check.', 0);";
                                ModelState.Clear();
                                return View(model);
                            }
                        }
                        mq1 = "select '-' item,'0' Sno,i.icode,i.iname,i.cpartno partno,u.master_name uom,h.master_name hsn,h.group_name taxrate,'0' qty_in_stock," +
                            "'0' iamount,'0' taxamount,'0' lineamount from item i " +
                            "inner join  master_setting u on u.master_id = i.uom and u.type = 'UMM' and find_in_set(u.client_unit_id, i.client_unit_id)= 1 " +
                            "inner join  master_setting h on h.master_id = i.hsno and h.type = 'HSN' and find_in_set(h.client_unit_id, i.client_unit_id)= 1 " +
                            "where i.client_unit_id = '" + unitid_mst + "' and i.type = 'IT'";
                        DataTable dt2 = sgen.getdata(userCode, mq1);
                        if (dt2.Rows.Count > 0)
                        {
                            var results = from table1 in dt.AsEnumerable()
                                          join table2 in dt2.AsEnumerable() on table1["icode"] equals table2["icode"]
                                          select new
                                          {
                                              item = table2["item"],
                                              sno = table2["sno"],
                                              icode = table1["icode"],
                                              iname = table2["iname"],
                                              partno = table2["partno"],
                                              hsn = table2["hsn"],
                                              uom = table2["uom"],
                                              taxrate = table2["taxrate"],
                                              qty_in_stock = table2["qty_in_stock"],
                                              qty_ord = table1["qty_ord"],
                                              irate = table1["irate"],
                                              disc_rate = table1["disc_rate"],
                                              iamount = table2["iamount"],
                                              taxamount = table2["taxamount"],
                                              discamount = table1["discamount"],
                                              lineamount = table2["lineamount"],
                                              delivery_date = table1["delivery_date"],
                                              remark = table1["remark"],
                                          };
                            results.ToList();
                            DataTable dtk = sgen.ToDataTable(results.ToList());
                            model[0].dt1 = dtk;
                            ViewBag.vsave = "";
                            ViewBag.vimport = "";
                        }
                    }
                }
                else if (command == "-")
                {
                    if (model[0].dt1.Rows.Count > 1) model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                    else { model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "KPO_DT"); }
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vsavenew = "";
                    ViewBag.vcalc = "";
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
        //=================== vendor_detail ===================//
        #region vendordetail
        //public ActionResult vendor_detail()
        //{
        //    FillMst();
        //    chkRef();
        //    ViewBag.vnew = "";
        //    ViewBag.vedit = "";
        //    ViewBag.vsave = "disabled='disabled'";
        //    ViewBag.scripCall = "disableForm();";
        //    List<Tmodelmain> model = new List<Tmodelmain>();
        //    List<SelectListItem> mod1 = new List<SelectListItem>();
        //    Tmodelmain tm1 = new Tmodelmain();
        //    mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
        //    MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
        //    if (mid_mst == "21008.1")
        //    {
        //        tm1.Col9 = "CUSTOMER DETAIL";
        //        tm1.Col12 = "BCD"; // BILLING CUSTOMER DETAILS               
        //    }
        //    else if (mid_mst.Trim().Equals("28005.1") || mid_mst.Trim().Equals("9008.6"))
        //    {
        //        tm1.Col9 = "VENDOR DETAIL";
        //        tm1.Col12 = "PVD"; // PURCHASE VENDOR DETAIL AND PAYROLL
        //    }
        //    else if (mid_mst == "41005.8")
        //    {
        //        tm1.Col9 = "VENDOR DETAIL";
        //        tm1.Col12 = "PVD"; // BANQUET HALL CUSTOMER DETAILS                
        //    }
        //    tm1.Col10 = "clients_mst";
        //    tm1.Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
        //    tm1.Col13 = "Save";
        //    tm1.Col14 = mid_mst.Trim();
        //    tm1.Col15 = MyGuid.Trim();
        //    tm1.Col39 = "Choose File";
        //    tm1.Col40 = "Choose File";
        //    tm1.Col41 = "Choose File";
        //    tm1.Col42 = "Choose File";
        //    tm1.Col43 = "Choose File";
        //    sgen.SetSession(MyGuid, "VD_TYPEMST", tm1.Col12);
        //    tm1.TList1 = mod1;
        //    tm1.SelectedItems1 = new string[] { "" };
        //    tm1.TList2 = mod1;
        //    tm1.SelectedItems2 = new string[] { "" };
        //    tm1.TList3 = mod1;
        //    tm1.SelectedItems3 = new string[] { "" };
        //    tm1.TList4 = mod1;
        //    tm1.SelectedItems4 = new string[] { "" };
        //    tm1.TList5 = mod1;
        //    tm1.SelectedItems5 = new string[] { "" };
        //    tm1.TList6 = mod1;
        //    tm1.SelectedItems6 = new string[] { "" };
        //    tm1.TList7 = mod1;
        //    tm1.SelectedItems7 = new string[] { "" };
        //    tm1.LTM1 = new List<Tmodelmain>();
        //    tm1.Col45 = "N";//chk acct type
        //    mq = "select mod_id from com_module where mod_id='22000' and com_code='" + userCode + "'";
        //    mq1 = sgen.seekval(userCode, mq, "mod_id");
        //    if (!mq1.Trim().Equals("0")) tm1.Col45 = "Y";
        //    model.Add(tm1);
        //    return View(model);
        //}
        //[HttpPost]
        //public ActionResult vendor_detail(List<Tmodelmain> model, string command, HttpPostedFileBase upd1, HttpPostedFileBase upd2, HttpPostedFileBase upd3, HttpPostedFileBase upd4, HttpPostedFileBase upd5)
        //{
        //    string ctype1 = "", ctype2 = "", ctype3 = "", ctype4 = "", ctype5 = "",
        //        finalpath1 = "", finalpath2 = "", finalpath3 = "", finalpath4 = "", finalpath5 = "", path1 = "", path2 = "", path3 = "", path4 = "", path5 = "",
        //        fileName1 = "", fileName2 = "", fileName3 = "", fileName4 = "", fileName5 = "", encpath1 = "", encpath2 = "", encpath3 = "", encpath4 = "", encpath5 = "",
        //    iscontractor = "N";
        //    FillMst(model[0].Col15);
        //    var tm = model.FirstOrDefault();
        //    DataTable dt = new DataTable();
        //    #region dropdown      
        //    List<SelectListItem> mod1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
        //    List<SelectListItem> mod2 = (List<SelectListItem>)TempData[MyGuid + "_TList2"];
        //    List<SelectListItem> mod3 = (List<SelectListItem>)TempData[MyGuid + "_TList3"];
        //    List<SelectListItem> mod4 = (List<SelectListItem>)TempData[MyGuid + "_TList4"];
        //    List<SelectListItem> mod5 = (List<SelectListItem>)TempData[MyGuid + "_TList5"];
        //    List<SelectListItem> mod6 = (List<SelectListItem>)TempData[MyGuid + "_TList6"];
        //    List<SelectListItem> mod7 = (List<SelectListItem>)TempData[MyGuid + "_TList7"];
        //    model[0].TList1 = mod1;
        //    model[0].TList2 = mod2;
        //    model[0].TList3 = mod3;
        //    model[0].TList4 = mod4;
        //    model[0].TList5 = mod5;
        //    model[0].TList6 = mod6;
        //    model[0].TList7 = mod7;
        //    TempData[MyGuid + "_TList1"] = model[0].TList1;
        //    TempData[MyGuid + "_TList2"] = model[0].TList2;
        //    TempData[MyGuid + "_TList3"] = model[0].TList3;
        //    TempData[MyGuid + "_TList4"] = model[0].TList4;
        //    TempData[MyGuid + "_TList5"] = model[0].TList5;
        //    TempData[MyGuid + "_TList6"] = model[0].TList6;
        //    TempData[MyGuid + "_TList7"] = model[0].TList7;
        //    if (model[0].SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
        //    if (model[0].SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
        //    if (model[0].SelectedItems3 == null) model[0].SelectedItems3 = new string[] { "" };
        //    if (model[0].SelectedItems4 == null) model[0].SelectedItems4 = new string[] { "" };
        //    if (model[0].SelectedItems5 == null) model[0].SelectedItems5 = new string[] { "" };
        //    if (model[0].SelectedItems6 == null) model[0].SelectedItems6 = new string[] { "" };
        //    if (model[0].SelectedItems7 == null) model[0].SelectedItems7 = new string[] { "" };
        //    #endregion
        //    if (command == "New")
        //    {
        //        sgen.SetSession(MyGuid, "EDMODE", "N");
        //        ViewBag.vnew = "disabled='disabled'";
        //        ViewBag.vedit = "disabled='disabled'";
        //        ViewBag.vsave = "";
        //        ViewBag.scripCall = "enableForm();";
        //        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
        //        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
        //        model[0].Col17 = vch_num;
        //        model[0].Col13 = "Save";
        //        model[0].Col35 = "R";
        //        model[0].Col44 = "Active";
        //        mod1 = new List<SelectListItem>();
        //        mod2 = new List<SelectListItem>();
        //        mod3 = new List<SelectListItem>();
        //        mod4 = new List<SelectListItem>();
        //        mod5 = new List<SelectListItem>();
        //        mod6 = new List<SelectListItem>();
        //        mod7 = new List<SelectListItem>();
        //        #region MASTERS
        //        #region country
        //        mq = "select distinct country_name,alpha_2 from country_state order by country_name";
        //        dt = sgen.getdata(userCode, mq);
        //        if (dt.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in dt.Rows)
        //            {
        //                mod1.Add(new SelectListItem { Text = dr["country_name"].ToString(), Value = dr["alpha_2"].ToString() });
        //            }
        //        }
        //        TempData[MyGuid + "_Tlist1"] = mod1;
        //        TempData[MyGuid + "_Tlist2"] = mod2;
        //        #endregion
        //        #region party type
        //        mod3 = cmd_Fun.partytype(userCode, unitid_mst);
        //        TempData[MyGuid + "_Tlist3"] = mod3;
        //        #endregion
        //        #region party location - fix
        //        mod4.Add(new SelectListItem { Text = "Domestic", Value = "001" });
        //        mod4.Add(new SelectListItem { Text = "Overseas", Value = "002" });
        //        TempData[MyGuid + "_Tlist4"] = mod4;
        //        #endregion
        //        #region acc type
        //        mod5 = cmd_Fun.acctypevdm(userCode, unitid_mst);
        //        TempData[MyGuid + "_Tlist5"] = mod5;
        //        #endregion
        //        #region currency type
        //        mod6 = cmd_Fun.curname(userCode, unitid_mst);
        //        TempData[MyGuid + "_Tlist6"] = mod6;
        //        #endregion
        //        #region bank name
        //        mod7 = cmd_Fun.bank(userCode, unitid_mst);
        //        TempData[MyGuid + "_Tlist7"] = mod7;
        //        #endregion
        //        #endregion
        //        model[0].TList1 = mod1;
        //        model[0].TList2 = mod2;
        //        model[0].TList3 = mod3;
        //        model[0].TList4 = mod4;
        //        model[0].TList5 = mod5;
        //        model[0].TList6 = mod6;
        //        model[0].TList7 = mod7;
        //    }
        //    else if (command == "Cancel")
        //    {
        //        return CancelFun(model);
        //        //return RedirectToAction("vendor_detail", new { @m_id = EncryptDecrypt.Encrypt(model[0].Col15), @mid = EncryptDecrypt.Encrypt(model[0].Col14) });
        //    }
        //    else if (command == "Callback")
        //    {
        //        model = StartCallback(model);
        //        //if (Session["delid"] != null) { btnval = "FDEL"; }
        //        //else if (Session["btnval"] != null) btnval = Session["btnval"].ToString();
        //        //model = CallbackFun(btnval, "N", "vendor_detail", "Purchase", model);
        //        //ViewBag.vnew = "disabled='disabled'";
        //        //ViewBag.vedit = "disabled='disabled'";
        //        //ViewBag.vsave = "";
        //    }
        //    else if (command == "Save" || command == "Update")
        //    {
        //        var tmodel = model.FirstOrDefault();
        //        string currdate = sgen.server_datetime(userCode);
        //        if (model[0].Chk1 == true) isgstr = "Y";
        //        if (model[0].Chk2 == true) iscontractor = "Y";
        //        type = model[0].Col12;
        //        if (model[0].Col44.Trim() == "Inactive")
        //        {
        //            type = "DD" + type;
        //            status = "N";
        //        }
        //        else status = "Y";
        //        DataTable dtstr = new DataTable();
        //        //dtstr = sgen.getdata(userCode, "select * from " + model[0].Col10 + " WHERE 1=2");
        //        dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
        //        if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
        //        {
        //            mq1 = " and vch_num<>'" + tmodel.Col17.Trim() + "'";
        //            isedit = true;
        //            vch_num = tmodel.Col17.Trim();
        //        }
        //        else
        //        {
        //            mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where type='" + model[0].Col12 + "' " + model[0].Col11 + "  ";
        //            vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
        //            isedit = false;
        //        }
        //        #region dtstr
        //        DataRow dr = dtstr.NewRow();
        //        dr["vch_num"] = vch_num.Trim();
        //        dr["vch_date"] = currdate;
        //        dr["type"] = type.Trim();
        //        dr["status"] = status.Trim();
        //        dr["c_name"] = model[0].Col18;
        //        dr["addr"] = model[0].Col22;
        //        dr["pincode"] = model[0].Col23;
        //        dr["type_desc"] = model[0].Col25;//search text
        //        dr["cpname"] = model[0].Col26;//cont per name
        //        dr["cpcont"] = model[0].Col27;//contno
        //        dr["cpaltcont"] = model[0].Col28;//alt contno
        //        dr["cpemail"] = model[0].Col29;//emailid
        //        dr["cpaddr"] = model[0].Col30;//cpaddr
        //        dr["cpdesig"] = model[0].Col31;//cpdesig
        //        dr["lsrno"] = model[0].Col32;//leisure value
        //        dr["bnkaddr"] = model[0].Col33;//bank address
        //        dr["micrno"] = model[0].Col34;
        //        dr["cfrm"] = "Y";//confirm
        //        dr["c_gstin"] = "Not Registered";
        //        var cgstin = model[0].Col24;
        //        if (isgstr == "Y" && cgstin != null)
        //        {
        //            dr["isgstr"] = isgstr;
        //            dr["c_gstin"] = cgstin;
        //            dr["tor"] = model[0].Col35;
        //        }
        //        dr["panno"] = model[0].Col36;
        //        dr["msmeno"] = model[0].Col37;
        //        dr["tanno"] = model[0].Col38;
        //        dr["country"] = model[0].SelectedItems1.FirstOrDefault();//country
        //        dr["state"] = model[0].SelectedItems2.FirstOrDefault();//state
        //        dr["ptype"] = model[0].SelectedItems3.FirstOrDefault();//ptype
        //        dr["ploc"] = model[0].SelectedItems4.FirstOrDefault();//ploc                
        //        dr["acctype"] = model[0].SelectedItems5.FirstOrDefault();
        //        dr["curtype"] = model[0].SelectedItems6.FirstOrDefault();
        //        dr["bnk"] = model[0].SelectedItems7.FirstOrDefault();
        //        dr["swftcd"] = model[0].Col50;
        //        dr["contr"] = iscontractor.Trim();
        //        dr["acctno"] = model[0].Col51;
        //        dr["ifsc"] = model[0].Col52;
        //        if (isedit)
        //        {
        //            dr["client_id"] = model[0].Col1.Trim();
        //            dr["client_unit_id"] = model[0].Col2.Trim();
        //            dr["vch_num"] = model[0].Col17;
        //            dr["rec_id"] = model[0].Col7;
        //            dr["ent_by"] = model[0].Col3;
        //            dr["ent_date"] = model[0].Col4;
        //            dr["edit_by"] = userid_mst;
        //            dr["edit_date"] = currdate;
        //        }
        //        else
        //        {
        //            dr["rec_id"] = "0";
        //            dr["client_id"] = clientid_mst;
        //            dr["client_unit_id"] = unitid_mst;
        //            dr["ent_by"] = userid_mst;
        //            dr["ent_date"] = currdate;
        //            dr["edit_by"] = "-";
        //            dr["edit_by"] = currdate;
        //        }
        //        #endregion
        //        dtstr.Rows.Add(dr);
        //        ok = sgen.Update_data(userCode, dtstr, model[0].Col10, tmodel.Col8, isedit);
        //        if (ok == true)
        //        {
        //            DataTable dtfile = new DataTable();
        //            //dtfile = sgen.getdata(userCode, "select * from file_tab WHERE 1=2");
        //            dtfile = cmd_Fun.GetStructure(userCode, "file_tab");
        //            string serverpath = Server.MapPath("~/Uploads/" + cg_com_name.Replace(" ", "") + "/");
        //            #region attachment
        //            //Pan Card
        //            try
        //            {
        //                HttpPostedFileBase file1 = upd1;
        //                ctype1 = file1.ContentType;
        //                fileName1 = file1.FileName;
        //                path1 = clientid_mst + unitid_mst + userid_mst + currdate + "_" + fileName1;
        //                encpath1 = sgen.Convert_Stringto64(path1).ToString();
        //                finalpath1 = serverpath + encpath1;
        //                file1.SaveAs(finalpath1);
        //                filesave(model, currdate, dtfile, fileName1, encpath1, "Pan Card", ctype1);
        //            }
        //            catch (Exception ex) { }
        //            //Msme
        //            try
        //            {
        //                HttpPostedFileBase file2 = upd2;
        //                ctype2 = file2.ContentType;
        //                fileName2 = file2.FileName;
        //                path2 = clientid_mst + unitid_mst + userid_mst + currdate + "_" + fileName2;
        //                encpath2 = sgen.Convert_Stringto64(path2).ToString();
        //                finalpath2 = serverpath + encpath2;
        //                file2.SaveAs(finalpath2);
        //                filesave(model, currdate, dtfile, fileName2, encpath2, "Msme", ctype2);
        //            }
        //            catch (Exception ex) { }
        //            //Gstin
        //            try
        //            {
        //                HttpPostedFileBase file3 = upd3;
        //                ctype3 = file3.ContentType;
        //                fileName3 = file3.FileName;
        //                path3 = clientid_mst + unitid_mst + userid_mst + currdate + "_" + fileName3;
        //                encpath3 = sgen.Convert_Stringto64(path3).ToString();
        //                finalpath3 = serverpath + encpath3;
        //                file3.SaveAs(finalpath3);
        //                filesave(model, currdate, dtfile, fileName3, encpath3, "Gstin", ctype3);
        //            }
        //            catch (Exception ex) { }
        //            //Vd_Reg
        //            try
        //            {
        //                HttpPostedFileBase file4 = upd4;
        //                ctype4 = file4.ContentType;
        //                fileName4 = file4.FileName;
        //                path4 = clientid_mst + unitid_mst + userid_mst + currdate + "_" + fileName4;
        //                encpath4 = sgen.Convert_Stringto64(path4).ToString();
        //                finalpath4 = serverpath + encpath4;
        //                file4.SaveAs(finalpath4);
        //                filesave(model, currdate, dtfile, fileName4, encpath4, "Vd_Reg", ctype4);
        //            }
        //            catch (Exception ex) { }
        //            //Chq_Copy
        //            try
        //            {
        //                HttpPostedFileBase file5 = upd5;
        //                ctype5 = file5.ContentType;
        //                fileName5 = file5.FileName;
        //                path5 = clientid_mst + unitid_mst + userid_mst + currdate + "_" + fileName5;
        //                encpath5 = sgen.Convert_Stringto64(path5).ToString();
        //                finalpath5 = serverpath + encpath5;
        //                file5.SaveAs(finalpath5);
        //                filesave(model, currdate, dtfile, fileName5, encpath5, "Chq_Copy", ctype5);
        //            }
        //            catch (Exception ex) { }
        //            #endregion
        //            res = sgen.Update_data(userCode, dtfile, "file_tab", "", false);
        //            if (!res) { ViewBag.scripCall = "showmsgJS(1, 'Something Went Wrong, File Not Save', 0);"; return View(model); }
        //            ViewBag.vnew = "";
        //            ViewBag.vedit = "";
        //            ViewBag.vsave = "disabled='disabled'";
        //            ViewBag.scripCall = "showmsgJS(1, 'Data Saved Successfully', 1);disableForm();";
        //            model = new List<Tmodelmain>();
        //            model.Add(new Tmodelmain()
        //            {
        //                Col9 = tmodel.Col9,
        //                Col10 = tmodel.Col10,
        //                Col11 = tmodel.Col11,
        //                Col12 = tmodel.Col12,
        //                Col13 = "Save",
        //                Col14 = tmodel.Col14,
        //                Col15 = tmodel.Col15,
        //                Col39 = tmodel.Col39,
        //                Col40 = tmodel.Col40,
        //                Col41 = tmodel.Col41,
        //                Col42 = tmodel.Col42,
        //                TList1 = mod1,
        //                TList2 = mod2,
        //                TList3 = mod3,
        //                TList4 = mod4,
        //                TList5 = mod5,
        //                TList6 = mod6,
        //                TList7 = mod7,
        //                SelectedItems1 = new string[] { "" },
        //                SelectedItems2 = new string[] { "" },
        //                SelectedItems3 = new string[] { "" },
        //                SelectedItems4 = new string[] { "" },
        //                SelectedItems5 = new string[] { "" },
        //                SelectedItems6 = new string[] { "" },
        //                SelectedItems7 = new string[] { "" },
        //                LTM1 = new List<Tmodelmain>()
        //            });
        //        }
        //        else { ViewBag.scripCall += "showmsgJS(1, 'Record Not Saved', 0);"; }
        //    }
        //    else if (command == "btnst")
        //    {
        //        #region state                   
        //        mq = "select distinct state_name,state_gst_code from country_state where alpha_2='" + model[0].SelectedItems1.FirstOrDefault() + "' and state_name!='-' order by state_name";
        //        dt = sgen.getdata(userCode, mq);
        //        if (dt.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in dt.Rows)
        //            {
        //                mod2.Add(new SelectListItem { Text = dr["state_name"].ToString(), Value = dr["state_gst_code"].ToString() });
        //            }
        //        }
        //        TempData[MyGuid + "_Tlist2"] = mod2;
        //        #endregion
        //        model[0].TList2 = mod2;
        //        ViewBag.vnew = "disabled='disabled'";
        //        ViewBag.vedit = "disabled='disabled'";
        //        ViewBag.vsave = "";
        //    }
        //    ModelState.Clear();
        //    return View(model);
        #endregion
        #region filesave
        private void filesave(List<Tmodelmain> model, string currdate, DataTable dtfile, string filename, string filepath, string filetitle, string content_type)
        {
            FillMst();
            DataRow drf = dtfile.NewRow();
            drf["vch_num"] = model[0].Col17;
            drf["type"] = model[0].Col12;
            drf["ref_code"] = model[0].Col17;
            drf["ref_code1"] = model[0].Col17;
            drf["col1"] = filetitle;
            drf["rec_id"] = "0";
            drf["vch_date"] = currdate;
            drf["file_name"] = filename;
            drf["file_path"] = filepath;
            drf["col2"] = content_type;
            drf["ent_by"] = userid_mst;
            drf["ent_date"] = currdate;
            drf["client_id"] = clientid_mst;
            drf["client_unit_id"] = unitid_mst;
            drf["edit_by"] = userid_mst;
            drf["edit_date"] = currdate;
            dtfile.Rows.Add(drf);
        }
        #endregion
        #region store_issue
        public ActionResult store_issue()
        {
            FillMst();
            chkRef();
            ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.vsave = "disabled='disabled'";
            ViewBag.vsavenew = "disabled='disabled'";
            ViewBag.scripCall = "disableForm();";
            List<Tmodelmain> model = new List<Tmodelmain>();
            Tmodelmain tm1 = new Tmodelmain();
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            tm1.Col9 = "STORE ISSUE"; tm1.Col12 = "STI"; // store issue
            tm1.Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = MyGuid.Trim();
            tm1.Col10 = "itransaction";
            tm1.Col13 = "Save";
            tm1.Col100 = "Save & New";
            tm1.Col121 = "S";
            tm1.Col122 = "<u>S</u>ave";
            tm1.Col123 = "Save & Ne<u>w</u>";
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<SelectListItem> mod2 = new List<SelectListItem>();
            tm1.TList1 = mod1;
            tm1.SelectedItems1 = new string[] { "" };
            tm1.TList2 = mod2;
            tm1.SelectedItems2 = new string[] { "" };
            sgen.SetSession(MyGuid, "STI_MID", mid_mst.Trim());
            sgen.SetSession(MyGuid, "STI__TYPEMST", tm1.Col12.Trim());
            sgen.SetSession(MyGuid, "STI__CONMST", tm1.Col11.Trim());
            sgen.SetSession(MyGuid, "STI_TABMST", tm1.Col10.Trim());
            DataTable dt = sgen.getdata(userCode, "select '0' as  SNo ,'-' as Icode,'-' as I_Name,'-'  HSN,'-' as UOM,'0' as  Qty_in_stock,'0' as Qty_Req,'0' as Qty_issue,'0' as Exp_Value,'-' as Remark from dual");
            tm1.dt1 = dt;
            sgen.SetSession(MyGuid, "dtbase", dt);
            model.Add(tm1);
            return View(model);
        }
        [HttpPost]
        public ActionResult store_issue(List<Tmodelmain> model, string command, string hfrow, string hftable)
        {
            try
            {
                FillMst(model[0].Col15);
                DataTable dtm = sgen.settable(hftable);
                model[0].dt1 = dtm;
                var tm = model.FirstOrDefault();
                List<SelectListItem> mod1 = new List<SelectListItem>();
                List<SelectListItem> mod2 = new List<SelectListItem>();
                TempData[MyGuid + "_TList1"] = model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
                TempData[MyGuid + "_TList2"] = model[0].TList2 = (List<SelectListItem>)TempData[MyGuid + "_TList2"];
                if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
                if (tm.SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
                DataTable dt = new DataTable();
                if (command == "New")
                {
                    sgen.SetSession(MyGuid, "EDMODE", "N");
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vsavenew = "";
                    ViewBag.scripCall = "enableForm();";
                    mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                    vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                    model[0].Col17 = "1";
                    model[0].Col18 = vch_num;
                    model[0].Col13 = "Save";
                    model[0].Col100 = "Save & New";
                    model[0].Col121 = "S";
                    model[0].Col122 = "<u>S</u>ave";
                    model[0].Col123 = "Save & Ne<u>w</u>";
                    #region getdept
                    TempData[MyGuid + "_Tlist1"] = model[0].TList1 = mod1 = cmd_Fun.dept(userCode, clientid_mst, unitid_mst);
                    #endregion
                    #region getdesig
                    TempData[MyGuid + "_Tlist2"] = model[0].TList2 = mod2 = cmd_Fun.desig(userCode, unitid_mst);
                    #endregion

                    model[0].SelectedItems1 = new string[] { "" };

                    model[0].SelectedItems2 = new string[] { "" };
                    //model[0].TList3 = mod3;
                    //model[0].SelectedItems3 = new string[] { "" };
                    //model[0].TList4 = mod4;
                    //model[0].SelectedItems4 = new string[] { "" };
                }
                else if (command == "Cancel")
                {
                    return CancelFun(model);
                    //return RedirectToAction("store_issue", new { @m_id = EncryptDecrypt.Encrypt(model[0].Col15), @mid = EncryptDecrypt.Encrypt(model[0].Col14) });
                }
                else if (command == "Callback")
                {
                    model = StartCallback(model);
                    //if (Session["btnval"] != null) btnval = Session["btnval"].ToString();
                    //model = CallbackFun(btnval, "N", "store_issue", "Purchase", model);
                    //ViewBag.vnew = "disabled='disabled'";
                    //ViewBag.vedit = "disabled='disabled'";
                    //ViewBag.vsave = "";
                }
                else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
                {
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime_local(userCode);
                    currdate = sgen.Savedate(currdate, true);
                    string vch_num = "";
                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        isedit = true;
                        vch_num = model[0].Col18;
                    }
                    else
                    {
                        isedit = false;
                        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                    }
                    //DataTable dataTable = sgen.getdata(userCode, "select  * from " + model[0].Col10.Trim() + "  where  1=2");
                    DataTable dataTable = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    for (int i = 0; i < model[0].dt1.Rows.Count; i++)
                    {
                        DataRow dr = dataTable.NewRow();
                        dr["rec_id"] = "0";
                        dr["type"] = model[0].Col12;
                        dr["vch_date"] = currdate;
                        dr["client_id"] = clientid_mst;
                        dr["client_unit_id"] = unitid_mst;
                        dr["vch_num"] = vch_num.Trim();
                        //dr["indent_no"] = model[0].Col19;
                        dr["date1"] = sgen.Savedate(model[0].Col20, false);
                        dr["department"] = model[0].SelectedItems1.FirstOrDefault();
                        dr["designation"] = model[0].SelectedItems2.FirstOrDefault();
                        dr["remark"] = model[0].Col21;
                        dr["i_remark"] = model[0].dt1.Rows[i]["Remark"].ToString();
                        //dr["i_type"] = model[0].Col22;
                        dr["iname"] = model[0].dt1.Rows[i]["I_Name"].ToString();
                        dr["icode"] = model[0].dt1.Rows[i]["Icode"].ToString();
                        dr["hsn"] = model[0].dt1.Rows[i]["HSN"].ToString();
                        dr["iqty_chl"] = model[0].dt1.Rows[i]["Qty_in_stock"].ToString();
                        dr["iqty_in"] = model[0].dt1.Rows[i]["Qty_Req"].ToString();
                        dr["iamount"] = model[0].dt1.Rows[i]["Exp_Value"].ToString();
                        dr["uom"] = model[0].dt1.Rows[i]["UOM"].ToString();
                        dr["iqty_out"] = model[0].dt1.Rows[i]["Qty_issue"].ToString();
                        if (isedit)
                        {
                            dr["client_id"] = model[0].Col1.Trim();
                            dr["client_unit_id"] = model[0].Col2.Trim();
                            dr["vch_num"] = model[0].Col18;
                            dr["rec_id"] = model[0].Col7;
                            dr["ent_by"] = model[0].Col3;
                            dr["ent_date"] = model[0].Col4;
                            dr["edit_by"] = userid_mst;
                            dr["edit_date"] = currdate;
                        }
                        else
                        {
                            dr["rec_id"] = "0";
                            dr["client_id"] = clientid_mst;
                            dr["client_unit_id"] = unitid_mst;
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
                    ViewBag.vsavenew = "disabled='disabled'";
                    if (Result == true)
                    {
                        //ViewBag.scripCall = "showmsgJS(1, 'Record Saved Successfully', 1);disableForm();";
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                        dt = sgen.getdata(userCode, "select '0' as  SNo ,'-' as Icode,'-' as I_Name,'-'  HSN,'-' as UOM,'0' as  Qty_in_stock,'0' as Qty_Req," +
            "'0' as Exp_Value,'-' as Remark");
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col13 = "Save",
                            Col100 = "Save & New",
                            Col121 = "S",
                            Col122 = "<u>S</u>ave",
                            Col123 = "Save & Ne<u>w</u>",
                            Col9 = "STORE ISSUE",
                            TList1 = mod1,
                            SelectedItems1 = new string[] { "" },
                            TList2 = mod2,
                            SelectedItems2 = new string[] { "" },
                            dt1 = dt
                        }
                            );
                    }
                }
                else if (command == "+")
                {
                    DataRow dr = model[0].dt1.NewRow();
                    model[0].dt1.Rows.Add(dr);
                }
                else if (command == "-")
                {
                    if (model[0].dt1.Rows.Count > 1)
                        model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
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
        #region Vendor Approval
        public ActionResult vendor_app()
        {
            FillMst();
            chkRef();
            ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.vsave = "disabled='disabled'";
            ViewBag.scripCall = "disableForm();";
            List<Tmodelmain> model = new List<Tmodelmain>();
            Tmodelmain tm1 = new Tmodelmain();
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            tm1.Col9 = "VENDOR-ITEM APPROVAL";
            tm1.Col10 = "ivendor";
            tm1.Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            tm1.Col12 = "VAP";
            tm1.Col13 = "Save";
            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = MyGuid.Trim();
            DataTable dt = sgen.getdata(userCode, "select '' Item,'1' SNo ,'-' Icode,'-' IName,'-' P_Unit,'-' S_Unit," +
                "'-' Part_No,'-' PartName,'-' Vendor_ICode,'-' Vendor_IName,'0' IRate,'0' DiscAmt,'0' DiscRate from dual");
            tm1.dt1 = dt;
            sgen.SetSession(MyGuid, "RVA_DT", dt);
            model.Add(tm1);
            ModelState.Clear();
            return View(model);
        }
        [HttpPost]
        public ActionResult vendor_app(List<Tmodelmain> model, string command, string hfrow, string hftable)
        {
            try
            {
                FillMst(model[0].Col15);
                DataTable dtm = sgen.settable(hftable);
                model[0].dt1 = dtm;
                var tm = model.FirstOrDefault();
                if (command == "New")
                {
                    sgen.SetSession(MyGuid, "EDMODE", "N");
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.scripCall = "enableForm();";
                    mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                    vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                    string currdate = sgen.server_datetime_local(userCode);
                    model[0].Col16 = vch_num;
                    model[0].Col17 = currdate;
                    model[0].Col19 = currdate;
                    model[0].Col20 = currdate;
                }
                else if (command == "Cancel")
                {
                    return CancelFun(model);
                    //return RedirectToAction("vendor_app", new { @m_id = EncryptDecrypt.Encrypt(model[0].Col15), @mid = EncryptDecrypt.Encrypt(model[0].Col14) });
                }
                else if (command == "Callback")
                {
                    model = StartCallback(model);
                    //if (Session["btnval"] != null) btnval = Session["btnval"].ToString();
                    //model = CallbackFun(btnval, "N", "vendor_app", "Purchase", model);
                    //ViewBag.vnew = "disabled='disabled'";
                    //ViewBag.vedit = "disabled='disabled'";
                    //ViewBag.vsave = "";
                }
                else if (command == "Save" || command == "Update")
                {
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
                    }
                    //DataTable dataTable = sgen.getdata(userCode, "select  * from " + model[0].Col10.Trim() + "  where  1=2");
                    DataTable dataTable = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    for (int i = 0; i < model[0].dt1.Rows.Count; i++)
                    {
                        DataRow dr = dataTable.NewRow();
                        dr["rec_id"] = "0";
                        dr["type"] = model[0].Col12;
                        dr["vch_num"] = vch_num.Trim();
                        dr["vch_date"] = sgen.Savedate(model[0].Col17, true);
                        dr["date1"] = sgen.Savedate(model[0].Col19, true);//effdt frm
                        dr["date2"] = sgen.Savedate(model[0].Col20, true);//effdt to
                        dr["acode"] = model[0].Col21;//vendorcode
                        dr["icode"] = model[0].dt1.Rows[i][2].ToString();
                        dr["iname"] = model[0].dt1.Rows[i][3].ToString();
                        dr["uom"] = model[0].dt1.Rows[i][4].ToString();
                        dr["uom2"] = model[0].dt1.Rows[i][5].ToString();
                        dr["cpartno"] = model[0].dt1.Rows[i][6].ToString();
                        dr["partname"] = model[0].dt1.Rows[i][7].ToString();
                        dr["vicode"] = model[0].dt1.Rows[i][8].ToString();
                        dr["viname"] = model[0].dt1.Rows[i][9].ToString();
                        dr["irate"] = sgen.Make_decimal(model[0].dt1.Rows[i][10].ToString());
                        dr["discamt"] = sgen.Make_decimal(model[0].dt1.Rows[i][11].ToString());
                        dr["discrate"] = model[0].dt1.Rows[i][12].ToString();
                        if (isedit)
                        {
                            dr["client_id"] = model[0].Col1.Trim();
                            dr["client_unit_id"] = model[0].Col2.Trim();
                            dr["ent_by"] = model[0].Col3;
                            dr["ent_date"] = model[0].Col4;
                            dr["edit_by"] = userid_mst;
                            dr["edit_date"] = currdate;
                        }
                        else
                        {
                            dr["client_id"] = clientid_mst;
                            dr["client_unit_id"] = unitid_mst;
                            dr["ent_by"] = userid_mst;
                            dr["ent_date"] = currdate;
                            dr["edit_by"] = "-";
                            dr["edit_date"] = currdate;
                        }
                        dataTable.Rows.Add(dr);
                    }
                    res = sgen.Update_data(userCode, dataTable, model[0].Col10.Trim(), model[0].Col8, isedit);
                    if (res == true)
                    {
                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        //ViewBag.scripCall = "showmsgJS(1, 'Record Saved Successfully', 1);disableForm();";
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
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
                            dt1 = (DataTable)sgen.GetSession(MyGuid, "RVA_DT")
                        });
                    }
                }
                else if (command == "-")
                {
                    if (model[0].dt1.Rows.Count > 1) model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                    else { model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "RVA_DT"); }
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
        #region PR Approval
        [HttpGet]
        public ActionResult GetFstg(string searchTerm, int pageSize, int pageNum, string icode)
        {
            FillMst();
            //Get the paged results and the total count of the results for this query. 
            DataSet ds = getitemData(userCode, "select distinct a.acode as icode ,b.c_name as iname, a.irate partno, uom,'-' hsn,'-' taxrate,'-' stock from ivendor a INNER JOIN clients_mst b " +
                "on a.acode=b.vch_num and b.type='BCD' and substr(b.vch_num,0,3)='203' and find_in_set(a.client_unit_id,b.client_unit_id)=1  where a.client_unit_id='" + unitid_mst + "' and a.type='VAP'" +
                " and a.icode='" + icode + "'", 1, searchTerm);
            return DstoJSonItems(ds);
        }
        [HttpGet]
        public ActionResult GetIrate(string searchTerm, int pageSize, int pageNum, string icode, string acode)
        {
            FillMst();
            //Get the paged results and the total count of the results for this query. 
            DataSet ds = getitemData(userCode, "select distinct irate as icode,irate iname ,'-' partno,'-' uom,'-' hsn,'-' taxrate,'-' stock " +
                "from ivendor where client_unit_id='" + unitid_mst + "' and acode='" + acode + "' and type='VAP'", 1, searchTerm);
            return DstoJSonItems(ds);
        }
        public JsonpResult DstoJSonItems(DataSet ds)
        {
            List<Item> Items = new List<Item>();
            foreach (DataRow row in ds.Tables["Main"].Rows)
            {
                Item it = new Item();
                it.Icode = row["icode"].ToString();
                it.Iname = row["iname"].ToString();
                it.partno = row["partno"].ToString();
                it.uom = row["uom"].ToString();
                it.hsn = row["hsn"].ToString();
                it.taxrate = row["taxrate"].ToString();
                it.stock = row["stock"].ToString();
                Items.Add(it);
            }
            int attendeeCount = sgen.Make_int(ds.Tables["Totals"].Rows[0][0].ToString());
            Select2PagedResult pagedAttendees = new Select2PagedResult();
            pagedAttendees.Results = new List<Select2Result>();
            foreach (Item a in Items)
            {
                pagedAttendees.Results.Add(new Select2Result { id = a.Icode.ToString() + "!~!" + a.partno + "!~!" + a.hsn + "!~!" + a.uom + "!~!" + a.taxrate + "!~!" + a.stock, text = a.Iname });
            }
            pagedAttendees.Total = attendeeCount;
            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public DataSet getitemData(string userCode, string query, int pageno, string searchval)
        {
            string colswhere = "";
            foreach (PropertyInfo prop in typeof(Item).GetProperties())
            {
                if (colswhere.Equals("")) colswhere = "NVL(" + prop.Name + ",'-')";
                else colswhere = colswhere + "||" + "NVL(" + prop.Name + ",'-') ";
            }
            string where = "where lower(" + colswhere + ") LIKE '%" + searchval.ToLower() + "%' ";
            // shiv = why select *
            query = "SELECT *  from (" + query + " ) tab " + where;
            mq = "SELECT Count(*) as cnt from (" + query + " ) tab";
            string rows = sgen.seekval(userCode, mq, "cnt");
            int olds = 0, limit = 10;
            if (limit == 0) limit = 1;
            mq1 = "SELECT '0' as chk,tab.* FROM( SELECT rownum+" + olds + " as Sr_No,t.* fROM (" + query + ") t) TAB WHERE Sr_No BETWEEN " + limit + " * (" + pageno + " - 1) + 1+" + olds + " AND (" + pageno + " *" + limit + ")+" + olds;
            DataTable dataTable = new DataTable();
            dataTable = sgen.getdata(userCode, mq1);
            dataTable.TableName = "Main";
            DataSet ds = new DataSet();
            ds.Tables.Add(dataTable);
            DataTable DT2 = new DataTable();
            DT2.TableName = "Totals";
            DT2.Columns.Add("Total", typeof(string));
            DataRow dataRow = DT2.NewRow();
            dataRow[0] = Convert.ToInt32(rows);
            DT2.Rows.Add(dataRow);
            ds.Tables.Add(DT2);
            return ds;
        }
        public ActionResult pr_app()
        {
            List<Tmodelmain> model = new List<Tmodelmain>();
            Tmodelmain tm1 = new Tmodelmain();
            try
            {
                FillMst();
                chkRef();
                mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
                MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
                tm1.Col9 = "INDENT APPROVAL";
                tm1.Col10 = "purchases";
                tm1.Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
                tm1.Col12 = "50";
                tm1.Col13 = "Save";
                tm1.Col121 = "S";
                tm1.Col122 = "<u>S</u>ave";
                tm1.Col123 = "Save & Ne<u>w</u>";
                tm1.Col14 = mid_mst.Trim();
                tm1.Col15 = MyGuid.Trim();
                tm1.Col22 = "002";
                tm1.Col20 = sgen.seekval(userCode, "select unit_gstin_no from company_unit_profile where cup_id='" + unitid_mst + "'", "unit_gstin_no");
                DataTable dt = sgen.getdata(userCode, "select '' App,'1' as  SNo,'-' as Indent_No,'-' as Indent_Date ,'-' as Icode,'-' as IName,'-' as UOM,'-' as HSN_NO," +
                    "'-' as Part_No,'-' as Tax_Rate,'-' as Indent_Qty,'-' as Order_Qty,'-' as Party_Code,'-' as Party_Name,'-' as IRate from dual");
                tm1.dt1 = dt;
                sgen.SetSession(MyGuid, "RPR_DT", dt);
                model.Add(tm1);
            }
            catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1, '" + ex.Message.ToString() + "', 0);"; }
            ModelState.Clear();
            return View(model);
        }
        [HttpPost]
        public ActionResult pr_app(List<Tmodelmain> model, string command, string hfrow, string hftable)
        {
            try
            {
                FillMst(model[0].Col15);
                DataTable dtm = sgen.settable(hftable);
                model[0].dt1 = dtm;
                var tm = model.FirstOrDefault();
                if (command == "New")
                {
                    sgen.SetSession(MyGuid, "EDMODE", "N");
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.scripCall = "enableForm();";
                    string currdate = sgen.server_datetime_local(userCode);
                    model[0].Col16 = currdate;
                    model[0].Col17 = currdate;
                }
                if (command == "Show")
                {
                    sgen.SetSession(MyGuid, "EDMODE", "N");
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.scripCall = "enableForm();";
                    string FromDt = sgen.SaveDate_dt(model[0].Col16.Trim(), true).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    string ToDt = sgen.SaveDate_dt(model[0].Col17.Trim(), true).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    mq = "SELECT  a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type||a.icode  as fstr,a.vch_num as Indent_No,to_char(convert_tz(a.vch_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "')" +
                        " Indent_Date,a.icode,b.iname ,b.uom,um.master_name as UOM_name,a.qtyord as Indent_Qty,a.qtyord" +
                        " as App_Qty,hsn.master_name as hsn,hsn.group_name taxrate,b.cpartno FROM purchases a INNER join item b on a.icode=b.icode and b.type='IT' and b.client_unit_id=a.client_unit_id " +
                        "inner join master_setting um on um.master_id=b.uom and um.type='UMM' inner join master_setting hsn on hsn.master_id=b.hsno and hsn.type='HSN' where a.client_unit_id='" + unitid_mst + "' and a.type='66' and" +
                        " convert_tz(a.vch_date, 'UTC', '" + sgen.Gettimezone() + "') between to_date('" + FromDt + "','yyyy-MM-dd') and to_date('" + ToDt + "','yyyy-MM-dd') order by a.vch_num";
                    dtm = sgen.getdata(userCode, mq);
                    if (dtm.Rows.Count > 0)
                    {
                        model[0].dt1 = new DataTable();
                        model[0].dt1 = ((DataTable)sgen.GetSession(MyGuid, "RPR_DT")).Clone();
                        for (int i = 0; i < dtm.Rows.Count; i++)
                        {
                            DataRow dr = model[0].dt1.NewRow();
                            dr["SNo"] = i + 1;
                            dr["Icode"] = dtm.Rows[i]["icode"].ToString();
                            dr["IName"] = dtm.Rows[i]["iname"].ToString();
                            dr["UOM"] = dtm.Rows[i]["UOM_name"].ToString();
                            dr["Indent_No"] = dtm.Rows[i]["Indent_No"].ToString();
                            dr["Indent_Date"] = dtm.Rows[i]["Indent_Date"].ToString();
                            dr["Indent_Qty"] = dtm.Rows[i]["Indent_Qty"].ToString();
                            dr["Order_Qty"] = dtm.Rows[i]["App_Qty"].ToString();
                            dr["HSN_NO"] = dtm.Rows[i]["hsn"].ToString();
                            dr["Tax_Rate"] = dtm.Rows[i]["taxrate"].ToString();
                            dr["Part_No"] = dtm.Rows[i]["cpartno"].ToString();
                            model[0].dt1.Rows.Add(dr);
                        }
                        model[0].dt1.AcceptChanges();
                    }
                }
                else if (command == "Cancel") { return CancelFun(model); }
                else if (command == "Callback") { StartCallback(model); }
                else if (command == "Approval")
                {
                    decimal sumamt = 0, sumdiscamt = 0, gstamt = 0, igst = 0, sgst = 0, cgst = 0, qtyord = 0, irate = 0, iamt = 0, taxamt = 0, lamt = 0, taxableamt = 0;
                    string vgstin = "", cgstin = "", taxrate = "", hf_pgsttype = "";
                    string currdate = sgen.server_datetime(userCode);
                    bool Result = false;
                    isedit = false;
                    //DataView dv = model[0].dt1.DefaultView;
                    DataTable dtmain = model[0].dt1;
                    dtmain = dtmain.AsEnumerable().Where(w => Convert.ToBoolean((string)w["App"].ToString()) == true).Select(s => s).CopyToDataTable();
                    DataView dv = dtmain.DefaultView;
                    DataTable dtparty = dv.ToTable(true, "Party_Code", "Party_Name");
                    foreach (DataRow drr in dtparty.Rows)
                    {
                        var dtitem = dtmain.AsEnumerable().Where(w => (string)w["Party_Code"] == drr["Party_Code"].ToString())
                                                                                        .Select(s => s).CopyToDataTable();
                        DataTable dtpt_detail = sgen.getdata(userCode, "select  b.c_gstin ,b.tor from clients_mst b  where find_in_set(b.client_unit_id,'" + unitid_mst + "')=1 and b.type='BCD' and substr(b.vch_num,0,3)='203'" +
               " and b.vch_num='" + drr["Party_Code"].ToString() + "'");
                        if (dtpt_detail.Rows[0]["c_gstin"].ToString() == "Not Registered")
                        {
                            vgstin = dtpt_detail.Rows[0]["c_gstin"].ToString();
                        }
                        else
                        {
                            vgstin = dtpt_detail.Rows[0]["c_gstin"].ToString().Substring(0, 2);
                        }
                        if (model[0].Col20.Trim().Length > 2)
                        {
                            cgstin = model[0].Col20.Substring(0, 2);
                        }
                        hf_pgsttype = dtpt_detail.Rows[0]["tor"].ToString();
                        dtitem.Columns.Add("lineamount", typeof(decimal));
                        dtitem.Columns.Add("igst", typeof(decimal));
                        dtitem.Columns.Add("cgst", typeof(decimal));
                        dtitem.Columns.Add("sgst", typeof(decimal));
                        dtitem.Columns.Add("gamt", typeof(decimal));
                        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                        dtitem = dtitem.AsEnumerable().GroupBy(r => new
                        {
                            Icode = r.Field<string>("Icode"),
                            IName = r.Field<string>("IName"),
                            Part_No = r.Field<string>("Part_No"),
                            HSN_NO = r.Field<string>("HSN_NO"),
                            UOM = r.Field<string>("UOM"),
                            Tax_Rate = r.Field<string>("Tax_Rate"),
                        }).Select(g =>
                        {
                            var row = dtitem.NewRow();
                            row["IRate"] = g.Min(r => sgen.Make_decimal(r.Field<string>("IRate").Split('!')[0]));
                            row["Order_Qty"] = g.Sum(r => sgen.Make_decimal(r.Field<string>("Order_Qty")));
                            row["Icode"] = g.Key.Icode;
                            row["IName"] = g.Key.IName;
                            row["Part_No"] = g.Key.Part_No;
                            row["HSN_NO"] = g.Key.HSN_NO;
                            row["UOM"] = g.Key.UOM;
                            row["Tax_Rate"] = g.Key.Tax_Rate;
                            var lineamt = g.Sum(r => sgen.Make_decimal(r.Field<string>("Order_Qty"))) * g.Min(r => sgen.Make_decimal(r.Field<string>("IRate").Split('!')[0]));
                            row["lineamount"] = lineamt;
                            if (vgstin == "Not Registered") { taxrate = "0"; }
                            else
                            {
                                if (hf_pgsttype == "C") { taxrate = "0"; }
                                else { taxrate = row["Tax_Rate"].ToString().Split('%')[0]; }
                            }
                            var trate = sgen.Make_decimal(taxrate) / 2;
                            if (vgstin == cgstin)
                            {
                                cgst = (lineamt * trate) / 100;
                                sgst = cgst;
                            }
                            else
                            {
                                igst = (lineamt * sgen.Make_decimal(taxrate)) / 100;
                            }
                            row["tax_rate"] = taxrate;
                            row["cgst"] = cgst;
                            row["sgst"] = sgst;
                            row["igst"] = igst;
                            var gamt1 = lineamt + igst + cgst + sgst;
                            row["gamt"] = gamt1;
                            ;
                            return row;
                        }).CopyToDataTable();
                        sumamt = sgen.Make_decimal(dtitem.Compute("sum(lineamount)", "").ToString());
                        igst = sgen.Make_decimal(dtitem.Compute("sum(igst)", "").ToString());
                        cgst = sgen.Make_decimal(dtitem.Compute("sum(cgst)", "").ToString());
                        sgst = sgen.Make_decimal(dtitem.Compute("sum(sgst)", "").ToString());
                        taxableamt = sumamt;
                        var grandtotal = sumamt + igst + cgst + sgst;
                        decimal gamt = sumamt + cgst + sgst + igst;//46;"F"
                        //DataTable dataTable = sgen.getdata(userCode, "select  * from " + model[0].Col10.Trim() + "  where  1=2");
                        DataTable dataTable = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                        for (int i = 0; i < dtitem.Rows.Count; i++)
                        {
                            DataRow dr = dataTable.NewRow();
                            dr["rec_id"] = "0";
                            dr["type"] = model[0].Col12;
                            dr["vch_num"] = vch_num.Trim();
                            dr["vch_date"] = currdate;
                            dr["acode"] = drr["Party_Code"].ToString();
                            dr["pay_term"] = "-";
                            dr["price_term"] = "-";
                            dr["tmode"] = "-";
                            dr["shipfrom"] = "-";
                            dr["shipto"] = "-";
                            dr["pur_type"] = model[0].Col22;
                            //dt====
                            dr["icode"] = dtitem.Rows[i]["ICODE"].ToString();
                            dr["iname"] = dtitem.Rows[i]["INAME"].ToString();
                            dr["cpartno"] = dtitem.Rows[i]["Part_No"].ToString();
                            dr["hsno"] = dtitem.Rows[i]["HSN_NO"].ToString();
                            dr["uom"] = dtitem.Rows[i]["UOM"].ToString();
                            dr["taxrate"] = dtitem.Rows[i]["Tax_Rate"].ToString();
                            dr["qtystk"] = "0";
                            dr["qtyord"] = dtitem.Rows[i]["ORDER_QTY"].ToString();
                            dr["irate"] = dtitem.Rows[i]["IRate"].ToString();
                            dr["disctype"] = "-";
                            dr["discrate"] = "0";
                            dr["discamt"] = "0";
                            dr["dlv_date"] = currdate;
                            dr["iremark"] = "-";
                            dr["indno"] = "0";
                            dr["inddate"] = currdate;
                            //======
                            dr["totremark"] = "-";
                            dr["cond"] = "-";
                            dr["gdisc"] = "0";
                            dr["gfreight"] = "0";
                            dr["insurance"] = "0";
                            dr["ginstlchrg"] = "0";
                            dr["gserchrg"] = "0";
                            dr["gamc"] = "0";
                            dr["gloadchrg"] = "0";
                            dr["gothrchrg"] = "0";
                            dr["iamount"] = dtitem.Rows[i]["lineamount"].ToString();
                            dr["taxamt"] = sgen.Make_decimal(dtitem.Rows[i]["cgst"].ToString()) + sgen.Make_decimal(dtitem.Rows[i]["igst"].ToString()) + sgen.Make_decimal(dtitem.Rows[i]["sgst"].ToString());
                            dr["lineamount"] = sgen.Make_decimal(dr["iamount"].ToString()) + sgen.Make_decimal(dr["taxamt"].ToString());
                            dr["gtaxamt"] = taxableamt;//42
                            dr["basicamt"] = sumamt; //43 ;"F"
                            dr["igst"] = igst;//43 ;"F"
                            dr["cgst"] = cgst;//44;"F"
                            dr["sgst"] = sgst;//45;"F"
                            dr["gamt"] = sumamt + cgst + sgst + igst;//46;"F"
                            if (isedit)
                            {
                                dr["client_id"] = model[0].Col1.Trim();
                                dr["client_unit_id"] = model[0].Col2.Trim();
                                dr["ent_by"] = model[0].Col3;
                                dr["ent_date"] = model[0].Col4;
                                dr["edit_by"] = userid_mst;
                                dr["edit_date"] = currdate;
                            }
                            else
                            {
                                dr["client_id"] = clientid_mst;
                                dr["client_unit_id"] = unitid_mst;
                                dr["ent_by"] = userid_mst;
                                dr["ent_date"] = currdate;
                                dr["edit_by"] = "-";
                                dr["edit_date"] = currdate;
                            }
                            dataTable.Rows.Add(dr);
                        }
                        Result = sgen.Update_data(userCode, dataTable, model[0].Col10.Trim(), model[0].Col8, isedit);
                    }
                    if (Result == true)
                    {
                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col10 = tm.Col10,
                            Col11 = tm.Col11,
                            Col13 = "Save",
                            Col121 = "S",
                            Col122 = "<u>S</u>ave",
                            Col123 = "Save & Ne<u>w</u>",
                            Col14 = tm.Col14,
                            Col15 = tm.Col15,
                            dt1 = (DataTable)sgen.GetSession(MyGuid, "RPR_DT")
                        });
                    }
                }
                else if (command == "-")
                {
                    if (model[0].dt1.Rows.Count > 1) model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                    else { model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "RPR_DT"); }
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
        #region PO Approval
        public ActionResult po_app()
        {
            List<Tmodelmain> model = new List<Tmodelmain>();
            Tmodelmain tm1 = new Tmodelmain();
            try
            {
                FillMst();
                chkRef();
                mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
                MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
                tm1.Col9 = "PO APPROVAL";
                tm1.Col10 = "purchases";
                tm1.Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
                tm1.Col12 = "";
                tm1.Col13 = "Save";
                tm1.Col100 = "Save & New";
                tm1.Col121 = "S";
                tm1.Col122 = "<u>S</u>ave";
                tm1.Col123 = "Save & Ne<u>w</u>";
                tm1.Col14 = mid_mst.Trim();
                tm1.Col15 = MyGuid.Trim();
                DataTable dt = sgen.getdata(userCode, "select '' as App,'' as Rej, '1' as  SNo,'-' Po_Type,'-' as PO_No,'-' as PO_Date,'' as Party_Code,'' Party_Name ,'' as PO_Amt from dual");
                tm1.dt1 = dt;
                sgen.SetSession(MyGuid, "PO_APP", dt);
                sgen.SetSession(MyGuid, "EDMODE", "Y");
                model.Add(tm1);
            }
            catch { }
            ModelState.Clear();
            return View(model);
        }
        [HttpPost]
        public ActionResult po_app(List<Tmodelmain> model, string command, string hfrow, string hftable)
        {
            try
            {
                FillMst(model[0].Col15);
                DataTable dtm = sgen.settable(hftable);
                model[0].dt1 = dtm;
                var tm = model.FirstOrDefault();
                if (command == "Pending PO")
                {
                    string gross_amount = sgen.seekval(userCode, "select col7 from enx_tab2 where client_unit_id = '" + unitid_mst + "' and type = 'APP' and col2 = '" + userid_mst + "'", "col7");
                    //     mq = "select  p.acode as Party_Code,c.c_name as Party_Name , p.type, t.master_name as Po_Type, p.vch_num PO_Number, to_char(convert_tz(p.vch_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "')" +
                    //" PO_Date,(p.gamt) GrossAmt from purchases p inner join master_setting t on p.type=t.master_id and t.type='KPO' " +
                    //"INNER JOIN clients_mst c on c.vch_num= p.acode and c.type='PVD' and p.client_unit_id=c.client_unit_id  " +
                    //"  where  p.client_unit_id = '" + unitid_mst + "' and  SUBstring(p.type,1,1)=5  and" +
                    //" p.client_unit_id='" + unitid_mst + "' and (p.Approved='0') " +
                    //" group by p.acode,c.c_name, p.vch_num,p.vch_date, t.master_name,p.type,p.gamt having max(p.gamt)<= '" + gross_amount + "'";
                    mq = "select  p.acode as Party_Code,c.c_name as Party_Name , p.type, t.master_name as Po_Type, p.vch_num PO_Number, to_char(convert_tz(p.vch_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "')" +
  " PO_Date,(p.gamt) GrossAmt from purchases p inner join master_setting t on p.type=t.master_id and t.type='KPO' " +
  "INNER JOIN clients_mst c on c.vch_num= p.acode and c.type='BCD' and substr(c.vch_num,0,3)='203' and find_in_set(p.client_unit_id,c.client_unit_id)=1  " +
  "  where  p.client_unit_id = '" + unitid_mst + "' and  SUBstr(p.type,1,1)='5'  and" +
  " p.client_unit_id='" + unitid_mst + "' and (p.Approved='0') " +
  " group by p.acode,c.c_name, p.vch_num,p.vch_date, t.master_name,p.type,p.gamt ";
                    DataTable dt = new DataTable();
                    dt = sgen.getdata(userCode, mq);
                    if (dt.Rows.Count > 0)
                    {
                        model[0].dt1 = new DataTable();
                        model[0].dt1 = ((DataTable)sgen.GetSession(MyGuid, "PO_APP")).Clone();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow dr = model[0].dt1.NewRow();
                            dr["SNo"] = i + 1;
                            dr["Po_Type"] = dt.Rows[i]["Type"].ToString();
                            dr["PO_No"] = dt.Rows[i]["PO_Number"].ToString();
                            dr["PO_Date"] = dt.Rows[i]["PO_Date"].ToString();
                            dr["PO_Amt"] = dt.Rows[i]["GrossAmt"].ToString();
                            dr["Party_Code"] = dt.Rows[i]["Party_Code"].ToString();
                            dr["Party_Name"] = dt.Rows[i]["Party_Name"].ToString();
                            model[0].dt1.Rows.Add(dr);
                        }
                        model[0].dt1.AcceptChanges();
                    }
                }
                else if (command == "Cancel")
                {
                    return CancelFun(model);
                    //return RedirectToAction("po_app", new { @m_id = EncryptDecrypt.Encrypt(model[0].Col15), @mid = EncryptDecrypt.Encrypt(model[0].Col14) });
                }
                else if (command == "Callback")
                {
                    if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
                    model = CallbackFun(btnval, "N", "po_app", "Purchase", model);
                }
                else if ((command == "Save") || (command == "Update"))
                {
                    isedit = true;
                    bool Result = false;
                    for (int i = 0; i < model[0].dt1.Rows.Count; i++)
                    {
                        String status = "";
                        if (Convert.ToBoolean(model[0].dt1.Rows[i][0].ToString()) == true)
                        {
                            status = "Y";
                        }
                        else if (Convert.ToBoolean(model[0].dt1.Rows[i][1].ToString()) == true)
                        {
                            status = "N";
                        }
                        if ((status == "N") || (status == "Y"))
                        {
                            Result = sgen.execute_cmd(userCode, "update purchases set approved='" + status + "' where client_unit_id='" + unitid_mst + "' and type='" + model[0].dt1.Rows[i][3].ToString() + "' and vch_num='" + model[0].dt1.Rows[i][4].ToString() + "'");
                        }
                    }
                    if (Result == true)
                    {
                        string gross_amount = sgen.seekval(userCode, "select col7 from enx_tab2 where client_unit_id = '" + unitid_mst + "' and type = 'APP' and col2 = '" + userid_mst + "'", "col7");
                        mq = "select  p.acode as Party_Code,c.c_name as Party_Name , p.type, t.master_name as Po_Type, p.vch_num PO_Number, to_char(convert_tz(p.vch_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "')" +
          " PO_Date,(p.gamt) GrossAmt from purchases p inner join master_setting t on p.type=t.master_id and t.type='KPO' " +
          "INNER JOIN clients_mst c on c.vch_num= p.acode and c.type='BCD' and substr(c.vch_num,0,3)='203' and p.client_unit_id=c.client_unit_id  " +
          "  where  p.client_unit_id = '" + unitid_mst + "' and  SUBstring(p.type,1,1)=5  and" +
          " p.client_unit_id='" + unitid_mst + "' and (p.Approved='0'||p.Approved='') " +
          " group by p.vch_num,p.vch_date,p.type having max(p.gamt)<= '" + gross_amount + "'";
                        DataTable dt = new DataTable();
                        dt = sgen.getdata(userCode, mq);
                        if (dt.Rows.Count > 0)
                        {
                            model[0].dt1 = new DataTable();
                            model[0].dt1 = ((DataTable)sgen.GetSession(MyGuid, "PO_APP")).Clone();
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                DataRow dr = model[0].dt1.NewRow();
                                dr["SNo"] = i + 1;
                                dr["Po_Type"] = dt.Rows[i]["Type"].ToString();
                                dr["PO_No"] = dt.Rows[i]["PO_Number"].ToString();
                                dr["PO_Date"] = dt.Rows[i]["PO_Date"].ToString();
                                dr["PO_Amount"] = dt.Rows[i]["GrossAmt"].ToString();
                                model[0].dt1.Rows.Add(dr);
                            }
                            model[0].dt1.AcceptChanges();
                        }
                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        //ViewBag.scripCall = "showmsgJS(1, 'Record Saved Successfully', 1);disableForm();";
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col10 = tm.Col10,
                            Col11 = tm.Col11,
                            Col13 = "Save",
                            Col121 = "S",
                            Col122 = "<u>S</u>ave",
                            Col123 = "Save & Ne<u>w</u>",
                            Col14 = tm.Col14,
                            Col15 = tm.Col15,
                            Col12 = tm.Col12,
                            Col9 = "PO APPROVAL",
                            dt1 = (DataTable)sgen.GetSession(MyGuid, "PO_APP")
                        });
                    }
                }
                else if (command == "-")
                {
                    if (model[0].dt1.Rows.Count > 1) model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                    else { model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "APP_AUTH"); }
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
        #region party_detail
        //  [OutputCache(Duration = 30)]
        [GZipOrDeflate]
        public ActionResult party()
        {
            FillMst();
            chkRef();
            ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.vsave = "disabled='disabled'";
            ViewBag.vsavenew = "disabled='disabled'";
            ViewBag.scripCall = "disableForm();";
            List<Tmodelmain> model = new List<Tmodelmain>();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            Tmodelmain tm1 = new Tmodelmain();
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            //01 to 20 customer
            //21 to 40 vendor
            //41 to 50 Inter Unit
            //51 to 60 Employee
            //91 to 99 Prospect
            switch (mid_mst)
            {
                case "21008.1":// BILLING CUSTOMER DETAILS  ////Customer wala page
                case "22005.2":
                case "20001.1":
                    tm1.Col9 = "ACCOUNT DETAIL";
                    tm1.Col119 = "CUSTOMER DETAIL";
                    tm1.Col12 = "BCD";
                    // tm1.Col131 = "01";
                    tm1.Col131 = "303";
                    tm1.Col91 = "BCD";
                    //tm1.Col91 = "BCN";
                    break;
                case "35004.3":// Manufacturer  ////Customer wala page
                    tm1.Col9 = "MANUFACTURER";
                    tm1.Col119 = "CUSTOMER DETAIL";
                    tm1.Col12 = "BCD";
                    // tm1.Col131 = "01";
                    tm1.Col131 = "303";
                    tm1.Col91 = "BCD";
                    tm1.Chk6 = true;
                    //tm1.Col91 = "BCN";
                    break;
                //case "20001.1"://Export Party ////Customer wala page
                //    tm1.Col9 = "EXPORT PARTY DETAIL";
                //    tm1.Col119 = "EXPORT PARTY";
                //    //tm1.Col12 = "BCD";
                //    //tm1.Col91 = "BCN";
                //    tm1.Col12 = "BCD";
                //   // tm1.Col131 = "02";
                //    tm1.Col131 = "303";
                //    tm1.Col91 = "BCD";
                //    break;
                case "28005.1":// PURCHASE VENDOR DETAIL AND PAYROLL
                case "9008.6":// Hr
                case "22005.1":// Vendor Account(accounts)
                case "41005.8": // BANQUET HALL CUSTOMER DETAILS    ////Vendor wala page
                    tm1.Col9 = "NEW VENDOR DETAIL";
                    tm1.Col119 = "VENDOR DETAIL";
                    //tm1.Col12 = "PVD";
                    //tm1.Col91 = "CON";
                    tm1.Col12 = "BCD";
                    //tm1.Col131 = "21";
                    tm1.Col131 = "203";
                    tm1.Col91 = "BCD";
                    break;
                //case "20002.1"://Import Party
                //    tm1.Col9 = "NEW IMPORT VENDOR";
                //    tm1.Col119 = "IMPORT PARTY";
                //    //tm1.Col12 = "PVD"; // BANQUET HALL CUSTOMER DETAILS    ////Vendor wala page
                //    //tm1.Col91 = "CON";
                //    tm1.Col12 = "BCD";
                //    tm1.Col131 = "22";
                //    tm1.Col131 = "203";
                //    tm1.Col91 = "BCD";
                //    break;
                case "40002.1":
                case "5001.1"://work allocation ////Customer wala page
                    tm1.Col9 = "NEW ACCOUNT DETAIL";
                    // tm1.Col119 = "NEW CLIENT";
                    tm1.Col119 = "CUSTOMER DETAIL";
                    //tm1.Col12 = "BCD"; // NEW CLIENT    
                    //tm1.Col91 = "BCN";
                    tm1.Col12 = "BCD";
                    //tm1.Col131 = "01";
                    tm1.Col131 = "303";
                    tm1.Col91 = "BCD";
                    break;
                case "22001.5":
                    tm1.Col9 = "INTER UNIT";      ////Customer wala page
                    tm1.Col119 = "VENDOR DETAIL";
                    //tm1.Col12 = "IUT"; // INTER UNIT (FOR STN)    
                    //tm1.Col91 = "BCN";
                    tm1.Col12 = "BCD";
                    // tm1.Col131 = "41";
                    tm1.Col131 = "103";
                    tm1.Col91 = "BCD";
                    break;
                case "16000.2.2":
                    tm1.Col9 = "NEW PUBLISHER";  ////Vendor wala page
                    tm1.Col119 = "PUBLISHER";
                    //tm1.Col12 = "PVD"; // ADD PUBLUSHER 
                    //tm1.Col91 = "CON";
                    tm1.Col12 = "BCD";
                    // tm1.Col131 = "23";
                    tm1.Col131 = "203";
                    tm1.Col91 = "BCD";
                    break;
                case "40002.9":
                    tm1.Col9 = "PROSPECT ACCOUNT";  ////Customer wala page
                    tm1.Col119 = "PROSPECT DATA";
                    tm1.Col12 = "PRO"; //  
                    //tm1.Col91 = "PON";
                    //tm1.Col131 = "91";
                    tm1.Col131 = "909";
                    tm1.Col91 = "PRO";
                    break;
            }
            tm1.Col141 = sgen.getstring(userCode, "select  a.first_name||' '||Replace(a.MIDDLE_NAME,'0','')||' '||Replace(a.last_name,'0','') from user_details a where a.vch_num='" + userid_mst + "'");
            tm1.Col10 = "clients_mst";
            //tm1.Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            tm1.Col11 = "";
            tm1.Col13 = "Save";
            tm1.Col100 = "Save & New";
            tm1.Col121 = "S";
            tm1.Col122 = "<u>S</u>ave";
            tm1.Col123 = "Save & Ne<u>w</u>";
            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = MyGuid.Trim();
            tm1.Col39 = "Choose File";
            tm1.Col40 = "Choose File";
            tm1.Col41 = "Choose File";
            tm1.Col42 = "Choose File";
            tm1.Col43 = "Choose File";
            sgen.SetSession(MyGuid, "VD_TYPEMST", tm1.Col12);
            sgen.SetSession(MyGuid, "VD_CONT", tm1.Col91);
            sgen.SetSession(MyGuid, "VD_PREFIX", tm1.Col131);
            #region DDL
            TempData[MyGuid + "_TList1"] = tm1.TList1 = mod1;
            //tm1.SelectedItems1 = new string[] { "" };
            TempData[MyGuid + "_TList2"] = tm1.TList2 = mod1;
            //tm1.SelectedItems2 = new string[] { "" };
            TempData[MyGuid + "_TList3"] = tm1.TList3 = mod1;
            //tm1.SelectedItems3 = new string[] { "" };
            TempData[MyGuid + "_TList4"] = tm1.TList4 = mod1;
            //tm1.SelectedItems4 = new string[] { "" };
            TempData[MyGuid + "_TList5"] = tm1.TList5 = mod1;
            //tm1.SelectedItems5 = new string[] { "" };
            TempData[MyGuid + "_TList6"] = tm1.TList6 = mod1;
            //tm1.SelectedItems6 = new string[] { "" };
            TempData[MyGuid + "_TList7"] = tm1.TList7 = mod1;
            //tm1.SelectedItems7 = new string[] { "" };
            TempData[MyGuid + "_TList8"] = tm1.TList8 = mod1;
            //tm1.SelectedItems8 = new string[] { "" };
            TempData[MyGuid + "_TList9"] = tm1.TList9 = mod1;
            //tm1.SelectedItems9 = new string[] { "" };
            TempData[MyGuid + "_TList10"] = tm1.TList10 = mod1;
            //tm1.SelectedItems10 = new string[] { "" };
            TempData[MyGuid + "_TList11"] = tm1.TList11 = mod1;
            //tm1.SelectedItems11 = new string[] { "" };
            TempData[MyGuid + "_TList12"] = tm1.TList12 = mod1;
            //tm1.SelectedItems12 = new string[] { "" };
            TempData[MyGuid + "_TList13"] = tm1.TList13 = mod1;
            //tm1.SelectedItems13 = new string[] { "" };
            TempData[MyGuid + "_TList14"] = tm1.TList14 = mod1;
            //tm1.SelectedItems14 = new string[] { "" };
            TempData[MyGuid + "_TList15"] = tm1.TList15 = mod1;
            //tm1.SelectedItems15 = new string[] { "" };
            TempData[MyGuid + "_TList16"] = tm1.TList16 = mod1;
            //tm1.SelectedItems16 = new string[] { "" };
            TempData[MyGuid + "_TList17"] = tm1.TList17 = mod1;
            //tm1.SelectedItems17 = new string[] { "" };
            TempData[MyGuid + "_TList18"] = tm1.TList18 = mod1;
            //tm1.SelectedItems18 = new string[] { "" };
            TempData[MyGuid + "_TList19"] = tm1.TList19 = mod1;
            //tm1.SelectedItems19 = new string[] { "" };
            TempData[MyGuid + "_TList20"] = tm1.TList20 = mod1;
            //tm1.SelectedItems20 = new string[] { "" };
            TempData[MyGuid + "_TList21"] = tm1.TList21 = mod1;
            //tm1.SelectedItems21 = new string[] { "" };
            TempData[MyGuid + "_TList22"] = tm1.TList22 = mod1;
            //tm1.SelectedItems22 = new string[] { "" };
            TempData[MyGuid + "_TList23"] = tm1.TList23 = mod1;
            //tm1.SelectedItems23 = new string[] { "" };
            TempData[MyGuid + "_TList24"] = tm1.TList24 = mod1;
            //tm1.SelectedItems24 = new string[] { "" };
            TempData[MyGuid + "_TList25"] = tm1.TList25 = mod1;
            //tm1.SelectedItems25 = new string[] { "" };
            TempData[MyGuid + "_TList26"] = tm1.TList26 = mod1;
            //tm1.SelectedItems26 = new string[] { "" };
            TempData[MyGuid + "_TList27"] = tm1.TList27 = mod1;
            //tm1.SelectedItems27 = new string[] { "" };
            TempData[MyGuid + "_TList28"] = tm1.TList28 = mod1;
            //tm1.SelectedItems28 = new string[] { "" };
            TempData[MyGuid + "_TList29"] = tm1.TList29 = mod1;
            //tm1.SelectedItems29 = new string[] { "" };
            TempData[MyGuid + "_TList30"] = tm1.TList30 = mod1;
            //tm1.SelectedItems30 = new string[] { "" };
            TempData[MyGuid + "_TList31"] = tm1.TList31 = mod1;
            //tm1.SelectedItems31 = new string[] { "" };
            TempData[MyGuid + "_TList32"] = tm1.TList32 = mod1;
            TempData[MyGuid + "_TList33"] = tm1.TList33 = mod1;
            TempData[MyGuid + "_TList34"] = tm1.TList34 = mod1;
            TempData[MyGuid + "_TList35"] = tm1.TList35 = mod1;




            tm1.SelectedItems35 = tm1.SelectedItems34 = tm1.SelectedItems33 = tm1.SelectedItems32 = tm1.SelectedItems31 = tm1.SelectedItems30 = tm1.SelectedItems29 = tm1.SelectedItems28 = tm1.SelectedItems27 = tm1.SelectedItems26 = tm1.SelectedItems25 = tm1.SelectedItems24 = tm1.SelectedItems23 = tm1.SelectedItems22 = tm1.SelectedItems21 = tm1.SelectedItems20 = tm1.SelectedItems19 = tm1.SelectedItems18 = tm1.SelectedItems17 = tm1.SelectedItems16 = tm1.SelectedItems15 = tm1.SelectedItems14 = tm1.SelectedItems13 = tm1.SelectedItems12 = tm1.SelectedItems11 = tm1.SelectedItems10 = tm1.SelectedItems9 = tm1.SelectedItems8 = tm1.SelectedItems7 = tm1.SelectedItems6 = tm1.SelectedItems5 = tm1.SelectedItems4 = tm1.SelectedItems3 = tm1.SelectedItems2 = tm1.SelectedItems1 = new string[] { "" };

            #endregion
            tm1.LTM1 = new List<Tmodelmain>();
            tm1.LTM2 = new List<Tmodelmain>();
            Tmodelmain tmltm2 = new Tmodelmain();
            tmltm2.Col1 = "1";
            tmltm2.SelectedItems32 = new string[] { "" };
            tmltm2.TList32 = mod1;
            tm1.LTM2.Add(tmltm2);
            //tm1.LTM1.Add(tmltm2);
            tm1.Col45 = "N";//chk acct type
            mq = "select mod_id from com_module where mod_id='22000' and com_code='" + userCode + "'";
            mq1 = sgen.seekval(userCode, mq, "mod_id");
            if (!mq1.Trim().Equals("0")) tm1.Col45 = "Y";
            mq = "select param2 from controls where param5='" + tm1.Col119 + "' and type='VDC' and upper(param3)='GEO LOCATION'";
            mq = sgen.seekval(userCode, mq, "param2");
            tm1.Col125 = mq;
            if (mq.Trim().Equals("Y"))
            {
                if (tm1.Col76 == null) ViewBag.scripCall = ViewBag.scripCall + "initialize(28.477450,77.034138);";
                else ViewBag.scripCall = ViewBag.scripCall + "initialize(" + tm1.Col76 + ");";
            }
            ViewBag.showextend = "N";
            try
            {
                if (sgen.Make_int(sgen.GetSession(MyGuid, "unitcnt").ToString()) > 1) ViewBag.showextend = "Y";
            }
            catch { }
            DataTable dtb = (DataTable)sgen.GetSession(MyGuid, "URIGHTS");
            try
            {
                dtb = dtb.Select("m_id='" + tm1.Col14 + "'").CopyToDataTable();
                if (dtb.Rows.Count > 0)
                {
                    tm1.Col127 = dtb.Rows[0]["btnnew"].ToString();
                    tm1.Col128 = dtb.Rows[0]["btnedit"].ToString();
                    tm1.Col129 = dtb.Rows[0]["btnview"].ToString();
                    tm1.Col130 = dtb.Rows[0]["btnextend"].ToString();
                }
            }
            catch (Exception ex) { }
            if ((mid_mst == "40002.6") || (mid_mst == "21008.2") || (mid_mst == "28005.7"))
            {
                sgen.SetSession(MyGuid, "VD_PREFIX", "01");
                tm1.Col9 = "NEW ACCOUNT DETAIL";
                tm1.Col119 = "VENDOR DETAIL";
                tm1.Col131 = "303";
                tm1.Col12 = "BCD";
                tm1.Col91 = "BCD";
                if (mid_mst == "28005.7")
                {
                    tm1.Col9 = "NEW VENDOR DETAIL";
                    tm1.Col119 = "CUSTOMER DETAIL";
                    tm1.Col131 = "203";
                }
                sgen.SetSession(MyGuid, "VD_CONT", tm1.Col91);
                model.Add(tm1);
                if (Request.QueryString["fstr"] != null)
                {
                    string fstr = "";
                    if (Request.QueryString["fstr"].StartsWith("EDIT"))
                    {
                        fstr = EncryptDecrypt.Decrypt(Request.QueryString["fstr"].ToString().Replace("EDIT", "").Trim());
                        sgen.SetSession(MyGuid, "SSEEKVAL", fstr);
                        CallbackFun("40002.6", "Y", "party", "purchase", model);
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                        ViewBag.scripCall = "enableForm();";

                    }

                    else if (Request.QueryString["fstr"].StartsWith("VIEW"))
                    {
                        fstr = EncryptDecrypt.Decrypt(Request.QueryString["fstr"].ToString().Replace("VIEW", "").Trim());
                        sgen.SetSession(MyGuid, "SSEEKVAL", fstr);
                        CallbackFun("40002.6", "N", "party", "purchase", model);
                        tm1.Col100 = "Save & New";
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "disabled='disabled'";
                        ViewBag.vsavenew = "disabled='disabled'";
                        ViewBag.scripCall = "disableForm();";

                    }

                    else
                    {
                        //CallbackFun("40002.6", "N", "party", "purchase", model);
                        CallbackFun(mid_mst, "N", "party", "purchase", model);
                        tm1.Col13 = "Save";
                        tm1.Col100 = "Save & New";
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "disabled='disabled'";
                        ViewBag.vsavenew = "disabled='disabled'";
                        ViewBag.scripCall = "disableForm();";
                    }

                }



                return View(model);
            }
            string leadfstr = EncryptDecrypt.Decrypt(Request.QueryString["fstr"]);
            //if ((Request.QueryString["fstr"] != null) && (leadfstr.Contains("BCD")))
            if ((Request.QueryString["fstr"] != null) && ((leadfstr.Contains("BCD")) || (leadfstr.Contains("PDA"))))
            {
                tm1.Col96 = EncryptDecrypt.Decrypt(Request.QueryString["leadid"].ToString()); ;
                tm1.Col9 = "NEW ACCOUNT DETAIL";
                tm1.Col119 = "CUSTOMER DETAIL";
                //tm1.Col12 = "BCD"; // NEW CLIENT  
                //tm1.Col91 = "BCN";
                tm1.Col12 = "BCD";
                tm1.Col131 = "303";
                tm1.Col91 = "BCD";
                model.Add(tm1);
                sgen.SetSession(MyGuid, "SSEEKVAL", EncryptDecrypt.Decrypt(Request.QueryString["fstr"].ToString()));
                CallbackFun("EDIT", "N", "party", "purchase", model);
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
                ViewBag.vsavenew = "";
                ViewBag.scripCall = "enableForm();";
                return View(model);
            }
            else if ((Request.QueryString["fstr"] != null) && (!leadfstr.Contains("BCD")) && (!leadfstr.Contains("PDA")))// direct convert lead into account or prospect
            {
                try
                {
                    string[] ld_fstr = (EncryptDecrypt.Decrypt(Request.QueryString["fstr"].ToString())).Split('_'); ;
                    tm1.Col96 = ld_fstr[0].ToString();
                    if (ld_fstr[1].ToString() == "ACC")
                    {
                        tm1.Col9 = "NEW ACCOUNT DETAIL";
                        tm1.Col119 = "CUSTOMER DETAIL";
                        tm1.Col12 = "BCD";
                        tm1.Col131 = "303";
                        tm1.Col91 = "BCD";
                    }
                    else if (ld_fstr[1].ToString() == "PRO")//from direct lead to prospect
                    {
                        //tm1.Col9 = "PROSPECT DATA";
                        //tm1.Col119 = "PROSPECT DATA";
                        ////tm1.Col12 = "PDA"; //  
                        ////tm1.Col91 = "PON";
                        //tm1.Col12 = "BCD";
                        //tm1.Col131 = "91";
                        //tm1.Col91 = "BCD";
                        tm1.Col9 = "PROSPECT ACCOUNT";  ////Customer wala page
                        tm1.Col119 = "PROSPECT DATA";
                        tm1.Col12 = "PRO"; //  
                                           //tm1.Col91 = "PON";
                                           //tm1.Col131 = "91";
                        tm1.Col131 = "909";
                        tm1.Col91 = "PRO";
                    }
                }
                catch
                {
                    tm1.Col96 = EncryptDecrypt.Decrypt(Request.QueryString["fstr"].ToString());
                    tm1.Col9 = "NEW ACCOUNT DETAIL";
                    tm1.Col119 = "CUSTOMER DETAIL";
                    tm1.Col12 = "BCD";
                    tm1.Col131 = "303";
                    tm1.Col91 = "BCD";
                }
                model.Add(tm1);
                newparty(model);
                if (sgen.GetSession(MyGuid, "Lead_Detail") != null)//fill details of accounts and pros data from lead
                {
                    DataTable dtdetail = (DataTable)sgen.GetSession(MyGuid, "Lead_Detail");
                    if (dtdetail.Rows.Count > 0)
                    {
                        if (dtdetail.Rows[0]["cp_ref_type"].ToString() == "909")
                        {
                            model[0].Col66 = dtdetail.Rows[0]["cust_id"].ToString();
                        }
                        model[0].Col18 = dtdetail.Rows[0]["cust_name"].ToString();// organisation
                        model[0].Col59 = dtdetail.Rows[0]["country_name"].ToString();// country
                        model[0].Col60 = dtdetail.Rows[0]["state_name"].ToString();// State
                        model[0].Col61 = dtdetail.Rows[0]["city_name"].ToString();// City
                        model[0].Col49 = dtdetail.Rows[0]["country_id"].ToString();// Country id
                        model[0].Col71 = dtdetail.Rows[0]["state_id"].ToString();// state id
                        model[0].Col64 = dtdetail.Rows[0]["city"].ToString();// ref_id (rec_id)
                        model[0].Col22 = dtdetail.Rows[0]["address"].ToString();// Address
                        model[0].Col23 = dtdetail.Rows[0]["pincode"].ToString();// Pincode
                        model[0].LTM2[0].Col26 = dtdetail.Rows[0]["cp_fname"].ToString();// First Name
                        model[0].LTM2[0].Col89 = dtdetail.Rows[0]["cp_mname"].ToString();// Middle Name
                        model[0].LTM2[0].Col90 = dtdetail.Rows[0]["cp_lname"].ToString();// Last Name
                        model[0].LTM2[0].Col31 = dtdetail.Rows[0]["desig"].ToString();// Designation
                        model[0].LTM2[0].Col78 = dtdetail.Rows[0]["dept"].ToString();// Department
                        model[0].LTM2[0].Col53 = dtdetail.Rows[0]["mobile_no"].ToString();// Mobile
                        model[0].LTM2[0].Col29 = dtdetail.Rows[0]["emailid"].ToString();// Email
                        model[0].LTM2[0].Col27 = dtdetail.Rows[0]["contact_no"].ToString();// contact
                        model[0].LTM2[0].Col28 = dtdetail.Rows[0]["al_contact"].ToString();// alternate Contact
                        model[0].LTM2[0].Col20 = dtdetail.Rows[0]["dob"].ToString();// DOB
                        model[0].LTM2[0].Col21 = dtdetail.Rows[0]["doa"].ToString();// DOA
                        model[0].LTM2[0].Col30 = dtdetail.Rows[0]["cp_add"].ToString();// Contact Person Add
                        model[0].Col54 = dtdetail.Rows[0]["remark"].ToString();// remark
                        String[] L14 = System.String.Join(",", dtdetail.Rows.OfType<DataRow>().Select(r => r["ld_source"].ToString()).Distinct()).Split(',');
                        model[0].SelectedItems14 = L14;
                        String[] L15 = System.String.Join(",", dtdetail.Rows.OfType<DataRow>().Select(r => r["bsn_type"].ToString()).Distinct()).Split(',');
                        model[0].SelectedItems15 = L15;
                        String[] L5 = System.String.Join(",", dtdetail.Rows.OfType<DataRow>().Select(r => r["ld_product"].ToString()).Distinct()).Split(',');
                        model[0].SelectedItems16 = L5;
                        return View(model);
                    }
                }
            }
            model.Add(tm1);
            return View(model);
        }
        public List<Tmodelmain> newparty(List<Tmodelmain> model)
        {
            model = getnew(model);
            if (model[0].Col14 == "35004.3")
            { model[0].Chk6 = true; }
            model[0].Col13 = "Save";
            try
            {
                model[0].Col17 = vch_num;
                model[0].Col13 = "Save";
                model[0].Col35 = "R";
                model[0].Col44 = "Active";
                string defval = "";
                if (model[0].Col125 == "Y")
                {
                    if (model[0].Col76 == null) ViewBag.scripCall = ViewBag.scripCall + "initialize(28.477450,77.034138);";
                    else ViewBag.scripCall = ViewBag.scripCall + "initialize(" + model[0].Col76 + ");";
                }
                #region DDL
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
                List<SelectListItem> mod12 = new List<SelectListItem>();
                List<SelectListItem> mod13 = new List<SelectListItem>();
                List<SelectListItem> mod14 = new List<SelectListItem>();
                List<SelectListItem> mod15 = new List<SelectListItem>();
                List<SelectListItem> mod16 = new List<SelectListItem>();
                List<SelectListItem> mod17 = new List<SelectListItem>();
                List<SelectListItem> mod18 = new List<SelectListItem>();
                List<SelectListItem> mod19 = new List<SelectListItem>();
                List<SelectListItem> mod20 = new List<SelectListItem>();
                List<SelectListItem> mod21 = new List<SelectListItem>();
                List<SelectListItem> mod22 = new List<SelectListItem>();
                List<SelectListItem> mod23 = new List<SelectListItem>();
                List<SelectListItem> mod24 = new List<SelectListItem>();
                List<SelectListItem> mod25 = new List<SelectListItem>();
                List<SelectListItem> mod26 = new List<SelectListItem>();
                List<SelectListItem> mod27 = new List<SelectListItem>();
                List<SelectListItem> mod28 = new List<SelectListItem>();
                List<SelectListItem> mod29 = new List<SelectListItem>();
                List<SelectListItem> mod30 = new List<SelectListItem>();
                List<SelectListItem> mod31 = new List<SelectListItem>();
                List<SelectListItem> mod32 = new List<SelectListItem>();
                List<SelectListItem> mod33 = new List<SelectListItem>();
                List<SelectListItem> mod34 = new List<SelectListItem>();
                List<SelectListItem> mod35 = new List<SelectListItem>();
                #region  ddl
                defval = "";
                TempData[MyGuid + "_Tlist3"] = model[0].TList3 = mod3 = cmd_Fun.partytype(userCode, unitid_mst, out defval);     //party type
                model[0].SelectedItems3 = new string[] { defval };
                mod4.Add(new SelectListItem { Text = "Domestic", Value = "001" });
                mod4.Add(new SelectListItem { Text = "Overseas", Value = "002" });
                TempData[MyGuid + "_Tlist4"] = model[0].TList4 = mod4;                               // party location - fix
                model[0].SelectedItems4 = new string[] { "001" };

                TempData[MyGuid + "_Tlist5"] = model[0].TList5 = mod5 = cmd_Fun.acctypevdm(userCode, unitid_mst);    //acc type

                TempData[MyGuid + "_Tlist6"] = model[0].TList6 = mod6 = cmd_Fun.curname(userCode, unitid_mst);    //currency type

                TempData[MyGuid + "_Tlist7"] = model[0].TList7 = mod7 = cmd_Fun.bank(userCode, unitid_mst);    //bank name
                defval = "";
                TempData[MyGuid + "_TList8"] = model[0].TList8 = mod8 = cmd_Fun.salearea(userCode, unitid_mst, out defval);    //SALES/SERVICE AREA
                model[0].SelectedItems8 = new string[] { defval };
                TempData[MyGuid + "_TList9"] = model[0].TList9 = mod9 = cmd_Fun.protype(userCode, unitid_mst);     //bindpro_type
                defval = "";
                TempData[MyGuid + "_TList10"] = model[0].TList10 = mod10 = cmd_Fun.occtype(userCode, unitid_mst, out defval);   //occupation type
                model[0].SelectedItems10 = new string[] { defval };
                defval = "";
                TempData[MyGuid + "_TList11"] = model[0].TList11 = mod11 = cmd_Fun.clienttype(userCode, unitid_mst, out defval);  //typeofclient
                model[0].SelectedItems11 = new string[] { defval };
                defval = "";
                TempData[MyGuid + "_TList12"] = model[0].TList12 = mod12 = cmd_Fun.addresstype(userCode, unitid_mst, out defval);  //typeofaddress
                TempData[MyGuid + "_TList13"] = model[0].TList13 = mod13 = mod12;  //typeofaddress
                model[0].SelectedItems12 = new string[] { defval };
                model[0].SelectedItems13 = new string[] { defval };
                defval = "";
                TempData[MyGuid + "_TList14"] = model[0].TList14 = mod14 = cmd_Fun.leadsource(userCode, unitid_mst, out defval);  //Lead source
                model[0].SelectedItems14 = new string[] { defval };
                defval = "";
                TempData[MyGuid + "_TList15"] = model[0].TList15 = mod15 = cmd_Fun.businesstype(userCode, unitid_mst, out defval);  // Business type
                model[0].SelectedItems15 = new string[] { defval };
                defval = "";
                TempData[MyGuid + "_TList16"] = model[0].TList16 = mod16 = cmd_Fun.product(userCode, unitid_mst, out defval);  //Product Type
                model[0].SelectedItems16 = new string[] { defval };
                defval = "";
                TempData[MyGuid + "_TList17"] = model[0].TList17 = mod17 = cmd_Fun.payterm(userCode, unitid_mst, out defval);  //Payment term
                model[0].SelectedItems17 = new string[] { defval };

                TempData[MyGuid + "_TList30"] = model[0].TList30 = mod17; //Payment term secondary


                TempData[MyGuid + "_TList25"] = model[0].TList25 = mod25 = cmd_Fun.username(userCode);   //Refered By User
                                                                                                         //model[0].SelectedItems25 = new string[] { "" };

                TempData[MyGuid + "_TList26"] = model[0].TList26 = mod26 = cmd_Fun.compunit(userCode, clientid_mst);  //comp_unit
                                                                                                                      //model[0].SelectedItems26 = new string[] { "" };

                TempData[MyGuid + "_TList27"] = model[0].TList27 = mod27 = cmd_Fun.Modeofpayment(userCode, unitid_mst);  // Payment Mode
                                                                                                                         //model[0].SelectedItems27 = new string[] { "" };

                TempData[MyGuid + "_TList31"] = model[0].TList31 = mod31 = mod27;  //Payment Mode secondary     
                //model[0].SelectedItems31 = new string[] { "" };

                TempData[MyGuid + "_TList28"] = model[0].TList28 = mod28 = cmd_Fun.cheqprint(userCode, unitid_mst);  //Cheque Print Location
                //model[0].SelectedItems28 = new string[] { "" };

                mod29.Add(new SelectListItem { Text = "Low", Value = "001" });
                mod29.Add(new SelectListItem { Text = "Medium", Value = "002" });
                mod29.Add(new SelectListItem { Text = "High", Value = "003" });
                //model[0].SelectedItems29 = new string[] { "" };
                TempData[MyGuid + "_TList29"] = model[0].TList29 = mod29;  //Risk Category
                TempData[MyGuid + "_TList33"] = model[0].TList33 = mod33 = mod12;  //typeofaddress

                mod34.Add(new SelectListItem { Text = "Company", Value = "Company" });
                mod34.Add(new SelectListItem { Text = "Not a Company", Value = "Not a Company" });
                TempData[MyGuid + "_TList34"] = model[0].TList34 = mod34;  //Deductee Type
                TempData[MyGuid + "_TList35"] = model[0].TList35 = mod35 = cmd_Fun.country(userCode, unitid_mst);  //country(FOR NRI)

                mod32 = new List<SelectListItem>();
                foreach (var mod in model[0].LTM2)
                {
                    if (mod.SelectedItems32 == null) mod.SelectedItems32 = new string[] { "" };


                    mod32.Add(new SelectListItem { Text = "Male", Value = "Male" });
                    mod32.Add(new SelectListItem { Text = "Female", Value = "FeMale" });
                    TempData[MyGuid + "_Tlist32"] = mod.TList32 = mod32;


                }
                #endregion
                if (sgen.GetSession(MyGuid, "tmp_client") != null)
                {
                    DataTable tmp = (DataTable)sgen.GetSession(MyGuid, "tmp_client");
                    if (tmp.Rows.Count > 0)
                    {
                        if ((tmp.Select("param3='RELATION MANAGER'")[0]["param2"].ToString() == "Y") || (tmp.Select("param3='SUB BROKER'")[0]["param2"].ToString() == "Y") || (tmp.Select("param3='CLIENT RATING'")[0]["param2"].ToString() == "Y") || (tmp.Select("param3='CREDIT RATING'")[0]["param2"].ToString() == "Y") || (tmp.Select("param3='QUALIFICATION'")[0]["param2"].ToString() == "Y") || (tmp.Select("param3='DP'")[0]["param2"].ToString() == "Y") || (tmp.Select("param3='ANNUAL INCOME'")[0]["param2"].ToString() == "Y"))
                        {
                            #region  Relation Manger
                            TempData[MyGuid + "_TList18"] = model[0].TList18 = mod18 = cmd_Fun.username(userCode);
                            model[0].SelectedItems18 = new string[] { "" };
                            #endregion
                            #region  sub broker
                            defval = "";
                            TempData[MyGuid + "_TList19"] = model[0].TList19 = mod19 = cmd_Fun.subbroker(userCode, unitid_mst, out defval);
                            model[0].SelectedItems19 = new string[] { defval };
                            #endregion
                            #region  Client rating
                            defval = "";
                            TempData[MyGuid + "_TList20"] = model[0].TList20 = mod20 = cmd_Fun.clientrating(userCode, unitid_mst, out defval);
                            model[0].SelectedItems20 = new string[] { defval };
                            #endregion
                            #region  Qualification
                            defval = "";
                            TempData[MyGuid + "_TList21"] = model[0].TList21 = mod21 = cmd_Fun.qualification(userCode, unitid_mst, out defval);
                            model[0].SelectedItems21 = new string[] { defval };
                            #endregion
                            #region  DP
                            mod22.Add(new SelectListItem { Text = "NSDL", Value = "NSDL" });
                            mod22.Add(new SelectListItem { Text = "CDSL", Value = "CDSL" });
                            model[0].SelectedItems22 = new string[] { "" };
                            TempData[MyGuid + "_TList22"] = model[0].TList22 = mod22;
                            #endregion
                            #region  annual income
                            TempData[MyGuid + "_TList23"] = model[0].TList23 = mod23 = cmd_Fun.annincome(userCode, unitid_mst);
                            model[0].SelectedItems23 = new string[] { "" };
                            #endregion
                            #region  Credit Rating
                            defval = "";
                            TempData[MyGuid + "_TList24"] = model[0].TList24 = mod24 = cmd_Fun.creditrating(userCode, unitid_mst, out defval);
                            model[0].SelectedItems24 = new string[] { defval };
                            #endregion
                        }
                        else
                        {
                            TempData[MyGuid + "_TList18"] = model[0].TList18 = mod18;
                            model[0].SelectedItems18 = new string[] { "" };
                            TempData[MyGuid + "_TList19"] = model[0].TList19 = mod19;
                            model[0].SelectedItems19 = new string[] { "" };
                            TempData[MyGuid + "_TList20"] = model[0].TList20 = mod20;
                            model[0].SelectedItems20 = new string[] { "" };
                            TempData[MyGuid + "_TList21"] = model[0].TList21 = mod21;
                            model[0].SelectedItems21 = new string[] { "" };
                            TempData[MyGuid + "_TList22"] = model[0].TList22 = mod22;
                            model[0].SelectedItems22 = new string[] { "" };
                            TempData[MyGuid + "_TList23"] = model[0].TList23 = mod23;
                            model[0].SelectedItems23 = new string[] { "" };
                            TempData[MyGuid + "_TList24"] = model[0].TList24 = mod24;
                            model[0].SelectedItems24 = new string[] { "" };
                        }
                    }
                }


                //model[0].TList1 = mod1;
                //model[0].TList2 = mod2;
                #endregion
                model[0].Col37 = "-";
                model[0].Col38 = "-";
                model[0].Col88 = "-";
                model[0].Col25 = "-";
                model[0].Col22 = "-";
                model[0].Col26 = "-";
                model[0].Col89 = "-";
                model[0].Col90 = "-";
                model[0].Col92 = "-";
                model[0].Col70 = "-";
                model[0].Col154 = "-";
                //model[0].LTM2[0].Col20 = "01/01/1900";
                //model[0].LTM2[0].Col21 = "01/01/1900";
            }
            catch (Exception ex) { }
            return model;
        }
        [HttpPost]
        [GZipOrDeflate]
        public ActionResult party(List<Tmodelmain> model, string modelstr, string command, HttpPostedFileBase upd1, HttpPostedFileBase upd2,
            HttpPostedFileBase upd3, HttpPostedFileBase upd4, HttpPostedFileBase upd5, HttpPostedFileBase upd6, HttpPostedFileBase upd7, HttpPostedFileBase upd8, HttpPostedFileBase upd9, HttpPostedFileBase upd10, HttpPostedFileBase upd11, string hfrow)
        {
            try
            {
                string ctype1 = "", ctype2 = "", ctype3 = "", ctype4 = "", ctype5 = "", ctype6 = "", ctype7 = "", ctype8 = "", ctype9 = "", ctype10 = "", ctype11 = "",
                    finalpath1 = "", finalpath2 = "", finalpath3 = "", finalpath4 = "", finalpath5 = "", finalpath6 = "", finalpath8 = "", finalpath9 = "", finalpath10 = "", finalpath11 = "", finalpath7 = "", path1 = "", path2 = "", path3 = "", path4 = "", path5 = "", path6 = "", path7 = "", path8 = "", path9 = "", path10 = "", path11 = "",
                    fileName1 = "", fileName2 = "", fileName3 = "", fileName4 = "", fileName5 = "", fileName6 = "", fileName7 = "", fileName8 = "", fileName9 = "", fileName10 = "", fileName11 = "", encpath1 = "", encpath2 = "", encpath3 = "", encpath4 = "", encpath5 = "", encpath6 = "", encpath7 = "", encpath8 = "", encpath9 = "", encpath10 = "", encpath11 = "",
                iscontractor = "N";
                FillMst(model[0].Col15);
                model = sgen.Make_Model(modelstr);
                try
                {
                    if (sgen.Make_int(sgen.GetSession(MyGuid, "unitcnt").ToString()) > 1) ViewBag.showextend = "Y";
                }
                catch { }
                var tm = model.FirstOrDefault();
                DataTable dt = new DataTable();
                #region dropdown      
                List<SelectListItem> mod1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
                List<SelectListItem> mod2 = (List<SelectListItem>)TempData[MyGuid + "_TList2"];
                List<SelectListItem> mod3 = (List<SelectListItem>)TempData[MyGuid + "_TList3"];
                List<SelectListItem> mod4 = (List<SelectListItem>)TempData[MyGuid + "_TList4"];
                List<SelectListItem> mod5 = (List<SelectListItem>)TempData[MyGuid + "_TList5"];
                List<SelectListItem> mod6 = (List<SelectListItem>)TempData[MyGuid + "_TList6"];
                List<SelectListItem> mod7 = (List<SelectListItem>)TempData[MyGuid + "_TList7"];
                List<SelectListItem> mod8 = (List<SelectListItem>)TempData[MyGuid + "_TList8"];
                List<SelectListItem> mod9 = (List<SelectListItem>)TempData[MyGuid + "_TList9"];
                List<SelectListItem> mod10 = (List<SelectListItem>)TempData[MyGuid + "_TList10"];
                List<SelectListItem> mod11 = (List<SelectListItem>)TempData[MyGuid + "_TList11"];
                List<SelectListItem> mod12 = (List<SelectListItem>)TempData[MyGuid + "_TList12"];
                List<SelectListItem> mod13 = (List<SelectListItem>)TempData[MyGuid + "_TList13"];
                List<SelectListItem> mod14 = (List<SelectListItem>)TempData[MyGuid + "_TList14"];
                List<SelectListItem> mod15 = (List<SelectListItem>)TempData[MyGuid + "_TList15"];
                List<SelectListItem> mod16 = (List<SelectListItem>)TempData[MyGuid + "_TList16"];
                List<SelectListItem> mod17 = (List<SelectListItem>)TempData[MyGuid + "_TList17"];
                List<SelectListItem> mod18 = (List<SelectListItem>)TempData[MyGuid + "_TList18"];
                List<SelectListItem> mod19 = (List<SelectListItem>)TempData[MyGuid + "_TList19"];
                List<SelectListItem> mod20 = (List<SelectListItem>)TempData[MyGuid + "_TList20"];
                List<SelectListItem> mod21 = (List<SelectListItem>)TempData[MyGuid + "_TList21"];
                List<SelectListItem> mod22 = (List<SelectListItem>)TempData[MyGuid + "_TList22"];
                List<SelectListItem> mod23 = (List<SelectListItem>)TempData[MyGuid + "_TList23"];
                List<SelectListItem> mod24 = (List<SelectListItem>)TempData[MyGuid + "_TList24"];
                List<SelectListItem> mod25 = (List<SelectListItem>)TempData[MyGuid + "_TList25"];
                List<SelectListItem> mod26 = (List<SelectListItem>)TempData[MyGuid + "_TList26"];
                List<SelectListItem> mod27 = (List<SelectListItem>)TempData[MyGuid + "_TList27"];
                List<SelectListItem> mod28 = (List<SelectListItem>)TempData[MyGuid + "_TList28"];
                List<SelectListItem> mod29 = (List<SelectListItem>)TempData[MyGuid + "_TList29"];
                List<SelectListItem> mod30 = (List<SelectListItem>)TempData[MyGuid + "_TList30"];
                List<SelectListItem> mod31 = (List<SelectListItem>)TempData[MyGuid + "_TList31"];
                List<SelectListItem> mod32 = (List<SelectListItem>)TempData[MyGuid + "_TList32"];
                List<SelectListItem> mod33 = (List<SelectListItem>)TempData[MyGuid + "_TList33"];
                List<SelectListItem> mod34 = (List<SelectListItem>)TempData[MyGuid + "_TList34"];
                List<SelectListItem> mod35 = (List<SelectListItem>)TempData[MyGuid + "_TList35"];
                TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
                TempData[MyGuid + "_TList2"] = model[0].TList2 = mod2;
                TempData[MyGuid + "_TList3"] = model[0].TList3 = mod3;
                TempData[MyGuid + "_TList4"] = model[0].TList4 = mod4;
                TempData[MyGuid + "_TList5"] = model[0].TList5 = mod5;
                TempData[MyGuid + "_TList6"] = model[0].TList6 = mod6;
                TempData[MyGuid + "_TList7"] = model[0].TList7 = mod7;
                TempData[MyGuid + "_TList8"] = model[0].TList8 = mod8;
                TempData[MyGuid + "_TList9"] = model[0].TList9 = mod9;
                TempData[MyGuid + "_TList10"] = model[0].TList10 = mod10;
                TempData[MyGuid + "_TList11"] = model[0].TList11 = mod11;
                TempData[MyGuid + "_TList12"] = model[0].TList12 = mod12;
                TempData[MyGuid + "_TList13"] = model[0].TList13 = mod13;
                TempData[MyGuid + "_TList14"] = model[0].TList14 = mod14;
                TempData[MyGuid + "_TList15"] = model[0].TList15 = mod15;
                TempData[MyGuid + "_TList16"] = model[0].TList16 = mod16;
                TempData[MyGuid + "_TList17"] = model[0].TList17 = mod17;
                TempData[MyGuid + "_TList18"] = model[0].TList18 = mod18;
                TempData[MyGuid + "_TList19"] = model[0].TList19 = mod19;
                TempData[MyGuid + "_TList20"] = model[0].TList20 = mod20;
                TempData[MyGuid + "_TList21"] = model[0].TList21 = mod21;
                TempData[MyGuid + "_TList22"] = model[0].TList22 = mod22;
                TempData[MyGuid + "_TList23"] = model[0].TList23 = mod23;
                TempData[MyGuid + "_TList24"] = model[0].TList24 = mod24;
                TempData[MyGuid + "_TList25"] = model[0].TList25 = mod25;
                TempData[MyGuid + "_TList26"] = model[0].TList26 = mod26;
                TempData[MyGuid + "_TList27"] = model[0].TList27 = mod27;
                TempData[MyGuid + "_TList28"] = model[0].TList28 = mod28;
                TempData[MyGuid + "_TList29"] = model[0].TList29 = mod29;
                TempData[MyGuid + "_TList30"] = model[0].TList30 = mod30;
                TempData[MyGuid + "_TList31"] = model[0].TList31 = mod31;
                TempData[MyGuid + "_TList32"] = model[0].TList32 = mod32;
                TempData[MyGuid + "_TList33"] = model[0].TList33 = mod33;
                TempData[MyGuid + "_TList34"] = model[0].TList34 = mod34;
                TempData[MyGuid + "_TList35"] = model[0].TList35 = mod35;

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
                if (model[0].SelectedItems12 == null) model[0].SelectedItems12 = new string[] { "" };
                if (model[0].SelectedItems13 == null) model[0].SelectedItems13 = new string[] { "" };
                if (model[0].SelectedItems14 == null) model[0].SelectedItems14 = new string[] { "" };
                if (model[0].SelectedItems15 == null) model[0].SelectedItems15 = new string[] { "" };
                if (model[0].SelectedItems16 == null) model[0].SelectedItems16 = new string[] { "" };
                if (model[0].SelectedItems17 == null) model[0].SelectedItems17 = new string[] { "" };
                if (model[0].SelectedItems18 == null) model[0].SelectedItems18 = new string[] { "" };
                if (model[0].SelectedItems19 == null) model[0].SelectedItems19 = new string[] { "" };
                if (model[0].SelectedItems20 == null) model[0].SelectedItems20 = new string[] { "" };
                if (model[0].SelectedItems21 == null) model[0].SelectedItems21 = new string[] { "" };
                if (model[0].SelectedItems22 == null) model[0].SelectedItems22 = new string[] { "" };
                if (model[0].SelectedItems23 == null) model[0].SelectedItems23 = new string[] { "" };
                if (model[0].SelectedItems24 == null) model[0].SelectedItems24 = new string[] { "" };
                if (model[0].SelectedItems25 == null) model[0].SelectedItems25 = new string[] { "" };
                if (model[0].SelectedItems26 == null) model[0].SelectedItems26 = new string[] { "" };
                if (model[0].SelectedItems27 == null) model[0].SelectedItems27 = new string[] { "" };
                if (model[0].SelectedItems28 == null) model[0].SelectedItems28 = new string[] { "" };
                if (model[0].SelectedItems29 == null) model[0].SelectedItems29 = new string[] { "" };
                if (model[0].SelectedItems30 == null) model[0].SelectedItems30 = new string[] { "" };
                if (model[0].SelectedItems31 == null) model[0].SelectedItems31 = new string[] { "" };
                if (model[0].SelectedItems32 == null) model[0].SelectedItems32 = new string[] { "" };
                if (model[0].SelectedItems33 == null) model[0].SelectedItems33 = new string[] { "" };
                if (model[0].SelectedItems34 == null) model[0].SelectedItems34 = new string[] { "" };
                if (model[0].SelectedItems35 == null) model[0].SelectedItems35 = new string[] { "" };

                foreach (var mod in model[0].LTM2)
                {
                    TempData[MyGuid + "_Tlist32"] = mod.TList32 = mod32;
                    if (mod.SelectedItems32 == null) mod.SelectedItems32 = new string[] { "" };
                }
                #endregion
                if (command == "New")
                {
                    if (model[0].Col127 == "N")
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for new " + model[0].Col9 + ", please contact your admin', 2);";
                        return View(model);
                    }
                    model = newparty(model);
                }
                else if (command == "Cancel")
                {
                    return CancelFun(model);
                }
                else if (command == "Callback")
                {
                    //if (sgen.GetSession(MyGuid, "delid") != null) { btnval = "FDEL"; sgen.SetSession(MyGuid, "btnval", btnval); }
                    //else if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();

                    if (sgen.GetSession(MyGuid, "delid") != null) { btnval = "FDEL"; }
                    else if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
                    if (model[0].Col130 == "N" && btnval.Trim().Equals("EXT"))
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for extend " + model[0].Col9 + ", please contact your admin', 2);disableForm();";
                        return View(model);
                    }
                    else if (model[0].Col128 == "N" && btnval.Trim().Equals("EDIT"))
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for edit " + model[0].Col9 + ", please contact your admin', 2);disableForm();";
                        return View(model);
                    }
                    model = CallbackFun(btnval, "N", "party", controllerName, model);

                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vsavenew = "";

                    if (btnval == "VIEW")
                    {
                        model[0].Col13 = "Save";
                        model[0].Col100 = "Save & New";
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "disabled='disabled'";
                        ViewBag.vsavenew = "disabled='disabled'";
                        ViewBag.scripCall = "disableForm();";
                    }
                    if (model[0].Col125 == "Y")
                    {
                        if (tm.Col76 == null) ViewBag.scripCall = ViewBag.scripCall + "initialize(28.477450,77.034138);";
                        else ViewBag.scripCall = ViewBag.scripCall + "initialize(" + tm.Col76 + ");";
                    }
                }
                else if (command == "hfbtnyes" || command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
                {
                    if (command != "hfbtnyes")
                    {
                        if (model[0].Col127 == "N")
                        {
                            ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for save " + model[0].Col9 + " , please contact your admin', 2);";
                            return View(model);
                        }
                        commval = command;
                    }

                    if ((model[0].LTM2[0].Col26 == "") && ((model[0].Col18 == "") || (model[0].Col18 == null)))
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'Please Enter Either Organisation Name Or Contact Person', 2);";
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                        ModelState.Clear();
                        return View(model);

                    }
                    if ((model[0].Col18 == null) || (model[0].Col18 == ""))
                    {
                        model[0].Col18 = model[0].LTM2[0].Col26;
                    }

                    string Isbilling = "", sez = "N";
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime(userCode);
                    if (model[0].Chk1 == true) isgstr = "Y";
                    if (model[0].Chk2 == true) iscontractor = "Y";
                    type = model[0].Col12;

                    if (model[0].Col44.Trim() == "Inactive")
                    {
                        type = "DD" + type;
                        status = "N";
                    }
                    else status = "Y";
                    DataTable dtstr = new DataTable();
                    dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        mq1 = " and vch_num<>'" + tmodel.Col17.Trim() + "'";
                        isedit = true;
                        model[0].Col17 = tmodel.Col17.Trim();
                    }
                    else
                    {
                        mq1 = "";
                        // mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where type='" + model[0].Col12 + "' " + model[0].Col11 + "  ";
                        mq = "select max(to_number(substr(vch_num,4,8))) as auto_genid from " + model[0].Col10.Trim() + " a where  type in ('" + model[0].Col12.Trim() + "' ,'DD" + model[0].Col12 + "')" + model[0].Col11.Trim() + " and substr(vch_num,0,3)='" + model[0].Col131 + "'";
                        vch_num = model[0].Col131 + sgen.genNo(userCode, mq, 6, "auto_genid");
                        //vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                        isedit = false;
                        model[0].Col17 = vch_num;
                        model[0].Col97 = "N";
                    }
                    if (command != "hfbtnyes")
                    {
                        string cond_vice = "0";
                        string ld_chk = "0";
                        if (model[0].Col18 != null)
                        {
                            string cond = sgen.seekval(userCode, "select c_name from " + model[0].Col10.Trim() + " where " +
                                  "type in ('" + model[0].Col12.Trim() + "','DD" + model[0].Col12 + "')" + model[0].Col11 + " " +
                                    " and  upper(c_name)='" + model[0].Col18.Trim().ToString().ToUpper() + "' and substr(vch_num,0,3)='" + model[0].Col131 + "' " + mq1 + "", "c_name");
                            if (m_module3 == "crmvmain")
                            {
                                if (model[0].Col17.Substring(0, 3) == "303")
                                {
                                    cond_vice = sgen.seekval(userCode, "select c_name from " + model[0].Col10.Trim() + " where " +
                                 "type in ('PRO','DDPRO')" + model[0].Col11 + " " +
                                   " and  upper(c_name)='" + model[0].Col18.Trim().ToString().ToUpper() + "' and substr(vch_num,0,3)=909 " + mq1 + "", "c_name");
                                }
                                if (model[0].Col17.Substring(0, 3) == "909")
                                {
                                    cond_vice = sgen.seekval(userCode, "select c_name from " + model[0].Col10.Trim() + " where " +
                                 "type in ('BCD','DDBCD')" + model[0].Col11 + " " +
                                   " and  upper(c_name)='" + model[0].Col18.Trim().ToString().ToUpper() + "' and substr(vch_num,0,3)=303 " + mq1 + "", "c_name");
                                }
                                ld_chk = sgen.seekval(userCode, "select cust_name from lead_master where type='LED' " + model[0].Col11 + " " +
                                     "and  upper(cust_name)='" + model[0].Col18.Trim().ToString().ToUpper() + "' and cust_id!=0", "cust_name");
                                if ((!cond.Equals("0")) || (!cond_vice.Equals("0")) || (!ld_chk.Equals("0")))
                                {
                                    ViewBag.scripCall = "showmsgJS(2, 'Account Name Already Exists. Do You Want To Continue', 1);";
                                    ViewBag.vnew = "disabled='disabled'";
                                    ViewBag.vedit = "disabled='disabled'";
                                    ViewBag.vsave = "";
                                    ViewBag.vsavenew = "";
                                    ModelState.Clear();
                                    return View(model);
                                }
                            }
                            else
                            {
                                if (!cond.Equals("0"))
                                {
                                    ViewBag.scripCall = "showmsgJS(2, 'Account Name Already Exists. Do You Want To Continue', 1);";
                                    ViewBag.vnew = "disabled='disabled'";
                                    ViewBag.vedit = "disabled='disabled'";
                                    ViewBag.vsave = "";
                                    ViewBag.vsavenew = "";
                                    ModelState.Clear();
                                    return View(model);
                                }
                            }
                        }

                        sgen.SetSession(MyGuid, "CL_VCHNUM", model[0].Col17);
                        if (model[0].Chk3 == true)
                        {
                            sez = "Y";
                        }
                        else { sez = "N"; }
                        if (model[0].Chk4 == true)
                        {
                            Isbilling = "Y";
                        }
                        else { Isbilling = "N"; }
                    }

                    if ((model[0].SelectedItems4.FirstOrDefault() == "") || (model[0].SelectedItems4.FirstOrDefault() == "0") || (model[0].SelectedItems4.FirstOrDefault() == null)) { model[0].SelectedItems4 = new string[] { "001" }; }
                    #region dtstr
                    if ((model[0].Col93 == null) || (model[0].Col93 == "")) model[0].Col93 = "ab@ab.ab";
                    if ((model[0].Col95 == null) || (model[0].Col95 == "") || (model[0].Col95 == "0")) model[0].Col95 = model[0].Col17;
                    DataRow dr = dtstr.NewRow();
                    dr["vch_num"] = model[0].Col17.Trim();
                    dr["vch_date"] = currdate;
                    dr["type"] = type.Trim();
                    dr["status"] = status.Trim();
                    dr["c_name"] = model[0].Col18;
                    dr["type_desc"] = model[0].Col25;//search text                   
                    dr["lsrno"] = model[0].Col32;//leisure value
                    dr["bnkaddr"] = model[0].Col33;//bank address
                    dr["micrno"] = model[0].Col34;
                    dr["website"] = model[0].Col88;
                    dr["cfrm"] = "Y";
                    dr["c_gstin"] = "Not Registered";
                    var cgstin = model[0].Col24;
                    if (isgstr == "Y" && cgstin != null)
                    {
                        dr["isgstr"] = isgstr;
                        dr["c_gstin"] = cgstin;
                        dr["tor"] = model[0].Col35;
                    }
                    if ((model[0].Col14 == "40002.1") && (model[0].Col66 != ""))
                    {
                        dr["Pros"] = "Y";//Pros data convert into account
                        dr["Prosno"] = model[0].Col66;//Pros data vch_num
                    }
                    if (model[0].Col14 == "40002.9")
                    {
                        dr["Pros"] = "N";//Pros data
                    }
                    if (model[0].Col36 == null)
                    { model[0].Col36 = "AAAAA0000A"; }
                    dr["panno"] = model[0].Col36;
                    dr["msmeno"] = model[0].Col37;
                    dr["tanno"] = model[0].Col38;


                    #region permanent address
                    dr["addtype1"] = model[0].SelectedItems12.FirstOrDefault().ToString().Trim(); // Address type 1
                    dr["country"] = model[0].Col49;//country_id//////////////
                    dr["state"] = model[0].Col71;//state_id//////////////                   
                    dr["city"] = model[0].Col64; //village ref vchnum///////////////
                    dr["addr"] = model[0].Col22;
                    dr["pincode"] = model[0].Col23;
                    #endregion

                    #region billing address
                    if (model[0].Chk4 == false)
                    {
                        if (model[0].SelectedItems13.FirstOrDefault() == "") { dr["addtype2"] = model[0].SelectedItems12.FirstOrDefault().ToString().Trim(); }
                        else { dr["addtype2"] = model[0].SelectedItems13.FirstOrDefault().ToString().Trim(); }
                        if (model[0].Col72 == "") { dr["country2"] = model[0].Col49; }
                        else { dr["country2"] = model[0].Col72; }
                        if (model[0].Col73 == "") { dr["state2"] = model[0].Col71; }
                        else { dr["state2"] = model[0].Col73; }
                        if (model[0].Col48 == "") { dr["city2"] = model[0].Col64; }
                        else { dr["city2"] = model[0].Col48; }
                        if (model[0].Col70 == "-") { dr["cpaddr2"] = model[0].Col22; }
                        else { dr["cpaddr2"] = model[0].Col70; }
                        if (model[0].Col79 == "") { dr["pincode_2"] = model[0].Col23; }
                        else { dr["pincode_2"] = model[0].Col79; }
                    }
                    else
                    {
                        dr["addtype2"] = model[0].SelectedItems13.FirstOrDefault().ToString().Trim(); // Address type 2
                        dr["country2"] = model[0].Col72;//country id 2//////////////
                        dr["state2"] = model[0].Col73;//state_id 2//////////////
                        dr["city2"] = model[0].Col48; //village ref vchnum billing add////////////  
                        dr["cpaddr2"] = model[0].Col70;
                        dr["pincode_2"] = model[0].Col79;
                    }
                    #endregion

                    #region shipping address
                    if (model[0].Chk11 == false)
                    {
                        if (model[0].SelectedItems33.FirstOrDefault() == "") { dr["addtype3"] = model[0].SelectedItems12.FirstOrDefault().ToString().Trim(); }
                        else { dr["addtype3"] = model[0].SelectedItems33.FirstOrDefault().ToString().Trim(); }
                        if (model[0].Col176 == "") { dr["country3"] = model[0].Col49; }
                        else { dr["country3"] = model[0].Col176; }
                        if (model[0].Col175 == "") { dr["state3"] = model[0].Col71; }
                        else { dr["state3"] = model[0].Col175; }
                        if (model[0].Col174 == "") { dr["city3"] = model[0].Col64; }
                        else { dr["city3"] = model[0].Col174; }
                        if (model[0].Col154 == "-") { dr["cpaddr3"] = model[0].Col22; }
                        else { dr["cpaddr3"] = model[0].Col154; }
                        if (model[0].Col155 == "") { dr["pincode_3"] = model[0].Col23; }
                        else { dr["pincode_3"] = model[0].Col155; }

                    }
                    else
                    {
                        dr["addtype3"] = model[0].SelectedItems33.FirstOrDefault();
                        dr["country3"] = model[0].Col176;
                        dr["state3"] = model[0].Col175;
                        dr["city3"] = model[0].Col174;
                        dr["cpaddr3"] = model[0].Col154;
                        dr["pincode_3"] = model[0].Col155;
                    }
                    #endregion

                    dr["city_mn"] = model[0].Col61;
                    dr["BILL_ADD"] = Isbilling;//Is billing Add Same
                    if (model[0].Chk11 == true) { dr["ship_ADD"] = "Y"; } else { dr["ship_ADD"] = "N"; }



                    try
                    {

                        dr["MT_DT"] = sgen.Savedate(model[0].Col75, true);//meeting datetime //////////////////
                    }
                    catch { }


                    dr["remark"] = model[0].Col54;//remark
                    dr["google_add"] = model[0].Col77;
                    dr["latlong"] = model[0].Col76;
                    dr["PR_TYPE"] = model[0].SelectedItems9.FirstOrDefault().ToString().Trim(); // type of property
                    dr["refered_by"] = model[0].Col55; // refered by
                    dr["file_no"] = model[0].Col56; // old file no
                    dr["occ_type"] = model[0].SelectedItems10.FirstOrDefault().ToString().Trim(); // // occupation
                    dr["salesarea"] = model[0].SelectedItems8.FirstOrDefault().ToString().Trim(); // Sales/services
                    dr["client_type"] = model[0].SelectedItems11.FirstOrDefault().ToString().Trim(); // client type                 

                    dr["sez"] = sez;
                    dr["ptype"] = model[0].SelectedItems3.FirstOrDefault();//ptype
                    dr["ploc"] = model[0].SelectedItems4.FirstOrDefault();//ploc                
                    dr["acctype"] = model[0].SelectedItems5.FirstOrDefault();
                    dr["curtype"] = model[0].SelectedItems6.FirstOrDefault();
                    dr["bnk"] = model[0].SelectedItems7.FirstOrDefault();
                    dr["swftcd"] = model[0].Col50;
                    dr["contr"] = iscontractor.Trim();
                    dr["acctno"] = model[0].Col51;
                    dr["ifsc"] = model[0].Col52;


                    dr["sch_cat"] = model[0].Col80;
                    dr["sch_med"] = model[0].Col81;
                    dr["no_std"] = model[0].Col82;
                    dr["no_teach"] = model[0].Col83;
                    dr["Aff_type"] = model[0].Col84;
                    dr["leadsrc"] = model[0].SelectedItems14.FirstOrDefault();
                    dr["bsn_type"] = string.Join(",", model[0].SelectedItems15);
                    dr["prd_type"] = string.Join(",", model[0].SelectedItems16);
                    dr["pay_term"] = model[0].SelectedItems17.FirstOrDefault();
                    dr["pay_term2"] = model[0].SelectedItems30.FirstOrDefault();
                    dr["fix_credit"] = model[0].Col98;
                    dr["temp_credit"] = model[0].Col99;
                    dr["valid_credit"] = sgen.Make_date_S(model[0].Col101);
                    if ((model[0].Col102 == null) || (model[0].Col102 == "0"))
                    {
                        dr["msme_from"] = "01/01/1900";
                    }
                    else
                    {
                        dr["msme_from"] = sgen.Make_date_S(model[0].Col102);
                    }
                    if ((model[0].Col103 == null) || (model[0].Col103 == "0"))
                    {
                        dr["msme_upto"] = "01/01/1900";
                    }
                    else
                    {
                        dr["msme_upto"] = sgen.Make_date_S(model[0].Col103);
                    }
                    if ((model[0].Col104 == null) || (model[0].Col104 == "0"))
                    {
                        dr["msme_cert"] = "01/01/1900";
                    }
                    else
                    {
                        dr["msme_cert"] = sgen.Make_date_S(model[0].Col104);
                    }
                    dr["rel_mgr"] = string.Join(",", model[0].SelectedItems18);
                    dr["sub_broker"] = string.Join(",", model[0].SelectedItems19);
                    dr["client_rating"] = string.Join(",", model[0].SelectedItems20);
                    dr["qualification"] = string.Join(",", model[0].SelectedItems21);
                    dr["dp"] = string.Join(",", model[0].SelectedItems22);
                    dr["ref_user"] = string.Join(",", model[0].SelectedItems25);
                    dr["ref_Ext_acc"] = model[0].Col138;
                    if (model[0].LTM2 != null)
                    {
                        if ((model[0].LTM2[0].Col53 == null) || (model[0].LTM2[0].Col53 == "")) model[0].LTM2[0].Col53 = "0000000000";
                        if ((model[0].LTM2[0].Col27 == null) || (model[0].LTM2[0].Col27 == "")) model[0].LTM2[0].Col27 = "0000000000";
                        if ((model[0].LTM2[0].Col28 == null) || (model[0].LTM2[0].Col28 == "")) model[0].LTM2[0].Col28 = "0000000000";
                        if ((model[0].LTM2[0].Col29 == null) || (model[0].LTM2[0].Col29 == "")) model[0].LTM2[0].Col29 = "ab@ab.ab";
                        dr["cpname"] = model[0].LTM2[0].Col26;//cont per name


                        dr["cpcont"] = model[0].LTM2[0].Col27;//contno
                        dr["cpaltcont"] = model[0].LTM2[0].Col28;//alt contno
                        dr["cpemail"] = model[0].LTM2[0].Col29;//emailid
                        dr["cpaddr"] = model[0].LTM2[0].Col30;//cpaddr
                        dr["cpdesig"] = model[0].LTM2[0].Col31;//cpdesig
                        dr["cplandno"] = model[0].LTM2[0].Col53;
                        dr["cpdept"] = model[0].LTM2[0].Col78;//department
                        dr["cp_gender"] = model[0].LTM2[0].SelectedItems32.FirstOrDefault();//gender

                        if ((model[0].LTM2[0].Col20 == null) || (model[0].LTM2[0].Col20 == "0"))//dob///////////////
                        {
                            dr["DOB"] = "01/01/1900";
                        }
                        else
                        {
                            dr["DOB"] = sgen.Make_date_S(model[0].LTM2[0].Col20);
                        }

                        if ((model[0].LTM2[0].Col21 == null) || (model[0].LTM2[0].Col21 == "0"))//doa/////////////////
                        {
                            dr["DOA"] = "01/01/1900";
                        }
                        else
                        {
                            dr["DOA"] = sgen.Make_date_S(model[0].LTM2[0].Col21);
                        }
                        dr["cp_mname"] = model[0].LTM2[0].Col89;//middle name
                        dr["cp_lname"] = model[0].LTM2[0].Col90; //last name
                        dr["cp_alias_name"] = model[0].LTM2[0].Col126;//cp_alias name
                        dr["cpcontother"] = model[0].LTM2[0].Col110;//other contact no
                        dr["cpemailother"] = model[0].LTM2[0].Col111;//other email id
                        if (model[0].LTM2[0].Chk1 == true) { dr["iscp_sms"] = true; { dr["iscp_sms"] = "N"; } }
                        if (model[0].LTM2[0].Chk2 == true) { dr["Iscp_email"] = true; { dr["Iscp_email"] = "N"; } }
                    }
                    dr["CIN_Number"] = model[0].Col92;
                    dr["comp_email"] = model[0].Col93;
                    dr["Parent_id"] = model[0].Col95;
                    dr["pref_add"] = model[0].Col107;
                    dr["alias_name"] = model[0].Col108;
                    dr["aadhar_no"] = model[0].Col109;
                    dr["dp_name"] = model[0].Col110;
                    dr["dp_id"] = model[0].Col111;
                    dr["ben_acc"] = model[0].Col112;
                    dr["uin_no"] = model[0].Col113;
                    dr["min_no"] = model[0].Col114;
                    dr["ann_inc"] = string.Join(",", model[0].SelectedItems23);
                    dr["credit_rating"] = string.Join(",", model[0].SelectedItems24);
                    dr["interunit"] = string.Join(",", model[0].SelectedItems26);
                    dr["pay_mode"] = model[0].SelectedItems27.FirstOrDefault();
                    dr["pay_mode2"] = model[0].SelectedItems31.FirstOrDefault();
                    dr["cheq_print"] = string.Join(",", model[0].SelectedItems28);
                    dr["tin_no"] = model[0].Col115;
                    dr["employer"] = model[0].Col116;
                    dr["infavour"] = model[0].Col139;
                    dr["credit_days"] = model[0].Col144;
                    dr["d_limit"] = model[0].Col145;
                    dr["s_limit"] = model[0].Col146;
                    dr["p_title"] = model[0].Col119;
                    dr["org_mobile"] = model[0].Col147;
                    dr["org_contact"] = model[0].Col148;
                    if (model[0].Chk8 == true) { dr["Isorg_email"] = true; { dr["Isorg_email"] = "Y"; } }
                    if (model[0].Chk9 == true) { dr["ISorg_sms"] = true; { dr["ISorg_sms"] = "Y"; } }
                    if (model[0].Chk5 == true) { dr["pubr"] = true; { dr["pubr"] = "Y"; } }
                    if (model[0].Chk6 == true) { dr["mftr"] = true; { dr["mftr"] = "Y"; } }
                    if (model[0].Chk7 == true) { dr["trans"] = true; { dr["trans"] = "Y"; } }
                    dr["risk_ctg"] = string.Join(",", model[0].SelectedItems29);
                    if ((model[0].Col117 == null) || (model[0].Col117 == "0"))
                    {
                        dr["kyc_dt"] = "01/01/1900";
                    }
                    else
                    {
                        dr["kyc_dt"] = sgen.Make_date_S(model[0].Col103);
                    }

                    if (model[0].Chk10 == true) { dr["s_citizen"] = true; { dr["s_citizen"] = "Y"; } }
                    dr["taxable"] = model[0].Col149;
                    dr["ghsub"] = model[0].Col150;


                    if (model[0].Chk12 == true) { dr["istds"] = "Y"; } else { dr["istds"] = "N"; }
                    dr["d_name"] = model[0].Col157;
                    dr["d_pan"] = model[0].Col158;
                    dr["d_type"] = model[0].SelectedItems34.FirstOrDefault();
                    if (model[0].Chk13 == true) { dr["isnri"] = "Y"; } else { dr["isnri"] = "N"; }
                    dr["nri_tax"] = model[0].Col159;
                    dr["nri_country"] = model[0].SelectedItems35.FirstOrDefault();
                    dr["d_add"] = model[0].Col170;
                    dr["d_address"] = model[0].Col160;
                    dr["d_pincode"] = model[0].Col161;
                    dr["d_city"] = model[0].Col173;
                    dr["d_state"] = model[0].Col172;
                    dr["d_country"] = model[0].Col171;
                    dr["tds_d"] = model[0].Col165;
                    dr["tds_rate"] = model[0].Col166;
                    dr["tds_limit"] = model[0].Col167;
                    if (model[0].Chk14 == true) { dr["d_lower"] = "Y"; } else { dr["d_lower"] = "N"; }
                    dr["d_cert"] = model[0].Col168;//
                    dr["d_valid"] = sgen.Make_date_S(model[0].Col169);
                    dr["trcom"] = model[0].Chk15 == true ? "Y" : "N";



                    if (isedit)
                    {
                        dr["client_id"] = model[0].Col1.Trim();
                        dr["client_unit_id"] = model[0].Col2.Trim();
                        dr["vch_num"] = model[0].Col17;
                        try { dr["rec_id"] = model[0].Col7; } catch { }
                        dr["ent_by"] = model[0].Col3;
                        dr["ent_date"] = model[0].Col4;
                        dr["edit_by"] = userid_mst;
                        dr["edit_date"] = currdate;
                    }
                    else
                    {
                        dr["rec_id"] = "0";
                        dr["client_id"] = clientid_mst;
                        dr["client_unit_id"] = unitid_mst;
                        dr["ent_by"] = userid_mst;
                        dr["ent_date"] = currdate;
                        dr["edit_by"] = "-";
                        dr["edit_date"] = currdate;
                    }
                    #endregion
                    dtstr.Rows.Add(dr);
                    // ok = sgen.Update_data_fast1(userCode, dtstr, model[0].Col10, tmodel.Col8, isedit);
                    Satransaction sat1 = new Satransaction(userCode, MyGuid);
                    var ok0 = sgen.Update_data_fast1_uncommit(userCode, dtstr, model[0].Col10, tmodel.Col8, isedit, sat1);
                    //if (ok == true)
                    //{
                    if ((model[0].Col14 == "40002.1") && (model[0].Col66 != "") && (model[0].Col66 != null))
                    {
                        mq = "update clients_mst set Pros = 'Y' where vch_num = '" + model[0].Col66 + "' and type = 'PRO' and find_in_set(client_unit_id,'" + unitid_mst + "')=1  and substr(vch_num,0,3)=909";
                        sgen.execute_cmd(userCode, mq);
                    }
                    DataTable dtfile = new DataTable();
                    //dtfile = sgen.getdata(userCode, "select * from file_tab WHERE 1=2");
                    dtfile = cmd_Fun.GetStructure(userCode, "file_tab");
                    string serverpath = Server.MapPath("~/Uploads/" + cg_com_name.Replace(" ", "") + "/");
                    #region contact detail
                    if (model[0].LTM2 != null)
                    {
                        dtstr = new DataTable();
                        dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                        int r = 0;
                        for (int k = 0; k < model[0].LTM2.Count; k++)
                        {
                            if (model[0].LTM2[k].Col132 == null || model[0].LTM2[k].Col132 == "")
                            {
                                mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where  type in " +
                                    "('" + model[0].Col12.Trim() + "' ,'DD" + model[0].Col12 + "')" + model[0].Col11.Trim() + " and ref_code='" + model[0].Col17.Trim() + "' and substr(ref_code,0,3)='" + model[0].Col131 + "'";
                                vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                                vch_num = (sgen.Make_decimal(vch_num) + sgen.Make_decimal(r)).ToString();
                                vch_num = sgen.padlc(sgen.Make_int(vch_num), 6, "0");
                                r = r + 1;
                            }
                            else { vch_num = model[0].LTM2[k].Col132; }
                            if ((model[0].LTM2[k].Col53 == null) || (model[0].LTM2[k].Col53 == "")) model[0].LTM2[k].Col53 = "0000000000";
                            if ((model[0].LTM2[k].Col27 == null) || (model[0].LTM2[k].Col27 == "")) model[0].LTM2[k].Col27 = "0000000000";
                            if ((model[0].LTM2[k].Col28 == null) || (model[0].LTM2[k].Col28 == "")) model[0].LTM2[k].Col28 = "0000000000";
                            if ((model[0].LTM2[k].Col29 == null) || (model[0].LTM2[k].Col29 == "")) model[0].LTM2[k].Col29 = "ab@ab.ab";
                            dr = dtstr.NewRow();
                            dr["ref_code"] = model[0].Col17.Trim();
                            dr["vch_num"] = vch_num;
                            dr["vch_date"] = currdate;
                            dr["type"] = model[0].Col91.Trim(); ;
                            dr["cpname"] = model[0].LTM2[k].Col26;//cont per name
                            dr["cp_mname"] = model[0].LTM2[k].Col89;//cont per middle name
                            dr["cp_lname"] = model[0].LTM2[k].Col90;//cont per last name
                            dr["cpcont"] = model[0].LTM2[k].Col27;//contno
                            dr["cpaltcont"] = model[0].LTM2[k].Col28;//alt contno
                            dr["cpemail"] = model[0].LTM2[k].Col29;//emailid
                            dr["cpaddr"] = model[0].LTM2[k].Col30;//cpaddr
                            dr["cpdesig"] = model[0].LTM2[k].Col31;//cpdesig
                            dr["cplandno"] = model[0].LTM2[k].Col53;//landline                            
                            dr["cpdept"] = model[0].LTM2[k].Col78;//department
                            dr["cp_gender"] = model[0].LTM2[k].SelectedItems32.FirstOrDefault();//gender
                            if ((model[0].LTM2[k].Col20 == null) || (model[0].LTM2[k].Col20 == "0"))//dob///////////////
                            {
                                dr["DOB"] = "01/01/1900";
                            }
                            else
                            {
                                dr["DOB"] = sgen.Make_date_S(model[0].LTM2[k].Col20);
                            }

                            if ((model[0].LTM2[k].Col21 == null) || (model[0].LTM2[k].Col21 == "0"))//doa/////////////////
                            {
                                dr["DOA"] = "01/01/1900";
                            }
                            else
                            {
                                dr["DOA"] = sgen.Make_date_S(model[0].LTM2[k].Col21);
                            }
                            dr["cp_alias_name"] = model[0].LTM2[k].Col126;//cp_alias name
                            dr["Parent_id"] = model[0].Col95;
                            dr["p_title"] = model[0].Col119;
                            dr["cpcontother"] = model[0].LTM2[k].Col110;//other contact no
                            dr["cpemailother"] = model[0].LTM2[k].Col111;//other email id
                            if (model[0].LTM2[k].Chk1 == true) { dr["iscp_sms"] = true; { dr["iscp_sms"] = "N"; } }
                            if (model[0].LTM2[k].Chk2 == true) { dr["Iscp_email"] = true; { dr["Iscp_email"] = "N"; } }
                            dr["rec_id"] = "0";
                            if (isedit)
                            {
                                dr["client_id"] = model[0].Col1.Trim();
                                dr["client_unit_id"] = model[0].Col2.Trim();
                                dr["ent_by"] = model[0].Col3;
                                dr["ent_date"] = model[0].Col4;
                                dr["edit_by"] = userid_mst;
                                dr["edit_date"] = currdate;
                            }
                            else
                            {
                                dr["client_id"] = clientid_mst;
                                dr["client_unit_id"] = unitid_mst;
                                dr["ent_by"] = userid_mst;
                                dr["ent_date"] = currdate;
                                dr["edit_by"] = "-";
                                dr["edit_date"] = currdate;
                            }
                            dtstr.Rows.Add(dr);
                        }
                        //ok = sgen.Update_data(userCode, dtstr, model[0].Col10, tmodel.Col85, isedit);
                        ok = sgen.Update_data_fast1_uncommit(userCode, dtstr, model[0].Col10, tmodel.Col85, isedit, sat1);
                    }
                    else { ok = true; }
                    #endregion
                    #region attachment
                    //Pan Card
                    try
                    {
                        HttpPostedFileBase file1 = upd1;
                        ctype1 = file1.ContentType;
                        fileName1 = file1.FileName;
                        path1 = clientid_mst + unitid_mst + userid_mst + currdate + "_" + fileName1;
                        encpath1 = sgen.Convert_Stringto64(path1).ToString();
                        finalpath1 = serverpath + encpath1;
                        file1.SaveAs(finalpath1);
                        filesave(model, currdate, dtfile, fileName1, encpath1, "Pan Card", ctype1);
                    }
                    catch (Exception ex) { }
                    //Msme
                    try
                    {
                        HttpPostedFileBase file2 = upd2;
                        ctype2 = file2.ContentType;
                        fileName2 = file2.FileName;
                        path2 = clientid_mst + unitid_mst + userid_mst + currdate + "_" + fileName2;
                        encpath2 = sgen.Convert_Stringto64(path2).ToString();
                        finalpath2 = serverpath + encpath2;
                        file2.SaveAs(finalpath2);
                        filesave(model, currdate, dtfile, fileName2, encpath2, "Msme", ctype2);
                    }
                    catch (Exception ex) { }
                    //Gstin
                    try
                    {
                        HttpPostedFileBase file3 = upd3;
                        ctype3 = file3.ContentType;
                        fileName3 = file3.FileName;
                        path3 = clientid_mst + unitid_mst + userid_mst + currdate + "_" + fileName3;
                        encpath3 = sgen.Convert_Stringto64(path3).ToString();
                        finalpath3 = serverpath + encpath3;
                        file3.SaveAs(finalpath3);
                        filesave(model, currdate, dtfile, fileName3, encpath3, "Gstin", ctype3);
                    }
                    catch (Exception ex) { }
                    //Vd_Reg
                    try
                    {
                        HttpPostedFileBase file4 = upd4;
                        ctype4 = file4.ContentType;
                        fileName4 = file4.FileName;
                        path4 = clientid_mst + unitid_mst + userid_mst + currdate + "_" + fileName4;
                        encpath4 = sgen.Convert_Stringto64(path4).ToString();
                        finalpath4 = serverpath + encpath4;
                        file4.SaveAs(finalpath4);
                        filesave(model, currdate, dtfile, fileName4, encpath4, "Vd_Reg", ctype4);
                    }
                    catch (Exception ex) { }
                    //Chq_Copy
                    try
                    {
                        HttpPostedFileBase file5 = upd5;
                        ctype5 = file5.ContentType;
                        fileName5 = file5.FileName;
                        path5 = clientid_mst + unitid_mst + userid_mst + currdate + "_" + fileName5;
                        encpath5 = sgen.Convert_Stringto64(path5).ToString();
                        finalpath5 = serverpath + encpath5;
                        file5.SaveAs(finalpath5);
                        filesave(model, currdate, dtfile, fileName5, encpath5, "Chq_Copy", ctype5);
                    }
                    catch (Exception ex) { }
                    //client_property
                    try
                    {
                        HttpPostedFileBase file6 = upd6;
                        ctype6 = file6.ContentType;
                        fileName6 = file6.FileName;
                        path6 = clientid_mst + unitid_mst + userid_mst + currdate + "_" + fileName6;
                        encpath6 = sgen.Convert_Stringto64(path6).ToString();
                        finalpath6 = serverpath + encpath6;
                        file6.SaveAs(finalpath6);
                        filesave(model, currdate, dtfile, fileName6, encpath6, "Property", ctype6);
                    }
                    catch (Exception ex) { }
                    //contact person Image
                    try
                    {
                        HttpPostedFileBase file7 = upd7;
                        ctype7 = file7.ContentType;
                        fileName7 = file7.FileName;
                        path7 = clientid_mst + unitid_mst + userid_mst + currdate + "_" + fileName7;
                        encpath7 = sgen.Convert_Stringto64(path7).ToString();
                        finalpath7 = serverpath + encpath7;
                        file7.SaveAs(finalpath7);
                        filesave(model, currdate, dtfile, fileName7, encpath7, "Client", ctype7);
                    }
                    catch (Exception ex) { }
                    try
                    {
                        HttpPostedFileBase file8 = upd8;
                        ctype8 = file8.ContentType;
                        fileName8 = file8.FileName;
                        path8 = clientid_mst + unitid_mst + userid_mst + currdate + "_" + fileName8;
                        encpath8 = sgen.Convert_Stringto64(path8).ToString();
                        finalpath8 = serverpath + encpath8;
                        file8.SaveAs(finalpath8);
                        filesave(model, currdate, dtfile, fileName8, encpath8, "Identity", ctype8);
                    }
                    catch (Exception ex) { }
                    try
                    {
                        HttpPostedFileBase file9 = upd9;
                        ctype9 = file9.ContentType;
                        fileName9 = file9.FileName;
                        path9 = clientid_mst + unitid_mst + userid_mst + currdate + "_" + fileName9;
                        encpath9 = sgen.Convert_Stringto64(path9).ToString();
                        finalpath9 = serverpath + encpath9;
                        file9.SaveAs(finalpath9);
                        filesave(model, currdate, dtfile, fileName9, encpath9, "Residence", ctype9);
                    }
                    catch (Exception ex) { }
                    try
                    {
                        HttpPostedFileBase file10 = upd10;
                        ctype10 = file10.ContentType;
                        fileName10 = file10.FileName;
                        path10 = clientid_mst + unitid_mst + userid_mst + currdate + "_" + fileName10;
                        encpath10 = sgen.Convert_Stringto64(path10).ToString();
                        finalpath10 = serverpath + encpath10;
                        file10.SaveAs(finalpath10);
                        filesave(model, currdate, dtfile, fileName10, encpath10, "KYC", ctype10);
                    }
                    catch (Exception ex) { }

                    //Aadhar Card Copy
                    try
                    {
                        HttpPostedFileBase file11 = upd11;
                        ctype11 = file11.ContentType;
                        fileName11 = file11.FileName;
                        path11 = clientid_mst + unitid_mst + userid_mst + currdate + "_" + fileName11;
                        encpath11 = sgen.Convert_Stringto64(path11).ToString();
                        finalpath11 = serverpath + encpath11;
                        file11.SaveAs(finalpath11);
                        filesave(model, currdate, dtfile, fileName11, encpath11, "Aadhar Card Copy", ctype11);
                    }
                    catch (Exception ex) { }
                    res = sgen.Update_data_fast1_uncommit(userCode, dtfile, "file_tab", "", false, sat1);
                    #endregion
                    #region Remark
                    bool okk = false;
                    if ((model[0].Col142 != null) && (model[0].Col142 != "") && (isedit == true))
                    {
                        mq = "select max(to_number(vch_num)) as auto_genid from kc_tab a where type='RMK'" + model[0].Col11.Trim() + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                        dtstr = new DataTable();
                        dtstr = cmd_Fun.GetStructure(userCode, "kc_tab");
                        dr = dtstr.NewRow();
                        dr["vch_num"] = vch_num;
                        dr["vch_date"] = currdate;
                        dr["type"] = "RMK";
                        dr["col1"] = model[0].Col17.Trim();
                        dr["col2"] = userid_mst;
                        dr["col48"] = model[0].Col142.Trim();
                        dr["date1"] = currdate;
                        dr["client_id"] = clientid_mst;
                        dr["client_unit_id"] = unitid_mst;
                        dr["ent_by"] = userid_mst;
                        dr["ent_date"] = currdate;
                        dr["edit_by"] = "-";
                        dr["edit_date"] = currdate;
                        dtstr.Rows.Add(dr);
                        okk = sgen.Update_data_fast1_uncommit(userCode, dtstr, "kc_tab", "", false, sat1);
                    }
                    else { okk = true; }
                    #endregion
                    //res = sgen.Update_data(userCode, dtfile, "file_tab", "", false);
                    if (ok0 && ok && okk)
                    {
                        sat1.Commit();
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain()
                        {
                            Col9 = tmodel.Col9,
                            Col10 = tmodel.Col10,
                            Col11 = tmodel.Col11,
                            Col12 = tmodel.Col12,
                            Col91 = tmodel.Col91,
                            Col13 = "Save",
                            Col100 = "Save & New",
                            Col121 = "S",
                            Col122 = "<u>S</u>ave",
                            Col123 = "Save & Ne<u>w</u>",
                            Col125 = tmodel.Col125,
                            Col14 = tmodel.Col14,
                            Col15 = tmodel.Col15,
                            Col39 = tmodel.Col39,
                            Col40 = tmodel.Col40,
                            Col41 = tmodel.Col41,
                            Col42 = tmodel.Col42,
                            Col119 = tmodel.Col119,
                            Col131 = tmodel.Col131,
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
                            TList12 = mod12,
                            TList13 = mod13,
                            TList14 = mod14,
                            TList15 = mod15,
                            TList16 = mod16,
                            TList17 = mod17,
                            TList18 = mod18,
                            TList19 = mod19,
                            TList20 = mod20,
                            TList21 = mod21,
                            TList22 = mod22,
                            TList23 = mod23,
                            TList24 = mod24,
                            TList25 = mod25,
                            TList26 = mod26,
                            TList27 = mod27,
                            TList28 = mod28,
                            TList29 = mod29,
                            TList30 = mod30,
                            TList31 = mod31,
                            TList33 = mod33,
                            TList34 = mod34,
                            TList35 = mod35,
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
                            SelectedItems12 = new string[] { "" },
                            SelectedItems13 = new string[] { "" },
                            SelectedItems14 = new string[] { "" },
                            SelectedItems15 = new string[] { "" },
                            SelectedItems16 = new string[] { "" },
                            SelectedItems17 = new string[] { "" },
                            SelectedItems18 = new string[] { "" },
                            SelectedItems19 = new string[] { "" },
                            SelectedItems20 = new string[] { "" },
                            SelectedItems21 = new string[] { "" },
                            SelectedItems22 = new string[] { "" },
                            SelectedItems23 = new string[] { "" },
                            SelectedItems24 = new string[] { "" },
                            SelectedItems25 = new string[] { "" },
                            SelectedItems26 = new string[] { "" },
                            SelectedItems27 = new string[] { "" },
                            SelectedItems28 = new string[] { "" },
                            SelectedItems29 = new string[] { "" },
                            SelectedItems30 = new string[] { "" },
                            SelectedItems31 = new string[] { "" },
                            SelectedItems33 = new string[] { "" },
                            SelectedItems34 = new string[] { "" },
                            SelectedItems35 = new string[] { "" },
                            LTM1 = new List<Tmodelmain>(),
                            LTM2 = new List<Tmodelmain>()
                        });



                        if (command == "hfbtnyes")
                        {
                            command = commval;
                        }
                        if (command.Trim().ToUpper().Equals("SAVE") || command.Trim().ToUpper().Equals("UPDATE"))
                        {
                            sgen.SetSession(MyGuid, "RNEW", "N");
                            ViewBag.vnew = "";
                            ViewBag.vedit = "";
                            ViewBag.vsave = "disabled='disabled'";
                            ViewBag.vsavenew = "disabled='disabled'";
                            ViewBag.vsavenew = "disabled='disabled'";
                            ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                        }
                        else if (command.Trim().ToUpper().Equals("SAVE & NEW") || command.Trim().ToUpper().Equals("UPDATE & NEW"))
                        {
                            sgen.SetSession(MyGuid, "RNEW", "Y");
                            model = newparty(model);
                            ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                        }
                        Tmodelmain tmltm2 = new Tmodelmain();
                        tmltm2.Col1 = "1";
                        tmltm2.TList32 = mod32;
                        tmltm2.SelectedItems32 = new string[] { "" };
                        model[0].LTM2.Add(tmltm2);
                        if ((model[0].Col14 == "40002.1") && (tmodel.Col96 != "") && (tmodel.Col96 != null))
                        {
                            return RedirectToAction("lead_convert", "vastu", new { fstr = EncryptDecrypt.Encrypt(tmodel.Col96) });
                        }
                    }
                    else
                    {
                        sat1.Rollback();

                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                        ViewBag.scripCall += "mytoast('error', 'toast-top-right', 'Data Not Saved');";
                    }
                    //if (!res) { ViewBag.scripCall = "showmsgJS(1, 'Something Went Wrong, File Not Save', 0);"; return View(model); }
                    //}                   
                    //else { ViewBag.scripCall += "showmsgJS(1, 'Record Not Saved', 0);"; }
                }
                else if (command == "btnst")
                {
                    #region state                   
                    mq = "select distinct state_name,state_gst_code from country_state where alpha_2='" + model[0].SelectedItems1.FirstOrDefault() + "' and state_name!='-' order by state_name";
                    dt = sgen.getdata(userCode, mq);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            mod2.Add(new SelectListItem { Text = dr["state_name"].ToString(), Value = dr["state_gst_code"].ToString() });
                        }
                    }
                    TempData[MyGuid + "_Tlist2"] = mod2;
                    #endregion
                    if (model[0].Col125 == "Y")
                    {
                        if (model[0].Col76 == null) ViewBag.scripCall = ViewBag.scripCall + "initialize(28.477450,77.034138);";
                        else ViewBag.scripCall = ViewBag.scripCall + "initialize(" + model[0].Col76 + ");";
                    }
                    model[0].TList2 = mod2;
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vsavenew = "";
                }
                else if (command == "Add")
                {
                    Tmodelmain ntm1 = new Tmodelmain();
                    ntm1.Col1 = (Convert.ToInt32(model[0].LTM2.Max(x => sgen.Make_int(x.Col1))) + 1).ToString();
                    ntm1.TList32 = mod32;
                    ntm1.SelectedItems32 = new string[] { "" };
                    ntm1.Col40 = "Choose File";
                    model[0].LTM2.Add(ntm1);
                    if (model[0].Col125 == "Y")
                    {
                        if (model[0].Col76 == null) ViewBag.scripCall = ViewBag.scripCall + "initialize(28.477450,77.034138);";
                        else ViewBag.scripCall = ViewBag.scripCall + "initialize(" + model[0].Col76 + ");";
                    }
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vsavenew = "";
                }
                else if (command == "Remove")
                {
                    if (model[0].Col125 == "Y")
                    {
                        if (model[0].Col76 == null) ViewBag.scripCall = ViewBag.scripCall + "initialize(28.477450,77.034138);";
                        else ViewBag.scripCall = ViewBag.scripCall + "initialize(" + model[0].Col76 + ");";
                    }
                    if (model[0].LTM2.Count > 1) model[0].LTM2.RemoveAt(sgen.Make_int(hfrow));
                    else
                    {
                        ViewBag.scripCall = "showmsgJS(1,'You cannot remove last entry',2)";
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                        return View(model);
                    }
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vsavenew = "";
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
        #region party_unit_detail
        //public ActionResult party_unit()
        //{
        //    FillMst();
        //    chkRef();
        //    ViewBag.vnew = "";
        //    ViewBag.vedit = "";
        //    ViewBag.vsave = "disabled='disabled'";
        //    ViewBag.vsavenew = "disabled='disabled'";
        //    ViewBag.scripCall = "disableForm();";
        //    List<Tmodelmain> model = new List<Tmodelmain>();
        //    List<SelectListItem> mod1 = new List<SelectListItem>();
        //    Tmodelmain tm1 = new Tmodelmain();
        //    mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
        //    MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
        //    if (mid_mst == "40002.5")
        //    {
        //        tm1.Col9 = "CLIENT UNIT MASTER";
        //        tm1.Col12 = "UNT"; // 
        //        sgen.SetSession(MyGuid, "NC_TYPEMST", "BCD");
        //    }
        //    if (mid_mst == "28005.2")
        //    {
        //        // BANQUET HALL&& Purchase CUSTOMER DETAILS 
        //        tm1.Col9 = "VENDOR DETAIL UNIT MASTER";
        //        tm1.Col12 = "BNT"; // 
        //        sgen.SetSession(MyGuid, "NC_TYPEMST", "PVD");
        //    }
        //    tm1.Col10 = "clients_mst";
        //    tm1.Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
        //    tm1.Col13 = "Save";
        //    tm1.Col100 = "Save & New";
        //    tm1.Col14 = mid_mst.Trim();
        //    tm1.Col15 = MyGuid.Trim();
        //    tm1.Col39 = "Choose File";
        //    tm1.Col40 = "Choose File";
        //    tm1.Col41 = "Choose File";
        //    tm1.Col42 = "Choose File";
        //    tm1.Col43 = "Choose File";
        //    sgen.SetSession(MyGuid, "UNT_TYPEMST", tm1.Col12);
        //    tm1.TList1 = mod1;
        //    tm1.SelectedItems1 = new string[] { "" };
        //    tm1.TList2 = mod1;
        //    tm1.SelectedItems2 = new string[] { "" };
        //    tm1.TList3 = mod1;
        //    tm1.SelectedItems3 = new string[] { "" };
        //    tm1.TList4 = mod1;
        //    tm1.SelectedItems4 = new string[] { "" };
        //    tm1.TList5 = mod1;
        //    tm1.SelectedItems5 = new string[] { "" };
        //    tm1.TList6 = mod1;
        //    tm1.SelectedItems6 = new string[] { "" };
        //    tm1.TList7 = mod1;
        //    tm1.SelectedItems7 = new string[] { "" };
        //    tm1.TList8 = mod1;
        //    tm1.SelectedItems8 = new string[] { "" };
        //    tm1.TList9 = mod1;
        //    tm1.SelectedItems9 = new string[] { "" };
        //    tm1.TList10 = mod1;
        //    tm1.SelectedItems10 = new string[] { "" };
        //    tm1.TList11 = mod1;
        //    tm1.SelectedItems11 = new string[] { "" };
        //    tm1.TList12 = mod1;
        //    tm1.SelectedItems12 = new string[] { "" };
        //    tm1.TList13 = mod1;
        //    tm1.SelectedItems13 = new string[] { "" };
        //    tm1.LTM1 = new List<Tmodelmain>();
        //    tm1.LTM2 = new List<Tmodelmain>();
        //    Tmodelmain tmltm2 = new Tmodelmain();
        //    tmltm2.Col1 = "1";
        //    tm1.LTM2.Add(tmltm2);
        //    tm1.Col45 = "N";//chk acct type
        //    mq = "select mod_id from com_module where mod_id='22000' and com_code='" + userCode + "'";
        //    mq1 = sgen.seekval(userCode, mq, "mod_id");
        //    if (!mq1.Trim().Equals("0")) tm1.Col45 = "Y";
        //    if (tm1.Col76 == null) ViewBag.scripCall = ViewBag.scripCall + "initialize(28.477450,77.034138);";
        //    else ViewBag.scripCall = ViewBag.scripCall + "initialize(" + tm1.Col76 + ");";
        //    if (mid_mst == "40002.7")
        //    {
        //        tm1.Col9 = "CLIENT UNIT MASTER";
        //        tm1.Col12 = "UNT"; // 
        //        sgen.SetSession(MyGuid, "NC_TYPEMST", "BCD");
        //        model.Add(tm1);
        //        CallbackFun("40002.7", "N", "party_unit", "purchase", model);
        //        ViewBag.vnew = "disabled='disabled'";
        //        ViewBag.vedit = "disabled='disabled'";
        //        ViewBag.vsave = "disabled='disabled'";
        //        ViewBag.vsavenew = "disabled='disabled'";
        //        ViewBag.scripCall = "disableForm();";
        //        return View(model);
        //    }
        //    model.Add(tm1);
        //    return View(model);
        //}
        //public List<Tmodelmain> newparty_unit(List<Tmodelmain> model)
        //{
        //    model = getnew(model);
        //    model[0].Col13 = "Save";
        //    try
        //    {
        //        sgen.SetSession(MyGuid, "EDMODE", "N");
        //        ViewBag.vnew = "disabled='disabled'";
        //        ViewBag.vedit = "disabled='disabled'";
        //        ViewBag.vsave = "";
        //        ViewBag.vsavenew = "";
        //        ViewBag.scripCall = "enableForm();";
        //        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
        //        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
        //        //string defval = "";
        //        model[0].Col16 = vch_num;
        //        model[0].Col13 = "Save";
        //        model[0].Col35 = "R";
        //        model[0].Col44 = "Active";
        //        if (model[0].Col76 == null) ViewBag.scripCall = ViewBag.scripCall + "initialize(28.477450,77.034138);";
        //        else ViewBag.scripCall = ViewBag.scripCall + "initialize(" + model[0].Col76 + ");";
        //        List<SelectListItem> mod1 = new List<SelectListItem>();
        //        List<SelectListItem> mod2 = new List<SelectListItem>();
        //        List<SelectListItem> mod3 = new List<SelectListItem>();
        //        List<SelectListItem> mod4 = new List<SelectListItem>();
        //        List<SelectListItem> mod5 = new List<SelectListItem>();
        //        List<SelectListItem> mod6 = new List<SelectListItem>();
        //        List<SelectListItem> mod7 = new List<SelectListItem>();
        //        List<SelectListItem> mod8 = new List<SelectListItem>();
        //        List<SelectListItem> mod9 = new List<SelectListItem>();
        //        List<SelectListItem> mod10 = new List<SelectListItem>();
        //        List<SelectListItem> mod11 = new List<SelectListItem>();
        //        List<SelectListItem> mod12 = new List<SelectListItem>();
        //        List<SelectListItem> mod13 = new List<SelectListItem>();
        //        #region party type
        //        mod3 = cmd_Fun.partytype(userCode, unitid_mst);
        //        TempData[MyGuid + "_Tlist3"] = mod3;
        //        #endregion
        //        #region party location - fix
        //        mod4.Add(new SelectListItem { Text = "Domestic", Value = "001" });
        //        mod4.Add(new SelectListItem { Text = "Overseas", Value = "002" });
        //        TempData[MyGuid + "_Tlist4"] = mod4;
        //        #endregion
        //        #region acc type
        //        mod5 = cmd_Fun.acctypevdm(userCode, unitid_mst);
        //        TempData[MyGuid + "_Tlist5"] = mod5;
        //        #endregion
        //        #region currency type
        //        mod6 = cmd_Fun.curname(userCode, unitid_mst);
        //        TempData[MyGuid + "_Tlist6"] = mod6;
        //        #endregion
        //        #region bank name
        //        mod7 = cmd_Fun.bank(userCode, unitid_mst);
        //        TempData[MyGuid + "_Tlist7"] = mod7;
        //        #endregion
        //        #region  SALES/SERVICE AREA
        //        mod8 = cmd_Fun.salearea(userCode, unitid_mst);
        //        TempData[MyGuid + "_TList8"] = mod8;
        //        #endregion
        //        #region bindpro_type
        //        mod9 = cmd_Fun.protype(userCode, unitid_mst);
        //        TempData[MyGuid + "_TList9"] = mod9;
        //        #endregion
        //        #region occupation type
        //        mod10 = cmd_Fun.occtype(userCode, unitid_mst);
        //        TempData[MyGuid + "_TList10"] = mod10;
        //        #endregion
        //        #region  typeofclient
        //        mod11 = cmd_Fun.clienttype(userCode, unitid_mst);
        //        TempData[MyGuid + "_TList11"] = mod11;
        //        #endregion
        //        #region  typeofaddress
        //        mod12 = cmd_Fun.addresstype(userCode, unitid_mst);
        //        mod13 = cmd_Fun.addresstype(userCode, unitid_mst);
        //        TempData[MyGuid + "_TList12"] = mod12;
        //        TempData[MyGuid + "_TList13"] = mod13;
        //        #endregion
        //        model[0].TList1 = mod1;
        //        model[0].TList2 = mod2;
        //        model[0].TList3 = mod3;
        //        model[0].TList4 = mod4;
        //        model[0].TList5 = mod5;
        //        model[0].TList6 = mod6;
        //        model[0].TList7 = mod7;
        //        model[0].TList8 = mod8;
        //        model[0].TList9 = mod9;
        //        model[0].TList10 = mod10;
        //        model[0].TList11 = mod11;
        //        model[0].TList12 = mod12;
        //        model[0].TList13 = mod13;
        //    }
        //    catch (Exception ex) { }
        //    return model;
        //}
        //[HttpPost]
        //public ActionResult party_unit(List<Tmodelmain> model, string command, HttpPostedFileBase upd1, HttpPostedFileBase upd2,
        //    HttpPostedFileBase upd3, HttpPostedFileBase upd4, HttpPostedFileBase upd5, HttpPostedFileBase upd6, HttpPostedFileBase upd7, string hfrow)
        //{
        //    string ctype1 = "", ctype2 = "", ctype3 = "", ctype4 = "", ctype5 = "", ctype6 = "", ctype7 = "",
        //        finalpath1 = "", finalpath2 = "", finalpath3 = "", finalpath4 = "", finalpath5 = "", finalpath6 = "", finalpath7 = "", path1 = "", path2 = "", path3 = "", path4 = "", path5 = "", path6 = "", path7 = "",
        //        fileName1 = "", fileName2 = "", fileName3 = "", fileName4 = "", fileName5 = "", fileName6 = "", fileName7 = "", encpath1 = "", encpath2 = "", encpath3 = "", encpath4 = "", encpath5 = "", encpath6 = "", encpath7 = "",
        //    iscontractor = "N";
        //    FillMst(model[0].Col15);
        //    var tm = model.FirstOrDefault();
        //    DataTable dt = new DataTable();
        //    #region dropdown      
        //    List<SelectListItem> mod1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
        //    List<SelectListItem> mod2 = (List<SelectListItem>)TempData[MyGuid + "_TList2"];
        //    List<SelectListItem> mod3 = (List<SelectListItem>)TempData[MyGuid + "_TList3"];
        //    List<SelectListItem> mod4 = (List<SelectListItem>)TempData[MyGuid + "_TList4"];
        //    List<SelectListItem> mod5 = (List<SelectListItem>)TempData[MyGuid + "_TList5"];
        //    List<SelectListItem> mod6 = (List<SelectListItem>)TempData[MyGuid + "_TList6"];
        //    List<SelectListItem> mod7 = (List<SelectListItem>)TempData[MyGuid + "_TList7"];
        //    List<SelectListItem> mod8 = (List<SelectListItem>)TempData[MyGuid + "_TList8"];
        //    List<SelectListItem> mod9 = (List<SelectListItem>)TempData[MyGuid + "_TList9"];
        //    List<SelectListItem> mod10 = (List<SelectListItem>)TempData[MyGuid + "_TList10"];
        //    List<SelectListItem> mod11 = (List<SelectListItem>)TempData[MyGuid + "_TList11"];
        //    List<SelectListItem> mod12 = (List<SelectListItem>)TempData[MyGuid + "_TList12"];
        //    List<SelectListItem> mod13 = (List<SelectListItem>)TempData[MyGuid + "_TList13"];
        //    model[0].TList1 = mod1;
        //    model[0].TList2 = mod2;
        //    model[0].TList3 = mod3;
        //    model[0].TList4 = mod4;
        //    model[0].TList5 = mod5;
        //    model[0].TList6 = mod6;
        //    model[0].TList7 = mod7;
        //    model[0].TList8 = mod8;
        //    model[0].TList9 = mod9;
        //    model[0].TList10 = mod10;
        //    model[0].TList11 = mod11;
        //    model[0].TList12 = mod12;
        //    model[0].TList13 = mod13;
        //    TempData[MyGuid + "_TList1"] = model[0].TList1;
        //    TempData[MyGuid + "_TList2"] = model[0].TList2;
        //    TempData[MyGuid + "_TList3"] = model[0].TList3;
        //    TempData[MyGuid + "_TList4"] = model[0].TList4;
        //    TempData[MyGuid + "_TList5"] = model[0].TList5;
        //    TempData[MyGuid + "_TList6"] = model[0].TList6;
        //    TempData[MyGuid + "_TList7"] = model[0].TList7;
        //    TempData[MyGuid + "_TList8"] = model[0].TList8;
        //    TempData[MyGuid + "_TList9"] = model[0].TList9;
        //    TempData[MyGuid + "_TList10"] = model[0].TList10;
        //    TempData[MyGuid + "_TList11"] = model[0].TList11;
        //    TempData[MyGuid + "_TList12"] = model[0].TList12;
        //    TempData[MyGuid + "_TList13"] = model[0].TList13;
        //    if (model[0].SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
        //    if (model[0].SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
        //    if (model[0].SelectedItems3 == null) model[0].SelectedItems3 = new string[] { "" };
        //    if (model[0].SelectedItems4 == null) model[0].SelectedItems4 = new string[] { "" };
        //    if (model[0].SelectedItems5 == null) model[0].SelectedItems5 = new string[] { "" };
        //    if (model[0].SelectedItems6 == null) model[0].SelectedItems6 = new string[] { "" };
        //    if (model[0].SelectedItems7 == null) model[0].SelectedItems7 = new string[] { "" };
        //    if (model[0].SelectedItems8 == null) model[0].SelectedItems8 = new string[] { "" };
        //    if (model[0].SelectedItems9 == null) model[0].SelectedItems9 = new string[] { "" };
        //    if (model[0].SelectedItems10 == null) model[0].SelectedItems10 = new string[] { "" };
        //    if (model[0].SelectedItems11 == null) model[0].SelectedItems11 = new string[] { "" };
        //    if (model[0].SelectedItems12 == null) model[0].SelectedItems12 = new string[] { "" };
        //    if (model[0].SelectedItems13 == null) model[0].SelectedItems13 = new string[] { "" };
        //    #endregion
        //    if (command == "New")
        //    {
        //        model = newparty_unit(model);
        //    }
        //    else if (command == "Cancel")
        //    {
        //        return CancelFun(model);
        //    }
        //    else if (command == "Callback")
        //    {
        //        if (sgen.GetSession(MyGuid, "delid") != null) { btnval = "FDEL"; }
        //        else if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
        //        model = StartCallback(model);
        //        if (tm.Col76 == null) ViewBag.scripCall = ViewBag.scripCall + "initialize(28.477450,77.034138);";
        //        else ViewBag.scripCall = ViewBag.scripCall + "initialize(" + tm.Col76 + ");";
        //    }
        //    else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
        //    {
        //        try
        //        {
        //            string Isbilling = "", sez = "N";
        //            var tmodel = model.FirstOrDefault();
        //            string currdate = sgen.server_datetime(userCode);
        //            if (model[0].Chk1 == true) isgstr = "Y";
        //            if (model[0].Chk2 == true) iscontractor = "Y";
        //            type = model[0].Col12;
        //            if (model[0].Col44.Trim() == "Inactive")
        //            {
        //                type = "DD" + type;
        //                status = "N";
        //            }
        //            else status = "Y";
        //            DataTable dtstr = new DataTable();
        //            //dtstr = sgen.getdata(userCode, "select * from " + model[0].Col10 + " WHERE 1=2");
        //            dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
        //            if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
        //            {
        //                mq1 = " and vch_num<>'" + tmodel.Col16.Trim() + "'";
        //                isedit = true;
        //                //vch_num = tmodel.Col17.Trim();
        //                model[0].Col16 = tmodel.Col16.Trim();
        //            }
        //            else
        //            {
        //                mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where type='" + model[0].Col12 + "' " + model[0].Col11 + "  ";
        //                vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
        //                isedit = false;
        //                model[0].Col16 = vch_num;
        //            }
        //            if (model[0].Chk3 == true)
        //            {
        //                sez = "Y";
        //            }
        //            else { sez = "N"; }
        //            if (model[0].Chk4 == true)
        //            {
        //                Isbilling = "Y";
        //            }
        //            else { Isbilling = "N"; }
        //            #region dtstr
        //            DataRow dr = dtstr.NewRow();
        //            dr["vch_num"] = model[0].Col16.Trim();
        //            dr["vch_date"] = currdate;
        //            dr["type"] = type.Trim();
        //            dr["status"] = status.Trim();
        //            dr["c_name"] = model[0].Col18;
        //            dr["addr"] = model[0].Col22;
        //            dr["pincode"] = model[0].Col23;
        //            dr["type_desc"] = model[0].Col25;//search text                 
        //            dr["lsrno"] = model[0].Col32;//leisure value
        //            dr["bnkaddr"] = model[0].Col33;//bank address
        //            dr["micrno"] = model[0].Col34;
        //            dr["cfrm"] = "Y";//confirm
        //            dr["c_gstin"] = "Not Registered";
        //            var cgstin = model[0].Col24;
        //            if (isgstr == "Y" && cgstin != null)
        //            {
        //                dr["isgstr"] = isgstr;
        //                dr["c_gstin"] = cgstin;
        //                dr["tor"] = model[0].Col35;
        //            }
        //            dr["panno"] = model[0].Col17;//client ref
        //            dr["occ_type"] = "BCD";//client ref type
        //            //dr["panno"] = model[0].Col36;
        //            //dr["msmeno"] = model[0].Col37;
        //            //dr["tanno"] = model[0].Col38;                 
        //            dr["country"] = model[0].Col49;//country_id//////////////
        //            dr["state"] = model[0].Col71;//state_id//////////////
        //            dr["country2"] = model[0].Col72;//country id 2//////////////
        //            dr["state2"] = model[0].Col73;//state_id 2//////////////
        //            dr["city"] = model[0].Col64; //village ref vchnum///////////////
        //            dr["city2"] = model[0].Col48; //village ref vchnum billing add////////////                  
        //            dr["cpaddr2"] = model[0].Col70;//address//////////////
        //                                           //////dr["cplandno"] = model[0].Col53;//landline
        //            dr["google_add"] = model[0].Col77;
        //            dr["latlong"] = model[0].Col76;
        //            dr["salesarea"] = model[0].SelectedItems8.FirstOrDefault().ToString().Trim(); // Sales/services
        //            dr["client_type"] = model[0].SelectedItems11.FirstOrDefault().ToString().Trim(); // client type
        //            dr["addtype1"] = model[0].SelectedItems12.FirstOrDefault().ToString().Trim(); // Address type 1
        //            dr["addtype2"] = model[0].SelectedItems13.FirstOrDefault().ToString().Trim(); // Address type 2
        //            //only for crm client
        //            dr["BILL_ADD"] = Isbilling;//Is billing Add Same
        //            dr["sez"] = sez;
        //            dr["ptype"] = model[0].SelectedItems3.FirstOrDefault();//ptype
        //            dr["ploc"] = model[0].SelectedItems4.FirstOrDefault();//ploc                
        //            dr["acctype"] = model[0].SelectedItems5.FirstOrDefault();
        //            dr["curtype"] = model[0].SelectedItems6.FirstOrDefault();
        //            dr["bnk"] = model[0].SelectedItems7.FirstOrDefault();
        //            dr["swftcd"] = model[0].Col50;
        //            dr["contr"] = iscontractor.Trim();
        //            dr["acctno"] = model[0].Col51;
        //            dr["ifsc"] = model[0].Col52;
        //            //////////dr["cpdept"] = model[0].Col78;
        //            dr["pincode_2"] = model[0].Col79;
        //            dr["sch_cat"] = model[0].Col80;
        //            dr["sch_med"] = model[0].Col81;
        //            dr["no_std"] = model[0].Col82;
        //            dr["no_teach"] = model[0].Col83;
        //            dr["Aff_type"] = model[0].Col84;
        //            dr["website"] = model[0].Col89;
        //            dr["cpname"] = model[0].LTM2[0].Col26;//cont per name
        //            dr["cpcont"] = model[0].LTM2[0].Col27;//contno
        //            dr["cpaltcont"] = model[0].LTM2[0].Col28;//alt contno
        //            dr["cpemail"] = model[0].LTM2[0].Col29;//emailid
        //            dr["cpaddr"] = model[0].LTM2[0].Col30;//cpaddr
        //            dr["cpdesig"] = model[0].LTM2[0].Col31;//cpdesig
        //            dr["cplandno"] = model[0].LTM2[0].Col53;//landline                   
        //            dr["cpdept"] = model[0].LTM2[0].Col78;//department
        //            dr["DOB"] = sgen.Savedate(model[0].LTM2[0].Col20, false);//dob///////////////
        //            dr["DOA"] = sgen.Savedate(model[0].LTM2[0].Col21, false);//doa/////////////////
        //            dr["cp_mname"] = model[0].LTM2[0].Col90;//cont per middle name
        //            dr["cp_lname"] = model[0].LTM2[0].Col91;//cont per last name
        //            if (isedit)
        //            {
        //                dr["client_id"] = model[0].Col1.Trim();
        //                dr["client_unit_id"] = model[0].Col2.Trim();
        //                dr["rec_id"] = model[0].Col7;
        //                dr["ent_by"] = model[0].Col3;
        //                dr["ent_date"] = model[0].Col4;
        //                dr["edit_by"] = userid_mst;
        //                dr["edit_date"] = currdate;
        //            }
        //            else
        //            {
        //                dr["rec_id"] = "0";
        //                dr["client_id"] = clientid_mst;
        //                dr["client_unit_id"] = unitid_mst;
        //                dr["ent_by"] = userid_mst;
        //                dr["ent_date"] = currdate;
        //                dr["edit_by"] = "-";
        //                dr["edit_by"] = currdate;
        //            }
        //            #endregion
        //            dtstr.Rows.Add(dr);
        //            Satransaction sat1 = new Satransaction(userCode, MyGuid);
        //            var ok0 = sgen.Update_data_fast1_uncommit(userCode, dtstr, model[0].Col10, tmodel.Col8, isedit, sat1);
        //            //if (ok0 == true)
        //            {
        //                DataTable dtfile = new DataTable();
        //                //dtfile = sgen.getdata(userCode, "select * from file_tab WHERE 1=2");
        //                dtfile = cmd_Fun.GetStructure(userCode, "file_tab");
        //                string serverpath = Server.MapPath("~/Uploads/" + cg_com_name.Replace(" ", "") + "/");
        //                #region contact detail
        //                dtstr = new DataTable();
        //                dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
        //                for (int k = 0; k < model[0].LTM2.Count; k++)
        //                {
        //                    dr = dtstr.NewRow();
        //                    dr["vch_num"] = model[0].Col16.Trim();
        //                    dr["vch_date"] = currdate;
        //                    dr["type"] = "UNC";//unit contact
        //                    dr["cpname"] = model[0].LTM2[k].Col26;//cont per name
        //                    dr["cpcont"] = model[0].LTM2[k].Col27;//contno
        //                    dr["cpaltcont"] = model[0].LTM2[k].Col28;//alt contno
        //                    dr["cpemail"] = model[0].LTM2[k].Col29;//emailid
        //                    dr["cpaddr"] = model[0].LTM2[k].Col30;//cpaddr
        //                    dr["cpdesig"] = model[0].LTM2[k].Col31;//cpdesig
        //                    dr["cplandno"] = model[0].LTM2[k].Col53;//landline                            
        //                    dr["cpdept"] = model[0].LTM2[k].Col78;//department
        //                    dr["DOB"] = sgen.Savedate(model[0].LTM2[k].Col20, false);//dob///////////////
        //                    dr["DOA"] = sgen.Savedate(model[0].LTM2[k].Col21, false);//doa/////////////////
        //                    dr["cp_mname"] = model[0].LTM2[0].Col90;//cont per middle name
        //                    dr["cp_lname"] = model[0].LTM2[0].Col91;//cont per last name
        //                    dr["rec_id"] = "0";
        //                    if (isedit)
        //                    {
        //                        dr["client_id"] = model[0].Col1.Trim();
        //                        dr["client_unit_id"] = model[0].Col2.Trim();
        //                        dr["ent_by"] = model[0].Col3;
        //                        dr["ent_date"] = model[0].Col4;
        //                        dr["edit_by"] = userid_mst;
        //                        dr["edit_date"] = currdate;
        //                    }
        //                    else
        //                    {
        //                        dr["client_id"] = clientid_mst;
        //                        dr["client_unit_id"] = unitid_mst;
        //                        dr["ent_by"] = userid_mst;
        //                        dr["ent_date"] = currdate;
        //                        dr["edit_by"] = "-";
        //                        dr["edit_by"] = currdate;
        //                    }
        //                    dtstr.Rows.Add(dr);
        //                }
        //                #endregion
        //                ok = sgen.Update_data_fast1_uncommit(userCode, dtstr, model[0].Col10, tmodel.Col85, isedit, sat1);
        //                #region attachment
        //                //Pan Card
        //                try
        //                {
        //                    HttpPostedFileBase file1 = upd1;
        //                    ctype1 = file1.ContentType;
        //                    fileName1 = file1.FileName;
        //                    path1 = clientid_mst + unitid_mst + userid_mst + currdate + "_" + fileName1;
        //                    encpath1 = sgen.Convert_Stringto64(path1).ToString();
        //                    finalpath1 = serverpath + encpath1;
        //                    file1.SaveAs(finalpath1);
        //                    filesave(model, currdate, dtfile, fileName1, encpath1, "Pan Card", ctype1);
        //                }
        //                catch (Exception ex) { }
        //                //Msme
        //                try
        //                {
        //                    HttpPostedFileBase file2 = upd2;
        //                    ctype2 = file2.ContentType;
        //                    fileName2 = file2.FileName;
        //                    path2 = clientid_mst + unitid_mst + userid_mst + currdate + "_" + fileName2;
        //                    encpath2 = sgen.Convert_Stringto64(path2).ToString();
        //                    finalpath2 = serverpath + encpath2;
        //                    file2.SaveAs(finalpath2);
        //                    filesave(model, currdate, dtfile, fileName2, encpath2, "Msme", ctype2);
        //                }
        //                catch (Exception ex) { }
        //                //Gstin
        //                try
        //                {
        //                    HttpPostedFileBase file3 = upd3;
        //                    ctype3 = file3.ContentType;
        //                    fileName3 = file3.FileName;
        //                    path3 = clientid_mst + unitid_mst + userid_mst + currdate + "_" + fileName3;
        //                    encpath3 = sgen.Convert_Stringto64(path3).ToString();
        //                    finalpath3 = serverpath + encpath3;
        //                    file3.SaveAs(finalpath3);
        //                    filesave(model, currdate, dtfile, fileName3, encpath3, "Gstin", ctype3);
        //                }
        //                catch (Exception ex) { }
        //                //Vd_Reg
        //                try
        //                {
        //                    HttpPostedFileBase file4 = upd4;
        //                    ctype4 = file4.ContentType;
        //                    fileName4 = file4.FileName;
        //                    path4 = clientid_mst + unitid_mst + userid_mst + currdate + "_" + fileName4;
        //                    encpath4 = sgen.Convert_Stringto64(path4).ToString();
        //                    finalpath4 = serverpath + encpath4;
        //                    file4.SaveAs(finalpath4);
        //                    filesave(model, currdate, dtfile, fileName4, encpath4, "Vd_Reg", ctype4);
        //                }
        //                catch (Exception ex) { }
        //                //Chq_Copy
        //                try
        //                {
        //                    HttpPostedFileBase file5 = upd5;
        //                    ctype5 = file5.ContentType;
        //                    fileName5 = file5.FileName;
        //                    path5 = clientid_mst + unitid_mst + userid_mst + currdate + "_" + fileName5;
        //                    encpath5 = sgen.Convert_Stringto64(path5).ToString();
        //                    finalpath5 = serverpath + encpath5;
        //                    file5.SaveAs(finalpath5);
        //                    filesave(model, currdate, dtfile, fileName5, encpath5, "Chq_Copy", ctype5);
        //                }
        //                catch (Exception ex) { }
        //                //client_property
        //                try
        //                {
        //                    HttpPostedFileBase file6 = upd6;
        //                    ctype6 = file6.ContentType;
        //                    fileName6 = file6.FileName;
        //                    path6 = clientid_mst + unitid_mst + userid_mst + currdate + "_" + fileName6;
        //                    encpath6 = sgen.Convert_Stringto64(path6).ToString();
        //                    finalpath6 = serverpath + encpath6;
        //                    file6.SaveAs(finalpath6);
        //                    filesave(model, currdate, dtfile, fileName6, encpath6, "Property", ctype6);
        //                }
        //                catch (Exception ex) { }
        //                //contact person Image
        //                try
        //                {
        //                    HttpPostedFileBase file7 = upd7;
        //                    ctype7 = file7.ContentType;
        //                    fileName7 = file7.FileName;
        //                    path7 = clientid_mst + unitid_mst + userid_mst + currdate + "_" + fileName7;
        //                    encpath7 = sgen.Convert_Stringto64(path7).ToString();
        //                    finalpath7 = serverpath + encpath7;
        //                    file7.SaveAs(finalpath7);
        //                    filesave(model, currdate, dtfile, fileName7, encpath7, "Client", ctype7);
        //                }
        //                catch (Exception ex) { }
        //                #endregion
        //                res = sgen.Update_data_fast1_uncommit(userCode, dtfile, "file_tab", "", false, sat1);
        //                if (ok0 && ok && res)
        //                {
        //                    sat1.Commit();
        //                }
        //                else
        //                {
        //                    sat1.Rollback();
        //                    ViewBag.scripCall += "showmsgJS(1, 'Record Not Saved', 0);";
        //                }
        //                //if (!res) { ViewBag.scripCall = "showmsgJS(1, 'Something Went Wrong, File Not Save', 0);"; return View(model); }
        //                if (command.Trim().ToUpper().Equals("SAVE") || command.Trim().ToUpper().Equals("UPDATE"))
        //                {
        //                    sgen.SetSession(MyGuid, "RNEW", "N");
        //                    ViewBag.vnew = "";
        //                    ViewBag.vedit = "";
        //                    ViewBag.vsave = "disabled='disabled'";
        //                    ViewBag.vsavenew = "disabled='disabled'";
        //                    ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
        //                }
        //                else if (command.Trim().ToUpper().Equals("SAVE & NEW") || command.Trim().ToUpper().Equals("UPDATE & NEW"))
        //                {
        //                    try
        //                    {
        //                        sgen.SetSession(MyGuid, "RNEW", "Y");
        //                        model = newparty_unit(model);
        //                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
        //                    }
        //                    catch (Exception ex) { }
        //                }
        //                model = new List<Tmodelmain>();
        //                model.Add(new Tmodelmain()
        //                {
        //                    Col9 = tmodel.Col9,
        //                    Col10 = tmodel.Col10,
        //                    Col11 = tmodel.Col11,
        //                    Col12 = tmodel.Col12,
        //                    Col16 = tmodel.Col16,
        //                    Col13 = "Save",
        //                    Col100 = "Save & New",
        //                    Col14 = tmodel.Col14,
        //                    Col15 = tmodel.Col15,
        //                    Col39 = tmodel.Col39,
        //                    Col40 = tmodel.Col40,
        //                    Col41 = tmodel.Col41,
        //                    Col42 = tmodel.Col42,
        //                    Col46 = tmodel.Col46,
        //                    TList1 = mod1,
        //                    TList2 = mod2,
        //                    TList3 = mod3,
        //                    TList4 = mod4,
        //                    TList5 = mod5,
        //                    TList6 = mod6,
        //                    TList7 = mod7,
        //                    TList8 = mod8,
        //                    TList9 = mod9,
        //                    TList10 = mod10,
        //                    TList11 = mod11,
        //                    TList12 = mod12,
        //                    TList13 = mod13,
        //                    SelectedItems1 = new string[] { "" },
        //                    SelectedItems2 = new string[] { "" },
        //                    SelectedItems3 = new string[] { "" },
        //                    SelectedItems4 = new string[] { "" },
        //                    SelectedItems5 = new string[] { "" },
        //                    SelectedItems6 = new string[] { "" },
        //                    SelectedItems7 = new string[] { "" },
        //                    SelectedItems8 = new string[] { "" },
        //                    SelectedItems9 = new string[] { "" },
        //                    SelectedItems10 = new string[] { "" },
        //                    SelectedItems11 = new string[] { "" },
        //                    SelectedItems12 = new string[] { "" },
        //                    SelectedItems13 = new string[] { "" },
        //                    LTM1 = new List<Tmodelmain>(),
        //                    LTM2 = new List<Tmodelmain>()
        //                });
        //                Tmodelmain tmltm2 = new Tmodelmain();
        //                tmltm2.Col1 = "1";
        //                model[0].LTM2.Add(tmltm2);
        //            }
        //            //else { ViewBag.scripCall += "showmsgJS(1, 'Record Not Saved', 0);"; }
        //        }
        //        catch (Exception ex) { ViewBag.scripCall += "showmsgJS(1, '" + ex.Message.ToString() + "', 0);"; }
        //    }
        //    else if (command == "Add")
        //    {
        //        try
        //        {
        //            Tmodelmain ntm1 = new Tmodelmain();
        //            ntm1.Col1 = (Convert.ToInt32(model[0].LTM2.Max(x => sgen.Make_int(x.Col1))) + 1).ToString();
        //            ntm1.TList2 = mod2;
        //            ntm1.SelectedItems2 = new string[] { "" };
        //            ntm1.Col40 = "Choose File";
        //            model[0].LTM2.Add(ntm1);
        //            ViewBag.vnew = "disabled='disabled'";
        //            ViewBag.vedit = "disabled='disabled'";
        //            ViewBag.vsave = "";
        //        }
        //        catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "',0);"; }
        //    }
        //    else if (command == "Remove")
        //    {
        //        if (model[0].LTM2.Count > 1) model[0].LTM2.RemoveAt(sgen.Make_int(hfrow));
        //        else
        //        {
        //            ViewBag.scripCall = "showmsgJS(1,'You cannot remove last entry',2)";
        //            ViewBag.vnew = "disabled='disabled'";
        //            ViewBag.vedit = "disabled='disabled'";
        //            ViewBag.vsave = "";
        //            return View(model);
        //        }
        //        ViewBag.vnew = "disabled='disabled'";
        //        ViewBag.vedit = "disabled='disabled'";
        //        ViewBag.vsave = "";
        //    }
        //    ModelState.Clear();
        //    return View(model);
        //}
        #endregion
        #region PO Schedule
        public ActionResult po_sch()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            ViewBag.vimport = "disabled='disabled";
            DataTable dt = new DataTable();
            model[0].Col10 = "kc_tab";
            model[0].Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            model[0].Col40 = "BPS"; //PO shedule
            dt = sgen.getdata(userCode, "select '' as Item, '1' as SNo ,'-' as Icode,'-' as IName,'-' PartNo,'-' as UOM,'0' as Order_qty,'0' as Plan_qty," +
                "'-' as Sdl_start_dt,'-' as Sdl_end_dt,'0' as PO_No,'-' PO_Date,'-' as Remark from dual");
            model[0].dt1 = dt;
            sgen.SetSession(MyGuid, "PO_SCH_DT", model[0].dt1);
            return View(model);
        }
        [HttpPost]
        public ActionResult po_sch(List<Tmodelmain> model, string command, string hfrow, string hftable, HttpPostedFileBase upd1)
        {
            try
            {
                FillMst();
                DataTable dtm = sgen.settable(hftable);
                model[0].dt1 = dtm;
                var tm = model.FirstOrDefault();
                if (command == "Cancel") { return CancelFun(model); }
                else if (command == "Callback")
                {
                    model = StartCallback(model);
                }
                else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
                {
                    if (model[0].Col133 == "N")
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for save Dis Schedule, please contact your admin', 2);";
                        return View(model);
                    }
                    if (model[0].dt1.Rows[0]["icode"].ToString().Trim().Equals("-"))
                    {
                        ViewBag.scripCall += "showmsgJS(1,'Please Select item in grid-1', 2);";
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                        return View(model);
                    }
                    DataTable dtbtch;
                    dtbtch = model[0].dt1.Clone();
                    var t1 = model[0].dt1.AsEnumerable().GroupBy(k => new
                    {
                        icode = k.Field<string>("icode"),
                        iname = k.Field<string>("iname"),
                        partno = k.Field<string>("partno"),
                        uom = k.Field<string>("uom"),
                    });
                    var t2 = t1.Select(c =>
                    {
                        var row = dtbtch.NewRow();
                        row["icode"] = c.Key.icode;
                        row["iname"] = c.Key.iname;
                        row["partno"] = c.Key.partno;
                        row["uom"] = c.Key.uom;
                        row["Order_qty"] = c.Sum(k => sgen.Make_decimal(k.Field<string>("Order_qty")));
                        row["Plan_qty"] = c.Sum(k => sgen.Make_decimal(k.Field<string>("Plan_qty")));
                        return row;
                    });
                    dtbtch = t2.CopyToDataTable();
                    decimal QTY_Out = 0, QTY_REQ = 0;
                    QTY_Out = sgen.Make_decimal(dtbtch.Rows[0]["Order_qty"].ToString());
                    QTY_REQ = sgen.Make_decimal(dtbtch.Rows[0]["Plan_qty"].ToString());
                    if (QTY_Out > 0)
                    {
                        if (QTY_Out < QTY_REQ)
                        {
                            ViewBag.scripCall += "showmsgJS(1, 'Plan Quantity Should not be more Then Order Quantity ,Please Check', 2);";
                            ModelState.Clear();
                            ViewBag.vnew = "disabled='disabled'";
                            ViewBag.vedit = "disabled='disabled'";
                            ViewBag.vsave = "";
                            ViewBag.vsavenew = "";
                            return View(model);
                        }
                    }
                    DataTable dataTable = new DataTable();
                    bool Result = false;
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
                    dataTable = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    for (int i = 0; i < model[0].dt1.Rows.Count; i++)
                    {
                        DataRow dr = dataTable.NewRow();
                        dr["vch_num"] = vch_num; //sdl no
                        dr["type"] = model[0].Col12;
                        dr["vch_date"] = sgen.Make_date_S(model[0].Col22);
                        dr["col1"] = model[0].Col49;//party code
                        dr["col4"] = model[0].Col29; //po no
                        dr["col21"] = model[0].Col28; //remark
                        dr["col20"] = model[0].Col40; //hf so type
                        dr["date2"] = sgen.Make_date_S(model[0].Col24); //sd st date
                        dr["date3"] = sgen.Make_date_S(model[0].Col25); //sd end date
                        dr["date7"] = sgen.Make_date_S(model[0].Col31); //po dt
                        dr["col5"] = model[0].dt1.Rows[i]["Icode"].ToString();
                        dr["col27"] = model[0].dt1.Rows[i]["IName"].ToString();
                        dr["col7"] = model[0].dt1.Rows[i]["PartNo"].ToString();
                        dr["col8"] = model[0].dt1.Rows[i]["UOM"].ToString();
                        dr["col9"] = model[0].dt1.Rows[i]["Order_qty"].ToString().Trim();
                        dr["col10"] = model[0].dt1.Rows[i]["Plan_qty"].ToString();
                        if (model[0].dt1.Rows[i]["Sdl_start_dt"].ToString() != "")
                        {
                            dr["date8"] = sgen.Make_date_S(model[0].dt1.Rows[i]["Sdl_start_dt"].ToString());
                            dr["date9"] = sgen.Make_date_S(model[0].dt1.Rows[i]["Sdl_end_dt"].ToString());
                        }
                        dr["col13"] = model[0].dt1.Rows[i]["PO_NO"].ToString();
                        dr["col12"] = model[0].dt1.Rows[i]["Remark"].ToString();
                        dr["date6"] = sgen.Make_date_S(model[0].dt1.Rows[i]["PO_Date"].ToString());
                        dr = getsave(currdate, dr, model, isedit);
                        dataTable.Rows.Add(dr);
                    }
                    Result = sgen.Update_data_fast1(userCode, dataTable, model[0].Col10.Trim(), model[0].Col8, isedit);
                    if (Result == true)
                    {
                        if (command.Trim().ToUpper().Equals("SAVE") || command.Trim().ToUpper().Equals("UPDATE"))
                        {
                            ViewBag.vnew = "";
                            ViewBag.vedit = "";
                            ViewBag.vsave = "disabled='disabled'";
                            ViewBag.vsavenew = "disabled='disabled'";
                            ViewBag.vimport = "disabled='disabled";
                            ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                        }
                        else if (command.Trim().ToUpper().Equals("SAVE & NEW") || command.Trim().ToUpper().Equals("UPDATE & NEW"))
                        {
                            try
                            {
                                Make_query("po_sch", "Select PO Schedule Type", "NEW", "1", "", "", MyGuid);
                                ViewBag.scripCall = "callFoo('Select Dispatch Schedule Type');";
                                ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                            }
                            catch (Exception ex)
                            {
                                ViewBag.scripCall += "mytoast('error', 'toast-top-right', '" + ex.Message.ToString().Trim() + "');";
                            }
                        }
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col10 = tm.Col10,
                            Col53 = tm.Col53,
                            Col54 = tm.Col54,
                            Col11 = tm.Col11,
                            Col13 = "Save",
                            Col100 = "Save & New",
                            Col121 = "S",
                            Col122 = "<u>S</u>ave",
                            Col123 = "Save & Ne<u>w</u>",
                            Col14 = tm.Col14,
                            Col15 = tm.Col15,
                            Col12 = tm.Col12,
                            dt1 = (DataTable)sgen.GetSession(tm.Col15, "PO_SCH_DT")
                        });
                    }
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
                                        //if (c > dt.Columns.Count) break;                                                                 
                                    }
                                    i++;
                                    dt.Rows.Add(dr);
                                }
                            }
                        }
                        dt.Rows[0].Delete();
                        for (int k = 0; k < dt.Rows.Count; k++)
                        {
                            mq = sgen.seekval(userCode, "select icode from item where icode='" + dt.Rows[k]["icode"].ToString() + "' and type='IT' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'", "icode");
                            if (mq.Trim() == "0")
                            {
                                ViewBag.vnew = "disabled='disabled'";
                                ViewBag.vedit = "disabled='disabled'";
                                ViewBag.vsave = "";
                                ViewBag.vsavenew = "";
                                ViewBag.scripCall += "showmsgJS(1,'Icode: " + dt.Rows[k]["icode"].ToString() + " not found,please check.', 0);";
                                ModelState.Clear();
                                return View(model);
                            }
                            else
                            {
                                mq = sgen.seekval(userCode, "select icode from purchasesc where icode='" + dt.Rows[k]["icode"].ToString() + "' and pur_type = 'NPI' and substr(type,1,1)='4' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'", "icode");
                                if (mq.Trim() == "0")
                                {
                                    ViewBag.vnew = "disabled='disabled'";
                                    ViewBag.vedit = "disabled='disabled'";
                                    ViewBag.vsave = "";
                                    ViewBag.vsavenew = "";
                                    ViewBag.scripCall += "showmsgJS(1,'Icode: " + dt.Rows[k]["icode"].ToString() + " not found in Sales Order,please check.', 0);";
                                    ModelState.Clear();
                                    return View(model);
                                }
                            }
                        }
                        mq1 = "select '-' as item,'0' Sno,it.icode as Icode,it.iname as Iname,it.cpartno Partno,hsn.master_name as HSN,nvl(pc.bal_qty, '0') as Order_qty," +
                               "um.master_name as UOM,pc.po_no as po_num,to_char(pc.date1, '" + sgen.Getsqldateformat() + "') as po_date,to_char" +
                               "(pc.vch_date, '" + sgen.Getsqldateformat() + "') as so_date,pc.vch_num as so_no from item it inner join (select a.po_no, a.vch_num, a.icode, a.type, " +
                               "a.so_qty,a.vch_date, a.date1,nvl(sum(a.invoice_qty), '0') as used_qty,a.so_qty - nvl(sum(a.invoice_qty), '0') as bal_qty from " +
                               "(select pc.vch_num, pc.gothrchrg as po_no, pc.date1, pc.vch_date, pc.icode, pc.type, pc.qtyord as so_qty, iv.qtyout as invoice_qty from purchasesc pc " +
                               "left join itransaction iv on iv.sono = pc.vch_num and iv.potype = '51' and pc.icode=iv.icode and iv.client_unit_id = pc.client_unit_id " +
                               "where pc.pur_type = 'NPI' and substr(pc.type, 1, 1) = '4' and pc.client_unit_id = '" + unitid_mst + "' union all " +
                               "select pc.vch_num, pc.gothrchrg as po_no, pc.date1, pc.vch_date, pc.icode, pc.type, pc.qtyord as so_qty, dp.qtyout as dsp_qty from purchasesc pc " +
                               "left join itransaction dp on dp.VEHNO = pc.vch_num and dp.type = '60' and pc.icode=dp.icode and and dp.client_unit_id = pc.client_unit_id" +
                               " where pc.pur_type = 'NPI' and substr(pc.type, 1, 1) = '4' union all select pc.vch_num, pc.gothrchrg as po_no, pc.date1,pc.vch_date, pc.icode, " +
                               "pc.type, pc.qtyord as so_qty, ds.col10 as dsp_qty from purchasesc pc left join kc_tab ds on ds.col13 = pc.vch_num and pc.icode=ds.col5 and ds.col20 = 'DSC' and" +
                               " ds.client_unit_id = pc.client_unit_id where pc.pur_type = 'NPI' and substr(pc.type, 1, 1) = '4') a group" +
                               " by(a.vch_num, a.icode, a.type, a.so_qty, a.po_no, a.vch_date, a.date1)) pc on pc.icode = it.icode left join master_setting um on" +
                               " um.master_id = it.uom and um.type = 'UMM' and find_in_set(um.client_unit_id, it.client_unit_id)= 1 " +
                               "left join master_setting hsn on hsn.master_id = it.hsno and hsn.type = 'HSN' and " +
                               "find_in_set(hsn.client_unit_id, it.client_unit_id)= 1 " +
                               "where it.type = 'IT' and find_in_set(it.client_unit_id, '" + unitid_mst + "')=1  and " +
                               "pc.type='" + sgen.GetSession(MyGuid, "BDPOTYPE").ToString() + "' and pc.vch_num " +
                               "in ('" + sgen.GetSession(MyGuid, "BDPONO").ToString().Trim().TrimEnd(',').Replace(",", "','").Trim().ToString() + "') and" +
                               " it.sale='Y' " + where + " ";
                        DataTable dt2 = sgen.getdata(userCode, mq1);
                        if (dt2.Rows.Count > 0)
                        {
                            var results = from table1 in dt.AsEnumerable()
                                          join table2 in dt2.AsEnumerable() on table1["icode"] equals table2["icode"]
                                          select new
                                          {
                                              item = table2["item"],
                                              sno = table2["sno"],
                                              icode = table1["icode"],
                                              iname = table2["iname"],
                                              partno = table2["partno"],
                                              uom = table2["uom"],
                                              order_qty = table2["order_qty"],
                                              PLAN_QTY = table1["PLAN_QTY"],
                                              SDL_START_DT = table1["SDL_START_DT"],
                                              SDL_END_DT = table1["SDL_END_DT"],
                                              PO_No = table2["po_num"],
                                              PO_Date = table2["po_date"],
                                              SO_No = table2["so_no"],
                                              SO_Date = table2["so_date"],
                                              remark = table1["remark"]
                                          };
                            results.ToList();
                            DataTable dtk = sgen.ToDataTable(results.ToList());
                            if (dtk.Rows.Count > 0)
                            {
                                model[0].dt1 = dtk;
                                ViewBag.vimport = "disabled='disabled";
                                ViewBag.vnew = "disabled='disabled'";
                                ViewBag.vedit = "disabled='disabled'";
                                ViewBag.vsave = "";
                                ViewBag.vsavenew = "";
                            }
                            else
                            {
                                ViewBag.vnew = "disabled='disabled'";
                                ViewBag.vedit = "disabled='disabled'";
                                ViewBag.vsave = "";
                                ViewBag.vsavenew = "";
                                ViewBag.scripCall += "showmsgJS(1,'Data Not Found,Please Check', 0);";
                                ModelState.Clear();
                                return View(model);
                            }
                            ViewBag.vsave = "";
                            ViewBag.vimport = "";
                        }
                    }
                }
                else if (command == "-")
                {
                    if (model[0].dt1.Rows.Count > 1) model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                    else { model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "PO_SCH_DT"); }
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vsavenew = "";
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
        [HttpGet]
        public FileResult sdltemp(List<Tmodelmain> model)
        {
            FillMst();
            model = (List<Tmodelmain>)sgen.GetSession(MyGuid, "model");
            DataTable dtl = new DataTable();
            mq = "SELECT '' icode,'' Plan_qty,'' Sdl_start_dt,'' Sdl_end_dt,'' Remark from Dual";
            dtl = sgen.getdata(userCode, mq);
            string cg_com_name = sgen.GetCookie(MyGuid, "cg_com_name");
            Byte[] fileBytes = sgen.Exp_to_csv_new(dtl, "sdltemp", cg_com_name);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "sdltemp");
        }
        #endregion
        #region pur_rpt
        public ActionResult pur_rpt()
        {
            FillMst();
            // shiv
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col10 = "itransaction";
            model[0].Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            model[0].Col13 = "Save";
            model[0].Col100 = "Save & New";
            model[0].Col14 = mid_mst.Trim();
            model[0].Col15 = MyGuid.Trim();
            return View(model);
        }
        [HttpPost]
        public ActionResult pur_rpt(List<Tmodelmain> model, string command)
        {
            try
            {
                string fdt = "", tdt = "", cl = "", ut = "";
                FillMst(model[0].Col15);
                if (sgen.GetSession(MyGuid, "KDDL_CL") != null) cl = sgen.GetSession(MyGuid, "KDDL_CL").ToString();
                if (sgen.GetSession(MyGuid, "KDDL_UT") != null) ut = sgen.GetSession(MyGuid, "KDDL_UT").ToString().Replace(",", "','");
                if (sgen.GetSession(MyGuid, "KPDFDT") != null) fdt = sgen.GetSession(MyGuid, "KPDFDT").ToString();
                if (sgen.GetSession(MyGuid, "KPDTDT") != null) tdt = sgen.GetSession(MyGuid, "KPDTDT").ToString();
                if (sgen.GetSession(MyGuid, "KPDCMD") != null) command = sgen.GetSession(MyGuid, "KPDCMD").ToString();
                var tm = model.FirstOrDefault();
                DataTable dt = new DataTable();
                string tcp = "", tut = "", tfromdt = "", ttodate = "";
                try
                {
                    tcp = sgen.GetSession(MyGuid, "KDDL_CL").ToString();
                    tut = sgen.GetSession(MyGuid, "KDDL_UT").ToString();
                    tfromdt = sgen.GetSession(MyGuid, "TXTFROMDATE").ToString();
                    ttodate = sgen.GetSession(MyGuid, "TXTTODATE").ToString();
                }
                catch (Exception err) { }
                string date_period = "from " + fdt + " to " + tdt + "";
                string title = "";
                switch (command.Trim())
                {
                    case "Pending Indent":
                        cmd = "select distinct (it.icode||to_char(it.vch_date,'yyyymmdd')||it.type||pc.ind_no||to_char(pc.vch_date, 'yyyymmdd')) fstr" +
                            ",pc.ind_no,to_char(pc.vch_date, '" + sgen.Getsqldateformat() + "') as ind_date,pc.icode as Icode,it.iname as Iname,it.cpartno Partno, um.master_name as UOM," +
                            "hsn.master_name as hsn,hsn.group_name taxrate, pc.type,pc.ind_qty,pc.Bal_qty from item it inner join(select a.ind_no, a.vch_date, a.type," +
                            "a.icode, a.client_id, a.client_unit_id, a.ind_qty, sum(a.po_qty) as used,(a.ind_qty - nvl(sum(a.po_qty), '0')) as Bal_qty " +
                            "from(select nd.vch_num as ind_no, nd.vch_date, nd.type, nd.icode, nd.client_id,nd.client_unit_id, nd.qtyord as ind_qty, nvl(b.qtyord, '0') as po_qty" +
                            " from purchases nd left join purchases b on b.icode = nd.icode and b.indno = nd.vch_num and b.type not in ('66') and b.pur_type = '001' and " +
                            "nd.client_unit_id = b.client_unit_id where nd.client_unit_id = '" + unitid_mst + "' and nd.type = '66' union all select nd.vch_num as ind_no, nd.vch_date, nd.type, nd.icode, nd.client_id, nd.client_unit_id," +
                            " nd.qtyord as ind_qty, nvl(b.col11, '0') as close_qty from purchases nd left join enx_tab b on b.col7 = nd.icode and b.col12 = nd.vch_num " +
                            "and b.type = 'CPI' and nd.client_unit_id = b.client_unit_id where " +
                            "nd.client_unit_id = '" + unitid_mst + "' and nd.type = '66') a " +
                            "group by(a.ind_no, a.vch_date, a.type, a.icode, a.client_id, a.client_unit_id, a.ind_qty)) pc on pc.icode = it.icode and " +
                            "find_in_set(pc.client_unit_id,it.client_unit_id)=1 " +
                            "and to_date(to_char(pc.vch_date,'DDMMYYYY'),'DDMMYYYY') between to_date('" + fdt + "','" + sgen.Getsqldateformat() + "') " +
                              "and to_date('" + tdt + "','" + sgen.Getsqldateformat() + "')  inner join master_setting um on um.master_id = " +
                              "it.uom and um.type = 'UMM' and" +
                              " find_in_set(um.client_unit_id,'" + unitid_mst + "')= 1 inner join master_setting hsn on " +
                              "hsn.master_id = it.hsno and hsn.type = 'HSN' and find_in_set(hsn.client_unit_id, '" + unitid_mst + "') = 1 where find_in_set(it.client_unit_id,'" + unitid_mst + "')=1 and it.type = 'IT'";
                        title = "Pending Indent" + date_period + "";
                        break;
                    case "PO Summary Item Wise":
                        cmd = "select p.icode as fstr, p.icode as item_code,it.iname as item_name,sum(p.igst) as IGST,sum(p.sgst) as SGST,sum(p.cgst) as CGST" +
                              ",sum(p.basicamt) as basic_amt,sum(p.netamt) as net_amount from purchases p inner join item it on p.icode = it.icode" +
                              " and find_in_set(p.client_unit_id, it.client_unit_id)= 1 and it.type = 'IT' " +
                              "where p.client_unit_id = '" + unitid_mst + "' and p.type in ('50', '52')" +
                              " and to_date(to_char(p.vch_date,'DDMMYYYY'),'DDMMYYYY') between to_date('" + fdt + "','" + sgen.Getsqldateformat() + "') " +
                              "and to_date('" + tdt + "','" + sgen.Getsqldateformat() + "') group by p.icode,it.iname";
                        title = "PO Summary Item Wise" + date_period + "";
                        sgen.SetSession(MyGuid, "PURRPTBTN", "POITSUM");
                        break;
                    case "PO Summary Party Wise":
                        cmd = "select p.acode as fstr, p.acode as party_code,c.C_name as Party_name,c.c_gstin as Gst_no,sum(p.igst) as IGST,sum(p.sgst) as SGST," +
                                  "sum(p.cgst) as CGST,sum(p.basicamt) as basic_amt,sum(p.netamt) as net_amount from purchases p inner join clients_mst c on" +
                                  " p.acode = c.vch_num and find_in_set(c.client_unit_id, p.client_unit_id)=1 and c.type = 'BCD' and " +
                                  "substr(c.vch_num,0,3)='203' where p.client_unit_id = '" + unitid_mst + "' and" +
                                  " p.type in ('50', '52') and to_date(to_char(p.vch_date,'DDMMYYYY'),'DDMMYYYY') between to_date('" + fdt + "','" + sgen.Getsqldateformat() + "') " +
                                  "and to_date('" + tdt + "','" + sgen.Getsqldateformat() + "') group by p.acode,c.C_name,c.c_gstin";
                        title = "PO Summary Party Wise" + date_period + "";
                        sgen.SetSession(MyGuid, "PURRPTBTN", "POPARTYSUM");
                        break;
                    case "Callback":
                        if (sgen.GetSession(MyGuid, "PURRPTBTN") != null) btnval = sgen.GetSession(MyGuid, "PURRPTBTN").ToString();
                        model = CallbackFun(btnval, "N", actionName, controllerName, model);
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                        break;
                    case "Pending PO":
                        cmd = "select (it.icode || pc.type || pc.po_no || to_char(it.vch_date, 'yyyymmdd') || it.type) as fstr,pc.acode as vcode,cl.c_name as vname,pc.po_no as po_no,to_char(pc.po_date, '" + sgen.Getsqldateformat() + "') as podate" +
                            ",it.icode as Icode,it.iname as Iname,it.cpartno Partno, um.master_name as UOM, hsn.master_name as HSN,pc.po_qty,pc.used_qty,nvl(pc.bal_qty, '0') as Balance_qty from item it " +
                            "inner join (select a.vch_num as po_no,a.vch_date as po_date,a.icode,a.type, a.po_qty,a.acode,a.client_unit_id,nvl(sum(a.mrn_qty), '0') as used_qty,a.po_qty - nvl(sum(a.mrn_qty), '0') as" +
                            " bal_qty from(select pc.vch_num, pc.vch_date, pc.icode, pc.type, pc.qtyord as po_qty, nvl(to_number(iv.qtyrcvd), '0') as mrn_qty,pc.acode, pc.client_unit_id from purchases pc " +
                            "left join itransaction iv on iv.pono = pc.vch_num and iv.type IN('02', '03') and pc.icode = iv.icode and iv.client_unit_id = pc.client_unit_id where substr(pc.type, 1, 1) = '5'" +
                            " and pc.client_unit_id = '" + unitid_mst + "' union all select pc.vch_num, pc.vch_date, pc.icode, pc.type, pc.qtyord as po_qty, nvl(to_number(ds.col10), '0') as sdl_qty," +
                            " pc.acode,pc.client_unit_id from purchases pc left join kc_tab ds on ds.col13 = pc.vch_num and pc.icode = ds.col5 and ds.col20 = 'BPS' and ds.client_unit_id = pc.client_unit_id" +
                            " where substr(pc.type, 1, 1) = '5' and pc.client_unit_id = '" + unitid_mst + "' union all select pc.vch_num, pc.vch_date, pc.icode, pc.type, pc.qtyord as po_qty," +
                            " nvl(to_number(iv.col11), '0') as mrn_qty, pc.acode, pc.client_unit_id from purchases pc left join enx_tab iv on iv.col12 = pc.vch_num and iv.col7 = pc.icode and " +
                            "iv.type = 'POR' and iv.client_unit_id = pc.client_unit_id where substr(pc.type, 1, 1) = '5' and pc.client_unit_id = '" + unitid_mst + "')" +
                            " a group by a.vch_num, a.icode, a.type,a.po_qty, a.vch_date,a.acode,a.client_unit_id) pc on pc.icode = it.icode inner join clients_mst cl on cl.vch_num = pc.acode and " +
                            "cl.type = 'BCD' and substr(cl.vch_num,0,3)= '203' and cl.client_unit_id = pc.client_unit_id left join master_setting um on um.master_id = it.uom and um.type = 'UMM' and" +
                            " find_in_set(um.client_unit_id, it.client_unit_id) = 1 left join master_setting hsn on hsn.master_id = it.hsno and hsn.type" +
                            " = 'HSN' and find_in_set(hsn.client_unit_id, it.client_unit_id)= 1 where it.type = 'IT' " +
                            " and find_in_set(it.client_unit_id,'" + unitid_mst + "')= 1 and to_date(to_char(pc.po_date,'DDMMYYYY'),'DDMMYYYY') between to_date('" + fdt + "','" + sgen.Getsqldateformat() + "') " +
                   "and to_date('" + tdt + "','" + sgen.Getsqldateformat() + "')";
                        //    cmd = "select (pono||podate||potype) fstr,pono,podate,potype,icode,iname,partycode,partyname,max(to_number(qtyord)) qtyord,sum(to_number(qtyin)) qtyin," +
                        //"(max(to_number(qtyord))-sum(to_number(qtyin))) qtybalance,sum(to_number(iamount)) iamount " +
                        //"from (select p.vch_num pono,to_char(p.vch_date,'" + sgen.Getsqldateformat() + "') podate,p.type potype," +
                        //"p.icode,it.iname,p.acode Partycode,cl.c_name Partyname,nvl(p.qtyord, 0) qtyord,nvl(i.qtyin, 0) qtyin,nvl(i.irate, 0) irate," +
                        //"nvl(i.iamount, 0) iamount,i.vch_num mrnno,to_char(i.vch_date,'" + sgen.Getsqldateformat() + "') mrndate," +
                        //"i.type mrntype,i.invno,to_char(i.invdate,'" + sgen.Getsqldateformat() + "') invdate,i.chlno," +
                        //"to_char(i.chldate,'" + sgen.Getsqldateformat() + "') chldate,utol,ltol " +
                        //"from purchases p " +
                        //"inner join itransaction i on i.pono = p.vch_num and to_char(i.podate, 'ddmmyyyy') = to_char(p.vch_date, 'ddmmyyyy') and i.client_id = p.client_id " +
                        //"and i.client_unit_id = p.client_unit_id and i.acode = p.acode and i.store = 'Y' " +
                        //"inner join item it on it.icode = p.icode and it.type = 'IT' and it.client_id = p.client_id and find_in_set(p.client_unit_id,it.client_unit_id)=1 " +
                        //"inner join clients_mst cl on cl.vch_num = p.acode and cl.type = 'BCD' and substr(cl.vch_num,0,3)='203' and find_in_set(cl.client_id, p.client_id)=1 and find_in_set(cl.client_unit_id, p.client_unit_id)=1 " +
                        //"where p.client_id = '" + clientid_mst + "' and p.client_unit_id = '" + unitid_mst + "' and p.type in ('50', '51', '52', '53') and to_date(p.vch_date) between" +
                        //" to_date('" + fdt + "','" + sgen.Getsqldateformat() + "') and to_date('" + tdt + "','" + sgen.Getsqldateformat() + "')) " +
                        ////"group by pono,podate,potype,icode,iname,partycode,partyname having (max(to_number(qtyord))-sum(to_number(qtyin)))> 0";
                        //"group by pono,podate,potype,icode,iname,partycode,partyname,ltol having sum(to_number(qtyin))<(max(to_number(qtyord))-round((max(to_number(qtyord))*to_number(ltol))/100))";
                        title = "Pending Po Summary" + date_period + "";
                        break;
                    case "Purchase Order Detailed":
                        cmd = "select (p.client_id||p.client_unit_id||p.vch_num||to_char(p.vch_date,'yyyymmdd')||p.type) as fstr,p.vch_num PO_Number," +
                   "to_char(p.vch_date, '" + sgen.Getsqldateformat() + "') PO_Date,c.vch_num as party_code,c.c_name as Party_Name,c.c_gstin as Party_Gstin,c.addr Address," +
                   " p.icode ItemCode, p.iname ItemName, p.cpartno PartNo, p.uom UOM, p.hsno HSN, p.qtystk Stock_Qty," +
                   "p.qtyord Order_Qty, p.irate ItemRate, p.taxrate TaxRate, iamount Item_Amt,p.taxamt taxamt, p.discrate Discount_Rate" +
                   ",p.discamt Discount_Amt, p.basicamt BasicAmt, p.lineamount LineAmt, p.netamt,p.indno IndentNo,to_char(p.inddate, '" + sgen.Getsqldateformat() + "') Indent_Date, p.qtyind Indent_Qty, to_char(p.dlv_date, '" + sgen.Getsqldateformat() + "') Delivery_Date" +
                   " from purchases p inner join clients_mst c on p.acode = c.vch_num and find_in_set(c.client_unit_id, p.client_unit_id)=1 and c.type = 'BCD' and substr(c.vch_num,0,3)='203' left join clients_mst sf on p.shipfrom = sf.vch_num " +
                   "and find_in_set(sf.client_unit_id, p.client_unit_id)=1 and sf.type = 'BCD' and substr(sf.vch_num,0,3)='203' " +
                   "left join clients_mst st on p.shipto = st.vch_num and find_in_set(st.client_unit_id, p.client_unit_id)=1 and " +
                   "st.type = 'BCD' and substr(st.vch_num,0,3)='203' where p.client_unit_id = '" + unitid_mst + "'" +
                   " and p.type in ('50', '52') and to_date(to_char(p.vch_date,'DDMMYYYY'),'DDMMYYYY') between to_date('" + fdt + "','" + sgen.Getsqldateformat() + "') " +
                   "and to_date('" + tdt + "','" + sgen.Getsqldateformat() + "')";
                        title = "Purchase Order Detailed" + date_period + "";
                        break;
                    case "HSN Wise Sale Summary":
                        cmd = "select '-' fstr, hsn.master_name as hsn_code,hsn.content,iv.IGST,iv.SGST,iv.CGST,iv.basic_amt,iv.net_amount from item it inner join (select iv.icode as item_code, iv.vch_date, " +
                         "sum(iv.gserchrg) as IGST,sum(iv.gloadchrg) as SGST,sum(iv.gamc) as CGST,sum(iv.basicamt) as basic_amt,sum(iv.ginstlchrg) GrossAmt,sum(iv.netamt) as net_amount from itransaction iv where iv.client_id = '" + clientid_mst + "' " +
                         "and iv.client_unit_id = '" + unitid_mst + "' and substr(iv.type, 1, 1) = '5' group by(iv.icode, iv.vch_date)) iv on iv.item_code = it.icode inner join master_setting" +
                         " hsn on hsn.master_id = it.hsno and hsn.type = 'HSN' and find_in_set(hsn.client_unit_id," +
                         " it.client_unit_id)= 1 where find_in_set(it.client_unit_id, '" + unitid_mst + "')=1 and it.type = 'IT' and substr(it.icode,1,1)= '9' and to_date(iv.vch_date) between to_date('" + fdt + "','" + sgen.Getsqldateformat() + "') and to_date('" + tdt + "','" + sgen.Getsqldateformat() + "')";
                        title = "HSN Wise Sale Summary" + date_period + "";
                        break;
                }
                if (command == "PO Summary Item Wise" || command == "PO Summary Party Wise")
                {
                    sgen.open_grid(title, cmd, 1);
                    ViewBag.scripCall = "callFoo('" + title + "');";
                    sgen.SetSession(MyGuid, "KPDCMD", null);
                }
                else if (command != "Callback" && command != "Auto Indents" && command != "Slow and Non Moving" && command != "PO Summary Item Wise" && command != "PO Summary Item Wise")
                {
                    sgen.open_grid(title, cmd, 0);
                    ViewBag.scripCall = "callFoo('" + title + "');";
                    sgen.SetSession(MyGuid, "KPDCMD", null);
                }
                else if (command == "Auto Indents")
                {
                    mq = @"select i.client_unit_id,i.icode,i.uom,i.cpartno, i.iname,max(reorder) as reorder,max(i.minstk) minstk,sum(nvl(s.qtyin,0)) as qtyin,sum(nvl(s.qtyout,0)) qtyout,sum(nvl(s.closing,0)) as closing  
from item i left outer join  (
select  trim(t.client_unit_id) as client_unit_id,trim(t.icode) as icode,sum(replace(nvl(t.qtyout,0),'NaN',0)) qtyout,sum(replace(nvl(t.qtyin,0),'NaN',0)) as qtyin
,sum(replace(nvl(t.qtyin,0),'NaN',0))-sum(replace(nvl(t.qtyout,0),'NaN',0)) as closing from  itransaction t where trim(t.store) not in ('R','W')  group by t.icode,t.client_unit_id
union all  
 select trim(p.client_unit_id) client_unit_id, trim(p.icode) as icode,sum(p.qtyord) as qtyord ,sum(replace(nvl(t.qtyin,0),'NaN',0)) as qtyin,
 sum(p.qtyord)-sum(replace(nvl(t.qtyin,0),'NaN',0)) as closing  from purchases p left  join itransaction t on p.client_unit_id=t.client_unit_id and p.icode=t.icode
 and p.vch_num=t.pono and to_char(p.vch_date,'DDMMYYYY')=TO_CHAR(t.podate,'DDMMYYYY') AND p.type=t.potype  and trim(t.store) not in ('R','W') group by p.icode,p.client_unit_id
  union all
 select trim(p.client_unit_id) client_unit_id, trim(p.icode) as icode,sum(p.qtyord) as qtyord ,sum(replace(nvl(t.qtyord,0),'NaN',0)) as 
 qtyin,sum(p.qtyord)-sum(replace(nvl(t.qtyord,0),'NaN',0)) as closing  from purchases p left  join purchases t on p.client_unit_id=t.client_unit_id and p.icode=t.icode
 and p.vch_num=t.indno and to_char(p.vch_date,'DDMMYYYY')=TO_CHAR(t.inddate,'DDMMYYYY') where p.type='66' and t.type<>'66' group by p.icode,p.client_unit_id
) s on  s.icode=i.icode and s.client_unit_id=i.client_unit_id   where length(trim(i.icode))>4 and i.client_unit_id='" + unitid_mst + "' " +
    "group by i.client_unit_id,i.icode, i.iname,i.uom,i.cpartno having max(i.minstk)>sum(nvl(s.closing,0))";
                    DataTable dt1 = new DataTable();
                    dt1 = sgen.getdata(userCode, mq);
                    if (dt1.Rows.Count > 0)
                    {
                        string currdate = sgen.server_datetime(userCode);
                        {
                            isedit = false;
                            mq = "select max(to_number(vch_num)) as auto_genid from purchases a where type='66' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
                            vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                        }
                        DataTable dataTable = cmd_Fun.GetStructure(userCode, "purchases");
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            DataRow dr = dataTable.NewRow();
                            dr["vch_num"] = vch_num;
                            dr["type"] = "66";
                            dr["vch_date"] = currdate;
                            dr["deptno"] = "001";
                            dr["deptname"] = "001";
                            dr["desig"] = "001";
                            dr["reqby"] = userid_mst;
                            dr["totremark"] = "Minimum Level Requisitions";
                            dr["icode"] = dt1.Rows[i]["Icode"].ToString();
                            dr["iname"] = dt1.Rows[i]["IName"].ToString();
                            dr["cpartno"] = dt1.Rows[i]["cpartno"].ToString();
                            dr["hsno"] = "-";
                            dr["uom"] = dt1.Rows[i]["UOM"].ToString();
                            dr["qtystk"] = dt1.Rows[i]["closing"].ToString();
                            dr["qtyord"] = dt1.Rows[i]["reorder"].ToString();
                            dr["iamount"] = "0";
                            dr["iremark"] = "-";
                            dr["rec_id"] = "0";
                            // shiv
                            if (isedit)
                            {
                                dr["client_id"] = clientid_mst;
                                dr["client_unit_id"] = unitid_mst;
                                dr["ent_by"] = userid_mst;
                                dr["ent_date"] = currdate;
                                dr["edit_by"] = userid_mst;
                                dr["edit_date"] = currdate;
                            }
                            else
                            {
                                dr["client_id"] = clientid_mst;
                                dr["client_unit_id"] = unitid_mst;
                                dr["ent_by"] = userid_mst;
                                dr["ent_date"] = currdate;
                                dr["edit_by"] = "-";
                                dr["edit_date"] = currdate;
                            }
                            dataTable.Rows.Add(dr);
                        }
                        bool Result = sgen.Update_data(userCode, dataTable, "purchases", "-", false);
                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        ViewBag.vsavenew = "disabled='disabled'";
                        if (Result == true)
                        {
                            ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                        }
                    }
                }
                else if (command == "Slow and Non Moving")
                {
                    mq = "";
                    dt = sgen.getdata(userCode, mq);
                    sgen.open_report_bydt_xml(userCode, dt, "slow_moving", "Report");
                    ViewBag.scripCall += "showRptnew('Slow and Non Moving';";

                    //ViewBag.scripCall = "OpenSingle('../../../../../erp/schoolReports_Rpts/Filter_Report_xml.aspx','95%','900px','Reports');";
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
        #region pur_rpt2
        public ActionResult pur_rpt2()
        {
            FillMst();
            // shiv
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "PURCHASE REPORT";
            model[0].Col10 = "itransaction";
            model[0].Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            model[0].Col13 = "Save";
            model[0].Col100 = "Save & New";
            model[0].Col14 = mid_mst.Trim();
            model[0].Col15 = MyGuid.Trim();
            return View(model);
        }
        [HttpPost]
        public ActionResult pur_rpt2(List<Tmodelmain> model, string command)
        {
            try
            {
                string fdt = "", tdt = "", title = "", btnval = "", cl = "", ut = "";
                int seektype = 0;
                FillMst(model[0].Col15);
                var tm = model.FirstOrDefault();
                DataTable dt = new DataTable();
                if (sgen.GetSession(MyGuid, "KDDL_CL") != null) cl = sgen.GetSession(MyGuid, "KDDL_CL").ToString();
                if (sgen.GetSession(MyGuid, "KDDL_UT") != null) ut = sgen.GetSession(MyGuid, "KDDL_UT").ToString().Replace(",", "','");
                if (sgen.GetSession(MyGuid, "KPDFDT") != null) fdt = sgen.GetSession(MyGuid, "KPDFDT").ToString();
                if (sgen.GetSession(MyGuid, "KPDTDT") != null) tdt = sgen.GetSession(MyGuid, "KPDTDT").ToString();
                if (sgen.GetSession(MyGuid, "KPDCMD") != null) command = sgen.GetSession(MyGuid, "KPDCMD").ToString();
                string tcp = "", tut = "", tfromdt = "", ttodate = "";
                try
                {
                    tcp = sgen.GetSession(MyGuid, "KDDL_CL").ToString();
                    tut = sgen.GetSession(MyGuid, "KDDL_UT").ToString();
                    tfromdt = sgen.GetSession(MyGuid, "TXTFROMDATE").ToString();
                    ttodate = sgen.GetSession(MyGuid, "TXTTODATE").ToString();
                }
                catch (Exception err)
                {
                }
                string date_period = " from " + fdt + " to " + tdt + "";
                switch (command.Trim())
                {
                    case "Pending Indent":
                        cmd = "select distinct (it.icode||to_char(it.vch_date,'yyyymmdd')||it.type||pc.ind_no||to_char(pc.vch_date, 'yyyymmdd')) fstr" +
                            ",pc.ind_no,to_char(pc.vch_date, '" + sgen.Getsqldateformat() + "') as ind_date,pc.icode as Icode,it.iname as Iname,it.cpartno Partno, um.master_name as UOM," +
                            "hsn.master_name as hsn,hsn.group_name taxrate, pc.type,pc.ind_qty,pc.Bal_qty from item it inner join(select a.ind_no, a.vch_date, a.type," +
                            "a.icode, a.client_id, a.client_unit_id, a.ind_qty, sum(a.po_qty) as used,(a.ind_qty - nvl(sum(a.po_qty), '0')) as Bal_qty " +
                            "from(select nd.vch_num as ind_no, nd.vch_date, nd.type, nd.icode, nd.client_id,nd.client_unit_id, nd.qtyord as ind_qty, nvl(b.qtyord, '0') as po_qty" +
                            " from purchases nd left join purchases b on b.icode = nd.icode and b.indno = nd.vch_num and b.type not in ('66') and b.pur_type = '001' and " +
                            "nd.client_unit_id = b.client_unit_id where nd.client_unit_id = '" + ut + "' and nd.type = '66' union all select nd.vch_num as ind_no, nd.vch_date, nd.type, nd.icode, nd.client_id, nd.client_unit_id," +
                            " nd.qtyord as ind_qty, nvl(b.col11, '0') as close_qty from purchases nd left join enx_tab b on b.col7 = nd.icode and b.col12 = nd.vch_num " +
                            "and b.type = 'CPI' and nd.client_unit_id = b.client_unit_id where " +
                            "nd.client_unit_id = '" + ut + "' and nd.type = '66') a " +
                            "group by(a.ind_no, a.vch_date, a.type, a.icode, a.client_id, a.client_unit_id, a.ind_qty)) pc on pc.icode = it.icode and " +
                            "find_in_set(pc.client_unit_id,it.client_unit_id)=1 " +
                            "and to_date(to_char(pc.vch_date,'DDMMYYYY'),'DDMMYYYY') between to_date('" + fdt + "','" + sgen.Getsqldateformat() + "') " +
                              "and to_date('" + tdt + "','" + sgen.Getsqldateformat() + "')  inner join master_setting um on um.master_id = " +
                              "it.uom and um.type = 'UMM' and" +
                              " find_in_set(um.client_unit_id,'" + ut + "')= 1 inner join master_setting hsn on " +
                              "hsn.master_id = it.hsno and hsn.type = 'HSN' and find_in_set(hsn.client_unit_id, '" + ut + "') = 1 where find_in_set(it.client_unit_id,'" + ut + "')=1 and it.type = 'IT'";
                        title = "Pending Indent" + date_period + "";
                        break;
                    case "PO Summary Item Wise":
                        cmd = "select p.icode as fstr, p.icode as item_code,it.iname as item_name,sum(p.igst) as IGST,sum(p.sgst) as SGST,sum(p.cgst) as CGST" +
                              ",sum(p.basicamt) as basic_amt,sum(p.netamt) as net_amount from purchases p inner join item it on p.icode = it.icode" +
                              " and find_in_set(p.client_id, it.client_id) = 1 and find_in_set(p.client_unit_id, it.client_unit_id)= 1 and it.type = 'IT' " +
                              "where p.client_unit_id = '" + ut + "' and p.type in ('50', '52')" +
                              " and to_date(to_char(p.vch_date,'DDMMYYYY'),'DDMMYYYY') between to_date('" + fdt + "','" + sgen.Getsqldateformat() + "') " +
                              "and to_date('" + tdt + "','" + sgen.Getsqldateformat() + "') group by p.icode,it.iname";
                        title = "PO Summary Item Wise" + date_period + "";
                        sgen.SetSession(MyGuid, "PURRPTBTN", "POITSUM");
                        break;
                    case "PO Summary Party Wise":
                        //cmd = "select p.acode as fstr, p.acode as party_code,c.C_name as Party_name,c.c_gstin as Gst_no,sum(p.igst) as IGST,sum(p.sgst) as SGST," +
                        //          "sum(p.cgst) as CGST,sum(p.basicamt) as basic_amt,sum(p.netamt) as net_amount from purchases p inner join clients_mst c on" +
                        //          " p.acode = c.vch_num and find_in_set(c.client_unit_id, p.client_unit_id)=1 and c.type = 'BCD' and " +
                        //          "substr(c.vch_num,0,3)='203' where p.client_unit_id = '" + unitid_mst + "' and" +
                        //          " p.type in ('50', '52') and to_date(to_char(p.vch_date,'DDMMYYYY'),'DDMMYYYY') between to_date('" + fdt + "','dd/mm/yyyy') " +
                        //          "and to_date('" + tdt + "','dd/mm/yyyy') group by p.acode,c.C_name,c.c_gstin";

                        cmd = "select p.acode fstr,p.acode party_code,c.c_name Party_name,c.c_gstin Gst_no,p.igst IGST,p.sgst SGST,p.cgst CGST," +
                            "p.basicamt basic_amt, p.netamt net_amount from purchases p " +
                            "inner join clients_mst c on p.acode = c.vch_num and find_in_set(c.client_unit_id, p.client_unit_id) = 1 and c.type = 'BCD' " +
                            "and substr(c.vch_num,0,3)= '203' where p.client_unit_id = '" + ut + "' and p.type in ('50', '52') and " +
                            "to_date(to_char(p.vch_date,'DDMMYYYY'),'DDMMYYYY') between to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') and to_date('" + tdt + "','" + sgen.Getsqldateformat() + "') " +
                            "group by p.acode,p.acode,c.c_name,c.c_gstin,p.igst,p.sgst,p.cgst,p.basicamt,p.netamt";
                        title = "PO Summary Party Wise" + date_period + "";
                        sgen.SetSession(MyGuid, "PURRPTBTN", "POPARTYSUM");
                        break;
                    case "Callback":
                        if (sgen.GetSession(MyGuid, "PURRPTBTN") != null) btnval = sgen.GetSession(MyGuid, "PURRPTBTN").ToString();
                        model = CallbackFun(btnval, "N", actionName, controllerName, model);
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                        break;
                    case "Pending PO":
                        cmd = "select (it.icode || pc.type || pc.po_no || to_char(it.vch_date, 'yyyymmdd') || it.type) as fstr,pc.po_no as po_num,to_char(pc.po_date, '" + sgen.Getsqldateformat() + "') as podate,pc.acode as vcode,cl.c_name as vname" +
                            ",it.icode as Icode,it.iname as Iname,it.cpartno Partno, um.master_name as UOM, hsn.master_name as HSN,pc.po_qty,pc.used_qty,nvl(pc.bal_qty, '0') as Balance_qty from item it " +
                            "inner join (select a.vch_num as po_no,a.vch_date as po_date,a.icode,a.type, a.po_qty,a.acode,a.client_unit_id,nvl(sum(a.mrn_qty), '0') as used_qty,a.po_qty - nvl(sum(a.mrn_qty), '0') as" +
                            " bal_qty from(select pc.vch_num, pc.vch_date, pc.icode, pc.type, pc.qtyord as po_qty, nvl(to_number(iv.qtyrcvd), '0') as mrn_qty,pc.acode, pc.client_unit_id from purchases pc " +
                            "left join itransaction iv on iv.pono = pc.vch_num and iv.type IN('02', '03') and pc.icode = iv.icode and iv.client_unit_id = pc.client_unit_id where substr(pc.type, 1, 1) = '5'" +
                            " and pc.client_unit_id = '" + ut + "' union all select pc.vch_num, pc.vch_date, pc.icode, pc.type, pc.qtyord as po_qty, nvl(to_number(ds.col10), '0') as sdl_qty," +
                            " pc.acode,pc.client_unit_id from purchases pc left join kc_tab ds on ds.col13 = pc.vch_num and pc.icode = ds.col5 and ds.col20 = 'BPS' and ds.client_unit_id = pc.client_unit_id" +
                            " where substr(pc.type, 1, 1) = '5' and pc.client_unit_id = '" + ut + "' union all select pc.vch_num, pc.vch_date, pc.icode, pc.type, pc.qtyord as po_qty," +
                            " nvl(to_number(iv.col11), '0') as mrn_qty, pc.acode, pc.client_unit_id from purchases pc left join enx_tab iv on iv.col12 = pc.vch_num and iv.col7 = pc.icode and " +
                            "iv.type = 'POR' and iv.client_unit_id = pc.client_unit_id where substr(pc.type, 1, 1) = '5' and pc.client_unit_id = '" + ut + "')" +
                            " a group by a.vch_num, a.icode, a.type,a.po_qty, a.vch_date,a.acode,a.client_unit_id) pc on pc.icode = it.icode inner join clients_mst cl on cl.vch_num = pc.acode and " +
                            "cl.type = 'BCD' and substr(cl.vch_num,0,3)= '203' and cl.client_unit_id = pc.client_unit_id left join master_setting um on um.master_id = it.uom and um.type = 'UMM' and" +
                            " find_in_set(um.client_unit_id, it.client_unit_id) = 1 left join master_setting hsn on hsn.master_id = it.hsno and hsn.type" +
                            " = 'HSN' and find_in_set(hsn.client_unit_id, it.client_unit_id)= 1 where it.type = 'IT' " +
                            " and find_in_set(it.client_unit_id,'" + ut + "')= 1 and to_date(to_char(pc.po_date,'DDMMYYYY'),'DDMMYYYY') between to_date('" + fdt + "','" + sgen.Getsqldateformat() + "') " +
                   "and to_date('" + tdt + "','" + sgen.Getsqldateformat() + "')";
                        //    cmd = "select (pono||podate||potype) fstr,pono,podate,potype,icode,iname,partycode,partyname,max(to_number(qtyord)) qtyord,sum(to_number(qtyin)) qtyin," +
                        //"(max(to_number(qtyord))-sum(to_number(qtyin))) qtybalance,sum(to_number(iamount)) iamount " +
                        //"from (select p.vch_num pono,to_char(p.vch_date,'" + sgen.Getsqldateformat() + "') podate,p.type potype," +
                        //"p.icode,it.iname,p.acode Partycode,cl.c_name Partyname,nvl(p.qtyord, 0) qtyord,nvl(i.qtyin, 0) qtyin,nvl(i.irate, 0) irate," +
                        //"nvl(i.iamount, 0) iamount,i.vch_num mrnno,to_char(i.vch_date,'" + sgen.Getsqldateformat() + "') mrndate," +
                        //"i.type mrntype,i.invno,to_char(i.invdate,'" + sgen.Getsqldateformat() + "') invdate,i.chlno," +
                        //"to_char(i.chldate,'" + sgen.Getsqldateformat() + "') chldate,utol,ltol " +
                        //"from purchases p " +
                        //"inner join itransaction i on i.pono = p.vch_num and to_char(i.podate, 'ddmmyyyy') = to_char(p.vch_date, 'ddmmyyyy') and i.client_id = p.client_id " +
                        //"and i.client_unit_id = p.client_unit_id and i.acode = p.acode and i.store = 'Y' " +
                        //"inner join item it on it.icode = p.icode and it.type = 'IT' and it.client_id = p.client_id and find_in_set(p.client_unit_id,it.client_unit_id)=1 " +
                        //"inner join clients_mst cl on cl.vch_num = p.acode and cl.type = 'BCD' and substr(cl.vch_num,0,3)='203' and find_in_set(cl.client_id, p.client_id)=1 and find_in_set(cl.client_unit_id, p.client_unit_id)=1 " +
                        //"where p.client_id = '" + clientid_mst + "' and p.client_unit_id = '" + unitid_mst + "' and p.type in ('50', '51', '52', '53') and to_date(p.vch_date) between" +
                        //" to_date('" + fdt + "','" + sgen.Getsqldateformat() + "') and to_date('" + tdt + "','" + sgen.Getsqldateformat() + "')) " +
                        ////"group by pono,podate,potype,icode,iname,partycode,partyname having (max(to_number(qtyord))-sum(to_number(qtyin)))> 0";
                        //"group by pono,podate,potype,icode,iname,partycode,partyname,ltol having sum(to_number(qtyin))<(max(to_number(qtyord))-round((max(to_number(qtyord))*to_number(ltol))/100))";
                        title = "Pending Po Summary" + date_period + "";
                        break;
                    case "Purchase Order Detailed":
                        cmd = "select (p.client_id||p.client_unit_id||p.vch_num||to_char(p.vch_date,'yyyymmdd')||p.type) as fstr,p.vch_num PO_Number," +
                   "to_char(p.vch_date, 'dd/MM/YYYY') PO_Date,c.vch_num as party_code,c.c_name as Party_Name,c.c_gstin as Party_Gstin,c.addr Address," +
                   " p.icode ItemCode, p.iname ItemName, p.cpartno PartNo, p.uom UOM, p.hsno HSN, p.qtystk Stock_Qty," +
                   "p.qtyord Order_Qty, p.irate ItemRate, p.taxrate TaxRate, iamount Item_Amt,p.taxamt taxamt, p.discrate Discount_Rate" +
                   ",p.discamt Discount_Amt, p.basicamt BasicAmt, p.lineamount LineAmt, p.netamt,p.indno IndentNo,to_char(p.inddate, 'dd/MM/YYYY') Indent_Date, p.qtyind Indent_Qty, to_char(p.dlv_date, 'dd/MM/YYYY') Delivery_Date" +
                   " from purchases p inner join clients_mst c on p.acode = c.vch_num and find_in_set(c.client_unit_id, p.client_unit_id)=1 and c.type = 'BCD' and substr(c.vch_num,0,3)='203' left join clients_mst sf on p.shipfrom = sf.vch_num " +
                   "and find_in_set(sf.client_unit_id, p.client_unit_id)=1 and sf.type = 'BCD' and substr(sf.vch_num,0,3)='203' " +
                   "left join clients_mst st on p.shipto = st.vch_num and find_in_set(st.client_unit_id, p.client_unit_id)=1 and " +
                   "st.type = 'BCD' and substr(st.vch_num,0,3)='203' where p.client_unit_id = '" + ut + "'" +
                   " and p.type in ('50', '52') and to_date(to_char(p.vch_date,'DDMMYYYY'),'DDMMYYYY') between to_date('" + fdt + "','dd/mm/yyyy') " +
                   "and to_date('" + tdt + "','dd/mm/yyyy')";
                        title = "Purchase Order Detailed" + date_period + "";
                        break;
                    case "HSN Wise Sale Summary":
                        cmd = "select '-' fstr, hsn.master_name as hsn_code,hsn.content,iv.IGST,iv.SGST,iv.CGST,iv.basic_amt,iv.net_amount from item it inner join (select iv.icode as item_code, iv.vch_date, " +
                         "sum(iv.gserchrg) as IGST,sum(iv.gloadchrg) as SGST,sum(iv.gamc) as CGST,sum(iv.basicamt) as basic_amt,sum(iv.ginstlchrg) GrossAmt,sum(iv.netamt) as net_amount from itransaction iv where iv.client_id = '" + cl + "' " +
                         "and iv.client_unit_id = '" + ut + "' and substr(iv.type, 1, 1) = '5' group by(iv.icode, iv.vch_date)) iv on iv.item_code = it.icode inner join master_setting" +
                         " hsn on hsn.master_id = it.hsno and hsn.type = 'HSN' and find_in_set(hsn.client_unit_id," +
                         " it.client_unit_id)= 1 where find_in_set(it.client_unit_id, '" + ut + "')=1 and it.type = 'IT' and substr(it.icode,1,1)= '9' and to_date(iv.vch_date) between to_date('" + fdt + "','dd/mm/yyyy') and to_date('" + tdt + "','dd/mm/yyyy')";
                        title = "HSN Wise Sale Summary" + date_period + "";
                        break;
                }
                if (command == "PO Summary Item Wise" || command == "PO Summary Party Wise")
                {
                    sgen.open_grid(title, cmd, 1);
                    ViewBag.scripCall = "callFoo('" + title + "');";
                    sgen.SetSession(MyGuid, "KPDCMD", null);
                }
                else if (command != "Callback" && command != "Auto Indents" && command != "Slow and Non Moving" && command != "PO Summary Item Wise" && command != "PO Summary Item Wise")
                {
                    sgen.open_grid(title, cmd, 0);
                    ViewBag.scripCall = "callFoo('" + title + "');";
                    sgen.SetSession(MyGuid, "KPDCMD", null);
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
        #region cust_enquiry
        //public ActionResult enq()
        //{
        //    FillMst();
        //    chkRef();
        //    ViewBag.vnew = "";
        //    ViewBag.vedit = "";
        //    ViewBag.vsave = "disabled='disabled'";
        //    ViewBag.vsavenew = "disabled='disabled'";
        //    ViewBag.scripCall = "disableForm();";
        //    List<Tmodelmain> model = new List<Tmodelmain>();
        //    List<SelectListItem> mod1 = new List<SelectListItem>();
        //    Tmodelmain tm1 = new Tmodelmain();
        //    mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
        //    MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
        //    tm1.Col9 = "ENQUIRY FORM";
        //    tm1.Col12 = "ENQ"; //          
        //    tm1.Col10 = "clients_mst";
        //    tm1.Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
        //    tm1.Col13 = "Save";
        //    tm1.Col100 = "Save & New";
        //    tm1.Col14 = mid_mst.Trim();
        //    tm1.Col15 = MyGuid.Trim();
        //    tm1.Col39 = "Choose File";
        //    tm1.Col40 = "Choose File";
        //    tm1.Col41 = "Choose File";
        //    tm1.Col42 = "Choose File";
        //    tm1.Col43 = "Choose File";
        //    sgen.SetSession(MyGuid, "ENQ_TYPEMST", tm1.Col12);
        //    tm1.TList1 = mod1;
        //    tm1.SelectedItems1 = new string[] { "" };
        //    tm1.TList2 = mod1;
        //    tm1.SelectedItems2 = new string[] { "" };
        //    tm1.LTM1 = new List<Tmodelmain>();
        //    tm1.LTM2 = new List<Tmodelmain>();
        //    Tmodelmain tmltm2 = new Tmodelmain();
        //    tmltm2.Col1 = "1";
        //    tm1.LTM2.Add(tmltm2);
        //    model.Add(tm1);
        //    return View(model);
        //}
        //public List<Tmodelmain> newenq(List<Tmodelmain> model)
        //{
        //    model = getnew(model);
        //    model[0].Col13 = "Save";
        //    try
        //    {
        //        model[0].Col44 = "Active";
        //        List<SelectListItem> mod1 = new List<SelectListItem>();
        //        List<SelectListItem> mod2 = new List<SelectListItem>();
        //        #region party type
        //        model[0].TList1 = mod1;
        //        model[0].SelectedItems1 = new string[] { "" };
        //        TempData[MyGuid + "_Tlist1"] = mod1;
        //        #endregion
        //        #region  new action master
        //        mod2 = cmd_Fun.nextact(userCode, unitid_mst);
        //        TempData[MyGuid + "_TList2"] = mod2;
        //        #endregion
        //        model[0].TList1 = mod1;
        //        model[0].TList2 = mod2;
        //    }
        //    catch (Exception ex) { }
        //    return model;
        //}
        //[HttpPost]
        //public ActionResult enq(List<Tmodelmain> model, string command, HttpPostedFileBase upd1, string hfrow)
        //{
        //    string ctype1 = "", ctype2 = "", ctype3 = "", ctype4 = "", ctype5 = "", ctype6 = "", ctype7 = "",
        //        finalpath1 = "", finalpath2 = "", finalpath3 = "", finalpath4 = "", finalpath5 = "", finalpath6 = "", finalpath7 = "", path1 = "", path2 = "", path3 = "", path4 = "", path5 = "", path6 = "", path7 = "",
        //        fileName1 = "", fileName2 = "", fileName3 = "", fileName4 = "", fileName5 = "", fileName6 = "", fileName7 = "", encpath1 = "", encpath2 = "", encpath3 = "", encpath4 = "", encpath5 = "", encpath6 = "", encpath7 = "",
        //    iscontractor = "N";
        //    FillMst(model[0].Col15);
        //    var tm = model.FirstOrDefault();
        //    DataTable dt = new DataTable();
        //    #region dropdown      
        //    List<SelectListItem> mod1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
        //    List<SelectListItem> mod2 = (List<SelectListItem>)TempData[MyGuid + "_TList2"];
        //    List<SelectListItem> mod3 = (List<SelectListItem>)TempData[MyGuid + "_TList3"];
        //    List<SelectListItem> mod4 = (List<SelectListItem>)TempData[MyGuid + "_TList4"];
        //    List<SelectListItem> mod5 = (List<SelectListItem>)TempData[MyGuid + "_TList5"];
        //    List<SelectListItem> mod6 = (List<SelectListItem>)TempData[MyGuid + "_TList6"];
        //    List<SelectListItem> mod7 = (List<SelectListItem>)TempData[MyGuid + "_TList7"];
        //    List<SelectListItem> mod8 = (List<SelectListItem>)TempData[MyGuid + "_TList8"];
        //    List<SelectListItem> mod9 = (List<SelectListItem>)TempData[MyGuid + "_TList9"];
        //    List<SelectListItem> mod10 = (List<SelectListItem>)TempData[MyGuid + "_TList10"];
        //    List<SelectListItem> mod11 = (List<SelectListItem>)TempData[MyGuid + "_TList11"];
        //    List<SelectListItem> mod12 = (List<SelectListItem>)TempData[MyGuid + "_TList12"];
        //    List<SelectListItem> mod13 = (List<SelectListItem>)TempData[MyGuid + "_TList13"];
        //    model[0].TList1 = mod1;
        //    model[0].TList2 = mod2;
        //    model[0].TList3 = mod3;
        //    model[0].TList4 = mod4;
        //    model[0].TList5 = mod5;
        //    model[0].TList6 = mod6;
        //    model[0].TList7 = mod7;
        //    model[0].TList8 = mod8;
        //    model[0].TList9 = mod9;
        //    model[0].TList10 = mod10;
        //    model[0].TList11 = mod11;
        //    model[0].TList12 = mod12;
        //    model[0].TList13 = mod13;
        //    TempData[MyGuid + "_TList1"] = model[0].TList1;
        //    TempData[MyGuid + "_TList2"] = model[0].TList2;
        //    TempData[MyGuid + "_TList3"] = model[0].TList3;
        //    TempData[MyGuid + "_TList4"] = model[0].TList4;
        //    TempData[MyGuid + "_TList5"] = model[0].TList5;
        //    TempData[MyGuid + "_TList6"] = model[0].TList6;
        //    TempData[MyGuid + "_TList7"] = model[0].TList7;
        //    TempData[MyGuid + "_TList8"] = model[0].TList8;
        //    TempData[MyGuid + "_TList9"] = model[0].TList9;
        //    TempData[MyGuid + "_TList10"] = model[0].TList10;
        //    TempData[MyGuid + "_TList11"] = model[0].TList11;
        //    TempData[MyGuid + "_TList12"] = model[0].TList12;
        //    TempData[MyGuid + "_TList13"] = model[0].TList13;
        //    if (model[0].SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
        //    if (model[0].SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
        //    if (model[0].SelectedItems3 == null) model[0].SelectedItems3 = new string[] { "" };
        //    if (model[0].SelectedItems4 == null) model[0].SelectedItems4 = new string[] { "" };
        //    if (model[0].SelectedItems5 == null) model[0].SelectedItems5 = new string[] { "" };
        //    if (model[0].SelectedItems6 == null) model[0].SelectedItems6 = new string[] { "" };
        //    if (model[0].SelectedItems7 == null) model[0].SelectedItems7 = new string[] { "" };
        //    if (model[0].SelectedItems8 == null) model[0].SelectedItems8 = new string[] { "" };
        //    if (model[0].SelectedItems9 == null) model[0].SelectedItems9 = new string[] { "" };
        //    if (model[0].SelectedItems10 == null) model[0].SelectedItems10 = new string[] { "" };
        //    if (model[0].SelectedItems11 == null) model[0].SelectedItems11 = new string[] { "" };
        //    if (model[0].SelectedItems12 == null) model[0].SelectedItems12 = new string[] { "" };
        //    if (model[0].SelectedItems13 == null) model[0].SelectedItems13 = new string[] { "" };
        //    #endregion
        //    if (command == "New")
        //    {
        //        model = newenq(model);
        //    }
        //    else if (command == "Cancel")
        //    {
        //        return CancelFun(model);
        //    }
        //    else if (command == "Callback")
        //    {
        //        if (sgen.GetSession(MyGuid, "delid") != null) { btnval = "FDEL"; }
        //        else if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
        //        model = StartCallback(model);
        //    }
        //    else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
        //    {
        //        try
        //        {
        //            string Isbilling = "", sez = "N";
        //            var tmodel = model.FirstOrDefault();
        //            string currdate = sgen.server_datetime(userCode);
        //            if (model[0].Chk1 == true) isgstr = "Y";
        //            if (model[0].Chk2 == true) iscontractor = "Y";
        //            type = model[0].Col12;
        //            DataTable dtstr = new DataTable();
        //            //dtstr = sgen.getdata(userCode, "select * from " + model[0].Col10 + " WHERE 1=2");
        //            dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
        //            if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
        //            {
        //                mq1 = " and vch_num<>'" + tmodel.Col17.Trim() + "'";
        //                isedit = true;
        //                //vch_num = tmodel.Col17.Trim();
        //                model[0].Col16 = tmodel.Col16.Trim();
        //            }
        //            else
        //            {
        //                mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where type='" + model[0].Col12 + "' " + model[0].Col11 + "  ";
        //                vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
        //                isedit = false;
        //                model[0].Col16 = vch_num;
        //            }
        //            #region dtstr
        //            DataRow dr = dtstr.NewRow();
        //            dr["vch_num"] = model[0].Col16.Trim();
        //            dr["vch_date"] = currdate;
        //            dr["type"] = type.Trim();
        //            dr["status"] = status.Trim();
        //            dr["c_name"] = model[0].Col20;//client_unit_id
        //            dr["panno"] = model[0].Col18;//client_id
        //            dr["COUNTRY"] = model[0].Col28;//remark
        //            dr["COUNTRY2"] = model[0].Col29;//follow remark
        //            dr["PTYPE"] = model[0].SelectedItems2.FirstOrDefault();//next action
        //            dr["cpname"] = model[0].SelectedItems1.FirstOrDefault();//contact
        //            dr["MT_DT"] = sgen.Savedate(model[0].Col30, false);//Next Action Date///////////////
        //            if (isedit)
        //            {
        //                dr["client_id"] = model[0].Col1.Trim();
        //                dr["client_unit_id"] = model[0].Col2.Trim();
        //                dr["vch_num"] = model[0].Col16;
        //                dr["rec_id"] = model[0].Col7;
        //                dr["ent_by"] = model[0].Col3;
        //                dr["ent_date"] = model[0].Col4;
        //                dr["edit_by"] = userid_mst;
        //                dr["edit_date"] = currdate;
        //            }
        //            else
        //            {
        //                dr["rec_id"] = "0";
        //                dr["client_id"] = clientid_mst;
        //                dr["client_unit_id"] = unitid_mst;
        //                dr["ent_by"] = userid_mst;
        //                dr["ent_date"] = currdate;
        //                dr["edit_by"] = "-";
        //                dr["edit_by"] = currdate;
        //            }
        //            #endregion
        //            dtstr.Rows.Add(dr);
        //            ok = sgen.Update_data(userCode, dtstr, model[0].Col10, tmodel.Col8, isedit);
        //            if (ok == true)
        //            {
        //                DataTable dtfile = new DataTable();
        //                //dtfile = sgen.getdata(userCode, "select * from file_tab WHERE 1=2");
        //                dtfile = cmd_Fun.GetStructure(userCode, "file_tab");
        //                string serverpath = Server.MapPath("~/Uploads/" + cg_com_name.Replace(" ", "") + "/");
        //                #region attachment
        //                //
        //                try
        //                {
        //                    HttpPostedFileBase file1 = upd1;
        //                    ctype1 = file1.ContentType;
        //                    fileName1 = file1.FileName;
        //                    path1 = clientid_mst + unitid_mst + userid_mst + currdate + "_" + fileName1;
        //                    encpath1 = sgen.Convert_Stringto64(path1).ToString();
        //                    finalpath1 = serverpath + encpath1;
        //                    file1.SaveAs(finalpath1);
        //                    filesave(model, currdate, dtfile, fileName1, encpath1, "Att", ctype1);
        //                }
        //                catch (Exception ex) { }
        //                #endregion
        //                res = sgen.Update_data(userCode, dtfile, "file_tab", "", false);
        //                if (!res) { ViewBag.scripCall = "showmsgJS(1, 'Something Went Wrong, File Not Save', 0);"; return View(model); }
        //                if (command.Trim().ToUpper().Equals("SAVE") || command.Trim().ToUpper().Equals("UPDATE"))
        //                {
        //                    sgen.SetSession(MyGuid, "RNEW", "N");
        //                    ViewBag.vnew = "";
        //                    ViewBag.vedit = "";
        //                    ViewBag.vsave = "disabled='disabled'";
        //                    ViewBag.vsavenew = "disabled='disabled'";
        //                    ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
        //                }
        //                else if (command.Trim().ToUpper().Equals("SAVE & NEW") || command.Trim().ToUpper().Equals("UPDATE & NEW"))
        //                {
        //                    try
        //                    {
        //                        sgen.SetSession(MyGuid, "RNEW", "Y");
        //                        model = newparty_unit(model);
        //                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
        //                    }
        //                    catch (Exception ex) { }
        //                }
        //                model = new List<Tmodelmain>();
        //                model.Add(new Tmodelmain()
        //                {
        //                    Col9 = tmodel.Col9,
        //                    Col16 = tmodel.Col16,
        //                    Col10 = tmodel.Col10,
        //                    Col11 = tmodel.Col11,
        //                    Col12 = tmodel.Col12,
        //                    Col13 = "Save",
        //                    Col100 = "Save & New",
        //                    Col14 = tmodel.Col14,
        //                    Col15 = tmodel.Col15,
        //                    Col39 = tmodel.Col39,
        //                    Col40 = tmodel.Col40,
        //                    Col41 = tmodel.Col41,
        //                    Col42 = tmodel.Col42,
        //                    TList1 = mod1,
        //                    TList2 = mod2,
        //                    SelectedItems1 = new string[] { "" },
        //                    SelectedItems2 = new string[] { "" },
        //                    LTM1 = new List<Tmodelmain>(),
        //                });
        //            }
        //            else { ViewBag.scripCall += "showmsgJS(1, 'Record Not Saved', 0);"; }
        //        }
        //        catch (Exception ex) { ViewBag.scripCall += "showmsgJS(1, '" + ex.Message.ToString() + "', 0);"; }
        //    }
        //    else if (command == "Contact")
        //    {
        //        try
        //        {
        //            #region state                   
        //            //mq = "select cpcont,cpemail from clients_mst where  type='UNC' and client_unit_id='" + unitid_mst+"' and " +
        //            //    "vch_num='"+model[0].Col20+"' and cpname='"+model[0].SelectedItems1.FirstOrDefault()+"'";
        //            mq = "select cpcont,cpemail from clients_mst where  type='UNC' and client_unit_id='" + unitid_mst + "' and " +
        //               "vch_num='" + model[0].Col20 + "' and rec_id='" + model[0].SelectedItems1.FirstOrDefault() + "'";
        //            dt = sgen.getdata(userCode, mq);
        //            if (dt.Rows.Count > 0)
        //            {
        //                model[0].Col21 = dt.Rows[0]["cpcont"].ToString();
        //                model[0].Col22 = dt.Rows[0]["cpemail"].ToString();
        //            }
        //            #endregion
        //            model[0].TList2 = mod2;
        //            ViewBag.vnew = "disabled='disabled'";
        //            ViewBag.vedit = "disabled='disabled'";
        //            ViewBag.vsave = "";
        //        }
        //        catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1, '" + ex.Message.ToString() + "', 0);"; }
        //    }
        //    else if (command == "Add")
        //    {
        //        try
        //        {
        //            Tmodelmain ntm1 = new Tmodelmain();
        //            ntm1.Col1 = (Convert.ToInt32(model[0].LTM2.Max(x => sgen.Make_int(x.Col1))) + 1).ToString();
        //            ntm1.TList2 = mod2;
        //            ntm1.SelectedItems2 = new string[] { "" };
        //            ntm1.Col40 = "Choose File";
        //            model[0].LTM2.Add(ntm1);
        //            ViewBag.vnew = "disabled='disabled'";
        //            ViewBag.vedit = "disabled='disabled'";
        //            ViewBag.vsave = "";
        //        }
        //        catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "',0);"; }
        //    }
        //    else if (command == "Remove")
        //    {
        //        if (model[0].LTM2.Count > 1) model[0].LTM2.RemoveAt(sgen.Make_int(hfrow));
        //        else
        //        {
        //            ViewBag.scripCall = "showmsgJS(1,'You cannot remove last entry',2)";
        //            ViewBag.vnew = "disabled='disabled'";
        //            ViewBag.vedit = "disabled='disabled'";
        //            ViewBag.vsave = "";
        //            return View(model);
        //        }
        //        ViewBag.vnew = "disabled='disabled'";
        //        ViewBag.vedit = "disabled='disabled'";
        //        ViewBag.vsave = "";
        //    }
        //    ModelState.Clear();
        //    return View(model);
        //}
        #endregion
        [HttpGet]
        public FileResult fdown(string value, string type, string Myguid)
        {
            string path = "", fileName = "";
            FillMst(Myguid);
            if (!value.Trim().Equals(""))
            {
                DataTable dt2 = new DataTable();
                mq = "select file_name,file_path from file_tab where rec_id='" + value.Trim() + "' and type='" + type + "' and client_id='"
                    + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
                dt2 = sgen.getdata(userCode, mq);
                if (dt2.Rows.Count > 0)
                {
                    path = Convert.ToString(dt2.Rows[0]["file_path"]);
                    fileName = Convert.ToString(dt2.Rows[0]["file_name"]);
                }
            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Uploads/" + cg_com_name.Replace(" ", "") + "/" + path));
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        //public void fdelete(string value, string type, string Myguid)
        //{
        //    sgen = new sgenFun(MyGuid);
        //    sgen.SetSession(Myguid, "delid", value);
        //}
        [HttpPost]
        public void fdelete(string value)
        {
            sgen = new sgenFun(MyGuid);
            sgen.SetSession(MyGuid, "delid", value);
        }
        #region poapprvd_po
        public ActionResult poappr()
        {
            FillMst();
            chkRef();
            //ViewBag.vnew = "";
            //ViewBag.vedit = "";
            //ViewBag.vsave = "disabled='disabled'";
            //ViewBag.scripCall = "disableForm();";
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            var model = new List<Tmodelmain>();
            Tmodelmain tm1 = new Tmodelmain();
            tm1.Col9 = "PO APPROVAL";
            tm1.Col10 = "purchases";
            tm1.Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            tm1.Col12 = "";
            tm1.Col13 = "";
            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = MyGuid.Trim();

            string amt = "", ig = "", cols = "", where = "", joins = "", fstr = "";
            mq = "select col2 ig,col4 amt,col5 apptype from enx_tab where type='KC2' and client_unit_id='" + unitid_mst + "' and col3='" + userid_mst + "'";
            DataTable dt = sgen.getdata(userCode, mq);
            if (dt.Rows.Count > 0)
            {
                mq0 = dt.Rows[0]["apptype"].ToString();
                tm1.Col21 = mq0;
                if (mq0.Trim() == "1")
                {
                    amt = dt.Rows[0]["amt"].ToString();
                    cols = "p.netamt ";
                    joins = "";
                    where = " and p.netamt <=" + sgen.Make_int(amt) + "";
                    fstr = "(p.vch_num||p.type||p.client_unit_id) Sno,";
                }
                else if (mq0.Trim() == "2")
                {
                    ig = dt.Rows[0]["ig"].ToString().Replace(",", "','");
                    cols = "p.icode,i.iname,p.qtyord ";
                    joins = "inner join item i on i.icode = p.icode and i.type = 'IT' and find_in_set(i.client_unit_id, p.client_unit_id)= 1 ";
                    where = " and substr(p.icode,1,3) in ('" + ig + "')";
                    fstr = "(p.vch_num||p.icode||p.type||p.client_unit_id) Sno,";
                }
                else if (mq0.Trim() == "3")
                {
                    amt = dt.Rows[0]["amt"].ToString();
                    ig = dt.Rows[0]["ig"].ToString().Replace(",", "','");
                    cols = "p.icode,i.iname,p.qtyord ";
                    joins = "inner join item i on i.icode = p.icode and i.type = 'IT' and find_in_set(i.client_unit_id, p.client_unit_id)= 1 ";
                    where = " and p.netamt <=" + sgen.Make_int(amt) + " and substr(p.icode,1,3) in ('" + ig + "')";
                    fstr = "(p.vch_num||p.icode||p.type||p.client_unit_id) Sno,";
                }

                mq = "select " + fstr + "'false' chk,p.ver,p.vch_num pono,to_char(p.vch_date,'yyyyMMdd') podate,u.user_id entby,p.ent_date," + cols + "" +
                     "from purchases p " +
                     "inner join user_details u on u.vch_num = p.ent_by and u.type = 'CPR' " +
                     "" + joins + "" +
                     "where p.type in ('50','51','52','53','54') and p.client_unit_id = '" + unitid_mst + "' and p.approved = 'N'" + where + "";
                tm1.dt1 = sgen.getdata(userCode, mq);
            }

            model.Add(tm1);
            return View(model);
        }
        [HttpPost]
        public ActionResult poappr(List<Tmodelmain> model, string command, string hfrow, string hftable)
        {
            try
            {
                string fstr = "";
                FillMst();
                DataSet ds = new DataSet();
                if (hftable.Trim() != "")
                {
                    ds = sgen.setDS(hftable);
                    model[0].dt1 = ds.Tables[0];
                }
                var tm = model.FirstOrDefault();
                var apptype = model[0].Col21;
                if (apptype == "1") fstr = "(vch_num||type||client_unit_id)";
                else fstr = "(vch_num||icode||type||client_unit_id)";
                if (command == "Callback")
                {
                    model = StartCallback(model);
                }
                else if (command == "Approved")
                {
                    string currdate = sgen.server_datetime(userCode);
                    for (int i = 0; i < model[0].dt1.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(model[0].dt1.Rows[i][1].ToString()) == true)
                        {
                            mq = "update purchases set approved='Y',appby='" + userid_mst + "',appdate=to_date('" + currdate + "','" + sgen.GetSaveSqlDateFormat() + "') where " +
                                "" + fstr + "='" + model[0].dt1.Rows[i][0].ToString() + "'";
                            res = sgen.execute_cmd(userCode, mq);
                        }
                    }
                    if (res)
                    {
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Approved Successfully');disableForm();";
                        return RedirectToAction(actionName, new { @m_id = EncryptDecrypt.Encrypt(model[0].Col15), @mid = EncryptDecrypt.Encrypt(model[0].Col14) });
                    }
                    else { ViewBag.scripCall += "showmsgJS(1, 'Something went wrong in approving', 0);"; }
                }
                else if (command == "Reject")
                {
                    string currdate = sgen.server_datetime(userCode);
                    for (int i = 0; i < model[0].dt1.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(model[0].dt1.Rows[i][0].ToString()) == true)
                        {
                            mq = "update purchases set approved='R',appby='" + userid_mst + "',appdate='" + currdate + "' where " +
                                "" + fstr + "='" + model[0].dt1.Rows[i][0].ToString() + "'";
                            res = sgen.execute_cmd(userCode, mq);
                        }
                    }
                    if (res)
                    {
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Reject Successfully');disableForm();";
                        return RedirectToAction(actionName, new { @m_id = EncryptDecrypt.Encrypt(model[0].Col15), @mid = EncryptDecrypt.Encrypt(model[0].Col14) });
                    }
                    else { ViewBag.scripCall += "showmsgJS(1, 'Something went wrong in approving', 0);"; }
                }
                else if (command == "Hold")
                {
                    string currdate = sgen.server_datetime(userCode);
                    for (int i = 0; i < model[0].dt1.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(model[0].dt1.Rows[i][0].ToString()) == true)
                        {
                            mq = "update purchases set approved='H',appby='" + userid_mst + "',appdate='" + currdate + "' where " +
                                "" + fstr + "='" + model[0].dt1.Rows[i][0].ToString() + "'";
                            res = sgen.execute_cmd(userCode, mq);
                        }
                    }
                    if (res)
                    {
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Hold Successfully');disableForm();";
                        return RedirectToAction(actionName, new { @m_id = EncryptDecrypt.Encrypt(model[0].Col15), @mid = EncryptDecrypt.Encrypt(model[0].Col14) });
                    }
                    else { ViewBag.scripCall += "showmsgJS(1, 'Something went wrong in approving', 0);"; }
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
        #region vd_po
        public ActionResult vd_po()
        {
            FillMst();
            chkRef();
            ViewBag.scripCall = "disableForm();";
            ViewBag.div_po1 = "style=display:none;";
            ViewBag.div_po2 = "style=display:none;";
            ViewBag.div_po3 = "style=display:none;";
            ViewBag.div_po4 = "style=display:none;";
            ViewBag.div_hr = "style=display:none;";
            List<Tmodelmain> model = new List<Tmodelmain>();
            Tmodelmain tm1 = new Tmodelmain();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<SelectListItem> mod2 = new List<SelectListItem>();
            List<SelectListItem> mod3 = new List<SelectListItem>();
            List<SelectListItem> mod4 = new List<SelectListItem>();
            List<SelectListItem> mod5 = new List<SelectListItem>();
            List<SelectListItem> mod6 = new List<SelectListItem>();
            List<SelectListItem> mod7 = new List<SelectListItem>();
            List<SelectListItem> mod8 = new List<SelectListItem>();
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            tm1.Col10 = "purchases";
            tm1.Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = MyGuid.Trim();
            tm1.TList1 = mod1;
            tm1.SelectedItems1 = new string[] { "" };
            tm1.TList2 = mod2;
            tm1.SelectedItems2 = new string[] { "" };
            tm1.TList3 = mod3;
            tm1.SelectedItems3 = new string[] { "" };
            tm1.TList4 = mod4;
            tm1.SelectedItems4 = new string[] { "" };
            tm1.TList5 = mod5;
            tm1.SelectedItems5 = new string[] { "" };
            tm1.TList6 = mod6;
            tm1.SelectedItems6 = new string[] { "" };
            tm1.TList7 = mod7;
            tm1.SelectedItems7 = new string[] { "" };
            tm1.TList8 = mod8;
            tm1.SelectedItems8 = new string[] { "" };
            TempData[MyGuid + "_TList1"] = tm1.TList1;
            TempData[MyGuid + "_TList2"] = tm1.TList2;
            TempData[MyGuid + "_TList3"] = tm1.TList3;
            TempData[MyGuid + "_TList4"] = tm1.TList4;
            TempData[MyGuid + "_TList5"] = tm1.TList5;
            TempData[MyGuid + "_TList6"] = tm1.TList6;
            TempData[MyGuid + "_TList7"] = tm1.TList7;
            TempData[MyGuid + "_TList8"] = tm1.TList8;
            tm1.Col18 = sgen.seekval(userCode, "select unit_gstin_no from company_unit_profile where cup_id='" + unitid_mst + "'", "unit_gstin_no");
            tm1.Col99 = sgen.seekval(userCode, "select ll_act from company_unit_profile where company_profile_id='" + clientid_mst + "' and cup_id='" + unitid_mst + "'", "ll_act");//local_currency
            sgen.SetSession(MyGuid, "LOCALCUR", tm1.Col99);
            model.Add(tm1);
            ModelState.Clear();
            return View(model);
        }
        [HttpPost]
        public ActionResult vd_po(List<Tmodelmain> model, string command, string hfrow, string hftable, HttpPostedFileBase upd1)
        {
            try
            {
                FillMst(model[0].Col15);
                DataTable dtm = sgen.settable(hftable);
                if (dtm.Rows.Count > 0)
                {
                    if (model[0].Col12 != null)
                    {
                        if (model[0].Col12.Trim().Equals("50") || model[0].Col12.Trim().Equals("52") || model[0].Col12.Trim().Equals("54"))
                        {
                            if (model[0].Col29 == "002") model[0].dt1 = dtm;
                            else model[0].dt2 = dtm;
                        }
                    }
                }
                var tm = model.FirstOrDefault();
                #region ddl          
                List<SelectListItem> mod1 = new List<SelectListItem>();
                List<SelectListItem> mod2 = new List<SelectListItem>();
                List<SelectListItem> mod3 = new List<SelectListItem>();
                List<SelectListItem> mod4 = new List<SelectListItem>();
                List<SelectListItem> mod5 = new List<SelectListItem>();
                List<SelectListItem> mod6 = new List<SelectListItem>();
                List<SelectListItem> mod7 = new List<SelectListItem>();
                List<SelectListItem> mod8 = new List<SelectListItem>();
                TempData[MyGuid + "_TList1"] = model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
                TempData[MyGuid + "_TList2"] = model[0].TList2 = (List<SelectListItem>)TempData[MyGuid + "_TList2"];
                TempData[MyGuid + "_TList3"] = model[0].TList3 = (List<SelectListItem>)TempData[MyGuid + "_TList3"];
                TempData[MyGuid + "_TList4"] = model[0].TList4 = (List<SelectListItem>)TempData[MyGuid + "_TList4"];
                TempData[MyGuid + "_TList5"] = model[0].TList5 = (List<SelectListItem>)TempData[MyGuid + "_TList5"];
                TempData[MyGuid + "_TList6"] = model[0].TList6 = (List<SelectListItem>)TempData[MyGuid + "_TList6"];
                TempData[MyGuid + "_TList7"] = model[0].TList7 = (List<SelectListItem>)TempData[MyGuid + "_TList7"];
                TempData[MyGuid + "_TList8"] = model[0].TList8 = (List<SelectListItem>)TempData[MyGuid + "_TList8"];
                if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
                if (tm.SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
                if (tm.SelectedItems3 == null) model[0].SelectedItems3 = new string[] { "" };
                if (tm.SelectedItems4 == null) model[0].SelectedItems4 = new string[] { "" };
                if (tm.SelectedItems5 == null) model[0].SelectedItems5 = new string[] { "" };
                if (tm.SelectedItems6 == null) model[0].SelectedItems6 = new string[] { "" };
                if (tm.SelectedItems7 == null) model[0].SelectedItems7 = new string[] { "" };
                if (tm.SelectedItems8 == null) model[0].SelectedItems8 = new string[] { "" };
                #endregion
                if (command == "Cancel")
                {
                    return CancelFun(model);
                }
                else if (command == "Callback")
                {
                    if (sgen.GetSession(MyGuid, "podelrno") != null) btnval = "CHRGDEL";
                    else if (sgen.GetSession(MyGuid, "delid") != null) { btnval = "FDEL"; }
                    else if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
                    model = CallbackFun(btnval, "N", actionName, controllerName, model);
                    ViewBag.scripCall = "disableForm();";
                    ViewBag.div_po1 = "style=display:block;";
                    ViewBag.div_po2 = "style=display:block;";
                    ViewBag.div_po3 = "style=display:block;";
                    ViewBag.div_po4 = "style=display:block;";
                    ViewBag.div_hr = "style=display:block;";
                }
                else if (command == "Accept PO")
                {
                    mq = "update purchases set vdst='A' where " + model[0].Col8 + "";
                    res = sgen.execute_cmd(userCode, mq);
                    if (res)
                    {
                        ViewBag.scripCall = "disableForm();";
                        ViewBag.div_po1 = "style=display:none;";
                        ViewBag.div_po2 = "style=display:none;";
                        ViewBag.div_po3 = "style=display:none;";
                        ViewBag.div_po4 = "style=display:none;";
                        ViewBag.div_hr = "style=display:none;";
                        ViewBag.scripCall += "showmsgJS(1, 'PO accepted', 1);";
                    }
                }
                else if (command == "Reject PO")
                {
                    mq = "update purchases set vdst='R' where " + model[0].Col8 + "";
                    res = sgen.execute_cmd(userCode, mq);
                    if (res)
                    {
                        ViewBag.scripCall = "disableForm();";
                        ViewBag.div_po1 = "style=display:none;";
                        ViewBag.div_po2 = "style=display:none;";
                        ViewBag.div_po3 = "style=display:none;";
                        ViewBag.div_po4 = "style=display:none;";
                        ViewBag.div_hr = "style=display:none;";
                        ViewBag.scripCall += "showmsgJS(1, 'PO rejected', 1);";
                    }
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
        #region vd_dis
        public ActionResult vd_dis()
        {
            FillMst();
            chkRef();
            ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.vsave = "disabled='disabled'";
            ViewBag.scripCall = "disableForm();";
            List<Tmodelmain> model = new List<Tmodelmain>();
            Tmodelmain tm1 = new Tmodelmain();
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            tm1.Col9 = "DISPATCH";
            tm1.Col10 = "purchases";
            tm1.Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            tm1.Col12 = "VDS";
            tm1.Col13 = "Save";
            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = MyGuid.Trim();
            tm1.Col30 = sgen.seekval(userCode, "select emp_id from user_details where type='CPR' and vch_num='" + userid_mst + "'", "emp_id");
            model.Add(tm1);
            ModelState.Clear();
            return View(model);
        }
        [HttpPost]
        public ActionResult vd_dis(List<Tmodelmain> model, string command, string hfrow, string hftable, HttpPostedFileBase upd1)
        {
            try
            {
                FillMst(model[0].Col15);
                DataTable dtm = sgen.settable(hftable);
                if (dtm.Rows.Count > 0) { model[0].dt1 = dtm; }
                var tm = model.FirstOrDefault();
                if (command == "New")
                {
                    sgen.SetSession(MyGuid, "EDMODE", "N");
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.scripCall = "enableForm();";
                    mq = "select max(to_number(vch_num)) auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12 + "'" + model[0].Col11 + "";
                    vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                    model[0].Col16 = vch_num;
                    model[0].Col17 = sgen.server_datetime_local(userCode);
                    DataTable dt = sgen.getdata(userCode, "select '0' SNo ,'-' Icode,'-' IName,'-' PartNo,'-' UOM,'0' Po_Qty,'0' Bal_Qty,'0' Dispatch_Qty," +
                        "'-' as Iremark,'0' Pono,'-' Podate from dual");
                    model[0].dt1 = dt;
                    sgen.SetSession(MyGuid, "dtvd_dis", dt);
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
                    DataTable dtf = new DataTable();
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
                    }
                    if (model[0].dt1.Rows.Count > 0)
                    {
                        if (model[0].dt1.Rows[0]["icode"].ToString().Trim().Equals("-") || model[0].dt1.Rows[0]["icode"].ToString().Trim().Equals(""))
                        {
                            ViewBag.scripCall += "showmsgJS(1,'Please Select item in grid-1', 2);";
                            ViewBag.vnew = "disabled='disabled'";
                            ViewBag.vedit = "disabled='disabled'";
                            ViewBag.vsave = "";
                            ViewBag.vsavenew = "";
                            return View(model);
                        }
                    }
                    DataTable dataTable = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    string vch_date = sgen.Make_date_S(model[0].Col17);
                    #region
                    for (int i = 0; i < model[0].dt1.Rows.Count; i++)
                    {
                        DataRow dr = dataTable.NewRow();
                        dr["rec_id"] = "0";
                        dr["type"] = model[0].Col12;
                        dr["vch_num"] = vch_num.Trim();
                        dr["vch_date"] = vch_date;
                        dr["acode"] = model[0].Col30;//vendor code                  
                        dr["price_term"] = model[0].Col18; //veh no.
                        dr["tmode"] = model[0].Col19;//driver name
                        dr["tname"] = model[0].Col20;//driver contact
                        dr["indno"] = model[0].Col21;//ewaybillno
                        dr["inddate"] = sgen.Make_date_S(model[0].Col22);//billdate
                        dr["col1"] = model[0].Col23;//disno
                        dr["date1"] = sgen.Make_date_S(model[0].Col24);//disdate
                        //dt====
                        dr["icode"] = model[0].dt1.Rows[i][1].ToString();
                        dr["iname"] = model[0].dt1.Rows[i][2].ToString();
                        dr["cpartno"] = model[0].dt1.Rows[i][3].ToString();
                        dr["uom"] = model[0].dt1.Rows[i][4].ToString();
                        dr["qtyord"] = model[0].dt1.Rows[i][5].ToString();//poqty
                        dr["qtybal"] = model[0].dt1.Rows[i][6].ToString();//balqty
                        dr["qtyind"] = model[0].dt1.Rows[i][7].ToString();//dispatchqty
                        dr["iremark"] = model[0].dt1.Rows[i][8].ToString();
                        dr["col2"] = model[0].dt1.Rows[i][9].ToString();    //pono                                            
                        dr["date2"] = sgen.Make_date_S(model[0].dt1.Rows[i][10].ToString());//podate
                        if (isedit)
                        {
                            dr["client_id"] = model[0].Col1.Trim();
                            dr["client_unit_id"] = model[0].Col2.Trim();
                            dr["ent_by"] = model[0].Col3;
                            dr["ent_date"] = model[0].Col4;
                            dr["edit_by"] = userid_mst;
                            dr["edit_date"] = currdate;
                        }
                        else
                        {
                            dr["client_id"] = clientid_mst;
                            dr["client_unit_id"] = unitid_mst;
                            dr["ent_by"] = userid_mst;
                            dr["ent_date"] = currdate;
                            dr["edit_by"] = "-";
                            dr["edit_date"] = currdate;
                        }
                        dataTable.Rows.Add(dr);
                    }
                    #endregion
                    bool Result = sgen.Update_data(userCode, dataTable, model[0].Col10.Trim(), model[0].Col8, isedit);
                    if (Result == true)
                    {
                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col10 = tm.Col10,
                            Col11 = tm.Col11,
                            Col13 = "Save",
                            Col100 = "Save & New",
                            Col121 = "S",
                            Col122 = "<u>S</u>ave",
                            Col123 = "Save & Ne<u>w</u>",
                            Col14 = tm.Col14,
                            Col15 = tm.Col15,
                            Col30 = tm.Col30,
                            dt1 = ((DataTable)sgen.GetSession(MyGuid, "dtvd_dis")).Copy()

                        });
                        //sgen.SetSession(MyGuid, "dtvd_dis", null);
                    }
                    else
                    {
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                        ViewBag.scripCall += "mytoast('error', 'toast-top-right', 'Data Not Saved');";
                        ModelState.Clear();
                        return View(model);
                    }
                }
                else if (command == "-")
                {
                    if (model[0].dt1.Rows.Count > 1) model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                    else model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "dtvd_dis");
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
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