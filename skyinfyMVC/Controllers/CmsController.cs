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
    public class CmsController : Controller
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

        string userCode = "",userid_mst = "", FN_From_Date = "", FN_To_Date = "", Ac_Year_id = "", cg_com_name = "", clientid_mst = "",
            unitid_mst = "", Ac_To_Date = "", Ac_From_Date = "", role_mst = "", clientname_mst = "", m_module3 = "", actionName = "", controllerName = "";


        // GET: Cms

        bool isedit = false, res = false, Isvalid = false;
        int cli = 0;
        sgenFun sgen;
        Cmd_Fun cmd_Fun;


        public void FillMst(string Myguid="")
        {
            MyGuid = Myguid;
            //sgen = new sgenFun(MyGuid);
            if (MyGuid == "") { MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]); }
            //if (MyGuid == "") MyGuid = sgen.GetCookie("", "MyGuid");
            sgen = new sgenFun(MyGuid);
            cmd_Fun = new Cmd_Fun(MyGuid);

            userCode = sgen.GetCookie(MyGuid,"userCode");
            userid_mst = sgen.GetCookie(MyGuid, "userid_mst");
            Ac_Year_id = sgen.GetCookie(MyGuid, "Ac_Year_id");
            cg_com_name = sgen.GetCookie(MyGuid, "cg_com_name");
            clientid_mst = sgen.GetCookie(MyGuid, "clientid_mst");
            unitid_mst = sgen.GetCookie(MyGuid, "unitid_mst");
            Ac_To_Date = sgen.GetCookie(MyGuid, "Ac_To_Date");
            Ac_From_Date = sgen.GetCookie(MyGuid, "Ac_From_Date");
            FN_From_Date = sgen.GetCookie(MyGuid, "year_from");
            FN_To_Date = sgen.GetCookie(MyGuid, "year_to");
            Ac_Year = sgen.GetCookie(MyGuid, "Ac_Year");
            role_mst = sgen.GetCookie(MyGuid, "role_mst");
            clientname_mst = sgen.GetCookie(MyGuid, "clientname_mst");
            m_module3 = sgen.GetCookie(MyGuid, "m_module3");
            actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            sgen.SetSession(MyGuid, "viewName", actionName);
            sgen.SetSession(MyGuid, "controllerName", controllerName);
        }

        //===============getload==========

        public List<Tmodelmain> getload(List<Tmodelmain> model)
        {
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            chkRef();
            Tmodelmain tm1 = new Tmodelmain();
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.vsave = "disabled='disabled'";
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
            sgen.SetSession(MyGuid,"EDMODE", "N");
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
            if (sgen.GetSession(MyGuid,"btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
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
                            sgen.SetSession(MyGuid,"EDMODE", "Y");

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

               
            }
            return model;
        }
        #endregion


        //=========makequery============

        #region make query

        public void Make_query(string viewname, string title, string btnval, string seektype)
        {
            FillMst();
            btnval = btnval.ToUpper();
            sgen.SetSession(MyGuid,"btnval", btnval);
            string cmd = "";
            clientid_mst = sgen.GetCookie(MyGuid,"clientid_mst");
            unitid_mst = sgen.GetCookie(MyGuid,"unitid_mst");
            switch (viewname.ToLower())
            {
                #region Export Party Assigned

                case "party_ass":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = "SELECT a.client_id||a.client_unit_id||a.master_id||to_char(a.vch_date,'yyyymmdd')||a.type as fstr, a.master_name as Education_Category," +
                        "Section_Strength as Class_Hours,classid as Class_Starting_Time,sectionid as Class_End_Time ,CASE when a.status='Y' THEN 'Active'  when a.status = '' THEN 'Active' when a.status='N' THEN 'Inactive' end as Status  " +
                           ",b.First_name ||' ' || b.middle_name || ' '|| b.last_name as Ent_By,to_char( convert_tz( a.master_EntDate,'UTC','" + sgen.Gettimezone() + "')," +
                           "" +
                           "'" + sgen.Getsqldateformat() + "')as Ent_Date from master_setting a,user_details b  where a.type in ( 'ECC','DDECC') and  a.master_entby =lpad(b.rec_id,6,'0') and a.client_id='" + clientid_mst + "' and a.client_unit_id='" + unitid_mst + "' order by a.master_name ";
                            break;
                    }

                    break;

                #endregion

               
            }

            sgen.open_grid(title, cmd, sgen.Make_int(seektype));
            ViewBag.pageurl = sgen.GetSession(MyGuid, "pageurl").ToString();
        }



        #endregion

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

            sgen.SetSession(MyGuid,"TR_MID", mid_mst.Trim());
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
                sgen.SetSession(MyGuid,"EDMODE", "N");
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
                ViewBag.scripCall = "enableForm();";
                mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10 + " where type in  ('DDECC' ,'ECC')" + model[0].Col11 + "";
                master_id = sgen.genNo(userCode, mq, 3, "auto_genid");


                model[0].Col16 = master_id;

                model[0].Col22 = "Y";
            }
            else if (command == "Edit")
            {
                try
                {
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1, '" + ex.Message.ToString() + "', 1);"; }
            }

            else if (command == "Cancel")
            {
                return RedirectToAction("party_ass", new { @m_id = EncryptDecrypt.Encrypt(model[0].Col15), @mid = EncryptDecrypt.Encrypt(model[0].Col14) });
            }
            else if (command == "Callback")
            {
                if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
                model = CallbackFun(btnval, "N", "party_ass", "Cms", model);
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
                        ViewBag.scripCall = "showmsgJS(1, 'Data Saved Successfully', 1);disableForm();";

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
        
    }
}