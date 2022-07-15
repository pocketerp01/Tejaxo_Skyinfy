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
using System.Text.RegularExpressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DevTrends.MvcDonutCaching;

namespace skyinfyMVC.Controllers
{
    [HandleError(View = "Error")]
    public class CmaController : Controller
    {
        static string mq = "", mq1 = "", mq0 = "", mq2 = "", vch_num = "", status = "", btnval = "", tab_name = "", type = "", type_desc = "", tab_name1 = "", where = "", mid_mst = "", m_id_mst = "", cond = "",
              cond_dpjoin = "", cond_dgjoin = "", cond_catjoin = "", status1 = "", status2 = "";

        string ctype1 = "", ctype2 = "", ctype3 = "", finalpath1 = "", finalpath2 = "", finalpath3 = "", path1 = "", path2 = "", path3 = "", fileName1 = "",
          fileName2 = "", fileName3 = "", encpath1 = "", encpath2 = "", encpath3 = "", MyGuid = "";


        string cmd="", userCode = "", html = "", userid_mst = "", cg_com_name = "", clientid_mst = "", username_mst = "", subdomain_mst = "", unitid_mst = "",
          role_mst = "", Ac_Year = "", actionName = "", controllerName = "", controls_mst = "", year_from = "", year_to = "", xprdrange = "";
        public static bool isedit = false;
        sgenFun sgen;
        Cmd_Fun cmd_Fun;
        #region all fun
        public void FillMst(string Myguid = "")
        {
            MyGuid = Myguid;
            //sgen = new sgenFun(MyGuid);
            if (MyGuid == "")
            {
                MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            }
            //if (MyGuid == "") MyGuid = sgen.GetCookie("", "MyGuid");
            sgen = new sgenFun(MyGuid);
            cmd_Fun = new Cmd_Fun(MyGuid);
            userCode = sgen.GetCookie(MyGuid, "userCode");
            userid_mst = sgen.GetCookie(MyGuid, "userid_mst");
            cg_com_name = sgen.GetCookie(MyGuid, "cg_com_name");
            clientid_mst = sgen.GetCookie(MyGuid, "clientid_mst");
            unitid_mst = sgen.GetCookie(MyGuid, "unitid_mst");
            role_mst = sgen.GetCookie(MyGuid, "role_mst");
            subdomain_mst = sgen.GetCookie(MyGuid, "subdomain_mst");
            Ac_Year = sgen.GetCookie(MyGuid, "Ac_Year");
            controls_mst = sgen.GetCookie(MyGuid, "controls_mst");
            actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            sgen.SetSession(MyGuid, "viewName", actionName);
            sgen.SetSession(MyGuid, "controllerName", controllerName);
            username_mst = sgen.GetCookie(Myguid, "username_mst");
            year_to = sgen.GetCookie(MyGuid, "year_to");
            year_from = sgen.GetCookie(MyGuid, "year_from");
            xprdrange = "and to_date(to_char(vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') " +
                "and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "')";
        }
        public List<Tmodelmain> getload(List<Tmodelmain> model)
        {
            //if (userCode.Equals("")) Response.Redirect(sgen.GetCookie("", sgenFun.callbackurl));
            //if (Request.UrlReferrer == null) { Response.Redirect(sgen.GetCookie("", sgenFun.callbackurl)); }
            chkRef();
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

     
        #region make query
        public void Make_query(string viewname, string title, string btnval, string seektype, string param1, string param2, string Myguid)
        {
            FillMst(Myguid);        
            sgen.SetSession(MyGuid, "btnval", btnval);
            string cmd = "";
            switch (viewname.ToLower())
            {
               
                #region Letter Of Credit
                case "lc":
                    switch (btnval.ToUpper())
                    {
                        case "PARTY":
                           
                            cmd = cmd_Fun.showparty(unitid_mst);
                            break;
                        case "EDIT":
                            cmd = "select  (a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr,a.vch_num doc_no," +
                                "to_char(a.vch_date) doc_date, nvl(b.c_name,a.col3) client_name  from enx_tab a left join clients_mst b on a.col2=b.vch_num and find_in_set(b.client_unit_id,'" + unitid_mst+"')=1" +
                                " and substr(b.vch_num,0,3)='303' where a.type='LOC' and a.client_unit_id='" + unitid_mst+"'";
                            break;
                       

                    }
                    break;
                #endregion
                #region Packing Credit Limit
                case "plc":
                    switch (btnval.ToUpper())
                    {
                        case "PARTY":

                            cmd = cmd_Fun.showparty(unitid_mst);
                            break;
                        case "EDIT":
                            cmd = "select  (a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr,a.vch_num doc_no," +
                                "to_char(a.vch_date) doc_date, nvl(b.c_name,a.col3) client_name  from enx_tab a left join clients_mst b on a.col2=b.vch_num and find_in_set(b.client_unit_id,'" + unitid_mst + "')=1" +
                                " and substr(b.vch_num,0,3)='303' where a.type='PCL' and a.client_unit_id='" + unitid_mst + "'";
                            break;


                    }
                    break;
                #endregion

                #region Bank Gurantee
                case "bank_gur":
                    switch (btnval.ToUpper())
                    {
                        case "PARTY":

                            cmd = cmd_Fun.showparty(unitid_mst);
                            break;
                        case "EDIT":
                            cmd = "select  (a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr,a.vch_num doc_no," +
                                  "to_char(a.vch_date) doc_date, nvl(b.c_name,a.col3) client_name  from enx_tab a left join clients_mst b on a.col2=b.vch_num and find_in_set(b.client_unit_id,'" + unitid_mst + "')=1" +
                                  " and substr(b.vch_num,0,3)='303' where a.type='BGR' and a.client_unit_id='" + unitid_mst + "'";
                            break;


                    }
                    break;
                    #endregion


            }
            sgen.open_grid(title, cmd, sgen.Make_int(seektype));
            ViewBag.pageurl = sgen.GetSession(MyGuid, "pageurl").ToString();
        }
        #endregion
        #region call back
        public List<Tmodelmain> CallbackFun(string btnval, string edmode, string viewName, string controllerName, List<Tmodelmain> model)
        {
            //FillMst(MyGuid);
            var tm = model.FirstOrDefault();
            String URL = sgen.GetSession(MyGuid, "SSEEKVAL").ToString();
            string defcall = "";

            DataTable dtt = new DataTable();
            switch (viewName.ToLower())
            {
                #region Letter Of Credit
                case "lc":
                    switch (btnval.ToUpper())
                    {
                        case "PARTY":

                            mq = cmd_Fun.getparty(URL);
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                model[0].Col18 = dtt.Rows[0]["vch_num"].ToString();
                                model[0].Col19 = dtt.Rows[0]["c_name"].ToString();
                            }
                                break;
                        case "EDIT":
                            mq = "select a.rec_id, a.vch_num,a.vch_date,a.client_id,a.client_unit_id,a.ent_by,a.ent_date,a.col2,a.col3,col7,a.col8,a.col9,a.col10," +
                                " a.col11,a.col12,a.col13,b.c_name from enx_tab a left join clients_mst b on a.col2=b.vch_num and find_in_set(b.client_unit_id,'" + unitid_mst+"')=1 where a.client_id|| a.client_unit_id|| a.vch_num|| to_char(a.vch_date, 'yyyymmdd')|| a.type='" + URL + "'";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                //model[0].Col13 = "Update";
                                //model[0].Col100 = "Update & New";
                                //model[0].Col121 = "U";
                                //model[0].Col122 = "<u>U</u>pdate";
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                                model[0].Col7 = dtt.Rows[0]["rec_id"].ToString();
                                model[0].Col8 = "(client_id||client_unit_id|| vch_num|| to_char(vch_date, 'yyyymmdd')|| type) = '" + URL + "'";                             
                                model[0].Col16 = dtt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dtt.Rows[0]["vch_date"].ToString();                              
                                model[0].Col18 = dtt.Rows[0]["col2"].ToString();
                                model[0].Col19 = dtt.Rows[0]["c_name"].ToString();
                                model[0].Col20 = dtt.Rows[0]["col3"].ToString();
                                model[0].Col21 = dtt.Rows[0]["col7"].ToString();
                                model[0].Col22 = dtt.Rows[0]["col8"].ToString();
                                model[0].Col23 = dtt.Rows[0]["col9"].ToString();
                                model[0].Col24 = dtt.Rows[0]["col10"].ToString();
                                model[0].Col28 = dtt.Rows[0]["col11"].ToString();
                                model[0].Col29 = dtt.Rows[0]["col12"].ToString();
                                model[0].Col30 = dtt.Rows[0]["col13"].ToString();
                            }
                            break;


                    }
                    break;
                #endregion
                #region Packing Credit 
                case "plc":
                    switch (btnval.ToUpper())
                    {
                        case "PARTY":

                            mq = cmd_Fun.getparty(URL);
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                model[0].Col18 = dtt.Rows[0]["vch_num"].ToString();
                                model[0].Col19 = dtt.Rows[0]["c_name"].ToString();
                            }
                            break;
                        case "EDIT":
                            mq = "select a.rec_id, a.vch_num,a.vch_date,a.client_id,a.client_unit_id,a.ent_by,a.ent_date,a.col2,a.col3,col7,a.col8,a.col9,a.col10," +
                                " a.col11,b.c_name from enx_tab a left join clients_mst b on a.col2=b.vch_num and find_in_set(b.client_unit_id,'" + unitid_mst + "')=1 where a.client_id|| a.client_unit_id|| a.vch_num|| to_char(a.vch_date, 'yyyymmdd')|| a.type='" + URL + "'";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                //model[0].Col13 = "Update";
                                //model[0].Col100 = "Update & New";
                                //model[0].Col121 = "U";
                                //model[0].Col122 = "<u>U</u>pdate";
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                                model[0].Col7 = dtt.Rows[0]["rec_id"].ToString();
                                model[0].Col8 = "(client_id||client_unit_id|| vch_num|| to_char(vch_date, 'yyyymmdd')|| type) = '" + URL + "'";
                                model[0].Col16 = dtt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dtt.Rows[0]["vch_date"].ToString();
                                model[0].Col18 = dtt.Rows[0]["col2"].ToString();
                                model[0].Col19 = dtt.Rows[0]["c_name"].ToString();
                                model[0].Col20 = dtt.Rows[0]["col3"].ToString();
                                model[0].Col21 = dtt.Rows[0]["col7"].ToString();
                                model[0].Col22 = dtt.Rows[0]["col8"].ToString();
                                model[0].Col23 = dtt.Rows[0]["col9"].ToString();
                                model[0].Col24 = dtt.Rows[0]["col10"].ToString();
                                model[0].Col27 = dtt.Rows[0]["col11"].ToString();
                                
                             
                            }
                            break;


                    }
                    break;
                #endregion

                #region Bank Guranatee
                case "bank_gur":
                    switch (btnval.ToUpper())
                    {
                        case "PARTY":

                            mq = cmd_Fun.getparty(URL);
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                model[0].Col18 = dtt.Rows[0]["vch_num"].ToString();
                                model[0].Col19 = dtt.Rows[0]["c_name"].ToString();
                            }
                            break;
                        case "EDIT":
                            mq = "select a.rec_id, a.vch_num,a.vch_date,a.client_id,a.client_unit_id,a.ent_by,a.ent_date,a.col2,a.col3,col7,a.col8,a.col9,a.col10," +
                                " a.col11,b.c_name from enx_tab a left join clients_mst b on a.col2=b.vch_num and find_in_set(b.client_unit_id,'" + unitid_mst + "')=1 where a.client_id|| a.client_unit_id|| a.vch_num|| to_char(a.vch_date, 'yyyymmdd')|| a.type='" + URL + "'";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                //model[0].Col13 = "Update";
                                //model[0].Col100 = "Update & New";
                                //model[0].Col121 = "U";
                                //model[0].Col122 = "<u>U</u>pdate";
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                                model[0].Col7 = dtt.Rows[0]["rec_id"].ToString();
                                model[0].Col8 = "(client_id||client_unit_id|| vch_num|| to_char(vch_date, 'yyyymmdd')|| type) = '" + URL + "'";
                                model[0].Col16 = dtt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dtt.Rows[0]["vch_date"].ToString();
                                model[0].Col18 = dtt.Rows[0]["col2"].ToString();
                                model[0].Col19 = dtt.Rows[0]["c_name"].ToString();
                                model[0].Col20 = dtt.Rows[0]["col3"].ToString();
                                model[0].Col21 = dtt.Rows[0]["col7"].ToString();
                                model[0].Col22 = dtt.Rows[0]["col8"].ToString();
                                model[0].Col23 = dtt.Rows[0]["col9"].ToString();
                                model[0].Col24 = dtt.Rows[0]["col10"].ToString();
                                model[0].Col27 = dtt.Rows[0]["col11"].ToString();


                            }
                            break;


                    }
                    break;
                    #endregion
            }
            return model;
        }
        #endregion
        #region fa_ratio
        public ActionResult fa_ratio()
        {
            FillMst("");
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);

            List<SelectListItem> mod1 = new List<SelectListItem>();
            model[0].Col9 = "FINANCIAL STATEMENT ANALYSIS";
            model[0].Col10 = "enx_tab2";
            model[0].Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
            model[0].Col12 = "SPT";
            model[0].TList1 = model[0].TList2 = mod1;
            model[0].SelectedItems1 = new string[] { "" };
            return View(model);
        }
        [HttpPost]
        public ActionResult fa_ratio(List<Tmodelmain> model, string command)
        {
            try
            {
                FillMst(model[0].Col15);
                var tm = model.FirstOrDefault();
                if (command == "New")
                {
                    if (model[0].Col55 == "N")
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for new lead, please contact your admin', 2);";
                        return View(model);
                    }
                    model = getnew(model);
                }
                else if (command == "Cancel")
                {
                    return CancelFun(model);
                }
                else if (command == "Callback")
                {
                    if ((model[0].Col56 == "N") && (btnval == "EDIT"))
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for edit lead, please contact your admin', 2);disableForm();";
                        return View(model);
                    }
                    model = StartCallback(model);
                }

                else if (command.Trim() == "Save" || command.Trim() == "Update")
                {
                    if (model[0].Col55 == "N")
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for save lead , please contact your admin', 2);";
                        return View(model);
                    }

                    var tmodel = model.FirstOrDefault();
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

                    DataRow dr = dtstr.NewRow();
                    dr["rec_id"] = "0";
                    dr["type"] = model[0].Col12;
                    dr["vch_num"] = vch_num.Trim();
                    dr["vch_date"] = sgen.Savedate(model[0].Col17, false);
                    dr = getsave(currdate, dr, model);
                    bool Result = sgen.Update_data_fast1(userCode, dtstr, model[0].Col10.Trim(), tmodel.Col8, isedit);
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
                            Col14 = tm.Col14,
                            Col15 = tm.Col15


                        });

                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        ViewBag.vsavenew = "disabled='disabled'";
                        ViewBag.scripCall = "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
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
        #region wacc
        public ActionResult wacc()
        {
            FillMst("");
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
           
            List<SelectListItem> mod1 = new List<SelectListItem>();
            model[0].Col9 = "WEIGHTED AVERAGE COST OF CAPITAL";
            model[0].Col10 = "enx_tab2";
            model[0].Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
            model[0].Col12 = "SPT";
            model[0].TList1 = model[0].TList2 = mod1;
            model[0].SelectedItems1 = new string[] { "" };
            return View(model);
        }
        [HttpPost]
        public ActionResult wacc(List<Tmodelmain> model, string command)
        {
            try
            {
                FillMst(model[0].Col15);
                var tm = model.FirstOrDefault();
                if (command == "New")
                {
                    if (model[0].Col55 == "N")
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for new lead, please contact your admin', 2);";
                        return View(model);
                    }
                    model = getnew(model);
                }
                else if (command == "Cancel")
                {
                    return CancelFun(model);
                }
                else if (command == "Callback")
                {
                    if ((model[0].Col56 == "N") && (btnval == "EDIT"))
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for edit lead, please contact your admin', 2);disableForm();";
                        return View(model);
                    }
                    model = StartCallback(model);
                }

                else if (command.Trim() == "Save" || command.Trim() == "Update")
                {
                    if (model[0].Col55 == "N")
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for save lead , please contact your admin', 2);";
                        return View(model);
                    }

                    var tmodel = model.FirstOrDefault();
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

                    DataRow dr = dtstr.NewRow();
                    dr["rec_id"] = "0";
                    dr["type"] = model[0].Col12;
                    dr["vch_num"] = vch_num.Trim();
                    dr["vch_date"] = sgen.Savedate(model[0].Col17, false);
                    dr = getsave(currdate, dr, model);
                    bool Result = sgen.Update_data_fast1(userCode, dtstr, model[0].Col10.Trim(), tmodel.Col8, isedit);
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
                            Col14 = tm.Col14,
                            Col15 = tm.Col15


                        });

                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        ViewBag.vsavenew = "disabled='disabled'";
                        ViewBag.scripCall = "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
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
        #region Letter Of Credit
        public ActionResult lc()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);

            List<SelectListItem> mod1 = new List<SelectListItem>();
            model[0].Col9 = "LETTER OF CREDIT";
            model[0].Col10 = "enx_tab";
            model[0].Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
            model[0].Col12 = "LOC";
            DataTable dtb = (DataTable)sgen.GetSession(MyGuid, "URIGHTS");
            try
            {
                dtb = dtb.Select("m_id='" + model[0].Col14 + "'").CopyToDataTable();
                if (dtb.Rows.Count > 0)
                {
                    model[0].Col55 = dtb.Rows[0]["btnnew"].ToString();
                    model[0].Col56 = dtb.Rows[0]["btnedit"].ToString();
                    model[0].Col57 = dtb.Rows[0]["btnview"].ToString();
                    // model[0].Col33 = dtb.Rows[0]["btnextend"].ToString();
                }
            }
            catch (Exception ex) { }
            return View(model);
        }
        [HttpPost]
        public ActionResult lc(List<Tmodelmain> model, string command)
        {
            try
            {
                FillMst(model[0].Col15);
                var tm = model.FirstOrDefault();
                if (command == "New")
                {
                    if (model[0].Col55 == "N")
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for new lead, please contact your admin', 2);";
                        return View(model);
                    }
                    model = getnew(model);
                    model[0].Col17 = sgen.server_datetime_local(userCode);

                }
                else if (command == "Cancel")
                {
                    return CancelFun(model);
                }
                else if (command == "Callback")
                {
                    if ((model[0].Col56 == "N") && (btnval == "EDIT"))
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for edit lead, please contact your admin', 2);disableForm();";
                        return View(model);
                    }
                    model = StartCallback(model);
                }

                else if (command.Trim() == "Save" || command.Trim() == "Update")
                {
                    if (model[0].Col55 == "N")
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for save lead , please contact your admin', 2);";
                        return View(model);
                    }

                    var tmodel = model.FirstOrDefault();
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

                    DataRow dr = dtstr.NewRow();
                    dr["rec_id"] = "0";
                    dr["type"] = model[0].Col12;
                    dr["vch_num"] = vch_num.Trim();
                    dr["vch_date"] = sgen.Savedate(model[0].Col17, false);
                    dr["col2"] = model[0].Col18;// client_id
                    dr["col3"] = model[0].Col20;//new client
                    dr["col7"] = model[0].Col21;//Annual Purchase Of Raw Material
                    dr["col8"] = model[0].Col22;//Annual Purchase Of Raw Material Under Usance LC
                    dr["col9"] = model[0].Col23;//  Annual Purchase Of Raw Material Under Demand LC
                    dr["col10"] = model[0].Col24;//   Time Between Opening Of LC And Shipment Of Goods(Lead time ) ( Month )
                    dr["col11"] = model[0].Col28;//   Transit Period Between Shipment And Receiving Of Goods ( Month )
                    dr["col12"] = model[0].Col29;//   Usance Period ( Month )
                    dr["col13"] = model[0].Col30;//  Value Of The Property Available For Mortgage
                    dr = getsave(currdate, dr, model);
                    dtstr.Rows.Add(dr);
                    bool Result = sgen.Update_data_fast1(userCode, dtstr, model[0].Col10.Trim(), tmodel.Col8, isedit);
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
                            Col14 = tm.Col14,
                            Col15 = tm.Col15


                        });

                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        ViewBag.vsavenew = "disabled='disabled'";
                        ViewBag.scripCall = "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
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
        #region Packing Credit
        public ActionResult plc()
        {
            FillMst("");
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
           
            List<SelectListItem> mod1 = new List<SelectListItem>();
            model[0].Col9 = "PACKING CREDIT LIMIT ";
            model[0].Col10 = "enx_tab";
            model[0].Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
            model[0].Col12 = "PCL";
            DataTable dtb = (DataTable)sgen.GetSession(MyGuid, "URIGHTS");
            try
            {
                dtb = dtb.Select("m_id='" + model[0].Col14 + "'").CopyToDataTable();
                if (dtb.Rows.Count > 0)
                {
                    model[0].Col55 = dtb.Rows[0]["btnnew"].ToString();
                    model[0].Col56 = dtb.Rows[0]["btnedit"].ToString();
                    model[0].Col57 = dtb.Rows[0]["btnview"].ToString();
                    // model[0].Col33 = dtb.Rows[0]["btnextend"].ToString();
                }
            }
            catch (Exception ex) { }
            return View(model);
        }
        [HttpPost]
        public ActionResult plc(List<Tmodelmain> model, string command)
        {
            try
            {
                FillMst(model[0].Col15);
                var tm = model.FirstOrDefault();
                if (command == "New")
                {
                    if (model[0].Col55 == "N")
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for new lead, please contact your admin', 2);";
                        return View(model);
                    }
                    model = getnew(model);
                    model[0].Col17 = sgen.server_datetime_local(userCode);
                }
                else if (command == "Cancel")
                {
                    return CancelFun(model);
                }
                else if (command == "Callback")
                {
                    if ((model[0].Col56 == "N") && (btnval == "EDIT"))
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for edit lead, please contact your admin', 2);disableForm();";
                        return View(model);
                    }
                    model = StartCallback(model);
                }

                else if (command.Trim() == "Save" || command.Trim() == "Update")
                {
                    if (model[0].Col55 == "N")
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for save lead , please contact your admin', 2);";
                        return View(model);
                    }

                    var tmodel = model.FirstOrDefault();
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

                    DataRow dr = dtstr.NewRow();
                    dr["rec_id"] = "0";
                    dr["type"] = model[0].Col12;
                    dr["vch_num"] = vch_num.Trim();
                    dr["vch_date"] = sgen.Savedate(model[0].Col17, false);
                    dr["col2"] = model[0].Col18;// client_id
                    dr["col3"] = model[0].Col20;//new client
                    dr["col7"] = model[0].Col21;// Projected Export Sales For Current/ Next Year
                    dr["col8"] = model[0].Col22;// Gross Margin
                    dr["col9"] = model[0].Col23;//    Cost of Goods Sold
                    dr["col10"] = model[0].Col24;//    Creditors Credit Peroid
                    dr["col11"] = model[0].Col27;//     Usance Period( Date Of Advance Payment To Realisation Of Sale Proceed
                  
                    dr = getsave(currdate, dr, model);
                    dtstr.Rows.Add(dr);

                    bool Result = sgen.Update_data_fast1(userCode, dtstr, model[0].Col10.Trim(), tmodel.Col8, isedit);
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
                            Col14 = tm.Col14,
                            Col15 = tm.Col15


                        });

                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        ViewBag.vsavenew = "disabled='disabled'";
                        ViewBag.scripCall = "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
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
        #region Bank Guranatee
        public ActionResult bank_gur()
        {
            FillMst("");
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            
            List<SelectListItem> mod1 = new List<SelectListItem>();
            model[0].Col9 = "BANK GURANATEE ";
            model[0].Col10 = "enx_tab";
            model[0].Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
            model[0].Col12 = "BGR";
            DataTable dtb = (DataTable)sgen.GetSession(MyGuid, "URIGHTS");
            try
            {
                dtb = dtb.Select("m_id='" + model[0].Col14 + "'").CopyToDataTable();
                if (dtb.Rows.Count > 0)
                {
                    model[0].Col55 = dtb.Rows[0]["btnnew"].ToString();
                    model[0].Col56 = dtb.Rows[0]["btnedit"].ToString();
                    model[0].Col57 = dtb.Rows[0]["btnview"].ToString();
                   
                }
            }
            catch (Exception ex) { }
            return View(model);
        }
        [HttpPost]
        public ActionResult bank_gur(List<Tmodelmain> model, string command)
        {
            try
            {
                FillMst(model[0].Col15);
                var tm = model.FirstOrDefault();
                if (command == "New")
                {
                    if (model[0].Col55 == "N")
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for new lead, please contact your admin', 2);";
                        return View(model);
                    }
                    model = getnew(model);
                    model[0].Col17 = sgen.server_datetime_local(userCode);
                }
                else if (command == "Cancel")
                {
                    return CancelFun(model);
                }
                else if (command == "Callback")
                {
                    if ((model[0].Col56 == "N") && (btnval == "EDIT"))
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for edit lead, please contact your admin', 2);disableForm();";
                        return View(model);
                    }
                    model = StartCallback(model);
                }

                else if (command.Trim() == "Save" || command.Trim() == "Update")
                {
                    if (model[0].Col55 == "N")
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for save lead , please contact your admin', 2);";
                        return View(model);
                    }

                    var tmodel = model.FirstOrDefault();
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

                    DataRow dr = dtstr.NewRow();
                    dr["rec_id"] = "0";
                    dr["type"] = model[0].Col12;
                    dr["vch_num"] = vch_num.Trim();
                    dr["vch_date"] = sgen.Savedate(model[0].Col17, false);
                    dr["col2"] = model[0].Col18;// client_id
                    dr["col3"] = model[0].Col20;//new client
                    dr["col7"] = model[0].Col21;// Total Order Required Bank Guarantee
                    dr["col8"] = model[0].Col22;// Requirement Of Bank Guarantee For Order
                    dr["col9"] = model[0].Col23;//   Value Of The Property Available For Mortgage
                    dr["col10"] = model[0].Col24;//     Period Of Bank Guarantee Required ( Performance /Financial )(Month )
                   
                    
                    dr = getsave(currdate, dr, model);
                    dtstr.Rows.Add(dr);

                    bool Result = sgen.Update_data_fast1(userCode, dtstr, model[0].Col10.Trim(), tmodel.Col8, isedit);
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
                            Col14 = tm.Col14,
                            Col15 = tm.Col15


                        });

                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        ViewBag.vsavenew = "disabled='disabled'";
                        ViewBag.scripCall = "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
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
        #region LC Purchase bill discounting limit
        public ActionResult lcpbd()
        {
            FillMst("");
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
          
            List<SelectListItem> mod1 = new List<SelectListItem>();
            model[0].Col9 = "LC PURCHASE BILL DISCOUNTING LIMIT ";
            model[0].Col10 = "enx_tab2";
            model[0].Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
            model[0].Col12 = "SPT";
            model[0].TList1 = model[0].TList2 = mod1;
            model[0].SelectedItems1 = new string[] { "" };
            return View(model);
        }
        [HttpPost]
        public ActionResult lcpbd(List<Tmodelmain> model, string command)
        {
            try
            {
                FillMst(model[0].Col15);
                var tm = model.FirstOrDefault();
                if (command == "New")
                {
                    if (model[0].Col55 == "N")
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for new lead, please contact your admin', 2);";
                        return View(model);
                    }
                    model = getnew(model);
                }
                else if (command == "Cancel")
                {
                    return CancelFun(model);
                }
                else if (command == "Callback")
                {
                    if ((model[0].Col56 == "N") && (btnval == "EDIT"))
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for edit lead, please contact your admin', 2);disableForm();";
                        return View(model);
                    }
                    model = StartCallback(model);
                }

                else if (command.Trim() == "Save" || command.Trim() == "Update")
                {
                    if (model[0].Col55 == "N")
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for save lead , please contact your admin', 2);";
                        return View(model);
                    }

                    var tmodel = model.FirstOrDefault();
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

                    DataRow dr = dtstr.NewRow();
                    dr["rec_id"] = "0";
                    dr["type"] = model[0].Col12;
                    dr["vch_num"] = vch_num.Trim();
                    dr["vch_date"] = sgen.Savedate(model[0].Col17, false);
                    dr = getsave(currdate, dr, model);
                    bool Result = sgen.Update_data_fast1(userCode, dtstr, model[0].Col10.Trim(), tmodel.Col8, isedit);
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
                            Col14 = tm.Col14,
                            Col15 = tm.Col15


                        });

                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        ViewBag.vsavenew = "disabled='disabled'";
                        ViewBag.scripCall = "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
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
        #region LC sale bill discounting limit
        public ActionResult lcsbd()
        {
            FillMst("");
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
         
            List<SelectListItem> mod1 = new List<SelectListItem>();
            model[0].Col9 = "LC SALE BILL DISCOUNTING LIMIT ";
            model[0].Col10 = "enx_tab2";
            model[0].Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
            model[0].Col12 = "SPT";
            model[0].TList1 = model[0].TList2 = mod1;
            model[0].SelectedItems1 = new string[] { "" };
            return View(model);
        }
        [HttpPost]
        public ActionResult lspbd(List<Tmodelmain> model, string command)
        {
            try
            {
                FillMst(model[0].Col15);
                var tm = model.FirstOrDefault();
                if (command == "New")
                {
                    if (model[0].Col55 == "N")
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for new lead, please contact your admin', 2);";
                        return View(model);
                    }
                    model = getnew(model);
                }
                else if (command == "Cancel")
                {
                    return CancelFun(model);
                }
                else if (command == "Callback")
                {
                    if ((model[0].Col56 == "N") && (btnval == "EDIT"))
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for edit lead, please contact your admin', 2);disableForm();";
                        return View(model);
                    }
                    model = StartCallback(model);
                }

                else if (command.Trim() == "Save" || command.Trim() == "Update")
                {
                    if (model[0].Col55 == "N")
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for save lead , please contact your admin', 2);";
                        return View(model);
                    }

                    var tmodel = model.FirstOrDefault();
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

                    DataRow dr = dtstr.NewRow();
                    dr["rec_id"] = "0";
                    dr["type"] = model[0].Col12;
                    dr["vch_num"] = vch_num.Trim();
                    dr["vch_date"] = sgen.Savedate(model[0].Col17, false);
                    dr = getsave(currdate, dr, model);
                    bool Result = sgen.Update_data_fast1(userCode, dtstr, model[0].Col10.Trim(), tmodel.Col8, isedit);
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
                            Col14 = tm.Col14,
                            Col15 = tm.Col15


                        });

                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        ViewBag.vsavenew = "disabled='disabled'";
                        ViewBag.scripCall = "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
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
        #region unsecured purchase bill discounting limit
        public ActionResult unpbd()
        {
            FillMst("");
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
           
            List<SelectListItem> mod1 = new List<SelectListItem>();
            model[0].Col9 = "UNSECURED PURCHASE BILL DISCOUNTING LIMIT ";
            model[0].Col10 = "enx_tab2";
            model[0].Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
            model[0].Col12 = "SPT";
            model[0].TList1 = model[0].TList2 = mod1;
            model[0].SelectedItems1 = new string[] { "" };
            return View(model);
        }
        [HttpPost]
        public ActionResult unpbd(List<Tmodelmain> model, string command, string hfrow, string hftable)
        {
            FillMst(model[0].Col15);
            var tm = model.FirstOrDefault();
            string cond = "";
            if (!role_mst.ToUpper().Equals("OWNER"))
            {
                string rptto = sgen.getstring(userCode, "select emp_id from user_details where type='CPR' and vch_num='" + userid_mst + "' ");

                cond = "and ( emp.rptto='" + rptto + "' or emp.rptto2='" + rptto + "')";
            }

            if (command == "Submit")
            {
                string status = "";
                Boolean isok = true;
                for (int i = 0; i < model.Count; i++)
                {
                    if (model[i].Col27 == "P")
                    {
                        status = "Pending";
                    }
                    else if (model[i].Col27 == "A")
                    {
                        status = "Approved";
                    }
                    else if (model[i].Col27 == "R")
                    {
                        status = "Rejected";
                    }
                    mq = "update enx_tab2 set col10='" + status + "' where type='SPT' and client_unit_id||vch_num||to_char(vch_date,'yyyymmdd')||type='" + model[i].Col24 + "'";
                    isok = sgen.execute_cmd(userCode, mq);
                }
                if (isok == true)
                {
                    DataTable dt = new DataTable();

                    mq = "select a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type as fstr,a.vch_num as doc_num,a.col5 as shift_start,a.col6 as shift_end" +
                       ",a.col7 as app_by,a.date1 as od_from,a.date2 as od_to,a.col4 as remark,emp.emp_code as Emp_Code" +
                       ",(emp.FIRST_NAME || ' ' || REPLACE(emp.MIDDLE_NAME, '0', '') || ' ' || REPLACE(emp.LAST_NAME, '0', '')) as Emp_Name" +
                       ",nvl(dg.master_name, '-') as designation,rs.master_name as reason,nvl(dp.master_name, '-') as department from enx_tab2 a inner join " +
                       "emp_master emp on emp.vch_num = a.col8 and emp.type = 'EMP' and emp.client_unit_id = '" + unitid_mst + "'and emp.emp_status = 'Y' " + cond + "" +
                       " left join master_setting dp on dp.master_id = emp.emp_dept and dp.type = 'MDP' and " +
                       "find_in_set(dp.client_unit_id,'" + unitid_mst + "')= 1 left join master_setting dg on dg.master_id = emp.emp_desig and " +
                       "dg.type = 'MDG' and find_in_set(dg.client_unit_id,'" + unitid_mst + "')= 1 inner join master_setting rs on rs.master_id = a.col9 and " +
                       "rs.type = 'SRM' and find_in_set(rs.client_unit_id,'" + unitid_mst + "')= 1 where a.type = 'OSL' and a.col10='Pending' and a.client_unit_id = '" + unitid_mst + "'";
                    dt = sgen.getdata(userCode, mq);
                    if (dt.Rows.Count > 0)
                    {
                        model = new List<Tmodelmain>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            model.Add(new Tmodelmain
                            {
                                Col9 = tm.Col9,
                                Col11 = tm.Col11,
                                Col12 = tm.Col12,
                                Col10 = tm.Col10,
                                Col13 = tm.Col13,
                                Col14 = tm.Col14,
                                Col15 = tm.Col15,
                                Col28 = tm.Col28,
                                Col29 = tm.Col29,
                                Col30 = tm.Col30,
                                Col16 = dr["doc_num"].ToString(),
                                Col17 = tm.Col17,
                                Col18 = dr["Emp_Name"].ToString(),
                                Col19 = dr["department"].ToString() + " / " + dr["designation"].ToString(),
                                Col20 = dr["Emp_Code"].ToString(),
                                Col21 = dr["reason"].ToString(),
                                Col22 = dr["remark"].ToString(),
                                Col23 = dr["doc_num"].ToString(),
                                Col24 = dr["fstr"].ToString(),
                                Col27 = "P",
                            });
                        }
                    }
                }

            }
            else if (command == "Pending")
            {
                DataTable dt = new DataTable();

                mq = "select a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type as fstr,a.vch_num as doc_num,a.col5 as shift_start,a.col6 as shift_end" +
                      ",a.col7 as app_by,a.date1 as od_from,a.date2 as od_to,a.col4 as remark,emp.emp_code as Emp_Code" +
                      ",(emp.FIRST_NAME || ' ' || REPLACE(emp.MIDDLE_NAME, '0', '') || ' ' || REPLACE(emp.LAST_NAME, '0', '')) as Emp_Name" +
                      ",nvl(dg.master_name, '-') as designation,rs.master_name as reason,nvl(dp.master_name, '-') as department from enx_tab2 a inner join " +
                      "emp_master emp on emp.vch_num = a.col8 and emp.type = 'EMP' and emp.client_unit_id = '" + unitid_mst + "'and emp.emp_status = 'Y' " + cond + "" +
                      " left join master_setting dp on dp.master_id = emp.emp_dept and dp.type = 'MDP' and " +
                      "find_in_set(dp.client_unit_id,'" + unitid_mst + "')= 1 left join master_setting dg on dg.master_id = emp.emp_desig and " +
                      "dg.type = 'MDG' and find_in_set(dg.client_unit_id,'" + unitid_mst + "')= 1 inner join master_setting rs on rs.master_id = a.col9 and " +
                      "rs.type = 'SRM' and find_in_set(rs.client_unit_id,'" + unitid_mst + "')= 1 where a.type = 'OSL' and a.col10='Pending' and a.client_unit_id = '" + unitid_mst + "'";

                dt = sgen.getdata(userCode, mq);
                if (dt.Rows.Count > 0)
                {
                    model = new List<Tmodelmain>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        model.Add(new Tmodelmain
                        {
                            Col9 = tm.Col9,
                            Col11 = tm.Col11,
                            Col12 = tm.Col12,
                            Col10 = tm.Col10,
                            Col13 = tm.Col13,
                            Col14 = tm.Col14,
                            Col15 = tm.Col15,
                            Col28 = tm.Col28,
                            Col29 = tm.Col29,
                            Col30 = tm.Col30,
                            Col16 = dr["doc_num"].ToString(),
                            Col17 = tm.Col17,
                            Col18 = dr["Emp_Name"].ToString(),
                            Col19 = dr["department"].ToString() + " / " + dr["designation"].ToString(),
                            Col20 = dr["Emp_Code"].ToString(),
                            Col21 = dr["reason"].ToString(),
                            Col22 = dr["remark"].ToString(),
                            Col23 = dr["doc_num"].ToString(),
                            Col24 = dr["fstr"].ToString(),
                            Col27 = "P",
                        });
                    }
                }
            }
            else if (command == "Cancel")
            {
                return CancelFun(model);
            }
            else if (command == "Callback")
            {
                model = StartCallback(model);
            }
            ModelState.Clear();
            return View(model);
        }
        #endregion
        #region loan against rent receivable
        public ActionResult loan_ag_rent()
        {
            FillMst("");
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
           
            List<SelectListItem> mod1 = new List<SelectListItem>();
            model[0].Col9 = "LOAN AGAINST RENT RECEIVABLE ";
            model[0].Col10 = "enx_tab2";
            model[0].Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
            model[0].Col12 = "SPT";
            model[0].TList1 = model[0].TList2 = mod1;
            model[0].SelectedItems1 = new string[] { "" };
            return View(model);
        }
        [HttpPost]
        public ActionResult loan_ag_rent(List<Tmodelmain> model, string command)
        {
            try
            {
                FillMst(model[0].Col15);
                var tm = model.FirstOrDefault();
                if (command == "New")
                {
                    if (model[0].Col55 == "N")
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for new lead, please contact your admin', 2);";
                        return View(model);
                    }
                    model = getnew(model);
                }
                else if (command == "Cancel")
                {
                    return CancelFun(model);
                }
                else if (command == "Callback")
                {
                    if ((model[0].Col56 == "N") && (btnval == "EDIT"))
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for edit lead, please contact your admin', 2);disableForm();";
                        return View(model);
                    }
                    model = StartCallback(model);
                }

                else if (command.Trim() == "Save" || command.Trim() == "Update")
                {
                    if (model[0].Col55 == "N")
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for save lead , please contact your admin', 2);";
                        return View(model);
                    }

                    var tmodel = model.FirstOrDefault();
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

                    DataRow dr = dtstr.NewRow();
                    dr["rec_id"] = "0";
                    dr["type"] = model[0].Col12;
                    dr["vch_num"] = vch_num.Trim();
                    dr["vch_date"] = sgen.Savedate(model[0].Col17, false);
                    dr = getsave(currdate, dr, model);
                    bool Result = sgen.Update_data_fast1(userCode, dtstr, model[0].Col10.Trim(), tmodel.Col8, isedit);
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
                            Col14 = tm.Col14,
                            Col15 = tm.Col15


                        });

                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        ViewBag.vsavenew = "disabled='disabled'";
                        ViewBag.scripCall = "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
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
    }
}