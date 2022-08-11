<%@ Page Language="c#" CodeBehind="RecruitmentDepartmentEdit.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master"
    AutoEventWireup="false" Inherits="CanhCam.Web.RecruitmentUI.RecruitmentDepartmentEditPage" %>

<%@ Register TagPrefix="Site" Assembly="CanhCam.Features.Recruitment" Namespace="CanhCam.Web.RecruitmentUI" %>
<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <site:recruitmentdisplaysettings id="displaySettings" runat="server" />
    <portal:breadcrumbadmin id="breadcrumb" runat="server"
        currentpagetitle="<%$Resources:RecruitmentResources, RecruitmentDepartmentEditPage %>" currentpageurl="~/Recruitment/Admincp/RecruitmentDepartmentEdit.aspx" />
    <div class="admin-content col-md-12">
        <asp:UpdatePanel ID="upButton" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <portal:headingpanel id="heading" runat="server">
                    <asp:Button SkinID="UpdateButton" ID="btnUpdate" Text="<%$Resources:Resource, UpdateButton %>" runat="server" />
                    <asp:Button SkinID="UpdateButton" ID="btnUpdateAndNew" Text="<%$Resources:Resource, UpdateAndNewButton %>" runat="server" />
                    <asp:Button SkinID="UpdateButton" ID="btnUpdateAndClose" Text="<%$Resources:Resource, UpdateAndCloseButton %>" runat="server" />
                    <asp:Button SkinID="InsertButton" ID="btnInsert" Text="<%$Resources:Resource, InsertButton %>" runat="server" />
                    <asp:Button SkinID="InsertButton" ID="btnInsertAndNew" Text="<%$Resources:Resource, InsertAndNewButton %>" runat="server" />
                    <asp:Button SkinID="InsertButton" ID="btnInsertAndClose" Text="<%$Resources:Resource, InsertAndCloseButton %>" runat="server" />
                    <asp:Button SkinID="DeleteButton" ID="btnDelete" runat="server" Text="<%$Resources:Resource, DeleteButton %>" CausesValidation="false" />
                    <asp:Button SkinID="DeleteButton" ID="btnDeleteLanguage" Visible="false" OnClick="btnDeleteLanguage_Click" Text="<%$Resources:Resource, DeleteLanguageButton %>" runat="server" CausesValidation="false" />
                </portal:headingpanel>
                <portal:notifymessage id="message" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="workplace form-horizontal">
            <asp:UpdatePanel ID="up" runat="server">
                <ContentTemplate>
                    <telerik:radtabstrip id="tabLanguage" ontabclick="tabLanguage_TabClick"
                        enableembeddedskins="false" enableembeddedbasestylesheet="false"
                        cssclass="subtabs" skinid="SubTabs" visible="false" selectedindex="0" runat="server" />
                    <div class="settingrow form-group">
                        <gb:sitelabel runat="server" forcontrol="txtName" cssclass="settinglabel control-label col-sm-3"
                            configkey="DepartmentNameLabel" resourcefile="RecruitmentResources" />
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtName" runat="server" MaxLength="255" />
                        </div>
                    </div>
                    <div class="settingrow form-group">
                        <gb:sitelabel id="lblDescription" runat="server" forcontrol="txtDescription" cssclass="settinglabel control-label col-sm-3"
                            configkey="DescriptionLabel" resourcefile="RecruitmentResources" />
                        <div class="col-sm-9">
                            <div class="input-group">
                                <gbe:editorcontrol id="edDescription" runat="server" />
                                <portal:gbhelplink id="GbHelpLink3" runat="server" helpkey="banneredit-description-help" />
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <portal:sessionkeepalivecontrol id="ka1" runat="server" />
    </div>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />