using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for properties_info
/// </summary>
public class properties_info
{
    //public string File_name { get; set; }
    //public string Cou_title { get; set; }
    //public string User_id { get; set; }
    //public string Mod_name { get; set; }
    //public string Cat_name { get; set; }
    //public string Cou_descp { get; set; }
    public string Groupid { get; set; }
    public string Groupname { get; set; }
    public string Descp { get; set; }



}

public class monthly
{
    public int id { get; set; }
    public string name { get; set; }
    public string startdate { get; set; }
    public string enddate { get; set; }
    public string starttime { get; set; }
    public string endtime { get; set; }
    public string color { get; set; }
    public string url { get; set; }
}


public class Reminder
{
    public string title { get; set; }
    public string datefrom { get; set; }
    public string dateto { get; set; }
    public string description { get; set; }
}


public class todolist
{
    public string title { get; set; }
    //public string datefrom { get; set; }
    //public string dateto { get; set; }
    public string message { get; set; }
    public string priority { get; set; }
}

public class dept_in_grp
{
    public string name { get; set; }
    public int y { get; set; }
    public string drilldown { get; set; }
}

public class user_in_grp
{
    public string name { get; set; }
    public string id { get; set; }
    public System.Collections.ArrayList data { get; set; }
}

public class yhuj
{
    public string[] Data { get; set; }
}


public class combine_user_dept
{
    public List<dept_in_grp> data1 { get; set; }
    public List<user_in_grp> data2 { get; set; }
}

public class questions_ans
{
    public int Quesid { get; set; }
    public string Question { get; set; }
    public string Options { get; set; }
    public string Option_filename { get; set; }
    public string Option_filepath { get; set; }
    public int correct_option { get; set; }
    public int quesno { get; set; }
    public int ansno { get; set; }

}


public class getQuestionPaper
{
    public int ques_no { get; set; }
    public string ques_paper_id { get; set; }
    public string ques_paper_name { get; set; }
    public string question_id { get; set; }
    public string question_name { get; set; }
    public string ans1 { get; set; }
    public string ansId1 { get; set; }
    public string ans2 { get; set; }
    public string ansId2 { get; set; }
    public string ans3 { get; set; }
    public string ansId3 { get; set; }
    public string ans4 { get; set; }
    public string ansId4 { get; set; }
    public string filename1 { get; set; }
    public string filename2 { get; set; }
    public string filename3 { get; set; }
    public string filename4 { get; set; }
    public string correctAnsId { get; set; }
    public string correctTextans { get; set; }
    public string correctansExplntn { get; set; }
    public string correctImageans { get; set; }
    public string wrngAnsId { get; set; }
    public int ctId { get; set; }
}


public class know_rep_detail
{
    public int kr_id { get; set; }
    public string country_name { get; set; }
    public string state_name { get; set; }
    public string city { get; set; }
    public string act_name { get; set; }
    public string rule_name { get; set; }
    public string l_area_name { get; set; }
    public string sl_area_name { get; set; }
    public string section { get; set; }
    public string rule_no { get; set; }
    public string form_no { get; set; }
    public string af_name { get; set; }
    public string due_on { get; set; }
    public string freq_name { get; set; }
    public string decription { get; set; }
    public string sta_auth_name { get; set; }
    public string ctype_name { get; set; }
    public string cost_of_non_compliance { get; set; }
    public string cost_text { get; set; }
    public string crit_name { get; set; }
    public string prior_name { get; set; }

}



public class conferenceroom
{
    public string Conf_roomid { get; set; }
    public string Conf_roomname { get; set; }
    public string Conf_roomtype { get; set; }
    public string Seating_capacity { get; set; }
    public string Projector { get; set; }
    public string Whitebord { get; set; }
    public string Laptops { get; set; }
    public string Wifi { get; set; }
    public string Video_conf { get; set; }
    public string Roomimg_name { get; set; }
    public string Roomimg_path { get; set; }
    public string Entby { get; set; }
    public string Entdt { get; set; }
    public string Editby { get; set; }
    public string Editdt { get; set; }
    public string vch_num { get; set; }
    public string Vc_type { get; set; }
    public string client_id { get; set; }
    public string client_unit_id { get; set; }
    public string vch_date { get; set; }
    public string type { get; set; }
}



public class paperresult
{
    public int Upr_id { get; set; }
    public string Company_name { get; set; }
    public string Unit_name { get; set; }
    public string User_id { get; set; }
    public string Ques_paperid { get; set; }
    public string Ques_papername { get; set; }
    public int qptype { get; set; }
    public int Total_ques { get; set; }
    public int Total_correct { get; set; }
    public int Total_incorrect { get; set; }
    public int Total_answered { get; set; }
    public int Total_unanswered { get; set; }
    public string Submit_date { get; set; }
    public string Status { get; set; }
    public string Ques_mandatory { get; set; }
}

public class mastertodolist
{
    public string todolist_id { get; set; }
    public string todolist_title { get; set; }
}




public class UserVicePaperResult
{
    public string userid { get; set; }
    public int ques_paper_id { get; set; }
    public string ques_paper_name { get; set; }
    public string total_ques { get; set; }
    public string total_correct { get; set; }
    public string total_incorrect { get; set; }
    public string total_answered { get; set; }
    public string total_unanswered { get; set; }
    public string submit_date { get; set; }
    public string status { get; set; }
}


public class QuespaperReport
{
    public string quesName { get; set; }
    public int total_correct_user { get; set; }
    public int total_incorrect_user { get; set; }
}



public class GroupUserInQuespaper
{
    public string userid { get; set; }
    public int ques_paper_id { get; set; }
    public string ques_paper_name { get; set; }
    public string total_ques { get; set; }
    public string total_correct { get; set; }
    public string total_incorrect { get; set; }
    public string total_answered { get; set; }
    public string total_unanswered { get; set; }
    public string submit_date { get; set; }
    public string status { get; set; }
    public int completedusercount { get; set; }
    public int notcompletedusercount { get; set; }
}


public class trnassignedsearch
{
    public int Usercourse_id { get; set; }
    public string User_id { get; set; }
    public string Mod_name { get; set; }
    public string Cat_name { get; set; }
    public string Cou_title { get; set; }
    public string Ref_code { get; set; }
    public string Dwn_permission { get; set; }
    public string Start_date { get; set; }
    public string End_date { get; set; }
    public string Usercourse_status { get; set; }
    public string Uc_ent_by { get; set; }
    public string Uc_ent_date { get; set; }
    public string Group_name { get; set; }
    public int group_id { get; set; }
    public string Usercoursegroup_status { get; set; }
}

public class quespaperasslist
{
    public int User_quespaperid { get; set; }
    public string User_id { get; set; }
    public string Mod_name { get; set; }
    public string Cat_name { get; set; }
    public string Cou_title { get; set; }
    public string Ref_code { get; set; }
    public string Quespaper_startdt { get; set; }
    public string Quespaper_enddt { get; set; }
    public int Ques_paper_id { get; set; }
    public int Group_id { get; set; }
    public string Group_name { get; set; }
    public string Status { get; set; }
    public string Userquespaper_ent_by { get; set; }
    public string Userquespaper_ent_dt { get; set; }
    public string Quespaper_status { get; set; }
    public string Quespaper_name { get; set; }
    public string Ent_by { get; set; }
    public string Ent_dt { get; set; }
    public int Group_quespaperid { get; set; }

}

public class assignpeopletogroup
{
    public string Userid { get; set; }
    public string Role { get; set; }
    public string Department { get; set; }
    public string First_name { get; set; }
    public string Last_name { get; set; }
    public string Email { get; set; }
    public string Group_id { get; set; }
    public string Group_name { get; set; }
    public string Status { get; set; }
    public string Rec_id { get; set; }
    public string Designation { get; set; }
    public Boolean chkRows { get; set; }

}

public class invite_meeting_status
{
    public string Usersid { get; set; }
    public string Useremail { get; set; }
    public string USsersname { get; set; }
    public string bookingsdt { get; set; }
    public string bookingedt { get; set; }
    public string Rec_id { get; set; }
    public string groupid { get; set; }
    public string group { get; set; }
    public string user_type { get; set; }
}


public class inviteerepeatrer
{
    public string User_id { get; set; }
}


public class menulist
{
    public string m_id { get; set; }
    public string m_name { get; set; }
    public string m_link { get; set; }
    public string m_level { get; set; }
    public string m_icon { get; set; }
    public string m_css { get; set; }
    public bool m_submenu { get; set; }
    public string m_module1 { get; set; }
    public string m_module2 { get; set; }
    public string m_module3 { get; set; }
    public string u_id { get; set; }
    public string m_order { get; set; }
    public bool m_default { get; set; }


}

public class submenulist
{
    public string sm_id { get; set; }
    public string sm_name { get; set; }
    public string sm_link { get; set; }
    public string sm_level { get; set; }
    public string sm_icon { get; set; }
    public string sm_css { get; set; }
    public bool sm_submenu { get; set; }
    public string sm_module1 { get; set; }
    public string sm_module2 { get; set; }
    public string sm_module3 { get; set; }
    public string su_id { get; set; }
    public string sm_order { get; set; }
    public bool sm_default { get; set; }
}
