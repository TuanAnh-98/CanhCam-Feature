<%@ Page Language="c#" CodeBehind="ProductTypeEdit.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master"
    AutoEventWireup="false" Inherits="CanhCam.Web.ProductUI.ProductTypeEditPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:BreadcrumbAdmin ID="breadcrumb" runat="server"
        ParentTitle="<%$Resources:ProductResources, ProductTypeListLabel %>" ParentUrl="~/Product/AdminCP/ProductTypeList.aspx"
        CurrentPageTitle="<%$Resources:ProductResources, ProductTypeEditLabel %>" CurrentPageUrl="~/Product/AdminCP/ProductTypeEdit.aspx" />
    <div class="admin-content col-md-12">
        <asp:UpdatePanel ID="upButton" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <portal:HeadingPanel ID="heading" runat="server">
                    <asp:Button SkinID="UpdateButton" ID="btnUpdate" ValidationGroup="CustomFields" Text="<%$Resources:Resource, UpdateButton %>" runat="server" />
                    <asp:Button SkinID="UpdateButton" ID="btnUpdateAndNew" ValidationGroup="CustomFields" Text="<%$Resources:Resource, UpdateAndNewButton %>" runat="server" />
                    <asp:Button SkinID="UpdateButton" ID="btnUpdateAndClose" ValidationGroup="CustomFields" Text="<%$Resources:Resource, UpdateAndCloseButton %>" runat="server" />
                    <asp:Button SkinID="InsertButton" ID="btnInsert" ValidationGroup="CustomFields" Text="<%$Resources:Resource, InsertButton %>" runat="server" />
                    <asp:Button SkinID="InsertButton" ID="btnInsertAndNew" ValidationGroup="CustomFields" Text="<%$Resources:Resource, InsertAndNewButton %>" runat="server" />
                    <asp:Button SkinID="InsertButton" ID="btnInsertAndClose" ValidationGroup="CustomFields" Text="<%$Resources:Resource, InsertAndCloseButton %>" runat="server" />
                    <asp:Button SkinID="DeleteButton" ID="btnDelete" runat="server" Text="<%$Resources:Resource, DeleteButton %>" CausesValidation="false" />
                </portal:HeadingPanel>
                <portal:NotifyMessage ID="message" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="workplace">
            <asp:UpdatePanel ID="up" runat="server">
                <ContentTemplate>
                    <telerik:RadTabStrip ID="tabLanguage" OnTabClick="tabLanguage_TabClick"
                        EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false"
                        CssClass="subtabs" SkinID="SubTabs" Visible="false" SelectedIndex="0" runat="server" />
                    <div class="form-horizontal">
                        <div class="settingrow form-group">
                            <gb:SiteLabel ID="lblZones" runat="server" ConfigKey="ProductTypeNameLabel" ResourceFile="ProductResources"
                                ForControl="cobZones" CssClass="settinglabel control-label col-sm-3" />
                            <div class="col-sm-9">
                                <asp:TextBox runat="server" ID="txtName" />
                            </div>
                        </div>

                        <div class="settingrow form-group">
                            <gb:SiteLabel ID="SiteLabel1" runat="server" ConfigKey="ProductTypeDescriptionLabel" ResourceFile="ProductResources"
                                ForControl="cobZones" CssClass="settinglabel control-label col-sm-3" />
                            <div class="col-sm-9">
                                <gbe:EditorControl ID="edDescription" runat="server" />
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="form-horizontal">
                <div id="divPrimaryImage" runat="server" class="settingrow form-group">
                    <gb:SiteLabel ID="lblPrimaryImage" runat="server" ForControl="txtPrimaryImage" CssClass="settinglabel control-label col-sm-3"
                        ConfigKey="ManufacturerPrimaryImageLabel" ResourceFile="ProductResources" />
                    <div class="col-sm-9">
                        <asp:Image ID="imgPrimaryImage" Style="max-width: 100px; display: block;" Visible="false" runat="server" AlternateText="" />
                        <div class="input-group">
                            <asp:TextBox ID="txtPrimaryImage" MaxLength="255" runat="server" />
                            <div class="input-group-addon">
                                <portal:FileBrowserTextBoxExtender ID="PrimaryImageFileBrowser" runat="server" BrowserType="image" />
                            </div>
                        </div>
                    </div>
                </div>
                <div id="divSecondImage" visible="false" runat="server" class="settingrow form-group">
                    <gb:SiteLabel ID="lblSecondImage" runat="server" ForControl="txtSecondImage" CssClass="settinglabel control-label col-sm-3"
                        ConfigKey="ManufacturerSecondImageLabel" ResourceFile="ProductResources" />
                    <div class="col-sm-9">
                        <asp:Image ID="imgSecondImage" Style="max-width: 100px; display: block;" Visible="false" runat="server" AlternateText="" />
                        <div class="input-group">
                            <asp:TextBox ID="txtSecondImage" MaxLength="255" runat="server" />
                            <div class="input-group-addon">
                                <portal:FileBrowserTextBoxExtender ID="SecondImageFileBrowser" runat="server" BrowserType="image" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <portal:SessionKeepAliveControl ID="ka1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
