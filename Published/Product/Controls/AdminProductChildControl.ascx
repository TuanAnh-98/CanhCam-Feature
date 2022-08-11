<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="AdminProductChildControl.ascx.cs" Inherits="CanhCam.Web.ProductUI.AdminProductChildControl" %>

<%@ Import Namespace="CanhCam.Web.ProductUI" %>
<%@ Register TagPrefix="Site" Assembly="CanhCam.Features.Product" Namespace="CanhCam.Web.ProductUI" %>
<Site:ProductDisplaySettings ID="displaySettings" runat="server" />
<asp:UpdatePanel ID="up" runat="server"  UpdateMode="Conditional">
    <ContentTemplate>
        <div class="row">
            <div class="col-12">
                <asp:Panel ID="pnlSearch" CssClass="headInfo admin-content-bg-white" runat="server">
                    <asp:LinkButton SkinID="UpdateButton" ID="btnGenarationProductChild" Text="<%$Resources:ProductResources, GenarationProductChildButton %>"
                        runat="server" data-toggle="modal" data-target="#pnlAddProductChildModal" />
                    <asp:LinkButton SkinID="UpdateButton" ID="btnAddChild" Text="<%$Resources:ProductResources, ProductAddProductChildButton %>"
                        runat="server" data-toggle="modal" data-target="#pnlAddProductChildModal" />
                    <asp:Button SkinID="UpdateButton" ID="btnUpdate" Text="<%$Resources:ProductResources, ProductUpdateButton %>"
                        runat="server" />
                    <asp:Button SkinID="DeleteButton" ID="btnDelete" Text="<%$Resources:ProductResources, ProductDeleteSelectedButton %>"
                        runat="server" CausesValidation="false" />
                </asp:Panel>
            </div>
            <div class="col-12">
                <div class="workplace admin-content-bg-white">
                    <telerik:RadGrid ID="grid" SkinID="radGridSkin" runat="server">
                        <MasterTableView DataKeyNames="ProductId,ZoneId,DisplayOrder,ViewCount,Code,StockQuantity,Price,OldPrice">
                            <Columns>
                                <telerik:GridTemplateColumn HeaderStyle-Width="35" HeaderText="<%$Resources:Resource, RowNumber %>">
                                    <ItemTemplate>
                                        <%# (grid.PageSize * grid.CurrentPageIndex) + Container.ItemIndex + 1%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderStyle-Width="100" HeaderText="<%$Resources:ProductResources, ProductPictureHeading %>">
                                    <ItemTemplate>
                                        <portal:MediaElement ID="ml" runat="server" Width="90" FileUrl='<%# ProductHelper.GetImageFilePath(siteSettings.SiteId, Convert.ToInt32(Eval("ProductId")), Eval("ImageFile").ToString(), Eval("ThumbnailFile").ToString()) %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderStyle-Width="115" HeaderStyle-Wrap="false" HeaderText="<%$Resources:ProductResources, ProductCodeLabel %>" UniqueName="ProductCode">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtProductCode" Width="95"
                                            MaxLength="50" Text='<%# Eval("Code") %>' runat="server" CssClass="form-control"/>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderStyle-Width="115" HeaderStyle-Wrap="false" HeaderText="<%$Resources:ProductResources, ProductStockQuantityLabel %>" UniqueName="StockQuantity">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtStockQuantity" Width="95" MaxLength="50" Text='<%# Eval("StockQuantity") %>' runat="server" CssClass="form-control"/>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="<%$Resources:ProductResources, ProductNameLabel %>">
                                    <ItemTemplate>
                                        <asp:HyperLink CssClass="cp-link" ID="Title" runat="server" Text='<%# Eval("Title").ToString() %>'
                                            NavigateUrl='<%# ProductHelper.FormatProductUrl(Eval("Url").ToString(), Convert.ToInt32(Eval("ProductId")), Convert.ToInt32(Eval("ZoneId")))  %>'>
                                        </asp:HyperLink>
                                        <%# GetShoppingPropertites(Convert.ToInt32(Eval("ProductId"))) %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderStyle-Width="100" HeaderText="<%$Resources:ProductResources, ProductPriceLabel %>" UniqueName="Price">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPrice" SkinID="PriceTextBox" Style="text-align: right"
                                            MaxLength="50" Text='<%# ProductHelper.FormatPrice(Convert.ToDecimal(Eval("Price"))) %>' runat="server" CssClass="form-control"/>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderStyle-Width="100" HeaderText="<%$Resources:ProductResources, ProductOldPriceLabel %>" UniqueName="OldPrice">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtOldPrice" SkinID="PriceTextBox" Style="text-align: right"
                                            MaxLength="50" Text='<%# ProductHelper.FormatPrice(Convert.ToDecimal(Eval("OldPrice"))) %>' runat="server" CssClass="form-control"/>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderStyle-Width="100" HeaderText="<%$Resources:ProductResources, ProductDisplayOrderLabel %>">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDisplayOrder" SkinID="NumericTextBox"
                                            MaxLength="4" Text='<%# Eval("DisplayOrder") %>' runat="server" CssClass="form-control"/>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn Visible="false" HeaderStyle-Width="100" HeaderText="<%$Resources:ProductResources, ProductViewCountLabel %>"
                                    AllowFiltering="false">
                                    <ItemTemplate>
                                        <%# Eval("ViewCount") %>
                                        <asp:TextBox ID="txtViewCount" SkinID="NumericTextBox" Visible="false"
                                            MaxLength="9" Text='<%# Eval("ViewCount") %>' runat="server" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderStyle-Width="50">
                                    <ItemTemplate>
                                        <asp:HyperLink CssClass="cp-link" ID="EditLink" runat="server"
                                            Visible='<%# CanEditProduct(Convert.ToInt32(Eval("UserId")), Convert.ToBoolean(Eval("IsPublished")), Eval("StateId")) %>'
                                            Text="<%$Resources:ProductResources, ProductEditLink %>" NavigateUrl='<%# siteRoot + EditPageUrl + "?ProductID=" + Eval("ProductId") %>'>
                                        </asp:HyperLink>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridClientSelectColumn HeaderStyle-Width="35" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
            </div>
        </div>
    
<asp:Panel CssClass="modal  fade" ID="pnlAddProductChildModal" Visible="false" runat="server" TabIndex="-1" role="dialog"
    aria-labelledby="pnlModalLabel">
    <div class="modal-dialog modal-lg" role="document">
        
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title" id="pnlModalLabel">
                            <gb:SiteLabel ID="lblChildProduct" runat="server" UseLabelTag="false" ConfigKey="ChildProductLabel"
                                ResourceFile="ProductResources" />
                        </h4>
                    </div>
                    <div class="modal-body">
                        <div id="divChildtabs" class="tabs">
                            <ul class="nav nav-pills margin-bottom">
                                <li role="presentation" class="nav-item"><a aria-controls="tabChildContent" role="tab" data-toggle="tab"
                                    href="#tabChildContent" class="nav-link active">
                                    <asp:Literal ID="litContentTab" runat="server" /></a>
                                </li>
                                <li role="presentation" id="liCustomField" visible="false" runat="server" class="nav-item">
                                    <asp:Literal ID="litCustomFieldTab" runat="server" />
                                </li>
                            </ul>
                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane fade active show in" id="tabChildContent">
                                    <div class="form-horizontal">
                                        <div class="settingrow form-group row align-items-center">
                                            <gb:SiteLabel ID="lblChildProductName" runat="server" ConfigKey="ChildProductNameLabel"
                                                ResourceFile="ProductResources" ForControl="txtChildProductName"
                                                CssClass="settinglabel control-label mb-0 col-sm-3" />
                                            <div class="col-sm-9">
                                                <asp:TextBox ID="txtChildProductName" runat="server" MaxLength="255"
                                                    CssClass="form-control" />
                                                <asp:RequiredFieldValidator ID="reqChildProductName" runat="server"
                                                    ControlToValidate="txtChildProductName" Display="Dynamic" SetFocusOnError="true"
                                                    CssClass="txterror" ValidationGroup="productChild" />
                                            </div>
                                        </div>
                                        <div id="divSubTitle" runat="server" class="settingrow form-group row align-items-center product-subtitle">
                                            <gb:SiteLabel ID="lblSubTitle" runat="server" ForControl="txtSubTitle"
                                                CssClass="settinglabel control-label mb-0 col-sm-3" ConfigKey="SubTitle"
                                                ResourceFile="ProductResources" />
                                            <div class="col-sm-9">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtSubTitle" runat="server" MaxLength="255"
                                                        CssClass="forminput verywidetextbox form-control" />
                                                    <portal:gbHelpLink ID="GbHelpLink2" runat="server"
                                                        HelpKey="productedit-subtitle-help" />
                                                </div>
                                            </div>
                                        </div>
                                        <div id="divProductCode" runat="server"
                                            class="settingrow form-group row align-items-center product-code">
                                            <gb:SiteLabel ID="lblProductCode" runat="server" ConfigKey="ProductCodeLabel"
                                                ResourceFile="ProductResources" ForControl="txtProductCode"
                                                CssClass="settinglabel control-label mb-0 col-sm-3" />
                                            <div class="col-sm-9">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtProductCode" MaxLength="50" runat="server"
                                                        CssClass="form-control w-p100" />
                                                </div>
                                            </div>
                                        </div>
                                        <div id="divPrice" runat="server"
                                            class="settingrow form-group row align-items-center product-price">
                                            <gb:SiteLabel ID="lblPrice" runat="server" ConfigKey="ProductPriceLabel"
                                                ResourceFile="ProductResources" ForControl="txtPrice"
                                                CssClass="settinglabel control-label mb-0 col-sm-3" />
                                            <div class="col-sm-9">
                                                <asp:TextBox ID="txtPrice" SkinID="PriceTextBox" MaxLength="50" runat="server"
                                                    CssClass="form-control w-p100" />
                                            </div>
                                        </div>
                                        <div id="divOldPrice" runat="server"
                                            class="settingrow form-group row align-items-center product-oldprice">
                                            <gb:SiteLabel ID="lblOldPrice" runat="server" ConfigKey="ProductOldPriceLabel"
                                                ResourceFile="ProductResources" ForControl="txtOldPrice"
                                                CssClass="settinglabel control-label mb-0 col-sm-3" />
                                            <div class="col-sm-9">
                                                <asp:TextBox ID="txtOldPrice" SkinID="PriceTextBox" MaxLength="50"
                                                    runat="server" CssClass="form-control w-p100" />
                                            </div>
                                        </div>
                                        <div id="divStockQuantity" runat="server" class="settingrow form-group row align-items-center">
                                            <gb:SiteLabel ID="lblStockQuantity" runat="server"
                                                ConfigKey="ProductStockQuantityLabel" ResourceFile="ProductResources"
                                                ForControl="txtStockQuantity" CssClass="settinglabel control-label mb-0 col-sm-3" />
                                            <div class="col-sm-9">
                                                <asp:TextBox ID="txtStockQuantity" SkinID="NumericTextBox" MaxLength="10"
                                                    runat="server" CssClass="form-control w-p100" />
                                            </div>
                                        </div>
                                        <div id="divChildProductPublished" runat="server"
                                            class="settingrow form-group row align-items-center">
                                            <div class="col-sm-9">
                                                <asp:CheckBox ID="chkChildProductPublished" runat="server" Checked="true" />
                                                <gb:SiteLabel ID="lblChildProductPublished" runat="server" ConfigKey="ChildProductPublishedLabel"
                                                    ResourceFile="ProductResources" ForControl="chkChildProductPublished"
                                                    CssClass="settinglabel control-label mb-0" />
                                            </div>
                                        </div>
                                        <div class="settingrow form-group row align-items-center">
                                            <div class="col-sm-9">
                                                <asp:CheckBox ID="chkChildProductChildImages" runat="server" Checked="true" />
                                                <gb:SiteLabel ID="lblChildProductChildImages" runat="server"
                                                    ConfigKey="ChildProductChildImagesLabel" ResourceFile="ProductResources"
                                                    ForControl="chkChildProductChildImages" CssClass="settinglabel control-label mb-0" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div role="tabpanel" class="tab-pane fade" id="tabCustomField" visible="false" runat="server">
                                    <div class="form-horizontal">
                                        <asp:Repeater ID="rptCustomFields" runat="server">
                                            <ItemTemplate>
                                                <div class="settingrow form-group row align-items-center">
                                                    <gb:SiteLabel ID="lblText" runat="server" Text='<%#Eval("Name") %>'
                                                        ForControl="txtField" CssClass="settinglabel control-label mb-0 col-sm-3" />
                                                    <div class="col-sm-9">
                                                        <asp:TextBox ID="txtField" runat="server" CssClass="form-control" />
                                                        <asp:DropDownList ID="ddlField" AppendDataBoundItems="true"
                                                            DataTextField="Name" DataValueField="CustomFieldOptionID" runat="server"
                                                            CssClass="form-control" />
                                                        <asp:CheckBoxList ID="cblField" DataTextField="Name"
                                                            DataValueField="CustomFieldOptionID" SkinID="Enum" runat="server" />
                                                        <asp:HiddenField ID="hdfCustomFieldId" Value='<%#Eval("CustomFieldId")%>'
                                                            runat="server" />
                                                        <asp:HiddenField ID="hdfDataType" Value='<%#Eval("DataType")%>'
                                                            runat="server" />
                                                        <asp:HiddenField ID="hdfOptions" Value='<%#Eval("Options")%>'
                                                            runat="server" />
                                                        <asp:Repeater ID="rptCustomFieldOptions" Visible="false" runat="server">
                                                            <ItemTemplate>
                                                                <div class="field-overridden">
                                                                    <asp:CheckBox ID="cbxField" runat="server" />
                                                                    <%# Eval("Name") %>
                                                                    <asp:HiddenField ID="hdfCustomFieldOptionId"
                                                                        Value='<%#Eval("CustomFieldOptionId")%>' runat="server" />
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnAddProductChild" CssClass="btn btn-primary" Text="Thêm sản phẩm con" ValidationGroup="productChild"
                            runat="server" />
                    </div>
                </div>
            
    </div>
</asp:Panel>
<asp:Panel CssClass="modal  fade" ID="pnlGenarationProductChild" Visible="false" runat="server" TabIndex="-1" role="dialog"
    aria-labelledby="pnlGenarationProductChild">
    <div class="modal-dialog modal-lg" role="document">
        
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title" id="pnlModalLabel2">
                            <gb:SiteLabel ID="SiteLabel1" runat="server" UseLabelTag="false" ConfigKey="GenarationProductChildLabel"
                                ResourceFile="ProductResources" />
                        </h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-horizontal">
                            <asp:Repeater ID="rptGenarationProduct" runat="server">
                                <ItemTemplate>
                                    <div class="settingrow form-group row align-items-center">
                                        <gb:SiteLabel ID="lblText" runat="server" Text='<%#Eval("Name") %>'
                                            ForControl="txtField" CssClass="settinglabel control-label mb-0 col-sm-3" />
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="txtField" runat="server" CssClass="form-control" />
                                            <asp:DropDownList ID="ddlField" AppendDataBoundItems="true"
                                                DataTextField="Name" DataValueField="CustomFieldOptionID" runat="server"
                                                CssClass="form-control" />
                                            <asp:CheckBoxList ID="cblField" DataTextField="Name"
                                                DataValueField="CustomFieldOptionID" SkinID="Enum" runat="server" />
                                            <asp:HiddenField ID="hdfCustomFieldId" Value='<%#Eval("CustomFieldId")%>'
                                                runat="server" />
                                            <asp:HiddenField ID="hdfDataType" Value='<%#Eval("DataType")%>'
                                                runat="server" />
                                            <asp:HiddenField ID="hdfOptions" Value='<%#Eval("Options")%>'
                                                runat="server" />
                                            <asp:Repeater ID="rptGenarationCustomFieldOptions" Visible="false" runat="server">
                                                <ItemTemplate>
                                                    <div class="field-overridden">
                                                        <asp:CheckBox ID="cbxField" runat="server" />
                                                        <%# Eval("Name") %>
                                                        <asp:HiddenField ID="hdfCustomFieldOptionId"
                                                            Value='<%#Eval("CustomFieldOptionId")%>' runat="server" />
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>


                            <div class="settingrow form-group row align-items-center">
                                <div class="col-sm-9">
                                    <asp:CheckBox ID="chkGenarationProductPublished" runat="server" Checked="true" />
                                    <gb:SiteLabel ID="SiteLabel2" runat="server" ConfigKey="ChildProductPublishedLabel"
                                        ResourceFile="ProductResources" ForControl="chkGenarationProductPublished"
                                        CssClass="settinglabel control-label mb-0" />
                                </div>
                            </div>
                            <div class="settingrow form-group row align-items-center">
                                <div class="col-sm-9">
                                    <asp:CheckBox ID="chkGenarationProductChildImages" runat="server" Checked="true" />
                                    <gb:SiteLabel ID="SiteLabel3" runat="server"
                                        ConfigKey="ChildProductChildImagesLabel" ResourceFile="ProductResources"
                                        ForControl="chkGenarationProductChildImages" CssClass="settinglabel control-label mb-0" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnGenarationProductChildByCustomFiled" CssClass="btn btn-primary" Text="<%$Resources:ProductResources, GenarationProductChildButton %>" ValidationGroup="productChild"
                            runat="server" />
                    </div>
                </div>
            
    </div>
</asp:Panel>

</ContentTemplate>
</asp:UpdatePanel>
