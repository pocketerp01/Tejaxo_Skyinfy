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
    public class LibController : Controller
    {

        System.Globalization.DateTimeFormatInfo monthName = new System.Globalization.DateTimeFormatInfo();
        string mq = "", mq1 = "", vch_num = "", btnval = "", type = "", type_desc = "", ent_date = "", status = "";
        string Master_Type = "", id = "", mid_mst = "", MyGuid = "", master_id, Ac_Year = "", iscfrm = "Y", where = "", cmd = "", tab_name = "", defval = "";

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

        string userCode = "", userid_mst = "", FN_From_Date = "", FN_To_Date = "", Ac_Year_id = "", cg_com_name = "", clientid_mst = "",
            unitid_mst = "", Ac_To_Date = "", Ac_From_Date = "", role_mst = "", clientname_mst = "", m_module3 = "", actionName = "", controllerName = "";


        // GET: Lib

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
            chkRef();
            Tmodelmain tm1 = new Tmodelmain();
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
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
            string defcall = "";
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<SelectListItem> mod2 = new List<SelectListItem>();
            List<SelectListItem> mod3 = new List<SelectListItem>();
            List<SelectListItem> mod4 = new List<SelectListItem>();
            List<SelectListItem> mod5 = new List<SelectListItem>();
            List<SelectListItem> mod6 = new List<SelectListItem>();

            var tm = model.FirstOrDefault();
            //controllerName = Session["controllerName"].ToString();
            //viewName = Session["viewName"].ToString();
            String URL = sgen.GetSession(MyGuid,"SSEEKVAL").ToString();
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

                #region add_pub
                case "add_pub":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select master_id,vch_num,rec_id,client_name,master_name,master_id,group_name,cont_person_name,client_gstin," +
                                "cont_person_number,cont_person_email,group_id,cont_person_altnumber,master_entby,master_editby,master_entdate,master_editdate,client_unit_id,client_id from master_setting " +
                                " where (client_id|| client_unit_id|| master_id||to_char(vch_date, 'ddmmyyyy')|| type)='" + URL + "'";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid,"EDMODE", "Y");
                                #region Country
                                DataTable dt3 = new DataTable();
                                dt3 = sgen.getdata(userCode, "select country_name,alpha_2 from country_state group by country_name,alpha_2 order by country_name,alpha_2");
                                if (dt3.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dt3.Rows)
                                    {
                                        mod1.Add(new SelectListItem { Text = dr["country_name"].ToString(), Value = dr["alpha_2"].ToString() });

                                    }
                                }
                                TempData[MyGuid + "_Tlist1"] = mod1;
                                #endregion
                                #region state
                                DataTable dt2 = new DataTable();
                                dt2 = sgen.getdata(userCode, "select distinct state_name,state_gst_code from country_state where alpha_2='" + dtt.Rows[0]["group_name"].ToString().Trim() + "' and state_name!='-' order by state_name");
                                if (dt2.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dt2.Rows)
                                    {
                                        mod2.Add(new SelectListItem { Text = dr["state_name"].ToString(), Value = dr["state_gst_code"].ToString() });

                                    }
                                }
                                TempData[MyGuid + "_Tlist2"] = mod2;
                                #endregion
                                #region city
                                dt = sgen.getdata(userCode, "SELECT city_name FROM (SELECT distinct city_name FROM country_state where state_gst_code='" + dtt.Rows[0]["cont_person_name"].ToString().Trim() + "' ) tab union SELECT 'Other' city_name from dual");
                                if (dt.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        mod3.Add(new SelectListItem { Text = dr["city_name"].ToString(), Value = dr["city_name"].ToString() });

                                    }
                                }
                                TempData[MyGuid + "_Tlist3"] = mod3;
                                #endregion
                                model[0].TList1 = mod1;
                                model[0].TList2 = mod2;
                                model[0].TList3 = mod3;
                                model[0].Col8 = "(client_id|| client_unit_id|| master_id|| to_char(vch_date, 'ddmmyyyy')|| type)='" + URL + "'";
                                model[0].Col16 = dtt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dtt.Rows[0]["client_name"].ToString();
                                model[0].Col18 = dtt.Rows[0]["master_name"].ToString();
                                model[0].Col22 = dtt.Rows[0]["cont_person_email"].ToString();
                                model[0].Col20 = dtt.Rows[0]["cont_person_number"].ToString();
                                model[0].Col19 = dtt.Rows[0]["group_id"].ToString();
                                model[0].Col21 = dtt.Rows[0]["cont_person_altnumber"].ToString();
                                model[0].Col26 = dtt.Rows[0]["master_id"].ToString();

                                model[0].Col5 = dtt.Rows[0]["master_editby"].ToString();
                                model[0].Col7 = dtt.Rows[0]["rec_id"].ToString();
                                model[0].Col3 = dtt.Rows[0]["master_entby"].ToString();
                                model[0].Col4 = dtt.Rows[0]["master_entdate"].ToString();
                                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                String[] L1 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["group_name"].ToString()).Distinct()).Split(',');
                                String[] L2 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["cont_person_name"].ToString()).Distinct()).Split(',');
                                String[] L3 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["client_gstin"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems1 = L1;
                                model[0].SelectedItems2 = L2;
                                model[0].SelectedItems3 = L3;
                                model[0].Col13 = "Update";
                            }
                            break;

                    }
                    break;
                #endregion

                

                #region book_issued

                case "book_issued":

                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            //        mq = "select to_char(convert_tz(a.date1,'UTC','" + sgen.Gettimezone()+"'),'"+sgen.Getsqldateformat()+ "') as Expected_Return_Date ," +
                            //            "to_char(convert_tz(a.vch_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') as vch_date ," +
                            //            " a.client_id||a.client_unit_id||a.deptname||'STD' as stdfsr,a.deptname staff_fstr," +
                            //            " a.vch_num,a.deptno,a.deptname,a.totremark,a.icode,a.qtyout,a.rec_id," +
                            //            "a.client_id,a.client_unit_id,a.ent_by,a.ent_date,a.edit_by,a.edit_date, b.col40 as Book_Name," +
                            //            " b.col1 as Author,b.col7 as edition,b.col8 as year" +
                            //            ",b.col10 as volume,b.col39 as Max_Days_For_Student,b.col21 as Max_Days_For_Teacher,'0' Expected_Return_Date,c.qtystk" +
                            //            "   from itransaction a inner join vehicle_master b on a.icode=b.vch_num and a.client_unit_id=b.client_unit_id and b.type='LBR'" +
                            //            "inner join (select a.icode,sum(a.qtyin) - sum(qtyout) as qtystk from (select vch_num as icode,sum(col11) " +
                            //"as QtyIn ,0 as qtyout from vehicle_master  where type='LBR' and client_unit_id='" + unitid_mst + "' group by vch_num " +
                            //"union " +
                            //"select icode,0 as qtyin ,sum(qtyout) as qtyout from itransaction  where type = '3A' and client_unit_id = '" + unitid_mst + "'  group by icode " +
                            //"union " +
                            //"select icode, sum(qtyin) as qtyin ,0 as qtyout from itransaction  where type = '1A' and client_unit_id = '" + unitid_mst + "'  group by icode) " +
                            //"a group by icode) c on c.icode=b.vch_num" + 
                            //            " where (a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type)='" + URL + "' ";


                            mq = "select to_char(convert_tz(a.date1,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') as Expected_Return_Date ," +
                           "to_char(convert_tz(a.vch_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') as vch_date ," +
                           " a.client_id||a.client_unit_id||a.deptname||'STD' as stdfsr,a.deptname staff_fstr," +
                           " a.vch_num,a.deptno,a.deptname,a.totremark,a.icode,a.qtyout,a.rec_id," +
                           "a.client_id,a.client_unit_id,a.ent_by,a.ent_date,a.edit_by,a.edit_date, b.col40 as Book_Name," +
                           " b.col1 as Author,b.col7 as edition,b.col8 as year" +
                           ",b.col10 as volume,b.col39 as Max_Days_For_Student,b.col21 as Max_Days_For_Teacher,'0' Expected_Return_Date,c.qtystk" +
                           "   from itransaction a inner join vehicle_master b on a.icode=b.vch_num and a.client_unit_id=b.client_unit_id and b.type='LBR'" +
                           "inner join (select a.icode,sum(a.qtyin) - sum(qtyout) as qtystk from (select vch_num as icode,sum(col11) " +
               "as QtyIn ,0 as qtyout from vehicle_master  where type='LBR' and client_unit_id='" + unitid_mst + "' group by vch_num " +
               "union " +
               "select icode,0 as qtyin ,sum(qtyout) as qtyout from itransaction  where type = '3A' and client_unit_id = '" + unitid_mst + "'  group by icode " +
               "union " +
               "select icode, sum(qtyin) as qtyin ,0 as qtyout from itransaction  where type = '1A' and client_unit_id = '" + unitid_mst + "'  group by icode) " +
               "a group by icode) c on c.icode=b.vch_num" +
                           " where (a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type)='" + URL + "' ";

                            dtt = sgen.getdata(userCode,mq);
                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid,"EDMODE", "Y");
                                if (dtt.Rows[0]["deptno"].ToString() == "001")
                                {
                                    model[0].Col22 = "Student Name";
                                    model[0].Col24 = "Membership Number";
                                    model[0].Col26 = "Class Section";
                                    

                                    mq = "select e.col37 as Membership_Number, c.Class as Class_Name,  d.master_name as Section, " +
                                    "((a.FIRST_NAME)||' '||replace(a.MIDDLE_NAME,'0','')||' '||replace(a.LAST_NAME,'0','')) as Student_Name," +
                                    "((a.f_firstname)||' '||replace(a.f_middlename,'0','')||' '||replace(a.f_lastname,'0','') ) as Father_Name" +
                                    " ,a.Gender ,a.RegNumber as Reg_Number,b.Roll_number as Roll_Number from student_detail b" +
                                   " inner join user_details a  on a.regnumber=b.reg_no and a.type= b.type and a.rec_id=b.vch_num" +
                                   " inner join add_class c on c.add_class_id=b.class_applied and c.type='EAC' and c.client_unit_id=b.client_unit_id  " +
                                   " inner join master_setting d on  b.section =d.master_id  and d.type='ECS' and d.client_unit_id=b.client_unit_id " +
                                   "inner join vehicle_master e on b.reg_no = e.col25  and e.type='LMS'  and e.client_unit_id=b.client_unit_id" +
                                   " where (b.client_id||b.client_unit_id||b.reg_no||b.type)='" + dtt.Rows[0]["stdfsr"].ToString() + "' " +
                                   " order by b.CLASS_APPLIED ,d.master_name";
                                    dt = sgen.getdata(userCode, mq);

                                    if (dt.Rows.Count > 0)
                                    {
                                        model[0].Col28 = dt.Rows[0]["Reg_Number"].ToString();
                                        model[0].Col23 = dt.Rows[0]["Student_Name"].ToString();
                                        model[0].Col25 = dt.Rows[0]["Membership_Number"].ToString();
                                        model[0].Col27 = dt.Rows[0]["Class_name"].ToString() + '-' + dt.Rows[0]["Section"].ToString();

                                    }
                                }

                                else
                                {
                                    model[0].Col22 = "Teacher Name";
                                    model[0].Col24 = "Department";
                                    model[0].Col26 = "Designation";

                                    mq = "SELECT a.rec_id,  Replace(a.first_name,'0','')||' '||Replace(a.MIDDLE_NAME,'0','')||" +
                                    "' '||Replace(a.last_name,'0','') AS Teacher_Name,a.rec_id,nvl(b.master_name,'-') as Designation ," +
                                    "nvl(c.master_name,'-') as Department FROM user_details a " +
                                    "left join master_setting b on a.designation=b.master_id and b.type='MDG'" +
                                    "left join master_setting c on a.department=c.master_id and b.type='MDP'" +
                                 " where(a.rec_id) ='" + dtt.Rows[0]["staff_fstr"].ToString() + "'  ";

                                    dt = sgen.getdata(userCode, mq);

                                    if (dt.Rows.Count > 0)
                                    {
                                        model[0].Col28 = dt.Rows[0]["rec_id"].ToString();
                                        model[0].Col23 = dt.Rows[0]["Teacher_Name"].ToString();
                                        model[0].Col25 = dt.Rows[0]["Department"].ToString();
                                        model[0].Col27 = dt.Rows[0]["Designation"].ToString();

                                    }
                                }
                                model[0].Col8 = "(client_id||client_unit_id|| vch_num|| to_char(vch_date, 'yyyymmdd')|| type) = '" + URL + "'";
                                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                                model[0].Col29 = dtt.Rows[0]["deptno"].ToString();
                                model[0].Col21 = dtt.Rows[0]["totremark"].ToString();
                                model[0].Col18 = dtt.Rows[0]["vch_num"].ToString();
                                model[0].Col20 = dtt.Rows[0]["vch_date"].ToString();
                                model[0].Col13 = "Update";
                                model[0].Col100 = "Update & New";

                                model[0].dt1 = new DataTable();
                                model[0].dt1 = ((DataTable)sgen.GetSession(MyGuid, "dtbase")).Clone();
                                for (int i = 0; i < dtt.Rows.Count; i++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["SNo"] = i + 1;
                                    dr["Icode"] = dtt.Rows[i]["icode"].ToString();
                                    dr["Book_Name"] = dtt.Rows[i]["Book_Name"].ToString();
                                    dr["Author"] = dtt.Rows[i]["Author"].ToString();
                                    dr["Volume"] = dtt.Rows[i]["volume"].ToString();
                                    dr["Available_Books"] = dtt.Rows[i]["qtystk"].ToString();
                                    dr["Qty_issue"] = dtt.Rows[i]["qtyout"].ToString();
                                    dr["Expected_Return_Date"] = dtt.Rows[i]["Expected_Return_Date"].ToString();


                                    model[0].dt1.Rows.Add(dr);
                                }
                                model[0].dt1.AcceptChanges();
                            }
                           
                            break;

                        case "NEW":

                            mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a " +
                                "where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                            vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                            model[0].Col18 = vch_num;
                            model[0].Col20 = sgen.server_datetime_local(userCode);

                            sgen.SetSession(MyGuid,"EDMODE", "N");
                            ViewBag.vnew = "disabled='disabled'";
                            ViewBag.vedit = "disabled='disabled'";
                            ViewBag.vsave = "";
                            ViewBag.scripCall = "enableForm();";
                            if (URL == "001")
                            {
                                model[0].Col22 = "Student Name";
                                model[0].Col24 = "Membership Number";
                                model[0].Col26 = "Class Section";
                                sgen.SetSession(MyGuid,"Issue_to","001");//STD

                            }

                            else
                            {
                                model[0].Col22 = "Teacher Name";
                                model[0].Col24 = "Department";
                                model[0].Col26 = "Designation";
                                
                                sgen.SetSession(MyGuid,"Issue_to", "002"); //STAFF
                            }
                            model[0].Col29 = URL;
                            break;

                        case "REG":

                            if (sgen.GetSession(MyGuid, "Issue_to").ToString() == "001")//STD
                            {
                                mq = "select e.col37 as Membership_Number, c.Class as Class_Name,  d.master_name as Section, " +
                                     "((a.FIRST_NAME)||' '||replace(a.MIDDLE_NAME,'0','')||' '||replace(a.LAST_NAME,'0','')) as Student_Name," +
                                     "((a.f_firstname)||' '||replace(a.f_middlename,'0','')||' '||replace(a.f_lastname,'0','') ) as Father_Name" +
                                     " ,a.Gender ,a.RegNumber as Reg_Number,b.Roll_number as Roll_Number from student_detail b" +
                                    " inner join user_details a  on a.regnumber=b.reg_no and a.type= b.type and a.rec_id=b.vch_num" +
                                    " inner join add_class c on c.add_class_id=b.class_applied and c.type='EAC' and c.client_unit_id=b.client_unit_id  " +
                                    " inner join master_setting d on  b.section =d.master_id  and d.type='ECS' and d.client_unit_id=b.client_unit_id " +
                                    "inner join vehicle_master e on b.reg_no = e.col25  and e.type='LMS'  and e.client_unit_id=b.client_unit_id" +
                                    " where (b.client_id||b.client_unit_id||b.vch_num||to_char(b.vch_date,'yyyymmdd')||b.type)='"+URL+"' " +
                                    " order by b.CLASS_APPLIED ,d.master_name";
                                dt = sgen.getdata(userCode, mq);

                                if (dt.Rows.Count > 0)
                                {
                                    model[0].Col28 = dt.Rows[0]["Reg_Number"].ToString();
                                    model[0].Col23 = dt.Rows[0]["Student_Name"].ToString();
                                    model[0].Col25 = dt.Rows[0]["Membership_Number"].ToString();
                                    model[0].Col27 = dt.Rows[0]["Class_name"].ToString() + '-' + dt.Rows[0]["Section"].ToString();

                                }

                            }

                            else if (sgen.GetSession(MyGuid, "Issue_to").ToString() == "002")//STAFF
                            {
                                mq = "SELECT a.rec_id,  Replace(a.first_name,'0','')||' '||Replace(a.MIDDLE_NAME,'0','')||" +
                                     "' '||Replace(a.last_name,'0','') AS Teacher_Name,a.rec_id,nvl(b.master_name,'-') as Designation ," +
                                     "nvl(c.master_name,'-') as Department FROM user_details a " +
                                     "left join master_setting b on a.designation=b.master_id and b.type='MDG'" +
                                     "left join master_setting c on a.department=c.master_id and b.type='MDP'" +
                                  " where(a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||b.type) ='"+URL+"'  ";

                                dt = sgen.getdata(userCode, mq);

                                if (dt.Rows.Count > 0)
                                {
                                    model[0].Col28 = dt.Rows[0]["rec_id"].ToString();
                                    model[0].Col23 = dt.Rows[0]["Teacher_Name"].ToString();
                                    model[0].Col25 = dt.Rows[0]["Department"].ToString();
                                    model[0].Col27 = dt.Rows[0]["Designation"].ToString();

                                }
                            }


                            break;

                        case "ITEM":

                           

                            mq = "select (b.client_id||b.client_unit_id||b.vch_num||to_char(b.vch_date,'yyyymmdd')||b.type) fstr," +
                                "b.vch_num as icode,b.col40 as Book_Name, b.col1 as Author,b.col7 as edition,b.col8 as year" +
                                ",b.col10 as volume,b.col39 as Max_Days_For_Student,b.col21 as Max_Days_For_Teacher,'0' Expected_Return_Date,a.qtystk" +
                                " from vehicle_master b  inner join (select a.icode,sum(a.qtyin) - sum(qtyout) as qtystk from (select vch_num as icode,sum(col11) " +
                    "as QtyIn ,0 as qtyout from vehicle_master  where type='LBR' and client_unit_id='" + unitid_mst + "' group by vch_num " +
                    "union " +
                    "select icode,0 as qtyin ,sum(qtyout) as qtyout from itransaction  where type = '3A' and client_unit_id = '" + unitid_mst + "'  group by icode " +
                    "union " +
                    "select icode, sum(qtyin) as qtyin ,0 as qtyout from itransaction  where type = '1A' and client_unit_id = '" + unitid_mst + "'  group by icode) " +
                    "a group by icode)a on a.icode=b.vch_num where (b.client_id||b.client_unit_id||b.vch_num||to_char(b.vch_date,'yyyymmdd')||b.type)='" + URL+"'";

                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                if (model[0].dt1.Rows[0]["icode"].ToString().Trim().Equals("-")) { model[0].dt1.Rows.RemoveAt(0); }

                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["SNo"] = dt.Rows[i]["fstr"].ToString(); ;
                                    dr["Icode"] = dt.Rows[i]["icode"].ToString();
                                    dr["Book_Name"] = dt.Rows[i]["Book_Name"].ToString();
                                    dr["Author"] = dt.Rows[i]["Author"].ToString();
                                    dr["Volume"] = dt.Rows[i]["volume"].ToString();
                                    dr["Available_Books"] = dt.Rows[i]["qtystk"].ToString();
                                    dr["Qty_issue"] = "0";
                                    dr["Expected_Return_Date"] = dt.Rows[i]["Expected_Return_Date"].ToString();
                                  

                                    model[0].dt1.Rows.Add(dr);
                                }
                            }
                            break;

                      
                    }
                    break;

                #endregion

                #region book_return

                case "book_return":

                    switch (btnval.ToUpper())
                    {
                        case "EDIT":


                            break;

                        case "NEW":

                            mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                            vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                            model[0].Col18 = vch_num;
                            model[0].Col20 = sgen.server_datetime_local(userCode);

                            sgen.SetSession(MyGuid,"EDMODE", "N");
                            ViewBag.vnew = "disabled='disabled'";
                            ViewBag.vedit = "disabled='disabled'";
                            ViewBag.vsave = "";
                            ViewBag.vsavenew = "";
                            ViewBag.scripCall = "enableForm();";
                            if (URL == "001")
                            {
                                model[0].Col22 = "Student Name";
                                model[0].Col24 = "Membership Number";
                                model[0].Col26 = "Class Section";
                                sgen.SetSession(MyGuid,"Return_From", "001");//STD

                            }

                            else
                            {
                                model[0].Col22 = "Teacher Name";
                                model[0].Col24 = "Department";
                                model[0].Col26 = "Designation";

                                sgen.SetSession(MyGuid,"Return_From", "002");//STAFF
                            }
                            model[0].Col29 = URL;
                            model[0].Col10 = "Save & New";
                            break;

                        case "REG":

                            if (sgen.GetSession(MyGuid, "Return_From").ToString() == "001")//STD
                            {
                                mq = "select e.col37 as Membership_Number, c.Class as Class_Name,  d.master_name as Section, " +
                                     "((a.FIRST_NAME)||' '||replace(a.MIDDLE_NAME,'0','')||' '||replace(a.LAST_NAME,'0','')) as Student_Name," +
                                     "((a.f_firstname)||' '||replace(a.f_middlename,'0','')||' '||replace(a.f_lastname,'0','') ) as Father_Name" +
                                     " ,a.Gender ,a.RegNumber as Reg_Number,b.Roll_number as Roll_Number from student_detail b" +
                                    " inner join user_details a  on a.regnumber=b.reg_no and a.type= b.type and a.rec_id=b.vch_num" +
                                    " inner join add_class c on c.add_class_id=b.class_applied and c.type='EAC' and c.client_unit_id=b.client_unit_id  " +
                                    " inner join master_setting d on  b.section =d.master_id  and d.type='ECS' and d.client_unit_id=b.client_unit_id " +
                                    "inner join vehicle_master e on b.reg_no = e.col25  and e.type='LMS'  and e.client_unit_id=b.client_unit_id" +
                                    " where (b.client_id||b.client_unit_id||b.vch_num||to_char(b.vch_date,'yyyymmdd')||b.type)='" + URL + "' " +
                                    " order by b.CLASS_APPLIED ,d.master_name";
                                dt = sgen.getdata(userCode, mq);

                                if (dt.Rows.Count > 0)
                                {
                                    model[0].Col28 = dt.Rows[0]["Reg_Number"].ToString();
                                    model[0].Col23 = dt.Rows[0]["Student_Name"].ToString();
                                    model[0].Col25 = dt.Rows[0]["Membership_Number"].ToString();
                                    model[0].Col27 = dt.Rows[0]["Class_name"].ToString() + '-' + dt.Rows[0]["Section"].ToString();

                                }
                                model[0].Col100 = tm.Col100;

                            }

                            else if (sgen.GetSession(MyGuid, "Return_From").ToString() == "002")//STAFF
                            {
                                mq = "SELECT a.rec_id,  Replace(a.first_name,'0','')||' '||Replace(a.MIDDLE_NAME,'0','')||" +
                                     "' '||Replace(a.last_name,'0','') AS Teacher_Name,a.rec_id,nvl(b.master_name,'-') as Designation ," +
                                     "nvl(c.master_name,'-') as Department FROM user_details a " +
                                     "left join master_setting b on a.designation=b.master_id and b.type='MDG'" +
                                     "left join master_setting c on a.department=c.master_id and b.type='MDP'" +
                                  " where(a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||b.type) ='" + URL + "'  ";

                                dt = sgen.getdata(userCode, mq);

                                if (dt.Rows.Count > 0)
                                {
                                    model[0].Col28 = dt.Rows[0]["rec_id"].ToString();
                                    model[0].Col23 = dt.Rows[0]["Teacher_Name"].ToString();
                                    model[0].Col25 = dt.Rows[0]["Department"].ToString();
                                    model[0].Col27 = dt.Rows[0]["Designation"].ToString();

                                }
                            }

                            sgen.SetSession(MyGuid,"Ret_Frm", model[0].Col28);
                            model[0].Col100 = tm.Col100;


                            break;

                        case "ITEM":



                            mq = "select (a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||b.type) as fstr ," +
                                "a.vch_num as Issue_NO ,to_char(convert_tz(a.vch_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') as Issue_Date" +
                                ", a.qtyout,a.icode ,b.col40 as Book_Name, b.col1 as Author,b.col7 as edition" +
                                ",b.col8 as year,b.col10 as volume,c.qtystk from itransaction a inner join vehicle_master b on a.icode = b.vch_num" +

                                " and b.type = 'LBR' and a.client_id = b.client_id and a.client_unit_id = b.client_unit_id inner join (select a.icode,sum(a.qtyin) - sum(qtyout) as qtystk from (select vch_num as icode,sum(col11) " +
                    "as QtyIn ,0 as qtyout from vehicle_master  where type='LBR' and client_unit_id='" + unitid_mst + "' group by vch_num " +
                    "union " +
                    "select icode,0 as qtyin ,sum(qtyout) as qtyout from itransaction  where type = '3A' and client_unit_id = '" + unitid_mst + "'  group by icode " +
                    "union " +
                    "select icode, sum(qtyin) as qtyin ,0 as qtyout from itransaction  where type = '1A' and client_unit_id = '" + unitid_mst + "'  group by icode) " +
                    "a group by icode)c on c.icode=b.vch_num where" +
                                " (a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||b.type)='"+URL+"'";

                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                if (model[0].dt1.Rows[0]["icode"].ToString().Trim().Equals("-")) { model[0].dt1.Rows.RemoveAt(0); }

                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["SNo"] = dt.Rows[i]["fstr"].ToString(); ;
                                    dr["Icode"] = dt.Rows[i]["icode"].ToString();
                                    dr["Book_Name"] = dt.Rows[i]["Book_Name"].ToString();
                                    dr["Author"] = dt.Rows[i]["Author"].ToString();
                                    dr["Volume"] = dt.Rows[i]["volume"].ToString();
                                    dr["Available_Books"] = dt.Rows[i]["qtystk"].ToString(); ;
                                    dr["Issue_No"] = dt.Rows[i]["Issue_NO"].ToString();
                                    dr["Issue_Date"] = dt.Rows[i]["Issue_Date"].ToString();
                                    dr["Qty_issue"] = dt.Rows[i]["qtyout"].ToString();
                                    dr["Qty_issue"] = dt.Rows[i]["qtyout"].ToString();
                                    dr["Qty_return"] = "0";
                                   


                                    model[0].dt1.Rows.Add(dr);
                                }
                                model[0].Col100 = tm.Col100;

                            }
                            break;


                    }
                    break;

                #endregion

                #region Add Journal

                case "add_jurnl":
                    switch (btnval.ToUpper())
                    {

                        case "EDIT":

                            #region Frequency 1
                            dt = sgen.getdata(userCode, "select master_id,master_name from master_setting where type='EFQ' and client_id='0' and client_unit_id='0'");
                            if (dt.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    mod1.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                                }
                                TempData[MyGuid + "_Tlist1"] = mod1;
                            }
                            #endregion

                            mq = "select a.vch_num,a.col1,a.client_id,a.client_unit_id,a.ent_by,a.type," +
                                "nvl(to_char(a.ent_date, '" + sgen.GetSaveSqlDateFormat() + "'), '0') as ent_date,a.rec_id,a.col2" +
                                ",nvl(to_char(a.vch_date, '" + sgen.GetSaveSqlDateFormat() + "'), '0') as vch_date," +
                                "a.col3 as Frequency,a.col4,a.col7,a.col8,a.col12,a.col31," +
                                "to_char(convert_tz(a.date1, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Due_date," +
                                "to_char(convert_tz(a.date2, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Start_date," +
                                "to_char(convert_tz(a.date3, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as End_date " +
                                "from vehicle_master " +
                                "a where (a.client_id || a.client_unit_id || a.vch_num || to_char(a.vch_date, 'yyyymmdd') || a.type) = '" + URL + "'";

                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid,"EDMODE", "Y");
                                model[0].Col16 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col8 = "client_id|| client_unit_id || vch_num ||To_Char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";
                                model[0].Col18 = dt.Rows[0]["col1"].ToString();        //Publisher
                                model[0].Col12 = dt.Rows[0]["type"].ToString();        
                                model[0].Col19 = dt.Rows[0]["col2"].ToString();       //Author
                                model[0].Col20 = dt.Rows[0]["col4"].ToString();        
                                model[0].Col21 = dt.Rows[0]["col7"].ToString();        
                                model[0].Col22 = dt.Rows[0]["Due_date"].ToString();       
                                model[0].Col26 = dt.Rows[0]["Start_date"].ToString();        
                                model[0].Col27 = dt.Rows[0]["End_date"].ToString();        
                                model[0].Col23 = dt.Rows[0]["col8"].ToString();       
                                model[0].Col24 = dt.Rows[0]["col12"].ToString();       
                                model[0].Col25 = dt.Rows[0]["col31"].ToString();       
                                model[0].Col3 = dt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dt.Rows[0]["ent_date"].ToString();
                                model[0].TList1 = mod1;
                                String[] L1 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["Frequency"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems1 = L1;    //Frequency
                                model[0].Col13 = "Update";
                                model[0].Col100 = "Update & New";

                            }

                            break;
                    }
                    break;

                #endregion

                #region Add BOOK

                case "add_book":
                    switch (btnval.ToUpper())
                    {

                        case "EDIT":
                            //mq = "select (ft.file_path),(ft.file_name),vm.vch_num,vm.col40,vm.col1,vm.col32,vm.col33,vm.col34,vm.col2,vm.col3,vm.col7,vm.col8," +
                            //     "vm.col9,vm.col10,vm.col11,vm.col13,vm.col14,vm.col15,vm.col37,vm.col18,vm.col19,vm.col20,vm.col10,vm.col11,vm.col13,vm.col14,vm.col15,vm.col37,vm.col18,vm.col19,vm.col20,vm.col28," +
                            //     " vm.col29,to_char(convert_tz(vm.date1,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') as pur_dt,vm.col30," +
                            //     " to_char(convert_tz(vm.date2,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "' )as bill_dt ,vm.col30,vm.vch_num,vm.rec_id,vm.client_id,vm.client_unit_id,vm.ent_by,vm.ent_date from vehicle_master vm " +
                            //     "left join file_tab ft on(vm.client_id || vm.client_unit_id || vm.vch_num || to_char(vm.vch_date, 'yyyymmdd') || vm.type) =" +
                            //     " (ft.client_id || ft.client_unit_id || ft.ref_code || to_char(ft.vch_date, 'yyyymmdd') || ft.type) " +
                            //     " where (vm.client_id||vm.client_unit_id||vm.vch_num||to_char(vm.vch_date,'yyyymmdd')||vm.type)= '" + URL + "'";

                            mq = "select (isd.client_id||isd.client_unit_id||isd.vch_num||to_char(isd.vch_date,'yyyymmdd')||isd.type) as isd_fstr," +
                                "vm.vch_num ,nvl((lcd.client_id || lcd.client_unit_id || lcd.vch_num || to_char(lcd.vch_date, 'yyyymmdd') || lcd.type), 0) as lcd_fstr," +
                                "nvl(isd.type, 0) as isd_type,nvl(lcd.type, 0) as lcd_type,nvl(lcd.col1, 0) as location_id,nvl(lcd.col4, 0) as lc_quantity,vm.col40,vm.col1," +
                                "vm.col32,vm.col33,vm.col34,vm.col2,vm.col3 as dd1,vm.col7,vm.col8,vm.col9,vm.col10,vm.col11,cl.vch_num as pub_id," +
                                "cl.c_name as publisher_name,vm.col13 as dd4,vm.col14,vm.col15 as dd2,vm.col37 as dd3,vm.col18,vm.col19,vm.col20," +
                                "vm.col10,vm.col11,vm.col14,vm.col18,vm.col19,vm.col20,vm.col28, vm.col29," +
                                "to_char(convert_tz(vm.date1, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as pur_dt,vm.col30, " +
                                "to_char(convert_tz(vm.date2, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as bill_dt ," +
                                "vm.col30,vm.vch_num,vm.rec_id,nvl(isd.col1, 0) as isu_ignst_id," +
                                "nvl(isd.col2, 0) as max_days_std,nvl(isd.col3, 0) as max_days_tchr,vm.client_id,vm.client_unit_id,vm.ent_by,vm.ent_date " +
                                "from vehicle_master vm left join clients_mst cl on cl.vch_num = vm.col2 and cl.type = 'PVD' and cl.client_unit_id = vm.client_unit_id" +
                                " and cl.client_id = vm.client_id left join enx_tab isd on isd.vch_num = vm.vch_num and isd.type = 'LBA' and " +
                                "isd.client_unit_id = vm.client_unit_id and isd.client_id = vm.client_id left join enx_tab lcd on " +
                                "lcd.vch_num = vm.vch_num and lcd.type = 'LBP' and lcd.client_unit_id = vm.client_unit_id and vm.client_id = lcd.client_id " +
                                "where (vm.client_id || vm.client_unit_id || vm.vch_num || to_char(vm.vch_date, 'yyyymmdd') || vm.type) = '"+URL+"'";

                            DataTable dtreg = sgen.getdata(userCode, mq);

                            #region Book Catagory 1
                            mod1 = cmd_Fun.book_ctgry(userCode, unitid_mst);
                            TempData[MyGuid + "_TList1"] = mod1;

                            #endregion

                            #region class 2
                            mod2 = cmd_Fun.lib_class(userCode, unitid_mst);
                            TempData[MyGuid + "_TList2"] = mod2;

                            #endregion

                            #region subject 3
                            mod3 = cmd_Fun.lib_subject(userCode, unitid_mst);
                            TempData[MyGuid + "_TList3"] = mod3;

                            #endregion

                            #region language 4
                            mod6 = cmd_Fun.lang(userCode, unitid_mst);
                            TempData[MyGuid + "_TList4"] = mod6;
                            #endregion

                            #region issue against ltm2 1
                            mod4 = cmd_Fun.issue_aginst(userCode, unitid_mst);
                            TempData[MyGuid + "_L2TList1"] = mod4;
                            #endregion

                            #region Section ltm1 1
                            mod5 = cmd_Fun.iloc(userCode, unitid_mst, out defcall);
                            TempData[MyGuid + "_L1TList1"] = mod5;
                            #endregion


                            if (dtreg.Rows.Count > 0)
                            {
                                model[0].Col8 = "(client_id || client_unit_id || vch_num || to_char(vch_date, 'yyyymmdd') || type) = '" + URL + "'";
                                model[0].Col85 = "(client_id||client_unit_id||vch_num||to_char(vch_date, 'yyyymmdd')|| type) = '" + dtreg.Rows[0]["isd_fstr"].ToString() + "'";
                                model[0].Col86 = "(client_id||client_unit_id||vch_num||to_char(vch_date, 'yyyymmdd')|| type) = '" + dtreg.Rows[0]["lcd_fstr"].ToString() + "'";

                                model[0].Col16 = dtreg.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dtreg.Rows[0]["col40"].ToString();
                                model[0].Col18 = dtreg.Rows[0]["col1"].ToString();
                                model[0].Col19 = dtreg.Rows[0]["col32"].ToString();
                                model[0].Col20 = dtreg.Rows[0]["col33"].ToString();
                                model[0].Col21 = dtreg.Rows[0]["col34"].ToString();
                                model[0].Col23 = dtreg.Rows[0]["col2"].ToString();   //publisher id
                                //ddl_book_category.Value = dtreg.Rows[0]["col3"].ToString();
                                model[0].Col24 = dtreg.Rows[0]["col7"].ToString();
                                model[0].Col25 = dtreg.Rows[0]["col8"].ToString();
                                model[0].Col26 = dtreg.Rows[0]["col9"].ToString();
                                model[0].Col27= dtreg.Rows[0]["col10"].ToString();
                                model[0].Col28 = dtreg.Rows[0]["col11"].ToString();
                                //txt_language.Text = dtreg.Rows[0]["col13"].ToString();
                                model[0].Col30 = dtreg.Rows[0]["col14"].ToString();
                                //ddl_class.Value = dtreg.Rows[0]["col15"].ToString();
                                //ddl_subject.Value = dtreg.Rows[0]["col37"].ToString();
                                model[0].Col31 = dtreg.Rows[0]["col18"].ToString();
                                model[0].Col32 = dtreg.Rows[0]["col19"].ToString();
                                model[0].Col33 = dtreg.Rows[0]["col20"].ToString();
                                model[0].Col34 = dtreg.Rows[0]["pur_dt"].ToString();
                                model[0].Col35 = dtreg.Rows[0]["col28"].ToString();
                                model[0].Col41 = dtreg.Rows[0]["col29"].ToString();
                                model[0].Col42 = dtreg.Rows[0]["col30"].ToString();
                                model[0].Col43 = dtreg.Rows[0]["bill_dt"].ToString();
                                model[0].Col1 = dtreg.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dtreg.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dtreg.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dtreg.Rows[0]["ent_date"].ToString();
                                model[0].Col7 = dtreg.Rows[0]["rec_id"].ToString();
                                model[0].TList1 = mod1;
                                model[0].TList2 = mod2;
                                model[0].TList3 = mod3;
                                model[0].TList4 = mod6;

                                String[] L1 = System.String.Join(",", dtreg.Rows.OfType<DataRow>().Select(r => r["dd1"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems1 = L1;       
                                String[] L2 = System.String.Join(",", dtreg.Rows.OfType<DataRow>().Select(r => r["dd2"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems2 = L2;         
                                String[] L3 = System.String.Join(",", dtreg.Rows.OfType<DataRow>().Select(r => r["dd3"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems3 = L3;       
                                String[] L4 = System.String.Join(",", dtreg.Rows.OfType<DataRow>().Select(r => r["dd4"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems4 = L4;

                                model[0].LTM2 = new List<Tmodelmain>();
                                if (!dtreg.Rows[0]["isu_ignst_id"].Equals(0))
                                {
                                    for (int i = 0; i < dtreg.Rows.Count; i++)
                                    {
                                        Tmodelmain ltm2 = new Tmodelmain();
                                        if (i == 0)
                                        {
                                            //model[0].LTM2[0].TList1 = mod5;
                                            //model[0].LTM1[0].TList1 = mod4;
                                            ltm2.TList1 = mod4;
                                            //ltm2.TList2 = mod5;
                                            ltm2.Col10 = dtreg.Rows[0]["max_days_std"].ToString();  //
                                            ltm2.Col11 = dtreg.Rows[0]["max_days_tchr"].ToString();  //

                                            String[] L5 = System.String.Join(",", dtreg.Rows.OfType<DataRow>().Select(r => r["isu_ignst_id"].ToString()).Distinct()).Split(',');
                                            ltm2.SelectedItems1 = L5;
                                            //String[] L6 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["curtype"].ToString()).Distinct()).Split(',');
                                            //ltm2.SelectedItems1 = L6;
                                        }
                                        model[0].LTM2.Add(ltm2);
                                    }
                                }
                                if (!dtreg.Rows[0]["location_id"].Equals("0"))
                                {
                                    for (int i = 0; i < dtreg.Rows.Count; i++)
                                    {
                                        Tmodelmain ltm2 = new Tmodelmain();
                                        if (i == 0)
                                        {
                                            //model[0].LTM2[0].TList1 = mod5;
                                            //model[0].LTM1[0].TList1 = mod4;
                                            ltm2.TList1 = mod5;
                                            //ltm2.TList2 = mod5;
                                            ltm2.Col10 = dtreg.Rows[0]["lc_quantity"].ToString();  //
                                            //ltm2.Col11 = dtt.Rows[0]["max_days_tchr"].ToString();  //

                                            String[] L5 = System.String.Join(",", dtreg.Rows.OfType<DataRow>().Select(r => r["location_id"].ToString()).Distinct()).Split(',');
                                            ltm2.SelectedItems1 = L5;
                                            //String[] L6 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["curtype"].ToString()).Distinct()).Split(',');
                                            //ltm2.SelectedItems1 = L6;
                                        }
                                        model[0].LTM1.Add(ltm2);
                                    }
                                }
                                //txt_billnum.Text = dtreg.Rows[0]["col30"].ToString();


                            }

                            break;



                        case "PUB":
                            mq = "select vm.vch_num,vm.c_name as PublisherName from clients_mst vm where (vm.client_id||vm.client_unit_id|| vm.vch_num||to_char(vm.vch_date, 'yyyymmdd')|| vm.type)='" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model[0].Col22 = dt.Rows[0]["PublisherName"].ToString();
                                model[0].Col23 = dt.Rows[0]["vch_num"].ToString();
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

        //public void Make_query(string viewname, string title, string btnval, string seektype)
        public void Make_query(string viewname, string title, string btnval, string seektype, string param1, string param2,string Myguid="")
        {
            string cond = "";
            FillMst(Myguid);
            btnval = btnval.ToUpper();
            sgen.SetSession(MyGuid,"btnval", btnval);
            string cmd = "";
            clientid_mst = sgen.GetCookie(MyGuid, "clientid_mst");
            unitid_mst = sgen.GetCookie(MyGuid, "unitid_mst");
            switch (viewname.ToLower())
            {
                #region Export Party Assigned

                case "party_ass":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = "SELECT a.client_id||a.client_unit_id||a.master_id||to_char(a.vch_date,'yyyymmdd')||a.type as fstr, a.master_name as Education_Category," +
                        "Section_Strength as Class_Hours,classid as Class_Starting_Time,sectionid as Class_End_Time ,(CASE when a.status='Y' THEN 'Active' when a.status = '' THEN 'Active' else 'Inactive' end) as Status  " +
                           ",b.First_name ||' ' || b.middle_name || ' '|| b.last_name as Ent_By,to_char( convert_tz( a.master_EntDate,'UTC','" + sgen.Gettimezone() + "')," +
                           "" +
                           "'" + sgen.Getsqldateformat() + "')as Ent_Date from master_setting a,user_details b  where a.type in ( 'ECC','DDECC') and  a.master_entby =lpad(b.rec_id,6,'0') and a.client_id='" + clientid_mst + "' and a.client_unit_id='" + unitid_mst + "' order by a.master_name ";
                            break;
                    }

                    break;

                #endregion

                #region add_pub
                case "add_pub":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = "Select DISTINCT (ms.client_id||ms.client_unit_id||ms.master_id||to_char(ms.vch_date,'ddmmyyyy')||ms.type) as fstr,ms. master_name as name,ms.cont_person_number as Contact,ms.cont_person_altnumber as Alter_number,ms.cont_person_email as  email,ms.group_id as address," +
                                "client_gstin as cilty_name, cs.country_name,cs.state_name from master_setting ms " +
                                "inner join  country_state cs on ms.group_name = cs.alpha_2 and cs.client_id = ms.client_id and ms.client_unit_id = cs.client_unit_id and ms.cont_person_name = cs.state_gst_code" +
                                " where ms.type = 'APM' and ms.client_id = '" + clientid_mst + "' and ms.client_unit_id = '" + unitid_mst + "'";
                            break;
                    }
                    break;
                #endregion

                #region book_issued

                case "book_issued":
                    //if (Session["KISTYPE"] != null) { type = Session["KISTYPE"].ToString().Trim(); }
                    //else { type = "30','32"; }

                    switch (btnval.ToUpper())
                    {
                        case "NEW":

                            cmd = "select '001' as fstr, 'Issue To Student' as Issue_to from dual union select '002' as fstr, 'Issue To Staff' as Issue_to from dual ";

                            break;

                        case "REG":

                            if (sgen.GetSession(MyGuid, "Issue_to").ToString() == "001")//STD
                            {
                                cmd = "select (b.client_id||b.client_unit_id||b.vch_num||to_char(b.vch_date,'yyyymmdd')||b.type) as fstr," +
                                    " e.col37 as Membership_Number, c.Class as Class_Name,  d.master_name as Section, " +
                                     "((a.FIRST_NAME)||' '||replace(a.MIDDLE_NAME,'0','')||' '||replace(a.LAST_NAME,'0','')) as Student_Name," +
                                     "((a.f_firstname)||' '||replace(a.f_middlename,'0','')||' '||replace(a.f_lastname,'0','') ) as Father_Name" +
                                     " ,a.Gender ,a.RegNumber as Reg_Number,b.Roll_number as Roll_Number from student_detail b" +
                                    " inner join user_details a  on a.regnumber=b.reg_no and a.type= b.type and a.rec_id=b.vch_num" +
                                    " inner join add_class c on c.add_class_id=b.class_applied and c.type='EAC' and c.client_unit_id=b.client_unit_id  " +
                                    " inner join master_setting d on  b.section =d.master_id  and d.type='ECS' and d.client_unit_id=b.client_unit_id " +
                                    "inner join vehicle_master e on b.reg_no = e.col25  and e.type='LMS'  and e.client_unit_id=b.client_unit_id" +
                                    " where b.type='STD' and b.client_id='" + clientid_mst + "' and b.client_unit_id='" + unitid_mst + "' " +
                                    " and b.Academic_year_Id='" + Ac_Year_id + "' and b.section IS NOT Null  and b.RollNo Is NOT Null " +
                                    " order by b.CLASS_APPLIED ,d.master_name";
                            }

                            else if (sgen.GetSession(MyGuid, "Issue_to").ToString() == "002" )//STAFF
                            {

                                cmd= "SELECT  (a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||b.type) as fstr," +
                                    " Replace(a.first_name,'0','')||' '||Replace(a.MIDDLE_NAME,'0','')||" +
                                    "' '||Replace(a.last_name,'0','') AS Teacher_Name,a.rec_id,nvl(b.master_name,'-') as Designation ," +
                                    "nvl(c.master_name,'-') as Department FROM user_details a " +
                                    "left join master_setting b on a.designation=b.master_id and b.type='MDG'" +
                                    "left join master_setting c on a.department=c.master_id and b.type='MDP'" +
                                 " where a.Type='CPR' and a.client_id='" + clientid_mst + "' and a.client_unit_id='" + unitid_mst + "' and a.status='active' ";

                            }


                            break;

                        case "EDIT":
                        case "VIEW":
                        case "PRINT":
                            cmd = @"select (a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr,
                                       a.vch_num as doc_no,(case when a.deptno=001 then 'Student' Else
                                  'Staff' End)as Issue_To ,nvl(ud.RegNumber, '-') as Reg_Number,c.col40 as Book_Name,
                             nvl((ud.FIRST_NAME) || ' ' || replace(ud.MIDDLE_NAME, '0', '') || ' ' || replace(ud.LAST_NAME, '0', ''), '-') as Student_Name,
                              nvl(Replace(t.first_name, '0', '') || ' ' || Replace(t.MIDDLE_NAME, '0', '') ||
                                     ' ' || Replace(t.last_name, '0', ''), '-') AS Teacher_Name ,a.qtyout
                            from itransaction a inner join vehicle_master c on c.vch_num=a.icode and c.type='LBR' and c.client_unit_id=a.client_unit_id left join user_details ud on a.deptname = ud.regnumber and ud.type = 'STD'
                      left join user_details t on t.type = 'CPR' and lpad(a.deptname,6,0)= lpad(t.rec_id, 6, 0)
                          where a.type = '3A' and a.client_unit_id = '" + unitid_mst+"'  order by a.vch_num desc";

                            break;

                        case "ITEM":

                            if (param1 != "") where = " and (b.client_id||b.client_unit_id||b.vch_num||to_char(b.vch_date,'yyyymmdd')||b.type) not in ('" + param1.Trim().TrimEnd(',').Replace(",", "','") + "')";

                            cmd = "select (b.client_id||b.client_unit_id||b.vch_num||to_char(b.vch_date,'yyyymmdd')||b.type) as fstr,b.col40 as Book_Name," +
                    " b.col1 as Author,b.col7 as edition,b.col8 as year,b.col10 as volume,a.qtystk as Stock_Qty" +
                    " from vehicle_master b inner join (select a.icode,sum(a.qtyin) - sum(qtyout) as qtystk from (select vch_num as icode,sum(col11) " +
                    "as QtyIn ,0 as qtyout from vehicle_master  where type='LBR' and client_unit_id='"+unitid_mst+"' group by vch_num " +
                    "union " +
                    "select icode,0 as qtyin ,sum(qtyout) as qtyout from itransaction  where type = '3A' and client_unit_id = '"+unitid_mst+"'  group by icode " +
                    "union " +
                    "select icode, sum(qtyin) as qtyin ,0 as qtyout from itransaction  where type = '1A' and client_unit_id = '"+unitid_mst+"'  group by icode) " +
                    "a group by icode)a on a.icode=b.vch_num" +
                    " where b.type='LBR' and b.client_id='" + clientid_mst + "' and b.client_unit_id='" + unitid_mst + "' and a.qtystk>0 "+ where + "";

                            break;
                    }

                    break;

                #endregion

                #region book_return

                case "book_return":
                    //if (Session["KISTYPE"] != null) { type = Session["KISTYPE"].ToString().Trim(); }
                    //else { type = "30','32"; }

                    switch (btnval.ToUpper())
                    {
                        case "NEW":

                            cmd = "select '001' as fstr, 'Return from  Student' as Return_From  from dual union select '002' as fstr," +
                                " 'Return from  Staff' as Return_From from dual ";

                            break;

                        case "REG":

                            if (sgen.GetSession(MyGuid, "Return_From").ToString() == "001")//STD
                            {
                                cmd = "select (b.client_id||b.client_unit_id||b.vch_num||to_char(b.vch_date,'yyyymmdd')||b.type) as fstr," +
                                    " e.col37 as Membership_Number, c.Class as Class_Name,  d.master_name as Section, " +
                                     "((a.FIRST_NAME)||' '||replace(a.MIDDLE_NAME,'0','')||' '||replace(a.LAST_NAME,'0','')) as Student_Name," +
                                     "((a.f_firstname)||' '||replace(a.f_middlename,'0','')||' '||replace(a.f_lastname,'0','') ) as Father_Name" +
                                     " ,a.Gender ,a.RegNumber as Reg_Number,b.Roll_number as Roll_Number from student_detail b" +
                                    " inner join user_details a  on a.regnumber=b.reg_no and a.type= b.type and a.rec_id=b.vch_num" +
                                    " inner join add_class c on c.add_class_id=b.class_applied and c.type='EAC' and c.client_unit_id=b.client_unit_id  " +
                                    " inner join master_setting d on  b.section =d.master_id  and d.type='ECS' and d.client_unit_id=b.client_unit_id " +
                                    "inner join vehicle_master e on b.reg_no = e.col25  and e.type='LMS'  and e.client_unit_id=b.client_unit_id" +
                                    " where b.type='STD' and b.client_id='" + clientid_mst + "' and b.client_unit_id='" + unitid_mst + "' " +
                                    " and b.Academic_year_Id='" + Ac_Year_id + "' and b.section IS NOT Null  and b.RollNo Is NOT Null " +
                                    " order by b.CLASS_APPLIED ,d.master_name";
                            }

                            else if (sgen.GetSession(MyGuid, "Return_From").ToString() == "002")//STAFF
                            {

                                cmd = "SELECT  (a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||b.type) as fstr," +
                                    " Replace(a.first_name,'0','')||' '||Replace(a.MIDDLE_NAME,'0','')||" +
                                    "' '||Replace(a.last_name,'0','') AS Teacher_Name,a.rec_id,nvl(b.master_name,'-') as Designation ," +
                                    "nvl(c.master_name,'-') as Department FROM user_details a " +
                                    "left join master_setting b on a.designation=b.master_id and b.type='MDG'" +
                                    "left join master_setting c on a.department=c.master_id and b.type='MDP'" +
                                 " where a.Type='CPR' and a.client_id='" + clientid_mst + "' and a.client_unit_id='" + unitid_mst + "' and a.status='active' ";

                            }


                            break;

                        case "EDIT":
                        case "VIEW":
                        case "PRINT":


                            break;

                        case "ITEM":

                            if (param1 != "") where = " and (a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type) not in ('" + param1.Trim().TrimEnd(',').Replace(",", "','") + "')";
                            //added b.type in place of a.type in fstr
                            cmd = "select (a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||b.type) as fstr ," +
                                "a.vch_num as Issue_NO,to_char(convert_tz(a.vch_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') as Issue_Date" +
                                ", a.qtyout,a.icode ,b.col40 as Book_Name, b.col1 as Author,b.col7 as edition" +
                                ",b.col8 as year,b.col10 as volume , b.qtystk as Stock_qty from itransaction a inner join vehicle_master b on a.icode = b.vch_num" +
                                " and b.type = 'LBR' and a.client_id = b.client_id and a.client_unit_id = b.client_unit_id inner join (select a.icode,sum(a.qtyin) - sum(qtyout) as qtystk from (select vch_num as icode,sum(col11) " +
                    "as QtyIn ,0 as qtyout from vehicle_master  where type='LBR' and client_unit_id='" + unitid_mst + "' group by vch_num " +
                    "union " +
                    "select icode,0 as qtyin ,sum(qtyout) as qtyout from itransaction  where type = '3A' and client_unit_id = '" + unitid_mst + "'  group by icode " +
                    "union " +
                    "select icode, sum(qtyin) as qtyin ,0 as qtyout from itransaction  where type = '1A' and client_unit_id = '" + unitid_mst + "'  group by icode) " +
                    "a group by icode)b on b.icode=a.vch_num where a.type = '3A'" +
                                " and a.client_id = '"+clientid_mst+"' and a.client_unit_id = '"+unitid_mst+"' and " +
                                "a.deptno = '"+ sgen.GetSession(MyGuid, "Return_From").ToString() + "' and a.deptname = '"+ sgen.GetSession(MyGuid, "Ret_Frm").ToString() + "'";

                            break;
                    }

                    break;

                #endregion

                #region Add Journal

                case "add_jurnl":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = "select  (a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr, a.vch_num as doc_num," +
                                "a.col1 as Publisher,a.col2 as Author,a.col4 as Rate,a.col7 as no_of_cpy,a.col8 as vol_no,frq.master_name as Frequency,a.col12 as topic," +
                                "a.col31 as sub_topic,nvl(to_char(convert_tz(a.date1, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "'), 0) as Due_date," +
                                "nvl(to_char(convert_tz(a.date2, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "'), 0) as Start_date," +
                                "nvl(to_char(convert_tz(a.date3, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "'), 0) as End_date " +
                                "from vehicle_master a inner join master_setting" +
                                " frq on frq.master_id = a.col3 and frq.type = 'EFQ' where a.type = 'JOR' and a.client_unit_id = '" + unitid_mst + "' and a.client_id = '" + clientid_mst + "'";
                            break;
                    }
                    break;

                #endregion


                #region Add Book

                case "add_book":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                            //cmd = "select (vm.client_id || vm.client_unit_id || vm.vch_num || to_char(vm.vch_date, 'yyyymmdd') || vm.type) as fstr" +
                            //    ",vm.col40 as Book_Title,vm.col1 as Author, pb.c_name as Publisher_Name,ct.master_name as Book_Category,vm.col7 as Book_Edition," +
                            //    "vm.col9 as Number_Of_Pages,vm.col10 as Volume,vm.col11 as Book_Quantity,vm.col13 as Language,(replace(ud.FIRST_NAME, '0', '') ||" +
                            //    " ' ' || replace(ud.MIDDLE_NAME, '0', '') || ' ' || replace(ud.LAST_NAME, '0', '')) as Ent_by, to_char(vm.ent_Date, 'dd/mm/yyyy') " +
                            //    "as Ent_Date from vehicle_master vm,clients_mst pb, master_setting ct,user_details ud where vm.type = 'LBR' " +
                            //    "and vm.client_id = '" + clientid_mst + "'and vm.client_unit_id = '" + unitid_mst + "' and pb.type = 'PVD'and vm.col2 = pb.vch_num " +
                            //    "and vm.client_id = pb.client_id and vm.client_unit_id = pb.client_unit_id and ct.type = 'BCM'and vm.col3 = ct.master_id " +
                            //    "and vm.client_id = ct.client_id and vm.client_unit_id = ct.client_unit_id and vm.ent_by = ud.rec_id  ";

                            cmd = "select (vm.client_id || vm.client_unit_id || vm.vch_num || to_char(vm.vch_date, 'yyyymmdd') || vm.type) as fstr," +
                                "vm.vch_num as doc_no,vm.col40 as Book_Title,vm.col1 as Author ,vm.col7 as Book_Edition,vm.col9 as Number_Of_Pages," +
                                "bc.master_name as Book_catagory,vm.col10 as Volume,vm.col11 as Book_Quantity,vm.col13 as Language," +
                                "to_char(vm.ent_Date, '" + sgen.Getsqldateformat() + "') as Ent_Date from vehicle_master vm inner join master_setting bc on " +
                                "bc.master_id = vm.col3 and bc.type = 'BCM' and find_in_set(vm.client_id, bc.client_id) = 1 and " +
                                "find_in_set(vm.client_unit_id, bc.client_unit_id)= 1 inner join master_setting lng on lng.master_id = vm.col13 and " +
                                "lng.type = 'KLM' and find_in_set(vm.client_id, bc.client_id) = 1 and find_in_set(vm.client_unit_id, bc.client_unit_id)= 1 " +
                                "where vm.type = 'LBR' and vm.client_id = '001'and vm.client_unit_id = '" + unitid_mst + "'";
                            break;

                        case "PUB":
                            cmd = "select(vm.client_id || vm.client_unit_id || vm.vch_num || to_char(vm.vch_date, 'yyyymmdd') || vm.type) as fstr," +
                                "vm.c_name as PublisherName,vm.cpaddr2 as Address from clients_mst vm " +
                                "WHERE vm.client_id = '" + clientid_mst + "' and vm.client_unit_id = '" + unitid_mst + "' and vm.type = 'PVD'";
                            break;

                        case "REG":

                            mq = "select (bn.client_id||bn.client_unit_id||bn.vch_num||(bn.vch_date,'yyyymmdd')||bn.type) as fstr, bn.master_name as Bin_Name,rc.master_name as Rack_Name" +
                                ",sc.master_name as Section_Name from master_setting bn,master_setting rc,master_setting sc where bn.type='LBM' and rc.type='LRM' and sc.type='LSM' " +
                                "and bn.classid=rc.master_id  and bn.group_id=sc.master_id and bn.client_id='" + clientid_mst + "' and bn.client_unit_id='" + unitid_mst + "' and " +
                                "bn.client_id=rc.client_id and bn.client_unit_id=rc.client_unit_id and bn.client_id=sc.client_id and bn.client_unit_id=sc.client_unit_id";
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
            chkRef();
            
            ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.vsave = "disabled='disabled'";
            ViewBag.scripCall = "disableForm();";
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);

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
            FillMst(model[0].Col15);
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
                if (sgen.GetSession(MyGuid,"btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
                model = CallbackFun(btnval, "N", "party_ass", "Lib", model);
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

        #region add_pub
        public ActionResult add_pub()
        {
            FillMst();
            chkRef();
            List<Tmodelmain> model = new List<Tmodelmain>();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            Tmodelmain tm1 = new Tmodelmain();
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            tm1.Col9 = "ADD PUBLUSHER";
            tm1.Col10 = "master_setting";
            tm1.Col11 = " and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";
            tm1.Col12 = "APM";//Add Publisher
            tm1.Col13 = "Save";
            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = MyGuid.Trim();
            ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.vsave = "disabled='disabled'";
            ViewBag.scripCall = "disableForm();";
            tm1.TList1 = mod1;
            tm1.TList2 = mod1;
            tm1.TList3 = mod1;
            tm1.SelectedItems1 = new string[] { "" };
            tm1.SelectedItems2 = new string[] { "" };
            tm1.SelectedItems3 = new string[] { "" };
            model.Add(tm1);
            return View(model);
        }
        [HttpPost]
        public ActionResult add_pub(List<Tmodelmain> model, string command, string mid)
        {
            FillMst(model[0].Col15);
            var tm = model.FirstOrDefault();
            #region Dropdwon All 
            List<SelectListItem> mod1 = (List<SelectListItem>)TempData[MyGuid + "_Tlist1"];
            List<SelectListItem> mod2 = (List<SelectListItem>)TempData[MyGuid + "_Tlist2"];
            List<SelectListItem> mod3 = (List<SelectListItem>)TempData[MyGuid + "_Tlist3"];
            TempData[MyGuid + "_Tlist1"] = mod1;
            TempData[MyGuid + "_Tlist2"] = mod2;
            TempData[MyGuid + "_Tlist3"] = mod3;
            model[0].TList1 = mod1;
            model[0].TList2 = mod2;
            model[0].TList3 = mod3;
            if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
            if (tm.SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
            if (tm.SelectedItems3 == null) model[0].SelectedItems3 = new string[] { "" };
            #endregion
            if (command == "New")
            {
                try
                {
                    sgen.SetSession(MyGuid,"EDMODE", "N");
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.scripCall = "enableForm();";
                    mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                    vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                    model[0].Col16 = vch_num;
                    mod1 = new List<SelectListItem>();
                    mod2 = new List<SelectListItem>();
                    mod3 = new List<SelectListItem>();
                    #region Country
                    DataTable dt3 = new DataTable();
                    dt3 = sgen.getdata(userCode, "select country_name,alpha_2 from country_state group by country_name,alpha_2 order by country_name,alpha_2");
                    if (dt3.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt3.Rows)
                        {
                            mod1.Add(new SelectListItem { Text = dr["country_name"].ToString(), Value = dr["alpha_2"].ToString() });

                        }
                    }
                    TempData[MyGuid + "_Tlist1"] = mod1;
                    #endregion
                    TempData[MyGuid + "_Tlist2"] = mod2;
                    TempData[MyGuid + "_Tlist3"] = mod3;
                    model[0].TList1 = mod1;
                    model[0].TList2 = mod2;
                    model[0].TList3 = mod3;
                }
                catch (Exception ex) { }
            }
            #region state
            else if (command == "Class")
            {
                try
                {
                    if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
                    if (tm.SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
                    if (tm.SelectedItems3 == null) model[0].SelectedItems3 = new string[] { "" };
                    mod2 = new List<SelectListItem>();
                    DataTable dt2 = new DataTable();
                    dt2 = sgen.getdata(userCode, "select distinct state_name,state_gst_code from country_state where alpha_2='" + model[0].SelectedItems1.FirstOrDefault() + "' and state_name!='-' order by state_name");
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
                catch (Exception ex) { }
            }
            #endregion
            #region city
            else if (command == "Section")
            {
                try
                {
                    if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
                    if (tm.SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
                    if (tm.SelectedItems3 == null) model[0].SelectedItems3 = new string[] { "" };
                    mod3 = new List<SelectListItem>();
                    DataTable dt3 = new DataTable();
                    dt3 = sgen.getdata(userCode, "SELECT city_name FROM (SELECT distinct city_name FROM country_state where state_gst_code='" + model[0].SelectedItems2.FirstOrDefault() + "' ) tab union SELECT 'Other' city_name from dual");
                    if (dt3.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt3.Rows)
                        {
                            mod3.Add(new SelectListItem { Text = dr["city_name"].ToString(), Value = dr["city_name"].ToString() });

                        }
                    }
                    TempData[MyGuid + "_Tlist3"] = mod3;
                    model[0].TList3 = mod3;


                }
                catch (Exception ex) { }
            }
            #endregion
            else if (command == "Save" || command == "Update")
            {
                try
                {
                    string found = "";
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime(userCode);
                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        isedit = true;
                        vch_num = model[0].Col16;
                        string myquery = "select a. master_name from master_setting a  where master_name='" + model[0].Col18 + "' and type='" + model[0].Col12 + "'   " + model[0].Col11 + "";
                        found = sgen.getstring(userCode, myquery);
                    }
                    else
                    {
                        string myquery = "select a.master_name from master_setting a where master_name='" + model[0].Col18 + "' and type='" + model[0].Col12 + "'  " + model[0].Col11 + "";
                        found = sgen.getstring(userCode, myquery);
                        isedit = false;
                        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                        model[0].Col16 = vch_num;

                        mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                        master_id = sgen.genNo(userCode, mq, 3, "auto_genid");
                    }
                    if (found != "")
                    {
                        ViewBag.scripCall = "showmsgJS(1, 'Name Already Exists', 2);";
                        goto End;
                    }
                    DataTable dataTable = sgen.getdata(userCode, "select * from " + model[0].Col10.Trim() + "  where  1=2");
                    DataRow dr = dataTable.NewRow();
                    dr["type"] = model[0].Col12;
                    dr["vch_date"] = currdate;
                    dr["client_id"] = clientid_mst;
                    dr["client_unit_id"] = unitid_mst;
                    dr["vch_num"] = model[0].Col16;
                    dr["client_name"] = model[0].Col17;
                    dr["master_name"] = model[0].Col18;
                    dr["master_id"] = master_id;
                    dr["group_name"] = model[0].SelectedItems1.FirstOrDefault();
                    dr["cont_person_name"] = model[0].SelectedItems2.FirstOrDefault();
                    dr["client_gstin"] = model[0].SelectedItems3.FirstOrDefault();
                    dr["cont_person_email"] = model[0].Col22;
                    dr["cont_person_number"] = model[0].Col20;
                    dr["group_id"] = model[0].Col19;
                    dr["cont_person_altnumber"] = model[0].Col21;
                    if (isedit)
                    {
                        dr["rec_id"] = model[0].Col7;
                        dr["master_id"] = model[0].Col26;
                        dr["client_id"] = model[0].Col1;
                        dr["client_unit_id"] = model[0].Col2;
                        dr["master_editby"] = model[0].Col3;
                        dr["master_editdate"] = model[0].Col4;
                        dr["master_entby"] = userid_mst;
                        dr["master_entdate"] = currdate;
                    }
                    else
                    {
                        dr["rec_id"] = "0";
                        dr["master_editby"] = userid_mst;
                        dr["master_editdate"] = currdate;
                        dr["master_entby"] = userid_mst;
                        dr["master_entdate"] = currdate;
                    }
                    dataTable.Rows.Add(dr);

                    bool Result = sgen.Update_data(userCode, dataTable, model[0].Col10, tmodel.Col8, isedit);
                    if (Result == true)
                    {
                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        ViewBag.scripCall = "showmsgJS(1, 'Record Saved Successfully', 1);disableForm();";
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
                            TList1 = mod1,
                            SelectedItems1 = new string[] { "" },
                            TList2 = mod2,
                            SelectedItems2 = new string[] { "" },
                            TList3 = mod3,
                            SelectedItems3 = new string[] { "" },

                        }
                    );

                    }
                    End:;
                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
            }
            else if (command == "Cancel")
            {
                return RedirectToAction("add_pub", new { @m_id = EncryptDecrypt.Encrypt(model[0].Col15), @mid = EncryptDecrypt.Encrypt(model[0].Col14) });
            }
            else if (command == "Callback")
            {
                if (sgen.GetSession(MyGuid,"btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
                model = CallbackFun(btnval, "N", "add_pub", "Lib", model);
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
            }

            ModelState.Clear();
            return View(model);
        }
        #endregion

        #region SECURITY MEMBERSHIP FEE
        public ActionResult mem_fee()
        {
            List<Tmodelmain> model = new List<Tmodelmain>();
            try
            {
                FillMst();
                chkRef();

                Tmodelmain tmodel = new Tmodelmain();

                mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
                MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
                tmodel.Col15 = MyGuid;
                #region Library Memebership Fee
                DataTable dt1 = sgen.getdata(userCode, "select a.client_id|| a.client_unit_id|| a.master_id|| to_char(a.vch_date,'yyyymmdd')||a.type as fstr, " +
                    "a.master_name,a.IsRegFeesBeforeAdm ,a.master_entby, a.master_entdate,master_id from master_setting a where a.type='LMF' " +
                    " and a.client_id='" + clientid_mst + "' and a.client_unit_id='" + unitid_mst + "' ");

                sgen.SetSession(MyGuid,"EDMODE", "N");
                if (dt1.Rows.Count > 0)
                {
                    sgen.SetSession(MyGuid,"EDMODE", "Y");
                    tmodel.Col16 = dt1.Rows[0]["master_name"].ToString();
                    tmodel.Col5 = dt1.Rows[0]["master_id"].ToString();
                    tmodel.Col8 = "";
                    string ForAll = (dt1.Rows[0]["IsRegFeesBeforeAdm"].ToString());

                    if (ForAll == "0")
                    {
                        tmodel.Col17 = "0";
                    }

                    else
                    {
                        tmodel.Col17 = "1";
                    }


                    tmodel.Col3 = dt1.Rows[0]["master_entby"].ToString();
                    tmodel.Col4 = dt1.Rows[0]["master_entdate"].ToString();
                    tmodel.Col13 = "Update";
                    tmodel.Col8 = "client_id||client_unit_id||master_id|| to_char(vch_date, 'yyyymmdd')|| type = '" + dt1.Rows[0]["fstr"].ToString() + "'";

                }

                else { tmodel.Col13 = "Save"; }
                tmodel.Col14 = mid_mst.Trim();
                tmodel.Col15 = MyGuid.Trim();
                tmodel.Col9 = "LIBRARY SECURITY MEMBERSHIP FEE";
                tmodel.Col10 = "master_setting";
                tmodel.Col12 = "LMF";

                tmodel.Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
                model.Add(tmodel);
                #endregion


            }
            catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "',1)"; }
            return View(model);
        }
        [HttpPost]
        public ActionResult mem_fee(List<Tmodelmain> model, string command)
        {
            try
            {
                FillMst(model[0].Col15);
                var tm = model.FirstOrDefault();
                if (command == "Save" || command == "Update")
                {
                    try
                    {

                        var tmodel = model.FirstOrDefault();
                        string currdate = sgen.server_datetime(userCode);
                        if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                        {
                            isedit = true;
                            vch_num = model[0].Col5;
                        }
                        else
                        {
                            isedit = false;
                            mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10.Trim() + " a " +
                                "where type='" + model[0].Col12.Trim() + "' and client_unit_id='" + unitid_mst + "'";
                            vch_num = sgen.genNo(userCode, mq, 3, "auto_genid");
                        }
                        DataTable dataTable = sgen.getdata(userCode, "select * from " + model[0].Col10.Trim() + "  where  1=2");


                        #region dtstr
                        DataRow dr = dataTable.NewRow();
                        dr["rec_id"] = "0";
                        dr["vch_date"] = currdate;
                        dr["type"] = model[0].Col12;
                        dr["master_id"] = vch_num;
                        dr["master_name"] = model[0].Col16;

                        dr["master_type"] = Master_Type.Trim();
                        dr["IsRegFeesBeforeAdm"] = model[0].Col17;
                        dr["client_id"] = clientid_mst;
                        dr["client_unit_id"] = unitid_mst;
                        #endregion

                        if (isedit)
                        {

                            dr["master_entby"] = model[0].Col3;
                            dr["master_entdate"] = sgen.Savedate(model[0].Col3, false);
                            dr["master_editby"] = userid_mst;
                            dr["master_editdate"] = currdate;
                        }
                        else
                        {
                            dr["master_entby"] = userid_mst;
                            dr["master_entdate"] = currdate;
                            dr["master_editby"] = "-";
                            dr["master_editdate"] = currdate;
                        }
                        dataTable.Rows.Add(dr);
                        bool Result = sgen.Update_data(userCode, dataTable, model[0].Col10, tmodel.Col8, isedit);
                        if (Result == true)
                        {
                            ViewBag.vnew = "";
                            ViewBag.vedit = "";
                            ViewBag.vsave = "disabled='disabled'";
                            ViewBag.scripCall = "showmsgJS(1, 'Record Saved Successfully', 1);disableForm();";
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


                            }
                       );
                        }
                    }
                    catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
                }

            }
            catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "',1)"; }
            ModelState.Clear();
            return View(model);
        }

        #endregion      


        #region book_issued
        public ActionResult book_issued()
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
            tm1.Col9 = "BOOK ISSUANCE";
            tm1.Col10 = "itransaction";
            tm1.Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            tm1.Col12 = "3A";
            tm1.Col13 = "Save";
            tm1.Col100 = "Save & New";
            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = MyGuid.Trim();
            tm1.Col22 = "Student Name";
            tm1.Col24 = "Membership Number";
            tm1.Col26 = "Class Section";

            DataTable dt = sgen.getdata(userCode, "select '-' as SNo ,'-' as Icode,'-' as Book_Name,'-' Author,'-' Volume," +
                " '0' as Available_Books,'0' as Qty_issue,'-' as Expected_Return_Date  from dual");
            tm1.dt1 = dt;
            sgen.SetSession(MyGuid,"dtbase", dt);
            //Session["KISTYPE"] = null;
            sgen.SetSession(MyGuid,"Issue_to", null);
            model.Add(tm1);
            return View(model);
        }
    
        [HttpPost]
        public ActionResult book_issued(List<Tmodelmain> model, string command, string hfrow, string hftable)
        {
            FillMst(model[0].Col15);
            DataTable dtm = sgen.settable(hftable);
            model[0].dt1 = dtm;
            var tm = model.FirstOrDefault();

           

            DataTable dt = new DataTable();
            if (command == "Cancel")
            {
                return RedirectToAction("book_issued", new { @m_id = EncryptDecrypt.Encrypt(model[0].Col15), @mid = EncryptDecrypt.Encrypt(model[0].Col14) });
            }
            else if (command == "Callback")
            {
                if (sgen.GetSession(MyGuid,"btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
                model = CallbackFun(btnval, "N", "book_issued", "Lib", model);
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
                ViewBag.vsavenew = "";
            }
            else if (command.Trim() == "Save" || command.Trim() == "Update" || command == "Save & New" || command == "Update & New")
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
                        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                        model[0].Col18 = vch_num;
                    }

                    DataTable dataTable = sgen.getdata(userCode, "select  * from " + model[0].Col10.Trim() + "  where  1=2");
                    for (int i = 0; i < model[0].dt1.Rows.Count; i++)
                    {
                        DataRow dr = dataTable.NewRow();
                        dr["vch_num"] = vch_num;
                        dr["type"] = model[0].Col12;
                        dr["vch_date"] = sgen.Savedate(model[0].Col20, false);
                        dr["date1"] = sgen.Savedate(model[0].dt1.Rows[i]["Expected_Return_Date"].ToString(), false);
                        dr["deptno"] = model[0].Col29;
                        dr["deptname"] = model[0].Col28;
                        //dr["desig"] = model[0].SelectedItems2.FirstOrDefault();
                        dr["totremark"] = model[0].Col21;
                        dr["icode"] = model[0].dt1.Rows[i]["Icode"].ToString();
                        dr["iname"] = model[0].dt1.Rows[i]["Book_Name"].ToString();
                        dr["qtystk"] = "0";                        
                        dr["qtyout"] = model[0].dt1.Rows[i]["Qty_issue"].ToString();                                        
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
                    //ViewBag.vnew = "";
                    //ViewBag.vedit = "";
                    //ViewBag.vsave = "disabled='disabled'";
                    if (Result == true)
                    {
                        dt = (DataTable)sgen.GetSession(MyGuid, "dtbase");
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col13 = "Save",
                            Col100 = "Save",
                            Col9 = tmodel.Col9,
                            Col10 = tmodel.Col10,
                            Col11 = tmodel.Col11,
                            Col12 = tmodel.Col12,
                            Col14 = tmodel.Col14,
                            Col15 = tmodel.Col15,                           
                            Col22 = tmodel.Col22,                           
                            Col24 = tmodel.Col24,                           
                            Col26 = tmodel.Col26,                           
                            dt1 = dt
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
                                Make_query("book_issued", "Select Item Issued Type", "NEW", "1", "", "");
                                ViewBag.scripCall = "callFoo('View');";

                                ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                            }
                            catch (Exception ex)
                            {
                                ViewBag.scripCall += "mytoast('error', 'toast-top-right', '" + ex.Message.ToString().Trim() + "');";

                            }
                        }
                    }
                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
            }
            else if (command == "-")
            {
                if (model[0].dt1.Rows.Count > 1) model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                else { model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "dtbase"); }
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
                ViewBag.vsavenew = "";
            }

            ModelState.Clear();
            return View(model);
        }

        #endregion                      

        #region book_return
        public ActionResult book_return()
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
            tm1.Col9 = "BOOK RETURN";
            tm1.Col10 = "itransaction";
            tm1.Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            tm1.Col12 = "1A";
            tm1.Col13 = "Save";
            tm1.Col100 = "Save & New";
            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = MyGuid.Trim();
            tm1.Col22 = "Student Name";
            tm1.Col24 = "Membership Number";
            tm1.Col26 = "Class Section";
            tm1.Col100 = "Save & New";
            DataTable dt = sgen.getdata(userCode, "select '-' as SNo ,'-' as Icode,'-' as Book_Name,'-' Author,'-' Volume," +
                " '0' as Available_Books, '-' as Issue_NO, '-' as Issue_Date,'0' as Qty_Issue,'0' as Qty_Return  from dual");
            tm1.dt1 = dt;
            sgen.SetSession(MyGuid,"dtbase", dt);
            //Session["KISTYPE"] = null;
            sgen.SetSession(MyGuid,"Return_From", null);
            model.Add(tm1);
            return View(model);
        }

        [HttpPost]
        public ActionResult book_return(List<Tmodelmain> model, string command, string hfrow, string hftable)
        {
            FillMst(model[0].Col15);
            DataTable dtm = sgen.settable(hftable);
            model[0].dt1 = dtm;
            var tm = model.FirstOrDefault();



            DataTable dt = new DataTable();
            if (command == "Cancel")
            {
                return RedirectToAction("book_return", new { @m_id = EncryptDecrypt.Encrypt(model[0].Col15), @mid = EncryptDecrypt.Encrypt(model[0].Col14) });
            }
            else if (command == "Callback")
            {
                if (sgen.GetSession(MyGuid,"btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
                model = CallbackFun(btnval, "N", "book_return", "Lib", model);
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
                ViewBag.vsavenew = "";
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
                        vch_num = model[0].Col18;
                    }
                    else
                    {
                        isedit = false;
                        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                        model[0].Col18 = vch_num;
                    }

                    DataTable dataTable = sgen.getdata(userCode, "select  * from " + model[0].Col10.Trim() + "  where  1=2");
                    for (int i = 0; i < model[0].dt1.Rows.Count; i++)
                    {
                        DataRow dr = dataTable.NewRow();
                        dr["vch_num"] = vch_num;
                        dr["type"] = model[0].Col12;
                        dr["vch_date"] = sgen.Savedate(model[0].Col20, false);
                        dr["deptno"] = model[0].Col29;
                        dr["deptname"] = model[0].Col28;
                        //dr["desig"] = model[0].SelectedItems2.FirstOrDefault();
                        dr["totremark"] = model[0].Col21;
                        dr["icode"] = model[0].dt1.Rows[i]["Icode"].ToString();
                        dr["iname"] = model[0].dt1.Rows[i]["Book_Name"].ToString();
                        dr["qtystk"] = "0";
                        dr["qtyin"] = model[0].dt1.Rows[i]["Qty_Return"].ToString();
                        dr["pono"] = model[0].dt1.Rows[i]["issue_no"].ToString();//issue no                        
                        dr["podate"] = sgen.Savedate(model[0].dt1.Rows[i]["issue_date"].ToString(), false);//issue date
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
                        if (command.Trim().ToUpper().Equals("SAVE") || command.Trim().ToUpper().Equals("UPDATE"))
                        {
                            ViewBag.vnew = "";
                            ViewBag.vedit = "";
                            ViewBag.vsave = "disabled='disabled'";
                            ViewBag.vsavenew = "disabled='disabled'";
                            //ViewBag.scripCall = "showmsgJS(1, 'Record Saved Successfully', 1);disableForm();";
                            ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                        }
                        else if (command.Trim().ToUpper().Equals("SAVE & NEW") || command.Trim().ToUpper().Equals("UPDATE & NEW"))
                        {
                            try
                            {
                                //model = newbank_ac(model);
                                Make_query("book_return", "Select Book Issued Type", "NEW", "1","","");
                                ViewBag.scripCall = "callFoo('View');";
                                ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";


                            }
                            catch (Exception ex) { }
                        }

                    {
                        dt = (DataTable)sgen.GetSession(MyGuid, "dtbase");
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col13 = "Save",
                            Col100 = "Save & New",
                            Col9 = tmodel.Col9,
                            Col10 = tmodel.Col10,
                            Col11 = tmodel.Col11,
                            Col12 = tmodel.Col12,
                            Col14 = tmodel.Col14,
                            Col15 = tmodel.Col15,
                            Col22 = tmodel.Col22,
                            Col24 = tmodel.Col24,
                            Col26 = tmodel.Col26,
                            dt1 = dt
                        });

                        ViewBag.scripCall = "showmsgJS(1, 'Record Saved Successfully', 1);disableForm();";
                    }
                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
            }
            else if (command == "-")
            {
                if (model[0].dt1.Rows.Count > 1) model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                else { model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "dtbase"); }
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
                ViewBag.vsavenew = "";
            }

            ModelState.Clear();
            return View(model);
        }

        #endregion

        #region Reports
        #region student_ledger
        public ActionResult lib_rpts()
        {
            FillMst();
            chkRef();

            List<Tmodelmain> model = new List<Tmodelmain>();
            Tmodelmain tm1 = new Tmodelmain();

            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);

          
            tm1.Col9 = "LIBRARY REPORTS";
            tm1.Col10 = mid_mst.Trim();
            tm1.Col11 = MyGuid.Trim();
         
            model.Add(tm1);

            return View(model);
        }

        [HttpPost]
        public ActionResult lib_rpts(List<Tmodelmain> model, string command)
        {
            FillMst(model[0].Col15);
            string cl = "", ut = "", monthid = "", dept = "", desig = "";
            DataSet dst = new DataSet();
            DataTable dt = new DataTable();
            DataTable dtm = new DataTable();

            if (command == "Student Book Ledger")
            {
                

                mq = "select ((ud.FIRST_NAME)||' '||replace(ud.MIDDLE_NAME,'0','')||' '||replace(ud.LAST_NAME,'0','')) as Student_Name," +
                                   "((ud.f_firstname)||' '||replace(ud.f_middlename,'0','')||' '||replace(ud.f_lastname,'0','') ) as Father_Name" +
                                   " ,ud.Gender ,ud.RegNumber as Reg_Number, e.col37 as Membership_Number, c.Class as Class_Name,  d.master_name as Section," +
                                   " a.icode, a.deptname,a.Issue_NO,a.Issue_Date,a.return_no,a.return_Date,a.qtyout,a.qtyin,b.col40 as Book_Name," +
                                   " b.col1 as Author,b.col7 as edition,b.col8 as year,b.col10 as volume from " +
                    " ( select  sum(a.qtyout) as qtyout, sum(qtyin) as qtyin,  a.deptname, Issue_NO,Issue_Date,max(return_no) as return_no,max(return_date) as return_date, a.icode, a.client_id, a.client_unit_id  from (select  a.deptname, a.vch_num as Issue_NO, to_char(convert_tz(a.vch_date, 'UTC', '" + sgen.Gettimezone()+"'), '"+sgen.Getsqldateformat()+"') as Issue_Date," +
                    " '-' as return_no, '-' as return_date, a.qtyout, '0' as qtyin, a.icode, a.client_id, a.client_unit_id  from itransaction a " +
                    "  where a.type = '3A' and a.client_unit_id = '"+unitid_mst+"' and a.deptno = '001' " +
                    "union " +
                    "select  a.deptname, a.pono as issue_no, to_char(convert_tz(a.podate, 'UTC', '"+sgen.Gettimezone() + "'), '"+sgen.Getsqldateformat()+"') as Issue_Date," +
                    " a.vch_num as return_no, to_char(convert_tz(a.vch_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as return_Date, '0' as qtyout," +
                    "a.qtyin, a.icode, a.client_id, a.client_unit_id   from itransaction a   where a.type = '1A' and a.client_unit_id = '"+unitid_mst+"'" +
                    "and a.deptno = '001' ) a group by  a.deptname, a.icode, a.client_id, a.client_unit_id,Issue_NO,Issue_Date ) a inner join vehicle_master b on a.icode = b.vch_num  and b.type = 'LBR' " +
                    "and a.client_id = b.client_id and a.client_unit_id = b.client_unit_id inner join user_details ud  on ud.regnumber=a.deptname" +
                    " and ud.type='STD' inner join student_detail sd on ud.regnumber=sd.reg_no and sd.type='STD'  inner join add_class c on c.add_class_id=sd.class_applied" +
                    " and c.type='EAC' and c.client_unit_id=sd.client_unit_id  " +
                                  " inner join master_setting d on  sd.section =d.master_id  and d.type='ECS' and d.client_unit_id=sd.client_unit_id " +
                                  "inner join vehicle_master e on sd.reg_no = e.col25  and e.type='LMS'  and e.client_unit_id=sd.client_unit_id ";

                 dt = sgen.getdata(userCode, mq);

                if (dt.Rows.Count > 0)
                {

                   
                    dt.TableName = "prepcur";
                    dst.Tables.Add(dt);
                    sgen.open_report_byDs_xml(userCode, dst, "std_book_ledger", "Student Book Ledger");
                }

            }

            else if (command == "Book Ledger")
            {
                mq = "select '001' as ref_to,'Student' as issue_to,((ud.FIRST_NAME)||' '||replace(ud.MIDDLE_NAME,'0','')||' '||replace(ud.LAST_NAME,'0','')) as Student_Name," +
                    "((ud.f_firstname)||' '||replace(ud.f_middlename,'0','')||' '||replace(ud.f_lastname,'0','') ) as Father_Name ,ud.Gender ," +
                    "ud.RegNumber as Reg_Number, e.col37 as Membership_Number, c.Class as Class_Name,  d.master_name as Section, a.icode," +
                    "a.Issue_NO,a.Issue_Date,a.return_no,a.return_Date,a.qtyout,a.qtyin ,b.col40 as Book_Name," +
                                   " b.col1 as Author,b.col7 as edition,b.col8 as year,b.col10 as volume from ( select  sum(a.qtyout) as qtyout, sum(qtyin) as qtyin,  a.deptname, Issue_NO,Issue_Date,max(return_no) as return_no,max(return_date) as return_date, a.icode, a.client_id, a.client_unit_id  from (select  a.deptname, a.vch_num as Issue_NO, to_char(convert_tz(a.vch_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Issue_Date," +
                    " '-' as return_no, '-' as return_date, a.qtyout, '0' as qtyin, a.icode, a.client_id, a.client_unit_id  from itransaction a " +
                    "  where a.type = '3A' and a.client_unit_id = '" + unitid_mst + "' and a.deptno = '001' " +
                    "union " +
                    "select  a.deptname, a.pono as issue_no, to_char(convert_tz(a.podate, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Issue_Date," +
                    " a.vch_num as return_no, to_char(convert_tz(a.vch_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as return_Date, '0' as qtyout," +
                    "a.qtyin, a.icode, a.client_id, a.client_unit_id   from itransaction a   where a.type = '1A' and a.client_unit_id = '" + unitid_mst + "'" +
                    "and a.deptno = '001' ) a group by  a.deptname, a.icode, a.client_id, a.client_unit_id,Issue_NO,Issue_Date ) a inner join vehicle_master b on a.icode = b.vch_num  and b.type = 'LBR' and a.client_id = b.client_id and a.client_unit_id = b.client_unit_id" +
                    " inner join user_details ud  on ud.regnumber = a.deptname and ud.type = 'STD' inner join student_detail sd on ud.regnumber = sd.reg_no and " +
                    "sd.type = 'STD'  inner join add_class c on c.add_class_id = sd.class_applied and c.type = 'EAC' and c.client_unit_id = sd.client_unit_id " +
                    "  inner join master_setting d on sd.section = d.master_id  and d.type = 'ECS' and d.client_unit_id = sd.client_unit_id inner join vehicle_master e " +
                    "on sd.reg_no = e.col25  and e.type = 'LMS'  and e.client_unit_id = sd.client_unit_id  ";

                mq1 = "select '002' as ref_to,'Staff' as issue_to , ((ud.FIRST_NAME)||' '||replace(ud.MIDDLE_NAME,'0','')||' '||replace(ud.LAST_NAME,'0','')) as Teacher_Name," +
                    "((ud.f_firstname)||' '||replace(ud.f_middlename,'0','')||' '||replace(ud.f_lastname,'0','') ) as Father_Name ,ud.Gender " +
                    ",to_char(ud.rec_id) as Reg_Number, '-' as Membership_Number,nvl(dg.master_name,'-') as Class_Name,  nvl(dp.master_name,'-') as Section, a.icode" +
                    ",a.Issue_NO,a.Issue_Date,a.return_no,a.return_Date,a.qtyout,a.qtyin ,b.col40 as Book_Name," +
                                   " b.col1 as Author,b.col7 as edition,b.col8 as year,b.col10 as volume from " +
                    " (select  sum(a.qtyout) as qtyout, sum(qtyin) as qtyin,  a.deptname, Issue_NO,Issue_Date,max(return_no) as return_no,max(return_date) as return_date, a.icode, a.client_id, a.client_unit_id from(select a.deptname, a.vch_num as Issue_NO, to_char(convert_tz(a.vch_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Issue_Date," +
                    " '-' as return_no, '-' as return_date, a.qtyout, '0' as qtyin, a.icode, a.client_id, a.client_unit_id  from itransaction a " +
                    "  where a.type = '3A' and a.client_unit_id = '" + unitid_mst + "' and a.deptno = '002' " +
                    "union " +
                    "select  a.deptname, a.pono as issue_no, to_char(convert_tz(a.podate, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Issue_Date," +
                    " a.vch_num as return_no, to_char(convert_tz(a.vch_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as return_Date, '0' as qtyout," +
                    "a.qtyin, a.icode, a.client_id, a.client_unit_id   from itransaction a   where a.type = '1A' and a.client_unit_id = '" + unitid_mst + "'" +
                    "and a.deptno = '002' ) a group by  a.deptname, a.icode, a.client_id, a.client_unit_id,Issue_NO,Issue_Date ) a inner join vehicle_master b on a.icode = b.vch_num  and b.type = 'LBR' and a.client_id = b.client_id and a.client_unit_id = b.client_unit_id " +
                    "inner join user_details ud  on ud.rec_id = a.deptname and ud.type = 'CPR'  left join master_setting dg on ud.designation = dg.master_id " +
                    "and dg.type = 'MDG' left join master_setting dp on ud.department = dp.master_id and dp.type = 'MDP'";

              
                mq = mq + " union all " + mq1;

                dt = sgen.getdata(userCode, mq);

                if (dt.Rows.Count > 0)
                {


                    dt.TableName = "prepcur";
                    dst.Tables.Add(dt);
                    sgen.open_report_byDs_xml(userCode, dst, "book_ledger", "Book Ledger");
                }
            }

            ViewBag.scripCall += "OpenSingle('../../../../../erp/schoolReports_Rpts/Filter_Report_xml.aspx','95%','900px','Reports');";

            ModelState.Clear();
            return View(model);
        }
        #endregion
        #endregion

        #region Add Journal
        //brijesh
        public ActionResult add_jurnl()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            model = getload(model);
            model[0].Col9 = "ADD JOURNAL";
            model[0].Col10 = "vehicle_master"; //TABLE NAME
            model[0].TList1 = mod1;
            model[0].SelectedItems1 = new string[] { "" };
            model[0].Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            model[0].Col12 = "JOR"; //JOURNAL
            model[0].Col13 = "Save";
            model[0].Col100 = "Save & New";
            ViewBag.vsavenew = "disabled='disabled'";
            return View(model);
        }

        public List<Tmodelmain> new_add_jurnl(List<Tmodelmain> model)
        {
            var tm = model.FirstOrDefault();
            model = getnew(model);

            #region Dropdwon
            List<SelectListItem> mod1 = new List<SelectListItem>();
            model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
            TempData[MyGuid + "_TList1"] = model[0].TList1;
            if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
            #endregion

            #region Frequency 1
            DataTable dt = sgen.getdata(userCode, "select master_id,master_name from master_setting where type='FRE' and client_id='0' and client_unit_id='0'");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    mod1.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                }
                TempData[MyGuid + "_Tlist1"] = mod1;
            }
            #endregion

            model[0].TList1 = mod1;

            model[0].Col13 = "Save";
            model[0].Col100 = "Save & New";

            return model;
        }
        [HttpPost]
        public ActionResult add_jurnl(List<Tmodelmain> model, string command)
        {
            FillMst(model[0].Col15);
            var tm = model.FirstOrDefault();

            #region Dropdwon
            List<SelectListItem> mod1 = new List<SelectListItem>();
            model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
            TempData[MyGuid + "_TList1"] = model[0].TList1;
            if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
            #endregion

            if (command == "New")
            {
                model = new_add_jurnl(model);

            }

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

                        //string cond = sgen.seekval(userCode, "select col10 from " + model[0].Col10 + " where type='" + model[0].Col12 + "' and client_id='" + clientid_mst + "'  and " +
                        //            "col10='" + model[0].Col27 + "' and client_unit_id='" + unitid_mst + "'" + mq1 + "", "col10");
                        //if (!cond.Equals("0"))
                        //{
                        //    //Means Already Exits....     
                        //    //ViewBag.scripCall = "showmsgJS(1,'Forward No Already Exists',2)";
                        //    ViewBag.vnew = "disabled='disabled'";
                        //    ViewBag.vedit = "disabled='disabled'";
                        //    ViewBag.scripCall = "showmsgJS(1,'Forward No Already Exists',2)"; return View(model);
                        //    //goto END;
                        //}
                    }

                    DataRow dr = dtstr.NewRow();
                    //dr["rec_id"] = "0";
                    dr["vch_num"] = vch_num;
                    dr["vch_date"] = currdate;
                    dr["type"] = model[0].Col12;
                    dr["col1"] = model[0].Col18; //Publisher
                    dr["col2"] = model[0].Col19; //Author
                    dr["col3"] = model[0].SelectedItems1.FirstOrDefault();  //quantity
                    dr["col4"] = model[0].Col20;  //Rate
                    dr["col7"] = model[0].Col21;   //No. Of Copy
                    dr["col8"] = model[0].Col23;   //Vol No.
                    dr["col12"] = model[0].Col24;  //Topic
                    dr["col31"] = model[0].Col25;  //Sub Topic
                    dr["date1"] = sgen.Savedate(model[0].Col22, false);      //Due Date
                    dr["date2"] = sgen.Savedate(model[0].Col26, false);      //Start Date
                    dr["date3"] = sgen.Savedate(model[0].Col27, false);      //end Date


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
                                //Make_query("book_fwd", "Select Bank", "NEW", "1");
                                //ViewBag.scripCall = "callFoo('View');";
                                model = new_add_jurnl(model);

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
                            Col13 = tm.Col13,
                            Col14 = tm.Col14,
                            Col15 = tm.Col15,
                            Col100 = "Save & New",
                            TList1 = mod1,
                            SelectedItems1 = new string[] { "" },
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

        #region Add Book
        //brijesh
        public ActionResult add_book()
        {

            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            List<SelectListItem> mod1 = new List<SelectListItem>();

            model = getload(model);
            model[0].Col9 = "ADD BOOK DETAILS";
            model[0].Col10 = "vehicle_master"; //TABLE NAME
            model[0].Col39 = "Choose File";
            model[0].Col40 = "Choose File";
            model[0].TList1 = mod1;
            model[0].TList2 = mod1;
            model[0].TList3 = mod1;
            model[0].TList4 = mod1;

            model[0].LTM1 = new List<Tmodelmain>();
            model[0].LTM2 = new List<Tmodelmain>();
            Tmodelmain tmltm1 = new Tmodelmain();
            Tmodelmain tmltm2 = new Tmodelmain();

            tmltm1.Col1 = "1";
            tmltm2.Col1 = "1";
            model[0].LTM1.Add(tmltm1);
            model[0].LTM2.Add(tmltm2);
            model[0].LTM2[0].TList1 = mod1;
            model[0].LTM1[0].TList1 = mod1;

            model[0].LTM2[0].SelectedItems1 = new string[] { "" };
            model[0].LTM1[0].SelectedItems1 = new string[] { "" };

            model[0].SelectedItems1 = new string[] { "" };
            model[0].SelectedItems2 = new string[] { "" };
            model[0].SelectedItems3 = new string[] { "" };
            model[0].SelectedItems4 = new string[] { "" };

            model[0].Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            model[0].Col12 = "LBR";            //library add book
            model[0].Col13 = "Save";
            model[0].Col100 = "Save & New";
            ViewBag.vsavenew = "disabled='disabled'";
            return View(model);
        }

        public List<Tmodelmain> new_add_book(List<Tmodelmain> model)
        {
            var tm = model.FirstOrDefault();
            model = getnew(model);
            model[0].Col13 = "Save";
            model[0].Col100 = "Save & New";
            //string defval="";

            #region Dropdowns
            #region Dropdwon
            List<SelectListItem> mod1 = new List<SelectListItem>();
            model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
            TempData[MyGuid + "_TList1"] = model[0].TList1;

            List<SelectListItem> mod2 = new List<SelectListItem>();
            model[0].TList2 = (List<SelectListItem>)TempData[MyGuid + "_TList2"];
            TempData[MyGuid + "_TList2"] = model[0].TList2;

            List<SelectListItem> mod3 = new List<SelectListItem>();
            model[0].TList3 = (List<SelectListItem>)TempData[MyGuid + "_TList3"];
            TempData[MyGuid + "_TList3"] = model[0].TList3;

            List<SelectListItem> mod6 = new List<SelectListItem>();
            model[0].TList4 = (List<SelectListItem>)TempData[MyGuid + "_TList4"];
            TempData[MyGuid + "_TList4"] = model[0].TList4;

            List<SelectListItem> mod4 = new List<SelectListItem>();
            model[0].LTM2[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_L2TList1"];
            TempData[MyGuid + "_L2TList1"] = model[0].LTM2[0].TList1;

            List<SelectListItem> mod5 = new List<SelectListItem>();
            model[0].LTM1[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_L1TList1"];
            TempData[MyGuid + "_L1TList1"] = model[0].LTM1[0].TList1;

            #endregion

            #region Book Catagory 1
         
            mod1 = cmd_Fun.book_ctgry(userCode, unitid_mst);
            TempData[MyGuid + "_TList1"] = mod1;
            model[0].TList1 = mod1;
            model[0].SelectedItems1 = new string[] { "" };

            #endregion

            #region class 2

            mod2 = cmd_Fun.lib_class(userCode, unitid_mst);
            TempData[MyGuid + "_TList2"] = mod2;
            model[0].TList2 = mod2;
            model[0].SelectedItems2 = new string[] { "" };
            #endregion

            #region subject 3
            mod3 = cmd_Fun.lib_subject(userCode, unitid_mst);
            TempData[MyGuid + "_TList3"] = mod3;
            model[0].TList3 = mod3;
            model[0].SelectedItems3 = new string[] { "" };
            #endregion

            #region language 4
            mod6 = cmd_Fun.lang(userCode, unitid_mst);
            TempData[MyGuid + "_TList4"] = mod6;
            model[0].TList4 = mod6;
            #endregion

            #region issue against ltm2 1
          
            mod4 = cmd_Fun.issue_aginst(userCode, unitid_mst);
            TempData[MyGuid + "_L2TList1"] = mod4;
            model[0].LTM2[0].TList1 = mod4;
            model[0].SelectedItems4 = new string[] { "" };
            #endregion

            #region Section ltm1 1
            defval = "";
            mod5 = cmd_Fun.iloc(userCode, unitid_mst, out defval);
            TempData[MyGuid + "_L1TList1"] = mod5;
            model[0].LTM1[0].TList1 = mod5;
            #endregion
            
           

            if (tm.LTM2[0].SelectedItems1 == null) model[0].LTM2[0].SelectedItems1 = new string[] { "" };

            if (tm.LTM1[0].SelectedItems1 == null) model[0].LTM1[0].SelectedItems1 = new string[] { "" };

           
            #endregion

            return model;
        }
        [HttpPost]
        public ActionResult add_book(List<Tmodelmain> model, string command,string hfrow, HttpPostedFileBase[] upd1, HttpPostedFileBase[] upd2)
        {
            string ctype1 = "", ctype2 = "",finalpath1 = "", finalpath2 = "", path1 = "", path2 = "",
               fileName1 = "", fileName2 = "", encpath1 = "", encpath2 = "";
           FillMst(model[0].Col15);
            var tm = model.FirstOrDefault();
            IList<HttpPostedFileBase> fileCollection1 = new List<HttpPostedFileBase>();
            IList<HttpPostedFileBase> fileCollection2 = new List<HttpPostedFileBase>();

            #region Dropdwon
            List<SelectListItem> mod1 = new List<SelectListItem>();
            model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
            TempData[MyGuid + "_TList1"] = model[0].TList1;
            if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };

            List<SelectListItem> mod2 = new List<SelectListItem>();
            model[0].TList2 = (List<SelectListItem>)TempData[MyGuid + "_TList2"];
            TempData[MyGuid + "_TList2"] = model[0].TList2;
            if (tm.SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };

            List<SelectListItem> mod3 = new List<SelectListItem>();
            model[0].TList3 = (List<SelectListItem>)TempData[MyGuid + "_TList3"];
            TempData[MyGuid + "_TList3"] = model[0].TList3;
            if (tm.SelectedItems3 == null) model[0].SelectedItems3 = new string[] { "" };

            List<SelectListItem> mod6 = new List<SelectListItem>();
            model[0].TList4 = (List<SelectListItem>)TempData[MyGuid + "_TList4"];
            TempData[MyGuid + "_TList4"] = model[0].TList4;
            if (tm.SelectedItems4 == null) model[0].SelectedItems4 = new string[] { "" };

            List<SelectListItem> mod4 = new List<SelectListItem>();
            model[0].LTM2[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_L2TList1"];
            TempData[MyGuid + "_L2TList1"] = model[0].LTM2[0].TList1;

            List<SelectListItem> mod5 = new List<SelectListItem>();
            model[0].LTM1[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_L1TList1"];
            TempData[MyGuid + "_L1TList1"] = model[0].LTM1[0].TList1;
                        #endregion
            mod4 = model[0].LTM2[0].TList1;
            mod5 = model[0].LTM1[0].TList1;

            foreach (var mod in model)
            {
                foreach (var md1 in mod.LTM2)
                {
                    md1.TList1 = mod4;
                }
                foreach (var md2 in mod.LTM1)
                {
                    md2.TList1 = mod5;
                }
            }
            if (tm.LTM2[0].SelectedItems1 == null) model[0].LTM2[0].SelectedItems1 = new string[] { "" };
            if (tm.LTM1[0].SelectedItems1 == null) model[0].LTM1[0].SelectedItems1 = new string[] { "" };

            if (command == "New")
            {
                model = new_add_book(model);

            }

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
                    string[] Ainsc = null; string[] Ainst = null;
                    string insc = "", inst = "", Vinsc = "", Vinst = "";
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime_local(userCode);
                    ent_date = sgen.Savedate(currdate, true);
                    //TempData[MyGuid + "_currdate"] = currdate;
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

                        string cond = sgen.seekval(userCode, "select col25 from " + model[0].Col10 + " where type='" + model[0].Col12 + "' and client_id='" + clientid_mst + "'  and " +
                                    "col24='" + model[0].Col17 + "' and client_unit_id='" + unitid_mst + "'" + mq1 + "", "col25");
                        if (!cond.Equals("0"))
                        {
                            //Means Already Exits....     
                            //ViewBag.scripCall = "showmsgJS(1,'Forward No Already Exists',2)";
                            ViewBag.vnew = "disabled='disabled'";
                            ViewBag.vedit = "disabled='disabled'";
                            ViewBag.scripCall = "showmsgJS(1,'Book Already Exist',2)";
                            return View(model);
                            //goto END;
                        }
                    }


                    #region Validation Logic
                    if (model[0].Col17 == "")
                    {
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.scripCall = "showmsgJS(1,'Please Enter Title',2)"; return View(model);
                    }

                    if (model[0].Col18 == "")
                    {
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.scripCall = "showmsgJS(1,'Please Enter Author 1',2)"; return View(model);
                    }

                    if ((model[0].Col23 == "") || (model[0].Col23 == "0"))
                    {
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.scripCall = "showmsgJS(1,'Please select Publisher',2)"; return View(model);
                    }

                    if ((model[0].SelectedItems1.FirstOrDefault() == "") || (model[0].SelectedItems1.FirstOrDefault() == "0"))
                    {
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.scripCall = "showmsgJS(1,'Please select Book Catagory',2)"; return View(model);

                    }
                    if (model[0].Col24 == "")
                    {
                        ViewBag.scripCall = "showmsgJS(1,'Please Enter Book Edition',2)"; return View(model);
                    }

                    if (model[0].Col25 == "")
                    {
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.scripCall = "showmsgJS(1,'Please Enter Year of Purchase',2)"; return View(model);
                    }

                    if (model[0].Col25.ToString().Length < 4)
                    {
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.scripCall = "showmsgJS(1,'Please Enter Year Of Purchase Of Four Digit',2)"; return View(model);
                    }
                    if (model[0].Col26 == "")

                    {
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.scripCall = "showmsgJS(1,'Please Enter No.of Pages',2)"; return View(model);
                    }

                    if (model[0].Col27 == "")

                    {
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        //sgen.showmsg(1, "Please Enter Volume Of Book", 2);
                        ViewBag.scripCall = "showmsgJS(1,'Please Enter Volume Of Book',2)"; return View(model);

                    }
                    if (model[0].Col28 == "")

                    {
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.scripCall = "showmsgJS(1,'Please Enter Quantity Of Book',2)"; return View(model);
                    }

                    if (model[0].SelectedItems4.FirstOrDefault() == "")

                    {
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.scripCall = "showmsgJS(1,'Please Enter Language Of Book',2)"; return View(model);
                    }

                    if (model[0].Col30 == "")

                    {
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        //sgen.showmsg(1, "Please Enter ISBN No. Of Book", 2);
                        ViewBag.scripCall = "showmsgJS(1,'Please Enter ISBN No. Of Book',2)"; return View(model);

                    }

                    if (model[0].Chk1 == true)
                    {
                        if ((model[0].SelectedItems2.FirstOrDefault() == "") || (model[0].SelectedItems2.FirstOrDefault() == "0"))
                        {
                            ViewBag.vnew = "disabled='disabled'";
                            ViewBag.vedit = "disabled='disabled'";
                            //sgen.showmsg(1, "Please Select Academic Book Class", 2);
                            //ddl_class.Focus();
                            //isvalid = false;
                            //return;
                            ViewBag.scripCall = "showmsgJS(1,'Please Select Academic Book Class',2)"; return View(model);

                        }

                        if ((model[0].SelectedItems3.FirstOrDefault() == "") || (model[0].SelectedItems3.FirstOrDefault() == "0"))
                        {
                            ViewBag.vnew = "disabled='disabled'";
                            ViewBag.vedit = "disabled='disabled'";
                            ViewBag.scripCall = "showmsgJS(1,'Please Select Academic Book Subject',2)"; return View(model);

                        }
                    }

                    if (model[0].Col34 == "")

                    {
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.scripCall = "showmsgJS(1,'Please Select Purchase Date',2)"; return View(model);
                    }


                    if (model[0].Col35 == "")

                    {
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        //sgen.showmsg(1, "Please Enter Source In Purchase Information", 2);
                        ViewBag.scripCall = "showmsgJS(1,'Please Enter Source In Purchase Information',2)"; return View(model);
                    }

                    if (model[0].Col36 == "")

                    {
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        //sgen.showmsg(1, "Please Enter Cost In Purchase Information", 2);
                        ViewBag.scripCall = "showmsgJS(1,'Please Enter Cost In Purchase Information',2)"; return View(model);
                    }

                    if (model[0].Col37 == "")

                    {
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        //sgen.showmsg(1, "Please Enter Bill No. In Purchase Information", 2);
                        ViewBag.scripCall = "showmsgJS(1,'Please Enter Bill No. In Purchase Purchase Information',2)"; return View(model);
                    }

                    if (model[0].Col38 == "")

                    {
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.scripCall = "showmsgJS(1,'Please Enter Bill Date. In Purchase Purchase Information',2)"; return View(model);
                    }
                    #endregion
                    dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    DataRow dr = dtstr.NewRow();
                    //dr["rec_id"] = "0";
                    dr["vch_num"] = vch_num;
                    dr["vch_date"] = currdate;
                    dr["type"] = model[0].Col12;
                    Ainst = inst.TrimEnd(',').Split(',');
                    //dr["vch_date"] = sgen.server_datetime(currdate);
                    dr["col40"] = model[0].Col17;
                    dr["col1"] = model[0].Col18;
                    dr["col32"] = model[0].Col19;
                    dr["col33"] = model[0].Col20;
                    dr["col34"] = model[0].Col21;
                    //dr["footer"] = txt_footer.Text;
                    dr["col2"] = model[0].Col23;
                    dr["col3"] = model[0].SelectedItems1.FirstOrDefault();
                    dr["col7"] = model[0].Col24;
                    dr["col8"] = model[0].Col25;
                    dr["col9"] = model[0].Col26;
                    dr["col10"] = model[0].Col27;
                    dr["col11"] = model[0].Col28;
                    dr["col13"] = model[0].SelectedItems4.FirstOrDefault();
                    dr["col14"] = model[0].Col30;
                    for (int j = 0; j < Ainst.Length; j++)
                    {
                        Vinst = Ainst[j];
                    }
                    dr["col15"] = model[0].SelectedItems2.FirstOrDefault();
                    dr["col37"] = model[0].SelectedItems3.FirstOrDefault();
                    dr["col18"] = model[0].Col31;
                    dr["col19"] = model[0].Col32;
                    dr["col20"] = model[0].Col33;
                    dr["date1"] = sgen.Savedate(model[0].Col34, false);
                    dr["col28"] = model[0].Col35;
                    dr["col29"] = model[0].Col41;
                    dr["col30"] = model[0].Col42;
                    dr["date2"] = sgen.Savedate(model[0].Col43, false);
                    dr["col38"] = Vinst;
                    //dr["col39"] = txt_maxstd.Text;
                    //dr["col21"] = txt_maxtea.Text;
                    //dr["rec_id"] = "0";
                    dr["client_id"] = clientid_mst;
                    dr["client_unit_id"] = unitid_mst;

                    dr = getsave(currdate, dr, model, isedit);

                    dtstr.Rows.Add(dr);

                    bool Result = sgen.Update_data(userCode, dtstr, model[0].Col10.Trim(), tmodel.Col8, isedit);
                    if (Result == true)
                    {

                        #region Issue Against LTM2

                        //currdate = TempData[MyGuid + "_currdate"].ToString();

                        DataTable dataTable = sgen.getdata(userCode, "select  * from enx_tab  where  1=2");

                        //vch_num = sgen.genNo(userCode, "select max(CONVERT (ifnull(vch_num,'0'), integer)) as max from enx_tab" +
                        //    " where type='LBA' client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "' ", 6, "max");
                       for (int k = 0; k < model[0].LTM2.Count; k++)
                        {
                            dr = dataTable.NewRow();
                            dr["vch_num"] = model[0].Col16.Trim();
                            dr["vch_date"] = currdate;
                            dr["type"] = "LBA";
                            dr["type_desc"] = "Library Book Against";
                            dr["client_id"] = clientid_mst;
                            dr["client_unit_id"] = unitid_mst;
                            dr["col1"] = model[0].LTM2[k].SelectedItems1;//
                            dr["col2"] = model[0].LTM2[k].Col10;//
                            dr["col3"] = model[0].LTM2[k].Col11;//               
                            dr["ref_code"] = model[0].Col16;
                            dr["ref_dt"] = currdate;
                            //dr["col4"] = bookid;//
                            dr = getsave(currdate, dr, model, isedit);
                            dataTable.Rows.Add(dr);
                        }
                        res = sgen.Update_data(userCode, dataTable, "enx_tab", tmodel.Col85, isedit);
                        //Session["MAKEDATATDT_BOOK"] = dataTable;
                        #endregion

                        #region Location LTM1
                        try
                        {
                            DataTable dataTable2 = sgen.getdata(userCode, "select  * from enx_tab  where  1=2");
                            //string currdate = ViewState["currdate"].ToString();
                            //vch_num = sgen.genNo(userCode, "select max(CONVERT (ifnull(vch_num,'0'), integer)) as max from enx_tab where type='LBP' client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "' ", 6, "max");
                            for (int k = 0; k < model[0].LTM1.Count; k++)
                            {
                                dr = dataTable2.NewRow();

                                dr["rec_id"] = "0";
                                dr["vch_num"] = vch_num;
                                dr["vch_date"] = currdate;
                                dr["type"] = "LBP";
                                dr["type_desc"] = "Lib Book Postions";
                                dr["client_id"] = clientid_mst;
                                dr["client_unit_id"] = unitid_mst;
                                dr["col1"] = model[0].LTM1[k].SelectedItems1.FirstOrDefault();
                                //dr["col2"] = ddl_rack.Value;
                                //dr["col3"] = ddl_bin.Value;
                                dr["col4"] = model[0].LTM1[k].Col10;
                                //dr["ref_code"] = Txt_VchNo.Text;
                                dr["ref_dt"] = currdate;
                                dr = getsave(currdate, dr, model, isedit);

                                dataTable2.Rows.Add(dr);
                            }
                            res = sgen.Update_data(userCode, dataTable2, "enx_tab", tmodel.Col86, isedit);

                            //Session["MAKEDATATDT"] = dataTable;
                        }
                        catch (Exception ex)
                        {
                            sgen.showmsg(1, ex.Message.ToString(), 0);
                        }
                        #endregion

                        #region fileupload

                        DataTable dtfile = new DataTable();
                        //dtfile = sgen.getdata(userCode, "select * from file_tab WHERE 1=2");
                        dtfile = cmd_Fun.GetStructure(userCode, "file_tab");
                        string serverpath = Server.MapPath("~/Uploads/" + cg_com_name.Replace(" ", "") + "/");


                        try
                        {
                            HttpPostedFileBase file1 = upd1.FirstOrDefault();
                            ctype1 = file1.ContentType;
                            fileName1 = file1.FileName;
                            path1 = clientid_mst + unitid_mst + userid_mst + currdate + "_" + fileName1;
                            encpath1 = sgen.Convert_Stringto64(path1).ToString();
                            finalpath1 = serverpath + encpath1;
                            file1.SaveAs(finalpath1);
                            filesave(model, currdate, dtfile, fileName1, encpath1, "Book_front", ctype1);
                        }
                        catch (Exception ex) { }

                        try
                        {
                            HttpPostedFileBase file2 = upd2.FirstOrDefault();
                            ctype2 = file2.ContentType;
                            fileName2 = file2.FileName;
                            path2 = clientid_mst + unitid_mst + userid_mst + currdate + "_" + fileName2;
                            encpath2 = sgen.Convert_Stringto64(path2).ToString();
                            finalpath2 = serverpath + encpath2;
                            file2.SaveAs(finalpath2);
                            filesave(model, currdate, dtfile, fileName2, encpath2, "Book_back", ctype2);
                        }
                        catch (Exception ex) { }
                        #endregion


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
                                //Make_query("book_fwd", "Select Bank", "NEW", "1");
                                //ViewBag.scripCall = "callFoo('View');";
                                model = new_add_jurnl(model);

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
                            Col13 = tm.Col13,
                            Col14 = tm.Col14,
                            Col15 = tm.Col15,
                            Col100 = "Save & New",
                            TList1 = mod1,
                            TList2 = mod2,
                            TList3 = mod3,
                            TList4 = mod6,
                            SelectedItems1 = new string[] { "" },
                            SelectedItems2 = new string[] { "" },
                            SelectedItems3 = new string[] { "" },
                            SelectedItems4 = new string[] { "" },
                            LTM1 = tm.LTM1,
                            LTM2 = tm.LTM2,
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


            else if (command == "Add")
            {
                try
                {
                    mod4 = (List<SelectListItem>)TempData[MyGuid + "_L2TList1"];
                    TempData[MyGuid + "_L2TList1"] = mod4;
                    Tmodelmain ntm1 = new Tmodelmain();

                    ntm1.Col1 = (Convert.ToInt32(model[0].LTM2.Max(x => sgen.Make_int(x.Col1))) + 1).ToString();
                    ntm1.TList1 = mod4;
                    ntm1.SelectedItems1 = new string[] { "" };
                    //ntm1.Col40 = "Choose File";
                    model[0].LTM2.Add(ntm1);

                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vsavenew = "";


                    //Tmodelmain ntm1 = new Tmodelmain();

                    //ntm1.Col1 = (Convert.ToInt32(model[0].LTM2.Max(x => sgen.Make_int(x.Col1))) + 1).ToString();
                    ////ntm1.TList3 = mod3;
                    ////ntm1.TList4 = mod4;
                    ////ntm1.TList5 = mod5;
                    //ntm1.SelectedItems2 = new string[] { "" };
                    //ntm1.SelectedItems1 = new string[] { "" };
                    //ntm1.SelectedItems3 = new string[] { "" };
                    //ntm1.LTM2[0].SelectedItems1 = new string[] { "" };
                    //ntm1.LTM1[0].SelectedItems1 = new string[] { "" };
                    //model[0].LTM2.Add(ntm1);

                    //ViewBag.vnew = "disabled='disabled'";
                    //ViewBag.vedit = "disabled='disabled'";
                    //ViewBag.vsave = "";
                    //ViewBag.vsavenew = "";

                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "',0);"; }
            }

            else if (command == "+")
            {
                try
                {
                    mod5 = (List<SelectListItem>)TempData[MyGuid + "_L1TList1"];
                    TempData[MyGuid + "_L1TList1"] = mod5;
                    Tmodelmain ntm1 = new Tmodelmain();

                    ntm1.Col1 = (Convert.ToInt32(model[0].LTM2.Max(x => sgen.Make_int(x.Col1))) + 1).ToString();
                    ntm1.TList1 = mod5;
                    ntm1.SelectedItems1 = new string[] { "" };
                    //ntm1.Col40 = "Choose File";
                    model[0].LTM1.Add(ntm1);
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

            else if (command == "x")
            {
                if (model[0].LTM1.Count > 1) model[0].LTM1.RemoveAt(sgen.Make_int(hfrow));
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


        private void filesave(List<Tmodelmain> model, string currdate, DataTable dtfile, string filename, string filepath, string filetitle, string content_type)
        {
            FillMst(model[0].Col16);
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

        [HttpGet]
        public FileResult fdown(string value)
        {
            string path = "", fileName = "";
            FillMst("");
            if (!value.Trim().Equals(""))
            {
                DataTable dt2 = new DataTable();
                mq = "select file_name,file_path from file_tab where rec_id='" + value.Trim() + "' and type='PVD' and client_id='"
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
        public void fdelete(string value)
        {
            sgen.SetSession(MyGuid,"delid", value);
        }
        #endregion

        //private void BookId()
        //{
        //    #region  Number Generation
        //    tab_name = TempData[MyGuid + "_Tablename"].ToString();
        //    String pattern_type = "BAN";
        //    DataTable DtReceipt = sgen.getdata(userCode, @"select * from  (SELECT 'BookCategory' as Content, Print_Sch_Name as PrintField,Req_Sch_Name as ReqField,Sep_Sch_Name as sepField,Position_Sch_Name as PositionField
        //                     FROM Receipt_Number WHERE client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "' and  Type='" + pattern_type + "' union" +
        //                         "  SELECT 'Bk_cat_vch' as Content, Print_Branch_Name as PrintField,Req__Branch_Name as ReqField,Sep_Branch_Name as sepField,Position_Branch_Name as PositionField " +
        //                         "FROM Receipt_Number WHERE client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "' and  Type='" + pattern_type + "'" +
        //                         " UNION SELECT 'BranchCode' as Content, Print_Branch_Code as PrintField,Req_Branch_Code as ReqField,Sep_Branch_Code as sepField,Position_Branch_Code as PositionField " +
        //                         "FROM Receipt_Number WHERE client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "' and  Type='" + pattern_type + "' UNION" +
        //                         " SELECT 'Year' as Content, Print_Year as PrintField,Req_Year as ReqField,Sep_Year as sepField,Position_Year as PositionField" +
        //                         " FROM Receipt_Number WHERE client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "' and  Type='" + pattern_type + "'" + " UNION" +
        //                         " SELECT 'Month' as Content, Print_Month as PrintField,Req_Month as ReqField,Sep_Month as sepField,Position_Month as PositionField " +
        //                         "FROM Receipt_Number WHERE client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "' and   Type='" + pattern_type + "'" + " UNION " +
        //                         "SELECT 'Date' as Content, Print_Date as PrintField,Req_Date as ReqField,Sep_Date as sepField,Position_Date as PositionField" +
        //                         " FROM Receipt_Number  WHERE client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "' and  Type='" + pattern_type + "'UNION" +
        //                         " SELECT 'Country' as Content, Print_Country as PrintField,Req_Country as ReqField,Sep_Country as sepField,Position_Country as PositionField" +
        //                         " FROM Receipt_Number WHERE client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "' and  Type='" + pattern_type + "' UNION" +
        //                         " SELECT 'State' as Content, Print_State as PrintField,Req_State as ReqField,Sep_State as sepField,Position_State as PositionField" +
        //                         " FROM Receipt_Number WHERE client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "' and  Type='" + pattern_type + "' UNION" +
        //                         " SELECT 'City' as Content, Print_City as PrintField,Req_City as ReqField,Sep_City as sepField,Position_City as PositionField " +
        //                         "FROM Receipt_Number WHERE client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "' and  Type='" + pattern_type + "'" +
        //                         " UNION SELECT 'StartNo' as Content, Print_RecId as PrintField,Req_RecId as ReqField,Sep_RecId as sepField,Position_RecId as PositionField" +
        //                         " FROM Receipt_Number WHERE client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "' and  Type='" + pattern_type + "'" +
        //                         " UNION SELECT 'Year_Pur' as Content, Print_DOB as PrintField,Req_DOB as ReqField,Sep_DOB as sepField,Position_DOB as PositionField" +
        //                         " FROM Receipt_Number WHERE client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "' and  Type='" + pattern_type + "'" +
        //                         " UNION SELECT 'Publisher' as Content, Print_Name as PrintField,Req_Name as ReqField,Sep_Name as sepField,Position_Name as PositionField" +
        //                         " FROM Receipt_Number WHERE client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "' and  Type='" + pattern_type + "'" +
        //                         " )a" +
        //                         "  where PositionField!=0 order by  PositionField");

        //    if (DtReceipt.Rows.Count == 0)
        //    {
        //        sgen.showmsg(1, "Please Set Accession No. Pattern in Library Master", 2);
        //        return;

        //    }
        //    if (DtReceipt.Rows.Count > 0)
        //    {

        //        string xnumber = "";
        //        string xsep = "";
        //        string xlastnumber = "";

        //        for (int i = 0; i < DtReceipt.Rows.Count; i++)
        //        {
        //            xnumber = xlastnumber;
        //            xnumber = DtReceipt.Rows[i]["PrintField"].ToString();
        //            xsep = DtReceipt.Rows[i]["sepField"].ToString();
        //            if (xsep == "None")
        //            {
        //                xsep = "";
        //            }

        //            if (DtReceipt.Rows[i]["Content"].ToString() == "StartNo")
        //            {
        //                {
        //                    string id = "";
        //                    DataTable dt = sgen.getdata(userCode, "SELECT  Print_Min as PrintField FROM Receipt_Number WHERE Type='" + pattern_type + "' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
        //                    Int32 MinDigit = 1;
        //                    try
        //                    {
        //                        MinDigit = Convert.ToInt32(dt.Rows[0][0].ToString());
        //                    }
        //                    catch { MinDigit = 1; }

        //                    if (dt.Rows.Count > 0)
        //                    {

        //                        DataTable dtrc = new DataTable();
        //                        DataTable dtcheck = sgen.getdata(userCode, @"SELECT distinct col12 FROM " + tab_name + " WHERE type='" + type + "' col12='" + Ac_Year_id + "' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
        //                        if (dtcheck.Rows.Count > 0)
        //                        {
        //                            TempData[MyGuid + "_NewNumber"] = false;

        //                        }

        //                        else
        //                        {
        //                            TempData[MyGuid + "_NewNumber"] = true;

        //                        }

        //                        if (Convert.ToBoolean(TempData[MyGuid + "_NewNumber"].ToString()) == false)
        //                        {
        //                            dtrc = sgen.getdata(userCode, @"Select ifnull(max(col13),0)+1 from " + tab_name + " where type='" + type + "' and col12='" + Ac_Year_id + "' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
        //                            if (dtrc.Rows.Count > 0)
        //                            {
        //                                TempData[MyGuid + "_Receipt_Number"] = dtrc.Rows[0][0].ToString();

        //                                if ((TempData[MyGuid + "_Receipt_Number"].ToString() == null) || (TempData[MyGuid + "_Receipt_Number"].ToString() == ""))
        //                                {
        //                                    TempData[MyGuid + "_Receipt_Number"] = 1;
        //                                }
        //                            }
        //                        }

        //                        else
        //                        {
        //                            id = sgen.genNo(userCode, "Select ifnull( max(Print_StartNO),0) as No from Receipt_Number where Type='" + pattern_type + "' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'", MinDigit, "No");

        //                            dtrc = sgen.getdata(userCode, "Select Print_StartNO from Receipt_Number where Type='" + pattern_type + "' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");

        //                            TempData[MyGuid + "_Receipt_Number"] = dtrc.Rows[0][0].ToString();
        //                        }




        //                        xnumber = xlastnumber + id + xsep;

        //                    }

        //                }
        //            }

        //            else if (DtReceipt.Rows[i]["Content"].ToString() == "Year")
        //            {
        //                string xyear = DtReceipt.Rows[i]["PrintField"].ToString();
        //                string displayyr = "";

        //                if (xyear == "yyyy yyyy")
        //                {
        //                    displayyr = Ac_Year;
        //                }
        //                else if (xyear == "yyyy")
        //                {
        //                    displayyr = Ac_Year.Substring(0, 4);
        //                }
        //                else if (xyear == "yy yy")
        //                {

        //                    displayyr = Ac_Year.Substring(2, 2) + ' ' + Ac_Year.Substring(7, 2);


        //                }

        //                else if (xyear == "yy")
        //                {
        //                    displayyr = Ac_Year.Substring(2, 2);
        //                }

        //                xnumber = xlastnumber + displayyr + xsep;


        //            }

        //            else if (DtReceipt.Rows[i]["Content"].ToString() == "Publisher")
        //            {
        //                if ((model[0].Col23 != "") || (ddl_publisher.Value != "0"))
        //                {
        //                    //xnumber = xlastnumber + txt_regnum.Text + xsep;

        //                    xnumber = xlastnumber + ddl_publisher.Items[ddl_publisher.SelectedIndex].Text.Substring(0, sgen.Make_int(xnumber)) + xsep;


        //                }

        //                else
        //                {

        //                    sgen.showmsg(1, "Please Select Publisher Detail", 2);
        //                    ddl_publisher.Focus();

        //                }
        //            }

        //            else if (DtReceipt.Rows[i]["Content"].ToString() == "Year_Pur")
        //            {
        //                if ((txt_year.Text != ""))
        //                {
        //                    //xnumber = xlastnumber + txt_regnum.Text + xsep;
        //                    xnumber = xlastnumber + txt_year.Text.Substring(0, sgen.Make_int(xnumber)) + xsep;

        //                }

        //                else
        //                {

        //                    sgen.showmsg(1, "Please Enter Year Of Purchase", 2);
        //                    txt_year.Focus();

        //                }
        //            }

        //            else if (DtReceipt.Rows[i]["Content"].ToString() == "BookCategory")
        //            {
        //                if ((ddl_book_category.Value != "") || (ddl_book_category.Value != "0"))
        //                {
        //                    //xnumber = xlastnumber + txt_regnum.Text + xsep;
        //                    string x = ddl_book_category.Items[0].Text;
        //                    xnumber = xlastnumber + ddl_book_category.Items[0].Text.Substring(0, sgen.Make_int(xnumber)) + xsep;

        //                }

        //                else
        //                {

        //                    sgen.showmsg(1, "Please Select Book Category Detail", 2);
        //                    ddl_book_category.Focus();

        //                }
        //            }

        //            else if (DtReceipt.Rows[i]["Content"].ToString() == "Bk_cat_vch")
        //            {
        //                if ((ddl_book_category.Value != "") || (ddl_book_category.Value != "0"))
        //                {
        //                    //xnumber = xlastnumber + txt_regnum.Text + xsep;
        //                    string bk_cat_vch = sgen.getstring(UserCode, "select vch_num from master_setting where type='BCM' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "' and master_id='" + ddl_book_category.Value + "'");

        //                    xnumber = xlastnumber + bk_cat_vch + xsep;

        //                }

        //                else
        //                {

        //                    sgen.showmsg(1, "Please Select Book Category Detail", 2);
        //                    ddl_book_category.Focus();

        //                }


        //            }


        //            else
        //            {
        //                xnumber = xlastnumber + xnumber + xsep;
        //            }
        //            xlastnumber = xnumber;

        //        }
        //        xnumber = xnumber.Substring(0, xnumber.Length - 1);
        //        //Txt_mem_no.Text = xnumber;
        //        bookid = xnumber;
        //    }

        //    #endregion
        //}

    }
}