using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


/// <summary>
/// Summary description for Icon_class
/// </summary>
/// 

public class Icon_class
{
    sgenFun sgen;
    string userCode = "";
    public static string MyGuid = "";

    public Icon_class(string Myguid)
    {
        MyGuid = Myguid;
        sgen = new sgenFun(MyGuid);
    }
    public void insert_icons()
    {
        userCode = sgen.getUserCode();
        string version = "001.001.001";
        if (version.Equals("001.001.001"))
        { }
        if (version.Equals("001.001.002"))
        { }
        if (version.Equals("001.001.003"))
        { }
        //main module------------------------------------
        Create_Icon("1000", "Training", "../../../../erp/dashboard", 1, "", "", true, "tr", "tmain", "tmain", 5, 1, true, "Enhancing human capabilities.", "Enhancing human capabilities through automate solutions.");
        #region
        //child menu-------------------------------------
        Create_Icon("1001", "Dashboard", "../../../../erp/dashboard", 2, "fa fa-home", "", false, "", "com", "common", 5, 1, true);
        //child menu-------------------------------------
        Create_Icon("1002", "Training Material", "", 2, "fa fa-graduation-cap", "nav child_menu", true, "tra", "tr", "tmain", 5, 11, true);
        //subchild menu-------------------------------------
        #region training

        Create_Icon("1002.1", "Training Upload", "/home/upload_training", 3, "", "", false, "", "tra", "tmain", 5, Convert.ToDecimal(11.01), true);
        //Create_Icon("1002.2", "Assign Individual", "../../../../../erp/corp_admin/corp_adm_assign_trn_to_people", 3, "", "", false, "", "tra", "tmain", 5, Convert.ToDecimal(11.02), true);
        Create_Icon("1002.3", "Assign Training", "../../../../../erp/corp_admin/corp_adm_ass_trn_to_groups", 3, "", "", false, "", "tra", "tmain", 5, Convert.ToDecimal(11.03), true);
        Create_Icon("1002.4", "Unassign Training", "../../../../../../erp/corp_admin/list_of_training_assigned_to_users", 3, "", "", false, "", "tra", "tmain", 5, Convert.ToDecimal(11.04), true);
        Create_Icon("1002.5", "Make Series", "../../../../../../erp/corp_admin/make_series", 3, "", "", false, "", "tra", "tmain", 5, Convert.ToDecimal(11.05), true);
        Create_Icon("1002.6", "Physical Attendance", "../../../../../../erp/corp_admin/physical_attendance", 3, "", "", false, "", "tra", "tmain", 5, Convert.ToDecimal(11.05), true);
        Create_Icon("1002.7", "Assign Training New", "../hall/asn_trng", 3, "", "", false, "", "tra", "tmain", 5, Convert.ToDecimal(11.06), true);

        #endregion
        //child menu-------------------------------------
        Create_Icon("1003", "Quiz Material", "", 2, "fa fa-user", "nav child_menu", true, "qpa", "tr", "tmain", 5, 12, true);
        //subchild menu-------------------------------------
        #region quiz material

        Create_Icon("1003.1", "Add Quiz", "../../../../erp/insert_obj_ques_answers", 3, "", "", false, "", "qpa", "tmain", 5, Convert.ToDecimal(12.01), true);
        Create_Icon("1003.9", "Add Quiz New", "../Training/ins_quiz", 3, "", "", false, "", "qpa", "tmain", 5, Convert.ToDecimal(12.01), true);
        Create_Icon("1003.2", "Make Quiz Praxis", "../../../../../erp/make_ques_paper", 3, "", "", false, "", "qpa", "tmain", 5, Convert.ToDecimal(12.02), true);
        Create_Icon("1003.10", "Make Quiz Praxis", "../Training/make_quiz", 3, "", "", false, "", "qpa", "tmain", 5, Convert.ToDecimal(12.02), true);
        Create_Icon("1003.11", "Assign Quiz Praxis New", "../Training/asn_quiz", 3, "", "", false, "", "qpa", "tmain", 5, Convert.ToDecimal(12.04), true);
        //Create_Icon("1003.3", "Assign Individual", "../../../../../../erp/corp_admin/corp_adm_ass_quespaper_to_people", 3, "", "", false, "", "qpa", "tmain", 5, Convert.ToDecimal(12.03), true);
        Create_Icon("1003.4", "Assign Quiz Praxis", "../../../../../../erp/corp_admin/corp_adm_ass_quespaper_to_groups", 3, "", "", false, "", "qpa", "tmain", 5, Convert.ToDecimal(12.04), true);
        Create_Icon("1003.5", "Unassign Quiz Praxis", "../../../../../../erp/corp_admin/list_of_quespaper_assigned", 3, "", "", false, "", "qpa", "tmain", 5, Convert.ToDecimal(12.05), true);
        //Create_Icon("1003.6", "My Quiz Paper", "../../../../../../erp/my_quespaper", 3, "fa fa-user", "", false, "-", "qpa", "tmain", 5, Convert.ToDecimal(12.06), true);
        Create_Icon("1003.7", "Quizz List", "../../../../../../erp/list_of_questions", 3, "", "", false, "-", "qpa", "tmain", 5, Convert.ToDecimal(12.07), true);
        Create_Icon("1003.8", "Quizz Praxis List", "../../../../../../erp/list_of_question_paper", 3, "", "", false, "-", "qpa", "tmain", 5, Convert.ToDecimal(12.08), true);

        #endregion

        #region common

        //child menu-------------------------------------
        Create_Icon("1006", "Common", "", 2, "fa fa-user", "nav child_menu", true, "com", "common", "common", 5, 3, true);
        //common menu-------------------------------------
        Create_Icon("1004", "My Task", "", 3, "", "nav child_menu", true, "cal", "com", "common", 5, 3, true);
        //common submenu-------------------------------
        #region subchild common task

        // Create_Icon("1004.1", "Calender", "../../../../../erp/Event_Calender", 4, "", "", false, "", "cal", "common", 5, Convert.ToDecimal(13.01), true);
        Create_Icon("1004.1", "Calender", "../Home/event_cal", 4, "", "", false, "", "cal", "common", 5, Convert.ToDecimal(13.01), true);
        Create_Icon("1004.3", "Calender Day wise", "../Home/event_cal_day", 4, "", "", false, "", "cal", "common", 5, Convert.ToDecimal(13.01), true);
        //Create_Icon("1004.2", "To Do List", "../../../../../erp/list_of_todotask", 4, "", "", false, "", "cal", "common", 5, Convert.ToDecimal(13.02), true);
        Create_Icon("1004.2", "To Do List", "../Home/todolist", 4, "", "", false, "", "cal", "common", 5, Convert.ToDecimal(13.02), true);

        #endregion
        //common menu-------------------------------------
        Create_Icon("1005", "Notification", "../../../../../erp/list_of_notification", 3, "", "", false, "", "com", "common", 5, 4, true);
        //child menu-------------------------------------
        Create_Icon("4008", "Refresh View & Index", "", 3, "", "", false, "", "com", "common", 5, 48, true, "onclick=$menuclick(this);$");
        //Create_Icon("1006", "Course", "", 2, "fa fa-user", "nav child_menu", true, "crtcourse", "tr", "tmain", 5, 12, true);
        ////common subchild menu-------------------------------------
        #region Course

        //Create_Icon("1006.1", "Add Course", "../../../../erp/corp_admin/corp_adm_entercoursedetails", 3, "", "", false, "", "crtcourse", "tmain", 5, Convert.ToDecimal(12.01), true);
        //Create_Icon("1006.2", "View Course", "../../../../../erp/corp_admin/corp_adm_view_course", 3, "", "", false, "", "crtcourse", "tmain", 5, Convert.ToDecimal(12.02), true);

        #endregion
        //common child menu-------------------------------------
        Create_Icon("1007", "Mail", "-", 3, "", "nav child_menu", true, "mail", "com", "common", 5, 5, true);
        //common subchild menu-------------------------- -----------
        #region submenu common mail

        Create_Icon("1007.1", "Compose", "../../../../../erp/corp_admin/corp_adm_compose", 4, "", "", false, "", "mail", "common", 5, Convert.ToDecimal(16.01), true);
        Create_Icon("1007.2", "Inbox", "../../../../../erp/corp_admin/corp_adm_message", 4, "", "", false, "", "mail", "common", 5, Convert.ToDecimal(16.02), true);
        Create_Icon("1007.3", "Sent", "../../../../../erp/corp_admin/corp_adm_message", 4, "", "", false, "", "mail", "common", 5, Convert.ToDecimal(16.03), true);
        Create_Icon("1007.4", "Draft", "../../../../../erp/corp_admin/corp_adm_message", 4, "", "", false, "", "mail", "common", 5, Convert.ToDecimal(16.04), true);
        Create_Icon("1007.5", "Archieve", "../../../../../erp/corp_admin/corp_adm_message", 4, "", "", false, "", "mail", "common", 5, Convert.ToDecimal(16.05), true);
        Create_Icon("1007.6", "Trash", "../../../../../erp/corp_admin/corp_adm_message", 4, "", "", false, "", "mail", "common", 5, Convert.ToDecimal(16.06), true);

        #endregion
        //common menu-------------------------------------
        Create_Icon("1008", "Admin Setting", "", 3, "", "nav child_menu", true, "usersettng", "com", "common", 5, 7, true);
        //common subchild menu-------------------------------------
        #region submenu common admin setting

        Create_Icon("1008.1", "Create Subuser", "../Home/create_user", 4, "", "", false, "", "usersettng", "common", 5, Convert.ToDecimal(17.01), true);
        Create_Icon("1008.2", "Create Company ", "../Home/client_profile", 4, "", "", false, "", "usersettng", "common", 5, Convert.ToDecimal(17.02), true);
        //Create_Icon("1008.3", "View Company", "../../../../../erp/company/list_of_client", 3, "", "", false, "", "usersettng", "common", 5, Convert.ToDecimal(17.03), true);
        Create_Icon("1008.3", "Change Menu Order", "../home/menueditor", 4, "", "", false, "", "usersettng", "common", 5, Convert.ToDecimal(17.03), true);
        Create_Icon("1008.4", "Create Unit", "../Home/unit_profile", 4, "", "", false, "", "usersettng", "common", 5, Convert.ToDecimal(17.04), true);
        //Create_Icon("1008.5", "View Unit", "../../../../../erp/company/list_of_client_unit", 3, "", "", false, "", "usersettng", "common", 5, Convert.ToDecimal(17.05), true);
        //Create_Icon("1008.7", "Config", "../../../../../erp/corp_admin/dept_designation", 3, "", "", false, "", "usersettng", "common", 5, Convert.ToDecimal(17.07), true);
        //Create_Icon("1008.8", "View User", "../../../../../erp/corp_admin/list_of_trainer_trainee", 3, "", "", false, "", "usersettng", "common", 5, Convert.ToDecimal(17.08), true);
        Create_Icon("1008.9", "Department", "../Mst/master_ctrl", 4, "", "", false, "", "usersettng", "common", 5, Convert.ToDecimal(17.09), true);
        Create_Icon("1008.10", "Designation", "../Mst/master_ctrl", 4, "", "", false, "", "usersettng", "common", 5, Convert.ToDecimal(17.10), true);
        Create_Icon("1008.11", "Company Logo", "../../../../../erp/corp_admin/admin_master", 4, "", "", false, "", "usersettng", "common", 5, Convert.ToDecimal(17.10), true);
        //Create_Icon("1008.12", "Bank Master", "../../../../../erp/corp_admin/bank", 4, "", "", false, "", "usersettng", "common", 5, Convert.ToDecimal(17.10), true);
        Create_Icon("1008.13", "Good Wishes", "../../../../../erp/corp_admin/good_wishes", 4, "", "", false, "", "usersettng", "common", 5, Convert.ToDecimal(17.10), true);
        Create_Icon("1008.14", "Backup Database", "", 4, "", "", false, "", "usersettng", "common", 5, Convert.ToDecimal(17.10), true);
        Create_Icon("1008.15", "Annual Income Master", "../Mst/master_ctrl", 4, "-", "-", false, "-", "usersettng", "common", 5, Convert.ToDecimal(17.10), true);
        Create_Icon("1008.16", "API Integration", "../Mst/tallyapi", 4, "-", "", false, "-", "usersettng", "common", 5, Convert.ToDecimal(17.10), true);
        Create_Icon("1008.17", "System Update", "../erp/sys_update", 4, "-", "", false, "-", "usersettng", "common", 5, Convert.ToDecimal(17.10), true);
        Create_Icon("1008.18", "Cpanel", "../Home/cpanal", 4, "-", "", false, "-", "usersettng", "common", 5, Convert.ToDecimal(17.10), true);
        Create_Icon("1008.19", "User Role Rights", "../Home/urights", 4, "-", "", false, "-", "usersettng", "common", 5, Convert.ToDecimal(17.10), true);
        Create_Icon("1008.20", "User Rights ", "../Home/urights", 4, "-", "", false, "-", "usersettng", "common", 5, Convert.ToDecimal(17.10), true);
        Create_Icon("1008.21", "Sub Department", "../Mst/cascade_ddl", 4, "-", "", false, "-", "usersettng", "common", 5, Convert.ToDecimal(17.10), true);
        Create_Icon("1008.22", "Add Year", "../Education/ac_yr", 4, "", "", false, "-", "usersettng", "common", 5, Convert.ToDecimal(73.11), true);
        Create_Icon("1008.23", "Activations", "../Home/Activations", 4, "", "", false, "-", "usersettng", "common", 5, Convert.ToDecimal(73.11), true);

        #endregion
        //common child menu-------------------------------------
        Create_Icon("10010", "Utility", "", 3, "", "nav child_menu", true, "utility", "com", "common", 5, 6, true);
        //common subchild menu-------------------------------------
        #region submenu common admin setting

        Create_Icon("10010.1", "Scientific Calculator", "../../../../../erp/corp_admin/corp_adm_creationofuserstep1", 4, "", "", false, "", "utility", "common", 5, Convert.ToDecimal(17.01), true);
        Create_Icon("10010.2", "Cheque Printing", "../../../../../erp/company/client_profile", 4, "", "", false, "", "utility", "common", 5, Convert.ToDecimal(17.02), true);


        #endregion

        //common child menu---------------------------------------------
        Create_Icon("1011", "User Setting", "", 3, "", "nav child_menu", true, "settng", "com", "common", 5, 20, true);
        //common subchild menu-------------------------------------
        #region subchild common setting

        Create_Icon("1011.1", "Mail Setup", "../../../../../erp/mail_sent_from_emailid", 4, "", "", false, "", "settng", "common", 5, Convert.ToDecimal(20.01), true);
        // Create_Icon("1011.2", "Dashboard Setting", "../../../../../erp/common/dashboard_setting", 4, "", "", false, "", "settng", "common", 5, Convert.ToDecimal(20.02), true);
        Create_Icon("1011.2", "Dashboard Setting", "../Home/db_set", 4, "", "", false, "", "settng", "common", 5, Convert.ToDecimal(20.02), true);
        Create_Icon("1011.3", "Admin Setting", "../../../../../erp/corp_admin/corp_adm_controls_config", 4, "", "", false, "", "settng", "common", 5, Convert.ToDecimal(20.03), true);
        Create_Icon("1011.4", "Mail Config", "../Home/mailConfig", 4, "", "", false, "", "settng", "common", 5, Convert.ToDecimal(20.03), true);
        Create_Icon("1011.5", "Manual Mail Config", "../Home/manualMail", 4, "", "", false, "", "settng", "common", 5, Convert.ToDecimal(20.03), true);
        #endregion

        #endregion

        #region subchild Video Conferencing
        //common child menu---------------------------------------------
        Create_Icon("1014", "Video Conferenceing", "", 3, "", "nav child_menu", true, "vdcon", "com", "common", 5, 20, true);
        //common subchild menu-------------------------------------
        #region subchild common setting

        Create_Icon("1014.1", "Video Conference", "/home/v_conference", 4, "", "", false, "", "vdcon", "common", 5, Convert.ToDecimal(20.01), true);
        Create_Icon("1014.2", "Video BroadCast", "/home/v_classes", 4, "", "", false, "", "vdcon", "common", 5, Convert.ToDecimal(20.02), true);
        Create_Icon("1014.3", "Schedule Meeting", "/home/v_schedule", 4, "", "", false, "", "vdcon", "common", 5, Convert.ToDecimal(20.02), true);
        //Create_Icon("1014.2", "Video BroadCast", "/home/v_broadcast", 4, "", "", false, "", "vdcon", "common", 5, Convert.ToDecimal(20.02), true);
        #endregion

        #endregion

        //child menu-------------------------------------
        Create_Icon("1009", "Group", "", 2, "fa fa-user", "nav child_menu", true, "crtgrp", "tr", "tmain", 5, 12, true);
        //subchild menu-------------------------------------
        #region Group 

        //Create_Icon("1009.1", "Add Group", "../../../../erp/corp_admin/corp_adm_creation_of_group", 3, "", "", false, "", "crtgrp", "tmain", 5, Convert.ToDecimal(12.01), true);
        //Create_Icon("1009.1", "Add Group", "../Training/add_grp", 3, "", "", false, "", "crtgrp", "tmain", 5, Convert.ToDecimal(12.01), true);
        //Create_Icon("1009.2", "View Group", "../../../../../erp/corp_admin/corp_adm_view_group", 3, "", "", false, "", "crtgrp", "tmain", 5, Convert.ToDecimal(12.02), true);
        Create_Icon("1009.1", "Add Group", "../Training/add_grp", 3, "", "", false, "", "crtgrp", "tmain", 5, Convert.ToDecimal(12.01), true);
        Create_Icon("1009.2", "View Group", "../Training/vw_grp", 3, "", "", false, "", "crtgrp", "tmain", 5, Convert.ToDecimal(12.02), true);
        Create_Icon("1009.3", "Assign People", "../../../../../erp/corp_admin/corp_adm_assign_people_to_group", 3, "", "", false, "", "crtgrp", "tmain", 5, Convert.ToDecimal(12.03), true);
        #endregion
        //child menu-------------------------------------
        Create_Icon("1010", "Report", "../../../../../erp/employr/user_report", 2, "fa fa-user", "", false, "report", "tr", "tmain", 5, 8, true);

        Create_Icon("999999", "Logout", "", 2, "fa fa-power-off", "", false, "settng", "com", "common", 5, Convert.ToDecimal(99999.00), true, "onclick=$menuclick(this);$");
        //child menu---------------------------------------------
        Create_Icon("1012", "My Assignment", "", 2, "fa fa-user", "nav child_menu", true, "myass", "tr", "tmain", 5, 21, true);
        //common subchild menu-------------------------------------
        #region myy assignment

        Create_Icon("1012.1", "Training Assignment", "../../../../../../erp/corp_admin/corp_adm_my_training", 3, "", "", false, "", "myass", "tmain", 5, Convert.ToDecimal(21.01), true);
        Create_Icon("1012.2", "Quiz Praxis Assignment", "../../../../../../erp/my_quespaper", 3, "", "", false, "", "myass", "tmain", 5, Convert.ToDecimal(21.02), true);
        Create_Icon("1012.3", "My Result", "../../../../../../erp/List_of_paperAndresult_byuser", 3, "", "", false, "", "myass", "tmain", 5, Convert.ToDecimal(21.03), true);
        Create_Icon("1012.4", "My Training New", "../Training/my_trn", 3, "", "", false, "", "myass", "tmain", 5, Convert.ToDecimal(22.01), true);

        #endregion
        //child menu---------------------------------------------
        Create_Icon("1013", "Training Master", "", 2, "fa fa-user", "nav child_menu", true, "tmod", "tr", "tmain", 5, 22, true);
        //common subchild menu-------------------------------------
        #region trining module

        Create_Icon("1013.1", "Module", "../Training/Module1", 3, "", "", false, "", "tmod", "tmain", 5, Convert.ToDecimal(22.01), true);
        Create_Icon("1013.2", "Category", "../Training/Module1", 3, "", "", false, "", "tmod", "tmain", 5, Convert.ToDecimal(22.02), true);
        Create_Icon("1013.3", "Title", "../Training/Module1", 3, "", "", false, "", "tmod", "tmain", 5, Convert.ToDecimal(22.03), true);
        Create_Icon("1013.4", "Quiz Praxis Format", "../../../../../../erp/corp_admin/training_master", 3, "", "", false, "", "tmod", "tmain", 5, Convert.ToDecimal(22.04), true);
        Create_Icon("1013.5", "Training level", "../Mst/master_ctrl", 3, "", "", false, "", "tmod", "tmain", 5, Convert.ToDecimal(22.03), true);


        #endregion
        #endregion

        //main module-------------------------------------------
        Create_Icon("2000", "Knowledge Repository", "../../../../erp/dashboard", 1, "fa fa-user fa-5x", "btn btn-sq-lg btn-primary", true, "kr", "krmain", "krmain", 5, 2, true, "Enhancing human capabilities through automate solutions.", "Retaining and sharing corporate knowledge");
        //child menu-------------------------------------------
        #region knowledge repository

        //child menu-------------------------------------
        Create_Icon("2001", "Add Repository", "../../../../../erp/MasterAdmin/list_of_knowledgeRepository", 2, "fa fa-user", "", false, "", "kr", "krmain", 5, 20, true);
        Create_Icon("2002", "Category", "../../../../../erp/category_for_know_repository", 2, "fa fa-user", "", false, "", "kr", "krmain", 5, 21, true);
        Create_Icon("2003", "Knowledge Config", "../../../../../erp/MasterAdmin/create_role", 2, "fa fa-user", "", false, "krc", "krcon", "krmain", 5, 22, true);
        Create_Icon("2003.1", "Legislation Area", "../Mst/master_ctrl", 3, "", "", false, "", "krc", "krmain", 5, Convert.ToDecimal(47.01), true);
        Create_Icon("2003.2", "Compliance Type", "../Mst/master_ctrl", 3, "", "", false, "", "krc", "krmain", 5, Convert.ToDecimal(47.01), true);
        Create_Icon("2003.3", "Statutory Authority Name", "../Mst/master_ctrl", 3, "", "", false, "", "krc", "krmain", 5, Convert.ToDecimal(47.01), true);
        Create_Icon("2003.4", "Compliance Frequency", "../Mst/master_ctrl", 3, "", "", false, "", "krc", "krmain", 5, Convert.ToDecimal(47.01), true);
        Create_Icon("2003.5", "KR Config", "../../../../../erp/MasterAdmin/create_role", 3, "", "", false, "", "krc", "krmain", 5, Convert.ToDecimal(47.01), true);
        Create_Icon("2004", "Add KR", "../../../../../erp/MasterAdmin/create_knowledgeRepository", 2, "fa fa-user", "", false, "", "kr", "krmain", 5, 23, true);

        #endregion

        //main module-------------------------------------------
        Create_Icon("3000", "Meeting Organiser", "../../../../erp/dashboard", 1, "fa fa-user fa-5x", "btn btn-sq-lg btn-primary", true, "cr", "crmain", "crmain", 5, 3, true, "Conference room booking management.", " Reserve Conference rooms and invitee through scheduling system.");
        //child menu-------------------------------------------
        #region conference

        //child menu-------------------------------------
        Create_Icon("3001", "Add Conference Room", "../../../../../erp/Add_confrnceroom_details", 2, "fa fa-user", "", false, "", "cr", "crmain", 5, 30, true);
        Create_Icon("3005", "Add Conference Room New", "../hall/conf_room", 2, "fa fa-user", "", false, "", "cr", "crmain", 5, 30, true);
        Create_Icon("3002", "Book Conference Room", "../../../../../erp/book_conferenceroom", 2, "fa fa-user", "", false, "", "cr", "crmain", 5, 31, true);
        Create_Icon("3003", "Schedule Meeting", "../../../../../erp/schedule_meeting", 2, "fa fa-user", "", false, "", "cr", "crmain", 5, 32, true);
        Create_Icon("3006", "Schedule Meeting New", "../hall/schdl_meet", 2, "fa fa-user", "", false, "", "cr", "crmain", 5, 32, true);
        Create_Icon("3004", "Conference Config", "../../../../../erp/conference_config", 2, "fa fa-user", "", false, "", "cr", "crmain", 5, 33, true);
        Create_Icon("3007", "Add/Assign Type Of Service", "../hall/meet_config", 2, "fa fa-user", "", false, "", "cr", "crmain", 5, 33, true);
        Create_Icon("3008", "Add Services For Conf Room", "../hall/meet_config", 2, "fa fa-user", "", false, "", "cr", "crmain", 5, 33, true);
        #endregion

        //main module-------------------------------------------
        Create_Icon("4000", "Admin", "../../../../erp/dashboard", 1, "fa fa-user fa-5x", "btn btn-sq-lg btn-primary", true, "mst", "mstmain", "mstmain", 5, 4, true);

        // admin child menu-------------------------------------  
        #region admin

        Create_Icon("4001", "Module Authority", "../../../../../erp/company/module_authority", 2, "fa fa-user", "", false, "", "mst", "mstmain", 5, 41, true);
        Create_Icon("4002", "Admin Setting", "../../../../../erp/MasterAdmin/create_role", 2, "fa fa-gears", "", false, "", "mst", "mstmain", 5, 42, true);
        Create_Icon("4003", "Validate Module", "../../../../../erp/company/validate_module", 2, "fa fa-user", "", false, "", "mst", "mstmain", 5, 43, true);
        Create_Icon("4004", "Customer Profile", "../../../../../erp/company/cust_profile", 2, "fa fa-user", "", false, "", "mst", "mstmain", 5, 44, true);
        Create_Icon("4005", "View Customer", "../../../../../erp/company/list_of_cust", 2, "fa fa-user", "", false, "", "mst", "mstmain", 5, 45, true);


        //Create_Icon("4006", "Dashboard Setting", "../../../../../erp/common/dashboard_setting", 2, "fa fa-user", "", false, "", "mst", "mstmain", 5, 46, true);
        Create_Icon("4006", "Dashboard Setting", "../Home/db_set", 2, "fa fa-user", "", false, "", "mst", "mstmain", 5, 46, true);

        Create_Icon("4007", "Module Masters", "", 2, "fa fa-user", "nav child_menu", true, "mmas", "mst", "mstmain", 5, 47, true);
        Create_Icon("4007.1", "Fuel Master", "../Mst/master_ctrl", 3, "", "", false, "", "mmas", "mstmain", 5, Convert.ToDecimal(47.01), true);
        Create_Icon("4007.2", "Caste Category", "../Mst/master_ctrl", 3, "", "", false, "-", "mmas", "mstmain", 5, Convert.ToDecimal(47.02), true);
        Create_Icon("4007.3", "Caste Type", "../../../../erp/school_admin/School_Master", 3, "", "", false, "-", "mmas", "mstmain", 5, Convert.ToDecimal(47.03), true);
        Create_Icon("4007.4", "Payment Mode", "../Mst/master_ctrl", 3, "", "", false, "-", "mmas", "mstmain", 5, Convert.ToDecimal(47.04), true);
        Create_Icon("4007.5", "Religion Master", "../Mst/master_ctrl", 3, "", "", false, "-", "mmas", "mstmain", 5, Convert.ToDecimal(47.05), true);
        Create_Icon("4007.6", "District Master", "../mst/address_master", 3, "", "", false, "-", "mmas", "mstmain", 5, Convert.ToDecimal(47.06), true);
        Create_Icon("4007.7", "Affiliation Master", "../Mst/master_ctrl", 3, "", "", false, "-", "mmas", "mstmain", 5, Convert.ToDecimal(47.06), true);
        Create_Icon("4007.8", "Board Master", "../Mst/master_ctrlr", 3, "", "", false, "-", "mmas", "mstmain", 5, Convert.ToDecimal(47.06), true);
        Create_Icon("4007.9", "Hobby Master", "../Mst/master_ctrl", 3, "", "", false, "-", "mmas", "mstmain", 5, Convert.ToDecimal(47.09), true);
        Create_Icon("4007.10", "Account Type", "../Mst/master_ctrl", 3, "", "", false, "-", "mmas", "mstmain", 5, Convert.ToDecimal(47.10), true);
        //Create_Icon("4008", "Refresh View & Index", "", 2, "fa fa-user", "", false, "", "mst", "mstmain", 5, 48, true, "onclick=$menuclick(this);$");
        Create_Icon("4009", "Bank Details", "", 2, "fa fa-user", "nav child_menu", true, "bnkmst", "mstmain", "mstmain", 5, 49, true);
        //Create_Icon("4009.1", "Bank Name Master", "../../../../erp/corp_admin/admin_master", 3, "fa fa-user", "", false, "", "bnkmst", "mstmain", 5, 49, true);
        Create_Icon("4009.2", "Bank Cheque Format", "../../../../erp/bank/Cheque_template", 3, "fa fa-user", "", false, "", "bnkmst", "mstmain", 5, 49, true);
        //Create_Icon("4009.3", "Currency Master", "../../../../erp/corp_admin/admin_master", 3, "fa fa-user", "", false, "", "bnkmst", "mstmain", 5, 49, true);
        Create_Icon("4009.4", "Cheque Print Form", "../../../../erp/corp_admin/cheque_printing_form", 3, "fa fa-user", "", false, "", "bnkmst", "mstmain", 5, 49, true);
        Create_Icon("4009.5", "Cheque Series", "../../../../erp/bank/cheque_series", 3, "fa fa-user", "", false, "", "bnkmst", "mstmain", 5, 49, true);
        Create_Icon("4010", "Manage Version", "../../../../../erp/company/manage_version", 2, "fa fa-user", "", false, "", "mst", "mstmain", 5, 46, true);
        Create_Icon("4011", "Tree View", "../../../../../erp/company/treeViewDemo", 2, "fa fa-user", "", false, "", "mst", "mstmain", 5, 45, true);
        #endregion

        //main module------------------------------------
        Create_Icon("5000", "Work Allocation", "../../../../erp/dashboard", 1, "fa fa-user fa-5x", "btn btn-sq-lg btn-primary", true, "cu", "camain", "camain", 5, 5, true, "Create new client and assign work", "View and discussion of the client assignment work with team and client");
        // child menu-------------------------------------  
        #region clientuser


        Create_Icon("5001", "Client Master", "", 2, "fa fa-user", "nav child_menu", true, "cltmas", "cu", "camain", 5, 47, true);
        Create_Icon("5001.1", "Client Detail", "../Purchase/party", 3, "", "", false, "", "cltmas", "camain", 5, 50, true);
        Create_Icon("5001.2", "Mode Of Conveyance", "../Mst/master_ctrl", 3, "", "", false, "", "wrkmas", "camain", 5, 50, true);

        Create_Icon("5002", "Work Allocation Master", "", 2, "fa fa-user", "nav child_menu", true, "wrkmas", "camain", "camain", 5, 47, true);
        Create_Icon("5002.1", "Form Control", "../Mst/form_ctrl", 3, "", "", false, "", "wrkmas", "camain", 5, 50, true);
        Create_Icon("28005.3", "Organisation Status", "../Mst/master_ctrl", 3, "", "", false, "", "wrkmas", "camain", 5, 50, true);
        Create_Icon("40001.6", "Sales/Service Area", "../Mst/master_ctrl", 3, "", "", false, "", "wrkmas", "camain", 5, 50, true);
        Create_Icon("40001.5", "Occupation Master", "../Mst/master_ctrl", 3, "", "", false, "", "wrkmas", "camain", 5, 50, true);
        Create_Icon("40001.3", "Type Of Account", "../Mst/master_ctrl", 3, "", "", false, "", "wrkmas", "camain", 5, 50, true);
        Create_Icon("40001.8", "Type Of Address", "../Mst/master_ctrl", 3, "", "", false, "", "wrkmas", "camain", 5, 50, true);
        Create_Icon("40001.4", "Business Type", "../Mst/master_ctrl", 3, "", "", false, "", "wrkmas", "camain", 5, 50, true);

        Create_Icon("7004.1.7", "Qualification", "../Mst/master_ctrl", 3, "", "", false, "", "wrkmas", "camain", 5, 50, true);
        Create_Icon("5002.2", "Assignment Name", "../Mst/master_ctrl", 3, "", "", false, "", "wrkmas", "camain", 5, 50, true);
        Create_Icon("5002.3", "Type Master", "../Mst/master_ctrl", 3, "", "", false, "", "wrkmas", "camain", 5, 50, true);
        Create_Icon("5002.4", "Class Master", "../Mst/master_ctrl", 3, "", "", false, "", "wrkmas", "camain", 5, 50, true);





        //child menu-------------------------------------


        //child menu-------------------------------------
        Create_Icon("5009", "Allocate Assignment", "../hr/asn_to_client", 2, "fa fa-user", "", false, "", "cu", "camain", 5, 51, true);

        //child menu-------------------------------------
        Create_Icon("5003", "Allocate User Assignment", "../hall/w_alloc", 2, "fa fa-user", "", false, "", "cu", "camain", 5, 52, true);


        //child menu-------------------------------------
        Create_Icon("5004", "Chat", "../../../../../erp/chat", 2, "fa fa-user", "", false, "", "cu", "camain", 5, 53, true);

        //child menu-------------------------------------
        //  Create_Icon("5005", "Config", "../../../../../erp/corp_admin/corp_adm_controls_config", 2, "fa fa-user", "", false, "", "cu", "camain", 5, Convert.ToDecimal(53.01), true);
        Create_Icon("5006", "Generate Pattern", "../../../../../erp/client_assignment/refcodepttrn", 2, "fa fa-user", "", false, "", "cu", "camain", 5, Convert.ToDecimal(53.01), true);

        Create_Icon("5007", "Costing", "", 2, "fa fa-user", "nav child_menu", true, "wrkcst", "camain", "camain", 5, 47, true);
        Create_Icon("5007.1", "Assignment Cost", "../Hall/csheet", 3, "", "", false, "", "wrkcst", "camain", 5, 50, true);
        Create_Icon("5007.2", "Charge Rate", "../Hall/rcard", 3, "", "", false, "", "wrkcst", "camain", 5, 50, true);
        Create_Icon("5007.3", "Conveyance Rate", "../Hall/con_rate", 3, "", "", false, "", "wrkcst", "camain", 5, 50, true);
        Create_Icon("5007.4", "Invoice", "../Hall/w_inv", 3, "", "", false, "", "wrkcst", "camain", 5, 50, true);

        Create_Icon("5008", "Attendance", "", 2, "fa fa-user", "nav child_menu", true, "wrkatt", "camain", "camain", 5, 47, true);
        Create_Icon("5008.1", "Atten Entry", "../Hall/att", 3, "", "", false, "", "wrkatt", "camain", 5, 50, true);
        //Create_Icon("5008.2", "Atten Report", "../Hall/att_ret", 3, "", "", false, "", "wrkatt", "camain", 5, 50, true);
        Create_Icon("5008.2", "Atten Report", "../Hall/att_rpt", 3, "", "", false, "", "wrkatt", "camain", 5, 50, true);


        #endregion

        //main module------------------------------------
        Create_Icon("6000", "Insurance", "../../../../erp/dashboard", 1, "fa fa-user fa-5x", "btn btn-sq-lg btn-primary", true, "ins", "insmain", "insmain", 5, 6, true, "Timely management of client insurance policy.", "Managing client insurance policy and allocate to the staff Team.");
        // child menu-------------------------------------  
        #region insurance

        //child menu-------------------------------------
        Create_Icon("6001", "Add Insurance", "../../../../../erp/insurance/insurance", 2, "fa fa-user", "", false, "", "ins", "insmain", 5, 60, true);

        //child menu-------------------------------------
        Create_Icon("6002", "Assign Policy", "../../../../../erp/insurance/assign_policy", 2, "fa fa-user", "", false, "", "ins", "insmain", 5, 61, true);

        //child menu-------------------------------------
        Create_Icon("6004", "Client Master", "../../../../../erp/insurance/add_client", 2, "fa fa-user", "", false, "", "ins", "insmain", 5, 62, true);

        //child menu-------------------------------------
        Create_Icon("6003", "Insurance Config", "", 2, "fa fa-user", "nav child_menu", true, "insmst", "ins", "insmain", 5, 63, true);

        Create_Icon("6003.1", "Document Group", "../../../../../erp/hr/doc_group", 3, "", "", false, "-", "insmst", "insmain", 5, Convert.ToDecimal(63.01), true);
        Create_Icon("6003.2", "Document Type", "../../../../../erp/hr/doc_type", 3, "", "", false, "-", "insmst", "insmain", 5, Convert.ToDecimal(63.02), true);
        Create_Icon("6003.3", "Document Group Type", "../../../../../erp/hr/doc_grptype", 3, "", "", false, "-", "insmst", "insmain", 5, Convert.ToDecimal(63.03), true);
        Create_Icon("6003.4", "Insurance Config", "../../../../../erp/insurance/ins_config", 3, "", "", false, "-", "insmst", "insmain", 5, Convert.ToDecimal(63.04), true);

        //child menu-------------------------------------
        Create_Icon("6005", "Add Insurance Document", "../../../../../erp/hr/attach_empdocument", 2, "fa fa-user", "", false, "", "ins", "insmain", 5, 65, true);

        Create_Icon("6006", "Ins Company Master", "", 2, "fa fa-user", "nav child_menu", true, "inscmst", "ins", "insmain", 5, 66, true);

        Create_Icon("6006.1", "Insurance Company Detail", "../../../../../erp/insurance/add_client", 3, "", "", false, "-", "inscmst", "insmain", 5, Convert.ToDecimal(66.01), true);
        Create_Icon("6006.2", "Insurance Type Master", "../../../../../erp/insurance/ins_producttypes", 3, "", "", false, "-", "inscmst", "insmain", 5, Convert.ToDecimal(66.02), true);

        Create_Icon("6007", "Commission Master", "../../../../../erp/insurance/commission_master", 2, "fa fa-user", "", false, "", "ins", "insmain", 5, 65, true);

        Create_Icon("6008", "Agent Master", "../../../../../erp/insurance/agent_master", 2, "fa fa-user", "", false, "", "ins", "insmain", 5, 68, true);
        Create_Icon("6009", "Activate Company", "../../../../../erp/insurance/activate_comp", 2, "fa fa-user", "", false, "", "ins", "insmain", 5, 69, true);
        //Create_Icon("6006.3", "Document Group Type", "../../../../../erp/hr/doc_grptype", 3, "", "", false, "-", "insmst", "insmain", 5, Convert.ToDecimal(63.03), true);        

        #endregion

        //main module------------------------------------
        Create_Icon("7000", "Education", "../../../../erp/dashboard", 1, "fa fa-user fa-5x", "btn btn-sq-lg btn-primary", true, "main", "edumain", "edumain", 5, 7, true, "School administration management system.", "Efficiently operating school’s administrative system.");
        #region Education

        //child menu-------------------------------------
        //Create_Icon("7001", "Admission Master", "", 2, "fa fa-user", "nav child_menu", true, "edumst", "main", "edumain", 5, 70, true);
        // Create_Icon("7001", "Admission Master", "", 2, "fa fa-user", "nav child_menu", true, "edumst", "main", "edumain", 5, 70, true);

        //Create_Icon("7001.1", "Registration No.pattern/Settings", "../../../../erp/school_admin/Frm_Create_RegNo", 3, "", "", false, "-", "edumst", "edumain", 5, Convert.ToDecimal(70.01), true);
        //Create_Icon("7001.2", "Roll No.pattern/Settings", "../../../../erp/school_admin/Frm_Create_RollNo", 3, "", "", false, "-", "edumst", "edumain", 5, Convert.ToDecimal(70.02), true);

        Create_Icon("7002", "Fee Master", "", 2, "fa fa-calculator", "nav child_menu", true, "feemst", "main", "edumain", 5, 19, true);

        //Create_Icon("7002.1", "Fee Settings", "../../../../erp/school_admin/Frm_Fees_Setting", 3, "", "", false, "-", "feemst", "edumain", 5, Convert.ToDecimal(71.01), true);
        // Create_Icon("7002.2", "Create Fee Master", "../../../../erp/school_admin/admin_head_fees", 3, "", "", false, "-", "feemst", "edumain", 5, Convert.ToDecimal(71.02), true);
        Create_Icon("7002.2", "Fee Type", "../Education/feetype", 3, "", "", false, "-", "feemst", "edumain", 5, Convert.ToDecimal(71.02), true);
        //Create_Icon("7002.3", "Fee Structure", "../../../../erp/school_admin/admin_fee_structure", 3, "", "", false, "-", "feemst", "edumain", 5, Convert.ToDecimal(71.03), true);
        //Create_Icon("7002.9", "Payment Mode", "../../../../erp/school_admin/School_Master", 3, "", "", false, "-", "feemst", "edumain", 5, Convert.ToDecimal(71.09), true);
        //Create_Icon("7002.4", "Receipt No.pattern/Settings", "../../../../erp/school_admin/Frm_Create_ReceiptNo", 3, "", "", false, "-", "feemst", "edumain", 5, Convert.ToDecimal(71.04), true);
        Create_Icon("7002.5", "Fees Fine Setting & Slab", "../../../../erp/school_academic_staff/Fine_Settings_Slab", 3, "", "", false, "-", "feemst", "edumain", 5, Convert.ToDecimal(71.05), true);
        Create_Icon("7002.6", "Fees Waive Off", "../../../../erp/school_academic_staff/Fine_Settings_Slab", 3, "", "", false, "-", "feemst", "edumain", 5, Convert.ToDecimal(71.06), true);
        Create_Icon("7002.19", "Fees Waive Off New", "../Education/late_fee", 3, "", "", false, "-", "feemst", "edumain", 5, Convert.ToDecimal(71.06), true);
        Create_Icon("7002.7", "Student Fees Waive Off", "../../../../erp/school_academic_staff/Student_Waveoff", 3, "", "", false, "-", "feemst", "edumain", 5, Convert.ToDecimal(71.07), true);
        Create_Icon("7002.8", "Mis. Fee Type", "../Education/mis_feetype", 3, "", "", false, "-", "feemst", "edumain", 5, Convert.ToDecimal(71.08), true);
        Create_Icon("7002.9", "Mis. Fee Head", "../Education/mis_fee_head", 3, "", "", false, "-", "feemst", "edumain", 5, Convert.ToDecimal(71.08), true);
        Create_Icon("7002.10", "Mis. Fee Structure", "../Education/mis_fee_structure", 3, "", "", false, "-", "feemst", "edumain", 5, Convert.ToDecimal(71.08), true);
        //Create_Icon("7002.11", "Opening Bal. Transfer", "../../../../erp/school_academic_staff/Fee_Opening", 3, "", "", false, "-", "feemst", "edumain", 5, Convert.ToDecimal(71.08), true);
        Create_Icon("7002.12", "Upload Opening Bal. (EDI)", "../../../../erp/school_academic_staff/Fee_Opening_EDI", 3, "", "", false, "-", "feemst", "edumain", 5, Convert.ToDecimal(71.08), true);
        Create_Icon("7002.13", "Student Fee Master", "../Education/std_fee_structure", 3, "", "", false, "-", "feemst", "edumain", 5, Convert.ToDecimal(71.08), true);
        Create_Icon("7002.14", "Fee Structure", "../Education/fee_structure", 3, "", "", false, "-", "feemst", "edumain", 5, Convert.ToDecimal(71.08), true);
        Create_Icon("7002.15", "Fee Reminder", "../Education/duefee", 3, "", "", false, "-", "sfee", "edumain", 5, Convert.ToDecimal(71.08), true);// transfer to fee receipt
        Create_Icon("7002.16", "Payment Mode", "../Mst/master_ctrl", 3, "", "", false, "-", "sfee", "edumain", 5, Convert.ToDecimal(71.08), true);// transfer to fee receipt
        Create_Icon("7002.17", "Fee Head", "../Education/feehead", 3, "", "", false, "-", "feemst", "edumain", 5, Convert.ToDecimal(71.02), true);
        Create_Icon("7002.18", "Fees Fine Setting & Slab New", "../Education/late_fee", 3, "", "", false, "-", "feemst", "edumain", 5, Convert.ToDecimal(71.05), true);//bd

        Create_Icon("7003", "Result Master", "", 2, "fa fa-trophy", "nav child_menu", true, "rstmst", "main", "edumain", 5, 20, true);

        //Create_Icon("7003.1", "Result Master", "../../../../erp/school_admin/resultmaster", 3, "", "", false, "-", "rstmst", "edumain", 5, Convert.ToDecimal(72.01), true);
        Create_Icon("7003.1", "Result Master", "../Education/result_mst", 3, "", "", false, "-", "rstmst", "edumain", 5, Convert.ToDecimal(72.01), true);
        Create_Icon("7003.2", "Exam Name", "../Education/ExamName", 3, "", "", false, "-", "rstmst", "edumain", 5, Convert.ToDecimal(72.01), true);

        Create_Icon("7004", "School Master", "", 2, "fa fa-wrench", "nav child_menu", true, "schmst", "main", "edumain", 5, 18, true);
        Create_Icon("7004.1", "Category & Type", "", 3, "fa fa-user", "nav child_menu", true, "schm1", "schmst", "edumain", 5, 73, true);

        Create_Icon("7004.1.1", "Education Category", "../Education/edu_category", 4, "", "", false, "-", "schm1", "edumain", 5, Convert.ToDecimal(73.02), true);
        Create_Icon("7004.1.2", "Caste Category", "../mst/master_ctrl", 4, "", "", false, "-", "schm1", "edumain", 5, Convert.ToDecimal(73.03), true);
        Create_Icon("7004.1.3", "Lunch Timings", "../Mst/master_ctrl", 4, "", "", false, "-", "schm1", "edumain", 5, Convert.ToDecimal(73.4), true);
        Create_Icon("7004.1.4", "Section", "../Mst/master_ctrl", 4, "", "", false, "-", "schm1", "edumain", 5, Convert.ToDecimal(73.5), true);
        Create_Icon("7004.1.5", "Caste", "../mst/master_ctrl", 4, "", "", false, "-", "schm1", "edumain", 5, Convert.ToDecimal(73.03), true);
        Create_Icon("4007.5", "Religion Master", "../mst/master_ctrl", 4, "", "", false, "", "schm1", "edumain", 5, 50, true);


        //Create_Icon("7004.1.5", "Payment Mode", "../../../../erp/school_admin/School_Master", 4, "", "", false, "-", "schm1", "edumain", 5, Convert.ToDecimal(73.7), true);
        Create_Icon("7004.1.6", "Subject", "../Mst/master_ctrl", 4, "", "", false, "-", "schm1", "edumain", 5, Convert.ToDecimal(73.8), true);

        Create_Icon("7004.1.7", "Qualification", "../Mst/master_ctrl", 4, "", "", false, "-", "schm1", "edumain", 5, Convert.ToDecimal(73.9), true);

        Create_Icon("7004.1.8", "Lecture Type", "../Mst/master_ctrl", 4, "", "", false, "-", "schm1", "edumain", 5, Convert.ToDecimal(73.10), true);
        Create_Icon("7004.1.9", "Health Checkup Type", "../Mst/master_ctrl", 4, "", "", false, "-", "schm1", "edumain", 5, Convert.ToDecimal(73.10), true);
        Create_Icon("7004.2", "Academic Year", "../Education/ac_yr", 3, "", "", false, "-", "schmst", "edumain", 5, Convert.ToDecimal(73.11), true);

        //Create_Icon("7004.3", "Admission Master", "", 3, "", "nav child_menu", true, "sadm", "schmst", "edumain", 5, 73, true);
        //Create_Icon("7004.3.1", "Registration No.pattern/Settings", "../../../../erp/school_admin/Frm_Create_RegNo", 4, "", "", false, "-", "sadm", "edumain", 5, Convert.ToDecimal(73.12), true);
        //Create_Icon("7004.3.2", "Roll No.pattern/Settings", "../../../../erp/school_admin/Frm_Create_RollNo", 4, "", "", false, "-", "sadm", "edumain", 5, Convert.ToDecimal(73.13), true);

        Create_Icon("7004.4", "Holiday Master", "", 3, "", "nav child_menu", true, "shol", "schmst", "edumain", 5, 73, true);
        Create_Icon("7004.4.1", "Holiday Master", "../Education/holiday_master", 4, "", "", false, "-", "shol", "edumain", 5, Convert.ToDecimal(73.14), true);

        Create_Icon("7004.5", "Tehsil Master", "../mst/address_master", 3, "", "", false, "-", "schmst", "edumain", 5, Convert.ToDecimal(73.15), true);

        Create_Icon("7004.6", "Class Master", "", 3, "", "nav child_menu", true, "scla", "schmst", "edumain", 5, 73, true);
        Create_Icon("7004.6.1", "Class Master", "../Education/class_master", 4, "", "", false, "-", "scla", "edumain", 5, Convert.ToDecimal(73.15), true);

        Create_Icon("7004.7", "Section Strength", "", 3, "", "nav child_menu", true, "sstr", "schmst", "edumain", 5, 73, true);
        Create_Icon("7004.7.1", "Section Strength", "../Education/section_strength", 4, "", "", false, "-", "sstr", "edumain", 5, Convert.ToDecimal(73.16), true);

        //Create_Icon("7004.8", "User Id Pattern", "", 3, "", "nav child_menu", true, "suser", "schmst", "edumain", 5, 73, true);
        //Create_Icon("7004.8.1", "User Id Pattern", "../../../../erp/school_admin/Create_User_Pattern", 4, "", "", false, "-", "suser", "edumain", 5, Convert.ToDecimal(73.17), true);
        Create_Icon("7004.9", "Village Master", "../mst/address_master", 3, "", "", false, "-", "schmst", "edumain", 5, Convert.ToDecimal(73.18), true);
        Create_Icon("7004.10", "ClassWise Subject", "", 3, "", "nav child_menu", true, "scls", "schmst", "edumain", 5, 73, true);
        Create_Icon("7004.10.1", "Subjects Detail", "../../../../erp/school_admin/ClassWise_Subjects", 4, "", "", false, "-", "scls", "edumain", 5, Convert.ToDecimal(73.19), true);
        Create_Icon("7004.10.2", "Subjects Detail", "../Education/class_sub", 4, "", "", false, "-", "schmst", "edumain", 5, Convert.ToDecimal(73.18), true);

        Create_Icon("7004.12", "Leave Reason Master", "../Mst/master_ctrl", 3, "", "", false, "-", "schmst", "edumain", 5, Convert.ToDecimal(73.22), true);
        // Create_Icon("7004.13", "Class Room Master", "../../../../erp/Add_confrnceroom_details", 3, "", "", false, "-", "schmst", "edumain", 5, Convert.ToDecimal(73.23), true);
        Create_Icon("7004.14", "Copy Work Profile", "../../../../erp/school_admin/Copy_Wrk_Profile", 3, "", "", false, "-", "schmst", "edumain", 5, Convert.ToDecimal(73.11), true);
        Create_Icon("7004.15", "Doc No Pattern", "../Education/pattern_gen", 3, "", "", false, "-", "schmst", "edumain", 5, Convert.ToDecimal(73.11), true);

        Create_Icon("7004.16", "SMS Config", "", 3, "", "nav child_menu", true, "sms", "schmst", "edumain", 5, 73, true);
        Create_Icon("7004.16.1", "SMS Template", "", 4, "", "", false, "-", "sms", "edumain", 5, Convert.ToDecimal(73.12), true);
        Create_Icon("7004.16.2", "SMS Settings", "../Mst/smsapi", 4, "", "", false, "-", "sms", "edumain", 5, Convert.ToDecimal(73.13), true);
        Create_Icon("7004.16.3", "SMS Alert", "../Mst/alert_config", 4, "", "", false, "-", "sms", "edumain", 5, Convert.ToDecimal(73.13), true);

        Create_Icon("7004.17", "Form Control", "../Mst/form_ctrl", 3, "", "", false, "-", "schmst", "edumain", 5, Convert.ToDecimal(73.15), true);

        Create_Icon("7005", "Admission Process", "", 2, "fa fa-sign-in", "nav child_menu", true, "sadmpr", "main", "edumain", 5, 9, true);

        //Create_Icon("7005.1", "Registration", "../../../../erp/Student/student_registration_form", 3, "", "", false, "-", "sadmpr", "edumain", 5, Convert.ToDecimal(74.01), true);
        // Create_Icon("7005.2", "Registration Confirmed", "../../../../erp/school_admin/Frm_Applied_Reg_Students", 3, "", "", false, "-", "sadmpr", "edumain", 5, Convert.ToDecimal(74.02), true);
        Create_Icon("7005.2", "Registration Confirmed", "../Education/std_confirm", 3, "", "", false, "-", "sadmpr", "edumain", 5, Convert.ToDecimal(74.02), true);
        // Create_Icon("7005.3", "Section Allocation", "../../../../erp/school_admin/applied_student_list", 3, "", "", false, "", "sadmpr", "edumain", 5, Convert.ToDecimal(74.03), true);
        Create_Icon("7005.3", "Section Allocation", "../Education/section_alloc", 3, "", "", false, "", "sadmpr", "edumain", 5, Convert.ToDecimal(74.03), true);

        // Create_Icon("7005.4", "Generate Roll No.", "../../../../erp/school_admin/Frm_Generate_RollNo", 3, "", "", false, "", "sadmpr", "edumain", 5, Convert.ToDecimal(74.04), true);
        Create_Icon("7005.4", "Generate Roll No.", "../Education/g_rollno", 3, "", "", false, "", "sadmpr", "edumain", 5, Convert.ToDecimal(74.04), true);
        Create_Icon("7005.5", "List Of Pending Applicants", "", 3, "", "", false, "", "sadmpr", "edumain", 5, Convert.ToDecimal(74.04), true, "onclick=$menuclick(this);$");
        Create_Icon("7005.6", "List of Students", "", 3, "", "", false, "", "sadmpr", "edumain", 5, Convert.ToDecimal(74.04), true, "onclick=$menuclick(this);$");
        Create_Icon("7005.7", "Blocked Students", "../Education/blocked_student", 3, "", "", false, "", "sadmpr", "edumain", 5, Convert.ToDecimal(74.04), true);
        //Create_Icon("7005.8", "Student Template", "../../../../erp/Student/student_template", 3, "", "", false, "", "sadmpr", "edumain", 5, Convert.ToDecimal(74.04), true);
        Create_Icon("7005.9", "Registration", "../Education/std_registration", 3, "", "", false, "", "sadmpr", "edumain", 5, Convert.ToDecimal(74.04), true);
        Create_Icon("7005.10", "Section / Roll No Allocation", "../Education/sec_roll_alloc", 3, "", "", false, "", "sadmpr", "edumain", 5, Convert.ToDecimal(74.03), true);

        //Create_Icon("7005", "Student Registration", "../../../../erp/Student/student_registration_form", 2, "fa fa-user", "", false, "", "main", "edumain", 5, 74, true);
        //Create_Icon("7006", "Student Registration Confiremed", "../../../../erp/school_admin/Frm_Applied_Reg_Students", 2, "fa fa-user", "", false, "", "main", "edumain", 5, 75, true);

        Create_Icon("7006", "Accounts", "", 2, "fa fa-calculator", "nav child_menu", true, "acctedu", "main", "schadmmain", 5, 19, true);

        Create_Icon("7006.1", "Expense Details", "../Education/expense_details", 3, "", "", false, "-", "acctedu", "schadmmain", 5, Convert.ToDecimal(71.01), true);

        //Create_Icon("7007", "Student Section Allocation", "../../../../erp/school_admin/applied_student_list", 2, "fa fa-user", "", false, "", "main", "edumain", 5, 76, true);
        //Create_Icon("7008", "Generate Roll No.", "../../../../erp/school_admin/Frm_Generate_RollNo", 2, "fa fa-user", "", false, "", "main", "edumain", 5, 77, true);


        // Create_Icon("7009", "ScholarShip Master", "../../../../erp/school_admin/Frm_Scholarship_master", 2, "fa fa-user", "", false, "-", "main", "edumain", 5, 78, true);

        // Create_Icon("7010", "Holiday Master", "../../../../erp/school_admin/frm_holidaymaster", 2, "fa fa-user", "", false, "-", "main", "edumain", 5, 79, true);

        //Create_Icon("7012", "Time Table", "../../../../erp/school_admin/Frm_Class_TimeTable", 2, "fa fa-calendar", "", false, "-", "main", "edumain", 5, 80, true);

        Create_Icon("7014", "Results & Promotions", "", 2, "fa fa-mortar-board", "nav child_menu", true, "sresult", "main", "edumain", 5, 16, true);
        Create_Icon("7014.1", "Result Entry", "../../../../erp/directlink/Result_app", 3, "", "", false, "-", "sresult", "edumain", 5, Convert.ToDecimal(82.01), true);
        Create_Icon("7014.2", "Promoted To Next Class", "../../../../erp/school_admin/Frm_Promoted_Students", 3, "", "", false, "-", "sresult", "edumain", 5, Convert.ToDecimal(82.02), true);
        Create_Icon("7014.3", "Datesheet", "../../../../erp/school_admin/date_sheet", 3, "", "", false, "-", "sresult", "edumain", 5, Convert.ToDecimal(82.02), true);
        Create_Icon("7014.4", "Result Approval", "../../../../erp/directlink/Result_app", 3, "", "", false, "-", "sresult", "edumain", 5, Convert.ToDecimal(82.02), true);
        Create_Icon("7014.5", "Admit Card", "../Education/std_cards", 3, "", "", false, "-", "sresult", "edumain", 5, Convert.ToDecimal(82.02), true);


        // Create_Icon("7013", "Promoted To Next Class", "../../../../erp/school_admin/Frm_Promoted_Students", 2, "fa fa-user", "", false, "", "main", "edumain", 5, 81, true);
        //Create_Icon("7014", "Result InPut", "../../../../erp/school_admin/Frm_ResultInput", 2, "fa fa-user", "", false, "", "main", "edumain", 5, 82, true);


        Create_Icon("7015", "Attendance", "", 2, "fa fa-check-square-o", "nav child_menu", true, "stdatt", "main", "edumain", 5, 10, true);
        Create_Icon("7015.1", "Attendance", "../Education/student_attendence", 3, "", "", false, "", "stdatt", "edumain", 5, Convert.ToDecimal(83.01), true);
        Create_Icon("7015.2", "Leave Apply", "../Education/leaveapplication", 3, "", "", false, "", "stdatt", "edumain", 5, Convert.ToDecimal(83.02), true);
        Create_Icon("7015.3", "Leave Confirmation", "../Education/l_apr", 3, "", "", false, "", "stdatt", "edumain", 5, Convert.ToDecimal(83.03), true);

        Create_Icon("7016", "Time Table", "../../../../erp/SchoolReports_Forms/student_Time_Table", 2, "fa fa-calendar", "", false, "", "main", "edumain", 5, 15, true);
        Create_Icon("7017", "Template Master", "", 2, "fa fa-certificate", "nav child_menu", true, "stmas", "main", "edumain", 5, 21, true);
        Create_Icon("7017.1", "Template Master", "../../../../erp/SchoolReports_Forms/template_master", 3, "", "", false, "-", "stmas", "edumain", 5, Convert.ToDecimal(85.01), true);
        Create_Icon("7017.3", "Header Master", "../../../../erp/SchoolReports_Forms/header_designer", 3, "", "", false, "-", "stmas", "edumain", 5, Convert.ToDecimal(85.01), true);
        Create_Icon("7017.2", "Create Certificate", "../../../../erp/SchoolReports_Forms/pic_template", 3, "", "", false, "", "stmas", "edumain", 5, Convert.ToDecimal(85.02), true);
        Create_Icon("7017.4", "Home Work Template", "../../../../erp/SchoolReports_Forms/home_work_report", 3, "", "", false, "", "stmas", "edumain", 5, Convert.ToDecimal(85.02), true);
        Create_Icon("7017.5", "Reports Template", "../Education/rpt_temp", 3, "", "", false, "", "stmas", "edumain", 5, Convert.ToDecimal(85.02), true);


        //Create_Icon("7016", "Create Sub User Profile", "../../../../erp/school_admin/corp_admin/list_of_trainer_trainee", 2, "fa fa-user", "", false, "-", "main", "edumain", 5, 84, true);
        //Create_Icon("7017", "Admission Fee", "../../../../erp/school_academic_staff/Frm_Admission_Fees", 2, "fa fa-user", "", false, "", "main", "edumain", 5, 85, true);
        //Create_Icon("7018", "Admission Fee Approval", "../../../../erp/school_academic_staff/Frm_Approval_AdmissionFee", 2, "fa fa-user", "", false, "", "main", "edumain", 5, 86, true);
        Create_Icon("7019", "Fee Receipt", "", 2, "fa fa-money", "nav child_menu", true, "sfee", "main", "edumain", 5, 12, true);
        //Create_Icon("7019.1", "Regular Fee", "../../../../erp/school_academic_staff/admin_fee_receipt", 3, "", "", false, "-", "sfee", "edumain", 5, Convert.ToDecimal(87.01), true);
        Create_Icon("7019.10", "Fees For Mid Term", "../../../../erp/school_academic_staff/admin_fee_receipt", 3, "", "", false, "-", "sfee", "edumain", 5, Convert.ToDecimal(87.01), true);
        Create_Icon("7019.6", "Regular Fee Concession", "../Education/fee_receipt", 3, "", "", false, "-", "sfee", "edumain", 5, Convert.ToDecimal(87.06), true);
        Create_Icon("7019.2", "Admission Fee", "../../../../erp/school_academic_staff/Frm_Admission_Fees", 3, "", "", false, "", "sfee", "edumain", 5, Convert.ToDecimal(87.02), true);
        // Create_Icon("7019.3", "Admission Fee Approval", "../../../../erp/school_academic_staff/Frm_Approval_AdmissionFee", 3, "", "", false, "", "sfee", "edumain", 5, Convert.ToDecimal(87.03), true);Create_Icon("7019.3", "Admission Fee Approval", "../../../../erp/school_academic_staff/Frm_Approval_AdmissionFee", 3, "", "", false, "", "sfee", "edumain", 5, Convert.ToDecimal(87.03), true);
        Create_Icon("7019.3", "Admission Fee Approval", "../Education/adm_app", 3, "", "", false, "", "sfee", "edumain", 5, Convert.ToDecimal(87.03), true); Create_Icon("7019.3", "Admission Fee Approval", "../../../../erp/school_academic_staff/Frm_Approval_AdmissionFee", 3, "", "", false, "", "sfee", "edumain", 5, Convert.ToDecimal(87.03), true);
        Create_Icon("7019.5", "Regular Fee", "../Education/fee_receipt_new", 3, "", "", false, "-", "sfee", "edumain", 5, Convert.ToDecimal(87.04), true);
      //  Create_Icon("7019.5", "Regular Fee", "../Education/fee_receipt", 3, "", "", false, "-", "sfee", "edumain", 5, Convert.ToDecimal(87.04), true);
        //Create_Icon("7019.7", "Regular Fee Consession Group", "../../../../erp/school_academic_staff/admin_fee_receipt_grp", 3, "", "", false, "-", "sfee", "edumain", 5, Convert.ToDecimal(87.07), true);
        Create_Icon("7019.8", "Fee Reversal", "../../../../erp/school_academic_staff/admin_fee_receipt", 3, "", "", false, "-", "sfee", "edumain", 5, Convert.ToDecimal(87.07), true);
        //Create_Icon("7019.9", "Mis. Fees", "../../../../erp/school_academic_staff/Mis_fee_receipt", 3, "", "", false, "-", "sfee", "edumain", 5, Convert.ToDecimal(87.07), true);
        Create_Icon("7019.9", "Mis. Fees", "../Education/mis_fee", 3, "", "", false, "-", "sfee", "edumain", 5, Convert.ToDecimal(87.07), true);
        Create_Icon("7019.11", "Conc Reversal", "../Education/fee_receipt", 3, "", "", false, "-", "sfee", "edumain", 5, Convert.ToDecimal(87.06), true);
        Create_Icon("7019.12", "Regular Fees 2", "../Education/fee_receipt", 3, "", "", false, "-", "sfee", "edumain", 5, Convert.ToDecimal(87.06), true);

        Create_Icon("7020", "Reports", "", 2, "fa fa-briefcase", "nav child_menu", true, "SchRpt", "main", "edumain", 5, 17, true);
       // Create_Icon("7020.1", "Applicant Detail", "../../../../erp/SchoolReports_Forms/Applicant_Rpts", 3, "", "", false, "-", "SchRpt", "edumain", 5, 17, true);
        Create_Icon("7020.1", "Applicant Detail", "../Education/applicant_rpt", 3, "", "", false, "-", "SchRpt", "edumain", 5, 17, true);
        Create_Icon("7020.2", "Student ICard", "../Education/std_cards", 3, "", "", false, "-", "SchRpt", "edumain", 5, Convert.ToDecimal(88.02), true);
       // Create_Icon("7020.3", "Student Attendance", "../../../../erp/SchoolReports_Forms/Student_attendance_report", 3, "", "", false, "-", "SchRpt", "edumain", 5, Convert.ToDecimal(88.03), true);
        Create_Icon("7020.3", "Student Attendance", "../Education/atten_rpt", 3, "", "", false, "-", "SchRpt", "edumain", 5, Convert.ToDecimal(88.03), true);
       // Create_Icon("7020.4", "Student Fee ", "../../../../erp/SchoolReports_Forms/Fees_Rpts", 3, "", "", false, "-", "SchRpt", "edumain", 5, Convert.ToDecimal(88.04), true);
        Create_Icon("7020.4", "Student Fee ", "../Education/fee_rpts", 3, "", "", false, "-", "SchRpt", "edumain", 5, Convert.ToDecimal(88.04), true);
        // Create_Icon("7020.6", "Result Report", "../../../../erp/SchoolReports_Forms/Result_Rpts", 3, "", "", false, "-", "SchRpt", "edumain", 5, Convert.ToDecimal(88.04), true);      
        Create_Icon("7020.6", "Result Report", "../Education/result_rpts", 3, "", "", false, "-", "SchRpt", "edumain", 5, Convert.ToDecimal(88.04), true);
        Create_Icon("7020.8", "Choose Template", "../Purchase/purchase_order", 3, "", "", false, "-", "SchRpt", "edumain", 5, Convert.ToDecimal(88.03), true);
        Create_Icon("35001.9", "new rpt", "../Education/fee_rpts", 3, "", "", false, "-", "SchRpt", "edumain", 5, Convert.ToDecimal(88.03), true);
        Create_Icon("7020.9", "School Strength", "../Education/s_strength", 3, "", "", false, "-", "SchRpt", "edumain", 5, Convert.ToDecimal(88.04), true);
        Create_Icon("7020.10", "Attendance Summary", "../Education/Att_summ", 3, "", "", false, "-", "SchRpt", "edumain", 5, Convert.ToDecimal(88.04), true);

        Create_Icon("7021", "Teacher Detail", "", 2, "fa fa-user", "nav child_menu", true, "stch", "main", "edumain", 5, 14, true);
        Create_Icon("7021.1", "Weekly Off", "../Education/teac_wo", 3, "", "", false, "-", "stch", "edumain", 5, Convert.ToDecimal(89.01), true);
        Create_Icon("7021.2", "Subject Allocation", "../../../../erp/school_admin/Frm_teacher_subject", 3, "", "", false, "-", "stch", "edumain", 5, Convert.ToDecimal(89.02), true);
        Create_Icon("7021.3", "Class Time Table", "../../../../erp/school_admin/TimeTable", 3, "", "", false, "-", "stch", "edumain", 5, Convert.ToDecimal(89.03), true);
        Create_Icon("7021.4", "Time Table Report", "../../../../erp/SchoolReports_Forms/Time_Table_Detail", 3, "", "", false, "-", "stch", "edumain", 5, Convert.ToDecimal(89.03), true);
        Create_Icon("7021.5", "Holiday Master", "../Education/teac_hm", 3, "", "", false, "-", "stch", "edumain", 5, Convert.ToDecimal(89.03), true);
        Create_Icon("7021.6", "Teacher Attendance", "../Education/student_attendence", 3, "", "", false, "-", "stch", "edumain", 5, Convert.ToDecimal(89.03), true);
        Create_Icon("7021.7", "Leave Apply", "../Education/leaveapplication", 3, "", "", false, "-", "stch", "edumain", 5, Convert.ToDecimal(89.03), true);
        Create_Icon("7021.8", "Subject Allocation (New)", "../Education/teacher_sub", 3, "", "", false, "-", "stch", "edumain", 5, Convert.ToDecimal(89.03), true);

        Create_Icon("7034", "School Directory", "", 2, "fa fa-thumb-tack", "nav child_menu", false, "fees", "kbank", "edumain", 5, 13, true);

        Create_Icon("7036", "Health Checkup", "../Education/health_checkup", 2, "fa fa-exclamation-circle", "", false, "", "main", "edumain", 5, 17, true);
        //Create_Icon("7037", "Home Work", "../../../../erp/school_admin/Assign_HomeWork", 2, "fa fa-exclamation-circle", "", false, "", "main", "edumain", 5, 89, true);


        Create_Icon("7037", "Home Work", "", 2, "fa fa-exclamation-circle", "nav child_menu", true, "sch_hw", "main", "edumain", 5, 11, true);
        Create_Icon("7037.1", "Add Home Work", "../education/mess", 3, "", "", false, "-", "sch_hw", "edumain", 5, Convert.ToDecimal(88.01), true);
        //Create_Icon("7037.2", "Print Home Work", "../../../../erp/SchoolReports_Forms/view_homework_report", 3, "", "", false, "-", "sch_hw", "edumain", 5, Convert.ToDecimal(88.02), true);

        Create_Icon("7038", "Transport Master", "", 2, "fa fa-exclamation-circle", "nav child_menu", true, "sch_trprt", "main", "edumain", 5, 11, true);
        Create_Icon("7038.1", "Location/Stoppage Master", "../Education/location_mst", 3, "", "", false, "-", "sch_trprt", "edumain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("7038.2", "Transport Fee Master", "../Education/transport_feemaster", 3, "", "", false, "-", "sch_trprt", "edumain", 5, Convert.ToDecimal(88.04), true);

        Create_Icon("7039", "Exam Room Mgmt", "", 2, "fa fa-exclamation-circle", "nav child_menu", true, "exrm", "main", "edumain", 5, 17, true);
        Create_Icon("7039.1", "Exam Room Master", "../Education/ER_Master", 3, "", "", false, "-", "exrm", "edumain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("7039.2", "Exam Room Allocation", "../Education/ER_Allocation", 3, "", "", false, "-", "exrm", "edumain", 5, Convert.ToDecimal(88.04), true);

        Create_Icon("7040", "PTM", "", 2, "fa fa-exclamation-circle", "nav child_menu", true, "ptm", "main", "edumain", 5, 17, true);
        Create_Icon("7040.1", "PTM Report", "../Education/student_remark_rpt", 3, "", "", false, "-", "ptm", "edumain", 5, Convert.ToDecimal(88.03), true);
        Create_Icon("7040.2", "PTM Discussion", "../Education/student_remark", 3, "", "", false, "-", "ptm", "edumain", 5, Convert.ToDecimal(88.04), true);

        Create_Icon("7041", "Online Classes", "", 2, "fa fa-television", "nav child_menu", true, "oclass", "main", "edumain", 5, 17, true);
        Create_Icon("7041.1", "Broadcast Meeting", "../home/v_classes2", 3, "", "", false, "-", "oclass", "edumain", 5, Convert.ToDecimal(88.03), true);
        Create_Icon("7041.2", "Attend Meeting", "../home/v_classes2", 3, "", "", false, "-", "oclass", "edumain", 5, Convert.ToDecimal(88.03), true);

        #endregion

        //main module------------------------------------
        Create_Icon("8000", "Employee Self Service", "../../../../erp/dashboard", 1, "", "", true, "exp", "exmain", "exmain", 5, 8, true, "A system to process, pay and audit employee-initiated", "Manage the expenses claim, authorization and repayment process.");
        #region child menu
        //child menu-------------------------------------
        Create_Icon("8001", "Expense Master", "", 2, "fa fa-user", "nav child_menu", true, "expm", "exmain", "exmain", 5, Convert.ToDecimal(80.01), true);
        Create_Icon("8001.1", "Nature Of Expense", "../../../../../erp/expense/expense_master", 3, "", "", false, "", "expm", "exmain", 5, Convert.ToDecimal(80.01), true);
        //Create_Icon("8001.2", "Approval Person", "../../../../../erp/expense/expense_master", 3, "", "", false, "", "expm", "exmain", 5, Convert.ToDecimal(80.01), true);
        Create_Icon("8001.2", "Approval Person", "../hr/create_team", 3, "", "", false, "", "expm", "exmain", 5, Convert.ToDecimal(80.01), true);
        Create_Icon("8001.3", "Mode Of Transport", "../Mst/master_ctrl", 3, "", "", false, "", "expm", "exmain", 5, Convert.ToDecimal(80.01), true);
        Create_Icon("8001.4", "Nature Of Expense Return", "../../../../../erp/expense/expense_master", 3, "", "", false, "", "expm", "exmain", 5, Convert.ToDecimal(80.01), true);
        Create_Icon("8001.5", "Separation Region", "../Mst/master_ctrl", 3, "", "", false, "", "expm", "exmain", 5, Convert.ToDecimal(80.01), true);


        Create_Icon("8002", "Expense", "", 2, "fa fa-user", "nav child_menu", true, "exp", "exmain", "exmain", 5, Convert.ToDecimal(80.01), true);
        Create_Icon("8002.1", "Daywise Expense", "../../../../../erp/expense/add_expense", 3, "", "", false, "", "exp", "exmain", 5, Convert.ToDecimal(80.01), true);
        Create_Icon("8002.2", "Bulk Expense", "../../../../../erp/expense/bulk_expense", 3, "", "", false, "", "exp", "exmain", 5, Convert.ToDecimal(80.01), true);
        Create_Icon("8002.3", "Daywise Expense New", "../hall/add_exp", 3, "", "", false, "", "exp", "exmain", 5, Convert.ToDecimal(80.01), true);
        Create_Icon("8002.4", "Bulk Expense New", "../hall/bulk_exp", 3, "", "", false, "", "exp", "exmain", 5, Convert.ToDecimal(80.01), true);
        Create_Icon("8002.5", "Approve Expense New", "../hall/exp_app", 3, "", "", false, "", "exp", "exmain", 5, Convert.ToDecimal(80.01), true);

        //Create_Icon("8003", "Help Desk", "", 2, "fa fa-bank", "nav child_menu", true, "hdesk", "exmain", "exmain", 5, 88, true);
        Create_Icon("8003", "Help Desk", "", 2, "fa fa-bank", "nav child_menu", true, "hdesk", "exmain", "exmain", 5, Convert.ToDecimal(80.01), true);
        Create_Icon("8003.1", "Help Type Master", "../mst/master_ctrl", 3, "", "", false, "", "hdesk", "exmain", 5, 110, true);
        Create_Icon("8003.2", "Helpdesk Workflow", "../hall/hd_wf", 3, "", "", false, "", "hdesk", "exmain", 5, 110, true);
        Create_Icon("8003.3", "FAQ Master", "../hall/faq_m", 3, "", "", false, "", "hdesk", "exmain", 5, 110, true);
        Create_Icon("8003.4", "Helpdesk FAQ", "../hall/faq_h", 3, "", "", false, "", "hdesk", "exmain", 5, 110, true);
        Create_Icon("8003.5", "Add Queries", "../hall/faq_add", 3, "", "", false, "", "hdesk", "exmain", 5, 110, true);


        Create_Icon("8004", "Office Reporting", "", 2, "fa fa-user", "nav child_menu", true, "exor", "exmain", "exmain", 5, Convert.ToDecimal(80.01), true);
        Create_Icon("8004.1", "Daily Report", "../hr/daily_work", 3, "", "", false, "", "exor", "exmain", 5, 110, true);
        Create_Icon("8004.2", "Admin Report", "../hr/adm_rpt", 3, "", "", false, "", "exor", "exmain", 5, 110, true);
        Create_Icon("8004.3", "Previous Report", "../hr/prvs_rpt", 3, "", "", false, "", "exor", "exmain", 5, 110, true);

        Create_Icon("8005", "Human Resource", "", 2, "fa fa-bank", "", false, "essmain", "exmain", "exmain", 5, 88, true);
        Create_Icon("8005.1", "Open Postion", "../hr/open_p", 3, "", "", false, "", "essmain", "exmain", 5, 110, true);
        Create_Icon("8005.2", "Interview Conduct", "../hr/intr", 3, "", "", false, "", "essmain", "exmain", 5, 50, true);
        Create_Icon("8005.3", "Mediclaim Detail", "../hr/med_ins", 3, "", "", false, "", "essmain", "exmain", 5, 50, true);
        Create_Icon("8005.4", "My Profile", "../hr/my_prf", 3, "", "", false, "", "essmain", "exmain", 5, 50, true);
        Create_Icon("8005.5", "Open Postion-Approve", "../hr/pos_app", 3, "", "", false, "", "essmain", "exmain", 5, 110, true);
        Create_Icon("8005.6", "My Profile_n", "../hall/mr_prf2", 3, "", "", false, "", "essmain", "exmain", 5, 110, true);

        Create_Icon("8006", "Application", "", 2, "fa fa-bank", "", false, "aplmain", "exmain", "exmain", 5, 88, true);
        Create_Icon("8006.1", "Leave Applied", "../Education/leaveapplication", 3, "", "", false, "", "aplmain", "exmain", 5, 110, true);
        Create_Icon("8006.2", "Loans/Advance", "../Hr/loan_appl", 3, "", "", false, "", "aplmain", "exmain", 5, 110, true);
        Create_Icon("8006.3", "OD Slip", "../Hr/od_slip", 3, "", "", false, "", "aplmain", "exmain", 5, 110, true);
        Create_Icon("8006.4", "Separation", "../Hr/separt", 3, "", "", false, "", "aplmain", "exmain", 5, 110, true);
       

        Create_Icon("8007", "Approval", "", 2, "fa fa-bank", "", false, "aprmain", "exmain", "exmain", 5, 88, true);
        Create_Icon("8007.1", "Leave Approval", "../Education/l_apr", 3, "", "", false, "", "aprmain", "exmain", 5, 110, true);
        Create_Icon("8007.2", "Loan Approval", "../hr/loan_apr", 3, "", "", false, "", "aprmain", "exmain", 5, 110, true);
        Create_Icon("8007.3", "OD Slip Approval", "../hr/od_apr", 3, "", "", false, "", "aprmain", "exmain", 5, 110, true);
        Create_Icon("8007.4", "Separation Approval", "../hr/sep_apr", 3, "", "", false, "", "aprmain", "exmain", 5, 110, true);

        Create_Icon("8008", "Visitor Reg", "../Hr/v_reg", 2, "fa fa-bank", "", false, "vstmain", "exmain", "exmain", 5, 88, true);
        Create_Icon("8008.1", "Visitor Registraion", "../hr/v_form", 3, "", "", false, "", "vstmain", "exmain", 5, 110, true);

        Create_Icon("8009", "Reporting Structure", "../Hr/team", 2, "fa fa-bank", "", false, "tmmain", "exmain", "exmain", 5, 88, true);
        Create_Icon("80010", "Asset With Me", "", 2, "fa fa-clipboard", "", false, "", "exmain", "exmain", 5, 10, true, "onclick=$menuclick(this);$");

        Create_Icon("8010", "Kaizen Program", "", 2, "fa fa-bank", "", false, "kzmain", "exmain", "exmain", 5, 88, true);
        Create_Icon("8010.1", "Kaizen Submit", "../Hr/kz_submit", 3, "", "", false, "", "kzmain", "exmain", 5, 110, true);
        Create_Icon("8010.2", "Kaizen Approval", "../Hr/kz_apr", 3, "", "", false, "", "kzmain", "exmain", 5, 110, true);
        Create_Icon("8010.3", "Kaizen Report", "../Hr/kz_rpt", 3, "", "", false, "", "kzmain", "exmain", 5, 110, true);

       





        #endregion

        //main module------------------------------------
        Create_Icon("9000", "Payroll", "../../../../erp/dashboard", 1, "", "", true, "pay", "paymain", "paymain", 5, 9, true, "Define integrated HR & ESS portal", "Create single ocean for the employee of any activity or requirement");
        // child menu-------------------------------------  
        #region payroll
        //child menu-------------------------------------
        Create_Icon("9001", "Declarations", "", 2, "fa fa-pencil", "", true, "paydc", "paymain", "paymain", 5, 9, true);
        Create_Icon("9001.1", "Employee Declaration", "../../../../../erp/hr/income_tax", 3, "", "", false, "", "paydc", "paymain", 5, 50, true);
        Create_Icon("9001.2", "Employee Declaration new", "../hall/inc_tax", 3, "", "", false, "", "paydc", "paymain", 5, 50, true);


        Create_Icon("9001.3", "Challan", "../hall/add_chl", 3, "", "", false, "", "paydc", "paymain", 5, 50, true);
        Create_Icon("9002", "Leave Master", "", 2, "fa fa-user", "nav child_menu", true, "lmas", "paymain", "paymain", 5, 90, true);
        #region leave master
        Create_Icon("9002.1", "Leave Config", "../../../../../erp/hr/leave_config", 3, "", "", false, "-", "lmas", "paymain", 5, Convert.ToDecimal(90.01), true);
        Create_Icon("9002.2", "Leave OB Import", "../hr/leave_ob_imp", 3, "", "", false, "-", "lmas", "paymain", 5, Convert.ToDecimal(90.02), true);
        Create_Icon("9002.3", "Attendance Type", "../../../../../erp/hr/import_master", 3, "", "", false, "-", "lmas", "paymain", 5, Convert.ToDecimal(90.02), true);
        Create_Icon("9002.4", "Employee Attendance", "../../../../../erp/hr/emp_attendance", 3, "", "", false, "-", "lmas", "paymain", 5, Convert.ToDecimal(90.02), true);
        //Create_Icon("9002.5", "Leave Year", "../../../../../erp/hr/leave_year", 3, "", "", false, "", "lmas", "paymain", 5, 50, true);
        Create_Icon("9002.5", "Leave Year", "../Hr/leave_year", 3, "", "", false, "", "lmas", "paymain", 5, 50, true);
        Create_Icon("9002.6", "Weekly Holiday", "../Hr/holiday_type", 3, "", "", false, "", "lmas", "paymain", 5, 50, true);
        Create_Icon("9002.7", "General Holiday", "../Hr/holiday_type", 3, "", "", false, "", "lmas", "paymain", 5, 50, true);
        Create_Icon("9002.8", "Attendance Rule", "../Hr/attn_rules", 3, "", "", false, "", "lmas", "paymain", 5, 50, true);
        Create_Icon("9002.9", "List Of Leaves", "../Hr/list_leave", 3, "", "", false, "", "lmas", "paymain", 5, 50, true);
        #endregion

        Create_Icon("9003", "Payroll Master", "", 2, "fa fa-gears", "", true, "paymst", "paymain", "paymain", 5, 9, true);
        #region payroll master
        Create_Icon("9003.1", "Statutory Master", "", 3, "", "nav child_menu", true, "paysta", "paymst", "paymain", 5, 73, true);
        Create_Icon("9003.1.1", "PF Setting", "../Hr/pf_setting", 4, "", "", false, "", "paysta", "paymain", 5, 50, true);
        Create_Icon("9003.1.2", "ESI Setting", "../Hr/esi_setting", 4, "", "", false, "", "paysta", "paymain", 5, 50, true);
        Create_Icon("9003.1.3", "PT Setting", "../Hr/pt_setting", 4, "", "", false, "", "paysta", "paymain", 5, 50, true);
        Create_Icon("9003.1.4", "LWF Setting", "../Hr/lwf_setting", 4, "", "", false, "", "paysta", "paymain", 5, 50, true);
        Create_Icon("9003.1.5", "Statutory Authority", "../hr/st_authority", 4, "", "", false, "", "paysta", "paymain", 5, 50, true);
        #endregion

        Create_Icon("9003.2", "Salary Master", "", 3, "", "nav child_menu", true, "paysm", "paymst", "paymain", 5, 73, true);
        #region salary master
        Create_Icon("9003.2.1", "Salary Components", "../../../../../erp/hr/salary_component", 4, "", "", false, "", "paysm", "paymain", 5, 50, true);
        //Create_Icon("9003.2.2", "Salary Rule", "../../../../../erp/hr/configure_rules", 4, "", "", false, "", "paysm", "paymain", 5, 50, true);
        Create_Icon("9003.2.2", "Salary Rule", "../hr/configure_rule", 4, "", "", false, "", "paysm", "paymain", 5, 50, true);
        Create_Icon("9003.2.3", "Salary Structure", "../../../../../erp/hr/salary_structure", 4, "", "", false, "", "paysm", "paymain", 5, 50, true);
        //Create_Icon("9003.2.4", "Salary Grade", "../../../../../erp/hr/salary_grade", 4, "", "", false, "", "paysm", "paymain", 5, 50, true);
        Create_Icon("9003.2.4", "Salary Grade", "../hr/salary_grade", 4, "", "", false, "", "paysm", "paymain", 5, 50, true);
        //Create_Icon("9003.2.5", "Salary Advance", "../hr/salary_advance", 4, "", "", false, "", "paysm", "paymain", 5, 50, true);
        Create_Icon("9003.2.5", "Salary Advance Policy", "../hr/salary_advance", 4, "", "", false, "", "paysm", "paymain", 5, 50, true);
        //Create_Icon("9003.2.6", "OT Config", "../../../../erp/hr/OT_Config", 4, "", "", false, "-", "paysm", "paymain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("9003.2.6", "OT Config", "../Hr/OT_Config", 4, "", "", false, "-", "paysm", "paymain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("9003.2.7", "Employee Salary", "../../../../../erp/hr/employee_salary", 4, "", "", false, "", "paysm", "paymain", 5, 50, true);
        Create_Icon("9003.2.8", "Bulk Employee Salary", "../../../../erp/hr/bulk_employee_salary", 4, "", "", false, "-", "paysm", "paymain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("9003.2.9", "Salary Print Order", "../Hr/head_orderwise", 4, "", "", false, "-", "paysm", "paymain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("9003.2.10", "Bonus Config", "../Hr/bonus_config", 4, "", "", false, "-", "paysm", "paymain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("9003.2.11", "Actual Salary Master", "../Hr/actual_salary_master", 4, "", "", false, "-", "paysm", "paymain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("9003.2.12", "Salary Slip Print", "../Hr/print_payslip", 4, "", "", false, "-", "paysm", "paymain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("9003.2.13", "Canteen Config", "../Hr/canteen_config", 4, "", "", false, "-", "paysm", "paymain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("9003.2.14", "Icard Print", "../Hr/print_payslip", 4, "", "", false, "-", "paysm", "paymain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("9003.2.15", "Edit Employee Salary", "../Hr/incr_edit", 4, "", "", false, "-", "paysm", "paymain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("9003.2.16", "Salary Structure New", "../Hr/sal_str", 4, "", "", false, "-", "paysm", "paymain", 5, Convert.ToDecimal(88.02), true);
        #endregion

        Create_Icon("9003.3", "General Master", "", 3, "", "nav child_menu", true, "paygm", "paymst", "paymain", 5, 73, true);
        #region general master
        Create_Icon("9003.3.1", "Grade Master", "../Mst/master_ctrl", 4, "", "", false, "", "paygm", "paymain", 5, 50, true);
        Create_Icon("9003.3.2", "Language Master", "../Mst/master_ctrl", 4, "", "", false, "", "paygm", "paymain", 5, 50, true);
        Create_Icon("9003.3.3", "Location Master", "../hr/location_master", 4, "", "", false, "", "paygm", "paymain", 5, 50, true);
        Create_Icon("9003.3.4", "Employee Type", "../Mst/master_ctrl", 4, "", "", false, "", "paygm", "paymain", 5, 50, true);
        Create_Icon("9003.3.5", "Employee Category", "../Mst/master_ctrl", 4, "", "", false, "", "paygm", "paymain", 5, 50, true);
        Create_Icon("9003.3.6", "Village Master", "../mst/address_master", 4, "", "", false, "", "paygm", "paymain", 5, 50, true);
        Create_Icon("9003.3.7", "Tehsil Master", "../mst/address_master", 4, "", "", false, "", "paygm", "paymain", 5, 50, true);
        Create_Icon("9003.3.8", "Pick Drop Master", "../Education/location_mst", 4, "", "", false, "-", "paygm", "paymain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("9003.3.9", "Joining Age", "../hr/exp_master", 4, "", "", false, "-", "paygm", "paymain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("9003.3.10", "Bank Name", "../Mst/master_ctrl", 4, "", "", false, "-", "paygm", "paymain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("9003.3.11", "Employee Code Pattern", "../Education/pattern_gen", 4, "", "", false, "-", "paygm", "paymain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("9003.3.12", "Location Region", "../Mst/master_ctrl", 4, "", "", false, "-", "paygm", "paymain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("4007.2", "Caste Category", "../Mst/master_ctrl", 4, "", "", false, "-", "paygm", "paymain", 5, Convert.ToDecimal(47.02), true);
        Create_Icon("7004.12", "Leave Reason Master", "../Mst/master_ctrl", 4, "", "", false, "-", "paygm", "paymain", 5, Convert.ToDecimal(73.22), true);
        Create_Icon("9003.3.13", "Form Control", "../Mst/form_ctrl", 4, "", "", false, "", "paygm", "paymain", 5, 50, true);
        Create_Icon("9003.3.14", "Report Control", "../Hr/asg_rpt", 4, "", "", false, "", "paygm", "paymain", 5, 50, true);
        Create_Icon("7004.1.5", "Caste", "../mst/master_ctrl", 4, "", "", false, "", "paygm", "paymain", 5, 50, true);
        Create_Icon("4007.5", "Religion Master", "../mst/master_ctrl", 4, "", "", false, "", "paygm", "paymain", 5, 50, true);

        #endregion

        Create_Icon("9008", "Employee Detail", "", 2, "fa fa-user", "nav child_menu", true, "payemp", "main", "paymain", 5, 90, true);
        #region employee details
        Create_Icon("9008.1", "General Details", "../Hr/employee_details", 3, "", "", false, "-", "payemp", "paymain", 5, Convert.ToDecimal(90.01), true);
        Create_Icon("9008.2", "Qualification Details", "../Hr/empdoc", 3, "", "", false, "-", "payemp", "paymain", 5, Convert.ToDecimal(90.02), true);
        Create_Icon("9008.3", "Work Experience Details", "../Hr/empdoc", 3, "", "", false, "-", "payemp", "paymain", 5, Convert.ToDecimal(90.03), true);
        Create_Icon("9008.4", "Hobby Details", "../Hr/empdoc", 3, "", "", false, "-", "payemp", "paymain", 5, Convert.ToDecimal(90.04), true);
        Create_Icon("9008.5", "Add Bulk Employee", "../../../../erp/hr/bulk_employee", 3, "", "", false, "-", "payemp", "paymain", 5, Convert.ToDecimal(90.05), true);
        Create_Icon("9008.6", "Vendor Details", "../Purchase/party", 3, "-", "-", false, "-", "payemp", "paymain", 5, Convert.ToDecimal(90.05), true);
        Create_Icon("9008.7", "Bond Details", "../Hr/empdoc", 3, "", "", false, "-", "payemp", "paymain", 5, Convert.ToDecimal(90.04), true);
        Create_Icon("9008.11", "Family Details", "../hall/family_det", 3, "", "", false, "-", "payemp", "paymain", 5, Convert.ToDecimal(90.04), true);
        Create_Icon("9008.12", "Nominee Details", "../hall/nomination", 3, "", "", false, "-", "payemp", "paymain", 5, Convert.ToDecimal(90.04), true);
        Create_Icon("9008.13", "Access Card Details", "../hall/card_det", 3, "", "", false, "-", "payemp", "paymain", 5, Convert.ToDecimal(90.04), true);
        #endregion

        Create_Icon("9010", "Salary Processing", "", 2, "fa fa-gears", "", true, "paysp", "paymain", "paymain", 5, 9, true);
        #region salary processing
        Create_Icon("9010.1", "Bank Letter", "", 3, "", "", false, "", "paysp", "paymain", 5, 50, true);
        Create_Icon("9010.2", "Employee Cheque Printing", "", 3, "", "", false, "", "paysp", "paymain", 5, 50, true);
        Create_Icon("9010.3", "Advance/Loan", "", 3, "", "", false, "", "paysp", "paymain", 5, 50, true);
        Create_Icon("9010.4", "Employee Advance Salary", "../hr/empsal_adv", 3, "", "", false, "-", "paysp", "paymain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("9010.5", "OT Entry", "/home/OT_Entry", 3, "", "", false, "-", "paysp", "paymain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("9010.6", "Employee Salary Process", "../hr/emp_salary_process", 3, "", "", false, "-", "paysp", "paymain", 5, Convert.ToDecimal(90.02), true);
        Create_Icon("9010.7", "Contractor Salary Process", "../hr/cont_salary", 3, "-", "-", false, "-", "paysp", "paymain", 5, Convert.ToDecimal(90.02), true);
        Create_Icon("9010.8", "Bonus Calculation", "../hr/bonus_calc", 3, "-", "-", false, "-", "paysp", "paymain", 5, Convert.ToDecimal(90.02), true);
        Create_Icon("9010.9", "Bulk Full & Final", "../hr/full_final_bulk", 3, "-", "-", false, "-", "paysp", "paymain", 5, Convert.ToDecimal(90.02), true);
        Create_Icon("9010.10", "Advance Pre-Payment", "../hr/adv_adjust", 3, "-", "-", false, "-", "paysp", "paymain", 5, Convert.ToDecimal(90.02), true);
        #endregion

        Create_Icon("9011", "Reports", "", 2, "fa fa-gears", "", true, "payrpt", "paymain", "paymain", 5, 9, true);
        #region PF
        Create_Icon("9011.1", "PF Report", "../Hr/stat_report", 3, "", "", false, "", "payrpt", "paymain", 5, 50, true);
        Create_Icon("9011.2", "ESI Report", "../Hr/stat_report", 3, "", "", false, "", "payrpt", "paymain", 5, 50, true);
        Create_Icon("9011.3", "PT Report", "../Hr/stat_report", 3, "", "", false, "", "payrpt", "paymain", 5, 50, true);
        Create_Icon("9011.4", "IT Report", "../Hr/stat_report", 3, "", "", false, "", "payrpt", "paymain", 5, 50, true);
        Create_Icon("9011.5", "LWF Report", "../Hr/stat_report", 3, "", "", false, "", "payrpt", "paymain", 5, 50, true);
        Create_Icon("9011.6", "Loan Detail", "../Hr/stat_report", 3, "", "", false, "", "payrpt", "paymain", 5, 50, true);
        Create_Icon("9011.7", "OT Report", "../Hr/stat_report", 3, "", "", false, "", "payrpt", "paymain", 5, 50, true);
        Create_Icon("9011.8", "Salary Report", "../Hr/stat_report", 3, "", "", false, "", "payrpt", "paymain", 5, 50, true);
        Create_Icon("9011.9", "Check Duplicate", "../Hr/stat_report", 3, "", "", false, "", "payrpt", "paymain", 5, 50, true);
        Create_Icon("9011.10", "Employee Strength", "../Hr/emp_rpt", 3, "", "", false, "", "payrpt", "paymain", 5, 50, true);
        Create_Icon("9011.11", "Variance Analysis", "../Hr/stat_report", 3, "", "", false, "", "payrpt", "paymain", 5, 50, true);
        Create_Icon("9011.12", "Employee", "../Hr/stat_report", 3, "", "", false, "", "payrpt", "paymain", 5, 50, true);
        Create_Icon("9011.13", "Gratuity Report", "../Hr/stat_report", 3, "", "", false, "", "payrpt", "paymain", 5, 50, true);
        Create_Icon("9011.14", "Salary Analysis", "../Hr/var_rpt", 3, "", "", false, "", "payrpt", "paymain", 5, 50, true);
        #endregion

        Create_Icon("9012", "Retirement Benefits", "", 2, "fa fa-gears", "", true, "rebe", "paymain", "paymain", 5, 9, true);
        #region PF
        Create_Icon("9012.1", "Gratuity", "../Hr/stat_rpt", 3, "", "", false, "", "rebe", "paymain", 5, 50, true);
        Create_Icon("9012.2", "Leave Encashment", "../Hr/stat_rpt", 3, "", "", false, "", "rebe", "paymain", 5, 50, true);       
        #endregion
        #endregion

        //main module------------------------------------
        Create_Icon("10000", "Quality Assurance", "../../../../erp/dashboard", 1, "", "", true, "qa", "qamain", "qamain", 5, 9, true, "Define the Quality parameter for new assignment", "Taking the declaration regarding quality assurance");
        // child menu-------------------------------------  
        #region Quality Assurance
        Create_Icon("10001", "QA Master", "", 2, "fa fa-star", "nav child_menu", true, "qamst", "qa", "qamain", 5, 110, true);
        Create_Icon("10001.1", "Testing Parameter", "../Mst/master_ctrl", 3, "", "", false, "-", "qamst", "qamain", 5, 110, true);
        Create_Icon("10001.2", "Testing Method", "../Mst/master_ctrl", 3, "", "", false, "-", "qamst", "qamain", 5, 110, true);
        Create_Icon("10001.3", "Test Template", "../Production/t_temp", 3, "", "", false, "-", "qamst", "qamain", 5, 110, true);
        #endregion

        //main module------------------------------------
        Create_Icon("11000", "Task Management", "../../../../erp/dashboard", 1, "", "", true, "tsk", "tskmain", "tskmain", 5, 9, true, "Create new task for the team", "View and discussion of the task with the team");
        // child menu-------------------------------------  
        #region task management
        // child menu-------------------------------------  
        Create_Icon("11001", "Assign Task", "", 2, "fa fa-user", "nav child_menu", true, "asstsk", "tsk", "tskmain", 5, 110, true);

        // sub child menu-------------------------------------       

        //Create_Icon("11001.1", "Assign Task", "../../../../../erp/corp_admin/taskassign", 3, "", "", false, "", "asstsk", "tskmain", 5, Convert.ToDecimal(110.01), true);
        Create_Icon("11001.1", "Assign Task", "../Hr/task_assign", 3, "", "", false, "", "asstsk", "tskmain", 5, Convert.ToDecimal(110.01), true);
        Create_Icon("11001.2", "View Task", "../Hr/taskinbox", 3, "", "", false, "", "asstsk", "tskmain", 5, Convert.ToDecimal(110.02), true);
        Create_Icon("11001.3", "Sent Task", "../Hr/taskinbox", 3, "", "", false, "", "asstsk", "tskmain", 5, Convert.ToDecimal(110.03), true);

        Create_Icon("11002", "Task Config", "../../../../../erp/corp_admin/taskconfig", 2, "fa fa-user", "", false, "", "tsk", "tskmain", 5, 111, true);
        Create_Icon("11003", "Task Report", "../../../../../erp/corp_admin/taskreport", 2, "fa fa-user", "", false, "", "tsk", "tskmain", 5, 111, true);

        Create_Icon("11004", "CheckList Mgmt", "", 2, "fa fa-user", "nav child_menu", true, "chkl", "tskmain", "tskmain", 5, 112, true);
        Create_Icon("11004.1", "Nature of Activity", "../Mst/master_ctrl", 3, "", "", false, "", "chkl", "tskmain", 5, Convert.ToDecimal(112.01), true);
        Create_Icon("11004.2", "Checklist Master", "../Hr/chk_temp", 3, "", "", false, "", "chkl", "tskmain", 5, Convert.ToDecimal(112.02), true);

        Create_Icon("11005", "Task Mgmt Master", "", 2, "fa fa-gears", "nav child_menu", true, "tskmst", "tskmain", "tskmain", 5, 9, true);
        Create_Icon("11005.1", "Task Type", "../Mst/master_ctrl", 3, "", "", false, "-", "tskmst", "tskmain", 5, 110, true);
        Create_Icon("11005.2", "Closer Approval", "../Mst/master_ctrl", 3, "", "", false, "-", "tskmst", "tskmain", 5, 110, true);
        #endregion

        //main module------------------------------------
        Create_Icon("12000", "CRM", "../../../../erp/dashboard", 1, "", "", true, "crm", "crmmain", "crmmain", 5, 9, true, "Open the door for new entrants", "Manage the customer relation with pro-active approach");
        // child menu-------------------------------------  
        #region
        // child menu-------------------------------------    
        //Create_Icon("12001", "Customer Master", "", 2, "fa fa-star", "nav child_menu", true, "cmrpro", "crm", "crmmain", 5, 110, true);
        //Create_Icon("12001.1", "Star Customers", "../Vastu/new_cust", 3, "", "", false, "", "cmrpro", "crmmain", 5, 110, true);
        //Create_Icon("12001.2", "Conversation", "../../../../../erp/corp_admin/conversation", 3, "", "", false, "", "cmrpro", "crmmain", 5, 110, true);

        //Create_Icon("12002", "Existing Customers", "", 2, "fa fa-info", "nav child_menu", true, "ccfrm", "crm", "crmmain", 5, 110, true);
        //Create_Icon("12002.1", "Existing Customers", "../../../../erp/crm/cust_prop", 3, "", "", false, "", "ccfrm", "crmmain", 5, 110, true);
        //Create_Icon("12002.2", "Discussion", "../../../../erp/crm/cust_prop", 3, "", "", false, "", "ccfrm", "crmmain", 5, 110, true);
        //Create_Icon("12002.3", "Customer Visit", "../../../../erp/crm/cust_prop", 3, "", "", false, "", "ccfrm", "crmmain", 5, 110, true);
        //Create_Icon("12002.4", "Daily Report", "../../../../erp/crm/cust_prop", 3, "", "", false, "", "ccfrm", "crmmain", 5, 110, true);
        //Create_Icon("12002.5", "Admin Report", "../../../../erp/crm/cust_prop", 3, "", "", false, "", "ccfrm", "crmmain", 5, 110, true);
        //Create_Icon("12002.6", "Previous Report", "../../../../erp/crm/cust_prop", 3, "", "", false, "", "ccfrm", "crmmain", 5, 110, true);

        //Create_Icon("12003", "CRM Master", "", 2, "fa fa-arrows-alt", "nav child_menu", true, "crmmst", "crm", "crmmain", 5, 110, true);
        //Create_Icon("12003.1", "Customer Type", "../Mst/master_ctrl", 3, "", "", false, "-", "crmmst", "crmmain", 5, 110, true);
        //Create_Icon("12003.2", "Next Action Master", "../Mst/master_ctrl", 3, "", "", false, "-", "crmmst", "crmmain", 5, 110, true);
        //Create_Icon("12003.3", "Product Master", "../Mst/master_ctrl", 3, "", "", false, "-", "crmmst", "crmmain", 5, 110, true);
        //Create_Icon("12003.4", "Deal Probability Master", "../Mst/master_ctrl", 3, "", "", false, "-", "crmmst", "crmmain", 5, 110, true);
        //Create_Icon("12003.5", "Lead Source Master", "../Mst/master_ctrl", 3, "", "", false, "-", "crmmst", "crmmain", 5, 110, true);
        //Create_Icon("12003.6", "Lead Status Master", "../Mst/master_ctrl", 3, "", "", false, "-", "crmmst", "crmmain", 5, 110, true);
        //Create_Icon("12003.7", "Sales/Service Area", "../Mst/master_ctrl", 3, "", "", false, "", "crmmst", "crmmain", 5, 50, true);
        //Create_Icon("12003.8", "Lead Closer Time", "../Mst/master_ctrl", 3, "", "", false, "", "crmmst", "crmmain", 5, 50, true);




        // sub child menu-------------------------------------  
        #endregion

        //main module------------------------------------
        Create_Icon("13000", "Parent Portal", "../../../../erp/dashboard", 1, "", "", true, "stp", "stpmain", "stpmain", 5, 9, true, "View of kids performance", "Parent login to access the performance of the kids");

        #region
        // child menu-------------------------------------        
        Create_Icon("999999", "Logout", "", 2, "fa fa-power-off", "", false, "", "stp", "stpmain", 5, Convert.ToDecimal(99999.00), true, "onclick=$menuclick(this);$");
        //common child menu-------------------------------------
        Create_Icon("1004", "My Task", "", 2, "fa fa-thumb-tack", "nav child_menu", true, "cal", "stp", "stpmain", 5, 13, true);        //common submenu-------------------------------
        Create_Icon("1004.1", "Calender", "../../../../../erp/Event_Calender", 3, "", "", false, "", "cal", "stpmain", 5, Convert.ToDecimal(13.01), true);
        Create_Icon("1004.2", "To Do List", "../../../../../erp/list_of_todotask", 3, "", "", false, "", "cal", "stpmain", 5, Convert.ToDecimal(13.02), true);
        Create_Icon("13000.2", "Attendance", "", 2, "fa fa-thumb-tack", "nav child_menu", false, "", "stpmain", "stpmain", 5, 13, true);

        Create_Icon("13000.3", "Holidays", "../../../../../erp/school_admin/holidays_report", 2, "fa fa-thumb-tack", "", false, "", "stp", "stpmain", 5, 13, true);
        //Create_Icon("13000.4", "School Directory", "../../../../../erp/school_admin/school_directory", 2, "fa fa-thumb-tack", "", false, "", "stpmain", "stpmain", 5, 13, true);
        Create_Icon("13000.4", "School Directory", "../Education/sch_directory", 2, "fa fa-thumb-tack", "", false, "", "stpmain", "stpmain", 5, 13, true);
        Create_Icon("13000.5", "Transport Detail", "../../../../../erp/school_admin/transport_report", 2, "fa fa-thumb-tack", "", false, "", "stpmain", "stpmain", 5, 13, true);
        Create_Icon("13000.6", "Photo Galary", "../../../../../erp/school_admin/photo_gallery", 2, "fa fa-thumb-tack", "", false, "", "stpmain", "stpmain", 5, 13, true);
        Create_Icon("13000.7", "Student Profile", "../../../../../erp/school_admin/std_profile", 2, "fa fa-thumb-tack", "", false, "", "stpmain", "stpmain", 5, 13, true);
        //  Create_Icon("13000.8", "Fees", "../../../../../erp/SchoolReports_Forms/Parent_fee_Report", 2, "fa fa-thumb-tack", "", false, "", "stpmain", "stpmain", 5, 13, true);
        Create_Icon("13000.8", "Fees", "../Education/fee_receipt", 2, "fa fa-thumb-tack", "", false, "", "stpmain", "stpmain", 5, 13, true);
        Create_Icon("13000.9", "Conversation", "", 2, "fa fa-thumb-tack", "nav child_menu", false, "", "stpmain", "stpmain", 5, 2, true);
        Create_Icon("13000.10", "Results", "", 2, "fa fa-thumb-tack", "nav child_menu", false, "", "stpmain", "stpmain", 5, 13, true);
        // Create_Icon("13000.11", "Leave Application", "../../../../../erp/school_admin/leave_apply", 2, "fa fa-thumb-tack", "", false, "", "stpmain", "stpmain", 5, 13, true);
        Create_Icon("13000.11", "Leave Application", "../Education/leaveapplication", 2, "fa fa-thumb-tack", "", false, "", "stpmain", "stpmain", 5, 13, true);
        //Create_Icon("13000.12", "TimeTable Report", "../../../../../erp/school_admin/timetable_report", 2, "fa fa-thumb-tack", "", false, "", "stp", "stpmain", 5, 13, true);
        Create_Icon("13000.12", "TimeTable Report", "../../../../../erp/SchoolReports_Forms/student_time_table", 2, "fa fa-thumb-tack", "", false, "", "stp", "stpmain", 5, 13, true);
        Create_Icon("13000.13", "Home Assignment", "../../../../../erp/SchoolReports_Forms/Home_Work_Rpt", 2, "fa fa-thumb-tack", "", false, "", "stpmain", "stpmain", 5, 13, true);
        #endregion        

        //main module------------------------------------
        Create_Icon("14000", "Student Portal", "../../../../erp/dashboard", 1, "", "", true, "std", "stdmain", "stdmain", 5, 9, true, "View of Home Work and others", "Access the study knowledge and class discussion");
        //// child menu-------------------------------------  
        #region
        Create_Icon("999999", "Logout", "", 2, "fa fa-power-off", "", false, "", "std", "stdmain", 5, Convert.ToDecimal(99999.00), true, "onclick=$menuclick(this);$");
        //common child menu-------------------------------------
        Create_Icon("1004", "My Task", "", 2, "fa fa-thumb-tack", "nav child_menu", true, "cal", "std", "stdmain", 5, 13, true);        //common submenu-------------------------------

        Create_Icon("1004.1", "Calender", "../../../../../erp/Event_Calender", 3, "", "", false, "", "cal", "stdmain", 5, Convert.ToDecimal(13.01), true);
        Create_Icon("1004.2", "To Do List", "../../../../../erp/list_of_todotask", 3, "", "", false, "", "cal", "stdmain", 5, Convert.ToDecimal(13.02), true);

        Create_Icon("13000.2", "Attendance", "", 2, "fa fa-thumb-tack", "nav child_menu", false, "", "stdmain", "stdmain", 5, 13, true);
        Create_Icon("13000.3", "Holidays", "../../../../../erp/school_admin/holidays_report", 2, "fa fa-thumb-tack", "nav child_menu", false, "", "stdmain", "stdmain", 5, 13, true);
        // Create_Icon("13000.4", "School Directory", "../../../../../erp/school_admin/school_directory", 2, "fa fa-thumb-tack", "", false, "", "stdmain", "stdmain", 5, 13, true);
        Create_Icon("13000.4", "School Directory", "../Education/sch_directory", 2, "fa fa-thumb-tack", "", false, "", "stdmain", "stdmain", 5, 13, true);
        Create_Icon("13000.5", "Transport Detail", "", 2, "fa fa-thumb-tack", "nav child_menu", false, "", "stdmain", "stdmain", 5, 13, true);
        Create_Icon("13000.6", "Photo Galary", "", 2, "fa fa-thumb-tack", "nav child_menu", false, "", "stdmain", "stdmain", 5, 13, true);
        Create_Icon("13000.7", "Student Profile", "../../../erp/student/student_profile", 2, "fa fa-thumb-tack", "nav child_menu", false, "", "stdmain", "stdmain", 5, 13, true);
        Create_Icon("13000.8", "Fees", "../Education/fee_receipt", 2, "fa fa-thumb-tack", "nav child_menu", false, "", "stdmain", "stdmain", 5, 13, true);
        Create_Icon("13000.10", "Results", "", 2, "fa fa-thumb-tack", "nav child_menu", false, "", "stdmain", "stdmain", 5, 13, true);
        Create_Icon("13000.12", "TimeTable Report", "../../../../../erp/SchoolReports_Forms/student_time_table", 2, "fa fa-thumb-tack", "", false, "", "stdmain", "stdmain", 5, 13, true);
        Create_Icon("13000.13", "Home Assignment", "../../../../../erp/SchoolReports_Forms/Home_Work_Rpt", 2, "fa fa-thumb-tack", "", false, "", "stdmain", "stdmain", 5, 13, true);
        #endregion

        //main module------------------------------------
        Create_Icon("15000", "Question Paper Designer", "../../../../erp/dashboard", 1, "", "", true, "qpd", "qpdmain", "qpdmain", 5, 9, true, "Create question and link with question paper", "Design Question and paper as per your requirement");
        //// child menu-------------------------------------  
        #region
        Create_Icon("15000.1", "Question Master", "", 2, "fa fa-sign-in", "nav child_menu", true, "qpmst", "qpmst", "qpdmain", 5, Convert.ToDecimal(73.20), true);
        Create_Icon("15000.1.1", "Question Type", "../../../../erp/quespaper/ques_master", 3, "", "", false, "-", "qpmst", "qpdmain", 5, Convert.ToDecimal(73.21), true);
        Create_Icon("15000.1.2", "Title Master", "../../../../erp/quespaper/create_topic", 3, "", "", false, "-", "qpmst", "qpdmain", 5, Convert.ToDecimal(73.22), true);

        Create_Icon("15000.2", "Question Paper", "", 2, "fa fa-sign-in", "nav child_menu", true, "qpques", "qpques", "qpdmain", 5, 86, true);
        Create_Icon("15000.2.1", "Create Question", "../../../../erp/school_admin/quespaper/create_ques", 3, "", "", false, "-", "qpques", "qpdmain", 5, Convert.ToDecimal(86.01), true);
        Create_Icon("15000.2.2", "Create Question Paper", "../../../../erp/school_admin/quespaper/create_quespaper", 3, "", "", false, "-", "qpques", "qpdmain", 5, Convert.ToDecimal(86.02), true);
        #endregion

        Create_Icon("16000", "Library", "../../../../erp/dashboard", 1, "", "", true, "", "lbrmain", "lbrmain", 5, 16, true, "Easy management over books and Fee", "Control over issuance and closing inventory of books along with fee management");
        //library
        #region
        Create_Icon("16000.1", "Library Master", "", 2, "fa fa-thumb-tack", "nav child_menu", true, "lbrmas", "lbrmain", "lbrmain", 5, 13, true);
        //Create_Icon("16000.1.1", "Buliding Master", "../Mst/master_ctrl", 3, "", "", false, "", "lbrmas", "lbrmain", 5, 110, true);
        //Create_Icon("16000.1.2", "Library Rack", "../../../../erp/library/library_master", 3, "", "", false, "", "lbrmas", "lbrmain", 5, 110, true);
        //Create_Icon("16000.1.3", "Library Bin", "../../../../erp/library/library_master", 3, "", "", false, "", "lbrmas", "lbrmain", 5, 110, true);
        Create_Icon("16000.1.4", "Book Category", "../Mst/master_ctrl", 3, "", "", false, "", "lbrmas", "lbrmain", 5, 110, true);
        Create_Icon("16000.1.5", "Book Against Card", "../Mst/master_ctrl", 3, "", "", false, "", "lbrmas", "lbrmain", 5, 110, true);
        Create_Icon("16000.1.7", "Membership Fees", "../lib/mem_fee", 3, "", "", false, "", "lbrmas", "lbrmain", 5, 110, true);
        Create_Icon("16000.1.8", "Doc No. Pattern", "../Education/pattern_gen", 3, "", "", false, "", "lbrmas", "lbrmain", 5, 110, true);
        //Create_Icon("16000.1.9", "Floor Name", "../Mst/cascade_mst", 3, "", "", false, "", "lbrmas", "lbrmain", 5, 110, true);
        //Create_Icon("16000.1.10", "Room Name", "../Mst/cascade_mst", 3, "", "", false, "", "lbrmas", "lbrmain", 5, 110, true);
        Create_Icon("17001.23", "Single Location", "../Mst/master_ctrl", 3, "", "", false, "", "lbrmas", "lbrmain", 5, 50, true);
        Create_Icon("2003.4", "Frequency", "../Mst/master_ctrl", 3, "", "", false, "", "lbrmas", "lbrmain", 5, Convert.ToDecimal(47.01), true);
        Create_Icon("9003.3.2", "Language Master", "../Mst/master_ctrl", 4, "", "", false, "", "lbrmas", "lbrmain", 5, 50, true);
        Create_Icon("16000.1.8", "Bulk Upload", "../vastu/bulk_acc", 3, "", "", false, "-", "lbrmas", "lbrmain", 5, Convert.ToDecimal(88.01), true);

        Create_Icon("16000.2", "Book Detail", "", 2, "fa fa-thumb-tack", "nav child_menu", true, "lbrbdt", "lbrmain", "lbrmain", 5, 13, true);
        Create_Icon("16000.2.1", "Add Book", "../../../../../erp/library/addbook", 3, "", "", false, "", "lbrbdt", "lbrmain", 5, 13, true);
        Create_Icon("16000.2.2", "Add Publisher", "../Purchase/party", 3, "", "", false, "", "lbrbdt", "lbrmain", 5, 13, true);
        Create_Icon("16000.2.3", "Add Journal", "../lib/add_jurnl", 3, "", "", false, "", "lbrbdt", "lbrmain", 5, 13, true);

        Create_Icon("16000.3", "Issue & Return", "", 2, "fa fa-thumb-tack", "nav child_menu", true, "lbri&r", "lbrmain", "lbrmain", 5, 13, true);
        Create_Icon("16000.3.1", "Book Issue", "../Lib/book_issued", 3, "", "", false, "", "lbri&r", "lbrmain", 5, 13, true);
        Create_Icon("16000.3.2", "Book Return", "../Lib/book_return", 3, "", "", false, "", "lbri&r", "lbrmain", 5, 13, true);

        Create_Icon("16000.4", "Book Availability", "../../../../../erp/library/reserve_book", 2, "fa fa-thumb-tack", "", false, "", "lbrmain", "lbrmain", 5, 13, true);
        Create_Icon("16000.5", "Membership Fees", "", 2, "fa fa-thumb-tack", "nav child_menu", true, "lbr", "lbrmain", "lbrmain", 5, 13, true);
        Create_Icon("16000.5.1", "Security Deposit", "../../../../erp/library/library_membership", 3, "", "", false, "-", "lbr", "lbrmain", 5, Convert.ToDecimal(73.21), true);
        Create_Icon("16000.5.2", "Library Fees", "../Education/fee_receipt", 3, "", "", false, "-", "lbr", "lbrmain", 5, Convert.ToDecimal(73.21), true);
        Create_Icon("16000.5.3", "Library Fees Setting", "../../../../erp/school_academic_staff/Lib_Fees_Setting", 3, "", "", false, "-", "lbr", "lbrmain", 5, Convert.ToDecimal(73.21), true);
        #endregion

        //main
        Create_Icon("17000", "Inventory", "../../../../erp/dashboard", 1, "", "", true, "", "invtmain", "invtmain", 5, 16, true, "Easy Inventory Control", "Control over issuance and closing inventory");
        #region
        Create_Icon("17001", "Inventory Master", "", 2, "fa fa-gears", "nav child_menu", true, "invm", "invtmain", "invtmain", 5, 9, true);
        Create_Icon("17001.1", "Unit Measurement", "../Mst/master_ctrl", 3, "", "", false, "", "invm", "invtmain", 5, 50, true);
        Create_Icon("17001.2", "HSN/SAC", "../Billing/billing_master", 3, "", "", false, "", "invm", "invtmain", 5, 50, true);
        Create_Icon("17001.3", "Item Master", "../Billing/itemservice", 3, "", "", false, "", "invm", "invtmain", 5, 50, true);
        //Create_Icon("17001.4", "Item Issued", "../Inventory/item_issued", 3, "", "", false, "", "invm", "invtmain", 5, 50, true);       
        Create_Icon("17001.5", "Item Group", "../Mst/doc_master", 3, "", "", false, "", "invm", "invtmain", 5, 50, true);
        //Create_Icon("17001.6", "Item Category", "../Mst/master_ctrl", 3, "", "", false, "", "invm", "invtmain", 5, 50, true);       
        //Create_Icon("17001.6", "Item Location", "../Mst/master_ctrl", 3, "", "", false, "", "invm", "invtmain", 5, 50, true); // replacment of item category      
        Create_Icon("17001.7", "MRN Type Master", "../Mst/doc_master", 3, "", "", false, "", "invm", "invtmain", 5, 50, true);
        Create_Icon("17001.8", "Item Sub Group", "../Billing/itemsubgrp", 3, "", "", false, "", "invm", "invtmain", 5, 50, true);
        Create_Icon("17001.9", "Item Issued Type", "../Mst/doc_master", 3, "", "", false, "", "invm", "invtmain", 5, 50, true);
        Create_Icon("17001.10", "Attribute Master", "../Mst/master_ctrl", 3, "", "", false, "", "invm", "invtmain", 5, 50, true);
        Create_Icon("17001.11", "Doc No. Pattern", "../Education/pattern_gen", 3, "", "", false, "", "invm", "invtmain", 5, 50, true);
        Create_Icon("17001.12", "Store Return Type", "../Mst/master_ctrl", 3, "", "", false, "", "invm", "invtmain", 5, 50, true);        
        Create_Icon("17001.14", "Packing Type", "../Mst/master_ctrl", 3, "", "", false, "", "invm", "invtmain", 5, 50, true);

        Create_Icon("17001.15.1", "Multi Location", "", 3, "", "nav child_menu", true, "mloc", "invm", "invtmain", 5, 50, true);
        //mloc
        Create_Icon("17001.15", "Building Name", "../Mst/master_ctrl", 4, "", "", false, "", "mloc", "invtmain", 5, 50, true);
        Create_Icon("17001.16", "Floor Name", "../Mst/cascade_mst", 4, "", "", false, "", "mloc", "invtmain", 5, 50, true);
        Create_Icon("17001.17", "Room Name", "../Mst/cascade_mst", 4, "", "", false, "", "mloc", "invtmain", 5, 50, true);
        Create_Icon("17001.18", "Rack Name", "../Mst/cascade_mst", 4, "", "", false, "", "mloc", "invtmain", 5, 50, true);
        Create_Icon("17001.19", "Bin Name", "../Mst/cascade_mst", 4, "", "", false, "", "mloc", "invtmain", 5, 50, true);
        //mloc

        //Create_Icon("17001.20", "Material Requisition", "../Inventory/mat_req", 3, "", "", false, "", "invm", "invtmain", 5, 50, true);       
        Create_Icon("17001.21", "Physical/Opening", "../Inventory/phy_verify", 3, "", "", false, "", "invm", "invtmain", 5, 50, true);
        Create_Icon("17001.22", "Storage Condition", "../Mst/master_ctrl", 3, "", "", false, "", "invm", "invtmain", 5, 50, true);
        Create_Icon("17001.23", "Single Location", "../Mst/master_ctrl", 3, "", "", false, "", "invm", "invtmain", 5, 50, true);
        Create_Icon("17001.24", "Bulk Item Upload", "../Billing/item_upd", 3, "", "", false, "", "invm", "invtmain", 5, 50, true);
        Create_Icon("17001.25", "Form Control", "../Mst/form_ctrl", 3, "", "", false, "", "invm", "invtmain", 5, 50, true);
        Create_Icon("17001.26", "Other Charges", "../Mst/master_ctrl", 3, "", "", false, "", "invm", "invtmain", 5, 50, true);
        // Create_Icon("17001.28", "Doc Template", "../Education/rpt_temp", 3, "", "", false, "", "invm", "invtmain", 5, Convert.ToDecimal(85.02), true);
        Create_Icon("17001.28", "Doc Print Template", "../Mst/prn_ctrl", 3, "", "", false, "", "invm", "invtmain", 5, Convert.ToDecimal(85.02), true);
        Create_Icon("28004.11", "Payment Term Master", "../Mst/master_ctrl", 3, "", "", false, "-", "invm", "invtmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("17001.29", "SMS/Mail Config", "../Inventory/sms_mail_tmp", 3, "", "", false, "", "invm", "invtmain", 5, 50, true);
        Create_Icon("17001.30", "Approval Config", "../Inventory/apprv_config", 3, "", "", false, "", "invm", "invtmain", 5, 50, true);
        Create_Icon("17001.31", "Item Opening", "../Inventory/item_opening", 3, "", "", false, "", "invm", "invtmain", 5, 50, true);
        Create_Icon("17001.32", "Physical New", "../Inventory/phy_verifyn", 3, "", "", false, "", "invm", "invtmain", 5, 50, true);
        Create_Icon("17001.33", "Item Opening New", "../Inventory/item_openingn", 3, "", "", false, "", "invm", "invtmain", 5, 50, true);

        Create_Icon("17002", "Inward", "", 2, "fa fa-sign-in", "nav child_menu", true, "inwd", "invtmain", "invtmain", 5, 9, true);
        Create_Icon("17002.1", "Create MRN", "../Inventory/quality", 3, "", "", false, "", "inwd", "invtmain", 5, 50, true);
        Create_Icon("17002.2", "Quality Confirmation", "../Inventory/mrn", 3, "", "", false, "", "inwd", "invtmain", 5, 50, true);
        Create_Icon("17002.3", "Daily Prod Entry", "../Inventory/dpr", 3, "-", "-", false, "-", "inwd", "invtmain", 5, 50, true);
        Create_Icon("17002.4", "Gate In", "../Inventory/g_in", 3, "-", "-", false, "-", "inwd", "invtmain", 5, 50, true);
        Create_Icon("17002.5", "Pending Gate In", "../Inventory/p_g_in", 3, "-", "-", false, "-", "inwd", "invtmain", 5, 50, true);
        Create_Icon("17002.6", "Close Pending MRN", "../Inventory/p_close", 3, "-", "-", false, "-", "inwd", "invtmain", 5, 50, true);
        Create_Icon("17002.7", "Gate In Item", "../Inventory/g_in_item", 3, "-", "-", false, "-", "inwd", "invtmain", 5, 50, true);
        Create_Icon("17002.8", "MRN New", "../Inventory/mrnn", 3, "", "", false, "", "inwd", "invtmain", 5, 50, true);
        Create_Icon("17001.13", "Store Return", "../Inventory/store_return", 3, "", "", false, "", "inwd", "invtmain", 5, 50, true);
        Create_Icon("17002.9", "Store Return New", "../Inventory/store_returnn", 3, "", "", false, "", "inwd", "invtmain", 5, 50, true);
        Create_Icon("17002.10", "Quality Confirmation New", "../Inventory/qualityn", 3, "", "", false, "", "inwd", "invtmain", 5, 50, true);

        Create_Icon("17003", "Outward", "", 2, "fa fa-sign-out", "nav child_menu", true, "outwd", "invtmain", "invtmain", 5, 9, true);
        Create_Icon("17003.1", "RGP Challan", "../Inventory/rgp_challan", 3, "", "", false, "", "outwd", "invtmain", 5, 50, true);
        Create_Icon("17003.2", "NRGP Challan", "../Inventory/nrgp_challan", 3, "", "", false, "", "outwd", "invtmain", 5, 50, true);
        Create_Icon("17003.3", "Material Issue", "../Inventory/item_issued", 3, "", "", false, "", "outwd", "invtmain", 5, 50, true);
        Create_Icon("17003.4", "Dispatch", "../Inventory/dispatch", 3, "", "", false, "", "outwd", "invtmain", 5, 50, true);
        Create_Icon("17003.5", "Gate Out", "../Inventory/g_out", 3, "", "", false, "", "outwd", "invtmain", 5, 50, true);
        Create_Icon("17003.6", "Pending Gate Out", "../Inventory/p_g_out", 3, "", "", false, "", "outwd", "invtmain", 5, 50, true);
        Create_Icon("17003.7", "Material Requisition", "../Inventory/mat_req", 3, "", "", false, "", "outwd", "invtmain", 5, 50, true);
        Create_Icon("17003.8", "Close Pending MRS", "../Inventory/p_close", 3, "", "", false, "", "outwd", "invtmain", 5, 50, true);
        Create_Icon("17003.9", "Close Pending Dispatch", "../Inventory/p_close", 3, "", "", false, "", "outwd", "invtmain", 5, 50, true);
        Create_Icon("17003.10", "Material Issue New", "../Inventory/item_issuedn", 3, "", "", false, "", "outwd", "invtmain", 5, 50, true);

        Create_Icon("17004", "Indent", "", 2, "fa fa-bank", "nav child_menu", true, "ind", "invtmain", "invtmain", 5, 88, true);
        Create_Icon("17004.1", "Create Indent", "../Purchase/indent_req", 3, "", "", false, "-", "ind", "invtmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("17004.2", "Close Pending Indent", "../Inventory/p_close", 3, "", "", false, "-", "ind", "invtmain", 5, Convert.ToDecimal(88.01), true);

        Create_Icon("17005", "Reports", "", 2, "fa fa-print", "nav child_menu", true, "invreps", "invtmain", "invtmain", 5, 88, true);
        //Create_Icon("17005.1", "Stock Reports", "../Inventory/invreps", 3, "", "", false, "-", "invreps", "invtmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("17005.1", "Stock Reports", "../Inventory/invreps2", 3, "", "", false, "-", "invreps", "invtmain", 5, Convert.ToDecimal(88.01), true);



        #endregion

        Create_Icon("18000", "School Admin", "../../../../erp/dashboard", 1, "", "", true, "", "schadmmain", "schadmmain", 5, 18, true, "Admin control Panel", "Define master and control setting");
        //school admin
        #region
        Create_Icon("18000.1", "User Login List", "", 2, "fa fa-thumb-tack", "", false, "", "schadmmain", "schadmmain", 5, 13, true, "onclick=$menuclick(this);$");

        Create_Icon("24001", "Knowledge Bank", "", 2, "fa fa-bank", "nav child_menu", true, "kbank", "main", "schadmmain", 5, 88, true);
        Create_Icon("24001.1", "Academic Material", "../../../../erp/quespaper/upload_ac_material", 3, "", "", false, "-", "kbank", "schadmmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("24001.2", "Exam Material", "../../../../erp/quespaper/upload_papers", 3, "", "", false, "-", "kbank", "schadmmain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("24001.3", "Phote Gallary", "../../../../erp/quespaper/upload_galary", 3, "", "", false, "-", "kbank", "schadmmain", 5, Convert.ToDecimal(88.03), true);
        #endregion

        Create_Icon("19000", "Performance", "../../../../erp/dashboard", 1, "", "", true, "", "schpermain", "schpermain", 5, 19, true, "Performance of the team", "View and discussion of the performance of the team");
        //performance
        #region
        // Create_Icon("19000.1", "Daily Work", "../../../../../erp/performance/daily_work", 2, "fa fa-thumb-tack", "", false, "", "schpermain", "schpermain", 5, 13, true);
        Create_Icon("19000.1", "Daily Work", "../hr/daily_work", 2, "fa fa-thumb-tack", "", false, "", "schpermain", "schpermain", 5, 13, true);
        Create_Icon("19000.2", "Work Report", "", 2, "fa fa-thumb-tack", "", false, "", "schpermain", "schpermain", 5, 13, true, "onclick=$menuclick(this);$");
        //Create_Icon("19000.3", "Create Team", "../../../../../erp/performance/create_team", 2, "fa fa-thumb-tack", "", false, "", "schpermain", "schpermain", 5, 13, true);
        Create_Icon("19000.3", "Create Team", "../hr/create_team", 2, "fa fa-thumb-tack", "", false, "", "schpermain", "schpermain", 5, 13, true);

        Create_Icon("19000.4", "Performance Master", "", 2, "fa fa-bank", "nav child_menu", true, "perf", "schpermain", "schpermain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("190004.1", "Nature Of Work", "../Mst/master_ctrl", 3, "", "", false, "-", "perf", "schpermain", 5, 13, true);
        Create_Icon("190004.2", "Daily Work Config", "../Mst/form_ctrl", 3, "", "", false, "-", "perf", "schpermain", 5, 13, true);
        #endregion

        Create_Icon("20000", "EXIM", "../../../../erp/dashboard", 1, "", "", true, "", "eximmain", "eximmain", 5, 20, true, "Export Import Process & Benefits", "Control over export import document & incentive");
        #region
        Create_Icon("20001", "Export", "../Purchase/party", 2, "fa fa-bank", "nav child_menu", true, "exm", "eximmain", "eximmain", 5, 88, true);
        Create_Icon("20001.1", "Export Party", "../Purchase/party", 3, "", "", false, "-", "exm", "eximmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("20001.2", "Export Party Assigned", "../Exim/party_ass", 3, "", "", false, "-", "exm", "eximmain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("20001.3", "Export Work Order", "", 3, "", "", false, "-", "exm", "eximmain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("20001.4", "Export Tax Invoice", "../Billing/invoice", 3, "", "", false, "-", "exm", "eximmain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("20001.5", "Export Comm. Invoice", "", 3, "", "", false, "-", "exm", "eximmain", 5, Convert.ToDecimal(88.02), true);

        Create_Icon("20002", "Import", "../Purchase/party", 2, "fa fa-bank", "nav child_menu", true, "impm", "eximmain", "eximmain", 5, 88, true);
        Create_Icon("20002.1", "Import Party", "", 3, "", "", false, "-", "impm", "eximmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("20002.2", "Import Party Assigned", "../Exim/party_ass", 3, "", "", false, "-", "impm", "eximmain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("20002.3", "Import PO/PI", "", 3, "", "", false, "-", "impm", "eximmain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("20002.4", "Import Invoice", "", 3, "", "", false, "-", "impm", "eximmain", 5, Convert.ToDecimal(88.02), true);

        Create_Icon("20003", "Export Documentation", "", 2, "fa fa-bank", "nav child_menu", true, "expd", "eximmain", "eximmain", 5, 88, true);
        Create_Icon("20003.1", "Performa Invoice", "", 3, "", "", false, "-", "expd", "eximmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("20003.2", "Packing List", "", 3, "", "", false, "-", "expd", "eximmain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("20003.3", "Certificate", "", 3, "", "", false, "-", "expd", "eximmain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("20003.4", "Bill Of Exchange", "", 3, "", "", false, "-", "expd", "eximmain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("20003.5", "Letter To Bank", "", 3, "", "", false, "-", "expd", "eximmain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("20003.6", "Shipping Bill", "../Exim/sb", 3, "", "", false, "-", "expd", "eximmain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("20003.7", "Bill Of Lading", "../Exim/bill_of_lading", 3, "", "", false, "-", "expd", "eximmain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("20003.8", "EPC Annexure", "", 3, "", "", false, "-", "expd", "eximmain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("20003.9", "EPCG Declaration", "", 3, "", "", false, "-", "expd", "eximmain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("20003.10", "Bulk Receipt", "../Exim/bulk_banking", 3, "", "", false, "-", "expd", "eximmain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("20003.11", "Bulk Commercial Invoice", "../Exim/bulk_comm_inv", 3, "", "", false, "-", "expd", "eximmain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("20003.12", "PI With Comm Inv", "../Exim/pi_comm", 3, "", "", false, "-", "expd", "eximmain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("20003.13", "BRC", "../Exim/brc", 3, "", "", false, "-", "expd", "eximmain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("20003.14", "Bulk PI", "../Exim/bulk_pi", 3, "", "", false, "-", "expd", "eximmain", 5, Convert.ToDecimal(88.02), true);

        Create_Icon("20004", "Import Documentation", "", 2, "fa fa-bank", "nav child_menu", true, "exid", "eximmain", "eximmain", 5, 88, true);
        Create_Icon("20004.1", "Bill Of Entry", "../Exim/bill_of_entry", 3, "", "", false, "-", "exid", "eximmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("20004.2", "Letter to Bank", "", 3, "", "", false, "-", "exid", "eximmain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("20004.3", "Insurance Declaration", "", 3, "", "", false, "-", "exid", "eximmain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("20004.4", "Bulk Payment", "../Exim/bulk_banking", 3, "", "", false, "-", "exid", "eximmain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("20004.5", "Bulk Invoice", "../Exim/bulk_comm_inv", 3, "", "", false, "-", "exid", "eximmain", 5, Convert.ToDecimal(88.02), true);

        Create_Icon("20005", "Export Incentive", "", 2, "fa fa-bank", "nav child_menu", true, "exin", "eximmain", "eximmain", 5, 88, true);
        Create_Icon("20005.1", "Advance Authorisation", "", 3, "", "", false, "-", "exin", "eximmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("20005.2", "Duty Drawback", "", 3, "", "", false, "-", "exin", "eximmain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("20005.3", "MEIS Scheme", "", 3, "", "", false, "-", "exin", "eximmain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("20005.4", "Incentive Master", "../Exim/exp_inc_m", 3, "", "", false, "-", "exin", "eximmain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("20005.5", "Incentive Due", "../Exim/due_inc_file", 3, "", "", false, "-", "exin", "eximmain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("20005.6", "Incentive Received", "../Exim/inc_rec", 3, "", "", false, "-", "exin", "eximmain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("20005.7", "Incentive Utilise", "../Exim/inc_use", 3, "", "", false, "-", "exin", "eximmain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("20005.8", "Incentive Type Master", "../Mst/master_ctrl", 3, "", "", false, "-", "exin", "eximmain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("20005.9", "Incentive Apply", "../Exim/inc_app", 3, "", "", false, "-", "exin", "eximmain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("20005.10", "Incentive Write Off", "../Exim/inc_wrt", 3, "", "", false, "-", "exin", "eximmain", 5, Convert.ToDecimal(88.02), true);

        Create_Icon("20006", "Bank Master", "", 2, "fa fa-bank", "nav child_menu", true, "exbm", "eximmain", "eximmain", 5, 88, true);
        Create_Icon("20006.1", "Bank Name", "../Mst/master_ctrl", 3, "", "", false, "-", "exbm", "eximmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("20006.2", "Bank Acct Type", "../Mst/master_ctrl", 3, "", "", false, "-", "exbm", "eximmain", 5, 88, true);
        Create_Icon("20006.3", "Bank Acct Details", "../Account/bank_ac", 3, "", "", false, "-", "exbm", "eximmain", 5, 88, true);
        Create_Icon("20006.4", "Currency Master", "../Mst/master_ctrl", 3, "", "", false, "-", "exbm", "eximmain", 5, 88, true);
        Create_Icon("20006.5", "Currency Rate", "../Account/cur_rate", 3, "", "", false, "-", "exbm", "eximmain", 5, 88, true);
        Create_Icon("20006.6", "Currency Rate Source", "../Mst/master_ctrl", 3, "", "", false, "-", "exbm", "eximmain", 5, 88, true);
        Create_Icon("20006.7", "Forward Currency Master", "../Mst/master_ctrl", 3, "", "", false, "-", "exbm", "eximmain", 5, 88, true);

        Create_Icon("20008", "Forward Contract", "", 2, "fa fa-bank", "nav child_menu", true, "frcon", "eximmain", "eximmain", 5, 88, true);
        Create_Icon("20008.1", "Book Forward", "../Exim/book_fwd", 3, "", "", false, "-", "frcon", "eximmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("20008.2", "Use Forward", "../Exim/use_fwd", 3, "", "", false, "-", "frcon", "eximmain", 5, Convert.ToDecimal(88.01), true);


        Create_Icon("20009", "Export Master", "", 2, "fa fa-bank", "nav child_menu", true, "exms", "eximmain", "eximmain", 5, 88, true);
        Create_Icon("20009.1", "Form Control", "../Mst/form_ctrl", 3, "", "", false, "-", "exms", "eximmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("20009.2", "Bulk Upload", "../vastu/bulk_acc", 3, "", "", false, "-", "exms", "eximmain", 5, Convert.ToDecimal(88.01), true);

        //currency Master

        Create_Icon("20007", "Country Master", "", 2, "fa fa-bank", "nav child_menu", true, "excm", "eximmain", "eximmain", 5, 88, true);
        Create_Icon("20007.1", "Select Country", "../Mst/doc_master", 3, "", "", false, "-", "excm", "eximmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("20007.2", "Continents Master", "../Exim/continents", 3, "", "", false, "-", "excm", "eximmain", 5, Convert.ToDecimal(88.01), true);

        Create_Icon("20010", "Export Banking", "", 2, "fa fa-bank", "nav child_menu", true, "exbn", "eximmain", "eximmain", 5, 88, true);
        Create_Icon("20010.1", "Discount PI", "../Exim/dis_pi", 3, "", "", false, "-", "exbn", "eximmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("20010.2", "Discount CI", "../Exim/dis_ci", 3, "", "", false, "-", "exbn", "eximmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("20010.3", "Advance Ag PI", "../Exim/adv_pi", 3, "", "", false, "-", "exbn", "eximmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("20010.4", "Direct Collection", "../Exim/dir_coll", 3, "", "", false, "-", "exbn", "eximmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("20010.5", "Bank Collection", "../Exim/bank_rec", 3, "", "", false, "-", "exbn", "eximmain", 5, Convert.ToDecimal(88.01), true);

        Create_Icon("20011", "Bank Document", "", 2, "fa fa-bank", "nav child_menu", true, "bndoc", "eximmain", "eximmain", 5, 88, true);
        Create_Icon("20011.1", "SB Submit In Bank", "../Exim/sb_bank", 3, "", "", false, "-", "bndoc", "eximmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("20011.2", "Document Submit For Col", "../Exim/bank_coll", 3, "", "", false, "-", "bndoc", "eximmain", 5, Convert.ToDecimal(88.01), true);
        #endregion

        Create_Icon("21000", "Billing & Collection", "../../../../erp/dashboard", 1, "", "", true, "", "bnpmain", "bnpmain", 5, 21, true, "Order to cash cycle", "Fully automate order to cash cycle");
        //billing module
        #region
        Create_Icon("21001", "Billing Master", "", 2, "fa fa-gears", "nav child_menu", true, "bmas", "bnpmain", "bnpmain", 5, 88, true);
        Create_Icon("21001.1", "Unit Measurement", "../Mst/master_ctrl", 3, "", "", false, "-", "bmas", "bnpmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("21001.2", "HSN/SAC Code", "../Billing/billing_master", 3, "", "", false, "-", "bmas", "bnpmain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("21001.3", "Item Master", "../Billing/itemservice", 3, "", "", false, "-", "bmas", "bnpmain", 5, Convert.ToDecimal(88.03), true);
        Create_Icon("21001.4", "Doc No. Pattern", "../Education/pattern_gen", 3, "", "", false, "-", "bmas", "bnpmain", 5, Convert.ToDecimal(88.03), true);
        Create_Icon("21001.6", "Invoice Config", "../../../../erp/billing_and_collection/invoice_config", 3, "", "", false, "-", "bmas", "bnpmain", 5, Convert.ToDecimal(88.03), true);
        Create_Icon("21001.7", "Payment Term Master", "../Mst/master_ctrl", 3, "", "", false, "-", "bmas", "bnpmain", 5, Convert.ToDecimal(88.03), true);
        //Create_Icon("21001.8", "Invoice Template", "../Home/temp_Report", 3, "", "", false, "-", "bmas", "bnpmain", 5, Convert.ToDecimal(88.03), true);
        Create_Icon("21001.8", "Invoice Template", "../Billing/inv_temp", 3, "", "", false, "-", "bmas", "bnpmain", 5, Convert.ToDecimal(88.03), true);
        Create_Icon("28004.10", "Transport Mode", "../Mst/master_ctrl", 3, "", "", false, "-", "bmas", "bnpmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("28004.11", "Payment Term Master", "../Mst/master_ctrl", 3, "", "", false, "-", "bmas", "bnpmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("40001.7", "Form Control", "../Mst/form_ctrl", 3, "", "", false, "", "bmas", "bnpmain", 5, 50, true);
        Create_Icon("21001.12", "SO Type Master", "../Mst/doc_master", 3, "", "", false, "-", "bmas", "bnpmain", 5, Convert.ToDecimal(88.03), true);
        //Create_Icon("21001.13", "Doc Template", "../Education/rpt_temp", 3, "", "", false, "", "bmas", "bnpmain", 5, Convert.ToDecimal(85.02), true);
        Create_Icon("21001.13", "Doc Template", "../Mst/prn_ctrl", 3, "", "", false, "", "bmas", "bnpmain", 5, Convert.ToDecimal(85.02), true);
        Create_Icon("21001.14", "Bulk Upload", "../vastu/bulk_acc", 3, "", "", false, "-", "bmas", "bnpmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("21001.15", "Approval Config", "../Inventory/apprv_config", 3, "", "", false, "-", "bmas", "bnpmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("21001.16", "Sales Channel Master", "../mst/master_ctrl", 3, "", "", false, "-", "bmas", "bnpmain", 5, Convert.ToDecimal(88.01), true);

        Create_Icon("21002", "Billing", "", 2, "fa fa-spinner", "nav child_menu", true, "bill", "bnpmain", "bnpmain", 5, 88, true);
        Create_Icon("21002.1", "Invoice", "../Billing/invoice", 3, "", "", false, "-", "bill", "bnpmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("21002.2", "Dispatch Schedule", "../Inventory/dis_sch", 3, "", "", false, "-", "bill", "bnpmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("21002.3", "Close Pending Dis Sch", "../Inventory/p_close", 3, "", "", false, "-", "bill", "bnpmain", 5, Convert.ToDecimal(88.01), true);

        //Create_Icon("21003", "Work Order", "", 2, "fa fa-bank", "nav child_menu", true, "bwo", "bnpmain", "bnpmain", 5, 88, true);
        //Create_Icon("21003.1", "Create WO", "../../../../erp/school_admin/School_Master", 3, "", "", false, "-", "bwo", "bnpmain", 5, Convert.ToDecimal(88.01), true);

        Create_Icon("21004", "Cash Inflow", "", 2, "fa fa-bank", "nav child_menu", true, "bcf", "bnpmain", "bnpmain", 5, 88, true);
        Create_Icon("21004.1", "Pending Payment", "../Billing/pend_pay", 3, "", "", false, "-", "bcf", "bnpmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("21004.2", "Payment Receipt", "../Billing/pay_rec", 3, "", "", false, "-", "bcf", "bnpmain", 5, Convert.ToDecimal(88.01), true);

        Create_Icon("21005", "Price Master", "", 2, "fa fa-gears", "nav child_menu", true, "prcms", "bnpmain", "bnpmain", 5, 88, true);
        Create_Icon("21005.1", "Standard Price", "../Billing/standard_price", 3, "", "", false, "-", "prcms", "bnpmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("21005.2", "Custom Price", "../Billing/custom_price", 3, "", "", false, "-", "prcms", "bnpmain", 5, Convert.ToDecimal(88.01), true);

        Create_Icon("21008", "Customer Master", "", 2, "fa fa-gears", "nav child_menu", true, "bcltmas", "bnpmain", "bnpmain", 5, 47, true);
        Create_Icon("21008.1", "Customer Detail", "../Purchase/party", 3, "", "", false, "", "bcltmas", "bnpmain", 5, 50, true);
        //Create_Icon("21008.2", "Customer Unit Detail", "", 3, "", "", false, "", "bcltmas", "bnpmain", 5, 50, true);
        Create_Icon("21008.2", "List of Customers", "", 3, "", "", false, "", "bcltmas", "bnpmain", 5, Convert.ToDecimal(74.04), true, "onclick=$menuclick(this);$");


        Create_Icon("21009", "Pre Billing", "", 2, "fa fa-umbrella", "nav child_menu", true, "pb", "bnpmain", "bnpmain", 5, 47, true);
        Create_Icon("21009.1", "Performa Invoice", "../Billing/p_invoice", 3, "", "", false, "", "pb", "bnpmain", 5, 50, true);
        Create_Icon("21009.2", "Sales Order", "../Billing/s_order", 3, "", "", false, "", "pb", "bnpmain", 5, 50, true);
        Create_Icon("21009.3", "Quotation", "../Billing/quot", 3, "", "", false, "", "pb", "bnpmain", 5, 50, true);
        Create_Icon("21009.4", "Close Pending SO", "../Inventory/p_close", 3, "", "", false, "", "pb", "bnpmain", 5, 50, true);
        Create_Icon("21009.5", "Sales Forcast", "../Billing/s_fcast", 3, "", "", false, "", "pb", "bnpmain", 5, 50, true);
        Create_Icon("21009.6", "Close Pending PI", "../Inventory/p_close", 3, "", "", false, "", "pb", "bnpmain", 5, 50, true);
        Create_Icon("21009.7", "Close Pending Quotation", "../Inventory/p_close", 3, "", "", false, "", "pb", "bnpmain", 5, 50, true);

        Create_Icon("21010", "Logistic", "", 2, "fa fa-bank", "nav child_menu", true, "lgs", "bnpmain", "bnpmain", 5, 88, true);
        Create_Icon("21010.1", "POD Entry", "../Billing/pod_entry", 3, "", "", false, "", "lgs", "bnpmain", 5, 50, true);


        //Create_Icon("21010", "Report", "", 2, "fa fa-user", "-", false, "", "bnpmain", "bnpmain", 5, 47, true);

        Create_Icon("17005", "Reports", "", 2, "fa fa-print", "nav child_menu", true, "bnprp", "bnpmain", "bnpmain", 5, 88, true);
        Create_Icon("17005.1", "Stock Reports", "../Inventory/invreps2", 3, "", "", false, "-", "bnprp", "bnpmain", 5, Convert.ToDecimal(88.01), true);
       // Create_Icon("17005.1", "Stock Reports", "../Inventory/invreps", 3, "", "", false, "-", "bnprp", "bnpmain", 5, Convert.ToDecimal(88.01), true);

        #endregion

        Create_Icon("22000", "Accounts & Finance", "../../../../erp/dashboard", 1, "", "", true, "", "acctmain", "acctmain", 5, 20, true);
        //account module
        #region
        Create_Icon("22001", "Account Master", "", 2, "fa fa-bank", "nav child_menu", true, "acct", "acctmain", "acctmain", 5, 88, true);
        Create_Icon("22001.1", "Group Master", "../Account/grp_mst", 3, "", "", false, "-", "acct", "acctmain", 5, 88, true);
        Create_Icon("22001.2", "Ledger Master", "../Account/lgr_mst", 3, "", "", false, "-", "acct", "acctmain", 5, 88, true);
        Create_Icon("22001.3", "Voucher Pattern", "../../../../erp/Account_Master/voucher_pattern", 3, "", "", false, "-", "acct", "acctmain", 5, 88, true);
        Create_Icon("22001.4", "Choose Template", "../Purchase/purchase_order", 3, "", "", false, "-", "acct", "acctmain", 5, 88, true);
        Create_Icon("22001.5", "Inter Unit", "", 3, "", "", false, "-", "acct", "acctmain", 5, 88, true);
        Create_Icon("22001.6", "Cheque Print Location", "../Mst/master_ctrl", 3, "", "", false, "-", "acct", "acctmain", 5, 88, true);
        Create_Icon("22001.7", "Ledger Type", "../Mst/doc_master", 3, "", "", false, "-", "acct", "acctmain", 5, 88, true);
        Create_Icon("22001.8", "Party Opening", "../Account/party_opening", 3, "", "", false, "-", "acct", "acctmain", 5, 88, true);
        Create_Icon("22001.9", "Account Mapping", "../Account/acc_link", 3, "", "", false, "-", "acct", "acctmain", 5, 88, true);
        //Create_Icon("22001.10", "Pay Amount", "../Account/payamt", 3, "", "", false, "-", "acct", "acctmain", 5, 88, true);
        //bank master
        Create_Icon("22007", "Bank Master", "", 2, "fa fa-bank", "nav child_menu", true, "bnkacct", "acctmain", "acctmain", 5, 88, true);
        Create_Icon("22007.1", "Bank Name", "../Mst/master_ctrl", 3, "", "", false, "-", "bnkacct", "acctmain", 5, 88, true);
        Create_Icon("22007.2", "Bank Acct Type", "../Mst/master_ctrl", 3, "", "", false, "-", "bnkacct", "acctmain", 5, 88, true);
        Create_Icon("22007.3", "Bank Acct Details", "../Account/bank_ac", 3, "", "", false, "-", "bnkacct", "acctmain", 5, 88, true);
        Create_Icon("22007.4", "Currency Master", "../Mst/master_ctrl", 3, "", "", false, "-", "bnkacct", "acctmain", 5, 88, true);
        Create_Icon("22007.5", "Currency Rate", "../Account/cur_rate", 3, "", "", false, "-", "bnkacct", "acctmain", 5, 88, true);
        //Create_Icon("22007.6", "Cheque Series", "../Account/chq_sr", 3, "", "", false, "-", "bnkacct", "acctmain", 5, 88, true);
        Create_Icon("22007.7", "Currency Rate Source", "../Mst/master_ctrl", 3, "", "", false, "-", "bnkacct", "acctmain", 5, 88, true);
        Create_Icon("22007.8", "Cheque Series", "../Account/chq_srl", 3, "", "", false, "-", "bnkacct", "acctmain", 5, 88, true);
        Create_Icon("22007.9", "Cheque Rejection", "../Account/chq_rej", 3, "", "", false, "-", "bnkacct", "acctmain", 5, 88, true);


        //Create_Icon("22002", "Vender", "../../../../erp/Accounts/add_customer", 2, "fa fa-bank", "", false, "", "acctmain", "acctmain", 5, 88, true);
        Create_Icon("22003", "Voucher Entry", "../Account/vou_entry", 2, "fa fa-bank", "", false, "vchacc", "acctmain", "acctmain", 5, 88, true);
        Create_Icon("22003.1", "Cash Receipt", "../Account/cash_vou", 3, "", "", false, "-", "vchacc", "acctmain", 5, 88, true);
        Create_Icon("22003.2", "Cash Payment", "../Account/cash_vou", 3, "", "", false, "-", "vchacc", "acctmain", 5, 88, true);
        Create_Icon("22003.3", "Bank Receipt", "../Account/rcm", 3, "", "", false, "-", "vchacc", "acctmain", 5, 88, true);
        Create_Icon("22003.4", "Bank Payment", "../Account/payamt", 3, "", "", false, "-", "vchacc", "acctmain", 5, 88, true);
        Create_Icon("22003.5", "Journal", "../Account/vou_entry", 3, "", "", false, "-", "vchacc", "acctmain", 5, 88, true);
        Create_Icon("22003.6", "Contra", "../Account/vou_entry", 3, "", "", false, "-", "vchacc", "acctmain", 5, 88, true);
        Create_Icon("22003.7", "Purchase", "../Account/vou_entry", 3, "", "", false, "-", "vchacc", "acctmain", 5, 88, true);
        Create_Icon("22003.8", "Sales", "../Account/sl_voucher", 3, "", "", false, "-", "vchacc", "acctmain", 5, 88, true);
        Create_Icon("22003.9", "Debit Note", "../Account/vou_entry", 3, "", "", false, "-", "vchacc", "acctmain", 5, 88, true);
        Create_Icon("22003.10", "Credit Note", "../Account/vou_entry", 3, "", "", false, "-", "vchacc", "acctmain", 5, 88, true);
        Create_Icon("22003.11", "Reciept", "../Account/cas_res", 3, "", "", false, "-", "vchacc", "acctmain", 5, 88, true);
        Create_Icon("22003.12", "Payment", "../Account/cas_res", 3, "", "", false, "-", "vchacc", "acctmain", 5, 88, true);

        Create_Icon("22004", "GST Report", "", 2, "fa fa-bank", "nav child_menu", true, "gst", "acctmain", "acctmain", 5, 88, true);
        Create_Icon("21004.1", "GST Report", "../Account/gst_rpt", 3, "", "", false, "-", "gst", "acctmain", 5, Convert.ToDecimal(88.01), true);
        //Create_Icon("21005.2", "GST on Input", "../../../../erp/", 3, "", "", false, "-", "gst", "acctmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("21004.3", "GST Return", "../Account/gst_ret", 3, "", "", false, "", "gst", "acctmain", 5, Convert.ToDecimal(88.01), true);
        //Create_Icon("21005.3.1", "GSTR 3B", "../../../../erp/", 4, "", "", false, "-", "gstret", "acctmain", 5, Convert.ToDecimal(88.01), true);
        //Create_Icon("21005.3.2", "GSTR 1", "../../../../erp/", 4, "", "", false, "-", "gstret", "acctmain", 5, Convert.ToDecimal(88.01), true);

        Create_Icon("21006", "TDS Report", "", 2, "fa fa-bank", "nav child_menu", true, "tdr", "acctmain", "acctmain", 5, 88, true);
        Create_Icon("21006.1", "TDS Receivable", "../../../../erp/", 3, "", "", false, "-", "tdr", "acctmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("21006.2", "TDS Payable", "../../../../erp/", 3, "", "", false, "-", "tdr", "acctmain", 5, Convert.ToDecimal(88.01), true);

        Create_Icon("22005", "Party Master", "", 2, "fa fa-bank", "nav child_menu", true, "prtmst", "acctmain", "acctmain", 5, 88, true);
        Create_Icon("22005.1", "Vendor Account", "../Purchase/party", 3, "", "", false, "-", "prtmst", "acctmain", 5, 88, true);
        Create_Icon("22005.2", "Customer Account", "../Purchase/party", 3, "", "", false, "-", "prtmst", "acctmain", 5, 88, true);

        Create_Icon("28005.3", "Organisation Status", "../Mst/master_ctrl", 3, "", "", false, "", "prtmst", "acctmain", 5, 50, true);
        Create_Icon("28005.4", "Bank Acct Type", "../Mst/master_ctrl", 3, "", "", false, "", "prtmst", "acctmain", 5, 50, true);
        Create_Icon("28005.5", "Currency Type", "../Mst/master_ctrl", 3, "", "", false, "", "prtmst", "acctmain", 5, 50, true);
        Create_Icon("28005.6", "Bank Name", "../Mst/master_ctrl", 3, "", "", false, "", "prtmst", "acctmain", 5, 50, true);
        Create_Icon("40001.6", "Sales/Service Area", "../Mst/master_ctrl", 3, "", "", false, "", "prtmst", "acctmain", 5, 50, true);
        Create_Icon("40001.5", "Occupation Master", "../Mst/master_ctrl", 3, "", "", false, "", "prtmst", "acctmain", 5, 50, true);
        Create_Icon("40001.3", "Type Of Account", "../Mst/master_ctrl", 3, "", "", false, "", "prtmst", "acctmain", 5, 50, true);
        Create_Icon("40001.8", "Type Of Address", "../Mst/master_ctrl", 3, "", "", false, "", "prtmst", "acctmain", 5, 50, true);
        Create_Icon("40001.4", "Business Type", "../Mst/master_ctrl", 3, "", "", false, "", "prtmst", "acctmain", 5, 50, true);
        Create_Icon("12003.3", "Product Category Master", "../Mst/master_ctrl", 3, "", "", false, "-", "prtmst", "acctmain", 5, 110, true);
        Create_Icon("21001.7", "Payment Term Master", "../Mst/master_ctrl", 3, "", "", false, "-", "prtmst", "acctmain", 5, Convert.ToDecimal(88.03), true);
        Create_Icon("7004.1.7", "Qualification", "../Mst/master_ctrl", 3, "", "", false, "", "prtmst", "acctmain", 5, 50, true);
        Create_Icon("27008", "Annual Income Master", "../Mst/master_ctrl", 2, "fa fa-bank", "", false, "", "prtmst", "acctmain", 5, 88, true);
        Create_Icon("22005.3", "Form Control", "../Mst/form_ctrl", 3, "", "", false, "", "prtmst", "acctmain", 5, 50, true);


        #endregion

        Create_Icon("23000", "Transportation", "../../../../erp/dashboard", 1, "fa fa-user fa-5x", "btn btn-sq-lg btn-primary", true, "trp", "trpmain", "trpmain", 5, 2, true, "Manage your Transport", "Manage your Vehicle along with Route creation");
        //18/06/2018  transportation Module
        #region
        Create_Icon("23001", "Route Master", "../../../../erp/transportation/route_mst", 2, "", "", false, "-", "trasmain", "trpmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("23002", "Location/Stoppage Master", "../Education/location_mst", 2, "", "", false, "-", "trasmain", "trpmain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("23003", "Vehicle Master", "../../../../erp/transportation/vehicle_mst", 2, "", "", false, "-", "trasmain", "trpmain", 5, Convert.ToDecimal(88.03), true);
        Create_Icon("23004", "Transport Fee Master", "../Education/transport_feemaster", 2, "", "", false, "-", "trasmain", "trpmain", 5, Convert.ToDecimal(88.04), true);
        #endregion

        Create_Icon("24000", "Registration/Approvals", "../../../../erp/dashboard", 1, "fa fa-user fa-5x", "btn btn-sq-lg btn-primary", true, "srm", "srmain", "srmain", 5, 2, true, "Manage your Transport", "Manage your Vehicle along with Route creation");
        #region
        //26/07/2018  Office Records 
        //Create_Icon("24001", "Reg/Approval Master", "", 2, "fa fa-bank", "nav child_menu", true, "regapp", "main", "srmain", 5, 88, true);
        //Create_Icon("24001.1", "Division Master", "../../../../erp/school_admin/School_Master", 3, "", "", false, "-", "regapp", "srmain", 5, Convert.ToDecimal(88.01), true);
        //Create_Icon("24001.2", "Type Master", "../../../../erp/school_admin/School_Master", 3, "", "", false, "-", "regapp", "srmain", 5, Convert.ToDecimal(88.01), true);
        //Create_Icon("24001.3", "File Category Master", "../../../../erp/school_admin/School_Master", 3, "", "", false, "-", "regapp", "srmain", 5, Convert.ToDecimal(88.01), true);
        //Create_Icon("24001.4", "Affiliation Authority Master", "../../../../erp/regapproval/aff_authority_master", 3, "", "", false, "-", "regapp", "srmain", 5, Convert.ToDecimal(88.01), true);
        //Create_Icon("24001.5", "Affiliation Master", "../../../../erp/regapproval/affiliation_master", 3, "", "", false, "-", "regapp", "srmain", 5, Convert.ToDecimal(88.01), true);
        //Create_Icon("24002", "Office Records", "", 2, "fa fa-bank", "nav child_menu", true, "offrec", "main", "srmain", 5, 88, true);
        //Create_Icon("24002.1", "Registration Approvals", "../../../../erp/regapproval/upload_certificate", 3, "", "", false, "-", "offrec", "srmain", 5, Convert.ToDecimal(88.01), true);
        
        Create_Icon("24001", "Reg/Approval Master", "", 2, "fa fa-bank", "nav child_menu", true, "regapp", "main", "srmain", 5, 88, true);
        Create_Icon("24001.1", "Division Master", "../mst/master_ctrl", 3, "", "", false, "-", "regapp", "srmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("24001.2", "Type Master", "../mst/master_ctrl", 3, "", "", false, "-", "regapp", "srmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("24001.3", "File Category Master", "../mst/master_ctrl", 3, "", "", false, "-", "regapp", "srmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("24001.4", "Affiliation Authority Master", "../mst/master_ctrl", 3, "", "", false, "-", "regapp", "srmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("24001.5", "Affiliation Master", "../mst/master_ctrl", 3, "", "", false, "-", "regapp", "srmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("24002", "Office Records", "", 2, "fa fa-bank", "nav child_menu", true, "offrec", "main", "srmain", 5, 88, true);
        //Create_Icon("24002.1", "Registration Approvals", "../../../../erp/regapproval/upload_certificate", 3, "", "", false, "-", "offrec", "srmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("24002.1", "Registration Approvals", "../hall/reg_apr", 3, "", "", false, "-", "offrec", "srmain", 5, Convert.ToDecimal(88.01), true);

        #endregion

        Create_Icon("25000", "Front Desk", "../../../../erp/dashboard", 1, "fa fa-user fa-5x", "btn btn-sq-lg btn-primary", true, "", "fdskmain", "fdskmain", 5, 2, true, "Visitor Relationship", "Manage Your Visitors");
        //front desk
        #region 
        Create_Icon("25001", "Visitor Pre-Registration", "../Hr/v_form", 2, "fa fa-bank", "", false, "", "fdskmain", "fdskmain", 5, 88, true);
        Create_Icon("25002", "Visitor", "../Hr/v_check", 2, "fa fa-bank", "", false, "", "fdskmain", "fdskmain", 5, 88, true);
        Create_Icon("25003", "Blocked Visitor", "../../../../../erp/help_desk/blocked_visitor", 2, "fa fa-bank", "", false, "", "fdskmain", "fdskmain", 5, 88, true);
        Create_Icon("25004", "Visitor Feedback", "../../../../../erp/help_desk/visitor_feedback", 2, "fa fa-bank", "", false, "", "fdskmain", "fdskmain", 5, 88, true);
        Create_Icon("25005", "Visitor Master", "", 2, "fa fa-bank", "", true, "fdsk", "fdskmain", "fdskmain", 5, 88, true);
        Create_Icon("25005.1", "Allow Camera Setting", "../../../../erp/help_desk/camera_permission", 3, "", "", false, "", "fdsk", "fdskmain", 5, 88, true);
        Create_Icon("25005.2", "Visitor Id Type", "../../../../erp/school_admin/School_Master", 3, "", "", false, "", "fdsk", "fdskmain", 5, 88, true);
        Create_Icon("25006", "Feedback Form Creator", "", 2, "fa fa-bank", "", true, "ffc", "fdskmain", "fdskmain", 5, 88, true);
        Create_Icon("25006.1", "Feedback Question", "../../../../erp/help_desk/visitor_questionpaper", 3, "", "", false, "", "ffc", "fdskmain", 5, 88, true);
        Create_Icon("25006.2", "Feedback Form", "../../../../erp/help_desk/visitor_paper", 3, "", "", false, "", "ffc", "fdskmain", 5, 88, true);
        Create_Icon("25007", "Admission Enquiry", "../Education/Enquiry", 2, "fa fa-bank", "", false, "", "fdskmain", "fdskmain", 5, 88, true);
        #endregion

        Create_Icon("26000", "Hostel", "../../../../erp/dashboard", 1, "fa fa-user fa-5x", "btn btn-sq-lg btn-primary", true, "", "hostmain", "hostmain", 5, 2, true, "Hostal Management", "Manage Your All Hostal Activities");
        //hostel
        #region hostel
        Create_Icon("26001", "Hostel Master", "", 2, "fa fa-bank", "nav child_menu", true, "host", "hostmain", "hostmain", 5, 88, true);
        Create_Icon("26001.1", "Building Name", "../Mst/master_ctrl", 3, "", "", false, "-", "host", "hostmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("26001.2", "Wing/Block Name", "../../../../erp/hostel/block_master", 3, "", "", false, "-", "host", "hostmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("26001.3", "Room Detail", "../../../../erp/hostel/add_room", 3, "", "", false, "-", "host", "hostmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("26001.4", "Hostel Fee Master", "../../../../erp/school_admin/quespaper/upload_ac_material", 3, "", "", false, "-", "host", "hostmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("26001.5", "Fee Setting", "../../../../erp/school_admin/quespaper/upload_ac_material", 3, "", "", false, "-", "host", "hostmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("26001.6", "Id Card Design", "../../../../erp/school_admin/quespaper/upload_ac_material", 3, "", "", false, "-", "host", "hostmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("26001.7", "Mess Setting", "../../../../erp/school_admin/quespaper/upload_ac_material", 3, "", "", false, "-", "host", "hostmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("26001.8", "Room Type", "../../../../erp/school_admin/School_Master", 3, "", "", false, "-", "host", "hostmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("26001.9", "Room Facility", "../../../../erp/school_admin/School_Master", 3, "", "", false, "-", "host", "hostmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("26001.10", "Doc No. Pattern", "../Education/pattern_gen", 3, "", "", false, "-", "host", "hostmain", 5, Convert.ToDecimal(88.01), true);

        Create_Icon("26002", "Room Fee", "", 2, "fa fa-money", "nav child_menu", true, "rmfee", "hostmain", "hostmain", 5, 88, true);
        Create_Icon("26002.1", "Room Security Fee", "", 3, "", "", false, "-", "rmfee", "hostmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("26002.2", "Room Regular Fee", "", 3, "", "", false, "-", "rmfee", "hostmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("26002.3", "Hostel Mess Fee", "", 3, "", "", false, "-", "rmfee", "hostmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("26002.4", "Refund Fee", "", 3, "", "", false, "-", "rmfee", "hostmain", 5, Convert.ToDecimal(88.01), true);

        Create_Icon("26003", "Room Allotment", "", 2, "fa fa-home", "nav child_menu", true, "rmalot", "hostmain", "hostmain", 5, 88, true);
        Create_Icon("26003.1", "Room Request", "../../../../../erp/hostel/room_request", 3, "", "", false, "-", "rmalot", "hostmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("26003.2", "Room Allotment", "../../../../../erp/hostel/room_allotment", 3, "", "", false, "-", "rmalot", "hostmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("26003.3", "Room Change Confirmation", "../../../../../erp/hostel/room_change_confirmation", 3, "", "", false, "-", "rmalot", "hostmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("26003.4", "Room Checkout", "../../../../../erp/hostel/room_checkout", 3, "", "", false, "-", "rmalot", "hostmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("26003.5", "Room Cancellation", "../../../../../erp/hostel/room_cancelation", 3, "", "", false, "-", "rmalot", "hostmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("26003.6", "Room Status", "../../../../../erp/hostel/room_status", 3, "", "", false, "-", "rmalot", "hostmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("26003.7", "Room Change Request", "../../../../../erp/hostel/room_change_request", 3, "", "", false, "-", "rmalot", "hostmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("26004", "Hostel Id Card", "../../../../../erp/hostel/hostel_card_pattern", 2, "fa fa-bank", "", false, "-", "hostmain", "hostmain", 5, Convert.ToDecimal(88.04), true);
        #endregion

        Create_Icon("27000", "Scholarship", "../../../../erp/dashboard", 1, "fa fa-user fa-5x", "btn btn-sq-lg btn-primary", true, "", "schlarmain", "schlarmain", 5, 2, true, "a fund of knowledge and learning", "a foundation to provide financial assistance to students.");
        //22 aug 2018 scholarship
        #region scholarship
        Create_Icon("27001", "Application Stage", "../../../../../erp/scholarship/application_stage", 2, "fa fa-bank", "", false, "", "schlarmain", "schlarmain", 5, 88, true);
        Create_Icon("27002", "Pre Screening", "../../../../../erp/scholarship/pre_screening", 2, "fa fa-bank", "", false, "", "schlarmain", "schlarmain", 5, 88, true);
        Create_Icon("27003", "Candidate Selection", "../../../../../erp/scholarship/candidate_selection", 2, "fa fa-bank", "", false, "", "schlarmain", "schlarmain", 5, 88, true);
        Create_Icon("27004", "Awarded", "", 2, "fa fa-bank", "", false, "", "schlarmain", "schlarmain", 5, 88, true);
        Create_Icon("27005", "Interview Stage", "", 2, "fa fa-bank", "", false, "", "schlarmain", "schlarmain", 5, 88, true);
        Create_Icon("27006", "Waitlist", "", 2, "fa fa-bank", "", false, "", "schlarmain", "schlarmain", 5, 88, true);
        Create_Icon("27007", "Declined", "", 2, "fa fa-bank", "", false, "", "schlarmain", "schlarmain", 5, 88, true);
        Create_Icon("27008", "Annual Income Master", "../Mst/master_ctrl", 2, "fa fa-bank", "", false, "", "schlarmain", "schlarmain", 5, 88, true);
        Create_Icon("27009", "Scholarship Master", "../../../../erp/school_admin/Frm_Scholarship_master", 2, "fa fa-bank", "", false, "", "schlarmain", "schlarmain", 5, 88, true);
        #endregion

        Create_Icon("28000", "Purchase", "../../../../erp/dashboard", 1, "", "", true, "", "purmain", "purmain", 5, 20, true, "", "");
        #region
        Create_Icon("28001", "Indent", "", 2, "fa fa-bank", "nav child_menu", true, "ind", "purmain", "purmain", 5, 88, true);
        Create_Icon("28001.1", "Create Indent", "../Purchase/indent_req", 3, "", "", false, "-", "ind", "purmain", 5, Convert.ToDecimal(88.01), true);

        Create_Icon("28002", "Purchase Order", "", 2, "fa fa-bank", "nav child_menu", true, "por", "purmain", "purmain", 5, 88, true);
        Create_Icon("28002.1", "PO Print", "../Purchase/purchase_order", 3, "", "", false, "-", "por", "purmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("28002.2", "Create PO", "../Purchase/po_withindent", 3, "", "", false, "-", "por", "purmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("28002.3", "Close Pending PO", "../Inventory/p_close", 3, "", "", false, "-", "por", "purmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("28002.4", "RFQ", "../Billing/rfq", 3, "", "", false, "-", "por", "purmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("28002.5", "PO Schedule", "../Purchase/po_sch", 3, "", "", false, "-", "por", "purmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("28002.6", "PO Import", "../Purchase/po_imp", 3, "", "", false, "-", "por", "purmain", 5, Convert.ToDecimal(88.01), true);

        Create_Icon("28003", "PO Config", "", 2, "fa fa-bank", "nav child_menu", true, "poc", "purmain", "purmain", 5, 88, true);
        Create_Icon("28003.1", "PO Print", "", 3, "", "", false, "-", "poc", "purmain", 5, Convert.ToDecimal(88.01), true);

        Create_Icon("28004", "Purchase Master", "", 2, "fa fa-bank", "nav child_menu", true, "purm", "purmain", "purmain", 5, 88, true);
        Create_Icon("28004.1", "Item Master", "../Billing/itemservice", 3, "", "", false, "-", "purm", "purmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("28004.3", "Doc No. Pattern", "../Education/pattern_gen", 3, "", "", false, "-", "purm", "purmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("28004.4", "PO Template", "", 3, "", "", false, "-", "purm", "purmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("28004.5", "Unit Measurement", "../Mst/master_ctrl", 3, "", "", false, "-", "purm", "purmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("28004.6", "HSN/SAC", "../Billing/billing_master", 3, "", "", false, "-", "purm", "purmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("28004.7", "PO Type Master", "../Mst/doc_master", 3, "", "", false, "-", "purm", "purmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("28004.8", "Delivery Term", "../Mst/master_ctrl", 3, "", "", false, "-", "purm", "purmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("28004.9", "Price Term", "../Mst/master_ctrl", 3, "", "", false, "-", "purm", "purmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("28004.10", "Transport Mode", "../Mst/master_ctrl", 3, "", "", false, "-", "purm", "purmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("28004.11", "Payment Term Master", "../Mst/master_ctrl", 3, "", "", false, "-", "purm", "purmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("17001.25", "Form Control", "../Mst/form_ctrl", 3, "", "", false, "", "purm", "purmain", 5, 50, true);
        Create_Icon("28004.12", "Print Config", "../mst/prn_ctrl", 3, "", "", false, "-", "purm", "purmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("28004.13", "Insured By Master", "../mst/master_ctrl", 3, "", "", false, "-", "purm", "purmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("28004.14", "Payment Mode", "../mst/master_ctrl", 3, "", "", false, "-", "purm", "purmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("28004.15", "Bulk Upload", "../vastu/bulk_acc", 3, "", "", false, "-", "purm", "purmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("28004.16", "Approval Config", "../Inventory/apprv_config", 3, "", "", false, "-", "purm", "purmain", 5, Convert.ToDecimal(88.01), true);

        Create_Icon("28005", "Vendor Master", "", 2, "fa fa-user", "nav child_menu", true, "vdm", "purmain", "purmain", 5, 47, true);
        Create_Icon("28005.1", "Vendor Detail", "../Purchase/party", 3, "", "", false, "", "vdm", "purmain", 5, 50, true);
        //Create_Icon("28005.2", "Vendor Unit Detail", "../../../../../erp/client_assignment/add_client_unit", 3, "", "", false, "", "vdm", "purmain", 5, 50, true);
        //  Create_Icon("28005.2", "Vendor Unit Detail", "../Purchase/party_unit", 3, "", "", false, "", "vdm", "purmain", 5, 50, true);
        Create_Icon("28005.3", "Party Type", "../Mst/master_ctrl", 3, "", "", false, "", "vdm", "purmain", 5, 50, true);
        Create_Icon("28005.4", "Bank Acct Type", "../Mst/master_ctrl", 3, "", "", false, "", "vdm", "purmain", 5, 50, true);
        Create_Icon("28005.5", "Currency Type", "../Mst/master_ctrl", 3, "", "", false, "", "vdm", "purmain", 5, 50, true);
        Create_Icon("28005.6", "Bank Name", "../Mst/master_ctrl", 3, "", "", false, "", "vdm", "purmain", 5, 50, true);
        Create_Icon("28005.7", "List of Vendors", "", 3, "", "", false, "", "vdm", "purmain", 5, Convert.ToDecimal(74.04), true, "onclick=$menuclick(this);$");

        Create_Icon("28006", "Approval", "", 2, "fa fa-user", "nav child_menu", true, "papp", "purmain", "purmain", 5, 47, true);
        Create_Icon("28006.1", "Vendor Approval", "../Purchase/vendor_app", 3, "", "", false, "", "papp", "purmain", 5, 50, true);
        // Create_Icon("28006.2", "PR Approval", "../Purchase/pr_app", 3, "", "", false, "", "papp", "purmain", 5, 50, true);
        Create_Icon("28006.2", "Indent Approval", "../Purchase/pr_app", 3, "", "", false, "", "papp", "purmain", 5, 50, true);
        Create_Icon("28006.3", "PO Approval", "../Purchase/po_app", 3, "", "", false, "", "papp", "purmain", 5, 50, true);

        Create_Icon("28007", "Report", "", 2, "fa fa-user", "nav child_menu", true, "prpt", "purmain", "purmain", 5, 47, true);
        //Create_Icon("28007.1", "Pur Report", "../Purchase/pur_rpt", 3, "", "", false, "", "prpt", "purmain", 5, 50, true);
        Create_Icon("28007.1", "Pur Report", "../Purchase/pur_rpt2", 3, "", "", false, "", "prpt", "purmain", 5, 50, true);

        #endregion

        Create_Icon("29000", "Fixed Asset Management", "../../../../erp/dashboard", 1, "", "", true, "", "fixmain", "fixmain", 5, 20, true, "", "");
        #region fam
        Create_Icon("29001", "Asset Master", "", 2, "fa fa-bank", "nav child_menu", true, "fam", "fixmain", "fixmain", 5, 88, true);
        Create_Icon("29001.1", "FA Major Head", "../Fam/fa_major_head", 3, "", "", false, "-", "fam", "fixmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("29001.2", "FA Minor Head", "../Fam/fa_m", 3, "", "", false, "-", "fam", "fixmain", 5, Convert.ToDecimal(88.01), true);

        //Create_Icon("29001.3", "Location Master", "", 2, "fa fa-bank", "nav child_menu", true, "fab", "fixmain", "fixmain", 5, 88, true);
        //Create_Icon("29001.3.1", "Building Name", "../Mst/master_ctrl", 3, "", "", false, "", "fab", "fixmain", 5, 50, true);
        //Create_Icon("29001.3.2", "Floor Name", "../Mst/cascade_mst", 3, "", "", false, "", "fab", "fixmain", 5, 50, true);
        //Create_Icon("29001.3.3", "Room Name", "../Mst/cascade_mst", 3, "", "", false, "", "fab", "fixmain", 5, 50, true);
        //Create_Icon("29001.3.4", "Rack Name", "../Mst/cascade_mst", 3, "", "", false, "", "fab", "fixmain", 5, 50, true);
        //Create_Icon("29001.3.5", "Bin Name", "../Mst/cascade_mst", 3, "", "", false, "", "fab", "fixmain", 5, 50, true);
        Create_Icon("17001.15.1", "Multi Location", "", 3, "", "nav child_menu", true, "mloc", "fam", "fixmain", 5, 50, true);


        Create_Icon("17001.15", "Building Name", "../Mst/master_ctrl", 4, "", "", false, "-", "mloc", "fixmain", 5, 50, true);
        Create_Icon("17001.16", "Floor Name", "../Mst/cascade_mst", 4, "", "", false, "-", "mloc", "fixmain", 5, 50, true);
        Create_Icon("17001.17", "Room Name", "../Mst/cascade_mst", 4, "", "", false, "-", "mloc", "fixmain", 5, 50, true);
        Create_Icon("17001.18", "Rack Name", "../Mst/cascade_mst", 4, "", "", false, "-", "mloc", "fixmain", 5, 50, true);
        Create_Icon("17001.19", "Bin Name", "../Mst/cascade_mst", 4, "", "", false, "-", "mloc", "fixmain", 5, 50, true);

        Create_Icon("17001.23", "Single Location", "../Mst/master_ctrl", 3, "", "", false, "", "fsg", "fixmain", 5, 50, true);




        //Create_Icon("29001.3", "Location Master", "../Fam/fam_master", 3, "", "", false, "-", "fam", "fixmain", 5, Convert.ToDecimal(88.01), true);
        //Create_Icon("29001.4", "Building/Unit", "", 3, "", "nav child_menu", true, "fab", "fam", "fixmain", 5, Convert.ToDecimal(88.01), true);
        //Create_Icon("29001.4.1", "Building/Unit", "", 4, "", "", false, "-", "fab", "fixmain", 5, Convert.ToDecimal(88.01), true);
        //Create_Icon("29001.4.2", "Block/Wing", "", 4, "", "", false, "-", "fab", "fixmain", 5, Convert.ToDecimal(88.01), true);




        Create_Icon("29001.4.3", "Area", "", 4, "", "", false, "-", "fab", "fixmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("29001.5", "Cost Center", "../Fam/cost_center", 3, "", "", false, "-", "fam", "fixmain", 5, Convert.ToDecimal(88.01), true);
        //Create_Icon("29001.6", "Currency Master", "../Mst/currency_master", 3, "", "", false, "-", "fam", "fixmain", 5, Convert.ToDecimal(88.01), true);
        //Create_Icon("29001.7", "Currency Rate", "../Fam/currency_rate", 3, "", "", false, "-", "fam", "fixmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("29001.8", "Asset Code Pattern", "../Education/pattern_gen", 3, "", "", false, "-", "fam", "fixmain", 5, Convert.ToDecimal(88.01), true);

        Create_Icon("29002", "Subsidy/Grant", "", 2, "fa fa-umbrella", "nav child_menu", true, "fsg", "fixmain", "fixmain", 5, 88, true);
        Create_Icon("29002.1", "Grant Receipt", "", 3, "", "", false, "-", "fsg", "fixmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("29002.2", "Grant Report", "", 3, "", "", false, "-", "fsg", "fixmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("29002.3", "EPCG", "../Fam/epcg_mst", 3, "", "", false, "-", "fsg", "fixmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("29002.4", "Govt. Grant Master", "../Fam/goverment_grant", 3, "", "", false, "-", "fsg", "fixmain", 5, Convert.ToDecimal(88.01), true);

        Create_Icon("29003", "Purchase Entry", "", 2, "fa fa-sign-in", "nav child_menu", true, "fpe", "fixmain", "fixmain", 5, 88, true);
        Create_Icon("29003.1", "Asset Entry", "../Fam/asset_ent", 3, "", "", false, "-", "fpe", "fixmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("29003.2", "Transfer Of Asset", "../Fam/transfer_asset", 3, "", "", true, "wb", "fpe", "fixmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("29003.2.1", "With Bill", "../../../../../erp/fam/transfer_of_asset", 4, "", "", false, "-", "wb", "fixmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("29003.2.2", "Without Bill", "../../../../../erp/fam/transfer_of_asset", 4, "", "", false, "-", "wb", "fixmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("29003.3", "Revaluation Of Asset", "", 3, "", "", false, "-", "fpe", "fixmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("29003.4", "Amc Entry", "../Fam/amc_ent", 3, "", "", false, "-", "fpe", "fixmain", 5, Convert.ToDecimal(88.01), true);

        Create_Icon("29004", "Sales/Dispose", "", 2, "fa fa-sign-out", "nav child_menu", true, "fsd", "fixmain", "fixmain", 5, 88, true);
        Create_Icon("29004.1", "Asset Sale", "../Fam/asset_sale", 3, "", "", false, "-", "fsd", "fixmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("29004.2", "Asset Write-off", "../Fam/asset_write", 3, "", "", false, "-", "fsd", "fixmain", 5, Convert.ToDecimal(88.01), true);

        Create_Icon("29005", "Physical Verification", "", 2, "fa fa-link", "nav child_menu", true, "fpv", "fixmain", "fixmain", 5, 88, true);
        Create_Icon("29005.1", "PV Statement", "", 3, "", "", false, "-", "fpv", "fixmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("29005.2", "PV Adjustment", "", 3, "", "", false, "-", "fpv", "fixmain", 5, Convert.ToDecimal(88.01), true);

        Create_Icon("29006", "Report", "", 2, "fa fa-briefcase", "nav child_menu", true, "far", "fixmain", "fixmain", 5, 88, true);
        Create_Icon("29006.1", "AMC Due", "", 3, "", "", false, "-", "far", "fixmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("29006.2", "Insurance Due", "", 3, "", "", false, "-", "far", "fixmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("29006.2", "Calibration Due", "", 3, "", "", false, "-", "far", "fixmain", 5, Convert.ToDecimal(88.01), true);

        Create_Icon("29007", "Insurance", "", 2, "fa fa-briefcase", "nav child_menu", true, "fin", "fixmain", "fixmain", 5, 88, true);
        Create_Icon("29007.1", "Ins Rev Entry", "../Fam/ins_rev", 3, "", "", false, "-", "fin", "fixmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("29007.2", "Ins Rev Report", "", 3, "", "", false, "-", "fin", "fixmain", 5, Convert.ToDecimal(88.01), true);

        Create_Icon("29008", "Depreciation", "", 2, "fa fa-briefcase", "nav child_menu", true, "fdep", "fixmain", "fixmain", 5, 88, true);
        Create_Icon("29008.1", "Dep Com Act", "../Fam/dep", 3, "", "", false, "-", "fdep", "fixmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("29008.2", "Dep I Tax Act", "../Fam/dep", 3, "", "", false, "-", "fdep", "fixmain", 5, Convert.ToDecimal(88.01), true);

        Create_Icon("28005", "Vendor Master", "", 2, "fa fa-user", "nav child_menu", true, "fvdm", "fixmain", "fixmain", 5, 47, true);
        Create_Icon("28005.1", "Vendor Detail", "../Purchase/vendor_detail", 3, "", "", false, "", "fvdm", "fixmain", 5, 50, true);
        Create_Icon("28005.2", "Vendor Unit Detail", "../../../../../erp/client_assignment/add_client_unit", 3, "", "", false, "", "fvdm", "fixmain", 5, 50, true);
        Create_Icon("28005.3", "Party Type", "../Mst/master_ctrl", 3, "", "", false, "", "fvdm", "fixmain", 5, 50, true);
        Create_Icon("28005.4", "Bank Acct Type", "../Mst/master_ctrl", 3, "", "", false, "", "fvdm", "fixmain", 5, 50, true);
        Create_Icon("28005.5", "Currency Type", "../Mst/master_ctrl", 3, "", "", false, "", "fvdm", "fixmain", 5, 50, true);
        Create_Icon("28005.6", "Bank Name", "../Mst/master_ctrl", 3, "", "", false, "", "fvdm", "fixmain", 5, 50, true);

        #endregion

        Create_Icon("30000", "Online Test", "../../../../erp/dashboard", 1, "", "", true, "", "ontmain", "ontmain", 5, 20, true, "", "");
        //Create_Icon("31000", "ESS", "../../../../erp/dashboard", 1, "", "", true, "", "essmain", "essmain", 5, 20, true, "", "");
        //#region Ess
        //Create_Icon("31001", "Help Desk", "", 2, "fa fa-bank", "", false, "", "hd", "essmain", 5, 88, true);
        //#endregion
        Create_Icon("32000", "Feedback Portal", "../../../../erp/dashboard", 1, "", "", true, "", "fpmain", "fpmain", 5, 20, true, "", "");

        Create_Icon("33000", "Human Resource", "../../../../erp/dashboard", 1, "", "", true, "", "hrmain", "hrmain", 5, 20, true, "", "");
        #region
        Create_Icon("33001", "Letters To Employee", "", 2, "fa fa-gears", "", true, "hrlte", "hrmain", "hrmain", 5, 9, true);
        Create_Icon("33001.1", "Header Template", "../../../../erp/SchoolReports_Forms/header_designer", 3, "", "", false, "", "hrlte", "hrmain", 5, 50, true);
        Create_Icon("33001.2", "Body Template", "./../../../erp/SchoolReports_Forms/template_master", 3, "", "", false, "", "hrlte", "hrmain", 5, 50, true);
        Create_Icon("33001.3", "Create Certificate", "../../../../erp/SchoolReports_Forms/pic_template", 3, "", "", false, "", "hrlte", "hrmain", 5, 50, true);
        Create_Icon("33001.4", "Letter Templete", "../hall/letter_tmp", 3, "", "", false, "", "hrlte", "hrmain", 5, 50, true);
        Create_Icon("33001.5", "Print Letter", "../hall/prnt_ltr", 3, "", "", false, "", "hrlte", "hrmain", 5, 50, true);

        Create_Icon("33002", "HR Master", "", 2, "fa fa-gears", "", true, "hrmst", "hrmain", "hrmain", 5, 9, true);
        Create_Icon("33002.2", "'Confirmation', ", "../hr/wf_confirm", 3, "", "", false, "", "hrmst", "hrmain", 5, 50, true);
        Create_Icon("33002.3", "'Separation', ", "../hr/sprn", 3, "", "", false, "", "hrmst", "hrmain", 5, 50, true);     
        Create_Icon("33002.5", "Expected Completion Time", "../Mst/master_ctrl", 3, "", "", false, "", "hrmst", "hrmain", 5, 50, true);
        Create_Icon("33002.6", "Area Of Improvement", "../Mst/master_ctrl", 3, "", "", false, "", "hrmst", "hrmain", 5, 50, true);
        Create_Icon("33002.8", "Loan Type", "../hr/Loan_type", 3, "", "", false, "", "hrmst", "hrmain", 5, 50, true);
        Create_Icon("33002.9", "Asset Master", "../hr/asst_mst", 3, "", "", false, "", "hrmst", "hrmain", 5, 50, true);
        Create_Icon("33002.10", "Goal Master", "../Mst/master_ctrl", 3, "", "", false, "", "hrmst", "hrmain", 5, 50, true);

        Create_Icon("33003", "Recruitment", "", 2, "fa fa-gears", "", true, "recmst", "hrmain", "hrmain", 5, 9, true);
        Create_Icon("33003.1", "Open Position", "../hr/open_p", 3, "", "", false, "", "recmst", "hrmain", 5, 50, true);
        Create_Icon("33003.2", "Add Candidate", "../hr/employee_details", 3, "", "", false, "", "recmst", "hrmain", 5, 50, true);
        Create_Icon("33003.3", "Interview", "../hr/intr", 3, "", "", false, "", "recmst", "hrmain", 5, 50, true);
        Create_Icon("33003.4", "Approved Open Position", "../hr/apprd_pos", 3, "", "", false, "", "recmst", "hrmain", 5, 50, true);
        Create_Icon("33003.5", "Interview Schedule", "../hr/intrw_sch", 3, "", "", false, "", "recmst", "hrmain", 5, 50, true);
        Create_Icon("33003.6", "Final Selection", "../hr/final_selc", 3, "", "", false, "", "recmst", "hrmain", 5, 50, true);       

        Create_Icon("33004", "Mediclaim", "", 2, "fa fa-gears", "", true, "mdmst", "hrmain", "hrmain", 5, 9, true);
        Create_Icon("33004.1", "Policy Detail", "../hr/pol_dtl", 3, "", "", false, "", "mdmst", "hrmain", 5, 50, true);
        Create_Icon("33004.2", "List Of Covered Emp", "../hr/memp_cov", 3, "", "", false, "", "mdmst", "hrmain", 5, 50, true);

        Create_Icon("33005", "Site Employee", "", 2, "fa fa-user", "", true, "semp", "hrmain", "hrmain", 5, 9, true);
        Create_Icon("33005.1", "Site Location", "../hall/s_loc", 3, "", "", false, "", "semp", "hrmain", 5, 100, true);
        Create_Icon("33005.2", "Site Employee Linking", "../hall/site_link", 3, "", "", false, "", "semp", "hrmain", 5, 100, true);
        
        Create_Icon("33006", "Asset To Employee", "", 2, "fa fa-user", "", true, "astemp", "hrmain", "hrmain", 5, 9, true);
        Create_Icon("33006.1", "Asset Allocate", "../hall/asst_lac", 3, "", "", false, "", "astemp", "hrmain", 5, 100, true);
        Create_Icon("33006.2", "Asset Return Back", "../hall/asst_return", 3, "", "", false, "", "astemp", "hrmain", 5, 100, true);
        Create_Icon("33006.3", "Assets Write Off", "../hall/awf", 3, "", "", false, "", "astemp", "hrmain", 5, 100, true);
        Create_Icon("33006.4", "List Of Assets", "../hall/ast_list", 3, "", "", false, "", "astemp", "hrmain", 5, 100, true);
        Create_Icon("33006.5", "Reports", "../hall/hr_report", 3, "", "", false, "", "astemp", "hrmain", 5, 100, true);

        Create_Icon("33007", "Communication", "", 2, "fa fa-user", "", true, "comm", "hrmain", "hrmain", 5, 9, true);
        Create_Icon("33007.1", "General Message", "../hr/gen_msg", 3, "", "", false, "", "comm", "hrmain", 5, 100, true);
        Create_Icon("33007.2", "Good Wishes", "../hr/good_wishes", 3, "", "", false, "", "comm", "hrmain", 5, 100, true);
        Create_Icon("33007.3", "Communication Master", "../Hr/commst", 3, "", "", false, "", "comm", "hrmain", 5, 100, true);
        Create_Icon("33007.4", "Employee Directory", "../Hr/empdir", 3, "", "", false, "", "comm", "hrmain", 5, 100, true);

        Create_Icon("33008", "Attachments", "", 2, "fa fa-user", "", true, "hrpolc", "hrmain", "hrmain", 5, 9, true);
        Create_Icon("33008.1", "Employee Expiry Document", "../hall/hr_empdoc", 3, "", "", false, "", "hrpolc", "hrmain", 5, 50, true);
        Create_Icon("33008.2", "Hr Policy", "../vastu/comp_doc", 3, "", "", false, "", "hrpolc", "hrmain", 5, 50, true);
        Create_Icon("33008.3", "List (Emp Expiry Doc)", "../hall/doc_list", 3, "", "", false, "", "hrpolc", "hrmain", 5, 50, true);
        Create_Icon("33008.4", "List (HR Policy)", "../hall/doc_list", 3, "", "", false, "", "hrpolc", "hrmain", 5, 50, true);

        Create_Icon("33009", "Overview", "../Hr/overview", 2, "fa fa-gears", "", false, "", "ovr", "hrmain", 5, 9, true);

        Create_Icon("330010", "Workflow", "", 2, "fa fa-user", "", true, "wflow", "hrmain", "hrmain", 5, 9, true);
        Create_Icon("330010.1", "Workflow Delegates", "../hall/delgts", 3, "", "", false, "", "wflow", "hrmain", 5, 50, true);
        Create_Icon("33002.7", "Role Master", "../hr/role_mt", 3, "", "", false, "", "wflow", "hrmain", 5, 50, true);
        Create_Icon("33002.1", "Work Flow", "../hr/w_flow", 3, "", "", false, "", "wflow", "hrmain", 5, 50, true);

        Create_Icon("330011", "Reporting Structure", "../Hr/team", 2, "fa fa-bank", "", false, "rptstr", "hrmain", "hrmain", 5, 88, true);

        Create_Icon("330012", "Application", "", 2, "fa fa-bank", "", false, "aplmain", "hrmain", "hrmain", 5, 88, true);
        Create_Icon("330012.1", "Leave Applied", "../Education/leaveapplication", 3, "", "", false, "", "aplmain", "hrmain", 5, 110, true);
        Create_Icon("330012.2", "Loans/Advance", "../Hr/loan_appl", 3, "", "", false, "", "aplmain", "hrmain", 5, 110, true);
        Create_Icon("330012.3", "OD Slip", "../Hr/od_slip", 3, "", "", false, "", "aplmain", "hrmain", 5, 110, true);
        Create_Icon("330012.4", "Separation", "../Hr/separt", 3, "", "", false, "", "aplmain", "hrmain", 5, 110, true);

        Create_Icon("330013", "Goal Setting", "../Hr/g_setting", 2, "fa fa-bank", "", false, "glset", "hrmain", "hrmain", 5, 88, true);
        Create_Icon("330014", "Goal Assign", "../Hr/g_assign", 2, "fa fa-bank", "", false, "glasn", "hrmain", "hrmain", 5, 88, true);


        #endregion
        Create_Icon("34000", "Compliance Management", "../../../../erp/dashboard", 1, "", "", true, "", "cmmain", "cmmain", 5, 20, true, "", "");

        Create_Icon("35000", "Financial Advisory Services", "../../../../erp/dashboard", 1, "", "", true, "", "tmmain", "tmmain", 5, 20, true, "", "");
        #region
        Create_Icon("35001", "Advisory Master", "", 2, "fa fa-gears", "nav child_menu", true, "admst", "tmmain", "tmmain", 5, 9, true);
        Create_Icon("28005.3", "Organisation Status", "../Mst/master_ctrl", 3, "", "", false, "", "admst", "tmmain", 5, 50, true);
        Create_Icon("28005.4", "Bank Acct Type", "../Mst/master_ctrl", 3, "", "", false, "", "admst", "tmmain", 5, 50, true);
        Create_Icon("28005.5", "Currency Type", "../Mst/master_ctrl", 3, "", "", false, "", "admst", "tmmain", 5, 50, true);
        Create_Icon("28005.6", "Bank Name", "../Mst/master_ctrl", 3, "", "", false, "", "admst", "tmmain", 5, 50, true);
        Create_Icon("28005.7", "Product Category Master", "../Mst/master_ctrl", 3, "", "", false, "", "admst", "tmmain", 5, 100, true);
        Create_Icon("40001.6", "Sales/Service Area", "../Mst/master_ctrl", 3, "", "", false, "", "admst", "tmmain", 5, 50, true);
        Create_Icon("40001.1", "Property Master", "../Mst/master_ctrl", 3, "", "", false, "", "admst", "tmmain", 5, 50, true);
        Create_Icon("40001.5", "Occupation Master", "../Mst/master_ctrl", 3, "", "", false, "", "admst", "tmmain", 5, 50, true);
        Create_Icon("40001.3", "Type Of Account", "../Mst/master_ctrl", 3, "", "", false, "", "admst", "tmmain", 5, 50, true);
        Create_Icon("40001.8", "Type Of Address", "../Mst/master_ctrl", 3, "", "", false, "", "admst", "tmmain", 5, 50, true);
        Create_Icon("12003.5", "Lead Source Master", "../Mst/master_ctrl", 3, "", "", false, "-", "admst", "tmmain", 5, 110, true);
        Create_Icon("40001.4", "Business Type", "../Mst/master_ctrl", 3, "", "", false, "", "admst", "tmmain", 5, 50, true);
        Create_Icon("12003.3", "Product Category Master", "../Mst/master_ctrl", 3, "", "", false, "-", "admst", "tmmain", 5, 110, true);
        Create_Icon("21001.7", "Payment Term Master", "../Mst/master_ctrl", 3, "", "", false, "-", "admst", "tmmain", 5, Convert.ToDecimal(88.03), true);
        Create_Icon("40001.11", "Sub Broker", "../Mst/master_ctrl", 3, "", "", false, "", "admst", "tmmain", 5, 50, true);
        Create_Icon("40001.10", "Client Rating", "../Mst/master_ctrl", 3, "", "", false, "", "admst", "tmmain", 5, 50, true);
        Create_Icon("40001.13", "Credit Rating", "../Mst/master_ctrl", 3, "", "", false, "", "admst", "tmmain", 5, 50, true);
        Create_Icon("7004.1.7", "Qualification", "../Mst/master_ctrl", 3, "", "", false, "", "admst", "tmmain", 5, 50, true);
        Create_Icon("27008", "Annual Income Master", "../Mst/master_ctrl", 2, "fa fa-bank", "", false, "", "admst", "tmmain", 5, 88, true);
        Create_Icon("35001.1", "Scheme Type", "../Mst/master_ctrl", 3, "", "", false, "", "admst", "tmmain", 5, 50, true);
        Create_Icon("35001.2", "Scheme Name", "../Mst/cascade_ddl", 3, "", "", false, "", "admst", "tmmain", 5, 50, true);
        Create_Icon("35001.3", "Switch Type", "../Mst/master_ctrl", 3, "", "", false, "", "admst", "tmmain", 5, 50, true);
        Create_Icon("35001.4", "Form Control", "../Mst/form_ctrl", 3, "", "", false, "", "admst", "tmmain", 5, 50, true);
        Create_Icon("35001.5", "Fund Master", "../Mst/master_ctrl", 3, "", "", false, "", "admst", "tmmain", 5, 50, true);
        Create_Icon("35001.6", "Transaction Type", "../Mst/master_ctrl", 3, "", "", false, "", "admst", "tmmain", 5, 50, true);
        Create_Icon("35001.7", "Redemption Type", "../Mst/master_ctrl", 3, "", "", false, "", "admst", "tmmain", 5, 50, true);
        Create_Icon("35001.8", "Policy Type", "../Mst/master_ctrl", 3, "", "", false, "", "admst", "tmmain", 5, 50, true);
        Create_Icon("35001.9", "Policy Sub Type", "../Mst/master_ctrl", 3, "", "", false, "", "admst", "tmmain", 5, 50, true);
        Create_Icon("35001.10", "Broker", "../Mst/master_ctrl", 3, "", "", false, "", "admst", "tmmain", 5, 50, true);
        Create_Icon("35001.11", "Channel Master", "../Mst/master_ctrl", 3, "", "", false, "", "admst", "tmmain", 5, 50, true);
        Create_Icon("35001.12", "Mail Master", "../vastu/mail_mst", 3, "", "", false, "", "admst", "tmmain", 5, 50, true);
        Create_Icon("35001.13", "Frequency Master", "../mst/freq_mst", 3, "", "", false, "", "admst", "tmmain", 5, 100, true);

        Create_Icon("35002", "Transaction", "", 2, "fa fa-gears", "nav child_menu", true, "trans", "tmmain", "tmmain", 5, 9, true);
        Create_Icon("35002.1", "Transaction Request", "../Vastu/trans_req", 3, "", "", false, "", "trans", "tmmain", 5, 50, true);
        Create_Icon("35002.2", "Transaction Report", "../Vastu/trans_rpt", 3, "", "", false, "", "trans", "tmmain", 5, 50, true);
        Create_Icon("35002.3", "Non Fin Trans Req", "../Vastu/n_finreq", 3, "", "", false, "", "trans", "tmmain", 5, 50, true);
        Create_Icon("35002.4", "Purchase Request", "../Vastu/trans_req", 3, "", "", false, "", "trans", "tmmain", 5, 50, true);
        Create_Icon("35002.5", "Redemption Request", "../Vastu/trans_req", 3, "", "", false, "", "trans", "tmmain", 5, 50, true);
        Create_Icon("35002.6", "Switch Request", "../Vastu/trans_req", 3, "", "", false, "", "trans", "tmmain", 5, 50, true);
        Create_Icon("35002.7", "Insurance", "../Vastu/fas_ins", 3, "", "", false, "", "trans", "tmmain", 5, 50, true);
        Create_Icon("35002.8", "P2P Lending", "../Vastu/fas_p2p", 3, "", "", false, "", "trans", "tmmain", 5, 50, true);
        Create_Icon("35002.9", "Fixed Deposit", "../Vastu/fas_fix", 3, "", "", false, "", "trans", "tmmain", 5, 50, true);
        Create_Icon("35002.10", "FD Redemption", "../Vastu/fas_fd", 3, "", "", false, "", "trans", "tmmain", 5, 50, true);

        Create_Icon("35003", "Accounts", "", 2, "fa fa-gears", "nav child_menu", true, "Facc", "tmmain", "tmmain", 5, 9, true);
        Create_Icon("40002.1", "New Account", "../Purchase/party", 3, "", "", false, "", "Facc", "tmmain", 5, 50, true);
        Create_Icon("40002.6", "List of Accounts", "", 3, "", "", false, "", "Facc", "tmmain", 5, Convert.ToDecimal(74.04), true, "onclick=$menuclick(this);$");

        Create_Icon("35004", "Product Master", "", 2, "fa fa-gears", "nav child_menu", true, "pdmst", "tmmain", "tmmain", 5, 9, true);
        Create_Icon("35004.1", "Product Name Master", "../Vastu/pn_unit", 3, "", "", false, "", "pdmst", "tmmain", 5, 50, true);
        Create_Icon("35004.2", "Prd Assign To Client", "../Vastu/p_a_client", 3, "", "", false, "", "pdmst", "tmmain", 5, 50, true);
        Create_Icon("35004.3", "Manufacturer", "../Purchase/party", 3, "", "", false, "", "pdmst", "tmmain", 5, 50, true);
        Create_Icon("35004.4", "Purchase Through", "../Mst/master_ctrl", 3, "", "", false, "", "pdmst", "tmmain", 5, 50, true);
        Create_Icon("35004.5", "Reminder Config", "../hr/remind_config", 3, "", "", false, "", "pdmst", "tmmain", 5, 50, true);
        //Create_Icon("35004.6", "Reminder Config new", "../mst/freq_mst", 3, "", "", false, "", "pdmst", "tmmain", 5, 50, true);
        #endregion
        Create_Icon("36000", "Event Management", "../../../../erp/dashboard", 1, "", "", true, "", "emmain", "emmain", 5, 20, true, "", "");
        Create_Icon("37000", "Day Care", "../../../../erp/dashboard", 1, "", "", true, "", "dcmain", "dcmain", 5, 20, true, "", "");
        #region
        Create_Icon("37001", "Day Care Master", "", 2, "fa fa-gears", "nav child_menu", true, "dcm", "dc", "dcmain", 5, 9, true);
        Create_Icon("37001.1", "Fees", "../Education/DC_Fee", 3, "", "", false, "", "dcm", "dcmain", 5, 50, true);
        Create_Icon("37001.2", "Activity", "../Education/DC_Activity", 3, "", "", false, "", "dcm", "dcmain", 5, 50, true);

        Create_Icon("37002", "Activity Updates", "../Education/DC_Act_Upd", 2, "fa fa-gears", "", false, "", "dc", "dcmain", 5, 9, true);
        Create_Icon("37003", "Day Care Students", "../Education/DC_Students", 2, "fa fa-gears", "", false, "", "dc", "dcmain", 5, 9, true);
        #endregion
        Create_Icon("38000", "Plant Maintenance", "../../../../erp/dashboard", 1, "", "", true, "", "pmmain", "pmmain", 5, 20, true, "", "");
        #region
        Create_Icon("38001", "Maintenance Master", "", 2, "fa fa-gears", "", true, "mainm", "pmmain", "pmmain", 5, 9, true);
        Create_Icon("38001.1", "Machine Master", "../Production/machine_master", 3, "", "", false, "", "mainm", "pmmain", 5, 50, true);
        Create_Icon("38001.2", "Mould Master", "../Production/mould_master", 3, "", "", false, "", "mainm", "pmmain", 5, 50, true);
        Create_Icon("38001.3", "Machine Breakdown", "../Production/machine_brkdwn", 3, "", "", false, "", "mainm", "pmmain", 5, 50, true);
        Create_Icon("38001.4", "Operator Name", "../Production/operator_master", 3, "", "", false, "", "mainm", "pmmain", 5, 50, true);
        Create_Icon("2003.4", "Frequency Master", "../Mst/master_ctrl", 3, "", "", false, "", "mainm", "pmmain", 5, 50, true);
        Create_Icon("17001.23", "Single Location", "../Mst/master_ctrl", 3, "", "", false, "", "mainm", "pmmain", 5, 50, true);
        Create_Icon("38001.5", "Doc Template", "../Education/rpt_temp", 3, "", "", false, "", "mainm", "pmmain", 5, Convert.ToDecimal(85.02), true);

        Create_Icon("38002", "Indent", "", 2, "fa fa-bank", "nav child_menu", true, "ind", "pmmain", "pmmain", 5, 88, true);
        Create_Icon("38002.1", "Create Indent", "../Purchase/indent_req", 3, "", "", false, "-", "ind", "pmmain", 5, Convert.ToDecimal(88.01), true);
        #endregion
        Create_Icon("39000", "Production", "../../../../erp/dashboard", 1, "", "", true, "", "pdmain", "pdmain", 5, 20, true, "", "");
        #region
        Create_Icon("39001", "Production Master", "", 2, "fa fa-gears", "nav child_menu", true, "pdmas", "pdmain", "pdmain", 5, 9, true);
        Create_Icon("39001.1", "Machine Master", "../Production/machine_master", 3, "", "", false, "", "pdmas", "pdmain", 5, 50, true);
        //   Create_Icon("39001.2", "Production Stage", "../Production/prod_stage", 3, "", "", false, "", "pdmas", "pdmain", 5, 50, true);
        Create_Icon("39001.3", "OH Master", "../Production/oh_mst", 3, "", "", false, "", "pdmas", "pdmain", 5, 50, true);
        Create_Icon("39001.4", "Shift Master", "../Mst/master_ctrl", 3, "", "", false, "", "pdmas", "pdmain", 5, 50, true);
        Create_Icon("39001.5", "Mould Master", "../Production/mould_master", 3, "", "", false, "", "pdmas", "pdmain", 5, 50, true);
        Create_Icon("39001.6", "Machine Breakdown", "../Production/machine_brkdwn", 3, "", "", false, "", "pdmas", "pdmain", 5, 50, true);
        Create_Icon("39001.7", "Operator Name", "../Production/operator_master", 3, "", "", false, "", "pdmas", "pdmain", 5, 50, true);
        Create_Icon("39001.8", "Breakdown Reason", "../Mst/master_ctrl", 3, "", "", false, "", "pdmas", "pdmain", 5, 50, true);
        Create_Icon("39001.9", "Stage Master", "../Mst/master_ctrl", 3, "", "", false, "", "pdmas", "pdmain", 5, 50, true);
        Create_Icon("39001.10", "OH Type Master", "../Mst/master_ctrl", 3, "", "", false, "", "pdmas", "pdmain", 5, 50, true);
        Create_Icon("39001.11", "BOM", "../Production/bom", 3, "", "", false, "", "pdmas", "pdmain", 5, 50, true);
        Create_Icon("39001.12", "MRP", "../Production/mrp_gen", 3, "", "", false, "", "pdmas", "pdmain", 5, 50, true);
        Create_Icon("39001.13", "Rejection Reason", "../Mst/master_ctrl", 3, "", "", false, "", "pdmas", "pdmain", 5, 50, true);
        Create_Icon("39001.14", "Doc Template", "../Education/rpt_temp", 3, "", "", false, "", "pdmas", "pdmain", 5, Convert.ToDecimal(85.02), true);
        Create_Icon("39001.15", "PDI Rejection Reason", "../Mst/master_ctrl", 3, "", "", false, "", "pdmas", "pdmain", 5, Convert.ToDecimal(85.02), true);
        Create_Icon("39001.16", "Machine Capacity Master", "../Mst/master_ctrl", 3, "", "", false, "", "pdmas", "pdmain", 5, Convert.ToDecimal(85.02), true);
        Create_Icon("39001.17", "Form Control", "../Mst/form_ctrl", 3, "", "", false, "", "pdmas", "pdmain", 5, 50, true);
        Create_Icon("39001.18", "Mould Wise Item", "../Production/mlditm", 3, "", "", false, "", "pdmas", "pdmain", 5, 50, true);
        Create_Icon("2003.4", "Frequency", "../Mst/master_ctrl", 3, "", "", false, "", "pdmas", "pdmain", 5, Convert.ToDecimal(47.01), true);
        Create_Icon("39001.19", "Type Master", "../Mst/doc_master", 3, "", "", false, "", "pdmas", "pdmain", 5, 50, true);

        Create_Icon("39002", "Preventive Maintenance", "../Production/preventive_maint", 2, "fa fa-gears", "", false, "", "pdmain", "pdmain", 5, 9, true);
        Create_Icon("39003", "Breakdown Entry", "../Production/preventive_maint", 2, "fa fa-gears", "", false, "", "pdmain", "pdmain", 5, 9, true);
        Create_Icon("39004", "Breakdown Close", "../Production/preventive_maint", 2, "fa fa-gears", "", false, "", "pdmain", "pdmain", 5, 9, true);

        Create_Icon("39005", "Production", "", 2, "fa fa-gears", "nav child_menu", true, "prd", "pdmain", "pdmain", 5, 9, true);
        Create_Icon("39005.1", "Production Entry", "../Production/prod_entry", 3, "", "", false, "", "prd", "pdmain", 5, 50, true);
        Create_Icon("39005.2", "Daily Production Entry", "../Inventory/dpr", 3, "", "", false, "", "prd", "pdmain", 5, 50, true);
        Create_Icon("39005.3", "PD Entry New", "../Production/iprod", 3, "", "", false, "", "prd", "pdmain", 5, 50, true);
        Create_Icon("39005.4", "Redbin Analysis", "../Production/irej", 3, "", "", false, "", "prd", "pdmain", 5, 50, true);
        Create_Icon("39005.5", "PDI", "../Inventory/pdi", 3, "", "", false, "", "prd", "pdmain", 5, 50, true);
        Create_Icon("39005.6", "Rework", "../Production/rewrk", 3, "", "", false, "", "prd", "pdmain", 5, 50, true);
        Create_Icon("39005.7", "Pd Reverse", "../Production/pdrevrse", 3, "", "", false, "", "prd", "pdmain", 5, 50, true);
        Create_Icon("39005.8", "Stage Transfer", "../Production/stgtransfer", 3, "", "", false, "", "prd", "pdmain", 5, 50, true);
        Create_Icon("39005.9", "PD Entry", "../Production/iprodn", 3, "", "", false, "", "prd", "pdmain", 5, 50, true);
        Create_Icon("39005.10", "PDI New", "../Inventory/pdin", 3, "", "", false, "", "prd", "pdmain", 5, 50, true);

        Create_Icon("39007", "Reports", "-", 2, "fa fa-print", "nav child_menu", true, "pdreps", "pdmain", "pdmain", 5, 88, true);
        //Create_Icon("39007.1", "Production Reports", "../Production/pdreps", 3, "-", "-", false, "-", "pdreps", "pdmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("39007.1", "Production Reports", "../Production/pdreps", 3, "-", "-", false, "-", "pdreps", "pdmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("39007.2", "Production Reports New", "../Production/pdreps2", 3, "-", "-", false, "-", "pdreps", "pdmain", 5, Convert.ToDecimal(88.01), true);

        Create_Icon("39006", "Indent", "", 2, "fa fa-bank", "nav child_menu", true, "ind", "pdmain", "pdmain", 5, 88, true);
        Create_Icon("39006.1", "Create Indent", "../Purchase/indent_req", 3, "", "", false, "-", "ind", "pdmain", 5, Convert.ToDecimal(88.01), true);

        Create_Icon("39008", "Production Planning", "", 2, "fa fa-gears", "nav child_menu", true, "pp", "pdmain", "pdmain", 5, 9, true);
        Create_Icon("39008.1", "Production Order", "../Production/p_ord", 3, "", "", false, "", "pp", "pdmain", 5, 50, true);
        Create_Icon("39008.2", "Close Pending Prd Order", "../Inventory/p_close", 3, "", "", false, "", "pp", "pdmain", 5, 50, true);

        #endregion        
        Create_Icon("40000", "CRM (Vastu)", "../../../../erp/dashboard", 1, "", "", true, "", "crmvmain", "crmvmain", 5, 20, true, "", "");
        #region
        Create_Icon("40001", "CRM Master", "", 2, "fa fa-gears", "nav child_menu", true, "crmv", "crmvmain", "crmvmain", 5, 9, true);
        Create_Icon("40001.1", "Property Master", "../Mst/master_ctrl", 3, "", "", false, "", "crmv", "crmvmain", 5, 50, true);
        Create_Icon("40001.2", "Relation Master", "../Mst/master_ctrl", 3, "", "", false, "", "crmv", "crmvmain", 5, 50, true);
        // Create_Icon("40001.3", "Client Type Master", "../Mst/master_ctrl", 3, "", "", false, "", "crmv", "crmvmain", 5, 50, true);
        Create_Icon("40001.3", "Type Of Account", "../Mst/master_ctrl", 3, "", "", false, "", "crmv", "crmvmain", 5, 50, true);
        //Create_Icon("40001.4", "Company Master", "../Mst/master_ctrl", 3, "", "", false, "", "crmv", "crmvmain", 5, 50, true);
        Create_Icon("40001.4", "Business Type", "../Mst/master_ctrl", 3, "", "", false, "", "crmv", "crmvmain", 5, 50, true);
        Create_Icon("40001.5", "Occupation Master", "../Mst/master_ctrl", 3, "", "", false, "", "crmv", "crmvmain", 5, 50, true);
        Create_Icon("40001.6", "Sales/Service Area", "../Mst/master_ctrl", 3, "", "", false, "", "crmv", "crmvmain", 5, 50, true);
        Create_Icon("40001.7", "Form Control", "../Mst/form_ctrl", 3, "", "", false, "", "crmv", "crmvmain", 5, 50, true);
        Create_Icon("40001.8", "Type Of Address", "../Mst/master_ctrl", 3, "", "", false, "", "crmv", "crmvmain", 5, 50, true);
        //Create_Icon("28005.3", "Party Type", "../Mst/master_ctrl", 3, "", "", false, "", "crmv", "crmvmain", 5, 50, true);
        Create_Icon("28005.3", "Organisation Status", "../Mst/master_ctrl", 3, "", "", false, "", "crmv", "crmvmain", 5, 50, true);
        Create_Icon("40001.9", "Financial Account", "../Mst/master_ctrl", 3, "", "", false, "", "crmv", "crmvmain", 5, 50, true);
        Create_Icon("28004.11", "Payment Term Master", "../Mst/master_ctrl", 3, "", "", false, "-", "crmv", "crmvmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("40001.10", "Client Rating", "../Mst/master_ctrl", 3, "", "", false, "", "crmv", "crmvmain", 5, 50, true);
        Create_Icon("40001.11", "Sub Broker", "../Mst/master_ctrl", 3, "", "", false, "", "crmv", "crmvmain", 5, 50, true);
        Create_Icon("40001.12", "Bulk Upload", "../vastu/bulk_acc", 3, "", "", false, "", "crmv", "crmvmain", 5, 50, true);
        Create_Icon("7004.1.7", "Qualification", "../Mst/master_ctrl", 3, "", "", false, "", "crmv", "crmvmain", 5, 50, true);
        Create_Icon("40001.13", "Credit Rating", "../Mst/master_ctrl", 3, "", "", false, "", "crmv", "crmvmain", 5, 50, true);
        Create_Icon("40001.14", "SMS/MAIL Template", "../Vastu/automail_tmp", 3, "", "", false, "", "crmv", "crmvmain", 5, 50, true);//bd
        Create_Icon("28005.6", "Bank Name", "../Mst/master_ctrl", 3, "", "", false, "", "crmv", "crmvmain", 5, 50, true);
        Create_Icon("28005.4", "Bank Acct Type", "../Mst/master_ctrl", 3, "", "", false, "", "crmv", "crmvmain", 5, 50, true);
        Create_Icon("27008", "Annual Income Master", "../Mst/master_ctrl", 2, "fa fa-bank", "", false, "", "crmv", "crmvmain", 5, 88, true);
        Create_Icon("40001.15", "Venue Master", "../Mst/master_ctrl", 3, "", "", false, "", "crmv", "crmvmain", 5, 50, true);//kc
        Create_Icon("40001.16", "Menu Master", "../Mst/master_ctrl", 3, "", "", false, "", "crmv", "crmvmain", 5, 50, true);//kc
        Create_Icon("40001.17", "Type of Function", "../Mst/master_ctrl", 3, "", "", false, "", "crmv", "crmvmain", 5, 50, true);//kc
        Create_Icon("40001.18", "Vendor Category", "../Mst/master_ctrl", 3, "", "", false, "", "crmv", "crmvmain", 5, 50, true);//kc
        Create_Icon("40001.19", "Mail Template", "../hall/letter_tmp", 3, "", "", false, "", "crmv", "crmvmain", 5, 50, true);//kc
        Create_Icon("40001.20", "Frequency Master", "../Vastu/freqMst", 3, "", "", false, "", "crmv", "crmvmain", 5, 50, true);//kc        

        Create_Icon("40002", "Account Detail", "", 2, "fa fa-users", "nav child_menu", true, "ncust", "crmvmain", "crmvmain", 5, 9, true);
        Create_Icon("40002.1", "New Account", "../Purchase/party", 3, "", "", false, "", "ncust", "crmvmain", 5, 50, true);
        //Create_Icon("40002.2", "Follow Up", "../Vastu/new_follow", 3, "", "", false, "", "ncust", "crmvmain", 5, 50, true);
        Create_Icon("40002.3", "Add Family Detail", "../Vastu/conf_cust", 3, "", "", false, "", "ncust", "crmvmain", 5, 50, true);
        Create_Icon("40002.4", "Account Property", "../Vastu/cust_prop", 3, "", "", false, "", "ncust", "crmvmain", 5, 50, true);
        //Create_Icon("40002.5", "Client Unit", "../Purchase/party_unit", 3, "", "", false, "", "ncust", "crmvmain", 5, 50, true);
        Create_Icon("40002.6", "List of Accounts", "", 3, "", "", false, "", "ncust", "crmvmain", 5, Convert.ToDecimal(74.04), true, "onclick=$menuclick(this);$");
        //Create_Icon("40002.7", "List of Units", "", 3, "", "", false, "", "ncust", "crmvmain", 5, Convert.ToDecimal(74.04), true, "onclick=$menuclick(this);$");
        //Create_Icon("40002.8", "Enquiry", "../Purchase/enq", 3, "", "", false, "", "ncust", "crmvmain", 5, 50, true);
        //Create_Icon("40002.9", "Prospect Data", "../Purchase/party", 3, "", "", false, "", "ncust", "crmvmain", 5, 50, true);
        Create_Icon("40002.9", "Prospect Account", "../Purchase/party", 3, "", "", false, "", "ncust", "crmvmain", 5, 50, true);
        Create_Icon("40002.10", "New Account(Small)", "../vastu/s_party", 3, "", "", false, "", "ncust", "crmvmain", 5, 50, true);
        Create_Icon("40002.11", "Booking", "../vastu/booking", 3, "", "", false, "", "ncust", "crmvmain", 5, 50, true);

        //Create_Icon("40003", "Existing Customer", "", 2, "fa fa-gears", "nav child_menu", true, "ecust", "crmvmain", "crmvmain", 5, 9, true);

        Create_Icon("40004", "Knowledge Bank", "", 2, "fa fa-trophy", "nav child_menu", true, "vastukb", "crmvmain", "crmvmain", 5, 9, true);
        Create_Icon("40004.1", "Mkt Material", "../Vastu/comp_doc", 3, "", "", false, "", "vastukb", "crmvmain", 5, 50, true);
        Create_Icon("40004.2", "Company Doc", "../Vastu/comp_doc", 3, "", "", false, "", "vastukb", "crmvmain", 5, 50, true);
        Create_Icon("40004.3", "AMC Plan", "../Vastu/amc_plan", 3, "", "", false, "", "vastukb", "crmvmain", 5, 50, true);
        Create_Icon("40004.4", "Good Wishes", "../Vastu/good_wishes", 3, "", "", false, "", "vastukb", "crmvmain", 5, 50, true);
        Create_Icon("40004.5", "General Message", "../Vastu/gen_msg", 3, "", "", false, "", "vastukb", "crmvmain", 5, 50, true);

        Create_Icon("40005", "Lead", "", 2, "fa fa-gears", "nav child_menu", true, "vastuld", "crmvmain", "crmvmain", 5, 9, true);
        Create_Icon("40005.1", "Create Lead", "../Vastu/lead", 3, "", "", false, "", "vastuld", "crmvmain", 5, 50, true);
        Create_Icon("40005.2", "List of Leads(User)", "../Vastu/lead_list", 3, "", "", false, "", "vastuld", "crmvmain", 5, 50, true);
        //Create_Icon("40005.3", "User Wise Leads", "../Vastu/lead_list", 3, "", "", false, "", "vastuld", "crmvmain", 5, 50, true);       
        Create_Icon("40005.3", "Lead Reports", "../vastu/crm_reports", 3, "", "", false, "", "vastuld", "crmvmain", 5, 50, true);
        Create_Icon("40005.4", "List of Closed Leads", "../Vastu/lead_list", 3, "", "", false, "", "vastuld", "crmvmain", 5, 50, true);
        Create_Icon("40005.5", "List of Leads(Assigned)", "../Vastu/lead_list", 3, "", "", false, "", "vastuld", "crmvmain", 5, 50, true);
        Create_Icon("40005.6", "List of Leads(Total)", "../Vastu/lead_list", 3, "", "", false, "", "vastuld", "crmvmain", 5, 50, true);
        Create_Icon("40005.7", "Lead Visit", "../Vastu/ld_visit", 3, "", "", false, "", "vastuld", "crmvmain", 5, 50, true);
        Create_Icon("40005.8", "Create Lead", "../Vastu/lead", 3, "", "", false, "", "vastuld", "crmvmain", 5, 50, true);
        Create_Icon("40005.9", "List of Leads Farm(Total)", "../Vastu/lead_list", 3, "", "", false, "", "vastuld", "crmvmain", 5, 50, true);

        Create_Icon("40006", "Lead Master", "", 2, "fa fa-gears", "nav child_menu", true, "cmld", "crmvmain", "crmvmain", 5, 9, true);
        Create_Icon("40006.1", "Status Setting", "../Vastu/lead_status", 3, "", "", false, "", "cmld", "crmvmain", 5, 50, true);
        Create_Icon("12003.6", "Lead Status Master", "../Mst/master_ctrl", 3, "", "", false, "-", "cmld", "crmvmain", 5, 110, true);
        Create_Icon("12003.8", "Lead Closer Time", "../Mst/master_ctrl", 3, "", "", false, "", "cmld", "crmvmain", 5, 50, true);
        Create_Icon("12003.5", "Lead Source Master", "../Mst/master_ctrl", 3, "", "", false, "-", "cmld", "crmvmain", 5, 110, true);
        Create_Icon("12003.2", "Next Action Master", "../Mst/master_ctrl", 3, "", "", false, "-", "cmld", "crmvmain", 5, 110, true);
        Create_Icon("12003.3", "Product Master", "../Mst/master_ctrl", 3, "", "", false, "-", "cmld", "crmvmain", 5, 110, true);
        Create_Icon("12003.4", "Deal Probability Master", "../Mst/master_ctrl", 3, "", "", false, "-", "cmld", "crmvmain", 5, 110, true);
        Create_Icon("40006.2", "Lead Rating Master", "../Mst/master_ctrl", 3, "", "", false, "-", "cmld", "crmvmain", 5, 110, true);

        Create_Icon("40007", "Order Master", "", 2, "fa fa-gears", "nav child_menu", true, "corms", "crmvmain", "crmvmain", 5, 9, true);
        Create_Icon("40007.1", "Order Status", "../Mst/master_ctrl", 3, "", "", false, "", "corms", "crmvmain", 5, 50, true);
        Create_Icon("40007.2", "Order Status Setting", "../Vastu/Order_status", 3, "", "", false, "", "corms", "crmvmain", 5, 50, true);

        Create_Icon("40008", "Order", "", 2, "fa fa-space-shuttle", "nav child_menu", true, "crmor", "crmvmain", "crmvmain", 5, 9, true);
        Create_Icon("40008.1", "List of Orders", "", 3, "", "", false, "", "crmor", "crmvmain", 5, Convert.ToDecimal(74.04), true, "onclick=$menuclick(this);$");
        Create_Icon("40008.2", "Create Order", "../vastu/order", 3, "", "", false, "-", "crmor", "crmvmain", 5, 110, true);
        Create_Icon("40008.3", "Pending Orders For Assign", "", 3, "", "", false, "", "crmor", "crmvmain", 5, Convert.ToDecimal(74.04), true, "onclick=$menuclick(this);$");


        Create_Icon("40009", "Site Visit", "", 2, "fa fa-road", "nav child_menu", true, "crmsv", "crmvmain", "crmvmain", 5, 9, true);
        Create_Icon("40009.1", "Visit Entry", "../vastu/s_visit", 3, "", "", false, "", "crmsv", "crmvmain", 5, 50, true);
        Create_Icon("40009.2", "Site Status", "../vastu/s_status", 3, "", "", false, "", "crmsv", "crmvmain", 5, 50, true);
        Create_Icon("40009.3", "Site HandOver", "../vastu/s_hover", 3, "", "", false, "", "crmsv", "crmvmain", 5, 50, true);
     
        Create_Icon("40010", "Lead Integration", "../vastu/i_mart", 2, "fa fa-plug", "nav child_menu", true, "crmin", "crmvmain", "crmvmain", 5, 9, true);
        //Create_Icon("40010.1", "India Mart", "../vastu/i_mart", 3, "", "", false, "", "crmin", "crmvmain", 5, 50, true);

        Create_Icon("40011", "Ticket Master", "", 2, "fa fa-gears", "nav child_menu", true, "crmtm", "crmvmain", "crmvmain", 5, 9, true);
        Create_Icon("400011.1", "Ticket Type", "../Mst/master_ctrl", 3, "", "", false, "", "crmtm", "crmvmain", 5, 50, true);
        Create_Icon("400011.2", "Ticket Status", "../Mst/master_ctrl", 3, "", "", false, "", "crmtm", "crmvmain", 5, 50, true);
        Create_Icon("400011.3", "Ticket Priority", "../Mst/master_ctrl", 3, "", "", false, "", "crmtm", "crmvmain", 5, 50, true);
        Create_Icon("400011.4", "Ticket Group", "../Hr/role_mt", 3, "", "", false, "", "crmtm", "crmvmain", 5, 50, true);
        Create_Icon("400011.5", "Mode Of Meeting", "../Mst/master_ctrl", 3, "", "", false, "", "crmtm", "crmvmain", 5, 50, true);
        Create_Icon("400011.6", "Ticket Source", "../Mst/master_ctrl", 3, "", "", false, "", "crmtm", "crmvmain", 5, 50, true);
        Create_Icon("400011.7", "Work Flow", "../hr/w_flow", 3, "", "", false, "", "crmtm", "crmvmain", 5, 50, true);
       


        Create_Icon("40012", "Ticket", "", 2, "fa fa-gears", "nav child_menu", true, "crmt", "crmvmain", "crmvmain", 5, 9, true);
        Create_Icon("400012.1", "Create Ticket", "../Vastu/cr_ticket", 3, "", "", false, "", "crmt", "crmvmain", 5, 50, true);
        Create_Icon("400012.2", "List Of Tickets", "../Vastu/ticket_list", 3, "", "", false, "", "crmt", "crmvmain", 5, 50, true);
        Create_Icon("400012.3", "List Of Closed Tickets", "../Vastu/ticket_list", 3, "", "", false, "", "crmt", "crmvmain", 5, 50, true);
        Create_Icon("400012.4", "List Of Web Tickets", "../Vastu/web_tlist", 3, "", "", false, "", "crmt", "crmvmain", 5, 50, true);

        Create_Icon("40013", "Minutes Of Meeting", "../Vastu/mom", 2, "fa fa-gears", "nav child_menu", true, "crmom", "crmvmain", "crmvmain", 5, 9, true);
        Create_Icon("40014", "CRM Summary", "../Vastu/crm_summ", 2, "fa fa-gears", "nav child_menu", true, "crmsum", "crmvmain", "crmvmain", 5, 9, true);

        Create_Icon("40015", "Communication", "", 2, "fa fa-gears", "nav child_menu", true, "crcm", "crmvmain", "crmvmain", 5, 9, true);
        Create_Icon("400015.1", "Call Log", "../Vastu/call_log", 3, "", "", false, "", "crcm", "crmvmain", 5, 50, true);
        Create_Icon("400015.2", "Appointment", "../Vastu/appointment", 3, "", "", false, "", "crcm", "crmvmain", 5, 50, true);
        Create_Icon("400015.3", "List Of Appointments", "../Vastu/app_list", 3, "", "", false, "", "crcm", "crmvmain", 5, 50, true);
        Create_Icon("400015.4", "Outcome Master", "../Vastu/outcome", 3, "", "", false, "", "crcm", "crmvmain", 5, 50, true);


        #endregion
        Create_Icon("41000", "Banquet Hall", "../../../../erp/dashboard", 1, "", "", true, "", "bhmain", "bhmain", 5, 20, true, "", "");
        #region
        Create_Icon("41001", "Hall Master", "", 2, "fa fa-gears", "nav child_menu", true, "bhm", "bhmain", "bhmain", 5, 9, true);
        Create_Icon("41001.1", "Expense Head", "../Mst/master_ctrl", 3, "", "", false, "", "bhm", "bhmain", 5, 50, true);
        Create_Icon("41001.2", "Function Type", "../Mst/master_ctrl", 3, "", "", false, "", "bhm", "bhmain", 5, 50, true);
        Create_Icon("41001.3", "Service Type", "../Mst/master_ctrl", 3, "", "", false, "", "bhm", "bhmain", 5, 50, true);
        Create_Icon("41001.4", "Menu Type", "../Mst/master_ctrl", 3, "", "", false, "", "bhm", "bhmain", 5, 50, true);
        Create_Icon("7002.16", "Payment Mode", "../Mst/master_ctrl", 3, "", "", false, "-", "bhm", "bhmain", 5, Convert.ToDecimal(71.08), true);// transfer to fee receipt


        Create_Icon("41002", "Costing", "", 2, "fa fa-money", "nav child_menu", true, "bhc", "bhmain", "bhmain", 5, 9, true);
        Create_Icon("41002.1", "Item Price", "../Purchase/vendor_app", 3, "", "", false, "", "bhc", "bhmain", 5, 50, true);
        Create_Icon("41002.2", "Other Cost", "-", 3, "", "", false, "", "bhc", "bhmain", 5, 50, true);
        Create_Icon("41002.3", "Function Cost", "../hall/amount_exp", 3, "", "", false, "", "bhc", "bhmain", 5, 50, true);

        Create_Icon("41003", "Inventory", "", 2, "fa fa-search", "nav child_menu", true, "bhim", "bhmain", "bhmain", 5, 9, true);
        Create_Icon("41003.1", "Item Receipt", "../Inventory/mrn", 3, "", "", false, "", "bhim", "bhmain", 5, 50, true);
        Create_Icon("41003.2", "Item Return", "../Inventory/store_return", 3, "", "", false, "", "bhim", "bhmain", 5, 50, true);
        Create_Icon("41003.3", "Item Issue", "../Vastu/comp_doc", 3, "", "", false, "", "bhim", "bhmain", 5, 50, true);
        Create_Icon("41003.4", "Material Requisition", "../Inventory/mat_req", 3, "", "", false, "", "bhim", "bhmain", 5, 50, true);
        Create_Icon("41003.5", "Physical/Opening", "../Inventory/phy_verify", 3, "", "", false, "", "bhim", "bhmain", 5, 50, true);
        Create_Icon("17001.26", "Other Charges", "../Mst/master_ctrl", 3, "", "", false, "", "bhim", "bhmain", 5, 50, true);

        Create_Icon("41004", "Function", "", 2, "fa fa-gears", "nav child_menu", true, "bhfn", "bhmain", "bhmain", 5, 9, true);
        Create_Icon("41004.1", "Booking", "../hall/prog", 3, "", "", false, "", "bhfn", "bhmain", 5, 50, true);
        Create_Icon("41004.2", "Fun Amt Received", "../hall/booking_amt", 3, "", "", false, "", "bhfn", "bhmain", 5, 50, true);

        Create_Icon("41005", "Inventory Master", "", 2, "fa fa-gears", "nav child_menu", true, "bhim", "bhmain", "bhmain", 5, 9, true);
        Create_Icon("41005.1", "Item Group", "../Mst/doc_master", 3, "", "", false, "", "bhim", "bhmain", 5, 50, true);
        Create_Icon("41005.2", "Item Sub Group", "../Billing/itemsubgrp", 3, "", "", false, "", "bhim", "bhmain", 5, 50, true);
        Create_Icon("41005.3", "Unit Measurement", "../Mst/master_ctrl", 3, "", "", false, "", "bhim", "bhmain", 5, 50, true);
        Create_Icon("41005.4", "Packing Type", "../Mst/master_ctrl", 3, "", "", false, "", "bhim", "bhmain", 5, 50, true);
        Create_Icon("41005.5", "Item Master", "../Billing/itemservice", 3, "", "", false, "", "bhim", "bhmain", 5, 50, true);
        Create_Icon("41005.6", "HSN/SAC", "../Billing/billing_master", 3, "", "", false, "", "bhim", "bhmain", 5, 50, true);
        Create_Icon("41005.7", "MRN Type Master", "../Mst/doc_master", 3, "", "", false, "", "bhim", "bhmain", 5, 50, true);
        Create_Icon("41005.8", "Vendor Master", "../Purchase/party", 3, "", "", false, "", "bhim", "bhmain", 5, 50, true);
        //  Create_Icon("28005.2", "Vendor Unit Detail", "../Purchase/party_unit", 3, "", "", false, "", "bhim", "bhmain", 5, 50, true);
        Create_Icon("17001.22", "Storage Condition", "../Mst/master_ctrl", 3, "", "", false, "", "bhim", "bhmain", 5, 50, true);
        Create_Icon("17001.25", "Form Control", "../Mst/form_ctrl", 3, "", "", false, "", "bhim", "bhmain", 5, 50, true);
        Create_Icon("17001.28", "Doc Template", "../Education/rpt_temp", 3, "", "", false, "", "bhim", "bhmain", 5, Convert.ToDecimal(85.02), true);
        Create_Icon("28004.11", "Payment Term Master", "../Mst/master_ctrl", 3, "", "", false, "-", "bhim", "bhmain", 5, Convert.ToDecimal(88.01), true);
        Create_Icon("17001.29", "Bulk Upload", "../vastu/bulk_acc", 3, "", "", false, "", "bhim", "bhmain", 5, Convert.ToDecimal(85.02), true);
        //Create_Icon("41005.9", "Location", "", 3, "", "nav child_menu", true, "bhloc", "bhim", "bhmain", 5, 50, true);

        //Create_Icon("41005.9.1", "Building Name", "../Mst/master_ctrl", 4, "", "", false, "-", "bhloc", "bhmain", 5, 50, true);
        //Create_Icon("41005.9.2", "Floor Name", "../Mst/cascade_mst", 4, "", "", false, "-", "bhloc", "bhmain", 5, 50, true);
        //Create_Icon("41005.9.3", "Room Name", "../Mst/cascade_mst", 4, "", "", false, "-", "bhloc", "bhmain", 5, 50, true);
        //Create_Icon("41005.9.4", "Rack Name", "../Mst/cascade_mst", 4, "", "", false, "-", "bhloc", "bhmain", 5, 50, true);
        //Create_Icon("41005.9.5", "Bin Name", "../Mst/cascade_mst", 4, "", "", false, "-", "bhloc", "bhmain", 5, 50, true);

        //Create_Icon("17001.15", "Building Name", "../Mst/master_ctrl", 3, "", "", false, "-", "bhim", "bhmain", 5, 50, true);
        //Create_Icon("17001.16", "Floor Name", "../Mst/cascade_mst", 3, "", "", false, "-", "bhim", "bhmain", 5, 50, true);
        //Create_Icon("17001.17", "Room Name", "../Mst/cascade_mst", 3, "", "", false, "-", "bhim", "bhmain", 5, 50, true);
        //Create_Icon("17001.18", "Rack Name", "../Mst/cascade_mst", 3, "", "", false, "-", "bhim", "bhmain", 5, 50, true);
        //Create_Icon("17001.19", "Bin Name", "../Mst/cascade_mst", 3, "", "", false, "-", "bhim", "bhmain", 5, 50, true);

        Create_Icon("17001.23", "Single Location", "../Mst/master_ctrl", 3, "", "", false, "", "bhim", "bhmain", 5, 50, true);

        Create_Icon("17005", "Reports", "", 2, "fa fa-print", "nav child_menu", true, "bhrp", "bhmain", "bhmain", 5, 88, true);
        Create_Icon("17005.1", "Stock Reports", "../Inventory/invreps2", 3, "", "", false, "-", "bhrp", "bhmain", 5, Convert.ToDecimal(88.01), true);

        #endregion
        Create_Icon("42000", "Service", "../../../../erp/dashboard", 1, "", "", true, "", "sermain", "sermain", 5, 20, true, "", "");
        Create_Icon("42001", "Service Master", "", 2, "fa fa-gears", "nav child_menu", true, "sermst", "sermain", "sermain", 5, 9, true);
        //Create_Icon("43001.1", "Item Master", "../Billing/itemservice", 3, "", "", false, "", "sermst", "sermain", 5, Convert.ToDecimal(85.02), true);
        Create_Icon("17001.2", "HSN/SAC", "../Billing/billing_master", 3, "", "", false, "", "sermst", "sermain", 5, Convert.ToDecimal(85.02), true);
        Create_Icon("42001.2", "Service Master", "../hall/serc_master", 3, "", "", false, "", "sermst", "sermain", 5, Convert.ToDecimal(85.02), true);
        Create_Icon("42001.3", "Sales Person", "../hall/s_person", 3, "", "", false, "", "sermst", "sermain", 5, Convert.ToDecimal(85.02), true);
        Create_Icon("42001.4", "Pay Term Master", "../hall/pay_term", 3, "", "", false, "", "sermst", "sermain", 5, Convert.ToDecimal(85.02), true);
        Create_Icon("17001.5", "Item Group", "../Mst/doc_master", 3, "", "", false, "", "sermst", "sermain", 5, 50, true);
        Create_Icon("17001.8", "Item Sub Group", "../Billing/itemsubgrp", 3, "", "", false, "", "sermst", "sermain", 5, 50, true);
        Create_Icon("42001.5", "Frequency Master", "../mst/freq_mst", 3, "", "", false, "", "sermst", "sermain", 5, 100, true);

        Create_Icon("42002", "Service Invoicing", "", 2, "fa fa-gears", "nav child_menu", true, "serinv", "sermain", "sermain", 5, 9, true);
        Create_Icon("42002.1", "invoice", "../hall/srv_inv", 3, "", "", false, "", "serinv", "sermain", 5, 50, true);
        Create_Icon("42002.2", "Recurring Invoice", "../hall/rcr_inv", 3, "", "", false, "", "serinv", "sermain", 5, 50, true);
        Create_Icon("42002.3", "Credit Note", "../hall/cr_note", 3, "", "", false, "", "serinv", "sermain", 5, 50, true);

        Create_Icon("42003", "Service Asset", "", 2, "fa fa-user", "", true, "asser", "sermain", "sermain", 5, 9, true);
        Create_Icon("42003.1", "Asset Master", "../hall/asst_mst", 3, "", "", false, "", "sermain", "sermain", 5, 50, true);
        Create_Icon("42003.2", "Asset Allocate", "../hall/asst_lac", 3, "", "", false, "", "asser", "sermain", 5, 100, true);
        Create_Icon("42003.3", "Asset Return Back", "../hall/asst_return", 3, "", "", false, "", "asser", "sermain", 5, 100, true);
        Create_Icon("42003.4", "Assets Write Off", "../hall/awf", 3, "", "", false, "", "asser", "sermain", 5, 100, true);
        Create_Icon("42003.5", "List Of Assets", "../hall/ast_list", 3, "", "", false, "", "asser", "sermain", 5, 100, true);
        Create_Icon("42003.6", "Asset Contracts", "../hall/as_contracts", 3, "", "", false, "", "asser", "sermain", 5, 100, true);
        Create_Icon("42003.7", "Reports", "../hall/hr_report", 3, "", "", false, "", "asser", "sermain", 5, 100, true);
        Create_Icon("42003.8", "Service Role", "../Hr/role_mt", 3, "", "", false, "", "asser", "sermain", 5, 50, true);
        Create_Icon("42003.9", "Service Type", "../mst/master_ctrl", 3, "", "", false, "", "asser", "sermain", 5, 100, true);
        Create_Icon("42003.10", "List Of Contracts", "../hall/contract_list", 3, "", "", false, "", "asser", "sermain", 5, 100, true);
        Create_Icon("42003.11", "Contract Approve", "../hall/contr_apr", 3, "", "", false, "", "asser", "sermain", 5, 100, true);
        Create_Icon("33002.4", "Asset Type", "../mst/master_ctrl", 3, "", "", false, "", "asser", "sermain", 5, 100, true);

        Create_Icon("21008", "Customer Master", "", 2, "fa fa-gears", "nav child_menu", true, "custmas", "sermain", "sermain", 5, 47, true);
        Create_Icon("21008.1", "Customer Detail", "../Purchase/party", 3, "", "", false, "", "custmas", "sermain", 5, 50, true);       
        Create_Icon("21008.2", "List of Customers", "", 3, "", "", false, "", "custmas", "sermain", 5, Convert.ToDecimal(74.04), true, "onclick=$menuclick(this);$");



        Create_Icon("43000", "Travel Management", "../../../../erp/dashboard", 1, "", "", true, "", "ttmain", "ttmain", 5, 20, true, "", "");
        #region
        Create_Icon("43001", "Travel Master", "", 2, "fa fa-gears", "nav child_menu", true, "trmst", "ttmain", "ttmain", 5, 9, true);
        Create_Icon("43002", "Travelling", "", 2, "fa fa-gears", "nav child_menu", true, "trvmst", "ttmain", "ttmain", 5, 9, true);
        Create_Icon("43002.1", "Travel Request", "../Hr/tr_req", 3, "", "", false, "", "trvmst", "ttmain", 5, Convert.ToDecimal(85.02), true);
        Create_Icon("43002.2", "Travel Approval", "../Hr/tr_app", 3, "", "", false, "", "trvmst", "ttmain", 5, Convert.ToDecimal(85.02), true);
        Create_Icon("43002.3", "Travel Confirmation", "-", 3, "", "", false, "", "trvmst", "ttmain", 5, Convert.ToDecimal(85.02), true);
        #endregion
        Create_Icon("44000", "Quality Inspection-Item", "../../../../erp/dashboard", 1, "", "", true, "", "qcmain", "qcmain", 5, 20, true, "", "");
        #region
        Create_Icon("44001", "Quality Master", "", 2, "fa fa-gears", "nav child_menu", true, "qcm", "qcmain", "qcmain", 5, 9, true);
        Create_Icon("44001.1", "QC Template", "../Inventory/qctemp", 3, "", "", false, "", "qcm", "qcmain", 5, 50, true);

        Create_Icon("44002", "Incoming Inspection", "", 2, "fa fa-gears", "nav child_menu", true, "inqc", "qcmain", "qcmain", 5, 9, true);
        Create_Icon("44002.1", "QC Inspection", "../Inventory/qcinsp", 3, "", "", false, "", "inqc", "qcmain", 5, 50, true);

        Create_Icon("44003", "Outgoing Inspection", "", 2, "fa fa-gears", "nav child_menu", true, "outqc", "qcmain", "qcmain", 5, 9, true);

        #endregion
        Create_Icon("45000", "Tally Dashboard", "../../../../erp/dashboard", 1, "", "", true, "", "tldmain", "tldmain", 5, 20, true, "", "");

        #region
        Create_Icon("45004", "Comapny Mapping", "../account/tly_cmap", 1, "", "", true, "", "tcmap", "tldmain", 5, 20, true, "", "");
        Create_Icon("45001", "Tally Dashboard", "../account/tly_gmap", 2, "", "", true, "", "tgmap", "tldmain", 5, 20, true, "", "");
        Create_Icon("45002", "Group Mapping", "../account/tly_gmap", 2, "", "", true, "", "tgmap", "tldmain", 5, 20, true, "", "");
        Create_Icon("45003", "Tally Reports", "../account/gst_rpt", 2, "", "", true, "", "trpt", "tldmain", 5, 20, true, "", "");
        #endregion
        Create_Icon("46000", "Vendor Portal", "../../../../erp/dashboard", 1, "", "", true, "", "vnd", "vdmain", 5, 20, true, "", "");
        #region
        Create_Icon("46001", "Purchase Order", "../Purchase/vd_po", 2, "fa fa-gears", "-", false, "-", "vnd", "vdmain", 5, 9, true);        
        Create_Icon("46002", "Dispatch", "../Purchase/vd_dis", 2, "fa fa-gears", "-", false, "-", "vnd", "vdmain", 5, 9, true);        
        #endregion
        Create_Icon("47000", "Customer Portal", "../../../../erp/dashboard", 1, "", "", true, "", "cstm", "cstmain", 5, 20, true, "", "");
        #region
        Create_Icon("47001", "Purchase Order", "../Billing/cust_po", 2, "fa fa-gears", "-", false, "-", "cstm", "cstmain", 5, 9, true);
        Create_Icon("47002", "List of Pending Orders", "", 2, "fa fa-clipboard", "", false, "", "cstm", "cstmain", 5, 10, true, "onclick=$menuclick(this);$");

        #endregion
        Create_Icon("48000", "Finance Services", "../../../../erp/dashboard", 1, "", "", true, "", "fsm", "fsmain", 5, 20, true, "", "");

        Create_Icon("48001", "CMA", "", 2, "fa fa-gears", "nav child_menu", true, "cmam", "fsmain", "fsmain", 5, 9, true);

        Create_Icon("48002", "Eligiblity Cal", "", 2, "fa fa-gears", "nav child_menu", true, "egcm", "fsmain", "fsmain", 5, 9, true);
        Create_Icon("48002.1", "Financial Ratio", "../Cma/fa_ratio", 3, "", "", false, "", "egcm", "fsmain", 5, 50, true);
        Create_Icon("48002.2", "WACC", "../Cma/wacc", 3, "", "", false, "", "egcm", "fsmain", 5, 50, true);
        Create_Icon("48002.3", "Letter Of Credit", "../Cma/lc", 3, "", "", false, "", "egcm", "fsmain", 5, 50, true);
        Create_Icon("48002.4", "Packing Credit", "../Cma/plc", 3, "", "", false, "", "egcm", "fsmain", 5, 50, true);
        Create_Icon("48002.5", "Bank Guranatee", "../Cma/bank_gur", 3, "", "", false, "", "egcm", "fsmain", 5, 50, true);
        Create_Icon("48002.6", "LC Purchase BD", "../Cma/lcpbd", 3, "", "", false, "", "egcm", "fsmain", 5, 50, true);
        Create_Icon("48002.7", "LC Sales BD", "../Cma/lcsbd", 3, "", "", false, "", "egcm", "fsmain", 5, 50, true);
        Create_Icon("48002.8", "Unsecured BD", "../Cma/unpbd", 3, "", "", false, "", "egcm", "fsmain", 5, 50, true);
        Create_Icon("48002.9", "Loan Against Rent Rec", "../Cma/loan_ag_rent", 3, "", "", false, "", "egcm", "fsmain", 5, 50, true);

        Create_Icon("48003", "General Cal", "", 2, "fa fa-gears", "nav child_menu", true, "genm", "fsmain", "fsmain", 5, 9, true);

        Create_Icon("48004", "Finance Master", "", 2, "fa fa-gears", "nav child_menu", true, "finm", "fsmain", "fsmain", 5, 9, true);
        Create_Icon("9003.3.10", "Bank Name", "../Mst/master_ctrl", 4, "", "", false, "-", "finm", "fsmain", 5, Convert.ToDecimal(88.02), true);
        Create_Icon("12003.3", "Product Name", "../Mst/master_ctrl", 3, "", "", false, "-", "finm", "fsmain", 5, 110, true);
        Create_Icon("48004.1", "Form Control", "../Mst/form_ctrl", 3, "", "", false, "", "finm", "fsmain", 5, 50, true);
        Create_Icon("40002.1", "Client Account", "../Purchase/party", 3, "", "", false, "", "finm", "fsmain", 5, 50, true);


    }
    private bool Create_Icon(string m_id, string m_name, string m_link, int m_level, string m_icon, string m_css, bool m_submenu, string m_module1, string m_module2, string m_module3, int u_id, decimal m_order, bool m_default)
    {
        bool data = false;
        int m_defaultno = Convert.ToInt32(m_default);
        int m_submenuno = Convert.ToInt32(m_submenu);
        if (is_icon(m_id, m_module3) == true)
        {
            data = sgen.execute_cmd(userCode, "insert into menus (m_id,m_name,m_link,m_level,m_icon,m_css,m_submenu,m_module1,m_module2,m_module3,u_id,m_order,m_default) " +
            "values ('" + m_id + "','" + m_name + "','" + m_link + "','" + m_level + "','" + m_icon + "','" + m_css + "'," + m_submenuno + ",'" + m_module1 + "','" + m_module2 + "','" + m_module3 + "','" + u_id + "','" + m_order + "'," + m_defaultno + ")");
            data = true;
            return data;
        }
        else
        {
            data = false;
            return data;
        }
    }
    private bool Create_Icon(string m_id, string m_name, string m_link, int m_level, string m_icon, string m_css, bool m_submenu, string m_module1, string m_module2, string m_module3, int u_id, decimal m_order, bool m_default, string attributes)
    {
        bool data = false;
        if (is_icon(m_id, m_module3) == true)
        {
            data = sgen.execute_cmd(userCode, "insert into menus (m_id,m_name,m_link,m_level,m_icon,m_css,m_submenu,m_module1,m_module2,m_module3,u_id,m_order,m_default,attributes) " +
            "values ('" + m_id + "','" + m_name + "','" + m_link + "','" + m_level + "','" + m_icon + "','" + m_css + "'," + m_submenu + ",'" + m_module1 + "','" + m_module2 + "','" + m_module3 + "','" + u_id + "','" + m_order + "'," + m_default + ",'" + attributes + "')");
            data = true;
            return data;
        }
        else
        {
            data = false;
            return data;
        }
    }
    private bool Create_Icon(string m_id, string m_name, string m_link, int m_level, string m_icon, string m_css, bool m_submenu, string m_module1, string m_module2, string m_module3, int u_id, decimal m_order, bool m_default, string m_textfront, string m_textback)
    {
        bool data = false;
        if (is_icon(m_id, m_module3) == true)
        {
            data = sgen.execute_cmd(userCode, "insert into menus (m_id,m_name,m_link,m_level,m_icon,m_css,m_submenu,m_module1,m_module2,m_module3,u_id,m_order,m_default,m_textfront,m_textback) " +
            "values ('" + m_id + "','" + m_name + "','" + m_link + "','" + m_level + "','" + m_icon + "','" + m_css + "'," + m_submenu + ",'" + m_module1 + "','" + m_module2 + "','" + m_module3 + "','" + u_id + "','" + m_order + "'," + m_default + ",'" + m_textfront + "','" + m_textback + "')");
            data = true;
            return data;
        }
        else
        {
            data = false;
            return data;
        }
    }
    private bool is_icon(string m_id, string m_module3)
    {
        bool result = true;
        string abcd = sgen.getstring(userCode, "select m_id from menus where m_id='" + m_id + "' and (m_module3='" + m_module3 + "' or m_module3='" + m_module3 + "_xx')");
        if (abcd == m_id)
        {
            result = false;
            return result;
        }
        else
        {
            result = true;
            return result;
        }

    }
}

public class role_class
{
    sgenFun sgen;
    string userCode = "";
    public void insert_roles()
    {
        userCode = sgen.getUserCode();
        //main module Training ------------------------------------
        #region
        create_role("1", "super_adm", "1000", "Training", "tmain-1", 1);
        create_role("2", "corp_adm", "1000", "Training", "tmain-2", 2);
        create_role("3", "corp_trainer", "1000", "Training", "tmain-3", 3);
        create_role("4", "corp_trainee", "1000", "Training", "tmain-4", 4);
        create_role("5", "kr_user", "1000", "Training", "tmain-5", 5);
        create_role("6", "conf_user", "1000", "Training", "tmain-6", 6);
        #endregion
        //main module KR------------------------------------
        #region
        create_role("7", "super_adm", "2000", "Knowledge Repository", "krmain-1", 1);
        create_role("8", "corp_adm", "2000", "Knowledge Repository", "krmain-2", 2);
        create_role("9", "corp_trainer", "2000", "Knowledge Repository", "krmain-3", 3);
        create_role("10", "corp_trainee", "2000", "Knowledge Repository", "krmain-4", 4);
        create_role("11", "kr_user", "2000", "Knowledge Repository", "krmain-5", 5);
        create_role("12", "conf_user", "2000", "Knowledge Repository", "krmain-6", 6);
        #endregion
        //main module Meeting Organiser------------------------------------
        #region
        create_role("13", "super_adm", "3000", "Conference", "crmain-1", 1);
        create_role("14", "corp_adm", "3000", "Conference", "crmain-2", 2);
        create_role("15", "corp_trainer", "3000", "Conference", "crmain-3", 3);
        create_role("16", "corp_trainee", "3000", "Conference", "crmain-4", 4);
        create_role("17", "kr_user", "3000", "Conference", "crmain-5", 5);
        create_role("18", "conf_user", "3000", "Conference", "crmain-6", 6);
        #endregion
        //main module admin------------------------------------
        #region
        create_role("19", "super_adm", "4000", "Admin", "mstmain-1", 1);
        create_role("20", "corp_adm", "4000", "Admin", "mstmain-2", 2);
        create_role("21", "corp_trainer", "4000", "Admin", "mstmain-3", 3);
        create_role("22", "corp_trainee", "4000", "Admin", "mstmain-4", 4);
        create_role("23", "kr_user", "4000", "Admin", "mstmain-5", 5);
        create_role("24", "conf_user", "4000", "Admin", "mstmain-6", 6);
        #endregion
        //main module Client User------------------------------------
        #region
        create_role("25", "super_adm", "5000", "Work Allocation", "camain-1", 1);
        create_role("26", "corp_adm", "5000", "Work Allocation", "camain-2", 2);
        create_role("27", "corp_trainer", "5000", "Work Allocation", "camain-3", 3);
        create_role("28", "corp_trainee", "5000", "Work Allocation", "camain-4", 4);
        create_role("29", "kr_user", "5000", "Work Allocation", "camain-5", 5);
        create_role("30", "conf_user", "5000", "Work Allocation", "camain-6", 6);
        #endregion
        //main module Insurance------------------------------------
        #region
        create_role("31", "super_adm", "6000", "Insurance", "insmain-1", 1);
        create_role("32", "corp_adm", "6000", "Insurance", "insmain-2", 2);
        create_role("33", "corp_trainer", "6000", "Insurance", "insmain-3", 3);
        create_role("34", "corp_trainee", "6000", "Insurance", "insmain-4", 4);
        create_role("35", "kr_user", "6000", "Insurance", "insmain-5", 5);
        create_role("36", "conf_user", "6000", "Insurance", "insmain-6", 6);
        #endregion
        //main module Education------------------------------------
        #region
        create_role("37", "super_adm", "7000", "Education", "edumain-1", 1);
        create_role("38", "corp_adm", "7000", "Education", "edumain-2", 2);
        create_role("39", "corp_trainer", "7000", "Education", "edumain-3", 3);
        create_role("40", "corp_trainee", "7000", "Education", "edumain-4", 4);
        create_role("41", "kr_user", "7000", "Education", "edumain-5", 5);
        create_role("42", "conf_user", "7000", "Education", "edumain-6", 6);
        #endregion
        //main module Expense------------------------------------
        #region
        create_role("43", "super_adm", "8000", "Employee Self Service", "exmain-1", 1);
        create_role("44", "corp_adm", "8000", "Employee Self Service", "exmain-2", 2);
        create_role("45", "corp_trainer", "8000", "Employee Self Service", "exmain-3", 3);
        create_role("46", "corp_trainee", "8000", "Employee Self Service", "exmain-4", 4);
        create_role("47", "kr_user", "8000", "Employee Self Service", "exmain-5", 5);
        create_role("48", "conf_user", "8000", "Employee Self Service", "exmain-6", 6);
        #endregion
        //main module Payroll------------------------------------
        #region
        create_role("49", "super_adm", "9000", "Payroll", "paymain-1", 1);
        create_role("50", "corp_adm", "9000", "Payroll", "paymain-2", 2);
        create_role("51", "corp_trainer", "9000", "Payroll", "paymain-3", 3);
        create_role("52", "corp_trainee", "9000", "Payroll", "paymain-4", 4);
        create_role("53", "kr_user", "9000", "Payroll", "paymain-5", 5);
        create_role("54", "conf_user", "9000", "Payroll", "paymain-6", 6);
        #endregion
        //main module Quality Assurance------------------------------------
        #region
        create_role("55", "super_adm", "10000", "Quality Assurance", "qamain-1", 1);
        create_role("56", "corp_adm", "10000", "Quality Assurance", "qamain-2", 2);
        create_role("57", "corp_trainer", "10000", "Quality Assurance", "qamain-3", 3);
        create_role("58", "corp_trainee", "10000", "Quality Assurance", "qamain-4", 4);
        create_role("59", "kr_user", "10000", "Quality Assurance", "qamain-5", 5);
        create_role("60", "conf_user", "10000", "Quality Assurance", "qamain-6", 6);
        #endregion
        //main module Task Management------------------------------------
        #region
        create_role("61", "super_adm", "11000", "Task Management", "tskmain-1", 1);
        create_role("62", "corp_adm", "11000", "Task Management", "tskmain-2", 2);
        create_role("63", "corp_trainer", "11000", "Task Management", "tskmain-3", 3);
        create_role("64", "corp_trainee", "11000", "Task Management", "tskmain-4", 4);
        create_role("65", "kr_user", "11000", "Task Management", "tskmain-5", 5);
        create_role("66", "conf_user", "11000", "Task Management", "tskmain-6", 6);
        #endregion
        //main module CRM------------------------------------
        #region
        create_role("67", "super_adm", "12000", "CRM", "crmmain-1", 1);
        create_role("68", "corp_adm", "12000", "CRM", "crmmain-2", 2);
        create_role("69", "corp_trainer", "12000", "CRM", "crmmain-3", 3);
        create_role("70", "corp_trainee", "12000", "CRM", "crmmain-4", 4);
        create_role("71", "kr_user", "12000", "CRM", "crmmain-5", 5);
        create_role("72", "conf_user", "12000", "CRM", "crmmain-6", 6);
        #endregion
        //main module Parent Portal------------------------------------
        #region
        create_role("73", "super_adm", "13000", "Parent Portal", "stpmain-1", 1);
        create_role("74", "corp_adm", "13000", "Parent Portal", "stpmain-2", 2);
        create_role("75", "corp_trainer", "13000", "Parent Portal", "stpmain-3", 3);
        create_role("76", "corp_trainee", "13000", "Parent Portal", "stpmain-4", 4);
        create_role("77", "kr_user", "13000", "Parent Portal", "stpmain-5", 5);
        create_role("78", "conf_user", "13000", "Parent Portal", "stpmain-6", 6);
        #endregion
        //main module Student Portal------------------------------------
        #region
        create_role("79", "super_adm", "14000", "Student Portal", "stdmain-1", 1);
        create_role("80", "corp_adm", "14000", "Student Portal", "stdmain-2", 2);
        create_role("81", "corp_trainer", "14000", "Student Portal", "stdmain-3", 3);
        create_role("82", "corp_trainee", "14000", "Student Portal", "stdmain-4", 4);
        create_role("83", "kr_user", "14000", "Student Portal", "stdmain-5", 5);
        create_role("84", "conf_user", "14000", "Student Portal", "stdmain-6", 6);
        #endregion
        //main module Question Paper Designer------------------------------------
        #region
        create_role("85", "super_adm", "15000", "Question Paper Designer", "qpdmain-1", 1);
        create_role("86", "corp_adm", "15000", "Question Paper Designer", "qpdmain-2", 2);
        create_role("87", "corp_trainer", "15000", "Question Paper Designer", "qpdmain-3", 3);
        create_role("88", "corp_trainee", "15000", "Question Paper Designer", "qpdmain-4", 4);
        create_role("89", "kr_user", "15000", "Question Paper Designer", "qpdmain-5", 5);
        create_role("90", "conf_user", "15000", "Question Paper Designer", "qpdmain-6", 6);
        #endregion
        //main module Library------------------------------------
        #region
        create_role("91", "super_adm", "16000", "Library", "lbrmain-1", 1);
        create_role("92", "corp_adm", "16000", "Library", "lbrmain-2", 2);
        create_role("93", "corp_trainer", "16000", "Library", "lbrmain-3", 3);
        create_role("94", "corp_trainee", "16000", "Library", "lbrmain-4", 4);
        create_role("95", "kr_user", "16000", "Library", "lbrmain-5", 5);
        create_role("96", "conf_user", "16000", "Library", "lbrmain-6", 6);
        #endregion
        //main module Inventory------------------------------------
        #region
        create_role("97", "super_adm", "17000", "Inventory", "invtmain-1", 1);
        create_role("98", "corp_adm", "17000", "Inventory", "invtmain-2", 2);
        create_role("99", "corp_trainer", "17000", "Inventory", "invtmain-3", 3);
        create_role("100", "corp_trainee", "17000", "Inventory", "invtmain-4", 4);
        create_role("101", "kr_user", "17000", "Inventory", "invtmain-5", 5);
        create_role("102", "conf_user", "17000", "Inventory", "invtmain-6", 6);
        #endregion
        //main module School Admin------------------------------------
        #region
        create_role("103", "super_adm", "18000", "School Admin", "schadmmain-1", 1);
        create_role("104", "corp_adm", "18000", "School Admin", "schadmmain-2", 2);
        create_role("105", "corp_trainer", "18000", "School Admin", "schadmmain-3", 3);
        create_role("106", "corp_trainee", "18000", "School Admin", "schadmmain-4", 4);
        create_role("107", "kr_user", "18000", "School Admin", "schadmmain-5", 5);
        create_role("108", "conf_user", "18000", "School Admin", "schadmmain-6", 6);
        #endregion
        //main module Performance------------------------------------
        #region
        create_role("109", "super_adm", "19000", "Performance", "schpermain-1", 1);
        create_role("110", "corp_adm", "19000", "Performance", "schpermain-2", 2);
        create_role("111", "corp_trainer", "19000", "Performance", "schpermain-3", 3);
        create_role("112", "corp_trainee", "19000", "Performance", "schpermain-4", 4);
        create_role("113", "kr_user", "19000", "Performance", "schpermain-5", 5);
        create_role("114", "conf_user", "19000", "Performance", "schpermain-6", 6);
        #endregion
        //main module EXIM------------------------------------
        #region
        create_role("115", "super_adm", "20000", "EXIM", "eximmain-1", 1);
        create_role("116", "corp_adm", "20000", "EXIM", "eximmain-2", 2);
        create_role("117", "corp_trainer", "20000", "EXIM", "eximmain-3", 3);
        create_role("118", "corp_trainee", "20000", "EXIM", "eximmain-4", 4);
        create_role("119", "kr_user", "20000", "EXIM", "eximmain-5", 5);
        create_role("120", "conf_user", "20000", "EXIM", "eximmain-6", 6);
        #endregion
        //main module Billing & Payment ------------------------------------
        #region
        create_role("121", "super_adm", "21000", "Billing & Payment", "bnpmain-1", 1);
        create_role("122", "corp_adm", "21000", "Billing & Payment", "bnpmain-2", 2);
        create_role("123", "corp_trainer", "21000", "Billing & Payment", "bnpmain-3", 3);
        create_role("124", "corp_trainee", "21000", "Billing & Payment", "bnpmain-4", 4);
        create_role("125", "kr_user", "21000", "Billing & Payment", "bnpmain-5", 5);
        create_role("126", "conf_user", "21000", "Billing & Payment", "bnpmain-6", 6);
        #endregion       
        //module Accounts & Finance
        #region
        create_role("127", "super_adm", "22000", "Accounts & Finance", "acctmain-1", 1);
        create_role("128", "corp_adm", "22000", "Accounts & Finance", "acctmain-2", 2);
        create_role("129", "corp_trainer", "22000", "Accounts & Finance", "acctmain-3", 3);
        create_role("130", "corp_trainee", "22000", "Accounts & Finance", "acctmain-4", 4);
        create_role("131", "kr_user", "22000", "Accounts & Finance", "acctmain-5", 5);
        create_role("132", "conf_user", "22000", "Accounts & Finance", "acctmain-6", 6);
        #endregion
        //module Transportation
        #region
        create_role("133", "super_adm", "23000", "Transportation", "trpmain-1", 1);
        create_role("134", "corp_adm", "23000", "Transportation", "trpmain-2", 2);
        create_role("135", "corp_trainer", "23000", "Transportation", "trpmain-3", 3);
        create_role("136", "corp_trainee", "23000", "Transportation", "trpmain-4", 4);
        create_role("137", "kr_user", "23000", "Transportation", "trpmain-5", 5);
        create_role("138", "conf_user", "23000", "Transportation", "trpmain-6", 6);
        #endregion
        //module Registration/Approvals
        #region
        create_role("139", "super_adm", "24000", "Registration/Approvals", "srmain-1", 1);
        create_role("140", "corp_adm", "24000", "Registration/Approvals", "srmain-2", 2);
        create_role("141", "corp_trainer", "24000", "Registration/Approvals", "srmain-3", 3);
        create_role("142", "corp_trainee", "24000", "Registration/Approvals", "srmain-4", 4);
        create_role("143", "kr_user", "24000", "Registration/Approvals", "srmain-5", 5);
        create_role("144", "conf_user", "24000", "Registration/Approvals", "srmain-6", 6);
        #endregion
        //module Front Desk
        #region
        create_role("145", "super_adm", "25000", "Front Desk", "fdskmain-1", 1);
        create_role("146", "corp_adm", "25000", "Front Desk", "fdskmain-2", 2);
        create_role("147", "corp_trainer", "25000", "Front Desk", "fdskmain-3", 3);
        create_role("148", "corp_trainee", "25000", "Front Desk", "fdskmain-4", 4);
        create_role("149", "kr_user", "25000", "Front Desk", "fdskmain-5", 5);
        create_role("150", "conf_user", "25000", "Front Desk", "fdskmain-6", 6);
        #endregion
        //module Hostel
        #region
        create_role("151", "super_adm", "26000", "Hostel", "hostmain-1", 1);
        create_role("152", "corp_adm", "26000", "Hostel", "hostmain-2", 2);
        create_role("153", "corp_trainer", "26000", "Hostel", "hostmain-3", 3);
        create_role("154", "corp_trainee", "26000", "Hostel", "hostmain-4", 4);
        create_role("155", "kr_user", "26000", "Hostel", "hostmain-5", 5);
        create_role("156", "conf_user", "26000", "Hostel", "hostmain-6", 6);
        #endregion
        //module Scholarship
        #region
        create_role("157", "super_adm", "27000", "Scholarship", "schlarmain-1", 1);
        create_role("158", "corp_adm", "27000", "Scholarship", "schlarmain-2", 2);
        create_role("159", "corp_trainer", "27000", "Scholarship", "schlarmain-3", 3);
        create_role("160", "corp_trainee", "27000", "Scholarship", "schlarmain-4", 4);
        create_role("161", "kr_user", "27000", "Scholarship", "schlarmain-5", 5);
        create_role("162", "conf_user", "27000", "Scholarship", "schlarmain-6", 6);
        #endregion
        //module Purchase
        #region
        create_role("163", "super_adm", "28000", "Purchase", "permain-1", 1);
        create_role("164", "corp_adm", "28000", "Purchase", "permain-2", 2);
        create_role("165", "corp_trainer", "28000", "Purchase", "permain-3", 3);
        create_role("166", "corp_trainee", "28000", "Purchase", "permain-4", 4);
        create_role("167", "kr_user", "28000", "Purchase", "permain-5", 5);
        create_role("168", "conf_user", "28000", "Purchase", "permain-6", 6);
        #endregion
        //module Fixed Asset Management
        #region
        create_role("169", "super_adm", "29000", "Fixed Asset Management", "fixmain-1", 1);
        create_role("170", "corp_adm", "29000", "Fixed Asset Management", "fixmain-2", 2);
        create_role("171", "corp_trainer", "29000", "Fixed Asset Management", "fixmain-3", 3);
        create_role("172", "corp_trainee", "29000", "Fixed Asset Management", "fixmain-4", 4);
        create_role("173", "kr_user", "29000", "Fixed Asset Management", "fixmain-5", 5);
        create_role("174", "conf_user", "29000", "Fixed Asset Management", "fixmain-6", 6);
        #endregion
        //module Online Test
        #region
        create_role("175", "super_adm", "30000", "Online Test", "ontmain-1", 1);
        create_role("176", "corp_adm", "30000", "Online Test", "ontmain-2", 2);
        create_role("177", "corp_trainer", "30000", "Online Test", "ontmain-3", 3);
        create_role("178", "corp_trainee", "30000", "Online Test", "ontmain-4", 4);
        create_role("179", "kr_user", "30000", "Online Test", "ontmain-5", 5);
        create_role("180", "conf_user", "30000", "Online Test", "ontmain-6", 6);
        #endregion
        //Module ess (no more)
        #region
        //create_role("181", "super_adm", "31000", "ESS", "essmain-1", 1);
        //create_role("182", "corp_adm", "31000", "ESS", "essmain-2", 2);
        //create_role("183", "corp_trainer", "31000", "ESS", "essmain-3", 3);
        //create_role("184", "corp_trainee", "31000", "ESS", "essmain-4", 4);
        //create_role("185", "kr_user", "31000", "ESS", "essmain-5", 5);
        //create_role("186", "conf_user", "31000", "ESS", "essmain-6", 6);
        #endregion
        //Module Feedback Portal
        #region
        create_role("187", "super_adm", "32000", "Feedback Portal", "fpmain-1", 1);
        create_role("188", "corp_adm", "32000", "Feedback Portal", "fpmain-2", 2);
        create_role("189", "corp_trainer", "32000", "Feedback Portal", "fpmain-3", 3);
        create_role("190", "corp_trainee", "32000", "Feedback Portal", "fpmain-4", 4);
        create_role("191", "kr_user", "32000", "Feedback Portal", "fpmain-5", 5);
        create_role("192", "conf_user", "32000", "Feedback Portal", "fpmain-6", 6);
        #endregion
        //Module Human Resource
        #region
        create_role("193", "super_adm", "33000", "Human Resourcel", "hrmain-1", 1);
        create_role("194", "corp_adm", "33000", "Human Resource", "hrmain-2", 2);
        create_role("195", "corp_trainer", "33000", "Human Resource", "hrmain-3", 3);
        create_role("196", "corp_trainee", "33000", "Human Resource", "hrmain-4", 4);
        create_role("197", "kr_user", "33000", "Human Resource", "hrmain-5", 5);
        create_role("198", "conf_user", "33000", "Human Resource", "hrmain-6", 6);
        #endregion
        //Module Compliance Management
        #region
        create_role("199", "super_adm", "34000", "Compliance Management", "cmmain-1", 1);
        create_role("200", "corp_adm", "34000", "Compliance Management", "cmmain-2", 2);
        create_role("201", "corp_trainer", "34000", "Compliance Management", "cmmain-3", 3);
        create_role("202", "corp_trainee", "34000", "Compliance Management", "cmmain-4", 4);
        create_role("203", "kr_user", "34000", "Compliance Management", "cmmain-5", 5);
        create_role("204", "conf_user", "34000", "Compliance Management", "cmmain-6", 6);
        #endregion
        //Module Financial Advisory Services
        #region
        create_role("205", "super_adm", "35000", "Financial Advisory Services", "tmmain-1", 1);
        create_role("206", "corp_adm", "35000", "Financial Advisory Services", "tmmain-2", 2);
        create_role("207", "corp_trainer", "35000", "Financial Advisory Services", "tmmain-3", 3);
        create_role("208", "corp_trainee", "35000", "Financial Advisory Services", "tmmain-4", 4);
        create_role("209", "kr_user", "35000", "Financial Advisory Services", "tmmain-5", 5);
        create_role("210", "conf_user", "35000", "Financial Advisory Services", "tmmain-6", 6);
        #endregion
        //Module Event Management
        #region
        create_role("211", "super_adm", "36000", "Event Management", "emmain-1", 1);
        create_role("212", "corp_adm", "36000", "Event Management", "emmain-2", 2);
        create_role("213", "corp_trainer", "36000", "Event Management", "emmain-3", 3);
        create_role("214", "corp_trainee", "36000", "Event Management", "emmain-4", 4);
        create_role("215", "kr_user", "36000", "Event Management", "emmain-5", 5);
        create_role("216", "conf_user", "36000", "Event Management", "emmain-6", 6);
        #endregion
        //Module Day Care
        #region
        create_role("217", "super_adm", "37000", "Day Care", "dcmain-1", 1);
        create_role("218", "corp_adm", "37000", "Day Care", "dcmain-2", 2);
        create_role("219", "corp_trainer", "37000", "Day Care", "dcmain-3", 3);
        create_role("220", "corp_trainee", "37000", "Day Care", "dcmain-4", 4);
        create_role("221", "kr_user", "37000", "Day Care", "dcmain-5", 5);
        create_role("222", "conf_user", "37000", "Day Care", "dcmain-6", 6);
        #endregion
        //Module Plant Maintainance
        #region
        create_role("223", "super_adm", "38000", "Plant Maintainance", "pmmain-1", 1);
        create_role("224", "corp_adm", "38000", "Plant Maintainance", "pmmain-2", 2);
        create_role("225", "corp_trainer", "38000", "Plant Maintainance", "pmmain-3", 3);
        create_role("226", "corp_trainee", "38000", "Plant Maintainance", "pmmain-4", 4);
        create_role("227", "kr_user", "38000", "Plant Maintainance", "pmmain-5", 5);
        create_role("228", "conf_user", "38000", "Plant Maintainance", "pmmain-6", 6);
        #endregion
        //Module Production
        #region
        create_role("229", "super_adm", "39000", "Production", "pdmain-1", 1);
        create_role("230", "corp_adm", "39000", "Production", "pdmain-2", 2);
        create_role("231", "corp_trainer", "39000", "Production", "pdmain-3", 3);
        create_role("232", "corp_trainee", "39000", "Production", "pdmain-4", 4);
        create_role("233", "kr_user", "39000", "Production", "pdmain-5", 5);
        create_role("234", "conf_user", "39000", "Production", "pdmain-6", 6);
        #endregion
        //CRM (Vastu)
        #region
        create_role("235", "super_adm", "40000", "CRM (Vastu)", "crmvmain-1", 1);
        create_role("236", "corp_adm", "40000", "CRM (Vastu)", "crmvmain-2", 2);
        create_role("237", "corp_trainer", "40000", "CRM (Vastu)", "crmvmain-3", 3);
        create_role("238", "corp_trainee", "40000", "CRM (Vastu)", "crmvmain-4", 4);
        create_role("239", "kr_user", "40000", "CRM (Vastu)", "crmvmain-5", 5);
        create_role("240", "conf_user", "40000", "CRM (Vastu)", "crmvmain-6", 6);
        #endregion
        //Banquet Hall
        #region
        create_role("241", "super_adm", "41000", "Banquette Hall", "bhmain-1", 1);
        create_role("242", "corp_adm", "41000", "Banquette Hall", "bhmain-2", 2);
        create_role("243", "corp_trainer", "41000", "Banquette Hall", "bhmain-3", 3);
        create_role("244", "corp_trainee", "41000", "Banquette Hall", "bhmain-4", 4);
        create_role("245", "kr_user", "41000", "Banquette Hall", "bhmain-5", 5);
        create_role("246", "conf_user", "41000", "Banquette Hall", "bhmain-6", 6);
        #endregion
        //main module Service------------------------------------
        #region
        create_role("247", "super_adm", "42000", "Service", "sermain-1", 1);
        create_role("248", "corp_adm", "42000", "Service", "sermain-2", 2);
        create_role("249", "corp_trainer", "42000", "Service", "sermain-3", 3);
        create_role("250", "corp_trainee", "42000", "Service", "sermain-4", 4);
        create_role("251", "kr_user", "42000", "Service", "sermain-5", 5);
        create_role("252", "conf_user", "42000", "Service", "sermain-6", 6);
        #endregion
        //Travel Management
        #region
        create_role("253", "super_adm", "43000", "Travel Management", "ttmain-1", 1);
        create_role("254", "corp_adm", "43000", "Travel Management", "ttmain-2", 2);
        create_role("255", "corp_trainer", "43000", "Travel Management", "ttmain-3", 3);
        create_role("256", "corp_trainee", "43000", "Travel Management", "ttmain-4", 4);
        create_role("257", "kr_user", "43000", "Travel Management", "ttmain-5", 5);
        create_role("258", "conf_user", "43000", "Travel Management", "ttmain-6", 6);
        #endregion
        //Quality Inspection-Item
        #region
        create_role("259", "super_adm", "44000", "Quality Inspection-Item", "qcmain-1", 1);
        create_role("260", "corp_adm", "44000", "Quality Inspection-Item", "qcmain-2", 2);
        create_role("261", "corp_trainer", "44000", "Quality Inspection-Item", "qcmain-3", 3);
        create_role("262", "corp_trainee", "44000", "Quality Inspection-Item", "qcmain-4", 4);
        create_role("263", "kr_user", "44000", "Quality Inspection-Item", "qcmain-5", 5);
        create_role("264", "conf_user", "44000", "Quality Inspection-Item", "qcmain-6", 6);
        #endregion
        //Tally Dashboard
        #region
        create_role("265", "super_adm", "45000", "Tally Dashboard", "tldmain-1", 1);
        create_role("266", "corp_adm", "45000", "Tally Dashboard", "tldmain-2", 2);
        create_role("267", "corp_trainer", "45000", "Tally Dashboard", "tldmain-3", 3);
        create_role("268", "corp_trainee", "45000", "Tally Dashboard", "tldmain-4", 4);
        create_role("269", "kr_user", "45000", "Tally Dashboard", "tldmain-5", 5);
        create_role("270", "conf_user", "45000", "Tally Dashboard", "tldmain-6", 6);
        #endregion
        //Vendor Portal
        #region
        create_role("271", "super_adm", "46000", "Vendor Portal", "vdmain-1", 1);
        create_role("272", "corp_adm", "46000", "Vendor Portal", "vdmain-2", 2);
        create_role("273", "corp_trainer", "46000", "Vendor Portal", "vdmain-3", 3);
        create_role("274", "corp_trainee", "46000", "Vendor Portal", "vdmain-4", 4);
        create_role("275", "kr_user", "46000", "Vendor Portal", "vdmain-5", 5);
        create_role("276", "conf_user", "46000", "Vendor Portal", "vdmain-6", 6);
        #endregion
        //Financial Service
        #region
        create_role("283", "super_adm", "48000", "Financial Service", "fsmain-1", 1);
        create_role("284", "corp_adm", "48000", "Financial Service", "fsmain-2", 2);
        create_role("285", "corp_trainer", "48000", "Financial Service", "fsmain-3", 3);
        create_role("286", "corp_trainee", "48000", "Financial Service", "fsmain-4", 4);
        create_role("287", "kr_user", "48000", "Financial Service", "fsmain-5", 5);
        create_role("288", "conf_user", "48000", "Financial Service", "fsmain-6", 6);
        #endregion
    }
    private bool create_role(string r_id, string r_name, string m_id, string m_name, string u_id, int u_level)
    {
        bool data = false;
        if (is_role(r_id) == true)
        {
            data = sgen.execute_cmd(userCode, "insert into role_authority (r_id,r_name,m_id,m_name,u_id,u_level) " +
            "values ('" + r_id + "','" + r_name + "','" + m_id + "','" + m_name + "','" + u_id + "','" + u_level + "')");
            data = true;
            return data;
        }
        else
        {
            data = false;
            return data;
        }
    }
    private bool is_role(string r_id)
    {
        bool result = true;
        string abcd = sgen.getstring(userCode, "select r_id from role_authority where r_id='" + r_id + "'");


        if (abcd == r_id)
        {
            result = false;
            return result;
        }
        else
        {
            result = true;
            return result;
        }
    }
}

public class com_module_class
{
    sgenFun sgen;
    string userCode = "";
    public void insert_com_modules()
    {
        userCode = sgen.getUserCode();
        create_com_module("1", "", "satech", "1000", 1, "1");
        create_com_module("2", "", "satech", "2000", 1, "2");
        create_com_module("3", "", "satech", "3000", 1, "3");
        create_com_module("4", "", "satech", "4000", 1, "4");
        create_com_module("5", "", "satech", "5000", 1, "5");
        create_com_module("6", "", "satech", "6000", 1, "6");
        create_com_module("7", "", "satech", "7000", 1, "7");
        create_com_module("8", "", "satech", "8000", 1, "8");
        create_com_module("9", "", "satech", "9000", 1, "9");
        create_com_module("10", "", "satech", "10000", 1, "10");
        create_com_module("11", "", "satech", "11000", 1, "11");
        create_com_module("12", "", "satech", "12000", 1, "12");
        create_com_module("13", "", "satech", "13000", 1, "13");
        create_com_module("14", "", "satech", "14000", 1, "14");
        create_com_module("15", "", "satech", "15000", 1, "15");
        create_com_module("16", "", "satech", "16000", 1, "16");
        create_com_module("17", "", "satech", "17000", 1, "17");
        create_com_module("18", "", "satech", "18000", 1, "18");
        create_com_module("19", "", "satech", "19000", 1, "19");
        create_com_module("20", "", "satech", "20000", 1, "20");
        create_com_module("21", "", "satech", "21000", 1, "21");
        create_com_module("22", "", "satech", "22000", 1, "22");
        create_com_module("23", "", "satech", "23000", 1, "23");
        create_com_module("24", "", "satech", "24000", 1, "24");
        create_com_module("25", "", "satech", "25000", 1, "25");
        create_com_module("26", "", "satech", "26000", 1, "26");
        create_com_module("27", "", "satech", "27000", 1, "27");
        create_com_module("28", "", "satech", "28000", 1, "28");
        create_com_module("29", "", "satech", "29000", 1, "29");
        create_com_module("30", "", "satech", "30000", 1, "30");
        //create_com_module("31", "", "satech", "31000", 1, "31");
        create_com_module("32", "", "satech", "32000", 1, "32");
        create_com_module("33", "", "satech", "33000", 1, "33");
        create_com_module("34", "", "satech", "34000", 1, "34");
        create_com_module("35", "", "satech", "35000", 1, "35");
        create_com_module("36", "", "satech", "36000", 1, "36");
        create_com_module("37", "", "satech", "37000", 1, "37");
        create_com_module("38", "", "satech", "38000", 1, "38");
        create_com_module("39", "", "satech", "39000", 1, "39");
        create_com_module("40", "", "satech", "40000", 1, "40");
        create_com_module("41", "", "satech", "41000", 1, "41");
        create_com_module("42", "", "satech", "42000", 1, "42");
        create_com_module("43", "", "satech", "43000", 1, "43");
        create_com_module("44", "", "satech", "44000", 1, "44");
        create_com_module("45", "", "satech", "45000", 1, "45");
        create_com_module("46", "", "satech", "46000", 1, "46");
        create_com_module("47", "", "satech", "47000", 1, "47");
        create_com_module("48", "", "satech", "48000", 1, "48");
    }
    private bool create_com_module(string cm_id, string com_id, string com_code, string mod_id, int mod_status, string vch_num)
    {
        bool data = false;
        if (is_com_module(cm_id) == true)
        {
            data = sgen.execute_cmd(userCode, "insert into com_module (cm_id,com_id,com_code,mod_id,mod_status,vch_num) " +
            "values ('" + cm_id + "','" + com_id + "','" + com_code + "','" + mod_id + "','" + mod_status + "','" + vch_num + "')");
            data = true;
            return data;
        }
        else { data = false; return data; }
    }
    private bool is_com_module(string cm_id)
    {
        bool result = true;
        string abcd = sgen.getstring(userCode, "select cm_id from com_module where cm_id='" + cm_id + "'");
        if (abcd == cm_id) { result = false; return result; }
        else { result = true; return result; }
    }
}
