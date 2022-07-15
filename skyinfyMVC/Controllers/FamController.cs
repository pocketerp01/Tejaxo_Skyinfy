using skyinfyMVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace skyinfyMVC.Controllers
{
    public class FamController : Controller
    {
        System.Globalization.DateTimeFormatInfo monthName = new System.Globalization.DateTimeFormatInfo();
        string mq = "", mq1 = "", vch_num = "", btnval = "", tab_name = "", type = "", type_desc = "", tab_name1 = "", ent_date = "", status = "";
        string Master_Type = "", where = "", id = "", Condition = "", isprmoted = "", mid_mst = "", MyGuid = "", master_id;



        Random random = new Random();
        public static string name = "", ctype1 = "", ctype2 = "", ctype3 = "", ctype4 = "", ctype5 = "", ctype6 = "", ctype7 = "",
        finalpath1 = "", finalpath2 = "", finalpath3 = "", finalpath4 = "", finalpath5 = "", finalpath6 = "", finalpath7 = "", path1 = "", path2 = "", path3 = "", path4 = "",
        path5 = "", path6 = "", path7 = "", fileName1 = "", fileName2 = "", fileName3 = "", fileName4 = "", fileName5 = "", fileName6 = "", fileName7 = "", encpath1 = "",
        encpath2 = "", encpath3 = "", encpath4 = "", encpath5 = "", encpath6 = "", encpath7 = "";
        public static string isscholarship = "N", istransfer = "N", issibling = "N", isfamily = "N", ishobby = "N", havetransport = "",
            isperadd = "N", isdisable = "N", sameas = "", gender = "", isemailauth = "", isphoneauth = "", sa_id = "", mq2 = "", mq3 = "";


        public static string userCode = "", userid_mst = "", FN_From_Date = "", FN_To_Date = "", Ac_Year_id = "", cg_com_name = "", clientid_mst = "", unitid_mst = "", Ac_To_Date = "", Ac_From_Date = "", fromclass = "", Txt_ProPer = "", hfrow = "", found = "", actionName = "", controllerName = "";


        bool res = false, Isvalid = false;
        public static sgenFun sgen;
        public static bool isedit = false;
        public static int cli = 0;

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
            //Ac_Year_id = sgen.GetCookie(MyGuid, "Ac_Year_id");
            cg_com_name = sgen.GetCookie(MyGuid, "cg_com_name");
            clientid_mst = sgen.GetCookie(MyGuid, "clientid_mst");
            unitid_mst = sgen.GetCookie(MyGuid, "unitid_mst");
            //Ac_To_Date = sgen.GetCookie(MyGuid, "Ac_To_Date");
            //Ac_From_Date = sgen.GetCookie(MyGuid, "Ac_From_Date");
            FN_From_Date = sgen.GetCookie(MyGuid, "year_from");
            FN_To_Date = sgen.GetCookie(MyGuid, "year_to");
            actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            sgen.SetSession(MyGuid, "viewName", actionName);
            sgen.SetSession(MyGuid, "controllerName", controllerName);
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
                //drf["vch_date"] = currdate;
                //drf["file_name"] = filename;
                //drf["file_path"] = filepath;
                //drf["col2"] = content_type;
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

        //===============getload==========

        #region get

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
            tm1.Col100 = "Save & New";
            tm1.Col121 = "S";
            tm1.Col122 = "<u>S</u>ave";
            tm1.Col123 = "Save & Ne<u>w</u>";
            ViewBag.scripCall = "disableForm();";
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
            if (sgen.GetSession(MyGuid, "delid") != null) { btnval = "FDEL"; }
            else if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
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
            //if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
            //{
            //    edit = true;
            //    vch_num = model[0].Col16.Trim();
            //}
            //else
            //{
            //    edit = false;
            //    mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
            //    vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
            //}

            //drn["vch_num"] = vch_num;
            drn["rec_id"] = "0";

            if (model[0].Col10.Trim() == "master_setting")
            {
                if (edit)
                {
                    //drn["rec_id"] = model[0].Col7;
                    drn["master_entby"] = model[0].Col3;
                    drn["master_entdate"] = model[0].Col4;
                    drn["master_editby"] = userid_mst;
                    drn["master_editdate"] = curdt;
                    drn["client_id"] = clientid_mst;
                    drn["client_unit_id"] = unitid_mst;
                }
                else
                {

                    drn["master_entby"] = userid_mst;
                    drn["master_entdate"] = curdt;
                    drn["master_editby"] = "-";
                    drn["master_editdate"] = curdt;
                    drn["client_id"] = clientid_mst;
                    drn["client_unit_id"] = unitid_mst;
                }

            }
            else
            {
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

        #endregion
        //======================================Call back ===========================================================
        #region Call back 
        public List<Tmodelmain> CallbackFun(string btnval, string edmode, string viewName, string controllerName, List<Tmodelmain> model)
        {
            string mq = "";
            FillMst();
            DataTable dt = new DataTable();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<SelectListItem> mod2 = new List<SelectListItem>();
            List<SelectListItem> mod3 = new List<SelectListItem>();
            List<SelectListItem> mod4 = new List<SelectListItem>();
            List<SelectListItem> mod5 = new List<SelectListItem>();
            List<SelectListItem> mod6 = new List<SelectListItem>();

            var tm = model.FirstOrDefault();

            String URL = sgen.GetSession(MyGuid, "SSEEKVAL").ToString();
            btnval = btnval.ToUpper();
            switch (viewName.ToLower())
            {
                #region group_mst
                case "group_mst":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select rec_id,col1,col2,col3,col4,col5,col6,col7,col8,r_no,ref_code,type,ent_by,ent_date,client_id,client_unit_id,vch_num,type_desc from " + model[0].Col10 + " where type='" + model[0].Col12 + "' and" +
                                " (client_id|| client_unit_id||vch_num||to_char(vch_date, 'yyyymmdd'),type)='" + URL + "'";
                            DataTable dtt = new DataTable();
                            dtt = sgen.getdata(userCode, mq);


                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");


                                model[0].Col8 = "(client_id||client_unit_id|| vch_num|| to_char(vch_date, 'yyyymmdd'), type) = '" + URL + "'";

                                model[0].Col13 = "Update";
                                model[0].Col100 = "Update & New";
                            }
                            else
                            {
                                model = new List<Tmodelmain>();
                                tm.Col17 = "";
                                tm.Col18 = "";
                                tm.Col19 = "";
                                tm.Col20 = "";
                                model.Add(tm);

                            }
                            break;
                    }
                    break;



                #endregion             

                #region fam_master
                case "fam_master":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select client_name ,master_id, a.vch_num, a.master_name,a.classid,sectionid,subjectid,a.master_entby as ent_by,a.master_entdate as ent_date,vch_date,rec_id,client_id,status,client_unit_id from " + model[0].Col10 + " a " +
                                                  "where a.client_id||a.client_unit_id||a.vch_num|| to_char(a.vch_date, 'yyyymmdd')|| a.type ='" + URL + "' ";
                            DataTable dtt = new DataTable();
                            dtt = sgen.getdata(userCode, mq);

                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");

                                #region Building dropdown
                                DataTable dt1 = new DataTable();
                                mq1 = "SELECT master_id,master_name FROM master_setting where type='HBM' " + model[0].Col11 + "";

                                dt1 = sgen.getdata(userCode, mq1);
                                if (dt1.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dt1.Rows)
                                    {
                                        mod1.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                                    }
                                    TempData[MyGuid + "_Tlist1"] = mod1;
                                }
                                #endregion

                                #region Block
                                mq1 = "SELECT DISTINCT vch_num,col4 FROM enx_tab where type='HBM'" + model[0].Col11 + "";

                                dt = sgen.getdata(userCode, mq1);
                                if (dt.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        mod2.Add(new SelectListItem { Text = dr["col4"].ToString(), Value = dr["vch_num"].ToString() });
                                    }

                                    TempData[MyGuid + "_Tlist2"] = mod2;
                                }
                                #endregion

                                model[0].TList1 = mod1;
                                model[0].TList2 = mod2;
                                model[0].Col8 = "client_id|| client_unit_id||vch_num||to_char(vch_date, 'yyyymmdd')||type  = '" + URL + "'";
                                model[0].Col16 = dtt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dtt.Rows[0]["subjectid"].ToString();
                                model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                                model[0].Col18 = dtt.Rows[0]["master_id"].ToString();
                                model[0].Col19 = dtt.Rows[0]["status"].ToString();
                                model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                                model[0].Col7 = dtt.Rows[0]["rec_id"].ToString();
                                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();

                                String[] L1 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["classid"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems1 = L1;
                                String[] L2 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["sectionid"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems2 = L2;
                                model[0].Col13 = "Update";
                                model[0].Col100 = "Update & New";
                            }
                            else
                            {
                                model = new List<Tmodelmain>();
                                tm.Col16 = "";
                                tm.Col17 = "";
                                tm.Col18 = "";
                                tm.Col19 = "";
                                tm.Col20 = "";
                                model.Add(tm);
                            }
                            break;
                    }
                    break;



                #endregion

                #region asset_ent
                case "asset_ent":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select a.c_name as Name,vm.rec_id,vm.col12,vm.col30,vm.col3,vm.col5,vm.col33,vm.col29,vm.col42,vm.col40,vm.col18,vm.col6,vm.col15,vm.col37,vm.ent_by,vm.ent_date," +
                                "vm.client_unit_id,vm.client_id,vm.vch_num,vm.col28 as Capax_amt,vm.col13," +
                                " vm.col36 as Grant_amt, a.country as Country,a.addr as Address, vm.col1 as Asset_Name,vm.col2 as Bill_No," +
                                "to_char(vm.date2, '" + sgen.Getsqldateformat() + "')" +
                                " as Bill_Date,to_char(vm.date1, '" + sgen.Getsqldateformat() + "') as Put_To_Use_Date,vm.col7 as" +
                                " Dock_no,vm.col8 as Bill_Amt_Company_curr,vm.col9 as Bill_Amt_Foreign_curr,vm.col13 as Sub_Area,vm.col25 as Asset_description," +
                                "vm.col17 as Warranty_If_Any,to_char(vm.date3, '" + sgen.Getsqldateformat() + "') as Warrenty_Expires_on,vm.col4 " +
                                "as minor_head,vm.col11 as Govt_Grant_Scheme,vm.col14 as cost_Centre,vm.col10 as Currency,vm.col19 as Asset_Head_Major " +
                                "FROM vehicle_master vm inner join clients_mst a on a.client_unit_id=vm.client_unit_id and a.vch_num = vm.col30 and a.type = 'BCD'" +
                                " inner join enx_tab mnr on vm.col4 = mnr.vch_num and mnr.type = 'AMH' inner join  enx_tab ggs on vm.col11 = ggs.vch_num and ggs.type = 'GGM' " +
                                "and ggs.client_unit_id = vm.client_unit_id inner join master_setting cc on vm.col14 = cc.master_id and cc.type = 'FAC' " +
                                "and cc.client_unit_id = vm.client_unit_id inner join master_setting c on c.master_id = vm.col10 and " +
                                "c.type = 'CTM' and c.client_unit_id = '" + unitid_mst + "' " +
                                "where (vm.client_id || vm.client_unit_id || vm.vch_num || to_char(vm.vch_date, 'yyyymmdd')|| vm.type)='" + URL + "' ";

                            DataTable dtt = new DataTable();
                            dtt = sgen.getdata(userCode, mq);

                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                #region 
                                TempData[MyGuid + "_Tlist1"] = model[0].TList1 = mod1 = cmd_Fun.assminorhead(userCode, unitid_mst);     //Asset minor dropdown 1
 
                                TempData[MyGuid + "_Tlist4"] = model[0].TList4 = mod4 = cmd_Fun.asscostcenter(userCode, unitid_mst);  //ASSET Cost Center  4
                                   
                                TempData[MyGuid + "_Tlist3"] = model[0].TList3 = mod3 = cmd_Fun.govtgrantsch(userCode, unitid_mst);    //Govt Grant Scheme 3

                                DataTable dt5 = new DataTable();
                                TempData[MyGuid + "_Tlist2"] = model[0].TList2 = mod2 = cmd_Fun.curname(userCode, unitid_mst);   //currency


                                TempData[MyGuid + "_Tlist6"] = model[0].TList6 = mod6 = cmd_Fun.iloc(userCode, unitid_mst);    // location
                            
                                mod5.Add(new SelectListItem { Text = "Yes", Value = "Y" });
                                mod5.Add(new SelectListItem { Text = "No", Value = "N" });
                                TempData[MyGuid + "_Tlist5"] = model[0].TList5 = mod5;
                                #endregion
                                model[0].Col8 = "(client_id|| client_unit_id||vch_num|| to_char(vch_date, 'yyyymmdd') || type) = '" + URL + "'";
                                model[0].Col16 = dtt.Rows[0]["vch_num"].ToString();
                                model[0].Col18 = dtt.Rows[0]["Bill_Date"].ToString();
                                model[0].Col34 = dtt.Rows[0]["Asset_Name"].ToString();
                                model[0].Col17 = dtt.Rows[0]["Bill_No"].ToString();
                                model[0].Col19 = dtt.Rows[0]["Put_To_Use_Date"].ToString();
                                model[0].Col20 = dtt.Rows[0]["Asset_Head_Major"].ToString();
                                //model[0].Col21 = dtt.Rows[0]["Asset_code"].ToString();
                                //model[0].Col22 = dtt.Rows[0]["Dock_no"].ToString();
                                model[0].Col23 = dtt.Rows[0]["Bill_Amt_Company_curr"].ToString();
                                model[0].Col24 = dtt.Rows[0]["Bill_Amt_Foreign_curr"].ToString();
                                model[0].Col26 = dtt.Rows[0]["Grant_amt"].ToString();
                                model[0].Col25 = dtt.Rows[0]["Capax_amt"].ToString();
                                model[0].Col43 = dtt.Rows[0]["col33"].ToString();   //capax it
                                model[0].Col29 = dtt.Rows[0]["col13"].ToString();
                                model[0].Col30 = dtt.Rows[0]["Asset_description"].ToString();
                                model[0].Col39 = dtt.Rows[0]["Name"].ToString();
                                model[0].Col41 = dtt.Rows[0]["Country"].ToString();
                                model[0].Col42 = dtt.Rows[0]["Address"].ToString();
                                model[0].Col40 = dtt.Rows[0]["col30"].ToString();
                                model[0].Col52 = dtt.Rows[0]["Warrenty_Expires_on"].ToString();
                                model[0].Col7 = dtt.Rows[0]["rec_id"].ToString();
                                model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();

                                model[0].Col44 = dtt.Rows[0]["col3"].ToString();
                                model[0].Col45 = dtt.Rows[0]["col5"].ToString();
                                model[0].Col46 = dtt.Rows[0]["col6"].ToString();
                                model[0].Col47 = dtt.Rows[0]["col15"].ToString();
                                model[0].Col48 = dtt.Rows[0]["col37"].ToString();
                                model[0].Col49 = dtt.Rows[0]["col18"].ToString();
                                model[0].Col50 = dtt.Rows[0]["col40"].ToString();
                                model[0].Col51 = dtt.Rows[0]["col42"].ToString();
                                model[0].Col22 = dtt.Rows[0]["col29"].ToString();

                                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                String[] L1 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["minor_head"].ToString()).Distinct()).Split(',');
                                String[] L2 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["Currency"].ToString()).Distinct()).Split(',');
                                String[] L3 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["Govt_Grant_Scheme"].ToString()).Distinct()).Split(',');
                                String[] L4 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["cost_Centre"].ToString()).Distinct()).Split(',');
                                String[] L5 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["Warranty_If_Any"].ToString()).Distinct()).Split(',');
                                String[] L6 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["col12"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems1 = L1;
                                model[0].SelectedItems3 = L3;
                                model[0].SelectedItems2 = L2;
                                model[0].SelectedItems4 = L4;
                                model[0].SelectedItems5 = L5;
                                model[0].SelectedItems6 = L6;
                                model[0].Col9 = tm.Col9;
                                model[0].Col10 = tm.Col10;
                                model[0].Col11 = tm.Col11;
                                model[0].Col12 = tm.Col12;
                                model[0].Col13 = "Update";
                                model[0].Col100 = "Update & New";
                                model[0].Col121 = "U";
                                model[0].Col122 = "<u>U</u>pdate";
                                model[0].Col123 = "Update & Ne<u>w</u>";

                                //DataTable dtf = new DataTable();
                                //dtf = sgen.getdata(userCode, "select rec_id,file_name,file_path,col1 from file_tab where" +
                                //    " ref_code='" + dtt.Rows[0]["vch_num"].ToString() + "' and type='FAE' and " +
                                //    "client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
                                //List<Tmodelmain> ltmf = new List<Tmodelmain>();
                                //foreach (DataRow drf in dtf.Rows)
                                //{
                                //    Tmodelmain tmf = new Tmodelmain();
                                //    tmf.Col1 = drf["file_name"].ToString();
                                //    tmf.Col2 = drf["col1"].ToString();
                                //    tmf.Col3 = drf["rec_id"].ToString();
                                //    ltmf.Add(tmf);
                                //}
                                //model[0].LTM1 = ltmf;

                                #region File
                                DataTable dtf = new DataTable();
                                dtf = sgen.getdata(userCode, "select client_id||client_unit_id||vch_num||to_char(vch_date,'yyyymmdd')||type as fstr, rec_id,file_name,file_path,col1,col2,description,Col3 from file_tab where vch_num='" + dtt.Rows[0]["vch_num"].ToString() + "' and type in " +
                                    "('" + tm.Col12 + "','DD" + model[0].Col12 + "') and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "' and col1='FAE'");
                                List<Tmodelmain> ltmf = new List<Tmodelmain>();
                                string des = "", title = "", f_recid = "";
                                if (dtf.Rows.Count > 0)
                                {
                                    model[0].Col31 = dtf.Rows[0]["col3"].ToString();
                                    model[0].Col32 = dtf.Rows[0]["description"].ToString();
                                    foreach (DataRow drf in dtf.Rows)
                                    {
                                        Tmodelmain tmf = new Tmodelmain();
                                        tmf.Col4 = drf["file_path"].ToString();
                                        tmf.Col1 = drf["file_name"].ToString();
                                        tmf.Col2 = drf["col1"].ToString();
                                        tmf.Col3 = drf["rec_id"].ToString();
                                        tmf.Col97 = drf["rec_id"].ToString();
                                        tmf.Col5 = drf["description"].ToString();
                                        des = drf["description"].ToString();
                                        title = drf["Col3"].ToString();
                                        tmf.Col6 = drf["col3"].ToString();
                                        tmf.Col7 = drf["col2"].ToString();
                                        ltmf.Add(tmf);
                                    }
                                }
                                model[0].LTM1 = ltmf; 
                                #endregion
                            }
                            
                            break;

                        case "PARTY":
                            mq = "select distinct a.c_name as name,b.country_name as country,a.addr,a.vch_num,a.tor from clients_mst a" +
                                " inner join country_state b on a.country = b.alpha_2 and" +
                                " a.client_unit_id = b.client_unit_id where (a.vch_num || to_char(a.vch_date, 'yyyymmdd') || a.type) = '" + URL + "' ";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                model[0].Col39 = dtt.Rows[0]["name"].ToString();
                                model[0].Col40 = dtt.Rows[0]["vch_num"].ToString();
                                model[0].Col41 = dtt.Rows[0]["addr"].ToString();
                                model[0].Col42 = dtt.Rows[0]["country"].ToString();
                            }
                            break;

                        case "FDEL":
                            string fid = sgen.GetSession(MyGuid, "delid").ToString();
                            res = sgen.execute_cmd(userCode, "Delete from file_tab WHERE rec_id='" + fid + "' and type='FAE' and client_id='"
              + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
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

                    }
                    break;
                #endregion

                #region cost_centre
                case "cost_center":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "SELECT a.client_name,a.master_id, a.vch_num,a.rec_id, a.master_name,a.classid,a.sectionid," +
                                "a.status,a.client_id,a.client_unit_id,a.master_entby as ent_by,a.master_entdate as ent_date,vch_date" +
                                " FROM " + model[0].Col10 + " a where (a.client_id || a.client_unit_id || a.vch_num || to_char(a.vch_date, 'yyyymmdd') || a.type) = '" + URL + "' ";


                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");

                                #region department
                                TempData[MyGuid + "_Tlist1"] = model[0].TList1 = mod1 = cmd_Fun.dept(userCode, clientid_mst, unitid_mst);
                                #endregion
                                model[0].Col8 = "(client_id|| client_unit_id|| vch_num|| to_char(vch_date, 'yyyymmdd') || type) = '" + URL + "'";
                                model[0].Col1 = dt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col17 = dt.Rows[0]["master_id"].ToString();
                                model[0].Col16 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col18 = dt.Rows[0]["master_name"].ToString();
                                model[0].Col19 = dt.Rows[0]["status"].ToString();
                                model[0].Col3 = dt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dt.Rows[0]["ent_date"].ToString();
                                model[0].Col7 = dt.Rows[0]["rec_id"].ToString();
                                String[] L1 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["classid"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems1 = L1;
                                model[0].Col9 = tm.Col9;
                                model[0].Col10 = tm.Col10;
                                model[0].Col11 = tm.Col11;
                                model[0].Col12 = tm.Col12;
                                model[0].Col13 = "Update";
                                model[0].Col100 = "Update & New";
                                model[0].Col121 = "U";
                                model[0].Col122 = "<u>U</u>pdate";
                                model[0].Col123 = "Update & Ne<u>w</u>";
                            }
                            else
                            {
                                model = new List<Tmodelmain>();
                                tm.Col17 = "";
                                tm.Col18 = "";
                                tm.Col19 = "";
                                tm.Col20 = "";
                                model.Add(tm);

                            }
                            break;
                    }
                    break;
                #endregion                                

                #region Asset sale
                case "asset_sale":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select to_char( vm.date1, '" + sgen.Getsqldateformat() + "') as Date_sales," +
                                "vm.vch_num,vm.rec_id,vm.ent_by,vm.col3 as WDV,vm.col5 as taxes,vm.col4 as Sales_Basic_Amount," +
                                "vm.ent_date,vm.client_id,vm.col2 as Asset_code,vm.client_unit_id,vm.col6 as Gross_Sales_Value" +
                                ",vm.col7 as Profit_Loss_On_Asset from vehicle_master vm " +
                                "where (vm.client_id || vm.client_unit_id || vm.vch_num || to_char(vm.vch_date, 'ddmmyyyy') || vm.type) = '" + URL + "' ";
                            
                            DataTable dtt = new DataTable();
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col8 = "(client_id|| client_unit_id|| vch_num|| to_char(vch_date, 'ddmmyyyy')||type) = '" + URL + "'";
                                model[0].Col16 = dtt.Rows[0]["vch_num"].ToString();
                                model[0].Col27 = dtt.Rows[0]["Date_sales"].ToString();
                                model[0].Col18 = dtt.Rows[0]["WDV"].ToString();
                                model[0].Col19 = dtt.Rows[0]["Sales_Basic_Amount"].ToString();
                                model[0].Col20 = dtt.Rows[0]["taxes"].ToString();
                                model[0].Col21 = dtt.Rows[0]["Gross_Sales_Value"].ToString();
                                model[0].Col22 = dtt.Rows[0]["Profit_Loss_On_Asset"].ToString();
                                model[0].Col7 = dtt.Rows[0]["rec_id"].ToString();
                                model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col13 = "Update";
                                model[0].Col100 = "Update & New";
                                model[0].Col121 = "U";
                                model[0].Col122 = "<u>U</u>pdate";
                                model[0].Col123 = "Update & Ne<u>w</u>";

                                mq1 = "select distinct (vm.client_id || vm.client_unit_id || vm.vch_num || to_char(vm.vch_date, 'yyyymmdd')|| vm.type) as fstr,vm.vch_num, vm.col1 as Asset_Name,vm.col2 as Bill_No,to_char(vm.date2, 'dd/mm/yyyy') as Bill_Date,vm.col29 as quantity" +
                      ",cc.master_name as cost_Centre,lc.master_name as location FROM vehicle_master vm left join master_setting cc on vm.col14 = cc.master_id and cc.type = 'FAC' AND cc.client_unit_id = vm.client_unit_id " +
                      "left join master_setting lc on vm.col12 = lc.master_id and lc.type = 'LC6' AND lc.client_unit_id = vm.client_unit_id where  vm.client_unit_id = '" + unitid_mst + "' and vm.type = 'FAE' and vm.vch_num = '" + dtt.Rows[0]["Asset_code"].ToString() + "'";
                                dt = new DataTable();
                                dt = sgen.getdata(userCode, mq1);
                                if (dt.Rows.Count > 0)
                                {
                                    model[0].Col30 = dt.Rows[0]["Asset_Name"].ToString();
                                    model[0].Col31 = dt.Rows[0]["vch_num"].ToString();
                                    model[0].Col32 = dt.Rows[0]["cost_Centre"].ToString();
                                    model[0].Col33 = dt.Rows[0]["location"].ToString();
                                    model[0].Col17 = dt.Rows[0]["quantity"].ToString();
                                }
                            }
                            break;
                        case "ASSET":
                            mq = "select distinct (vm.client_id || vm.client_unit_id || vm.vch_num || to_char(vm.vch_date, 'yyyymmdd')|| vm.type) as fstr,vm.vch_num, vm.col1 as Asset_Name,vm.col2 as Bill_No,to_char(vm.date2, 'dd/mm/yyyy') as Bill_Date,vm.col29 as quantity" +
                                ",cc.master_name as cost_Centre,lc.master_name as location FROM vehicle_master vm left join master_setting cc on vm.col14 = cc.master_id and cc.type = 'FAC' AND cc.client_unit_id = vm.client_unit_id " +
                                "left join master_setting lc on vm.col12 = lc.master_id and lc.type = 'LC6' AND lc.client_unit_id = vm.client_unit_id where (vm.client_id || vm.client_unit_id || vm.vch_num || to_char(vm.vch_date, 'yyyymmdd')|| vm.type) = '" + URL + "'";
                            dtt = new DataTable();
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                model[0].Col30 = dtt.Rows[0]["Asset_Name"].ToString();
                                model[0].Col31 = dtt.Rows[0]["vch_num"].ToString();
                                model[0].Col32 = dtt.Rows[0]["cost_Centre"].ToString();
                                model[0].Col33 = dtt.Rows[0]["location"].ToString();
                                model[0].Col17 = dtt.Rows[0]["quantity"].ToString();
                            }
                            break;
                    }
                    break;
                #endregion

                #region Asset Write Off
                case "asset_write":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select vm.vch_num,vm.rec_id,vm.ent_by,vm.ent_date,vm.client_id,vm.client_unit_id,to_char(date1, '" + sgen.Getsqldateformat() + "') as Date_of_sale," +
                                "vm.col3 as WDV,vm.col4 as Profit_Loss_On_Sale,vm.col1 as Asset_code from vehicle_master vm " +
                                "where (vm.client_id || vm.client_unit_id || vm.vch_num || to_char(vm.vch_date, 'ddmmyyyy') || vm.type) = '" + URL + "'";

                            DataTable dtt = new DataTable();
                            dtt = sgen.getdata(userCode, mq);

                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col8 = "(client_id|| client_unit_id|| vch_num|| to_char(vch_date, 'ddmmyyyy')|| type) = '" + URL + "'";
                                model[0].Col16 = dtt.Rows[0]["vch_num"].ToString();
                                model[0].Col24 = dtt.Rows[0]["Date_of_sale"].ToString();
                                model[0].Col18 = dtt.Rows[0]["WDV"].ToString();
                                model[0].Col19 = dtt.Rows[0]["Profit_Loss_On_Sale"].ToString();
                                model[0].Col7 = dtt.Rows[0]["rec_id"].ToString();
                                model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col13 = "Update";
                                model[0].Col100 = "Update & New";
                                model[0].Col121 = "U";
                                model[0].Col122 = "<u>U</u>pdate";
                                model[0].Col123 = "Update & Ne<u>w</u>";
                                mq1 = "select distinct (vm.client_id || vm.client_unit_id || vm.vch_num || to_char(vm.vch_date, 'yyyymmdd')|| vm.type) as fstr,vm.vch_num, vm.col1 as Asset_Name,vm.col2 as Bill_No,to_char(vm.date2, 'dd/mm/yyyy') as Bill_Date,vm.col29 as quantity" +
                              ",cc.master_name as cost_Centre,lc.master_name as location FROM vehicle_master vm left join master_setting cc on vm.col14 = cc.master_id and cc.type = 'FAC' AND cc.client_unit_id = vm.client_unit_id " +
                              "left join master_setting lc on vm.col12 = lc.master_id and lc.type = 'LC6' AND lc.client_unit_id = vm.client_unit_id where  vm.client_unit_id = '" + unitid_mst + "' and vm.type = 'FAE' and vm.vch_num = '" + dtt.Rows[0]["Asset_code"].ToString() + "'";
                                dt = new DataTable();
                                dt = sgen.getdata(userCode, mq1);
                                if (dt.Rows.Count > 0)
                                {
                                    model[0].Col25 = dt.Rows[0]["vch_num"].ToString();
                                    model[0].Col21 = dt.Rows[0]["cost_Centre"].ToString();
                                    model[0].Col22 = dt.Rows[0]["location"].ToString();
                                    model[0].Col20 = dt.Rows[0]["Asset_Name"].ToString();
                                    model[0].Col17 = dt.Rows[0]["quantity"].ToString();
                                }
                            }
                            break;
                        case "ASSET":
                            mq = "select distinct (vm.client_id || vm.client_unit_id || vm.vch_num || to_char(vm.vch_date, 'yyyymmdd')|| vm.type) as fstr,vm.vch_num, vm.col1 as Asset_Name,vm.col2 as Bill_No,to_char(vm.date2, 'dd/mm/yyyy') as Bill_Date,vm.col29 as quantity" +
                                ",cc.master_name as cost_Centre,lc.master_name as location FROM vehicle_master vm left join master_setting cc on vm.col14 = cc.master_id and cc.type = 'FAC' AND cc.client_unit_id = vm.client_unit_id " +
                                "left join master_setting lc on vm.col12 = lc.master_id and lc.type = 'LC6' AND lc.client_unit_id = vm.client_unit_id where (vm.client_id || vm.client_unit_id || vm.vch_num || to_char(vm.vch_date, 'yyyymmdd')|| vm.type) = '"+URL+"'";
                            dtt = new DataTable();
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                model[0].Col25 = dtt.Rows[0]["vch_num"].ToString();
                                model[0].Col21 = dtt.Rows[0]["cost_Centre"].ToString();
                                model[0].Col22 = dtt.Rows[0]["location"].ToString();
                                model[0].Col20 = dtt.Rows[0]["Asset_Name"].ToString();
                                model[0].Col17 = dtt.Rows[0]["quantity"].ToString();
                            }
                            break;
                    }
                    break;
                #endregion

                #region fa_m 
                case "fa_m":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select rec_id,col1,col2,col3,col4,col5,col6,col7," +
                                "col8,r_no,ref_code,type,ent_by,ent_date,client_id,client_unit_id,vch_num," +
                                "type_desc from " + model[0].Col10 + " where type='" + model[0].Col12 + "' " +
                                "and client_id||client_unit_id||vch_num||to_char(vch_date, 'yyyymmdd')||type ='" + URL + "'";
                            DataTable dtt = new DataTable();
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");

                                #region Asset minor dropdown
                                mq1 = "Select master_id, master_name from master_setting where type = 'MAH' " + model[0].Col11.Trim() + "";
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


                                model[0].Col8 = "client_id||client_unit_id||vch_num||to_char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";
                                model[0].Col26 = dtt.Rows[0]["col1"].ToString();
                                model[0].Col18 = dtt.Rows[0]["col4"].ToString();
                                model[0].Col19 = dtt.Rows[0]["col7"].ToString();
                                model[0].Col20 = dtt.Rows[0]["col8"].ToString();
                                model[0].Col21 = dtt.Rows[0]["col3"].ToString();
                                model[0].Col23 = dtt.Rows[0]["col5"].ToString();
                                model[0].Col22 = dtt.Rows[0]["col6"].ToString();
                                model[0].Col24 = dtt.Rows[0]["r_no"].ToString();
                                model[0].Col25 = dtt.Rows[0]["ref_code"].ToString();
                                model[0].Col17 = dtt.Rows[0]["type_desc"].ToString();
                                model[0].Col7 = dtt.Rows[0]["rec_id"].ToString();
                                model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col16 = dtt.Rows[0]["vch_num"].ToString();
                                String[] L1 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["col2"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems1 = L1;
                                model[0].Col13 = "Update";
                                model[0].Col100 = "Update & New";
                                model[0].Col121 = "U";
                                model[0].Col122 = "<u>U</u>pdate";
                                model[0].Col123 = "Update & Ne<u>w</u>";
                            }
                            else
                            {
                                model = new List<Tmodelmain>();
                                tm.Col17 = "";
                                tm.Col18 = "";
                                tm.Col19 = "";
                                tm.Col20 = "";
                                model.Add(tm);

                            }
                            break;
                    }
                    break;



                #endregion

                #region epcg_mst
                case "epcg_mst":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select rec_id,col6,to_char(convert_tz(date1, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as date1,col3 as date1," +
                                "col7,col8,col3,to_char(convert_tz(date2, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as date2," +
                                "col1,type,ent_by,ent_date,client_id,client_unit_id,vch_num from " + sgen.GetSession(MyGuid, "FA_TBL_GGM") + " where type='EPC' " +
                                "and (client_id||client_unit_id||vch_num||to_char(vch_date, 'ddmmyyyyy')||type)='" + URL + "'";

                            DataTable dtt = new DataTable();
                            dtt = sgen.getdata(userCode, mq);
                            model[0].Col37 = "file_tab";
                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");

                                model[0].Col8 = "(client_id|| client_unit_id||vch_num||to_char(vch_date, 'ddmmyyyyy')|| type) = '" + URL + "'";
                                model[0].Col38 = "(client_id|| client_unit_id||vch_num||to_char(vch_date, 'ddmmyyyyy')|| type) = '" + URL + "'";
                                model[0].Col16 = dtt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dtt.Rows[0]["col6"].ToString();
                                model[0].Col18 = dtt.Rows[0]["date1"].ToString();
                                model[0].Col19 = dtt.Rows[0]["col3"].ToString();
                                model[0].Col22 = dtt.Rows[0]["date2"].ToString();
                                model[0].Col21 = dtt.Rows[0]["col1"].ToString();
                                model[0].Col20 = dtt.Rows[0]["col7"].ToString();
                                model[0].Col23 = dtt.Rows[0]["col8"].ToString();
                                model[0].Col7 = dtt.Rows[0]["rec_id"].ToString();
                                model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col13 = "Update";
                                model[0].Col100 = "Update & New";

                                DataTable dtf = new DataTable();
                                dtf = sgen.getdata(userCode, "select rec_id,file_name,file_path,col1 from file_tab where" +
                                                             " ref_code='" + dtt.Rows[0]["vch_num"].ToString() + "' and type='EPC' and " +
                                                             "client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
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
                            else
                            {
                                model = new List<Tmodelmain>();
                                tm.Col17 = "";
                                tm.Col18 = "";
                                tm.Col19 = "";
                                tm.Col20 = "";
                                model.Add(tm);

                            }
                            break;

                        case "FDEL":
                            string fid = sgen.GetSession(MyGuid, "delid").ToString();
                            res = sgen.execute_cmd(userCode, "Delete from file_tab WHERE rec_id='" + fid + "' and type='" + model[0].Col12 + "' and client_id='"
                             + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
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
                    }
                    break;
                #endregion

                #region fa_major_head
                case "fa_major_head":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select master_name,rec_id,client_name,Status,vch_num,master_id,master_entby as ent_by ,master_editby,Inactive_date,vch_date from " + model[0].Col10 + "  " +
                                "where (client_id|| client_unit_id||master_id||to_char(vch_date, 'yyyymmdd')||type)= '" + URL + "'";
                            DataTable dtt = new DataTable();
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col8 = "(client_id||client_unit_id|| master_id||to_char(vch_date, 'yyyymmdd')||type) = '" + URL + "'";
                                model[0].Col16 = dtt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dtt.Rows[0]["master_name"].ToString();
                                model[0].Col18 = dtt.Rows[0]["client_name"].ToString();
                                model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                                model[0].Col7 = dtt.Rows[0]["rec_id"].ToString();
                                model[0].Col19 = dtt.Rows[0]["Status"].ToString();
                                model[0].Col5 = dtt.Rows[0]["master_editby"].ToString();
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

                #region Goverment Grant master
                case "goverment_grant":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select rec_id,col6,to_char(convert_tz(date1, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as date1,col3,to_char(convert_tz(date2, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as date2,col1,type,ent_by,ent_date,client_id,client_unit_id,vch_num from " + sgen.GetSession(MyGuid, "FA_TBL_GGM") + " where type='" + sgen.GetSession(MyGuid, "FA_TYPE_GGM") + "' and" +
                                " (client_id|| client_unit_id||vch_num||to_char(vch_date, 'yyyymmdd')||type)='" + URL + "'";
                            DataTable dtt = new DataTable();
                            dtt = sgen.getdata(userCode, mq);

                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].TList1 = mod1;
                                model[0].Col8 = "(client_id|| client_unit_id|| vch_num|| to_char(vch_date, 'yyyymmdd')|| type) = '" + URL + "'";
                                model[0].Col16 = dtt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dtt.Rows[0]["col6"].ToString();
                                model[0].Col18 = dtt.Rows[0]["date1"].ToString();
                                model[0].Col19 = dtt.Rows[0]["col3"].ToString();
                                model[0].Col20 = dtt.Rows[0]["date2"].ToString();
                                model[0].Col21 = dtt.Rows[0]["col1"].ToString();
                                model[0].Col7 = dtt.Rows[0]["rec_id"].ToString();
                                model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col13 = "Update";
                                model[0].Col100 = "Update & New";
                            }
                            else
                            {
                                model = new List<Tmodelmain>();
                                tm.Col17 = "";
                                tm.Col18 = "";
                                tm.Col19 = "";
                                tm.Col20 = "";
                                model.Add(tm);

                            }
                            break;
                    }
                    break;
                #endregion

                #region transfer of Asset
                case "transfer_asset":

                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select vch_num,rec_id,ent_by,ent_date,client_id,client_unit_id," +
                                "to_char(date1, '" + sgen.Getsqldateformat() + "') as  Date_Of_Transfer,col1,col2,col3,col4,col7,col8 from enx_tab  " +
                                "where (client_id||client_unit_id|| vch_num|| to_char(vch_date, 'ddmmyyyy')|| type)='" + URL + "'";

                            DataTable dtt = new DataTable();
                            dtt = sgen.getdata(userCode, mq);

                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");

                                model[0].Col8 = "(client_id||client_unit_id||vch_num||to_char(vch_date, 'ddmmyyyy')|| type) = '" + URL + "'";
                                model[0].Col16 = dtt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dtt.Rows[0]["Date_Of_Transfer"].ToString();
                                model[0].Col18 = dtt.Rows[0]["col2"].ToString();
                                model[0].Col19 = dtt.Rows[0]["col4"].ToString();
                                model[0].Col32 = dtt.Rows[0]["col7"].ToString();
                                model[0].Col33 = dtt.Rows[0]["col8"].ToString();
                                model[0].Col29 = dtt.Rows[0]["col1"].ToString();
                                model[0].Col30 = dtt.Rows[0]["col3"].ToString();
                                model[0].Col7 = dtt.Rows[0]["rec_id"].ToString();
                                model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col13 = "Update";
                                model[0].Col100 = "Update & New";


                                mq = "select (ms.client_id||ms.client_unit_id||ms.vch_num||to_char(ms.vch_date,'ddmmyyyy')||ms.type) as fstr," +
                               "bg.master_name as Building,ex.col4 as Block,ms.subjectid as Area," +
                               "(b.First_name|| ' '|| b.middle_name||' '||b.last_name) as Ent_By," +
                               "to_char(convert_tz(ms.master_EntDate, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Ent_Date " +
                               "from master_setting ms" +
                               " inner join master_setting bg on bg.master_id = ms.classid and bg.type='HBM' and bg.client_unit_id=ms.client_unit_id inner join enx_tab ex on " +
                               "ex.vch_num = ms.sectionid and ex.type = 'HBM' and ex.client_unit_id = ms.client_unit_id " +
                               "inner join user_details b on ms.master_entby = b.vch_num " +
                               "and b.type = 'CPR' where ms.client_unit_id = '" + unitid_mst + "' and ms.type = 'FAL' and ms.master_id = '" + dtt.Rows[0]["col8"].ToString() + "'";
                                DataTable dtm = sgen.getdata(userCode, mq);
                                if (dtm.Rows.Count > 0)
                                {
                                    model[0].Col26 = dtm.Rows[0]["area"].ToString();
                                    model[0].Col28 = dtm.Rows[0]["building"].ToString();
                                    model[0].Col27 = dtm.Rows[0]["block"].ToString();
                                }
                                mq1 = "select vm.rec_id, vm.col15 as Asset_description,ex.col1 as Asset_Head_Minor,vm.col13 as Sub_area,c.subjectid as area,bg.master_name as building,BK.COL4 as block from vehicle_master vm " +
                                    "inner join enx_tab ex on ex.vch_num = vm.col4 and ex.type = 'AMH' " +
                                    "inner join master_setting c on c.master_id = vm.col12 and c.type = 'FAL'" +
                                    "inner join master_setting bg on bg.master_id = c.classid and bg.type = 'HBM'" +
                                    "inner join enx_tab bk on bk.vch_num = c.sectionid and bk.type = 'HBM'  where vm.client_unit_id='" + unitid_mst + "' and vm.type='FAE' and vm.rec_id='" + dtt.Rows[0]["col7"].ToString() + "'";
                                DataTable dt1 = sgen.getdata(userCode, mq1);
                                if (dt1.Rows.Count > 0)
                                {
                                    model[0].Col20 = dt1.Rows[0]["Asset_description"].ToString();
                                    model[0].Col21 = dt1.Rows[0]["Asset_Head_Minor"].ToString();
                                    model[0].Col22 = dt1.Rows[0]["area"].ToString();
                                    model[0].Col23 = dt1.Rows[0]["block"].ToString();
                                    model[0].Col24 = dt1.Rows[0]["building"].ToString();
                                    model[0].Col25 = dt1.Rows[0]["Sub_area"].ToString();
                                }
                            }
                            break;
                        case "ADD1":

                            mq = "select(ms.client_id||ms.client_unit_id||ms.vch_num||to_char(ms.vch_date,'yyyymmdd')||ms.type) as fstr,bg.master_name as Building, ex.col4 as Block,ms.subjectid as Area,(b.First_name||' '|| b.middle_name|| ' '||b.last_name) as Ent_By,ms.master_id,to_char(convert_tz(ms.master_EntDate, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "')as Ent_Date from master_setting ms " +
                            "inner join master_setting bg on bg.master_id = ms.classid and bg.type = 'HBM' and bg.client_unit_id = ms.client_unit_id  " +
                             "inner join(select DISTINCT vch_num, col4 from enx_tab where type= 'HBM' and client_unit_id = '" + unitid_mst + "') ex on ex.vch_num = ms.sectionid  " +
                        "inner join user_details b on  ms.master_entby = b.vch_num and b.type = 'CPR' " +
                      "where (ms.client_id|| ms.client_unit_id|| ms.vch_num||to_char(ms.vch_date, 'ddmmyyyy')|| ms.type) = '" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {

                                model[0].Col26 = dt.Rows[0]["Area"].ToString();
                                model[0].Col28 = dt.Rows[0]["Building"].ToString();
                                model[0].Col27 = dt.Rows[0]["Block"].ToString();
                                model[0].Col33 = dt.Rows[0]["master_id"].ToString();
                            }
                            break;

                        case "ASSETDESC":
                            mq1 = "Select vm.rec_id, vm.col15 as Asset_description,ex.col1 as Asset_Head_Minor,vm.col13 as Sub_area,c.subjectid as area,bg.master_name as building,BK.COL4 as block from vehicle_master vm " +
                                "inner join enx_tab ex on ex.vch_num = vm.col4 and ex.type = 'AMH' inner join master_setting c on c.master_id = vm.col12 and c.type = 'FAL'" +
                                "inner join master_setting bg on bg.master_id = c.classid and bg.type = 'HBM'" +
                                "inner join enx_tab bk on bk.vch_num = c.sectionid and bk.type = 'HBM'" +
                                " where (vm.client_id||vm.client_unit_id||vm.vch_num|| to_char(vm.vch_date, 'ddmmyyyy')||vm.type)='" + URL + "'";
                            dtt = sgen.getdata(userCode, mq1);
                            if (dtt.Rows.Count > 0)
                            {

                                model[0].Col20 = dtt.Rows[0]["Asset_description"].ToString();
                                model[0].Col21 = dtt.Rows[0]["Asset_Head_Minor"].ToString();
                                model[0].Col22 = dtt.Rows[0]["area"].ToString();
                                model[0].Col23 = dtt.Rows[0]["block"].ToString();
                                model[0].Col24 = dtt.Rows[0]["building"].ToString();
                                model[0].Col25 = dtt.Rows[0]["Sub_area"].ToString();
                                model[0].Col32 = dtt.Rows[0]["rec_id"].ToString();


                            }
                            break;


                    }
                    break;
                #endregion

                #region pv_stmt

                case "pv_stmt":

                    switch (btnval.ToUpper())
                    {
                        case "EDIT":

                            mq = @"select DISTINCT a.*,to_char(convert_tz(vch_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') vch_date1 from itransaction a " +
                                "where (client_id||client_unit_id||vch_num|| to_char(vch_date, 'yyyymmdd')||type)='" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
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
                                model[0].Col8 = "(client_id||client_unit_id||vch_num|| to_char(vch_date, 'yyyymmdd')||type) = '" + URL + "'";
                                model[0].Col10 = tm.Col10;
                                model[0].Col11 = tm.Col11;
                                model[0].Col12 = dt.Rows[0]["type"].ToString();
                                model[0].Col13 = "Update";
                                model[0].Col100 = "Update & New";
                                model[0].Col14 = tm.Col14;
                                model[0].Col15 = tm.Col15;
                                model[0].Col18 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col20 = dt.Rows[0]["vch_date1"].ToString();
                                model[0].Col21 = dt.Rows[0]["totremark"].ToString();

                                model[0].Col9 = sgen.seekval(userCode, "select upper(master_name) master_name from master_setting where master_id='" + dt.Rows[0]["type"].ToString() + "'", "master_name");

                             
                                
                                model[0].SelectedItems1 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["deptno"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems2 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["desig"].ToString()).Distinct()).Split(',');

                                model[0].dt1 = model[0].dt1.Clone();

                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["sno"] = i.ToString();
                                    dr["IName"] = dt.Rows[i]["iname"].ToString();
                                    dr["Icode"] = dt.Rows[i]["icode"].ToString();
                                    dr["UOM"] = dt.Rows[i]["uom"].ToString();
                                    dr["partno"] = dt.Rows[i]["cpartno"].ToString();
                                    dr["Qty_in_stock"] = dt.Rows[i]["qtystk"].ToString();
                                    dr["Qty_Phy"] = dt.Rows[i]["qtychl"].ToString();
                                    dr["Variation"] = dt.Rows[i]["qtyout"].ToString();
                                    dr["Variation"] = dt.Rows[i]["qtyin"].ToString();
                                    dr["Remark"] = dt.Rows[i]["iremark"].ToString();
                                    model[0].dt1.Rows.Add(dr);
                                }
                            }
                            break;

                        case "ITEM":

                            //mq = "select (it.client_id||it.client_unit_id||it.icode||to_char(it.vch_date,'yyyymmdd')||it.type) fstr,it.icode as Icode,it.iname as Iname,it.cpartno," +
                            //   "um.master_name as uom,s.closing as qty_in_stock,'0' as Qty_Phy,'0' Variation,'' remark,'0' loccode,'' loc from item it " +
                            //   "inner join master_setting um on um.master_id=it.uom and um.type='UMM' and um.client_id='" + clientid_mst + "' and um.client_unit_id='" + unitid_mst + "' " +
                            //   "inner join master_setting hsn on hsn.master_id=it.hsno and hsn.type='HSN' and hsn.client_id='" + clientid_mst + "' and hsn.client_unit_id='" + unitid_mst + "' " +
                            //   "left join (select (nvl(t.client_unit_id,'0')||i.icode) as fstr,i.icode,i.iname as Item_Name ," +
                            //   "sum(nvl(t.qtyin,0))-sum(nvl(t.qtyout,0)) as closing  from item i left join itransaction t on t.icode=i.icode and T.client_unit_id='" + unitid_mst + "' where " +
                            //   "length(i.icode)>4 group by t.client_unit_id,i.icode,i.iname order by i.icode) s on it.icode=s.icode " +
                            //   "where it.type='IT' and it.client_id='" + clientid_mst + "' and it.client_unit_id='" + unitid_mst + "' " +
                            //   "and (it.client_id||it.client_unit_id||it.icode||to_char(it.vch_date,'yyyymmdd')||it.type) in ('" + URL + "')";

                            mq = "select vm.vch_num as Asset_no, a.c_name as Name,'0' as Asset_code,vm.rec_id,vm.ent_date,vm.client_unit_id,vm.client_id,vm.vch_num,vm.col28 " +
                                "as Capax_amt,vm.col13, vm.col36 as Grant_amt, a.country as Country,a.addr as Address, vm.col1 as Asset_Name," +
                                "'1' as qty_in_stock,'0' as Qty_Phy,'0' Variation,'' remark,vm.col2 as Bill_No," +
                                "to_char(vm.date2, '" + sgen.Getsqldateformat() + "') as Bill_Date," +
                                "to_char(vm.date1, '" + sgen.Getsqldateformat() + "') as Put_To_Use_Date,vm.col7 as Life_In_Years," +
                                "vm.col8 as Bill_Amt_Company_curr, vm.col25 as Asset_description,vm.col17 as Warranty_If_Any," +
                                "to_char(vm.date3, '" + sgen.Getsqldateformat() + "') as Warrenty_Expires_on,vm.col4 as minor_head," +
                                "vm.col11 as Govt_Grant_Scheme,vm.col14 as cost_Centre,vm.col10 as Currency,vm.col19 as Asset_Head_Major " +
                                "FROM vehicle_master vm inner join clients_mst a on a.client_unit_id = " +
                                "vm.client_unit_id and a.vch_num = vm.col30 and a.type = 'BCD' inner join enx_tab mnr on vm.col4 = mnr.vch_num" +
                                " and mnr.type = 'AMH' inner join  enx_tab ggs on vm.col11 = ggs.vch_num " +
                                "and ggs.type = 'GGM' and ggs.client_unit_id = vm.client_unit_id " +
                                "inner join master_setting cc on vm.col14 = cc.master_id and cc.type = 'FAC' and cc.client_unit_id = vm.client_unit_id inner join master_setting c on c.master_id = vm.col10 and " +
                                "c.type = 'CTM' and c.client_unit_id = '" + unitid_mst + "' " +
                                "where(vm.client_id || vm.client_unit_id || vm.vch_num || to_char(vm.vch_date, 'yyyymmdd') || vm.type) = '" + URL + "'";

                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                if (model[0].dt1.Rows[0]["ADD_ITEMS"].ToString().Trim().Equals("-")) { model[0].dt1.Rows.RemoveAt(0); }

                                model[0].dt1 = new DataTable();
                                model[0].dt1 = ((DataTable)sgen.GetSession(MyGuid, "BEX_DT")).Clone();
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["SNo"] = i + 1;
                                    dr["Asset_Code"] = dt.Rows[i]["Asset_code"].ToString();
                                    dr["Asset_Name"] = dt.Rows[i]["Asset_Name"].ToString();
                                    dr["Asset_no"] = dt.Rows[i]["Asset_no"].ToString();
                                    dr["minor_head"] = dt.Rows[i]["minor_head"].ToString();
                                    dr["Asset_Head_Major"] = dt.Rows[i]["Asset_Head_Major"].ToString();
                                    dr["Qty_in_stock"] = dt.Rows[i]["qty_in_stock"].ToString();
                                    dr["Qty_Phy"] = dt.Rows[i]["Qty_Phy"].ToString();
                                    dr["Variation"] = dt.Rows[i]["Variation"].ToString();
                                    dr["Put_To_Use_Date"] = dt.Rows[i]["Put_To_Use_Date"].ToString();
                                    //dr["Loc"] = dt.Rows[i]["loc"].ToString();
                                    model[0].dt1.Rows.Add(dr);
                                }
                                model[0].dt1.AcceptChanges();

                            }
                            //TempData[MyGuid + "_Tlist1"] = mod1;
                            //TempData[MyGuid + "_Tlist1"] = mod1;

                            break;
                    }
                    break;

                #endregion

                #region AMC_Ent
                case "amc_ent":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            //mq = "select distinct kt.rec_id,kt.type,kt.vch_num,kt.col1,to_char(convert_tz(kt.date1, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Bil_Date," +
                            //      "a.c_name as Party_Name,b.country_name as country,a.addr as Address," +
                            //   "a.vch_num,a.pincode Pincode,kt.col2 as Bill_Amount,kt.col3 as Remark,kt.col7 as P_id,to_char(convert_tz(kt.date2, 'UTC', '+05:30'), 'dd/MM/YYYY') as gd_Valid_to," +
                            //   "kt.col5 as GD_remark,kt.col6 as gd_Asset_code," +
                            //   "vm.col1 as Asset_Name,vm.vch_num as Asset_No,vm.col2 as Bill_No,kt.client_id,kt.client_unit_id,kt.ent_by,kt.ent_date,kt.edit_by,kt.edit_date," +
                            //   "to_char(convert_tz(vm.date2, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Bill_Date," +
                            //   "to_char(convert_tz(vm.date1, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Put_To_Use_Date," +
                            //   "vm.col6 as Asset_code,vm.col28 as Capex_Coact,vm.col33 as Capex_IT,vm.col18 as Life_In_Years,vm.col12 as Location,vm.col13 as Sub_Area," +
                            //   "vm.col25 as Description,vm.col8 as Bill_Amt_Company_curr,vm.col9 as Bill_Amt_Foreign_curr,vm.col15 as Asset_description,vm.col17 as Warranty," +
                            //   "to_char(convert_tz(vm.date3, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Expiring_On,mnr.col1 as minor_head," +
                            //   "vm.col29 as Quantity,cc.master_name as cost_Centre," +
                            //   "c.master_name as Currency,ms.master_name as Major_Head from kc_tab kt inner join clients_mst a on a.vch_num = kt.col7 and a.type = 'BCD' inner join country_state b on " +
                            //   "a.country = b.alpha_2 and a.client_unit_id = b.client_unit_id inner join  vehicle_master vm on vm.vch_num = kt.col6 and vm.type = 'FAE' " +
                            //   "inner join enx_tab mnr on vm.col4 = mnr.vch_num and mnr.type = 'AMH' and mnr.client_id = vm.client_id inner join master_setting cc on vm.col14 = cc.master_id" +
                            //   " and cc.type = 'FAC' AND cc.client_id = vm.client_id and cc.client_unit_id = vm.client_unit_id inner join master_setting c on c.master_id = vm.col10 " +
                            //   "and c.type = 'CTM' and c.client_id = vm.client_id and c.client_unit_id = vm.client_unit_id " +
                            //   "inner join master_setting ms on ms.master_id = mnr.col2 and ms.type = 'MAH' and ms.client_id = mnr.client_id and ms.client_unit_id = mnr.client_unit_id " +
                            //   "where (kt.client_id || kt.client_unit_id || kt.vch_num || to_char(kt.vch_date, 'yyyymmdd')|| kt.type)='"+URL+"'";

                            mq = "select distinct kt.rec_id,kt.type,kt.vch_num,kt.col1,kt.client_id,kt.client_unit_id,kt.ent_by,kt.ent_date,kt.edit_by,kt.edit_date,kt.col7 as P_id," +
                                  "kt.col1 as Bil_no,to_char(kt.date1, '" + sgen.Getsqldateformat() + "') as Bill_Date,aa.c_name as Party_Name,bb.country_name as country,aa.addr as Address," +
                                  "aa.vch_num as P_code,kt.col2 as Bill_Amount,kt.col3 as Remark,kt.col5 as GD_remark,to_char(kt.date2, '" + sgen.Getsqldateformat() + "') as gd_Valid_to," +
                                  "kt.col6 as gd_Asset_code,vm.col1 as Asset_Name,vm.vch_num as Asset_No," +
                                  "(b.master_name || ct.param1 || f.master_name || ct.param1 || rm.master_name || ct.param1 || rk.master_name || ct.param1 || a.master_name) Loc,vm.col2 as Bill_No," +
                                  "to_char(vm.date2, '" + sgen.Getsqldateformat() + "') as Bil_Date,to_char(vm.date1, '" + sgen.Getsqldateformat() + "') as Put_To_Use_Date," +
                                  "vm.col6 as Asset_code,vm.col28 as Capex_Coact,vm.col33 as Capex_IT,vm.col18 as Life_In_Years,vm.col13 as Sub_Area,vm.col25 as Description," +
                                  "vm.col8 as Bill_Amt_Company_curr,vm.col9 as Bill_Amt_Foreign_curr,vm.col17 as Warranty," +
                                  "to_char(vm.date3, '" + sgen.Getsqldateformat() + "') as Expiring_On,mnr.col1 as minor_head,vm.col29 as Quantity,cc.master_name as cost_Centre," +
                                  "c.master_name as Currency,ms.master_name as Major_Head from master_setting a inner join master_setting b on b.master_id = a.classid and b.type = 'HBM' and a.client_unit_id = b.client_unit_id inner join master_setting f on f.master_id = a.sectionid and f.type = 'IN0' and a.client_id = f.client_id and " +
                                  "a.client_unit_id = f.client_unit_id inner join master_setting rm on rm.master_id = a.client_name and rm.type = 'IN1' and a.client_unit_id = rm.client_unit_id inner join master_setting rk on rk.master_id = a.subjectid and rk.type = 'IN2' and a.client_id = rk.client_id and " +
                                  "a.client_unit_id = rk.client_unit_id inner join vehicle_master vm on vm.col12 = (a.client_id || a.client_unit_id || b.master_id || f.master_id || rm.master_id || rk.master_id || a.master_id)" +
                                  " and a.type = 'IN3' inner join enx_tab mnr on vm.col4 = mnr.vch_num and mnr.type = 'AMH' inner join master_setting cc" +
                                  " on vm.col14 = cc.master_id and cc.type = 'FAC' and cc.client_unit_id = vm.client_unit_id inner join master_setting c" +
                                  " on c.master_id = vm.col10 and c.type = 'CTM' and c.client_unit_id = vm.client_unit_id inner join controls ct on 1 = 1" +
                                  " and ct.id = '000010' inner join kc_tab kt  on kt.col6 = vm.vch_num inner join clients_mst aa on aa.vch_num = kt.col7 and aa.type = 'BCD' " +
                                  "inner join country_state bb on aa.country = bb.alpha_2 and aa.client_unit_id = bb.client_unit_id inner join  vehicle_master vm on vm.vch_num = kt.col6 " +
                                  "and vm.type = 'FAE' " +
                                  "inner join master_setting ms on ms.master_id = mnr.col2 and ms.type = 'MAH' and ms.client_unit_id = mnr.client_unit_id " +
                                  "where (kt.client_id || kt.client_unit_id || kt.vch_num || to_char(kt.vch_date, 'yyyymmdd')|| kt.type)='" + URL + "' ";


                            dt = sgen.getdata(userCode, mq);
                            model[0].Col37 = "file_tab";
                            if (dt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col1 = dt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dt.Rows[0]["ent_date"].ToString();
                                model[0].Col5 = dt.Rows[0]["edit_by"].ToString();
                                model[0].Col6 = dt.Rows[0]["edit_date"].ToString();
                                model[0].Col8 = "(client_id||client_unit_id||vch_num|| to_char(vch_date, 'yyyymmdd')||type) = '" + URL + "'";
                                model[0].Col38 = "(client_id|| client_unit_id||vch_num||to_char(vch_date, 'ddmmyyyyy')|| type) = '" + URL + "'";
                                model[0].Col10 = tm.Col10;
                                model[0].Col11 = tm.Col11;
                                model[0].Col12 = dt.Rows[0]["type"].ToString();
                                model[0].Col16 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col39 = dt.Rows[0]["Party_Name"].ToString();     //
                                model[0].Col40 = dt.Rows[0]["P_id"].ToString();           ////////PARTY
                                model[0].Col41 = dt.Rows[0]["Address"].ToString();        ////
                                model[0].Col42 = dt.Rows[0]["country"].ToString();        //
                                model[0].Col17 = dt.Rows[0]["col1"].ToString();            //Bill No.
                                model[0].Col18 = dt.Rows[0]["Bil_Date"].ToString();            //Bill Date
                                model[0].Col19 = dt.Rows[0]["Bill_Amount"].ToString();             //Bill Amount
                                model[0].Col20 = dt.Rows[0]["Remark"].ToString();             //Remark

                                model[0].dt1 = model[0].dt1.Clone();
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["SNO"] = i + 1;
                                    dr["ASSET_NO"] = dt.Rows[i]["vch_num"].ToString();
                                    dr["BILL_DATE"] = dt.Rows[i]["Bill_Date"].ToString();
                                    //dr["Bill_Amount"] = dt.Rows[i]["icode"].ToString();
                                    dr["BILL_NO"] = dt.Rows[i]["Bill_No"].ToString();
                                    dr["ASSET_NAME"] = dt.Rows[i]["Asset_Name"].ToString();   //
                                    dr["PTU_DATE"] = dt.Rows[i]["Put_To_Use_Date"].ToString();  //
                                    dr["MINOR_HEAD"] = dt.Rows[i]["minor_head"].ToString();
                                    dr["MAJOR_HEAD"] = dt.Rows[i]["Major_Head"].ToString();
                                    dr["ASSET_CODE"] = dt.Rows[i]["gd_Asset_code"].ToString();
                                    dr["QUANTITY"] = dt.Rows[i]["Quantity"].ToString();
                                    dr["CAPEX_CO_ACT"] = dt.Rows[i]["Capex_Coact"].ToString();
                                    dr["CAPEX_IT"] = dt.Rows[i]["Capex_IT"].ToString();
                                    dr["LOCATION"] = dt.Rows[i]["Loc"].ToString();
                                    dr["SUB_AREA"] = dt.Rows[i]["Sub_Area"].ToString();
                                    dr["COST_CENTER"] = dt.Rows[i]["cost_Centre"].ToString();
                                    dr["DESCRIPTION"] = dt.Rows[i]["Description"].ToString();
                                    dr["WARRANTY"] = dt.Rows[i]["Warranty"].ToString();
                                    //dr["WDV_CO_ACT"] = dt.Rows[i]["iremark"].ToString();
                                    //dr["WDV_IT_ACT"] = dt.Rows[i]["iremark"].ToString();
                                    dr["LIFE_IN_YEARS"] = dt.Rows[i]["Life_In_Years"].ToString();
                                    dr["VALID_UP_TO"] = dt.Rows[i]["gd_Valid_to"].ToString();
                                    dr["REMARK"] = dt.Rows[i]["GD_remark"].ToString();

                                    model[0].dt1.Rows.Add(dr);
                                }
                                model[0].Col13 = "Update";
                                model[0].Col100 = "Update & New";

                                model[0].Col121 = "U";
                                model[0].Col122 = "<u>U</u>pdate";
                                model[0].Col123 = "Update & Ne<u>w</u>";
                                DataTable dtf = new DataTable();
                                dtf = sgen.getdata(userCode, "select rec_id,file_name,file_path,col1 from file_tab where" +
                                    " ref_code='" + dt.Rows[0]["vch_num"].ToString() + "' and type='FAE' and " +
                                    "client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
                                List<Tmodelmain> ltmf = new List<Tmodelmain>();
                                foreach (DataRow drf in dtf.Rows)
                                {
                                    Tmodelmain tmf = new Tmodelmain();
                                    tmf.Col1 = drf["file_name"].ToString();
                                    tmf.Col2 = drf["col1"].ToString();
                                    tmf.Col3 = drf["rec_id"].ToString();
                                    ltmf.Add(tmf);
                                }
                                model[0].LTM1 = ltmf;
                            }
                            break;
                        case "PARTY":
                            mq = "select distinct a.c_name as name,b.country_name as country,a.addr,a.vch_num,a.tor from clients_mst a" +
                                " inner join country_state b on a.country = b.alpha_2 and" +
                                " a.client_unit_id = b.client_unit_id where(a.client_id || a.client_unit_id || a.vch_num || to_char(a.vch_date, 'yyyymmdd') || a.type) = '" + URL + "' ";
                            DataTable dtt = new DataTable();
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                model[0].Col39 = dtt.Rows[0]["name"].ToString();
                                model[0].Col40 = dtt.Rows[0]["vch_num"].ToString();
                                model[0].Col41 = dtt.Rows[0]["addr"].ToString();
                                model[0].Col42 = dtt.Rows[0]["country"].ToString();
                                // model[0].Col52 = dtt.Rows[0]["tor"].ToString();
                            }
                            break;

                        case "FDEL":
                            string fid = sgen.GetSession(MyGuid, "delid").ToString();
                            res = sgen.execute_cmd(userCode, "Delete from file_tab WHERE rec_id='" + fid + "' and type='AMC' and client_id='"
                               + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
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

                        case "ITEM":
                            //mq = "select vm.col1 as Asset_Name,vm.vch_num,vm.col2 as Bill_No,to_char(convert_tz(vm.date2, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Bill_Date," +
                            //    "to_char(convert_tz(vm.date1, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Put_To_Use_Date,vm.col6 as Asset_code,vm.col28 as Capex_Coact," +
                            //    "vm.col33 as Capex_IT,vm.col18 as Life_In_Years,vm.col12 as Location,vm.col13 as Sub_Area,vm.col25 as Description," +
                            //    "vm.col8 as Bill_Amt_Company_curr,vm.col9 as Bill_Amt_Foreign_curr,vm.col15 as Asset_description,vm.col17 as Warranty," +
                            //    "to_char(convert_tz(vm.date3, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Warrenty_Expires_on," +
                            //    "mnr.col1 as minor_head,vm.col29 as Quantity,cc.master_name as cost_Centre,ms.master_name as Major_Head" +
                            //    ",c.master_name as Currency from vehicle_master vm inner join enx_tab mnr on vm.col4 = mnr.vch_num and mnr.type = 'AMH' and " +
                            //    "mnr.client_id = vm.client_id inner join master_setting cc on vm.col14 = cc.master_id and cc.type = 'FAC' AND cc.client_id = vm.client_id " +
                            //    "and cc.client_unit_id = vm.client_unit_id inner join master_setting c on c.master_id = vm.col10 and c.type = 'CTM' and " +
                            //    "c.client_id = vm.client_id and c.client_unit_id = vm.client_unit_id " +
                            //    "inner join master_setting ms on ms.master_id = mnr.col2 and ms.type = 'MAH' and ms. client_id = mnr.client_id and ms.client_unit_id = mnr.client_unit_id " +
                            //    "where (vm.client_id || vm.client_unit_id || vm.vch_num || to_char(vm.vch_date, 'yyyymmdd')|| vm.type) in ('" + URL + "')";

                            mq = "select vm.col1 as Asset_Name,vm.vch_num,vm.col2 as Bill_No,to_char(vm.date2, '" + sgen.Getsqldateformat() + "') as Bill_Date," +
                                "to_char(vm.date1, '" + sgen.Getsqldateformat() + "') as Put_To_Use_Date,vm.col6 as Asset_code,vm.col28 as Capex_Coact,vm.col33 as Capex_IT," +
                                "vm.col18 as Life_In_Years,vm.col13 as Sub_Area,vm.col25 as Description,vm.col8 as Bill_Amt_Company_curr,vm.col9 as Bill_Amt_Foreign_curr,vm.col15 as " +
                                "Asset_description,vm.col17 as Warranty,(a.client_id || a.client_unit_id || b.master_id || f.master_id || rm.master_id || rk.master_id || a.master_id) as " +
                                "Loc_id,(b.master_name || ct.param1 || f.master_name || ct.param1 || rm.master_name || ct.param1 || rk.master_name || ct.param1 || a.master_name) Location," +
                                "to_char(vm.date3, '" + sgen.Getsqldateformat() + "') as Warrenty_Expires_on,mnr.col1 as minor_head,vm.col29 as Quantity,cc.master_name as cost_Centre," +
                                "nvl(ms.master_name,'-') as Major_Head,c.master_name as Currency from master_setting a inner join master_setting b on b.master_id = a.classid and b.type = 'HBM' and " +
                                "a.client_unit_id = b.client_unit_id inner join master_setting f on f.master_id = a.sectionid and f.type = 'IN0' and " +
                                "a.client_unit_id = f.client_unit_id inner join master_setting rm on rm.master_id = a.client_name and rm.type = 'IN1' and " +
                                "a.client_unit_id = rm.client_unit_id inner join master_setting rk on rk.master_id = a.subjectid and rk.type = 'IN2' and " +
                                "a.client_unit_id = rk.client_unit_id inner join controls ct on 1 = 1 and ct.id = '000010' " +
                                "inner join vehicle_master vm on vm.col12 = (a.client_id || a.client_unit_id || b.master_id || f.master_id || rm.master_id || rk.master_id || a.master_id) " +
                                "and a.type = 'IN3' inner join enx_tab mnr on vm.col4 = mnr.vch_num and mnr.type = 'AMH' inner join master_setting cc on " +
                                "vm.col14 = cc.master_id and cc.type = 'FAC' and cc.client_unit_id = vm.client_unit_id inner join master_setting c on " +
                                "c.master_id = vm.col10 and c.type = 'CTM' and c.client_unit_id = vm.client_unit_id left join master_setting ms on " +
                                "ms.master_id = mnr.col2 and ms.type = 'MAH' and ms.client_unit_id = mnr.client_unit_id " +
                                "where(vm.client_id || vm.client_unit_id || vm.vch_num || to_char(vm.vch_date, 'yyyymmdd') || vm.type) in ('" + URL + "')";
                            
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                if (model[0].dt1.Rows[0]["Add_Asset"].ToString().Trim().Equals("-")) { model[0].dt1.Rows.RemoveAt(0); }

                                model[0].dt1 = new DataTable();
                                model[0].dt1 = ((DataTable)sgen.GetSession(MyGuid, "BEX_DT")).Clone();
                                model[0].dt1 = model[0].dt1.Clone();
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["SNO"] = i + 1;
                                    dr["ASSET_NO"] = dt.Rows[i]["vch_num"].ToString();
                                    dr["BILL_DATE"] = dt.Rows[i]["Bill_Date"].ToString();
                                    //dr["Bill_Amount"] = dt.Rows[i]["icode"].ToString();
                                    //dr["SNO"] = dt.Rows[i]["cpartno"].ToString();
                                    dr["BILL_NO"] = dt.Rows[i]["Bill_No"].ToString();
                                    dr["ASSET_NAME"] = dt.Rows[i]["Asset_Name"].ToString();   //
                                    dr["PTU_DATE"] = dt.Rows[i]["Put_To_Use_Date"].ToString();  //
                                    dr["MINOR_HEAD"] = dt.Rows[i]["minor_head"].ToString();
                                    dr["MAJOR_HEAD"] = dt.Rows[i]["Major_Head"].ToString();
                                    //dr["ASSET_CODE"] = dt.Rows[i]["iremark"].ToString();
                                    dr["QUANTITY"] = dt.Rows[i]["Quantity"].ToString();
                                    dr["CAPEX_CO_ACT"] = dt.Rows[i]["Capex_Coact"].ToString();
                                    dr["CAPEX_IT"] = dt.Rows[i]["Capex_IT"].ToString();
                                    dr["LOCATION"] = dt.Rows[i]["Location"].ToString();
                                    dr["SUB_AREA"] = dt.Rows[i]["Sub_Area"].ToString();
                                    dr["COST_CENTER"] = dt.Rows[i]["cost_Centre"].ToString();
                                    dr["DESCRIPTION"] = dt.Rows[i]["Description"].ToString();
                                    dr["WARRANTY"] = dt.Rows[i]["Warranty"].ToString();
                                    //dr["WDV_CO_ACT"] = dt.Rows[i]["iremark"].ToString();
                                    //dr["WDV_IT_ACT"] = dt.Rows[i]["iremark"].ToString();
                                    dr["LIFE_IN_YEARS"] = dt.Rows[i]["Life_In_Years"].ToString();
                                    //dr["REMARK"] = dt.Rows[i]["iremark"].ToString();

                                    model[0].dt1.Rows.Add(dr);
                                }
                                model[0].dt1.AcceptChanges();
                            }
                            break;
                    }
                    break;
                #endregion

                #region ins_rev
                case "ins_rev":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select et.client_id,et.client_unit_id,et.ent_by,et.ent_date,et.edit_by,et.edit_date,et.type,et.vch_num,vm.col1 as Asset_Name,vm.vch_num as Asset_no," +
                                "et.col2 as Remark,et.col3 as Rev_Amt,vm.col6 as Asset_code," +
                                "to_char(vm.date1, '" + sgen.Getsqldateformat() + "') as Put_To_Use_Date,cc.master_name as cost_Centre," +
                                "to_char(et.date2, '" + sgen.Getsqldateformat() + "') as Rev_Date from enx_tab et" +
                                " inner join vehicle_master vm on vm.vch_num = et.col4 " +
                                "and vm.type = 'FAE' inner join master_setting cc on vm.col14 = cc.master_id and cc.type = 'FAC' and " +
                                "cc.client_unit_id = vm.client_unit_id " +
                                "where(et.client_id || et.client_unit_id || et.vch_num || to_char(et.vch_date, 'yyyymmdd') || et.type) = '" + URL + "'";
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
                                model[0].Col8 = "(client_id||client_unit_id||vch_num|| to_char(vch_date, 'yyyymmdd')||type) = '" + URL + "'";
                                model[0].Col10 = tm.Col10;
                                model[0].Col11 = tm.Col11;
                                model[0].Col12 = dt.Rows[0]["type"].ToString();
                                model[0].Col16 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dt.Rows[0]["Rev_Date"].ToString();            //Bill Amount
                                model[0].Col18 = dt.Rows[0]["Remark"].ToString();             //Remark

                                model[0].dt1 = model[0].dt1.Clone();
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["SNO"] = i + 1;
                                    dr["ASSET_NO"] = dt.Rows[i]["Asset_no"].ToString();
                                    dr["ASSET_NAME"] = dt.Rows[i]["Asset_Name"].ToString();   //
                                    dr["PTU_DATE"] = dt.Rows[i]["Put_To_Use_Date"].ToString();  //
                                    //dr["ASSET_CODE"] = dt.Rows[i]["gd_Asset_code"].ToString();
                                    dr["COST_CENTER"] = dt.Rows[i]["cost_Centre"].ToString();
                                    dr["REVALUATION_AMOUNT"] = dt.Rows[i]["Rev_Amt"].ToString();
                                    //dr["WDV_CO_ACT"] = dt.Rows[i]["iremark"].ToString();
                                    //dr["WDV_IT_ACT"] = dt.Rows[i]["iremark"].ToString();
                                    //dr["REMARK"] = dt.Rows[i]["GD_remark"].ToString();

                                    model[0].dt1.Rows.Add(dr);
                                }
                                model[0].Col13 = "Update";
                                model[0].Col100 = "Update & New";
                            }


                            break;
                        case "ITEM":
                            mq = "select vm.col1 as Asset_Name,vm.vch_num as Asset_no,vm.col6 as Asset_code,to_char(convert_tz(vm.date1, 'UTC', '+05:30'), 'dd/MM/YYYY') as " +
                                "Put_To_Use_Date,cc.master_name as cost_Centre from vehicle_master vm inner join master_setting cc on vm.col14 = cc.master_id and" +
                                " cc.type = 'FAC' AND cc.client_id = vm.client_id and cc.client_unit_id = vm.client_unit_id " +
                                "where (vm.client_id || vm.client_unit_id || vm.vch_num || to_char(vm.vch_date, 'yyyymmdd') || vm.type) in ('" + URL + "')";

                            dt = sgen.getdata(userCode, mq);

                            if (dt.Rows.Count > 0)
                            {
                                model[0].dt1 = new DataTable();
                                model[0].dt1 = ((DataTable)sgen.GetSession(MyGuid, "BEX_DT")).Clone();
                                model[0].dt1 = model[0].dt1.Clone();

                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["SNO"] = i + 1;
                                    dr["ASSET_NO"] = dt.Rows[i]["Asset_no"].ToString();
                                    dr["ASSET_NAME"] = dt.Rows[i]["Asset_Name"].ToString();   //
                                    dr["PTU_DATE"] = dt.Rows[i]["Put_To_Use_Date"].ToString();  //
                                    dr["COST_CENTER"] = dt.Rows[i]["cost_Centre"].ToString();   //

                                    //dr["BILL_DATE"] = dt.Rows[i]["Bill_Date"].ToString();
                                    //dr["Bill_Amount"] = dt.Rows[i]["icode"].ToString();
                                    //dr["SNO"] = dt.Rows[i]["cpartno"].ToString();

                                    //dr["ASSET_CODE"] = dt.Rows[i]["iremark"].ToString();


                                    model[0].dt1.Rows.Add(dr);
                                }
                                model[0].dt1.AcceptChanges();
                            }

                            break;

                    }
                    break;
                #endregion

                #region Joining Age Master
                case "age_mst":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            #region Gender
                            mod1.Add(new SelectListItem { Text = "Male", Value = "001" });
                            mod1.Add(new SelectListItem { Text = "Female", Value = "002" });
                            TempData[MyGuid + "_TList1"] = mod1;

                            #endregion
                            mq = "select master_id,vch_num,master_name,classid,master_entby,master_entdate,vch_date,Status," +
                                "client_id,client_unit_id from master_setting where " +
                                "(client_id|| client_unit_id || vch_num || to_char(vch_date, 'yyyymmdd') || type)= '" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col1 = dt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dt.Rows[0]["master_entby"].ToString();
                                model[0].Col4 = dt.Rows[0]["master_entdate"].ToString();
                                model[0].Col16 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col8 = "(client_id|| client_unit_id|| vch_num|| to_char(vch_date, 'yyyymmdd')|| type) = '" + URL + "'";
                                model[0].Col12 = tm.Col12;
                                model[0].Col10 = tm.Col10;
                                model[0].TList1 = mod1;
                                String[] L1 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["master_name"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems1 = L1;
                                model[0].Col17 = dt.Rows[0]["classid"].ToString();
                                model[0].Col19 = dt.Rows[0]["master_name"].ToString();
                            }
                            model[0].Col13 = "Update";
                            model[0].Col20 = "disabled";
                            break;
                    }
                    break;
                #endregion

                #region Salary Grade
                case "salary_grade":

                    switch (btnval)
                    {
                        case "VIEW":
                        case "EDIT":

                            #region Grade 1
                            dt = sgen.getdata(userCode, "select master_id,master_name from master_setting where type='KGM' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
                            if (dt.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    mod1.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                                }
                                TempData[MyGuid + "_Tlist1"] = mod1;
                            }
                            #endregion

                            #region Department 2
                            dt = sgen.getdata(userCode, "SELECT master_id,master_name FROM master_setting WHERE Type='MDP'");
                            if (dt.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    mod2.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                                }
                                TempData[MyGuid + "_Tlist2"] = mod2;
                            }
                            #endregion

                            #region Department 3
                            dt = sgen.getdata(userCode, "select master_id,master_name from master_setting where type='MDG'");
                            if (dt.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    mod3.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                                }
                                TempData[MyGuid + "_Tlist3"] = mod3;
                            }
                            #endregion

                            mq = "select GROUP_CONCAT(DISTINCT col1) dept,col2,GROUP_CONCAT(DISTINCT col3) desig,ent_by,ent_date,vch_num,client_id,client_unit_id from " +
                                  "enx_tab where (client_id|| client_unit_id|| vch_num|| to_char(vch_date, 'yyyymmdd')|| type)='" + URL + "' group by col2,ent_by,ent_date,vch_num," +
                                  "client_id,client_unit_id";

                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col1 = dt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dt.Rows[0]["ent_date"].ToString();
                                model[0].Col16 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col8 = "(client_id|| client_unit_id|| vch_num|| to_char(vch_date, 'yyyymmdd')|| type) = '" + URL + "'";
                                model[0].Col12 = tm.Col12;
                                model[0].Col10 = tm.Col10;
                                model[0].TList1 = mod1;
                                model[0].TList2 = mod2;
                                model[0].TList3 = mod3;
                                String[] L1 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["col2"].ToString()).Distinct()).Split(',');
                                String[] L2 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["dept"].ToString()).Distinct()).Split(',');
                                String[] L3 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["desig"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems1 = L1;
                                model[0].SelectedItems2 = L2;
                                model[0].SelectedItems3 = L3;
                                model[0].Col10 = tm.Col10;
                                model[0].Col11 = tm.Col11;
                                model[0].Col12 = tm.Col12;

                            }
                            model[0].Col13 = "Update";

                            break;
                    }
                    break;
                    #endregion

            }
            return model;
        }
        #endregion//callback

        //====================================== Make Query ===========================================================

        #region make query
        public void Make_query(string viewname, string title, string btnval, string seektype, string Myguid = "")
        {
            FillMst(Myguid);

            btnval = btnval.ToUpper();
            sgen.SetSession(MyGuid, "btnval", btnval);
            string cmd = "";

            switch (viewname.ToLower())
            {

                #region pv_stmt

                case "pv_stmt":
                    switch (btnval.ToUpper())
                    {

                        case "EDIT":
                        case "VIEW":
                            cmd = @"select (i.client_id||i.client_unit_id||i.vch_num||to_char(i.vch_date,'yyyymmdd')||i.type) as fstr," +
                          "to_char(i.vch_date, '" + sgen.Getsqldateformat() + "') as Verification_date," +
                          "d.master_name as Dept_name,dg.master_name as desig,i.icode as Icode,i.iname as Item_Name,i.cpartno Partno,i.qtystk Qty_In_Stock,i.qtychl Qty_Phy," +
                          "i.qtyin Qty_In,i.qtyout Qty_Out from itransaction i " +
                          "inner join master_setting d on d.master_id=i.deptno and d.type='MDP' " +
                          "inner join master_setting dg on  dg.master_id  = i.desig and dg.type='MDG' " +
                          "where i.type in ('" + type + "') and i.client_unit_id='" + unitid_mst + "'";

                            break;

                        case "ITEM":

                            //if (param1 != "") where = " and (it.client_id||it.client_unit_id||it.icode||to_char(it.vch_date,'yyyymmdd')||it.type) not in ('" + param1.Trim().TrimEnd(',').Replace(",", "','") + "')";
                            cmd = "select DISTINCT (it.client_id||it.client_unit_id||it.icode||to_char(it.vch_date,'yyyymmdd')||it.type) fstr,it.icode as Icode,it.iname as Iname,it.cpartno Partno," +
                                "um.master_name as UOM,s.closing from item it " +
                                "inner join master_setting um on um.master_id=it.uom and um.type='UMM' and um.client_unit_id='" + unitid_mst + "' " +
                                "left join (select (nvl(t.client_unit_id,'0')||i.icode) as fstr,i.icode,i.iname as Item_Name,sum(nvl(t.qtyin,0)) as Received,sum(nvl(t.qtyout,0)) as Issued," +
                                "sum(nvl(t.qtyin,0))-sum(nvl(t.qtyout,0)) as closing from item i left join itransaction t on t.icode=i.icode and T.client_unit_id='" + unitid_mst + "' where " +
                                "length(i.icode)>4 group by i.icode,t.icode,t.client_unit_id,i.iname order by t.icode) s on it.icode=s.icode " +
                                "where it.type='IT' and it.client_unit_id='" + unitid_mst + "'" + where + "";

                            //cmd = "select distinct concat(vm.client_id, vm.client_unit_id, vm.vch_num, to_char(vm.vch_date, 'yyyymmdd')|| vm.type) as fstr,vm.col1 as party_name,vm.col2 as Bill_No,to_number(convert_tz(vm.date1, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Bill_Date,to_char(convert_tz(vm.date2, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Bill_Date as Put_To_Use_Date,vm.col6 as Asset_code,vm.col7 as Life_In_Years,vm.col10 as Bill_Amt_Company_curr,vm.col8 as Bill_Amt_Foreign_curr,vm.col13 as Sub_Area,vm.col15 as Asset_description,vm.col17 as Warranty_If_Any," +
                            //    "to_char(convert_tz(vm.date3, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Bill_Date as Warrenty_Expires_on,mnr.col1 as minor_head,ggs.col6 as Govt_Grant_Scheme,cc.master_name as cost_Centre,c.master_name as Currency,vm.col5 as Asset_Head_Major FROM vehicle_master vm   " +
                            //    "inner join enx_tab mnr on vm.col4 = mnr.vch_num and mnr.type = 'AMH' and mnr.client_id = vm.client_id  " +
                            //    " inner join  enx_tab ggs on vm.col11 = ggs.vch_num and ggs.type = 'GGM' and ggs.client_id = vm.client_id and ggs.client_unit_id = vm.client_unit_id inner join master_setting cc on vm.col14 = cc.master_id and cc.type = 'FAC' AND cc.client_id = vm.client_id and cc.client_unit_id = vm.client_unit_id " +
                            //    "inner join master_setting c on c.master_id = vm.col10 and c.type = 'CTM' and c.client_id = vm.client_id and c.client_unit_id = vm.client_unit_id where vm.client_id = '" + clientid_mst + "' and vm.client_unit_id ='" + unitid_mst + "' and vm.type = 'FAE'";

                            cmd = "select distinct (vm.client_id || vm.client_unit_id || vm.vch_num || to_char(vm.vch_date, 'yyyymmdd')|| vm.type) as fstr," +
                                "a.c_name Name,vm.col28 as capax_amt, vm.col36 as Grant_amt, a.country,vm.col1 as Asset_Name,vm.col2 as Bill_No,to_char(vm.date2, '" + sgen.Getsqldateformat() + "') as Bill_Date," +
                                "to_char(vm.date1, '" + sgen.Getsqldateformat() + "') as Put_To_Use_Date,vm.col6 as Asset_code,vm.col7 as" +
                                " Life_In_Years,vm.col8 as Bill_Amt_Company_curr,vm.col9 as Bill_Amt_Foreign_curr,vm.col13 as Sub_Area,vm.col15 as Asset_description," +
                                "vm.col17 as Warranty_If_Any,to_char(vm.date3, '" + sgen.Getsqldateformat() + "') as Warrenty_Expires_on,mnr.col1 " +
                                "as minor_head,ggs.col6 as Govt_Grant_Scheme,cc.master_name as cost_Centre,c.master_name as Currency,vm.col5 as Asset_Head_Major " +
                                "FROM vehicle_master vm inner join clients_mst a on a.client_unit_id=vm.client_unit_id and a.vch_num = vm.col30 and a.type = 'BCD'" +
                                " inner join enx_tab mnr on vm.col4 = mnr.vch_num and mnr.type = 'AMH' inner join  enx_tab ggs on vm.col11 = ggs.vch_num and ggs.type = 'GGM' and ggs.client_unit_id = vm.client_unit_id inner join master_setting cc on vm.col14 = cc.master_id and cc.type = 'FAC' AND" +
                                " cc.client_unit_id = vm.client_unit_id inner join master_setting c on c.master_id = vm.col10 and " +
                                "c.type = 'CTM' and c.client_unit_id = vm.client_unit_id where vm.client_unit_id = '" + unitid_mst + "' and vm.type = 'FAE' ";


                            break;

                    }

                    break;

                #endregion
                #region  fam_master
                case "fam_master":
                    switch (btnval)
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = "select(ms.client_id || ms.client_unit_id || ms.vch_num || to_char(ms.vch_date, 'yyyymmdd') || ms.type) as fstr, bg.master_name as Building,ex.col4 as Block,ms.subjectid as Area,b.First_name || ' ' || b.middle_name || ' ' || b.last_name as Ent_By, " +
                                " (CASE when ms.status = 'Y' THEN 'Active'  else 'Inactive' end) as Status from master_setting ms" +
                                " inner join master_setting bg on bg.master_id = ms.classid and bg.type = 'HBM' and bg.client_unit_id = ms.client_unit_id  " +
                                "inner join(select DISTINCT vch_num, col4 from enx_tab where type= 'HBM' and client_unit_id = '" + unitid_mst + "') ex on ex.vch_num = ms.sectionid " +
                                "inner join user_details b on ms.master_entby = b.vch_num and b.type = 'CPR' where ms.type in ('FAL', 'DDFAL')  and ms.client_unit_id = '" + unitid_mst + "'";

                            break;
                    }
                    break;
                #endregion
                #region asset_ent
                case "asset_ent":
                    switch (btnval)
                    {
                        case "EDIT":
                        case "VIEW":
                            //cmd = "select distinct (vm.client_id || vm.client_unit_id || vm.vch_num || to_char(vm.vch_date, 'yyyymmdd')|| vm.type) as fstr," +
                            //    "a.c_name Vendor_Name,a.country,vm.col28 as capax_amt, vm.col36 as Grant_amt, vm.col1 as Asset_Name,vm.col2 as Bill_No,to_char(vm.date2, '" + sgen.Getsqldateformat() + "')" +
                            //    " as Bill_Date,to_char(vm.date1, '" + sgen.Getsqldateformat() + "') as Put_To_Use_Date,vm.col6 as Asset_code,vm.col7 as" +
                            //    " Life_In_Years,vm.col8 as Bill_Amt_Company_curr,vm.col9 as Bill_Amt_Foreign_curr,vm.col13 as Sub_Area,vm.col15 as Asset_description," +
                            //    "vm.col17 as Warranty_If_Any,to_char(vm.date3, '" + sgen.Getsqldateformat() + "') as Warrenty_Expires_on,mnr.col1 " +
                            //    "as minor_head,ggs.col6 as Govt_Grant_Scheme,cc.master_name as cost_Centre,c.master_name as Currency,vm.col5 as Asset_Head_Major " +
                            //    "FROM vehicle_master vm inner join clients_mst a on a.client_unit_id=vm.client_unit_id and a.vch_num = vm.col30 and a.type = 'BCD'" +
                            //    " inner join enx_tab mnr on vm.col4 = mnr.vch_num and mnr.type = 'AMH' inner join  enx_tab ggs on vm.col11 = ggs.vch_num and ggs.type = 'GGM' " +
                            //    "and ggs.client_unit_id = vm.client_unit_id inner join master_setting cc on vm.col14 = cc.master_id and cc.type = 'FAC' AND" +
                            //    " cc.client_unit_id = vm.client_unit_id inner join master_setting c on c.master_id = vm.col10 and " +
                            //    "c.type = 'CTM' and c.client_unit_id = vm.client_unit_id where vm.client_unit_id = '" + unitid_mst + "' and vm.type = 'FAE' ";

                            cmd = "select distinct (vm.client_id || vm.client_unit_id || vm.vch_num || to_char(vm.vch_date, 'yyyymmdd')|| vm.type) as fstr,a.vch_num Vendor_Code, a.c_name Vendor_Name, " +
                                "a.country,vm.vch_num as Asset_code, vm.col1 as Asset_Name,vm.col2 as Bill_No,to_char(vm.date2, 'dd/mm/yyyy') as Bill_Date,to_char(vm.date1, 'dd/mm/yyyy') as Put_To_Use_Date,mnr.col1 as minor_head," +
                                "vm.col19 as Asset_Head_Major,vm.col8 as Bill_Amt_Actt_cur,vm.col29 as quantity,vm.col28 as capax_amt,vm.col33 as capax_it," +
                                "vm.col9 as Bill_Amt_Foreign_curr,c.master_name as Currency,vm.col7 as Life_In_Years,vm.col13 as Sub_Area,vm.col25 as Asset_description,cc.master_name as cost_Centre,vm.col17 as Warranty_If_Any," +
                                "to_char(vm.date3, 'dd/mm/yyyy') as Warrenty_Exp_on,ggs.col6 as Govt_Grant_Scheme, vm.col36 as Grant_amt, vm.col3 as comp_act_rate,vm.col5 as inc_tax_rate,vm.col6 as salvage_in_per,vm.col15 as salvage_abs_amt,vm.col18 as life_of_asset," +
                                "vm.col37 as calibration_req FROM vehicle_master vm left join clients_mst a on a.client_unit_id = vm.client_unit_id and a.vch_num = vm.col30 and a.type = 'BCD' left join enx_tab mnr on vm.col4 = mnr.vch_num " +
                                "and mnr.type = 'AMH' inner join enx_tab ggs on vm.col11 = ggs.vch_num and ggs.type = 'GGM' and ggs.client_unit_id = vm.client_unit_id left join master_setting cc on vm.col14 = cc.master_id and cc.type = 'FAC' " +
                                "AND cc.client_unit_id = vm.client_unit_id left join master_setting c on c.master_id = vm.col10 and c.type = 'CTM' and c.client_unit_id = vm.client_unit_id where  vm.client_unit_id = '" + unitid_mst + "' and vm.type = 'FAE'";
                            break;

                        case "PARTY":
                            cmd = "select distinct (a.vch_num || to_char(a.vch_date, 'yyyymmdd') || a.type) as fstr" +
                                ",a.c_name as name,b.country_name as country,a.addr as Address,a.vch_num,a.pincode Pincode,a.c_gstin as GSTIN," +
                                "(case when a.tor = 'R' then 'Regular' else 'Composition' END) GSTType,a.cpemail as Email from clients_mst a inner join country_state b on " +
                                "a.country = b.alpha_2 and a.client_unit_id = b.client_unit_id where a.type = 'BCD' and substr(a.vch_num,0,3)='203' and " +
                                "find_in_set(a.client_unit_id, '" + unitid_mst + "')=1 and b.type = 'CST'";
                            break;

                    }
                    break;

                #endregion
                #region cost_center
                case "cost_center":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = "select (ms.client_id||ms.client_unit_id||ms.vch_num||to_char(ms.vch_date,'ddmmyyyy') || ms.type) as fstr,ms.master_id as ID,ms.master_name" +
                                " as Cost_Centre,ms1.master_name as Department,(b.First_name || ' ' || Replace(b.middle_name,'0','') || ' ' || b.last_name) as Ent_By," +
                                "to_char(convert_tz(ms.master_EntDate, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Ent_Date from master_setting ms " +
                                "inner join master_setting ms1 on ms1.master_id = ms.classid and ms1.type = 'MDP' inner join user_details b " +
                                "on ms.master_entby = b.vch_num and b.type = 'CPR' where ms.type = 'FAC' and ms.client_unit_id = '" + unitid_mst + "'";
                            break;
                    }
                    break;
                #endregion
                #region ASSET WRITE OFF
                case "asset_write":
                    switch (btnval)
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = "select distinct (vm.client_id||vm.client_unit_id|| vm.vch_num|| to_char(vm.vch_date, 'ddmmyyyy')|| vm.type) as fstr,vm.vch_num As Doc_no, " +
                                "to_char( vm.date1, '" + sgen.Getsqldateformat() + "') as Date_sale," +
                                "vm.col3 as WDV,vm.col4 as Profit_Loss_On_Sale,b.Asset_Name,b.quantity from vehicle_master vm " +
                                "inner join (select distinct (vm.client_id || vm.client_unit_id || vm.vch_num || to_char(vm.vch_date, 'yyyymmdd')|| vm.type) as fstr,vm.vch_num as Asset_code, vm.col1 as Asset_Name,vm.col2 as Bill_No,to_char(vm.date2, 'dd/mm/yyyy') as Bill_Date,vm.col29 as quantity" +
                                ",cc.master_name as cost_Centre FROM vehicle_master vm left join master_setting cc on vm.col14 = cc.master_id and cc.type = 'FAC' AND cc.client_unit_id = vm.client_unit_id where vm.client_unit_id = '" + unitid_mst + "' and vm.type = 'FAE') b on b.Asset_code=vm.col1 " +
                                "where vm.type = 'AWO' and vm.client_unit_id = '" + unitid_mst + "'";
                            break;
                        case "ASSET":
                            cmd = "select distinct (vm.client_id || vm.client_unit_id || vm.vch_num || to_char(vm.vch_date, 'yyyymmdd')|| vm.type) as fstr,vm.vch_num as Asset_code, vm.col1 as Asset_Name,vm.col2 as Bill_No,to_char(vm.date2, 'dd/mm/yyyy') as Bill_Date,vm.col29 as quantity" +
                                ",cc.master_name as cost_Centre FROM vehicle_master vm left join master_setting cc on vm.col14 = cc.master_id and cc.type = 'FAC' AND cc.client_unit_id = vm.client_unit_id where vm.client_unit_id = '" + unitid_mst + "' and vm.type = 'FAE'";
                            break;
                            //case "AREA":

                            //    cmd = @" select (ms.client_id|| ms.client_unit_id||ms.vch_num|| to_char(ms.vch_date, 'ddmmyyyy')||ms.type) as fstr,ms.subjectid as area,b.master_name as building,ex.col4 as block from master_setting ms 
                            //    inner join master_setting b on b.master_id = ms.classid and b.type = 'HBM' 
                            //    inner join(select DISTINCT vch_num, col4 from enx_tab where type= 'HBM' and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "') ex on ex.vch_num = ms.sectionid " +
                            //   "where ms.client_id = '" + clientid_mst + "' and ms.client_unit_id = '" + unitid_mst + "' and ms.type = 'FAL'";
                            //    break;


                    }
                    break;

                #endregion
                #region asset_sale
                case "asset_sale":
                    switch (btnval)
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = "select distinct (vm.client_id||vm.client_unit_id|| vm.vch_num|| to_char(vm.vch_date, 'ddmmyyyy')|| vm.type) as fstr,vm.vch_num As Doc_no,b.Asset_code,b.Asset_Name,b.quantity," +
                                "to_char( vm.date1, '" + sgen.Getsqldateformat() + "') as Date_sale,vm.col3 as WDV," +
                                "vm.col4 as Sales_Basic_Amt,vm.col5 as Taxes,vm.col6 as Gross_Sales_Value,vm.col7 as Profit_Loss_On_Sale from vehicle_master vm inner join (select distinct (vm.client_id || vm.client_unit_id || vm.vch_num || to_char(vm.vch_date, 'yyyymmdd')|| vm.type) as fstr,vm.vch_num as Asset_code, vm.col1 as Asset_Name,vm.col2 as Bill_No,to_char(vm.date2, 'dd/mm/yyyy') as Bill_Date,vm.col29 as quantity" +
                                ",cc.master_name as cost_Centre FROM vehicle_master vm left join master_setting cc on vm.col14 = cc.master_id and cc.type = 'FAC' AND cc.client_unit_id = vm.client_unit_id where vm.client_unit_id = '" + unitid_mst + "' and vm.type = 'FAE') b on b.Asset_code=vm.col2 where vm.type = 'ASM' and vm.client_unit_id = '" + unitid_mst + "'";

                            break;

                        case "ASSET":
                            cmd = "select (vm.client_id || vm.client_unit_id || vm.vch_num || to_char(vm.vch_date, 'yyyymmdd')|| vm.type) as fstr,vm.vch_num as Asset_code, vm.col1 as Asset_Name,vm.col2 as Bill_No,to_char(vm.date2, 'dd/mm/yyyy') as Bill_Date,vm.col29 as quantity" +
                                ",cc.master_name as cost_Centre FROM vehicle_master vm left join master_setting cc on vm.col14 = cc.master_id and cc.type = 'FAC' AND cc.client_unit_id = vm.client_unit_id where vm.client_unit_id = '" + unitid_mst + "' and vm.type = 'FAE'";
                            break;

                    }
                    break;

                #endregion
                #region  EPCG
                case "epcg_mst":
                    switch (btnval)
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = @"select (ex.client_id||ex.client_unit_id||ex.vch_num||to_char(ex.vch_date, 'ddmmyyyyy')||ex.type) as fstr,ex.col6 as EPCG_No,
                              to_char(ex.date1, '" + sgen.Getsqldateformat() + "') as EPCG_Date,col3 as EPCG_Amount," +
                              "to_char(ex.date2, '" + sgen.Getsqldateformat() + "') as Expiry_On," +
                              "ex.col7 as Benifit_Amt" +
                              ",(b.First_name||' '|| b.middle_name||' '|| b.last_name) as Ent_By,to_char(convert_tz(ex.ent_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Ent_Date,ex.col8 as Remark from enx_tab ex " +
                                "inner join user_details b on ex.ent_by = b.vch_num where ex.type = 'EPC' and ex.client_unit_id = '" + unitid_mst + "' order by ex.created_date desc";
                            break;
                    }
                    break;
                #endregion
                #region fa_m
                case "fa_m":

                    switch (btnval)
                    {
                        case "EDIT":
                        case "VIEW":

                            cmd = "select (ex.client_id|| ex.client_unit_id || ex.vch_num || to_char(ex.vch_date, 'yyyymmdd')||ex.type) as fstr,ex.col1 as Minor_Head," +
                                "ex.type_desc as Minor_Asset_Abbreviation,ex.col4 as Comp_Act_Rate,ex.col7 as Inc_Tax_Rate,ex.col3 as Salvage_Abs_Amt,ex.col8" +
                                " as Salvage_Value_Per,ex.col5 as Life_Of_Asset,ex.r_no as Cal_Method, " +
                                "(case When ex.col6 = 'Y' then 'Yes' else 'No' end ) as Caliberation_Req," +
                                "(case when ex.ref_code = 'S' then 'Simple' when ex.ref_code = 'D' then 'Double' when ex.ref_code = 'T'  then 'Triple' else 'Continuos' end) as Dep_Method,(b.First_name || ' ' || Replace(b.middle_name,'0','') || ' ' || b.last_name) as Ent_By, " +
                                "to_char(convert_tz(ex.Ent_Date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Ent_Date from enx_tab ex inner join" +
                                " user_details b on ex.ent_by = b.vch_num and b.type = 'CPR' inner join master_setting ms on ms.master_id = ex.col2 and ms.type = 'MAH' " +
                                "and ms.client_unit_id = ex.client_unit_id where ex.type = 'AMH' AND ex.client_unit_id = '" + unitid_mst + "'";


                            break;
                    }
                    break;

                #endregion
                #region  Goverment grant master
                case "goverment_grant":
                    switch (btnval)
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = "select (ex.client_id||ex.client_unit_id||ex.vch_num||to_char(ex.vch_date, 'yyyymmdd')||ex.type) as fstr,ex.col6 as Government_Scheme_Name,to_char(ex.date1, '" + sgen.Getsqldateformat() + "') as Grant_Date,col3 as Grant_Amount,to_char(ex.date2, '" + sgen.Getsqldateformat() + "') as Grant_Expiry, ex.col1 as Remark," +
                                "(b.First_name|| ' '|| b.middle_name|| ' '|| b.last_name) as Ent_By," +
                                "to_char(convert_tz(ex.ent_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Ent_Date from enx_tab ex " +
                                "inner join user_details b on ex.ent_by = b.vch_num where ex.type = '" + sgen.GetSession(MyGuid, "FA_TYPE_GGM") + "' and ex.client_unit_id = '" + unitid_mst + "' order by ex.created_date desc";

                            break;
                    }
                    break;
                #endregion
                #region  fa_major_head comp
                case "fa_major_head":
                    switch (btnval)
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = "SELECT(a.client_id || a.client_unit_id || a.master_id || to_char(a.vch_date, 'yyyymmdd') || a.type) as fstr, a.master_name as Major_Head , a.client_name as Major_Head_Abbrevation," +
                                "(CASE when a.status = 'Y' THEN 'Active'  when a.status = '' THEN 'Active' else 'Inactive' end) as Status," +
                                "to_char(convert_tz(a.master_entdate, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Ent_Date from master_setting a where a.type = 'MAH' and a.client_unit_id = '" + unitid_mst + "'";

                            break;
                    }
                    break;
                #endregion
                #region transfer_of_asset

                case "transfer_asset":
                    switch (btnval)
                    {
                        case "EDIT":
                        case "VIEW":
                            //cmd = "select (ex.client_id||ex.client_unit_id||ex.vch_num||to_char(ex.vch_date,'ddmmyyyy')||ex.type) as fstr, ex.date1,ex.col2 as Gross_Value,ex.col4 as WDV, ex.col1 as Sub_area,ex.col3 as remark  from enx_tab ex where ex.type = 'TOA' and ex.client_id = '" + clientid_mst + "' and ex.client_unit_id = '" + unitid_mst + "'";

                            cmd = "select (tsf.client_id||tsf.client_unit_id||tsf.vch_num|| to_char(tsf.vch_date, 'ddmmyyyy')||tsf.type) as fstr," +
                                "(b.master_name || ct.param1 || f.master_name || ct.param1 || rm.master_name || ct.param1 || rk.master_name || ct.param1 || a.master_name) Loc," +
                                "vm.rec_id,vm.col1 as Asset_Name,vm.col13 as Sub_Area, mnr.col1 as minor_head" +
                                ",to_char(vm.date2, '" + sgen.Getsqldateformat() + "') as Bill_Date,vm.col2 as Bill_No," +
                                "to_char(vm.date1, '" + sgen.Getsqldateformat() + "') as Put_To_Use_Date,vm.col6 as Asset_code," +
                                " tsf.vch_num as Doc_no,tsf.col2 as gross_vl,to_char(tsf.date2, '" + sgen.Getsqldateformat() + "') as Transfer_Date," +
                                "tsf.col3 as Wdv, tsf.col13 as Remark from master_setting a inner join master_setting b on b.master_id = a.classid" +
                                " and b.type = 'HBM' and a.client_unit_id = b.client_unit_id inner join master_setting f" +
                                " on f.master_id = a.sectionid and f.type = 'IN0' and a.client_unit_id = f.client_unit_id" +
                                " inner join master_setting rm on rm.master_id = a.client_name and rm.type = 'IN1' and a.client_unit_id = rm.client_unit_id inner join master_setting rk on rk.master_id = a.subjectid and rk.type = 'IN2' " +
                                "and a.client_unit_id = rk.client_unit_id inner join vehicle_master vm on " +
                                "vm.col12 = (a.client_id || a.client_unit_id || b.master_id || f.master_id || rm.master_id || rk.master_id || a.master_id) " +
                                "and vm.type = 'FAE' inner join enx_tab tsf on tsf.col8 = vm.rec_id and tsf.type = 'TOA' " +
                                "inner join enx_tab mnr on vm.col4 = mnr.vch_num and mnr.type = 'AMH' inner join controls ct on 1 = 1 and ct.id = '000010' where vm.client_unit_id = '" + unitid_mst + "' and tsf.type = 'TOA'";

                            break;

                        case "ASSETDESC":
                            //cmd = "select (vm.client_id||vm.client_unit_id||vm.vch_num||to_char(vm.vch_date,'ddmmyyyy')||vm.type) as fstr,vm.col1 as Asset_Name,vm.vch_num Doc_no," +
                            //    " mnr.col1 as minor_head,to_char(convert_tz(vm.date2, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Bill_Date,vm.col2 as Bill_No," +
                            //    "to_char(convert_tz(vm.date1, 'UTC', '"+sgen.Gettimezone()+"'), '"+sgen.Getsqldateformat()+"') as Put_To_Use_Date,vm.col6 as Asset_code from vehicle_master vm " +
                            //    "inner join enx_tab mnr on vm.col4 = mnr.vch_num and mnr.type = 'AMH' and mnr.client_id = vm.client_id " +
                            //    "where vm.type = 'FAE' and vm.client_id = '"+clientid_mst+"' and vm.client_unit_id = '"+unitid_mst+"'";

                            cmd = "select (vm.client_id||vm.client_unit_id||vm.vch_num|| to_char(vm.vch_date, 'ddmmyyyy')||vm.type) as fstr," +
                                  "(b.master_name || ct.param1 || f.master_name || ct.param1 || rm.master_name || ct.param1 || rk.master_name || ct.param1 || a.master_name) Loc," +
                                  "vm.rec_id,vm.col1 as Asset_Name,vm.col13 as Sub_Area,vm.vch_num Doc_no, mnr.col1 as minor_head," +
                                  "to_char(vm.date2, '" + sgen.Getsqldateformat() + "') as Bill_Date,vm.col2 as Bill_No," +
                                  "to_char(vm.date1, '" + sgen.Getsqldateformat() + "') as Put_To_Use_Date," +
                                  "vm.col6 as Asset_code from master_setting a inner join master_setting b on b.master_id = a.classid and b.type = 'HBM' " +
                                  "and a.client_unit_id = b.client_unit_id " +
                                  "inner join master_setting f on f.master_id = a.sectionid and f.type = 'IN0' and a.client_unit_id = f.client_unit_id " +
                                  "inner join master_setting rm on rm.master_id = a.client_name and rm.type = 'IN1' and a.client_unit_id = rm.client_unit_id " +
                                  "inner join master_setting rk on rk.master_id = a.subjectid and rk.type = 'IN2' and a.client_unit_id = rk.client_unit_id " +
                                  "inner join vehicle_master vm on vm.col12 = (a.client_id || a.client_unit_id || b.master_id || f.master_id || rm.master_id || rk.master_id || a.master_id) and vm.type = 'FAE' " +
                                  "inner join enx_tab mnr on vm.col4 = mnr.vch_num and mnr.type = 'AMH' inner join controls ct on 1 = 1 and ct.id = '000010' " +
                                  "where vm.type = 'FAE' and vm.client_unit_id = '" + unitid_mst + "'";

                            break;

                    }

                    break;
                #endregion
                #region AMC_Ent

                case "amc_ent":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":



                            cmd = "select distinct (kt.client_id || kt.client_unit_id || kt.vch_num || to_char(kt.vch_date, 'yyyymmdd')|| kt.type) as fstr,kt.rec_id,kt.type,kt.vch_num," +
                                "kt.col1 as Bil_no,to_char(kt.date1, '" + sgen.Getsqldateformat() + "') as Bill_Date,aa.c_name as Party_Name,bb.country_name as country,aa.addr as Address," +
                                "aa.vch_num as P_code,kt.col2 as Bill_Amount,kt.col3 as Remark,kt.col5 as GD_remark,to_char(kt.date2, '" + sgen.Getsqldateformat() + "') as gd_Valid_to," +
                                "kt.col6 as gd_Asset_code,vm.col1 as Asset_Name,vm.vch_num as Asset_No," +
                                "(b.master_name || ct.param1 || f.master_name || ct.param1 || rm.master_name || ct.param1 || rk.master_name || ct.param1 || a.master_name) Loc,vm.col2 as Bill_No," +
                                "to_char(vm.date2, '" + sgen.Getsqldateformat() + "') as Bil_Date,to_char(vm.date1, '" + sgen.Getsqldateformat() + "') as Put_To_Use_Date," +
                                "vm.col6 as Asset_code,vm.col28 as Capex_Coact,vm.col33 as Capex_IT,vm.col18 as Life_In_Years,vm.col13 as Sub_Area,vm.col25 as Description," +
                                "vm.col8 as Bill_Amt_Company_curr,vm.col9 as Bill_Amt_Foreign_curr,vm.col15 as Asset_description,vm.col17 as Warranty," +
                                "to_char(vm.date3, '" + sgen.Getsqldateformat() + "') as Expiring_On,mnr.col1 as minor_head,vm.col29 as Quantity,cc.master_name as cost_Centre," +
                                "c.master_name as Currency from master_setting a inner join master_setting b on b.master_id = a.classid and b.type = 'HBM' and " +
                                "a.client_unit_id = b.client_unit_id inner join master_setting f on f.master_id = a.sectionid and f.type = 'IN0' and " +
                                "a.client_unit_id = f.client_unit_id inner join master_setting rm on rm.master_id = a.client_name and rm.type = 'IN1' and " +
                                "a.client_unit_id = rm.client_unit_id inner join master_setting rk on rk.master_id = a.subjectid and rk.type = 'IN2' and " +
                                "a.client_unit_id = rk.client_unit_id inner join vehicle_master vm on vm.col12 = (a.client_id || a.client_unit_id || b.master_id || f.master_id || rm.master_id || rk.master_id || a.master_id)" +
                                " and a.type = 'IN3' inner join enx_tab mnr on vm.col4 = mnr.vch_num and mnr.type = 'AMH' inner join master_setting cc" +
                                " on vm.col14 = cc.master_id and cc.type = 'FAC' and cc.client_unit_id = vm.client_unit_id inner join master_setting c" +
                                " on c.master_id = vm.col10 and c.type = 'CTM' and c.client_unit_id = vm.client_unit_id inner join controls ct on 1 = 1" +
                                " and ct.id = '000010' inner join kc_tab kt  on kt.col6 = vm.vch_num inner join clients_mst aa on aa.vch_num = kt.col7 and aa.type = 'BCD' " +
                                "inner join country_state bb on aa.country = bb.alpha_2 and aa.client_unit_id = bb.client_unit_id inner join  vehicle_master vm on vm.vch_num = kt.col6 " +
                                "and vm.type = 'FAE' where kt.type = 'AMC' and kt.client_unit_id = '" + unitid_mst + "' ";

                            break;

                        case "PARTY":
                            cmd = "select distinct (a.vch_num || to_char(a.vch_date, 'yyyymmdd') || a.type) as fstr" +
                                   ",a.c_name as name,b.country_name as country,a.addr as Address,a.vch_num,a.pincode Pincode,a.c_gstin as GSTIN,(case when a.tor = 'R' then 'Regular' else 'Composition' END),'Unregistered') GSTType,a.cpemail as Email from clients_mst a inner join country_state b on " +
                                   "a.country = b.alpha_2 and a.client_unit_id = b.client_unit_id where a.type = 'BCD' and substr(a.vch_num,0,3)='203' and " +
                                   "find_in_set(a.client_unit_id, '" + unitid_mst + "')=1 and b.type = 'CST'";
                            break;
                        case "ITEM":
                            
                            cmd = "select distinct (vm.client_id || vm.client_unit_id || vm.vch_num || to_char(vm.vch_date, 'yyyymmdd')|| vm.type) as fstr,vm.col1 as Asset_Name,vm.vch_num,vm.col2 as Bill_No," +
                                "to_char(vm.date2, '" + sgen.Getsqldateformat() + "') as Bill_Date,to_char(vm.date1, '" + sgen.Getsqldateformat() + "') as Put_To_Use_Date," +
                                "vm.col6 as Asset_code,vm.col28 as Capex_Coact,vm.col33 as Capex_IT,vm.col18 as Life_In_Years,vm.col13 as Sub_Area,vm.col25 as Description," +
                                "vm.col8 as Bill_Amt_Company_curr,vm.col9 as Bill_Amt_Foreign_curr,vm.col15 as Asset_description,vm.col17 as Warranty," +
                                "(b.master_name || ct.param1 || f.master_name || ct.param1 || rm.master_name || ct.param1 || rk.master_name || ct.param1 || a.master_name) Location," +
                                "to_char(vm.date3, '" + sgen.Getsqldateformat() + "') as Warrenty_Expires_on,mnr.col1 as minor_head,vm.col29 as Quantity,cc.master_name as cost_Centre," +
                                "nvl(ms.master_name,'-') as Major_Head,c.master_name as Currency from master_setting a inner join master_setting b on b.master_id = a.classid and b.type = 'HBM' and " +
                                "a.client_unit_id = b.client_unit_id inner join master_setting f on f.master_id = a.sectionid and f.type = 'IN0' and " +
                                "a.client_unit_id = f.client_unit_id inner join master_setting rm on rm.master_id = a.client_name and rm.type = 'IN1' and " +
                                "a.client_unit_id = rm.client_unit_id inner join master_setting rk on rk.master_id = a.subjectid and rk.type = 'IN2' and " +
                                "a.client_unit_id = rk.client_unit_id inner join controls ct on 1 = 1 and ct.id = '000010' inner join vehicle_master vm on " +
                                "vm.col12 = (a.client_id || a.client_unit_id || b.master_id || f.master_id || rm.master_id || rk.master_id || a.master_id) and a.type = 'IN3' " +
                                "inner join enx_tab mnr on vm.col4 = mnr.vch_num and mnr.type = 'AMH' inner join master_setting cc on vm.col14 = cc.master_id " +
                                "and cc.type = 'FAC' and cc.client_unit_id = vm.client_unit_id inner join master_setting c on c.master_id = vm.col10 and" +
                                " c.type = 'CTM' and c.client_unit_id = vm.client_unit_id left join master_setting ms on ms.master_id = mnr.col2 and" +
                                " ms.type = 'MAH' and ms.client_unit_id = mnr.client_unit_id where vm.type = 'FAE' and " +
                                "vm.client_unit_id = '" + unitid_mst + "'";

                            break;
                    }
                    break;
                #endregion
                #region ins_rev

                case "ins_rev":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = "select (et.client_id || et.client_unit_id || et.vch_num || to_char(et.vch_date, 'yyyymmdd') || et.type) as fstr," +
                                "vm.col1 as Asset_Name,vm.vch_num as Asset_no,to_char(et.date2, '" + sgen.Getsqldateformat() + "') as Rev_Date,et.col2 as Remark,et.col3 as Rev_Amt,vm.col6 as Asset_code," +
                                "to_char(vm.date1, '" + sgen.Getsqldateformat() + "') as Put_To_Use_Date,cc.master_name as " +
                                "cost_Centre from enx_tab et " +
                                "inner join vehicle_master vm on vm.vch_num = et.col4 and vm.type = 'FAE' inner join master_setting cc on vm.col14 = cc.master_id " +
                                "and cc.type = 'FAC' and cc.client_unit_id = vm.client_unit_id where et.type = 'IOR' and " +
                                "et.client_unit_id = '" + unitid_mst + "'";
                            break;

                        case "ITEM":
                            cmd = "select (vm.client_id||vm.client_unit_id||vm.vch_num|| to_char(vm.vch_date, 'yyyymmdd')||vm.type) as fstr,vm.col1 as Asset_Name," +
                                "vm.vch_num as Asset_no,to_char(vm.date1, '" + sgen.Getsqldateformat() + "') as Put_To_Use_Date," +
                                  "cc.master_name as cost_Centre from vehicle_master vm inner join master_setting cc on vm.col14 = cc.master_id and cc.type = 'FAC' " +
                                  "AND cc.client_unit_id = vm.client_unit_id where vm.type = 'FAE' and vm.client_unit_id = '" + unitid_mst + "'";
                            break;
                    }
                    break;
                #endregion
                #region JOINING Age Master
                case "age_mst":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = "SELECT (a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr," +
                                "(case WHEN a.master_name = '001' THEN 'Male' else 'Female' END) as Gender," +
                                "a.Classid as Joining_Age,(b.First_name || ' ' || b.middle_name || ' ' || b.last_name) as Ent_By," +
                                "to_char(convert_tz(a.master_EntDate, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Ent_Date " +
                                "from master_setting a,user_details b " +
                                "where a.master_entby = b.vch_num and a.type = 'JAM' and " +
                                "a.client_unit_id = '" + unitid_mst + "' order by a.master_name ";
                            break;
                    }
                    break;
                #endregion
                #region Salary Grade
                case "salary_grade":

                    switch (btnval)
                    {
                        case "VIEW":
                        case "EDIT":

                            cmd = "SELECT (m.client_id||m.client_unit_id||m.vch_num|| to_char(m.vch_date, 'yyyymmdd')||m.type) as fstr,GROUP_CONCAT(DISTINCT gr.master_name) as Grade," +
                                "GROUP_CONCAT(DISTINCT mdp.master_name) as Department,GROUP_CONCAT(DISTINCT mdg.master_name) as Designation from enx_tab m " +
                                "INNER JOIN master_setting mdp on m.col1 = mdp.master_id and mdp.type = 'MDP' " +
                                "INNER JOIN master_setting mdg on m.col3 = mdg.master_id and mdg.type = 'MDG' " +
                                "INNER JOIN master_setting gr on m.col2=gr.master_id and gr.type='KGM' and gr.client_unit_id='" + unitid_mst + "' " +
                                "where m.type = 'KSG' and m.client_unit_id = '" + unitid_mst + "' " +
                                "GROUP BY (m.client_id||m.client_unit_id||m.vch_num|| to_char(m.vch_date, 'yyyymmdd')||m.type)";
                            break;
                    }

                    break;
                    #endregion
            }

            sgen.open_grid(title, cmd, sgen.Make_int(seektype));
            ViewBag.pageurl = sgen.GetSession(MyGuid, "pageurl").ToString();

        }

        #endregion

        //====================================== Action Method ===========================================================

        #region fam_master LOCATION MASTER
        //public ActionResult fam_master()
        //{
        //    FillMst();

        //    List<Tmodelmain> model = new List<Tmodelmain>();
        //    List<SelectListItem> mod1 = new List<SelectListItem>();
        //    List<SelectListItem> mod2 = new List<SelectListItem>();
        //    model = getload(model);
        //    model[0].Col9 = "LOCATION MASTER";
        //    model[0].Col10 = "master_setting";
        //    model[0].Col11 = "and client_id=" + clientid_mst + " and client_unit_id=" + unitid_mst + "";
        //    model[0].Col12 = "FAL";
        //    model[0].TList1 = mod1;
        //    model[0].SelectedItems1 = new string[] { "" };
        //    model[0].TList2 = mod2;
        //    model[0].SelectedItems2 = new string[] { "" };

        //    return View(model);
        //}

        //public List<Tmodelmain> newfam_master(List<Tmodelmain> model)
        //{

        //    model = getnew(model);

        //    //mod1 = new List<SelectListItem>();
        //    //mod2 = new List<SelectListItem>();

        //    #region Building dropdown
        //    //DataTable dt = new DataTable();
        //    //mq1 = "SELECT master_id,master_name FROM master_setting where type='HBM' " + model[0].Col11 + "";

        //    //dt = sgen.getdata(userCode, mq1);
        //    //if (dt.Rows.Count > 0)
        //    //{
        //    //    foreach (DataRow dr in dt.Rows)
        //    //    {
        //    //        mod1.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
        //    //    }
        //    //    TempData[MyGuid + "_Tlist1"] = mod1;
        //    //}
        //    #endregion

        //    #region Block
        //    //mq1 = "SELECT DISTINCT vch_num,col4 FROM enx_tab where type='HBM'" + model[0].Col11 + "";

        //    //dt = sgen.getdata(userCode, mq1);
        //    //if (dt.Rows.Count > 0)
        //    //{
        //    //    foreach (DataRow dr in dt.Rows)
        //    //    {
        //    //        mod2.Add(new SelectListItem { Text = dr["col4"].ToString(), Value = dr["vch_num"].ToString() });
        //    //    }

        //    //    TempData[MyGuid + "_Tlist2"] = mod2;
        //    //}
        //    #endregion
        //    //model[0].TList1 = mod1;
        //    //model[0].TList2 = mod2;
        //    model[0].SelectedItems1 = new string[] { "" };
        //    model[0].SelectedItems2 = new string[] { "" };
        //    model[0].Col19 = "Y";
        //    model[0].Col16 = vch_num;
        //    model[0].Col13 = "Save";


        //    return model;
        //}

        //[HttpPost]
        //public ActionResult fam_master(List<Tmodelmain> model, string command)
        //{

        //    FillMst();
        //    var tm = model.FirstOrDefault();
        //    List<SelectListItem> mod1 = new List<SelectListItem>();
        //    List<SelectListItem> mod2 = new List<SelectListItem>();
        //    model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
        //    model[0].TList2 = (List<SelectListItem>)TempData[MyGuid + "_TList2"];
        //    TempData[MyGuid + "_Tlist1"] = model[0].TList1;
        //    TempData[MyGuid + "_Tlist2"] = model[0].TList2;
        //    if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
        //    if (tm.SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
        //    if (command == "New")
        //    {

        //        model = newfam_master(model);
        //        //model = getnew(model);

        //        //    mod1 = new List<SelectListItem>();
        //        //    mod2 = new List<SelectListItem>();

        //        //    #region Building dropdown
        //        //    DataTable dt = new DataTable();
        //        //    mq1 = "SELECT master_id,master_name FROM master_setting where type='HBM' " + model[0].Col11 + "";

        //        //    dt = sgen.getdata(userCode, mq1);
        //        //    if (dt.Rows.Count > 0)
        //        //    {
        //        //        foreach (DataRow dr in dt.Rows)
        //        //        {
        //        //            mod1.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
        //        //        }
        //        //        TempData[MyGuid + "_Tlist1"] = mod1;
        //        //    }
        //        //    #endregion

        //        //    #region Block
        //        //    mq1 = "SELECT DISTINCT vch_num,col4 FROM enx_tab where type='HBM'" + model[0].Col11 + "";

        //        //    dt = sgen.getdata(userCode, mq1);
        //        //    if (dt.Rows.Count > 0)
        //        //    {
        //        //        foreach (DataRow dr in dt.Rows)
        //        //        {
        //        //            mod2.Add(new SelectListItem { Text = dr["col4"].ToString(), Value = dr["vch_num"].ToString() });
        //        //        }

        //        //        TempData[MyGuid + "_Tlist2"] = mod2;
        //        //    }
        //        //    #endregion
        //        //    model[0].TList1 = mod1;
        //        //    model[0].TList2 = mod2;
        //        //    model[0].SelectedItems1 = new string[] { "" };
        //        //    model[0].SelectedItems2 = new string[] { "" };
        //        //    model[0].Col19 = "Y";
        //        //    model[0].Col16 = vch_num;
        //        //    model[0].Col13 = "Save";

        //    }
        //    else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
        //    {
        //        string building = "", block = "", mastername = "", cond = "", type_desc = "location", area = "";
        //        try
        //        {
        //            var tmodel = model.FirstOrDefault();
        //            string currdate = sgen.server_datetime(userCode);
        //            ent_date = currdate;
        //            //DataTable dtstr = new DataTable();
        //            //dtstr = sgen.getdata(userCode, "select * from " + model[0].Col10 + " WHERE 1=2");

        //            type = model[0].Col12;
        //            if (model[0].Col19.Trim() == "N")
        //            {
        //                type = "DD" + type;

        //            }
        //            if (sgen.GetSession(MyGuid,"EDMODE").ToString().Equals("Y"))
        //            {
        //                mq1 = " and vch_num<>'" + tmodel.Col16.Trim() + "'";
        //                isedit = true;
        //                vch_num = tmodel.Col16.Trim();
        //                id = tmodel.Col18.Trim();
        //            }
        //            else
        //            {
        //                mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where type='" + model[0].Col12 + "' " + model[0].Col11 + "  ";
        //                vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");

        //                string mid = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10 + " where type='" + model[0].Col12 + "' " + model[0].Col11 + "";
        //                id = sgen.genNo(userCode, mid, 3, "auto_genid");
        //                isedit = false;
        //            }

        //            #region dtstr
        //            //DataTable dtstr = sgen.getdata(userCode, "select  * from " + model[0].Col10 + " where 1=2");
        //            DataTable dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());

        //            building = model[0].SelectedItems1.FirstOrDefault();
        //            block = model[0].SelectedItems2.FirstOrDefault();
        //            area = model[0].Col17;
        //            DataRow dr = dtstr.NewRow();
        //            dr["vch_num"] = vch_num;
        //            dr["vch_date"] = currdate;
        //            dr["type"] = model[0].Col12.Trim();
        //            dr["master_id"] = id;
        //            dr["master_type"] = type_desc;
        //            dr["classid"] = building;
        //            dr["sectionid"] = block;
        //            dr["subjectid"] = area;
        //            dr["status"] = model[0].Col19.Trim();

        //            dr = getsave(currdate, dr, model);
        //            #endregion

        //            //if (isedit)
        //            //{

        //            //    dr["rec_id"] = model[0].Col7.Trim();
        //            //    dr["client_id"] = model[0].Col1.Trim();
        //            //    dr["client_unit_id"] = model[0].Col2.Trim();
        //            //    dr["vch_num"] = model[0].Col16;
        //            //    dr["master_entby"] = model[0].Col3;
        //            //    dr["master_entdate"] = model[0].Col4;
        //            //    dr["master_editby"] = userid_mst;
        //            //    dr["master_editdate"] = currdate;
        //            //}
        //            //else
        //            //{

        //            //    dr["client_id"] = clientid_mst;
        //            //    dr["client_unit_id"] = unitid_mst;
        //            //    dr["master_entby"] = userid_mst;
        //            //    dr["master_entdate"] = currdate;
        //            //    dr["master_editby"] = "-";
        //            //    dr["master_editdate"] = currdate;
        //            //}
        //            dtstr.Rows.Add(dr);
        //            bool Result = sgen.Update_data(userCode, dtstr, model[0].Col10, tmodel.Col8, isedit);
        //            if (Result == true)
        //            {

        //                ViewBag.vnew = "";
        //                ViewBag.vedit = "";
        //                ViewBag.vsave = "disabled='disabled'";
        //                ViewBag.vsavenew = "disabled='disabled'";

        //                ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
        //                model = new List<Tmodelmain>();
        //                tmodel.Col17 = "";
        //                tmodel.Col16 = "";
        //                tmodel.Col18 = "";
        //                tmodel.Col20 = "";
        //                tmodel.Col21 = "";
        //                tmodel.Col13 = "Save";
        //                tmodel.Col100 = "Save & New";
        //                //tmodel.Col9 = "LOCATION MASTER";

        //                tmodel.TList1 = mod1;
        //                tmodel.SelectedItems1 = new string[] { "" };
        //                tmodel.TList2 = mod2;
        //                tmodel.SelectedItems2 = new string[] { "" };
        //                model.Add(tmodel);

        //            }
        //            else { ViewBag.scripCall = "showmsgJS(3, 'Record Not Saved', 1);"; }
        //        }
        //        catch (Exception ex) { sgen.showmsg(1, ex.Message.ToString(), 0); }
        //    }

        //    else if (command == "Cancel")
        //    {
        //        return CancelFun(model);
        //    }
        //    else if (command == "Callback")
        //    {
        //        model = StartCallback(model);
        //    }

        //    ModelState.Clear();
        //    return View(model);
        //}
        #endregion

        #region Group Master

        public ActionResult group_mst()
        {
            FillMst();

            List<Tmodelmain> model = new List<Tmodelmain>();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            model = getload(model);

            model[0].Col9 = "GROUP MASTER";
            model[0].Col10 = "master_setting";
            model[0].Col12 = "AGM";//Acount Group Master
            return View(model);
        }

        public List<Tmodelmain> newgroup_mst(List<Tmodelmain> model)
        {
            model = getnew(model);
            model[0].Col13 = "Save";

            model[0].Col16 = vch_num;
            return model;
        }

        [HttpPost]
        public ActionResult group_mst(List<Tmodelmain> model, string command)
        {
            FillMst(model[0].Col15);
            List<SelectListItem> mod1 = new List<SelectListItem>();
            var tm = model.FirstOrDefault();
            if (command == "New")
            {
                try
                {
                    model = newgroup_mst(model);

                    //model[0].Col13 = "Save";

                    //model[0].Col16 = vch_num;
                }
                catch (Exception ex) { }
            }

            else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
            {
                try
                {
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime_local(userCode);
                    currdate = sgen.Savedate(currdate, true);
                    string vch_num = "", found = "";


                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        isedit = true;
                        vch_num = model[0].Col16;
                    }
                    else
                    {

                        isedit = false;
                        mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                        master_id = sgen.genNo(userCode, mq, 3, "auto_genid");
                        //     found = sgen.getstring(userCode, "select a.master_name from " + tab_name + " a where a.master_name='" + txt_master.Text + "' and a.type='" + type + "'" +
                        //" and a.master_type='" + type_desc + "' " + Condition + "");

                        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");




                    }
                    //DataTable dataTable = sgen.getdata(userCode, "select * from " + model[0].Col10.Trim() + "  where  1=2");
                    DataTable dataTable = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    DataRow dt = dataTable.NewRow();

                    dt["rec_id"] = "0";
                    dt["vch_num"] = vch_num;
                    dt["vch_date"] = currdate;
                    dt["type"] = model[0].Col12;
                    //dr["master_id"] = "";
                    dt["master_name"] = model[0].Col16;
                    dt["master_type"] = model[0].Col22;
                    dt["sectionid"] = model[0].Col17;
                    dt["client_id"] = clientid_mst;
                    dt["client_unit_id"] = unitid_mst;

                    if (isedit)
                    {
                        dt["master_entby"] = model[0].Col3;
                        dt["master_entdate"] = model[0].Col4;
                        dt["master_editby"] = userid_mst;
                        dt["master_editdate"] = currdate;
                    }
                    else
                    {
                        dt["master_editby"] = "-";
                        dt["master_editdate"] = currdate;
                        dt["master_editby"] = userid_mst;
                        dt["master_entdate"] = currdate;
                    }
                    dataTable.Rows.Add(dt);
                    bool Result = sgen.Update_data(userCode, dataTable, model[0].Col10, tmodel.Col8, isedit);
                    ViewBag.vnew = "";
                    ViewBag.vedit = "";
                    ViewBag.vsave = "disabled='disabled'";
                    ViewBag.vsavenew = "disabled='disabled'";
                    if (Result == true)
                    {
                        //ViewBag.scripCall = "showmsgJS(1, 'Record Saved Successfully', 1);disableForm();";
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col17 = "",
                            Col1 = "",
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

        #region asset_ent
        public ActionResult asset_ent()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<SelectListItem> mod2 = new List<SelectListItem>();
            List<SelectListItem> mod3 = new List<SelectListItem>();
            List<SelectListItem> mod4 = new List<SelectListItem>();
            List<SelectListItem> mod5 = new List<SelectListItem>();
            List<SelectListItem> mod6 = new List<SelectListItem>();
            model = getload(model);
            model[0].Col9 = "ASSET ENTRY";
            model[0].Col10 = "vehicle_master";
            model[0].Col12 = "FAE";                              // fixed asset entry
            model[0].Col11 = "and client_id=" + clientid_mst + " and client_unit_id=" + unitid_mst + "";
            model[0].TList1 = mod1;
            model[0].SelectedItems1 = new string[] { "" };                              //Asset Head Minor
            model[0].TList2 = mod2;
            model[0].SelectedItems2 = new string[] { "" };                               //Currency name
            model[0].TList3 = mod3;
            model[0].SelectedItems3 = new string[] { "" };                                 //Govt Grant Scheme Name
            model[0].TList4 = mod4;
            model[0].SelectedItems4 = new string[] { "" };                                  //Warranty If Any 
            model[0].TList5 = mod5;
            model[0].SelectedItems5 = new string[] { "" };                                  //Cost Centre
            model[0].TList6 = mod6;
            model[0].SelectedItems6 = new string[] { "" };                                   //Item Location
            model[0].LTM1 = new List<Tmodelmain>();
            return View(model);
        }
        public List<Tmodelmain> newasset_ent(List<Tmodelmain> model)
        {
            model = getnew(model);
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<SelectListItem> mod2 = new List<SelectListItem>();
            List<SelectListItem> mod3 = new List<SelectListItem>();
            List<SelectListItem> mod4 = new List<SelectListItem>();
            List<SelectListItem> mod5 = new List<SelectListItem>();
            List<SelectListItem> mod6 = new List<SelectListItem>();
            #region Asset minor dropdown 1
            TempData[MyGuid + "_Tlist1"] = model[0].TList1 = mod1 = cmd_Fun.assminorhead(userCode, unitid_mst);
            #endregion
            #region  ASSET Cost Center  4
            TempData[MyGuid + "_Tlist4"] = model[0].TList4 = mod4 = cmd_Fun.asscostcenter(userCode, unitid_mst);
            #endregion
            #region     Govt Grant Scheme 3
            TempData[MyGuid + "_Tlist3"] = model[0].TList3 = mod3 = cmd_Fun.govtgrantsch(userCode, unitid_mst);
            #endregion
            #region Select Currency for 2
            TempData[MyGuid + "_Tlist2"] = mod2 = model[0].TList2 = cmd_Fun.curname(userCode, unitid_mst);
            #endregion
            #region location
         
            TempData[MyGuid + "_Tlist6"] = model[0].TList6 = mod6 = cmd_Fun.iloc(userCode,unitid_mst);
            #endregion
            mod5.Add(new SelectListItem { Text = "Yes", Value = "Y" });
            mod5.Add(new SelectListItem { Text = "No", Value = "N" });
            TempData[MyGuid + "_Tlist5"] = model[0].TList5 = mod5;
            model[0].SelectedItems1 = new string[] { "" };
            model[0].SelectedItems2 = new string[] { "" };
            model[0].SelectedItems3 = new string[] { "" };
            model[0].SelectedItems4 = new string[] { "" };
            model[0].SelectedItems5 = new string[] { "" };
            model[0].SelectedItems6 = new string[] { "" };
            return model;
        }
        [HttpPost]
        public ActionResult asset_ent(List<Tmodelmain> model, string command, HttpPostedFileBase upd1)
        {
            FillMst(model[0].Col15);
            var tm = model.FirstOrDefault();
            #region ddls
            List<SelectListItem> mod1 = (List<SelectListItem>)TempData[MyGuid + "_Tlist1"];
            List<SelectListItem> mod2 = (List<SelectListItem>)TempData[MyGuid + "_Tlist2"];
            List<SelectListItem> mod3 = (List<SelectListItem>)TempData[MyGuid + "_Tlist3"];
            List<SelectListItem> mod4 = (List<SelectListItem>)TempData[MyGuid + "_Tlist4"];
            List<SelectListItem> mod5 = (List<SelectListItem>)TempData[MyGuid + "_Tlist5"];
            List<SelectListItem> mod6 = (List<SelectListItem>)TempData[MyGuid + "_Tlist6"];
            TempData[MyGuid + "_Tlist1"] = model[0].TList1 = mod1;                 
            TempData[MyGuid + "_Tlist2"] = model[0].TList2 = mod2;
            TempData[MyGuid + "_Tlist3"] = model[0].TList3 = mod3;
            TempData[MyGuid + "_Tlist4"] = model[0].TList4 = mod4;
            TempData[MyGuid + "_Tlist5"] = model[0].TList5 = mod5;
            TempData[MyGuid + "_Tlist6"] = model[0].TList6 = mod6;
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
                    model = newasset_ent(model);
                }
                catch (Exception ex) { }
            }
            else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
            {
                try
                {
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime_local(userCode);
                    currdate = sgen.Savedate(currdate, true);
                    string vch_num = "", calb = "", dep = "", cal = "";
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

                    DataTable dataTable = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    type_desc = "fixed asset entry";
                    //for (int i = 0; i < Convert.ToInt32(model[0].Col22); i++)
                    for (int i = 0; i < model.Count; i++)
                    {
                        DataRow dr = dataTable.NewRow();
                        dr["type"] = model[0].Col12;
                        dr["vch_num"] = vch_num;   //Dock no
                        dr["date1"] = sgen.Make_date_S(model[0].Col18);   //bill date
                        dr["col1"] = model[0].Col34;      //asset name
                        dr["col2"] = model[0].Col17;      //bill no
                        dr["date2"] = sgen.Make_date_S(model[0].Col19);      //put to use date
                        dr["col4"] = model[0].SelectedItems1.FirstOrDefault();   //Asset Head Minor
                        dr["col19"] = model[0].Col20;   //Asset Head Major
                        //dr["col39"] = model[0].Col21;      //Asset Code 
                        dr["date3"] = sgen.Make_date_S(model[0].Col52);      //Expiry Date
                        dr["col8"] = model[0].Col23;      //Bill Amount In Company Currency
                        dr["col9"] = model[0].Col24;      //Currency Name 
                        dr["col28"] = model[0].Col25;      //Capex Amount 
                        dr["col36"] = model[0].Col26;      //grant amount
                        dr["col33"] = model[0].Col43;       //capax it
                        dr["col10"] = model[0].SelectedItems2.FirstOrDefault();      //Foreign Currency 
                        dr["col11"] = model[0].SelectedItems3.FirstOrDefault();     //Govt Grant Scheme Name 
                        dr["col12"] = model[0].SelectedItems6.FirstOrDefault();  //location 
                        dr["col13"] = model[0].Col29;                               //Sub Area 
                        dr["col14"] = model[0].SelectedItems4.FirstOrDefault();      //Cost Centre 
                        dr["col25"] = model[0].Col30;                               //Asset Description 
                        dr["col30"] = model[0].Col40;                          //p_recid
                        dr["col29"] = model[0].Col22;                   //Quantity
                        dr["col17"] = model[0].SelectedItems5.FirstOrDefault();     //Warranty If Any 
                        dr["col3"] = model[0].Col44;       //cmpy act  
                        dr["col5"] = model[0].Col45;     //income tax
                        dr["col6"] = model[0].Col46;     //salvage in %
                        dr["col15"] = model[0].Col47;    //salvage in absolute
                        dr["col18"] = model[0].Col49;     //life of asset
                        calb = model[0].Col48;            //Calibration Required Or Not
                        dep = model[0].Col51;            //Preferable Depreciation Method  rbt
                        cal = model[0].Col50;           //Calculation Method        rbt
                        dr["col37"] = calb;    //calibration required or not
                        dr["col40"] = cal;     //calculation method
                        dr["col42"] = dep;      //preferrable
                        dr["col34"] = type_desc;
                        dr = getsave(currdate, dr, model);
                        dataTable.Rows.Add(dr);
                    }
                    bool Result = sgen.Update_data_fast1(userCode, dataTable, model[0].Col10, tmodel.Col8, isedit);
                    if (Result == true)
                    {
                        #region attachment
                        DataTable dtfile = cmd_Fun.GetStructure(userCode, "file_tab");
                        string serverpath = Server.MapPath("~/Uploads/" + cg_com_name.Replace(" ", "") + "/");
                        //purchase Upload
                        try
                        {
                            HttpPostedFileBase file1 = upd1;
                            ctype1 = file1.ContentType;
                            fileName1 = file1.FileName;
                            path1 = clientid_mst + unitid_mst + userid_mst + currdate + "_" + fileName1;
                            encpath1 = sgen.Convert_Stringto64(path1).ToString();
                            finalpath1 = serverpath + encpath1;
                            file1.SaveAs(finalpath1);
                            filesave(model, currdate, dtfile, fileName1, encpath1, "FAE", ctype1, model[0].Col32, model[0].Col31, "", "");
                        }
                        catch (Exception ex) { }
                        bool res = false;
                        if (dtfile.Rows.Count > 0)
                        {
                            res = sgen.Update_data_fast1(userCode, dtfile, "file_tab", "", false);
                        }
                        #endregion
                        if (Result)
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
                                TList2 = mod2,
                                SelectedItems2 = new string[] { "" },
                                TList3 = mod3,
                                SelectedItems3 = new string[] { "" },
                                TList4 = mod4,
                                SelectedItems4 = new string[] { "" },
                                TList5 = mod5,
                                SelectedItems5 = new string[] { "" },
                                TList6 = mod6,
                                SelectedItems6 = new string[] { "" },
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
                                    model = newasset_ent(model);
                                    ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                                }
                                catch (Exception ex) { }
                            }

                        }
                        else { ViewBag.scripCall = "showmsgJS(1, 'Something Went Wrong, Data Not Saved', 0);"; }
                    }
                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
            }

            else if (command == "Class")
            {

                sgen.SetSession(MyGuid, "EDMODE", "N");
                ViewBag.vnew = "disabled='disabled'";
                ViewBag.vedit = "disabled='disabled'";
                ViewBag.vsave = "";
                ViewBag.vsavenew = "";
                DataTable dt1 = new DataTable();

                string dllvalue = model[0].SelectedItems1.FirstOrDefault();

                //mq1 = @"select ex. rec_id,ms.master_name as major_head,ex.col1 as minor_head,ex.vch_num from enx_tab ex
                // inner join master_setting ms on ms.master_id = ex.col2 and ms.type = 'MAH' and ms. client_id = ex.client_id and ms.client_unit_id = ex.client_unit_id 
                //    where ex. type = 'AMH'  and ms.client_id = '" + clientid_mst + "' and ex.client_unit_id = '" + unitid_mst + "'and ex.vch_num = '" + dllvalue + "'";


                mq1 = @"select ex. rec_id,ms.master_name as major_head,ex.col1 as minor_head,ex.vch_num,ex.rec_id,ex.col2,ex.col3,ex.col4,ex.col5,ex.col6," +
                    "ex.col7,ex.col8,ex.r_no,ex.ref_code,ex.type,ex.ent_by,ex.ent_date,ex.client_id,ex.client_unit_id from enx_tab ex " +
                    "inner join master_setting ms on ms.master_id = ex.col2 and ms.type = 'MAH' and " +
                    "ms.client_unit_id = ex.client_unit_id " +
                    "where ex. type = 'AMH' and ex.client_unit_id = '" + unitid_mst + "'and ex.vch_num = '" + dllvalue + "'";

                dt1 = sgen.getdata(userCode, mq1);
                if (dt1.Rows.Count > 0)
                {
                    model[0].Col20 = dt1.Rows[0]["major_head"].ToString();
                    model[0].Col44 = dt1.Rows[0]["col4"].ToString();
                    model[0].Col45 = dt1.Rows[0]["col7"].ToString();
                    model[0].Col46 = dt1.Rows[0]["col8"].ToString();
                    model[0].Col47 = dt1.Rows[0]["col3"].ToString();
                    model[0].Col49 = dt1.Rows[0]["col5"].ToString();
                    model[0].Col48 = dt1.Rows[0]["col6"].ToString();
                    model[0].Col50 = dt1.Rows[0]["r_no"].ToString();
                    model[0].Col51 = dt1.Rows[0]["ref_code"].ToString();
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

        private void filesave(List<Tmodelmain> model, string currdate, DataTable dtfile, string filename, string filepath, string filetitle, string content_type, string description)
        {

            DataRow drf = dtfile.NewRow();
            //type = "UQF";
            type = model[0].Col12;

            drf["vch_num"] = model[0].Col16;
            drf["rec_id"] = 0;
            drf["vch_date"] = currdate;
            drf["type"] = type;

            drf["ref_code"] = model[0].Col16;
            drf["ref_code1"] = model[0].Col16;
            drf["file_name"] = filename;
            drf["file_path"] = filepath;
            drf["col1"] = filetitle;
            drf["col2"] = content_type;

            drf["ent_by"] = userid_mst;
            drf["ent_date"] = currdate;
            drf["client_id"] = clientid_mst;
            drf["client_unit_id"] = unitid_mst;
            drf["edit_by"] = "-";
            drf["edit_date"] = currdate;

            dtfile.Rows.Add(drf);
        }
        //[HttpGet]
        //public FileResult fdown(string value)
        //{
        //    FillMst(MyGuid);
        //    List<Tmodelmain> model = new List<Tmodelmain>();
        //    if (!value.Trim().Equals(""))
        //    {
        //        DataTable dt2 = new DataTable();
        //        mq = "select file_name,file_path from file_tab where rec_id='" + value.Trim() + "' and type='" + model[0].Col12 + "' and client_id='"
        //            + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
        //        dt2 = sgen.getdata(userCode, mq);
        //        if (dt2.Rows.Count > 0)
        //        {
        //            path1 = Convert.ToString(dt2.Rows[0]["file_path"]);
        //            fileName1 = Convert.ToString(dt2.Rows[0]["file_name"]);
        //        }
        //    }
        //    byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Uploads/" + cg_com_name.Replace(" ", "") + "/" + path1));
        //    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName1);
        //}
        //[HttpPost]
        //public void fdelete(string value, string Myguid)
        //{
        //    if (!value.Trim().Equals(""))
        //    {
        //        sgen.SetSession(Myguid, "delid", value);
        //    }
        //}

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
        [HttpPost]
        public ContentResult filedata()
        {
            if (sgen.GetSession(MyGuid, "Avch_num_std") != null)
            {
                sgen = new sgenFun(MyGuid); string vch = sgen.GetSession(MyGuid, "Avch_num_std").ToString();
                DataTable dt = new DataTable();
                dt = sgen.getdata(userCode, "select rec_id,file_name,file_path,col1 from file_tab where ref_code='" + vch + "' and type='" + type + "' and " +
                    "client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
                dt.TableName = "MainData";

                DataSet ds3 = new DataSet();
                ds3.Tables.Add(dt);
                return Content(ds3.GetXml());
            }
            else { return Content(""); }
        }


        #endregion

        #region cost center
        public ActionResult cost_center()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            model = getload(model);
            model[0].Col9 = "COST CENTER";
            model[0].Col10 = "master_setting";
            model[0].Col12 = "FAC"; // Cost center 
            model[0].Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            model[0].TList1 = mod1;
            model[0].SelectedItems1 = new string[] { "" };
            return View(model);
        }
        public List<Tmodelmain> newcost_center(List<Tmodelmain> model)
        {
            model = getnew(model);
            List<SelectListItem> mod1 = new List<SelectListItem>();
            model[0].Col19 = "Y";
            mod1 = new List<SelectListItem>();
            #region department
            TempData[MyGuid + "_Tlist1"] = model[0].TList1 = mod1 = cmd_Fun.dept(userCode, clientid_mst, unitid_mst);
            model[0].SelectedItems1 = new string[] { "" };
            #endregion
            return model;
        }
        [HttpPost]
        public ActionResult cost_center(List<Tmodelmain> model, string command)
        {
            FillMst(model[0].Col15);
            var tm = model.FirstOrDefault();
            Master_Type = "Select department";
            string inactive_date = "";
            List<SelectListItem> mod1 = new List<SelectListItem>();
            TempData[MyGuid + "_Tlist1"] = model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_Tlist1"];
            if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };

            if (command == "New")
            {
                try
                {
                    model = newcost_center(model);
                }
                catch (Exception ex)
                {
                    sgen.showmsg(1, ex.Message.ToString(), 2);
                }
            }

            else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
            {
                try
                {
                    type_desc = "cost centre";
                    string currdate = sgen.server_datetime_local(userCode);
                    ent_date = sgen.Savedate(currdate, true);
                    status = tm.Col19.Trim();
                    if (status == "N") { model[0].Col19 = "DDFAC"; }
                    DataTable dtstr = new DataTable();
                    dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        mq1 = " and vch_num<>'" + model[0].Col16.Trim() + "'";
                        isedit = true;
                        vch_num = model[0].Col16.Trim();
                        master_id = vch_num.Substring(3, 3);
                    }
                    else
                    {
                        mq = sgen.seekval(userCode, "select classid from " + model[0].Col10 + " WHERE TYPE  ='" + model[0].Col12 + "' " + model[0].Col11 + " and classid='Y' ", "classid");

                        if (!mq.Trim().Equals("0"))
                        {
                            ViewBag.scripCall = "showmsgJS(3, 'You already checked for COST_CENTER', 1);";
                            ViewBag.vnew = "disabled='disabled'";
                            ViewBag.vedit = "disabled='disabled'";
                            ViewBag.vsave = "";
                            ViewBag.vsavenew = "";
                            return View(model);
                        }
                        mq = sgen.seekval(userCode, "Select master_setting as master_setting from " + model[0].Col10 + " WHERE TYPE ='" + model[0].Col12 + "' and master_setting ='" + model[0].Col18 + "' " + model[0].Col11 + ""
                          , "master_setting");
                        if (!mq.Trim().Equals("0"))
                        {
                            ViewBag.scripCall = "showmsgJS(3, 'You already saved cost center', 1);";
                            ViewBag.vnew = "disabled='disabled'";
                            ViewBag.vedit = "disabled='disabled'";
                            ViewBag.vsave = "";
                            ViewBag.vsavenew = "";
                            return View(model);
                        }
                        else
                        {
                            mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " WHERE TYPE ='" + model[0].Col12 + "' " + model[0].Col11 + "  ";
                            vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                            isedit = false;

                            mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10 + " WHERE TYPE ='" + model[0].Col12 + "' " + model[0].Col11 + "  ";
                            master_id = sgen.genNo(userCode, mq, 3, "auto_genid");
                            isedit = false;
                        }
                    }

                    #region dtstr
                    DataRow dr = dtstr.NewRow();
                    dr["rec_id"] = "0";
                    dr["vch_num"] = vch_num;
                    dr["vch_date"] = ent_date;
                    dr["type"] = model[0].Col12;
                    dr["Master_type"] = type_desc;
                    dr["Master_Name"] = model[0].Col18;   //txt_cost_centre
                    dr["master_id"] = master_id;
                    dr["classid"] = model[0].SelectedItems1.FirstOrDefault();   //Select Department
                    dr["sectionid"] = isprmoted;
                    dr["status"] = model[0].Col19;    //Status
                    dr["subjectid"] = Master_Type;
                    dr["client_id"] = clientid_mst;
                    dr["client_unit_id"] = unitid_mst;
                    dr = getsave(ent_date, dr, model);
                    #endregion
                    dtstr.Rows.Add(dr);
                    bool Result = sgen.Update_data_fast1(userCode, dtstr, model[0].Col10, tm.Col8, isedit);
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
                                model = newcost_center(model);
                                ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                            }
                            catch (Exception ex) { }
                        }

                    }

                    else { ViewBag.scripCall = "showmsgJS(3, 'Record Not Saved', 1);"; }

                }
                catch (Exception ex) { sgen.showmsg(1, ex.Message.ToString(), 0); }
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

        #region ASSET WRITE OFF
        public ActionResult asset_write()
        {
            FillMst("");
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "ASSET WRITE OFF";
            model[0].Col10 = "vehicle_master";
            model[0].Col12 = "AWO"; // ASSET WRITE OFF
            model[0].Col11 = "and client_id = '" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            return View(model);
        }
        public List<Tmodelmain> newasset_write(List<Tmodelmain> model)
        {
            model = getnew(model);
            return model;
        }
        [HttpPost]
        public ActionResult asset_write(List<Tmodelmain> model, string command)
        {
            FillMst(model[0].Col15);
            var tm = model.FirstOrDefault();
            if (command == "New")
            {
                try
                {
                    model = newasset_write(model);
                }
                catch (Exception ex) { }
            }
            else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
            {
                try
                {
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime_local(userCode);
                    currdate = sgen.Savedate(currdate, true);
                    string vch_num = "", calb = "";
                    string cond = "";
                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        isedit = true;
                        vch_num = model[0].Col16;
                    }
                    else
                    {
                        cond = "";
                        isedit = false;
                        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                    }
                    DataTable dataTable = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    DataRow dr = dataTable.NewRow();
                    dr["type"] = model[0].Col12;
                    dr["vch_num"] = vch_num;
                    dr["date1"] = sgen.Savedate(model[0].Col24, false);
                    dr["col1"] = model[0].Col25; //asset code
                    //dr["col2"] = model[0].Col17;
                    dr["col3"] = model[0].Col18;
                    dr["col4"] = model[0].Col19;
                    dr = getsave(currdate, dr, model);
                    dataTable.Rows.Add(dr);
                    bool Result = sgen.Update_data(userCode, dataTable, model[0].Col10, tmodel.Col8, isedit);
                    if (Result == true)
                    {
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col17 = "",
                            Col1 = "",
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
                                model = newasset_write(model);
                                ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                            }
                            catch (Exception ex) { }
                        }
                    }
                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
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

        #region asset_sale
        public ActionResult asset_sale()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "ASSET SALE";
            model[0].Col10 = "vehicle_master";
            model[0].Col12 = "ASM"; // ASSET SALE MASTER
            model[0].Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            return View(model);
        }
        public List<Tmodelmain> newasset_sale(List<Tmodelmain> model)
        {
            model = getnew(model);
            return model;
        }
        [HttpPost]
        public ActionResult asset_sale(List<Tmodelmain> model, string command)
        {
            FillMst(model[0].Col15);
            var tm = model.FirstOrDefault();
            if (command == "New")
            {
                try
                {
                    model = newasset_sale(model);
                }
                catch (Exception ex) { }
            }

            else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
            {
                try
                {
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime_local(userCode);
                    currdate = sgen.Savedate(currdate, true);
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
                    DataTable dataTable = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    DataRow dr = dataTable.NewRow();
                    dr["type"] = model[0].Col12;
                    dr["vch_num"] = vch_num;
                    dr["date1"] = sgen.Savedate(model[0].Col27, false);
                    //dr["col1"] = model[0].Col17;
                    dr["col2"] = model[0].Col31;
                    dr["col3"] = model[0].Col18;
                    dr["col4"] = model[0].Col19;
                    dr["col5"] = model[0].Col20;
                    dr["col6"] = model[0].Col21;
                    dr["col7"] = model[0].Col22;
                    dr = getsave(currdate, dr, model);
                    dataTable.Rows.Add(dr);
                    bool Result = sgen.Update_data(userCode, dataTable, model[0].Col10, tmodel.Col8, isedit);
                    if (Result == true)
                    {
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col17 = "",
                            Col1 = "",
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
                                model = newasset_sale(model);
                                ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                            }
                            catch (Exception ex) { }
                        }

                    }
                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
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

        #region fa_major_head
        public ActionResult fa_major_head()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "FA MAJOR HEAD MASTER";
            model[0].Col10 = "master_setting";
            model[0].Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            model[0].Col12 = "MAH"; // FA MAJOR HEAD MASTER
            return View(model);
        }
        public List<Tmodelmain> newfa_major_head(List<Tmodelmain> model)
        {
            model = getnew(model);
            model[0].Col19 = "Y";
            return model;
        }
        [HttpPost]
        public ActionResult fa_major_head(List<Tmodelmain> model, string command)
        {
            FillMst(model[0].Col15);
            var tm = model.FirstOrDefault();
            if (command == "New")
            {
                try
                {
                    model = newfa_major_head(model);
                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
            }
            else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
            {
                try
                {
                    string currdate = sgen.server_datetime(userCode);

                    if (model[0].Col19.Trim() == "N")
                    {
                        type = "DD" + type;
                    }
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
                    DataTable dataTable = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    DataRow dr = dataTable.NewRow();

                    dr["rec_id"] = "0";
                    dr["type"] = model[0].Col12;
                    dr["vch_num"] = vch_num;
                    dr["master_id"] = vch_num;
                    dr["vch_date"] = currdate;
                    dr["master_name"] = model[0].Col17;
                    dr["client_name"] = model[0].Col18;
                    dr["Status"] = model[0].Col19;
                    dr["Inactive_date"] = currdate;
                    if (isedit)
                    {
                        dr["rec_id"] = model[0].Col7;
                        dr["client_id"] = model[0].Col1;
                        dr["client_unit_id"] = model[0].Col2;
                        dr["master_entby"] = model[0].Col3;
                        dr["master_entdate"] = currdate;
                        dr["master_editby"] = userid_mst;
                        dr["master_editdate"] = currdate;
                    }
                    else
                    {
                        dr["master_entby"] = userid_mst;
                        dr["master_entdate"] = currdate;
                        dr["master_editby"] = "-";
                        dr["master_entdate"] = currdate;
                        dr["client_id"] = clientid_mst;
                        dr["client_unit_id"] = unitid_mst;
                    }
                    dataTable.Rows.Add(dr);
                    res = sgen.Update_data_fast1(userCode, dataTable, model[0].Col10, model[0].Col8, isedit);
                    if (res == true)
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
                                model = newfa_major_head(model);
                                ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                            }
                            catch (Exception ex) { }
                        }

                    }
                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
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

        #region Goverment Grant Master
        public ActionResult goverment_grant()
        {
            FillMst();

            List<Tmodelmain> model = new List<Tmodelmain>();

            model = getload(model);
            model[0].Col9 = "GOVERNMENT GRANT MASTER";
            model[0].Col10 = "enx_tab";
            model[0].Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            model[0].Col12 = "GGM"; // GOVERNMENT GRANT MASTER

            sgen.SetSession(MyGuid, "FA_TR_MID", mid_mst.Trim());
            sgen.SetSession(MyGuid, "FA_TYPE_GGM", model[0].Col12.Trim());
            sgen.SetSession(MyGuid, "FA_COND_GGM", model[0].Col11.Trim());
            sgen.SetSession(MyGuid, "FA_TBL_GGM", model[0].Col10.Trim());

            return View(model);
        }

        public List<Tmodelmain> newgoverment_grant(List<Tmodelmain> model)
        {
            model = getnew(model);
            model[0].Col13 = "Save";
            return model;
        }

        [HttpPost]
        public ActionResult goverment_grant(List<Tmodelmain> model, string command)
        {
            FillMst(model[0].Col15);

            var tm = model.FirstOrDefault();
            if (command == "New")
            {
                try
                {
                    model = newgoverment_grant(model);

                }
                catch (Exception ex) { }
            }

            else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
            {
                try
                {
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime_local(userCode);


                    type_desc = "govt grant master";
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
                    //DataTable dataTable = sgen.getdata(userCode, "select * from " + model[0].Col10.Trim() + "  where  1=2");
                    DataTable dataTable = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    DataRow dr = dataTable.NewRow();
                    dr["type"] = model[0].Col12;
                    dr["vch_num"] = vch_num.Trim();
                    dr["vch_date"] = currdate;
                    dr["col6"] = model[0].Col17;
                    dr["date1"] = sgen.Savedate(model[0].Col18.Trim(), false);
                    dr["col3"] = model[0].Col19;
                    dr["col1"] = model[0].Col21;
                    dr["date2"] = sgen.Savedate(model[0].Col20.Trim(), false);
                    dr["type_desc"] = type_desc;
                    if (isedit)
                    {

                        dr["rec_id"] = model[0].Col7;
                        dr["ent_by"] = model[0].Col3;
                        dr["ent_date"] = model[0].Col4;
                        dr["client_id"] = model[0].Col1;
                        dr["client_unit_id"] = model[0].Col2;
                        dr["edit_by"] = userid_mst;
                        dr["edit_date"] = currdate;
                    }
                    else
                    {
                        dr["rec_id"] = "0";
                        dr["ent_by"] = userid_mst;
                        dr["ent_date"] = currdate;
                        dr["client_id"] = clientid_mst;
                        dr["client_unit_id"] = unitid_mst;
                        dr["edit_by"] = "-";
                        dr["edit_date"] = currdate;
                    }
                    dataTable.Rows.Add(dr);
                    bool Result = sgen.Update_data(userCode, dataTable, model[0].Col10, tmodel.Col8, isedit);
                    ViewBag.vnew = "";
                    ViewBag.vedit = "";
                    ViewBag.vsave = "disabled='disabled'";
                    ViewBag.vsavenew = "disabled='disabled'";
                    if (Result == true)
                    {
                        //ViewBag.scripCall = "showmsgJS(1, 'Record Saved Successfully', 1);disableForm();";
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col17 = "",
                            Col1 = "",
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

        #region epcg_mst
        public ActionResult epcg_mst()
        {
            FillMst();

            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);

            model[0].Col9 = "EPCG";
            model[0].Col10 = "enx_tab";
            model[0].Col37 = "file_tab";
            model[0].Col32 = "Choose File";
            model[0].Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            model[0].Col12 = "EPC"; //EPCG
            sgen.SetSession(MyGuid, "FA_TR_MID", mid_mst.Trim());
            sgen.SetSession(MyGuid, "FA_TYPE_GGM", model[0].Col12.Trim());
            sgen.SetSession(MyGuid, "FA_COND_GGM", model[0].Col11.Trim());
            sgen.SetSession(MyGuid, "FA_TBL_GGM", model[0].Col10.Trim());
            return View(model);
        }

        public List<Tmodelmain> newepcg_mst(List<Tmodelmain> model)
        {
            model = getnew(model);
            model[0].Col13 = "Save";
            model[0].Col16 = vch_num;
            return model;
        }

        [HttpPost]
        public ActionResult epcg_mst(List<Tmodelmain> model, string command, HttpPostedFileBase[] upd1)
        {
            FillMst(model[0].Col15);
            var tm = model.FirstOrDefault();
            IList<HttpPostedFileBase> fileCollection1 = new List<HttpPostedFileBase>();
            if (command == "New")
            {
                try
                {
                    model = newepcg_mst(model);

                }
                catch (Exception ex) { }
            }

            else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
            {
                try
                {
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime_local(userCode);
                    currdate = sgen.Savedate(currdate, true);
                    type_desc = "EPCG";
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
                    //DataTable dataTable = sgen.getdata(userCode, "select * from " + model[0].Col10.Trim() + "  where  1=2");
                    DataTable dataTable = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    DataRow dr = dataTable.NewRow();
                    dr["type"] = model[0].Col12;
                    dr["vch_num"] = vch_num.Trim();
                    dr["vch_date"] = sgen.Savedate(currdate, true);
                    dr["col6"] = model[0].Col17;
                    dr["date1"] = sgen.Savedate(model[0].Col18.Trim(), false);
                    dr["col3"] = model[0].Col19;
                    dr["col1"] = model[0].Col21;
                    dr["date2"] = sgen.Savedate(model[0].Col22.Trim(), false);
                    dr["col7"] = model[0].Col20;
                    dr["col8"] = model[0].Col23;

                    dr["type_desc"] = type_desc;
                    if (isedit)
                    {
                        dr["rec_id"] = model[0].Col7;
                        dr["ent_by"] = model[0].Col3;
                        dr["ent_date"] = model[0].Col4;
                        dr["client_id"] = model[0].Col1;
                        dr["client_unit_id"] = model[0].Col2;
                        dr["edit_by"] = userid_mst;
                        dr["edit_date"] = sgen.Savedate(currdate, true);
                    }
                    else
                    {
                        dr["rec_id"] = "0";
                        dr["ent_by"] = userid_mst;
                        dr["ent_date"] = sgen.Savedate(currdate, true);
                        dr["client_id"] = clientid_mst;
                        dr["client_unit_id"] = unitid_mst;
                        dr["edit_by"] = "-";
                        dr["edit_date"] = sgen.Savedate(currdate, false);
                    }
                    dataTable.Rows.Add(dr);
                    bool Result = sgen.Update_data(userCode, dataTable, model[0].Col10, tmodel.Col8, isedit);
                    if (Result == true)
                    {

                        vch_num = model[0].Col16;
                        DataTable dtfile = new DataTable();
                        dtfile = sgen.getdata(userCode, "select * from file_tab WHERE 1=2");
                        type_desc = "EPCG";
                        string serverpath = Server.MapPath("~/Uploads/" + cg_com_name.Replace(" ", "") + "/");
                        try
                        {
                            foreach (HttpPostedFileBase file in upd1)
                            {
                                HttpPostedFileBase file1 = file;
                                ctype1 = file1.ContentType;
                                fileName1 = file1.FileName;
                                path1 = clientid_mst + unitid_mst + userid_mst + currdate + "_" + fileName1;
                                encpath1 = sgen.Convert_Stringto64(path1).ToString();
                                finalpath1 = serverpath + encpath1;
                                file1.SaveAs(finalpath1);
                                filesave(model, currdate, dtfile, fileName1, encpath1, "EPCG", ctype1, "");
                            }
                        }

                        catch (Exception ex) { }
                        bool res = sgen.Update_data(userCode, dtfile, model[0].Col37, model[0].Col38, false);
                        if (Result)
                        {
                            ViewBag.vnew = "";
                            ViewBag.vedit = "";
                            ViewBag.vsave = "disabled='disabled'";
                            ViewBag.vsavenew = "disabled='disabled'";
                            //ViewBag.scripCall = "showmsgJS(1, 'Record Saved Successfully', 1);disableForm();";
                            ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                            model = new List<Tmodelmain>();
                            model.Add(new Tmodelmain
                            {
                                Col17 = "",
                                Col1 = "",
                                Col9 = tm.Col9,
                                Col10 = tm.Col10,
                                Col11 = tm.Col11,
                                Col12 = tm.Col12,
                                Col13 = tm.Col13,
                                Col14 = tm.Col14,
                                Col15 = tm.Col15,

                            });
                        }
                    }
                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
            }
            else if (command == "Cancel")
            {
                return CancelFun(model);
            }
            else if (command == "Callback")
            {
                if (sgen.GetSession(MyGuid, "delid") != null) { btnval = "FDEL"; sgen.SetSession(MyGuid, "btnval", btnval); }
                model = StartCallback(model);
            }
            ModelState.Clear();
            return View(model);
        }

        #endregion

        #region fa_m FA MINOR HEAD 
        public ActionResult fa_m()
        {
            FillMst();

            List<Tmodelmain> model = new List<Tmodelmain>();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            model = getload(model);
            model[0].Col9 = "FA MINOR HEAD";
            model[0].Col10 = "enx_tab";
            model[0].Col12 = "AMH"; // Asset Minor Head
            model[0].Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            model[0].TList1 = mod1;
            model[0].SelectedItems1 = new string[] { "" };
            return View(model);
        }
        public List<Tmodelmain> newfa_m(List<Tmodelmain> model)
        {
            model = getnew(model);
            List<SelectListItem> mod1 = new List<SelectListItem>();
            mod1 = new List<SelectListItem>();
            #region Asset minor dropdown
            DataTable dt = new DataTable();
            mq1 = "Select master_id, master_name from master_setting where type = 'MAH'" + model[0].Col11.Trim() + "";
            dt = sgen.getdata(userCode, mq1);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    mod1.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                }
                TempData[MyGuid + "_Tlist1"] = model[0].TList1 = mod1;
            }
            #endregion
            
            model[0].SelectedItems1 = new string[] { "" };
            return model;
        }
        [HttpPost]
        public ActionResult fa_m(List<Tmodelmain> model, string command)
        {
            FillMst(model[0].Col15);
            var tm = model.FirstOrDefault();

            List<SelectListItem> mod1 = new List<SelectListItem>();
            TempData[MyGuid + "_Tlist1"] = model[0].TList1 = mod1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
         
            if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
            if (command == "New")
            {
                model = newfa_m(model);
            }
            else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
            {
                try
                {
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime(userCode);
                    string vch_num = "", calb = "", dep = "", cal = "";
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
                    DataTable dataTable = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    DataRow dr = dataTable.NewRow();
                    calb = model[0].Col22;            //Calibration Required Or Not
                    dep = model[0].Col25;            //Preferable Depreciation Method  rbt
                    cal = model[0].Col24;           //Calculation Method        rbt
                    dr["rec_id"] = "0";
                    dr["type"] = model[0].Col12;
                    dr["date1"] = currdate;
                    dr["client_id"] = clientid_mst;
                    dr["client_unit_id"] = unitid_mst;
                    dr["vch_num"] = vch_num;
                    dr["col1"] = model[0].Col26;       //Asset Minor Head
                    dr["col2"] = model[0].SelectedItems1.FirstOrDefault();    //Asset Major Head 
                    dr["col3"] = model[0].Col21;    //Salvage Value in Absolute Amount
                    dr["col4"] = model[0].Col18;    //Companies Act / Custom Rate
                    dr["col5"] = model[0].Col23;     //Life Of Asset In Years
                    dr["col6"] = calb;                  //
                    dr["col7"] = model[0].Col19;        //Income Tax Rate
                    dr["col8"] = model[0].Col20;       //Salvage Value in % 
                    dr["r_no"] = cal;
                    dr["ref_code"] = dep;
                    dr["type_desc"] = model[0].Col17;     //Asset Minor Head Abbreviation
                    dr = getsave(currdate, dr, model);
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
                            Col13 = "Save",
                            Col14 = tm.Col14,
                            Col15 = tm.Col15,
                            Col100 = "Save & New",
                            Col121 = "S",
                            Col122 = "<u>S</u>ave",
                            Col123 = "Save & Ne<u>w</u>",
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
                                model = newfa_m(model);
                                ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                            }
                            catch (Exception ex) { }
                        }

                    }
                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
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

        #region Transfer_of_asset

        public ActionResult transfer_asset()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);

            List<SelectListItem> mod2 = new List<SelectListItem>();
            model[0].TList2 = mod2;
            model[0].SelectedItems2 = new string[] { "" };
            TempData[MyGuid + "_TList2"] = model[0].TList2;

            model[0].Col9 = "TRANSFER OF ASSET";
            model[0].Col10 = "enx_tab";
            model[0].Col11 = "and client_id = '" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            model[0].Col12 = "TOA"; // TRANSFER OF ASSET
            return View(model);
        }

        public List<Tmodelmain> newtransfer_asset(List<Tmodelmain> model)
        {
            model = getnew(model);
            List<SelectListItem> mod2 = new List<SelectListItem>();
            mod2 = new List<SelectListItem>();

            #region location  2
            DataTable dt2 = new DataTable();

            mod2 = cmd_Fun.itemlocation(userCode, clientid_mst, unitid_mst);
            TempData[MyGuid + "_Tlist2"] = mod2;
            #endregion

            model[0].Col16 = vch_num;
            model[0].TList2 = mod2;
            model[0].SelectedItems2 = new string[] { "" };

            model[0].Col13 = "Save";
            return model;
        }


        [HttpPost]
        public ActionResult transfer_asset(List<Tmodelmain> model, string command)
        {
            FillMst(model[0].Col15);
            var tm = model.FirstOrDefault();

            List<SelectListItem> mod2 = new List<SelectListItem>();
            model[0].TList12 = (List<SelectListItem>)TempData[MyGuid + "_TList2"];
            TempData[MyGuid + "_Tlist2"] = model[0].TList2;
            model[0].TList2 = mod2;
            if (tm.SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };

            if (command == "New")
            {
                try
                {
                    model = newtransfer_asset(model);
                }
                catch (Exception ex) { }
            }

            else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
            {
                try
                {
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime(userCode);
                    type_desc = "";
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
                    //DataTable dataTable = sgen.getdata(userCode, "select * from " + model[0].Col10.Trim() + "  where  1=2");
                    DataTable dataTable = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    DataRow dr = dataTable.NewRow();

                    type_desc = "fixed asset entry";
                    dr["type"] = model[0].Col12;
                    dr["vch_num"] = vch_num.Trim();
                    dr["vch_date"] = currdate;
                    dr["date1"] = currdate;
                    dr["date2"] = sgen.Savedate(model[0].Col17, false);      //Date Of Transfer
                    dr["col2"] = model[0].Col18;   //gross value
                    dr["col3"] = model[0].Col19;      //WDV
                    dr["col8"] = model[0].Col32;       //rec_id
                    dr["col5"] = model[0].SelectedItems2.FirstOrDefault();    //LOC 2
                    dr["col12"] = model[0].Col23;      //         sub area 2
                    dr["col13"] = model[0].Col24;      //         Remark
                    dr["col4"] = type_desc;

                    dr = getsave(currdate, dr, model);
                    dataTable.Rows.Add(dr);
                    bool Result = sgen.Update_data(userCode, dataTable, model[0].Col10, tmodel.Col8, isedit);
                    if (Result == true)
                    {
                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        ViewBag.vsavenew = "disabled='disabled'";
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                        //ViewBag.scripCall = "showmsgJS(1, 'Record Saved Successfully', 1);disableForm();";
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
                            Col16 = "",
                            Col18 = "",
                            Col19 = "",
                            Col20 = "",
                            Col21 = "",
                            Col22 = "",
                            Col23 = "",
                            Col24 = "",
                            Col25 = "",
                            TList2 = mod2,
                            SelectedItems2 = new string[] { "" },
                        });

                    }
                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
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

        #region PV Statement
        public ActionResult pv_stmt()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<SelectListItem> mod2 = new List<SelectListItem>();
            model[0].Col9 = "PV Statement";
            model[0].Col10 = "vehicle_master";
            model[0].Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            model[0].Col12 = "PVS";
            model[0].TList1 = mod1;
            model[0].SelectedItems1 = new string[] { "" };
            model[0].TList2 = mod2;
            model[0].SelectedItems2 = new string[] { "" };

            TempData[MyGuid + "_TList1"] = model[0].TList1;
            TempData[MyGuid + "_TList2"] = model[0].TList2;

            DataTable dt = sgen.getdata(userCode, "select '' as Add_Items,'1' as SNo,'-' as Asset_Code,'-' as Asset_Name,'-' as Asset_no,'-' minor_head,'-'" +
                " as Asset_Head_Major,'0' as Qty_in_stock,'0' as Qty_Phy,'0' Variation,'-' as Put_To_Use_Date,'0' Remark,'-' Status from dual");
            model[0].dt1 = dt;
            sgen.SetSession(MyGuid, "BEX_DT", dt);

            return View(model);
        }
        [HttpPost]
        public ActionResult pv_stmt(List<Tmodelmain> model, string command, string hftable, string hfrow)
        {
            FillMst(model[0].Col15);
            var tm = model.FirstOrDefault();
            DataTable dtm = sgen.settable(hftable);
            model[0].dt1 = dtm;

            #region Dropdowns
            List<SelectListItem> mod1 = new List<SelectListItem>();
            TempData[MyGuid + "_Tlist1"] = model[0].TList1 = mod1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
          
            if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };

            List<SelectListItem> mod2 = new List<SelectListItem>();
            mod2 = (List<SelectListItem>)TempData[MyGuid + "_TList2"];
            TempData[MyGuid + "_Tlist2"] = mod2;
            model[0].TList2 = mod2;
            if (tm.SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
            #endregion

            if (command == "New")
            {
                try
                {
                    model = getnew(model);
                    model[0].Col16 = vch_num;
                    mod1 = new List<SelectListItem>();
                    mod2 = new List<SelectListItem>();

                    #region getdept
                    DataTable dt2 = new DataTable();
                    dt2 = sgen.getdata(userCode, "SELECT master_id,master_name FROM master_setting WHERE Type='MDP'");
                    if (dt2.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt2.Rows)
                        {
                            mod1.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                        }
                       
                    }
                    TempData[MyGuid + "_Tlist1"] = model[0].TList1 = mod1;
                    #endregion
                    #region getdesig
                    dt2 = sgen.getdata(userCode, "SELECT master_id,master_name FROM master_setting WHERE Type='MDG'");
                    if (dt2.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt2.Rows)
                        {
                            mod2.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                        }
                     
                    }
                    TempData[MyGuid + "_Tlist2"] = model[0].TList2 = mod2;
                    #endregion

                    model[0].Col13 = "Save";
                 
                    model[0].SelectedItems1 = new string[] { "" };
                  
                    model[0].SelectedItems2 = new string[] { "" };
                    model[0].Col13 = "Save";
                }
                catch (Exception ex) { }
            }
            else if (command.Trim() == "Save" || command.Trim() == "Update" || command == "Save & New" || command == "Update & New")
            {

                try
                {
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime(userCode);
                    type_desc = "";
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
                    //dtstr = sgen.getdata(userCode, "select * from " + model[0].Col10 + " WHERE 1=2");

                    DataTable dataTable = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    for (int i = 0; i < model[0].dt1.Rows.Count; i++)
                    {
                        DataRow dr = dataTable.NewRow();
                        dr["rec_id"] = "0";
                        dr["type"] = model[0].Col12;
                        dr["vch_date"] = sgen.Savedate(currdate, false);
                        dr["vch_num"] = vch_num.Trim();
                        //dr["col1"] = model[0].Col17;                       //Doc No
                        dr["date2"] = model[0].Col18;                       //Verification dt
                        dr["col2"] = model[0].Col19;                       //Remark 1
                        dr["col1"] = model[0].SelectedItems1.FirstOrDefault();                      // Dept
                        dr["col3"] = model[0].SelectedItems2.FirstOrDefault();                      // Designation

                        dr["col4"] = model[0].dt1.Rows[i]["ASSET_NO"].ToString(); //VM Doc
                        dr["col7"] = model[0].dt1.Rows[i]["QTY_IN_STOCK"].ToString();
                        dr["col8"] = model[0].dt1.Rows[i]["QTY_PHY"].ToString();           // 
                        dr["col9"] = model[0].dt1.Rows[i]["VARIATION"].ToString();
                        dr["col12"] = model[0].dt1.Rows[i]["REMARK"].ToString();     //Grid REMARK 
                                                                                     //dr["col5"] = model[0].dt1.Rows[i]["STATUS"].ToString();     //
                        dr = getsave(currdate, dr, model);

                        dataTable.Rows.Add(dr);
                    }

                    bool Result = sgen.Update_data(userCode, dataTable, model[0].Col10.Trim(), tmodel.Col8, isedit);
                    if (Result == true)
                    {
                        vch_num = model[0].Col16;

                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        ViewBag.vsavenew = "disabled='disabled'";
                        //ViewBag.scripCall = "showmsgJS(1, 'Data Saved Successfully', 1); disableForm();";
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col10 = tm.Col10,
                            Col11 = tm.Col11,
                            Col12 = tm.Col12,
                            Col13 = "Save",
                            Col100 = "Save & New",
                            Col14 = tm.Col14,
                            Col15 = tm.Col15,
                            Col9 = tm.Col9,
                            TList1 = mod1,
                            SelectedItems1 = new string[] { "" },
                            TList2 = mod2,
                            SelectedItems2 = new string[] { "" },
                            dt1 = (DataTable)sgen.GetSession(MyGuid, "BEX_DT")
                        });
                    }
                    else
                    {
                        ViewBag.scripCall = "showmsgJS(1, 'Record Not Saved', 2);";
                    }
                }
                catch (Exception ex) { sgen.showmsg(1, ex.Message.ToString(), 0); }
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

        #region ins_rev

        public ActionResult ins_rev()
        {
            FillMst();

            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);

            model[0].Col9 = "INSURANCE REVALUATION";
            model[0].Col10 = "enx_tab";
            model[0].Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            model[0].Col12 = "IOR"; // Insurence Revolution

            DataTable dt = sgen.getdata(userCode, "select '' as Add_Asset,'1' as SNo,'-' As Asset_No,'-' as Asset_Name,'-' as Asset_Code," +
            "'-' as PTU_Date,'-' as Cost_Center,'-' as WDV,'0' as Revaluation_Amount from dual");

            model[0].dt1 = dt;
            sgen.SetSession(MyGuid, "BEX_DT", dt);

            return View(model);
        }
        [HttpPost]
        public ActionResult ins_rev(List<Tmodelmain> model, string command, string hftable, string hfrow)
        {
            FillMst(model[0].Col15);
            var tm = model.FirstOrDefault();
            DataTable dtm = sgen.settable(hftable);
            model[0].dt1 = dtm;

            if (command == "New")
            {
                try
                {
                    model = getnew(model);
                    model[0].Col16 = vch_num;
                    model[0].Col13 = "Save";
                }
                catch (Exception ex) { }
            }

            else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
            {
                try
                {
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime(userCode);
                    type_desc = "";
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
                    //DataTable dataTable = sgen.getdata(userCode, "select * from " + model[0].Col10.Trim() + "  where  1=2");
                    DataTable dataTable = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    DataRow dr = dataTable.NewRow();
                    for (int i = 0; i < model[0].dt1.Rows.Count; i++)
                    {
                        dr["rec_id"] = "0";
                        dr["type"] = model[0].Col12;
                        dr["vch_num"] = vch_num.Trim();
                        dr["vch_date"] = currdate;
                        dr["date1"] = currdate;
                        dr["date2"] = sgen.Savedate(model[0].Col17, false);
                        dr["col2"] = model[0].Col18;
                        dr["col3"] = model[0].dt1.Rows[i][8].ToString();     //revaluation amt
                        dr["col4"] = model[0].dt1.Rows[i][2].ToString();     //Asset vchnum
                    }
                    dr = getsave(currdate, dr, model);


                    dataTable.Rows.Add(dr);
                    bool Result = sgen.Update_data(userCode, dataTable, model[0].Col10, tmodel.Col8, isedit);
                    if (Result == true)
                    {
                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        ViewBag.vsavenew = "disabled='disabled'";
                        //ViewBag.scripCall = "showmsgJS(1, 'Record Saved Successfully', 1);disableForm();";
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col16 = "",
                            Col17 = "",
                            Col1 = "",
                            Col9 = tm.Col9,
                            Col10 = tm.Col10,
                            Col11 = tm.Col11,
                            Col12 = tm.Col12,
                            Col13 = tm.Col13,
                            Col14 = tm.Col14,
                            Col15 = tm.Col15,
                            Col18 = "",
                            dt1 = (DataTable)sgen.GetSession(MyGuid, "BEX_DT")

                        }
                   );
                    }
                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
            }
            else if (command == "Cancel")
            {
                return CancelFun(model);
            }
            else if (command == "Callback")
            {
                model = StartCallback(model);
            }
            else if (command == "-")
            {
                if (model[0].dt1.Rows.Count > 1) model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                else { model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "BEX_DT"); }
            }
            ModelState.Clear();
            return View(model);
        }
        #endregion

        #region dep

        public ActionResult dep()
        {
            FillMst();

            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);

            model[0].Col9 = "Depreciation";
            model[0].Col10 = "enx_tab";
            model[0].Col12 = ""; // Depreciation
            return View(model);
        }
        [HttpPost]
        public ActionResult dep(List<Tmodelmain> model, string command)
        {
            FillMst(model[0].Col15);
            var tm = model.FirstOrDefault();
            if (command == "New")
            {
                try
                {
                    model = getnew(model);

                    model[0].Col16 = vch_num;
                    model[0].Col13 = "Save";
                }
                catch (Exception ex) { }
            }

            else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
            {
                try
                {
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime(userCode);
                    type_desc = "";
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
                    //DataTable dataTable = sgen.getdata(userCode, "select * from " + model[0].Col10.Trim() + "  where  1=2");
                    DataTable dataTable = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    DataRow dr = dataTable.NewRow();
                    dr["type"] = model[0].Col12;
                    dr["vch_num"] = vch_num.Trim();
                    dr["vch_date"] = currdate;
                    dr["date1"] = currdate;
                    dr["col1"] = model[0].Col29;
                    dr["col2"] = model[0].Col18;
                    dr["col3"] = model[0].Col30;
                    dr["col4"] = model[0].Col19;
                    dr["col7"] = model[0].Col32;
                    dr["col8"] = model[0].Col33;
                    if (isedit)
                    {
                        dr["rec_id"] = model[0].Col7;
                        dr["ent_by"] = model[0].Col3;
                        dr["ent_date"] = model[0].Col4;
                        dr["client_id"] = model[0].Col1;
                        dr["client_unit_id"] = model[0].Col2;
                        dr["edit_by"] = userid_mst;
                        dr["edit_date"] = currdate;
                    }
                    else
                    {
                        dr["rec_id"] = "0";
                        dr["ent_by"] = userid_mst;
                        dr["ent_date"] = currdate;
                        dr["client_id"] = clientid_mst;
                        dr["client_unit_id"] = unitid_mst;
                        dr["edit_by"] = "-";
                        dr["edit_date"] = currdate;
                    }
                    dataTable.Rows.Add(dr);
                    bool Result = sgen.Update_data(userCode, dataTable, model[0].Col10, tmodel.Col8, isedit);
                    if (Result == true)
                    {
                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        ViewBag.vsavenew = "disabled='disabled'";
                        //ViewBag.scripCall = "showmsgJS(1, 'Record Saved Successfully', 1);disableForm();";
                        ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col17 = "",
                            Col1 = "",
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

        #region amc_ent

        public ActionResult amc_ent()
        {
            FillMst();

            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "AMC ENTRY";
            model[0].Col10 = "kc_tab";
            model[0].Col37 = "file_tab";
            model[0].Col32 = "Choose File";
            model[0].Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            model[0].Col12 = "AMC"; // AMC OF ASSETS

            DataTable dt = sgen.getdata(userCode, "select '' as Add_Asset,'1' as SNo,'-' As Asset_No,'-' as Bill_No,'-' as Asset_Name,'-' as PTU_Date,'-' Minor_Head,'-'" +
                         " as Major_Head,'-' as Asset_Code,'-' as Bill_Date,'0' as Quantity,'-' as Capex_Co_Act,'-' as Capex_IT," +
                        "'-' as Location,'-' as Sub_Area,'-' as Cost_Center,'-' as Description,'-' as Warranty,'-' as WDV_Co_Act,'-' as WDV_IT_Act" +
                        ",'-' as Life_in_Years,'-' as Valid_Up_To,'-' as Remark from dual");

            model[0].dt1 = dt;
            sgen.SetSession(MyGuid, "BEX_DT", dt);

            return View(model);
        }
        public List<Tmodelmain> new_amc_entry(List<Tmodelmain> model)
        {
            model = getnew(model);
            return model;
        }

        [HttpPost]
        public ActionResult amc_ent(List<Tmodelmain> model, string command, string hftable, string hfrow, HttpPostedFileBase[] upd1)
        {
            FillMst(model[0].Col15);
            var tm = model.FirstOrDefault();
            IList<HttpPostedFileBase> fileCollection1 = new List<HttpPostedFileBase>();
            DataTable dtm = sgen.settable(hftable);
            model[0].dt1 = dtm;

            if (command == "New")
            {
                try
                {
                    model = new_amc_entry(model);
                }
                catch (Exception ex) { }
            }
            else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
            {
                try
                {
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime(userCode);
                    type_desc = "";
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
                    DataTable dataTable = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    DataRow dr = dataTable.NewRow();
                    for (int i = 0; i < model[0].dt1.Rows.Count; i++)
                    {
                        dr["rec_id"] = "0";
                        dr["type"] = model[0].Col12;
                        dr["vch_num"] = vch_num.Trim();
                        dr["col1"] = model[0].Col17;                       //Bill no
                        dr["date1"] = model[0].Col18;                       //Bill Date
                        dr["col2"] = model[0].Col19;                       //Bill Amount
                        dr["col3"] = model[0].Col20;                      // Remark
                        dr["col7"] = model[0].Col40;                      // Party_id
                        dr["date2"] = sgen.Savedate(model[0].dt1.Rows[i][21].ToString(), false); //Valid Up To
                        dr["col5"] = model[0].dt1.Rows[i][22].ToString();   //Remark
                        dr["col6"] = model[0].dt1.Rows[i][2].ToString();     //Asset_code
                    }
                    dr = getsave(currdate, dr, model);
                    dataTable.Rows.Add(dr);
                    bool Result = sgen.Update_data_fast1(userCode, dataTable, model[0].Col10, tmodel.Col8, isedit);
                    if (Result == true)
                    {
                        vch_num = model[0].Col16;
                        DataTable dtfile = new DataTable();
                        dtfile = cmd_Fun.GetStructure(userCode, "file_tab");
                        type_desc = "AMC Entry";
                        string serverpath = Server.MapPath("~/Uploads/" + cg_com_name.Replace(" ", "") + "/");
                        try
                        {
                            foreach (HttpPostedFileBase file in upd1)
                            {
                                HttpPostedFileBase file1 = file;
                                ctype1 = file1.ContentType;
                                fileName1 = file1.FileName;
                                path1 = clientid_mst + unitid_mst + userid_mst + currdate + "_" + fileName1;
                                encpath1 = sgen.Convert_Stringto64(path1).ToString();
                                finalpath1 = serverpath + encpath1;
                                file1.SaveAs(finalpath1);
                                filesave(model, currdate, dtfile, fileName1, encpath1, "AMC Entry", ctype1, "");
                            }
                        }

                        catch (Exception ex) { }
                        bool res = sgen.Update_data_fast1(userCode, dtfile, model[0].Col37, model[0].Col38, false);
                        if (Result)
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
                                    model = new_amc_entry(model);
                                    ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                                }
                                catch (Exception ex) { }
                            }
                        }
                        else { ViewBag.scripCall = "showmsgJS(1, 'Something Went Wrong, Data Not Saved', 0);"; }
                    }
                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
            }
            else if (command == "Cancel")
            {
                return CancelFun(model);
            }
            else if (command == "Callback")
            {
                if (sgen.GetSession(MyGuid, "delid") != null) { btnval = "FDEL"; sgen.SetSession(MyGuid, "btnval", btnval); }
                model = StartCallback(model);
            }
            else if (btnval == "FDEL")
            {
                ViewBag.vnew = "";
                ViewBag.vedit = "";
                ViewBag.vsave = "disabled='disabled'";
                ViewBag.vsavenew = "disabled='disabled'";
                ViewBag.scripCall = "showmsgJS(1, 'File Deleted Permanently, Press New to create a new file', 1);disableForm();";

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
                });

            }

            else if (command == "-")
            {
                if (model[0].dt1.Rows.Count > 1) model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                else { model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "BEX_DT"); }
            }
            ModelState.Clear();
            return View(model);
        }
        [HttpGet]
        public FileResult fdown(string value, string typ)
        {
            FillMst();

            if (!value.Trim().Equals(""))
            {
                type = typ;
                DataTable dt2 = new DataTable();
                mq = "select file_name,file_path from file_tab where rec_id='" + value.Trim() + "' and type='" + type + "' and client_id='"
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
        #endregion

        #region Calibration
        public ActionResult calibration()
        {
            FillMst();

            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            List<SelectListItem> mod1 = new List<SelectListItem>();
            model[0].Col9 = "Calibration Required";
            model[0].Col10 = "enx_tab";
            model[0].Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            model[0].Col12 = "CLB"; // Calibration
            model[0].TList1 = mod1;
            model[0].SelectedItems1 = new string[] { "" };                              //GD_Frequency

            //model[0].dt1 = dt;
            //sgen.GetSession(MyGuid,"BEX_DT") = dt;

            return View(model);
        }
        [HttpPost]
        public ActionResult calibration(List<Tmodelmain> model, string command)
        {
            FillMst(model[0].Col15);
            var tm = model.FirstOrDefault();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            TempData[MyGuid + "_Tlist1"] = model[0].TList1 = mod1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
      
            if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };

            foreach (var tm2 in model)
            {

                tm2.TList1 = mod1;

            }

            if (command == "New")
            {
                try
                {
                    mod1 = new List<SelectListItem>();


                    model = getnew(model);
                    model[0].Col16 = vch_num;
                    model[0].Col13 = "Save";
                    mq = "select vm.col1 as Asset_Name,vm.vch_num as Asset_no,to_char(vm.date1, '" + sgen.Getsqldateformat() + "') as Put_To_Use_Date," +
                          "'-' as Asset_code,to_char(vm.date3, '" + sgen.Getsqldateformat() + "') as Warrenty_Expires_on" +
                          " from vehicle_master vm " +
                          "where vm.type = 'FAE' and vm.client_unit_id = '" + unitid_mst + "'";

                    #region DD1
                    mq1 = "select frequency_id,frequencyname from school_frequency where client_unit_id='" + unitid_mst + "'";

                    DataTable dt1 = sgen.getdata(userCode, mq1);
                    TempData[MyGuid + "_Tlist1"] = mod1 = sgen.dt_to_selectlist(dt1);
                
                    #endregion

                    DataTable dt = sgen.getdata(userCode, mq);
                    model = new List<Tmodelmain>();
                    if (dt.Rows.Count > 0)
                    {

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            Tmodelmain tmm = new Tmodelmain();


                            tmm.Col17 = dt.Rows[i]["Asset_Name"].ToString();
                            tmm.Col19 = dt.Rows[i]["Asset_no"].ToString();
                            tmm.Col18 = dt.Rows[i]["Asset_code"].ToString();
                            //tmm.Col20 = dt.Rows[i]["Warrenty_Expires_on"].ToString();
                            tmm.SelectedItems1 = dt1.Rows[0]["frequencyname"].ToString().Split(',');
                            tmm.TList1 = mod1;
                            tmm.Col21 = tm.Col21;
                            tmm.Col14 = tm.Col14;
                            tmm.Col15 = tm.Col15;
                            tmm.Col9 = tm.Col9;
                            tmm.Col10 = tm.Col10;
                            tmm.Col20 = tm.Col20;
                            tmm.Col18 = tm.Col18;
                            tmm.Col13 = tm.Col13;
                            tmm.Col12 = tm.Col12;
                            tmm.Col11 = tm.Col11;
                            model.Add(tmm);
                        }

                    }

                    else
                    {

                        model = new List<Tmodelmain>();
                        tm.Col16 = "";
                        tm.Col17 = "";
                        tm.Col18 = "";
                        tm.Col19 = "";
                        model.Add(tm);
                    }



                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
            }
            else if (command == "Save" || command == "Update")
            {
                try
                {
                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime(userCode);
                    type_desc = "";
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
                    model[0].Col16 = vch_num;
                    DataTable dtstr = sgen.getdata(userCode, "select  * from " + model[0].Col10 + " where 1=2");

                    for (int i = 0; i < model.Count; i++)
                    {
                        DataRow dr = dtstr.NewRow();
                        if (model[i].SelectedItems1 == null) model[i].SelectedItems1 = new string[] { "" };
                        if (model[0].Col16 != null)
                        {
                            dr["type"] = model[0].Col12;
                            dr["vch_num"] = model[i].Col16;
                            dr["col1"] = model[i].Col19;
                            dr["col2"] = model[i].Col20;
                            dr["col3"] = model[i].Col21;
                            dr["col4"] = model[i].SelectedItems1.FirstOrDefault();
                        }
                        dr = getsave(currdate, dr, model);
                        dtstr.Rows.Add(dr);
                    }

                    bool Result = sgen.Update_data(userCode, dtstr, model[0].Col10.Trim(), model[0].Col8, isedit);
                    if (Result == true)
                    {
                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        ViewBag.scripCall = "showmsgJS(1, 'Data Saved Successfully', 1);disableForm();";

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

                        });
                    }
                    else
                    {
                        ViewBag.scripCall = "showmsgJS(1, 'Record Not Saved', 2);";
                    }

                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
            }
            else if (command == "Cancel")
            {
                return CancelFun(model);
            }
            else if (command == "Callback")
            {
                model = StartCallback(model);
            }

            TempData[MyGuid + "_Tlist1"] = model.FirstOrDefault().TList1;



            foreach (var tm2 in model)
            {

                tm2.TList1 = mod1;

            }
            ModelState.Clear();
            return View(model);
        }
        #endregion
    }
}