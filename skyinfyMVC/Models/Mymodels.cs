using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace skyinfyMVC.Models
{
    public class UserModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }

        public static List<UserModel> getUsers()
        {
            List<UserModel> users = new List<UserModel>()
{
                new UserModel (){ ID=1, Name="Anubhav", SurName="Chaudhary" },
                new UserModel (){ ID=2, Name="Mohit", SurName="Singh" },
                new UserModel (){ ID=3, Name="Sonu", SurName="Garg" },
                new UserModel (){ ID=4, Name="Shalini", SurName="Goel" },
                new UserModel (){ ID=5, Name="James", SurName="Bond" },
};
            return users;
        }
    }
    public class Mymodels
    {
    }
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
    public class login
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
    public class Tmodel
    {
        public string Col1 { get; set; }
        public string Col2 { get; set; }
        public string Col3 { get; set; }
        public string Col4 { get; set; }
        public string Col5 { get; set; }
        public bool Chk1 { get; set; }
        public bool Chk2 { get; set; }
        public bool Chk3 { get; set; }
        public bool Chk4 { get; set; }
        public bool Chk5 { get; set; }


    }
    public class Party
    {
        public string Acode { get; set; }
        public string Aname { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string Pincode { get; set; }
    }
    public class ADJTYPE_ACCOUNT
    {
        public string Adj_type_code { get; set; }
        public string Adj_type { get; set; }
    }
    public class Tmodel5
    {
        public string Col1 { get; set; }
        public string Col2 { get; set; }
        public string Col3 { get; set; }
        public string Col4 { get; set; }
        public string Col5 { get; set; }
        public bool Chk1 { get; set; }
        public bool Chk2 { get; set; }
        public bool Chk3 { get; set; }
        public bool Chk4 { get; set; }
        public bool Chk5 { get; set; }


    }

    public class TableResultModel
    {

        public DataTable CampaignDataTable { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }
        public int iPageRecords { get; set; }
        public bool divtempl { get; set; }
        public string hforderby { get; set; }
        public string hfddcolumns { get; set; }
        public string txttname { get; set; }
        public string hfddpages { get; set; }
        public string hfddshowcols { get; set; }
        public string hfGridView1SV { get; set; }
        public string hfGridView1SH { get; set; }
        public string lbltotrecords { get; set; }
        public string txtsearch { get; set; }
        public string selectedrows { get; set; }
        public string[] ddpagesSelected { get; set; }
        public List<SelectListItem> ddpages { get; set; }


        public string[] ddcolumnsSelected { get; set; }
        public List<SelectListItem> ddcolumns { get; set; }

        public string[] ddshowcolsSelected { get; set; }
        public List<SelectListItem> ddshowcols { get; set; }

        public string[] ddorderbySelected { get; set; }
        public List<SelectListItem> ddorderby { get; set; }
        public List<Filters> filters { get; set; }
        public string Myguid { get; set; }
        public string error { get; set; }
        public TableResultModel()
        {


            ddcolumns = new List<SelectListItem>();
            ddshowcols = new List<SelectListItem>();
            ddorderby = new List<SelectListItem>();
            ddpages = new List<SelectListItem>();

            ddcolumnsSelected = new string[] { "" };
            ddshowcolsSelected = new string[] { "" };
            ddorderbySelected = new string[] { "" };
            ddpagesSelected = new string[] { "" };
            SortColumn = "";
            SortDirection = "";
            iPageRecords = 0;
            divtempl = false;
            hforderby = "";
            hfddcolumns = "";
            txttname = "";
            txttname = "";
            hfddpages = "";
            hfddshowcols = "";
            hfGridView1SV = "";
            hfGridView1SH = "";

            lbltotrecords = "";
            txtsearch = "";
            selectedrows = "";
            CampaignDataTable = new DataTable();
            filters = new List<Filters>();
            Myguid = "";
            error = "N";


        }

        public string sumofopening { get; set; }
        public string sumofdramt { get; set; }
        public string sumofcramt { get; set; }
        public string sumofclosing { get; set; }
        public string showtot { get; set; }

    }
    public class Filters
    {

        public string[] ddoperatorsSelected { get; set; }
        public List<SelectListItem> ddoperators { get; set; }

        public string[] ddnamesSelected { get; set; }
        public List<SelectListItem> ddnames { get; set; }

        public string txtsval { get; set; }
        public Filters()
        {
            ddoperators = new List<SelectListItem>();
            ddnames = new List<SelectListItem>();
            ddoperatorsSelected = new string[] { "" };
            ddnamesSelected = new string[] { "" };
            txtsval = "";
        }
    }

    public class Ddl
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Tmodel10
    {
        public string Col1 { get; set; }
        public string Col2 { get; set; }
        public string Col3 { get; set; }
        public string Col4 { get; set; }
        public string Col5 { get; set; }
        public string Col6 { get; set; }
        public string Col7 { get; set; }
        public string Col8 { get; set; }
        public string Col9 { get; set; }
        public string Col10 { get; set; }
        public bool Chk1 { get; set; }
        public bool Chk2 { get; set; }
        public bool Chk3 { get; set; }
        public bool Chk4 { get; set; }
        public bool Chk5 { get; set; }

    }
    public class Tmodel15
    {
        public string Col1 { get; set; }
        public string Col2 { get; set; }
        public string Col3 { get; set; }
        public string Col4 { get; set; }
        public string Col5 { get; set; }
        public string Col6 { get; set; }
        public string Col7 { get; set; }
        public string Col8 { get; set; }
        public string Col9 { get; set; }
        public string Col10 { get; set; }
        public string Col11 { get; set; }
        public string Col12 { get; set; }
        public string Col13 { get; set; }
        public string Col14 { get; set; }
        public string Col15 { get; set; }
        public bool Chk1 { get; set; }
        public bool Chk2 { get; set; }
        public bool Chk3 { get; set; }
        public bool Chk4 { get; set; }
        public bool Chk5 { get; set; }

    }
    public class Tmodel20
    {
        public string Col1 { get; set; }
        public string Col2 { get; set; }
        public string Col3 { get; set; }
        public string Col4 { get; set; }
        public string Col5 { get; set; }
        public string Col6 { get; set; }
        public string Col7 { get; set; }
        public string Col8 { get; set; }
        public string Col9 { get; set; }
        public string Col10 { get; set; }
        public string Col11 { get; set; }
        public string Col12 { get; set; }
        public string Col13 { get; set; }
        public string Col14 { get; set; }
        public string Col15 { get; set; }
        public string Col16 { get; set; }
        public string Col17 { get; set; }
        public string Col18 { get; set; }
        public string Col19 { get; set; }
        public string Col20 { get; set; }
        public bool Chk1 { get; set; }
        public bool Chk2 { get; set; }
        public bool Chk3 { get; set; }
        public bool Chk4 { get; set; }
        public bool Chk5 { get; set; }
        public string[] SelectedItems1 { get; set; }
        public List<SelectListItem> TList { get; set; }


    }

    public class Tmodelmain
    {

        public string Col1 { get; set; }
        public string Col2 { get; set; }
        public string Col3 { get; set; }
        public string Col4 { get; set; }
        public string Col5 { get; set; }
        public string Col6 { get; set; }
        public string Col7 { get; set; }
        public string Col8 { get; set; }
        public string Col9 { get; set; }
        public string Col10 { get; set; }
        public string Col11 { get; set; }
        public string Col12 { get; set; }
        public string Col13 { get; set; }
        public string Col14 { get; set; }
        public string Col15 { get; set; }
        public string Col16 { get; set; }
        public string Col17 { get; set; }
        public string Col18 { get; set; }
        public string Col19 { get; set; }
        [Required]
        [AllowHtml]
        public string Col20 { get; set; }
        public string Col21 { get; set; }
        public string Col22 { get; set; }
        public string Col23 { get; set; }
        public string Col24 { get; set; }
        public string Col25 { get; set; }
        public string Col26 { get; set; }
        public string Col27 { get; set; }
        public string Col28 { get; set; }
        public string Col29 { get; set; }
        public string Col30 { get; set; }
        public string Col31 { get; set; }
        public string Col32 { get; set; }
        public string Col33 { get; set; }
        public string Col34 { get; set; }
        public string Col35 { get; set; }
        public string Col36 { get; set; }
        public string Col37 { get; set; }
        public string Col38 { get; set; }
        public string Col39 { get; set; }
        public string Col40 { get; set; }
        public string Col41 { get; set; }
        public string Col42 { get; set; }
        public string Col43 { get; set; }
        public string Col44 { get; set; }
        public string Col45 { get; set; }
        public string Col46 { get; set; }
        public string Col47 { get; set; }
        public string Col48 { get; set; }
        public string Col49 { get; set; }
        public string Col50 { get; set; }
        public string Col51 { get; set; }
        public string Col52 { get; set; }
        public string Col53 { get; set; }
        public string Col54 { get; set; }
        public string Col55 { get; set; }
        public string Col56 { get; set; }
        public string Col57 { get; set; }
        public string Col58 { get; set; }
        public string Col59 { get; set; }
        public string Col60 { get; set; }
        public string Col61 { get; set; }
        public string Col62 { get; set; }
        public string Col63 { get; set; }
        public string Col64 { get; set; }
        public string Col65 { get; set; }
        public string Col66 { get; set; }
        public string Col67 { get; set; }
        public string Col68 { get; set; }
        public string Col69 { get; set; }
        public string Col70 { get; set; }
        public string Col71 { get; set; }
        public string Col72 { get; set; }
        public string Col73 { get; set; }
        public string Col74 { get; set; }
        public string Col75 { get; set; }
        public string Col76 { get; set; }
        public string Col77 { get; set; }
        public string Col78 { get; set; }
        public string Col79 { get; set; }
        public string Col80 { get; set; }
        public string Col81 { get; set; }
        public string Col82 { get; set; }
        public string Col83 { get; set; }
        public string Col84 { get; set; }
        public string Col85 { get; set; }
        public string Col86 { get; set; }
        public string Col87 { get; set; }
        public string Col88 { get; set; }
        public string Col89 { get; set; }
        public string Col90 { get; set; }
        public string Col91 { get; set; }
        public string Col92 { get; set; }
        public string Col93 { get; set; }
        public string Col94 { get; set; }
        public string Col95 { get; set; }
        public string Col96 { get; set; }
        public string Col97 { get; set; }
        public string Col98 { get; set; }
        public string Col99 { get; set; }
        public string Col100 { get; set; }
        public string Col101 { get; set; }
        public string Col102 { get; set; }
        public string Col103 { get; set; }
        public string Col104 { get; set; }
        public string Col105 { get; set; }
        public string Col106 { get; set; }
        public string Col107 { get; set; }
        public string Col108 { get; set; }
        public string Col109 { get; set; }
        public string Col110 { get; set; }
        public string Col111 { get; set; }
        public string Col112 { get; set; }
        public string Col113 { get; set; }
        public string Col114 { get; set; }
        public string Col115 { get; set; }
        public string Col116 { get; set; }
        public string Col117 { get; set; }
        public string Col118 { get; set; }
        public string Col119 { get; set; }
        public string Col120 { get; set; }
        public string Col121 { get; set; }
        [AllowHtml] public string Col122 { get; set; }
        [AllowHtml] public string Col123 { get; set; }
        public string Col124 { get; set; }
        public string Col125 { get; set; }
        public string Col126 { get; set; }
        public string Col127 { get; set; }
        public string Col128 { get; set; }
        public string Col129 { get; set; }
        public string Col130 { get; set; }
        public string Col131 { get; set; }
        [AllowHtml] public string Col132 { get; set; }
        [AllowHtml] public string Col133 { get; set; }     // new permission

        // public string Col132 { get; set; }
        //public string Col133 { get; set; }     // new permission


        public string Col134 { get; set; }           // edit permission    
        public string Col135 { get; set; }               // view permission 
        public string Col136 { get; set; }                // extend permission
        public string Col137 { get; set; }
        public string Col138 { get; set; }
        public string Col139 { get; set; }
        public string Col140 { get; set; }
        public string Col141 { get; set; }
        public string Col142 { get; set; }
        public string Col143 { get; set; }
        public string Col144 { get; set; }
        public string Col145 { get; set; }
        public string Col146 { get; set; }
        public string Col147 { get; set; }
        public string Col148 { get; set; }
        public string Col149 { get; set; }
        public string Col150 { get; set; }
        public string Col151 { get; set; }
        public string Col152 { get; set; }
        public string Col153 { get; set; }
        public string Col154 { get; set; }
        public string Col155 { get; set; }
        public string Col156 { get; set; }
        public string Col157 { get; set; }
        public string Col158 { get; set; }
        public string Col159 { get; set; }
        public string Col160 { get; set; }
        public string Col161 { get; set; }
        public string Col162 { get; set; }
        public string Col163 { get; set; }
        public string Col164 { get; set; }
        public string Col165 { get; set; }
        public string Col166 { get; set; }
        public string Col167 { get; set; }
        public string Col168 { get; set; }
        public string Col169 { get; set; }
        public string Col170 { get; set; }
        public string Col171 { get; set; }
        public string Col172 { get; set; }
        public string Col173 { get; set; }
        public string Col174 { get; set; }
        public string Col175 { get; set; }
        public string Col176 { get; set; }


        public bool Chk1 { get; set; }
        public bool Chk2 { get; set; }
        public bool Chk3 { get; set; }
        public bool Chk4 { get; set; }
        public bool Chk5 { get; set; }
        public bool Chk6 { get; set; }
        public bool Chk7 { get; set; }
        public bool Chk8 { get; set; }
        public bool Chk9 { get; set; }
        public bool Chk10 { get; set; }
        public bool Chk11 { get; set; }
        public bool Chk12 { get; set; }
        public bool Chk13 { get; set; }
        public bool Chk14 { get; set; }
        public bool Chk15 { get; set; }
        public bool Chk16 { get; set; }
        public bool Chk17 { get; set; }
        public bool Chk18 { get; set; }
        public bool Chk19 { get; set; }
        public bool Chk20 { get; set; }
        public bool Chk21 { get; set; }
        public bool Chk22 { get; set; }
        public bool Chk23 { get; set; }
        public bool Chk24 { get; set; }
        public bool Chk25 { get; set; }
        public bool Chk26 { get; set; }
        public bool Chk27 { get; set; }
        public bool Chk28 { get; set; }
        public bool Chk29 { get; set; }
        public bool Chk30 { get; set; }
        public bool Chk31 { get; set; }
        public bool Chk32 { get; set; }
        public bool Chk33 { get; set; }
        public bool Chk34 { get; set; }
        public bool Chk35 { get; set; }
        public bool Chk36 { get; set; }
        public bool Chk37 { get; set; }
        public bool Chk38 { get; set; }

        public string[] SelectedItems1 { get; set; }
        public string[] SelectedItems2 { get; set; }
        public string[] SelectedItems3 { get; set; }
        public string[] SelectedItems4 { get; set; }
        public string[] SelectedItems5 { get; set; }
        public string[] SelectedItems6 { get; set; }
        public string[] SelectedItems7 { get; set; }
        public string[] SelectedItems8 { get; set; }
        public string[] SelectedItems9 { get; set; }
        public string[] SelectedItems10 { get; set; }
        public string[] SelectedItems11 { get; set; }
        public string[] SelectedItems12 { get; set; }
        public string[] SelectedItems13 { get; set; }
        public string[] SelectedItems14 { get; set; }
        public string[] SelectedItems15 { get; set; }
        public string[] SelectedItems16 { get; set; }
        public string[] SelectedItems17 { get; set; }
        public string[] SelectedItems18 { get; set; }
        public string[] SelectedItems19 { get; set; }
        public string[] SelectedItems20 { get; set; }
        public string[] SelectedItems21 { get; set; }
        public string[] SelectedItems22 { get; set; }
        public string[] SelectedItems23 { get; set; }
        public string[] SelectedItems24 { get; set; }
        public string[] SelectedItems25 { get; set; }
        public string[] SelectedItems26 { get; set; }
        public string[] SelectedItems27 { get; set; }
        public string[] SelectedItems28 { get; set; }
        public string[] SelectedItems29 { get; set; }
        public string[] SelectedItems30 { get; set; }
        public string[] SelectedItems31 { get; set; }
        public string[] SelectedItems32 { get; set; }
        public string[] SelectedItems33 { get; set; }
        public string[] SelectedItems34 { get; set; }
        public string[] SelectedItems35 { get; set; }

        public List<SelectListItem> TList1 { get; set; }
        public List<SelectListItem> TList2 { get; set; }
        public List<SelectListItem> TList3 { get; set; }
        public List<SelectListItem> TList4 { get; set; }
        public List<SelectListItem> TList5 { get; set; }
        public List<SelectListItem> TList6 { get; set; }
        public List<SelectListItem> TList7 { get; set; }
        public List<SelectListItem> TList8 { get; set; }
        public List<SelectListItem> TList9 { get; set; }
        public List<SelectListItem> TList10 { get; set; }
        public List<SelectListItem> TList11 { get; set; }
        public List<SelectListItem> TList12 { get; set; }
        public List<SelectListItem> TList13 { get; set; }
        public List<SelectListItem> TList14 { get; set; }
        public List<SelectListItem> TList15 { get; set; }
        public List<SelectListItem> TList16 { get; set; }
        public List<SelectListItem> TList17 { get; set; }
        public List<SelectListItem> TList18 { get; set; }
        public List<SelectListItem> TList19 { get; set; }
        public List<SelectListItem> TList20 { get; set; }
        public List<SelectListItem> TList21 { get; set; }
        public List<SelectListItem> TList22 { get; set; }
        public List<SelectListItem> TList23 { get; set; }
        public List<SelectListItem> TList24 { get; set; }
        public List<SelectListItem> TList25 { get; set; }
        public List<SelectListItem> TList26 { get; set; }
        public List<SelectListItem> TList27 { get; set; }
        public List<SelectListItem> TList28 { get; set; }
        public List<SelectListItem> TList29 { get; set; }
        public List<SelectListItem> TList30 { get; set; }
        public List<SelectListItem> TList31 { get; set; }
        public List<SelectListItem> TList32 { get; set; }
        public List<SelectListItem> TList33 { get; set; }
        public List<SelectListItem> TList34 { get; set; }
        public List<SelectListItem> TList35 { get; set; }

        public DataTable dt1 { get; set; }
        public DataTable dt2 { get; set; }
        public DataTable dt3 { get; set; }
        public DataTable dt4 { get; set; }
        public DataTable dt5 { get; set; }
        public List<Tmodelmain> LTM1 { get; set; }
        public List<Tmodelmain> LTM2 { get; set; }
        public List<Tmodelmain> LTM3 { get; set; }
        public List<Tmodelmain> LTM4 { get; set; }
        public List<HttpPostedFileBase> File1 { get; set; }
        public List<HttpPostedFileBase> File2 { get; set; }
        public List<HttpPostedFileBase> File3 { get; set; }
        public HttpPostedFileBase File_S1 { get; set; }
        public List<Files> Files1 { get; set; }
        public Tmodelmain()
        {

            Col20 = Col19 = Col18 = Col17 = Col16 = Col15 = Col14 = Col13 = Col12 = Col11 = Col10 = Col9 = Col8 = Col7 = Col6 = Col5 = Col4 = Col3 = Col2 = Col1 = "";
            Col40 = Col39 = Col38 = Col37 = Col36 = Col35 = Col34 = Col33 = Col32 = Col31 = Col30 = Col29 = Col28 = Col27 = Col26 = Col25 = Col24 = Col23 = Col22 = Col21 = "";
            Col60 = Col59 = Col58 = Col57 = Col56 = Col55 = Col54 = Col53 = Col52 = Col51 = Col50 = Col49 = Col48 = Col47 = Col46 = Col45 = Col44 = Col43 = Col42 = Col41 = "";
            Col80 = Col79 = Col78 = Col77 = Col76 = Col75 = Col74 = Col73 = Col72 = Col71 = Col70 = Col69 = Col68 = Col67 = Col66 = Col65 = Col64 = Col63 = Col62 = Col61 = "";
            Col100 = Col99 = Col98 = Col97 = Col96 = Col95 = Col94 = Col93 = Col92 = Col91 = Col90 = Col89 = Col88 = Col87 = Col86 = Col85 = Col84 = Col83 = Col82 = Col81 = "";
            Col120 = Col119 = Col118 = Col117 = Col116 = Col115 = Col114 = Col113 = Col112 = Col111 = Col110 = Col109 = Col108 = Col107 = Col106 = Col105 = Col104 = Col103 = Col102 = Col101 = "";

            #region
            //Col1 = "";
            //Col2 = "";
            //Col3 = "";
            //Col4 = "";
            //Col5 = "";
            //Col6 = "";
            //Col7 = "";
            //Col8 = "";
            //Col9 = "";
            //Col10 = "";
            //Col11 = "";
            //Col12 = "";
            //Col13 = "";
            //Col14 = "";
            //Col15 = "";
            //Col16 = "";
            //Col17 = "";
            //Col18 = "";
            //Col19 = "";
            //Col20 = "";
            //Col21 = "";
            //Col22 = "";
            //Col23 = "";
            //Col24 = "";
            //Col25 = "";
            //Col26 = "";
            //Col27 = "";
            //Col28 = "";
            //Col29 = "";
            //Col30 = "";
            //Col31 = "";
            //Col32 = "";
            //Col33 = "";
            //Col34 = "";
            //Col35 = "";
            //Col36 = "";
            //Col37 = "";
            //Col38 = "";
            //Col39 = "";
            //Col40 = "";
            //Col41 = "";
            //Col42 = "";
            //Col43 = "";
            //Col44 = "";
            //Col45 = "";
            //Col46 = "";
            //Col47 = "";
            //Col48 = "";
            //Col49 = "";
            //Col50 = "";
            //Col51 = "";
            //Col52 = "";
            //Col53 = "";
            //Col54 = "";
            //Col55 = "";
            //Col56 = "";
            //Col57 = "";
            //Col58 = "";
            //Col59 = "";
            //Col60 = "";
            //Col61 = "";
            //Col62 = "";
            //Col63 = "";
            //Col64 = "";
            //Col65 = "";
            //Col66 = "";
            //Col67 = "";
            //Col68 = "";
            //Col69 = "";
            //Col70 = "";
            //Col71 = "";
            //Col72 = "";
            //Col73 = "";
            //Col74 = "";
            //Col75 = "";
            //Col76 = "";
            //Col77 = "";
            //Col78 = "";
            //Col79 = "";
            //Col80 = "";
            //Col81 = "";
            //Col82 = "";
            //Col83 = "";
            //Col84 = "";
            //Col85 = "";
            //Col86 = "";
            //Col87 = "";
            //Col88 = "";
            //Col89 = "";
            //Col90 = "";
            //Col91 = "";
            //Col92 = "";
            //Col93 = "";
            //Col94 = "";
            //Col95 = "";
            //Col96 = "";
            //Col97 = "";
            //Col98 = "";
            //Col99 = "";
            //Col100 = "";
            //Col101 = "";
            //Col102 = "";
            //Col103 = "";
            //Col104 = "";
            //Col105 = "";
            //Col106 = "";
            //Col107 = "";
            //Col108 = "";
            //Col109 = "";
            //Col110 = "";
            //Col111 = "";
            //Col112 = "";
            //Col113 = "";
            //Col114 = "";
            //Col115 = "";
            //Col116 = "";
            //Col117 = "";
            //Col118 = "";
            //Col119 = "";
            //Col120 = "";
            #endregion
            Col121 = "";
            Col122 = "";
            Col123 = "";
            Col124 = "";
            Col125 = "";
            Col126 = "";
            Col127 = "";
            Col128 = "";
            Col129 = "";
            Col130 = "";
            Col131 = "";
            Col132 = "";
            Col133 = "";     // new permission

            Col132 = "";
            Col133 = "";     // new permission


            Col134 = "";           // edit permission    
            Col135 = "";               // view permission 
            Col136 = "";                // extend permission
            Col137 = "";
            Col138 = "";
            Col139 = "";
            Col140 = "";
            Col141 = "";
            Col142 = "";
            Col143 = "";
            Col144 = "";
            Col145 = "";
            Col146 = "";
            Col147 = "";
            Col148 = "";
            Col149 = "";
            Col150 = "";

            Chk20 = Chk19 = Chk18 = Chk17 = Chk16 = Chk15 = Chk14 = Chk13 = Chk12 = Chk11 = Chk10 = Chk9 = Chk8 = Chk7 = Chk6 = Chk5 = Chk4 = Chk3 = Chk2 = Chk1 = false;
            Chk38 = Chk37 = Chk36 = Chk35 = Chk34 = Chk33 = Chk32 = Chk31 = Chk30 = Chk29 = Chk28 = Chk27 = Chk26 = Chk25 = Chk24 = Chk23 = Chk22 = Chk21 = false;

            #region
            //Chk1 = false;
            //Chk2 = false;
            //Chk3 = false;
            //Chk4 = false;
            //Chk5 = false;
            //Chk6 = false;
            //Chk7 = false;
            //Chk8 = false;
            //Chk9 = false;
            //Chk10 = false;
            //Chk11 = false;
            //Chk12 = false;
            //Chk13 = false;
            //Chk14 = false;
            //Chk15 = false;
            //Chk16 = false;
            //Chk17 = false;
            //Chk18 = false;
            //Chk19 = false;
            //Chk20 = false;
            //Chk21 = false;
            //Chk22 = false;
            //Chk23 = false;
            //Chk24 = false;
            //Chk25 = false;
            //Chk26 = false;
            //Chk27 = false;
            //Chk28 = false;
            //Chk29 = false;
            //Chk30 = false;
            //Chk31 = false;
            //Chk32 = false;
            //Chk33 = false;
            //Chk34 = false;
            //Chk35 = false;
            //Chk36 = false;
            //Chk37 = false;
            //Chk38 = false;
            #endregion

            SelectedItems10 = SelectedItems9 = SelectedItems8 = SelectedItems7 = SelectedItems6 = SelectedItems5 = SelectedItems4 = SelectedItems3 = SelectedItems2 = SelectedItems1 = new string[] { "" };
            SelectedItems20 = SelectedItems19 = SelectedItems18 = SelectedItems17 = SelectedItems16 = SelectedItems15 = SelectedItems14 = SelectedItems13 = SelectedItems12 = SelectedItems11 = new string[] { "" };
            SelectedItems30 = SelectedItems29 = SelectedItems28 = SelectedItems27 = SelectedItems26 = SelectedItems25 = SelectedItems24 = SelectedItems23 = SelectedItems22 = SelectedItems21 = new string[] { "" };
            SelectedItems31 = new string[] { "" };
            #region
            //SelectedItems1 = new string[] { "" };
            //SelectedItems2 = new string[] { "" };
            //SelectedItems3 = new string[] { "" };
            //SelectedItems4 = new string[] { "" };
            //SelectedItems5 = new string[] { "" };
            //SelectedItems6 = new string[] { "" };
            //SelectedItems7 = new string[] { "" };
            //SelectedItems8 = new string[] { "" };
            //SelectedItems9 = new string[] { "" };
            //SelectedItems10 = new string[] { "" };
            //SelectedItems11 = new string[] { "" };
            //SelectedItems12 = new string[] { "" };
            //SelectedItems13 = new string[] { "" };
            //SelectedItems14 = new string[] { "" };
            //SelectedItems15 = new string[] { "" };
            //SelectedItems16 = new string[] { "" };
            //SelectedItems17 = new string[] { "" };
            //SelectedItems18 = new string[] { "" };
            //SelectedItems19 = new string[] { "" };
            //SelectedItems20 = new string[] { "" };
            //SelectedItems21 = new string[] { "" };
            //SelectedItems22 = new string[] { "" };
            //SelectedItems23 = new string[] { "" };
            //SelectedItems24 = new string[] { "" };
            //SelectedItems25 = new string[] { "" };
            //SelectedItems26 = new string[] { "" };
            //SelectedItems27 = new string[] { "" };
            //SelectedItems28 = new string[] { "" };
            //SelectedItems29 = new string[] { "" };
            //SelectedItems30 = new string[] { "" };
            #endregion

            TList20 = TList19 = TList18 = TList17 = TList16 = TList15 = TList14 = TList13 = TList12 = TList11 = TList10 = TList9 = TList8 = TList7 = TList6 = TList5 = TList4 = TList3 = TList2 = TList1 = new List<SelectListItem>();
            TList31 = TList30 = TList29 = TList28 = TList27 = TList26 = TList25 = TList24 = TList23 = TList22 = TList21 = new List<SelectListItem>();
            #region
            //TList1 = new List<SelectListItem>();
            //TList2 = new List<SelectListItem>();
            //TList3 = new List<SelectListItem>();
            //TList4 = new List<SelectListItem>();
            //TList5 = new List<SelectListItem>();
            //TList6 = new List<SelectListItem>();
            //TList7 = new List<SelectListItem>();
            //TList8 = new List<SelectListItem>();
            //TList9 = new List<SelectListItem>();
            //TList10 = new List<SelectListItem>();
            //TList11 = new List<SelectListItem>();
            //TList12 = new List<SelectListItem>();
            //TList13 = new List<SelectListItem>();
            //TList14 = new List<SelectListItem>();
            //TList15 = new List<SelectListItem>();
            //TList16 = new List<SelectListItem>();
            //TList17 = new List<SelectListItem>();
            //TList18 = new List<SelectListItem>();
            //TList19 = new List<SelectListItem>();
            //TList20 = new List<SelectListItem>();
            //TList21 = new List<SelectListItem>();
            //TList22 = new List<SelectListItem>();
            //TList23 = new List<SelectListItem>();
            //TList24 = new List<SelectListItem>();
            //TList25 = new List<SelectListItem>();
            //TList26 = new List<SelectListItem>();
            //TList27 = new List<SelectListItem>();
            //TList28 = new List<SelectListItem>();
            //TList29 = new List<SelectListItem>();
            //TList30 = new List<SelectListItem>();
            //TList31 = new List<SelectListItem>();
            #endregion
            dt4 = dt3 = dt2 = dt1 = new DataTable();
            //dt2 = new DataTable();
            //dt3 = new DataTable();
            //dt4 = new DataTable();
            LTM4 = LTM3 = LTM2 = LTM1 = new List<Tmodelmain>();
            //LTM2 = new List<Tmodelmain>();
            //LTM3 = new List<Tmodelmain>();
            //LTM4 = new List<Tmodelmain>();
            File3 = File2 = File1 = new List<HttpPostedFileBase>();
            //File2 = new List<HttpPostedFileBase>();
            //File3 = new List<HttpPostedFileBase>();
            Files1 = new List<Files>();
        }
    }

    public class Item
    {
        public string Icode { get; set; }
        public string Iname { get; set; }
        public string partno { get; set; }
        public string uom { get; set; }
        public string hsn { get; set; }
        public string taxrate { get; set; }
        public string stock { get; set; }
    }
    public class Files
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public HttpPostedFileBase PostedFile { get; set; }
        public string FileBase64 { get; set; }
        public string FileTitle { get; set; }
        public string FileDesc { get; set; }
    }
    public class Select2PagedResult
    {
        public int Total { get; set; }
        public List<Select2Result> Results { get; set; }
    }

    public class Select2Result
    {
        public string id { get; set; }
        public string text { get; set; }
    }
    public class JsonpResult : JsonResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            var request = context.HttpContext.Request;
            var response = context.HttpContext.Response;
            string jsoncallback = (context.RouteData.Values["callback"] as string) ?? request["callback"];
            if (!string.IsNullOrEmpty(jsoncallback))
            {
                if (string.IsNullOrEmpty(base.ContentType))
                {
                    base.ContentType = "application/x-javascript";
                }
                response.Write(string.Format("{0}(", jsoncallback));
            }
            base.ExecuteResult(context);
            if (!string.IsNullOrEmpty(jsoncallback))
            {
                response.Write(")");
            }
        }
    }

    public class MailObjs
    {

        public string smtp { get; set; }
        public string fromAddress { get; set; }
        public string fromName { get; set; }
        public string fromPwd { get; set; }
        public string toAddress { get; set; }
        public string CC { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public bool ssl { get; set; }
        public int port { get; set; }
        public IList<HttpPostedFile> fileCollection { get; set; }
    }
    public class SMSObjs
    {

        public string ApiUrl { get; set; }
        public bool Unicode { get; set; }

    }
    public class SMSRES
    {
        public string result { get; set; }


    }
    public class DataTableData
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        //public List<DataItem> data { get; set; }
        public DataTable data { get; set; }
    }
    public class MyCustomerBinder : DefaultModelBinder
    {
        protected override void OnModelUpdated(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            base.OnModelUpdated(controllerContext, bindingContext);

            controllerContext.Controller.TempData["model"] = bindingContext.Model;
        }
    }

    public class KCFile {

        public string FileName { get; set; }

        public string ContentType { get; set; }

        public long ContentLength { get; set; }

        public Stream InputStream { get; set; }
    }
}