using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
//using Oracle.DataAccess.Client;
//using System.Data.OracleClient;

namespace skyinfyMVC.Models
{
    public class Multiton
    {
        //read-only dictionary to track multitons
        private static IDictionary<string, Multiton> _Tracker = new Dictionary<string, Multiton> { };

    
        public String UserCode;
   

        private Multiton(string key, string Ucode)
        {
            UserCode = Ucode;
          


        }
        public static Multiton GetInstance(string key)
        {
            //value to return
            Multiton item = null;
            //if (key == null) key = "";
            //lock collection to prevent changes during operation
            lock (_Tracker)
            {
                //if value not found, create and add
                if (!_Tracker.TryGetValue(key, out item))
                {
                    string Ucode = "";
                    Ucode = GetCookie(key, "userCode");
                    if (Ucode == "") { Ucode = (String)GetSession(key, "userCode"); }
                    if (Ucode == null || Ucode == "") { Ucode = getUserCode(); }
                    if (Ucode == null || Ucode == "") { }
                    else
                    {
                        item = new Multiton(key, Ucode);
                        _Tracker.Add(key, item);
                    }
                    //calculate next key
                    //int newIdent = _Tracker.Keys.Max() + 1;
                    //add item
                    
                }
            }
            return item;
        }
        public static Multiton ReNew(string key)
        {
            //value to return
            Multiton item = null;
            //if (key == null) key = "";
            
            //lock collection to prevent changes during operation
            lock (_Tracker)
            {
                _Tracker.Remove(key);
                //if value not found, create and add
                if (!_Tracker.TryGetValue(key, out item))
                {
                    string Ucode = "";
                    Ucode = GetCookie(key, "userCode");
                    if (Ucode == "") { Ucode = (String)GetSession(key, "userCode"); }
                    if (Ucode == null || Ucode == "") { Ucode = getUserCode(); }
                    if (Ucode == null || Ucode == "") { }
                    else
                    {
                        item = new Multiton(key, Ucode);
                        _Tracker.Add(key, item);
                    }
                  

                }
            }
            return item;
        }
        public static string GetCookie(string MyGuid, string name)
        {
            string val = "";
            if (HttpContext.Current.Request.Cookies[MyGuid + "_" + name] != null)
            {
                val = HttpContext.Current.Request.Cookies[MyGuid + "_" + name].Value.ToString();
            }
            return val;
        }
        public static void SetCookie(string MyGuid, string name, string value)
        {
            //Writing Multiple values in single cookie
            HttpContext.Current.Response.Cookies.Remove(MyGuid + "_" + name);
            HttpCookie hc = new HttpCookie(MyGuid + "_" + name);
            hc.Value = value;
            HttpContext.Current.Response.Cookies.Add(hc);
        }
        public static void SetSession(string MyGuid, string SessionName, object value)
        {
            HttpContext.Current.Session[MyGuid + "_" + SessionName] = value;
        }
        public static object GetSession(string MyGuid, string SessionName)
        {
            return HttpContext.Current.Session[MyGuid + "_" + SessionName];
        }
        public static string getUserCode()
        {
            string res = "-";
            //if (HttpContext.Current.Session[MyGuid + "_cocd_mst"] != null)
            //{
            //    res = (String)HttpContext.Current.Session[MyGuid + "_cocd_mst"];
            //}


            string url = new Uri(HttpContext.Current.Request.Url.AbsoluteUri).OriginalString;

            //http://localhost:13660/erp/login_main
            //url = "http://test.skyinfy.com/erp/school_admin/school_admin_configpage";
            //url = "http://skyinfy.com/erp/school_admin/school_admin_configpage";

            //url = "https://cali.skyinfy.com/home/login";
            //url = "http://name.skyinfy.com/Inventory/mat_req?m_id=b8LgZhj%2BfsvYOywM82wsGg%3D%3D&mid=Tf2i5qHhrAg%3D";
            var cnt = 0;
            try
            {
                String[] HEADS = url.Split('/');
                if (HEADS[2].ToString().Trim().ToUpper().Contains(".COM") || HEADS[2].ToString().Trim().ToUpper().Contains(".IN"))
                {
                    //if (url.ToLower().Contains("https"))
                    //{
                    //    cnt = url.Replace("https://", "").Split('/')[0].Split('.').Count();
                    //    if (cnt >= 3) res = url.Replace("https://", "").Split('/')[0].Split('.')[0].ToString();
                    //    //if (res.Trim().ToUpper().Equals("WWW")) res = (String)HttpContext.Current.Session[MyGuid + "_cocd_mst"];
                    //}
                    //else if (url.ToLower().Contains("http"))
                    //{

                    //    cnt = url.Replace("http://", "").Split('/')[0].Split('.').Count();
                    //    if (cnt >= 3) res = url.Replace("http://", "").Split('/')[0].Split('.')[0].ToString();
                    //    //if (res.Trim().ToUpper().Equals("WWW")) res = (String)HttpContext.Current.Session[MyGuid + "_cocd_mst"];

                    //}
                    res = HEADS[2].Split('.')[0];
                }
            }
            catch (Exception err)
            { }

            //string path = @"C:\skyinfy\mytns2.txt";
            string path = HttpRuntime.AppDomainAppPath + "\\mytns2.txt";

            string str = "", srv = "", PWD = "", constr = "", IP = "";
            if (res.Trim().Length < 2)
            {
                try
                {
                    if (File.Exists(path))
                    {
                        StreamReader sr = new StreamReader(path);
                        str = sr.ReadToEnd().Trim();
                        if (str.Contains("\r")) str = str.Replace("\r", ",");
                        if (str.Contains("\n")) str = str.Replace("\n", ",");
                        str = str.Replace(",,", ",");
                        IP = str.Split(',')[1];
                        if (res.Trim().Equals("-")) res = str.Split(',')[0].ToLower();
                        sr.Close();
                    }
                }
                catch (Exception err)
                {
                    //showmsg(1, err.Message.ToString(), 2);
                }
            }
            return res.ToUpper();
        }
        public static string GetError(Exception exception)
        {
            int lineno = 0;
            int i = 0;
            string fName = "";
            StackFrame fram;
            try
            {
                do
                {
                    fram = new System.Diagnostics.StackTrace(exception, true).GetFrame(i);
                    lineno = fram.GetFileLineNumber();
                    i++;
                }
                while (lineno < 1);
                fName = fram.GetFileName().Split('\\').Last();
            }
            catch (Exception err)
            {
                return exception.Message;
            }
            return lineno + " in File " + fName;
        }
        public static string connString(string UserCode)
        {

            //string path = @"C:\skyinfy\mytns2.txt";
            string path = HttpRuntime.AppDomainAppPath + "\\mytns2.txt";

            string str = "", srv = "", PWD = "", constr = "", IP = "";
            try
            {
                if (File.Exists(path))
                {
                    StreamReader sr = new StreamReader(path);
                    str = sr.ReadToEnd().Trim();
                    if (str.Contains("\r")) str = str.Replace("\r", ",");
                    if (str.Contains("\n")) str = str.Replace("\n", ",");
                    str = str.Replace(",,", ",");
                    IP = str.Split(',')[1]; srv = str.Split(',')[2];

                    sr.Close();
                    PWD = "SATECH";
                    constr = "Data Source=(DESCRIPTION="
                    + "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST= " + IP.Trim() + ")(PORT=1521)))"
                    + "(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=" + srv + ")));"
                    + "User Id=" + UserCode.ToUpper() + "; Password=" + PWD + ";";
                    //"Pooling=true;";

                }
            }
            catch (Exception err) { }
            return constr;
        }
    }
}