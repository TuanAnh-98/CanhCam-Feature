<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="OrderAdd.aspx.cs" Inherits="CanhCam.Web.ProductUI.OrderAddPage" %>

<%@ Register TagPrefix="Site" Assembly="CanhCam.Features.Product" Namespace="CanhCam.Web.ProductUI" %>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" SkinID="radLoadingPanelSkin"
        runat="server" Transparency="30" MinDisplayTime="500" BackColor="#E0E0E0">
        <img src='<%= RadAjaxLoadingPanel.GetWebResourceUrl(Page, "Telerik.Web.UI.Skins.Default.Ajax.loading.gif") %>' alt="Loading..." style="border: 0;" />
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="autCustomers">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="autCustomers" />
                    <telerik:AjaxUpdatedControl ControlID="txtBillingFirstName" />
                    <telerik:AjaxUpdatedControl ControlID="txtBillingLastName" />
                    <telerik:AjaxUpdatedControl ControlID="txtBillingEmail" />
                    <telerik:AjaxUpdatedControl ControlID="txtBillingPhone" />
                    <telerik:AjaxUpdatedControl ControlID="txtBillingAddress" />
                    <telerik:AjaxUpdatedControl ControlID="ddBillingProvince" />
                    <telerik:AjaxUpdatedControl ControlID="ddBillingDistrict" />
                    <telerik:AjaxUpdatedControl ControlID="divRewardPoints" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="autProducts">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="autProducts" />
                    <telerik:AjaxUpdatedControl ControlID="litOrderSubtotal" />
                    <telerik:AjaxUpdatedControl ControlID="litOrderTotal" />
                    <telerik:AjaxUpdatedControl ControlID="grid" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="chkSameAddress">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divShippingInfo" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ddShippingProvince">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ddShippingDistrict" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ddBillingProvince">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ddBillingDistrict" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ddlShippingMethod">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="litShippingFee" />
                    <telerik:AjaxUpdatedControl ControlID="ddlShippingOptions" />
                    <telerik:AjaxUpdatedControl ControlID="litOrderTotal" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grid" />
                    <telerik:AjaxUpdatedControl ControlID="litOrderSubtotal" />
                    <telerik:AjaxUpdatedControl ControlID="litOrderTotal" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <Site:ProductDisplaySettings ID="displaySettings" runat="server" />
    <portal:BreadcrumbAdmin ID="breadcrumb" runat="server"
        ParentTitle="<%$Resources:ProductResources, OrderAdminTitle %>" ParentUrl="~/Product/AdminCP/OrderList.aspx"
        CurrentPageTitle="<%$Resources:ProductResources, OrderDetailTitle %>" CurrentPageUrl="~/Product/AdminCP/OrderEdit.aspx" />
    <div class="admin-content">

        <portal:NotifyMessage ID="message" runat="server" />
        <portal:HeadingPanel ID="heading" runat="server">
            <asp:Button ID="btnUpdate" SkinID="UpdateButton" Text="<%$Resources:Resource, InsertButton %>" runat="server" CausesValidation="true" />
        </portal:HeadingPanel> 

        <div class="workplace">
            <asp:Literal runat="server" ID="litProcessBar" />
            <div class="row">
                <div class="col-md-8">
                    <div class="admin-content-bg-white">
                        <div class="mrb10">
                            <telerik:RadAutoCompleteBox ID="autProducts" TextSettings-SelectionMode="Single" Skin="Simple" runat="server" Width="100%" DropDownWidth="500" DropDownHeight="200"
                                EmptyMessage="Tìm sản phẩm">
                                <WebServiceSettings Method="GetProductNames" Path="/Product/AdminCP/OrderEdit.aspx" />
                                <ClientDropDownItemTemplate>
                                    <table cellpadding="0" cellspacing="3" width="100%">
                                        <tr>
                                            <td style="width:70%"><strong>#= Text #</strong></td>
                                            <td style="width:20%;text-align:right;">#= Attributes.Price #</td>
                                        </tr>
                                    </table>
                                </ClientDropDownItemTemplate>
                            </telerik:RadAutoCompleteBox>
                        </div>
                        <telerik:RadGrid ID="grid" SkinID="radGridSkin" runat="server">
                            <MasterTableView DataKeyNames="Guid,ProductId,AttributeDescription,AttributesXml,Quantity,DiscountAmount" AllowPaging="false" AllowSorting="false">
                                <Columns>
                                    <telerik:GridTemplateColumn Visible="true" UniqueName="Delete" HeaderStyle-Width="35">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" CommandArgument='<%#Eval("ProductId").ToString()%>'><i class="fa fa-trash"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderStyle-Width="65">
                                        <ItemTemplate>
                                            <portal:MediaElement ID="ml" runat="server" Title=" " Width="50" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Sản phẩm" UniqueName="ProductName">
                                        <ItemTemplate>
                                            <asp:Literal ID="litProductCode" runat="server" />
                                            <div>
                                                <asp:Literal ID="litProductName" runat="server" />
                                            </div>
                                            <asp:Literal ID="litAttributes" runat="server" />
                                            <div class="gifts">
                                                <%#Eval("AttributeDescription")%>
                                            </div>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="<%$Resources:ProductResources, OrderQuantityLabel %>" UniqueName="Quantity">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtQuantity" SkinID="NumericTextBox" Visible="true"
                                                MaxLength="4" Text='<%# Eval("Quantity") %>' runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtQuantity_TextChanged" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderText="<%$Resources:ProductResources, OrderPriceLabel %>" UniqueName="OrderPrice">
                                        <ItemTemplate>
                                            <%#CanhCam.Web.ProductUI.ProductHelper.FormatPrice(Convert.ToDecimal(Eval("Price")), true)%>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderText="Khuyến mãi" UniqueName="DiscountAmount">
                                        <ItemTemplate>
                                            <%#CanhCam.Web.ProductUI.ProductHelper.FormatPrice(Convert.ToDecimal(Eval("DiscountAmount")), true)%>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderText="<%$Resources:ProductResources, OrderTotalPriceLabel %>" UniqueName="OrderTotalPrice">
                                        <ItemTemplate>
                                            <%#CanhCam.Web.ProductUI.ProductHelper.FormatPrice(Convert.ToDecimal(Eval("Price")) * Convert.ToInt32(Eval("Quantity")) - Convert.ToDecimal(Eval("DiscountAmount")), true) %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </div>
                    <br />

                    <div class="row">
                        <div class="col-sm-8">
                            <div class="form-horizontal">
                                <div style="position: relative;" class="mrb10">
                                    <telerik:RadAutoCompleteBox ID="cbStaff" TextSettings-SelectionMode="Single" Skin="Simple" runat="server" Width="100%" DropDownWidth="250" DropDownHeight="200"
                                        EmptyMessage="Tìm nhân viên" OnClientEntryAdding="OnClientEntryAddingHandler">
                                        <WebServiceSettings Method="GetStaff" Path="/Product/AdminCP/OrderEdit.aspx" />

                                        <ClientDropDownItemTemplate>
                                            <table cellpadding="0" cellspacing="3" width="100%">
                                                <tr>
                                                    <td style="width:70%"><strong>#= Text #</strong></td>
                                                </tr>
                                            </table>
                                        </ClientDropDownItemTemplate>
                                    </telerik:RadAutoCompleteBox>
                                </div>
                                <div class="settingrow form-group">
                                    <gb:SiteLabel ID="lblOrderCode" runat="server" CssClass="settinglabel control-label mb-0 text-left text-strong col-xs-12 col-sm-12 col-md-12 col-lg-3" ConfigKey="OrderCodeLabel" ResourceFile="ProductResources" />
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-9">
                                        <p class="form-control-static mb-0 text-strong">
                                            <asp:Literal ID="litOrderCode" runat="server" />
                                        </p>
                                    </div>
                                </div>
                                <%--Order Source--%>
                                <div class="settingrow form-group">
                                    <gb:SiteLabel ID="SiteLabel1" runat="server" CssClass="settinglabel control-label mb-0 text-left col-xs-12 col-sm-12 col-md-12 col-lg-3"
                                        ConfigKey="OrderSource" ResourceFile="ProductResources" />
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-9">
                                        <p class="form-control-static mb-0">
                                            <asp:DropDownList ID="ddlSource" runat="server" CssClass="form-control" />
                                        </p>
                                    </div>
                                </div>
                                <div class="settingrow form-group">
                                    <gb:SiteLabel ID="SiteLabel4" runat="server" CssClass="settinglabel control-label mb-0 text-left col-xs-12 col-sm-12 col-md-12 col-lg-3"
                                        Text="Tình trạng gửi ERP" />
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-9">
                                        <p class="form-control-static mb-0">
                                            <asp:DropDownList ID="ddlERPStatus" runat="server" CssClass="form-control">
                                                <asp:ListItem Value="0">Chưa gửi</asp:ListItem>
                                                <asp:ListItem Value="10">Đã gửi</asp:ListItem>
                                            </asp:DropDownList>
                                        </p>
                                    </div>
                                </div>
                                <%--Order Status--%>
                                <div class="settingrow form-group">
                                    <gb:SiteLabel ID="lblOrderStatus" runat="server" CssClass="settinglabel control-label mb-0 text-left col-xs-12 col-sm-12 col-md-12 col-lg-3" ConfigKey="OrderStatusLabel" ResourceFile="ProductResources" />
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-9">
                                        <p class="form-control-static mb-0">
                                            <asp:DropDownList ID="ddOrderStatus" runat="server" CssClass="form-control" />
                                            <asp:Label ID="litOrderStatus" Visible="false" runat="server" />
                                        </p>
                                    </div>
                                </div>
                                <%--Shipping Method--%>
                                <div class="settingrow form-group" id="divShippingMethod" runat="server">
                                    <gb:SiteLabel ID="lblShippingMethod" runat="server" CssClass="settinglabel control-label mb-0 text-left col-xs-12 col-sm-12 col-md-12 col-lg-3" ConfigKey="ShippingMethodLabel" ResourceFile="ProductResources" />
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-9">
                                        <p class="form-control-static mb-0">
                                            <asp:DropDownList ID="ddlShippingMethod" runat="server" DataTextField="Name"
                                                DataValueField="ShippingMethodId" CssClass="form-control" AutoPostBack="true" />
                                        </p>
                                    </div>
                                </div>
                                <div class="settingrow form-group" id="divShippinggOptions" runat="server">
                                    <gb:SiteLabel ID="SiteLabel3" runat="server"
                                        CssClass="settinglabel control-label mb-0 text-left col-xs-12 col-sm-12 col-md-12 col-lg-3"
                                        Text="Dịch vụ vận chuyển" />
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-9">
                                        <p class="form-control-static mb-0">
                                            <asp:DropDownList ID="ddlShippingOptions" runat="server"
                                                CssClass="form-control" AutoPostBack="true" AppendDataBoundItems="true" />
                                        </p>
                                    </div>
                                </div>
                                <div class="settingrow form-group">
                                    <gb:SiteLabel ID="SiteLabel6" runat="server"
                                        CssClass="settinglabel control-label mb-0 text-left col-xs-12 col-sm-12 col-md-12 col-lg-3"
                                        Text="Trạng Thái Giao Vận" />
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-9">
                                        <p class="form-control-static mb-0">
                                            <asp:DropDownList ID="ddlShippingStatus" runat="server"
                                                CssClass="form-control">
                                                <asp:ListItem Value="0">None</asp:ListItem>
                                                <asp:ListItem Value="10">Đã gửi giao vận</asp:ListItem>
                                                <asp:ListItem Value="20">Đã giao hàng</asp:ListItem>
                                            </asp:DropDownList>
                                        </p>
                                    </div>
                                </div>
                                <%--Payment Method--%>
                                <div class="settingrow form-group" id="divPaymentMethod" runat="server">
                                    <gb:SiteLabel ID="lblPaymentMethod" runat="server" CssClass="settinglabel control-label mb-0 text-left col-xs-12 col-sm-12 col-md-12 col-lg-3" ConfigKey="PaymentMethodLabel" ResourceFile="ProductResources" />
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-9">
                                        <p class="form-control-static mb-0">
                                            <asp:DropDownList ID="ddlPaymentMethod" runat="server" DataTextField="Name" DataValueField="PaymentMethodId" CssClass="form-control" />
                                        </p>
                                    </div>
                                </div>
                                <%--Payment Status--%>
                                <div class="settingrow form-group">
                                    <gb:SiteLabel ID="lblPaymentStatus" runat="server" CssClass="settinglabel control-label mb-0 text-left col-xs-12 col-sm-12 col-md-12 col-lg-3" ConfigKey="PaymentStatusLabel" ResourceFile="ProductResources" />
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-9">
                                        <asp:DropDownList ID="ddlPaymentStatus" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="Đã thanh toán" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Pending" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Chưa thanh toán" Value="3"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <%--Order Note--%>
                                <div class="settingrow form-group">
                                    <gb:SiteLabel ID="lblOrderNote" runat="server" CssClass="settinglabel control-label mb-0 text-left col-xs-12 col-sm-12 col-md-12 col-lg-3" ConfigKey="OrderNoteLabel" ResourceFile="ProductResources" />
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-9">
                                        <asp:TextBox ID="txtOrderNote" TextMode="MultiLine" Style="min-height: 50px" runat="server" CssClass="form-control" />
                                    </div>
                                </div>
                                <%--Order Old Store--%>
                                <div class="settingrow form-group" id="divOrderOldStore" runat="server">
                                    <gb:SiteLabel ID="lblOrderOldStore" runat="server" CssClass="settinglabel control-label mb-0 text-left col-xs-12 col-sm-12 col-md-12 col-lg-3" ConfigKey="OrderOldStoreLabel" ResourceFile="ProductResources" />
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-9">
                                        <p class="form-control-static mb-0 text-strong">
                                            <asp:Literal ID="litOrderOldStore" runat="server" />
                                        </p>
                                    </div>
                                </div>
                                <%--Order Store--%>
                                <div class="settingrow form-group" id="divOrderStore" runat="server">
                                    <gb:SiteLabel ID="lblOrderStore" runat="server" CssClass="settinglabel control-label mb-0 text-left col-xs-12 col-sm-12 col-md-12 col-lg-3" ConfigKey="OrderStoreLabel" ResourceFile="StoreResources" />
                                    <gb:SiteLabel ID="lblOrderNewStore" runat="server" CssClass="settinglabel control-label mb-0 text-left col-xs-12 col-sm-12 col-md-12 col-lg-3" ConfigKey="OrderNewStoreLabel" ResourceFile="ProductResources" Visible="false" />
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-9">
                                        <p class="form-control-static mb-0">
                                            <asp:Literal ID="litOrderStore" runat="server" />
                                            <asp:DropDownList ID="ddStore" runat="server" DataTextField="Name" DataValueField="StoreID" AppendDataBoundItems="true" CssClass="form-control" Visible="False" />

                                        </p>
                                    </div>
                                </div>
                                <div id="divOrderGift" runat="server" visible="false" class="settingrow form-group">
                                    <gb:SiteLabel ID="lblOrderGift" runat="server" CssClass="settinglabel control-label mb-0 text-left col-xs-12 col-sm-12 col-md-12 col-lg-3" Text="Quà tặng" ResourceFile="ProductResources" />
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-9">
                                        <div class="form-control-static mb-0 gifts">
                                            <asp:Literal ID="litOrderGift" runat="server" />
                                        </div>
                                    </div>
                                </div>
                                <div id="divDiscounts" runat="server" visible="false" class="settingrow form-group">
                                    <gb:SiteLabel ID="lblDiscounts" runat="server" CssClass="settinglabel control-label mb-0 text-left col-xs-12 col-sm-12 col-md-12 col-lg-3" Text="Chương trình KM" ResourceFile="ProductResources" />
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-9">
                                        <p class="form-control-static mb-0">
                                            <asp:Literal ID="litDiscounts" runat="server" />
                                        </p>
                                    </div>
                                </div>

                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-horizontal">
                                <%--Order Subtotal--%>
                                <div class="settingrow form-group">
                                    <gb:SiteLabel ID="lblOrderSubtotal" runat="server" CssClass="settinglabel mb-0 col-xs-12 col-sm-12 col-md-12 col-lg-6" ConfigKey="OrderSubTotalLabel" ResourceFile="ProductResources" />
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-6 text-right">
                                        <p class="form-control-static mb-0">
                                            <asp:Literal ID="litOrderSubtotal" runat="server" />
                                        </p>
                                    </div>
                                </div>
                                <%--Order Discount--%>
                                <div id="divOrderDiscount" runat="server" visible="false" class="settingrow form-group">
                                    <gb:SiteLabel ID="lblOrderDiscount" runat="server" CssClass="settinglabel control-label mb-0 col-xs-12 col-sm-12 col-md-12 col-lg-6" ConfigKey="OrderDiscountLabel" ResourceFile="ProductResources" />
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-6 text-right">
                                        <p class="form-control-static mb-0">
                                            <asp:Literal ID="litOrderDiscount" runat="server" />
                                        </p>
                                    </div>
                                </div>
                                <div id="divDiscountItems" runat="server" class="settingrow form-group">
                                    <gb:SiteLabel ID="lblDiscountItems" runat="server" CssClass="settinglabel control-label mb-0 col-xs-12 col-sm-12 col-md-12 col-lg-6" Text="Giảm giá tiền hàng" ResourceFile="ProductResources" />
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-6 text-right">
                                        <p class="form-control-static mb-0">
                                            <asp:Literal ID="litDiscountItems" runat="server" />
                                        </p>
                                    </div>
                                </div>
                                <div id="divDiscountOrder" runat="server" class="settingrow form-group">
                                    <gb:SiteLabel ID="lblDiscountOrder" runat="server" CssClass="settinglabel control-label mb-0 col-xs-12 col-sm-12 col-md-12 col-lg-6" Text="Giảm giá hóa đơn" ResourceFile="ProductResources" />
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-6 text-right">
                                        <p class="form-control-static mb-0">
                                            <asp:Literal ID="litDiscountOrder" runat="server" />
                                        </p>
                                    </div>
                                </div>
                                <%--Shipping Fee--%>
                                <div class="settingrow form-group">
                                    <gb:SiteLabel ID="lblShippingFee" runat="server" CssClass="settinglabel control-label mb-0 col-xs-12 col-sm-12 col-md-12 col-lg-6" ConfigKey="OrderShippingFeeLabel" ResourceFile="ProductResources" />
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-6 text-right">
                                        <p class="form-control-static mb-0">
                                            <asp:Literal ID="litShippingFee" runat="server" />
                                        </p>
                                    </div>
                                </div>

                                <asp:Panel ID="pnCouponDiscount" runat="server" Visible="false">
                                    <div class="settingrow form-group">
                                        <gb:SiteLabel ID="SiteLabel5" runat="server"
                                            CssClass="settinglabel control-label mb-0 col-xs-12 col-sm-12 col-md-12 col-lg-6"
                                            Text="Coupon " />
                                        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-6 text-right">
                                            <p class="form-control-static mb-0">
                                                <asp:Literal ID="litCouponDiscount" runat="server" />
                                            </p>
                                        </div>
                                    </div>
                                </asp:Panel>    
                                <div class="settingrow form-group">
                                    <gb:SiteLabel ID="SiteLabel8" runat="server"
                                        CssClass="settinglabel control-label mb-0 col-xs-12 col-sm-12 col-md-12 col-lg-6"
                                        Text="Phiếu thanh toán (Vouchers)" />
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-6 text-right">
                                        <p class="form-control-static mb-0">
                                            <asp:Literal ID="litVoucherAmount" runat="server" />
                                        </p>
                                        <p class="form-control-static mb-0">
                                            <asp:Literal runat="server" ID="litVoucherCodes" />
                                        </p>
                                    </div>
                                </div>
                                <%--Order Total--%>
                                <div class="settingrow form-group">
                                    <gb:SiteLabel ID="lblOrderTotal" runat="server" CssClass="settinglabel control-label mb-0 col-xs-12 col-sm-12 col-md-12 col-lg-6" ConfigKey="OrderTotalLabel" ResourceFile="ProductResources" />
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-6 text-right">
                                        <p class="form-control-static mb-0">
                                            <asp:Literal ID="litOrderTotal" runat="server" />
                                        </p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4 admin-content-bg-white">
                    <div style="position: relative;" class="mrb10">
                        <telerik:RadAutoCompleteBox ID="autCustomers" TextSettings-SelectionMode="Single" Skin="Simple" runat="server" Width="100%" DropDownWidth="250" DropDownHeight="200"
                            EmptyMessage="Tìm khách hàng" OnClientEntryAdding="OnClientEntryAddingHandler">
                            <WebServiceSettings Method="GetUserNames" Path="/Product/AdminCP/OrderEdit.aspx" />
                            <ClientDropDownItemTemplate>
                                <table cellpadding="0" cellspacing="3" width="100%">
                                    <tr>
                                        <td style="width:70%"><strong>#= Text #</strong></td>
                                        <%--<td style="text-align:right;">#= Attributes.FirstName #</td>--%>
                                    </tr>
                                </table>
                            </ClientDropDownItemTemplate>
                        </telerik:RadAutoCompleteBox>
                    </div>
                    <%--<h4 class="order-title" id="formOrderUser" runat="server">
                        <gb:SiteLabel ID="lblCustomerInfo" runat="server" ConfigKey="OrderCustomerInfo" ResourceFile="ProductResources" UseLabelTag="false" />
                    </h4>--%>
                    <div class="form-horizontal">
                        <div class="settingrow form-group">
                            <gb:SiteLabel ID="lblBillingFirstName" runat="server" CssClass="settinglabel control-label col-sm-4 mb-0" ConfigKey="OrderFirstNameLabel" ResourceFile="ProductResources" />
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtBillingFirstName" runat="server" CssClass="form-control" />
                                <asp:RequiredFieldValidator ID="reqFirstName" ErrorMessage="Vui lòng nhập Họ tên" Display="Dynamic"
                                    ControlToValidate="txtBillingFirstName" runat="server" ValidationGroup="OrderEdit" SkinID="UserAddress" />
                                <asp:Literal ID="litBillingFirstName" Visible="false" runat="server" />
                            </div>
                        </div>
                        <div class="settingrow form-group">
                            <gb:SiteLabel ID="lblBillingLastName" runat="server" CssClass="settinglabel control-label col-sm-4 mb-0" ConfigKey="OrderLastNameLabel" ResourceFile="ProductResources" />
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtBillingLastName" runat="server" CssClass="form-control" />
                                <asp:Literal ID="litBillingLastName" Visible="false" runat="server" />
                            </div>
                        </div>
                        <div class="settingrow form-group">
                            <gb:SiteLabel ID="lblBillingEmail" runat="server" CssClass="settinglabel control-label col-sm-4 mb-0" ConfigKey="OrderEmailLabel" ResourceFile="ProductResources" />
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtBillingEmail" runat="server" CssClass="form-control" />
                                <asp:Literal ID="litBillingEmail" Visible="false" runat="server" />
                            </div>
                        </div>
                        <div class="settingrow form-group">
                            <gb:SiteLabel ID="lblBillingPhone" runat="server" CssClass="settinglabel control-label col-sm-4 mb-0" ConfigKey="OrderPhoneLabel" ResourceFile="ProductResources" />
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtBillingPhone" runat="server" CssClass="form-control" />
                                <asp:Literal ID="litBillingPhone" Visible="false" runat="server" />
                            </div>
                        </div>
                        <div class="settingrow form-group">
                            <gb:SiteLabel ID="lblBillingAddress" runat="server" CssClass="settinglabel control-label col-sm-4 mb-0" ConfigKey="OrderAddressLabel" ResourceFile="ProductResources" />
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtBillingAddress" runat="server" CssClass="form-control" />
                                <asp:Literal ID="litBillingAddress" Visible="false" runat="server" />
                            </div>
                        </div>
                        <div class="settingrow form-group">
                            <gb:SiteLabel ID="lblBillingProvince" runat="server" CssClass="settinglabel control-label col-sm-4 mb-0" ConfigKey="OrderProvinceLabel" ResourceFile="ProductResources" />
                            <div class="col-sm-8">
                                <div class="row">
                                    <div class="col-sm-6">
                                        <asp:DropDownList ID="ddBillingProvince" AutoPostBack="true" AppendDataBoundItems="true" DataValueField="Guid" DataTextField="Name" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="<%$Resources:ProductResources, OrderSelectLabel %>" Value=""></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Literal ID="litBillingProvince" Visible="false" runat="server" />
                                    </div>
                                    <div class="col-sm-6">
                                        <asp:DropDownList ID="ddBillingDistrict" DataValueField="Guid" DataTextField="Name" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="<%$Resources:ProductResources, OrderSelectLabel %>" Value=""></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Literal ID="litBillingDistrict" Visible="false" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--Shipping--%>
                    <div class="form-horizontal">
                        <div class="settingrow form-group">
                            <div class="col-sm-4"></div>
                            <div class="col-sm-8">
                                <asp:CheckBox ID="chkSameAddress" runat="server" Text="Giao hàng đến địa chỉ khác" AutoPostBack="true" />
                            </div>
                        </div>
                    </div>
                    <div id="divShippingInfo" runat="server" visible="false" class="form-horizontal">
                        <div class="settingrow form-group">
                            <gb:SiteLabel ID="lblShippingFirstName" runat="server" CssClass="settinglabel control-label col-sm-4 mb-0" ConfigKey="OrderFirstNameLabel" ResourceFile="ProductResources" />
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtShippingFirstName" runat="server" CssClass="form-control" />
                                <asp:Literal ID="litShippingFirstName" Visible="false" runat="server" />
                            </div>
                        </div>
                        <div class="settingrow form-group">
                            <gb:SiteLabel ID="lblShippingLastName" runat="server" CssClass="settinglabel control-label col-sm-4 mb-0" ConfigKey="OrderLastNameLabel" ResourceFile="ProductResources" />
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtShippingLastName" runat="server" CssClass="form-control" />
                                <asp:Literal ID="litShippingLastName" Visible="false" runat="server" />
                            </div>
                        </div>
                        <div class="settingrow form-group">
                            <gb:SiteLabel ID="lblShippingEmail" runat="server" CssClass="settinglabel control-label col-sm-4 mb-0" ConfigKey="OrderEmailLabel" ResourceFile="ProductResources" />
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtShippingEmail" runat="server" CssClass="form-control" />
                                <asp:Literal ID="litShippingEmail" Visible="false" runat="server" />
                            </div>
                        </div>
                        <div class="settingrow form-group">
                            <gb:SiteLabel ID="lblShippingPhone" runat="server" CssClass="settinglabel control-label col-sm-4 mb-0" ConfigKey="OrderPhoneLabel" ResourceFile="ProductResources" />
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtShippingPhone" runat="server" CssClass="form-control" />
                                <asp:Literal ID="litShippingPhone" Visible="false" runat="server" />
                            </div>
                        </div>
                        <div class="settingrow form-group">
                            <gb:SiteLabel ID="lblShippingAddress" runat="server" CssClass="settinglabel control-label col-sm-4 mb-0" ConfigKey="OrderAddressLabel" ResourceFile="ProductResources" />
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtShippingAddress" runat="server" CssClass="form-control" />
                                <asp:Literal ID="litShippingAddress" Visible="false" runat="server" />
                            </div>
                        </div>
                        <div class="settingrow form-group">
                            <gb:SiteLabel ID="lblShippingProvince" runat="server" CssClass="settinglabel control-label col-sm-4 mb-0" ConfigKey="OrderProvinceLabel" ResourceFile="ProductResources" />
                            <div class="col-sm-8">
                                <div class="row">
                                    <div class="col-sm-6">
                                        <asp:DropDownList ID="ddShippingProvince" AutoPostBack="true" AppendDataBoundItems="true" DataValueField="Guid" DataTextField="Name" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="<%$Resources:ProductResources, OrderSelectLabel %>" Value=""></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Literal ID="litShippingProvince" Visible="false" runat="server" />
                                    </div>
                                    <div class="col-sm-6">
                                        <asp:DropDownList ID="ddShippingDistrict" AutoPostBack="true" DataValueField="Guid" DataTextField="Name" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="<%$Resources:ProductResources, OrderSelectLabel %>" Value=""></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Literal ID="litShippingDistrict" Visible="false" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--Invoice--%>
                    <h4 class="order-title" id="formInvoice" runat="server">
                        <gb:SiteLabel ID="lblInvoiceInfo" runat="server" ConfigKey="InvoiceInfoLabel" ResourceFile="ProductResources" UseLabelTag="false" />
                    </h4>
                    <div id="divInvoice" runat="server" class="form-horizontal">
                        <div class="settingrow form-group">
                            <gb:SiteLabel ID="lblInvoiceCompanyName" runat="server" CssClass="settinglabel control-label col-sm-4 mb-0" ConfigKey="CheckoutCompanyName" ResourceFile="ProductResources" />
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtInvoiceCompanyName" runat="server" CssClass="form-control" />
                                <asp:Literal ID="litInvoiceCompanyName" Visible="false" runat="server" />
                            </div>
                        </div>
                        <div class="settingrow form-group">
                            <gb:SiteLabel ID="lblInvoiceCompanyAddress" runat="server" CssClass="settinglabel control-label col-sm-4 mb-0" ConfigKey="CheckoutCompanyAddress" ResourceFile="ProductResources" />
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtInvoiceCompanyAddress" runat="server" CssClass="form-control" />
                                <asp:Literal ID="litInvoiceCompanyAddress" Visible="false" runat="server" />
                            </div>
                        </div>
                        <div class="settingrow form-group">
                            <gb:SiteLabel ID="lblInvoiceCompanyTaxCode" runat="server" CssClass="settinglabel control-label col-sm-4 mb-0" ConfigKey="CheckoutCompanyTaxCode" ResourceFile="ProductResources" />
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtInvoiceCompanyTaxCode" runat="server" CssClass="form-control" />
                                <asp:Literal ID="litInvoiceCompanyTaxCode" Visible="false" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>
    <style type="text/css">
        .attributes {
            font-size: 12px;
            font-style: italic
        }

        .RadComboBox, .RadComboBoxDropDown {
            font-family: "Helvetica Neue",Helvetica,Arial,sans-serif;
            font-size: 13px
        }

            .RadComboBox .rcbActionButton {
                width: 34px;
                height: 34px
            }

            .RadComboBox .rcbInner {
                height: auto;
                padding: 8px 22px 8px 5px
            }

            .RadComboBox .rcbInput, .RadComboBox .rcbInput:focus {
                box-shadow: none
            }

        .text-strong {
            font-weight: 800 !important
        }

        .gifts {
            font-style: italic;
            font-size: 12px
        }

            .gifts ul {
                margin: 0;
                padding: 0
            }

                .gifts ul li {
                    list-style: none
                }

            .gifts img {
                max-width: 20px
            }
    </style>
    <script>
        function OnClientEntryAddingHandler(sender, eventArgs) {
            if (sender.get_entries().get_count() > 0) {
                eventArgs.set_cancel(true);
            }
        }
    </script>
    <script type="text/javascript">
        function getUrlParameter(sParam) {
            var sPageURL = decodeURIComponent(window.location.search.substring(1)),
                sURLVariables = sPageURL.split('&'),
                sParameterName,
                i;

            for (i = 0; i < sURLVariables.length; i++) {
                sParameterName = sURLVariables[i].split('=');

                if (sParameterName[0] === sParam) {
                    return sParameterName[1] === undefined ? true : sParameterName[1];
                }
            }
        }

        function printOrder() {
            var orderId;
            var sPageURL = decodeURIComponent(window.location.search.substring(1)),
                sURLVariables = sPageURL.split('&'),
                sParameterName,
                i;

            for (i = 0; i < sURLVariables.length; i++) {
                sParameterName = sURLVariables[i].split('=');

                if (sParameterName[0] === "OrderID") {
                    orderId = sParameterName[1] === undefined ? true : sParameterName[1];
                }
            }
            //var orderId = getUrlParameter("OrderID");
            $.ajax({
                url: "/Product/Services/OrderPrint.ashx",
                type: "GET",
                dataType: "html",
                data: {
                    id: orderId
                },
                cache: !0,
                success: function (data) {
                    var oldstr = document.body.innerHTML;
                    document.body.innerHTML = "<html><head><title></title></head><body>" + data + "</body>";
                    window.print();
                    document.body.innerHTML = oldstr;
                }
            })

            return false;
        }
    </script>
</asp:Content>
