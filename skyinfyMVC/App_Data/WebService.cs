using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Xml;

/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService
{
    //string userCode;
    //sgenFun sgen = new sgenFun();

    public WebService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }



    //[WebMethod]
    //public void InsertUserInput()
    //{
 
    //}




    //[WebMethod]
    //public string HelloWorld()
    //{
    //    return "Hello World";
    //}



    //[WebMethod]
    //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]//Specify return format.
    //public string Get_event_data()
    //{
    //    userCode = sgen.getUserCode();
    //    DataTable dt = new DataTable();
    //    dt = sgen.getdata(userCode, "select Notification_Id,Notification_Title,Not_Activity_Date_From,Not_Activity_Date_To,Notification_Description,Notification_Ent_by from Notifications where Not_Activity_Date_From is not null and Not_Activity_Date_To is not null");


    //    List<monthly> eventslist = new List<monthly>();
    //    for (int i = 0; i < dt.Rows.Count; i++)
    //    {
    //        monthly evnt = new monthly();
    //        evnt.id = Convert.ToInt32(dt.Rows[i]["Notification_Id"]);
    //        evnt.name = Convert.ToString(dt.Rows[i]["Notification_Title"]);
    //        evnt.startdate = Convert.ToDateTime(dt.Rows[i]["Not_Activity_Date_From"]).ToString("yyyy-MM-dd");
    //        evnt.starttime = Convert.ToDateTime(dt.Rows[i]["Not_Activity_Date_From"]).ToString("HH:mm");

    //        evnt.enddate = Convert.ToDateTime(dt.Rows[i]["Not_Activity_Date_To"]).ToString("yyyy-MM-dd");
    //        evnt.endtime = Convert.ToDateTime(dt.Rows[i]["Not_Activity_Date_To"]).ToString("HH:mm");

    //        evnt.color = "#666699";
    //        evnt.url = "../../../../../erp/Notification_detail.aspx?INId=" + dt.Rows[i]["Notification_Id"];
    //        eventslist.Add(evnt);
    //    }

    //    return new JavaScriptSerializer().Serialize(eventslist);

    //    //XmlDocument xdoc = new XmlDocument();
    //    //XmlNode monthly = xdoc.CreateElement("monthly");
    //    //xdoc.AppendChild(monthly);

    //    //for (int i = 0; i < dt.Rows.Count; i++)
    //    //{
    //    //    var des = xdoc.CreateElement("event");

    //    //    var id = xdoc.CreateElement("id");
    //    //    id.InnerText = Convert.ToString(dt.Rows[i]["Notification_Id"]);
    //    //    des.AppendChild(id);

    //    //    var name = xdoc.CreateElement("name");
    //    //    name.InnerText = Convert.ToString(dt.Rows[i]["Notification_Title"]);
    //    //    des.AppendChild(name);

    //    //    var startdate = xdoc.CreateElement("startdate");
    //    //    startdate.InnerText = "2017-09-7";
    //    //    des.AppendChild(startdate);

    //    //    var enddate = xdoc.CreateElement("enddate");
    //    //    enddate.InnerText = "2017-09-12";
    //    //    des.AppendChild(enddate);

    //    //    var color = xdoc.CreateElement("color");
    //    //    color.InnerText = "red";
    //    //    des.AppendChild(color);


    //    //    monthly.AppendChild(des);
    //    //}


    //    //return xdoc;
    //}



    //[WebMethod]
    //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]//Specify return format.
    //public List<todolist> Get_todolist()
    //{
    //    //string userid = Convert.ToString(Session["user_id"]);
    //    userCode = sgen.getUserCode();
    //    DataTable dt = new DataTable();
    //    dt = sgen.getdata(userCode, "select * from My_To_Do_List where todolist_Date_From is not null and todolist_Date_To is not null");

    //    List<todolist> todo_list = new List<todolist>();
    //    for (int i = 0; i < dt.Rows.Count; i++)
    //    {
    //        todolist task = new todolist();
    //        task.title = Convert.ToString(dt.Rows[i]["todolist_Title"]);
    //        task.message = (dt.Rows[i]["todolist_Description"]).ToString() + "\r\n" + dt.Rows[i]["todolist_Date_From"].ToString() + "\r\n" + dt.Rows[i]["todolist_Date_To"].ToString();
    //        todo_list.Add(task);
    //    }
    //    return todo_list;


    //}




    //[WebMethod]
    //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    //public string get_dept_in_group()
    //{
    //    userCode = sgen.getUserCode();
    //    List<combine_user_dept> grouplist = new List<combine_user_dept>();

    //    dept_in_grp deptGroup = new dept_in_grp();
    //    List<dept_in_grp> deptlist = new List<dept_in_grp>();
    //    //deptGroup.name = "hr_group";
    //    //deptGroup.name = "IT_group";
    //    //deptGroup.name = "Main_group";
    //    //deptGroup.y = 3;
    //    //deptGroup.y = 4;
    //    //deptGroup.y = 2;
    //    //deptGroup.drilldown = "drilldown1";
    //    //deptGroup.drilldown = "drilldown2";
    //    //deptGroup.drilldown = "drilldown3";
    //    //deptlist.Add(deptGroup);

    //    user_in_grp userinGroup = new user_in_grp();
    //    List<user_in_grp> userlist=new List<user_in_grp>();
    //    //userinGroup.name = "hr_group";
    //    //userinGroup.name = "hr_gIT_grouproup";
    //    //userinGroup.name = "Main_group";
    //    //userinGroup.id = "drilldown1";
    //    //userinGroup.id = "drilldown2";
    //    //userinGroup.id = "drilldown3";
    //    Dictionary< string, int> dict=new Dictionary<string,int>();
    //    //dict.Add("Finance",2);
    //    //dict.Add("HR", 5);
    //    //dict.Add("IT", 7);
    //    //userinGroup.data=dict;
    //    //userlist.Add(userinGroup);

    //    combine_user_dept combine_user_deptList = new combine_user_dept();
    //    combine_user_deptList.data1 = deptlist;
    //    combine_user_deptList.data2 = userlist;

    //    return new JavaScriptSerializer().Serialize(combine_user_deptList);


    //    //DataTable dt = new DataTable();
    //    //dt = sgen.getdata(userCode, "Select group_id,group_name, count(*) as dep_count from (Select distinct gd.group_id, group_name,department from USERGROUP g, user_details d,GROUP_DETAILS gd where g.user_id= d.user_id and gd.group_id = g.group_id ) a group by group_id,group_name");

    //    //for (int i = 0; i < dt.Rows.Count; i++)
    //    //{
    //    //    dept_in_grp deptGroup = new dept_in_grp();
    //    //    deptGroup.name = Convert.ToString(dt.Rows[i]["Group_Name"]);
    //    //    deptGroup.y = Convert.ToInt32(dt.Rows[i]["dep_count"]);   
    //    //}

    //    //DataTable dt1=new DataTable();
    //    // dt1 = sgen.getdata(userCode, "Select group_id, group_name, department, count(*) from(Select gd.group_id, group_name,department from USERGROUP g, user_details d,GROUP_DETAILS gd where g.user_id= d.user_id and gd.group_id = g.group_id) a group by group_id, group_name, departmenT");
    //    // for (int j = 0; j < dt1.Rows.Count; j++)
    //    // {
    //    //     user_in_grp userinGroup = new user_in_grp();
    //    //     userinGroup.name = Convert.ToString(dt1.Rows[j]["Group_Name"]);
    //    //     userinGroup.id = Convert.ToString(dt1.Rows[j]["Group_Name"]);
    //    //     string key=(dt1.Rows[j]["Department"]).ToString();
    //    //     int value= Convert.ToInt32(dt1.Rows[j]["usercount"]);
    //    //     userinGroup.data = (key,value);  
    //    // }

    //    //     return new JavaScriptSerializer().Serialize(grouplist);
    //}

    //SELECT USERGROUP.USERGROUP_ID, USERGROUP.USER_ID,USERGROUP.GROUP_ID,GROUP_DETAILS.GROUP_NAME FROM USERGROUP JOIN GROUP_DETAILS ON USERGROUP.GROUP_ID=GROUP_DETAILS.GROUP_ID

    //[WebMethod]
    //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    //public string get_all_groupuser()
    //{
    //    userCode = sgen.getUserCode();
    //    DataTable dt = new DataTable();
    //    dt = sgen.getdata(userCode, "select * from usergroup");

    //    List<user_in_group> usergrouplist = new List<user_in_group>();
    //    for (int i = 0; i < dt.Rows.Count; i++)
    //    {
    //        user_in_group usergroups = new user_in_group();
    //        usergroups.groupId = Convert.ToInt32(dt.Rows[i]["Group_Id"]);
    //        usergroups.userId = Convert.ToString(dt.Rows[i]["User_Id"]);
    //        usergrouplist.Add(usergroups);
    //    }
    //    return new JavaScriptSerializer().Serialize(usergrouplist);
    //}

}
