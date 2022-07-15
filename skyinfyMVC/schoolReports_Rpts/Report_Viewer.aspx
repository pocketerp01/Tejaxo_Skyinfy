<%@ Page Title="" Language="C#" MasterPageFile="~/erp/MasterPage.master" AutoEventWireup="true" Inherits="erp_Reports_Report_Viewer" Codebehind="Report_Viewer.aspx.cs" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <CR:CrystalReportViewer ID="CrystalReportViewer1" HasToggleGroupTreeButton="false" runat="server" AutoDataBind="true" />

</asp:Content>

