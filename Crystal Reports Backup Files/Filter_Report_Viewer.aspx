<%@ Page Language="C#" AutoEventWireup="true" Inherits="erp_schoolReports_Rpts_Filter_Report_Viewer2" Codebehind="Filter_Report_Viewer.aspx.cs" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../aspnet_client/system_web/4_0_30319/crystalreportviewers13/js/crviewer/crv.js" type="text/javascript"></script>
	<script language="javascript" src="../aspnet_client/system_web/4_0_30319/crystalreportviewers13/js/crviewer/../../allStrings_en.js"></script>
<script language="javascript" src="../aspnet_client/system_web/4_0_30319/crystalreportviewers13/js/crviewer/../../allInOne.js"></script>
<link rel="stylesheet" type="text/css" href="~/erp/aspnet_client/system_web/4_0_30319/crystalreportviewers13/js/crviewer/../dhtmllib/images/skin_standard/style.css">
</head>
<body>
    <form id="form1" runat="server" style="background-color:transparent">
        <asp:HiddenField ID="hfmyguid" runat="server" />
        <div style="margin: 10px; margin-top: 35px">
        <div align="center">
             <CR:CrystalReportViewer ID="CrystalReportViewer1" HasToggleGroupTreeButton="false" runat="server" AutoDataBind="true" />
        </div>
            </div>
    </form>
</body>
</html>
