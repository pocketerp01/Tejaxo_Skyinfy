using skyinfyMVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace skyinfyMVC.Controllers
{
    public class MstController : Controller
    {
        public string mq = "", mq1 = "", vch_num = "", btnval = "", tab_name = "", type = "", type_desc = "", tab_name1 = "", ent_date = "", status = "",
             cond = "", mid_mst = "", m_id_mst = "", where = "", cmd = "";
        string sess_name = "";
        static string MyGuid = "";
        sgenFun sgen;
        Cmd_Fun cmd_Fun;
        public static bool isedit = false, res = false, ok = false;
        public static string userCode, userid_mst, Ac_Year_id, cg_com_name, clientid_mst, unitid_mst, actionName = "", controllerName = "", m_module3 = "";
        public static string name = "", ctype1 = "", ctype2 = "", ctype3 = "", ctype4 = "", ctype5 = "", ctype6 = "", ctype7 = "",
finalpath1 = "", finalpath2 = "", finalpath3 = "", finalpath4 = "", finalpath5 = "", finalpath6 = "", finalpath7 = "", path1 = "", path2 = "", path3 = "", path4 = "",
path5 = "", path6 = "", path7 = "", fileName1 = "", fileName2 = "", fileName3 = "", fileName4 = "", fileName5 = "", fileName6 = "", fileName7 = "", encpath1 = "",
encpath2 = "", encpath3 = "", encpath4 = "", encpath5 = "", encpath6 = "", encpath7 = "";

        #region MAIN FUNC
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
            m_module3 = sgen.GetCookie(MyGuid, "m_module3");
            actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            sgen.SetSession(MyGuid, "viewName", actionName);
            sgen.SetSession(MyGuid, "controllerName", controllerName);

            if (sgen.GetSession(MyGuid, "unitcnt") == null)
            {
                mq = sgen.seekval(userCode, "SELECT COUNT(*) AS CNT FROM company_unit_profile where 1=1 and unit_status = '1'", "cnt");
                sgen.SetSession(MyGuid, "unitcnt", mq);
            }

        }
        public List<Tmodelmain> getload(List<Tmodelmain> model)
        {
            chkRef();
            Tmodelmain tm1 = new Tmodelmain();
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            m_id_mst = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
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

        //===============make query===========

        #region make query

        public void Make_query(string viewname, string title, string btnval, string seektype, string param1, string Myguid = "")
        {
            string minid = "", join_mst = "", col_mst = "", id_mst = "", l1_mst = "L1", l2_mst = "L2", l3_mst = "L3", l4_mst = "L4", l5_mst = "L5";
            FillMst(Myguid);
            sgen.SetSession(MyGuid, "btnval", btnval);
            switch (viewname.ToLower())
            {
                #region master_ctrl

                case "master_ctrl":

                    if (sgen.GetSession(MyGuid, "TYPE_MST").ToString() != null && sgen.GetSession(MyGuid, "COND_MST").ToString() != null)
                    {
                        type = sgen.GetSession(MyGuid, "TYPE_MST").ToString().Trim();
                        //if (type.Equals("ECS") || type.Equals("ESW")) { mq = "a.client_name as Abbreviation,"; }
                        //else if (type.Equals("EBD") || type.Equals("UMM")) { mq = "a.client_name as Description,"; }
                        where = sgen.GetSession(MyGuid, "COND_MST").ToString();

                        if (where.Trim().Contains("client_id") && where.Trim().Contains("client_unit_id")) cond = " and find_in_set(a.client_id,'" + clientid_mst + "')=1 and find_in_set(a.client_unit_id,'" + unitid_mst + "')=1";
                        else if (where.Trim().Contains("client_id")) cond = " and find_in_set(a.client_id,'" + clientid_mst + "')=1";
                        else if (where.Trim().Contains("client_unit_id")) cond = " and find_in_set(a.client_unit_id,'" + unitid_mst + "')=1";
                        else cond = " and find_in_set(a.client_id,'" + clientid_mst + "')=1 and find_in_set(a.client_unit_id,'" + unitid_mst + "')=1";

                        switch (btnval.ToUpper())
                        {
                            case "DETAIL":
                                cmd = "select (a.master_id||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr," +
                                    "a.master_id as Doc_No,a.master_name as Name,a.client_name as description," + mq + "" +
                                    "(CASE when a.status = 'Y' THEN 'Active' when a.status = '' THEN 'Active' when a.status = 'N' THEN 'Inactive' end) Status,a.sectionid Deflt,a.client_unit_id Unit_id," +
                                    "(b.First_name|| ' '|| replace(nvl(b.middle_name, ''),'0','')|| ' '|| replace(nvl(b.last_name,''),'0','')) as Ent_By," +
                                    "to_char(convert_tz(a.master_EntDate, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Ent_Date " +
                                    "from master_setting a " +
                                    "left join user_details b on a.master_entby = b.vch_num and b.type = 'CPR' " +
                                    "where a.type in ('" + type + "','DD" + type + "') order by a.master_id asc";
                                break;
                            case "EDIT":
                                cmd = "select (a.master_id||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr," +
                                    "a.master_id as Doc_No,a.master_name as Name,a.client_name as description," + mq + "" +
                                    "(CASE when a.status = 'Y' THEN 'Active' when a.status = '' THEN 'Active' when a.status = 'N' THEN 'Inactive' end) Status,a.sectionid Deflt,a.client_unit_id Unit_id, " +
                                    "(b.First_name|| ' '|| replace(nvl(b.middle_name, ''),'0','')|| ' '|| replace(nvl(b.last_name,''),'0','')) as Ent_By," +
                                    "to_char(convert_tz(a.master_EntDate, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Ent_Date " +
                                    "from master_setting a " +
                                    "left join user_details b on a.master_entby = b.vch_num and b.type = 'CPR' " +
                                    "where a.type in ('" + type + "','DD" + type + "')" + cond + " order by a.master_id asc";
                                break;

                            case "EXT":
                                cmd = "select (a.master_id||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr," +
                                    "a.master_id as Doc_No,a.master_name as Name,a.client_name as description," + mq + "" +
                                    "(CASE when a.status = 'Y' THEN 'Active' when a.status = '' THEN 'Active' when a.status = 'N' THEN 'Inactive' end) Status,a.sectionid Deflt,a.client_id Company_id,a.client_unit_id Unit_id," +
                                    "(b.First_name|| ' '|| replace(nvl(b.middle_name, ''),'0','')|| ' '|| replace(nvl(b.last_name,''),'0','')) as Ent_By," +
                                    "to_char(convert_tz(a.master_EntDate, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Ent_Date " +
                                    "from master_setting a " +
                                    "left join user_details b on a.master_entby = b.vch_num and b.type = 'CPR' " +
                                    "where a.type in ('" + type + "','DD" + type + "')" + cond + " and a.sectionid<>'Y' order by a.master_id asc";
                                break;
                            case "UNIT":
                                cmd = "select (cup_id||company_profile_id||to_char(vch_date,'yyyymmdd')) fstr,company_profile_id Company_d,(unit_name||' ('||cup_id||')') " +
                                    "Unit_Name from company_unit_profile where unit_status='1' and cup_id<>'" + unitid_mst + "' order by unit_name asc";
                                break;
                            case "PARTY":
                                cmd = "select distinct (a.vch_num|| to_char(a.vch_date,'yyyymmdd')||a.type) as fstr,a.vch_num as Party_code,a.c_name as Name,a.c_gstin as GSTIN from clients_mst a where " +
                              "find_in_set(a.client_id,'" + clientid_mst + "')=1 and find_in_set(a.client_unit_id,'" + unitid_mst + "')=1 and substr(a.vch_num,0,3)='403' and a.type='LDG'";
                                break;

                        }
                    }
                    break;

                #endregion

                #region doc_master

                case "doc_master":

                    #region
                    //if (Session["M_TYPE_MST"].ToString() != null && Session["M_COND_MST"].ToString() != null)
                    //{
                    //    type = Session["M_TYPE_MST"].ToString().Trim();
                    //    where = Session["M_COND_MST"].ToString();

                    //    if (where.Trim().Contains("client_id") && where.Trim().Contains("client_unit_id")) cond = " and find_in_set(a.client_id,'" + clientid_mst + "')=1 and find_in_set(a.client_unit_id,'" + unitid_mst + "')=1";
                    //    else if (where.Trim().Contains("client_id")) cond = " and find_in_set(a.client_id,'" + clientid_mst + "')=1";
                    //    else if (where.Trim().Contains("client_unit_id")) cond = " and find_in_set(a.client_unit_id,'" + unitid_mst + "')=1";
                    //    else cond = " and find_in_set(a.client_id,'" + clientid_mst + "')=1 and find_in_set(a.client_unit_id,'" + unitid_mst + "')=1";
                    #endregion

                    switch (btnval.ToUpper())
                    {
                        case "MTYPE":
                        case "MEXT":

                            #region

                            //case "EDIT":
                            //case "VIEW":
                            //case "EXT":
                            //if (type == "KIG")
                            //{
                            //    cmd = "select (a.master_id||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr," +
                            //        "(case when a.master_id = '99' then '99' else substr(a.master_id, 1, 1) end) GrpType,mg.master_name GrpName, a.master_id Type, a.master_name Name," +
                            //        "a.client_name Description,(CASE when a.status = 'Y' THEN 'Active'  when a.status = '' THEN 'Active' when " +
                            //        "a.status = 'N' THEN 'Inactive' end) as Status from master_setting a inner join master_setting mg on mg.classid = (case when a.master_id = '99' then " +
                            //        "'99' else substr(a.master_id, 1, 1) end) and mg.type = 'KGP' where a.type in ('" + type + "', 'DD" + type + "') " + where + "";
                            //}
                            //else if (type == "KPO")
                            //{
                            //    if (param1.Trim().Equals("21001.12")) mq1 = " and substr(a.master_id, 0, 1) = '4'";
                            //    else if (param1.Trim().Equals("28004.7")) mq1 = " and substr(a.master_id, 0, 1) = '5'";
                            //    cmd = "select (a.master_id||TO_CHAR(a.vch_date,'yyyymmdd')||a.type) as fstr,a.master_id Type,a.master_name Name," +
                            //            "a.client_name Description,(CASE when a.status = 'Y' THEN 'Active' when a.status = '' THEN 'Active' when a.status = 'N' THEN 'Inactive' end) Status," +
                            //            "(b.First_name|| ' '|| nvl(b.middle_name, '')|| ' '|| b.last_name) as Ent_By," +
                            //            "to_char(convert_tz(a.master_EntDate, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Ent_Date " +
                            //            "from master_setting a " +
                            //            "inner join user_details b on a.master_entby = lpad(b.rec_id, 6, 0) and b.type = 'CPR' " +
                            //            "where a.type in ('" + type + "','DD" + type + "')" + cond + "" + mq1 + "";
                            //}
                            //else
                            //{                                    
                            //    cmd = "select (a.master_id||TO_CHAR(a.vch_date,'yyyymmdd')||a.type) as fstr,a.master_id Type,a.master_name as Name,a.client_name Description," +
                            //          "(CASE when a.status = 'Y' THEN 'Active' when a.status = '' THEN 'Active' when a.status = 'N' THEN 'Inactive' end) as Status," +
                            //          "(b.First_name|| ' '|| nvl(b.middle_name, '')|| ' '|| b.last_name) as Ent_By," +
                            //          "to_char(convert_tz(a.master_EntDate, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Ent_Date " +
                            //          "from master_setting a " +
                            //          "inner join user_details b on a.master_entby = lpad(b.rec_id, 6, 0) and b.type = 'CPR' where a.type in ('" + type + "','DD" + type + "')" + cond + "";
                            //}
                            #endregion

                            switch (m_module3.ToLower())
                            {
                                case "invtmain":
                                case "bhmain":
                                    cmd = "select '000' as fstr,'MRN Type' Doc_type from dual union select '001' as fstr,'Issue Type' from dual union select '002' as fstr,'Return Type' from dual " +
     "union select '003' as fstr,'Opening Type' from dual union select '004' as fstr,'MRS Type' from dual union select '005' as fstr,'DPR Type' from dual " +
     "union select '006' as fstr,'RGP Type' from dual union select '007' as fstr,'NRGP Type' from dual union select '008' as fstr,'Dispatch Type' from dual";
                                    break;
                                case "purmain":
                                    cmd = "select '009' as fstr,'PO Type' from dual";
                                    break;
                                case "bnpmain":
                                    cmd = "select '010' as fstr,'SO Type' Doc_type from dual union all select '014' fstr,'Invoice Against Document' Doc_type from dual";
                                    break;
                                case "acctmain":
                                    cmd = "select (master_id||TO_CHAR(vch_date,'yyyymmdd')||type) as fstr,master_id,master_name from master_setting where type='TAC' and client_id = '" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
                                    break;
                                case "pdmain":
                                    cmd = "select '011' fstr,'DPR Type' from dual union all select '012' fstr,'PDO Type' from dual union all select '013' as fstr,'PD Type' from dual";
                                    break;
                            }
                            break;

                        case "TYPEVAL":
                        case "EXT":
                            string cond2 = "";
                            switch (param1.Trim())
                            {
                                case "000"://mrn
                                    type = "KMR";
                                    cond = " and find_in_set(a.client_id,'" + clientid_mst + "')=1 and find_in_set(a.client_unit_id,'" + unitid_mst + "')=1";
                                    mq = "01,02,03,05,07";
                                    sgen.SetSession(MyGuid, "MTYPE", "MRN TYPE");
                                    sgen.SetSession(MyGuid, "MCOND", cond);
                                    break;

                                case "001"://issue
                                    type = "KIS";
                                    cond = " and find_in_set(a.client_id,'" + clientid_mst + "')=1 and find_in_set(a.client_unit_id,'" + unitid_mst + "')=1";
                                    mq = "30,32";
                                    sgen.SetSession(MyGuid, "MTYPE", "ISSUE TYPE");
                                    sgen.SetSession(MyGuid, "MCOND", cond);
                                    break;

                                case "002"://return
                                    type = "KSR";
                                    cond = " and find_in_set(a.client_id,'" + clientid_mst + "')=1 and find_in_set(a.client_unit_id,'" + unitid_mst + "')=1";
                                    mq = "10,11";
                                    sgen.SetSession(MyGuid, "MTYPE", "RETURN TYPE");
                                    sgen.SetSession(MyGuid, "MCOND", cond);
                                    break;

                                case "003"://op
                                    type = "KPV";
                                    cond = " and find_in_set(a.client_id,'" + clientid_mst + "')=1 and find_in_set(a.client_unit_id,'" + unitid_mst + "')=1";
                                    mq = "36,37";
                                    sgen.SetSession(MyGuid, "MTYPE", "OPENING TYPE");
                                    sgen.SetSession(MyGuid, "MCOND", cond);
                                    break;

                                case "004"://mrs
                                    type = "KRQ";
                                    cond = " and find_in_set(a.client_id,'" + clientid_mst + "')=1 and find_in_set(a.client_unit_id,'" + unitid_mst + "')=1";
                                    mq = "3A,3B";
                                    sgen.SetSession(MyGuid, "MTYPE", "MRS TYPE");
                                    sgen.SetSession(MyGuid, "MCOND", cond);
                                    break;

                                case "005"://dpr
                                case "011"://dpr
                                    type = "KDP";
                                    cond = " and find_in_set(a.client_id,'" + clientid_mst + "')=1 and find_in_set(a.client_unit_id,'" + unitid_mst + "')=1";
                                    mq = "16,17";
                                    sgen.SetSession(MyGuid, "MTYPE", "DPR TYPE");
                                    sgen.SetSession(MyGuid, "MCOND", cond);
                                    break;

                                case "006"://rgp
                                    type = "KRG";
                                    cond = " and find_in_set(a.client_id,'" + clientid_mst + "')=1 and find_in_set(a.client_unit_id,'" + unitid_mst + "')=1";
                                    mq = "21";
                                    mq1 = " and a.master_id = '21'";
                                    sgen.SetSession(MyGuid, "MTYPE", "RGP TYPE");
                                    sgen.SetSession(MyGuid, "MCOND", cond);
                                    break;

                                case "007"://nrgp
                                    type = "KRG";
                                    cond = " and find_in_set(a.client_id,'" + clientid_mst + "')=1 and find_in_set(a.client_unit_id,'" + unitid_mst + "')=1";
                                    mq = "22";
                                    mq1 = " and a.master_id = '22'";
                                    sgen.SetSession(MyGuid, "MTYPE", "NRGP TYPE");
                                    sgen.SetSession(MyGuid, "MCOND", cond);
                                    break;

                                case "008"://dispatch
                                    type = "KDS";
                                    cond = " and find_in_set(a.client_id,'" + clientid_mst + "')=1 and find_in_set(a.client_unit_id,'" + unitid_mst + "')=1";
                                    mq = "60,61,62";
                                    sgen.SetSession(MyGuid, "MTYPE", "DISPATCH TYPE");
                                    sgen.SetSession(MyGuid, "MCOND", cond);
                                    break;

                                case "009"://po                                
                                    type = "KPO";
                                    cond = " and find_in_set(a.client_id,'" + clientid_mst + "')=1 and find_in_set(a.client_unit_id,'" + unitid_mst + "')=1";
                                    mq = "40,41,42,43,44,50,51,52,53";
                                    mq1 = " and substr(a.master_id, 0, 1) = '5'";
                                    sgen.SetSession(MyGuid, "MTYPE", "PO TYPE");
                                    sgen.SetSession(MyGuid, "MCOND", cond);
                                    break;

                                case "010"://so
                                    type = "KPO";
                                    cond = " and find_in_set(a.client_id,'" + clientid_mst + "')=1 and find_in_set(a.client_unit_id,'" + unitid_mst + "')=1";
                                    mq = "40,41,42,43,44,48,49,50,51,52,53";
                                    mq1 = " and substr(a.master_id, 0, 1) = '4'";
                                    sgen.SetSession(MyGuid, "MTYPE", "SO TYPE");
                                    sgen.SetSession(MyGuid, "MCOND", cond);
                                    break;

                                case "012"://PDO Type
                                    type = "PTP";
                                    cond = " and find_in_set(a.client_id,'" + clientid_mst + "')=1 and find_in_set(a.client_unit_id,'" + unitid_mst + "')=1";
                                    mq = "70,71";
                                    sgen.SetSession(MyGuid, "MTYPE", "PDO TYPE");
                                    sgen.SetSession(MyGuid, "MCOND", cond);
                                    break;

                                case "013"://PD Type
                                    type = "KPD";
                                    cond = " and find_in_set(a.client_id,'" + clientid_mst + "')=1 and find_in_set(a.client_unit_id,'" + unitid_mst + "')=1";
                                    mq = "100,101";
                                    sgen.SetSession(MyGuid, "MTYPE", "PD TYPE");
                                    sgen.SetSession(MyGuid, "MCOND", cond);
                                    break;

                                case "014"://invoice Type
                                    type = "KIV";
                                    cond = " and find_in_set(a.client_id,'" + clientid_mst + "')=1 and find_in_set(a.client_unit_id,'" + unitid_mst + "')=1";
                                    mq = "51,52,53,54,54";
                                    mq1 = " and substr(a.master_id, 0, 1) = '5'";
                                    sgen.SetSession(MyGuid, "MTYPE", "INVOICE TYPE");
                                    sgen.SetSession(MyGuid, "MCOND", cond);
                                    break;

                                case "1":
                                case "2":
                                case "3":
                                case "4":
                                case "5":
                                case "6":
                                    type = "ALM";
                                    cond = " and find_in_set(a.client_unit_id,'" + unitid_mst + "')=1 ";
                                    cond2 = " and substr(a.master_id,1,1)='" + param1 + "'";
                                    mq = "1,2,3,4,5,6";
                                    sgen.SetSession(MyGuid, "MTYPE", "LEDGER TYPE MASTER");
                                    sgen.SetSession(MyGuid, "MCOND", cond);
                                    break;
                            }

                            cmd = "select (a.master_id||TO_CHAR(a.vch_date,'yyyymmdd')||a.type) as fstr,a.master_id doc_Type,a.master_name as Name,a.client_name Description, a.client_unit_id unit_id," +
                                  "(CASE when a.status = 'Y' THEN 'Active' when a.status = '' THEN 'Active' when a.status = 'N' THEN 'Inactive' end) as Status," +
                                  "(b.First_name||''|| replace(nvl(b.middle_name, ''),'0','')||''|| replace(nvl(b.last_name,''),'0','')) as Ent_By," +
                                  "to_char(convert_tz(a.master_EntDate, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Ent_Date " +
                                  "from master_setting a " +
                                  "inner join user_details b on a.master_entby = b.vch_num and b.type = 'CPR' where a.type in ('" + type + "','DD" + type + "')" + cond + "" + mq1 + "" + cond2 + "";

                            break;

                        case "UNIT":
                            cmd = "select (cup_id||company_profile_id||to_char(vch_date,'yyyymmdd')) fstr,company_profile_id Company_d,(unit_name||' ('||cup_id||')') " +
                                "Unit_Name from company_unit_profile where unit_status='1' and cup_id<>'" + unitid_mst + "' order by unit_name asc";
                            break;

                        case "PARTY":
                            cmd = "select distinct (a.vch_num|| to_char(a.vch_date,'yyyymmdd')||a.type) as fstr,a.vch_num as Ledger_code,a.c_name as Ledger_Name from clients_mst a where " +
                          "find_in_set(a.client_id,'" + clientid_mst + "')=1 and find_in_set(a.client_unit_id,'" + unitid_mst + "')=1 and type='LDG'";
                            break;
                    }
                    //}
                    break;

                #endregion

                #region cascade_mst

                case "cascade_mst":

                    if (sgen.GetSession(MyGuid, "CS_TYPEMST").ToString() != null && sgen.GetSession(MyGuid, "CS_CONDMST").ToString() != null && sgen.GetSession(MyGuid, "CS_MIDMST").ToString() != null)
                    {
                        type = sgen.GetSession(MyGuid, "CS_TYPEMST").ToString().Trim();
                        where = sgen.GetSession(MyGuid, "CS_CONDMST").ToString().Trim();
                        minid = sgen.GetSession(MyGuid, "CS_MIDMST").ToString().Trim();

                        if (sgen.GetSession(MyGuid, "L1_MST").ToString() != null && sgen.GetSession(MyGuid, "L2_MST").ToString() != null && sgen.GetSession(MyGuid, "L3_MST").ToString() != null && sgen.GetSession(MyGuid, "L4_MST").ToString() != null && sgen.GetSession(MyGuid, "L5_MST").ToString() != null)
                        {
                            l1_mst = sgen.GetSession(MyGuid, "L1_MST").ToString().Replace(" ", "_").Trim();
                            l2_mst = sgen.GetSession(MyGuid, "L2_MST").ToString().Replace(" ", "_").Trim();
                            l3_mst = sgen.GetSession(MyGuid, "L3_MST").ToString().Replace(" ", "_").Trim();
                            l4_mst = sgen.GetSession(MyGuid, "L4_MST").ToString().Replace(" ", "_").Trim();
                            l5_mst = sgen.GetSession(MyGuid, "L5_MST").ToString().Replace(" ", "_").Trim();
                        }

                        if ((minid.Trim() == "17001.16"))
                        {
                            join_mst = " inner join master_setting b on b.master_id=a.classid and b.type='HBM' and a.client_id=b.client_id and a.client_unit_id=b.client_unit_id ";
                            col_mst = "b.master_name as " + l1_mst + ",a.master_name as " + l2_mst + ",";
                            id_mst = "b.master_id||a.master_id DocNo,";
                        }
                        else if ((minid.Trim() == "17001.17"))
                        {
                            join_mst = " inner join master_setting b on b.master_id=a.classid and b.type='HBM' and a.client_id=b.client_id and a.client_unit_id=b.client_unit_id " +
                                "inner join master_setting f on f.master_id=a.sectionid and f.type='IN0' and a.client_id=f.client_id and a.client_unit_id=f.client_unit_id ";
                            col_mst = "b.master_name as " + l1_mst + ",f.master_name as " + l2_mst + ",a.master_name as " + l3_mst + ",";
                            id_mst = "b.master_id||f.master_id||a.master_id DocNo,";
                        }
                        else if ((minid.Trim() == "17001.18"))
                        {
                            join_mst = " inner join master_setting b on b.master_id=a.classid and b.type='HBM' and a.client_id=b.client_id and a.client_unit_id=b.client_unit_id " +
                               "inner join master_setting f on f.master_id=a.sectionid and f.type='IN0' and a.client_id=f.client_id and a.client_unit_id=f.client_unit_id " +
                               "inner join master_setting rm on rm.master_id=a.client_name and rm.type='IN1' and a.client_id=rm.client_id and a.client_unit_id=rm.client_unit_id ";
                            col_mst = "b.master_name as " + l1_mst + ",f.master_name as " + l2_mst + ",rm.master_name as " + l3_mst + ",a.master_name as " + l4_mst + ",";
                            id_mst = "b.master_id||f.master_id||rm.master_id||a.master_id DocNo,";
                        }
                        else if ((minid.Trim() == "17001.19"))
                        {
                            join_mst = " inner join master_setting b on b.master_id=a.classid and b.type='HBM' and a.client_id=b.client_id and a.client_unit_id=b.client_unit_id " +
                                 "inner join master_setting f on f.master_id=a.sectionid and f.type='IN0' and a.client_id=f.client_id and a.client_unit_id=f.client_unit_id " +
                                 "inner join master_setting rm on rm.master_id=a.client_name and rm.type='IN1' and a.client_id=rm.client_id and a.client_unit_id=rm.client_unit_id " +
                                 "inner join master_setting rk on rk.master_id=a.subjectid and rk.type='IN2' and a.client_id=rk.client_id and a.client_unit_id=rk.client_unit_id ";
                            col_mst = "b.master_name as " + l1_mst + ",f.master_name as " + l2_mst + ",rm.master_name as " + l3_mst + ",rk.master_name as " + l4_mst + ",a.master_name as " + l5_mst + ",";
                            id_mst = "b.master_id||f.master_id||rm.master_id||rk.master_id||a.master_id DocNo,";
                        }

                        switch (btnval.ToUpper())
                        {
                            case "EDIT":
                            case "DETAIL":

                                cmd = "select a.client_id||a.client_unit_id||a.master_id||TO_CHAR(a.vch_date,'yyyymmdd')||a.type as fstr," + id_mst + "" + col_mst + "" +
                                    "(CASE when a.status = 'Y' THEN 'Active' when a.status = '' THEN 'Active' when a.status = 'N' THEN 'Inactive' end) as Status " +
                                    "from master_setting a " + join_mst + "" +
                                    "where a.type in ('" + type + "','DD" + type + "') and a.client_id='" + clientid_mst + "' and a.client_unit_id='" + unitid_mst + "'";
                                break;
                        }
                    }
                    break;

                #endregion

                #region address_master
                case "address_master":
                    if (sgen.GetSession(MyGuid, "TYPE_MST").ToString() != null && sgen.GetSession(MyGuid, "COND_MST").ToString() != null)
                    {
                        type = sgen.GetSession(MyGuid, "TYPE_MST").ToString().Trim();

                        switch (btnval)
                        {
                            case "EDIT":
                            case "VIEW":
                                if (type == "CSD")
                                {

                                    cmd = "SELECT (rec_id||alpha_2||state_gst_code||type) as fstr,alpha_2,alpha_3,country_name,region,sub_region,std_code," +
                                    "state_gst_code,state_name,district_id,district_name from country_state where type='" + type + "'";
                                }
                                if (type == "CST")
                                {

                                    cmd = "SELECT (rec_id||alpha_2||state_gst_code||type) as fstr,teh_name as tehsil,district_name,state_name,country_name,state_gst_code," +
                                      "region FROM country_state where type = '" + type + "' and client_unit_id='" + unitid_mst + "'";
                                }
                                if (type == "CSV")
                                {
                                    cmd = "SELECT (rec_id||alpha_2||state_gst_code||type) as fstr,v_name as village,teh_name as tehsil" +
                                            ",district_name,state_name,country_name,region,pincode,state_gst_code FROM country_state where " +
                                           "type = '" + type + "' and client_unit_id='" + unitid_mst + "'";
                                }
                                break;
                            case "STATE":

                                if (type == "CSD")
                                {
                                    cmd = "select DISTINCT (alpha_2||state_gst_code) as fstr, alpha_2,alpha_3," +
                                         "country_name,region,sub_region,std_code,state_gst_code,state_name from country_state  ";
                                }
                                if (type == "CST")
                                {

                                    cmd = "SELECT DISTINCT (alpha_2||state_gst_code||district_name) as fstr,district_name,state_gst_code,state_name,country_name,region " +
                                             "FROM country_state where alpha_2='IN' union SELECT DISTINCT (alpha_2||state_gst_code||district_name) as fstr,district_name,state_gst_code,state_name,country_name," +
                                                "region FROM country_state where type = 'CSD' and client_unit_id='" + unitid_mst + "' and alpha_2='IN'";
                                }
                                if (type == "CSV")
                                {

                                    cmd = "SELECT DISTINCT (alpha_2||state_gst_code||teh_name) as fstr," +
                                                      "teh_name as tehsil,district_name,state_name,country_name,state_gst_code,region " +
                                                      "FROM country_state where type = 'CST' and client_unit_id='" + unitid_mst + "'";
                                }
                                break;
                        }
                        break;
                    }
                    break;

                #endregion

                #region Form Control

                case "form_ctrl":
                    switch (btnval.ToUpper())
                    {
                        case "VIEW":
                        case "EDIT":
                            string CF_MID = sgen.GetSession(MyGuid, "CF_MID").ToString();
                            string cond = "";
                            switch (CF_MID)
                            {
                                case "48004.1":
                                    cond = "and param5 in ('CUSTOMER DETAIL')";
                                    break;
                                case "35001.4":
                                    cond = "and param5 in ('TRANSACTION REQUEST','CUSTOMER DETAIL')";
                                    break;
                                case "20009.1":
                                    cond = "and param5 in ('CUSTOMER DETAIL')";
                                    break;

                                case "190004.2":
                                    cond = "and param5='DAILY WORK'";
                                    break;

                                case "5002.1":
                                    //cond = "and param5='NEW CLIENT'";
                                    cond = "and param5='CUSTOMER DETAIL'";
                                    break;
                                case "17001.25":
                                    cond = "and upper(param5) in ('VENDOR DETAIL','ITEM/SERVICE MASTER','MRN','PO','POIMP','MATERIAL ISSUANCE') ";
                                    break;
                                case "43001.3":
                                    cond = "and upper(param5) in ('ITEM/SERVICE MASTER')";
                                    break;
                                case "39001.17":
                                    cond = "and upper(param5) in ('MACHINE MASTER','MOULD / TOOL MASTER','PD') ";
                                    break;
                                case "40001.7":
                                    if (m_module3 == "bnpmain")
                                    {
                                        cond = "and param5 in ('CUSTOMER DETAIL','SALES ORDER','INVOICE')";
                                    }

                                    else if (m_module3 == "crmvmain")
                                    {
                                        string found = sgen.getstring(userCode, "select m_module3 from menus where m_id='40009' and m_module3='crmvmain'");
                                        //cond = "and param5 in ('NEW CLIENT','PROSPECT DATA','LEAD MASTER','SITE VISIT')";
                                        if (found == "")
                                        {
                                            cond = "and param5 in ('CUSTOMER DETAIL','PROSPECT DATA','LEAD MASTER')";

                                        }
                                        else
                                        {
                                            cond = "and param5 in ('CUSTOMER DETAIL','PROSPECT DATA','LEAD MASTER','SITE VISIT')";
                                        }

                                    }
                                    break;
                                case "9003.3.13":
                                    cond = "and param5 in ('EMPLOYEE DETAIL','VENDOR DETAIL')";
                                    break;

                                case "22005.3":
                                    cond = "and param5 in ('CUSTOMER DETAIL','VENDOR DETAIL')";
                                    break;
                                case "7004.17":
                                    cond = "and param5 in ('STDREG')";
                                    break;
                                default:
                                    cond = "";
                                    break;
                            }

                            cmd = "select distinct (a.client_id||a.client_unit_id||a.type||a.param5) as fstr," +
                                " (case when  (a.param8 IS NULL or a.param8='0' or a.param8='')   Then a.param5 else upper(a.param8) END ) " +
                                " As Page_Name from controls a where a.type='VDC' and a.client_unit_id='" + unitid_mst + "' " + cond + "";
                            break;
                    }
                    break;

                #endregion

                #region doc print config
                case "prn_ctrl":
                    switch (btnval.ToUpper())
                    {
                        case "VIEW":
                        case "EDIT":
                            cmd = "select DISTINCT (k.client_id||k.client_unit_id||k.vch_num||to_char(k.vch_date,'yyyymmdd')||k.type) fstr,k.vch_num doc_no," +
                                "k.col1 doc_control_no,k.col2 ft,f.master_name form_type,k.col3 mt,m.master_name mst_type," +
                                "k.col4 Mstmain_type,r.master_name rpt from kc_tab k " +
                                "inner join master_setting f on f.master_id=k.col2 and f.type='DPC' " +
                                "inner join (select master_id,master_name from master_setting where type in ('KMR','KPO','KRQ')) m on m.master_id=k.col3 " +
                                "inner join master_setting r on r.master_id=k.col5 and r.classid=f.master_id and r.sectionid=m.master_id " +
                                "where k.type='PRN' and k.client_id='" + clientid_mst + "' and k.client_unit_id='" + unitid_mst + "'";
                            break;
                        case "NEW":
                            cmd = "SELECT (client_id||client_unit_id||master_id||to_char(vch_date,'yyyymmdd')||type) fstr,MASTER_ID AS FORM_ID," +
                               "upper(MASTER_NAME) AS FORM_NAME FROM MASTER_SETTING WHERE TYPE='DPC' AND SUBSTR(MASTER_ID,1,1)='8' AND status='Y'";
                               //"CLIENT_ID='" + clientid_mst + "' AND CLIENT_UNIT_ID='" + unitid_mst + "'";
                            break;
                        case "ORDTYPE":
                            string ctltype = param1.Split(new string[] { "!~!~!~!~!" }, StringSplitOptions.None)[1].Trim();
                            string formtype = param1.Split(new string[] { "!~!~!~!~!" }, StringSplitOptions.None)[0].Trim();
                            if (ctltype.Equals("KPO") && formtype.Equals("84"))
                            {
                                cmd = "select (master_id||to_char(vch_date,'yyyymmdd')||type) fstr, master_id,master_name from master_setting " +
                                    "where type='" + ctltype + "' and substr(master_id,1,1)='5' and find_in_set(client_id,'" + clientid_mst + "')=1 and " +
                                    "find_in_set(client_unit_id,'" + unitid_mst + "')=1 and master_id not in (select param2 from controls where type='PRN'" +
                                    " and param1='" + formtype + "' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "')";
                            }
                            else if (ctltype.Equals("KPO") && !formtype.Equals("84"))
                            {
                                cmd = "select (master_id||to_char(vch_date,'yyyymmdd')||type) fstr, master_id,master_name from master_setting " +
                                    "where type='" + ctltype + "' and substr(master_id,1,1)='4' and find_in_set(client_id,'" + clientid_mst + "')=1 and " +
                                    "find_in_set(client_unit_id,'" + unitid_mst + "')=1 and master_id not in (select param2 from controls where type='PRN'" +
                                    " and param1='" + formtype + "' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "')";
                            }
                            else
                            {
                                cmd = "select (master_id||to_char(vch_date,'yyyymmdd')||type) fstr, master_id,master_name from master_setting " +
    "where type='" + ctltype + "' and find_in_set(client_id,'" + clientid_mst + "')=1 and " +
    "find_in_set(client_unit_id,'" + unitid_mst + "')=1 and master_id not in (select param2 from controls where type='PRN'" +
    " and param1='" + formtype + "' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "')";
                            }
                            break;
                    }
                    break;
                #endregion

                #region sms template
                case "sms_tmp":
                    switch (btnval.ToUpper())
                    {
                        case "VIEW":
                        case "EDIT":

                            cmd = "select (client_id||client_unit_id|| vch_num || to_char(vch_date, 'yyyymmdd') || type) as fstr,col2 as Temp_Set_For, (case when col9 ='Y' then 'Yes' else 'No' end ) as Send_Admin_Mail " +
                                ", (case when col10 = 'Y' then 'Yes' else 'No' end ) as Send_Admin_SMS," +
                                "(case when col11 = 'Y' then 'Yes' else 'No' end ) as Send_Outside_Mail" +
                                ", (case when col12 = 'Y' then 'Yes' else 'No' end ) as Send_Outside_SMS ," +
                                "col1 as Body_Text,col8 as Email_Heading from enx_tab where type = 'TMP' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";

                            break;

                    }
                    break;
                #endregion

                #region cascade_ddl

                case "cascade_ddl":

                    if (sgen.GetSession(MyGuid, "CD_TYPEMST").ToString() != null && sgen.GetSession(MyGuid, "CD_CONDMST").ToString() != null && sgen.GetSession(MyGuid, "CD_MIDMST").ToString() != null)
                    {
                        type = sgen.GetSession(MyGuid, "CD_TYPEMST").ToString().Trim();
                        where = sgen.GetSession(MyGuid, "CD_CONDMST").ToString().Trim();
                        minid = sgen.GetSession(MyGuid, "CD_MIDMST").ToString().Trim();

                        if (where.Trim().Contains("client_id") && where.Trim().Contains("client_unit_id")) cond = " and find_in_set(a.client_id,'" + clientid_mst + "')=1 and find_in_set(a.client_unit_id,'" + unitid_mst + "')=1";
                        else if (where.Trim().Contains("client_id")) cond = " and find_in_set(a.client_id,'" + clientid_mst + "')=1";
                        else if (where.Trim().Contains("client_unit_id")) cond = " and find_in_set(a.client_unit_id,'" + unitid_mst + "')=1";
                        else cond = " and find_in_set(a.client_id,'" + clientid_mst + "')=1 and find_in_set(a.client_unit_id,'" + unitid_mst + "')=1";

                        if (minid.Trim() == "1008.21")
                        {
                            join_mst = " inner join master_setting b on b.master_id=a.classid and b.type='MDP' and find_in_set(b.client_unit_id,a.client_unit_id)=1 ";
                            col_mst = "a.master_id DocNo,a.master_name SubDept,b.master_name Dept,a.group_name Description,";
                        }

                        if (minid.Trim() == "35001.2")
                        {
                            join_mst = " inner join master_setting b on b.master_id=a.classid and b.type='SCH' and find_in_set(b.client_unit_id,a.client_unit_id)=1 ";
                            col_mst = "a.master_id DocNo,a.master_name scheme_name,b.master_name scheme_type,a.group_name Description,";
                        }

                        switch (btnval.ToUpper())
                        {
                            case "EDIT":
                                cmd = "select (a.master_id||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr," + col_mst + "" +
                                     "(CASE when a.status = 'Y' THEN 'Active' when a.status = '' THEN 'Active' when a.status = 'N' THEN 'Inactive' end) Status " +
                                    "from master_setting a " + join_mst + "" +
                                    "where a.type in ('" + type + "','DD" + type + "')" + cond + "";
                                break;

                            case "EXT":
                                cmd = "select (a.master_id||to_char(a.vch_date,'yyyymmdd')||a.type||a.classid) as fstr," + col_mst + "" +
                                     "(CASE when a.status = 'Y' THEN 'Active' when a.status = '' THEN 'Active' when a.status = 'N' THEN 'Inactive' end) Status " +
                                    "from master_setting a " + join_mst + "" +
                                    "where a.type in ('" + type + "','DD" + type + "')" + cond + "";
                                break;

                            case "DETAIL":
                                cmd = "select (a.master_id||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr," + col_mst + "" +
                                    "(CASE when a.status = 'Y' THEN 'Active' when a.status = '' THEN 'Active' when a.status = 'N' THEN 'Inactive' end) Status " +
                                    "from master_setting a " + join_mst + "" +
                                    "where a.type in ('" + type + "','DD" + type + "')";
                                break;

                            case "UNIT":
                                cmd = "select (cup_id||company_profile_id||to_char(vch_date,'yyyymmdd')) fstr,company_profile_id Company_d,(unit_name||' ('||cup_id||')') " +
                                    "Unit_Name from company_unit_profile where unit_status='1' and cup_id<>'" + unitid_mst + "' order by unit_name asc";
                                break;
                        }
                    }
                    break;

                #endregion

                #region frequency master
                case "freq_mst":
                        switch (btnval.ToUpper())
                        {
                            case "EDIT":
                            case "DETAIL":
                            cmd = "select (a.master_id||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr,a.master_id as Doc_No,a.master_name as Name," +
                                  "a.classid as rem_time_in_days,a.SUBJECTID frq_count,a.SECTIONID as interval_in_days,a.client_unit_id Unit_id, " +
                                  "(b.First_name|| ' '|| replace(nvl(b.middle_name, ''),'0','')|| ' '|| replace(nvl(b.last_name,''),'0','')) as Ent_By," +
                                  "to_char(convert_tz(a.master_EntDate, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Ent_Date " +
                                  "from master_setting a " +
                                  "left join user_details b on a.master_entby = b.vch_num and b.type = 'CPR' " +
                                  "where a.type = 'IIT' and a.client_unit_id='" + unitid_mst + "' order by a.master_id asc";
                                break;
                    }
                    break;

                    #endregion
            }
            sgen.open_grid(title, cmd, sgen.Make_int(seektype));
            ViewBag.pageurl = sgen.GetSession(MyGuid, "pageurl").ToString();
        }
        #endregion

        //========================callback==============

        #region call back
        public List<Tmodelmain> CallbackFun(string btnval, string edmode, string viewName, string controllerName, List<Tmodelmain> model)
        {
            string L1_type = "", L2_type = "", L3_type = "", L4_type = "", L5_type = "";
            string L_cond = "", locsep = "";
            String URL = sgen.GetSession(MyGuid, "SSEEKVAL").ToString();
            List<Tmodelmain> rpt_model = new List<Tmodelmain>();
            List<SelectListItem> grptype = new List<SelectListItem>();
            List<SelectListItem> sotype = new List<SelectListItem>();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<SelectListItem> mod2 = new List<SelectListItem>();
            List<SelectListItem> mod3 = new List<SelectListItem>();
            List<SelectListItem> mod4 = new List<SelectListItem>();
            DataTable dtt = new DataTable();
            DataTable dt = new DataTable();
            switch (viewName.ToLower())
            {
                #region master_ctrl

                case "master_ctrl":

                    switch (btnval.ToUpper())
                    {
                        case "EDIT":

                            mq = "select a.rec_id,a.client_name,a.master_id,a.group_refrence_number,a.master_name,a.master_entby as ent_by,a.classid digit,sectionid deflt," +
                                "to_char(a.master_entdate,'" + sgen.GetSaveSqlDateFormat() + "') as ent_date,a.status,a.master_editby edit_by," +
                                "to_char(a.master_editdate,'" + sgen.GetSaveSqlDateFormat() + "') as edit_date,a.client_id,a.client_unit_id," +
                                "to_char(convert_tz(Inactive_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') inactive_dt " +
                                "from " + model[0].Col10.Trim() + " a where (a.master_id|| to_char(a.vch_date, 'yyyymmdd')|| a.type)='" + URL + "' ";
                            dtt = sgen.getdata(userCode, mq);
                            var tm = model.FirstOrDefault();
                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                                //model[0].Col5 = dtt.Rows[0]["edit_by"].ToString();
                                //model[0].Col6 = dtt.Rows[0]["edit_date"].ToString();
                                model[0].Col7 = dtt.Rows[0]["rec_id"].ToString();
                                model[0].Col8 = "(master_id|| to_char(vch_date, 'yyyymmdd')|| type) = '" + URL + "'";
                                model[0].Col9 = tm.Col9;
                                model[0].Col10 = tm.Col10;
                                model[0].Col11 = tm.Col11;
                                model[0].Col12 = tm.Col12;
                                model[0].Col13 = "Update";
                                model[0].Col14 = tm.Col14;
                                model[0].Col15 = tm.Col15;
                                model[0].Col100 = "Update & New";
                                model[0].Col121 = "U";
                                model[0].Col122 = "<u>U</u>pdate";
                                model[0].Col123 = "Update & Ne<u>w</u>";
                                model[0].Chk1 = dtt.Rows[0]["deflt"].ToString() == "Y" ? true : false;

                                model[0].Col16 = dtt.Rows[0]["master_id"].ToString();
                                model[0].Col17 = tm.Col17;
                                model[0].Col18 = dtt.Rows[0]["client_name"].ToString();
                                model[0].Col19 = tm.Col19;
                                model[0].Col20 = dtt.Rows[0]["master_name"].ToString();
                                model[0].Col21 = dtt.Rows[0]["status"].ToString() == "Y" ? "Active" : "Inactive";
                                if (model[0].Col14.Equals("17001.26"))
                                {
                                    model[0].Col25 = dtt.Rows[0]["group_refrence_number"].ToString();
                                }
                                if (tm.Col14.Trim().Equals("22007.4") || tm.Col14.Trim().Equals("20006.4") || tm.Col14.Trim().Equals("28005.5"))
                                {
                                    model[0].Col22 = dtt.Rows[0]["digit"].ToString();
                                    model[0].Col99 = sgen.GetSession(MyGuid, "LOCALCUR").ToString();
                                }


                            }
                            break;

                        case "EXT":
                            mq = "select master_id,master_name,group_name,client_id,status,content,client_unit_id,vch_num,rec_id," +
                                "master_entby,master_entdate,master_editby,master_editdate,client_name from master_setting where  " +
                                 "(master_id||to_char(vch_date, 'yyyymmdd')|| type) in ('" + URL + "')";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "MSTDATA", URL.Trim());
                                Make_query(viewName.ToLower(), "Select Unit", "UNIT", "2", "");
                                ViewBag.scripCall += "callFoo('Select Unit');";
                            }
                            break;

                        case "UNIT":
                            try
                            {
                                string currdate = sgen.server_datetime(userCode);
                                ent_date = currdate;
                                string[] uts = URL.Replace("','", ",").Split(',');
                                string[] mst_data = sgen.GetSession(MyGuid, "MSTDATA").ToString().Replace("','", ",").Split(',');
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

                                        cp.Add(clientid_mst);
                                        up.Add(unitid_mst);
                                        cp.Distinct().ToList();
                                        up.Distinct().ToList();

                                        mq = "update master_setting set master_editby='" + userid_mst + "',master_editdate=TO_DATE('" + currdate + "', 'YYYY-MM-DD HH24:MI:SS')," +
                                            "client_id='" + String.Join(",", cp) + "',client_unit_id='" + String.Join(",", up) + "' " +
                                            "where (master_id||to_char(vch_date, 'yyyymmdd')|| type)='" + mst + "'";
                                        res = sgen.execute_cmd(userCode, mq);
                                    }
                                }
                                catch (Exception ex) { res = false; }

                                #endregion

                                if (res)
                                {
                                    List<string> apps = new List<string>();
                                    foreach (var key in HttpContext.Application.Keys)
                                    {
                                        if (key.ToString().ToUpper().Contains("_U_" + userCode + ""))
                                        {
                                            apps.Add(key.ToString());
                                        }
                                    }
                                    foreach (var ap in apps)
                                    {
                                        HttpContext.Application[ap.ToString()] = null;
                                    }

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

                        case "PARTY":
                            mq = "select a.c_name as name,a.vch_num from clients_mst a " +
                         "where a.vch_num||To_Char(a.vch_date, 'yyyymmdd')|| a.type = '" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model[0].Col25 = dt.Rows[0]["vch_num"].ToString();
                            }
                            break;
                    }
                    break;

                #endregion

                #region doc_master

                case "doc_master":

                    switch (btnval.ToUpper())
                    {
                        //case "EDIT":
                        //case "VIEW":
                        //case "EXT":
                        case "MTYPE":
                            if (m_module3.ToLower() == "acctmain")
                            {
                                mq = "select master_id,master_name from master_setting where (master_id||TO_CHAR(vch_date,'yyyymmdd')||type) = '" + URL + "'";
                                dtt = sgen.getdata(userCode, mq);
                                if (dtt.Rows.Count > 0)
                                {
                                    Make_query(viewName.ToLower(), "Select Master Type Value", "TYPEVAL", "1", dtt.Rows[0]["master_id"].ToString());
                                    ViewBag.scripCall += "callFoo('Select Master Type Value');";
                                }
                            }
                            else
                            {
                                Make_query(viewName.ToLower(), "Select Master Type Value", "TYPEVAL", "1", URL);
                                ViewBag.scripCall += "callFoo('Select Master Type Value');";
                            }
                            break;
                        case "MEXT":
                            if (m_module3.ToLower() == "acctmain")
                            {
                                mq = "select master_id,master_name from master_setting where (master_id||TO_CHAR(vch_date,'yyyymmdd')||type) = '" + URL + "'";
                                dtt = sgen.getdata(userCode, mq);
                                if (dtt.Rows.Count > 0)
                                {
                                    Make_query(viewName.ToLower(), "Select Master Type Value", "TYPEVAL", "1", dtt.Rows[0]["master_id"].ToString());
                                    ViewBag.scripCall += "callFoo('Select Master Type Value');";
                                }
                            }
                            else
                            {
                                Make_query(viewName.ToLower(), "Select Master Type Value", "EXT", "2", URL);
                                ViewBag.scripCall += "callFoo('Select Master Type Value');";
                            }
                            break;
                        case "TYPEVAL":

                            //    Make_query(viewName.ToLower(), "Select Master Type Value", "TYPEVAL", "1", URL);
                            //    ViewBag.scripCall += "callFoo('Select Master Type Value');";
                            //    break;

                            //case "EDIT":

                            //mq = "select a.rec_id,a.client_name description,a.master_id,a.master_name,a.classid iso,a.sectionid pattern,a.master_entby as ent_by,a.master_entdate as ent_date," +
                            //    "a.vch_date,a.status,a.master_editby edit_by,a.master_editdate as edit_date,a.client_id,a.client_unit_id,group_name evaltype from " + model[0].Col10.Trim() + " a " +
                            //    "where (a.master_id||to_char(a.vch_date,'yyyymmdd')||a.type)='" + URL + "' ";

                            mq = "select a.rec_id,a.client_name description,a.master_id,a.group_refrence_number,a.master_name,a.classid iso,a.sectionid pattern,a.master_entby as ent_by,a.master_entdate as ent_date," +
                               "a.vch_date,a.status,a.master_editby edit_by,a.master_editdate as edit_date,a.client_id,a.client_unit_id,a.type from " + model[0].Col10.Trim() + " a " +
                               "where (a.master_id||to_char(a.vch_date,'yyyymmdd')||a.type)='" + URL + "' ";
                            dtt = sgen.getdata(userCode, mq);
                            var tm = model.FirstOrDefault();
                            if (dtt.Rows.Count > 0)
                            {
                                #region item group                         
                                //mq = "select a.classid,(a.master_name||' ('||a.classid||')') master_name from master_setting a where a.type='KGP'";
                                //dt = sgen.getdata(userCode, mq);
                                //if (dt.Rows.Count > 0)
                                //{
                                //    foreach (DataRow dr in dt.Rows)
                                //    {
                                //        grptype.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["classid"].ToString() });
                                //    }
                                //    TempData[MyGuid + "_Tlist1"] = grptype;
                                //}

                                //model[0].TList1 = grptype;
                                //model[0].SelectedItems1 = new string[] { "" };

                                #endregion

                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                                model[0].Col5 = dtt.Rows[0]["edit_by"].ToString();
                                model[0].Col6 = dtt.Rows[0]["edit_date"].ToString();
                                model[0].Col7 = dtt.Rows[0]["rec_id"].ToString();
                                model[0].Col8 = "(master_id||to_char(vch_date,'yyyymmdd')||type) = '" + URL + "'";
                                model[0].Col9 = sgen.GetSession(MyGuid, "MTYPE").ToString();
                                model[0].Col10 = tm.Col10;
                                model[0].Col11 = sgen.GetSession(MyGuid, "MCOND").ToString();
                                model[0].Col12 = dtt.Rows[0]["type"].ToString();
                                model[0].Col13 = "Update";
                                model[0].Col14 = tm.Col14;
                                model[0].Col15 = tm.Col15;
                                model[0].Col16 = dtt.Rows[0]["master_id"].ToString();
                                model[0].Col17 = dtt.Rows[0]["iso"].ToString();
                                model[0].Col18 = dtt.Rows[0]["pattern"].ToString();
                                model[0].Col19 = tm.Col19;
                                model[0].Col20 = dtt.Rows[0]["master_name"].ToString();
                                model[0].Col21 = dtt.Rows[0]["status"].ToString() == "Y" ? "Active" : "Inactive";
                                model[0].Col22 = dtt.Rows[0]["description"].ToString();
                                model[0].Col25 = dtt.Rows[0]["group_refrence_number"].ToString();
                                //model[0].Col23 = tm.Col23;
                                //model[0].Col24 = dtt.Rows[0]["evaltype"].ToString();
                                //model[0].Col100 = "Update & New";
                                //model[0].TList1 = grptype;
                                //if (model[0].Col12 == "KIG") model[0].SelectedItems1 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["master_id"].ToString().Substring(0, 1)).Distinct()).Split(',');
                                //else model[0].SelectedItems1 = new string[] { "" };
                            }
                            break;
                        case "EXT":
                            mq = "select master_id,master_name,group_name,client_id,status,content,client_unit_id,vch_num,rec_id," +
                                "master_entby,master_entdate,master_editby,master_editdate,client_name from master_setting where  " +
                                 "(master_id||to_char(vch_date, 'yyyymmdd')|| type) in ('" + URL + "')";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "DOCMSTDATA", URL.Trim());
                                Make_query(viewName.ToLower(), "Select Unit", "UNIT", "2", "");
                                ViewBag.scripCall += "callFoo('Select Unit');";
                            }
                            break;
                        case "UNIT":
                            try
                            {
                                mq1 = "";
                                string currdate = sgen.server_datetime(userCode);
                                ent_date = currdate;
                                string[] uts = URL.Replace("','", ",").Split(',');
                                string[] mst_data = sgen.GetSession(MyGuid, "DOCMSTDATA").ToString().Replace("','", ",").Split(',');
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

                                        cp.Add(clientid_mst);
                                        up.Add(unitid_mst);
                                        cp.Distinct().ToList();
                                        up.Distinct().ToList();

                                        mq = "update master_setting set master_editby='" + userid_mst + "',master_editdate=TO_DATE('" + currdate + "', 'YYYY-MM-DD HH24:MI:SS')," +
                                            "client_id='" + String.Join(",", cp) + "',client_unit_id='" + String.Join(",", up) + "' " +
                                            "where (master_id||to_char(vch_date, 'yyyymmdd')|| type)='" + mst + "'";
                                        res = sgen.execute_cmd(userCode, mq);
                                        if (!res)
                                        {
                                            mq1 = mq1 + mst + ",";
                                        }
                                    }
                                }
                                catch (Exception ex) { res = false; }

                                #endregion

                                if (mq1 != "")
                                {
                                    ViewBag.vnew = "";
                                    ViewBag.vedit = "";
                                    ViewBag.vsave = "disabled='disabled'";
                                    ViewBag.vsavenew = "disabled='disabled'";
                                    ViewBag.scripCall += "mytoast('warning', 'toast-top-right', 'Data extended successfully, except these record id (" + mq1.TrimEnd(',').Trim().Substring(0, 2).Trim() + ")');disableForm(); ";
                                }
                                else { ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Extended Successfully Except');disableForm(); "; }
                            }
                            catch (Exception ex) { sgen.showmsg(1, ex.Message.ToString(), 0); }
                            break;

                        case "PARTY":
                            mq = "select a.c_name as name,a.vch_num from clients_mst a " +
                         "where a.vch_num||To_Char(a.vch_date, 'yyyymmdd')|| a.type = '" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model[0].Col25 = dt.Rows[0]["vch_num"].ToString();
                            }
                            break;
                    }
                    break;

                #endregion

                #region cascade_mst

                case "cascade_mst":

                    switch (btnval.ToUpper())
                    {
                        case "EDIT":

                            if (sgen.GetSession(MyGuid, "CS_LOCSEP") != null) locsep = sgen.GetSession(MyGuid, "CS_LOCSEP").ToString().Trim();

                            if (model[0].Col14.Trim().Equals("17001.16"))//floor (L2)
                            {
                                L_cond = "a.classid mst_id,";
                            }
                            else if (model[0].Col14.Trim().Equals("17001.17")) //room master (L3)
                            {
                                L_cond = "(a.classid||'" + locsep + "'||a.sectionid) mst_id,";
                            }
                            else if (model[0].Col14.Trim().Equals("17001.18"))//rack master (L4)
                            {
                                L_cond = "(a.classid||'" + locsep + "'||a.sectionid||'" + locsep + "'||a.client_name) mst_id,";
                            }
                            else if (model[0].Col14.Trim().Equals("17001.19"))//bin master (L5)
                            {
                                L_cond = "(a.classid||'" + locsep + "'||a.sectionid||'" + locsep + "'||a.client_name||'" + locsep + "'||a.subjectid) mst_id,";
                            }

                            mq = "select a.rec_id,a.master_id,a.master_name," + L_cond + "a.group_name,a.master_entby ent_by,a.master_entdate as ent_date," +
                            "a.vch_date,a.status,a.master_editby edit_by,a.master_editdate edit_date,a.client_id,a.client_unit_id from " + model[0].Col10.Trim() + " a " +
                            "where (a.client_id||a.client_unit_id||a.master_id||to_char(a.vch_date,'yyyymmdd')||a.type)='" + URL + "'";
                            dtt = sgen.getdata(userCode, mq);
                            var tm = model.FirstOrDefault();
                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                L1_type = "HBM";
                                L2_type = "IN0";
                                L3_type = "IN1";
                                L4_type = "IN2";
                                L5_type = "IN3";

                                if (model[0].Col14.Trim().Equals("17001.16"))//floor (L2)
                                {
                                    #region building L1

                                    mq = "select master_id,master_name from master_setting where type='" + L1_type + "'" + model[0].Col11 + "";
                                    dt = sgen.getdata(userCode, mq);
                                    if (dt.Rows.Count > 0)
                                    {
                                        foreach (DataRow dr in dt.Rows)
                                        {
                                            mod1.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                                        }
                                    }
                                    TempData[MyGuid + "_Tlist1"] = mod1;

                                    #endregion
                                }
                                else if (model[0].Col14.Trim().Equals("17001.17")) //room master (L3)
                                {
                                    #region floor L2

                                    mq = "select (l1.master_id||ct.param1||l2.master_id) master_id,(l1.master_name||ct.param1||l2.master_name) master_name from master_setting l2 " +
                                        "inner join master_setting l1 on l1.master_id = l2.classid and l1.type = '" + L1_type + "' and l1.client_id = l2.client_id and l1.client_unit_id = l2.client_unit_id " +
                                        "inner join controls ct on 1 = 1 and ct.id = '000010' and ct.type = 'SEP' and ct.m_module3 = 'invtmain' " +
                                        "where l2.type = '" + L2_type + "' and l2.client_id = '" + clientid_mst + "' and l2.client_unit_id = '" + unitid_mst + "'";

                                    dt = sgen.getdata(userCode, mq);
                                    if (dt.Rows.Count > 0)
                                    {
                                        foreach (DataRow dr in dt.Rows)
                                        {
                                            mod1.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                                        }
                                    }
                                    TempData[MyGuid + "_Tlist1"] = mod1;

                                    #endregion
                                }
                                else if (model[0].Col14.Trim().Equals("17001.18"))//rack master (L4)
                                {
                                    #region room L3

                                    mq = "select(l1.master_id || ct.param1 || l2.master_id || ct.param1 || l3.master_id) master_id,(l1.master_name || ct.param1 || l2.master_name || ct.param1 || l3.master_name) master_name " +
                                        "from master_setting l3 " +
                                        "inner join master_setting l1 on l1.master_id = l3.classid and l1.type = '" + L1_type + "' and l1.client_id = l3.client_id and l1.client_unit_id = l3.client_unit_id " +
                                        "inner join master_setting l2 on l2.master_id = l3.sectionid and l2.type = '" + L2_type + "' and l2.client_id = l3.client_id and l2.client_unit_id = l3.client_unit_id " +
                                        "inner join controls ct on 1 = 1 and ct.id = '000010' and ct.type = 'SEP' and ct.m_module3 = 'invtmain' " +
                                        "where l3.type = '" + L3_type + "' and l3.client_id = '" + clientid_mst + "' and l3.client_unit_id = '" + unitid_mst + "'";

                                    dt = sgen.getdata(userCode, mq);
                                    if (dt.Rows.Count > 0)
                                    {
                                        foreach (DataRow dr in dt.Rows)
                                        {
                                            mod1.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                                        }
                                    }
                                    TempData[MyGuid + "_Tlist1"] = mod1;

                                    #endregion
                                }
                                else if (model[0].Col14.Trim().Equals("17001.19"))//bin master (L5)
                                {
                                    #region rack L4

                                    mq = "select (l1.master_id||ct.param1||l2.master_id||ct.param1||l3.master_id||ct.param1||l4.master_id) master_id,(l1.master_name||ct.param1||l2.master_name||ct.param1||l3.master_name||ct.param1||l4.master_name) master_name " +
                                        "from master_setting l4 " +
                                        "inner join master_setting l1 on l1.master_id = l4.classid and l1.type = '" + L1_type + "' and l1.client_id = l4.client_id and l1.client_unit_id = l4.client_unit_id " +
                                        "inner join master_setting l2 on l2.master_id = l4.sectionid and l2.type = '" + L2_type + "' and l2.client_id = l4.client_id and l2.client_unit_id = l4.client_unit_id " +
                                        "inner join master_setting l3 on l3.master_id = l4.client_name and l3.type = '" + L3_type + "' and l3.client_id = l4.client_id and l3.client_unit_id = l4.client_unit_id " +
                                        "inner join controls ct on 1 = 1 and ct.id = '000010' and ct.type = 'SEP' and ct.m_module3 = 'invtmain' " +
                                        "where l4.type = '" + L4_type + "' and l4.client_id = '" + clientid_mst + "' and l4.client_unit_id = '" + unitid_mst + "'";

                                    dt = sgen.getdata(userCode, mq);
                                    if (dt.Rows.Count > 0)
                                    {
                                        foreach (DataRow dr in dt.Rows)
                                        {
                                            mod1.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                                        }
                                    }
                                    TempData[MyGuid + "_Tlist1"] = mod1;

                                    #endregion
                                }



                                //if (m_module3 == "invtmain")
                                //{
                                //#region building

                                //mq = "select master_id,master_name from master_setting where type='HBM'" + tm.Col11 + "";
                                //    dt = sgen.getdata(userCode, mq);
                                //    if (dt.Rows.Count > 0)
                                //    {
                                //        foreach (DataRow dr in dt.Rows)
                                //        {
                                //            mod1.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                                //        }
                                //    }
                                //    TempData[MyGuid + "_Tlist1"] = mod1;
                                //    #endregion

                                //    #region Floor                   
                                //    mq = "select master_id,master_name from master_setting where type='IN0'" + tm.Col11 + "";
                                //    dt = sgen.getdata(userCode, mq);
                                //    if (dt.Rows.Count > 0)
                                //    {
                                //        foreach (DataRow dr in dt.Rows)
                                //        {
                                //            mod2.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                                //        }
                                //    }
                                //    TempData[MyGuid + "_Tlist2"] = mod2;
                                //    #endregion

                                //    #region Room
                                //    mq = "select master_id,master_name from master_setting where type='IN1'" + tm.Col11 + "";
                                //    dt = sgen.getdata(userCode, mq);
                                //    if (dt.Rows.Count > 0)
                                //    {
                                //        foreach (DataRow dr in dt.Rows)
                                //        {
                                //            mod3.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                                //        }
                                //    }
                                //    TempData[MyGuid + "_Tlist3"] = mod3;
                                //    #endregion

                                //    #region Rack                    
                                //    mq = "select master_id,master_name from master_setting where type='IN2'" + tm.Col11 + "";
                                //    dt = sgen.getdata(userCode, mq);
                                //    if (dt.Rows.Count > 0)
                                //    {
                                //        foreach (DataRow dr in dt.Rows)
                                //        {
                                //            mod4.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                                //        }
                                //    }
                                //    TempData[MyGuid + "_Tlist4"] = mod4;
                                //    #endregion
                                //}

                                model[0].TList1 = mod1;
                                model[0].SelectedItems1 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["mst_id"].ToString()).Distinct()).Split(',');

                                //model[0].TList2 = mod2;
                                //model[0].SelectedItems2 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["sectionid"].ToString()).Distinct()).Split(',');

                                //model[0].TList3 = mod3;
                                //model[0].SelectedItems3 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["client_name"].ToString()).Distinct()).Split(',');

                                //model[0].TList4 = mod4;
                                //model[0].SelectedItems4 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["subjectid"].ToString()).Distinct()).Split(',');

                                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                                model[0].Col5 = dtt.Rows[0]["edit_by"].ToString();
                                model[0].Col6 = dtt.Rows[0]["edit_date"].ToString();
                                model[0].Col7 = dtt.Rows[0]["rec_id"].ToString();
                                model[0].Col8 = "(client_id||client_unit_id||master_id||to_char(vch_date,'yyyymmdd')||type) = '" + URL + "'";
                                model[0].Col9 = tm.Col9;
                                model[0].Col10 = tm.Col10;
                                model[0].Col11 = tm.Col11;
                                model[0].Col12 = tm.Col12;
                                model[0].Col13 = "Update";
                                model[0].Col14 = tm.Col14;
                                model[0].Col15 = tm.Col15;
                                model[0].Col16 = dtt.Rows[0]["master_id"].ToString();

                                model[0].Col32 = dtt.Rows[0]["master_name"].ToString();//name
                                model[0].Col34 = dtt.Rows[0]["group_name"].ToString();//description
                                model[0].Col33 = dtt.Rows[0]["status"].ToString() == "Y" ? "Active" : "Inactive";//status
                            }
                            break;
                    }
                    break;

                #endregion

                #region address_master
                case "address_master":
                    switch (btnval.ToUpper())

                    {
                        case "EDIT":
                            if (sgen.GetSession(MyGuid, "TYPE_MST").ToString() != null && sgen.GetSession(MyGuid, "COND_MST").ToString() != null)
                            {
                                type = sgen.GetSession(MyGuid, "TYPE_MST").ToString().Trim();

                                mq = "select * from " + model[0].Col10 + " where (rec_id||alpha_2||state_gst_code||type)='" + URL + "' and type='" + type + "' and Rownum = 1";
                                dtt = sgen.getdata(userCode, mq);
                                if (dtt.Rows.Count > 0)
                                {
                                    sgen.SetSession(MyGuid, "EDMODE", "Y");
                                    model[0].Col8 = "(rec_id||alpha_2||state_gst_code||type)='" + URL + "'";
                                    model[0].Col16 = dtt.Rows[0]["vch_num"].ToString();


                                    if (model[0].Col14 == "4007.6")
                                    {
                                        model[0].Col17 = dtt.Rows[0]["state_name"].ToString();
                                        model[0].Col18 = dtt.Rows[0]["district_name"].ToString();
                                    }
                                    if (model[0].Col14 == "7004.5" || model[0].Col14 == "9003.3.7")
                                    {
                                        model[0].Col18 = dtt.Rows[0]["teh_name"].ToString();
                                        model[0].Col17 = dtt.Rows[0]["district_name"].ToString();

                                    }
                                    if (model[0].Col14 == "7004.9" || model[0].Col14 == "9003.3.6")
                                    {
                                        model[0].Col17 = dtt.Rows[0]["teh_name"].ToString();
                                        model[0].Col18 = dtt.Rows[0]["v_name"].ToString();
                                        model[0].Col22 = dtt.Rows[0]["pincode"].ToString();


                                    }

                                    model[0].Col7 = dtt.Rows[0]["rec_id"].ToString();
                                    model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                                    model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                                    model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                    model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                    sgen.SetSession(MyGuid, "DTF", dtt);
                                    model[0].Col13 = "Update";
                                    model[0].Col100 = "Update & New";
                                    model[0].Col121 = "U";
                                    model[0].Col122 = "<u>U</u>pdate";
                                    model[0].Col123 = "Update & Ne<u>w</u>";
                                }

                            }
                            break;
                        case "STATE":
                            if (sgen.GetSession(MyGuid, "TYPE_MST").ToString() != null && sgen.GetSession(MyGuid, "COND_MST").ToString() != null)
                            {
                                type = sgen.GetSession(MyGuid, "TYPE_MST").ToString().Trim();
                                mq = "select * from " + model[0].Col10 + " where (alpha_2||state_gst_code||district_name)='" + URL + "' and Rownum = 1";
                                dt = sgen.getdata(userCode, mq);
                                if (dt.Rows.Count > 0)
                                {
                                    sgen.SetSession(MyGuid, "DTF", dt);
                                    model[0].Col17 = dt.Rows[0]["district_name"].ToString();

                                }

                            }
                            if (sgen.GetSession(MyGuid, "TYPE_MST").ToString() != null && sgen.GetSession(MyGuid, "COND_MST").ToString() != null)
                            {
                                type = sgen.GetSession(MyGuid, "TYPE_MST").ToString().Trim();
                                mq = "select * from " + model[0].Col10 + " where (alpha_2||state_gst_code)='" + URL + "'and Rownum = 1";
                                dt = sgen.getdata(userCode, mq);
                                if (dt.Rows.Count > 0)
                                {
                                    sgen.SetSession(MyGuid, "DTF", dt);
                                    model[0].Col17 = dt.Rows[0]["state_name"].ToString();

                                }
                            }

                            if (sgen.GetSession(MyGuid, "TYPE_MST").ToString() != null && sgen.GetSession(MyGuid, "COND_MST").ToString() != null)
                            {
                                type = sgen.GetSession(MyGuid, "TYPE_MST").ToString().Trim();
                                mq = "select * from " + model[0].Col10 + " where (alpha_2||state_gst_code||teh_name)='" + URL + "' and Rownum = 1";

                                dt = sgen.getdata(userCode, mq);
                                if (dt.Rows.Count > 0)
                                {
                                    sgen.SetSession(MyGuid, "DTF", dt);
                                    model[0].Col17 = dt.Rows[0]["teh_name"].ToString();

                                }
                            }
                            break;
                    }
                    break;
                #endregion

                #region Form Control
                case "form_ctrl":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            try
                            {
                                //model = getnew(model);
                                DataTable data = new DataTable();
                                var tmm = model.FirstOrDefault();
                                mq = "select rec_id, id as sr_no,name as new_name ,param1 as mand,param2 as visib,param4 as date_chk,upper(param3) as orignal_name" +
                                    ",(case when  (param8 IS NULL or param8='0' or param8='')   Then param5 else upper(param8) END ) page_name, param5,upper(param6) as param6,param7,param8,param9 from controls where " +
                                    " client_id||client_unit_id||type||param5='" + URL + "' order by id asc ";
                                data = sgen.getdata(userCode, mq);
                                model = new List<Tmodelmain>();
                                foreach (DataRow dr in data.Rows)
                                {
                                    Tmodelmain tm = new Tmodelmain();
                                    tm.Col18 = dr["orignal_name"].ToString();
                                    tm.Col19 = dr["sr_no"].ToString();
                                    tm.Col20 = dr["new_name"].ToString();
                                    tm.Col9 = dr["page_name"].ToString();
                                    tm.Col22 = dr["param6"].ToString();
                                    tm.Col23 = dr["param7"].ToString();
                                    tm.Col24 = dr["param8"].ToString();
                                    tm.Col25 = dr["param9"].ToString();
                                    tm.Col26 = dr["param5"].ToString();
                                    tm.Col8 = "client_id||client_unit_id||type||param5 = '" + URL + "'";
                                    tm.Col7 = dr["rec_id"].ToString();
                                    tm.Chk1 = (Convert.ToBoolean(dr["mand"].ToString() == "Y" || Convert.ToBoolean(dr["mand"].ToString() == "M") ? true : false));
                                    tm.Chk2 = Convert.ToBoolean(dr["visib"].ToString() == "Y" ? true : false);
                                    tm.Chk3 = Convert.ToBoolean(dr["date_chk"].ToString() == "Y" ? true : false);

                                    tm.Col21 = dr["mand"].ToString();
                                    model.Add(tm);
                                    model[0].Col13 = "Save";
                                    model[0].Col100 = "Save & New";
                                    model[0].Col121 = "S";
                                    model[0].Col122 = "<u>S</u>ave";
                                    model[0].Col123 = "Save & Ne<u>w</u>";
                                    model[0].Col14 = tmm.Col14;
                                    model[0].Col15 = tmm.Col15;
                                    if (dr["Param5"].ToString() == "MRN") { model[0].Col143 = "dtmrn"; }
                                    else if (dr["Param5"].ToString() == "PO") { model[0].Col143 = "dtpo"; }
                                    else if (dr["Param5"].ToString() == "ITEM/SERVICE MASTER") { model[0].Col143 = "dtitem"; }
                                    else if (dr["Param5"].ToString() == "PD") { model[0].Col143 = "dtpd"; }
                                    else { model[0].Col143 = dr["param5"].ToString(); }




                                }
                            }
                            catch (Exception ex) { }

                            break;

                    }
                    break;


                #endregion

                #region doc print config
                case "prn_ctrl":
                    switch (btnval.ToUpper())
                    {
                        case "NEW":
                            mq = "SELECT MASTER_ID,MASTER_NAME,master_type FROM MASTER_SETTING WHERE (client_id||client_unit_id||master_id||to_char(vch_date,'yyyymmdd')||type) = '" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model[0].Col9 = dt.Rows[0]["MASTER_NAME"].ToString();
                                model[0].Col22 = dt.Rows[0]["MASTER_ID"].ToString();
                                model[0].Col24 = dt.Rows[0]["master_type"].ToString();
                                #region ddl reports 
                                List<SelectListItem> mod5 = new List<SelectListItem>();
                                dt = new DataTable();
                                dt = sgen.getdata(userCode, "select master_id ,master_name from master_setting where type='IRF' and classid='" + model[0].Col22 + "'");
                                    //"find_in_set(client_id,'" + clientid_mst + "')=1 and find_in_set(client_unit_id,'" + unitid_mst + "')=1 and classid='" + model[0].Col22 + "'");
                                if (dt.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        mod5.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                                    }
                                    TempData[MyGuid + "_TList5"] = mod5;
                                }
                                model[0].TList5 = mod5;
                                model[0].SelectedItems5 = new string[] { "" };
                                #endregion                                
                                sgen.SetSession(MyGuid, "EDMODE", "N");
                                ViewBag.vnew = "disabled='disabled'";
                                ViewBag.vedit = "disabled='disabled'";
                                ViewBag.vsave = "";
                                ViewBag.vsavenew = "";
                                ViewBag.scripCall = "enableForm();";
                                mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "' " + model[0].Col11.Trim() + "";
                                vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                                model[0].Col16 = vch_num;
                                isedit = false;
                                Make_query(viewName.ToLower(), "Select Type", "ORDTYPE", "1", model[0].Col22 + "!~!~!~!~!" + model[0].Col24);
                                ViewBag.scripCall = "callFoo('Select Type');";
                            }
                            break;

                        case "VIEW":
                        case "EDIT":

                            mq = "select (k.client_id||k.client_unit_id||k.vch_num||to_char(k.vch_date,'yyyymmdd')||k.type) fstr,k.vch_num," +
                                "to_char(k.vch_date,'dd/mm/yyyy') vch_date,k.type,k.rec_id,k.client_id,k.client_unit_id,k.ent_by,k.ent_date," +
                          "k.col73 term_and_cond,k.col74 Back_page_terms,k.col1 doc_control_no,k.col2 ft,f.master_name formtype,k.col3 mt," +
                          "m.master_name msttype,k.col4 mstmain,k.col5 rpt from kc_tab k " +
                          "inner join master_setting f on f.master_id=k.col2 and f.type='DPC' " +
                          "inner join (select master_id,master_name from master_setting where type in ('KMR','KPO','KRQ')) m on m.master_id=k.col3 " +
                          "where (k.client_id || k.client_unit_id || k.vch_num||to_char(k.vch_date,'yyyymmdd')||k.type) = '" + URL + "'";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                                model[0].Col7 = dtt.Rows[0]["rec_id"].ToString();
                                model[0].Col8 = "client_id||client_unit_id||vch_num||to_char(vch_date,'yyyymmdd')||type = '" + URL + "'";
                                model[0].Col9 = " " + dtt.Rows[0]["formtype"].ToString() + " " + dtt.Rows[0]["msttype"].ToString();
                                model[0].Col12 = dtt.Rows[0]["type"].ToString();
                                model[0].Col13 = "Update";
                                model[0].Col16 = dtt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dtt.Rows[0]["term_and_cond"].ToString();
                                model[0].Col18 = dtt.Rows[0]["Back_page_terms"].ToString();
                                model[0].Col19 = dtt.Rows[0]["doc_control_no"].ToString();
                                model[0].Col22 = dtt.Rows[0]["ft"].ToString();
                                model[0].Col23 = dtt.Rows[0]["mt"].ToString();
                                model[0].Col24 = dtt.Rows[0]["mstmain"].ToString();
                                #region ddl reports 
                                List<SelectListItem> mod5 = new List<SelectListItem>();
                                dt = new DataTable();
                                dt = sgen.getdata(userCode, "select master_id ,master_name from master_setting where type='IRF' and classid='" + model[0].Col22 + "'");
                                if (dt.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        mod5.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                                    }
                                    TempData[MyGuid + "_TList5"] = mod5;
                                }
                                model[0].TList5 = mod5;
                                String[] L5 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["rpt"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems5 = L5;
                                #endregion

                                #region attachment
                                DataTable dtf = new DataTable();
                                dtf = sgen.getdata(userCode, "select client_id||client_unit_id||vch_num||to_char(vch_date,'yyyymmdd')||type as fstr, rec_id,file_name,file_path,col1,col2,description,Col3 from file_tab where client_id||client_unit_id ||vch_num || to_char(vch_date, 'yyyymmdd') || type = '" + URL + "' and col1='PRN'");
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
                                        if (model[0].Files1.Count == 2)
                                        {
                                            Files tm3 = new Files();
                                            tm3.FileName = "-";
                                            model[0].Files1.Add(tm3);
                                        }
                                        if (model[0].Files1.Count == 1)
                                        {
                                            Files tm3 = new Files();
                                            tm3.FileName = "-";
                                            model[0].Files1.Add(tm3);
                                            model[0].Files1.Add(tm3);
                                        }
                                    }
                                    catch (Exception err)
                                    {
                                        model[0].Files1 = new List<Files>();
                                        Files tm3 = new Files();
                                        tm3.FileName = "-";
                                        model[0].Files1.Add(tm3);
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
                                    model[0].Files1.Add(tm3);
                                }
                                #endregion
                            }
                            break;

                        case "FDEL":
                            string fid = sgen.GetSession(MyGuid, "delid").ToString();
                            res = sgen.execute_cmd(userCode, "Delete from file_tab WHERE rec_id='" + fid + "'  and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
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

                        case "ORDTYPE":
                            mq = "select master_id,upper(master_name) master_name,to_char(convert_tz(utc_timestamp(),'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') po_date " +
                                "from master_Setting where (master_id||To_Char(vch_date, 'yyyymmdd')||type) = '" + URL + "' ";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model[0].Col9 += "  " + dt.Rows[0]["master_name"].ToString();
                                sgen.SetSession(MyGuid, "EDMODE", "N");
                                model[0].Col23 = dt.Rows[0]["master_id"].ToString();
                                sgen.SetSession(MyGuid, "ord_type", model[0].Col23.Trim());
                            }
                            break;
                    }
                    break;
                #endregion

                #region sms_temp
                case "sms_tmp":
                    switch (btnval.ToUpper())
                    {



                        case "VIEW":
                        case "EDIT":
                            mq = "select a.vch_num,a.rec_id, a.client_id,a.client_unit_id,a.ent_by," +
                                " to_char(convert_tz(a.ent_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') ent_date," +
                                "a.col1,a.col3,a.col2,a.col8,a.col7,a.col9,a.col10,a.col11,a.col12,a.col13" +
                                "  from enx_tab a where (a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date, 'yyyymmdd')|| a.type) = '" + URL + "'";
                            dtt = sgen.getdata(userCode, mq);

                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                mod1 = new List<SelectListItem>();
                                model[0].Col16 = dtt.Rows[0]["vch_num"].ToString();
                                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                                model[0].Col7 = dtt.Rows[0]["rec_id"].ToString();
                                model[0].Col8 = "(client_id||client_unit_id||vch_num||to_char(vch_date, 'yyyymmdd')|| type) = '" + URL + "'";

                                #region temp_set_for 1
                                mod1 = new List<SelectListItem>();
                                dt = sgen.getdata(userCode, "select 'Account' as set_For from dual Union all select 'Prospect Data' as set_For from dual  ");
                                if (dt.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        mod1.Add(new SelectListItem { Text = dr["set_For"].ToString(), Value = dr["set_For"].ToString() });
                                    }
                                }

                                TempData[MyGuid + "_Tlist1"] = mod1;
                                model[0].TList1 = mod1;
                                model[0].SelectedItems1 = new string[] { "" };
                                #endregion


                                model[0].Chk1 = dtt.Rows[0]["col9"].ToString() == "Y" ? true : false;
                                model[0].Chk2 = dtt.Rows[0]["col10"].ToString() == "Y" ? true : false;
                                model[0].Chk3 = dtt.Rows[0]["col11"].ToString() == "Y" ? true : false;
                                model[0].Chk4 = dtt.Rows[0]["col12"].ToString() == "Y" ? true : false;
                                model[0].Chk5 = dtt.Rows[0]["col13"].ToString() == "Y" ? true : false;

                                model[0].Col18 = dtt.Rows[0]["col1"].ToString();
                                model[0].Col19 = dtt.Rows[0]["col3"].ToString();
                                model[0].Col17 = dtt.Rows[0]["col8"].ToString();
                                model[0].Col20 = dtt.Rows[0]["col7"].ToString();

                                String[] L1 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["col2"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems1 = L1;


                            }


                            break;



                    }
                    break;
                #endregion

                #region cascade_ddl

                case "cascade_ddl":

                    switch (btnval.ToUpper())
                    {
                        case "EDIT":

                            mq = "select a.rec_id,a.master_id,a.master_name,a.classid mstid,group_name descp,a.master_entby ent_by,a.master_entdate as ent_date,a.vch_date,a.status," +
                                "a.master_editby edit_by,a.master_editdate edit_date,a.client_id,a.client_unit_id from " + model[0].Col10.Trim() + " a " +
                                "where (a.master_id||to_char(a.vch_date,'yyyymmdd')||a.type)='" + URL + "'";
                            dtt = sgen.getdata(userCode, mq);
                            var tm = model.FirstOrDefault();
                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");

                                if (model[0].Col14.Trim().Equals("1008.21")) { mod1 = cmd_Fun.dept(userCode, clientid_mst, unitid_mst); }
                                if (model[0].Col14.Trim().Equals("35001.2")) { mod1 = cmd_Fun.schtype(userCode, unitid_mst); }


                                TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
                                model[0].SelectedItems1 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["mstid"].ToString()).Distinct()).Split(',');

                                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                                model[0].Col5 = dtt.Rows[0]["edit_by"].ToString();
                                model[0].Col6 = dtt.Rows[0]["edit_date"].ToString();
                                model[0].Col7 = dtt.Rows[0]["rec_id"].ToString();
                                model[0].Col8 = "(master_id||to_char(vch_date,'yyyymmdd')||type) = '" + URL + "'";
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
                                model[0].Col16 = dtt.Rows[0]["master_id"].ToString();
                                model[0].Col19 = dtt.Rows[0]["master_name"].ToString();//name
                                model[0].Col20 = dtt.Rows[0]["descp"].ToString();//description
                                model[0].Col21 = dtt.Rows[0]["status"].ToString() == "Y" ? "Active" : "Inactive";//status
                            }
                            break;

                        case "EXT":
                            mq = "select master_id,master_name,group_name,classid,client_id,status,content,client_unit_id,vch_num,rec_id," +
                                "master_entby,master_entdate,master_editby,master_editdate,client_name from master_setting where  " +
                                 "(master_id||to_char(vch_date, 'yyyymmdd')|| type||classid) in ('" + URL + "')";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "MSTDATA", URL.Trim());
                                Make_query(viewName.ToLower(), "Select Unit", "UNIT", "2", "");
                                ViewBag.scripCall += "callFoo('Select Unit');disableForm();";
                            }
                            break;

                        case "UNIT":
                            try
                            {
                                string currdate = sgen.server_datetime(userCode);
                                ent_date = currdate;
                                string[] uts = URL.Replace("','", ",").Split(',');
                                string[] mst_data = sgen.GetSession(MyGuid, "MSTDATA").ToString().Replace("','", ",").Split(',');
                                #region dtstr                                

                                try
                                {
                                    foreach (string mst in mst_data)
                                    {
                                        List<string> cp = new List<string>();
                                        List<string> up = new List<string>();
                                        foreach (string ut in uts)
                                        {
                                            string dpt = mst.Substring(14, (mst.Length - 14)).Trim();
                                            mq = sgen.seekval(userCode, "select master_id from master_setting where type='MDP' and master_id='" + dpt + "' and " +
                                                "find_in_set(client_unit_id,'" + ut.Substring(0, 6) + "')=1", "master_id");
                                            if (mq != "0")
                                            {
                                                cp.Add(ut.Substring(6, 3).Trim());
                                                up.Add(ut.Substring(0, 6).Trim());
                                            }
                                            else
                                            {
                                                ViewBag.scripCall = "showmsgJS(1, 'Record not extended, extend department first', 0);";
                                                return model;
                                            }
                                        }

                                        cp.Add(clientid_mst);
                                        up.Add(unitid_mst);
                                        cp.Distinct();
                                        up.Distinct();

                                        mq = "update master_setting set master_editby='" + userid_mst + "',master_editdate=TO_DATE('" + currdate + "', 'YYYY-MM-DD HH24:MI:SS')," +
                                            "client_id='" + String.Join(",", cp) + "',client_unit_id='" + String.Join(",", up) + "' " +
                                            "where (master_id||to_char(vch_date, 'yyyymmdd')|| type||classid)='" + mst + "'";
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

                #region frequency master

                case "freq_mst":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select a.rec_id,a.client_name,a.master_id,a.sectionid,a.SECTIONTYPE,a.SUBJECTID,a.master_name,a.master_entby as ent_by,a.classid," +
                                "to_char(a.master_entdate,'" + sgen.GetSaveSqlDateFormat() + "') as ent_date,a.status,a.master_editby edit_by," +
                                "to_char(a.master_editdate,'" + sgen.GetSaveSqlDateFormat() + "') as edit_date,a.client_id,a.client_unit_id," +
                                "to_char(convert_tz(Inactive_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') inactive_dt " +
                                "from " + model[0].Col10.Trim() + " a where (a.master_id|| to_char(a.vch_date, 'yyyymmdd')|| a.type)='" + URL + "' ";
                            dtt = sgen.getdata(userCode, mq);
                            var tm = model.FirstOrDefault();
                            mod1.Add(new SelectListItem { Text = "YEAR", Value = "YEAR" });
                            mod1.Add(new SelectListItem { Text = "MONTH", Value = "MONTH" });
                            mod1.Add(new SelectListItem { Text = "DAY", Value = "DAY" });
                            TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                                model[0].Col7 = dtt.Rows[0]["rec_id"].ToString();
                                model[0].Col8 = "(master_id|| to_char(vch_date, 'yyyymmdd')|| type) = '" + URL + "'";
                                model[0].Col9 = tm.Col9;
                                model[0].Col10 = tm.Col10;
                                model[0].Col11 = tm.Col11;
                                model[0].Col12 = tm.Col12;
                                model[0].Col13 = "Update";
                                model[0].Col14 = tm.Col14;
                                model[0].Col15 = tm.Col15;
                                model[0].Col100 = "Update & New";
                                model[0].Col121 = "U";
                                model[0].Col122 = "<u>U</u>pdate";
                                model[0].Col123 = "Update & Ne<u>w</u>";
                                model[0].Col16 = dtt.Rows[0]["master_id"].ToString();
                                model[0].Col17 = dtt.Rows[0]["master_name"].ToString();
                                model[0].Col18 = dtt.Rows[0]["SUBJECTID"].ToString();
                                model[0].Col19 = dtt.Rows[0]["classid"].ToString();
                                model[0].Col20 = dtt.Rows[0]["SECTIONID"].ToString();
                                String[] L1 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["SECTIONTYPE"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems1 = L1;
                            }
                            break;

                        case "EXT":
                            mq = "select master_id,master_name,group_name,client_id,status,content,client_unit_id,vch_num,rec_id," +
                                "master_entby,master_entdate,master_editby,master_editdate,client_name from master_setting where  " +
                                 "(master_id||to_char(vch_date, 'yyyymmdd')|| type) in ('" + URL + "')";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "MSTDATA", URL.Trim());
                                Make_query(viewName.ToLower(), "Select Unit", "UNIT", "2", "");
                                ViewBag.scripCall += "callFoo('Select Unit');";
                            }
                            break;

                        case "UNIT":
                            try
                            {
                                string currdate = sgen.server_datetime(userCode);
                                ent_date = currdate;
                                string[] uts = URL.Replace("','", ",").Split(',');
                                string[] mst_data = sgen.GetSession(MyGuid, "MSTDATA").ToString().Replace("','", ",").Split(',');
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

                                        cp.Add(clientid_mst);
                                        up.Add(unitid_mst);
                                        cp.Distinct().ToList();
                                        up.Distinct().ToList();

                                        mq = "update master_setting set master_editby='" + userid_mst + "',master_editdate=TO_DATE('" + currdate + "', 'YYYY-MM-DD HH24:MI:SS')," +
                                            "client_id='" + String.Join(",", cp) + "',client_unit_id='" + String.Join(",", up) + "' " +
                                            "where (master_id||to_char(vch_date, 'yyyymmdd')|| type)='" + mst + "'";
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

                        case "PARTY":
                            mq = "select a.c_name as name,a.vch_num from clients_mst a " +
                         "where a.vch_num||To_Char(a.vch_date, 'yyyymmdd')|| a.type = '" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model[0].Col25 = dt.Rows[0]["vch_num"].ToString();
                            }
                            break;
                    }
                    break;

                    #endregion
            }
            return model;
        }
        #endregion

        //================= masters==================

        #region master
        public ActionResult master_ctrl()
        {
            string m_name = "";
            FillMst();
            chkRef();
            ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.vsave = "disabled='disabled'";
            ViewBag.vsavenew = "disabled='disabled'";
            ViewBag.scripCall = "disableForm();";

            string layout = "";
            try { layout = TempData[MyGuid + "_layout"].ToString(); }
            catch (Exception) { }
            var model = new List<Tmodelmain>();
            Tmodelmain tm1 = new Tmodelmain();

            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            m_id_mst = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);

            #region
            switch (mid_mst)
            {
                case "7004.1.7":
                    tm1.Col9 = "QUALIFICATION"; tm1.Col12 = "EQU";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Qualification"; tm1.Col17 = "Description";
                    tm1.Col143 = "qualification";
                    break;

                case "7004.1.8":
                    tm1.Col9 = "LECTURE TYPE"; tm1.Col12 = "ECL";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Lecture Type";
                    tm1.Col143 = "lectype";
                    break;

                case "7004.1.3":
                    tm1.Col9 = "LUNCH TIMING"; tm1.Col12 = "ELT";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Lunch Timing (In Min.)";
                    tm1.Col143 = "lunchtm";
                    break;

                case "7004.1.9":
                    tm1.Col9 = "HEALTH CHECKUP TYPE"; tm1.Col12 = "HCT";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Health Checkup Type"; tm1.Col17 = "Description";
                    tm1.Col143 = "chkuptype";
                    break;

                // school class section
                case "7004.1.4":
                    tm1.Col9 = "SECTION"; tm1.Col12 = "ECS";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Section Name"; tm1.Col17 = "Abbreviation";
                    tm1.Col143 = "BindSection";
                    break;


                case "7004.1.6":
                    tm1.Col9 = "SUBJECT"; tm1.Col12 = "ESW";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Subject Name"; tm1.Col17 = "Abbreviation";
                    tm1.Col143 = "lib_subject";
                    break;

                case "7004.12":
                    tm1.Col9 = "LEAVE REASON"; tm1.Col12 = "ELM";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Reason Name";
                    tm1.Col143 = "leavereas";
                    break;

                case "4007.2":
                case "7004.1.2":
                    tm1.Col9 = "CASTE CATEGORY"; tm1.Col12 = "ECT";
                    tm1.Col11 = ""; tm1.Col19 = "Caste Category";
                    tm1.Col143 = "caste";
                    break;

                // payment mode - by cash , cheque or TT or DD
                //case "4007.4":
                //    tm1.Col9 = "PAYMENT MODE"; tm1.Col12 = "EPM";
                //    tm1.Col11 = ""; tm1.Col19 = "Payment Mode";
                //    tm1.Col143 = "Modeofpayment";
                //    break;
                //  Like petrol or Diesel
                case "4007.1":
                    tm1.Col9 = "FUEL TYPE"; tm1.Col12 = "FUE";
                    tm1.Col11 = ""; tm1.Col19 = "Fuel Type";
                    tm1.Col143 = "fueltype";
                    break;
                // Hindu, Muslim, Sikha or christian
                case "4007.5":
                    tm1.Col9 = "RELIGION"; tm1.Col12 = "ERT";
                    tm1.Col11 = ""; tm1.Col19 = "Religion Type";
                    tm1.Col143 = "religion";
                    break;
                // name of affiliation type
                case "4007.7":
                    tm1.Col9 = "AFFILIATION"; tm1.Col12 = "AFM";
                    tm1.Col11 = ""; tm1.Col19 = "Affiliation Type";
                    break;
                // like MP Board or CBSC
                case "4007.8":
                    tm1.Col9 = "BOARD NAME"; tm1.Col12 = "EBD";
                    tm1.Col11 = ""; tm1.Col19 = "Board Name"; tm1.Col17 = "Description";
                    break;

                case "4007.9":
                    tm1.Col9 = "HOBBY"; tm1.Col12 = "HBY";
                    tm1.Col11 = ""; tm1.Col19 = "Hobby Name";
                    tm1.Col143 = "hobby";
                    break;

                case "4007.10":
                    tm1.Col9 = "ACCOUNT TYPE"; tm1.Col12 = "KAC";
                    tm1.Col11 = ""; tm1.Col19 = "Account Type";
                    tm1.Col143 = "acctype";
                    break;

                case "1008.9":
                    tm1.Col9 = "DEPARTMENT"; tm1.Col12 = "MDP";
                    tm1.Col11 = ""; tm1.Col19 = "Department"; tm1.Col17 = "Description";
                    tm1.Col143 = "dept";
                    break;

                case "1008.10":
                    tm1.Col9 = "DESIGNATION"; tm1.Col12 = "MDG";
                    tm1.Col11 = ""; tm1.Col19 = "Designation"; tm1.Col17 = "Description";
                    tm1.Col143 = "desig";
                    break;
                // Temporary or Permanent
                case "9003.3.4":
                    tm1.Col9 = "EMPLOYEE TYPE"; tm1.Col12 = "KET";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Employee Type";
                    tm1.Col143 = "emptype";
                    break;

                case "9003.3.5":
                    tm1.Col9 = "EMPLOYEE CATEGORY"; tm1.Col12 = "KEC";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Employee Category";
                    tm1.Col143 = "empcat";
                    break;

                case "12003.5":
                    tm1.Col9 = "LEAD SOURCE MASTER"; tm1.Col12 = "SRC";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Lead Source Type"; tm1.Col17 = "Description";
                    tm1.Col143 = "leadsource";
                    break;

                case "12003.6":
                    tm1.Col9 = "LEAD STATUS MASTER"; tm1.Col12 = "LST";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Lead Status Type"; tm1.Col17 = "Description";
                    tm1.Col143 = "leadstatus";
                    break;

                case "12003.1":
                    tm1.Col9 = "CUSTOMER TYPE"; tm1.Col12 = "CST";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Customer Type Name";
                    tm1.Col143 = "custtyp";
                    break;

                case "12003.2":
                    tm1.Col9 = "NEXT ACTION MASTER"; tm1.Col12 = "NAM";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Next Action Type"; tm1.Col17 = "Description";
                    tm1.Col143 = "nextact";
                    break;

                case "12003.3":
                    tm1.Col9 = "PRODUCT CATEGORY NAME"; tm1.Col12 = "PNM";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Product Category Name"; tm1.Col17 = "Description";
                    tm1.Col143 = "product";
                    break;
                case "28005.7":
                    tm1.Col9 = "PRODUCT SUB CATEGORY"; tm1.Col12 = "PSC";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Product Sub Category Name"; tm1.Col17 = "Description";
                    tm1.Col143 = "productsubcat";
                    break;

                case "12003.4":
                    tm1.Col9 = "DEAL PROBABILITY"; tm1.Col12 = "DPM";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Deal Probability"; tm1.Col17 = "Description";
                    tm1.Col143 = "dealprb";
                    break;

                case "9003.3.1":
                    tm1.Col9 = "GRADE"; tm1.Col12 = "KGM";
                    tm1.Col11 = "and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Grade Name";
                    tm1.Col143 = "grade";
                    break;

                case "9003.3.2":
                    tm1.Col9 = "LANGUAGE KNOWN"; tm1.Col12 = "KLM";
                    tm1.Col11 = ""; tm1.Col19 = "Language Known";
                    tm1.Col143 = "lang";
                    break;

                case "16000.1.4":
                    tm1.Col9 = "BOOK CATEGORY"; tm1.Col12 = "BCM";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Book Category"; tm1.Col17 = "Description";
                    tm1.Col143 = "book_ctgry";
                    break;

                case "16000.1.5":
                    tm1.Col9 = "BOOK AGAINST CARD"; tm1.Col12 = "BAC";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Book Against Card";
                    tm1.Col143 = "issue_aginst";
                    break;

                case "17001.15":
                    m_name = sgen.seekval(userCode, "select m_name from menus where m_id='" + mid_mst.Trim() + "'", "m_name");
                    tm1.Col9 = m_name.Trim().ToUpper(); tm1.Col12 = "HBM";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = sgen.ProperCase(m_name.Trim()); tm1.Col17 = "Description";
                    break;

                case "27008":
                case "1008.15":
                    tm1.Col9 = "ANNUAL INCOME"; tm1.Col12 = "KAI";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Annual Income"; tm1.Col17 = "Description";
                    tm1.Col143 = "annincome";
                    break;

                case "21001.7":
                case "28004.11":
                    tm1.Col9 = "PAYMENT TERM MASTER"; tm1.Col12 = "PTM";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Payment Term"; tm1.Col17 = "Description";
                    tm1.Col143 = "payterm";
                    break;

                case "9003.3.10":
                case "22007.1":
                case "20006.1":
                case "28005.6":
                    tm1.Col9 = "ADD BANK NAME"; tm1.Col12 = "ABD";
                    tm1.Col11 = "";
                    tm1.Col19 = "Bank Name"; tm1.Col17 = "Description";
                    tm1.Col143 = "bank";
                    break;

                case "40001.3":
                    tm1.Col9 = "TYPE OF ACCOUNT"; tm1.Col12 = "CLI";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Type Of Client"; tm1.Col17 = "Description";
                    tm1.Col143 = "clienttype";
                    break;

                case "40001.4":
                    tm1.Col9 = "BUSINESS TYPE"; tm1.Col12 = "CMN";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Business Type"; tm1.Col17 = "Description";
                    tm1.Col143 = "businesstype";
                    break;

                case "40001.5":
                    tm1.Col9 = "OCCUPATION"; tm1.Col12 = "OCC";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Occupation Master"; tm1.Col17 = "Description";
                    tm1.Col143 = "occtype";
                    break;

                case "28004.8":
                    tm1.Col9 = "DELIVERY TERM"; tm1.Col12 = "DEL";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Delivery term"; tm1.Col17 = "Description";
                    tm1.Col143 = "delterm";
                    break;

                case "28004.9":
                    tm1.Col9 = "PRICE TERM"; tm1.Col12 = "PRI";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Price Term"; tm1.Col17 = "Description";
                    tm1.Col143 = "";
                    break;

                case "28004.10":
                case "8001.3":
                case "5001.2":
                    if (mid_mst.Trim() == "5001.2")
                    {
                        tm1.Col9 = "MODE OF CONVEYANCE";
                        tm1.Col19 = "Mode Of Conveyance";
                        tm1.Col143 = "convmode";
                    }
                    else
                    {
                        tm1.Col9 = "MODE OF TRANSPORT";
                        tm1.Col19 = "Mode Of Transport";
                        tm1.Col143 = "transportmode";
                    }
                    tm1.Col12 = "AMT";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col17 = "Description";

                    break;

                case "21001.1":
                case "17001.1":
                case "28004.5":
                case "41005.3":
                    tm1.Col9 = "UNIT MEASUREMENT"; tm1.Col12 = "UMM";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Unit Name"; tm1.Col17 = " Description";
                    tm1.Col143 = "unitmeas";
                    break;

                case "39001.13":
                    tm1.Col9 = "REJECTION REASON"; tm1.Col12 = "REJ";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Rejection Reason"; tm1.Col17 = " Description";
                    // tm1.Col143 = "";
                    break;

                case "10001.1":
                    tm1.Col9 = "TESTING PARAMETER"; tm1.Col12 = "REJ";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Testing Parameter"; tm1.Col17 = " Description";
                    //tm1.Col143 = "";
                    break;

                case "10001.2":
                    tm1.Col9 = "TESTING METHOD NAME"; tm1.Col12 = "REJ";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Testing Method"; tm1.Col17 = " Description";
                    // tm1.Col143 = "";
                    break;

                case "39001.8":
                    tm1.Col9 = "BREAKDOWN REASON MASTER"; tm1.Col12 = "BRM";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Reason Name"; tm1.Col17 = "Description";
                    tm1.Col143 = "break_upreason";
                    break;

                case "40001.2":
                    tm1.Col9 = "RELATION MASTER"; tm1.Col12 = "REL";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Relation To Main Customer";
                    tm1.Col143 = "relation";
                    tm1.Col17 = "Description";
                    break;

                case "40001.1":
                    tm1.Col9 = "PROPERTY TYPE MASTER"; tm1.Col12 = "PRT";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Property Type"; tm1.Col17 = "Description";
                    tm1.Col143 = "protype";
                    break;
                // Box or Packets or Bundles
                case "17001.14":
                case "41005.4":
                    tm1.Col9 = "PACKING TYPE"; tm1.Col12 = "KPK";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Packing Type"; tm1.Col17 = "Description";
                    tm1.Col143 = "packtype";
                    break;

                case "22007.4":
                case "20006.4":
                case "28005.5":

                    tm1.Col99 = sgen.seekval(userCode, "select ll_act from company_unit_profile where company_profile_id='" + clientid_mst + "' and cup_id='" + unitid_mst + "'", "ll_act");//local_currency
                    sgen.SetSession(MyGuid, "LOCALCUR", tm1.Col99);

                    tm1.Col9 = "CURRENCY NAME MASTER"; tm1.Col12 = "CTM";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Currency Name"; tm1.Col17 = " Description";
                    tm1.Col143 = "curname";
                    break;

                case "22007.2":
                case "20006.2":
                case "28005.4":
                    tm1.Col9 = "BANK ACCOUNT TYPE"; tm1.Col12 = "BTM";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Account Type"; tm1.Col17 = " Description";
                    tm1.Col143 = "acctypevdm";
                    break;

                case "40001.6":
                case "12003.7":
                    tm1.Col9 = "SALES / SERVICE AREA"; tm1.Col12 = "SSA";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Area"; tm1.Col17 = " Description";
                    tm1.Col143 = "salearea";
                    break;

                case "28005.3":
                    tm1.Col9 = "ORGANISATION STATUS"; tm1.Col12 = "PT1";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Party Type"; tm1.Col17 = " Description";
                    tm1.Col143 = "partytype";
                    break;

                case "20005.8":
                    tm1.Col9 = "INCENTIVE TYPE MASTER"; tm1.Col12 = "ITM";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Incentive Type"; tm1.Col17 = " Description";
                    tm1.Col143 = "incmst";
                    break;

                case "39001.4":
                    tm1.Col9 = "SHIFT TYPE MASTER"; tm1.Col12 = "STM";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Shift Type"; tm1.Col17 = " Description";
                    tm1.Col143 = "getshft";
                    break;

                case "22007.7":
                case "20006.6":
                    tm1.Col9 = "CURRENCY RATE SOURCE"; tm1.Col12 = "CRS";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Rate Source"; tm1.Col17 = " Description";
                    tm1.Col143 = "currsrc";
                    break;

                case "41001.2":
                    tm1.Col9 = "FUNCTION TYPE"; tm1.Col12 = "FUN";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Function Type"; tm1.Col17 = " Description";
                    tm1.Col143 = "funtype";
                    break;

                case "41001.1":
                    tm1.Col9 = "EXPENSE HEAD"; tm1.Col12 = "BEH";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Expense Head"; tm1.Col17 = " Description";
                    tm1.Col143 = "exphead";
                    break;

                case "7004.1.5":
                    tm1.Col9 = "CASTE"; tm1.Col12 = "CAS";
                    tm1.Col11 = ""; tm1.Col19 = "Caste";
                    tm1.Col143 = "caste_cate";
                    break;

                case "39001.9":
                    tm1.Col9 = "STAGE MASTER"; tm1.Col12 = "KPS";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Stage Name"; tm1.Col17 = " Description";
                    tm1.Col143 = "prodstage";
                    break;

                case "39001.10":
                    tm1.Col9 = "OH TYPE MASTER"; tm1.Col12 = "OHT";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Oh Type"; tm1.Col17 = " Description";
                    tm1.Col143 = "ohtype";
                    break;

                case "17001.22":
                    tm1.Col9 = "STORAGE CONDITION MASTER"; tm1.Col12 = "KSC";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Storage Condition"; tm1.Col17 = " Description";
                    tm1.Col143 = "st_cond";
                    break;

                case "12003.8":
                    tm1.Col9 = "LEAD CLOSER TIME"; tm1.Col12 = "LCT";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Lead Closer Time"; tm1.Col17 = " Description";
                    tm1.Col143 = "leadcls";
                    break;

                case "17001.23":
                    tm1.Col9 = "SINGLE LOCATION"; tm1.Col12 = "LC6";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Location Name"; tm1.Col17 = " Description";
                    tm1.Col143 = "iloc";
                    break;

                case "190004.1":
                    tm1.Col9 = "NATURE OF WORK"; tm1.Col12 = "NOW";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Nature Of Work"; tm1.Col17 = " Description";
                    tm1.Col143 = "natwrk";
                    break;

                case "11004.1":
                    tm1.Col9 = "NATURE OF ACTIVITY"; tm1.Col12 = "NOA";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Nature Of Activity"; tm1.Col17 = " What else if not complete";
                    tm1.Col143 = "natact";
                    break;

                case "20006.7":
                    tm1.Col9 = "FORWARD CURRENCY MASTER"; tm1.Col12 = "FCM";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Fwd Currency Name"; tm1.Col17 = "Description";
                    tm1.Col143 = "fwdcurname";
                    break;

                case "40001.8":
                    tm1.Col9 = "TYPE OF ADDRESS"; tm1.Col12 = "TOA";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Type Of Address"; tm1.Col17 = "Description";
                    tm1.Col143 = "addresstype";
                    break;

                case "41001.3":
                    tm1.Col9 = "TYPE OF SERVICE"; tm1.Col12 = "TBS";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Type Of Service"; tm1.Col17 = "Description";
                    tm1.Col143 = "sertype";
                    break;

                case "41001.4":
                    tm1.Col9 = "TYPE OF MENU"; tm1.Col12 = "TBM";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Type Of Menu"; tm1.Col17 = "Description";
                    tm1.Col143 = "menustype";
                    break;

                case "2003.1":
                    tm1.Col9 = "LEGISLATION AREA"; tm1.Col12 = "LEA";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Legislation Area"; tm1.Col17 = "Description";
                    tm1.Col143 = "legislationstype";
                    break;

                case "2003.2":
                    tm1.Col9 = "COMPLIANCE TYPE"; tm1.Col12 = "COT";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Type Of Compliance"; tm1.Col17 = "Description";
                    tm1.Col143 = "compliancetype";
                    break;

                case "2003.3":
                    tm1.Col9 = "STATUTORY AUTHORITY NAME"; tm1.Col12 = "SAN";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Name of Statutory Authority"; tm1.Col17 = "Description";
                    tm1.Col143 = "statauttype";
                    break;

                case "2003.4":
                    tm1.Col9 = "FREQUENCY"; tm1.Col12 = "FRE";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Frequency"; tm1.Col17 = "Description";
                    tm1.Col143 = "freqtype";
                    break;

                case "4007.4":
                case "7002.16":
                case "28004.14":
                    tm1.Col9 = "MODE OF PAYMENT"; tm1.Col12 = "EPM";
                    tm1.Col11 = "";
                    tm1.Col19 = "Mode Of Payment"; tm1.Col17 = "Description";
                    tm1.Col143 = "Modeofpayment";
                    break;

                case "17001.26":
                    tm1.Col9 = "OTHER CHARGES MASTER"; tm1.Col12 = "MR0"; //mrn other charges
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Charge Name"; tm1.Col17 = "Description";
                    tm1.Col143 = "othchg";
                    break;

                case "40007.1":
                    tm1.Col9 = "ORDER STATUS"; tm1.Col12 = "ORS";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Order Status"; tm1.Col17 = "Description";
                    tm1.Col143 = "orderstatus";
                    break;

                case "40001.9":
                    tm1.Col9 = "FINANCIAL ACCOUNT"; tm1.Col12 = "FAN";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Financial Account"; tm1.Col17 = "Description";
                    tm1.Col143 = "finacc";
                    break;

                case "17001.10":
                    tm1.Col9 = "ATTRIBUTE MASTER"; tm1.Col12 = "ATB";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Attribute Name";
                    tm1.Col143 = "attmst";
                    break;

                case "39001.15":
                    tm1.Col9 = "PDI REJECTION REASON"; tm1.Col12 = "PDI";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "PDI Rejection Reason"; tm1.Col17 = "Description";
                    tm1.Col143 = "Rej_type";
                    break;

                case "28004.13":
                    tm1.Col9 = "INSURED BY MASTER"; tm1.Col12 = "K01";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Insured By"; tm1.Col17 = "Description";
                    tm1.Col143 = "insby";
                    break;

                case "39001.16":
                    tm1.Col9 = "MACHINE CAPACITY MASTER"; tm1.Col12 = "K02";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Capacity"; tm1.Col17 = "Description";
                    tm1.Col143 = "mcap";
                    break;

                case "40001.10":
                    tm1.Col9 = "CLIENT RATING MASTER"; tm1.Col12 = "CRM";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Client Rating"; tm1.Col17 = "Description";
                    tm1.Col143 = "clientrating";
                    break;

                case "40001.11":
                    tm1.Col9 = "SUB BROKER MASTER"; tm1.Col12 = "SBM";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Sub Broker"; tm1.Col17 = "Description";
                    tm1.Col143 = "subbroker";
                    break;

                case "40001.13":
                    tm1.Col9 = "CREDIT RATING MASTER"; tm1.Col12 = "CRD";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Credit Rating"; tm1.Col17 = "Description";
                    tm1.Col143 = "creditrating";
                    break;

                case "35001.1":
                    tm1.Col9 = "SCHEME TYPE"; tm1.Col12 = "SCH";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Scheme Type"; tm1.Col17 = "Description";
                    tm1.Col143 = "schtype";
                    break;

                case "9003.3.12":
                    tm1.Col9 = "LOCATION REGION"; tm1.Col12 = "KLR";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Region Name"; tm1.Col17 = "Description";
                    break;

                case "22001.6":
                    tm1.Col9 = "CHEQUE PRINTING LOCATION"; tm1.Col12 = "CPL";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Cheque Printing Location"; tm1.Col17 = "Description";
                    tm1.Col143 = "cheqprint";
                    break;

                case "35001.3":
                    tm1.Col9 = "SWITCH TYPE"; tm1.Col12 = "SWT";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Switch Type"; tm1.Col17 = "Description";
                    tm1.Col143 = "switchtype";
                    break;

                case "5002.2":
                    tm1.Col9 = "ASSIGNMENT NAME"; tm1.Col12 = "ASG";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Assignment Name"; tm1.Col17 = "Description";
                    tm1.Col143 = "assgname";
                    break;

                case "35001.5":
                    tm1.Col9 = "FUND NAME"; tm1.Col12 = "FUD";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Fund Name"; tm1.Col17 = "Description";
                    tm1.Col143 = "fundname";
                    break;

                case "11005.1":
                    tm1.Col9 = "TASK TYPE"; tm1.Col12 = "TSK";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Task Type"; tm1.Col17 = "Description";
                    tm1.Col143 = "tasktype";
                    break;

                case "11005.2":
                    tm1.Col9 = "CLOSER APPROVAL"; tm1.Col12 = "CLS";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Closer Approval"; tm1.Col17 = "Description";
                    tm1.Col143 = "";
                    break;

                case "35001.6":
                    tm1.Col9 = "TRANSACTION TYPE"; tm1.Col12 = "TRA";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Transaction Type"; tm1.Col17 = "Description";
                    tm1.Col143 = "transtype";
                    break;

                case "35001.7":
                    tm1.Col9 = "REDEMPTION TYPE"; tm1.Col12 = "RDP";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Redemption Type"; tm1.Col17 = "Description";
                    tm1.Col143 = "rdmptype";
                    break;

                case "35004.4":
                    tm1.Col9 = "PURCHASE THROUGH"; tm1.Col12 = "PTH";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Purchase Through"; tm1.Col17 = "Description";
                    tm1.Col143 = "purthr";
                    break;
                case "35001.8":
                    tm1.Col9 = "POLICY TYPE"; tm1.Col12 = "IPT";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Policy Type"; tm1.Col17 = "Description";
                    tm1.Col143 = "bpolicy";
                    break;
                case "35001.9":
                    tm1.Col9 = "POLICY SUB TYPE"; tm1.Col12 = "SPT";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Policy Sub Type"; tm1.Col17 = "Description";
                    tm1.Col143 = "bsubpolicy";
                    break;
                case "35001.10":
                    tm1.Col9 = "BROKER MASTER"; tm1.Col12 = "BBM";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Broker Name"; tm1.Col17 = "Description";
                    tm1.Col143 = "broker";
                    break;
                case "35001.11":
                    tm1.Col9 = "CHANNEL MASTER"; tm1.Col12 = "CHM";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Channel Name"; tm1.Col17 = "Description";
                    tm1.Col143 = "channelmaster";
                    break;
                case "11004.2":
                    tm1.Col9 = "CHECKLIST MASTER"; tm1.Col12 = "CLM";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Checklist Name"; tm1.Col17 = "Description";
                    tm1.Col143 = "checklistmaster";
                    break;
                case "5002.3":
                    tm1.Col9 = "TRAVEL TYPE MASTER"; tm1.Col12 = "TTM";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Type Name"; tm1.Col17 = "Description";
                    tm1.Col143 = "trvltypemaster";
                    break;
                case "5002.4":
                    tm1.Col9 = "CLASS MASTER"; tm1.Col12 = "TCM";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Class Name"; tm1.Col17 = "Description";
                    tm1.Col143 = "trvlclassmaster";
                    break;
                case "8001.5":
                    tm1.Col9 = "SEPARATION REASON MASTER"; tm1.Col12 = "SRM";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Reason Name"; tm1.Col17 = "Description";
                    tm1.Col143 = "reasonmaster";
                    break;

                case "1013.5":
                    tm1.Col9 = "TRAINING LEVEL"; tm1.Col12 = "TRL";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Training Level"; tm1.Col17 = "Description";
                    tm1.Col143 = "traininglevel";
                    break;

                case "400011.1":
                    tm1.Col9 = "TICKET TYPE"; tm1.Col12 = "TKT";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Ticket Type"; tm1.Col17 = "Description";
                    tm1.Col143 = "tickettype";
                    break;

                case "400011.2":
                    tm1.Col9 = "TICKET STATUS"; tm1.Col12 = "TKS";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Ticket Status"; tm1.Col17 = "Description";
                    tm1.Col143 = "ticketstatus";
                    break;

                case "400011.3":
                    tm1.Col9 = "TICKET PRIORITY"; tm1.Col12 = "TKP";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Ticket Priority"; tm1.Col17 = "Description";
                    tm1.Col143 = "ticketpriority";
                    break;

                case "400011.5":
                    tm1.Col9 = "MODE OF MEETING"; tm1.Col12 = "MOM";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Mode Of Meeting"; tm1.Col17 = "Description";
                    tm1.Col143 = "modeofmeet";
                    break;

                case "33002.5":
                    tm1.Col9 = "AREA OF IMPROVEMENT"; tm1.Col12 = "AIM";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Area Of Improvement"; tm1.Col17 = "Description";
                    tm1.Col143 = "areaimp";
                    break;

                case "33002.6":
                    tm1.Col9 = "EXPECTED COMPLETION TIME"; tm1.Col12 = "EXC";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Expected Completion Time"; tm1.Col17 = "Description";
                    tm1.Col143 = "expcomp";
                    break;

                case "33002.4":
                    tm1.Col9 = "ASSET TYPE"; tm1.Col12 = "ATY";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Asset Type Name"; tm1.Col17 = "Description";
                    tm1.Col143 = "assettype";
                    break;
                case "24001.1":
                    tm1.Col9 = "DIVISION MASTER"; tm1.Col12 = "DIV";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Division Name"; tm1.Col17 = "Description";
                    tm1.Col143 = "divisionmaster";
                    break;
                case "24001.2":
                    tm1.Col9 = "TYPE MASTER"; tm1.Col12 = "TYP";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Type Name"; tm1.Col17 = "Description";
                    tm1.Col143 = "filecatmaster";
                    break;
                case "24001.3":
                    tm1.Col9 = "FILE CATEGORY MASTER"; tm1.Col12 = "FCT";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "File Category Name"; tm1.Col17 = "Description";
                    tm1.Col143 = "filecatmaster";
                    break;
                case "24001.4":
                    tm1.Col9 = "AFFILIATION AUTHORITY MASTER"; tm1.Col12 = "AAM";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Authority Name"; tm1.Col17 = "Description";
                    tm1.Col143 = "autoritymaster";
                    break;
                case "24001.5":
                    tm1.Col9 = "AFFILIATION MASTER"; tm1.Col12 = "AFN";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Affiliation Name"; tm1.Col17 = "Description";
                    tm1.Col143 = "affliatmaster";
                    break;
                case "8003.1":
                    tm1.Col9 = "HELP TYPE MASTER"; tm1.Col12 = "HTM";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Type Name"; tm1.Col17 = "Description";
                    tm1.Col143 = "htypemaster";
                    break;

                case "33002.10":
                    tm1.Col9 = "GOAL MASTER"; tm1.Col12 = "GLM";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Goal Name"; tm1.Col17 = "Description";
                    tm1.Col143 = "goalmaster";
                    break;

                case "400011.6":
                    tm1.Col9 = "TICKET SOURCE"; tm1.Col12 = "TSC";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Ticket Source"; tm1.Col17 = "Description";
                    tm1.Col143 = "ticketsrc";
                    break;
                case "40006.2":
                    tm1.Col9 = "LEAD RATING MASTER"; tm1.Col12 = "RMA";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Lead Rating"; tm1.Col17 = "Description";
                    tm1.Col143 = "leadrating";
                    break;
                case "21001.16":
                    tm1.Col9 = "SALES CHANNEL MASTER"; tm1.Col12 = "SCM";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Channel Name"; tm1.Col17 = "Description";
                    tm1.Col143 = "schanelmst";
                    break;
                case "2003.7":
                    tm1.Col9 = "CRITICALITY MASTER"; tm1.Col12 = "CMT";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Criticality Name"; tm1.Col17 = "Description";
                    tm1.Col143 = "criticality";
                    break;
                case "2003.9":
                    tm1.Col9 = "PRIORITY MASTER"; tm1.Col12 = "PMT";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Priority Name"; tm1.Col17 = "Description";
                    tm1.Col143 = "criticality";
                    break;
                case "2003.8":
                    tm1.Col9 = "RULE NAME MASTER"; tm1.Col12 = "RNM";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Rule Name"; tm1.Col17 = "Description";
                    tm1.Col143 = "criticality";
                    break;
                case "2003.10":
                    tm1.Col9 = "NAME OF ACT"; tm1.Col12 = "NAC";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Act Name"; tm1.Col17 = "Description";
                    tm1.Col143 = "criticality";
                    break;

                case "42003.9":
                    tm1.Col9 = "SERVICE TYPE"; tm1.Col12 = "SER";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Service Name"; tm1.Col17 = "Description";
                    tm1.Col143 = "sertype";
                    break;

                case "40001.15":
                    tm1.Col9 = "VENUE MASTER"; tm1.Col12 = "VEN";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Venue Name"; tm1.Col17 = "Description";
                    tm1.Col143 = "ventype";
                    break;

                case "40001.16":
                    tm1.Col9 = "MENU MASTER"; tm1.Col12 = "MEN";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Menu Name"; tm1.Col17 = "Description";
                    tm1.Col143 = "mentype";
                    break;

                case "40001.17":
                    tm1.Col9 = "TYPE OF FUNCTION"; tm1.Col12 = "TOF";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Function Type"; tm1.Col17 = "Description";
                    tm1.Col143 = "toftype";
                    break;

                case "40001.18":
                    tm1.Col9 = "VENDOR CATEGORY"; tm1.Col12 = "VCG";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Catgory Name"; tm1.Col17 = "Description";
                    tm1.Col143 = "vcgtype";
                    break;
            }


            #endregion

            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = m_id_mst.Trim();
            tm1.Col10 = "master_setting";
            tm1.Col13 = "Save";
            tm1.Col100 = "Save & New";
            tm1.Col121 = "S";
            tm1.Col122 = "<u>S</u>ave";
            tm1.Col123 = "Save & Ne<u>w</u>";
            tm1.Col50 = layout;

            sgen.SetSession(MyGuid, "TR_MID", mid_mst.Trim());
            sgen.SetSession(MyGuid, "TYPE_MST", tm1.Col12);
            sgen.SetSession(MyGuid, "COND_MST", tm1.Col11);
            sgen.SetSession(MyGuid, "TBL_MST", tm1.Col10);

            DataTable dtb = (DataTable)sgen.GetSession(MyGuid, "URIGHTS");
            try
            {
                dtb = dtb.Select("m_id='" + tm1.Col14 + "'").CopyToDataTable();
                if (dtb.Rows.Count > 0)
                {
                    tm1.Col30 = dtb.Rows[0]["btnnew"].ToString();
                    tm1.Col31 = dtb.Rows[0]["btnedit"].ToString();
                    tm1.Col32 = dtb.Rows[0]["btnview"].ToString();
                    tm1.Col33 = dtb.Rows[0]["btnextend"].ToString();
                }
            }
            catch (Exception ex) { }

            model.Add(tm1);
            ViewBag.showextend = "N";
            try
            {
                if (sgen.Make_int(sgen.GetSession(MyGuid, "unitcnt").ToString()) > 1) ViewBag.showextend = "Y";
            }
            catch (Exception err)
            {
                mq = "select count(*) unitcnt from company_unit_profile where unit_status = '1' and company_profile_id='" + clientid_mst + "'";
                mq = sgen.seekval(userCode, mq, "unitcnt");
                sgen.SetSession(MyGuid, "unitcnt", mq);
                if (sgen.Make_int(sgen.GetSession(MyGuid, "unitcnt").ToString()) > 1) ViewBag.showextend = "Y";
            }
            if (mid_mst == "17001.10") ViewBag.showextend = "N";
            return View(model);
        }

        [HttpPost]
        public ActionResult master_ctrl(List<Tmodelmain> model, string command, string mid, string layout)
        {
            try
            {

                FillMst(model[0].Col15);
                if (sgen.Make_int(sgen.GetSession(MyGuid, "unitcnt").ToString()) > 1) ViewBag.showextend = "Y";
                model[0].Col50 = layout;
                if (command == "Save # New" || command == "Update # New") { command = command.Replace("#", "&"); }
                if (command == "New")
                {
                    if (model[0].Col30 == "N")
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for new voucher, please contact your admin', 2);";
                        return View(model);
                    }

                    #region before_save
                    if (model[0].Col14 == "17001.10")
                    {
                        DataTable dt = new DataTable();
                        dt = sgen.getdata(userCode, "select count(master_name) as tot_att from master_setting where type in ('ATB','DDATB') and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
                        if (dt.Rows.Count > 0)
                        {
                            int att_count = int.Parse(dt.Rows[0]["tot_att"].ToString());
                            if (att_count > 4)
                            {
                                ViewBag.vnew = "";
                                ViewBag.vedit = "";
                                ViewBag.vsave = "disabled='disabled'";
                                ViewBag.vsavenew = "disabled='disabled'";
                                ViewBag.scripCall = "showmsgJS(1, 'You cannot create more than 5 attributes', 2);disableForm();";
                                return View(model);
                            }
                        }
                    }
                    #endregion
                    sgen.SetSession(MyGuid, "EDMODE", "N");
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vsavenew = "";
                    ViewBag.scripCall = "enableForm();";
                    mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10.Trim() + " a where " +
                        "type in ('" + model[0].Col12 + "','DD" + model[0].Col12 + "')";
                    if (model[0].Col14.Equals("17001.6")) vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                    else vch_num = sgen.genNo(userCode, mq, 3, "auto_genid");
                    model[0].Col16 = vch_num;
                    model[0].Col21 = "Active";
                    model[0].Col13 = "Save";
                    model[0].Col100 = "Save & New";
                    model[0].Col121 = "S";
                    model[0].Col122 = "<u>S</u>ave";
                    model[0].Col123 = "Save & Ne<u>w</u>";
                }
                else if (command == "Cancel")
                {
                    TempData[MyGuid + "_layout"] = layout;
                    return CancelFun(model);
                }
                else if (command == "Callback")
                {
                    if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();

                    if (model[0].Col33 == "N" && btnval.Trim().Equals("EXT"))
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for extend voucher, please contact your admin', 2);disableForm();";
                        return View(model);
                    }
                    else if (model[0].Col31 == "N" && btnval.Trim().Equals("EDIT"))
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for edit voucher, please contact your admin', 2);disableForm();";
                        return View(model);
                    }

                    model = CallbackFun(btnval, "N", actionName, controllerName, model);
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vsavenew = "";
                    if (btnval.Trim() == "UNIT")
                    {
                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        ViewBag.vsavenew = "disabled='disabled'";
                    }
                }
                else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
                {
                    string name = "", desp = "";
                    string wrdtype = sgen.seekval(userCode, "select param1 from controls where type='CTL' and m_module3='-' and id='000001'", "param1");
                    if (wrdtype == "0")
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'Please check word setting.', 2);";
                        return View(model);
                    }
                    string deflt = "N";
                    if (model[0].Col30 == "N" && command == "Save & New")
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'You dont have permission for save voucher, please contact your admin', 2);";
                        return View(model);
                    }

                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime(userCode);
                    ent_date = currdate;
                    type = model[0].Col12;

                    if (model[0].Col21.Trim() == "Inactive")
                    {
                        type = "DD" + type;
                        status = "N";
                    }
                    else status = "Y";

                    DataTable dtstr = new DataTable();
                    dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());

                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        mq1 = " and master_id<>'" + model[0].Col16.Trim() + "'";
                        isedit = true;
                        vch_num = model[0].Col16.Trim();
                    }
                    else
                    {
                        mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10.Trim() + " a where type in ('" + model[0].Col12 + "','DD" + model[0].Col12 + "')";
                        if (model[0].Col14.Equals("17001.6")) vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                        else vch_num = sgen.genNo(userCode, mq, 3, "auto_genid");
                        isedit = false;
                    }

                    if (model[0].Col20 == null || model[0].Col20.Trim().Equals(""))
                    {
                        ViewBag.scripCall = "showmsgJS(1, 'Please Fill Atleast One Value', 2);";
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                        return View(model);
                    }
                    DataTable dtch = sgen.getdata(userCode, "select client_id,client_unit_id,master_name from " + model[0].Col10.Trim() + " a where upper(master_name)='" + model[0].Col20.Trim().ToString().ToUpper() + "' " +
                        "and type in ('" + model[0].Col12 + "','DD" + model[0].Col12 + "') and client_id='" + clientid_mst + "'" + mq1 + "");
                    if (dtch.Rows.Count > 0)
                    {
                        ViewBag.scripCall = "showmsgJS(1, 'Name Already Exists in clientid- " + dtch.Rows[0]["client_id"].ToString() + " and unitid- " + dtch.Rows[0]["client_unit_id"].ToString() + "', 2);";
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                        return View(model);
                    }

                    if (model[0].Chk1 == true) deflt = "Y";

                    if (deflt.Trim().Equals("Y"))
                    {
                        DataTable dtk = sgen.getdata(userCode, "select master_name name,sectionid deflt from master_setting where type in ('" + model[0].Col12 + "','DD" + model[0].Col12 + "') " +
                            "and find_in_set(client_id,'" + clientid_mst + "')=1 and find_in_set(client_unit_id,'" + unitid_mst + "')=1 " + mq1 + " and sectionid='Y'");
                        if (dtk.Rows.Count > 0)
                        {
                            ViewBag.scripCall = "showmsgJS(1, 'You cannot create two default record in same unit,please remove <u><b>" + dtk.Rows[0]["name"].ToString() + "</b></u> as default.', 2);";
                            ViewBag.vnew = "disabled='disabled'";
                            ViewBag.vedit = "disabled='disabled'";
                            ViewBag.vsave = "";
                            ViewBag.vsavenew = "";
                            model[0].Chk1 = false;
                            return View(model);
                        }
                    }

                    #region dtstr module

                    DataRow dr = dtstr.NewRow();
                    dr["master_id"] = vch_num.Trim();
                    dr["vch_date"] = ent_date;
                    dr["type"] = type.Trim();
                    if (m_id_mst.Trim().Equals("17001.23")) dr["master_type"] = "1";

                    switch (wrdtype)
                    {
                        case "1":
                            name = sgen.UpperCase(model[0].Col20);
                            desp = sgen.UpperCase(model[0].Col18);
                            break;
                        case "2":
                            name = sgen.LowerCase(model[0].Col20);
                            desp = sgen.LowerCase(model[0].Col18);
                            break;
                        case "3":
                            name = sgen.WordCase(model[0].Col20);
                            desp = sgen.WordCase(model[0].Col18);
                            break;
                        case "4":
                            name = model[0].Col20;
                            desp = model[0].Col18;
                            break;
                        case "5":
                            name = sgen.ProperCase(model[0].Col20);
                            desp = sgen.ProperCase(model[0].Col18);
                            break;
                    }

                    dr["master_name"] = name;

                    if (model[0].Col14.Trim().Equals("7004.1.4") || model[0].Col14.Trim().Equals("7004.1.6") || model[0].Col14.Trim().Equals("21001.1") || model[0].Col14.Trim().Equals("1008.9") ||
                        model[0].Col14.Trim().Equals("17001.1") || model[0].Col14.Trim().Equals("28004.5") || model[0].Col14.Trim().Equals("27008") || model[0].Col14.Trim().Equals("1008.10") ||
                        model[0].Col14.Trim().Equals("1008.15") || model[0].Col14.Trim().Equals("4007.8") || model[0].Col14.Trim().Equals("39001.8") || model[0].Col14.Trim().Equals("35004.4") ||
                        model[0].Col14.Trim().Equals("9003.3.10") || model[0].Col14.Trim().Equals("21001.7") || model[0].Col14.Trim().Equals("28004.11") ||
                        model[0].Col14.Trim().Equals("22007.4") || model[0].Col14.Trim().Equals("20006.4") || model[0].Col14.Trim().Equals("20006.1") ||
                        model[0].Col14.Trim().Equals("20006.2") || model[0].Col14.Equals("22007.1") || model[0].Col14.Equals("22007.2") || model[0].Col14.Equals("41005.4") ||
                        model[0].Col14.Equals("26001.1") || model[0].Col14.Equals("17001.15") || model[0].Col14.Equals("12003.7") || model[0].Col14.Equals("17001.14") ||
                        model[0].Col14.Equals("40001.6") || model[0].Col14.Equals("28005.5") || model[0].Col14.Equals("28005.4") || model[0].Col14.Equals("40001.1") ||
                        model[0].Col14.Equals("28005.6") || model[0].Col14.Equals("29001.3.1") || model[0].Col14.Equals("41005.9.1") ||
                        model[0].Col14.Equals("39001.9") || model[0].Col14.Equals("39001.10") || model[0].Col14.Equals("17001.22") || model[0].Col14.Equals("35001.1") ||
                        model[0].Col14.Equals("17001.23") || model[0].Col14.Equals("16000.1.4") || model[0].Col14.Equals("12003.8") || model[0].Col14.Equals("11004.1") ||
                        model[0].Col14.Equals("20006.7") || model[0].Col14.Equals("28004.5") || model[0].Col14.Equals("28007") || model[0].Col14.Equals("2003.1") ||
                        model[0].Col14.Equals("2003.2") || model[0].Col14.Equals("2003.3") || model[0].Col14.Equals("2003.4") || model[0].Col14.Equals("41001.2") ||
                        model[0].Col14.Equals("12003.6") || model[0].Col14.Equals("12003.2") || model[0].Col14.Equals("12003.5") || model[0].Col14.Equals("12003.3") ||
                        model[0].Col14.Equals("12003.4") || model[0].Col14.Equals("39001.13") || model[0].Col14.Equals("28005.3") || model[0].Col14.Equals("7004.1.9") ||
                        model[0].Col14.Equals("41001.1") || model[0].Col14.Equals("20006.6") || model[0].Col14.Equals("20005.8") || model[0].Col14.Equals("7004.1.7") ||
                        model[0].Col14.Equals("39001.4") || model[0].Col14.Equals("28004.9") || model[0].Col14.Equals("28004.10") || model[0].Col14.Equals("7002.16") ||
                        model[0].Col14.Equals("28004.8") || model[0].Col14.Equals("10001.1") || model[0].Col14.Equals("9003.3.10") || model[0].Col14.Equals("17001.26") ||
                        model[0].Col14.Equals("40001.3") || model[0].Col14.Equals("40001.4") || model[0].Col14.Equals("40001.5") || model[0].Col14.Equals("40001.11") ||
                        model[0].Col14.Equals("10001.2") || model[0].Col14.Equals("41001.3") || model[0].Col14.Equals("41001.4") || model[0].Col14.Equals("40001.13") ||
                        model[0].Col14.Equals("190004.1") || model[0].Col14.Equals("40001.8") || model[0].Col14.Equals("41005.3") || model[0].Col14.Equals("39001.15")
                        || model[0].Col14.Equals("39001.16") || model[0].Col14.Equals("28004.13") || model[0].Col14.Equals("9003.3.12") || model[0].Col14.Equals("35001.5") ||
                        model[0].Col14.Equals("35001.8") || model[0].Col14.Equals("35001.9") || model[0].Col14.Equals("35001.10") || model[0].Col14.Equals("35001.11") || 
                        model[0].Col14.Equals("5002.4") || model[0].Col14.Equals("5002.3") || model[0].Col14.Equals("8001.5") || model[0].Col14.Equals("28005.7")|| 
                        model[0].Col14.Equals("33002.4")|| model[0].Col14.Equals("24001.1")|| model[0].Col14.Equals("24001.2")|| model[0].Col14.Equals("24001.3")|| model[0].Col14.Equals("24001.4")|| 
                        model[0].Col14.Equals("24001.5")|| model[0].Col14.Equals("8003.1")|| model[0].Col14.Equals("21001.16")|| model[0].Col14.Equals("2003.7")|| model[0].Col14.Equals("2003.8")|| model[0].Col14.Equals("2003.9")|| model[0].Col14.Equals("2003.10"))
                    {
                        //dr["client_name"] = desp;
                    }
                    dr["client_name"] = desp;
                    if (model[0].Col14.Equals("17001.26"))
                    {
                        dr["group_refrence_number"] = model[0].Col25;
                    }
                    dr["classid"] = model[0].Col22;
                    dr["Status"] = status;
                    dr["Inactive_date"] = ent_date;
                    dr["master_type"] = "0";
                    dr["sectionid"] = deflt;

                    if (isedit)
                    {
                        dr["master_entby"] = model[0].Col3;
                        dr["master_entdate"] = model[0].Col4;
                        dr["client_id"] = model[0].Col1;
                        dr["client_unit_id"] = model[0].Col2;
                        dr["rec_id"] = model[0].Col7;
                        dr["master_editby"] = userid_mst;
                        dr["master_editdate"] = ent_date;
                    }
                    else
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

                    res = sgen.Update_data_fast1(userCode, dtstr, model[0].Col10.Trim(), model[0].Col8, isedit);
                    if (res)
                    {
                        //if ((model[0].Col143 != "") && (model[0].Col143 != null))
                        //{
                        //    sgen.SetSession(MyGuid, model[0].Col143, null);
                        //}

                        List<string> apps = new List<string>();
                        foreach (var key in HttpContext.Application.Keys)
                        {
                            if (key.ToString().ToUpper().Contains("_U_" + userCode + ""))
                            {
                                apps.Add(key.ToString());
                            }
                        }
                        foreach (var ap in apps)
                        {
                            HttpContext.Application[ap.ToString()] = null;
                        }

                        model = new List<Tmodelmain>();
                        tmodel.Col16 = "";
                        tmodel.Col18 = "";
                        tmodel.Col20 = "";
                        tmodel.Col21 = "";
                        tmodel.Col22 = "";
                        tmodel.Col13 = "Save";
                        tmodel.Col100 = "Save & New";
                        tmodel.Col121 = "S";
                        tmodel.Col122 = "<u>S</u>ave";
                        tmodel.Col123 = "Save & Ne<u>w</u>";
                        tmodel.Chk1 = false;
                        model.Add(tmodel);

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
                                #region before_save
                                if (model[0].Col14 == "17001.10")
                                {
                                    DataTable dt = new DataTable();
                                    dt = sgen.getdata(userCode, "select count(master_name) as tot_att from master_setting where type in ('ATB','DDATB') and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
                                    if (dt.Rows.Count > 0)
                                    {
                                        int att_count = int.Parse(dt.Rows[0]["tot_att"].ToString());
                                        if (att_count > 4)
                                        {
                                            ViewBag.vnew = "";
                                            ViewBag.vedit = "";
                                            ViewBag.vsave = "disabled='disabled'";
                                            ViewBag.vsavenew = "disabled='disabled'";
                                            ViewBag.scripCall = "showmsgJS(1, 'You cannot create more than 5 attributes', 2);disableForm();";
                                            return View(model);
                                        }
                                    }
                                }
                                #endregion
                                sgen.SetSession(MyGuid, "EDMODE", "N");
                                ViewBag.vnew = "disabled='disabled'";
                                ViewBag.vedit = "disabled='disabled'";
                                ViewBag.vsave = "";
                                ViewBag.vsavenew = "";
                                ViewBag.scripCall = "enableForm();";
                                mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10.Trim() + " a where type in ('" + model[0].Col12 + "','DD" + model[0].Col12 + "')";
                                if (model[0].Col14.Equals("17001.6")) vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                                else vch_num = sgen.genNo(userCode, mq, 3, "auto_genid");
                                model[0].Col16 = vch_num;
                                model[0].Col21 = "Active";
                                ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                            }
                            catch (Exception ex) { }
                        }
                    }
                    else { ViewBag.scripCall = "showmsgJS(1, '" + mq1 + "', 0);"; }
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

        #region alert_config
        public ActionResult alert_config()
        {
            FillMst();
            chkRef();
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            m_id_mst = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            List<Tmodelmain> model = new List<Tmodelmain>();
            Tmodelmain tm = new Tmodelmain();
            DataTable data = new DataTable();

            mq = "select client_unit_id||type||m_module3 as fstr, id,name,param1,param2,param3,param4 from controls where type='SMS' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "' and m_module3='edumain'";
            data = sgen.getdata(userCode, mq);
            foreach (DataRow dr in data.Rows)
            {
                tm = new Tmodelmain();
                tm.Col2 = dr["name"].ToString();
                tm.Col1 = dr["id"].ToString();

                tm.Chk1 = Convert.ToBoolean(dr["param1"].ToString() == "1" ? true : false);
                tm.Col6 = dr["param1"].ToString();

                tm.Chk2 = Convert.ToBoolean(dr["param2"].ToString() == "1" ? true : false);
                tm.Col7 = dr["param2"].ToString();

                tm.Chk3 = Convert.ToBoolean(dr["param3"].ToString() == "1" ? true : false);
                tm.Col8 = dr["param3"].ToString();

                tm.Chk4 = Convert.ToBoolean(dr["param4"].ToString() == "1" ? true : false);
                tm.Col9 = dr["param4"].ToString();
                tm.Col15 = m_id_mst;
                tm.Col8 = "client_unit_id || type || m_module3 ='" + dr["fstr"].ToString() + "'";


                model.Add(tm);
            }

            ModelState.Clear();
            return View(model);

        }

        [HttpPost]
        public ActionResult alert_config(List<Tmodelmain> model, string command)
        {
            FillMst(model[0].Col15);

            if (command == "Save" || command == "Update")
            {
                try
                {
                    DataTable dtstr = new DataTable();
                    //dtstr = sgen.getdata(userCode, "select * from controls WHERE 1=2");
                    dtstr = cmd_Fun.GetStructure(userCode, "controls");
                    isedit = true;

                    #region dtstr module

                    for (int k = 0; k < model.Count; k++)
                    {
                        DataRow dr = dtstr.NewRow();
                        dr["id"] = model[k].Col1;
                        dr["name"] = model[k].Col2;
                        dr["type"] = "SMS";
                        dr["m_module3"] = "edumain";
                        if (model[k].Col6 == "2") dr["param1"] = "2";
                        else dr["param1"] = model[k].Chk1 == true ? "1" : "0";

                        if (model[k].Col7 == "2") dr["param2"] = "2";
                        else dr["param2"] = model[k].Chk2 == true ? "1" : "0";

                        if (model[k].Col8 == "2") dr["param3"] = "2";
                        else dr["param3"] = model[k].Chk3 == true ? "1" : "0";

                        if (model[k].Col9 == "2") dr["param4"] = "2";
                        else dr["param4"] = model[k].Chk4 == true ? "1" : "0";

                        dr["rec_id"] = "0";
                        dr["client_id"] = clientid_mst;
                        dr["client_unit_id"] = unitid_mst;
                        dtstr.Rows.Add(dr);
                    }
                    #endregion

                    //bool Result = sgen.Update_data(userCode, dtstr, "controls", "type='SMS' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'", isedit);
                    bool Result = sgen.Update_data_fast1(userCode, dtstr, "controls", model[0].Col8, isedit);
                    if (Result == true)
                    {
                        ViewBag.scripCall = "showmsgJS(1, 'Data Saved Successfully', 1);";

                    }

                    else { ViewBag.scripCall += "showmsgJS(1, 'Something Went Wrong, Data Not Saved', 0);"; }
                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
            }
            ModelState.Clear();
            return View(model);
        }
        #endregion

        #region smsapi
        public ActionResult smsapi()
        {
            FillMst();
            chkRef();
            ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.vsave = "";
            ViewBag.scripCall = "enableForm();";


            Tmodelmain tm = new Tmodelmain();
            List<Tmodelmain> model = new List<Tmodelmain>();
            ent_date = sgen.server_datetime(userCode);
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            m_id_mst = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);

            mq = "select a.rec_id,a.client_name,a.master_id,a.master_name,a.master_entby as ent_by,a.master_entdate as ent_date,a.vch_date,a.status,a.master_editby edit_by,a.master_editdate as edit_date," +
                "a.client_id,a.client_unit_id,a.cont_person_name,a.cont_person_email,a.group_refrence_number,a.group_id,to_char(convert_tz(Inactive_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') " +
                "from master_setting a where type='API' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            DataTable dtt = sgen.getdata(userCode, mq);
            sgen.SetSession(MyGuid, "EDMODE", "Y");
            if (dtt.Rows.Count == 0)
            {

                mq = "select max(to_number(master_id)) as auto_genid from master_setting a where type='API'";
                vch_num = sgen.genNo(userCode, mq, 3, "auto_genid");

                mq = "select 0 rec_id,'-' cont_person_name,'" + vch_num + "' master_id,'' master_name,'" + userid_mst + "'ent_by,'" + ent_date + "' as ent_date,'" + ent_date + "' vch_date,'-' status," +
                               "'" + userid_mst + "' edit_by,'" + ent_date + "' as edit_date,'" + clientid_mst + "' client_id,'" + unitid_mst + "' client_unit_id,'-' cont_person_email,'-' group_refrence_number,'-' group_id from dual";
                dtt = sgen.getdata(userCode, mq);
                sgen.SetSession(MyGuid, "EDMODE", "N");
            }


            if (dtt.Rows.Count > 0)
            {
                DataRow dr = dtt.Rows[0];
                tm.Col1 = dr["client_id"].ToString();
                tm.Col2 = dr["client_unit_id"].ToString();
                tm.Col3 = dr["ent_by"].ToString();
                tm.Col4 = dr["ent_date"].ToString();
                tm.Col5 = dr["edit_by"].ToString();
                tm.Col6 = dr["edit_date"].ToString();
                tm.Col7 = dr["rec_id"].ToString();
                tm.Col8 = "type='API'";
                tm.Col9 = tm.Col9;
                tm.Col10 = "master_setting";
                tm.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                tm.Col12 = "API";
                tm.Col13 = "Update";
                tm.Col14 = mid_mst.Trim();
                tm.Col15 = m_id_mst.Trim();
                tm.Col16 = dr["master_id"].ToString();
                tm.Col17 = tm.Col17;
                tm.Col19 = "SMS API";
                tm.Col21 = dr["status"].ToString() == "Y" ? "Active" : "Inactive";
                tm.Col22 = dr["cont_person_name"].ToString();
                tm.Col23 = dr["cont_person_email"].ToString();
                tm.Col24 = dr["group_refrence_number"].ToString();
                tm.Col25 = dr["group_id"].ToString();
                List<SelectListItem> lt1 = new List<SelectListItem>();
                lt1.Add(new SelectListItem { Text = "Skyinfy", Value = "100" });
                TempData[MyGuid + "_TLIST1"] = lt1;
                tm.TList1 = lt1;
                tm.SelectedItems1 = new string[] { dr["master_name"].ToString() };
            }


            model.Add(tm);
            ModelState.Clear();
            return View(model);
        }

        [HttpPost]
        public ActionResult smsapi(List<Tmodelmain> model, string command)
        {
            try
            {

                FillMst(model[0].Col15);
                List<SelectListItem> lt1 = new List<SelectListItem>(); ;
                lt1 = (List<SelectListItem>)TempData[MyGuid + "_TLIST1"];
                TempData[MyGuid + "_TLIST1"] = lt1;
                model[0].TList1 = lt1;
                if (command == "Save" || command == "Update")
                {
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime(userCode);
                    if (model[0].Col21 == "Inactive")
                    {
                        type = "DD" + type;
                        status = "N";
                    }
                    else status = "Y";
                    DataTable dtstr = new DataTable();
                    //dtstr = sgen.getdata(userCode, "select * from " + model[0].Col10.Trim() + " WHERE 1=2");
                    dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        isedit = true;
                    }
                    else
                    {
                        mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11 + "";
                        vch_num = sgen.genNo(userCode, mq, 3, "auto_genid");
                        isedit = false;
                    }

                    #region dtstr module

                    DataRow dr = dtstr.NewRow();
                    dr["master_id"] = model[0].Col16;
                    dr["vch_date"] = currdate;
                    dr["type"] = model[0].Col12.Trim().Trim();
                    dr["cont_person_name"] = model[0].Col22;
                    dr["cont_person_email"] = model[0].Col23;
                    dr["group_refrence_number"] = model[0].Col24;
                    dr["group_id"] = model[0].Col25;
                    dr["master_name"] = model[0].SelectedItems1.FirstOrDefault();
                    dr["Status"] = status;
                    dr["Inactive_date"] = currdate;

                    if (isedit)
                    {
                        dr["master_entby"] = model[0].Col3;
                        dr["master_entdate"] = model[0].Col4;
                        dr["client_id"] = model[0].Col1;
                        dr["client_unit_id"] = model[0].Col2;
                        dr["rec_id"] = model[0].Col7;
                        dr["master_editby"] = userid_mst;
                        dr["master_editdate"] = currdate;
                    }
                    else
                    {
                        dr["rec_id"] = "0";
                        dr["master_entby"] = userid_mst;
                        dr["master_entdate"] = currdate;
                        dr["master_editby"] = "-";
                        dr["master_editdate"] = currdate;
                        dr["client_id"] = clientid_mst;
                        dr["client_unit_id"] = unitid_mst;
                    }
                    dtstr.Rows.Add(dr);

                    #endregion

                    bool Result = sgen.Update_data(userCode, dtstr, model[0].Col10.Trim(), model[0].Col8, isedit);
                    if (Result == true)
                    {
                        ViewBag.vsave = "";
                        ViewBag.scripCall += "showmsgJS(1, 'Data Saved Successfully', 1);";
                    }
                    else { ViewBag.scripCall += "showmsgJS(1, 'Something Went Wrong, Data Not Saved', 0);"; }
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
        public ContentResult smsbal(string id, string pass)
        {
            string baseurl = "";

            WebClient client = new WebClient();
            baseurl = "http://login.smsmantra.online/api/mt/GetBalance?User=" + Uri.EscapeUriString(id) + "&Password=" + Uri.EscapeUriString(pass) + "";

            Console.Write(baseurl);
            Stream data = client.OpenRead(baseurl);
            StreamReader reader = new StreamReader(data);
            string s = reader.ReadToEnd();
            string SMSStatus = null;
            SMSStatus = s.Trim();
            if (SMSStatus.ToString().ToLower().Contains("trans"))
            {
                SMSStatus = SMSStatus.ToString().Split(',')[3].Split('|')[1].Replace('}', ' ').Trim().Split(':')[1].Trim();
                SMSStatus = SMSStatus.Remove(SMSStatus.Length - 1);
            }
            else
                data.Close();
            reader.Close();

            return Content(SMSStatus);

        }
        #endregion

        #region tallyapi
        public ActionResult tallyapi()
        {
            FillMst();
            chkRef();
            ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.vsave = "";
            ViewBag.scripCall = "enableForm();";

            Tmodelmain tm = new Tmodelmain();
            List<Tmodelmain> model = new List<Tmodelmain>();
            ent_date = sgen.server_datetime(userCode);
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            m_id_mst = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);

            //tm.Col8 = "type='TAL'";
            tm.Col9 = tm.Col9;
            tm.Col10 = "master_setting";
            tm.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
            tm.Col12 = "TAL";
            tm.Col13 = "Update";
            tm.Col14 = mid_mst.Trim();
            tm.Col15 = m_id_mst.Trim();

            tm.Col17 = tm.Col17;
            tm.Col19 = "TALLY API";



            mq = "select a.section_strength, a.rec_id,a.client_name,a.master_id,a.master_name,a.master_entby as ent_by,a.master_entdate as ent_date,a.vch_date,a.status,a.master_editby edit_by,a.master_editdate as edit_date," +
              "a.client_id,a.client_unit_id,a.cont_person_name,a.cont_person_email,a.group_refrence_number,a.group_id,to_char(convert_tz(Inactive_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "'), " +
              "a.master_type,a.classid ,a.GROUP_REFRENCE_NUMBER,a.sectionid,a.subjectid,a.group_id from master_setting a where type='" + tm.Col12.Trim() + "' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "' order by master_id";
            DataTable dtt = sgen.getdata(userCode, mq);
            sgen.SetSession(MyGuid, "EDMODE", "Y");
            if (dtt.Rows.Count == 0)
            {
                sgen.SetSession(MyGuid, "EDMODE", "N");
                mq = "select max(to_number(master_id)) as auto_genid from master_setting a where type='" + tm.Col12.Trim() + "'";
                vch_num = sgen.genNo(userCode, mq, 3, "auto_genid");

                mq = "select 0 rec_id,'-' cont_person_name,'" + vch_num + "' master_id,'' master_name,'" + userid_mst + "'ent_by,'" + ent_date + "' as ent_date,'" + ent_date + "' vch_date,'-' status," +
                           "'" + userid_mst + "' edit_by,'" + ent_date + "' as edit_date,'" + clientid_mst + "' client_id,'" + unitid_mst + "' client_unit_id,'-' cont_person_email,'-' group_refrence_number,'-' group_id from dual";
            }
            dtt = sgen.getdata(userCode, mq);
          
            if (dtt.Rows.Count > 0)
            {
                DataRow dr = dtt.Rows[0];
                tm.Col1 = dr["client_id"].ToString();
                tm.Col2 = dr["client_unit_id"].ToString();
                tm.Col3 = dr["ent_by"].ToString();
                tm.Col4 = dr["ent_date"].ToString();
                //tm.Col5 = dr["edit_by"].ToString();
                tm.Col6 = dr["edit_date"].ToString();
                tm.Col7 = dr["rec_id"].ToString();
                tm.Col21 = dr["status"].ToString() == "Y" ? "Active" : "Inactive";

                //tm.Col8 ="Master_id||type="+ dr["master_id"].ToString()+"TAL";
                string  urlid = "TAL"+unitid_mst;
                tm.Col8 = "type||client_unit_id='"+urlid+"'";
                tm.Col16 = dtt.Rows[0]["master_id"].ToString();
                tm.Col22 = dr["master_name"].ToString();
                tm.Col23 = dr["cont_person_name"].ToString();
                tm.Col24 = dr["cont_person_email"].ToString();
                tm.Col25 = dr["group_id"].ToString();

                // i mart

                if (dtt.Rows.Count > 1)
                {
                    tm.Col26 = dtt.Rows[1]["CLIENT_NAME"].ToString();// Crm id
                    tm.Col27 = dtt.Rows[1]["classid"].ToString();// mobile no
                    tm.Col36 = dtt.Rows[1]["master_id"].ToString();// 
                    tm.Col39 = dtt.Rows[1]["rec_id"].ToString();
                    tm.Chk1 = dtt.Rows[1]["SECTION_STRENGTH"].ToString() == "Y" ? true : false;
                   

                }

                if (dtt.Rows.Count > 2)
                {
                    //just dial
                    tm.Col28 = dtt.Rows[2]["GROUP_REFRENCE_NUMBER"].ToString();// reg email
                    tm.Col29 = dtt.Rows[2]["sectionid"].ToString();// pswd
                    tm.Col30 = dtt.Rows[2]["subjectid"].ToString();// imap port no//993
                    tm.Col31 = dtt.Rows[2]["group_id"].ToString();// email from
                    tm.Col37 = dtt.Rows[2]["master_id"].ToString();// 
                    tm.Col40 = dtt.Rows[2]["rec_id"].ToString();
                    tm.Col42 = dtt.Rows[2]["client_name"].ToString();// imap  email id
                    tm.Chk2 = dtt.Rows[2]["SECTION_STRENGTH"].ToString() == "Y" ? true : false;
                    
                }

                if (dtt.Rows.Count > 3)
                {
                    //99 acres
                    tm.Col32 = dtt.Rows[3]["GROUP_REFRENCE_NUMBER"].ToString();// reg email
                    tm.Col33 = dtt.Rows[3]["sectionid"].ToString();// pswd
                    tm.Col34 = dtt.Rows[3]["subjectid"].ToString();// imap port no
                    tm.Col35 = dtt.Rows[3]["group_id"].ToString();// email from
                    tm.Col38 = dtt.Rows[3]["master_id"].ToString();// 
                    tm.Col41 = dtt.Rows[3]["rec_id"].ToString();
                    tm.Col43 = dtt.Rows[3]["client_name"].ToString();// imap  email id
                    tm.Chk3 = dtt.Rows[3]["SECTION_STRENGTH"].ToString() == "Y" ? true : false;
                    
                }

                if (dtt.Rows.Count > 4)
                {
                    //magic brick
                    tm.Col44 = dtt.Rows[4]["GROUP_REFRENCE_NUMBER"].ToString();// reg email
                    tm.Col45 = dtt.Rows[4]["sectionid"].ToString();// pswd
                    tm.Col47 = dtt.Rows[4]["subjectid"].ToString();// imap port no
                    tm.Col46 = dtt.Rows[4]["group_id"].ToString();// email from
                    tm.Col49 = dtt.Rows[4]["master_id"].ToString();// 
                    tm.Col48 = dtt.Rows[4]["client_name"].ToString();// imap  email id
                    tm.Chk4 = dtt.Rows[4]["SECTION_STRENGTH"].ToString() == "Y" ? true : false;
                    
                }

                if (dtt.Rows.Count > 5)
                {
                    //twak to
                    tm.Col50 = dtt.Rows[5]["GROUP_REFRENCE_NUMBER"].ToString();// reg email
                    tm.Col51 = dtt.Rows[5]["sectionid"].ToString();// pswd
                    tm.Col52 = dtt.Rows[5]["group_id"].ToString();// email from
                    tm.Col53 = dtt.Rows[5]["subjectid"].ToString();// imap port no                  
                    tm.Col54 = dtt.Rows[5]["client_name"].ToString();// imap  email id
                    tm.Col55 = dtt.Rows[5]["master_id"].ToString();// 
                    tm.Chk5 = dtt.Rows[5]["SECTION_STRENGTH"].ToString() == "Y" ? true : false;
                   
                }

            }


            model.Add(tm);
            ModelState.Clear();
            return View(model);
        }

        [HttpPost]
        public ActionResult tallyapi(List<Tmodelmain> model, string command)
        {
            try
            {

                FillMst();

                if (command == "Save" || command == "Update")
                {
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime(userCode);
                    if (model[0].Col21 == "Inactive")
                    {
                        type = "DD" + type;
                        status = "N";
                    }
                    else status = "Y";
                    DataTable dtstr = new DataTable();
                    dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        isedit = true;
                    }
                    else
                    {
                        mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11 + "";
                        vch_num = sgen.genNo(userCode, mq, 3, "auto_genid");
                        isedit = false;
                        model[0].Col16 = "001";
                        model[0].Col36 = "002";
                        model[0].Col37 = "003";
                        model[0].Col38 = "004";
                        model[0].Col49 = "005";
                        model[0].Col55 = "006";
                    }

                    #region dtstr module
                    bool Result = false;
                    for (int i = 0; i < 6; i++)
                    {
                        DataRow dr = dtstr.NewRow();
                        dr["master_id"] = model[0].Col16;
                        dr["vch_date"] = currdate;
                        dr["type"] = model[0].Col12.Trim().Trim();
                        dr["Status"] = status;
                        dr["Inactive_date"] = currdate;
                        dr["rec_id"] = "0";


                        if (i == 0)
                        {
                            dr["master_name"] = model[0].Col22;
                            dr["cont_person_name"] = model[0].Col23;
                            dr["cont_person_email"] = model[0].Col24;
                            dr["group_id"] = model[0].Col25;
                           

                        }


                        // i mart
                        if (i == 1)
                        {
                            dr["CLIENT_NAME"] = model[0].Col26;// Crm id
                            dr["master_name"] = "India Mart";
                            dr["classid"] = model[0].Col27;// mobile no
                            dr["master_id"] = model[0].Col36;
                            if (model[0].Chk1 == true) { dr["SECTION_STRENGTH"] = "Y"; } else { dr["SECTION_STRENGTH"] = "N"; }
                        }

                        if (i == 2)////just dial
                        {
                            dr["GROUP_REFRENCE_NUMBER"] = model[0].Col28;// reg email id
                            dr["sectionid"] = model[0].Col29;// pswd
                            dr["subjectid"] = model[0].Col30;// imap port no
                            dr["group_id"] = model[0].Col31;// email from
                            dr["master_id"] = model[0].Col37;                            
                            dr["client_name"] = model[0].Col42;
                            if (model[0].Chk2 == true) { dr["SECTION_STRENGTH"] = "Y"; } else { dr["SECTION_STRENGTH"] = "N"; }
                            dr["master_name"] = "Just Dial";
                        }

                        if (i == 3)////99 acres
                        {
                            dr["GROUP_REFRENCE_NUMBER"] = model[0].Col32;// reg email id
                            dr["sectionid"] = model[0].Col33;// pswd
                            dr["subjectid"] = model[0].Col34;// imap port no
                            dr["group_id"] = model[0].Col35;// email from
                            dr["master_id"] = model[0].Col38;
                            dr["rec_id"] = model[0].Col41;
                            dr["client_name"] = model[0].Col43;
                            if (model[0].Chk3 == true) { dr["SECTION_STRENGTH"] = "Y"; } else { dr["SECTION_STRENGTH"] = "N"; }
                            dr["master_name"] = "99 Acres";

                        }

                        if (i == 4)////magic brick
                        {
                            dr["GROUP_REFRENCE_NUMBER"] = model[0].Col44;// reg email id
                            dr["sectionid"] = model[0].Col45;// pswd
                            dr["subjectid"] = model[0].Col47;// imap port no
                            dr["group_id"] = model[0].Col46;// email from
                            dr["master_id"] = model[0].Col49;
                            dr["rec_id"] = "0";
                            dr["client_name"] = model[0].Col48;
                            if (model[0].Chk4 == true) { dr["SECTION_STRENGTH"] = "Y"; } else { dr["SECTION_STRENGTH"] = "N"; }
                            dr["master_name"] = "Magic Brick";

                        }

                        if (i == 5)////tawk to
                        {
                            dr["GROUP_REFRENCE_NUMBER"] = model[0].Col50;// reg email id
                            dr["sectionid"] = model[0].Col51;// pswd
                            dr["subjectid"] = model[0].Col53;// imap port no
                            dr["group_id"] = model[0].Col52;// email from
                            dr["master_id"] = model[0].Col55;
                            dr["rec_id"] = "0";
                            dr["client_name"] = model[0].Col54;
                            if (model[0].Chk5 == true) { dr["SECTION_STRENGTH"] = "Y"; } else { dr["SECTION_STRENGTH"] = "N"; }
                            dr["master_name"] = "Tawk To";

                        }





                        if (isedit)
                        {
                            dr["master_entby"] = model[0].Col3;
                            dr["master_entdate"] = model[0].Col4;
                            dr["client_id"] = model[0].Col1;
                            dr["client_unit_id"] = model[0].Col2;
                            dr["master_editby"] = userid_mst;
                            dr["master_editdate"] = currdate;

                        }
                        else
                        {
                            dr["master_entby"] = userid_mst;
                            dr["master_entdate"] = currdate;
                            dr["master_editby"] = "-";
                            dr["master_editdate"] = currdate;
                            dr["client_id"] = clientid_mst;
                            dr["client_unit_id"] = unitid_mst;
                        }
                        dtstr.Rows.Add(dr);

                        //string url = dr["master_id"].ToString()+"TAL";                       
                        //model[0].Col8 = "(Master_id||type) = '" + url + "'";


                    }
                    Result = sgen.Update_data_fast1(userCode, dtstr, model[0].Col10.Trim(), model[0].Col8, isedit);

                    #endregion


                    if (Result == true)
                    {
                        ViewBag.vsave = "";
                        ViewBag.scripCall += "showmsgJS(1, 'Data Saved Successfully', 1);";
                    }
                    else { ViewBag.scripCall += "showmsgJS(1, 'Something Went Wrong, Data Not Saved', 0);"; }
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

        #region doc_master
        public ActionResult doc_master()
        {
            FillMst();
            chkRef();
            //ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.vsave = "disabled='disabled'";
            //ViewBag.vsavenew = "disabled='disabled'";
            ViewBag.scripCall = "disableForm();";

            List<Tmodelmain> model = new List<Tmodelmain>();
            List<SelectListItem> grptype = new List<SelectListItem>();
            Tmodelmain tm1 = new Tmodelmain();
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            m_id_mst = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            tm1.Col19 = "Type";
            if (m_module3.ToLower().Equals("acctmain"))
            {
                tm1.Col9 = "LEDGER TYPE MASTER";
            }
            else
            {
                tm1.Col9 = "TYPE MASTER";
            }
            //if ((mid_mst.Trim().Equals("17001.7")) || (mid_mst.Trim().Equals("41005.7"))) //mrn type master
            //{
            //    tm1.Col9 = "MRN TYPE MASTER"; tm1.Col12 = "KMR";
            //    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
            //    tm1.Col19 = "Mrn Type"; tm1.Col23 = "01";
            //}

            //if (mid_mst.Trim().Equals("21001.12")) //SO type master
            //{
            //    tm1.Col9 = "SO TYPE MASTER"; tm1.Col12 = "KPO";
            //    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
            //    tm1.Col19 = "SO Type"; tm1.Col23 = "40";
            //}

            //if (mid_mst.Trim().Equals("28004.7")) //po type master
            //{
            //    tm1.Col9 = "PO TYPE MASTER"; tm1.Col12 = "KPO";
            //    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
            //    tm1.Col19 = "Po Type"; tm1.Col23 = "50";
            //}

            //if (mid_mst.Trim().Equals("17001.9")) //item issued type master
            //{
            //    tm1.Col9 = "ITEM ISSUED TYPE MASTER"; tm1.Col12 = "KIS";
            //    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
            //    tm1.Col19 = "Item Issued Type"; tm1.Col23 = "30";
            //}            

            //if (mid_mst.Trim().Equals("17001.12"))//store return type master
            //{
            //    tm1.Col9 = "STORE RETURN TYPE MASTER"; tm1.Col12 = "KSR";
            //    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
            //    tm1.Col19 = "Store Return Type"; tm1.Col23 = "1";
            //}

            //if ((mid_mst.Trim().Equals("17001.5")) || (mid_mst.Trim().Equals("41005.1"))) //item group master
            //{
            //    tm1.Col9 = "ITEM GROUP MASTER"; tm1.Col12 = "KIG";
            //    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
            //    tm1.Col19 = "Item Group";

            //    mq = "select master_name evaltype from master_setting where type='CF0' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            //    mq = sgen.seekval(userCode, mq, "evaltype");
            //    tm1.Col24 = mq;
            //    if (mq.Trim() == "0")
            //    {
            //        ViewBag.vnew = "";
            //        ViewBag.vedit = "";
            //        ViewBag.vsave = "disabled='disabled'";
            //        ViewBag.vsavenew = "disabled='disabled'";
            //        ViewBag.scripCall += "showmsgJS(1, 'Please select Inventory Evaluation Type in Inventory Config', 2);";
            //    }
            //}

            //tm1.TList1 = grptype;            
            //tm1.SelectedItems1 = new string[] { "" };            

            tm1.Col10 = "master_setting";
            tm1.Col13 = "Save";
            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = m_id_mst.Trim();
            //tm1.Col100 = "Save & New";
            //Session["M_TYPE_MST"] = tm1.Col12.Trim();
            //Session["M_COND_MST"] = tm1.Col11.Trim();

            model.Add(tm1);
            return View(model);
        }

        [HttpPost]
        public ActionResult doc_master(List<Tmodelmain> model, string command)
        {
            try
            {

                FillMst();
                DataTable dt = new DataTable();
                var tm = model.FirstOrDefault();
                #region dropdown

                //List<SelectListItem> grptype = new List<SelectListItem>();
                //model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
                //TempData[MyGuid + "_TList1"] = model[0].TList1;

                //if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };

                //if (tm.Col14.Trim().Equals("21001.12"))
                //{
                //    List<SelectListItem> sotype = new List<SelectListItem>();
                //    model[0].TList2 = (List<SelectListItem>)TempData[MyGuid + "_TList2"];
                //    TempData[MyGuid + "_TList2"] = model[0].TList2;
                //    if (tm.SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };

                //}
                #endregion
                if (command == "New")
                {
                    //try
                    //{
                    //    Session["EDMODE"] = "N";
                    //    ViewBag.vnew = "disabled='disabled'";
                    //    ViewBag.vedit = "disabled='disabled'";
                    //    ViewBag.vsave = "";
                    //    ViewBag.vsavenew = "";
                    //    ViewBag.scripCall = "enableForm();";
                    //    if ((model[0].Col14 != "17001.5") && (model[0].Col14 != "41005.1"))
                    //    {
                    //        mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11 + "";
                    //        vch_num = sgen.genNo(userCode, mq, 2, "auto_genid", Convert.ToInt32(model[0].Col23.Trim()));
                    //        model[0].Col16 = vch_num;
                    //    }
                    //    model[0].Col13 = "Save";
                    //    model[0].Col100 = "Save & New";
                    //    model[0].Col21 = "Active";

                    //    #region item group

                    //    mq = "select a.classid,(a.master_name||' ('||a.classid||')') master_name from master_setting a where a.type='KGP'";
                    //    dt = sgen.getdata(userCode, mq);
                    //    if (dt.Rows.Count > 0)
                    //    {
                    //        foreach (DataRow dr in dt.Rows)
                    //        {
                    //            grptype.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["classid"].ToString() });
                    //        }
                    //        TempData[MyGuid + "_Tlist1"] = grptype;
                    //    }

                    //    model[0].TList1 = grptype;
                    //    model[0].SelectedItems1 = new string[] { "" };
                    //    model[0].Col24 = tm.Col24;

                    //    #endregion
                    //}
                    //catch (Exception ex) { }
                }
                else if (command == "Cancel")
                {
                    return RedirectToAction("doc_master", new { @m_id = EncryptDecrypt.Encrypt(model[0].Col15), @mid = EncryptDecrypt.Encrypt(model[0].Col14) });
                }
                else if (command == "Callback")
                {
                    if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
                    model = CallbackFun(btnval, "N", actionName, controllerName, model);
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vsavenew = "";
                }
                else if (command == "btnok")
                {
                    //    return RedirectToAction("inv_conf","Inventory", new { @m_id = EncryptDecrypt.Encrypt("17001.27"), @mid = EncryptDecrypt.Encrypt("17001.27") });
                }
                else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
                {

                    string gtype = "", chktype = "";
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime(userCode);
                    ent_date = currdate;
                    type = model[0].Col12;

                    if (model[0].Col21.Trim() == "Inactive")
                    {
                        type = "DD" + type;
                        status = "N";
                    }
                    else
                    {
                        if (type.Trim().StartsWith("DD")) type = type.Substring(2, type.Length - 2);                      
                        status = "Y";

                    }

                    DataTable dtstr = new DataTable();
                    dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        mq1 = " and master_id<>'" + model[0].Col16.Trim() + "'";
                        isedit = true;
                        vch_num = model[0].Col16.Trim();
                        //if ((model[0].Col14 == "17001.5") || (model[0].Col14 == "41005.1"))
                        //{
                        //    gtype = model[0].SelectedItems1.FirstOrDefault();
                        //}                     
                    }
                    else
                    {
                        //mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11 + "";

                        //if ((model[0].Col14 == "17001.5") || (model[0].Col14 == "41005.1"))
                        //{
                        //    gtype = model[0].SelectedItems1.FirstOrDefault();
                        //    mq = "select max(to_number(substr(master_id,2,2))) as auto_genid from " + model[0].Col10.Trim() + " a where " +
                        //        "type='" + model[0].Col12.Trim() + "'" + model[0].Col11 + " and substr(master_id,1,1)='" + gtype + "'";
                        //    vch_num = sgen.genNo(userCode, mq, 2, "auto_genid");

                        //    if (sgen.Make_int(vch_num) >= 99)
                        //    {
                        //        ViewBag.vnew = "disabled='disabled'";
                        //        ViewBag.vedit = "disabled='disabled'";
                        //        ViewBag.vsave = "";
                        //        ViewBag.vsavenew = "";
                        //        ViewBag.scripCall = "showmsgJS(1, 'Max Number Allowed on this group is " + gtype + (Convert.ToInt32(vch_num) - 1).ToString() + ", 2);";
                        //        return View(model);
                        //    }
                        //    vch_num = gtype + vch_num;

                        //    if (gtype.Substring(0, 1) != vch_num.Substring(0, 1) && gtype != "99")
                        //    {
                        //        model[0].Col16 = "";
                        //        ViewBag.vnew = "disabled='disabled'";
                        //        ViewBag.vedit = "disabled='disabled'";
                        //        ViewBag.vsave = "";
                        //        ViewBag.vsavenew = "";
                        //        ViewBag.scripCall = "showmsgJS(1, 'Max Number Allowed on this group is " + (Convert.ToInt32(vch_num) - 1).ToString() + ", 2);";
                        //        return View(model);
                        //    }
                        //    else { model[0].Col16 = vch_num; }
                        //}
                        //else { vch_num = sgen.genNo(userCode, mq, 2, "auto_genid", Convert.ToInt32(model[0].Col23.Trim())); }
                        //isedit = false;
                    }

                    //if (!gtype.Trim().Equals("")) chktype = " and substr(master_id,1,1)='" + gtype + "'";
                    cond = sgen.seekval(userCode, "select master_name from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" +
                        "" + model[0].Col11 + " and upper(master_name)='" + model[0].Col20.Trim().ToString().ToUpper() + "'" + chktype + "" + mq1 + "", "master_name");
                    if (!cond.Equals("0"))
                    {
                        ViewBag.scripCall = "showmsgJS(1, 'Name Already Exists', 2);";
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                        return View(model);
                    }

                    #region dtstr module

                    DataRow dr = dtstr.NewRow();
                    dr["master_id"] = model[0].Col16.Trim();
                    dr["vch_date"] = ent_date;
                    dr["type"] = type.Trim();
                    dr["master_name"] = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(model[0].Col20.ToLower());
                    dr["classid"] = model[0].Col17;
                    dr["sectionid"] = model[0].Col18;
                    dr["client_name"] = model[0].Col22;
                    if (model[0].Col14 == "21001.12" || model[0].Col14 == "28004.7" || model[0].Col14 == "17001.7")
                    {
                        dr["group_refrence_number"] = model[0].Col25;
                    }
                    //dr["subjectid"] = gtype;
                    //dr["group_name"] = model[0].Col24;//inv eval type in case og item grp type
                    dr["Status"] = status;
                    dr["Inactive_date"] = ent_date;

                    if (isedit)
                    {
                        dr["master_entby"] = model[0].Col3;
                        dr["master_entdate"] = model[0].Col4;
                        dr["client_id"] = model[0].Col1;
                        dr["client_unit_id"] = model[0].Col2;
                        dr["rec_id"] = model[0].Col7;
                        dr["master_editby"] = userid_mst;
                        dr["master_editdate"] = ent_date;
                    }
                    else
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

                    res = sgen.Update_data_fast1(userCode, dtstr, model[0].Col10.Trim(), model[0].Col8, isedit);
                    if (res == true)
                    {
                        //model = new List<Tmodelmain>();
                        //tmodel.Col17 = "";
                        //tmodel.Col18 = "";
                        //tmodel.Col20 = "";
                        //tmodel.Col21 = "";
                        //tmodel.Col22 = "";
                        //tmodel.Col13 = "Save";
                        //tmodel.Col100 = "Save & New";
                        //tmodel.SelectedItems1 = new string[] { "" };
                        //tmodel.SelectedItems2 = new string[] { "" };
                        //tmodel.TList1 = grptype;
                        //tmodel.TList2 = sotype;
                        //model.Add(tmodel);

                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col9 = tm.Col9,
                            Col10 = tm.Col10,
                            Col19 = tm.Col19,
                            Col13 = "Save",
                            Col14 = tm.Col14,
                            Col15 = tm.Col15,
                        });
                        sgen.SetSession(MyGuid, "MTYPE", null);
                        sgen.SetSession(MyGuid, "MCOND", null);

                        //ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        //ViewBag.vsavenew = "disabled='disabled'";
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";

                        #region
                        //if (command.Trim().ToUpper().Equals("SAVE") || command.Trim().ToUpper().Equals("UPDATE"))
                        //{                            
                        //}
                        //else if (command.Trim().ToUpper().Equals("SAVE & NEW") || command.Trim().ToUpper().Equals("UPDATE & NEW"))
                        //{
                        //try
                        //{
                        //    Session["EDMODE"] = "N";
                        //    ViewBag.vnew = "disabled='disabled'";
                        //    ViewBag.vedit = "disabled='disabled'";
                        //    ViewBag.vsave = "";
                        //    ViewBag.vsavenew = "";
                        //    ViewBag.scripCall = "enableForm();";
                        //    //if ((model[0].Col14 != "17001.5") && (model[0].Col14 != "41005.1"))
                        //    //{
                        //    //    mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11 + "";
                        //    //    vch_num = sgen.genNo(userCode, mq, 2, "auto_genid", Convert.ToInt32(model[0].Col23.Trim()));
                        //    //    model[0].Col16 = vch_num;
                        //    //}
                        //    model[0].Col13 = "Save";
                        //    model[0].Col100 = "Save & New";
                        //    model[0].Col21 = "Active";

                        //    #region item group

                        //    mq = "select a.classid,(a.master_name||' ('||a.classid||')') master_name from master_setting a where a.type='KGP'";
                        //    dt = sgen.getdata(userCode, mq);
                        //    if (dt.Rows.Count > 0)
                        //    {
                        //        foreach (DataRow dr1 in dt.Rows)
                        //        {
                        //            grptype.Add(new SelectListItem { Text = dr1["master_name"].ToString(), Value = dr1["classid"].ToString() });
                        //        }
                        //        TempData[MyGuid + "_Tlist1"] = grptype;
                        //    }

                        //    model[0].TList1 = grptype;
                        //    model[0].SelectedItems1 = new string[] { "" };

                        //    #endregion
                        //}
                        //catch (Exception ex) { }
                        //}
                        #endregion
                    }
                    else { ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Something Went Wrong, Data Not Saved');"; }
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

        #region cascade_mst
        public ActionResult cascade_mst()
        {
            string spnstr = "", md_str = "", ctrl_str = "", m_name = "";
            FillMst();
            chkRef();
            ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.vsave = "disabled='disabled'";
            ViewBag.scripCall = "disableForm();";

            List<Tmodelmain> model = new List<Tmodelmain>();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            Tmodelmain tm1 = new Tmodelmain();

            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            m_id_mst = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);

            tm1.Col10 = "master_setting";
            tm1.Col13 = "Save";
            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = m_id_mst.Trim();

            DataTable dtm = new DataTable();

            if ((m_module3 == "invtmain") || (m_module3 == "bhmain") || (m_module3 == "fixmain") || (m_module3 == "lbrmain"))
            {
                dtm = sgen.getdata(userCode, "select m_name from menus where m_module3='" + m_module3 + "' and m_id in ('17001.15','17001.16','17001.17','17001.18','17001.19') order by m_id");
            }
            //else if (m_module3 == "lbrmain")
            //{
            //    dtm = sgen.getdata(userCode, "select m_name from menus where m_id in ('16000.1.1','16000.1.2','16000.1.3','16000.1.9','16000.1.10')");
            //}

            sgen.SetSession(MyGuid, "L1_MST", dtm.Rows[0]["m_name"].ToString().Trim());
            sgen.SetSession(MyGuid, "L2_MST", dtm.Rows[1]["m_name"].ToString().Trim());
            sgen.SetSession(MyGuid, "L3_MST", dtm.Rows[2]["m_name"].ToString().Trim());
            sgen.SetSession(MyGuid, "L4_MST", dtm.Rows[3]["m_name"].ToString().Trim());
            sgen.SetSession(MyGuid, "L5_MST", dtm.Rows[4]["m_name"].ToString().Trim());

            if ((mid_mst.Trim().Equals("17001.16")) || (mid_mst.Trim().Equals("16000.1.2"))) //floor master
            {
                tm1.Col9 = dtm.Rows[1]["m_name"].ToString().Trim().ToUpper();//heading
                tm1.Col12 = "IN0";//type
                tm1.Col17 = dtm.Rows[0]["m_name"].ToString().Trim();//ddl1 label (L1)
                tm1.Col29 = sgen.ProperCase(dtm.Rows[1]["m_name"].ToString().Trim());//txt label (L2)
            }
            if (mid_mst.Trim().Equals("17001.17") || (mid_mst.Trim().Equals("16000.1.3"))) //room master
            {
                tm1.Col9 = dtm.Rows[2]["m_name"].ToString().Trim().ToUpper();//heading
                tm1.Col12 = "IN1";//type
                tm1.Col17 = dtm.Rows[1]["m_name"].ToString().Trim();//ddl1 label (L2)
                                                                    //tm1.Col17 = dtm.Rows[0]["m_name"].ToString().Trim();//ddl1 label
                                                                    //tm1.Col20 = dtm.Rows[1]["m_name"].ToString().Trim();//ddl2 label
                tm1.Col29 = sgen.ProperCase(dtm.Rows[2]["m_name"].ToString().Trim());//txt label (L3)
            }
            if ((mid_mst.Trim().Equals("17001.18")) || (mid_mst.Trim().Equals("16000.1.9")))//rack master
            {
                tm1.Col9 = dtm.Rows[3]["m_name"].ToString().Trim().ToUpper();//heading
                tm1.Col12 = "IN2";//type
                tm1.Col17 = dtm.Rows[2]["m_name"].ToString().Trim();//ddl1 label (L3)
                                                                    //tm1.Col17 = dtm.Rows[0]["m_name"].ToString().Trim();//ddl1 label
                                                                    //tm1.Col20 = dtm.Rows[1]["m_name"].ToString().Trim();//ddl2 label
                                                                    //tm1.Col23 = dtm.Rows[2]["m_name"].ToString().Trim();//ddl3 label
                tm1.Col29 = sgen.ProperCase(dtm.Rows[3]["m_name"].ToString().Trim());//txt label (L4)
            }
            if ((mid_mst.Trim().Equals("17001.19")) || (mid_mst.Trim().Equals("16000.1.10")))//bin master
            {
                tm1.Col9 = dtm.Rows[4]["m_name"].ToString().Trim().ToUpper();//heading
                tm1.Col12 = "IN3";//type
                tm1.Col17 = dtm.Rows[3]["m_name"].ToString().Trim();//ddl1 label (L4)
                                                                    //tm1.Col17 = dtm.Rows[0]["m_name"].ToString().Trim();//ddl1 label
                                                                    //tm1.Col20 = dtm.Rows[1]["m_name"].ToString().Trim();//ddl2 label
                                                                    //tm1.Col23 = dtm.Rows[2]["m_name"].ToString().Trim();//ddl3 label
                                                                    //tm1.Col26 = dtm.Rows[3]["m_name"].ToString().Trim();//ddl4 label
                tm1.Col29 = sgen.ProperCase(dtm.Rows[4]["m_name"].ToString().Trim());//txt label (L5)
            }

            tm1.Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            tm1.TList1 = mod1;
            tm1.SelectedItems1 = new string[] { "" };
            //tm1.TList2 = mod1;
            //tm1.SelectedItems2 = new string[] { "" };
            //tm1.TList3 = mod1;
            //tm1.SelectedItems3 = new string[] { "" };
            //tm1.TList4 = mod1;
            //tm1.SelectedItems4 = new string[] { "" };

            tm1.Col35 = sgen.seekval(userCode, "select param1 from controls where id='000010' and type='SEP' and m_module3='invtmain'", "param1");
            sgen.SetSession(MyGuid, "CS_LOCSEP", tm1.Col35);//loc seperator
            sgen.SetSession(MyGuid, "CS_TYPEMST", tm1.Col12.Trim()); //type
            sgen.SetSession(MyGuid, "CS_CONDMST", tm1.Col11.Trim()); //cond
            sgen.SetSession(MyGuid, "CS_MIDMST", tm1.Col14.Trim()); //mid
            model.Add(tm1);
            return View(model);
        }

        [HttpPost]
        public ActionResult cascade_mst(List<Tmodelmain> model, string command)
        {
            try
            {

                string L1_type = "", L2_type = "", L3_type = "", L4_type = "", L5_type = "";
                FillMst();
                DataTable dt = new DataTable();

                #region dropdown      
                List<SelectListItem> mod1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
                model[0].TList1 = mod1;
                TempData[MyGuid + "_TList1"] = model[0].TList1;
                if (model[0].SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
                #endregion

                if (command == "New")
                {

                    sgen.SetSession(MyGuid, "EDMODE", "N");
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.scripCall = "enableForm();";

                    mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10.Trim() + " where type='" + model[0].Col12.Trim() + "'" + model[0].Col11 + "";
                    vch_num = sgen.genNo(userCode, mq, 3, "auto_genid");
                    model[0].Col16 = vch_num;
                    model[0].Col13 = "Save";
                    model[0].Col33 = "Active";
                    mod1 = new List<SelectListItem>();
                    //mod2 = new List<SelectListItem>();
                    //mod3 = new List<SelectListItem>();
                    //mod4 = new List<SelectListItem>();               

                    L1_type = "HBM";
                    L2_type = "IN0";
                    L3_type = "IN1";
                    L4_type = "IN2";
                    L5_type = "IN3";

                    if (model[0].Col14.Trim().Equals("17001.16"))//floor (L2)
                    {
                        #region building L1

                        mq = "select master_id,master_name from master_setting where type='" + L1_type + "'" + model[0].Col11 + "";
                        dt = sgen.getdata(userCode, mq);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                mod1.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                            }
                        }
                        TempData[MyGuid + "_Tlist1"] = mod1;

                        #endregion
                    }
                    else if (model[0].Col14.Trim().Equals("17001.17")) //room master (L3)
                    {
                        #region floor L2

                        mq = "select (l1.master_id||ct.param1||l2.master_id) master_id,(l1.master_name||ct.param1||l2.master_name) master_name from master_setting l2 " +
                            "inner join master_setting l1 on l1.master_id = l2.classid and l1.type = '" + L1_type + "' and l1.client_id = l2.client_id and l1.client_unit_id = l2.client_unit_id " +
                            "inner join controls ct on 1 = 1 and ct.id = '000010' and ct.type = 'SEP' and ct.m_module3 = 'invtmain' " +
                            "where l2.type = '" + L2_type + "' and l2.client_id = '" + clientid_mst + "' and l2.client_unit_id = '" + unitid_mst + "'";

                        dt = sgen.getdata(userCode, mq);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                mod1.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                            }
                        }
                        TempData[MyGuid + "_Tlist1"] = mod1;

                        #endregion
                    }
                    else if (model[0].Col14.Trim().Equals("17001.18"))//rack master (L4)
                    {
                        #region room L3

                        mq = "select(l1.master_id || ct.param1 || l2.master_id || ct.param1 || l3.master_id) master_id,(l1.master_name || ct.param1 || l2.master_name || ct.param1 || l3.master_name) master_name " +
                            "from master_setting l3 " +
                            "inner join master_setting l1 on l1.master_id = l3.classid and l1.type = '" + L1_type + "' and l1.client_id = l3.client_id and l1.client_unit_id = l3.client_unit_id " +
                            "inner join master_setting l2 on l2.master_id = l3.sectionid and l2.type = '" + L2_type + "' and l2.client_id = l3.client_id and l2.client_unit_id = l3.client_unit_id " +
                            "inner join controls ct on 1 = 1 and ct.id = '000010' and ct.type = 'SEP' and ct.m_module3 = 'invtmain' " +
                            "where l3.type = '" + L3_type + "' and l3.client_id = '" + clientid_mst + "' and l3.client_unit_id = '" + unitid_mst + "'";

                        dt = sgen.getdata(userCode, mq);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                mod1.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                            }
                        }
                        TempData[MyGuid + "_Tlist1"] = mod1;

                        #endregion
                    }
                    else if (model[0].Col14.Trim().Equals("17001.19"))//bin master (L5)
                    {
                        #region rack L4

                        mq = "select (l1.master_id||ct.param1||l2.master_id||ct.param1||l3.master_id||ct.param1||l4.master_id) master_id,(l1.master_name||ct.param1||l2.master_name||ct.param1||l3.master_name||ct.param1||l4.master_name) master_name " +
                            "from master_setting l4 " +
                            "inner join master_setting l1 on l1.master_id = l4.classid and l1.type = '" + L1_type + "' and l1.client_id = l4.client_id and l1.client_unit_id = l4.client_unit_id " +
                            "inner join master_setting l2 on l2.master_id = l4.sectionid and l2.type = '" + L2_type + "' and l2.client_id = l4.client_id and l2.client_unit_id = l4.client_unit_id " +
                            "inner join master_setting l3 on l3.master_id = l4.client_name and l3.type = '" + L3_type + "' and l3.client_id = l4.client_id and l3.client_unit_id = l4.client_unit_id " +
                            "inner join controls ct on 1 = 1 and ct.id = '000010' and ct.type = 'SEP' and ct.m_module3 = 'invtmain' " +
                            "where l4.type = '" + L4_type + "' and l4.client_id = '" + clientid_mst + "' and l4.client_unit_id = '" + unitid_mst + "'";

                        dt = sgen.getdata(userCode, mq);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                mod1.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                            }
                        }
                        TempData[MyGuid + "_Tlist1"] = mod1;

                        #endregion
                    }

                    model[0].TList1 = mod1;
                }
                else if (command == "Cancel")
                {
                    return RedirectToAction("cascade_mst", new { @m_id = EncryptDecrypt.Encrypt(model[0].Col15), @mid = EncryptDecrypt.Encrypt(model[0].Col14) });
                }
                else if (command == "Callback")
                {
                    if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
                    model = CallbackFun(btnval, "N", "cascade_mst", "Mst", model);
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                }
                else if (command == "Save" || command == "Update")
                {
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime(userCode);
                    ent_date = currdate;
                    type = model[0].Col12;

                    if (model[0].Col33.Trim() == "Inactive")
                    {
                        type = "DD" + type;
                        status = "N";
                    }
                    else status = "Y";

                    DataTable dtstr = new DataTable();
                    DataTable dtl6 = new DataTable();
                    dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    dtl6 = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        mq1 = " and master_id<>'" + model[0].Col16.Trim() + "'";
                        isedit = true;
                        vch_num = model[0].Col16.Trim();
                    }
                    else
                    {
                        mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10.Trim() + " where type='" + model[0].Col12.Trim() + "'" + model[0].Col11 + "";
                        vch_num = sgen.genNo(userCode, mq, 3, "auto_genid");
                        isedit = false;
                    }

                    string bld = "", L1 = "0", L2 = "0", L3 = "0", L4 = "0";
                    if (model[0].Col14 == "17001.16")
                    {
                        bld = " and classid='" + model[0].SelectedItems1.FirstOrDefault() + "'";
                        L1 = model[0].SelectedItems1.FirstOrDefault().Split('-')[0].Trim();
                    }
                    if (model[0].Col14 == "17001.17")
                    {
                        bld = " and classid='" + model[0].SelectedItems1.FirstOrDefault().Split('-')[0].Trim() + "' and sectionid='" + model[0].SelectedItems1.FirstOrDefault().Split('-')[1].Trim() + "'";
                        L1 = model[0].SelectedItems1.FirstOrDefault().Split('-')[0].Trim();
                        L2 = model[0].SelectedItems1.FirstOrDefault().Split('-')[1].Trim();
                    }
                    if (model[0].Col14 == "17001.18")
                    {
                        bld = " and classid='" + model[0].SelectedItems1.FirstOrDefault().Split('-')[0].Trim() + "' and sectionid='" + model[0].SelectedItems1.FirstOrDefault().Split('-')[1].Trim() + "' and client_name='" + model[0].SelectedItems1.FirstOrDefault().Split('-')[2].Trim() + "'";
                        L1 = model[0].SelectedItems1.FirstOrDefault().Split('-')[0].Trim();
                        L2 = model[0].SelectedItems1.FirstOrDefault().Split('-')[1].Trim();
                        L3 = model[0].SelectedItems1.FirstOrDefault().Split('-')[2].Trim();
                    }
                    if (model[0].Col14 == "17001.19")
                    {
                        bld = " and classid='" + model[0].SelectedItems1.FirstOrDefault().Split('-')[0].Trim() + "' and sectionid='" + model[0].SelectedItems1.FirstOrDefault().Split('-')[1].Trim() + "' and client_name='" + model[0].SelectedItems1.FirstOrDefault().Split('-')[2].Trim() + "' and subjectid='" + model[0].SelectedItems1.FirstOrDefault().Split('-')[3].Trim() + "'";
                        L1 = model[0].SelectedItems1.FirstOrDefault().Split('-')[0].Trim();
                        L2 = model[0].SelectedItems1.FirstOrDefault().Split('-')[1].Trim();
                        L3 = model[0].SelectedItems1.FirstOrDefault().Split('-')[2].Trim();
                        L4 = model[0].SelectedItems1.FirstOrDefault().Split('-')[3].Trim();
                    }

                    cond = sgen.seekval(userCode, "select master_name from " + model[0].Col10.Trim() + " where upper(master_name)='" + model[0].Col32.Trim().ToString().ToUpper() + "' and " +
                        "type='" + model[0].Col12.Trim() + "'" + model[0].Col11 + "" + mq1 + "" + bld + "", "master_name");
                    if (!cond.Equals("0"))
                    {
                        ViewBag.scripCall = "showmsgJS(1, 'Name Already Exists', 2);";
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        return View(model);
                    }

                    #region dtstr module

                    DataRow dr = dtstr.NewRow();
                    dr["master_id"] = vch_num.Trim();
                    dr["vch_date"] = ent_date;
                    dr["type"] = type.Trim();
                    dr["master_name"] = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(model[0].Col32.ToLower());//name
                    dr["classid"] = L1;//building (L1)
                    dr["sectionid"] = L2;//floor (L2)
                    dr["client_name"] = L3;//room (L3)
                    dr["subjectid"] = L4;//rack (L4)
                    dr["group_name"] = model[0].Col34;//description
                    dr["Status"] = status;
                    dr["Inactive_date"] = ent_date;

                    if (isedit)
                    {
                        dr["master_entby"] = model[0].Col3;
                        dr["master_entdate"] = model[0].Col4;
                        dr["client_id"] = model[0].Col1;
                        dr["client_unit_id"] = model[0].Col2;
                        dr["rec_id"] = model[0].Col7;
                        dr["master_editby"] = userid_mst;
                        dr["master_editdate"] = ent_date;
                    }
                    else
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

                    res = sgen.Update_data(userCode, dtstr, model[0].Col10.Trim(), model[0].Col8, isedit);
                    if (res == true)
                    {
                        #region level6
                        if (model[0].Col14.Trim().Equals("17001.19"))
                        {
                            if (!isedit)
                            {
                                //mq1 = sgen.seekval(userCode, "select param1 from controls where id='000010' and type='SEP' and m_module3='invtmain'", "param1");
                                mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10.Trim() + " where type='LC6'" + model[0].Col11 + "";
                                string l6id = sgen.genNo(userCode, mq, 3, "auto_genid");
                                DataRow drl = dtl6.NewRow();
                                drl["master_id"] = l6id.Trim();
                                drl["vch_date"] = ent_date;
                                drl["type"] = "LC6";
                                drl["master_type"] = "2";
                                drl["master_name"] = model[0].TList1.Where(w => w.Value == model[0].SelectedItems1.FirstOrDefault()).FirstOrDefault().Text + model[0].Col35 + System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(model[0].Col32.ToLower());
                                drl["client_name"] = model[0].SelectedItems1.FirstOrDefault().Split('-')[0].Trim() + model[0].SelectedItems1.FirstOrDefault().Split('-')[1].Trim() + model[0].SelectedItems1.FirstOrDefault().Split('-')[2].Trim() + model[0].SelectedItems1.FirstOrDefault().Split('-')[3].Trim() + vch_num;
                                drl["Status"] = "Y";
                                drl["Inactive_date"] = ent_date;

                                drl["rec_id"] = "0";
                                drl["master_entby"] = userid_mst;
                                drl["master_entdate"] = ent_date;
                                drl["master_editby"] = "-";
                                drl["master_editdate"] = ent_date;
                                drl["client_id"] = clientid_mst;
                                drl["client_unit_id"] = unitid_mst;
                                dtl6.Rows.Add(drl);
                                ok = sgen.Update_data(userCode, dtl6, model[0].Col10.Trim(), model[0].Col8, isedit);
                                if (!ok) ViewBag.scripCall += "mytoast('Warning', 'toast-top-right', 'Level6 Not Saved');";
                            }
                        }

                        #endregion

                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        //ViewBag.scripCall = "showmsgJS(1, 'Data Saved Successfully', 1);disableForm();";
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";

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
                            Col17 = tmodel.Col17,
                            //Col20 = tmodel.Col20,
                            //Col22 = tmodel.Col22,
                            //Col23 = tmodel.Col23,
                            //Col25 = tmodel.Col25,
                            //Col26 = tmodel.Col26,
                            //Col28 = tmodel.Col28,
                            Col29 = tmodel.Col29,
                            TList1 = mod1,
                            SelectedItems1 = new string[] { "" },
                            //TList2 = mod2,
                            //SelectedItems2 = new string[] { "" },
                            //TList3 = mod3,
                            //SelectedItems3 = new string[] { "" },
                            //TList4 = mod4,
                            //SelectedItems4 = new string[] { "" }
                        });
                    }
                    else { ViewBag.scripCall = "showmsgJS(1, 'Something Went Wrong, Data Not Saved', 0);"; }
                }
                //else if (command == "btnval1")
                //{
                //    try
                //    {
                //        string L_type = "IN0";
                //        #region Floor                   
                //        mq = "select master_id,master_name from master_setting where type='" + L_type + "' and classid='" + model[0].SelectedItems1.FirstOrDefault() + "'" + model[0].Col11 + "";
                //        dt = sgen.getdata(userCode, mq);
                //        if (dt.Rows.Count > 0)
                //        {
                //            foreach (DataRow dr in dt.Rows)
                //            {
                //                mod2.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                //            }
                //        }
                //        else mod2 = new List<SelectListItem>();
                //        TempData[MyGuid + "_Tlist2"] = mod2;
                //        #endregion

                //        model[0].TList2 = mod2;
                //        ViewBag.vnew = "disabled='disabled'";
                //        ViewBag.vedit = "disabled='disabled'";
                //        ViewBag.vsave = "";
                //    }
                //    catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1, '" + ex.Message.ToString() + "', 0);"; }
                //}
                //else if (command == "btnval2")
                //{
                //    try
                //    {
                //        string L_type = "IN1";
                //        #region Room
                //        mq = "select master_id,master_name from master_setting where type='" + L_type + "' and sectionid='" + model[0].SelectedItems2.FirstOrDefault() + "'" + model[0].Col11 + "";
                //        dt = sgen.getdata(userCode, mq);
                //        if (dt.Rows.Count > 0)
                //        {
                //            foreach (DataRow dr in dt.Rows)
                //            {
                //                mod3.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                //            }
                //        }
                //        else mod3 = new List<SelectListItem>();
                //        TempData[MyGuid + "_Tlist3"] = mod3;
                //        #endregion

                //        model[0].TList3 = mod3;
                //        ViewBag.vnew = "disabled='disabled'";
                //        ViewBag.vedit = "disabled='disabled'";
                //        ViewBag.vsave = "";
                //    }
                //    catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1, '" + ex.Message.ToString() + "', 0);"; }
                //}
                //else if (command == "btnval3")
                //{
                //    try
                //    {
                //        string L_type = "IN2";
                //        #region Rack                    
                //        mq = "select master_id,master_name from master_setting where type='" + L_type + "' and client_name='" + model[0].SelectedItems3.FirstOrDefault() + "'" + model[0].Col11 + "";
                //        dt = sgen.getdata(userCode, mq);
                //        if (dt.Rows.Count > 0)
                //        {
                //            foreach (DataRow dr in dt.Rows)
                //            {
                //                mod4.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                //            }
                //        }
                //        else mod4 = new List<SelectListItem>();
                //        TempData[MyGuid + "_Tlist4"] = mod4;
                //        #endregion                  

                //        model[0].TList4 = mod4;
                //        ViewBag.vnew = "disabled='disabled'";
                //        ViewBag.vedit = "disabled='disabled'";
                //        ViewBag.vsave = "";
                //    }
                //    catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1, '" + ex.Message.ToString() + "', 0);"; }
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

        #region multiuplaod

        public ActionResult multiupload()
        {
            FillMst();
            chkRef();
            @ViewBag.vupload = "";
            @ViewBag.vsave = "disabled='disabled'";
            ViewBag.scripCall = "disableForm();";

            List<Tmodelmain> model = new List<Tmodelmain>();
            Tmodelmain tm1 = new Tmodelmain();

            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            m_id_mst = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            tm1.Col9 = "MULTI UPLOAD";
            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = m_id_mst.Trim();
            model.Add(tm1);
            return View(model);

        }
        [HttpPost]
        public ActionResult multiupload(List<Tmodelmain> model, string command, string hftable)
        {
            try
            {

                FillMst();
                DataTable dtm = sgen.settable(hftable);
                model[0].dt1 = dtm;
                if (userCode.Equals("")) { return Redirect(sgenFun.callbackurl); }
                if (Request.UrlReferrer == null) { return Redirect(sgenFun.callbackurl); }

                DataTable dt = new DataTable();
                if (command == "Upload")
                {

                    @ViewBag.vupload = "disabled='disabled'";
                    @ViewBag.vsave = "";
                    HttpPostedFileBase file;
                    if (model[0].File1[0] != null)
                    {
                        file = model[0].File1[0];
                        dt = Read_xls(file);
                        if (dt != null && dt.Rows.Count > 0)
                        {

                            //model[0].dt1 = dt;
                            sgen.SetSession(MyGuid, "dtn", dt);
                        }
                    }
                }
                else if (command == "Save")
                {
                    tab_name = "";
                    if (model[0].Col1 != null)
                    {

                        tab_name = model[0].Col1;

                        dt = (DataTable)sgen.GetSession(MyGuid, "dtn");
                        dt = dt.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is DBNull || !string.IsNullOrWhiteSpace(field as string))).CopyToDataTable();
                        //DataTable dtf = sgen.getdata(userCode, "select * from " + tab_name + " where 1=2");
                        DataTable dtf = cmd_Fun.GetStructure(userCode, "tab_name");

                        foreach (DataRow dr in dt.Rows)
                        {
                            DataRow drf = dtf.NewRow();
                            drf["rec_id"] = "0";
                            foreach (DataColumn dc in dt.Columns) { drf[dc.ColumnName] = dr[dc.ColumnName]; }

                            dtf.Rows.Add(drf);
                        }
                        res = sgen.Update_data(userCode, dtf, tab_name, "", isedit);
                        if (res)
                        {
                            model[0].dt1 = null;
                            ViewBag.scripCall = "showmsgJS(1, 'Data Saved Sucessfully', 1);";
                        }
                        else { ViewBag.scripCall = "showmsgJS(1, 'Something Went Wrong', 0);"; }
                    }
                    else { ViewBag.scripCall = "showmsgJS(1, 'Please Fill Table Name', 2);"; }
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

        private DataTable Read_xls(HttpPostedFileBase file1)
        {
            string excelConString = "";
            DataTable dt = new DataTable();
            if (file1.ContentLength > 1)
            {
                string ext = Path.GetExtension(file1.FileName).ToLower();
                if (ext == ".xls")
                {
                    string filesavepath = AppDomain.CurrentDomain.BaseDirectory + "\\Uploads\\" + cg_com_name.Replace(" ", "") + "\\file" + DateTime.Now.ToString("ddMMyyyyhhmmfff") + ".xls";
                    file1.SaveAs(filesavepath);
                    excelConString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + filesavepath + ";Extended Properties=\"Excel 8.0;HDR=YES;\"";
                }
                else
                {
                    ViewBag.scripCall = "showmsgJS(1, 'Please Select Excel File only in xls format!!', 2);";

                    return null;
                }
                try
                {
                    OleDbConnection OleDbConn = new OleDbConnection(); OleDbConn.ConnectionString = excelConString;
                    OleDbConn.Open();
                    dt = OleDbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    OleDbConn.Close();
                    String[] excelSheets = new String[dt.Rows.Count];
                    int i = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        excelSheets[i] = row["TABLE_NAME"].ToString();
                        i++;
                    }
                    OleDbCommand OleDbCmd = new OleDbCommand();
                    String Query = "";
                    Query = "SELECT  * FROM [" + excelSheets[0] + "]";
                    OleDbCmd.CommandText = Query;
                    OleDbCmd.Connection = OleDbConn;
                    OleDbCmd.CommandTimeout = 0;
                    OleDbDataAdapter objAdapter = new OleDbDataAdapter();
                    objAdapter.SelectCommand = OleDbCmd;
                    objAdapter.SelectCommand.CommandTimeout = 0;
                    dt = null;
                    dt = new DataTable();
                    objAdapter.Fill(dt);


                }
                catch (Exception err)
                {

                    ViewBag.scripCall = "showmsgJS(1, '" + err.Message.ToString() + "', 2);";
                }


            }
            return dt;
        }
        #endregion

        #region address_master
        public ActionResult address_master()
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
            m_id_mst = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            switch (mid_mst)
            {
                case "4007.6":
                    // if (mid_mst.Trim().Equals("4007.6"))
                    //{
                    tm1.Col9 = "DISTRICT MASTER"; tm1.Col12 = "CSD";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "State";
                    tm1.Col20 = "District";
                    //  }
                    break;
                case "7004.5":
                case "9003.3.7":
                    //if ((mid_mst.Trim().Equals("7004.5")) || (mid_mst.Trim().Equals("9003.3.7")))
                    //{
                    tm1.Col9 = "TEHSIL MASTER"; tm1.Col12 = "CST";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "District";
                    tm1.Col20 = "Tehsil";
                    break;
                // }
                case "7004.9":
                case "9003.3.6":
                    //  if ((mid_mst.Trim().Equals("7004.9")) || (mid_mst.Trim().Equals("9003.3.6")))
                    //  {
                    tm1.Col9 = "VILLAGE MASTER"; tm1.Col12 = "CSV";
                    tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
                    tm1.Col19 = "Tehsil";
                    tm1.Col20 = "Village";
                    tm1.Col21 = "Pin Code ";
                    break;
                    //   }
            }
            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = m_id_mst.Trim();
            tm1.Col10 = "country_state";
            tm1.Col13 = "Save";
            tm1.Col100 = "Save & New";
            tm1.Col121 = "S";
            tm1.Col122 = "<u>S</u>ave";
            tm1.Col123 = "Save & Ne<u>w</u>";


            sgen.SetSession(MyGuid, "TR_MID", mid_mst.Trim());
            sgen.SetSession(MyGuid, "TYPE_MST", tm1.Col12.Trim());
            sgen.SetSession(MyGuid, "COND_MST", tm1.Col11.Trim());
            sgen.SetSession(MyGuid, "TBL_MST", tm1.Col10.Trim());
            model.Add(tm1);
            return View(model);

        }
        public List<Tmodelmain> new_address_master(List<Tmodelmain> model)
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
        [HttpPost]
        public ActionResult address_master(List<Tmodelmain> model, string command, string mid)
        {
            try
            {

                FillMst(model[0].Col15);
                var tm = model.FirstOrDefault();
                if (command == "New")
                {
                    model = new_address_master(model);
                }
                else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
                {

                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime(userCode);

                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        isedit = true;
                        vch_num = model[0].Col16;
                        mq1 = " and vch_num<>'" + model[0].Col16.Trim() + "'";
                    }
                    else
                    {
                        isedit = false;
                        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                        model[0].Col16 = vch_num;
                    }
                    if (model[0].Col14 == "4007.6")
                    {
                        cond = sgen.seekval(userCode, "select district_name from " + model[0].Col10 + " where type='" + model[0].Col12 + "'" +
                            " and upper (district_name ) = upper('" + model[0].Col18.Trim() + "')" + mq1 + "", "district_name");

                        if (!cond.Equals("0"))
                        {
                            {
                                ViewBag.scripCall = "showmsgJS(1, 'Data Already Exists', 2);";
                                goto End;
                            }
                        }
                    }
                    if ((model[0].Col14 == "7004.5") || (model[0].Col14 == "9003.3.7"))
                    {
                        cond = sgen.seekval(userCode, "select teh_name from " + model[0].Col10 + " where type='" + model[0].Col12 + "'" +
                            " and upper (teh_name)= upper ('" + model[0].Col18.Trim() + "') and client_id='" + clientid_mst + "' " + "" +
                            "and client_unit_id='" + unitid_mst + "' " + mq1 + "", "teh_name");
                        if (!cond.Equals("0"))
                        {
                            {
                                ViewBag.scripCall = "showmsgJS(1, 'Data Already Exists', 2);";
                                goto End;
                            }
                        }
                    }
                    if ((model[0].Col14 == "7004.9") || (model[0].Col14 == "9003.3.6"))

                    {
                        cond = sgen.seekval(userCode, "select v_name from " + model[0].Col10 + " where type='" + model[0].Col12 + "' " +
                            "and upper (v_name)= upper ('" + model[0].Col18.Trim() + "') and client_id='" + clientid_mst + "' " +
                     "and client_unit_id='" + unitid_mst + "' " + mq1 + "", "v_name");

                        if (!cond.Equals("0"))
                        {
                            {
                                ViewBag.scripCall = "showmsgJS(1, 'Data Already Exists', 2);";
                                goto End;
                            }
                        }
                    }
                    //DataTable dataTable = sgen.getdata(userCode, "select * from " + model[0].Col10.Trim() + "  where  1=2");
                    DataTable dataTable = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    DataRow dr = dataTable.NewRow();
                    DataTable dtf = (DataTable)sgen.GetSession(MyGuid, "DTF");
                    foreach (DataColumn dc in dtf.Columns)
                    {
                        if (dc.ColumnName.ToUpper().Equals("REC_ID")) { }
                        else dr[dc.ColumnName] = dtf.Rows[0][dc.ColumnName].ToString();
                    }
                    dr["rec_id"] = "0";
                    dr["type"] = model[0].Col12;
                    dr["client_id"] = clientid_mst;
                    dr["client_unit_id"] = unitid_mst;
                    dr["vch_num"] = model[0].Col16;
                    dr["vch_date"] = currdate;
                    dr["created_date"] = currdate;
                    if (model[0].Col14 == "4007.6")
                    {
                        dr["district_id"] = model[0].Col16;
                        dr["district_name"] = model[0].Col18;
                    }
                    if ((model[0].Col14 == "7004.5") || (model[0].Col14 == "9003.3.7"))
                    {
                        dr["teh_id"] = model[0].Col16;
                        dr["teh_name"] = model[0].Col18;
                    }
                    if ((model[0].Col14 == "7004.9") || (model[0].Col14 == "9003.3.6"))
                    {
                        dr["v_id"] = model[0].Col16;
                        dr["v_name"] = model[0].Col18;
                        dr["pincode"] = model[0].Col22;
                    }
                    if (isedit)
                    {
                        dr["ent_by"] = model[0].Col3;
                        dr["ent_date"] = model[0].Col4;
                        dr["edit_by"] = userid_mst;
                        dr["edit_date"] = currdate;
                        dr["created_date"] = currdate;
                        dr["client_id"] = clientid_mst;
                        dr["client_unit_id"] = unitid_mst;
                    }
                    else
                    {
                        dr["ent_by"] = userid_mst;
                        dr["ent_date"] = currdate;
                        dr["edit_by"] = "-";
                        dr["edit_date"] = currdate;

                    }
                    dataTable.Rows.Add(dr);
                    bool Result = sgen.Update_data_fast1(userCode, dataTable, model[0].Col10, tmodel.Col8, isedit);
                    if (Result == true)
                    {
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {

                            Col9 = tm.Col9,
                            Col10 = tm.Col10,
                            Col11 = tm.Col11,
                            Col12 = tm.Col12,
                            Col13 = tm.Col13,
                            Col100 = "Save & New",
                            Col121 = "S",
                            Col122 = "<u>S</u>ave",
                            Col123 = "Save & Ne<u>w</u>",
                            Col14 = tm.Col14,
                            Col15 = tm.Col15,
                            Col19 = tm.Col19,
                            Col20 = tm.Col20,
                            Col21 = tm.Col21,
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
                                model = new_address_master(model);
                                ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                            }
                            catch (Exception ex) { }
                        }

                    }
                End:;
                }

                else if (command == "Cancel")
                {
                    return CancelFun(model);
                }
                else if (command == "Callback")
                {
                    model = StartCallback(model);
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

        #region  Form Control
        public ActionResult form_ctrl()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "MASTER CONFIG";
            model[0].Col10 = "controls";
            model[0].Col12 = "VDC"; // Vendor Detail Controlls
            model[0].Col11 = "and client_id=" + clientid_mst + " and client_unit_id=" + unitid_mst + "";
            ViewBag.vsave = "disabled='disabled'";
            sgen.SetSession(MyGuid, "CF_MID", model[0].Col14);
            return View(model);
        }
        [HttpPost]
        public ActionResult form_ctrl(List<Tmodelmain> model, string command)
        {
            try
            {
                FillMst();
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
                    DataTable dtstr = new DataTable();

                    dtstr = cmd_Fun.GetStructure(userCode, "controls");
                    isedit = true;

                    #region dtstr module
                    for (int i = 0; i < model.Count; i++)
                    {
                        DataRow dr = dtstr.NewRow();
                        dr["id"] = model[i].Col19;
                        dr["name"] = model[i].Col20;
                        dr["type"] = "VDC";
                        dr["param1"] = model[i].Chk1 == true ? "Y" : "N";
                        dr["param2"] = model[i].Chk2 == true ? "Y" : "N";
                        dr["param4"] = model[i].Chk3 == true ? "Y" : "N";
                        dr["param3"] = model[i].Col18;
                        dr["param5"] = model[0].Col26;
                        dr["param6"] = model[i].Col22;   //data type
                        dr["param7"] = model[i].Col23;  //char length
                        dr["param8"] = model[0].Col24;  //
                        dr["param9"] = model[0].Col25;  //remark
                        dr["rec_id"] = model[i].Col7;
                        dr["client_id"] = clientid_mst;
                        dr["client_unit_id"] = unitid_mst;

                        dtstr.Rows.Add(dr);
                    }
                    #endregion

                    var duplicates = dtstr.AsEnumerable()
             .Select(dr => dr.Field<string>("name"))
             .GroupBy(x => x)
             .Where(g => g.Count() > 1)
             .Select(g => g.Key)
             .ToList();

                    if (duplicates.Count > 0)
                    {
                        ViewBag.scripCall += "mytoast('warning', 'toast-top-right', 'Please Enter Unique Lable Name :- " + String.Join(",", duplicates) + "');";
                        return View(model);

                    }
                    res = sgen.Update_data_fast1(userCode, dtstr, "controls", model[0].Col8, isedit);
                    // res = sgen.Update_data(userCode, dtstr, "controls", model[0].Col8, isedit);

                    if (res == true)
                    {
                        //if ((model[0].Col143 != "") && (model[0].Col143 != null))
                        //{
                        //    sgen.SetSession(MyGuid, model[0].Col143, null);
                        //}

                        List<string> apps = new List<string>();
                        foreach (var key in HttpContext.Application.Keys)
                        {
                            if (key.ToString().ToUpper().Contains("_U_" + userCode + ""))
                            {
                                apps.Add(key.ToString());
                            }
                        }
                        foreach (var ap in apps)
                        {
                            HttpContext.Application[ap.ToString()] = null;
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
                                Make_query("form_ctrl", "Select Page To Update Controls", "EDIT", "1", "", "");
                                ViewBag.scripCall = "callFoo('Select Page To Update Controls');";
                                ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                            }
                            catch (Exception ex) { }
                        }

                    }
                    else { ViewBag.scripCall += "showmsgJS(1, 'Something Went Wrong, Data Not Saved', 0);"; }
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

        #region  Print Control Config

        public ActionResult prn_ctrl()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            List<SelectListItem> mod5 = new List<SelectListItem>();
            model = getload(model);
            model[0].Col10 = "kc_tab";
            model[0].Col11 = "and client_id=" + clientid_mst + " and client_unit_id=" + unitid_mst + "";
            model[0].Col12 = "PRN"; // DOC PRINT CONFIG            
            //model[0].Col40 = "Choose File";
            //model[0].Col41 = "Choose File";
            //model[0].Col42 = "Choose File";
            model[0].Files1 = new List<Files>();
            Files tm3 = new Files();
            tm3.FileName = "-";
            model[0].Files1.Add(tm3);
            model[0].Files1.Add(tm3);
            model[0].Files1.Add(tm3);
            sgen.SetSession(MyGuid, "CF_MID", model[0].Col15);
            model[0].TList5 = mod5;
            model[0].SelectedItems5 = new string[] { "" };
            return View(model);
        }

        [HttpPost]
        public ActionResult prn_ctrl(List<Tmodelmain> model, string command, string modelstr)
        {
            try
            {

                //string ctype1 = "", ctype2 = "", ctype3 = "", ctype4 = "", finalpath1 = "", finalpath2 = "", finalpath3 = "", finalpath4 = "",
                //    fileName1 = "", fileName2 = "", fileName3 = "", fileName4 = "", encpath1 = "", encpath2 = "", encpath3 = "", encpath4 = "",
                //    path1 = "", path2 = "", path3 = "", path4 = "";
                var tm = model.FirstOrDefault();
                FillMst();
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
                List<SelectListItem> mod5 = (List<SelectListItem>)TempData[MyGuid + "_Tlist5"];
                TempData[MyGuid + "_Tlist5"] = mod5;
                model[0].TList5 = mod5;
                if (model[0].SelectedItems5 == null) model[0].SelectedItems5 = new string[] { "" };

                if (command == "Cancel")
                { return CancelFun(model); }
                else if (command == "Callback")
                {
                    if (sgen.GetSession(MyGuid, "delid") != null) { btnval = "FDEL"; }
                    else if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
                    model = CallbackFun(btnval, "N", actionName, controllerName, model);
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vsavenew = "";
                }
                else if (command == "Invoice")
                {
                    try
                    {
                        DataTable dt = new DataTable();

                        dt = sgen.getdata(userCode, "select 'name' name,'gstin' sgtin,'address' address,'rec_id' rec_id,'created_date' " +
                            "created_date,'vch_num' vch_num,'vch_date' vch_date,'type' type,'client_id' client_id,'client_unit_id'" +
                            " client_unit_id, 'ent_by' ent_by, 'ent_date' ent_date, 'edit_by' edit_by,'iname' iname, 'icode' icode, " +
                            "'acode' acode, 'aname' aname, 'chlno' chlno, 'chldate' chldate,'invno' invno, 'invdate' invdate, 'ewayno' " +
                            "ewayno, 'ewaydate' ewaydate, 'qtystk' qtystk,'qtyin' qtyin,0 qtyout, 'qtychl' qtychl, 'uom'" +
                            " uom, 'uom2' uom2, 'irate' irate, 'pono' pono, 'podate' podate, 'qtypo' qtypo, 'inspected' inspected, " +
                            "'qtybal' qtybal,'deptno' deptno, 'deptname' deptname,0 iamount, 'rgptype' rgptype, 'totremark' " +
                            "totremark, 'contactno' contactno, 'date1' date1, 'date2' date2, 'desig' desig, 'iremark' iremark, " +
                            "'pay_term' pay_term, 'price_term' price_term, 'hsno' hsno, 'igst' igst, 'cgst' cgst,'sgst' sgst," +
                            " 'shipfrom' shipfrom, 'shipto' shipto, 'cpartno' cpartno, 'ordqty' ordqty,0 as taxamt, 'taxrate' taxrate," +
                            " 'disctype' disctype, 0 as discamt, 0 as lineamount, 'cond' cond, 0 as basicamt," +
                            " 'gdisc' gdisc, 'gfreight' gfreight, 'insurance' insurance, 'ginstlchrg' ginstlchrg, 'gserchrg' gserchrg," +
                            " 'gamc' gamc, 'gloadchrg' gloadchrg, 'gothrchrg' gothrchrg,0 as gtaxamt, 0 gamt, 'potype' " +
                            "potype, 'store' store, 'tmode' tmode, 'partno' partno, 'vch_date1' vch_date1 from dual");
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                DataTable dt1 = new DataTable();

                                string rpt_name = sgen.getstring(userCode, "select client_name  from master_setting where type='IRF' and master_id='" + string.Join(",", model[0].SelectedItems5) + "'");

                                //string rpt = "../../erp/schoolReports_Rpts/" + rpt_name;
                                string rpt = "../schoolReports_Rpts/" + rpt_name;
                                //rpt = rpt_name;
                                sgen.open_report_bydt_xml(userCode, dt, rpt, "Invoice Report");
                                ViewBag.scripCall += "showRptnew('Invoice Report');";

                                //ViewBag.scripCall = "OpenSingle('../../../../../erp/schoolReports_Rpts/Filter_Report_xml.aspx','95%','900px','Reports');";
                            }
                        }

                    }
                    catch (Exception ex)
                    { sgen.showmsg(1, ex.Message.ToString(), 2); }
                }
                else if ((command == "Save") || (command == "Update"))
                {

                    DataTable dtstr = new DataTable();
                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        isedit = true;
                        vch_num = model[0].Col16;
                    }
                    else
                    {
                        isedit = false;
                        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "' " + model[0].Col11.Trim() + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                        model[0].Col16 = vch_num;
                    }
                    string currdate = sgen.server_datetime(userCode);
                    dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10);
                    #region dtstr module
                    if (model.Count > 0)
                    {
                        DataRow dr = dtstr.NewRow();
                        dr["vch_num"] = vch_num;
                        dr["vch_date"] = currdate;
                        dr["type"] = model[0].Col12;
                        dr["col73"] = model[0].Col17;//gen cond
                        dr["col74"] = model[0].Col18;//back page gen cond
                        dr["col1"] = model[0].Col19;//doc cont no.
                        dr["col2"] = model[0].Col22;//form type
                        dr["col3"] = model[0].Col23;//mst type
                        dr["col4"] = model[0].Col24;//mstmain type
                        dr["col5"] = model[0].SelectedItems5.FirstOrDefault();//rpt
                        dr["rec_id"] = "0";
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
                        dtstr.Rows.Add(dr);
                    }
                    #endregion
                    Satransaction sat = new Satransaction(userCode, MyGuid);
                    bool result = sgen.Update_data_fast1_uncommit(userCode, dtstr, model[0].Col10.Trim(), tm.Col8, isedit, sat);
                    if (result)
                    {
                        #region attachment
                        string serverpath = Server.MapPath("~/Uploads/" + cg_com_name.Replace(" ", "") + "/");
                        var dtf = sgen.getdata(userCode, "select * from file_tab where " + tm.Col8 + "");
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
                        sat.Execute_cmd("delete from file_tab where " + tm.Col8 + "");

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
                                            filesave(model, currdate, dtfile, fileName1, encpath1, "PRN", ctype1, "File Saved From Print Config","-", "", "");
                                            result = sgen.Update_data_fast1_uncommit(userCode, dtfile, "file_tab", tm.Col8, isedit, sat);
                                            if (!result)
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
                        if (result)
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
                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        ViewBag.vsavenew = "disabled='disabled'";
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col10 = tm.Col10,
                            Col11 = tm.Col11,
                            Col13 = "Save",
                            Col14 = tm.Col14,
                            Col15 = tm.Col15,
                            TList5 = mod5,
                            SelectedItems5 = new string[] { "" },
                        });

                        model[0].Files1 = new List<Files>();
                        Files tm3 = new Files();
                        tm3.FileName = "-";
                        model[0].Files1.Add(tm3);
                        model[0].Files1.Add(tm3);
                        model[0].Files1.Add(tm3);
                    }
                    else { ViewBag.scripCall += "showmsgJS(1, 'Something Went Wrong, Data Not Saved', 0);"; }
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

        #region sms_tmp
        public ActionResult sms_tmp()
        {
            FillMst();

            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);

            model[0].Col9 = "SMS TEMPLATE";
            model[0].Col10 = "enx_tab";
            model[0].Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            model[0].Col12 = "TMP";



            List<SelectListItem> mod1 = new List<SelectListItem>();

            #region temp_set_for 1
            mod1 = new List<SelectListItem>();
            DataTable dt = sgen.getdata(userCode, "select 'Account' as set_For from dual Union all select 'Prospect Data' as set_For from dual  ");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    mod1.Add(new SelectListItem { Text = dr["set_For"].ToString(), Value = dr["set_For"].ToString() });
                }
            }

            TempData[MyGuid + "_Tlist1"] = mod1;
            model[0].TList1 = mod1;
            model[0].SelectedItems1 = new string[] { "" };
            #endregion


            return View(model);
        }

        [HttpPost]
        public ActionResult sms_tmp(List<Tmodelmain> model, string command, string mid)
        {
            try
            {

                string fdt = "", tdt = "";

                FillMst();
                var tm = model.FirstOrDefault();


                List<SelectListItem> mod1 = (List<SelectListItem>)TempData[MyGuid + "_Tlist1"];
                model[0].TList1 = mod1;
                TempData[MyGuid + "_Tlist1"] = model[0].TList1;
                if (model[0].SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };

                if (command == "New")
                {
                    model = getnew(model);
                }
                else if (command == "Cancel")
                {
                    return CancelFun(model);
                }
                else if (command == "Callback")
                {
                    model = StartCallback(model);
                }

                else if (command.Trim() == "Save" || command.Trim() == "Update")
                {

                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime(userCode);
                    string cond = "", found = "";
                    DataTable dtstr = new DataTable();

                    //dtstr = sgen.getdata(userCode, "select * from " + model[0].Col10 + " WHERE 1=2");
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
                        mq1 = "";
                    }

                    DataRow dr = dtstr.NewRow();
                    dr["rec_id"] = "0";
                    dr["type"] = model[0].Col12;
                    dr["vch_num"] = vch_num.Trim();
                    dr["vch_date"] = sgen.Savedate(model[0].Col17, false);
                    dr["col1"] = model[0].Col18;      // body text
                    dr["col3"] = model[0].Col19;      // singnature
                    dr["col2"] = model[0].SelectedItems1.FirstOrDefault();      // signature
                    dr["col8"] = model[0].Col17;      // email heading
                    dr["col7"] = model[0].Col20;      // language

                    dr["col9"] = model[0].Chk1 == true ? "Y" : "N"; //Admin Mail
                    dr["col10"] = model[0].Chk2 == true ? "Y" : "N"; //Admin Phone (SMS)
                    dr["col11"] = model[0].Chk3 == true ? "Y" : "N"; // Outside Mail
                    dr["col12"] = model[0].Chk4 == true ? "Y" : "N"; // Outside Phone (SMS)
                    dr["col13"] = model[0].Chk5 == true ? "Y" : "N"; //Custom Signature
                    dr["client_id"] = clientid_mst;
                    dr["client_unit_id"] = unitid_mst;
                    if (isedit)
                    {
                        dr["ent_by"] = model[0].Col3;
                        dr["ent_date"] = model[0].Col4;
                        dr["edit_by"] = userid_mst;
                        dr["edit_date"] = currdate;
                        dr["created_date"] = currdate;

                    }
                    else
                    {
                        dr["ent_by"] = userid_mst;
                        dr["ent_date"] = currdate;
                        dr["edit_by"] = "-";
                        dr["edit_date"] = currdate;

                    }
                    //dr = getsave(currdate, dr, model, isedit);
                    dtstr.Rows.Add(dr);

                    //  bool Result = sgen.Update_data(userCode, dtstr, model[0].Col10.Trim(), tmodel.Col8, isedit);
                    bool Result = sgen.Update_data_fast1(userCode, dtstr, model[0].Col10.Trim(), tmodel.Col8, isedit);
                    if (Result == true)
                    {
                        vch_num = model[0].Col16;
                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        ViewBag.scripCall = "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col10 = tm.Col10,
                            Col11 = tm.Col11,
                            Col12 = tm.Col12,
                            Col13 = "Save",
                            Col14 = tm.Col14,
                            Col15 = tm.Col15,
                            Col9 = tm.Col9,
                            TList1 = mod1,
                            SelectedItems1 = new string[] { "" },
                        });
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

        #region file
        private void filesave(List<Tmodelmain> model, string currdate, DataTable dtfile, string filename, string filepath, string filetitle, string content_type)
        {
            FillMst();
            DataRow drf = dtfile.NewRow();
            drf["vch_num"] = model[0].Col16;
            drf["type"] = model[0].Col12;
            drf["ref_code"] = model[0].Col16;
            drf["ref_code1"] = model[0].Col16;
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
        [HttpGet]
        public FileResult fdown(string value, string type)
        {
            string path = "", fileName = "";
            FillMst();
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

        [HttpPost]
        public void fdelete(string value, string type)
        {
            sgen.SetSession(MyGuid, "delid", value);

        }
        #endregion

        #region cascade_ddl
        public ActionResult cascade_ddl()
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
            m_id_mst = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);

            tm1.Col10 = "master_setting";
            tm1.Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            tm1.Col13 = "Save";
            tm1.Col100 = "Save & New";
            tm1.Col121 = "S";
            tm1.Col122 = "<u>S</u>ave";
            tm1.Col123 = "Save & Ne<u>w</u>";
            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = m_id_mst.Trim();

            if (mid_mst.Trim().Equals("1008.21"))
            {
                tm1.Col9 = "SUB DEPARTMENT";//heading
                tm1.Col12 = "CD1";//type
                tm1.Col17 = "Department";//ddl label              
                tm1.Col18 = "Sub Department Name";//ddl label  
                tm1.Col143 = "subdept";
            }

            if (mid_mst.Trim().Equals("35001.2"))
            {
                tm1.Col9 = "SCHEME NAME";//heading
                tm1.Col12 = "SHN";//type
                tm1.Col17 = "Scheme Type";//ddl label              
                tm1.Col18 = "Scheme Name";//ddl label              
            }
            tm1.TList1 = mod1;
            tm1.SelectedItems1 = new string[] { "" };

            sgen.SetSession(MyGuid, "CD_TYPEMST", tm1.Col12.Trim()); //type
            sgen.SetSession(MyGuid, "CD_CONDMST", tm1.Col11.Trim()); //cond
            sgen.SetSession(MyGuid, "CD_MIDMST", tm1.Col14.Trim()); //mid
            model.Add(tm1);
            ViewBag.showextend = "N";
            if (sgen.Make_int(sgen.GetSession(MyGuid, "unitcnt").ToString()) > 1) ViewBag.showextend = "Y";
            return View(model);
        }

        public List<Tmodelmain> new_csddl(List<Tmodelmain> model)
        {
            sgen.SetSession(MyGuid, "EDMODE", "N");
            ViewBag.vnew = "disabled='disabled'";
            ViewBag.vedit = "disabled='disabled'";
            ViewBag.vsave = "";
            ViewBag.vsavenew = "";
            ViewBag.scripCall = "enableForm();";

            mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10.Trim() + " where type in ('DD" + model[0].Col12.Trim() + "','" + model[0].Col12.Trim() + "')";
            vch_num = sgen.genNo(userCode, mq, 3, "auto_genid");
            model[0].Col16 = vch_num;
            model[0].Col21 = "Active";
            //   string defval = "";
            List<SelectListItem> mod1 = new List<SelectListItem>();

            if (model[0].Col14.Trim().Equals("1008.21")) { mod1 = cmd_Fun.dept(userCode, clientid_mst, unitid_mst); }
            if (model[0].Col14.Trim().Equals("35001.2")) { mod1 = cmd_Fun.schtype(userCode, unitid_mst); }

            model[0].TList1 = mod1;
            TempData[MyGuid + "_TList1"] = mod1;
            model[0].SelectedItems1 = new string[] { "" }; return model;
        }
        [HttpPost]
        public ActionResult cascade_ddl(List<Tmodelmain> model, string command)
        {
            try
            {

                FillMst();
                DataTable dt = new DataTable();

                #region dropdown      
                List<SelectListItem> mod1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
                model[0].TList1 = mod1;
                TempData[MyGuid + "_TList1"] = model[0].TList1;
                if (model[0].SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
                #endregion

                if (command == "New")
                {
                    model = new_csddl(model);
                }
                else if (command == "Cancel") { return CancelFun(model); }
                else if (command == "Callback") { StartCallback(model); }
                else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
                {
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime(userCode);
                    ent_date = currdate;
                    type = model[0].Col12;

                    if (model[0].Col21.Trim() == "Inactive")
                    {
                        type = "DD" + type;
                        status = "N";
                    }
                    else status = "Y";

                    DataTable dtstr = new DataTable();
                    dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        mq1 = " and master_id<>'" + model[0].Col16.Trim() + "'";
                        isedit = true;
                        vch_num = model[0].Col16.Trim();
                    }
                    else
                    {
                        mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10.Trim() + " where type in ('DD" + model[0].Col12.Trim() + "','" + model[0].Col12 + "')";
                        vch_num = sgen.genNo(userCode, mq, 3, "auto_genid");
                        isedit = false;
                    }

                    cond = sgen.seekval(userCode, "select master_name from " + model[0].Col10.Trim() + " where upper(master_name)='" + model[0].Col18.Trim().ToString().ToUpper() + "' " +
                        "and type in ('DD" + model[0].Col12.Trim() + "','" + model[0].Col12 + "')" + mq1 + "", "master_name");
                    if (!cond.Equals("0"))
                    {
                        ViewBag.scripCall += "showmsgJS(1, 'Name Already Exists', 2);";
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                        return View(model);
                    }

                    #region dtstr module

                    DataRow dr = dtstr.NewRow();
                    dr["master_id"] = vch_num.Trim();
                    dr["vch_date"] = ent_date;
                    dr["type"] = type.Trim();
                    dr["master_name"] = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(model[0].Col19.ToLower());//name
                    dr["group_name"] = model[0].Col20;
                    dr["classid"] = model[0].SelectedItems1.FirstOrDefault();
                    dr["Status"] = status;
                    dr["Inactive_date"] = ent_date;

                    if (isedit)
                    {
                        dr["master_entby"] = model[0].Col3;
                        dr["master_entdate"] = model[0].Col4;
                        dr["client_id"] = model[0].Col1;
                        dr["client_unit_id"] = model[0].Col2;
                        dr["rec_id"] = model[0].Col7;
                        dr["master_editby"] = userid_mst;
                        dr["master_editdate"] = ent_date;
                    }
                    else
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

                    res = sgen.Update_data_fast1(userCode, dtstr, model[0].Col10.Trim(), model[0].Col8, isedit);
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
                            Col100 = "Save & New",
                            Col121 = "S",
                            Col122 = "<u>S</u>ave",
                            Col123 = "Save & Ne<u>w</u>",
                            Col14 = tmodel.Col14,
                            Col15 = tmodel.Col15,
                            Col17 = tmodel.Col17,
                            Col18 = tmodel.Col18,
                            TList1 = mod1,
                            SelectedItems1 = new string[] { "" },
                        });
                        if (command.Trim().ToUpper().Equals("SAVE") || command.Trim().ToUpper().Equals("UPDATE"))
                        {
                            try
                            {
                                if (sgen.Make_int(sgen.GetSession(MyGuid, "unitcnt").ToString()) > 1) ViewBag.showextend = "Y";
                            }
                            catch (Exception err)
                            {
                                mq = "select count(*) unitcnt from company_unit_profile where unit_status = '1' and company_profile_id='" + clientid_mst + "'";
                                mq = sgen.seekval(userCode, mq, "unitcnt");
                                sgen.SetSession(MyGuid, "unitcnt", mq);
                                if (sgen.Make_int(sgen.GetSession(MyGuid, "unitcnt").ToString()) > 1) ViewBag.showextend = "Y";
                            }
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
                                model = new_csddl(model);
                                ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                            }
                            catch (Exception ex) { }
                        }
                    }
                    else { ViewBag.scripCall = "showmsgJS(1, 'Something Went Wrong, Data Not Saved', 0);"; }
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

        #region frequency master
        public ActionResult freq_mst()
        {
            FillMst();
            chkRef();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "FREQUENCY CONFIG";
            model[0].Col10 = "master_setting";
            model[0].Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            model[0].Col12 = "IIT";
            List<SelectListItem> md1 = new List<SelectListItem>();
            TempData[MyGuid + "_TList1"] = model[0].TList1 = md1;
            model[0].SelectedItems1 = new string[] { "" };
            return View(model);
        }
        public List<Tmodelmain> new_freq_mst(List<Tmodelmain> model)
        {
            List<SelectListItem> mod1 = new List<SelectListItem>();
            model = getnew(model);
            mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
            vch_num = sgen.genNo(userCode, mq, 3, "auto_genid");
            model[0].Col16 = vch_num;
            mod1.Add(new SelectListItem { Text = "YEAR", Value = "YEAR" });
            mod1.Add(new SelectListItem { Text = "MONTH", Value = "MONTH" });
            mod1.Add(new SelectListItem { Text = "DAY", Value = "DAY" });
            TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
            model[0].SelectedItems1 = new string[] { "" };
            return model;
        }
        [HttpPost]
        public ActionResult freq_mst(List<Tmodelmain> model, string command)
        {
            try
            {

                FillMst();
                DataTable dt = new DataTable();
                var tm = model.FirstOrDefault();
                #region dropdown
                List<SelectListItem> mod1 = new List<SelectListItem>();
                model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
                TempData[MyGuid + "_TList1"] = model[0].TList1;
                if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
                #endregion
                if (command == "New")
                {
                    try
                    {
                        model = new_freq_mst(model);
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
                else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
                {
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime(userCode);
                    ent_date = currdate;
                    DataTable dtstr = new DataTable();
                    dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        mq1 = " and master_id<>'" + model[0].Col16.Trim() + "'";
                        isedit = true;
                        vch_num = model[0].Col16.Trim();
                    }
                    else
                    {
                        isedit = false;
                        vch_num = model[0].Col16;
                        mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10.Trim() + " a where " +
                       "type in ('" + model[0].Col12 + "','DD" + model[0].Col12 + "') " + model[0].Col11 + "";
                        vch_num = sgen.genNo(userCode, mq, 3, "auto_genid");
                        model[0].Col16 = vch_num;
                    }
                    cond = sgen.seekval(userCode, "select master_name from " + model[0].Col17.Trim() + " a where type='" + model[0].Col12.Trim() + "'" +
                        "" + model[0].Col11 + " " + mq1 + "", "master_name");
                    if (!cond.Equals("0"))
                    {
                        ViewBag.scripCall = "showmsgJS(1, 'Name Already Exists', 2);";
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                        return View(model);
                    }
                    #region dtstr module

                    DataRow dr = dtstr.NewRow();
                    dr["master_id"] = model[0].Col16.Trim();
                    dr["vch_date"] = currdate;
                    dr["type"] = model[0].Col12;
                    dr["master_name"] = model[0].Col17;
                    dr["SECTIONTYPE"] = model[0].SelectedItems1.FirstOrDefault();
                    dr["SUBJECTID"] = model[0].Col18; //Frequency Count
                    dr["classid"] = model[0].Col19; //Reminder Time (in Days)\
                    dr["SECTIONID"] = model[0].Col20;//Interval (In Days)
                    
                    if (isedit)
                    {
                        dr["master_entby"] = model[0].Col3;
                        dr["master_entdate"] = model[0].Col4;
                        dr["client_id"] = model[0].Col1;
                        dr["client_unit_id"] = model[0].Col2;
                        dr["rec_id"] = model[0].Col7;
                        dr["master_editby"] = userid_mst;
                        dr["master_editdate"] = ent_date;
                    }
                    else
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
                    res = sgen.Update_data_fast1(userCode, dtstr, model[0].Col10.Trim(), model[0].Col8, isedit);
                    if (res == true)
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
                            ViewBag.vsavenew = "disabled='disabled'";
                            ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                        }
                        else if (command.Trim().ToUpper().Equals("SAVE & NEW") || command.Trim().ToUpper().Equals("UPDATE & NEW"))
                        {
                            try
                            {
                                model = new_freq_mst(model);
                                ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                            }
                            catch (Exception ex) { }
                        }

                    }
                    else { ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Something Went Wrong, Data Not Saved');"; }
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
        private void filesave(List<Tmodelmain> model, string currdate, DataTable dtfile, string filename, string filepath, string filetitle, string content_type, string description, string page_title, string ref1, string ref2)
        {
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

    }
}