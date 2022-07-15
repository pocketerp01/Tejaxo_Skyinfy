using System.Web;
using System.Web.Optimization;

namespace skyinfyMVC
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at https://modernizr.com to pick only the tests you need.

            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));
            
            //========================
            #region css master mvc
            bundles.Add(new StyleBundle("~/bundles/linkedcss").Include(
        "~/vendors/bootstrap/dist/css/bootstrap.min.css",
        "~/Content/css/bootstrap-toggle.min.css",
        "~/js/toast/toastrsample.css",
        "~/js/toast/toastr.min.css",
        "~/vendors/bootstrap-daterangepicker/daterangepicker.css",
        "~/vendors/bootstrap-datetimepicker/build/css/bootstrap-datetimepicker.css",
        "~/vendors/font-awesome/css/font-awesome.min.css",
        "~/build/css/custom.min.css",
        "~/Content/css/gridviewstyle.css",
        "~/Content/css/colorbox.css",
        //"~/Content/css/sgen.css",
        "~/Content/jquery-ui.min.css",
        "~/Content/css/select2/select2.min.css",
        "~/Scripts/fastselect/fastselect.min.css",
        "~/Scripts/htmlextStyle/jHtmlArea.css",
        "~/Content/css/bootstrap-datepicker.css",
        "~/Scripts/inputmast/css/inputmask.css",
        "~/vendors/nprogress/nprogress.css",
        "~/Content/css/monthly.css"
        ));
            #endregion

            #region css sgen
            bundles.Add(new StyleBundle("~/bundles/sgencss").Include(
"~/Content/css/sgen.css"
));
            #endregion            

            #region js master mvc
            bundles.Add(new ScriptBundle("~/bundles/linkedjs").Include(
                "~/Scripts/jquery-3.1.1.min.js",
                //"~/Scripts/jquery-2.2.4.min.js",                                             
                "~/vendors/jquery/dist/jquery.min.js", 
                "~/Scripts/angular.js",
                "~/Scripts/Global.js",
                //"~/Scripts/gmap/gmaponline.js",
                "~/vendors/bootstrap/dist/js/bootstrap.min.js",
                "~/Scripts/jquery-ui.min.js",
                 "~/js/bootstrap-toggle.min.js",
                "~/js/jquery.toaster.min.js",
                "~/js/toast/toastr.min.js",
                "~/js/toast/toastrsample.min.js",
                "~/vendors/moment/min/moment.min.js",
                "~/vendors/bootstrap-daterangepicker/daterangepicker.min.js",
                "~/vendors/bootstrap-datetimepicker/build/js/bootstrap-datetimepicker.min.js",
                "~/build/js/custom.min.js",
                "~/Scripts/jquery.colorbox-min.js",
                //"~/Scripts/sgen.js",
                "~/Content/css/select2/select2_4.0.6.min.js",
                "~/Scripts/fastselect/fastselect.standalone.min.js",
                "~/Scripts/htmlext/jHtmlArea-0.8.min.js",
                "~/js/bootstrap-datepicker.min.js",
                "~/student/jquery-migrate-1.2.1.min.js",
                "~/Scripts/inputmast/dist/jquery.inputmask.min.js",
                "~/Scripts/decimal/decimal.min.js"
                //"~/js/monthly.js"
                   ));
            #endregion
            
            #region js sgen
            bundles.Add(new ScriptBundle("~/bundles/sgenjs").Include(
                "~/Scripts/sgen.js"
                   ));
            #endregion            

            #region monthly js
            bundles.Add(new ScriptBundle("~/bundles/monthlyjs").Include(
                "~/js/monthly.js"
                   ));
            #endregion

            //========================
            #region css landing page
            bundles.Add(new StyleBundle("~/bundles/lpcss").Include(
"~/vendors/bootstrap/dist/css/bootstrap.min.css",
"~/js/toast/toastrsample.css",
"~/js/toast/toastr.min.css",
"~/Content/landing/Lp_bootstrap.min.css",
"~/Content/landing/Lp_font-awesome.min.css"
));
            #endregion

            #region js landing page
            bundles.Add(new ScriptBundle("~/bundles/lpjs").Include(
                "~/Scripts/jquery-3.1.1.min.js",                                                   
                "~/vendors/jquery/dist/jquery.min.js",  
                "~/vendors/bootstrap/dist/js/bootstrap.min.js",              
                "~/js/toast/toastr.min.js",
                "~/js/toast/toastrsample.min.js",
                "~/Content/landing/Lp_jquery.min.js",
                "~/Content/landing/Lp_js_bootstrap.min.js"
                   ));
            #endregion

            //========================
            #region css login master
            bundles.Add(new StyleBundle("~/bundles/logincss").Include(
                "~/Styles/colorbox.css",
                "~/vendors/bootstrap/dist/css/bootstrap.min.css",
                "~/vendors/font-awesome/css/font-awesome.min.css",
                "~/vendors/animate.css/animate.min.css",
                "~/build/css/custom.min.css"
));
            #endregion

            #region js login master
            bundles.Add(new ScriptBundle("~/bundles/loginjs").Include(
                "~/Scripts/jquery-3.3.1.min.js",
                "~/Scripts/jquery.colorbox.js",
                "~/Scripts/sgen.js"                                             
                   ));                                   

            #endregion

            //========================
            #region css footable
            bundles.Add(new StyleBundle("~/bundles/foocss").Include(
                "~/js/GridViewScroll-master/css/web.css",
                "~/directlink/footable.min.css"
                ));
            #endregion

            #region js footable
            bundles.Add(new ScriptBundle("~/bundles/foojs").Include(
               "~/js/GridViewScroll-master/js/gridviewscroll.js",
                "~/directlink/footable.js"
                ));

            bundles.Add(new StyleBundle("~/bundles/foocss1").Include(
             "~/adminlite/dist/css/AdminLTE.min.css",
             "~/Content/css/select2/select2.min.css"           
             ));
            #endregion

            #region js footable
            bundles.Add(new ScriptBundle("~/bundles/foojs1").Include(
                "~/adminlite/js/adminlte.min.js",             
                "~/content/css/select2/select2_4.0.6.min.js"
                ));
            #endregion

            #region css PDF Print
            bundles.Add(new StyleBundle("~/bundles/pdfcss").Include(
                "~/Content/pdfjs1030/web/viewer.css"                
                ));
            #endregion
            #region js PDF Print

            bundles.Add(new ScriptBundle("~/bundles/pdfjs").Include(

                "~/Content/pdfjs1030/web/compatibility.js",
                "~/Content/pdfjs1030/web/l10n.js",
                "~/Content/pdfjs1030/build/pdf.js",
                "~/Content/pdfjs1030/web/debugger.js",
                "~/Content/pdfjs1030/web/viewer.js"
                ));
            #endregion

            //========================
            #region css gmap
            bundles.Add(new StyleBundle("~/bundles/gmapcss").Include(
             "~/Scripts/gmap/gmap.css"
             ));
            #endregion

            #region online js gmap
            bundles.UseCdn = true;
            var gmapCdnPath = "http://maps.googleapis.com/maps/api/js?v=3.exp&libraries=geometry,places&key=AIzaSyBccgatgVcUJK-BAqG_8ab0USdEqtkSy1c";            
            bundles.Add(new ScriptBundle("~/bundles/gmapjs", gmapCdnPath).Include(                
                ));
            #endregion

            #region js gmap
            bundles.Add(new ScriptBundle("~/bundles/gmapjs2").Include(                
                "~/Scripts/gmap/gmap.js"
                ));
            #endregion

            //=========================
            #region css asp master     
            bundles.Add(new StyleBundle("~/bundles/aspmascss").Include(
                "~/Content/css/monthly.css",
                "~/vendors/bootstrap/dist/css/bootstrap.min.css",
                "~/Content/css/bootstrap-toggle.min.css",
                "~/Content/css/passwordstrength.css",
                "~/vendors/bootstrap-daterangepicker/daterangepicker.css",
                "~/vendors/bootstrap-datetimepicker/build/css/bootstrap-datetimepicker.css",
                "~/vendors/font-awesome/css/font-awesome.min.css",
                "~/build/css/custom.min.css",
                "~/content/css/gridviewstyle.css",
                "~/vendors/datatables.net-bs/css/dataTables.bootstrap.min.css",
                "~/vendors/datatables.net-buttons-bs/css/buttons.bootstrap.min.css",
                "~/vendors/datatables.net-fixedheader-bs/css/fixedHeader.bootstrap.min.css",
                "~/vendors/datatables.net-responsive-bs/css/responsive.bootstrap.min.css",
                "~/vendors/datatables.net-scroller-bs/css/scroller.bootstrap.min.css",
                "~/Content/colorbox.css",
                "~/Content/css/sgen.css",
                "~/Content/css/select2/select2.min.css",
                "~/Scripts/fastselect/fastselect.min.css",
                "~/Scripts/htmlextStyle/jHtmlArea.css",
                "~/Content/css/bootstrap-datepicker.css",
                "~/student/std_reg.css",
                "~/Scripts/inputmast/css/inputmask.css"
));
            #endregion

            #region jquery asp master
            bundles.Add(new ScriptBundle("~/bundles/aspmjquery").Include(
                 "~/Scripts/jquery-3.1.1.min.js",
                "~/vendors/jquery/dist/jquery.min.js",
                "~/vendors/bootstrap/dist/js/bootstrap.min.js"
                   ));
            #endregion

            #region bootstrap js asp master
            bundles.Add(new ScriptBundle("~/bundles/aspmbootstrap").Include(            
                "~/vendors/bootstrap/dist/js/bootstrap.min.js"
                   ));
            #endregion

            #region js asp master
            bundles.Add(new ScriptBundle("~/bundles/aspmasjsall").Include(

                               "~/js/monthly.js",
             "~/js/bootstrap-toggle.min.js",
             "~/js/jquery.toaster.js",
             "~/vendors/moment/min/moment.min.js",
             "~/vendors/bootstrap-daterangepicker/daterangepicker.js",
             "~/vendors/bootstrap-datetimepicker/build/js/bootstrap-datetimepicker.min.js",
              "~/Scripts/jquery.colorbox-min.js",
                "~/Scripts/htmlext/jHtmlArea-0.8.js",
                "~/js/bootstrap-datepicker.js",
                "~/student/jquery-migrate-1.2.1.min.js",
                "~/Scripts/inputmast/dist/jquery.inputmask.js",
                "~/vendors/fastclick/lib/fastclick.js",
                "~/vendors/datatables.net/js/jquery.dataTables.min.js",
                "~/vendors/datatables.net-bs/js/dataTables.bootstrap.min.js",
                "~/vendors/datatables.net-buttons/js/dataTables.buttons.min.js",
                "~/vendors/datatables.net-buttons-bs/js/buttons.bootstrap.min.js",
                "~/vendors/datatables.net-buttons/js/buttons.flash.min.js",
                "~/vendors/datatables.net-buttons/js/buttons.html5.min.js",
                "~/vendors/datatables.net-buttons/js/buttons.print.min.js",
                "~/vendors/datatables.net-fixedheader/js/dataTables.fixedHeader.min.js",
                "~/vendors/datatables.net-keytable/js/dataTables.keyTable.min.js",
                "~/vendors/datatables.net-responsive/js/dataTables.responsive.min.js",
                "~/vendors/datatables.net-responsive-bs/js/responsive.bootstrap.js",
                "~/vendors/datatables.net-scroller/js/dataTables.scroller.min.js",
                  "~/Scripts/sgen.js",
                 "~/Content/css/select2/select2_4.0.6.min.js",
                 "~/Scripts/fastselect/fastselect.standalone.js"



                ));
            bundles.Add(new ScriptBundle("~/bundles/aspmasjs").Include(

                                  "~/js/monthly.js",
                "~/js/bootstrap-toggle.min.js",
                "~/js/jquery.toaster.js",
                "~/vendors/moment/min/moment.min.js",
                "~/vendors/bootstrap-daterangepicker/daterangepicker.js",
                "~/vendors/bootstrap-datetimepicker/build/js/bootstrap-datetimepicker.min.js",
                "~/Scripts/jquery.colorbox-min.js",
                "~/Scripts/htmlext/jHtmlArea-0.8.js",
                "~/js/bootstrap-datepicker.js",
                "~/student/jquery-migrate-1.2.1.min.js",
                "~/Scripts/inputmast/dist/jquery.inputmask.js",
                "~/vendors/fastclick/lib/fastclick.js",
                "~/vendors/datatables.net/js/jquery.dataTables.min.js",
                "~/vendors/datatables.net-bs/js/dataTables.bootstrap.min.js",
                "~/vendors/datatables.net-buttons/js/dataTables.buttons.min.js",
                "~/vendors/datatables.net-buttons-bs/js/buttons.bootstrap.min.js",
                "~/vendors/datatables.net-buttons/js/buttons.flash.min.js",
                "~/vendors/datatables.net-buttons/js/buttons.html5.min.js",
                "~/vendors/datatables.net-buttons/js/buttons.print.min.js",
                "~/vendors/datatables.net-fixedheader/js/dataTables.fixedHeader.min.js",
                "~/vendors/datatables.net-keytable/js/dataTables.keyTable.min.js",
                "~/vendors/datatables.net-responsive/js/dataTables.responsive.min.js",
                "~/vendors/datatables.net-responsive-bs/js/responsive.bootstrap.js",
                "~/vendors/datatables.net-scroller/js/dataTables.scroller.min.js"
              

                   ));
            bundles.Add(new ScriptBundle("~/bundles/aspmascus").Include(

              "~/build/js/custom.min.js"

               ));
          
            #endregion            

            //==========================
            #region css dashboard
            bundles.Add(new StyleBundle("~/bundles/dashcss").Include(
                "~/Scripts/calender/css/responsive-calendar.min.css"
                ));
            #endregion

            #region js dashboard

            bundles.Add(new ScriptBundle("~/bundles/dashcaljs").Include(
      "~/Scripts/calender/js/responsive-calendar.min.js"
          ));

            bundles.Add(new ScriptBundle("~/bundles/dashjs").Include(
                "~/Scripts/highcharts/highcharts.js",
                "~/Scripts/highcharts/highcharts-3d.js",
                "~/Scripts/highcharts/exporting.js",
                "~/Scripts/highcharts/export-data.js",
                "~/Scripts/highcharts/highcharts-more.js"
                ));

            #endregion

            //====================
            #region css 
            bundles.Add(new StyleBundle("~/bundles/updcss").Include(
              "~/Scripts/fancybox-master/dist/jquery.fancybox.min.css"
                ));
            #endregion

            #region css upd          
            bundles.Add(new ScriptBundle("~/bundles/updjs").Include(
                "~/Scripts/fancybox-master/dist/jquery.fancybox.min.js",
                "~/js/pdf.worker.js",
                "~/Scripts/pdf/pdf.js"
          ));

            #endregion

            //============
            #region skyinfycss 
            bundles.Add(new StyleBundle("~/bundles/skyinfycss").Include(
              "~/Content/skyinfycss.css"
                ));
            #endregion

            #region skyinfyjs
            bundles.Add(new ScriptBundle("~/bundles/skyinfyjs").Include(
                "~/Scripts/skyinfyjs.js"
          ));

            #endregion

            #region ckeditorcss 
            bundles.Add(new StyleBundle("~/bundles/ckeditorcss").Include(
              "~/Scripts/ckeditor/skin/moono-lisa/editor.css",
              "~/Scripts/ckeditor/skin/moono-lisa/dialog.css"
                ));
            #endregion

            #region ckeditorjs
            bundles.Add(new ScriptBundle("~/bundles/ckeditorjs").Include(
                "~/Scripts/ckeditor/ckeditor.js",
                "~/Scripts/ckeditor/styles.js",
                "~/Scripts/ckeditor/config.js",
                "~/Scripts/ckeditor/lang/en.js"
          ));

            #endregion

            #region tabcss
            bundles.Add(new StyleBundle("~/bundles/tabcss").Include(
             "~/student/std_reg.css"
               ));
            #endregion

            BundleTable.EnableOptimizations = true;           
        }
    }
}

