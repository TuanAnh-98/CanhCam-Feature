<%@ Page ValidateRequest="false" Language="c#" CodeBehind="ManufacturerEdit.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master"
    AutoEventWireup="false" Inherits="CanhCam.Web.ProductUI.ManufacturerEditPage" %>

<%@ Register TagPrefix="Site" Assembly="CanhCam.Features.Product" Namespace="CanhCam.Web.ProductUI" %>

<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <Site:ManufacturerDisplaySettings ID="displaySettings" runat="server" />
    <portal:BreadcrumbAdmin ID="breadcrumb" runat="server"
        ParentTitle="<%$Resources:ProductResources, ManufacturersTitle %>" ParentUrl="~/Product/AdminCP/Manufacturers.aspx"
        CurrentPageTitle="<%$Resources:ProductResources, ManufacturerEditTitle %>" CurrentPageUrl="~/Product/AdminCP/ManufacturerEdit.aspx" />
    <div class="admin-content col-md-12">
        <asp:UpdatePanel ID="upButton" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <portal:HeadingPanel ID="heading" runat="server">
                    <asp:Button SkinID="UpdateButton" ID="btnUpdate" Text="<%$Resources:Resource, UpdateButton %>" ValidationGroup="manufacturer" runat="server" />
                    <asp:Button SkinID="UpdateButton" ID="btnUpdateAndNew" Text="<%$Resources:Resource, UpdateAndNewButton %>" ValidationGroup="manufacturer" runat="server" />
                    <asp:Button SkinID="UpdateButton" ID="btnUpdateAndClose" Text="<%$Resources:Resource, UpdateAndCloseButton %>" ValidationGroup="manufacturer" runat="server" />
                    <asp:Button SkinID="InsertButton" ID="btnInsert" Text="<%$Resources:Resource, InsertButton %>" ValidationGroup="manufacturer" runat="server" />
                    <asp:Button SkinID="InsertButton" ID="btnInsertAndNew" Text="<%$Resources:Resource, InsertAndNewButton %>" ValidationGroup="manufacturer" runat="server" />
                    <asp:Button SkinID="InsertButton" ID="btnInsertAndClose" Text="<%$Resources:Resource, InsertAndCloseButton %>" ValidationGroup="manufacturer" runat="server" />
                    <asp:Button SkinID="DeleteButton" ID="btnDelete" runat="server" Text="Delete this item" CausesValidation="false" />
                    <asp:Button SkinID="DeleteButton" ID="btnDeleteLanguage" Visible="false" OnClick="btnDeleteLanguage_Click" Text="<%$Resources:Resource, DeleteLanguageButton %>" runat="server" CausesValidation="false" />
                </portal:HeadingPanel>
                <portal:NotifyMessage ID="message" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="workplace">
            <div id="divtabs" class="tabs">
                <ul id="ulTabs" runat="server" class="nav nav-tabs">
                    <li role="presentation" class="active"><a aria-controls="tabContent" role="tab" data-toggle="tab" href="#tabContent">
                        <asp:Literal ID="litContentTab" runat="server" /></a></li>
                    <li role="presentation">
                        <asp:Literal ID="litMetaTab" runat="server" /></li>
                </ul>
                <div class="tab-content">
                    <div role="tabpanel" class="tab-pane fade active in" id="tabContent">
                        <div class="form-horizontal">
                            <div id="divPages" runat="server" class="settingrow form-group">
                                <gb:SiteLabel ID="lblPages" runat="server" ForControl="ddPages" CssClass="settinglabel control-label col-sm-3"
                                    ConfigKey="ZoneSettingsPageLabel" />
                                <div class="col-sm-9">
                                    <asp:DropDownList ID="ddPages" runat="server" DataTextField="PageName" DataValueField="PageID" />
                                </div>
                            </div>
                            <div id="divZones" runat="server" class="settingrow form-group">
                                <gb:SiteLabel ID="lblZones" runat="server" ConfigKey="ZoneLabel" ResourceFile="Resource"
                                    ForControl="cobZones" CssClass="settinglabel control-label col-sm-3" />
                                <div class="col-sm-9">
                                    <portal:ComboBox ID="cobZones" SelectionMode="Multiple" runat="server" />
                                </div>
                            </div>
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
                            <div id="divShowOption" visible="false" runat="server" class="settingrow form-group">
                                <gb:SiteLabel ID="lblShowOption" runat="server" ForControl="chlShowOption" ConfigKey="ShowOptionLabel"
                                    ResourceFile="ProductResources" CssClass="settinglabel control-label col-sm-3" />
                                <div class="col-sm-9">
                                    <asp:CheckBoxList ID="chlShowOption" SkinID="Enum" runat="server" />
                                </div>
                            </div>
                            <div id="divIsPublished" class="settingrow form-group" runat="server">
                                <gb:SiteLabel ID="lblIsPublished" runat="server" ForControl="chkIsPublished" ConfigKey="ManufacturerIsPublishedLabel"
                                    ResourceFile="ProductResources" CssClass="settinglabel control-label col-sm-3" />
                                <div class="col-sm-9">
                                    <asp:CheckBox ID="chkIsPublished" runat="server" Checked="true" />
                                </div>
                            </div>
                            <asp:UpdatePanel ID="up" runat="server">
                                <ContentTemplate>
                                    <telerik:RadTabStrip ID="tabLanguage" OnTabClick="tabLanguage_TabClick"
                                        EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false"
                                        CssClass="subtabs" SkinID="SubTabs" Visible="false" SelectedIndex="0" runat="server" />
                                    <div class="settingrow form-group">
                                        <gb:SiteLabel ID="lblName" runat="server" ForControl="txtName" CssClass="settinglabel control-label col-sm-3"
                                            ConfigKey="ManufacturerNameLabel" ResourceFile="ProductResources" />
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="txtName" runat="server" MaxLength="255" CssClass="forminput verywidetextbox" />
                                            <asp:RequiredFieldValidator ID="reqName" runat="server" ControlToValidate="txtName"
                                                Display="Dynamic" SetFocusOnError="true" CssClass="txterror" ValidationGroup="manufacturer" />
                                        </div>
                                    </div>
                                    <div id="divUrl" runat="server" class="settingrow form-group">
                                        <gb:SiteLabel ID="lblUrl" runat="server" ForControl="txtUrl" CssClass="settinglabel control-label col-sm-3"
                                            ConfigKey="ManufacturerUrlLabel" ResourceFile="ProductResources" />
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="txtUrl" runat="server" MaxLength="255" CssClass="forminput verywidetextbox" />
                                            <span id="spnUrlWarning" runat="server" style="font-weight: normal; display: none;" class="txterror"></span>
                                            <asp:HiddenField ID="hdnTitle" runat="server" />
                                            <asp:RegularExpressionValidator ID="regexUrl" runat="server" ControlToValidate="txtUrl"
                                                ValidationExpression="((http\://|https\://|~/){1}(\S+){0,1})" Display="Dynamic" SetFocusOnError="true"
                                                ValidationGroup="manufacturer" />
                                        </div>
                                    </div>
                                    <div id="divDescription" runat="server" class="settingrow form-group">
                                        <gb:SiteLabel ID="lblDescription" runat="server" CssClass="settinglabel control-label col-sm-3" ConfigKey="ManufacturerDescriptionLabel"
                                            ResourceFile="ProductResources" />
                                        <div class="col-sm-9">
                                            <gbe:EditorControl ID="edDescription" runat="server" />
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div role="tabpanel" class="tab-pane fade" id="tabMeta" runat="server">
                        <asp:UpdatePanel ID="upSEO" runat="server">
                            <ContentTemplate>
                                <telerik:RadTabStrip ID="tabLanguageSEO" OnTabClick="tabLanguageSEO_TabClick"
                                    EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false"
                                    CssClass="subtabs" SkinID="SubTabs" Visible="false" SelectedIndex="0" runat="server" />
                                <div class="form-horizontal">
                                    <div class="settingrow form-group">
                                        <gb:SiteLabel ID="lblMetaTitle" runat="server" ForControl="txtMetaTitle" CssClass="settinglabel control-label col-sm-3"
                                            ConfigKey="MetaTitleLabel" />
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="txtMetaTitle" runat="server" MaxLength="65" />
                                        </div>
                                    </div>
                                    <div class="settingrow form-group">
                                        <gb:SiteLabel ID="lblMetaDescription" runat="server" ForControl="txtMetaDescription" CssClass="settinglabel control-label col-sm-3"
                                            ConfigKey="MetaDescriptionLabel" ResourceFile="ProductResources" />
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="txtMetaDescription" runat="server" MaxLength="156" />
                                        </div>
                                    </div>
                                    <div class="settingrow form-group">
                                        <gb:SiteLabel ID="lblMetaKeywords" runat="server" ForControl="txtMetaKeywords" CssClass="settinglabel control-label col-sm-3"
                                            ConfigKey="MetaKeywordsLabel" ResourceFile="ProductResources" />
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="txtMetaKeywords" runat="server" MaxLength="156" />
                                        </div>
                                    </div>
                                    <div id="divAdditionalMetaTags" runat="server" visible="false" class="settingrow form-group">
                                        <gb:SiteLabel ID="lblAdditionalMetaTags" ForControl="txtAdditionalMetaTags" CssClass="settinglabel control-label col-sm-3" runat="server" ConfigKey="MetaAdditionalLabel" />
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="txtAdditionalMetaTags" CssClass="form-control" TextMode="MultiLine" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
        <portal:SessionKeepAliveControl ID="ka1" runat="server" />
    </div>
</asp:Content>