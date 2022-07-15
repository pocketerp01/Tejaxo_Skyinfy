<%@ Page Language="C#" AutoEventWireup="true" Inherits="erp_schoolReports_Rpts_Filter_Report_Viewer1" Codebehind="Filter_Report_Viewer - Copy.aspx.cs" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server" style="background-color:transparent">
        <div style="margin: 10px; margin-top: 35px">
        <div align="center">
             <CR:CrystalReportViewer ID="CrystalReportViewer1" HasToggleGroupTreeButton="false" runat="server" AutoDataBind="true" />
        </div>
            </div>
    </form>
</body>
</html>
