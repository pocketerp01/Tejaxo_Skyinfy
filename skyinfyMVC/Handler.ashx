<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Globalization;

public class Handler : IHttpHandler, IRequiresSessionState
{
    string userCode = "", userid_mst = "", clientid_mst = "", unitid_mst = "", role_mst = "";
    static string MyGuid = "",mid="";
    string mq = "", txtclr = "";
    public void ProcessRequest(HttpContext context)
    {
        try
        {
            MyGuid = context.Request.QueryString["m_id"].ToString();
            mid = context.Request.QueryString["mid"].ToString();
            sgenFun sgen = new sgenFun(MyGuid);
            userCode = sgen.GetCookie(MyGuid, "userCode");
            if (context.Request.UrlReferrer == null)
            {
                context.Response.Redirect("~/erp/login_main.aspx");
                return;
            }
            if (userCode.Equals(""))
            {
                context.Response.Redirect("~/erp/login_main.aspx");
                return;
            }

            userid_mst = sgen.GetCookie(MyGuid, "userid_mst");
            clientid_mst = sgen.GetCookie(MyGuid, "clientid_mst");
            unitid_mst = sgen.GetCookie(MyGuid, "unitid_mst");
            role_mst = sgen.GetCookie(MyGuid, "role_mst");

            System.Xml.XmlDocument xdoc = new System.Xml.XmlDocument();
            System.Xml.XmlNode monthly = xdoc.CreateElement("monthly");
            xdoc.AppendChild(monthly);

            //crm follow up
            #region crm follow up

            System.Data.DataTable datatablef = new System.Data.DataTable();
            string entBy = sgen.getstring(userCode, "select Ent_By from user_Details where vch_num='" + Convert.ToInt32(userid_mst) + "'");
            string cond = "";
            //if (!role_mst.ToUpper().Equals("OWNER")) cond = "and( a.ent_by='" + userid_mst + "' or l.ct_person='"+userid_mst+"' )";
            //if (!role_mst.ToUpper().Equals("OWNER")) cond = "and( a.ent_by='" + userid_mst + "' or a.col5='"+userid_mst+"' )";

            if (!role_mst.ToUpper().Equals("OWNER")) cond = "and( l.ent_by='" + userid_mst + "' or a.col5='"+userid_mst+"' )";
            string currdate = sgen.server_datetime_dt(userCode).ToString(sgen.Getdateformat());
            {
                System.Data.DataTable dt = new System.Data.DataTable();


                mq = "select (l.client_id||l.client_unit_id||l.vch_num||to_char(l.vch_date,'yyyymmdd')||l.type) as fstr," +
                    "  a.col2 as lead_id,ud.first_name || ' ' || replace(ud.middle_name, '0', '') || '' || replace(ud.last_name, '0', '')as assign," +
                     "(case when l.cust_id=0 then l.cust_name else cp.c_name  end)||(case when cp.cplandno='0000000000000' or " +
                     "cp.cplandno='0000000000' or cp.cplandno IS NULL then '' else '-' end) ||(case when cp.cplandno='0000000000000'" +
                     " or cp.cplandno='0000000000' then '' else cplandno end) acc_dtl,a.vch_num," +
                     "to_char(convert_tz(a.date2,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') as date2 ," +
                     " n.master_name as nxt_fl from enx_tab2 a inner join lead_master l on l.vch_num=a.col2 and l.type='LED' and l.lead_con='N' and " +
                     "a.client_unit_id= l.client_unit_id  left join master_setting n " +
                     "on n.master_id = a.col7 and n.type = 'NAM' and find_in_set(n.client_unit_id, a.client_unit_id)= 1 left join " +
                     "clients_mst cp on cp.vch_num=l.cust_id and find_in_set(cp.client_unit_id,'" + unitid_mst + "') =1 " +
                     "left join user_details ud on ud.vch_num = a.col5 where a.type = 'VFC' and a.client_unit_id='" + unitid_mst + "' and a.vch_num in (select max(vch_num) from enx_tab2" +
                     " where type='VFC' and client_unit_id in (" + unitid_mst + ") group by col2)  "+cond+"";


                string mq1 = "select (l.client_id||l.client_unit_id||l.vch_num||to_char(l.vch_date,'yyyymmdd')||l.type) as fstr," +
              "  a.col2 as lead_id,ud.first_name || ' ' || replace(ud.middle_name, '0', '') || '' || replace(ud.last_name, '0', '')as assign," +
               "(case when l.cust_id=0 then l.cust_name else cp.c_name  end)||(case when cp.cplandno='0000000000000' or " +
               "cp.cplandno='0000000000' or cp.cplandno IS NULL then '' else '-' end) ||(case when cp.cplandno='0000000000000'" +
               " or cp.cplandno='0000000000' then '' else cplandno end) acc_dtl,a.vch_num," +
               "to_char(convert_tz(a.date2,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') as date2 ," +
               " n.master_name as nxt_fl from enx_tab2 a inner join lead_master l on l.vch_num=a.col2 and l.type='LED' and l.lead_con='C' and to_date(to_char(a.vch_date,'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldateformat() + "') = to_date('" + currdate+ "','" + sgen.Getsqldateformat() + "') and " +
               "a.client_unit_id= l.client_unit_id  left join master_setting n " +
               "on n.master_id = a.col7 and n.type = 'NAM' and find_in_set(n.client_unit_id, a.client_unit_id)= 1 left join " +
               "clients_mst cp on cp.vch_num=l.cust_id and find_in_set(cp.client_unit_id,'" + unitid_mst + "') =1 " +
               "left join user_details ud on ud.vch_num = a.col5 where a.type = 'VFC' and a.client_unit_id='" + unitid_mst + "' and a.vch_num in (select max(vch_num) from enx_tab2" +
               " where type='VFC' and client_unit_id in (" + unitid_mst + ") group by col2)  "+cond+" ";

                mq = mq + " union  " + mq1;

                dt = sgen.getdata(userCode, mq);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var des = xdoc.CreateElement("event");

                        var id = xdoc.CreateElement("id");
                        id.InnerText = Convert.ToString(dt.Rows[i]["lead_id"]);
                        des.AppendChild(id);

                        var name = xdoc.CreateElement("name");
                        // name.InnerText =  "Lead No:- ("+Convert.ToString(dt.Rows[i]["lead_id"]) +") -"+ Convert.ToString(dt.Rows[i]["acc_dtl"])+'-'+ Convert.ToString(dt.Rows[i]["nxt_fl"])+ " (Assign To " +Convert.ToString(dt.Rows[i]["assign"]) +")";
                        name.InnerText =   Convert.ToString(dt.Rows[i]["acc_dtl"]) +'-'+" (Assign To " +Convert.ToString(dt.Rows[i]["assign"]) +")" +'-'+ Convert.ToString(dt.Rows[i]["nxt_fl"])+'-'+ Convert.ToString(dt.Rows[i]["lead_id"]);
                        des.AppendChild(name);
                        var startdate = xdoc.CreateElement("startdate");
                        startdate.InnerText = DateTime.ParseExact(dt.Rows[i]["date2"].ToString(), sgen.Getdateformat(), CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                        des.AppendChild(startdate);

                        var enddate = xdoc.CreateElement("enddate");
                        enddate.InnerText = DateTime.ParseExact(dt.Rows[i]["date2"].ToString(), sgen.Getdateformat(), CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                        des.AppendChild(enddate);

                        //var not_id = EncryptDecrypt.Encrypt(dt.Rows[i]["Notification_Id"].ToString());

                        var url = xdoc.CreateElement("url");
                        // url.InnerText = "../../../../../erp/Notification_detail.aspx?INId=" + EncryptDecrypt.Encrypt(dt.Rows[i]["vch_num"].ToString());
                        //url.InnerText = "../vastu/lead_detail?fstr=" + EncryptDecrypt.Encrypt(dt.Rows[i]["fstr"].ToString());
                        url.InnerText = "../vastu/lead_detail?fstr=" + EncryptDecrypt.Encrypt(dt.Rows[i]["fstr"].ToString());

                        des.AppendChild(url);


                        var color = xdoc.CreateElement("color");
                        //color.InnerText = "#000080";
                        color.InnerText = "#008000";
                        des.AppendChild(color);

                        monthly.AppendChild(des);
                    }
                }

            }
            #endregion


            #region Ticket follow up

            datatablef = new System.Data.DataTable();
            entBy = sgen.getstring(userCode, "select Ent_By from user_Details where vch_num='" + Convert.ToInt32(userid_mst) + "'");
            cond = "";
            //if (!role_mst.ToUpper().Equals("OWNER")) cond = "and( a.ent_by='" + userid_mst + "' or l.ct_person='"+userid_mst+"' )";
            //if (!role_mst.ToUpper().Equals("OWNER")) cond = "and( a.ent_by='" + userid_mst + "' or a.col5='"+userid_mst+"' )";

            if (!role_mst.ToUpper().Equals("OWNER")) cond = "and( l.ent_by='" + userid_mst + "' or a.col5='"+userid_mst+"' )";
            currdate = sgen.server_datetime_dt(userCode).ToString(sgen.Getdateformat());
            {
                System.Data.DataTable dt = new System.Data.DataTable();


                mq = "select (l.client_id||l.client_unit_id||l.vch_num||to_char(l.vch_date,'yyyymmdd')||l.type) as fstr," +
                    "  a.col2 as lead_id,ud.first_name || ' ' || replace(ud.middle_name, '0', '') || '' || replace(ud.last_name, '0', '')as assign," +
                     "(case when l.cust_id=0 then l.cust_name else cp.c_name  end)||(case when cp.cplandno='0000000000000' or " +
                     "cp.cplandno='0000000000' or cp.cplandno IS NULL then '' else '-' end) ||(case when cp.cplandno='0000000000000'" +
                     " or cp.cplandno='0000000000' then '' else cplandno end) acc_dtl,a.vch_num," +
                     "to_char(convert_tz(a.date2,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') as date2 ," +
                     " n.master_name as nxt_fl from enx_tab2 a inner join lead_master l on l.vch_num=a.col2 and l.type='TCK' and l.lead_con='N' and " +
                     "a.client_unit_id= l.client_unit_id  left join master_setting n " +
                     "on n.master_id = a.col7 and n.type = 'NAM' and find_in_set(n.client_unit_id, a.client_unit_id)= 1 left join " +
                     "clients_mst cp on cp.vch_num=l.cust_id and find_in_set(cp.client_unit_id,'" + unitid_mst + "') =1 " +
                     "left join user_details ud on ud.vch_num = a.col5 where a.type = 'TCK' and a.client_unit_id='" + unitid_mst + "' and a.vch_num in (select max(vch_num) from enx_tab2" +
                     " where type='TCK' and client_unit_id in (" + unitid_mst + ") group by col2)  "+cond+"";


                string mq1 = "select (l.client_id||l.client_unit_id||l.vch_num||to_char(l.vch_date,'yyyymmdd')||l.type) as fstr," +
              "  a.col2 as lead_id,ud.first_name || ' ' || replace(ud.middle_name, '0', '') || '' || replace(ud.last_name, '0', '')as assign," +
               "(case when l.cust_id=0 then l.cust_name else cp.c_name  end)||(case when cp.cplandno='0000000000000' or " +
               "cp.cplandno='0000000000' or cp.cplandno IS NULL then '' else '-' end) ||(case when cp.cplandno='0000000000000'" +
               " or cp.cplandno='0000000000' then '' else cplandno end) acc_dtl,a.vch_num," +
               "to_char(convert_tz(a.date2,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') as date2 ," +
               " n.master_name as nxt_fl from enx_tab2 a inner join lead_master l on l.vch_num=a.col2 and l.type='TCK' and l.lead_con='C' and to_date(to_char(a.vch_date,'" + sgen.Getsqldateformat() + "'),'" + sgen.Getsqldateformat() + "') = to_date('" + currdate+ "','" + sgen.Getsqldateformat() + "') and " +
               "a.client_unit_id= l.client_unit_id  left join master_setting n " +
               "on n.master_id = a.col7 and n.type = 'NAM' and find_in_set(n.client_unit_id, a.client_unit_id)= 1 left join " +
               "clients_mst cp on cp.vch_num=l.cust_id and find_in_set(cp.client_unit_id,'" + unitid_mst + "') =1 " +
               "left join user_details ud on ud.vch_num = a.col5 where a.type = 'TCK' and a.client_unit_id='" + unitid_mst + "' and a.vch_num in (select max(vch_num) from enx_tab2" +
               " where type='TCK' and client_unit_id in (" + unitid_mst + ") group by col2)  "+cond+" ";

                mq = mq + " union  " + mq1;

                dt = sgen.getdata(userCode, mq);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var des = xdoc.CreateElement("event");

                        var id = xdoc.CreateElement("id");
                        id.InnerText = Convert.ToString(dt.Rows[i]["lead_id"]);
                        des.AppendChild(id);

                        var name = xdoc.CreateElement("name");

                        name.InnerText =   Convert.ToString(dt.Rows[i]["acc_dtl"]) +'-'+" (Assign To " +Convert.ToString(dt.Rows[i]["assign"]) +")" +'-'+ Convert.ToString(dt.Rows[i]["nxt_fl"])+'-'+ Convert.ToString(dt.Rows[i]["lead_id"]);
                        des.AppendChild(name);
                        var startdate = xdoc.CreateElement("startdate");
                        startdate.InnerText = DateTime.ParseExact(dt.Rows[i]["date2"].ToString(), sgen.Getdateformat(), CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                        des.AppendChild(startdate);

                        var enddate = xdoc.CreateElement("enddate");
                        enddate.InnerText = DateTime.ParseExact(dt.Rows[i]["date2"].ToString(), sgen.Getdateformat(), CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                        des.AppendChild(enddate);



                        var url = xdoc.CreateElement("url");


                        url.InnerText = "../vastu/ticket_detail?fstr=" + EncryptDecrypt.Encrypt(dt.Rows[i]["fstr"].ToString());

                        des.AppendChild(url);


                        var color = xdoc.CreateElement("color");

                        color.InnerText = "#8f008f";
                        des.AppendChild(color);

                        monthly.AppendChild(des);
                    }
                }

            }
            #endregion

            #region Appointments

            datatablef = new System.Data.DataTable();
            entBy = sgen.getstring(userCode, "select Ent_By from user_Details where vch_num='" + Convert.ToInt32(userid_mst) + "'");
            cond = "";

            if (!role_mst.ToUpper().Equals("OWNER")) cond = "and( l.ent_by='" + userid_mst + "' or a.col5='"+userid_mst+"' )";
            currdate = sgen.server_datetime_dt(userCode).ToString(sgen.Getdateformat());
            {
                System.Data.DataTable dt = new System.Data.DataTable();



                mq = " Select  group_concat( ud.first_name || ' ' || replace(ud.middle_name, '0', '') || '' || replace(ud.last_name, '0', '')) as attend_by," +
                    "(a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr,  a.vch_num doc_no," +
                                "to_char(a.vch_date, '" + sgen.Getsqldateformat() + "') doc_date,replace(nvl(pr.c_name, '-'), 0, '-') as Parent_Account,b.vch_num as Account_No, " +
                                " b.c_name as Organisation_Name,((replace(ct.cpname, '0', '-') || ' ' || replace(ct.cp_mname, '0', '') || ' ' || replace(ct.cp_lname, '0', ''))) " +
                                "cp_name,a.col7 Related_To, a.col3 title,( case when a.col12 = 'Y' then 'YES' else 'No' end) Full_Day, " +
                                " ( case when a.col12='Y' then to_char(a.date1 ,'" + sgen.Getsqldateformat() + "') else  to_char(a.date1,'" + sgen.Getsqldatetimeformat() + "') end) from_date, " +
                                " ( case when a.col12='Y' then to_char(a.date2 ,'" + sgen.Getsqldateformat() + "') else  to_char(a.date2,'" + sgen.Getsqldatetimeformat() + "') end) to_date" +
                                " from enx_tab2 a inner join clients_mst b on b.vch_num = a.Col5 and" +
                                " find_in_set(b.client_unit_id,'" + unitid_mst + "')= 1 left join clients_mst pr" +
                                " on b.parent_id = pr.vch_num  and find_in_set(pr.client_unit_id,'" + unitid_mst + "')= 1 and pr.vch_num!='0' " +
                                " left join clients_mst ct  on a.col5=ct.ref_code and a.col6=ct.vch_num  and find_in_set(ct.client_unit_id,'" + unitid_mst + "')= 1" +
                                "  inner join user_details ud on find_in_set (a.col9,ud.vch_num)=1 and ud.type='CPR'" +
                                " where a.type = 'APP' and a.client_unit_id = '" + unitid_mst + "'" +
                                " group by(a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type)" +
                                " ,a.vch_num ,to_char(a.vch_date, '"+sgen.Getsqldateformat()+"'),replace(nvl(pr.c_name, '-'), 0, '-'),b.vch_num,  b.c_name " +
                                " ,((replace(ct.cpname, '0', '-') || ' ' || replace(ct.cp_mname, '0', '') || ' ' || replace(ct.cp_lname, '0', '')))" +
                                ",a.col7,a.col3,( case when a.col12='Y' then to_char(a.date1 ,'"+sgen.Getsqldateformat()+"') else " +
                                " to_char(a.date1,'"+sgen.Getsqldatetimeformat()+"') end),( case when a.col12='Y' then to_char(a.date2 ,'dd/MM/YYYY') else " +
                                " to_char(a.date2,'"+sgen.Getsqldatetimeformat()+"') end),( case when a.col12 = 'Y' then 'YES' else 'No' end)" +
                                " order by a.vch_num desc ";




                dt = sgen.getdata(userCode, mq);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var des = xdoc.CreateElement("event");

                        var id = xdoc.CreateElement("id");
                        id.InnerText = Convert.ToString(dt.Rows[i]["doc_no"]);
                        des.AppendChild(id);

                        var name = xdoc.CreateElement("name");

                        // name.InnerText =   Convert.ToString(dt.Rows[i]["Organisation_name"]) +'-'+" (Assign To " +Convert.ToString(dt.Rows[i]["assign"]) +")" +'-'+ Convert.ToString(dt.Rows[i]["nxt_fl"])+'-'+ Convert.ToString(dt.Rows[i]["lead_id"]);
                        name.InnerText =   Convert.ToString(dt.Rows[i]["Organisation_name"]) +'-'+" (Contact Person " +Convert.ToString(dt.Rows[i]["cp_name"]) +")" +'-'+" (Attend By " +Convert.ToString(dt.Rows[i]["Attend_By"]) +")" +'-'+  Convert.ToString(dt.Rows[i]["Doc_no"]);
                        des.AppendChild(name);
                        var startdate = xdoc.CreateElement("startdate");
                        var enddate = xdoc.CreateElement("enddate");
                        if (dt.Rows[i]["Full_Day"].ToString() == "No")
                        {
                        startdate.InnerText = DateTime.ParseExact(dt.Rows[i]["From_date"].ToString(), sgen.Getdatetimeformat(), CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH24:MI:ss");
                        des.AppendChild(startdate);


                        enddate.InnerText = DateTime.ParseExact(dt.Rows[i]["From_date"].ToString(), sgen.Getdatetimeformat(), CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH24:MI:ss");
                        des.AppendChild(enddate);
                        }
                        else
                        {
                            startdate.InnerText = DateTime.ParseExact(dt.Rows[i]["From_date"].ToString(), sgen.Getdateformat(), CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                            des.AppendChild(startdate);


                            enddate.InnerText = DateTime.ParseExact(dt.Rows[i]["From_date"].ToString(), sgen.Getdateformat(), CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                            des.AppendChild(enddate);
                        }



                        var url = xdoc.CreateElement("url");


                        url.InnerText = "../vastu/appointment?fstr=" + EncryptDecrypt.Encrypt(dt.Rows[i]["fstr"].ToString());

                        des.AppendChild(url);


                        var color = xdoc.CreateElement("color");

                        color.InnerText = "#008f8f";
                        des.AppendChild(color);

                        monthly.AppendChild(des);
                    }
                }

            }
            #endregion
            //notifications
            #region notification

            System.Data.DataTable dtn = new System.Data.DataTable();
            mq = "select a.vch_num,a.title,to_char(convert_tz(a.fdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') fdt," +
            "to_char(convert_tz(a.tdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') tdt," +
            "to_char(convert_tz(a.pdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') pdt," +
            "a.descp from Notifications a " +
            "where a.type='NOT' and a.client_unit_id='" + unitid_mst + "'";
            dtn = sgen.getdata(userCode, mq);
            if (dtn.Rows.Count > 0)
            {
                for (int i = 0; i < dtn.Rows.Count; i++)
                {
                    var des = xdoc.CreateElement("event");

                    var id = xdoc.CreateElement("id");
                    id.InnerText = Convert.ToString(dtn.Rows[i]["vch_num"]);
                    des.AppendChild(id);

                    var name = xdoc.CreateElement("name");
                    name.InnerText = Convert.ToString(dtn.Rows[i]["title"]);
                    des.AppendChild(name);

                    var startdate = xdoc.CreateElement("startdate");
                    startdate.InnerText = DateTime.ParseExact(dtn.Rows[i]["pdt"].ToString(), sgen.Getdateformat(), CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                    des.AppendChild(startdate);

                    var enddate = xdoc.CreateElement("enddate");
                    enddate.InnerText = DateTime.ParseExact(dtn.Rows[i]["pdt"].ToString(), sgen.Getdateformat(), CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                    des.AppendChild(enddate);

                    var not_id = EncryptDecrypt.Encrypt(dtn.Rows[i]["vch_num"].ToString());

                    var url = xdoc.CreateElement("url");
                    url.InnerText = "../Home/notelist?m_id=" + EncryptDecrypt.Encrypt(MyGuid) + "&mid=" + EncryptDecrypt.Encrypt(mid) + "";
                    des.AppendChild(url);

                    var color = xdoc.CreateElement("color");
                    color.InnerText = "tomato";
                    des.AppendChild(color);

                    monthly.AppendChild(des);
                }
            }
            //}
            #endregion

            //my todolist
            #region todolist

            System.Data.DataTable datatable1 = new System.Data.DataTable();
            if (role_mst.Trim().ToUpper().Equals("OWNER"))
            {
                mq = "select ud.first_name || ' ' || replace(ud.middle_name, '0', '') || '' || replace(ud.last_name, '0', '') as user_name, a.vch_num,a.title," +
                    "to_char(convert_tz(a.fdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') fdt,ud.role,nvl(ud.team,'0')," +
                    "to_char(convert_tz(a.tdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') tdt,a.descp,a.ent_by from Notifications a " +
                    "inner join user_details ud on ud.vch_num = a.ent_by and ud.type='CPR' " +
                    "where (a.ent_by='" + userid_mst + "' or ud.team='" + userid_mst + "') and a.type='TDL'";
            }
            else
            {
                mq = "select ud.first_name || ' ' || replace(ud.middle_name, '0', '') || '' || replace(ud.last_name, '0', '') as user_name, " +
                    "a.vch_num,a.title,to_char(convert_tz(a.fdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') fdt,ud.role," +
                    "to_char(convert_tz(a.tdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') tdt,a.descp,a.ent_by from Notifications a " +
                    "inner join user_details ud on ud.vch_num = a.ent_by and ud.type='CPR' " +
                    "where a.ent_by='" + userid_mst + "' and a.type='TDL'";
            }
            datatable1 = sgen.getdata(userCode, mq);
            if (datatable1.Rows.Count > 0)
            {
                for (int i = 0; i < datatable1.Rows.Count; i++)
                {
                    var des = xdoc.CreateElement("event");

                    var id = xdoc.CreateElement("id");
                    id.InnerText = Convert.ToString(datatable1.Rows[i]["vch_num"]);
                    des.AppendChild(id);

                    var name = xdoc.CreateElement("name");
                    name.InnerText =  "("+ Convert.ToString(datatable1.Rows[i]["user_name"])+" )"+Convert.ToString(datatable1.Rows[i]["title"]);
                    des.AppendChild(name);

                    var startdate = xdoc.CreateElement("startdate");
                    startdate.InnerText = DateTime.ParseExact(datatable1.Rows[i]["fdt"].ToString(), sgen.Getdateformat(), CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                    des.AppendChild(startdate);

                    var enddate = xdoc.CreateElement("enddate");
                    enddate.InnerText = DateTime.ParseExact(datatable1.Rows[i]["tdt"].ToString(), sgen.Getdateformat(), CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                    des.AppendChild(enddate);

                    var color = xdoc.CreateElement("color");
                    if (datatable1.Rows[i]["role"].ToString().Trim().ToLower().Equals("owner")) txtclr = "#21618C";
                    else txtclr = "#5499C7";
                    color.InnerText = txtclr;
                    des.AppendChild(color);

                    var todolist_id = EncryptDecrypt.Encrypt(datatable1.Rows[i]["vch_num"].ToString());

                    var url = xdoc.CreateElement("url");
                    url.InnerText = "../home/task_list2?m_id=" + EncryptDecrypt.Encrypt(MyGuid) + "&mid=" + EncryptDecrypt.Encrypt(mid) + "";
                    des.AppendChild(url);

                    monthly.AppendChild(des);
                }
            }

            #endregion

            //System.Data.DataTable datatable2 = new System.Data.DataTable();
            //datatable2 = sgen.getdata(userCode, "select course_id,cou_title,date_format(convert_tz(start_date,'+0:00','" + sgen.Gettimezone() + "'),'"
            //    + sgen.Getsqldateformat() + "') as start_date,date_format(convert_tz(end_date,'+0:00','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') as end_date " +
            //    "from usercourse where user_id='" + userid_mst + "' and usercourse_status='assigned' and training_status='" + "Unopened" + "'");

            //if (datatable2.Rows.Count > 0)
            //{
            //    for (int i = 0; i < datatable2.Rows.Count; i++)
            //    {
            //        var des = xdoc.CreateElement("event");

            //        var id = xdoc.CreateElement("id");
            //        id.InnerText = Convert.ToString(datatable2.Rows[i]["course_id"]);
            //        des.AppendChild(id);

            //        var name = xdoc.CreateElement("name");
            //        name.InnerText = Convert.ToString(datatable2.Rows[i]["cou_title"]);
            //        des.AppendChild(name);

            //        var startdate = xdoc.CreateElement("startdate");
            //        startdate.InnerText = DateTime.ParseExact(datatable1.Rows[i]["start_date"].ToString(), sgen.Getdateformat(), CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            //        des.AppendChild(startdate);

            //        var enddate = xdoc.CreateElement("enddate");
            //        enddate.InnerText = DateTime.ParseExact(datatable1.Rows[i]["end_date"].ToString(), sgen.Getdateformat(), CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            //        des.AppendChild(enddate);

            //        var color = xdoc.CreateElement("color");
            //        color.InnerText = "#EDBB99";
            //        des.AppendChild(color);

            //        // var todolist_id = EncryptDecrypt.Encrypt(datatable1.Rows[i]["todolist_Id"].ToString());

            //        var url = xdoc.CreateElement("url");
            //        url.InnerText = "../../../../../erp/corp_admin/corp_adm_my_training.aspx";
            //        des.AppendChild(url);

            //        monthly.AppendChild(des);
            //    }
            //}



            //System.Data.DataTable datatable3 = new System.Data.DataTable();
            //datatable3 = sgen.getdata(userCode, "select ques_paper_id,quespaper_name,date_format(convert_tz(quespaper_startdt,'+0:00','"
            //    + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') as quespaper_startdt,date_format(convert_tz(quespaper_enddt,'+0:00','"
            //    + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') as quespaper_enddt from user_quespaper where user_id='" + userid_mst + "' " +
            //    "and status='assigned' and quespaper_status='" + "not completed" + "'");

            //if (datatable3.Rows.Count > 0)
            //{

            //    for (int i = 0; i < datatable3.Rows.Count; i++)
            //    {
            //        var des = xdoc.CreateElement("event");

            //        var id = xdoc.CreateElement("id");
            //        id.InnerText = Convert.ToString(datatable3.Rows[i]["ques_paper_id"]);
            //        des.AppendChild(id);

            //        var name = xdoc.CreateElement("name");
            //        name.InnerText = Convert.ToString(datatable3.Rows[i]["quespaper_name"]);
            //        des.AppendChild(name);

            //        var startdate = xdoc.CreateElement("startdate");
            //        startdate.InnerText = DateTime.ParseExact(datatable1.Rows[i]["quespaper_startdt"].ToString(), sgen.Getdateformat(), CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            //        des.AppendChild(startdate);

            //        var enddate = xdoc.CreateElement("enddate");
            //        enddate.InnerText = DateTime.ParseExact(datatable1.Rows[i]["quespaper_enddt"].ToString(), sgen.Getdateformat(), CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            //        des.AppendChild(enddate);

            //        var color = xdoc.CreateElement("color");
            //        color.InnerText = "#E8DAEF";
            //        des.AppendChild(color);

            //        // var todolist_id = EncryptDecrypt.Encrypt(datatable1.Rows[i]["todolist_Id"].ToString());

            //        var url = xdoc.CreateElement("url");
            //        url.InnerText = "../../../../../erp/my_quespaper.aspx";
            //        des.AppendChild(url);

            //        monthly.AppendChild(des);
            //    }
            //}

            //conference room
            #region

            //System.Data.DataTable datatable4 = new System.Data.DataTable();
            //datatable3 = sgen.getdata(userCode, "select rec_id,booking_id,conf_roomid,conf_roomname,date_format(convert_tz(startdatetime,'+0:00','"
            //    + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') as startdatetime,date_format(convert_tz(enddatetime,'+0:00','"
            //    + sgen.Gettimezone() + "'),'" + sgen.Getsqldateformat() + "') as enddatetime,entby,meeting_type from bookings where user_id='" + userid_mst + "'");

            //if (datatable3.Rows.Count > 0)
            //{

            //    for (int i = 0; i < datatable3.Rows.Count; i++)
            //    {
            //        var des = xdoc.CreateElement("event");

            //        var id = xdoc.CreateElement("id");
            //        id.InnerText = Convert.ToString(datatable3.Rows[i]["booking_id"]);
            //        des.AppendChild(id);

            //        var name = xdoc.CreateElement("name");
            //        name.InnerText = Convert.ToString(datatable3.Rows[i]["meeting_type"]);
            //        des.AppendChild(name);

            //        var startdate = xdoc.CreateElement("startdate");
            //        startdate.InnerText = DateTime.ParseExact(datatable1.Rows[i]["startdatetime"].ToString(), sgen.Getdateformat(), CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            //        des.AppendChild(startdate);

            //        var enddate = xdoc.CreateElement("enddate");
            //        enddate.InnerText = DateTime.ParseExact(datatable1.Rows[i]["enddatetime"].ToString(), sgen.Getdateformat(), CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            //        des.AppendChild(enddate);

            //        var color = xdoc.CreateElement("color");
            //        color.InnerText = "#26b99a";
            //        des.AppendChild(color);

            //        var booking_id = EncryptDecrypt.Encrypt(datatable3.Rows[i]["rec_id"].ToString());

            //        var url = xdoc.CreateElement("url");
            //        url.InnerText = "../../../../../erp/calendar_conf_details.aspx?rec_Id=" + booking_id;
            //        //url.InnerText = "../../../../../erp/my_quespaper.aspx";
            //        des.AppendChild(url);

            //        monthly.AppendChild(des);
            //    }
            //}

            #endregion

            string xml = xdoc.InnerXml.ToString();
            // return xdoc;

            context.Response.Write(xml);

            //context.Response.Clear(); //Optional: if we've sent anything before
            context.Response.ContentType = "text/xml"; //Must be 'text/xml'
                                                       //context.Response.ContentEncoding = System.Text.Encoding.UTF8; //We'd like UTF-8
                                                       //xdoc.Save(context.Response.Output); //Save to the text-writer
                                                       //using the encoding of the text-writer
                                                       //(which comes from response.contentEncoding)Response.ContentEncoding = System.Text.Encoding.UTF8; //We'd like UTF-8
                                                       //xdoc.Save(context.Response.Output); //Save to the text-writer
                                                       //using the encoding of the text-writer
                                                       //(which comes from response.contentEncoding)
                                                       //context.Response.End(); 
        }
        catch (Exception ex)
        {
            sgenFun sgen = new sgenFun(MyGuid);
            sgen.showmsg(1, ex.Message.ToString(), 0);
            //context.Response.Redirect("~/login_main.aspx");
        }
    }


    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}