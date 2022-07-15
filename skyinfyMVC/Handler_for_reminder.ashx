<%@ WebHandler Language="C#" Class="Handler_for_reminder" %>

using System;
using System.Web;
using System.Globalization;

public class Handler_for_reminder : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    string userCode = "", userid_mst = "", clientid_mst = "", unitid_mst = "";
    static string MyGuid = "";

    public void ProcessRequest(HttpContext context)
    {
        try
        {
            MyGuid = context.Request.QueryString["m_id"].ToString();
            sgenFun sgen= new sgenFun(MyGuid);
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

            string callback = context.Request.QueryString["callback"];
            System.Collections.Generic.List<todolist> todo_list = new System.Collections.Generic.List<todolist>();
            System.Data.DataTable datatable = new System.Data.DataTable();
            //string entBy = sgen.getstring(userCode, "select Ent_By from user_Details where rec_id='" + Convert.ToInt32(userid_mst) + "'");
            //if (!entBy.Trim().Equals(""))
            //{
            //System.Data.DataTable dt1 = new System.Data.DataTable();
            //dt1 = sgen.getdata(userCode, "select vch_num,title,to_char(convert_tz(fdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') fdt," +
            //    "to_char(convert_tz(tdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') tdt,descp " +
            //    "from Notifications where type='NOT' and client_unit_id='" + unitid_mst + "'");
            //if (dt1.Rows.Count > 0)
            //{
            //    for (int i = 0; i < dt1.Rows.Count; i++)
            //    {
            //        DateTime from = DateTime.ParseExact(dt1.Rows[i]["fdt"].ToString(), sgen.Getdatetimeformat(), CultureInfo.InvariantCulture);
            //        DateTime currentdate = sgen.server_datetime_dt_local(userCode);
            //        DateTime currentdate1 = currentdate.AddMinutes(10);
            //        if (from < currentdate1 && from > currentdate)
            //        {
            //            todolist task = new todolist();
            //            task.title = Convert.ToString(dt1.Rows[i]["title"]);
            //            task.message = (dt1.Rows[i]["descp"]).ToString() + "\r\n" + dt1.Rows[i]["fdt"].ToString() + "\r\n" + dt1.Rows[i]["tdt"].ToString();
            //            task.priority = "warning";
            //            todo_list.Add(task);
            //        }
            //    }
            //}
            //}


            //mytodolist
            System.Data.DataTable dt = new System.Data.DataTable();
            dt = sgen.getdata(userCode, "select vch_num,title,to_char(convert_tz(fdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') fdt," +
          "to_char(convert_tz(tdt,'UTC','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') tdt,descp,ent_by from Notifications where " +
          "ent_by='" + userid_mst + "' and type='TDL'");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DateTime from = DateTime.ParseExact(dt.Rows[i]["fdt"].ToString(), sgen.Getdatetimeformat(), CultureInfo.InvariantCulture);
                DateTime currentdate = sgen.server_datetime_dt_local(userCode);
                DateTime currentdate1 = currentdate.AddMinutes(30);
                if (from < currentdate1 && from > currentdate)
                {
                    todolist task = new todolist();
                    task.title = Convert.ToString(dt.Rows[i]["title"]);
                    task.message = (dt.Rows[i]["descp"]).ToString() + "\r\n" + dt.Rows[i]["fdt"].ToString() + "\r\n" + dt.Rows[i]["tdt"].ToString();
                    task.priority = "warning";
                    todo_list.Add(task);
                }
            }

            //conference
            #region
            //System.Data.DataTable dt3 = new System.Data.DataTable();
            //dt3 = sgen.getdata(userCode, "select booking_id,conf_roomid,conf_roomname,date_format(convert_tz(startdatetime,'+0:00','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') as startdatetime,date_format(convert_tz(enddatetime,'+0:00','" + sgen.Gettimezone() + "'),'" + sgen.Getsqldatetimeformat() + "') as enddatetime,entby,meeting_type from bookings where user_id='" + userid_mst + "'");
            //for (int i = 0; i < dt3.Rows.Count; i++)
            //{
            //    DateTime from = DateTime.ParseExact(dt3.Rows[i]["startdatetime"].ToString(), sgen.Getdatetimeformat(), CultureInfo.InvariantCulture);
            //    DateTime currentdate = sgen.server_datetime_dt_local(userCode);
            //    DateTime currentdate1 = currentdate.AddMinutes(10);

            //    if (from < currentdate1 && from > currentdate)
            //    {
            //        todolist task = new todolist();
            //        task.title = Convert.ToString(dt3.Rows[i]["meeting_type"]);
            //        task.message = (dt3.Rows[i]["conf_roomname"]).ToString() + "\r\n" + dt3.Rows[i]["startdatetime"].ToString() + "\r\n" + dt3.Rows[i]["enddatetime"].ToString();
            //        task.priority = "warning";
            //        todo_list.Add(task);
            //    }
            //}
            #endregion

            string json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(todo_list);
            if (!string.IsNullOrEmpty(callback))
            {
                json = string.Format("{0}({1});", callback, json);
            }
            context.Response.ContentType = "text/json";
            context.Response.Write(json);
        }
        catch (Exception ex)
        {
            sgenFun sgen= new sgenFun(MyGuid); ;
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