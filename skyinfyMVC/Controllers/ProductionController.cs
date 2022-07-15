using skyinfyMVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
namespace skyinfyMVC.Controllers
{
    public class ProductionController : Controller  //Production
    {
        public string mq = "", mq1 = "", vch_num = "", btnval = "", tab_name = "", type = "", type_desc = "", tab_name1 = "", ent_date = "", status = "", defval = "",
     cond = "", mid_mst = "", where = "", cmd = "", id = "", inactive_date = "", Master_Type = "";
        #region 18 jan
        Random random = new Random();
        public static string name = "", ctype1 = "", ctype2 = "", ctype3 = "", ctype4 = "", ctype5 = "", ctype6 = "", ctype7 = "",
        finalpath1 = "", finalpath2 = "", finalpath3 = "", finalpath4 = "", finalpath5 = "", finalpath6 = "", finalpath7 = "", path1 = "", path2 = "", path3 = "", path4 = "",
        path5 = "", path6 = "", path7 = "", fileName1 = "", fileName2 = "", fileName3 = "", fileName4 = "", fileName5 = "", fileName6 = "", fileName7 = "", encpath1 = "",
        encpath2 = "", encpath3 = "", encpath4 = "", encpath5 = "", encpath6 = "", encpath7 = "", year_to ="", year_from="";
        public static string isscholarship = "N", istransfer = "N", issibling = "N", isfamily = "N", ishobby = "N", havetransport = "",
            isperadd = "N", isdisable = "N", sameas = "", gender = "", isemailauth = "", isphoneauth = "", sa_id = "", mq2 = "", mq3 = "";
        #endregion
        static string MyGuid = "";
        sgenFun sgen;
        Cmd_Fun cmd_Fun;
        public static bool isedit = false, res = false, ok = false;
        public static string userCode, userid_mst, Ac_Year_id, cg_com_name, clientid_mst, unitid_mst, actionName = "", controllerName = "", xprdrange = "", role_mst = "";
        #region main func
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
            role_mst = sgen.GetCookie(MyGuid, "role_mst");
            Ac_Year_id = sgen.GetCookie(MyGuid, "Ac_Year_id");
            cg_com_name = sgen.GetCookie(MyGuid, "cg_com_name");
            clientid_mst = sgen.GetCookie(MyGuid, "clientid_mst");
            unitid_mst = sgen.GetCookie(MyGuid, "unitid_mst");
            year_to = sgen.GetCookie(MyGuid, "year_to");
            year_from = sgen.GetCookie(MyGuid, "year_from");
            actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            sgen.SetSession(MyGuid, "viewName", actionName);
            sgen.SetSession(MyGuid, "controllerName", controllerName);
            sgen.SetSession(MyGuid, "BackToBack", "");
            xprdrange = "and to_date(to_char(vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') " +
               "and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "')";
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
        public List<Tmodelmain> getload(List<Tmodelmain> model)
        {
            chkRef();
            Tmodelmain tm1 = new Tmodelmain();
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            if (MyGuid == "") MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
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
        public void chkRef()
        {
            if (userCode.Equals("")) Response.Redirect(sgenFun.callbackurl);
            if (Request.UrlReferrer == null) { Response.Redirect(sgenFun.callbackurl); }
        }
        #endregion
        //=====================make query===========
        #region make query
        public void Make_query(string viewname, string title, string btnval, string seektype, string param1, string param2, string Myguid)
        {
            FillMst(Myguid);
            sgen.SetSession(MyGuid, "btnval", btnval);
            switch (viewname.ToLower())
            {
                #region machine_master
                case "machine_master":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            cmd = @"select k.client_id||k.client_unit_id||k.vch_num||to_char(k.vch_date,'yyyymmdd')||k.type as fstr
                                   ,k.vch_num Doc_No, k.col31 as Machine_Name,k.col2 as Machine_model,k.col9 as Machine_loc from kc_tab k where k.client_unit_id='" + unitid_mst + "' and (k.type='PMM' or K.type='DDPMM')";
                            break;
                        case "VIEW":
                            cmd = @"select k.client_id||k.client_unit_id||k.vch_num||to_char(k.vch_date,'yyyymmdd')||k.type as fstr
                                   ,k.vch_num Doc_No, k.col31 as Machine_Name,k.col1 as Srno,k.col32 as Manufact_name,k.col2 as Machine_model,k.col9 as Machine_loc,
                                   nvl(c.master_name,'0') Machine_cap,k.col4 as Average_spm,to_char(convert_tz(k.date1, 'UTC', '" + sgen.Gettimezone() + "'), " +
                                   "'" + sgen.Getsqldateformat() + "') as Purchase_Date,to_char(convert_tz(k.date2, 'UTC', '" + sgen.Gettimezone() + "'), " +
                                   "'" + sgen.Getsqldateformat() + "') as Date2,k.col8 as frequency,k.col6 as no_of_strock,k.col7 as status from kc_tab k " +
                                   "left join master_setting c on c.master_id=k.col14 and c.type='K02' and " +
                                   "find_in_set(c.client_unit_id, k.client_unit_id) = 1 " +
                                   " where k.client_unit_id='" + unitid_mst + "' and (k.type='PMM' or K.type='DDPMM')";
                            break;
                    }
                    break;
                #endregion
                #region mould_master
                case "mould_master":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            cmd = "select (k.client_id||k.client_unit_id||k.vch_num||to_char(k.vch_date,'yyyymmdd')||k.type) fstr,k.vch_num DocNo," +
                                "k.col31 Mould_Name,k.col1 Mould_no,k.col32 Part_name, k.col3 Status " +
                                "from kc_tab k " +
                                "where k.client_unit_id = '" + unitid_mst + "' and (k.type = 'MOS' or k.type = 'DDMOS') " +
                                "group by(k.client_id|| k.client_unit_id || k.vch_num || to_char(k.vch_date, 'yyyymmdd') || k.type),k.vch_num,k.col31," +
                                "k.col1,k.col32,k.col3 order by k.vch_num asc";
                            break;
                        case "VIEW":
                            //cmd = @"select k.client_id||k.client_unit_id||k.vch_num||to_char(k.vch_date,'yyyymmdd')||k.type as fstr,
                            //      k.col31 as Mould_Name,k.col1 as Mould_no,k.col32 as Party_name,k.col2 as Part_No,k.col33 as Mould_cavity,
                            //     to_char(convert_tz(k.date1, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Date," +
                            //     " k.col5 as frequency,k.col6 as no_of_strock,k.col3 as Status from kc_tab k where (k.type = 'MOS' || K.type='DDMOS') " +
                            //     "and k.client_id = '" + clientid_mst + "' and k.client_unit_id = '" + unitid_mst + "'";
                            //cmd = "select (k.client_id||k.client_unit_id||k.vch_num||to_char(k.vch_date,'yyyymmdd')||k.type) as fstr," +
                            //    "k.col31 as Mould_Name,k.col1 as Mould_no ,k.col32 as Part_name,k.col2 as Part_No,k.col33 as Mould_cavity," +
                            //    "to_char(convert_tz(k.date1, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as mould_Date," +
                            //    "k.col3 as Status, k.col7 as frequency,k.col6 as no_of_strock,nvl(c.master_name,'0') mould_caps from kc_tab k " +
                            //   "left join master_setting c on c.master_id=k.col14 and c.type='K02' and find_in_set(c.client_id, k.client_id)= 1 and " +
                            //       "find_in_set(c.client_unit_id, k.client_unit_id) = 1 " +
                            //    "where (k.type = 'MOS' or k.type = 'DDMOS') and k.client_id = '" + clientid_mst + "' and k.client_unit_id = '" + unitid_mst + "'";
                            cmd = "select (k.client_id||k.client_unit_id||k.vch_num||to_char(k.vch_date,'yyyymmdd')||k.type) fstr,k.vch_num DocNo," +
                                "k.col31 Mould_Name,k.col1 Mould_no,k.col32 Part_name,k.col2 Part_No, k.col33 Mould_cavity," +
                                "to_char(convert_tz(k.date1, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') mould_Date,k.col3 Status," +
                                " k.col7 frequency,k.col6 no_of_strock, nvl(c.master_name, '0') mould_caps,nvl(group_concat(m.master_name), '0') machine_compatibility " +
                                "from kc_tab k " +
                                "left join master_setting c on c.master_id = k.col14 and c.type = 'K02' and find_in_set(c.client_unit_id, k.client_unit_id) = 1 " +
                                "left join master_setting m on find_in_set(m.master_id, k.col34)= 1 and m.type = 'K02' and find_in_set(m.client_unit_id, k.client_unit_id) = 1 " +
                                "where k.client_unit_id = '" + unitid_mst + "' and (k.type = 'MOS' or k.type = 'DDMOS') " +
                                "group by(k.client_id|| k.client_unit_id || k.vch_num || to_char(k.vch_date, 'yyyymmdd') || k.type),k.vch_num,k.col31," +
                                "k.col1,k.col32,k.col2,k.col33,to_char(convert_tz(k.date1, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "')," +
                                "k.col3,k.col7,k.col6,nvl(c.master_name, '0') order by k.vch_num asc";
                            break;
                    }
                    break;
                #endregion
                #region prod_entry
                case "prod_entry":
                    switch (btnval.ToUpper())
                    {
                        case "VIEW":
                            cmd = "select client_id||client_unit_id||vch_num||to_char(vch_date,'yyyymmdd')||type fstr,max(fg) as fg,max(fgname) fgname,sum(pqty) as pqty,max(sf) as sf," +
                               "max(sfname) sfname,sum(cqty) cqty,max(tstg) ToStage,max(fstg) FromStage,opname Operator,mcname Machine,mouldname Mould,stime StartTime,etime EndTime,deptname Department," +
                               "shftname ShiftName FROM (" +
                               "SELECT client_id,client_unit_id,vch_num,vch_date,type,icode as fg,iname fgname,qtyin as pqty,'-' as sf,'-' sfname,to_number('0') as cqty, stage tstg,'-' fstg,rno,opname,mcname," +
                               "mouldname,stime,etime,deptname,shftname,pflag FROM iprod where type = '86' and pflag = '1' and client_unit_id = '" + unitid_mst + "' " +
                               "union " +
                               "SELECT client_id,client_unit_id,vch_num,vch_date,type,'-' as fg,'-' fgname,to_number('0') as pqty,icode as sf,iname sfname,qtyout as pqty,'-' tstg,stage fstg,rno,opname,mcname," +
                               "mouldname,stime,etime,deptname,shftname,pflag FROM iprod where client_unit_id = '" + unitid_mst + "' and type = '86' and pflag = '0') a " +
                               " group by rno,(client_id||client_unit_id||vch_num||to_char(vch_date,'yyyymmdd')||type),opname ,mcname ,mouldname ,stime ,etime ,deptname ,shftname ";
                            break;
                        case "EDIT":
                            cmd = "select client_id||client_unit_id||vch_num||to_char(vch_date,'yyyymmdd')||type fstr,max(fg) as fg,max(fgname) fgname,sum(pqty) as pqty,max(sf) as sf," +
                                "max(sfname) sfname,sum(cqty) cqty,max(tstg) ToStage,max(fstg) FromStage,mcname Machine,mouldname Mould,                                                                                                                     deptname Department," +
                                "shftname ShiftName FROM (" +
                                "SELECT client_id,client_unit_id,vch_num,vch_date,type,icode as fg,iname fgname,qtyin as pqty,'-' as sf,'-' sfname,to_number('0') as cqty, stage tstg,'-' fstg,rno,mcname," +
                                "mouldname,deptname,shftname,pflag FROM iprod where type = '86' and pflag = '1' and client_unit_id = '" + unitid_mst + "' " +
                                "union " +
                                "SELECT client_id,client_unit_id,vch_num,vch_date,type,'-' as fg,'-' fgname,to_number('0') as pqty,icode as sf,iname sfname,qtyout as pqty,'-' tstg,stage fstg,rno,mcname," +
                                "mouldname,deptname,shftname,pflag FROM iprod where client_unit_id = '" + unitid_mst + "' and type = '86' and pflag = '0') a " +
                                " group by rno,(client_id||client_unit_id||vch_num||to_char(vch_date,'yyyymmdd')||type),opname ,mcname ,mouldname ,stime ,etime ,deptname ,shftname ";
                            break;
                        case "ITEMFROM":
                        case "ITEM":
                            if (param1 != "") where = " and it.icode not in (" + param1.Trim().TrimEnd(',') + ")";
                            cmd = "select it.icode||to_char(it.vch_date,'yyyymmdd')||it.type fstr,it.icode as Icode,it.iname as Iname,it.cpartno Partno," +
                                "um.master_name as UOM,hsn.master_name as hsn,hsn.group_name taxrate from item it " +
                                "inner join master_setting um on um.master_id=it.uom and um.type='UMM' and find_in_set(um.client_unit_id,it.client_unit_id)=1 " +
                                "inner join master_setting hsn on hsn.master_id=it.hsno and hsn.type='HSN' and find_in_set(hsn.client_unit_id,it.client_unit_id)=1 " +
                                "where find_in_set(it.client_unit_id,'" + unitid_mst + "')=1 and it.type='IT' " + where + "";
                            break;
                    }
                    break;
                #endregion
                #region operator_master
                case "operator_master":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            cmd = @"select (ms.client_id||ms.client_unit_id||ms.master_id||to_char(ms.vch_date,'yyyymmdd')||ms.type) fstr,ms.vch_num doc_no," +
                              "ms.master_name as operator_name from master_setting ms " +
                              "where ms.client_unit_id = '" + unitid_mst + "' and ms.type in ('OPR','DDOPR')";
                            break;
                        case "VIEW":
                            cmd = @"select (ms.client_id||ms.client_unit_id||ms.master_id||to_char(ms.vch_date,'yyyymmdd')||ms.type) fstr,ms.vch_num doc_no," +
                                 "ms.master_name as operator_name,dep.master_name as dept,des.master_name as des,(CASE when ms.status = 'Y' THEN 'Active' " +
                                 "when ms.status = '' THEN 'Active' else 'Inactive' end) Status,b.First_name|| ' '|| nvl(b.middle_name, '')" +
                                 "||' '|| b.last_name as Ent_By,to_char(convert_tz(ms.master_EntDate, 'UTC', '" + sgen.Gettimezone() + "'), '" +
                                 sgen.Getsqldateformat() + "') as Ent_Date from master_setting ms " +
                                 "left join user_details b on ms.master_entby = b.vch_num and b.type = 'CPR' " +
                                 "inner join master_setting dep on dep.master_id = ms.classid and dep.type = 'MDP' and find_in_set(dep.client_unit_id,ms.client_unit_id)=1 " +
                                 "inner join master_setting des on des.master_id = ms.sectionid and des.type = 'MDG' and find_in_set(des.client_unit_id,ms.client_unit_id)=1 " +
                                 "where ms.client_unit_id = '" + unitid_mst + "' and ms.type in ('OPR','DDOPR')";
                            break;
                    }
                    break;
                #endregion
                #region item_stage
                case "item_stage":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = @"select ex.client_id||ex.client_unit_id||ex.vch_num||to_char(ex.vch_date,'yyyymmdd')||ex.type as fstr
                                ,ex.col1 as item_code ,ex.col3 as order_No,ex.col4 as Item_Name,ms.client_name as stage from enx_tab2 ex
                                inner Join master_setting ms on ms.master_id = ex.col2 and ms.type = 'KPS'  ";
                            break;
                        case "ITEM_NAME":
                            cmd = @"select client_id,icode||client_unit_id|| to_char(vch_date, 'yyyymmdd')|| type as fstr ,icode as item_code,iname as item_name from item 
                            where find_in_set(client_unit_id , '" + unitid_mst + "')=1 and type = 'IT'";
                            break;
                    }
                    break;
                #endregion
                #region shift_master
                case "shift_master":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            cmd = @"select ms.client_id||ms.client_unit_id||ms.master_id||to_char(ms.vch_date,'yyyymmdd'),ms.type as fstr,
                                  ms.master_id as doc_no,ms.master_name as shift_name,ms.classid as shift_type from master_setting ms " +
                                " where ms.client_unit_id = '" + unitid_mst + "' and (ms.type = 'STM' || ms.type = 'DDSTM')";
                            break;
                        case "VIEW":
                            cmd = @"select ms.client_id||ms.client_unit_id||ms.master_id||to_char(ms.vch_date,'yyyymmdd'),ms.type as fstr,
                                  ms.master_id as doc_no,ms.master_name as shift_name,ms.classid as shift_type,(CASE when ms.status = 'Y' THEN 'Active' 
                                  when ms.status = '' THEN 'Active' else 'Inactive' end) as Status,
                                 b.First_name|| ' '|| nvl(b.middle_name, '')|| ' '|| b.last_name as Ent_By,
                                 to_char(convert_tz(ms.master_EntDate, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Ent_Date" +
                                 " from master_setting ms inner join user_details b on ms.master_entby = b.vch_num and b.type = 'CPR'" +
                                 " where ms.client_unit_id = '" + unitid_mst + "' and (ms.type = 'STM' || ms.type = 'DDSTM')";
                            break;
                    }
                    break;
                #endregion
                #region machine_brkdwn
                case "machine_brkdwn":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            cmd = "select  c.client_id||c.client_unit_id||c.vch_num||to_char(c.vch_date,'yyyymmdd')||c.type as fstr,c.cou_title as Machine_name," +
                                "c.cou_descp as location,ms.master_name as reason from courses c " +
                                "inner join master_setting ms on ms.master_id = c.cou_payment and ms.type = 'BRM' and ms.client_unit_id = '" + unitid_mst + "' " +
                                "where c.client_unit_id = '" + unitid_mst + "' and c.type = 'MBD' and to_date(to_char(c.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "')";
                            break;
                        case "VIEW":
                            //cmd = @"select c.client_id||c.client_unit_id||c.vch_num||to_char(c.vch_date,'yyyymmdd')||c.type as fstr,c.cou_title as Machine_name,
                            //     c.cou_descp as location,c.cou_payment as reason,to_char(convert_tz(c.occ_date, 'UTC', '" + sgen.Gettimezone() + "'), " +
                            //     "'" + sgen.Getsqldateformat() + "') as Datetime,b.First_name|| ' '|| nvl(b.middle_name, '')|| ' '|| b.last_name as Ent_By," +
                            //     "to_char(convert_tz(C.COU_ENT_DATE, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Ent_Date" +
                            //     " from courses c inner join user_details b on C.COU_ENT_BY = lpad(b.rec_id, 6, 0) and b.type = 'CPR' " +
                            //     "where(c.type = 'MBD' || c.type = 'DDMBD') and c.client_id = '" + clientid_mst + "' and c.client_unit_id = '" + unitid_mst + "'";
                            cmd = "select  c.client_id||c.client_unit_id||c.vch_num||to_char(c.vch_date,'yyyymmdd')||c.type as fstr,c.cou_title as Machine_name," +
                                "c.cou_descp as location,ms.master_name as reason,to_char(convert_tz(c.occ_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Datetime," +
                                "to_char(convert_tz(C.COU_ENT_DATE, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Ent_Date,b.First_name || ' ' || nvl(b.middle_name, '') || ' ' || b.last_name as Ent_By" +
                                " from courses c inner join master_setting ms on ms.master_id = c.cou_payment and ms.type = 'BRM' and ms.client_unit_id = '" + unitid_mst + "' " +
                                "inner join user_details b on c.COU_ENT_BY = b.vch_num and b.type = 'CPR' " +
                                "where c.client_unit_id = '" + unitid_mst + "' and c.type = 'MBD' and to_date(to_char(c.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "')";
                            break;
                    }
                    break;
                #endregion
                #region bom
                case "bom":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "PRINT":
                            cmd = "select distinct (ex.client_id||ex.client_unit_id||ex.vch_num||to_char(ex.vch_date,'yyyymmdd')||ex.type) fstr,ex.vch_num DocNo," +
                               "ex.acode MainItem,it.iname MainItemName, um.master_name Uom,ex.irate Bom_Lot_Size " +
                               "from itransactionc ex " +
                               "inner join item it on ex.acode = it.icode and it.type = 'IT' and find_in_set(it.client_unit_id, ex.client_unit_id)=1 " +
                               "inner join master_setting um on um.master_id = it.uom and um.type = 'UMM' and find_in_set(um.client_unit_id, it.client_unit_id)= 1 " +
                               "where ex.type = 'BOM' and ex.client_unit_id = '" + unitid_mst + "' and to_date(to_char(ex.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "')";
                            break;

                        case "VIEW":
                        case "COPYBM":
                       
                            cmd = "select distinct (ex.client_id||ex.client_unit_id||ex.vch_num||to_char(ex.vch_date,'yyyymmdd')||ex.type) fstr,ex.vch_num DocNo," +
                                "to_char(convert_tz(ex.vch_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') Bom_Date,ex.acode MainItem,it.iname MainItemName," +
                                "um.master_name Uom,ex.irate Bom_Lot_Size from itransactionc ex " +
                                "inner join item it on ex.acode = it.icode and it.type = 'IT' and find_in_set(it.client_unit_id, ex.client_unit_id)=1 " +
                                "inner join master_setting um on um.master_id = it.uom and um.type = 'UMM' and find_in_set(um.client_unit_id, it.client_unit_id)= 1 " +
                                "where ex.type = 'BOM' and ex.client_unit_id = '" + unitid_mst + "' and to_date(to_char(ex.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "')";
                            break;
                        case "BOMITEM":
                            where = "";
                            //where = " and (it.client_id||it.client_unit_id||it.icode)<>'" + param2 + "'";
                            cmd = "select (it.client_id||it.client_unit_id||it.icode) fstr,it.icode as Icode,it.iname as Iname,it.cpartno Partno," +
                               "pum.master_name as UOM,mg.master_name MainGrp,gp.master_name ItemGrp from item it " +
                               "inner join master_setting pum on pum.master_id=it.uom and pum.type='UMM' and find_in_set(pum.client_unit_id,'" + unitid_mst + "')=1 " +
                               "inner join master_setting mg on mg.classid=substr(it.icode,1,1) and mg.type='KGP' " +
                               "inner join master_setting gp on gp.master_id=substr(it.icode,1,3) and gp.type='KIG' and find_in_set(gp.client_unit_id,'" + unitid_mst + "')=1 " +
                               "inner join item sg on sg.icode=substr(it.icode,1,5) and sg.type='SG' and find_in_set(sg.client_unit_id,'" + unitid_mst + "')=1 " +
                               "where find_in_set(it.client_unit_id,'" + unitid_mst + "')=1 and it.type='IT' " + where + "";
                            break;
                        case "ITEM":
                            cmd = "select (it.client_id||it.client_unit_id||it.icode) fstr,it.icode as Icode,it.iname as Iname,it.cpartno Partno,pum.master_name as UOM," +
                                "mg.master_name MainGrp, gp.master_name ItemGrp from item it " +
                                "inner join master_setting pum on pum.master_id = it.uom and pum.type = 'UMM' and find_in_set(pum.client_unit_id,'" + unitid_mst + "')=1 " +
                                "inner join master_setting mg on mg.classid = substr(it.icode, 1, 1) and mg.type = 'KGP' " +
                                "inner join master_setting gp on gp.master_id = substr(it.icode, 1, 3) and gp.type = 'KIG' and find_in_set(gp.client_unit_id,'" + unitid_mst + "')=1 " +
                                "inner join item sg on sg.icode = substr(it.icode, 1, 5) and sg.type = 'SG' and find_in_set(sg.client_unit_id,'" + unitid_mst + "')=1" +
                                " where find_in_set(it.client_unit_id ,'" + unitid_mst + "')=1 and it.type = 'IT' and substr(trim(it.icode),1,1) in ('8', '9') and it.icode not in (select distinct acode from itransactionc where client_unit_id = '" + unitid_mst + "' and type = 'BOM')";
                            break;
                    }
                    break;
                #endregion
                #region p order
                case "p_ord":
                    switch (btnval.ToUpper())
                    {
                        case "NEW":
                            //cmd = "select '70' as ID ,'WITH SALES ORDER' as PRODUCTION_OREDER from dual union all select '71' as ID ," +
                            //    "'WITHOUT SALES ORDER' as Invoice_Against from dual";
                            cmd = "select (master_id||to_char(vch_date,'yyyymmdd')||type) fstr,master_id as ID,master_name as PD_Type from master_setting where find_in_set(client_unit_id,'" + unitid_mst + "')='1' and type='PTP' and substr(master_id,1,1)='7'";
                            break;
                        case "PRINT":
                        case "EDIT":
                            cmd = "select (sd.client_id||sd.client_unit_id||sd.vch_num||to_char(sd.vch_date,'yyyymmdd')||sd.type) as fstr,sd.vch_num as Doc_no ," +
                                "to_char(sd.vch_date, '" + sgen.Getsqldateformat() + "') as Doc_date,sd.col4 as So_no,(case when sd.col20 = '70' then 'WITH SALES ORDER' " +
                                "when sd.col20 = '71' then 'WITHOUT SALES ORDER' else '-' end) as so_type,sd.col5 as Icode,sd.col27 as IName, sd.col8 as UOM,sd.col21 as Remark," +
                                "sd.col9 as Order_qty,sd.col12 as remark2,sd.col13 as So_no2 " +
                                "from kc_tab sd where sd.client_unit_id = '" + unitid_mst + "' and sd.type = 'PSO' and to_date(to_char(sd.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "')";
                            break;
                        case "VIEW":
                       
                            //cmd = " select (sd.client_id||sd.client_unit_id||sd.vch_num||to_char(sd.vch_date,'yyyymmdd')||sd.type) as fstr, sd.vch_num as Doc_no ," +
                            //    "to_char(sd.vch_date,'" + sgen.Getsqldateformat() + "') as Doc_date,sd.col2 as Sdl_no,sd.col4 as So_no,sd.col21 as Remark," +
                            //    "to_char(sd.date2,'" + sgen.Getsqldateformat() + "') as ord_st_date," +
                            //    "to_char(sd.date3,'" + sgen.Getsqldateformat() + "') as ord_end_date," +
                            //    "to_char(sd.date5,'" + sgen.Getsqldateformat() + "') as So_dt1," +
                            //    "to_char(sd.date6,'" + sgen.Getsqldateformat() + "') as So_dt2,sd.col5 as Icode,sd.col27 as IName," +
                            //    "sd.col7 as PartNo, sd.col8 as UOM, sd.col9 as Order_qty,sd.col10 as Plan_qty,sd.col11 as PO_No_grd,sd.col12 as remark2,sd.col13 as So_no2 from kc_tab" +
                            //    " sd where sd.type = 'PSO' and sd.client_id = '" + clientid_mst + "' and sd.client_unit_id = '" + unitid_mst + "'";
                            
                            
                         
                            cmd = "select (sd.client_id||sd.client_unit_id||sd.vch_num||to_char(sd.vch_date,'yyyymmdd')||sd.type) as fstr,sd.vch_num as Doc_no ," +
                                "to_char(sd.vch_date, '" + sgen.Getsqldateformat() + "') as Doc_date,sd.col4 as So_no,(case when sd.col20 = '70' then 'WITH SALES ORDER' " +
                                "when sd.col20 = '71' then 'WITHOUT SALES ORDER' else '-' end) as so_type,sd.col5 as Icode,sd.col27 as IName,sd.col7 as PartNo, sd.col8 as UOM,sd.col21 as Remark," +
                                "to_char(sd.date2, '" + sgen.Getsqldateformat() + "') as ord_st_date,to_char(sd.date3, '" + sgen.Getsqldateformat() + "') as ord_end_date,to_char(sd.date5, '" + sgen.Getsqldateformat() + "') as So_dt1," +
                                "to_char(sd.date6, '" + sgen.Getsqldateformat() + "') as So_dt2,sd.col9 as Order_qty,sd.col10 as Plan_qty,sd.col12 as remark2,sd.col13 as So_no2 " +
                                "from kc_tab sd where sd.client_unit_id = '" + unitid_mst + "' and sd.type = 'PSO' and to_date(to_char(sd.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "')";
                            break;
                        case "GETSO":
                            //cmd = "select distinct (a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr," +
                            //    "(case when a.type = '40' then 'Domestic Invoice' when a.type = '41' then 'Export Invoice' when a.type = '42'" +
                            //    " then 'FA Sale' when a.type = '43' then 'Service Invoice' when a.type = '44' then 'Purchase Return' end)" +
                            //    " as so_type,a.vch_num as so_no,to_char(a.vch_date,'" + sgen.Getsqldateformat() + "') as so_date  from purchasesc a where a.client_id = '" + clientid_mst + "' and " +
                            //    "a.client_unit_id = '" + unitid_mst + "' and substr(a.type,1,1)= '4' and a.pur_type = 'NPI'";

                            cmd = "select distinct (a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr,tp.master_name as So_Type,a.vch_num as so_no,to_char(a.vch_date,'" + sgen.Getsqldateformat() + "') as so_date from purchasesc a " +
                                "inner join master_setting tp on tp.master_id = a.type and tp.type = 'KPO' and substr(master_id,1,1)= '4' and find_in_set(tp.client_unit_id,'" + unitid_mst + "')= 1 where a.client_unit_id = '" + unitid_mst + "' and substr(a.type,1,1)= '4'" +
                                " and a.pur_type = 'NPI'";
                            break;
                        case "ITEM":
                            if (param1 != null)
                            {
                                if (param1 != "") where = " and it.icode not in ('" + param1.Trim().TrimEnd(',').Replace(",", "','").Trim().ToString() + "')";
                            }
                            if (sgen.GetSession(MyGuid, "PO_TYPE").ToString().Equals("70"))
                            {
                                string condi = " and pc.vch_num in ('" + sgen.GetSession(MyGuid, "So_num").ToString().Trim().TrimEnd(',').Replace(",", "','").Trim().ToString() + "')";
                                cmd = " select distinct (it.icode||pc.type || pc.vch_num ||to_char(it.vch_date,'yyyymmdd')||it.type) as fstr,it.icode as Icode,it.iname as  Iname," +
                            "it.cpartno Partno, pc.hsno as HSN,pc.uom as UOM,to_char(pc.dlv_date,'" + sgen.Getsqldateformat() + "') as dlv_date,to_char(pc.vch_date,'" + sgen.Getsqldateformat() + "') as so_date," +
                            "pc.vch_num as so_no from item it " +
                            "inner join purchasesc pc on pc.icode = it.icode and pc.pur_type = 'NPI' inner join clients_mst cl on cl.vch_num = pc.acode and cl.type='BCD' and substr(cl.vch_num,0,3)='303' and find_in_set(cl.client_unit_id,pc.client_unit_id)=1 where find_in_set(it.client_unit_id, '" + unitid_mst + "')=1 and it.type = 'IT' " + condi + " and" +
                            " it.sale='Y' " + where + "";
                            }
                            else
                            {
                                cmd = "select it.icode||to_char(it.vch_date,'yyyymmdd')||it.type as fstr,it.icode as Icode,it.iname as Iname,it.cpartno Partno, um.master_name as UOM,hsn.master_name as hsn,hsn.group_name taxrate, nvl(s.closing, '0') as qty_in_stock from item it left join master_setting um on um.master_id = it.uom and um.type = 'UMM' and find_in_set(um.client_unit_id, it.client_unit_id)= 1 left join master_setting hsn on hsn.master_id = it.hsno and hsn.type = 'HSN' and find_in_set(hsn.client_unit_id, it.client_unit_id)= 1 left join(select t.client_unit_id, t.icode, sum(nvl(t.qtyin, 0)) as Received,sum(nvl(t.qtyout, 0)) as Issued,sum(nvl(t.qtyin, 0)) - sum(nvl(t.qtyout, 0)) as closing from itransaction t where trim(t.store) = 'Y' group by t.icode,t.client_unit_id ) s on it.icode = s.icode and find_in_set(it.client_unit_id, s.client_unit_id)=1 where find_in_set(it.client_unit_id, '" + unitid_mst + "')=1 and it.type = 'IT' and it.sale = 'Y' and substr(it.icode,1,1)= '9' " + where + "";
                            }
                            break;
                    }
                    break;
                #endregion
                #region mrp_gen
                case "mrp_gen":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = "select (ex.client_id||ex.client_unit_id||ex.vch_num||to_char(ex.vch_date,'yyyymmdd')||ex.type) fstr,ex.acode MainItem,ex.icode as Icode,ex.iname as IName," +
                                "ex.uom as Primary_Unit,ex.cpartno as Secondary_Unit,ex.hsno as Conversion_Unit,ex.qtychl as Qty_Req,ex.qtyin as Qty_Rej from itransaction ex " +
                                "where ex.client_unit_id = '" + unitid_mst + "' and ex.type ='BOM' and to_date(to_char(ex.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "')";
                            break;
                        case "ITEM":
                            cmd = "select bom.acode,mg.master_name MainGrp,gp.master_name ItemGrp,it.iname as Iname,it.cpartno Partno,pum.master_name as Primary_Unit," +
                                "suom.master_name as Secondary_Unit,it.cuom Conversion_Unit,hsn.master_name as hsn,hsn.group_name taxrate from itransaction bom inner join item it on " +
                                "it.icode=bom.acode and it.type='IT' and find_in_set(it.client_unit_id,bom.client_unit_id)=1 " +
                                "inner join itransaction bom on bom.acode=it.icode and find_in_set(bom.client_unit_id,it.client_unit_id)=1 " +
                                "inner join master_setting pum on pum.master_id=it.uom and pum.type='UMM' and find_in_set(pum.client_unit_id,it.client_unit_id)=1 " +
                                "inner join master_setting suom on suom.master_id=it.uom and suom.type='UMM' and find_in_set(suom.client_unit_id,it.client_unit_id)=1 " +
                                "inner join master_setting hsn on hsn.master_id=it.hsno and hsn.type='HSN' and find_in_set(hsn.client_unit_id,it.client_unit_id)=1 " +
                                "inner join master_setting mg on mg.classid=substr(bom.acode,1,1) and mg.type='KGP'" +
                                "inner join master_setting gp on gp.master_id=substr(bom.acode,1,2) and gp.type='KIG' and find_in_set(gp.client_unit_id,bom.client_unit_id)=1 " +
                                "inner join item sg on sg.icode=substr(bom.acode,1,4) and sg.type='SG' and find_in_set(sg.client_unit_id,'" + unitid_mst + "')=1" +
                                "where client_unit_id='" + unitid_mst + "' and type='BOM'";
                            // cmd = "select (it.client_id||it.client_unit_id||it.icode||to_char(it.vch_date,'yyyymmdd')||it.type) fstr,mg.master_name MainGrp,gp.master_name ItemGrp," +
                            //"it.icode as Icode,it.iname as Iname,it.cpartno Partno,pum.master_name as Primary_Unit,suom.master_name as Secondary_Unit,it.cuom Conversion_Unit," +
                            //"hsn.master_name as hsn,hsn.group_name taxrate from item it " +
                            //"inner join master_setting pum on pum.master_id=it.uom and pum.type='UMM' and pum.client_id='" + clientid_mst + "' and pum.client_unit_id='" + unitid_mst + "' " +
                            //"inner join master_setting suom on suom.master_id=it.uom and suom.type='UMM' and suom.client_id='" + clientid_mst + "' and suom.client_unit_id='" + unitid_mst + "' " +
                            //"inner join master_setting hsn on hsn.master_id=it.hsno and hsn.type='HSN' and hsn.client_id='" + clientid_mst + "' and hsn.client_unit_id='" + unitid_mst + "' " +
                            //"inner join master_setting mg on mg.classid=substr(it.icode,1,1) and mg.type='KGP'" +
                            //"inner join master_setting gp on gp.master_id=substr(it.icode,1,2) and gp.type='KIG' and gp.client_id='" + clientid_mst + "' and gp.client_unit_id='" + unitid_mst + "'" +
                            //"inner join item sg on sg.icode=substr(it.icode,1,4) and sg.type='SG' and sg.client_id='" + clientid_mst + "' and sg.client_unit_id='" + unitid_mst + "'" +
                            //"inner join itransaction bom on bom.acode=it.icode and bom.client_id=it.client_id and bom.client_unit_id=it.client_unit_id "+
                            //"where it.type='IT' and it.client_id='" + clientid_mst + "' and it.client_unit_id='" + unitid_mst + "'" + where + "";
                            break;
                    }
                    break;
                #endregion
                #region iprod
                case "iprod":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "PRINT":
                            cmd = "select distinct (ex.client_id||ex.client_unit_id||ex.vch_num||to_char(ex.vch_date,'yyyymmdd')||ex.type) fstr,ex.vch_num DocNo," +
                                "to_char(convert_tz(ex.vch_date, 'UTC', '" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') Production_Date,ex.acode MainItem," +
                                "it.iname MainItemName,ex.qtyin Produce_Qty from iprod ex " +
                                "inner join item it on it.icode=ex.acode and it.type='IT' and find_in_set(it.client_unit_id,ex.client_unit_id)=1 " +
                             "where ex.client_unit_id = '" + unitid_mst + "' and ex.type ='100' and to_date(to_char(ex.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "') order by ex.vch_num desc";
                            break;
                        case "VIEW":
                            cmd = "select distinct (ex.client_id||ex.client_unit_id||ex.vch_num||to_char(ex.vch_date,'yyyymmdd')||ex.type) fstr,ex.vch_num DocNo," +
                           "to_char(convert_tz(ex.vch_date, 'UTC', '" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') Production_Date,ex.acode MainItem," +
                           "it.iname MainItemName,um.master_name Uom,stf.master_name From_Stage,stt.master_name To_Stage,ex.qtyin Produce_Qty,'-' Reference from iprod ex " +
                           "inner join item it on it.icode=ex.acode and it.type='IT' and find_in_set(it.client_unit_id,ex.client_unit_id)=1 " +
                           "inner join master_setting um on um.master_id=it.uom and um.type='UMM' and find_in_set(um.client_unit_id,it.client_unit_id)=1 " +
                           "inner join master_setting stf on stf.master_id=ex.stage and stf.type='KPS' " +
                           "inner join master_setting stt on stt.master_id=ex.mstage and stt.type='KPS' " +
                           "where ex.client_unit_id = '" + unitid_mst + "' and ex.type ='100' and to_date(to_char(ex.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "') order by ex.vch_num desc";
                            //"union all "+
                            break;
                        case "PDITEM": //pd + click item
                            #region old command
                            //                        cmd = "select (bm.client_id||bm.client_unit_id||bm.icode||to_char(bm.vch_date,'yyyymmdd')||bm.type) fstr,bm.acode MainItem,bm.icode Icode,it.iname Iname," +
                            //"pum.master_name uom,mg.master_name MainGrp, gp.master_name ItemGrp from itransactionc bm " +
                            //"inner join item it on it.icode = bm.icode and it.type = 'IT' and it.client_id = bm.client_id and it.client_unit_id = bm.client_unit_id " +
                            //"inner join master_setting pum on pum.master_id = it.uom and pum.type = 'UMM' and find_in_set(pum.client_id,'" + clientid_mst + "')= 1 and find_in_set(pum.client_unit_id,'" + unitid_mst + "')= 1 " +
                            //"inner join master_setting mg on mg.classid = substr(bm.icode, 1, 1) and mg.type = 'KGP' " +
                            //"inner join master_setting gp on gp.master_id = substr(bm.icode, 1, 3) and gp.type = 'KIG' and gp.client_id = '" + clientid_mst + "' and gp.client_unit_id = '" + unitid_mst + "' " +
                            //"inner join item sg on sg.icode = substr(bm.icode, 1, 5) and sg.type = 'SG' and sg.client_id = '" + clientid_mst + "' and sg.client_unit_id = '" + unitid_mst + "' " +
                            //"where bm.type = 'BOM' and bm.client_id = '" + clientid_mst + "' and bm.client_unit_id = '" + unitid_mst + "' and bm.acode = '" + Regex.Split(param2, "!~!~!~!~!")[0].Trim() + "' and qtychl = '" + Regex.Split(param2, "!~!~!~!~!")[1].Trim() + "'";
                            //cmd = "select (it.client_id||it.client_unit_id||it.icode||to_char(it.vch_date,'yyyymmdd')||it.type) fstr,nvl(bm.acode,'-') MainICode,it.icode Icode," +
                            //    "mg.master_name MainGrp, gp.master_name ItemGrp, it.iname Iname, pum.master_name uom from item it " +
                            //    "left outer join itransactionc bm on bm.icode = it.icode and bm.type = 'BOM' and bm.client_id = it.client_id and bm.client_unit_id = it.client_unit_id and bm.acode = '" + Regex.Split(param2, "!~!~!~!~!")[0].Trim() + "' and bm.pono = '" + Regex.Split(param2, "!~!~!~!~!")[1].Trim() + "' " +
                            //    "inner join master_setting pum on pum.master_id = it.uom and pum.type = 'UMM' and find_in_set(pum.client_id,it.client_id)= 1 and find_in_set(pum.client_unit_id,it.client_unit_id)= 1 " +
                            //    "inner join master_setting mg on mg.classid = substr(it.icode, 1, 1) and mg.type = 'KGP' " +
                            //    "inner join master_setting gp on gp.master_id = substr(it.icode, 1, 3) and gp.type = 'KIG' and gp.client_id = it.client_id and gp.client_unit_id = it.client_unit_id " +
                            //    "inner join item sg on sg.icode = substr(it.icode, 1, 5) and sg.type = 'SG' and sg.client_id = it.client_id and sg.client_unit_id = it.client_unit_id " +
                            //    "where it.type = 'IT' and it.client_id = '" + clientid_mst + "' and it.client_unit_id = '" + unitid_mst + "' and it.icode not in ('" + param1.TrimEnd(',').Replace(",","','").Trim() + "')";
                            //cmd = "select(bt.client_id || bt.client_unit_id || bt.icode || to_char(bt.vch_date, 'yyyymmdd') || bt.type) fstr,nvl(bm.acode, '-') MainICode,bt.icode Icode," +
                            //    "it.iname ItemName, pum.master_name UOM, mg.master_name MainGrp, gp.master_name ItemGrp, sg.iname SubGrp,sum(to_number(bt.qtyin) + to_number(bt.qtyout)) Floor_Stk " +
                            //    "from btchtrans bt " +
                            //    "left join itransactionc bm on bm.icode = bt.icode and bm.type = 'BOM' and bm.client_id = bt.client_id and bm.client_unit_id = bt.client_unit_id and " +
                            //    "bm.acode = '" + Regex.Split(param2, "!~!~!~!~!")[0].Trim() + "' and bm.pono = '" + Regex.Split(param2, "!~!~!~!~!")[1].Trim() + "' " +
                            //    "inner join item it on it.icode = bt.icode and it.type = 'IT' and it.client_id = bt.client_id and it.client_unit_id = bt.client_unit_id " +
                            //    "inner join master_setting pum on pum.master_id = it.uom and pum.type = 'UMM' and find_in_set(pum.client_id, bt.client_id)= 1 and find_in_set(pum.client_unit_id, bt.client_unit_id)= 1 " +
                            //    "inner join master_setting mg on mg.classid = substr(bt.icode, 1, 1) and mg.type = 'KGP' " +
                            //    "inner join master_setting gp on gp.master_id = substr(bt.icode, 1, 3) and gp.type = 'KIG' and gp.client_id = bt.client_id and gp.client_unit_id = bt.client_unit_id " +
                            //    "inner join item sg on sg.icode = substr(bt.icode, 1, 5) and sg.type = 'SG' and sg.client_id = bt.client_id and sg.client_unit_id = bt.client_unit_id " +
                            //    "where bt.type in ('30', '32') and bt.client_id = '" + clientid_mst + "' and bt.client_unit_id = '" + unitid_mst + "' and bt.pono = 'W' and " +
                            //    "bt.icode not in ('" + param1.TrimEnd(',').Replace(",", "','").Trim() + "') " +
                            //    "group by(bt.client_id|| bt.client_unit_id || bt.icode || to_char(bt.vch_date, 'yyyymmdd') || bt.type),nvl(bm.acode, '-'),bt.icode,it.iname,pum.master_name," +
                            //    "mg.master_name,gp.master_name,sg.iname";
                            #endregion
                            string mainic = Regex.Split(param2, "!~!~!~!~!")[0].Trim();
                            string tstg = Regex.Split(param2, "!~!~!~!~!")[1].Trim();
                            string sumqty = Regex.Split(param2, "!~!~!~!~!")[2].Trim();//3
                            string bmlotqty = Regex.Split(param2, "!~!~!~!~!")[3].Trim();
                            string totqty = Regex.Split(param2, "!~!~!~!~!")[4].Trim();//2
                            mq2 = " union all select '" + clientid_mst + "','" + unitid_mst + "','BOM','" + mainic + "','" + tstg + "','" + bmlotqty + "','" + mainic + "' icode ,'T' flg," + sumqty + " est from dual ";
                            cmd = "select (s.icode||s.client_id||s.client_unit_id) fstr,nvl(bm.acode,'-') MainItem,max(nvl(est,0)) bmiqty,max(nvl(bm.irate,0)) lotqty,s.icode,s.iname,s.uom" +
                                ",sum(s.qtystk) as qtystk,s.fstg fstgcode, st.master_name fstg from " +
                         "(select i.icode, i.iname, um.master_name uom, sum(nvl(a.qtyin, 0)) qtyin, sum(nvl(a.qtyout, 0)) qtyout, sum(nvl(a.qtyin, 0)) - sum(nvl(a.qtyout, 0)) qtystk," +
                        "nvl(a.fstg, '001') fstg, i.type, i.client_id, i.client_unit_id from item i " +
                        "left join (select icode, (case when type='100' then 0 else qtyin end) qtyin, qtyout qtyout, stage fstg, type, " +
                        "client_id, client_unit_id,vch_num,vch_date from iprod where (type like '3%' or type = '10P' or type='100') " +
                        "union " +
                        "select acode icode, (to_number(max(qtyin))+to_number(max(qtyrw))) qtyin, 0 qtyout, stage1 fstg, type, client_id, client_unit_id ,vch_num,vch_date from iprod " +
                        "where type = '100' group by acode, stage1, type, client_id, client_unit_id ,vch_num,vch_date " +
                        "union " +
                        "select acode icode, to_number(max(qtyrej)) qtyin, 0 qtyout, '101' fstg, type, client_id, client_unit_id,vch_num,vch_date from iprod where type = '100' " +
                        "group by acode, type, client_id, client_unit_id,vch_num,vch_date " +
                        "union " +
                        "select icode, sum(qtyin) qtyin, 0 qtyout, stage1 fstg, type, client_id, client_unit_id ,vch_num,vch_date from iprod where type = '10R' " +
                        "group by icode, stage1, type, client_id, client_unit_id,vch_num,vch_date " +
                        "union " +
                        "select icode, 0 qtyin, sum(qtyout) qtyout, stage fstg, type,client_id, client_unit_id,vch_num,vch_date from iprod where type = '10R' " +
                        "group by icode, stage, type, client_id, client_unit_id,vch_num,vch_date " +
                        "union " +
                        "select icode, sum(qtyout) qtyin, 0 qtyout, stage1 fstg, type,client_id, client_unit_id,vch_num,vch_date from iprod where type = '10R' " +
                        "group by icode, stage1, type, client_id, client_unit_id,vch_num,vch_date) a on a.icode = i.icode and find_in_set(a.client_unit_id, i.client_unit_id)=1 " +
                        "inner join master_setting um on um.master_id = i.uom and um.type = 'UMM' and find_in_set(um.client_unit_id, i.client_unit_id)= 1 " +
                        "where i.type = 'IT' and find_in_set(i.client_unit_id, '" + unitid_mst + "')=1 " +
                        "group by i.icode,i.iname,um.master_name,nvl(a.fstg, '001'),i.type,i.client_id,i.client_unit_id) s " +
                        "left join " +
                         "(select client_id, client_unit_id, type, acode, pono, irate, icode, 'B' as flg, (to_number(qtyin) + to_number(qtyout)) / to_number(irate) * " + totqty + " est " +
                        "from itransactionc where type = 'BOM' and acode='" + mainic + "' and pono = '" + tstg + "' " + mq2 + ") bm " +
                        "on s.icode = bm.icode and s.client_unit_id = bm.client_unit_id and s.fstg in ('001', '" + tstg + "') " +
                        "left join master_setting st on st.master_id = s.fstg and st.type = 'KPS' " +
                        "where s.client_unit_id = '" + unitid_mst + "' " +
                        "and s.fstg in ('001','" + tstg + "') group by (s.icode||s.client_id||s.client_unit_id) ,nvl(bm.acode,'-'),s.icode,s.iname,s.uom,s.fstg, st.master_name order by icode ";
                            break;
                        case "ITEM": //main item                            
                            #region old command
                            //cmd = "select(bt.client_id || bt.client_unit_id || bt.icode || to_char(bt.vch_date, 'yyyymmdd') || bt.type) fstr,nvl(bm.acode, '-') MainICode,bt.icode Icode," +
                            //    "it.iname ItemName, pum.master_name UOM, mg.master_name MainGrp, gp.master_name ItemGrp, sg.iname SubGrp,sum(to_number(bt.qtyin) + to_number(bt.qtyout)) Floor_Stk " +
                            //    "from btchtrans bt " +
                            //    "left join itransactionc bm on bm.icode = bt.icode and bm.type = 'BOM' and bm.client_id = bt.client_id and bm.client_unit_id = bt.client_unit_id " +
                            //    "and bm.acode = '9010100001' and bm.pono = '004' " +
                            //    "inner join item it on it.icode = bt.icode and it.type = 'IT' and it.client_id = bt.client_id and it.client_unit_id = bt.client_unit_id " +
                            //    "inner join master_setting pum on pum.master_id = it.uom and pum.type = 'UMM' and find_in_set(pum.client_id, bt.client_id)= 1 and find_in_set(pum.client_unit_id, bt.client_unit_id)= 1 " +
                            //    "inner join master_setting mg on mg.classid = substr(bt.icode, 1, 1) and mg.type = 'KGP' " +
                            //    "inner join master_setting gp on gp.master_id = substr(bt.icode, 1, 3) and gp.type = 'KIG' and gp.client_id = bt.client_id and gp.client_unit_id = bt.client_unit_id " +
                            //    "inner join item sg on sg.icode = substr(bt.icode, 1, 5) and sg.type = 'SG' and sg.client_id = bt.client_id and sg.client_unit_id = bt.client_unit_id " +
                            //    "where bt.type in ('30', '32') and bt.client_id = '"+clientid_mst+"' and bt.client_unit_id = '"+unitid_mst+"' and bt.pono = 'W' " +
                            //    "group by(bt.client_id|| bt.client_unit_id || bt.icode || to_char(bt.vch_date, 'yyyymmdd') || bt.type),nvl(bm.acode, '-'),bt.icode, " +
                            //    "it.iname,pum.master_name,mg.master_name,gp.master_name,sg.iname";
                            //cmd = "select distinct (it.client_id||it.client_unit_id||it.icode) fstr,it.icode as Icode,it.iname as Iname,it.cpartno Partno,pum.master_name as UOM," +
                            //    "mg.master_name MainGrp, gp.master_name ItemGrp, sg.iname SubGrp,(case when nvl(bm.icode, 'N') = 'N' then 'N' else 'Y' end) bom_status from item it " +
                            //    "inner join master_setting pum on pum.master_id = it.uom and pum.type = 'UMM' and find_in_set(pum.client_id, it.client_id)= 1 and find_in_set(pum.client_unit_id, it.client_unit_id)= 1 " +
                            //    "inner join master_setting mg on mg.classid = substr(it.icode, 1, 1) and mg.type = 'KGP' " +
                            //    "inner join master_setting gp on gp.master_id = substr(it.icode, 1, 3) and gp.type = 'KIG' and gp.client_id = it.client_id and gp.client_unit_id = it.client_unit_id " +
                            //    "inner join item sg on sg.icode = substr(it.icode, 1, 5) and sg.type = 'SG' and sg.client_id = it.client_id and sg.client_unit_id = it.client_unit_id " +
                            //    "left join itransactionc bm on bm.acode = it.icode and bm.type = 'BOM' and bm.client_id = it.client_id and bm.client_unit_id = it.client_unit_id " +
                            //    "where it.type = 'IT' and substr(trim(it.icode),1,1) in ('8', '9') and it.client_id = '" + clientid_mst + "' and it.client_unit_id = '" + unitid_mst + "'";
                            #endregion
                            if (param1.Trim() == "100")
                            {
                                cmd = "select distinct (bm.client_id||bm.client_unit_id||bm.acode||bm.type) fstr,mg.master_name MainGrp,gp.master_name ItemGrp,sg.iname SubGrp," +
                                    "bm.acode,it.iname,it.cpartno Partno, pum.master_name as UOM from itransactionc bm " +
                                    "inner join item it on it.icode = bm.acode and it.type = 'IT' and find_in_set(it.client_unit_id, bm.client_unit_id)=1 " +
                                    "inner join master_setting pum on pum.master_id = it.uom and pum.type = 'UMM' and find_in_set(pum.client_unit_id, bm.client_unit_id)= 1 " +
                                    "inner join master_setting mg on mg.classid = substr(bm.acode, 1, 1) and mg.type = 'KGP' " +
                                    "inner join master_setting gp on gp.master_id = substr(bm.acode, 1, 3) and gp.type = 'KIG' and find_in_set(gp.client_unit_id, bm.client_unit_id)=1 " +
                                    "inner join item sg on sg.icode = substr(bm.acode, 1, 5) and sg.type = 'SG' and find_in_set(sg.client_unit_id, bm.client_unit_id)=1 " +
                                    "where bm.client_unit_id = '" + unitid_mst + "' and bm.type = 'BOM'";
                            }
                            else
                            {
                                cmd = "select distinct (bm.client_id||bm.client_unit_id||bm.acode||bm.type) fstr,mg.master_name MainGrp,gp.master_name ItemGrp,sg.iname SubGrp, bm.acode,it.iname,it.cpartno Partno, pum.master_name as UOM from itransactionc bm inner join item it on it.icode = bm.acode and it.type = 'IT' and find_in_set(it.client_unit_id, bm.client_unit_id)=1 inner join master_setting pum on pum.master_id = it.uom and pum.type = 'UMM' and find_in_set(pum.client_unit_id, bm.client_unit_id)= 1 inner join master_setting mg on mg.classid = substr(bm.acode, 1, 1) and mg.type = 'KGP' inner join master_setting gp on gp.master_id = substr(bm.acode, 1, 3) and gp.type = 'KIG' and find_in_set(gp.client_unit_id, bm.client_unit_id)=1 inner join item sg on sg.icode = substr(bm.acode, 1, 5) and sg.type = 'SG' and find_in_set(sg.client_unit_id, bm.client_unit_id)=1 left join(select a.vch_num, a.vch_date, a.type, a.icode, a.client_id, a.client_unit_id, (a.ord_qty), sum(to_number(a.used_qty)) as used_qty ,(a.ord_qty - nvl(sum(a.used_qty), '0')) as Bal_qty from(select w.vch_num, w.vch_date, w.col5 as icode, w.client_id , w.client_unit_id, w.type, (case when w.col10 = '-' then '0' else w.col10 end) as ord_qty,nvl(ex.qtyin, '0') used_qty from kc_tab w left join iprod ex on ex.pdono = w.vch_num and ex.type='101' and ex.client_unit_id = w.client_unit_id where w.type = 'PSO' and w.client_unit_id = '" + unitid_mst + "' union all select w.vch_num,w.vch_date,w.col5 as icode,w.client_id,w.client_unit_id,w.type,(case when w.col10 = '-' then '0' else w.col10 end) as ord_qty,nvl(to_number(ex.col11), '0') as close_qty from kc_tab w left join enx_tab ex on ex.col12 = w.vch_num and ex.type='COR' and ex.client_unit_id = w.client_unit_id where w.type = 'PSO' and w.client_unit_id = '" + unitid_mst + "') a group by (a.vch_num, a.vch_date, a.type, a.icode, a.client_id, a.client_unit_id, a.ord_qty)) w on w.icode = bm.acode and w.client_unit_id = bm.client_unit_id where bm.client_unit_id = '" + unitid_mst + "' and bm.type = 'BOM'";
                                //cmd = "select distinct (bm.client_id||bm.client_unit_id||bm.acode||bm.type) fstr,mg.master_name MainGrp,gp.master_name ItemGrp,sg.iname SubGrp," +
                                //   "bm.acode,it.iname,it.cpartno Partno, pum.master_name as UOM from itransactionc bm " +
                                //   "inner join item it on it.icode = bm.acode and it.type = 'IT' and it.client_id = bm.client_id and it.client_unit_id = bm.client_unit_id " +
                                //   "inner join master_setting pum on pum.master_id = it.uom and pum.type = 'UMM' and find_in_set(pum.client_id, bm.client_id)= 1 and find_in_set(pum.client_unit_id, bm.client_unit_id)= 1 " +
                                //   "inner join master_setting mg on mg.classid = substr(bm.acode, 1, 1) and mg.type = 'KGP' " +
                                //   "inner join master_setting gp on gp.master_id = substr(bm.acode, 1, 3) and gp.type = 'KIG' and gp.client_id = bm.client_id and gp.client_unit_id = bm.client_unit_id " +
                                //   "inner join item sg on sg.icode = substr(bm.acode, 1, 5) and sg.type = 'SG' and sg.client_id = bm.client_id and sg.client_unit_id = bm.client_unit_id " +
                                //   "inner join kc_tab w on w.col5=bm.acode and w.type='PSO' and w.client_id=bm.client_id and w.client_unit_id=bm.client_unit_id " +
                                //   "where bm.type = 'BOM' and bm.client_id = '" + clientid_mst + "' and bm.client_unit_id = '" + unitid_mst + "'";
                            }
                            break;
                        #region wiploc command
                        //case "WIPLOC":
                        //    cmd = "select (bm.client_id||bm.client_unit_id||bm.icode||to_char(bm.vch_date,'yyyymmdd')||bm.type) fstr,bm.acode MainItem,bm.icode,it.iname,pum.master_name uom," +
                        //           "sum(to_number(bm.qtyin) + to_number(bm.qtyout)) bmiqty,sum(to_number(lc.qtyin) + to_number(lc.qtyout)) lciqty,bm.irate lotqty,lc.loc," +
                        //           "location_name(bm.client_id || bm.client_unit_id || lc.loc) locname from itransactionc bm " +
                        //           "inner join item it on it.icode = bm.icode and it.type = 'IT' and it.client_id = bm.client_id and it.client_unit_id = bm.client_unit_id " +
                        //           "inner join master_setting pum on pum.master_id = it.uom and pum.type = 'UMM' and find_in_set(pum.client_id, bm.client_id)= 1 and " +
                        //           "find_in_set(pum.client_unit_id, bm.client_unit_id)= 1 " +
                        //           "inner join btchtrans lc on lc.icode = bm.icode and lc.type in ('30', '32') and lc.client_id = bm.client_id and lc.client_unit_id = bm.client_unit_id and " +
                        //           "lc.pono = 'W' and lc.loc = '" + Regex.Split(param2, "!~!~!~!~!")[2].Trim() + "' " +
                        //           "where bm.type = 'BOM' and bm.client_id = '" + clientid_mst + "' and bm.client_unit_id = '" + unitid_mst + "' and bm.acode = '" + Regex.Split(param2, "!~!~!~!~!")[0].Trim() + "' and " +
                        //           "bm.pono = '" + Regex.Split(param2, "!~!~!~!~!")[1].Trim() + "' and bm.icode<>'" + param1.TrimEnd(',').Replace(",", "','").Trim() + "' " +
                        //           "group by(bm.client_id|| bm.client_unit_id || bm.icode || to_char(bm.vch_date, 'yyyymmdd') || bm.type),bm.acode,bm.icode,it.iname,pum.master_name,bm.pono," +
                        //           "bm.cond,bm.irate,lc.loc,location_name(bm.client_id || bm.client_unit_id || lc.loc)";
                        //    break;
                        #endregion
                        case "NEW":
                            cmd = "select (master_id||type) fstr,master_id Type,master_name Name " +
                              "from master_setting where find_in_set(client_unit_id,'" + unitid_mst + "')=1 and type='KPD'";
                            break;
                            //case "WPDO":
                            //    cmd = "select (sd.client_id||sd.client_unit_id||sd.vch_num||to_char(sd.vch_date,'yyyymmdd')||sd.type) fstr,sd.vch_num as Doc_no," +
                            //        "to_char(sd.vch_date, '" + sgen.Getsqldateformat() + "') as Doc_date,sd.col5 as Icode,i.iname,i.cpartno Partno,u.master_name Uom," +
                            //        "sd.col9 ord_qty,sd.col10 plan_qty from kc_tab sd " +
                            //        "inner join item i on i.icode=sd.icode and i.type='IT' and i.client_id=sd.client_id and i.client_unit_id=sd.client_unit_id " +
                            //        "inner join u on u.master_id=i.uom and u.type='UMM' and find_in_set(u.client_id,sd.client_id)=1 and find_in_set(u.client_unit_id,sd.client_unit_id)=1 " +
                            //        "where sd.type = 'PSO' and sd.client_id = '" + clientid_mst + "' and sd.client_unit_id = '" + unitid_mst + "'";
                            //    break;
                    }
                    break;
                #endregion
                #region iprodn
                case "iprodn":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "PRINT":
                            cmd = "select distinct (ex.client_id||ex.client_unit_id||ex.vch_num||to_char(ex.vch_date,'yyyymmdd')||ex.type) fstr,ex.vch_num DocNo," +
                                "to_char(convert_tz(ex.vch_date, 'UTC', '" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') Production_Date,ex.acode MainItem," +
                                "it.iname MainItemName,ex.qtyin Produce_Qty from iprod ex " +
                                "inner join item it on it.icode=ex.acode and it.type='IT' and find_in_set(it.client_unit_id,ex.client_unit_id)=1 " +
                               "where ex.client_unit_id = '" + unitid_mst + "' and ex.type in ('100','101') and to_date(to_char(ex.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "') order by ex.vch_num desc";
                            break;
                        case "VIEW":
                            cmd = "select distinct (ex.client_id||ex.client_unit_id||ex.vch_num||to_char(ex.vch_date,'yyyymmdd')||ex.type) fstr,ex.vch_num DocNo," +
                           "to_char(convert_tz(ex.vch_date, 'UTC', '" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') Production_Date,ex.acode MainItem," +
                           "it.iname MainItemName,um.master_name Uom,stf.master_name From_Stage,stt.master_name To_Stage,ex.qtyin Produce_Qty,'-' Reference from iprod ex " +
                           "inner join item it on it.icode=ex.acode and it.type='IT' and find_in_set(it.client_unit_id,ex.client_unit_id)=1 " +
                           "inner join master_setting um on um.master_id=it.uom and um.type='UMM' and find_in_set(um.client_unit_id,it.client_unit_id)=1 " +
                           "inner join master_setting stf on stf.master_id=ex.stage and stf.type='KPS' " +
                           "inner join master_setting stt on stt.master_id=ex.mstage and stt.type='KPS' " +
                           "where ex.client_unit_id = '" + unitid_mst + "' and ex.type in ('100','101') and to_date(to_char(ex.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "') order by ex.vch_num desc";
                            //"union all "+
                            break;
                        case "PDITEM": //pd + click item                            
                            string mainic = Regex.Split(param2, "!~!~!~!~!")[0].Trim();
                            string tstg = Regex.Split(param2, "!~!~!~!~!")[1].Trim();
                            string sumqty = Regex.Split(param2, "!~!~!~!~!")[2].Trim();//3
                            string bmlotqty = Regex.Split(param2, "!~!~!~!~!")[3].Trim();
                            string totqty = Regex.Split(param2, "!~!~!~!~!")[4].Trim();//2
                            mq2 = " union all select '" + clientid_mst + "','" + unitid_mst + "','BOM','" + mainic + "','" + tstg + "','" + bmlotqty + "','" + mainic + "' icode ,'T' flg," + sumqty + " est from dual ";
                            cmd = "select (s.icode||s.client_id||s.client_unit_id) fstr,nvl(bm.acode,'-') MainItem,max(nvl(est,0)) bmiqty,max(nvl(bm.irate,0)) lotqty,s.icode,s.iname,s.uom" +
                                ",sum(s.qtystk) as qtystk,s.fstg fstgcode, st.master_name fstg from " +
                         "(select i.icode, i.iname, um.master_name uom, sum(nvl(a.qtyin, 0)) qtyin, sum(nvl(a.qtyout, 0)) qtyout, sum(nvl(a.qtyin, 0)) - sum(nvl(a.qtyout, 0)) qtystk," +
                        "nvl(a.fstg, '001') fstg, i.type, i.client_id, i.client_unit_id from item i " +
                        "left join (select icode, (case when type='100' then 0 else qtyin end) qtyin, qtyout qtyout, stage fstg, type, " +
                        "client_id, client_unit_id,vch_num,vch_date from iprod where (type like '3%' or type = '10P' or type='100') " +
                        "union " +
                        "select acode icode, (to_number(max(qtyin))+to_number(max(qtyrw))) qtyin, 0 qtyout, stage1 fstg, type, client_id, client_unit_id ,vch_num,vch_date from iprod " +
                        "where type = '100' group by acode, stage1, type, client_id, client_unit_id ,vch_num,vch_date " +
                        "union " +
                        "select acode icode, to_number(max(qtyrej)) qtyin, 0 qtyout, '101' fstg, type, client_id, client_unit_id,vch_num,vch_date from iprod where type = '100' " +
                        "group by acode, type, client_id, client_unit_id,vch_num,vch_date " +
                        "union " +
                        "select icode, sum(qtyin) qtyin, 0 qtyout, stage1 fstg, type, client_id, client_unit_id ,vch_num,vch_date from iprod where type = '10R' " +
                        "group by icode, stage1, type, client_id, client_unit_id,vch_num,vch_date " +
                        "union " +
                        "select icode, 0 qtyin, sum(qtyout) qtyout, stage fstg, type,client_id, client_unit_id,vch_num,vch_date from iprod where type = '10R' " +
                        "group by icode, stage, type, client_id, client_unit_id,vch_num,vch_date " +
                        "union " +
                        "select icode, sum(qtyout) qtyin, 0 qtyout, stage1 fstg, type,client_id, client_unit_id,vch_num,vch_date from iprod where type = '10R' " +
                        "group by icode, stage1, type, client_id, client_unit_id,vch_num,vch_date) a on a.icode = i.icode and find_in_set(a.client_unit_id, i.client_unit_id)=1 " +
                        "inner join master_setting um on um.master_id = i.uom and um.type = 'UMM' and find_in_set(um.client_unit_id, i.client_unit_id)= 1 " +
                        "where i.type = 'IT' and find_in_set(i.client_unit_id, '" + unitid_mst + "')=1 " +
                        "group by i.icode,i.iname,um.master_name,nvl(a.fstg, '001'),i.type,i.client_id,i.client_unit_id) s " +
                        "left join " +
                         "(select client_id, client_unit_id, type, acode, pono, irate, icode, 'B' as flg, (to_number(qtyin) + to_number(qtyout)) / to_number(irate) * " + totqty + " est " +
                        "from itransactionc where type = 'BOM' and acode='" + mainic + "' and pono = '" + tstg + "' " + mq2 + ") bm " +
                        "on s.icode = bm.icode and s.client_unit_id = bm.client_unit_id and s.fstg in ('001', '" + tstg + "') " +
                        "left join master_setting st on st.master_id = s.fstg and st.type = 'KPS' " +
                        "where s.client_unit_id = '" + unitid_mst + "' " +
                        "and s.fstg in ('001','" + tstg + "') group by (s.icode||s.client_id||s.client_unit_id) ,nvl(bm.acode,'-'),s.icode,s.iname,s.uom,s.fstg, st.master_name order by icode ";
                            break;
                        case "ITEM": //main item                                                        
                            if (param1.Trim() == "100")
                            {
                                cmd = "select distinct (bm.client_id||bm.client_unit_id||bm.acode||bm.type) fstr,mg.master_name MainGrp,gp.master_name ItemGrp,sg.iname SubGrp," +
                                    "bm.acode,it.iname,it.cpartno Partno, pum.master_name as UOM from itransactionc bm " +
                                    "inner join item it on it.icode = bm.acode and it.type = 'IT' and find_in_set(it.client_unit_id, bm.client_unit_id)=1 " +
                                    "inner join master_setting pum on pum.master_id = it.uom and pum.type = 'UMM' and find_in_set(pum.client_unit_id, bm.client_unit_id)= 1 " +
                                    "inner join master_setting mg on mg.classid = substr(bm.acode, 1, 1) and mg.type = 'KGP' " +
                                    "inner join master_setting gp on gp.master_id = substr(bm.acode, 1, 3) and gp.type = 'KIG' and find_in_set(gp.client_unit_id, bm.client_unit_id)=1 " +
                                    "inner join item sg on sg.icode = substr(bm.acode, 1, 5) and sg.type = 'SG' and find_in_set(sg.client_unit_id, bm.client_unit_id)=1 " +
                                    "where bm.client_unit_id = '" + unitid_mst + "' and bm.type = 'BOM'";
                            }
                            else
                            {
                                cmd = "select distinct (bm.client_id||bm.client_unit_id||bm.acode||bm.type) fstr,mg.master_name MainGrp,gp.master_name ItemGrp,sg.iname SubGrp, bm.acode,it.iname,it.cpartno Partno, pum.master_name as UOM from itransactionc bm inner join item it on it.icode = bm.acode and it.type = 'IT' and find_in_set(it.client_unit_id, bm.client_unit_id)=1 inner join master_setting pum on pum.master_id = it.uom and pum.type = 'UMM' and find_in_set(pum.client_unit_id, bm.client_unit_id)= 1 inner join master_setting mg on mg.classid = substr(bm.acode, 1, 1) and mg.type = 'KGP' inner join master_setting gp on gp.master_id = substr(bm.acode, 1, 3) and gp.type = 'KIG' and find_in_set(gp.client_unit_id, bm.client_unit_id)=1 inner join item sg on sg.icode = substr(bm.acode, 1, 5) and sg.type = 'SG' and find_in_set(sg.client_unit_id, bm.client_unit_id)=1 left join(select a.vch_num, a.vch_date, a.type, a.icode, a.client_id, a.client_unit_id, (a.ord_qty), sum(to_number(a.used_qty)) as used_qty ,(a.ord_qty - nvl(sum(a.used_qty), '0')) as Bal_qty from(select w.vch_num, w.vch_date, w.col5 as icode, w.client_id , w.client_unit_id, w.type, (case when w.col10 = '-' then '0' else w.col10 end) as ord_qty,nvl(ex.qtyin, '0') used_qty from kc_tab w left join iprod ex on ex.pdono = w.vch_num and ex.type='101' and ex.client_unit_id = w.client_unit_id where w.type = 'PSO' and w.client_unit_id = '" + unitid_mst + "' union all select w.vch_num,w.vch_date,w.col5 as icode,w.client_id,w.client_unit_id,w.type,(case when w.col10 = '-' then '0' else w.col10 end) as ord_qty,nvl(to_number(ex.col11), '0') as close_qty from kc_tab w left join enx_tab ex on ex.col12 = w.vch_num and ex.type='COR' and ex.client_unit_id = w.client_unit_id where w.type = 'PSO' and w.client_unit_id = '" + unitid_mst + "') a group by (a.vch_num, a.vch_date, a.type, a.icode, a.client_id, a.client_unit_id, a.ord_qty)) w on w.icode = bm.acode and w.client_unit_id = bm.client_unit_id where bm.client_unit_id = '" + unitid_mst + "' and bm.type = 'BOM'";
                                //cmd = "select distinct (bm.client_id||bm.client_unit_id||bm.acode||bm.type) fstr,mg.master_name MainGrp,gp.master_name ItemGrp,sg.iname SubGrp," +
                                //   "bm.acode,it.iname,it.cpartno Partno, pum.master_name as UOM from itransactionc bm " +
                                //   "inner join item it on it.icode = bm.acode and it.type = 'IT' and it.client_id = bm.client_id and it.client_unit_id = bm.client_unit_id " +
                                //   "inner join master_setting pum on pum.master_id = it.uom and pum.type = 'UMM' and find_in_set(pum.client_id, bm.client_id)= 1 and find_in_set(pum.client_unit_id, bm.client_unit_id)= 1 " +
                                //   "inner join master_setting mg on mg.classid = substr(bm.acode, 1, 1) and mg.type = 'KGP' " +
                                //   "inner join master_setting gp on gp.master_id = substr(bm.acode, 1, 3) and gp.type = 'KIG' and gp.client_id = bm.client_id and gp.client_unit_id = bm.client_unit_id " +
                                //   "inner join item sg on sg.icode = substr(bm.acode, 1, 5) and sg.type = 'SG' and sg.client_id = bm.client_id and sg.client_unit_id = bm.client_unit_id " +
                                //   "inner join kc_tab w on w.col5=bm.acode and w.type='PSO' and w.client_id=bm.client_id and w.client_unit_id=bm.client_unit_id " +
                                //   "where bm.type = 'BOM' and bm.client_id = '" + clientid_mst + "' and bm.client_unit_id = '" + unitid_mst + "'";
                            }
                            break;                        
                        case "NEW":
                            cmd = "select (master_id||type) fstr,master_id Type,master_name Name " +
                              "from master_setting where find_in_set(client_unit_id,'" + unitid_mst + "')=1 and type='KPD'";
                            break;                         
                    }
                    break;
                #endregion
                #region pdrevrse
                case "pdrevrse":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            cmd = "select distinct (ex.client_id||ex.client_unit_id||ex.vch_num||to_char(ex.vch_date,'yyyymmdd')||ex.type) fstr,ex.vch_num DocNo," +
                                "to_char(convert_tz(ex.vch_date, 'UTC', '" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') Production_Date,ex.acode MainItem," +
                                "it.iname MainItemName,ex.qtyin Produce_Qty from iprod ex " +
                                "inner join item it on it.icode=ex.acode and it.type='IT' and find_in_set(it.client_unit_id,ex.client_unit_id)=1 " +
                                "where ex.client_unit_id = '" + unitid_mst + "' and ex.type ='100' and to_date(to_char(ex.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "') order by ex.vch_num desc";
                            break;
                        case "VIEW":
                            cmd = "select distinct (ex.client_id||ex.client_unit_id||ex.vch_num||to_char(ex.vch_date,'yyyymmdd')||ex.type) fstr,ex.vch_num DocNo," +
                                "to_char(convert_tz(ex.vch_date, 'UTC', '" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') Production_Date,ex.acode MainItem," +
                                "it.iname MainItemName,um.master_name Uom,stf.master_name From_Stage,stt.master_name To_Stage,ex.qtyin Produce_Qty from iprod ex " +
                                "inner join item it on it.icode=ex.acode and it.type='IT' and find_in_set(it.client_unit_id,ex.client_unit_id)=1 " +
                                "inner join master_setting um on um.master_id=it.uom and um.type='UMM' and find_in_set(um.client_unit_id,it.client_unit_id)=1 " +
                                "inner join master_setting stf on stf.master_id=ex.stage and stf.type='KPS' " +
                                "inner join master_setting stt on stt.master_id=ex.mstage and stt.type='KPS' " +
                                "where ex.client_unit_id = '" + unitid_mst + "' and ex.refcode<>'' and to_date(to_char(ex.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "') order by ex.vch_num desc";
                            break;
                    }
                    break;
                #endregion
                #region invreps
                case "invreps":
                    switch (btnval.ToUpper())
                    {
                        case "WIPS":
                            cmd = "select a.icode,b.iname,a.stage,c.master_name,a.client_id,a.client_unit_id,sum(a.qtyout) AS qtyout,sum(a.qtyin) as qtyin ,sum(a.qtyin)-sum(a.qtyout)" +
                                "  as Closing from (select icode, stage, client_id, client_unit_id, TO_NUMBER(qtyout) AS qtyout, 0 as qtyin from iprod " +
                                "  select icode, stage, client_id, client_unit_id, 0 qtyout, to_number(qtyin) as qtyin from ( select max(a.qtyin) as qtyin, " +
                                "a.acode as icode, a.stage1 as stage, a.client_id, a.client_unit_id,a.vch_num, to_char(a.vch_date, 'yyyyMMdd') as vch_date " +
                                "from iprod a  group by a.acode, a.stage1, a.client_id, a.client_unit_id, a.vch_num, to_char(a.vch_date, 'yyyyMMdd')) b) a " +
                                " inner join item b on trim(a.icode) = trim(b.icode) and find_in_set(a.client_unit_id, " +
                                "b.client_unit_id)= 1   and b.type = 'IT' inner join master_setting c on trim(a.stage)= trim(c.master_id) and " +
                                "find_in_set(a.client_unit_id, c.client_unit_id)= 1 and c.type = 'KPS' " +
                                "group by a.icode,a.stage,a.client_id,a.client_unit_id,b.iname,c.master_name";
                            break;
                    }
                    break;
                #endregion
                #region pdreps
                case "pdreps":
                    switch (btnval.ToUpper())
                    {
                        case "IDET":
                            // kashish 4th last line
                            cmd = "select (i.icode||nvl(a.fstg, '001')) fstr,a.vch_num,a.vch_date,i.icode, i.iname, um.master_name uom, sum(nvl(a.qtyin, 0)) qtyin, sum(nvl(a.qtyout, 0)) qtyout, " +
                                "sum(nvl(a.qtyin, 0)) - sum(nvl(a.qtyout, 0)) qtystk,nvl(a.fstg, '001') fstg, a.type, i.client_id, i.client_unit_id from item i " +
                                "left join(select icode, (case when type= '100' then 0 else qtyin end) qtyin, qtyout qtyout, stage fstg, type, client_id, client_unit_id," +
                                "vch_num,vch_date from iprod where(type like '3%' or type = '10P' or type = '100') " +
                                "union all select acode icode, (to_number(max(qtyin)) + to_number(max(qtyrw))) qtyin,0 qtyout,stage1 fstg,type,client_id,client_unit_id," +
                                "vch_num,vch_date from iprod where type = '100' group by acode, stage1, type, client_id, client_unit_id ,vch_num,vch_date " +
                                "union all select acode icode, to_number(max(qtyrej)) qtyin, 0 qtyout, '101' fstg, type, client_id, client_unit_id,vch_num,vch_date from iprod " +
                                "where type = '100' group by acode, type, client_id, client_unit_id,vch_num,vch_date " +
                                "union all select icode, sum(qtyin) qtyin, 0 qtyout, stage1 fstg, type, client_id, client_unit_id, vch_num, vch_date from iprod " +
                                "where type = '10R' group by icode, stage1, type, client_id, client_unit_id,vch_num,vch_date " +
                                "union all select icode, 0 qtyin, sum(qtyout) qtyout, stage fstg, type, client_id, client_unit_id, vch_num, vch_date from iprod " +
                                "where type = '10R' group by icode, stage, type, client_id, client_unit_id,vch_num,vch_date " +
                                "union all select icode, sum(qtyout) qtyin, 0 qtyout, stage1 fstg, type, client_id, client_unit_id, vch_num, vch_date from iprod " +
                                "where type = '10R' group by icode, stage1, type, client_id, client_unit_id,vch_num,vch_date) a " +
                                "on a.icode = i.icode and find_in_set(a.client_unit_id, i.client_unit_id)= 1 " +
                                "inner join master_setting um on um.master_id = i.uom and um.type = 'UMM' and find_in_set(um.client_unit_id, i.client_unit_id)= 1 " +
                                "where i.type = 'IT' and find_in_set(i.client_unit_id,a.client_unit_id)= 1 and (i.icode||nvl(a.fstg, '001'))='" + param1 + "' and " +
                                "to_date(to_char(a.vch_date,'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldateformat() + "') between " +
                                "to_date('" + param2.Split(new string[] { "!~!~!~!~!" }, StringSplitOptions.None)[0].Trim() + "','" + sgen.Getsqldateformat() + "') and " +
                                "to_date('" + param2.Split(new string[] { "!~!~!~!~!" }, StringSplitOptions.None)[1].Trim() + "','" + sgen.Getsqldateformat() + "')" +
                                "group by (i.icode||nvl(a.fstg, '001')),a.vch_num,a.vch_date,a.type,i.icode,i.iname,um.master_name,nvl(a.fstg, '001'),i.type,i.client_id,i.client_unit_id";
                            break;
                    }
                    break;
                #endregion
                #region irej
                case "irej":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            cmd = "select (ex.client_id||ex.client_unit_id||ex.vch_num||to_char(ex.vch_date,'yyyymmdd')||ex.type) fstr,ex.vch_num DocNo," +
                                "to_char(convert_tz(ex.vch_date, 'UTC', '" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') Doc_Date,ex.acode Acode," +
                                "it.iname Aname,ex.qtychl RedbinQty," +
                                "ex.qtychl Analysis_Qty,ex.qtyout ApprovedQty,ex.qtyrej RejQty from iprod ex " +
                                "inner join item it on it.icode=ex.acode and it.type='IT' and find_in_set(it.client_unit_id,ex.client_unit_id)=1 " +
                                 "where ex.type ='10R' and ex.client_unit_id = '" + unitid_mst + "' and to_date(to_char(ex.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "')";
                            break;
                        case "VIEW":
                            cmd = "select (ex.client_id||ex.client_unit_id||ex.vch_num||to_char(ex.vch_date,'yyyymmdd')||ex.type) fstr,ex.vch_num DocNo," +
                                "to_char(convert_tz(ex.vch_date, 'UTC', '" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') Doc_Date,ex.acode Acode," +
                                "it.iname Aname,um.master_name Uom,(case when ex.mstage='100' then 'PDI' else stf.master_name end) Rej_Stage,ex.qtychl RedbinQty," +
                                "ex.qtychl Analysis_Qty,ex.qtyout ApprovedQty,ex.qtyrej RejQty," +
                                "(case when ex.stage1='100' then 'PDI' when ex.stage1='102' then 'Rework' else stt.master_name end) Approved_Stage " +
                                "from iprod ex " +
                                "inner join item it on it.icode=ex.acode and it.type='IT' and find_in_set(it.client_unit_id,ex.client_unit_id)=1 " +
                                "inner join master_setting um on um.master_id=it.uom and um.type='UMM' and find_in_set(um.client_unit_id,it.client_unit_id)=1 " +
                                "left join master_setting stf on stf.master_id=ex.mstage and stf.type='KPS' " +
                                "left join master_setting stt on stt.master_id=ex.stage1 and stt.type='KPS' " +
                                "where ex.type ='10R' and ex.client_unit_id = '" + unitid_mst + "' and to_date(to_char(ex.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "')";
                            break;
                        case "ITEM": //main item                                                     
                            cmd = "select (a.client_id||a.client_unit_id||a.icode||a.fstg) fstr,a.icode,i.iname,u.master_name uom,sum(a.qtyin)-sum(a.qtyout) qtystk,a.fstg fstgcode," +
                                "(case when a.fstg = '101' then 'Rejection' when a.fstg='100' then 'PDI' else s.master_name end) fstg,a.client_id,a.client_unit_id from " +
                                "(select acode icode,to_number(max(qtyrej)) qtyin, 0 qtyout, mstage fstg, type, client_id, client_unit_id, vch_num, vch_date from iprod where type = '100' " +
                                "group by acode, type, client_id, client_unit_id,vch_num,vch_date,mstage " +
                                "union " +
                                "select icode, sum(qtyin) qtyin, sum(qtychl) qtyout, mstage fstg, type,client_id, client_unit_id, vch_num, vch_date from iprod where " +
                                "type = '10R' group by icode, mstage, type, client_id, client_unit_id,vch_num,vch_date " +
                                "union " +
                                "select icode, 0 qtyin, sum(qtyout) qtyout, stage fstg, type, client_id, client_unit_id, vch_num, vch_date from iprod where type = '10R' " +
                                "group by icode, stage, type, client_id, client_unit_id,vch_num,vch_date) a " +
                                "inner join item i on i.icode = a.icode and i.type = 'IT' and find_in_set(i.client_unit_id, a.client_unit_id)=1 " +
                                "inner join master_setting u on u.master_id = i.uom and u.type = 'UMM' and find_in_set(u.client_unit_id, a.client_unit_id)= 1 " +
                                "left join master_setting s on s.master_id = a.fstg and s.type = 'KPS' " +
                                "where a.client_unit_id = '" + unitid_mst + "' " +
                                "group by a.icode,i.iname,u.master_name,a.fstg,s.master_name,a.client_id,a.client_unit_id having sum(qtyin) - sum(qtyout) > 0";
                            break;
                    }
                    break;
                #endregion
                #region rewrk
                case "rewrk":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            cmd = "select (ex.client_id||ex.client_unit_id||ex.vch_num||to_char(ex.vch_date,'yyyymmdd')||ex.type) fstr,ex.vch_num DocNo," +
                             "to_char(convert_tz(ex.vch_date, 'UTC', '" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') Doc_Date,ex.acode Acode," +
                             "it.iname Aname,ex.qtychl ReworkQty," +
                             "ex.qtychl Analysis_Qty,ex.qtyout ApprovedQty,ex.qtyrej RejQty from iprod ex " +
                             "inner join item it on it.icode=ex.acode and it.type='IT' and find_in_set(it.client_unit_id,ex.client_unit_id)=1 " +
                             "where ex.type ='10W' and ex.client_unit_id = '" + unitid_mst + "' and to_date(to_char(ex.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "')";
                            break;
                        case "VIEW":
                            cmd = "select (ex.client_id||ex.client_unit_id||ex.vch_num||to_char(ex.vch_date,'yyyymmdd')||ex.type) fstr,ex.vch_num DocNo," +
                              "to_char(convert_tz(ex.vch_date, 'UTC', '" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') Doc_Date,ex.acode Acode," +
                              "it.iname Aname,um.master_name Uom,(case when ex.mstage='102' then 'Rework' else stf.master_name end) Rew_Stage,ex.qtychl ReworkQty," +
                              "ex.qtychl Analysis_Qty,ex.qtyout ApprovedQty,ex.qtyrej RejQty," +
                              "(case when ex.stage1='100' then 'PDI' when ex.stage1='102' then 'Rework' else stt.master_name end) Approved_Stage " +
                              "from iprod ex " +
                              "inner join item it on it.icode=ex.acode and it.type='IT' and find_in_set(it.client_unit_id,ex.client_unit_id)=1 " +
                              "inner join master_setting um on um.master_id=it.uom and um.type='UMM' and find_in_set(um.client_unit_id,it.client_unit_id)=1 " +
                              "left join master_setting stf on stf.master_id=ex.mstage and stf.type='KPS' " +
                              "left join master_setting stt on stt.master_id=ex.stage1 and stt.type='KPS' " +
                              "where ex.type ='10W' and ex.client_unit_id = '" + unitid_mst + "' and to_date(to_char(ex.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "')";
                            break;
                        case "ITEM": //main item                            
                            cmd = "select (a.client_id||a.client_unit_id||a.icode||a.fstg) fstr,a.icode,i.iname,u.master_name uom,sum(a.qtyin)-sum(a.qtyout) qtystk,a.fstg fstgcode," +
                                "(case when a.fstg = '102' then 'Rework' else s.master_name end) fstg,a.client_id,a.client_unit_id from " +
                                "(select icode, sum(qtyin) qtyin, 0 qtyout, stage1 fstg, type, client_id, client_unit_id, vch_num, vch_date from iprod where type = '10R' " +
                                "group by icode, stage1, type, client_id, client_unit_id,vch_num,vch_date " +
                                "union " +
                                "select icode, 0 qtyin, sum(qtychl) qtyout, stage fstg, type, client_id, client_unit_id, vch_num, vch_date from iprod where type = '10R' " +
                                "group by icode, stage,type, client_id, client_unit_id,vch_num,vch_date " +
                                "union " +
                                "select icode, sum(qtyout) qtyin, 0 qtyout, stage1 fstg, type, client_id,client_unit_id, vch_num, vch_date from iprod where type = '10R' " +
                                "group by icode, stage1, type, client_id, client_unit_id,vch_num,vch_date) a " +
                                "inner join item i on i.icode = a.icode and i.type = 'IT' and find_in_set(i.client_unit_id, a.client_unit_id)=1 " +
                                "inner join master_setting u on u.master_id = i.uom and u.type = 'UMM' and find_in_set(u.client_unit_id, a.client_unit_id)= 1 " +
                                "left join master_setting s on s.master_id = a.fstg and s.type = 'KPS' " +
                                "where a.fstg = '102' and a.client_unit_id = '" + unitid_mst + "' " +
                                "group by a.icode,i.iname,u.master_name,a.fstg,s.master_name,a.client_id,a.client_unit_id having sum(qtyin) - sum(qtyout) > 0";
                            break;
                    }
                    break;
                #endregion
                #region stgtransfer
                case "stgtransfer":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            cmd = "select (ex.client_unit_id||ex.vch_num||to_char(ex.vch_date,'yyyymmdd')||ex.type) fstr,ex.vch_num DocNo," +
                                "to_char(convert_tz(ex.vch_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') Doc_Date," +
                                "ex.icode FromIcode,i.iname FromIname,ex.stage FromStgcode,ex.qtystk,ex.qtyin,ex.acode ToIcode,it.ToIname," +
                                "ex.stage1 ToStgcode from iprod ex " +
                                "inner join item i on ex.icode = i.icode and i.type = 'IT' and find_in_set(i.client_unit_id, ex.client_unit_id)=1 " +
                                "inner join item it on ex.acode = it.icode and it.type = 'IT' and find_in_set(it.client_unit_id, ex.client_unit_id)=1 " +
                                "where ex.type = 'PSG' and ex.client_unit_id = '" + unitid_mst + "' and to_date(to_char(ex.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "')";
                            break;
                        case "VIEW":
                            cmd = "select (ex.client_unit_id||ex.vch_num||to_char(ex.vch_date,'yyyymmdd')||ex.type) fstr,ex.vch_num DocNo," +
                                "to_char(convert_tz(ex.vch_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') Doc_Date," +
                                "ex.icode FromIcode,i.iname FromIname,ex.stage FromStgcode,sf.master_name FromStage,ex.qtystk,ex.qtyin,ex.acode ToIcode,it.ToIname," +
                                "ex.stage1 ToStgcode,st.master_name ToStage from iprod ex " +
                                "inner join item i on ex.icode = i.icode and i.type = 'IT' and find_in_set(i.client_unit_id, ex.client_unit_id)=1 " +
                                "inner join master_setting sf on sf.master_id = ex.stage and sf.type = 'KPS' " +
                                "inner join item it on ex.acode = it.icode and it.type = 'IT' and find_in_set(it.client_unit_id, ex.client_unit_id)=1 " +
                                "inner join master_setting st on st.master_id = ex.stage1 and st.type = 'KPS' " +
                                "where ex.type = 'PSG' and ex.client_unit_id = '" + unitid_mst + "' and to_date(to_char(ex.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "')";
                            break;
                        case "FITEM":
                        case "TITEM":
                            cmd = "select distinct (p.client_unit_id||p.acode||p.type) fstr,mg.master_name MainGrp,gp.master_name ItemGrp,sg.iname SubGrp," +
                                    "p.acode,i.iname,i.cpartno Partno from iprod p " +
                                    "inner join item i on i.icode = p.acode and i.type = 'IT' and find_in_set(i.client_unit_id, p.client_unit_id)=1 " +
                                    "inner join master_setting mg on mg.classid = substr(p.acode, 1, 1) and mg.type = 'KGP' " +
                                    "inner join master_setting gp on gp.master_id = substr(p.acode, 1, 3) and gp.type = 'KIG' and find_in_set(gp.client_unit_id, p.client_unit_id)=1 " +
                                    "inner join item sg on sg.icode = substr(p.acode, 1, 5) and sg.type = 'SG' and find_in_set(sg.client_unit_id, p.client_unit_id)=1 " +
                                    "where p.client_unit_id = '" + unitid_mst + "' and p.type in ('100','101') and to_date(to_char(p.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "')";
                            break;
                    }
                    break;
                #endregion
                #region mlditm
                case "mlditm":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                            cmd = "select distinct (ex.client_unit_id||ex.vch_num||to_char(ex.vch_date,'yyyymmdd')||ex.type) fstr,ex.vch_num DocNo," +
                                "to_char(convert_tz(ex.vch_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') Doc_Date," +
                                "ex.col3 MouldId,m.col31 MouldName from enx_tab ex " +
                                "inner join item i on ex.col2 = i.icode and i.type = 'IT' and find_in_set(i.client_unit_id, ex.client_unit_id)=1 " +
                                "inner join kc_tab m on m.vch_num=ex.col3 and m.type='MOS' and m.client_unit_id=ex.client_unit_id " +
                                "where ex.type = 'IMD' and ex.client_unit_id = '" + unitid_mst + "' and to_date(to_char(ex.vch_DATE,'ddMMyyyy'),'ddMMyyyy') between to_date('" + year_from + "','" + sgen.Getsqldatetimeformat() + "') and to_date('" + year_to + "','" + sgen.Getsqldatetimeformat() + "')";
                            break;
                        case "MLD":
                            cmd = "select (k.client_unit_id||k.vch_num||to_char(k.vch_date,'yyyymmdd')||k.type) fstr,k.vch_num DocNo,k.col31 Mould_Name,k.col1 Mould_no," +
                                   "k.col32 Part_name,k.col2 Part_No, k.col33 Mould_cavity,k.col3 Status,k.col7 frequency,k.col6 no_of_strock," +
                                   "nvl(c.master_name, '0') mould_caps,nvl(group_concat(m.master_name), '0') machine_compatibility " +
                                   "from kc_tab k " +
                                   "left join master_setting c on c.master_id = k.col14 and c.type = 'K02' and find_in_set(c.client_unit_id, k.client_unit_id) = 1 " +
                                   "left join master_setting m on find_in_set(m.master_id, k.col34)= 1 and m.type = 'K02' and find_in_set(m.client_unit_id, k.client_unit_id) = 1 " +
                                   "where k.client_unit_id = '" + unitid_mst + "' and (k.type = 'MOS' or k.type = 'DDMOS') " +
                                   "group by (k.client_unit_id || k.vch_num || to_char(k.vch_date, 'yyyymmdd') || k.type),k.vch_num,k.col31,k.col1,k.col32,k.col2,k.col33," +
                                   "k.col3,k.col7,k.col6,nvl(c.master_name, '0') order by k.vch_num asc";
                            break;
                        case "ITEM":
                            cmd = "select (it.icode||to_char(it.vch_date,'yyyymmdd')||it.type) fstr,it.icode Icode,it.iname Iname,it.cpartno,um.master_name uom " +
                               "from item it " +
                               "inner join master_setting um on um.master_id = it.uom and um.type = 'UMM' and find_in_set(um.client_unit_id, it.client_unit_id)= 1 " +
                               "where it.type = 'IT' and find_in_set(it.client_unit_id,'" + unitid_mst + "')=1";
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
            String URL = sgen.GetSession(MyGuid, "SSEEKVAL").ToString();
            FillMst(MyGuid);
            List<Tmodelmain> rpt_model = new List<Tmodelmain>();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<SelectListItem> mod2 = new List<SelectListItem>();
            List<SelectListItem> mod3 = new List<SelectListItem>();
            List<SelectListItem> mod4 = new List<SelectListItem>();
            List<SelectListItem> mod5 = new List<SelectListItem>();
            List<SelectListItem> mod6 = new List<SelectListItem>();
            List<SelectListItem> mod7 = new List<SelectListItem>();
            DataTable dtt = new DataTable();
            DataTable dtp = new DataTable();
            string defcall = "", fdt = "", tdt = "";
            var tm = model.FirstOrDefault();
            switch (viewName.ToLower())
            {
                #region machine_master
                case "machine_master":
                    mq = @"select k.col8,k.col9,k.col31 Machine_Name,k.col1 Srno,k.col32 Manufact_name,k.col2 Machine_model,k.col33 Machine_loc,k.col3 Machine_cap,k.col4 Average_spm," +
                          "to_char(k.date1, '" + sgen.Getsqldateformat() + "') Purchase_Date,to_char(k.date2, '" + sgen.Getsqldateformat() + "') as Date2,k.col5 frequency,k.col6 no_of_strock,k.col7 as status,k.vch_num,k.ent_by," +
                         "k.ent_date,k.edit_by,k.edit_date,k.client_id,k.client_unit_id,k.vch_date,k.rec_id,k.col10,to_char(k.date3, '" + sgen.Getsqldateformat() + "') date3," +
                         "k.col34,k.col11,k.col12,k.col13,k.col14,to_char(k.date4, '" + sgen.Getsqldateformat() + "') date4,k.col15,k.col16,k.col17,k.col18," +
                         "to_char(k.date5, '" + sgen.Getsqldateformat() + "') date5,to_char(k.date6, '" + sgen.Getsqldateformat() + "') date6,to_char(k.date7, '" + sgen.Getsqldateformat() + "') date7," +
                         "k.col19,k.col20,k.col21 " +
                       " from kc_tab k  where k.client_id|| k.client_unit_id|| k.vch_num||to_char(k.vch_date, 'yyyymmdd')|| k.type='" + URL + "' ";
                    dtt = sgen.getdata(userCode, mq);
                    if (dtt.Rows.Count > 0)
                    {
                        model[0].Col17 = dtt.Rows[0]["Machine_Name"].ToString();
                        model[0].Col18 = dtt.Rows[0]["Srno"].ToString();
                        model[0].Col19 = dtt.Rows[0]["Manufact_name"].ToString();
                        model[0].Col20 = dtt.Rows[0]["Machine_model"].ToString();
                        model[0].Col21 = dtt.Rows[0]["Machine_loc"].ToString();
                        model[0].Col22 = dtt.Rows[0]["Machine_cap"].ToString();
                        model[0].Col23 = dtt.Rows[0]["Average_spm"].ToString();
                        model[0].Col24 = dtt.Rows[0]["Purchase_Date"].ToString();
                        model[0].Col25 = dtt.Rows[0]["Date2"].ToString();
                        model[0].Col26 = dtt.Rows[0]["frequency"].ToString();
                        model[0].Col27 = dtt.Rows[0]["no_of_strock"].ToString();
                        model[0].Col29 = dtt.Rows[0]["status"].ToString();
                        model[0].Col16 = dtt.Rows[0]["vch_num"].ToString();
                        model[0].Col38 = dtt.Rows[0]["col10"].ToString();
                        model[0].Col39 = dtt.Rows[0]["date3"].ToString();
                        model[0].Col40 = dtt.Rows[0]["col34"].ToString();
                        model[0].Col41 = dtt.Rows[0]["col11"].ToString();
                        model[0].Col42 = dtt.Rows[0]["col12"].ToString();
                        model[0].Col43 = dtt.Rows[0]["col13"].ToString();
                        model[0].Col44 = dtt.Rows[0]["date4"].ToString();
                        model[0].Col45 = dtt.Rows[0]["col15"].ToString();
                        model[0].Col46 = dtt.Rows[0]["col16"].ToString();
                        model[0].Col47 = dtt.Rows[0]["col17"].ToString();
                        model[0].Col48 = dtt.Rows[0]["col18"].ToString();
                        model[0].Col49 = dtt.Rows[0]["date5"].ToString();
                        model[0].Col50 = dtt.Rows[0]["date6"].ToString();
                        model[0].Col51 = dtt.Rows[0]["date7"].ToString();
                        model[0].Col52 = dtt.Rows[0]["col19"].ToString();
                        model[0].Col53 = dtt.Rows[0]["col20"].ToString();
                        model[0].Col54 = dtt.Rows[0]["col21"].ToString();
                        #region Frequency
                        mod1 = new List<SelectListItem>();
                        mod1 = cmd_Fun.freq(userCode, unitid_mst);
                        model[0].TList1 = mod1;
                        TempData[MyGuid + "_TList1"] = model[0].TList1;
                        #endregion
                        #region single Location
                        mod2 = new List<SelectListItem>();
                        mod2 = cmd_Fun.iloc(userCode, unitid_mst, out defcall);
                        model[0].TList2 = mod2;
                        TempData[MyGuid + "_TList2"] = model[0].TList2;
                        #endregion
                        #region machine capacity
                        mod3 = new List<SelectListItem>();
                        mod3 = cmd_Fun.mcap(userCode, unitid_mst);
                        model[0].TList3 = mod3;
                        TempData[MyGuid + "_TList3"] = model[0].TList3;
                        #endregion
                        String[] L1 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["col8"].ToString()).Distinct()).Split(',');
                        model[0].SelectedItems1 = L1;
                        String[] L2 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["col9"].ToString()).Distinct()).Split(',');
                        model[0].SelectedItems2 = L2;
                        String[] L3 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["col14"].ToString()).Distinct()).Split(',');
                        model[0].SelectedItems3 = L3;
                        DataTable dtf = new DataTable();
                        dtf = sgen.getdata(userCode, "select rec_id,file_name,file_path,col1 from file_tab where ref_code='" + dtt.Rows[0]["vch_num"].ToString() + "' and type='PMM' and client_unit_id='" + unitid_mst + "'");
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
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                DataTable dt = new DataTable();
                                DataRow dr = dtt.Rows[0];
                                model[0].Col8 = "client_id|| client_unit_id|| vch_num|| to_char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";
                                model[0].Col3 = dr["ent_by"].ToString();
                                model[0].Col4 = dr["ent_date"].ToString();
                                model[0].Col16 = dr["vch_num"].ToString();
                                model[0].Col1 = dr["client_id"].ToString();
                                model[0].Col2 = dr["client_unit_id"].ToString();
                                model[0].Col7 = dr["rec_id"].ToString();
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
                        case "VIEW":
                            if (dtt.Rows.Count > 0)
                            {
                                ViewBag.scripCall = "disableForm();";
                            }
                            break;
                        case "FDEL":
                            string fid = sgen.GetSession(MyGuid, "delid").ToString();
                            res = sgen.execute_cmd(userCode, "Delete from file_tab WHERE rec_id='" + fid + "' and type='PMM' and client_id='"
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
                #region mould_master
                case "mould_master":
                    //mq = @"select k.col31 as Mould_Name,k.col1 as Mouldno,k.col32 as Partname,k.col2 as PartNo,k.col33 as Mould_cavity,k.col3 as Status,
                    //  k.col4 as Std_avgtym,to_char(convert_tz(k.date1, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as date" +
                    //  ",k.col5 as frequency,k.col6 as no_of_strock,k.client_id,k.client_unit_id,k.vch_num,k.ent_by,k.ent_date,k.edit_by,k.edit_date,k.rec_id " +
                    //  "from kc_tab k where k.client_id|| k.client_unit_id|| k.vch_num|| to_char(k.vch_date, 'yyyymmdd')|| k.type='" + URL + "'";
                    mq = "select k.col7, k.col31 as Mould_Name,k.col1 as Mould_no,k.client_id,k.col4 as Std_avgtym,k.col32 as Part,k.col2 as Part_No,k.col33 as Mould_cavity," +
                        "to_char(k.date1, '" + sgen.Getsqldateformat() + "') as mould_Date,k.col3 as Status, k.col5 as frequency,k.col6 as no_of_strock," +
                        "k.client_unit_id,k.vch_num,k.ent_by,k.ent_date,k.col8,k.edit_by,k.edit_date,k.rec_id,k.col10,to_char(k.date3, '" + sgen.Getsqldateformat() + "') date3," +
                         "k.col34,k.col11,k.col12,k.col13,k.col14,to_char(k.date4, '" + sgen.Getsqldateformat() + "') date4,k.col15,k.col16,k.col17,k.col18," +
                         "to_char(k.date5, '" + sgen.Getsqldateformat() + "') date5,to_char(k.date6, '" + sgen.Getsqldateformat() + "') date6,to_char(k.date7, '" + sgen.Getsqldateformat() + "') date7," +
                         "k.col19,k.col20,k.col21,col34 from kc_tab k " +
                        "where(k.client_id || k.client_unit_id || k.vch_num || to_char(k.vch_date, 'yyyymmdd') || k.type) = '" + URL + "'";
                    dtt = sgen.getdata(userCode, mq);
                    if (dtt.Rows.Count > 0)
                    {
                        model[0].Col17 = dtt.Rows[0]["Mould_Name"].ToString();
                        model[0].Col18 = dtt.Rows[0]["Mould_no"].ToString();
                        model[0].Col19 = dtt.Rows[0]["Part"].ToString();
                        model[0].Col20 = dtt.Rows[0]["Part_No"].ToString();
                        model[0].Col21 = dtt.Rows[0]["Mould_cavity"].ToString();
                        model[0].Col22 = dtt.Rows[0]["Status"].ToString();
                        model[0].Col23 = dtt.Rows[0]["Std_avgtym"].ToString();
                        model[0].Col24 = dtt.Rows[0]["mould_Date"].ToString();
                        model[0].Col25 = dtt.Rows[0]["frequency"].ToString();
                        model[0].Col26 = dtt.Rows[0]["no_of_strock"].ToString();
                        model[0].Col28 = dtt.Rows[0]["col8"].ToString();
                        model[0].Col16 = dtt.Rows[0]["vch_num"].ToString();
                        model[0].Col38 = dtt.Rows[0]["col10"].ToString();
                        model[0].Col39 = dtt.Rows[0]["date3"].ToString();
                        model[0].Col40 = dtt.Rows[0]["col34"].ToString();
                        model[0].Col41 = dtt.Rows[0]["col11"].ToString();
                        model[0].Col42 = dtt.Rows[0]["col12"].ToString();
                        model[0].Col43 = dtt.Rows[0]["col13"].ToString();
                        model[0].Col44 = dtt.Rows[0]["date4"].ToString();
                        model[0].Col45 = dtt.Rows[0]["col15"].ToString();
                        model[0].Col46 = dtt.Rows[0]["col16"].ToString();
                        model[0].Col47 = dtt.Rows[0]["col17"].ToString();
                        model[0].Col48 = dtt.Rows[0]["col18"].ToString();
                        model[0].Col49 = dtt.Rows[0]["date5"].ToString();
                        model[0].Col50 = dtt.Rows[0]["date6"].ToString();
                        model[0].Col51 = dtt.Rows[0]["date7"].ToString();
                        model[0].Col52 = dtt.Rows[0]["col19"].ToString();
                        model[0].Col53 = dtt.Rows[0]["col20"].ToString();
                        model[0].Col54 = dtt.Rows[0]["col21"].ToString();
                        #region Frequency
                        mod1 = cmd_Fun.freq(userCode, unitid_mst);
                        model[0].TList1 = mod1;
                        TempData[MyGuid + "_TList1"] = mod1;
                        String[] L1 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["col7"].ToString()).Distinct()).Split(',');
                        model[0].SelectedItems1 = L1;
                        #endregion
                        #region mcap
                        mod3 = cmd_Fun.mcap(userCode, unitid_mst);
                        model[0].TList3 = mod3;
                        TempData[MyGuid + "_TList3"] = mod3;
                        String[] L3 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["col14"].ToString()).Distinct()).Split(',');
                        model[0].SelectedItems3 = L3;
                        model[0].TList5 = mod3;
                        TempData[MyGuid + "_TList5"] = mod3;
                        String[] L5 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["col34"].ToString()).Distinct()).Split(',');
                        model[0].SelectedItems5 = L5;
                        #endregion
                        DataTable dtf = new DataTable();
                        dtf = sgen.getdata(userCode, "select rec_id,file_name,file_path,col1 from file_tab where" +
                            " ref_code='" + dtt.Rows[0]["vch_num"].ToString() + "' and type='MOS' and " +
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
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                DataTable dt = new DataTable();
                                DataRow dr = dtt.Rows[0];
                                model[0].Col8 = "(client_id || client_unit_id || vch_num || to_char(vch_date, 'yyyymmdd') || type) = '" + URL + "'";
                                model[0].Col3 = dr["ent_by"].ToString();
                                model[0].Col4 = dr["ent_date"].ToString();
                                model[0].Col16 = dr["vch_num"].ToString();
                                model[0].Col1 = dr["client_id"].ToString();
                                model[0].Col2 = dr["client_unit_id"].ToString();
                                model[0].Col7 = dr["rec_id"].ToString();
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
                        case "VIEW":
                            if (dtt.Rows.Count > 0)
                            {
                                ViewBag.scripCall = "disableForm();";
                            }
                            break;
                    }
                    break;
                #endregion
                #region prod_entry
                case "prod_entry":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select * from iprod where (client_id||client_unit_id||vch_num||to_char(vch_date,'yyyymmdd')||type)='" + URL + "'";
                            break;
                        case "ITEM":
                            mq = "select it.icode,it.iname,'-' Con,'-' From_Icode,'-' From_Iname,'-' FromStage,'-' FStgCode,'-'  ToStage,'-' TstgCode,'0' Qty_Planned,'0' Qty_Produce," +
                "'0' Qty_Consume,'-' Operator,'-' OpCode,'-' Machine,'-' MachineCode,'-' Mould,'-' MouldCode,'-' as Tool_StTime,'-' Tool_EdTime,'-' Remark" +
                " from item it where it.icode||to_char(it.vch_date,'yyyymmdd')||it.type in ('" + URL + "')";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                if (model[0].dt1.Rows[0]["icode"].ToString().Trim().Equals("-")) { model[0].dt1.Rows.RemoveAt(0); }
                                for (int i = 0; i < dtt.Rows.Count; i++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["SNo"] = i + 1;
                                    dr["Icode"] = dtt.Rows[i]["icode"].ToString();
                                    dr["IName"] = dtt.Rows[i]["iname"].ToString();
                                    model[0].dt1.Rows.Add(dr);
                                }
                                model[0].dt1.AcceptChanges();
                            }
                            break;
                        case "ITEMFROM":
                            mq = "select it.icode,it.iname from item it where it.icode||to_char(it.vch_date,'yyyymmdd')||it.type in ('" + URL + "')";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                model[0].dt1.Rows[sgen.Make_int(model[0].Col24)]["From_Icode"] = dtt.Rows[0]["icode"].ToString();
                                model[0].dt1.Rows[sgen.Make_int(model[0].Col24)]["From_Iname"] = dtt.Rows[0]["iname"].ToString();
                                model[0].dt1.AcceptChanges();
                            }
                            break;
                    }
                    break;
                #endregion
                #region Operator_master
                case "operator_master":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select ms.client_id,ms.client_unit_id,ms.master_editby,ms.master_editdate,ms.master_entby,ms.master_entdate,ms.master_id,ms.vch_num,ms.rec_id,ms.classid,ms.sectionid," +
                                "ms.master_name,dep.master_name dept, des.master_name des, ms.Status,b.First_name || ' ' || replace(b.middle_name, '0', '') || ' ' || b.last_name Ent_By from master_setting ms " +
                                "inner join user_details b on ms.master_entby = b.vch_num and b.type = 'CPR' " +
                                "inner join master_setting dep on dep.master_id = ms.classid and dep.type = 'MDP' " +
                                "inner join master_setting des on des.master_id = ms.sectionid and des.type = 'MDG' " +
                                "where(ms.client_id || ms.client_unit_id || ms.master_id || to_char(ms.vch_date, 'yyyymmdd') || ms.type) = '" + URL + "'";                        
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                #region binddept
                                mod1 = cmd_Fun.dept(userCode, clientid_mst, unitid_mst);
                                TempData[MyGuid + "_Tlist1"] = mod1;
                                #endregion
                                #region Binddesig
                                mod2 = cmd_Fun.desig(userCode, unitid_mst);
                                TempData[MyGuid + "_Tlist2"] = mod2;
                                #endregion
                                //foreach (DataRow dr in dtt.Rows)
                                //{
                                //    rpt_model.Add(new Tmodelmain
                                //    {
                                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dtt.Rows[0]["master_entby"].ToString();
                                model[0].Col4 = dtt.Rows[0]["master_entdate"].ToString();
                                model[0].Col5 = dtt.Rows[0]["master_editby"].ToString();
                                model[0].Col6 = dtt.Rows[0]["master_editdate"].ToString();
                                model[0].Col7 = dtt.Rows[0]["rec_id"].ToString();
                                model[0].Col8 = "client_id||client_unit_id||master_id|| to_char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";
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
                                model[0].Col16 = dtt.Rows[0]["vch_num"].ToString();
                                model[0].Col21 = dtt.Rows[0]["master_id"].ToString();
                                model[0].Col17 = dtt.Rows[0]["master_name"].ToString();
                                model[0].Col20 = dtt.Rows[0]["status"].ToString();
                                //    });
                                //}
                                String[] L1 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["classid"].ToString()).Distinct()).Split(',');
                                String[] L2 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["sectionid"].ToString()).Distinct()).Split(',');
                                model[0].TList1 = mod1;
                                model[0].TList2 = mod2;
                                model[0].SelectedItems1 = L1;
                                model[0].SelectedItems2 = L2;
                                //model = rpt_model;
                            }
                            break;
                    }
                    break;
                #endregion
                #region item_stage
                case "item_stage":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = @"select  ex.client_id,ex.client_unit_id, ex.vch_num,ex.col2,ex.rec_id,ex.col1,ex.col3,ex.col4,ex.ent_by,ex.ent_date from enx_tab2 ex
                           where ex.client_id|| ex.client_unit_id||ex.vch_num||to_char(ex.vch_date, 'yyyymmdd'),ex.type='" + URL + "'";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                #region  Stage
                                mod1 = cmd_Fun.prodstage(userCode, unitid_mst);
                                //DataTable dt = new DataTable();
                                //dt = sgen.getdata(userCode, "Select master_id,client_name from master_setting where type='KPS'and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
                                //if (dt.Rows.Count > 0)
                                //{
                                //    foreach (DataRow dr in dt.Rows)
                                //    {
                                //        mod1.Add(new SelectListItem { Text = dr["client_name"].ToString(), Value = dr["master_id"].ToString() });
                                //    }
                                TempData[MyGuid + "_Tlist1"] = mod1;
                                //}
                                #endregion
                                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                                model[0].Col8 = "client_id|| client_unit_id|| vch_num|| to_char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";
                                model[0].Col9 = tm.Col9;
                                model[0].Col10 = tm.Col10;
                                model[0].Col11 = tm.Col11;
                                model[0].Col12 = tm.Col12;
                                model[0].Col13 = "Update";
                                model[0].Col100 = "Update & New";
                                model[0].Col14 = tm.Col14;
                                model[0].Col15 = tm.Col15;
                                model[0].Col16 = dtt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dtt.Rows[0]["col1"].ToString();
                                model[0].Col19 = dtt.Rows[0]["col4"].ToString();
                                foreach (DataRow dr in dtt.Rows)
                                {
                                    rpt_model.Add(new Tmodelmain
                                    {
                                        Col20 = dr["col3"].ToString(),
                                        TList1 = mod1,
                                        SelectedItems1 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => dr["col2"].ToString()).Distinct()).Split(',')
                                    });
                                }
                                model[0].LTM1 = rpt_model;
                            }
                            break;
                        case "ITEM_NAME":
                            mq = @"select icode ,iname from item " +
                                "where client_id||icode|| client_unit_id|| to_char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                model[0].Col17 = dtt.Rows[0]["icode"].ToString();
                                model[0].Col19 = dtt.Rows[0]["iname"].ToString();
                            }
                            break;
                    }
                    break;
                #endregion
                #region shift_master
                case "shift_master":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = @"select ms.client_id,ms.client_unit_id,ms.master_editby,ms.master_editdate,ms.master_entby,
                                  ms.master_id,ms.vch_num,ms.rec_id,ms.classid as shift_type,ms.master_name,ms.Status,
                                 b.First_name|| ' '|| nvl(b.middle_name, '')|| ' '|| b.last_name as Ent_By,
                               to_char(convert_tz(ms.master_EntDate, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "')" +
                               " as master_EntDate  from master_setting ms inner join user_details b on ms.master_entby = b.vch_num and " +
                               "b.type = 'CPR' where ms.client_id||ms.client_unit_id|| ms.master_id||to_char(ms.vch_date, 'yyyymmdd')|| ms.type= '" + URL + "'";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dtt.Rows[0]["master_entby"].ToString();
                                model[0].Col4 = dtt.Rows[0]["master_entdate"].ToString();
                                model[0].Col5 = dtt.Rows[0]["master_editby"].ToString();
                                model[0].Col6 = dtt.Rows[0]["master_editdate"].ToString();
                                model[0].Col7 = dtt.Rows[0]["rec_id"].ToString();
                                model[0].Col8 = "client_id||client_unit_id||master_id||to_char(vch_date, 'yyyymmdd')|| type= '" + URL + "'";
                                model[0].Col9 = tm.Col9;
                                model[0].Col10 = tm.Col10;
                                model[0].Col11 = tm.Col11;
                                model[0].Col12 = tm.Col12;
                                model[0].Col13 = "Update";
                                model[0].Col100 = "Update & New";
                                model[0].Col14 = tm.Col14;
                                model[0].Col15 = tm.Col15;
                                model[0].Col16 = dtt.Rows[0]["master_id"].ToString();
                                model[0].Col21 = dtt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dtt.Rows[0]["shift_type"].ToString();
                                model[0].Col18 = dtt.Rows[0]["master_name"].ToString();
                                model[0].Col19 = dtt.Rows[0]["status"].ToString();
                            }
                            break;
                    }
                    break;
                #endregion
                #region machine_brkdwn
                case "machine_brkdwn":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            //mq = @"select cou_title as machine_name,cou_descp as location,cou_payment as reason,
                            //      to_char(convert_tz(occ_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Datetime," +
                            //      "course_id,cou_ent_by,cou_ent_date,cou_edit_by,cou_edit_date,rec_id,client_id,client_unit_id,vch_num " +
                            //      "from courses where client_id||client_unit_id||vch_num|| to_char(vch_date, 'yyyymmdd')|| type= '" + URL + "'";
                            mq = "select  c.cou_title as Machine_name,c.cou_descp as location,c.vch_num,c.course_id,c.cou_payment as reason,c.client_id,c.client_unit_id,c.cou_ent_by,c.cou_ent_date," +
                                "c.cou_edit_by,c.cou_edit_date,c.rec_id," +
                                "to_char(convert_tz(c.occ_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldatetimeformat() + "') as Datetime ," +
                                "to_char(convert_tz(C.COU_ENT_DATE, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') as Ent_Date ," +
                                "b.First_name || ' ' || nvl(b.middle_name, '') || ' ' || b.last_name as Ent_By from courses c " +
                                "inner join user_details b on c.COU_ENT_BY = b.vch_num and b.type = 'CPR' " +
                                "where c.client_id || c.client_unit_id || c.vch_num || to_char(c.vch_date, 'yyyymmdd') || c.type = '" + URL + "'";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                #region reason for breakup
                                //mq = "select master_id ,master_name from master_setting where type='BRM' and client_unit_id='" + unitid_mst + "'";
                                //DataTable dt = sgen.getdata(userCode, mq);
                                //if (dt.Rows.Count > 0)
                                //{
                                //    foreach (DataRow dr in dt.Rows)
                                //    {
                                //        mod1.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                                //    }
                                //}
                                mod1 = cmd_Fun.break_upreason(userCode, unitid_mst);
                                TempData[MyGuid + "_Tlist1"] = mod1;
                                #endregion
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dtt.Rows[0]["cou_ent_by"].ToString();
                                model[0].Col4 = dtt.Rows[0]["cou_ent_date"].ToString();
                                model[0].Col5 = dtt.Rows[0]["cou_edit_by"].ToString();
                                model[0].Col6 = dtt.Rows[0]["cou_edit_date"].ToString();
                                model[0].Col7 = dtt.Rows[0]["rec_id"].ToString();
                                model[0].Col8 = "client_id || client_unit_id || vch_num || to_char(vch_date, 'yyyymmdd') || type = '" + URL + "'";
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
                                model[0].Col21 = dtt.Rows[0]["course_id"].ToString();
                                model[0].Col16 = dtt.Rows[0]["vch_num"].ToString();
                                model[0].TList1 = mod1;
                                model[0].Col17 = dtt.Rows[0]["machine_name"].ToString();
                                model[0].Col18 = dtt.Rows[0]["location"].ToString();
                                String[] L1 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["reason"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems1 = L1;
                                //model[0].Col19 = dtt.Rows[0]["reason"].ToString();
                                model[0].Col20 = dtt.Rows[0]["Datetime"].ToString();
                            }
                            break;
                    }
                    break;
                #endregion
                #region bom
                case "bom":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                        case "VIEW":
                        case "COPYBM":
                            mq = @"select to_char(convert_tz(vch_date, 'UTC', '" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') as bom_date,acode,icode,cond,iname,uom as puom," +
                                "qtyin reqqty,qtyout altqty,pono stgcode,irate bomsz,tmode mldcode,req_by mld,client_id,client_unit_id,ent_by,ent_date,edit_by,edit_date,vch_num,rno from itransactionc " +
                                "where (client_id||client_unit_id||vch_num||to_char(vch_date,'yyyymmdd')||type)='" + URL + "'";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                if (btnval.ToUpper().Equals("EDIT") || btnval.ToUpper().Equals("VIEW"))
                                {
                                    sgen.SetSession(MyGuid, "EDMODE", "Y");
                                    model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                    model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                    model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                                    model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                                    model[0].Col5 = dtt.Rows[0]["edit_by"].ToString();
                                    model[0].Col6 = dtt.Rows[0]["edit_date"].ToString();
                                    model[0].Col8 = "(client_id||client_unit_id||vch_num||to_char(vch_date,'yyyymmdd')||type) = '" + URL + "'";
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
                                    model[0].Col16 = dtt.Rows[0]["vch_num"].ToString();
                                    model[0].Col17 = dtt.Rows[0]["bom_date"].ToString();
                                    mq = "select it.icode as Icode,it.iname as Iname,pum.master_name as UOM from item it " +
                                        "inner join master_setting pum on pum.master_id=it.uom and pum.type='UMM' and find_in_set(pum.client_unit_id,'" + unitid_mst + "') =1 " +
                                        "where find_in_set(it.client_unit_id,'" + unitid_mst + "')=1 and it.type='IT' and it.icode='" + dtt.Rows[0]["acode"].ToString() + "'";
                                    DataTable dtm = sgen.getdata(userCode, mq);
                                    if (dtm.Rows.Count > 0)
                                    {
                                        model[0].Col18 = dtm.Rows[0]["icode"].ToString();
                                        model[0].Col19 = dtm.Rows[0]["icode"].ToString();
                                        model[0].Col20 = dtm.Rows[0]["Iname"].ToString();
                                        model[0].Col23 = dtm.Rows[0]["UOM"].ToString();
                                    }
                                }
                                else
                                {
                                    sgen.SetSession(MyGuid, "EDMODE", "N");
                                }
                                model[0].Col21 = dtt.Rows[0]["bomsz"].ToString();
                                model[0].dt1 = new DataTable();
                                model[0].dt1 = ((DataTable)sgen.GetSession(MyGuid, "dtbom")).Clone();
                                for (int i = 0; i < dtt.Rows.Count; i++)
                                {
                                    DataView dv = dtt.DefaultView;
                                    dv.Sort = "rno";
                                    dtt = dv.ToTable();
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["Sno"] = i + 1;
                                    dr["Icode"] = dtt.Rows[i]["icode"].ToString();
                                    dr["IName"] = dtt.Rows[i]["iname"].ToString();
                                    dr["Puom"] = dtt.Rows[i]["puom"].ToString();//puom
                                    dr["Req_qty"] = dtt.Rows[i]["reqqty"].ToString();//4 
                                    dr["Alt_qty"] = dtt.Rows[i]["altqty"].ToString();//5                                      
                                    dr["Stg_code"] = dtt.Rows[i]["stgcode"].ToString();//stgcode
                                    dr["Stage"] = dtt.Rows[i]["cond"].ToString();//stg                                   
                                    dr["Mld_code"] = dtt.Rows[i]["mldcode"].ToString();//stg                                   
                                    dr["Mld"] = dtt.Rows[i]["mld"].ToString();//stg                                   
                                    model[0].dt1.Rows.Add(dr);
                                }
                                model[0].dt1.AcceptChanges();
                            }
                            break;
                        case "ITEM":
                            mq = "select it.icode as Icode,it.iname as Iname,pum.master_name as UOM from item it " +
                                "inner join master_setting pum on pum.master_id=it.uom and pum.type='UMM' and find_in_set(pum.client_unit_id,'" + unitid_mst + "')=1 " +
                                " where (it.client_id||it.client_unit_id||it.icode) = '" + URL + "'";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                model[0].Col18 = dtt.Rows[0]["Icode"].ToString();
                                model[0].Col19 = dtt.Rows[0]["Icode"].ToString();
                                model[0].Col20 = dtt.Rows[0]["Iname"].ToString();
                                model[0].Col23 = dtt.Rows[0]["UOM"].ToString();
                            }
                            break;
                        case "BOMITEM":
                            mq = "select it.icode as icode,it.iname as iname,pum.master_name as puom,'0' qty_req,'0' as alt_qty,'-' stage from item it " +
                                "inner join master_setting pum on pum.master_id=it.uom and pum.type='UMM' and find_in_set(pum.client_unit_id,'" + unitid_mst + "')=1 " +
                                "where (it.client_id||it.client_unit_id||it.icode) in ('" + URL + "')";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                try
                                {
                                    if (model[0].dt1.Rows[0]["icode"].ToString().Trim().Equals("-")) { model[0].dt1.Rows.RemoveAt(0); }
                                    else
                                    {
                                        if (model[0].Col30 == "Y")
                                        {
                                            if (model[0].dt1.Rows[0]["icode"].ToString().Trim() != "-" && model[0].dt1.Rows[0]["icode"].ToString().Trim() != "")
                                            {
                                                model[0].dt1.Rows.RemoveAt(0);
                                            }
                                        }
                                    }
                                    for (int i = 0; i < dtt.Rows.Count; i++)
                                    {
                                        DataRow dr = model[0].dt1.NewRow();
                                        dr["SNo"] = i + 1;
                                        dr["Icode"] = dtt.Rows[i]["icode"].ToString();
                                        dr["IName"] = dtt.Rows[i]["iname"].ToString();
                                        dr["PUOM"] = dtt.Rows[i]["puom"].ToString();
                                        dr["req_qty"] = dtt.Rows[i]["qty_req"].ToString();
                                        dr["alt_qty"] = dtt.Rows[i]["alt_qty"].ToString();
                                        dr["stage"] = dtt.Rows[i]["stage"].ToString();
                                        model[0].dt1.Rows.Add(dr);
                                    }
                                    model[0].dt1.AcceptChanges();
                                }
                                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1," + ex.Message.ToString() + ",0);"; }
                            }
                            break;
                        case "PRINT":
                            mq = @"select (b.client_id||b.client_unit_id||b.vch_num||to_char(b.vch_date,'yyyymmdd')||b.type) fstr,b.vch_num,to_char(convert_tz(b.vch_date, 'UTC', '" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') vch_date," +
                                "b.acode micode,m.iname miname,um.master_name muom,b.irate lotsize,b.icode,i.iname,ui.master_name uom,b.qtyin reqqty," +
                                "b.qtyout altqty,b.pono stgcode,s.master_name stg from itransactionc b " +
                                "inner join item m on m.icode=b.acode and m.type='IT' and find_in_set(m.client_unit_id,b.client_unit_id)=1 " +
                                "inner join master_setting um on um.master_id=m.uom and um.type='UMM' and find_in_set(um.client_unit_id,b.client_unit_id)=1 " +
                                "inner join item i on i.icode=b.icode and i.type='IT' and find_in_set(i.client_unit_id,b.client_unit_id)=1 " +
                                "inner join master_setting ui on ui.master_id=i.uom and ui.type='UMM' and find_in_set(ui.client_unit_id,b.client_unit_id)=1 " +
                                "inner join master_setting s on s.master_id=b.pono and s.type='KPS' " +
                                "where (b.client_id||b.client_unit_id||b.vch_num||to_char(b.vch_date,'yyyymmdd')||b.type) in ('" + URL + "')";
                            DataSet ds = new DataSet();
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                dtt.TableName = "prepcur";
                                ds.Tables.Add(dtt);
                                sgen.open_report_byDs_xml(userCode, ds, "bom_report", "BOM Detail");
                                ViewBag.scripCall += "showRptnew('BOM Detail');disableForm();";

                                //ViewBag.scripCall = "OpenSingle('../../../../../erp/schoolReports_Rpts/Filter_Report_xml.aspx','95%','900px','Reports');";
                                //ViewBag.scripCall += "disableForm();";
                            }
                            break;
                    }
                    break;
                #endregion
                #region p_order
                case "p_ord":
                    switch (btnval.ToUpper())
                    {
                        case "NEW":
                            mq = "select master_id as ID,master_name as Invoice_Against from master_setting where (master_id||to_char(vch_date,'yyyymmdd')||type) = '" + URL + "'";
                            DataTable dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model[0].Col40 = dt.Rows[0]["ID"].ToString();
                                //Session["SO_type"] = dt.Rows[0]["master_id"].ToString();
                                sgen.SetSession(MyGuid, "PO_TYPE", dt.Rows[0]["ID"].ToString());
                                mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12 + "'" + model[0].Col11.Trim() + "";
                                //mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + dtt.Rows[0]["master_id"].ToString() + "'" + model[0].Col11.Trim() + "";
                                vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                                sgen.SetSession(MyGuid, "EDMODE", "N");
                                ViewBag.vnew = "disabled='disabled'";
                                ViewBag.vedit = "disabled='disabled'";
                                ViewBag.vsave = "";
                                ViewBag.vsavenew = "";
                                ViewBag.scripCall = "enableForm();";
                                model[0].Col13 = "Save";
                                model[0].Col100 = "Save & New";
                                model[0].Col16 = vch_num;
                                model[0].Col9 = dt.Rows[0]["Invoice_Against"].ToString();
                                if (model[0].Col40 == "70")
                                {
                                    Make_query(viewName.ToLower(), "Select Sales Order no.", "GETSO", "2", "", "",MyGuid);
                                    ViewBag.scripCall = "callFoo('Select Sales Order no.');";
                                }
                                else
                                {
                                    dt = sgen.getdata(userCode, "select '' as Item, '1' as SNo ,'-' as Icode,'-' as IName,'-' PartNo,'-' as UOM,'0' as Order_qty,'0' as Plan_qty," +
"'-' as prd_start_dt,'-' as prd_end_dt,'-' as Remark from dual");
                                    model[0].dt1 = dt;
                                    sgen.SetSession(MyGuid, "P_ORD_DT", dt.Copy());
                                }
                            }
                            break;
                        case "GETSO":
                            mq = "select group_concat(distinct a.vch_num) as so_no,a.type from purchasesc a where (a.client_id || a.client_unit_id || a.vch_num || to_char(a.vch_date, 'yyyymmdd') || a.type) in ('" + URL + "') group by (a.type) ";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                model[0].Col29 = dt.Rows[0]["so_no"].ToString();
                                sgen.SetSession(MyGuid, "So_num", dt.Rows[0]["so_no"].ToString());
                            }
                            string[] xfstr = URL.Split(',');
                            //if (xfstr[0].Length > 26)
                            //{
                            string xfstrnew = xfstr[0].Substring(0, 25);
                            //}
                            //else { xfstrnew = xfstr[0]; }
                            mq1 = "select distinct to_char(a.vch_date,'" + sgen.Getsqldateformat() + "') as so_date from purchasesc a where (a.client_id || a.client_unit_id || a.vch_num || to_char(a.vch_date, 'yyyymmdd') " +
                                "|| a.type) in ('" + xfstrnew + "') ";
                            dt = sgen.getdata(userCode, mq1);
                            if (dt.Rows.Count > 0)
                            {
                                model[0].Col31 = dt.Rows[0]["so_date"].ToString();
                                //Session["So_num"] = dt.Rows[0]["so_no"].ToString();
                                Make_query(viewName.ToLower(), "Select Item", "ITEM", "2", "", "",MyGuid);
                                ViewBag.scripCall = "callFoo('Select Item');";
                            }
                            break;
                        case "EDIT":
                            //mq = " select sd.vch_num as Doc_no,sd.rec_id,sd.client_id,sd.client_unit_id,sd.ent_by,sd.ent_date,sd.edit_by,sd.edit_date,sd.col1 as pt_code,sd.type," +
                            //    "to_char(convert_tz(sd.vch_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') as Doc_date,sd.col2 as Sdl_no," +
                            //    "sd.col3 as PO_no1,sd.col4 as So_no,(case when sd.col20='70' then 'WITH SALES ORDER' when sd.col20='70' then 'WITHOUT SALES ORDER' end) as so_tp," +
                            //    "sd.col21 as Remark1,to_char(convert_tz(sd.date2,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') as sdl_st_date," +
                            //    "sd.col20 as so_tp,to_char(convert_tz(sd.date3,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') as sdl_end_date," +
                            //    "to_char(convert_tz(sd.date6 ,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') as So_dt2" +
                            //    ",to_char(convert_tz(sd.date7 ,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') as So_dt1," +
                            //    "to_char(convert_tz(sd.date8 ,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') as prd_start_dt," +
                            //    "to_char(convert_tz(sd.date9 ,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') as prd_en_dt,sd.col5 as Icode," +
                            //    "sd.col27 as IName,sd.col7 as PartNo, sd.col8 as UOM, sd.col9 as Order_qty,sd.col10 as Plan_qty,sd.col11 as PO_No2," +
                            //    "sd.col12 as remark2,sd.col13 as So_no2 from kc_tab sd where (sd.client_id || sd.client_unit_id || sd.vch_num || to_char(sd.vch_date, 'yyyymmdd') || sd.type) = '" + URL + "'";
                            mq = "select sd.vch_num as Doc_no,sd.rec_id,sd.client_id,sd.client_unit_id,sd.ent_by,sd.ent_date,sd.edit_by,sd.edit_date," +
                                "sd.type,to_char(convert_tz(sd.vch_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') as Doc_date,sd.col2 as Sdl_no,sd.col3 as PO_no1," +
                                "sd.col4 as So_no,(case when sd.col20 = '70' then 'WITH SALES ORDER' when sd.col20 = '71' then 'WITHOUT SALES ORDER' else '-' end)" +
                                " as so_type,sd.col21 as Remark1,to_char(sd.date2, '" + sgen.Getsqldateformat() + "') as sdl_st_date,sd.col20 as so_tp," +
                                "to_char(sd.date3, '" + sgen.Getsqldateformat() + "') as sdl_end_date,to_char(sd.date6,'" + sgen.Getsqldateformat() + "') as So_dt2,to_char(sd.date7, '" + sgen.Getsqldateformat() + "') as So_dt1," +
                                "to_char(sd.date8, '" + sgen.Getsqldateformat() + "') as prd_start_dt,to_char(sd.date9, '" + sgen.Getsqldateformat() + "') " +
                                "as prd_en_dt,sd.col5 as Icode,sd.col27 as IName,sd.col7 as PartNo,sd.col8 as UOM, sd.col9 as Order_qty,sd.col10 as Plan_qty," +
                                "sd.col11 as PO_No2,sd.col12 as remark2,sd.col13 as So_no2 from kc_tab sd where " +
                                "(sd.client_id || sd.client_unit_id || sd.vch_num || to_char(sd.vch_date, 'yyyymmdd') || sd.type) = '" + URL + "'";
                            dt = sgen.getdata(userCode, mq);
                            if (dt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col12 = dt.Rows[0]["type"].ToString();
                                model = getedit(model, dt, URL);
                                model[0].Col16 = dt.Rows[0]["Doc_no"].ToString();
                                //model[0].Col23 = dt.Rows[0]["Sdl_no"].ToString();
                                model[0].Col28 = dt.Rows[0]["Remark1"].ToString();
                                model[0].Col40 = dt.Rows[0]["so_tp"].ToString();
                                sgen.SetSession(MyGuid, "PO_TYPE", dt.Rows[0]["so_tp"].ToString());
                                if (model[0].Col40 == "70")
                                {
                                    model[0].Col29 = dt.Rows[0]["So_no"].ToString();
                                    model[0].Col31 = dt.Rows[0]["So_dt1"].ToString();
                                    sgen.SetSession(MyGuid, "So_num", dt.Rows[0]["So_no"].ToString());
                                }
                                model[0].Col24 = dt.Rows[0]["sdl_st_date"].ToString();
                                model[0].Col25 = dt.Rows[0]["sdl_end_date"].ToString();
                                //model[0].Col27 = dt.Rows[0]["po_date"].ToString();
                                ////model[0].Col31 = dt.Rows[0]["So_dt1"].ToString();
                                //model[0].Col26 = dt.Rows[0]["PO_no1"].ToString();
                                model[0].Col9 = dt.Rows[0]["so_type"].ToString();
                                //model[0].Col9 = sgen.seekval(userCode, "select upper(master_name) master_name from master_setting where master_id='" + dt.Rows[0]["so_tp"].ToString() + "' and type='KPO'", "master_name");
                                if (model[0].Col40 == "70")
                                {
                                    DataTable dt2 = sgen.getdata(userCode, "select '' as Item, '1' as SNo ,'-' as Icode,'-' as IName,'-' PartNo,'-' as UOM,'0' as Order_qty,'0' as Plan_qty," +
"'-' as prd_start_dt,'-' as prd_end_dt,'-' So_no,'-' So_date,'-' as Remark from dual");
                                    model[0].dt1 = dt2;
                                    sgen.SetSession(MyGuid, "P_ORD_DT", dt2.Copy());
                                }
                                else
                                {
                                    DataTable dt2 = sgen.getdata(userCode, "select '' as Item, '1' as SNo ,'-' as Icode,'-' as IName,'-' PartNo,'-' as UOM,'0' as Order_qty,'0' as Plan_qty," +
"'-' as prd_start_dt,'-' as prd_end_dt,'-' as Remark from dual");
                                    model[0].dt1 = dt2;
                                    sgen.SetSession(MyGuid, "P_ORD_DT", dt2.Copy());
                                }
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
                                    if (model[0].Col40 == "70")
                                    {
                                        dr["SO_No"] = dt.Rows[i]["So_no2"].ToString();
                                        dr["SO_Date"] = dt.Rows[i]["So_dt2"].ToString();
                                    }
                                    dr["prd_start_dt"] = dt.Rows[i]["prd_start_dt"].ToString();
                                    dr["prd_end_dt"] = dt.Rows[i]["prd_en_dt"].ToString();
                                    dr["Remark"] = dt.Rows[i]["remark2"].ToString();
                                    model[0].dt1.Rows.Add(dr);
                                }
                                model[0].dt1.AcceptChanges();
                            }
                            break;
                        case "ITEM":
                            if (sgen.GetSession(MyGuid, "PO_TYPE").ToString().Equals("70"))
                            {
                                mq = "select '-' as fstr,it.icode as Icode,it.iname as Iname,it.cpartno Partno,pc.hsno as HSN,pc.uom as UOM,kt.plan_qty," +
                                    "(pc.qtyord-nvl(kt.plan_qty,0)) as Order_qty,pc.gothrchrg as po_num,to_char(pc.date1,'" + sgen.Getsqldateformat() + "') as po_date" +
                                    ",to_char(pc.dlv_date,'" + sgen.Getsqldateformat() + "') dlv_date, to_char(pc.vch_date,'" + sgen.Getsqldateformat() + "') as so_date,pc.vch_num as so_no " +
                                    "from item it inner join purchasesc pc on pc.icode = it.icode and pc.pur_type = 'NPI' and " +
                                    "find_in_set(pc.client_unit_id, it.client_unit_id)=1 left join(select sum(col10) as plan_qty ,col4 from kc_tab " +
                                    "where client_unit_id = '" + unitid_mst + "' and type = 'PSO' group by col4) kt on kt.col4 = pc.vch_num" +
                                    " where(it.icode || pc.type || pc.vch_num || to_char(it.vch_date, 'yyyymmdd') || it.type) in ('" + URL + "')";
                            }
                            else
                            {
                                mq = "select distinct '-' fstr,it.icode,it.iname,it.cpartno as partno,um.master_name as UOM,hsn.master_name as hsn,hsn.group_name taxrate, " +
                                    "nvl(s.closing, 0) AS qtystk,'0' qtyord,'0' ind_no,'-' ind_date from item it left join master_setting um on um.master_id = it.uom and " +
                                    "um.type = 'UMM' and find_in_set(um.client_unit_id,it.client_unit_id)=1 left join master_setting hsn on hsn.master_id = it.hsno and hsn.type = 'HSN' " +
                                    " and find_in_set(hsn.client_unit_id,it.client_unit_id)=1 left join(select t.client_unit_id, t.icode, sum(nvl(t.qtyin,0)) as Received,sum(nvl(t.qtyout, 0)) " +
                                    "as Issued,sum(nvl(t.qtyin, 0)) - sum(nvl(t.qtyout, 0)) as closing from itransaction t where trim(t.store) = 'Y' group by t.icode,t.client_unit_id )" +
                                    " s on it.icode = s.icode and find_in_set(it.client_unit_id, s.client_unit_id)=1 where " +
                                    "it.icode || to_char(it.vch_date, 'yyyymmdd') || it.type in ('" + URL + "')";
                            }
                            //mq = "select '-' as fstr,it.icode as Icode,it.iname as Iname,it.cpartno Partno,hsn.master_name as HSN,nvl(pc.bal_qty, '0') as Order_qty," +
                            //    "um.master_name as UOM,pc.po_no as po_num,to_char(pc.date1, '" + sgen.Getsqldateformat() + "') as po_date,to_char" +
                            //    "(pc.vch_date, '" + sgen.Getsqldateformat() + "') as so_date,pc.vch_num as so_no from item it inner join (select a.po_no, a.vch_num, a.icode, a.type, " +
                            //    "a.so_qty,a.vch_date, a.date1,nvl(sum(a.invoice_qty), '0') as used_qty,a.so_qty - nvl(sum(a.invoice_qty), '0') as bal_qty from " +
                            //    "(select pc.vch_num, pc.gothrchrg as po_no, pc.date1, pc.vch_date, pc.icode, pc.type, pc.qtyord as so_qty, iv.qtyout as invoice_qty from purchasesc pc " +
                            //    "left join itransaction iv on iv.sono = pc.vch_num and iv.type = '51' and iv.client_id = pc.client_id and iv.client_unit_id = pc.client_unit_id " +
                            //    "where pc.pur_type = 'NPI' and substr(pc.type, 1, 1) = '4' and pc.client_id = '" + clientid_mst + "' and pc.client_unit_id = '" + unitid_mst + "' union all " +
                            //    "select pc.vch_num, pc.gothrchrg as po_no, pc.date1, pc.vch_date, pc.icode, pc.type, pc.qtyord as so_qty, dp.qtyout as dsp_qty from purchasesc pc " +
                            //    "left join itransaction dp on dp.VEHNO = pc.vch_num and dp.type = '60' and dp.client_id = pc.client_id and dp.client_unit_id = pc.client_unit_id" +
                            //    " where pc.pur_type = 'NPI' and substr(pc.type, 1, 1) = '4' union all select pc.vch_num, pc.gothrchrg as po_no, pc.date1,pc.vch_date, pc.icode, " +
                            //    "pc.type, pc.qtyord as so_qty, ds.col10 as dsp_qty from purchasesc pc left join kc_tab ds on ds.col13 = pc.vch_num and ds.type = 'DSC' and" +
                            //    " ds.client_id = pc.client_id and ds.client_unit_id = pc.client_unit_id where pc.pur_type = 'NPI' and substr(pc.type, 1, 1) = '4') a group" +
                            //    " by(a.vch_num, a.icode, a.type, a.so_qty, a.po_no, a.vch_date, a.date1)) pc on pc.icode = it.icode left join master_setting um on" +
                            //    " um.master_id = it.uom and um.type = 'UMM' and find_in_set(um.client_id, it.client_id) = 1 and find_in_set(um.client_unit_id, it.client_unit_id)= 1 " +
                            //    "left join master_setting hsn on hsn.master_id = it.hsno and hsn.type = 'HSN' and find_in_set(hsn.client_id, it.client_id)= 1 and " +
                            //    "find_in_set(hsn.client_unit_id, it.client_unit_id)= 1 " +
                            //    "where(it.client_id || it.client_unit_id || it.icode || pc.type || pc.vch_num || to_char(it.vch_date, 'yyyymmdd') || it.type) in ('" + URL + "')";
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
                                    if (model[0].Col40 == "70")
                                    {
                                        dr["Order_qty"] = dt.Rows[i]["Order_qty"].ToString();
                                        dr["SO_No"] = dt.Rows[i]["so_no"].ToString();
                                        dr["SO_Date"] = dt.Rows[i]["so_date"].ToString();
                                    }
                                    else
                                    {
                                        dr["Order_qty"] = "-";
                                    }
                                    //dr["PO_No"] = dt.Rows[i]["po_num"].ToString();
                                    //dr["PO_Date"] = dt.Rows[i]["po_date"].ToString();
                                    model[0].dt1.Rows.Add(dr);
                                }
                                model[0].dt1.AcceptChanges();
                            }
                            break;
                    }
                    break;
                #endregion
                #region mrp_gen
                case "mrp_gen":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = @"select to_char(convert_tz(vch_date, 'UTC', '" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') as bom_date,acode,icode,iname,uom puom," +
                                "cpartno suom,hsno cuom,qtychl qtyreq,qtyin qtyrej,irate bomsz,client_id,client_unit_id,ent_by,ent_date,edit_by,edit_date,vch_num from itransaction " +
                                "where (client_id||client_unit_id||vch_num||to_char(vch_date,'yyyymmdd')||type)='" + URL + "'";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                                model[0].Col5 = dtt.Rows[0]["edit_by"].ToString();
                                model[0].Col6 = dtt.Rows[0]["edit_date"].ToString();
                                model[0].Col8 = "(client_id||client_unit_id||vch_num||to_char(vch_date,'yyyymmdd')||type) = '" + URL + "'";
                                model[0].Col9 = tm.Col9;
                                model[0].Col10 = tm.Col10;
                                model[0].Col11 = tm.Col11;
                                model[0].Col13 = "Update";
                                model[0].Col100 = "Update & New";
                                model[0].Col14 = tm.Col14;
                                model[0].Col15 = tm.Col15;
                                model[0].Col16 = dtt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dtt.Rows[0]["bom_date"].ToString();
                                model[0].Col21 = dtt.Rows[0]["bomsz"].ToString();
                                mq = "select it.icode as Icode,it.iname as Iname from item it where it.type='IT' and find_in_set(it.client_unit_id,'" +
                                    unitid_mst + "')=1 and it.icode='" + dtt.Rows[0]["acode"].ToString() + "'";
                                DataTable dtm = sgen.getdata(userCode, mq);
                                if (dtm.Rows.Count > 0)
                                {
                                    model[0].Col18 = dtm.Rows[0]["icode"].ToString();
                                    model[0].Col19 = dtm.Rows[0]["icode"].ToString();
                                    model[0].Col20 = dtm.Rows[0]["Iname"].ToString();
                                }
                                model[0].dt1 = new DataTable();
                                model[0].dt1 = ((DataTable)sgen.GetSession(MyGuid, "dtbom")).Clone();
                                for (int i = 0; i < dtt.Rows.Count; i++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["sno"] = i.ToString();
                                    dr["Icode"] = dtt.Rows[i]["icode"].ToString();
                                    dr["IName"] = dtt.Rows[i]["iname"].ToString();
                                    dr["PUOM"] = dtt.Rows[i]["puom"].ToString();//puom
                                    dr["SUOM"] = dtt.Rows[i]["suom"].ToString();//suom
                                    dr["CUOM"] = dtt.Rows[i]["cuom"].ToString();//cuom                                                                        
                                    dr["Qty_Req"] = dtt.Rows[i]["qtyreq"].ToString();
                                    dr["Qty_Rej"] = dtt.Rows[i]["qtyrej"].ToString();
                                    model[0].dt1.Rows.Add(dr);
                                }
                            }
                            break;
                        case "ITEM":
                            mq = "select it.icode as Icode,it.iname as Iname from item it where (it.icode||to_char(it.vch_date, 'yyyymmdd')||it.type) = '" + URL + "'";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                model[0].Col18 = dtt.Rows[0]["Icode"].ToString();
                                model[0].Col19 = dtt.Rows[0]["Icode"].ToString();
                                model[0].Col20 = dtt.Rows[0]["Iname"].ToString();
                            }
                            break;
                        case "BOMITEM":
                            mq = "select it.icode as icode,it.iname as iname,pum.master_name as puom,suom.master_name as suom,it.cuom,'0' qty_req,'0' as qty_rej " +
                                "from item it inner join master_setting pum on pum.master_id=it.uom and pum.type='UMM' and find_in_set(pum.client_unit_id,'" + unitid_mst + "')=1 " +
                                "inner join master_setting suom on suom.master_id=it.uom and suom.type='UMM' and find_in_set(suom.client_unit_id,'" + unitid_mst + "')=1 " +
                                "where (it.icode||to_char(it.vch_date,'yyyymmdd')||it.type) in ('" + URL + "')";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                try
                                {
                                    if (model[0].dt1.Rows[0]["icode"].ToString().Trim().Equals("-")) { model[0].dt1.Rows.RemoveAt(0); }
                                    for (int i = 0; i < dtt.Rows.Count; i++)
                                    {
                                        DataRow dr = model[0].dt1.NewRow();
                                        dr["SNo"] = i + 1;
                                        dr["Icode"] = dtt.Rows[i]["icode"].ToString();
                                        dr["IName"] = dtt.Rows[i]["iname"].ToString();
                                        dr["PUOM"] = dtt.Rows[i]["puom"].ToString();
                                        dr["SUOM"] = dtt.Rows[i]["suom"].ToString();
                                        dr["CUOM"] = dtt.Rows[i]["cuom"].ToString();
                                        dr["Qty_Req"] = dtt.Rows[i]["qty_req"].ToString();
                                        dr["Qty_Rej"] = dtt.Rows[i]["qty_rej"].ToString();
                                        model[0].dt1.Rows.Add(dr);
                                    }
                                    model[0].dt1.AcceptChanges();
                                }
                                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1," + ex.Message.ToString() + ",0);"; }
                            }
                            break;
                    }
                    break;
                #endregion
                #region iprod
                case "iprod":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select ex.vch_num,ex.type,to_char(convert_tz(ex.vch_date, 'UTC', '" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') pddate," +
                                "to_char(convert_tz(ex.date1,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') date1," +
                                "to_char(convert_tz(ex.date2,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') date2," +
                                "to_char(convert_tz(ex.date3,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') date3,ex.acode," +
                             "ex.mstage fstg,ex.stage1 tstg,ex.opcode,ex.mccode,ex.mouldcode,ex.shftno,ex.qtyin pqty,ex.qtyrej,ex.icode,it.iname,um.master_name uom,ex.qtychl estcon_qty," +
                             "ex.qtyout actcon_qty,ex.qtyplanned shortex,ex.client_id,ex.client_unit_id,ex.ent_by,ex.ent_date,ex.stime,ex.etime,ex.pflag,ex.rno,ex.qtystk,ex.cstk,ex.stage," +
                             "ex.alt,ex.qtyrw,ex.pdono,to_char(convert_tz(ex.pdodt, 'UTC', '" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') pdodt,ex.qtypdo," +
                             "ex.col1,ex.col2,ex.col3,ex.col4,ex.col5,ex.col6,ex.mctxt,ex.mouldtxt " +
                             "from iprod ex " +
                             "inner join item it on it.icode=ex.icode and it.type='IT' and find_in_set(it.client_unit_id,ex.client_unit_id)=1 " +
                             "inner join master_setting um on um.master_id=it.uom and um.type='UMM' and find_in_set(um.client_unit_id,it.client_unit_id)=1 " +
                             "where (ex.client_id||ex.client_unit_id||ex.vch_num||to_char(ex.vch_date,'yyyymmdd')||ex.type)='" + URL + "'";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                //op name
                                mod3 = cmd_Fun.opname(userCode, unitid_mst);
                                TempData[MyGuid + "_TList3"] = mod3;
                                //machine
                                mod4 = cmd_Fun.mcname(userCode, unitid_mst);
                                TempData[MyGuid + "_TList4"] = mod4;
                                //mould
                                //mod5 = cmd_Fun.mldname(userCode, unitid_mst);
                                //TempData[MyGuid + "_TList5"] = mod5;
                                //shift
                                mod6 = cmd_Fun.shftname(userCode, unitid_mst);
                                TempData[MyGuid + "_TList6"] = mod6;
                                //iloc  
                                mod7 = mod4;
                                TempData[MyGuid + "_TList7"] = mod7;
                                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                                model[0].Col8 = "(client_id||client_unit_id||vch_num||to_char(vch_date,'yyyymmdd')||type) = '" + URL + "'";
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
                                model[0].Col12 = dtt.Rows[0]["type"].ToString();
                                model[0].Col16 = dtt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dtt.Rows[0]["pddate"].ToString();
                                model[0].Col25 = dtt.Rows[0]["stime"].ToString();
                                model[0].Col26 = dtt.Rows[0]["etime"].ToString();
                                model[0].Col29 = dtt.Rows[0]["cstk"].ToString();
                                model[0].Col99 = dtt.Rows[0]["rno"].ToString();
                                model[0].Col101 = dtt.Rows[0]["pflag"].ToString();
                                model[0].Col98 = dtt.Rows[0]["tstg"].ToString();
                                model[0].Col23 = dtt.Rows[0]["pqty"].ToString();
                                model[0].Col30 = dtt.Rows[0]["qtyrej"].ToString();
                                model[0].Col31 = dtt.Rows[0]["qtyrw"].ToString();
                                model[0].Col32 = dtt.Rows[0]["pdono"].ToString();
                                model[0].Col33 = dtt.Rows[0]["pdodt"].ToString();
                                model[0].Col34 = dtt.Rows[0]["qtypdo"].ToString();
                                model[0].Col35 = dtt.Rows[0]["col1"].ToString();
                                model[0].Col36 = dtt.Rows[0]["col2"].ToString();
                                model[0].Col37 = dtt.Rows[0]["col3"].ToString();
                                model[0].Col38 = dtt.Rows[0]["date1"].ToString();
                                model[0].Col39 = dtt.Rows[0]["date2"].ToString();
                                model[0].Col40 = dtt.Rows[0]["date3"].ToString();
                                model[0].Col41 = dtt.Rows[0]["col4"].ToString();
                                model[0].Col42 = dtt.Rows[0]["col5"].ToString();
                                model[0].Col43 = dtt.Rows[0]["col6"].ToString();
                                model[0].Col50 = dtt.Rows[0]["mctxt"].ToString();
                                model[0].Col51 = dtt.Rows[0]["mouldtxt"].ToString();
                                model[0].TList3 = mod3;
                                model[0].TList4 = mod4;
                                //model[0].TList5 = mod5;
                                model[0].TList6 = mod6;
                                model[0].TList7 = mod7;
                                model[0].SelectedItems3 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["opcode"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems4 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["mccode"].ToString()).Distinct()).Split(',');
                                //model[0].SelectedItems5 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["mouldcode"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems6 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["shftno"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems7 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["mccode"].ToString()).Distinct()).Split(',');
                                mq = "select it.icode as Icode,it.iname as Iname,pum.master_name as UOM from item it " +
                               "inner join master_setting pum on pum.master_id=it.uom and pum.type='UMM' and find_in_set(pum.client_unit_id,it.client_unit_id)=1 " +
                               "where find_in_set(it.client_unit_id,'" + unitid_mst + "')=1 and it.type='IT' and it.icode='" + dtt.Rows[0]["acode"].ToString() + "'";
                                DataTable dtm = sgen.getdata(userCode, mq);
                                if (dtm.Rows.Count > 0)
                                {
                                    model[0].Col18 = dtm.Rows[0]["Icode"].ToString();
                                    model[0].Col19 = dtm.Rows[0]["Icode"].ToString();
                                    model[0].Col20 = dtm.Rows[0]["Iname"].ToString();
                                    model[0].Col21 = dtm.Rows[0]["UOM"].ToString();
                                    //mq = "select * from (select bm.pono stgcode,bm.cond stg,bm.rno from itransactionc bm where bm.type = 'BOM' and bm.client_id='" + clientid_mst + "' " +
                                    //    "and bm.client_unit_id='" + unitid_mst + "' and bm.acode = '" + dtt.Rows[0]["acode"].ToString() + "' " +
                                    //    "union " +
                                    //    "select '001' stgcode,'0' stg,'0' rno from dual) a order by rno asc";                                 

                                    mq = "select bm.pono stgcode,bm.cond stg,bm.rno from itransactionc bm where bm.type = 'BOM' and bm.client_unit_id='" + unitid_mst + "' and bm.acode = '" + dtt.Rows[0]["acode"].ToString() + "' order by rno asc";
                                    dtp = sgen.getdata(userCode, mq);
                                    DataTable stgs = new DataTable();
                                    stgs.Columns.Add("stgcode");
                                    string lastscode = "", newscode = "";
                                    if (dtp.Rows.Count > 0)
                                    {
                                        for (int i = 0; i < dtp.Rows.Count; i++)
                                        {
                                            newscode = dtp.Rows[i]["stgcode"].ToString().Trim();
                                            if (newscode != lastscode)
                                            {
                                                DataRow dr = stgs.NewRow();
                                                dr[0] = newscode;
                                                stgs.Rows.Add(dr);
                                                lastscode = newscode;
                                            }
                                        }
                                        stgs.AcceptChanges();
                                        DataTable dts = sgen.getdata(userCode, "SELECT master_id stgcode,master_name stg FROM master_setting where type = 'KPS'");
                                        var results = from table1 in stgs.AsEnumerable()
                                                      join table2 in dts.AsEnumerable() on table1["stgcode"] equals table2["stgcode"]
                                                      select new
                                                      {
                                                          stgcode = table1["stgcode"],
                                                          stg = table2["stg"]
                                                      };
                                        results.ToList();
                                        DataTable dtkk = sgen.ToDataTable(results.ToList());
                                        //if (dtkk.Rows.Count > 0)
                                        //{
                                        //    foreach (DataRow dr in dtkk.Rows)
                                        //    {
                                        //        mod1.Add(new SelectListItem { Text = dr["stg"].ToString(), Value = dr["stgcode"].ToString() });
                                        //    }
                                        //}
                                        //TempData[MyGuid + "_TList1"] = mod1;
                                        //model[0].TList1 = mod1;
                                        //dtkk = dtkk.Select("stgcode not in ('001')").CopyToDataTable();
                                        if (dtkk.Rows.Count > 0)
                                        {
                                            foreach (DataRow dr in dtkk.Rows)
                                            {
                                                mod2.Add(new SelectListItem { Text = dr["stg"].ToString(), Value = dr["stgcode"].ToString() });
                                            }
                                        }
                                        TempData[MyGuid + "_TList2"] = mod2;
                                        model[0].TList2 = mod2;
                                    }
                                }
                                model[0].SelectedItems2 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["fstg"].ToString()).Distinct()).Split(',');

                                mq = "select b.tmode mldid,m.col31 mldname from itransactionc b inner join kc_tab m on m.vch_num=b.tmode and m.type = 'MOS' and m.client_unit_id = b.client_unit_id " +
                                    "where b.type='BOM' and b.acode='" + model[0].Col18 + "' and b.pono='" + model[0].SelectedItems2.FirstOrDefault() + "'";
                                dtp = sgen.getdata(userCode, mq);
                                if (dtp.Rows.Count > 0) { foreach (DataRow dr in dtp.Rows) { mod5.Add(new SelectListItem { Text = dr["mldname"].ToString(), Value = dr["mldid"].ToString() }); } }
                                else { mod5 = cmd_Fun.mldname(userCode, unitid_mst); }
                                model[0].TList5 = mod5;
                                TempData[MyGuid + "_TList5"] = mod5;
                                model[0].SelectedItems5 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["mouldcode"].ToString()).Distinct()).Split(',');
                                //model[0].SelectedItems1 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["fstg"].ToString()).Distinct()).Split(',');

                                model[0].dt1 = new DataTable();
                                model[0].dt1 = ((DataTable)sgen.GetSession(MyGuid, "dtipd")).Clone();
                                for (int i = 0; i < dtt.Rows.Count; i++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["sno"] = i + 1;
                                    dr["Icode"] = dtt.Rows[i]["icode"].ToString();
                                    dr["Iname"] = dtt.Rows[i]["iname"].ToString();
                                    dr["Uom"] = dtt.Rows[i]["uom"].ToString();//puom
                                    dr["Qtystk"] = dtt.Rows[i]["qtystk"].ToString();//puom
                                    dr["Stgcode"] = dtt.Rows[i]["stage"].ToString();//puom
                                    dr["Stg"] = dtt.Rows[i]["stage"].ToString();//puom
                                    dr["Est_consume_qty"] = dtt.Rows[i]["estcon_qty"].ToString();//estimated consume qty
                                    dr["Act_consume_qty"] = dtt.Rows[i]["actcon_qty"].ToString();//actual consume qty
                                    dr["Short_Excess"] = dtt.Rows[i]["shortex"].ToString();//shortexcess
                                    dr["Alternate"] = dtt.Rows[i]["alt"].ToString();//shortexcess
                                    model[0].dt1.Rows.Add(dr);
                                }
                                model[0].dt1.AcceptChanges();
                                #region
                                //mq1 = "select (i.client_unit_id||i.icode||i.loc) fstr,i.icode,it.iname,um.master_name uom,i.qtystk,i.qtyout,i.loc,i.locname from btchtrans i " +
                                // "inner join item it on it.icode=i.icode and it.type='IT' and it.client_id=i.client_id and it.client_unit_id=i.client_unit_id " +
                                // "inner join master_setting um on um.master_id=it.uom and um.type='UMM' and find_in_set(um.client_id,it.client_id)=1 and find_in_set(um.client_unit_id,it.client_unit_id)=1 " +
                                // "where (i.client_id||i.client_unit_id||i.vch_num|| to_char(i.vch_date, 'yyyymmdd')||i.type)='" + URL + "'";
                                //dtt = sgen.getdata(userCode, mq1);
                                //if (dtt.Rows.Count > 0)
                                //{
                                //    model[0].dt2 = new DataTable();
                                //    DataTable dt2 = sgen.getdata(userCode, "select '-' Sno, '-' Icode, '-' Iname, '-' Uom, '0' Qty_stk, '0' Act_Consume_Qty, '0' Loc, '-' LocName from dual");
                                //    Session["dtipd2"] = dt2;
                                //    model[0].dt2 = ((DataTable)Session["dtipd2"]).Clone();
                                //    for (int i = 0; i < dtt.Rows.Count; i++)
                                //    {
                                //        DataRow dr = model[0].dt2.NewRow();
                                //        //dr["sno"] = dtt.Rows[i]["fstr"].ToString();
                                //        dr["sno"] = (i + 1).ToString();
                                //        dr["IName"] = dtt.Rows[i]["iname"].ToString();
                                //        dr["Icode"] = dtt.Rows[i]["icode"].ToString();
                                //        dr["UOM"] = dtt.Rows[i]["uom"].ToString();
                                //        dr["Qty_Stk"] = dtt.Rows[i]["qtystk"].ToString();
                                //        dr["Act_Consume_Qty"] = dtt.Rows[i]["qtyout"].ToString();
                                //        dr["Loc"] = dtt.Rows[i]["loc"].ToString();
                                //        dr["LocName"] = dtt.Rows[i]["locname"].ToString();
                                //        model[0].dt2.Rows.Add(dr);
                                //    }
                                //    model[0].dt2.AcceptChanges();
                                //}
                                #endregion
                            }
                            break;
                        case "ITEM": //main item                         
                            type = model[0].Col12;
                            if (type.Trim().Equals("100"))
                            {
                                mq = "select bm.acode,it.iname,pum.master_name uom,bm.irate lotqty,bm.pono stgcode,bm.rno from itransactionc bm " +
                                    "inner join item it on it.icode = bm.acode and it.type = 'IT' and find_in_set(it.client_unit_id, bm.client_unit_id)=1 " +
                                    "inner join master_setting pum on pum.master_id = it.uom and pum.type = 'UMM' and find_in_set(pum.client_unit_id, bm.client_unit_id)= 1 " +
                                    //"inner join master_setting s on s.master_id = bm.pono and s.type = 'KPS' and find_in_set(s.client_id, bm.client_id)= 1 and find_in_set(s.client_unit_id, bm.client_unit_id)= 1 " +
                                    "where(bm.client_id || bm.client_unit_id || bm.acode || bm.type) = '" + URL + "' order by bm.rno asc";
                            }
                            else
                            {
                                //mq = "select bm.acode,it.iname,pum.master_name uom,bm.irate lotqty,bm.pono stgcode,bm.rno,w.vch_num pdono," +
                                //    "to_char(w.vch_date, '" + sgen.Getsqldateformat() + "') pdodt,w.col9 ord_qty,w.col10 plan_qty from itransactionc bm " +
                                //    "inner join item it on it.icode = bm.acode and it.type = 'IT' and it.client_id = bm.client_id and it.client_unit_id = bm.client_unit_id " +
                                //    "inner join master_setting pum on pum.master_id = it.uom and pum.type = 'UMM' and find_in_set(pum.client_id, bm.client_id)= 1 and find_in_set(pum.client_unit_id, bm.client_unit_id)= 1 " +
                                //    "inner join kc_tab w on w.col5=bm.acode and w.type='PSO' and w.client_id=bm.client_id and w.client_unit_id=bm.client_unit_id " +
                                //    "where (bm.client_id || bm.client_unit_id || bm.acode || bm.type) = '" + URL + "' order by bm.rno asc";
                             
                               
                                mq = "select bm.acode,it.iname,pum.master_name uom,bm.irate lotqty,bm.pono stgcode,bm.rno,w.vch_num pdono, to_char(w.vch_date, '" + sgen.Getsqldateformat() + "') pdodt," +
                                    "w.Bal_qty plan_qty, w.ord_qty from itransactionc bm " +
                                    "inner join item it on it.icode = bm.acode and it.type = 'IT' and find_in_set(it.client_unit_id, bm.client_unit_id)=1 " +
                                    "inner join master_setting pum on pum.master_id = it.uom and pum.type = 'UMM' and find_in_set(pum.client_unit_id, bm.client_unit_id)= 1 " +
                                    "inner join (select a.vch_num, a.vch_date, a.type, a.icode, a.client_id, a.client_unit_id, (a.ord_qty), sum(to_number(a.used_qty)) as used_qty,(a.ord_qty - nvl(sum(a.used_qty), '0')) as Bal_qty " +
                                    "from(select w.vch_num, w.vch_date, w.col5 as icode, w.client_id,w.client_unit_id, w.type, (case when w.col10 = '-' then '0' else w.col10 end) as ord_qty,nvl(ex.qtyin, '0') used_qty from kc_tab w " +
                                    "left join iprod ex on ex.pdono = w.vch_num and ex.client_unit_id = w.client_unit_id and ex.type='101' " +
                                    "where w.client_unit_id = '" + unitid_mst + "' and w.type = 'PSO' union all " +
                                    "select w.vch_num,w.vch_date,w.col5 as icode,w.client_id,w.client_unit_id,w.type,(case when w.col10 = '-' then '0' else w.col10 end) ord_qty," +
                                    "nvl(to_number(ex.col11), '0') as close_qty from kc_tab w left join enx_tab ex on ex.col12 = w.vch_num and ex.client_unit_id = w.client_unit_id " +
                                    "and ex.type='COR' where w.client_unit_id = '" + unitid_mst + "' and w.type = 'PSO') a group by(a.vch_num, a.vch_date, a.type, a.icode, a.client_id,a.client_unit_id, a.ord_qty)) w on w.icode = bm.acode and w.client_unit_id = bm.client_unit_id where (bm.client_id || bm.client_unit_id || bm.acode || bm.type) = '" + URL + "' order by bm.rno asc";
                            }
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                model[0].Col18 = dtt.Rows[0]["acode"].ToString();
                                model[0].Col19 = dtt.Rows[0]["acode"].ToString();
                                model[0].Col20 = dtt.Rows[0]["iname"].ToString();
                                model[0].Col21 = dtt.Rows[0]["uom"].ToString();
                                model[0].Col97 = dtt.Rows[0]["lotqty"].ToString();//bom_lotqty
                                model[0].Col30 = "0"; //rej qty
                                model[0].Col31 = "0"; //rw qty
                                if (type.Trim().Equals("101"))
                                {
                                    model[0].Col32 = dtt.Rows[0]["pdono"].ToString();//pdo no
                                    model[0].Col33 = dtt.Rows[0]["pdodt"].ToString();//pdo dt
                                    model[0].Col34 = dtt.Rows[0]["plan_qty"].ToString();//plan qty
                                }

                                model[0].TList5 = mod5;
                                TempData[MyGuid + "_TList5"] = mod5;

                                //mq = "select bm.acode MainItem,bm.pono stgcode,bm.cond stg,bm.rno from itransactionc bm where bm.type = 'BOM' and " +
                                //    "(bm.client_id||bm.client_unit_id||bm.acode) = '" + URL + "' order by bm.rno asc";
                                //mq = "select * from (select bm.pono stgcode,bm.cond stg,bm.rno from itransactionc bm where bm.type = 'BOM' " +
                                //    "and (bm.client_id || bm.client_unit_id || bm.acode) = '" + URL + "' " +
                                //    "union " +
                                //    "select '001' stgcode,'0' stg,'0' rno from dual) a order by rno asc";
                                //mq = "select bm.pono stgcode,bm.cond stg,bm.rno from itransactionc bm where bm.type = 'BOM' and " +
                                //    "(bm.client_id || bm.client_unit_id || bm.acode) = '" + URL + "'order by rno asc";
                                //dtp = sgen.getdata(userCode, mq);
                                DataTable stgs = new DataTable();
                                stgs.Columns.Add("stgcode");
                                string lastscode = "", newscode = "";
                                //if (dtp.Rows.Count > 0)
                                //{
                                for (int i = 0; i < dtt.Rows.Count; i++)
                                {
                                    newscode = dtt.Rows[i]["stgcode"].ToString().Trim();
                                    if (newscode != lastscode)
                                    {
                                        DataRow dr = stgs.NewRow();
                                        dr[0] = newscode;
                                        stgs.Rows.Add(dr);
                                        lastscode = newscode;
                                    }
                                }
                                stgs.AcceptChanges();
                                DataTable dts = sgen.getdata(userCode, "SELECT master_id stgcode,master_name stg FROM master_setting where type = 'KPS'");
                                var results = from table1 in stgs.AsEnumerable()
                                              join table2 in dts.AsEnumerable() on table1["stgcode"] equals table2["stgcode"]
                                              select new
                                              {
                                                  stgcode = table1["stgcode"],
                                                  stg = table2["stg"]
                                              };
                                results.ToList();
                                DataTable dtkk = sgen.ToDataTable(results.ToList());
                                if (dtkk.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dtkk.Rows)
                                    {
                                        mod2.Add(new SelectListItem { Text = dr["stg"].ToString(), Value = dr["stgcode"].ToString() });
                                    }
                                }
                                //TempData[MyGuid + "_TList1"] = mod1;
                                //model[0].TList1 = mod1;
                                TempData[MyGuid + "_TList2"] = mod2;
                                model[0].TList2 = mod2;
                                //dtkk = dtkk.Select("stgcode not in ('001')").CopyToDataTable();
                                //if (stgs.Rows.Count > 0)
                                //{
                                //    foreach (DataRow dr in stgs.Rows)
                                //    {
                                //        mod2.Add(new SelectListItem { Text = dr["stg"].ToString(), Value = dr["stgcode"].ToString() });
                                //    }
                                //}
                                //TempData[MyGuid + "_TList2"] = mod2;
                                //model[0].TList2 = mod2;
                                //}
                            }
                            else
                            {
                                ViewBag.scripCall += "showmsgJS(1,'No data found for this item,please check again.', 2);";
                                return model;
                            }
                            break;
                        case "PDREQ": //pd bom calc stage wise
                            #region first query item wise
                            //mq = "select (bm.client_id||bm.client_unit_id||bm.icode||to_char(bm.vch_date,'yyyymmdd')||bm.type) fstr,bm.acode MainItem,bm.icode,it.iname,pum.master_name uom," +
                            //    "bm.pono stgcode, bm.cond stg, sum(to_number(bm.qtyin) + to_number(bm.qtyout)) bmiqty,bm.irate lotqty, mg.master_name MainGrp,gp.master_name ItemGrp,sg.iname Subgrp " +
                            //    "from itransactionc bm " +
                            //    "inner join item it on it.icode = bm.icode and it.type = 'IT' and it.client_id = bm.client_id and it.client_unit_id = bm.client_unit_id " +
                            //    "inner join master_setting pum on pum.master_id = it.uom and pum.type = 'UMM' and find_in_set(pum.client_id,'" + clientid_mst + "')= 1 and find_in_set(pum.client_unit_id,'" + clientid_mst + "')= 1 " +
                            //    "inner join master_setting mg on mg.classid = substr(bm.icode, 1, 1) and mg.type = 'KGP' " +
                            //    "inner join master_setting gp on gp.master_id = substr(bm.icode, 1, 3) and gp.type = 'KIG' and gp.client_id = '" + clientid_mst + "' and gp.client_unit_id = '" + unitid_mst + "' " +
                            //    "inner join item sg on sg.icode = substr(bm.icode, 1, 5) and sg.type = 'SG' and sg.client_id = '" + clientid_mst + "' and sg.client_unit_id = '" + unitid_mst + "' " +
                            //    "where bm.type = 'BOM' and bm.client_id = '" + clientid_mst + "' and bm.client_unit_id = '" + unitid_mst + "' and bm.acode = '" + model[0].Col19 + "' and bm.pono = '" + model[0].SelectedItems2.FirstOrDefault().Trim() + "' " +
                            //    "group by(bm.client_id|| bm.client_unit_id || bm.icode || to_char(bm.vch_date, 'yyyymmdd') || bm.type),bm.acode,bm.icode,it.iname,pum.master_name,bm.pono,bm.cond," +
                            //    "bm.irate,mg.master_name,gp.master_name,sg.iname";                            
                            //mq = "select (bm.client_id||bm.client_unit_id||bm.icode||to_char(bm.vch_date,'yyyymmdd')||bm.type) fstr,bm.acode MainItem,bm.icode,it.iname,pum.master_name uom," +
                            //"bm.pono stgcode,bm.cond stg,sum(to_number(bm.qtyin) + to_number(bm.qtyout)) bmiqty,bm.irate lotqty " +
                            //"from itransactionc bm " +
                            //"inner join item it on it.icode = bm.icode and it.type = 'IT' and it.client_id = bm.client_id and it.client_unit_id = bm.client_unit_id " +
                            //"inner join master_setting pum on pum.master_id = it.uom and pum.type = 'UMM' and find_in_set(pum.client_id,bm.client_id)= 1 and find_in_set(pum.client_unit_id,bm.client_unit_id)= 1 " +
                            //"where bm.type = 'BOM' and bm.client_id = '" + clientid_mst + "' and bm.client_unit_id = '" + unitid_mst + "' and bm.acode = '" + model[0].Col19 + "' and bm.pono = '" + model[0].SelectedItems2.FirstOrDefault().Trim() + "' " +
                            //"group by(bm.client_id|| bm.client_unit_id || bm.icode || to_char(bm.vch_date, 'yyyymmdd') || bm.type),bm.acode,bm.icode,it.iname,pum.master_name,bm.pono,bm.cond,bm.irate order by bm.icode asc";
                            #endregion
                            model[0].dt1 = ((DataTable)sgen.GetSession(MyGuid, "dtipd")).Copy();
                            var totqty = sgen.Make_decimal(model[0].Col23) + sgen.Make_decimal(model[0].Col30);
                            var sumqty = sgen.Make_decimal(model[0].Col23) + sgen.Make_decimal(model[0].Col30) + sgen.Make_decimal(model[0].Col31);
                            decimal qtyrw = sgen.Make_decimal(model[0].Col31);
                            mq = "select bm.pono stgcode,bm.cond stg,bm.rno from itransactionc bm where bm.type = 'BOM' and " +
                               "bm.client_unit_id = '" + unitid_mst + "' and bm.acode = '" + model[0].Col19 + "' order by bm.rno asc";
                            dtp = sgen.getdata(userCode, mq);
                            DataTable stgss = new DataTable();
                            stgss.Columns.Add("stgcode");
                            string lastcode = "", newcode = "";
                            if (dtp.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtp.Rows.Count; i++)
                                {
                                    newcode = dtp.Rows[i]["stgcode"].ToString().Trim();
                                    if (newcode != lastcode)
                                    {
                                        DataRow dr = stgss.NewRow();
                                        dr[0] = newcode;
                                        stgss.Rows.Add(dr);
                                        lastcode = newcode;
                                    }
                                }
                                stgss.AcceptChanges();
                                try
                                {
                                    int totstg = stgss.Rows.Count;
                                    int stg = sgen.seekval_dt_rowindex(stgss, "stgcode='" + model[0].SelectedItems2.FirstOrDefault() + "'");
                                    //if (stg == 0)
                                    //{
                                    //    ViewBag.scripCall = "showmsgJS(1,'Invalid stage, please select correct stage', 2);";
                                    //    return model;
                                    //}
                                    if (stg > 1)
                                    {
                                        mq2 = " union all select '" + clientid_mst + "','" + unitid_mst + "','BOM','" + model[0].Col19 + "','" + model[0].SelectedItems2.FirstOrDefault() + "','" + model[0].Col97 + "','" + model[0].Col19 + "' icode ,'T' flg," + sumqty + " est,'N' Alternate from dual ";
                                        #region stock checking validation
                                        //mq1 = "select qtyin from iprod where type='100' and acode='" + model[0].Col19 + "' and stage1='" + model[0].SelectedItems2.FirstOrDefault() + "'";
                                        //mq1 = sgen.seekval(userCode, mq1, "qtyin");
                                        ////mq1 = sgen.seekval_dt(userCode, dtt, "icode", model[0].Col19, "qtystk");
                                        ////if (mq1.Trim() != "0")
                                        //model[0].Col29 = mq1;
                                        //if (totqty > sgen.Make_decimal(mq1))
                                        //{
                                        //    model[0].Col99 = "Y";
                                        //    ViewBag.scripCall += "showmsgJS(1,'Produce And Rejection Qty cannot be greater than Stock At Current Stage', 2);";
                                        //}
                                        #endregion
                                    }
                                    else
                                    {
                                        mq2 = "";
                                        model[0].Col99 = "N";
                                        if (qtyrw > 0)
                                        {
                                            mq2 = " union all select '" + clientid_mst + "','" + unitid_mst + "','BOM','" + model[0].Col19 + "','" + model[0].SelectedItems2.FirstOrDefault() + "','" + model[0].Col97 + "','" + model[0].Col19 + "' icode ,'T' flg," + qtyrw + " est,'N' Alternate from dual ";
                                        }
                                    }
                                    model[0].Col101 = "C";//consumption pd
                                    if (totstg == stg) model[0].Col101 = "F";//final pd
                                    //model[0].SelectedItems1 = new string[] { stgss.Rows[stg - 2]["stgcode"].ToString().Trim() };
                                    try { model[0].Col98 = stgss.Rows[stg]["stgcode"].ToString().Trim(); }//nxt stg
                                    catch (Exception err)
                                    {
                                        model[0].Col98 = "100";                                     
                                    }
                                }
                                catch (Exception ex)
                                {
                                    #region waste
                                    //model[0].SelectedItems1 = new string[] { "001" };
                                    //model[0].Col29 = "0";
                                    //mq = "select (bm.client_id||bm.client_unit_id||bm.icode||to_char(bm.vch_date,'yyyymmdd')||bm.type) fstr,bm.acode MainItem,bm.icode,it.iname,pum.master_name uom," +
                                    //    "bm.pono stgcode, bm.cond stg, sum(to_number(bm.qtyin) + to_number(bm.qtyout)) bmiqty,sum(to_number(lc.qtyin) + to_number(lc.qtyout)) lciqty,bm.irate lotqty,lc.loc," +
                                    //    "location_name(bm.client_id || bm.client_unit_id || lc.loc) locname from itransactionc bm " +
                                    //    "inner join item it on it.icode = bm.icode and it.type = 'IT' and it.client_id = bm.client_id and it.client_unit_id = bm.client_unit_id " +
                                    //    "inner join master_setting pum on pum.master_id = it.uom and pum.type = 'UMM' and find_in_set(pum.client_id, bm.client_id)= 1 and " +
                                    //    "find_in_set(pum.client_unit_id, bm.client_unit_id)= 1 " +
                                    //    "inner join btchtrans lc on lc.icode = bm.icode and lc.type in ('30', '32') and lc.client_id = bm.client_id and lc.client_unit_id = bm.client_unit_id and " +
                                    //    "lc.pono = 'W' and lc.loc = '" + model[0].SelectedItems7.FirstOrDefault() + "' " +
                                    //    "where bm.type = 'BOM' and bm.client_id = '" + clientid_mst + "' and bm.client_unit_id = '" + unitid_mst + "' and bm.acode = '" + model[0].Col19 + "' and bm.pono = '" + model[0].SelectedItems2.FirstOrDefault().Trim() + "' " +
                                    //    "group by(bm.client_id|| bm.client_unit_id || bm.icode || to_char(bm.vch_date, 'yyyymmdd') || bm.type),bm.acode,bm.icode,it.iname,pum.master_name,bm.pono," +
                                    //    "bm.cond,bm.irate,lc.loc,location_name(bm.client_id || bm.client_unit_id || lc.loc) order by bm.icode asc";
                                    //dtt = new DataTable();
                                    //dtt = sgen.getdata(userCode, mq);
                                    //if (dtt.Rows.Count > 0)
                                    //{
                                    //    try
                                    //    {
                                    //        DataTable dt2 = sgen.getdata(userCode, "select '-' Sno, '-' Icode, '-' Iname, '-' Uom, '0' Qty_stk, '0' Act_Consume_Qty, '0' Loc, '-' LocName from dual");
                                    //        Session["dtipd2"] = dt2;
                                    //        model[0].dt2 = (DataTable)Session["dtipd2"];
                                    //        if (model[0].dt2.Rows[0]["icode"].ToString().Trim().Equals("-")) { model[0].dt2.Rows.RemoveAt(0); }
                                    //        for (int i = 0; i < dtt.Rows.Count; i++)
                                    //        {
                                    //            DataRow dr = model[0].dt2.NewRow();
                                    //            dr["SNo"] = i + 1;
                                    //            dr["Icode"] = dtt.Rows[i]["icode"].ToString();
                                    //            dr["Iname"] = dtt.Rows[i]["iname"].ToString();
                                    //            dr["Uom"] = dtt.Rows[i]["uom"].ToString();
                                    //            dr["Qty_stk"] = dtt.Rows[i]["lciqty"].ToString();
                                    //            //var bmiqty = (sgen.Make_decimal(model[0].Col23) * (sgen.Make_decimal(dtt.Rows[i]["bmiqty"].ToString()) / sgen.Make_decimal(dtt.Rows[i]["lotqty"].ToString())));
                                    //            var bmiqty = ((sgen.Make_decimal(model[0].Col23) + sgen.Make_decimal(model[0].Col30)) * (sgen.Make_decimal(dtt.Rows[i]["bmiqty"].ToString()) / sgen.Make_decimal(dtt.Rows[i]["lotqty"].ToString())));
                                    //            var lciqty = sgen.Make_decimal(dtt.Rows[i]["lciqty"].ToString());
                                    //            if (lciqty > bmiqty)
                                    //            {
                                    //                //dr["Act_Consume_Qty"] = (sgen.Make_decimal(model[0].Col23) * (sgen.Make_decimal(dtt.Rows[i]["bmiqty"].ToString()) / sgen.Make_decimal(dtt.Rows[i]["lotqty"].ToString())));
                                    //                dr["Act_Consume_Qty"] = ((sgen.Make_decimal(model[0].Col23) + sgen.Make_decimal(model[0].Col30)) * (sgen.Make_decimal(dtt.Rows[i]["bmiqty"].ToString()) / sgen.Make_decimal(dtt.Rows[i]["lotqty"].ToString())));
                                    //            }
                                    //            else
                                    //            {
                                    //                dr["Act_Consume_Qty"] = dtt.Rows[i]["lciqty"].ToString();
                                    //            }
                                    //            dr["Loc"] = dtt.Rows[i]["loc"].ToString();
                                    //            dr["LocName"] = dtt.Rows[i]["locname"].ToString();
                                    //            model[0].dt2.Rows.Add(dr);
                                    //        }
                                    //        model[0].dt2.AcceptChanges();
                                    //    }
                                    //    catch (Exception exx) { ViewBag.scripCall = "showmsgJS(1," + exx.Message.ToString() + ",0);"; }
                                    //}
                                    #endregion
                                }
                            }
                            #region old query
                            //mq = "select bm.acode MainItem,est bmiqty,bm.irate lotqty,bm.icode,s.iname,s.uom,s.qtystk,s.fstg fstgcode, st.master_name fstg, bm.flg from " +
                            //    "(select client_id, client_unit_id, type, acode, pono, irate, icode,'B' as flg,(to_number(qtyin) + to_number(qtyout)) / to_number(irate) * " + totqty + " est " +
                            //    "from itransactionc where type = 'BOM' "+mq2+") bm " +
                            //    "inner join " +
                            //    "(select i.icode, i.iname, um.master_name uom, sum(nvl(a.qtyin, 0)) qtyin, sum(nvl(a.qtyout, 0)) qtyout, sum(nvl(a.qtyin, 0)) - sum(nvl(a.qtyout, 0)) qtystk," +
                            //    "nvl(a.fstg, '001') fstg, i.type, i.client_id, i.client_unit_id from item i " +
                            //    "left join (select icode, (case when type='100' then 0 else qtyin end)  qtyin, qtyout qtyout, stage fstg, type, " +
                            //    "client_id, client_unit_id,vch_num,vch_date from iprod where (type like '3%' or type='100' or type = '10P') " +
                            //    "union " +
                            //    "select acode icode, to_number(max(qtyin)) qtyin, 0 qtyout, stage1 fstg, type, client_id, client_unit_id ,vch_num,vch_date from iprod " +
                            //    "where type = '100' group by acode, stage1, type, client_id, client_unit_id ,vch_num,vch_date " +
                            //    "union " +
                            //    "select acode icode, to_number(max(qtyrej)) qtyin, 0 qtyout, '101' fstg, type, client_id, client_unit_id,vch_num,vch_date from iprod where type = '100' " +
                            //    "group by acode, type, client_id, client_unit_id,vch_num,vch_date " +
                            //    "union " +
                            //    "select icode, sum(qtyin) qtyin, 0 qtyout, stage1 fstg, type, client_id, client_unit_id ,vch_num,vch_date from iprod where type = '10R' " +
                            //    "group by icode, stage1, type, client_id, client_unit_id,vch_num,vch_date " +
                            //    "union " +
                            //    "select icode, 0 qtyin, sum(qtyout) qtyout, stage fstg, type,client_id, client_unit_id,vch_num,vch_date from iprod where type = '10R' " +
                            //    "group by icode, stage, type, client_id, client_unit_id,vch_num,vch_date " +
                            //    "union " +
                            //    "select icode, sum(qtyout) qtyin, 0 qtyout, stage1 fstg, type,client_id, client_unit_id,vch_num,vch_date from iprod where type = '10R' " +
                            //    "group by icode, stage1, type, client_id, client_unit_id,vch_num,vch_date) a on a.icode = i.icode and a.client_id = i.client_id and a.client_unit_id = i.client_unit_id " +
                            //    "inner join master_setting um on um.master_id = i.uom and um.type = 'UMM' and find_in_set(um.client_id, i.client_id)= 1 and find_in_set(um.client_unit_id, i.client_unit_id)= 1 " +
                            //    "where i.type = 'IT' and i.client_id = '" + clientid_mst + "' and i.client_unit_id = '" + unitid_mst + "' " +
                            //    "group by i.icode,i.iname,um.master_name,nvl(a.fstg, '001'),i.type,i.client_id,i.client_unit_id) s on s.icode = bm.icode and " +
                            //    "s.client_id = bm.client_id and s.client_unit_id = bm.client_unit_id and s.fstg in ('001', '" + model[0].SelectedItems2.FirstOrDefault() + "') " +
                            //    "inner join master_setting st on st.master_id = s.fstg and st.type = 'KPS' and find_in_set(st.client_id, bm.client_id)= 1 and find_in_set(st.client_unit_id, bm.client_unit_id)= 1 " +
                            //    "where bm.type = 'BOM' and bm.client_id = '" + clientid_mst + "' and bm.client_unit_id = '" + unitid_mst + "' and bm.acode = '" + model[0].Col19 + "' and bm.pono = '" + model[0].SelectedItems2.FirstOrDefault() + "'";
                            #endregion
                            mq = " select * from (select nvl(bm.acode,'-') MainItem,max(nvl(est,0)) bmiqty,max(nvl(bm.irate,0)) lotqty,s.icode,s.iname,s.uom,sum(s.qtystk) qtystk,s.fstg fstgcode," +
                                "st.master_name fstg,bm.flg,bm.Alternate from " +
                                "(select i.icode, i.iname, um.master_name uom, sum(nvl(a.qtyin, 0)) qtyin, sum(nvl(a.qtyout, 0)) qtyout, sum(nvl(a.qtyin, 0)) - sum(nvl(a.qtyout, 0)) qtystk," +
                               "nvl(a.fstg, '001') fstg, i.type, i.client_id, i.client_unit_id from item i " +
                               "left join (select icode, (case when type='100' then 0 else qtyin end) qtyin, qtyout qtyout, stage fstg, type, " +
                               "client_id, client_unit_id,vch_num,vch_date from iprod where (type like '3%' or type = '10P' or type='100') " +
                               "union " +
                               "select acode icode, (to_number(max(qtyin))+to_number(max(qtyrw))) qtyin, 0 qtyout, stage1 fstg, type, client_id, client_unit_id ,vch_num,vch_date from iprod " +
                               "where type = '100' group by acode, stage1, type, client_id, client_unit_id ,vch_num,vch_date " +
                               "union " +
                               "select acode icode, to_number(max(qtyrej)) qtyin, 0 qtyout, '101' fstg, type, client_id, client_unit_id,vch_num,vch_date from iprod where type = '100' " +
                               "group by acode, type, client_id, client_unit_id,vch_num,vch_date " +
                               "union " +
                               "select icode, sum(qtyin) qtyin, 0 qtyout, stage1 fstg, type, client_id, client_unit_id ,vch_num,vch_date from iprod where type = '10R' " +
                               "group by icode, stage1, type, client_id, client_unit_id,vch_num,vch_date " +
                               "union " +
                               "select icode, 0 qtyin, sum(qtyout) qtyout, stage fstg, type,client_id, client_unit_id,vch_num,vch_date from iprod where type = '10R' " +
                               "group by icode, stage, type, client_id, client_unit_id,vch_num,vch_date " +
                               "union " +
                               "select icode, sum(qtyout) qtyin, 0 qtyout, stage1 fstg, type,client_id, client_unit_id,vch_num,vch_date from iprod where type = '10R' " +
                               "group by icode, stage1, type, client_id, client_unit_id,vch_num,vch_date) a " +
                               "on a.icode = i.icode and find_in_set(a.client_unit_id, i.client_unit_id)=1 " +
                               "inner join master_setting um on um.master_id = i.uom and um.type = 'UMM' and find_in_set(um.client_unit_id, i.client_unit_id)= 1 " +
                               "where i.type = 'IT' and find_in_set(i.client_unit_id, '" + unitid_mst + "')=1 " +
                               "group by i.icode,i.iname,um.master_name,nvl(a.fstg, '001'),i.type,i.client_id,i.client_unit_id) s " +
                               "inner join " +
                                "(select client_id, client_unit_id, type, acode, pono, irate, icode, 'B' as flg, (to_number(qtyin) + to_number(qtyout)) / to_number(irate) * " + totqty + " est," +
                                "(case when to_number(qtyin)=0 then 'Y' else 'N' end) Alternate " +
                               "from itransactionc where type = 'BOM' and acode='" + model[0].Col19 + "' and pono = '" + model[0].SelectedItems2.FirstOrDefault() + "' " + mq2 + ") bm " +
                               "on s.icode = bm.icode and s.client_unit_id = bm.client_unit_id and s.fstg in ('001', '" + model[0].SelectedItems2.FirstOrDefault() + "') " +
                               "inner join master_setting st on st.master_id = s.fstg and st.type = 'KPS' " +
                               "where s.client_unit_id = '" + unitid_mst + "' and s.fstg in ('001', '" + model[0].SelectedItems2.FirstOrDefault() + "') " +
                               "group by nvl(bm.acode, '-'),s.icode,s.iname,s.uom,s.fstg, st.master_name,bm.flg,bm.Alternate order by s.icode) a where a.qtystk<>0";
                            dtt = sgen.getdata(userCode, mq);
                            try
                            {
                                dtt = dtt.AsEnumerable().Except(dtt.Select(" icode='" + model[0].Col19.Trim() + "' and flg='B'")
                                    .CopyToDataTable().AsEnumerable(), DataRowComparer.Default).CopyToDataTable();
                            }
                            catch (Exception err) { }
                            if (dtt.Rows.Count > 0)
                            {
                                try
                                {
                                    if (model[0].dt1.Rows[0]["icode"].ToString().Trim().Equals("-")) { model[0].dt1.Rows.RemoveAt(0); }
                                    for (int i = 0; i < dtt.Rows.Count; i++)
                                    {
                                        DataRow dr = model[0].dt1.NewRow();
                                        dr["SNo"] = i + 1;
                                        dr["Icode"] = dtt.Rows[i]["icode"].ToString();
                                        dr["Iname"] = dtt.Rows[i]["iname"].ToString();
                                        dr["Uom"] = dtt.Rows[i]["uom"].ToString();
                                        dr["Qtystk"] = dtt.Rows[i]["qtystk"].ToString();
                                        dr["Stgcode"] = dtt.Rows[i]["fstgcode"].ToString();
                                        dr["Stg"] = dtt.Rows[i]["fstg"].ToString();
                                        dr["Alternate"] = dtt.Rows[i]["Alternate"].ToString();
                                        dr["Est_consume_qty"] = dtt.Rows[i]["bmiqty"].ToString();
                                        dr["Act_consume_qty"] = dtt.Rows[i]["bmiqty"].ToString();
                                        dr["short_excess"] = sgen.Make_decimal(dtt.Rows[i]["bmiqty"].ToString()) - sgen.Make_decimal(dtt.Rows[i]["bmiqty"].ToString());
                                        if (dtt.Rows[i]["Alternate"].ToString().Trim().Equals("Y"))
                                        {
                                            dr["Act_consume_qty"] = "0";
                                            dr["short_excess"] = "0";
                                        }
                                        model[0].dt1.Rows.Add(dr);
                                    }
                                    model[0].dt1.AcceptChanges();
                                }
                                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1," + ex.Message.ToString() + ",0);"; }
                            }
                            break;
                        case "PDITEM": //pd + click item
                            #region
                            //mq = "select (bm.client_id||bm.client_unit_id||bm.icode||to_char(bm.vch_date,'yyyymmdd')||bm.type) fstr,bm.acode MainItem,bm.icode,it.iname," +
                            //    "sum(to_number(bm.qtyin)+to_number(bm.qtyout)) estqty,ex.irate lotqty,pum.master_name as uom,mg.master_name MainGrp,gp.master_name ItemGrp from itransactionc bm " +
                            //    "inner join item it on it.icode = bm.icode and it.type = 'IT' and it.client_id = bm.client_id and it.client_unit_id = bm.client_unit_id " +
                            //    "inner join master_setting pum on pum.master_id = it.uom and pum.type = 'UMM' and find_in_set(pum.client_id,'" + clientid_mst + "')= 1 and find_in_set(pum.client_unit_id,'" + unitid_mst + "')= 1 " +
                            //    "inner join master_setting mg on mg.classid = substr(bm.icode, 1, 1) and mg.type = 'KGP' " +
                            //    "inner join master_setting gp on gp.master_id = substr(bm.icode, 1, 3) and gp.type = 'KIG' and gp.client_id = '" + clientid_mst + "' and gp.client_unit_id = '" + unitid_mst + "' " +
                            //    "inner join item sg on sg.icode = substr(bm.icode, 1, 5) and sg.type = 'SG' and sg.client_id = '" + clientid_mst + "' and sg.client_unit_id = '" + unitid_mst + "' " +
                            //    "where bm.type = 'BOM' and bm.client_id = '" + clientid_mst + "' and bm.client_unit_id = '" + unitid_mst + "' and bm.acode = '" + model[0].Col19 + "' and qtychl = '" + model[0].SelectedItems2.FirstOrDefault().Trim() + "' and qtyin<>='0'";
                            //mq = "select nvl(bm.acode,'-') MainICode,it.icode Icode,sum(to_number(bm.qtyin) + to_number(bm.qtyout)) estqty,bm.irate lotqty,mg.master_name MainGrp," +
                            //    "gp.master_name ItemGrp,it.iname Iname,pum.master_name uom from item it " +
                            //    "left outer join itransactionc bm on bm.icode = it.icode and bm.type = 'BOM' and bm.client_id = it.client_id and bm.client_unit_id = it.client_unit_id " +
                            //    "and bm.acode = '" + model[0].Col19 + "' and bm.qtychl = '" + model[0].SelectedItems2.FirstOrDefault().Trim() + "' " +
                            //    "inner join master_setting pum on pum.master_id = it.uom and pum.type = 'UMM' and find_in_set(pum.client_id,'" + clientid_mst + "')= 1 and find_in_set(pum.client_unit_id,'" + unitid_mst + "')= 1 " +
                            //    "inner join master_setting mg on mg.classid = substr(it.icode, 1, 1) and mg.type = 'KGP' " +
                            //    "inner join master_setting gp on gp.master_id = substr(it.icode, 1, 3) and gp.type = 'KIG' and gp.client_id = it.client_id and gp.client_unit_id = it.client_unit_id " +
                            //    "inner join item sg on sg.icode = substr(it.icode, 1, 5) and sg.type = 'SG' and sg.client_id = it.client_id and sg.client_unit_id = it.client_unit_id " +
                            //    "where (it.client_id || it.client_unit_id || it.icode || to_char(it.vch_date, 'yyyymmdd') || it.type) in ('" + URL + "') " +
                            //    "group by nvl(bm.acode, '-'),it.icode,bm.irate,mg.master_name,gp.master_name,it.iname,pum.master_name ";                  
                            //mq = "select (bt.client_id || bt.client_unit_id || bt.icode || to_char(bt.vch_date, 'yyyymmdd') || bt.type) fstr,nvl(bm.acode, '-') MainICode,mg.master_name MainGrp," +
                            //    "gp.master_name ItemGrp, sg.iname SubGrp, bt.icode,it.iname,pum.master_name uom, sum(to_number(bm.qtyin) + to_number(bm.qtyout)) estqty," +
                            //    "sum(to_number(bt.qtyin) + to_number(bt.qtyout)) lciqty,bm.irate lotqty,bt.loc,location_name(bt.client_id||bt.client_unit_id||bt.loc) locname from btchtrans bt " +
                            //    "left join itransactionc bm on bm.icode = bt.icode and bm.type = 'BOM' and bm.client_id = bt.client_id and bm.client_unit_id = bt.client_unit_id and " +
                            //    "bm.acode = '" + model[0].Col19 + "' and bm.pono = '" + model[0].SelectedItems2.FirstOrDefault().Trim() + "' " +
                            //    "inner join item it on it.icode = bt.icode and it.type = 'IT' and it.client_id = bt.client_id and it.client_unit_id = bt.client_unit_id " +
                            //    "inner join master_setting pum on pum.master_id = it.uom and pum.type = 'UMM' and find_in_set(pum.client_id, bt.client_id)= 1 and find_in_set(pum.client_unit_id, bt.client_unit_id) = 1 " +
                            //    "inner join master_setting mg on mg.classid = substr(bt.icode, 1, 1) and mg.type = 'KGP' " +
                            //    "inner join master_setting gp on gp.master_id = substr(bt.icode, 1, 3) and gp.type = 'KIG' and gp.client_id = bt.client_id and gp.client_unit_id = bt.client_unit_id " +
                            //    "inner join item sg on sg.icode = substr(bt.icode, 1, 5) and sg.type = 'SG' and sg.client_id = bt.client_id and sg.client_unit_id = bt.client_unit_id " +
                            //    "where (bt.client_id || bt.client_unit_id || bt.icode || to_char(bt.vch_date, 'yyyymmdd') || bt.type) in ('" + URL + "') and bt.pono = 'W' " +
                            //    "group by(bt.client_id|| bt.client_unit_id || bt.icode || to_char(bt.vch_date, 'yyyymmdd') || bt.type),nvl(bm.acode, '-'),mg.master_name,gp.master_name,sg.iname," +
                            //    "bt.icode,it.iname,pum.master_name,bm.irate,bt.loc,location_name(bt.client_id||bt.client_unit_id||bt.loc)";                                                     
                            //mq = "select nvl(est,'0')  as  bmiqty,s.icode,s.iname,s.uom,s.qtystk,s.fstg fstgcode, st.master_name fstg, nvl(bm.flg, 'B') as flg from " +
                            //    "(select i.icode, i.iname, um.master_name uom, sum(nvl(a.qtyin, 0)) qtyin, sum(nvl(a.qtyout, 0)) qtyout, sum(nvl(a.qtyin, 0)) - sum(nvl(a.qtyout, 0)) qtystk," +
                            //    "nvl(a.fstg, '001') fstg, i.type, i.client_id, i.client_unit_id from item i " +
                            //    "left join (select icode, qtyin qtyin, qtyout qtyout, stage fstg, type, client_id, client_unit_id from iprod where type like '3%' " +
                            //    "union " +
                            //    "select acode icode, to_number(max(qtyin)) qtyin, to_number(max(qtyout)) qtyout, stage1 fstg, type, client_id, client_unit_id from iprod " +
                            //    "where type = '100' group by acode, stage1, type, client_id, client_unit_id " +
                            //    "union " +
                            //    "select acode icode, to_number(max(qtyrej)) qtyin, 0 qtyout, '101' fstg, type, client_id, client_unit_id from iprod where type = '100' " +
                            //    "group by acode, type, client_id, client_unit_id " +
                            //    "union " +
                            //    "select icode, sum(qtyin) qtyin, sum(qtyout) qtyout, stage1 fstg, type, client_id, client_unit_id from iprod where type = '10R' " +
                            //    "group by icode, stage1, type, client_id, client_unit_id) a on a.icode = i.icode and a.client_id = i.client_id and a.client_unit_id = i.client_unit_id " +
                            //    "inner join master_setting um on um.master_id = i.uom and um.type = 'UMM' and find_in_set(um.client_id, i.client_id)= 1 and find_in_set(um.client_unit_id, i.client_unit_id)= 1 " +
                            //    "where i.type = 'IT' and i.client_id = '" + clientid_mst + "' and i.client_unit_id = '" + unitid_mst + "' " +
                            //    "group by i.icode,i.iname,um.master_name,nvl(a.fstg, '001'),i.type,i.client_id,i.client_unit_id) s " +
                            //    "left JOIN (select client_id, client_unit_id, type, acode, pono, irate, icode, 'B' as flg, " +
                            //    "(to_number(qtyin) + to_number(qtyout)) / to_number(irate) * " + totqty1 + " est from itransactionc where type = 'BOM' " +
                            //    "union all " +
                            //    "select '" + clientid_mst + "', '" + unitid_mst + "', 'BOM', '" + model[0].Col19 + "', '" + model[0].SelectedItems2.FirstOrDefault() + "', '" + model[0].Col97 + "', " +
                            //    "'" + model[0].Col19 + "' as icode, 'T' as flg, " + totqty1 + " as est from dual) bm on s.icode = bm.icode and s.client_id = bm.client_id and " +
                            //    "s.client_unit_id = bm.client_unit_id and s.fstg in ('001', '" + model[0].SelectedItems2.FirstOrDefault() + "') and bm.pono = '" + model[0].SelectedItems2.FirstOrDefault() + "' " +
                            //    "left join master_setting st on st.master_id = s.fstg and st.type = 'KPS' and find_in_set(st.client_id, S.client_id)= 1 and " +
                            //    "find_in_set(st.client_unit_id, S.client_unit_id)= 1 where s.client_id = '" + clientid_mst + "' and s.client_unit_id = '" + unitid_mst + "'";
                            #endregion
                            var totqty1 = sgen.Make_decimal(model[0].Col23) + sgen.Make_decimal(model[0].Col30);
                            var sumqty1 = sgen.Make_decimal(model[0].Col23) + sgen.Make_decimal(model[0].Col30) + sgen.Make_decimal(model[0].Col31);
                            decimal qtyrw1 = sgen.Make_decimal(model[0].Col31);
                            mq = "select bm.pono stgcode,bm.cond stg,bm.rno from itransactionc bm where bm.client_unit_id = '" + unitid_mst + "' and bm.type = 'BOM' and bm.acode = '" + model[0].Col19 + "' order by bm.rno asc";
                            dtp = sgen.getdata(userCode, mq);
                            DataTable stgss1 = new DataTable();
                            stgss1.Columns.Add("stgcode");
                            string lastcode1 = "", newcode1 = "";
                            if (dtp.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtp.Rows.Count; i++)
                                {
                                    newcode1 = dtp.Rows[i]["stgcode"].ToString().Trim();
                                    if (newcode1 != lastcode1)
                                    {
                                        DataRow dr = stgss1.NewRow();
                                        dr[0] = newcode1;
                                        stgss1.Rows.Add(dr);
                                        lastcode1 = newcode1;
                                    }
                                }
                                stgss1.AcceptChanges();
                                try
                                {
                                    int totstg = stgss1.Rows.Count;
                                    int stg = sgen.seekval_dt_rowindex(stgss1, "stgcode='" + model[0].SelectedItems2.FirstOrDefault() + "'");
                                    if (stg > 1)
                                    {
                                        mq2 = " union all select '" + clientid_mst + "','" + unitid_mst + "','BOM','" + model[0].Col19 + "','" + model[0].SelectedItems2.FirstOrDefault() + "','" + model[0].Col97 + "','" + model[0].Col19 + "' icode ,'T' flg," + sumqty1 + " est,'N' Alternate from dual ";
                                    }
                                    else
                                    {
                                        mq2 = "";
                                        model[0].Col99 = "N";
                                        if (qtyrw1 > 0)
                                        {
                                            mq2 = " union all select '" + clientid_mst + "','" + unitid_mst + "','BOM','" + model[0].Col19 + "','" + model[0].SelectedItems2.FirstOrDefault() + "','" + model[0].Col97 + "','" + model[0].Col19 + "' icode ,'T' flg," + qtyrw1 + " est,'N' Alternate from dual ";
                                        }
                                    }
                                    model[0].Col101 = "C";//consumption pd
                                    if (totstg == stg) model[0].Col101 = "F";//final pd                                    
                                    try { model[0].Col98 = stgss1.Rows[stg]["stgcode"].ToString().Trim(); }//nxt stg
                                    catch (Exception err) { model[0].Col98 = "100"; }
                                }
                                catch (Exception ex) { }
                            }
                            mq = "select * from (select (s.icode||s.client_id||s.client_unit_id) fstr,nvl(bm.acode,'-') MainItem,max(nvl(est,0)) bmiqty,max(nvl(bm.irate,0)) lotqty," +
                                "s.icode,s.iname,s.uom,sum(s.qtystk) as qtystk,s.fstg fstgcode, st.master_name fstg,NVL(bm.Alternate,'E') Alternate from (select i.icode, i.iname, um.master_name uom," +
                                "sum(nvl(a.qtyin, 0)) qtyin, sum(nvl(a.qtyout, 0)) qtyout, sum(nvl(a.qtyin, 0)) - sum(nvl(a.qtyout, 0)) qtystk, nvl(a.fstg, '001') fstg," +
                                "i.type, i.client_id, i.client_unit_id from item i " +
                                "left join (select icode, (case when type= '100' then 0 else qtyin end)  qtyin, qtyout qtyout," +
                                "stage fstg, type, client_id, client_unit_id,vch_num,vch_date from iprod where(type like '3%' or type = '10P' or type='100') " +
                                "union " +
                                "select acode icode, (to_number(max(qtyin))+to_number(max(qtyrw))) qtyin, 0 qtyout, stage1 fstg, type, client_id, client_unit_id, vch_num, vch_date from iprod where type = '100' " +
                                "group by acode, stage1, type, client_id, client_unit_id ,vch_num,vch_date " +
                                "union " +
                                "select acode icode, to_number(max(qtyrej)) qtyin, 0 qtyout, '101' fstg,type, client_id, client_unit_id,vch_num,vch_date from iprod where type = '100' " +
                                "group by acode, type, client_id, client_unit_id,vch_num,vch_date " +
                                "union " +
                                "select icode, sum(qtyin) qtyin, 0 qtyout, stage1 fstg, type, client_id, client_unit_id, vch_num, vch_date from iprod where type = '10R' group by " +
                                "icode, stage1, type, client_id, client_unit_id,vch_num,vch_date " +
                                "union " +
                                "select icode, 0 qtyin, sum(qtyout) qtyout, stage fstg, type, client_id,client_unit_id, vch_num, vch_date from iprod where type = '10R' " +
                                "group by icode, stage, type, client_id, client_unit_id,vch_num,vch_date " +
                                "union " +
                                "select icode, sum(qtyout) qtyin, 0 qtyout, stage1 fstg, type, client_id, client_unit_id, vch_num, vch_date from iprod where type = '10R' " +
                                "group by icode, stage1, type, client_id, client_unit_id,vch_num,vch_date) a " +
                                "on a.icode = i.icode and find_in_set(a.client_unit_id, i.client_unit_id)=1 " +
                                "inner join master_setting um on um.master_id = i.uom and um.type = 'UMM' and find_in_set(um.client_unit_id, i.client_unit_id)= 1 " +
                                "where i.type = 'IT' and find_in_set(i.client_unit_id, '" + unitid_mst + "')=1 group by i.icode,i.iname,um.master_name,nvl(a.fstg, '001'),i.type,i.client_id,i.client_unit_id) s " +
                                "left join (select client_id, client_unit_id, type, acode, pono, irate, icode, 'B' as flg, (to_number(qtyin) + to_number(qtyout)) / to_number(irate) * " + totqty1 + " est," +
                                "(case when to_number(qtyin)=0 then 'Y' else 'N' end) Alternate from " +
                                "itransactionc where type = 'BOM' and acode = '" + model[0].Col19 + "' and pono = '" + model[0].SelectedItems2.FirstOrDefault() + "' " +
                                " " + mq2 + " ) bm on s.icode = bm.icode " +
                                "and s.client_unit_id = bm.client_unit_id and s.fstg in ('001', '" + model[0].SelectedItems2.FirstOrDefault() + "') left join master_setting st on " +
                                "st.master_id = s.fstg and st.type = 'KPS' " +
                                "where s.client_unit_id = '" + unitid_mst + "'  and s.fstg in ('001', '" + model[0].SelectedItems2.FirstOrDefault() + "') group by(s.icode|| s.client_id || s.client_unit_id) ," +
                                "nvl(bm.acode, '-'),s.icode,s.iname,s.uom,s.fstg, st.master_name,bm.Alternate order by s.icode) a " +
                                "where a.fstr in ('" + URL + "') ";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                try
                                {
                                    if (model[0].dt1.Rows[0]["icode"].ToString().Trim().Equals("-")) { model[0].dt1.Rows.RemoveAt(0); }
                                    for (int i = 0; i < dtt.Rows.Count; i++)
                                    {
                                        DataRow dr = model[0].dt1.NewRow();
                                        dr["SNo"] = i + 1;
                                        dr["Icode"] = dtt.Rows[i]["icode"].ToString();
                                        dr["Iname"] = dtt.Rows[i]["iname"].ToString();
                                        dr["Uom"] = dtt.Rows[i]["uom"].ToString();
                                        dr["Qtystk"] = dtt.Rows[i]["qtystk"].ToString();
                                        dr["Stgcode"] = dtt.Rows[i]["fstgcode"].ToString();
                                        dr["Stg"] = dtt.Rows[i]["fstg"].ToString();
                                        dr["Alternate"] = dtt.Rows[i]["Alternate"].ToString();
                                        dr["Est_consume_qty"] = dtt.Rows[i]["bmiqty"].ToString();
                                        dr["Act_consume_qty"] = dtt.Rows[i]["bmiqty"].ToString();
                                        dr["short_excess"] = sgen.Make_decimal(dtt.Rows[i]["bmiqty"].ToString()) - sgen.Make_decimal(dtt.Rows[i]["bmiqty"].ToString());
                                        if (dtt.Rows[i]["Alternate"].ToString().Trim().Equals("Y"))
                                        {
                                            dr["Act_consume_qty"] = "0";
                                            dr["short_excess"] = "0";
                                        }
                                        model[0].dt1.Rows.Add(dr);
                                    }
                                    model[0].dt1.AcceptChanges();
                                    ViewBag.vgetdt = "";
                                    ViewBag.vchngdt = "disabled='disabled'";
                                    #region loc grid
                                    //if (model[0].dt2 != null)
                                    //{
                                    //    if (model[0].dt2.Rows[0]["icode"].ToString().Trim().Equals("-")) { model[0].dt2.Rows.RemoveAt(0); }
                                    //    decimal bmiqty = 0;
                                    //    for (int i = 0; i < dtt.Rows.Count; i++)
                                    //    {
                                    //        DataRow dr = model[0].dt2.NewRow();
                                    //        dr["SNo"] = i + 1;
                                    //        dr["Icode"] = dtt.Rows[i]["icode"].ToString();
                                    //        dr["Iname"] = dtt.Rows[i]["iname"].ToString();
                                    //        dr["Uom"] = dtt.Rows[i]["uom"].ToString();
                                    //        dr["Qty_stk"] = dtt.Rows[i]["lciqty"].ToString();
                                    //        try
                                    //        {
                                    //            //bmiqty = sgen.Make_decimal(sgen.Make_decimal(model[0].Col23) * (sgen.Make_decimal(dtt.Rows[i]["estqty"].ToString()) / sgen.Make_decimal(dtt.Rows[i]["lotqty"].ToString())));
                                    //            bmiqty = sgen.Make_decimal((sgen.Make_decimal(model[0].Col23) + sgen.Make_decimal(model[0].Col30)) * (sgen.Make_decimal(dtt.Rows[i]["estqty"].ToString()) / sgen.Make_decimal(dtt.Rows[i]["lotqty"].ToString())));
                                    //        }
                                    //        catch (Exception ex) { bmiqty = 0; }
                                    //        var lciqty = sgen.Make_decimal(dtt.Rows[i]["lciqty"].ToString());
                                    //        if (lciqty > bmiqty)
                                    //        {
                                    //            try
                                    //            {
                                    //                //dr["Act_Consume_Qty"] = (sgen.Make_decimal(model[0].Col23) * (sgen.Make_decimal(dtt.Rows[i]["estqty"].ToString()) / sgen.Make_decimal(dtt.Rows[i]["lotqty"].ToString())));
                                    //                dr["Act_Consume_Qty"] = ((sgen.Make_decimal(model[0].Col23) + sgen.Make_decimal(model[0].Col30)) * (sgen.Make_decimal(dtt.Rows[i]["estqty"].ToString()) / sgen.Make_decimal(dtt.Rows[i]["lotqty"].ToString())));
                                    //            }
                                    //            catch (Exception ex) { dr["Act_Consume_Qty"] = 0; }
                                    //        }
                                    //        else
                                    //        {
                                    //            dr["Act_Consume_Qty"] = dtt.Rows[i]["lciqty"].ToString();
                                    //        }
                                    //        dr["Loc"] = dtt.Rows[i]["loc"].ToString();
                                    //        dr["LocName"] = dtt.Rows[i]["locname"].ToString();
                                    //        model[0].dt2.Rows.Add(dr);
                                    //    }
                                    //    model[0].dt2.AcceptChanges();
                                    //}
                                    #endregion
                                }
                                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1," + ex.Message.ToString() + ",0);"; }
                            }
                            break;
                        #region wiploc
                        //case "WIPLOC":
                        //    mq = "select (bm.client_id||bm.client_unit_id||bm.icode||to_char(bm.vch_date,'yyyymmdd')||bm.type) fstr,bm.acode MainItem,bm.icode,it.iname,pum.master_name uom," +
                        //              "bm.pono stgcode, bm.cond stg, sum(to_number(bm.qtyin) + to_number(bm.qtyout)) bmiqty,sum(to_number(lc.qtyin) + to_number(lc.qtyout)) lciqty,bm.irate lotqty,lc.loc," +
                        //              "location_name(bm.client_id || bm.client_unit_id || lc.loc) locname from itransactionc bm " +
                        //              "inner join item it on it.icode = bm.icode and it.type = 'IT' and it.client_id = bm.client_id and it.client_unit_id = bm.client_unit_id " +
                        //              "inner join master_setting pum on pum.master_id = it.uom and pum.type = 'UMM' and find_in_set(pum.client_id, bm.client_id)= 1 and " +
                        //              "find_in_set(pum.client_unit_id, bm.client_unit_id)= 1 " +
                        //              "inner join btchtrans lc on lc.icode = bm.icode and lc.type in ('30', '32') and lc.client_id = bm.client_id and lc.client_unit_id = bm.client_unit_id and " +
                        //              "lc.pono = 'W' and lc.loc = '" + model[0].SelectedItems7.FirstOrDefault() + "' " +
                        //              "where bm.acode = '" + model[0].Col19 + "' and bm.pono = '" + model[0].SelectedItems2.FirstOrDefault().Trim() + "' and " +
                        //              "(bm.client_id||bm.client_unit_id||bm.icode||to_char(bm.vch_date,'yyyymmdd')||bm.type)='" + URL + "' " +
                        //              "group by(bm.client_id|| bm.client_unit_id || bm.icode || to_char(bm.vch_date, 'yyyymmdd') || bm.type),bm.acode,bm.icode,it.iname,pum.master_name,bm.pono," +
                        //              "bm.cond,bm.irate,lc.loc,location_name(bm.client_id || bm.client_unit_id || lc.loc)";
                        //    dtt = new DataTable();
                        //    dtt = sgen.getdata(userCode, mq);
                        //    if (dtt.Rows.Count > 0)
                        //    {
                        //        try
                        //        {
                        //            if (model[0].dt2.Rows[0]["icode"].ToString().Trim().Equals("-")) { model[0].dt2.Rows.RemoveAt(0); }
                        //            for (int i = 0; i < dtt.Rows.Count; i++)
                        //            {
                        //                DataRow dr = model[0].dt2.NewRow();
                        //                dr["SNo"] = i + 1;
                        //                dr["Icode"] = dtt.Rows[i]["icode"].ToString();
                        //                dr["Iname"] = dtt.Rows[i]["iname"].ToString();
                        //                dr["Uom"] = dtt.Rows[i]["uom"].ToString();
                        //                dr["Qty_stk"] = dtt.Rows[i]["lciqty"].ToString();
                        //                dr["Act_Consume_Qty"] = "0";
                        //                dr["Loc"] = dtt.Rows[i]["loc"].ToString();
                        //                dr["LocName"] = dtt.Rows[i]["locname"].ToString();
                        //                model[0].dt2.Rows.Add(dr);
                        //            }
                        //            model[0].dt2.AcceptChanges();
                        //        }
                        //        catch (Exception exx) { ViewBag.scripCall = "showmsgJS(1," + exx.Message.ToString() + ",0);"; }
                        //    }
                        //    break;
                        #endregion
                        case "NEW":
                            mq = "select master_id,upper(master_name) master_name,to_char(convert_tz(utc_timestamp(),'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') pdo_date " +
                                "from master_Setting where (master_id||type) = '" + URL + "'";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                try
                                {
                                    sgen.SetSession(MyGuid, "EDMODE", "N");
                                    sgen.SetSession(MyGuid, "btnval", "NEW");
                                    ViewBag.vnew = "disabled='disabled'";
                                    ViewBag.vedit = "disabled='disabled'";
                                    ViewBag.vsave = "";
                                    ViewBag.vsavenew = "";
                                    ViewBag.scripCall = "enableForm();";
                                    mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + dtt.Rows[0]["master_id"].ToString() + "'" + model[0].Col11.Trim() + "";
                                    vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                                    model[0].Col16 = vch_num;
                                    model[0].Col9 = dtt.Rows[0]["master_name"].ToString();
                                    model[0].Col12 = dtt.Rows[0]["master_id"].ToString();
                                    model[0].Col17 = sgen.server_datetime_local(userCode);
                                    model[0].Col38 = sgen.server_datetime_local(userCode);
                                    model[0].Col39 = sgen.server_datetime_local(userCode);
                                    model[0].Col40 = sgen.server_datetime_local(userCode);
                                    model[0].Col22 = "1";
                                    model[0].Col101 = "C";// consumption pd                                    
                                    mod2 = new List<SelectListItem>();
                                    mod3 = new List<SelectListItem>();
                                    mod4 = new List<SelectListItem>();
                                    mod5 = new List<SelectListItem>();
                                    mod6 = new List<SelectListItem>();
                                    mod7 = new List<SelectListItem>();
                                    //op name
                                    mod3 = cmd_Fun.opname(userCode, unitid_mst);
                                    TempData[MyGuid + "_TList3"] = mod3;
                                    //machine
                                    //mod4 = cmd_Fun.mcname(userCode, clientid_mst, unitid_mst);
                                    TempData[MyGuid + "_TList4"] = mod4;
                                    //mould
                                    //mod5 = cmd_Fun.mldname(userCode, unitid_mst);
                                    TempData[MyGuid + "_TList5"] = mod5;
                                    //shift
                                    mod6 = cmd_Fun.shftname(userCode, unitid_mst);
                                    TempData[MyGuid + "_TList6"] = mod6;
                                    TempData[MyGuid + "_TList7"] = mod7;
                                    model[0].TList2 = mod2;
                                    model[0].SelectedItems2 = new string[] { "" };
                                    model[0].TList3 = mod3;
                                    model[0].SelectedItems3 = new string[] { "" };
                                    model[0].TList4 = mod4;
                                    model[0].SelectedItems4 = new string[] { "" };
                                    model[0].TList5 = mod5;
                                    model[0].SelectedItems5 = new string[] { "" };
                                    model[0].TList6 = mod6;
                                    model[0].SelectedItems6 = new string[] { "" };
                                    model[0].TList7 = mod7;
                                    model[0].SelectedItems7 = new string[] { "" };
                                    ViewBag.vgetdt = "";
                                    ViewBag.vchngdt = "disabled='disabled'";
                                    //if (dtt.Rows[0]["master_id"].ToString().Trim().Equals("101"))
                                    //{
                                    //    Make_query(viewName.ToLower(), "Select Production Order No", "WPDO", "2", "", "");
                                    //    ViewBag.scripCall = "callFoo('Select Production Order No');";
                                    //}
                                }
                                catch (Exception ex) { }
                            }
                            break;
                    }
                    break;
                #endregion
                #region iprodn
                case "iprodn":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select ex.vch_num,ex.type,to_char(convert_tz(ex.vch_date, 'UTC', '" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') pddate," +
                                "to_char(convert_tz(ex.date1,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') date1," +
                                "to_char(convert_tz(ex.date2,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') date2," +
                                "to_char(convert_tz(ex.date3,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') date3,ex.acode," +
                             "ex.mstage fstg,ex.stage1 tstg,ex.opcode,ex.mccode,ex.mouldcode,ex.shftno,ex.qtyin pqty,ex.qtyrej,ex.icode,it.iname,um.master_name uom,ex.qtychl estcon_qty," +
                             "ex.qtyout actcon_qty,ex.qtyplanned shortex,ex.client_id,ex.client_unit_id,ex.ent_by,ex.ent_date,ex.stime,ex.etime,ex.pflag,ex.rno,ex.qtystk,ex.cstk,ex.stage," +
                             "ex.alt,ex.qtyrw,ex.pdono,to_char(convert_tz(ex.pdodt, 'UTC', '" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') pdodt,ex.qtypdo," +
                             "ex.col1,ex.col2,ex.col3,ex.col4,ex.col5,ex.col6,ex.mctxt,ex.mouldtxt " +
                             "from iprod ex " +
                             "inner join item it on it.icode=ex.icode and it.type='IT' and find_in_set(it.client_unit_id,ex.client_unit_id)=1 " +
                             "inner join master_setting um on um.master_id=it.uom and um.type='UMM' and find_in_set(um.client_unit_id,it.client_unit_id)=1 " +
                             "where (ex.client_id||ex.client_unit_id||ex.vch_num||to_char(ex.vch_date,'yyyymmdd')||ex.type)='" + URL + "'";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                //op name
                                mod3 = cmd_Fun.opname(userCode, unitid_mst);
                                TempData[MyGuid + "_TList3"] = mod3;
                                //machine
                                mod4 = cmd_Fun.mcname(userCode, unitid_mst);
                                TempData[MyGuid + "_TList4"] = mod4;
                                //mould
                                //mod5 = cmd_Fun.mldname(userCode, unitid_mst);
                                //TempData[MyGuid + "_TList5"] = mod5;
                                //shift
                                mod6 = cmd_Fun.shftname(userCode, unitid_mst);
                                TempData[MyGuid + "_TList6"] = mod6;
                                //iloc  
                                mod7 = mod4;
                                TempData[MyGuid + "_TList7"] = mod7;
                                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                                model[0].Col8 = "(client_id||client_unit_id||vch_num||to_char(vch_date,'yyyymmdd')||type) = '" + URL + "'";
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
                                model[0].Col12 = dtt.Rows[0]["type"].ToString();
                                model[0].Col16 = dtt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dtt.Rows[0]["pddate"].ToString();
                                model[0].Col25 = dtt.Rows[0]["stime"].ToString();
                                model[0].Col26 = dtt.Rows[0]["etime"].ToString();
                                model[0].Col29 = dtt.Rows[0]["cstk"].ToString();
                                model[0].Col96 = dtt.Rows[0]["rno"].ToString();
                                model[0].Col101 = dtt.Rows[0]["pflag"].ToString();
                                model[0].Col98 = dtt.Rows[0]["tstg"].ToString();
                                model[0].Col23 = dtt.Rows[0]["pqty"].ToString();
                                model[0].Col30 = dtt.Rows[0]["qtyrej"].ToString();
                                model[0].Col31 = dtt.Rows[0]["qtyrw"].ToString();
                                model[0].Col32 = dtt.Rows[0]["pdono"].ToString();
                                model[0].Col33 = dtt.Rows[0]["pdodt"].ToString();
                                model[0].Col34 = dtt.Rows[0]["qtypdo"].ToString();
                                model[0].Col35 = dtt.Rows[0]["col1"].ToString();
                                model[0].Col36 = dtt.Rows[0]["col2"].ToString();
                                model[0].Col37 = dtt.Rows[0]["col3"].ToString();
                                model[0].Col38 = dtt.Rows[0]["date1"].ToString();
                                model[0].Col39 = dtt.Rows[0]["date2"].ToString();
                                model[0].Col40 = dtt.Rows[0]["date3"].ToString();
                                model[0].Col41 = dtt.Rows[0]["col4"].ToString();
                                model[0].Col42 = dtt.Rows[0]["col5"].ToString();
                                model[0].Col43 = dtt.Rows[0]["col6"].ToString();
                                model[0].Col50 = dtt.Rows[0]["mctxt"].ToString();
                                model[0].Col51 = dtt.Rows[0]["mouldtxt"].ToString();
                                model[0].TList3 = mod3;
                                model[0].TList4 = mod4;
                                //model[0].TList5 = mod5;
                                model[0].TList6 = mod6;
                                model[0].TList7 = mod7;
                                model[0].SelectedItems3 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["opcode"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems4 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["mccode"].ToString()).Distinct()).Split(',');
                                //model[0].SelectedItems5 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["mouldcode"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems6 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["shftno"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems7 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["mccode"].ToString()).Distinct()).Split(',');
                                mq = "select it.icode as Icode,it.iname as Iname,pum.master_name as UOM from item it " +
                               "inner join master_setting pum on pum.master_id=it.uom and pum.type='UMM' and find_in_set(pum.client_unit_id,it.client_unit_id)=1 " +
                               "where find_in_set(it.client_unit_id,'" + unitid_mst + "')=1 and it.type='IT' and it.icode='" + dtt.Rows[0]["acode"].ToString() + "'";
                                DataTable dtm = sgen.getdata(userCode, mq);
                                if (dtm.Rows.Count > 0)
                                {
                                    model[0].Col18 = dtm.Rows[0]["Icode"].ToString();
                                    model[0].Col19 = dtm.Rows[0]["Icode"].ToString();
                                    model[0].Col20 = dtm.Rows[0]["Iname"].ToString();
                                    model[0].Col21 = dtm.Rows[0]["UOM"].ToString();
                                    //mq = "select * from (select bm.pono stgcode,bm.cond stg,bm.rno from itransactionc bm where bm.type = 'BOM' and bm.client_id='" + clientid_mst + "' " +
                                    //    "and bm.client_unit_id='" + unitid_mst + "' and bm.acode = '" + dtt.Rows[0]["acode"].ToString() + "' " +
                                    //    "union " +
                                    //    "select '001' stgcode,'0' stg,'0' rno from dual) a order by rno asc";                                 

                                    mq = "select bm.pono stgcode,bm.cond stg,bm.rno from itransactionc bm where bm.type = 'BOM' and bm.client_unit_id='" + unitid_mst + "' and bm.acode = '" + dtt.Rows[0]["acode"].ToString() + "' order by rno asc";
                                    dtp = sgen.getdata(userCode, mq);
                                    DataTable stgs = new DataTable();
                                    stgs.Columns.Add("stgcode");
                                    string lastscode = "", newscode = "";
                                    if (dtp.Rows.Count > 0)
                                    {
                                        for (int i = 0; i < dtp.Rows.Count; i++)
                                        {
                                            newscode = dtp.Rows[i]["stgcode"].ToString().Trim();
                                            if (newscode != lastscode)
                                            {
                                                DataRow dr = stgs.NewRow();
                                                dr[0] = newscode;
                                                stgs.Rows.Add(dr);
                                                lastscode = newscode;
                                            }
                                        }
                                        stgs.AcceptChanges();
                                        DataTable dts = sgen.getdata(userCode, "SELECT master_id stgcode,master_name stg FROM master_setting where type = 'KPS'");
                                        var results = from table1 in stgs.AsEnumerable()
                                                      join table2 in dts.AsEnumerable() on table1["stgcode"] equals table2["stgcode"]
                                                      select new
                                                      {
                                                          stgcode = table1["stgcode"],
                                                          stg = table2["stg"]
                                                      };
                                        results.ToList();
                                        DataTable dtkk = sgen.ToDataTable(results.ToList());
                                        //if (dtkk.Rows.Count > 0)
                                        //{
                                        //    foreach (DataRow dr in dtkk.Rows)
                                        //    {
                                        //        mod1.Add(new SelectListItem { Text = dr["stg"].ToString(), Value = dr["stgcode"].ToString() });
                                        //    }
                                        //}
                                        //TempData[MyGuid + "_TList1"] = mod1;
                                        //model[0].TList1 = mod1;
                                        //dtkk = dtkk.Select("stgcode not in ('001')").CopyToDataTable();
                                        if (dtkk.Rows.Count > 0)
                                        {
                                            foreach (DataRow dr in dtkk.Rows)
                                            {
                                                mod2.Add(new SelectListItem { Text = dr["stg"].ToString(), Value = dr["stgcode"].ToString() });
                                            }
                                        }
                                        TempData[MyGuid + "_TList2"] = mod2;
                                        model[0].TList2 = mod2;
                                    }
                                }
                                model[0].SelectedItems2 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["fstg"].ToString()).Distinct()).Split(',');

                                mq = "select b.tmode mldid,m.col31 mldname from itransactionc b inner join kc_tab m on m.vch_num=b.tmode and m.type = 'MOS' and m.client_unit_id = b.client_unit_id " +
                                    "where b.type='BOM' and b.acode='" + model[0].Col18 + "' and b.pono='" + model[0].SelectedItems2.FirstOrDefault() + "'";
                                dtp = sgen.getdata(userCode, mq);
                                if (dtp.Rows.Count > 0) { foreach (DataRow dr in dtp.Rows) { mod5.Add(new SelectListItem { Text = dr["mldname"].ToString(), Value = dr["mldid"].ToString() }); } }
                                else { mod5 = cmd_Fun.mldname(userCode, unitid_mst); }
                                model[0].TList5 = mod5;
                                TempData[MyGuid + "_TList5"] = mod5;
                                model[0].SelectedItems5 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["mouldcode"].ToString()).Distinct()).Split(',');
                                //model[0].SelectedItems1 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["fstg"].ToString()).Distinct()).Split(',');

                                model[0].dt1 = new DataTable();
                                model[0].dt1 = ((DataTable)sgen.GetSession(MyGuid, "dtipd")).Clone();
                                for (int i = 0; i < dtt.Rows.Count; i++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["sno"] = i + 1;
                                    dr["Icode"] = dtt.Rows[i]["icode"].ToString();
                                    dr["Iname"] = dtt.Rows[i]["iname"].ToString();
                                    dr["Uom"] = dtt.Rows[i]["uom"].ToString();//puom
                                    dr["Qtystk"] = dtt.Rows[i]["qtystk"].ToString();//puom
                                    dr["Stgcode"] = dtt.Rows[i]["stage"].ToString();//puom
                                    dr["Stg"] = dtt.Rows[i]["stage"].ToString();//puom
                                    dr["Est_consume_qty"] = dtt.Rows[i]["estcon_qty"].ToString();//estimated consume qty
                                    dr["Act_consume_qty"] = dtt.Rows[i]["actcon_qty"].ToString();//actual consume qty
                                    dr["Short_Excess"] = dtt.Rows[i]["shortex"].ToString();//shortexcess
                                    dr["Alternate"] = dtt.Rows[i]["alt"].ToString();//shortexcess
                                    model[0].dt1.Rows.Add(dr);
                                }
                                model[0].dt1.AcceptChanges();
                                #region
                                //mq1 = "select (i.client_unit_id||i.icode||i.loc) fstr,i.icode,it.iname,um.master_name uom,i.qtystk,i.qtyout,i.loc,i.locname from btchtrans i " +
                                // "inner join item it on it.icode=i.icode and it.type='IT' and it.client_id=i.client_id and it.client_unit_id=i.client_unit_id " +
                                // "inner join master_setting um on um.master_id=it.uom and um.type='UMM' and find_in_set(um.client_id,it.client_id)=1 and find_in_set(um.client_unit_id,it.client_unit_id)=1 " +
                                // "where (i.client_id||i.client_unit_id||i.vch_num|| to_char(i.vch_date, 'yyyymmdd')||i.type)='" + URL + "'";
                                //dtt = sgen.getdata(userCode, mq1);
                                //if (dtt.Rows.Count > 0)
                                //{
                                //    model[0].dt2 = new DataTable();
                                //    DataTable dt2 = sgen.getdata(userCode, "select '-' Sno, '-' Icode, '-' Iname, '-' Uom, '0' Qty_stk, '0' Act_Consume_Qty, '0' Loc, '-' LocName from dual");
                                //    Session["dtipd2"] = dt2;
                                //    model[0].dt2 = ((DataTable)Session["dtipd2"]).Clone();
                                //    for (int i = 0; i < dtt.Rows.Count; i++)
                                //    {
                                //        DataRow dr = model[0].dt2.NewRow();
                                //        //dr["sno"] = dtt.Rows[i]["fstr"].ToString();
                                //        dr["sno"] = (i + 1).ToString();
                                //        dr["IName"] = dtt.Rows[i]["iname"].ToString();
                                //        dr["Icode"] = dtt.Rows[i]["icode"].ToString();
                                //        dr["UOM"] = dtt.Rows[i]["uom"].ToString();
                                //        dr["Qty_Stk"] = dtt.Rows[i]["qtystk"].ToString();
                                //        dr["Act_Consume_Qty"] = dtt.Rows[i]["qtyout"].ToString();
                                //        dr["Loc"] = dtt.Rows[i]["loc"].ToString();
                                //        dr["LocName"] = dtt.Rows[i]["locname"].ToString();
                                //        model[0].dt2.Rows.Add(dr);
                                //    }
                                //    model[0].dt2.AcceptChanges();
                                //}
                                #endregion
                            }
                            break;
                        case "ITEM": //main item                         
                            type = model[0].Col12;
                            if (type.Trim().Equals("100"))
                            {
                                mq = "select bm.acode,it.iname,pum.master_name uom,bm.irate lotqty,bm.pono stgcode,bm.rno from itransactionc bm " +
                                    "inner join item it on it.icode = bm.acode and it.type = 'IT' and find_in_set(it.client_unit_id, bm.client_unit_id)=1 " +
                                    "inner join master_setting pum on pum.master_id = it.uom and pum.type = 'UMM' and find_in_set(pum.client_unit_id, bm.client_unit_id)= 1 " +
                                    //"inner join master_setting s on s.master_id = bm.pono and s.type = 'KPS' and find_in_set(s.client_id, bm.client_id)= 1 and find_in_set(s.client_unit_id, bm.client_unit_id)= 1 " +
                                    "where(bm.client_id || bm.client_unit_id || bm.acode || bm.type) = '" + URL + "' order by bm.rno asc";
                            }
                            else
                            {
                                //mq = "select bm.acode,it.iname,pum.master_name uom,bm.irate lotqty,bm.pono stgcode,bm.rno,w.vch_num pdono," +
                                //    "to_char(w.vch_date, '" + sgen.Getsqldateformat() + "') pdodt,w.col9 ord_qty,w.col10 plan_qty from itransactionc bm " +
                                //    "inner join item it on it.icode = bm.acode and it.type = 'IT' and it.client_id = bm.client_id and it.client_unit_id = bm.client_unit_id " +
                                //    "inner join master_setting pum on pum.master_id = it.uom and pum.type = 'UMM' and find_in_set(pum.client_id, bm.client_id)= 1 and find_in_set(pum.client_unit_id, bm.client_unit_id)= 1 " +
                                //    "inner join kc_tab w on w.col5=bm.acode and w.type='PSO' and w.client_id=bm.client_id and w.client_unit_id=bm.client_unit_id " +
                                //    "where (bm.client_id || bm.client_unit_id || bm.acode || bm.type) = '" + URL + "' order by bm.rno asc";
                                
                                
                               
                                mq = "select bm.acode,it.iname,pum.master_name uom,bm.irate lotqty,bm.pono stgcode,bm.rno,w.vch_num pdono, to_char(w.vch_date, '" + sgen.Getsqldateformat() + "') pdodt," +
                                    "w.Bal_qty plan_qty, w.ord_qty from itransactionc bm " +
                                    "inner join item it on it.icode = bm.acode and it.type = 'IT' and find_in_set(it.client_unit_id, bm.client_unit_id)=1 " +
                                    "inner join master_setting pum on pum.master_id = it.uom and pum.type = 'UMM' and find_in_set(pum.client_unit_id, bm.client_unit_id)= 1 " +
                                    "inner join (select a.vch_num, a.vch_date, a.type, a.icode, a.client_id, a.client_unit_id, (a.ord_qty), sum(to_number(a.used_qty)) as used_qty,(a.ord_qty - nvl(sum(a.used_qty), '0')) as Bal_qty " +
                                    "from(select w.vch_num, w.vch_date, w.col5 as icode, w.client_id,w.client_unit_id, w.type, (case when w.col10 = '-' then '0' else w.col10 end) as ord_qty,nvl(ex.qtyin, '0') used_qty from kc_tab w " +
                                    "left join iprod ex on ex.pdono = w.vch_num and ex.client_unit_id = w.client_unit_id and ex.type='101' " +
                                    "where w.client_unit_id = '" + unitid_mst + "' and w.type = 'PSO' union all " +
                                    "select w.vch_num,w.vch_date,w.col5 as icode,w.client_id,w.client_unit_id,w.type,(case when w.col10 = '-' then '0' else w.col10 end) ord_qty," +
                                    "nvl(to_number(ex.col11), '0') as close_qty from kc_tab w left join enx_tab ex on ex.col12 = w.vch_num and ex.client_unit_id = w.client_unit_id " +
                                    "and ex.type='COR' where w.client_unit_id = '" + unitid_mst + "' and w.type = 'PSO') a group by(a.vch_num, a.vch_date, a.type, a.icode, a.client_id,a.client_unit_id, a.ord_qty)) w on w.icode = bm.acode and w.client_unit_id = bm.client_unit_id where (bm.client_id || bm.client_unit_id || bm.acode || bm.type) = '" + URL + "' order by bm.rno asc";
                            }
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                model[0].Col18 = dtt.Rows[0]["acode"].ToString();
                                model[0].Col19 = dtt.Rows[0]["acode"].ToString();
                                model[0].Col20 = dtt.Rows[0]["iname"].ToString();
                                model[0].Col21 = dtt.Rows[0]["uom"].ToString();
                                model[0].Col97 = dtt.Rows[0]["lotqty"].ToString();//bom_lotqty
                                model[0].Col30 = "0"; //rej qty
                                model[0].Col31 = "0"; //rw qty
                                if (type.Trim().Equals("101"))
                                {
                                    model[0].Col32 = dtt.Rows[0]["pdono"].ToString();//pdo no
                                    model[0].Col33 = dtt.Rows[0]["pdodt"].ToString();//pdo dt
                                    model[0].Col34 = dtt.Rows[0]["plan_qty"].ToString();//plan qty
                                }

                                model[0].TList5 = mod5;
                                TempData[MyGuid + "_TList5"] = mod5;

                                //mq = "select bm.acode MainItem,bm.pono stgcode,bm.cond stg,bm.rno from itransactionc bm where bm.type = 'BOM' and " +
                                //    "(bm.client_id||bm.client_unit_id||bm.acode) = '" + URL + "' order by bm.rno asc";
                                //mq = "select * from (select bm.pono stgcode,bm.cond stg,bm.rno from itransactionc bm where bm.type = 'BOM' " +
                                //    "and (bm.client_id || bm.client_unit_id || bm.acode) = '" + URL + "' " +
                                //    "union " +
                                //    "select '001' stgcode,'0' stg,'0' rno from dual) a order by rno asc";
                                //mq = "select bm.pono stgcode,bm.cond stg,bm.rno from itransactionc bm where bm.type = 'BOM' and " +
                                //    "(bm.client_id || bm.client_unit_id || bm.acode) = '" + URL + "'order by rno asc";
                                //dtp = sgen.getdata(userCode, mq);
                                DataTable stgs = new DataTable();
                                stgs.Columns.Add("stgcode");
                                string lastscode = "", newscode = "";
                                //if (dtp.Rows.Count > 0)
                                //{
                                for (int i = 0; i < dtt.Rows.Count; i++)
                                {
                                    newscode = dtt.Rows[i]["stgcode"].ToString().Trim();
                                    if (newscode != lastscode)
                                    {
                                        DataRow dr = stgs.NewRow();
                                        dr[0] = newscode;
                                        stgs.Rows.Add(dr);
                                        lastscode = newscode;
                                    }
                                }
                                stgs.AcceptChanges();
                                DataTable dts = sgen.getdata(userCode, "SELECT master_id stgcode,master_name stg FROM master_setting where type = 'KPS'");
                                var results = from table1 in stgs.AsEnumerable()
                                              join table2 in dts.AsEnumerable() on table1["stgcode"] equals table2["stgcode"]
                                              select new
                                              {
                                                  stgcode = table1["stgcode"],
                                                  stg = table2["stg"]
                                              };
                                results.ToList();
                                DataTable dtkk = sgen.ToDataTable(results.ToList());
                                if (dtkk.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dtkk.Rows)
                                    {
                                        mod2.Add(new SelectListItem { Text = dr["stg"].ToString(), Value = dr["stgcode"].ToString() });
                                    }
                                }
                                //TempData[MyGuid + "_TList1"] = mod1;
                                //model[0].TList1 = mod1;
                                TempData[MyGuid + "_TList2"] = mod2;
                                model[0].TList2 = mod2;
                                //dtkk = dtkk.Select("stgcode not in ('001')").CopyToDataTable();
                                //if (stgs.Rows.Count > 0)
                                //{
                                //    foreach (DataRow dr in stgs.Rows)
                                //    {
                                //        mod2.Add(new SelectListItem { Text = dr["stg"].ToString(), Value = dr["stgcode"].ToString() });
                                //    }
                                //}
                                //TempData[MyGuid + "_TList2"] = mod2;
                                //model[0].TList2 = mod2;
                                //}
                            }
                            else
                            {
                                ViewBag.scripCall += "showmsgJS(1,'No data found for this item,please check again.', 2);";
                                return model;
                            }
                            break;
                        case "PDREQ": //pd bom calc stage wise                            
                            model[0].dt1 = ((DataTable)sgen.GetSession(MyGuid, "dtipd")).Copy();
                            var totqty = sgen.Make_decimal(model[0].Col23) + sgen.Make_decimal(model[0].Col30);
                            var sumqty = sgen.Make_decimal(model[0].Col23) + sgen.Make_decimal(model[0].Col30) + sgen.Make_decimal(model[0].Col31);
                            decimal qtyrw = sgen.Make_decimal(model[0].Col31);
                            mq = "select bm.pono stgcode,bm.cond stg,bm.rno from itransactionc bm where bm.type = 'BOM' and " +
                               "bm.client_unit_id = '" + unitid_mst + "' and bm.acode = '" + model[0].Col19 + "' order by bm.rno asc";
                            dtp = sgen.getdata(userCode, mq);
                            DataTable stgss = new DataTable();
                            stgss.Columns.Add("stgcode");
                            string lastcode = "", newcode = "", prestg = "", fststg = "";
                            if (dtp.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtp.Rows.Count; i++)
                                {
                                    newcode = dtp.Rows[i]["stgcode"].ToString().Trim();
                                    if (newcode != lastcode)
                                    {
                                        DataRow dr = stgss.NewRow();
                                        dr[0] = newcode;
                                        stgss.Rows.Add(dr);
                                        lastcode = newcode;
                                    }
                                }
                                stgss.AcceptChanges();
                                try
                                {
                                    int totstg = stgss.Rows.Count;
                                    int stg = sgen.seekval_dt_rowindex(stgss, "stgcode='" + model[0].SelectedItems2.FirstOrDefault() + "'");                                   
                                    if (stg > 1)
                                    {
                                        int stgidx = stg - 2;                                        
                                        prestg = stgss.Rows[stgidx]["stgcode"].ToString();
                                        model[0].Col99 = "";
                                        mq2 = " union all select '" + clientid_mst + "','" + unitid_mst + "','BOM','" + model[0].Col19 + "','" + model[0].SelectedItems2.FirstOrDefault() + "','" + model[0].Col97 + "','" + model[0].Col19 + "' icode ,'T' flg," + sumqty + " est,'N' Alternate from dual ";
                                    }
                                    else
                                    {
                                        mq2 = "";
                                        prestg = model[0].SelectedItems2.FirstOrDefault();
                                        fststg = model[0].SelectedItems1.FirstOrDefault();
                                        model[0].Col99 = "N";
                                        if (qtyrw > 0)
                                        {
                                            mq2 = " union all select '" + clientid_mst + "','" + unitid_mst + "','BOM','" + model[0].Col19 + "','" + model[0].SelectedItems2.FirstOrDefault() + "','" + model[0].Col97 + "','" + model[0].Col19 + "' icode ,'T' flg," + qtyrw + " est,'N' Alternate from dual ";
                                        }
                                    }
                                    model[0].Col101 = "C";//consumption pd
                                    if (totstg == stg) model[0].Col101 = "F";//final pd                                    
                                    try { model[0].Col98 = model[0].SelectedItems2.FirstOrDefault(); }//nxt stg
                                    catch (Exception err) { }
                                }
                                catch (Exception ex) { }
                            }
                            mq = " select * from (select nvl(bm.acode,'-') MainItem,max(nvl(est,0)) bmiqty,max(nvl(bm.irate,0)) lotqty,s.icode,s.iname,s.uom,sum(s.qtystk) qtystk,s.fstg fstgcode," +
                                "st.master_name fstg,bm.flg,bm.Alternate from " +
                                "(select i.icode, i.iname, um.master_name uom, sum(nvl(a.qtyin, 0)) qtyin, sum(nvl(a.qtyout, 0)) qtyout, sum(nvl(a.qtyin, 0)) - sum(nvl(a.qtyout, 0)) qtystk," +
                               "nvl(a.fstg, '001') fstg, i.type, i.client_id, i.client_unit_id from item i " +
                               "left join (select icode, (case when type='100' then 0 else qtyin end) qtyin, qtyout qtyout, stage fstg, type, " +
                               "client_id, client_unit_id,vch_num,vch_date from iprod where (type like '3%' or type = '10P' or type='100') " +
                               "union " +
                               "select acode icode, (to_number(max(qtyin))+to_number(max(qtyrw))) qtyin, 0 qtyout, stage1 fstg, type, client_id, client_unit_id ,vch_num,vch_date from iprod " +
                               "where type = '100' group by acode, stage1, type, client_id, client_unit_id ,vch_num,vch_date " +
                               "union " +
                               "select acode icode, to_number(max(qtyrej)) qtyin, 0 qtyout, '101' fstg, type, client_id, client_unit_id,vch_num,vch_date from iprod where type = '100' " +
                               "group by acode, type, client_id, client_unit_id,vch_num,vch_date " +
                               "union " +
                               "select icode, sum(qtyin) qtyin, 0 qtyout, stage1 fstg, type, client_id, client_unit_id ,vch_num,vch_date from iprod where type = '10R' " +
                               "group by icode, stage1, type, client_id, client_unit_id,vch_num,vch_date " +
                               "union " +
                               "select icode, 0 qtyin, sum(qtyout) qtyout, stage fstg, type,client_id, client_unit_id,vch_num,vch_date from iprod where type = '10R' " +
                               "group by icode, stage, type, client_id, client_unit_id,vch_num,vch_date " +
                               "union " +
                               "select icode, sum(qtyout) qtyin, 0 qtyout, stage1 fstg, type,client_id, client_unit_id,vch_num,vch_date from iprod where type = '10R' " +
                               "group by icode, stage1, type, client_id, client_unit_id,vch_num,vch_date) a " +
                               "on a.icode = i.icode and find_in_set(a.client_unit_id, i.client_unit_id)=1 " +
                               "inner join master_setting um on um.master_id = i.uom and um.type = 'UMM' and find_in_set(um.client_unit_id, i.client_unit_id)= 1 " +
                               "where i.type = 'IT' and find_in_set(i.client_unit_id, '" + unitid_mst + "')=1 " +
                               "group by i.icode,i.iname,um.master_name,nvl(a.fstg, '001'),i.type,i.client_id,i.client_unit_id) s " +
                               "inner join " +
                                "(select client_id, client_unit_id, type, acode, pono, irate, icode, 'B' as flg, (to_number(qtyin) + to_number(qtyout)) / to_number(irate) * " + totqty + " est," +
                                "(case when to_number(qtyin)=0 then 'Y' else 'N' end) Alternate " +
                               "from itransactionc where type = 'BOM' and acode='" + model[0].Col19 + "' and pono = '" + model[0].SelectedItems2.FirstOrDefault() + "' " + mq2 + ") bm " +
                               "on s.icode = bm.icode and s.client_unit_id = bm.client_unit_id and s.fstg in ('001', '" + fststg + "','" + prestg + "') " +
                               "inner join master_setting st on st.master_id = s.fstg and st.type = 'KPS' " +
                               "where s.client_unit_id = '" + unitid_mst + "' and s.fstg in ('001', '" + fststg + "','" + prestg + "') " +
                               "group by nvl(bm.acode, '-'),s.icode,s.iname,s.uom,s.fstg, st.master_name,bm.flg,bm.Alternate order by s.icode) a where a.qtystk<>0";
                            dtt = sgen.getdata(userCode, mq);
                            try
                            {
                                dtt = dtt.AsEnumerable().Except(dtt.Select(" icode='" + model[0].Col19.Trim() + "' and flg='B'")
                                    .CopyToDataTable().AsEnumerable(), DataRowComparer.Default).CopyToDataTable();
                            }
                            catch (Exception err) { }
                            if (dtt.Rows.Count > 0)
                            {
                                try
                                {
                                    if (model[0].dt1.Rows[0]["icode"].ToString().Trim().Equals("-")) { model[0].dt1.Rows.RemoveAt(0); }
                                    for (int i = 0; i < dtt.Rows.Count; i++)
                                    {
                                        DataRow dr = model[0].dt1.NewRow();
                                        dr["SNo"] = i + 1;
                                        dr["Icode"] = dtt.Rows[i]["icode"].ToString();
                                        dr["Iname"] = dtt.Rows[i]["iname"].ToString();
                                        dr["Uom"] = dtt.Rows[i]["uom"].ToString();
                                        dr["Qtystk"] = dtt.Rows[i]["qtystk"].ToString();
                                        dr["Stgcode"] = dtt.Rows[i]["fstgcode"].ToString();
                                        dr["Stg"] = dtt.Rows[i]["fstg"].ToString();
                                        dr["Alternate"] = dtt.Rows[i]["Alternate"].ToString();
                                        dr["Est_consume_qty"] = dtt.Rows[i]["bmiqty"].ToString();
                                        dr["Act_consume_qty"] = dtt.Rows[i]["bmiqty"].ToString();
                                        dr["short_excess"] = sgen.Make_decimal(dtt.Rows[i]["bmiqty"].ToString()) - sgen.Make_decimal(dtt.Rows[i]["bmiqty"].ToString());
                                        if (dtt.Rows[i]["Alternate"].ToString().Trim().Equals("Y"))
                                        {
                                            dr["Act_consume_qty"] = "0";
                                            dr["short_excess"] = "0";
                                        }
                                        model[0].dt1.Rows.Add(dr);
                                    }
                                    model[0].dt1.AcceptChanges();
                                }
                                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1," + ex.Message.ToString() + ",0);"; }
                            }
                            break;
                        case "PDITEM": //pd + click item                            
                            var totqty1 = sgen.Make_decimal(model[0].Col23) + sgen.Make_decimal(model[0].Col30);
                            var sumqty1 = sgen.Make_decimal(model[0].Col23) + sgen.Make_decimal(model[0].Col30) + sgen.Make_decimal(model[0].Col31);
                            decimal qtyrw1 = sgen.Make_decimal(model[0].Col31);
                            mq = "select bm.pono stgcode,bm.cond stg,bm.rno from itransactionc bm where bm.client_unit_id = '" + unitid_mst + "' and bm.type = 'BOM' and bm.acode = '" + model[0].Col19 + "' order by bm.rno asc";
                            dtp = sgen.getdata(userCode, mq);
                            DataTable stgss1 = new DataTable();
                            stgss1.Columns.Add("stgcode");
                            string lastcode1 = "", newcode1 = "";
                            prestg = ""; fststg = "";
                            if (dtp.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtp.Rows.Count; i++)
                                {
                                    newcode1 = dtp.Rows[i]["stgcode"].ToString().Trim();
                                    if (newcode1 != lastcode1)
                                    {
                                        DataRow dr = stgss1.NewRow();
                                        dr[0] = newcode1;
                                        stgss1.Rows.Add(dr);
                                        lastcode1 = newcode1;
                                    }
                                }
                                stgss1.AcceptChanges();
                                try
                                {
                                    int totstg = stgss1.Rows.Count;
                                    int stg = sgen.seekval_dt_rowindex(stgss1, "stgcode='" + model[0].SelectedItems2.FirstOrDefault() + "'");
                                    if (stg > 1)
                                    {
                                        int stgidx = stg - 2;
                                        prestg = stgss1.Rows[stgidx]["stgcode"].ToString();
                                        model[0].Col99 = "";
                                        mq2 = " union all select '" + clientid_mst + "','" + unitid_mst + "','BOM','" + model[0].Col19 + "','" + model[0].SelectedItems2.FirstOrDefault() + "','" + model[0].Col97 + "','" + model[0].Col19 + "' icode ,'T' flg," + sumqty1 + " est,'N' Alternate from dual ";
                                    }
                                    else
                                    {
                                        mq2 = "";
                                        prestg = model[0].SelectedItems2.FirstOrDefault();
                                        fststg = model[0].SelectedItems1.FirstOrDefault();
                                        model[0].Col99 = "N";
                                        if (qtyrw1 > 0)
                                        {
                                            mq2 = " union all select '" + clientid_mst + "','" + unitid_mst + "','BOM','" + model[0].Col19 + "','" + model[0].SelectedItems2.FirstOrDefault() + "','" + model[0].Col97 + "','" + model[0].Col19 + "' icode ,'T' flg," + qtyrw1 + " est,'N' Alternate from dual ";
                                        }
                                    }
                                    model[0].Col101 = "C";//consumption pd
                                    if (totstg == stg) model[0].Col101 = "F";//final pd                                    
                                    try { model[0].Col98 = model[0].SelectedItems2.FirstOrDefault(); }//nxt stg
                                    catch (Exception err) { }
                                }
                                catch (Exception ex) { }
                            }
                            mq = "select * from (select (s.icode||s.client_id||s.client_unit_id) fstr,nvl(bm.acode,'-') MainItem,max(nvl(est,0)) bmiqty,max(nvl(bm.irate,0)) lotqty," +
                                "s.icode,s.iname,s.uom,sum(s.qtystk) as qtystk,s.fstg fstgcode, st.master_name fstg,NVL(bm.Alternate,'E') Alternate from (select i.icode, i.iname, um.master_name uom," +
                                "sum(nvl(a.qtyin, 0)) qtyin, sum(nvl(a.qtyout, 0)) qtyout, sum(nvl(a.qtyin, 0)) - sum(nvl(a.qtyout, 0)) qtystk, nvl(a.fstg, '001') fstg," +
                                "i.type, i.client_id, i.client_unit_id from item i " +
                                "left join (select icode, (case when type= '100' then 0 else qtyin end)  qtyin, qtyout qtyout," +
                                "stage fstg, type, client_id, client_unit_id,vch_num,vch_date from iprod where(type like '3%' or type = '10P' or type='100') " +
                                "union " +
                                "select acode icode, (to_number(max(qtyin))+to_number(max(qtyrw))) qtyin, 0 qtyout, stage1 fstg, type, client_id, client_unit_id, vch_num, vch_date from iprod where type = '100' " +
                                "group by acode, stage1, type, client_id, client_unit_id ,vch_num,vch_date " +
                                "union " +
                                "select acode icode, to_number(max(qtyrej)) qtyin, 0 qtyout, '101' fstg,type, client_id, client_unit_id,vch_num,vch_date from iprod where type = '100' " +
                                "group by acode, type, client_id, client_unit_id,vch_num,vch_date " +
                                "union " +
                                "select icode, sum(qtyin) qtyin, 0 qtyout, stage1 fstg, type, client_id, client_unit_id, vch_num, vch_date from iprod where type = '10R' group by " +
                                "icode, stage1, type, client_id, client_unit_id,vch_num,vch_date " +
                                "union " +
                                "select icode, 0 qtyin, sum(qtyout) qtyout, stage fstg, type, client_id,client_unit_id, vch_num, vch_date from iprod where type = '10R' " +
                                "group by icode, stage, type, client_id, client_unit_id,vch_num,vch_date " +
                                "union " +
                                "select icode, sum(qtyout) qtyin, 0 qtyout, stage1 fstg, type, client_id, client_unit_id, vch_num, vch_date from iprod where type = '10R' " +
                                "group by icode, stage1, type, client_id, client_unit_id,vch_num,vch_date) a " +
                                "on a.icode = i.icode and find_in_set(a.client_unit_id, i.client_unit_id)=1 " +
                                "inner join master_setting um on um.master_id = i.uom and um.type = 'UMM' and find_in_set(um.client_unit_id, i.client_unit_id)= 1 " +
                                "where i.type = 'IT' and find_in_set(i.client_unit_id, '" + unitid_mst + "')=1 group by i.icode,i.iname,um.master_name,nvl(a.fstg, '001'),i.type,i.client_id,i.client_unit_id) s " +
                                "left join (select client_id, client_unit_id, type, acode, pono, irate, icode, 'B' as flg, (to_number(qtyin) + to_number(qtyout)) / to_number(irate) * " + totqty1 + " est," +
                                "(case when to_number(qtyin)=0 then 'Y' else 'N' end) Alternate from " +
                                "itransactionc where type = 'BOM' and acode = '" + model[0].Col19 + "' and pono = '" + model[0].SelectedItems2.FirstOrDefault() + "' " +
                                " " + mq2 + " ) bm on s.icode = bm.icode " +
                                "and s.client_unit_id = bm.client_unit_id and s.fstg in ('001', '" + fststg + "','" + prestg + "') left join master_setting st on " +
                                "st.master_id = s.fstg and st.type = 'KPS' " +
                                "where s.client_unit_id = '" + unitid_mst + "'  and s.fstg in ('001', '" + fststg + "','" + prestg + "') group by(s.icode|| s.client_id || s.client_unit_id) ," +
                                "nvl(bm.acode, '-'),s.icode,s.iname,s.uom,s.fstg, st.master_name,bm.Alternate order by s.icode) a " +
                                "where a.fstr in ('" + URL + "') ";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                try
                                {
                                    if (model[0].dt1.Rows[0]["icode"].ToString().Trim().Equals("-")) { model[0].dt1.Rows.RemoveAt(0); }
                                    for (int i = 0; i < dtt.Rows.Count; i++)
                                    {
                                        DataRow dr = model[0].dt1.NewRow();
                                        dr["SNo"] = i + 1;
                                        dr["Icode"] = dtt.Rows[i]["icode"].ToString();
                                        dr["Iname"] = dtt.Rows[i]["iname"].ToString();
                                        dr["Uom"] = dtt.Rows[i]["uom"].ToString();
                                        dr["Qtystk"] = dtt.Rows[i]["qtystk"].ToString();
                                        dr["Stgcode"] = dtt.Rows[i]["fstgcode"].ToString();
                                        dr["Stg"] = dtt.Rows[i]["fstg"].ToString();
                                        dr["Alternate"] = dtt.Rows[i]["Alternate"].ToString();
                                        dr["Est_consume_qty"] = dtt.Rows[i]["bmiqty"].ToString();
                                        dr["Act_consume_qty"] = dtt.Rows[i]["bmiqty"].ToString();
                                        dr["short_excess"] = sgen.Make_decimal(dtt.Rows[i]["bmiqty"].ToString()) - sgen.Make_decimal(dtt.Rows[i]["bmiqty"].ToString());
                                        if (dtt.Rows[i]["Alternate"].ToString().Trim().Equals("Y"))
                                        {
                                            dr["Act_consume_qty"] = "0";
                                            dr["short_excess"] = "0";
                                        }
                                        model[0].dt1.Rows.Add(dr);
                                    }
                                    model[0].dt1.AcceptChanges();
                                    ViewBag.vgetdt = "";
                                    ViewBag.vchngdt = "disabled='disabled'";                                    
                                }
                                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1," + ex.Message.ToString() + ",0);"; }
                            }
                            break;                        
                        case "NEW":
                            mq = "select master_id,upper(master_name) master_name,to_char(convert_tz(utc_timestamp(),'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') pdo_date " +
                                "from master_Setting where (master_id||type) = '" + URL + "'";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                try
                                {
                                    sgen.SetSession(MyGuid, "EDMODE", "N");
                                    sgen.SetSession(MyGuid, "btnval", "NEW");
                                    ViewBag.vnew = "disabled='disabled'";
                                    ViewBag.vedit = "disabled='disabled'";
                                    ViewBag.vsave = "";
                                    ViewBag.vsavenew = "";
                                    ViewBag.scripCall = "enableForm();";
                                    mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + dtt.Rows[0]["master_id"].ToString() + "'" + model[0].Col11.Trim() + "";
                                    vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                                    model[0].Col16 = vch_num;
                                    model[0].Col9 = dtt.Rows[0]["master_name"].ToString();
                                    model[0].Col12 = dtt.Rows[0]["master_id"].ToString();
                                    model[0].Col17 = sgen.server_datetime_local(userCode);
                                    model[0].Col38 = sgen.server_datetime_local(userCode);
                                    model[0].Col39 = sgen.server_datetime_local(userCode);
                                    model[0].Col40 = sgen.server_datetime_local(userCode);
                                    model[0].Col22 = "1";
                                    model[0].Col101 = "C";// consumption pd                                    
                                    mod1 = new List<SelectListItem>();
                                    mod2 = new List<SelectListItem>();
                                    mod3 = new List<SelectListItem>();
                                    mod4 = new List<SelectListItem>();
                                    mod5 = new List<SelectListItem>();
                                    mod6 = new List<SelectListItem>();
                                    mod7 = new List<SelectListItem>();
                                    //all stg
                                    mod1 = cmd_Fun.prodtostage(userCode, unitid_mst);
                                    TempData[MyGuid + "_TList1"] = mod1;
                                    //op name
                                    mod3 = cmd_Fun.opname(userCode, unitid_mst);
                                    TempData[MyGuid + "_TList3"] = mod3;
                                    //machine
                                    //mod4 = cmd_Fun.mcname(userCode, clientid_mst, unitid_mst);
                                    TempData[MyGuid + "_TList4"] = mod4;
                                    //mould
                                    //mod5 = cmd_Fun.mldname(userCode, unitid_mst);
                                    TempData[MyGuid + "_TList5"] = mod5;
                                    //shift
                                    mod6 = cmd_Fun.shftname(userCode, unitid_mst);
                                    TempData[MyGuid + "_TList6"] = mod6;
                                    TempData[MyGuid + "_TList7"] = mod7;

                                    model[0].TList1 = mod1;
                                    model[0].SelectedItems1 = new string[] { "" };
                                    model[0].TList2 = mod2;
                                    model[0].SelectedItems2 = new string[] { "" };
                                    model[0].TList3 = mod3;
                                    model[0].SelectedItems3 = new string[] { "" };
                                    model[0].TList4 = mod4;
                                    model[0].SelectedItems4 = new string[] { "" };
                                    model[0].TList5 = mod5;
                                    model[0].SelectedItems5 = new string[] { "" };
                                    model[0].TList6 = mod6;
                                    model[0].SelectedItems6 = new string[] { "" };
                                    model[0].TList7 = mod7;
                                    model[0].SelectedItems7 = new string[] { "" };
                                    ViewBag.vgetdt = "";
                                    ViewBag.vchngdt = "disabled='disabled'";
                                    //if (dtt.Rows[0]["master_id"].ToString().Trim().Equals("101"))
                                    //{
                                    //    Make_query(viewName.ToLower(), "Select Production Order No", "WPDO", "2", "", "");
                                    //    ViewBag.scripCall = "callFoo('Select Production Order No');";
                                    //}
                                }
                                catch (Exception ex) { }
                            }
                            break;
                    }
                    break;
                #endregion
                #region pdrevrse
                case "pdrevrse":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select ex.vch_num,ex.type,to_char(convert_tz(ex.vch_date, 'UTC', '" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') pddate," +
                                "to_char(convert_tz(ex.date1,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') date1," +
                                "to_char(convert_tz(ex.date2,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') date2," +
                                "to_char(convert_tz(ex.date3,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') date3,ex.acode," +
                             "ex.mstage fstg,ex.stage1 tstg,ex.opcode,ex.mccode,ex.mouldcode,ex.shftno,ex.qtyin pqty,ex.qtyrej,ex.icode,it.iname,um.master_name uom,ex.qtychl estcon_qty," +
                             "ex.qtyout actcon_qty,ex.qtyplanned shortex,ex.client_id,ex.client_unit_id,ex.ent_by,ex.ent_date,ex.stime,ex.etime,ex.pflag,ex.rno,ex.qtystk,ex.cstk,ex.stage," +
                             "ex.alt,ex.qtyrw,ex.pdono,to_char(convert_tz(ex.pdodt, 'UTC', '" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') pdodt,ex.qtypdo," +
                             "ex.col1,ex.col2,ex.col3,ex.col4,ex.col5,ex.col6,ex.mctxt,ex.mouldtxt " +
                             "from iprod ex " +
                             "inner join item it on it.icode=ex.icode and it.type='IT' and find_in_set(it.client_unit_id,ex.client_unit_id)=1 " +
                             "inner join master_setting um on um.master_id=it.uom and um.type='UMM' and find_in_set(um.client_unit_id,it.client_unit_id)=1 " +
                             "where (ex.client_id||ex.client_unit_id||ex.vch_num||to_char(ex.vch_date,'yyyymmdd')||ex.type)='" + URL + "'";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "N");
                                //op name
                                mod3 = cmd_Fun.opname(userCode, unitid_mst);
                                TempData[MyGuid + "_TList3"] = mod3;
                                //machine
                                mod4 = cmd_Fun.mcname(userCode, unitid_mst);
                                TempData[MyGuid + "_TList4"] = mod4;
                                //mould
                                mod5 = cmd_Fun.mldname(userCode, unitid_mst);
                                TempData[MyGuid + "_TList5"] = mod5;
                                //shift
                                mod6 = cmd_Fun.shftname(userCode, unitid_mst);
                                TempData[MyGuid + "_TList6"] = mod6;
                                //iloc  
                                mod7 = mod4;
                                TempData[MyGuid + "_TList7"] = mod7;
                                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                                model[0].Col8 = "";
                                model[0].Col9 = tm.Col9;
                                model[0].Col10 = tm.Col10;
                                model[0].Col11 = tm.Col11;
                                model[0].Col13 = tm.Col13;
                                model[0].Col100 = tm.Col100;
                                model[0].Col121 = tm.Col121;
                                model[0].Col122 = tm.Col122;
                                model[0].Col123 = tm.Col123;
                                model[0].Col14 = tm.Col14;
                                model[0].Col15 = tm.Col15;
                                model[0].Col12 = dtt.Rows[0]["type"].ToString();
                                model[0].Col16 = dtt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dtt.Rows[0]["pddate"].ToString();
                                model[0].Col25 = dtt.Rows[0]["stime"].ToString();
                                model[0].Col26 = dtt.Rows[0]["etime"].ToString();
                                model[0].Col29 = dtt.Rows[0]["cstk"].ToString();
                                model[0].Col99 = dtt.Rows[0]["rno"].ToString();
                                model[0].Col101 = dtt.Rows[0]["pflag"].ToString();
                                model[0].Col98 = dtt.Rows[0]["tstg"].ToString();
                                model[0].Col23 = "-" + dtt.Rows[0]["pqty"].ToString();
                                model[0].Col30 = "-" + dtt.Rows[0]["qtyrej"].ToString();
                                model[0].Col31 = "-" + dtt.Rows[0]["qtyrw"].ToString();
                                model[0].Col32 = dtt.Rows[0]["pdono"].ToString();
                                model[0].Col33 = dtt.Rows[0]["pdodt"].ToString();
                                model[0].Col34 = "-" + dtt.Rows[0]["qtypdo"].ToString();
                                model[0].Col35 = dtt.Rows[0]["col1"].ToString();
                                model[0].Col36 = dtt.Rows[0]["col2"].ToString();
                                model[0].Col37 = dtt.Rows[0]["col3"].ToString();
                                model[0].Col38 = dtt.Rows[0]["date1"].ToString();
                                model[0].Col39 = dtt.Rows[0]["date2"].ToString();
                                model[0].Col40 = dtt.Rows[0]["date3"].ToString();
                                model[0].Col41 = dtt.Rows[0]["col4"].ToString();
                                model[0].Col42 = dtt.Rows[0]["col5"].ToString();
                                model[0].Col43 = dtt.Rows[0]["col6"].ToString();
                                model[0].Col50 = dtt.Rows[0]["mctxt"].ToString();
                                model[0].Col51 = dtt.Rows[0]["mouldtxt"].ToString();
                                model[0].Col45 = URL.Substring(9, URL.Length - 9);
                                model[0].TList3 = mod3;
                                model[0].TList4 = mod4;
                                model[0].TList5 = mod5;
                                model[0].TList6 = mod6;
                                model[0].TList7 = mod7;
                                model[0].SelectedItems3 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["opcode"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems4 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["mccode"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems5 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["mouldcode"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems6 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["shftno"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems7 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["mccode"].ToString()).Distinct()).Split(',');
                                mq = "select it.icode as Icode,it.iname as Iname,pum.master_name as UOM from item it " +
                               "inner join master_setting pum on pum.master_id=it.uom and pum.type='UMM' and find_in_set(pum.client_unit_id,it.client_unit_id)=1 " +
                               "where find_in_set(it.client_unit_id,'" + unitid_mst + "')=1 and it.type='IT' and it.icode='" + dtt.Rows[0]["acode"].ToString() + "'";
                                DataTable dtm = sgen.getdata(userCode, mq);
                                if (dtm.Rows.Count > 0)
                                {
                                    model[0].Col18 = dtm.Rows[0]["Icode"].ToString();
                                    model[0].Col19 = dtm.Rows[0]["Icode"].ToString();
                                    model[0].Col20 = dtm.Rows[0]["Iname"].ToString();
                                    model[0].Col21 = dtm.Rows[0]["UOM"].ToString();
                                    mq = "select bm.pono stgcode,bm.cond stg,bm.rno from itransactionc bm where bm.type = 'BOM' and bm.client_unit_id='" + unitid_mst + "' and bm.acode = '" + dtt.Rows[0]["acode"].ToString() + "' order by rno asc";
                                    dtp = sgen.getdata(userCode, mq);
                                    DataTable stgs = new DataTable();
                                    stgs.Columns.Add("stgcode");
                                    string lastscode = "", newscode = "";
                                    if (dtp.Rows.Count > 0)
                                    {
                                        for (int i = 0; i < dtp.Rows.Count; i++)
                                        {
                                            newscode = dtp.Rows[i]["stgcode"].ToString().Trim();
                                            if (newscode != lastscode)
                                            {
                                                DataRow dr = stgs.NewRow();
                                                dr[0] = newscode;
                                                stgs.Rows.Add(dr);
                                                lastscode = newscode;
                                            }
                                        }
                                        stgs.AcceptChanges();
                                        DataTable dts = sgen.getdata(userCode, "SELECT master_id stgcode,master_name stg FROM master_setting where type = 'KPS'");
                                        var results = from table1 in stgs.AsEnumerable()
                                                      join table2 in dts.AsEnumerable() on table1["stgcode"] equals table2["stgcode"]
                                                      select new
                                                      {
                                                          stgcode = table1["stgcode"],
                                                          stg = table2["stg"]
                                                      };
                                        results.ToList();
                                        DataTable dtkk = sgen.ToDataTable(results.ToList());
                                        if (dtkk.Rows.Count > 0)
                                        {
                                            foreach (DataRow dr in dtkk.Rows)
                                            {
                                                mod2.Add(new SelectListItem { Text = dr["stg"].ToString(), Value = dr["stgcode"].ToString() });
                                            }
                                        }
                                        TempData[MyGuid + "_TList2"] = mod2;
                                        model[0].TList2 = mod2;
                                    }
                                }
                                //model[0].SelectedItems1 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["fstg"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems2 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["fstg"].ToString()).Distinct()).Split(',');
                                model[0].dt1 = new DataTable();
                                model[0].dt1 = ((DataTable)sgen.GetSession(MyGuid, "dtipd")).Clone();
                                for (int i = 0; i < dtt.Rows.Count; i++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["sno"] = i + 1;
                                    dr["Icode"] = dtt.Rows[i]["icode"].ToString();
                                    dr["Iname"] = dtt.Rows[i]["iname"].ToString();
                                    dr["Uom"] = dtt.Rows[i]["uom"].ToString();//puom
                                    dr["Qtystk"] = dtt.Rows[i]["qtystk"].ToString();//puom
                                    dr["Stgcode"] = dtt.Rows[i]["stage"].ToString();//puom
                                    dr["Stg"] = dtt.Rows[i]["stage"].ToString();//puom
                                    dr["Est_consume_qty"] = "-" + dtt.Rows[i]["estcon_qty"].ToString();//estimated consume qty
                                    dr["Act_consume_qty"] = "-" + dtt.Rows[i]["actcon_qty"].ToString();//actual consume qty
                                    dr["Short_Excess"] = dtt.Rows[i]["shortex"].ToString();//shortexcess
                                    dr["Alternate"] = dtt.Rows[i]["alt"].ToString();//shortexcess
                                    model[0].dt1.Rows.Add(dr);
                                }
                                model[0].dt1.AcceptChanges();
                            }
                            break;
                    }
                    break;
                #endregion
                #region irej
                case "irej":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select ex.client_id,ex.client_unit_id,ex.ent_by,ex.ent_date,ex.vch_num DocNo," +
                               "to_char(convert_tz(ex.vch_date, 'UTC', '" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') Doc_Date,ex.acode Acode," +
                               "it.iname Aname,um.master_name Uom,ex.mstage,(case when ex.mstage='100' then 'PDI' else stf.master_name end) Rej_Stage,ex.qtychl RedbinQty," +
                               "ex.qtychl Analysis_Qty,ex.qtyout ApprovedQty,ex.qtyrej RejQty,ex.stage1," +
                               "(case when ex.stage1='100' then 'PDI' when ex.stage1='102' then 'Rework' else stt.master_name end) Approved_Stage," +
                               "ex.iremark remark from iprod ex " +
                               "inner join item it on it.icode=ex.acode and it.type='IT' and find_in_set(it.client_unit_id,ex.client_unit_id)=1 " +
                               "inner join master_setting um on um.master_id=it.uom and um.type='UMM' and find_in_set(um.client_unit_id,it.client_unit_id)=1 " +
                               "left join master_setting stf on stf.master_id=ex.mstage and stf.type='KPS' " +
                               "left join master_setting stt on stt.master_id=ex.stage1 and stt.type='KPS' " +
                               "where (ex.client_id||ex.client_unit_id||ex.vch_num||to_char(ex.vch_date,'yyyymmdd')||ex.type) in ('" + URL + "')";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                                model[0].Col8 = "(client_id||client_unit_id||vch_num||to_char(vch_date,'yyyymmdd')||type) = '" + URL + "'";
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
                                model[0].Col16 = dtt.Rows[0]["DocNo"].ToString();
                                model[0].Col17 = dtt.Rows[0]["Doc_Date"].ToString();
                                model[0].dt1 = new DataTable();
                                model[0].dt1 = ((DataTable)sgen.GetSession(MyGuid, "dtirej")).Clone();
                                for (int i = 0; i < dtt.Rows.Count; i++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["sno"] = i + 1;
                                    dr["acode"] = dtt.Rows[i]["Acode"].ToString();
                                    dr["aname"] = dtt.Rows[i]["Aname"].ToString();
                                    dr["Uom"] = dtt.Rows[i]["Uom"].ToString();//puom
                                    dr["Rej_Stgcode"] = dtt.Rows[i]["mstage"].ToString();//frm stg code
                                    dr["Rej_Stg"] = dtt.Rows[i]["Rej_Stage"].ToString();//frm stg
                                    dr["Redbin_Qty"] = dtt.Rows[i]["RedbinQty"].ToString();//shortexcess
                                    dr["Analysis_Qty"] = dtt.Rows[i]["Analysis_Qty"].ToString();//shortexcess
                                    dr["Approved_Qty"] = dtt.Rows[i]["ApprovedQty"].ToString();//shortexcess
                                    dr["Rej_Qty"] = dtt.Rows[i]["RejQty"].ToString();//shortexcess
                                    dr["Approved_Stgcode"] = dtt.Rows[i]["stage1"].ToString();//shortexcess
                                    dr["Approved_Stg"] = dtt.Rows[i]["Approved_Stage"].ToString();//shortexcess
                                    dr["Remark"] = dtt.Rows[i]["remark"].ToString();//iremark
                                    model[0].dt1.Rows.Add(dr);
                                }
                                model[0].dt1.AcceptChanges();
                            }
                            break;
                        case "ITEM": //main item                           
                            //  mq = "select p.vch_num,to_char(convert_tz(p.vch_date,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') vch_date,p.acode,i.iname," +
                            //      "u.master_name Uom,p.type,p.qtyin ProduceQty,nvl(p.qtyrej,0) RedbinQty,p.stage sfcode,sf.master_name FromStg,p.stage1 stcode,st.master_name ToStg from iprod p " +
                            //     "inner join item i on i.icode=p.acode and i.type='IT' and i.client_id=p.client_id and i.client_unit_id=p.client_unit_id " +
                            //     "inner join master_setting u on i.uom=u.master_id and u.type='UMM' and find_in_set(u.client_id,p.client_id)=1 and find_in_set(u.client_unit_id,p.client_unit_id)=1 " +
                            //     "inner join master_setting sf on sf.master_id=p.stage and sf.type='KPS' and find_in_set(sf.client_id,p.client_id)=1 and find_in_set(sf.client_unit_id,p.client_unit_id)=1 " +
                            //     "inner join master_setting st on st.master_id=p.stage1 and st.type='KPS' and find_in_set(st.client_id,p.client_id)=1 and find_in_set(st.client_unit_id,p.client_unit_id)=1 " +
                            //"where (p.client_id||p.client_unit_id||p.vch_num||to_char(p.vch_date,'yyyymmdd')||p.type)='" + URL + "'";
                            mq = "select a.icode,i.iname,u.master_name uom,sum(a.qtyin) qtyin,sum(a.qtyout) qtyout,sum(a.qtyin)-sum(a.qtyout) qtystk,a.fstg fstgcode," +
                              "(case when a.fstg = '101' then 'Rejection' when a.fstg='100' then 'PDI' else s.master_name end) fstg,a.client_id,a.client_unit_id from (select acode icode, " +
                              "to_number(max(qtyrej)) qtyin, 0 qtyout, mstage fstg, type, client_id, client_unit_id, vch_num, vch_date from iprod where type = '100' " +
                              "group by acode, type, client_id, client_unit_id,vch_num,vch_date,mstage " +
                              "union " +
                              "select icode, sum(qtyin) qtyin, sum(qtychl) qtyout, mstage fstg, type,client_id, client_unit_id, vch_num, vch_date from iprod where " +
                              "type = '10R' group by icode, mstage, type, client_id, client_unit_id,vch_num,vch_date " +
                              "union " +
                              "select icode, 0 qtyin, sum(qtyout) qtyout, stage fstg, type, client_id, client_unit_id, vch_num, vch_date from iprod where type = '10R' " +
                              "group by icode, stage, type, client_id, client_unit_id,vch_num,vch_date) a " +
                              "inner join item i on i.icode = a.icode and i.type = 'IT' and find_in_set(i.client_unit_id, a.client_unit_id)=1 " +
                              "inner join master_setting u on u.master_id = i.uom and u.type = 'UMM' and find_in_set(u.client_unit_id, a.client_unit_id)= 1 " +
                              "left join master_setting s on s.master_id = a.fstg and s.type = 'KPS' " +
                              "where (a.client_id||a.client_unit_id||a.icode||a.fstg)='" + URL + "' " +
                              "group by a.icode,i.iname,u.master_name,a.fstg,s.master_name,a.client_id,a.client_unit_id having sum(qtyin) - sum(qtyout) > 0";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                try
                                {
                                    if (model[0].dt1.Rows[0]["acode"].ToString().Trim().Equals("-")) { model[0].dt1.Rows.RemoveAt(0); }
                                    for (int i = 0; i < dtt.Rows.Count; i++)
                                    {
                                        DataRow dr = model[0].dt1.NewRow();
                                        dr["SNo"] = i + 1;
                                        dr["acode"] = dtt.Rows[i]["icode"].ToString();
                                        dr["aname"] = dtt.Rows[i]["iname"].ToString();
                                        dr["Uom"] = dtt.Rows[i]["uom"].ToString();
                                        dr["Rej_StgCode"] = dtt.Rows[i]["fstgcode"].ToString();
                                        dr["Rej_Stg"] = dtt.Rows[i]["fstg"].ToString();
                                        dr["Redbin_Qty"] = dtt.Rows[i]["qtystk"].ToString();
                                        dr["Analysis_Qty"] = "0";
                                        dr["Approved_Qty"] = "0";
                                        dr["Rej_Qty"] = "0";
                                        model[0].dt1.Rows.Add(dr);
                                    }
                                    model[0].dt1.AcceptChanges();
                                }
                                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1," + ex.Message.ToString() + ",0);"; }
                            }
                            break;
                    }
                    break;
                #endregion
                #region rewrk
                case "rewrk":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select ex.client_id,ex.client_unit_id,ex.ent_by,ex.ent_date,ex.vch_num DocNo," +
                               "to_char(convert_tz(ex.vch_date, 'UTC', '" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') Doc_Date,ex.acode Acode," +
                               "it.iname Aname,um.master_name Uom,ex.mstage,(case when ex.mstage='102' then 'Rework' else stf.master_name end) Rew_Stage,ex.qtychl ReworkQty," +
                               "ex.qtychl Analysis_Qty,ex.qtyout ApprovedQty,ex.qtyrej RejQty,ex.stage1,(case when ex.stage1='100' then 'PDI' else stt.master_name end) Approved_Stage," +
                               "ex.iremark remark from iprod ex " +
                               "inner join item it on it.icode=ex.acode and it.type='IT' and find_in_set(it.client_unit_id,ex.client_unit_id)=1 " +
                               "inner join master_setting um on um.master_id=it.uom and um.type='UMM' and find_in_set(um.client_unit_id,it.client_unit_id)=1 " +
                               "left join master_setting stf on stf.master_id=ex.mstage and stf.type='KPS' " +
                               "left join master_setting stt on stt.master_id=ex.stage1 and stt.type='KPS' " +
                               "where (ex.client_id||ex.client_unit_id||ex.vch_num||to_char(ex.vch_date,'yyyymmdd')||ex.type) in ('" + URL + "')";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                                model[0].Col8 = "(client_id||client_unit_id||vch_num||to_char(vch_date,'yyyymmdd')||type) = '" + URL + "'";
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
                                model[0].Col16 = dtt.Rows[0]["DocNo"].ToString();
                                model[0].Col17 = dtt.Rows[0]["Doc_Date"].ToString();
                                model[0].dt1 = new DataTable();
                                model[0].dt1 = ((DataTable)sgen.GetSession(MyGuid, "dtire")).Clone();
                                for (int i = 0; i < dtt.Rows.Count; i++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["sno"] = i + 1;
                                    dr["acode"] = dtt.Rows[i]["Acode"].ToString();
                                    dr["aname"] = dtt.Rows[i]["Aname"].ToString();
                                    dr["Uom"] = dtt.Rows[i]["Uom"].ToString();//puom
                                    dr["Rew_Stgcode"] = dtt.Rows[i]["mstage"].ToString();//frm stg code
                                    dr["Rew_Stg"] = dtt.Rows[i]["Rew_Stage"].ToString();//frm stg
                                    dr["Rework_Qty"] = dtt.Rows[i]["ReworkQty"].ToString();//shortexcess
                                    dr["Analysis_Qty"] = dtt.Rows[i]["Analysis_Qty"].ToString();//shortexcess
                                    dr["Approved_Qty"] = dtt.Rows[i]["ApprovedQty"].ToString();//shortexcess
                                    dr["Rej_Qty"] = dtt.Rows[i]["RejQty"].ToString();//shortexcess
                                    dr["Approved_Stgcode"] = dtt.Rows[i]["stage1"].ToString();//shortexcess
                                    dr["Approved_Stg"] = dtt.Rows[i]["Approved_Stage"].ToString();//shortexcess
                                    dr["Remark"] = dtt.Rows[i]["remark"].ToString();//iremark
                                    model[0].dt1.Rows.Add(dr);
                                }
                                model[0].dt1.AcceptChanges();
                            }
                            break;
                        case "ITEM": //main item                                          
                            mq = "select (a.client_id || a.client_unit_id || a.icode || a.fstg) fstr,a.icode,i.iname,u.master_name uom, sum(a.qtyin)-sum(a.qtyout) qtystk,a.fstg fstgcode," +
                              "(case when a.fstg = '102' then 'Rework' else s.master_name end) fstg,a.client_id,a.client_unit_id from " +
                              "(select icode, sum(qtyin) qtyin, 0 qtyout, stage1 fstg, type, client_id, client_unit_id, vch_num, vch_date from iprod where type = '10R' " +
                              "group by icode, stage1, type, client_id, client_unit_id,vch_num,vch_date " +
                              "union " +
                              "select icode, 0 qtyin, sum(qtychl) qtyout, stage fstg, type, client_id, client_unit_id, vch_num, vch_date from iprod where type = '10R' " +
                              "group by icode, stage,type, client_id, client_unit_id,vch_num,vch_date " +
                              "union " +
                              "select icode, sum(qtyout) qtyin, 0 qtyout, stage1 fstg, type, client_id,client_unit_id, vch_num, vch_date from iprod where type = '10R' " +
                              "group by icode, stage1, type, client_id, client_unit_id,vch_num,vch_date) a " +
                              "inner join item i on i.icode = a.icode and i.type = 'IT' and find_in_set(i.client_unit_id, a.client_unit_id)=1 " +
                              "inner join master_setting u on u.master_id = i.uom and u.type = 'UMM' and find_in_set(u.client_unit_id, a.client_unit_id)= 1 " +
                              "left join master_setting s on s.master_id = a.fstg and s.type = 'KPS' " +
                              "where (a.client_id||a.client_unit_id||a.icode||a.fstg)='" + URL + "' " +
                              "group by a.icode,i.iname,u.master_name,a.fstg,s.master_name,a.client_id,a.client_unit_id having sum(qtyin) - sum(qtyout) > 0";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                try
                                {
                                    if (model[0].dt1.Rows[0]["acode"].ToString().Trim().Equals("-")) { model[0].dt1.Rows.RemoveAt(0); }
                                    for (int i = 0; i < dtt.Rows.Count; i++)
                                    {
                                        DataRow dr = model[0].dt1.NewRow();
                                        dr["SNo"] = i + 1;
                                        dr["acode"] = dtt.Rows[i]["icode"].ToString();
                                        dr["aname"] = dtt.Rows[i]["iname"].ToString();
                                        dr["Uom"] = dtt.Rows[i]["uom"].ToString();
                                        dr["Rew_StgCode"] = dtt.Rows[i]["fstgcode"].ToString();
                                        dr["Rew_Stg"] = dtt.Rows[i]["fstg"].ToString();
                                        dr["Rework_Qty"] = dtt.Rows[i]["qtystk"].ToString();
                                        dr["Analysis_Qty"] = "0";
                                        dr["Approved_Qty"] = "0";
                                        dr["Rej_Qty"] = "0";
                                        model[0].dt1.Rows.Add(dr);
                                    }
                                    model[0].dt1.AcceptChanges();
                                }
                                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1," + ex.Message.ToString() + ",0);"; }
                            }
                            break;
                    }
                    break;
                #endregion
                #region pdreps
                case "pdreps":
                    try
                    {
                        fdt = sgen.GetSession(MyGuid, "KPDFDT").ToString();
                        tdt = sgen.GetSession(MyGuid, "KPDTDT").ToString();
                    }
                    catch (Exception er) { }
                    switch (btnval.ToUpper())
                    {
                        case "BOM":
                            mq = @"select (b.client_id||b.client_unit_id||b.vch_num||to_char(b.vch_date,'yyyymmdd')||b.type) fstr,b.vch_num,to_char(convert_tz(b.vch_date, 'UTC', '" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') vch_date," +
                                "b.acode micode,m.iname miname,um.master_name muom,b.irate lotsize,b.icode,i.iname,ui.master_name uom,b.qtyin reqqty," +
                                "b.qtyout altqty,b.pono stgcode,s.master_name stg from itransactionc b " +
                                "inner join item m on m.icode=b.acode and m.type='IT' and find_in_set(m.client_unit_id,b.client_unit_id)=1 " +
                                "inner join master_setting um on um.master_id=m.uom and um.type='UMM' and find_in_set(um.client_unit_id,b.client_unit_id)=1 " +
                                "inner join item i on i.icode=b.icode and i.type='IT' and find_in_set(i.client_unit_id,b.client_unit_id)=1 " +
                                "inner join master_setting ui on ui.master_id=i.uom and ui.type='UMM' and find_in_set(ui.client_unit_id,b.client_unit_id)=1 " +
                                "inner join master_setting s on s.master_id=b.pono and s.type='KPS' " +
                                "where (b.client_id||b.client_unit_id||b.vch_num||to_char(b.vch_date,'yyyymmdd')||b.type) in ('" + URL + "')";
                            DataSet ds = new DataSet();
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                dtt.TableName = "prepcur";
                                ds.Tables.Add(dtt);
                                sgen.open_report_byDs_xml(userCode, ds, "bom_report", "BOM Detail");
                                ViewBag.scripCall += "showRptnew('BOM Detail');disableForm();";

                                //ViewBag.scripCall = "OpenSingle('../../../../../erp/schoolReports_Rpts/Filter_Report_xml.aspx','95%','900px','Reports');";
                            }
                            break;
                        case "ISUM":
                            Make_query(viewName, "Detailed Item Transactions", "IDET", "0", URL, fdt + "!~!~!~!~!" + tdt,MyGuid);
                            ViewBag.scripCall = "callFoo('Detailed Item Transactions');";
                            break;
                    }
                    break;
                #endregion
                #region pdreps2
                case "pdreps2":
                    switch (btnval.ToUpper())
                    {
                        case "BOM":
                            mq = @"select (b.client_id||b.client_unit_id||b.vch_num||to_char(b.vch_date,'yyyymmdd')||b.type) fstr,b.vch_num,to_char(convert_tz(b.vch_date, 'UTC', '" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') vch_date," +
                                "b.acode micode,m.iname miname,um.master_name muom,b.irate lotsize,b.icode,i.iname,ui.master_name uom,b.qtyin reqqty," +
                                "b.qtyout altqty,b.pono stgcode,s.master_name stg from itransactionc b " +
                                "inner join item m on m.icode=b.acode and m.type='IT' and find_in_set(m.client_unit_id,b.client_unit_id)=1 " +
                                "inner join master_setting um on um.master_id=m.uom and um.type='UMM' and find_in_set(um.client_unit_id,b.client_unit_id)=1 " +
                                "inner join item i on i.icode=b.icode and i.type='IT' and find_in_set(i.client_unit_id,b.client_unit_id)=1 " +
                                "inner join master_setting ui on ui.master_id=i.uom and ui.type='UMM' and find_in_set(ui.client_unit_id,b.client_unit_id)=1 " +
                                "inner join master_setting s on s.master_id=b.pono and s.type='KPS' " +
                                "where (b.client_id||b.client_unit_id||b.vch_num||to_char(b.vch_date,'yyyymmdd')||b.type) in ('" + URL + "')";
                            DataSet ds = new DataSet();
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                dtt.TableName = "prepcur";
                                ds.Tables.Add(dtt);
                                sgen.open_report_byDs_xml(userCode, ds, "bom_report", "BOM Detail");
                                ViewBag.scripCall += "showRptnew('BOM Detail');disableForm();";

                                //ViewBag.scripCall = "OpenSingle('../../../../../erp/schoolReports_Rpts/Filter_Report_xml.aspx','95%','900px','Reports');";
                            }
                            break;
                    }
                    break;
                #endregion
                #region stgtransfer
                case "stgtransfer":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select ex.vch_num,to_char(convert_tz(ex.vch_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') vch_date," +
                                 "ex.rec_id,ex.client_id,ex.client_unit_id,ex.ent_by,ex.ent_date,ex.icode,i.iname finame,ex.stage fstgcode,ex.qtystk," +
                                 "ex.qtyin,ex.acode,it.tiname,ex.stage1 tstgcode from iprod ex " +
                                 "inner join item i on ex.icode = i.icode and i.type = 'IT' and find_in_set(i.client_unit_id, ex.client_unit_id)=1 " +
                                 "inner join item it on ex.acode = it.icode and it.type = 'IT' and find_in_set(it.client_unit_id, ex.client_unit_id)=1 " +
                                 "where (ex.client_unit_id||ex.vch_num||to_char(ex.vch_date,'yyyymmdd')||ex.type)='" + URL + "'";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                //fromstg
                                mod1 = cmd_Fun.prodstage(userCode, unitid_mst);
                                TempData[MyGuid + "_TList1"] = mod1;
                                //tostg
                                mod2 = mod1;
                                TempData[MyGuid + "_TList2"] = mod2;
                                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                                model[0].Col8 = "(client_unit_id||vch_num||to_char(vch_date,'yyyymmdd')||type) = '" + URL + "'";
                                model[0].Col9 = tm.Col9;
                                model[0].Col10 = tm.Col10;
                                model[0].Col11 = tm.Col11;
                                model[0].Col12 = dtt.Rows[0]["type"].ToString();
                                model[0].Col13 = "Update";
                                model[0].Col100 = "Update & New";
                                model[0].Col121 = "U";
                                model[0].Col122 = "<u>U</u>pdate";
                                model[0].Col123 = "Update & Ne<u>w</u>";
                                model[0].Col14 = tm.Col14;
                                model[0].Col15 = tm.Col15;
                                model[0].Col16 = dtt.Rows[0]["vch_num"].ToString();
                                model[0].Col17 = dtt.Rows[0]["icode"].ToString();
                                model[0].Col19 = dtt.Rows[0]["finame"].ToString();
                                model[0].Col22 = dtt.Rows[0]["acode"].ToString();
                                model[0].Col24 = dtt.Rows[0]["tiname"].ToString();
                                model[0].Col20 = dtt.Rows[0]["qtystk"].ToString();
                                model[0].Col21 = dtt.Rows[0]["qtyin"].ToString();
                                model[0].TList1 = mod1;
                                model[0].TList2 = mod2;
                                model[0].SelectedItems1 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["fstgcode"].ToString()).Distinct()).Split(',');
                                model[0].SelectedItems2 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["tstgcode"].ToString()).Distinct()).Split(',');
                            }
                            break;
                        case "FITEM": //main item from
                            mq = "select distinct p.acode,i.iname from iprod p " +
                                "inner join item i on i.icode = p.acode and i.type = 'IT' and find_in_set(i.client_unit_id, p.client_unit_id)=1 " +
                                "where (p.client_unit_id || p.acode || p.type) = '" + URL + "'";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                model[0].Col17 = dtt.Rows[0]["acode"].ToString();
                                model[0].Col19 = dtt.Rows[0]["iname"].ToString();
                            }
                            break;
                        case "TITEM": //main item to
                            mq = "select distinct p.acode,i.iname from iprod p " +
                               "inner join item i on i.icode = p.acode and i.type = 'IT' and find_in_set(i.client_unit_id, p.client_unit_id)=1 " +
                               "where (p.client_unit_id || p.acode || p.type) = '" + URL + "'";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                model[0].Col22 = dtt.Rows[0]["acode"].ToString();
                                model[0].Col24 = dtt.Rows[0]["iname"].ToString();
                            }
                            break;
                    }
                    break;
                #endregion
                #region mlditm
                case "mlditm":
                    switch (btnval.ToUpper())
                    {
                        case "EDIT":
                            mq = "select ex.vch_num,to_char(convert_tz(ex.vch_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') vch_date," +
                                 "ex.client_id,ex.client_unit_id,ex.ent_by,ex.ent_date,ex.col2 icode,i.iname,ex.col3 mld from enx_tab ex " +
                                 "inner join item i on ex.col2 = i.icode and i.type = 'IT' and find_in_set(i.client_unit_id, ex.client_unit_id)=1 " +
                                 "where (ex.client_unit_id||ex.vch_num||to_char(ex.vch_date,'yyyymmdd')||ex.type)='" + URL + "'";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                                model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                                model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                                model[0].Col8 = "(client_unit_id||vch_num||to_char(vch_date,'yyyymmdd')||type) = '" + URL + "'";
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
                                model[0].Col16 = dtt.Rows[0]["vch_num"].ToString();

                                mq = "select group_concat(distinct e.col3) mldid,group_concat(distinct m.col31) mldname from enx_tab e inner join kc_tab m on find_in_set(m.vch_num,e.col3)=1 and m.type='MOS' " +
                                    "and find_in_set(m.client_unit_id,e.client_unit_id)=1 where (e.client_unit_id||e.vch_num||to_char(e.vch_date,'yyyymmdd')||e.type)='" + URL + "'";
                                dtp = sgen.getdata(userCode, mq);
                                if (dtp.Rows.Count > 0)
                                {
                                    model[0].Col17 = dtp.Rows[0]["mldname"].ToString();
                                    model[0].Col18 = dtp.Rows[0]["mldid"].ToString();
                                }

                                mq = "select '-' as SNo,'-' as ICode,'-' as IName from dual";
                                model[0].dt1 = sgen.getdata(userCode, mq);
                                if (model[0].dt1.Rows[0]["icode"].ToString().Trim().Equals("-")) { model[0].dt1.Rows.RemoveAt(0); }

                                var dtm = dtt.Copy();
                                dtm.Columns.Remove("mld");
                                DataView dv = dtm.DefaultView;
                                dtm = dv.ToTable(true, "icode", "iname", "vch_num", "vch_date", "client_id", "client_unit_id", "ent_by", "ent_date");
                                for (int i = 0; i < dtm.Rows.Count; i++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["SNo"] = i.ToString();
                                    dr["IName"] = dtm.Rows[i]["iname"].ToString();
                                    dr["Icode"] = dtm.Rows[i]["icode"].ToString();
                                    model[0].dt1.Rows.Add(dr);
                                }
                            }
                            break;
                        case "MLD":
                            mq = "select k.col31 MldName,k.vch_num from kc_tab k " +
                           "where (k.client_unit_id || k.vch_num || to_char(k.vch_date, 'yyyymmdd') || k.type) in ('" + URL + "')";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                for (int k = 0; k < dtt.Rows.Count; k++)
                                {
                                    model[0].Col17 = model[0].Col17 + dtt.Rows[k]["MldName"].ToString() + ",";
                                    model[0].Col18 = model[0].Col18 + dtt.Rows[k]["vch_num"].ToString() + ",";
                                }
                                model[0].Col17 = model[0].Col17.TrimEnd(',');
                                model[0].Col18 = model[0].Col18.TrimEnd(',');
                            }
                            break;
                        case "ITEM":
                            mq = "select (it.icode||to_char(it.vch_date,'yyyymmdd')||it.type) fstr,it.icode Icode,it.iname Iname,it.cpartno,um.master_name uom " +
                                "from item it " +
                                "inner join master_setting um on um.master_id = it.uom and um.type = 'UMM' and find_in_set(um.client_unit_id, it.client_unit_id)= 1 " +
                                "where (it.icode||to_char(it.vch_date,'yyyymmdd')||it.type) in ('" + URL + "')";
                            dtt = sgen.getdata(userCode, mq);
                            if (dtt.Rows.Count > 0)
                            {
                                if (model[0].dt1.Rows[0]["icode"].ToString().Trim().Equals("-")) { model[0].dt1.Rows.RemoveAt(0); }
                                for (int i = 0; i < dtt.Rows.Count; i++)
                                {
                                    DataRow dr = model[0].dt1.NewRow();
                                    dr["SNo"] = dtt.Rows[i]["fstr"].ToString();
                                    dr["Icode"] = dtt.Rows[i]["icode"].ToString();
                                    dr["Iname"] = dtt.Rows[i]["iname"].ToString();
                                    model[0].dt1.Rows.Add(dr);
                                }
                            }
                            break;
                    }
                    break;
                #endregion
                #region ageing_filter
                case "ageing_filter":
                    switch (btnval.ToUpper())
                    {
                        case "DETAIL":
                            mq = "SELECT edit_by,rec_id ,ent_by,col1,col2,col3,col4,col5,col6,col7,col8,col9,col10,col11,col12,col13,client_id,client_unit_id,ent_by,ent_date,vch_num,vch_date " +
                            "from enx_tab where (client_unit_id||vch_num||to_char(vch_date, 'yyyymmdd')|| type)='" + URL + "'";
                            dtt = sgen.getdata(userCode, mq);
                            //mod1.Add(new SelectListItem { Text = "Ageing By Bill Date", Value = "ABD" });
                            //mod1.Add(new SelectListItem { Text = "Ageing By Due Date", Value = "ADD" });
                            //model[0].TList1 = mod1;
                            //TempData[MyGuid + "_TList1"] = mod1;
                            if (dtt.Rows.Count > 0)
                            {
                                sgen.SetSession(MyGuid, "EDMODE", "Y");
                                model[0].Col8 = "client_unit_id||vch_num|| to_char(vch_date, 'yyyymmdd')|| type = '" + URL + "'";
                                //String[] L1 = System.String.Join(",", dtt.Rows.OfType<DataRow>().Select(r => r["col13"].ToString()).Distinct()).Split(',');
                                //model[0].SelectedItems1 = L1;
                                model[0].Col16 = dtt.Rows[0]["vch_num"].ToString();
                                model[0].Col29 = dtt.Rows[0]["col13"].ToString();
                                model[0].Col17 = dtt.Rows[0]["col1"].ToString();
                                model[0].Col18 = dtt.Rows[0]["col2"].ToString();
                                model[0].Col19 = dtt.Rows[0]["col3"].ToString();
                                model[0].Col20 = dtt.Rows[0]["col4"].ToString();
                                model[0].Col21 = dtt.Rows[0]["col5"].ToString();
                                model[0].Col22 = dtt.Rows[0]["col6"].ToString();
                                model[0].Col23 = dtt.Rows[0]["col7"].ToString();
                                model[0].Col24 = dtt.Rows[0]["col8"].ToString();
                                model[0].Col25 = dtt.Rows[0]["col9"].ToString();
                                model[0].Col26 = dtt.Rows[0]["col10"].ToString();
                                model[0].Col27 = dtt.Rows[0]["col11"].ToString();
                                model[0].Col28 = dtt.Rows[0]["col12"].ToString();
                                model[0].Col3 = dtt.Rows[0]["ent_by"].ToString();
                                model[0].Col7 = dtt.Rows[0]["rec_id"].ToString();
                                model[0].Col5 = dtt.Rows[0]["edit_by"].ToString();
                                model[0].Col4 = dtt.Rows[0]["ent_date"].ToString();
                                model[0].Col1 = dtt.Rows[0]["client_id"].ToString();
                                model[0].Col2 = dtt.Rows[0]["client_unit_id"].ToString();
                            }
                            break;
                    }
                    break;
                    #endregion
            }
            return model;
        }
        #endregion
        //-======machine master================
        #region
        public ActionResult machine_master()
        {
            FillMst();
            sgen.SetSession(MyGuid, "sch_upd1", null);
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "MACHINE MASTER";
            model[0].Col10 = "kc_tab";
            model[0].Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            model[0].Col12 = "PMM"; // Plant machine master                                              
            model[0].Col28 = "Choose File";
            model[0].Col30 = "file_tab";
            sgen.SetSession(MyGuid, "M_MID", mid_mst.Trim());
            sgen.SetSession(MyGuid, "M_MST", model[0].Col12.Trim());
            sgen.SetSession(MyGuid, "M_COND_MST", model[0].Col11.Trim());
            sgen.SetSession(MyGuid, "M_TBL_MST", model[0].Col10.Trim());
            model[0].LTM1 = new List<Tmodelmain>();
            List<SelectListItem> md1 = new List<SelectListItem>();
            List<SelectListItem> md2 = new List<SelectListItem>();
            List<SelectListItem> md3 = new List<SelectListItem>();
            model[0].TList1 = md1;
            model[0].TList2 = md2;
            model[0].TList3 = md3;
            //TempData[MyGuid + "_TList1"] = model[0].TList1;
            //TempData[MyGuid + "_TList2"] = model[0].TList2;
            //TempData[MyGuid + "_TList3"] = model[0].TList3;
            return View(model);
        }
        public List<Tmodelmain> newmachine_master(List<Tmodelmain> model)
        {
            model = getnew(model);
            model[0].Col29 = "Y";
            //string defval = "";
            #region Frequency
            List<SelectListItem> mod1 = new List<SelectListItem>();
            mod1 = cmd_Fun.freq(userCode, unitid_mst);
            model[0].TList1 = mod1;
            TempData[MyGuid + "_TList1"] = model[0].TList1;
            model[0].SelectedItems1 = new string[] { "" };
            #endregion
            #region single Location
            List<SelectListItem> mod2 = new List<SelectListItem>();
            defval = "";
            mod2 = cmd_Fun.iloc(userCode, unitid_mst, out defval);
            model[0].TList2 = mod2;
            TempData[MyGuid + "_TList2"] = model[0].TList2;
            model[0].SelectedItems2 = new string[] { defval };
            #endregion
            #region machine capacity
            List<SelectListItem> mod3 = new List<SelectListItem>();
            mod3 = cmd_Fun.mcap(userCode, unitid_mst);
            model[0].TList3 = mod3;
            TempData[MyGuid + "_TList3"] = model[0].TList3;
            model[0].SelectedItems3 = new string[] { "" };
            #endregion
            return model;
        }
        [HttpPost]
        public ActionResult machine_master(List<Tmodelmain> model, string command, HttpPostedFileBase[] upd1, HttpPostedFileBase upd2)
        {
            try
            {
                FillMst(model[0].Col15);
                var tmodel = model.FirstOrDefault();
                type = model[0].Col12;
                tab_name = model[0].Col10;
                where = model[0].Col11;
                #region ddl
                List<SelectListItem> mod1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
                model[0].TList1 = mod1;
                TempData[MyGuid + "_TList1"] = model[0].TList1;
                if (model[0].SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
                List<SelectListItem> mod2 = (List<SelectListItem>)TempData[MyGuid + "_TList2"];
                model[0].TList2 = mod2;
                TempData[MyGuid + "_TList2"] = model[0].TList2;
                if (model[0].SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
                List<SelectListItem> mod3 = (List<SelectListItem>)TempData[MyGuid + "_TList3"];
                model[0].TList3 = mod3;
                TempData[MyGuid + "_TList3"] = model[0].TList3;
                if (model[0].SelectedItems3 == null) model[0].SelectedItems3 = new string[] { "" };
                #endregion
                IList<HttpPostedFileBase> fileCollection1 = new List<HttpPostedFileBase>();
                IList<HttpPostedFileBase> fileCollection2 = new List<HttpPostedFileBase>();
                if (command == "New")
                {
                    try
                    {
                        model = newmachine_master(model);
                    }
                    catch { }
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
                        Satransaction sat1 = new Satransaction(userCode, MyGuid);
                        //var tmodel = model.FirstOrDefault();
                        string currdate = sgen.server_datetime_local(userCode);
                        currdate = sgen.Savedate(currdate, true);
                        status = tmodel.Col29.Trim();
                        if (status == "N") { model[0].Col12 = "DDPMM"; }
                        DataTable dtstr = new DataTable();
                        dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                        if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                        {
                            mq1 = " and vch_num<>'" + tmodel.Col16.Trim() + "'";
                            //mq = sgen.seekval(userCode, "select col31 master_name from " + model[0].Col10 + " where type='" + model[0].Col12 + "' " + model[0].Col11 + " " +
                            //    "and upper(col31)='" + model[0].Col17.ToUpper() + "' " + mq1 + " ", "master_name");
                            //if (!mq.Trim().Equals("0"))
                            //{
                            //    ViewBag.vnew = "disabled='disabled'";
                            //    ViewBag.vedit = "disabled='disabled'";
                            //    ViewBag.vsave = "";
                            //    ViewBag.scripCall = "showmsgJS(3, 'Duplicate Name Found!', 2);";
                            //    return View(model);
                            //}
                            isedit = true;
                            vch_num = tmodel.Col16.Trim();
                            //id = tmodel.Col36.Trim();
                        }
                        else
                        {
                            mq1 = " and vch_num<>'" + tmodel.Col16.Trim() + "'";
                            mq = sgen.seekval(userCode, "select col31 as  master_name from " + model[0].Col10 + " where type='" + model[0].Col12 + "' " + model[0].Col11 + " " +
                                "and upper(col31)='" + model[0].Col17.ToUpper() + "' " + mq1 + " ", "master_name");
                            if (!mq.Trim().Equals("0"))
                            {
                                ViewBag.vnew = "disabled='disabled'";
                                ViewBag.vedit = "disabled='disabled'";
                                ViewBag.vsave = "";
                                ViewBag.scripCall = "showmsgJS(3, 'Duplicate Name Found!', 2);";
                                return View(model);
                            }
                            else
                            {
                                mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where type='" + model[0].Col12 + "' " + model[0].Col11 + "  ";
                                vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                                model[0].Col16 = vch_num;
                                isedit = false;
                            }
                        }
                        #region dtstr
                        DataRow dr = dtstr.NewRow();
                        dr["vch_num"] = vch_num.Trim();
                        dr["vch_date"] = sgen.Savedate(currdate, true);
                        dr["type"] = model[0].Col12.Trim();
                        dr["col1"] = model[0].Col18;
                        dr["col2"] = model[0].Col20;
                        dr["col3"] = model[0].Col22;
                        dr["col4"] = model[0].Col23;
                        dr["date1"] = sgen.Savedate(model[0].Col24, false);
                        dr["date2"] = sgen.Savedate(model[0].Col25, false);
                        dr["col5"] = model[0].Col26;
                        dr["col6"] = model[0].Col27;
                        dr["col7"] = model[0].Col29;
                        dr["col8"] = model[0].SelectedItems1.FirstOrDefault();// frequency
                        dr["col9"] = model[0].SelectedItems2.FirstOrDefault();//single location
                        dr["col10"] = model[0].Col38;
                        dr["date3"] = sgen.Make_date_S(model[0].Col39);
                        dr["col34"] = model[0].Col40;//remark
                        dr["col11"] = model[0].Col41;
                        dr["col12"] = model[0].Col42;
                        dr["col13"] = model[0].Col43;
                        dr["col14"] = model[0].SelectedItems3.FirstOrDefault();
                        dr["date4"] = sgen.Make_date_S(model[0].Col44);
                        dr["col15"] = model[0].Col45;
                        dr["col16"] = model[0].Col46;
                        dr["col17"] = model[0].Col47;
                        dr["col18"] = model[0].Col48;
                        dr["date5"] = sgen.Make_date_S(model[0].Col49);
                        dr["date6"] = sgen.Make_date_S(model[0].Col50);
                        dr["date7"] = sgen.Make_date_S(model[0].Col51);
                        dr["col19"] = model[0].Col52;
                        dr["col20"] = model[0].Col53;
                        dr["col21"] = model[0].Col54;
                        dr["col31"] = model[0].Col17;
                        dr["col32"] = model[0].Col19;
                        dr["col33"] = model[0].Col21;
                        if (isedit)
                        {
                            dr["ent_by"] = model[0].Col3;
                            dr["rec_id"] = model[0].Col7;
                            dr["ent_date"] = model[0].Col4.Trim();
                            dr["client_id"] = model[0].Col1.Trim();
                            dr["client_unit_id"] = model[0].Col2.Trim();
                            dr["edit_by"] = userid_mst.Trim();
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
                        #endregion
                        dtstr.Rows.Add(dr);
                        ok = sgen.Update_data_fast1_uncommit(userCode, dtstr, model[0].Col10.Trim(), model[0].Col8, isedit, sat1);
                        if (ok)
                        {
                            DataTable dtfile = new DataTable();
                            //dtfile = sgen.getdata(userCode, "select * from " + model[0].Col30 + " WHERE 1=2");
                            dtfile = cmd_Fun.GetStructure(userCode, model[0].Col30.Trim());
                            type_desc = "machine master file";
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
                                    filesave(model, currdate, dtfile, fileName1, encpath1, "Machine Master", ctype1);
                                }
                            }
                            catch (Exception ex) { }
                            res = sgen.Update_data(userCode, dtfile, model[0].Col30, model[0].Col31, false);
                            if (res)
                            {
                                sat1.Commit();
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
                                    Col28 = tmodel.Col28,
                                    Col30 = tmodel.Col30,
                                    TList1 = mod1,
                                    SelectedItems1 = new string[] { "" },
                                    TList2 = mod2,
                                    SelectedItems2 = new string[] { "" },
                                    TList3 = mod3,
                                    SelectedItems3 = new string[] { "" },
                                });
                                sgen.SetSession(MyGuid, "sch_upd1", null);
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
                                        model = newmachine_master(model);
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
                else if (command == "Import")
                {
                    HttpPostedFileBase file1 = upd2;
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
                        try
                        {
                            dt.Rows[0].Delete();
                            string currdate = sgen.server_datetime(userCode);
                            ent_date = currdate;
                            for (int k = 0; k < dt.Rows.Count; k++)
                            {
                                mq = sgen.seekval(userCode, "Select col31 machine_name from " + tab_name + " where type in ('" + type + "','DD" + type + "') " + where + " " +
                                    "and col31='" + dt.Rows[k]["MACHINE NAME"].ToString() + "'", "machine_name");
                                if (!mq.Trim().Equals("0"))
                                {
                                    ViewBag.scripCall += "showmsgJS(3, 'You already have " + dt.Rows[k]["MACHINE NAME"].ToString() + " Machine Name saved', 2);";
                                    ViewBag.vnew = "disabled='disabled'";
                                    ViewBag.vedit = "disabled='disabled'";
                                    ViewBag.vsave = "";
                                    ViewBag.vsavenew = "";
                                    return View(model);
                                }
                            }
                            DataTable dataTable = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                            int inc = 0, inc1 = 0;
                            mq = "select max(to_number(vch_num)) as auto_genid from " + tab_name + " where type in ('" + type + "','DD" + type + "')" + where + "";
                            inc = sgen.Make_int(sgen.genNo(userCode, mq, 6, "auto_genid"));
                            #region Frequency
                            mod1 = new List<SelectListItem>();
                            mod1 = cmd_Fun.freq(userCode, unitid_mst);
                            model[0].TList1 = mod1;
                            TempData[MyGuid + "_TList1"] = model[0].TList1;
                            model[0].SelectedItems1 = new string[] { "" };
                            #endregion
                            #region single Location
                            mod2 = new List<SelectListItem>();
                            defval = "";
                            mod2 = cmd_Fun.iloc(userCode, unitid_mst, out defval);
                            model[0].TList2 = mod2;
                            TempData[MyGuid + "_TList2"] = model[0].TList2;
                            model[0].SelectedItems2 = new string[] { defval };
                            #endregion
                            #region machine capacity
                            mod3 = new List<SelectListItem>();
                            mod3 = cmd_Fun.mcap(userCode, unitid_mst);
                            model[0].TList3 = mod3;
                            TempData[MyGuid + "_TList3"] = model[0].TList3;
                            model[0].SelectedItems3 = new string[] { "" };
                            #endregion
                            DataTable dtf = sgen.getdata(userCode, "SELECT master_id,upper(master_name) master_name FROM master_setting where type = 'FRE' and " +
                          "find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                            DataTable dtmcp = sgen.getdata(userCode, "SELECT master_id,upper(master_name) master_name FROM master_setting where type = 'K02' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                            DataTable dtlc = sgen.getdata(userCode, "SELECT master_id,upper(master_name) master_name,sectionid FROM master_setting where type = 'LC6' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
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
                                dr["col1"] = dt.Rows[i]["MACHINE SR NO"].ToString();
                                dr["col2"] = dt.Rows[i]["MACHINE MODEL"].ToString();
                                dr["col3"] = dt.Rows[i]["MACHINE CAPACITY"].ToString();
                                dr["col4"] = dt.Rows[i]["AVERAGE SPM"].ToString();
                                dr["date1"] = sgen.Make_date_S(dt.Rows[i]["PURCHASE DATE"].ToString());
                                dr["date2"] = sgen.Make_date_S(dt.Rows[i]["DUE DATE"].ToString());
                                dr["col5"] = model[0].Col26;
                                dr["col6"] = dt.Rows[i]["NO OF STROKE"].ToString();
                                dr["col7"] = dt.Rows[i]["STATUS"].ToString();
                                dr["col31"] = dt.Rows[i]["MACHINE NAME"].ToString();
                                dr["col32"] = dt.Rows[i]["MACHINE MANUFACTURER"].ToString();
                                if (dt.Rows[i]["FREQUENCY"].ToString() != "")
                                {
                                    if (dtf.Rows.Count > 0)
                                    {
                                        try
                                        {
                                            DataTable dtf2 = sgen.seekval_dt(dtf, "master_id='" + dt.Rows[i]["FREQUENCY"].ToString().ToUpper() + "'");
                                            dr["col8"] = dtf2.Rows[0]["master_id"].ToString();
                                        }
                                        catch (Exception)
                                        {
                                            ViewBag.vnew = "disabled='disabled'";
                                            ViewBag.vedit = "disabled='disabled'";
                                            ViewBag.vsave = "";
                                            ViewBag.vsavenew = "";
                                            ViewBag.scripCall = "showmsgJS(1, 'Entered FREQUENCY does not belongs to Masters, Please Check ! ', 2);";
                                            return View(model);
                                        }
                                    }
                                }
                                if (dt.Rows[i]["MACHINE LOCATION"].ToString() != "")
                                {
                                    if (dtlc.Rows.Count > 0)
                                    {
                                        try
                                        {
                                            DataTable dtlc2 = sgen.seekval_dt(dtlc, "master_id='" + dt.Rows[i]["MACHINE LOCATION"].ToString() + "'");
                                            dr["col9"] = dtlc2.Rows[0]["master_id"].ToString();
                                        }
                                        catch (Exception)
                                        {
                                            ViewBag.vnew = "disabled='disabled'";
                                            ViewBag.vedit = "disabled='disabled'";
                                            ViewBag.vsave = "";
                                            ViewBag.vsavenew = "";
                                            ViewBag.scripCall = "showmsgJS(1, 'Entered MACHINE LOCATION does not belongs to Masters, Please Check ! ', 2);";
                                            return View(model);
                                        }
                                    }
                                }
                                dr["col10"] = dt.Rows[i]["INVOICE NO"].ToString();
                                dr["date3"] = sgen.Make_date_S(dt.Rows[i]["ORG WARN EXPIRY DATE"].ToString());
                                dr["col34"] = dt.Rows[i]["REMARK"].ToString();
                                dr["col11"] = dt.Rows[i]["CONTACT PERSON NAME"].ToString();
                                dr["col12"] = dt.Rows[i]["CONTACT PERSON NO"].ToString();
                                dr["col13"] = dt.Rows[i]["CONTACT PERSON EMAILID"].ToString();
                                //dr["col14"] = dt.Rows[i]["MACHINE CAPACITY MASTER"].ToString();
                                if (dt.Rows[i]["MACHINE LOCATION"].ToString() != "")
                                {
                                    if (dtmcp.Rows.Count > 0)
                                    {
                                        try
                                        {
                                            DataTable dtmcp2 = sgen.seekval_dt(dtmcp, "master_id='" + dt.Rows[i]["MACHINE CAPACITY MASTER"].ToString() + "'");
                                            dr["col14"] = dtmcp2.Rows[0]["master_id"].ToString();
                                        }
                                        catch (Exception)
                                        {
                                            ViewBag.vnew = "disabled='disabled'";
                                            ViewBag.vedit = "disabled='disabled'";
                                            ViewBag.vsave = "";
                                            ViewBag.vsavenew = "";
                                            ViewBag.scripCall = "showmsgJS(1, 'Entered MACHINE CAPACITY MASTER does not belongs to Masters, Please Check ! ', 2);";
                                            return View(model);
                                        }
                                    }
                                }
                                dr["col15"] = dt.Rows[i]["WARRANTY TERMS"].ToString();
                                dr["date4"] = sgen.Make_date_S(dt.Rows[i]["EXTENDED WARRANTY EXPIRY DATE"].ToString());
                                //dr["col16"] = model[0].Col46;
                                //dr["col17"] = model[0].Col47;
                                //dr["col18"] = model[0].Col48;
                                //dr["date5"] = sgen.Make_date_S(model[0].Col49);
                                //dr["date6"] = sgen.Make_date_S(model[0].Col50);
                                //dr["date7"] = sgen.Make_date_S(model[0].Col51);
                                //dr["col19"] = model[0].Col52;
                                //dr["col20"] = model[0].Col53;
                                //dr["col21"] = model[0].Col54;
                                dr["rec_id"] = "0";
                                dr["ent_by"] = userid_mst;
                                dr["ent_date"] = currdate;
                                dr["client_id"] = clientid_mst;
                                dr["client_unit_id"] = unitid_mst;
                                dr["edit_by"] = "-";
                                dr["edit_date"] = currdate;
                                dataTable.Rows.Add(dr);
                            }
                            res = sgen.Update_data(userCode, dataTable, tab_name, model[0].Col6, false);
                            if (res == true)
                            {
                                model = new List<Tmodelmain>();
                                model.Add(new Tmodelmain
                                {
                                    Col13 = "Save",
                                    Col100 = "Save & New",
                                    Col121 = "S",
                                    Col122 = "<u>S</u>ave",
                                    Col123 = "Save & Ne<u>w</u>",
                                    TList1 = mod1,
                                    SelectedItems1 = new string[] { "" },
                                    TList2 = mod2,
                                    SelectedItems2 = new string[] { "" },
                                    TList3 = mod3,
                                    SelectedItems3 = new string[] { "" },
                                    Col9 = tmodel.Col9
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
            }
            catch (Exception err)
            {
                var LineNumber = new StackTrace(err, true).GetFrame(0).GetFileLineNumber();
                ViewBag.scripCall += "showmsgJS(1, '" + err.Message.Replace("'", "") + " At Line number " + LineNumber + "', 1);";
            }
            ModelState.Clear();
            return View(model);
        }
        private void filesave(List<Tmodelmain> model, string currdate, DataTable dtfile, string filename, string filepath, string filetitle, string content_type)
        {
            DataRow drf = dtfile.NewRow();
            drf["vch_num"] = vch_num;
            drf["rec_id"] = 0;
            drf["vch_date"] = currdate;
            drf["type"] = model[0].Col12;
            drf["ref_code"] = vch_num;
            drf["ref_code1"] = vch_num;
            drf["file_name"] = filename;
            drf["file_path"] = filepath;
            drf["col1"] = filetitle;
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
        //[HttpGet]
        //public FileResult fdown(string value)
        //{
        //    FillMst(MyGuid);
        //    if (!value.Trim().Equals(""))
        //    {
        //        DataTable dt2 = new DataTable();
        //        mq = "select file_name,file_path from file_tab where rec_id='" + value.Trim() + "' and type='EMP' and client_id='"
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
                sgenFun sgen = new sgenFun(MyGuid);
                string vch = sgen.GetSession(MyGuid, "Avch_num_std").ToString();
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
        [HttpGet]
        public FileResult mcmsttemp(List<Tmodelmain> model, string Myguid = "")
        {
            FillMst();
            model = (List<Tmodelmain>)sgen.GetSession(MyGuid, "model");
            DataTable dtl = new DataTable();
            mq = "select 'MACHINE MANUFACTURER' ictrl from dual union select  'MACHINE SR NO' ictrl from dual union " +
"select  'MACHINE NAME' ictrl from dual union SELECT upper(PARAM3) ictrl FROM CONTROLS WHERE TYPE = 'VDC' " +
"AND UPPER(PARAM5)= 'MACHINE MASTER' And param2 = 'Y' AND ID NOT IN " +
"('000012','000013','000014','000015','000016','000017','000018','000019','000020') and " +
"client_unit_id='" + unitid_mst + "'";
            dtl = sgen.getdata(userCode, mq);
            DataTable dtc = new DataTable();
            if (dtl.Rows.Count > 0)
            {
                for (int k = 0; k < dtl.Rows.Count; k++)
                {
                    dtc.Columns.Add(dtl.Rows[k]["ictrl"].ToString().Trim(), typeof(string));
                }
            }
            dtc.AcceptChanges();
            string cg_com_name = sgen.GetCookie(MyGuid, "cg_com_name");
            Byte[] fileBytes = sgen.Exp_to_csv_new(dtc, "mcmsttemp", cg_com_name);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "mcmsttemp");
        }
        #endregion
        //=================mould master===============
        #region
        public ActionResult mould_master()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "MOULD / TOOL MASTER";
            model[0].Col10 = "kc_tab";
            model[0].Col30 = "file_tab";
            model[0].Col12 = "MOS"; // mould master
            model[0].Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            model[0].Col27 = "Choose File";
            sgen.SetSession(MyGuid, "MM_MID", mid_mst.Trim());
            sgen.SetSession(MyGuid, "MM_MST", model[0].Col12.Trim());
            sgen.SetSession(MyGuid, "MM_COND_MST", model[0].Col11.Trim());
            sgen.SetSession(MyGuid, "MM_TBL_MST", model[0].Col10.Trim());
            model[0].LTM1 = new List<Tmodelmain>();
            List<SelectListItem> md1 = new List<SelectListItem>();
            TempData[MyGuid + "_TList1"] = model[0].TList1 = md1;
            TempData[MyGuid + "_TList3"] = model[0].TList3 = md1;
            TempData[MyGuid + "_TList5"] = model[0].TList5 = md1;
            model[0].SelectedItems1 = new string[] { "" };
            model[0].SelectedItems3 = new string[] { "" };
            model[0].SelectedItems5 = new string[] { "" };
            //TempData[MyGuid + "_TList1"] = model[0].TList1;
            //TempData[MyGuid + "_TList3"] = model[0].TList3;
            //TempData[MyGuid + "_TList5"] = model[0].TList5;
            //model.Add(tm);
            return View(model);
        }
        public List<Tmodelmain> newmould_master(List<Tmodelmain> model)
        {
            FillMst();
            model = getnew(model);
            model[0].Col22 = "Y";
            #region ddl
            List<SelectListItem> mod1 = new List<SelectListItem>();
            TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1 = cmd_Fun.freq(userCode, unitid_mst);
            //model[0].TList1 = mod1;
            //TempData[MyGuid + "_TList1"] = model[0].TList1;
            List<SelectListItem> mod3 = new List<SelectListItem>();
            TempData[MyGuid + "_TList3"] = model[0].TList3 = mod3 = cmd_Fun.mcap(userCode, unitid_mst);
            //model[0].TList3 = mod3;
            //TempData[MyGuid + "_TList3"] = model[0].TList3;
            TempData[MyGuid + "_TList5"] = model[0].TList5 = mod3;
            //TempData[MyGuid + "_TList5"] = model[0].TList5;
            #endregion
            return model;
        }
        [HttpPost]
        public ActionResult mould_master(List<Tmodelmain> model, string command, HttpPostedFileBase[] upd1, HttpPostedFileBase upd2)
        {
            try
            {
                FillMst(model[0].Col15);
                var tmodel = model.FirstOrDefault();
                type = model[0].Col12;
                tab_name = model[0].Col10;
                where = model[0].Col11;
                #region
                List<SelectListItem> mod1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
                TempData[MyGuid + "_TList1"] = model[0].TList1 = mod1;
                //TempData[MyGuid + "_TList1"] = model[0].TList1;
                if (model[0].SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
                List<SelectListItem> mod3 = (List<SelectListItem>)TempData[MyGuid + "_TList3"];
                TempData[MyGuid + "_TList3"] = model[0].TList3 = mod3;
                //TempData[MyGuid + "_TList3"] = model[0].TList3;
                if (model[0].SelectedItems3 == null) model[0].SelectedItems3 = new string[] { "" };
                List<SelectListItem> mod5 = (List<SelectListItem>)TempData[MyGuid + "_TList5"];
                TempData[MyGuid + "_TList5"] = model[0].TList5 = mod5;
                //TempData[MyGuid + "_TList5"] = model[0].TList5;
                if (model[0].SelectedItems5 == null) model[0].SelectedItems5 = new string[] { "" };
                #endregion
                if (command == "New")
                {
                    try
                    {
                        model = newmould_master(model);
                    }
                    catch { }
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
                        string currdate = sgen.server_datetime_local(userCode);
                        currdate = sgen.Savedate(currdate, true);
                        status = tmodel.Col22.Trim();
                        if (status == "N") { model[0].Col12 = "DDMOS"; }
                        DataTable dtstr = new DataTable();
                        dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                        if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                        {
                            mq1 = " and vch_num<>'" + tmodel.Col16.Trim() + "'";
                            mq = sgen.seekval(userCode, "select master_name from " + model[0].Col10 + " where type='" + model[0].Col12 + "' " + model[0].Col11 + " " +
                                "and master_name='" + model[0].Col17 + "' " + mq1 + " ", "master_name");
                            //if (!mq.Trim().Equals("0"))
                            //{
                            //    ViewBag.vnew = "disabled='disabled'";
                            //    ViewBag.vedit = "disabled='disabled'";
                            //    ViewBag.vsave = "";
                            //    ViewBag.scripCall = "showmsgJS(3, 'Duplicate Name Found!', 2);";
                            //    return View(model);
                            //}
                            //else
                            //{
                            isedit = true;
                            vch_num = tmodel.Col16.Trim();
                            //}
                        }
                        else
                        {
                            mq = sgen.seekval(userCode, "select master_name from " + model[0].Col10 + " where type='" + model[0].Col12 + "' " + model[0].Col11 + " and master_name='" + model[0].Col17 + "' ", "master_name");
                            if (!mq.Trim().Equals("0"))
                            {
                                ViewBag.vnew = "disabled='disabled'";
                                ViewBag.vedit = "disabled='disabled'";
                                ViewBag.vsave = "";
                                ViewBag.vsavenew = "";
                                ViewBag.scripCall = "showmsgJS(3, 'Duplicate Name Found!', 2);";
                                return View(model);
                            }
                            else
                            {
                                mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where type='" + model[0].Col12 + "' " + model[0].Col11 + "  ";
                                vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                                model[0].Col16 = vch_num;
                                isedit = false;
                            }
                        }
                        #region dtstr
                        DataRow dr = dtstr.NewRow();
                        dr["vch_num"] = vch_num.Trim();
                        dr["vch_date"] = sgen.Savedate(currdate, true);
                        dr["type"] = model[0].Col12.Trim();
                        dr["col1"] = model[0].Col18;
                        dr["col2"] = model[0].Col20;
                        dr["col3"] = model[0].Col22;
                        dr["col4"] = model[0].Col23;
                        dr["date1"] = sgen.Savedate(model[0].Col24, false);
                        dr["col5"] = model[0].Col25;
                        dr["col6"] = model[0].Col26;
                        dr["col7"] = model[0].SelectedItems1.FirstOrDefault();
                        dr["col8"] = model[0].Col28;  //Machine Tonnage
                        dr["col10"] = model[0].Col38;
                        dr["date3"] = sgen.Make_date_S(model[0].Col39);
                        dr["col34"] = model[0].Col40;//remark
                        dr["col11"] = model[0].Col41;
                        dr["col12"] = model[0].Col42;
                        dr["col13"] = model[0].Col43;
                        dr["col14"] = model[0].SelectedItems3.FirstOrDefault();
                        dr["date4"] = sgen.Make_date_S(model[0].Col44);
                        dr["col15"] = model[0].Col45;
                        dr["col16"] = model[0].Col46;
                        dr["col17"] = model[0].Col47;
                        dr["col18"] = model[0].Col48;
                        dr["date5"] = sgen.Make_date_S(model[0].Col49);
                        dr["date6"] = sgen.Make_date_S(model[0].Col50);
                        dr["date7"] = sgen.Make_date_S(model[0].Col51);
                        dr["col19"] = model[0].Col52;
                        dr["col20"] = model[0].Col53;
                        dr["col21"] = model[0].Col54;
                        dr["col31"] = model[0].Col17;
                        dr["col32"] = model[0].Col19;
                        dr["col33"] = model[0].Col21;
                        dr["col34"] = string.Join(",", model[0].SelectedItems5);
                        if (isedit)
                        {
                            dr["ent_by"] = model[0].Col3;
                            dr["rec_id"] = model[0].Col7;
                            dr["ent_date"] = model[0].Col4.Trim();
                            dr["client_id"] = model[0].Col1.Trim();
                            dr["client_unit_id"] = model[0].Col2.Trim();
                            dr["edit_by"] = userid_mst.Trim();
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
                        #endregion
                        dtstr.Rows.Add(dr);
                        bool ok = sgen.Update_data_fast1(userCode, dtstr, model[0].Col10, model[0].Col8, isedit);
                        if (ok)
                        {
                            DataTable dtfile = new DataTable();
                            //dtfile = sgen.getdata(userCode, "select * from " + model[0].Col30 + " WHERE 1=2");
                            dtfile = cmd_Fun.GetStructure(userCode, model[0].Col30.Trim());
                            type_desc = "mould master file";
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
                                    filesave(model, currdate, dtfile, fileName1, encpath1, "Mould Master", ctype1);
                                }
                            }
                            catch (Exception ex) { }
                            bool res = sgen.Update_data(userCode, dtfile, model[0].Col30, model[0].Col31, false);
                            if (res)
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
                                    Col27 = tmodel.Col27,
                                    Col30 = tmodel.Col30,
                                    Col100 = "Save & New",
                                    Col121 = "S",
                                    Col122 = "<u>S</u>ave",
                                    Col123 = "Save & Ne<u>w</u>",
                                    TList1 = mod1,
                                    TList3 = mod3,
                                    TList5 = mod5,
                                    SelectedItems1 = new string[] { "" },
                                    SelectedItems3 = new string[] { "" },
                                    SelectedItems5 = new string[] { "" }
                                });
                                sgen.SetSession(MyGuid, "sch_upd1", null);
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
                                        model = newmould_master(model);
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
                else if (command == "Import")
                {
                    HttpPostedFileBase file1 = upd2;
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
                        try
                        {
                            dt.Rows[0].Delete();
                            string currdate = sgen.server_datetime(userCode);
                            ent_date = currdate;
                            for (int k = 0; k < dt.Rows.Count; k++)
                            {
                                mq = sgen.seekval(userCode, "Select col1 machine_name from " + tab_name + " where type in ('" + type + "','DD" + type + "') " + where + " " +
                                    "and col1='" + dt.Rows[k]["Mould/Tool No"].ToString() + "'", "machine_name");
                                if (!mq.Trim().Equals("0"))
                                {
                                    ViewBag.scripCall += "showmsgJS(3, 'You already have " + dt.Rows[k]["Mould/Tool No"].ToString() + " Mould tool No. saved', 2);";
                                    ViewBag.vnew = "disabled='disabled'";
                                    ViewBag.vedit = "disabled='disabled'";
                                    ViewBag.vsave = "";
                                    ViewBag.vsavenew = "";
                                    return View(model);
                                }
                            }
                            DataTable dataTable = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                            int inc = 0, inc1 = 0;
                            mq = "select max(to_number(vch_num)) as auto_genid from " + tab_name + " where type in ('" + type + "','DD" + type + "')" + where + "";
                            inc = sgen.Make_int(sgen.genNo(userCode, mq, 6, "auto_genid"));
                            //string mid = "select max(to_number(master_id)) as auto_genid from " + tab_name + " where type in ('" + type + "','DD" + type + "')" + where + "";
                            //inc1 = sgen.Make_int(sgen.genNo(userCode, mid, 3, "auto_genid"));
                            mod1 = new List<SelectListItem>();
                            mod1 = cmd_Fun.freq(userCode, unitid_mst);
                            model[0].TList1 = mod1;
                            TempData[MyGuid + "_TList1"] = model[0].TList1;
                            mod3 = new List<SelectListItem>();
                            mod3 = cmd_Fun.mcap(userCode, unitid_mst);
                            model[0].TList3 = mod3;
                            TempData[MyGuid + "_TList3"] = model[0].TList3;
                            DataTable dtf = sgen.getdata(userCode, "SELECT master_id,master_name FROM master_setting where type = 'FRE' and " +
                                    "find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                            DataTable dtmcp = sgen.getdata(userCode, "SELECT master_id,master_name FROM master_setting where type = 'K02' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                if (i == 0)
                                {
                                    inc = inc + i;
                                }
                                else
                                {
                                    inc = inc + 1;
                                }
                                DataRow dr = dataTable.NewRow();
                                vch_num = sgen.padlc(inc, 6);
                                dr["vch_num"] = vch_num.Trim();
                                dr["vch_date"] = ent_date;
                                dr["type"] = type.Trim();
                                dr["col31"] = dt.Rows[i]["Mould/Tool"].ToString();
                                dr["col32"] = dt.Rows[i]["Part NAME"].ToString();
                                dr["col33"] = dt.Rows[i]["Mould Cavity"].ToString();
                                dr["col1"] = dt.Rows[i]["Mould/Tool No"].ToString();
                                dr["col2"] = dt.Rows[i]["PART No"].ToString();
                                dr["col3"] = dt.Rows[i]["Status"].ToString();
                                dr["col4"] = dt.Rows[i]["Cycle Time"].ToString();
                                dr["date1"] = sgen.Make_date_S(dt.Rows[i]["Due Date"].ToString());
                                //dr["date2"] = sgen.Make_date_S(dt.Rows[i]["DUE DATE"].ToString());
                                //dr["col5"] = model[0].Col25;
                                dr["col6"] = dt.Rows[i]["NO_OF_STROKE"].ToString();
                                //dr["col7"] = dt.Rows[i]["STATUS"].ToString();
                                if (dt.Rows[i]["FREQUENCY"].ToString() != "")
                                {
                                    if (dtf.Rows.Count > 0)
                                    {
                                        try
                                        {
                                            DataTable dtf2 = sgen.seekval_dt(dtf, "master_id='" + dt.Rows[i]["FREQUENCY"].ToString() + "'");
                                            dr["col7"] = dtf2.Rows[0]["master_id"].ToString();
                                        }
                                        catch (Exception)
                                        {
                                            ViewBag.vnew = "disabled='disabled'";
                                            ViewBag.vedit = "disabled='disabled'";
                                            ViewBag.vsave = "";
                                            ViewBag.vsavenew = "";
                                            ViewBag.scripCall = "showmsgJS(1, 'Entered FREQUENCY does not belongs to Masters, Please Check ! ', 2);";
                                            return View(model);
                                        }
                                    }
                                }
                                dr["col8"] = dt.Rows[i]["Machine Tonnage"].ToString();// frequency
                                                                                      //dr["col9"] = dt.Rows[i]["MACHINE LOCATION"].ToString();//single location
                                dr["col10"] = dt.Rows[i]["INVOICE NO"].ToString();
                                dr["date3"] = sgen.Make_date_S(dt.Rows[i]["ORG WARN EXPIRY DATE"].ToString());
                                dr["col34"] = dt.Rows[i]["REMARK"].ToString();
                                dr["col11"] = dt.Rows[i]["CONTACT PERSON NAME"].ToString();
                                dr["col12"] = dt.Rows[i]["CONTACT PERSON NO"].ToString();
                                dr["col13"] = dt.Rows[i]["CONTACT PERSON EMAILID"].ToString();
                                if (dt.Rows[i]["MACHINE CAPACITY MASTER"].ToString() != "")
                                {
                                    if (dtmcp.Rows.Count > 0)
                                    {
                                        try
                                        {
                                            DataTable dtmcp2 = sgen.seekval_dt(dtmcp, "master_id='" + dt.Rows[i]["MACHINE CAPACITY MASTER"].ToString() + "'");
                                            dr["col14"] = dtmcp2.Rows[0]["master_id"].ToString();
                                        }
                                        catch (Exception)
                                        {
                                            ViewBag.vnew = "disabled='disabled'";
                                            ViewBag.vedit = "disabled='disabled'";
                                            ViewBag.vsave = "";
                                            ViewBag.vsavenew = "";
                                            ViewBag.scripCall = "showmsgJS(1, 'Entered MACHINE CAPACITY MASTER does not belongs to Masters, Please Check ! ', 2);";
                                            return View(model);
                                        }
                                    }
                                }
                                dr["col15"] = dt.Rows[i]["WARRANTY TERMS"].ToString();
                                dr["date4"] = sgen.Make_date_S(dt.Rows[i]["EXTENDED WARRANTY EXPIRY DATE"].ToString());
                                dr["rec_id"] = "0";
                                dr["ent_by"] = userid_mst;
                                dr["ent_date"] = currdate;
                                dr["client_id"] = clientid_mst;
                                dr["client_unit_id"] = unitid_mst;
                                dr["edit_by"] = "-";
                                dr["edit_date"] = currdate;
                                dataTable.Rows.Add(dr);
                            }
                            res = sgen.Update_data(userCode, dataTable, tab_name, model[0].Col6, false);
                            if (res == true)
                            {
                                model = new List<Tmodelmain>();
                                model.Add(new Tmodelmain
                                {
                                    Col13 = "Save",
                                    Col100 = "Save & New",
                                    Col121 = "S",
                                    Col122 = "<u>S</u>ave",
                                    Col123 = "Save & Ne<u>w</u>",
                                    TList1 = mod1,
                                    SelectedItems1 = new string[] { "" },
                                    TList3 = mod3,
                                    SelectedItems3 = new string[] { "" },
                                    Col9 = tmodel.Col9
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
        public FileResult fdown(string value, string typ, string Myguid = "")
        {
            FillMst(Myguid);
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
        //[HttpPost]
        //public void fdelete(string value)
        //{
        //    if (!value.Trim().Equals(""))
        //    {
        //        Session["delid"] = value;
        //    }
        //}
        [HttpGet]
        public FileResult mtmsttemp(List<Tmodelmain> model)
        {
            FillMst();
            model = (List<Tmodelmain>)sgen.GetSession(MyGuid, "model");
            DataTable dtl = new DataTable();
            mq = "select 'Due Date' ictrl from dual union select 'Frequency' ictrl from dual union select 'NO_OF_STROKE'" +
                " ictrl from dual union select 'Status' ictrl from dual union select 'Mould/Tool' ictrl from dual " +
                "union select 'Mould/Tool No' ictrl from dual union select  'PART NAME' ictrl from dual union select" +
                " 'Part No' ictrl from dual union select 'Mould Cavity' ictrl from dual union select 'Cycle Time' " +
                "ictrl from dual union select 'Machine Tonnage' ictrl from dual union SELECT UPPER(PARAM3) FROM " +
                "CONTROLS WHERE TYPE = 'VDC' AND UPPER(PARAM5)= 'MOULD / TOOL MASTER' And param2 = 'Y' AND ID NOT" +
                " IN ('000012','000013','000014','000015','000016','000017','000018','000019','000020') and " +
                "client_unit_id='" + unitid_mst + "'";
            dtl = sgen.getdata(userCode, mq);
            DataTable dtc = new DataTable();
            if (dtl.Rows.Count > 0)
            {
                for (int k = 0; k < dtl.Rows.Count; k++)
                {
                    dtc.Columns.Add(dtl.Rows[k]["ictrl"].ToString().Trim(), typeof(string));
                }
            }
            dtc.AcceptChanges();
            string cg_com_name = sgen.GetCookie(MyGuid, "cg_com_name");
            Byte[] fileBytes = sgen.Exp_to_csv_new(dtc, "mtmsttemp", cg_com_name);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "mtmsttemp");
        }
        #endregion
        //==================machine breakdown==============
        #region
        public ActionResult machine_brkdwn()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            model = getload(model);
            model[0].Col9 = "MACHINE BREAKDOWN";
            model[0].Col10 = "courses";
            model[0].Col12 = "MBD"; // mmachine breakdown
            model[0].Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            sgen.SetSession(MyGuid, "MB_MID", mid_mst.Trim());
            sgen.SetSession(MyGuid, "MB_MST", model[0].Col12.Trim());
            sgen.SetSession(MyGuid, "MB_COND_MST", model[0].Col11.Trim());
            sgen.SetSession(MyGuid, "MB_TBL_MST", model[0].Col10.Trim());
            model[0].TList1 = mod1;
            model[0].SelectedItems1 = new string[] { "" };
            return View(model);
        }
        public List<Tmodelmain> newmachine_brkdwn(List<Tmodelmain> model)
        {
            model = getnew(model);
            var tm = model.FirstOrDefault();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            #region reason for breakup
            mod1 = cmd_Fun.break_upreason(userCode, unitid_mst);
            TempData[MyGuid + "_Tlist1"] = mod1;
            #endregion
            model[0].TList1 = mod1;
            if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
            return model;
        }
        [HttpPost]
        public ActionResult machine_brkdwn(List<Tmodelmain> model, string command)
        {
            try
            {
                FillMst(model[0].Col15);
                var tm = model.FirstOrDefault();
                List<SelectListItem> mod1 = new List<SelectListItem>();
                model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
                TempData[MyGuid + "_TList1"] = model[0].TList1;
                if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
                if (command == "New")
                {
                    try
                    {
                        model = newmachine_brkdwn(model);
                    }
                    catch { }
                }
                else if (command == "Cancel")
                {
                    return CancelFun(model);
                }
                else if (command == "Callback")
                {
                    model = StartCallback(model);
                    //if (Session["btnval"] != null) btnval = Session["btnval"].ToString();
                    //model = CallbackFun(btnval, "N", "machine_brkdwn", "Inventory", model);
                    //ViewBag.vnew = "disabled='disabled'";
                    //ViewBag.vedit = "disabled='disabled'";
                    //ViewBag.vsave = "";
                }
                else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
                {
                    try
                    {
                        var tmodel = model.FirstOrDefault();
                        string currdate = sgen.server_datetime_local(userCode);
                        currdate = sgen.Savedate(currdate, true);
                        //status = tmodel.Col19.Trim();
                        //if (status == "N") { model[0].Col22 = "DDMBD"; }
                        type_desc = "machine_brkdwn";
                        DataTable dtstr = new DataTable();
                        //dtstr = sgen.getdata(userCode, "select * from " + model[0].Col10 + " WHERE 1=2");
                        dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                        if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                        {
                            mq1 = " and vch_num<>'" + tmodel.Col16.Trim() + "'";
                            isedit = true;
                            vch_num = tmodel.Col16.Trim();
                            id = tmodel.Col21.Trim();
                        }
                        else
                        {
                            //mq = sgen.seekval(userCode, "select master_name from " + model[0].Col10 + " where type='" + model[0].Col12 + "' " + model[0].Col11 + " and master_name='" + model[0].Col17 + "' ", "master_name");
                            //if (!mq.Trim().Equals("0"))
                            //{
                            //    ViewBag.scripCall = "showmsgJS(3, 'You Already Saved Mould Name', 1);";
                            //    return View(model);
                            //}
                            //else
                            //{
                            mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where type='" + model[0].Col12 + "' " + model[0].Col11 + "  ";
                            vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                            model[0].Col16 = vch_num;
                            string mid = "select max(to_number(course_id)) as auto_genid from " + model[0].Col10 + " where type='" + model[0].Col12 + "' " + model[0].Col11 + "";
                            id = sgen.genNo(userCode, mid, 3, "auto_genid");
                            isedit = false;
                            //}
                        }
                        #region dtstr
                        DataRow dr = dtstr.NewRow();
                        dr["rec_id"] = "0";
                        dr["vch_num"] = vch_num.Trim();
                        dr["vch_date"] = currdate;
                        dr["type"] = model[0].Col12.Trim();
                        dr["course_id"] = id.Trim();
                        dr["cou_title"] = model[0].Col17; //name
                        dr["cou_descp"] = model[0].Col18; // location
                        dr["cou_payment"] = model[0].SelectedItems1.FirstOrDefault(); // reason 
                        dr["occ_date"] = sgen.Savedate(model[0].Col20, true); // datetime
                        #endregion
                        if (isedit)
                        {
                            dr["client_id"] = model[0].Col1.Trim();
                            dr["client_unit_id"] = model[0].Col2.Trim();
                            dr["vch_num"] = model[0].Col16;
                            dr["rec_id"] = model[0].Col7;
                            dr["cou_ent_by"] = model[0].Col3;
                            dr["cou_ent_date"] = model[0].Col4;
                            dr["cou_edit_by"] = userid_mst;
                            dr["cou_edit_date"] = currdate;
                        }
                        else
                        {
                            dr["rec_id"] = "0";
                            dr["client_id"] = clientid_mst;
                            dr["client_unit_id"] = unitid_mst;
                            dr["cou_ent_by"] = userid_mst;
                            dr["cou_ent_date"] = currdate;
                            dr["cou_edit_by"] = "-";
                            dr["cou_edit_date"] = currdate;
                        }
                        dtstr.Rows.Add(dr);
                        bool Result = sgen.Update_data(userCode, dtstr, model[0].Col10, tmodel.Col8, isedit);
                        ViewBag.vnew = "";
                        ViewBag.vedit = "";
                        ViewBag.vsave = "disabled='disabled'";
                        if (Result == true)
                        {
                            model = new List<Tmodelmain>();
                            tmodel.Col10 = tmodel.Col10;
                            tmodel.Col14 = tmodel.Col14;
                            tmodel.Col11 = tmodel.Col11;
                            tmodel.Col12 = tmodel.Col12;
                            tmodel.Col13 = "Save";
                            tmodel.Col100 = "Save & New";
                            tmodel.Col121 = "S";
                            tmodel.Col122 = "<u>S</u>ave";
                            tmodel.Col123 = "Save & Ne<u>w</u>";
                            tmodel.Col9 = "MACHINE BREAKDOWN MASTER";
                            tmodel.Col16 = "";
                            tmodel.Col17 = "";
                            tmodel.Col18 = "";
                            tmodel.Col19 = "";
                            tmodel.Col20 = "";
                            tmodel.TList1 = mod1;
                            tmodel.SelectedItems1 = new string[] { "" };
                            model.Add(tmodel);
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
                                    model = newmachine_brkdwn(model);
                                    ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                                }
                                catch (Exception ex) { }
                            }

                        }
                        else { ViewBag.scripCall = "showmsgJS(3, 'Record Not Saved', 1);"; }
                    }
                    catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
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
        //===================================================vivek
        #region operator_master
        public ActionResult operator_master()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<SelectListItem> mod2 = new List<SelectListItem>();
            if (Request.QueryString["url"] != null && Request.QueryString["btnval"] != null)
            {
                model[0].Col50 = Request.QueryString["url"].ToString();
                model[0].Col51 = Request.QueryString["btnval"].ToString();
                model[0].Col52 = "Y";
            }
            model[0].Col9 = "OPERATOR NAME MASTER ";
            model[0].Col10 = "master_setting";
            model[0].Col12 = "OPR"; //OPERATOR NAME MASTER
            model[0].Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            model[0].TList1 = mod1;
            model[0].SelectedItems1 = new string[] { "" };
            model[0].TList2 = mod2;
            model[0].SelectedItems2 = new string[] { "" };
            return View(model);
        }
        public List<Tmodelmain> newoperator_master(List<Tmodelmain> model)
        {
            model = getnew(model);
            model[0].Col20 = "Y";
            #region binddept
            List<SelectListItem> mod1 = cmd_Fun.dept(userCode, clientid_mst, unitid_mst);
            TempData[MyGuid + "_Tlist1"] = mod1;
            #endregion
            #region Binddesig
            List<SelectListItem> mod2 = cmd_Fun.desig(userCode, unitid_mst);
            TempData[MyGuid + "_Tlist2"] = mod2;
            #endregion
            model[0].TList1 = mod1;
            model[0].TList2 = mod2;
            model[0].SelectedItems2 = new string[] { "" };
            model[0].SelectedItems1 = new string[] { "" };
            return model;
        }
        [HttpPost]
        public ActionResult operator_master(List<Tmodelmain> model, string command, string url)
        {
            try
            {
                FillMst(model[0].Col15);
                #region
                List<SelectListItem> mod1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
                List<SelectListItem> mod2 = (List<SelectListItem>)TempData[MyGuid + "_TList2"];
                model[0].TList1 = mod1;
                model[0].TList2 = mod2;
                TempData[MyGuid + "_TList1"] = mod1;
                TempData[MyGuid + "_TList2"] = mod2;
                if (model[0].SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
                if (model[0].SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
                #endregion
                if (command == "New")
                {
                    try
                    {
                        model = newoperator_master(model);
                        #region
                        sgen.SetSession(MyGuid, "EDMODE", "N");
                      
                        #endregion
                        //isedit = false;
                        model[0].Col50 = "ok";
                        model[0].Col52 = "N";
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
                    try
                    {
                        var tmodel = model.FirstOrDefault();
                        string currdate = sgen.server_datetime(userCode);
                        ent_date = currdate;
                        if (model[0].Col20 == "Y")
                        {
                            status = "Y";
                            model[0].Col12 = "OPR";
                            inactive_date = currdate;
                        }
                        else
                        {
                            status = "N";
                            model[0].Col12 = "DDOPR";
                            inactive_date = currdate;
                        }
                        if (model[0].Col17 == null && model[0].SelectedItems1.FirstOrDefault() == "" && model[0].SelectedItems2.FirstOrDefault() == "")
                        {
                            ViewBag.vnew = "disabled='disabled'";
                            ViewBag.vedit = "disabled='disabled'";
                            ViewBag.vsave = "";
                            ViewBag.vsavenew = "";
                            ViewBag.scripCall += "showmsgJS(1, 'All Fields Are Mandatory To Fill, Please Check', 2);";
                            return View(model);
                        }
                        DataTable dtstr = new DataTable();
                        //dtstr = sgen.getdata(userCode, "select * from " + model[0].Col10 + " WHERE 1=2");
                        dtstr = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                        if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                        {
                            mq1 = " and vch_num<>'" + tmodel.Col16.Trim() + "'";
                            isedit = true;
                            vch_num = tmodel.Col16.Trim();
                            id = tmodel.Col21.Trim();
                        }
                        else
                        {
                            isedit = false;
                            mq = "select max(to_number(master_id)) as auto_genid from " + model[0].Col10 + " where  (type='OPR' or type ='DDOPR' ) " + model[0].Col11 + "";
                            id = sgen.genNo(userCode, mq, 3, "auto_genid");
                            mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10 + " where (type='OPR' or type ='DDOPR' ) " + model[0].Col11 + "";
                            vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                            tmodel.Col21 = id;
                            cond = sgen.seekval(userCode, "select master_name from " + model[0].Col10 + " where upper(master_name)='" + model[0].Col17.Trim().ToUpper() + "' and " +
                             "type in ('" + model[0].Col12 + "','DD" + model[0].Col12 + "') " + model[0].Col11 + "" + mq1 + "", "master_name");
                            if (cond != "0")
                            {
                                ViewBag.vnew = "disabled='disabled'";
                                ViewBag.vedit = "disabled='disabled'";
                                ViewBag.vsave = "";
                                ViewBag.vsavenew = "";
                                ViewBag.scripCall += "showmsgJS(1, 'Data Already Exists', 2);";
                                return View(model);
                            }
                        }
                        #region dtstr operator_master
                        DataRow dr = dtstr.NewRow();
                        dr["rec_id"] = "0";
                        dr["vch_num"] = vch_num;
                        dr["vch_date"] = sgen.Savedate(currdate, false);
                        dr["type"] = model[0].Col12;
                        dr["master_id"] = id;
                        dr["master_name"] = model[0].Col17;
                        dr["classid"] = model[0].SelectedItems1.FirstOrDefault();
                        dr["sectionid"] = model[0].SelectedItems2.FirstOrDefault();
                        dr["Status"] = status;
                        dr["Inactive_date"] = sgen.Savedate(inactive_date, false);
                        dr["client_id"] = clientid_mst;
                        dr["client_unit_id"] = unitid_mst;
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
                            dr["master_entby"] = userid_mst;
                            dr["master_entdate"] = currdate;
                            dr["master_editby"] = "-";
                            dr["master_editdate"] = currdate;
                        }
                        dtstr.Rows.Add(dr);
                        #endregion
                        bool Result = sgen.Update_data(userCode, dtstr, model[0].Col10, tmodel.Col8, isedit);
                        if (Result == true)
                        {
                            sgen.SetSession(MyGuid, "parent_op", tmodel.Col21 + "~!~!~!~!" + tmodel.Col51);
                            model = new List<Tmodelmain>();
                            model.Add(new Tmodelmain
                            {
                                Col9 = tmodel.Col9,
                                Col10 = tmodel.Col10,
                                Col11 = tmodel.Col11,
                                Col12 = tmodel.Col12,
                                Col13 = "Save",
                                Col121 = "S",
                                Col122 = "<u>S</u>ave",
                                Col123 = "Save & Ne<u>w</u>",
                                Col100 = "Save & New",
                                Col14 = tmodel.Col14,
                                Col15 = tmodel.Col15,
                                TList1 = mod1,
                                SelectedItems1 = new string[] { "" },
                                TList2 = mod2,
                                SelectedItems2 = new string[] { "" },
                                Col50 = "ok",
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
                                    model = newoperator_master(model);
                                    ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                                }
                                catch (Exception ex) { }
                            }

                        }
                        else { ViewBag.scripCall = "showmsgJS(3, 'Record Not Saved', 1);"; }
                    END:;
                    }
                    catch (Exception ex) { ViewBag.scripCall = "showmsgJS(3, '" + ex.Message.ToString().Replace("'", "") + ", 1);"; }
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
        //========================================================= 
        #region pm
        public ActionResult preventive_maint()
        {
            FillMst();
            //if (userCode.Equals("")) Response.Redirect(sgen.GetCookie("", "",sgenFun.callbackurl));
            ////if (Request.UrlReferrer == null) { Response.Redirect(sgen.GetCookie("", "",sgenFun.callbackurl)); }
            //ViewBag.vnew = "";
            //ViewBag.vedit = "";
            //ViewBag.vsave = "disabled='disabled'";
            //ViewBag.scripCall = "disableForm();";
            //mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            //MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            //Tmodelmain tm1 = new Tmodelmain();
            //tm1.Col14 = mid_mst.Trim();
            //tm1.Col15 = MyGuid.Trim();
            model[0].Col9 = "PREVENTIVE MAINTENANCE";
            model[0].Col10 = "enx";
            model[0].Col12 = "PRM"; //preventive  MASTER
            model[0].Col13 = "Save";
            model[0].Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            sgen.SetSession(MyGuid, "PM_MID", mid_mst.Trim());
            sgen.SetSession(MyGuid, "PM_TYPE_MST", model[0].Col12.Trim());
            sgen.SetSession(MyGuid, "PM_COND_MST", model[0].Col11.Trim());
            sgen.SetSession(MyGuid, "PM_TBL_MST", model[0].Col10.Trim());
            List<SelectListItem> mod1 = new List<SelectListItem>();
            model[0].TList1 = mod1;
            model[0].SelectedItems1 = new string[] { "" };
            List<SelectListItem> mod2 = new List<SelectListItem>();
            model[0].TList2 = mod2;
            model[0].SelectedItems2 = new string[] { "" };
            List<SelectListItem> mod3 = new List<SelectListItem>();
            model[0].TList3 = mod3;
            model[0].SelectedItems3 = new string[] { "" };
            //model.Add(tm1);
            return View(model);
        }
        public List<Tmodelmain> newpreventive_maint(List<Tmodelmain> model)
        {
            model = getnew(model);
            //model[0].TList1 = mod1;
            //model[0].TList2 = mod2;
            //model[0].TList3 = mod3;
            model[0].SelectedItems2 = new string[] { "" };
            model[0].SelectedItems1 = new string[] { "" };
            model[0].SelectedItems3 = new string[] { "" };
            model[0].Col25 = "Y";
            model[0].Chk7 = true;
            return model;
        }
        [HttpPost]
        public ActionResult preventive_maint(List<Tmodelmain> model, string command)
        {
            try
            {
                FillMst(model[0].Col15);
                List<SelectListItem> mod1 = new List<SelectListItem>();
                List<SelectListItem> mod2 = new List<SelectListItem>();
                List<SelectListItem> mod3 = new List<SelectListItem>();
                if (command == "New")
                {
                    try
                    {
                        model = newpreventive_maint(model);
                        //Session["EDMODE"] = "N";
                        //ViewBag.vnew = "disabled='disabled'";
                        //ViewBag.vedit = "disabled='disabled'";
                        //ViewBag.vsave = "";
                        //ViewBag.scripCall = "enableForm();";
                        model[0].TList1 = mod1;
                        model[0].TList2 = mod2;
                        model[0].TList3 = mod3;
                        //model[0].SelectedItems2 = new string[] { "" };
                        //model[0].SelectedItems1 = new string[] { "" };
                        //model[0].SelectedItems3 = new string[] { "" };
                        //model[0].Col25 = "Y";
                        //model[0].Chk7 = true;
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
                    //if (Session["btnval"] != null) btnval = Session["btnval"].ToString();
                    //model = CallbackFun(btnval, "N", "preventive_maint", "Production", model);
                    //ViewBag.vnew = "disabled='disabled'";
                    //ViewBag.vedit = "disabled='disabled'";
                    //ViewBag.vsave = "";
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
        #region get grid master for prod_entry
        [HttpGet]
        public ActionResult Getpdstg(string searchTerm, int pageSize, int pageNum, string icode)
        {
            FillMst(MyGuid);
            //Get the paged results and the total count of the results for this query. 
            mq = "select distinct bm.pono icode,bm.cond iname,'-' partno,'-' uom,'-' hsn,'-' taxrate,'-' stock from itransactionc bm where bm.type = 'BOM' and " +
                "bm.client_unit_id = '" + unitid_mst + "' and bm.acode = '" + icode + "' " +
                "union all " +
                "select '100' icode,'PDI' iname,'-' partno,'-' uom,'-' hsn,'-' taxrate,'-' stock from dual bm " +
                "union all " +
                "select '102' icode,'Rework' iname,'-' partno,'-' uom,'-' hsn,'-' taxrate,'-' stock from dual bm";
            DataSet ds = getitemData(userCode, mq, 1, searchTerm);
            return DstoJSonItems(ds);
        }
        [HttpGet]
        public ActionResult GetFstg(string searchTerm, int pageSize, int pageNum, string icode)
        {
            FillMst();
            //Get the paged results and the total count of the results for this query. 
            DataSet ds = getitemData(userCode, "select master_id icode,client_name iname,'-' partno,'-' uom,'-' hsn,'-' taxrate,'-' stock from master_Setting " +
                "where type='KPS'", 1, searchTerm);
            return DstoJSonItems(ds);
        }
        [HttpGet]
        public ActionResult Getmachine(string searchTerm, int pageSize, int pageNum)
        {
            FillMst();
            //Get the paged results and the total count of the results for this query. 
            DataSet ds = getitemData(userCode, "select vch_num icode,col31 iname,'-' partno,'-' uom,'-' hsn,'-' taxrate,'-' stock from kc_tab " +
                "where client_unit_id='" + unitid_mst + "' and type='PMM'", 1, searchTerm);
            return DstoJSonItems(ds);
        }
        [HttpGet]
        public ActionResult Getop(string searchTerm, int pageSize, int pageNum)
        {
            FillMst();
            //Get the paged results and the total count of the results for this query. 
            DataSet ds = getitemData(userCode, "select master_id icode,master_name iname,'-' partno,'-' uom,'-' hsn,'-' taxrate,'-' stock from master_Setting " +
                "where client_unit_id='" + unitid_mst + "' and type='OPR'", 1, searchTerm);
            return DstoJSonItems(ds);
        }
        [HttpGet]
        public ActionResult Getmould(string searchTerm, int pageSize, int pageNum)
        {
            FillMst();
            //Get the paged results and the total count of the results for this query. 
            DataSet ds = getitemData(userCode, "select vch_num icode,col31 iname,'-' partno,'-' uom,'-' hsn,'-' taxrate,'-' stock from kc_tab " +
                "where client_unit_id='" + unitid_mst + "' and type='MOS'", 1, searchTerm);
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
        #endregion
        #region prod_entry
        public ActionResult prod_entry()
        {
            FillMst();
            //if (userCode.Equals("")) { return Redirect(sgenFun.callbackurl); }
            //if (Request.UrlReferrer == null) { return Redirect(sgenFun.callbackurl); }
            //ViewBag.vnew = "";
            //ViewBag.vedit = "";
            //ViewBag.vsave = "disabled='disabled'";
            //ViewBag.scripCall = "disableForm();";
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            List<SelectListItem> ddl_shft = new List<SelectListItem>();
            List<SelectListItem> ddl_dept = new List<SelectListItem>();
            //Tmodelmain tm1 = new Tmodelmain();
            //mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            //MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            model[0].Col9 = "PRODUCTION ENTRY"; //heading
            model[0].Col10 = "iprod";
            model[0].Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            model[0].Col12 = "86"; // type
            //model[0].Col14 = mid_mst.Trim();
            //model[0].Col15 = MyGuid.Trim();
            DataTable dt = sgen.getdata(userCode, "select '1' as  SNo,'-' as Icode,'-' as IName,'-' Con,'-' From_Icode,'-' From_Iname,'-' FStgCode,'-' FromStage,'-' TstgCode,'-' ToStage,'0' Qty_Planned,'0' Qty_Produce," +
                "'0' Qty_Consume,'-' OpCode,'-' Operator,'-' MachineCode,'-' Machine,'-' MouldCode,'-' Mould,'-' as Tool_StTime,'-' Tool_EdTime,'-' Remark from dual");
            model[0].dt1 = dt;
            sgen.SetSession(MyGuid, "KPD_DT", dt);
            model[0].TList1 = ddl_shft;
            model[0].TList2 = ddl_dept;
            model[0].SelectedItems1 = new string[] { "" };
            model[0].SelectedItems2 = new string[] { "" };
            //model.Add(tm1);
            ModelState.Clear();
            return View(model);
        }
        public List<Tmodelmain> newprod_entry(List<Tmodelmain> model)
        {
            model = getnew(model);
            return model;
        }
        [HttpPost]
        public ActionResult prod_entry(List<Tmodelmain> model, string command, string hfrow, string hftable)
        {
            try
            {
                FillMst(model[0].Col15);
                DataTable dtm = sgen.settable(hftable);
                DataTable dt = new DataTable();
                model[0].dt1 = dtm;
                model[0].Col24 = hfrow;
                var tm = model.FirstOrDefault();
                List<SelectListItem> ddl_shft = new List<SelectListItem>();
                List<SelectListItem> ddl_dept = new List<SelectListItem>();
                model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
                model[0].TList2 = (List<SelectListItem>)TempData[MyGuid + "_TList2"];
                TempData[MyGuid + "_TList1"] = model[0].TList1;
                TempData[MyGuid + "_TList2"] = model[0].TList2;
                if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
                if (tm.SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
                if (command == "New")
                {
                    try
                    {
                        model = getnew(model);
                        //Session["EDMODE"] = "N";
                        //ViewBag.vnew = "disabled='disabled'";
                        //ViewBag.vedit = "disabled='disabled'";
                        //ViewBag.vsave = "";
                        //ViewBag.scripCall = "enableForm();";
                        //mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                        //vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                        model[0].Col17 = "1";
                        model[0].Col20 = vch_num;
                        //model[0].Col13 = "Save";
                        //model[0].Col100 = "Save & New";
                        #region getshft
                        ddl_shft = cmd_Fun.getshft(userCode, unitid_mst);
                        //dt = sgen.getdata(userCode, "SELECT master_id,master_name FROM master_setting WHERE Type='STM'" + model[0].Col11.Trim() + "");
                        //if (dt.Rows.Count > 0)
                        //{
                        //    foreach (DataRow dr in dt.Rows)
                        //    {
                        //        ddl_shft.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                        //    }
                        //}
                        TempData[MyGuid + "_Tlist1"] = ddl_shft;
                        #endregion
                        #region getdept
                        //dt = sgen.getdata(userCode, "SELECT master_id,master_name FROM master_setting WHERE Type='MDP'");
                        //if (dt.Rows.Count > 0)
                        //{
                        //    foreach (DataRow dr in dt.Rows)
                        //    {
                        //        ddl_dept.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                        //    }
                        //}
                        ddl_dept = cmd_Fun.dept(userCode, clientid_mst, unitid_mst);
                        TempData[MyGuid + "_Tlist2"] = ddl_dept;
                        #endregion
                        model[0].TList1 = ddl_shft;
                        model[0].TList2 = ddl_dept;
                        model[0].SelectedItems1 = new string[] { "" };
                        model[0].SelectedItems2 = new string[] { "" };
                    }
                    catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "',0);"; }
                }
                if (command == "Cancel")
                {
                    return CancelFun(model);
                }
                else if (command == "Callback")
                {
                    model = StartCallback(model);
                    //if (Session["btnval"] != null) btnval = Session["btnval"].ToString();
                    //model = CallbackFun(btnval, "N", "prod_entry", "Production", model);
                    //ViewBag.vnew = "disabled='disabled'";
                    //ViewBag.vedit = "disabled='disabled'";
                    //ViewBag.vsave = "";
                }
                else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
                {
                    try
                    {
                        string currdate = sgen.server_datetime(userCode);
                        if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                        {
                            isedit = true;
                            vch_num = model[0].Col20;
                        }
                        else
                        {
                            isedit = false;
                            mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                            vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                            model[0].Col20 = vch_num;
                        }
                        //DataTable dataTable = sgen.getdata(userCode, "select  * from " + model[0].Col10.Trim() + "  where  1=2");
                        DataTable dataTable = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                        for (int i = 0; i < model[0].dt1.Rows.Count; i++)
                        {
                            for (int k = 0; k < 2; k++)
                            {
                                DataRow dr = dataTable.NewRow();
                                dr["rec_id"] = "0";
                                dr["type"] = model[0].Col12;
                                dr["vch_num"] = vch_num.Trim();
                                dr["vch_date"] = sgen.Savedate(model[0].Col21, true);
                                dr["shftno"] = model[0].SelectedItems1.FirstOrDefault();
                                dr["shftname"] = model[0].TList1.SingleOrDefault(w => w.Value == model[0].SelectedItems1.FirstOrDefault()).Text;
                                dr["deptno"] = model[0].SelectedItems2.FirstOrDefault();
                                dr["deptname"] = model[0].TList2.SingleOrDefault(w => w.Value == model[0].SelectedItems2.FirstOrDefault()).Text;
                                dr["totremark"] = model[0].Col22;
                                //dt====
                                dr["qtyplanned"] = sgen.Make_decimal(model[0].dt1.Rows[i][10].ToString());
                                dr["rno"] = model[0].dt1.Rows[i][0].ToString();
                                if (k == 0) //produce
                                {
                                    dr["icode"] = model[0].dt1.Rows[i][1].ToString();
                                    dr["iname"] = model[0].dt1.Rows[i][2].ToString();
                                    dr["stage"] = model[0].dt1.Rows[i][6].ToString();
                                    dr["qtyin"] = model[0].dt1.Rows[i][11].ToString();
                                    dr["pflag"] = "1";
                                }
                                else //consume
                                {
                                    dr["icode"] = model[0].dt1.Rows[i][4].ToString();
                                    dr["iname"] = model[0].dt1.Rows[i][5].ToString();
                                    dr["stage"] = model[0].dt1.Rows[i][8].ToString();
                                    dr["qtyout"] = model[0].dt1.Rows[i][12].ToString();
                                    dr["pflag"] = "0";
                                }
                                dr["opname"] = model[0].dt1.Rows[i][14].ToString();
                                dr["opcode"] = model[0].dt1.Rows[i][13].ToString();
                                dr["mcname"] = model[0].dt1.Rows[i][16].ToString();
                                dr["mccode"] = model[0].dt1.Rows[i][15].ToString();
                                dr["mouldname"] = model[0].dt1.Rows[i][18].ToString();
                                dr["mouldcode"] = model[0].dt1.Rows[i][17].ToString();
                                dr["stime"] = model[0].dt1.Rows[i][19].ToString();
                                dr["etime"] = model[0].dt1.Rows[i][20].ToString();
                                dr["iremark"] = model[0].dt1.Rows[i][21].ToString();
                                //======
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
                        }
                        bool Result = sgen.Update_data(userCode, dataTable, model[0].Col10.Trim(), model[0].Col8, isedit);
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
                                TList1 = ddl_shft,
                                TList2 = ddl_dept,
                                SelectedItems1 = new string[] { "" },
                                SelectedItems2 = new string[] { "" },
                                dt1 = (DataTable)sgen.GetSession(MyGuid, "KPD_DT")
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
                                    model = newprod_entry(model);
                                    ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                                }
                                catch (Exception ex) { }
                            }
                        }
                    }
                    catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
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
        #region  item_stage
        public ActionResult item_stage()
        {
            FillMst();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            //ViewBag.vnew = "";
            //ViewBag.vedit = "";
            //ViewBag.vsave = "disabled='disabled'";
            //ViewBag.scripCall = "disableForm();";
            //mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            //MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            //Tmodelmain tm = new Tmodelmain();
            //tm.Col14 = mid_mst.Trim();
            //tm.Col15 = MyGuid.Trim();
            model[0].Col9 = "ITEM STAGE";
            model[0].Col10 = "enx_tab2";
            model[0].Col12 = "INS"; //ITEM STAGE
                                    //model[0].Col13 = "Save";
            model[0].Col11 = "and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            //model[0]
            model[0].LTM1 = new List<Tmodelmain>();
            Tmodelmain tmltm1 = new Tmodelmain();
            tmltm1.Col1 = "1";
            model[0].LTM1.Add(tmltm1);
            model[0].LTM1[0].TList1 = mod1;
            TempData[MyGuid + "_TList1"] = mod1;
            model[0].LTM1[0].SelectedItems1 = new string[] { "" };
            //model.Add(tm);
            return View(model);
        }
        public List<Tmodelmain> newitem_stage(List<Tmodelmain> model)
        {
            model = getnew(model);
            return model;
        }
        [HttpPost]
        public ActionResult item_stage(List<Tmodelmain> model, string command, string hfrow)
        {
            try
            {
                FillMst(model[0].Col15);
                var tm = model.FirstOrDefault();
                List<SelectListItem> mod1 = new List<SelectListItem>();
                mod1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
                Tmodelmain tmltm1 = new Tmodelmain();
                tmltm1.TList1 = mod1;
                TempData[MyGuid + "_TList1"] = mod1;
                foreach (var mod in model) { foreach (var modl in mod.LTM1) { modl.TList1 = mod1; } }
                if (tmltm1.SelectedItems1 == null) tmltm1.SelectedItems1 = new string[] { "" };
                if (command == "New")
                {
                    try
                    {
                        model = getnew(model);
                        //Session["EDMODE"] = "N";
                        //ViewBag.vnew = "disabled='disabled'";
                        //ViewBag.vedit = "disabled='disabled'";
                        //ViewBag.vsave = "";
                        //ViewBag.scripCall = "enableForm();";
                        //mq1 = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + "  where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                        //vch_num = sgen.genNo(userCode, mq1, 6, "auto_genid");
                        //model[0].Col16 = vch_num;
                        mod1 = new List<SelectListItem>();
                        DataTable dt = new DataTable();
                        #region  Stage
                        mod1 = cmd_Fun.prodstage(userCode, unitid_mst);
                        //dt = sgen.getdata(userCode, "Select master_id,client_name from master_setting where type='KPS'and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
                        //if (dt.Rows.Count > 0)
                        //{
                        //    foreach (DataRow dr in dt.Rows)
                        //    {
                        //        mod1.Add(new SelectListItem { Text = dr["client_name"].ToString(), Value = dr["master_id"].ToString() });
                        //    }
                        //    TempData[MyGuid + "_Tlist1"] = mod1;
                        //}
                        #endregion
                        tmltm1.TList1 = mod1;
                        TempData[MyGuid + "_Tlist1"] = mod1;
                        tmltm1.SelectedItems1 = new string[] { "" };
                        tmltm1.Col1 = "1";
                        model[0].LTM1 = new List<Tmodelmain>();
                        model[0].LTM1.Add(tmltm1);
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
                    //if (Session["btnval"] != null) btnval = Session["btnval"].ToString();
                    //model = CallbackFun(btnval, "N", "item_stage", "Production", model);
                    //ViewBag.vnew = "disabled='disabled'";
                    //ViewBag.vedit = "disabled='disabled'";
                    //ViewBag.vsave = "";
                }
                else if (command == "Add")
                {
                    try
                    {
                        mod1 = (List<SelectListItem>)TempData[MyGuid + "_Tlist1"];
                        TempData[MyGuid + "_TList1"] = mod1;
                        Tmodelmain ntm = new Tmodelmain();
                        ntm.Col1 = (Convert.ToInt32(model[0].LTM1.Max(x => sgen.Make_int(x.Col1))) + 1).ToString();
                        ntm.TList1 = mod1;
                        ntm.SelectedItems1 = new string[] { "" };
                        model[0].LTM1.Add(ntm);
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                    }
                    catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "',0);"; }
                }
                else if (command == "Remove")
                {
                    model[0].LTM1.RemoveAt(sgen.Make_int(hfrow));
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
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
                            mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                            vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                        }
                        //DataTable dataTable = sgen.getdata(userCode, "select * from " + model[0].Col10.Trim() + "  where  1=2");
                        DataTable dataTable = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                        for (int j = 0; j < model[0].LTM1.Count; j++)
                        {
                            DataRow dr = dataTable.NewRow();
                            dr["rec_id"] = "0";
                            dr["vch_num"] = vch_num;
                            dr["type"] = model[0].Col12;
                            dr["type_desc"] = "ItemStage";
                            dr["col1"] = model[0].Col17;
                            dr["col4"] = model[0].Col19;
                            dr["col2"] = model[0].LTM1[j].SelectedItems1.FirstOrDefault();
                            dr["col3"] = model[0].LTM1[j].Col20;
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
                                LTM1 = new List<Tmodelmain>() { new Tmodelmain {
                                TList1=mod1,
                                SelectedItems1=new string[]{ "" }
                            } }
                            });
                        }
                    }
                    catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
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
        #region prod1
        public ActionResult prod1()
        {
            FillMst();
            //if (userCode.Equals("")) { return Redirect(sgenFun.callbackurl); }
            //if (Request.UrlReferrer == null) { return Redirect(sgenFun.callbackurl); }
            //ViewBag.vnew = "";
            //ViewBag.vedit = "";
            //ViewBag.vsave = "disabled='disabled'";
            //ViewBag.scripCall = "disableForm();";
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            List<SelectListItem> ddl_shft = new List<SelectListItem>();
            List<SelectListItem> ddl_dept = new List<SelectListItem>();
            List<SelectListItem> ddl_opr = new List<SelectListItem>();
            List<SelectListItem> ddl_machine = new List<SelectListItem>();
            List<SelectListItem> ddl_mould = new List<SelectListItem>();
            List<SelectListItem> ddl_stage = new List<SelectListItem>();
            //Tmodelmain tm1 = new Tmodelmain();
            //mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            //MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            model[0].Col9 = "PRODUCTION ENTRY"; //heading
            model[0].Col10 = "iprod";
            model[0].Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            model[0].Col12 = "86"; // type
                                   //model[0].Col13 = "Save";
            model[0].Col14 = mid_mst.Trim();
            model[0].Col15 = MyGuid.Trim();
            model[0].dt1 = sgen.getdata(userCode, "select '1' as  SNo,'-' as Icode,'-' as IName,'0' Qty,'-' Remark from dual");
            sgen.SetSession(MyGuid, "KPD_DT1", model[0].dt1);
            model[0].dt2 = sgen.getdata(userCode, "select '1' as  SNo,'-' as Icode,'-' as IName,'0' Qty,'-' Remark from dual");
            sgen.SetSession(MyGuid, "KPD_DT2", model[0].dt2);
            model[0].TList1 = ddl_shft;
            model[0].TList2 = ddl_dept;
            model[0].TList3 = ddl_opr;
            model[0].TList4 = ddl_machine;
            model[0].TList5 = ddl_mould;
            model[0].TList6 = ddl_stage;
            model[0].SelectedItems1 = new string[] { "" };
            model[0].SelectedItems2 = new string[] { "" };
            model[0].SelectedItems3 = new string[] { "" };
            model[0].SelectedItems4 = new string[] { "" };
            model[0].SelectedItems5 = new string[] { "" };
            model[0].SelectedItems6 = new string[] { "" };
            //model.Add(tm1);
            ModelState.Clear();
            return View(model);
        }
        public List<Tmodelmain> newprod1(List<Tmodelmain> model)
        {
            model = getnew(model);
            return model;
        }
        [HttpPost]
        public ActionResult prod1(List<Tmodelmain> model, string command, string hfrow, string hftable, string hftable2)
        {
            try
            {
                FillMst(model[0].Col15);
                model[0].dt1 = sgen.settable(hftable);
                model[0].dt2 = sgen.settable(hftable2);
                model[0].Col24 = hfrow;
                var tm = model.FirstOrDefault();
                DataTable dt = new DataTable();
                List<SelectListItem> ddl_shft = new List<SelectListItem>();
                List<SelectListItem> ddl_dept = new List<SelectListItem>();
                List<SelectListItem> ddl_opr = new List<SelectListItem>();
                List<SelectListItem> ddl_machine = new List<SelectListItem>();
                List<SelectListItem> ddl_mould = new List<SelectListItem>();
                List<SelectListItem> ddl_stage = new List<SelectListItem>();
                model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
                model[0].TList2 = (List<SelectListItem>)TempData[MyGuid + "_TList2"];
                model[0].TList3 = (List<SelectListItem>)TempData[MyGuid + "_TList3"];
                model[0].TList4 = (List<SelectListItem>)TempData[MyGuid + "_TList4"];
                model[0].TList5 = (List<SelectListItem>)TempData[MyGuid + "_TList5"];
                model[0].TList6 = (List<SelectListItem>)TempData[MyGuid + "_TList6"];
                TempData[MyGuid + "_TList1"] = model[0].TList1;
                TempData[MyGuid + "_TList2"] = model[0].TList2;
                TempData[MyGuid + "_TList3"] = model[0].TList3;
                TempData[MyGuid + "_TList4"] = model[0].TList4;
                TempData[MyGuid + "_TList5"] = model[0].TList5;
                TempData[MyGuid + "_TList6"] = model[0].TList6;
                if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
                if (tm.SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
                if (tm.SelectedItems3 == null) model[0].SelectedItems3 = new string[] { "" };
                if (tm.SelectedItems4 == null) model[0].SelectedItems4 = new string[] { "" };
                if (tm.SelectedItems5 == null) model[0].SelectedItems5 = new string[] { "" };
                if (tm.SelectedItems6 == null) model[0].SelectedItems6 = new string[] { "" };
                if (command == "New")
                {
                    try
                    {
                        model = getnew(model);
                        //Session["EDMODE"] = "N";
                        //ViewBag.vnew = "disabled='disabled'";
                        //ViewBag.vedit = "disabled='disabled'";
                        //ViewBag.vsave = "";
                        //ViewBag.scripCall = "enableForm();";
                        //mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                        //vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                        model[0].Col17 = "1";
                        model[0].Col20 = vch_num;
                        model[0].Col13 = "Save";
                        model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "KPD_DT1");
                        model[0].dt2 = (DataTable)sgen.GetSession(MyGuid, "KPD_DT2");
                        #region getshft
                        ddl_shft = cmd_Fun.getshft(userCode, unitid_mst);
                        //dt = sgen.getdata(userCode, "SELECT master_id,master_name FROM master_setting WHERE Type='STM'" + model[0].Col11.Trim() + "");
                        //if (dt.Rows.Count > 0)
                        //{
                        //    foreach (DataRow dr in dt.Rows)
                        //    {
                        //        ddl_shft.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                        //    }
                        //}
                        TempData[MyGuid + "_Tlist1"] = ddl_shft;
                        #endregion
                        #region getdept
                        //dt = sgen.getdata(userCode, "SELECT master_id,master_name FROM master_setting WHERE Type='MDP'");
                        //if (dt.Rows.Count > 0)
                        //{
                        //    foreach (DataRow dr in dt.Rows)
                        //    {
                        //        ddl_dept.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                        //    }
                        //}
                        ddl_dept = cmd_Fun.dept(userCode, clientid_mst, unitid_mst);
                        TempData[MyGuid + "_Tlist2"] = ddl_dept;
                        #endregion
                        #region getOperator
                        //   dt = sgen.getdata(userCode, "select master_id,master_name,'' partno,'' uom,'' hsn,'' taxrate,'' stock from master_Setting " +
                        //"where type='OPR' and  client_unit_id='" + unitid_mst + "'");
                        //   if (dt.Rows.Count > 0)
                        //   {
                        //       foreach (DataRow dr in dt.Rows)
                        //       {
                        //           ddl_opr.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                        //       }
                        //   }
                        ddl_opr = cmd_Fun.opname(userCode, unitid_mst);
                        TempData[MyGuid + "_Tlist3"] = ddl_opr;
                        #endregion
                        #region getMachine
                        //    dt = sgen.getdata(userCode, "select vch_num master_id,col31 master_name,'' partno,'' uom,'' hsn,'' taxrate,'' stock from kc_tab " +
                        //"where type='PMM' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
                        //    if (dt.Rows.Count > 0)
                        //    {
                        //        foreach (DataRow dr in dt.Rows)
                        //        {
                        //            ddl_machine.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                        //        }
                        //    }
                        ddl_machine = cmd_Fun.mcname(userCode, unitid_mst);
                        TempData[MyGuid + "_Tlist4"] = ddl_machine;
                        #endregion
                        #region getMould
                        //    dt = sgen.getdata(userCode, "select vch_num master_id,col31 master_name,'' partno,'' uom,'' hsn,'' taxrate,'' stock from kc_tab " +
                        //"where type='MOS' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
                        //    if (dt.Rows.Count > 0)
                        //    {
                        //        foreach (DataRow dr in dt.Rows)
                        //        {
                        //            ddl_mould.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                        //        }
                        //    }
                        ddl_mould = cmd_Fun.mldname(userCode, unitid_mst);
                        TempData[MyGuid + "_Tlist5"] = ddl_mould;
                        #endregion
                        #region getSatage
                        //     dt = sgen.getdata(userCode, "select master_id master_id,client_name master_name,'' partno,'' uom,'' hsn,'' taxrate,'' stock from master_Setting " +
                        //"where type='KPS' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
                        //     if (dt.Rows.Count > 0)
                        //     {
                        //         foreach (DataRow dr in dt.Rows)
                        //         {
                        //             ddl_stage.Add(new SelectListItem { Text = dr["master_name"].ToString(), Value = dr["master_id"].ToString() });
                        //         }
                        //     }
                        ddl_stage = cmd_Fun.prodstage(userCode, unitid_mst);
                        TempData[MyGuid + "_Tlist6"] = ddl_stage;
                        #endregion
                        model[0].TList1 = ddl_shft;
                        model[0].TList2 = ddl_dept;
                        model[0].TList3 = ddl_opr;
                        model[0].TList4 = ddl_machine;
                        model[0].TList5 = ddl_mould;
                        model[0].TList6 = ddl_stage;
                        model[0].SelectedItems1 = new string[] { "" };
                        model[0].SelectedItems2 = new string[] { "" };
                        model[0].SelectedItems3 = new string[] { "" };
                        model[0].SelectedItems4 = new string[] { "" };
                        model[0].SelectedItems5 = new string[] { "" };
                        model[0].SelectedItems6 = new string[] { "" };
                        model[0].SelectedItems7 = new string[] { "" };
                    }
                    catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "',0);"; }
                }
                if (command == "Cancel")
                {
                    return CancelFun(model);
                }
                else if (command == "Callback")
                {
                    model = StartCallback(model);
                    //if (Session["btnval"] != null) btnval = Session["btnval"].ToString();
                    //model = CallbackFun(btnval, "N", "prod1", "Production", model);
                    //ViewBag.vnew = "disabled='disabled'";
                    //ViewBag.vedit = "disabled='disabled'";
                    //ViewBag.vsave = "";
                }
                else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
                {
                    try
                    {
                        string currdate = sgen.server_datetime(userCode);
                        if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                        {
                            isedit = true;
                            vch_num = model[0].Col20;
                        }
                        else
                        {
                            isedit = false;
                            mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                            vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                            model[0].Col20 = vch_num;
                        }
                        //DataTable dataTable = sgen.getdata(userCode, "select  * from " + model[0].Col10.Trim() + "  where  1=2");
                        DataTable dataTable = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                        string pflag = "";
                        DataTable dtf = new DataTable();
                        int k = 0;
                        if (k == 0) { dtf = model[0].dt2; }
                        else { dtf = model[0].dt1; }
                        do
                        {
                            for (int i = 0; i < dtf.Rows.Count; i++)
                            {
                                DataRow dr = dataTable.NewRow();
                                dr["rec_id"] = "0";
                                dr["type"] = model[0].Col12;
                                dr["vch_num"] = vch_num.Trim();
                                dr["vch_date"] = sgen.Savedate(model[0].Col21, true);
                                dr["shftno"] = model[0].SelectedItems1.FirstOrDefault();
                                dr["shftname"] = model[0].TList1.SingleOrDefault(w => w.Value == model[0].SelectedItems1.FirstOrDefault()).Text;
                                dr["deptno"] = model[0].SelectedItems2.FirstOrDefault();
                                dr["deptname"] = model[0].TList2.SingleOrDefault(w => w.Value == model[0].SelectedItems2.FirstOrDefault()).Text;
                                dr["opname"] = model[0].TList3.SingleOrDefault(w => w.Value == model[0].SelectedItems3.FirstOrDefault()).Text;
                                dr["opcode"] = model[0].SelectedItems3.FirstOrDefault();
                                dr["mcname"] = model[0].TList4.SingleOrDefault(w => w.Value == model[0].SelectedItems4.FirstOrDefault()).Text;
                                dr["mccode"] = model[0].SelectedItems4.FirstOrDefault();
                                dr["mouldname"] = model[0].TList5.SingleOrDefault(w => w.Value == model[0].SelectedItems5.FirstOrDefault()).Text;
                                dr["mouldcode"] = model[0].SelectedItems5.FirstOrDefault();
                                dr["stime"] = model[0].Col23;
                                dr["etime"] = model[0].Col24;
                                dr["totremark"] = model[0].Col22;
                                dr["qtyplanned"] = model[0].Col25;
                                dr["rno"] = dtf.Rows[i][0].ToString();
                                if (k == 0) //Consume
                                {
                                    dr["icode"] = dtf.Rows[i][1].ToString();
                                    dr["iname"] = dtf.Rows[i][2].ToString();
                                    dr["stage"] = model[0].SelectedItems6.FirstOrDefault();
                                    dr["qtyout"] = dtf.Rows[i][3].ToString();
                                    dr["pflag"] = k.ToString();
                                }
                                else //Produce
                                {
                                    dr["icode"] = dtf.Rows[i][1].ToString();
                                    dr["iname"] = dtf.Rows[i][2].ToString();
                                    dr["stage"] = model[0].SelectedItems7.FirstOrDefault();
                                    dr["qtyin"] = dtf.Rows[i][3].ToString();
                                    dr["pflag"] = k.ToString();
                                }
                                dr["iremark"] = dtf.Rows[i][4].ToString();
                                //======
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
                            k++;
                        }
                        while (k < 2);
                        bool Result = sgen.Update_data(userCode, dataTable, model[0].Col10.Trim(), model[0].Col8, isedit);
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
                                Col13 = "Save",
                                Col14 = tm.Col14,
                                Col15 = tm.Col15,
                                TList1 = ddl_shft,
                                TList2 = ddl_dept,
                                SelectedItems1 = new string[] { "" },
                                SelectedItems2 = new string[] { "" },
                                dt1 = (DataTable)sgen.GetSession(MyGuid, "KPD_DT")
                            });
                        }
                    }
                    catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
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
        #region  bom
        public ActionResult bom()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "BOM";
            model[0].Col10 = "itransactionc";
            model[0].Col12 = "BOM"; // BOM       
            model[0].Col11 = " and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            DataTable dt = sgen.getdata(userCode, "select '0' as  SNo ,'-' as Icode,'-' as IName,'-' as PUOM,'-' as req_qty,'-' as alt_qty ,'' stg_code,'' as Stage,'' Mld_code,'' Mld from dual");
            model[0].dt1 = dt;
            sgen.SetSession(MyGuid, "dtbom", dt);
            return View(model);
        }
        public List<Tmodelmain> new_bom(List<Tmodelmain> model)
        {
            model = getnew(model);
            model[0].Col17 = sgen.server_datetime_local(userCode);
            model[0].Col22 = "1";
            return model;
        }
        [HttpPost]
        public ActionResult bom(List<Tmodelmain> model, string command, string hfrow, string hftable)
        {
            try
            {
                FillMst(model[0].Col15);
                DataTable dtm = sgen.settable(hftable);
                model[0].dt1 = dtm;
                var tm = model.FirstOrDefault();
                command = command.Trim();
                if (command == "New")
                {
                    try
                    {
                        model = new_bom(model);
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
                    model = CallbackFun(btnval, "N", actionName, controllerName, model);
                    if (btnval.ToUpper().Equals("VIEW"))
                    {
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "disabled='disabled'";
                        ViewBag.vsavenew = "disabled='disabled'";
                        ViewBag.scripCall += "disableForm();";
                    }
                    else
                    {
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                    }
                }
                else if (command == "btnmain")
                {
                    if (model[0].Col18 != null && model[0].Col18 != "")
                    {
                        mq = sgen.seekval(userCode, "select acode from itransactionc where type='" + model[0].Col12 + "'" + model[0].Col11 + " and acode='" + model[0].Col18 + "'", "acode");
                        if (mq.Trim() != "0")
                        {
                            ViewBag.scripCall += "showmsgJS(1,'Data already exist for this acode', 2);";
                        }
                        else
                        {
                            sgen.SetSession(MyGuid, "SSEEKVAL", clientid_mst + unitid_mst + model[0].Col18);
                            CallbackFun("ITEM", "", actionName, controllerName, model);
                        }
                    }
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vsavenew = "";
                }
                else if (command == "btnicode")
                {
                    if (model[0].Col18 != null && model[0].Col18 != "")
                    {
                        model[0].Col30 = "Y";
                        sgen.SetSession(MyGuid, "SSEEKVAL", clientid_mst + unitid_mst + model[0].dt1.Rows[sgen.Make_int(hfrow)][1].ToString());
                        CallbackFun("BOMITEM", "", actionName, controllerName, model);
                    }
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
                            mq1 = " and vch_num<>'" + model[0].Col16 + "'";
                            isedit = true;
                            vch_num = model[0].Col16;
                        }
                        else
                        {
                            mq1 = "";
                            isedit = false;
                            mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                            vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                            model[0].Col16 = vch_num;
                        }
                        mq = "select acode from itransactionc where type='" + model[0].Col12 + "'" + model[0].Col11 + " and acode='" + model[0].Col19 + "'" + mq1 + "";
                        cond = sgen.seekval(userCode, mq, "acode");
                        if (cond != "0")
                        {
                            ViewBag.vnew = "disabled='disabled'";
                            ViewBag.vedit = "disabled='disabled'";
                            ViewBag.vsave = "";
                            ViewBag.vsavenew = "";
                            ViewBag.scripCall += "showmsgJS(1,'Data already exist for this acode', 2);";
                        }
                        DataTable dataTable = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                        for (int i = 0; i < model[0].dt1.Rows.Count; i++)
                        {
                            DataRow dr = dataTable.NewRow();
                            dr["type"] = model[0].Col12;
                            dr["vch_num"] = vch_num.Trim();
                            dr["vch_date"] = sgen.Savedate(model[0].Col17, true);
                            dr["acode"] = model[0].Col19;//main icode
                                                         //dt====
                            dr["rno"] = model[0].dt1.Rows[i][0].ToString().Trim();
                            dr["icode"] = model[0].dt1.Rows[i][1].ToString();
                            dr["iname"] = model[0].dt1.Rows[i][2].ToString();
                            dr["uom"] = model[0].dt1.Rows[i][3].ToString();//PUOM
                            dr["qtyin"] = model[0].dt1.Rows[i][4].ToString();//req qty
                            dr["qtyout"] = model[0].dt1.Rows[i][5].ToString();//alt qty                  
                            dr["pono"] = model[0].dt1.Rows[i][6].ToString();//stg code
                            dr["cond"] = model[0].dt1.Rows[i][7].ToString();//stage                       
                            dr["tmode"] = model[0].dt1.Rows[i][8].ToString();//mld code
                            dr["req_by"] = model[0].dt1.Rows[i][9].ToString();//mld                       
                                                                              //======
                            dr["irate"] = model[0].Col21;//bom lot size  
                            dr = getsave(currdate, dr, model, isedit);
                            dataTable.Rows.Add(dr);
                        }
                        bool Result = sgen.Update_data(userCode, dataTable, model[0].Col10.Trim(), tmodel.Col8, isedit);
                        if (Result == true)
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
                                dt1 = (DataTable)sgen.GetSession(MyGuid, "dtbom"),
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
                                    model = new_bom(model);
                                    ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                                }
                                catch (Exception ex) { }
                            }
                        }
                    }
                    catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
                }
                else if (command == "-")
                {
                    if (model[0].dt1.Rows.Count > 1) model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                    else model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "dtbom");
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
        public ActionResult Getmld(string searchTerm, int pageSize, int pageNum, string icode, string Myguid)
        {
            FillMst(Myguid);
            DataSet ds = getitemData(userCode, "select vch_num as icode,col31 as iname ,'-' partno,'-' uom,'-' hsn,'-' taxrate,'-' stock from kc_tab " +
                "where type='MOS' and client_unit_id='" + unitid_mst + "' ", 1, searchTerm);
            return DstoJSonItems(ds);
        }

        #endregion
        #region production order
        public ActionResult p_ord()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            DataTable dt = new DataTable();
            model[0].Col10 = "kc_tab";//for out
            model[0].Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            model[0].Col12 = "PSO"; //production order
            dt = sgen.getdata(userCode, "select '' as Item, '1' as SNo ,'-' as Icode,'-' as IName,'-' PartNo,'-' as UOM,'0' as Order_qty,'0' as Plan_qty," +
                "'-' as prd_start_dt,'-' as prd_end_dt,'0' SO_No,'-' SO_Date,'-' as Remark from dual");
            model[0].dt1 = dt;
            sgen.SetSession(MyGuid, "P_ORD_DT", dt);
            return View(model);
        }
        [HttpPost]
        public ActionResult p_ord(List<Tmodelmain> model, string command, string hfrow, string hftable)
        {
            try
            {
                FillMst(model[0].Col15);
                DataTable dtm = sgen.settable(hftable);
                model[0].dt1 = dtm;
                var tm = model.FirstOrDefault();
                if (command == "Cancel") { return CancelFun(model); }
                else if (command == "Callback") { model = StartCallback(model); }
                #region Fill loc
                //else if (command == "Fill Location")
                //{
                //    if (model[0].dt1.Rows[0]["icode"].ToString().Trim().Equals("-"))
                //    {
                //        ViewBag.scripCall += "showmsgJS(1,'Please Select item in grid-1', 0);";
                //        ViewBag.vnew = "disabled='disabled'";
                //        ViewBag.vedit = "disabled='disabled'";
                //        ViewBag.vsave = "";
                //        ViewBag.vsavenew = "";
                //        return View(model);
                //    }
                //    try
                //    {
                //        if (model[0].dt2 != null && model[0].dt2.Rows[0]["icode"].ToString().Trim().Equals("-")) { model[0].dt2.Rows.RemoveAt(0); }
                //    }
                //    catch (Exception err) { }
                //    var dttt = model[0].dt2.Clone();
                //    model[0].dt2 = dttt;
                //    foreach (DataRow dr1 in model[0].dt1.Rows)
                //    {
                //        try
                //        {
                //            if (model[0].dt1.Rows[0]["icode"].ToString().Trim().Equals("-"))
                //            {
                //                ViewBag.scripCall += "showmsgJS(1,'Please Select item in grid-1', 2);";
                //                ViewBag.vnew = "disabled='disabled'";
                //                ViewBag.vedit = "disabled='disabled'";
                //                ViewBag.vsave = "";
                //                ViewBag.vsavenew = "";
                //                return View(model);
                //            }
                //            try
                //            {
                //                if (model[0].dt2.Rows[0]["icode"].ToString().Trim().Equals("-")) { model[0].dt2.Rows.RemoveAt(0); }
                //            }
                //            catch (Exception err) { }
                //            var dt7 = model[0].dt2.Clone();
                //            model[0].dt2 = dt7;
                //            foreach (DataRow dr3 in model[0].dt1.Rows)
                //            {
                //                decimal qtyout = sgen.Make_decimal(dr3["qty_out"].ToString().Trim());
                //                if (qtyout <= 0)
                //                {
                //                    ViewBag.vnew = "disabled='disabled'";
                //                    ViewBag.vedit = "disabled='disabled'";
                //                    ViewBag.vsave = "";
                //                    ViewBag.vsavenew = "";
                //                    ViewBag.scripCall += "showmsgJS(1,'Please FILL Out Qty in Item " + dr3["iname"].ToString() + "', 2);";
                //                    model[0].dt2 = (DataTable)Session["KNRGP_DT2"];
                //                    break;
                //                }
                //                string icode = dr3["icode"].ToString().Trim();
                //                DataTable dloc = sgen.getdata(userCode, "select itb.loc,location_name(itb.client_id||itb.client_unit_id||itb.loc) from itbal itb " +
                //       "where trim(itb.icode)='" + icode + "' and itb.type='IT' and itb.client_id='" + clientid_mst + "' and itb.client_unit_id='" + unitid_mst + "'");
                //                DataTable dtlocs = sgen.locclosing_all(userCode, unitid_mst + icode, "");
                //                int rcnt = dtlocs.Rows.Count;
                //                if (rcnt == 0)
                //                {
                //                    ViewBag.scripCall += "showmsgJS(1,'Item " + icode + " stock is zero' , 0);";
                //                    return View(model);
                //                }
                //                DataColumn dc = new DataColumn("ordd", typeof(int));
                //                dc.DefaultValue = "0";
                //                dtlocs.Columns.Add(dc);
                //                var rind = sgen.seekval_dt_rowindex(dtlocs, "loc='" + dloc.Rows[0]["loc"].ToString().Trim() + "'");
                //                try
                //                {
                //                    dtlocs.Rows[rind - 1]["ordd"] = rcnt;
                //                }
                //                catch (Exception err) { }
                //                DataView dv = dtlocs.DefaultView;
                //                dv.Sort = "ordd asc";
                //                dtlocs = dv.ToTable();
                //                foreach (DataRow dr in dtlocs.Rows)
                //                {
                //                    if (qtyout <= 0) break;
                //                    DataRow dr2 = model[0].dt2.NewRow();
                //                    dr2["sno"] = unitid_mst + dr3["icode"].ToString().Trim() + dloc.Rows[0][0].ToString().Trim();
                //                    dr2["Icode"] = dr3["icode"].ToString().Trim();
                //                    dr2["INAME"] = dr3["INAME"].ToString().Trim();
                //                    dr2["PARTNO"] = dr3["PARTNO"].ToString().Trim();
                //                    dr2["UOM"] = dr3["UOM"].ToString().Trim();
                //                    dr2["Qty_In_Stock"] = dr["closing"].ToString().Trim();
                //                    decimal lclos = sgen.Make_decimal(dr["closing"].ToString().Trim());
                //                    if (qtyout > lclos)
                //                    {
                //                        dr2["Qty_Out"] = lclos;
                //                        qtyout = qtyout - lclos;
                //                    }
                //                    else
                //                    {
                //                        dr2["Qty_Out"] = qtyout;
                //                        qtyout = 0;
                //                    }
                //                    dr2["Remark"] = "-";
                //                    dr2["LocCode"] = dr["loc"].ToString().Trim();
                //                    dr2["LOC"] = dr["loc_name"].ToString().Trim();
                //                    model[0].dt2.Rows.Add(dr2);
                //                }
                //                ViewBag.vnew = "disabled='disabled'";
                //                ViewBag.vedit = "disabled='disabled'";
                //                ViewBag.vsave = "";
                //                ViewBag.vsavenew = "";
                //            }
                //        }
                //        catch (Exception ex) { ViewBag.scripCall += "showmsgJS(1,'" + ex.Message.ToString().Replace('\'', ' ') + "', 0);"; }
                //    }
                //    if (model[0].dt2.Rows.Count == 0) model[0].dt2 = ((DataTable)Session["KNRGP_DT2"]);
                //    if (model[0].dt1.Rows.Count == 0) model[0].dt1 = ((DataTable)Session["KNRGP_DT1"]);
                //} 
                #endregion
                else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
                {
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
                        //qty_in_stock = k.Field<string>("qty_in_stock"),
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
                    //QTY_Out2 += out2 = sgen.Make_decimal(row["QTY_Out"].ToString());
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
                    //Satransaction sat1 = new Satransaction(userCode,MyGuid);
                    try
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
                            model[0].Col16 = vch_num;
                        }
                        //string ret_date = sgen.Savedate(model[0].Col22, false);
                        dataTable = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                        for (int i = 0; i < model[0].dt1.Rows.Count; i++)
                        {
                            DataRow dr = dataTable.NewRow();
                            dr["vch_num"] = vch_num;
                            dr["type"] = model[0].Col12;
                            dr["vch_date"] = currdate;
                            //dr["col1"] = model[0].Col49;//party code
                            //dr["date1"] = sgen.Savedate(model[0].Col22, false);    // doc date
                            //dr["date1"] = sgen.Make_date_S(model[0].Col22);    // doc date
                            //dr["col2"] = model[0].Col23;//sdl no
                            //dr["col3"] = model[0].Col26;//po no
                            if (model[0].Col40 == "70")
                            {
                                dr["col4"] = model[0].Col29; //so no
                                dr["date7"] = sgen.Make_date_S(model[0].Col31); //so dt
                            }
                            dr["col21"] = model[0].Col28; //remark
                            dr["col20"] = model[0].Col40; //hf po type
                            dr["date2"] = sgen.Make_date_S(model[0].Col24); //sd st date
                            dr["date3"] = sgen.Make_date_S(model[0].Col25); //sd end date
                                                                            //dr["date4"] = sgen.Make_date_S(model[0].Col27); //po dt
                            dr["col5"] = model[0].dt1.Rows[i]["Icode"].ToString();
                            dr["col27"] = model[0].dt1.Rows[i]["IName"].ToString();
                            dr["col7"] = model[0].dt1.Rows[i]["PartNo"].ToString();
                            dr["col8"] = model[0].dt1.Rows[i]["UOM"].ToString();
                            dr["col9"] = model[0].dt1.Rows[i]["Order_qty"].ToString().Trim();
                            dr["col10"] = model[0].dt1.Rows[i]["Plan_qty"].ToString();
                            //dr["col11"] = model[0].dt1.Rows[i]["PO_No"].ToString();
                            if (model[0].Col40 == "70")
                            {
                                dr["col13"] = model[0].dt1.Rows[i]["SO_No"].ToString();
                                dr["date6"] = sgen.Make_date_S(model[0].dt1.Rows[i]["SO_Date"].ToString());
                            }
                            if (model[0].dt1.Rows[i]["prd_start_dt"].ToString() != "")
                            {
                                dr["date8"] = sgen.Make_date_S(model[0].dt1.Rows[i]["prd_start_dt"].ToString());
                                dr["date9"] = sgen.Make_date_S(model[0].dt1.Rows[i]["prd_end_dt"].ToString());
                            }
                            dr["col12"] = model[0].dt1.Rows[i]["Remark"].ToString();
                            // shiv
                            dr = getsave(currdate, dr, model, isedit);
                            dataTable.Rows.Add(dr);
                        }
                        Result = sgen.Update_data(userCode, dataTable, model[0].Col10.Trim(), model[0].Col8, isedit);
                        if (Result == true)
                        {
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
                                dt1 = ((DataTable)sgen.GetSession(MyGuid, "P_ORD_DT")).Copy()
                            });
                        }

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
                                Make_query("P_ord", "Select Production Order Type", "NEW", "1", "", "",MyGuid);
                                ViewBag.scripCall = "callFoo('Select Production Order Type');";
                                ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                            }
                            catch (Exception ex)
                            {
                                ViewBag.scripCall += "mytoast('error', 'toast-top-right', '" + ex.Message.ToString().Trim() + "');";
                            }
                        }
                    }
                    catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
                }
                else if (command == "-")
                {
                    if (model[0].dt1.Rows.Count > 1) model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                    else { model[0].dt1 = ((DataTable)sgen.GetSession(MyGuid, "P_ORD_DT")).Copy(); }
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vsavenew = "";
                }
                //else if (command == "2-")
                //{
                //    if (model[0].dt2.Rows.Count > 1) model[0].dt2.Rows.RemoveAt(sgen.Make_int(hfrow));
                //    else { model[0].dt2 = (DataTable)Session["DIS_SCH_DT"]; }
                //    ViewBag.vnew = "disabled='disabled'";
                //    ViewBag.vedit = "disabled='disabled'";
                //    ViewBag.vsave = "";
                //    ViewBag.vsavenew = "";
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
        #region mrp_gen
        public ActionResult mrp_gen()
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
            Tmodelmain tm1 = new Tmodelmain();
            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = MyGuid.Trim();
            tm1.Col9 = "MRP GENERATION";
            tm1.Col10 = "itransaction";
            tm1.Col12 = "MRP";
            tm1.Col13 = "Save";
            tm1.Col11 = " and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            DataTable dt = sgen.getdata(userCode, "select '0' as  SNo ,'-' as Icode,'-' as IName,'-' as PUOM,'-' as SUOM,'' as CUOM,'0' as Qty_Req,'0' as Qty_Rej from dual");
            tm1.dt1 = dt;
            sgen.SetSession(MyGuid, "dtmrp", dt);
            model.Add(tm1);
            return View(model);
        }
        public List<Tmodelmain> newmmrp_gen(List<Tmodelmain> model)
        {
            model = getnew(model);
            return model;
        }
        [HttpPost]
        public ActionResult mrp_gen(List<Tmodelmain> model, string command, string hfrow, string hftable)
        {
            try
            {
                FillMst(model[0].Col15);
                DataTable dtm = sgen.settable(hftable);
                model[0].dt1 = dtm;
                var tm = model.FirstOrDefault();
                if (command == "New")
                {
                    try
                    {
                        model = getnew(model);
                        //Session["EDMODE"] = "N";
                        //ViewBag.vnew = "disabled='disabled'";
                        //ViewBag.vedit = "disabled='disabled'";
                        //ViewBag.vsave = "";
                        //ViewBag.scripCall = "enableForm();";
                        //mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                        //vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                        //model[0].Col16 = vch_num;
                        model[0].Col17 = sgen.server_datetime_local(userCode);
                        model[0].Col13 = "Save";
                        model[0].Col22 = "1";
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
                    //if (Session["btnval"] != null) btnval = Session["btnval"].ToString();
                    //model = CallbackFun(btnval, "N", "mrp_gen", "Production", model);
                    //ViewBag.vnew = "disabled='disabled'";
                    //ViewBag.vedit = "disabled='disabled'";
                    //ViewBag.vsave = "";
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
                            mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                            vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                            model[0].Col16 = vch_num;
                        }
                        //DataTable dataTable = sgen.getdata(userCode, "select * from " + model[0].Col10.Trim() + "  where  1=2");
                        DataTable dataTable = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                        for (int i = 0; i < model[0].dt1.Rows.Count; i++)
                        {
                            DataRow dr = dataTable.NewRow();
                            dr["rec_id"] = "0";
                            dr["type"] = model[0].Col12;
                            dr["vch_num"] = vch_num.Trim();
                            dr["vch_date"] = sgen.Savedate(model[0].Col17, true);
                            dr["acode"] = model[0].Col19;//main icode
                                                         //dt====
                            dr["icode"] = model[0].dt1.Rows[i][1].ToString();
                            dr["iname"] = model[0].dt1.Rows[i][2].ToString();
                            dr["uom"] = model[0].dt1.Rows[i][3].ToString();//PUOM
                            dr["cpartno"] = model[0].dt1.Rows[i][4].ToString();//SUOM
                            dr["hsno"] = model[0].dt1.Rows[i][5].ToString();//CUOM                        
                            dr["qtychl"] = model[0].dt1.Rows[i][6].ToString();//qty_req
                            dr["qtyin"] = model[0].dt1.Rows[i][7].ToString();//qty_rej                        
                                                                             //======
                            dr["irate"] = model[0].Col21;//bom lot size                        
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
                        if (Result == true)
                        {
                            ViewBag.vnew = "";
                            ViewBag.vedit = "";
                            ViewBag.vsave = "disabled='disabled'";
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
                                dt1 = (DataTable)sgen.GetSession(MyGuid, "dtbom")
                            });
                            //ViewBag.scripCall = "showmsgJS(1, 'Record Saved Successfully', 1);disableForm();";
                            ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                        }
                    }
                    catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
                }
                else if (command == "-")
                {
                    if (model[0].dt1.Rows.Count > 1) model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                    else model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "dtbom");
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
        #region oh_master
        public ActionResult oh_mst()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "OVERHEAD MASTER";
            model[0].Col10 = "itransaction";
            model[0].Col12 = "MRP";
            model[0].Col11 = " and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            DataTable dt = sgen.getdata(userCode, "select '0' as  SNo ,'-' as Icode,'-' as IName,'-' as PUOM,'-' as SUOM,'' as CUOM,'0' as Qty_Req,'0' as Qty_Rej from dual");
            model[0].dt1 = dt;
            sgen.SetSession(MyGuid, "dtmrp", dt);
            return View(model);
        }
        public List<Tmodelmain> newoh_mst(List<Tmodelmain> model)
        {
            model = getnew(model);
            //model[0].Col17 = sgen.server_datetime_local(userCode);
            //model[0].Col22 = "1";
            return model;
        }
        [HttpPost]
        public ActionResult oh_mst(List<Tmodelmain> model, string command, string hfrow, string hftable)
        {
            try
            {
                FillMst(model[0].Col15);
                DataTable dtm = sgen.settable(hftable);
                model[0].dt1 = dtm;
                var tm = model.FirstOrDefault();
                if (command == "New")
                {
                    try
                    {
                        model = newoh_mst(model);
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
                    try
                    {
                        var tmodel = model.FirstOrDefault();
                        string currdate = sgen.server_datetime(userCode);
                        //model[0].Col17 = sgen.server_datetime_local(userCode);
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
                            dr["rec_id"] = "0";
                            dr["type"] = model[0].Col12;
                            dr["vch_num"] = vch_num.Trim();
                            dr["vch_date"] = sgen.Savedate(model[0].Col17, true);
                            dr["acode"] = model[0].Col19;//main icode
                                                         //dt====
                            dr["icode"] = model[0].dt1.Rows[i][1].ToString();
                            dr["iname"] = model[0].dt1.Rows[i][2].ToString();
                            dr["uom"] = model[0].dt1.Rows[i][3].ToString();//PUOM
                            dr["cpartno"] = model[0].dt1.Rows[i][4].ToString();//SUOM
                            dr["hsno"] = model[0].dt1.Rows[i][5].ToString();//CUOM                        
                            dr["qtychl"] = model[0].dt1.Rows[i][6].ToString();//qty_req
                            dr["qtyin"] = model[0].dt1.Rows[i][7].ToString();//qty_rej                        
                                                                             //======
                            dr["irate"] = model[0].Col21;//bom lot size                        
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
                        if (Result == true)
                        { 
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
                                Col100 = "Save & New",
                                Col121 = "S",
                                Col122 = "<u>S</u>ave",
                                Col123 = "Save & Ne<u>w</u>",
                                dt1 = (DataTable)sgen.GetSession(MyGuid, "dtbom")
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
                                    model = newoh_mst(model);
                                    ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                                }
                                catch (Exception ex) { }
                            }
                        }
                    }
                    catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
                }
                else if (command == "-")
                {
                    if (model[0].dt1.Rows.Count > 1) model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                    else model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "dtbom");
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
        #region Test Template
        public ActionResult t_temp()
        {
            FillMst();
            ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.vsave = "disabled='disabled'";
            ViewBag.scripCall = "disableForm();";
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            List<Tmodelmain> model = new List<Tmodelmain>();
            Tmodelmain tm1 = new Tmodelmain();
            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = MyGuid.Trim();
            tm1.Col9 = "TEST TEMPLATE";
            tm1.Col10 = "itransaction";
            tm1.Col12 = "MRP";
            tm1.Col13 = "Save";
            tm1.Col11 = " and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            DataTable dt = sgen.getdata(userCode, "select '0' as  SNo ,'-' as Icode,'-' as IName,'-' as PUOM,'-' as SUOM,'' as CUOM,'0' as Qty_Req,'0' as Qty_Rej from dual");
            tm1.dt1 = dt;
            sgen.SetSession(MyGuid, "dtmrp", dt);
            model.Add(tm1);
            return View(model);
        }
        public List<Tmodelmain> newt_temp(List<Tmodelmain> model)
        {
            model = getnew(model);
            model[0].Col17 = sgen.server_datetime_local(userCode);
            model[0].Col13 = "Save";
            model[0].Col22 = "1";
            return model;
        }
        [HttpPost]
        public ActionResult t_temp(List<Tmodelmain> model, string command, string hfrow, string hftable)
        {
            try
            {
                FillMst(model[0].Col15);
                DataTable dtm = sgen.settable(hftable);
                model[0].dt1 = dtm;
                var tm = model.FirstOrDefault();
                if (command == "New")
                {
                    try
                    {
                        model = newoh_mst(model);
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
                            mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                            vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                            model[0].Col16 = vch_num;
                        }
                        DataTable dataTable = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                        for (int i = 0; i < model[0].dt1.Rows.Count; i++)
                        {
                            DataRow dr = dataTable.NewRow();
                            dr["rec_id"] = "0";
                            dr["type"] = model[0].Col12;
                            dr["vch_num"] = vch_num.Trim();
                            dr["vch_date"] = sgen.Savedate(model[0].Col17, true);
                            dr["acode"] = model[0].Col19;//main icode
                                                         //dt====
                            dr["icode"] = model[0].dt1.Rows[i][1].ToString();
                            dr["iname"] = model[0].dt1.Rows[i][2].ToString();
                            dr["uom"] = model[0].dt1.Rows[i][3].ToString();//PUOM
                            dr["cpartno"] = model[0].dt1.Rows[i][4].ToString();//SUOM
                            dr["hsno"] = model[0].dt1.Rows[i][5].ToString();//CUOM                        
                            dr["qtychl"] = model[0].dt1.Rows[i][6].ToString();//qty_req
                            dr["qtyin"] = model[0].dt1.Rows[i][7].ToString();//qty_rej                        
                                                                             //======
                            dr["irate"] = model[0].Col21;//bom lot size                        
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
                        if (Result == true)
                        {
                            ViewBag.vnew = "";
                            ViewBag.vedit = "";
                            ViewBag.vsave = "disabled='disabled'";
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
                                dt1 = (DataTable)sgen.GetSession(MyGuid, "dtbom")
                            });
                            ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                        }
                    }
                    catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
                }
                else if (command == "-")
                {
                    if (model[0].dt1.Rows.Count > 1) model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                    else model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "dtbom");
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
        #region iprod
        public ActionResult iprod()
        {
            FillMst();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "";
            model[0].Col10 = "iprod";
            //model[0].Col12 = "100"; // pd      
            model[0].Col11 = " and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            DataTable dt = sgen.getdata(userCode, "select '0' sno ,'-' icode,'-' iname,'-' uom,'0' qtystk,'-' stgcode,'-' stg,'0' est_consume_qty,'0' act_consume_qty,'0' short_excess,'-' Alternate from dual");
            model[0].dt1 = dt;
            sgen.SetSession(MyGuid, "dtipd", dt);
            //model[0].dt2 = null;
            //Session["dtipd2"] = null;
            //model[0].TList1 = mod1;
            model[0].TList2 = mod1;
            model[0].TList3 = mod1;
            model[0].TList4 = mod1;
            model[0].TList5 = mod1;
            model[0].TList6 = mod1;
            model[0].TList7 = mod1;
            //model[0].SelectedItems1 = new string[] { "" };
            model[0].SelectedItems2 = new string[] { "" };
            model[0].SelectedItems3 = new string[] { "" };
            model[0].SelectedItems4 = new string[] { "" };
            model[0].SelectedItems5 = new string[] { "" };
            model[0].SelectedItems6 = new string[] { "" };
            model[0].SelectedItems7 = new string[] { "" };
            model[0].Col99 = "N";//do not check stk calc
            ViewBag.vgetdt = "disabled='disabled'";
            ViewBag.vchngdt = "disabled='disabled'";
            sgen.SetSession(MyGuid, "btnval", "");
            return View(model);
        }
        [HttpPost]
        public ActionResult iprod(List<Tmodelmain> model, string command, string hfrow, string hftable)
        {
            try
            {
                FillMst(model[0].Col15);
                DataSet ds = new DataSet();
                if (hftable.Trim() != "")
                {
                    ds = sgen.setDS(hftable);
                    model[0].dt1 = ds.Tables[0];
                }
                var tm = model.FirstOrDefault();
                #region
                //List<SelectListItem> mod1 = new List<SelectListItem>();
                List<SelectListItem> mod2 = new List<SelectListItem>();
                List<SelectListItem> mod3 = new List<SelectListItem>();
                List<SelectListItem> mod4 = new List<SelectListItem>();
                List<SelectListItem> mod5 = new List<SelectListItem>();
                List<SelectListItem> mod6 = new List<SelectListItem>();
                List<SelectListItem> mod7 = new List<SelectListItem>();
                //model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
                model[0].TList2 = (List<SelectListItem>)TempData[MyGuid + "_TList2"];
                model[0].TList3 = (List<SelectListItem>)TempData[MyGuid + "_TList3"];
                model[0].TList4 = (List<SelectListItem>)TempData[MyGuid + "_TList4"];
                model[0].TList5 = (List<SelectListItem>)TempData[MyGuid + "_TList5"];
                model[0].TList6 = (List<SelectListItem>)TempData[MyGuid + "_TList6"];
                model[0].TList7 = (List<SelectListItem>)TempData[MyGuid + "_TList7"];
                //TempData[MyGuid + "_TList1"] = model[0].TList1;
                TempData[MyGuid + "_TList2"] = model[0].TList2;
                TempData[MyGuid + "_TList3"] = model[0].TList3;
                TempData[MyGuid + "_TList4"] = model[0].TList4;
                TempData[MyGuid + "_TList5"] = model[0].TList5;
                TempData[MyGuid + "_TList6"] = model[0].TList6;
                TempData[MyGuid + "_TList7"] = model[0].TList7;
                //if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
                if (tm.SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
                if (tm.SelectedItems3 == null) model[0].SelectedItems3 = new string[] { "" };
                if (tm.SelectedItems4 == null) model[0].SelectedItems4 = new string[] { "" };
                if (tm.SelectedItems5 == null) model[0].SelectedItems5 = new string[] { "" };
                if (tm.SelectedItems6 == null) model[0].SelectedItems6 = new string[] { "" };
                if (tm.SelectedItems7 == null) model[0].SelectedItems7 = new string[] { "" };
                #endregion
                if (command == "New")
                {
                    //try
                    //{
                    //    model = getnew(model);
                    //    model[0].Col17 = sgen.server_datetime_local(userCode);
                    //    //model[0].Col13 = "Save";
                    //    model[0].Col22 = "1";
                    //    model[0].Col101 = "C";// consumption pd
                    //    //mod1 = new List<SelectListItem>();
                    //    mod2 = new List<SelectListItem>();
                    //    mod3 = new List<SelectListItem>();
                    //    mod4 = new List<SelectListItem>();
                    //    mod5 = new List<SelectListItem>();
                    //    mod6 = new List<SelectListItem>();
                    //    //mod7 = new List<SelectListItem>();
                    //    ////from stage
                    //    //mod1 = cmd_Fun.prodstage(userCode, clientid_mst, unitid_mst);
                    //    //TempData[MyGuid + "_TList1"] = mod1;
                    //    ////to stage
                    //    //mod2 = cmd_Fun.prodtostage(userCode, clientid_mst, unitid_mst);
                    //    //TempData[MyGuid + "_TList2"] = mod2;
                    //    //op name
                    //    mod3 = cmd_Fun.opname(userCode, clientid_mst, unitid_mst);
                    //    TempData[MyGuid + "_TList3"] = mod3;
                    //    //machine
                    //    mod4 = cmd_Fun.mcname(userCode, clientid_mst, unitid_mst);
                    //    TempData[MyGuid + "_TList4"] = mod4;
                    //    //mould
                    //    mod5 = cmd_Fun.mldname(userCode, clientid_mst, unitid_mst);
                    //    TempData[MyGuid + "_TList5"] = mod5;
                    //    //shift
                    //    mod6 = cmd_Fun.shftname(userCode, clientid_mst, unitid_mst);
                    //    TempData[MyGuid + "_TList6"] = mod6;
                    //    //loc
                    //    //mod7 = cmd_Fun.wiploc(userCode, clientid_mst, unitid_mst);
                    //    //TempData[MyGuid + "_TList7"] = mod7;
                    //    //model[0].TList1 = mod1;
                    //    //model[0].SelectedItems1 = new string[] { "" };
                    //    model[0].TList2 = mod2;
                    //    model[0].SelectedItems2 = new string[] { "" };
                    //    model[0].TList3 = mod3;
                    //    model[0].SelectedItems3 = new string[] { "" };
                    //    model[0].TList4 = mod4;
                    //    model[0].SelectedItems4 = new string[] { "" };
                    //    model[0].TList5 = mod5;
                    //    model[0].SelectedItems5 = new string[] { "" };
                    //    model[0].TList6 = mod6;
                    //    model[0].SelectedItems6 = new string[] { "" };
                    //    //model[0].TList7 = mod7;
                    //    //model[0].SelectedItems7 = new string[] { "" };
                    //    ViewBag.vgetdt = "";
                    //    ViewBag.vchngdt = "disabled='disabled'";
                    //}
                    //catch (Exception ex) { }
                }
                else if (command == "Cancel")
                {
                    return CancelFun(model);
                }
                else if (command == "Callback")
                {
                    model = StartCallback(model);
                    try
                    {
                        sgen.SetSession(MyGuid, "btnval", Regex.Split(sgen.GetSession(MyGuid, "parent_op").ToString(), "~!~!~!~!")[1].ToString());
                        sgen.SetSession(MyGuid, "opname", null);
                        mod3 = cmd_Fun.opname(userCode, unitid_mst);
                        TempData[MyGuid + "_TList3"] = mod3;
                        model[0].TList3 = mod3;
                        string opid = Regex.Split(sgen.GetSession(MyGuid, "parent_op").ToString(), "~!~!~!~!")[0].ToString();
                        model[0].SelectedItems3 = new string[] { opid };
                    }
                    catch (Exception ex) { }
                    if (sgen.GetSession(MyGuid, "btnval").ToString() == "ITEM")
                    {
                        ViewBag.vchngdt = "disabled='disabled'";
                        ViewBag.vgetdt = "";
                    }
                    else
                    {
                        ViewBag.vchngdt = "";
                        //ViewBag.vgetdt = "disabled='disabled'";
                    }
                }
                else if (command == "Get Data")
                {
                    try
                    {
                        CallbackFun("PDREQ", "N", actionName, controllerName, model);
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                        ViewBag.vgetdt = "disabled='disabled'";
                        ViewBag.vchngdt = "";
                    }
                    catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
                }
                else if (command == "Change Qty")
                {
                    try
                    {
                        model[0].dt1 = ((DataTable)sgen.GetSession(MyGuid, "dtipd")).Copy();
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                        ViewBag.vgetdt = "";
                        ViewBag.vchngdt = "disabled='disabled'";
                    }
                    catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
                }
                else if (command == "ddlmach")
                {
                    mod4 = new List<SelectListItem>();
                    mod7 = new List<SelectListItem>();
                    string mld = model[0].SelectedItems5.FirstOrDefault();
                    //mq = sgen.seekval(userCode, "select col14 mc_caps from kc_tab where type='MOS'" + model[0].Col11 + " and vch_num='" + mld + "'", "mc_caps");
                    //mq = sgen.seekval(userCode, "select c.master_name mc_caps from kc_tab d inner join master_setting c on c.master_id = d.col14 and " +
                    //    "c.type = 'K02' and find_in_set(c.client_id, d.client_id) = 1 and find_in_set(c.client_unit_id, d.client_unit_id) = 1 where " +
                    //    "d.type = 'MOS' and d.client_id = '" + clientid_mst + "' and d.client_unit_id = '" + unitid_mst + "' and d.vch_num = '" + mld + "'", "mc_caps");
                    mq = "select c.master_name mc_caps,group_concat(m.master_name) mcomp from kc_tab d " +
                        "inner join master_setting c on c.master_id = d.col14 and c.type = 'K02' and find_in_set(c.client_unit_id, d.client_unit_id) = 1 " +
                        "inner join master_setting m on find_in_set(m.master_id, d.col34)= 1 and m.type = 'K02' and find_in_set(m.client_unit_id, d.client_unit_id) = 1 " +
                        "where d.client_unit_id = '" + unitid_mst + "' and d.type = 'MOS' and d.vch_num = '" + mld + "' " +
                        "group by d.col14,c.master_name,d.col34";
                    DataTable dtc = sgen.getdata(userCode, mq);
                    if (dtc.Rows.Count > 0)
                    {
                        mq1 = "select a.vch_num,a.col31 mcname,nvl(b.master_id,'-') master_id,nvl(b.master_name,'0') master_name from kc_tab a " +
                            "inner join master_setting b on b.master_id = a.col14 and b.type = 'K02' and find_in_set(b.client_unit_id, a.client_unit_id) = 1 and to_number(nvl(b.master_name,0))>= " + sgen.Make_int(dtc.Rows[0]["mc_caps"].ToString()) + " " +
                            "where a.client_unit_id = '" + unitid_mst + "' and a.type = 'PMM' order by a.vch_num asc";
                        DataTable dt = sgen.getdata(userCode, mq1);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                mod4.Add(new SelectListItem { Text = dr["mcname"].ToString(), Value = dr["vch_num"].ToString() });
                            }
                        }
                        mq1 = "select a.vch_num,a.col31 mcname,nvl(b.master_id,'-') master_id,nvl(b.master_name,'0') master_name from kc_tab a " +
                           "inner join master_setting b on b.master_id = a.col14 and b.type = 'K02' and find_in_set(b.client_unit_id, a.client_unit_id) = 1 and nvl(b.master_name,0) in ('" + dtc.Rows[0]["mcomp"].ToString().Replace(",", "','") + "') " +
                           "where a.client_unit_id = '" + unitid_mst + "' and a.type = 'PMM' order by a.vch_num asc";
                        dt = sgen.getdata(userCode, mq1);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                mod7.Add(new SelectListItem { Text = dr["mcname"].ToString(), Value = dr["vch_num"].ToString() });
                            }
                        }
                    }
                    TempData[MyGuid + "_TList4"] = mod4;
                    model[0].TList4 = mod4;
                    model[0].SelectedItems4 = new string[] { "" };
                    TempData[MyGuid + "_TList7"] = mod7;
                    model[0].TList7 = mod7;
                    model[0].SelectedItems7 = new string[] { "" };
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vsavenew = "";
                    ViewBag.vgetdt = "";
                    ViewBag.vchngdt = "disabled='disabled'";
                }
                else if (command == "ddlmld")
                {
                    mod5 = new List<SelectListItem>();
                    mq = "select b.tmode mldid,m.col31 mldname from itransactionc b inner join kc_tab m on m.vch_num=b.tmode and m.type = 'MOS' and m.client_unit_id = b.client_unit_id " +
                        "where b.type='BOM' and b.acode='" + model[0].Col18 + "' and b.pono='" + model[0].SelectedItems2.FirstOrDefault() + "'";
                    DataTable dtp = new DataTable();
                    dtp = sgen.getdata(userCode, mq);
                    if (dtp.Rows.Count > 0) { foreach (DataRow dr in dtp.Rows) { mod5.Add(new SelectListItem { Text = dr["mldname"].ToString(), Value = dr["mldid"].ToString() }); } }
                    else { mod5 = cmd_Fun.mldname(userCode, unitid_mst); }
                    TempData[MyGuid + "_TList5"] = mod5;
                    model[0].TList5 = mod5;
                    model[0].SelectedItems5 = new string[] { "" };
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vsavenew = "";
                    ViewBag.vgetdt = "";
                    ViewBag.vchngdt = "disabled='disabled'";
                }
                else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
                {
                    try
                    {
                        sgen.SetSession(MyGuid, "btnval", "");
                        if (model[0].dt1.Rows[0]["icode"].ToString().Trim().Equals("-"))
                        {
                            ViewBag.scripCall += "showmsgJS(1,'Please Select item in grid-1', 2);";
                            ViewBag.vnew = "disabled='disabled'";
                            ViewBag.vedit = "disabled='disabled'";
                            ViewBag.vsave = "";
                            ViewBag.vsavenew = "";
                            return View(model);
                        }
                        #region check grid 2 calc
                        //if (model[0].dt2 != null)
                        //{
                        //    if (model[0].dt2.Rows[0]["icode"].ToString().Trim().Equals("-"))
                        //    {
                        //        ViewBag.scripCall += "showmsgJS(1,'Please FILL location in grid-2', 2);";
                        //        ViewBag.vnew = "disabled='disabled'";
                        //        ViewBag.vedit = "disabled='disabled'";
                        //        ViewBag.vsave = "";
                        //        ViewBag.vsavenew = "";
                        //        return View(model);
                        //    }
                        //    foreach (DataRow dr in ds.Tables[0].Rows)
                        //    {
                        //        decimal Act_Consume_Qty = 0, Act_Consume_Qty2 = 0;
                        //        string fstr = dr["sno"].ToString();
                        //        string icode = dr["icode"].ToString();
                        //        string INAME = dr["INAME"].ToString();
                        //        Act_Consume_Qty = sgen.Make_decimal(dr["Act_Consume_Qty"].ToString());
                        //        var filtered = model[0].dt2.Select("icode='" + icode + "'");
                        //        foreach (var row in filtered)
                        //        {
                        //            decimal consume_qty = 0;
                        //            Act_Consume_Qty2 += consume_qty = sgen.Make_decimal(row["Act_Consume_Qty"].ToString());
                        //        }
                        //        if (Act_Consume_Qty != Act_Consume_Qty2)
                        //        {
                        //            ViewBag.scripCall += "showmsgJS(1, 'Act_Consume_Qty does not match with Location Grid of Item " + INAME + "', 2);";
                        //            ViewBag.vnew = "disabled='disabled'";
                        //            ViewBag.vedit = "disabled='disabled'";
                        //            ViewBag.vsave = "";
                        //            return View(model);
                        //        }
                        //    }
                        //}
                        #endregion
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            decimal act_consume_qty = 0, qtystk = 0;
                            string icode = "", iname = "";
                            icode = dr["icode"].ToString();
                            iname = dr["iname"].ToString();
                            act_consume_qty = sgen.Make_decimal(dr["Act_Consume_Qty"].ToString());
                            qtystk = sgen.Make_decimal(dr["Qtystk"].ToString());
                            if (act_consume_qty > qtystk)
                            {
                                ViewBag.scripCall += "showmsgJS(1, 'Act_Consume_Qty can not be more than Qtystk of Icode " + icode + "', 2);";
                                ViewBag.vnew = "disabled='disabled'";
                                ViewBag.vedit = "disabled='disabled'";
                                ViewBag.vsave = "";
                                ViewBag.vsavenew = "";
                                ViewBag.vgetdt = "";
                                ViewBag.vchngdt = "disabled='disabled'";
                                return View(model);
                            }
                        }
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
                        mq2 = "select vch_num,to_char(to_date(to_char(vch_date,'DD-MM-YYYY')||' '||stime,'DD-MM-YYYY HH24:MI:SS'),'DD-MM-YYYY HH24:MI:SS') stime," +
                            "to_char(to_date(to_char(vch_date, 'DD-MM-YYYY') || ' ' || etime, 'DD-MM-YYYY HH24:MI:SS'), 'DD-MM-YYYY HH24:MI:SS') etime from iprod " +
                            "where type = '" + model[0].Col12 + "'" + model[0].Col11 + " and mccode = '" + model[0].SelectedItems4 + "' and " +
                            "to_date('" + model[0].Col17 + "' || ' ' || '" + model[0].Col25 + "', '" + sgen.dateformat + " HH24:MI:SS') < to_date(to_char(vch_date, 'DD-MM-YYYY') || ' ' || etime, 'DD-MM-YYYY HH24:MI:SS')";
                        DataTable dttm = sgen.getdata(userCode, mq2);
                        if (dttm.Rows.Count > 0)
                        {
                            ViewBag.scripCall += "showmsgJS(1,'Please change Tool Start Time, start time of the machine is lower than the last production voucher no - " + dttm.Rows[0]["vch_num"].ToString() + "', 2);";
                            ViewBag.vnew = "disabled='disabled'";
                            ViewBag.vedit = "disabled='disabled'";
                            ViewBag.vsave = "";
                            ViewBag.vsavenew = "";
                            return View(model);
                        }

                        if (isedit == false)
                        {
                            if (model[0].Col98.Trim().Equals("100"))
                            {
                                mq3 = "select pdi from item where icode='" + model[0].Col19.Trim() + "'";
                                mq3 = sgen.seekval(userCode, mq3, "pdi");
                                if (mq3.Trim().Equals("N"))
                                {
                                    string vchid = sgen.genNo(userCode, "select max(to_number (vch_num)) as max from itransaction where client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "' and type='19'", 6, "max");
                                    #region itransaction
                                    DataTable dtstr = cmd_Fun.GetStructure(userCode, "itransaction");
                                    DataRow dr = dtstr.NewRow();
                                    dr["vch_num"] = vchid;
                                    dr["type"] = "19";
                                    dr["vch_date"] = sgen.Make_date_S(model[0].Col17);
                                    dr["aname"] = "-";//operator name
                                    dr["acode"] = model[0].SelectedItems3.FirstOrDefault();//operator code                                                                             
                                    dr["icode"] = model[0].Col19;
                                    dr["iname"] = model[0].Col20;
                                    dr["qtychl"] = sgen.Make_decimal(model[0].Col23) + sgen.Make_decimal(model[0].Col31);
                                    dr["qtyin"] = sgen.Make_decimal(model[0].Col23) + sgen.Make_decimal(model[0].Col31);
                                    dr["store"] = "Y";
                                    dr = getsave(currdate, dr, model, false);
                                    dtstr.Rows.Add(dr);
                                    #endregion
                                    #region iprod
                                    DataTable dri0 = cmd_Fun.GetStructure(userCode, "Iprod");
                                    dr = dri0.NewRow();
                                    dr["vch_num"] = vch_num;
                                    dr["type"] = "10P";
                                    dr["vch_date"] = sgen.Make_date_S(model[0].Col17);
                                    dr["icode"] = model[0].Col19;
                                    dr["iname"] = model[0].Col20;
                                    dr["stage"] = "100";
                                    dr["stage1"] = "-";
                                    dr["qtyout"] = sgen.Make_decimal(model[0].Col23) + sgen.Make_decimal(model[0].Col31);
                                    dr = getsave(currdate, dr, model, false);
                                    dri0.Rows.Add(dr);
                                    #endregion
                                    #region btchtrans
                                    DataTable dri1 = cmd_Fun.GetStructure(userCode, "btchtrans");
                                    dr = dri1.NewRow();
                                    dr["vch_num"] = vchid;
                                    dr["type"] = "19";
                                    dr["vch_date"] = sgen.Make_date_S(model[0].Col17);//dsp date
                                    dr["acode"] = model[0].Col21;//op
                                    dr["chldate"] = currdate;//return date
                                    dr["chlno"] = "-";
                                    dr["icode"] = model[0].Col19;
                                    dr["iname"] = model[0].Col20;
                                    dr["qtyin"] = sgen.Make_decimal(model[0].Col23) + sgen.Make_decimal(model[0].Col31);
                                    DataTable dtl = sgen.getdata(userCode, "select loc,location_name('" + clientid_mst + "'||'" + unitid_mst + "'||loc) locname from itbal where icode='" + model[0].Col19 + "' and type='IT' and client_unit_id='" + unitid_mst + "'");
                                    if (dtl.Rows.Count > 0)
                                    {
                                        dr["loc"] = dtl.Rows[0]["loc"].ToString();
                                        dr["locname"] = dtl.Rows[0]["locname"].ToString();
                                    }
                                    dr["store"] = "Y";
                                    dr = getsave(currdate, dr, model, false);
                                    dri1.Rows.Add(dr);
                                    #endregion
                                    bool re1 = sgen.Update_data_fast1_uncommit(userCode, dtstr, "itransaction", "", false, sat);
                                    bool re2 = sgen.Update_data_fast1_uncommit(userCode, dri0, "iprod", "", false, sat);
                                    bool re3 = sgen.Update_data_fast1_uncommit(userCode, dri1, "btchtrans", "", false, sat);
                                }
                            }
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
                                                                                    //if 4 or 7
                            if (model[0].SelectedItems4.FirstOrDefault() == "")
                            {
                                dr["mccode"] = model[0].SelectedItems7.FirstOrDefault();//machine
                            }
                            else
                            {
                                dr["mccode"] = model[0].SelectedItems4.FirstOrDefault();//machine
                            }
                            dr["mouldcode"] = model[0].SelectedItems5.FirstOrDefault();//mould
                            dr["mctxt"] = model[0].Col50;//machine txt
                            dr["mouldtxt"] = model[0].Col51;//mould txt
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
                            dr = getsave(currdate, dr, model, isedit);
                            dataTable.Rows.Add(dr);
                        }
                        res = sgen.Update_data_uncommit2(userCode, dataTable, model[0].Col10.Trim(), tmodel.Col8, isedit, sat);
                        if (res == true)
                        {
                            #region for dt2
                            //if (model[0].dt2 != null)
                            //{
                            //    DataTable dti = cmd_Fun.GetStructure(userCode, "itransaction");
                            //    for (int i = 0; i < model[0].dt2.Rows.Count; i++)
                            //    {
                            //        DataRow dri = dti.NewRow();
                            //        dri["type"] = model[0].Col12;
                            //        dri["vch_num"] = vch_num.Trim();
                            //        dri["vch_date"] = sgen.Savedate(model[0].Col17, true);
                            //        dri["acode"] = model[0].Col19;//main icode                           
                            //        //dt====
                            //        dri["icode"] = model[0].dt2.Rows[i][1].ToString();
                            //        dri["iname"] = model[0].dt2.Rows[i][2].ToString();
                            //        dri["uom"] = model[0].dt2.Rows[i][3].ToString();
                            //        dri["qtystk"] = model[0].dt2.Rows[i][4].ToString();//qty stock
                            //        dri["qtyout"] = model[0].dt2.Rows[i][5].ToString();//Act con qty                         
                            //        dri["store"] = "Y";
                            //        dri = getsave(currdate, dri, model, isedit);
                            //        dti.Rows.Add(dri);
                            //    }
                            //    ok = sgen.Update_data_uncommit2(userCode, dti, "itransaction", tmodel.Col8, isedit, sat);
                            //    if (!ok)
                            //    {
                            //        sat.Rollback();
                            //        ViewBag.vnew = "disabled='disabled'";
                            //        ViewBag.vedit = "disabled='disabled'";
                            //        ViewBag.vsave = "";
                            //        ViewBag.vsavenew = "";
                            //        ViewBag.scripCall += "showmsgJS(1, 'stock error', 0);";
                            //        ModelState.Clear();
                            //        return View(model);
                            //    }
                            //    DataTable dtl = cmd_Fun.GetStructure(userCode, "btchtrans");
                            //    for (int i = 0; i < model[0].dt2.Rows.Count; i++)
                            //    {
                            //        DataRow dr = dtl.NewRow();
                            //        dr["type"] = model[0].Col12;
                            //        dr["vch_num"] = vch_num.Trim();
                            //        dr["vch_date"] = sgen.Savedate(model[0].Col17, true);
                            //        dr["acode"] = model[0].Col19;//main icode                           
                            //        //dt====
                            //        dr["icode"] = model[0].dt2.Rows[i][1].ToString();
                            //        dr["iname"] = model[0].dt2.Rows[i][2].ToString();
                            //        dr["uom"] = model[0].dt2.Rows[i][3].ToString();
                            //        dr["qtystk"] = model[0].dt2.Rows[i][4].ToString();//qty stock
                            //        dr["qtyout"] = model[0].dt2.Rows[i][5].ToString();//Act con qty
                            //        dr["loc"] = model[0].dt2.Rows[i][5].ToString();//locid  
                            //        dr["locname"] = model[0].dt2.Rows[i][6].ToString();//locname
                            //        dr["store"] = "Y";
                            //        //======                        
                            //        dr = getsave(currdate, dr, model, isedit);
                            //        dtl.Rows.Add(dr);
                            //    }
                            //    ok = sgen.Update_data_uncommit2(userCode, dtl, "btchtrans", tmodel.Col8, isedit, sat);
                            //    if (!ok)
                            //    {
                            //        sat.Rollback();
                            //        ViewBag.vnew = "disabled='disabled'";
                            //        ViewBag.vedit = "disabled='disabled'";
                            //        ViewBag.vsave = "";
                            //        ViewBag.vsavenew = "";
                            //        ViewBag.scripCall += "showmsgJS(1, 'Location error', 0);";
                            //        ModelState.Clear();
                            //        return View(model);
                            //    }
                            //}
                            #endregion
                            #region final stg waste
                            //if (model[0].Col101.Trim().Equals("F"))//final pd in itransaction
                            //{
                            //if (!isedit)
                            //{
                            //    DataTable dtf = cmd_Fun.GetStructure(userCode, "itransaction");
                            //    DataRow dr = dtf.NewRow();
                            //    dr["type"] = model[0].Col12;
                            //    dr["vch_num"] = vch_num.Trim();
                            //    dr["vch_date"] = sgen.Savedate(model[0].Col17, true);
                            //    dr["icode"] = model[0].Col19;//main icode                                                                                   
                            //    dr["iname"] = model[0].Col20;
                            //    dr["uom"] = model[0].Col21;
                            //    dr["qtyin"] = model[0].Col23;//pqty
                            //    dr["store"] = "Y";
                            //    dr = getsave(currdate, dr, model, isedit);
                            //    dtf.Rows.Add(dr);
                            //    ok = sgen.Update_data_uncommit2(userCode, dtf, "itransaction", tmodel.Col8, isedit, sat);
                            //    if (!ok)
                            //    {
                            //        sat.Rollback();
                            //        ViewBag.vnew = "disabled='disabled'";
                            //        ViewBag.vedit = "disabled='disabled'";
                            //        ViewBag.vsave = "";
                            //        ViewBag.vsavenew = "";
                            //        ViewBag.scripCall += "showmsgJS(1, 'Final Production error', 0);";
                            //        ModelState.Clear();
                            //        return View(model);
                            //    }
                            //}
                            //}
                            #endregion
                            model = new List<Tmodelmain>();
                            model.Add(new Tmodelmain
                            {
                                //Col9 = tmodel.Col9,
                                Col10 = tmodel.Col10,
                                Col11 = tmodel.Col11,
                                //Col12 = tmodel.Col12,
                                Col13 = "Save",
                                Col100 = "Save & New",
                                Col121 = "S",
                                Col122 = "<u>S</u>ave",
                                Col123 = "Save & Ne<u>w</u>",
                                Col14 = tmodel.Col14,
                                Col15 = tmodel.Col15,
                                //TList1 = mod1,
                                TList2 = mod2,
                                TList3 = mod3,
                                TList4 = mod4,
                                TList5 = mod5,
                                TList6 = mod6,
                                TList7 = mod7,
                                //SelectedItems1 = new string[] { "" },
                                SelectedItems2 = new string[] { "" },
                                SelectedItems3 = new string[] { "" },
                                SelectedItems4 = new string[] { "" },
                                SelectedItems5 = new string[] { "" },
                                SelectedItems6 = new string[] { "" },
                                SelectedItems7 = new string[] { "" },
                                dt1 = ((DataTable)sgen.GetSession(MyGuid, "dtipd")).Copy(),
                            });
                            sat.Commit();
                            if (command.Trim().ToUpper().Equals("SAVE") || command.Trim().ToUpper().Equals("UPDATE"))
                            {
                                ViewBag.vnew = "";
                                ViewBag.vedit = "";
                                ViewBag.vsave = "disabled='disabled'";
                                ViewBag.vsavenew = "disabled='disabled'";
                                ViewBag.vgetdt = "disabled='disabled'";
                                ViewBag.vchngdt = "disabled='disabled'";
                                ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                            }
                            else if (command.Trim().ToUpper().Equals("SAVE & NEW") || command.Trim().ToUpper().Equals("UPDATE & NEW"))
                            {
                                Make_query(actionName, "Select Production Type", "NEW", "1", "", "",MyGuid);
                                ViewBag.scripCall += "callFoo('Select Production Type')";
                                ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                            }
                        }
                        else
                        {
                            sat.Rollback();
                            ViewBag.vnew = "disabled='disabled'";
                            ViewBag.vedit = "disabled='disabled'";
                            ViewBag.vsave = "";
                            ViewBag.vsavenew = "";
                            ViewBag.vgetdt = "";
                            ViewBag.vchngdt = "disabled='disabled'";
                            ViewBag.scripCall += "showmsgJS(1, 'Something went wrong', 0);";
                            ModelState.Clear();
                            return View(model);
                        }
                    }
                    catch (Exception ex) { ViewBag.scripCall += "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
                }
                else if (command == "btnmain")
                {
                    sgen.SetSession(MyGuid, "SSEEKVAL", clientid_mst + unitid_mst + model[0].Col18 + "BOM");
                    CallbackFun("ITEM", "", actionName, controllerName, model);
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vchngdt = "disabled='disabled'";
                    ViewBag.vgetdt = "";
                }
                else if (command == "-")
                {
                    if (model[0].dt1.Rows.Count > 1) model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                    else model[0].dt1 = ((DataTable)sgen.GetSession(MyGuid, "dtipd")).Copy();
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vgetdt = "";
                    ViewBag.vchngdt = "disabled='disabled'";
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
        #region iprodn
        public ActionResult iprodn()
        {
            FillMst();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "";
            model[0].Col10 = "iprod";
            //model[0].Col12 = "100"; // pd      
            model[0].Col11 = " and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            DataTable dt = sgen.getdata(userCode, "select '0' sno ,'-' icode,'-' iname,'-' uom,'0' qtystk,'-' stgcode,'-' stg,'0' est_consume_qty,'0' act_consume_qty,'0' short_excess,'-' Alternate from dual");
            model[0].dt1 = dt;
            sgen.SetSession(MyGuid, "dtipd", dt);
            //model[0].dt2 = null;
            //Session["dtipd2"] = null;
            model[0].TList1 = mod1;
            model[0].TList2 = mod1;
            model[0].TList3 = mod1;
            model[0].TList4 = mod1;
            model[0].TList5 = mod1;
            model[0].TList6 = mod1;
            model[0].TList7 = mod1;
            model[0].SelectedItems1 = new string[] { "" };
            model[0].SelectedItems2 = new string[] { "" };
            model[0].SelectedItems3 = new string[] { "" };
            model[0].SelectedItems4 = new string[] { "" };
            model[0].SelectedItems5 = new string[] { "" };
            model[0].SelectedItems6 = new string[] { "" };
            model[0].SelectedItems7 = new string[] { "" };
            model[0].Col99 = "N";//do not check stk calc
            model[0].Col96 = "N";//fromstg
            ViewBag.vgetdt = "disabled='disabled'";
            ViewBag.vchngdt = "disabled='disabled'";
            sgen.SetSession(MyGuid, "btnval", "");
            return View(model);
        }
        [HttpPost]
        public ActionResult iprodn(List<Tmodelmain> model, string command, string hfrow, string hftable)
        {
            try
            {
                FillMst(model[0].Col15);
                DataSet ds = new DataSet();
                if (hftable.Trim() != "")
                {
                    ds = sgen.setDS(hftable);
                    model[0].dt1 = ds.Tables[0];
                }
                var tm = model.FirstOrDefault();
                #region
                List<SelectListItem> mod1 = new List<SelectListItem>();
                List<SelectListItem> mod2 = new List<SelectListItem>();
                List<SelectListItem> mod3 = new List<SelectListItem>();
                List<SelectListItem> mod4 = new List<SelectListItem>();
                List<SelectListItem> mod5 = new List<SelectListItem>();
                List<SelectListItem> mod6 = new List<SelectListItem>();
                List<SelectListItem> mod7 = new List<SelectListItem>();
                model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
                model[0].TList2 = (List<SelectListItem>)TempData[MyGuid + "_TList2"];
                model[0].TList3 = (List<SelectListItem>)TempData[MyGuid + "_TList3"];
                model[0].TList4 = (List<SelectListItem>)TempData[MyGuid + "_TList4"];
                model[0].TList5 = (List<SelectListItem>)TempData[MyGuid + "_TList5"];
                model[0].TList6 = (List<SelectListItem>)TempData[MyGuid + "_TList6"];
                model[0].TList7 = (List<SelectListItem>)TempData[MyGuid + "_TList7"];
                TempData[MyGuid + "_TList1"] = model[0].TList1;
                TempData[MyGuid + "_TList2"] = model[0].TList2;
                TempData[MyGuid + "_TList3"] = model[0].TList3;
                TempData[MyGuid + "_TList4"] = model[0].TList4;
                TempData[MyGuid + "_TList5"] = model[0].TList5;
                TempData[MyGuid + "_TList6"] = model[0].TList6;
                TempData[MyGuid + "_TList7"] = model[0].TList7;
                if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
                if (tm.SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
                if (tm.SelectedItems3 == null) model[0].SelectedItems3 = new string[] { "" };
                if (tm.SelectedItems4 == null) model[0].SelectedItems4 = new string[] { "" };
                if (tm.SelectedItems5 == null) model[0].SelectedItems5 = new string[] { "" };
                if (tm.SelectedItems6 == null) model[0].SelectedItems6 = new string[] { "" };
                if (tm.SelectedItems7 == null) model[0].SelectedItems7 = new string[] { "" };
                #endregion
                if (command == "Cancel") { return CancelFun(model); }
                else if (command == "Callback")
                {
                    model = StartCallback(model);
                    try
                    {
                        sgen.SetSession(MyGuid, "btnval", Regex.Split(sgen.GetSession(MyGuid, "parent_op").ToString(), "~!~!~!~!")[1].ToString());
                        sgen.SetSession(MyGuid, "opname", null);
                        mod3 = cmd_Fun.opname(userCode, unitid_mst);
                        TempData[MyGuid + "_TList3"] = mod3;
                        model[0].TList3 = mod3;
                        string opid = Regex.Split(sgen.GetSession(MyGuid, "parent_op").ToString(), "~!~!~!~!")[0].ToString();
                        model[0].SelectedItems3 = new string[] { opid };
                    }
                    catch (Exception ex) { }
                    if (sgen.GetSession(MyGuid, "btnval").ToString() == "ITEM")
                    {
                        ViewBag.vchngdt = "disabled='disabled'";
                        ViewBag.vgetdt = "";
                    }
                    else
                    {
                        ViewBag.vchngdt = "";
                        //ViewBag.vgetdt = "disabled='disabled'";
                    }
                }
                else if (command == "Get Data")
                {
                    try
                    {
                        CallbackFun("PDREQ", "N", actionName, controllerName, model);
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                        ViewBag.vgetdt = "disabled='disabled'";
                        ViewBag.vchngdt = "";
                    }
                    catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
                }
                else if (command == "Change Qty")
                {
                    try
                    {
                        model[0].dt1 = ((DataTable)sgen.GetSession(MyGuid, "dtipd")).Copy();
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                        ViewBag.vgetdt = "";
                        ViewBag.vchngdt = "disabled='disabled'";
                    }
                    catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
                }
                else if (command == "ddlmach")
                {
                    mod4 = new List<SelectListItem>();
                    mod7 = new List<SelectListItem>();
                    string mld = model[0].SelectedItems5.FirstOrDefault();
                    mq = "select c.master_name mc_caps,group_concat(m.master_name) mcomp from kc_tab d " +
                        "inner join master_setting c on c.master_id = d.col14 and c.type = 'K02' and find_in_set(c.client_unit_id, d.client_unit_id) = 1 " +
                        "inner join master_setting m on find_in_set(m.master_id, d.col34)= 1 and m.type = 'K02' and find_in_set(m.client_unit_id, d.client_unit_id) = 1 " +
                        "where d.client_unit_id = '" + unitid_mst + "' and d.type = 'MOS' and d.vch_num = '" + mld + "' " +
                        "group by d.col14,c.master_name,d.col34";
                    DataTable dtc = sgen.getdata(userCode, mq);
                    if (dtc.Rows.Count > 0)
                    {
                        mq1 = "select a.vch_num,a.col31 mcname,nvl(b.master_id,'-') master_id,nvl(b.master_name,'0') master_name from kc_tab a " +
                            "inner join master_setting b on b.master_id = a.col14 and b.type = 'K02' and find_in_set(b.client_unit_id, a.client_unit_id) = 1 and to_number(nvl(b.master_name,0))>= " + sgen.Make_int(dtc.Rows[0]["mc_caps"].ToString()) + " " +
                            "where a.client_unit_id = '" + unitid_mst + "' and a.type = 'PMM' order by a.vch_num asc";
                        DataTable dt = sgen.getdata(userCode, mq1);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                mod4.Add(new SelectListItem { Text = dr["mcname"].ToString(), Value = dr["vch_num"].ToString() });
                            }
                        }
                        mq1 = "select a.vch_num,a.col31 mcname,nvl(b.master_id,'-') master_id,nvl(b.master_name,'0') master_name from kc_tab a " +
                           "inner join master_setting b on b.master_id = a.col14 and b.type = 'K02' and find_in_set(b.client_unit_id, a.client_unit_id) = 1 and nvl(b.master_name,0) in ('" + dtc.Rows[0]["mcomp"].ToString().Replace(",", "','") + "') " +
                           "where a.client_unit_id = '" + unitid_mst + "' and a.type = 'PMM' order by a.vch_num asc";
                        dt = sgen.getdata(userCode, mq1);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                mod7.Add(new SelectListItem { Text = dr["mcname"].ToString(), Value = dr["vch_num"].ToString() });
                            }
                        }
                    }
                    TempData[MyGuid + "_TList4"] = mod4;
                    model[0].TList4 = mod4;
                    model[0].SelectedItems4 = new string[] { "" };
                    TempData[MyGuid + "_TList7"] = mod7;
                    model[0].TList7 = mod7;
                    model[0].SelectedItems7 = new string[] { "" };
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vsavenew = "";
                    ViewBag.vgetdt = "";
                    ViewBag.vchngdt = "disabled='disabled'";
                }
                else if (command == "ddlmld")
                {
                    mod5 = new List<SelectListItem>();

                    mq = "select bm.pono stgcode,bm.cond stg,bm.rno from itransactionc bm where bm.type = 'BOM' and " +
                           "bm.client_unit_id = '" + unitid_mst + "' and bm.acode = '" + model[0].Col19 + "' order by bm.rno asc";
                    DataTable dtk = sgen.getdata(userCode, mq);
                    DataTable stgss = new DataTable();
                    stgss.Columns.Add("stgcode");
                    string lastcode = "", newcode = "";
                    if (dtk.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtk.Rows.Count; i++)
                        {
                            newcode = dtk.Rows[i]["stgcode"].ToString().Trim();
                            if (newcode != lastcode)
                            {
                                DataRow dr = stgss.NewRow();
                                dr[0] = newcode;
                                stgss.Rows.Add(dr);
                                lastcode = newcode;
                            }
                        }
                        stgss.AcceptChanges();
                        int totstg = stgss.Rows.Count;
                        int stg = sgen.seekval_dt_rowindex(stgss, "stgcode='" + model[0].SelectedItems2.FirstOrDefault() + "'");
                        if (stg == 1) { model[0].Col96 = "Y"; }
                        else model[0].Col96 = "N";
                    }
                    mq = "select b.tmode mldid,m.col31 mldname from itransactionc b inner join kc_tab m on m.vch_num=b.tmode and m.type = 'MOS' and m.client_unit_id = b.client_unit_id " +
            "where b.type='BOM' and b.acode='" + model[0].Col18 + "' and b.pono='" + model[0].SelectedItems2.FirstOrDefault() + "'";
                    DataTable dtp = new DataTable();
                    dtp = sgen.getdata(userCode, mq);
                    if (dtp.Rows.Count > 0) { foreach (DataRow dr in dtp.Rows) { mod5.Add(new SelectListItem { Text = dr["mldname"].ToString(), Value = dr["mldid"].ToString() }); } }
                    else { mod5 = cmd_Fun.mldname(userCode, unitid_mst); }
                    TempData[MyGuid + "_TList5"] = mod5;
                    model[0].TList5 = mod5;
                    model[0].SelectedItems5 = new string[] { "" };
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vsavenew = "";
                    ViewBag.vgetdt = "";
                    ViewBag.vchngdt = "disabled='disabled'";
                }
                else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
                {
                    try
                    {
                        sgen.SetSession(MyGuid, "btnval", "");
                        if (model[0].dt1.Rows[0]["icode"].ToString().Trim().Equals("-"))
                        {
                            ViewBag.scripCall += "showmsgJS(1,'Please Select item in grid-1', 2);";
                            ViewBag.vnew = "disabled='disabled'";
                            ViewBag.vedit = "disabled='disabled'";
                            ViewBag.vsave = "";
                            ViewBag.vsavenew = "";
                            return View(model);
                        }
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            decimal act_consume_qty = 0, qtystk = 0;
                            string icode = "", iname = "";
                            icode = dr["icode"].ToString();
                            iname = dr["iname"].ToString();
                            act_consume_qty = sgen.Make_decimal(dr["Act_Consume_Qty"].ToString());
                            qtystk = sgen.Make_decimal(dr["Qtystk"].ToString());
                            if (act_consume_qty > qtystk)
                            {
                                ViewBag.scripCall += "showmsgJS(1, 'Act_Consume_Qty can not be more than Qtystk of Icode " + icode + "', 2);";
                                ViewBag.vnew = "disabled='disabled'";
                                ViewBag.vedit = "disabled='disabled'";
                                ViewBag.vsave = "";
                                ViewBag.vsavenew = "";
                                ViewBag.vgetdt = "";
                                ViewBag.vchngdt = "disabled='disabled'";
                                return View(model);
                            }
                        }
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
                        mq2 = "select vch_num,to_char(to_date(to_char(vch_date,'DD-MM-YYYY')||' '||stime,'DD-MM-YYYY HH24:MI:SS'),'DD-MM-YYYY HH24:MI:SS') stime," +
                            "to_char(to_date(to_char(vch_date, 'DD-MM-YYYY') || ' ' || etime, 'DD-MM-YYYY HH24:MI:SS'), 'DD-MM-YYYY HH24:MI:SS') etime from iprod " +
                            "where type = '" + model[0].Col12 + "'" + model[0].Col11 + " and mccode = '" + model[0].SelectedItems4 + "' and " +
                            "to_date('" + model[0].Col17 + "' || ' ' || '" + model[0].Col25 + "', '" + sgen.dateformat + " HH24:MI:SS') < to_date(to_char(vch_date, 'DD-MM-YYYY') || ' ' || etime, 'DD-MM-YYYY HH24:MI:SS')";
                        DataTable dttm = sgen.getdata(userCode, mq2);
                        if (dttm.Rows.Count > 0)
                        {
                            ViewBag.scripCall += "showmsgJS(1,'Please change Tool Start Time, start time of the machine is lower than the last production voucher no - " + dttm.Rows[0]["vch_num"].ToString() + "', 2);";
                            ViewBag.vnew = "disabled='disabled'";
                            ViewBag.vedit = "disabled='disabled'";
                            ViewBag.vsave = "";
                            ViewBag.vsavenew = "";
                            return View(model);
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
                            //if 4 or 7
                            if (model[0].SelectedItems4.FirstOrDefault() == "") dr["mccode"] = model[0].SelectedItems7.FirstOrDefault();//machine
                            else dr["mccode"] = model[0].SelectedItems4.FirstOrDefault();//machine

                            dr["mouldcode"] = model[0].SelectedItems5.FirstOrDefault();//mould
                            dr["mctxt"] = model[0].Col50;//machine txt
                            dr["mouldtxt"] = model[0].Col51;//mould txt
                            dr["shftno"] = model[0].SelectedItems6.FirstOrDefault();//shft
                            dr["stime"] = model[0].Col25;//tool starttime
                            dr["etime"] = model[0].Col26;//tool endtime
                            dr["qtyin"] = sgen.Make_decimal(model[0].Col23);//produce qty
                            dr["qtyrej"] = sgen.Make_decimal(model[0].Col30);//rejection qty
                            dr["qtyrw"] = sgen.Make_decimal(model[0].Col31);//rework qty
                            dr["totremark"] = model[0].Col24;//remark
                            dr["cstk"] = sgen.Make_decimal(model[0].Col29);//currentstk
                            //dr["rno"] = model[0].Col99;//check stk or not
                            dr["rno"] = model[0].Col96;//from stg
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
                            dr = getsave(currdate, dr, model, isedit);
                            dataTable.Rows.Add(dr);
                        }
                        res = sgen.Update_data_uncommit2(userCode, dataTable, model[0].Col10.Trim(), tmodel.Col8, isedit, sat);
                        if (res == true)
                        {
                            model = new List<Tmodelmain>();
                            model.Add(new Tmodelmain
                            {
                                //Col9 = tmodel.Col9,
                                Col10 = tmodel.Col10,
                                Col11 = tmodel.Col11,
                                //Col12 = tmodel.Col12,
                                Col13 = "Save",
                                Col100 = "Save & New",
                                Col121 = "S",
                                Col122 = "<u>S</u>ave",
                                Col123 = "Save & Ne<u>w</u>",
                                Col14 = tmodel.Col14,
                                Col15 = tmodel.Col15,
                                //TList1 = mod1,
                                TList2 = mod2,
                                TList3 = mod3,
                                TList4 = mod4,
                                TList5 = mod5,
                                TList6 = mod6,
                                TList7 = mod7,
                                //SelectedItems1 = new string[] { "" },
                                SelectedItems2 = new string[] { "" },
                                SelectedItems3 = new string[] { "" },
                                SelectedItems4 = new string[] { "" },
                                SelectedItems5 = new string[] { "" },
                                SelectedItems6 = new string[] { "" },
                                SelectedItems7 = new string[] { "" },
                                dt1 = ((DataTable)sgen.GetSession(MyGuid, "dtipd")).Copy(),
                            });
                            sat.Commit();
                            if (command.Trim().ToUpper().Equals("SAVE") || command.Trim().ToUpper().Equals("UPDATE"))
                            {
                                ViewBag.vnew = "";
                                ViewBag.vedit = "";
                                ViewBag.vsave = "disabled='disabled'";
                                ViewBag.vsavenew = "disabled='disabled'";
                                ViewBag.vgetdt = "disabled='disabled'";
                                ViewBag.vchngdt = "disabled='disabled'";
                                ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                            }
                            else if (command.Trim().ToUpper().Equals("SAVE & NEW") || command.Trim().ToUpper().Equals("UPDATE & NEW"))
                            {
                                Make_query(actionName, "Select Production Type", "NEW", "1", "", "",MyGuid);
                                ViewBag.scripCall += "callFoo('Select Production Type')";
                                ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                            }
                        }
                        else
                        {
                            sat.Rollback();
                            ViewBag.vnew = "disabled='disabled'";
                            ViewBag.vedit = "disabled='disabled'";
                            ViewBag.vsave = "";
                            ViewBag.vsavenew = "";
                            ViewBag.vgetdt = "";
                            ViewBag.vchngdt = "disabled='disabled'";
                            ViewBag.scripCall += "showmsgJS(1, 'Something went wrong', 0);";
                            ModelState.Clear();
                            return View(model);
                        }
                    }
                    catch (Exception ex) { ViewBag.scripCall += "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
                }
                else if (command == "btnmain")
                {
                    sgen.SetSession(MyGuid, "SSEEKVAL", clientid_mst + unitid_mst + model[0].Col18 + "BOM");
                    CallbackFun("ITEM", "", actionName, controllerName, model);
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vchngdt = "disabled='disabled'";
                    ViewBag.vgetdt = "";
                }
                else if (command == "-")
                {
                    if (model[0].dt1.Rows.Count > 1) model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                    else model[0].dt1 = ((DataTable)sgen.GetSession(MyGuid, "dtipd")).Copy();
                    ViewBag.vnew = "disabled='disabled'";
                    ViewBag.vedit = "disabled='disabled'";
                    ViewBag.vsave = "";
                    ViewBag.vgetdt = "";
                    ViewBag.vchngdt = "disabled='disabled'";
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
        #region pdrevrse
        public ActionResult pdrevrse()
        {
            FillMst();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col10 = "iprod";
            model[0].Col11 = " and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            DataTable dt = sgen.getdata(userCode, "select '0' sno ,'-' icode,'-' iname,'-' uom,'0' qtystk,'-' stgcode,'-' stg,'0' est_consume_qty,'0' act_consume_qty,'0' short_excess,'-' Alternate from dual");
            model[0].dt1 = dt;
            sgen.SetSession(MyGuid, "dtipd", dt);
            //model[0].TList1 = mod1;
            model[0].TList2 = mod1;
            model[0].TList3 = mod1;
            model[0].TList4 = mod1;
            model[0].TList5 = mod1;
            model[0].TList6 = mod1;
            model[0].TList7 = mod1;
            //model[0].SelectedItems1 = new string[] { "" };
            model[0].SelectedItems2 = new string[] { "" };
            model[0].SelectedItems3 = new string[] { "" };
            model[0].SelectedItems4 = new string[] { "" };
            model[0].SelectedItems5 = new string[] { "" };
            model[0].SelectedItems6 = new string[] { "" };
            model[0].SelectedItems7 = new string[] { "" };
            model[0].Col99 = "N";//do not check stk calc          
            sgen.SetSession(MyGuid, "btnval", "");
            return View(model);
        }
        [HttpPost]
        public ActionResult pdrevrse(List<Tmodelmain> model, string command, string hfrow, string hftable)
        {
            try
            {
                FillMst(model[0].Col15);
                DataSet ds = new DataSet();
                if (hftable.Trim() != "")
                {
                    ds = sgen.setDS(hftable);
                    model[0].dt1 = ds.Tables[0];
                }
                var tm = model.FirstOrDefault();
                #region
                //List<SelectListItem> mod1 = new List<SelectListItem>();
                List<SelectListItem> mod2 = new List<SelectListItem>();
                List<SelectListItem> mod3 = new List<SelectListItem>();
                List<SelectListItem> mod4 = new List<SelectListItem>();
                List<SelectListItem> mod5 = new List<SelectListItem>();
                List<SelectListItem> mod6 = new List<SelectListItem>();
                List<SelectListItem> mod7 = new List<SelectListItem>();
                //model[0].TList1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
                model[0].TList2 = (List<SelectListItem>)TempData[MyGuid + "_TList2"];
                model[0].TList3 = (List<SelectListItem>)TempData[MyGuid + "_TList3"];
                model[0].TList4 = (List<SelectListItem>)TempData[MyGuid + "_TList4"];
                model[0].TList5 = (List<SelectListItem>)TempData[MyGuid + "_TList5"];
                model[0].TList6 = (List<SelectListItem>)TempData[MyGuid + "_TList6"];
                model[0].TList7 = (List<SelectListItem>)TempData[MyGuid + "_TList7"];
                //TempData[MyGuid + "_TList1"] = model[0].TList1;
                TempData[MyGuid + "_TList2"] = model[0].TList2;
                TempData[MyGuid + "_TList3"] = model[0].TList3;
                TempData[MyGuid + "_TList4"] = model[0].TList4;
                TempData[MyGuid + "_TList5"] = model[0].TList5;
                TempData[MyGuid + "_TList6"] = model[0].TList6;
                TempData[MyGuid + "_TList7"] = model[0].TList7;
                //if (tm.SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
                if (tm.SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };
                if (tm.SelectedItems3 == null) model[0].SelectedItems3 = new string[] { "" };
                if (tm.SelectedItems4 == null) model[0].SelectedItems4 = new string[] { "" };
                if (tm.SelectedItems5 == null) model[0].SelectedItems5 = new string[] { "" };
                if (tm.SelectedItems6 == null) model[0].SelectedItems6 = new string[] { "" };
                if (tm.SelectedItems7 == null) model[0].SelectedItems7 = new string[] { "" };
                #endregion               
                if (command == "Cancel") { return CancelFun(model); }
                else if (command == "Callback")
                {
                    model = StartCallback(model);
                }
                else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
                {
                    try
                    {
                        sgen.SetSession(MyGuid, "btnval", "");
                        if (model[0].dt1.Rows[0]["icode"].ToString().Trim().Equals("-"))
                        {
                            ViewBag.scripCall += "showmsgJS(1,'Please Select item in grid-1', 2);";
                            ViewBag.vnew = "disabled='disabled'";
                            ViewBag.vedit = "disabled='disabled'";
                            ViewBag.vsave = "";
                            ViewBag.vsavenew = "";
                            return View(model);
                        }
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            decimal act_consume_qty = 0, qtystk = 0;
                            string icode = "", iname = "";
                            icode = dr["icode"].ToString();
                            iname = dr["iname"].ToString();
                            act_consume_qty = sgen.Make_decimal(dr["Act_Consume_Qty"].ToString());
                            qtystk = sgen.Make_decimal(dr["Qtystk"].ToString());
                            if (act_consume_qty > qtystk)
                            {
                                ViewBag.scripCall += "showmsgJS(1, 'Act_Consume_Qty can not be more than Qtystk of Icode " + icode + "', 2);";
                                ViewBag.vnew = "disabled='disabled'";
                                ViewBag.vedit = "disabled='disabled'";
                                ViewBag.vsave = "";
                                ViewBag.vsavenew = "";
                                ViewBag.vgetdt = "";
                                ViewBag.vchngdt = "disabled='disabled'";
                                return View(model);
                            }
                        }
                        Satransaction sat = new Satransaction(userCode, MyGuid);
                        var tmodel = model.FirstOrDefault();
                        string currdate = sgen.server_datetime(userCode);
                        if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                        {
                            isedit = false;
                            mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                            vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                            model[0].Col16 = vch_num;
                        }
                        mq2 = "select vch_num,to_char(to_date(to_char(vch_date,'DD-MM-YYYY')||' '||stime,'DD-MM-YYYY HH24:MI:SS'),'DD-MM-YYYY HH24:MI:SS') stime," +
                            "to_char(to_date(to_char(vch_date, 'DD-MM-YYYY') || ' ' || etime, 'DD-MM-YYYY HH24:MI:SS'), 'DD-MM-YYYY HH24:MI:SS') etime from iprod " +
                            "where type = '" + model[0].Col12 + "'" + model[0].Col11 + " and mccode = '" + model[0].SelectedItems4 + "' and " +
                            "to_date('" + model[0].Col17 + "' || ' ' || '" + model[0].Col25 + "', '" + sgen.dateformat + " HH24:MI:SS') < to_date(to_char(vch_date, 'DD-MM-YYYY') || ' ' || etime, 'DD-MM-YYYY HH24:MI:SS')";
                        DataTable dttm = sgen.getdata(userCode, mq2);
                        if (dttm.Rows.Count > 0)
                        {
                            ViewBag.scripCall += "showmsgJS(1,'Please change Tool Start Time, start time of the machine is lower than the last production voucher no - " + dttm.Rows[0]["vch_num"].ToString() + "', 2);";
                            ViewBag.vnew = "disabled='disabled'";
                            ViewBag.vedit = "disabled='disabled'";
                            ViewBag.vsave = "";
                            ViewBag.vsavenew = "";
                            return View(model);
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
                                                                                    //if 4 or 7
                            if (model[0].SelectedItems4.FirstOrDefault() == "")
                            {
                                dr["mccode"] = model[0].SelectedItems7.FirstOrDefault();//machine
                            }
                            else
                            {
                                dr["mccode"] = model[0].SelectedItems4.FirstOrDefault();//machine
                            }
                            dr["mouldcode"] = model[0].SelectedItems5.FirstOrDefault();//mould
                            dr["mctxt"] = model[0].Col50;//machine txt
                            dr["mouldtxt"] = model[0].Col51;//mould txt
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
                            dr["refcode"] = model[0].Col45;
                            //dt====
                            dr["icode"] = model[0].dt1.Rows[i][1].ToString();
                            dr["iname"] = model[0].dt1.Rows[i][2].ToString();
                            dr["qtystk"] = sgen.Make_decimal(model[0].dt1.Rows[i][4].ToString().Trim());//qtystk
                            dr["stage"] = model[0].dt1.Rows[i][5].ToString();//item stage                      
                            dr["qtychl"] = sgen.Make_decimal(model[0].dt1.Rows[i][7].ToString().Trim());//est con qty
                            dr["qtyout"] = sgen.Make_decimal(model[0].dt1.Rows[i][8].ToString().Trim());//act con qty               
                            dr["qtyplanned"] = sgen.Make_decimal(model[0].dt1.Rows[i][9].ToString().Trim());//short_excess
                            dr["alt"] = model[0].dt1.Rows[i][10].ToString().Trim();//alternate
                            dr = getsave(currdate, dr, model, isedit);
                            dataTable.Rows.Add(dr);
                        }
                        res = sgen.Update_data_uncommit2(userCode, dataTable, model[0].Col10.Trim(), tmodel.Col8, isedit, sat);
                        if (res == true)
                        {
                            model = new List<Tmodelmain>();
                            model.Add(new Tmodelmain
                            {
                                //Col9 = tmodel.Col9,
                                Col10 = tmodel.Col10,
                                Col11 = tmodel.Col11,
                                //Col12 = tmodel.Col12,
                                Col13 = "Save",
                                Col14 = tmodel.Col14,
                                Col15 = tmodel.Col15,
                                //TList1 = mod1,
                                TList2 = mod2,
                                TList3 = mod3,
                                TList4 = mod4,
                                TList5 = mod5,
                                TList6 = mod6,
                                TList7 = mod7,
                                //SelectedItems1 = new string[] { "" },
                                SelectedItems2 = new string[] { "" },
                                SelectedItems3 = new string[] { "" },
                                SelectedItems4 = new string[] { "" },
                                SelectedItems5 = new string[] { "" },
                                SelectedItems6 = new string[] { "" },
                                SelectedItems7 = new string[] { "" },
                                dt1 = ((DataTable)sgen.GetSession(MyGuid, "dtipd")).Copy(),
                                Col100 = "Save & New",
                                Col121 = "S",
                                Col122 = "<u>S</u>ave",
                                Col123 = "Save & Ne<u>w</u>",
                            });
                            sat.Commit();
                            if (command.Trim().ToUpper().Equals("SAVE") || command.Trim().ToUpper().Equals("UPDATE"))
                            {
                                ViewBag.vnew = "";
                                ViewBag.vedit = "";
                                ViewBag.vsave = "disabled='disabled'";
                                ViewBag.vsavenew = "disabled='disabled'";
                                ViewBag.vgetdt = "disabled='disabled'";
                                ViewBag.vchngdt = "disabled='disabled'";
                                ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                            }
                            //else if (command.Trim().ToUpper().Equals("SAVE & NEW") || command.Trim().ToUpper().Equals("UPDATE & NEW"))
                            //{
                            //    ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                            //    Make_query(actionName, "Production Order Type", "NEW", "1", "", "");
                            //    ViewBag.scripCall += "callFoo('Stage Wise Stock')";
                            //    //CallbackFun("NEW", "N", actionName, controllerName, model);                            
                            //}
                        }
                        else
                        {
                            sat.Rollback();
                            ViewBag.vnew = "disabled='disabled'";
                            ViewBag.vedit = "disabled='disabled'";
                            ViewBag.vsave = "";
                            ViewBag.vsavenew = "";
                            ViewBag.vgetdt = "";
                            ViewBag.vchngdt = "disabled='disabled'";
                            ViewBag.scripCall += "showmsgJS(1, 'Something went wrong', 0);";
                            ModelState.Clear();
                            return View(model);
                        }
                    }
                    catch (Exception ex) { ViewBag.scripCall += "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
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
        #region pdreps
        public ActionResult pdreps()
        {
            FillMst();
            chkRef();
            // shiv
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            List<Tmodelmain> model = new List<Tmodelmain>();
            Tmodelmain tm1 = new Tmodelmain();
            tm1.Col9 = "PRODUCTION REPORT";
            tm1.Col10 = mid_mst.Trim();
            tm1.Col11 = MyGuid.Trim();
            sgen.SetSession(MyGuid, "PD_MID", mid_mst.Trim());
            model.Add(tm1);
            return View(model);
        }
        [HttpPost]
        public ActionResult pdreps(List<Tmodelmain> model, string command)
        {
            try
            {
                FillMst(model[0].Col11);
                var tm = model.FirstOrDefault();
                string fdt = "", tdt = "", title = "";
                DataSet dst = new DataSet();
                DataTable dt = new DataTable();
                if (sgen.GetSession(MyGuid, "KPDFDT") != null) fdt = sgen.GetSession(MyGuid, "KPDFDT").ToString();
                if (sgen.GetSession(MyGuid, "KPDTDT") != null) tdt = sgen.GetSession(MyGuid, "KPDTDT").ToString();
                if (sgen.GetSession(MyGuid, "KPDCMD") != null) command = sgen.GetSession(MyGuid, "KPDCMD").ToString();
                if (command.Trim().ToUpper() == "STAGE WISE" || command.Trim().ToUpper() == "REDBIN STOCK" || command.Trim().ToUpper() == "BOM REPORT" || command.Trim().ToUpper() == "CALLBACK") { sgen.SetSession(MyGuid, "KPDCMD", command); }
                else
                {
                    if (fdt == "") { ViewBag.scripCall += "showmsgJS(1,'Please Select From Date',2)"; return View(model); }
                    if (tdt == "") { ViewBag.scripCall += "showmsgJS(1,'Please Select To Date',2)"; return View(model); }
                }
                DataTable dtr = new DataTable();
                dtr.Columns.Add("temp");
                DataRow dr = dtr.NewRow();
                dr["temp"] = "temp";
                dtr.Rows.Add(dr);
                string date_period = "from " + fdt + " to " + tdt + "";
                if (command == "Callback")
                {
                    model = StartCallback(model);
                }
                else if (command == "Stage Wise")
                {
                    //mq = "select a.icode as fstr, a.icode,b.iname,a.stage,Upper(c.master_name) sname,a.client_id,a.client_unit_id,sum(a.qtyout) AS qtyout,sum(a.qtyin) as qtyin " +
                    //    ",sum(a.qtyin)-sum(a.qtyout)  as Closing from (select icode, stage, client_id, client_unit_id, TO_NUMBER(qtyout) AS qtyout," +
                    //    " 0 as qtyin from iprod   union all select icode, stage, client_id, client_unit_id, 0 qtyout, to_number(qtyin) as qtyin from (" +
                    //    "select max(a.qtyin) as qtyin, a.acode as icode, a.stage1 as stage, a.client_id, a.client_unit_id,a.vch_num, " +
                    //    "to_char(a.vch_date, 'yyyyMMdd') as vch_date from iprod a  group by a.acode, a.stage1, a.client_id, a.client_unit_id, " +
                    //    "a.vch_num, to_char(a.vch_date, 'yyyyMMdd')) b) a  inner join item b on trim(a.icode) = trim(b.icode) and " +
                    //    "find_in_set(a.client_id, b.client_id)= 1 and find_in_set(a.client_unit_id, b.client_unit_id)= 1   and b.type = 'IT' " +
                    //    "inner join master_setting c on trim(a.stage)= trim(c.master_id) and find_in_set(a.client_id, c.client_id)= 1 and " +
                    //    "find_in_set(a.client_unit_id, c.client_unit_id)= 1 and c.type = 'KPS' where a.client_id='" + clientid_mst + "' and " +
                    //    " a.client_unit_id='" + unitid_mst + "' group by a.icode,a.stage,a.client_id,a.client_unit_id,b.iname,c.master_name order by icode asc";
                    mq = "select i.icode fstr,i.icode, i.iname, um.master_name uom, sum(nvl(a.qtyin, 0)) qtyin, sum(nvl(a.qtyout, 0)) qtyout, sum(nvl(a.qtyin, 0)) - sum(nvl(a.qtyout, 0)) qtystk," +
                         "nvl(a.fstg, '001') fstgCode,st.master_name fstg from item i " +
                         "left join (select icode, (case when type='100' then 0 else qtyin end)  qtyin, qtyout qtyout, stage fstg, type, " +
                         "client_id, client_unit_id,vch_num,vch_date from iprod where (type like '3%' or type = '10P') " +
                         "union " +
                         "select acode icode, (to_number(max(qtyin))+to_number(max(qtyrw))) qtyin, 0 qtyout, stage1 fstg, type, client_id, client_unit_id ,vch_num,vch_date from iprod " +
                         "where type = '100' group by acode, stage1, type, client_id, client_unit_id ,vch_num,vch_date " +
                         "union " +
                         "select acode icode, to_number(max(qtyrej)) qtyin, 0 qtyout, '101' fstg, type, client_id, client_unit_id,vch_num,vch_date from iprod where type = '100' " +
                         "group by acode, type, client_id, client_unit_id,vch_num,vch_date " +
                         "union " +
                         "select icode, sum(qtyin) qtyin, 0 qtyout, stage1 fstg, type, client_id, client_unit_id ,vch_num,vch_date from iprod where type = '10R' " +
                         "group by icode, stage1, type, client_id, client_unit_id,vch_num,vch_date " +
                         "union " +
                         "select icode, 0 qtyin, sum(qtyout) qtyout, stage fstg, type,client_id, client_unit_id,vch_num,vch_date from iprod where type = '10R' " +
                         "group by icode, stage, type, client_id, client_unit_id,vch_num,vch_date " +
                         "union " +
                         "select icode, sum(qtyout) qtyin, 0 qtyout, stage1 fstg, type,client_id, client_unit_id,vch_num,vch_date from iprod where type = '10R' " +
                         "group by icode, stage1, type, client_id, client_unit_id,vch_num,vch_date) a on a.icode = i.icode and find_in_set(a.client_unit_id, i.client_unit_id)=1 " +
                         "inner join master_setting um on um.master_id = i.uom and um.type = 'UMM' and find_in_set(um.client_unit_id, i.client_unit_id)= 1 " +
                         "inner join master_setting st on st.master_id=nvl(a.fstg, '001') and st.type='KPS' " +
                         "where i.client_unit_id = '" + unitid_mst + "' and i.type = 'IT' group by i.icode,i.iname,um.master_name,nvl(a.fstg, '001'),st.master_name";
                    dt = sgen.getdata(userCode, mq);
                    if (dt.Rows.Count > 0)
                    {
                        //Pivot pv = new Pivot(dt);
                        //dt = pv.PivotData("closing", AggregateFunction.Sum, new string[] { "fstr", "Icode", "Iname" }, new string[] { "Sname" });
                        //mq1 = sgen.open_grid_dt("Stage Wise Production", mq, 0);
                        sgen.open_grid("Stage Wise Stock", mq, 0);
                        ViewBag.scripCall = "callFoo('Stage Wise Stock');";
                    }
                    else { ViewBag.scripCall += "showmsgJS(1,'No Data Found',2);"; }
                    //return View(model);
                }
                else if (command == "Stage Wise Production")
                {
                    // kashish 9 line
                    mq = "select * from (select (ex.client_id||ex.client_unit_id||ex.vch_num||to_char(ex.vch_date,'yyyymmdd')||ex.type) fstr,ex.vch_num DocNo," +
                        "to_char(convert_tz(ex.vch_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getdateformat() + "') Production_Date,ex.acode MainItem," +
                        "it.iname MainItemName, um.master_name Uom,stf.master_name From_Stage, stt.master_name To_Stage, max(ex.qtyin) Produce_Qty from iprod ex " +
                        "inner join item it on it.icode = ex.acode and it.type = 'IT' and find_in_set(it.client_unit_id, ex.client_unit_id)=1 " +
                        "inner join master_setting um on um.master_id = it.uom and um.type = 'UMM' and find_in_set(um.client_unit_id, it.client_unit_id)= 1 " +
                        "inner join master_setting stf on stf.master_id = ex.mstage and stf.type = 'KPS' " +
                        "inner join master_setting stt on stt.master_id = ex.stage1 and stt.type = 'KPS' " +
                        "where ex.type = '100' and ex.client_unit_id = '" + unitid_mst + "' " +
                        "and ex.vch_date between TO_DATE('" + fdt + "','" + sgen.Getsqldateformat() + "') AND TO_DATE('" + tdt + "','" + sgen.Getsqldateformat() + "') " +
                        "group by (ex.client_id|| ex.client_unit_id || ex.vch_num || to_char(ex.vch_date, 'yyyymmdd') || ex.type),ex.vch_num," +
                        "to_char(convert_tz(ex.vch_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getdateformat() + "'),ex.acode,it.iname,um.master_name," +
                        "stf.master_name,stt.master_name) a order by DocNo asc";
                    dt = sgen.getdata(userCode, mq);
                    if (dt.Rows.Count > 0)
                    {
                        sgen.open_grid("Stage Wise Production", mq, 0);
                        ViewBag.scripCall = "callFoo('Stage Wise Production');";
                    }
                    else { ViewBag.scripCall += "showmsgJS(1,'No Data Found',2);"; }
                    //return View(model);
                }
                else if (command == "Slow and Non Moving")
                {
                    mq = "";
                    dt = sgen.getdata(userCode, mq);
                    if (dt.Rows.Count > 1)
                    {
                        sgen.open_report_bydt_xml(userCode, dt, "slow_moving", "Report");
                        ViewBag.scripCall += "showRptnew('Slow and Non Moving');";

                        //ViewBag.scripCall = "OpenSingle('../../../../../erp/schoolReports_Rpts/Filter_Report_xml.aspx','95%','900px','Reports');";
                    }
                    else { ViewBag.scripCall += "showmsgJS(1,'No Data Found',2);"; }
                }
                else if (command == "Item Wise PDI")
                {

                    // kashish 2, 7 and 8 line
                    mq = "select fstr,icode,iname,uom,qtyin,vch_date from (SELECT(t.client_id || t.client_unit_id || t.icode || to_char(t.vch_date, 'ddmmyyy') || t.type) fstr," +
                        "T.ICODE,I.INAME, U.MASTER_NAME uom,SUM(TO_NUMBER(T.QTYIN)) QTYIN, TO_CHAR(T.VCH_DATE, '" + sgen.Getsqldateformat() + "') VCH_DATE, t.vch_date dt2 " +
                        "FROM ITRANSACTION T " +
                        "INNER JOIN ITEM I ON I.ICODE = T.ICODE AND I.TYPE = 'IT' AND find_in_set(I.CLIENT_UNIT_ID, T.CLIENT_UNIT_ID)=1 " +
                        "INNER JOIN MASTER_SETTING U ON U.MASTER_ID = I.UOM AND U.TYPE = 'UMM' AND FIND_IN_SET(U.CLIENT_UNIT_ID, T.CLIENT_UNIT_ID) = 1 " +
                        "WHERE T.TYPE = '19' and t.client_unit_id = '" + unitid_mst + "' AND TO_DATE(T.VCH_DATE) BETWEEN " +
                        "TO_DATE('" + fdt + "', '" + sgen.Getsqldateformat() + "') AND TO_DATE('" + tdt + "', '" + sgen.Getsqldateformat() + "') GROUP BY (t.client_id || t.client_unit_id || t.icode || to_char(t.vch_date, 'ddmmyyy') || t.type)," +
                        "T.ICODE, I.INAME,U.MASTER_NAME, TO_CHAR(T.VCH_DATE, '" + sgen.Getsqldateformat() + "'), t.vch_date) a order by dt2 asc";
                    dt = sgen.getdata(userCode, mq);
                    mq1 = sgen.open_grid_dt("Item Wise PDI", dt, 0);
                    if (mq1 == "ok") { ViewBag.scripCall = "callFoo('Item Wise PDI');"; }
                    else { ViewBag.scripCall = "showmsgJS(1,'No Data Found',2);"; }
                    //return View(model);
                    //ViewBag.scripCall = "OpenSingle('../../../../../erp/schoolReports_Rpts/Filter_Report_xml.aspx','95%','900px','Reports');";
                }
                else if (command == "Redbin Stock")
                {
                    mq = "select (a.client_id || a.client_unit_id || a.icode || a.fstg) fstr,a.icode,i.iname,u.master_name uom, sum(a.qtyin)-sum(a.qtyout) qtystk,a.fstg fstgcode," +
                         "(case when a.fstg = '101' then 'Rejection' when a.fstg='100' then 'PDI' else s.master_name end) fstg from " +
                         "(select acode icode,to_number(max(qtyrej)) qtyin, 0 qtyout, mstage fstg, type, client_id, client_unit_id, vch_num, vch_date from iprod where type = '100' " +
                         "group by acode, type, client_id, client_unit_id,vch_num,vch_date,mstage " +
                         "union " +
                         "select icode, sum(qtyin) qtyin, sum(qtychl) qtyout, mstage fstg, type,client_id, client_unit_id, vch_num, vch_date from iprod where " +
                         "type = '10R' group by icode, mstage, type, client_id, client_unit_id,vch_num,vch_date " +
                         "union " +
                         "select icode, 0 qtyin, sum(qtyout) qtyout, stage fstg, type, client_id, client_unit_id, vch_num, vch_date from iprod where type = '10R' " +
                         "group by icode, stage, type, client_id, client_unit_id,vch_num,vch_date) a " +
                         "inner join item i on i.icode = a.icode and i.type = 'IT' and find_in_set(i.client_unit_id, a.client_unit_id)=1 " +
                         "inner join master_setting u on u.master_id = i.uom and u.type = 'UMM' and find_in_set(u.client_unit_id, a.client_unit_id)= 1 " +
                         "left join master_setting s on s.master_id = a.fstg and s.type = 'KPS' " +
                         "where a.client_unit_id = '" + unitid_mst + "' " +
                         "group by (a.client_id || a.client_unit_id || a.icode || a.fstg),a.icode,i.iname,u.master_name,a.fstg,s.master_name having sum(qtyin) - sum(qtyout) > 0";
                    dt = sgen.getdata(userCode, mq);
                    if (dt.Rows.Count > 0)
                    {
                        sgen.open_grid("Red Bin Stock", mq, 0);
                        ViewBag.scripCall = "callFoo('Red Bin Stock');";
                    }
                    else { ViewBag.scripCall += "showmsgJS(1,'No Data Found',2);"; }
                    //return View(model);
                }
                else if (command == "Stage Wise Rejection")
                {

                    // kashish date
                    mq = "select fstr,DocNo,DocDate,MainItem,ItemName,Uom,From_Stage,To_Stage,Reject_Qty from (select b.fstr,b.vch_num DocNo,b.vch_date DocDate," +
                        "b.dt2, b.acode MainItem, it.iname ItemName, um.master_name Uom,(case when b.fstg = '100' then 'PDI' when b.fstg = '101' then 'Rejection' else stf.master_name end) From_Stage," +
                        "(case when b.tstg = '100' then 'PDI' when b.tstg = '101' then 'Rejection' else stt.master_name end) To_Stage,b.qtyrej Reject_Qty," +
                        "b.type,b.client_id,b.client_unit_id from " +
                        "(select (ex.client_id|| ex.client_unit_id || ex.vch_num || to_char(ex.vch_date, 'yyyymmdd') || ex.type) fstr," +
                        "ex.vch_num,to_char(convert_tz(ex.vch_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getdateformat() + "') vch_date,ex.vch_date dt2," +
                        "ex.acode acode, ex.mstage fstg, ex.stage1 tstg, max(ex.qtyrej) qtyrej,ex.type,ex.client_id,ex.client_unit_id from iprod ex " +
                        "where ex.type = '100' and ex.client_unit_id = '" + unitid_mst + "' and ex.vch_date between TO_DATE('" + fdt + "', '" + sgen.Getsqldateformat() + "') " +
                        "AND TO_DATE('" + tdt + "','" + sgen.Getsqldateformat() + "') " +
                        "group by (ex.client_id|| ex.client_unit_id || ex.vch_num || to_char(ex.vch_date, 'yyyymmdd') || ex.type),ex.vch_num," +
                        "to_char(convert_tz(ex.vch_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getdateformat() + "'),ex.vch_date, ex.acode,ex.mstage," +
                        "ex.stage1,ex.type,ex.client_id,ex.client_unit_id " +
                        "union all " +
                        "select (ex.client_id || ex.client_unit_id || ex.vch_num || to_char(ex.vch_date, 'yyyymmdd') || ex.type) fstr,ex.vch_num," +
                        "to_char(convert_tz(ex.vch_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getdateformat() + "') vch_date,ex.vch_date dt2,ex.icode acode," +
                        "ex.mstage fstg, ex.stage1 tstg, max(ex.qtyin) qtyrej,ex.type,ex.client_id,ex.client_unit_id from iprod ex " +
                        "where ex.type = '10R' and ex.client_unit_id = '" + unitid_mst + "' and ex.vch_date between TO_DATE('" + fdt + "', '" + sgen.Getsqldateformat() + "') " +
                        "AND TO_DATE('" + tdt + "','" + sgen.Getsqldateformat() + "') " +
                        "group by(ex.client_id|| ex.client_unit_id || ex.vch_num || to_char(ex.vch_date, 'yyyymmdd') || ex.type),ex.vch_num, " +
                        "to_char(convert_tz(ex.vch_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getdateformat() + "'),ex.vch_date,ex.icode,ex.mstage," +
                        "ex.stage1,ex.type,ex.client_id,ex.client_unit_id) b " +
                        "inner join item it on it.icode = b.acode and it.type = 'IT' and find_in_set(it.client_unit_id, b.client_unit_id=1 " +
                        "inner join master_setting um on um.master_id = it.uom and um.type = 'UMM' and find_in_set(um.client_unit_id, b.client_unit_id)= 1 " +
                        "left join master_setting stf on stf.master_id = b.fstg and stf.type = 'KPS' " +
                        "left join master_setting stt on stt.master_id = b.tstg and stt.type = 'KPS') a " +
                        "order by DocNo, dt2 asc";
                    dt = sgen.getdata(userCode, mq);
                    if (dt.Rows.Count > 0)
                    {
                        sgen.open_grid("Stage Wise Rejection", mq, 0);
                        ViewBag.scripCall = "callFoo('Stage Wise Rejection');";
                    }
                    else { ViewBag.scripCall += "showmsgJS(1,'No Data Found',2);"; }
                    //return View(model);
                }
                else if (command == "Item Wise Scrap")
                {

                    // kashish date
                    mq = "select fstr,DocNo,DocDate,MainItem,ItemName,Uom,From_Stage,To_Stage,Reject_Qty from " +
                        "(select b.fstr, b.vch_num DocNo, b.vch_date DocDate, b.dt2, b.acode MainItem, it.iname ItemName, um.master_name Uom, " +
                        "(case when b.fstg = '100' then 'PDI' when b.fstg = '101' then 'Rejection' else stf.master_name end) From_Stage," +
                        "(case when b.tstg = '101' then 'Rejection' else stt.master_name end) To_Stage,b.qtyrej Reject_Qty, b.type,b.client_id,b.client_unit_id from " +
                        "(select (ex.client_id || ex.client_unit_id || ex.vch_num || to_char(ex.vch_date, 'yyyymmdd') || ex.type) fstr,ex.vch_num," +
                        "to_char(convert_tz(ex.vch_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') vch_date,ex.vch_date dt2, " +
                        "ex.icode acode, ex.mstage fstg, ex.stage tstg, ex.qtyrej,ex.type,ex.client_id,ex.client_unit_id from iprod ex " +
                        "where ex.type = '10R' and ex.client_unit_id = '" + unitid_mst + "' and ex.vch_date between " +
                        "TO_DATE('" + fdt + "', '" + sgen.Getsqldateformat() + "') AND TO_DATE('" + tdt + "','" + sgen.Getsqldateformat() + "') and ex.qtyrej > 0) b " +
                        "inner join item it on it.icode = b.acode and it.type = 'IT' and find_in_set(it.client_unit_id, b.client_unit_id)=1 " +
                        "inner join master_setting um on um.master_id = it.uom and um.type = 'UMM' and find_in_set(um.client_unit_id, b.client_unit_id)= 1 " +
                        "left join master_setting stf on stf.master_id = b.fstg and stf.type = 'KPS' " +
                        "left join master_setting stt on stt.master_id = b.tstg and stt.type = 'KPS') a " +
                        "order by DocNo, dt2 asc";
                    dt = sgen.getdata(userCode, mq);
                    mq1 = sgen.open_grid_dt("Item Wise Scrap", dt, 0);
                    if (mq1 == "ok") { ViewBag.scripCall = "callFoo('Item Wise Scrap');"; }
                    else { ViewBag.scripCall = "showmsgJS(1,'No Data Found',2);"; }
                    //return View(model);
                    //ViewBag.scripCall = "OpenSingle('../../../../../erp/schoolReports_Rpts/Filter_Report_xml.aspx','95%','900px','Reports');";
                }
                else if (command == "BOM Report")
                {
                    mq = "select distinct (ex.client_id||ex.client_unit_id||ex.vch_num||to_char(ex.vch_date,'yyyymmdd')||ex.type) fstr,ex.vch_num DocNo," +
                         "to_char(convert_tz(ex.vch_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') Bom_Date,ex.acode MainItem,it.iname MainItemName," +
                         "um.master_name Uom,ex.irate Bom_Lot_Size from itransactionc ex " +
                         "inner join item it on ex.acode = it.icode and it.type = 'IT' and find_in_set(it.client_unit_id, ex.client_unit_id)=1 " +
                         "inner join master_setting um on um.master_id = it.uom and um.type = 'UMM' and find_in_set(um.client_unit_id, it.client_unit_id)= 1 " +
                         "where ex.type = 'BOM' and ex.client_unit_id = '" + unitid_mst + "'";
                    //dt = sgen.getdata(userCode, mq);
                    //if (dt.Rows.Count > 0)
                    {
                        sgen.SetSession(MyGuid, "btnval", "BOM");
                        sgen.open_grid("BOM Detail", mq, 2);
                        ViewBag.scripCall = "callFoo('BOM Details');";
                    }
                    //else { ViewBag.scripCall += "showmsgJS(1,'No Data Found',2);"; }
                    //return View(model);
                }
                else if (command == "Production Summary")
                {
                    //mq = "select i.icode fstr,i.icode,i.iname,um.master_name uom,sum(nvl(a.qtyin, 0)) qtyin,sum(nvl(a.qtyout, 0)) qtyout,sum(nvl(a.qtyin, 0)) - sum(nvl(a.qtyout, 0)) qtystk," +
                    //     "nvl(a.fstg, '001') fstgCode,st.master_name fstg from item i " +
                    //     "left join (select icode, (case when type='100' then 0 else qtyin end)  qtyin, qtyout qtyout, stage fstg, type, " +
                    //     "client_id, client_unit_id,vch_num,vch_date from iprod where (type like '3%' or type = '10P') " +
                    //     "union " +
                    //     "select acode icode, (to_number(max(qtyin))+to_number(max(qtyrw))) qtyin, 0 qtyout, stage1 fstg, type, client_id, client_unit_id ,vch_num,vch_date from iprod " +
                    //     "where type = '100' group by acode, stage1, type, client_id, client_unit_id ,vch_num,vch_date " +
                    //     "union " +
                    //     "select acode icode, to_number(max(qtyrej)) qtyin, 0 qtyout, '101' fstg, type, client_id, client_unit_id,vch_num,vch_date from iprod where type = '100' " +
                    //     "group by acode, type, client_id, client_unit_id,vch_num,vch_date " +
                    //     "union " +
                    //     "select icode, sum(qtyin) qtyin, 0 qtyout, stage1 fstg, type, client_id, client_unit_id ,vch_num,vch_date from iprod where type = '10R' " +
                    //     "group by icode, stage1, type, client_id, client_unit_id,vch_num,vch_date " +
                    //     "union " +
                    //     "select icode, 0 qtyin, sum(qtyout) qtyout, stage fstg, type,client_id, client_unit_id,vch_num,vch_date from iprod where type = '10R' " +
                    //     "group by icode, stage, type, client_id, client_unit_id,vch_num,vch_date " +
                    //     "union " +
                    //     "select icode, sum(qtyout) qtyin, 0 qtyout, stage1 fstg, type,client_id, client_unit_id,vch_num,vch_date from iprod where type = '10R' " +
                    //     "group by icode, stage1, type, client_id, client_unit_id,vch_num,vch_date) a on a.icode = i.icode and find_in_set(a.client_unit_id, i.client_unit_id)=1 " +
                    //     "inner join master_setting um on um.master_id = i.uom and um.type = 'UMM' and find_in_set(um.client_id, i.client_id)= 1 and find_in_set(um.client_unit_id, i.client_unit_id)= 1 " +
                    //     "inner join master_setting st on st.master_id=nvl(a.fstg, '001') and st.type='KPS' and find_in_set(st.client_id,i.client_id)=1 and find_in_set(st.client_unit_id,i.client_unit_id)=1 " +
                    //     "where i.client_unit_id = '" + unitid_mst + "' and i.type = 'IT' and to_date(to_char(a.vch_date,'" + sgen.Getsqldateformat() + "'),'DD/MM/YYYY') " +
                    //     "between TO_DATE('" + fdt + "', 'DD/MM/YYYY') and TO_DATE('" + tdt + "','DD/MM/YYYY')  " +
                    //     "group by i.icode,i.iname,um.master_name,nvl(a.fstg, '001'),st.master_name";


                    // kashish date
                    mq = "select * from (select (s.icode||s.fstg) fstr,s.icode,s.iname,s.uom,sum(s.qtystk) qtystk,s.fstg fstgcode,st.master_name fstg from " +
                        "(select a.vch_num,a.vch_date,i.icode, i.iname, um.master_name uom, sum(nvl(a.qtyin, 0)) qtyin, sum(nvl(a.qtyout, 0)) qtyout, sum(nvl(a.qtyin, 0)) - sum(nvl(a.qtyout, 0)) qtystk," +
                        "nvl(a.fstg, '001') fstg, i.type, i.client_id, i.client_unit_id from item i " +
                        "left join (select icode, (case when type = '100' then 0 else qtyin end) qtyin, qtyout qtyout, stage fstg, type, client_id, client_unit_id," +
                        "vch_num,vch_date from iprod where(type like '3%' or type = '10P' or type = '100') " +
                        "union all " +
                        "select acode icode, (to_number(max(qtyin)) + to_number(max(qtyrw))) qtyin,0 qtyout,stage1 fstg,type,client_id,client_unit_id,vch_num,vch_date from iprod " +
                        "where type = '100' group by acode, stage1, type, client_id, client_unit_id ,vch_num,vch_date " +
                        "union all " +
                        "select acode icode, to_number(max(qtyrej)) qtyin, 0 qtyout, '101' fstg, type, client_id, client_unit_id,vch_num,vch_date from iprod " +
                        "where type = '100' group by acode, type, client_id, client_unit_id,vch_num,vch_date " +
                        "union all " +
                        "select icode, sum(qtyin) qtyin, 0 qtyout, stage1 fstg, type, client_id, client_unit_id, vch_num, vch_date from iprod " +
                        "where type = '10R' group by icode, stage1, type, client_id, client_unit_id,vch_num,vch_date " +
                        "union all " +
                        "select icode, 0 qtyin, sum(qtyout) qtyout, stage fstg, type, client_id, client_unit_id, vch_num, vch_date from iprod " +
                        "where type = '10R' group by icode, stage, type, client_id, client_unit_id,vch_num,vch_date " +
                        "union all " +
                        "select icode, sum(qtyout) qtyin, 0 qtyout, stage1 fstg, type, client_id, client_unit_id, vch_num, vch_date from iprod " +
                        "where type = '10R' group by icode, stage1, type, client_id, client_unit_id,vch_num,vch_date) a on a.icode = i.icode and find_in_set(a.client_unit_id, i.client_unit_id)= 1 " +
                        "inner join master_setting um on um.master_id = i.uom and um.type = 'UMM' and find_in_set(um.client_unit_id, i.client_unit_id)= 1 " +
                        "where i.type = 'IT' and find_in_set(i.client_unit_id, a.client_unit_id)= 1 " +
                        "group by a.vch_num,a.vch_date,i.icode,i.iname,um.master_name,nvl(a.fstg, '001'),i.type,i.client_id,i.client_unit_id) s " +
                        "inner join master_setting st on st.master_id = s.fstg and st.type = 'KPS' " +
                        "where s.client_unit_id = '" + unitid_mst + "' and to_date(to_char(s.vch_date,'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldateformat() + "') " +
                        "between TO_DATE('" + fdt + "', '" + sgen.Getsqldateformat() + "') and TO_DATE('" + tdt + "','" + sgen.Getsqldateformat() + "') " +
                        "group by s.icode,s.iname,s.uom,s.fstg, st.master_name order by s.icode) a";
                    dt = sgen.getdata(userCode, mq);
                    if (dt.Rows.Count > 0)
                    {
                        title = "Stage Wise Stock from " + date_period;
                        sgen.SetSession(MyGuid, "btnval", "ISUM");
                        sgen.open_grid(title, mq, 1);
                        ViewBag.scripCall = "callFoo('Stage Wise Stock');";
                    }
                    else { ViewBag.scripCall += "showmsgJS(1,'No Data Found',2);"; }
                }
                //ViewBag.scripCall = "OpenSingle('../../../../../erp/schoolReports_Rpts/Filter_Report_xml.aspx','95%','900px','Reports');";
                if (command != "Production Summary")
                {
                    sgen.SetSession(MyGuid, "KPDFDT", null);
                    sgen.SetSession(MyGuid, "KPDTDT", null);
                }
                sgen.SetSession(MyGuid, "KPDCMD", null);
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
        #region pdreps2
        public ActionResult pdreps2()
        {
            FillMst();
            chkRef();
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            List<Tmodelmain> model = new List<Tmodelmain>();
            Tmodelmain tm1 = new Tmodelmain();
            tm1.Col9 = "PRODUCTION REPORT";
            tm1.Col10 = "39007.1";
            //tm1.Col10 = mid_mst.Trim();
            tm1.Col11 = MyGuid.Trim();
            sgen.SetSession(MyGuid, "PD_MID", mid_mst.Trim());
            model.Add(tm1);
            return View(model);
        }
        [HttpPost]
        public ActionResult pdreps2(List<Tmodelmain> model, string command)
        {
            try
            {
                FillMst(model[0].Col11);
                var tm = model.FirstOrDefault();
                string fdt = "", tdt = "";
                DataSet dst = new DataSet();
                DataTable dt = new DataTable();
                if (sgen.GetSession(MyGuid, "KPDFDT") != null) fdt = sgen.GetSession(MyGuid, "KPDFDT").ToString();
                if (sgen.GetSession(MyGuid, "KPDTDT") != null) tdt = sgen.GetSession(MyGuid, "KPDTDT").ToString();
                if (sgen.GetSession(MyGuid, "KPDCMD") != null) command = sgen.GetSession(MyGuid, "KPDCMD").ToString();
                if (command.Trim().ToUpper() == "STAGE WISE STOCK" || command.Trim().ToUpper() == "REDBIN STOCK" || command.Trim().ToUpper() == "BOM REPORT" || command.Trim().ToUpper() == "CALLBACK") { sgen.SetSession(MyGuid, "KPDCMD", command); }
                else
                {
                    if (fdt == "") { ViewBag.scripCall += "showmsgJS(1,'Please Select From Date',2)"; return View(model); }
                    if (tdt == "") { ViewBag.scripCall += "showmsgJS(1,'Please Select To Date',2)"; return View(model); }
                }
                DataTable dtr = new DataTable();
                dtr.Columns.Add("temp");
                DataRow dr = dtr.NewRow();
                dr["temp"] = "temp";
                dtr.Rows.Add(dr);
                if (command == "Callback")
                {
                    model = StartCallback(model);
                }
                else if (command == "Stage Wise Stock")
                {
                    //mq = "select a.icode as fstr, a.icode,b.iname,a.stage,Upper(c.master_name) sname,a.client_id,a.client_unit_id,sum(a.qtyout) AS qtyout,sum(a.qtyin) as qtyin " +
                    //    ",sum(a.qtyin)-sum(a.qtyout)  as Closing from (select icode, stage, client_id, client_unit_id, TO_NUMBER(qtyout) AS qtyout," +
                    //    " 0 as qtyin from iprod   union all select icode, stage, client_id, client_unit_id, 0 qtyout, to_number(qtyin) as qtyin from (" +
                    //    "select max(a.qtyin) as qtyin, a.acode as icode, a.stage1 as stage, a.client_id, a.client_unit_id,a.vch_num, " +
                    //    "to_char(a.vch_date, 'yyyyMMdd') as vch_date from iprod a  group by a.acode, a.stage1, a.client_id, a.client_unit_id, " +
                    //    "a.vch_num, to_char(a.vch_date, 'yyyyMMdd')) b) a  inner join item b on trim(a.icode) = trim(b.icode) and " +
                    //    "find_in_set(a.client_id, b.client_id)= 1 and find_in_set(a.client_unit_id, b.client_unit_id)= 1   and b.type = 'IT' " +
                    //    "inner join master_setting c on trim(a.stage)= trim(c.master_id) and find_in_set(a.client_id, c.client_id)= 1 and " +
                    //    "find_in_set(a.client_unit_id, c.client_unit_id)= 1 and c.type = 'KPS' where a.client_id='" + clientid_mst + "' and " +
                    //    " a.client_unit_id='" + unitid_mst + "' group by a.icode,a.stage,a.client_id,a.client_unit_id,b.iname,c.master_name order by icode asc";
                    
                    //mq = "select i.icode, i.iname, um.master_name uom, sum(nvl(a.qtyin, 0)) qtyin, sum(nvl(a.qtyout, 0)) qtyout, sum(nvl(a.qtyin, 0)) - sum(nvl(a.qtyout, 0)) qtystk," +
                    //     "nvl(a.fstg, '001') fstgCode,st.master_name fstg from item i " +
                    //     "left join (select icode, (case when type='100' then 0 else qtyin end)  qtyin, qtyout qtyout, stage fstg, type, " +
                    //     "client_id, client_unit_id,vch_num,vch_date from iprod where (type like '3%' or type = '10P' or type = '100') " +
                    //     "union " +
                    //     "select acode icode, (to_number(max(qtyin))+to_number(max(qtyrw))) qtyin, 0 qtyout, stage1 fstg, type, client_id, client_unit_id ,vch_num,vch_date from iprod " +
                    //     "where type = '100' group by acode, stage1, type, client_id, client_unit_id ,vch_num,vch_date " +
                    //     "union " +
                    //     "select acode icode, to_number(max(qtyrej)) qtyin, 0 qtyout, '101' fstg, type, client_id, client_unit_id,vch_num,vch_date from iprod where type = '100' " +
                    //     "group by acode, type, client_id, client_unit_id,vch_num,vch_date " +
                    //     "union " +
                    //     "select icode, sum(qtyin) qtyin, 0 qtyout, stage1 fstg, type, client_id, client_unit_id ,vch_num,vch_date from iprod where type = '10R' " +
                    //     "group by icode, stage1, type, client_id, client_unit_id,vch_num,vch_date " +
                    //     "union " +
                    //     "select icode, 0 qtyin, sum(qtyout) qtyout, stage fstg, type,client_id, client_unit_id,vch_num,vch_date from iprod where type = '10R' " +
                    //     "group by icode, stage, type, client_id, client_unit_id,vch_num,vch_date " +
                    //     "union " +
                    //     "select icode, sum(qtyout) qtyin, 0 qtyout, stage1 fstg, type,client_id, client_unit_id,vch_num,vch_date from iprod where type = '10R' " +
                    //     "group by icode, stage1, type, client_id, client_unit_id,vch_num,vch_date) a on a.icode = i.icode and find_in_set(a.client_unit_id, i.client_unit_id)=1 " +
                    //     "inner join master_setting um on um.master_id = i.uom and um.type = 'UMM' and find_in_set(um.client_unit_id, i.client_unit_id)= 1 " +
                    //     "inner join master_setting st on st.master_id=nvl(a.fstg, '001') and st.type='KPS' " +
                    //     "where i.client_unit_id = '" + unitid_mst + "' and i.type = 'IT' group by i.icode,i.iname,um.master_name,nvl(a.fstg, '001'),st.master_name";

                    mq = "select i.icode, i.iname, um.master_name uom, sum(nvl(a.qtyin, 0)) qtyin, sum(nvl(a.qtyout, 0)) qtyout, " +
                       "sum(nvl(a.qtyin, 0)) - sum(nvl(a.qtyout, 0)) qtystk,nvl(a.fstg, '001') fstgCode,st.master_name fstg from item i " +
                       "left join (select icode, (case when type = '100' then 0 else qtyin end) qtyin, qtyout qtyout, stage fstg, type, client_id, " +
                       "client_unit_id,vch_num,vch_date from iprod where (type like '3%' or type = '10P' or type = '100') " +
                       "union " +
                       "select acode icode, (to_number(max(qtyin)) + to_number(max(qtyrw))) qtyin, 0 qtyout, stage1 fstg, type, client_id, client_unit_id," +
                       "vch_num, vch_date from iprod where type = '100' group by acode, stage1, type, client_id, client_unit_id ,vch_num,vch_date " +
                       "union " +
                       "select acode icode, to_number(max(qtyrej)) qtyin, 0 qtyout, '101' fstg, type, client_id, client_unit_id,vch_num,vch_date from iprod " +
                       "where type = '100' group by acode, type, client_id, client_unit_id,vch_num,vch_date " +
                       "union " +
                       "select icode, sum(qtyin) qtyin, 0 qtyout, stage1 fstg, type, client_id, client_unit_id, vch_num, vch_date from iprod where " +
                       "type = '10R' group by icode, stage1, type, client_id, client_unit_id,vch_num,vch_date " +
                       "union " +
                       "select icode, 0 qtyin, sum(qtyout) qtyout, stage fstg, type, client_id, client_unit_id, vch_num, vch_date from iprod where " +
                       "type = '10R' group by icode, stage, type, client_id, client_unit_id,vch_num,vch_date " +
                       "union " +
                       "select icode, sum(qtyout) qtyin, 0 qtyout, stage1 fstg, type, client_id, client_unit_id, vch_num, vch_date from iprod where " +
                       "type = '10R' group by icode, stage1, type, client_id, client_unit_id,vch_num,vch_date) a " +
                       "on a.icode = i.icode and find_in_set(a.client_unit_id, i.client_unit_id)= 1 " +
                       "inner join master_setting um on um.master_id = i.uom and um.type = 'UMM' and find_in_set(um.client_unit_id, i.client_unit_id)= 1 " +
                       "inner join master_setting st on st.master_id = nvl(a.fstg, '001') and st.type = 'KPS' " +
                       "where i.client_unit_id = '" + unitid_mst + "' and i.type = 'IT' group by i.icode,i.iname,um.master_name,nvl(a.fstg, '001'),st.master_name";

                    dt = sgen.getdata(userCode, mq);
                    if (dt.Rows.Count > 0)
                    {
                        //Pivot pv = new Pivot(dt);
                        //dt = pv.PivotData("closing", AggregateFunction.Sum, new string[] { "fstr", "Icode", "Iname" }, new string[] { "Sname" });
                        //mq1 = sgen.open_grid_dt("Stage Wise Production", mq, 0);
                        sgen.open_grid("Stage Wise Stock", mq, 0);
                        ViewBag.scripCall = "callFoo('Stage Wise Stock');";
                    }
                    else { ViewBag.scripCall += "showmsgJS(1,'No Data Found',2);"; }
                    //return View(model);
                }
                else if (command == "Stage Wise Production")
                {

                    // kashish date
                    mq = "select * from (select (ex.client_id||ex.client_unit_id||ex.vch_num||to_char(ex.vch_date,'yyyymmdd')||ex.type) fstr,ex.vch_num DocNo," +
                        "to_char(convert_tz(ex.vch_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getdateformat() + "') Production_Date,ex.acode MainItem," +
                        "it.iname MainItemName, um.master_name Uom,stf.master_name From_Stage, stt.master_name To_Stage, max(ex.qtyin) Produce_Qty from iprod ex " +
                        "inner join item it on it.icode = ex.acode and it.type = 'IT' and find_in_set(it.client_unit_id, ex.client_unit_id)=1 " +
                        "inner join master_setting um on um.master_id = it.uom and um.type = 'UMM' and find_in_set(um.client_unit_id, it.client_unit_id)= 1 " +
                        "inner join master_setting stf on stf.master_id = ex.mstage and stf.type = 'KPS' " +
                        "inner join master_setting stt on stt.master_id = ex.stage1 and stt.type = 'KPS' " +
                        "where ex.type = '100' and ex.client_unit_id = '" + unitid_mst + "' " +
                        "and ex.vch_date between TO_DATE('" + fdt + "','" + sgen.Getsqldateformat() + "') AND TO_DATE('" + tdt + "','" + sgen.Getsqldateformat() + "') " +
                        "group by (ex.client_id|| ex.client_unit_id || ex.vch_num || to_char(ex.vch_date, 'yyyymmdd') || ex.type),ex.vch_num," +
                        "to_char(convert_tz(ex.vch_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getdateformat() + "'),ex.acode,it.iname,um.master_name," +
                        "stf.master_name,stt.master_name) a order by DocNo asc";
                    dt = sgen.getdata(userCode, mq);
                    if (dt.Rows.Count > 0)
                    {
                        sgen.open_grid("Stage Wise Production", mq, 0);
                        ViewBag.scripCall = "callFoo('Stage Wise Production');";
                    }
                    else { ViewBag.scripCall += "showmsgJS(1,'No Data Found',2);"; }
                    //return View(model);
                }
                else if (command == "Slow and Non Moving")
                {
                    mq = "";
                    dt = sgen.getdata(userCode, mq);
                    if (dt.Rows.Count > 1)
                    {
                        sgen.open_report_bydt_xml(userCode, dt, "slow_moving", "Report");
                        ViewBag.scripCall += "showRptnew('Slow and Non Moving');";

                        //ViewBag.scripCall = "OpenSingle('../../../../../erp/schoolReports_Rpts/Filter_Report_xml.aspx','95%','900px','Reports');";
                    }
                    else { ViewBag.scripCall += "showmsgJS(1,'No Data Found',2);"; }
                }
                else if (command == "Item Wise PDI")
                {

                    // kashish date
                    mq = "select fstr,icode,iname,uom,qtyin,vch_date from (SELECT(t.client_id || t.client_unit_id || t.icode || to_char(t.vch_date, 'ddmmyyy') || t.type) fstr," +
                        "T.ICODE,I.INAME, U.MASTER_NAME uom,SUM(TO_NUMBER(T.QTYIN)) QTYIN, TO_CHAR(T.VCH_DATE, '" + sgen.Getsqldateformat() + "') VCH_DATE, t.vch_date dt2 " +
                        "FROM ITRANSACTION T " +
                        "INNER JOIN ITEM I ON I.ICODE = T.ICODE AND I.TYPE = 'IT' AND find_in_set(I.CLIENT_UNIT_ID, T.CLIENT_UNIT_ID)=1 " +
                        "INNER JOIN MASTER_SETTING U ON U.MASTER_ID = I.UOM AND U.TYPE = 'UMM' AND FIND_IN_SET(U.CLIENT_UNIT_ID, T.CLIENT_UNIT_ID) = 1 " +
                        "WHERE T.TYPE = '19' and t.client_unit_id = '" + unitid_mst + "' AND TO_DATE(T.VCH_DATE) BETWEEN " +
                        "TO_DATE('" + fdt + "', '" + sgen.Getsqldateformat() + "') AND TO_DATE('" + tdt + "', '" + sgen.Getsqldateformat() + "') GROUP BY (t.client_id || t.client_unit_id || t.icode || to_char(t.vch_date, 'ddmmyyy') || t.type)," +
                        "T.ICODE, I.INAME,U.MASTER_NAME, TO_CHAR(T.VCH_DATE, '" + sgen.Getsqldateformat() + "'), t.vch_date) a order by dt2 asc";
                    dt = sgen.getdata(userCode, mq);
                    mq1 = sgen.open_grid_dt("Item Wise PDI", dt, 0);
                    if (mq1 == "ok") { ViewBag.scripCall = "callFoo('Item Wise PDI');"; }
                    else { ViewBag.scripCall = "showmsgJS(1,'No Data Found',2);"; }
                    //return View(model);
                    //ViewBag.scripCall = "OpenSingle('../../../../../erp/schoolReports_Rpts/Filter_Report_xml.aspx','95%','900px','Reports');";
                }
                else if (command == "Redbin Stock")
                {
                    mq = "select (a.client_id || a.client_unit_id || a.icode || a.fstg) fstr,a.icode,i.iname,u.master_name uom, sum(a.qtyin)-sum(a.qtyout) qtystk,a.fstg fstgcode," +
                         "(case when a.fstg = '101' then 'Rejection' when a.fstg='100' then 'PDI' else s.master_name end) fstg from " +
                         "(select acode icode,to_number(max(qtyrej)) qtyin, 0 qtyout, mstage fstg, type, client_id, client_unit_id, vch_num, vch_date from iprod where type = '100' " +
                         "group by acode, type, client_id, client_unit_id,vch_num,vch_date,mstage " +
                         "union " +
                         "select icode, sum(qtyin) qtyin, sum(qtychl) qtyout, mstage fstg, type,client_id, client_unit_id, vch_num, vch_date from iprod where " +
                         "type = '10R' group by icode, mstage, type, client_id, client_unit_id,vch_num,vch_date " +
                         "union " +
                         "select icode, 0 qtyin, sum(qtyout) qtyout, stage fstg, type, client_id, client_unit_id, vch_num, vch_date from iprod where type = '10R' " +
                         "group by icode, stage, type, client_id, client_unit_id,vch_num,vch_date) a " +
                         "inner join item i on i.icode = a.icode and i.type = 'IT' and find_in_set(i.client_unit_id, a.client_unit_id)=1 " +
                         "inner join master_setting u on u.master_id = i.uom and u.type = 'UMM' and find_in_set(u.client_unit_id, a.client_unit_id)= 1 " +
                         "left join master_setting s on s.master_id = a.fstg and s.type = 'KPS' " +
                         "where a.client_unit_id = '" + unitid_mst + "' " +
                         "group by (a.client_id || a.client_unit_id || a.icode || a.fstg),a.icode,i.iname,u.master_name,a.fstg,s.master_name having sum(qtyin) - sum(qtyout) > 0";
                    dt = sgen.getdata(userCode, mq);
                    if (dt.Rows.Count > 0)
                    {
                        sgen.open_grid("Red Bin Stock", mq, 0);
                        ViewBag.scripCall = "callFoo('Red Bin Stock');";
                    }
                    else { ViewBag.scripCall += "showmsgJS(1,'No Data Found',2);"; }
                    //return View(model);
                }
                else if (command == "Stage Wise Rejection")
                {

                    // kashish date
                    mq = "select fstr,DocNo,DocDate,MainItem,ItemName,Uom,From_Stage,To_Stage,Reject_Qty from (select b.fstr,b.vch_num DocNo,b.vch_date DocDate," +
                        "b.dt2, b.acode MainItem, it.iname ItemName, um.master_name Uom,(case when b.fstg = '100' then 'PDI' when b.fstg = '101' then 'Rejection' else stf.master_name end) From_Stage," +
                        "(case when b.tstg = '100' then 'PDI' when b.tstg = '101' then 'Rejection' else stt.master_name end) To_Stage,b.qtyrej Reject_Qty," +
                        "b.type,b.client_id,b.client_unit_id from " +
                        "(select (ex.client_id|| ex.client_unit_id || ex.vch_num || to_char(ex.vch_date, 'yyyymmdd') || ex.type) fstr," +
                        "ex.vch_num,to_char(convert_tz(ex.vch_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getdateformat() + "') vch_date,ex.vch_date dt2," +
                        "ex.acode acode, ex.mstage fstg, ex.stage1 tstg, max(ex.qtyrej) qtyrej,ex.type,ex.client_id,ex.client_unit_id from iprod ex " +
                        "where ex.type = '100' and ex.client_unit_id = '" + unitid_mst + "' and ex.vch_date between TO_DATE('" + fdt + "', '" + sgen.Getsqldateformat() + "') " +
                        "AND TO_DATE('" + tdt + "','" + sgen.Getsqldateformat() + "') " +
                        "group by (ex.client_id|| ex.client_unit_id || ex.vch_num || to_char(ex.vch_date, 'yyyymmdd') || ex.type),ex.vch_num," +
                        "to_char(convert_tz(ex.vch_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getdateformat() + "'),ex.vch_date, ex.acode,ex.mstage," +
                        "ex.stage1,ex.type,ex.client_id,ex.client_unit_id " +
                        "union all " +
                        "select (ex.client_id || ex.client_unit_id || ex.vch_num || to_char(ex.vch_date, 'yyyymmdd') || ex.type) fstr,ex.vch_num," +
                        "to_char(convert_tz(ex.vch_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getdateformat() + "') vch_date,ex.vch_date dt2,ex.icode acode," +
                        "ex.mstage fstg, ex.stage1 tstg, max(ex.qtyin) qtyrej,ex.type,ex.client_id,ex.client_unit_id from iprod ex " +
                        "where ex.type = '10R' and ex.client_unit_id = '" + unitid_mst + "' and ex.vch_date between TO_DATE('" + fdt + "', '" + sgen.Getsqldateformat() + "') " +
                        "AND TO_DATE('" + tdt + "','" + sgen.Getsqldateformat() + "') " +
                        "group by(ex.client_id|| ex.client_unit_id || ex.vch_num || to_char(ex.vch_date, 'yyyymmdd') || ex.type),ex.vch_num, " +
                        "to_char(convert_tz(ex.vch_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getdateformat() + "'),ex.vch_date,ex.icode,ex.mstage," +
                        "ex.stage1,ex.type,ex.client_id,ex.client_unit_id) b " +
                        "inner join item it on it.icode = b.acode and it.type = 'IT' and find_in_set(it.client_unit_id, b.client_unit_id=1 " +
                        "inner join master_setting um on um.master_id = it.uom and um.type = 'UMM' and find_in_set(um.client_unit_id, b.client_unit_id)= 1 " +
                        "left join master_setting stf on stf.master_id = b.fstg and stf.type = 'KPS' " +
                        "left join master_setting stt on stt.master_id = b.tstg and stt.type = 'KPS') a " +
                        "order by DocNo, dt2 asc";
                    dt = sgen.getdata(userCode, mq);
                    if (dt.Rows.Count > 0)
                    {
                        sgen.open_grid("Stage Wise Rejection", mq, 0);
                        ViewBag.scripCall = "callFoo('Stage Wise Rejection');";
                    }
                    else { ViewBag.scripCall += "showmsgJS(1,'No Data Found',2);"; }
                    //return View(model);
                }
                else if (command == "Item Wise Scrap")
                {

                    // kashish date
                    mq = "select fstr,DocNo,DocDate,MainItem,ItemName,Uom,From_Stage,To_Stage,Reject_Qty from " +
                        "(select b.fstr, b.vch_num DocNo, b.vch_date DocDate, b.dt2, b.acode MainItem, it.iname ItemName, um.master_name Uom, " +
                        "(case when b.fstg = '100' then 'PDI' when b.fstg = '101' then 'Rejection' else stf.master_name end) From_Stage," +
                        "(case when b.tstg = '101' then 'Rejection' else stt.master_name end) To_Stage,b.qtyrej Reject_Qty, b.type,b.client_id,b.client_unit_id from " +
                        "(select (ex.client_id || ex.client_unit_id || ex.vch_num || to_char(ex.vch_date, 'yyyymmdd') || ex.type) fstr,ex.vch_num," +
                        "to_char(convert_tz(ex.vch_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') vch_date,ex.vch_date dt2, " +
                        "ex.icode acode, ex.mstage fstg, ex.stage tstg, ex.qtyrej,ex.type,ex.client_id,ex.client_unit_id from iprod ex " +
                        "where ex.type = '10R' and ex.client_unit_id = '" + unitid_mst + "' and ex.vch_date between " +
                        "TO_DATE('" + fdt + "', '" + sgen.Getsqldateformat() + "') AND TO_DATE('" + tdt + "','" + sgen.Getsqldateformat() + "') and ex.qtyrej > 0) b " +
                        "inner join item it on it.icode = b.acode and it.type = 'IT' and find_in_set(it.client_unit_id, b.client_unit_id)=1 " +
                        "inner join master_setting um on um.master_id = it.uom and um.type = 'UMM' and find_in_set(um.client_unit_id, b.client_unit_id)= 1 " +
                        "left join master_setting stf on stf.master_id = b.fstg and stf.type = 'KPS' " +
                        "left join master_setting stt on stt.master_id = b.tstg and stt.type = 'KPS') a " +
                        "order by DocNo, dt2 asc";
                    dt = sgen.getdata(userCode, mq);
                    mq1 = sgen.open_grid_dt("Item Wise Scrap", dt, 0);
                    if (mq1 == "ok") { ViewBag.scripCall = "callFoo('Item Wise Scrap');"; }
                    else { ViewBag.scripCall = "showmsgJS(1,'No Data Found',2);"; }
                    //return View(model);
                    //ViewBag.scripCall = "OpenSingle('../../../../../erp/schoolReports_Rpts/Filter_Report_xml.aspx','95%','900px','Reports');";
                }
                else if (command == "BOM Report")
                {
                    mq = "select distinct (ex.client_id||ex.client_unit_id||ex.vch_num||to_char(ex.vch_date,'yyyymmdd')||ex.type) fstr,ex.vch_num DocNo," +
                         "to_char(convert_tz(ex.vch_date, 'UTC', '" + sgen.Gettimezone() + "'), '" + sgen.Getsqldateformat() + "') Bom_Date,ex.acode MainItem,it.iname MainItemName," +
                         "um.master_name Uom,ex.irate Bom_Lot_Size from itransactionc ex " +
                         "inner join item it on ex.acode = it.icode and it.type = 'IT' and find_in_set(it.client_unit_id, ex.client_unit_id)=1 " +
                         "inner join master_setting um on um.master_id = it.uom and um.type = 'UMM' and find_in_set(um.client_unit_id, it.client_unit_id)= 1 " +
                         "where ex.type = 'BOM' and ex.client_unit_id = '" + unitid_mst + "'";
                    //dt = sgen.getdata(userCode, mq);
                    //if (dt.Rows.Count > 0)
                    {
                        sgen.SetSession(MyGuid, "btnval", "BOM");
                        sgen.open_grid("BOM Detail", mq, 2);
                        ViewBag.scripCall = "callFoo('BOM Details');";
                    }
                    //else { ViewBag.scripCall += "showmsgJS(1,'No Data Found',2);"; }
                    //return View(model);
                }
                //ViewBag.scripCall = "OpenSingle('../../../../../erp/schoolReports_Rpts/Filter_Report_xml.aspx','95%','900px','Reports');";
                sgen.SetSession(MyGuid, "KPDFDT", null);
                sgen.SetSession(MyGuid, "KPDTDT", null);
                sgen.SetSession(MyGuid, "KPDCMD", null);
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
        #region irej
        public ActionResult irej()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "REDBIN ANALYSIS";
            model[0].Col10 = "iprod";
            model[0].Col12 = "10R  "; // rejpd    
            model[0].Col11 = " and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            DataTable dt = sgen.getdata(userCode, "select '0' SNo,'-' Acode,'-' AName,'-' UOM,'-' Rej_Stgcode,'-' Rej_Stg,'0' Redbin_Qty,'0' Analysis_Qty,'0' Approved_Qty,'0' Rej_Qty,'-' Approved_Stgcode,'-' Approved_Stg,'-' Remark from dual");
            model[0].dt1 = dt;
            sgen.SetSession(MyGuid, "dtirej", dt);
            return View(model);
        }
        public List<Tmodelmain> new_irej(List<Tmodelmain> model)
        {
            try
            {
                model = getnew(model);
                model[0].Col17 = sgen.server_datetime_local(userCode);
                model[0].Col22 = "1";
            }
            catch (Exception ex) { }
            return model;
        }
        [HttpPost]
        public ActionResult irej(List<Tmodelmain> model, string command, string hfrow, string hftable)
        {
            try
            {
                FillMst(model[0].Col15);
                DataSet ds = new DataSet();
                if (hftable.Trim() != "")
                {
                    ds = sgen.setDS(hftable);
                    model[0].dt1 = ds.Tables[0];
                    if (model[0].dt1.Rows.Count == 0) model[0].dt1 = ((DataTable)sgen.GetSession(MyGuid, "dtirej"));
                    try
                    {
                        model[0].dt2 = ds.Tables[1];
                        if (model[0].dt2.Rows.Count == 0) model[0].dt2 = ((DataTable)sgen.GetSession(MyGuid, "dtirej"));
                    }
                    catch (Exception ex) { model[0].dt2 = null; }
                }
                var tm = model.FirstOrDefault();
                if (command == "New")
                {
                    try
                    {
                        model = new_irej(model);
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
                    try
                    {
                        if (model[0].dt1.Rows[0]["acode"].ToString().Trim().Equals("-"))
                        {
                            ViewBag.scripCall += "showmsgJS(1,'Please Select item in grid', 2);";
                            ViewBag.vnew = "disabled='disabled'";
                            ViewBag.vedit = "disabled='disabled'";
                            ViewBag.vsave = "";
                            ViewBag.vsavenew = "";
                            return View(model);
                        }
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
                            dr["vch_date"] = sgen.Savedate(model[0].Col17, true);
                            dr["acode"] = model[0].dt1.Rows[i][1].ToString();//main icode
                            dr["icode"] = model[0].dt1.Rows[i][1].ToString();//main icode
                            dr["iname"] = model[0].dt1.Rows[i][2].ToString();//main iname
                            dr["mstage"] = model[0].dt1.Rows[i][4].ToString();//main stage                                             
                            dr["stage"] = "101";//rej stage
                            dr["qtystk"] = model[0].dt1.Rows[i][6].ToString();//redbin qty
                            dr["qtychl"] = model[0].dt1.Rows[i][7].ToString();//analysis qty
                            dr["qtyout"] = model[0].dt1.Rows[i][8].ToString();//approved qty
                            dr["qtyrej"] = model[0].dt1.Rows[i][9].ToString();//rejection qty                                           
                            dr["stage1"] = model[0].dt1.Rows[i][10].ToString();//approved stage                                                                                                      
                            dr["iremark"] = model[0].dt1.Rows[i][12].ToString();//iremark
                            dr = getsave(currdate, dr, model, isedit);
                            dataTable.Rows.Add(dr);
                        }
                        res = sgen.Update_data_uncommit2(userCode, dataTable, model[0].Col10.Trim(), tmodel.Col8, isedit, sat);
                        if (res == true)
                        {
                            sat.Commit();
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
                                dt1 = (DataTable)sgen.GetSession(MyGuid, "dtirej"),
                            });
                            if (command.Trim().ToUpper().Equals("SAVE") || command.Trim().ToUpper().Equals("UPDATE"))
                            {
                                ViewBag.vnew = "";
                                ViewBag.vedit = "";
                                ViewBag.vsave = "disabled='disabled'";
                                ViewBag.vsavenew = "disabled='disabled'";
                                ViewBag.vgetdt = "disabled='disabled'";
                                ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                            }
                            else if (command.Trim().ToUpper().Equals("SAVE & NEW") || command.Trim().ToUpper().Equals("UPDATE & NEW"))
                            {
                                try
                                {
                                    model = new_irej(model);
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
                            sat.Rollback();
                            ViewBag.vnew = "disabled='disabled'";
                            ViewBag.vedit = "disabled='disabled'";
                            ViewBag.vsave = "";
                            ViewBag.vsavenew = "";
                            ViewBag.scripCall += "showmsgJS(1, 'Something went wrong, please check', 0);";
                            ModelState.Clear();
                            return View(model);
                        }
                    }
                    catch (Exception ex) { ViewBag.scripCall += "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
                }
                else if (command == "-")
                {
                    if (model[0].dt1.Rows.Count > 1) model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                    else model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "dtirej");
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
        #region date_filter
        public ActionResult date_filter(string headid, string mid, string m_id, string btnid)
        {
            FillMst(m_id);
            chkRef();
            List<Tmodelmain> model = new List<Tmodelmain>();
            Tmodelmain tm1 = new Tmodelmain();
            tm1.Col9 = headid;
            tm1.Col10 = mid;
            tm1.Col11 = m_id;
            tm1.Col12 = btnid;
            model.Add(tm1);
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult date_filter(List<Tmodelmain> model, string command)
        {
            FillMst(model[0].Col11);
            if (command == "Search")
            {
                sgen.SetSession(MyGuid, "KPDCMD", model[0].Col12.Trim());
                sgen.SetSession(MyGuid, "KPDFDT", model[0].Col17);
                sgen.SetSession(MyGuid, "KPDTDT", model[0].Col18);
            }
            ModelState.Clear();
            return PartialView(model);
        }
        #endregion
        #region ageing_filter
        public ActionResult ageing_filter(string headid, string mid, string m_id, string btnid)
        {
            FillMst(m_id);
            chkRef();
            List<Tmodelmain> model = new List<Tmodelmain>();
            Tmodelmain tm1 = new Tmodelmain();
            tm1.Col9 = headid;
            tm1.Col10 = mid;
            tm1.Col11 = m_id;
            tm1.Col12 = btnid;
            tm1.Col17 = "0";
            tm1.Col29 = "ABD";
            sgen.SetSession(MyGuid, "EDMODE", "N");
            //List<SelectListItem> mod1 = new List<SelectListItem>();
            //#region ddl1
            //mod1.Add(new SelectListItem { Text = "Ageing By Bill Date", Value = "ABD"});
            //mod1.Add(new SelectListItem { Text = "Ageing By Due Date", Value = "ADD"});
            //TempData[MyGuid + "_TList1"] = mod1;
            //tm1.TList1 = mod1;
            //tm1.SelectedItems1 = new string[] { };
            //#endregion
            cond = sgen.seekval(userCode, "select (client_unit_id||vch_num||to_char(vch_date, 'yyyymmdd')||type) as fstr from enx_tab where type='BAG' and client_unit_id='" + unitid_mst + "'", "fstr");
            model.Add(tm1);
            if (!cond.Equals("0"))
            {
                sgen.SetSession(MyGuid, "SSEEKVAL", cond);
                model = CallbackFun("DETAIL", "N", "ageing_filter", "Production", model);
                sgen.SetSession(MyGuid, "EDMODE", "Y");
            }
            //model.Add(tm1);
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult ageing_filter(List<Tmodelmain> model, string command)
        {
            FillMst(model[0].Col11);
            //List<SelectListItem> mod1 = new List<SelectListItem>();
            //mod1 = (List<SelectListItem>)TempData[MyGuid + "_TList1"];
            //TempData[MyGuid + "_TList1"] = mod1;
            //model[0].TList1 = mod1;
            if (model[0].SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
            if (command == "Submit")
            {
                sgen.SetSession(MyGuid, "BDAGCMD", model[0].Col12.Trim());
                try
                {
                if (sgen.GetSession(MyGuid, "EDMODE").ToString().Equals("Y"))
                {
                    isedit = true;
                    vch_num = model[0].Col16;
                }
                else
                {
                    isedit = false;
                    mq = "select max(to_number(vch_num)) as auto_genid from enx_tab a where type='BAG' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
                    vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                    model[0].Col16 = vch_num;
                }
                string currdate = sgen.server_datetime(userCode);
                DataTable dtstr = new DataTable();
                dtstr = cmd_Fun.GetStructure(userCode, "enx_tab");
                DataRow dr = dtstr.NewRow();
                dr["type"] = "BAG";
                dr["vch_num"] = vch_num.Trim();
                dr["vch_date"] = sgen.Savedate(currdate, false);
                dr["col1"] = model[0].Col17; 
                dr["col2"] = model[0].Col18; 
                dr["col3"] = model[0].Col19; 
                dr["col4"] = model[0].Col20; 
                dr["col5"] = model[0].Col21; 
                dr["col6"] = model[0].Col22; 
                dr["col7"] = model[0].Col23; 
                dr["col8"] = model[0].Col24; 
                dr["col9"] = model[0].Col25; 
                dr["col10"] = model[0].Col26;
                dr["col11"] = model[0].Col27;
                dr["col12"] = model[0].Col28;
                dr["col13"] = model[0].Col29;
                dr = getsave(currdate, dr, model, false);
                dtstr.Rows.Add(dr);
                bool Result = sgen.Update_data_fast1(userCode, dtstr, "enx_tab", model[0].Col8, isedit);
                }
                catch (Exception ex)
                {
                }
            }
            ModelState.Clear();
            return PartialView(model);
        }
        #endregion
        #region dtcu_filter
        public ActionResult dtcu_filter(string headid, string mid, string m_id, string btnid)
        {
            FillMst(m_id);
            chkRef();
            List<Tmodelmain> model = new List<Tmodelmain>();
            Tmodelmain tm1 = new Tmodelmain();
            tm1.Col9 = headid;
            tm1.Col10 = mid;
            tm1.Col11 = m_id;
            tm1.Col12 = btnid;

            List<SelectListItem> ct_list = new List<SelectListItem>();
            List<SelectListItem> ut_list = new List<SelectListItem>(); 
            
            #region client id's
            mq = sgen.seekval(userCode, "select client_id from user_details where rec_id='" + sgen.Make_int(userid_mst) + "'", "client_id");
            if (!role_mst.ToUpper().Equals("OWNER")) where = " and company_profile_id in ('" + mq.Replace(",", "','") + "')";
            DataTable dtcomp = new DataTable();
            mq = "SELECT Company_Profile_Id, company_name ||'('|| company_profile_id ||')'  as nameid FROM company_profile " +
                "where type='CP' " + where + " order by company_name";
            dtcomp = sgen.getdata(userCode, mq);
            if (dtcomp.Rows.Count > 0)
            {
                foreach (DataRow dr in dtcomp.Rows)
                {
                    ct_list.Add(new SelectListItem { Text = dr["nameid"].ToString(), Value = dr["Company_Profile_Id"].ToString() });
                }
            }
            TempData[MyGuid + "_ct_list"] = ct_list;
            #endregion                       
            #region unit id's                   
            mq1 = sgen.seekval(userCode, "select client_unit_id from user_details where vch_num='" + userid_mst + "'", "client_unit_id");
            if (!role_mst.ToUpper().Equals("OWNER")) where = " and cup_id in ('" + mq1.Replace(",", "','") + "')";
            mq = clientid_mst;
            tm1.Col13 = clientid_mst;
            mq = "SELECT cup_id,(unit_name||' ('||cup_Id||')') as nameid FROM company_unit_profile where company_profile_id in (" + mq + ")" + where + " and unit_status='1' order by unit_name";
            DataTable dtunit = new DataTable();
            dtunit = sgen.getdata(userCode, mq);
            if (dtunit.Rows.Count > 0)
            {
                foreach (DataRow dr in dtunit.Rows)
                {
                    ut_list.Add(new SelectListItem { Text = dr["nameid"].ToString(), Value = dr["cup_id"].ToString() });
                }
            }
            TempData[MyGuid + "_ut_list"] = ut_list;
            #endregion

            tm1.TList1 = ct_list;
            tm1.TList2 = ut_list;
            tm1.SelectedItems1 = new string[] { clientid_mst };
            tm1.SelectedItems2 = new string[] { unitid_mst };            

            model.Add(tm1);
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult dtcu_filter(List<Tmodelmain> model, string command)
        {
            FillMst(model[0].Col11);
            List<SelectListItem> ct_list = new List<SelectListItem>();
            List<SelectListItem> ut_list = new List<SelectListItem>();
            ct_list = (List<SelectListItem>)TempData[MyGuid + "_ct_list"];
            TempData[MyGuid + "_ct_list"] = ct_list;
            model[0].TList1 = ct_list;
            model[0].TList2 = ut_list;
            if (model[0].SelectedItems1 == null) model[0].SelectedItems1 = new string[] { "" };
            if (model[0].SelectedItems2 == null) model[0].SelectedItems2 = new string[] { "" };

            if (command == "DDL_CHANGE")
            {
                try
                {
                    #region unit id's                   
                    mq1 = sgen.seekval(userCode, "select client_unit_id from user_details where rec_id='" + Convert.ToInt32(userid_mst) + "'", "client_unit_id");
                    if (!role_mst.ToUpper().Equals("OWNER")) where = " and cup_id in ('" + mq1.Replace(",", "','") + "')";
                    mq = string.Join(",", model[0].SelectedItems1);
                    model[0].Col13 = mq;
                    mq = "SELECT cup_id,(unit_name||' ('||cup_Id||')') as nameid FROM company_unit_profile where company_profile_id in (" + mq + ")" + where + " and unit_status='1' order by unit_name";
                    DataTable dtunit = new DataTable();
                    dtunit = sgen.getdata(userCode, mq);
                    if (dtunit.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtunit.Rows)
                        {
                            ut_list.Add(new SelectListItem { Text = dr["nameid"].ToString(), Value = dr["cup_id"].ToString() });
                        }
                    }
                    TempData[MyGuid + "_ut_list"] = ut_list;
                    #endregion
                    model[0].TList1 = ct_list;
                    model[0].TList2 = ut_list;                 
                    model[0].SelectedItems2 = new string[] { "" };
                }
                catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1, '" + ex.Message.ToString() + "', 0);"; }
            }
            else if (command == "Search")
            {
                sgen.SetSession(MyGuid, "KPDCMD", model[0].Col12.Trim());
                sgen.SetSession(MyGuid, "KPDFDT", model[0].Col17);
                sgen.SetSession(MyGuid, "KPDTDT", model[0].Col18);
                sgen.SetSession(MyGuid, "KDDL_CL", model[0].Col13);
                sgen.SetSession(MyGuid, "KDDL_UT", String.Join(",", model[0].SelectedItems2));
            }
            ModelState.Clear();
            return PartialView(model);
        }
        #endregion

        #region rewrk
        public ActionResult rewrk()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            model = getload(model);
            model[0].Col9 = "REWORK";
            model[0].Col10 = "iprod";
            model[0].Col12 = "10W  "; // rejpd    
            model[0].Col11 = " and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            DataTable dt = sgen.getdata(userCode, "select '0' SNo,'-' Acode,'-' AName,'-' UOM,'-' Rew_Stgcode,'-' Rew_Stg,'0' Rework_Qty,'0' Analysis_Qty,'0' Approved_Qty,'0' Rej_Qty,'-' Approved_Stgcode,'-' Approved_Stg,'-' Remark from dual");
            model[0].dt1 = dt;
            sgen.SetSession(MyGuid, "dtire", dt);
            return View(model);
        }
        public List<Tmodelmain> new_rewrk(List<Tmodelmain> model)
        {
            try
            {
                model = getnew(model);
                model[0].Col17 = sgen.server_datetime_local(userCode);
                model[0].Col22 = "1";
            }
            catch (Exception ex) { }
            return model;
        }
        [HttpPost]
        public ActionResult rewrk(List<Tmodelmain> model, string command, string hfrow, string hftable)
        {
            try
            {
                FillMst(model[0].Col15);
                DataSet ds = new DataSet();
                if (hftable.Trim() != "")
                {
                    ds = sgen.setDS(hftable);
                    model[0].dt1 = ds.Tables[0];
                    if (model[0].dt1.Rows.Count == 0) model[0].dt1 = ((DataTable)sgen.GetSession(MyGuid, "dtirej"));
                    try
                    {
                        model[0].dt2 = ds.Tables[1];
                        if (model[0].dt2.Rows.Count == 0) model[0].dt2 = ((DataTable)sgen.GetSession(MyGuid, "dtirej"));
                    }
                    catch (Exception ex) { model[0].dt2 = null; }
                }
                var tm = model.FirstOrDefault();
                if (command == "New")
                {
                    try
                    {
                        model = new_rewrk(model);
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

                    if (model[0].dt1.Rows[0]["acode"].ToString().Trim().Equals("-"))
                    {
                        ViewBag.scripCall += "showmsgJS(1,'Please Select item in grid', 2);";
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                        return View(model);
                    }
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
                        dr["vch_date"] = sgen.Savedate(model[0].Col17, true);
                        dr["acode"] = model[0].dt1.Rows[i][1].ToString();//main icode
                        dr["icode"] = model[0].dt1.Rows[i][1].ToString();//main icode
                        dr["iname"] = model[0].dt1.Rows[i][2].ToString();//main iname
                        dr["mstage"] = model[0].dt1.Rows[i][4].ToString();//main stage                                             
                        dr["stage"] = "102";//rej stage
                        dr["qtystk"] = model[0].dt1.Rows[i][6].ToString();//redbin qty
                        dr["qtychl"] = model[0].dt1.Rows[i][7].ToString();//analysis qty
                        dr["qtyin"] = model[0].dt1.Rows[i][8].ToString();//approved qty
                        dr["qtyrej"] = model[0].dt1.Rows[i][9].ToString();//rejection qty                                           
                        dr["stage1"] = model[0].dt1.Rows[i][10].ToString();//approved stage                                                                                                      
                        dr["iremark"] = model[0].dt1.Rows[i][12].ToString();//iremark
                        dr = getsave(currdate, dr, model, isedit);
                        dataTable.Rows.Add(dr);
                    }
                    res = sgen.Update_data_uncommit2(userCode, dataTable, model[0].Col10.Trim(), tmodel.Col8, isedit, sat);
                    if (res == true)
                    {
                        sat.Commit();
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
                            dt1 = (DataTable)sgen.GetSession(MyGuid, "dtire"),
                        });
                        if (command.Trim().ToUpper().Equals("SAVE") || command.Trim().ToUpper().Equals("UPDATE"))
                        {
                            ViewBag.vnew = "";
                            ViewBag.vedit = "";
                            ViewBag.vsave = "disabled='disabled'";
                            ViewBag.vsavenew = "disabled='disabled'";
                            ViewBag.vgetdt = "disabled='disabled'";
                            ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');disableForm();";
                        }
                        else if (command.Trim().ToUpper().Equals("SAVE & NEW") || command.Trim().ToUpper().Equals("UPDATE & NEW"))
                        {
                            try
                            {
                                model = new_rewrk(model);
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
                        sat.Rollback();
                        ViewBag.vnew = "disabled='disabled'";
                        ViewBag.vedit = "disabled='disabled'";
                        ViewBag.vsave = "";
                        ViewBag.vsavenew = "";
                        ViewBag.scripCall += "showmsgJS(1, 'Something went wrong, please check', 0);";
                        ModelState.Clear();
                        return View(model);
                    }
                }
                else if (command == "-")
                {
                    if (model[0].dt1.Rows.Count > 1) model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                    else model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "dtire");
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
        #region stgtransfer
        public ActionResult stgtransfer()
        {
            FillMst();
            List<Tmodelmain> model = new List<Tmodelmain>();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            model = getload(model);
            model[0].Col9 = "STAGE TRANSFER";
            model[0].Col10 = "iprod";
            model[0].Col11 = " and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'";
            model[0].Col12 = "100"; //production stage transfer
            model[0].TList1 = mod1;
            model[0].TList2 = mod1;
            model[0].SelectedItems1 = new string[] { "" };
            model[0].SelectedItems2 = new string[] { "" };
            return View(model);
        }

        public List<Tmodelmain> new_stgtransfer(List<Tmodelmain> model)
        {
            model = getnew(model);
            List<SelectListItem> mod1 = new List<SelectListItem>();
            List<SelectListItem> mod2 = new List<SelectListItem>();
            mod1 = cmd_Fun.prodallstage(userCode, unitid_mst);
            model[0].TList1 = mod1;
            mod2 = mod1;
            model[0].TList2 = mod2;
            TempData[MyGuid + "_TList1"] = mod1;
            TempData[MyGuid + "_TList2"] = mod2;
            return model;
        }
        [HttpPost]
        public ActionResult stgtransfer(List<Tmodelmain> model, string command)
        {
            try
            {
                FillMst(model[0].Col15);
                var tm = model.FirstOrDefault();
                command = command.Trim();
                #region
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
                        model = new_stgtransfer(model);
                    }
                    catch (Exception ex) { }
                }
                else if (command == "Cancel") { return CancelFun(model); }
                else if (command == "Callback") { model = StartCallback(model); }
                else if (command == "Stk")
                {                   
                    mq = "select i.icode, i.iname, um.master_name uom, sum(nvl(a.qtyin, 0)) qtyin, sum(nvl(a.qtyout, 0)) qtyout, " +
                        "sum(nvl(a.qtyin, 0)) - sum(nvl(a.qtyout, 0)) qtystk,nvl(a.fstg, '001') fstgCode,st.master_name fstg from item i " +
                        "left join (select icode, (case when type = '100' then 0 else qtyin end) qtyin, qtyout qtyout, stage fstg, type, client_id, " +
                        "client_unit_id,vch_num,vch_date from iprod where (type like '3%' or type = '10P' or type = '100') " +
                        "union " +
                        "select acode icode, (to_number(max(qtyin)) + to_number(max(qtyrw))) qtyin, 0 qtyout, stage1 fstg, type, client_id, client_unit_id," +
                        "vch_num, vch_date from iprod where type = '100' group by acode, stage1, type, client_id, client_unit_id ,vch_num,vch_date " +
                        "union " +
                        "select acode icode, to_number(max(qtyrej)) qtyin, 0 qtyout, '101' fstg, type, client_id, client_unit_id,vch_num,vch_date from iprod " +
                        "where type = '100' group by acode, type, client_id, client_unit_id,vch_num,vch_date " +
                        "union " +
                        "select icode, sum(qtyin) qtyin, 0 qtyout, stage1 fstg, type, client_id, client_unit_id, vch_num, vch_date from iprod where " +
                        "type = '10R' group by icode, stage1, type, client_id, client_unit_id,vch_num,vch_date " +
                        "union " +
                        "select icode, 0 qtyin, sum(qtyout) qtyout, stage fstg, type, client_id, client_unit_id, vch_num, vch_date from iprod where " +
                        "type = '10R' group by icode, stage, type, client_id, client_unit_id,vch_num,vch_date " +
                        "union " +
                        "select icode, sum(qtyout) qtyin, 0 qtyout, stage1 fstg, type, client_id, client_unit_id, vch_num, vch_date from iprod where " +
                        "type = '10R' group by icode, stage1, type, client_id, client_unit_id,vch_num,vch_date) a " +
                        "on a.icode = i.icode and find_in_set(a.client_unit_id, i.client_unit_id)= 1 " +
                        "inner join master_setting um on um.master_id = i.uom and um.type = 'UMM' and find_in_set(um.client_unit_id, i.client_unit_id)= 1 " +
                        "inner join master_setting st on st.master_id = nvl(a.fstg, '001') and st.type = 'KPS' " +
                        "where i.client_unit_id = '"+unitid_mst+"' and i.type = 'IT' and i.icode = '" + model[0].Col17 + "' and a.fstg = '" + model[0].SelectedItems1.FirstOrDefault() + "' " +
                        "group by i.icode,i.iname,um.master_name,nvl(a.fstg, '001'),st.master_name";
                    DataTable dts = new DataTable();
                    dts = sgen.getdata(userCode, mq);
                    if (dts.Rows.Count > 0) { model[0].Col20 = dts.Rows[0]["qtystk"].ToString(); }
                    else model[0].Col20 = "0";
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
                            mq1 = " and vch_num<>'" + model[0].Col16 + "'";
                            isedit = true;
                            vch_num = model[0].Col16;
                        }
                        else
                        {
                            mq1 = "";
                            isedit = false;
                            mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12.Trim() + "'" + model[0].Col11.Trim() + "";
                            vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
                            model[0].Col16 = vch_num;
                        }
                        //mq = "select acode from itransactionc where type='" + model[0].Col12 + "'" + model[0].Col11 + " and acode='" + model[0].Col19 + "'" + mq1 + "";
                        //cond = sgen.seekval(userCode, mq, "acode");
                        //if (cond != "0")
                        //{
                        //    ViewBag.vnew = "disabled='disabled'";
                        //    ViewBag.vedit = "disabled='disabled'";
                        //    ViewBag.vsave = "";
                        //    ViewBag.vsavenew = "";
                        //    ViewBag.scripCall += "showmsgJS(1,'Data already exist for this acode', 2);";
                        //}
                        DataTable dataTable = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                        DataRow dr = dataTable.NewRow();
                        dr["type"] = model[0].Col12;
                        dr["vch_num"] = vch_num.Trim();
                        dr["vch_date"] = currdate;
                        dr["icode"] = model[0].Col17;//from icode
                        dr["iname"] = model[0].Col19;
                        dr["stage"] = model[0].SelectedItems1.FirstOrDefault();
                        dr["qtystk"] = model[0].Col20;
                        dr["qtyin"] = model[0].Col21;
                        dr["qtyout"] = model[0].Col21;
                        dr["qtychl"] = model[0].Col21;
                        dr["acode"] = model[0].Col22;//to icode
                        dr["stage1"] = model[0].SelectedItems2.FirstOrDefault();
                        dr["mstage"] = model[0].SelectedItems2.FirstOrDefault();
                        dr["pflag"] = "T";
                        dr = getsave(currdate, dr, model, isedit);
                        dataTable.Rows.Add(dr);
                        res = sgen.Update_data(userCode, dataTable, model[0].Col10.Trim(), tmodel.Col8, isedit);
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
                                Col100 = "Save & New",
                                Col121 = "S",
                                Col122 = "<u>S</u>ave",
                                Col123 = "Save & Ne<u>w</u>",
                                TList1 = mod1,
                                TList2 = mod2,
                                SelectedItems1 = new string[] { "" },
                                SelectedItems2 = new string[] { "" }
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
                                    model = new_stgtransfer(model);
                                    ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                                }
                                catch (Exception ex) { }
                            }
                        }
                    }
                    catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString() + "', 0);"; }
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
        #region mlditm
        public ActionResult mlditm()
        {
            MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]);
            mid_mst = EncryptDecrypt.Decrypt(Request.QueryString["mid"]);
            FillMst(MyGuid);
            chkRef();
            ViewBag.vnew = "";
            ViewBag.vedit = "";
            ViewBag.vsave = "disabled='disabled'";
            ViewBag.scripCall = "disableForm();";
            List<Tmodelmain> model = new List<Tmodelmain>();
            Tmodelmain tm1 = new Tmodelmain();
            List<SelectListItem> mod1 = new List<SelectListItem>();
            tm1.Col9 = "MOULD WISE ITEM";
            tm1.Col10 = "enx_tab";
            tm1.Col11 = " and client_id = '" + clientid_mst + "' and client_unit_id = '" + unitid_mst + "'";
            tm1.Col12 = "IMD";
            tm1.Col13 = "Save";
            tm1.Col100 = "Save & New";
            tm1.Col121 = "S";
            tm1.Col122 = "<u>S</u>ave";
            tm1.Col123 = "Save & Ne<u>w</u>";
            tm1.Col14 = mid_mst.Trim();
            tm1.Col15 = MyGuid.Trim();
            model.Add(tm1);
            return View(model);
        }
        public List<Tmodelmain> new_mlditm(List<Tmodelmain> model)
        {
            sgen.SetSession(MyGuid, "EDMODE", "N");
            ViewBag.vnew = "disabled='disabled'";
            ViewBag.vedit = "disabled='disabled'";
            ViewBag.vsave = "";
            ViewBag.vsavenew = "";
            ViewBag.scripCall = "enableForm();";
            mq = "select max(to_number(vch_num)) as auto_genid from " + model[0].Col10.Trim() + " a where type='" + model[0].Col12 + "'" + model[0].Col11.Trim() + "";
            vch_num = sgen.genNo(userCode, mq, 6, "auto_genid");
            model[0].Col16 = vch_num;
            mq = "select '-' as SNo,'-' as ICode,'-' as IName from dual";
            DataTable dtn = sgen.getdata(userCode, mq);
            model[0].dt1 = dtn;
            sgen.SetSession(MyGuid, "KMLDITM", dtn);
            return model;
        }

        [HttpPost]
        public ActionResult mlditm(List<Tmodelmain> model, string command, string hfrow, string hftable)
        {
            try
            {
                FillMst(model[0].Col15);
                DataTable dtm = sgen.settable(hftable);
                model[0].dt1 = dtm;
                var tmodel = model.FirstOrDefault();
                DataTable dt = new DataTable();
                if (command == "Cancel") { return CancelFun(model); }
                else if (command == "Callback") { model = StartCallback(model); }
                else if (command == "New")
                {
                    model = new_mlditm(model);
                }
                else if (command == "Save" || command == "Update" || command == "Save & New" || command == "Update & New")
                {
                    try
                    {
                        if (model[0].dt1.Rows[0]["icode"].ToString().Trim().Equals("-"))
                        {
                            ViewBag.scripCall += "showmsgJS(1,'Please Select item in grid', 2);";
                            ViewBag.vnew = "disabled='disabled'";
                            ViewBag.vedit = "disabled='disabled'";
                            ViewBag.vsave = "";
                            ViewBag.vsavenew = "";
                            return View(model);
                        }
                        type = model[0].Col12.Trim();
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
                        dt = cmd_Fun.GetStructure(userCode, model[0].Col10.Trim());
                        string[] mlds = model[0].Col18.Split(',');
                        for (int i = 0; i < mlds.Length; i++)
                        {
                            for (int k = 0; k < model[0].dt1.Rows.Count; k++)
                            {
                                DataRow dr = dt.NewRow();
                                dr["vch_num"] = vch_num;
                                dr["type"] = type;
                                dr["vch_date"] = currdate;
                                dr["col2"] = model[0].dt1.Rows[k]["Icode"].ToString();
                                dr["col1"] = model[0].dt1.Rows[k]["IName"].ToString();
                                dr["col3"] = mlds[i].ToString();
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
                                dt.Rows.Add(dr);
                            }
                        }
                        res = sgen.Update_data(userCode, dt, model[0].Col10.Trim(), model[0].Col8, isedit);
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
                                dt1 = null
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
                                    model = new_mlditm(model);
                                    ViewBag.scripCall += "mytoast('success', 'toast-top-right', 'Data Saved Successfully');";
                                }
                                catch (Exception ex) { }
                            }

                        }
                        else
                        {
                            ViewBag.vnew = "disabled='disabled'";
                            ViewBag.vedit = "disabled='disabled'";
                            ViewBag.vsave = "";
                            ViewBag.vsavenew = "";
                            ViewBag.scripCall += "showmsgJS(1, 'Data Not Saved', 0);";
                        }
                    }
                    catch (Exception ex) { ViewBag.scripCall = "showmsgJS(1,'" + ex.Message.ToString().Replace('\'', ' ') + "', 0);"; }
                }
                else if (command == "-")
                {
                    if (model[0].dt1.Rows.Count > 1) model[0].dt1.Rows.RemoveAt(sgen.Make_int(hfrow));
                    else
                    {
                        model[0].dt1 = (DataTable)sgen.GetSession(MyGuid, "KMLDITM");
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
                ViewBag.scripCall += "showmsgJS(1, '" + err.Message.Replace("'", "") + " At Line number " + LineNumber + "', 0);";
            }
            ModelState.Clear();
            return View(model);
        }
        #endregion
    }
}