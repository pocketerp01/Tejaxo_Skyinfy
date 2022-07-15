using skyinfyMVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Web.Mvc;
using System.Threading;
using System.Net;

namespace skyinfyMVC.Controllers
{
    public class TrainingController : Controller
    {

        bool res = false;
        static string MyGuid = "";
        sgenFun sgen;
        Cmd_Fun cmd_Fun;
        public static bool isedit = false;
        public static int cli = 0;
        static string mq = "", mq1 = "", vch_num = "", btnval = "", tab_name = "", cmd = "", type = "", where = "", type_desc = "", tab_name1 = "", mid_mst = "", m_id_mst = "", Ac_Year_id = "", isgstr = "N";
        public static string userCode = "", userid_mst = "", unitname_mst = "", subdomain_mst = "", clientname_mst = "", cg_com_name = "", role_mst = "", clientid_mst = "", unitid_mst = "", actionName = "", controllerName = "";
        string cond = "", mod_status = "", ent_date = "", smtpvalue = "", emailIdvalue = "", passwordvalue = "";
        int portvalue = 0;

        //     public static string name = "", ctype1 = "", ctype2 = "", ctype3 = "", ctype4 = "", ctype5 = "", ctype6 = "", ctype7 = "",
        //finalpath1 = "", finalpath2 = "", finalpath3 = "", finalpath4 = "", finalpath5 = "", finalpath6 = "", finalpath7 = "", path1 = "", path2 = "", path3 = "", path4 = "",
        //path5 = "", path6 = "", path7 = "", fileName1 = "", fileName2 = "", fileName3 = "", fileName4 = "", fileName5 = "", fileName6 = "", fileName7 = "", encpath1 = "",
        //encpath2 = "", encpath3 = "", encpath4 = "", encpath5 = "", encpath6 = "", encpath7 = "", m_module3 = "";


        public static string name = "", ctype1 = "", ctype2 = "", ctype3 = "", ctype4 = "", ctype5 = "", ctype6 = "", ctype7 = "",
            finalpath1 = "", finalpath2 = "", finalpath3 = "", finalpath4 = "", finalpath5 = "", finalpath6 = "", finalpath7 = "", path1 = "", path2 = "", path3 = "", path4 = "",
        path5 = "", path6 = "", path7 = "", fileName1 = "", fileName2 = "", fileName3 = "", fileName4 = "", fileName5 = "", fileName6 = "", fileName7 = "", encpath1 = "",
        encpath2 = "", encpath3 = "", encpath4 = "", encpath5 = "", encpath6 = "", encpath7 = "";

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
            m_id_mst = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.scripCall = "disableForm();";
            tm1.Col13 = "Save";
            ViewBag.vsave = "disabled='disabled'";
            ViewBag.vsavenew = "disabled='disabled'";
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
            //if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
            if (sgen.GetSession(MyGuid, "delid") != null) { btnval = "FDEL"; }
            else if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
            model = CallbackFun(btnval, "N", actionName, controllerName, model);
            ViewBag.vnew = "disabled='disabled'";
            ViewBag.vedit = "disabled='disabled'";
            ViewBag.vsave = "";
            return model;
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

        private ActionResult CancelFun(List<Tmodelmain> model)
        {
            return RedirectToAction(actionName, new { @m_id = EncryptDecrypt.Encrypt(model[0].Col15), @mid = EncryptDecrypt.Encrypt(model[0].Col14) });
        }

        [HttpGet]
        public FileResult fdown(string value, string typ, string Myguid,string tab_name)
        {
            FillMst(MyGuid);

            if (!value.Trim().Equals(""))
            {
                type = typ;
                DataTable dt2 = new DataTable();
                if (tab_name == "file_tab")
                {
                    mq = "select file_name,file_path from " + tab_name + " where rec_id='" + value.Trim() + "' and type='" + typ + "' and client_id='"
                        + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
                }

                else
                {
                    mq = "select option_filename file_name,option_filepath file_path from " + tab_name + " where rec_id='" + value.Trim() + "' and type='" + typ + "' and client_id='"
                       + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
                }
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

        // GET: Training
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
            //FillMst();
            DataTable dt = new DataTable();
            String URL = sgen.GetSession(MyGuid, "SSEEKVAL").ToString();
            btnval = btnval.ToUpper();
            List<Tmodelmain> rpt_model = new List<Tmodelmain>();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<SelectListItem> mod2 = new List<SelectListItem>();
            List<SelectListItem> mod3 = new List<SelectListItem>();
            List<SelectListItem> mod4 = new List<SelectListItem>();
            List<SelectListItem> mod5 = new List<SelectListItem>();

            DataTable dtt = new DataTable();
            var tm = model.FirstOrDefault();
            switch (viewName.ToLower())
            {
                #region upload training

                case "upload_training":
                    switch (btnval)
                    {
                        case "MODULE":
                            mq = " SELECT (a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date, 'yyyymmdd')||a.type) AS fstr, " +
                                " a.ref_code ,a.course_id,a.cou_title  ,b.CAT_ID, b.cat_name,c.MOD_ID, c.mod_name from courses a inner join" +
                                "  category b on a.cat_id =b.cat_id  and b.type = 'TCT' and a.client_unit_id=b.client_unit_id inner join module c" +
                                "  on c.type = 'TMD' and a.mod_id = c.mod_id and a.client_unit_id=c.client_unit_id where " +
                                "  (a.client_id|| a.client_unit_id||a.vch_num||to_char(a.vch_date, 'yyyymmdd')||a.type) = '" + URL + "'";

                            dt = new DataTable();
                            dt = sgen.getdata(userCode, mq);

                            if (dt.Rows.Count > 0)
                            {
                                model[0].Col17 = dt.Rows[0]["MOD_ID"].ToString();
                                model[0].Col18 = dt.Rows[0]["mod_name"].ToString();
                                model[0].Col19 = dt.Rows[0]["CAT_ID"].ToString();
                                model[0].Col20 = dt.Rows[0]["cat_name"].ToString();
                                model[0].Col21 = dt.Rows[0]["course_id"].ToString();
                                model[0].Col22 = dt.Rows[0]["cou_title"].ToString();
                                model[0].Col23 = dt.Rows[0]["ref_code"].ToString();

                            }
                           
                            break;
                        case "EDIT":
                            sgen.SetSession(MyGuid, "EDMODE", "Y");
                            mq = " SELECT a.vch_num, a.rec_id,a.client_id,a.client_unit_id,  cou_content_ent_by,a.cou_content_ent_dt," +
                                "a.ref_code ,a.course_id,a.cou_title  ,c.CAT_ID, c.cat_name,b.MOD_ID, b.mod_name ,a.trn_duration,a.training_level " +
                                "from course_content a inner join  module b on a.mod_id = b.mod_id  and b.type = 'TMD' and " +
                                "a.client_unit_id = b.client_unit_id  inner join  category c on a.cat_id = c.cat_id  and c.type = 'TCT'" +
                                " and b.mod_id = c.mod_id and a.client_unit_id = c.client_unit_id where " +
                                "  (a.client_id|| a.client_unit_id|| a.vch_num||to_char(a.vch_date, 'yyyymmdd')|| a.type) = '" + URL + "'";

                            dt = new DataTable();
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {

                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1 = cmd_Fun.traininglevel(userCode, unitid_mst);
                                model[0].Col16 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dt.Rows[0]["MOD_ID"].ToString();
                                model[0].Col18 = dt.Rows[0]["mod_name"].ToString();
                                model[0].Col19 = dt.Rows[0]["CAT_ID"].ToString();
                                model[0].Col20 = dt.Rows[0]["cat_name"].ToString();
                                model[0].Col21 = dt.Rows[0]["course_id"].ToString();
                                model[0].Col22 = dt.Rows[0]["cou_title"].ToString();
                                model[0].Col23 = dt.Rows[0]["ref_code"].ToString();
                                //model[0].Col20 = dt.Rows[0]["REC_ID"].ToString();
                          
                                model[0].Col3 = dt.Rows[0]["cou_content_ent_by"].ToString();
                                model[0].Col4 = dt.Rows[0]["cou_content_ent_dt"].ToString();
                                model[0].Col1 = dt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col7 = dt.Rows[0]["rec_id"].ToString();
                                model[0].Col8 = "(client_id||client_unit_id||vch_num||to_char(vch_date, 'yyyymmdd')|| type) = '" + URL + "'";
                                model[0].Col24 = dt.Rows[0]["trn_duration"].ToString();
                                model[0].Col13 = "Update";
                                model[0].Col121 = "U";
                                model[0].Col122 = "<u>U</u>pdate";
                                model[0].Col123 = "Update & Ne<u>w</u>";
                                String[] L1 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["training_level"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems1 = L1;

                                DataTable dtf = new DataTable();
                                dtf = sgen.getdata(userCode, "select client_id||client_unit_id||vch_num||to_char(vch_date,'yyyymmdd')||type as fstr, rec_id,file_name,file_path,col1,col2,description,Col3 from file_tab where vch_num='" + dt.Rows[0]["vch_num"].ToString() + "' and type = " +
                                    " '" + model[0].Col12 + "' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "' and col1='UPD TRAIN'");

                                //if (dtf.Rows.Count > 0)
                                //{
                                //    model[0].Col29 = dtf.Rows[0]["file_name"].ToString();
                                //    model[0].Col26 = dtf.Rows[0]["file_path"].ToString();
                                //    model[0].Col25 = dtf.Rows[0]["col1"].ToString();
                                //    model[0].Col28 = "(client_id||client_unit_id|| vch_num|| to_char(vch_date, 'yyyymmdd')|| type) = '" + dtf.Rows[0]["fstr"].ToString() + "'";
                                //}

                                List<Tmodelmain> ltmf = new List<Tmodelmain>();
                                foreach (DataRow drf in dtf.Rows)
                                {
                                    Tmodelmain tmf = new Tmodelmain();
                                    tmf.Col4 = drf["file_path"].ToString();
                                    tmf.Col1 = drf["file_name"].ToString();
                                    tmf.Col2 = drf["col1"].ToString();
                                    tmf.Col3 = drf["rec_id"].ToString();
                                    tmf.Col5 = drf["col2"].ToString();
                                    tmf.Col28 = "(client_id||client_unit_id|| vch_num|| to_char(vch_date, 'yyyymmdd')|| type) = '" + dtf.Rows[0]["fstr"].ToString() + "'";
                                    ltmf.Add(tmf);
                                }
                                model[0].LTM1 = ltmf;
                            }

                            break;

                        case "FDEL":
                          
                        
                            string fid = sgen.GetSession(MyGuid, "delid").ToString();
                            res = sgen.execute_cmd(userCode, "Delete from file_tab WHERE rec_id='" + fid + "' and type='TCR' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
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
                #region module
                case "module1":
                    switch (btnval)
                    {
                        case "EDIT":
                            #region module
                            dtt = new DataTable();
                            if (!sgen.GetSession(MyGuid, "TR_MID").ToString().Trim().Equals(null))
                            {
                                model[0].Col13 = "Update";
                                model[0].Col100 = "Update & New";
                                model[0].Col121 = "U";
                                model[0].Col122 = "<u>U</u>pdate";
                                model[0].Col123 = "Update & Ne<u>w</u>";
                                if (sgen.GetSession(MyGuid, "TR_MID").ToString().Trim().Equals("1013.1"))
                                {
                                    mq = "select mod_id,mod_name,client_id,m_status,client_unit_id,vch_num,vch_date,rec_id,mod_ent_by,mod_ent_date,mod_edit_by,mod_edit_date from module where  " +
                                         "client_id||client_unit_id||vch_num||to_char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";

                                    dtt = new DataTable();
                                    dtt = sgen.getdata(userCode, mq);
                                    if (dtt.Rows.Count > 0)
                                    {
                                        sgen.SetSession(MyGuid, "EDMODE", "Y");
                                        model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                        model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                        model[0].Col3 = dtt.Rows[0]["mod_ent_by"].ToString();
                                        model[0].Col4 = dtt.Rows[0]["mod_ent_date"].ToString();
                                        model[0].Col7 = dtt.Rows[0]["rec_id"].ToString();
                                        model[0].Col8 = "client_id||client_unit_id||vch_num|| to_char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";
                                        model[0].Col16 = dtt.Rows[0]["vch_num"].ToString();
                                        model[0].Col22 = dtt.Rows[0]["mod_name"].ToString();
                                        model[0].Col23 = dtt.Rows[0]["m_status"].ToString();

                                    }
                                }
                                else if (sgen.GetSession(MyGuid, "TR_MID").ToString().Trim().Equals("1013.2"))
                                {
                                    mq = "select mod_id,cat_id,cat_name,client_id,c_status,client_unit_id,vch_num,vch_date,rec_id,ent_by,ent_date,edit_by,edit_date from category where  " +
                                        "client_id||client_unit_id||vch_num||to_char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";
                                    dtt = sgen.getdata(userCode, mq);

                                    #region bindmodule
                                    DataTable dt1 = new DataTable();
                                    mod1 = new List<SelectListItem>();
                                    dt1 = sgen.getdata(userCode, "select mod_id,mod_name from module where type='TMD'");
                                    if (dt1.Rows.Count > 0)
                                    {
                                        foreach (DataRow dr in dt1.Rows)
                                        {
                                            mod1.Add(new SelectListItem { Text = dr["mod_name"].ToString(), Value = dr["mod_id"].ToString() });
                                        }
                                    }
                                    TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;

                                    #endregion
                                    if (dtt.Rows.Count > 0)
                                    {
                                        sgen.SetSession(MyGuid, "EDMODE", "Y");
                                        model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                        model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                        model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                                        model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                                        model[0].Col7 = dtt.Rows[0]["rec_id"].ToString();
                                        model[0].Col8 = "client_id||client_unit_id||vch_num|| to_char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";
                                        model[0].Col16 = dtt.Rows[0]["vch_num"].ToString();
                                        model[0].Col22 = dtt.Rows[0]["cat_name"].ToString();
                                        model[0].Col23 = dtt.Rows[0]["c_status"].ToString();
                                        String[] L1 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["mod_id"].ToString()).Distinct()).Split(',');
                                        model[0].SelectedItems1 = L1;
                                    }
                                }
                                else if (sgen.GetSession(MyGuid, "TR_MID").ToString().Trim().Equals("1013.3"))
                                {
                                    mq = "select c.course_id,c.mod_id,c.cat_id,c.cou_title,c.cou_descp,c.client_id,c.cou_status,c.client_unit_id,c.vch_num,c.vch_date,c.rec_id,c.cou_ent_by,c.cou_ent_date,c.cou_edit_by,c.cou_edit_date,c.ref_code,m.mod_name,ct.cat_name from courses c inner join module m on m.mod_id = c.mod_id and m.type = 'TMD' inner join category ct on ct.cat_id = c.cat_id and ct.type = 'TCT' where  " +
                                        "c.client_id||c.client_unit_id||c.vch_num||to_char(c.vch_date, 'yyyymmdd')||c. type = '" + URL + "'";

                                    dtt = sgen.getdata(userCode, mq);
                                    if (dtt.Rows.Count > 0)
                                    {
                                        sgen.SetSession(MyGuid, "EDMODE", "Y");
                                        model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                        model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                        model[0].Col3 = dtt.Rows[0]["cou_ent_by"].ToString();
                                        model[0].Col4 = dtt.Rows[0]["cou_ent_date"].ToString();
                                        model[0].Col7 = dtt.Rows[0]["rec_id"].ToString();
                                        model[0].Col8 = "client_id||client_unit_id||vch_num|| to_char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";
                                        model[0].Col16 = dtt.Rows[0]["vch_num"].ToString();
                                        model[0].Col22 = dtt.Rows[0]["cou_title"].ToString();
                                        model[0].Col19 = dtt.Rows[0]["cou_descp"].ToString();
                                        model[0].Col23 = dtt.Rows[0]["cou_status"].ToString();
                                        model[0].Col24 = dtt.Rows[0]["cat_id"].ToString();
                                        model[0].Col25 = dtt.Rows[0]["mod_id"].ToString();
                                        model[0].Col17 = dtt.Rows[0]["mod_name"].ToString();
                                        model[0].Col18 = dtt.Rows[0]["cat_name"].ToString();
                                        model[0].Col20 = dtt.Rows[0]["ref_code"].ToString();

                                    }
                                }
                            }
                            #endregion

                            break;

                        case "ADD2":
                            mq = "select ct.mod_id,ct.cat_id,m.mod_name,ct.cat_name from category ct inner join module m on m.mod_id=ct.mod_id and m.type='TMD' where ct.client_id||ct.client_unit_id||ct.vch_num||to_char(ct.vch_date, 'yyyymmdd')|| ct.type = '" + URL + "'";
                            var tmm = model.FirstOrDefault();
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                DataRow dr = dtt.Rows[0];
                                model[0].Col17 = dr["mod_name"].ToString();
                                model[0].Col25 = dr["mod_id"].ToString();
                                model[0].Col18 = dr["cat_name"].ToString();
                                model[0].Col24 = dr["cat_id"].ToString();
                            }
                            break;
                    }
                    break;

                #endregion
                #region add_grp
                case "add_grp":
                    switch (btnval)
                    {
                        case "EDIT":
                            dtt = new DataTable();
                            model[0].Col13 = "Update";
                            model[0].Col100 = "Update & New";
                            model[0].Col121 = "U";
                            model[0].Col122 = "<u>U</u>pdate";
                            model[0].Col123 = "Update & Ne<u>w</u>";
                            mq = "select vch_num,group_id,group_name,rec_id,description,type,client_id,client_unit_id,ent_by,ent_date,vch_date" +
                                " from group_details where client_id || client_unit_id || vch_num || to_char(vch_date, 'yyyymmdd') || type = '" + URL + "'";

                            dtt = new DataTable();
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                                model[0].Col7 = dtt.Rows[0]["rec_id"].ToString();
                                model[0].Col8 = "client_id||client_unit_id||vch_num|| to_char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";
                                model[0].Col16 = dtt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dtt.Rows[0]["group_name"].ToString();
                                model[0].Col18 = dtt.Rows[0]["description"].ToString();
                                model[0].Col19 = dtt.Rows[0]["group_id"].ToString();
                            }
                            break;
                    }
                    break;

                #endregion
                #region make_quiz
                case "make_quiz":
                    switch (btnval)
                    {
                        case "CHOOSETITLE":
                            mq1 = "select course_id,mod_id,cat_id,ref_code from courses where client_id||client_unit_id||course_id||to_char(vch_date, 'yyyymmdd')|| type='" + URL + "'";
                            dt = sgen.getdata(userCode, mq1);
                            if (dt.Rows.Count > 0)
                            {
                                TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1 = cmd_Fun.tr_mod(userCode, unitid_mst);

                                TempData[MyGuid + "_TList2"] = model[0].TList2 = mod2 = cmd_Fun.getcatagory(userCode, unitid_mst);
                                TempData[MyGuid + "_TList3"] = model[0].TList3 = mod3 = cmd_Fun.gettitle(userCode, unitid_mst);
                                String[] L1 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["mod_id"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems1 = L1;
                                String[] L2 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["cat_id"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems2 = L2;
                                String[] L3 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["course_id"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems3 = L3;
                                model[0].Col18 = dt.Rows[0]["ref_code"].ToString();
                                DataTable dtno = new DataTable();
                                string mq0 = "select count(ques_id) as noofques from questions where mod_id='" + dt.Rows[0]["mod_id"].ToString() + "' and cat_id='" + dt.Rows[0]["cat_id"].ToString() + "' and cou_title='" + dt.Rows[0]["course_id"].ToString() + "' and ques_ent_by='" + userid_mst + "'";
                                dtno = sgen.getdata(userCode, mq0);
                                if (dtno.Rows.Count > 0) model[0].Col17 = dtno.Rows[0]["noofques"].ToString();
                                else model[0].Col17 = "No Questions";

                                DataTable dtques = new DataTable();
                                mq = "select questions.ques_id,questions.question,questions.mod_id,questions.cat_id,questions.ref_code," +
                                    "questions.ans_explanation,module.mod_name,category.cat_name,courses.cou_title,questions.ques_ent_by from questions inner join module on " +
                                    "module.mod_id=questions.mod_id and module.type='TMD' and  module.client_unit_id='" + unitid_mst + "'inner join category on category.cat_id = questions.cat_id and category.type='TCT'" +
                                    " and questions.mod_id=category.mod_id  and category.client_unit_id='" + unitid_mst + "' inner join " +
                                    "courses on courses.ref_code = questions.ref_code and courses.type='TCR' and courses.client_unit_id='" + unitid_mst + "'  where questions.mod_id='" + dt.Rows[0]["mod_id"].ToString()
                                    + "' and questions.cat_id='" + dt.Rows[0]["cat_id"].ToString() + "' and questions.cou_title='" + dt.Rows[0]["course_id"].ToString()
                                    + "' and questions.ques_ent_by='" + userid_mst + "' and questions.type='TQU' and questions.client_id='" + clientid_mst + "' and questions.client_unit_id='" + unitid_mst + "'";
                                dtques = sgen.getdata(userCode, mq);
                                if (dtques.Rows.Count > 0)
                                {
                                    model = new List<Tmodelmain>();
                                    foreach (DataRow dr in dtques.Rows)
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
                                            Chk1 = tm.Chk1,
                                            Col16 = tm.Col16,
                                            Col17 = tm.Col17,
                                            Col18 = tm.Col18,
                                            Col19 = tm.Col19,
                                            Col20 = tm.Col20,
                                            Col100 = tm.Col100,
                                            Col121 = tm.Col121,
                                            Col122 = tm.Col122,
                                            Col123 = tm.Col123,

                                            Col21 = dr["ques_id"].ToString(),
                                            Col22 = dr["MOD_NAME"].ToString(),
                                            Col23 = dr["CAT_NAME"].ToString(),
                                            Col24 = dr["COU_TITLE"].ToString(),
                                            Col25 = dr["QUESTION"].ToString(),
                                            TList1 = tm.TList1,
                                            SelectedItems1 = tm.SelectedItems1,
                                            TList2 = tm.TList2,
                                            SelectedItems2 = tm.SelectedItems2,
                                            TList3 = tm.TList3,
                                            SelectedItems3 = tm.SelectedItems3,
                                        });
                                    }
                                }
                            }
                            break;

                        case "EDIT":
                            mq = "select a.ques_paper_id,a.ques_paper_name,a.vch_num,a.mod_id,a.cat_id,a.cou_title,ques_paper_ent_by,a.ques_paper_ent_date from ques_paper a " +
                                "where a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date, 'yyyymmdd')|| a.type='" + URL + "' ";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1 = cmd_Fun.tr_mod(userCode, unitid_mst);
                                TempData[MyGuid + "_TList2"] = model[0].TList2 = mod2 = cmd_Fun.getcatagory(userCode, unitid_mst);
                                TempData[MyGuid + "_TList3"] = model[0].TList3 = mod3 = cmd_Fun.gettitle(userCode, unitid_mst);
                                String[] L1 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["mod_id"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems1 = L1;
                                String[] L2 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["cat_id"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems2 = L2;
                                String[] L3 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["cou_title"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems3 = L3;

                                model[0].Col3 = dt.Rows[0]["ques_paper_ent_by"].ToString();
                                model[0].Col4 = dt.Rows[0]["ques_paper_ent_date"].ToString();
                                model[0].Col16 = dt.Rows[0]["vch_num"].ToString();
                                model[0].Col19 = dt.Rows[0]["ques_paper_name"].ToString();
                                model[0].Col26 = dt.Rows[0]["ques_paper_id"].ToString();
                                model[0].Col8 = "client_id|| client_unit_id|| vch_num|| to_char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";

                                DataTable dtno = new DataTable();
                                string mq0 = "select count(ques_id) as noofques from questions where mod_id='" + dt.Rows[0]["mod_id"].ToString() + "' " +
                                    "and cat_id='" + dt.Rows[0]["cat_id"].ToString() + "' and cou_title='" + dt.Rows[0]["cou_title"].ToString() + "' and ques_ent_by='" + userid_mst + "'";
                                dtno = sgen.getdata(userCode, mq0);
                                if (dtno.Rows.Count > 0) model[0].Col17 = dtno.Rows[0]["noofques"].ToString();
                                else model[0].Col17 = "No Questions";

                                DataTable dtques = new DataTable();
                                mq = "select  a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date, 'yyyymmdd')|| a.type as fstr, a.sel_ques_id,a.ques_id,a.ref_code,q.question,b.MOD_NAME ,c.CAT_NAME ,cr.COU_TITLE from selected_ques a  inner join module b on a.mod_id = b.mod_id  and b.type = 'TMD'" +
                                    " and a.client_unit_id = b.client_unit_id inner join category c on a.cat_id = c.cat_id and a.mod_id = c.mod_id and c.type = 'TCT' and a.client_unit_id = c.client_unit_id " +
                                    " inner join courses cr on cr.course_id = a.cou_title and cr.type = 'TCR' and cr.client_unit_id = a.client_unit_id  inner join questions q on a.ques_id=q.ques_id" +
                                    " and q.type='TQU' and q.client_unit_id = a.client_unit_id where a.vch_num = '" + dt.Rows[0]["vch_num"].ToString() + "' " +
                                    "and a.type = 'TPQ' and a.client_unit_id = '" + unitid_mst + "'";
                                dtques = sgen.getdata(userCode, mq);
                                if (dtques.Rows.Count > 0)
                                {
                                    model = new List<Tmodelmain>();
                                    foreach (DataRow dr in dtques.Rows)
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
                                            Chk1 = true,
                                            Col16 = tm.Col16,
                                            Col17 = tm.Col17,
                                            Col19 = tm.Col19,
                                            Col20 = tm.Col20,
                                            Col100 = tm.Col100,
                                            Col121 = tm.Col121,
                                            Col122 = tm.Col122,
                                            Col123 = tm.Col123,

                                            Col18 = dr["ref_code"].ToString(),
                                            Col21 = dr["ques_id"].ToString(),
                                            Col22 = dr["MOD_NAME"].ToString(),
                                            Col23 = dr["CAT_NAME"].ToString(),
                                            Col24 = dr["COU_TITLE"].ToString(),
                                            Col25 = dr["QUESTION"].ToString(),
                                            Col27 = "client_id|| client_unit_id|| vch_num|| to_char(vch_date, 'yyyymmdd')|| type = '" + dr["fstr"].ToString() + "'",

                                            TList1 = tm.TList1,
                                            SelectedItems1 = tm.SelectedItems1,
                                            TList2 = tm.TList2,
                                            SelectedItems2 = tm.SelectedItems2,
                                            TList3 = tm.TList3,
                                            SelectedItems3 = tm.SelectedItems3,
                                        });
                                    }
                                }
                            }

                            break;

                    }
                    break;

                #endregion
                #region asn_quiz
                case "asn_quiz":
                    switch (btnval)
                    {
                        case "CHOOSETITLE":
                            mq1 = "select qp.cou_title,qp.mod_id,qp.cat_id,cr.ref_code from ques_paper qp inner join courses cr on qp.cou_title = cr.course_id and " +
                                "cr.type = 'TCR' and cr.client_id = qp.client_id and cr.client_unit_id = qp.client_unit_id where " +
                                "qp.client_id|| qp.client_unit_id|| qp.ques_paper_id|| to_char(qp.vch_date, 'yyyymmdd')|| qp.type= '" + URL + "'";
                            dt = sgen.getdata(userCode, mq1);
                            if (dt.Rows.Count > 0)
                            {
                                TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1 = cmd_Fun.tr_mod(userCode, unitid_mst);
                                TempData[MyGuid + "_TList2"] = model[0].TList2 = mod2 = cmd_Fun.getcatagory(userCode, unitid_mst);
                                TempData[MyGuid + "_TList3"] = model[0].TList3 = mod3 = cmd_Fun.gettitle(userCode, unitid_mst);
                                String[] L1 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["mod_id"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems1 = L1;
                                String[] L2 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["cat_id"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems2 = L2;
                                String[] L3 = System.String.Join(",", dt.Rows.OfType<DataRow>().Select(r => r["cou_title"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems3 = L3;
                                mod4.Add(new SelectListItem { Text = "No", Value = "0" });
                                mod4.Add(new SelectListItem { Text = "Yes", Value = "1" });
                                mod5.Add(new SelectListItem { Text = "Created Group", Value = "1" });
                                mod5.Add(new SelectListItem { Text = "Individual", Value = "2" });
                                TempData[MyGuid + "_TList4"] = model[0].TList4 = mod4;
                                TempData[MyGuid + "_TList5"] = model[0].TList5 = mod5;
                                model[0].Col18 = dt.Rows[0]["ref_code"].ToString();
                                DataTable dtques = new DataTable();
                                mq = "select qp.ques_paper_id,qp.ques_paper_name,qp.mod_id,qp.cat_id,qp.cou_title as course_id," +
                                    "module.mod_name as mod_name,category.cat_name as cat_name,courses.cou_title as cou_title," +
                                    "qp.ques_paper_ent_by from ques_paper qp inner join module on module.mod_id=qp.mod_id and module.type=" +
                                    "'TMD' and module.client_unit_id='" + unitid_mst + "' inner join category on category.cat_id = qp.cat_id AND category.type = 'TCT' and module.mod_id=category.mod_id and  category.client_unit_id='" + unitid_mst + "' inner join courses " +
                                    "on courses.COURSE_ID = qp.COU_TITLE and courses.type = 'TCR' AND courses.client_id = qp.client_id " +
                                    "and courses.client_unit_id = qp.client_unit_id where qp.mod_id = '" + dt.Rows[0]["mod_id"].ToString() + "' and " +
                                    "qp.cat_id = '" + dt.Rows[0]["cat_id"].ToString() + "' and qp.cou_title = '" + dt.Rows[0]["cou_title"].ToString() + "' and qp.type = 'TPQ' " +
                                    "and qp.QUES_PAPER_ENT_BY = '" + userid_mst + "'";
                                dtques = sgen.getdata(userCode, mq);
                                if (dtques.Rows.Count > 0)
                                {
                                    model = new List<Tmodelmain>();
                                    foreach (DataRow dr in dtques.Rows)
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
                                            Chk1 = tm.Chk1,
                                            Col16 = tm.Col16,
                                            Col17 = tm.Col17,
                                            Col18 = tm.Col18,
                                            Col19 = tm.Col19,
                                            Col20 = tm.Col20,
                                            Col21 = dr["ques_paper_id"].ToString(),
                                            Col22 = dr["mod_name"].ToString(),
                                            Col23 = dr["cat_name"].ToString(),
                                            Col24 = dr["cou_title"].ToString(),
                                            Col25 = dr["ques_paper_name"].ToString(),
                                            TList1 = tm.TList1,
                                            SelectedItems1 = tm.SelectedItems1,
                                            TList2 = tm.TList2,
                                            SelectedItems2 = tm.SelectedItems2,
                                            TList3 = tm.TList3,
                                            SelectedItems3 = tm.SelectedItems3,
                                            TList4 = tm.TList4,
                                            SelectedItems4 = tm.SelectedItems4,
                                            TList5 = tm.TList5,
                                            SelectedItems5 = tm.SelectedItems5,
                                        });
                                    }
                                }
                            }
                            break;
                        case "UNASSIGN":
                            mq1 = "select com_email,com_password,com_port,com_smtp from company_profile where Company_Profile_Id='" + clientid_mst + "' and type='CP'";
                            dt = sgen.getdata(userCode, mq1);
                            if (dt.Rows.Count > 0)
                            {
                                smtpvalue = Convert.ToString(dt.Rows[0]["com_smtp"]);
                                emailIdvalue = Convert.ToString(dt.Rows[0]["com_email"]);
                                passwordvalue = EncryptDecrypt.Decrypt(dt.Rows[0]["com_password"].ToString());
                                portvalue = Convert.ToInt32(dt.Rows[0]["com_port"]);
                                if (smtpvalue == "" && passwordvalue == "" && portvalue == 0)
                                {
                                    ViewBag.scripCall = "showmsgJS(1, 'Please configure your mail first', 2);";

                                }

                                TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1 = cmd_Fun.tr_mod(userCode, unitid_mst);
                                TempData[MyGuid + "_TList2"] = model[0].TList2 = mod2 = cmd_Fun.getcatagory(userCode, unitid_mst);
                                TempData[MyGuid + "_TList3"] = model[0].TList3 = mod3 = cmd_Fun.gettitle(userCode, unitid_mst);

                                mod4.Add(new SelectListItem { Text = "No", Value = "0" });
                                mod4.Add(new SelectListItem { Text = "Yes", Value = "1" });
                                mod5.Add(new SelectListItem { Text = "Created Group", Value = "1" });
                                mod5.Add(new SelectListItem { Text = "Individual", Value = "2" });
                                TempData[MyGuid + "_TList4"] = model[0].TList4 = mod4;
                                TempData[MyGuid + "_TList5"] = model[0].TList5 = mod5;
                                DataTable dtqp = new DataTable();
                                mq = "select up.user_id,up.vch_num,up.group_name,up.quespaper_name,ud.email,ud.first_name,ud.user_id from user_quespaper up join user_details ud on up.user_id=lpad(ud.rec_id,6,0) where up.client_id||up.client_unit_id||up.vch_num||to_char(up.vch_date, 'yyyymmdd')|| up.type in('" + URL + "')";

                                dtqp = sgen.getdata(userCode, mq);
                                if (dtqp.Rows.Count > 0)
                                {
                                    mq = "delete from user_quespaper WHERE vch_num='" + dtqp.Rows[0]["vch_num"].ToString() + "' and type='TQA' " + where + " ";
                                    res = sgen.execute_cmd(userCode, mq);
                                    if (res == true)
                                    {
                                        for (int k = 0; k < dtqp.Rows.Count; k++)
                                        {
                                            MailMessage msg = new MailMessage();
                                            msg.From = new MailAddress(emailIdvalue);
                                            msg.CC.Add(emailIdvalue);
                                            msg.To.Add(dtqp.Rows[k]["email"].ToString());
                                            msg.Subject = "Quiz Praxis Unassign";
                                            string msgbody = "<b style='color: #3caee9; font-size: 20px'>Quiz Praxis Unassigned By Admin</b><br /><hr style='border:1px solid black' /><p>Hi <b>" + dtqp.Rows[k]["first_name"].ToString() + "</b>,</p><p>This is to notify that you have been unassigned Quiz Praxis. your unassigned quiz praxis details are as below :-</p><table style='font-weight:600'><tr><td>Company Name </td><td>: " + clientname_mst + " </td></tr><tr><td> Unit Name </td><td>: " + unitname_mst + " </td></tr><tr><td> Quiz Praxis </td><td>: " + dtqp.Rows[k]["quespaper_name"].ToString() + " </td></tr><tr><td> URL </td><td>: " + subdomain_mst + " </td></tr></table><br /><p>Please ask your admin for further clarification and information.</p><p>Thank you,<br />Administrator<br /></p><br /><p>Note:- Please do not reply to this mail as it is an unmonitered alias.</p>";
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
                                        ViewBag.scripCall = "showmsgJS(1, 'Quiz paper Unassigned', 1);";
                                    }
                                }
                            }
                            break;
                    }
                    break;

                #endregion
                #region ins_quiz
                case "ins_quiz":
                    switch (btnval)
                    {

                        case "EDIT":

                            mq = "select a.quiz_id, a.client_id, a.client_unit_id,a.rec_id,a.ques_id,a.mod_id,a.cat_id,a.cou_title,a.ref_code,a.question,a.ques_ent_by,a.ques_ent_date," +
                                "a.ans_explanation,a.vch_num ,b.rec_id as ans_rec_id,b.option_filename,b.option_filepath,b.correct_option,b.options" +
                                ",b.vch_num ans_vchnum,nvl(b.img_type,0) img_type , (b.client_id|| b.client_unit_id|| b.vch_num|| to_char(b.vch_date|| 'yyyymmdd')|| b.type) ans_fstr,b.ans_id" +
                                " from questions a inner join answers b on a.ques_id = b.ques_id and b.type = 'TQU'" +
                                " and a.client_unit_id = b.client_unit_id where (a.client_id|| a.client_unit_id||a.vch_num|| to_char(a.vch_date|| 'yyyymmdd')||a.type)='" + URL + "' order by b.ans_id";

                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1 = cmd_Fun.tr_mod(userCode, unitid_mst);
                                model[0].Col13 = "Update";
                                model[0].Col100 = "Update & New";
                                model[0].Col121 = "U";
                                model[0].Col122 = "<u>U</u>pdate";
                                model[0].Col123 = "Update & Ne<u>w</u>";
                                #region question
                                model[0].Col8 = "client_id|| client_unit_id|| vch_num|| to_char(vch_date|| 'yyyymmdd')|| type = '" + URL + "'";
                                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dtt.Rows[0]["ques_ent_by"].ToString();
                                model[0].Col4 = dtt.Rows[0]["ques_ent_date"].ToString();
                                model[0].Col16 = dtt.Rows[0]["vch_num"].ToString();
                                model[0].Col29 = dtt.Rows[0]["ques_id"].ToString();
                                model[0].Col18 = dtt.Rows[0]["ref_code"].ToString();
                                model[0].Col19 = dtt.Rows[0]["question"].ToString();
                                model[0].Col17 = dtt.Rows[0]["quiz_id"].ToString();
                                model[0].Col25 = dtt.Rows[0]["ans_explanation"].ToString();
                                model[0].SelectedItems1 = System.String.Join(",", dtt.Rows[0]["mod_id"].ToString()).Split(',');

                                dt = new DataTable();
                                dt = sgen.getdata(userCode, "select cat_id,cat_name,mod_id from category where mod_id='" + model[0].SelectedItems1.FirstOrDefault() + "' and type='TCT'" +
                                    " and client_unit_id='" + unitid_mst + "'");
                                if (dt.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        mod2.Add(new SelectListItem { Text = dr["cat_name"].ToString(), Value = dr["cat_id"].ToString() });
                                    }

                                }
                                TempData[MyGuid + "_TList2"] = model[0].TList2 = mod2;
                                model[0].SelectedItems2 = System.String.Join(",", dtt.Rows[0]["cat_id"].ToString()).Split(',');

                                mq = "select course_id,cat_id,cou_title,cou_ent_by from courses where cat_id='" + model[0].SelectedItems2.FirstOrDefault() + "' " +
"and cou_ent_by='" + userid_mst + "' and type='TCR' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
                                dt = sgen.getdata(userCode, mq);
                                if (dt.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        mod3.Add(new SelectListItem { Text = dr["cou_title"].ToString(), Value = dr["course_id"].ToString() });
                                    }

                                }
                                TempData[MyGuid + "_TList3"] = model[0].TList3 = mod3;
                                model[0].SelectedItems3 = System.String.Join(",", dtt.Rows[0]["cou_title"].ToString()).Split(',');
                                #endregion


                                #region answers

                                model[0].Col27 = dtt.Rows[0]["ans_vchnum"].ToString();                             
                                model[0].Col28 = "client_id|| client_unit_id|| vch_num|| to_char(vch_date|| 'yyyymmdd')|| type = '" + dtt.Rows[0]["ans_fstr"].ToString() + "'"; ;
                                
                                model[0].Col30 = dtt.Rows[0]["ans_id"].ToString();

                                if (dtt.Rows[0]["correct_option"].ToString() == "1") { model[0].Col21 = "1"; }
                                if (dtt.Rows[1]["correct_option"].ToString() == "1") { model[0].Col21 = "2"; }
                                if (dtt.Rows[2]["correct_option"].ToString() == "1") { model[0].Col21 = "3"; }
                                if (dtt.Rows[3]["correct_option"].ToString() == "1") { model[0].Col21 = "4"; }
                                model[0].Col30 = dtt.Rows[0]["ans_id"].ToString();
                                model[0].Col26 = dtt.Rows[0]["options"].ToString();
                                model[0].Col22 = dtt.Rows[1]["options"].ToString();
                                model[0].Col23 = dtt.Rows[2]["options"].ToString();
                                model[0].Col24 = dtt.Rows[3]["options"].ToString();

                                model[0].Col39 = "ANS";
                                model[0].Col31 = dtt.Rows[0]["option_filepath"].ToString();
                                model[0].Col32 = dtt.Rows[1]["option_filepath"].ToString();
                                model[0].Col33 = dtt.Rows[2]["option_filepath"].ToString();
                                model[0].Col34 = dtt.Rows[3]["option_filepath"].ToString();

                                if (model[0].Col31 != "0") { model[0].Chk1 = true; } else { model[0].Chk1 = false; }

                                #region attachment
                                List<Tmodelmain> ltmf = new List<Tmodelmain>();
                                foreach (DataRow drf in dtt.Rows)
                                {
                                    Tmodelmain tmf = new Tmodelmain();
                                    tmf.Col4 = drf["option_filepath"].ToString();
                                    tmf.Col1 = drf["option_filename"].ToString();
                                   // tmf.Col2 = drf["col1"].ToString();
                                    tmf.Col3 = drf["ans_rec_id"].ToString();
                                    tmf.Col5 = drf["img_type"].ToString();
                                   // tmf.Col6 = drf["col3"].ToString();
                                    ltmf.Add(tmf);
                                }
                                model[0].LTM1 = ltmf;
                                #endregion

                                #endregion


                            }
                            break;

                        case "FDEL":


                            string fid = sgen.GetSession(MyGuid, "delid").ToString();
                            res = sgen.execute_cmd(userCode, "Delete from answers WHERE rec_id='" + fid + "' and type='Tqu' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
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
            }
            ModelState.Clear();
            return model;

        }
        #endregion

        //=========makequery============

        #region make query
        public void Make_query(string viewname, string title, string btnval, string seektype, string param1, string param2, string Myguid = "")
        {
            FillMst(Myguid);
            sgen.SetSession(MyGuid, "btnval", btnval);
            string cmd = "";
            switch (viewname.ToLower())
            {
                #region UPLOAD TRAINIG
                case "upload_training":
                    switch (btnval)
                    {
                        case "MODULE":
                          

                            cmd = "SELECT (a.client_id|| a.client_unit_id|| a.vch_num|| to_char(a.vch_date, 'yyyymmdd')|| a.type) AS fstr, a.ref_code ,a.course_id," +
                                "a.cou_title,b.cat_name,c.mod_name from courses a inner join  category b on a.cat_id = b.cat_id and b.type = 'TCT'  and a.client_unit_id = b.client_unit_id " +
                                "inner join module c on c.type = 'TMD' and a.mod_id = c.mod_id  and a.client_unit_id = c.client_unit_id  where a.type = 'TCR' and a.client_unit_id = '" + unitid_mst + "'";
                            break;
                        case "EDIT":
                        case "VIEW":

                      

                            cmd = "SELECT (a.client_id|| a.client_unit_id||a.vch_num|| to_char(a.vch_date, 'yyyymmdd')||a.type) AS fstr, a.vch_num doc_no, a.ref_code ," +
                               "a.course_id,a.cou_title,b.cat_name,c.mod_name from course_content a inner join  category b on a.cat_id =b.cat_id and b.type = 'TCT' and a.client_unit_id=b.client_unit_id" +
                               " inner join module c on c.type = 'TMD' and a.mod_id = c.mod_id  and a.client_unit_id = c.client_unit_id " +
                               "  where a.type = 'TCR' and a.client_unit_id='" + unitid_mst + "' order by a.vch_num desc";

                            break;

                    }
                    break;

                #endregion
                #region modulem, category, title
                case "module1":
                    if (sgen.GetSession(MyGuid, "TR_MID") != null)
                    {
                        switch (btnval)
                        {
                            case "EDIT":
                            case "VIEW":
                                if (sgen.GetSession(MyGuid, "TR_MID").ToString().Trim().Equals("1013.1"))
                                {
                                    cmd = "select client_id||client_unit_id||vch_num||to_char(vch_date,'yyyymmdd')||type as fstr,mod_name module_name, (CASE when m_status='Active' THEN 'Active'  when m_status = '' THEN 'Active' else 'Inactive' end) as Status " +
                                        ", client_unit_id unit_id, to_char(vch_date,'" + sgen.Getsqldateformat() + "')as Ent_Date from module where type in ('TMD','DDTMD') and client_unit_id='" + unitid_mst + "' order by vch_num desc";
                                }
                                else if (sgen.GetSession(MyGuid, "TR_MID").ToString().Trim().Equals("1013.2"))
                                {
                                    cmd = "select ct.client_id||ct.client_unit_id||ct.vch_num||to_char(ct.vch_date,'yyyymmdd')||ct.type as  fstr,m.mod_name,ct.cat_name, (CASE when ct.c_status='Active' THEN 'Active'  when ct.c_status = '' THEN 'Active' else 'Inactive' end) as Status, " +
                                        "ct.client_unit_id unit_id, ud.user_id as ent_by,  to_char(ct.vch_date,'" + sgen.Getsqldateformat() + "')as Ent_Date from category ct " +
                                        "inner join module m on m.mod_id = ct.mod_id and m.type = 'TMD' and m.client_unit_id='" + unitid_mst + "' inner join user_details ud on ud.vch_num = ct.ent_by and ud.type = 'CPR' where ct.type in ('TCT', 'DDTCT') " +
                                        "and ct.client_unit_id='" + unitid_mst + "' order by ct.vch_num desc";
                                }
                                else if (sgen.GetSession(MyGuid, "TR_MID").ToString().Trim().Equals("1013.3"))
                                {
                                    cmd = "select c.client_id||c.client_unit_id||c.vch_num||to_char(c.vch_date,'yyyymmdd')||c.type as fstr" +
                                        ",module.mod_name Module_name, c.cou_title course_title, category.cat_name, c.ref_code, c.cou_status, to_char(c.vch_date,'" + sgen.Getsqldateformat() + "')as Ent_Date, " +
                                        "ud.user_id, c.client_unit_id unit_id from courses c INNER join module on module.mod_id = c.mod_id and module.type = 'TMD' and module.client_unit_id='"+unitid_mst+"' " +
                                        "INNER join category on category.cat_id = c.cat_id and category.type = 'TCT' and category.client_unit_id='"+unitid_mst+"' INNER join user_details ud ON c.Cou_ent_by" +
                                        " = ud.vch_num and ud.type = 'CPR' where c.type in ('TCR', 'DDTCR') and c.client_unit_id = '" + unitid_mst + "' and c.cou_ent_by = '" + userid_mst + "'";

                                }
                                break;
                            case "ADD2":
                                cmd = "select ct.client_id||ct.client_unit_id||ct.vch_num||to_char(ct.vch_date,'yyyymmdd')||ct.type as " +
                                    "fstr,m.mod_name as module,ct.cat_name as category from category ct inner join module m on " +
                                    "m.mod_id=ct.mod_id and m.type='TMD' where ct.client_unit_id='" + unitid_mst + "'";
                                break;
                        }
                    }
                    break;

                #endregion
                #region Add Group
                case "add_grp":
                    switch (btnval)
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = "select gd.fstr,count(gd.user_id) as User_In_Group,gd.group_id,gd.group_name,gd.Ent_By,gd.Ent_Date from (select ug.user_id,gd.client_id || gd.client_unit_id || gd.vch_num || to_char(gd.vch_date, 'yyyymmdd') || gd.type as fstr,gd.group_id,gd.group_name,gd.description as dsp,b.First_name || ' ' || b.middle_name || ' ' || b.last_name as Ent_By,to_char(gd.ent_date, '" + sgen.Getsqldateformat() + "') as Ent_Date from group_details gd inner join user_details b on gd.ent_by = lpad(b.rec_id, 6, '0') left join usergroup ug on ug.group_id = gd.group_id and ug.type = 'TGP' AND ug.client_id = gd.client_id and ug.client_unit_id = gd.client_unit_id where gd.type = 'TGP' and gd.client_unit_id = '" + unitid_mst + "') gd group by gd.fstr,gd.group_id,gd.group_name,gd.Ent_By,gd.Ent_Date";
                            break;
                    }
                    break;

                #endregion
                #region make_quiz
                case "make_quiz":
                    switch (btnval)
                    {
                        case "CHOOSETITLE":
                            cmd = "SELECT cr.client_id||cr.client_unit_id||cr.course_id||to_char(cr.vch_date, 'yyyymmdd')||cr.type as fstr,md.mod_name as Module," +
                       "ct.cat_name as Category,cou_title as Course_Title,ref_code as Ref_Code FROM courses cr INNER join module md on cr.MOD_ID=md.MOD_ID and md.type='TMD' and md.client_unit_id='" + unitid_mst + "' INNER JOIN " +
                       "category ct on cr.CAT_ID=ct.CAT_ID and ct.type='TCT' and ct.client_unit_id='" + unitid_mst + "' where cr.client_id='" + clientid_mst + "' and cr.client_unit_id='" + unitid_mst + "' and cr.type='TCR'";
                            break;

                        case "EDIT":
                        case "VIEW":
                            cmd = "select a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date, 'yyyymmdd')||a.type as fstr, a.ques_paper_id,a.ques_paper_name," +
                                "b.MOD_NAME module,c.CAT_NAME category ,cr.COU_TITLE Title from ques_paper a inner join module b on a.mod_id=b.mod_id " +
                                " and b.type='TMD' and a.client_unit_id=b.client_unit_id  inner join category c on a.cat_id = c.cat_id and a.mod_id = c.mod_id and" +
                                " c.type = 'TCT' and a.client_unit_id = c.client_unit_id inner join courses cr on cr.course_id = a.cou_title and cr.type = 'TCR' and " +
                                "cr.client_unit_id = a.client_unit_id where a.type = 'TPQ' and a.client_unit_id = '" + unitid_mst + "' and a.ques_paper_ent_by=''" + userid_mst + "";
                            break;
                    }
                    break;

                #endregion
                #region asn_quiz
                case "asn_quiz":
                    switch (btnval)
                    {
                        case "CHOOSETITLE":
                            cmd = "SELECT qp.client_id||qp.client_unit_id||qp.QUES_PAPER_ID||to_char(qp.vch_date, 'yyyymmdd')||qp.type as " +
                                "fstr,md.mod_name as Module,ct.cat_name as Category,cr.COU_TITLE,cr.ref_code as Ref_Code,qp.ques_paper_name " +
                                "as Question_Paper FROM ques_paper qp  INNER join module md on qp.MOD_ID = md.MOD_ID and md.type = 'TMD' and md.client_unit_id=qp.client_unit_id  " +
                                "INNER JOIN category ct on qp.CAT_ID = ct.CAT_ID and ct.type = 'TCT'  and md.mod_id=ct.mod_id and ct.client_unit_id=qp.client_unit_id INNER JOIN courses cr ON cr.COURSE_ID " +
                                "= qp.COU_TITLE and cr.type = 'TCR' and cr.client_id = qp.client_id and cr.client_unit_id = " +
                                "qp.client_unit_id where qp.client_id = '" + clientid_mst + "' and qp.client_unit_id = '" + unitid_mst + "'" +
                                " and qp.type = 'TPQ'";
                            break;

                        case "UNASSIGN":
                            cmd = "select uqp.client_id||uqp.client_unit_id||uqp.vch_num||to_char(uqp.vch_date, 'yyyymmdd')||uqp.type as fstr," +
                                "count(uqp.rec_id) as Usercount,uqp.vch_num,GROUP_CONCAT(distinct uqp.group_name) as Group_Name,uqp.mod_name as Module," +
                                "uqp.cat_name as Category,cr.cou_title as Title,uqp.ref_code as Ref_code,uqp.quespaper_startdt as Start_Date," +
                                "uqp.quespaper_enddt as End_Date,uqp.status as Status,uqp.ques_mandatory as Mandatory from user_quespaper uqp " +
                                "inner join user_details ud on ud.rec_id = uqp.user_id and ud.type = 'CPR' inner join courses cr on cr.course_id = " +
                                "uqp.cou_title and cr.type = 'TCR' and cr.client_id = uqp.client_id where uqp.ent_by = '" + userid_mst + "' and " +
                                "uqp.status = 'assigned' GROUP by uqp.client_id||uqp.client_unit_id||uqp.vch_num||to_char(uqp.vch_date, 'yyyymmdd')||uqp.type" +
                                ",uqp.vch_num,uqp.mod_name,uqp.cat_name,uqp.cou_title,uqp.ref_code,uqp.quespaper_startdt,uqp.quespaper_enddt," +
                                "uqp.status ORDER BY uqp.user_quespaperid desc";
                            break;
                        case "GETTITLE":
                            cmd = "SELECT cr.client_id||cr.client_unit_id||cr.course_id||to_char(cr.vch_date, 'yyyymmdd')||cr.type as fstr,md.mod_name as " +
                                "Module, ct.cat_name as Category, cr.cou_title as Course_Title, cr.ref_code as Ref_Code, cr.trn_duration as Duration," +
                                "(CASE WHEN cc.training_level is null THEN '-' ELSE (CASE WHEN cc.training_level=01 THEN 'Beginners' ELSE " +
                                "(CASE WHEN cc.training_level=02 THEN 'Intermediate' ELSE (CASE WHEN cc.training_level=03 THEN 'Advance' ELSE " +
                                "(CASE WHEN cc.training_level=04 THEN 'Expert' END)END)END)END)END) as Level, (case WHEN cc.FILE_NAME IS NULL THEN" +
                                " 'N' ELSE 'Y' END) as Attachment FROM courses cr INNER join module md on cr.MOD_ID = md.MOD_ID and md.type = 'TMD' " +
                                "INNER JOIN category ct on cr.CAT_ID = ct.CAT_ID and ct.type = 'TCT' left join course_content cc on cr.COURSE_ID = " +
                                "cc.COURSE_ID and cc.type = 'TCR' and cc.client_id = cr.client_id and cc.client_unit_id = cr.client_unit_id where " +
                                "cr.client_id = '" + clientid_mst + "' and cr.client_unit_id = '" + unitid_mst + "' and cr.type = 'TCR' ";
                            break;
                    }
                    break;

                #endregion
                #region ins_quiz
                case "ins_quiz":
                    switch (btnval)
                    {

                        case "EDIT":
                        case "VIEW":

                            cmd = "select (a.client_id|| a.client_unit_id||a.vch_num|| to_char(a.vch_date|| 'yyyymmdd')||a.type) AS fstr," +
                                " a.vch_num  Doc_no, to_char(a.vch_date,'" + sgen.Getsqldateformat() + "') doc_date,b.mod_name module_name,c.cat_name category,a.question from questions a  " +
                                " inner join module b on a.mod_id = b.mod_id and b.type = 'TMD' and b.client_unit_id = a.client_unit_id" +
                                " inner join category c on a.cat_id = c.cat_id and b.mod_id = c.mod_id and b.type = 'TMD' and c.client_unit_id = a.client_unit_id" +
                                " where a.type = 'TQU' and a.client_unit_id = '" + unitid_mst + "' order by a.vch_num desc";
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

        //==============Module=================

        #region module
        public ActionResult Module1()
        {
            FillMst("");
            chkRef();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            if (mid_mst.Trim().Equals("1013.1")) { model[0].Col9 = "MODULE"; model[0].Col21 = "Module"; }
            if (mid_mst.Trim().Equals("1013.2")) { model[0].Col9 = "CATEGORY"; model[0].Col21 = "Category"; }
            if (mid_mst.Trim().Equals("1013.3")) { model[0].Col9 = "TITLE"; model[0].Col21 = "Title"; }
            sgen.SetSession(MyGuid, "TR_MID", mid_mst.Trim());
            model[0].Col11 = " and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";

            model[0].TList1 = mod1;
            model[0].SelectedItems1 = new string[] { "" };
            return View(model);
        }
        public List<Tmodelmain> new_Module1(List<Tmodelmain> model)
        {
            List<SelectListItem> mod1 = new List<SelectListItem>();

            model = getnew(model);
            #region bindmodule
            TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1 = cmd_Fun.tr_mod(userCode, unitid_mst);

            #endregion
            model[0].SelectedItems1 = new string[] { "" };
            model[0].Col23 = "Active";
            return model;
        }
        [HttpPost]
        public ActionResult Module1(List<Tmodelmain> model, string command)
        {
            string mod_id = "", col_name = "";
            FillMst(model[0].Col15);
            List<SelectListItem> mod1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
            TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
            if (model[0].SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
            if (model[0].Col14.Trim().Equals("1013.1"))
            {
                model[0].Col10 = "module";
                model[0].Col12 = "TMD";
                col_name = "mod_name";
            }
            if (model[0].Col14.Trim().Equals("1013.2"))
            {
                model[0].Col10 = "category";
                model[0].Col12 = "TCT";
                col_name = "cat_name";
            }
            if (model[0].Col14.Trim().Equals("1013.3"))
            {
                model[0].Col10 = "courses";
                model[0].Col12 = "TCR";
                col_name = "cou_title";
            }
            if (command == "New")
            {
                try
                {
                    model = new_Module1(model);
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
            else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
            {
                try
                {
                    var tmodel = model.FirstOrDefault();
                    //string currdate = sgen.server_datetime_local(userCode);
                    //ent_date = sgen.Savedate(currdate, true);

                    string currdate = sgen.server_datetime(userCode);
                    mod_status = tmodel.Col23.Trim();
                    if (mod_status == "Inactive") { type = "DDTCR"; }
                    DataTable dtstr = new DataTable();
                    dtstr = sgen.getdata(userCode, "select * from " + model[0].Col10 + " WHERE 1=2");

                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        mq1 = " and vch_num<>'" + tmodel.Col16.Trim() + "'";
                        isedit = true;
                        vch_num = tmodel.Col16.Trim();
                        mod_id = tmodel.Col16;
                    }
                    else
                    {
                        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where type='" + model[0].Col12 + "'" + model[0].Col11 + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                        string mid = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where type='" + model[0].Col12 + "'" + model[0].Col11 + "";
                        mod_id = sgen.genNo(userCode, mid, 6, "auto_genid");
                        isedit = false;
                    }

                    cond = sgen.seekval(userCode, "select " + col_name + " from " + model[0].Col10 + " where type='" + model[0].Col12 + "' and mod_name='" + model[0].Col22.Trim() + "'" + model[0].Col11 + "" + mq1 + "", col_name);
                    if (!cond.Equals("0"))
                    {
                        //Means Already Exits....            
                        ViewBag.scripCall = "showmsgJS(1, 'Name Already Exists', 2);";
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        return View(model);
                    }

                    #region dtstr module
                    if (model[0].Col14.Trim().Equals("1013.1"))
                    {
                        if (mod_status == "Inactive") { type = "DDTMD"; }
                        DataRow dr = dtstr.NewRow();
                        dr["vch_num"] = vch_num.Trim();
                        dr["vch_date"] = currdate;
                        dr["type"] = model[0].Col12;
                        dr["mod_id"] = mod_id.Trim();
                        dr["mod_name"] = model[0].Col22;
                        dr["m_status"] = mod_status.Trim();

                        if (isedit)
                        {
                            dr["mod_ent_by"] = model[0].Col3;
                            dr["mod_ent_date"] = sgen.Savedate(model[0].Col4, false);
                            dr["client_id"] = clientid_mst;
                            dr["client_unit_id"] = unitid_mst;
                            dr["rec_id"] = model[0].Col7;
                            dr["mod_edit_by"] = userid_mst;
                            dr["mod_edit_date"] = currdate;
                        }
                        else
                        {
                            dr["rec_id"] = "0";
                            dr["mod_ent_by"] = userid_mst;
                            dr["mod_ent_date"] = currdate;
                            dr["mod_edit_by"] = "-";
                            dr["mod_edit_date"] = currdate;
                            dr["client_id"] = clientid_mst;
                            dr["client_unit_id"] = unitid_mst;
                        }
                        dtstr.Rows.Add(dr);
                    }
                    #endregion

                    #region dtstr category
                    if (model[0].Col14.Trim().Equals("1013.2"))
                    {
                        if (mod_status == "Inactive") { model[0].Col12 = "DDTCT"; }
                        DataRow dr = dtstr.NewRow();
                        dr["vch_num"] = vch_num.Trim();
                        dr["vch_date"] = currdate;
                        dr["type"] = model[0].Col12;
                        dr["mod_id"] = tmodel.SelectedItems1.FirstOrDefault().ToString().Trim();
                        dr["cat_id"] = mod_id.Trim();
                        dr["cat_name"] = tmodel.Col22;
                        dr["c_status"] = mod_status.Trim();
                        if (isedit)
                        {
                            dr["ent_by"] = model[0].Col3;
                            dr["ent_date"] = sgen.Savedate(model[0].Col4, false);
                            dr["client_id"] = clientid_mst;
                            dr["client_unit_id"] = unitid_mst;
                            dr["rec_id"] = tmodel.Col7;
                            dr["edit_by"] = userid_mst;
                            dr["edit_date"] = currdate;
                        }
                        else
                        {
                            dr["rec_id"] = "0";
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

                    #region dtstr title
                    if (model[0].Col14.Trim().Equals("1013.3"))
                    {
                        DataRow dr = dtstr.NewRow();
                        dr["vch_num"] = vch_num.Trim();
                        dr["vch_date"] = currdate;
                        dr["type"] = model[0].Col12;
                        dr["course_id"] = mod_id.Trim();
                        dr["mod_id"] = model[0].Col25;
                        dr["cat_id"] = model[0].Col24;
                        dr["cou_title"] = model[0].Col22;
                        dr["cou_descp"] = model[0].Col19;
                        dr["cou_status"] = mod_status.Trim();
                        dr["ref_code"] = model[0].Col20;
                        if (isedit)
                        {
                            dr["cou_ent_by"] = model[0].Col3;
                            dr["cou_ent_date"] = sgen.Savedate(model[0].Col4, false);
                            dr["client_id"] = clientid_mst;
                            dr["client_unit_id"] = unitid_mst;
                            dr["rec_id"] = model[0].Col7;
                            dr["cou_edit_by"] = userid_mst;
                            dr["cou_edit_date"] = currdate;
                        }
                        else
                        {
                            dr["rec_id"] = "0";
                            dr["cou_ent_by"] = userid_mst;
                            dr["cou_ent_date"] = currdate;
                            dr["cou_edit_by"] = "-";
                            dr["cou_edit_date"] = currdate;
                            dr["client_id"] = clientid_mst;
                            dr["client_unit_id"] = unitid_mst;
                        }
                        dtstr.Rows.Add(dr);
                    }
                    #endregion

                    bool Result = sgen.Update_data_fast1(userCode, dtstr, model[0].Col10, tmodel.Col8, isedit);

                    if (Result == true)
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
                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain
                        {
                            Col21 = tmodel.Col21,
                            Col14 = tmodel.Col14,
                            Col15 = tmodel.Col15,
                            Col9 = tmodel.Col9,
                            Col10 = tmodel.Col10,
                            Col11 = tmodel.Col11,
                            Col12 = tmodel.Col12,
                            Col13 = "Save",
                            Col100 = "Save & New",
                            Col121 = "S",
                            Col122 = "<u>S</u>ave",
                            Col123 = "Save & Ne<u>w</u>",
                            TList1 = mod1,
                            SelectedItems1 = new string[] { "" }
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
                                model = new_Module1(model);
                                ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                            }
                            catch (Exception ex) { }
                        }
                    }
                    else { ViewBag.scripCall = "showmsgJS(3, 'Record Not Saved', 1);"; }
                }
                catch (Exception ex) { sgen.showmsg(1, ex.Message.ToString(), 0); }
            }

            ModelState.Clear();
            return View(model);
        }
        #region checkrefcode
        [HttpPost]
        public ContentResult checkrefcode(string value)
        {
            try
            {
                if (!value.Trim().Equals(""))
                {
                    mq = sgen.seekval(userCode, "select refcode from courses where cou_title='" + value.Trim() + "' and type='" + type
                             + "'" + where + "", "refcode");

                    if (!mq.Trim().Equals("0")) { mq = "Y"; return Content(mq); }
                    else { mq = "N"; return Content(mq); }
                }
                else { mq = "N"; return Content(mq); }
            }
            catch (Exception ex) { mq = "E"; return Content(mq); }
        }

        #endregion
        #region checktitle
        [HttpPost]
        public ContentResult checkcoutitle(string value, string txtName)
        {
            try
            {
                if (!value.Trim().Equals(""))
                {
                    if (txtName.Trim().Equals("Module"))
                    {
                        mq = sgen.seekval(userCode, "select mod_name from module where mod_name='" + value.Trim() + "' and type='" + type + "'", "mod_name");
                    }
                    if (txtName.Trim().Equals("Category"))
                    {
                        mq = sgen.seekval(userCode, "select cat_name  from category where cat_name='" + value.Trim() + "' and type='" + type
                        + "'" + where + "", "cat_name");
                    }
                    if (txtName.Trim().Equals("Title"))
                    {
                        mq = sgen.seekval(userCode, "select cou_title  from courses where cou_title='" + value.Trim() + "' and type='" + type
                        + "'" + where + "", "cou_title");
                    }


                    if (!mq.Trim().Equals("0")) { mq = "Y"; return Content(mq); }
                    else { mq = "N"; return Content(mq); }
                }
                else { mq = "N"; return Content(mq); }
            }
            catch (Exception ex) { mq = "E"; return Content(mq); }
        }
        #endregion

        #endregion
        //==========Group Master==============
        #region add group
        public ActionResult add_grp()
        {
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            FillMst(MyGuid);
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "CREATE GROUP";
            model[0].Col10 = "group_details";
            model[0].Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            switch (model[0].Col14)
            {
                #region training group
                case "1009.1":
                    model[0].Col12 = "TGP";// training group
                    break;
                #endregion
                #region Conference Room
                case "4007.2":
                    model[0].Col12 = "CGP";//Conference group
                    break;
                    #endregion
            }
            List<SelectListItem> md1 = new List<SelectListItem>();
            TempData[MyGuid + "_TList1"] = model[0].TList1 = md1;
            model[0].SelectedItems1 = new string[] { "" };
            return View(model);
        }
        public List<Tmodelmain> new_add_grp(List<Tmodelmain> model)
        {
            var tm = model.FirstOrDefault();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            model = getnew(model);
            #region ddls
            TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1 = cmd_Fun.desig(userCode, unitid_mst);
            model[0].SelectedItems1 = new string[] { "" };
            #endregion
            return model;
        }
        [HttpPost]
        public ActionResult add_grp(List<Tmodelmain> model, string command, string mid)
        {
            try
            {
                string groupid = "";
                FillMst(model[0].Col15);
                var tm = model.FirstOrDefault();
                List<SelectListItem> mod1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
                TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
                if (model[0].SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };

                if (command == "New")
                {
                    model = new_add_grp(model);
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
                    string currdate = "";
                    DataTable dtstr = new DataTable();
                    dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    currdate = sgen.server_datetime(userCode);
                    if (model[0].Col17 == "")
                    {
                        ViewBag.scripCall += "showmsgJS(1,'Please Fill Group Name', 2);";
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
                        groupid = model[0].Col19;
                    }
                    else
                    {
                        isedit = false;
                        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " a where type='" + model[0].Col12 + "'" + model[0].Col11 + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                        groupid = "select max(to_number(group_id)) as auto_genid from " + model[0].Col10 + " where type='" + model[0].Col12 + "'" + model[0].Col11 + "";
                        groupid = sgen.genNo(userCode, groupid, 6, "auto_genid");
                        model[0].Col16 = vch_num;
                        model[0].Col19 = groupid;
                        string cond = sgen.seekval(userCode, "select group_name from " + model[0].Col10 + " where type='" + model[0].Col12 + "' " + model[0].Col11 + "" +
                                    "group_name='" + model[0].Col17 + "' " + mq1 + "", "group_name");
                        if (cond != "0")
                        {
                            ViewBag.scripCall += "showmsgJS(1,'Data Already Exists', 2);";
                            ViewBag.vnew = "disabled='disabled'";
                            ViewBag.vedit = "disabled='disabled'";
                            ViewBag.vsave = "";
                            ViewBag.vsavenew = "";
                            return View(model);
                        }

                    }
                    DataRow dr = dtstr.NewRow();
                    dr["rec_id"] = "0";
                    dr["vch_num"] = vch_num;
                    dr["vch_date"] = currdate;
                    dr["type"] = model[0].Col12;
                    dr["group_id"] = groupid;
                    dr["group_name"] = model[0].Col17;
                    dr["Description"] = model[0].Col18;
                    dr = getsave(currdate, dr, model, isedit);
                    dtstr.Rows.Add(dr);

                    bool Result = sgen.Update_data(userCode, dtstr, model[0].Col10.Trim(), tmodel.Col8, isedit);
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
                            //  TList2 = mod2,
                            SelectedItems1 = new string[] { "" },
                            //  SelectedItems2 = new string[] { "" },
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
                                model = new_add_grp(model);
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
        #region View group
        public ActionResult vw_grp()
        {
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            FillMst(MyGuid);
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            m_id_mst = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            List<Tmodelmain> model = new List<Tmodelmain>();
            Tmodelmain tm1 = new Tmodelmain();
            tm1.Col9 = "VIEW GROUP";
            tm1.Col10 = "group_details";
            tm1.Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            tm1.Col12 = "TGP";
            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = m_id_mst.Trim();
            model.Add(tm1);
            model = bind_groups(model);
            ViewBag.scripCall = "enableForm();";
            return View(model);
        }
        public List<Tmodelmain> bind_groups(List<Tmodelmain> model)
        {
            var tm = model.FirstOrDefault();
            DataTable dt = new DataTable();
            dt = sgen.getdata(userCode, "SELECT  * from " + model[0].Col10 + " where type='" + model[0].Col12 + "'" + model[0].Col11 + "");
            if (dt.Rows.Count > 0)
            {
                model = new List<Tmodelmain>();
                ViewBag.vsave = "";
                ViewBag.vsavenew = "";
                foreach (DataRow dr in dt.Rows)
                {
                    string usercount = "";
                    DataTable dt1 = sgen.getdata(userCode, "select count(user_id) as usercount from usergroup where group_id='" + dr["group_id"].ToString() + "' and status='assigned' and type='" + tm.Col12 + "'" + tm.Col11 + "");
                    if (dt1.Rows.Count > 0)
                    {
                        usercount = dt1.Rows[0]["usercount"].ToString();
                    }
                    model.Add(new Tmodelmain
                    {
                        Col9 = tm.Col9,
                        Col11 = tm.Col11,
                        Col12 = tm.Col12,
                        Col10 = tm.Col10,
                        Col13 = tm.Col13,
                        Col14 = tm.Col14,
                        Col15 = tm.Col15,
                        Chk1 = tm.Chk1,
                        Chk2 = tm.Chk2,
                        Col16 = dr["group_id"].ToString(),
                        Col17 = dr["group_name"].ToString(),
                        Col18 = usercount,
                        Col19 = dr["DESCRIPTION"].ToString(),
                    });
                }
            }
            return model;
        }
        [HttpPost]
        public ActionResult vw_grp(List<Tmodelmain> model, string command, string hfrow)
        {
            try
            {
                string groupid = "";
                FillMst(model[0].Col15);
                var tm = model.FirstOrDefault();
                List<SelectListItem> mod1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
                TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
                if (model[0].SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
                else if (command == "Cancel")
                {
                    return CancelFun(model);
                }
                else if (command == "Callback")
                {
                    model = StartCallback(model);
                }
                else if (command == "DELETE")
                {
                    DataTable dt = new DataTable();
                    bool data = sgen.execute_cmd(userCode, "DELETE FROM " + model[0].Col10 + " WHERE group_id='" + model[Convert.ToInt32(hfrow)].Col16 + "' and type='" + model[0].Col12 + "'" + model[0].Col11 + "");
                    if (data == true)
                    {
                        ViewBag.scripcall = "sgen.showmsg(1, 'Deleted', 1);";
                        model = bind_groups(model);
                    }
                    else
                    {
                        ViewBag.scripcall = "sgen.showmsg(1, 'Try Again',2);";
                        return View(model);
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
        #region edit group
        public ActionResult ed_grp()
        {
            MyGuid = EncryptDecrypt.Decrypt(HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["m_id"]);
            FillMst(MyGuid);
            List<Tmodelmain> model = new List<Tmodelmain>();
            Tmodelmain tm1 = new Tmodelmain();
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            m_id_mst = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            tm1.Col9 = "EDIT GROUP";
            tm1.Col10 = "group_details";
            tm1.Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            tm1.Col12 = "TGP";// training group
            ViewBag.scripCall = "enableForm();";

            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = m_id_mst.Trim();
            if (Request.QueryString["fstr"] != null)
            {
                tm1.Col16 = EncryptDecrypt.Decrypt(Request.QueryString["fstr"].ToString());
                DataTable dt = sgen.getdata(userCode, "select * from group_details Where group_id ='" + tm1.Col16 + "' and type='" + tm1.Col12 + "'" + tm1.Col11 + "");
                if (dt.Rows.Count > 0)
                {
                    tm1.Col17 = Convert.ToString(dt.Rows[0]["group_name"]);
                    tm1.Col18 = Convert.ToString(dt.Rows[0]["DESCRIPTION"]);
                }
            }
            model.Add(tm1);
            return View(model);
        }
        [HttpPost]
        public ActionResult ed_grp(List<Tmodelmain> model, string command, string mid)
        {
            try
            {
                string groupid = "";
                FillMst(model[0].Col15);
                var tm = model.FirstOrDefault();
                List<SelectListItem> mod1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
                TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
                if (model[0].SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };

                if (command == "New")
                {
                    model = new_add_grp(model);
                }
                else if (command == "Cancel")
                {
                    return RedirectToAction("vw_grp", new { @m_id = EncryptDecrypt.Encrypt(model[0].Col15), @mid = EncryptDecrypt.Encrypt(model[0].Col14) });
                }
                else if (command == "Callback")
                {
                    model = StartCallback(model);
                }
                else if (command == "Update Group")
                {
                    string currdate = sgen.server_datetime(userCode);


                    bool res = sgen.execute_cmd(userCode, "UPDATE group_details SET group_name='" + model[0].Col17 + "',description='" + model[0].Col18 + "', edit_date='" + currdate + "' " +
                        "WHERE group_id='" + model[0].Col16 + "' and type='" + model[0].Col12 + "'" + model[0].Col11 + "");
                    if (res)
                    {
                        sgen.execute_cmd(userCode, "commit");
                        ViewBag.scripCall += "showmsgJS(1,'Updated Successfully', 1);";
                        return RedirectToAction("vw_grp", new { @m_id = EncryptDecrypt.Encrypt(model[0].Col15), @mid = EncryptDecrypt.Encrypt(model[0].Col14) });

                    }
                    else
                    {
                        ViewBag.scripCall += "showmsgJS(1,'Data Not Saved', 2);";
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

        #region asign group
        public ActionResult asn_grp()
        {
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            FillMst(MyGuid);
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            m_id_mst = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            List<Tmodelmain> model = new List<Tmodelmain>();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            Tmodelmain tm1 = new Tmodelmain();
            tm1.Col9 = "ASSIGN OR UNASSIGN INDIVIDUAL TO OR FROM GROUP";
            tm1.Col10 = "usergroup";
            tm1.Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            tm1.Col12 = "TGP";
            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = m_id_mst.Trim();
            role_mst = sgen.GetCookie(MyGuid, "role_mst");
            subdomain_mst = sgen.GetCookie(MyGuid, "subdomain_mst");
            clientname_mst = sgen.getstring(userCode, "select company_name from company_profile where company_profile_id='" + clientid_mst + "'");
            unitname_mst = sgen.getstring(userCode, "select unit_name from company_unit_profile where cup_id='" + unitid_mst + "'");
            sgen.SetCookie(MyGuid, "clientname_mst", clientname_mst);
            sgen.SetCookie(MyGuid, "unitname_mst", unitname_mst);

            DataTable datatable = new DataTable();
            datatable = sgen.getdata(userCode, "select com_email,com_password,com_port,com_smtp from company_profile where Company_Profile_Id='" + clientid_mst + "' and type='CP' ");
            if (datatable.Rows.Count > 0)
            {
                smtpvalue = Convert.ToString(datatable.Rows[0]["com_smtp"]);
                emailIdvalue = Convert.ToString(datatable.Rows[0]["com_email"]);
                passwordvalue = EncryptDecrypt.Decrypt(datatable.Rows[0]["com_password"].ToString());
                portvalue = Convert.ToInt32(datatable.Rows[0]["com_port"].ToString());
                sgen.SetSession(MyGuid, "smtpvalue", smtpvalue);
                sgen.SetSession(MyGuid, "emailIdvalue", emailIdvalue);
                sgen.SetSession(MyGuid, "passwordvalue", passwordvalue);
                sgen.SetSession(MyGuid, "portvalue", portvalue);
            }

            DataTable dt1 = sgen.getdata(userCode, "select distinct gd.group_id,gd.group_name from group_details gd where gd.type = 'TGP' and gd.client_unit_id = '" + unitid_mst + "'");
            foreach (DataRow dr in dt1.Rows)
            {
                mod1.Add(new SelectListItem { Text = dr["group_name"].ToString(), Value = dr["group_id"].ToString() });
            }
            TempData[MyGuid + "_TList1"] = tm1.TList1 = mod1;
            tm1.SelectedItems1 = new string[] { "" };
            model.Add(tm1);
            model = bind_group__to_assign(model);
            ViewBag.scripCall = "enableForm();";
            return View(model);
        }
        public List<Tmodelmain> bind_group__to_assign(List<Tmodelmain> model)
        {
            var tm = model.FirstOrDefault();
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            mq = "select ud.user_id,msdp.master_name as department,ud.first_name,ud.last_name,ud.email,ud.client_id,mdg.master_name as designation,ud.rec_id " +
                "fROM user_details ud INNER join master_setting msdp on ud.department = msdp.master_id and msdp.type = 'MDP' " +
                "INNER join master_setting mdg on ud.designation = mdg.master_Id and mdg.type = 'MDG' " +
                "where ud.client_id IN ('" + clientid_mst.Trim(new char[] { (char)39 }) + "')  and ud.status = 'active' and ud.type = 'CPR' and find_in_set('1000', ud.mod_id)=1";
            dt = sgen.getdata(userCode, mq);
            if (dt.Rows.Count > 0)
            {
                model = new List<Tmodelmain>();
                ViewBag.vsave = "";
                ViewBag.vsavenew = "";
                foreach (DataRow dr in dt.Rows)
                {
                    string rec_id = "", urec_id = "";
                    rec_id = Convert.ToString(dr["rec_id"].ToString());
                    urec_id = sgen.padlc(Convert.ToInt16(rec_id), 6);
                    model.Add(new Tmodelmain
                    {
                        Col9 = tm.Col9,
                        Col11 = tm.Col11,
                        Col12 = tm.Col12,
                        Col10 = tm.Col10,
                        Col13 = tm.Col13,
                        Col14 = tm.Col14,
                        Col15 = tm.Col15,
                        Chk1 = tm.Chk1,
                        Col16 = dr["user_id"].ToString(),
                        Col17 = dr["department"].ToString(),
                        Col18 = dr["first_name"].ToString(),
                        Col19 = dr["last_name"].ToString(),
                        Col20 = dr["designation"].ToString(),
                        Col21 = dr["email"].ToString(),
                        Col23 = urec_id,
                        Col22 = "-",
                        TList1 = tm.TList1,
                        SelectedItems1 = tm.SelectedItems1,
                    });
                }
            }
            return model;
        }
        [HttpPost]
        public ActionResult asn_grp(List<Tmodelmain> model, string command, string hfrow)
        {
            try
            {
                string groupid = "";
                string dt_ft = "YYYY-MM-dd HH24:MI:SS";
                FillMst(model[0].Col15);
                var tm = model.FirstOrDefault();
                List<SelectListItem> mod1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
                TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
                if (model[0].SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };

                smtpvalue = sgen.GetSession(MyGuid, "smtpvalue").ToString();
                emailIdvalue = sgen.GetSession(MyGuid, "emailIdvalue").ToString();
                passwordvalue = sgen.GetSession(MyGuid, "passwordvalue").ToString();
                portvalue = (int)sgen.GetSession(MyGuid, "portvalue");
                if (command == "Cancel")
                {
                    return CancelFun(model);
                }
                else if (command == "Callback")
                {
                    model = StartCallback(model);
                }
                else if (command == "Assign/Unassign")
                {
                    string currdate = sgen.server_datetime(userCode);
                    string grpid = model[0].SelectedItems1.FirstOrDefault();
                    string grpnm = ((List<SelectListItem>)TempData[MyGuid + "_TList1"]).SingleOrDefault(item => item.Value == model[0].SelectedItems1.FirstOrDefault().ToString()).Text.ToString();

                    for (int i = 0; i < model.Count; i++)// REPEATER Rows
                    {
                        string usrid = model[i].Col16;
                        string email = model[i].Col21;

                        string name = model[i].Col18;
                        string rec_id = model[i].Col23;
                        string urec_id = sgen.padlc(Convert.ToInt32(rec_id), 6);

                        if (model[i].Chk1 == true)
                        {
                            DataTable dtuser = sgen.getdata(userCode, "select user_id from " + model[0].Col10 + " where group_id ='" + grpid + "' and user_id='" + urec_id + "' " +
                                "and status='assigned' and type='" + model[0].Col12 + "'" + model[0].Col11 + "");
                            if (dtuser.Rows.Count <= 0)
                            {
                                if (smtpvalue == "" && passwordvalue == "" && portvalue == 0)
                                {
                                    ViewBag.scripCall += "showmsgJS(1, 'Please configure your mail first', 2);";
                                    return View(model);
                                }
                                else
                                {
                                    if (model[0].SelectedItems1.FirstOrDefault() == "")
                                    {
                                        ViewBag.scripCall += "showmsgJS(1, 'Please select the group first', 2);";
                                        return View(model);
                                    }
                                    else
                                    {
                                        mq = "select max(to_number(usergroup_id)) as auto_genid from " + model[0].Col10 + " where type='" + model[0].Col12 + "'" + model[0].Col11 + "";
                                        string grupid = sgen.genNo(userCode, mq, 6, "auto_genid");
                                        vch_num = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where type='" + model[0].Col12 + "'" + model[0].Col11 + "";
                                        vch_num = sgen.genNo(userCode, vch_num, 6, "auto_genid");
                                        bool data1 = sgen.execute_cmd(userCode, "insert into USERGROUP(usergroup_id,group_id,user_id,status,ug_ent_by,ug_ent_date,ent_by,ent_date,EDIT_BY,EDIT_DATE,client_id,ug_edit_by,ug_ent_date,CREATED_DATE,VCH_DATE," +
                                            "client_unit_id,vch_num,type)values('" + grupid + "','" + grpid + "','" + urec_id + "','assigned','" + userid_mst + "',to_date('" + currdate + "','" + dt_ft + "'),'" + userid_mst + "',to_date('" + currdate + "','" + dt_ft + "'),'" + userid_mst + "',to_date('" + currdate + "','" + dt_ft + "'),'" + clientid_mst + "',to_date('" + currdate + "','" + dt_ft + "'),'" + userid_mst + "',to_date('" + currdate + "','" + dt_ft + "'),'" + unitid_mst + "','" + vch_num + "','" + model[0].Col12 + "')");

                                        if (data1 == true)
                                        {
                                            MailMessage mailMsg = new MailMessage();
                                            mailMsg.Subject = "Group Update";
                                            string msgbody = "<b style='color: #3caee9; font-size: 20px'>Group Assign</b><br /><hr style='border:1px solid black' /><p>Hi " +
                                        "<b>" + name + "</b>,</p><p>Congratulations, This is to notify you that you have been added in Group. Your New Group Details " +
                                        "are as below :-</p><table style='font-weight:600'><tr><td>Company Name </td><td>: " + clientname_mst.TrimEnd(',') + " </td></tr><tr><td> " +
                                        "Unit Name </td><td>: " + unitname_mst.TrimEnd(',') + " </td></tr><tr><td> " +
                                        "Group Name </td><td>: " + grpnm + " </td> </tr><tr><td> URL </td><td>: " + subdomain_mst + " </td></tr></table><br /><p>Please " +
                                        "login and check if further any query contact administrator.</p><p>Thank you,<br />Administrator<br /></p><br /><p>Note:- " +
                                        "Please do not reply to this mail as it is an unmonitered alias.</p>";
                                            mailMsg.Body = msgbody;

                                            mailMsg.To.Add(new MailAddress(email));
                                            mailMsg.CC.Add(emailIdvalue);
                                            mailMsg.From = new MailAddress(emailIdvalue);
                                            mailMsg.IsBodyHtml = true;
                                            mailMsg.Priority = MailPriority.High;
                                            SendEmailInBackgroundThread(mailMsg);
                                        }
                                    }
                                }
                            }
                        }
                        else  //if checkbox is unchecked
                        {
                            DataTable dt2 = new DataTable();
                            dt2 = sgen.getdata(userCode, "select user_id from " + model[0].Col10 + " where group_id ='" + grpid + "' and user_id='" + urec_id + "' and " +
                                "status='assigned' and type='" + model[0].Col12 + "'" + model[0].Col11 + "");
                            if (dt2.Rows.Count > 0)
                            {  // unassign
                                if (smtpvalue == "" && passwordvalue == "" && portvalue == 0)
                                {
                                    ViewBag.scripCall += "showmsgJS(1, 'Please configure your mail first', 2);";
                                    return View(model);
                                }
                                else
                                {
                                    string gname = ((List<SelectListItem>)TempData[MyGuid + "_TList1"]).SingleOrDefault(item => item.Value == model[0].SelectedItems1.FirstOrDefault().ToString()).Text.ToString();
                                    if (model[0].SelectedItems1.FirstOrDefault() == "")
                                    {
                                        ViewBag.scripCall += "showmsgJS(1, 'Please select the group first', 2);";
                                        return View(model);
                                    }
                                    else
                                    {

                                        bool data1 = sgen.execute_cmd(userCode, "Delete from " + model[0].Col10 + " WHERE USER_ID='" + urec_id + "' and group_id='" + grpid + "'" +
                                            " and type='" + model[0].Col12 + "'" + model[0].Col11 + "");
                                        if (data1 == true)
                                        {
                                            MailMessage mailMsg = new MailMessage();
                                            mailMsg.Subject = "Group Update";

                                            string msgbody = "<b style='color: #3caee9; font-size: 20px'>Group Unassign</b><br /><hr style='border:1px solid black' /><p>Hi " +
                                        "<b>" + name + "</b>,</p><p>Congratulations, This is to notify you that you have been Removed from Group. Your Unassign Group Details " +
                                        "are as below :-</p><table style='font-weight:600'><tr><td>Company Name </td><td>: " + clientname_mst.TrimEnd(',') + " </td></tr><tr><td> " +
                                        "Unit Name </td><td>: " + unitname_mst.TrimEnd(',') + " </td></tr><tr><td> " +
                                        "Group Name </td><td>: " + gname + " </td> </tr><tr><td> URL </td><td>: " + subdomain_mst + " </td></tr></table><br /><p>Please " +
                                        "login and check if further any query contact administrator.</p><p>Thank you,<br />Administrator<br /></p><br /><p>Note:- " +
                                        "Please do not reply to this mail as it is an unmonitered alias.</p>";
                                            mailMsg.Body = msgbody;
                                            mailMsg.To.Add(new MailAddress(email));
                                            mailMsg.CC.Add(emailIdvalue);
                                            mailMsg.From = new MailAddress(emailIdvalue);
                                            mailMsg.IsBodyHtml = true;
                                            mailMsg.Priority = MailPriority.High;
                                            SendEmailInBackgroundThread(mailMsg);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Group Assigned To User');disableForm();";

                    return RedirectToAction(actionName, new { @m_id = EncryptDecrypt.Encrypt(model[0].Col15), @mid = EncryptDecrypt.Encrypt(model[0].Col14) });
                }
                else if (command == "Group")
                {
                    groupid = model[0].SelectedItems1.FirstOrDefault(); ;
                    DataTable dt1 = new DataTable();
                    mq = "select ud.user_id,msdp.master_name as department,ud.first_name,ud.last_name,ud.email,ud.client_id,mdg.master_name as designation,ud.rec_id " +
                "fROM user_details ud INNER join master_setting msdp on ud.department = msdp.master_id and msdp.type = 'MDP' INNER join master_setting mdg on ud.designation = mdg.master_Id and mdg.type = 'MDG' " +
                "where ud.client_id IN ('" + clientid_mst.Trim(new char[] { (char)39 }) + "') and ud.status = 'active' and ud.type = 'CPR'and find_in_set('1000', ud.mod_id)=1";
                    dt1 = sgen.getdata(userCode, mq);
                    if (dt1.Rows.Count > 0)
                    {
                        model = new List<Tmodelmain>();
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            string urec_id = sgen.padlc(Convert.ToInt16(dt1.Rows[i]["rec_id"].ToString()), 6);

                            DataTable dt = new DataTable();
                            mq1 = @"select user_details.rec_id,user_details.user_id,user_details.role,mdp.master_name as department,user_details.first_name,user_details.last_name,user_details.email,usergroup.status,group_details.GROUP_NAME,mdg.master_name as designation from user_details
                            INNER join usergroup on usergroup.user_id = lpad(user_details.rec_id, 6, 0) and usergroup.type = 'TGP' and usergroup.client_id = '" + clientid_mst + "' AND usergroup.GROUP_ID = '" + groupid + "' and usergroup.status = 'assigned' and usergroup.user_id = '" + urec_id + "' INNER JOIN group_details ON group_details.GROUP_ID = usergroup.GROUP_ID and group_details.type = 'TGP' and group_details.client_id = '" + clientid_mst + "' INNER JOIN master_setting mdp on user_details.DEPARTMENT = mdp.master_id and mdp.type = 'MDP' INNER JOIN master_setting mdg on user_details.designation = mdg.master_id and mdg.type = 'MDG' WHERE user_details.type = 'CPR' and user_details.STATUS = 'active' and find_in_set('1000',user_details.mod_id)=1 and find_in_set('" + clientid_mst + "',user_details.client_id)=1";

                            dt = sgen.getdata(userCode, mq1);
                            if (dt.Rows.Count > 0)
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
                                    Chk1 = true,
                                    Col16 = dt.Rows[i]["user_id"].ToString(),
                                    Col17 = dt.Rows[i]["department"].ToString(),
                                    Col18 = dt.Rows[i]["first_name"].ToString(),
                                    Col19 = dt.Rows[i]["last_name"].ToString(),
                                    Col20 = dt.Rows[i]["designation"].ToString(),
                                    Col21 = dt.Rows[i]["email"].ToString(),
                                    Col23 = dt.Rows[i]["rec_id"].ToString(),
                                    Col22 = "assigned",
                                    TList1 = tm.TList1,
                                    SelectedItems1 = tm.SelectedItems1,
                                });
                            }
                            else
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
                                    Chk1 = false,
                                    Col16 = dt1.Rows[i]["user_id"].ToString(),
                                    Col17 = dt1.Rows[i]["department"].ToString(),
                                    Col18 = dt1.Rows[i]["first_name"].ToString(),
                                    Col19 = dt1.Rows[i]["last_name"].ToString(),
                                    Col20 = dt1.Rows[i]["designation"].ToString(),
                                    Col21 = dt1.Rows[i]["email"].ToString(),
                                    Col23 = dt1.Rows[i]["rec_id"].ToString(),
                                    Col22 = "Unassigned",
                                    TList1 = tm.TList1,
                                    SelectedItems1 = tm.SelectedItems1,
                                });
                            }
                        }
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
        public void SendEmailInBackgroundThread(MailMessage mailMessage)
        {
            Thread bgThread = new Thread(new ParameterizedThreadStart(SendEmail));
            bgThread.IsBackground = true;
            bgThread.Start(mailMessage);
        }
        public void SendEmail(Object mailMsg)
        {
            MailMessage mailMessage = (MailMessage)mailMsg;
            /* Setting should be kept somewhere so no need to 
               pass as a parameter (might be in web.config)       */
            try
            {
                SmtpClient smtpClient = new SmtpClient(smtpvalue);
                NetworkCredential networkCredential = new NetworkCredential();
                networkCredential.UserName = emailIdvalue;
                networkCredential.Password = passwordvalue;
                smtpClient.Credentials = networkCredential;
                //if (!String.IsNullOrEmpty(port_no))
                smtpClient.Port = Convert.ToInt32(portvalue);

                //If you are using gmail account then
                smtpClient.EnableSsl = true;

                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                smtpClient.Send(mailMessage);
            }
            catch (SmtpException ex)
            {
                //Response.Redirect("~/erp/login_main.aspx");
            }

            finally
            {
                mailMessage.Dispose();
            }
        }
        //==========upload training==============
        #region upload traing

        public List<SelectListItem> GetAllTeaTypes()
        {
            List<SelectListItem> mod1 = new List<SelectListItem>();
            mod1.Add(new SelectListItem { Text = "Beginners", Value = "01" });
            mod1.Add(new SelectListItem { Text = "Intermediate", Value = "02" });
            mod1.Add(new SelectListItem { Text = "Advance", Value = "03" });
            mod1.Add(new SelectListItem { Text = "Expert", Value = "04" });

            return mod1;
        }

        public ActionResult upload_training()
        {
            FillMst();
            //chkRef();
            //ViewBag.vnew = "";
            //ViewBag.vedit = "";
            //ViewBag.vsave = "disabled='disabled'";
            //ViewBag.vsavenew = "disabled='disabled'";

            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "UPLOAD TRAINING";
            model[0].Col10 = "course_content";
            model[0].Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            model[0].Col12 = "TCR";
            //List<Tmodelmain> cards = new List<Tmodelmain>
            //{
            //    new Tmodelmain{ Col1="",Col2="",TList1=GetAllTeaTypes() }

            //};

            //return View(cards);
            List<SelectListItem> mod1 = new List<SelectListItem>();
            TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
            model[0].SelectedItems1 = new string[] { "" };

            return View(model);

        }

        [HttpPost]
        public ActionResult upload_training(List<Tmodelmain> model, string command, HttpPostedFileBase upd1, string mytxt, string mytxt1)
        {
            bool result = false;
            FillMst();
            var tm = model.FirstOrDefault();
            List<SelectListItem> mod1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
            TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
            if (model[0].SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
            if (command == "New")
            {
                model = getnew(model);
                //mod1.Add(new SelectListItem { Text = "Beginners", Value = "01" });
                //mod1.Add(new SelectListItem { Text = "Intermediate", Value = "02" });
                //mod1.Add(new SelectListItem { Text = "Advance", Value = "03" });
                //mod1.Add(new SelectListItem { Text = "Expert", Value = "04" });
                TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1 = cmd_Fun.traininglevel(userCode, unitid_mst);
            }
            else if (command == "Cancel")
            {
                return CancelFun(model);
            }
            else if (command == "Callback")
            {
                if (sgen.GetSession(MyGuid, "delid") != null) { btnval = "FDEL"; sgen.SetSession(MyGuid, "btnval", btnval); }
                else if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
                model = StartCallback(model);
            }

            else if (command.Trim() == "Save" || command.Trim() == "Update")
            {

                var tmodel = model.FirstOrDefault();
                string currdate = sgen.server_datetime(userCode);
                DataTable dtstr = new DataTable();
                dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10);

                {
                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        mq1 = " and vch_num<>'" + model[0].Col16.Trim() + "'";
                        isedit = true;


                    }
                    else
                    {
                        if (upd1 == null)
                        {
                            ViewBag.scripCall += "mytoast('warning', 'toast-top-right', 'Please Select Upload Document');";
                            return View(model);
                        }
                        isedit = false;
                        mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where type ='" + model[0].Col12 + "'" + model[0].Col11 + "";
                        vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                    }
                }
                #region save

               



                DataTable dtagnt = new DataTable();
                dtagnt = cmd_Fun.GetStructure(userCode, model[0].Col10);
                DataRow dr = dtagnt.NewRow();
                dr["rec_id"] = "0";
                dr["type"] = model[0].Col12;
                dr["vch_num"] = vch_num.Trim();
                dr["vch_date"] = currdate;
                dr["cou_content_id"] = vch_num.Trim();
                dr["course_id"] = tmodel.Col21;
                dr["mod_id"] = tmodel.Col17;
                dr["cat_id"] = tmodel.Col19;
                dr["cou_title"] = tmodel.Col22;
                dr["ref_code"] = tmodel.Col23;
                dr["file_name"] = fileName2;
                dr["file_path"] = encpath2;
                dr["trn_duration"] = tmodel.Col24;
                dr["client_id"] = clientid_mst;
                dr["client_unit_id"] = unitid_mst;
                dr["training_level"] = tmodel.SelectedItems1.FirstOrDefault().ToString();
                if (isedit)
                {

                    dr["cou_content_ent_by"] = model[0].Col3;
                    dr["COU_CONTENT_ENT_DT"] = model[0].Col4;
                    dr["cou_content_edit_by"] = userid_mst;
                    dr["COU_CONTENT_EDIT_DT"] = currdate;
                }
                else
                {

                    dr["cou_content_ent_by"] = userid_mst;
                    dr["COU_CONTENT_ENT_DT"] = currdate;
                    dr["cou_content_edit_by"] = '-';
                    dr["COU_CONTENT_EDIT_DT"] = currdate;

                }
                dtagnt.Rows.Add(dr);
                result = sgen.Update_data_fast1(userCode, dtagnt, model[0].Col10, tmodel.Col8, isedit);
                #endregion

                #region attachment
                DataTable dtfile = cmd_Fun.GetStructure(userCode, "file_tab");
                string serverpath = Server.MapPath("~/Uploads/" + cg_com_name.Replace(" ", "") + "/");

                try
                {
                    HttpPostedFileBase file1 = upd1;
                    ctype1 = file1.ContentType;
                    fileName1 = file1.FileName;
                    path1 = clientid_mst + unitid_mst + userid_mst + currdate + "_" + fileName1;
                    encpath1 = sgen.Convert_Stringto64(path1).ToString();
                    finalpath1 = serverpath + encpath1;
                    file1.SaveAs(finalpath1);
                    filesave(model, currdate, dtfile, fileName1, encpath1, "UPD TRAIN", ctype1);
                    res = sgen.Update_data_fast1(userCode, dtfile, "file_tab", tmodel.Col28, isedit);
                }
                catch (Exception ex) { }



                #endregion
                if (result == true)

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
                        TList1 = mod1,
                        SelectedItems1 = new string[] { "" },

                    });

                    ViewBag.vnew = "";
                    ViewBag.vedit = "";
                    ViewBag.vsave = "disabled='disabled'";
                    ViewBag.vsavenew = "disabled='disabled'";
                    ViewBag.scripCall = "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                }




            }
            //else if (command == "Cancel")
            //{
            //    ViewBag.vnew = "";
            //    ViewBag.vedit = "";
            //    ViewBag.vsave = "disabled='disabled'";
            //    ViewBag.vsavenew = "disabled='disabled'";


            //}
            //else if (command == "Callback")
            //{

            //    if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
            //    model = CallbackFun(btnval, "N", "upload_training", "HOME", model);
            //    ViewBag.vnew = "disabled='disabled'";
            //    ViewBag.vedit = "disabled='disabled'";
            //    ViewBag.vsave = "";
            //    ViewBag.vsavenew = "";
            //}

            //var tmodel1 = model.FirstOrDefault();
            //tmodel1.TList1 = GetAllTeaTypes();
            //if (tmodel1.SelectedItems1 != null && tmodel1.SelectedItems1.Length > 0 && tmodel1.TList1.Count() > 0)
            //{

            //    foreach (SelectListItem item in tmodel1.TList1)
            //    {

            //        if (tmodel1.SelectedItems1.Contains(item.Value))
            //        {
            //            item.Selected = true;
            //        }
            //    }

            //    //List<SelectListItem> selectedItems = tmodel.TList.Where(p => tmodel.SelectedItems1.Contains(p.Value)).ToList();
            //    //foreach (var T in selectedItems)
            //    //{
            //    //    T.Selected = true;
            //    //    //ViewBag.Message += T.Text + " | ";
            //    //}

            //}
            ModelState.Clear();

            return View(model);
        }
        #endregion
        #region ins_quiz
        public ActionResult ins_quiz()
        {
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            FillMst(MyGuid);
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "INSERT QUIZ";
            model[0].Col10 = "questions";
            model[0].Col20 = "answers";
            model[0].Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            model[0].Col12 = "TQU";
            //ViewBag.scripCall = "enableForm();";
            List<SelectListItem> md1 = new List<SelectListItem>();
            List<SelectListItem> md2 = new List<SelectListItem>();
            List<SelectListItem> md3 = new List<SelectListItem>();
            TempData[MyGuid + "_TList1"] = model[0].TList1 = md1 = cmd_Fun.tr_mod(userCode, unitid_mst);

            TempData[MyGuid + "_TList2"] = model[0].TList2 = md2;
            TempData[MyGuid + "_TList3"] = model[0].TList3 = md3;
            model[0].SelectedItems1 = new string[] { "" };
            model[0].SelectedItems2 = new string[] { "" };
            model[0].SelectedItems3 = new string[] { "" };
            return View(model);
        }
        [HttpPost]
        public ActionResult ins_quiz(List<Tmodelmain> model, string command, string mid, HttpPostedFileBase upd1, HttpPostedFileBase upd2, HttpPostedFileBase upd3, HttpPostedFileBase upd4)
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
                    model = getnew(model);
                }
                else if (command == "Cancel")
                {
                    return CancelFun(model);
                }
                else if (command == "Module")
                {

                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vsavenew = "";
                    ViewBag.scripCall = "enableForm();";
                    DataTable dt = new DataTable();
                    dt = sgen.getdata(userCode, "select cat_id,cat_name,mod_id from category where mod_id='" + model[0].SelectedItems1.FirstOrDefault() + "' and type='TCT'" +
                        " and client_unit_id='" + unitid_mst + "'");
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            mod2.Add(new SelectListItem { Text = dr["cat_name"].ToString(), Value = dr["cat_id"].ToString() });
                        }

                    }
                    TempData[MyGuid + "_TList2"] = model[0].TList2 = mod2;
                    DataTable dt1 = new DataTable();
                    dt1 = sgen.getdata(userCode, "select count(ques_id)+1 as quesno from questions where mod_id='" + model[0].SelectedItems1.FirstOrDefault() + "' " +
                        "and type='" + model[0].Col12 + "' ");
                    if (dt1.Rows.Count > 0)
                    {
                        string qno = dt1.Rows[0]["quesno"].ToString();
                        model[0].Col17 = qno;
                    }
                }
                else if (command == "Catagory")
                {
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vsavenew = "";
                    ViewBag.scripCall = "enableForm();";

                    DataTable dt = new DataTable();

                    mq = "select course_id,cat_id,cou_title,cou_ent_by from courses where cat_id='" + model[0].SelectedItems2.FirstOrDefault() + "' " +
            "and cou_ent_by='" + userid_mst + "' and type='TCR' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
                    dt = sgen.getdata(userCode, mq);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            mod3.Add(new SelectListItem { Text = dr["cou_title"].ToString(), Value = dr["course_id"].ToString() });
                        }

                    }
                    TempData[MyGuid + "_TList3"] = model[0].TList3 = mod3;
                    DataTable dt1 = new DataTable();
                    dt1 = sgen.getdata(userCode, "select count(ques_id)+1 as quesno from questions where mod_id='" + model[0].SelectedItems1.FirstOrDefault() + "' and cat_id='"
                        + model[0].SelectedItems2.FirstOrDefault() + "' and ques_ent_by='" + userid_mst + "' and type='" + model[0].Col12 + "'");
                    if (dt1.Rows.Count > 0)
                    {
                        string qno = dt1.Rows[0]["quesno"].ToString();
                        model[0].Col17 = qno;

                    }
                }
                else if (command == "Title")
                {
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vsavenew = "";
                    ViewBag.scripCall = "enableForm();";

                    DataTable dt1 = new DataTable();
                    dt1 = sgen.getdata(userCode, "select course_id,ref_code from courses where course_id='" + model[0].SelectedItems3.FirstOrDefault() + "' and type='TCR' and " +
            "client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
                    if (dt1.Rows.Count > 0)
                    {
                        model[0].Col18 = dt1.Rows[0]["ref_code"].ToString();
                    }
                    DataTable dt = new DataTable();
                    dt = sgen.getdata(userCode, "select count(ques_id)+1 as quesno from questions where mod_id='" + model[0].SelectedItems1.FirstOrDefault() + "' and cat_id='"
           + model[0].SelectedItems2.FirstOrDefault() + "' and cou_title='" + model[0].SelectedItems3.FirstOrDefault() + "' and type='" + model[0].Col12 + "'");
                    if (dt.Rows.Count > 0)
                    {
                        string qno = dt.Rows[0]["quesno"].ToString();
                        model[0].Col17 = qno;

                    }
                }
                else if (command == "Callback")
                {
                    if (sgen.GetSession(MyGuid, "delid") != null) { btnval = "FDEL"; sgen.SetSession(MyGuid, "btnval", btnval); }
                    else if (sgen.GetSession(MyGuid, "btnval") != null) btnval = sgen.GetSession(MyGuid, "btnval").ToString();
                    model = StartCallback(model);
                }
                else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
                {

                    var tmodel = model.FirstOrDefault();
                    string currdate = "";
                    DataTable dtstr = new DataTable();
                    string quesid = "", vch_num = "", vch_num1 = "", ansid = "";
                    Satransaction sat1 = new Satransaction(userCode, MyGuid);


                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        isedit = true;
                        quesid = model[0].Col29;
                        vch_num = model[0].Col27;
                        vch_num1 = model[0].Col16;
                        ansid = model[0].Col30;
                    }
                    else
                    {

                        isedit = false;
                        quesid = sgen.genNo(userCode, "select max(to_number (vch_num)) as max from " + model[0].Col10 + " where type='" + model[0].Col12 + "'" + model[0].Col11 + "", 6, "max");
                        vch_num = sgen.genNo(userCode, "select max(to_number (vch_num)) as max from " + model[0].Col20 + " where type='" + model[0].Col12 + "'" + model[0].Col11 + "", 6, "max");
                        vch_num1 = sgen.genNo(userCode, "select max(to_number (vch_num)) as max from " + model[0].Col10 + " where type='" + model[0].Col12 + "'" + model[0].Col11 + "", 6, "max");
                        ansid = sgen.genNo(userCode, "select max(to_number (vch_num)) as max from " + model[0].Col20 + " where type='" + model[0].Col12 + "'" + model[0].Col11 + "", 6, "max");


                    }

                    


                    DataTable dtfile = cmd_Fun.GetStructure(userCode, "file_tab");
                    string serverpath = Server.MapPath("~/Uploads/" + cg_com_name.Replace(" ", "") + "/");

                    if (model[0].Chk1 == true)
                    {
                        

                        if ((upd1 == null || upd2 == null || upd3 == null || upd4 == null))

                        {
                            ViewBag.scripCall += "showmsgJS(1, 'Please Fill Ques,Ans And Tick The correct Option', 2);";
                            return View(model);
                        }
                    }
                    else
                    {
                        if ((model[0].Col22 == "" || model[0].Col23 == "" || model[0].Col24 == "" || model[0].Col26 == "") || (model[0].Col21 == ""))
                        {
                            ViewBag.scripCall += "showmsgJS(1, 'Please Fill Ques,Ans And Tick The correct Option', 2);";
                            return View(model);
                        }
                    }

                   
                    currdate = sgen.server_datetime(userCode);

                    
                    #region questions
                    dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    DataRow dr = dtstr.NewRow();
                    dr["rec_id"] = "0";
                    dr["type"] = model[0].Col12;
                    dr["vch_date"] = currdate;
                    dr["CREATED_DATE"] = currdate;
                    dr["vch_num"] = vch_num1;
                    dr["client_id"] = clientid_mst;
                    dr["client_unit_id"] = unitid_mst;
                    dr["ques_id"] = quesid;
                    dr["mod_id"] = model[0].SelectedItems1.FirstOrDefault();
                    dr["cat_id"] = model[0].SelectedItems2.FirstOrDefault();
                    dr["cou_title"] = model[0].SelectedItems3.FirstOrDefault();
                    dr["quiz_id"] = model[0].Col17;
                    dr["ref_code"] = model[0].Col18;
                    dr["question"] = model[0].Col19;
                    dr["ans_explanation"] = model[0].Col25;

                    if (isedit)
                    {
                        dr["ques_ent_by"] = model[0].Col3;
                        dr["ques_ent_date"] = model[0].Col4;
                        dr["ques_edit_by"] = userid_mst;
                        dr["ques_edit_date"] = currdate;

                    }
                    else
                    {
                        dr["ques_ent_by"] = userid_mst;
                        dr["ques_ent_date"] = currdate;
                        dr["ques_edit_by"] = "-";
                        dr["ques_edit_date"] = currdate;

                    }
                    dtstr.Rows.Add(dr);
                    bool Result = sgen.Update_data_uncommit2(userCode, dtstr, model[0].Col10, model[0].Col8, isedit, sat1);
                    #endregion
                    DataTable dt = new DataTable();
                    dt = sgen.getdata(userCode, "select max(ques_id) as qid from questions where type='" + model[0].Col12 + "'" + model[0].Col11 + "");

                    if (dt.Rows.Count > 0)
                    {
                        string id_ques = dt.Rows[0]["qid"].ToString();
                        dtstr = cmd_Fun.GetStructure(userCode, model[0].Col20.Trim());
                        int correct_option = 0;
                        string options = "";
                        for (Int32 i = 0; i < 4; i++)
                        {

                            ansid = (sgen.Make_decimal(ansid) + sgen.Make_decimal(i)).ToString();
                            ansid = sgen.padlc(sgen.Make_int(ansid), 6);
                            if (i == 0) { if (model[0].Col21 == "1") { correct_option = 1; } else { correct_option = 0; } }
                            if (i == 1) { if (model[0].Col21 == "2") { correct_option = 1; } else { correct_option = 0; } }
                            if (i == 2) { if (model[0].Col21 == "3") { correct_option = 1; } else { correct_option = 0; } }
                            if (i == 3) { if (model[0].Col21 == "4") { correct_option = 1; } else { correct_option = 0; } }

                            if (i == 0)
                            {
                                options = model[0].Col26;
                                if (upd1 != null)
                                {
                                    HttpPostedFileBase file1 = upd1;
                                    ctype1 = file1.ContentType;
                                    fileName2 = file1.FileName;
                                    path2 = clientid_mst + unitid_mst + userid_mst + currdate + "_" + fileName2;
                                    encpath2 = sgen.Convert_Stringto64(path2).ToString();
                                    finalpath2 = serverpath + encpath2;
                                    file1.SaveAs(finalpath2);
                                }
                            }
                            if (i == 1)
                            {
                                options = model[0].Col22;
                                if (upd2 != null)
                                {
                                    HttpPostedFileBase file1 = upd2;
                                    ctype1 = file1.ContentType;
                                    fileName2 = file1.FileName;
                                    path2 = clientid_mst + unitid_mst + userid_mst + currdate + "_" + fileName2;
                                    encpath2 = sgen.Convert_Stringto64(path2).ToString();
                                    finalpath2 = serverpath + encpath2;
                                    file1.SaveAs(finalpath2);
                                }
                            }

                            if (i == 2)
                            {
                                options = model[0].Col23;
                                if ((upd3 != null))
                                {
                                    HttpPostedFileBase file1 = upd3;
                                    ctype1 = file1.ContentType;
                                    fileName2 = file1.FileName;
                                    path2 = clientid_mst + unitid_mst + userid_mst + currdate + "_" + fileName2;
                                    encpath2 = sgen.Convert_Stringto64(path2).ToString();
                                    finalpath2 = serverpath + encpath2;
                                    file1.SaveAs(finalpath2);
                                }
                            }

                            if (i == 3)
                            {
                                options = model[0].Col26;
                                if (upd4 != null)
                                {
                                    HttpPostedFileBase file1 = upd4;
                                    ctype1 = file1.ContentType;
                                    fileName2 = file1.FileName;
                                    path2 = clientid_mst + unitid_mst + userid_mst + currdate + "_" + fileName2;
                                    encpath2 = sgen.Convert_Stringto64(path2).ToString();
                                    finalpath2 = serverpath + encpath2;
                                    file1.SaveAs(finalpath2);
                                }
                            }

                            dr = dtstr.NewRow();
                            dr["rec_id"] = "0";
                            dr["type"] = model[0].Col12;
                            dr["vch_num"] = vch_num;
                            dr["vch_date"] = currdate;
                            dr["CREATED_DATE"] = currdate;
                            dr["client_id"] = clientid_mst;
                            dr["client_unit_id"] = unitid_mst;
                            dr["ans_id"] = ansid;
                            dr["ques_id"] = vch_num1;
                            dr["img_type"] = ctype1;
                            dr["option_filename"] = fileName2;
                            dr["option_filepath"] = encpath2;
                            dr["correct_option"] = correct_option;
                            dr["options"] = options;
                            dr["COL1"] = "-";
                            dr["COL2"] = "-";
                            dr["COL3"] = "-";
                            dr["COL4"] = "-";
                            dr["COL5"] = "-";
                            dr["COL6"] = "-";
                            dr["COL7"] = "-";
                            dr["COL8"] = "-";
                            dr["COL9"] = "-";
                            dr["COL10"] = "-";
                            dr["COL11"] = "-";
                            dr["R_NO"] = "-";
                            dr["isradio"] = "-";
                            if (isedit)
                            {
                                dr["ent_by"] = model[0].Col3;
                                dr["ent_date"] = model[0].Col4;
                                dr["edit_by"] = userid_mst;
                                dr["edit_date"] = currdate;
                            }
                            else
                            {
                                dr["ent_by"] = userid_mst;
                                dr["ent_date"] = currdate;
                                dr["edit_by"] = "-";
                                dr["edit_date"] = currdate;
                            }
                            dtstr.Rows.Add(dr);

                        }


                        bool ok = sgen.Update_data_uncommit2(userCode, dtstr, model[0].Col20, model[0].Col28, isedit, sat1);
                        if (ok && Result)
                        {
                            sat1.Commit();

                            model = new List<Tmodelmain>();
                            model.Add(new Tmodelmain()
                            {
                                Col9 = tmodel.Col9,
                                Col10 = tmodel.Col10,
                                Col11 = tmodel.Col11,
                                Col12 = tmodel.Col12,
                                Col13 = tmodel.Col13,
                                Col14 = tmodel.Col14,
                                Col15 = tmodel.Col15,
                                Col100 = "Save & New",
                                Col121 = "S",
                                Col122 = "<u>S</u>ave",
                                Col123 = "Save & Ne<u>w</u>",


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
                                    model = getnew(model);
                                    ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                                }
                                catch (Exception ex) { }
                            }
                        }

                        else
                        {
                            sat1.Rollback();
                            ViewBag.scripCall += "showmsgJS(1, 'Record Not Saved ', 0);disableForm();";
                        }


                       
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
        #region make quiz paper
        public ActionResult make_quiz()
        {
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            FillMst(MyGuid);
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "CREATE QUIZ PRAXIS";
            model[0].Col10 = "ques_paper";
            model[0].Col20 = "selected_ques";
            model[0].Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            model[0].Col12 = "TPQ";

            List<SelectListItem> md1 = new List<SelectListItem>();
            List<SelectListItem> md2 = new List<SelectListItem>();
            List<SelectListItem> md3 = new List<SelectListItem>();
            TempData[MyGuid + "_TList1"] = model[0].TList1 = md1 = cmd_Fun.tr_mod(userCode, unitid_mst);
            TempData[MyGuid + "_TList2"] = model[0].TList2 = md2;
            TempData[MyGuid + "_TList3"] = model[0].TList3 = md3;
            model[0].SelectedItems1 = new string[] { "" };
            model[0].SelectedItems2 = new string[] { "" };
            model[0].SelectedItems3 = new string[] { "" };
            return View(model);
        }
        [HttpPost]
        public ActionResult make_quiz(List<Tmodelmain> model, string command, string mid, HttpPostedFileBase upd1, HttpPostedFileBase upd2, HttpPostedFileBase upd3, HttpPostedFileBase upd4)
        {
            try
            {
                type = model[0].Col12;
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
                    model = getnew(model);
                }
                else if (command == "Cancel")
                {
                    return CancelFun(model);
                }
                else if (command == "Module")
                {
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vsavenew = "";
                    ViewBag.scripCall = "enableForm();";

                    DataTable dt = new DataTable();
                    dt = sgen.getdata(userCode, "select cat_id,cat_name,mod_id from category where mod_id='" + model[0].SelectedItems1.FirstOrDefault() + "' and type='TCT' and client_unit_id='" + unitid_mst + "'");
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            mod2.Add(new SelectListItem { Text = dr["cat_name"].ToString(), Value = dr["cat_id"].ToString() });
                        }
                        //mod2.Add(new SelectListItem { Text = "---ALL---", Value = "ALL" });
                        TempData[MyGuid + "_TList2"] = model[0].TList2 = mod2;
                    }
                    //if (model[0].SelectedItems2.FirstOrDefault() == "ALL")
                    //{
                    //    DataTable dt5 = new DataTable();
                    //    dt5 = sgen.getdata(userCode, "select count(ques_id) as noofques from questions where mod_id='" + model[0].SelectedItems1.FirstOrDefault() + "' and ques_ent_by='"
                    //        + userid_mst + "' and type='TQU' and client_id='" + clientid_mst + "' and client_uniit_id='" + unitid_mst + "'");
                    //    if (dt5.Rows.Count > 0)
                    //    {
                    //        model[0].Col17 = dt5.Rows[0]["noofques"].ToString();
                    //    }
                    //    else
                    //    {
                    //        model[0].Col17 = "No Questions";
                    //    }
                    //}
                }
                else if (command == "Catagory")
                {
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vsavenew = "";
                    ViewBag.scripCall = "enableForm();";
                    DataTable dt = new DataTable();

                    mq = "select course_id,cat_id,cou_title,cou_ent_by from courses where mod_id='" + model[0].SelectedItems1.FirstOrDefault() + "' and cat_id='" + model[0].SelectedItems2.FirstOrDefault() + "' " +
            "and cou_ent_by='" + userid_mst + "' and type='TCR' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
                    dt = sgen.getdata(userCode, mq);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            mod3.Add(new SelectListItem { Text = dr["cou_title"].ToString(), Value = dr["course_id"].ToString() });
                        }
                        //mod3.Add(new SelectListItem { Text = "---ALL---", Value = "ALL" });
                        TempData[MyGuid + "_TList3"] = model[0].TList3 = mod3;
                    }
                    //if (model[0].SelectedItems3.FirstOrDefault() == "ALL")
                    //{
                    //    DataTable dt3 = new DataTable();

                    //    DataTable dt4 = new DataTable();
                    //    dt4 = sgen.getdata(userCode, "select count(ques_id) as noofques from questions where mod_id='" + model[0].SelectedItems1.FirstOrDefault() + "' and cat_id='"
                    //        + model[0].SelectedItems2.FirstOrDefault() + "' and ques_ent_by='" + userid_mst + "' and type='TQU' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
                    //    if (dt4.Rows.Count > 0)
                    //    {
                    //        model[0].Col17 = dt4.Rows[0]["noofques"].ToString();
                    //    }
                    //    else
                    //    {
                    //        model[0].Col17 = "No Questions";
                    //    }
                    //}

                    //if (model[0].SelectedItems2.FirstOrDefault() == "ALL")
                    //{
                    //    model[0].Col18 = "";
                    //}
                }
                else if (command == "Title")
                {
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vsavenew = "";
                    ViewBag.scripCall = "enableForm();";

                    DataTable dt1 = new DataTable();
                    dt1 = sgen.getdata(userCode, "select course_id,ref_code from courses where course_id='" + model[0].SelectedItems3.FirstOrDefault() + "' and type='TCR' and " +
            "client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
                    if (dt1.Rows.Count > 0)
                    {
                        model[0].Col18 = dt1.Rows[0]["ref_code"].ToString();
                    }
                    DataTable dt2 = sgen.getdata(userCode, "select count(ques_id) as noofques from questions where ref_code='" + model[0].Col18 + "' and type='TQU'  " +
               "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
                    if (dt2.Rows.Count > 0)
                    {
                        model[0].Col17 = dt2.Rows[0]["noofques"].ToString();
                    }
                    else
                    {
                        model[0].Col17 = "No Questions";
                    }
                    DataTable dt = new DataTable();
                    mq = "select questions.ques_id,questions.question,questions.mod_id,questions.cat_id,questions.ref_code,questions.ans_explanation," +
                        "module.mod_name,category.cat_name,courses.cou_title,questions.ques_ent_by from questions inner join module on module.mod_id=questions.mod_id and module.type='TMD' and module.client_unit_id='" + unitid_mst + "' " +
                        " inner join category on category.cat_id = questions.cat_id and category.type='TCT'  and questions.mod_id=category.mod_id  and category.client_unit_id='" + unitid_mst + "'inner join courses on courses.ref_code = questions.ref_code and " +
                        "courses.type='TCR' and courses.client_unit_id='" + unitid_mst + "'  where questions.ref_code='" + model[0].Col18 + "' and questions.cou_title='" + model[0].SelectedItems3.FirstOrDefault()
                        + "' and questions.ques_ent_by='" + userid_mst + "' and questions.type='TQU' and questions.client_id='" + clientid_mst + "' and questions.client_unit_id='" + unitid_mst + "'";
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
                                Chk1 = tm.Chk1,
                                Col16 = tm.Col16,
                                Col17 = tm.Col17,
                                Col18 = tm.Col18,
                                Col19 = tm.Col19,
                                Col20 = tm.Col20,
                                Col100 = tm.Col100,
                                Col121 = tm.Col121,
                                Col122 = tm.Col122,
                                Col123 = tm.Col123,
                                Col21 = dr["ques_id"].ToString(),
                                Col22 = dr["MOD_NAME"].ToString(),
                                Col23 = dr["CAT_NAME"].ToString(),
                                Col24 = dr["COU_TITLE"].ToString(),
                                Col25 = dr["QUESTION"].ToString(),
                                TList1 = tm.TList1,
                                SelectedItems1 = tm.SelectedItems1,
                                TList2 = tm.TList2,
                                SelectedItems2 = tm.SelectedItems2,
                                TList3 = tm.TList3,
                                SelectedItems3 = tm.SelectedItems3,
                            });
                        }
                        if (model[0].SelectedItems3.FirstOrDefault() == "ALL")
                        {
                            model[0].Col18 = "";
                        }
                    }
                }
                else if (command == "Callback")
                {
                    model = StartCallback(model);
                }
                else if (command == "Pname")
                {
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vsavenew = "";
                    ViewBag.scripCall = "enableForm();";
                    //if (model[0].SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
                    //if (model[0].SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
                    //if (model[0].SelectedItems3 == null) model[0].SelectedItems3 = new string[] { "" };
                    DataTable dt = new DataTable();
                    dt = sgen.getdata(userCode, "select QUES_PAPER_NAME,QUES_PAPER_ID,QUES_PAPER_ENT_BY from ques_paper where QUES_PAPER_NAME='" + model[0].Col19 + "' and type='TQP' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
                    if (dt.Rows.Count > 0)
                    {
                        string userid = Convert.ToString(dt.Rows[0]["QUES_PAPER_ENT_BY"]);
                        ViewBag.scripCall = "showmsgJS(1, 'Ques Paper Name Already Taken By " + userid + "', 2);";
                        return View(model);
                    }
                }
                else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
                {
                    //var tmodel = model.FirstOrDefault();
                    //string dt_ft = "YYYY-MM-dd HH24:MI:SS";
                    //string currdate = sgen.server_datetime(userCode);

                    //DataTable dtstr = new DataTable();
                    //dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());

                    //string qpaperid = "select to_number(max(ques_paper_id)) as auto_genid from ques_paper where type='TQP' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
                    //qpaperid = sgen.genNo(userCode, qpaperid, 6, "auto_genid");

                    //string vch_num = "select to_number(max(vch_num)) as auto_genid from ques_paper where type='TQP' and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
                    //vch_num = sgen.genNo(userCode, vch_num, 6, "auto_genid");

                    var tmodel = model.FirstOrDefault();
                    string currdate = sgen.server_datetime(userCode);

                    DataTable dtstr = new DataTable();
                    string qpaperid = "", vch_num = "", vch_num1 = "", ansid = "", selquesid = "";
                    bool ok = false;
                    Satransaction sat1 = new Satransaction(userCode, MyGuid);


                    if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                    {
                        isedit = true;
                        qpaperid = model[0].Col16;
                        vch_num = model[0].Col16;
                        selquesid = model[0].Col16;

                    }
                    else
                    {

                        isedit = false;
                        qpaperid = sgen.genNo(userCode, "select max(to_number (ques_paper_id)) as max from " + model[0].Col10 + " where type='" + model[0].Col12 + "'" + model[0].Col11 + "", 6, "max");
                        vch_num = sgen.genNo(userCode, "select max(to_number (vch_num)) as max from " + model[0].Col10 + " where type='" + model[0].Col12 + "'" + model[0].Col11 + "", 6, "max");
                        selquesid = sgen.genNo(userCode, "select max(to_number (sel_ques_id)) as max from " + model[0].Col20 + " where type='" + model[0].Col12 + "'" + model[0].Col11 + "", 6, "max");


                    }

                    if (model[0].Col19 == "")
                    {
                        ViewBag.scripCall = "showmsgJS(1, 'Quiz Praxis Name Required', 2);";

                        return View(model);
                    }

                    for (int i = 0; i < model.Count; i++)
                    {
                        //if (model[0].Chk1 == false)
                        //{
                        //    ViewBag.scripCall = "showmsgJS(1, 'Please select Quespaper', 2);";
                        //    return View(model);
                        //}
                    }
                    //res = sgen.execute_cmd(userCode, "insert into ques_paper(ques_paper_id,ques_paper_name,ques_paper_ent_by,ques_paper_ent_date,cou_title,mod_id,cat_id," +
                    //    "client_id,client_unit_id,type,vch_num,CREATED_DATE,VCH_DATE)values('" + qpaperid + "','" + model[0].Col19 + "','" + userid_mst + "',to_date('" + currdate + "','" + dt_ft + "')" +
                    //    ",'" + model[0].SelectedItems3.FirstOrDefault() + "','" + model[0].SelectedItems1.FirstOrDefault() + "','" + model[0].SelectedItems3.FirstOrDefault() + "','" + clientid_mst
                    //    + "','" + unitid_mst + "','TQP','" + vch_num + "',to_date('" + currdate + "','" + dt_ft + "'),to_date('" + currdate + "','" + dt_ft + "'))");
                    #region questions
                    dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                    DataRow dr = dtstr.NewRow();
                    dr["rec_id"] = "0";
                    dr["type"] = model[0].Col12;
                    dr["vch_date"] = currdate;
                    dr["CREATED_DATE"] = currdate;
                    dr["vch_num"] = vch_num;
                    dr["client_id"] = clientid_mst;
                    dr["client_unit_id"] = unitid_mst;
                    dr["ques_paper_id"] = qpaperid;
                    dr["ques_paper_name"] = model[0].Col19;
                    dr["mod_id"] = model[0].SelectedItems1.FirstOrDefault();
                    dr["cat_id"] = model[0].SelectedItems2.FirstOrDefault();
                    dr["cou_title"] = model[0].SelectedItems3.FirstOrDefault();





                    if (isedit)
                    {
                        dr["ques_paper_ent_by"] = model[0].Col3;
                        dr["ques_paper_ent_date"] = model[0].Col4;
                        dr["ques_paper_edit_by"] = userid_mst;
                        dr["ques_paper_edit_date"] = currdate;

                    }
                    else
                    {
                        dr["ques_paper_ent_by"] = userid_mst;
                        dr["ques_paper_ent_date"] = currdate;
                        dr["ques_paper_edit_by"] = "-";
                        dr["ques_paper_edit_date"] = currdate;

                    }
                    dtstr.Rows.Add(dr);
                    bool Result = sgen.Update_data_uncommit2(userCode, dtstr, model[0].Col10, model[0].Col8, isedit, sat1);
                    #endregion
                    dtstr = cmd_Fun.GetStructure(userCode, model[0].Col20.Trim());
                    for (int i = 0; i < model.Count; i++)
                    {
                        if (model[i].Chk1)
                        {
                            //string selquesid = "select to_number(max(sel_ques_id)) as auto_genid from selected_ques where type='" + model[0].Col12 + "'" + model[0].Col11 + "";
                            //selquesid = sgen.genNo(userCode, selquesid, 6, "auto_genid");

                            DataTable dt1 = new DataTable();
                            dt1 = sgen.getdata(userCode, "select max(ques_paper_id) as quepaperid from ques_paper where type='TQP' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
                            if (dt1.Rows.Count > 0)
                            {
                                try
                                {
                                    string quepapr = dt1.Rows[0]["quepaperid"].ToString();
                                    //sgen.execute_cmd(userCode, "insert into selected_ques(sel_ques_id,ques_id,ques_paper_id,mod_id,cat_id,cou_title,ref_code," +
                                    //    "sel_ques_ent_by,sel_ques_ent_date,client_id,client_unit_id,type,vch_num,CREATED_DATE,VCH_DATE)values('" + selquesid + "','" + model[i].Col21 + "','" + quepapr
                                    //    + "','" + model[0].SelectedItems1.FirstOrDefault() + "','" + model[0].SelectedItems2.FirstOrDefault() + "','" + model[0].SelectedItems1.FirstOrDefault() + "','"
                                    //    + model[0].Col18 + "','" + userid_mst + "',to_date('" + currdate + "','" + dt_ft + "'),'" + clientid_mst + "','" + unitid_mst + "','TQP','" + vch_num + "',to_date('" + currdate + "','" + dt_ft + "'),to_date('" + currdate + "','" + dt_ft + "'))");

                                    #region selected questions

                                    dr = dtstr.NewRow();
                                    dr["rec_id"] = "0";
                                    dr["type"] = model[0].Col12;
                                    dr["vch_date"] = currdate;
                                    dr["CREATED_DATE"] = currdate;
                                    dr["vch_num"] = vch_num;
                                    dr["client_id"] = clientid_mst;
                                    dr["client_unit_id"] = unitid_mst;
                                    dr["sel_ques_id"] = selquesid;
                                    dr["ref_code"] = model[0].Col18;
                                    dr["ques_id"] = model[i].Col21;
                                    dr["ques_paper_id"] = quepapr;
                                    dr["mod_id"] = model[0].SelectedItems1.FirstOrDefault();
                                    dr["cat_id"] = model[0].SelectedItems2.FirstOrDefault();
                                    dr["cou_title"] = model[0].SelectedItems3.FirstOrDefault();
                                    if (isedit)
                                    {
                                        dr["sel_ques_ent_by"] = model[0].Col3;
                                        dr["sel_ques_ent_date"] = model[0].Col4;
                                        dr["sel_ques_edit_by"] = userid_mst;
                                        dr["sel_ques_edit_dt"] = currdate;

                                    }
                                    else
                                    {
                                        dr["sel_ques_ent_by"] = userid_mst;
                                        dr["sel_ques_ent_date"] = currdate;
                                        dr["sel_ques_edit_by"] = "-";
                                        dr["sel_ques_edit_dt"] = currdate;

                                    }
                                    dtstr.Rows.Add(dr);

                                    #endregion


                                }
                                catch (Exception ex)
                                {
                                    ViewBag.scripCall = "showmsgJS(1, '" + ex.Message.ToString() + "', 0);";

                                }
                            }
                        }
                    }
                    ok = sgen.Update_data_uncommit2(userCode, dtstr, model[0].Col20, model[0].Col27, isedit, sat1);
                    if (ok && Result)
                    {
                        sat1.Commit();

                        model = new List<Tmodelmain>();
                        model.Add(new Tmodelmain()
                        {
                            Col9 = tmodel.Col9,
                            Col10 = tmodel.Col10,
                            Col11 = tmodel.Col11,
                            Col12 = tmodel.Col12,
                            Col13 = tmodel.Col13,
                            Col14 = tmodel.Col14,
                            Col15 = tmodel.Col15,
                            Col100 = "Save & New",
                            Col121 = "S",
                            Col122 = "<u>S</u>ave",
                            Col123 = "Save & Ne<u>w</u>",


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
                                model = getnew(model);
                                ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                            }
                            catch (Exception ex) { }
                        }
                    }

                    else
                    {
                        sat1.Rollback();
                        ViewBag.scripCall += "showmsgJS(1, 'Record Not Saved ', 0);disableForm();";
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
        #region assign quiz
        public ActionResult asn_quiz()
        {
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            FillMst(MyGuid);
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].LTM1 = new List<Tmodelmain>();
            Tmodelmain tmltm1 = new Tmodelmain();
            model[0].LTM2 = new List<Tmodelmain>();
            Tmodelmain tmltm2 = new Tmodelmain();
            model[0].Col9 = "ASSIGN QUIZ PRAXIS TO TRAINEE";
            model[0].Col10 = "user_quespaper";
            //model[0].Col20 = "answers";
            model[0].Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            model[0].Col12 = "TRS";
            model[0].Col30 = "CP";
            model[0].Col31 = "TQA";
            ViewBag.scripCall = "enableForm();";
            List<SelectListItem> md1 = new List<SelectListItem>();
            List<SelectListItem> md2 = new List<SelectListItem>();
            List<SelectListItem> md3 = new List<SelectListItem>();
            List<SelectListItem> md4 = new List<SelectListItem>();
            List<SelectListItem> md5 = new List<SelectListItem>();
            TempData[MyGuid + "_TList1"] = model[0].TList1 = md1 = cmd_Fun.tr_mod(userCode, unitid_mst);
            TempData[MyGuid + "_TList2"] = model[0].TList2 = md2;
            TempData[MyGuid + "_TList3"] = model[0].TList3 = md3;
            md4.Add(new SelectListItem { Text = "No", Value = "0" });
            md4.Add(new SelectListItem { Text = "Yes", Value = "1" });
            md5.Add(new SelectListItem { Text = "Created Group", Value = "1" });
            md5.Add(new SelectListItem { Text = "Individual", Value = "2" });
            TempData[MyGuid + "_TList4"] = model[0].TList4 = md4;
            TempData[MyGuid + "_TList5"] = model[0].TList5 = md5;
            model[0].SelectedItems4 = new string[] { "" };
            model[0].SelectedItems5 = new string[] { "" };
            model[0].SelectedItems1 = new string[] { "" };
            model[0].SelectedItems2 = new string[] { "" };
            model[0].SelectedItems3 = new string[] { "" };
            tmltm1.Col1 = "1";
            model[0].LTM1.Add(tmltm1);
            tmltm2.Col1 = "1";
            model[0].LTM2.Add(tmltm2);
            return View(model);
        }
        [HttpPost]
        public ActionResult asn_quiz(List<Tmodelmain> model, string command, string mid, HttpPostedFileBase upd1, HttpPostedFileBase upd2, HttpPostedFileBase upd3, HttpPostedFileBase upd4)
        {
            try
            {
                type = model[0].Col12;
                FillMst(model[0].Col15);
                var tm = model.FirstOrDefault();
                #region dropdowns
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
                List<SelectListItem> mod5 = (List<SelectListItem>)TempData[MyGuid + "_TList5"];
                TempData[MyGuid + "_TList5"] = model[0].TList5 = mod5;
                if (model[0].SelectedItems5 == null) model[0].SelectedItems5 = new string[] { "" };
                #endregion
                if (command == "Cancel")
                {
                    return CancelFun(model);
                }
                else if (command == "GTYPE")
                {
                    DataTable dt = new DataTable();

                    if (model[0].SelectedItems5.FirstOrDefault() == "2")
                    {
                        //mq = "select user_id,rec_id,department,first_name,last_name,email,designation fROM user_details where " +
                        //    " find_in_set(client_unit_id,'"+unitid_mst+"')=1 and  status='active' and type='CPR' and find_in_set('1000', mod_id)";
                        mq = "select  ms.master_id  dept,ud.user_id,ud.vch_num,ud.client_unit_id,ud.first_name || '' || " +
                            "replace(ud.middle_name, '0', '')|| ' ' || replace(ud.last_name, '0', '') AS Name," +
                            "ms.master_name as department,d.master_name as designation,ud.email from user_details ud left join master_setting" +
                            " ms on ms.master_id = ud.department and ms.type = 'MDP' left join master_setting d on d.master_id = " +
                            "ud.designation and d.type = 'MDG' where ud.type = 'CPR' and ud.status = 'active' and find_in_set('1000',ud.mod_id)= 1 " +
                            "and find_in_set('" + unitid_mst + "',ud.client_unit_id)= 1  ";
                        dt = sgen.getdata(userCode, mq);
                        model[0].LTM2 = new List<Tmodelmain>();

                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {

                                Tmodelmain tltm2 = new Tmodelmain();
                                tltm2.Col1 = dt.Rows[i]["user_id"].ToString();
                                tltm2.Col2 = dt.Rows[i]["vch_num"].ToString();
                                tltm2.Col3 = dt.Rows[i]["Name"].ToString();
                                tltm2.Col4 = dt.Rows[i]["department"].ToString();
                                tltm2.Col5 = dt.Rows[i]["designation"].ToString();
                                tltm2.Col6 = dt.Rows[i]["email"].ToString();
                                model[0].LTM2.Add(tltm2);

                            }
                        }
                    }


                    if (model[0].SelectedItems5.FirstOrDefault() == "1")
                    {
                        dt = sgen.getdata(userCode, "select group_id as col1,group_name as col2,'' as col3 from group_details " +
                            "where type='TGP' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
                        model[0].LTM1 = new List<Tmodelmain>();

                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                Tmodelmain ltm2 = new Tmodelmain();
                                DataTable dt1 = sgen.getdata(userCode, "select count(user_id) as usercount from usergroup where " +
                                   "group_id='" + dt.Rows[i]["col1"].ToString() + "' and status='assigned' and type='TGP' " + model[0].Col11 + "");

                                ltm2.Col21 = dt.Rows[i]["col1"].ToString(); //
                                ltm2.Col22 = dt.Rows[i]["col2"].ToString();  //
                                ltm2.Col23 = dt1.Rows[0]["usercount"].ToString();  //
                                model[0].LTM1.Add(ltm2);
                            }
                        }
                    }
                }
                
                else if (command == "Module")
                {
                    //ViewBag.scripCall = "enableForm();";
                    //if (model[0].SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
                    //if (model[0].SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
                    //if (model[0].SelectedItems3 == null) model[0].SelectedItems3 = new string[] { "" };
                    //if (model[0].SelectedItems4 == null) model[0].SelectedItems4 = new string[] { "" };
                    //if (model[0].SelectedItems5 == null) model[0].SelectedItems5 = new string[] { "" };

                    DataTable dt = new DataTable();
                    dt = sgen.getdata(userCode, "select cat_id,cat_name,mod_id from category where mod_id='" + model[0].SelectedItems1.FirstOrDefault() + "' and client_unit_id='" + unitid_mst + "' and type='TCT'");
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            mod2.Add(new SelectListItem { Text = dr["cat_name"].ToString(), Value = dr["cat_id"].ToString() });
                        }
                        //mod2.Add(new SelectListItem { Text = "---ALL---", Value = "ALL" });
                        TempData[MyGuid + "_TList2"] = model[0].TList2 = mod2;
                    }
                    //if (model[0].SelectedItems2.FirstOrDefault() == "ALL")
                    //{
                    //    DataTable dt5 = new DataTable();
                    //    dt5 = sgen.getdata(userCode, "select count(ques_id) as noofques from questions where mod_id='" + model[0].SelectedItems1.FirstOrDefault() + "' and ques_ent_by='"
                    //        + userid_mst + "' and type='TQU' and client_id='" + clientid_mst + "' and client_uniit_id='" + unitid_mst + "'");
                    //    if (dt5.Rows.Count > 0)
                    //    {
                    //        model[0].Col17 = dt5.Rows[0]["noofques"].ToString();
                    //    }
                    //    else
                    //    {
                    //        model[0].Col17 = "No Questions";
                    //    }
                    //}
                }
                else if (command == "Catagory")
                {
                    //ViewBag.scripCall = "enableForm();";
                    //if (model[0].SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
                    //if (model[0].SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
                    //if (model[0].SelectedItems3 == null) model[0].SelectedItems3 = new string[] { "" };
                    //if (model[0].SelectedItems4 == null) model[0].SelectedItems4 = new string[] { "" };
                    //if (model[0].SelectedItems5 == null) model[0].SelectedItems5 = new string[] { "" };
                    DataTable dt = new DataTable();

                    mq = "select course_id,cat_id,cou_title,cou_ent_by from courses where  mod_id='" + model[0].SelectedItems1.FirstOrDefault() + "' and cat_id='" + model[0].SelectedItems2.FirstOrDefault() + "' " +
            "and cou_ent_by='" + userid_mst + "' and type='TCR' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
                    dt = sgen.getdata(userCode, mq);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            mod3.Add(new SelectListItem { Text = dr["cou_title"].ToString(), Value = dr["course_id"].ToString() });
                        }
                        //mod3.Add(new SelectListItem { Text = "---ALL---", Value = "ALL" });
                        TempData[MyGuid + "_TList3"] = model[0].TList3 = mod3;
                    }
                    //if (model[0].SelectedItems3.FirstOrDefault() == "ALL")
                    //{
                    //    DataTable dt3 = new DataTable();

                    //    DataTable dt4 = new DataTable();
                    //    dt4 = sgen.getdata(userCode, "select count(ques_id) as noofques from questions where mod_id='" + model[0].SelectedItems1.FirstOrDefault() + "' and cat_id='"
                    //        + model[0].SelectedItems2.FirstOrDefault() + "' and ques_ent_by='" + userid_mst + "' and type='TQU' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
                    //    if (dt4.Rows.Count > 0)
                    //    {
                    //        model[0].Col17 = dt4.Rows[0]["noofques"].ToString();
                    //    }
                    //    else
                    //    {
                    //        model[0].Col17 = "No Questions";
                    //    }
                    //}

                    //if (model[0].SelectedItems2.FirstOrDefault() == "ALL")
                    //{
                    //    model[0].Col18 = "";
                    //}
                }
                else if (command == "Title")
                {
                    DataTable dt1 = new DataTable();
                    dt1 = sgen.getdata(userCode, "select course_id,ref_code from courses where course_id='" + model[0].SelectedItems3.FirstOrDefault() + "' and type='TCR' and " +
            "client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
                    if (dt1.Rows.Count > 0)
                    {
                        model[0].Col18 = dt1.Rows[0]["ref_code"].ToString();
                    }
                    DataTable dt2 = sgen.getdata(userCode, "select count(ques_id) as noofques from questions where ref_code='" + model[0].Col18 + "' and type='TQU'  " +
               "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
                    if (dt2.Rows.Count > 0)
                    {
                        model[0].Col17 = dt2.Rows[0]["noofques"].ToString();
                    }
                    else
                    {
                        model[0].Col17 = "No Questions";
                    }
                    DataTable dt = new DataTable();
                    mq = "select questions.ques_id,questions.question,questions.mod_id,questions.cat_id,questions.ref_code,questions.ans_explanation," +
                        "module.mod_name,category.cat_name,courses.cou_title,questions.ques_ent_by from questions inner join module on module.mod_id=questions.mod_id and module.type='TMD' and module.client_unit_id='" + unitid_mst + "' " +
                        " inner join category on category.cat_id = questions.cat_id and category.type='TCT' and category.mod_id=module.mod_id and category.client_unit_id='" + unitid_mst + "' inner join courses on courses.ref_code = questions.ref_code and " +
                        "courses.type='TCR'  and courses.client_unit_id='" + unitid_mst + "' where questions.ref_code='" + model[0].Col18 + "' and questions.cou_title='" + model[0].SelectedItems3.FirstOrDefault()
                        + "' and questions.ques_ent_by='" + userid_mst + "' and questions.type='TQU' and questions.client_id='" + clientid_mst + "' and questions.client_unit_id='" + unitid_mst + "'";
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
                                Chk1 = tm.Chk1,
                                Chk2 = tm.Chk2,
                                Col16 = tm.Col16,
                                Col17 = tm.Col17,
                                Col18 = tm.Col18,
                                Col19 = tm.Col19,
                                Col20 = tm.Col20,
                                Col30 = tm.Col30,
                                Col31 = tm.Col31,
                                Col21 = dr["ques_id"].ToString(),
                                Col22 = dr["MOD_NAME"].ToString(),
                                Col23 = dr["CAT_NAME"].ToString(),
                                Col24 = dr["COU_TITLE"].ToString(),
                                Col25 = dr["QUESTION"].ToString(),
                                TList1 = tm.TList1,
                                SelectedItems1 = tm.SelectedItems1,
                                TList2 = tm.TList2,
                                SelectedItems2 = tm.SelectedItems2,
                                TList3 = tm.TList3,
                                SelectedItems3 = tm.SelectedItems3,
                                TList4 = tm.TList4,
                                SelectedItems4 = tm.SelectedItems4,
                                TList5 = tm.TList5,
                                SelectedItems5 = tm.SelectedItems5,
                            });
                        }
                        if (model[0].SelectedItems3.FirstOrDefault() == "ALL")
                        {
                            model[0].Col18 = "";
                        }
                    }
                }
                else if (command == "Callback")
                {
                    model = StartCallback(model);
                }
                else if (command == "Pname")
                {
                    ViewBag.scripCall = "enableForm();";
                    if (model[0].SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
                    if (model[0].SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
                    if (model[0].SelectedItems3 == null) model[0].SelectedItems3 = new string[] { "" };
                    if (model[0].SelectedItems4 == null) model[0].SelectedItems4 = new string[] { "" };
                    if (model[0].SelectedItems5 == null) model[0].SelectedItems5 = new string[] { "" };
                    DataTable dt = new DataTable();
                    dt = sgen.getdata(userCode, "select QUES_PAPER_NAME,QUES_PAPER_ID,QUES_PAPER_ENT_BY from ques_paper where QUES_PAPER_NAME='" + model[0].Col19 + "' and type='TQP' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
                    if (dt.Rows.Count > 0)
                    {
                        string userid = Convert.ToString(dt.Rows[0]["QUES_PAPER_ENT_BY"]);
                        ViewBag.scripCall = "showmsgJS(1, 'Ques Paper Name Already Taken By " + userid + "', 2);";
                        return View(model);
                    }
                }
                else if (command == "Assign")
                {
                    Satransaction sat = new Satransaction(userCode, MyGuid);
                    string quepaperid = "", quespapername = "", groupquepaperid = "", grpid = "", grpname = "", mandatory = "No";
                    if (model[0].SelectedItems4.FirstOrDefault() == "1") mandatory = "Yes";
                    string currdate = sgen.server_datetime(userCode);

                    DataTable dtb = new DataTable();
                    dtb = sgen.getdata(userCode, "select com_email,com_password,com_port,com_smtp from company_profile where Company_Profile_Id='" + clientid_mst + "' and type='" + model[0].Col30 + "'");
                    if (dtb.Rows.Count > 0)
                    {
                        smtpvalue = Convert.ToString(dtb.Rows[0]["com_smtp"]);
                        emailIdvalue = Convert.ToString(dtb.Rows[0]["com_email"]);
                        passwordvalue = EncryptDecrypt.Decrypt(dtb.Rows[0]["com_password"].ToString());
                        portvalue = Convert.ToInt32(dtb.Rows[0]["com_port"]);
                    }
                    if (smtpvalue == "" && passwordvalue == "" && portvalue == 0) sgen.showmsg(1, "Please configure your mail first", 2);
                    groupquepaperid = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where type='" + model[0].Col12 + "' and client_unit_id='" + unitid_mst + "'";
                    groupquepaperid = sgen.genNo(userCode, groupquepaperid, 6, "auto_genid");
                    vch_num = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where type='" + model[0].Col12 + "' and client_unit_id='" + unitid_mst + "'";
                    vch_num = sgen.genNo(userCode, vch_num, 6, "auto_genid");
                    DataTable dataTable = cmd_Fun.GetStructure(userCode, model[0].Col10);
                    string email = "", quespaper = "";

                    for (int i = 0; i < model.Count; i++)
                    {
                        if (model[i].Chk1 == true)
                        {
                            if (model[0].SelectedItems5.FirstOrDefault() == "2")
                            {
                                for (int j = 0; j < model[0].LTM2.Count; j++)
                                {
                                    if (model[0].LTM2[j].Chk1 == true)
                                    {
                                        DataRow dr = dataTable.NewRow();
                                        dr["rec_id"] = "0";
                                        dr["type"] = model[0].Col12;
                                        dr["vch_date"] = currdate;
                                        dr["ent_by"] = userid_mst;
                                        dr["ent_date"] = currdate;
                                        dr["client_id"] = clientid_mst;
                                        dr["client_unit_id"] = unitid_mst;
                                        dr["vch_num"] = vch_num.Trim();
                                        dr["user_quespaperid"] = groupquepaperid.Trim();
                                        dr["mod_id"] = model[0].SelectedItems1.FirstOrDefault();
                                        dr["cat_id"] = model[0].SelectedItems2.FirstOrDefault();
                                        dr["cou_title"] = model[0].SelectedItems3.FirstOrDefault();
                                        dr["ref_code"] = model[0].Col18;
                                        dr["quespaper_startdt"] = sgen.Make_date_S(model[0].Col17);
                                        dr["quespaper_enddt"] = sgen.Make_date_S(model[0].Col19);
                                        dr["ques_paper_id"] = model[i].Col21;
                                        dr["quespaper_name"] = model[i].Col25;
                                        dr["user_id"] = model[0].LTM2[j].Col2;
                                        dr["status"] = "assigned";
                                        dr["quespaper_status"] = "not completed";
                                        dr["mod_name"] = ((List<SelectListItem>)TempData[MyGuid + "_Tlist1"]).SingleOrDefault(item => item.Value == model[0].SelectedItems1.FirstOrDefault().ToString()).Value.ToString();
                                        dr["cat_name"] = ((List<SelectListItem>)TempData[MyGuid + "_Tlist2"]).SingleOrDefault(item => item.Value == model[0].SelectedItems2.FirstOrDefault().ToString()).Value.ToString();
                                        dr["group_name"] = "Individual";
                                        dr["group_id"] = "0";
                                        dr["ques_mandatory"] = mandatory;
                                        //dr["trn_id"] = Url;
                                        if ((model[0].LTM2[j].Col6 != "") && (model[0].LTM2[j].Col6 != null) && (model[0].LTM2[j].Col6 != "0"))
                                        {
                                            if (email == "")
                                            {
                                                email = model[0].LTM2[j].Col6;
                                            }
                                            else { email = email + ',' + model[0].LTM2[j].Col6; }
                                        }

                                        if ((model[i].Col25 != "") && (model[i].Col25 != null) && (model[i].Col25 != "0"))
                                        {
                                            if (quespaper == "")
                                            {
                                                quespaper = model[i].Col25;
                                            }
                                            else { quespaper = quespaper + ',' + model[i].Col25; }
                                        }
                                        dataTable.Rows.Add(dr);
                                    }
                                }
                            }

                        }
                        if (email != "")
                        {
                            email = email.TrimEnd(',');
                        }
                        if (quespaper != "")
                        {
                            quespaper = quespaper.TrimEnd(',');
                        }
                        
                    }

                    bool Result = sgen.Update_data_fast1(userCode, dataTable, model[0].Col10.Trim(), model[0].Col8, isedit);
                    if (Result == true)
                    {
                        #region mail
                        if (email != "")
                        {
                            string msgbody = "";
                            MailMessage mailMsg = new MailMessage();
                            mailMsg.Subject = "Assign Quiz Praxis";

                            msgbody = "<b style='color: #3caee9; font-size: 20px'>Quiz Praxis Assigned By Admin</b><br /><hr style='border:1px solid black' /><p>Hi ,</p><p>Congratulations, This is to notify that you have been assigned quiz praxis. your quiz praxis details are as below :-</p><table style='font-weight:600'><tr><td>Company Name </td><td>: " + clientname_mst + " </td></tr><tr><td> Unit Name </td><td>: " + unitname_mst + " </td></tr><tr><td> Quiz Praxis</td><td>: " + quespaper + " </td></tr><tr><td> URL </td><td>: " + subdomain_mst + " </td></tr></table><br /><p>Please login and complete your quiz praxis.</p><p>Thank you,<br />Administrator<br /></p><br /><p>Note:- Please do not reply to this mail as it is an unmonitered alias.</p>";

                            mailMsg.Body = msgbody;
                            mailMsg.To.Add(new MailAddress(emailIdvalue));
                            mailMsg.CC.Add(email);
                            mailMsg.From = new MailAddress(emailIdvalue);
                            mailMsg.IsBodyHtml = true;
                            mailMsg.Priority = MailPriority.High;
                            SendEmailInBackgroundThread(mailMsg);
                            model = new List<Tmodelmain>();
                            model.Add(new Tmodelmain
                            {
                                Col10 = tm.Col10,
                                Col11 = tm.Col11,
                                Col12 = tm.Col12,
                                
                                Col14 = tm.Col14,
                                Col15 = tm.Col15,
                                Col9 = tm.Col9,
                                TList1 = mod1,
                                TList2 = mod2,
                                TList3 = mod3,
                                TList4 = mod4,
                                TList5 = mod5,
                                

                                SelectedItems1 = new string[] { "" },
                                SelectedItems2 = new string[] { "" },
                                SelectedItems3 = new string[] { "" },
                                SelectedItems4 = new string[] { "" },
                                SelectedItems5 = new string[] { "" },
                               

                            });
                            ViewBag.scripCall= "mytoast('success', 'toast-top-right', 'Assign Quiz Praxis Successfully');disableForm();";
                        }
                        #endregion;
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

        #region My Training
        public ActionResult my_trn()
        {
            FillMst();


            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "MY TRAINING";
            model[0].Col10 = "usercourse";
            model[0].Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            model[0].Col12 = "TCU";
            var tm = model.FirstOrDefault();
            string mq = "select uc.rec_id,uc.ent_by,uc.usercourse_id,uc.group_name,m.mod_name,ct.cat_name,cr.cou_title,uc.ref_code,uc.user_id," +
                "uc.user_presense,uc.dwn_permission,to_char(convert_tz(uc.start_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldatetimeformat() + "') as start_date " +
                ",to_char(convert_tz(uc.end_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldatetimeformat() + "') as end_date,uc.training_status,uc.Usercourse_status," +
                "uc.trn_duration,uc.group_id,uc.trn_mandatory,uc.trn_type as training_type  from usercourse uc inner join module m on m.mod_id = uc.mod_id" +
                " and m.type = 'TMD' and m.client_unit_id = '" + unitid_mst + "' inner join category ct on ct.cat_id = uc.cat_id and ct.type = 'TCT' " +
                "and m.mod_id = ct.mod_id and ct.client_unit_id = '" + unitid_mst + "' inner join courses cr on cr.course_id = uc.course_id and cr.type = 'TCR'" +
                " and cr.client_id = uc.client_id inner join user_details ud on ud.vch_num = uc.user_id and ud.type = 'CPR'  and find_in_set(ud.client_unit_id,'" + unitid_mst + "')= 1" +
                " where uc.usercourse_status = 'assigned' and uc.type = 'TCU'  and uc.client_unit_id = '" + unitid_mst + "'   order by uc.ent_date DESC";

            DataTable dt = sgen.getdata(userCode, mq);

            if (dt.Rows.Count > 0)
            {
                model = new List<Tmodelmain>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Tmodelmain tmm = new Tmodelmain();
                    tmm.Col9 = tm.Col9;
                    tmm.Col10 = tm.Col10;
                    tmm.Col11 = tm.Col11;
                    tmm.Col12 = tm.Col12;
                    tmm.Col14 = tm.Col14;
                    tmm.Col15 = tm.Col15;
                    tmm.Col17 = dt.Rows[i]["usercourse_id"].ToString();
                    tmm.Col18 = dt.Rows[i]["user_id"].ToString();
                    tmm.Col19 = dt.Rows[i]["mod_name"].ToString();
                    tmm.Col20 = dt.Rows[i]["cat_name"].ToString();
                    tmm.Col21 = dt.Rows[i]["cou_title"].ToString();
                    tmm.Col22 = dt.Rows[i]["group_name"].ToString();
                    tmm.Col23 = dt.Rows[i]["start_date"].ToString();
                    tmm.Col24 = dt.Rows[i]["end_date"].ToString();
                    tmm.Col25 = dt.Rows[i]["trn_duration"].ToString();
                    tmm.Col26 = dt.Rows[i]["trn_mandatory"].ToString();
                    tmm.Col27 = dt.Rows[i]["training_type"].ToString();
                    tmm.Col28 = dt.Rows[i]["training_status"].ToString();
                    tmm.Col29 = dt.Rows[i]["user_presense"].ToString();
                    model.Add(tmm);

                }

            }

            return View(model);

        }

        [HttpPost]
        public ActionResult my_trn(List<Tmodelmain> model, string command, string mytxt, string mytxt1)
        {
            bool result = false;
            FillMst();
            var tm = model.FirstOrDefault();
            if (command.Trim() == "Save" || command.Trim() == "Update")
            {




            }



            ModelState.Clear();

            return View(model);
        }
        #endregion

        #region filesave
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
        #endregion
    }

}
