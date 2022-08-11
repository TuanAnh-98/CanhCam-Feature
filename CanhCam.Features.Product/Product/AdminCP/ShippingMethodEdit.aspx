<%@ Page Language="c#" CodeBehind="ShippingMethodEdit.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master"
    AutoEventWireup="false" Inherits="CanhCam.Web.ProductUI.ShippingMethodEditPage" %>

<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:BreadcrumbAdmin ID="breadcrumb" runat="server"
        ParentTitle="<%$Resources:ProductResources, ShippingMethodsTitle %>" ParentUrl="~/Product/AdminCP/ShippingMethods.aspx"
        CurrentPageTitle="<%$Resources:ProductResources, ShippingMethodEditTitle %>" CurrentPageUrl="~/Product/AdminCP/ShippingMethodEdit.aspx" />
    <div class="admin-content col-md-12">
        <asp:UpdatePanel ID="upButton" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <portal:HeadingPanel ID="heading" runat="server">
                    <asp:Button SkinID="UpdateButton" ID="btnUpdate" ValidationGroup="ShippingMethodEdit" Text="<%$Resources:Resource, UpdateButton %>" runat="server" />
                    <asp:Button SkinID="UpdateButton" ID="btnUpdateAndNew" ValidationGroup="ShippingMethodEdit" Text="<%$Resources:Resource, UpdateAndNewButton %>" runat="server" />
                    <asp:Button SkinID="UpdateButton" ID="btnUpdateAndClose" ValidationGroup="ShippingMethodEdit" Text="<%$Resources:Resource, UpdateAndCloseButton %>" runat="server" />
                    <asp:Button SkinID="InsertButton" ID="btnInsert" ValidationGroup="ShippingMethodEdit" Text="<%$Resources:Resource, InsertButton %>" runat="server" />
                    <asp:Button SkinID="InsertButton" ID="btnInsertAndNew" ValidationGroup="ShippingMethodEdit" Text="<%$Resources:Resource, InsertAndNewButton %>" runat="server" />
                    <asp:Button SkinID="InsertButton" ID="btnInsertAndClose" ValidationGroup="ShippingMethodEdit" Text="<%$Resources:Resource, InsertAndCloseButton %>" runat="server" />
                    <asp:Button SkinID="DeleteButton" ID="btnDelete" runat="server" Text="<%$Resources:Resource, DeleteButton %>" CausesValidation="false" />
                    <asp:Button SkinID="DeleteButton" ID="btnDeleteLanguage" Visible="false" OnClick="btnDeleteLanguage_Click" Text="<%$Resources:Resource, DeleteLanguageButton %>" runat="server" CausesValidation="false" />
                </portal:HeadingPanel>
                <portal:NotifyMessage ID="message" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="workplace">
            <div class="form-horizontal">
                <div class="settingrow form-group">
                    <gb:SiteLabel ID="lblShippingProvider" runat="server" ForControl="ddlShippingProvider" CssClass="settinglabel control-label col-sm-3" ConfigKey="ShippingMethodProviderLabel" ResourceFile="ProductResources" />
                    <div class="col-sm-9">
                        <asp:DropDownList ID="ddlShippingProvider" runat="server" AutoPostBack="true" />
                    </div>
                </div>
                <div class="settingrow form-group">
                    <gb:SiteLabel ID="lblIsActive" runat="server" ForControl="chkIsActive" CssClass="settinglabel control-label col-sm-3" ConfigKey="ShippingMethodIsActiveLabel" ResourceFile="ProductResources" />
                    <div class="col-sm-9">
                        <asp:CheckBox ID="chkIsActive" Checked="true" runat="server" />
                    </div>
                </div>
                <asp:UpdatePanel ID="upFreeShipping" runat="server">
                    <ContentTemplate>
                        <div id="divFreeShippingOverXEnabled" runat="server" class="settingrow form-group">
                            <gb:SiteLabel ID="lblFreeShippingOverXEnabled" runat="server" ForControl="chkFreeShippingOverXEnabled" CssClass="settinglabel control-label col-sm-3" ConfigKey="ShippingMethodFreeShippingOverXLabel" ResourceFile="ProductResources" />
                            <div class="col-sm-9">
                                <asp:CheckBox ID="chkFreeShippingOverXEnabled" AutoPostBack="true" runat="server" />
                            </div>
                        </div>
                        <div id="divFreeShippingOverXValue" runat="server" class="settingrow form-group">
                            <gb:SiteLabel ID="lblFreeShippingOverXValue" runat="server" ForControl="txtFreeShippingOverXValue" CssClass="settinglabel control-label col-sm-3" ConfigKey="ShippingMethodFreeShippingOverXValueLabel" ResourceFile="ProductResources" />
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtFreeShippingOverXValue" runat="server" MaxLength="20" SkinID="PriceTextBox" />
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="up" runat="server">
                    <ContentTemplate>
                        <telerik:RadTabStrip ID="tabLanguage" OnTabClick="tabLanguage_TabClick"
                            EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false"
                            CssClass="subtabs" SkinID="SubTabs" Visible="false" SelectedIndex="0" runat="server" />
                        <div class="settingrow form-group">
                            <gb:SiteLabel ID="lblName" runat="server" ForControl="txtName" CssClass="settinglabel control-label col-sm-3" ShowRequired="true" ConfigKey="ShippingMethodNameLabel" ResourceFile="ProductResources" />
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtName" MaxLength="255" runat="server" />
                                <asp:RequiredFieldValidator ID="reqName" runat="server" ControlToValidate="txtName"
                                    ErrorMessage="<%$Resources:ProductResources, ShippingMethodNameRequiredWarning %>"
                                    Display="Dynamic" SetFocusOnError="true" CssClass="txterror" ValidationGroup="ShippingMethodEdit" />
                            </div>
                        </div>
                        <div class="settingrow form-group">
                            <gb:SiteLabel ID="lblDescription" runat="server" ForControl="edDescription" CssClass="settinglabel control-label col-sm-3" ConfigKey="ShippingMethodDescriptionLabel" ResourceFile="ProductResources" />
                            <div class="col-sm-9">
                                <gbe:EditorControl ID="edDescription" runat="server" />
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <h3>
                <asp:Literal ID="litShippingMethod" runat="server" />
            </h3>
            <div id="divShippingTableRate" runat="server" class="form-horizontal">
                <div class="settingrow form-group">
                    <gb:SiteLabel ID="lblShippingFee" runat="server" ForControl="txtShippingFee" CssClass="settinglabel control-label col-sm-3" />
                    <div id="divShippingFee" runat="server" class="col-sm-9">
                        <asp:TextBox ID="txtShippingFee" runat="server" MaxLength="20" SkinID="PriceTextBox" />
                    </div>
                </div>
                <div id="divImportExport" runat="server">
                    <div class="settingrow form-group">
                        <gb:SiteLabel ID="lblFileUpload" runat="server" ForControl="fileUpload" CssClass="settinglabel control-label col-sm-3" ConfigKey="ShippingFeeImportLabel" ResourceFile="ProductResources" />
                        <div class="col-sm-9">
                            <telerik:RadAsyncUpload ID="fileUpload" SkinID="radAsyncUploadSkin" MultipleFileSelection="Disabled" AllowedFileExtensions="xls" runat="server" />
                        </div>
                    </div>
                    <div class="settingrow form-group">
                        <div class="col-sm-9 col-sm-offset-3">
                            <asp:Button ID="btnExportTemplate" CssClass="btn btn-default btn-export" Text="<%$Resources:ProductResources, ShippingFeeExportTemplate %>" runat="server" />
                            <asp:CheckBox ID="chkExportDistrict" Text="<%$Resources:ProductResources, ShippingFeeExportDistrict %>" runat="server" />
                        </div>
                    </div>
                    <div id="divExportData" runat="server" visible="false" class="settingrow form-group">
                        <div class="col-sm-9 col-sm-offset-3">
                            <asp:Button ID="btnExportData" CssClass="btn btn-default btn-export" Text="<%$Resources:ProductResources, ShippingFeeExportData %>" runat="server" />
                        </div>
                    </div>
                    <telerik:RadGrid ID="grid" SkinID="radGridSkin" runat="server">
                        <MasterTableView DataKeyNames="ShippingTableRateId">
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="<%$Resources:ProductResources, ShippingFeeGeoZoneName %>" UniqueName="GeoZoneName">
                                    <ItemTemplate>
                                        <%# Eval("GeoZoneName")%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="<%$Resources:ProductResources, ShippingFeeByOrderTotal %>" UniqueName="FromValue">
                                    <ItemTemplate>
                                        <%# CanhCam.Web.ProductUI.ProductHelper.FormatPrice(Convert.ToDecimal(Eval("FromValue")), true) %><%# GetAdditional(Convert.ToDecimal(Eval("AdditionalValue")))%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>       
                                <telerik:GridTemplateColumn HeaderText="<%$Resources:ProductResources, ShippingFeeByOrderWeight %>" UniqueName="FromValueOrderWeight" Visible="false">
                                    <ItemTemplate>
                                        <%# CanhCam.Web.ProductUI.ProductHelper.FormatPrice(Convert.ToDecimal(Eval("FromValue"))) %> g
                                        <%# GetAdditional(Convert.ToDecimal(Eval("AdditionalValue")))%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="<%$Resources:ProductResources, ShippingFeeLabel %>">
                                    <ItemTemplate>
                                        <%# CanhCam.Web.ProductUI.ProductHelper.FormatPrice(Convert.ToDecimal(Eval("ShippingFee")), true) %><%# GetAdditional(Convert.ToDecimal(Eval("AdditionalFee")))%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn Visible="false" HeaderText="<%$Resources:ProductResources, ShippingMethodFreeShippingOverX %>" UniqueName="FreeShippingOverXValue">
                                    <ItemTemplate>
                                        <%# CanhCam.Web.ProductUI.ProductHelper.FormatPrice(Convert.ToDecimal(Eval("FreeShippingOverXValue")), true)%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn  HeaderText="Cửa hàng" UniqueName="StoreCode">
                                    <ItemTemplate>
                                        <%# GetStoreName(Convert.ToInt32(Eval("StoreId"))) %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
            </div>
        </div>
        <portal:SessionKeepAliveControl ID="ka1" runat="server" />
    </div>
</asp:Content>