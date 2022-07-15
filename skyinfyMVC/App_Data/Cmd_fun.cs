using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

using System.Web.Mvc;
using skyinfyMVC.Models;

public class Cmd_Fun
{
    sgenFun sgen;
    public string mq = "", cmd = "", mq0 = "";

    public Cmd_Fun(string Myguid)
    {
        Multiton multiton = Multiton.GetInstance(Myguid);
        sgen = new sgenFun(Myguid);
    }

    //what to do
    #region drop down session

    public List<SelectListItem> Modeofpayment(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "Modeofpayment_u_" + usercode + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {

            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'EPM' ");
            if (HttpContext.Current.Application["Rpt_Menuid"] != null)
            {
                if (HttpContext.Current.Application["Rpt_Menuid"].ToString() == "7019.6")
                {
                    dt = sgen.getdata(usercode, "select 'Concession' as master_id,'Concession' as master_name from dual ");
                }
            }
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
        }

        return sgen.dt_to_selectlist(dt);
    }


    public List<SelectListItem> dept(String usercode, string clientid_mst, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "dept_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'MDP' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'MDP' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }
      public List<SelectListItem> aff_auth(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "aff_auth_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'AAM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'AAM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }
    
      public List<SelectListItem> ddl_division(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "ddl_division_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'DIV' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'DIV' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }
    
      public List<SelectListItem> ddl_type(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "ddl_type_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'TYP' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'TYP' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }


    public List<SelectListItem> dept(String usercode, string unitid_mst, out string defval)
    {
        string val = "";
        DataTable dt = new DataTable();
        string sessionname = "dept_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting where type = 'MDP' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting where type = 'MDP' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        try
        {
            defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
        }
        catch (Exception ex)
        {
            defval = "0";
        }
        return sgen.dt_to_selectlist(dt);
    }


    public List<SelectListItem> caste(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "caste_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'ECT' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'ECT' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }

        return sgen.dt_to_selectlist(dt);
    }
    public List<SelectListItem> conf_room(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "conf_room_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "select conf_roomid,conf_roomname from conference_room where client_unit_id='" + unitid_mst + "' and type='CRD'");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "select conf_roomid,conf_roomname from conference_room where client_unit_id='" + unitid_mst + "' and type='CRD'");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }

        return sgen.dt_to_selectlist(dt);
    }
    public List<SelectListItem> hospitality(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "hospitality_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "select master_id,master_name from master_setting where type='CRS' and classid='001' and sectionid ='Hospitality Service'");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "select master_id,master_name from master_setting where type='CRS' and classid='001' and sectionid ='Hospitality Service'");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }

        return sgen.dt_to_selectlist(dt);
    }
    public List<SelectListItem> it_services(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "it_services_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "select master_id,master_name from master_setting where type='CRS' and classid='002' and sectionid ='It Service'");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "select master_id,master_name from master_setting where type='CRS' and classid='002' and sectionid ='It Service'");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }

        return sgen.dt_to_selectlist(dt);
    }


    public List<SelectListItem> funtype(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "funtype_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'FUN' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'FUN' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }

        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> sertype(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "sertype_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'TBS' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'TBS' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }

        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> menustype(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "menustype_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'TBM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'TBM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }

        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> legislationstype(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "legislationstype_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'LEA' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'LEA' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }

        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> compliancetype(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "compliancetype_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'COT' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'COT' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }

        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> statauttype(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "statauttype_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'SAN' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'SAN' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }

        return sgen.dt_to_selectlist(dt);
    }
      public List<SelectListItem> criticality(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "criticality_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'CMT' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'CMT' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }

        return sgen.dt_to_selectlist(dt);
    }
    
      public List<SelectListItem> priority(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "priority_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'PMT' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'PMT' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }

        return sgen.dt_to_selectlist(dt);
    }
    
      public List<SelectListItem> rule_name(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "rule_name_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'RNM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'RNM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }

        return sgen.dt_to_selectlist(dt);
    }
    
      public List<SelectListItem> act_name(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "act_name_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'NAC' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'NAC' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }

        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> freqtype(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "freqtype_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'FRE' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'FRE' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }

        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> caste_cate(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "caste_cate_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'CAS' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'CAS' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }

        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> religion(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "religion_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type='ERT' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type='ERT' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }
    public List<SelectListItem> pickdrop(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "pickdrop_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'LOC' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'LOC' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }

        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> desig(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "desig_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null) || (dt.Rows.Count == 0))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'MDG' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'MDG' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }

        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> desig(String usercode, string unitid_mst, out string defval)
    {
        string val = "";
        DataTable dt = new DataTable();
        string sessionname = "desig_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting where type = 'MDG' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;

        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting where type = 'MDG' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        try
        {
            defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
        }
        catch (Exception ex)
        {
            defval = "0";
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> emptype(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "emptype_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'KET' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'KET' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }

        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> emptype(String usercode, string unitid_mst, out string defval)
    {
        string val = "";
        DataTable dt = new DataTable();
        string sessionname = "emptype_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting where type = 'KET' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting where type = 'KET' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        try
        {
            defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
        }
        catch (Exception ex)
        {
            defval = "0";
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> month(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "month_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'MNT' order by femaleratio");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'MNT' order by femaleratio");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }

        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> hobby(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "hobby_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'HBY' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'HBY' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }
     public List<SelectListItem> asset_type(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "asset_type_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'ATY' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'ATY' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }
    
     public List<SelectListItem> help_type(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "help_type_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'HTM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'HTM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> lang(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "lang_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'ATY' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'ATY' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }

        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> book_ctgry(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "book_ctgry_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'BCM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'BCM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }

        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> lib_subject(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "lib_subject_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'ESW' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'ESW' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }

        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> issue_aginst(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "issue_aginst_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'BAC' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'BAC' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }

        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> lib_class(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "lib_class_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "select add_class_id as Class_id,class,cast(sequence as int) from add_class where type='EAC' and (status='Y' or status='') and find_in_set(client_unit_id,'" + unitid_mst + "')=1  order by 3");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "select add_class_id as Class_id,class,cast(sequence as int) from add_class where type='EAC' and (status='Y' or status='') and find_in_set(client_unit_id,'" + unitid_mst + "')=1  order by 3");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }

        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> partytype(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "partytype_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'PT1' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'PT1' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }

        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> partytype(String usercode, string unitid_mst, out string defval)
    {
        string val = "";
        DataTable dt = new DataTable();
        string sessionname = "partytype_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting where type = 'PT1' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
          
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting where type = 'PT1' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
               
            }
           
        }
        try
        {
            defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
           
        }
        catch (Exception ex)
        {
            defval = "0";
            
        }
        
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> bank(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "bank_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'ABD' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'ABD' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }

        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> acctype(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "acctype_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'KAC' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'KAC' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }

        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> acctypevdm(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "acctypevdm_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'BTM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'BTM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> ddl_vendor(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "ddl_vendor_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            //dt = sgen.getdata(usercode, "select '000000' vch_num,'Self' c_name from dual union all select vch_num, c_name from clients_mst where type='PVD' and contr='Y' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
            dt = sgen.getdata(usercode, "select '000000' vch_num,'Self' c_name from dual union all select vch_num, c_name from clients_mst where type='BCD' and substr(vch_num,0,3)='203' and contr='Y' and client_unit_id='" + unitid_mst + "'");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "select '000000' vch_num,'Self' c_name from dual union all select vch_num, c_name from clients_mst where type='BCD' and substr(vch_num,0,3)='203' and contr='Y' and client_unit_id='" + unitid_mst + "'");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }

        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> empcat(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "empcat_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'KEC' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'KEC' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> empcat(String usercode, string unitid_mst, out string defval)
    {
        string val = "";
        DataTable dt = new DataTable();
        string sessionname = "empcat_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting  where type = 'KEC' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
            try
            {
                defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                val = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
            }
            catch (Exception ex)
            {
                defval = "0";
                val = "0";
            }
            HttpContext.Current.Application[sessionname + "_val"] = val;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting  where type = 'KEC' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
                try
                {
                    defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                    val = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                }
                catch (Exception ex)
                {
                    defval = "0";
                    val = "0";
                }
                HttpContext.Current.Application[sessionname + "_val"] = val;
            }
            else
            {
                val = HttpContext.Current.Application[sessionname + "_val"].ToString();
                defval = val;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> country(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "country_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "select DISTINCT alpha_2,country_name from country_state order by country_name");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "select DISTINCT alpha_2,country_name from country_state order by country_name");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }

        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> curname(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "curname";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id, upper(master_name) master_name FROM master_setting  where type = 'CTM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id, upper(master_name) master_name FROM master_setting  where type = 'CTM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    //left
    public List<SelectListItem> fwdcurname(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "fwdcurname_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id, upper(master_name) master_name FROM master_setting  where type = 'FCM'");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id, upper(master_name) master_name FROM master_setting  where type = 'FCM'");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }
    public List<SelectListItem> payterm2(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "payterm2_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id, upper(master_name) master_name FROM master_setting  where type = 'PT2' and client_unit_id='"+ unitid_mst + "'");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id, upper(master_name) master_name FROM master_setting  where type = 'PT2' and client_unit_id='" + unitid_mst + "'");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> payterm(String usercode, string unitid_mst, out string defval)
    {
        string val = "";
        DataTable dt = new DataTable();
        string sessionname = "payterm_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting  where type = 'PTM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
            try
            {
                defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                val = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
            }
            catch (Exception ex)
            {
                defval = "0";
                val = "0";
            }
            HttpContext.Current.Application[sessionname + "_val"] = val;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting  where type = 'PTM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
                try
                {
                    defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                    val = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                }
                catch (Exception ex)
                {
                    defval = "0";
                    val = "0";
                }
                HttpContext.Current.Application[sessionname + "_val"] = val;
            }
            else
            {
                val = HttpContext.Current.Application[sessionname + "_val"].ToString();
                defval = val;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> gin_no(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "gin_no_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "select (client_id||vch_num||type) as master_id,vch_num as master_name from kc_tab where type = 'GIN' and client_unit_id = '" + unitid_mst + "' " +
                "and client_id||vch_num||type not in (select gatein_no from itransaction where type" +
                " in ('01', '03', '02', '05', '09') and client_unit_id = '" + unitid_mst + "' and gatein_no " +
                "is not null and gatein_no <> '0') and client_id||vch_num||type not in (select client_id || col6 || col5 from kc_tab where type = 'PGN' and client_unit_id = '" + unitid_mst + "')");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "select (client_id||vch_num||type) as master_id,vch_num as master_name from kc_tab where type = 'GIN' and client_unit_id = '" + unitid_mst + "' " +
    "and client_id||vch_num||type not in (select gatein_no from itransaction where type" +
    " in ('01', '03', '02', '05', '09') and client_unit_id = '" + unitid_mst + "' and gatein_no " +
    "is not null and gatein_no <> '0') and client_id||vch_num||type not in (select client_id || col6 || col5 from kc_tab where type = 'PGN' and client_unit_id = '" + unitid_mst + "')");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }
    public List<SelectListItem> gin_no2(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "gin_no2_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "select distinct (client_id||vch_num||type) as master_id,vch_num as master_name from kc_tab where type = 'GIN' and client_unit_id = '" + unitid_mst + "'");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "select distinct (client_id||vch_num||type) as master_id,vch_num as master_name from kc_tab where type = 'GIN' and client_unit_id = '" + unitid_mst + "'");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }
    public List<SelectListItem> getshft(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "getshft_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting WHERE Type='STM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting WHERE Type='STM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }
    public List<SelectListItem> paycomp(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "paycomp_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "select col1,vch_num from vehicle_master where type='KSP' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "select col1,vch_num from vehicle_master where type='KSP' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> transportmode(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "transportmode_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'AMT' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'AMT' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }
    public List<SelectListItem> sales_channel(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "sales_channel_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'SCM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'SCM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> bind_doc(String usercode, string clientid_mst, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "bind_doc_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "select master_id,master_name from master_setting where master_type='VIT' and find_in_set(client_id,'" + clientid_mst + "')=1 and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "select master_id,master_name from master_setting where master_type='VIT' and find_in_set(client_id,'" + clientid_mst + "')=1 and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }
    public List<SelectListItem> assignment(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "assignment_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "select master_id,master_name from master_setting where type='ASG' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "select master_id,master_name from master_setting where type='ASG' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }
    public List<SelectListItem> traveltype(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "traveltype_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "select master_id,master_name from master_setting where type='TTM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "select master_id,master_name from master_setting where type='TTM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }
    public List<SelectListItem> travelclass(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "travelclass_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "select master_id,master_name from master_setting where type='TCM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "select master_id,master_name from master_setting where type='TCM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }
    public List<SelectListItem> separation_reason(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "separation_reason_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "select master_id,master_name from master_setting where type='SRM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "select master_id,master_name from master_setting where type='SRM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }
    public List<SelectListItem> occtype(String usercode, string unitid_mst, out string defval)
    {
        string val = "";
        DataTable dt = new DataTable();
        string sessionname = "occtype_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting  where type = 'OCC' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
            try
            {
                defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                val = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
            }
            catch (Exception ex)
            {
                defval = "0";
                val = "0";
            }
            HttpContext.Current.Application[sessionname + "_val"] = val;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting  where type = 'OCC' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
                try
                {
                    defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                    val = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                }
                catch (Exception ex)
                {
                    defval = "0";
                    val = "0";
                }
                HttpContext.Current.Application[sessionname + "_val"] = val;
            }
            else
            {
                val = HttpContext.Current.Application[sessionname + "_val"].ToString();
                defval = val;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> salearea(String usercode, string unitid_mst, out string defval)
    {
        string val = "";
        DataTable dt = new DataTable();
        string sessionname = "salearea_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting  where type = 'SSA' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;

            try
            {
                defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                val = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
            }
            catch (Exception ex)
            {
                defval = "0";
                val = "0";
            }
            HttpContext.Current.Application[sessionname + "_val"] = val;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting  where type = 'SSA' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;

                try
                {
                    defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                    val = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                }
                catch (Exception ex)
                {
                    defval = "0";
                    val = "0";
                }
                HttpContext.Current.Application[sessionname + "_val"] = val;
            }
            else
            {
                val = HttpContext.Current.Application[sessionname + "_val"].ToString();
                defval = val;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    //public List<SelectListItem> salearea(String usercode, string unitid_mst)
    //{
    //    DataTable dt = new DataTable();
    //    string sessionname = "salearea";
    //    if ((HttpContext.Current.Application[sessionname] == null)|| (dt.Rows.Count==0))
    //    {

    //        dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'SSA' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
    //        HttpContext.Current.Application[sessionname] = dt;

    //        //defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
    //        //HttpContext.Current.Application[sessionname + "_val"] = defval;
    //    }
    //    else
    //    {
    //        dt = ((DataTable)HttpContext.Current.Application[sessionname]);
    //        //defval = HttpContext.Current.Application[sessionname + "_val"].ToString();
    //    }
    //    return sgen.dt_to_selectlist(dt);
    //}

    public List<SelectListItem> protype(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "protype_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'PRT' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'PRT' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    //left
    public List<SelectListItem> itemgrp(String usercode, string clientid_mst, string unitid_mst)
    {
        DataTable dt = new DataTable();
        //string sessionname = "itemgrp_u_" + usercode + "_" + unitid_mst + "";
        //if ((HttpContext.Current.Application[sessionname] == null))
        //{
            //dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'KIG'");
            dt = sgen.getdata(usercode, "SELECT mg.master_id,(mg.master_name||' ('||mg.master_id||'-'||gp.master_name||')') FROM master_setting " +
                "mg inner join master_setting gp on gp.classid = substr(mg.master_id, 1, 1) and gp.type = 'KGP' where mg.type = 'KIG' and find_in_set(mg.client_id,'" + clientid_mst + "')=1 and find_in_set(mg.client_unit_id,'" + unitid_mst + "')=1");
            //HttpContext.Current.Application[sessionname] = dt;
        //}
        //else
        //{
    //        dt = ((DataTable)HttpContext.Current.Application[sessionname]);
    //        if (dt == null || dt.Rows.Count == 0)
    //        {
    //            dt = sgen.getdata(usercode, "SELECT mg.master_id,(mg.master_name||' ('||mg.master_id||'-'||gp.master_name||')') FROM master_setting " +
    //"mg inner join master_setting gp on gp.classid = substr(mg.master_id, 1, 1) and gp.type = 'KGP' where mg.type = 'KIG' and find_in_set(mg.client_id,'" + clientid_mst + "')=1 and find_in_set(mg.client_unit_id,'" + unitid_mst + "')=1");
    //            HttpContext.Current.Application[sessionname] = dt;
    //        }
        //}
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> relation(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "relation_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'REL' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'REL' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    //kc
    //public List<SelectListItem> packtype(String usercode, string clientid_mst, string unitid_mst)
    //{
    //    DataTable dt = new DataTable();
    //    string sessionname = "packtype";
    //    if ((HttpContext.Current.Application[sessionname] == null)|| (dt.Rows.Count==0))
    //    {
    //        dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'KPK' and find_in_set(client_id,'" + clientid_mst + "')=1 and " +
    //            "find_in_set(client_unit_id,'" + unitid_mst + "')=1");
    //        HttpContext.Current.Application[sessionname] = dt;
    //    }
    //    else
    //    {
    //        dt = ((DataTable)HttpContext.Current.Application[sessionname]);
    //    }
    //    return sgen.dt_to_selectlist(dt);
    //}

    //kc
    public List<SelectListItem> packtype(String usercode, string unitid_mst, out string defval)
    {
        string val = "";
        DataTable dt = new DataTable();
        string sessionname = "packtype_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting where type = 'KPK' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
            try
            {
                defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                val = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
            }
            catch (Exception ex)
            {
                defval = "0";
                val = "0";
            }
            HttpContext.Current.Application[sessionname + "_val"] = val;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting where type = 'KPK' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
                try
                {
                    defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                    val = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                }
                catch (Exception ex)
                {
                    defval = "0";
                    val = "0";
                }
                HttpContext.Current.Application[sessionname + "_val"] = val;
            }
            else
            {
                val = HttpContext.Current.Application[sessionname + "_val"].ToString();
                defval = val;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }


    public List<SelectListItem> clienttype(String usercode, string unitid_mst, out string defval)
    {
        string val = "";
        DataTable dt = new DataTable();
        string sessionname = "clienttype_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting  where type = 'CLI' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
            try
            {
                defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                val = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
            }
            catch (Exception ex)
            {
                defval = val = "0";
            }
            HttpContext.Current.Application[sessionname + "_val"] = val;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting  where type = 'CLI' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
                try
                {
                    defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                    val = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                }
                catch (Exception ex)
                {
                    defval = val = "0";
                }
                HttpContext.Current.Application[sessionname + "_val"] = val;
            }
            else
            {
                val = HttpContext.Current.Application[sessionname + "_val"].ToString();
                defval = val;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> addresstype(String usercode, string unitid_mst, out string defval)
    {
        string val = "";
        DataTable dt = new DataTable();
        string sessionname = "addresstype_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting  where type = 'TOA' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
            try
            {
                defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                val = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
            }
            catch (Exception ex)
            {
                defval = val = "0";
            }
            HttpContext.Current.Application[sessionname + "_val"] = val;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting  where type = 'TOA' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
                try
                {
                    defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                    val = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                }
                catch (Exception ex)
                {
                    defval = val = "0";
                }
                HttpContext.Current.Application[sessionname + "_val"] = val;
            }
            else
            {
                val = HttpContext.Current.Application[sessionname + "_val"].ToString();
                defval = val;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> leadsource(String usercode, string unitid_mst, out string defval)
    {
        string val = "";
        DataTable dt = new DataTable();
        string sessionname = "leadsource_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting where type = 'SRC' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
            try
            {
                defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                val = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
            }
            catch (Exception ex)
            {
                defval = val = "0";
            }
            HttpContext.Current.Application[sessionname + "_val"] = val;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting where type = 'SRC' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
                try
                {
                    defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                    val = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                }
                catch (Exception ex)
                {
                    defval = val = "0";
                }
                HttpContext.Current.Application[sessionname + "_val"] = val;
            }
            else
            {
                val = HttpContext.Current.Application[sessionname + "_val"].ToString();
                defval = val;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    //public List<SelectListItem>leadsource(String usercode, string unitid_mst)
    //{
    //    DataTable dt = new DataTable();
    //    string sessionname = "leadsource";
    //    if ((HttpContext.Current.Application[sessionname] == null)|| (dt.Rows.Count==0))
    //    {

    //        dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'SRC' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
    //        HttpContext.Current.Application[sessionname] = dt;
    //    }
    //    else
    //    {
    //        dt = ((DataTable)HttpContext.Current.Application[sessionname]);
    //    }
    //    return sgen.dt_to_selectlist(dt);
    //}

    public List<SelectListItem> leadstatus(String usercode, string unitid_mst, out string defval)
    {
        string val = "";
        DataTable dt = new DataTable();
        string sessionname = "leadstatus_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting where type = 'LST' and find_in_set(client_unit_id,'" + unitid_mst + "')=1 order by sectiontype");
            HttpContext.Current.Application[sessionname] = dt;
            try
            {
                defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                val = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
            }
            catch (Exception ex)
            {
                defval = val = "0";
            }
            HttpContext.Current.Application[sessionname + "_val"] = val;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting where type = 'LST' and find_in_set(client_unit_id,'" + unitid_mst + "')=1 order by sectiontype");
                HttpContext.Current.Application[sessionname] = dt;
                try
                {
                    defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                    val = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                }
                catch (Exception ex)
                {
                    defval = val = "0";
                }
                HttpContext.Current.Application[sessionname + "_val"] = val;
            }
            else
            {
                val = HttpContext.Current.Application[sessionname + "_val"].ToString();
                defval = val;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    //public List<SelectListItem> leadstatus(String usercode, string unitid_mst)
    //{
    //    DataTable dt = new DataTable();
    //    string sessionname = "leadstatus";
    //    if ((HttpContext.Current.Application[sessionname] == null)|| (dt.Rows.Count==0))
    //    {

    //        dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'LST' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
    //        HttpContext.Current.Application[sessionname] = dt;
    //    }
    //    else
    //    {
    //        dt = ((DataTable)HttpContext.Current.Application[sessionname]);
    //    }
    //    return sgen.dt_to_selectlist(dt);
    //}

    public List<SelectListItem> product(String usercode, string unitid_mst, out string defval)
    {
        string val = "";
        DataTable dt = new DataTable();
        string sessionname = "product_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting where type = 'PNM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
            try
            {
                defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                val = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
            }
            catch (Exception ex)
            {
                defval = val = "0";
            }
            HttpContext.Current.Application[sessionname + "_val"] = val;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting where type = 'PNM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
                try
                {
                    defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                    val = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                }
                catch (Exception ex)
                {
                    defval = val = "0";
                }
                HttpContext.Current.Application[sessionname + "_val"] = val;
            }
            else
            {
                val = HttpContext.Current.Application[sessionname + "_val"].ToString();
                defval = val;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    //public List<SelectListItem> product(String usercode, string unitid_mst)
    //{
    //    DataTable dt = new DataTable();
    //    string sessionname = "product";
    //    if ((HttpContext.Current.Application[sessionname] == null)|| (dt.Rows.Count==0))
    //    {

    //        dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'PNM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
    //        HttpContext.Current.Application[sessionname] = dt;
    //    }
    //    else
    //    {
    //        dt = ((DataTable)HttpContext.Current.Application[sessionname]);
    //    }
    //    return sgen.dt_to_selectlist(dt);
    //}

    public List<SelectListItem> nextact(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "nextact_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'NAM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'NAM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    //kc
    public List<SelectListItem> unitmeas(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "unitmeas_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'UMM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;

            //defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
            //HttpContext.Current.Application[sessionname + "_val"] = defval;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'UMM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
            //defval = HttpContext.Current.Application[sessionname + "_val"].ToString();
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> unitmeas(String usercode, string unitid_mst, out string defval)
    {
        string val = "";
        DataTable dt = new DataTable();
        string sessionname = "unitmeas_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting where type = 'UMM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
            try
            {
                defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                val = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
            }
            catch (Exception ex)
            {
                defval = val = "0";
            }
            HttpContext.Current.Application[sessionname + "_val"] = val;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting where type = 'UMM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
                try
                {
                    defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                    val = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                }
                catch (Exception ex)
                {
                    defval = val = "0";
                }
                HttpContext.Current.Application[sessionname + "_val"] = val;
            }
            else
            {
                val = HttpContext.Current.Application[sessionname + "_val"].ToString();
                defval = val;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    //kc
    public List<SelectListItem> hsn(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "hsn_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'HSN' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'HSN' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> break_upreason(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "break_upreason_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "select master_id ,master_name from master_setting where type='BRM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "select master_id ,master_name from master_setting where type='BRM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> prodallstage(String usercode, string unitid_mst)
    {
        mq = "";
        DataTable dt = new DataTable();
        string sessionname = "prodallstage_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            mq = "SELECT master_id, master_name FROM master_setting where type = 'KPS' and find_in_set(client_unit_id,'" + unitid_mst + "')= 1 and master_id<>'001' " +
                "union all select '001' master_id,'Soft Floor' master_name from dual union all select '100' master_id,'PDI' master_name from dual " +
                "union all select '101' master_id,'Rejection' master_name from dual";
            dt = sgen.getdata(usercode, mq);
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                mq = "SELECT master_id, master_name FROM master_setting where type = 'KPS' and find_in_set(client_unit_id,'" + unitid_mst + "')= 1 and master_id<>'001' " +
                "union all select '001' master_id,'Soft Floor' master_name from dual union all select '100' master_id,'PDI' master_name from dual " +
                "union all select '101' master_id,'Rejection' master_name from dual";
                dt = sgen.getdata(usercode, mq);
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> prodstage(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "prodstage_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'KPS' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'KPS' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> prodtostage(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "prodtostage_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null) || (dt.Rows.Count == 0))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'KPS' and find_in_set(client_unit_id,'" + unitid_mst + "')=1 and master_id<>'001'");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'KPS' and find_in_set(client_unit_id,'" + unitid_mst + "')=1 and master_id<>'001'");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> priceterm(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "priceterm_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'PRI' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'PRI' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> priceterm(String usercode, string unitid_mst, out string defval)
    {
        DataTable dt = new DataTable();
        string sessionname = "priceterm_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting  where type = 'PRI' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
            defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
            HttpContext.Current.Application[sessionname + "_val"] = defval;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting  where type = 'PRI' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
                defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                HttpContext.Current.Application[sessionname + "_val"] = defval;
            }
            else
            {
                defval = HttpContext.Current.Application[sessionname + "_val"].ToString();
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> hsn(String usercode, string unitid_mst, out string defval)
    {
        DataTable dt = new DataTable();
        string sessionname = "hsn_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'HSN' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
            defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
            HttpContext.Current.Application[sessionname + "_val"] = defval;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'HSN' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
                defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                HttpContext.Current.Application[sessionname + "_val"] = defval;
            }
            else
            {
                defval = HttpContext.Current.Application[sessionname + "_val"].ToString();
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> leavereas(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "leavereas_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'ELM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'ELM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> qualification(String usercode, string unitid_mst, out string defval)
    {
        string val = "";
        DataTable dt = new DataTable();
        string sessionname = "qualification_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting where type = 'EQU' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
            try
            {
                defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                val = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
            }
            catch (Exception ex)
            {
                defval = "0";
                val = "0";
            }
            HttpContext.Current.Application[sessionname + "_val"] = val;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting where type = 'EQU' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
                try
                {
                    defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                    val = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                }
                catch (Exception ex)
                {
                    defval = "0";
                    val = "0";
                }
                HttpContext.Current.Application[sessionname + "_val"] = val;
            }
            else
            {
                val = HttpContext.Current.Application[sessionname + "_val"].ToString();
                defval = val;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> annincome(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "annincome";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'KAI' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'KAI' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> govtgrantsch(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "govtgrantsch_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "Select vch_num,col6 from enx_tab where type = 'GGM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "Select vch_num,col6 from enx_tab where type = 'GGM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> assminorhead(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "assminorhead_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "Select vch_num,col1 from enx_tab where type = 'AMH' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "Select vch_num,col1 from enx_tab where type = 'AMH' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> asscostcenter(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "asscostcenter_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'FAC' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'FAC' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> exptype(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "exptype_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "select '001' as Expense_id ,'Type1' as Expense from dual union all select '002' as Expense_id ,'Type2' as Expense from dual");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "select '001' as Expense_id ,'Type1' as Expense from dual union all select '002' as Expense_id ,'Type2' as Expense from dual");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    //kc
    public List<SelectListItem> st_cond(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "st_cond_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'KSC' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
            //defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
            //HttpContext.Current.Application[sessionname + "_val"] = defval;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'KSC' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
            //defval = HttpContext.Current.Application[sessionname + "_val"].ToString();
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> tr_mod(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "tr_mod_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT mod_id,mod_name from module where type='TMD' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT mod_id,mod_name from module where type='TMD' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }
    public List<SelectListItem> gettitle(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "gettitle_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "select course_id,cou_title from courses where type='TCR' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "select course_id,cou_title from courses where type='TCR' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }
    public List<SelectListItem> getcatagory(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "getcatagory_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "select cat_id,cat_name from category where type='TCT' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "select cat_id,cat_name from category where type='TCT' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }


    #endregion
    //what to do
    #region Party command
    public string showparty(string unitid_mst)
    {
        cmd = "select distinct (a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr,b.vch_num as Parent_code,b.c_name as Parent_Account," +
          "a.vch_num as account_code,a.c_name as Account_Name,a.type_desc as Search_text," +
          "a.cpname as PerName,(case when a.cpcont='0000000000' then '' else a.cpcont end)  as CP_No, 'Account Detail' as Data_Source from clients_mst a left join clients_mst b on a.parent_id=b.vch_num " +
          " and a.client_unit_id=b.client_unit_id and b.type='BCD' where a.type in ('BCD') and a.client_unit_id='" + unitid_mst + "' and substr(a.vch_num,0,3)='303' and a.mftr<>'Y'";

        return cmd;
    }
    public string showvendor(string unitid_mst)
    {
        cmd = "select distinct (a.client_id||a.client_unit_id||a.vch_num||to_char(a.vch_date,'yyyymmdd')||a.type) as fstr,b.vch_num as Parent_code,b.c_name as Parent_Account," +
          "a.vch_num as account_code,a.c_name as Account_Name,a.type_desc as Search_text," +
          "a.cpname as PerName,(case when a.cpcont='0000000000' then '' else a.cpcont end)  as CP_No, 'Account Detail' as Data_Source from clients_mst a left join clients_mst b on a.parent_id=b.vch_num " +
          " and a.client_unit_id=b.client_unit_id and b.type='BCD' where a.type in ('BCD') and a.client_unit_id='" + unitid_mst + "' and substr(a.vch_num,0,3)='203' ";

        return cmd;
    }
    #endregion
    #region Party command
    public string getparty(string URL)
    {
        mq = "select cp.vch_num,cp.c_name  from clients_mst cp " +
                             "where cp.client_id|| cp.client_unit_id|| cp.vch_num|| to_char(cp.vch_date, 'yyyymmdd')|| cp.type='" + URL + "'";

        return mq;
    }
    #endregion
    //what to do
    public DataTable GetStructure(string usercode, string Tabname)
    {
        DataTable dt = new DataTable();
        //string sessionname = "structure" + Tabname;
        //if (HttpContext.Current.Application[sessionname] == null || ((DataTable)HttpContext.Current.Application[sessionname]) == null)
        //{
        dt = sgen.getdata(usercode, "select * from " + Tabname + " where 1=2");
        //HttpContext.Current.Application[sessionname] = dt;
        //}
        //else
        //{
        //    dt = ((DataTable)HttpContext.Current.Application[sessionname]).Clone();
        //}
        return dt;
    }
    //what to do
    public bool Frequency(String usercode, string clientid_mst, string unitid_mst, string userid_mst, string vch_num, string master_id, string master_name)
    {
        bool Result = false, data = false;
        string currdate = sgen.server_datetime_local(usercode);
        currdate = sgen.Savedate(currdate, true);
        #region Create Frequency


        string feeheadid = sgen.getstring(usercode, "select master_id from master_setting where type='EFQ'and master_name='" + master_name + "'");

        if (feeheadid == "")
        {

            DataTable dataTable = sgen.getdata(usercode, "select  * from master_setting  where  1=2");

            DataRow dr = dataTable.NewRow();
            dr["rec_id"] = "0";
            dr["vch_num"] = vch_num;
            dr["vch_date"] = currdate;
            dr["type"] = "EFQ";
            dr["master_type"] = "Education Fees Head";
            dr["master_id"] = master_id;
            dr["master_name"] = master_name;
            dr["subjectid"] = 1;//Active or deactive                                
            try
            {
                dr["master_entby"] = userid_mst;
                dr["master_entdate"] = currdate;
                dr["master_editby"] = '-';
                dr["master_editdate"] = currdate;
            }
            catch (Exception ex)
            { }

            dataTable.Rows.Add(dr);
            Result = sgen.Update_data(usercode, dataTable, "master_setting", "", false);
        }
        #endregion

        return true;
    }
    //what to do
    public bool PreHead(String UserCode, string clientid_mst, string unitid_mst, string userid_mst, string FeeType)
    {
        string currdate = sgen.server_datetime(UserCode);
        currdate = sgen.Savedate(currdate, true);

        #region Create Feetype
        string id = sgen.genNo(UserCode, "select max(to_number (feetype_id)) as max from school_Fee_Type where  type='0'" +
        " and client_unit_id='" + unitid_mst + "' ", 3, "max");

        string vch_num = sgen.genNo(UserCode, "select max(to_number (vch_num)) as max from school_Fee_Type where   type='0'" +
       " and client_unit_id='" + unitid_mst + "' ", 6, "max");




        string chk_feetype = sgen.getstring(UserCode, "select FeeTypeName from school_Fee_Type where FeeTypeName='" + FeeType + "'  and client_unit_id='" + unitid_mst + "' and type='0'");
        if (chk_feetype == "")
        {
            DataTable dataTable = sgen.getdata(UserCode, "select  * from school_Fee_Type  where  1=2");

            DataRow dr = dataTable.NewRow();
            dr["rec_id"] = "0";
            dr["vch_num"] = vch_num;
            dr["vch_date"] = currdate;
            dr["type"] = "";
            dr["FeeType_Id"] = id;
            dr["FeeTypeName"] = FeeType;
            dr["IsPredefined"] = "1";//                            
            dr["client_id"] = clientid_mst;
            dr["client_unit_id"] = unitid_mst;
            dr["Ent_By"] = userid_mst;
            dr["Ent_date"] = currdate;
            dr["Edit_By"] = '-';
            dr["Edit_Date"] = currdate;


            dataTable.Rows.Add(dr);
            bool Result = sgen.Update_data(UserCode, dataTable, "school_Fee_Type", "", false);
        }
        #endregion

        if (FeeType != "Admission")
        {
            #region Create FineHead


            string feetypeid = id;

            string feeheadid = sgen.getstring(UserCode, "select master_id from master_setting where type='EFH'and master_name='" + FeeType + "'" +
                " and  client_unit_id='" + unitid_mst + "' and master_type='Education Fees Head' and sectionid " +
                "= (SELECT distinct FeeType_Id FROM  school_fee_type where   client_unit_id='" + unitid_mst + "'" +
                " and type='0' and feetypename='" + FeeType + "' )");

            if (feeheadid == "")
            {
                string master_id = sgen.genNo(UserCode, "select max(to_number (vch_num)) as max from master_setting where type='EFH' " +
                    " and client_unit_id='" + unitid_mst + "' and master_type='Education Fees Head' and sectionid= (SELECT distinct FeeType_Id FROM  school_fee_type where" +
                    "   client_unit_id='" + unitid_mst + "' and type='' and feetypename='" + FeeType + "' )", 3, "max");
                vch_num = sgen.genNo(UserCode, "select max(to_number (vch_num)) as max from master_setting where type='EFH' " +
                   " and client_unit_id='" + unitid_mst + "' and master_type='Education Fees Head' and sectionid= (SELECT distinct FeeType_Id FROM  school_fee_type where" +
                   "   client_unit_id='" + unitid_mst + "' and type='' and feetypename='" + FeeType + "' )", 6, "max");



                DataTable dataTable = sgen.getdata(UserCode, "select * from master_setting where  1=2");

                DataRow dr = dataTable.NewRow();
                dr["rec_id"] = "0";
                dr["vch_num"] = vch_num;
                dr["vch_date"] = currdate;
                dr["type"] = "EFH";
                dr["master_type"] = "Education Fees Head";
                dr["master_id"] = master_id;
                dr["master_name"] = FeeType;
                dr["sectionid"] = feetypeid;//Fee Type
                dr["subjectid"] = 1;//Active or deactive
                dr["Section_Strength"] = "B"; //Fee for which type ofstudent                       
                dr["client_id"] = clientid_mst;
                dr["client_unit_id"] = unitid_mst;



                try
                {


                    dr["master_entby"] = userid_mst;
                    dr["master_entdate"] = currdate;
                    dr["master_editby"] = '-';
                    dr["master_editdate"] = currdate;

                }
                catch (Exception ex)
                { }





                dataTable.Rows.Add(dr);
                bool Result = sgen.Update_data(UserCode, dataTable, "master_setting", "", false);
            }


            #endregion
        }
        return true;
    }

    public List<SelectListItem> itemlocation(String usercode, string clientid_mst, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "itemlocation-" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            mq = "select (a.client_id||a.client_unit_id||b.master_id||f.master_id||rm.master_id||rk.master_id||a.master_id) as fstr," +
                                "(b.master_name || ct.param1 || f.master_name || ct.param1 || rm.master_name || ct.param1 || rk.master_name || ct.param1 || a.master_name) Loc from master_setting a " +
                                "inner join master_setting b on b.master_id = a.classid and b.type = 'HBM' and a.client_id = b.client_id and a.client_unit_id = b.client_unit_id " +
                                "inner join master_setting f on f.master_id = a.sectionid and f.type = 'IN0' and a.client_id = f.client_id and a.client_unit_id = f.client_unit_id " +
                                "inner join master_setting rm on rm.master_id = a.client_name and rm.type = 'IN1' and a.client_id = rm.client_id and a.client_unit_id = rm.client_unit_id " +
                                "inner join master_setting rk on rk.master_id = a.subjectid and rk.type = 'IN2' and a.client_id = rk.client_id and a.client_unit_id = rk.client_unit_id " +
                                "inner join controls ct on 1 = 1 and ct.id = '000010'" +
                                "where a.type='IN3' and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";

            dt = sgen.getdata(usercode, mq);
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                mq = "select (a.client_id||a.client_unit_id||b.master_id||f.master_id||rm.master_id||rk.master_id||a.master_id) as fstr," +
                    "(b.master_name || ct.param1 || f.master_name || ct.param1 || rm.master_name || ct.param1 || rk.master_name || ct.param1 || a.master_name) Loc from master_setting a " +
                    "inner join master_setting b on b.master_id = a.classid and b.type = 'HBM' and a.client_id = b.client_id and a.client_unit_id = b.client_unit_id " +
                    "inner join master_setting f on f.master_id = a.sectionid and f.type = 'IN0' and a.client_id = f.client_id and a.client_unit_id = f.client_unit_id " +
                    "inner join master_setting rm on rm.master_id = a.client_name and rm.type = 'IN1' and a.client_id = rm.client_id and a.client_unit_id = rm.client_unit_id " +
                    "inner join master_setting rk on rk.master_id = a.subjectid and rk.type = 'IN2' and a.client_id = rk.client_id and a.client_unit_id = rk.client_unit_id " +
                    "inner join controls ct on 1 = 1 and ct.id = '000010'" +
                    "where a.type='IN3' and a.client_id = '" + clientid_mst + "' and a.client_unit_id = '" + unitid_mst + "'";

                dt = sgen.getdata(usercode, mq);
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    //=========================

    public List<SelectListItem> opname(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "opname_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'OPR' and client_unit_id='" + unitid_mst + "'");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'OPR' and client_unit_id='" + unitid_mst + "'");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }

        return sgen.dt_to_selectlist(dt);
    }
    //what to do
    public List<SelectListItem> username(String usercode)
    {
        DataTable dt = new DataTable();
        string sessionname = "username_u_"+usercode+"";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            //dt = sgen.getdata(usercode, "select lpad(rec_id,6,0) as id,first_name||' '||replace(middle_name,'0','')||''||replace(last_name,'0','') as user_name from user_details where type='CPR' ");
            dt = sgen.getdata(usercode, "select vch_num as id,first_name||' '||replace(middle_name,'0','')||''||replace(last_name,'0','') as user_name from user_details where type='CPR' ");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                //dt = sgen.getdata(usercode, "select lpad(rec_id,6,0) as id,first_name||' '||replace(middle_name,'0','')||''||replace(last_name,'0','') as user_name from user_details where type='CPR' ");
                dt = sgen.getdata(usercode, "select vch_num as id,first_name||' '||replace(middle_name,'0','')||''||replace(last_name,'0','') as user_name from user_details where type='CPR' ");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }

        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> username_emp(String usercode)
    {
        DataTable dt = new DataTable();
        string sessionname = "username_emp_u_" + usercode + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            //dt = sgen.getdata(usercode, "select lpad(rec_id,6,0) as id,first_name||' '||replace(middle_name,'0','')||''||replace(last_name,'0','') as user_name from user_details where type='CPR' ");
            dt = sgen.getdata(usercode, "select vch_num as id,first_name||' '||replace(middle_name,'0','')||''||replace(last_name,'0','') as user_name from user_details where type='CPR' and emp_id!='0' ");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                //dt = sgen.getdata(usercode, "select lpad(rec_id,6,0) as id,first_name||' '||replace(middle_name,'0','')||''||replace(last_name,'0','') as user_name from user_details where type='CPR' ");
                dt = sgen.getdata(usercode, "select vch_num as id,first_name||' '||replace(middle_name,'0','')||''||replace(last_name,'0','') as user_name from user_details where type='CPR' and emp_id!='0'");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }

        return sgen.dt_to_selectlist(dt);
    }
    //what to do
    public List<SelectListItem> username_mod(String usercode, string m_module3)
    {
        DataTable dt = new DataTable();
        string sessionname = "username_u_"+usercode+"";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            //  dt = sgen.getdata(usercode, "select lpad(rec_id,6,0) as id,first_name||' '||replace(middle_name,'0','')||''||replace(last_name,'0','') as user_name from user_details where type='CPR' ");
            dt = sgen.getdata(usercode, "select vch_num as id,first_name||' '||replace(middle_name,'0','')||''||replace(last_name,'0','') as user_name," +
                "mod_id from user_details where type='CPR' and (find_in_set(mod_id, (select distinct(m.m_id) from menus m join com_module cm on cm.mod_id = m.m_id" +
                " left join (select *from role_authority ) ra on m.m_id = ra.m_id where m.m_module3 in ('" + m_module3 + "') and trim(upper(cm.com_code)) = '" + usercode.ToUpper().Trim() + "'))= 1 or role='owner' ) ");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "select vch_num as id,first_name||' '||replace(middle_name,'0','')||''||replace(last_name,'0','') as user_name," +
    "mod_id from user_details where type='CPR' and (find_in_set(mod_id, (select distinct(m.m_id) from menus m join com_module cm on cm.mod_id = m.m_id" +
    " left join (select *from role_authority ) ra on m.m_id = ra.m_id where m.m_module3 in ('" + m_module3 + "') and trim(upper(cm.com_code)) = '" + usercode.ToUpper().Trim() + "'))= 1 or role='owner' ) ");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }

        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> natwrk(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "natwrk_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'NOW' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'NOW' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }

        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> account(String usercode, string unitid_mst, string type)
    {
        DataTable dt = new DataTable();
        string sessionname = "account_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT vch_num,c_name from clients_mst where substr(a.vch_num,0,2)='01' and  type = '" + type + "' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT vch_num,c_name from clients_mst where substr(a.vch_num,0,2)='01' and  type = '" + type + "' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }

        return sgen.dt_to_selectlist(dt);
    }

    //kcitem location 6 level
    public List<SelectListItem> iloc(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "iloc_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'LC6' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'LC6' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> iloc(String usercode, string unitid_mst, out string defval)
    {
        string val = "";
        DataTable dt = new DataTable();
        string sessionname = "iloc_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting where type = 'LC6' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;

            try
            {
                defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                val = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
            }
            catch (Exception ex)
            {
                defval = "0";
                val = "0";
            }
            HttpContext.Current.Application[sessionname + "_val"] = val;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting where type = 'LC6' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;

                try
                {
                    defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                    val = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                }
                catch (Exception ex)
                {
                    defval = "0";
                    val = "0";
                }
                HttpContext.Current.Application[sessionname + "_val"] = val;
            }
            else
            {
                val = HttpContext.Current.Application[sessionname + "_val"].ToString();
                defval = val;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    //what to do
    public DataTable getsessiondt(String usercode, string sessionname, string mq)
    {
         
        DataTable dt = new DataTable();
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, mq);
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, mq);
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return dt;
    }


    public List<SelectListItem> freq(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "freq_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'FRE' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'FRE' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }

        return sgen.dt_to_selectlist(dt);
    }
      public List<SelectListItem> freq_reminder(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "freq_reminder_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'IIT' and client_unit_id='" + unitid_mst + "'");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'IIT' and client_unit_id='" + unitid_mst + "'");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }

        return sgen.dt_to_selectlist(dt);
    }

    //brijesh rejection type
    public List<SelectListItem> Rej_type(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "Rej_type_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'PDI' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'PDI' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }

        return sgen.dt_to_selectlist(dt);
    }

    //kc //machine name
    public List<SelectListItem> mcname(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "mcname_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "select vch_num,col31 mcname from kc_tab where type='PMM' and client_unit_id='" + unitid_mst + "'");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "select vch_num,col31 mcname from kc_tab where type='PMM' and client_unit_id='" + unitid_mst + "'");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    //kc mould name
    public List<SelectListItem> mldname(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "mldname_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "select vch_num,col31 mldname from kc_tab where type='MOS' and client_unit_id='" + unitid_mst + "'");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "select vch_num,col31 mldname from kc_tab where type='MOS' and client_unit_id='" + unitid_mst + "'");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    //kc shift name
    public List<SelectListItem> shftname(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "shftname_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting WHERE Type='STM' and client_unit_id='" + unitid_mst + "'");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting WHERE Type='STM' and client_unit_id='" + unitid_mst + "'");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    //kc
    public List<SelectListItem> wiploc(String usercode, string clientid_mst, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "wiploc_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "select distinct loc,locname from btchtrans where type in ('30','32') and pono='W' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "select distinct loc,locname from btchtrans where type in ('30','32') and pono='W' and client_id='" + clientid_mst + "' and client_unit_id='" + unitid_mst + "'");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> businesstype(String usercode, string unitid_mst, out string defval)
    {
        string val = "";
        DataTable dt = new DataTable();
        string sessionname = "businesstype_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting where type = 'CMN' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
            try
            {
                defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                val = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
            }
            catch (Exception ex)
            {
                defval = val = "0";
            }

            HttpContext.Current.Application[sessionname + "_val"] = val;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting where type = 'CMN' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
                try
                {
                    defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                    val = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                }
                catch (Exception ex)
                {
                    defval = val = "0";
                }

                HttpContext.Current.Application[sessionname + "_val"] = val;
            }
            val = HttpContext.Current.Application[sessionname + "_val"].ToString();
            defval = val;
        }
        return sgen.dt_to_selectlist(dt);
    }

    //public List<SelectListItem> businesstype(String usercode, string unitid_mst)
    //{
    //    DataTable dt = new DataTable();
    //    string sessionname = "businesstype";
    //    if ((HttpContext.Current.Application[sessionname] == null)|| (dt.Rows.Count==0))
    //    {
    //        dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'CMN' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
    //        HttpContext.Current.Application[sessionname] = dt;
    //    }
    //    else
    //    {
    //        dt = ((DataTable)HttpContext.Current.Application[sessionname]);
    //    }
    //    return sgen.dt_to_selectlist(dt);
    //}

    public List<SelectListItem> nextaction(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "nextaction_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'NAM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'NAM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> grandgrp(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "grandgrp_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "select a.classid,(a.master_name||' ('||a.classid||')') master_name from master_setting a where a.type='KGP'");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "select a.classid,(a.master_name||' ('||a.classid||')') master_name from master_setting a where a.type='KGP'");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> examtype(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "examtype_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "select master_id,master_name from master_setting  where  type='EEM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "select master_id,master_name from master_setting  where  type='EEM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }
      public List<SelectListItem> typeof_service(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "typeof_service_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "select master_id,master_name from master_setting where type='TOS' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "select master_id,master_name from master_setting where type='TOS' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> chkuptype(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "chkuptype_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "select master_id,master_name from master_setting  where  type='HCT' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "select master_id,master_name from master_setting  where  type='HCT' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> teacher(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "teacher_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            //dt = sgen.getdata(usercode, "select lpad(rec_id,6,0) as id,first_name||' '||replace(middle_name,'0','')||''||replace(last_name,'0','') as user_name from user_details where type='CPR' and status='active' and " +
            dt = sgen.getdata(usercode, "select vch_num as id,first_name||' '||replace(middle_name,'0','')||''||replace(last_name,'0','') as user_name from user_details where type='CPR' and status='active' and " +
                "find_in_set(client_unit_id,'" + unitid_mst + "')=1 ");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                //dt = sgen.getdata(usercode, "select lpad(rec_id,6,0) as id,first_name||' '||replace(middle_name,'0','')||''||replace(last_name,'0','') as user_name from user_details where type='CPR' and status='active' and " +
                dt = sgen.getdata(usercode, "select vch_num as id,first_name||' '||replace(middle_name,'0','')||''||replace(last_name,'0','') as user_name from user_details where type='CPR' and status='active' and " +
    "find_in_set(client_unit_id,'" + unitid_mst + "')=1 ");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }

        return sgen.dt_to_selectlist(dt);
    }

    //what to do
    public List<SelectListItem> mailcc(String usercode)
    {
        DataTable dt = new DataTable();
        string sessionname = "mailcc_u_"+usercode+"";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "select user_id||' ('||nvl(email,'-')||')' name  FROM user_details WHERE type<>'STD' and type<>'STP' ");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "select user_id||' ('||nvl(email,'-')||')' name  FROM user_details WHERE type<>'STD' and type<>'STP' ");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }

        return sgen.dt_to_selectlist(dt);
    }

    //kc insby 
    public List<SelectListItem> insby(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "insby_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'K01' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'K01' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    //kc locreg
    public List<SelectListItem> locreg(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "locreg_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'KLR' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'DEL' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    //kc delivery term 
    public List<SelectListItem> delterm(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "delterm_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'DEL' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'DEL' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    //kc machine capacity master
    public List<SelectListItem> mcap(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "mcap_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'K02' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'K02' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> orderstatus(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "orderstatus_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'ORS' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'ORS' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    //public List<SelectListItem> subbroker(String usercode, string clientid_mst, string unitid_mst)
    //{
    //    DataTable dt = new DataTable();
    //    string sessionname = "subbroker";
    //    if ((HttpContext.Current.Application[sessionname] == null)|| (dt.Rows.Count==0))
    //    {

    //        dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'SBM' and find_in_set(client_id,'" + clientid_mst + "')=1 and " +
    //            "find_in_set(client_unit_id,'" + unitid_mst + "')=1");
    //        HttpContext.Current.Application[sessionname] = dt;
    //    }
    //    else
    //    {
    //        dt = ((DataTable)HttpContext.Current.Application[sessionname]);
    //    }
    //    return sgen.dt_to_selectlist(dt);
    //}

    public List<SelectListItem> subbroker(String usercode, string unitid_mst, out string defval)
    {
        string val = "";
        DataTable dt = new DataTable();
        string sessionname = "subbroker_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting where type = 'SBM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;

            try
            {
                defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                val = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
            }
            catch (Exception ex)
            {
                defval = "0";
                val = "0";
            }

            HttpContext.Current.Application[sessionname + "_val"] = val;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting where type = 'SBM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;

                try
                {
                    defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                    val = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                }
                catch (Exception ex)
                {
                    defval = "0";
                    val = "0";
                }

                HttpContext.Current.Application[sessionname + "_val"] = val;
            }
            else
            {
                val = HttpContext.Current.Application[sessionname + "_val"].ToString();
                defval = val;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> clientrating(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "clientrating_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'CRM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'CRM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> clientrating(String usercode, string unitid_mst, out string defval)
    {
        string val = "";
        DataTable dt = new DataTable();
        string sessionname = "clientrating_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting where type = 'CRM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;

            try
            {
                defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                val = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
            }
            catch (Exception ex)
            {
                defval = "0";
                val = "0";
            }

            HttpContext.Current.Application[sessionname + "_val"] = val;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting where type = 'CRM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;

                try
                {
                    defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                    val = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                }
                catch (Exception ex)
                {
                    defval = "0";
                    val = "0";
                }

                HttpContext.Current.Application[sessionname + "_val"] = val;
            }
            else
            {
                val = HttpContext.Current.Application[sessionname + "_val"].ToString();
                defval = val;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> creditrating(String usercode, string unitid_mst, out string defval)
    {
        string val = "";
        DataTable dt = new DataTable();
        string sessionname = "creditrating_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting  where type = 'CRD' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
            try
            {
                defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                val = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
            }
            catch (Exception ex)
            {
                defval = "0";
                val = "0";
            }

            HttpContext.Current.Application[sessionname + "_val"] = val;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting  where type = 'CRD' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
                try
                {
                    defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                    val = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                }
                catch (Exception ex)
                {
                    defval = "0";
                    val = "0";
                }

                HttpContext.Current.Application[sessionname + "_val"] = val;
            }
            else
            {
                val = HttpContext.Current.Application[sessionname + "_val"].ToString();
                defval = val;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }
    //what to do
    public List<SelectListItem> compunit(String usercode, string clientid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "compunit_c_" + usercode + "_" + clientid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "select cup_id ,Unit_Name  from company_unit_profile where company_profile_id='" + clientid_mst + "'");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "select cup_id ,Unit_Name  from company_unit_profile where company_profile_id='" + clientid_mst + "'");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> schtype(String usercode, string unitid_mst, out string defval)
    {
        string val = "";
        DataTable dt = new DataTable();
        string sessionname = "schtype_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting where type = 'SCH' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;

            try
            {
                defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                val = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
            }
            catch (Exception ex)
            {
                defval = "0";
                val = "0";
            }

            HttpContext.Current.Application[sessionname + "_val"] = val;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting where type = 'SCH' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;

                try
                {
                    defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                    val = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                }
                catch (Exception ex)
                {
                    defval = "0";
                    val = "0";
                }

                HttpContext.Current.Application[sessionname + "_val"] = val;
            }
            else
            {
                val = HttpContext.Current.Application[sessionname + "_val"].ToString();
                defval = val;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> schtype(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "schtype_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'SCH' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'SCH' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> feetype(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "feetype_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "select FeeType_id, FeeTypeName from school_fee_type where type='0' and client_unit_id='" + unitid_mst + "' and (master_type='0' or  master_type='') and FeeTypeName!='Fine' and FeeTypeName!='Transport'  and FeeTypeName!='Library' and feetypename!='Previous Year Fees'");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "select FeeType_id, FeeTypeName from school_fee_type where type='0' and client_unit_id='" + unitid_mst + "' and (master_type='0' or  master_type='') and FeeTypeName!='Fine' and FeeTypeName!='Transport'  and FeeTypeName!='Library' and feetypename!='Previous Year Fees'");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }
    public List<SelectListItem> feetypeall(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "feetypeall_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "select FeeType_id, FeeTypeName from school_fee_type where type='0' and client_unit_id='" + unitid_mst + "' and (master_type='0' or  master_type='') ");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "select FeeType_id, FeeTypeName from school_fee_type where type='0' and client_unit_id='" + unitid_mst + "' and (master_type='0' or  master_type='') ");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> cheqprint(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "cheqprint_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'CPL' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'CPL' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> switchtype(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "switchtype_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'SWT' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'SWT' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }
    //what to do
    public List<SelectListItem> ctrystate(String usercode, string country_id)
    {
        DataTable dt = new DataTable();
        string sessionname = "ctrystate_u_" + usercode + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "select distinct state_gst_code,state_name from country_state where alpha_2='" + country_id + "'");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "select distinct state_gst_code,state_name from country_state where alpha_2='" + country_id + "'");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }
    public List<SelectListItem> assgname(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "assgname_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'ASG' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'ASG' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> fundname(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "fundname_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'FUD' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'FUD' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    #region 02012020 

    public List<SelectListItem> lunchtm(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "lunchtm_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'ELT'  and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'ELT'  and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }
    public List<SelectListItem> lectype(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "lectype";
        if ((HttpContext.Current.Application[sessionname] == null) || (dt.Rows.Count > 0))
        {

            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'ECL'  and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
        }
        return sgen.dt_to_selectlist(dt);
    }
    public List<SelectListItem> fueltype(String usercode, out string defval)
    {
        string val = "";
        DataTable dt = new DataTable();
        string sessionname = "fueltype";
        if ((HttpContext.Current.Application[sessionname] == null) || (dt.Rows.Count > 0))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting where type = 'FUE'");
            HttpContext.Current.Application[sessionname] = dt;

            try
            {
                defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                val = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
            }
            catch (Exception ex)
            {
                defval = "0";
                val = "0";
            }

            HttpContext.Current.Application[sessionname + "_val"] = val;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            val = HttpContext.Current.Application[sessionname + "_val"].ToString();
            defval = val;
        }
        return sgen.dt_to_selectlist(dt);
    }
    public List<SelectListItem> custtyp(String usercode, string unitid_mst, out string defval)
    {
        string val = "";
        DataTable dt = new DataTable();
        string sessionname = "custtyp";
        if ((HttpContext.Current.Application[sessionname] == null) || (dt.Rows.Count > 0))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting where type = 'CST' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;

            try
            {
                defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                val = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
            }
            catch (Exception ex)
            {
                defval = "0";
                val = "0";
            }

            HttpContext.Current.Application[sessionname + "_val"] = val;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            val = HttpContext.Current.Application[sessionname + "_val"].ToString();
            defval = val;
        }
        return sgen.dt_to_selectlist(dt);
    }
    public List<SelectListItem> dealprb(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "dealprb";
        if ((HttpContext.Current.Application[sessionname] == null) || (dt.Rows.Count > 0))
        {

            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'DPM'  and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
        }
        return sgen.dt_to_selectlist(dt);
    }


    //public List<SelectListItem> grade(String usercode, string unitid_mst)
    //{
    //    DataTable dt = new DataTable();
    //    string sessionname = "grade";
    //    if ((HttpContext.Current.Application[sessionname] == null) || (dt.Rows.Count == 0))
    //    {
    //        dt = sgen.getdata(usercode, "select master_id,master_name from master_setting where type='KGM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");

    //        HttpContext.Current.Application[sessionname] = dt;
    //    }
    //    else
    //    {
    //        dt = ((DataTable)HttpContext.Current.Application[sessionname]);
    //    }

    //    return sgen.dt_to_selectlist(dt);
    //}


    public List<SelectListItem> grade(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "grade";
        if ((HttpContext.Current.Application[sessionname] == null) || (dt.Rows.Count > 0))
        {

            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'KGM'  and " +
                "find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
        }
        return sgen.dt_to_selectlist(dt);
    }
    public List<SelectListItem> incentive_type(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "incentive_type";
        if ((HttpContext.Current.Application[sessionname] == null) || (dt.Rows.Count > 0))
        {

            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'ITM'  and " +
                "find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
        }
        return sgen.dt_to_selectlist(dt);
    }
    public List<SelectListItem> incmst(String usercode, string unitid_mst, out string defval)
    {
        string val = "";
        DataTable dt = new DataTable();
        string sessionname = "incmst";
        if ((HttpContext.Current.Application[sessionname] == null) || (dt.Rows.Count > 0))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting where type = 'ITM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;

            try
            {
                defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                val = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
            }
            catch (Exception ex)
            {
                defval = "0";
                val = "0";
            }

            HttpContext.Current.Application[sessionname + "_val"] = val;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            val = HttpContext.Current.Application[sessionname + "_val"].ToString();
            defval = val;
        }
        return sgen.dt_to_selectlist(dt);
    }
    public List<SelectListItem> currsrc(String usercode, string unitid_mst, out string defval)
    {
        string val = "";
        DataTable dt = new DataTable();
        string sessionname = "currsrc";
        if ((HttpContext.Current.Application[sessionname] == null) || (dt.Rows.Count > 0))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting where type = 'CRS' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;

            try
            {
                defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                val = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
            }
            catch (Exception ex)
            {
                defval = "0";
                val = "0";
            }

            HttpContext.Current.Application[sessionname + "_val"] = val;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            val = HttpContext.Current.Application[sessionname + "_val"].ToString();
            defval = val;
        }
        return sgen.dt_to_selectlist(dt);
    }
    public List<SelectListItem> exphead(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "exphead";
        if ((HttpContext.Current.Application[sessionname] == null) || (dt.Rows.Count > 0))
        {

            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'BEH' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
        }
        return sgen.dt_to_selectlist(dt);
    }
    public List<SelectListItem> ohtype(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "ohtype";
        if ((HttpContext.Current.Application[sessionname] == null) || (dt.Rows.Count > 0))
        {

            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'OHT'  and " +
                "find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
        }
        return sgen.dt_to_selectlist(dt);
    }
    public List<SelectListItem> leadcls(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "leadcls";
        if ((HttpContext.Current.Application[sessionname] == null) || (dt.Rows.Count > 0))
        {

            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'LCT'  and " +
                "find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
        }
        return sgen.dt_to_selectlist(dt);
    }
    public List<SelectListItem> othchg(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "othchg";
        if ((HttpContext.Current.Application[sessionname] == null) || (dt.Rows.Count > 0))
        {

            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'MR0'  and " +
                "find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
        }
        return sgen.dt_to_selectlist(dt);
    }
    public List<SelectListItem> natact(String usercode, string unitid_mst, out string defval)
    {
        string val = "";
        DataTable dt = new DataTable();
        string sessionname = "natact";
        if ((HttpContext.Current.Application[sessionname] == null) || (dt.Rows.Count > 0))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name,sectionid FROM master_setting where type = 'NOA' and " +
                "find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;

            try
            {
                defval = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
                val = dt.Select("sectionid='Y'")[0].ItemArray[0].ToString();
            }
            catch (Exception ex)
            {
                defval = "0";
                val = "0";
            }

            HttpContext.Current.Application[sessionname + "_val"] = val;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            val = HttpContext.Current.Application[sessionname + "_val"].ToString();
            defval = val;
        }
        return sgen.dt_to_selectlist(dt);
    }
    public List<SelectListItem> finacc(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "finacc";
        if ((HttpContext.Current.Application[sessionname] == null) || (dt.Rows.Count > 0))
        {

            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'FAN'  and " +
                "find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
        }
        return sgen.dt_to_selectlist(dt);
    }
    public List<SelectListItem> attmst(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "finacc";
        if ((HttpContext.Current.Application[sessionname] == null) || (dt.Rows.Count > 0))
        {

            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'ATB'  and " +
                "find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
        }
        return sgen.dt_to_selectlist(dt);
    }
    #endregion

    #region 03022020
    public List<SelectListItem> subdept(String usercode, string unitid_mst, string dept_id)
    {
        DataTable dt = new DataTable();
        string sessionname = "subdept_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'CD1' and find_in_set(client_unit_id,'" + unitid_mst + "')=1 and classid='" + dept_id + "'");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'CD1' and find_in_set(client_unit_id,'" + unitid_mst + "')=1 and classid='" + dept_id + "'");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }
    #endregion

    public List<SelectListItem> tasktype(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "tasktype_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'TSK'  and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'TSK'  and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> board(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "board_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'EBD'  and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'EBD'  and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }
    //what to do
    public List<SelectListItem> comp_type(String usercode)
    {
        DataTable dt = new DataTable();
        string sessionname = "comp_type_u_" + usercode + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'TOC'");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'TOC'");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    //what to do
    public List<SelectListItem> timezone(String usercode)
    {
        DataTable dt = new DataTable();
        string sessionname = "timezone_fun_u_" + usercode + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT classid||'#'||master_id,(master_name||' ('||classid||')') as master_name from master_setting where type='TZN'");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT classid||'#'||master_id,(master_name||' ('||classid||')') as master_name from master_setting where type='TZN'");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> transtype(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "transtype_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'TRA' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'TRA' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }
    public List<SelectListItem> broker(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "broker_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'BBM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'BBM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> manuf(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "manuf_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "select vch_num man_code,c_name as man_name from clients_mst where type='BCD' and substr(vch_num,0,3)='303' and find_in_set(client_unit_id,'" + unitid_mst + "')=1 and mftr='Y'");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "select vch_num man_code,c_name as man_name from clients_mst where type='BCD' and substr(vch_num,0,3)='303' and find_in_set(client_unit_id,'" + unitid_mst + "')=1 and mftr='Y'");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }
    public List<SelectListItem> policytype(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "policytype_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'IPT' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'IPT' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }
    public List<SelectListItem> policysubtype(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "policysubtype_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'SPT' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'SPT' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }
     public List<SelectListItem> prd_sub_cat(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "prd_sub_cat_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'PSC' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'PSC' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> rdmptype(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "rdmptype_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'RDP' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'RDP' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> purthr(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "purthr_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'PTH' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'PTH' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }
    public List<SelectListItem> channel(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "channel_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'CHM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'CHM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }
    //what to do
    public List<SelectListItem> ac_year(String usercode, string client_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "ac_year_c_" + usercode + "_" + client_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null) || (dt.Rows.Count == 0))
        {
            dt = sgen.getdata(usercode, "select academic_year_id,academic_year from add_academic_year where  client_id='" + client_mst + "' and type='ACY'");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "select academic_year_id,academic_year from add_academic_year where  client_id='" + client_mst + "' and type='ACY'");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> traininglevel(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "traininglevel_u_" + usercode + "_" + unitid_mst + "";        
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'TRL' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'TRL' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }


    public List<SelectListItem> tickettype(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "tickettype_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'TKT' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'TKT' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> ticketstatus(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "ticketstatus_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'TKS' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'TKS' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> ticketpriority(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "ticketpriority_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'TKP' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'TKP' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> ticketgroup(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "ticketgroup_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "select distinct  vch_num grp_id,col5 grp_name from enx_tab2 where type='GRP' and client_unit_id='"+unitid_mst+"'");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "select distinct  vch_num grp_id,col5 grp_name from enx_tab2 where type='GRP' and client_unit_id='"+unitid_mst+"'");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> areaimp(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "areaimp_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'AIM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'AIM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> expcomp(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "expcomp_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'EXC' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'EXC' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> loantype(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "loantype_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'LON' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'LON' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> modeofmeet(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "modeofmeet_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'MOM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'MOM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> goalmaster(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "goalmaster_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'GLM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'GLM' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }
    public List<SelectListItem> ticketsrc(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "ticketsrc_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'TSC' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'TSC' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> leadrating(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "leadrating_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'RMA' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'RMA' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> leadVenue(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "leadVenue_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'VEN' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'VEN' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> leadFreq(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "leadFreq_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'IIT' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'IIT' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> leadMenu(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "leadMenu_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'MEN' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'MEN' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> leadtof(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "leadtof_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'TOF' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting where type = 'TOF' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }

    public List<SelectListItem> outcome(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "outcome_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'OUT' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'OUT' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }
    public List<SelectListItem> servtype(String usercode, string unitid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "servtype_u_" + usercode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'SER' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = sgen.getdata(usercode, "SELECT master_id,master_name FROM master_setting  where type = 'SER' and find_in_set(client_unit_id,'" + unitid_mst + "')=1");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return sgen.dt_to_selectlist(dt);
    }   
}




