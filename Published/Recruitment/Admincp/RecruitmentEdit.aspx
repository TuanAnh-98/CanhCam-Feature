<%@ Page ValidateRequest="false" Language="c#" CodeBehind="RecruitmentEdit.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master"
    AutoEventWireup="false" Inherits="CanhCam.Web.RecruitmentUI.RecruitmentEditPage" %>

<%@ Register TagPrefix="Site" TagName="AdminRecruitmentEdit" Src="~/Recruitment/Controls/AdminRecruitmentEditControl.ascx" %>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <Site:AdminRecruitmentEdit ID="adminRecruitmentEdit" runat="server" 
        RecruitmentType="0" EditPageUrl="/Recruitment/AdminCP/RecruitmentEdit.aspx" ListPageUrl="/Recruitment/AdminCP/RecruitmentList.aspx"
        />
</asp:Content>