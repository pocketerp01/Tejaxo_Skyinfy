using Newtonsoft.Json;
using skyinfyMVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace skyinfyMVC.Controllers
{
    public class GridController : Controller
    {
        public static string userCode = "", subdomain_mst = "", userid_mst = "", cg_com_name = "", clientid_mst = "", unitid_mst = "", html = "", Ac_Year_id = "", msg = "", role_mst = "",
             controls_mst = "", actionName = "", controllerName = "", clientname_mst = "", type1 = "", unitname_mst, MyGuid = "";

        sgenFun sgen;
        Cmd_Fun cmd_Fun;
        public void FillMst(string Myguid="")
        {
            MyGuid = Myguid;
            //sgen = new sgenFun(MyGuid);
            if (MyGuid == "") { MyGuid = EncryptDecrypt.Decrypt(Request.QueryString["m_id"]); }
            //if (MyGuid == "") MyGuid = sgen.GetCookie("", "MyGuid");
            Multiton multiton = Multiton.GetInstance(MyGuid);
            sgen = new sgenFun(MyGuid);
            cmd_Fun = new Cmd_Fun(MyGuid);

            userCode = multiton.UserCode;
            userid_mst = sgen.GetCookie(MyGuid, "userid_mst");
            Ac_Year_id = sgen.GetCookie(MyGuid, "Ac_Year_id");
            cg_com_name = sgen.GetCookie(MyGuid, "cg_com_name");
            clientid_mst = sgen.GetCookie(MyGuid, "clientid_mst");
            unitid_mst = sgen.GetCookie(MyGuid, "unitid_mst");
            subdomain_mst = sgen.GetCookie(MyGuid, "subdomain_mst");
            role_mst = sgen.GetCookie(MyGuid, "role_mst");
            controls_mst = sgen.GetCookie(MyGuid, "controls_mst");
            clientname_mst = sgen.GetCookie(MyGuid, "clientname_mst");
            unitname_mst = sgen.GetCookie(MyGuid, "unitname_mst");
            actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            sgen.SetSession(MyGuid, "viewName", actionName);
            sgen.SetSession(MyGuid, "controllerName", controllerName);
        }

        // GET: Grid
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult gridag()
        {
            return View();
        }
        [HttpPost]
        public ActionResult gridag(string model)
        {
            //List<UserModel> users = UserModel.getUsers();        
            return View();
        }

        private const int TOTAL_ROWS = 995;
        private static readonly List<DataItem> _data = CreateData();

        public class DataItem
        {
            public string Name { get; set; }
            public string Age { get; set; }
            public string DoB { get; set; }
        }

        public class DataTableData
        {
            public int draw { get; set; }
            public int recordsTotal { get; set; }
            public int recordsFiltered { get; set; }
            //public List<DataItem> data { get; set; }
            public DataTable data { get; set; }
        }

        // here we simulate data from a database table. 
        // !!!!DO NOT DO THIS IN REAL APPLICATION !!!!
        private static List<DataItem> CreateData()
        {
            Random rnd = new Random();
            List<DataItem> list = new List<DataItem>();
            for (int i = 1; i <= TOTAL_ROWS; i++)
            {
                DataItem item = new DataItem();
                item.Name = "Name_" + i.ToString().PadLeft(5, '0');
                DateTime dob = new DateTime(1900 + rnd.Next(1, 100), rnd.Next(1, 13), rnd.Next(1, 28));
                item.Age = ((DateTime.Now - dob).Days / 365).ToString();
                item.DoB = dob.ToShortDateString();
                list.Add(item);
            }
            return list;
        }

        private static DataTable CreateData1()
        {
            Random rnd = new Random();
            DataTable list = new DataTable();
            list.Columns.Add("Name");
            list.Columns.Add("Age");
            list.Columns.Add("DoB");
            for (int i = 1; i <= TOTAL_ROWS; i++)
            {
                DataRow dr = list.NewRow();

                DateTime dob = new DateTime(1900 + rnd.Next(1, 100), rnd.Next(1, 13), rnd.Next(1, 28));
                dr["Name"] = "Name_" + i.ToString().PadLeft(5, '0');
                dr["DoB"] = dob;
                dr["Age"] = ((DateTime.Now - dob).Days / 365).ToString();
                list.Rows.Add(dr);
            }
            return list;
        }

        private int SortString(string s1, string s2, string sortDirection)
        {
            return sortDirection == "asc" ? s1.CompareTo(s2) : s2.CompareTo(s1);
        }

        private int SortInteger(string s1, string s2, string sortDirection)
        {
            int i1 = int.Parse(s1);
            int i2 = int.Parse(s2);
            return sortDirection == "asc" ? i1.CompareTo(i2) : i2.CompareTo(i1);
        }

        private int SortDateTime(string s1, string s2, string sortDirection)
        {
            DateTime d1 = DateTime.Parse(s1);
            DateTime d2 = DateTime.Parse(s2);
            return sortDirection == "asc" ? d1.CompareTo(d2) : d2.CompareTo(d1);
        }

        // here we simulate SQL search, sorting and paging operations
        // !!!! DO NOT DO THIS IN REAL APPLICATION !!!!
        private List<DataItem> FilterData(ref int recordFiltered, int start, int length, string search, int sortColumn, string sortDirection)
        {
            List<DataItem> list = new List<DataItem>();
            if (search == null)
            {
                list = _data;
            }
            else
            {
                // simulate search
                foreach (DataItem dataItem in _data)
                {
                    if (dataItem.Name.ToUpper().Contains(search.ToUpper()) ||
                        dataItem.Age.ToString().Contains(search.ToUpper()) ||
                        dataItem.DoB.ToString().Contains(search.ToUpper()))
                    {
                        list.Add(dataItem);
                    }
                }
            }
            // simulate sort
            if (sortColumn == 0)
            {// sort Name
                list.Sort((x, y) => SortString(x.Name, y.Name, sortDirection));
            }
            else if (sortColumn == 1)
            {// sort Age
                list.Sort((x, y) => SortInteger(x.Age, y.Age, sortDirection));
            }
            else if (sortColumn == 2)
            {   // sort DoB
                list.Sort((x, y) => SortDateTime(x.DoB, y.DoB, sortDirection));
            }
            recordFiltered = list.Count;
            // get just one page of data
            list = list.GetRange(start, Math.Min(length, list.Count - start));
            return list;
        }

       


        public ActionResult grid1()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoadData(string myguid)
        {
            try
            {
                FillMst(myguid);
                string sortDirection = "asc";
                //Creating instance of DatabaseContext class  
                //using (DatabaseContext _context = new DatabaseContext())
                //{
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
                var gridid = Request.Form.GetValues("gridid").FirstOrDefault().Trim().ToLower();

                var pageno = "1";
                string basequey = sgen.GetSession(MyGuid, "gridq_" + gridid + "").ToString();
                string where = "";
                string cond = "";
                if (sgen.GetSession(MyGuid, "dt_" + gridid + "") != null)
                {
                    var dtcols = (DataTable)sgen.GetSession(MyGuid, "dt_" + gridid + "");
                    string opname = "", colname = "", sval = "", searchval = "", colswhere = "";
                    for (int k = 0; k < dtcols.Columns.Count; k++)
                    {
                        if (colswhere.Equals("")) colswhere = "NVL(" + dtcols.Columns[k].ColumnName + ",'0')";
                        else colswhere = colswhere + "||" + "NVL(" + dtcols.Columns[k].ColumnName + ",'0') ";
                        try
                        {
                            colname = dtcols.Columns[k].ColumnName.ToString();
                            sval = Request.Form.GetValues("columns[" + k + "][search][value]").FirstOrDefault();
                        }
                        catch (Exception err)
                        { }
                        if (!sval.Equals(""))
                        {
                            if (!sval.Trim().Equals("")) cond += " and lower(NVL(" + colname + ",'0')) LIKE '%" + sval + "%'";
                        }
                    }
                    if (!colswhere.Trim().Equals("")) where = " where lower(" + colswhere + ") LIKE '%" + searchValue.ToLower() + "%'  " + cond;
                }
                string query = "SELECT tab.*,rownum as Sr_No  from (" + basequey + " ) tab " + where;
                //Paging Size(10,20,50,100)    
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                string orderby = "";
                if (sortColumn.Trim() != "")
                {
                    orderby = "order by " + sortColumn + " " + sortColumnDir;
                }
                //int pageSize = 10;
                //int skip = 0;
                //int recordsTotal = 0;
                string mq0 = "SELECT Count(*) as cnt from (" + query + " ) tab";
                string query3 = "SELECT tab.* FROM( " + query + ") TAB WHERE Sr_No BETWEEN " + start + " + 1 AND " + (sgen.Make_int(start) + sgen.Make_int(length)) + " " + orderby;
                DataSet dsm = sgen.Get_SP2Q(userCode, mq0, query3);
                string rows = dsm.Tables[0].Rows[0][0].ToString();
                DataTable dt = dsm.Tables[1].Copy();
                DataTableData dataTable = new DataTableData();
                dataTable.recordsFiltered = sgen.Make_int(rows);
                dataTable.recordsTotal = sgen.Make_int(rows);
                dataTable.draw = Convert.ToInt32(draw);
                dataTable.data = dt;
                return Json(JsonConvert.SerializeObject(dataTable), JsonRequestBehavior.AllowGet);
                //}
            }
            catch (Exception err)
            {
                throw;
            }
        }
        [HttpPost]
        public JsonResult GetCols(string gridid,string myguid)
        {
            FillMst(myguid);
            //DataTable dt = new DataTable();
            //dt.Columns.Add("CustomerID");
            //dt.Columns.Add("CompanyName");
            //dt.Columns.Add("ContactName");
            //dt.Columns.Add("ContactTitle");
            //dt.Columns.Add("City");
            //dt.Columns.Add("PostalCode");
            //dt.Columns.Add("Country");
            //dt.Columns.Add("Phone");
            //DataRow drr = dt.NewRow();
            //dt.Rows.Add(drr);

            string mq = sgen.GetSession(MyGuid, "gridq_" + gridid.ToLower() + "").ToString();
            mq = "select * from (" + mq + ") where rownum=1";
            DataTable dt = sgen.getdata(userCode, mq);
            sgen.SetSession(MyGuid,"dt_" + gridid.ToLower() + "", dt);
            return Json(JsonConvert.SerializeObject(dt), JsonRequestBehavior.AllowGet);
        }

    }
}