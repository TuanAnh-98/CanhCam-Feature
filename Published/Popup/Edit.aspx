<%@ Page Language="c#" CodeBehind="Edit.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master"
    AutoEventWireup="false" Inherits="CanhCam.Web.PopupUI.PopupEdit" %>

<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:BreadcrumbAdmin ID="breadcrumb" runat="server" 
        CurrentPageTitle="<%$Resources:PopupResources, EditPageTitle %>" CurrentPageUrl="~/Popup/Edit.aspx" />
    <div class="admin-content col-md-12">
        <asp:UpdatePanel ID="upButton" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <portal:HeadingPanel ID="heading" runat="server">
                    <asp:Button SkinID="UpdateButton" ID="btnUpdate" ValidationGroup="popup" Text="<%$Resources:Resource, UpdateButton %>" runat="server" />
                    <asp:Button SkinID="UpdateButton" ID="btnUpdateAndNew" ValidationGroup="popup" Text="<%$Resources:Resource, UpdateAndNewButton %>" runat="server" />
                    <asp:Button SkinID="UpdateButton" ID="btnUpdateAndClose" ValidationGroup="popup" Text="<%$Resources:Resource, UpdateAndCloseButton %>" runat="server" />
                    <asp:Button SkinID="InsertButton" ID="btnInsert" ValidationGroup="popup" Text="<%$Resources:Resource, InsertButton %>" runat="server" />
                    <asp:Button SkinID="InsertButton" ID="btnInsertAndNew" ValidationGroup="popup" Text="<%$Resources:Resource, InsertAndNewButton %>" runat="server" />
                    <asp:Button SkinID="InsertButton" ID="btnInsertAndClose" ValidationGroup="popup" Text="<%$Resources:Resource, InsertAndCloseButton %>" runat="server" />
                    <asp:Button SkinID="DeleteButton" ID="btnDelete" runat="server" Text="<%$Resources:Resource, DeleteButton %>" CausesValidation="false" />
                    <asp:Button SkinID="DeleteButton" ID="btnDeleteLanguage" Visible="false" OnClick="btnDeleteLanguage_Click" Text="<%$Resources:Resource, DeleteLanguageButton %>" runat="server" CausesValidation="false" />
                </portal:HeadingPanel>
                <portal:NotifyMessage ID="message" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="workplace form-horizontal">
            <div class="settingrow form-group">
                <gb:SiteLabel ID="lblZones" runat="server" ConfigKey="ZoneLabel" ResourceFile="Resource"
                    ForControl="ddZones" CssClass="settinglabel control-label col-sm-3" />
                <div class="col-sm-9">
                    <portal:ComboBox ID="cobZones" SelectionMode="Multiple" runat="server" />
                </div>
            </div>
            <div class="settingrow form-group">
                <gb:SiteLabel ID="lblActiveFrom" runat="server" ConfigKey="ActiveFromLabel"
                    ForControl="dpActiveFrom" ResourceFile="PopupResources" CssClass="settinglabel control-label col-sm-3" />
                <div class="col-sm-9">
                    <gb:DatePickerControl ID="dpActiveFrom" runat="server" ShowTime="True" SkinID="news" />
                    <asp:RequiredFieldValidator ID="reqActiveFrom" runat="server" ControlToValidate="dpActiveFrom"
                        Display="Dynamic" SetFocusOnError="true" CssClass="txterror" ValidationGroup="popup" />
                </div>
            </div>
            <div class="settingrow form-group">
                <gb:SiteLabel ID="lblEndDate" runat="server" ConfigKey="ActiveToLabel" ResourceFile="PopupResources"
                    ForControl="dpActiveTo" CssClass="settinglabel control-label col-sm-3" />
                <div class="col-sm-9">
                    <gb:DatePickerControl ID="dpActiveTo" runat="server" ShowTime="True" SkinID="news" />
                </div>
            </div>
            <div class="settingrow form-group">
                <gb:SiteLabel ID="lblCookieExpiryTime" runat="server" ConfigKey="CookieExpiryTimeLabel" ResourceFile="PopupResources"
                    CssClass="settinglabel control-label col-sm-3" />
                <div class="col-sm-9">
                    <asp:TextBox ID="txtCookieExpiryTime" SkinID="NumericTextBox" Text="0" MaxLength="20" runat="server" />
                </div>
            </div>
            <div id="divPosition" runat="server" visible="false" class="settingrow form-group">
                <gb:SiteLabel ID="lblPosition" runat="server" ConfigKey="PositionLabel" ResourceFile="PopupResources"
                    ForControl="chkPosition" CssClass="settinglabel control-label col-sm-3" />
                <div class="col-sm-9">
                    <asp:CheckBox ID="chkPosition" runat="server" />
                </div>
            </div>
            <asp:UpdatePanel ID="up" runat="server">
                <ContentTemplate>
                    <telerik:RadTabStrip ID="tabLanguage" OnTabClick="tabLanguage_TabClick" 
                        EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" 
                        CssClass="subtabs" SkinID="SubTabs" Visible="false" SelectedIndex="0" runat="server" />
                    <div class="settingrow form-group">
                        <gb:SiteLabel ID="lblTitle" runat="server" ForControl="txtTitle" CssClass="settinglabel control-label col-sm-3"
                            ConfigKey="TitleLabel" ResourceFile="PopupResources" />
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtTitle" runat="server" />
                            <asp:RequiredFieldValidator ID="reqTitle" runat="server" ControlToValidate="txtTitle"
                                Display="Dynamic" SetFocusOnError="true" CssClass="txterror" ValidationGroup="popup" />
                        </div>
                    </div>
                    <div class="settingrow form-group">
                        <gb:SiteLabel ID="lblContent" runat="server" ForControl="edContent" CssClass="settinglabel control-label col-sm-3"
                            ConfigKey="ContentLabel" ResourceFile="PopupResources" />
                        <div class="col-sm-9">
                            <gbe:EditorControl ID="edContent" runat="server" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <portal:SessionKeepAliveControl ID="ka1" runat="server" />
    </div>
</asp:Content>
