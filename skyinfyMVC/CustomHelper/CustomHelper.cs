using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace skyinfyMVC.CustomHelper
{
    public class CustomHelper
    {
        public static sgenFun sgen;
        public static string html = "";
        public static int cli = 0;
        public static string MyGuid = "";

        public static MvcHtmlString image(string source, string altTxt, string width, string height)
        {
            //TagBuilder creates a new tag with the tag name specified    
            var ImageTag = new TagBuilder("img");
            //MergeAttribute Adds attribute to the tag    
            ImageTag.MergeAttribute("src", source);
            ImageTag.MergeAttribute("alt", altTxt);
            ImageTag.MergeAttribute("width", width);
            ImageTag.MergeAttribute("height", height);
            //Return an HTML encoded string with SelfClosing TagRenderMode    
            return MvcHtmlString.Create(ImageTag.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString SAMenu(string MyGuid)
        {
            try
            {
                sgen = new sgenFun(MyGuid);
                html = Mymenu(MyGuid);
                html = html.Replace("''", "'");
            }
            catch (Exception err)
            {
                html = "<div></div>";
            }
            return MvcHtmlString.Create(html);
        }
        public static string Mymenu(string MyGuid)
        {
            try
            {
                cli = 0;
                sgen = new sgenFun(MyGuid);
                string userCode = sgen.GetCookie(MyGuid, "userCode");
                string userid_mst = sgen.GetCookie(MyGuid, "userid_mst");
                string m_module1 = sgen.GetCookie(MyGuid, "m_module1").ToString();
                string m_module2 = sgen.GetCookie(MyGuid, "m_module2").ToString();
                string m_module3 = sgen.GetCookie(MyGuid, "m_module3").ToString();
                string ulevel_mst = sgen.GetCookie(MyGuid, "ulevel_mst").ToString();
                string m_id = sgen.GetCookie(MyGuid, "m_id").ToString();
                string utype_mst = sgen.GetCookie(MyGuid, "utype_mst").ToString();
                string role_mst = sgen.GetCookie(MyGuid, "role_mst");
                if (ulevel_mst == "") { ulevel_mst = "0"; }
                string olduser = sgen.GetCookie(MyGuid, "OlduserCode");
                string old_module3 = sgen.GetCookie(MyGuid, "Oldm_module3");
                if (!olduser.Trim().Equals(userCode.Trim()) || !old_module3.Trim().Equals(m_module3.Trim()))
                {
                    sgen.SetSession(MyGuid, "menudt", null);
                    sgen.SetCookie(MyGuid, "OlduserCode", userCode);
                    sgen.SetCookie(MyGuid, "Oldm_module3", m_module3);
                }
                //string mq0 = "select m_id,m_level,m_name,m_order,m_submenu,m_module1,m_module2,m_module3 from menus where m_module3='tmain' order by m_id,m_level, m_order";
                string mq = "";
                mq = "select distinct userid from urights where type='K98' and userid='" + userid_mst + "'";
                mq = sgen.seekval(userCode, mq, "userid");

                if (utype_mst.Equals("STP") || utype_mst.Equals("STD"))
                {
                    ////mq = "select * from menus where m_module3 = '" + m_module3 + "' and u_id>= '" + ulevel_mst + "' order by m_id,m_level, m_order";                   
                    //if (mq.Trim().Equals("0"))
                    //{
                    //    mq = "select u.m_id,m.m_name,m.m_link,m.m_level,m.m_icon,m.m_css,m.m_submenu,m.m_module1,m.m_module2,u.m_module3,m.u_id,m.m_order,m.m_default," +
                    //         "m.and_type,m.and_link,m.i_link,m.attributes,m.activity_type,u.role,u.btnnew,u.btnedit,u.btnsave,u.btnsavenew,u.btnview,u.btnextend from menus m " +
                    //         "inner join urights u on u.m_id = m.m_id and u.m_module3 = m.m_module3 and u.type = 'K99' and u.role= '" + m_module3 + "-" + ulevel_mst + "' " +
                    //         "where m.m_module3 = '" + m_module3 + "' order by u.m_id,m.m_level,m.m_order";
                    //}
                    //else
                    //{
                    //    mq = "select u.m_id,m.m_name,m.m_link,m.m_level,m.m_icon,m.m_css,m.m_submenu,m.m_module1,m.m_module2,u.m_module3,m.u_id,m.m_order,m.m_default," +
                    //         "m.and_type,m.and_link,m.i_link,m.attributes,m.activity_type,u.role,u.btnnew,u.btnedit,u.btnsave,u.btnsavenew,u.btnview,u.btnextend from menus m " +
                    //         "inner join urights u on u.m_id = m.m_id and u.m_module3 = m.m_module3 and u.type = 'K98' and u.userid='" + userid_mst + "' " +
                    //         "where m.m_module3 = '" + m_module3 + "' order by u.m_id,m.m_level,m.m_order";
                    //}                    

                    if (utype_mst.Equals("STD"))
                    {
                        mq = "select * from menus where m_module3 = 'stdmain' and u_id>= '" + ulevel_mst + "' order by m_order";
                    }
                    else
                    {
                        mq = "select * from menus where m_module3 = 'stpmain' and u_id>= '" + ulevel_mst + "' order by m_order";
                    }
                }
                else
                {
                    if (role_mst.Trim().ToUpper().Equals("OWNER"))
                    {
                        mq = "select * from menus where  (m_module3 = '" + m_module3 + "' or m_module3 = 'common') and m_module3 not in ('stpmain','stdmain')  and u_id>= '" + ulevel_mst + "' order by m_order";
                    }
                    else
                    {
                        if (mq.Trim().Equals("0"))
                        {
                            mq = "select u.m_id,m.m_name,m.m_link,m.m_level,m.m_icon,m.m_css,m.m_submenu,m.m_module1,m.m_module2,u.m_module3,m.u_id,m.m_order,m.m_default," +
                                 "m.and_type,m.and_link,m.i_link,m.attributes,m.activity_type,u.role,u.btnnew,u.btnedit,u.btnsave,u.btnsavenew,u.btnview,u.btnextend from " +
                                 "menus m " +
                                 "inner join urights u on u.m_id = m.m_id and u.m_module3 = m.m_module3 and u.type = 'K99' and u.role in ( '" + m_module3 + "-" + ulevel_mst + "' ,'-') " +
                                 "where (m.m_module3 = '" + m_module3 + "' or m.m_module3 = 'common') and m.m_module3 not in ('stpmain','stdmain') order by u.m_id,m.m_level,m.m_order";
                        }
                        else
                        {
                            mq = "select u.m_id,m.m_name,m.m_link,m.m_level,m.m_icon,m.m_css,m.m_submenu,m.m_module1,m.m_module2,u.m_module3,m.u_id,m.m_order,m.m_default," +
                                 "m.and_type,m.and_link,m.i_link,m.attributes,m.activity_type,u.role,u.btnnew,u.btnedit,u.btnsave,u.btnsavenew,u.btnview,u.btnextend from menus m " +
                                 "inner join urights u on u.m_id = m.m_id and u.m_module3 = m.m_module3 and u.type = 'K98' and userid='" + userid_mst + "' " +
                                 "where (m.m_module3 = '" + m_module3 + "' or m.m_module3 = 'common') and m.m_module3 not in ('stpmain','stdmain') order by u.m_id,m.m_level,m.m_order";
                        }
                    }
                }

                DataTable dtparent = new DataTable();
                if (sgen.GetSession(MyGuid, "menudt") != null)
                {
                    dtparent = (DataTable)sgen.GetSession(MyGuid, "menudt");
                }
                else
                {
                    dtparent = sgen.getdata(userCode, mq);
                    sgen.SetSession(MyGuid, "menudt", dtparent);
                }

                foreach (DataRow drk in dtparent.Rows)
                {
                    if (drk["m_id"].ToString().Equals("7005.6"))
                    {

                    }
                    if (drk["m_submenu"].ToString().Trim().ToUpper().Equals("0")
                        && drk["m_link"].ToString().Trim().Length > 3
                        && !drk["m_link"].ToString().EndsWith(".aspx")
                         && drk["m_link"].ToString().Contains("erp")
                        )
                    {
                        drk["m_link"] = drk["m_link"].ToString().Trim() + ".aspx";

                    }
                    if (drk["m_submenu"].ToString().Trim().ToUpper().Equals("0"))
                    {
                        if (drk["attributes"].ToString().Trim().Contains("onclick=$"))
                        {
                            var bval = drk["attributes"].ToString().Trim();
                            bval = bval.Replace("onclick=$", "");
                            do
                            {
                                bval = bval.Replace("showwait();", "");
                            }
                            while (bval.Contains("showwait();"));
                            drk["attributes"] = "onclick=$" + bval;
                        }
                        else drk["attributes"] = "onclick=$menuclick(this,'" + MyGuid + "');$";
                    }
                }

                dtparent.AcceptChanges();
                sgen.SetSession(MyGuid, "dtparent", dtparent);

                DataTable dt0 = dtparent.AsEnumerable().Where(w => Convert.ToInt64(w["m_level"]) == 2).Select(s => s).CopyToDataTable();
                html = "<ul id='myMenu' class='nav side-menu'>";
                foreach (DataRow dr in dt0.Rows)
                {
                    if (dr["m_id"].ToString().Equals("7005.6"))
                    {

                    }
                    if (dr["m_submenu"].ToString().Equals("0"))
                    {
                        cli++;
                        if (dr["m_link"].ToString().Trim().Length > 4) html = html + "<li id='l" + cli + "' onclick='givemyid(this)'> <a runat='server' id='m" + dr["m_id"].ToString() + "' " + dr["attributes"].ToString().Replace('$', '\"') + "'><i class='" + dr["m_icon"].ToString().Trim() + "'></i>" + dr["m_name"].ToString() + "</a></li>";
                        else html = html + "<li id='l" + cli + "' onclick='givemyid(this)'> <a runat='server' id='m" + dr["m_id"].ToString() + "' " + dr["attributes"].ToString().Replace('$', '\"') + "><i class='" + dr["m_icon"].ToString().Trim() + "'></i>" + dr["m_name"].ToString() + "</a></li>";
                    }
                    else
                    {
                        cli++;
                        html = html + "<li id='l" + cli + "' onclick='liclick(this,event);'> <a id='a" + cli + "'><i class='" + dr["m_icon"].ToString().Trim() + "'></i>" + dr["m_name"].ToString() + "<span class='fa fa-chevron-down'></span></a>";
                        html = html + "<ul id='u" + cli + "' class='nav child_menu'>";
                        makemenu(dtparent, dr["m_module3"].ToString(), dr["m_module1"].ToString(), Convert.ToInt64(dr["m_level"].ToString()) + 1);
                        html = html + " </ul>";
                    }
                }
                html = html + " </ul>";
            }
            catch (Exception err)
            {

            }
            return html;
        }
        public static void makemenu(DataTable dtparent, string module3, string module1, long level)
        {
            string m_id = sgen.GetCookie(MyGuid, "m_id").ToString();
            if (dtparent.Columns["m_level"].DataType.ToString() != "System.Decimal")
            {

            }
            try
            {
                DataTable dtstatuswise = dtparent.AsEnumerable().Where(w => (string)w["m_module3"] == module3 &&
                                (string)w["m_module2"] == module1 && Convert.ToInt64(w["m_level"]) == level)
                                                    .Select(s => s).CopyToDataTable();

                foreach (DataRow dr in dtstatuswise.Rows)
                {
                    if (dr["m_submenu"].ToString().Equals("0"))
                    {
                        cli++;
                        if (dr["m_link"].ToString().Trim().Length > 4) html = html + "<li id='l" + cli + "' onclick='givemyid(this)'> <a runat='server' id='m" + dr["m_id"].ToString() + "' " + dr["attributes"].ToString().Replace('$', '\"') + " ><i class='" + dr["m_icon"].ToString() + "'></i>" + dr["m_name"].ToString() + "</a></li>";
                        else html = html + "<li id='l" + cli + "' onclick='givemyid(this)'> <a runat='server' id='m" + dr["m_id"].ToString() + "' " + dr["attributes"].ToString().Replace('$', '\"') + "><i class='" + dr["m_icon"].ToString() + "'></i>" + dr["m_name"].ToString() + "</a></li>";
                    }
                    else
                    {
                        cli++;
                        html = html + "<li id='l" + cli + "' onclick='liclick(this,event);' > <a id='a" + cli + "'><i class='" + dr["m_icon"].ToString() + "'></i>" + dr["m_name"].ToString() + "<span class='fa fa-chevron-down'></span></a>";
                        html = html + "<ul id='u" + cli + "' class='nav child_menu'>";
                        makemenu(dtparent, dr["m_module3"].ToString(), dr["m_module1"].ToString(), Convert.ToInt64(dr["m_level"].ToString()) + 1);
                        html = html + " </ul>";
                    }
                }
            }
            catch (Exception err) { }
        }
    }
}

