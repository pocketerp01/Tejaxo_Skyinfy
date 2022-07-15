using QRCoder;
using skyinfyMVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Reflection;
using System.Collections;

namespace skyinfyMVC.Controllers
{
    public class AccountController : Controller
    {
        System.Globalization.DateTimeFormatInfo monthName = new System.Globalization.DateTimeFormatInfo();
        string mq0 = "", mq = "", mq1 = "", vch_num = "", btnval = "", tab_name = "", type = "", type_desc = "", tab_name1 = "", ent_date = "", status = "";
        string Master_Type = "", id = "", Condition = "", mid_mst = "", MyGuid = "", master_id, Ac_Year = "", iscfrm = "Y", regnum = "";

        public static string userCode = "", year_to = "", year_from = "", xprdrange = "", clientname_mst = "", m_module3 = "", userid_mst = "", FN_From_Date = "", FN_To_Date = "", Ac_Year_id = "", cg_com_name = "", clientid_mst = "",
          unitid_mst = "", Ac_To_Date = "", Ac_From_Date = "", role_mst = "", cmd = "", actionName = "", controllerName = "";
        string q1 = "", q2 = "", q3 = "", q4 = "";
        bool res = false, Isvalid = false;
        public static string name = "", ctype1 = "", ctype2 = "", ctype3 = "", ctype4 = "", ctype5 = "", ctype6 = "", ctype7 = "",
      finalpath1 = "", finalpath2 = "", finalpath3 = "", finalpath4 = "", finalpath5 = "", finalpath6 = "", finalpath7 = "", path1 = "", path2 = "", path3 = "", path4 = "",
      path5 = "", path6 = "", path7 = "", fileName1 = "", fileName2 = "", fileName3 = "", fileName4 = "", fileName5 = "", fileName6 = "", fileName7 = "", encpath1 = "",
      encpath2 = "", encpath3 = "", encpath4 = "", encpath5 = "", encpath6 = "", encpath7 = "";

        public static sgenFun sgen;
        Cmd_Fun cmd_Fun;
        public static bool isedit = false;
        public static int cli = 0;
        //===============getload==========
        #region MAIN Functions
        public void FillMst(string Myguid = "")
        {
            MyGuid = Myguid;
            if (MyGuid == "") { MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]); }
            Multiton multiton = Multiton.GetInstance(MyGuid);
            sgen = new sgenFun(MyGuid);
            cmd_Fun = new Cmd_Fun(MyGuid);
            userCode = multiton.UserCode;
            userid_mst = sgen.GetCookie(MyGuid, "userid_mst");
            Ac_Year_id = sgen.GetCookie(MyGuid, "Ac_Year_id");
            Ac_Year = sgen.GetCookie(MyGuid, "Ac_Year");
            year_to = sgen.GetCookie(MyGuid, "year_to");
            year_from = sgen.GetCookie(MyGuid, "year_from");
            cg_com_name = sgen.GetCookie(MyGuid, "cg_com_name");
            clientid_mst = sgen.GetCookie(MyGuid, "clientid_mst");
            unitid_mst = sgen.GetCookie(MyGuid, "unitid_mst");
            clientname_mst = sgen.GetCookie(MyGuid, "clientname_mst");
            role_mst = sgen.GetCookie(MyGuid, "role_mst");
            m_module3 = sgen.GetCookie(MyGuid, "m_module3");
            FN_From_Date = sgen.GetCookie(MyGuid, "year_from");
            actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            sgen.SetSession(MyGuid, "viewName", actionName);
            sgen.SetSession(MyGuid, "controllerName", controllerName);
            sgen.SetSession(MyGuid, "BackToBack", "");
            xprdrange = "and to_date(to_char(vch_date,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') " +
                "and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "')";
        }
        public List<Tmodelmain> getload(List<Tmodelmain> model)
        {

            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            chkRef();
            Tmodelmain tm1 = new Tmodelmain();
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
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
            if (actionName == "bank_ac")
            {
                mq = "select max(to_number(substr(vch_num,4,8))) as auto_genid from " + model[0].Col10.Trim() + " a where  type in ('" + model[0].Col12.Trim() + "' ,'DD" + model[0].Col12 + "')" + model[0].Col11.Trim() + " and substr(vch_num,0,3)='" + model[0].Col131 + "'";
                vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                vch_num = model[0].Col131 + vch_num;
                model[0].Col16 = vch_num;

            }
            else
            {
                mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                model[0].Col16 = vch_num;
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
        public DataRow getsave(string curdt, DataRow drn, List<Tmodelmain> model)
        {
            bool edit = false;
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
        private List<Tmodelmain> StartCallback(List<Tmodelmain> model)
        {
            if (sgen.GetSession(MyGuid, "delid") != null) { btnval = "FDEL"; }
            else if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
            model = CallbackFun(btnval, "N", actionName, controllerName, model);
            ViewBag.vnew = "disabled='disabled'";
            ViewBag.vedit = "disabled='disabled'";
            ViewBag.vsave = "";
            return model;
        }
        private List<Tmodelmain> StartCallback(List<Tmodelmain> model, string btnval)
        {
            if (sgen.GetSession(MyGuid, "delid") != null) { btnval = "FDEL"; }
            else if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString(); model = CallbackFun(btnval, "N", actionName, controllerName, model);
            ViewBag.vnew = "disabled='disabled'";
            ViewBag.vedit = "disabled='disabled'";
            ViewBag.vsave = "";
            ViewBag.vsavenew = "";
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
        public ActionResult Index()
        {
            return View();
        }
        public void chkRef()
        {
            if (userCode.Equals("")) Response.Redirect(sgenFun.callbackurl);
            if (Request.UrlReferrer == null) { Response.Redirect(sgenFun.callbackurl); }
        }

        #region filesave
        private void filesave(List<Tmodelmain> model, string currdate, DataTable dtfile, string filename, string filepath, string filetitle, string content_type, string description)
        {
            DataRow drf = dtfile.NewRow();
            drf["rec_id"] = "0";
            drf["vch_num"] = model[0].Col18;
            drf["vch_date"] = currdate;
            drf["type"] = model[0].Col12;
            drf["ref_code"] = model[0].Col18;
            drf["ref_code1"] = model[0].Col18;
            drf["col3"] = model[0].Col19;
            drf["col1"] = filetitle;
            drf["description"] = description;

            drf["col2"] = content_type;
            if (model[0].Col7 == null || model[0].Col7 == "0")
            {
                drf["file_name"] = filename;
                drf["file_path"] = filepath;
                drf["col2"] = content_type;
            }
            else
            {
                if ((model[0].Col14 == "40004.2" || model[0].Col14 == "40004.1" || model[0].Col14 == "33008.2") && isedit == true)
                {
                    drf["rec_id"] = model[0].Col7;
                }
                else
                {
                    drf["rec_id"] = "0";
                }
            }
            drf["ent_by"] = userid_mst;
            drf["ent_date"] = currdate;
            drf["client_id"] = clientid_mst;
            drf["client_unit_id"] = unitid_mst;
            drf["edit_by"] = userid_mst;
            drf["edit_date"] = currdate;
            dtfile.Rows.Add(drf);
        }
        private void filesave(List<Tmodelmain> model, string currdate, DataTable dtfile, string filename, string filepath, string filetitle, string content_type, string description, string ref1, string ref2)
        {
            DataRow drf = dtfile.NewRow();
            drf["vch_num"] = model[0].Col18;
            drf["type"] = model[0].Col12;
            drf["ref_code"] = ref1;
            drf["ref_code1"] = ref2;
            drf["col3"] = model[0].Col19;
            drf["col1"] = filetitle;
            drf["description"] = description;
            drf["vch_date"] = currdate;
            drf["file_name"] = filename;
            drf["file_path"] = filepath;
            drf["col2"] = content_type;
            drf["r_no"] = ref2;
            if ((model[0].Col14 == "40004.2" || model[0].Col14 == "40004.1") && isedit == true)
            {
                drf["rec_id"] = model[0].Col7;
            }
            else
            {
                drf["rec_id"] = "0";
                //drf["vch_date"] = currdate;
                //drf["file_name"] = filename;
                //drf["file_path"] = filepath;
                //drf["col2"] = content_type;
            }
            drf["ent_by"] = userid_mst;
            drf["ent_date"] = currdate;
            drf["client_id"] = clientid_mst;
            drf["client_unit_id"] = unitid_mst;
            drf["edit_by"] = userid_mst;
            drf["edit_date"] = currdate;
            dtfile.Rows.Add(drf);
        }
        private void filesave(List<Tmodelmain> model, string currdate, DataTable dtfile, string filename, string filepath, string filetitle, string content_type, string description, string page_title, string ref1, string ref2)
        {
            //filesave(model, currdate, dtfile, fileName1, encpath1, "RMK", ctype1, desc, pagetitle, ref1, f_rec_id);
            DataRow drf = dtfile.NewRow();
            drf["vch_num"] = model[0].Col16;
            drf["type"] = model[0].Col12;
            drf["ref_code"] = ref1;
            drf["col3"] = page_title;
            drf["col1"] = filetitle;
            drf["description"] = description;
            drf["vch_date"] = currdate;
            drf["file_name"] = filename;
            drf["file_path"] = filepath;
            drf["col2"] = content_type;
            if (ref2 == null || ref2 == "0")
            {
                drf["rec_id"] = "0";
            }
            else
            {
                if ((model[0].Col14 == "40000") && isedit == true)
                {
                    drf["rec_id"] = ref2;
                }
                else
                {
                    drf["rec_id"] = "0";
                }
            }
            drf["ent_by"] = userid_mst;
            drf["ent_date"] = currdate;
            drf["client_id"] = clientid_mst;
            drf["client_unit_id"] = unitid_mst;
            drf["edit_by"] = userid_mst;
            drf["edit_date"] = currdate;
            dtfile.Rows.Add(drf);
        }

        [HttpGet]
        public FileResult fdown(string value, string typ, string Myguid)
        {
            FillMst(Myguid);

            if (!value.Trim().Equals(""))
            {
                type = typ;
                DataTable dt2 = new DataTable();
                mq = "select file_name,file_path from file_tab where rec_id='" + value.Trim() + "' and type='" + typ + "' and client_id='"
                    + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
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
        [HttpPost]
        public void fdelete(string value, string Myguid)
        {
            if (!value.Trim().Equals(""))
            {
                sgen = new sgenFun(MyGuid);
                sgen.SetSession(Myguid, "delid", value);
            }
        }
        #endregion
        #endregion
        //=========CallBack============
        #region call back
        public List<Tmodelmain> CallbackFun(string btnval, string edmode, string viewName, string controllerName, List<Tmodelmain> model)
        {
            string mq = "", caption = "", ttype;
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<SelectListItem> mod2 = new List<SelectListItem>();
            List<SelectListItem> mod3 = new List<SelectListItem>();
            List<SelectListItem> mod4 = new List<SelectListItem>();
            List<SelectListItem> mod5 = new List<SelectListItem>();
            List<SelectListItem> mod6 = new List<SelectListItem>();
            List<SelectListItem> mod7 = new List<SelectListItem>();
            List<SelectListItem> mod8 = new List<SelectListItem>();
            string year = year_from.Substring(6, 4);
            String URL = sgen.GetSession(MyGuid, "SSEEKVAL").ToString();
            btnval = btnval.ToUpper();
            string fdt = "", tdt = "";
            switch (viewName.ToLower())
            {
                #region vou_entry
                case "vou_entry":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            //mq = "select vu.vch_num as doc_num,vu.client_id,vu.client_unit_id,vu.ent_by,vu.ent_date,vu.edit_by,vu.edit_date," +
                            //    "vu.rec_id,to_char(vu.vch_date, '" + sgen.Getsqldateformat() + "') doc_date,vu.type,vu.invno as invoice_no" +
                            //    ",to_char(vu.invdate, '" + sgen.Getsqldateformat() + "') as invoice_date,vu.totremark as remark ," +
                            //    "vu.acode as party_code,nvl(cl.c_name,'-') party_name,vu.cramount,vu.dramount from vouchers vu inner join clients_mst cl on cl.vch_num = vu.acode and find_in_set(vu.client_unit_id, cl.client_unit_id) = 1 " +
                            //    "where (vu.client_unit_id || vu.vch_num || to_char(vu.vch_date, 'yyyymmdd') || vu.type) = '" + URL + "'";
                            mq = "select vu.vch_num as doc_num,vu.BALAMT,vu.type1,vu.adj_type,vu.chqno,vu.rno,vu.chqamt,vu.reftype,vu.client_id,vu.client_unit_id,vu.ent_by,vu.ent_date,vu.edit_by,vu.edit_date," +
                                "vu.rec_id,to_char(vu.chqdate, '" + sgen.Getsqldateformat() + "') chqdate,to_char(vu.vch_date, '" + sgen.Getsqldateformat() + "') doc_date,vu.type,vu.invamt,vu.invno as invoice_no" +
                                ",to_char(vu.invdate, '" + sgen.Getsqldateformat() + "') as invoice_date,vu.totremark as remark,vu.remark as remarkss ," +
                                "vu.acode as party_code,nvl(cl.aname,'-') party_name,vu.cramount,vu.dramount from vouchers vu INNER JOIN " +
                                "(select vch_num as acode,c_name as aname,CLIENT_UNIT_ID from clients_mst where type='BCD' and find_in_set(client_unit_id, '" + unitid_mst + "')=1 union all select vch_num as acode ,c_name as aname,CLIENT_UNIT_ID from clients_mst " +
                                "where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "')=1 union all select cm.vch_num as acode," +
                                "nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname,cm.CLIENT_UNIT_ID from master_setting a inner join clients_mst cm on a.master_id" +
                                " = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id where cm.type = 'BAD' and cm.client_unit_id" +
                                " = '" + unitid_mst + "') cl on cl.acode = vu.acode and find_in_set(vu.client_unit_id, cl.client_unit_id) = 1 " +
                                "where (vu.client_unit_id || vu.vch_num || to_char(vu.vch_date, 'yyyymmdd') || vu.type) = '" + URL + "' and (to_number(vu.dramount)+to_number(vu.cramount))>0 order by vu.vch_num desc";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                model = getedit(model, dtt, URL);
                                model[0].Col8 = "client_unit_id||vch_num|| to_char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";
                                model[0].Col12 = dtt.Rows[0]["type"].ToString();
                                model[0].Col16 = dtt.Rows[0]["doc_num"].ToString();
                                model[0].Col19 = dtt.Rows[0]["invoice_no"].ToString();
                                model[0].Col20 = dtt.Rows[0]["invoice_date"].ToString();
                                model[0].Col21 = dtt.Rows[0]["remark"].ToString();
                                model[0].Col17 = dtt.Rows[0]["doc_date"].ToString();
                                //model[0].Col40 = dtt.Rows[0]["type1"].ToString();
                                //model[0].Col41 = dtt.Rows[0]["adj_type"].ToString();
                                //model[0].Col42 = dtt.Rows[0]["reftype"].ToString();
                                //model[0].Col43 = dtt.Rows[0]["chqno"].ToString();
                                //model[0].Col44 = dtt.Rows[0]["chqdate"].ToString();
                                //model[0].Col45 = dtt.Rows[0]["chqamt"].ToString();
                                model[0].dt1 = model[0].dt1.Clone();
                                for (int i = 0; i < dtt.Rows.Count; i++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["SNo"] = i + 1;
                                    dr["Select_"] = dtt.Rows[i]["invoice_no"].ToString() + "!~!~!" + dtt.Rows[i]["invoice_date"].ToString()
                                        + "!~!~!" + dtt.Rows[i]["rno"].ToString() + "!~!~!" + dtt.Rows[i]["adj_type"].ToString() + "!~!~!" +
                                        dtt.Rows[i]["reftype"].ToString() + "!~!~!" + dtt.Rows[i]["chqamt"].ToString() + "!~!~!" + dtt.Rows[i]["remarkss"].ToString() + "!~!~!" + dtt.Rows[i]["BALAMT"].ToString() + "!~!~!" + dtt.Rows[i]["invamt"].ToString();
                                    dr["Acode"] = dtt.Rows[i]["party_code"].ToString();
                                    dr["Aname"] = dtt.Rows[i]["party_name"].ToString();
                                    dr["CrAmount"] = dtt.Rows[i]["cramount"].ToString();
                                    dr["DrAmount"] = dtt.Rows[i]["dramount"].ToString();
                                    model[0].dt1.Rows.Add(dr);
                                }
                                model[0].dt1.AcceptChanges();
                            }
                            break;
                        case "PRINT":
                            mq = "select vu.vch_num as doc_num,vu.client_id,vu.client_unit_id,to_char(vu.vch_date, '" + sgen.Getsqldateformat() + "') doc_date,vu.type,vu.invno as invoice_no" +
                            ",to_char(vu.invdate, '" + sgen.Getsqldateformat() + "') as invoice_date,vu.totremark as remark ," +
                            "vu.acode as party_code,nvl(cl.aname,'-') party_name,(case when vu.type='32' then 'CREDIT NOTE' when vu.type='31' then 'DEBIT NOTE' when vu.type='30' then 'JOURNAL VOUCHER' when vu.type='33' then 'CONTRA VOUCHER' ELSE 'VOUCHER' end) AS VOUTITLE" +
                            ",to_number(vu.cramount) cramount,to_number(vu.dramount) dramount from vouchers vu INNER JOIN " +
                              "(select vch_num as acode,c_name as aname,CLIENT_UNIT_ID from clients_mst where type='BCD' and find_in_set(client_unit_id, '" + unitid_mst + "')=1 union all select vch_num as acode ,c_name as aname,CLIENT_UNIT_ID from clients_mst " +
                              "where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "')=1 union all select cm.vch_num as acode," +
                              "nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname,cm.CLIENT_UNIT_ID from master_setting a inner join clients_mst cm on a.master_id" +
                              " = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id where cm.type = 'BAD' and cm.client_unit_id" +
                              " = '" + unitid_mst + "') cl on cl.acode = vu.acode and find_in_set(vu.client_unit_id, cl.client_unit_id) = 1 " +
                              "where (vu.client_unit_id || vu.vch_num || to_char(vu.vch_date, 'yyyymmdd') || vu.type) = '" + URL + "' order by vu.vch_num desc";
                            //mq = "select vu.vch_num as doc_num,vu.client_id,vu.client_unit_id,to_char(vu.vch_date, '" + sgen.Getsqldateformat() + "') doc_date,vu.type,vu.invno as invoice_no" +
                            //",to_char(vu.invdate, '" + sgen.Getsqldateformat() + "') as invoice_date,vu.totremark as remark ," +
                            //"vu.acode as party_code,nvl(cl.c_name,'-') party_name,(case when vu.type='32' then 'CREDIT NOTE' when vu.type='31' then 'DEBIT NOTE' when vu.type='30' then 'JOURNAL VOUCHER' when vu.type='33' then 'CONTRA VOUCHER' ELSE 'VOUCHER' end) AS VOUTITLE,to_number(vu.cramount) cramount,to_number(vu.dramount) dramount from vouchers vu inner join clients_mst cl on cl.vch_num = vu.acode and find_in_set(vu.client_unit_id, cl.client_unit_id) = 1 " +
                            //"where (vu.client_unit_id || vu.vch_num || to_char(vu.vch_date, 'yyyymmdd') || vu.type) = '" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                DataSet ds = new DataSet();
                                dt.TableName = "prepcur";
                                ds.Tables.Add(dt);
                                sgen.open_report_byDs_xml(userCode, ds, "voucher1", "Print Voucher");
                                ViewBag.scripCall += "showRptnew('Print Voucher');disableForm();";
                            }
                            else
                            {
                                ViewBag.scripCall = "showmsgJS(1, 'Data Not Exist', 2);";
                            }
                            break;
                        case "PARTY":
                            mq = "select * from (select a.vch_num||a.type as fstr,a.vch_num ,a.c_name as Client_name,a.addr as address,a.c_gstin as GSTIN," +
                                "(case when a.tor = 'R' then 'Regular' when a.tor='C' then 'Composition' else 'Not Registered' END) GSTType," +
                                "a.type_desc as Search_text from clients_mst a  where a.type = 'BCD' and substr(a.vch_num,0,3)='203' and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "' " +
                                "union all select a.vch_num || a.type as fstr,a.vch_num,ledger_name Client_name, ledger_abb address,to_char(group_code) GSTIN," +
                                "'-' GSTType,'-' Search_text from ledger_master a where a.type = 'ALM' and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "') " +
                                "a where a.fstr in ('" + URL + "')";
                            dtt = sgen.getdata(userCode, mq);
                            if (model[0].dt1.Rows[0]["acode"].ToString().Trim().Equals("-"))
                            {
                                model[0].dt1.Rows.RemoveAt(0);
                            }
                            if (dtt.Rows.Count > 0)
                            {
                                DataRow dr = model[0].dt1.NewRow();
                                dr[0] = dtt.Rows[0]["fstr"].ToString();
                                dr[1] = dtt.Rows[0]["vch_num"].ToString();
                                dr[2] = dtt.Rows[0]["Client_name"].ToString();
                                dr[3] = "0";
                                dr[4] = "0";
                                model[0].dt1.Rows.Add(dr);
                            }
                            break;
                    }
                    break;
                #endregion
                #region groupmaster
                case "grp_mst":
                    model[0].Col20 = ""; model[0].Col19 = ""; model[0].Col21 = ""; model[0].Col22 = ""; model[0].Col23 = "";
                    q1 = "SELECT client_id||client_unit_id||vch_num||TO_CHAR(vch_date, 'YYYYMMDD')|| type as fstr,master_type as Financial_Category_Code , master_name AS Financial_Category,'-' Main_Head,'-' as Sub_Head,'-' as Sub_Sub_Head,COL1 as recon FROM master_setting where type = 'AGM' AND TRIM(CLIENT_ID)= '-' AND TRIM(CLIENT_UNIT_ID)= '-'";
                    q2 = "SELECT  (a.client_id||a.client_unit_id|| a.vch_num|| TO_CHAR(a.vch_date, 'YYYYMMDD')|| a.type) as fstr,a.master_type as Financial_Category_Code ,b.master_name as Financial_Category,a.master_name as Main_Head,'-' as Sub_Head,'-' as Sub_Sub_Head,a.COL1 as recon FROM master_setting a INNER JOIN master_setting b ON SUBSTR(a.master_type, 1, 1) = SUBSTR(b.master_type, 1, 1)  AND TO_NUMBER(SUBSTR(b.master_type,2,7))= 0   and b.type = 'AGM'  where TO_NUMBER(SUBSTR(a.master_type,2,3))> 0 AND TO_NUMBER(SUBSTR(a.master_type,4,7))= 0 and a.type = 'AGM' and a.client_id='" + clientid_mst + "' and a.client_unit_id='" + unitid_mst + "'";
                    q3 = "SELECT (a.client_id|| a.client_unit_id|| a.vch_num|| TO_CHAR(a.vch_date, 'YYYYMMDD')|| a.type) as fstr,a.master_type as Financial_Category_Code ,b.master_name as Financial_Category ,c.master_name as Main_Head,a.master_name as Sub_Head,'-' as Sub_Sub_Head,a.COL1 as recon  FROM master_setting a INNER JOIN master_setting b ON SUBSTR(a.master_type, 1, 1) = SUBSTR(b.master_type, 1, 1)  AND TO_NUMBER(SUBSTR(b.master_type,2,7))= 0   and b.type = 'AGM'  INNER JOIN master_setting c ON SUBSTR(a.master_type,1,3)= SUBSTR(c.master_type, 1, 3)  AND TO_NUMBER(SUBSTR(c.master_type,4,7))= 0   and c.type = 'AGM'  where TO_NUMBER(SUBSTR(a.master_type,4,5))> 0 AND TO_NUMBER(SUBSTR(a.master_type,6,7))= 0 and a.type = 'AGM' and a.client_id='" + clientid_mst + "' and a.client_unit_id='" + unitid_mst + "'  and a.client_id=c.client_id and  a.client_unit_id = c.client_unit_id";
                    q4 = "SELECT  (a.client_id|| a.client_unit_id|| a.vch_num|| TO_CHAR(a.vch_date, 'YYYYMMDD')|| a.type) as fstr,a.master_type as Financial_Category_Code ,b.master_name AS Financial_Category,c.master_name as Main_Head,d.master_name as Sub_Head,a.master_name as Sub_Sub_Head,a.COL1 as recon  FROM master_setting a INNER JOIN master_setting b ON SUBSTR(a.master_type, 1, 1) = SUBSTR(b.master_type, 1, 1)  AND TO_NUMBER(SUBSTR(b.master_type,2,7))= 0   and b.type = 'AGM'  INNER JOIN master_setting c ON SUBSTR(a.master_type,1,3)= SUBSTR(c.master_type, 1, 3)  AND TO_NUMBER(SUBSTR(c.master_type,4,7))= 0   and c.type = 'AGM'  INNER JOIN master_setting d ON SUBSTR(a.master_type,1,5)= SUBSTR(d.master_type, 1, 5)  AND TO_NUMBER(SUBSTR(d.master_type,6,7))= 0   and d.type = 'AGM'  where TO_NUMBER(SUBSTR(a.master_type,6,7))> 0  and a.type = 'AGM' and a.client_id='" + clientid_mst + "' and a.client_unit_id='" + unitid_mst + "'  and a.client_id=c.client_id and a.client_unit_id= c.client_unit_id and a.client_id=d.client_id and a.client_unit_id=d.client_unit_id";
                    switch (btnval.ToUpper())
                    {
                        case "GRP":
                            mq = q1 + " union " + q2 + " union " + q3;
                            mq = "select * from (" + mq + ")a where a.fstr='" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                if (dt.Rows[0]["Sub_Head"].ToString() != "-")
                                {
                                    model[0].Col19 = dt.Rows[0]["Sub_Head"].ToString();
                                }
                                if (dt.Rows[0]["Sub_Head"].ToString() == "-")
                                {
                                    model[0].Col19 = dt.Rows[0]["Main_Head"].ToString();
                                }
                                if (dt.Rows[0]["Main_Head"].ToString() == "-")
                                {
                                    model[0].Col19 = dt.Rows[0]["Financial_Category"].ToString();
                                }
                                model[0].Col20 = dt.Rows[0]["Financial_Category"].ToString();
                                model[0].Col21 = dt.Rows[0]["Main_Head"].ToString();
                                model[0].Col22 = dt.Rows[0]["Sub_Head"].ToString();
                                sgen.SetSession(MyGuid, "EDMODE", "N");
                                string ff = ""; int k = 0;
                                foreach (var ch in dt.Rows[0]["Financial_Category_Code"].ToString().ToCharArray().Reverse())
                                {
                                    if (!ch.Equals('0')) { break; }
                                    else ff = ff + ch;
                                }
                                char[] chars = ff.ToCharArray();
                                chars.Reverse();
                                ff = new string(chars);
                                string mqf = dt.Rows[0]["Financial_Category_Code"].ToString().TrimEnd(chars);
                                model[0].Col24 = mqf;
                                ff = ""; k = 0;
                                mqf = "";
                                if (model[0].Col23 != "")
                                {
                                    foreach (var ch in model[0].Col23.ToCharArray().Reverse())
                                    {
                                        if (!ch.Equals('0')) { break; }
                                        else ff = ff + ch;
                                    }
                                    chars = ff.ToCharArray();
                                    chars.Reverse();
                                    ff = new string(chars);
                                    mqf = model[0].Col23.ToString().TrimEnd(chars);
                                    mqf = mqf.Substring(0, mqf.Trim().Length - 2);
                                }
                                if (!mqf.Equals(model[0].Col24.Trim()))
                                {
                                    string id1 = sgen.genNo(userCode, "select max(to_number(substr(master_type," + (model[0].Col24.Length + 1) + ", 2))) as max " +
                 "from " + model[0].Col10 + " where type='" + model[0].Col12 + "'  and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "' " +
                 "and  substr(master_type,1," + model[0].Col24.Length + ")='" + model[0].Col24 + "'", 2, "max");
                                    string finalid = (model[0].Col24 + id1.ToString()).PadRight(7, '0');
                                    model[0].Col25 = "Y";
                                    model[0].Col23 = finalid;
                                }
                            }
                            break;
                        case "EDIT":
                            model[0].Col8 = "(client_id|| client_unit_id|| vch_num|| TO_CHAR(vch_date, 'YYYYMMDD')|| type)='" + URL + "'";
                            model[0].Col25 = "";
                            model[0].Col13 = "Update";
                            model[0].Col100 = "Update & New";
                            sgen.SetSession(MyGuid, "EDMODE", "Y");
                            isedit = true;
                            mq = q2 + " union " + q3 + " union " + q4;
                            mq = "select * from (" + mq + ")a where a.fstr='" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                if (dt.Rows[0]["Sub_Head"].ToString() != "-")
                                {
                                    model[0].Col19 = dt.Rows[0]["Sub_Head"].ToString();
                                    model[0].Col16 = dt.Rows[0]["Sub_Head"].ToString();
                                }
                                if (dt.Rows[0]["Sub_Head"].ToString() == "-")
                                {
                                    model[0].Col19 = dt.Rows[0]["Main_Head"].ToString();
                                    model[0].Col16 = dt.Rows[0]["Main_Head"].ToString();
                                }
                                if (dt.Rows[0]["Main_Head"].ToString() == "-")
                                {
                                    model[0].Col19 = dt.Rows[0]["Financial_Category"].ToString();
                                    model[0].Col16 = dt.Rows[0]["Financial_Category"].ToString();
                                }
                                model[0].Col20 = dt.Rows[0]["Financial_Category"].ToString();
                                model[0].Col21 = dt.Rows[0]["Main_Head"].ToString();
                                model[0].Col22 = dt.Rows[0]["Sub_Head"].ToString();
                                model[0].Col23 = dt.Rows[0]["Financial_Category_Code"].ToString();
                                if (dt.Rows[0]["recon"].ToString().Trim().ToUpper().Equals("Y")) model[0].Chk1 = true;
                                else model[0].Chk1 = false;
                                mq = "select * from " + model[0].Col10 + " a where (a.client_id|| a.client_unit_id|| a.vch_num||TO_CHAR(a.vch_date, 'YYYYMMDD')|| a.type)='" + URL + "'";
                                dt = sgen.getdata(userCode, mq);
                                model[0].Col17 = dt.Rows[0]["sectionid"].ToString();
                                model[0].Col1 = dt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dt.Rows[0]["master_entby"].ToString();
                                model[0].Col4 = dt.Rows[0]["master_entdate"].ToString();
                                model[0].Col7 = dt.Rows[0]["rec_id"].ToString();
                                model[0].Col27 = dt.Rows[0]["vch_num"].ToString();
                            }
                            break;
                    }
                    break;
                #endregion
                #region cur_rate
                case "cur_rate":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select a.client_id,a.client_unit_id,a.master_id,a.rec_id,a.vch_num,a.master_name,a.status,a.classid,a.sectionid,a.subjectid," +
                                 "to_char(convert_tz(a.date1,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') effdate," +
                                 "a.optional_subject,a.teacher_category,a.master_entby ent_by,a.master_entdate ent_date,a.vch_date from master_setting a " +
                                 "where (a.client_id||a.client_unit_id|| a.master_id||to_char(a.vch_date, 'yyyymmdd')|| a.type)='" + URL + "'";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");

                                #region Currency

                                mq1 = "select master_id, master_name from master_setting where  type='CTM' " + model[0].Col11 + "";

                                dt = sgen.getdata(userCode, mq1);
                                if (dt.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        mod1.Add(new SelectListItem { Text = dr["master_name"].ToString().ToUpper(), Value = dr["master_id"].ToString() });
                                    }
                                }
                                TempData[MyGuid + "_Tlist1"] = mod1;
                                #endregion
                                #region Currency Rate Reference

                                mq1 = "select master_id, master_name from master_setting where  type='CRS' " + model[0].Col11 + "";

                                dt = sgen.getdata(userCode, mq1);
                                if (dt.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        mod2.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                                    }
                                }
                                TempData[MyGuid + "_Tlist2"] = mod2;
                                #endregion
                                #region Applicable For

                                mq1 = "select master_id, master_name from master_setting where  type='CRA' ";

                                dt = sgen.getdata(userCode, mq1);
                                if (dt.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        mod3.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                                    }
                                }
                                TempData[MyGuid + "_Tlist3"] = mod3;
                                #endregion

                                model[0].TList1 = mod1;
                                model[0].TList2 = mod2;
                                model[0].TList3 = mod3;

                                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                                model[0].Col7 = dtt.Rows[0]["rec_id"].ToString();
                                model[0].Col8 = "(client_id|| client_unit_id ||master_id|| to_char(vch_date, 'yyyymmdd')|| type) = '" + URL + "'";
                                model[0].Col13 = "Update";
                                model[0].Col16 = dtt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dtt.Rows[0]["master_name"].ToString();
                                model[0].Col18 = dtt.Rows[0]["sectionid"].ToString();
                                model[0].Col19 = dtt.Rows[0]["subjectid"].ToString();
                                model[0].Col20 = dtt.Rows[0]["status"].ToString();
                                model[0].Col26 = dtt.Rows[0]["master_id"].ToString();
                                model[0].Col29 = dtt.Rows[0]["effdate"].ToString();
                                String[] L1 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["classid"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems1 = L1;
                                String[] L2 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["optional_subject"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems2 = L2;
                                String[] L3 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["teacher_category"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems3 = L3;
                            }
                            break;
                    }
                    break;
                #endregion
                #region bank_ac
                case "bank_ac":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select cm.C_name as designation,cm.ptype,cm.client_unit_id,cm.ploc,cm.ent_date,cm.client_id,cm.Panno,cs.country_name as cntry," +
                                "(cm2.client_id||cm2.client_unit_id||cm2.vch_num||to_char(cm2.vch_date, 'yyyymmdd')|| cm2.type) as ct_fstr," +
                                "cs.alpha_2 as country_id,cm.ent_by,cm.rec_id,cs.rec_id as cty_code,cm.remark as bnk_addr,cm.vch_num,cm.cpname as Bank_Acc," +
                                "cm.REFERED_BY as Branch_Name,nvl(a.master_name,'-') as Bank_name,cs.state_gst_code as state_id ,cm.bnk as fwd_link,cs.state_name,cs.district_name,cs.city_name," +
                                "cs.teh_name,cm.country,cm.State,cm.City,cm.bnkaddr as cp_Address,cm.msmeno as IFSC,cm.tanno as MICR,cm.cpdesig as Swift_code," +
                                "cm.cpaddr as Contect_person,cm.Cpaltcont as Mob_no,(case when cm.cpemail='0' then '' else cm.cpemail end) as Email,cm.Addr as Department,cm2.C_name as designation2,cm2.Addr as Department2," +
                                "cm2.Cpaltcont as Mob_no2,(case when cm2.cpemail='0' then '' else cm2.cpemail end) as Email2,cm2.cpaddr as Contect_person2,cm2.bnkaddr as cp_Address2 from clients_mst cm " +
                                "left join clients_mst cm2 on cm2.vch_num = cm.vch_num and cm2.type = 'BCP' and cm2.client_unit_id = cm.client_unit_id left join master_setting a on a.master_id = cm.ptype and a.type = 'ABD' and " +
                                "a.client_unit_id = cm.client_unit_id left join country_state cs on cs.rec_id = cm.city" +
                                " where (cm.client_id || cm.client_unit_id || cm.vch_num || to_char(cm.vch_date, 'yyyymmdd') || cm.type) = '" + URL + "'";

                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                #region ddls
                                TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1 = cmd_Fun.acctypevdm(userCode, unitid_mst);
                                TempData[MyGuid + "_TList2"] = model[0].TList2 = mod2 = cmd_Fun.curname(userCode, unitid_mst);
                                TempData[MyGuid + "_TList3"] = model[0].TList3 = mod3 = cmd_Fun.bank(userCode, unitid_mst);
                                #endregion
                                model[0].Col7 = dtt.Rows[0]["rec_id"].ToString();
                                model[0].Col8 = "client_id|| client_unit_id||vch_num|| To_Char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";
                                model[0].Col85 = "(client_id||client_unit_id||vch_num||to_char(vch_date, 'yyyymmdd')|| type) = '" + dtt.Rows[0]["ct_fstr"].ToString() + "'";
                                model[0].Col16 = dtt.Rows[0]["vch_num"].ToString();
                                model[0].Col18 = dtt.Rows[0]["Bank_Acc"].ToString();
                                model[0].Col19 = dtt.Rows[0]["IFSC"].ToString();
                                model[0].Col20 = dtt.Rows[0]["MICR"].ToString();
                                model[0].Col21 = dtt.Rows[0]["Swift_code"].ToString();
                                model[0].Col29 = dtt.Rows[0]["bnk_addr"].ToString();
                                model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                                model[0].Col7 = dtt.Rows[0]["rec_id"].ToString();
                                model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col28 = dtt.Rows[0]["Branch_Name"].ToString();
                                //model[0].Col17 = dtt.Rows[0]["Bank_name"].ToString();
                                //model[0].Col30 = dtt.Rows[0]["ptype"].ToString();
                                model[0].Col31 = dtt.Rows[0]["fwd_link"].ToString();
                                model[0].Col59 = dtt.Rows[0]["cntry"].ToString();
                                model[0].Col60 = dtt.Rows[0]["state_name"].ToString();
                                model[0].Col61 = dtt.Rows[0]["city_name"].ToString();
                                model[0].Col64 = dtt.Rows[0]["cty_code"].ToString();
                                model[0].Col49 = dtt.Rows[0]["country_id"].ToString();
                                model[0].Col71 = dtt.Rows[0]["state_id"].ToString();
                                model[0].Col13 = "Update";
                                model[0].Col100 = "Update & New";
                                model[0].Col121 = "U";
                                model[0].Col122 = "<u>U</u>pdate";
                                model[0].Col123 = "Update & Ne<u>w</u>";
                                String[] L1 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["ploc"].ToString()).Distinct()).Split(',');
                                String[] L2 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["Panno"].ToString()).Distinct()).Split(',');
                                String[] L3 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["ptype"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems1 = L1;
                                model[0].SelectedItems2 = L2;
                                model[0].SelectedItems3 = L3;
                                model[0].LTM2 = new List<Tmodelmain>();
                                for (int i = 0; i < dtt.Rows.Count; i++)
                                {
                                    Tmodelmain ltm2 = new Tmodelmain();
                                    if (i == 0)
                                    {
                                        ltm2.Col22 = dtt.Rows[0]["Contect_person"].ToString(); //
                                        ltm2.Col23 = dtt.Rows[0]["Mob_no"].ToString();  //
                                        ltm2.Col24 = dtt.Rows[0]["Email"].ToString();  //
                                        ltm2.Col25 = dtt.Rows[0]["Department"].ToString(); //
                                        ltm2.Col26 = dtt.Rows[0]["designation"].ToString(); //
                                        ltm2.Col27 = dtt.Rows[0]["cp_Address"].ToString(); //
                                    }
                                    else
                                    {
                                        ltm2.Col22 = dtt.Rows[i]["Contect_person2"].ToString(); //
                                        ltm2.Col23 = dtt.Rows[i]["Mob_no2"].ToString();  //
                                        ltm2.Col24 = dtt.Rows[i]["Email2"].ToString();  //
                                        ltm2.Col25 = dtt.Rows[i]["Department2"].ToString(); //
                                        ltm2.Col26 = dtt.Rows[i]["designation2"].ToString(); //
                                        ltm2.Col27 = dtt.Rows[i]["cp_Address2"].ToString(); //
                                    }
                                    model[0].LTM2.Add(ltm2);
                                }
                            }
                            break;
                        //case "BANK_NAME":
                        //    mq = "select a.master_name,master_id from master_setting a where a.client_id||a.client_unit_id||a.master_id|| to_char(a.vch_date,'yyyymmdd')||a.type='" + URL + "' ";
                        //    dt = sgen.getdata(userCode, mq);
                        //    {
                        //        model[0].Col17 = dt.Rows[0]["master_name"].ToString();
                        //        model[0].Col30 = dt.Rows[0]["master_id"].ToString();
                        //    }
                        //    break;
                        case "ADD1":
                            mq = "select * from country_state where alpha_2||state_gst_code ||city_name='" + URL + "'";
                            DataTable dt3 = new DataTable();
                            dt3 = sgen.getdata(userCode, mq);
                            if (dt3.Rows.Count > 0)
                            {
                                model[0].Col59 = dt3.Rows[0]["country_name"].ToString();
                                model[0].Col60 = dt3.Rows[0]["state_name"].ToString();
                                model[0].Col61 = dt3.Rows[0]["city_name"].ToString();
                                //model[0].Col26 = dt1.Rows[0]["city_name"].ToString();
                                //model[0].Col62 = dt1.Rows[0]["teh_name"].ToString();
                                //model[0].Col63 = dt1.Rows[0]["v_name"].ToString();
                                model[0].Col64 = dt3.Rows[0]["rec_id"].ToString();
                                model[0].Col49 = dt3.Rows[0]["alpha_2"].ToString();
                                model[0].Col71 = dt3.Rows[0]["state_gst_code"].ToString();
                            }
                            break;
                    }
                    break;
                #endregion
                #region  ledger_mst
                case "lgr_mst":
                    q1 = "SELECT client_id||client_unit_id||vch_num||TO_CHAR(vch_date, 'YYYYMMDD')|| type as fstr,master_type as Financial_Category_Code , master_name AS Financial_Category,'-' Main_Head,'-' as Sub_Head,'-' as Sub_Sub_Head FROM master_setting where type = 'AGM' AND TRIM(CLIENT_ID)= '-' AND TRIM(CLIENT_UNIT_ID)= '-'";
                    q2 = "SELECT  (a.client_id||a.client_unit_id|| a.vch_num|| TO_CHAR(a.vch_date, 'YYYYMMDD')|| a.type) as fstr,a.master_type as Financial_Category_Code ,b.master_name as Financial_Category,a.master_name as Main_Head,'-' as Sub_Head,'-' as Sub_Sub_Head FROM master_setting a INNER JOIN master_setting b ON SUBSTR(a.master_type, 1, 1) = SUBSTR(b.master_type, 1, 1)  AND TO_NUMBER(SUBSTR(b.master_type,2,7))= 0   and b.type = 'AGM'  where TO_NUMBER(SUBSTR(a.master_type,2,3))> 0 AND TO_NUMBER(SUBSTR(a.master_type,4,7))= 0 and a.type = 'AGM' and a.client_id='" + clientid_mst + "' and a.client_unit_id='" + unitid_mst + "'";
                    q3 = "SELECT (a.client_id|| a.client_unit_id|| a.vch_num|| TO_CHAR(a.vch_date, 'YYYYMMDD')|| a.type) as fstr,a.master_type as Financial_Category_Code ,b.master_name as Financial_Category ,c.master_name as Main_Head,a.master_name as Sub_Head,'-' as Sub_Sub_Head  FROM master_setting a INNER JOIN master_setting b ON SUBSTR(a.master_type, 1, 1) = SUBSTR(b.master_type, 1, 1)  AND TO_NUMBER(SUBSTR(b.master_type,2,7))= 0   and b.type = 'AGM'  INNER JOIN master_setting c ON SUBSTR(a.master_type,1,3)= SUBSTR(c.master_type, 1, 3)  AND TO_NUMBER(SUBSTR(c.master_type,4,7))= 0   and c.type = 'AGM'  where TO_NUMBER(SUBSTR(a.master_type,4,5))> 0 AND TO_NUMBER(SUBSTR(a.master_type,6,7))= 0 and a.type = 'AGM' and a.client_id='" + clientid_mst + "' and a.client_unit_id='" + unitid_mst + "'  and a.client_id=c.client_id and  a.client_unit_id = c.client_unit_id";
                    q4 = "SELECT  (a.client_id|| a.client_unit_id|| a.vch_num|| TO_CHAR(a.vch_date, 'YYYYMMDD')|| a.type) as fstr,a.master_type as Financial_Category_Code ,b.master_name AS Financial_Category,c.master_name as Main_Head,d.master_name as Sub_Head,a.master_name as Sub_Sub_Head  FROM master_setting a INNER JOIN master_setting b ON SUBSTR(a.master_type, 1, 1) = SUBSTR(b.master_type, 1, 1)  AND TO_NUMBER(SUBSTR(b.master_type,2,7))= 0   and b.type = 'AGM'  INNER JOIN master_setting c ON SUBSTR(a.master_type,1,3)= SUBSTR(c.master_type, 1, 3)  AND TO_NUMBER(SUBSTR(c.master_type,4,7))= 0   and c.type = 'AGM'  INNER JOIN master_setting d ON SUBSTR(a.master_type,1,5)= SUBSTR(d.master_type, 1, 5)  AND TO_NUMBER(SUBSTR(d.master_type,6,7))= 0   and d.type = 'AGM'  where TO_NUMBER(SUBSTR(a.master_type,6,7))> 0  and a.type = 'AGM' and a.client_id='" + clientid_mst + "' and a.client_unit_id='" + unitid_mst + "'  and a.client_id=c.client_id and a.client_unit_id= c.client_unit_id and a.client_id=d.client_id and a.client_unit_id=d.client_unit_id";
                    switch (btnval.ToUpper())
                    {

                        case "EDIT":
                            model[0].Col8 = "(client_id|| client_unit_id|| vch_num|| TO_CHAR(vch_date, 'YYYYMMDD')|| type)='" + URL + "'";
                            model[0].Col13 = "Update";
                            model[0].Col100 = "Update & New";
                            isedit = true;
                            mq = q1 + " union   " + q2 + " union " + q3 + " union " + q4;
                            mq = "Select (b.vch_num|| TO_CHAR(b.vch_date, 'YYYYMMDD')|| type )as fstr," +
                                " a.Financial_Category_Code,a.Financial_Category,a.Main_Head,a.Sub_Head,a.Sub_Sub_Head" +
                                ",b.c_name as ledger_name,b.CPNAME as ledger_abb,b.vch_num as ledger_code,b.CP_MNAME as tally_name" +
                                ",b.CPCONT as dd1,b.CPALTCONT as dd2,b.client_id,b.client_unit_id,b.ent_by,b.ent_date,b.rec_id,b.ref_code as vch_num" +
                                " from (" + mq + ") a inner join CLIENTS_MST b on a.Financial_Category_Code=b.CPLANDNO" +
                                " where b.client_id='" + clientid_mst + "' and b.client_unit_id='" + unitid_mst + "' and b.type='LDG' and " +
                                "(b.vch_num|| TO_CHAR(b.vch_date, 'YYYYMMDD')|| b.type)='" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                #region Group type
                                DataTable dtdd1 = new DataTable();
                                dtdd1 = sgen.getdata(userCode, "select master_id,master_name from master_setting where type='TAC' and client_id = '" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
                                if (dtdd1.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dtdd1.Rows)
                                    {
                                        mod1.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                                    }
                                }
                                TempData[MyGuid + "_TList1"] = mod1;
                                #endregion
                                DataTable dtdd2 = new DataTable();
                                //dtdd2 = sgen.getdata(userCode, "select master_id,master_name from master_setting where type='ALM' and client_id = '" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
                                dtdd2 = sgen.getdata(userCode, "select substr(master_type,0,3) as master_id,master_name from master_setting where type='AGM' and client_id = '" + clientid_mst + "' and client_unit_id='" + unitid_mst + "' ");
                                if (dtdd2.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dtdd2.Rows)
                                    {
                                        mod2.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                                    }
                                }
                                TempData[MyGuid + "_TList2"] = mod2;
                                model[0].TList1 = mod1;
                                model[0].TList2 = mod2;
                                String[] L1 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["dd1"].ToString()).Distinct()).Split(',');
                                String[] L2 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["dd2"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems1 = L1;
                                model[0].SelectedItems2 = L2;
                                model[0].Col19 = dt.Rows[0]["Financial_Category"].ToString();
                                model[0].Col20 = dt.Rows[0]["Main_Head"].ToString();
                                model[0].Col21 = dt.Rows[0]["Sub_Head"].ToString();
                                model[0].Col22 = dt.Rows[0]["Sub_sub_Head"].ToString();
                                model[0].Col24 = dt.Rows[0]["Financial_Category_code"].ToString();
                                model[0].Col23 = dt.Rows[0]["Financial_Category_code"].ToString();
                                model[0].Col17 = dt.Rows[0]["ledger_name"].ToString();
                                model[0].Col18 = dt.Rows[0]["ledger_abb"].ToString();
                                model[0].Col25 = dt.Rows[0]["ledger_code"].ToString();
                                model[0].Col31 = dt.Rows[0]["tally_name"].ToString();
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col1 = dt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dt.Rows[0]["ent_date"].ToString();
                                model[0].Col7 = dt.Rows[0]["rec_id"].ToString();
                                model[0].Col16 = dt.Rows[0]["vch_num"].ToString();
                            }
                            break;
                        case "GRP":
                            mq = q1 + " union   " + q2 + " union " + q3 + " union " + q4;
                            mq = "select * from (" + mq + ")a where a.fstr='" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model[0].Col19 = dt.Rows[0]["Financial_Category"].ToString();
                                model[0].Col20 = dt.Rows[0]["Main_Head"].ToString();
                                model[0].Col21 = dt.Rows[0]["Sub_Head"].ToString();
                                model[0].Col22 = dt.Rows[0]["Sub_sub_Head"].ToString();
                                model[0].Col24 = dt.Rows[0]["Financial_Category_code"].ToString();
                                model[0].Col23 = dt.Rows[0]["Financial_Category_code"].ToString();
                            }
                            break;
                        case "EXT":
                            mq = q1 + " union   " + q2 + " union " + q3 + " union " + q4;
                            mq = "Select (b.client_id|| b.client_unit_id|| b.vch_num|| TO_CHAR(b.vch_date, 'YYYYMMDD')|| type )as fstr," +
                                " a.Financial_Category_Code,a.Financial_Category,a.Main_Head,a.Sub_Head,a.Sub_Sub_Head" +
                                ",b.c_name as ledger_name,b.CPNAME as ledger_abb,b.vch_num as ledger_code,b.CP_MNAME as tally_name" +
                                ",b.CPCONT as dd1,b.CPALTCONT as dd2,b.client_id,b.client_unit_id,b.ent_by,b.ent_date,b.rec_id,b.ref_code as vch_num" +
                                " from (" + mq + ") a inner join CLIENTS_MST b on a.Financial_Category_Code=b.CPLANDNO" +
                                " where b.client_id='" + clientid_mst + "' and b.client_unit_id='" + unitid_mst + "' and b.type='LDG' and " +
                                "(b.client_id|| b.client_unit_id|| b.vch_num|| TO_CHAR(b.vch_date, 'YYYYMMDD')|| b.type)='" + URL + "'";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "LGRDATA", URL.Trim());
                                #region Group type
                                DataTable dtdd1 = new DataTable();
                                dtdd1 = sgen.getdata(userCode, "select master_id,master_name from master_setting where type='TAC' and client_id = '" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
                                if (dtdd1.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dtdd1.Rows)
                                    {
                                        mod1.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                                    }
                                }
                                TempData[MyGuid + "_TList1"] = mod1;
                                #endregion
                                DataTable dtdd2 = new DataTable();
                                dtdd2 = sgen.getdata(userCode, "select master_id,master_name from master_setting where type='ALM' and client_id = '" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
                                if (dtdd2.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dtdd2.Rows)
                                    {
                                        mod2.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                                    }
                                }
                                #region TEMPDATA Tlist
                                TempData[MyGuid + "_Tlist1"] = mod1;
                                TempData[MyGuid + "_Tlist2"] = mod2;
                                model[0].TList1 = mod1;
                                model[0].TList2 = mod2;
                                #endregion
                                Make_query(viewName.ToLower(), "Select Unit", "UNIT", "2", "", "");
                                ViewBag.scripCall += "callFoo('Select Unit');";
                            }
                            break;
                        case "UNIT":
                            try
                            {
                                string currdate = sgen.server_datetime(userCode);
                                ent_date = currdate;
                                string[] uts = URL.Replace("','", ",").Split(',');
                                string[] mst_data = sgen.GetSession(MyGuid, "LGRDATA").ToString().Replace("','", ",").Split(',');
                                #region dtstr                                
                                #region Group type
                                DataTable dtdd1 = new DataTable();
                                dtdd1 = sgen.getdata(userCode, "select master_id,master_name from master_setting where type='TAC' and client_id = '" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
                                if (dtdd1.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dtdd1.Rows)
                                    {
                                        mod1.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                                    }
                                }
                                TempData[MyGuid + "_TList1"] = mod1;
                                #endregion
                                DataTable dtdd2 = new DataTable();
                                dtdd2 = sgen.getdata(userCode, "select master_id,master_name from master_setting where type='ALM' and client_id = '" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
                                if (dtdd2.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dtdd2.Rows)
                                    {
                                        mod2.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                                    }
                                }
                                #region TEMPDATA Tlist
                                TempData[MyGuid + "_Tlist1"] = mod1;
                                TempData[MyGuid + "_Tlist2"] = mod2;
                                model[0].TList1 = mod1;
                                model[0].TList2 = mod2;
                                #endregion
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

                                        cp.Add(clientid_mst);
                                        up.Add(unitid_mst);
                                        cp.Distinct();
                                        up.Distinct();

                                        mq = "update clients_mst set edit_by='" + userid_mst + "',edit_date=TO_DATE('" + currdate + "', 'YYYY-MM-DD HH24:MI:SS')," +
                                            "client_id='" + String.Join(",", cp) + "',client_unit_id='" + String.Join(",", up) + "' " +
                                            "where (client_id||client_unit_id|| vch_num|| TO_CHAR(vch_date, 'YYYYMMDD')|| type) ='" + mst + "'";
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
                #region sale voucher
                case "sl_voucher":
                    switch (btnval.ToUpper())
                    {
                        case "PRINT":
                            //string title = model[0].Col9;
                            mq = "select vu.vch_num as doc_num,vu.client_id,vu.client_unit_id,to_char(vu.vch_date, '" + sgen.Getsqldateformat() + "') doc_date,vu.type,vu.invno as invoice_no" +
                            ",to_char(vu.invdate, '" + sgen.Getsqldateformat() + "') as invoice_date,vu.totremark as remark ," +
                            "vu.acode as party_code,nvl(cl.c_name,'-') party_name,(case when vu.type='40' then 'SALES VOUCHERS' ELSE 'PURCHASE VOUCHERS' end) AS VOUTITLE,to_number(vu.cramount) cramount,to_number(vu.dramount) dramount from vouchers vu inner join clients_mst cl on cl.vch_num = vu.acode and find_in_set(vu.client_unit_id, cl.client_unit_id) = 1 " +
                            "where (vu.client_unit_id || vu.vch_num || to_char(vu.vch_date, 'yyyymmdd') || vu.type) = '" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                DataSet ds = new DataSet();
                                dt.TableName = "prepcur";
                                ds.Tables.Add(dt);
                                sgen.open_report_byDs_xml(userCode, ds, "voucher1", "Print Voucher");
                                ViewBag.scripCall += "showRptnew('Print Voucher');disableForm();";
                            }
                            else
                            {
                                ViewBag.scripCall = "showmsgJS(1, 'Data Not Exist', 2);";
                            }
                            break;
                        case "EDIT":
                            mq = "select vu.vch_num as doc_num,vu.client_id,vu.client_unit_id,vu.ent_by,vu.ent_date,vu.edit_by,vu.edit_date," +
                                "vu.rec_id,to_char(vu.vch_date, '" + sgen.Getsqldateformat() + "') doc_date,vu.type,vu.invno as invoice_no" +
                                ",to_char(vu.invdate, '" + sgen.Getsqldateformat() + "') as invoice_date,vu.totremark as remark ," +
                                "vu.acode as party_code,nvl(cl.c_name,'-') party_name,vu.cramount,vu.dramount from vouchers vu inner join clients_mst cl on cl.vch_num = vu.acode and find_in_set(vu.client_unit_id, cl.client_unit_id) = 1 " +
                                "where (vu.client_unit_id || vu.vch_num || to_char(vu.vch_date, 'yyyymmdd') || vu.type) = '" + URL + "' and (vu.cramount + vu.dramount) > 0";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                model = getedit(model, dtt, URL);
                                model[0].Col8 = "client_unit_id||vch_num|| to_char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";
                                model[0].Col12 = dtt.Rows[0]["type"].ToString();
                                model[0].Col16 = dtt.Rows[0]["doc_num"].ToString();
                                model[0].Col19 = dtt.Rows[0]["invoice_no"].ToString();
                                model[0].Col20 = dtt.Rows[0]["invoice_date"].ToString();
                                model[0].Col21 = dtt.Rows[0]["remark"].ToString();
                                model[0].Col17 = dtt.Rows[0]["doc_date"].ToString();
                                model[0].dt1 = model[0].dt1.Clone();
                                for (int i = 0; i < dtt.Rows.Count; i++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["SNo"] = i + 1;
                                    dr["Acode"] = dtt.Rows[i]["party_code"].ToString();
                                    dr["Aname"] = dtt.Rows[i]["party_name"].ToString();
                                    dr["CrAmount"] = dtt.Rows[i]["cramount"].ToString();
                                    dr["DrAmount"] = dtt.Rows[i]["dramount"].ToString();
                                    model[0].dt1.Rows.Add(dr);
                                }
                                model[0].dt1.AcceptChanges();
                            }
                            break;
                        case "PARTY":
                            //mq = "select * from (select a.vch_num||a.type as fstr,a.vch_num ,a.c_name as Client_name,a.addr as address,a.c_gstin as GSTIN," +
                            //    "(case when a.tor = 'R' then 'Regular' when a.tor='C' then 'Composition' else 'Not Registered' END) GSTType," +
                            //    "a.type_desc as Search_text from clients_mst a  where a.type = 'BCD' and substr(a.vch_num,0,3)='203' and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "' " +
                            //    "union all select a.vch_num || a.type as fstr,a.vch_num,ledger_name Client_name, ledger_abb address,to_char(group_code) GSTIN," +
                            //    "'-' GSTType,'-' Search_text from ledger_master a where a.type = 'ALM' and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "') " +
                            //    "a where a.fstr in ('" + URL + "')";
                            mq = "select (a.acode|| to_char(a.vch_date,'yyyymmdd')||a.type) as fstr,a.acode,a.aname as ledger_name from " +
                                "(select vch_num as acode,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'BCD' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                "union all select vch_num as acode ,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                "union all select cm.vch_num as acode,nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname,cm.CLIENT_UNIT_ID,cm.vch_date,cm.type from master_setting a inner join clients_mst cm on " +
                                "a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id where cm.type = 'BAD' and cm.client_unit_id = '" + unitid_mst + "') a where a.acode||To_Char(a.vch_date, 'yyyymmdd')|| a.type = '" + URL + "'";
                            dtt = sgen.getdata(userCode, mq);
                            if (model[0].dt1.Rows[0]["acode"].ToString().Trim().Equals("-")) model[0].dt1.Rows.RemoveAt(0);
                            if (dtt.Rows.Count > 0)
                            {
                                DataRow dr = model[0].dt1.NewRow();
                                dr[0] = dtt.Rows[0]["fstr"].ToString();
                                dr[1] = dtt.Rows[0]["acode"].ToString();
                                dr[2] = dtt.Rows[0]["ledger_name"].ToString();
                                dr[3] = "0";
                                dr[4] = "0";
                                model[0].dt1.Rows.Add(dr);
                            }
                            break;
                    }
                    //switch (btnval.ToUpper())
                    //{
                    //    case "EDIT":
                    //        mq = "select vu.vch_num as doc_num,vu.type as vtype,vu.client_id,vu.client_unit_id,vu.ent_by,vu.ent_date,vu.edit_by,vu.edit_date," +
                    //            "vu.rec_id,to_char(vu.vch_date, '" + sgen.Getsqldateformat() + "') doc_date,vu.type,vu.invno as invoice_no" +
                    //            ",to_char(vu.invdate, '" + sgen.Getsqldateformat() + "') as invoice_date,vu.totremark as remark ," +
                    //            "vu.acode as party_code,nvl(lg.ledger_name,'-') party_name,vu.cramount,vu.dramount " +
                    //            "from vouchers vu left join(select* from (Select (b.vch_num|| to_char(b.vch_date, 'YYYYMMDD') || type ) as fstr," +
                    //            "a.Financial_Category_Code,a.Financial_Category,a.Main_Head,a.Sub_Head,a.Sub_Sub_Head,b.c_name as ledger_name," +
                    //            "b.CPNAME as Ledger_Name_Abbrevation,b.vch_num as ledger_code,b.CP_MNAME as tally_name,b.ref_code as vch_num from (SELECT client_id || client_unit_id || vch_num || TO_CHAR(vch_date, 'YYYYMMDD') || type as fstr, master_type as Financial_Category_Code,master_name AS Financial_Category, '-' Main_Head, '-' as Sub_Head, '-' as Sub_Sub_Head FROM master_setting where type = 'AGM' AND TRIM(CLIENT_ID) = '-' AND TRIM(CLIENT_UNIT_ID) = '-' union   SELECT(a.client_id || a.client_unit_id || a.vch_num || TO_CHAR(a.vch_date,'YYYYMMDD') || a.type) as fstr, a.master_type as Financial_Category_Code, b.master_name as Financial_Category, a.master_name as Main_Head, '-' as Sub_Head, '-' as Sub_Sub_Head FROM master_setting a INNER JOIN master_setting b ON SUBSTR(a.master_type, 1, 1) = SUBSTR(b.master_type, 1, 1)  AND TO_NUMBER(SUBSTR(b.master_type, 2, 7)) = 0   and b.type = 'AGM'  where TO_NUMBER(SUBSTR(a.master_type, 2, 3)) > 0 AND TO_NUMBER(SUBSTR(a.master_type, 4, 7)) = 0 and a.type = 'AGM' and a.client_id = '001' and a.client_unit_id = '001001' union SELECT (a.client_id || a.client_unit_id || a.vch_num || TO_CHAR(a.vch_date, 'YYYYMMDD') || a.type) as fstr, a.master_type as Financial_Category_Code, b.master_name as Financial_Category, c.master_name as Main_Head, a.master_name as Sub_Head, '-' as Sub_Sub_Head  FROM master_setting a INNER JOIN master_setting b ON SUBSTR(a.master_type, 1, 1) = SUBSTR(b.master_type, 1, 1)  AND TO_NUMBER(SUBSTR(b.master_type, 2, 7)) = 0 and b.type = 'AGM'  INNER JOIN master_setting c ON SUBSTR(a.master_type, 1, 3) = SUBSTR(c.master_type, 1, 3)  AND TO_NUMBER(SUBSTR(c.master_type, 4, 7)) = 0   and c.type = 'AGM'  where TO_NUMBER(SUBSTR(a.master_type, 4, 5)) > 0 AND TO_NUMBER(SUBSTR(a.master_type, 6, 7)) = 0 and a.type = 'AGM' and a.client_id = '001' and a.client_unit_id = '001001'  and a.client_id = c.client_id and  a.client_unit_id = c.client_unit_id union SELECT(a.client_id || a.client_unit_id || a.vch_num || TO_CHAR(a.vch_date, 'YYYYMMDD') || a.type) as fstr, a.master_type as Financial_Category_Code, b.master_name AS Financial_Category, c.master_name as Main_Head, d.master_name as Sub_Head, a.master_name as Sub_Sub_Head  FROM master_setting a INNER JOIN master_setting b ON SUBSTR(a.master_type, 1, 1) = SUBSTR(b.master_type, 1, 1)  AND TO_NUMBER(SUBSTR(b.master_type, 2, 7)) = 0   and b.type = 'AGM'  INNER JOIN master_setting c ON SUBSTR(a.master_type, 1, 3) = SUBSTR(c.master_type, 1, 3)  AND TO_NUMBER(SUBSTR(c.master_type, 4, 7)) = 0   and c.type = 'AGM'  INNER JOIN master_setting d ON SUBSTR(a.master_type, 1, 5) = SUBSTR(d.master_type, 1, 5)  AND TO_NUMBER(SUBSTR(d.master_type, 6, 7)) = 0   and d.type = 'AGM'  where TO_NUMBER(SUBSTR(a.master_type, 6, 7)) > 0  and a.type = 'AGM' and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'  and a.client_id = c.client_id and a.client_unit_id = c.client_unit_id and a.client_id = d.client_id and a.client_unit_id = d.client_unit_id) a inner join clients_mst b on a.Financial_Category_Code = b.CPLANDNO where find_in_set(b.client_id,'" + clientid_mst + "')= 1 and find_in_set(client_unit_id,'" + unitid_mst + "')= 1 and type = 'LDG')) lg on lg.ledger_code = vu.acode where (vu.client_id || vu.client_unit_id || vu.vch_num || to_char(vu.vch_date, 'yyyymmdd') || vu.type) = '" + URL + "'";

                    //        dtt = sgen.getdata(userCode, mq);
                    //        if (dtt.Rows.Count > 0)
                    //        {
                    //            model = getedit(model, dtt, URL);
                    //            model[0].Col14 = model[0].Col14;
                    //            model[0].Col15 = model[0].Col15;
                    //            model[0].Col16 = dtt.Rows[0]["doc_num"].ToString();
                    //            model[0].Col19 = dtt.Rows[0]["invoice_no"].ToString();
                    //            model[0].Col20 = dtt.Rows[0]["invoice_date"].ToString();
                    //            model[0].Col21 = dtt.Rows[0]["remark"].ToString();
                    //            model[0].Col17 = dtt.Rows[0]["doc_date"].ToString();
                    //            model[0].Col12 = dtt.Rows[0]["vtype"].ToString();
                    //            model[0].dt1 = model[0].dt1.Clone();
                    //            for (int i = 0; i < dtt.Rows.Count; i++)
                    //            {
                    //                DataRow dr = model[0].dt1.NewRow();
                    //                dr["SNo"] = i + 1;
                    //                dr["Select_"] = dtt.Rows[i]["party_code"].ToString();
                    //                dr["Acode"] = dtt.Rows[i]["party_code"].ToString();
                    //                dr["Aname"] = dtt.Rows[i]["party_name"].ToString();
                    //                dr["CrAmount"] = dtt.Rows[i]["cramount"].ToString();
                    //                dr["DrAmount"] = dtt.Rows[i]["dramount"].ToString();
                    //                model[0].dt1.Rows.Add(dr);
                    //            }
                    //            model[0].dt1.AcceptChanges();
                    //        }
                    //        break;

                    //    case "PARTY":

                    //        mq = "select * from (select a.vch_num||a.type as fstr,a.vch_num ,a.c_name as Client_name,a.addr as address,a.c_gstin as GSTIN," +
                    //            "(case when a.tor = 'R' then 'Regular' when a.tor='C' then 'Composition' else 'Not Registered' END) GSTType," +
                    //            "a.type_desc as Search_text from clients_mst a  where a.type = 'BCD' and substr(a.vch_num,0,3)='203' and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "' " +
                    //            "union all select a.vch_num || a.type as fstr,a.vch_num,ledger_name Client_name, ledger_abb address,to_char(group_code) GSTIN," +
                    //            "'-' GSTType,'-' Search_text from ledger_master a where a.type = 'ALM' and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "') " +
                    //            "a where a.fstr in ('" + URL + "')";
                    //        dtt = sgen.getdata(userCode, mq);
                    //        if (model[0].dt1.Rows[0]["acode"].ToString().Trim().Equals("-"))
                    //        {
                    //            model[0].dt1.Rows.RemoveAt(0);
                    //        }
                    //        //if (dtt.Rows.Count > 0)
                    //        //{
                    //        //    DataRow dr = model[0].dt1.NewRow();
                    //        //    dr[0] = dtt.Rows[0]["fstr"].ToString();
                    //        //    dr[1] = i + 1;
                    //        //    dr[1] = dtt.Rows[0]["vch_num"].ToString();
                    //        //    dr[2] = dtt.Rows[0]["Client_name"].ToString();
                    //        //    dr[3] = "0";
                    //        //    dr[4] = "0";
                    //        //    dr[5] = "0";
                    //        //    model[0].dt1.Rows.Add(dr);
                    //        //}
                    //        for (int i = 0; i < dtt.Rows.Count; i++)
                    //        {
                    //            DataRow dr = model[0].dt1.NewRow();
                    //            dr[0] = dtt.Rows[0]["fstr"].ToString();
                    //            dr[1] = i + 1;
                    //            dr[2] = dtt.Rows[0]["vch_num"].ToString();
                    //            dr[3] = dtt.Rows[0]["Client_name"].ToString();
                    //            dr[4] = "0";
                    //            dr[5] = "0";
                    //            model[0].dt1.Rows.Add(dr);
                    //        }
                    //        model[0].dt1.AcceptChanges();
                    //        break;
                    //}
                    break;
                #endregion
                #region bank reciept
                case "rcm":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select cm.*,to_char(cm.chqdate,'" + sgen.Getsqldateformat() + "') as chqdate1,to_char(cm.vch_date,'" + sgen.Getsqldateformat() + "') as vch_date1 from vouchers cm " +
                                "where (cm.client_unit_id|| cm.vch_num|| to_char(cm.vch_date, 'yyyymmdd')||cm.type ) = '" + URL + "' and cm.reftype = 'BNK'";
                            dt = new DataTable();
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model = getedit(model, dt, URL);
                                model[0].Col8 = "client_unit_id||vch_num|| to_char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";
                                mq1 = "select nvl(a.master_name,'-')||'('||cm.cpname||')'  as bank_name,cm.cpname as bank_ac,cm.vch_num as bankid from clients_mst cm " +
              "inner join master_setting a on a.master_id = cm.ptype and a.type = 'ABD' and find_in_set(a.client_unit_id, cm.client_unit_id)=1 " +
              "where cm.type = 'BAD' and find_in_set(cm.client_unit_id, '" + unitid_mst + "')=1";
                                DataTable bnk = new DataTable();
                                bnk = sgen.getdata(userCode, mq1);
                                if (bnk.Rows.Count > 0)
                                {
                                    foreach (DataRow item in bnk.Rows)
                                    {
                                        mod1.Add(new SelectListItem { Text = item["bank_name"].ToString(), Value = item["bankid"].ToString() });
                                    }
                                }
                                TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
                                mod2.Add(new SelectListItem { Text = "Advance", Value = "ADV" });
                                mod2.Add(new SelectListItem { Text = "Against Ref.", Value = "AGR" });
                                mod2.Add(new SelectListItem { Text = "New Ref.", Value = "NEW" });
                                mod2.Add(new SelectListItem { Text = "On Account", Value = "OAC" });
                                TempData[MyGuid + "_TList2"] = model[0].TList2 = mod2;
                                String[] L1 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["acode"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems1 = L1;
                                String[] L2 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["adj_type"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems2 = L2;
                                model[0].Col28 = dt.Rows[0]["chqno"].ToString();
                                model[0].Col16 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dt.Rows[0]["vch_date1"].ToString();
                                model[0].Col29 = dt.Rows[0]["chqdate1"].ToString();
                                model[0].Col26 = dt.Rows[0]["chqamt"].ToString();
                                model[0].Col40 = dt.Rows[0]["totremark"].ToString();
                            }
                            mq = "select cm.*,to_char(cm.chqdate,'" + sgen.Getsqldateformat() + "') as chqdate1 from vouchers cm " +
                              "where (cm.client_unit_id|| cm.vch_num|| to_char(cm.vch_date, 'yyyymmdd')||cm.type ) = '" + URL + "' and cm.reftype = 'ADV'";
                            dt = new DataTable();
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                //                                mq1 = "select (a.acode|| to_char(a.vch_date,'yyyymmdd')||a.type) as fstr,a.acode,a.aname as ledger_name,nvl(p.closing,'0') as closing from " +
                                //                                  "(select vch_num as acode,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'BCD' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                //                                  "union all select vch_num as acode ,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                //                                  "union all select cm.vch_num as acode,nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname,cm.CLIENT_UNIT_ID,cm.vch_date,cm.type from master_setting a inner join clients_mst cm on " +
                                //                                  "a.master_id = cm.ptype and a.type = 'ABD' and find_in_set(a.client_unit_id, cm.client_unit_id)=1 where cm.type = 'BAD' and find_in_set(cm.client_unit_id,'" + unitid_mst + "')=1) a " +
                                //                                  " LEFT JOIN(SELECT ACODE,CLIENT_UNIT_ID,SUM(OPBAL) AS OPBAL,SUM(DRAMOUNT),SUM(CRAMOUNT),SUM(OPBAL)+SUM(DRAMOUNT)-SUM(CRAMOUNT) as closing from (select acode,CLIENT_UNIT_ID, OP_" + year + " as opbal,0 as cramount,0 as dramount,0 as cl from acbal union all select acode,CLIENT_UNIT_ID,sum(dramount)-sum(cramount) as opbal" +
                                //",0 as cramount,0 as dramount,0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and " +
                                //                                      "to_date('" + year_from.Split(' ')[0].Trim() + "','" + sgen.Getsqldateformat() + "')-1 group by acode,CLIENT_UNIT_ID union all select acode,CLIENT_UNIT_ID,0 as opbal,TO_NUMBER(cramount) cramount,TO_NUMBER(dramount) dramount,0 cl from vouchers " +
                                //                                      "where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                                //                                      "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + model[0].Col17.Trim() + "','" + sgen.Getsqldateformat() + "')) group by acode,CLIENT_UNIT_ID) p on p.acode=a.acode and p.client_unit_id=a.client_unit_id where find_in_set(a.client_unit_id, '" + unitid_mst + "')=1 and a.acode='" + dt.Rows[0]["acode"].ToString() + "'";
                                //                                DataTable party = new DataTable();
                                //                                party = sgen.getdata(userCode, mq1);
                                //                                if (party.Rows.Count > 0)
                                //                                {
                                //                                    model[0].Col23 = party.Rows[0]["acode"].ToString();
                                //                                    model[0].Col24 = party.Rows[0]["ledger_name"].ToString();
                                //                                    //model[0].Col25 = party.Rows[0]["address"].ToString();
                                //                                    model[0].Col42 = party.Rows[0]["closing"].ToString();
                                //                                    //model[0].Col42 = sgen.seekval(userCode, "select sum(to_number(cramount))-sum(to_number(dramount)) as bal_amt from vouchers where type='11' and acode='" + model[0].Col23 + "' and reftype='adv' and type1='BLKRCPT' group by acode", "bal_amt");
                                //                                }
                                mq1 = "select a.acode,a.aname||'-'||'('||nvl(p.closing,'0')||')' as ledger_name from " +
                                                            "(select vch_num as acode,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'BCD' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                                            "union all select vch_num as acode ,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                                            "union all select cm.vch_num as acode,nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname,cm.CLIENT_UNIT_ID,cm.vch_date,cm.type from master_setting a inner join clients_mst cm on " +
                                                            "a.master_id = cm.ptype and a.type = 'ABD' and find_in_set(a.client_unit_id, cm.client_unit_id)=1 where cm.type = 'BAD' and find_in_set(cm.client_unit_id,'" + unitid_mst + "')=1) a " +
                                                            "LEFT JOIN (SELECT ACODE,CLIENT_UNIT_ID,SUM(OPBAL) AS OPBAL,SUM(DRAMOUNT),SUM(CRAMOUNT),SUM(OPBAL)+SUM(DRAMOUNT)-SUM(CRAMOUNT) as closing from " +
                                                            "(select acode,CLIENT_UNIT_ID, OP_" + year + " as opbal,0 as cramount,0 as dramount,0 as cl from acbal union all select acode,CLIENT_UNIT_ID,sum(dramount)-sum(cramount) as opbal" +
                            ",0 as cramount,0 as dramount,0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and " +
                                                              "to_date('" + year_from.Split(' ')[0].Trim() + "','" + sgen.Getsqldateformat() + "')-1 group by acode,CLIENT_UNIT_ID union all select acode,CLIENT_UNIT_ID,0 as opbal,TO_NUMBER(cramount) cramount,TO_NUMBER(dramount) dramount,0 cl from vouchers " +
                                                              "where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                                                              "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + model[0].Col17.Trim() + "','" + sgen.Getsqldateformat() + "')) group by acode,CLIENT_UNIT_ID) p on p.acode=a.acode and p.client_unit_id=a.client_unit_id where find_in_set(a.client_unit_id, '" + unitid_mst + "')=1";
                                DataTable dtemp = new DataTable();
                                dtemp = sgen.getdata(userCode, mq1);
                                if (dtemp.Rows.Count > 0)
                                {
                                    foreach (DataRow item in dtemp.Rows)
                                    {
                                        mod3.Add(new SelectListItem { Text = item["ledger_name"].ToString(), Value = item["acode"].ToString() });
                                    }
                                }
                                TempData[MyGuid + "_TList3"] = model[0].TList3 = mod3;
                                String[] L3 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["acode"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems3 = L3;
                                model[0].Col28 = dt.Rows[0]["chqno"].ToString();
                                model[0].Col29 = dt.Rows[0]["chqdate1"].ToString();
                                model[0].Col26 = dt.Rows[0]["chqamt"].ToString();
                                model[0].Col40 = dt.Rows[0]["totremark"].ToString();
                                model[0].Col27 = dt.Rows[0]["cramount"].ToString();
                            }
                            mq = "select cm.*,to_char(cm.invdate,'" + sgen.Getsqldateformat() + "') as invdate1 from vouchers cm " +
                          "where (cm.client_unit_id|| cm.vch_num|| to_char(cm.vch_date, 'yyyymmdd')||cm.type ) = '" + URL + "' and cm.reftype = 'AGR'";
                            dt = new DataTable();
                            var tm = model.FirstOrDefault();
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model = new List<Tmodelmain>();
                                foreach (DataRow dr in dt.Rows)
                                {
                                    model.Add(new Tmodelmain
                                    {
                                        Col3 = tm.Col3,
                                        Col4 = tm.Col4,
                                        Col8 = tm.Col8,
                                        Col9 = tm.Col9,
                                        Col11 = tm.Col11,
                                        Col12 = tm.Col12,
                                        Col10 = tm.Col10,
                                        Col13 = tm.Col13,
                                        Col14 = tm.Col14,
                                        Col15 = tm.Col15,
                                        Col16 = tm.Col16,
                                        Col17 = tm.Col17,
                                        Col18 = tm.Col18,
                                        Col19 = tm.Col19,
                                        Col20 = tm.Col20,
                                        Col21 = tm.Col21,
                                        Col22 = tm.Col22,
                                        Col23 = tm.Col23,
                                        Col24 = tm.Col24,
                                        Col42 = tm.Col42,
                                        Col25 = tm.Col25,
                                        Col26 = tm.Col26,
                                        Col27 = tm.Col27,
                                        Col28 = tm.Col28,
                                        Col29 = tm.Col29,
                                        Col40 = tm.Col40,
                                        Col100 = tm.Col100,
                                        Col121 = tm.Col121,
                                        Col122 = tm.Col122,
                                        Col123 = tm.Col123,
                                        Col30 = dr["invno"].ToString(),
                                        Col31 = dr["invdate1"].ToString(),
                                        Col32 = dr["invamt"].ToString(),
                                        Col34 = dr["remark"].ToString(),
                                        Col33 = dr["cramount"].ToString(),
                                        TList1 = tm.TList1,
                                        SelectedItems1 = tm.SelectedItems1,
                                        TList2 = tm.TList2,
                                        SelectedItems2 = tm.SelectedItems2,
                                        TList3 = tm.TList3,
                                        SelectedItems3 = tm.SelectedItems3,
                                    });
                                }
                            }
                            break;
                        case "BANK":
                            mq = "select nvl(a.master_name,'-') as bank_name,cm.vch_num,cm.cpname as Acc_No,cm.REFERED_BY as Branch_Name" +
                                ",cm.remark as bnk_Address,cm.msmeno as IFSC from clients_mst cm left join master_setting a on " +
                                "a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id where " +
                                "(cm.client_unit_id|| cm.vch_num|| to_char(cm.vch_date, 'yyyymmdd')||cm.type ) = '" + URL + "'";
                            dt = new DataTable();
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model[0].Col18 = dt.Rows[0]["Acc_No"].ToString();
                                model[0].Col19 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col20 = dt.Rows[0]["bank_name"].ToString();
                                model[0].Col21 = dt.Rows[0]["Branch_Name"].ToString();
                                model[0].Col22 = dt.Rows[0]["IFSC"].ToString();
                            }
                            break;
                        case "PRINT":
                            mq = "select vu.vch_num as doc_num,vu.client_id,vu.client_unit_id,to_char(vu.vch_date, '" + sgen.Getsqldateformat() + "') doc_date,vu.type,vu.invno as invoice_no" +
                            ",to_char(vu.invdate, '" + sgen.Getsqldateformat() + "') as invoice_date,vu.totremark as remark ," +
                            "vu.acode as party_code,vu.aname party_name,to_number(vu.cramount) cramount,(case when vu.type='11' then 'BANK RECIEPT' ELSE 'VOUCHER' end) AS VOUTITLE,to_number(vu.dramount) dramount from vouchers vu " +
                            "where (vu.client_unit_id || vu.vch_num || to_char(vu.vch_date, 'yyyymmdd') || vu.type) = '" + URL + "' and (vu.cramount + vu.dramount) > 0";
                            dt = sgen.getdata(userCode, mq);
                            model[0].TList1 = mod1;
                            model[0].TList2 = mod2;
                            model[0].TList3 = mod3;
                            if (dt.Rows.Count > 0)
                            {
                                DataSet ds = new DataSet();
                                dt.TableName = "prepcur";
                                ds.Tables.Add(dt);
                                sgen.open_report_byDs_xml(userCode, ds, "voucher2", "Print Voucher");
                                ViewBag.scripCall += "showRptnew('Print Voucher');disableForm();";
                            }
                            else
                            {
                                ViewBag.scripCall = "showmsgJS(1, 'Data Not Exist', 2);";
                            }
                            break;
                        case "PARTY":
                            //                            mq = "select a.c_name as name,a.c_gstin as gstin,a.addr as Address,a.vch_num,a.tor,nvl(ba.country_name,'-') as country_name," +
                            //"nvl(ba.state_name,'-') state_name,nvl(ba.city_name,'-') as city_name,a.pincode_2,nvl(p.closing,'0') as closing from clients_mst a " +
                            //"left join country_state ba on ba.rec_id = a.city2 LEFT JOIN(SELECT ACODE,CLIENT_UNIT_ID,SUM(OPBAL) AS OPBAL,SUM(DRAMOUNT),SUM(CRAMOUNT),SUM(OPBAL)+SUM(DRAMOUNT)-SUM(CRAMOUNT) as closing from (select acode,CLIENT_UNIT_ID, OP_" + year + " as opbal,0 as cramount,0 as dramount,0 as cl from acbal union all select acode,CLIENT_UNIT_ID,sum(dramount)-sum(cramount) as opbal" +
                            //",0 as cramount,0 as dramount,0 as cl from vouchers where type in ('11','10','21','20') and to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and " +
                            //                                      "to_date('" + year_from.Split(' ')[0].Trim() + "','" + sgen.Getsqldateformat() + "')-1 group by acode,CLIENT_UNIT_ID union all select acode,CLIENT_UNIT_ID,0 as opbal,TO_NUMBER(cramount) cramount,TO_NUMBER(dramount) dramount,0 cl from vouchers " +
                            //                                      "where type in ('11','10','21','20') and to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                            //                                      "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + model[0].Col17.Trim() + "','" + sgen.Getsqldateformat() + "')) group by acode,CLIENT_UNIT_ID) p on p.acode=a.vch_num and p.client_unit_id=a.client_unit_id where a.vch_num||To_Char(a.vch_date, 'yyyymmdd')|| a.type = '" + URL + "'";
                            mq = "select (a.acode|| to_char(a.vch_date,'yyyymmdd')||a.type) as fstr,a.acode,a.aname as ledger_name,nvl(p.closing,'0') as closing from " +
                                  "(select vch_num as acode,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'BCD' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                  "union all select vch_num as acode ,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                  "union all select cm.vch_num as acode,nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname,cm.CLIENT_UNIT_ID,cm.vch_date,cm.type from master_setting a inner join clients_mst cm on " +
                                  "a.master_id = cm.ptype and a.type = 'ABD' and find_in_set(a.client_unit_id, cm.client_unit_id)=1 where cm.type = 'BAD' and find_in_set(cm.client_unit_id,'" + unitid_mst + "')=1) a " +
                                  " LEFT JOIN(SELECT ACODE,CLIENT_UNIT_ID,SUM(OPBAL) AS OPBAL,SUM(DRAMOUNT),SUM(CRAMOUNT),SUM(OPBAL)+SUM(DRAMOUNT)-SUM(CRAMOUNT) as closing from (select acode,CLIENT_UNIT_ID, OP_" + year + " as opbal,0 as cramount,0 as dramount,0 as cl from acbal union all select acode,CLIENT_UNIT_ID,sum(dramount)-sum(cramount) as opbal" +
",0 as cramount,0 as dramount,0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and " +
                                      "to_date('" + year_from.Split(' ')[0].Trim() + "','" + sgen.Getsqldateformat() + "')-1 group by acode,CLIENT_UNIT_ID union all select acode,CLIENT_UNIT_ID,0 as opbal,TO_NUMBER(cramount) cramount,TO_NUMBER(dramount) dramount,0 cl from vouchers " +
                                      "where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                                      "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + model[0].Col17.Trim() + "','" + sgen.Getsqldateformat() + "')) group by acode,CLIENT_UNIT_ID) p on p.acode=a.acode and p.client_unit_id=a.client_unit_id where a.acode||To_Char(a.vch_date, 'yyyymmdd')|| a.type = '" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model[0].Col23 = dt.Rows[0]["acode"].ToString();
                                model[0].Col24 = dt.Rows[0]["ledger_name"].ToString();
                                //model[0].Col25 = dt.Rows[0]["address"].ToString();
                                model[0].Col42 = dt.Rows[0]["closing"].ToString();
                            }
                            break;
                    }
                    break;
                #endregion
                #region party_opening
                case "party_opening":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select i.vch_num,it.acode,it.aname,i.client_id,i.client_unit_id," +
                                "i.ent_date,i.edit_by,i.edit_date,i.ent_by,to_char(i.vch_date, '" + sgen.Getsqldateformat() + "') doc_date,i.loc," +
                                "i.OP_" + year + " as Opening_amount from acbal i " +
                                "inner join (select vch_num as acode,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'BCD' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                  "union all select vch_num as acode ,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                  "union all select cm.vch_num as acode,nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname,cm.CLIENT_UNIT_ID,cm.vch_date,cm.type from master_setting a inner join clients_mst cm on " +
                                  "a.master_id = cm.ptype and a.type = 'ABD' and find_in_set(a.client_unit_id, cm.client_unit_id)=1 where cm.type = 'BAD' and find_in_set(cm.client_unit_id,'" + unitid_mst + "')=1) it on it.acode = i.icode and find_in_set(it.client_unit_id, i.client_unit_id)= 1 " +
                                "where(i.client_unit_id || i.vch_num || to_char(i.vch_date, 'yyyymmdd') || i.type) = '" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                DataTable dt2 = new DataTable();
                                model[0].Col1 = dt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dt.Rows[0]["ent_date"].ToString();
                                model[0].Col5 = dt.Rows[0]["edit_by"].ToString();
                                model[0].Col6 = dt.Rows[0]["edit_date"].ToString();
                                model[0].Col8 = "(client_unit_id||vch_num|| to_char(vch_date, 'yyyymmdd')||type) = '" + URL + "'";
                                model[0].Col13 = "Update";
                                model[0].Col100 = "Update & New";
                                model[0].Col121 = "U";
                                model[0].Col122 = "<u>U</u>pdate";
                                model[0].Col123 = "Update & Ne<u>w</u>";
                                model[0].Col16 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dt.Rows[0]["doc_date"].ToString();
                                if (model[0].dt1.Rows[0]["acode"].ToString().Trim().Equals("-")) { model[0].dt1.Rows.RemoveAt(0); }
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["sno"] = i.ToString();
                                    dr["aname"] = dt.Rows[i]["aname"].ToString();
                                    dr["acode"] = dt.Rows[i]["acode"].ToString();
                                    dr["Opening_amount"] = dt.Rows[i]["Opening_amount"].ToString();
                                    model[0].dt1.Rows.Add(dr);
                                }
                            }
                            break;
                        case "ITEM":
                            mq = "select a.acode,a.aname as ledger_name from (select vch_num as acode,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'BCD' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                  "union all select vch_num as acode ,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                  "union all select cm.vch_num as acode,nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname,cm.CLIENT_UNIT_ID,cm.vch_date,cm.type from master_setting a inner join clients_mst cm on " +
                                  "a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id where cm.type = 'BAD' and cm.client_unit_id = '" + unitid_mst + "') a where (a.acode||to_char(a.vch_date, 'yyyymmdd')|| a.type) = '" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                if (model[0].dt1.Rows[0]["acode"].ToString().Trim().Equals("-")) { model[0].dt1.Rows.RemoveAt(0); }
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["Select_"] = dt.Rows[i]["acode"].ToString();
                                    dr["SNo"] = i + 1;
                                    dr["acode"] = dt.Rows[i]["acode"].ToString();
                                    dr["aname"] = dt.Rows[i]["ledger_name"].ToString();
                                    dr["opening_amount"] = "0";
                                    model[0].dt1.Rows.Add(dr);
                                }
                            }
                            break;
                    }
                    break;
                #endregion
                #region gst_rpt
                case "gst_rpt":
                    try
                    {
                        fdt = sgen.GetSession(MyGuid, "KPDFDT").ToString();
                        tdt = sgen.GetSession(MyGuid, "KPDTDT").ToString();
                    }
                    catch (Exception er) { }
                    switch (btnval.ToUpper())
                    {
                        case "PARTYLEDGDET":
                            string acode = sgen.seekval(userCode, "select a.vch_num as acode from clients_mst a where a.vch_num||To_Char(a.vch_date, 'yyyymmdd')|| a.type = '" + URL + "'", "acode");
                            if (acode != "0")
                            {
                                //mq1 = "select a.vch_num AS PARTY_CODE,a.c_name as PARTY_NAME,a.c_gstin as gstin,a.addr as Address,a.tor,nvl(ba.country_name,'-') as country_name,nvl(ba.state_name,'-') state_name,nvl(ba.city_name,'-') as city_name,p.vch_num as doc_no,nvl(to_char(p.vch_date,'dd/mm/yyyy'),'01/04/" + year + "') as doc_date,p.dr as dramount,p.cr as cramount,nvl(p.closing,'0') as closing,'" + fdt + "' as from_date,'" + tdt + "' as to_date,p.type as voutype from clients_mst a left join country_state ba on ba.rec_id = a.city2 " +
                                //    "LEFT JOIN(SELECT sn,VCH_NUM,VCH_DATE,CLIENT_UNIT_ID,TYPE,SUM(OPBAL) AS OPBAL,SUM(DRAMOUNT) as dr,SUM(CRAMOUNT) as cr,SUM(OPBAL)+SUM(DRAMOUNT)-SUM(CRAMOUNT) as closing from(select '0' sn,'0' rvscode,'0' VCH_NUM,NULL VCH_DATE,TYPE,CLIENT_UNIT_ID, OP_" + year + " as opbal,0 as cramount,0 as dramount,0 as cl from acbal WHERE ACODE='" + acode + "' union all " +
                                //    "select '1' sn,rvscode,'0' VCH_NUM,NULL VCH_DATE,TYPE,CLIENT_UNIT_ID,sum(dramount)-sum(cramount) as opbal,0 as cramount,0 as dramount,0 as cl from vouchers where ACODE='" + acode + "'" +
                                //    " and to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and " +
                                //      "to_date('" + fdt.Split(' ')[0].Trim() + "','" + sgen.Getsqldateformat() + "')-1 group by CLIENT_UNIT_ID,rvscode,VCH_NUM,VCH_DATE,TYPE union all select '1' sn,rvscode,VCH_NUM,VCH_DATE,TYPE,CLIENT_UNIT_ID,0 as opbal,TO_NUMBER(cramount) cramount,TO_NUMBER(dramount) dramount,0 cl from vouchers where ACODE='" + acode + "' " +
                                //    " and to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                                //      "to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') and to_date('" + tdt + "','" + sgen.Getsqldateformat() + "')) group by sn,CLIENT_UNIT_ID,rvscode,VCH_NUM,VCH_DATE,TYPE) p ON p.client_unit_id=a.client_unit_id where a.vch_num||To_Char(a.vch_date, 'yyyymmdd')|| a.type = '" + URL + "' order by p.sn";

                                mq1 = "select (v.client_unit_id|| v.vch_num|| to_char(v.vch_date, 'yyyymmdd')||v.type) as fstr,v.vch_num as doc_no,to_char(v.vch_date,'" + sgen.Getsqldateformat() + "') as doc_date,v.aname lgrname,v.acode lgrcode,v.rvscode othrlgrcode,c.c_name as othrldgrname,v.chqno,TO_NUMBER(v.dramount) as dramount" +
                                    ",TO_NUMBER(v.cramount) as cramount,'-' as closing,v.type,nvl(p.OPBAL,'0') as OPBAL,'" + fdt + "' as from_date,'" + tdt + "' as to_date from vouchers v INNER JOIN(select a.vch_num, a.c_name from(select vch_num, c_name, CLIENT_UNIT_ID, vch_date, type from clients_mst where type = 'BCD' and " +
                                    "find_in_set(client_unit_id, '" + unitid_mst + "') = 1 union all select vch_num as acode, c_name as aname, CLIENT_UNIT_ID, vch_date, type from clients_mst where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "') = 1 union all " +
                                    "select cm.vch_num as acode, nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname,cm.CLIENT_UNIT_ID, cm.vch_date, cm.type from master_setting a inner join clients_mst cm on a.master_id = cm.ptype and a.type = 'ABD' " +
                                    "and a.client_unit_id = cm.client_unit_id where cm.type = 'BAD' and cm.client_unit_id = '" + unitid_mst + "') a) c on c.vch_num = v.rvscode  left join " +
                                    "(SELECT ACODE, CLIENT_UNIT_ID, SUM(OPBAL) AS OPBAL, SUM(DRAMOUNT), SUM(CRAMOUNT), SUM(OPBAL)+SUM(DRAMOUNT) - SUM(CRAMOUNT) as closing from " +
                                    "(select acode, CLIENT_UNIT_ID, OP_" + year + " as opbal, 0 as cramount, 0 as dramount, 0 as cl from acbal union all select acode, CLIENT_UNIT_ID,sum(dramount) - sum(cramount) as opbal" +
                                    ", 0 as cramount, 0 as dramount, 0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and " +
                                      "to_date('" + fdt.Split(' ')[0].Trim() + "','" + sgen.Getsqldateformat() + "')-1 group by acode, CLIENT_UNIT_ID) GROUP BY ACODE,CLIENT_UNIT_ID) p on p.acode = v.acode and p.CLIENT_UNIT_ID = v.client_unit_id " +
                                      "where v.acode = '" + acode + "' and v.client_unit_id = '" + unitid_mst + "' and(to_number(v.dramount) + to_number(v.cramount)) > 0 and to_date(to_char(v.vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                                      "to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') and to_date('" + tdt + "','" + sgen.Getsqldateformat() + "') order by v.vch_date,v.vch_num,v.type";
                                dt = sgen.getdata(userCode, mq1);
                                if (dt.Rows.Count > 0)
                                {
                                    dt.TableName = "prepcur";
                                    if (dt.Rows.Count > 0) { sgen.open_report_bydt_xml(userCode, dt, "party_ldgr2", "Party Ledger"); }
                                    ViewBag.scripCall += "showRptnew('Party Ledger');";
                                }
                            }
                            break;
                        case "TBDETAIL":
                        case "DBDETAIL":
                        case "SRDETAIL":
                        case "PRDETAIL":
                        case "JRDETAIL":
                        case "PAYDETAIL":
                        case "RRDETAIL":
                        case "CRDETAIL":
                            if (btnval.Trim().ToUpper() == "TBDETAIL")
                            {
                                caption = "Detailed Trial Balance";
                                ttype = "TBDETAIL";
                            }
                            if (btnval.Trim().ToUpper() == "CFDETAIL")
                            {
                                caption = "Detailed Cash/Fund Flow";
                                ttype = "CFDETAIL";
                            }
                            else if (btnval.Trim().ToUpper() == "DBDETAIL")
                            {
                                caption = "Detailed Day Book";
                                ttype = "DBDETAIL";
                            }
                            else if (btnval.Trim().ToUpper() == "SRDETAIL")
                            {
                                caption = "Detailed Sales Register";
                                ttype = "SRDETAIL";
                            }
                            else if (btnval.Trim().ToUpper() == "PRDETAIL")
                            {
                                caption = "Detailed Purchase Register";
                                ttype = "PRDETAIL";
                            }
                            else if (btnval.Trim().ToUpper() == "JRDETAIL")
                            {
                                caption = "Detailed Journal Register";
                                ttype = "JRDETAIL";
                            }
                            else if (btnval.Trim().ToUpper() == "PAYDETAIL")
                            {
                                caption = "Detailed Payment Register";
                                ttype = "PAYDETAIL";
                            }
                            else if (btnval.Trim().ToUpper() == "RRDETAIL")
                            {
                                caption = "Detailed Receipt Register";
                                ttype = "RRDETAIL";
                            }
                            else if (btnval.Trim().ToUpper() == "CRDETAIL")
                            {
                                caption = "Detailed Credit Register";
                                ttype = "CRDETAIL";
                            }
                            Make_query(viewName, caption, btnval, "2", URL, fdt + "!~!~!~!~!" + tdt);
                            ViewBag.scripCall = "callFoo('" + caption + "');";
                            break;
                        case "BBOOK":
                        case "CBOOK":
                        case "SBOOK":
                        case "PBOOK":
                        case "JBOOK":
                            Make_query(viewName, "DETAILED VOUCHER", btnval, "2", URL, fdt + "!~!~!~!~!" + tdt);
                            ViewBag.scripCall = "callFoo('" + caption + "');";
                            break;

                            //case "DBVOU":                           
                            //    return RedirectToAction("sl_voucher", new { @m_id = EncryptDecrypt.Encrypt(model[0].Col15), @mid = EncryptDecrypt.Encrypt(model[0].Col14), @vid = EncryptDecrypt.Encrypt(URL) });
                            //    break;
                            //case "SRVOU":
                            //    return RedirectToAction("sl_voucher", new { @m_id = EncryptDecrypt.Encrypt(model[0].Col15), @mid = EncryptDecrypt.Encrypt(model[0].Col14), @vid = EncryptDecrypt.Encrypt(URL) });
                            //    break;
                            //case "PRVOU":
                            //    return RedirectToAction("sl_voucher", new { @m_id = EncryptDecrypt.Encrypt(model[0].Col15), @mid = EncryptDecrypt.Encrypt(model[0].Col14), @vid = EncryptDecrypt.Encrypt(URL) });
                            //    break;
                            //case "JRVOU":
                            //    return RedirectToAction("sl_voucher", new { @m_id = EncryptDecrypt.Encrypt(model[0].Col15), @mid = EncryptDecrypt.Encrypt(model[0].Col14), @vid = EncryptDecrypt.Encrypt(URL) });
                            //    break;
                            //case "PAYVOU":
                            //    return RedirectToAction("sl_voucher", new { @m_id = EncryptDecrypt.Encrypt(model[0].Col15), @mid = EncryptDecrypt.Encrypt(model[0].Col14), @vid = EncryptDecrypt.Encrypt(URL) });
                            //    break;
                            //case "RRVOU":
                            //    return RedirectToAction("sl_voucher", new { @m_id = EncryptDecrypt.Encrypt(model[0].Col15), @mid = EncryptDecrypt.Encrypt(model[0].Col14), @vid = EncryptDecrypt.Encrypt(URL) });
                            //    break;
                            //case "CRVOU":
                            //    return RedirectToAction("sl_voucher", new { @m_id = EncryptDecrypt.Encrypt(model[0].Col15), @mid = EncryptDecrypt.Encrypt(model[0].Col14), @vid = EncryptDecrypt.Encrypt(URL) });
                            //    break;                            
                    }
                    break;
                #endregion
                #region pay amount
                case "payamt":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select cm.*,to_char(cm.chqdate,'" + sgen.Getsqldateformat() + "') as chqdate1,to_char(cm.vch_date,'" + sgen.Getsqldateformat() + "') as vch_date1 from vouchers cm " +
                                "where (cm.client_unit_id|| cm.vch_num|| to_char(cm.vch_date, 'yyyymmdd')||cm.type ) = '" + URL + "' and cm.reftype = 'BNK'";
                            dt = new DataTable();
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model = getedit(model, dt, URL);
                                model[0].Col16 = dt.Rows[0]["vch_num"].ToString();

                                model[0].Col8 = "client_unit_id||vch_num|| to_char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";

                                mq1 = "select nvl(a.master_name,'-')||'('||cm.cpname||')'  as bank_name,cm.cpname as bank_ac,cm.vch_num as bankid from clients_mst cm " +
              "inner join master_setting a on a.master_id = cm.ptype and a.type = 'ABD' and find_in_set(a.client_unit_id,cm.client_unit_id)=1 " +
              "where cm.type = 'BAD' and find_in_set(cm.client_unit_id ,'" + unitid_mst + "')=1";
                                DataTable bnk = new DataTable();
                                bnk = sgen.getdata(userCode, mq1);
                                if (bnk.Rows.Count > 0)
                                {
                                    foreach (DataRow item in bnk.Rows)
                                    {
                                        mod1.Add(new SelectListItem { Text = item["bank_name"].ToString(), Value = item["bankid"].ToString() });
                                    }
                                }
                                TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
                                String[] L1 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["acode"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems1 = L1;
                                string from = sgen.seekval(userCode, "select col9 as fromval from enx_tab where type='CSR' and client_unit_id='" + unitid_mst + "' and col2 = '" + model[0].SelectedItems1.FirstOrDefault() + "'", "fromval");
                                string to = sgen.seekval(userCode, "select col10 as toval from enx_tab where type='CSR' and client_unit_id='" + unitid_mst + "' and col2 ='" + model[0].SelectedItems1.FirstOrDefault() + "'", "toval");
                                dtt = sgen.getdata(userCode, "SELECT R FROM (Select Rownum r From dual Connect By Rownum <= " + sgen.Make_int(to) + ") WHERE R BETWEEN " + sgen.Make_int(from) + " AND " + sgen.Make_int(to) + " AND to_char(R) NOT IN(select chqno from vouchers where type='21' and type1 = 'BLKPAY' and acode='" + model[0].SelectedItems1.FirstOrDefault() + "' and vch_num <> '" + model[0].Col16 + "')");
                                if (dtt.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dtt.Rows)
                                    {
                                        mod3.Add(new SelectListItem { Text = dr["R"].ToString(), Value = dr["R"].ToString() });
                                    }
                                }
                                TempData[MyGuid + "_TList3"] = model[0].TList3 = mod3;
                                mod2.Add(new SelectListItem { Text = "Advance", Value = "ADV" });
                                mod2.Add(new SelectListItem { Text = "Against Ref.", Value = "AGR" });
                                mod2.Add(new SelectListItem { Text = "New Ref.", Value = "NEW" });
                                mod2.Add(new SelectListItem { Text = "On Account", Value = "OAC" });
                                TempData[MyGuid + "_TList2"] = model[0].TList2 = mod2;
                                String[] L2 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["adj_type"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems2 = L2;
                                String[] L3 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["chqno"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems3 = L3;
                                //model[0].Col28 = dt.Rows[0]["chqno"].ToString();
                                model[0].Col17 = dt.Rows[0]["vch_date1"].ToString();
                                model[0].Col29 = dt.Rows[0]["chqdate1"].ToString();
                                model[0].Col26 = dt.Rows[0]["chqamt"].ToString();
                                model[0].Col40 = dt.Rows[0]["totremark"].ToString();
                            }
                            mq = "select cm.*,to_char(cm.chqdate,'" + sgen.Getsqldateformat() + "') as chqdate1 from vouchers cm " +
                              "where (cm.client_unit_id|| cm.vch_num|| to_char(cm.vch_date, 'yyyymmdd')||cm.type ) = '" + URL + "' and cm.reftype = 'ADV'";
                            dt = new DataTable();
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                mq1 = "select a.acode,a.aname||'-'||'('||nvl(p.closing,'0')||')' as ledger_name from " +
                                                     "(select vch_num as acode,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'BCD' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                                     "union all select vch_num as acode ,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                                     "union all select cm.vch_num as acode,nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname,cm.CLIENT_UNIT_ID,cm.vch_date,cm.type from master_setting a inner join clients_mst cm on " +
                                                     "a.master_id = cm.ptype and a.type = 'ABD' and find_in_set(a.client_unit_id, cm.client_unit_id)=1 where cm.type = 'BAD' and find_in_set(cm.client_unit_id,'" + unitid_mst + "')=1) a " +
                                                     "LEFT JOIN (SELECT ACODE,CLIENT_UNIT_ID,SUM(OPBAL) AS OPBAL,SUM(DRAMOUNT),SUM(CRAMOUNT),SUM(OPBAL)+SUM(DRAMOUNT)-SUM(CRAMOUNT) as closing from " +
                                                     "(select acode,CLIENT_UNIT_ID, OP_" + year + " as opbal,0 as cramount,0 as dramount,0 as cl from acbal union all select acode,CLIENT_UNIT_ID,sum(dramount)-sum(cramount) as opbal" +
                       ",0 as cramount,0 as dramount,0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and " +
                                                       "to_date('" + year_from.Split(' ')[0].Trim() + "','" + sgen.Getsqldateformat() + "')-1 group by acode,CLIENT_UNIT_ID union all select acode,CLIENT_UNIT_ID,0 as opbal,TO_NUMBER(cramount) cramount,TO_NUMBER(dramount) dramount,0 cl from vouchers " +
                                                       "where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                                                       "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + model[0].Col17.Trim() + "','" + sgen.Getsqldateformat() + "')) group by acode,CLIENT_UNIT_ID) p on p.acode=a.acode and p.client_unit_id=a.client_unit_id where find_in_set(a.client_unit_id, '" + unitid_mst + "')=1";
                                DataTable dtemp = new DataTable();
                                dtemp = sgen.getdata(userCode, mq1);
                                if (dtemp.Rows.Count > 0)
                                {
                                    foreach (DataRow item in dtemp.Rows)
                                    {
                                        mod4.Add(new SelectListItem { Text = item["ledger_name"].ToString(), Value = item["acode"].ToString() });
                                    }
                                }
                                TempData[MyGuid + "_TList4"] = model[0].TList4 = mod4;
                                String[] L4 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["acode"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems4 = L4;
                                //model[0].Col28 = dt.Rows[0]["chqno"].ToString();
                                model[0].Col29 = dt.Rows[0]["chqdate1"].ToString();
                                model[0].Col26 = dt.Rows[0]["chqamt"].ToString();
                                model[0].Col40 = dt.Rows[0]["totremark"].ToString();
                                model[0].Col27 = dt.Rows[0]["dramount"].ToString();
                            }
                            mq = "select cm.*,to_char(cm.invdate,'" + sgen.Getsqldateformat() + "') as invdate1 from vouchers cm " +
                          "where (cm.client_unit_id|| cm.vch_num|| to_char(cm.vch_date, 'yyyymmdd')||cm.type ) = '" + URL + "' and cm.reftype = 'BILL'";
                            dt = new DataTable();
                            var tm = model.FirstOrDefault();
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model = new List<Tmodelmain>();
                                foreach (DataRow dr in dt.Rows)
                                {
                                    model.Add(new Tmodelmain
                                    {
                                        Col3 = tm.Col3,
                                        Col4 = tm.Col4,
                                        Col8 = tm.Col8,
                                        Col9 = tm.Col9,
                                        Col11 = tm.Col11,
                                        Col12 = tm.Col12,
                                        Col10 = tm.Col10,
                                        Col13 = tm.Col13,
                                        Col14 = tm.Col14,
                                        Col15 = tm.Col15,
                                        Col16 = tm.Col16,
                                        Col17 = tm.Col17,
                                        Col18 = tm.Col18,
                                        Col19 = tm.Col19,
                                        Col20 = tm.Col20,
                                        Col21 = tm.Col21,
                                        Col22 = tm.Col22,
                                        Col23 = tm.Col23,
                                        Col24 = tm.Col24,
                                        Col42 = tm.Col42,
                                        Col25 = tm.Col25,
                                        Col26 = tm.Col26,
                                        Col27 = tm.Col27,
                                        Col28 = tm.Col28,
                                        Col29 = tm.Col29,
                                        Col40 = tm.Col40,
                                        Col100 = tm.Col100,
                                        Col121 = tm.Col121,
                                        Col122 = tm.Col122,
                                        Col123 = tm.Col123,
                                        Col30 = dr["invno"].ToString(),
                                        Col31 = dr["invdate1"].ToString(),
                                        Col32 = dr["invamt"].ToString(),
                                        Col34 = dr["remark"].ToString(),
                                        Col33 = dr["dramount"].ToString(),
                                        TList1 = tm.TList1,
                                        SelectedItems1 = tm.SelectedItems1,
                                        TList2 = tm.TList2,
                                        SelectedItems2 = tm.SelectedItems2,
                                        TList3 = tm.TList3,
                                        SelectedItems3 = tm.SelectedItems3,
                                        TList4 = tm.TList4,
                                        SelectedItems4 = tm.SelectedItems4,
                                    });
                                }
                            }
                            break;
                        case "BANK":
                            mq = "select nvl(a.master_name,'-') as bank_name,cm.vch_num,cm.cpname as Acc_No,cm.REFERED_BY as Branch_Name" +
                                ",cm.remark as bnk_Address,cm.msmeno as IFSC from clients_mst cm left join master_setting a on " +
                                "a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id where " +
                                "(cm.client_unit_id|| cm.vch_num|| to_char(cm.vch_date, 'yyyymmdd')||cm.type ) = '" + URL + "'";
                            dt = new DataTable();
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model[0].Col18 = dt.Rows[0]["Acc_No"].ToString();
                                model[0].Col19 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col20 = dt.Rows[0]["bank_name"].ToString();
                                model[0].Col21 = dt.Rows[0]["Branch_Name"].ToString();
                                model[0].Col22 = dt.Rows[0]["IFSC"].ToString();
                            }
                            break;
                        case "PRINT":
                            mq = "select vu.vch_num as doc_num,vu.client_id,vu.client_unit_id,to_char(vu.vch_date, '" + sgen.Getsqldateformat() + "') doc_date,vu.type,vu.invno as invoice_no" +
                            ",to_char(vu.invdate, '" + sgen.Getsqldateformat() + "') as invoice_date,vu.totremark as remark ," +
                            "vu.acode as party_code,vu.aname party_name,(case when vu.type='21' then 'BANK PAYMENT' ELSE 'VOUCHER' end) AS VOUTITLE,to_number(vu.cramount) cramount,to_number(vu.dramount) dramount from vouchers vu " +
                            "where (vu.client_unit_id || vu.vch_num || to_char(vu.vch_date, 'yyyymmdd') || vu.type) = '" + URL + "' and (vu.cramount + vu.dramount) > 0";
                            model[0].TList1 = mod1;
                            model[0].TList2 = mod2;
                            model[0].TList3 = mod3;
                            model[0].TList4 = mod4;
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                DataSet ds = new DataSet();
                                dt.TableName = "prepcur";
                                ds.Tables.Add(dt);
                                sgen.open_report_byDs_xml(userCode, ds, "voucher2", "Print Voucher");
                                ViewBag.scripCall += "showRptnew('Print Voucher');disableForm();";
                            }
                            else
                            {
                                ViewBag.scripCall = "showmsgJS(1, 'Data Not Exist', 2);";
                            }
                            break;
                        case "PARTY":
                            mq = "select (a.acode|| to_char(a.vch_date,'yyyymmdd')||a.type) as fstr,a.acode,a.aname as ledger_name,nvl(p.closing,'0') as closing from " +
                                  "(select vch_num as acode,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'BCD' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                  "union all select vch_num as acode ,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                  "union all select cm.vch_num as acode,nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname,cm.CLIENT_UNIT_ID,cm.vch_date,cm.type from master_setting a inner join clients_mst cm on " +
                                  "a.master_id = cm.ptype and a.type = 'ABD' and find_in_set(a.client_unit_id, cm.client_unit_id)=1 where cm.type = 'BAD' and find_in_set(cm.client_unit_id,'" + unitid_mst + "')=1) a " +
                                  " LEFT JOIN(SELECT ACODE,CLIENT_UNIT_ID,SUM(OPBAL) AS OPBAL,SUM(DRAMOUNT),SUM(CRAMOUNT),SUM(OPBAL)+SUM(DRAMOUNT)-SUM(CRAMOUNT) as closing from (select acode,CLIENT_UNIT_ID, OP_" + year + " as opbal,0 as cramount,0 as dramount,0 as cl from acbal union all select acode,CLIENT_UNIT_ID,sum(dramount)-sum(cramount) as opbal" +
",0 as cramount,0 as dramount,0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and " +
                                      "to_date('" + year_from.Split(' ')[0].Trim() + "','" + sgen.Getsqldateformat() + "')-1 group by acode,CLIENT_UNIT_ID union all select acode,CLIENT_UNIT_ID,0 as opbal,TO_NUMBER(cramount) cramount,TO_NUMBER(dramount) dramount,0 cl from vouchers " +
                                      "where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                                      "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + model[0].Col17.Trim() + "','" + sgen.Getsqldateformat() + "')) group by acode,CLIENT_UNIT_ID) p on p.acode=a.acode and p.client_unit_id=a.client_unit_id where a.acode||To_Char(a.vch_date, 'yyyymmdd')|| a.type = '" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model[0].Col23 = dt.Rows[0]["acode"].ToString();
                                model[0].Col24 = dt.Rows[0]["ledger_name"].ToString();
                                model[0].Col42 = dt.Rows[0]["closing"].ToString();
                            }
                            break;
                    }
                    break;
                #endregion
                #region cash transaction
                case "cash_vou":
                    switch (btnval.ToUpper())
                    {
                        case "PRINT":
                            mq = "select vu.vch_num as doc_num,vu.client_id,vu.client_unit_id,to_char(vu.vch_date, '" + sgen.Getsqldateformat() + "') doc_date,vu.type,vu.invno as invoice_no" +
                            ",to_char(vu.invdate, '" + sgen.Getsqldateformat() + "') as invoice_date,vu.totremark as remark ," +
                            "vu.acode as party_code,vu.aname party_name,(case when vu.type='20' then 'CASH PAYMENT' when vu.type='10' then 'CASH RECIEPT' ELSE 'VOUCHER' end) AS VOUTITLE,to_number(vu.cramount) cramount,to_number(vu.dramount) dramount from vouchers vu " +
                            "where (vu.client_unit_id || vu.vch_num || to_char(vu.vch_date, 'yyyymmdd') || vu.type) = '" + URL + "' and (vu.cramount + vu.dramount) > 0";
                            dt = sgen.getdata(userCode, mq);
                            model[0].TList1 = mod1;
                            model[0].TList2 = mod2;
                            model[0].TList3 = mod3;
                            if (dt.Rows.Count > 0)
                            {
                                DataSet ds = new DataSet();
                                dt.TableName = "prepcur";
                                ds.Tables.Add(dt);
                                sgen.open_report_byDs_xml(userCode, ds, "voucher2", "Print Voucher");
                                ViewBag.scripCall += "showRptnew('Print Voucher');disableForm();";
                            }
                            else
                            {
                                ViewBag.scripCall = "showmsgJS(1, 'Data Not Exist', 2);";
                            }
                            break;
                        case "EDIT":
                            mq = "select cm.*,to_char(cm.chqdate,'" + sgen.Getsqldateformat() + "') as chqdate1,to_char(cm.vch_date,'" + sgen.Getsqldateformat() + "') as vch_date1 from vouchers cm " +
                                "where (cm.client_unit_id|| cm.vch_num|| to_char(cm.vch_date, 'yyyymmdd')||cm.type ) = '" + URL + "' and cm.reftype = 'CSH'";
                            dt = new DataTable();
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model = getedit(model, dt, URL);
                                model[0].Col8 = "client_unit_id||vch_num|| to_char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";
                                model[0].Col16 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dt.Rows[0]["vch_date1"].ToString();
                                model[0].Col26 = dt.Rows[0]["chqamt"].ToString();
                                model[0].Col40 = dt.Rows[0]["totremark"].ToString();
                                mod2.Add(new SelectListItem { Text = "Advance", Value = "ADV" });
                                mod2.Add(new SelectListItem { Text = "Against Ref.", Value = "AGR" });
                                mod2.Add(new SelectListItem { Text = "New Ref.", Value = "NEW" });
                                mod2.Add(new SelectListItem { Text = "On Account", Value = "OAC" });
                                TempData[MyGuid + "_TList2"] = model[0].TList2 = mod2;
                                String[] L2 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["adj_type"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems2 = L2;
                            }
                            mq = "select cm.*,to_char(cm.chqdate,'" + sgen.Getsqldateformat() + "') as chqdate1 from vouchers cm " +
                              "where (cm.client_unit_id|| cm.vch_num|| to_char(cm.vch_date, 'yyyymmdd')||cm.type ) = '" + URL + "' and cm.reftype = 'ADV'";
                            dt = new DataTable();
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                mq1 = "select a.acode,a.aname||'-'||'('||nvl(p.closing,'0')||')' as ledger_name from " +
                                                                                      "(select vch_num as acode,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'BCD' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                                                                      "union all select vch_num as acode ,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                                                                      "union all select cm.vch_num as acode,nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname,cm.CLIENT_UNIT_ID,cm.vch_date,cm.type from master_setting a inner join clients_mst cm on " +
                                                                                      "a.master_id = cm.ptype and a.type = 'ABD' and find_in_set(a.client_unit_id, cm.client_unit_id)=1 where cm.type = 'BAD' and find_in_set(cm.client_unit_id,'" + unitid_mst + "')=1) a " +
                                                                                      "LEFT JOIN (SELECT ACODE,CLIENT_UNIT_ID,SUM(OPBAL) AS OPBAL,SUM(DRAMOUNT),SUM(CRAMOUNT),SUM(OPBAL)+SUM(DRAMOUNT)-SUM(CRAMOUNT) as closing from " +
                                                                                      "(select acode,CLIENT_UNIT_ID, OP_" + year + " as opbal,0 as cramount,0 as dramount,0 as cl from acbal union all select acode,CLIENT_UNIT_ID,sum(dramount)-sum(cramount) as opbal" +
                                                      ",0 as cramount,0 as dramount,0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and " +
                                                                                        "to_date('" + year_from.Split(' ')[0].Trim() + "','" + sgen.Getsqldateformat() + "')-1 group by acode,CLIENT_UNIT_ID union all select acode,CLIENT_UNIT_ID,0 as opbal,TO_NUMBER(cramount) cramount,TO_NUMBER(dramount) dramount,0 cl from vouchers " +
                                                                                        "where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                                                                                        "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + model[0].Col17.Trim() + "','" + sgen.Getsqldateformat() + "')) group by acode,CLIENT_UNIT_ID) p on p.acode=a.acode and p.client_unit_id=a.client_unit_id where find_in_set(a.client_unit_id, '" + unitid_mst + "')=1";
                                DataTable dtemp = new DataTable();
                                dtemp = sgen.getdata(userCode, mq1);
                                if (dtemp.Rows.Count > 0)
                                {
                                    foreach (DataRow item in dtemp.Rows)
                                    {
                                        mod3.Add(new SelectListItem { Text = item["ledger_name"].ToString(), Value = item["acode"].ToString() });
                                    }
                                }
                                TempData[MyGuid + "_TList3"] = model[0].TList3 = mod3;
                                String[] L3 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["acode"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems3 = L3;
                                model[0].Col26 = dt.Rows[0]["chqamt"].ToString();
                                model[0].Col40 = dt.Rows[0]["totremark"].ToString();
                                if (model[0].Col14.Trim().Equals("22003.1"))
                                {
                                    if (model[0].SelectedItems2.FirstOrDefault() == "ADV")
                                    {
                                        model[0].Col26 = dt.Rows[0]["cramount"].ToString();
                                    }
                                    else if (model[0].SelectedItems2.FirstOrDefault() == "OAC")
                                    {
                                        model[0].Col27 = dt.Rows[0]["cramount"].ToString();
                                    }
                                    else
                                    {
                                        model[0].Col27 = dt.Rows[0]["cramount"].ToString();
                                    }
                                }
                                else
                                {
                                    if (model[0].SelectedItems2.FirstOrDefault() == "ADV")
                                    {
                                        model[0].Col26 = dt.Rows[0]["dramount"].ToString();
                                    }
                                    else
                                    {
                                        model[0].Col27 = dt.Rows[0]["dramount"].ToString();
                                    }
                                }
                            }
                            mq = "select cm.*,to_char(cm.invdate,'" + sgen.Getsqldateformat() + "') as invdate1 from vouchers cm " +
                          "where (cm.client_unit_id|| cm.vch_num|| to_char(cm.vch_date, 'yyyymmdd')||cm.type ) = '" + URL + "' and cm.reftype = 'BILL'";
                            dt = new DataTable();
                            var tm = model.FirstOrDefault();
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model = new List<Tmodelmain>();
                                foreach (DataRow dr in dt.Rows)
                                {
                                    model.Add(new Tmodelmain
                                    {
                                        Col3 = tm.Col3,
                                        Col4 = tm.Col4,
                                        Col8 = tm.Col8,
                                        Col9 = tm.Col9,
                                        Col11 = tm.Col11,
                                        Col12 = tm.Col12,
                                        Col10 = tm.Col10,
                                        Col13 = tm.Col13,
                                        Col14 = tm.Col14,
                                        Col15 = tm.Col15,
                                        Col16 = tm.Col16,
                                        Col17 = tm.Col17,
                                        Col18 = tm.Col18,
                                        Col19 = tm.Col19,
                                        Col20 = tm.Col20,
                                        Col21 = tm.Col21,
                                        Col22 = tm.Col22,
                                        Col23 = tm.Col23,
                                        Col24 = tm.Col24,
                                        Col42 = tm.Col42,
                                        Col25 = tm.Col25,
                                        Col26 = tm.Col26,
                                        Col27 = tm.Col27,
                                        Col28 = tm.Col28,
                                        Col29 = tm.Col29,
                                        Col40 = tm.Col40,
                                        Col100 = tm.Col100,
                                        Col121 = tm.Col121,
                                        Col122 = tm.Col122,
                                        Col123 = tm.Col123,
                                        TList2 = tm.TList2,
                                        SelectedItems2 = tm.SelectedItems2,
                                        TList3 = tm.TList3,
                                        SelectedItems3 = tm.SelectedItems3,
                                        Col30 = dr["invno"].ToString(),
                                        Col31 = dr["invdate1"].ToString(),
                                        Col32 = dr["invamt"].ToString(),
                                        Col34 = dr["remark"].ToString(),
                                        Col33 = (tm.Col14 == "22003.1") ? dr["cramount"].ToString() : dr["dramount"].ToString(),
                                    });
                                }
                            }
                            break;
                        case "PARTY":
                            if (model[0].Col14 == "22003.1")
                            {
                                mq = "select a.aname,a.acode,nvl(p.closing,'0') as closing from (select vch_num as acode,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'BCD' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                  "union all select vch_num as acode ,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                  "union all select cm.vch_num as acode,nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname,cm.CLIENT_UNIT_ID,cm.vch_date,cm.type from master_setting a inner join clients_mst cm on " +
                                  "a.master_id = cm.ptype and a.type = 'ABD' and find_in_set(a.client_unit_id, cm.client_unit_id)=1 where cm.type = 'BAD' and find_in_set(cm.client_unit_id,'" + unitid_mst + "')=1) a LEFT JOIN(SELECT ACODE,CLIENT_UNIT_ID,SUM(OPBAL) AS OPBAL,SUM(DRAMOUNT),SUM(CRAMOUNT),SUM(OPBAL)+SUM(DRAMOUNT)-SUM(CRAMOUNT) as closing from (select acode,CLIENT_UNIT_ID, OP_" + year + " as opbal,0 as cramount,0 as dramount,0 as cl from acbal union all select acode,CLIENT_UNIT_ID,sum(dramount)-sum(cramount) as opbal" +
",0 as cramount,0 as dramount,0 as cl from vouchers where type in ('11','10','21','20') and to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and " +
                                      "to_date('" + year_from.Split(' ')[0].Trim() + "','" + sgen.Getsqldateformat() + "')-1 group by acode,CLIENT_UNIT_ID union all select acode,CLIENT_UNIT_ID,0 as opbal,TO_NUMBER(cramount) cramount,TO_NUMBER(dramount) dramount,0 cl from vouchers " +
                                      "where type in ('11','10','21','20') and to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                                      "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + model[0].Col17.Trim() + "','" + sgen.Getsqldateformat() + "')) group by acode,CLIENT_UNIT_ID) p on p.acode=a.acode and p.client_unit_id=a.client_unit_id where a.acode||To_Char(a.vch_date, 'yyyymmdd')|| a.type = '" + URL + "'";
                            }
                            else
                            {
                                mq = "select a.aname,a.acode,nvl(p.closing,'0') as closing from (select vch_num as acode,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'BCD' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                 "union all select vch_num as acode ,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                 "union all select cm.vch_num as acode,nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname,cm.CLIENT_UNIT_ID,cm.vch_date,cm.type from master_setting a inner join clients_mst cm on " +
                                 "a.master_id = cm.ptype and a.type = 'ABD' and find_in_set(a.client_unit_id, cm.client_unit_id)=1 where cm.type = 'BAD' and find_in_set(cm.client_unit_id,'" + unitid_mst + "')=1) a LEFT JOIN(SELECT ACODE,CLIENT_UNIT_ID,SUM(OPBAL) AS OPBAL,SUM(DRAMOUNT),SUM(CRAMOUNT),SUM(OPBAL)+SUM(DRAMOUNT)-SUM(CRAMOUNT) as closing from (select acode,CLIENT_UNIT_ID, OP_" + year + " as opbal,0 as cramount,0 as dramount,0 as cl from acbal union all select acode,CLIENT_UNIT_ID,sum(dramount)-sum(cramount) as opbal" +
",0 as cramount,0 as dramount,0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and " +
                                     "to_date('" + year_from.Split(' ')[0].Trim() + "','" + sgen.Getsqldateformat() + "')-1 group by acode,CLIENT_UNIT_ID union all select acode,CLIENT_UNIT_ID,0 as opbal,TO_NUMBER(cramount) cramount,TO_NUMBER(dramount) dramount,0 cl from vouchers " +
                                     "where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                                     "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + model[0].Col17.Trim() + "','" + sgen.Getsqldateformat() + "')) group by acode,CLIENT_UNIT_ID) p on p.acode=a.acode and p.client_unit_id=a.client_unit_id where  a.acode||To_Char(a.vch_date, 'yyyymmdd')|| a.type = '" + URL + "'";
                            }
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model[0].Col23 = dt.Rows[0]["acode"].ToString();
                                model[0].Col24 = dt.Rows[0]["aname"].ToString();
                                model[0].Col42 = dt.Rows[0]["closing"].ToString();
                            }
                            break;
                    }
                    break;
                #endregion
                #region tally_group mapping
                case "tly_gmap":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select i.vch_num,to_char(i.vch_date,'" + sgen.Getsqldateformat() + "') as docdate,i.col1,i.col2,i.col3" +
                                ",i.col4,i.client_id,i.client_unit_id,i.ent_by,i.ent_date,i.edit_by,i.edit_date from enx_tab i where (i.client_unit_id || i.vch_num || to_char(i.vch_date, 'yyyymmdd') || i.type) = '" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");

                                model[0].Col16 = dt.Rows[0]["vch_num"].ToString();
                                mod1.Add(new SelectListItem { Text = "DEBTORS", Value = "01" });
                                mod1.Add(new SelectListItem { Text = "CREDITORS", Value = "02" });
                                mod1.Add(new SelectListItem { Text = "BANK", Value = "03" });
                                mod1.Add(new SelectListItem { Text = "CASH", Value = "04" });
                                mod1.Add(new SelectListItem { Text = "SALE", Value = "05" });
                                mod1.Add(new SelectListItem { Text = "PURCHASE", Value = "06" });
                                TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
                                mq = "select a.vch_num as acode,a.c_name as aname from tally_clients_mst a where a.type='GRP' and a.vch_num not in (select a.vch_num as acode from tally_clients_mst a inner join (select col2 from enx_tab where type = 'TGM' and client_unit_id = '" + unitid_mst + "' and trim(vch_num) <> '" + model[0].Col16.Trim() + "') b on find_in_set(b.col2, a.vch_num) = 1 where a.type = 'GRP')";
                                dtt = sgen.getdata(userCode, mq);
                                if (dtt.Rows.Count > 0)
                                {
                                    foreach (DataRow item in dtt.Rows)
                                    {
                                        mod2.Add(new SelectListItem { Text = item["aname"].ToString(), Value = item["acode"].ToString() });
                                    }
                                }
                                TempData[MyGuid + "_TList2"] = model[0].TList2 = mod2;
                                String[] L1 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["col4"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems1 = L1;
                                String[] L2 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["col2"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems2 = L2;
                                model[0].Col1 = dt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dt.Rows[0]["ent_date"].ToString();
                                model[0].Col5 = dt.Rows[0]["edit_by"].ToString();
                                model[0].Col6 = dt.Rows[0]["edit_date"].ToString();
                                model[0].Col8 = "(client_unit_id||vch_num|| to_char(vch_date, 'yyyymmdd')||type) = '" + URL + "'";
                                model[0].Col13 = "Update";
                                model[0].Col100 = "Update & New";
                                model[0].Col121 = "U";
                                model[0].Col122 = "<u>U</u>pdate";
                                model[0].Col123 = "Update & Ne<u>w</u>";
                            }
                            break;
                    }
                    break;
                #endregion
                #region accReps
                case "accReps":
                    try
                    {
                        fdt = sgen.GetSession(MyGuid, "KPDFDT").ToString();
                        tdt = sgen.GetSession(MyGuid, "KPDTDT").ToString();
                    }
                    catch (Exception er) { }
                    switch (btnval.ToUpper())
                    {
                        case "ISUM":
                            Make_query(viewName, "Detailed Item Transactions", "IDET", "0", URL, fdt + "!~!~!~!~!" + tdt);
                            ViewBag.scripCall = "callFoo('Detailed Item Transactions');";
                            break;
                        case "IFIFO":
                            DataTable dtin = new DataTable();
                            DataTable dtout = new DataTable();
                            DataTable dtavg = new DataTable();
                            decimal qtyout = 0, qtyin = 0, cl = 0, bal = 0, rt = 0, totamt = 0, totqty = 0, myrate = 0, amt = 0;
                            mq1 = "select group_name evaltype from master_setting mg where md.subjectid='" + URL.Substring(0, 1).Trim() + "'";
                            mq1 = sgen.seekval(userCode, mq1, "evaltype");//evaluation type
                            if (mq1.Trim() == "0")
                            {
                                mq1 = "select master_name evaltype from master_setting where type='CF0' and client_unit_id='" + unitid_mst + "'";
                                mq1 = sgen.seekval(userCode, mq1, "evaltype");//evaluation type
                                if (mq1.Trim() == "0")
                                {
                                    ViewBag.scripCall += "mytoast('warning', 'toast-top-right', 'Please create master of inventory evaluation');";
                                    return model;
                                }
                            }
                            if (mq1.Trim() == "002")
                            {
                                mq = "select i.rec_id,i.vch_num,i.vch_date,i.type,i.icode,i.qtyin,i.qtyout,i.irate,i.iamount,'0' cl,'0' bal from itransaction i " +
                                    "where (it.icode||it.client_id||it.client_unit_id)='" + URL + "' and i.type in ('Q01','Q02','36','10','11','30','32','21','22') and i.store='Y' " +
                                    "order by i.vch_date,i.type asc"; //mrn_w_po,w/o_po,openg,return both,issue both,rgp,nrgp
                                dtavg = sgen.getdata(userCode, mq); //in
                                if (dtavg.Rows.Count <= 0)
                                {
                                    ViewBag.scripCall += "mytoast('warning', 'toast-top-right', 'No data found for IN OUT Stock');";
                                    return model;
                                }
                            }
                            else if (mq1.Trim() == "001")
                            {
                                mq = "select i.rec_id,i.vch_num,i.vch_date,i.type,i.icode,i.qtyin,i.qtyout,i.irate,i.iamount,'0' cl,'0' bal from itransaction i " +
                                    "where (i.icode||i.client_id||i.client_unit_id)='" + URL + "' and i.type in ('Q01','Q02','36') and i.store='Y' " +
                                    "order by i.vch_date,i.type asc"; //mrn_w_po,w/o_po,openg
                                dtin = sgen.getdata(userCode, mq); //in
                                if (dtin.Rows.Count <= 0)
                                {
                                    ViewBag.scripCall += "mytoast('warning', 'toast-top-right', 'No data found for IN Stock');";
                                    return model;
                                }
                                mq0 = "select i.rec_id,i.vch_num,i.vch_date,i.type,i.icode,i.qtyin,i.qtyout,i.irate,i.iamount,'0' cl,'0' bal from itransaction i " +
                                    "where (i.icode||i.client_id||i.client_unit_id) in ('" + URL + "') and type in ('10','11','30','32','21','22') and i.store='Y' " +
                                    "order by i.vch_date,i.type asc";//gen_ret,agnst_issue,w_req,w/o_req,rgp,nrgp
                                dtout = sgen.getdata(userCode, mq0); //out
                                if (dtout.Rows.Count <= 0)
                                {
                                    ViewBag.scripCall += "mytoast('warning', 'toast-top-right', 'No data found for OUT Stock');";
                                    return model;
                                }
                            }
                            //fifo
                            if (mq1.Trim() == "001")
                            {
                                List<string> recs = new List<string>();
                                mq = "UPDATE itransaction SET irate = CASE";
                                mq1 = "UPDATE itransaction SET iamount = CASE";
                                for (int o = 0; o < dtout.Rows.Count; o++) //out dt
                                {
                                    if (dtout.Rows[o]["type"].ToString().StartsWith("1"))
                                    {
                                        totamt = 0; totqty = 0;
                                        qtyin = sgen.Make_decimal(dtout.Rows[o]["qtyin"].ToString());
                                        for (int i = 0; i < dtout.Rows.Count; i++) //in dt
                                        {
                                            qtyout = sgen.Make_decimal(dtout.Rows[i]["qtyout"].ToString());
                                            cl = sgen.Make_decimal(dtout.Rows[i]["cl"].ToString());
                                            rt = sgen.Make_decimal(dtout.Rows[i]["irate"].ToString());
                                            bal = qtyout - cl;
                                            if (bal == qtyin)
                                            {
                                                totqty = totqty + qtyin;
                                                dtout.Rows[i]["cl"] = qtyout;//in bal
                                                totamt += rt * qtyin;
                                                qtyin = 0;
                                            }
                                            else if (bal > qtyin)
                                            {
                                                totqty += qtyin;
                                                dtout.Rows[i]["cl"] = cl + qtyin;//in bal
                                                totamt += rt * qtyin;
                                                qtyin = 0;
                                            }
                                            else if (bal < qtyin)
                                            {
                                                totqty += bal;
                                                dtout.Rows[i]["cl"] = cl + bal;//in bal
                                                totamt += rt * bal;
                                                qtyin = qtyin - bal;
                                            }
                                            if (qtyin == 0) break;
                                        }
                                        myrate = totamt / totqty;
                                        dtout.Rows[o]["irate"] = myrate;
                                        dtout.Rows[o]["iamount"] = totamt;//out amt
                                    }
                                    else
                                    {
                                        totamt = 0; totqty = 0;
                                        qtyout = sgen.Make_decimal(dtout.Rows[o]["qtyout"].ToString());
                                        for (int i = 0; i < dtin.Rows.Count; i++) //in dt
                                        {
                                            qtyin = sgen.Make_decimal(dtin.Rows[i]["qtyin"].ToString());
                                            cl = sgen.Make_decimal(dtin.Rows[i]["cl"].ToString());
                                            rt = sgen.Make_decimal(dtin.Rows[i]["irate"].ToString());
                                            bal = qtyin - cl;
                                            if (bal == qtyout)
                                            {
                                                totqty = totqty + qtyout;
                                                dtin.Rows[i]["cl"] = qtyin;//in bal
                                                totamt += rt * qtyout;
                                                qtyout = 0;
                                            }
                                            else if (bal > qtyout)
                                            {
                                                totqty += qtyout;
                                                dtin.Rows[i]["cl"] = cl + qtyout;//in bal
                                                totamt += rt * qtyout;
                                                qtyout = 0;
                                            }
                                            else if (bal < qtyout)
                                            {
                                                totqty += bal;
                                                dtin.Rows[i]["cl"] = cl + bal;//in bal
                                                totamt += rt * bal;
                                                qtyout = qtyout - bal;
                                            }
                                            if (qtyout == 0) break;
                                        }
                                        myrate = totamt / totqty;
                                        dtout.Rows[o]["irate"] = myrate;
                                        dtout.Rows[o]["iamount"] = totamt;//out amt
                                    }
                                    string rec = dtout.Rows[o]["recid"].ToString();
                                    recs.Add(rec);
                                    mq += " WHEN rec_id = '" + rec + "' THEN " + myrate + "";
                                    mq1 += " WHEN rec_id = '" + rec + "' THEN " + totamt + "";
                                }
                                mq += " where rec_id in (" + string.Join("','", recs) + "'";
                                mq1 += " where rec_id in (" + string.Join("','", recs) + "'";
                                sgen.execute_cmd(userCode, mq);
                                sgen.execute_cmd(userCode, mq1);
                            }
                            //average moving price
                            else if (mq1.Trim() == "002")
                            {
                                dtin = dtavg.Select("type in ('Q01','Q02','36')").CopyToDataTable();
                                dtout = dtavg.Select("type in ('10','11','30','32','21','22')").CopyToDataTable();
                                List<string> recs = new List<string>();
                                mq = "UPDATE itransaction SET irate = CASE";
                                mq1 = "UPDATE itransaction SET iamount = CASE";
                                for (int o = 0; o < dtout.Rows.Count; o++) //out dt
                                {
                                    var dtin_t = dtin.AsEnumerable().Where(w => w.Field<DateTime>("vch_date") <= ((DateTime)dtout.Rows[o]["vch_date"])).CopyToDataTable();
                                    DataTable dtttt = new DataTable();
                                    dtttt.Columns.Add("totqty");
                                    dtttt.Columns.Add("totamt");
                                    var t1 = dtin_t.AsEnumerable().GroupBy(k => new
                                    {
                                        icode = k.Field<string>("icode")
                                    });
                                    var t2 = t1.Select(c =>
                                    {
                                        var row = dtttt.NewRow();
                                        row["icode"] = c.Key.icode;
                                        row["totqty"] = c.Sum(k => k.Field<decimal>("qtyin"));
                                        row["totamt"] = c.Sum(k => k.Field<decimal>("iamount"));
                                        return row;
                                    });
                                    dtttt = t2.CopyToDataTable();
                                    totamt = 0; totqty = 0;
                                    qtyout = sgen.Make_decimal(dtout.Rows[o]["qtyout"].ToString());
                                    bal = sgen.Make_decimal(dtttt.Rows[0][0].ToString());
                                    amt = sgen.Make_decimal(dtttt.Rows[0][1].ToString());
                                    rt = amt / bal;
                                    //myrate = totamt / totqty;
                                    dtout.Rows[o]["irate"] = rt;
                                    dtout.Rows[o]["iamount"] = qtyout * rt;//out amt
                                    string rec = dtout.Rows[o]["recid"].ToString();
                                    recs.Add(rec);
                                    mq += " WHEN rec_id = '" + rec + "' THEN " + myrate + "";
                                    mq1 += " WHEN rec_id = '" + rec + "' THEN " + totamt + "";
                                }
                                mq += " where rec_id in (" + string.Join("','", recs) + "'";
                                mq1 += " where rec_id in (" + string.Join("','", recs) + "'";
                                sgen.execute_cmd(userCode, mq);
                                sgen.execute_cmd(userCode, mq1);
                            }
                            break;
                        case "ILEDGER":
                            mq = "select (t.icode||t.client_id||t.client_unit_id) fstr,t.vch_num,to_char(convert_tz(t.vch_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') vch_date," +
                                "t.client_id,t.client_unit_id,t.type,t.ewayno billno, to_char(convert_tz(t.ewaydate, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') billdate," +
                                "t.icode,i.iname,i.cpartno,um.master_name uom,to_number(t.irate) rate,to_number(t.iamount) amount,to_number(t.qtyin) Received,to_number(t.qtyout) Issued," +
                                "(case when t.type in ('36', '30', '32', '10', '11') then t.deptno when t.type in ('Q01', 'Q02', '21', '22') then t.acode else '-' end) partycode," +
                                "(case when t.type in ('36', '30', '32', '10', '11') then nvl(d.master_name, '-') when t.type in ('Q01', 'Q02', '21', '22') then vd.c_name else '-' end) partyname " +
                                "from itransaction t " +
                                "inner join item i on t.icode = i.icode and i.type = 'IT' and find_in_set(t.client_unit_id,i.client_unit_id)=1 " +
                                "left join master_setting um on um.master_id = i.uom and um.type = 'UMM' and find_in_set(um.client_unit_id, i.client_unit_id)= 1 " +
                                "left join master_setting d on d.master_id = t.deptno and d.Type = 'MDP' and find_in_set(d.client_unit_id, t.client_unit_id)= 1 " +
                                "left join clients_mst vd on t.acode = vd.vch_num and substr(vd.vch_num,0,3)='203' and vd.type = 'BCD' and find_in_set(t.client_unit_id,vd.client_unit_id)=1 " +
                                "where (t.icode || t.client_id || t.client_unit_id) in ('" + URL + "') and t.type in ('Q01', 'Q02', '36', '10', '11', '30', '32', '21', '22') " +
                                "and t.store = 'Y' order by t.vch_date,t.type asc";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0) { sgen.open_report_bydt_xml(userCode, dt, "iledger", "Item Ledger"); }
                            ViewBag.scripCall += "showRptnew('Item Ledger');";

                            //ViewBag.scripCall = "OpenSingle('../../../../../erp/schoolReports_Rpts/Filter_Report_xml.aspx','95%','900px','Reports');";
                            break;
                        case "PPO":
                            Make_query(viewName, "Detailed Pending PO", "PPODET", "0", URL, "");
                            ViewBag.scripCall = "callFoo('Detailed Pending PO');";
                            break;
                        case "STKMM":
                            mq = "select t.icode fstr,t.icode Item_Code,i.iname Item_Name,gd.master_name Maingrp,mg.master_name Itemgrp,sg.iname Itemsubgrp,t.vch_num DocNo," +
                                "to_char(t.vch_date,'" + sgen.Getsqldateformat() + "') Dated,i.cpartno PartNo,t.type Type,to_number(t.qtyin) Received," +
                                "to_number(t.qtyout) Issued,to_number(t.irate) Rate,to_number(t.iamount) Amount,nvl(d.master_name,'-') Department," +
                                "nvl(t.pktno,0) pktno from itransaction t " +
                              "inner join item i on t.icode=i.icode and i.type='IT' and find_in_set(i.client_unit_id,t.client_unit_id)=1 " +
                              "left join master_setting d on d.master_id=t.deptno and d.Type='MDP' and d.client_unit_id=t.client_unit_id " +
                              "inner join master_setting gd on gd.classid = SUBSTR(i.icode, 1, 1) and gd.type = 'KGP' " +
                              "inner join master_setting mg on mg.master_id = substr(i.icode, 1, 3) and mg.type = 'KIG' and find_in_set(mg.client_unit_id,i.client_unit_id)=1 " +
                              "inner join item sg on sg.icode = substr(i.icode, 1, 5) and sg.type = 'SG' and find_in_set(sg.client_unit_id,i.client_unit_id)=1 " +
                              "where concat(t.client_unit_id,t.icode) in ('" + URL + "') and t.store='Y' and to_date(to_char(t.vch_date,'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldateformat() + "') between " +
                              "to_date('" + fdt + "','" + sgen.Getsqldateformat() + "') and to_date('" + tdt + "','" + sgen.Getsqldateformat() + "') order by t.icode,t.vch_date asc";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0) { sgen.open_report_bydt_xml(userCode, dt, "stkmm", "Stock Moment Summary"); }
                            ViewBag.scripCall += "showRptnew('Stock Moment Summary');";

                            //ViewBag.scripCall = "OpenSingle('../../../../../erp/schoolReports_Rpts/Filter_Report_xml.aspx','95%','900px','Reports');";
                            break;
                    }
                    break;
                #endregion
                #region tally_comp mapping
                case "tly_cmap":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select i.vch_num,to_char(i.vch_date,'" + sgen.Getsqldateformat() + "') as docdate,i.col1,i.col2,i.col3" +
                                ",i.col4,i.client_id,i.client_unit_id,i.ent_by,i.ent_date,i.edit_by,i.edit_date from enx_tab i where (i.client_unit_id || i.vch_num || to_char(i.vch_date, 'yyyymmdd') || i.type) = '" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");

                                model[0].Col16 = dt.Rows[0]["vch_num"].ToString();
                                mq1 = "select a.vch_num as comid,a.company_name from company_profile a where a.type='CP' order by vch_num";
                                DataTable ddt = sgen.getdata(userCode, mq1);
                                if (ddt.Rows.Count > 0)
                                {
                                    foreach (DataRow item in ddt.Rows)
                                    {
                                        mod1.Add(new SelectListItem { Text = item["company_name"].ToString(), Value = item["comid"].ToString() });
                                    }
                                }
                                TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
                                mq1 = "select a.vch_num,a.col1 from enx_tab a where a.type='TCP' order by vch_num ";
                                dtt = sgen.getdata(userCode, mq1);
                                if (dtt.Rows.Count > 0)
                                {
                                    foreach (DataRow item in dtt.Rows)
                                    {
                                        mod2.Add(new SelectListItem { Text = item["col1"].ToString(), Value = item["vch_num"].ToString() });
                                    }
                                }
                                TempData[MyGuid + "_TList2"] = model[0].TList2 = mod2;
                                //mq = "select a.vch_num as acode,a.c_name as aname from tally_clients_mst a where a.type='GRP'" +
                                //    " and a.vch_num not in (select a.vch_num as acode from tally_clients_mst a inner " +
                                //    "join (" +
                                //    "select col2 from enx_tab where type = 'TGM' and client_unit_id = '" + unitid_mst + "' " +
                                //    "and trim(vch_num) <> '" + model[0].Col16.Trim() + "') b on find_in_set(b.col2, a.vch_num)" +
                                //    " = 1 where a.type = 'GRP')";
                                //dtt = sgen.getdata(userCode, mq);
                                //if (dtt.Rows.Count > 0)
                                //{
                                //    foreach (DataRow item in dtt.Rows)
                                //    {
                                //        mod2.Add(new SelectListItem { Text = item["aname"].ToString(), Value = item["acode"].ToString() });
                                //    }
                                //}
                                //TempData[MyGuid + "_TList2"] = model[0].TList2 = mod2;
                                String[] L1 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["col2"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems1 = L1;
                                String[] L2 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["col4"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems2 = L2;
                                model[0].Col1 = dt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dt.Rows[0]["ent_date"].ToString();
                                model[0].Col5 = dt.Rows[0]["edit_by"].ToString();
                                model[0].Col6 = dt.Rows[0]["edit_date"].ToString();
                                model[0].Col8 = "(client_unit_id||vch_num|| to_char(vch_date, 'yyyymmdd')||type) = '" + URL + "'";
                                model[0].Col13 = "Update";
                                model[0].Col100 = "Update & New";
                                model[0].Col121 = "U";
                                model[0].Col122 = "<u>U</u>pdate";
                                model[0].Col123 = "Update & Ne<u>w</u>";
                            }
                            break;
                    }
                    break;
                #endregion
                #region chq_srl
                case "chq_srl":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select i.vch_num,to_char(i.vch_date,'" + sgen.Getsqldateformat() + "') as docdate,i.col1,i.COL2,i.col9,i.col10,i.rec_id,i.client_id,i.client_unit_id,i.ent_by,i.ent_date,i.edit_by,i.edit_date from enx_tab i where (i.client_id||i.client_unit_id || i.vch_num || to_char(i.vch_date, 'yyyymmdd') || i.type) = '" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model = getedit(model, dt, URL);
                                model[0].Col16 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dt.Rows[0]["docdate"].ToString();
                                model[0].Col19 = dt.Rows[0]["col9"].ToString();
                                model[0].Col20 = dt.Rows[0]["col10"].ToString();
                                model[0].Col21 = dt.Rows[0]["col1"].ToString();
                                mq1 = "select nvl(a.master_name,'-')||'('||cm.cpname||')' as bank_name,cm.cpname as bank_ac,cm.vch_num as bankid from clients_mst cm " +
                   "inner join master_setting a on a.master_id = cm.ptype and a.type = 'ABD' and find_in_set(a.client_unit_id,cm.client_unit_id)=1 " +
                   "where cm.type = 'BAD' and find_in_set(cm.client_unit_id, '" + unitid_mst + "')=1";
                                dtt = new DataTable();
                                dtt = sgen.getdata(userCode, mq1);
                                if (dtt.Rows.Count > 0)
                                {
                                    foreach (DataRow item in dtt.Rows)
                                    {
                                        mod1.Add(new SelectListItem { Text = item["bank_name"].ToString(), Value = item["bankid"].ToString() });
                                    }
                                }
                                TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
                                String[] L1 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["col2"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems1 = L1;
                            }
                            break;
                    }
                    break;
                #endregion
                #region acc_link
                case "acc_link":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select i.vch_num,to_char(i.vch_date,'" + sgen.Getsqldateformat() + "') as docdate,i.col1,i.COL2,i.COL5,i.COL6,i.COL7,i.col9,i.col10,i.rec_id,i.client_id,i.client_unit_id,i.ent_by,i.ent_date,i.edit_by,i.edit_date from enx_tab i where (i.client_id||i.client_unit_id || i.vch_num || to_char(i.vch_date, 'yyyymmdd') || i.type) = '" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            dtt = sgen.getdata(userCode, "select master_id,master_name from master_setting where type='TAC' and client_id = '" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
                            if (dtt.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dtt.Rows)
                                {
                                    mod1.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                                }
                            }
                            TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
                            mod4.Add(new SelectListItem { Text = "IGST (PAYBLE)", Value = "P001" });
                            mod4.Add(new SelectListItem { Text = "SGST (PAYBLE)", Value = "P002" });
                            mod4.Add(new SelectListItem { Text = "CGST (PAYBLE)", Value = "P003" });
                            mod4.Add(new SelectListItem { Text = "IGST (RECIEVABLE)", Value = "P001" });
                            mod4.Add(new SelectListItem { Text = "SGST (RECIEVABLE)", Value = "P002" });
                            mod4.Add(new SelectListItem { Text = "CGST (RECIEVABLE)", Value = "P003" });
                            TempData[MyGuid + "_TList4"] = model[0].TList4 = mod4;
                            if (dt.Rows.Count > 0)
                            {
                                model = getedit(model, dt, URL);
                                model[0].Col16 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dt.Rows[0]["col1"].ToString();
                                String[] L1 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["col5"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems1 = L1;
                                String[] L4 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["col2"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems4 = L4;
                                dtt = sgen.getdata(userCode, "select substr(master_type,0,3) as master_id,master_name from master_setting where type='AGM' and client_id = '" + clientid_mst + "' and client_unit_id='" + unitid_mst + "' and substr(master_id,1,1)='" + model[0].SelectedItems1.FirstOrDefault() + "'");
                                if (dtt.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dtt.Rows)
                                    {
                                        mod2.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                                    }
                                }
                                TempData[MyGuid + "_TList2"] = model[0].TList2 = mod2;
                                String[] L2 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["col6"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems2 = L2;
                                mq = "select a.vch_num as Ledger_code,a.c_name as Ledger_Name from clients_mst a where " +
                                   "find_in_set(a.client_id,'" + clientid_mst + "')=1 and find_in_set(a.client_unit_id,'" + unitid_mst + "')=1 and substr(a.vch_num,0,3)='" + model[0].SelectedItems2.FirstOrDefault() + "' and type='LDG'";
                                dtt = sgen.getdata(userCode, mq);
                                if (dtt.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dtt.Rows)
                                    {
                                        mod3.Add(new SelectListItem { Text = dr["Ledger_Name"].ToString(), Value = dr["Ledger_code"].ToString() });
                                    }
                                }
                                TempData[MyGuid + "_TList3"] = model[0].TList3 = mod3;
                                String[] L3 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["col7"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems3 = L3;
                            }
                            break;
                    }
                    break;
                #endregion
                #region chq_rej
                case "chq_rej":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select i.vch_num,to_char(i.vch_date,'" + sgen.Getsqldateformat() + "') as docdate,i.col1,i.COL2,i.COL3 as Cheque_no,i.col9,i.col10,i.rec_id,i.client_id,i.client_unit_id,i.ent_by,i.ent_date,i.edit_by,i.edit_date from enx_tab i where (i.client_id||i.client_unit_id || i.vch_num || to_char(i.vch_date, 'yyyymmdd') || i.type) = '" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model = getedit(model, dt, URL);
                                model[0].Col16 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dt.Rows[0]["docdate"].ToString();
                                mq1 = "select nvl(a.master_name,'-')||'('||cm.cpname||')' as bank_name,cm.cpname as bank_ac,cm.vch_num as bankid from clients_mst cm " +
                   "inner join master_setting a on a.master_id = cm.ptype and a.type = 'ABD' and find_in_set(a.client_unit_id,cm.client_unit_id)=1 " +
                   "where cm.type = 'BAD' and find_in_set(cm.client_unit_id, '" + unitid_mst + "')=1";
                                dtt = new DataTable();
                                dtt = sgen.getdata(userCode, mq1);
                                if (dtt.Rows.Count > 0)
                                {
                                    foreach (DataRow item in dtt.Rows)
                                    {
                                        mod1.Add(new SelectListItem { Text = item["bank_name"].ToString(), Value = item["bankid"].ToString() });
                                    }
                                }
                                TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
                                String[] L1 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["col2"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems1 = L1;
                                model[0].dt1 = model[0].dt1.Clone();
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr[0] = dt.Rows[i]["Cheque_no"].ToString();
                                    dr[1] = i + 1;
                                    dr[2] = dt.Rows[i]["Cheque_no"].ToString();
                                    dr[3] = dt.Rows[i]["col1"].ToString();
                                    model[0].dt1.Rows.Add(dr);
                                }
                                model[0].dt1.AcceptChanges();
                            }
                            break;
                        case "CHEQUES":
                            string from = sgen.seekval(userCode, "select col9 as fromval from enx_tab where type='CSR' and client_unit_id='" + unitid_mst + "' and col2 = '" + model[0].SelectedItems1.FirstOrDefault() + "'", "fromval");
                            string to = sgen.seekval(userCode, "select col10 as toval from enx_tab where type='CSR' and client_unit_id='" + unitid_mst + "' and col2 ='" + model[0].SelectedItems1.FirstOrDefault() + "'", "toval");
                            mq = "SELECT R as fstr,R as Cheque_no FROM (Select Rownum r From dual Connect By Rownum <= " + sgen.Make_int(to) + ") WHERE R BETWEEN " +
                                "" + sgen.Make_int(from) + " AND " + sgen.Make_int(to) + " and R in ('" + URL + "')";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                if (model[0].dt1.Rows[0]["Cheque_no"].ToString().Trim().Equals("-")) model[0].dt1.Rows.RemoveAt(0);
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr[0] = dt.Rows[i]["fstr"].ToString();
                                    dr[1] = i + 1;
                                    dr[2] = dt.Rows[i]["Cheque_no"].ToString();
                                    model[0].dt1.Rows.Add(dr);
                                }
                                model[0].dt1.AcceptChanges();
                            }
                            break;
                    }
                    break;
                #endregion
                #region recil
                case "recil":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select cm.*,to_char(cm.date1,'" + sgen.Getsqldateformat() + "') as chqdate1,to_char(cm.date2,'yyyymmdd') as voudate,to_char(cm.vch_date,'" + sgen.Getsqldateformat() + "') as vch_date1 from enx_tab2 cm " +
                                " where (cm.client_unit_id|| cm.vch_num|| to_char(cm.vch_date, 'yyyymmdd')||cm.type ) = '" + URL + "'";
                            dt = new DataTable();
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model = getedit(model, dt, URL);
                                model[0].Col8 = "client_unit_id||vch_num|| to_char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";
                                model[0].Col16 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dt.Rows[0]["chqdate1"].ToString();
                                model[0].Col19 = dt.Rows[0]["col7"].ToString();
                                mq1 = "select nvl(a.master_name,'-')||'('||cm.cpname||')'  as bank_name,cm.cpname as bank_ac,cm.vch_num as bankid from clients_mst cm " +
         "inner join master_setting a on a.master_id = cm.ptype and a.type = 'ABD' and find_in_set(a.client_unit_id, cm.client_unit_id)=1 " +
         "where cm.type = 'BAD' and find_in_set(cm.client_unit_id, '" + unitid_mst + "')=1";
                                DataTable bnk = new DataTable();
                                bnk = sgen.getdata(userCode, mq1);
                                if (bnk.Rows.Count > 0)
                                {
                                    foreach (DataRow item in bnk.Rows)
                                    {
                                        mod1.Add(new SelectListItem { Text = item["bank_name"].ToString(), Value = item["bankid"].ToString() });
                                    }
                                }
                                TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
                                String[] L1 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["col5"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems1 = L1;

                                mq1 = "select p.closing from (SELECT ACODE,CLIENT_UNIT_ID,SUM(OPBAL) AS OPBAL,SUM(DRAMOUNT),SUM(CRAMOUNT),SUM(OPBAL)+SUM(DRAMOUNT)-SUM(CRAMOUNT) as closing from " +
                                                       "(select acode,CLIENT_UNIT_ID, OP_" + year + " as opbal,0 as cramount,0 as dramount,0 as cl from acbal union all select acode,CLIENT_UNIT_ID,sum(dramount)-sum(cramount) as opbal" +
                       ",0 as cramount,0 as dramount,0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and " +
                                                         "to_date('" + year_from.Split(' ')[0].Trim() + "','" + sgen.Getsqldateformat() + "')-1 group by acode,CLIENT_UNIT_ID union all select acode,CLIENT_UNIT_ID,0 as opbal,TO_NUMBER(cramount) cramount,TO_NUMBER(dramount) dramount,0 cl from vouchers " +
                                                         "where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                                                         "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + model[0].Col17 + "','" + sgen.Getsqldateformat() + "')) group by acode,CLIENT_UNIT_ID) p where p.acode='" + model[0].SelectedItems1.FirstOrDefault() + "'";
                                DataTable dtc = sgen.getdata(userCode, mq1);
                                if (dtc.Rows.Count > 0)
                                {
                                    model[0].Col21 = dtc.Rows[0]["closing"].ToString();
                                }
                                mq1 = "select (v.client_unit_id || v.vch_num || to_char(v.vch_date, 'yyyymmdd') || v.type) as fstr,v.type || '_' || v.vch_num as doc_no,to_char(v.vch_date, 'dd/MM/YYYY') as doc_date,c.c_name as party" +
                          ",v.chqno,to_char(v.bankdate,'" + sgen.Getsqldateformat() + "') as bkdate,TO_NUMBER(v.dramount) as dramount,TO_NUMBER(v.cramount) as cramount from vouchers v INNER JOIN(select a.vch_num, a.c_name from(select vch_num, c_name, " +
                          "CLIENT_UNIT_ID, vch_date, type from clients_mst where type = 'BCD' and find_in_set(client_unit_id, '" + unitid_mst + "') = 1 union all " +
                          "select vch_num as acode, c_name as aname, CLIENT_UNIT_ID, vch_date, type from clients_mst where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "') = 1" +
                          " union all " +
                          "select cm.vch_num as acode, nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname, cm.CLIENT_UNIT_ID, cm.vch_date, cm.type from master_setting a " +
                          "inner join clients_mst cm on a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id where cm.type = 'BAD' and cm.client_unit_id = '" + unitid_mst + "') a) c " +
                          "on c.vch_num = V.acode where v.acode = '" + model[0].SelectedItems1.FirstOrDefault() + "' and v.client_unit_id = '" + unitid_mst + "'" +
                          " and  v.type ||'_'||v.vch_num || to_char(v.vch_date, 'yyyymmdd') = '" + dt.Rows[0]["col22"].ToString() + dt.Rows[0]["voudate"].ToString() + "' and (to_number(v.dramount) + to_number(v.cramount)) > 0 order by v.vch_num desc";

                                //            mq1 = "select (v.client_unit_id|| v.vch_num|| to_char(v.vch_date, 'yyyymmdd')||v.type) as fstr,v.type||'_'||v.vch_num as doc_no,to_char(v.vch_date,'dd/MM/YYYY') as doc_date,c.c_name as party" +
                                //",v.chqno,to_char(v.bankdate,'" + sgen.Getsqldateformat() + "') as bkdate,TO_NUMBER(v.dramount) as dramount,TO_NUMBER(v.cramount) as cramount from vouchers v INNER JOIN(select a.vch_num, a.c_name from(select vch_num, c_name, " +
                                //"CLIENT_UNIT_ID, vch_date, type from clients_mst where type = 'BCD' and find_in_set(client_unit_id, '" + unitid_mst + "') = 1 union all " +
                                //"select vch_num as acode, c_name as aname, CLIENT_UNIT_ID, vch_date, type from clients_mst where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "') = 1" +
                                //" union all " +
                                //"select cm.vch_num as acode, nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname, cm.CLIENT_UNIT_ID, cm.vch_date, cm.type from master_setting a " +
                                //"inner join clients_mst cm on a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id where cm.type = 'BAD' and cm.client_unit_id = '" + unitid_mst + "') a) c " +
                                //"on c.vch_num = V.acode where v.acode = '" + model[0].SelectedItems1.FirstOrDefault() + "' and v.client_unit_id = '" + unitid_mst + "' and  v.type ||'_'||v.vch_num || to_char(v.vch_date, 'yyyymmdd') = '" + dt.Rows[0]["col22"].ToString() + dt.Rows[0]["voudate"].ToString() + "' and (to_number(v.dramount) + to_number(v.cramount)) > 0 and v.bankdate is not null or to_char(v.bankdate,'ddmmyyyy') != '01011900' order by v.vch_num desc";

                                var tm = model.FirstOrDefault();
                                dt = sgen.getdata(userCode, mq1);
                                if (dt.Rows.Count > 0)
                                {
                                    model = new List<Tmodelmain>();
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        model.Add(new Tmodelmain
                                        {
                                            Col30 = dr["doc_no"].ToString(),
                                            Col31 = dr["doc_date"].ToString(),
                                            Col32 = dr["chqno"].ToString(),
                                            Col33 = dr["dramount"].ToString(),
                                            Col34 = dr["cramount"].ToString(),
                                            Col35 = dr["bkdate"].ToString(),
                                            Col36 = dr["party"].ToString(),
                                            Col8 = tm.Col8,
                                            Col9 = tm.Col9,
                                            Col11 = tm.Col11,
                                            Col12 = tm.Col12,
                                            Col10 = tm.Col10,
                                            Col13 = tm.Col13,
                                            Col14 = tm.Col14,
                                            Col15 = tm.Col15,
                                            Col16 = tm.Col16,
                                            Col17 = tm.Col17,
                                            Col18 = tm.Col18,
                                            Col19 = tm.Col19,
                                            Col20 = tm.Col20,
                                            Col21 = tm.Col21,
                                            Col22 = tm.Col22,
                                            Col23 = tm.Col23,
                                            Col24 = tm.Col24,
                                            Col25 = tm.Col25,
                                            Col26 = tm.Col26,
                                            Chk1 = tm.Chk1,
                                            Chk2 = tm.Chk2,
                                            Col100 = tm.Col100,
                                            Col121 = tm.Col121,
                                            Col122 = tm.Col122,
                                            Col123 = tm.Col123,
                                            TList1 = tm.TList1,
                                            SelectedItems1 = tm.SelectedItems1,
                                        });
                                    }
                                }
                            }
                            break;
                    }
                    break;
                #endregion
                #region balsheet
                case "balsheet":
                    try
                    {
                        fdt = sgen.GetSession(MyGuid, "KPDFDT").ToString();
                        tdt = sgen.GetSession(MyGuid, "KPDTDT").ToString();
                    }
                    catch (Exception er) { }
                    switch (btnval.ToUpper())
                    {
                        case "GETPLDET":
                        case "GETASDET":
                        case "GETLBDET":
                            caption = "Balance Sheet";
                            sgen.SetSession(MyGuid, "l4fstr", URL);
                            Make_query(viewName, caption, btnval, "1", URL, fdt + "!~!~!~!~!" + tdt);
                            sgen.SetSession(MyGuid, "footitle", "Balance Sheet");
                            ViewBag.scripCall = "callFoo3('" + caption + "');";
                            //sgen.SetSession(MyGuid, "LEVEL4", 4);
                            break;
                        case "TBDETAIL":
                        case "DBDETAIL":
                        case "SRDETAIL":
                        case "PRDETAIL":
                        case "JRDETAIL":
                        case "PAYDETAIL":
                        case "RRDETAIL":
                        case "CRDETAIL":
                            if (btnval.Trim().ToUpper() == "TBDETAIL")
                            {
                                caption = "Detailed Trial Balance";
                                ttype = "TBDETAIL";
                            }
                            else if (btnval.Trim().ToUpper() == "DBDETAIL")
                            {
                                caption = "Detailed Day Book";
                                ttype = "DBDETAIL";
                            }
                            else if (btnval.Trim().ToUpper() == "SRDETAIL")
                            {
                                caption = "Detailed Sales Register";
                                ttype = "SRDETAIL";
                            }
                            else if (btnval.Trim().ToUpper() == "PRDETAIL")
                            {
                                caption = "Detailed Purchase Register";
                                ttype = "PRDETAIL";
                            }
                            else if (btnval.Trim().ToUpper() == "JRDETAIL")
                            {
                                caption = "Detailed Journal Register";
                                ttype = "JRDETAIL";
                            }
                            else if (btnval.Trim().ToUpper() == "PAYDETAIL")
                            {
                                caption = "Detailed Payment Register";
                                ttype = "PAYDETAIL";
                            }
                            else if (btnval.Trim().ToUpper() == "RRDETAIL")
                            {
                                caption = "Detailed Receipt Register";
                                ttype = "RRDETAIL";
                            }
                            else if (btnval.Trim().ToUpper() == "CRDETAIL")
                            {
                                caption = "Detailed Credit Register";
                                ttype = "CRDETAIL";
                            }
                            Make_query(viewName, caption, btnval, "2", URL, fdt + "!~!~!~!~!" + tdt);
                            ViewBag.scripCall = "callFoo3('" + caption + "');";
                            break;
                        case "BBOOK":
                        case "CBOOK":
                        case "SBOOK":
                        case "PBOOK":
                        case "JBOOK":
                            Make_query(viewName, "DETAILED VOUCHER", btnval, "2", URL, fdt + "!~!~!~!~!" + tdt);
                            ViewBag.scripCall = "callFoo3('" + caption + "');";
                            break;
                    }
                    break;
                #endregion
                #region cas_rec
                case "cas_rec":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            string ptyp = "";
                            ptyp = (model[0].Col12 == "11" || model[0].Col12 == "21") ? "BNK" : "CSH";
                            mq = "select cm.*,to_char(cm.chqdate,'" + sgen.Getsqldateformat() + "') as chqdate1,to_char(cm.vch_date,'" + sgen.Getsqldateformat() + "') as vch_date1 from vouchers cm " +
                         "where (cm.client_unit_id|| cm.vch_num|| to_char(cm.vch_date, 'yyyymmdd')||cm.type ) = '" + URL + "' and cm.reftype = '" + ptyp + "'";
                            dt = new DataTable();
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model = getedit(model, dt, URL);
                                if (model[0].Col12.Trim().Equals("11") || model[0].Col12.Trim().Equals("21"))
                                {
                                    mq1 = "select nvl(a.master_name,'-')||'('||cm.cpname||')' as bank_name,cm.cpname as bank_ac,cm.vch_num as bankid from clients_mst cm " +
                                        "inner join master_setting a on a.master_id = cm.ptype and a.type = 'ABD' and find_in_set(a.client_unit_id,cm.client_unit_id)=1 " +
                                        "where cm.type = 'BAD' and find_in_set(cm.client_unit_id, '" + unitid_mst + "')=1";
                                }
                                else
                                {
                                    mq1 = "select C_NAME AS bank_name,cm.vch_num as bankid from clients_mst cm where SUBSTR(cm.VCH_NUM,0,3) = '301' and " +
                                        "find_in_set(cm.client_unit_id, '" + unitid_mst + "')=1";
                                }
                                DataTable bnk = new DataTable();
                                bnk = sgen.getdata(userCode, mq1);
                                if (bnk.Rows.Count > 0)
                                {
                                    foreach (DataRow item in bnk.Rows)
                                    {
                                        mod1.Add(new SelectListItem { Text = item["bank_name"].ToString(), Value = item["bankid"].ToString() });
                                    }
                                }
                                TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
                                String[] L1 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["acode"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems1 = L1;
                                model[0].Col8 = "client_unit_id||vch_num|| to_char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";

                                model[0].Col12 = dt.Rows[0]["type"].ToString();
                                model[0].Col16 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dt.Rows[0]["vch_date1"].ToString();
                                model[0].Col26 = dt.Rows[0]["chqamt"].ToString();
                                model[0].Col40 = dt.Rows[0]["totremark"].ToString();
                            }
                            mq = "select cm.* from vouchers cm " +
                         "where (cm.client_unit_id|| cm.vch_num|| to_char(cm.vch_date, 'yyyymmdd')||cm.type ) = '" + URL + "' and cm.reftype = 'ADV'";
                            dt = new DataTable();
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model[0].Col27 = (model[0].Col12 == "11" || model[0].Col12 == "10") ? dt.Rows[0]["cramount"].ToString() : dt.Rows[0]["dramount"].ToString();
                            }
                            mq = "select vu.vch_num as doc_num,vu.type1,vu.adj_type,vu.chqno,vu.chqamt,vu.reftype,vu.client_id,vu.client_unit_id,vu.ent_by,vu.ent_date,vu.edit_by,vu.edit_date," +
                                "vu.rec_id,to_char(vu.chqdate, '" + sgen.Getsqldateformat() + "') chqdate,to_char(vu.vch_date, '" + sgen.Getsqldateformat() + "') doc_date,vu.type,vu.invno" +
                                ",to_char(vu.invdate, '" + sgen.Getsqldateformat() + "') as invdate1,vu.totremark,vu.remark," +
                                "vu.acode,nvl(cl.aname,'-') party_name,vu.cramount,vu.dramount from vouchers vu INNER JOIN " +
                                "(select vch_num as acode,c_name as aname,CLIENT_UNIT_ID from clients_mst where type='BCD' and find_in_set(client_unit_id, '" + unitid_mst + "')=1 union all select vch_num as acode ,c_name as aname,CLIENT_UNIT_ID from clients_mst " +
                                "where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "')=1 union all select cm.vch_num as acode," +
                                "nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname,cm.CLIENT_UNIT_ID from master_setting a inner join clients_mst cm on a.master_id" +
                                " = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id where cm.type = 'BAD' and cm.client_unit_id" +
                                " = '" + unitid_mst + "') cl on cl.acode = vu.acode and find_in_set(vu.client_unit_id, cl.client_unit_id) = 1 " +
                                "where (vu.client_unit_id || vu.vch_num || to_char(vu.vch_date, 'yyyymmdd') || vu.type) = '" + URL + "' and vu.reftype = 'PTY' and (to_number(vu.dramount)+to_number(vu.cramount))>0 order by vu.vch_num desc";
                            dt = new DataTable();
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model[0].dt1 = model[0].dt1.Clone();
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["SNo"] = i + 1;
                                    dr["Ref_no"] = dt.Rows[i]["invno"].ToString();
                                    dr["Ref_date"] = dt.Rows[i]["invdate1"].ToString();
                                    dr["Acode"] = dt.Rows[i]["acode"].ToString();
                                    dr["Aname"] = dt.Rows[i]["party_name"].ToString();
                                    dr["cramount"] = dt.Rows[i]["cramount"].ToString();
                                    dr["dramount"] = dt.Rows[i]["dramount"].ToString();
                                    dr["Adj_type_code"] = dt.Rows[i]["adj_type"].ToString();
                                    dr["Adj_Type"] = sgen.seekval(userCode, "select a.Adj_type as adty from (select 'ADV' as Adj_type_code,'Advance' as Adj_type from dual union all" +
                " select 'AGR' as Adj_type_code,'Against Ref' as Adj_type from dual union all select 'NEW' as Adj_type_code,'New Ref' as Adj_type from dual" +
                " union all select 'OAC' as Adj_type_code,'On Account' as Adj_type from dual) a where a.Adj_type_code = '" + dt.Rows[i]["adj_type"].ToString() + "'", "adty");
                                    dr["Remark"] = dt.Rows[i]["remark"].ToString();
                                    model[0].dt1.Rows.Add(dr);
                                }
                                model[0].dt1.AcceptChanges();
                            }
                            break;
                        case "PRINT":
                            mq = "select vu.vch_num as doc_num,vu.client_id,vu.client_unit_id,to_char(vu.vch_date, '" + sgen.Getsqldateformat() + "') doc_date,vu.type,vu.invno as invoice_no" +
                            ",to_char(vu.invdate, '" + sgen.Getsqldateformat() + "') as invoice_date,vu.totremark as remark ," +
                            "vu.acode as party_code,nvl(cl.aname,'-') party_name,(case when vu.type='20' then 'CASH PAYMENT' when vu.type='10' then 'CASH RECIEPT' when vu.type='21' then 'BANK PAYMENT' when vu.type='11' then 'BANK RECIEPT' when vu.type='32' then 'CREDIT NOTE' when vu.type='31' then 'DEBIT NOTE' when vu.type='30' then 'JOURNAL VOUCHER' when vu.type='33' then 'CONTRA VOUCHER' ELSE 'VOUCHER' end) AS VOUTITLE" +
                            ",to_number(vu.cramount) cramount,to_number(vu.dramount) dramount from vouchers vu INNER JOIN " +
                              "(select vch_num as acode,c_name as aname,CLIENT_UNIT_ID from clients_mst where type='BCD' and find_in_set(client_unit_id, '" + unitid_mst + "')=1 union all select vch_num as acode ,c_name as aname,CLIENT_UNIT_ID from clients_mst " +
                              "where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "')=1 union all select cm.vch_num as acode," +
                              "nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname,cm.CLIENT_UNIT_ID from master_setting a inner join clients_mst cm on a.master_id" +
                              " = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id where cm.type = 'BAD' and cm.client_unit_id" +
                              " = '" + unitid_mst + "') cl on cl.acode = vu.acode and find_in_set(vu.client_unit_id, cl.client_unit_id) = 1 " +
                              "where (vu.client_unit_id || vu.vch_num || to_char(vu.vch_date, 'yyyymmdd') || vu.type) = '" + URL + "' order by vu.vch_num desc";
                            //mq = "select vu.vch_num as doc_num,vu.client_id,vu.client_unit_id,to_char(vu.vch_date, '" + sgen.Getsqldateformat() + "') doc_date,vu.type,vu.invno as invoice_no" +
                            //",to_char(vu.invdate, '" + sgen.Getsqldateformat() + "') as invoice_date,vu.totremark as remark ," +
                            //"vu.acode as party_code,nvl(cl.c_name,'-') party_name,(case when vu.type='32' then 'CREDIT NOTE' when vu.type='31' then 'DEBIT NOTE' when vu.type='30' then 'JOURNAL VOUCHER' when vu.type='33' then 'CONTRA VOUCHER' ELSE 'VOUCHER' end) AS VOUTITLE,to_number(vu.cramount) cramount,to_number(vu.dramount) dramount from vouchers vu inner join clients_mst cl on cl.vch_num = vu.acode and find_in_set(vu.client_unit_id, cl.client_unit_id) = 1 " +
                            //"where (vu.client_unit_id || vu.vch_num || to_char(vu.vch_date, 'yyyymmdd') || vu.type) = '" + URL + "'";
                            model[0].TList1 = mod1;
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                DataSet ds = new DataSet();
                                dt.TableName = "prepcur";
                                ds.Tables.Add(dt);
                                sgen.open_report_byDs_xml(userCode, ds, "voucher1", "Print Voucher");
                                ViewBag.scripCall += "showRptnew('Print Voucher');disableForm();";
                            }
                            else
                            {
                                ViewBag.scripCall = "showmsgJS(1, 'Data Not Exist', 2);";
                            }
                            break;
                        case "PARTY":
                            mq = "select * from (select a.vch_num||a.type as fstr,a.vch_num ,a.c_name as Client_name,a.addr as address,a.c_gstin as GSTIN," +
                                "(case when a.tor = 'R' then 'Regular' when a.tor='C' then 'Composition' else 'Not Registered' END) GSTType," +
                                "a.type_desc as Search_text from clients_mst a  where a.type = 'BCD' and substr(a.vch_num,0,3)='203' and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "' " +
                                "union all select a.vch_num || a.type as fstr,a.vch_num,ledger_name Client_name, ledger_abb address,to_char(group_code) GSTIN," +
                                "'-' GSTType,'-' Search_text from ledger_master a where a.type = 'ALM' and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "') " +
                                "a where a.fstr in ('" + URL + "')";
                            dtt = sgen.getdata(userCode, mq);
                            if (model[0].dt1.Rows[0]["acode"].ToString().Trim().Equals("-"))
                            {
                                model[0].dt1.Rows.RemoveAt(0);
                            }
                            if (dtt.Rows.Count > 0)
                            {
                                DataRow dr = model[0].dt1.NewRow();
                                dr[0] = dtt.Rows[0]["fstr"].ToString();
                                dr[1] = dtt.Rows[0]["vch_num"].ToString();
                                dr[2] = dtt.Rows[0]["Client_name"].ToString();
                                dr[3] = "0";
                                dr[4] = "0";
                                model[0].dt1.Rows.Add(dr);
                            }
                            break;
                    }
                    break;
                #endregion
                #region cas_recn
                case "cas_recn":
                    var tmm = model.FirstOrDefault();
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "COPYOLD":
                            string ptyp = "";
                            if (model[0].Col12 == "11" || model[0].Col12 == "21" || model[0].Col12 == "10" || model[0].Col12 == "20")
                            {
                                ptyp = (model[0].Col12 == "11" || model[0].Col12 == "21") ? "BNK" : "CSH";
                                mq = "select cm.*,to_char(cm.chqdate,'" + sgen.Getsqldateformat() + "') as chqdate1,to_char(cm.vch_date,'" + sgen.Getsqldateformat() + "') as vch_date1 from vouchers cm " +
                             "where (cm.client_unit_id|| cm.vch_num|| to_char(cm.vch_date, 'yyyymmdd')||cm.type ) = '" + URL + "' and cm.reftype = '" + ptyp + "'";
                                dt = new DataTable();
                                dt = sgen.getdata(userCode, mq);
                                if (dt.Rows.Count > 0)
                                {
                                    if (model[0].Col12.Trim().Equals("11") || model[0].Col12.Trim().Equals("21"))
                                    {
                                        mq1 = "select nvl(a.master_name,'-')||'('||cm.cpname||')' as bank_name,cm.cpname as bank_ac,cm.vch_num as bankid from clients_mst cm " +
                                            "inner join master_setting a on a.master_id = cm.ptype and a.type = 'ABD' and find_in_set(a.client_unit_id,cm.client_unit_id)=1 " +
                                            "where cm.type = 'BAD' and find_in_set(cm.client_unit_id, '" + unitid_mst + "')=1";
                                    }
                                    else
                                    {
                                        mq1 = "select C_NAME AS bank_name,cm.vch_num as bankid from clients_mst cm where SUBSTR(cm.VCH_NUM,0,3) = '301' and " +
                                            "find_in_set(cm.client_unit_id, '" + unitid_mst + "')=1";
                                    }
                                    DataTable bnk = new DataTable();
                                    bnk = sgen.getdata(userCode, mq1);
                                    if (bnk.Rows.Count > 0)
                                    {
                                        foreach (DataRow item in bnk.Rows)
                                        {
                                            mod1.Add(new SelectListItem { Text = item["bank_name"].ToString(), Value = item["bankid"].ToString() });
                                        }
                                    }
                                    TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
                                    String[] L1 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["acode"].ToString()).Distinct()).Split(',');
                                    model[0].SelectedItems1 = L1;
                                }
                            }
                            #region ddls
                            mq1 = "select a.* from (select vch_num as acode,c_name as aname from clients_mst where type='BCD' and " +
                                          "find_in_set(client_unit_id, '" + unitid_mst + "')=1 union all select vch_num as acode ,c_name as aname from clients_mst where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "')=1 union all select cm.vch_num as acode,nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname from master_setting a inner join clients_mst cm on a.master_id = cm.ptype and a.type = 'ABD' and find_in_set(a.client_unit_id, cm.client_unit_id)=1 where cm.type = 'BAD' " +
                                          "and find_in_set(cm.client_unit_id,'" + unitid_mst + "')=1) a where a.aname<>'0'";
                            DataTable dtldg = sgen.getdata(userCode, mq1);
                            List<SelectListItem> ldgmode = new List<SelectListItem>();
                            List<SelectListItem> methodmod = new List<SelectListItem>();
                            if (dtldg.Rows.Count > 0)
                            {
                                ldgmode = sgen.dt_to_selectlist(dtldg);
                            }
                            mq1 = "select 'ADV' as Adj_type_code,'Advance' as Adj_type from dual union all" +
                        " select 'AGR' as Adj_type_code,'Against Ref' as Adj_type from dual union all select 'NEW' as Adj_type_code,'New Ref' as Adj_type from dual union all select 'OAC' as Adj_type_code,'On Account' as Adj_type from dual";
                            DataTable dtbk = sgen.getdata(userCode, mq1);
                            if (dtbk.Rows.Count > 0)
                            {
                                methodmod = sgen.dt_to_selectlist(dtbk);
                            }
                            #endregion
                            mq = "select vu.vch_num,vu.PRNO,vu.RNO,vu.type1,vu.adj_type,vu.chqno,vu.chqamt,vu.client_id,vu.ent_by,vu.ent_date,vu.rec_id,vu.client_unit_id,vu.reftype,to_char(vu.chqdate, '" + sgen.Getsqldateformat() + "') chqdate,to_char(vu.vch_date, '" + sgen.Getsqldateformat() + "') doc_date,vu.type,vu.invno" +
                                ",to_char(vu.invdate, '" + sgen.Getsqldateformat() + "') as invdate1,vu.invamt,vu.BALAMT AS BALAMT,vu.totremark,vu.remark," +
                                "vu.acode,nvl(cl.aname,'-') party_name,vu.cramount,vu.dramount from vouchers vu INNER JOIN " +
                                "(select vch_num as acode,c_name as aname,CLIENT_UNIT_ID from clients_mst where type='BCD' and find_in_set(client_unit_id, '" + unitid_mst + "')=1 union all select vch_num as acode ,c_name as aname,CLIENT_UNIT_ID from clients_mst " +
                                "where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "')=1 union all select cm.vch_num as acode," +
                                "nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname,cm.CLIENT_UNIT_ID from master_setting a inner join clients_mst cm on a.master_id" +
                                " = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id where cm.type = 'BAD' and cm.client_unit_id" +
                                " = '" + unitid_mst + "') cl on cl.acode = vu.acode and find_in_set(vu.client_unit_id, cl.client_unit_id) = 1 " +
                                "where (vu.client_unit_id || vu.vch_num || to_char(vu.vch_date, 'yyyymmdd') || vu.type) = '" + URL + "' and vu.reftype not in ('BNK','CSH') and (to_number(vu.dramount)+to_number(vu.cramount))>0 order by vu.RNO ASC";
                            dt = new DataTable();
                            DataTable dtmain = new DataTable();
                            dt = sgen.getdata(userCode, mq);
                            dtmain = dt.Copy();
                            if (btnval == "EDIT")
                            {
                                model = getedit(model, dt, URL);
                                model[0].Col8 = "client_unit_id||vch_num|| to_char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";
                                model[0].Col12 = dt.Rows[0]["type"].ToString();
                                model[0].Col16 = dt.Rows[0]["vch_num"].ToString();
                            }
                            model[0].Col17 = dt.Rows[0]["doc_date"].ToString();
                            model[0].Col26 = dt.Rows[0]["chqamt"].ToString();
                            model[0].Col40 = dt.Rows[0]["totremark"].ToString();
                            DataView dv = dtmain.DefaultView;
                            var dtrno = dv.ToTable(true, "rno");
                            tmm = model.FirstOrDefault();
                            model = new List<Tmodelmain>();
                            for (int i = 0; i < dtrno.Rows.Count; i++)
                            {
                                var dtacode = dtmain.Select("rno='" + dtrno.Rows[i]["rno"].ToString() + "'").CopyToDataTable();
                                var typee = dtacode.Rows[0]["adj_type"].ToString();

                                Tmodelmain tmain = new Tmodelmain();
                                TempData[MyGuid + "_TList2"] = tmain.TList2 = ldgmode;
                                TempData[MyGuid + "_TList3"] = tmain.TList3 = methodmod;
                                tmain.TList1 = tmm.TList1;
                                tmain.SelectedItems1 = tmm.SelectedItems1;
                                tmain.Col3 = tmm.Col3;
                                tmain.Col4 = tmm.Col4;
                                tmain.Col8 = tmm.Col8;
                                tmain.Col9 = tmm.Col9;
                                tmain.Col11 = tmm.Col11;
                                tmain.Col12 = tmm.Col12;
                                tmain.Col10 = tmm.Col10;
                                tmain.Col13 = tmm.Col13;
                                tmain.Col14 = tmm.Col14;
                                tmain.Col15 = tmm.Col15;
                                tmain.Col16 = tmm.Col16;
                                tmain.Col17 = tmm.Col17;
                                tmain.Col18 = tmm.Col18;
                                tmain.Col19 = tmm.Col19;
                                tmain.Col20 = tmm.Col20;
                                tmain.Col21 = tmm.Col21;
                                tmain.Col22 = tmm.Col22;
                                tmain.Col23 = tmm.Col23;
                                tmain.Col24 = tmm.Col24;
                                tmain.Col42 = tmm.Col42;
                                tmain.Col25 = tmm.Col25;
                                tmain.Col26 = tmm.Col26;
                                tmain.Col27 = tmm.Col27;
                                tmain.Col28 = tmm.Col28;
                                tmain.Col29 = tmm.Col29;
                                tmain.Col40 = tmm.Col40;
                                tmain.Col41 = tmm.Col41;
                                tmain.Col42 = tmm.Col42;
                                tmain.Col100 = tmm.Col100;
                                tmain.Col121 = tmm.Col121;
                                tmain.Col122 = tmm.Col122;
                                tmain.LTM3 = tmm.LTM3;
                                tmain.Col123 = tmm.Col123;
                                tmain.SelectedItems2 = new string[] { dtacode.Rows[0]["acode"].ToString() };
                                tmain.SelectedItems3 = new string[] { dtacode.Rows[0]["adj_type"].ToString() };
                                tmain.Col32 = dtacode.Rows[0]["remark"].ToString();
                                tmain.Col31 = dtacode.Rows[0]["cramount"].ToString();
                                tmain.Col30 = dtacode.Rows[0]["dramount"].ToString();
                                tmain.LTM1 = new List<Tmodelmain>();
                                if (typee == "AGR")
                                {
                                    var bills = dtacode;
                                    if (bills.Rows.Count > 0)
                                    {
                                        foreach (DataRow dr in bills.Rows)
                                        {
                                            Tmodelmain titu = new Tmodelmain();
                                            titu.Col11 = dr["invno"].ToString();
                                            titu.Col12 = dr["invdate1"].ToString();
                                            titu.Col13 = dr["invamt"].ToString();
                                            titu.Col15 = dr["BALAMT"].ToString();
                                            var amt = "0";
                                            amt = (titu.Col15.StartsWith("-")) ? dr["dramount"].ToString() : dr["cramount"].ToString();
                                            titu.Col14 = amt;
                                            tmain.LTM1.Add(titu);
                                        }
                                    }
                                }
                                if (typee == "NEW")
                                {
                                    var bills = dtacode;
                                    if (bills.Rows.Count > 0)
                                    {
                                        foreach (DataRow dr in bills.Rows)
                                        {
                                            Tmodelmain titu = new Tmodelmain();
                                            titu.Col11 = dr["invno"].ToString();
                                            titu.Col12 = dr["invdate1"].ToString();
                                            tmain.LTM1.Add(titu);
                                        }
                                    }
                                }
                                model.Add(tmain);
                            }
                            #region attachment
                            DataTable dtf = new DataTable();
                            dtf = sgen.getdata(userCode, "select client_id||client_unit_id||vch_num||to_char(vch_date,'yyyymmdd')||type as fstr, rec_id,file_name,file_path,col1,col2,description,Col3 from file_tab where client_unit_id ||vch_num || to_char(vch_date, 'yyyymmdd') || type = '" + URL + "' and col1='ACC'");
                            model[0].Files1 = new List<Files>();
                            List<Files> ltmf = new List<Files>();
                            string des = "", title = "";
                            if (dtf.Rows.Count > 0)
                            {
                                try
                                {
                                    string serverpath = Server.MapPath("~/Uploads/" + cg_com_name.Replace(" ", "") + "/");
                                    model[0].Col41 = dtf.Rows[0]["col3"].ToString();
                                    model[0].Col42 = dtf.Rows[0]["description"].ToString();
                                    foreach (DataRow drf in dtf.Rows)
                                    {
                                        Files tmf = new Files();
                                        var fullPath = serverpath + drf["file_path"].ToString();
                                        byte[] imageArray = System.IO.File.ReadAllBytes(fullPath);
                                        string base64 = Convert.ToBase64String(imageArray);
                                        tmf.FileBase64 = base64;
                                        tmf.FileName = drf["file_name"].ToString();
                                        tmf.ContentType = drf["col2"].ToString();
                                        tmf.FileTitle = drf["col3"].ToString();
                                        tmf.FileDesc = drf["description"].ToString();
                                        ltmf.Add(tmf);
                                    }
                                    model[0].Files1 = ltmf;
                                }
                                catch (Exception err)
                                {
                                    model[0].Files1 = new List<Files>();
                                    Files tm3 = new Files();
                                    tm3.FileName = "-";
                                    model[0].Files1.Add(tm3);
                                    model[0].Files1.Add(tm3);
                                }
                            }
                            else
                            {

                                model[0].Files1 = new List<Files>();
                                Files tm3 = new Files();
                                tm3.FileName = "-";
                                model[0].Files1.Add(tm3);
                                model[0].Files1.Add(tm3);
                            }

                            #endregion
                            break;

                        case "FDEL":
                            string fid = sgen.GetSession(MyGuid, "delid").ToString();
                            res = sgen.execute_cmd(userCode, "Delete from file_tab WHERE rec_id='" + fid + "' and type='" + model[0].Col12 + "' and col1='ACC' and client_id='"
              + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
                            sgen.SetSession(MyGuid, "delid", null);
                            if (res)
                            {
                                for (int d = 0; d < model[0].LTM3.Count; d++)
                                {
                                    var id = model[0].LTM3[d].Col3;
                                    if (id == fid) { model[0].LTM3.RemoveAt(d); }
                                }
                                model[0].Col29 = "N";
                            }
                            break;
                        case "PRINT":
                            mq = "select vu.vch_num as doc_num,vu.client_id,vu.client_unit_id,to_char(vu.vch_date, '" + sgen.Getsqldateformat() + "') doc_date,vu.type,vu.invno as invoice_no" +
                            ",to_char(vu.invdate, '" + sgen.Getsqldateformat() + "') as invoice_date,nvl(vu.totremark,'-') as remark, REPLACE(ud.FIRST_NAME|| ' '|| ud.MIDDLE_NAME|| ' '|| ud.last_name,'0','') as created_by," +
                            "vu.acode as party_code,nvl(cl.aname,'-') party_name,(case when vu.type='20' then 'CASH PAYMENT' when vu.type='10' then 'CASH RECIEPT' when vu.type='21' then 'BANK PAYMENT' when vu.type='11' then 'BANK RECIEPT' when vu.type='32' then 'CREDIT NOTE' when vu.type='31' then 'DEBIT NOTE' when vu.type='30' then 'JOURNAL VOUCHER' when vu.type='33' then 'CONTRA VOUCHER' ELSE 'VOUCHER' end) AS VOUTITLE" +
                            ",to_number(vu.cramount) cramount,to_number(vu.dramount) dramount from vouchers vu INNER JOIN " +
                              "(select vch_num as acode,c_name as aname,CLIENT_UNIT_ID from clients_mst where type='BCD' and find_in_set(client_unit_id, '" + unitid_mst + "')=1 union all select vch_num as acode ,c_name as aname,CLIENT_UNIT_ID from clients_mst " +
                              "where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "')=1 union all select cm.vch_num as acode," +
                              "nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname,cm.CLIENT_UNIT_ID from master_setting a inner join clients_mst cm on a.master_id" +
                              " = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id where cm.type = 'BAD' and cm.client_unit_id" +
                              " = '" + unitid_mst + "') cl on cl.acode = vu.acode and find_in_set(vu.client_unit_id, cl.client_unit_id) = 1 " +
                              "inner join user_details ud on vu.ent_by = ud.vch_num and ud.type = 'CPR' " +
                              "where (vu.client_unit_id || vu.vch_num || to_char(vu.vch_date, 'yyyymmdd') || vu.type) = '" + URL + "' and to_number(vu.dramount)+to_number(vu.cramount) > 0 order by vu.rno";
                            //mq = "select vu.vch_num as doc_num,vu.client_id,vu.client_unit_id,to_char(vu.vch_date, '" + sgen.Getsqldateformat() + "') doc_date,vu.type,vu.invno as invoice_no" +
                            //",to_char(vu.invdate, '" + sgen.Getsqldateformat() + "') as invoice_date,vu.totremark as remark ," +
                            //"vu.acode as party_code,nvl(cl.c_name,'-') party_name,(case when vu.type='32' then 'CREDIT NOTE' when vu.type='31' then 'DEBIT NOTE' when vu.type='30' then 'JOURNAL VOUCHER' when vu.type='33' then 'CONTRA VOUCHER' ELSE 'VOUCHER' end) AS VOUTITLE,to_number(vu.cramount) cramount,to_number(vu.dramount) dramount from vouchers vu inner join clients_mst cl on cl.vch_num = vu.acode and find_in_set(vu.client_unit_id, cl.client_unit_id) = 1 " +
                            //"where (vu.client_unit_id || vu.vch_num || to_char(vu.vch_date, 'yyyymmdd') || vu.type) = '" + URL + "'";
                            model[0].TList1 = mod1;
                            model[0].TList2 = mod2;
                            model[0].TList3 = mod3;
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                DataSet ds = new DataSet();
                                dt.TableName = "prepcur";
                                ds.Tables.Add(dt);
                                sgen.open_report_byDs_xml(userCode, ds, "voucher1", "Print Voucher");
                                ViewBag.scripCall += "showRptnew('Print Voucher');disableForm();";
                            }
                            else
                            {
                                ViewBag.scripCall = "showmsgJS(1, 'Data Not Exist', 2);";
                            }
                            break;
                        case "PARTY":
                            mq = "select * from (select a.vch_num||a.type as fstr,a.vch_num ,a.c_name as Client_name,a.addr as address,a.c_gstin as GSTIN," +
                                "(case when a.tor = 'R' then 'Regular' when a.tor='C' then 'Composition' else 'Not Registered' END) GSTType," +
                                "a.type_desc as Search_text from clients_mst a  where a.type = 'BCD' and substr(a.vch_num,0,3)='203' and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "' " +
                                "union all select a.vch_num || a.type as fstr,a.vch_num,ledger_name Client_name, ledger_abb address,to_char(group_code) GSTIN," +
                                "'-' GSTType,'-' Search_text from ledger_master a where a.type = 'ALM' and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "') " +
                                "a where a.fstr in ('" + URL + "')";
                            dtt = sgen.getdata(userCode, mq);
                            if (model[0].dt1.Rows[0]["acode"].ToString().Trim().Equals("-"))
                            {
                                model[0].dt1.Rows.RemoveAt(0);
                            }
                            if (dtt.Rows.Count > 0)
                            {
                                DataRow dr = model[0].dt1.NewRow();
                                dr[0] = dtt.Rows[0]["fstr"].ToString();
                                dr[1] = dtt.Rows[0]["vch_num"].ToString();
                                dr[2] = dtt.Rows[0]["Client_name"].ToString();
                                dr[3] = "0";
                                dr[4] = "0";
                                model[0].dt1.Rows.Add(dr);
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
        #region Make Query
        public void Make_query(string viewname, string title, string btnval, string seektype, string param1, string param2, string Myguid = "")
        {
            string cond = "", mid = "", joinmst = "", colmst = "", grpmst = "", btnvou = "";
            FillMst(Myguid);
            sgen.SetSession(MyGuid, "btnval", btnval);
            string year = year_from.Substring(6, 4);
            switch (viewname.ToLower())
            {
                #region vou_entry
                case "vou_entry":
                    switch (btnval.ToUpper())
                    {
                        case "PRINT":
                        case "VIEW":
                        case "EDIT":
                            type = "";
                            if (sgen.GetSession(MyGuid, "VOUTYPE") != null)
                            {
                                type = "" + sgen.GetSession(MyGuid, "VOUTYPE").ToString() + "";
                            }
                            else
                            {
                                type = "VOU','ESP";
                            }
                            //cmd = "select (vu.client_unit_id||vu.vch_num|| to_char(vu.vch_date, 'yyyymmdd')|| vu.type) as fstr,vu.vch_num as doc_num,to_char(vu.vch_date, 'dd/MM/YYYY') doc_date,vu.type, vu.invno as ref_no,to_char(vu.invdate, 'dd/MM/YYYY') as ref_date,vu.acode as party_code,nvl(cl.c_name, '-') party_name,vu.dramount,vu.cramount,vu.totremark as remark from vouchers vu inner join clients_mst cl on cl.vch_num = vu.acode and find_in_set(vu.client_unit_id, cl.client_unit_id) = 1 where vu.type in ('" + type + "') and vu.client_unit_id = '" + unitid_mst + "' order by vu.vch_num desc";
                            cmd = "select (vu.client_unit_id||vu.vch_num|| to_char(vu.vch_date, 'yyyymmdd')|| vu.type) as fstr,vu.vch_num as doc_num,to_char(vu.vch_date, 'dd/MM/YYYY') doc_date,vu.type,vu.invno as ref_no,to_char(vu.invdate, 'dd/MM/YYYY') as ref_date,vu.acode as party_code" +
                                ",vu.rvscode as reverse_code,nvl(cl.aname, '-') party_name,vu.dramount,vu.cramount,vu.totremark as remark from vouchers vu INNER JOIN (select vch_num as acode,c_name as aname,CLIENT_UNIT_ID from clients_mst " +
                                "where type='BCD' and find_in_set(client_unit_id, '" + unitid_mst + "')=1 union all select vch_num as acode ,c_name as aname,CLIENT_UNIT_ID from clients_mst where type = 'LDG' " +
                                "and find_in_set(client_unit_id, '" + unitid_mst + "')=1 union all select cm.vch_num as acode,nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname,cm.CLIENT_UNIT_ID from master_setting a inner join clients_mst cm on " +
                                "a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id where cm.type = 'BAD' and cm.client_unit_id = '" + unitid_mst + "') cl on cl.acode = vu.acode and find_in_set(vu.client_unit_id, cl.client_unit_id) = 1 " +
                                "where vu.type in ('" + type + "') and vu.client_unit_id = '" + unitid_mst + "' order by vu.vch_num desc";
                            break;
                        case "PARTY":
                            cmd = "select a.vch_num||a.type as fstr,a.c_name as Client_name,a.addr as address,a.c_gstin as GSTIN," +
                                "(case when a.tor = 'R' then 'Regular' when a.tor='C' then 'Composition' else 'Not Registered' END) GSTType," +
                                "a.type_desc as Search_text from clients_mst a  where a.type = 'BCD' and substr(a.vch_num,0,3)='203' and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "' " +
                                "union all select a.vch_num || a.type as fstr,ledger_name Client_name, ledger_abb address,to_char(group_code) GSTIN, " +
                                "'-' GSTType,'-' Search_text from ledger_master a where a.type = 'ALM' and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                            break;
                    }
                    break;
                #endregion
                #region Groupmaster
                case "grp_mst":
                    q1 = "SELECT client_id||client_unit_id||vch_num||TO_CHAR(vch_date, 'YYYYMMDD')|| type as fstr,master_type as Financial_Category_Code , master_name AS Financial_Category" +
        ",'-' Main_Head,'-' as Sub_Head,'-' as Sub_Sub_Head FROM master_setting where type = 'AGM' AND TRIM(CLIENT_ID)= '-' AND TRIM(CLIENT_UNIT_ID)= '-'";

                    q2 = "SELECT  (a.client_id||a.client_unit_id|| a.vch_num|| TO_CHAR(a.vch_date, 'YYYYMMDD')|| a.type) as fstr,a.master_type as Financial_Category_Code" +
                               " ,b.master_name as Financial_Category,a.master_name as Main_Head,'-' as Sub_Head,'-' as Sub_Sub_Head FROM master_setting a " +
                               "INNER JOIN master_setting b ON SUBSTR(a.master_type, 1, 1) = SUBSTR(b.master_type, 1, 1)  AND TO_NUMBER(SUBSTR(b.master_type,2,7))= 0" +
                               "   and b.type = 'AGM'  where TO_NUMBER(SUBSTR(a.master_type,2,3))> 0 AND TO_NUMBER(SUBSTR(a.master_type,4,7))= 0 " +
                               "and a.type = 'AGM' and a.client_id='" + clientid_mst + "' and a.client_unit_id='" + unitid_mst + "'";
                    q3 = "SELECT (a.client_id|| a.client_unit_id|| a.vch_num|| TO_CHAR(a.vch_date, 'YYYYMMDD')|| a.type) as fstr,a.master_type as Financial_Category_Code " +
                               ",b.master_name as Financial_Category ,c.master_name as Main_Head,a.master_name as Sub_Head,'-' as Sub_Sub_Head  FROM master_setting a" +
                               " INNER JOIN master_setting b ON SUBSTR(a.master_type, 1, 1) = SUBSTR(b.master_type, 1, 1)  AND TO_NUMBER(SUBSTR(b.master_type,2,7))= 0 " +
                               "  and b.type = 'AGM'  INNER JOIN master_setting c ON SUBSTR(a.master_type,1,3)= SUBSTR(c.master_type, 1, 3)  AND TO_NUMBER(SUBSTR(c.master_type,4,7))= 0 " +
                               "  and c.type = 'AGM'  where TO_NUMBER(SUBSTR(a.master_type,4,5))> 0 AND TO_NUMBER(SUBSTR(a.master_type,6,7))= 0 and " +
                               "a.type = 'AGM' and a.client_id='" + clientid_mst + "' and a.client_unit_id='" + unitid_mst + "'  and a.client_id=c.client_id and  a.client_unit_id = c.client_unit_id";
                    q4 = "SELECT  (a.client_id|| a.client_unit_id|| a.vch_num|| TO_CHAR(a.vch_date, 'YYYYMMDD')|| a.type) as fstr,a.master_type as Financial_Category_Code" +
                              " ,b.master_name AS Financial_Category,c.master_name as Main_Head,d.master_name as Sub_Head,a.master_name as Sub_Sub_Head  FROM master_setting a" +
                              " INNER JOIN master_setting b ON SUBSTR(a.master_type, 1, 1) = SUBSTR(b.master_type, 1, 1)  AND TO_NUMBER(SUBSTR(b.master_type,2,7))= 0 " +
                              "  and b.type = 'AGM'  INNER JOIN master_setting c ON SUBSTR(a.master_type,1,3)= SUBSTR(c.master_type, 1, 3)  AND TO_NUMBER(SUBSTR(c.master_type,4,7))= 0" +
                              "   and c.type = 'AGM'  INNER JOIN master_setting d ON SUBSTR(a.master_type,1,5)= SUBSTR(d.master_type, 1, 5)  AND TO_NUMBER(SUBSTR(d.master_type,6,7))= 0 " +
                              "  and d.type = 'AGM'  where TO_NUMBER(SUBSTR(a.master_type,6,7))> 0  and a.type = 'AGM' and a.client_id='" + clientid_mst + "' and a.client_unit_id='" + unitid_mst + "'" +
                              "  and a.client_id=c.client_id and a.client_unit_id= c.client_unit_id and a.client_id=d.client_id and a.client_unit_id=d.client_unit_id";

                    switch (btnval.ToUpper())
                    {

                        case "VIEW":
                            cmd = q1 + " union   " + q2 + " union " + q3 + " union " + q4;
                            break;

                        case "EDIT":

                            cmd = q2 + " union " + q3 + " union " + q4;
                            break;
                        case "GRP":
                            mq = q2 + " union " + q3;
                            cmd = q1 + " union  select  a.fstr,a.Financial_Category_Code,a.Financial_Category,a.Main_Head,a.Sub_Head,a.Sub_Sub_Head from (" + mq + ") a";
                            break;

                    }
                    break;
                #endregion
                #region  currency_rate master
                case "cur_rate":
                    switch (btnval)
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = "select (ms.client_id|| ms.client_unit_id|| ms.master_id||to_char(ms.vch_date, 'yyyymmdd')|| ms.type) fstr,ms.classid cur," +
                                "cs.master_name Currency,ms.master_name Custom_Dept_Reference_Rate, ms.sectionid Sale_Rate, ms.subjectid Purchase_Rate," +
                                "(b.First_name || ' ' || replace(b.middle_name, '0', '') || ' ' || b.last_name) Ent_By," +
                                "to_char(convert_tz(ms.master_entdate, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') Ent_Date from master_setting ms " +
                                "inner join master_setting cs on cs.master_id = ms.classid and cs.type = 'CTM' and cs.client_unit_id = ms.client_unit_id " +
                                "inner join user_details b on ms.master_entby = b.vch_num and b.type = 'CPR' " +
                                "where ms.type in ('FCR', 'DDFCR') and ms.client_unit_id = '" + unitid_mst + "'";
                            break;
                    }
                    break;
                #endregion
                #region  ledger_mst
                case "lgr_mst":

                    q1 = "SELECT client_id||client_unit_id||vch_num||TO_CHAR(vch_date, 'YYYYMMDD')|| type as fstr,master_type as Financial_Category_Code , master_name AS Financial_Category,'-' Main_Head,'-' as Sub_Head,'-' as Sub_Sub_Head FROM master_setting where type = 'AGM' AND TRIM(CLIENT_ID)= '-' AND TRIM(CLIENT_UNIT_ID)= '-'";
                    q2 = "SELECT  (a.client_id||a.client_unit_id|| a.vch_num|| TO_CHAR(a.vch_date, 'YYYYMMDD')|| a.type) as fstr,a.master_type as Financial_Category_Code ,b.master_name as Financial_Category,a.master_name as Main_Head,'-' as Sub_Head,'-' as Sub_Sub_Head FROM master_setting a INNER JOIN master_setting b ON SUBSTR(a.master_type, 1, 1) = SUBSTR(b.master_type, 1, 1)  AND TO_NUMBER(SUBSTR(b.master_type,2,7))= 0   and b.type = 'AGM'  where TO_NUMBER(SUBSTR(a.master_type,2,3))> 0 AND TO_NUMBER(SUBSTR(a.master_type,4,7))= 0 and a.type = 'AGM' and a.client_id='" + clientid_mst + "' and a.client_unit_id='" + unitid_mst + "'";
                    q3 = "SELECT (a.client_id|| a.client_unit_id|| a.vch_num|| TO_CHAR(a.vch_date, 'YYYYMMDD')|| a.type) as fstr,a.master_type as Financial_Category_Code ,b.master_name as Financial_Category ,c.master_name as Main_Head,a.master_name as Sub_Head,'-' as Sub_Sub_Head  FROM master_setting a INNER JOIN master_setting b ON SUBSTR(a.master_type, 1, 1) = SUBSTR(b.master_type, 1, 1)  AND TO_NUMBER(SUBSTR(b.master_type,2,7))= 0   and b.type = 'AGM'  INNER JOIN master_setting c ON SUBSTR(a.master_type,1,3)= SUBSTR(c.master_type, 1, 3)  AND TO_NUMBER(SUBSTR(c.master_type,4,7))= 0   and c.type = 'AGM'  where TO_NUMBER(SUBSTR(a.master_type,4,5))> 0 AND TO_NUMBER(SUBSTR(a.master_type,6,7))= 0 and a.type = 'AGM' and a.client_id='" + clientid_mst + "' and a.client_unit_id='" + unitid_mst + "'  and a.client_id=c.client_id and  a.client_unit_id = c.client_unit_id";
                    q4 = "SELECT  (a.client_id|| a.client_unit_id|| a.vch_num|| TO_CHAR(a.vch_date, 'YYYYMMDD')|| a.type) as fstr,a.master_type as Financial_Category_Code ,b.master_name AS Financial_Category,c.master_name as Main_Head,d.master_name as Sub_Head,a.master_name as Sub_Sub_Head  FROM master_setting a INNER JOIN master_setting b ON SUBSTR(a.master_type, 1, 1) = SUBSTR(b.master_type, 1, 1)  AND TO_NUMBER(SUBSTR(b.master_type,2,7))= 0   and b.type = 'AGM'  INNER JOIN master_setting c ON SUBSTR(a.master_type,1,3)= SUBSTR(c.master_type, 1, 3)  AND TO_NUMBER(SUBSTR(c.master_type,4,7))= 0   and c.type = 'AGM'  INNER JOIN master_setting d ON SUBSTR(a.master_type,1,5)= SUBSTR(d.master_type, 1, 5)  AND TO_NUMBER(SUBSTR(d.master_type,6,7))= 0   and d.type = 'AGM'  where TO_NUMBER(SUBSTR(a.master_type,6,7))> 0  and a.type = 'AGM' and a.client_id='" + clientid_mst + "' and a.client_unit_id='" + unitid_mst + "'  and a.client_id=c.client_id and a.client_unit_id= c.client_unit_id and a.client_id=d.client_id and a.client_unit_id=d.client_unit_id";

                    switch (btnval.ToUpper())
                    {

                        case "VIEW":
                        case "EDIT":
                        case "EXT":
                            mq = q1 + " union   " + q2 + " union " + q3 + " union " + q4;
                            cmd = "Select (b.vch_num|| to_char(b.vch_date, 'YYYYMMDD')|| type ) as fstr," +
                                " a.Financial_Category_Code,a.Financial_Category,a.Main_Head,a.Sub_Head,a.Sub_Sub_Head" +
                                ",b.c_name as ledger_name,b.CPNAME as Ledger_Name_Abbrevation,b.vch_num as ledger_code,b.CP_MNAME as tally_name" +
                        ",b.ref_code as vch_num from (" + mq + ") a inner join clients_mst b on a.Financial_Category_Code=b.CPLANDNO" +
                                " where find_in_set(b.client_id,'" + clientid_mst + "')=1 and find_in_set(client_unit_id,'" + unitid_mst + "')=1 and type='LDG'";
                            break;
                        case "GRP":
                            cmd = q1 + " union   " + q2 + " union " + q3 + " union " + q4;
                            break;
                        case "UNIT":
                            cmd = "select (cup_id||company_profile_id||to_char(vch_date,'yyyymmdd')) fstr,company_profile_id Company_d,(unit_name||' ('||cup_id||')') " +
                                "Unit_Name from company_unit_profile where unit_status='1' order by unit_name asc";
                            break;
                    }
                    break;
                #endregion
                #region bank_ac
                case "bank_ac":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = "select (cm.client_id||cm.client_unit_id|| cm.vch_num|| to_char(cm.vch_date, 'yyyymmdd')||cm.type )as fstr,cm.vch_num as account_code,nvl(a.master_name,'-') as bank_name ,cm.cpname as Bank_Acc_No,cm.REFERED_BY " +
                                "as Branch_Name,cm.remark as bnk_Address,cm.msmeno as IFSC,cm.tanno as MICR" +
                                ",cm.cpdesig as Swift_code ,(case when cm.bnk='Y' then cm.bnk else '-' end) as Fwd_Link,cm.cpaddr as Contect_person,cm.Cpaltcont as" +
                                " Mob_no,cm.cpemail as Email,cm.Addr as Department,cm.C_name as designation,nvl(cr.master_name,'-') as cur_type from clients_mst cm inner join master_setting a on a.master_id = cm.ptype and " +
                                "a.type = 'ABD' and find_in_set(a.client_unit_id,cm.client_unit_id)=1 left join master_setting cr on cr.master_id = cm.Panno and " +
                                "cr.type = 'CTM' and cr.client_unit_id = cm.client_unit_id  where cm.type = 'BAD' and " +
                                "cm.client_unit_id = '" + unitid_mst + "'";
                            break;

                        case "BANK_NAME":
                            cmd = "select (a.client_id||a.client_unit_id||a.master_id|| to_char(a.vch_date,'yyyymmdd')||a.type) as fstr," +
                            " master_name as Bank_Name from  master_setting  a where a.type='ABD' and a.client_id='" + clientid_mst + "'" +
                               " and a.client_unit_id='" + unitid_mst + "'";
                            break;

                        case "ADD1":
                            //cmd = "SELECT client_id||client_unit_id||vch_num||to_char(vch_date, 'yyyymmdd')||type as fstr,v_name as village,teh_name as tehsil,district_name," +
                            //    "state_gst_code,state_name,country_name,std_code,alpha_2,alpha_3,country_code,region FROM country_state where type = 'CSV' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";

                            cmd = "select distinct alpha_2||state_gst_code ||city_name as fstr," +
                            "country_name,state_name ,city_name from country_state order by state_name desc ";
                            break;

                    }
                    break;
                #endregion
                #region gst_rpt
                case "gst_rpt":
                    switch (btnval.ToUpper())
                    {
                        case "TBDETAIL":
                            cmd = "select (t.client_unit_id||t.acode) fstr,t.vch_num,nvl(to_char(t.vch_date,'" + sgen.Getsqldateformat() + "'),'01/01/1900') vch_date,t.acode LedgerCode," +
                                "c.c_name Ledger,(case when substr(t.type,1,1)='1' and t.type<>'10' then 'Receipt' when substr(t.type,1,1)='1' and t.type='10' then 'Cash' when substr(t.type,1,1)='2' " +
                                "then 'Payment' when substr(t.type,1,1)='3' then 'Journal' when substr(t.type,1,1)='4' then 'Sales' when substr(t.type,1,1)='5' then 'Purchase' else 'OP' end) Type," +
                                "t.dramount DrAmount,t.cramount CrAmount from " +
                                "(select client_unit_id, vch_num as acode, '0' as vch_num, null as vch_date, to_number(opbal) as op,'OP' type, 0 as dramount, 0 as cramount from tally_clients_mst " +
                                "where type = 'BCD' " +
                                "union all " +
                                "select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, sum(to_number(dramount)) - sum(to_number(cramount)) as op,type,0 as dramount," +
                                " 0 as cramount from tally_vouchers where vch_date < to_date('" + param2.Split(new string[] { "!~!~!~!~!" }, StringSplitOptions.None)[0].Trim() + "','" + sgen.Getsqldateformat() + "') - 1 " +
                                "group by client_unit_id, acode,type " +
                                "union all " +
                                "select client_unit_id, acode as acode, vch_num, vch_date, 0 as op,type, to_number(dramount), to_number(cramount) from tally_vouchers " +
                                "where vch_date between to_date('" + param2.Split(new string[] { "!~!~!~!~!" }, StringSplitOptions.None)[0].Trim() + "','" + sgen.Getsqldateformat() + "') and " +
                                "to_date('" + param2.Split(new string[] { "!~!~!~!~!" }, StringSplitOptions.None)[1].Trim() + "','" + sgen.Getsqldateformat() + "')) t " +
                                "inner join tally_clients_mst c on c.vch_num = t.acode and c.type = 'BCD' where (t.client_unit_id || t.acode) = '" + param1 + "'";
                            break;
                        case "CFDETAIL":
                            cmd = "select (t.client_unit_id||t.acode) fstr,t.vch_num,nvl(to_char(t.vch_date,'" + sgen.Getsqldateformat() + "'),'01/01/1900') vch_date,t.acode LedgerCode," +
                                "c.c_name Ledger,(case when substr(t.type,1,1)='1' and t.type<>'10' then 'Receipt' when substr(t.type,1,1)='1' and t.type='10' then 'Cash' when substr(t.type,1,1)='2' " +
                                "then 'Payment' when substr(t.type,1,1)='3' then 'Journal' when substr(t.type,1,1)='4' then 'Sales' when substr(t.type,1,1)='5' then 'Purchase' else 'OP' end) Type," +
                                "t.dramount DrAmount,t.cramount CrAmount from " +
                                "(select client_unit_id, vch_num as acode, '0' as vch_num, null as vch_date, to_number(opbal) as op,'OP' type, 0 as dramount, 0 as cramount from tally_clients_mst " +
                                "where type = 'BCD' " +
                                "union all " +
                                "select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, sum(to_number(dramount)) - sum(to_number(cramount)) as op,type,0 as dramount," +
                                " 0 as cramount from tally_vouchers where vch_date < to_date('" + param2.Split(new string[] { "!~!~!~!~!" }, StringSplitOptions.None)[0].Trim() + "','" + sgen.Getsqldateformat() + "') - 1 " +
                                "group by client_unit_id, acode,type " +
                                "union all " +
                                "select client_unit_id, acode as acode, vch_num, vch_date, 0 as op,type, to_number(dramount), to_number(cramount) from tally_vouchers " +
                                "where vch_date between to_date('" + param2.Split(new string[] { "!~!~!~!~!" }, StringSplitOptions.None)[0].Trim() + "','" + sgen.Getsqldateformat() + "') and " +
                                "to_date('" + param2.Split(new string[] { "!~!~!~!~!" }, StringSplitOptions.None)[1].Trim() + "','" + sgen.Getsqldateformat() + "')) t " +
                                "inner join tally_clients_mst c on c.vch_num = t.acode and c.type = 'BAD' where (t.client_unit_id || t.acode) = '" + param1 + "'";
                            break;
                        case "DBDETAIL":
                        case "SRDETAIL":
                        case "PRDETAIL":
                        case "JRDETAIL":
                        case "PAYDETAIL":
                        case "RRDETAIL":
                        case "CRDETAIL":
                            if (btnval.Trim().ToUpper() == "DBDETAIL") btnvou = "DBVOU";
                            else if (btnval.Trim().ToUpper() == "SRDETAIL") btnvou = "SRVOU";
                            else if (btnval.Trim().ToUpper() == "PRDETAIL") btnvou = "PRVOU";
                            else if (btnval.Trim().ToUpper() == "JRDETAIL") btnvou = "JRVOU";
                            else if (btnval.Trim().ToUpper() == "PAYDETAIL") btnvou = "PAYVOU";
                            else if (btnval.Trim().ToUpper() == "RRDETAIL") btnvou = "RRVOU";
                            else if (btnval.Trim().ToUpper() == "CRDETAIL") btnvou = "CRVOU";
                            cmd = "select (v.client_unit_id||v.vch_num||to_char(v.vch_date,'yyyymmdd')||v.type) fstr,v.acode LedgerCode,c.c_name LedgerName,v.rvscode ReverseCode,v.vch_num VoucherNo," +
                                "TO_CHAR(v.vch_date,'dd/mm/yyyy') VoucherDate, v.type,to_number(v.dramount) DrAmount,to_number(v.cramount) CrAmount from vouchers v " +
                                "inner join clients_mst c on c.vch_num = v.acode and c.type = 'BCD' " +
                                "where (v.client_unit_id||v.vch_num||to_char(v.vch_date,'yyyymmdd')||v.type) = '" + param1.Trim() + "' and (to_number(v.cramount) + to_number(v.dramount)) > 0";
                            sgen.SetSession(MyGuid, "btnval", btnvou);
                            break;
                        case "BBOOK":
                        case "CBOOK":
                        case "SBOOK":
                        case "PBOOK":
                        case "JBOOK":
                            if (btnval.Trim().ToUpper() == "BBOOK") btnvou = "BVOU";
                            else if (btnval.Trim().ToUpper() == "CBOOK") btnvou = "CVOU";
                            else if (btnval.Trim().ToUpper() == "SBOOK") btnvou = "SVOU";
                            else if (btnval.Trim().ToUpper() == "PBOOK") btnvou = "PVOU";
                            else if (btnval.Trim().ToUpper() == "JBOOK") btnvou = "JVOU";
                            cmd = "select (v.client_unit_id||v.vch_num||to_char(v.vch_date,'yyyymmdd')||v.type) fstr,v.vch_num VoucherNo,TO_CHAR(v.vch_date,'dd/mm/yyyy') VoucherDate," +
                                "v.acode LedgerCode,c.c_name LedgerName,v.type,to_number(v.dramount) DrAmount,to_number(v.cramount) CrAmount from vouchers v " +
                                "inner join clients_mst c on c.vch_num = v.acode and c.type = 'BCD' " +
                                "where(v.client_unit_id || v.vch_num || v.type) = '" + param1.Trim() + "' and (to_number(v.cramount) + to_number(v.dramount)) > 0";
                            sgen.SetSession(MyGuid, "btnval", btnvou);
                            break;
                    }
                    break;
                #endregion
                #region create reciept money
                case "rcm":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                        case "PRINT":
                            //cmd = "select (cm.client_unit_id|| cm.vch_num|| to_char(cm.vch_date, 'yyyymmdd')||cm.type )as fstr,nvl(a.master_name,'-') as bank_name " +
                            //    ",cm.cpname as  Bank_Acc_No,cm.REFERED_BY as Branch_Name,cm.remark as bnk_Address,cm.msmeno as IFSC from clients_mst cm " +
                            //    "left join master_setting a on a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id where cm.type = 'BAD' " +
                            //    "and cm.client_unit_id = '" + unitid_mst + "'";
                            //cmd = "select distinct (v.client_unit_id|| v.vch_num|| to_char(v.vch_date, 'yyyymmdd')||v.type) as fstr,v.vch_num as doc_no,to_char(v.vch_date,'" + sgen.Getsqldateformat() + "') as doc_date " +
                            //    ",cl.c_name as party_name from vouchers v inner join clients_mst cl on cl.vch_num=v.acode and cl.client_unit_id=v.client_unit_id where v.type = '11' and v.type1='BLKRCPT' and v.client_unit_id = '" + unitid_mst + "'";
                            //cmd = "select distinct (v.client_unit_id|| v.vch_num|| to_char(v.vch_date, 'yyyymmdd')||v.type) as fstr,v.vch_num as doc_no,to_char(v.vch_date,'" + sgen.Getsqldateformat() + "') as doc_date " +
                            //   ",v.aname as party,v.chqno,to_char(v.chqdate,'" + sgen.Getsqldateformat() + "') as cheqdate,(case when adj_type='ADV' then 'Advance' when adj_type='AGR' then 'Against Ref' when adj_type='NEW' then 'New Ref' when adj_type='OAC' then 'On Account' else 'Not Assingned' end) Adjustment_method,v.chqamt from vouchers v where v.type = '11' and v.type1='BLKRCPT' and v.client_unit_id = '" + unitid_mst + "'  order by v.vch_num desc";

                            cmd = "select (v.client_unit_id|| v.vch_num|| to_char(v.vch_date, 'yyyymmdd')||v.type) as fstr,v.vch_num as doc_no,to_char(v.vch_date,'" + sgen.Getsqldateformat() + "') as doc_date" +
                                  ",v.aname as party,sum(v.dramount) as dramount,sum(v.cramount) as cramount,(case when adj_type= 'ADV' then 'Advance' when adj_type = 'AGR' " +
                                  "then 'Against Ref' when adj_type = 'NEW' then 'New Ref' when adj_type = 'OAC' then 'On Account' else 'Not Assingned' end) Adjustment_method" +
                                  " from vouchers v where v.type = '11' and v.type1='BLKRCPT' and v.client_unit_id = '" + unitid_mst + "' group by v.client_unit_id,v.vch_num,v.vch_date,v.type,v.aname,v.adj_type order by v.vch_num desc";

                            break;
                        case "BANK":
                            cmd = "select (cm.client_unit_id|| cm.vch_num|| to_char(cm.vch_date, 'yyyymmdd')||cm.type )as fstr,nvl(a.master_name,'-') as bank_name " +
                                ",cm.cpname as  Bank_Acc_No,cm.REFERED_BY as Branch_Name,cm.remark as bnk_Address,cm.msmeno as IFSC from clients_mst cm " +
                                "left join master_setting a on a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id where cm.type = 'BAD' " +
                                "and cm.client_unit_id = '" + unitid_mst + "'";
                            break;
                        case "PARTY":
                            //                            cmd = "select distinct (a.vch_num|| to_char(a.vch_date,'yyyymmdd')||a.type) as fstr,a.vch_num as Party_code," +
                            //                        "a.c_name as Name,a.addr address,a.pincode as Pincode,a.type_desc as Search_text,(case when a.cpcont='0000000000' then '-' else a.cpcont end) ContactNo,(case when a.cpemail='ab@ab.ab' then '-' else a.cpemail end) as Email,nvl(p.closing,'0') as closing from clients_mst a" +
                            //                        " LEFT JOIN (SELECT ACODE,CLIENT_UNIT_ID,SUM(OPBAL) AS OPBAL,SUM(DRAMOUNT),SUM(CRAMOUNT),SUM(OPBAL)+SUM(DRAMOUNT)-SUM(CRAMOUNT) as closing from (select acode,CLIENT_UNIT_ID, OP_" + year + " as opbal,0 as cramount,0 as dramount,0 as cl from acbal union all select acode,CLIENT_UNIT_ID,sum(dramount)-sum(cramount) as opbal" +
                            //",0 as cramount,0 as dramount,0 as cl from vouchers where type in ('11','10','21','20') and to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and " +
                            //                                  "to_date('" + year_from.Split(' ')[0].Trim() + "','" + sgen.Getsqldateformat() + "')-1 group by acode,CLIENT_UNIT_ID union all select acode,CLIENT_UNIT_ID,0 as opbal,TO_NUMBER(cramount) cramount,TO_NUMBER(dramount) dramount,0 cl from vouchers " +
                            //                                  "where type in ('11','10','21','20') and to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                            //                                  "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + param1.Trim() + "','" + sgen.Getsqldateformat() + "')) group by acode,CLIENT_UNIT_ID) p on p.acode=a.vch_num and p.client_unit_id=a.client_unit_id where find_in_set(a.client_unit_id, '" + unitid_mst + "')=1 AND a.type = 'BCD' and substr(a.vch_num,0,3) in ('303','203')";
                            cmd = "select (a.acode|| to_char(a.vch_date,'yyyymmdd')||a.type) as fstr,a.acode,a.aname as ledger_name,nvl(p.closing,'0') as closing from " +
                                  "(select vch_num as acode,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'BCD' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                  "union all select vch_num as acode ,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                  "union all select cm.vch_num as acode,nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname,cm.CLIENT_UNIT_ID,cm.vch_date,cm.type from master_setting a inner join clients_mst cm on " +
                                  "a.master_id = cm.ptype and a.type = 'ABD' and find_in_set(a.client_unit_id, cm.client_unit_id)=1 where cm.type = 'BAD' and find_in_set(cm.client_unit_id,'" + unitid_mst + "')=1) a " +
                                  "LEFT JOIN (SELECT ACODE,CLIENT_UNIT_ID,SUM(OPBAL) AS OPBAL,SUM(DRAMOUNT),SUM(CRAMOUNT),SUM(OPBAL)+SUM(DRAMOUNT)-SUM(CRAMOUNT) as closing from " +
                                  "(select acode,CLIENT_UNIT_ID, OP_" + year + " as opbal,0 as cramount,0 as dramount,0 as cl from acbal union all select acode,CLIENT_UNIT_ID,sum(dramount)-sum(cramount) as opbal" +
  ",0 as cramount,0 as dramount,0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and " +
                                    "to_date('" + year_from.Split(' ')[0].Trim() + "','" + sgen.Getsqldateformat() + "')-1 group by acode,CLIENT_UNIT_ID union all select acode,CLIENT_UNIT_ID,0 as opbal,TO_NUMBER(cramount) cramount,TO_NUMBER(dramount) dramount,0 cl from vouchers " +
                                    "where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                                    "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + param1.Trim() + "','" + sgen.Getsqldateformat() + "')) group by acode,CLIENT_UNIT_ID) p on p.acode=a.acode and p.client_unit_id=a.client_unit_id where find_in_set(a.client_unit_id, '" + unitid_mst + "')=1";
                            break;
                    }
                    break;
                #endregion
                #region sale_voucher
                case "sl_voucher":
                    switch (btnval.ToUpper())
                    {
                        case "PRINT":
                        case "VIEW":
                        case "EDIT":
                            type = "";
                            if (sgen.GetSession(MyGuid, "SLVOUTYPE") != null)
                            {
                                type = "" + sgen.GetSession(MyGuid, "SLVOUTYPE").ToString() + "";
                                if (type == "50")
                                {
                                    type = "01,02,03,04,05,50";
                                }
                            }
                            else
                            {
                                type = "VOU','ESP";
                            }
                            cmd = "select (vu.client_unit_id||vu.vch_num|| to_char(vu.vch_date, 'yyyymmdd')|| vu.type) as fstr,vu.vch_num as doc_num,to_char(vu.vch_date, 'dd/MM/YYYY') doc_date,vu.type, vu.invno as ref_no," +
                                "to_char(vu.invdate, 'dd/MM/YYYY') as ref_date,vu.acode as party_code,vu.rvscode as reverse_code,nvl(cl.c_name, '-') party_name,vu.dramount,vu.cramount,vu.totremark as remark from vouchers vu " +
                                "inner join clients_mst cl on cl.vch_num = vu.acode and find_in_set(vu.client_unit_id, cl.client_unit_id) = 1 where vu.type in ('" + type.Trim().TrimEnd(',').Replace(",", "','").Trim().ToString() + "') " +
                                "and vu.client_unit_id = '" + unitid_mst + "' and (vu.cramount + vu.dramount) > 0 order by vu.vch_num desc";
                            break;
                        case "PARTY":
                            //cmd = "select a.vch_num||a.type as fstr,a.c_name as Client_name,a.addr as address,a.c_gstin as GSTIN," +
                            //    "(case when a.tor = 'R' then 'Regular' when a.tor='C' then 'Composition' else 'Not Registered' END) GSTType," +
                            //    "a.type_desc as Search_text from clients_mst a  where a.type = 'BCD' and substr(a.vch_num,0,3)='203' and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "' " +
                            //    "union all select a.vch_num || a.type as fstr,ledger_name Client_name, ledger_abb address,to_char(group_code) GSTIN, " +
                            //    "'-' GSTType,'-' Search_text from ledger_master a where a.type = 'ALM' and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                            cmd = "select (a.acode|| to_char(a.vch_date,'yyyymmdd')||a.type) as fstr,a.acode,a.aname as ledger_name from " +
                                  "(select vch_num as acode,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'BCD' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                  "union all select vch_num as acode ,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                  "union all select cm.vch_num as acode,nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname,cm.CLIENT_UNIT_ID,cm.vch_date,cm.type from master_setting a inner join clients_mst cm on " +
                                  "a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id where cm.type = 'BAD' and cm.client_unit_id = '" + unitid_mst + "') a ";
                            break;
                    }
                    break;
                #endregion
                #region party_opening
                case "party_opening":
                    string where = "";
                    string mq2 = "";
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = "select (i.client_unit_id||i.vch_num||to_char(i.vch_date,'yyyymmdd')||i.type) as fstr,i.vch_num Docno,to_char(i.vch_date, '" + sgen.Getsqldateformat() + "') doc_date," +
                                "i.acode,i.OP_2020 as Opening_amount from acbal i where i.type = 'AC' and i.client_unit_id = '" + unitid_mst + "' " + xprdrange.Replace("vch_date", "i.vch_date") + "";
                            break;
                        case "ITEM":
                            if (param1 != "") where = " and (a.acode||to_char(a.vch_date,'yyyymmdd')||a.type) not in ('" + param1.Trim().TrimEnd(',').Replace(",", "','") + "')";
                            mq1 = "select (a.acode|| to_char(a.vch_date,'yyyymmdd')||a.type) as fstr,a.acode,a.aname as ledger_name from " +
                                   "(select vch_num as acode,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'BCD' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                   "union all select vch_num as acode ,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                   "union all select cm.vch_num as acode,nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname,cm.CLIENT_UNIT_ID,cm.vch_date,cm.type from master_setting a inner join clients_mst cm on " +
                                   "a.master_id = cm.ptype and a.type = 'ABD' and find_in_set(a.client_unit_id, cm.client_unit_id)=1 where cm.type = 'BAD' and find_in_set(cm.client_unit_id,'" + unitid_mst + "')=1) a where a.client_unit_id='" + unitid_mst + "'" + where + "";
                            mq2 = "select itb.acode from acbal itb where itb.type='AC' and acyear='" + year_from.Substring(6, 4) + "'";
                            cmd = "select * from (" + mq1 + ") a where a.acode not in (" + mq2 + ")";
                            break;
                    }
                    break;
                #endregion
                #region pay amount
                case "payamt":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                        case "PRINT":
                            //cmd = "select distinct (v.client_unit_id|| v.vch_num|| to_char(v.vch_date, 'yyyymmdd')||v.type) as fstr,v.vch_num as doc_no,to_char(v.vch_date,'" + sgen.Getsqldateformat() + "') as doc_date " +
                            //   ",v.aname as party,v.chqno,to_char(v.chqdate,'" + sgen.Getsqldateformat() + "') as cheqdate,(case when adj_type='ADV' then 'Advance' when adj_type='AGR' then 'Against Ref' when adj_type='NEW' then 'New Ref' when adj_type='OAC' then 'On Account' else 'Not Assingned' end) Adjustment_method,v.chqamt from vouchers v where v.type = '21' and v.type1='BLKPAY' and v.client_unit_id = '" + unitid_mst + "' order by v.vch_num desc";
                            cmd = "select (v.client_unit_id|| v.vch_num|| to_char(v.vch_date, 'yyyymmdd')||v.type) as fstr,v.vch_num as doc_no,to_char(v.vch_date,'" + sgen.Getsqldateformat() + "') as doc_date" +
                                ",v.aname as party,sum(v.dramount) as dramount,sum(v.cramount) as cramount,(case when adj_type= 'ADV' then 'Advance' when adj_type = 'AGR' " +
                                "then 'Against Ref' when adj_type = 'NEW' then 'New Ref' when adj_type = 'OAC' then 'On Account' else 'Not Assingned' end) Adjustment_method" +
                                " from vouchers v where v.type = '21' and v.client_unit_id = '" + unitid_mst + "' and v.type1='BLKPAY' group by v.client_unit_id,v.vch_num,v.vch_date,v.type,v.aname,v.adj_type order by v.vch_num desc";

                            break;
                        case "BANK":
                            cmd = "select (cm.client_unit_id|| cm.vch_num|| to_char(cm.vch_date, 'yyyymmdd')||cm.type )as fstr,nvl(a.master_name,'-') as bank_name " +
                                ",cm.cpname as  Bank_Acc_No,cm.REFERED_BY as Branch_Name,cm.remark as bnk_Address,cm.msmeno as IFSC from clients_mst cm " +
                                "left join master_setting a on a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id where cm.type = 'BAD' " +
                                "and cm.client_unit_id = '" + unitid_mst + "'";
                            break;
                        case "PARTY":
                            cmd = "select (a.acode|| to_char(a.vch_date,'yyyymmdd')||a.type) as fstr,a.acode,a.aname as ledger_name,nvl(p.closing,'0') as closing from " +
                                  "(select vch_num as acode,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'BCD' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                  "union all select vch_num as acode ,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                  "union all select cm.vch_num as acode,nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname,cm.CLIENT_UNIT_ID,cm.vch_date,cm.type from master_setting a inner join clients_mst cm on " +
                                  "a.master_id = cm.ptype and a.type = 'ABD' and find_in_set(a.client_unit_id, cm.client_unit_id)=1 where cm.type = 'BAD' and find_in_set(cm.client_unit_id,'" + unitid_mst + "')=1) a " +
                                  " LEFT JOIN (SELECT ACODE,CLIENT_UNIT_ID,SUM(OPBAL) AS OPBAL,SUM(DRAMOUNT),SUM(CRAMOUNT),SUM(OPBAL)+SUM(DRAMOUNT)-SUM(CRAMOUNT) as closing from (select acode,CLIENT_UNIT_ID, OP_" + year + " as opbal,0 as cramount,0 as dramount,0 as cl from acbal union all select acode,CLIENT_UNIT_ID,sum(dramount)-sum(cramount) as opbal" +
      ",0 as cramount,0 as dramount,0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and " +
                                        "to_date('" + year_from.Split(' ')[0].Trim() + "','" + sgen.Getsqldateformat() + "')-1 group by acode,CLIENT_UNIT_ID union all select acode,CLIENT_UNIT_ID,0 as opbal,TO_NUMBER(cramount) cramount,TO_NUMBER(dramount) dramount,0 cl from vouchers " +
                                        "where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                                        "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + param1.Trim() + "','" + sgen.Getsqldateformat() + "')) group by acode,CLIENT_UNIT_ID) p on p.acode=a.acode and p.client_unit_id=a.client_unit_id where find_in_set(a.client_unit_id, '" + unitid_mst + "')=1";
                            //                       cmd = "select distinct (a.vch_num|| to_char(a.vch_date,'yyyymmdd')||a.type) as fstr,a.vch_num as Party_code," +
                            //                        "a.c_name as Name,a.addr address,a.pincode as Pincode,a.type_desc as Search_text,(case when a.cpcont='0000000000' then '-' else a.cpcont end) ContactNo,(case when a.cpemail='ab@ab.ab' then '-' else a.cpemail end) as Email,nvl(p.closing,'0') as closing from clients_mst a inner join itransaction pi on a.vch_num" +
                            //                        " = pi.acode and pi.type in ('01','02','03','05','07','08','09') and " +
                            //                        "find_in_set(a.client_unit_id, pi.client_unit_id)=1  LEFT JOIN(SELECT ACODE,CLIENT_UNIT_ID,SUM(OPBAL) AS OPBAL,SUM(DRAMOUNT),SUM(CRAMOUNT),SUM(OPBAL)+SUM(CRAMOUNT)-SUM(DRAMOUNT) as closing from (select acode,CLIENT_UNIT_ID, OP_" + year + " as opbal,0 as cramount,0 as dramount,0 as cl from acbal union all select acode,CLIENT_UNIT_ID,sum(cramount)-sum(dramount) as opbal" +
                            //",0 as cramount,0 as dramount,0 as cl from vouchers where type in ('21','20','10','21') and to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and " +
                            //                                  "to_date('" + year_from.Split(' ')[0].Trim() + "','" + sgen.Getsqldateformat() + "')-1 group by acode,CLIENT_UNIT_ID union all select acode,CLIENT_UNIT_ID,0 as opbal,TO_NUMBER(cramount) cramount,TO_NUMBER(dramount) dramount,0 cl from vouchers " +
                            //                                  "where type in ('21','20','10','21') and to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                            //                                  "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + param1.Trim() + "','" + sgen.Getsqldateformat() + "')) group by acode,CLIENT_UNIT_ID) p on p.acode=a.vch_num and p.client_unit_id=a.client_unit_id where find_in_set(a.client_unit_id, '" + unitid_mst + "')=1 and substr(a.vch_num,0,3)='203'";
                            break;
                    }
                    break;
                #endregion
                #region cash transaction
                case "cash_vou":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                        case "PRINT":
                            if (param1.Trim().Equals("22003.1"))
                            {
                                // cmd = "select distinct (v.client_unit_id|| v.vch_num|| to_char(v.vch_date, 'yyyymmdd')||v.type) as fstr,v.vch_num as doc_no,to_char(v.vch_date,'" + sgen.Getsqldateformat() + "') as doc_date " +
                                //",v.aname as party,(case when adj_type='ADV' then 'Advance' when adj_type='AGR' then 'Against Ref' when adj_type='NEW' then 'New Ref' when adj_type='OAC' then 'On Account' else 'Not Assingned' end) Adjustment_method,v.chqamt as Cash_amount from vouchers v where v.type = '10' and v.type1='CASHRCPT' and v.client_unit_id = '" + unitid_mst + "' order by v.vch_num desc";
                                cmd = "select (v.client_unit_id|| v.vch_num|| to_char(v.vch_date, 'yyyymmdd')||v.type) as fstr,v.vch_num as doc_no,to_char(v.vch_date,'" + sgen.Getsqldateformat() + "') as doc_date" +
                              ",v.aname as party,sum(v.dramount) as dramount,sum(v.cramount) as cramount,(case when adj_type= 'ADV' then 'Advance' when adj_type = 'AGR' " +
                              "then 'Against Ref' when adj_type = 'NEW' then 'New Ref' when adj_type = 'OAC' then 'On Account' else 'Not Assingned' end) Adjustment_method" +
                              " from vouchers v where v.type = '10' and v.client_unit_id = '" + unitid_mst + "' group by v.client_unit_id,v.vch_num,v.vch_date,v.type,v.aname,v.adj_type order by v.vch_num desc";
                            }
                            else
                            {
                                // cmd = "select distinct (v.client_unit_id|| v.vch_num|| to_char(v.vch_date, 'yyyymmdd')||v.type) as fstr,v.vch_num as doc_no,to_char(v.vch_date,'" + sgen.Getsqldateformat() + "') as doc_date " +
                                //",v.aname as party,(case when adj_type='ADV' then 'Advance' when adj_type='AGR' then 'Against Ref' when adj_type='NEW' then 'New Ref' when adj_type='OAC' then 'On Account' else 'Not Assingned' end) Adjustment_method,v.chqamt as Cash_amount from vouchers v where v.type = '20' and v.type1='CASHRCPT' and v.client_unit_id = '" + unitid_mst + "'";
                                cmd = "select (v.client_unit_id|| v.vch_num|| to_char(v.vch_date, 'yyyymmdd')||v.type) as fstr,v.vch_num as doc_no,to_char(v.vch_date,'" + sgen.Getsqldateformat() + "') as doc_date" +
                            ",v.aname as party,sum(v.dramount) as dramount,sum(v.cramount) as cramount,(case when adj_type= 'ADV' then 'Advance' when adj_type = 'AGR' " +
                            "then 'Against Ref' when adj_type = 'NEW' then 'New Ref' when adj_type = 'OAC' then 'On Account' else 'Not Assingned' end) Adjustment_method" +
                            " from vouchers v where v.type = '20' and v.client_unit_id = '" + unitid_mst + "' group by v.client_unit_id,v.vch_num,v.vch_date,v.type,v.aname,v.adj_type order by v.vch_num desc";
                            }
                            break;
                        case "PARTY":
                            if (param2.Trim().Equals("22003.1"))
                            {
                                //                            cmd = "select distinct (a.vch_num|| to_char(a.vch_date,'yyyymmdd')||a.type) as fstr,a.vch_num as Party_code," +
                                //                        "a.c_name as Name,a.addr address,a.pincode as Pincode,a.type_desc as Search_text,(case when a.cpcont='0000000000' then '-' else a.cpcont end) ContactNo,(case when a.cpemail='ab@ab.ab' then '-' else a.cpemail end) as Email,nvl(p.closing,'0') as closing from clients_mst a " +
                                //                        "LEFT JOIN(SELECT ACODE,CLIENT_UNIT_ID,SUM(OPBAL) AS OPBAL,SUM(DRAMOUNT),SUM(CRAMOUNT),SUM(OPBAL)+SUM(DRAMOUNT)-SUM(CRAMOUNT) as closing from (select acode,CLIENT_UNIT_ID, OP_" + year + " as opbal,0 as cramount,0 as dramount,0 as cl from acbal union all select acode,CLIENT_UNIT_ID,sum(dramount)-sum(cramount) as opbal" +
                                //",0 as cramount,0 as dramount,0 as cl from vouchers where type in ('11','10') and to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and " +
                                //                                  "to_date('" + year_from.Split(' ')[0].Trim() + "','" + sgen.Getsqldateformat() + "')-1 group by acode,CLIENT_UNIT_ID union all select acode,CLIENT_UNIT_ID,0 as opbal,TO_NUMBER(cramount) cramount,TO_NUMBER(dramount) dramount,0 cl from vouchers " +
                                //                                  "where type in ('11','10') and to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                                //                                  "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + param1.Trim() + "','" + sgen.Getsqldateformat() + "')) group by acode,CLIENT_UNIT_ID) p on p.acode=a.vch_num and p.client_unit_id=a.client_unit_id where find_in_set(a.client_unit_id, '" + unitid_mst + "')=1 AND a.type = 'BCD' and substr(a.vch_num,0,3) in ('303','203')";

                                cmd = "select (a.acode|| to_char(a.vch_date,'yyyymmdd')||a.type) as fstr,a.acode,a.aname as ledger_name,nvl(p.closing,'0') as closing from " +
                                "(select vch_num as acode,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'BCD' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                "union all select vch_num as acode ,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                "union all select cm.vch_num as acode,nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname,cm.CLIENT_UNIT_ID,cm.vch_date,cm.type from master_setting a inner join clients_mst cm on " +
                                "a.master_id = cm.ptype and a.type = 'ABD' and find_in_set(a.client_unit_id, cm.client_unit_id)=1 where cm.type = 'BAD' and find_in_set(cm.client_unit_id,'" + unitid_mst + "')=1) a " +
                                "LEFT JOIN (SELECT ACODE,CLIENT_UNIT_ID,SUM(OPBAL) AS OPBAL,SUM(DRAMOUNT),SUM(CRAMOUNT),SUM(OPBAL)+SUM(DRAMOUNT)-SUM(CRAMOUNT) as closing from " +
                                "(select acode,CLIENT_UNIT_ID, OP_" + year + " as opbal,0 as cramount,0 as dramount,0 as cl from acbal union all select acode,CLIENT_UNIT_ID,sum(dramount)-sum(cramount) as opbal" +
",0 as cramount,0 as dramount,0 as cl from vouchers where type in ('11','10','21','20') and to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and " +
                                  "to_date('" + year_from.Split(' ')[0].Trim() + "','" + sgen.Getsqldateformat() + "')-1 group by acode,CLIENT_UNIT_ID union all select acode,CLIENT_UNIT_ID,0 as opbal,TO_NUMBER(cramount) cramount,TO_NUMBER(dramount) dramount,0 cl from vouchers " +
                                  "where type in ('11','10','21','20') and to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                                  "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + param1.Trim() + "','" + sgen.Getsqldateformat() + "')) group by acode,CLIENT_UNIT_ID) p on p.acode=a.acode and p.client_unit_id=a.client_unit_id where find_in_set(a.client_unit_id, '" + unitid_mst + "')=1";
                            }
                            else
                            {
                                //cmd = "select distinct (a.vch_num|| to_char(a.vch_date,'yyyymmdd')||a.type) as fstr,a.vch_num as Party_code," +
                                //                        "a.c_name as Name,a.addr address,a.pincode as Pincode,a.type_desc as Search_text,(case when a.cpcont='0000000000' then '-' else a.cpcont end) ContactNo,(case when a.cpemail='ab@ab.ab' then '-' else a.cpemail end) as Email,nvl(p.closing,'0') as closing from clients_mst a LEFT JOIN(SELECT ACODE,CLIENT_UNIT_ID,SUM(OPBAL) AS OPBAL,SUM(DRAMOUNT),SUM(CRAMOUNT),SUM(OPBAL)+SUM(CRAMOUNT)-SUM(DRAMOUNT) as closing from (select acode,CLIENT_UNIT_ID, OP_" + year + " as opbal,0 as cramount,0 as dramount,0 as cl from acbal union all select acode,CLIENT_UNIT_ID,sum(cramount)-sum(dramount) as opbal" +
                                //",0 as cramount,0 as dramount,0 as cl from vouchers where type in ('21','20') and to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and " +
                                //                                  "to_date('" + year_from.Split(' ')[0].Trim() + "','" + sgen.Getsqldateformat() + "')-1 group by acode,CLIENT_UNIT_ID union all select acode,CLIENT_UNIT_ID,0 as opbal,TO_NUMBER(cramount) cramount,TO_NUMBER(dramount) dramount,0 cl from vouchers " +
                                //                                  "where type in ('21','20') and to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                                //                                  "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + param1.Trim() + "','" + sgen.Getsqldateformat() + "')) group by acode,CLIENT_UNIT_ID) p on p.acode=a.vch_num and p.client_unit_id=a.client_unit_id where find_in_set(a.client_unit_id, '" + unitid_mst + "')=1 and substr(a.vch_num,0,3) in ('303','203')";

                                cmd = "select (a.acode|| to_char(a.vch_date,'yyyymmdd')||a.type) as fstr,a.acode,a.aname as ledger_name,nvl(p.closing,'0') as closing from " +
                                "(select vch_num as acode,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'BCD' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                "union all select vch_num as acode ,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                "union all select cm.vch_num as acode,nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname,cm.CLIENT_UNIT_ID,cm.vch_date,cm.type from master_setting a inner join clients_mst cm on " +
                                "a.master_id = cm.ptype and a.type = 'ABD' and find_in_set(a.client_unit_id, cm.client_unit_id)=1 where cm.type = 'BAD' and find_in_set(cm.client_unit_id,'" + unitid_mst + "')=1) a " +
                                "LEFT JOIN (SELECT ACODE,CLIENT_UNIT_ID,SUM(OPBAL) AS OPBAL,SUM(DRAMOUNT),SUM(CRAMOUNT),SUM(OPBAL)+SUM(DRAMOUNT)-SUM(CRAMOUNT) as closing from " +
                                "(select acode,CLIENT_UNIT_ID, OP_" + year + " as opbal,0 as cramount,0 as dramount,0 as cl from acbal union all select acode,CLIENT_UNIT_ID,sum(dramount)-sum(cramount) as opbal" +
  ",0 as cramount,0 as dramount,0 as cl from vouchers where type in ('21','11','10','20') and to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and " +
                                  "to_date('" + year_from.Split(' ')[0].Trim() + "','" + sgen.Getsqldateformat() + "')-1 group by acode,CLIENT_UNIT_ID union all select acode,CLIENT_UNIT_ID,0 as opbal,TO_NUMBER(cramount) cramount,TO_NUMBER(dramount) dramount,0 cl from vouchers " +
                                  "where type in ('21','11','10','20') and to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                                  "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + param1.Trim() + "','" + sgen.Getsqldateformat() + "')) group by acode,CLIENT_UNIT_ID) p on p.acode=a.acode and p.client_unit_id=a.client_unit_id where find_in_set(a.client_unit_id, '" + unitid_mst + "')=1";
                            }
                            break;
                    }
                    break;
                #endregion
                #region tally_group
                case "tly_gmap":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = "select (v.client_unit_id|| v.vch_num|| to_char(v.vch_date, 'yyyymmdd')||v.type) as fstr,v.vch_num as doc_no,to_char(v.vch_date,'" + sgen.Getsqldateformat() + "') as doc_date " +
                               ",v.col3 as Group_name,group_concat(tc.c_name) as Mapped_groups from enx_tab v inner join tally_clients_mst tc on find_in_set(tc.vch_num,v.col2)=1 and tc.type='GRP' where v.type = 'TGM' and v.client_unit_id = '" + unitid_mst + "' group by v.client_unit_id,v.type,v.vch_num,v.vch_date,v.col3";
                            break;
                    }
                    break;
                #endregion
                #region tally_company_mapping
                case "tly_cmap":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = "select (v.client_unit_id|| v.vch_num|| to_char(v.vch_date, 'yyyymmdd')||v.type) as fstr,v.vch_num as doc_no,to_char(v.vch_date,'" + sgen.Getsqldateformat() + "') as doc_date " +
                               ",v.col1 as MY_Companies,v.col3 as Tally_Companies from enx_tab v where v.type = 'TCM' and v.client_unit_id = '" + unitid_mst + "'";
                            break;
                    }
                    break;
                #endregion
                #region accReps
                case "accReps":
                    switch (btnval.ToUpper())
                    {
                        case "PDSP":
                            cmd = "select '-' as fstr,nvl(iv.invoice_no,'-') as invoice_no,nvl(to_char(iv.invoice_dt, '" + sgen.Getsqldateformat() + "'),'-') as invoice_dt," +
                                "dp.acode,cl.c_name as party_name,dp.type2 ord_type,dp.icode,dp.vch_num as dsp_no,to_char(dp.vch_date,'" + sgen.Getsqldateformat() + "') as dsp_date,dp.qtychl as disp_qty," +
                                "nvl(iv.invoice_qty,'0') invoice_qty,dp.qtychl - nvl(iv.invoice_qty, '0') as pending_disp from itransaction dp " +
                                "left join (select a.invoice_no,a.invoice_dt,a.acode,a.ord_type,a.icode,a.dspno,a.dspdate,sum(a.qtyout) as invoice_qty,a.client_id," +
                                "a.client_unit_id from(select vch_num invoice_no, vch_date invoice_dt, acode,type as ord_type,icode,dspno,dspdate," +
                                "qtyout,client_id,client_unit_id from itransaction where potype = '53') a group by (a.invoice_no, a.invoice_dt, a.acode," +
                                "a.ord_type,a.icode,a.dspno,a.dspdate,a.client_id,a.client_unit_id)) iv on dp.vch_num = iv.dspno and dp.icode = iv.icode " +
                                "and dp.acode = iv.acode and iv.ord_type = dp.type2 and dp.client_unit_id=iv.client_unit_id and dp.vch_date = iv.dspdate " +
                                "inner join clients_mst cl on cl.vch_num = dp.acode and cl.type = 'BCD' and find_in_set(cl.client_unit_id,dp.client_unit_id)=1 " +
                                "and substr(cl.vch_num,0,3)='303' where substr(dp.type,1,1)= '6' and dp.client_unit_id = '" + unitid_mst + "'";
                            break;
                        case "ISUM":
                            cmd = "select (nvl(t.client_unit_id,'0')||i.icode) as fstr,i.icode Item_Code,gd.master_name Maingrp,mg.master_name Itemgrp,sg.iname Itemsubgrp," +
                                "i.iname Item_Name,i.cpartno PartNo,um.master_name as UOM,sum(t.qtyin) as Received,sum(t.qtyout) Issued,sum(t.qtyin) - sum(t.qtyout) Closing," +
                                "round(case when sum(t.qtyin)- sum(t.qtyout) = 0 then 0 else (sum(t.iamount) / (sum(t.qtyin) - sum(t.qtyout))) end) Rate," +
                                "sum(t.iamount) Amount from item i " +
                                "left join(select client_id, client_unit_id, icode, iname, to_number(nvl(qtyin,0)) qtyin,to_number(nvl(qtyout, 0)) qtyout," +
                                "(case when to_number(nvl(qtyin, 0)) = 0 then - to_number(nvl(iamount, 0)) else to_number(nvl(iamount, 0)) end) iamount from itransaction " +
                                "where length(icode) > 5 and store = 'Y') t on t.icode = i.icode and t.client_unit_id = i.client_unit_id " +
                                "inner join master_setting gd on gd.classid = SUBSTR(i.icode, 1, 1) and gd.type = 'KGP' " +
                                "inner join master_setting mg on mg.master_id = substr(i.icode, 1, 3) and mg.type = 'KIG' and find_in_set(mg.client_unit_id,i.client_unit_id)=1 " +
                                "inner join item sg on sg.icode = substr(i.icode, 1, 5) and sg.type = 'SG' and find_in_set(sg.client_unit_id,i.client_unit_id)=1 " +
                                "inner join master_setting um on um.master_id = i.uom and um.type = 'UMM' and find_in_set(um.client_unit_id, i.client_unit_id)= 1 " +
                                "where length(i.icode)> 5 and find_in_set(i.client_unit_id, '" + unitid_mst + "')= 1 " +
                                "group by nvl(t.client_unit_id, '0'),i.icode,gd.master_name,mg.master_name,sg.iname,i.iname,um.master_name,i.cpartno order by i.icode";
                            break;
                        case "IDET":
                            cmd = "select t.icode fstr,t.icode Item_Code,i.iname Item_Name,t.vch_num DocNo,to_char(t.vch_date,'" + sgen.Getsqldateformat() + "') Dated," +
                            "(case when substr(t.type,1,1)='Q' then substr(t.type,2,2) else t.type end) Type,nvl(m.master_name,'-') TypeDesc,t.qtyin Received,t.qtyout Issued," +
                            "t.irate Rate,t.iamount Amount,nvl(d.master_name,'-') Department,nvl(t.pktno,0) pktno from itransaction t " +
                            "inner join item i on t.icode=i.icode and find_in_set(i.client_unit_id,t.client_unit_id)=1 " +
                            "left join master_setting d on d.master_id=t.deptno and d.Type='MDP' and find_in_set(d.client_unit_id,t.client_unit_id)=1 " +
                            "left join (select master_id,master_name,client_id from master_setting where type in ('KMR','KSR','KIS','KRG','KPV','KPO','KDS','KPD','KRQ','KDP')) m " +
                            "on m.master_id=(case when substr(t.type,1,1)='Q' then substr(t.type,2,2) else t.type end) and m.client_id=t.client_id " +
                            "where concat(t.client_unit_id,t.icode)='" + param1 + "' and t.store='Y' and to_date(to_char(t.vch_date,'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldateformat() + "') between " +
                            "to_date('" + param2.Split(new string[] { "!~!~!~!~!" }, StringSplitOptions.None)[0].Trim() + "','" + sgen.Getsqldateformat() + "') and " +
                            "to_date('" + param2.Split(new string[] { "!~!~!~!~!" }, StringSplitOptions.None)[1].Trim() + "','" + sgen.Getsqldateformat() + "') order by t.vch_date desc";
                            break;
                        case "IFIFO":
                        case "ILEDGER":
                            cmd = "select (it.icode||it.client_id||it.client_unit_id) fstr,it.icode,it.iname,it.cpartno,um.master_name Primary_Unit from item it " +
                                "inner join master_setting um on it.uom = um.master_id and um.type = 'UMM' and find_in_set(um.client_unit_id, it.client_unit_id) = 1 " +
                                "where it.type = 'IT' and find_in_set(it.client_unit_id, '" + unitid_mst + "')=1 and it.status = 'Y' and substr(icode,1,1) not in ('8', '9')";
                            break;
                        case "CISUM":
                            cmd = "select '-' fstr,icode,iname,uom,sum(to_number(qtyin)) qtyin,sum(to_number(qtyout)) qtyout,sum(to_number(qtyin))-sum(to_number(qtyout)) closing,client_id,client_unit_id from " +
    "(select lc.icode, i.iname, um.master_name uom, ml.master_name loc, nvl(lc.qtyin, 0) qtyin, nvl(lc.qtyout, 0) qtyout,(case when lc.pono = '0' then 'C' else lc.pono end) store," +
    "lc.type,lc.client_id,lc.client_unit_id from btchtrans lc " +
    "inner join item i on i.icode = lc.icode and i.type = 'IT' and find_in_set(i.client_unit_id,lc.client_unit_id)=1 " +
    "inner join master_setting um on um.master_id = i.uom and um.type = 'UMM' and find_in_set(um.client_unit_id, lc.client_unit_id)= 1 " +
    "inner join master_setting ml on ml.master_id = lc.loc and ml.type = 'LC6' and ind_in_set(ml.client_unit_id,lc.client_unit_id)=1 " +
    "where length(lc.icode)>5 and lc.store = 'Y') a " +
    "group by icode, iname, uom, client_id, client_unit_id order by icode asc";
                            break;
                        case "LOCISUM":
                            cmd = "select '-' fstr,icode,iname,uom,loc,sum(to_number(qtyin)) qtyin,sum(to_number(qtyout)) qtyout,sum(to_number(qtyin))-sum(to_number(qtyout)) closing,store,client_id,client_unit_id " +
                                "from (select lc.icode, i.iname, um.master_name uom, ml.master_name loc, nvl(lc.qtyin, 0) qtyin, nvl(lc.qtyout, 0) qtyout, (case when lc.pono = '0' then 'C' else lc.pono end) store," +
                                "lc.type,lc.client_id,lc.client_unit_id from btchtrans lc " +
                                "inner join item i on i.icode = lc.icode and i.type = 'IT' and find_in_set(i.client_unit_id,lc.client_unit_id)=1 " +
                                "inner join master_setting um on um.master_id = i.uom and um.type = 'UMM' and find_in_set(um.client_unit_id, lc.client_unit_id)=1 " +
                                "inner join master_setting ml on ml.master_id = lc.loc and ml.type = 'LC6' and find_in_set(ml.client_unit_id,lc.client_unit_id)=1 " +
                                "where length(lc.icode)>5 and lc.store = 'Y') a group by icode, iname, uom, loc, store,client_id, client_unit_id order by icode asc";
                            break;
                        case "GRPSUM":
                            cmd = "select '-' fstr, mg_code,mg,sg_code,sg,sum(to_number(iamount)) iamount from " +
                                "(select lc.icode, mg.master_name mg,mg.master_id as mg_code, sg.iname sg,sg.icode as sg_code, i.iname, um.master_name uom, ml.master_name loc, nvl(lc.qtyin, 0) qtyin, nvl(lc.qtyout, 0) qtyout," +
                                "(case when lc.pono = '0' then 'C' else lc.pono end) store,nvl(lc.irate, 0) irate,nvl(lc.iamount, 0) iamount,lc.type,lc.client_id," +
                                "lc.client_unit_id from btchtrans lc " +
                                "inner join item i on i.icode = lc.icode and i.type = 'IT' and find_in_set(lc.client_unit_id,i.client_unit_id)=1 " +
                                "inner join master_setting um on um.master_id = i.uom and um.type = 'UMM' and find_in_set(um.client_unit_id, lc.client_unit_id)= 1 " +
                                "inner join master_setting ml on ml.master_id = lc.loc and ml.type = 'LC6' and find_in_set(ml.client_unit_id,lc.client_unit_id)=1 " +
                                "inner join master_setting mg on mg.master_id = substr(lc.icode, 1, 3) and mg.type = 'KIG' and find_in_set(mg.client_unit_id,lc.client_unit_id)=1 " +
                                "inner join item sg on sg.icode = substr(lc.icode, 1, 5) and sg.type = 'SG' and find_in_set(sg.client_unit_id,lc.client_unit_id)=1 " +
                                "where length(lc.icode)> 5 and lc.store = 'Y' and lc.client_unit_id = '" + unitid_mst + "') a " +
                                "group by mg, sg,mg_code,sg_code";
                            break;
                        case "PPO":
                            cmd = "select (pono||podate||potype) fstr,pono,podate,potype,icode,iname,partycode,partyname,max(to_number(qtyord)) qtyord,sum(to_number(qtyin)) qtyin," +
                                "(max(to_number(qtyord))-sum(to_number(qtyin))) qtybalance,sum(to_number(iamount)) iamount " +
                                "from (select p.vch_num pono,to_char(p.vch_date,'" + sgen.Getsqldateformat() + "') podate,p.type potype," +
                                "p.icode,it.iname,p.acode Partycode,cl.c_name Partyname,nvl(p.qtyord, 0) qtyord,nvl(i.qtyin, 0) qtyin,nvl(i.irate, 0) irate," +
                                "nvl(i.iamount, 0) iamount,i.vch_num mrnno,to_char(i.vch_date,'" + sgen.Getsqldateformat() + "') mrndate," +
                                "i.type mrntype,i.invno,to_char(i.invdate,'" + sgen.Getsqldateformat() + "') invdate,i.chlno," +
                                "to_char(i.chldate,'" + sgen.Getsqldateformat() + "') chldate,utol,ltol " +
                                "from purchases p " +
                                "inner join itransaction i on i.pono = p.vch_num and to_char(i.podate, 'ddmmyyyy') = to_char(p.vch_date, 'ddmmyyyy') " +
                                "and i.client_unit_id = p.client_unit_id and i.acode = p.acode and i.store = 'Y' " +
                                "inner join item it on it.icode = p.icode and it.type = 'IT' and find_in_set(p.client_unit_id,it.client_unit_id)=1 " +
                                "inner join clients_mst cl on cl.vch_num = p.acode and substr(cl.vch_num,0,3)='203' and cl.type='BCD' and find_in_set(cl.client_unit_id,p.client_unit_id)=1 " +
                                "where p.type in ('50', '51', '52', '53') and p.client_unit_id = '" + unitid_mst + "') " +
                                //"group by pono,podate,potype,icode,iname,partycode,partyname having (max(to_number(qtyord))-sum(to_number(qtyin)))> 0";
                                "group by pono,podate,potype,icode,iname,partycode,partyname,ltol having sum(to_number(qtyin))<(max(to_number(qtyord))-round((max(to_number(qtyord))*to_number(ltol))/100))";
                            break;
                        case "PPODET":
                            cmd = "select (p.vch_num||to_char(p.vch_date,'yyyymmdd')||p.type) fstr,p.vch_num pono,to_char(convert_tz(p.vch_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') podate,p.type potype," +
                                "p.icode,it.iname,p.acode Partycode,cl.c_name Partyname,nvl(p.qtyord, 0) qtyord,nvl(i.qtyin, 0) qtyin,nvl(i.irate, 0) irate," +
                                "nvl(i.iamount, 0) iamount,i.vch_num mrnno,to_char(i.vch_date,'" + sgen.Getsqldateformat() + "') mrndate," +
                                "i.type mrntype,i.invno,to_char(i.invdate,'" + sgen.Getsqldateformat() + "') invdate,i.chlno," +
                                "to_char(i.chldate,'" + sgen.Getsqldateformat() + "') chldate " +
                                "from purchases p " +
                                "inner join itransaction i on i.pono = p.vch_num and to_char(i.podate, 'ddmmyyyy') = to_char(p.vch_date, 'ddmmyyyy') " +
                                "and i.client_unit_id = p.client_unit_id and i.acode = p.acode and i.store = 'Y' " +
                                "inner join item it on it.icode = p.icode and it.type = 'IT' and find_in_set(p.client_unit_id,it.client_unit_id)=1 " +
                                "inner join clients_mst cl on cl.vch_num = p.acode and substr(cl.vch_num,0,3)='203' and cl.type='BCD' and find_in_set(cl.client_unit_id,p.client_unit_id)=1 " +
                                "where p.type in ('50', '51', '52', '53') and p.client_unit_id = '" + unitid_mst + "' and " +
                                "(p.vch_num||to_char(p.vch_date,'" + sgen.Getsqldateformat() + "')||p.type)='" + param1.Trim() + "'";
                            break;
                    }
                    break;
                #endregion
                #region balsheet
                case "balsheet":
                    string fdt = year_from.Split(' ')[0].Trim(); string tdt = year_to.Split(' ')[0].Trim();
                    string vtyp = "";
                    switch (btnval.ToUpper())
                    {
                        case "GETLB":
                            cmd = "select (m.master_id) lbfstr,m.master_name lbname,sum(v.closing) lbcl from master_setting m " +
            "inner join (select client_unit_id, acode, sum(op) op, sum(dramount) dramount, sum(cramount) cramount, (sum(op) + sum(dramount) - sum(cramount)) closing from " +
            "(select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, OP_" + Ac_Year.Split(' ')[0].Trim() + " as op, 0 as dramount, 0 as cramount," +
            " 0 as cl from acbal union all " +
            "select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, sum(to_number(dramount)) - sum(to_number(cramount)) as op, 0 as dramount," +
            "0 as cramount, 0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
            "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') - 1 group by client_unit_id, acode " +
            "union all " +
            "select client_unit_id, acode as acode, vch_num, vch_date, 0 as op, to_number(dramount) dramount, to_number(cramount) cramount, 0 as cl from vouchers " +
            "where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') " +
            "and to_date('" + tdt + "', '" + sgen.Getsqldateformat() + "')) t group by client_unit_id, acode) v on substr(m.master_id,1,3)=substr(v.acode,1,3) " +
            "where m.type = 'AGM' and trim(substr(m.master_id,1,1))='" + param1 + "' and trim(substr(master_type,4,7))='0000' " +
            "group by m.master_name,m.master_id order by m.master_id asc";
                            Multiton.SetSession(MyGuid, "LEVEL", 4);
                            Multiton.SetSession(MyGuid, "MAXLEVEL", 4);
                            sgen.SetSession(MyGuid, "btnval", btnval);
                            sgen.SetSession(MyGuid, "btnval1st", btnval);
                            sgen.SetSession(MyGuid, "fstr0th", param1);
                            break;
                        case "LDG":
                            cmd = myquery(3, param1, fdt + "!~!~!~!~!" + tdt, " and (t.client_unit_id || t.acode) = '" + unitid_mst + "" + param1.Trim() + "'");
                            Multiton.SetSession(MyGuid, "LEVEL", 2);
                            Multiton.SetSession(MyGuid, "MAXLEVEL", 3);
                            sgen.SetSession(MyGuid, "btnval", btnval);
                            sgen.SetSession(MyGuid, "btnval1st", btnval);
                            sgen.SetSession(MyGuid, "fstr0th", param1);
                            break;
                        case "GETAS":

                            cmd = "select (m.master_id) asfstr,m.master_name asname,sum(v.closing) ascl from master_setting m " +
            "inner join (select client_unit_id, acode, sum(op) op, sum(dramount) dramount, sum(cramount) cramount, (sum(op) + sum(dramount) - sum(cramount)) closing from " +
            "(select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, OP_" + Ac_Year.Split(' ')[0].Trim() + " as op, 0 as dramount, 0 as cramount," +
            " 0 as cl from acbal " +
            "union all " +
            "select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, sum(to_number(dramount)) - sum(to_number(cramount)) as op, 0 as dramount," +
            "0 as cramount, 0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
            "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') - 1 group by client_unit_id, acode " +
            "union all " +
            "select client_unit_id, acode as acode, vch_num, vch_date, 0 as op, to_number(dramount) dramount, to_number(cramount) cramount, 0 as cl from vouchers " +
            "where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') " +
            "and to_date('" + tdt + "', '" + sgen.Getsqldateformat() + "')) t group by client_unit_id, acode) v on substr(m.master_id,1,3)=substr(v.acode,1,3) " +
            "where m.type = 'AGM' and trim(substr(m.master_id,1,1))='" + param1 + "' and trim(substr(master_type,4,7))='0000' " +
            "group by m.master_name,m.master_id order by m.master_id asc";
                            Multiton.SetSession(MyGuid, "LEVEL", 4);
                            Multiton.SetSession(MyGuid, "MAXLEVEL", 4);
                            sgen.SetSession(MyGuid, "btnval", btnval);
                            sgen.SetSession(MyGuid, "btnval1st", btnval);
                            sgen.SetSession(MyGuid, "fstr0th", param1);

                            break;
                        case "GETPL":

                            cmd = "select (m.master_id) asfstr,m.master_name asname,sum(v.closing) ascl from master_setting m " +
            "inner join (select client_unit_id, acode, sum(op) op, sum(dramount) dramount, sum(cramount) cramount, (sum(op) + sum(dramount) - sum(cramount)) closing from " +
            "(select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, OP_" + Ac_Year.Split(' ')[0].Trim() + " as op, 0 as dramount, 0 as cramount," +
            " 0 as cl from acbal " +
            "union all " +
            "select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, sum(to_number(dramount)) - sum(to_number(cramount)) as op, 0 as dramount," +
            "0 as cramount, 0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
            "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') - 1 group by client_unit_id, acode " +
            "union all " +
            "select client_unit_id, acode as acode, vch_num, vch_date, 0 as op, to_number(dramount) dramount, to_number(cramount) cramount, 0 as cl from vouchers " +
            "where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') " +
            "and to_date('" + tdt + "', '" + sgen.Getsqldateformat() + "')) t group by client_unit_id, acode) v on substr(m.master_id,1,3)=substr(v.acode,1,3) " +
            "where m.type = 'AGM' and trim(substr(m.master_id,1,1)) in ('" + param1.Replace(".", "','") + "') and trim(substr(master_type,4,7))='0000' " +
            "group by m.master_name,m.master_id order by m.master_id asc";
                            Multiton.SetSession(MyGuid, "LEVEL", 4);
                            sgen.SetSession(MyGuid, "btnval", btnval);
                            Multiton.SetSession(MyGuid, "MAXLEVEL", 4);
                            sgen.SetSession(MyGuid, "btnval1st", btnval);
                            sgen.SetSession(MyGuid, "fstr0th", param1);
                            break;
                        case "GETLBDET":
                        case "GETPLDET":
                        case "GETASDET":
                            if (param1 != null) vtyp = param1.Substring(0, 1);
                            cmd = "select (a.client_unit_id||a.acode) fstr,a.acode LedgerCode,b.c_name Ledger,'-' first_grp,'-' second_grp,a.op,a.dramount,a.cramount,a.closing from " +
                           "(select client_unit_id, acode,max(vtype) as vtype, sum(op) op, sum(dramount) dramount, sum(cramount) cramount, (sum(op) + sum(dramount) - sum(cramount)) closing from " +
                           "(select client_unit_id, acode as acode,'-' as vtype, '0' as vch_num, null as vch_date, OP_" + Ac_Year.Split(' ')[0].Trim() + " as op, 0 as dramount, 0 as cramount," +
                           " 0 as cl from acbal " +
                           "union all " +
                           "select client_unit_id, acode as acode,max(type) as vtype, '0' as vch_num, null as vch_date, sum(to_number(dramount)) - sum(to_number(cramount)) as op, 0 as dramount," +
                           "0 as cramount, 0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                           "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') - 1 group by client_unit_id, acode " +
                           "union all " +
                           "select client_unit_id, acode as acode,type as vtype, vch_num, vch_date, 0 as op, to_number(dramount) dramount, to_number(cramount) cramount, 0 as cl from vouchers " +
                           "where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') " +
                           "and to_date('" + tdt + "', '" + sgen.Getsqldateformat() + "')) t group by client_unit_id, acode) a " +
                           "inner join clients_mst b on a.acode = b.vch_num and substr(b.VCH_NUM,1,1)='" + vtyp + "' where a.client_unit_id = '" + unitid_mst + "'";
                            
                            title = "Balance Sheet";
                            seektype = "1";
                            string qucond = "TB";
                            Multiton.SetSession(MyGuid, "LEVEL", 3);
                            sgen.SetSession(MyGuid, "btnval", btnval);
                            sgen.SetSession(MyGuid, "qucond", qucond);
                            sgen.SetSession(MyGuid, "btnval2nd", btnval);
                            sgen.SetSession(MyGuid, "actype", param1);
                            Multiton.SetSession(MyGuid, "SHOWTOT", "Y");

                            //sgen.SetSession(MyGuid, "footitlebs", "TRIAL BALANCE");
                            break;

                    }
                    break;
                #endregion
                #region pl
                case "pl":
                    fdt = year_from.Split(' ')[0].Trim(); tdt = year_to.Split(' ')[0].Trim();
                    vtyp = "";
                    switch (btnval.ToUpper())
                    {
                        case "GETLB":
                            cmd = "select (m.master_id) lbfstr,m.master_name lbname,sum(v.closing) lbcl from master_setting m " +
            "inner join (select client_unit_id, acode, sum(op) op, sum(dramount) dramount, sum(cramount) cramount, (sum(op) + sum(dramount) - sum(cramount)) closing from " +
            "(select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, OP_" + Ac_Year.Split(' ')[0].Trim() + " as op, 0 as dramount, 0 as cramount," +
            " 0 as cl from acbal union all " +
            "select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, sum(to_number(dramount)) - sum(to_number(cramount)) as op, 0 as dramount," +
            "0 as cramount, 0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
            "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') - 1 group by client_unit_id, acode " +
            "union all " +
            "select client_unit_id, acode as acode, vch_num, vch_date, 0 as op, to_number(dramount) dramount, to_number(cramount) cramount, 0 as cl from vouchers " +
            "where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') " +
            "and to_date('" + tdt + "', '" + sgen.Getsqldateformat() + "')) t group by client_unit_id, acode) v on substr(m.master_id,1,3)=substr(v.acode,1,3) " +
            "where m.type = 'AGM' and trim(substr(m.master_id,1,1))='" + param1 + "' and trim(substr(master_type,4,7))='0000' " +
            "group by m.master_name,m.master_id order by m.master_id asc";
                            Multiton.SetSession(MyGuid, "LEVEL", 4);
                            Multiton.SetSession(MyGuid, "MAXLEVEL", 4);
                            sgen.SetSession(MyGuid, "btnval", btnval);
                            sgen.SetSession(MyGuid, "btnval1st", btnval);
                            sgen.SetSession(MyGuid, "fstr0th", param1);
                            break;
                        case "LDG":
                            cmd = myquery(3, param1, fdt + "!~!~!~!~!" + tdt, " and (t.client_unit_id || t.acode) = '" + unitid_mst + "" + param1.Trim() + "'");
                            Multiton.SetSession(MyGuid, "LEVEL", 2);
                            Multiton.SetSession(MyGuid, "MAXLEVEL", 3);
                            sgen.SetSession(MyGuid, "btnval", btnval);
                            sgen.SetSession(MyGuid, "btnval1st", btnval);
                            sgen.SetSession(MyGuid, "fstr0th", param1);
                            break;
                        case "GETAS":

                            cmd = "select (m.master_id) asfstr,m.master_name asname,sum(v.closing) ascl from master_setting m " +
            "inner join (select client_unit_id, acode, sum(op) op, sum(dramount) dramount, sum(cramount) cramount, (sum(op) + sum(dramount) - sum(cramount)) closing from " +
            "(select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, OP_" + Ac_Year.Split(' ')[0].Trim() + " as op, 0 as dramount, 0 as cramount," +
            " 0 as cl from acbal " +
            "union all " +
            "select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, sum(to_number(dramount)) - sum(to_number(cramount)) as op, 0 as dramount," +
            "0 as cramount, 0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
            "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') - 1 group by client_unit_id, acode " +
            "union all " +
            "select client_unit_id, acode as acode, vch_num, vch_date, 0 as op, to_number(dramount) dramount, to_number(cramount) cramount, 0 as cl from vouchers " +
            "where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') " +
            "and to_date('" + tdt + "', '" + sgen.Getsqldateformat() + "')) t group by client_unit_id, acode) v on substr(m.master_id,1,3)=substr(v.acode,1,3) " +
            "where m.type = 'AGM' and trim(substr(m.master_id,1,1))='" + param1 + "' and trim(substr(master_type,4,7))='0000' " +
            "group by m.master_name,m.master_id order by m.master_id asc";
                            Multiton.SetSession(MyGuid, "LEVEL", 4);
                            Multiton.SetSession(MyGuid, "MAXLEVEL", 4);
                            sgen.SetSession(MyGuid, "btnval", btnval);
                            sgen.SetSession(MyGuid, "btnval1st", btnval);
                            sgen.SetSession(MyGuid, "fstr0th", param1);

                            break;
                        case "GETPL":

                            cmd = "select (m.master_id) asfstr,m.master_name asname,sum(v.closing) ascl from master_setting m " +
            "inner join (select client_unit_id, acode, sum(op) op, sum(dramount) dramount, sum(cramount) cramount, (sum(op) + sum(dramount) - sum(cramount)) closing from " +
            "(select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, OP_" + Ac_Year.Split(' ')[0].Trim() + " as op, 0 as dramount, 0 as cramount," +
            " 0 as cl from acbal " +
            "union all " +
            "select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, sum(to_number(dramount)) - sum(to_number(cramount)) as op, 0 as dramount," +
            "0 as cramount, 0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
            "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') - 1 group by client_unit_id, acode " +
            "union all " +
            "select client_unit_id, acode as acode, vch_num, vch_date, 0 as op, to_number(dramount) dramount, to_number(cramount) cramount, 0 as cl from vouchers " +
            "where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') " +
            "and to_date('" + tdt + "', '" + sgen.Getsqldateformat() + "')) t group by client_unit_id, acode) v on substr(m.master_id,1,3)=substr(v.acode,1,3) " +
            "where m.type = 'AGM' and trim(substr(m.master_id,1,1)) in ('" + param1.Replace(".", "','") + "') and trim(substr(master_type,4,7))='0000' " +
            "group by m.master_name,m.master_id order by m.master_id asc";
                            Multiton.SetSession(MyGuid, "LEVEL", 4);
                            sgen.SetSession(MyGuid, "btnval", btnval);
                            Multiton.SetSession(MyGuid, "MAXLEVEL", 4);
                            sgen.SetSession(MyGuid, "btnval1st", btnval);
                            sgen.SetSession(MyGuid, "fstr0th", param1);
                            break;
                        case "GETLBDET":
                        case "GETPLDET":
                        case "GETASDET":
                            if (param1 != null) vtyp = param1.Substring(0, 1);
                            cmd = "select (a.client_unit_id||a.acode) fstr,a.acode LedgerCode,b.c_name Ledger,'-' first_grp,'-' second_grp,a.op,a.dramount,a.cramount,a.closing from " +
                           "(select client_unit_id, acode,max(vtype) as vtype, sum(op) op, sum(dramount) dramount, sum(cramount) cramount, (sum(op) + sum(dramount) - sum(cramount)) closing from " +
                           "(select client_unit_id, acode as acode,'-' as vtype, '0' as vch_num, null as vch_date, OP_" + Ac_Year.Split(' ')[0].Trim() + " as op, 0 as dramount, 0 as cramount," +
                           " 0 as cl from acbal " +
                           "union all " +
                           "select client_unit_id, acode as acode,max(type) as vtype, '0' as vch_num, null as vch_date, sum(to_number(dramount)) - sum(to_number(cramount)) as op, 0 as dramount," +
                           "0 as cramount, 0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                           "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') - 1 group by client_unit_id, acode " +
                           "union all " +
                           "select client_unit_id, acode as acode,type as vtype, vch_num, vch_date, 0 as op, to_number(dramount) dramount, to_number(cramount) cramount, 0 as cl from vouchers " +
                           "where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') " +
                           "and to_date('" + tdt + "', '" + sgen.Getsqldateformat() + "')) t group by client_unit_id, acode) a " +
                           "inner join clients_mst b on a.acode = b.vch_num and substr(b.VCH_NUM,1,1)='" + vtyp + "' where a.client_unit_id = '" + unitid_mst + "'";
                            
                            title = "Balance Sheet";
                            seektype = "1";
                            string qucond = "TB";
                            Multiton.SetSession(MyGuid, "LEVEL", 3);
                            sgen.SetSession(MyGuid, "btnval", btnval);
                            sgen.SetSession(MyGuid, "qucond", qucond);
                            sgen.SetSession(MyGuid, "btnval2nd", btnval);
                            sgen.SetSession(MyGuid, "actype", param1);
                            Multiton.SetSession(MyGuid, "SHOWTOT", "Y");

                            //sgen.SetSession(MyGuid, "footitlebs", "TRIAL BALANCE");
                            break;

                    }
                    break;
                #endregion
                #region chq_srl
                case "chq_srl":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = "select (v.client_id||v.client_unit_id|| v.vch_num|| to_char(v.vch_date, 'yyyymmdd')||v.type) as fstr,v.vch_num as doc_no,to_char(v.vch_date,'" + sgen.Getsqldateformat() + "') as doc_date" +
                                ",cl.bank_name,col9||'-'||col10 as series_between from enx_tab v inner join (select nvl(a.master_name,'-')||'('||cm.cpname||')' as bank_name,cm.cpname as bank_ac,cm.vch_num as bankid from clients_mst cm " +
                    "inner join master_setting a on a.master_id = cm.ptype and a.type = 'ABD' and find_in_set(a.client_unit_id,cm.client_unit_id)=1 " +
                    "where cm.type = 'BAD' and find_in_set(cm.client_unit_id, '" + unitid_mst + "')=1) cl on cl.bankid = v.col2 where v.type = 'CSR' and v.client_unit_id = '" + unitid_mst + "' order by v.vch_num desc";
                            break;
                    }
                    break;
                #endregion
                #region acc_link
                case "acc_link":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                            //        cmd = "select (v.client_id||v.client_unit_id|| v.vch_num|| to_char(v.vch_date, 'yyyymmdd')||v.type) as fstr,v.vch_num as doc_no,to_char(v.vch_date,'" + sgen.Getsqldateformat() + "') as doc_date" +
                            //            ",cl.bank_name,col9||'-'||col10 as series_between from enx_tab v inner join (select nvl(a.master_name,'-')||'('||cm.cpname||')' as bank_name,cm.cpname as bank_ac,cm.vch_num as bankid from clients_mst cm " +
                            //"inner join master_setting a on a.master_id = cm.ptype and a.type = 'ABD' and find_in_set(a.client_unit_id,cm.client_unit_id)=1 " +
                            //"where cm.type = 'BAD' and find_in_set(cm.client_unit_id, '" + unitid_mst + "')=1) cl on cl.bankid = v.col2 where v.type = 'CSR' and v.client_unit_id = '" + unitid_mst + "' order by v.vch_num desc";

                            cmd = "select (v.client_id||v.client_unit_id|| v.vch_num|| to_char(v.vch_date, 'yyyymmdd')||v.type) as fstr,v.vch_num as doc_no,to_char(v.vch_date,'" + sgen.Getsqldateformat() + "') as doc_date" +
                                ",v.col1 as account_name,cl.c_name as ledger_name from enx_tab v inner join clients_mst cl on cl.vch_num = v.col7 and find_in_set(cl.client_unit_id,v.client_unit_id)=1" +
                                " where v.type = 'ACL' and v.client_unit_id = '" + unitid_mst + "' order by v.vch_num desc";
                            break;
                    }
                    break;
                #endregion
                #region chq_rej
                case "chq_rej":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = "select (v.client_id||v.client_unit_id|| v.vch_num|| to_char(v.vch_date, 'yyyymmdd')||v.type) as fstr,v.vch_num as doc_no,to_char(v.vch_date,'" + sgen.Getsqldateformat() + "') as doc_date" +
                                ",cl.bank_name,v.col3 as cheque_no,v.col1 as rej_reason from enx_tab v inner join (select nvl(a.master_name,'-')||'('||cm.cpname||')' as bank_name,cm.cpname as bank_ac,cm.vch_num as bankid from clients_mst cm " +
                    "inner join master_setting a on a.master_id = cm.ptype and a.type = 'ABD' and find_in_set(a.client_unit_id,cm.client_unit_id)=1 " +
                    "where cm.type = 'BAD' and find_in_set(cm.client_unit_id, '" + unitid_mst + "')=1) cl on cl.bankid = v.col2 where v.type = 'CRJ' and v.client_unit_id = '" + unitid_mst + "' order by v.vch_num desc";
                            break;
                        case "CHEQUES":
                            if (param2 != "")
                            {
                                string from = sgen.seekval(userCode, "select col9 as fromval from enx_tab where type='CSR' and client_unit_id='" + unitid_mst + "' and col2 = '" + param2.ToString() + "'", "fromval");
                                string to = sgen.seekval(userCode, "select col10 as toval from enx_tab where type='CSR' and client_unit_id='" + unitid_mst + "' and col2 ='" + param2.ToString() + "'", "toval");
                                cmd = "SELECT R as fstr,R as Cheque_no FROM (Select Rownum r From dual Connect By Rownum <= " + sgen.Make_int(to) + ") WHERE R BETWEEN " +
                                    "" + sgen.Make_int(from) + " AND " + sgen.Make_int(to) + " AND to_char(R) NOT IN (" +
                                    "select chqno from vouchers where type='21' and type1 = 'BLKPAY' and acode='" + param2.ToString() + "' union all " +
                                    "select col3 as chqno from enx_tab where type='CRJ' and client_unit_id = '" + unitid_mst + "' and col2='" + param2.ToString() + "')";
                            }
                            break;
                    }
                    break;
                #endregion
                #region recil
                case "recil":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = "select (v.client_unit_id|| v.vch_num|| to_char(v.vch_date, 'yyyymmdd')||v.type) as fstr,v.vch_num as doc_no,to_char(v.vch_date,'" + sgen.Getsqldateformat() + "') as doc_date,v.col22 as voucher,to_char(v.date2,'" + sgen.Getsqldateformat() + "') as vou_date,to_char(v.date3,'" + sgen.Getsqldateformat() + "') as reco_date" +
                                " from enx_tab2 v where v.type = 'RCL' and v.client_unit_id = '" + unitid_mst + "' order by v.vch_num desc";
                            break;
                    }
                    break;
                #endregion
                #region cas_rec
                case "cas_rec":
                    switch (btnval.ToUpper())
                    {
                        case "PRINT":
                        case "VIEW":
                        case "EDIT":
                            type = "";
                            if (sgen.GetSession(MyGuid, "SALTYPE") != null)
                            {
                                type = "" + sgen.GetSession(MyGuid, "SALTYPE").ToString() + "";
                            }
                            else
                            {
                                type = "VOU','ESP";
                            }
                            cmd = "select distinct (vu.client_unit_id||vu.vch_num|| to_char(vu.vch_date, 'yyyymmdd')|| vu.type) as fstr,vu.vch_num as doc_num,to_char(vu.vch_date, 'dd/MM/YYYY') doc_date,vu.type,vu.invno as ref_no,to_char(vu.invdate, 'dd/MM/YYYY') as ref_date,vu.adj_type,vu.acode as party_code" +
                                ",vu.rvscode as reverse_code,nvl(cl.aname, '-') party_name,vu.dramount,vu.cramount from vouchers vu INNER JOIN (select vch_num as acode,c_name as aname,CLIENT_UNIT_ID from clients_mst " +
                                "where type='BCD' and find_in_set(client_unit_id, '" + unitid_mst + "')=1 union all select vch_num as acode ,c_name as aname,CLIENT_UNIT_ID from clients_mst where type = 'LDG' " +
                                "and find_in_set(client_unit_id, '" + unitid_mst + "')=1 union all select cm.vch_num as acode,nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname,cm.CLIENT_UNIT_ID from master_setting a inner join clients_mst cm on " +
                                "a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id where cm.type = 'BAD' and cm.client_unit_id = '" + unitid_mst + "') cl on cl.acode = vu.acode and find_in_set(vu.client_unit_id, cl.client_unit_id) = 1 " +
                                "where vu.type in ('" + type + "') and vu.client_unit_id = '" + unitid_mst + "' and (to_number(vu.dramount)+to_number(vu.cramount))>0 order by vu.vch_num desc";
                            break;
                        case "PARTY":
                            cmd = "select a.vch_num||a.type as fstr,a.c_name as Client_name,a.addr as address,a.c_gstin as GSTIN," +
                                "(case when a.tor = 'R' then 'Regular' when a.tor='C' then 'Composition' else 'Not Registered' END) GSTType," +
                                "a.type_desc as Search_text from clients_mst a  where a.type = 'BCD' and substr(a.vch_num,0,3)='203' and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "' " +
                                "union all select a.vch_num || a.type as fstr,ledger_name Client_name, ledger_abb address,to_char(group_code) GSTIN, " +
                                "'-' GSTType,'-' Search_text from ledger_master a where a.type = 'ALM' and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                            break;
                    }
                    break;
                #endregion
                #region cas_recn
                case "cas_recn":
                    switch (btnval.ToUpper())
                    {
                        case "PRINT":
                        case "VIEW":
                        case "EDIT":
                        case "COPYOLD":
                            type = "";
                            if (sgen.GetSession(MyGuid, "SALTYPE") != null)
                            {
                                type = "" + sgen.GetSession(MyGuid, "SALTYPE").ToString() + "";
                                if (type == "50")
                                {
                                    type = "01,02,03,50";
                                }
                            }
                            else
                            {
                                type = "VOU'";
                            }
                            cmd = "select distinct (vu.client_unit_id||vu.vch_num|| to_char(vu.vch_date, 'yyyymmdd')|| vu.type) as fstr,vu.rno,vu.vch_num as doc_num,to_char(vu.vch_date, 'dd/MM/YYYY') doc_date,vu.type,vu.invno as ref_no,to_char(vu.invdate, 'dd/MM/YYYY') as ref_date,vu.adj_type,vu.acode as party_code" +
                                ",vu.rvscode as reverse_code,nvl(cl.aname, '-') party_name,vu.dramount,vu.cramount from vouchers vu INNER JOIN (select vch_num as acode,c_name as aname,CLIENT_UNIT_ID from clients_mst " +
                                "where type='BCD' and find_in_set(client_unit_id, '" + unitid_mst + "')=1 union all select vch_num as acode ,c_name as aname,CLIENT_UNIT_ID from clients_mst where type = 'LDG' " +
                                "and find_in_set(client_unit_id, '" + unitid_mst + "')=1 union all select cm.vch_num as acode,nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname,cm.CLIENT_UNIT_ID from master_setting a inner join clients_mst cm on " +
                                "a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id where cm.type = 'BAD' and cm.client_unit_id = '" + unitid_mst + "') cl on cl.acode = vu.acode and find_in_set(vu.client_unit_id, cl.client_unit_id) = 1 " +
                                "where vu.type in ('" + type.Trim().TrimEnd(',').Replace(",", "','").Trim().ToString() + "') and vu.client_unit_id = '" + unitid_mst + "' and (to_number(vu.dramount)+to_number(vu.cramount))>0 order by vu.vch_num desc,vu.rno asc";
                            break;
                        case "PARTY":
                            cmd = "select a.vch_num||a.type as fstr,a.c_name as Client_name,a.addr as address,a.c_gstin as GSTIN," +
                                "(case when a.tor = 'R' then 'Regular' when a.tor='C' then 'Composition' else 'Not Registered' END) GSTType," +
                                "a.type_desc as Search_text from clients_mst a  where a.type = 'BCD' and substr(a.vch_num,0,3)='203' and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "' " +
                                "union all select a.vch_num || a.type as fstr,ledger_name Client_name, ledger_abb address,to_char(group_code) GSTIN, " +
                                "'-' GSTType,'-' Search_text from ledger_master a where a.type = 'ALM' and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                            break;
                    }
                    break;
                    #endregion
            }
            sgen.open_grid(title, cmd, sgen.Make_int(seektype));
            ViewBag.pageurl = sgen.GetSession(MyGuid, "pageurl").ToString();
        }
        #endregion
        //============Method=========//
        #region groupmaster
        public ActionResult grp_mst()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "GROUP MASTER";
            model[0].Col10 = "master_setting";
            model[0].Col12 = "AGM";
            ViewBag.vsavenew = "disabled='disabled'";
            return View(model);
        }
        public List<Tmodelmain> new_grpmst(List<Tmodelmain> model)
        {
            model[0].Col13 = "Save";
            model[0].Col100 = "Save & New";
            sgen.SetSession(MyGuid, "EDMODE", "Y");
            ViewBag.vnew = "disabled='disabled'";
            ViewBag.vedit = "disabled='disabled'";
            ViewBag.vsave = "";
            ViewBag.vsavenew = "";
            ViewBag.scripCall = "enableForm();";
            return model;
        }
        [HttpPost]
        public ActionResult grp_mst(List<Tmodelmain> model, string command)
        {
            FillMst();
            var tm = model.FirstOrDefault();

            if (command == "New")
            {
                try
                {
                    model = new_grpmst(model);
                }
                catch (Exception ex)
                { sgen.showmsg(1, ex.Message.ToString(), 2); }
            }
            else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
            {
                string cond = "";
                try
                {
                    string currdate = sgen.server_datetime_local(userCode);
                    string found = "";
                    string id1 = "", finalid = ""; ;
                    if (model[0].Col16 == "")
                    {
                        sgen.showmsg(1, "Please Enter Group Name (Long Text)", 2);
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                        return View(model);
                    }
                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        isedit = true;
                        model[0].Col27 = tm.Col27;
                        cond = "and vch_num<>'" + model[0].Col27 + "'";
                    }
                    else
                    {
                        isedit = false;
                        vch_num = sgen.genNo(userCode, "select max(to_number(vch_num)) as max from " + model[0].Col10 + " " +
                            "where type='" + model[0].Col12 + "' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'", 6, "max");
                        model[0].Col27 = vch_num;
                    }
                    if (model[0].Col25 == "Y")
                    {
                        id1 = sgen.genNo(userCode, "select max(to_number(substr(master_type," + (model[0].Col24.Length + 1) + ", 2))) as max " +
       "from " + model[0].Col10 + " where type='" + model[0].Col12 + "'  and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "' " +
       "and  substr(master_type,1," + model[0].Col24.Length + ")='" + model[0].Col24 + "'", 2, "max");

                        finalid = (model[0].Col24 + id1.ToString()).PadRight(7, '0');
                        model[0].Col23 = finalid;
                    }
                    found = sgen.getstring(userCode, "select master_name from " + model[0].Col10 + " where master_name='" + model[0].Col16 + "' and" +
                               " (client_id='001' or client_id='-' ) and (client_unit_id='001001' or client_unit_id='-')  and type='" + model[0].Col12 + "' " + cond + "");
                    if (found != "")
                    {
                        ViewBag.scripCall = "showmsgJS(1,'Data Exists Already',2)";
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                        return View(model);
                    }
                    if (model[0].Col19 == "")
                    {
                        sgen.showmsg(1, "Please Select Group Name", 2);
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                        return View(model);
                    }
                    DataTable dataTable = cmd_Fun.GetStructure(userCode, model[0].Col10);
                    DataRow dr = dataTable.NewRow();
                    dr["rec_id"] = "0";
                    dr["vch_num"] = model[0].Col27;
                    dr["vch_date"] = currdate;
                    dr["type"] = model[0].Col12;
                    dr["master_name"] = model[0].Col16;
                    dr["COL1"] = model[0].Chk1 == true ? "Y" : "N";
                    dr["master_type"] = model[0].Col23;
                    dr["master_id"] = model[0].Col23.Substring(0, 3);
                    dr["sectionid"] = model[0].Col17;
                    dr["client_id"] = clientid_mst;
                    dr["client_unit_id"] = unitid_mst;
                    if (isedit)
                    {
                        dr["master_entby"] = model[0].Col3;
                        dr["master_entdate"] = model[0].Col4;
                        dr["master_editby"] = userid_mst;
                        dr["master_editdate"] = currdate;
                    }
                    else
                    {
                        dr["master_editby"] = "-";
                        dr["master_editdate"] = currdate;
                        dr["master_editby"] = userid_mst;
                        dr["master_entdate"] = currdate;
                    }
                    dataTable.Rows.Add(dr);
                    if (dataTable.Rows.Count > 0)
                    {
                        bool Result = sgen.Update_data(userCode, dataTable, model[0].Col10, model[0].Col8, isedit);
                        if (Result == true)
                        {
                            if (command.Trim().ToUpper().Equals("SAVE") || command.Trim().ToUpper().Equals("UPDATE"))
                            {
                                //sat1.Commit();
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
                                    model = new_grpmst(model);
                                    ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                                }
                                catch (Exception ex) { }
                            }
                            model = new List<Tmodelmain>();
                            model.Add(new Tmodelmain
                            {
                                Col9 = tm.Col9,
                                Col10 = tm.Col10,
                                Col11 = tm.Col11,
                                Col13 = "Save",
                                Col100 = "Save & New",
                                Col14 = tm.Col14,
                                Col15 = tm.Col15,
                                Col12 = tm.Col12,
                            });
                        }
                        else
                        {
                            ViewBag.scripCall = "showmsgJS(1,'Data Exists Already',0)";
                        }
                    }
                }
                catch (Exception ex)
                {
                    sgen.showmsg(1, ex.Message.ToString(), 0);
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
        #region Auto Fill Party
        public DataSet getitemData(string userCode, string query, int pageno, string searchval)
        {
            string colswhere = "";
            foreach (PropertyInfo prop in typeof(Party).GetProperties())
            {
                if (colswhere.Equals("")) colswhere = "NVL(" + prop.Name + ",'-')";
                else colswhere = colswhere + "||" + "NVL(" + prop.Name + ",'-') ";
            }
            //string where = "where lower(" + colswhere + ") LIKE '%" + searchval.ToLower() + "%') > 0 ";
            string where = "where lower(" + colswhere + ") LIKE '%" + searchval.ToLower().Trim() + "%' ";
            // shiv
            query = "SELECT *  from (" + query + " ) tab " + where;
            mq = "SELECT Count(*) as cnt from (" + query + " ) tab";
            string rows = sgen.seekval(userCode, mq, "cnt");
            int olds = 0, limit = 10;
            if (limit == 0) limit = 1;
            // shiv
            mq1 = "SELECT '0' as chk,tab.* FROM( SELECT rownum as  Sr_No,t.* FROM (" + query + ") t  " +
                ") TAB WHERE Sr_No BETWEEN " + limit + " * (" + pageno + " - 1) + 1+" + olds + " AND (" + pageno + " *" + limit + ")+" + olds;
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
        [HttpGet]
        public ActionResult Getparty(string searchTerm, int pageSize, int pageNum, string icode, string Myguid)
        {
            FillMst(Myguid);
            //DataSet ds = getitemData(userCode, "select vch_num as acode ,c_name as aname,'-' as Address" +
            //    ",'-' as Mobile,'-' as Email,'-' as Pincode ,'-' cramount,'-' dramount from clients_mst " +
            //    "where type in ('BCD','LDG','BAD') and find_in_set(client_unit_id,'" + unitid_mst + "')=1", 1, searchTerm);
            DataSet ds = getitemData(userCode, "select vch_num as acode,c_name as aname,'-' as Address,'-' as Mobile,'-' as Email,'-' as Pincode ,'-' cramount,'-' dramount from clients_mst where type='BCD' and " +
                "find_in_set(client_unit_id, '" + unitid_mst + "')=1 union all select vch_num as acode ,c_name as aname,'-' as Address,'-' as Mobile,'-' as Email,'-' as Pincode ,'-' cramount,'-' dramount " +
                "from clients_mst where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "')=1 union all select cm.vch_num as acode,nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname, '-' as Address, '-' as Mobile, '-' as Email" +
                ", '-' as Pincode, '-' cramount, '-' dramount from master_setting a inner join clients_mst cm on a.master_id = cm.ptype and a.type = 'ABD' and find_in_set(a.client_unit_id, cm.client_unit_id)=1 where cm.type = 'BAD' " +
                "and find_in_set(cm.client_unit_id,'" + unitid_mst + "')=1", 1, searchTerm);
            return DstoJSonItems(ds);
        }
        public JsonpResult DstoJSonItems(DataSet ds)
        {
            List<Party> parties = new List<Party>();
            foreach (DataRow row in ds.Tables["Main"].Rows)
            {
                Party it = new Party();
                it.Acode = row["acode"].ToString();
                it.Aname = row["aname"].ToString();
                it.Address = row["Address"].ToString();
                it.Mobile = row["Mobile"].ToString();
                it.Email = row["Email"].ToString();
                it.Pincode = row["Pincode"].ToString();
                parties.Add(it);
            }
            int attendeeCount = sgen.Make_int(ds.Tables["Totals"].Rows[0][0].ToString());
            Select2PagedResult pagedAttendees = new Select2PagedResult();
            pagedAttendees.Results = new List<Select2Result>();
            foreach (Party a in parties)
            {
                pagedAttendees.Results.Add(new Select2Result { id = a.Acode.ToString() + "!~!" + a.Pincode + "!~!" + a.Address + "!~!" + a.Email + "!~!" + a.Mobile, text = a.Aname });
            }
            pagedAttendees.Total = attendeeCount;
            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        #endregion
        #region Auto Fill Method Of Adjustment
        public DataSet getitemDataadj(string userCode, string query, int pageno, string searchval)
        {
            string colswhere = "";
            foreach (PropertyInfo prop in typeof(ADJTYPE_ACCOUNT).GetProperties())
            {
                if (colswhere.Equals("")) colswhere = "NVL(" + prop.Name + ",'-')";
                else colswhere = colswhere + "||" + "NVL(" + prop.Name + ",'-') ";
            }
            //string where = "where lower(" + colswhere + ") LIKE '%" + searchval.ToLower() + "%') > 0 ";
            string where = "where lower(" + colswhere + ") LIKE '%" + searchval.ToLower().Trim() + "%' ";
            // shiv
            query = "SELECT *  from (" + query + " ) tab " + where;
            mq = "SELECT Count(*) as cnt from (" + query + " ) tab";
            string rows = sgen.seekval(userCode, mq, "cnt");
            int olds = 0, limit = 10;
            if (limit == 0) limit = 1;
            // shiv
            mq1 = "SELECT '0' as chk,tab.* FROM( SELECT rownum as  Sr_No,t.* FROM (" + query + ") t  " +
                ") TAB WHERE Sr_No BETWEEN " + limit + " * (" + pageno + " - 1) + 1+" + olds + " AND (" + pageno + " *" + limit + ")+" + olds;
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
        [HttpGet]
        public ActionResult Getadjtype(string searchTerm, int pageSize, int pageNum, string icode, string Myguid)
        {
            FillMst(Myguid);
            DataSet ds = getitemDataadj(userCode, "select 'ADV' as Adj_type_code,'Advance' as Adj_type from dual union all" +
                " select 'AGR' as Adj_type_code,'Against Ref' as Adj_type from dual union all select 'NEW' as Adj_type_code,'New Ref' as Adj_type from dual" +
                " union all select 'OAC' as Adj_type_code,'On Account' as Adj_type from dual", 1, searchTerm);
            return DstoJSonItemsadj(ds);
        }
        public JsonpResult DstoJSonItemsadj(DataSet ds)
        {
            List<ADJTYPE_ACCOUNT> parties = new List<ADJTYPE_ACCOUNT>();
            foreach (DataRow row in ds.Tables["Main"].Rows)
            {
                ADJTYPE_ACCOUNT it = new ADJTYPE_ACCOUNT();
                it.Adj_type_code = row["Adj_type_code"].ToString();
                it.Adj_type = row["Adj_type"].ToString();
                parties.Add(it);
            }
            int attendeeCount = sgen.Make_int(ds.Tables["Totals"].Rows[0][0].ToString());
            Select2PagedResult pagedAttendees = new Select2PagedResult();
            pagedAttendees.Results = new List<Select2Result>();
            foreach (ADJTYPE_ACCOUNT a in parties)
            {
                pagedAttendees.Results.Add(new Select2Result { id = a.Adj_type_code.ToString(), text = a.Adj_type });
            }
            pagedAttendees.Total = attendeeCount;
            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        #endregion
        #region vou_entry
        public ActionResult vou_entry()
        {
            FillMst("");
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "VOUCHER ENTRY";
            model[0].Col10 = "vouchers";
            model[0].Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            if (model[0].Col14 == "22003.5")
            {
                model[0].Col12 = "30";
                model[0].Col9 = "JOURNAL VOUCHER";
            }
            else if (model[0].Col14 == "22003.10")
            {
                model[0].Col12 = "32";
                model[0].Col9 = "CREDIT NOTE";

            }
            else if (model[0].Col14 == "22003.9")
            {
                model[0].Col12 = "31";
                model[0].Col9 = "DEBIT NOTE";

            }
            else if (model[0].Col14 == "22003.6")
            {
                model[0].Col12 = "33";
                model[0].Col9 = "CONTRA VOUCHER";
            }
            else
            {
                model[0].Col12 = "VOU";
                model[0].Col9 = "VOUCHER ENTRY";
            }
            sgen.SetSession(MyGuid, "VOUTYPE", model[0].Col12);
            DataTable dt = sgen.getdata(userCode, "select '0' as  Select_ ,'1' as  Sno ,'-' as Acode,'-' as AName,'0' as DrAmount,'0' as  CrAmount from dual");
            model[0].dt1 = dt;
            sgen.SetSession(MyGuid, "KDTVOU", dt.Copy());
            if (Request.QueryString["vid"] != null)
            {
                string fstr = EncryptDecrypt.Decrypt(Request.QueryString["vid"].ToString().Trim());
                sgen.SetSession(MyGuid, "SSEEKVAL", fstr);
                CallbackFun("EDIT", "Y", "vou_entry", "Account", model);
                switch (model[0].Col12)
                {
                    case "33":
                        model[0].Col9 = "CONTRA VOUCHER";
                        break;
                    case "11":
                        model[0].Col9 = "BANK RECIEPT";
                        break;
                    case "10":
                        model[0].Col9 = "CASH RECIEPT";
                        break;
                    case "20":
                        model[0].Col9 = "CASH PAYMENT";
                        break;
                    case "21":
                        model[0].Col9 = "BANK PAYMENT";
                        break;
                    case "32":
                        model[0].Col9 = "CREDIT NOTE";
                        break;
                    case "31":
                        model[0].Col9 = "DEBIT NOTE";
                        break;
                    case "30":
                        model[0].Col9 = "JOURNAL VOUCHER";
                        break;
                    case "40":
                        model[0].Col9 = "SALE VOUCHER";
                        break;
                    case "50":
                        model[0].Col9 = "PURCHASE VOUCHER";
                        break;
                    default:
                        model[0].Col9 = "VOUCHER ENTRY";
                        break;
                }
                sgen.SetSession(MyGuid, "VOUTYPE", model[0].Col12);
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
                ViewBag.vsavenew = "";
                ViewBag.scripCall = "enableForm();";
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult vou_entry(List<Tmodelmain> model, string command, string hfrow, string hftable)
        {
            FillMst(model[0].Col15);
            DataTable dtm = sgen.settable(hftable);
            model[0].dt1 = dtm;
            var tm = model.FirstOrDefault();
            DataTable dt = new DataTable();
            if (command == "New")
            {
                model = getnew(model);
            }
            else if (command == "Cancel")
            {
                return CancelFun(model);
            }
            else if (command == "Sync")
            {
                bool res = false;
                dt = new DataTable();
                mq = "select a.*,b.c_name as tally_name from vouchers a inner join CLIENTS_MST b on a.acode=b.vch_num and FIND_IN_SET(a.client_unit_id,b.client_unit_id)=1 ";
                dt = sgen.getdata(userCode, mq);
                res = sgen.Tally_Sync(sgen.JV_Multi_2("##SVCURRENTCOMPANY", "Journal", dt));
                if (res)
                {
                    ViewBag.vnew = "";
                    ViewBag.vedit = "";
                    ViewBag.vsave = "disabled='disabled'";
                    ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Synced Successfully');disableForm();";
                }
            }
            else if (command == "Callback")
            {
                model = StartCallback(model);
                if (btnval == "PRINT")
                {
                    ViewBag.vnew = "";
                    ViewBag.vedit = "";
                    ViewBag.vsave = "disabled='disabled'";
                    ViewBag.vsavenew = "disabled='disabled'";
                }
            }
            else if (command == "Save" || command == "Update")
            {
                try
                {
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
                        mq = "select max(to_number(vch_num)) as maxno from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "maxno");
                        model[0].Col16 = vch_num;
                    }
                    DataTable dataTable = sgen.getdata(userCode, "select * from " + model[0].Col10.Trim() + " where  1=2");
                    decimal cr = model[0].dt1.AsEnumerable().Sum(x => sgen.Make_decimal(x["CRAMOUNT"].ToString()));
                    decimal drr = model[0].dt1.AsEnumerable().Sum(x => sgen.Make_decimal(x["DRAMOUNT"].ToString()));
                    if (cr != drr)
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'Cr Amount Should be Equal To Dr Amount ,Please Check', 2);";
                        ModelState.Clear();
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                        return View(model);
                    }
                    string cr_rcode = "";
                    string dr_rcode = "";
                    for (int j = 0; j < model[0].dt1.Rows.Count; j++)
                    {
                        if (model[0].dt1.Rows[j][2].ToString() != "")
                        {
                            if (sgen.Make_decimal(model[0].dt1.Rows[j][4].ToString()) > 0)
                            {
                                if (cr_rcode == "") cr_rcode = model[0].dt1.Rows[j][2].ToString();///this is for debit row
                            }
                            else
                            {
                                dr_rcode = model[0].dt1.Rows[j][2].ToString();///this is for credit row
                            }
                        }
                    }
                    for (int i = 0; i < model[0].dt1.Rows.Count; i++)
                    {
                        DataRow dr = dataTable.NewRow();
                        dr["vch_num"] = vch_num;
                        dr["type"] = model[0].Col12;
                        dr["vch_date"] = currdate;
                        dr["invno"] = model[0].Col19;
                        dr["invdate"] = sgen.Savedate(model[0].Col20, false);
                        dr["totremark"] = model[0].Col21;
                        dr["rno"] = i.ToString();
                        if (sgen.Make_decimal(model[0].dt1.Rows[i][4].ToString()) > 0) dr["rvscode"] = dr_rcode;
                        else dr["rvscode"] = cr_rcode;
                        if (model[0].Col12.Trim() == "10" || model[0].Col12.Trim() == "20" || model[0].Col12.Trim() == "11" || model[0].Col12.Trim() == "21")
                        {
                            try
                            {
                                dr["invno"] = model[0].dt1.Rows[i]["Select_"].ToString().Split(new string[] { "!~!~!" }, StringSplitOptions.None)[0].ToString();
                                dr["invdate"] = sgen.Make_date_S(model[0].dt1.Rows[i]["Select_"].ToString().Split(new string[] { "!~!~!" }, StringSplitOptions.None)[1].ToString());
                                dr["rno"] = model[0].dt1.Rows[i]["Select_"].ToString().Split(new string[] { "!~!~!" }, StringSplitOptions.None)[2].ToString();
                                dr["adj_type"] = model[0].dt1.Rows[i]["Select_"].ToString().Split(new string[] { "!~!~!" }, StringSplitOptions.None)[3].ToString();
                                dr["reftype"] = model[0].dt1.Rows[i]["Select_"].ToString().Split(new string[] { "!~!~!" }, StringSplitOptions.None)[4].ToString();
                                dr["chqamt"] = model[0].dt1.Rows[i]["Select_"].ToString().Split(new string[] { "!~!~!" }, StringSplitOptions.None)[5].ToString();
                                dr["remark"] = model[0].dt1.Rows[i]["Select_"].ToString().Split(new string[] { "!~!~!" }, StringSplitOptions.None)[6].ToString();
                                dr["BALAMT"] = model[0].dt1.Rows[i]["Select_"].ToString().Split(new string[] { "!~!~!" }, StringSplitOptions.None)[7].ToString();
                                dr["invamt"] = model[0].dt1.Rows[i]["Select_"].ToString().Split(new string[] { "!~!~!" }, StringSplitOptions.None)[8].ToString();
                            }
                            catch (Exception ex) { }
                        }
                        //dr["Select_"] = dtt.Rows[i]["invoice_no"].ToString() + "_" + dtt.Rows[i]["invoice_date"].ToString() + "_" 
                        //    + dtt.Rows[i]["type1"].ToString() + "_" + dtt.Rows[i]["adj_type"].ToString() + "_" + dtt.Rows[i]["chqno"].ToString() + "_" + dtt.Rows[i]["chqdate"].ToString() + "_" + dtt.Rows[i]["chqamt"].ToString();
                        dr["acode"] = model[0].dt1.Rows[i]["Acode"].ToString();
                        dr["aname"] = model[0].dt1.Rows[i]["Aname"].ToString();
                        dr["cramount"] = model[0].dt1.Rows[i]["CrAmount"].ToString();
                        dr["dramount"] = model[0].dt1.Rows[i]["DrAmount"].ToString();
                        dr = getsave(currdate, dr, model);
                        dataTable.Rows.Add(dr);
                    }
                    bool Result = sgen.Update_data_fast1(userCode, dataTable, model[0].Col10.Trim(), tmodel.Col8, isedit);
                    if (Result == true)
                    {
                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                        dt = (DataTable)sgen.GetSession(MyGuid, "KDTVOU");
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col13 = "Save",
                            Col100 = tmodel.Col100,
                            Col9 = tmodel.Col9,
                            Col10 = tmodel.Col10,
                            Col11 = tmodel.Col11,
                            Col12 = tmodel.Col12,
                            Col14 = tmodel.Col14,
                            Col15 = tmodel.Col15,
                            dt1 = dt
                        });
                    }
                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
            }
            else if (command == "+")
            {
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
                ViewBag.vsavenew = "";
                if (model[0].dt1.Rows.Count != 0) model[0].dt1.Rows.Add(sgen.Make_int(hfrow));
                else { model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "KDTVOU"); }
            }
            else if (command == "-")
            {
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
                ViewBag.vsavenew = "";
                if (model[0].dt1.Rows.Count > 1) model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                else { model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "KDTVOU"); }
            }

            ModelState.Clear();
            return View(model);
        }
        #endregion
        #region gst_rpt
        public ActionResult gst_rpt()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult gst_rpt(List<Tmodelmain> model, string command)
        {
            string year = year_from.Substring(6, 4);
            string fdt = "", tdt = "", title = "", cl = "", ut = "", qucond = "";
            int seektype = 0;
            FillMst(model[0].Col15);
            var tm = model.FirstOrDefault();
            DataTable dt = new DataTable();
            if (command == "Callback2")
            {
                var mlvl = sgen.Make_int(Multiton.GetSession(MyGuid, "MAXLEVEL").ToString());
                var lvl = sgen.Make_int(Multiton.GetSession(MyGuid, "LEVEL").ToString()) + 2;
                if (mlvl == lvl)
                {
                    command = sgen.GetSession(MyGuid, "KPDCMD2").ToString();
                    sgen.SetSession(MyGuid, "KPDCMD", command);
                }
            }
            if (sgen.GetSession(MyGuid, "KDDL_CL") != null) cl = sgen.GetSession(MyGuid, "KDDL_CL").ToString();
            if (sgen.GetSession(MyGuid, "KDDL_UT") != null) ut = sgen.GetSession(MyGuid, "KDDL_UT").ToString().Replace(",", "','");
            if (sgen.GetSession(MyGuid, "KPDFDT") != null) fdt = sgen.GetSession(MyGuid, "KPDFDT").ToString();
            if (sgen.GetSession(MyGuid, "KPDTDT") != null) tdt = sgen.GetSession(MyGuid, "KPDTDT").ToString();
            if (sgen.GetSession(MyGuid, "KPDCMD") != null) command = sgen.GetSession(MyGuid, "KPDCMD").ToString();
            if (sgen.GetSession(MyGuid, "BDAGCMD") != null) command = sgen.GetSession(MyGuid, "BDAGCMD").ToString();

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
            switch (command.Trim())
            {
                case "Customer Wise Ageing":
                    try
                    {
                        string f1 = "", t1 = "", f2 = "", t2 = "", f3 = "", t3 = "", f4 = "", t4 = "", f5 = "", t5 = "", f6 = "", t6 = "";
                        dt = new DataTable();
                        string currdate = sgen.server_datetime_dt(userCode).ToString("dd/MM/yyyy");
                        dt = sgen.getdata(userCode, "select * from enx_tab where type='BAG' and client_unit_id='" + unitid_mst + "'");
                        if (dt.Rows.Count > 0)
                        {
                            f1 = dt.Rows[0]["col1"].ToString();
                            t1 = dt.Rows[0]["col2"].ToString();
                            f2 = dt.Rows[0]["col3"].ToString();
                            t2 = dt.Rows[0]["col4"].ToString();
                            f3 = dt.Rows[0]["col5"].ToString();
                            t3 = dt.Rows[0]["col6"].ToString();
                            f4 = dt.Rows[0]["col7"].ToString();
                            t4 = dt.Rows[0]["col8"].ToString();
                            f5 = dt.Rows[0]["col9"].ToString();
                            t5 = dt.Rows[0]["col10"].ToString();
                            f6 = dt.Rows[0]["col11"].ToString();
                            t6 = dt.Rows[0]["col12"].ToString();
                            string condi = "";
                            if (dt.Rows[0]["col13"].ToString() == "ABD")
                            {
                                condi = "(sysdate - a.inv_date)";
                                title = "Customer Wise Ageing (By Bill Date)";
                            }
                            else if (dt.Rows[0]["col13"].ToString() == "ADD")
                            {
                                condi = "(sysdate-a.due_date)";
                                title = "Customer Wise Ageing (By Due Date)";
                            }
                            else
                            {
                                condi = "";
                                title = "Customer Wise Ageing";
                            }
                            cmd = "select fstr,party_code,party,nvl(party_closing,'0') as closing_amount,cred_days,advance,on_account,(case when days between " + Convert.ToInt32(f1) + " and " + Convert.ToInt32(t1) + " then amount else '0' end) as Days_" + f1.Trim() + "_" + t1.Trim() + "" +
                                 ",(case when days between " + Convert.ToInt32(f2) + " and " + Convert.ToInt32(t2) + " then amount else '0' end) as Days_" + f2.Trim() + "_" + t2.Trim() + "" +
                                ", (case when days between " + Convert.ToInt32(f3) + " and " + Convert.ToInt32(t3) + " then amount else '0' end) as Days_" + f3.Trim() + "_" + t3.Trim() + "" +
                                ", (case when days between " + Convert.ToInt32(f4) + " and " + Convert.ToInt32(t4) + " then amount else '0' end) as Days_" + f4.Trim() + "_" + t4.Trim() + "" +
                                ", (case when days between " + Convert.ToInt32(f5) + " and " + Convert.ToInt32(t5) + " then amount else '0' end) as Days_" + f5.Trim() + "_" + t5.Trim() + "" +
                                ", (case when days between " + Convert.ToInt32(f6) + " and " + Convert.ToInt32(t6) + " then amount else '0' end) as Days_" + f6.Trim() + "_" + t6.Trim() + " " +
                                "from (select a.inv_no as fstr, a.inv_no, to_char(a.inv_date, '" + sgen.Getsqldateformat() + "') as inv_date, to_char(a.due_date, '" + sgen.Getsqldateformat() + "') as due_date, a.party_code, a.party, a.cred_days, to_char(a.closing) as party_closing, to_char(a.on_account) as on_account, to_char(a.amount) amount, a.advance," +
                                "" + condi + " as days from(select max(a.vch_num) as inv_no, max(a.vch_date) as inv_date, max(a.vch_date) + max(nvl(cl.credit_days, '0')) as due_date, cl.vch_num as party_code, cl.c_name as party, max(nvl(cl.credit_days, '0')) as cred_days, sum(a.amt) as amount, sum(a.advance) as advance,max(p.closing) as closing, sum(a.onacc) as on_account from " +
                                "clients_mst cl left join (" +
                                //"select i.acode,i.INVNO vch_num,i.INVDATE vch_date,to_number(i.netamt) as billamt,-to_number(i.netamt) as amt,0 as advance,0 as onacc, i.client_unit_id, i.type from itransaction i" +
                                //" where i.type in ('01', '02', '03') and client_unit_id = '" + unitid_mst + "' " +
                                //"union all " +
                                //"select i.acode, i.vch_num as doc_no,i.vch_date doc_date, to_number(i.netamt) as billamt,to_number(i.netamt) as amt" +
                                //",0 as advance,0 as onacc, i.client_unit_id, i.type from itransaction i where substr(i.type,1,1)= '4' and i.client_unit_id = '" + unitid_mst + "' and substr(trim(i.acode),1,3)='303' " +
                                " select acode, invno as vch_num, invdate as vch_date," +
                                "0 as billamt,0 as amt,(to_number(dramount) - to_number(cramount)) as advance,0 as onacc, client_unit_id, adj_type as type from VOUCHERS where adj_type in ('ADV') and client_unit_id = '" + unitid_mst + "' and substr(trim(acode),1,3) = '303'" +
                                "UNION ALL select acode, invno, invdate,0 as billamt,0 as amt,0 as advance,(to_number(dramount) - to_number(cramount)) as onacc, client_unit_id, adj_type as type from VOUCHERS where adj_type in ('OAC') " +
                                "and client_unit_id = '" + unitid_mst + "' and substr(trim(acode),1,3) = '303' union all select i.acode,i.invno,i.invdate doc_date,(to_number(i.dramount) - to_number(i.cramount)) as billamt,(to_number(i.dramount) - to_number(i.cramount)) as amt" +
                                ",0 as advance,0 as onacc, i.client_unit_id,i.type from vouchers i where i.client_unit_id = '" + unitid_mst + "' and i.adj_type in ('AGR','NEW') and substr(trim(i.acode),1,3) = '303') a on cl.vch_num = a.acode LEFT JOIN(SELECT ACODE, CLIENT_UNIT_ID, " +
                                "SUM(OPBAL) AS OPBAL, SUM(DRAMOUNT), SUM(CRAMOUNT), SUM(OPBAL) + SUM(DRAMOUNT) - SUM(CRAMOUNT) as closing from(select acode, CLIENT_UNIT_ID, OP_" + year + " as opbal, 0 as cramount, 0 as dramount, 0 as cl from acbal union all " +
                                "select acode, CLIENT_UNIT_ID, sum(dramount) - sum(cramount) as opbal, 0 as cramount, 0 as dramount, 0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and " +
         "to_date('" + year_from.Split(' ')[0].Trim() + "','" + sgen.Getsqldateformat() + "')-1 group by acode, CLIENT_UNIT_ID union all select acode, CLIENT_UNIT_ID, 0 as opbal, TO_NUMBER(cramount) cramount, TO_NUMBER(dramount) dramount, 0 cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
         "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + currdate.Trim() + "','" + sgen.Getsqldateformat() + "')) group by acode, CLIENT_UNIT_ID) p on p.acode = cl.vch_num and find_in_set(cl.client_unit_id, p.client_unit_id) = 1 " +
         "LEFT JOIN(SELECT ACODE, CLIENT_UNIT_ID, SUM(OPBAL) AS OPBAL, SUM(DRAMOUNT), SUM(CRAMOUNT), SUM(OPBAL) + SUM(DRAMOUNT) - SUM(CRAMOUNT) as onacc from (select acode, CLIENT_UNIT_ID, OP_" + year + " as opbal, 0 as cramount, 0 as dramount, 0 as cl from acbal union all select acode, CLIENT_UNIT_ID, sum(dramount) - sum(cramount) as opbal, 0 as cramount, 0 as dramount, 0 as cl from vouchers where rowid not in (select rowid from vouchers where substr(type, 1, 1) = '1'  AND(adj_type = 'AGR' or REFTYPE = 'ADV')) and substr(type, 1, 1) not in '4' " +
         "and to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and " +
         "to_date('" + year_from.Split(' ')[0].Trim() + "','" + sgen.Getsqldateformat() + "')-1 group by acode, CLIENT_UNIT_ID union all select acode, CLIENT_UNIT_ID, 0 as opbal, TO_NUMBER(cramount) cramount, TO_NUMBER(dramount) dramount, 0 cl from vouchers where rowid not in (select rowid from vouchers where substr(type, 1, 1) = '1' AND (adj_type = 'AGR' and adj_type = 'ADV')) and substr(type, 1, 1) not in '4' " +
         "and to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
         "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + currdate.Trim() + "','" + sgen.Getsqldateformat() + "')) t group by acode, CLIENT_UNIT_ID) d on d.acode = cl.vch_num and find_in_set(cl.client_unit_id, d.client_unit_id)= 1 where cl.type= 'BCD' and " +
                                "find_in_set(cl.client_unit_id, '" + unitid_mst + "') = 1 and substr(trim(cl.vch_num),1,3) = '303' group by cl.c_name,cl.vch_num) a)";
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    //title = "Ageing Party Wise";
                    seektype = 0;
                    sgen.SetSession(MyGuid, "BDAGCMD", null);
                    break;
                case "Vendor Wise Ageing":
                    try
                    {
                        string f1 = "", t1 = "", f2 = "", t2 = "", f3 = "", t3 = "", f4 = "", t4 = "", f5 = "", t5 = "", f6 = "", t6 = "";
                        dt = new DataTable();
                        string currdate = sgen.server_datetime_dt(userCode).ToString("dd/MM/yyyy");
                        dt = sgen.getdata(userCode, "select * from enx_tab where type='BAG' and client_unit_id='" + unitid_mst + "'");
                        if (dt.Rows.Count > 0)
                        {
                            f1 = dt.Rows[0]["col1"].ToString();
                            t1 = dt.Rows[0]["col2"].ToString();
                            f2 = dt.Rows[0]["col3"].ToString();
                            t2 = dt.Rows[0]["col4"].ToString();
                            f3 = dt.Rows[0]["col5"].ToString();
                            t3 = dt.Rows[0]["col6"].ToString();
                            f4 = dt.Rows[0]["col7"].ToString();
                            t4 = dt.Rows[0]["col8"].ToString();
                            f5 = dt.Rows[0]["col9"].ToString();
                            t5 = dt.Rows[0]["col10"].ToString();
                            f6 = dt.Rows[0]["col11"].ToString();
                            t6 = dt.Rows[0]["col12"].ToString();
                            string condi = "";
                            if (dt.Rows[0]["col13"].ToString() == "ABD")
                            {
                                condi = "(sysdate - a.inv_date)";
                                title = "Vendor Wise Ageing (By Bill Date)";
                            }
                            else if (dt.Rows[0]["col13"].ToString() == "ADD")
                            {
                                condi = "(sysdate-a.due_date)";
                                title = "Vendor Wise Ageing (By Due Date)";
                            }
                            else
                            {
                                condi = "0";
                                title = "Vendor Wise Ageing";
                            }
                            cmd = "select fstr,party_code,party,nvl(party_closing,'0') as closing_amount,cred_days,advance,on_account,(case when days between " + Convert.ToInt32(f1) + " and " + Convert.ToInt32(t1) + " then amount else '0' end) as Days_" + f1.Trim() + "_" + t1.Trim() + "" +
                                 ",(case when days between " + Convert.ToInt32(f2) + " and " + Convert.ToInt32(t2) + " then amount else '0' end) as Days_" + f2.Trim() + "_" + t2.Trim() + "" +
                                ", (case when days between " + Convert.ToInt32(f3) + " and " + Convert.ToInt32(t3) + " then amount else '0' end) as Days_" + f3.Trim() + "_" + t3.Trim() + "" +
                                ", (case when days between " + Convert.ToInt32(f4) + " and " + Convert.ToInt32(t4) + " then amount else '0' end) as Days_" + f4.Trim() + "_" + t4.Trim() + "" +
                                ", (case when days between " + Convert.ToInt32(f5) + " and " + Convert.ToInt32(t5) + " then amount else '0' end) as Days_" + f5.Trim() + "_" + t5.Trim() + "" +
                                ", (case when days between " + Convert.ToInt32(f6) + " and " + Convert.ToInt32(t6) + " then amount else '0' end) as Days_" + f6.Trim() + "_" + t6.Trim() + " " +
                                "from (select a.inv_no as fstr, a.inv_no, to_char(a.inv_date, '" + sgen.Getsqldateformat() + "') as inv_date, to_char(a.due_date, '" + sgen.Getsqldateformat() + "') as due_date, a.party_code, a.party, a.cred_days, to_char(a.closing) as party_closing, to_char(a.on_account) as on_account, to_char(a.amount) amount, a.advance," +
                                "" + condi + " as days from(select max(a.vch_num) as inv_no, max(a.vch_date) as inv_date, max(a.vch_date) + max(nvl(cl.credit_days, '0')) as due_date, cl.vch_num as party_code, cl.c_name as party, max(nvl(cl.credit_days, '0')) as cred_days, sum(a.amt) as amount, sum(a.advance) as advance,max(p.closing) as closing, sum(a.onacc) as on_account from " +
                                "clients_mst cl left join (" +
                                //"select i.acode,i.INVNO vch_num,i.INVDATE vch_date,to_number(i.netamt) as billamt,-to_number(i.netamt) as amt,0 as advance,0 as onacc, i.client_unit_id, i.type from itransaction i" +
                                //" where i.type in ('01', '02', '03') and client_unit_id = '" + unitid_mst + "' " +
                                //"union all " +
                                //"select i.acode, i.vch_num as doc_no,i.vch_date doc_date, to_number(i.netamt) as billamt,to_number(i.netamt) as amt" +
                                //",0 as advance,0 as onacc, i.client_unit_id, i.type from itransaction i where substr(i.type,1,1)= '4' and i.client_unit_id = '" + unitid_mst + "' and substr(trim(i.acode),1,3)='303' " +
                                " select acode, invno as vch_num, invdate as vch_date," +
                                "0 as billamt,0 as amt,(to_number(dramount) - to_number(cramount)) as advance,0 as onacc, client_unit_id, adj_type as type from VOUCHERS where adj_type in ('ADV') and client_unit_id = '" + unitid_mst + "' and substr(trim(acode),1,3) = '203'" +
                                "UNION ALL select acode, invno, invdate,0 as billamt,0 as amt,0 as advance,(to_number(dramount) - to_number(cramount)) as onacc, client_unit_id, adj_type as type from VOUCHERS where adj_type in ('OAC') " +
                                "and client_unit_id = '" + unitid_mst + "' and substr(trim(acode),1,3) = '203' union all select i.acode,i.invno,i.invdate doc_date,(to_number(i.dramount) - to_number(i.cramount)) as billamt,(to_number(i.dramount) - to_number(i.cramount)) as amt" +
                                ",0 as advance,0 as onacc, i.client_unit_id,i.type from vouchers i where i.client_unit_id = '" + unitid_mst + "' and i.adj_type in ('AGR','NEW') and substr(trim(i.acode),1,3) = '203') a on cl.vch_num = a.acode LEFT JOIN(SELECT ACODE, CLIENT_UNIT_ID, " +
                                "SUM(OPBAL) AS OPBAL, SUM(DRAMOUNT), SUM(CRAMOUNT), SUM(OPBAL) + SUM(DRAMOUNT) - SUM(CRAMOUNT) as closing from(select acode, CLIENT_UNIT_ID, OP_" + year + " as opbal, 0 as cramount, 0 as dramount, 0 as cl from acbal union all " +
                                "select acode, CLIENT_UNIT_ID, sum(dramount) - sum(cramount) as opbal, 0 as cramount, 0 as dramount, 0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and " +
         "to_date('" + year_from.Split(' ')[0].Trim() + "','" + sgen.Getsqldateformat() + "')-1 group by acode, CLIENT_UNIT_ID union all select acode, CLIENT_UNIT_ID, 0 as opbal, TO_NUMBER(cramount) cramount, TO_NUMBER(dramount) dramount, 0 cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
         "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + currdate.Trim() + "','" + sgen.Getsqldateformat() + "')) group by acode, CLIENT_UNIT_ID) p on p.acode = cl.vch_num and find_in_set(cl.client_unit_id, p.client_unit_id) = 1 " +
         "LEFT JOIN(SELECT ACODE, CLIENT_UNIT_ID, SUM(OPBAL) AS OPBAL, SUM(DRAMOUNT), SUM(CRAMOUNT), SUM(OPBAL) + SUM(DRAMOUNT) - SUM(CRAMOUNT) as onacc from (select acode, CLIENT_UNIT_ID, OP_" + year + " as opbal, 0 as cramount, 0 as dramount, 0 as cl from acbal union all select acode, CLIENT_UNIT_ID, sum(dramount) - sum(cramount) as opbal, 0 as cramount, 0 as dramount, 0 as cl from vouchers where rowid not in (select rowid from vouchers where substr(type, 1, 1) = '1'  AND(adj_type = 'AGR' or REFTYPE = 'ADV')) and substr(type, 1, 1) not in '4' " +
         "and to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and " +
         "to_date('" + year_from.Split(' ')[0].Trim() + "','" + sgen.Getsqldateformat() + "')-1 group by acode, CLIENT_UNIT_ID union all select acode, CLIENT_UNIT_ID, 0 as opbal, TO_NUMBER(cramount) cramount, TO_NUMBER(dramount) dramount, 0 cl from vouchers where rowid not in (select rowid from vouchers where substr(type, 1, 1) = '1' AND (adj_type = 'AGR' and adj_type = 'ADV')) and substr(type, 1, 1) not in '4' " +
         "and to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
         "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + currdate.Trim() + "','" + sgen.Getsqldateformat() + "')) t group by acode, CLIENT_UNIT_ID) d on d.acode = cl.vch_num and find_in_set(cl.client_unit_id, d.client_unit_id)= 1 where cl.type= 'BCD' and " +
                                "find_in_set(cl.client_unit_id, '" + unitid_mst + "') = 1  and substr(trim(cl.vch_num),1,3) = '203' group by cl.c_name,cl.vch_num) a)";
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    //title = "Ageing Party Wise";
                    seektype = 0;
                    sgen.SetSession(MyGuid, "BDAGCMD", null);
                    break;
                case "Bill Wise Ageing (Customer)":
                    try
                    {
                        string f1 = "", t1 = "", f2 = "", t2 = "", f3 = "", t3 = "", f4 = "", t4 = "", f5 = "", t5 = "", f6 = "", t6 = "";
                        dt = new DataTable();
                        string currdate = sgen.server_datetime_dt(userCode).ToString("dd/MM/yyyy");
                        dt = sgen.getdata(userCode, "select * from enx_tab where type='BAG' and client_unit_id='" + unitid_mst + "'");
                        if (dt.Rows.Count > 0)
                        {
                            f1 = dt.Rows[0]["col1"].ToString();
                            t1 = dt.Rows[0]["col2"].ToString();
                            f2 = dt.Rows[0]["col3"].ToString();
                            t2 = dt.Rows[0]["col4"].ToString();
                            f3 = dt.Rows[0]["col5"].ToString();
                            t3 = dt.Rows[0]["col6"].ToString();
                            f4 = dt.Rows[0]["col7"].ToString();
                            t4 = dt.Rows[0]["col8"].ToString();
                            f5 = dt.Rows[0]["col9"].ToString();
                            t5 = dt.Rows[0]["col10"].ToString();
                            f6 = dt.Rows[0]["col11"].ToString();
                            t6 = dt.Rows[0]["col12"].ToString();
                            string condi = "";
                            if (dt.Rows[0]["col13"].ToString() == "ABD")
                            {
                                condi = "(sysdate - a.inv_date)";
                                title = "Ageing Bill Wise (By Bill Date)";
                            }
                            else if (dt.Rows[0]["col13"].ToString() == "ADD")
                            {
                                condi = "(sysdate-a.due_date)";
                                title = "Ageing Bill Wise (By Due Date)";
                            }
                            else
                            {
                                condi = "0";
                                title = "Ageing Bill Wise";
                            }
                            cmd = "select fstr,inv_no,inv_date,due_date,party_code,party,cred_days,on_account,(case when days between " + Convert.ToInt32(f1) + " and " + Convert.ToInt32(t1) + " then amount else '0' end) as Days_" + f1.Trim() + "_" + t1.Trim() + "" +
                              ",(case when days between " + Convert.ToInt32(f2) + " and " + Convert.ToInt32(t2) + " then amount else '0' end) as Days_" + f2.Trim() + "_" + t2.Trim() + "" +
                             ", (case when days between " + Convert.ToInt32(f3) + " and " + Convert.ToInt32(t3) + " then amount else '0' end) as Days_" + f3.Trim() + "_" + t3.Trim() + "" +
                             ", (case when days between " + Convert.ToInt32(f4) + " and " + Convert.ToInt32(t4) + " then amount else '0' end) as Days_" + f4.Trim() + "_" + t4.Trim() + "" +
                             ", (case when days between " + Convert.ToInt32(f5) + " and " + Convert.ToInt32(t5) + " then amount else '0' end) as Days_" + f5.Trim() + "_" + t5.Trim() + "" +
                             ", (case when days between " + Convert.ToInt32(f6) + " and " + Convert.ToInt32(t6) + " then amount else '0' end) as Days_" + f6.Trim() + "_" + t6.Trim() + " " +
                             "from (select a.inv_no as fstr, a.inv_no, to_char(a.inv_date, 'dd/MM/YYYY') as inv_date, to_char(a.due_date, 'dd/MM/YYYY') as due_date, a.party_code,a.party, a.cred_days, to_char(a.amount) amount, a.on_account," + condi + " as days" +
                             " from(select a.vch_num as inv_no, a.vch_date as inv_date, a.vch_date + max(nvl(cl.credit_days, '0')) as due_date,a.acode as party_code,max(cl.c_name) as party, max(nvl(cl.credit_days, '0')) as cred_days, sum(a.amt) as amount, sum(a.advance) as on_account from clients_mst cl left join (" +
                             //"select i.acode,i.INVNO vch_num,i.INVDATE vch_date,to_number(i.netamt) as billamt,-to_number(i.netamt) as amt,0 as advance, i.client_unit_id, i.type from itransaction i where " +
                             //"i.type in ('01', '02', '03') and client_unit_id = '" + unitid_mst + "' " +
                             //"union all select i.acode, i.vch_num as doc_no,i.vch_date doc_date, to_number(i.netamt) as billamt,to_number(i.netamt) as amt,0 as advance, i.client_unit_id, i.type from " +
                             //"itransaction i where substr(i.type,1,1)= '4' and substr(trim(i.acode),1,3)='303' and client_unit_id = '" + unitid_mst + "' " +
                             " select acode, invno vch_num, invdate vch_date,0 as billamt,0 as amt,(to_number(dramount) - to_number(cramount)) as advance, client_unit_id, adj_type as type " +
                             "from VOUCHERS where adj_type in ('ADV','OAC') and client_unit_id = '" + unitid_mst + "' and substr(trim(acode),1,3) in ('303') UNION ALL select i.acode,i.invno,i.invdate doc_date,(to_number(i.dramount) - to_number(i.cramount)) as billamt,(to_number(i.dramount) - to_number(i.cramount)) as amt,0 as advance" +
                             ", i.client_unit_id,i.type from vouchers i where i.client_unit_id = '" + unitid_mst + "' and i.adj_type in ('AGR','NEW') and substr(trim(i.acode),1,3) in ('303')) a on cl.vch_num = a.acode where find_in_set(cl.client_unit_id, '" + unitid_mst + "') = 1 group by a.vch_num,a.vch_date,a.acode) a)";
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    seektype = 0;
                    sgen.SetSession(MyGuid, "BDAGCMD", null);
                    break;
                case "Bill Wise Ageing (Vendor)":
                    try
                    {
                        string f1 = "", t1 = "", f2 = "", t2 = "", f3 = "", t3 = "", f4 = "", t4 = "", f5 = "", t5 = "", f6 = "", t6 = "";
                        dt = new DataTable();
                        string currdate = sgen.server_datetime_dt(userCode).ToString("dd/MM/yyyy");
                        dt = sgen.getdata(userCode, "select * from enx_tab where type='BAG' and client_unit_id='" + unitid_mst + "'");
                        if (dt.Rows.Count > 0)
                        {
                            f1 = dt.Rows[0]["col1"].ToString();
                            t1 = dt.Rows[0]["col2"].ToString();
                            f2 = dt.Rows[0]["col3"].ToString();
                            t2 = dt.Rows[0]["col4"].ToString();
                            f3 = dt.Rows[0]["col5"].ToString();
                            t3 = dt.Rows[0]["col6"].ToString();
                            f4 = dt.Rows[0]["col7"].ToString();
                            t4 = dt.Rows[0]["col8"].ToString();
                            f5 = dt.Rows[0]["col9"].ToString();
                            t5 = dt.Rows[0]["col10"].ToString();
                            f6 = dt.Rows[0]["col11"].ToString();
                            t6 = dt.Rows[0]["col12"].ToString();
                            string condi = "";
                            if (dt.Rows[0]["col13"].ToString() == "ABD")
                            {
                                condi = "(sysdate - a.inv_date)";
                                title = "Ageing Bill Wise (By Bill Date)";
                            }
                            else if (dt.Rows[0]["col13"].ToString() == "ADD")
                            {
                                condi = "(sysdate-a.due_date)";
                                title = "Ageing Bill Wise (By Due Date)";
                            }
                            else
                            {
                                condi = "0";
                                title = "Ageing Bill Wise";
                            }
                            cmd = "select fstr,inv_no,inv_date,due_date,party_code,party,cred_days,on_account,(case when days between " + Convert.ToInt32(f1) + " and " + Convert.ToInt32(t1) + " then amount else '0' end) as Days_" + f1.Trim() + "_" + t1.Trim() + "" +
                              ",(case when days between " + Convert.ToInt32(f2) + " and " + Convert.ToInt32(t2) + " then amount else '0' end) as Days_" + f2.Trim() + "_" + t2.Trim() + "" +
                             ", (case when days between " + Convert.ToInt32(f3) + " and " + Convert.ToInt32(t3) + " then amount else '0' end) as Days_" + f3.Trim() + "_" + t3.Trim() + "" +
                             ", (case when days between " + Convert.ToInt32(f4) + " and " + Convert.ToInt32(t4) + " then amount else '0' end) as Days_" + f4.Trim() + "_" + t4.Trim() + "" +
                             ", (case when days between " + Convert.ToInt32(f5) + " and " + Convert.ToInt32(t5) + " then amount else '0' end) as Days_" + f5.Trim() + "_" + t5.Trim() + "" +
                             ", (case when days between " + Convert.ToInt32(f6) + " and " + Convert.ToInt32(t6) + " then amount else '0' end) as Days_" + f6.Trim() + "_" + t6.Trim() + " " +
                             "from (select a.inv_no as fstr, a.inv_no, to_char(a.inv_date, 'dd/MM/YYYY') as inv_date, to_char(a.due_date, 'dd/MM/YYYY') as due_date, a.party_code,a.party, a.cred_days, to_char(a.amount) amount, a.on_account," + condi + " as days" +
                             " from(select a.vch_num as inv_no, a.vch_date as inv_date, a.vch_date + max(nvl(cl.credit_days, '0')) as due_date,a.acode as party_code,max(cl.c_name) as party, max(nvl(cl.credit_days, '0')) as cred_days, sum(a.amt) as amount, sum(a.advance) as on_account from clients_mst cl left join (" +
                             //"select i.acode,i.INVNO vch_num,i.INVDATE vch_date,to_number(i.netamt) as billamt,-to_number(i.netamt) as amt,0 as advance, i.client_unit_id, i.type from itransaction i where " +
                             //"i.type in ('01', '02', '03') and client_unit_id = '" + unitid_mst + "' " +
                             //"union all select i.acode, i.vch_num as doc_no,i.vch_date doc_date, to_number(i.netamt) as billamt,to_number(i.netamt) as amt,0 as advance, i.client_unit_id, i.type from " +
                             //"itransaction i where substr(i.type,1,1)= '4' and substr(trim(i.acode),1,3)='303' and client_unit_id = '" + unitid_mst + "' " +
                             " select acode, invno vch_num, invdate vch_date,0 as billamt,0 as amt,(to_number(dramount) - to_number(cramount)) as advance, client_unit_id, adj_type as type " +
                             "from VOUCHERS where adj_type in ('ADV','OAC') and client_unit_id = '" + unitid_mst + "' and substr(trim(acode),1,3) in ('203') UNION ALL select i.acode,i.invno,i.invdate doc_date,(to_number(i.dramount) - to_number(i.cramount)) as billamt,(to_number(i.dramount) - to_number(i.cramount)) as amt,0 as advance" +
                             ", i.client_unit_id,i.type from vouchers i where i.client_unit_id = '" + unitid_mst + "' and i.adj_type in ('AGR','NEW') and substr(trim(i.acode),1,3) in ('203')) a on cl.vch_num = a.acode where find_in_set(cl.client_unit_id, '" + unitid_mst + "') = 1 group by a.vch_num,a.vch_date,a.acode) a)";
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    seektype = 0;
                    sgen.SetSession(MyGuid, "BDAGCMD", null);
                    break;
                #region other 
                case "Item Wise Sale":
                    cmd = "select '-' as fstr, it.icode,it.iname,hsn.master_name as hsn_code,iv.IGST,iv.SGST,iv.CGST,iv.basic_amt,iv.GrossAmt,to_char(iv.net_amount) as net_amount from item it inner join(select iv.icode as item_code,iv.iname as item_name,sum(iv.gserchrg) as IGST,sum(iv.gloadchrg) as SGST,sum(iv.gamc) as CGST,sum(iv.basicamt) as basic_amt,sum(iv.ginstlchrg) GrossAmt,sum(iv.netamt) as net_amount from itransaction iv where substr(iv.type, 1, 1) = '4' and substr(iv.potype,1,1)= '5' and iv.client_id = '" + cl + "' and iv.client_unit_id = '" + ut + "' and to_date(iv.vch_date) between " +
                         "to_date('" + fdt + "','" + sgen.Getsqldateformat() + "') and to_date('" + tdt + "','" + sgen.Getsqldateformat() + "') group by(iv.icode, iv.iname) ) iv on it.icode = iv.item_code inner join master_setting hsn on hsn.master_id = it.hsno and hsn.type = 'HSN' and find_in_set(hsn.client_id, it.client_id)= 1 and find_in_set(hsn.client_unit_id, it.client_unit_id)= 1 where it.type = 'IT' and it.sale='Y' and it.client_id = '" + cl + "' and it.client_unit_id = '" + ut + "'";
                    title = "Item Wise Sale " + date_period + "";
                    break;
                case "Party Ledger":
                    //cmd = "select distinct a.vch_num||To_Char(a.vch_date, 'yyyymmdd')|| a.type as fstr,a.vch_num as part_code,a.c_name as party_name,a.c_gstin as gstin,a.addr as Address,a.tor,nvl(ba.country_name,'-') as country_name,nvl(ba.state_name,'-') state_name,nvl(ba.city_name,'-') as city_name,a.pincode from clients_mst a inner join vouchers v on v.acode = a.vch_num and v.client_unit_id = a.client_unit_id and to_date(v.vch_date) between " +
                    //     "to_date('" + fdt + "','" + sgen.Getsqldateformat() + "') and to_date('" + tdt + "','" + sgen.Getsqldateformat() + "') left join country_state ba on ba.rec_id = a.city2 where a.type in ('BCD','BAD','LDG') and a.client_unit_id = '" + ut + "'";

                    cmd = "select distinct a.acode||To_Char(a.vch_date, 'yyyymmdd')|| a.type as fstr,a.acode,a.aname as ledger_name from (select vch_num as acode,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'BCD' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                "union all select vch_num as acode ,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 union all " +
                                "select cm.vch_num as acode,nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname,cm.CLIENT_UNIT_ID,cm.vch_date,cm.type from master_setting a inner join clients_mst cm on " +
                                "a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id where cm.type = 'BAD' and cm.client_unit_id = '" + unitid_mst + "') a inner join vouchers v on v.acode = a.acode and v.client_unit_id = a.CLIENT_UNIT_ID and to_date(v.vch_date) between " +
                         "to_date('" + fdt + "','" + sgen.Getsqldateformat() + "') and to_date('" + tdt + "','" + sgen.Getsqldateformat() + "')";
                    btnval = "PARTYLEDGDET";
                    seektype = 1;
                    sgen.SetSession(MyGuid, "btnval", btnval);
                    title = "Party Ledger " + date_period + "";
                    break;
                case "Invoice Wise Sales Summary":
                    cmd = "select (p.client_id||p.client_unit_id||p.vch_num||to_char(p.vch_date,'yyyymmdd')||p.type) as fstr,p.vch_num Invoice_Number,to_char(convert_tz(p.vch_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') Invoice_Date,(case when p.potype = '51' then 'Sales Order' when p.potype = '52' then 'Dispatch schedule' when p.potype = '53' then 'Disp Chl' when p.potype = '55' then 'With PI' else 'Direct Invoice' end) as Against,c.c_name as Party_Name,c.c_gstin as Party_Gstin,c.addr as Party_Address,p.icode,p.iname as item_name,p.gdisc Gross_Discount, p.discrate,p.iamount, nvl(sf.c_name, '-') as From_Name,nvl(sf.c_gstin, '-') as From_Gstin,nvl(sf.cpaddr, '-') FromAddress,nvl(st.c_name, '-') as To_Name,nvl(st.c_gstin, '-') as To_Gstin,nvl(st.cpaddr, '-') ToAddress," +
                         "p.gothrchrg po_no, p.gserchrg IGST, p.gloadchrg SGST, p.gamc CGST , p.basicamt BasicAmt, p.ginstlchrg GrossAmt, p.netamt net_amount" +
                         " from itransaction p inner join clients_mst c on p.acode = c.vch_num and c.client_id = p.client_id and c.client_unit_id = p.client_unit_id and c.type = 'BCD' " +
                         "left join clients_mst sf on p.shipfrom = sf.vch_num and sf.client_id = p.client_id and sf.client_unit_id = p.client_unit_id and sf.type = 'BCD' " +
                         "left join clients_mst st on p.shipto = st.vch_num and st.client_id = p.client_id and st.client_unit_id = p.client_unit_id and st.type = 'BCD' " +
                         "where p.client_id = '" + cl + "' and p.client_unit_id = '" + ut + "' and SUBSTR(p.type, 1, 1) = '4' and substr(p.potype,1,1)= '5' and to_date(p.vch_date) between to_date('" + fdt + "','" + sgen.Getsqldateformat() + "') and to_date('" + tdt + "','" + sgen.Getsqldateformat() + "') order by p.vch_num";
                    title = "Invoice Wise Sales Summary " + date_period + "";
                    break;
                case "Party Wise Sale":
                    cmd = "select '-' as fstr, iv.acode as party_code,cl.C_name as Party_name,cl.c_gstin as Gst_no,sum(iv.gserchrg) as IGST,sum(iv.gloadchrg) as SGST,sum(iv.gamc) as CGST,to_char(sum(iv.basicamt)) as basic_amt,to_char(sum(iv.ginstlchrg)) GrossAmt,to_char(sum(iv.netamt)) as net_amount from itransaction iv inner join clients_mst cl on cl.vch_num = iv.acode and cl.client_id = iv.client_id and cl.client_unit_id = iv.client_unit_id and cl.type = 'BCD' where substr(iv.type,1,1)= '4' and iv.client_id = '" + cl + "' and iv.client_unit_id = '" + ut + "' and to_date(iv.vch_date) between to_date('" + fdt + "','" + sgen.Getsqldateformat() + "') and to_date('" + tdt + "','" + sgen.Getsqldateformat() + "') group by(iv.acode, cl.C_name, cl.c_gstin)";
                    title = "Party Wise Sale " + date_period + "";
                    break;
                case "Purchase Order Detailed":
                    cmd = "select (p.client_id||p.client_unit_id||p.vch_num||to_char(p.vch_date,'yyyymmdd')||p.type) as fstr,p.vch_num PO_Number," +
                    "to_char(p.vch_date, 'dd/MM/YYYY') PO_Date,c.vch_num as party_code,c.c_name as Party_Name,c.c_gstin as Party_Gstin,c.addr Address," +
                    " p.icode ItemCode, p.iname ItemName, p.cpartno PartNo, p.uom UOM, p.hsno HSN, p.qtystk Stock_Qty," +
                    "p.qtyord Order_Qty, p.irate ItemRate, p.taxrate TaxRate, iamount Item_Amt,p.taxamt taxamt, p.discrate Discount_Rate" +
                    ",p.discamt Discount_Amt, p.basicamt BasicAmt, p.lineamount LineAmt, p.netamt,p.indno IndentNo,to_char(p.inddate, 'dd/MM/YYYY') Indent_Date, p.qtyind Indent_Qty, to_char(p.dlv_date, 'dd/MM/YYYY') Delivery_Date" +
                    " from purchases p inner join clients_mst c on p.acode = c.vch_num and c.client_id = p.client_id " +
                    "and c.client_unit_id = p.client_unit_id and c.type = 'BCD' and substr(c.vch_num,0,2)= '21' left join clients_mst sf on p.shipfrom = sf.vch_num " +
                    "and sf.client_id = p.client_id and sf.client_unit_id = p.client_unit_id and sf.type = 'BCD' and substr(sf.vch_num,0,2)= '21' " +
                    "left join clients_mst st on p.shipto = st.vch_num and st.client_id = p.client_id and st.client_unit_id = p.client_unit_id and " +
                    "st.type = 'BCD' and substr(st.vch_num,0,2)= '21' where p.client_id = '" + cl + "' and p.client_unit_id = '" + ut + "'" +
                    " and p.type in ('50', '52') and to_date(to_char(p.vch_date,'DDMMYYYY'),'DDMMYYYY') between to_date('" + fdt + "','dd/mm/yyyy') " +
                    "and to_date('" + tdt + "','dd/mm/yyyy')";
                    title = "Purchase Order Detailed " + date_period + "";
                    break;
                case "HSN Wise Sale Summary":
                    //cmd = "select '-' fstr, hsn.master_name as hsn_code,hsn.content,iv.IGST,iv.SGST,iv.CGST,iv.basic_amt,iv.net_amount from item it inner join (select iv.icode as item_code, iv.vch_date, " +
                    // "sum(iv.gserchrg) as IGST,sum(iv.gloadchrg) as SGST,sum(iv.gamc) as CGST,sum(iv.basicamt) as basic_amt,sum(iv.ginstlchrg) GrossAmt,sum(iv.netamt) as net_amount from itransaction iv where " +
                    // "substr(iv.type, 1, 1) = '4' and substr(iv.potype, 1, 1) = '5' and iv.client_id = '" + cl + "' " +
                    // "and iv.client_unit_id = '" + ut + "' group by(iv.icode, iv.vch_date)) iv on iv.item_code = it.icode inner join master_setting" +
                    // " hsn on hsn.master_id = it.hsno and hsn.type = 'HSN' and find_in_set(hsn.client_id, it.client_id)= 1 and find_in_set(hsn.client_unit_id," +
                    // " it.client_unit_id)= 1 where it.type = 'IT' and it.sale='Y' and it.client_id = '" + cl + "' and " +
                    // "it.client_unit_id = '" + ut + "' and to_date(iv.vch_date) between to_date('" + fdt + "','dd/mm/yyyy') and to_date('" + tdt + "','dd/mm/yyyy')";

                    cmd = "select '-' fstr, it.master_name as hsn_code,it.content,iv.IGST,iv.SGST,iv.CGST,iv.basic_amt,to_char(iv.net_amount) as net_amount from master_setting it inner join (select iv.hsno, iv.client_unit_id, max(iv.vch_date) as vch_date," +
                      "sum(iv.gserchrg) as IGST, sum(iv.gloadchrg) as SGST, sum(iv.gamc) as CGST, sum(iv.basicamt) as basic_amt, sum(iv.ginstlchrg) GrossAmt, sum(iv.netamt) as net_amount from itransaction iv where substr(iv.type, 1, 1) = '4' " +
                      "and substr(iv.potype, 1, 1) = '5' and iv.client_unit_id = '" + ut + "' group by(iv.hsno, iv.client_unit_id)) iv on iv.hsno = it.master_name and find_in_set(iv.client_unit_id, it.client_unit_id)= 1 where it.type = 'HSN' " +
                      "and find_in_set(it.client_unit_id, '" + ut + "') = 1 and to_date(iv.vch_date) between to_date('" + fdt + "','" + sgen.Getsqldateformat() + "') and to_date('" + tdt + "','" + sgen.Getsqldateformat() + "')";
                    title = "HSN Wise Sale Summary " + date_period + "";
                    break;
                #endregion
                #region trial bal
                case "Trial Balance":
                    //cmd = "select (a.client_unit_id||a.acode) fstr,a.acode LedgerCode,b.c_name Ledger,'-' first_grp,'-' second_grp,a.op,a.dramount,a.cramount,a.closing from " +
                    //    "(select client_unit_id, acode, sum(op) op, sum(dramount) dramount, sum(cramount) cramount,(sum(op)+sum(dramount)-sum(cramount)) closing from " +
                    //    "(select client_unit_id,acode as acode,'0' as vch_num,null as vch_date,OP_" + Ac_Year.Split(' ')[0].Trim() + " as op,0 as cramount,0 as dramount,0 as cl from acbal  " +
                    //    "union all " +
                    //    "select client_unit_id, acode as acode,'0' as vch_num,null as vch_date,sum(to_number(dramount)) - sum(to_number(cramount)) as op,0 as dramount,0 as cramount,0 as cl from vouchers " +
                    //    "where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                    //    "to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') and to_date('" + fdt + "','" + sgen.Getsqldateformat() + "')-1 " +
                    //    "group by client_unit_id,acode " +
                    //    "union all " +
                    //    "select client_unit_id,acode as acode,vch_num,vch_date,0 as op,to_number(dramount) dramount,to_number(cramount) cramount,0 as cl from vouchers " +
                    //    "where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldateformat() + "') between " +
                    //    "to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') and to_date('" + tdt + "','" + sgen.Getsqldateformat() + "')) t " +
                    //    "group by client_unit_id,acode) a " +
                    //    "inner join clients_mst b on a.acode = b.vch_num and b.type = 'BCD'";

                    //cmd = "select (a.client_unit_id||a.acode) fstr,a.acode LedgerCode,b.c_name Ledger,'-' first_grp,'-' second_grp,a.op,a.dramount,a.cramount,a.closing from " +
                    //    "(select client_unit_id, acode, sum(op) op, sum(dramount) dramount, sum(cramount) cramount, (sum(op) + sum(dramount) - sum(cramount)) closing from " +
                    //    "(select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, OP_" + Ac_Year.Split(' ')[0].Trim() + " as op, 0 as cramount, 0 as dramount," +
                    //    "0 as cl from acbal " +
                    //    "union all " +
                    //    "select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, sum(to_number(dramount)) - sum(to_number(cramount)) as op, 0 as dramount," +
                    //    "0 as cramount, 0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                    //    "to_date('" + year_from + "', '" + sgen.Getsqldateformat() + "') and to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') - 1 group by client_unit_id, acode " +
                    //    "union all " +
                    //    "select client_unit_id, acode as acode, vch_num, vch_date, 0 as op, to_number(dramount) dramount, to_number(cramount) cramount, 0 as cl from vouchers " +
                    //   "where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') " +
                    //   "and to_date('" + tdt + "', '" + sgen.Getsqldateformat() + "')) t group by client_unit_id, acode) a " +
                    //   "inner join clients_mst b on a.acode = b.vch_num and b.type = 'BCD' where a.client_unit_id = '" + unitid_mst + "'";

                    cmd = "select (a.client_unit_id||a.acode) fstr,a.acode LedgerCode,b.c_name Ledger,'-' first_grp,'-' second_grp,a.op,a.dramount,a.cramount,a.closing from " +
                        "(select client_unit_id, acode, sum(op) op, sum(dramount) dramount, sum(cramount) cramount, (sum(op) + sum(dramount) - sum(cramount)) closing from " +
                        "(select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, OP_" + Ac_Year.Split(' ')[0].Trim() + " as op, 0 as dramount, 0 as cramount," +
                        " 0 as cl from acbal " +
                        "union all " +
                        "select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, sum(to_number(dramount)) - sum(to_number(cramount)) as op, 0 as dramount," +
                        "0 as cramount, 0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                        "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') - 1 group by client_unit_id, acode " +
                        "union all " +
                        "select client_unit_id, acode as acode, vch_num, vch_date, 0 as op, to_number(dramount) dramount, to_number(cramount) cramount, 0 as cl from vouchers " +
                        "where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') " +
                        "and to_date('" + tdt + "', '" + sgen.Getsqldateformat() + "')) t group by client_unit_id, acode) a " +
                        "inner join (select a.vch_num,a.c_name from (select vch_num,c_name,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'BCD' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                        "union all select vch_num as acode ,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                        " union all select cm.vch_num as acode,nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname,cm.CLIENT_UNIT_ID,cm.vch_date,cm.type from master_setting a inner join clients_mst cm on " +
                        "a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id where cm.type = 'BAD' and cm.client_unit_id = '" + unitid_mst + "') a) " +
                        "b on b.vch_num=a.acode where a.client_unit_id = '" + ut + "'";
                    title = "Trial Balance " + date_period + "";
                    btnval = "TBDETAIL";
                    seektype = 1;
                    qucond = "TB";
                    Multiton.SetSession(MyGuid, "LEVEL", 3);
                    Multiton.SetSession(MyGuid, "SHOWTOT", "Y");
                    Multiton.SetSession(MyGuid, "MAXLEVEL", 4);

                    sgen.SetSession(MyGuid, "btnval", btnval);
                    sgen.SetSession(MyGuid, "qucond", qucond);
                    sgen.SetSession(MyGuid, "footitle", "TRIAL BALANCE");
                    break;
                #endregion
                #region Cash/Fund Flow
                case "Cash/Fund Flow":
                    cmd = "select (a.client_unit_id||a.acode) fstr,a.acode LedgerCode,b.c_name Ledger,'-' first_grp,'-' second_grp,a.op,a.dramount,a.cramount,a.closing from " +
                        "(select client_unit_id, acode, sum(op) op, sum(dramount) dramount, sum(cramount) cramount, (sum(op) + sum(dramount) - sum(cramount)) closing from " +
                        "(select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, OP_" + Ac_Year.Split(' ')[0].Trim() + " as op, 0 as cramount, 0 as dramount," +
                        " 0 as cl from acbal " +
                        "union all " +
                        "select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, sum(to_number(dramount)) - sum(to_number(cramount)) as op, 0 as dramount," +
                        "0 as cramount, 0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                        "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') - 1 group by client_unit_id, acode " +
                        "union all " +
                        "select client_unit_id, acode as acode, vch_num, vch_date, 0 as op, to_number(dramount) dramount, to_number(cramount) cramount, 0 as cl from vouchers " +
                        "where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') " +
                        "and to_date('" + tdt + "', '" + sgen.Getsqldateformat() + "')) t group by client_unit_id, acode) a " +
                        "inner join (select a.vch_num,a.c_name from ( select vch_num ,c_name,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 union all " +
                        "select cm.vch_num as acode,nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname,cm.CLIENT_UNIT_ID,cm.vch_date,cm.type from master_setting a inner join clients_mst cm on " +
                        "a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id where cm.type = 'BAD' and cm.client_unit_id = '" + unitid_mst + "') a) " +
                        "b on b.vch_num=a.acode where a.client_unit_id = '" + ut + "'";
                    title = "Cash/Fund Flow " + date_period + "";
                    btnval = "CFDETAIL";
                    seektype = 1;
                    qucond = "CF";
                    Multiton.SetSession(MyGuid, "LEVEL", 3);
                    Multiton.SetSession(MyGuid, "SHOWTOT", "Y");
                    sgen.SetSession(MyGuid, "btnval", btnval);
                    sgen.SetSession(MyGuid, "qucond", qucond);
                    sgen.SetSession(MyGuid, "footitle", "Cash/Fund Flow");

                    Multiton.SetSession(MyGuid, "MAXLEVEL", 4);

                    break;
                #endregion
                #region register
                case "Sales Register":
                    cmd = "select t.icode itemcode,i.iname itemname,t.acode partycode,c.c_name partyname,c.c_gstin gstin,c.state statecode,nvl(s.state_name,'-') statename," +
                        "t.hsno hsn, t.basicamt,t.qtyout,t.netamt,t.taxrate,t.taxamt,t.type from itransaction t " +
                        "inner join item i on i.icode = t.icode and i.type = 'IT' " +
                        "inner join clients_mst c on c.vch_num = t.acode and c.type = 'BCD' " +
                        "left join(select distinct state_gst_code, state_name from country_state) s on s.state_gst_code = c.state " +
                        "where substr(t.type,1,1)= '4' and to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                        "to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') and " +
                        "to_date('" + tdt + "', '" + sgen.Getsqldateformat() + "') order by t.acode asc";
                    title = "Sales Register " + date_period + "";
                    //btnval = "SRDETAIL";
                    seektype = 0;
                    //sgen.SetSession(MyGuid, "btnval", btnval);
                    sgen.SetSession(MyGuid, "footitle", "SALES REGISTER");

                    break;
                case "Purchase Register":
                    //cmd = "select (v.client_unit_id || v.vch_num || to_char(v.vch_date, 'yyyymmdd') || v.type) fstr,v.acode LedgerCode,c.c_name LedgerName,v.rvscode ReverseCode,v.vch_num VoucherNo," +
                    //    "v.vch_date VoucherDate, v.type,to_number(dramount) DrAmount,to_number(cramount) CrAmount from vouchers v " +
                    //    "inner join clients_mst c on c.vch_num = v.acode where to_date(to_char(v.vch_date,'DDMMYYYY'),'DDMMYYYY') between to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') and to_date('" + tdt + "', '" + sgen.Getsqldateformat() + "') and v.type = '50'";
                    cmd = "select (v.client_unit_id || v.vch_num || to_char(v.vch_date, 'yyyymmdd') || v.type) fstr,v.acode LedgerCode,c.c_name LedgerName,v.rvscode ReverseCode,v.vch_num VoucherNo," +
                        "v.vch_date VoucherDate, v.type,to_number(dramount) DrAmount,to_number(cramount) CrAmount from vouchers v " +
                        "inner join (select a.vch_num,a.c_name from (select vch_num,c_name,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'BCD' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                "union all select vch_num as acode ,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                " union all " +
                                "select cm.vch_num as acode,nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname,cm.CLIENT_UNIT_ID,cm.vch_date,cm.type from master_setting a inner join clients_mst cm on " +
                                "a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id where cm.type = 'BAD' and cm.client_unit_id = '" + unitid_mst + "') a) c on c.vch_num=v.acode where to_date(to_char(v.vch_date,'DDMMYYYY'),'DDMMYYYY') between to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') and to_date('" + tdt + "', '" + sgen.Getsqldateformat() + "') and v.type = '50'";
                    title = "Purchase Register " + date_period + "";
                    btnval = "PRDETAIL";
                    seektype = 2;
                    sgen.SetSession(MyGuid, "btnval", btnval);
                    sgen.SetSession(MyGuid, "footitle", "PURCHASE REGISTER");
                    break;
                case "Journal Register":
                    //cmd = "select (v.client_unit_id || v.vch_num || to_char(v.vch_date, 'yyyymmdd') || v.type) fstr,v.acode LedgerCode,c.c_name LedgerName,v.rvscode ReverseCode,v.vch_num VoucherNo," +
                    //    "v.vch_date VoucherDate, v.type,to_number(dramount) DrAmount,to_number(cramount) CrAmount from vouchers v " +
                    //    "inner join clients_mst c on c.vch_num = v.acode " +
                    //    "where to_date(to_char(v.vch_date,'DDMMYYYY'),'DDMMYYYY') between to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') and to_date('" + tdt + "', '" + sgen.Getsqldateformat() + "') and substr(v.type,1,1)='3'";
                    cmd = "select (v.client_unit_id || v.vch_num || to_char(v.vch_date, 'yyyymmdd') || v.type) fstr,v.acode LedgerCode,c.c_name LedgerName,v.rvscode ReverseCode,v.vch_num VoucherNo," +
                       "v.vch_date VoucherDate, v.type,to_number(dramount) DrAmount,to_number(cramount) CrAmount from vouchers v " +
                       "inner join (select a.vch_num,a.c_name from (select vch_num,c_name,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'BCD' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                               "union all select vch_num as acode ,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                               " union all " +
                               "select cm.vch_num as acode,nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname,cm.CLIENT_UNIT_ID,cm.vch_date,cm.type from master_setting a inner join clients_mst cm on " +
                               "a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id where cm.type = 'BAD' and cm.client_unit_id = '" + unitid_mst + "') a) c on c.vch_num=v.acode " +
                       "where to_date(to_char(v.vch_date,'DDMMYYYY'),'DDMMYYYY') between to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') and to_date('" + tdt + "', '" + sgen.Getsqldateformat() + "') and substr(v.type,1,1)='3'";
                    title = "Journal Register " + date_period + "";
                    btnval = "JRDETAIL";
                    seektype = 2;
                    sgen.SetSession(MyGuid, "btnval", btnval);
                    sgen.SetSession(MyGuid, "footitle", "JOURNAL REGISTER");
                    break;
                case "Payment Register":
                    cmd = "select (v.client_unit_id || v.vch_num || to_char(v.vch_date, 'yyyymmdd') || v.type) fstr,v.acode LedgerCode,c.c_name LedgerName,v.rvscode ReverseCode,v.vch_num VoucherNo," +
                        "v.vch_date VoucherDate, v.type,to_number(dramount) DrAmount,to_number(cramount) CrAmount from vouchers v " +
                        "inner join (select a.vch_num,a.c_name from (select vch_num,c_name,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'BCD' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                "union all select vch_num as acode ,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                " union all " +
                                "select cm.vch_num as acode,nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname,cm.CLIENT_UNIT_ID,cm.vch_date,cm.type from master_setting a inner join clients_mst cm on " +
                                "a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id where cm.type = 'BAD' and cm.client_unit_id = '" + unitid_mst + "') a) c on c.vch_num=v.acode " +
                        "where to_date(to_char(v.vch_date,'DDMMYYYY'),'DDMMYYYY') between to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') and to_date('" + tdt + "', '" + sgen.Getsqldateformat() + "') and substr(v.type,1,1)='2' and (to_number(v.cramount) + to_number(v.dramount)) > 0";
                    title = "Payment Register " + date_period + "";
                    btnval = "PAYDETAIL";
                    seektype = 2;
                    sgen.SetSession(MyGuid, "btnval", btnval);
                    sgen.SetSession(MyGuid, "footitle", "PAYMENT REGISTER");

                    break;
                case "Receipt Register":
                    //cmd = "select (v.client_unit_id || v.vch_num || to_char(v.vch_date, 'yyyymmdd') || v.type) fstr,v.acode LedgerCode,c.c_name LedgerName,v.rvscode ReverseCode,v.vch_num VoucherNo," +
                    //    "v.vch_date VoucherDate, v.type,to_number(dramount) DrAmount,to_number(cramount) CrAmount from vouchers v " +
                    //    "inner join clients_mst c on c.vch_num = v.acode where to_date(to_char(v.vch_date,'DDMMYYYY'),'DDMMYYYY') between to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') and to_date('" + tdt + "', '" + sgen.Getsqldateformat() + "') and substr(v.type,1,1)='1' and v.type<>'10' and (to_number(v.cramount) + to_number(v.dramount)) > 0";
                    //title = "Receipt Register " + date_period + "";
                    cmd = "select (v.client_unit_id || v.vch_num || to_char(v.vch_date, 'yyyymmdd') || v.type) fstr,v.acode LedgerCode,c.c_name LedgerName,v.rvscode ReverseCode,v.vch_num VoucherNo," +
                        "v.vch_date VoucherDate, v.type,to_number(dramount) DrAmount,to_number(cramount) CrAmount from vouchers v " +
                        "inner join (select a.vch_num,a.c_name from (select vch_num,c_name,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'BCD' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                "union all select vch_num as acode ,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                " union all " +
                                "select cm.vch_num as acode,nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname,cm.CLIENT_UNIT_ID,cm.vch_date,cm.type from master_setting a inner join clients_mst cm on " +
                                "a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id where cm.type = 'BAD' and cm.client_unit_id = '" + unitid_mst + "') a) c on c.vch_num=v.acode where to_date(to_char(v.vch_date,'DDMMYYYY'),'DDMMYYYY') between to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') and to_date('" + tdt + "', '" + sgen.Getsqldateformat() + "') and substr(v.type,1,1)='1' and (to_number(v.cramount) + to_number(v.dramount)) > 0";
                    title = "Receipt Register " + date_period + "";
                    //cmd = "select (v.client_unit_id||v.acode||v.vch_num||v.type) fstr,v.acode LedgerCode,c.c_name LedgerName,v.rvscode ReverseCode,v.vch_num VoucherNo," +
                    //    "v.vch_date VoucherDate, v.type,to_number(dramount) DrAmount,to_number(cramount) CrAmount from vouchers v " +
                    //    "inner join clients_mst c on c.vch_num = v.acode where v.vch_date between to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') and to_date('" + tdt + "', '" + sgen.Getsqldateformat() + "') and substr(v.type,1,1)='1' and v.type<>'10' and (to_number(v.cramount) + to_number(v.dramount)) > 0";
                    //title = "Receipt Register " + date_period + "";
                    btnval = "RRDETAIL";
                    seektype = 2;
                    sgen.SetSession(MyGuid, "btnval", btnval);
                    sgen.SetSession(MyGuid, "footitle", "RECIEPT REGISTER");

                    break;
                case "Cash Register":
                    //cmd = "select (v.client_unit_id || v.vch_num || to_char(v.vch_date, 'yyyymmdd') || v.type) fstr,v.acode LedgerCode,c.c_name LedgerName,v.rvscode ReverseCode,v.vch_num VoucherNo," +
                    //    "v.vch_date VoucherDate, v.type,to_number(dramount) DrAmount,to_number(cramount) CrAmount from vouchers v " +
                    //    "inner join clients_mst c on c.vch_num = v.acode where to_date(to_char(v.vch_date,'DDMMYYYY'),'DDMMYYYY') between to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') and to_date('" + tdt + "', '" + sgen.Getsqldateformat() + "') and v.type='10' and (to_number(v.cramount) + to_number(v.dramount)) > 0";
                    cmd = "select (v.client_unit_id || v.vch_num || to_char(v.vch_date, 'yyyymmdd') || v.type) fstr,v.acode LedgerCode,c.c_name LedgerName,v.rvscode ReverseCode,v.vch_num VoucherNo," +
                        "v.vch_date VoucherDate, v.type,to_number(dramount) DrAmount,to_number(cramount) CrAmount from vouchers v " +
                        "inner join (select a.vch_num,a.c_name from (select vch_num,c_name,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'BCD' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                "union all select vch_num as acode ,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                " union all " +
                                "select cm.vch_num as acode,nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname,cm.CLIENT_UNIT_ID,cm.vch_date,cm.type from master_setting a inner join clients_mst cm on " +
                                "a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id where cm.type = 'BAD' and cm.client_unit_id = '" + unitid_mst + "') a) c on c.vch_num=v.acode where to_date(to_char(v.vch_date,'DDMMYYYY'),'DDMMYYYY') between to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') and to_date('" + tdt + "', '" + sgen.Getsqldateformat() + "') and v.type='10' and (to_number(v.cramount) + to_number(v.dramount)) > 0";
                    title = "Cash Register " + date_period + "";
                    btnval = "CRDETAIL";
                    seektype = 2;
                    sgen.SetSession(MyGuid, "btnval", btnval);
                    sgen.SetSession(MyGuid, "footitle", "CASH REGISTER");

                    break;
                #endregion
                #region book
                case "Day Book":
                    //cmd = "select (v.client_unit_id||v.acode||v.vch_num||v.type) fstr,v.acode LedgerCode,c.c_name LedgerName,v.rvscode ReverseCode,v.vch_num VoucherNo," +
                    //    "v.vch_date VoucherDate, v.type,to_number(dramount) DrAmount,to_number(cramount) CrAmount from vouchers v " +
                    //    "inner join clients_mst c on c.vch_num = v.acode and c.type = 'BCD' " +
                    //    "where v.vch_date between to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') and to_date('" + tdt + "', 'dd/mm/yyyy') and v.rno = '1'";

                    title = "Day Book " + date_period + "";
                    btnval = "DBOOK";
                    seektype = 1;
                    qucond = "DB";
                    Multiton.SetSession(MyGuid, "LEVEL", 3);
                    Multiton.SetSession(MyGuid, "SHOWTOT", "Y");
                    sgen.SetSession(MyGuid, "btnval", btnval);
                    sgen.SetSession(MyGuid, "qucond", qucond);
                    sgen.SetSession(MyGuid, "footitle", "DAY BOOK");
                    cmd = myquery(3, "", fdt + "!~!~!~!~!" + tdt, qucond);
                    Multiton.SetSession(MyGuid, "LEVEL", (3 - 1).ToString());
                    Multiton.SetSession(MyGuid, "MAXLEVEL", 3);
                    break;
                case "Bank Book":
                    //cmd = "select (t.client_unit_id||t.vch_num||t.type) fstr,t.vch_num,nvl(to_char(t.vch_date,'dd/MM/YYYY'),'01/01/1900') vch_date,t.acode LedgerCode," +
                    //    "c.c_name Ledger, t.type Type, t.dramount DrAmount, t.cramount CrAmount from " +
                    //    "(select client_unit_id, vch_num as acode, '0' as vch_num, null as vch_date, to_number(opbal) as op, 'OP' type, 0 as dramount, 0 as cramount " +
                    //    "from tally_clients_mst where type = 'BCD' " +
                    //    "union all " +
                    //    "select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, sum(to_number(dramount)) - sum(to_number(cramount)) as op, type, " +
                    //    "0 as dramount, 0 as cramount from tally_vouchers where vch_date < to_date('" + fdt + "', 'dd/MM/YYYY') - 1 group by client_unit_id, acode, type " +
                    //    "union all " +
                    //    "select client_unit_id, acode as acode, vch_num, vch_date, 0 as op, type, to_number(dramount), to_number(cramount) from tally_vouchers " +
                    //    "where vch_date between to_date('" + fdt + "', 'dd/MM/YYYY') and to_date('" + tdt + "', 'dd/MM/YYYY')) t " +
                    //    "inner join tally_clients_mst c on c.vch_num = t.acode and c.type = 'BCD' " +
                    //    "where substr(t.type,'1','1') in ('1', '2') and t.type not in ('10', '20')";

                    title = "Bank Book " + date_period + "";
                    btnval = "BBOOK";
                    seektype = 1;
                    qucond = "where substr(t.type,'1','1') in ('1', '2') and t.type not in ('10', '20')";
                    Multiton.SetSession(MyGuid, "LEVEL", 3);
                    Multiton.SetSession(MyGuid, "SHOWTOT", "Y");
                    sgen.SetSession(MyGuid, "btnval", btnval);
                    sgen.SetSession(MyGuid, "qucond", qucond);
                    sgen.SetSession(MyGuid, "footitle", "BANK BOOK");

                    cmd = myquery(3, "", fdt + "!~!~!~!~!" + tdt, qucond);
                    Multiton.SetSession(MyGuid, "LEVEL", (3 - 1).ToString());
                    Multiton.SetSession(MyGuid, "MAXLEVEL", 3);
                    break;
                case "Cash Book":
                    //cmd = "select (t.client_unit_id||t.vch_num||t.type) fstr,t.vch_num,nvl(to_char(t.vch_date,'dd/MM/YYYY'),'01/01/1900') vch_date,t.acode LedgerCode," +
                    //    "c.c_name Ledger, t.type Type, t.dramount DrAmount, t.cramount CrAmount from " +
                    //    "(select client_unit_id, vch_num as acode, '0' as vch_num, null as vch_date, to_number(opbal) as op, 'OP' type, 0 as dramount, 0 as cramount " +
                    //    "from tally_clients_mst where type = 'BCD' " +
                    //    "union all " +
                    //    "select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, sum(to_number(dramount)) - sum(to_number(cramount)) as op, type, " +
                    //    "0 as dramount, 0 as cramount from tally_vouchers where vch_date < to_date('" + fdt + "', 'dd/MM/YYYY') - 1 group by client_unit_id, acode, type " +
                    //    "union all " +
                    //    "select client_unit_id, acode as acode, vch_num, vch_date, 0 as op, type, to_number(dramount), to_number(cramount) from tally_vouchers " +
                    //    "where vch_date between to_date('" + fdt + "', 'dd/MM/YYYY') and to_date('" + tdt + "', 'dd/MM/YYYY')) t " +
                    //    "inner join tally_clients_mst c on c.vch_num = t.acode and c.type = 'BCD' " +
                    //    "where t.type in ('10', '20')";

                    title = "Cash Book " + date_period + "";
                    btnval = "CBOOK";
                    seektype = 1;
                    qucond = "where t.type in ('10', '20')";
                    Multiton.SetSession(MyGuid, "LEVEL", 3);
                    Multiton.SetSession(MyGuid, "SHOWTOT", "Y");
                    sgen.SetSession(MyGuid, "btnval", btnval);
                    sgen.SetSession(MyGuid, "qucond", qucond);
                    sgen.SetSession(MyGuid, "footitle", "CASH BOOK");

                    cmd = myquery(3, "", fdt + "!~!~!~!~!" + tdt, qucond);
                    Multiton.SetSession(MyGuid, "LEVEL", (3 - 1).ToString());
                    Multiton.SetSession(MyGuid, "MAXLEVEL", 3);
                    break;
                case "Sales Book":
                    //cmd = "select (t.client_unit_id||t.vch_num||t.type) fstr,t.vch_num,nvl(to_char(t.vch_date,'dd/MM/YYYY'),'01/01/1900') vch_date,t.acode LedgerCode," +
                    //    "c.c_name Ledger, t.type Type, t.dramount DrAmount, t.cramount CrAmount from " +
                    //    "(select client_unit_id, vch_num as acode, '0' as vch_num, null as vch_date, to_number(opbal) as op, 'OP' type, 0 as dramount, 0 as cramount " +
                    //    "from tally_clients_mst where type = 'BCD' " +
                    //    "union all " +
                    //    "select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, sum(to_number(dramount)) - sum(to_number(cramount)) as op, type, " +
                    //    "0 as dramount, 0 as cramount from tally_vouchers where vch_date < to_date('" + fdt + "', 'dd/MM/YYYY') - 1 group by client_unit_id, acode, type " +
                    //    "union all " +
                    //    "select client_unit_id, acode as acode, vch_num, vch_date, 0 as op, type, to_number(dramount), to_number(cramount) from tally_vouchers " +
                    //    "where vch_date between to_date('" + fdt + "', 'dd/MM/YYYY') and to_date('" + tdt + "', 'dd/MM/YYYY')) t " +
                    //    "inner join tally_clients_mst c on c.vch_num = t.acode and c.type = 'BCD' " +
                    //    "where substr(t.type,'1','1') in ('4')";
                    title = "Sales Book " + date_period + "";
                    btnval = "SBOOK";
                    seektype = 1;
                    qucond = "where substr(t.type,'1','1') in ('4')";
                    Multiton.SetSession(MyGuid, "LEVEL", 3);
                    Multiton.SetSession(MyGuid, "SHOWTOT", "Y");
                    sgen.SetSession(MyGuid, "btnval", btnval);
                    sgen.SetSession(MyGuid, "footitle", "SALES BOOK");
                    sgen.SetSession(MyGuid, "qucond", qucond);
                    cmd = myquery(3, "", fdt + "!~!~!~!~!" + tdt, qucond);
                    Multiton.SetSession(MyGuid, "LEVEL", (3 - 1).ToString());
                    Multiton.SetSession(MyGuid, "MAXLEVEL", 3);
                    break;
                case "Purchase Book":
                    //cmd = "select (t.client_unit_id||t.vch_num||t.type) fstr,t.vch_num,nvl(to_char(t.vch_date,'dd/MM/YYYY'),'01/01/1900') vch_date,t.acode LedgerCode," +
                    //    "c.c_name Ledger, t.type Type, t.dramount DrAmount, t.cramount CrAmount from " +
                    //    "(select client_unit_id, vch_num as acode, '0' as vch_num, null as vch_date, to_number(opbal) as op, 'OP' type, 0 as dramount, 0 as cramount " +
                    //    "from tally_clients_mst where type = 'BCD' " +
                    //    "union all " +
                    //    "select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, sum(to_number(dramount)) - sum(to_number(cramount)) as op, type, " +
                    //    "0 as dramount, 0 as cramount from tally_vouchers where vch_date < to_date('" + fdt + "', 'dd/MM/YYYY') - 1 group by client_unit_id, acode, type " +
                    //    "union all " +
                    //    "select client_unit_id, acode as acode, vch_num, vch_date, 0 as op, type, to_number(dramount), to_number(cramount) from tally_vouchers " +
                    //    "where vch_date between to_date('" + fdt + "', 'dd/MM/YYYY') and to_date('" + tdt + "', 'dd/MM/YYYY')) t " +
                    //    "inner join tally_clients_mst c on c.vch_num = t.acode and c.type = 'BCD' " +
                    //    "where substr(t.type,'1','1') in ('5')";

                    title = "Purchase Book " + date_period + "";
                    btnval = "PBOOK";
                    seektype = 1;
                    qucond = "where substr(t.type,'1','1') in ('5')";
                    Multiton.SetSession(MyGuid, "LEVEL", 3);
                    Multiton.SetSession(MyGuid, "SHOWTOT", "Y");
                    sgen.SetSession(MyGuid, "btnval", btnval);
                    sgen.SetSession(MyGuid, "qucond", qucond);
                    sgen.SetSession(MyGuid, "footitle", "PURCHASE BOOK");
                    cmd = myquery(3, "", fdt + "!~!~!~!~!" + tdt, qucond);
                    Multiton.SetSession(MyGuid, "LEVEL", (3 - 1).ToString());
                    Multiton.SetSession(MyGuid, "MAXLEVEL", 3);
                    break;
                case "Journal Book":
                    //cmd = "select (t.client_unit_id||t.vch_num||t.type) fstr,t.vch_num,nvl(to_char(t.vch_date,'dd/MM/YYYY'),'01/01/1900') vch_date,t.acode LedgerCode," +
                    //    "c.c_name Ledger, t.type Type, t.dramount DrAmount, t.cramount CrAmount from " +
                    //    "(select client_unit_id, vch_num as acode, '0' as vch_num, null as vch_date, to_number(opbal) as op, 'OP' type, 0 as dramount, 0 as cramount " +
                    //    "from tally_clients_mst where type = 'BCD' " +
                    //    "union all " +
                    //    "select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, sum(to_number(dramount)) - sum(to_number(cramount)) as op, type, " +
                    //    "0 as dramount, 0 as cramount from tally_vouchers where vch_date < to_date('" + fdt + "', 'dd/MM/YYYY') - 1 group by client_unit_id, acode, type " +
                    //    "union all " +
                    //    "select client_unit_id, acode as acode, vch_num, vch_date, 0 as op, type, to_number(dramount), to_number(cramount) from tally_vouchers " +
                    //    "where vch_date between to_date('" + fdt + "', 'dd/MM/YYYY') and to_date('" + tdt + "', 'dd/MM/YYYY')) t " +
                    //    "inner join tally_clients_mst c on c.vch_num = t.acode and c.type = 'BCD' " +
                    //    "where substr(t.type,'1','1') in ('3')";
                    title = "Journal Book " + date_period + "";
                    btnval = "JBOOK";
                    seektype = 1;
                    qucond = "where substr(t.type,'1','1') in ('3')";
                    Multiton.SetSession(MyGuid, "LEVEL", 3);
                    Multiton.SetSession(MyGuid, "SHOWTOT", "Y");
                    sgen.SetSession(MyGuid, "btnval", btnval);
                    sgen.SetSession(MyGuid, "qucond", qucond);
                    sgen.SetSession(MyGuid, "footitle", "JOURNAL BOOK");
                    cmd = myquery(3, "", fdt + "!~!~!~!~!" + tdt, qucond);
                    Multiton.SetSession(MyGuid, "LEVEL", (3 - 1).ToString());
                    Multiton.SetSession(MyGuid, "MAXLEVEL", 3);
                    break;
                #endregion
                #region callback
                case "Callback":
                    if (sgen.GetSession(MyGuid, "btnval") != null && sgen.GetSession(MyGuid, "btnval").ToString() == "PARTYLEDGDET")
                    {
                        btnval = sgen.GetSession(MyGuid, "btnval").ToString();
                        model = CallbackFun(btnval, "N", actionName, controllerName, model);
                    }
                    else
                    {
                        try
                        {
                            string cond = sgen.GetSession(MyGuid, "qucond").ToString();
                            int level = sgen.Make_int(Multiton.GetSession(MyGuid, "LEVEL").ToString());
                            string param1 = sgen.GetSession(MyGuid, "SSEEKVAL").ToString();
                            string param2 = fdt + "!~!~!~!~!" + tdt;
                            if (level == 1)
                            {
                                if (level == 1) sgen.SetSession(MyGuid, "l1fstr", param1);
                                String vouurl = param1;
                                return RedirectToAction("cas_recn", new { @m_id = EncryptDecrypt.Encrypt(model[0].Col15), @mid = EncryptDecrypt.Encrypt(model[0].Col14), @vid = EncryptDecrypt.Encrypt(vouurl) });
                            }
                            else if (level == 0) { }
                            else
                            {
                                seektype = 1;
                                sgen.SetSession(MyGuid, "btnval", btnval);

                                cmd = myquery(level, param1, param2, cond);
                                sgen.open_grid(title, cmd, seektype);

                                if (level == 2) sgen.SetSession(MyGuid, "l2fstr", param1);
                                else if (level == 3) sgen.SetSession(MyGuid, "l3fstr", param1);
                                else if (level == 4) sgen.SetSession(MyGuid, "l4fstr", param1);
                                else if (level == 5) sgen.SetSession(MyGuid, "l5fstr", param1);
                                if (sgen.GetSession(MyGuid, "footitle") != null)
                                {
                                    string btn = sgen.GetSession(MyGuid, "footitle").ToString();
                                    switch (btn.Trim())
                                    {
                                        case "DAY BOOK":
                                            title = "DETAILED DAY BOOK";
                                            break;
                                        case "JOURNAL BOOK":
                                            title = "DETAILED JOURNAL BOOK";
                                            break;
                                        case "CASH BOOK":
                                            title = "DETAILED CASH BOOK";
                                            break;
                                        case "SALES BOOK":
                                            title = "DETAILED SALES BOOK";
                                            break;
                                        case "PURCHASE BOOK":
                                            title = "DETAILED PURCHASE BOOK";
                                            break;
                                        case "BANK BOOK":
                                            title = "DETAILED BANK BOOK";
                                            break;
                                        case "TRIAL BALANCE":
                                            title = "DETAILED TRIAL BALANCE";
                                            break;
                                        case "SALES REGISTER":
                                            title = "DETAILED SALES REGISTER";
                                            break;
                                        case "PURCHASE REGISTER":
                                            title = "DETAILED PURCHASE REGISTER";
                                            break;
                                        case "JOURNAL REGISTER":
                                            title = "DETAILED JOURNAL REGISTER";
                                            break;
                                        case "PAYMENT REGISTER":
                                            title = "DETAILED PAYMENT REGISTER";
                                            break;
                                        case "RECIEPT REGISTER":
                                            title = "DETAILED RECIEPT REGISTER";
                                            break;
                                        case "CASH REGISTER":
                                            title = "DETAILED CASH REGISTER";
                                            break;
                                        case "Cash/Fund Flow":
                                            title = "Detailed Cash/Fund Flow";
                                            break;
                                        default:
                                            title = "Detailed Report";
                                            break;
                                    }
                                }
                                //ViewBag.scripCall = "callFoo('" + title + "');";
                                ViewBag.scripCall = "callFoo3('" + title + "');";
                            }
                            Multiton.SetSession(MyGuid, "LEVEL", (level - 1).ToString());
                        }
                        catch (Exception ex) { }
                        if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
                        switch (btnval.ToUpper())
                        {
                            case "DBVOU":
                            case "SRVOU":
                            case "PRVOU":
                            case "JRVOU":
                            case "PAYVOU":
                            case "RRVOU":
                            case "CRVOU":
                            case "BVOU":
                            case "CVOU":
                            case "SVOU":
                            case "PVOU":
                            case "JVOU":
                                String vouurl = sgen.GetSession(MyGuid, "SSEEKVAL").ToString();
                                return RedirectToAction("cas_recn", new { @m_id = EncryptDecrypt.Encrypt(model[0].Col15), @mid = EncryptDecrypt.Encrypt(model[0].Col14), @vid = EncryptDecrypt.Encrypt(vouurl) });
                                break;
                            default:
                                model = CallbackFun(btnval, "N", actionName, controllerName, model);
                                break;

                        }
                    }
                    break;
                #endregion
                #region callback2
                case "Callback2":
                    try
                    {
                        string cond = sgen.GetSession(MyGuid, "qucond").ToString();
                        int level = sgen.Make_int(Multiton.GetSession(MyGuid, "LEVEL").ToString()) + 2;
                        string param1 = sgen.GetSession(MyGuid, "SSEEKVAL").ToString();
                        string param2 = fdt + "!~!~!~!~!" + tdt;
                        seektype = 1;
                        sgen.SetSession(MyGuid, "btnval", btnval);
                        string pfstr = "";
                        if (level == 1) pfstr = sgen.GetSession(MyGuid, "l1fstr").ToString();
                        if (level == 2) pfstr = sgen.GetSession(MyGuid, "l2fstr").ToString();
                        if (level == 3) pfstr = sgen.GetSession(MyGuid, "l3fstr").ToString();
                        if (level == 4) pfstr = sgen.GetSession(MyGuid, "l4fstr").ToString();
                        if (level == 5) pfstr = sgen.GetSession(MyGuid, "l5fstr").ToString();
                        cmd = myquery(level, pfstr, param2, cond);
                        sgen.open_grid(title, cmd, seektype);
                        if (sgen.GetSession(MyGuid, "footitle") != null)
                        {
                            string btn = sgen.GetSession(MyGuid, "footitle").ToString();
                            switch (btn.Trim())
                            {
                                case "DAY BOOK":
                                    title = "DETAILED DAY BOOK";
                                    break;
                                case "JOURNAL BOOK":
                                    title = "DETAILED JOURNAL BOOK";
                                    break;
                                case "CASH BOOK":
                                    title = "DETAILED CASH BOOK";
                                    break;
                                case "SALES BOOK":
                                    title = "DETAILED SALES BOOK";
                                    break;
                                case "PURCHASE BOOK":
                                    title = "DETAILED PURCHASE BOOK";
                                    break;
                                case "BANK BOOK":
                                    title = "DETAILED BANK BOOK";
                                    break;
                                case "TRIAL BALANCE":
                                    title = "DETAILED TRIAL BALANCE";
                                    break;
                                case "SALES REGISTER":
                                    title = "DETAILED SALES REGISTER";
                                    break;
                                case "PURCHASE REGISTER":
                                    title = "DETAILED PURCHASE REGISTER";
                                    break;
                                case "JOURNAL REGISTER":
                                    title = "DETAILED JOURNAL REGISTER";
                                    break;
                                case "PAYMENT REGISTER":
                                    title = "DETAILED PAYMENT REGISTER";
                                    break;
                                case "RECIEPT REGISTER":
                                    title = "DETAILED RECIEPT REGISTER";
                                    break;
                                case "CASH REGISTER":
                                    title = "DETAILED CASH REGISTER";
                                    break;
                                case "Cash/Fund Flow":
                                    title = "Detailed Cash/Fund Flow";
                                    break;
                                default:
                                    title = "Detailed Report";
                                    break;
                            }
                        }
                        ViewBag.scripCall = "callFoo3('" + title + "');";
                        Multiton.SetSession(MyGuid, "LEVEL", (level - 1).ToString());
                    }
                    catch (Exception ex) { }
                    if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
                    switch (btnval.ToUpper())
                    {
                        case "DBVOU":
                        case "SRVOU":
                        case "PRVOU":
                        case "JRVOU":
                        case "PAYVOU":
                        case "RRVOU":
                        case "CRVOU":
                        case "BVOU":
                        case "CVOU":
                        case "SVOU":
                        case "PVOU":
                        case "JVOU":
                            String vouurl = sgen.GetSession(MyGuid, "SSEEKVAL").ToString();
                            return RedirectToAction("cas_recn", new { @m_id = EncryptDecrypt.Encrypt(model[0].Col15), @mid = EncryptDecrypt.Encrypt(model[0].Col14), @vid = EncryptDecrypt.Encrypt(vouurl) });
                            break;
                        default:
                            model = CallbackFun(btnval, "N", actionName, controllerName, model);
                            break;
                    }
                    break;
                    #endregion
            }
            if (command != "Callback2" && command != "Callback" && command != "PO Summary Item Wise" && command != "PO Summary Item Wise")
            {
                sgen.open_grid(title, cmd, seektype);
                ViewBag.scripCall = "callFoo('" + title + "');";
                try
                {
                    sgen.SetSession(MyGuid, "KPDCMD2", sgen.GetSession(MyGuid, "KPDCMD").ToString());
                }
                catch (Exception ex)
                {
                }
                sgen.SetSession(MyGuid, "KPDCMD", null);
            }
            ModelState.Clear();
            return View(model);
        }
        public string myquery(int level, string param1, string param2, string qucond)
        {
            string qu = "", cond = "";
            switch (level)
            {
                case 4:
                    qu = "select(a.client_unit_id || a.acode) fstr,a.acode LedgerCode, b.c_name Ledger,a.op,a.dramount,a.cramount,a.closing from " +
                        "(select client_unit_id, acode, sum(op) op, sum(dramount) dramount, sum(cramount) cramount, (sum(op) + sum(dramount) - sum(cramount)) closing from " +
                        "(select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, OP_" + Ac_Year.Split(' ')[0].Trim() + " as op, 0 as dramount, 0 as cramount," +
                        " 0 as cl from acbal " +
                        "union all " +
                        "select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, sum(to_number(dramount)) - sum(to_number(cramount)) as op, 0 as dramount," +
                        "0 as cramount, 0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                        "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and " +
                        "to_date('" + param2.Split(new string[] { "!~!~!~!~!" }, StringSplitOptions.None)[0].Trim() + "', '" + sgen.Getsqldateformat() + "') - 1 group by client_unit_id, acode " +
                        "union all " +
                        "select client_unit_id, acode as acode, vch_num, vch_date, 0 as op, to_number(dramount) dramount, to_number(cramount) cramount, 0 as cl from vouchers " +
                        "where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                        "to_date('" + param2.Split(new string[] { "!~!~!~!~!" }, StringSplitOptions.None)[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and " +
                        "to_date('" + param2.Split(new string[] { "!~!~!~!~!" }, StringSplitOptions.None)[0].Trim() + "', '" + sgen.Getsqldateformat() + "')) t group by client_unit_id, acode) a " +
                        "inner join clients_mst b on a.acode = b.vch_num and b.type = 'BCD' where a.client_unit_id = '" + unitid_mst + "' and " +
                        "substr(a.acode,1,3)='" + param1 + "'";
                    break;
                case 3://Single Row Vouchers
                       //                qu = "select (t.client_unit_id||t.vch_num||to_char(t.vch_date,'yyyyMMdd')||t.type) fstr,t.vch_num,nvl(to_char(t.vch_date,'" + sgen.Getsqldateformat() + "'),'01/01/1900') vch_date,t.acode LedgerCode," +
                       //"c.c_name Ledger,(case when substr(t.type,1,1)='1' and t.type<>'10' then 'Receipt' when substr(t.type,1,1)='1' and t.type='10' then 'Cash' when substr(t.type,1,1)='2' " +
                       //"then 'Payment' when substr(t.type,1,1)='3' then 'Journal' when substr(t.type,1,1)='4' then 'Sales' when substr(t.type,1,1)='5' then 'Purchase' else 'OP' end) Type," +
                       //"t.dramount DrAmount,t.cramount CrAmount from " +
                       //"(select client_unit_id, vch_num as acode, '0' as vch_num, null as vch_date, to_number(opbal) as op,'OP' type, 0 as dramount, 0 as cramount from tally_clients_mst " +
                       //"where type = 'BCD' " +
                       //"union all " +
                       //"select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, sum(to_number(dramount)) - sum(to_number(cramount)) as op,type,0 as dramount," +
                       //" 0 as cramount from tally_vouchers where vch_date < to_date('" + param2.Split(new string[] { "!~!~!~!~!" }, StringSplitOptions.None)[0].Trim() + "','" + sgen.Getsqldateformat() + "') - 1 " +
                       //"group by client_unit_id, acode,type " +
                       //"union all " +
                       //"select client_unit_id, acode as acode, vch_num, vch_date, 0 as op,type, to_number(dramount), to_number(cramount) from tally_vouchers " +
                       //"where vch_date between to_date('" + param2.Split(new string[] { "!~!~!~!~!" }, StringSplitOptions.None)[0].Trim() + "','" + sgen.Getsqldateformat() + "') and " +
                       //"to_date('" + param2.Split(new string[] { "!~!~!~!~!" }, StringSplitOptions.None)[1].Trim() + "','" + sgen.Getsqldateformat() + "')) t " +
                       //"inner join tally_clients_mst c on c.vch_num = t.acode and c.type = 'BCD' where (t.client_unit_id || t.acode) = '" + param1 + "'";

                    //qu = "select (t.client_unit_id||t.vch_num||to_char(t.vch_date,'yyyyMMdd')||t.type) fstr,t.vch_num,nvl(to_char(t.vch_date,'" + sgen.Getsqldateformat() + "'),'01/01/1900') vch_date," +
                    //    "t.acode LedgerCode, c.c_name Ledger,(case when substr(t.type, 1, 1) = '1' and t.type <> '10' then 'Receipt' when substr(t.type,1,1)= '1' and t.type = '10' " +
                    //    "then 'Cash' when substr(t.type,1,1)= '2' then 'Payment' when substr(t.type,1,1)= '3' then 'Journal' when substr(t.type,1,1)= '4' then 'Sales' " +
                    //    "when substr(t.type,1,1)= '5' then 'Purchase' else 'OP' end) Type,t.dramount DrAmount, t.cramount CrAmount from " +
                    //    "(select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, OP_" + Ac_Year.Split(' ')[0].Trim() + " as op,'OP' type," +
                    //    "0 as cramount, 0 as dramount, 0 as cl from acbal " +
                    //    "union all " +
                    //    "select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, sum(to_number(dramount)) - sum(to_number(cramount)) as op, " +
                    //    "type,0 as dramount, 0 as cramount, 0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') " +
                    //    "between to_date('" + param2.Split(new string[] { "!~!~!~!~!" }, StringSplitOptions.None)[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and " +
                    //    "to_date('" + param2.Split(new string[] { "!~!~!~!~!" }, StringSplitOptions.None)[0].Trim() + "', '" + sgen.Getsqldateformat() + "') - 1 " +
                    //    "group by client_unit_id, acode, type " +
                    //    "union all " +
                    //    "select client_unit_id, acode as acode, vch_num, vch_date, 0 as op, type, to_number(dramount) dramount, to_number(cramount) cramount, 0 as cl " +
                    //    "from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                    //    "to_date('" + param2.Split(new string[] { "!~!~!~!~!" }, StringSplitOptions.None)[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and " +
                    //    "to_date('" + param2.Split(new string[] { "!~!~!~!~!" }, StringSplitOptions.None)[1].Trim() + "', '" + sgen.Getsqldateformat() + "')) t " +
                    //    "inner join clients_mst c on c.vch_num = t.acode and c.type = 'BCD' where(t.client_unit_id || t.acode) = '" + param1.Trim() + "'";

                    if (qucond.Trim().Equals("TB")) { cond = "where (t.client_unit_id || t.acode) = '" + param1.Trim() + "'"; }//trial bal
                    else if (qucond.Trim().Equals("CF")) { cond = "where (t.client_unit_id || t.acode) = '" + param1.Trim() + "'"; }//trial bal
                    else if (qucond.Trim().Equals("DB")) { cond = "where t.client_unit_id='" + unitid_mst + "' and t.type<>'OP'"; }//daybook
                    else { cond = qucond; }

                    qu = "select * from (select (t.client_unit_id||t.vch_num||to_char(t.vch_date,'yyyyMMdd')||t.type) fstr,t.vch_num," +
                        "nvl(to_char(t.vch_date,'" + sgen.Getsqldateformat() + "'),'01/01/1900') vch_date,t.acode LedgerCode, c.c_name Ledger,t.type," +
                        "t.dramount DrAmount,t.cramount CrAmount from " +
                        "(select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, OP_" + Ac_Year.Split(' ')[0].Trim() + " as op, 'OP' type," +
                        "0 as dramount, 0 as cramount, 0 as cl from acbal " +
                        "union all " +
                        "select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, sum(to_number(dramount)) - sum(to_number(cramount)) as op," +
                        "type,0 as dramount, 0 as cramount, 0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') " +
                        "between to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + param2.Split(new string[] { "!~!~!~!~!" }, StringSplitOptions.None)[0].Trim() + "', '" + sgen.Getsqldateformat() + "') - 1 " +
                        "group by client_unit_id, acode, type " +
                        "union all " +
                        "select client_unit_id, acode as acode, vch_num, vch_date, 0 as op, type, to_number(dramount) dramount, to_number(cramount) cramount, 0 as cl " +
                        "from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                        "to_date('" + param2.Split(new string[] { "!~!~!~!~!" }, StringSplitOptions.None)[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and " +
                        "to_date('" + param2.Split(new string[] { "!~!~!~!~!" }, StringSplitOptions.None)[1].Trim() + "', '" + sgen.Getsqldateformat() + "')) t " +
                        "inner join (select a.vch_num,a.c_name from (select vch_num,c_name,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'BCD' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                        "union all select vch_num as acode ,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                        " union all select cm.vch_num as acode,nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname,cm.CLIENT_UNIT_ID,cm.vch_date,cm.type from master_setting a inner join clients_mst cm on " +
                        "a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id where cm.type = 'BAD' and cm.client_unit_id = '" + unitid_mst + "') a) " +
                        "c on c.vch_num=t.acode and (t.dramount+t.cramount)>0" + cond + ") a order by vch_num,vch_date, type asc";
                    break;
                case 2://MultiRow Vouchers
                    //cmd = "select (v.client_unit_id||v.vch_num||to_char(v.vch_date,'yyyyMMdd')||v.type) fstr,v.vch_num VoucherNo,TO_CHAR(v.vch_date,'dd/mm/yyyy') VoucherDate," +
                    //      "v.acode LedgerCode,c.c_name LedgerName,v.type,to_number(dramount) DrAmount,to_number(cramount) CrAmount from vouchers v " +
                    //      "inner join clients_mst c on c.vch_num = v.acode and c.type = 'BCD' " +
                    //      "where  (v.client_unit_id||v.vch_num||to_char(v.vch_date,'yyyyMMdd')||v.type) = '" + param1.Trim() + "'";

                    qu = "select (v.client_unit_id||v.vch_num||to_char(v.vch_date,'yyyyMMdd')||v.type) fstr,v.vch_num VoucherNo,TO_CHAR(v.vch_date,'dd/mm/yyyy') VoucherDate," +
                        "v.acode LedgerCode, nvl(c.ledger_name, '-') LedgerName,v.type,to_number(v.dramount) DrAmount,to_number(v.cramount) CrAmount from vouchers v " +
                        "left join (select a.acode,a.aname as ledger_name from (select vch_num as acode,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'BCD' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                "union all select vch_num as acode ,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                " union all " +
                                "select cm.vch_num as acode,nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname,cm.CLIENT_UNIT_ID,cm.vch_date,cm.type from master_setting a inner join clients_mst cm on " +
                                "a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id where cm.type = 'BAD' and cm.client_unit_id = '" + unitid_mst + "') a) c on c.acode=v.acode " +
                                "where(v.client_unit_id || v.vch_num || to_char(v.vch_date, 'yyyyMMdd') || v.type) = '" + param1.Trim() + "' and (to_number(v.dramount)+to_number(v.cramount))>0";
                    break;

            }
            return qu;
        }
        #endregion
        #region gst_ret
        public ActionResult gst_ret()
        {
            FillMst();
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
        public ActionResult gst_ret(List<Tmodelmain> model, string command)
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
            catch (Exception err) { }
            if (command == "Callback")
            {
                model = StartCallback(model);
            }
            else if (command == "Item Wise Sale")
            {
                mq = "select '-' as fstr, it.icode,it.iname,hsn.master_name as hsn_code,iv.IGST,iv.SGST,iv.CGST,iv.basic_amt,iv.GrossAmt,iv.net_amount from item it inner join(select iv.icode as item_code,iv.iname as item_name,sum(iv.gserchrg) as IGST,sum(iv.gloadchrg) as SGST,sum(iv.gamc) as CGST,sum(iv.basicamt) as basic_amt,sum(iv.ginstlchrg) GrossAmt,sum(iv.netamt) as net_amount from itransaction iv where substr(iv.type, 1, 1) = '5' and iv.client_id = '" + cl + "' and iv.client_unit_id = '" + ut + "' and to_date(iv.vch_date) between " +
                   "to_date('" + fdt + "','" + sgen.Getsqldateformat() + "') and to_date('" + tdt + "','" + sgen.Getsqldateformat() + "') group by(iv.icode, iv.iname) ) iv on it.icode = iv.item_code inner join master_setting hsn on hsn.master_id = it.hsno and hsn.type = 'HSN' and find_in_set(hsn.client_id, it.client_id)= 1 and find_in_set(hsn.client_unit_id, it.client_unit_id)= 1 where it.type = 'IT' and substr(it.icode,1,1)= '9' and it.client_id = '" + cl + "' and it.client_unit_id = '" + ut + "'";
                dt = sgen.getdata(userCode, mq);
                mq1 = sgen.open_grid_dt("Item Wise Sale", dt, 0);
                if (mq1 == "ok") { ViewBag.scripCall = "callFoo('Item Wise Sale');"; }
                else { ViewBag.scripCall = "showmsgJS(1,'No Data Found',2);"; }
            }
            else if (command == "Party Wise Sale")
            {
                mq = "select '-' as fstr, iv.acode as party_code,cl.C_name as Party_name,cl.c_gstin as Gst_no,sum(iv.gserchrg) as IGST,sum(iv.gloadchrg) as SGST,sum(iv.gamc) as CGST,sum(iv.basicamt) as basic_amt,sum(iv.ginstlchrg) GrossAmt,sum(iv.netamt) as net_amount from itransaction iv inner join clients_mst cl on cl.vch_num = iv.acode and cl.client_id = iv.client_id and cl.client_unit_id = iv.client_unit_id and cl.type = 'BCD' where substr(iv.type,1,1)= '5' and iv.client_id = '" + cl + "' and iv.client_unit_id = '" + ut + "' and to_date(iv.vch_date) between to_date('" + fdt + "','" + sgen.Getsqldateformat() + "') and to_date('" + tdt + "','" + sgen.Getsqldateformat() + "') group by(iv.acode, cl.C_name, cl.c_gstin)";

                dt = sgen.getdata(userCode, mq);
                mq1 = sgen.open_grid_dt("Party Wise Sale", dt, 0);
                if (mq1 == "ok") { ViewBag.scripCall = "callFoo('Party Wise Sale');"; }
                else { ViewBag.scripCall = "showmsgJS(1,'No Data Found',2);"; }
            }
            else if (command == "HSN Wise Sale Summary")
            {
                mq = "select '-' fstr, hsn.master_name as hsn_code,hsn.content,iv.IGST,iv.SGST,iv.CGST,iv.basic_amt,iv.net_amount from item it inner join (select iv.icode as item_code, iv.vch_date, " +
                           "sum(iv.gserchrg) as IGST,sum(iv.gloadchrg) as SGST,sum(iv.gamc) as CGST,sum(iv.basicamt) as basic_amt,sum(iv.ginstlchrg) GrossAmt,sum(iv.netamt) as net_amount from itransaction iv where substr(iv.type, 1, 1) = '5' and iv.client_id = '" + cl + "' " +
                           "and iv.client_unit_id = '" + ut + "' group by(iv.icode, iv.vch_date)) iv on iv.item_code = it.icode inner join master_setting" +
                           " hsn on hsn.master_id = it.hsno and hsn.type = 'HSN' and find_in_set(hsn.client_id, it.client_id)= 1 and find_in_set(hsn.client_unit_id," +
                           " it.client_unit_id)= 1 where it.type = 'IT' and substr(it.icode,1,1)= '9' and it.client_id = '" + cl + "' and " +
                           "it.client_unit_id = '" + ut + "' and to_date(iv.vch_date) between to_date('" + fdt + "','dd/mm/yyyy') and to_date('" + tdt + "','dd/mm/yyyy')";
                dt = sgen.getdata(userCode, mq);
                mq1 = sgen.open_grid_dt("HSN Wise Sale Summary", dt, 0);
                if (mq1 == "ok") { ViewBag.scripCall = "callFoo('HSN Wise Sale Summary');"; }
                else { ViewBag.scripCall = "showmsgJS(1,'No Data Found',2);"; }
            }
            else if (command == "Pending MRS")
            {
                mq = "";

                dt = sgen.getdata(userCode, mq);
                sgen.open_report_bydt_xml(userCode, dt, "slow_moving", "Report");
                ViewBag.scripCall += "showRptnew('Report');";

                //ViewBag.scripCall = "OpenSingle('../../../../../erp/schoolReports_Rpts/Filter_Report_xml.aspx','95%','900px','Reports');";
            }
            ModelState.Clear();
            return View(model);
        }
        #endregion
        #region Currency Rate master
        public ActionResult cur_rate()
        {
            FillMst();
            chkRef();
            ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.vsave = "disabled='disabled'";
            ViewBag.scripCall = "disableForm();";
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            List<Tmodelmain> model = new List<Tmodelmain>();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            Tmodelmain tm1 = new Tmodelmain();
            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = MyGuid.Trim();
            tm1.Col9 = "CURRENCY RATE MASTER";
            tm1.Col10 = "master_setting";
            tm1.Col12 = "FCR"; //Currency Rate master
            tm1.Col13 = "Save";
            tm1.Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";

            tm1.TList1 = mod1;
            tm1.SelectedItems1 = new string[] { "" };
            tm1.TList2 = mod1;
            tm1.SelectedItems2 = new string[] { "" };
            tm1.TList3 = mod1;
            tm1.SelectedItems3 = new string[] { "" };

            model.Add(tm1);
            return View(model);
        }
        [HttpPost]
        public ActionResult cur_rate(List<Tmodelmain> model, string command)
        {
            FillMst();
            var tm = model.FirstOrDefault();
            DataTable dt = new DataTable();

            #region ddl
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<SelectListItem> mod2 = new List<SelectListItem>();
            List<SelectListItem> mod3 = new List<SelectListItem>();

            TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
            TempData[MyGuid + "_TList2"] = model[0].TList2 = mod2 = (List<SelectListItem>)TempData[MyGuid + "_TList2"];
            TempData[MyGuid + "_TList3"] = model[0].TList3 = mod3 = (List<SelectListItem>)TempData[MyGuid + "_TList3"];

            if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
            if (tm.SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
            if (tm.SelectedItems3 == null) model[0].SelectedItems3 = new string[] { "" };

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
                    mod1 = new List<SelectListItem>();
                    mod2 = new List<SelectListItem>();
                    mod3 = new List<SelectListItem>();

                    #region Currency

                    mq1 = "select master_id, master_name from master_setting where  type='CTM' " + model[0].Col11 + "";

                    dt = sgen.getdata(userCode, mq1);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            mod1.Add(new SelectListItem { Text = dr["master_name"].ToString().ToUpper(), Value = dr["master_id"].ToString() });
                        }
                    }
                    TempData[MyGuid + "_Tlist1"] = model[0].TList1 = mod1;
                    #endregion
                    #region Currency Rate Reference

                    mq1 = "select master_id, master_name from master_setting where  type='CRS' " + model[0].Col11 + "";

                    dt = sgen.getdata(userCode, mq1);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            mod2.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                        }
                    }
                    TempData[MyGuid + "_Tlist2"] = model[0].TList2 = mod2;
                    #endregion
                    #region Applicable For

                    mq1 = "select master_id, master_name from master_setting where  type='CRA' ";

                    dt = sgen.getdata(userCode, mq1);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            mod3.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                        }
                    }
                    TempData[MyGuid + "_Tlist3"] = model[0].TList3 = mod3;
                    #endregion





                    mq1 = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                    string vch_num = sgen.genNo(userCode, mq1, 3, "auto_genid");
                    model[0].Col13 = "Save";
                    model[0].Col16 = vch_num;
                    model[0].Col20 = "Y";
                }
                catch (Exception ex) { }
            }
            else if (command == "Cancel")
            {
                return RedirectToAction("cur_rate", new { @m_id = EncryptDecrypt.Encrypt(model[0].Col15), @mid = EncryptDecrypt.Encrypt(model[0].Col14) });
            }
            else if (command == "Callback")
            {
                if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
                model = CallbackFun(btnval, "N", "cur_rate", "Fam", model);
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
            }
            else if (command == "Save" || command == "Update")
            {
                try
                {
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime(userCode);
                    string vch_num = "", found = "";
                    string type_desc = "currency rate";
                    if (model[0].Col20.Trim() == "N")
                    {
                        type = "DD" + type;
                    }
                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        mq1 = " and vch_num<>'" + tmodel.Col16.Trim() + "'";
                        isedit = true;
                        master_id = model[0].Col26;
                        vch_num = model[0].Col16;
                    }
                    else
                    {
                        isedit = false;
                        mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                        master_id = sgen.genNo(userCode, mq, 3, "auto_genid");
                        model[0].Col26 = master_id;
                        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                        model[0].Col16 = vch_num;
                    }
                    DataTable dataTable = sgen.getdata(userCode, "select * from " + model[0].Col10.Trim() + "  where  1=2");
                    DataRow dr = dataTable.NewRow();
                    dr["rec_id"] = "0";
                    dr["vch_num"] = vch_num;
                    dr["vch_date"] = currdate;
                    dr["type"] = model[0].Col12;
                    dr["master_type"] = type_desc;
                    dr["master_id"] = master_id;
                    dr["master_name"] = model[0].Col17;//custom dept ref rate
                    dr["classid"] = model[0].SelectedItems1.FirstOrDefault();//currency                    
                    dr["sectionid"] = model[0].Col18;//sale rate
                    dr["subjectid"] = model[0].Col19;//purchase rate
                    dr["optional_subject"] = model[0].SelectedItems2.FirstOrDefault();//applicable for
                    dr["teacher_category"] = model[0].SelectedItems3.FirstOrDefault();//rate source
                    //dr["status"] = model[0].Col20;//status
                    dr["date1"] = sgen.Make_date_S(model[0].Col29);//effdate
                    //dr["client_id"] = clientid_mst;
                    //dr["client_unit_id"] = unitid_mst;

                    if (isedit)
                    {
                        //dr["rec_id"] = "0";
                        dr["client_id"] = model[0].Col1;
                        dr["client_unit_id"] = model[0].Col2;
                        dr["master_entby"] = model[0].Col3;
                        dr["master_entdate"] = currdate;
                        dr["master_editby"] = userid_mst;
                        dr["master_editdate"] = currdate;
                    }
                    else
                    {
                        //dr["rec_id"] = "0";
                        dr["master_entby"] = userid_mst;
                        dr["master_entdate"] = currdate;
                        dr["master_editby"] = "-";
                        dr["master_editdate"] = currdate;
                        dr["client_id"] = clientid_mst;
                        dr["client_unit_id"] = unitid_mst; ;
                    }
                    dataTable.Rows.Add(dr);
                    bool Result = sgen.Update_data(userCode, dataTable, model[0].Col10, tmodel.Col8, isedit);
                    if (Result == true)
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
                            Col13 = tm.Col13,
                            Col14 = tm.Col14,
                            Col15 = tm.Col15,
                            TList1 = mod1,
                            TList2 = mod2,
                            TList3 = mod3,
                            SelectedItems1 = new string[] { "" },
                            SelectedItems2 = new string[] { "" },
                            SelectedItems3 = new string[] { "" },
                        });
                    }
                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
            }
            ModelState.Clear();
            return View(model);
        }

        #endregion
        #region bank_ac
        public ActionResult bank_ac()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<SelectListItem> mod2 = new List<SelectListItem>();
            List<SelectListItem> mod3 = new List<SelectListItem>();
            model[0].Col9 = "BANK ACCOUNT DETAILS";
            model[0].Col10 = "clients_mst";
            model[0].Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            model[0].Col12 = "BAD";
            model[0].Col131 = "306";
            model[0].Col31 = "Y";
            model[0].SelectedItems1 = new string[] { "" };
            model[0].SelectedItems2 = new string[] { "" };
            model[0].SelectedItems3 = new string[] { "" };
            model[0].Col31 = "N";
            TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
            TempData[MyGuid + "_TList2"] = model[0].TList2 = mod2;
            TempData[MyGuid + "_TList3"] = model[0].TList3 = mod3;
            sgen.SetSession(MyGuid, "BNK_PREFIX", model[0].Col131);
            model[0].LTM2 = new List<Tmodelmain>();
            Tmodelmain tmltm2 = new Tmodelmain();
            tmltm2.Col1 = "1";
            model[0].LTM2.Add(tmltm2);
            return View(model);
        }
        public List<Tmodelmain> newbank_ac(List<Tmodelmain> model)
        {
            try
            {
                string mq = "";
                model = getnew(model);
                List<SelectListItem> mod1 = new List<SelectListItem>();
                List<SelectListItem> mod2 = new List<SelectListItem>();
                List<SelectListItem> mod3 = new List<SelectListItem>();
                #region ddls
                TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1 = cmd_Fun.acctypevdm(userCode, unitid_mst);
                TempData[MyGuid + "_TList2"] = model[0].TList2 = mod2 = cmd_Fun.curname(userCode, unitid_mst);
                TempData[MyGuid + "_TList3"] = model[0].TList3 = mod3 = cmd_Fun.bank(userCode, unitid_mst);
                #endregion
                model[0].SelectedItems1 = new string[] { "" };
                model[0].SelectedItems2 = new string[] { "" };
                model[0].SelectedItems3 = new string[] { "" };
                model[0].Col31 = "N";
            }
            catch (Exception ex) { }
            return model;
        }
        [HttpPost]
        public ActionResult bank_ac(List<Tmodelmain> model, string command, string hfrow)
        {
            FillMst();
            var tm = model.FirstOrDefault();
            DataTable dt = new DataTable();
            #region  All dropdwon
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<SelectListItem> mod2 = new List<SelectListItem>();
            List<SelectListItem> mod3 = new List<SelectListItem>();
            TempData[MyGuid + "_TList1"] = model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
            TempData[MyGuid + "_TList2"] = model[0].TList2 = (List<SelectListItem>)TempData[MyGuid + "_TList2"];
            TempData[MyGuid + "_TList3"] = model[0].TList3 = (List<SelectListItem>)TempData[MyGuid + "_TList3"];
            if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
            if (tm.SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
            if (tm.SelectedItems3 == null) model[0].SelectedItems3 = new string[] { "" };
            #endregion
            if (command == "New")
            {
                model = newbank_ac(model);
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
                        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + " and substr(vch_num,0,3)='" + model[0].Col131 + "'";
                        //vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                        if (vch_num.Length > 6) { }
                        else vch_num = model[0].Col131 + vch_num;
                        model[0].Col16 = vch_num;
                        //vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                    }
                    DataTable dataTable = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    DataRow dr = dataTable.NewRow();
                    #region Dtstr
                    dr["type"] = model[0].Col12;
                    dr["vch_date"] = currdate;
                    dr["vch_num"] = vch_num;  //vch_num
                    dr["cpname"] = model[0].Col18;   //acc no
                    dr["msmeno"] = model[0].Col19;   //ifsc
                    dr["tanno"] = model[0].Col20;    //micr
                    dr["cpdesig"] = model[0].Col21;  //swift code
                    dr["ploc"] = model[0].SelectedItems1.FirstOrDefault();  //acc type
                    dr["Panno"] = model[0].SelectedItems2.FirstOrDefault();  //currecncy
                    dr["REFERED_BY"] = model[0].Col28;//Branch Name
                    dr["ptype"] = model[0].SelectedItems3.FirstOrDefault();//bnk id
                    dr["remark"] = model[0].Col29;//bnk add
                    dr["country"] = model[0].Col49;//country
                    dr["State"] = model[0].Col71;//state id
                    dr["City"] = model[0].Col64;//city_id
                    dr["bnk"] = model[0].Col31;//forward link
                    dr["cpaddr"] = model[0].LTM2[0].Col22;  //cp name--
                    dr["Cpaltcont"] = model[0].LTM2[0].Col23;  //mobile--
                    dr["cpemail"] = model[0].LTM2[0].Col24;  //email--
                    dr["Addr"] = model[0].LTM2[0].Col25;   //department--
                    dr["bnkaddr"] = model[0].LTM2[0].Col27;   //address--
                    dr["C_name"] = model[0].LTM2[0].Col26;  //designation--
                    dr = getsave(currdate, dr, model);
                    #endregion
                    dataTable.Rows.Add(dr);
                    bool Result = sgen.Update_data_fast1(userCode, dataTable, model[0].Col10, tmodel.Col8, isedit);
                    if (Result == true)
                    {
                        #region contact detail
                        DataTable dtstr = new DataTable();
                        dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                        for (int k = 0; k < model[0].LTM2.Count; k++)
                        {
                            dr = dtstr.NewRow();
                            dr["vch_num"] = model[0].Col16.Trim();
                            dr["vch_date"] = currdate;
                            dr["type"] = "BCP";
                            dr["cpaddr"] = model[0].LTM2[k].Col22;  //cp name--
                            dr["Cpaltcont"] = model[0].LTM2[k].Col23;  //mobile--
                            dr["cpemail"] = model[0].LTM2[k].Col24;  //email--
                            dr["Addr"] = model[0].LTM2[k].Col25;   //department--
                            dr["bnkaddr"] = model[0].LTM2[k].Col27;   //address--
                            dr["C_name"] = model[0].LTM2[k].Col26;  //designation--
                            dr = getsave(currdate, dr, model);
                            dtstr.Rows.Add(dr);
                        }
                        res = sgen.Update_data_fast1(userCode, dtstr, model[0].Col10, tmodel.Col85, isedit);
                        #endregion
                        if (res)
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
                                Col131 = tm.Col131,
                                Col31 = "N",
                                TList1 = mod1,
                                TList2 = mod2,
                                TList3 = mod3,
                                SelectedItems1 = new string[] { "" },
                                SelectedItems2 = new string[] { "" },
                                SelectedItems3 = new string[] { "" },
                                LTM2 = new List<Tmodelmain>()
                            });
                            Tmodelmain tmltm2 = new Tmodelmain();
                            tmltm2.Col1 = "1";
                            model[0].LTM2.Add(tmltm2);
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
                                    model = newbank_ac(model);
                                    ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                                }
                                catch (Exception ex) { }
                            }
                        }
                        else
                        {
                            ViewBag.scripCall = "showmsgJS(1,'Error Occured During Saving Contact Person Data, Please Check !', 0);";
                        }

                    }
                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
            }

            else if (command == "Add")
            {
                try
                {
                    Tmodelmain ntm1 = new Tmodelmain();
                    ntm1.Col1 = (Convert.ToInt32(model[0].LTM2.Max(x => sgen.Make_int(x.Col1))) + 1).ToString();
                    ntm1.SelectedItems2 = new string[] { "" };
                    ntm1.SelectedItems1 = new string[] { "" };
                    ntm1.SelectedItems3 = new string[] { "" };
                    model[0].LTM2.Add(ntm1);
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vsavenew = "";
                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "',0);"; }
            }
            else if (command == "Remove")
            {
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
            ModelState.Clear();
            return View(model);
        }
        #endregion
        #region chq_sr
        public ActionResult chq_sr()
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
            tm1.Col9 = "CHEQUE SERIES";
            tm1.Col10 = "vehicle_master";
            tm1.Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            tm1.Col12 = "CHK";
            tm1.Col13 = "Save";
            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = MyGuid.Trim();


            model.Add(tm1);
            return View(model);
        }

        [HttpPost]
        public ActionResult chq_sr(List<Tmodelmain> model, string command)
        {
            FillMst();


            DataTable dt = new DataTable();
            if (command == "New")
            {
                try
                {
                    sgen.SetSession(MyGuid, "EDMODE", "N");
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.scripCall = "enableForm();";

                    mq = "select max(to_number(vch_num)) as maxno from " + model[0].Col10 + " where type='" + model[0].Col12 + "' " + model[0].Col11 + " ";
                    vch_num = sgen.genNo(userCode, mq, 6, "maxno");
                    model[0].Col18 = vch_num;
                    model[0].Col13 = "Save";
                }

                catch (Exception ex)
                {
                    sgen.showmsg(1, ex.Message.ToString(), 2);
                }
            }
            else if (command == "Cancel")
            {
                return RedirectToAction("chq_sr", new { @m_id = EncryptDecrypt.Encrypt(model[0].Col15), @mid = EncryptDecrypt.Encrypt(model[0].Col14) });
            }
            else if (command == "Callback")
            {
                if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
                model = CallbackFun(btnval, "N", "chq_sr", "Account", model);
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
            }
            else if (command == "Save" || command == "Update")
            {
                try
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
                        mq = "select max(to_number(vch_num)) as maxno from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                        model[0].Col18 = vch_num;
                    }

                    DataTable dataTable = sgen.getdata(userCode, "select * from " + model[0].Col10.Trim() + " where  1=2");
                    for (int i = 0; i < model[0].dt1.Rows.Count; i++)
                    {
                        DataRow dr = dataTable.NewRow();
                        dr["vch_num"] = vch_num;
                        dr["type"] = model[0].Col12;
                        dr["vch_date"] = currdate;

                        dr["rec_id"] = "0";

                        dr["col39"] = model[0].Col16; // bank id
                        dr["col1"] = model[0].Col17; // country
                        dr["col2"] = model[0].Col18;  // state
                        dr["col3"] = model[0].Col19;  // district
                        dr["col13"] = model[0].Col20; // tehcil
                        dr["col40"] = model[0].Col21; // village
                        dr["col25"] = model[0].Col22;  //address
                        dr["col37"] = model[0].Col23; //serise from
                        dr["col38"] = model[0].Col24; //serise to
                        dr["col11"] = model[0].Col25; // currency 
                        dr["col12"] = model[0].Col26; // account id


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
                    if (Result == true)
                    {
                        dt = (DataTable)sgen.GetSession(MyGuid, "dtbase");
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col13 = "Save",
                            Col9 = tmodel.Col9,
                            Col10 = tmodel.Col10,
                            Col11 = tmodel.Col11,
                            Col12 = tmodel.Col12,
                            Col14 = tmodel.Col14,
                            Col15 = tmodel.Col15,
                            dt1 = dt
                        });

                        //ViewBag.scripCall = "showmsgJS(1, 'Record Saved Successfully', 1);disableForm();";
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                    }
                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
            }


            ModelState.Clear();
            return View(model);
        }

        #endregion
        #region Ledger master
        public ActionResult lgr_mst()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<SelectListItem> mod2 = new List<SelectListItem>();
            model = getload(model);
            model[0].Col9 = "LEDGER MASTER";
            model[0].Col10 = "clients_mst";
            model[0].Col12 = "LDG";//Acount Ledger Master
            model[0].Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            model[0].Col24 = ""; model[0].Col25 = "";
            model[0].TList1 = mod1;
            model[0].SelectedItems1 = new string[] { "" };
            model[0].TList2 = mod2;
            model[0].SelectedItems2 = new string[] { "" };
            return View(model);
        }
        public List<Tmodelmain> new_lgrmst(List<Tmodelmain> model)
        {
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<SelectListItem> mod2 = new List<SelectListItem>();
            #region Group type
            DataTable dt = new DataTable();
            dt = sgen.getdata(userCode, "select master_id,master_name from master_setting where type='TAC' and client_id = '" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    mod1.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                }
            }
            TempData[MyGuid + "_TList1"] = mod1;
            #endregion
            model[0].Col13 = "Save";
            model[0].Col100 = "Save & New";
            sgen.SetSession(MyGuid, "EDMODE", "N");
            ViewBag.vnew = "disabled='disabled'";
            ViewBag.vedit = "disabled='disabled'";
            ViewBag.vsave = "";
            ViewBag.vsavenew = "";
            ViewBag.scripCall = "enableForm();";
            model[0].Col16 = sgen.genNo(userCode, "select max(to_number(substr(vch_num,4,8))) as max from " + model[0].Col10 + " " +
                     "where type='" + model[0].Col12 + "' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'", 6, "max");
            model[0].TList1 = mod1;
            model[0].TList2 = mod2;
            model[0].SelectedItems1 = new string[] { "" };
            model[0].SelectedItems2 = new string[] { "" };
            return model;
        }
        [HttpPost]
        public ActionResult lgr_mst(List<Tmodelmain> model, string command)
        {
            FillMst();
            var tm = model.FirstOrDefault();
            #region ddl          
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<SelectListItem> mod2 = new List<SelectListItem>();
            model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
            model[0].TList2 = (List<SelectListItem>)TempData[MyGuid + "_TList2"];
            TempData[MyGuid + "_TList1"] = model[0].TList1;
            TempData[MyGuid + "_TList2"] = model[0].TList2;
            if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
            if (tm.SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
            #endregion
            if (command == "New")
            {
                try
                {
                    model = new_lgrmst(model);
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
            else if (command == "GROUP")
            {
                DataTable dt = new DataTable();
                try
                {
                    if (model[0].SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
                    if (model[0].SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
                    //dt = sgen.getdata(userCode, "select master_id,master_name from master_setting where type='ALM' and client_id = '" + clientid_mst + "' and client_unit_id='" + unitid_mst + "' and substr(master_id,1,1)='" + model[0].SelectedItems1.FirstOrDefault() + "'");
                    dt = sgen.getdata(userCode, "select substr(master_type,0,3) as master_id,master_name from master_setting where type='AGM' and client_id = '" + clientid_mst + "' and client_unit_id='" + unitid_mst + "' and substr(master_id,1,1)='" + model[0].SelectedItems1.FirstOrDefault() + "'");
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            mod2.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                        }
                    }
                    TempData[MyGuid + "_TList2"] = mod2;
                    model[0].TList2 = mod2;
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vsavenew = "";
                }
                catch (Exception ex) { }
            }
            else if (command == "LEDGER")
            {
                try
                {
                    if (model[0].SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
                    if (model[0].SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
                    model[0].Col25 = model[0].SelectedItems2.FirstOrDefault() + model[0].Col16;
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vsavenew = "";
                }
                catch (Exception ex) { }
            }
            else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
            {
                try
                {
                    string currdate = sgen.server_datetime_local(userCode);
                    currdate = sgen.Savedate(currdate, false);
                    string found = "", cond = "";

                    if (model[0].Col17 == "")
                    {
                        sgen.showmsg(1, "Please Enter Ledger Name (Long Text)", 2);
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                        return View(model);
                    }

                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        isedit = true;
                        vch_num = model[0].Col16;
                        cond = " and vch_num<>'" + model[0].Col16 + "'";
                    }
                    else
                    {
                        isedit = false;
                        model[0].Col16 = sgen.genNo(userCode, "select max(to_number(vch_num)) as max from " + model[0].Col10 + " " +
                            "where type='" + model[0].Col12 + "' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'", 6, "max");

                        vch_num = model[0].Col16;
                    }
                    found = sgen.getstring(userCode, "select c_name from " + model[0].Col10 + " where c_name='" + model[0].Col17 + "' and" +
                       " client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "' and type='" + model[0].Col12 + "'" + model[0].Col11 + "" + cond + "");
                    if (found != "")
                    {
                        ViewBag.scripCall = "showmsgJS(1,'Data Exists Already',2)";
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                        return View(model);
                    }
                    if (model[0].Col19 == "")
                    {
                        ViewBag.scripCall = "showmsgJS(1,'Please Select Group Name',2)";
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                        return View(model);
                    }
                    DataTable dataTable = sgen.getdata(userCode, "select * from " + model[0].Col10 + " where 1=2"); ;
                    DataRow dr = dataTable.NewRow();
                    dr["REF_CODE"] = vch_num;
                    dr["vch_date"] = currdate;
                    dr["type"] = model[0].Col12;
                    //dr["ledger_name"] = model[0].Col17;
                    //dr["ledger_abb"] = model[0].Col18;
                    //dr["Group_code"] = model[0].Col24;
                    //dr["ledger_code"] = model[0].Col25;
                    //dr["tally_name"] = model[0].Col31;
                    //dr["ledger_name"] = model[0].Col17;
                    //dr["ledger_abb"] = model[0].Col18;
                    //dr["Group_code"] = model[0].Col24;
                    //dr["ledger_code"] = model[0].Col25;
                    //dr["tally_name"] = model[0].Col31;
                    dr["c_name"] = model[0].Col17;
                    dr["CPNAME"] = model[0].Col18;
                    dr["CPLANDNO"] = model[0].Col24;
                    dr["vch_num"] = model[0].Col25;
                    dr["CP_MNAME"] = model[0].Col31;
                    dr["CPCONT"] = model[0].SelectedItems1.FirstOrDefault();
                    dr["CPALTCONT"] = model[0].SelectedItems2.FirstOrDefault();
                    dr["client_id"] = clientid_mst;
                    dr["client_unit_id"] = unitid_mst;
                    dr = getsave(currdate, dr, model);
                    dataTable.Rows.Add(dr);
                    if (dataTable.Rows.Count > 0)
                    {
                        bool Result = sgen.Update_data_fast1(userCode, dataTable, model[0].Col10, tm.Col8, isedit);
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
                                    model = new_lgrmst(model);
                                    ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                                }
                                catch (Exception ex) { }
                            }
                            model = new List<Tmodelmain>();
                            model.Add(new Tmodelmain
                            {
                                Col9 = tm.Col9,
                                Col10 = tm.Col10,
                                Col11 = tm.Col11,
                                Col13 = "Save",
                                Col100 = "Save & New",
                                Col14 = tm.Col14,
                                Col15 = tm.Col15,
                                Col12 = tm.Col12,
                                Col16 = tm.Col16,
                                TList2 = mod2,
                                TList1 = mod1,
                                SelectedItems2 = new string[] { "" },
                                SelectedItems1 = new string[] { "" },
                            });
                        }
                        else
                        {
                            ViewBag.scripCall = "showmsgJS(1,'Please Select Group Name',0)";
                        }
                    }
                }
                catch (Exception ex)
                {
                    sgen.showmsg(1, ex.Message.ToString(), 0);
                }
            }
            ModelState.Clear();
            return View(model);
        }
        #endregion
        #region Sale Voucher
        public ActionResult sl_voucher()
        {
            FillMst("");
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "VOUCHER ENTRY";
            model[0].Col10 = "vouchers";
            model[0].Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            if (model[0].Col14 == "22003.7")
            {
                model[0].Col12 = "50";
                model[0].Col9 = "PURCHASE VOUCHER";
            }
            else if (model[0].Col14 == "22003.8")
            {
                model[0].Col12 = "40";
                model[0].Col9 = "SALE VOUCHER";
            }
            else
            {
                model[0].Col12 = "VOU";
                model[0].Col9 = "VOUCHER ENTRY";
            }
            sgen.SetSession(MyGuid, "SLVOUTYPE", model[0].Col12);
            DataTable dt = sgen.getdata(userCode, "select '0' as  Select_ ,'1' as  Sno ,'-' as Acode,'-' as AName,'0' as DrAmount,'0' as  CrAmount from dual");
            model[0].dt1 = dt;
            sgen.SetSession(MyGuid, "KDTVOU", dt.Copy());
            if (Request.QueryString["vid"] != null)
            {
                string fstr = EncryptDecrypt.Decrypt(Request.QueryString["vid"].ToString().Trim());
                sgen.SetSession(MyGuid, "SSEEKVAL", fstr);
                CallbackFun("EDIT", "Y", "srv_inv", "Account", model);
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
                ViewBag.vsavenew = "";
                ViewBag.scripCall = "enableForm();";
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult sl_voucher(List<Tmodelmain> model, string command, string hfrow, string hftable)
        {
            FillMst(model[0].Col15);
            DataTable dtm = sgen.settable(hftable);
            model[0].dt1 = dtm;
            var tm = model.FirstOrDefault();
            DataTable dt = new DataTable();
            if (command == "New")
            {
                model = getnew(model);
            }
            else if (command == "Cancel")
            {
                return CancelFun(model);
            }
            else if (command == "Sync")
            {
                dt = new DataTable();
                mq = "select a.*,b.tally_name from vouchers a inner join ledger_master b on a.acode=b.vch_num and a.client_unit_id=b.client_unit_id "; ;
                dt = sgen.getdata(userCode, mq);
                sgen.Tally_Sync(sgen.JV_Multi_2("##SVCURRENTCOMPANY", "Journal", dt));
            }
            else if (command == "Callback")
            {
                model = StartCallback(model);
                if (btnval == "PRINT")
                {
                    ViewBag.vnew = "";
                    ViewBag.vedit = "";
                    ViewBag.vsave = "disabled='disabled'";
                    ViewBag.vsavenew = "disabled='disabled'";
                }
            }
            else if (command == "Save" || command == "Update")
            {
                try
                {
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
                        mq = "select max(to_number(vch_num)) as maxno from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "maxno");
                        model[0].Col16 = vch_num;
                    }
                    DataTable dataTable = sgen.getdata(userCode, "select * from " + model[0].Col10.Trim() + " where  1=2");
                    decimal cr = model[0].dt1.AsEnumerable().Sum(x => sgen.Make_decimal(x["CRAMOUNT"].ToString()));
                    decimal drr = model[0].dt1.AsEnumerable().Sum(x => sgen.Make_decimal(x["DRAMOUNT"].ToString()));
                    if (cr != drr)
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'Cr Amount Should be Equal To Dr Amount ,Please Check', 2);";
                        ModelState.Clear();
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                        return View(model);
                    }
                    string cr_rcode = "";
                    string dr_rcode = "";
                    for (int j = 0; j < model[0].dt1.Rows.Count; j++)
                    {
                        if (model[0].dt1.Rows[j][2].ToString() != "")
                        {
                            if (sgen.Make_decimal(model[0].dt1.Rows[j][4].ToString()) > 0)
                            {
                                if (cr_rcode == "") cr_rcode = model[0].dt1.Rows[j][2].ToString();///this is for debit row
                            }
                            else
                            {
                                dr_rcode = model[0].dt1.Rows[j][2].ToString();///this is for credit row
                            }
                        }
                    }
                    for (int i = 0; i < model[0].dt1.Rows.Count; i++)
                    {
                        DataRow dr = dataTable.NewRow();
                        dr["vch_num"] = vch_num;
                        if (model[0].Col14 == "22003.7") dr["type"] = "50";
                        else dr["type"] = model[0].Col12;
                        dr["vch_date"] = currdate;
                        dr["invno"] = model[0].Col19;
                        dr["invdate"] = sgen.Savedate(model[0].Col20, false);
                        dr["totremark"] = model[0].Col21;
                        dr["rno"] = i.ToString();
                        if (sgen.Make_decimal(model[0].dt1.Rows[i][4].ToString()) > 0) dr["rvscode"] = dr_rcode;
                        else dr["rvscode"] = cr_rcode;
                        dr["acode"] = model[0].dt1.Rows[i]["Acode"].ToString();
                        dr["aname"] = model[0].dt1.Rows[i]["Aname"].ToString();
                        dr["cramount"] = model[0].dt1.Rows[i]["CrAmount"].ToString();
                        dr["dramount"] = model[0].dt1.Rows[i]["DrAmount"].ToString();
                        dr = getsave(currdate, dr, model);
                        dataTable.Rows.Add(dr);
                    }
                    bool Result = sgen.Update_data_fast1(userCode, dataTable, model[0].Col10.Trim(), tmodel.Col8, isedit);
                    if (Result == true)
                    {
                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                        dt = (DataTable)sgen.GetSession(MyGuid, "KDTVOU");
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col13 = "Save",
                            Col100 = tmodel.Col100,
                            Col9 = tmodel.Col9,
                            Col10 = tmodel.Col10,
                            Col11 = tmodel.Col11,
                            Col12 = tmodel.Col12,
                            Col14 = tmodel.Col14,
                            Col15 = tmodel.Col15,
                            dt1 = dt
                        });
                    }
                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
            }
            else if (command == "+")
            {
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
                ViewBag.vsavenew = "";
                if (model[0].dt1.Rows.Count != 0) model[0].dt1.Rows.Add(sgen.Make_int(hfrow));
                else { model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "KDTVOU"); }
            }
            else if (command == "-")
            {
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
                ViewBag.vsavenew = "";
                if (model[0].dt1.Rows.Count > 1) model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                else { model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "KDTVOU"); }
            }

            ModelState.Clear();
            return View(model);
        }
        #endregion
        #region bank reciept
        public ActionResult rcm()
        {
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            FillMst(MyGuid);
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "BANK RECIEPT";
            model[0].Col10 = "vouchers";
            model[0].Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            model[0].Col12 = "11";
            List<SelectListItem> md1 = new List<SelectListItem>();
            TempData[MyGuid + "_TList1"] = model[0].TList1 = md1;
            TempData[MyGuid + "_TList2"] = model[0].TList2 = md1;
            TempData[MyGuid + "_TList3"] = model[0].TList3 = md1;
            model[0].SelectedItems1 = new string[] { "" };
            model[0].SelectedItems2 = new string[] { "" };
            model[0].SelectedItems3 = new string[] { "" };
            return View(model);
        }
        public List<Tmodelmain> new_rcm(List<Tmodelmain> model)
        {
            string year = year_from.Substring(6, 4);
            string currdate = sgen.server_datetime_dt(userCode).ToString("dd/MM/yyyy");
            model = getnew(model);
            var tm = model.FirstOrDefault();
            model[0].Col17 = sgen.server_datetime_local(userCode);
            #region Dropdwon
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<SelectListItem> mod2 = new List<SelectListItem>();
            List<SelectListItem> mod3 = new List<SelectListItem>();
            mq = "select nvl(a.master_name,'-')||'('||cm.cpname||')' as bank_name,cm.cpname as bank_ac,cm.vch_num as bankid from clients_mst cm " +
                "inner join master_setting a on a.master_id = cm.ptype and a.type = 'ABD' and find_in_set(a.client_unit_id,cm.client_unit_id)=1 " +
                "where cm.type = 'BAD' and find_in_set(cm.client_unit_id, '" + unitid_mst + "')=1";
            DataTable dt = new DataTable();
            dt = sgen.getdata(userCode, mq);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    mod1.Add(new SelectListItem { Text = item["bank_name"].ToString(), Value = item["bankid"].ToString() });
                }
            }
            mod2.Add(new SelectListItem { Text = "Advance", Value = "ADV" });
            mod2.Add(new SelectListItem { Text = "Against Ref.", Value = "AGR" });
            mod2.Add(new SelectListItem { Text = "New Ref.", Value = "NEW" });
            mod2.Add(new SelectListItem { Text = "On Account", Value = "OAC" });
            mq1 = "select a.acode,a.aname||'-'||'('||nvl(p.closing,'0')||')' as ledger_name from " +
                                "(select vch_num as acode,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'BCD' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                "union all select vch_num as acode ,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                                "union all select cm.vch_num as acode,nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname,cm.CLIENT_UNIT_ID,cm.vch_date,cm.type from master_setting a inner join clients_mst cm on " +
                                "a.master_id = cm.ptype and a.type = 'ABD' and find_in_set(a.client_unit_id, cm.client_unit_id)=1 where cm.type = 'BAD' and find_in_set(cm.client_unit_id,'" + unitid_mst + "')=1) a " +
                                "LEFT JOIN (SELECT ACODE,CLIENT_UNIT_ID,SUM(OPBAL) AS OPBAL,SUM(DRAMOUNT),SUM(CRAMOUNT),SUM(OPBAL)+SUM(DRAMOUNT)-SUM(CRAMOUNT) as closing from " +
                                "(select acode,CLIENT_UNIT_ID, OP_" + year + " as opbal,0 as cramount,0 as dramount,0 as cl from acbal union all select acode,CLIENT_UNIT_ID,sum(dramount)-sum(cramount) as opbal" +
",0 as cramount,0 as dramount,0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and " +
                                  "to_date('" + year_from.Split(' ')[0].Trim() + "','" + sgen.Getsqldateformat() + "')-1 group by acode,CLIENT_UNIT_ID union all select acode,CLIENT_UNIT_ID,0 as opbal,TO_NUMBER(cramount) cramount,TO_NUMBER(dramount) dramount,0 cl from vouchers " +
                                  "where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                                  "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + currdate.Trim() + "','" + sgen.Getsqldateformat() + "')) group by acode,CLIENT_UNIT_ID) p on p.acode=a.acode and p.client_unit_id=a.client_unit_id where find_in_set(a.client_unit_id, '" + unitid_mst + "')=1";
            DataTable dtemp = new DataTable();
            dtemp = sgen.getdata(userCode, mq1);
            if (dtemp.Rows.Count > 0)
            {
                foreach (DataRow item in dtemp.Rows)
                {
                    mod3.Add(new SelectListItem { Text = item["ledger_name"].ToString(), Value = item["acode"].ToString() });
                }
            }
            TempData[MyGuid + "_TList3"] = model[0].TList3 = mod3;
            TempData[MyGuid + "_TList2"] = model[0].TList2 = mod2;
            TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
            model[0].SelectedItems1 = new string[] { "" };
            model[0].SelectedItems2 = new string[] { "" };
            model[0].SelectedItems3 = new string[] { "" };
            #endregion
            return model;
        }
        [HttpPost]
        public ActionResult rcm(List<Tmodelmain> model, string command, string mid)
        {
            try
            {
                FillMst(model[0].Col15);
                var tm = model.FirstOrDefault();
                List<SelectListItem> mod1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
                TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
                if (model[0].SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
                List<SelectListItem> mod2 = (List<SelectListItem>)TempData[MyGuid + "_TList2"];
                TempData[MyGuid + "_TList2"] = model[0].TList2 = mod2;
                if (model[0].SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
                List<SelectListItem> mod3 = (List<SelectListItem>)TempData[MyGuid + "_TList3"];
                TempData[MyGuid + "_TList3"] = model[0].TList3 = mod3;
                if (model[0].SelectedItems3 == null) model[0].SelectedItems3 = new string[] { "" };
                if (command == "New")
                {
                    model = new_rcm(model);
                }
                else if (command == "Cancel")
                {
                    return CancelFun(model);
                }
                else if (command == "Get Data")
                {
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vsavenew = "";
                    if (model[0].SelectedItems3.FirstOrDefault() != "")
                    {
                        if (model[0].SelectedItems2.FirstOrDefault() == "NEW")
                        {
                            mq = "select (i.client_id||i.client_unit_id||i.doc_no||to_char(i.doc_date, 'yyyymmdd')|| i.type) as fstr,i.doc_no,to_char(i.doc_date, 'dd/MM/YYYY') as doc_date" +
                ",i.amt,i.type,to_number(i.amt) - to_number(nvl(v.bal, '0')) as bal_qty from (select i.acode, i.vch_num as doc_no,max(i.vch_date) doc_date,sum(i.gtaxamt) as amt" +
                ", i.client_id, i.client_unit_id, i.type from purchasesc i where i.pur_type = 'NPI' and substr(i.type, 1, 1) = '4' and to_number(i.gtaxamt) > 0 and client_unit_id='" + unitid_mst + "' " +
                "group by i.acode, i.vch_num, i.client_id,i.client_unit_id, i.type) i left join (select sum(cramount) as bal,invno,acode,client_unit_id,invdate from VOUCHERS v " +
                "where v.TYPE in ('10','11') AND v.REFTYPE = 'bill' group by invno,acode,client_unit_id,invdate ) v on v.invno = i.doc_no and v.acode = i.acode and v.client_unit_id = " +
                "i.client_unit_id  and to_char(i.doc_date,'yyyyMMdd') = to_char(v.invdate,'yyyyMMdd') where i.acode = '" + model[0].SelectedItems3.FirstOrDefault() + "'";
                        }
                        else
                        {
                            mq = "select (i.client_id||i.client_unit_id||i.doc_no||to_char(i.doc_date, 'yyyymmdd')|| i.type) as fstr,i.doc_no,to_char(i.doc_date, 'dd/MM/YYYY') as doc_date" +
                                ",i.amt,i.type,to_number(i.amt) - to_number(nvl(v.bal, '0')) as bal_qty from (select i.acode, i.vch_num as doc_no,max(i.vch_date) doc_date,sum(i.netamt) as amt" +
                                ", i.client_id, i.client_unit_id, i.type from itransaction i where i.type = '40' and substr(i.potype, 1, 1) = '5' and to_number(i.netamt) > 0 and client_unit_id='" + unitid_mst + "' " +
                                "group by i.acode, i.vch_num, i.client_id,i.client_unit_id, i.type) i left join (select sum(cramount) as bal,invno,acode,client_unit_id,invdate from VOUCHERS v " +
                                "where v.TYPE in ('11','10') AND v.REFTYPE = 'bill' group by invno,acode,client_unit_id,invdate ) v on v.invno = i.doc_no and v.acode = i.acode and v.client_unit_id = " +
                                "i.client_unit_id  and to_char(i.doc_date,'yyyyMMdd') = to_char(v.invdate,'yyyyMMdd') where i.acode = '" + model[0].SelectedItems3.FirstOrDefault() + "'";
                        }
                        DataTable dtques = new DataTable();
                        dtques = sgen.getdata(userCode, mq);
                        if (dtques.Rows.Count > 0)
                        {
                            model = new List<Tmodelmain>();
                            foreach (DataRow dr in dtques.Rows)
                            {
                                model.Add(new Tmodelmain
                                {
                                    Col8 = tm.Col8,
                                    Col9 = tm.Col9,
                                    Col11 = tm.Col11,
                                    Col12 = tm.Col12,
                                    Col10 = tm.Col10,
                                    Col13 = tm.Col13,
                                    Col14 = tm.Col14,
                                    Col15 = tm.Col15,
                                    Col16 = tm.Col16,
                                    Col17 = tm.Col17,
                                    Col18 = tm.Col18,
                                    Col19 = tm.Col19,
                                    Col20 = tm.Col20,
                                    Col21 = tm.Col21,
                                    Col22 = tm.Col22,
                                    Col23 = tm.Col23,
                                    Col24 = tm.Col24,
                                    Col42 = tm.Col42,
                                    Col25 = tm.Col25,
                                    Col26 = tm.Col26,
                                    Col27 = tm.Col27,
                                    Col40 = tm.Col40,
                                    Col28 = tm.Col28,
                                    Col29 = tm.Col29,
                                    Col100 = tm.Col100,
                                    Col121 = tm.Col121,
                                    Col122 = tm.Col122,
                                    Col123 = tm.Col123,
                                    TList1 = tm.TList1,
                                    SelectedItems1 = tm.SelectedItems1,
                                    TList2 = tm.TList2,
                                    SelectedItems2 = tm.SelectedItems2,
                                    TList3 = tm.TList3,
                                    SelectedItems3 = tm.SelectedItems3,
                                    Col30 = dr["doc_no"].ToString(),
                                    Col31 = dr["doc_date"].ToString(),
                                    Col32 = dr["bal_qty"].ToString(),
                                    Col33 = "0",
                                    Col34 = "-",
                                });
                            }
                        }
                    }
                }
                else if (command == "Callback")
                {
                    model = StartCallback(model);
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
                    string currdate = sgen.server_datetime(userCode);
                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        isedit = true;
                        vch_num = model[0].Col16;
                    }
                    else
                    {
                        isedit = false;
                        mq = "select max(to_number(vch_num)) as maxno from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "maxno");
                        model[0].Col16 = vch_num;
                    }
                    DataTable dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10);
                    decimal cr1 = 0, drr = 0, dr2 = 0;
                    if (model[0].SelectedItems2.FirstOrDefault() == "ADV")
                    {
                        cr1 = model.AsEnumerable().Sum(x => sgen.Make_decimal(x.Col26));
                        drr = model.AsEnumerable().Sum(x => sgen.Make_decimal(x.Col26));
                    }
                    else if (model[0].SelectedItems2.FirstOrDefault() == "OAC")
                    {
                        cr1 = model.AsEnumerable().Sum(x => sgen.Make_decimal(x.Col26));
                        drr = model.AsEnumerable().Sum(x => sgen.Make_decimal(x.Col27));
                    }
                    else
                    {
                        cr1 = model.AsEnumerable().Sum(x => sgen.Make_decimal(x.Col26));
                        drr = model.AsEnumerable().Sum(x => sgen.Make_decimal(x.Col33));
                        dr2 = model.AsEnumerable().Sum(x => sgen.Make_decimal(x.Col27));
                    }
                    if (cr1 != drr + dr2)
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'Cr Amount Should be Equal To Dr Amount ,Please Check', 2);";
                        ModelState.Clear();
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                        return View(model);
                    }
                    string bankname = ((List<SelectListItem>)TempData[MyGuid + "_TList1"]).SingleOrDefault(item => item.Value == model[0].SelectedItems1.FirstOrDefault().ToString()).Text.ToString();
                    string ledgername = ((List<SelectListItem>)TempData[MyGuid + "_TList3"]).SingleOrDefault(item => item.Value == model[0].SelectedItems3.FirstOrDefault().ToString()).Text.ToString();
                    ledgername = ledgername.Trim().Split('-')[0].Trim();

                    //--------bank
                    DataRow dr = dtstr.NewRow();
                    dr["vch_num"] = vch_num;
                    dr["rno"] = "0";
                    dr["type"] = model[0].Col12;
                    dr["vch_date"] = sgen.Make_date_S(model[0].Col17);
                    dr["totremark"] = model[0].Col40;
                    dr["acode"] = model[0].SelectedItems1.FirstOrDefault();
                    dr["adj_type"] = model[0].SelectedItems2.FirstOrDefault();
                    dr["aname"] = bankname;
                    dr["chqno"] = model[0].Col28;
                    dr["chqdate"] = sgen.Make_date_S(model[0].Col29);
                    dr["chqamt"] = model[0].Col26;
                    dr["reftype"] = "BNK";
                    dr["cramount"] = "0";
                    dr["rvscode"] = model[0].SelectedItems3.FirstOrDefault();//party reverse code
                    dr["dramount"] = model[0].Col26;
                    dr["invno"] = model[0].Col30;
                    dr["remark"] = model[0].Col34;
                    dr["type1"] = "BLKRCPT";
                    dr["invdate"] = sgen.Make_date_S(model[0].Col31);
                    dr = getsave(currdate, dr, model);
                    dtstr.Rows.Add(dr);
                    //--------advance
                    dr = dtstr.NewRow();
                    dr["vch_num"] = vch_num;
                    dr["rno"] = "0";
                    dr["type"] = model[0].Col12;
                    dr["vch_date"] = sgen.Make_date_S(model[0].Col17);
                    dr["totremark"] = model[0].Col40;
                    dr["acode"] = model[0].SelectedItems3.FirstOrDefault();
                    dr["aname"] = ledgername;
                    dr["chqno"] = model[0].Col28;
                    dr["chqdate"] = sgen.Make_date_S(model[0].Col29);
                    dr["adj_type"] = model[0].SelectedItems2.FirstOrDefault();
                    dr["rvscode"] = model[0].SelectedItems1.FirstOrDefault();//bank reverse code
                    dr["chqamt"] = model[0].Col26;
                    dr["reftype"] = "ADV";
                    if (model[0].SelectedItems2.FirstOrDefault() == "ADV")
                    {
                        dr["dramount"] = "0";
                        dr["cramount"] = model[0].Col26;
                    }
                    else if (model[0].SelectedItems2.FirstOrDefault() == "OAC")
                    {
                        dr["dramount"] = "0";
                        dr["cramount"] = model[0].Col27;
                    }
                    else
                    {
                        dr["dramount"] = "0";
                        dr["cramount"] = model[0].Col27;
                    }
                    dr["type1"] = "BLKRCPT";
                    dr = getsave(currdate, dr, model);
                    dtstr.Rows.Add(dr);
                    //-------------BILL
                    for (int i = 0; i < model.Count; i++)
                    {
                        if (model[0].Col30 != null)
                        {
                            if (!model[i].Col33.Trim().Equals("0") && !model[i].Col33.Trim().Equals(""))
                            {
                                dr = dtstr.NewRow();
                                dr["vch_num"] = vch_num;
                                dr["rno"] = i.ToString();
                                dr["type"] = model[0].Col12;
                                dr["vch_date"] = sgen.Make_date_S(model[0].Col17);
                                dr["invno"] = model[i].Col30;
                                dr["invamt"] = model[i].Col32;
                                dr["remark"] = model[i].Col34;
                                dr["invdate"] = sgen.Make_date_S(model[i].Col31);
                                dr["totremark"] = model[0].Col40;
                                //dr["acode"] = model[0].Col23;
                                //dr["aname"] = model[0].Col24;
                                dr["acode"] = model[0].SelectedItems3.FirstOrDefault();
                                dr["aname"] = ledgername;
                                dr["chqno"] = model[0].Col21;
                                dr["rvscode"] = model[0].SelectedItems1.FirstOrDefault();//bank reverse code
                                dr["adj_type"] = model[0].SelectedItems2.FirstOrDefault();
                                dr["reftype"] = "BILL";
                                dr["chqno"] = model[0].Col28;
                                dr["chqamt"] = model[0].Col26;
                                dr["chqdate"] = sgen.Make_date_S(model[0].Col29);
                                dr["cramount"] = model[i].Col33;
                                dr["dramount"] = "0";
                                dr["type1"] = "BLKRCPT";
                                dr = getsave(currdate, dr, model);
                                dtstr.Rows.Add(dr);
                            }
                        }
                    }
                    bool Result = sgen.Update_data_fast1(userCode, dtstr, model[0].Col10.Trim(), tm.Col8, isedit);
                    if (Result)
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
                            TList2 = mod2,
                            TList3 = mod3,
                            SelectedItems1 = new string[] { "" },
                            SelectedItems2 = new string[] { "" },
                            SelectedItems3 = new string[] { "" },
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
                                model = new_rcm(model);
                                ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                            }
                            catch (Exception ex) { }
                        }
                    }
                    else
                    {
                        ViewBag.scripCall = "showmsgJS(1, 'Record Not Saved', 2);";
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
        #region PARTY_opening
        public ActionResult party_opening()
        {
            FillMst();
            chkRef();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            List<SelectListItem> mod1 = new List<SelectListItem>();
            model[0].Col10 = "acbal";
            model[0].Col9 = "LEDGER OPENING";
            model[0].Col12 = "AC";
            model[0].Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            model[0].SelectedItems1 = new string[] { "" };
            TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
            DataTable dt = sgen.getdata(userCode, "select '-' as  Select_, '0' as SNo,'-' as acode,'-' as AName,'0' as Opening_amount from dual");
            model[0].dt1 = dt;
            sgen.SetSession(MyGuid, "ACBDT", dt);
            ViewBag.vopbal = "disabled='disabled'";
            return View(model);
        }
        public List<Tmodelmain> new_party_opening(List<Tmodelmain> model)
        {
            List<SelectListItem> mod1 = new List<SelectListItem>();
            model = getnew(model);
            ViewBag.vopbal = "";
            return model;
        }
        [HttpPost]
        public ActionResult party_opening(List<Tmodelmain> model, string command, string hfrow, string hftable, HttpPostedFileBase upd1)
        {
            try
            {
                FillMst(model[0].Col15);
                DataTable dtm = sgen.settable(hftable);
                string year = year_from.Substring(6, 4);
                model[0].dt1 = dtm;
                var tmodel = model.FirstOrDefault();
                #region ddl
                //List<SelectListItem> mod1 = new List<SelectListItem>();
                //model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
                //TempData[MyGuid + "_TList1"] = model[0].TList1;
                //if (tmodel.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
                #endregion
                DataTable dt = new DataTable();
                DataTable dataTable = new DataTable();
                if (command == "New")
                {
                    model = new_party_opening(model);
                }
                if (command == "Cancel") { return CancelFun(model); }
                else if (command == "Callback") { model = StartCallback(model); }
                else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
                {
                    try
                    {
                        if (model[0].Col133 == "N")
                        {
                            ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for save data , please contact your admin', 2);";
                            return View(model);
                        }
                        if (model[0].dt1.Rows[0]["acode"].ToString().Trim().Equals("-"))
                        {
                            ViewBag.scripCall += "showmsgJS(1,'Please Select Party in grid', 2);";
                            ViewBag.vnew = "disabled='disabled'";
                            ViewBag.vedit = "disabled='disabled'";
                            ViewBag.vsave = "";
                            ViewBag.vsavenew = "";
                            return View(model);
                        }
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
                        string vdate = sgen.Make_date_S(model[0].Col17);
                        dataTable = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                        string cond = "";
                        for (int i = 0; i < model[0].dt1.Rows.Count; i++)
                        {
                            if (model[0].dt1.Rows[i]["Opening_amount"].ToString().Trim().Equals("0") || model[0].dt1.Rows[i]["Opening_amount"].ToString().Trim().Equals(""))
                            {
                                ViewBag.scripCall += "showmsgJS(1,'Please Fill Opening Quantity and Amount in grid-1', 0);";
                                ViewBag.vnew = "disabled='disabled'";
                                ViewBag.vedit = "disabled='disabled'";
                                ViewBag.vsave = "";
                                ViewBag.vsavenew = "";
                                return View(model);
                            }
                            DataRow dr = dataTable.NewRow();
                            dr["vch_num"] = vch_num;
                            dr["type"] = model[0].Col12;
                            dr["vch_date"] = vdate;
                            dr["acyear"] = year;
                            dr["acode"] = model[0].dt1.Rows[i]["acode"].ToString();
                            dr["OP_" + year] = model[0].dt1.Rows[i]["Opening_amount"].ToString();
                            cond += "('" + model[0].dt1.Rows[i]["acode"].ToString() + year + "')";
                            dr = getsave(currdate, dr, model);
                            dataTable.Rows.Add(dr);
                        }
                        //Satransaction sat = new Satransaction(userCode, MyGuid);
                        if (cond.Length > 3) cond = cond.Substring(0, cond.Length);
                        cond = "type='AC' and trim(acode)||trim(acyear) in " + cond + "";
                        bool ok = sgen.Update_data_fast1(userCode, dataTable, model[0].Col10.Trim(), cond, true);
                        if (ok == true)
                        {
                            //for (int i = 0; i < model[0].dt1.Rows.Count; i++)
                            //{
                            //    cmd = "select nvl(sum(op_" + year + "),0) from " + model[0].Col10 + " where acode='" + model[0].dt1.Rows[i]["acode"].ToString() + "' and acyear='" + year + "' and vch_num not in ('" + vch_num + "')";

                            //    res = sat.Execute_cmd("update " + model[0].Col10 + " set op_" + year + " = (" + cmd + " + " + sgen.Make_decimal(model[0].dt1.Rows[i]["Opening_amount"].ToString()) + ") " +
                            //        "WHERE ACODE='" + model[0].dt1.Rows[i]["acode"].ToString() + "' and client_unit_id='" + unitid_mst + "'");
                            //    if (!res) break;
                            //}
                            //if (res)
                            //{
                            //sat.Commit();
                            //ViewBag.vnew = "";
                            //ViewBag.vedit = "";
                            //ViewBag.vsave = "disabled='disabled'";
                            //ViewBag.vsavenew = "disabled='disabled'";
                            ViewBag.vopbal = "disabled='disabled'";
                            dt = (DataTable)sgen.GetSession(MyGuid, "ACBDT");
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
                                //TList1 = mod1,
                                //SelectedItems1 = new string[] { "" },
                                dt1 = dt
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
                                    model = new_party_opening(model);
                                    ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                                }
                                catch (Exception ex) { }
                            }
                            //  ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                        }
                        //else
                        //{
                        //    sat.Rollback();
                        //    ViewBag.vnew = "disabled='disabled'";
                        //    ViewBag.vedit = "disabled='disabled'";
                        //    ViewBag.vopbal = "";
                        //    ViewBag.vsave = "";
                        //    ViewBag.vsavenew = "";
                        //    ViewBag.scripCall += "showmsgJS(1, 'Error in Updation of Stock', 0);";
                        //}
                        //}
                        else
                        {
                            //sat.Rollback();
                            ViewBag.vnew = "disabled='disabled'";
                            ViewBag.vedit = "disabled='disabled'";
                            ViewBag.vsave = "";
                            ViewBag.vopbal = "";
                            ViewBag.vsavenew = "";
                            ViewBag.scripCall += "showmsgJS(1, 'Data Not Saved', 0);";
                        }
                    }
                    catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString().Replace('\'', ' ') + "', 0);"; }
                }
                else if (command == "-")
                {
                    if (model[0].dt1.Rows.Count > 1) model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                    else { model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "ACBDT"); }
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
        #region payamt
        public ActionResult payamt()
        {
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            FillMst(MyGuid);
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "BANK PAYMENT";
            model[0].Col10 = "vouchers";
            model[0].Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            model[0].Col12 = "21";
            List<SelectListItem> md1 = new List<SelectListItem>();
            TempData[MyGuid + "_TList1"] = model[0].TList1 = md1;
            TempData[MyGuid + "_TList2"] = model[0].TList2 = md1;
            TempData[MyGuid + "_TList3"] = model[0].TList3 = md1;
            TempData[MyGuid + "_TList4"] = model[0].TList4 = md1;
            model[0].SelectedItems1 = new string[] { "" };
            model[0].SelectedItems2 = new string[] { "" };
            model[0].SelectedItems3 = new string[] { "" };
            model[0].SelectedItems4 = new string[] { "" };
            return View(model);
        }
        public List<Tmodelmain> new_payamt(List<Tmodelmain> model)
        {
            string year = year_from.Substring(6, 4);
            string currdate = sgen.server_datetime_dt(userCode).ToString("dd/MM/yyyy");
            model = getnew(model);
            var tm = model.FirstOrDefault();
            model[0].Col17 = sgen.server_datetime_local(userCode);
            #region Dropdwon
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<SelectListItem> mod2 = new List<SelectListItem>();
            List<SelectListItem> mod4 = new List<SelectListItem>();
            List<SelectListItem> mod3 = new List<SelectListItem>();
            mq = "select nvl(a.master_name,'-')||'('||cm.cpname||')' as bank_name,cm.cpname as bank_ac,cm.vch_num as bankid from clients_mst cm " +
                "inner join master_setting a on a.master_id = cm.ptype and a.type = 'ABD' and find_in_set(a.client_unit_id, cm.client_unit_id)=1 " +
                "where cm.type = 'BAD' and find_in_set(cm.client_unit_id, '" + unitid_mst + "')=1";
            DataTable dt = new DataTable();
            dt = sgen.getdata(userCode, mq);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    mod1.Add(new SelectListItem { Text = item["bank_name"].ToString(), Value = item["bankid"].ToString() });
                }
            }
            TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
            TempData[MyGuid + "_TList3"] = model[0].TList3 = mod3;
            mod2.Add(new SelectListItem { Text = "Advance", Value = "ADV" });
            mod2.Add(new SelectListItem { Text = "Against Ref.", Value = "AGR" });
            mod2.Add(new SelectListItem { Text = "New Ref.", Value = "NEW" });
            mod2.Add(new SelectListItem { Text = "On Account", Value = "OAC" });
            TempData[MyGuid + "_TList2"] = model[0].TList2 = mod2;
            mq1 = "select a.acode,a.aname||'-'||'('||nvl(p.closing,'0')||')' as ledger_name from " +
                              "(select vch_num as acode,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'BCD' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                              "union all select vch_num as acode ,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                              "union all select cm.vch_num as acode,nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname,cm.CLIENT_UNIT_ID,cm.vch_date,cm.type from master_setting a inner join clients_mst cm on " +
                              "a.master_id = cm.ptype and a.type = 'ABD' and find_in_set(a.client_unit_id, cm.client_unit_id)=1 where cm.type = 'BAD' and find_in_set(cm.client_unit_id,'" + unitid_mst + "')=1) a " +
                              "LEFT JOIN (SELECT ACODE,CLIENT_UNIT_ID,SUM(OPBAL) AS OPBAL,SUM(DRAMOUNT),SUM(CRAMOUNT),SUM(OPBAL)+SUM(DRAMOUNT)-SUM(CRAMOUNT) as closing from " +
                              "(select acode,CLIENT_UNIT_ID, OP_" + year + " as opbal,0 as cramount,0 as dramount,0 as cl from acbal union all select acode,CLIENT_UNIT_ID,sum(dramount)-sum(cramount) as opbal" +
",0 as cramount,0 as dramount,0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and " +
                                "to_date('" + year_from.Split(' ')[0].Trim() + "','" + sgen.Getsqldateformat() + "')-1 group by acode,CLIENT_UNIT_ID union all select acode,CLIENT_UNIT_ID,0 as opbal,TO_NUMBER(cramount) cramount,TO_NUMBER(dramount) dramount,0 cl from vouchers " +
                                "where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                                "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + currdate.Trim() + "','" + sgen.Getsqldateformat() + "')) group by acode,CLIENT_UNIT_ID) p on p.acode=a.acode and p.client_unit_id=a.client_unit_id where find_in_set(a.client_unit_id, '" + unitid_mst + "')=1";
            DataTable dtemp = new DataTable();
            dtemp = sgen.getdata(userCode, mq1);
            if (dtemp.Rows.Count > 0)
            {
                foreach (DataRow item in dtemp.Rows)
                {
                    mod4.Add(new SelectListItem { Text = item["ledger_name"].ToString(), Value = item["acode"].ToString() });
                }
            }
            TempData[MyGuid + "_TList4"] = model[0].TList4 = mod4;
            model[0].SelectedItems1 = new string[] { "" };
            model[0].SelectedItems2 = new string[] { "" };
            model[0].SelectedItems3 = new string[] { "" };
            model[0].SelectedItems4 = new string[] { "" };
            #endregion
            return model;
        }
        [HttpPost]
        public ActionResult payamt(List<Tmodelmain> model, string command, string mid)
        {
            try
            {
                FillMst(model[0].Col15);
                var tm = model.FirstOrDefault();
                List<SelectListItem> mod1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
                TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
                if (model[0].SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
                List<SelectListItem> mod2 = (List<SelectListItem>)TempData[MyGuid + "_TList2"];
                TempData[MyGuid + "_TList2"] = model[0].TList2 = mod2;
                if (model[0].SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
                List<SelectListItem> mod3 = (List<SelectListItem>)TempData[MyGuid + "_TList3"];
                TempData[MyGuid + "_TList3"] = model[0].TList3 = mod3;
                if (model[0].SelectedItems3 == null) model[0].SelectedItems3 = new string[] { "" };
                List<SelectListItem> mod4 = (List<SelectListItem>)TempData[MyGuid + "_TList4"];
                TempData[MyGuid + "_TList4"] = model[0].TList4 = mod4;
                if (model[0].SelectedItems4 == null) model[0].SelectedItems4 = new string[] { "" };
                if (command == "New")
                {
                    model = new_payamt(model);
                }
                else if (command == "Cancel")
                {
                    return CancelFun(model);
                }
                else if (command == "Bank")
                {
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vsavenew = "";
                    ViewBag.scripCall = "enableForm();";
                    DataTable dt = new DataTable();
                    string from = sgen.seekval(userCode, "select col9 as fromval from enx_tab where type='CSR' and client_unit_id='" + unitid_mst + "' and col2 = '" + model[0].SelectedItems1.FirstOrDefault() + "'", "fromval");
                    string to = sgen.seekval(userCode, "select col10 as toval from enx_tab where type='CSR' and client_unit_id='" + unitid_mst + "' and col2 ='" + model[0].SelectedItems1.FirstOrDefault() + "'", "toval");
                    dt = sgen.getdata(userCode, "SELECT R FROM (Select Rownum r From dual Connect By Rownum <= " + sgen.Make_int(to) + ") WHERE R BETWEEN " + sgen.Make_int(from) + " AND " + sgen.Make_int(to) + " AND to_char(R) NOT IN (select chqno from vouchers where type='21' and type1 = 'BLKPAY' and acode='" + model[0].SelectedItems1.FirstOrDefault() + "' union all " +
                        "select col3 as chqno from enx_tab where type='CRJ' and client_unit_id = '" + unitid_mst + "' and col2='" + model[0].SelectedItems1.FirstOrDefault() + "')");
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            mod3.Add(new SelectListItem { Text = dr["R"].ToString(), Value = dr["R"].ToString() });
                        }
                    }
                    TempData[MyGuid + "_TList3"] = model[0].TList3 = mod3;
                }
                else if (command == "Get Data")
                {
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vsavenew = "";
                    if (model[0].SelectedItems4.FirstOrDefault() != "")
                    {
                        DataTable dtques = new DataTable();
                        if (model[0].SelectedItems2.FirstOrDefault() == "NEW")
                        {
                            mq = "select (i.client_id||i.client_unit_id||i.doc_no||to_char(i.doc_date, 'yyyymmdd')|| i.type) as fstr,i.doc_no,to_char(i.doc_date, 'dd/MM/YYYY') as doc_date" +
                     ",i.amt,i.type,to_number(i.amt) - to_number(nvl(v.bal, '0')) as bal_qty from (select i.acode, i.vch_num as doc_no,max(i.vch_date) doc_date,sum(i.netamt) as amt" +
                     ", i.client_id, i.client_unit_id, i.type from purchases i where i.pur_type in ('001','002') and substr(i.type, 1, 1) = '5' and to_number(i.netamt) > 0 and client_unit_id='" + unitid_mst + "' " +
                     "group by i.acode, i.vch_num, i.client_id,i.client_unit_id, i.type) i left join (select sum(dramount) as bal,invno,acode,client_unit_id,invdate from VOUCHERS v " +
                     "where v.TYPE in ('21','20') AND v.REFTYPE = 'bill' group by invno,acode,client_unit_id,invdate ) v on v.invno = i.doc_no and v.acode = i.acode and v.client_unit_id = " +
                     "i.client_unit_id  and to_char(i.doc_date,'yyyyMMdd') = to_char(v.invdate,'yyyyMMdd') where i.acode = '" + model[0].SelectedItems4.FirstOrDefault() + "'";
                        }
                        else
                        {
                            mq = "select (i.client_id||i.client_unit_id||i.doc_no||to_char(i.doc_date, 'yyyymmdd')|| i.type) as fstr,i.doc_no,to_char(i.doc_date, 'dd/MM/YYYY') as doc_date" +
                        ",i.amt,i.type,to_number(i.amt) - to_number(nvl(v.bal, '0')) as bal_qty from (select i.acode, i.vch_num as doc_no,max(i.vch_date) doc_date,sum(i.netamt) as amt" +
                        ", i.client_id, i.client_unit_id, i.type from itransaction i where i.type in ('01','02','03','05','07','08','09') and to_number(i.netamt) > 0 and client_unit_id='" + unitid_mst + "' " +
                        "group by i.acode, i.vch_num, i.client_id,i.client_unit_id, i.type) i left join (select sum(dramount) as bal,invno,acode,client_unit_id,invdate from VOUCHERS v " +
                        "where v.TYPE in ('21','20') AND v.REFTYPE = 'bill' group by invno,acode,client_unit_id,invdate ) v on v.invno = i.doc_no and v.acode = i.acode and v.client_unit_id = " +
                        "i.client_unit_id  and to_char(i.doc_date,'yyyyMMdd') = to_char(v.invdate,'yyyyMMdd') where i.acode = '" + model[0].SelectedItems4.FirstOrDefault() + "'";
                        }
                        dtques = sgen.getdata(userCode, mq);
                        if (dtques.Rows.Count > 0)
                        {
                            model = new List<Tmodelmain>();
                            foreach (DataRow dr in dtques.Rows)
                            {
                                model.Add(new Tmodelmain
                                {
                                    Col8 = tm.Col8,
                                    Col9 = tm.Col9,
                                    Col11 = tm.Col11,
                                    Col12 = tm.Col12,
                                    Col10 = tm.Col10,
                                    Col13 = tm.Col13,
                                    Col14 = tm.Col14,
                                    Col15 = tm.Col15,
                                    Col16 = tm.Col16,
                                    Col17 = tm.Col17,
                                    Col18 = tm.Col18,
                                    Col19 = tm.Col19,
                                    Col20 = tm.Col20,
                                    Col21 = tm.Col21,
                                    Col22 = tm.Col22,
                                    Col23 = tm.Col23,
                                    Col24 = tm.Col24,
                                    Col42 = tm.Col42,
                                    Col25 = tm.Col25,
                                    Col26 = tm.Col26,
                                    Col27 = tm.Col27,
                                    Col40 = tm.Col40,
                                    Col28 = tm.Col28,
                                    Col29 = tm.Col29,
                                    Col100 = tm.Col100,
                                    Col121 = tm.Col121,
                                    Col122 = tm.Col122,
                                    Col123 = tm.Col123,
                                    TList1 = tm.TList1,
                                    SelectedItems1 = tm.SelectedItems1,
                                    TList2 = tm.TList2,
                                    SelectedItems2 = tm.SelectedItems2,
                                    TList3 = tm.TList3,
                                    SelectedItems3 = tm.SelectedItems3,
                                    TList4 = tm.TList4,
                                    SelectedItems4 = tm.SelectedItems4,
                                    Col30 = dr["doc_no"].ToString(),
                                    Col31 = dr["doc_date"].ToString(),
                                    Col32 = dr["bal_qty"].ToString(),
                                    Col33 = "0",
                                    Col34 = "-",
                                });
                            }
                        }
                    }
                }
                else if (command == "Callback")
                {
                    model = StartCallback(model);
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
                    string currdate = sgen.server_datetime(userCode);
                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        isedit = true;
                        vch_num = model[0].Col16;
                    }
                    else
                    {
                        isedit = false;
                        mq = "select max(to_number(vch_num)) as maxno from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "maxno");
                        model[0].Col16 = vch_num;
                    }
                    DataTable dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10);
                    decimal cr1 = 0, drr = 0, dr2 = 0;
                    if (model[0].SelectedItems2.FirstOrDefault() == "ADV")
                    {
                        cr1 = model.AsEnumerable().Sum(x => sgen.Make_decimal(x.Col26));
                        drr = model.AsEnumerable().Sum(x => sgen.Make_decimal(x.Col26));
                        //dr2 = model.AsEnumerable().Sum(x => sgen.Make_decimal(x.Col27));
                    }
                    else if (model[0].SelectedItems2.FirstOrDefault() == "OAC")
                    {
                        cr1 = model.AsEnumerable().Sum(x => sgen.Make_decimal(x.Col26));
                        drr = model.AsEnumerable().Sum(x => sgen.Make_decimal(x.Col27));
                        //dr2 = model.AsEnumerable().Sum(x => sgen.Make_decimal(x.Col27));
                    }
                    else
                    {
                        cr1 = model.AsEnumerable().Sum(x => sgen.Make_decimal(x.Col26));
                        drr = model.AsEnumerable().Sum(x => sgen.Make_decimal(x.Col33));
                        dr2 = model.AsEnumerable().Sum(x => sgen.Make_decimal(x.Col27));
                    }
                    if (cr1 != drr + dr2)
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'Cr Amount Should be Equal To Dr Amount ,Please Check', 2);";
                        ModelState.Clear();
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                        return View(model);
                    }
                    string bankname = ((List<SelectListItem>)TempData[MyGuid + "_TList1"]).SingleOrDefault(item => item.Value == model[0].SelectedItems1.FirstOrDefault().ToString()).Text.ToString();
                    string ledgername = ((List<SelectListItem>)TempData[MyGuid + "_TList4"]).SingleOrDefault(item => item.Value == model[0].SelectedItems4.FirstOrDefault().ToString()).Text.ToString();
                    ledgername = ledgername.Trim().Split('-')[0].Trim();
                    //--------bank
                    DataRow dr = dtstr.NewRow();
                    dr["vch_num"] = vch_num;
                    dr["rno"] = "0";
                    dr["type"] = model[0].Col12;
                    dr["vch_date"] = sgen.Make_date_S(model[0].Col17);
                    dr["totremark"] = model[0].Col40;
                    dr["acode"] = model[0].SelectedItems1.FirstOrDefault();
                    dr["adj_type"] = model[0].SelectedItems2.FirstOrDefault();
                    dr["aname"] = bankname;
                    dr["chqno"] = model[0].SelectedItems3.FirstOrDefault();
                    //dr["chqno"] = model[0].Col28;
                    dr["chqdate"] = sgen.Make_date_S(model[0].Col29);
                    dr["chqamt"] = model[0].Col26;
                    dr["reftype"] = "bnk";
                    dr["cramount"] = model[0].Col26;
                    dr["rvscode"] = model[0].SelectedItems4.FirstOrDefault();//party reverse code
                    dr["dramount"] = "0";
                    dr["invno"] = model[0].Col30;
                    dr["remark"] = model[0].Col34;
                    dr["type1"] = "BLKPAY";
                    dr["invdate"] = sgen.Make_date_S(model[0].Col31);
                    dr = getsave(currdate, dr, model);
                    dtstr.Rows.Add(dr);
                    //--------advance
                    dr = dtstr.NewRow();
                    dr["vch_num"] = vch_num;
                    dr["rno"] = "0";
                    dr["type"] = model[0].Col12;
                    dr["vch_date"] = sgen.Make_date_S(model[0].Col17);
                    dr["totremark"] = model[0].Col40;
                    dr["acode"] = model[0].SelectedItems4.FirstOrDefault();
                    dr["aname"] = ledgername;
                    dr["chqno"] = model[0].SelectedItems3.FirstOrDefault();
                    //dr["chqno"] = model[0].Col28;
                    dr["chqdate"] = sgen.Make_date_S(model[0].Col29);
                    dr["adj_type"] = model[0].SelectedItems2.FirstOrDefault();
                    dr["rvscode"] = model[0].SelectedItems1.FirstOrDefault();//bank reverse code
                    dr["chqamt"] = model[0].Col26;
                    dr["reftype"] = "adv";
                    if (model[0].SelectedItems2.FirstOrDefault() == "ADV")
                    {
                        dr["cramount"] = "0";
                        dr["dramount"] = model[0].Col26;
                    }
                    else if (model[0].SelectedItems2.FirstOrDefault() == "OAC")
                    {
                        dr["cramount"] = "0";
                        dr["dramount"] = model[0].Col27;
                    }
                    else
                    {
                        dr["cramount"] = "0";
                        dr["dramount"] = model[0].Col27;
                    }
                    dr["type1"] = "BLKPAY";
                    dr = getsave(currdate, dr, model);
                    dtstr.Rows.Add(dr);
                    //-------------BILL
                    for (int i = 0; i < model.Count; i++)
                    {
                        if (model[i].Col30 != null)
                        {
                            if (!model[i].Col33.Trim().Equals("0") && !model[i].Col33.Trim().Equals(""))
                            {
                                dr = dtstr.NewRow();
                                dr["vch_num"] = vch_num;
                                dr["rno"] = i.ToString();
                                dr["type"] = model[0].Col12;
                                dr["vch_date"] = sgen.Make_date_S(model[0].Col17);
                                dr["invno"] = model[i].Col30;
                                dr["invamt"] = model[i].Col32;
                                dr["remark"] = model[i].Col34;
                                dr["invdate"] = sgen.Make_date_S(model[i].Col31);
                                dr["totremark"] = model[0].Col40;
                                dr["acode"] = model[0].SelectedItems4.FirstOrDefault();
                                dr["aname"] = ledgername;
                                dr["chqno"] = model[0].SelectedItems3.FirstOrDefault();
                                //dr["chqno"] = model[0].Col28;
                                dr["adj_type"] = model[0].SelectedItems2.FirstOrDefault();
                                dr["rvscode"] = model[0].SelectedItems1.FirstOrDefault();//bank reverse code
                                dr["reftype"] = "bill";
                                dr["chqno"] = model[0].Col28;
                                dr["chqamt"] = model[0].Col26;
                                dr["chqdate"] = sgen.Make_date_S(model[0].Col29);
                                dr["cramount"] = "0";
                                dr["dramount"] = model[i].Col33;
                                dr["type1"] = "BLKPAY";
                                dr = getsave(currdate, dr, model);
                                dtstr.Rows.Add(dr);
                            }
                        }
                    }
                    bool Result = sgen.Update_data_fast1(userCode, dtstr, model[0].Col10.Trim(), tm.Col8, isedit);
                    if (Result)
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
                            TList2 = mod2,
                            TList3 = mod3,
                            TList4 = mod4,
                            SelectedItems1 = new string[] { "" },
                            SelectedItems2 = new string[] { "" },
                            SelectedItems3 = new string[] { "" },
                            SelectedItems4 = new string[] { "" },
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
                                model = new_payamt(model);
                                ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                            }
                            catch (Exception ex) { }
                        }
                    }
                    else
                    {
                        ViewBag.scripCall = "showmsgJS(1, 'Record Not Saved', 2);";
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
        #region CASH PAY RECIEVE
        public ActionResult cash_vou()
        {
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            FillMst(MyGuid);
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            if (model[0].Col14.Trim().Equals("22003.1"))
            {
                model[0].Col9 = "CASH RECIEPT";
                model[0].Col12 = "10";
            }
            else if (model[0].Col14.Trim().Equals("22003.2"))
            {
                model[0].Col9 = "CASH PAYMENT";
                model[0].Col12 = "20";
            }
            else
            {
                model[0].Col9 = "CASH TRANSACTION";
                model[0].Col12 = "";
            }
            model[0].Col10 = "vouchers";
            model[0].Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            List<SelectListItem> md1 = new List<SelectListItem>();
            TempData[MyGuid + "_TList1"] = model[0].TList1 = md1;
            TempData[MyGuid + "_TList2"] = model[0].TList2 = md1;
            TempData[MyGuid + "_TList3"] = model[0].TList3 = md1;
            model[0].SelectedItems1 = new string[] { "" };
            model[0].SelectedItems2 = new string[] { "" };
            model[0].SelectedItems3 = new string[] { "" };
            return View(model);
        }
        public List<Tmodelmain> new_cash_vou(List<Tmodelmain> model)
        {
            string year = year_from.Substring(6, 4);
            string currdate = sgen.server_datetime_dt(userCode).ToString("dd/MM/yyyy");
            model = getnew(model);
            var tm = model.FirstOrDefault();
            List<SelectListItem> mod2 = new List<SelectListItem>();
            List<SelectListItem> mod3 = new List<SelectListItem>();
            mod2.Add(new SelectListItem { Text = "Advance", Value = "ADV" });
            mod2.Add(new SelectListItem { Text = "Against Ref.", Value = "AGR" });
            mod2.Add(new SelectListItem { Text = "New Ref.", Value = "NEW" });
            mod2.Add(new SelectListItem { Text = "On Account", Value = "OAC" });
            TempData[MyGuid + "_TList2"] = model[0].TList2 = mod2;
            mq1 = "select a.acode,a.aname||'-'||'('||nvl(p.closing,'0')||')' as ledger_name from " +
                      "(select vch_num as acode,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'BCD' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                      "union all select vch_num as acode ,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                      "union all select cm.vch_num as acode,nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname,cm.CLIENT_UNIT_ID,cm.vch_date,cm.type from master_setting a inner join clients_mst cm on " +
                      "a.master_id = cm.ptype and a.type = 'ABD' and find_in_set(a.client_unit_id, cm.client_unit_id)=1 where cm.type = 'BAD' and find_in_set(cm.client_unit_id,'" + unitid_mst + "')=1) a " +
                      "LEFT JOIN (SELECT ACODE,CLIENT_UNIT_ID,SUM(OPBAL) AS OPBAL,SUM(DRAMOUNT),SUM(CRAMOUNT),SUM(OPBAL)+SUM(DRAMOUNT)-SUM(CRAMOUNT) as closing from " +
                      "(select acode,CLIENT_UNIT_ID, OP_" + year + " as opbal,0 as cramount,0 as dramount,0 as cl from acbal union all select acode,CLIENT_UNIT_ID,sum(dramount)-sum(cramount) as opbal" +
",0 as cramount,0 as dramount,0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and " +
                        "to_date('" + year_from.Split(' ')[0].Trim() + "','" + sgen.Getsqldateformat() + "')-1 group by acode,CLIENT_UNIT_ID union all select acode,CLIENT_UNIT_ID,0 as opbal,TO_NUMBER(cramount) cramount,TO_NUMBER(dramount) dramount,0 cl from vouchers " +
                        "where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                        "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + currdate.Trim() + "','" + sgen.Getsqldateformat() + "')) group by acode,CLIENT_UNIT_ID) p on p.acode=a.acode and p.client_unit_id=a.client_unit_id where find_in_set(a.client_unit_id, '" + unitid_mst + "')=1";
            DataTable dtemp = new DataTable();
            dtemp = sgen.getdata(userCode, mq1);
            if (dtemp.Rows.Count > 0)
            {
                foreach (DataRow item in dtemp.Rows)
                {
                    mod3.Add(new SelectListItem { Text = item["ledger_name"].ToString(), Value = item["acode"].ToString() });
                }
            }
            TempData[MyGuid + "_TList3"] = model[0].TList3 = mod3;
            model[0].SelectedItems2 = new string[] { "" };
            model[0].SelectedItems3 = new string[] { "" };
            model[0].Col17 = sgen.server_datetime_local(userCode);
            return model;
        }
        [HttpPost]
        public ActionResult cash_vou(List<Tmodelmain> model, string command, string mid)
        {
            try
            {
                FillMst(model[0].Col15);
                var tm = model.FirstOrDefault();
                List<SelectListItem> mod1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
                List<SelectListItem> mod2 = (List<SelectListItem>)TempData[MyGuid + "_TList2"];
                List<SelectListItem> mod3 = (List<SelectListItem>)TempData[MyGuid + "_TList3"];
                TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
                TempData[MyGuid + "_TList2"] = model[0].TList2 = mod2;
                TempData[MyGuid + "_TList3"] = model[0].TList3 = mod3;
                if (model[0].SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
                if (model[0].SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
                if (model[0].SelectedItems3 == null) model[0].SelectedItems3 = new string[] { "" };
                if (command == "New")
                {
                    model = new_cash_vou(model);
                }
                else if (command == "Cancel")
                {
                    return CancelFun(model);
                }
                else if (command == "Get Data")
                {
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vsavenew = "";
                    if (model[0].SelectedItems3.FirstOrDefault() != "")
                    {
                        DataTable dtques = new DataTable();
                        if (model[0].Col14 == "22003.1")
                        {
                            if (model[0].SelectedItems2.FirstOrDefault() == "NEW")
                            {
                                mq = "select (i.client_id||i.client_unit_id||i.doc_no||to_char(i.doc_date, 'yyyymmdd')|| i.type) as fstr,i.doc_no,to_char(i.doc_date, 'dd/MM/YYYY') as doc_date" +
                    ",i.amt,i.type,to_number(i.amt) - to_number(nvl(v.bal, '0')) as bal_qty from (select i.acode, i.vch_num as doc_no,max(i.vch_date) doc_date,sum(i.gtaxamt) as amt" +
                    ", i.client_id, i.client_unit_id, i.type from purchasesc i where i.pur_type = 'NPI' and substr(i.type, 1, 1) = '4' and to_number(i.gtaxamt) > 0 and client_unit_id='" + unitid_mst + "' " +
                    "group by i.acode, i.vch_num, i.client_id,i.client_unit_id, i.type) i left join (select sum(cramount) as bal,invno,acode,client_unit_id,invdate from VOUCHERS v " +
                    "where v.TYPE in ('11','10') AND v.REFTYPE = 'bill' group by invno,acode,client_unit_id,invdate ) v on v.invno = i.doc_no and v.acode = i.acode and v.client_unit_id = " +
                    "i.client_unit_id  and to_char(i.doc_date,'yyyyMMdd') = to_char(v.invdate,'yyyyMMdd') where i.acode = '" + model[0].SelectedItems3.FirstOrDefault() + "'";
                            }
                            else
                            {
                                mq = "select (i.client_id||i.client_unit_id||i.doc_no||to_char(i.doc_date, 'yyyymmdd')|| i.type) as fstr,i.doc_no,to_char(i.doc_date, 'dd/MM/YYYY') as doc_date" +
                              ",i.amt,i.type,to_number(i.amt) - to_number(nvl(v.bal, '0')) as bal_qty from (select i.acode, i.vch_num as doc_no,max(i.vch_date) doc_date,sum(i.netamt) as amt" +
                              ", i.client_id, i.client_unit_id, i.type from itransaction i where i.type = '40' and substr(i.potype, 1, 1) = '5' and to_number(i.netamt) > 0 and client_unit_id='" + unitid_mst + "' " +
                              "group by i.acode, i.vch_num, i.client_id,i.client_unit_id, i.type) i left join (select sum(cramount) as bal,invno,acode,client_unit_id,invdate from VOUCHERS v " +
                              "where v.TYPE in ('11','10') AND v.REFTYPE = 'bill' group by invno,acode,client_unit_id,invdate ) v on v.invno = i.doc_no and v.acode = i.acode and v.client_unit_id = " +
                              "i.client_unit_id  and to_char(i.doc_date,'yyyyMMdd') = to_char(v.invdate,'yyyyMMdd') where i.acode = '" + model[0].SelectedItems3.FirstOrDefault() + "'";
                            }
                        }
                        else
                        {
                            if (model[0].SelectedItems2.FirstOrDefault() == "NEW")
                            {
                                mq = "select (i.client_id||i.client_unit_id||i.doc_no||to_char(i.doc_date, 'yyyymmdd')|| i.type) as fstr,i.doc_no,to_char(i.doc_date, 'dd/MM/YYYY') as doc_date" +
                         ",i.amt,i.type,to_number(i.amt) - to_number(nvl(v.bal, '0')) as bal_qty from (select i.acode, i.vch_num as doc_no,max(i.vch_date) doc_date,sum(i.netamt) as amt" +
                         ", i.client_id, i.client_unit_id, i.type from purchases i where i.pur_type in ('001','002') and substr(i.type, 1, 1) = '5' and to_number(i.netamt) > 0 and client_unit_id='" + unitid_mst + "' " +
                         "group by i.acode, i.vch_num, i.client_id,i.client_unit_id, i.type) i left join (select sum(dramount) as bal,invno,acode,client_unit_id,invdate from VOUCHERS v " +
                         "where v.TYPE in ('21','20') AND v.REFTYPE = 'bill' group by invno,acode,client_unit_id,invdate ) v on v.invno = i.doc_no and v.acode = i.acode and v.client_unit_id = " +
                         "i.client_unit_id  and to_char(i.doc_date,'yyyyMMdd') = to_char(v.invdate,'yyyyMMdd') where i.acode = '" + model[0].SelectedItems3.FirstOrDefault() + "'";
                            }
                            else
                            {
                                mq = "select (i.client_id||i.client_unit_id||i.doc_no||to_char(i.doc_date, 'yyyymmdd')|| i.type) as fstr,i.doc_no,to_char(i.doc_date, 'dd/MM/YYYY') as doc_date" +
                            ",i.amt,i.type,to_number(i.amt) - to_number(nvl(v.bal, '0')) as bal_qty from (select i.acode, i.vch_num as doc_no,max(i.vch_date) doc_date,sum(i.netamt) as amt" +
                            ", i.client_id, i.client_unit_id, i.type from itransaction i where i.type in ('01','02','03','05','07','08','09') and to_number(i.netamt) > 0 and client_unit_id='" + unitid_mst + "' " +
                            "group by i.acode, i.vch_num, i.client_id,i.client_unit_id, i.type) i left join (select sum(dramount) as bal,invno,acode,client_unit_id,invdate from VOUCHERS v " +
                            "where v.TYPE in ('21','20') AND v.REFTYPE = 'bill' group by invno,acode,client_unit_id,invdate ) v on v.invno = i.doc_no and v.acode = i.acode and v.client_unit_id = " +
                            "i.client_unit_id  and to_char(i.doc_date,'yyyyMMdd') = to_char(v.invdate,'yyyyMMdd') where i.acode = '" + model[0].SelectedItems3.FirstOrDefault() + "'";

                            }
                        }
                        dtques = sgen.getdata(userCode, mq);
                        if (dtques.Rows.Count > 0)
                        {
                            model = new List<Tmodelmain>();
                            foreach (DataRow dr in dtques.Rows)
                            {
                                model.Add(new Tmodelmain
                                {
                                    Col8 = tm.Col8,
                                    Col9 = tm.Col9,
                                    Col11 = tm.Col11,
                                    Col12 = tm.Col12,
                                    Col10 = tm.Col10,
                                    Col13 = tm.Col13,
                                    Col14 = tm.Col14,
                                    Col15 = tm.Col15,
                                    Col16 = tm.Col16,
                                    Col17 = tm.Col17,
                                    Col18 = tm.Col18,
                                    Col19 = tm.Col19,
                                    Col20 = tm.Col20,
                                    Col21 = tm.Col21,
                                    Col22 = tm.Col22,
                                    Col23 = tm.Col23,
                                    Col24 = tm.Col24,
                                    Col42 = tm.Col42,
                                    Col25 = tm.Col25,
                                    Col26 = tm.Col26,
                                    Col27 = tm.Col27,
                                    Col40 = tm.Col40,
                                    Col28 = tm.Col28,
                                    Col29 = tm.Col29,
                                    Col100 = tm.Col100,
                                    Col121 = tm.Col121,
                                    Col122 = tm.Col122,
                                    Col123 = tm.Col123,
                                    TList2 = tm.TList2,
                                    SelectedItems2 = tm.SelectedItems2,
                                    TList3 = tm.TList3,
                                    SelectedItems3 = tm.SelectedItems3,
                                    Col30 = dr["doc_no"].ToString(),
                                    Col31 = dr["doc_date"].ToString(),
                                    Col32 = dr["bal_qty"].ToString(),
                                    Col33 = "0",
                                    Col34 = "-",
                                });
                            }
                        }
                    }
                }
                else if (command == "Callback")
                {
                    model = StartCallback(model);
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
                    string currdate = sgen.server_datetime(userCode);
                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        isedit = true;
                        vch_num = model[0].Col16;
                    }
                    else
                    {
                        isedit = false;
                        mq = "select max(to_number(vch_num)) as maxno from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "maxno");
                        model[0].Col16 = vch_num;
                    }
                    DataTable dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10);
                    decimal cr1 = 0, drr = 0, dr2 = 0;
                    if (model[0].SelectedItems2.FirstOrDefault() == "ADV")
                    {
                        cr1 = model.AsEnumerable().Sum(x => sgen.Make_decimal(x.Col26));
                        drr = model.AsEnumerable().Sum(x => sgen.Make_decimal(x.Col26));
                    }
                    else if (model[0].SelectedItems2.FirstOrDefault() == "OAC")
                    {
                        cr1 = model.AsEnumerable().Sum(x => sgen.Make_decimal(x.Col26));
                        drr = model.AsEnumerable().Sum(x => sgen.Make_decimal(x.Col27));
                    }
                    else
                    {
                        cr1 = model.AsEnumerable().Sum(x => sgen.Make_decimal(x.Col26));
                        drr = model.AsEnumerable().Sum(x => sgen.Make_decimal(x.Col33));
                        dr2 = model.AsEnumerable().Sum(x => sgen.Make_decimal(x.Col27));
                    }
                    if (cr1 != drr + dr2)
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'Cr Amount Should be Equal To Dr Amount ,Please Check', 2);";
                        ModelState.Clear();
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                        return View(model);
                    }
                    string ledgername = ((List<SelectListItem>)TempData[MyGuid + "_TList3"]).SingleOrDefault(item => item.Value == model[0].SelectedItems3.FirstOrDefault().ToString()).Text.ToString();
                    ledgername = ledgername.Trim().Split('-')[0].Trim();
                    DataRow dr = dtstr.NewRow();
                    //--------cash
                    dr["vch_num"] = vch_num;
                    dr["rno"] = "0";
                    dr["type"] = model[0].Col12;
                    dr["vch_date"] = sgen.Make_date_S(model[0].Col17);
                    dr["totremark"] = model[0].Col40;
                    dr["acode"] = "001";
                    dr["aname"] = "CASH";
                    dr["chqamt"] = model[0].Col26;
                    dr["reftype"] = "cash";
                    dr["adj_type"] = model[0].SelectedItems2.FirstOrDefault();
                    if (model[0].Col14 == "22003.1")
                    {

                        dr["cramount"] = "0";
                        dr["dramount"] = model[0].Col26;
                    }
                    else
                    {
                        dr["cramount"] = model[0].Col26;
                        dr["dramount"] = "0";
                    }
                    dr["type1"] = "CASHRCPT";
                    dr["adj_type"] = model[0].SelectedItems2.FirstOrDefault();
                    dr["rvscode"] = model[0].SelectedItems3.FirstOrDefault();//party reverse code
                    dr["invno"] = model[0].Col30;
                    dr["remark"] = model[0].Col34;
                    dr["invdate"] = sgen.Make_date_S(model[0].Col31);
                    dr = getsave(currdate, dr, model);
                    dtstr.Rows.Add(dr);
                    //--------advance
                    dr = dtstr.NewRow();
                    dr["vch_num"] = vch_num;
                    dr["rno"] = "0";
                    dr["type"] = model[0].Col12;
                    dr["vch_date"] = sgen.Make_date_S(model[0].Col17);
                    dr["totremark"] = model[0].Col40;
                    dr["acode"] = model[0].SelectedItems3.FirstOrDefault();
                    dr["aname"] = ledgername;
                    dr["rvscode"] = "001";
                    dr["chqamt"] = model[0].Col26;
                    dr["reftype"] = "adv";
                    dr["adj_type"] = model[0].SelectedItems2.FirstOrDefault();
                    if (model[0].Col14 == "22003.1")
                    {
                        if (model[0].SelectedItems2.FirstOrDefault() == "ADV")
                        {
                            dr["cramount"] = "0";
                            dr["cramount"] = model[0].Col26;
                        }
                        else if (model[0].SelectedItems2.FirstOrDefault() == "OAC")
                        {
                            dr["cramount"] = "0";
                            dr["cramount"] = model[0].Col27;
                        }
                        else
                        {
                            dr["cramount"] = "0";
                            dr["cramount"] = model[0].Col27;
                        }
                    }
                    else
                    {
                        if (model[0].SelectedItems2.FirstOrDefault() == "ADV")
                        {
                            dr["cramount"] = "0";
                            dr["dramount"] = model[0].Col26;
                        }
                        else if (model[0].SelectedItems2.FirstOrDefault() == "OAC")
                        {
                            dr["cramount"] = "0";
                            dr["dramount"] = model[0].Col27;
                        }
                        else
                        {
                            dr["cramount"] = "0";
                            dr["dramount"] = model[0].Col27;
                        }
                    }
                    dr["type1"] = "CASHRCPT";
                    dr = getsave(currdate, dr, model);
                    dtstr.Rows.Add(dr);
                    //-------------BILL
                    for (int i = 0; i < model.Count; i++)
                    {
                        if (model[i].Col30 != null)
                        {
                            if (!model[i].Col33.Trim().Equals("0") && !model[i].Col33.Trim().Equals(""))
                            {
                                dr = dtstr.NewRow();
                                dr["vch_num"] = vch_num;
                                dr["rno"] = i.ToString();
                                dr["type"] = model[0].Col12;
                                dr["vch_date"] = sgen.Make_date_S(model[0].Col17);
                                dr["invno"] = model[i].Col30;
                                dr["invamt"] = model[i].Col32;
                                dr["remark"] = model[i].Col34;
                                dr["invdate"] = sgen.Make_date_S(model[i].Col31);
                                dr["totremark"] = model[0].Col40;
                                dr["acode"] = model[0].SelectedItems3.FirstOrDefault();
                                dr["aname"] = ledgername;
                                dr["rvscode"] = "001";
                                dr["reftype"] = "bill";
                                dr["chqamt"] = model[0].Col26;
                                dr["adj_type"] = model[0].SelectedItems2.FirstOrDefault();
                                if (model[0].Col14 == "22003.1")
                                {
                                    dr["cramount"] = model[i].Col33;
                                    dr["dramount"] = "0";
                                }
                                else
                                {
                                    dr["cramount"] = "0";
                                    dr["dramount"] = model[i].Col33;
                                }
                                dr["type1"] = "CASHRCPT";
                                dr = getsave(currdate, dr, model);
                                dtstr.Rows.Add(dr);
                            }
                        }
                    }
                    bool Result = sgen.Update_data_fast1(userCode, dtstr, model[0].Col10.Trim(), tm.Col8, isedit);
                    if (Result)
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
                            TList2 = mod2,
                            TList3 = mod3,
                            SelectedItems1 = new string[] { "" },
                            SelectedItems2 = new string[] { "" },
                            SelectedItems3 = new string[] { "" },
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
                                model = new_cash_vou(model);
                                ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                            }
                            catch (Exception ex) { }
                        }
                    }
                    else
                    {
                        ViewBag.scripCall = "showmsgJS(1, 'Record Not Saved', 2);";
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
        #region tally_group_mapping
        public ActionResult tly_gmap()
        {
            FillMst();
            chkRef();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<SelectListItem> mod2 = new List<SelectListItem>();
            model[0].Col10 = "enx_tab";
            model[0].Col9 = "GROUP MAPPING";
            model[0].Col12 = "TGM";
            model[0].Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            model[0].SelectedItems1 = new string[] { "" };
            model[0].SelectedItems2 = new string[] { "" };
            TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
            TempData[MyGuid + "_TList2"] = model[0].TList2 = mod2;
            return View(model);
        }
        public List<Tmodelmain> new_tly_gmap(List<Tmodelmain> model)
        {
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<SelectListItem> mod2 = new List<SelectListItem>();
            model = getnew(model);
            mq = "select a.* from (select '01' fstr,'DEBTORS' master_nm from dual union all select '02' fstr,'CREDITORS' master_nm from dual" +
                " union all select '03' fstr,'BANK' master_nm from dual union all select '04' fstr,'CASH' master_nm from dual" +
                " union all select '05' fstr,'SALE' master_nm from dual union all select '06' fstr,'PURCHASE' master_nm from dual) a where a.fstr not in (select col4 from enx_tab where type='TGM' and client_unit_id='" + unitid_mst + "')";
            DataTable dt = sgen.getdata(userCode, mq);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    mod1.Add(new SelectListItem { Text = item["master_nm"].ToString(), Value = item["fstr"].ToString() });
                }
            }
            TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
            mq1 = "select a.vch_num as acode,a.c_name as aname from tally_clients_mst a where a.type='GRP' and a.vch_num not in (select a.vch_num as acode from tally_clients_mst a inner join (select col2 from enx_tab where type = 'TGM' and client_unit_id = '" + unitid_mst + "') b on find_in_set(b.col2, a.vch_num) = 1 where a.type = 'GRP')";
            dt = sgen.getdata(userCode, mq1);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    mod2.Add(new SelectListItem { Text = item["aname"].ToString(), Value = item["acode"].ToString() });
                }
            }
            TempData[MyGuid + "_TList2"] = model[0].TList2 = mod2;
            return model;
        }
        [HttpPost]
        public ActionResult tly_gmap(List<Tmodelmain> model, string command)
        {
            try
            {
                FillMst(model[0].Col15);
                var tmodel = model.FirstOrDefault();
                #region ddl
                List<SelectListItem> mod1 = new List<SelectListItem>();
                model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
                TempData[MyGuid + "_TList1"] = model[0].TList1;
                if (tmodel.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
                List<SelectListItem> mod2 = new List<SelectListItem>();
                model[0].TList2 = (List<SelectListItem>)TempData[MyGuid + "_TList2"];
                TempData[MyGuid + "_TList2"] = model[0].TList2;
                if (tmodel.SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
                #endregion
                DataTable dt = new DataTable();
                DataTable dataTable = new DataTable();
                if (command == "New")
                {
                    model = new_tly_gmap(model);
                }
                if (command == "Cancel") { return CancelFun(model); }
                else if (command == "Callback") { model = StartCallback(model); }
                else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
                {
                    try
                    {
                        string currdate = sgen.server_datetime(userCode);

                        if (model[0].Col133 == "N")
                        {
                            ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for save data , please contact your admin', 2);";
                            return View(model);
                        }
                        if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                        {
                            mq1 = " and vch_num<>'" + model[0].Col16.Trim() + "'";
                            isedit = true;
                            vch_num = model[0].Col16;
                        }
                        else
                        {
                            mq1 = "";
                            isedit = false;
                            mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                            vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                        }
                        DataTable dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                        //string groupname = ((List<SelectListItem>)TempData[MyGuid + "_TList2"]).SingleOrDefault(item => item.Value == model[0].SelectedItems2.FirstOrDefault().ToString()).Text.ToString();
                        string dgroupname = ((List<SelectListItem>)TempData[MyGuid + "_TList1"]).SingleOrDefault(item => item.Value == model[0].SelectedItems1.FirstOrDefault().ToString()).Text.ToString();
                        DataRow dr = dtstr.NewRow();
                        dr["type"] = model[0].Col12;
                        dr["vch_num"] = vch_num.Trim();
                        dr["vch_date"] = currdate;
                        dr["col4"] = model[0].SelectedItems1.FirstOrDefault();//dashboard grp
                        dr["col2"] = string.Join(",", model[0].SelectedItems2);//tly grp    
                        dr["col3"] = dgroupname;//tly grp    
                        dr = getsave(currdate, dr, model);
                        dtstr.Rows.Add(dr);
                        bool res = false;
                        res = sgen.Update_data_fast1(userCode, dtstr, model[0].Col10.Trim(), model[0].Col8, isedit);
                        if (res)
                        {
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
                                    model = new_tly_gmap(model);
                                    ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                                }
                                catch (Exception ex) { }
                            }
                            else
                            {
                                //sat.Rollback();
                                ViewBag.vnew = "disabled='disabled'";
                                ViewBag.vedit = "disabled='disabled'";
                                ViewBag.vsave = "";
                                ViewBag.vopbal = "";
                                ViewBag.vsavenew = "";
                                ViewBag.scripCall += "showmsgJS(1, 'Data Not Saved', 0);";
                            }
                        }
                    }
                    catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString().Replace('\'', ' ') + "', 0);"; }
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
        #region tally_company_mapping
        public ActionResult tly_cmap()
        {
            FillMst();
            chkRef();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<SelectListItem> mod2 = new List<SelectListItem>();
            model[0].Col10 = "enx_tab";
            model[0].Col9 = "TALLY COMPANY MAPPING";
            model[0].Col12 = "TCM";
            model[0].Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            model[0].SelectedItems1 = new string[] { "" };
            model[0].SelectedItems2 = new string[] { "" };
            TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
            TempData[MyGuid + "_TList2"] = model[0].TList2 = mod2;
            return View(model);
        }
        public List<Tmodelmain> new_tly_cmap(List<Tmodelmain> model)
        {
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<SelectListItem> mod2 = new List<SelectListItem>();
            model = getnew(model);
            mq = "select a.vch_num as comid,a.company_name from company_profile a where a.type='CP' and a.vch_num not in (select col2 from enx_tab where type='TCM' and client_unit_id='" + unitid_mst + "') order by vch_num";
            DataTable dt = sgen.getdata(userCode, mq);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    mod1.Add(new SelectListItem { Text = item["company_name"].ToString(), Value = item["comid"].ToString() });
                }
            }
            TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
            mq1 = "select a.vch_num,a.col1 from enx_tab a where a.type='TCP' and a.vch_num not in (select col4 from enx_tab where type = 'TCM' and client_unit_id = '" + unitid_mst + "') order by vch_num ";
            dt = sgen.getdata(userCode, mq1);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    mod2.Add(new SelectListItem { Text = item["col1"].ToString(), Value = item["vch_num"].ToString() });
                }
            }
            TempData[MyGuid + "_TList2"] = model[0].TList2 = mod2;
            return model;
        }
        [HttpPost]
        public ActionResult tly_cmap(List<Tmodelmain> model, string command)
        {
            try
            {
                FillMst(model[0].Col15);
                var tmodel = model.FirstOrDefault();
                #region ddl
                List<SelectListItem> mod1 = new List<SelectListItem>();
                model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
                TempData[MyGuid + "_TList1"] = model[0].TList1;
                if (tmodel.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
                List<SelectListItem> mod2 = new List<SelectListItem>();
                model[0].TList2 = (List<SelectListItem>)TempData[MyGuid + "_TList2"];
                TempData[MyGuid + "_TList2"] = model[0].TList2;
                if (tmodel.SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
                #endregion
                DataTable dt = new DataTable();
                DataTable dataTable = new DataTable();
                if (command == "New")
                {
                    model = new_tly_cmap(model);
                }
                if (command == "Cancel") { return CancelFun(model); }
                else if (command == "Callback") { model = StartCallback(model); }
                else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
                {
                    try
                    {
                        string currdate = sgen.server_datetime(userCode);
                        if (model[0].Col133 == "N")
                        {
                            ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for save data , please contact your admin', 2);";
                            return View(model);
                        }
                        if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                        {
                            mq1 = " and vch_num<>'" + model[0].Col16.Trim() + "'";
                            isedit = true;
                            vch_num = model[0].Col16;
                        }
                        else
                        {
                            mq1 = "";
                            isedit = false;
                            mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                            vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                        }
                        Satransaction sat = new Satransaction(userCode, MyGuid);
                        DataTable dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                        //string groupname = ((List<SelectListItem>)TempData[MyGuid + "_TList2"]).SingleOrDefault(item => item.Value == model[0].SelectedItems2.FirstOrDefault().ToString()).Text.ToString();
                        string MYcname = ((List<SelectListItem>)TempData[MyGuid + "_TList1"]).SingleOrDefault(item => item.Value == model[0].SelectedItems1.FirstOrDefault().ToString()).Text.ToString();
                        string TALLYcname = ((List<SelectListItem>)TempData[MyGuid + "_TList2"]).SingleOrDefault(item => item.Value == model[0].SelectedItems2.FirstOrDefault().ToString()).Text.ToString();
                        DataRow dr = dtstr.NewRow();
                        dr["type"] = model[0].Col12;
                        dr["vch_num"] = vch_num.Trim();
                        dr["vch_date"] = currdate;
                        dr["col2"] = model[0].SelectedItems1.FirstOrDefault();//MY COMPANY
                        dr["col4"] = model[0].SelectedItems2.FirstOrDefault();//tly COMPANY    
                        dr["col1"] = MYcname;//COMNAME    
                        dr["col3"] = TALLYcname;//tly CNAME    
                        dr = getsave(currdate, dr, model);
                        dtstr.Rows.Add(dr);
                        bool res = false;
                        res = sgen.Update_data_fast1_uncommit(userCode, dtstr, model[0].Col10.Trim(), model[0].Col8, isedit, sat);
                        if (res)
                        {
                            mq = "update COMPANY_PROFILE set DATABASE_NAME = '" + TALLYcname + "' where vch_num = '" + model[0].SelectedItems1.FirstOrDefault() + "' and type = 'CP'";
                            bool ok = sat.Execute_cmd(mq);
                            if (ok)
                            {
                                sat.Commit();
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
                                        model = new_tly_cmap(model);
                                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                                    }
                                    catch (Exception ex) { }
                                }
                            }
                            else
                            {
                                sat.Rollback();
                                ViewBag.vnew = "disabled='disabled'";
                                ViewBag.vedit = "disabled='disabled'";
                                ViewBag.vsave = "";
                                ViewBag.vopbal = "";
                                ViewBag.vsavenew = "";
                                ViewBag.scripCall += "showmsgJS(1, 'Data Not Saved', 0);";
                            }
                        }
                        else
                        {
                            sat.Rollback();
                            ViewBag.vnew = "disabled='disabled'";
                            ViewBag.vedit = "disabled='disabled'";
                            ViewBag.vsave = "";
                            ViewBag.vopbal = "";
                            ViewBag.vsavenew = "";
                            ViewBag.scripCall += "showmsgJS(1, 'Data Not Saved', 0);";
                        }
                    }
                    catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString().Replace('\'', ' ') + "', 0);"; }
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
        #region tallyDash
        public ActionResult tallyDash()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "TALLY DASHBOARD";
            model[0].Col10 = "tally_vouchers";
            model[0].Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            model[0].Col12 = "";
            //string parentid = "000253";

            //mq = "select a.* from (select '01' fstr,'DEBTORS' master_nm from dual union all select '02' fstr,'CREDITORS' master_nm from dual" +
            //    " union all select '03' fstr,'BANK' master_nm from dual union all select '04' fstr,'CASH' master_nm from dual" +
            //    " union all select '05' fstr,'SALE' master_nm from dual union all select '06' fstr,'PURCHASE' master_nm from dual) a where a.fstr not in (select col4 from enx_tab where type='TGM' and client_unit_id='" + unitid_mst + "')";
            mq = "select col2,col3,col4 from enx_tab where type='TGM'";
            DataTable dt = new DataTable();
            dt = sgen.getdata(userCode, mq);
            if (dt.Rows.Count > 0)
            {
                try { model[0].Col22 = dt.Select("col4='01'")[0].ItemArray[0].ToString(); }
                catch (Exception ex) { model[0].Col22 = ""; }
                try { model[0].Col23 = dt.Select("col4='02'")[0].ItemArray[0].ToString(); }
                catch (Exception ex) { model[0].Col23 = ""; }
                try { model[0].Col24 = dt.Select("col4='03'")[0].ItemArray[0].ToString(); }
                catch (Exception ex) { model[0].Col24 = ""; }
                try { model[0].Col25 = dt.Select("col4='04'")[0].ItemArray[0].ToString(); }
                catch (Exception ex) { model[0].Col25 = ""; }
                try { model[0].Col26 = dt.Select("col4='05'")[0].ItemArray[0].ToString(); }
                catch (Exception ex) { model[0].Col26 = ""; }
                try { model[0].Col27 = dt.Select("col4='06'")[0].ItemArray[0].ToString(); }
                catch (Exception ex) { model[0].Col27 = ""; }
            }

            //mq = "select sum(dramount) totamt from " + model[0].Col10 + " where upper(reftype)='SALES'";
            //mq = "select sum(dramount) totamt from " + model[0].Col10 + "";
            mq = "select sum(a.closing-to_number(b.opbal)) closing from " +
               "(SELECT client_unit_id,vch_date, ACODE, SUM(OPBAL) OPBAL, SUM(DRAMOUNT) dramt, SUM(CRAMOUNT) cramt, SUM(OPBAL) + SUM(DRAMOUNT) - SUM(CRAMOUNT) closing from " +
               "(select client_unit_id,vch_date, acode, 0 as opbal, TO_NUMBER(cramount) cramount, TO_NUMBER(dramount) dramount, 0 cl from tally_vouchers) group by ACODE, vch_date,client_unit_id) a " +
               "inner join tally_clients_mst b on a.acode = b.vch_num and find_in_set_one('" + model[0].Col26 + "',b.parent)=1";
            model[0].Col16 = sgen.seekval(userCode, mq, "closing");

            //mq = "select sum(dramount) totamt from " + model[0].Col10 + " where upper(reftype)='PURCHASE'";
            //mq = "select sum(dramount) totamt from " + model[0].Col10 + "";
            mq = "select sum(a.closing-to_number(b.opbal)) closing from " +
               "(SELECT client_unit_id,vch_date, ACODE, SUM(OPBAL) OPBAL, SUM(DRAMOUNT) dramt, SUM(CRAMOUNT) cramt, SUM(OPBAL) + SUM(DRAMOUNT) - SUM(CRAMOUNT) closing from " +
               "(select client_unit_id,vch_date, acode, 0 as opbal, TO_NUMBER(cramount) cramount, TO_NUMBER(dramount) dramount, 0 cl from tally_vouchers) group by ACODE, vch_date,client_unit_id) a " +
               "inner join tally_clients_mst b on a.acode = b.vch_num and find_in_set_one('" + model[0].Col27 + "',b.parent)=1";
            model[0].Col17 = sgen.seekval(userCode, mq, "closing");

            mq = "select sum(a.closing-to_number(b.opbal)) closing from " +
               "(SELECT vch_date, ACODE, SUM(OPBAL) OPBAL, SUM(DRAMOUNT) dramt, SUM(CRAMOUNT) cramt, SUM(OPBAL) + SUM(DRAMOUNT) - SUM(CRAMOUNT) closing from " +
               "(select vch_date, acode, 0 as opbal, TO_NUMBER(cramount) cramount, TO_NUMBER(dramount) dramount, 0 cl from tally_vouchers) group by ACODE, vch_date) a " +
               "inner join tally_clients_mst b on a.acode = b.vch_num and find_in_set_one('" + model[0].Col22 + "',b.parent)=1";
            model[0].Col18 = sgen.seekval(userCode, mq, "closing");

            mq = "select sum(a.closing-to_number(b.opbal)) closing from " +
               "(SELECT vch_date, ACODE, SUM(OPBAL) OPBAL, SUM(DRAMOUNT) dramt, SUM(CRAMOUNT) cramt, SUM(OPBAL) + SUM(DRAMOUNT) - SUM(CRAMOUNT) closing from " +
               "(select vch_date, acode, 0 as opbal, TO_NUMBER(cramount) cramount, TO_NUMBER(dramount) dramount, 0 cl from tally_vouchers) group by ACODE, vch_date) a " +
               "inner join tally_clients_mst b on a.acode = b.vch_num and find_in_set_one('" + model[0].Col23 + "',b.parent)=1";
            model[0].Col19 = sgen.seekval(userCode, mq, "closing");

            mq = "select sum(a.closing-to_number(b.opbal)) closing from " +
               "(SELECT vch_date, ACODE, SUM(OPBAL) OPBAL, SUM(DRAMOUNT) dramt, SUM(CRAMOUNT) cramt, SUM(OPBAL) + SUM(DRAMOUNT) - SUM(CRAMOUNT) closing from " +
               "(select vch_date, acode, 0 as opbal, TO_NUMBER(cramount) cramount, TO_NUMBER(dramount) dramount, 0 cl from tally_vouchers) group by ACODE, vch_date) a " +
               "inner join tally_clients_mst b on a.acode = b.vch_num and find_in_set_one('" + model[0].Col24 + "',b.parent)=1";
            model[0].Col20 = sgen.seekval(userCode, mq, "closing");

            mq = "select sum(a.closing-to_number(b.opbal)) closing from " +
               "(SELECT vch_date, ACODE, SUM(OPBAL) OPBAL, SUM(DRAMOUNT) dramt, SUM(CRAMOUNT) cramt, SUM(OPBAL) + SUM(DRAMOUNT) - SUM(CRAMOUNT) closing from " +
               "(select vch_date, acode, 0 as opbal, TO_NUMBER(cramount) cramount, TO_NUMBER(dramount) dramount, 0 cl from tally_vouchers) group by ACODE, vch_date) a " +
               "inner join tally_clients_mst b on a.acode = b.vch_num and find_in_set_one('" + model[0].Col25 + "',b.parent)=1";
            model[0].Col21 = sgen.seekval(userCode, mq, "closing");

            return View(model);
        }
        #endregion
        #region purBook
        public ActionResult purBook()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            ViewBag.scripCall = "enableForm();";
            model[0].Col9 = "PURCHASE BOOK";
            model[0].Col10 = "tally_vouchers";
            model[0].Col11 = " and v.client_unit_id = '" + unitid_mst + "'";
            model[0].Col12 = "06";
            model[0].Col16 = "001";
            model[0].Col19 = EncryptDecrypt.Decrypt(Request.QueryString["tid"].ToString());
            //string parentid = "000253";

            //mq = "select (v.client_unit_id||to_char(v.vch_date,'ddmmyyyy')) Action,to_char(v.vch_date,'" + sgen.Getsqldatetimeformat() + "') vchdate,sum(v.dramount) amount from tally_vouchers v " +
            //            //"where upper(v.reftype)= '" + model[0].Col12 + "'" + model[0].Col11 + " group by v.vch_date";
            //            "where " + model[0].Col11.Replace("and", "").Trim() + " group by (v.client_unit_id||to_char(v.vch_date,'ddmmyyyy')),v.vch_date";

            //mq1 = "select (v.client_unit_id||c.vch_num) Action,c.c_name partyname,sum(v.dramount) amount from tally_vouchers v " +
            //    "inner join tally_clients_mst c on c.vch_num = v.acode and c.type = 'BCD' and c.client_unit_id=v.client_unit_id " +
            //    //"where upper(v.reftype)= '" + model[0].Col12 + "'" + model[0].Col11 + " group by c.c_name";
            //    "where " + model[0].Col11.Replace("and", "").Trim() + " group by (v.client_unit_id||c.vch_num),c.c_name";

            mq = "select (a.client_unit_id||to_char(a.vch_date,'ddmmyyyy')) Action,to_char(a.vch_date,'dd/mm/yyyy') VchDate,sum(a.closing-to_number(b.opbal)) closing from " +
                "(SELECT client_unit_id,vch_date, ACODE, SUM(OPBAL) OPBAL, SUM(DRAMOUNT) dramt, SUM(CRAMOUNT) cramt, SUM(OPBAL) + SUM(DRAMOUNT) - SUM(CRAMOUNT) closing from " +
                "(select client_unit_id,vch_date, acode, 0 as opbal, TO_NUMBER(cramount) cramount, TO_NUMBER(dramount) dramount, 0 cl from tally_vouchers) group by ACODE, vch_date,client_unit_id) a " +
                "inner join tally_clients_mst b on a.acode = b.vch_num and find_in_set_one('" + model[0].Col19 + "',b.parent)=1 group by a.vch_date,a.client_unit_id";

            mq1 = "select (a.client_unit_id||a.ACODE) action,b.c_name name,a.closing-to_number(b.opbal) closing from " +
                "(SELECT client_unit_id,ACODE,SUM(OPBAL) OPBAL,SUM(DRAMOUNT) dramt,SUM(CRAMOUNT) cramt,SUM(OPBAL)+SUM(DRAMOUNT)-SUM(CRAMOUNT) closing from " +
                "(select client_unit_id,acode, 0 as opbal,TO_NUMBER(cramount) cramount,TO_NUMBER(dramount) dramount, 0 cl from tally_vouchers) group by ACODE,client_unit_id) a " +
                "inner join tally_clients_mst b on a.acode = b.vch_num and find_in_set_one('" + model[0].Col19 + "',b.parent)=1";

            sgen.SetSession(MyGuid, "gridq_demogrid", mq);
            sgen.SetSession(MyGuid, "gridq_grd2", mq1);

            return View(model);
        }
        [HttpPost]
        public ActionResult purBook(List<Tmodelmain> model, string command)
        {
            try
            {
                FillMst();
                if (command.Trim().ToUpper() == "GETDATA")
                {
                    string yrf = model[0].Col17;
                    string yrt = model[0].Col18;
                    string parentid = model[0].Col19;
                    //string parentid = "000253";

                    //mq = "select (v.client_unit_id||to_char(v.vch_date,'ddmmyyyy')) Action,to_char(v.vch_date,'" + sgen.Getsqldatetimeformat() + "') vchdate,sum(v.dramount) amount from tally_vouchers v " +
                    ////"where upper(v.reftype)= '" + model[0].Col12 + "'" + model[0].Col11 + " group by v.vch_date";
                    //"where " + model[0].Col11.Replace("and", "").Trim() + " and to_date(to_char(v.vch_DATE,'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldateformat() + "') between " +
                    //"to_date('" + yrf + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + yrt + "','" + sgen.Getsqldatetimeformat() + "') " +
                    //"group by (v.client_unit_id||to_char(v.vch_date,'ddmmyyyy')),v.vch_date";

                    mq = "select (a.client_unit_id||to_char(a.vch_date,'ddmmyyyy')) Action,to_char(a.vch_date,'dd/mm/yyyy') VchDate,sum(a.closing-to_number(b.opbal)) closing from " +
             "(SELECT client_unit_id,vch_date, ACODE, SUM(OPBAL) OPBAL, SUM(DRAMOUNT) dramt, SUM(CRAMOUNT) cramt, SUM(OPBAL) + SUM(DRAMOUNT) - SUM(CRAMOUNT) closing from " +
             "(select client_unit_id,vch_date, acode, 0 as opbal, TO_NUMBER(cramount) cramount, TO_NUMBER(dramount) dramount, 0 cl from tally_vouchers) group by ACODE,vch_date,client_unit_id) a " +
             "inner join tally_clients_mst b on a.acode = b.vch_num and find_in_set_one('" + parentid + "',b.parent)=1 " +
             "where to_date(to_char(a.vch_date,'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldateformat() + "') between " +
             "to_date('" + yrf + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + yrt + "','" + sgen.Getsqldatetimeformat() + "') group by a.vch_date,a.client_unit_id";

                    //mq1 = "select (v.client_unit_id||c.vch_num) Action,c.c_name partyname,sum(v.dramount) amount from tally_vouchers v " +
                    //"inner join tally_clients_mst c on c.vch_num = v.acode and c.type = 'BCD' and c.client_unit_id=v.client_unit_id " +
                    ////"where upper(v.reftype)= '" + model[0].Col12 + "'" + model[0].Col11 + " group by c.c_name";
                    //"where " + model[0].Col11.Replace("and", "").Trim() + " and to_date(to_char(v.vch_DATE,'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldateformat() + "') between " +
                    //"to_date('" + yrf + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + yrt + "','" + sgen.Getsqldatetimeformat() + "') " +
                    //"group by (v.client_unit_id||c.vch_num),c.c_name";

                    mq1 = "select (a.client_unit_id||a.ACODE) action,b.c_name name,a.closing-to_number(b.opbal) as closing from " +
                        "(SELECT client_unit_id,ACODE,SUM(OPBAL) OPBAL,SUM(DRAMOUNT) dramt,SUM(CRAMOUNT) cramt,SUM(OPBAL)+SUM(DRAMOUNT)-SUM(CRAMOUNT) closing from " +
                        "(select client_unit_id,acode, 0 as opbal,TO_NUMBER(cramount) cramount,TO_NUMBER(dramount) dramount, 0 cl from tally_vouchers) group by ACODE,client_unit_id) a " +
                        "inner join tally_clients_mst b on a.acode = b.vch_num and find_in_set_one('" + parentid + "',b.parent)=1 " +
                        "where to_date(to_char(a.vch_date,'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldateformat() + "') between " +
                        "to_date('" + yrf + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + yrt + "','" + sgen.Getsqldatetimeformat() + "')";

                    sgen.SetSession(MyGuid, "gridq_demogrid", mq);
                    sgen.SetSession(MyGuid, "gridq_grd2", mq1);
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
        #region salesBook
        public ActionResult salesBook()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            ViewBag.scripCall = "enableForm();";
            model[0].Col9 = "SALES BOOK";
            string postedfrom = "";
            postedfrom = Request.QueryString["tid"].ToString();
            if (postedfrom.Trim() == "TACC")
            {
                model[0].Col10 = "tally_vouchers";
                model[0].Col11 = " and v.client_unit_id = '" + unitid_mst + "'";
                model[0].Col12 = "05";
                model[0].Col16 = "001";
                model[0].Col19 = EncryptDecrypt.Decrypt(Request.QueryString["tid"].ToString());
                //string parentid = "000253";

                //mq = "select (v.client_unit_id||to_char(v.vch_date,'ddmmyyyy')) Action,to_char(v.vch_date,'"+ sgen.Getsqldatetimeformat() + "') vchdate,sum(v.dramount) amount from tally_vouchers v " +
                //    //"where upper(v.reftype)= '" + model[0].Col12 + "'" + model[0].Col11 + " group by v.vch_date";
                //    "where " + model[0].Col11.Replace("and", "").Trim() + " group by (v.client_unit_id||to_char(v.vch_date,'ddmmyyyy')),v.vch_date";

                //mq1 = "select (v.client_unit_id||c.vch_num) Action,c.c_name partyname,sum(v.dramount) amount from tally_vouchers v " +
                //    "inner join tally_clients_mst c on c.vch_num = v.acode and c.type = 'BCD' and c.client_unit_id=v.client_unit_id " +
                //    //"where upper(v.reftype)= '" + model[0].Col12 + "'" + model[0].Col11 + " group by c.c_name";
                //    "where " + model[0].Col11.Replace("and", "").Trim() + " group by (v.client_unit_id||c.vch_num),c.c_name";

                mq = "select (a.client_unit_id||to_char(a.vch_date,'ddmmyyyy')) Action,to_char(a.vch_date,'dd/mm/yyyy') VchDate,sum(a.closing-to_number(b.opbal)) closing from " +
                  "(SELECT client_unit_id,vch_date, ACODE, SUM(OPBAL) OPBAL, SUM(DRAMOUNT) dramt, SUM(CRAMOUNT) cramt, SUM(OPBAL) + SUM(DRAMOUNT) - SUM(CRAMOUNT) closing from " +
                  "(select client_unit_id,vch_date, acode, 0 as opbal, TO_NUMBER(cramount) cramount, TO_NUMBER(dramount) dramount, 0 cl from tally_vouchers) group by ACODE, vch_date,client_unit_id) a " +
                  "inner join tally_clients_mst b on a.acode = b.vch_num and find_in_set_one('" + model[0].Col19 + "',b.parent)=1 group by a.vch_date,a.client_unit_id";

                mq1 = "select (a.client_unit_id||a.ACODE) action,b.c_name name,a.closing-to_number(b.opbal) as closing from " +
                    "(SELECT client_unit_id,ACODE,SUM(OPBAL) OPBAL,SUM(DRAMOUNT) dramt,SUM(CRAMOUNT) cramt,SUM(OPBAL)+SUM(DRAMOUNT)-SUM(CRAMOUNT) closing from " +
                    "(select client_unit_id,acode, 0 as opbal,TO_NUMBER(cramount) cramount,TO_NUMBER(dramount) dramount, 0 cl from tally_vouchers) group by ACODE,client_unit_id) a " +
                    "inner join tally_clients_mst b on a.acode = b.vch_num and find_in_set_one('" + model[0].Col19 + "',b.parent)=1";
            }
            else if (postedfrom.Trim() == "SACC")
            {
                model[0].Col10 = "tally_vouchers";
                model[0].Col11 = " and v.client_unit_id = '" + unitid_mst + "'";
                model[0].Col12 = "5";
                model[0].Col16 = "001";
                model[0].Col19 = EncryptDecrypt.Decrypt(Request.QueryString["tid"].ToString());

                mq = "select (a.client_unit_id||to_char(a.vch_date,'ddmmyyyy')) Action,to_char(a.vch_date,'dd/mm/yyyy') VchDate,sum(a.closing-to_number(b.opbal)) closing from " +
                  "(SELECT client_unit_id,vch_date, ACODE, SUM(OPBAL) OPBAL, SUM(DRAMOUNT) dramt, SUM(CRAMOUNT) cramt, SUM(OPBAL) + SUM(DRAMOUNT) - SUM(CRAMOUNT) closing from " +
                  "(select client_unit_id,vch_date, acode, 0 as opbal, TO_NUMBER(cramount) cramount, TO_NUMBER(dramount) dramount, 0 cl from tally_vouchers) group by ACODE, vch_date,client_unit_id) a " +
                  "inner join tally_clients_mst b on a.acode = b.vch_num and find_in_set_one('" + model[0].Col19 + "',b.parent)=1 group by a.vch_date,a.client_unit_id";

                mq1 = "select (a.client_unit_id||a.ACODE) action,b.c_name name,a.closing-to_number(b.opbal) as closing from " +
                    "(SELECT client_unit_id,ACODE,SUM(OPBAL) OPBAL,SUM(DRAMOUNT) dramt,SUM(CRAMOUNT) cramt,SUM(OPBAL)+SUM(DRAMOUNT)-SUM(CRAMOUNT) closing from " +
                    "(select client_unit_id,acode, 0 as opbal,TO_NUMBER(cramount) cramount,TO_NUMBER(dramount) dramount, 0 cl from tally_vouchers) group by ACODE,client_unit_id) a " +
                    "inner join tally_clients_mst b on a.acode = b.vch_num and find_in_set_one('" + model[0].Col19 + "',b.parent)=1";

            }
            sgen.SetSession(MyGuid, "gridq_demogrid", mq);
            sgen.SetSession(MyGuid, "gridq_grd2", mq1);

            return View(model);
        }
        [HttpPost]
        public ActionResult salesBook(List<Tmodelmain> model, string command)
        {
            try
            {
                FillMst();
                if (command.Trim().ToUpper() == "GETDATA")
                {
                    string yrf = model[0].Col17;
                    string yrt = model[0].Col18;
                    string parentid = model[0].Col19;
                    //string parentid = "000253";

                    //mq = "select (v.client_unit_id||to_char(v.vch_date,'ddmmyyyy')) Action,to_char(v.vch_date,'" + sgen.Getsqldatetimeformat() + "') vchdate,sum(v.dramount) amount from tally_vouchers v " +
                    ////"where upper(v.reftype)= '" + model[0].Col12 + "'" + model[0].Col11 + " group by v.vch_date";
                    //"where " + model[0].Col11.Replace("and", "").Trim() + " and to_date(to_char(v.vch_DATE,'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldateformat() + "') between " +
                    //"to_date('" + yrf + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + yrt + "','" + sgen.Getsqldatetimeformat() + "') " +
                    //"group by (v.client_unit_id||to_char(v.vch_date,'ddmmyyyy')),v.vch_date";

                    mq = "select (a.client_unit_id||to_char(a.vch_date,'ddmmyyyy')) Action,to_char(a.vch_date,'dd/mm/yyyy') VchDate,sum(a.closing-to_number(b.opbal)) closing from " +
             "(SELECT client_unit_id,vch_date, ACODE, SUM(OPBAL) OPBAL, SUM(DRAMOUNT) dramt, SUM(CRAMOUNT) cramt, SUM(OPBAL) + SUM(DRAMOUNT) - SUM(CRAMOUNT) closing from " +
             "(select client_unit_id,vch_date, acode, 0 as opbal, TO_NUMBER(cramount) cramount, TO_NUMBER(dramount) dramount, 0 cl from tally_vouchers) group by ACODE,vch_date,client_unit_id) a " +
             "inner join tally_clients_mst b on a.acode = b.vch_num and find_in_set_one('" + parentid + "',b.parent)=1 " +
             "where to_date(to_char(a.vch_date,'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldateformat() + "') between " +
             "to_date('" + yrf + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + yrt + "','" + sgen.Getsqldatetimeformat() + "') group by a.vch_date,a.client_unit_id";

                    //mq1 = "select (v.client_unit_id||c.vch_num) Action,c.c_name partyname,sum(v.dramount) amount from tally_vouchers v " +
                    //"inner join tally_clients_mst c on c.vch_num = v.acode and c.type = 'BCD' and c.client_unit_id=v.client_unit_id " +
                    ////"where upper(v.reftype)= '" + model[0].Col12 + "'" + model[0].Col11 + " group by c.c_name";
                    //"where " + model[0].Col11.Replace("and", "").Trim() + " and to_date(to_char(v.vch_DATE,'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldateformat() + "') between " +
                    //"to_date('" + yrf + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + yrt + "','" + sgen.Getsqldatetimeformat() + "') " +
                    //"group by (v.client_unit_id||c.vch_num),c.c_name";

                    mq1 = "select (a.client_unit_id||a.ACODE) action,b.c_name name,a.closing-to_number(b.opbal) as closing from " +
                        "(SELECT client_unit_id,ACODE,SUM(OPBAL) OPBAL,SUM(DRAMOUNT) dramt,SUM(CRAMOUNT) cramt,SUM(OPBAL)+SUM(DRAMOUNT)-SUM(CRAMOUNT) closing from " +
                        "(select client_unit_id,acode, 0 as opbal,TO_NUMBER(cramount) cramount,TO_NUMBER(dramount) dramount, 0 cl from tally_vouchers) group by ACODE,client_unit_id) a " +
                        "inner join tally_clients_mst b on a.acode = b.vch_num and find_in_set_one('" + parentid + "',b.parent)=1 " +
                        "where to_date(to_char(a.vch_date,'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldateformat() + "') between " +
                        "to_date('" + yrf + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + yrt + "','" + sgen.Getsqldatetimeformat() + "')";

                    sgen.SetSession(MyGuid, "gridq_demogrid", mq);
                    sgen.SetSession(MyGuid, "gridq_grd2", mq1);
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
        public ContentResult getdetail(string val, string vchnum)
        {
            DataSet ds = new DataSet();
            try
            {
                if (val.Trim() != null && vchnum.Trim() != null)
                {
                    string parentid = val.Trim().Split('~')[2].Trim();
                    if (val.Trim().Split('~')[0].Trim() == "05" || val.Trim().Split('~')[0].Trim() == "06")
                    {
                        if (val.Trim().Split('~')[1].Trim() == "001")
                        {
                            //mq = "select to_char(a.vch_date,'dd/mm/yyyy') Vchdate,a.dramount Amount from tally_vouchers a " +
                            //    "inner join tally_clients_mst c on c.vch_num = a.acode and find_in_set_one('000253',c.parent)= 1 " +
                            //    "where (a.client_unit_id||to_char(a.vch_date,'ddmmyyyy')) = '" + vchnum + "'";

                            //mq = "select 'OP' DocNo,a.created_date DocDate,-to_number(a.opbal) DrAmount,'0' as CrAmount " +
                            //    "from tally_clients_mst a where find_in_set_one('" + parentid + "', a.parent) = 1 and(a.client_unit_id || to_char(a.vch_date, 'ddmmyyyy')) = '" + vchnum + "' " +
                            //    "union all " +
                            //    "select a.client_unit_id,a.vch_num,a.vch_date,to_number(a.dramount),a.cramount Amount from tally_vouchers a " +
                            //    "inner join tally_clients_mst c on c.vch_num = a.acode and find_in_set_one('" + parentid + "', c.parent) = 1 " +
                            //    "where(a.client_unit_id || to_char(a.vch_date, 'ddmmyyyy')) = '" + vchnum + "'";

                            mq = "select a.vch_num DocNo,to_char(a.vch_date,'dd/mm/yyyy') DocDate,to_number(a.dramount) DrAmount,a.cramount CrAmount from tally_vouchers a " +
                            "inner join tally_clients_mst c on c.vch_num = a.acode and find_in_set_one('" + parentid + "', c.parent) = 1 " +
                            "where(a.client_unit_id || to_char(a.vch_date, 'ddmmyyyy')) = '" + vchnum + "'";
                        }
                        else
                        {
                            //mq = "select c.c_name Party,a.dramount Amount from tally_vouchers a " +
                            //    "inner join tally_clients_mst c on c.vch_num = a.acode and find_in_set_one('000253',c.parent)= 1 " +
                            //    "where (a.client_unit_id||a.ACODE) = '" + vchnum + "'";

                            mq = "select a.vch_num DocNo,to_char(a.vch_date,'dd/mm/yyyy') DocDate,c.c_name as Party,to_number(a.dramount) DrAmount,a.cramount CrAmount from tally_vouchers a " +
                                "inner join tally_clients_mst c on c.vch_num = a.acode and find_in_set_one('" + parentid + "',c.parent)= 1 " +
                                "where(a.client_unit_id || a.ACODE) = '" + vchnum + "'";
                        }
                    }
                    else if (val.Trim().Split('~')[0].Trim() == "01" || val.Trim().Split('~')[0].Trim() == "02" || val.Trim().Split('~')[0].Trim() == "03" || val.Trim().Split('~')[0].Trim() == "04")
                    {
                        //mq = "select c.c_name Party,a.dramount Amount from tally_vouchers a " +
                        //      "inner join tally_clients_mst c on c.vch_num = a.acode and find_in_set_one('000253',c.parent)= 1 " +
                        //      "where (a.client_unit_id||a.ACODE) = '" + vchnum + "'";

                        mq = "select 'OP' DocNo,to_char(a.created_date,'dd/mm/yyyy') DocDate,a.c_name as Party,-to_number(a.opbal) DrAmount,'0' as CrAmount from tally_clients_mst a " +
                        "where find_in_set_one('" + parentid + "',a.parent)= 1 and (a.client_unit_id || a.vch_num) = '" + vchnum + "' union all " +
                        "select a.vch_num DocNo,to_char(a.vch_date,'dd/mm/yyyy') DocDate,c.c_name as Party,to_number(a.dramount) DrAmount,a.cramount CrAmount from tally_vouchers a " +
                        "inner join tally_clients_mst c on c.vch_num = a.acode and find_in_set_one('" + parentid + "',c.parent)= 1 " +
                        "where(a.client_unit_id || a.ACODE) = '" + vchnum + "'";
                    }

                    DataTable dt = sgen.getdata(userCode, mq);
                    if (dt.Rows.Count > 0)
                    {
                        dt.TableName = "DetailData";
                        ds.Tables.Add(dt);
                        return Content(ds.GetXml());
                    }
                    else return Content("N");

                }
                else { return Content("N"); }
            }
            catch (Exception err) { return Content("N"); }
        }
        #endregion
        #region Creditors
        public ActionResult Creditors()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "CREDITORS";
            model[0].Col10 = "tally_vouchers";
            model[0].Col11 = " and client_unit_id = '" + unitid_mst + "'";
            model[0].Col12 = "02";
            model[0].Col19 = EncryptDecrypt.Decrypt(Request.QueryString["tid"].ToString());
            //string parentid = "000253";

            mq = "select (a.client_unit_id||a.ACODE) action,b.c_name name,a.closing-to_number(b.opbal) as closing from " +
                "(SELECT client_unit_id,ACODE,SUM(OPBAL) OPBAL,SUM(DRAMOUNT) dramt,SUM(CRAMOUNT) cramt,SUM(OPBAL)+SUM(DRAMOUNT)-SUM(CRAMOUNT) closing from " +
                "(select client_unit_id,acode, 0 as opbal,TO_NUMBER(cramount) cramount,TO_NUMBER(dramount) dramount, 0 cl from tally_vouchers) group by ACODE,client_unit_id) a " +
                "inner join tally_clients_mst b on a.acode = b.vch_num and find_in_set_one('" + model[0].Col19 + "',b.parent)=1";
            sgen.SetSession(MyGuid, "gridq_demogrid", mq);
            return View(model);
        }
        #endregion
        #region Debtors
        public ActionResult Debtors()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "DEBTORS";
            model[0].Col10 = "vouchers";
            model[0].Col11 = " and client_unit_id = '" + unitid_mst + "'";
            model[0].Col12 = "01";
            model[0].Col19 = EncryptDecrypt.Decrypt(Request.QueryString["tid"].ToString());
            //string parentid = "000253";

            mq = "select (a.client_unit_id||a.ACODE) action,b.c_name name,a.closing-to_number(b.opbal) as closing from " +
                "(SELECT client_unit_id,ACODE,SUM(OPBAL) OPBAL,SUM(DRAMOUNT) dramt,SUM(CRAMOUNT) cramt,SUM(OPBAL)+SUM(DRAMOUNT)-SUM(CRAMOUNT) closing from " +
                "(select client_unit_id,acode, 0 as opbal,TO_NUMBER(cramount) cramount, TO_NUMBER(dramount) dramount, 0 cl from tally_vouchers) group by ACODE,client_unit_id) a " +
                "inner join tally_clients_mst b on a.acode = b.vch_num and find_in_set_one('" + model[0].Col19 + "',b.parent)=1";
            sgen.SetSession(MyGuid, "gridq_demogrid", mq);
            return View(model);
        }
        #endregion
        #region cashBal
        public ActionResult cashBal()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "CASH BALANCE";
            model[0].Col10 = "vouchers";
            model[0].Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            model[0].Col12 = "04";
            model[0].Col19 = EncryptDecrypt.Decrypt(Request.QueryString["tid"].ToString());
            //string parentid = "000253";
            mq = "select (a.client_unit_id||a.ACODE) action,b.c_name name,a.closing-to_number(b.opbal) as closing from " +
                  "(SELECT client_unit_id,ACODE,SUM(OPBAL) OPBAL,SUM(DRAMOUNT) dramt,SUM(CRAMOUNT) cramt,SUM(OPBAL)+SUM(DRAMOUNT)-SUM(CRAMOUNT) closing from " +
                  "(select client_unit_id,acode, 0 as opbal,TO_NUMBER(cramount) cramount, TO_NUMBER(dramount) dramount, 0 cl from tally_vouchers) group by ACODE,client_unit_id) a " +
                  "inner join tally_clients_mst b on a.acode = b.vch_num and find_in_set_one('" + model[0].Col19 + "',b.parent)=1";
            sgen.SetSession(MyGuid, "gridq_demogrid", mq);

            return View(model);
        }
        #endregion
        #region bankBal
        public ActionResult bankBal()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "BANK BALANCE";
            model[0].Col10 = "vouchers";
            model[0].Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            model[0].Col12 = "03";
            model[0].Col19 = EncryptDecrypt.Decrypt(Request.QueryString["tid"].ToString());
            //string parentid = "000253";

            mq = "select (a.client_unit_id||a.ACODE) action,b.c_name name,a.closing-to_number(b.opbal) as closing from " +
               "(SELECT client_unit_id,ACODE,SUM(OPBAL) OPBAL,SUM(DRAMOUNT) dramt,SUM(CRAMOUNT) cramt,SUM(OPBAL)+SUM(DRAMOUNT)-SUM(CRAMOUNT) closing from " +
               "(select client_unit_id,acode, 0 as opbal,TO_NUMBER(cramount) cramount, TO_NUMBER(dramount) dramount, 0 cl from tally_vouchers) group by ACODE,client_unit_id) a " +
               "inner join tally_clients_mst b on a.acode = b.vch_num and find_in_set_one('" + model[0].Col19 + "',b.parent)=1";
            sgen.SetSession(MyGuid, "gridq_demogrid", mq);

            return View(model);
        }
        #endregion
        #region accReps
        public ActionResult accReps()
        {
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            FillMst(MyGuid);
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            chkRef();
            List<Tmodelmain> model = new List<Tmodelmain>();
            Tmodelmain tm1 = new Tmodelmain();
            tm1.Col9 = "ACCOUNT REPORTS";
            tm1.Col10 = "tally_vouchers";
            tm1.Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            tm1.Col12 = "";
            tm1.Col13 = "";
            tm1.Col100 = "";
            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = MyGuid.Trim();
            model.Add(tm1);
            return View(model);
        }
        [HttpPost]
        public ActionResult accReps(List<Tmodelmain> model, string command)
        {
            try
            {
                string fdt = "", tdt = "", title = "", btnval = "";
                int seektype = 0;
                FillMst(model[0].Col15);
                var tm = model.FirstOrDefault();
                DataTable dt = new DataTable();
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
                catch (Exception err) { }
                string date_period = "from " + fdt + " to " + tdt + "";
                switch (command.Trim())
                {
                    case "Stock Summary":
                        cmd = "select (nvl(t.client_unit_id,'0')||i.icode) as fstr,i.icode Item_Code,gd.master_name Maingrp,mg.master_name Itemgrp,sg.iname Itemsubgrp," +
                            "i.iname Item_Name,i.cpartno PartNo,um.master_name as UOM,sum(t.qtyin) as Received,sum(t.qtyout) Issued," +
                            "sum(t.qtyin) - sum(t.qtyout) Closing,sum(nvl(t.pktno, 0)) pktno,round(case when sum(t.qtyin)- sum(t.qtyout) = 0 then 0 else " +
                            "(sum(t.iamount) / (sum(t.qtyin) - sum(t.qtyout))) end) Rate,sum(t.iamount) Amount from item i " +
                            "left join (select vch_date,client_id, client_unit_id, icode, iname, to_number(nvl(qtyin,0)) qtyin,to_number(nvl(qtyout, 0)) qtyout," +
                            "(case when to_number(nvl(qtyin, 0)) = 0 then - to_number(nvl(iamount, 0)) else to_number(nvl(iamount, 0)) end) iamount," +
                            "(case when substr(type, 1, 1) in ('0', '1') then pktno when type = '36' then pktno when substr(type,1,1) in ('2', '3', '4') then - pktno else pktno end) pktno " +
                            "from itransaction where length(icode) > 5 and store = 'Y') t on t.icode = i.icode and t.client_unit_id = i.client_unit_id " +
                            "and to_date(to_char(t.vch_date,'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldateformat() + "') between TO_DATE('" + fdt + "', '" + sgen.Getsqldateformat() + "') AND TO_DATE('" + tdt + "','" + sgen.Getsqldateformat() + "') " +
                            "inner join master_setting gd on gd.classid = SUBSTR(i.icode, 1, 1) and gd.type = 'KGP' " +
                            "inner join master_setting mg on mg.master_id = substr(i.icode, 1, 3) and mg.type = 'KIG' and mg.client_unit_id = i.client_unit_id " +
                            "inner join item sg on sg.icode = substr(i.icode, 1, 5) and sg.type = 'SG' and sg.client_unit_id = i.client_unit_id " +
                            "inner join master_setting um on um.master_id = i.uom and um.type = 'UMM' and find_in_set(um.client_unit_id, i.client_unit_id)= 1 " +
                            "where length(i.icode)> 5 and find_in_set(i.client_unit_id, '" + unitid_mst + "')= 1 " +
                            "group by nvl(t.client_unit_id, '0'),i.icode,gd.master_name,mg.master_name,sg.iname,i.iname,um.master_name,i.cpartno order by i.icode";
                        title = "Stock Summary " + date_period + "";
                        if (command.Trim().Equals("Stock MReport"))
                        {
                            btnval = "STKMM";
                            seektype = 2;
                        }
                        else
                        {
                            btnval = "ISUM";
                            seektype = 1;
                        }
                        sgen.SetSession(MyGuid, "btnval", btnval);
                        break;
                    case "Callback":
                        btnval = sgen.GetSession(MyGuid, "btnval").ToString();
                        model = StartCallback(model, btnval);
                        break;
                }
                if (command != "Callback")
                {
                    sgen.open_grid(title, cmd, seektype);
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
        #region balSheet
        public ActionResult balSheet(string fdt, string tdt)
        {
            FillMst();
            fdt = year_from.Split(' ')[0].Trim(); tdt = year_to.Split(' ')[0].Trim();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            ViewBag.scripCall = "enableForm();";
            model[0].Col9 = "BALANCE SHEET";
            model[0].Col10 = "vouchers";
            model[0].Col11 = " and v.client_unit_id = '" + unitid_mst + "'";
            model[0].Col12 = "05";

            var dtlib = new DataTable();
            var dtass = new DataTable();
            var dtpl = new DataTable();
            var dtmain = new DataTable();

            mq = "select (m.master_id) lbfstr,m.master_name lbname,sum(v.closing) lbcl from master_setting m " +
    "inner join (select client_unit_id, acode, sum(op) op, sum(dramount) dramount, sum(cramount) cramount, (sum(op) + sum(dramount) - sum(cramount)) closing from " +
    "(select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, OP_" + Ac_Year.Split(' ')[0].Trim() + " as op, 0 as dramount, 0 as cramount," +
    " 0 as cl from acbal " +
    "union all " +
    "select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, sum(to_number(dramount)) - sum(to_number(cramount)) as op, 0 as dramount," +
    "0 as cramount, 0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
    "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') - 1 group by client_unit_id, acode " +
    "union all " +
    "select client_unit_id, acode as acode, vch_num, vch_date, 0 as op, to_number(dramount) dramount, to_number(cramount) cramount, 0 as cl from vouchers " +
    "where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') " +
    "and to_date('" + tdt + "', '" + sgen.Getsqldateformat() + "')) t group by client_unit_id, acode) v on substr(m.master_id,1,1)=substr(v.acode,1,1) " +
    "where m.type = 'AGM' and trim(substr(m.master_id,1,1)) in ('1', '2') and trim(substr(master_type,2,7))='000000' group by m.master_name,m.master_id order by m.master_id asc";
            dtlib = sgen.getdata(userCode, mq);

            //(v.client_unit_id || m.master_id || m.type) asfstr,
            mq = "select (m.master_id) asfstr,m.master_name asname,sum(v.closing) ascl from master_setting m " +
            "inner join (select client_unit_id, acode, sum(op) op, sum(dramount) dramount, sum(cramount) cramount, (sum(op) + sum(dramount) - sum(cramount)) closing from " +
            "(select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, OP_" + Ac_Year.Split(' ')[0].Trim() + " as op, 0 as dramount, 0 as cramount," +
            " 0 as cl from acbal " +
            "union all " +
            "select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, sum(to_number(dramount)) - sum(to_number(cramount)) as op, 0 as dramount," +
            "0 as cramount, 0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
            "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') - 1 group by client_unit_id, acode " +
            "union all " +
            "select client_unit_id, acode as acode, vch_num, vch_date, 0 as op, to_number(dramount) dramount, to_number(cramount) cramount, 0 as cl from vouchers " +
            "where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') " +
            "and to_date('" + tdt + "', '" + sgen.Getsqldateformat() + "')) t group by client_unit_id, acode) v on substr(m.master_id,1,1)=substr(v.acode,1,1) " +
            "where m.type = 'AGM' and trim(substr(m.master_id,1,1)) in ('3') and trim(substr(master_type,2,7))='000000' group by m.master_name,m.master_id";
            dtass = sgen.getdata(userCode, mq);

            mq = "select '4.5.6' as plfstr,'Profit and Loss' as plname, sum(income)-sum(expn) as plcl from( select (case when substr(acode,1,1)='4' then closing " +
            "else 0 end) as income,(case when substr(acode, 1, 1) in ('5', '6') then closing else 0 end) as expn from " +
            "(select client_unit_id, acode, sum(op) op, sum(dramount) dramount, sum(cramount) cramount, (sum(op) + sum(dramount) - sum(cramount)) closing from " +
            "(select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, OP_" + Ac_Year.Split(' ')[0].Trim() + " as op, 0 as dramount, 0 as cramount, 0 as cl " +
            "from acbal " +
            "union all " +
            "select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, sum(to_number(dramount)) - sum(to_number(cramount)) as op," +
            "0 as dramount, 0 as cramount, 0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') " +
            "between to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') - 1 group by client_unit_id, acode " +
            "union all " +
            "select client_unit_id, acode as acode, vch_num, vch_date, 0 as op, to_number(dramount) dramount, to_number(cramount) cramount, 0 as cl " +
            "from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
            "to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') and to_date('" + tdt + "', '" + sgen.Getsqldateformat() + "')) t group by client_unit_id, acode) t )t";
            dtpl = sgen.getdata(userCode, mq);
            model[0].dt1 = dtlib;
            model[0].dt2 = dtass;
            model[0].dt3 = dtpl;
            model[0].Col16 = model[0].dt1.AsEnumerable().Sum(x => sgen.Make_decimal(x[2].ToString())).ToString();
            model[0].Col17 = model[0].dt2.AsEnumerable().Sum(x => sgen.Make_decimal(x[2].ToString())).ToString();
            model[0].Col18 = model[0].dt3.AsEnumerable().Sum(x => sgen.Make_decimal(x[2].ToString())).ToString();
            return View(model);
        }
        [HttpPost]
        public ActionResult balSheet(List<Tmodelmain> model, string command)
        {
            string fdt = year_from.Split(' ')[0].Trim(); string tdt = year_to.Split(' ')[0].Trim();
            FillMst();
            sgen.SetSession(MyGuid, "KPDFDT", fdt);
            sgen.SetSession(MyGuid, "KPDTDT", tdt);
            string title = "";
            int seektype = 0;
            try
            {
                if (command == "Callback2")
                {
                    var mlvl = sgen.Make_int(Multiton.GetSession(MyGuid, "MAXLEVEL").ToString());
                    var lvl = sgen.Make_int(Multiton.GetSession(MyGuid, "LEVEL").ToString()) + 2;
                    if (mlvl == lvl)
                    {
                        string param1 = "";
                        if (sgen.GetSession(MyGuid, "fstr0th") != null) param1 = sgen.GetSession(MyGuid, "fstr0th").ToString();
                        cmd = myquery(3, param1, fdt + "!~!~!~!~!" + tdt, " and (t.client_unit_id || t.acode) = '" + unitid_mst + "" + param1.Trim() + "'");
                        sgen.open_grid("Balance Sheet", cmd, 1);
                        ViewBag.scripCall = "callFoo('Balance Sheet');";
                        Multiton.SetSession(MyGuid, "LEVEL", 2);
                        Multiton.SetSession(MyGuid, "MAXLEVEL", 3);
                        goto End;
                    }
                }
                else if(command == "Back")
                {
                    try
                    {
                        return RedirectToAction("gst_rpt", "Account", new { @m_id = EncryptDecrypt.Encrypt(model[0].Col15), @mid = EncryptDecrypt.Encrypt(model[0].Col14) });
                    }
                    catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "',0);"; }
                }
                #region callback
                else if (command == "Callback")
                {
                    try
                    {
                        string cond = "";
                        try
                        {
                            cond = sgen.GetSession(MyGuid, "qucond").ToString();
                        }
                        catch (Exception ex) { }
                        int level = sgen.Make_int(Multiton.GetSession(MyGuid, "LEVEL").ToString());
                        string param1 = sgen.GetSession(MyGuid, "SSEEKVAL").ToString();
                        string param2 = fdt + "!~!~!~!~!" + tdt;
                        if (level == 1)
                        {
                            if (level == 1) sgen.SetSession(MyGuid, "l1fstr", param1);
                            String vouurl = param1;
                            return RedirectToAction("cas_recn", new { @m_id = EncryptDecrypt.Encrypt(model[0].Col15), @mid = EncryptDecrypt.Encrypt(model[0].Col14), @vid = EncryptDecrypt.Encrypt(vouurl) });
                        }
                        else if (level == 0) { }
                        else
                        {
                            seektype = 1;
                            sgen.SetSession(MyGuid, "btnval", btnval);

                            cmd = myquery(level, param1, param2, cond);
                            sgen.open_grid(title, cmd, seektype);

                            if (level == 2) sgen.SetSession(MyGuid, "l2fstr", param1);
                            else if (level == 3) sgen.SetSession(MyGuid, "l3fstr", param1);
                            else if (level == 4) sgen.SetSession(MyGuid, "l4fstr", param1);
                            else if (level == 5) sgen.SetSession(MyGuid, "l5fstr", param1);
                            ViewBag.scripCall = "callFoo3('Balance Sheet Detailed');";
                        }
                        Multiton.SetSession(MyGuid, "LEVEL", (level - 1).ToString());
                    }
                    catch (Exception ex) { }
                }
                #endregion
                #region callback2
                if (command == "Callback2")
                {

                    try
                    {
                        string cond = "";
                        try
                        {
                            cond = sgen.GetSession(MyGuid, "qucond").ToString();
                        }
                        catch (Exception ex) { }
                        int level = sgen.Make_int(Multiton.GetSession(MyGuid, "LEVEL").ToString()) + 2;
                        string param1 = sgen.GetSession(MyGuid, "SSEEKVAL").ToString();
                        string param2 = fdt + "!~!~!~!~!" + tdt;
                        seektype = 1;
                        sgen.SetSession(MyGuid, "btnval", btnval);
                        string pfstr = "";
                        if (level == 1) pfstr = sgen.GetSession(MyGuid, "l1fstr").ToString();
                        if (level == 2) pfstr = sgen.GetSession(MyGuid, "l2fstr").ToString();
                        if (level == 3) pfstr = sgen.GetSession(MyGuid, "l3fstr").ToString();
                        if (level == 4) pfstr = sgen.GetSession(MyGuid, "l4fstr").ToString();
                        if (level == 5) pfstr = sgen.GetSession(MyGuid, "l5fstr").ToString();
                        cmd = myquery(level, pfstr, param2, cond);
                        sgen.open_grid(title, cmd, seektype);
                        ViewBag.scripCall = "callFoo3('Balance Sheet');";
                        Multiton.SetSession(MyGuid, "LEVEL", (level - 1).ToString());
                    }
                    catch (Exception ex) { }
                }
                #endregion
                else if (!command.Trim().ToUpper().Equals("CALLBACK") && !command.Trim().ToUpper().Equals("CALLBACK2"))
                {
                    sgen.open_grid(title, cmd, seektype);
                    ViewBag.scripCall = "callFoo('" + title + "');";
                }
            }
            catch (Exception err)
            {
                var LineNumber = new StackTrace(err, true).GetFrame(0).GetFileLineNumber();
                ViewBag.scripCall += "showmsgJS(1, '" + err.Message.Replace("'", "") + " At Line number " + LineNumber + "', 1);";
            }
            End:
            ModelState.Clear();
            return View(model);
        }
        #endregion
        #region pl
        public ActionResult pl(string fdt, string tdt)
        {
            FillMst();
            fdt = year_from.Split(' ')[0].Trim(); tdt = year_to.Split(' ')[0].Trim();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            ViewBag.scripCall = "enableForm();";
            model[0].Col9 = "PROFIT AND LOSS";
            model[0].Col10 = "vouchers";
            model[0].Col11 = " and v.client_unit_id = '" + unitid_mst + "'";
            model[0].Col12 = "";
            var dtlib = new DataTable();
            var dtass = new DataTable();
            var dtpl = new DataTable();
            var dtmain = new DataTable();
            mq = "select (m.master_id) lbfstr,m.master_name lbname,sum(v.closing) lbcl from master_setting m " +
    "inner join (select client_unit_id, acode, sum(op) op, sum(dramount) dramount, sum(cramount) cramount, (sum(op) + sum(cramount) - sum(dramount)) closing from " +
    "(select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, OP_" + Ac_Year.Split(' ')[0].Trim() + " as op, 0 as dramount, 0 as cramount," +
    " 0 as cl from acbal " +
    "union all " +
    "select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, sum(to_number(dramount)) - sum(to_number(cramount)) as op, 0 as dramount," +
    "0 as cramount, 0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
    "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') - 1 group by client_unit_id, acode " +
    "union all " +
    "select client_unit_id, acode as acode, vch_num, vch_date, 0 as op, to_number(dramount) dramount, to_number(cramount) cramount, 0 as cl from vouchers " +
    "where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') " +
    "and to_date('" + tdt + "', '" + sgen.Getsqldateformat() + "')) t group by client_unit_id, acode) v on substr(m.master_id,1,1)=substr(v.acode,1,1) " +
    "where m.type = 'AGM' and trim(substr(m.master_id,1,1)) in ('4') and trim(substr(master_type,2,7))='000000' group by m.master_name,m.master_id order by m.master_id asc";
            dtlib = sgen.getdata(userCode, mq);

            //(v.client_unit_id || m.master_id || m.type) asfstr,
            mq = "select (m.master_id) asfstr,m.master_name asname,sum(v.closing) ascl from master_setting m " +
            "inner join (select client_unit_id, acode, sum(op) op, sum(dramount) dramount, sum(cramount) cramount, (sum(op) + sum(dramount) - sum(cramount)) closing from " +
            "(select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, OP_" + Ac_Year.Split(' ')[0].Trim() + " as op, 0 as dramount, 0 as cramount," +
            " 0 as cl from acbal " +
            "union all " +
            "select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, sum(to_number(dramount)) - sum(to_number(cramount)) as op, 0 as dramount," +
            "0 as cramount, 0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
            "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') - 1 group by client_unit_id, acode " +
            "union all " +
            "select client_unit_id, acode as acode, vch_num, vch_date, 0 as op, to_number(dramount) dramount, to_number(cramount) cramount, 0 as cl from vouchers " +
            "where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') " +
            "and to_date('" + tdt + "', '" + sgen.Getsqldateformat() + "')) t group by client_unit_id, acode) v on substr(m.master_id,1,1)=substr(v.acode,1,1) " +
            "where m.type = 'AGM' and trim(substr(m.master_id,1,1)) in ('5') and trim(substr(master_type,2,7))='000000' group by m.master_name,m.master_id";
            dtass = sgen.getdata(userCode, mq);

            //mq = "select '4.5.6' as plfstr,'Profit and Loss' as plname, sum(income)-sum(expn) as plcl from( select (case when substr(acode,1,1)='4' then closing " +
            //"else 0 end) as income,(case when substr(acode, 1, 1) in ('5', '6') then closing else 0 end) as expn from " +
            //"(select client_unit_id, acode, sum(op) op, sum(dramount) dramount, sum(cramount) cramount, (sum(op) + sum(dramount) - sum(cramount)) closing from " +
            //"(select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, OP_" + Ac_Year.Split(' ')[0].Trim() + " as op, 0 as dramount, 0 as cramount, 0 as cl " +
            //"from acbal " +
            //"union all " +
            //"select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, sum(to_number(dramount)) - sum(to_number(cramount)) as op," +
            //"0 as dramount, 0 as cramount, 0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') " +
            //"between to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') - 1 group by client_unit_id, acode " +
            //"union all " +
            //"select client_unit_id, acode as acode, vch_num, vch_date, 0 as op, to_number(dramount) dramount, to_number(cramount) cramount, 0 as cl " +
            //"from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
            //"to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') and to_date('" + tdt + "', '" + sgen.Getsqldateformat() + "')) t group by client_unit_id, acode) t )t";
            //dtpl = sgen.getdata(userCode, mq);
            model[0].dt1 = dtlib;
            model[0].dt2 = dtass;
            //model[0].dt3 = dtpl;
            //model[0].Col16 = sgen.Make_decimal(model[0].dt1.Rows[0][0].ToString());
            //model[0].Col17 = sgen.Make_decimal(model[0].dt1.Rows[0][0].ToString());
            //model[0].Col18 = sgen.Make_decimal(model[0].dt1.Rows[0][0].ToString());
            model[0].Col16 = model[0].dt1.AsEnumerable().Sum(x => sgen.Make_decimal(x[2].ToString())).ToString();
            model[0].Col17 = model[0].dt2.AsEnumerable().Sum(x => sgen.Make_decimal(x[2].ToString())).ToString();
            model[0].Col18 = (sgen.Make_decimal(model[0].Col16) - sgen.Make_decimal(model[0].Col17)).ToString();


            //dtmain.Columns.Add("lbfstr");
            //dtmain.Columns.Add("lbname");
            //dtmain.Columns.Add("lbcl");

            //dtmain.Columns.Add("asfstr");
            //dtmain.Columns.Add("asname");
            //dtmain.Columns.Add("ascl");

            //int lrows = dtlib.Rows.Count;
            //int mrows = dtmain.Rows.Count - 1;

            //try
            //{
            //    for (int l = 0; l < lrows; l++)
            //    {
            //        if (mrows > l)
            //        {
            //            dtmain.Rows[l]["lbfsr"] = dtlib.Rows[l]["lbfstr"].ToString();
            //            dtmain.Rows[l]["lbname"] = dtlib.Rows[l]["lbname"].ToString();
            //            dtmain.Rows[l]["lbcl"] = dtlib.Rows[l]["lbcl"].ToString();
            //        }
            //        else
            //        {
            //            DataRow dr = dtmain.NewRow();
            //            dr["lbfstr"] = dtlib.Rows[l]["lbfstr"].ToString();
            //            dr["lbname"] = dtlib.Rows[l]["lbname"].ToString();
            //            dr["lbcl"] = dtlib.Rows[l]["lbcl"].ToString();
            //            dtmain.Rows.Add(dr);
            //        }
            //    }

            //    int plrows = dtpl.Rows.Count;
            //    mrows = dtmain.Rows.Count - 1;
            //    for (int l = 0; l < plrows; l++)
            //    {
            //        if (mrows > l)
            //        {
            //            dtmain.Rows[l]["lbfstr"] = dtpl.Rows[l]["plfstr"].ToString();
            //            dtmain.Rows[l]["lbname"] = dtpl.Rows[l]["plname"].ToString();
            //            dtmain.Rows[l]["lbcl"] = dtpl.Rows[l]["plcl"].ToString();
            //        }
            //        else
            //        {
            //            DataRow dr = dtmain.NewRow();
            //            dr["lbfstr"] = dtpl.Rows[l]["plfstr"].ToString();
            //            dr["lbname"] = dtpl.Rows[l]["plname"].ToString();
            //            dr["lbcl"] = dtpl.Rows[l]["plcl"].ToString();
            //            dtmain.Rows.Add(dr);
            //        }
            //    }

            //    int asrows = dtass.Rows.Count;
            //    mrows = dtmain.Rows.Count - 1;
            //    for (int l = 0; l < asrows; l++)
            //    {
            //        if (mrows > l)
            //        {
            //            dtmain.Rows[l]["asfstr"] = dtass.Rows[l]["asfstr"].ToString();
            //            dtmain.Rows[l]["asname"] = dtass.Rows[l]["asname"].ToString();
            //            dtmain.Rows[l]["ascl"] = dtass.Rows[l]["ascl"].ToString();
            //        }
            //        else
            //        {
            //            DataRow dr = dtmain.NewRow();
            //            dr["asfstr"] = dtass.Rows[l]["asfstr"].ToString();
            //            dr["asname"] = dtass.Rows[l]["asname"].ToString();
            //            dr["ascl"] = dtass.Rows[l]["ascl"].ToString();
            //            dtmain.Rows.Add(dr);
            //        }
            //    }
            //}
            //catch (Exception ex) { }
            //dtmain.AcceptChanges();
            //model[0].dt1 = dtmain;
            return View(model);
        }
        [HttpPost]
        public ActionResult pl(List<Tmodelmain> model, string command)
        {
            string fdt = year_from.Split(' ')[0].Trim(); string tdt = year_to.Split(' ')[0].Trim();
            FillMst();
            sgen.SetSession(MyGuid, "KPDFDT", fdt);
            sgen.SetSession(MyGuid, "KPDTDT", tdt);
            string title = "";
            int seektype = 0;
            try
            {
                if (command == "Callback2")
                {
                    var mlvl = sgen.Make_int(Multiton.GetSession(MyGuid, "MAXLEVEL").ToString());
                    var lvl = sgen.Make_int(Multiton.GetSession(MyGuid, "LEVEL").ToString()) + 2;
                    if (mlvl == lvl)
                    {
                        string param1 = "";
                        if (sgen.GetSession(MyGuid, "fstr0th") != null) param1 = sgen.GetSession(MyGuid, "fstr0th").ToString();
                        cmd = myquery(3, param1, fdt + "!~!~!~!~!" + tdt, " and (t.client_unit_id || t.acode) = '" + unitid_mst + "" + param1.Trim() + "'");
                        sgen.open_grid("Balance Sheet", cmd, 1);
                        ViewBag.scripCall = "callFoo('Balance Sheet');";
                        Multiton.SetSession(MyGuid, "LEVEL", 2);
                        Multiton.SetSession(MyGuid, "MAXLEVEL", 3);
                        goto End;
                    }
                }
                else if (command == "Back")
                {
                    try
                    {
                        return RedirectToAction("gst_rpt", "Account", new { @m_id = EncryptDecrypt.Encrypt(model[0].Col15), @mid = EncryptDecrypt.Encrypt(model[0].Col14) });
                    }
                    catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "',0);"; }
                }
                #region callback
                else if (command == "Callback")
                {
                    try
                    {
                        string cond = "";
                        try
                        {
                            cond = sgen.GetSession(MyGuid, "qucond").ToString();
                        }
                        catch (Exception ex) { }
                        int level = sgen.Make_int(Multiton.GetSession(MyGuid, "LEVEL").ToString());
                        string param1 = sgen.GetSession(MyGuid, "SSEEKVAL").ToString();
                        string param2 = fdt + "!~!~!~!~!" + tdt;
                        if (level == 1)
                        {
                            if (level == 1) sgen.SetSession(MyGuid, "l1fstr", param1);
                            String vouurl = param1;
                            return RedirectToAction("cas_recn", new { @m_id = EncryptDecrypt.Encrypt(model[0].Col15), @mid = EncryptDecrypt.Encrypt(model[0].Col14), @vid = EncryptDecrypt.Encrypt(vouurl) });
                        }
                        else if (level == 0) { }
                        else
                        {
                            seektype = 1;
                            sgen.SetSession(MyGuid, "btnval", btnval);

                            cmd = myquery(level, param1, param2, cond);
                            sgen.open_grid(title, cmd, seektype);

                            if (level == 2) sgen.SetSession(MyGuid, "l2fstr", param1);
                            else if (level == 3) sgen.SetSession(MyGuid, "l3fstr", param1);
                            else if (level == 4) sgen.SetSession(MyGuid, "l4fstr", param1);
                            else if (level == 5) sgen.SetSession(MyGuid, "l5fstr", param1);
                            ViewBag.scripCall = "callFoo3('Balance Sheet Detailed');";
                        }
                        Multiton.SetSession(MyGuid, "LEVEL", (level - 1).ToString());
                    }
                    catch (Exception ex) { }
                }
                #endregion
                #region callback2
                if (command == "Callback2")
                {

                    try
                    {
                        string cond = "";
                        try
                        {
                            cond = sgen.GetSession(MyGuid, "qucond").ToString();
                        }
                        catch (Exception ex) { }
                        int level = sgen.Make_int(Multiton.GetSession(MyGuid, "LEVEL").ToString()) + 2;
                        string param1 = sgen.GetSession(MyGuid, "SSEEKVAL").ToString();
                        string param2 = fdt + "!~!~!~!~!" + tdt;
                        seektype = 1;
                        sgen.SetSession(MyGuid, "btnval", btnval);
                        string pfstr = "";
                        if (level == 1) pfstr = sgen.GetSession(MyGuid, "l1fstr").ToString();
                        if (level == 2) pfstr = sgen.GetSession(MyGuid, "l2fstr").ToString();
                        if (level == 3) pfstr = sgen.GetSession(MyGuid, "l3fstr").ToString();
                        if (level == 4) pfstr = sgen.GetSession(MyGuid, "l4fstr").ToString();
                        if (level == 5) pfstr = sgen.GetSession(MyGuid, "l5fstr").ToString();
                        cmd = myquery(level, pfstr, param2, cond);
                        sgen.open_grid(title, cmd, seektype);
                        ViewBag.scripCall = "callFoo3('Balance Sheet');";
                        Multiton.SetSession(MyGuid, "LEVEL", (level - 1).ToString());
                    }
                    catch (Exception ex) { }
                }
                #endregion
                else if (!command.Trim().ToUpper().Equals("CALLBACK") && !command.Trim().ToUpper().Equals("CALLBACK2"))
                {
                    sgen.open_grid(title, cmd, seektype);
                    ViewBag.scripCall = "callFoo('" + title + "');";
                }
            }
            catch (Exception err)
            {
                var LineNumber = new StackTrace(err, true).GetFrame(0).GetFileLineNumber();
                ViewBag.scripCall += "showmsgJS(1, '" + err.Message.Replace("'", "") + " At Line number " + LineNumber + "', 1);";
            }
        End:
            ModelState.Clear();
            return View(model);
        }
        #endregion
        #region chq_series
        public ActionResult chq_srl()
        {
            FillMst("");
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "CHEQUE SERIES";
            model[0].Col10 = "ENX_TAB";
            model[0].Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            model[0].Col12 = "CSR";
            List<SelectListItem> md1 = new List<SelectListItem>();
            TempData[MyGuid + "_TList1"] = model[0].TList1 = md1;
            TempData[MyGuid + "_TList2"] = model[0].TList2 = md1;
            model[0].SelectedItems1 = new string[] { "" };
            model[0].SelectedItems2 = new string[] { "" };
            return View(model);
        }
        [HttpPost]
        public ActionResult chq_srl(List<Tmodelmain> model, string command)
        {
            FillMst(model[0].Col15);
            var tm = model.FirstOrDefault();
            List<SelectListItem> mod1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
            TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
            if (model[0].SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
            DataTable dt = new DataTable();
            if (command == "New")
            {
                model = getnew(model);
                model[0].Col17 = sgen.server_datetime_local(userCode);
                #region Dropdwon
                mod1 = new List<SelectListItem>();
                mq = "select nvl(a.master_name,'-')||'('||cm.cpname||')' as bank_name,cm.cpname as bank_ac,cm.vch_num as bankid from clients_mst cm " +
                    "inner join master_setting a on a.master_id = cm.ptype and a.type = 'ABD' and find_in_set(a.client_unit_id,cm.client_unit_id)=1 " +
                    "where cm.type = 'BAD' and find_in_set(cm.client_unit_id, '" + unitid_mst + "')=1 and cm.vch_num not in (select col2 from enx_tab where type='CSR' and client_unit_id='" + unitid_mst + "')";
                dt = new DataTable();
                dt = sgen.getdata(userCode, mq);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        mod1.Add(new SelectListItem { Text = item["bank_name"].ToString(), Value = item["bankid"].ToString() });
                    }
                }
                TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
                model[0].SelectedItems1 = new string[] { "" };
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
            else if (command == "Save" || command == "Update")
            {
                try
                {
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
                        mq = "select max(to_number(vch_num)) as maxno from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "maxno");
                        model[0].Col16 = vch_num;
                    }
                    DataTable dataTable = sgen.getdata(userCode, "select * from " + model[0].Col10.Trim() + " where  1=2");
                    DataRow dr = dataTable.NewRow();
                    dr["vch_num"] = vch_num;
                    dr["type"] = model[0].Col12;
                    dr["vch_date"] = sgen.Make_date_S(model[0].Col17);
                    dr["col2"] = model[0].SelectedItems1.FirstOrDefault();
                    dr["col9"] = model[0].Col19;
                    dr["col10"] = model[0].Col20;
                    dr["col1"] = model[0].Col21; //remark
                    dr = getsave(currdate, dr, model);
                    dataTable.Rows.Add(dr);
                    bool Result = sgen.Update_data_fast1(userCode, dataTable, model[0].Col10.Trim(), tmodel.Col8, isedit);
                    if (Result == true)
                    {
                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
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
                        });
                    }
                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
            }
            ModelState.Clear();
            return View(model);
        }
        #endregion
        #region account linking
        public ActionResult acc_link()
        {
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            FillMst(MyGuid);
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "ACCOUNT LINKING";
            model[0].Col10 = "enx_tab";
            model[0].Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            model[0].Col12 = "ACL";
            List<SelectListItem> md1 = new List<SelectListItem>();
            TempData[MyGuid + "_TList1"] = model[0].TList1 = md1;
            TempData[MyGuid + "_TList2"] = model[0].TList2 = md1;
            TempData[MyGuid + "_TList3"] = model[0].TList3 = md1;
            TempData[MyGuid + "_TList4"] = model[0].TList4 = md1;
            model[0].SelectedItems1 = new string[] { "" };
            model[0].SelectedItems2 = new string[] { "" };
            model[0].SelectedItems3 = new string[] { "" };
            model[0].SelectedItems4 = new string[] { "" };
            return View(model);
        }
        public List<Tmodelmain> new_acc_link(List<Tmodelmain> model)
        {
            model = getnew(model);
            var tm = model.FirstOrDefault();
            //model[0].Col17 = sgen.server_datetime_local(userCode);
            #region ddls
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<SelectListItem> mod2 = new List<SelectListItem>();
            List<SelectListItem> mod3 = new List<SelectListItem>();
            List<SelectListItem> mod4 = new List<SelectListItem>();
            #region Group type
            DataTable dt = new DataTable();
            dt = sgen.getdata(userCode, "select master_id,master_name from master_setting where type='TAC' and client_id = '" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    mod1.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                }
            }
            TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
            #endregion
            TempData[MyGuid + "_TList2"] = model[0].TList2 = mod2;
            TempData[MyGuid + "_TList3"] = model[0].TList3 = mod3;
            mq = "select * from (" +
                "select 'P001' master_id,'IGST (PAYBLE)' master_name from dual union all " +
                "select 'P002' master_id,'SGST (PAYBLE)' master_name from dual union all " +
                "select 'P003' master_id,'CGST (PAYBLE)' master_name from dual union all " +
                "select 'R001' master_id,'IGST (RECIEVABLE)' master_name from dual union all " +
                "select 'R002' master_id,'SGST (RECIEVABLE)' master_name from dual union all " +
               "select 'R003' master_id,'CGST (RECIEVABLE)' master_name from dual) a where master_id not in" +
               " (select col2 from enx_tab where type='ACL' and client_unit_id='" + unitid_mst + "')";
            dt = sgen.getdata(userCode, mq);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    mod4.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                }
            }
            TempData[MyGuid + "_TList4"] = model[0].TList4 = mod4;
            model[0].SelectedItems1 = new string[] { "" };
            model[0].SelectedItems2 = new string[] { "" };
            model[0].SelectedItems3 = new string[] { "" };
            model[0].SelectedItems4 = new string[] { "" };
            #endregion
            return model;
        }
        [HttpPost]
        public ActionResult acc_link(List<Tmodelmain> model, string command, string mid)
        {
            try
            {
                FillMst(model[0].Col15);
                var tm = model.FirstOrDefault();
                List<SelectListItem> mod1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
                TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
                if (model[0].SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
                List<SelectListItem> mod2 = (List<SelectListItem>)TempData[MyGuid + "_TList2"];
                TempData[MyGuid + "_TList2"] = model[0].TList2 = mod2;
                if (model[0].SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
                List<SelectListItem> mod3 = (List<SelectListItem>)TempData[MyGuid + "_TList3"];
                TempData[MyGuid + "_TList3"] = model[0].TList3 = mod3;
                if (model[0].SelectedItems3 == null) model[0].SelectedItems3 = new string[] { "" };
                List<SelectListItem> mod4 = (List<SelectListItem>)TempData[MyGuid + "_TList4"];
                TempData[MyGuid + "_TList4"] = model[0].TList4 = mod4;
                if (model[0].SelectedItems4 == null) model[0].SelectedItems4 = new string[] { "" };
                if (command == "New")
                {
                    model = new_acc_link(model);
                }
                else if (command == "Cancel")
                {
                    return CancelFun(model);
                }
                else if (command == "Callback")
                {
                    model = StartCallback(model);
                }
                else if (command == "EMPLOYEE")
                {
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vsavenew = "";
                    ViewBag.scripCall = "enableForm();";
                    mod2 = new List<SelectListItem>();
                    model[0].SelectedItems2 = new string[] { "" };
                    DataTable dt = new DataTable();
                    dt = sgen.getdata(userCode, "select substr(master_type,0,3) as master_id,master_name from master_setting where type='AGM' and client_id = '" + clientid_mst + "' and client_unit_id='" + unitid_mst + "' and substr(master_id,1,1)='" + model[0].SelectedItems1.FirstOrDefault() + "'");
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            mod2.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                        }
                    }
                    TempData[MyGuid + "_TList2"] = model[0].TList2 = mod2;
                }
                else if (command == "LEDGER")
                {
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vsavenew = "";
                    ViewBag.scripCall = "enableForm();";
                    mod3 = new List<SelectListItem>();
                    model[0].SelectedItems3 = new string[] { "" };
                    DataTable dt = new DataTable();
                    mq = "select a.vch_num as Ledger_code,a.c_name as Ledger_Name from clients_mst a where " +
                       "find_in_set(a.client_id,'" + clientid_mst + "')=1 and find_in_set(a.client_unit_id,'" + unitid_mst + "')=1 and substr(a.vch_num,0,3)='" + model[0].SelectedItems2.FirstOrDefault() + "' and type='LDG'";
                    dt = sgen.getdata(userCode, mq);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            mod3.Add(new SelectListItem { Text = dr["Ledger_Name"].ToString(), Value = dr["Ledger_code"].ToString() });
                        }
                    }
                    TempData[MyGuid + "_TList3"] = model[0].TList3 = mod3;
                }
                else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
                {

                    var tmodel = model.FirstOrDefault();
                    string currdate = "";
                    DataTable dtstr = new DataTable();
                    dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    currdate = sgen.server_datetime(userCode);

                    #region dtstr
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
                    string TAXNAME = ((List<SelectListItem>)TempData[MyGuid + "_TList4"]).SingleOrDefault(item => item.Value == model[0].SelectedItems4.FirstOrDefault().ToString()).Text.ToString();
                    DataRow dr = dtstr.NewRow();
                    dr["type"] = model[0].Col12;
                    dr["vch_num"] = vch_num.Trim();
                    dr["vch_date"] = sgen.Make_date_S(currdate);
                    dr["col2"] = model[0].SelectedItems4.FirstOrDefault(); // acc name
                    dr["col1"] = TAXNAME; // acc name
                    dr["col5"] = model[0].SelectedItems1.FirstOrDefault(); // grp 
                    dr["col6"] = model[0].SelectedItems2.FirstOrDefault(); //  subgrp
                    dr["col7"] = model[0].SelectedItems3.FirstOrDefault(); // ledger
                    dr = getsave(currdate, dr, model);
                    dtstr.Rows.Add(dr);
                    #endregion
                    bool Result = sgen.Update_data_fast1(userCode, dtstr, model[0].Col10.Trim(), tmodel.Col8, isedit);
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
                            TList2 = mod2,
                            TList3 = mod3,
                            TList4 = mod4,
                            SelectedItems1 = new string[] { "" },
                            SelectedItems4 = new string[] { "" },
                            SelectedItems2 = new string[] { "" },
                            SelectedItems3 = new string[] { "" },
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
                                model = new_acc_link(model);
                                ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                            }
                            catch (Exception ex) { }
                        }
                    }
                    else
                    {
                        ViewBag.scripCall = "showmsgJS(1, 'Record Not Saved', 2);";
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
        #region cheque rejection
        public ActionResult chq_rej()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "CHEQUE REJECTION";
            model[0].Col10 = "ENX_TAB";
            model[0].Col11 = " and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            model[0].Col12 = "CRJ"; //CHQ REJ
            List<SelectListItem> md1 = new List<SelectListItem>();
            TempData[MyGuid + "_TList1"] = model[0].TList1 = md1;
            model[0].SelectedItems1 = new string[] { "" };
            DataTable dt = sgen.getdata(userCode, "select '0' as  Select_ ,'1' as  Sno ,'-' as Cheque_no,'-' as Reason_of_Rejection from dual");
            model[0].dt1 = dt;
            sgen.SetSession(MyGuid, "BDCHQRJ", dt.Copy());
            return View(model);
        }
        [HttpPost]
        public ActionResult chq_rej(List<Tmodelmain> model, string command, string hfrow, string hftable)
        {
            try
            {
                FillMst(model[0].Col15);
                var tm = model.FirstOrDefault();
                DataTable dtm = sgen.settable(hftable);
                model[0].dt1 = dtm;
                List<SelectListItem> mod1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
                TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
                if (model[0].SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
                DataSet dst = new DataSet();
                if (command == "Cancel")
                {
                    return CancelFun(model);
                }
                else if (command == "New")
                {
                    model = getnew(model);

                    model[0].Col17 = sgen.server_datetime_local(userCode);
                    #region Dropdwon
                    mod1 = new List<SelectListItem>();
                    mq = "select nvl(a.master_name,'-')||'('||cm.cpname||')' as bank_name,cm.cpname as bank_ac,cm.vch_num as bankid from clients_mst cm " +
                        "inner join master_setting a on a.master_id = cm.ptype and a.type = 'ABD' and find_in_set(a.client_unit_id,cm.client_unit_id)=1 " +
                        "where cm.type = 'BAD' and find_in_set(cm.client_unit_id, '" + unitid_mst + "')=1";
                    DataTable dt = new DataTable();
                    dt = sgen.getdata(userCode, mq);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            mod1.Add(new SelectListItem { Text = item["bank_name"].ToString(), Value = item["bankid"].ToString() });
                        }
                    }
                    TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
                    model[0].SelectedItems1 = new string[] { "" };
                    #endregion
                }
                else if (command == "Callback")
                {
                    model = StartCallback(model);
                }
                else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
                {
                    var tmodel = model.FirstOrDefault();
                    string currdate = "";
                    DataTable dtstr = new DataTable();
                    dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    currdate = sgen.server_datetime(userCode);
                    #region dtstr
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
                        dr["type"] = model[0].Col12;
                        dr["vch_date"] = sgen.Make_date_S(model[0].Col17);
                        dr["col2"] = model[0].SelectedItems1.FirstOrDefault();//as bank
                        dr["col3"] = model[0].dt1.Rows[i]["Cheque_no"].ToString();
                        dr["col1"] = model[0].dt1.Rows[i]["Reason_of_Rejection"].ToString();
                        dr = getsave(currdate, dr, model);
                        dtstr.Rows.Add(dr);
                    }
                    bool Result = sgen.Update_data_fast1(userCode, dtstr, model[0].Col10.Trim(), tmodel.Col8, isedit);
                    #endregion
                    if (Result == true)
                    {
                        dtm = (DataTable)sgen.GetSession(MyGuid, "BDCHQRJ");
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
                            dt1 = dtm,
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
                                model = new_acc_link(model);
                                ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                            }
                            catch (Exception ex) { }
                        }
                    }
                    else
                    {
                        ViewBag.scripCall = "showmsgJS(1, 'Record Not Saved', 2);";
                    }
                }
                else if (command == "-")
                {
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vsavenew = "";
                    if (model[0].dt1.Rows.Count > 1) model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                    else { model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "BDCHQRJ"); }
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
        #region reconciliation
        public ActionResult recil()
        {
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            FillMst(MyGuid);
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "BANK RECONCILIATION";
            model[0].Col10 = "enx_tab2";
            model[0].Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            model[0].Col12 = "RCL";
            List<SelectListItem> md1 = new List<SelectListItem>();
            TempData[MyGuid + "_TList1"] = model[0].TList1 = md1;
            TempData[MyGuid + "_TList2"] = model[0].TList2 = md1;
            TempData[MyGuid + "_TList3"] = model[0].TList3 = md1;
            model[0].SelectedItems1 = new string[] { "" };
            model[0].SelectedItems2 = new string[] { "" };
            model[0].SelectedItems3 = new string[] { "" };
            return View(model);
        }
        public List<Tmodelmain> new_recil(List<Tmodelmain> model)
        {
            var tm = model.FirstOrDefault();
            string year = year_from.Substring(6, 4);
            //string currdate = sgen.server_datetime_dt(userCode).ToString("dd/MM/yyyy");
            model = getnew(model);
            model[0].Col17 = sgen.server_datetime_local(userCode);
            #region Dropdwon
            List<SelectListItem> mod1 = new List<SelectListItem>();
            mq = "select nvl(a.master_name,'-')||'('||cm.cpname||')' as bank_name,cm.cpname as bank_ac,cm.vch_num as bankid from clients_mst cm " +
                "inner join master_setting a on a.master_id = cm.ptype and a.type = 'ABD' and find_in_set(a.client_unit_id,cm.client_unit_id)=1 " +
                "where cm.type = 'BAD' and find_in_set(cm.client_unit_id, '" + unitid_mst + "')=1";
            DataTable dt = new DataTable();
            dt = sgen.getdata(userCode, mq);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    mod1.Add(new SelectListItem { Text = item["bank_name"].ToString(), Value = item["bankid"].ToString() });
                }
            }
            TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
            model[0].SelectedItems1 = new string[] { "" };
            #endregion
            return model;
        }
        [HttpPost]
        public ActionResult recil(List<Tmodelmain> model, string command, string mid)
        {
            try
            {
                string year = year_from.Substring(6, 4);
                FillMst(model[0].Col15);
                var tm = model.FirstOrDefault();
                List<SelectListItem> mod1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
                TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
                if (model[0].SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
                if (command == "New")
                {
                    model = new_recil(model);
                }
                else if (command == "Cancel")
                {
                    return CancelFun(model);
                }
                else if (command == "Closing")
                {
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vsavenew = "";
                    ViewBag.scripCall = "enableForm();";
                    model[0].Col21 = "";
                    model[0].Col22 = "";
                    model[0].Col23 = "";
                    model[0].Col24 = "";
                    model[0].Col25 = "";
                    model[0].Col26 = "";
                    DataTable dt = new DataTable();
                    mq = "select p.closing from (SELECT ACODE,CLIENT_UNIT_ID,SUM(OPBAL) AS OPBAL,SUM(DRAMOUNT),SUM(CRAMOUNT),SUM(OPBAL)+SUM(DRAMOUNT)-SUM(CRAMOUNT) as closing from " +
                                                            "(select acode,CLIENT_UNIT_ID, OP_" + year + " as opbal,0 as cramount,0 as dramount,0 as cl from acbal union all select acode,CLIENT_UNIT_ID,sum(dramount)-sum(cramount) as opbal" +
                            ",0 as cramount,0 as dramount,0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and " +
                                                              "to_date('" + year_from.Split(' ')[0].Trim() + "','" + sgen.Getsqldateformat() + "')-1 group by acode,CLIENT_UNIT_ID union all select acode,CLIENT_UNIT_ID,0 as opbal,TO_NUMBER(cramount) cramount,TO_NUMBER(dramount) dramount,0 cl from vouchers " +
                                                              "where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                                                              "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + model[0].Col17 + "','" + sgen.Getsqldateformat() + "')) group by acode,CLIENT_UNIT_ID) p where p.acode='" + model[0].SelectedItems1.FirstOrDefault() + "'";
                    dt = sgen.getdata(userCode, mq);
                    if (dt.Rows.Count > 0)
                    {
                        model[0].Col21 = dt.Rows[0]["closing"].ToString();
                    }
                }
                else if (command == "Fill Data")
                {
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vsavenew = "";
                    if (model[0].Chk1)
                    {
                        mq = "select (v.client_unit_id|| v.vch_num|| to_char(v.vch_date, 'yyyymmdd')||v.type) as fstr,v.type||'_'||v.vch_num as doc_no,to_char(v.vch_date,'dd/MM/YYYY') as doc_date,c.c_name as party" +
                   ",v.chqno,to_char(v.bankdate,'" + sgen.Getsqldateformat() + "') as bankdate,TO_NUMBER(v.dramount) as dramount,TO_NUMBER(v.cramount) as cramount from vouchers v INNER JOIN(select a.vch_num, a.c_name from(select vch_num, c_name, " +
                   "CLIENT_UNIT_ID, vch_date, type from clients_mst where type = 'BCD' and find_in_set(client_unit_id, '" + unitid_mst + "') = 1 union all " +
                   "select vch_num as acode, c_name as aname, CLIENT_UNIT_ID, vch_date, type from clients_mst where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "') = 1" +
                   " union all " +
                   "select cm.vch_num as acode, nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname, cm.CLIENT_UNIT_ID, cm.vch_date, cm.type from master_setting a " +
                   "inner join clients_mst cm on a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id where cm.type = 'BAD' and cm.client_unit_id = '" + unitid_mst + "') a) c " +
                   "on c.vch_num = v.rvscode where v.acode = '" + model[0].SelectedItems1.FirstOrDefault() + "' and v.client_unit_id = '" + unitid_mst + "' and(to_number(v.dramount) + to_number(v.cramount)) > 0 and nvl(v.reco,'-') = 'Y' order by v.vch_num desc";
                    }
                    else
                    {
                        if (model[0].SelectedItems1.FirstOrDefault() != "")
                        {
                            mq = "select (v.client_unit_id|| v.vch_num|| to_char(v.vch_date, 'yyyymmdd')||v.type) as fstr,v.type||'_'||v.vch_num as doc_no,to_char(v.vch_date,'dd/MM/YYYY') as doc_date,c.c_name as party" +
                            ",v.chqno,TO_NUMBER(v.dramount) as dramount,'' as bankdate,TO_NUMBER(v.cramount) as cramount from vouchers v INNER JOIN(select a.vch_num, a.c_name from(select vch_num, c_name, " +
                            "CLIENT_UNIT_ID, vch_date, type from clients_mst where type = 'BCD' and find_in_set(client_unit_id, '" + unitid_mst + "') = 1 union all " +
                            "select vch_num as acode, c_name as aname, CLIENT_UNIT_ID, vch_date, type from clients_mst where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "') = 1" +
                            " union all " +
                            "select cm.vch_num as acode, nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname, cm.CLIENT_UNIT_ID, cm.vch_date, cm.type from master_setting a " +
                            "inner join clients_mst cm on a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id where cm.type = 'BAD' and cm.client_unit_id = '" + unitid_mst + "') a) c " +
                            "on c.vch_num = v.rvscode where v.acode = '" + model[0].SelectedItems1.FirstOrDefault() + "' and v.client_unit_id = '" + unitid_mst + "' and(to_number(v.dramount) + to_number(v.cramount)) > 0 and nvl(v.reco,'-') in ('-','0') order by v.vch_num desc";
                        }
                    }
                    DataTable dtques = new DataTable();
                    dtques = sgen.getdata(userCode, mq);
                    if (dtques.Rows.Count > 0)
                    {
                        model = new List<Tmodelmain>();
                        foreach (DataRow dr in dtques.Rows)
                        {
                            model.Add(new Tmodelmain
                            {

                                Col30 = dr["doc_no"].ToString(),
                                Col31 = dr["doc_date"].ToString(),
                                Col32 = dr["chqno"].ToString(),
                                Col33 = dr["dramount"].ToString(),
                                Col34 = dr["cramount"].ToString(),
                                Col35 = dr["bankdate"].ToString(),
                                Col36 = dr["party"].ToString(),
                                Col8 = tm.Col8,
                                Col9 = tm.Col9,
                                Col11 = tm.Col11,
                                Col12 = tm.Col12,
                                Col10 = tm.Col10,
                                Col13 = tm.Col13,
                                Col14 = tm.Col14,
                                Col15 = tm.Col15,
                                Col16 = tm.Col16,
                                Col17 = tm.Col17,
                                Col18 = tm.Col18,
                                Col19 = tm.Col19,
                                Col20 = tm.Col20,
                                Col21 = tm.Col21,
                                Col22 = tm.Col22,
                                Col23 = tm.Col23,
                                Col24 = tm.Col24,
                                Col25 = tm.Col25,
                                Col26 = tm.Col26,
                                Chk1 = tm.Chk1,
                                Chk2 = tm.Chk2,
                                Col100 = tm.Col100,
                                Col121 = tm.Col121,
                                Col122 = tm.Col122,
                                Col123 = tm.Col123,
                                TList1 = tm.TList1,
                                SelectedItems1 = tm.SelectedItems1,
                            });
                        }
                    }
                }
                else if (command == "Callback")
                {
                    model = StartCallback(model);
                }
                else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
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
                        mq = "select max(to_number(vch_num)) as maxno from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "maxno");
                        model[0].Col16 = vch_num;
                    }
                    DataTable dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10);
                    Satransaction sat = new Satransaction(userCode, MyGuid);
                    if (model.Count > 0)
                    {
                        DataRow dr;
                        for (int i = 0; i < model.Count; i++)
                        {
                            if (model[i].Col35 != null)
                            {
                                if (model[i].Col35 != "")
                                {
                                    dr = dtstr.NewRow();
                                    dr["vch_num"] = vch_num;
                                    dr["type"] = model[0].Col12;
                                    dr["vch_date"] = currdate;
                                    dr["date1"] = sgen.Make_date_S(model[0].Col17);
                                    dr["col5"] = model[0].SelectedItems1.FirstOrDefault();
                                    dr["col7"] = model[0].Col19; //checking bal
                                    dr["col9"] = model[0].Col22;//total dr
                                    dr["col10"] = model[0].Col23; ////total cr
                                    dr["col11"] = model[0].Col24;//bank balance 
                                    dr["col21"] = model[0].Col25; //diff
                                    dr["col22"] = model[i].Col30; //vounmbr
                                    dr["date2"] = sgen.Make_date_S(model[i].Col31);//voudate
                                    dr["date3"] = sgen.Make_date_S(model[i].Col35);//bank date
                                    dr = getsave(currdate, dr, model);
                                    dtstr.Rows.Add(dr);
                                }
                            }
                        }
                    }
                    bool Result = sgen.Update_data_fast1_uncommit(userCode, dtstr, model[0].Col10.Trim(), tm.Col8, isedit, sat);
                    if (Result)
                    {
                        string url = "";
                        bool res1 = false;
                        for (int i = 0; i < model.Count; i++)
                        {
                            if (model[i].Col35 != null)
                            {
                                if (model[i].Col35 != "")
                                {
                                    var xDate = sgen.SaveDate_dt(model[i].Col35, true);
                                    string dt_ft = "dd-MM-YYYY HH24:MI:ss";
                                    string date = sgen.SaveDate_dt(model[i].Col31, true).ToString("yyyyMMdd");
                                    url = unitid_mst + model[i].Col30.Split('_')[1].ToString() + date + "" + model[i].Col30.Split('_')[0].ToString() + "";
                                    mq = "update vouchers set bankdate=to_date('" + xDate + "','" + dt_ft + "'),reco='Y' where client_unit_id||vch_num||to_char(vch_date, 'yyyymmdd')||type='" + url + "'";
                                    res1 = sgen.execute_cmd(userCode, mq);
                                }
                            }
                        }
                        if (Result && res1) sat.Commit();
                        else sat.Rollback();
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
                                model = new_rcm(model);
                                ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                            }
                            catch (Exception ex) { }
                        }
                    }
                    else
                    {
                        ViewBag.scripCall = "showmsgJS(1, 'Record Not Saved', 2);";
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
        #region cash/rec
        public ActionResult cas_rec()
        {
            FillMst("");
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col10 = "vouchers";
            model[0].Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            switch (model[0].Col14)
            {
                case "22003.1":
                    model[0].Col12 = "10";
                    model[0].Col9 = "CASH RECIEPT VOUCHER";
                    break;
                case "22003.2":
                    model[0].Col12 = "20";
                    model[0].Col9 = "CASH PAYMENT VOUCHER";
                    break;
                case "22003.3":
                    model[0].Col12 = "11";
                    model[0].Col9 = "BANK RECIEPT VOUCHER";
                    break;
                case "22003.4":
                    model[0].Col12 = "21";
                    model[0].Col9 = "BANK PAYMENT VOUCHER";
                    break;
                default:
                    model[0].Col12 = "VOU";
                    model[0].Col9 = "VOUCHER ENTRY";
                    break;
            }
            sgen.SetSession(MyGuid, "SALTYPE", model[0].Col12);
            DataTable dt = sgen.getdata(userCode, "select '0' as  Select_ ,'1' as  Sno ,'-' as Acode,'' as AName,'0' as DrAmount,'0' as  CrAmount,'-' as  Adj_type_Code,'' as  Adj_Type,'-' as  Ref_no,'-' as  Ref_date,'-' as  Remark from dual");
            model[0].dt1 = dt;
            sgen.SetSession(MyGuid, "SALVOU", dt.Copy());
            List<SelectListItem> md1 = new List<SelectListItem>();
            TempData[MyGuid + "_TList1"] = model[0].TList1 = md1;
            model[0].SelectedItems1 = new string[] { "" };
            return View(model);
        }
        [HttpPost]
        public ActionResult cas_rec(List<Tmodelmain> model, string command, string hfrow, string hftable)
        {
            FillMst(model[0].Col15);
            DataTable dtm = sgen.settable(hftable);
            model[0].dt1 = dtm;
            List<SelectListItem> mod1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
            TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
            if (model[0].SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
            var tm = model.FirstOrDefault();
            DataTable dt = new DataTable();
            if (command == "New")
            {
                model = getnew(model);
                model[0].Col17 = sgen.server_datetime_local(userCode);
                mod1 = new List<SelectListItem>();
                if (model[0].Col12.Trim().Equals("11") || model[0].Col12.Trim().Equals("21"))
                {
                    mq = "select nvl(a.master_name,'-')||'('||cm.cpname||')' as bank_name,cm.cpname as bank_ac,cm.vch_num as bankid from clients_mst cm " +
                        "inner join master_setting a on a.master_id = cm.ptype and a.type = 'ABD' and find_in_set(a.client_unit_id,cm.client_unit_id)=1 " +
                        "where cm.type = 'BAD' and find_in_set(cm.client_unit_id, '" + unitid_mst + "')=1";
                }
                else
                {
                    mq = "select C_NAME AS bank_name,cm.vch_num as bankid from clients_mst cm where SUBSTR(cm.VCH_NUM,0,3) = '301' and " +
                        "find_in_set(cm.client_unit_id, '" + unitid_mst + "')=1";
                }
                dt = new DataTable();
                dt = sgen.getdata(userCode, mq);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        mod1.Add(new SelectListItem { Text = item["bank_name"].ToString(), Value = item["bankid"].ToString() });
                    }
                }
                TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
                model[0].SelectedItems1 = new string[] { "" };
            }
            else if (command == "Cancel")
            {
                return CancelFun(model);
            }
            else if (command == "Sync")
            {
                bool res = false;
                dt = new DataTable();
                mq = "select a.*,b.c_name as tally_name from vouchers a inner join CLIENTS_MST b on a.acode=b.vch_num and FIND_IN_SET(a.client_unit_id,b.client_unit_id)=1 ";
                dt = sgen.getdata(userCode, mq);
                res = sgen.Tally_Sync(sgen.JV_Multi_2("##SVCURRENTCOMPANY", "Journal", dt));
                if (res)
                {
                    ViewBag.vnew = "";
                    ViewBag.vedit = "";
                    ViewBag.vsave = "disabled='disabled'";
                    ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Synced Successfully');disableForm();";
                }
            }
            else if (command == "Callback")
            {
                model = StartCallback(model);
                if (btnval == "PRINT")
                {
                    ViewBag.vnew = "";
                    ViewBag.vedit = "";
                    ViewBag.vsave = "disabled='disabled'";
                    ViewBag.vsavenew = "disabled='disabled'";
                }
            }
            else if (command == "Save" || command == "Update")
            {
                try
                {
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime(userCode);

                    if (model[0].dt1.Rows[0]["acode"].ToString().Trim().Equals("-"))
                    {
                        ViewBag.scripCall += "showmsgJS(1,'Please Select Ledger in grid-1', 2);";
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                        return View(model);
                    }
                    if (model[0].dt1.Rows[0]["adj_type"].ToString().Trim().Equals("") || model[0].dt1.Rows[0]["adj_type_code"].ToString().Trim().Equals("-"))
                    {
                        ViewBag.scripCall += "showmsgJS(1,'Please Select Method Of Adjustment in grid-1', 2);";
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                        return View(model);
                    }
                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        isedit = true;
                        vch_num = model[0].Col16;
                    }
                    else
                    {
                        isedit = false;
                        mq = "select max(to_number(vch_num)) as maxno from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "maxno");
                        model[0].Col16 = vch_num;
                    }
                    decimal cr = 0; decimal drr = 0;
                    DataTable dataTable = sgen.getdata(userCode, "select * from " + model[0].Col10.Trim() + " where  1=2");
                    if (model[0].Col12 == "11" || model[0].Col12 == "10")
                    {
                        cr = model[0].dt1.AsEnumerable().Sum(x => sgen.Make_decimal(x["CRAMOUNT"].ToString())) + model.AsEnumerable().Sum(x => sgen.Make_decimal(x.Col27));
                        drr = model.AsEnumerable().Sum(x => sgen.Make_decimal(x.Col26));
                    }
                    else
                    {
                        drr = model[0].dt1.AsEnumerable().Sum(x => sgen.Make_decimal(x["DRAMOUNT"].ToString())) + model.AsEnumerable().Sum(x => sgen.Make_decimal(x.Col27));
                        cr = model.AsEnumerable().Sum(x => sgen.Make_decimal(x.Col26));
                    }
                    if (cr != drr)
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'Cr Amount Should be Equal To Dr Amount ,Please Check', 2);";
                        ModelState.Clear();
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                        return View(model);
                    }
                    //string cr_rcode = "";
                    //string dr_rcode = "";
                    //for (int j = 0; j < model[0].dt1.Rows.Count; j++)
                    //{
                    //    if (model[0].dt1.Rows[j][2].ToString() != "")
                    //    {
                    //        if (sgen.Make_decimal(model[0].dt1.Rows[j][4].ToString()) > 0)
                    //        {
                    //            if (cr_rcode == "") cr_rcode = model[0].dt1.Rows[j][2].ToString();///this is for debit row
                    //        }
                    //        else
                    //        {
                    //            dr_rcode = model[0].dt1.Rows[j][2].ToString();///this is for credit row
                    //        }
                    //    }
                    //}
                    string ledgername = ((List<SelectListItem>)TempData[MyGuid + "_TList1"]).SingleOrDefault(item => item.Value == model[0].SelectedItems1.FirstOrDefault().ToString()).Text.ToString();

                    //--------bank
                    DataRow dr = dataTable.NewRow();
                    dr["vch_num"] = vch_num;
                    dr["rno"] = "0";
                    dr["type"] = model[0].Col12;
                    dr["vch_date"] = sgen.Make_date_S(model[0].Col17);
                    dr["totremark"] = model[0].Col40;
                    dr["acode"] = model[0].SelectedItems1.FirstOrDefault();
                    dr["aname"] = ledgername;
                    dr["chqamt"] = model[0].Col26;
                    dr["reftype"] = (model[0].Col12 == "11" || model[0].Col12 == "21") ? "BNK" : "CSH";
                    dr["rvscode"] = model[0].dt1.Rows[0]["Acode"].ToString();//party reverse code
                    dr["dramount"] = (model[0].Col12 == "11" || model[0].Col12 == "10") ? model[0].Col26 : "0";
                    dr["cramount"] = (model[0].Col12 == "21" || model[0].Col12 == "20") ? model[0].Col26 : "0";
                    dr["adj_type"] = model[0].dt1.Rows[0]["Adj_type_code"].ToString();
                    dr["invno"] = model[0].dt1.Rows[0]["Ref_no"].ToString();
                    dr["remark"] = model[0].dt1.Rows[0]["Remark"].ToString();
                    dr["invdate"] = sgen.Make_date_S(model[0].dt1.Rows[0]["Ref_date"].ToString());
                    dr = getsave(currdate, dr, model);
                    dataTable.Rows.Add(dr);
                    //--------advance
                    if (model.AsEnumerable().Sum(x => sgen.Make_decimal(x.Col27)) > 0)
                    {
                        dr = dataTable.NewRow();
                        dr["vch_num"] = vch_num;
                        dr["rno"] = "0";
                        dr["type"] = model[0].Col12;
                        dr["vch_date"] = sgen.Make_date_S(model[0].Col17);
                        dr["totremark"] = model[0].Col40;
                        dr["acode"] = model[0].dt1.Rows[0]["Acode"].ToString();
                        dr["aname"] = model[0].dt1.Rows[0]["Aname"].ToString();
                        dr["chqamt"] = model[0].Col26;
                        dr["reftype"] = "ADV";
                        dr["rvscode"] = model[0].SelectedItems1.FirstOrDefault();//bank reverse code
                        dr["cramount"] = (model[0].Col12 == "11" || model[0].Col12 == "10") ? model[0].Col27 : "0";
                        dr["dramount"] = (model[0].Col12 == "21" || model[0].Col12 == "20") ? model[0].Col27 : "0";
                        dr["adj_type"] = model[0].dt1.Rows[0]["Adj_type_code"].ToString();
                        dr["invno"] = model[0].dt1.Rows[0]["Ref_no"].ToString();
                        dr["invdate"] = sgen.Make_date_S(model[0].dt1.Rows[0]["Ref_date"].ToString());
                        dr["remark"] = model[0].dt1.Rows[0]["Remark"].ToString();
                        dr = getsave(currdate, dr, model);
                        dataTable.Rows.Add(dr);
                    }
                    //-------------BILL
                    for (int i = 0; i < model[0].dt1.Rows.Count; i++)
                    {
                        dr = dataTable.NewRow();
                        dr["rno"] = i.ToString();
                        dr["vch_num"] = vch_num;
                        dr["type"] = model[0].Col12;
                        dr["vch_date"] = currdate;
                        dr["invno"] = model[0].dt1.Rows[i]["Ref_no"].ToString();
                        dr["invdate"] = sgen.Make_date_S(model[0].dt1.Rows[0]["Ref_date"].ToString());
                        dr["totremark"] = model[0].Col40;
                        dr["rvscode"] = model[0].SelectedItems1.FirstOrDefault();
                        dr["acode"] = model[0].dt1.Rows[i]["Acode"].ToString();
                        dr["aname"] = model[0].dt1.Rows[i]["Aname"].ToString();
                        dr["chqamt"] = model[0].Col26;
                        dr["reftype"] = "PTY";
                        dr["rvscode"] = model[0].SelectedItems1.FirstOrDefault();//bank reverse code
                        dr["cramount"] = (model[0].Col12 == "11" || model[0].Col12 == "10") ? model[0].dt1.Rows[i]["cramount"].ToString() : "0";
                        dr["dramount"] = (model[0].Col12 == "21" || model[0].Col12 == "20") ? model[0].dt1.Rows[i]["dramount"].ToString() : "0";
                        dr["adj_type"] = model[0].dt1.Rows[i]["Adj_type_code"].ToString();
                        dr["remark"] = model[0].dt1.Rows[i]["Remark"].ToString();
                        dr = getsave(currdate, dr, model);
                        dataTable.Rows.Add(dr);
                    }
                    bool Result = sgen.Update_data_fast1(userCode, dataTable, model[0].Col10.Trim(), tmodel.Col8, isedit);
                    if (Result == true)
                    {
                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                        dt = (DataTable)sgen.GetSession(MyGuid, "SALVOU");
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col13 = "Save",
                            Col100 = tmodel.Col100,
                            Col9 = tmodel.Col9,
                            Col10 = tmodel.Col10,
                            Col11 = tmodel.Col11,
                            Col12 = tmodel.Col12,
                            Col14 = tmodel.Col14,
                            Col15 = tmodel.Col15,
                            dt1 = dt
                        });
                    }
                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
            }
            else if (command == "+")
            {
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
                ViewBag.vsavenew = "";
                if (model[0].dt1.Rows.Count != 0) model[0].dt1.Rows.Add(sgen.Make_int(hfrow));
                else { model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "SALVOU"); }
            }
            else if (command == "-")
            {
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
                ViewBag.vsavenew = "";
                if (model[0].dt1.Rows.Count > 1) model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                else { model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "SALVOU"); }
            }

            ModelState.Clear();
            return View(model);
        }
        #endregion
        #region cash/rec new
        public ActionResult cas_recn()
        {
            FillMst("");
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col10 = "vouchers";
            model[0].Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            switch (model[0].Col14)
            {
                case "22003.1":
                    model[0].Col12 = "10";
                    model[0].Col9 = "CASH RECIEPT VOUCHER";
                    break;
                case "22003.2":
                    model[0].Col12 = "20";
                    model[0].Col9 = "CASH PAYMENT VOUCHER";
                    break;
                case "22003.7":
                    model[0].Col12 = "50";
                    model[0].Col9 = "PURCHASE VOUCHER";
                    break;
                case "22003.8":
                    model[0].Col12 = "40";
                    model[0].Col9 = "SALE VOUCHER";
                    break;
                case "22003.3":
                    model[0].Col12 = "11";
                    model[0].Col9 = "BANK RECIEPT VOUCHER";
                    break;
                case "22003.4":
                    model[0].Col12 = "21";
                    model[0].Col9 = "BANK PAYMENT VOUCHER";
                    break;
                case "22003.5":
                    model[0].Col12 = "30";
                    model[0].Col9 = "JOURNAL VOUCHER";
                    break;
                case "22003.10":
                    model[0].Col12 = "32";
                    model[0].Col9 = "CREDIT NOTE";
                    break;
                case "22003.9":
                    model[0].Col12 = "31";
                    model[0].Col9 = "DEBIT NOTE";
                    break;
                case "22003.6":
                    model[0].Col12 = "33";
                    model[0].Col9 = "CONTRA VOUCHER";
                    break;
                default:
                    model[0].Col12 = "VOU";
                    model[0].Col9 = "VOUCHER ENTRY";
                    break;
            }
            List<SelectListItem> md1 = new List<SelectListItem>();
            model[0].Col50 = "1";
            TempData[MyGuid + "_TList1"] = model[0].TList1 = md1;
            TempData[MyGuid + "_TList2"] = model[0].TList2 = md1;
            TempData[MyGuid + "_TList2"] = model[0].TList3 = md1;
            model[0].SelectedItems1 = new string[] { "" };
            model[0].SelectedItems2 = new string[] { "" };
            model[0].SelectedItems2 = new string[] { "" };
            model[0].Col29 = "Y";
            model[0].LTM1 = new List<Tmodelmain>();
            model[0].Files1 = new List<Files>();
            Files tm3 = new Files();
            tm3.FileName = "-";
            model[0].Files1.Add(tm3);
            model[0].Files1.Add(tm3);
            if (Request.QueryString["vid"] != null)
            {
                string fstr = EncryptDecrypt.Decrypt(Request.QueryString["vid"].ToString().Trim());
                sgen.SetSession(MyGuid, "SSEEKVAL", fstr);
                model[0].Col12 = fstr.Substring(fstr.Length - 2);
                model = CallbackFun("EDIT", "Y", "cas_recn", "Account", model);
                switch (model[0].Col12)
                {
                    case "33":
                        model[0].Col9 = "CONTRA VOUCHER";
                        break;
                    case "11":
                        model[0].Col9 = "BANK RECIEPT";
                        break;
                    case "10":
                        model[0].Col9 = "CASH RECIEPT";
                        break;
                    case "20":
                        model[0].Col9 = "CASH PAYMENT";
                        break;
                    case "21":
                        model[0].Col9 = "BANK PAYMENT";
                        break;
                    case "32":
                        model[0].Col9 = "CREDIT NOTE";
                        break;
                    case "31":
                        model[0].Col9 = "DEBIT NOTE";
                        break;
                    case "30":
                        model[0].Col9 = "JOURNAL VOUCHER";
                        break;
                    case "40":
                        model[0].Col9 = "SALE VOUCHER";
                        break;
                    case "50":
                    case "01":
                    case "02":
                    case "03":
                        model[0].Col9 = "PURCHASE VOUCHER";
                        break;
                    default:
                        model[0].Col9 = "VOUCHER ENTRY";
                        break;
                }
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
                ViewBag.vsavenew = "";
                ViewBag.scripCall = "enableForm();";
            }
            sgen.SetSession(MyGuid, "SALTYPE", model[0].Col12);
            return View(model);
        }

        [HttpPost]
        public ActionResult cas_recn(List<Tmodelmain> model, string modelstr, string command, string hfrow)
        {
            try
            {
                if (model[0].Files1 != null)
                {
                    for (int f = 0; f < model[0].Files1.Count; f++)
                    {
                        try
                        {
                            string base64String = "", fname = "", ftype = "";
                            var fi = model[0].Files1[f].PostedFile;
                            if (Request.Files["[0].Files1[" + f + "].PostedFile"] != null &&
                                Request.Files["[0].Files1[" + f + "].PostedFile"].ContentLength > 100)
                            {
                                fi = Request.Files["[0].Files1[" + f + "].PostedFile"];

                                Stream fs = fi.InputStream;
                                BinaryReader br = new BinaryReader(fs);
                                Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                                base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                                fname = fi.FileName;
                                ftype = fi.ContentType;
                                model[0].Files1[f].FileBase64 = base64String;
                                model[0].Files1[f].FileName = fname;
                                model[0].Files1[f].ContentType = ftype;
                            }
                        }
                        catch (Exception ex)
                        {
                            sgen.SetSession(MyGuid, actionName + f, null);
                        }
                    }
                }
            }
            catch (Exception err) { }
            List<Tmodelmain> model1 = new List<Tmodelmain>();

            if (modelstr != null)
            {
                model1 = sgen.Make_Model(modelstr);
                model1[0].Files1 = model[0].Files1;
                model = model1;
            }

            FillMst(model[0].Col15);
            var tm = model.FirstOrDefault();
            List<SelectListItem> mod1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
            List<SelectListItem> mod2 = (List<SelectListItem>)TempData[MyGuid + "_TList2"];
            List<SelectListItem> mod3 = (List<SelectListItem>)TempData[MyGuid + "_TList3"];
            TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
            if (model[0].SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
            foreach (var mod in model)
            {
                TempData[MyGuid + "_TList2"] = mod.TList2 = mod2;
                TempData[MyGuid + "_TList3"] = mod.TList3 = mod3;
                if (mod.SelectedItems2 == null) mod.SelectedItems2 = new string[] { "" };
                if (mod.SelectedItems3 == null) mod.SelectedItems3 = new string[] { "" };
            }
            DataTable dt = new DataTable();
            if (command == "New")
            {
                model = getnew(model);
                model[0].Col17 = sgen.server_datetime_local(userCode);
                mod1 = new List<SelectListItem>();
                mod2 = new List<SelectListItem>();
                mod3 = new List<SelectListItem>();
                Tmodelmain tm1 = new Tmodelmain();
                if (model[0].Col12.Trim().Equals("11") || model[0].Col12.Trim().Equals("21"))
                {
                    mq = "select nvl(a.master_name,'-')||'('||cm.cpname||')' as bank_name,cm.cpname as bank_ac,cm.vch_num as bankid from clients_mst cm " +
                        "inner join master_setting a on a.master_id = cm.ptype and a.type = 'ABD' and find_in_set(a.client_unit_id,cm.client_unit_id)=1 " +
                        "where cm.type = 'BAD' and find_in_set(cm.client_unit_id, '" + unitid_mst + "')=1";
                }
                else
                {
                    mq = "select C_NAME AS bank_name,cm.vch_num as bankid from clients_mst cm where SUBSTR(cm.VCH_NUM,0,3) = '301' and " +
                        "find_in_set(cm.client_unit_id, '" + unitid_mst + "')=1";
                }
                dt = new DataTable();
                dt = sgen.getdata(userCode, mq);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        mod1.Add(new SelectListItem { Text = item["bank_name"].ToString(), Value = item["bankid"].ToString() });
                    }
                }
                TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
                model[0].SelectedItems1 = new string[] { "" };
                foreach (var mod in model)
                {
                    if (mod.SelectedItems2 == null) mod.SelectedItems2 = new string[] { "" };
                    if (mod.SelectedItems3 == null) mod.SelectedItems3 = new string[] { "" };
                    mq1 = "select a.* from (select vch_num as acode,c_name as aname from clients_mst where substr(vch_num,0,3) in ('203','303') and " +
              "find_in_set(client_unit_id, '" + unitid_mst + "')=1 union all select vch_num as acode ,c_name as aname from clients_mst where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "')=1 union all select cm.vch_num as acode,nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname from master_setting a inner join clients_mst cm on a.master_id = cm.ptype and a.type = 'ABD' and find_in_set(a.client_unit_id, cm.client_unit_id)=1 where cm.type = 'BAD' " +
              "and find_in_set(cm.client_unit_id,'" + unitid_mst + "')=1) a where a.aname<>'0'";
                    dt = sgen.getdata(userCode, mq1);
                    if (dt.Rows.Count > 0)
                    {
                        var mymod = sgen.dt_to_selectlist(dt);
                        TempData[MyGuid + "_TList2"] = mod.TList2 = mymod;
                    }
                    mq1 = "select 'ADV' as Adj_type_code,'Advance' as Adj_type from dual union all" +
                " select 'AGR' as Adj_type_code,'Against Ref' as Adj_type from dual union all select 'NEW' as Adj_type_code,'New Ref' as Adj_type from dual union all select 'OAC' as Adj_type_code,'On Account' as Adj_type from dual";
                    dt = sgen.getdata(userCode, mq1);
                    if (dt.Rows.Count > 0)
                    {
                        var mymod = sgen.dt_to_selectlist(dt);
                        TempData[MyGuid + "_TList3"] = mod.TList3 = mymod;
                    }
                }
                try
                {
                    model[0].Files1 = new List<Files>();
                    Files ltm = new Files();
                    ltm.FileName = "-";
                    model[0].Files1.Add(ltm);
                    model[0].Files1.Add(ltm);
                }
                catch (Exception ex) { }
            }
            if (command == "Mode")
            {
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
                ViewBag.vsavenew = "";
                int index = sgen.Make_int(hfrow);
                if (model[index].SelectedItems2 == null) model[index].SelectedItems2 = new string[] { "" };
                if (model[index].SelectedItems3 == null) model[index].SelectedItems3 = new string[] { "" };
                mq = "";
                if (model[index].SelectedItems2.FirstOrDefault() != "")
                {
                    var acode = model[index].SelectedItems2.FirstOrDefault();
                    var ddlmethod = model[index].SelectedItems3.FirstOrDefault();
                    model[index].LTM1 = new List<Tmodelmain>();
                    if (ddlmethod == "AGR")
                    {
                        mq = "select a.acode,a.doc_no,to_char(a.doc_date,'dd/mm/yyyy') as doc_date,sum(a.amt) as bal_qty,max(a.billamt) as billamt,a.client_unit_id from (select i.acode,i.INVNO as doc_no,i.INVDATE doc_date," +
                            "to_number(i.netamt) as billamt,0 as amt, i.client_id, i.client_unit_id, i.type from itransaction i where i.type in ('01', '02', '03') " +
                            "and client_unit_id = '" + unitid_mst + "' union all select i.acode, i.vch_num as doc_no,i.vch_date doc_date, to_number(i.netamt) as billamt,0 as amt," +
                            "i.client_id, i.client_unit_id, i.type from itransaction i where substr(i.type,1,1)= '4' and to_number(i.netamt) > 0 and client_unit_id = '" + unitid_mst + "' union all " +
                            "select i.acode,i.invno,i.invdate doc_date,(to_number(i.dramount) - to_number(i.cramount)) as billamt,(to_number(i.dramount) - to_number(i.cramount)) as amt, i.client_id, i.client_unit_id, i.type from " +
                            "vouchers i where client_unit_id = '" + unitid_mst + "' AND i.adj_type in ('NEW','AGR')) a where a.acode = '" + acode + "' group by a.acode, a.doc_no,to_char(a.doc_date,'dd/mm/yyyy') ,a.client_unit_id ";

                        DataTable dtques = new DataTable();
                        dtques = sgen.getdata(userCode, mq);
                        if (dtques.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtques.Rows)
                            {
                                Tmodelmain titu = new Tmodelmain();
                                titu.Col11 = dr["doc_no"].ToString();
                                titu.Col12 = dr["doc_date"].ToString();
                                titu.Col13 = dr["billamt"].ToString();
                                titu.Col15 = dr["bal_qty"].ToString();
                                model[index].LTM1.Add(titu);
                            }
                        }
                    }
                    if (ddlmethod == "NEW")
                    {
                        Tmodelmain titu = new Tmodelmain();
                        titu.Col11 = "-";
                        titu.Col12 = "";
                        model[index].LTM1.Add(titu);
                    }
                }
            }
            else if (command == "+")
            {
                try
                {
                    Tmodelmain ntm = new Tmodelmain();
                    ntm.Col50 = (Convert.ToInt32(model.Max(x => sgen.Make_int(x.Col50))) + 1).ToString();
                    TempData[MyGuid + "_TList2"] = ntm.TList2 = mod2 = (List<SelectListItem>)TempData[MyGuid + "_TList2"];
                    TempData[MyGuid + "_TList3"] = ntm.TList3 = mod3 = (List<SelectListItem>)TempData[MyGuid + "_TList3"];
                    ntm.LTM1 = new List<Tmodelmain>();
                    model.Add(ntm);
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "',0);"; }
            }
            else if (command == "Back")
            {
                try
                {
                    return RedirectToAction("gst_rpt", "Account", new { @m_id = EncryptDecrypt.Encrypt(model[0].Col15), @mid = EncryptDecrypt.Encrypt(model[0].Col14) });
                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "',0);"; }
            }
            else if (command == "x")
            {
                if (model.Count > 1) model.RemoveAt(sgen.Make_int(hfrow));
                else
                {
                    ViewBag.scripCall += "showmsgJS(1,'You cannot remove last entry',2)";
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    return View(model);
                }
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
            }
            else if (command == "Cancel")
            {
                return CancelFun(model);
            }
            else if (command == "Sync")
            {
                bool res = false;
                dt = new DataTable();
                mq = "select a.*,b.c_name as tally_name from vouchers a inner join CLIENTS_MST b on a.acode=b.vch_num and FIND_IN_SET(a.client_unit_id,b.client_unit_id)=1 ";
                dt = sgen.getdata(userCode, mq);
                res = sgen.Tally_Sync(sgen.JV_Multi_2("##SVCURRENTCOMPANY", "Journal", dt));
                if (res)
                {
                    ViewBag.vnew = "";
                    ViewBag.vedit = "";
                    ViewBag.vsave = "disabled='disabled'";
                    ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Synced Successfully');disableForm();";
                }
            }
            else if (command == "Callback")
            {
                model = StartCallback(model);
                if (btnval == "PRINT")
                {
                    ViewBag.vnew = "";
                    ViewBag.vedit = "";
                    ViewBag.vsave = "disabled='disabled'";
                    ViewBag.vsavenew = "disabled='disabled'";
                }
            }
            else if (command == "Save" || command == "Update")
            {
                try
                {
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
                        mq = "select max(to_number(vch_num)) as maxno from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "maxno");
                        model[0].Col16 = vch_num;
                    }
                    decimal cramt = 0; decimal dramt = 0;
                    dramt = model.AsEnumerable().Sum(x => sgen.Make_decimal(x.Col30));
                    cramt = model.AsEnumerable().Sum(x => sgen.Make_decimal(x.Col31));
                    DataTable dataTable = sgen.getdata(userCode, "select * from " + model[0].Col10.Trim() + " where  1=2");
                    if (model[0].Col12 == "11" || model[0].Col12 == "10") dramt += model.AsEnumerable().Sum(x => sgen.Make_decimal(x.Col26));
                    else cramt += model.AsEnumerable().Sum(x => sgen.Make_decimal(x.Col26));
                    if ((cramt + dramt) <= 0)
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'Please Fill Out Cr And Dr Amounts !!', 2);";
                        ModelState.Clear();
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                        return View(model);
                    }
                    if (cramt != dramt)
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'Cr Amount Should be Equal To Dr Amount ,Please Check !!', 2);";
                        ModelState.Clear();
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                        return View(model);
                    }
                    string ledger = ((List<SelectListItem>)TempData[MyGuid + "_TList2"]).SingleOrDefault(item => item.Value == model[0].SelectedItems2.FirstOrDefault().ToString()).Text.ToString();
                    //---------------------------------------------------------------------bank
                    DataRow dr;
                    string cr_rcode = "";
                    string dr_rcode = "";
                    if (model[0].Col12 == "10" || model[0].Col12 == "11" || model[0].Col12 == "20" || model[0].Col12 == "21")
                    {
                        string bankcash = ((List<SelectListItem>)TempData[MyGuid + "_TList1"]).SingleOrDefault(item => item.Value == model[0].SelectedItems1.FirstOrDefault().ToString()).Text.ToString();
                        dr = dataTable.NewRow();
                        dr["vch_num"] = vch_num;
                        dr["rno"] = "0";
                        dr["type"] = model[0].Col12;
                        dr["vch_date"] = sgen.Make_date_S(model[0].Col17);
                        dr["totremark"] = model[0].Col40;
                        dr["acode"] = model[0].SelectedItems1.FirstOrDefault();
                        dr["aname"] = bankcash;
                        dr["chqamt"] = model[0].Col26;
                        dr["reftype"] = (model[0].Col12 == "11" || model[0].Col12 == "21") ? "BNK" : "CSH";
                        dr["rvscode"] = model[0].SelectedItems2.FirstOrDefault();//party reverse code
                        dr["dramount"] = (model[0].Col12 == "11" || model[0].Col12 == "10") ? model[0].Col26 : "0";
                        dr["cramount"] = (model[0].Col12 == "21" || model[0].Col12 == "20") ? model[0].Col26 : "0";
                        dr["adj_type"] = model[0].SelectedItems3.FirstOrDefault();
                        dr = getsave(currdate, dr, model);
                        dataTable.Rows.Add(dr);
                    }
                    else
                    {
                        for (int j = 0; j < model.Count; j++)
                        {
                            if (model[j].SelectedItems2.FirstOrDefault() != "")
                            {
                                if (sgen.Make_decimal(model[j].Col30) > 0)
                                {
                                    if (cr_rcode == "") cr_rcode = model[j].SelectedItems2.FirstOrDefault();///this is for debit row
                                }
                                else
                                {
                                    dr_rcode = model[j].SelectedItems2.FirstOrDefault();///this is for credit row
                                }
                            }
                        }
                    }
                    //-------------BILL
                    for (int i = 0; i < model.Count; i++)
                    {
                        ledger = ((List<SelectListItem>)TempData[MyGuid + "_TList2"]).SingleOrDefault(item => item.Value == model[i].SelectedItems2.FirstOrDefault().ToString()).Text.ToString();
                        if (model[i].SelectedItems3.FirstOrDefault() != "AGR" && model[i].SelectedItems3.FirstOrDefault() != "NEW")
                        {
                            if ((sgen.Make_decimal(model[i].Col31) + sgen.Make_decimal(model[i].Col30)) > 0)
                            {
                                dr = dataTable.NewRow();
                                dr["rno"] = (i + 1).ToString();
                                dr["prno"] = "0";
                                dr["vch_num"] = vch_num;
                                dr["type"] = model[0].Col12;
                                dr["vch_date"] = sgen.Make_date_S(model[0].Col17);
                                dr["totremark"] = model[0].Col40;
                                dr["acode"] = model[i].SelectedItems2.FirstOrDefault();
                                dr["aname"] = ledger;
                                dr["chqamt"] = model[0].Col26;
                                dr["reftype"] = model[i].SelectedItems3.FirstOrDefault();
                                if (model[0].Col12 == "10" || model[0].Col12 == "11" || model[0].Col12 == "20" || model[0].Col12 == "21") dr["rvscode"] = model[0].SelectedItems1.FirstOrDefault();//bank reverse code
                                else
                                {
                                    if (sgen.Make_decimal(model[i].Col30) > 0) dr["rvscode"] = dr_rcode;
                                    else dr["rvscode"] = cr_rcode;
                                }
                                dr["cramount"] = model[i].Col31;
                                dr["dramount"] = model[i].Col30;
                                dr["adj_type"] = model[i].SelectedItems3.FirstOrDefault();
                                dr["remark"] = model[i].Col32;
                                dr = getsave(currdate, dr, model);
                                dataTable.Rows.Add(dr);
                            }
                        }
                        else if (model[i].SelectedItems3.FirstOrDefault() == "NEW")
                        {
                            for (int j = 0; j < model[i].LTM1.Count; j++)
                            {
                                if ((sgen.Make_decimal(model[i].Col31) + sgen.Make_decimal(model[i].Col30)) > 0)
                                {
                                    dr = dataTable.NewRow();
                                    dr["rno"] = (i + 1).ToString();
                                    dr["prno"] = (j + 1).ToString();
                                    dr["prno"] = "0";
                                    dr["vch_num"] = vch_num;
                                    dr["type"] = model[0].Col12;
                                    dr["vch_date"] = sgen.Make_date_S(model[0].Col17);
                                    dr["invno"] = model[i].LTM1[j].Col11;
                                    dr["invdate"] = sgen.Make_date_S(model[i].LTM1[j].Col12);
                                    dr["totremark"] = model[0].Col40;
                                    dr["acode"] = model[i].SelectedItems2.FirstOrDefault();
                                    dr["aname"] = ledger;
                                    dr["chqamt"] = model[0].Col26;
                                    dr["reftype"] = model[i].SelectedItems3.FirstOrDefault();
                                    if (model[0].Col12 == "10" || model[0].Col12 == "11" || model[0].Col12 == "20" || model[0].Col12 == "21") dr["rvscode"] = model[0].SelectedItems1.FirstOrDefault();//bank reverse code
                                    else
                                    {
                                        if (sgen.Make_decimal(model[i].Col30) > 0) dr["rvscode"] = dr_rcode;
                                        else dr["rvscode"] = cr_rcode;
                                    }
                                    dr["cramount"] = model[i].Col31;
                                    dr["dramount"] = model[i].Col30;
                                    dr["adj_type"] = model[i].SelectedItems3.FirstOrDefault();
                                    dr["remark"] = model[i].Col32;
                                    dr = getsave(currdate, dr, model);
                                    dataTable.Rows.Add(dr);
                                }
                            }
                        }
                        else
                        {
                            for (int j = 0; j < model[i].LTM1.Count; j++)
                            {
                                if (model[i].LTM1[j].Col14 != "0" && model[i].LTM1[j].Col14 != "")
                                {
                                    dr = dataTable.NewRow();
                                    dr["rno"] = (i + 1).ToString();
                                    dr["prno"] = (j + 1).ToString();
                                    dr["vch_num"] = vch_num;
                                    dr["type"] = model[0].Col12;
                                    dr["vch_date"] = sgen.Make_date_S(model[0].Col17);
                                    dr["invno"] = model[i].LTM1[j].Col11;
                                    dr["invdate"] = sgen.Make_date_S(model[i].LTM1[j].Col12);
                                    dr["totremark"] = model[0].Col40;
                                    dr["acode"] = model[i].SelectedItems2.FirstOrDefault();
                                    dr["aname"] = ledger;
                                    dr["chqamt"] = model[0].Col26;
                                    dr["invamt"] = model[i].LTM1[j].Col13;
                                    dr["BALAMT"] = model[i].LTM1[j].Col15;
                                    if (model[0].Col12.Substring(0, 1) == "4")
                                    {
                                        dr["BALAMT"] = model[i].LTM1[j].Col15.Contains("-") ? model[i].LTM1[j].Col15.Split('-')[1].ToString() : model[i].LTM1[j].Col15;
                                    }
                                    dr["reftype"] = model[i].SelectedItems3.FirstOrDefault();
                                    dr["rvscode"] = model[0].SelectedItems1.FirstOrDefault();//bank reverse code
                                    dr["cramount"] = model[i].LTM1[j].Col15.StartsWith("-") ? "0" : model[i].LTM1[j].Col14;
                                    dr["dramount"] = model[i].LTM1[j].Col15.StartsWith("-") ? model[i].LTM1[j].Col14 : "0";
                                    dr["adj_type"] = model[i].SelectedItems3.FirstOrDefault();
                                    dr["remark"] = model[i].Col32;
                                    dr = getsave(currdate, dr, model);
                                    dataTable.Rows.Add(dr);
                                }
                            }
                        }
                    }
                    Satransaction sat = new Satransaction(userCode, MyGuid);
                    bool Result = sgen.Update_data_fast1_uncommit(userCode, dataTable, model[0].Col10.Trim(), tmodel.Col8, isedit, sat);
                    if (Result == true)
                    {
                        #region attachment
                        string serverpath = Server.MapPath("~/Uploads/" + cg_com_name.Replace(" ", "") + "/");
                        var dtf = sgen.getdata(userCode, "select * from file_tab where " + tmodel.Col8 + "");
                        List<Tmodel> filenames = new List<Tmodel>();
                        foreach (DataRow drr in dtf.Rows)
                        {
                            try
                            {
                                var fileNameOnly = drr["file_path"].ToString();
                                var fullPath = serverpath + drr["file_path"].ToString();
                                string path = Path.GetDirectoryName(fullPath);
                                var newname = "temp_" + fileNameOnly;
                                System.IO.File.Move(fullPath, path + newname);
                                Tmodel tmf = new Tmodel();
                                tmf.Col1 = path;
                                tmf.Col2 = fileNameOnly;
                                tmf.Col3 = newname;
                                filenames.Add(tmf);

                            }
                            catch (Exception err) { }

                        }
                        sat.Execute_cmd("delete from file_tab where " + tmodel.Col8 + "");

                        if (model[0].Files1 != null)
                        {
                            for (int f = 0; f < model[0].Files1.Count; f++)
                            {
                                try
                                {
                                    if (model[0].Files1[f] != null)
                                    {

                                        string fname = model[0].Files1[f].FileName;
                                        string ftype = model[0].Files1[f].ContentType;
                                        string base64String = model[0].Files1[f].FileBase64;
                                        if (base64String != null && base64String.Trim().Length > 100)
                                        {
                                            ctype1 = ftype;
                                            fileName1 = fname;
                                            path1 = clientid_mst + unitid_mst + userid_mst + currdate + "_" + fileName1;
                                            encpath1 = sgen.Convert_Stringto64(path1).ToString();
                                            finalpath1 = serverpath + encpath1;
                                            System.IO.File.WriteAllBytes(finalpath1, Convert.FromBase64String(base64String));

                                            DataTable dtfile = cmd_Fun.GetStructure(userCode, "file_tab");
                                            filesave(model, sgen.Make_date_S(model[0].Col17), dtfile, fileName1, encpath1, "ACC", ctype1, model[0].Col42, model[0].Col41, "", "");
                                            Result = sgen.Update_data_fast1_uncommit(userCode, dtfile, "file_tab", tmodel.Col8, isedit, sat);
                                            if (!Result)
                                            {
                                                sat.Rollback();
                                                break;
                                            }
                                        }
                                    }
                                }
                                catch (Exception err)
                                {
                                    sat.Rollback();
                                    break;
                                }
                            }
                        }

                        #endregion
                        if (Result)
                        {
                            sat.Commit();
                            foreach (var el in filenames)
                            {
                                System.IO.File.Delete(el.Col1 + el.Col3);
                            }
                        }
                        else
                        {
                            foreach (var el in filenames)
                            {
                                System.IO.File.Move(el.Col1 + el.Col3, el.Col1 + el.Col2);
                            }
                        }


                        string url = "";
                        string date = sgen.SaveDate_dt(tmodel.Col17, false).ToString("yyyyMMdd");
                        url = unitid_mst + vch_num + date + "" + model[0].Col12 + "";
                        sgen.SetSession(MyGuid, "SSEEKVAL", url);
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
                            TList1 = tmodel.TList1,
                            SelectedItems1 = new string[] { "" },
                            TList2 = tmodel.TList2,
                            SelectedItems2 = new string[] { "" },
                            TList3 = tmodel.TList3,
                            SelectedItems3 = new string[] { "" },
                            LTM1 = new List<Tmodelmain>(),
                        });
                        model[0].Files1 = new List<Files>();
                        Files tm3 = new Files();
                        tm3.FileName = "-";
                        model[0].Files1.Add(tm3);
                        model[0].Files1.Add(tm3);
                        if (command.Trim().ToUpper().Equals("SAVE") || command.Trim().ToUpper().Equals("UPDATE"))
                        {
                            ViewBag.vnew = "";
                            ViewBag.vedit = "";
                            ViewBag.vsave = "disabled='disabled'";
                            ViewBag.vsavenew = "disabled='disabled'";
                            ViewBag.scripCall += "showmsgJS(2, 'Data Saved Successfully. Do You Want To Print', 1);disableForm();";
                        }
                    }
                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
            }
            else if (command == "hfbtnyes")
            {
                model = CallbackFun("PRINT", "N", "cas_recn", "Account", model);
                ViewBag.vnew = "";
                ViewBag.vedit = "";
                ViewBag.vsave = "disabled='disabled'";
                ViewBag.vsavenew = "disabled='disabled'";
            }
            ModelState.Clear();
            return View(model);
        }
        #endregion
        #region sadash
        public ActionResult sadash()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "ACCOUNTS DASHBOARD";
            model[0].Col10 = "vouchers";
            model[0].Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            model[0].Col12 = "";
            //string parentid = "000253";

            //mq = "select a.* from (select '01' fstr,'DEBTORS' master_nm from dual union all select '02' fstr,'CREDITORS' master_nm from dual" +
            //    " union all select '03' fstr,'BANK' master_nm from dual union all select '04' fstr,'CASH' master_nm from dual" +
            //    " union all select '05' fstr,'SALE' master_nm from dual union all select '06' fstr,'PURCHASE' master_nm from dual) a where a.fstr not in (select col4 from enx_tab where type='TGM' and client_unit_id='" + unitid_mst + "')";
            mq = "SELECT MASTER_ID,MASTER_NAME FROM MASTER_SETTING WHERE TYPE='TAC'";
            DataTable dt = new DataTable();
            dt = sgen.getdata(userCode, mq);
            if (dt.Rows.Count > 0)
            {
                try { model[0].Col22 = dt.Select("master_id='1'")[0].ItemArray[0].ToString(); }
                catch (Exception ex) { model[0].Col22 = ""; }
                try { model[0].Col23 = dt.Select("master_id='2'")[0].ItemArray[0].ToString(); }
                catch (Exception ex) { model[0].Col23 = ""; }
                try { model[0].Col24 = dt.Select("master_id='3'")[0].ItemArray[0].ToString(); }
                catch (Exception ex) { model[0].Col24 = ""; }
                try { model[0].Col25 = dt.Select("master_id='4'")[0].ItemArray[0].ToString(); }
                catch (Exception ex) { model[0].Col25 = ""; }
                try { model[0].Col26 = dt.Select("master_id='5'")[0].ItemArray[0].ToString(); }
                catch (Exception ex) { model[0].Col26 = ""; }
                try { model[0].Col27 = dt.Select("master_id='6'")[0].ItemArray[0].ToString(); }
                catch (Exception ex) { model[0].Col27 = ""; }
            }
            string fdt = "01/04/2020"; string tdt = "31/03/2021";
            mq = "select sum(v.closing) closing from master_setting m inner join (select client_unit_id, acode, sum(op) op, sum(dramount) dramount, sum(cramount) cramount, (sum(op) + sum(dramount) - sum(cramount)) closing from " +
           "(select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, OP_" + Ac_Year.Split(' ')[0].Trim() + " as op, 0 as cramount, 0 as dramount," +
           " 0 as cl from acbal union all select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, sum(to_number(dramount)) - sum(to_number(cramount)) as op, 0 as dramount," +
           "0 as cramount, 0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
           "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') - 1 group by client_unit_id, acode " +
           "union all select client_unit_id, acode as acode, vch_num, vch_date, 0 as op, to_number(dramount) dramount, to_number(cramount) cramount, 0 as cl from vouchers " +
           "where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') " +
           "and to_date('" + tdt + "', '" + sgen.Getsqldateformat() + "')) t group by client_unit_id, acode) v on substr(m.master_id,1,3)=substr(v.acode,1,3) " +
           "where m.type = 'AGM' and trim(substr(m.master_id,0,3)) in ('401') and trim(substr(master_type,4,7))='0000' " +
           "group by m.master_name,m.master_id order by m.master_id asc";
            model[0].Col16 = sgen.seekval(userCode, mq, "closing");
            mq = "select sum(v.closing) closing from master_setting m inner join (select client_unit_id, acode, sum(op) op, sum(dramount) dramount, sum(cramount) cramount, (sum(op) + sum(dramount) - sum(cramount)) closing from " +
         "(select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, OP_" + Ac_Year.Split(' ')[0].Trim() + " as op, 0 as cramount, 0 as dramount," +
         " 0 as cl from acbal union all select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, sum(to_number(dramount)) - sum(to_number(cramount)) as op, 0 as dramount," +
         "0 as cramount, 0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
         "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') - 1 group by client_unit_id, acode " +
         "union all select client_unit_id, acode as acode, vch_num, vch_date, 0 as op, to_number(dramount) dramount, to_number(cramount) cramount, 0 as cl from vouchers " +
         "where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') " +
         "and to_date('" + tdt + "', '" + sgen.Getsqldateformat() + "')) t group by client_unit_id, acode) v on substr(m.master_id,1,3)=substr(v.acode,1,3) " +
         "where m.type = 'AGM' and trim(substr(m.master_id,0,3)) in ('501') and trim(substr(master_type,4,7))='0000' " +
         "group by m.master_name,m.master_id order by m.master_id asc";
            model[0].Col17 = sgen.seekval(userCode, mq, "closing");
            mq = "select sum(v.closing) closing from master_setting m inner join (select client_unit_id, acode, sum(op) op, sum(dramount) dramount, sum(cramount) cramount, (sum(op) + sum(dramount) - sum(cramount)) closing from " +
    "(select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, OP_" + Ac_Year.Split(' ')[0].Trim() + " as op, 0 as cramount, 0 as dramount," +
    " 0 as cl from acbal union all select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, sum(to_number(dramount)) - sum(to_number(cramount)) as op, 0 as dramount," +
    "0 as cramount, 0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
    "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') - 1 group by client_unit_id, acode " +
    "union all select client_unit_id, acode as acode, vch_num, vch_date, 0 as op, to_number(dramount) dramount, to_number(cramount) cramount, 0 as cl from vouchers " +
    "where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') " +
    "and to_date('" + tdt + "', '" + sgen.Getsqldateformat() + "')) t group by client_unit_id, acode) v on substr(m.master_id,1,3)=substr(v.acode,1,3) " +
    "where m.type = 'AGM' and trim(substr(m.master_id,0,3)) in ('303') and trim(substr(master_type,4,7))='0000' " +
    "group by m.master_name,m.master_id order by m.master_id asc";
            model[0].Col18 = sgen.seekval(userCode, mq, "closing");
            mq = "select sum(v.closing) closing from master_setting m inner join (select client_unit_id, acode, sum(op) op, sum(dramount) dramount, sum(cramount) cramount, (sum(op) + sum(dramount) - sum(cramount)) closing from " +
  "(select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, OP_" + Ac_Year.Split(' ')[0].Trim() + " as op, 0 as cramount, 0 as dramount," +
  " 0 as cl from acbal union all select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, sum(to_number(dramount)) - sum(to_number(cramount)) as op, 0 as dramount," +
  "0 as cramount, 0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
  "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') - 1 group by client_unit_id, acode " +
  "union all select client_unit_id, acode as acode, vch_num, vch_date, 0 as op, to_number(dramount) dramount, to_number(cramount) cramount, 0 as cl from vouchers " +
  "where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') " +
  "and to_date('" + tdt + "', '" + sgen.Getsqldateformat() + "')) t group by client_unit_id, acode) v on substr(m.master_id,1,3)=substr(v.acode,1,3) " +
  "where m.type = 'AGM' and trim(substr(m.master_id,0,3)) in ('203') and trim(substr(master_type,4,7))='0000' " +
  "group by m.master_name,m.master_id order by m.master_id asc";
            model[0].Col19 = sgen.seekval(userCode, mq, "closing");
            mq = "select sum(v.closing) closing from master_setting m inner join (select client_unit_id, acode, sum(op) op, sum(dramount) dramount, sum(cramount) cramount, (sum(op) + sum(dramount) - sum(cramount)) closing from " +
"(select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, OP_" + Ac_Year.Split(' ')[0].Trim() + " as op, 0 as cramount, 0 as dramount," +
" 0 as cl from acbal union all select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, sum(to_number(dramount)) - sum(to_number(cramount)) as op, 0 as dramount," +
"0 as cramount, 0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
"to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') - 1 group by client_unit_id, acode " +
"union all select client_unit_id, acode as acode, vch_num, vch_date, 0 as op, to_number(dramount) dramount, to_number(cramount) cramount, 0 as cl from vouchers " +
"where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') " +
"and to_date('" + tdt + "', '" + sgen.Getsqldateformat() + "')) t group by client_unit_id, acode) v on substr(m.master_id,1,3)=substr(v.acode,1,3) " +
"where m.type = 'AGM' and trim(substr(m.master_id,0,3)) in ('301') and trim(substr(master_type,4,7))='0000' " +
"group by m.master_name,m.master_id order by m.master_id asc";
            model[0].Col20 = sgen.seekval(userCode, mq, "closing");
            mq = "select sum(v.closing) closing from master_setting m inner join (select client_unit_id, acode, sum(op) op, sum(dramount) dramount, sum(cramount) cramount, (sum(op) + sum(dramount) - sum(cramount)) closing from " +
"(select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, OP_" + Ac_Year.Split(' ')[0].Trim() + " as op, 0 as cramount, 0 as dramount," +
" 0 as cl from acbal union all select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, sum(to_number(dramount)) - sum(to_number(cramount)) as op, 0 as dramount," +
"0 as cramount, 0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
"to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') - 1 group by client_unit_id, acode " +
"union all select client_unit_id, acode as acode, vch_num, vch_date, 0 as op, to_number(dramount) dramount, to_number(cramount) cramount, 0 as cl from vouchers " +
"where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') " +
"and to_date('" + tdt + "', '" + sgen.Getsqldateformat() + "')) t group by client_unit_id, acode) v on substr(m.master_id,1,3)=substr(v.acode,1,3) " +
"where m.type = 'AGM' and trim(substr(m.master_id,0,3)) in ('306') and trim(substr(master_type,4,7))='0000' " +
"group by m.master_name,m.master_id order by m.master_id asc";
            model[0].Col21 = sgen.seekval(userCode, mq, "closing");
            return View(model);
        }
        #endregion
        [HttpGet]
        public ContentResult getdrills(string lvl, string myguid, string fstr)
        {
            FillMst(myguid);
            string fdt = ""; string tdt = "";
            year_to = sgen.GetCookie(MyGuid, "year_to");
            year_from = sgen.GetCookie(MyGuid, "year_from");
            fdt = year_from.Split(' ')[0].Trim();
            tdt = year_to.Split(' ')[0].Trim();
            string cdt1 = fdt;
            cmd = "";
            string year = year_from.Substring(6, 4);
            switch (lvl)
            {
                case "1":
                    if (fstr.Trim() == "4")
                    {
                        cmd = "select (m.master_id) lbfstr,m.master_name lbname,sum(v.closing) lbcl from master_setting m " +
   "inner join (select client_unit_id, acode, sum(op) op, sum(dramount) dramount, sum(cramount) cramount, (sum(op) + sum(cramount) - sum(dramount)) closing from " +
   "(select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, OP_" + Ac_Year.Split(' ')[0].Trim() + " as op, 0 as dramount, 0 as cramount," +
   " 0 as cl from acbal union all " +
   "select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, sum(to_number(dramount)) - sum(to_number(cramount)) as op, 0 as dramount," +
   "0 as cramount, 0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
   "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') - 1 group by client_unit_id, acode " +
   "union all " +
   "select client_unit_id, acode as acode, vch_num, vch_date, 0 as op, to_number(dramount) dramount, to_number(cramount) cramount, 0 as cl from vouchers " +
   "where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') " +
   "and to_date('" + tdt + "', '" + sgen.Getsqldateformat() + "')) t group by client_unit_id, acode) v on substr(m.master_id,1,3)=substr(v.acode,1,3) " +
   "where m.type = 'AGM' and trim(substr(m.master_id,1,1))='" + fstr + "' and trim(substr(master_type,4,7))='0000' " +
   "group by m.master_name,m.master_id order by m.master_id asc";
                    }
                    else
                    {
                        cmd = "select (m.master_id) lbfstr,m.master_name lbname,sum(v.closing) lbcl from master_setting m " +
    "inner join (select client_unit_id, acode, sum(op) op, sum(dramount) dramount, sum(cramount) cramount, (sum(op) + sum(dramount) - sum(cramount)) closing from " +
    "(select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, OP_" + Ac_Year.Split(' ')[0].Trim() + " as op, 0 as dramount, 0 as cramount," +
    " 0 as cl from acbal union all " +
    "select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, sum(to_number(dramount)) - sum(to_number(cramount)) as op, 0 as dramount," +
    "0 as cramount, 0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
    "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') - 1 group by client_unit_id, acode " +
    "union all " +
    "select client_unit_id, acode as acode, vch_num, vch_date, 0 as op, to_number(dramount) dramount, to_number(cramount) cramount, 0 as cl from vouchers " +
    "where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') " +
    "and to_date('" + tdt + "', '" + sgen.Getsqldateformat() + "')) t group by client_unit_id, acode) v on substr(m.master_id,1,3)=substr(v.acode,1,3) " +
    "where m.type = 'AGM' and trim(substr(m.master_id,1,1))='" + fstr + "' and trim(substr(master_type,4,7))='0000' " +
    "group by m.master_name,m.master_id order by m.master_id asc";
                    }
                    break;
                case "2":
                    string cond = "";
                    //cmd = "SELECT vu.ACODE as ledgercode,cl.c_name as ledgername,SUM(OPBAL) AS OPBAL,SUM(DRAMOUNT) as dramt,SUM(CRAMOUNT) as cramt,SUM(OPBAL)+SUM(DRAMOUNT)-SUM(CRAMOUNT) as closing " +
                    //    "from (select acode,CLIENT_UNIT_ID, OP_2020 as opbal,0 as cramount,0 as dramount,0 as cl from acbal union all select acode,CLIENT_UNIT_ID,sum(dramount)-sum(cramount) as opbal" +
                    //    ",0 as cramount,0 as dramount,0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                    //    "to_date('" + cdt1.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + fdt + "','" + sgen.Getsqldateformat() + "')-1 group by acode,CLIENT_UNIT_ID union all select acode,CLIENT_UNIT_ID,0 as opbal" +
                    //    ",TO_NUMBER(cramount) cramount,TO_NUMBER(dramount) dramount,0 cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                    //    "to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') and to_date('" + tdt + "','" + sgen.Getsqldateformat() + "')) vu INNER JOIN (select vch_num,c_name,CLIENT_UNIT_ID from clients_mst " +
                    //    "where type='BCD' and find_in_set(client_unit_id, '" + unitid_mst + "')=1 union all select vch_num as acode ,c_name as aname,CLIENT_UNIT_ID from clients_mst " +
                    //    "where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "')=1 union all select cm.vch_num as acode,nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname" +
                    //    ",cm.CLIENT_UNIT_ID from master_setting a inner join clients_mst cm on a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id " +
                    //    "where cm.type = 'BAD' and cm.client_unit_id = '" + unitid_mst + "') cl on cl.vch_num = vu.acode and find_in_set(vu.client_unit_id, cl.client_unit_id) = 1 " + cond + "  group by vu.acode,cl.c_name";


                    cond = " and substr(b.VCH_NUM,1," + fstr.Trim().Length + ") ='" + fstr + "'";
                    if (fstr.Trim().Substring(0, 2).Equals("40"))
                    {
                        cmd = "select a.acode LedgerCode,b.c_name Ledger,a.closing,a.op,a.dramount,a.cramount from " +
           "(select client_unit_id, acode,max(vtype) as vtype, sum(op) op, sum(dramount) dramount, sum(cramount) cramount, (sum(op) + sum(cramount) - sum(dramount)) closing from " +
           "(select client_unit_id, acode as acode,'-' as vtype, '0' as vch_num, null as vch_date, OP_" + Ac_Year.Split(' ')[0].Trim() + " as op, 0 as dramount, 0 as cramount," +
           " 0 as cl from acbal " +
           "union all " +
           "select client_unit_id, acode as acode,max(type) as vtype, '0' as vch_num, null as vch_date, sum(to_number(dramount)) - sum(to_number(cramount)) as op, 0 as dramount," +
           "0 as cramount, 0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
           "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') - 1 group by client_unit_id, acode " +
           "union all " +
           "select client_unit_id, acode as acode,type as vtype, vch_num, vch_date, 0 as op, to_number(dramount) dramount, to_number(cramount) cramount, 0 as cl from vouchers " +
           "where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') " +
           "and to_date('" + tdt + "', '" + sgen.Getsqldateformat() + "')) t group by client_unit_id, acode) a " +
           "inner join clients_mst b on a.acode = b.vch_num " + cond + " where a.client_unit_id = '" + unitid_mst + "' order by b.c_name";

                    }
                    else
                    {
                        cmd = "select a.acode LedgerCode,b.c_name Ledger,a.closing,a.op,a.dramount,a.cramount from " +
                       "(select client_unit_id, acode,max(vtype) as vtype, sum(op) op, sum(dramount) dramount, sum(cramount) cramount, (sum(op) + sum(dramount) - sum(cramount)) closing from " +
                       "(select client_unit_id, acode as acode,'-' as vtype, '0' as vch_num, null as vch_date, OP_" + Ac_Year.Split(' ')[0].Trim() + " as op, 0 as dramount, 0 as cramount," +
                       " 0 as cl from acbal " +
                       "union all " +
                       "select client_unit_id, acode as acode,max(type) as vtype, '0' as vch_num, null as vch_date, sum(to_number(dramount)) - sum(to_number(cramount)) as op, 0 as dramount," +
                       "0 as cramount, 0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                       "to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') - 1 group by client_unit_id, acode " +
                       "union all " +
                       "select client_unit_id, acode as acode,type as vtype, vch_num, vch_date, 0 as op, to_number(dramount) dramount, to_number(cramount) cramount, 0 as cl from vouchers " +
                       "where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') " +
                       "and to_date('" + tdt + "', '" + sgen.Getsqldateformat() + "')) t group by client_unit_id, acode) a " +
                       "inner join clients_mst b on a.acode = b.vch_num " + cond + " where a.client_unit_id = '" + unitid_mst + "' order by b.c_name";
                    }
                    break;
                case "3":
                    cond = " and c.vch_num ='" + fstr + "'";
                    cmd = "select * from (select (t.client_unit_id||t.vch_num||to_char(t.vch_date,'yyyyMMdd')||t.type) fstr,t.vch_num," +
                     "nvl(to_char(t.vch_date,'" + sgen.Getsqldateformat() + "'),'01/01/1900') vch_date,t.acode LedgerCode, c.c_name Ledger,t.type," +
                     "t.dramount DrAmount,t.cramount CrAmount from " +
                     "(select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, OP_" + Ac_Year.Split(' ')[0].Trim() + " as op, 'OP' type," +
                     "0 as dramount, 0 as cramount, 0 as cl from acbal union all " +
                     "select client_unit_id, acode as acode, '0' as vch_num, null as vch_date, sum(to_number(dramount)) - sum(to_number(cramount)) as op," +
                     "type,0 as dramount, 0 as cramount, 0 as cl from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') " +
                     "between to_date('" + year_from.Split(' ')[0].Trim() + "', '" + sgen.Getsqldateformat() + "') and to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') - 1 " +
                     "group by client_unit_id, acode, type union all " +
                     "select client_unit_id, acode as acode, vch_num, vch_date, 0 as op, type, to_number(dramount) dramount, to_number(cramount) cramount, 0 as cl " +
                     "from vouchers where to_date(to_char(vch_date, '" + sgen.Getsqldateformat() + "'), '" + sgen.Getsqldateformat() + "') between " +
                     "to_date('" + fdt + "', '" + sgen.Getsqldateformat() + "') and to_date('" + tdt + "', '" + sgen.Getsqldateformat() + "')) t " +
                     "inner join (select a.vch_num,a.c_name from (select vch_num,c_name,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'BCD' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                     "union all select vch_num as acode ,c_name as aname,CLIENT_UNIT_ID,vch_date,type from clients_mst where type = 'LDG' and find_in_set(client_unit_id, '" + unitid_mst + "')= 1 " +
                     " union all select cm.vch_num as acode,nvl(a.master_name, '-') || '(' || cm.cpname || ')' as aname,cm.CLIENT_UNIT_ID,cm.vch_date,cm.type from master_setting a inner join clients_mst cm on " +
                     "a.master_id = cm.ptype and a.type = 'ABD' and a.client_unit_id = cm.client_unit_id where cm.type = 'BAD' and cm.client_unit_id = '" + unitid_mst + "') a) " +
                     "c on c.vch_num=t.acode and (t.dramount+t.cramount)>0" + cond + ") a order by vch_num,vch_date, type asc";

                    sgen.open_grid("Vouchers", cmd, 1);
                    ViewBag.scripCall = "callFoo3('Vouchers');";
                    break;

            }
            DataSet ds = new DataSet();
            DataTable dt = sgen.getdata(userCode, cmd);
            dt.TableName = "MainData";
            ds.Tables.Add(dt);
            return Content(ds.GetXml());
        }
    }

    public class HttpPostedFileBaseCustom : HttpPostedFileBase
    {
        MemoryStream stream;
        string contentType;
        string fileName;

        public HttpPostedFileBaseCustom(MemoryStream stream, string contentType, string fileName)
        {
            this.stream = stream;
            this.contentType = contentType;
            this.fileName = fileName;
        }

        public override int ContentLength
        {
            get { return (int)stream.Length; }
        }

        public override string ContentType
        {
            get { return contentType; }
        }

        public override string FileName
        {
            get { return fileName; }
        }

        public override Stream InputStream
        {
            get { return stream; }
        }

    }

}