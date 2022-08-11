<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="RecruitmentList.aspx.cs" Inherits="CanhCam.Web.RecruitmentUI.RecruitmentListPage" %>

<%@ Register TagPrefix="Site" TagName="AdminRecruitments" Src="~/Recruitment/Controls/AdminRecruitmentControl.ascx" %>
<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <Site:AdminRecruitments ID="adminRecruitments" runat="server"
        PageTitle="<%$Resources:RecruitmentResources, RecruitmentListPage %>" PageUrl="/Recruitment/AdminCP/RecruitmentList.aspx" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />