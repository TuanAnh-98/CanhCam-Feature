<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="AdminProductEditControl.ascx.cs" Inherits="CanhCam.Web.ProductUI.AdminProductEditControl" %>

<%@ Register TagPrefix="Site" Assembly="CanhCam.Features.Product" Namespace="CanhCam.Web.ProductUI" %>
<%@ Register TagPrefix="Site" TagName="ProductRelated" Src="~/Product/Controls/ProductRelatedControl.ascx" %>
<%@ Register TagPrefix="Site" TagName="ProductGroup" Src="~/Product/Controls/ProductGroupControl.ascx" %>
<%@ Register TagPrefix="Site" TagName="ProductChild" Src="~/Product/Controls/AdminProductChildControl.ascx" %>
<%@ Register TagPrefix="Site" TagName="NewsRelated" Src="~/Product/Controls/NewsRelatedControl.ascx" %>

<Site:ProductDisplaySettings ID="displaySettings" runat="server" />
<portal:BreadcrumbAdmin ID="breadcrumb" runat="server"
    CurrentPageTitle="<%$Resources:ProductResources, ProductAddNewTitle %>" CurrentPageUrl="~/Product/AdminCP/ProductEdit.aspx" />
<div class="admin-content col-md-12">
    <asp:UpdatePanel ID="upButton" UpdateMode="Conditional" runat="server"> 
        <ContentTemplate>
            <portal:HeadingPanel ID="heading" runat="server">
                <asp:Button SkinID="UpdateButton" ID="btnUpdate" Text="<%$Resources:Resource, UpdateButton %>" ValidationGroup="product" runat="server" />
                <asp:Button SkinID="UpdateButton" ID="btnUpdateAndNew" Text="<%$Resources:Resource, UpdateAndNewButton %>" ValidationGroup="product" runat="server" />
                <asp:Button SkinID="UpdateButton" ID="btnUpdateAndClose" Text="<%$Resources:Resource, UpdateAndCloseButton %>" ValidationGroup="product" runat="server" />
                <asp:Button SkinID="InsertButton" ID="btnInsert" Text="<%$Resources:Resource, InsertButton %>" ValidationGroup="product" runat="server" />
                <asp:Button SkinID="InsertButton" ID="btnInsertAndNew" Text="<%$Resources:Resource, InsertAndNewButton %>" ValidationGroup="product" runat="server" />
                <asp:Button SkinID="InsertButton" ID="btnInsertAndClose" Text="<%$Resources:Resource, InsertAndCloseButton %>" ValidationGroup="product" runat="server" />
                <asp:Button SkinID="DefaultButton" ID="btnCopyModal" Visible="false" data-toggle="modal" data-target="#pnlModal" Text="Copy" runat="server" CausesValidation="false" />
                <asp:HyperLink SkinID="DefaultButton" ID="lnkPreview" Visible="false" runat="server" />
                <asp:Button SkinID="DeleteButton" ID="btnDelete" runat="server" Text="Delete this item" CausesValidation="false" />
                <asp:Button SkinID="DeleteButton" ID="btnDeleteLanguage" Visible="false" OnClick="btnDeleteLanguage_Click" Text="<%$Resources:Resource, DeleteLanguageButton %>" runat="server" CausesValidation="false" />
            </portal:HeadingPanel>
            <portal:NotifyMessage ID="message" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="workplace">
        <asp:Panel ID="pnlNews" runat="server" DefaultButton="btnUpdate">
            <div id="divtabs" class="tabs">
                <ul class="nav nav-tabs">
                    <li role="presentation">
                        <a aria-controls="tabContent" class="active" role="tab" data-toggle="tab" href="#tabContent">
                            <asp:Literal ID="litContentTab" runat="server" />
                        </a>
                    </li>
                    <li role="presentation" id="liImages" runat="server">
                        <asp:Literal ID="litImagesTab" runat="server" />
                    </li>
                    <li role="presentation" id="liAttribute" runat="server">
                        <asp:Literal ID="litAttributeTab" runat="server" />
                    </li>
                    <li role="presentation" id="liCustomField" visible="false" runat="server">
                        <asp:Literal ID="litCustomFieldTab" runat="server" />
                    </li>
                    <li role="presentation" id="liRelatedNews" runat="server">
                        <asp:Literal ID="litRelatedNewsTab" runat="server" />
                    </li>
                    <li role="presentation" id="liRelatedProduct" runat="server">
                        <asp:Literal ID="litRelatedProductTab" runat="server" />
                    </li>
                    <li role="presentation" id="liGroupProduct" runat="server">
                        <asp:Literal ID="litGroupProduct" runat="server" />
                    </li>
                    <li role="presentation">
                        <a aria-controls="tabMeta" role="tab" data-toggle="tab" href="#tabMeta">
                        <asp:Literal ID="litMetaTab" runat="server" /></a>
                    </li>
                    <li role="presentation" id="liChildProduct" runat="server">
                        <asp:Literal ID="litChildProductTab" runat="server" />
                    </li>
                </ul>
                <div class="tab-content">
                    <div role="tabpanel" class="tab-pane fade active in" id="tabContent">
                        <div class="form-horizontal">
                            <div class="settingrow form-group">
                                <gb:SiteLabel ID="lblZones" runat="server" ConfigKey="ZoneLabel"
                                    ForControl="ddZones" CssClass="settinglabel control-label col-sm-3" />
                                <div class="col-sm-9">
                                    <div class="input-group">
                                        <asp:DropDownList ID="ddZones" runat="server" />
                                        <portal:gbHelpLink ID="GbHelpLink9" runat="server" HelpKey="productedit-selectzone-help" />
                                    </div>
                                </div>
                            </div>
                            <div class="settingrow form-group" runat="server" id="divOtherZone" visible="false">
                                <gb:SiteLabel ID="lblZoneList" runat="server" ConfigKey="ZoneListLabel"
                                    ResourceFile="ProductResources" ForControl="cobZoneList"
                                    CssClass="settinglabel control-label col-sm-3" />
                                <div class="col-sm-9">
                                    <portal:ComboBox ID="cobZoneList" SelectionMode="Multiple" runat="server" />
                                </div>
                            </div>
                            <div id="divManufacturers" runat="server" class="settingrow form-group">
                                <gb:SiteLabel ID="lblManufacturers" runat="server" ConfigKey="ManufacturerLabel" ResourceFile="ProductResources"
                                    ForControl="ddManufacturers" CssClass="settinglabel control-label col-sm-3" />
                                <div class="col-sm-9">
                                    <asp:DropDownList ID="ddManufacturers" AppendDataBoundItems="true" DataTextField="Name" DataValueField="ManufacturerId" runat="server">
                                        <asp:ListItem Value="-1" Text="<%$Resources:ProductResources, ManufacturerSelectLabel %>"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="settingrow form-group">
                                <gb:SiteLabel ID="SiteLabel2" runat="server" ConfigKey="ProductTypeLabel" ResourceFile="ProductResources"
                                    ForControl="ddZones" CssClass="settinglabel control-label col-sm-3" />
                                <div class="col-sm-9">
                                    <div class="input-group">
                                        <asp:DropDownList ID="ddProductType" runat="server" DataTextField="Name" DataValueField="ProductTypeId" AppendDataBoundItems="true" />
                                        <portal:gbHelpLink ID="GbHelpLink12" runat="server" HelpKey="productedit-selectProducType-help" />
                                    </div>
                                </div>
                            </div>
                            <div id="divParent" class="settingrow form-group row align-items-center" runat="server">
                                <gb:SiteLabel ID="lblParent" runat="server" ForControl="autParent" CssClass="settinglabel control-label col-sm-3" Text="Tên sản phẩm cha" />
                                <div class="col-sm-9">
                                    <telerik:RadAutoCompleteBox ID="cobParent" SkinID="radAutoCompleteBoxSkin"
                                        Width="100%" DropDownWidth="300" AllowCustomEntry="false" OnClientEntryAdding="OnClientEntryAddingHandler"
                                        WebServiceSettings-Path="~/Product/AdminCP/ProductEdit.aspx" WebServiceSettings-Method="GetProductParent"
                                        runat="server" />
                                    <script type="text/javascript">
                                        function OnClientEntryAddingHandler(sender, eventArgs) {
                                            if (sender.get_entries().get_count() > 0) {
                                                eventArgs.set_cancel(true);
                                            }
                                        }
                                    </script>
                                </div>
                            </div>
                            <div id="divProductCode" runat="server" class="settingrow form-group product-code">
                                <gb:SiteLabel ID="lblProductCode" runat="server" ConfigKey="ProductCodeLabel" ResourceFile="ProductResources"
                                    ForControl="txtProductCode" CssClass="settinglabel control-label col-sm-3" />
                                <div class="col-sm-9">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtProductCode" MaxLength="50" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div id="divPrice" runat="server" class="settingrow form-group product-price">
                                <gb:SiteLabel ID="lblPrice" runat="server" ConfigKey="ProductPriceLabel" ResourceFile="ProductResources"
                                    ForControl="txtPrice" CssClass="settinglabel control-label col-sm-3" />
                                <div class="col-sm-9">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtPrice" SkinID="PriceTextBox" MaxLength="50" runat="server" />
                                        <portal:gbHelpLink ID="GbHelpLink13" runat="server" HelpKey="productedit-price-help" />
                                    </div>
                                </div>
                            </div>
                            <div id="divOldPrice" runat="server" class="settingrow form-group product-oldprice">
                                <gb:SiteLabel ID="lblOldPrice" runat="server" ConfigKey="ProductOldPriceLabel" ResourceFile="ProductResources"
                                    ForControl="txtOldPrice" CssClass="settinglabel control-label col-sm-3" />
                                <div class="col-sm-9">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtOldPrice" SkinID="PriceTextBox" MaxLength="50" runat="server" />
                                        <portal:gbHelpLink ID="GbHelpLink14" runat="server" HelpKey="productedit-oldprice-help" />
                                    </div>
                                </div>
                            </div>

                            <div id="divGroupPrice" runat="server" class="settingrow form-group product-groupprice" visible="false">
                                <gb:SiteLabel ID="SiteLabel3" runat="server" Text="Giá khi mua nhóm sản phẩm"
                                    ForControl="txtOldPrice" CssClass="settinglabel control-label col-sm-3" />
                                <div class="col-sm-9">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtGroupPrice" SkinID="PriceTextBox" MaxLength="50" runat="server" />
                                        <portal:gbHelpLink runat="server" HelpKey="productedit-groupprice-help" />
                                    </div>
                                </div>
                            </div>
                            <div id="divApiProductID" runat="server"
                                class="settingrow form-group" visible="false">
                                <gb:SiteLabel ID="lblApiProductID" runat="server" Text="Mã sản phẩm API"
                                    ForControl="txtApiProductID" CssClass="settinglabel control-label col-sm-3" />
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtApiProductID" MaxLength="255" runat="server"
                                        CssClass="form-control" />
                                    <div class="input-group-btn">
                                        <asp:Button SkinID="DefaultButton" ID="btnSentToNhanh" Text="Cập nhật sang Nhanh"
                                            runat="server" CausesValidation="false" />
                                    </div>
                                </div>
                            </div>
                            <div id="divFileAttachment" runat="server" class="settingrow form-group product-attachfile">
                                <gb:SiteLabel ID="FileAttachmentLabel" runat="server" ForControl="txtFileAttachment"
                                    ResourceFile="ProductResources" ConfigKey="FileAttachmentLabel" CssClass="settinglabel control-label col-sm-3" />
                                <div class="col-sm-9">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtFileAttachment" MaxLength="255" runat="server" />
                                        <%--  <div class="input-group-addon">
                                            <portal:FileBrowserTextBoxExtender ID="FileAttachmentBrowser" runat="server" BrowserType="file" />
                                        </div>--%>
                                        <portal:gbHelpLink ID="GbHelpLink7" runat="server" HelpKey="productedit-fileattachment-help" />
                                    </div>
                                </div>
                            </div>
                            <div id="divStockQuantity" runat="server" class="settingrow form-group">
                                <gb:SiteLabel ID="lblStockQuantity" runat="server" ConfigKey="ProductStockQuantityLabel" ResourceFile="ProductResources"
                                    ForControl="txtStockQuantity" CssClass="settinglabel control-label col-sm-3" />
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtStockQuantity" SkinID="NumericTextBox" MaxLength="10" runat="server" />
                                </div>
                            </div>
                            <div id="divWeight" runat="server" class="settingrow form-group">
                                <gb:SiteLabel ID="lblWeight" runat="server" ConfigKey="ProductWeightLabel"
                                    ResourceFile="ProductResources" ForControl="txtWeight"
                                    CssClass="settinglabel control-label col-sm-3" />
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtWeight" SkinID="NumericTextBox" MaxLength="10"
                                        runat="server" />
                                </div>
                            </div>
                            <div id="divRatingSum" runat="server" class="settingrow form-group">
                                <gb:SiteLabel ID="lblRatingSum" runat="server" Text="Điểm đánh giá"
                                    ForControl="txtRatingSum" CssClass="settinglabel control-label col-sm-3 " />
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtRatingSum" SkinID="NumericTextBox" MaxLength="10"
                                        runat="server" CssClass="" />
                                </div>
                            </div>
                            <div id="divRatingVotes" runat="server" class="settingrow form-group">
                                <gb:SiteLabel ID="lblRatingVotes" runat="server" Text="Số bình chọn"
                                    ForControl="txtRatingSum" CssClass="settinglabel control-label col-sm-3 " />
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtRatingVotes" SkinID="NumericTextBox" MaxLength="10"
                                        runat="server" CssClass="" />
                                </div>
                            </div>

                            <asp:Panel runat="server" ID="pnlProductService" Visible="false">
                                <asp:Repeater runat="server" ID="rpProductServiceFee">
                                    <ItemTemplate>
                                        <br />
                                        <div class="settingrow form-group">
                                            <asp:HiddenField runat="server" ID="hdType" Value='<%#Eval("Type")%>' />
                                            <asp:HiddenField runat="server" ID="hdServiceFeeGuid" Value='<%#Eval("itemGuid")%>' />
                                            <gb:SiteLabel runat="server" ID="gbLabelServiceTitle" CssClass="settinglabel control-label col-sm-3 " />
                                            <div class="col-sm-9">
                                            </div>
                                        </div>
                                        <div class="settingrow form-group">
                                            <gb:SiteLabel runat="server" Text="Phí"
                                                ForControl="txtServiceFee" CssClass="settinglabel control-label col-sm-3 " />
                                            <div class="col-sm-9">
                                                <asp:TextBox ID="txtServiceFee" SkinID="PriceTextBox" MaxLength="50" Text='<%#  CanhCam.Web.ProductUI.ProductHelper.FormatPrice(Convert.ToDecimal( Eval("Price")))  %>'
                                                    runat="server" CssClass="" />
                                            </div>
                                        </div>
                                        <div class="settingrow form-group">
                                            <gb:SiteLabel runat="server" Text="Miễn phí khi đơn hàng >="
                                                ForControl="txtFeeForOrderTotal" CssClass="settinglabel control-label col-sm-3 " />
                                            <div class="col-sm-9">
                                                <asp:TextBox ID="txtFeeForOrderTotal" SkinID="PriceTextBox" MaxLength="50" Text='<%# CanhCam.Web.ProductUI.ProductHelper.FormatPrice(Convert.ToDecimal( Eval("FeeForOrderTotal")))%>'
                                                    runat="server" CssClass="" />
                                            </div>
                                        </div>
                                        <%--  <div class="settingrow form-group">
                                            <gb:SiteLabel runat="server" Text="Miễn Phí khi thanh toán online"
                                                ForControl="cbFreeForPaymentOnline" CssClass="settinglabel control-label col-sm-3 " />
                                            <div class="col-sm-9">
                                                <asp:CheckBox runat="server" ID="cbFreeForPaymentOnline" Checked='<%#Eval("FreeForPaymentOnline")%>' /> 
                                            </div>
                                        </div>--%>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </asp:Panel>
                            <asp:UpdatePanel ID="up" runat="server">
                                <ContentTemplate>
                                    <telerik:RadTabStrip ID="tabLanguage" OnTabClick="tabLanguage_TabClick"
                                        EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false"
                                        CssClass="subtabs" SkinID="SubTabs" Visible="false" SelectedIndex="0" runat="server" />
                                    <div class="settingrow form-group">
                                        <gb:SiteLabel ID="lblTitle" runat="server" ForControl="txtTitle" CssClass="settinglabel control-label col-sm-3"
                                            ConfigKey="ProductNameLabel" ResourceFile="ProductResources" />
                                        <div class="col-sm-9">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtTitle" runat="server" MaxLength="255" CssClass="forminput verywidetextbox" />
                                                <portal:gbHelpLink ID="GbHelpLink1" runat="server" HelpKey="productedit-title-help" />
                                            </div>
                                            <asp:RequiredFieldValidator ID="reqTitle" runat="server" ControlToValidate="txtTitle"
                                                Display="Dynamic" SetFocusOnError="true" CssClass="txterror" ValidationGroup="product" />
                                        </div>
                                    </div>
                                    <div id="divSubTitle" runat="server" class="settingrow form-group product-subtitle">
                                        <gb:SiteLabel ID="lblSubTitle" runat="server" ForControl="txtSubTitle" CssClass="settinglabel control-label col-sm-3"
                                            ConfigKey="SubTitle" ResourceFile="ProductResources" />
                                        <div class="col-sm-9">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtSubTitle" runat="server" MaxLength="255" CssClass="forminput verywidetextbox" />
                                                <portal:gbHelpLink ID="GbHelpLink2" runat="server" HelpKey="productedit-subtitle-help" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="settingrow form-group">
                                        <gb:SiteLabel ID="lblUrl" runat="server" ForControl="txtUrl" CssClass="settinglabel control-label col-sm-3"
                                            ConfigKey="ProductEditItemUrlLabel" ResourceFile="ProductResources" />
                                        <div class="col-sm-9">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtUrl" runat="server" MaxLength="255" CssClass="forminput verywidetextbox" />
                                                <portal:gbHelpLink ID="GbHelpLink3" runat="server" HelpKey="productedit-url-help" />
                                            </div>
                                            <span id="spnUrlWarning" runat="server" style="font-weight: normal; display: none;" class="txterror"></span>
                                            <asp:HiddenField ID="hdnTitle" runat="server" />
                                            <asp:RegularExpressionValidator ID="regexUrl" runat="server" ControlToValidate="txtUrl"
                                                ValidationExpression="((http\://|https\://|~/){1}(\S+){0,1})" Display="Dynamic" SetFocusOnError="true"
                                                ValidationGroup="product" />
                                        </div>
                                    </div>
                                    <div class="settingrow form-group">
                                        <gb:SiteLabel ID="lblBriefContent" runat="server" CssClass="settinglabel control-label col-sm-3" ConfigKey="ProductEditBriefContentLabel"
                                            ResourceFile="ProductResources" />
                                        <div class="col-sm-9">
                                            <div class="input-group">
                                                <gbe:EditorControl ID="edBriefContent" runat="server" />
                                                <portal:gbHelpLink ID="GbHelpLink4" runat="server" HelpKey="productedit-briefcontent-help" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="settingrow form-group">
                                        <gb:SiteLabel ID="lblFullContent" runat="server" CssClass="settinglabel control-label col-sm-3" ConfigKey="ProductEditContentLabel"
                                            ResourceFile="ProductResources" />
                                        <div class="col-sm-9">
                                            <div class="input-group">
                                                <gbe:EditorControl ID="edFullContent" runat="server" />
                                                <portal:gbHelpLink ID="GbHelpLink5" runat="server" HelpKey="productedit-fullcontent-help" />
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div class="settingrow form-group" runat="server" id="divUploadImage">
                                <gb:SiteLabel ID="lblImage" runat="server" ConfigKey="ImageFile" ResourceFile="ProductResources" CssClass="settinglabel control-label col-sm-3" />
                                <div class="col-sm-9">
                                    <telerik:RadAsyncUpload ID="fileImage" SkinID="radAsyncUploadSkin" MultipleFileSelection="Automatic"
                                        AllowedFileExtensions="jpg,jpeg,gif,png" runat="server" />
                                </div>
                            </div>

                            <div id="divProductTags" class="settingrow form-group product-tags" runat="server">
                                <gb:SiteLabel ID="lblProductTags" runat="server" ForControl="autProductTags" CssClass="settinglabel control-label col-sm-3"
                                    ConfigKey="ProductTagsLabel" ResourceFile="ProductResources"></gb:SiteLabel>
                                <div class="col-sm-9">
                                    <telerik:RadAutoCompleteBox ID="autProductTags" SkinID="radAutoCompleteBoxSkin"
                                        Width="100%" DropDownWidth="100%" AllowCustomEntry="true"
                                        WebServiceSettings-Path="~/Product/AdminCP/ProductEdit.aspx" WebServiceSettings-Method="GetProductTags"
                                        runat="server">
                                    </telerik:RadAutoCompleteBox>
                                </div>
                            </div>
                            <div id="divPosition" visible="false" runat="server" class="settingrow form-group">
                                <gb:SiteLabel ID="lblPosition" runat="server" ForControl="chlPosition" ConfigKey="ProductPositionLabel"
                                    ResourceFile="ProductResources" CssClass="settinglabel control-label col-sm-3" />
                                <div class="col-sm-9">
                                    <asp:CheckBoxList ID="chlPosition" SkinID="Enum" DataValueField="Value" DataTextField="Name" runat="server" />
                                </div>
                            </div>
                            <div id="divShowOption" visible="false" runat="server" class="settingrow form-group">
                                <gb:SiteLabel ID="lblShowOption" runat="server" ForControl="chlShowOption" ConfigKey="ShowOptionLabel"
                                    ResourceFile="ProductResources" CssClass="settinglabel control-label col-sm-3" />
                                <div class="col-sm-9">
                                    <asp:CheckBoxList ID="chlShowOption" SkinID="Enum" runat="server" />
                                </div>
                            </div>
                            <div class="settingrow form-group">
                                <gb:SiteLabel ID="lblOpenInNewWindow" runat="server" ForControl="chkOpenInNewWindow" ConfigKey="OpenInNewWindowLabel"
                                    ResourceFile="ProductResources" CssClass="settinglabel control-label col-sm-3" />
                                <div class="col-sm-9">
                                    <asp:CheckBox ID="chkOpenInNewWindow" runat="server" Checked="false" />
                                </div>
                            </div>
                            <div class="settingrow form-group">
                                <gb:SiteLabel ID="SiteLabel1" runat="server" ForControl="chkOpenInNewWindow" ConfigKey="EnableBuyButton"
                                    ResourceFile="ProductResources" CssClass="settinglabel control-label col-sm-3" />
                                <div class="col-sm-9">
                                    <asp:CheckBox ID="cbEnableBuyButton" runat="server" Checked="true" />
                                </div>
                            </div>
                            <div id="divIsPublished" class="settingrow form-group" runat="server">
                                <gb:SiteLabel ID="lblIsPublished" runat="server" ForControl="chkIsPublished" ConfigKey="IsPublishedLabel"
                                    ResourceFile="ProductResources" CssClass="settinglabel control-label col-sm-3" />
                                <div class="col-sm-9">
                                    <asp:CheckBox ID="chkIsPublished" runat="server" Checked="true" />
                                </div>
                            </div>
                            <div class="settingrow form-group">
                                <gb:SiteLabel ID="lblStartDate" runat="server" ConfigKey="ProductEditStartDateLabel"
                                    ResourceFile="ProductResources" CssClass="settinglabel control-label col-sm-3" />
                                <div class="col-sm-9">
                                    <gb:DatePickerControl ID="dpBeginDate" runat="server" ShowTime="True" SkinID="product"
                                        CssClass="forminput"></gb:DatePickerControl>
                                    <portal:gbHelpLink ID="GbHelpLink10" runat="server" RenderWrapper="false" HelpKey="productedit-startdate-help" />
                                    <asp:RequiredFieldValidator ID="reqStartDate" runat="server" ControlToValidate="dpBeginDate"
                                        Display="Dynamic" SetFocusOnError="true" CssClass="txterror" ValidationGroup="product" />
                                </div>
                            </div>
                            <div id="divEndDate" runat="server" visible="false" class="settingrow form-group">
                                <gb:SiteLabel ID="lblEndDate" runat="server" ConfigKey="ProductEditEndDateLabel" ResourceFile="ProductResources"
                                    CssClass="settinglabel control-label col-sm-3" />
                                <div class="col-sm-9">
                                    <gb:DatePickerControl ID="dpEndDate" runat="server" ShowTime="True" SkinID="product"
                                        CssClass="forminput"></gb:DatePickerControl>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div role="tabpanel" class="tab-pane fade" id="tabImages" runat="server">
                        <asp:UpdatePanel ID="updImages" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="form-horizontal">
                                    <div class="settingrow form-group">
                                        <gb:SiteLabel ID="lblProductImages" runat="server" ConfigKey="ProductImagesLabel" ResourceFile="ProductResources"
                                            CssClass="settinglabel control-label col-sm-3" />
                                        <div class="col-sm-9">
                                            <telerik:RadAsyncUpload ID="uplImageFile" SkinID="radAsyncUploadSkin" MultipleFileSelection="Automatic" HideFileInput="true"
                                                AllowedFileExtensions="jpg,jpeg,gif,png" Localization-Select="<%$Resources:ProductResources, SelectFromComputerLabel %>" runat="server" />
                                        </div>
                                    </div>
                                    <div id="divProductVideos" runat="server" class="settingrow form-group">
                                        <gb:SiteLabel ID="lblVideoPath" runat="server" ConfigKey="ProductVideoUrlLabel" ResourceFile="ProductResources"
                                            CssClass="settinglabel control-label col-sm-3" />
                                        <div class="col-sm-9">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtVideoPath" MaxLength="255" runat="server" />
                                                <div class="input-group-addon">
                                                    <portal:FileBrowserTextBoxExtender ID="VideoFileBrowser" Text='<%$Resources:ProductResources, BrowserOnServerLink %>' runat="server" BrowserType="video" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="settingrow form-group">
                                        <div class="col-sm-offset-3 col-sm-9">
                                            <asp:Button SkinID="DefaultButton" ID="btnUpdateImage" OnClick="btnUpdateImage_Click"
                                                Text="<%$Resources:ProductResources, ProductUpdateButton %>" runat="server" CausesValidation="False" />
                                            <asp:Button SkinID="DefaultButton" ID="btnDeleteImage" Visible="false" OnClick="btnDeleteImage_Click"
                                                Text="<%$Resources:ProductResources, ProductDeleteSelectedButton %>" runat="server"
                                                CausesValidation="False" />
                                            <portal:gbHelpLink ID="GbHelpLink11" runat="server" RenderWrapper="false" HelpKey="productedit-mediaupload-help" />
                                        </div>
                                    </div>
                                </div>
                                <telerik:RadGrid ID="grid" SkinID="radGridSkin" runat="server"
                                    OnNeedDataSource="grid_NeedDataSource" OnItemDataBound="grid_ItemDataBound">
                                    <MasterTableView DataKeyNames="Guid,LanguageId,DisplayOrder,Title,MediaType,MediaFile,ThumbnailFile">
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderStyle-Width="35" HeaderText="<%$Resources:Resource, RowNumber %>" AllowFiltering="false">
                                                <ItemTemplate>
                                                    <%# (grid.PageSize * grid.CurrentPageIndex) + Container.ItemIndex + 1%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="<%$Resources:ProductResources, ProductThumbnailLabel %>">
                                                <ItemTemplate>
                                                    <portal:MediaElement ID="ml1" runat="server" Width="100" FileUrl='<%# CanhCam.Web.ProductUI.ProductHelper.GetMediaFilePath(thumbnailImageFolderPath, Eval("ThumbnailFile").ToString()) %>' />
                                                    <telerik:RadAsyncUpload ID="fupThumbnail" SkinID="radAsyncUploadSkin" Width="100%" HideFileInput="true" MultipleFileSelection="Disabled"
                                                        AllowedFileExtensions="jpg,jpeg,gif,png" Localization-Select="<%$Resources:ProductResources, SelectFromComputerLabel %>" runat="server" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="<%$Resources:ProductResources, ProductImagesLabel %>" UniqueName="MediaFile">
                                                <ItemTemplate>
                                                    <portal:MediaElement ID="ml2" runat="server" Width="100" IncludeDownloadLinkForMedia="false" FileUrl='<%# CanhCam.Web.ProductUI.ProductHelper.GetMediaFilePath(imageFolderPath, Eval("MediaFile").ToString()) %>' />
                                                    <telerik:RadAsyncUpload ID="fupImageFile" SkinID="radAsyncUploadSkin" Width="100%" HideFileInput="true" MultipleFileSelection="Disabled"
                                                        AllowedFileExtensions="jpg,jpeg,gif,png" Localization-Select="<%$Resources:ProductResources, SelectFromComputerLabel %>" runat="server" />
                                                    <asp:TextBox ID="txtVideoPath" MaxLength="255" Text='<%# Eval("MediaFile").ToString().Contains("/") ? Eval("MediaFile").ToString() : "" %>' runat="server" />
                                                    <portal:FileBrowserTextBoxExtender ID="VideoFileBrowser" Text='<%$Resources:VideoResources, FileBrowserLink %>' runat="server" BrowserType="video" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="<%$Resources:ProductResources, ProductImageTitleLabel %>">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtTitle" CssClass="input-grid" Width="97%" MaxLength="255" Text='<%# Eval("Title") %>'
                                                        runat="server" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn Visible="false" HeaderStyle-Width="150" HeaderText="<%$Resources:ProductResources, ProductColorLabel %>" UniqueName="MediaType">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlMediaType" AppendDataBoundItems="true" Visible="false" DataValueField="CustomFieldOptionID" DataTextField="Name" runat="server" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderStyle-Width="150" HeaderText="<%$Resources:ProductResources, ProductEditLanguageLabel %>" UniqueName="LanguageID">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlLanguage" AppendDataBoundItems="true" DataValueField="LanguageID" DataTextField="Name" runat="server" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderStyle-Width="100" HeaderText="<%$Resources:ProductResources, ProductEditDisplayOrderLabel %>">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDisplayOrder" SkinID="NumericTextBox" MaxLength="4" Text='<%# Eval("DisplayOrder") %>'
                                                        runat="server" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridClientSelectColumn HeaderStyle-Width="35" />
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div role="tabpanel" class="tab-pane fade" id="tabAttribute" runat="server">
                        <asp:UpdatePanel ID="updAttribute" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-3">
                                        <div class="input-group">
                                            <asp:ListBox ID="lbAttribute" Width="100%" Height="500" AutoPostBack="true" runat="server"
                                                AppendDataBoundItems="true" DataTextField="Title" DataValueField="Guid" />
                                            <div class="input-group-addon">
                                                <ul class="nav sorter">
                                                    <li>
                                                        <asp:LinkButton ID="btnAttributeUp" CssClass="btn btn-default" CommandName="up" runat="server"><i class="fa fa-angle-up"></i></asp:LinkButton></li>
                                                    <li>
                                                        <asp:LinkButton ID="btnAttributeDown" CssClass="btn btn-default" CommandName="down" runat="server"><i class="fa fa-angle-down"></i></asp:LinkButton></li>
                                                    <li>
                                                        <asp:LinkButton ID="btnDeleteAttribute" CssClass="btn btn-default" runat="server" CausesValidation="False"><i class="fa fa-trash"></i></asp:LinkButton></li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-9">
                                        <telerik:RadTabStrip ID="tabAttributeLanguage" OnTabClick="tabAttributeLanguage_TabClick"
                                            EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false"
                                            CssClass="subtabs" SkinID="SubTabs" Visible="false" SelectedIndex="0" runat="server" />
                                        <div class="form-horizontal">
                                            <div class="settingrow form-group">
                                                <gb:SiteLabel ID="lblAttributeTitle" runat="server" CssClass="settinglabel control-label col-sm-3" ForControl="txtAttributeTitle" ConfigKey="AttributeTitle"
                                                    ResourceFile="ProductResources" />
                                                <div class="col-sm-9">
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtAttributeTitle" runat="server" MaxLength="255" />
                                                        <portal:gbHelpLink ID="GbHelpLink8" runat="server" HelpKey="productedit-attribute-help" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="settingrow form-group">
                                                <div class="col-sm-offset-3 col-sm-9">
                                                    <asp:Button SkinID="DefaultButton" ID="btnAttributeUpdate" Text="<%$Resources:ProductResources, AttributeUpdateButton %>" runat="server" />
                                                    <asp:Button SkinID="DefaultButton" ID="btnDeleteAttributeLanguage" Visible="false" OnClick="btnDeleteAttributeLanguage_Click" Text="<%$Resources:Resource, DeleteLanguageButton %>" runat="server" CausesValidation="false" />
                                                </div>
                                            </div>
                                            <div class="settingrow form-group">
                                                <gb:SiteLabel ID="lblAttributeContent" runat="server" CssClass="settinglabel" ConfigKey="AttributeContent"
                                                    ResourceFile="ProductResources" />
                                                <div>
                                                    <gbe:EditorControl ID="edAttributeContent" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div role="tabpanel" class="tab-pane fade" id="tabCustomField" visible="false" runat="server">
                        <div class="form-horizontal">
                            <asp:Repeater ID="rptCustomFields" runat="server">
                                <ItemTemplate>
                                    <div class="settingrow form-group">
                                        <gb:SiteLabel ID="lblText" runat="server" Text='<%#Eval("Name") %>' ForControl="txtField" CssClass="settinglabel control-label col-sm-3" />
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="txtField" runat="server" />
                                            <asp:DropDownList ID="ddlField" AppendDataBoundItems="true" DataTextField="Name" DataValueField="CustomFieldOptionID" runat="server" />
                                            <asp:CheckBoxList ID="cblField" DataTextField="Name" DataValueField="CustomFieldOptionID" SkinID="Enum" runat="server" />
                                            <asp:HiddenField ID="hdfCustomFieldId" Value='<%#Eval("CustomFieldId")%>' runat="server" />
                                            <asp:HiddenField ID="hdfDataType" Value='<%#Eval("DataType")%>' runat="server" />
                                            <asp:HiddenField ID="hdfOptions" Value='<%#Eval("Options")%>' runat="server" />

                                            <asp:Repeater ID="rptCustomFieldOptions" Visible="false" runat="server">
                                                <ItemTemplate>
                                                    <div class="field-overridden">
                                                        <asp:CheckBox ID="cbxField" runat="server" />
                                                        <%# Eval("Name") %>
                                                        <asp:TextBox ID="txtOverriddenPrice" SkinID="PriceTextBox" MaxLength="50" runat="server" />
                                                        <asp:HiddenField ID="hdfCustomFieldOptionId" Value='<%#Eval("CustomFieldOptionId")%>' runat="server" />
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                    <div role="tabpanel" class="tab-pane fade" id="tabRelatedProduct" runat="server">
                        <Site:ProductRelated ID="relatedProduct" runat="server" />
                    </div>
                    <div role="tabpanel" class="tab-pane fade" id="tabRelatedNews" runat="server">
                        <Site:NewsRelated ID="relatedNews" runat="server" />
                    </div>
                    <div role="tabpanel" class="tab-pane fade" id="tabGroupProduct" runat="server">
                        <Site:ProductGroup ID="groupProduct" runat="server" />
                    </div>
                    <div role="tabpanel" class="tab-pane fade" id="tabChildProduct" runat="server">
                        <Site:ProductChild ID="productChild" runat="server" />
                    </div>
                    <div role="tabpanel" class="tab-pane fade" id="tabMeta">
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
                                            <div class="input-group">
                                                <asp:TextBox ID="txtMetaTitle" runat="server" MaxLength="65" />
                                                <portal:gbHelpLink ID="GbHelpLink6" runat="server" HelpKey="productedit-metatitle-help" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="settingrow form-group">
                                        <gb:SiteLabel ID="lblMetaDescription" runat="server" ForControl="txtMetaDescription" CssClass="settinglabel control-label col-sm-3"
                                            ConfigKey="MetaDescriptionLabel" ResourceFile="ProductResources" />
                                        <div class="col-sm-9">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtMetaDescription" runat="server" MaxLength="156" />
                                                <portal:gbHelpLink ID="gbHelpLink22" runat="server" HelpKey="productedit-metakeywords-help" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="settingrow form-group">
                                        <gb:SiteLabel ID="lblMetaKeywords" runat="server" ForControl="txtMetaKeywords" CssClass="settinglabel control-label col-sm-3"
                                            ConfigKey="MetaKeywordsLabel" ResourceFile="ProductResources" />
                                        <div class="col-sm-9">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtMetaKeywords" runat="server" MaxLength="156" />
                                                <portal:gbHelpLink ID="gbHelpLink23" runat="server" HelpKey="productedit-metadescription-help" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="settingrow form-group">
                                        <gb:SiteLabel ID="lblAdditionalMetaTags" ForControl="txtAdditionalMetaTags" CssClass="settinglabel control-label col-sm-3" runat="server" ConfigKey="MetaAdditionalLabel" />
                                        <div class="col-sm-9">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtAdditionalMetaTags" CssClass="form-control" TextMode="MultiLine" runat="server" />
                                                <portal:gbHelpLink ID="gbHelpLink25" runat="server" HelpKey="productedit-additionalmeta-help" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
    <portal:SessionKeepAliveControl ID="ka1" runat="server" />
</div>
<asp:Panel CssClass="modal fade" ID="pnlModal" Visible="false" runat="server" TabIndex="-1" role="dialog" aria-labelledby="pnlModalLabel">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title" id="pnlModalLabel">
                    <gb:SiteLabel ID="lblCopyProduct" runat="server" UseLabelTag="false" ConfigKey="CopyProductLabel" ResourceFile="ProductResources" />
                </h4>
            </div>
            <div class="modal-body">
                <div class="form-horizontal">
                    <div class="settingrow form-group">
                        <gb:SiteLabel ID="lblCopyProductName" runat="server" ConfigKey="CopyProductNameLabel" ResourceFile="ProductResources"
                            ForControl="txtCopyProductName" CssClass="settinglabel control-label col-sm-3" />
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtCopyProductName" runat="server" MaxLength="255" />
                            <asp:RequiredFieldValidator ID="reqCopyProductName" runat="server" ControlToValidate="txtCopyProductName"
                                Display="Dynamic" SetFocusOnError="true" CssClass="txterror" ValidationGroup="productCopy" />
                        </div>
                    </div>
                    <div id="divCopyProductPublished" runat="server" class="settingrow form-group">
                        <gb:SiteLabel ID="lblCopyProductPublished" runat="server" ConfigKey="CopyProductPublishedLabel" ResourceFile="ProductResources"
                            ForControl="chkCopyProductPublished" CssClass="settinglabel control-label col-sm-3" />
                        <div class="col-sm-9">
                            <asp:CheckBox ID="chkCopyProductPublished" runat="server" Checked="true" />
                        </div>
                    </div>
                    <div class="settingrow form-group">
                        <gb:SiteLabel ID="lblCopyProductCopyImages" runat="server" ConfigKey="CopyProductCopyImagesLabel" ResourceFile="ProductResources"
                            ForControl="chkCopyProductCopyImages" CssClass="settinglabel control-label col-sm-3" />
                        <div class="col-sm-9">
                            <asp:CheckBox ID="chkCopyProductCopyImages" runat="server" Checked="true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <asp:Button ID="btnCopyProduct" CssClass="btn btn-primary" Text="Copy" ValidationGroup="productCopy" runat="server" />
            </div>
        </div>
    </div>
</asp:Panel>

<style type="text/css">
    .field-overridden {
        margin-bottom: 5px;
    }

        .field-overridden input[type='text'] {
            display: inline-block;
            width: 150px !important;
        }
</style>
