<%@ Page Language="c#" CodeBehind="DiscountEdit.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master"
    AutoEventWireup="false" Inherits="CanhCam.Web.ProductUI.DiscountEditPage" %>

<%@ Import Namespace="CanhCam.Web.ProductUI" %>
<%@ Register TagPrefix="Site" Assembly="CanhCam.Features.Product" Namespace="CanhCam.Web.ProductUI" %>

<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <Site:PromotionDisplaySettings ID="displaySettings" runat="server" />
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <%--<telerik:AjaxSetting AjaxControlID="ddlAppliedAllProducts">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divAppliedToProducts" />
                    <telerik:AjaxUpdatedControl ControlID="divAppliedToCategories" />
                </UpdatedControls>
            </telerik:AjaxSetting>--%>
            <telerik:AjaxSetting AjaxControlID="btnOrderRangeNewRow">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridOrderDiscountRange" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="gridOrderDiscountRange">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridOrderDiscountRange" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnSearch2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grid2" />
                    <telerik:AjaxUpdatedControl ControlID="gridRelated2" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnAdd2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridRelated2" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnRemove2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridRelated2" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <%--<telerik:AjaxSetting AjaxControlID="btnSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grid" />
                    <telerik:AjaxUpdatedControl ControlID="gridRelated" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnAdd">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridRelated" />
                    <telerik:AjaxUpdatedControl ControlID="divExcludedZones" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnRemove">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridRelated" />
                    <telerik:AjaxUpdatedControl ControlID="divExcludedZones" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnAll">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridRelated" />
                    <telerik:AjaxUpdatedControl ControlID="divExcludedZones" />
                </UpdatedControls>
            </telerik:AjaxSetting>--%>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <portal:BreadcrumbAdmin ID="breadcrumb" runat="server"
        ParentTitle="<%$Resources:ProductResources, DiscountPageTitle %>" ParentUrl="~/Product/AdminCP/Promotions/Discounts.aspx"
        CurrentPageTitle="<%$Resources:ProductResources, DiscountEditTitle %>" CurrentPageUrl="~/Product/AdminCP/Promotions/DiscountEdit.aspx" />
    <div class="admin-content col-md-12">
        <portal:HeadingPanel ID="heading" runat="server">
            <asp:Button SkinID="UpdateButton" ID="btnUpdate" ValidationGroup="DiscountEdit" Text="<%$Resources:Resource, UpdateButton %>" runat="server" />
            <asp:Button SkinID="UpdateButton" ID="btnUpdateAndNew" ValidationGroup="DiscountEdit" Text="<%$Resources:Resource, UpdateAndNewButton %>" runat="server" />
            <asp:Button SkinID="UpdateButton" ID="btnUpdateAndClose" ValidationGroup="DiscountEdit" Text="<%$Resources:Resource, UpdateAndCloseButton %>" runat="server" />
            <asp:Button SkinID="InsertButton" ID="btnInsert" ValidationGroup="DiscountEdit" Text="<%$Resources:Resource, InsertButton %>" runat="server" />
            <asp:Button SkinID="InsertButton" ID="btnInsertAndNew" ValidationGroup="DiscountEdit" Text="<%$Resources:Resource, InsertAndNewButton %>" runat="server" />
            <asp:Button SkinID="InsertButton" ID="btnInsertAndClose" ValidationGroup="DiscountEdit" Text="<%$Resources:Resource, InsertAndCloseButton %>" runat="server" />
            <asp:Button SkinID="DeleteButton" ID="btnDelete" runat="server" Text="<%$Resources:Resource, DeleteButton %>" CausesValidation="false" />
        </portal:HeadingPanel>
        <portal:NotifyMessage ID="message" runat="server" />
        <div class="workplace">
            <div id="divtabs" runat="server" class="tabs">
                <ul id="ulTabs" class="nav nav-tabs" runat="server">
                    <li role="presentation">
                        <asp:Literal ID="litTabContent" runat="server" /></li>
                    <li role="presentation" id="liTabCoupon" runat="server" visible="false">
                        <asp:Literal ID="litTabCoupon" runat="server" /></li>
                    <li role="presentation">
                        <asp:Literal ID="litTabLandingPage" runat="server" /></li>
                    <li role="presentation">
                        <asp:Literal ID="litTabHistory" runat="server" /></li>
                </ul>
                <div class="tab-content">
                    <div role="tabpanel" class="tab-pane fade active in" id="tabContent">
                        <div class="row">
                            <div class="col-sm-6">
                                <div id="divDiscountType" runat="server" class="settingrow form-group">
                                    <gb:SiteLabel ID="lblDiscountType" runat="server" ForControl="ddlDiscountType" ShowRequired="true" CssClass="settinglabel control-label col-sm-3" Text="Loại khuyến mãi" />
                                    <div class="col-sm-9">
                                        <asp:DropDownList ID="ddlDiscountType" runat="server" />
                                    </div>
                                </div>
                                <div class="settingrow form-group">
                                    <gb:SiteLabel ID="lblName" runat="server" ForControl="txtName" ShowRequired="true" CssClass="settinglabel control-label col-sm-3" Text="Tên chương trình" />
                                    <div class="col-sm-9">
                                        <asp:TextBox ID="txtName" MaxLength="255" runat="server" />
                                        <asp:RequiredFieldValidator ID="reqName" runat="server" ControlToValidate="txtName"
                                            ErrorMessage="<%$Resources:ProductResources, DiscountNameRequiredWarning %>"
                                            Display="Dynamic" SetFocusOnError="true" CssClass="txterror" ValidationGroup="DiscountEdit" />
                                    </div>
                                </div>
                                <div class="settingrow form-group">
                                    <gb:SiteLabel ID="lblCode" runat="server" ForControl="txtCode" CssClass="settinglabel control-label col-sm-3" Text="Mã chương trình" />
                                    <div class="col-sm-9">
                                        <asp:TextBox ID="txtCode" MaxLength="255" runat="server" />
                                    </div>
                                </div>
                                <div id="divPriority" runat="server" class="settingrow form-group">
                                    <gb:SiteLabel ID="lblPriority" runat="server" ForControl="ddlPriority" ShowRequired="true" CssClass="settinglabel control-label col-sm-3" Text="Độ ưu tiên" />
                                    <div class="col-sm-9">
                                        <div class="input-group">
                                            <asp:DropDownList ID="ddlPriority" runat="server">
                                                <asp:ListItem Text="Số 0 (Thấp nhất)" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Số 1" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Số 2" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Số 3" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="Số 4" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="Số 5" Value="5"></asp:ListItem>
                                                <asp:ListItem Text="Số 6" Value="6"></asp:ListItem>
                                                <asp:ListItem Text="Số 7" Value="7"></asp:ListItem>
                                                <asp:ListItem Text="Số 8" Value="8"></asp:ListItem>
                                                <asp:ListItem Text="Số 9" Value="9"></asp:ListItem>
                                                <asp:ListItem Text="Số 10 (Cao nhất)" Value="10"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div id="divDiscountAmount" class="settingrow form-group" runat="server" visible="false">
                                    <gb:SiteLabel ID="lblDiscount" runat="server" ForControl="txtDiscountAmount" ShowRequired="true" CssClass="settinglabel control-label col-sm-3" Text="Giảm giá đơn hàng" />
                                    <div class="col-sm-9">
                                        <asp:TextBox ID="txtDiscountAmount" SkinID="PriceTextBox" Style="display: inline-block" MaxLength="20" runat="server" />
                                        <asp:DropDownList ID="ddlUsePercentage" Style="width: auto; display: inline-block" runat="server">
                                            <asp:ListItem Text="%" Value="true"></asp:ListItem>
                                            <asp:ListItem Text="VND" Value="false"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div id="divMaximumDiscount" visible="false" runat="server" class="settingrow form-group">
                                    <gb:SiteLabel ID="lblMaximumDiscount" runat="server" ForControl="txtMaximumDiscount" CssClass="settinglabel control-label col-sm-3" Text="Tiền giảm tối đa" />
                                    <div class="col-sm-9">
                                        <asp:TextBox ID="txtMaximumDiscount" SkinID="PriceTextBox" MaxLength="20" runat="server" />
                                    </div>
                                </div>
                                <div id="divPaymentMethod" runat="server" visible="false" class="settingrow form-group">
                                    <gb:SiteLabel ID="lblPayment" runat="server" ForControl="cobPaymentMethod" ShowRequired="false" CssClass="settinglabel control-label col-sm-3" Text="Cổng thanh toán" />
                                    <div class="col-sm-9">
                                        <portal:ComboBox ID="cobPaymentMethod" SelectionMode="Multiple" runat="server" DataTextField="Name" DataValueField="PaymentMethodId" CssClass="form-control" />
                                    </div>
                                </div>
                                <%--<div id="divAlwaysOnDisplay" runat="server" visible="false" class="settingrow form-group">
                                <gb:SiteLabel id="lblAlwaysOnDisplay" runat="server" ForControl="chkAlwaysOnDisplay" ShowRequired="false" CssClass="settinglabel control-label col-sm-3" Text="Luôn luôn hiển thị" />
                                <div class="col-sm-9">
                                    <asp:CheckBox ID="chkAlwaysOnDisplay" runat="server" />
                                </div>
                            </div>--%>
                                <div id="divShowOption" visible="false" runat="server" class="settingrow form-group">
                                    <gb:SiteLabel ID="lblShowOption" runat="server" ForControl="chlShowOption" ConfigKey="ShowOptionLabel"
                                        ResourceFile="ProductResources" CssClass="settinglabel control-label col-sm-3" />
                                    <div class="col-sm-9">
                                        <asp:CheckBoxList ID="chlShowOption" SkinID="Enum" runat="server" />
                                    </div>
                                </div>
                                <%--<div id="divAppliedAllProducts" runat="server" visible="false" class="settingrow form-group">
                                <gb:SiteLabel id="lblAppliedAllProducts" runat="server" ForControl="ddlAppliedAllProducts" ShowRequired="false" CssClass="settinglabel control-label col-sm-3" Text="Áp dụng cho" />
                                <div class="col-sm-9">
                                    <asp:DropDownList ID="ddlAppliedAllProducts" AutoPostBack="true" runat="server">
                                        <asp:ListItem Text="Tất cả sản phẩm" Value="true"></asp:ListItem>
                                        <asp:ListItem Text="Danh mục/Sản phẩm được chọn" Value="false" Selected="True"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>--%>
                            </div>
                            <div class="col-sm-6">
                                <div class="settingrow form-group">
                                    <gb:SiteLabel ID="lblActiveStatus" runat="server" ForControl="ddActiveStatus" CssClass="settinglabel control-label col-sm-3" ConfigKey="CouponActiveStatusLabel" ResourceFile="ProductResources" />
                                    <div class="col-sm-9">
                                        <asp:DropDownList ID="ddActiveStatus" runat="server">
                                            <asp:ListItem Text="<%$Resources:ProductResources, CouponActiveLabel %>" Value="true"></asp:ListItem>
                                            <asp:ListItem Text="<%$Resources:ProductResources, CouponInactiveLabel %>" Value="false"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="settingrow form-group">
                                    <gb:SiteLabel ID="lblFromDate" runat="server" ForControl="dpStartDate" ShowRequired="true" CssClass="settinglabel control-label col-sm-3" ConfigKey="DiscountStartDateLabel" ResourceFile="ProductResources" />
                                    <div class="col-sm-9">
                                        <telerik:RadDateTimePicker ID="dpStartDate" Width="250" Height="30" Culture="en-US" DateInput-DateFormat="dd/MM/yyyy HH:mm" TimeView-TimeFormat="HH:mm" runat="server" Skin="Simple" />
                                        <asp:RequiredFieldValidator ID="reqStartDate" SkinID="Discount" ForeColor="Red" runat="server" ControlToValidate="dpStartDate"
                                            ErrorMessage="<%$Resources:ProductResources, DiscountStartDateRequiredWarning %>"
                                            Display="Dynamic" SetFocusOnError="true" ValidationGroup="DiscountEdit" />
                                    </div>
                                </div>
                                <div class="settingrow form-group">
                                    <gb:SiteLabel ID="lblExpiryDate" runat="server" ForControl="dpEndDate" ShowRequired="false" CssClass="settinglabel control-label col-sm-3" ConfigKey="DiscountEndDateLabel" ResourceFile="ProductResources" />
                                    <div class="col-sm-9">
                                        <telerik:RadDateTimePicker ID="dpEndDate" Width="250" Height="30" Culture="en-US" DateInput-DateFormat="dd/MM/yyyy HH:mm" TimeView-TimeFormat="HH:mm" runat="server" Skin="Simple" />
                                        <%--<asp:RequiredFieldValidator ID="reqEndDate" SkinID="Discount" ForeColor="Red" runat="server" ControlToValidate="dpEndDate"
                                        ErrorMessage="<%$Resources:ProductResources, DiscountEndDateRequiredWarning %>"
                                        Display="Dynamic" SetFocusOnError="true" ValidationGroup="DiscountEdit" />--%>
                                    </div>
                                </div>
                                <div id="divMinPurchase" runat="server" visible="false" class="settingrow form-group">
                                    <gb:SiteLabel ID="lblMinPurchase" runat="server" ForControl="txtMinPurchase" CssClass="settinglabel control-label col-sm-3" Text="Đơn hàng tối thiểu" ResourceFile="ProductResources" />
                                    <div class="col-sm-9">
                                        <asp:TextBox ID="txtMinPurchase" SkinID="PriceTextBox" MaxLength="20" runat="server" />
                                    </div>
                                </div>
                                <div id="divDiscountQtyStep" visible="false" runat="server" class="settingrow form-group">
                                    <gb:SiteLabel ID="lblDiscountQtyStep" runat="server" ForControl="ddlDiscountQtyStep" CssClass="settinglabel control-label col-sm-3" Text="SL sản phẩm được áp dụng trong mỗi đơn hàng" ResourceFile="ProductResources" />
                                    <div class="col-sm-9">
                                        <asp:DropDownList ID="ddlDiscountQtyStep" runat="server">
                                            <asp:ListItem Value="0" Text="Không giới hạn"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                            <asp:ListItem Value="5" Text="5"></asp:ListItem>
                                            <asp:ListItem Value="6" Text="6"></asp:ListItem>
                                            <asp:ListItem Value="7" Text="7"></asp:ListItem>
                                            <asp:ListItem Value="8" Text="8"></asp:ListItem>
                                            <asp:ListItem Value="9" Text="9"></asp:ListItem>
                                            <asp:ListItem Value="10" Text="10"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div id="divShareType" runat="server" visible="false" class="settingrow form-group">
                                    <gb:SiteLabel ID="lblIsShared" runat="server" ForControl="chkListShareType" CssClass="settinglabel control-label col-sm-3" Text="Sử dụng KM" ResourceFile="ProductResources" />
                                    <div class="col-sm-9">
                                        <asp:CheckBoxList ID="chkListShareType" CssClass="share-type" runat="server" SkinID="DiscountShareType" RepeatLayout="UnorderedList" RepeatColumns="1">
                                            <%--<asp:ListItem Text="Cho phép dùng chung với KM khác có cùng tùy chọn này" Value="2"></asp:ListItem>--%>
                                            <asp:ListItem Text="Luôn luôn sử dụng KM này" Value="1"></asp:ListItem>
                                        </asp:CheckBoxList>
                                        <asp:Literal ID="litShareType" runat="server" Text="Loại KM này không dùng chung với KM khác" Visible="false" />
                                        <style>
                                            .share-type{margin: 0;padding: 0;}.share-type li {list-style: none}.share-type li label {font-weight: normal !important}
                                        </style>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-horizontal">
                            <div id="divOrderDiscountRange" visible="false" runat="server">
                                <%--<asp:UpdatePanel ID="upOrderDiscountRange" runat="server">
                                <ContentTemplate>--%>
                                <telerik:RadGrid ID="gridOrderDiscountRange" SkinID="radGridSkin" runat="server">
                                    <MasterTableView DataKeyNames="ItemID,DiscountType,FromPrice,ToPrice,DiscountAmount,MaximumDiscount">
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderText="Giá trị đơn hàng từ">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtFromPrice" Text='<%# ProductHelper.FormatPrice(Convert.ToDecimal(Eval("FromPrice"))) %>' runat="server" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Giá trị đơn hàng đến">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtToPrice" Text='<%# ProductHelper.FormatPrice(Convert.ToDecimal(Eval("ToPrice"))) %>' runat="server" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Chiết khấu">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDiscountAmount" Text='<%# ProductHelper.FormatPrice(Convert.ToDecimal(Eval("DiscountAmount"))) %>' runat="server" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Tính theo">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlDiscountType" CssClass="percentage" Style="width: auto; display: inline-block" runat="server">
                                                        <asp:ListItem Value="1" Text="%"></asp:ListItem>
                                                        <asp:ListItem Value="2" Text="VND"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Giảm tối đa">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtMaximumDiscount" SkinID="PriceTextBox" Text='<%# ProductHelper.FormatPrice(Convert.ToDecimal(Eval("MaximumDiscount"))) %>' runat="server" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemTemplate>
                                                    <asp:HyperLink CssClass="popup-link" ID="GiftProducts" runat="server"
                                                        Font-Bold='<%# !string.IsNullOrEmpty(Eval("GiftHtml").ToString()) %>'
                                                        Text="Quà tặng" NavigateUrl='<%# SiteRoot + "/Product/AdminCP/Promotions/DiscountGiftDialog.aspx?ItemID=" + Eval("ItemID") %>'>
                                                    </asp:HyperLink>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemTemplate>
                                                    <asp:Button SkinID="DefaultButton" ID="btnOrderRangeDelete" Text="Xóa" runat="server" CausesValidation="false" CommandName="Delete" CommandArgument='<%#Eval("ItemID").ToString()%>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <%--<telerik:GridClientSelectColumn HeaderStyle-Width="35" />--%>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                                <div class="mrt10">
                                    <asp:Button SkinID="DefaultButton" ID="btnOrderRangeNewRow" Text="Thêm dòng" runat="server" CausesValidation="false" />
                                </div>
                                <%--</ContentTemplate>
                            </asp:UpdatePanel>--%>
                            </div>
                            <div id="divAppliedToCategories" visible="false" runat="server">
                                <h5>
                                    <gb:SiteLabel ID="lblAppliedToCategories" runat="server" UseLabelTag="false" Text="Danh mục sản phẩm" ResourceFile="ProductResources" />
                                </h5>
                                <div class="appliedtocats">
                                    <%--<asp:UpdatePanel ID="upZones" runat="server">
                                    <ContentTemplate>--%>
                                    <div class="row">
                                        <div class="col-sm-5">
                                            <div class="settingrow form-group">
                                                <asp:Button SkinID="DefaultButton" ID="btnSearch2" Text="Load danh mục" runat="server" CausesValidation="false" />
                                            </div>
                                            <telerik:RadGrid ID="grid2" SkinID="radGridSkin" runat="server">
                                                <MasterTableView DataKeyNames="ZoneId" PageSize="10" AllowFilteringByColumn="true">
                                                    <Columns>
                                                        <telerik:GridTemplateColumn HeaderText="Danh mục"
                                                            DataField="Name" UniqueName="Name" SortExpression="Name" CurrentFilterFunction="Contains"
                                                            AutoPostBackOnFilter="true" ShowFilterIcon="false" FilterControlWidth="100%">
                                                            <ItemTemplate>
                                                                <asp:HyperLink CssClass="cp-link" ID="ZonesEditSettings" runat="server" Text='<%# Eval("Name") %>'
                                                                    NavigateUrl='<%# SiteRoot + "/AdminCP/ZoneSettings.aspx?zoneid=" + Eval("ZoneID") %>'>
                                                                </asp:HyperLink>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridClientSelectColumn HeaderStyle-Width="35" />
                                                    </Columns>
                                                </MasterTableView>
                                            </telerik:RadGrid>
                                        </div>
                                        <div class="col-sm-1 mrt80 text-center">
                                            <asp:LinkButton ID="btnRemove2" CssClass="btn btn-default" runat="server" ToolTip="Xóa danh mục được chọn bên phải"><i class="fa fa-angle-left"></i></asp:LinkButton>
                                            <asp:LinkButton ID="btnAdd2" CssClass="btn btn-default" runat="server" ToolTip="Thêm danh mục được chọn từ bên trái"><i class="fa fa-angle-right"></i></asp:LinkButton>
                                        </div>
                                        <div class="col-sm-6">
                                            <div class="mrb20 mrt5">
                                                <gb:SiteLabel ID="lblSelectedZones" runat="server" UseLabelTag="false" Text="Danh mục được chọn" />
                                            </div>
                                            <telerik:RadGrid ID="gridRelated2" SkinID="radGridSkin" runat="server">
                                                <MasterTableView DataKeyNames="Guid,ItemId,UsePercentage,DiscountAmount,MaximumDiscount" PageSize="10">
                                                    <Columns>
                                                        <telerik:GridTemplateColumn HeaderText="Danh mục">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litZoneName" runat="server" />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Giảm giá" UniqueName="DiscountAmount">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtDiscountAmount" SkinID="PriceTextBox" Style="display: inline-block" Text='<%# ProductHelper.FormatPrice(Convert.ToDecimal(Eval("DiscountAmount"))) %>' MaxLength="20" runat="server" />
                                                                <asp:DropDownList ID="ddlDiscountType" CssClass="percentage" Style="width: auto; display: inline-block" runat="server">
                                                                    <asp:ListItem Value="1" Text="%"></asp:ListItem>
                                                                    <asp:ListItem Value="2" Text="VND"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Giảm tối đa" Visible="false" Display="false">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtMaximumDiscount" SkinID="PriceTextBox" Text='<%# ProductHelper.FormatPrice(Convert.ToDecimal(Eval("MaximumDiscount"))) %>' runat="server" />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn>
                                                            <ItemTemplate>
                                                                <%--<asp:HyperLink CssClass="popup-link" ID="GiftProducts" runat="server"
                                                                    Font-Bold='<%# Convert.ToInt32(Eval("GiftType")) > 0 %>'
                                                                    Text="Quà tặng" NavigateUrl='<%# SiteRoot + "/Product/AdminCP/Promotions/DiscountChooseGiftDialog.aspx?Guid=" + Eval("Guid") %>'>
                                                                </asp:HyperLink>--%>
                                                                <asp:HyperLink CssClass="popup-link" ID="GiftProducts" runat="server"
                                                                    Font-Bold='<%# !string.IsNullOrEmpty(Eval("GiftHtml").ToString()) %>'
                                                                    Text="Quà tặng" NavigateUrl='<%# SiteRoot + "/Product/AdminCP/Promotions/DiscountGiftDialog.aspx?Guid=" + Eval("Guid") %>'>
                                                                </asp:HyperLink>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridClientSelectColumn HeaderStyle-Width="35" />
                                                    </Columns>
                                                </MasterTableView>
                                            </telerik:RadGrid>
                                        </div>
                                    </div>
                                    <%--</ContentTemplate>
                                </asp:UpdatePanel>--%>
                                </div>
                            </div>
                            <div id="divAppliedToProducts" runat="server" visible="false" class="appliedtoproducts">
                                <h5 class="mrt20">
                                    <gb:SiteLabel ID="lblAppliedToProducts" runat="server" UseLabelTag="false" Text="Sản phẩm" ResourceFile="ProductResources" />
                                </h5>
                                <asp:UpdatePanel ID="up2" runat="server">
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnProductRelatedExport" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <div class="row">
                                            <div class="col-sm-4">
                                                <asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server">
                                                    <div class="settingrow form-group">
                                                        <gb:SiteLabel ID="lblZones" runat="server" ConfigKey="ZoneLabel"
                                                            ForControl="cobZones" CssClass="settinglabel control-label col-sm-3" />
                                                        <div class="col-sm-9">
                                                            <portal:ComboBox ID="cobZones" Width="100%" SelectionMode="Single" AutoPostBack="false" runat="server"></portal:ComboBox>
                                                        </div>
                                                    </div>
                                                    <div class="settingrow form-group">
                                                        <gb:SiteLabel ID="lblTitle" runat="server" ConfigKey="ProductNameLabel"
                                                            ResourceFile="ProductResources" ForControl="txtTitle" CssClass="settinglabel control-label col-sm-3" />
                                                        <div class="col-sm-9">
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtTitle" runat="server" MaxLength="255" />
                                                                <div class="input-group-btn">
                                                                    <asp:Button SkinID="DefaultButton" ID="btnSearch" Text="<%$Resources:ProductResources, ProductSearchButton %>"
                                                                        runat="server" CausesValidation="false" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                                <telerik:RadGrid ID="grid" SkinID="radGridSkin" runat="server">
                                                    <MasterTableView DataKeyNames="ProductId" AllowPaging="true" PageSize="10">
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderStyle-Width="50" HeaderText="<%$Resources:ProductResources, ProductPictureHeading %>">
                                                                <ItemTemplate>
                                                                    <portal:MediaElement ID="ml" runat="server" Width="40" Title=" " FileUrl='<%# ProductHelper.GetImageFilePath(siteSettings.SiteId, Convert.ToInt32(Eval("ProductId")), Eval("ImageFile").ToString(), Eval("ThumbnailFile").ToString()) %>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Mã">
                                                                <ItemTemplate>
                                                                    <%# Eval("Code") %>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Sản phẩm">
                                                                <ItemTemplate>
                                                                    <asp:HyperLink CssClass="cp-link" ID="Title" runat="server" Text='<%# Eval("Title").ToString() %>'
                                                                        NavigateUrl='<%# ProductHelper.FormatProductUrl(Eval("Url").ToString(), Convert.ToInt32(Eval("ProductId")), Convert.ToInt32(Eval("ZoneID")))  %>'>
                                                                    </asp:HyperLink>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderStyle-Width="75" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" HeaderText="<%$Resources:ProductResources, ProductPriceLabel %>" UniqueName="Price">
                                                                <ItemTemplate>
                                                                    <%# ProductHelper.FormatPrice(Convert.ToDecimal(Eval("Price")), true) %>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn Visible="false" HeaderStyle-Width="50" HeaderText="Hot?" UniqueName="SpecialProduct">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnSpecialProduct" runat="server"
                                                                        CommandName="SpecialProduct" CommandArgument='<%#Eval("ProductId")%>' ImageUrl='<%# GetSpecialProductImageUrl(Convert.ToInt32(Eval("ProductId")))%>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridClientSelectColumn HeaderStyle-Width="35" />
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </div>
                                            <div class="col-sm-1 text-center" style="margin-top: 155px">
                                                <asp:LinkButton ID="btnRemove" CssClass="btn btn-default" runat="server" ToolTip="Xóa sản phẩm được chọn bên phải"><i class="fa fa-angle-left"></i></asp:LinkButton>
                                                <asp:LinkButton ID="btnAdd" CssClass="btn btn-default" runat="server" ToolTip="Thêm sản phẩm được chọn từ bên trái"><i class="fa fa-angle-right"></i></asp:LinkButton>
                                                <div class="mrt5">
                                                    <asp:LinkButton ID="btnAll" CssClass="btn btn-default" runat="server" ToolTip="Thêm tất cả sản phẩm"><i class="fa fa-angle-double-right"></i></asp:LinkButton>
                                                </div>
                                            </div>
                                            <div class="col-sm-7 mrt0">
                                                <div id="divProductRelatedImport" runat="server">
                                                    <div class="settingrow form-group row align-items-center">
                                                        <gb:SiteLabel ID="lblProductRelatedFile" runat="server"
                                                            CssClass="settinglabel control-label col-sm-3 mb-0" ShowRequired="true" Text="Import từ excel" />
                                                        <div class="col-sm-9">
                                                            <div class="input-group">
                                                                <telerik:RadAsyncUpload ID="fupProductRelated" Localization-Select="Chọn file" SkinID="radAsyncUploadSkin" MultipleFileSelection="Disabled" AllowedFileExtensions="xls" runat="server" />
                                                                <div class="input-group-btn">
                                                                    <asp:Button SkinID="DefaultButton" ID="btnProductRelatedImport" Text="Import" runat="server" />
                                                                    <asp:LinkButton ID="btnProductRelatedExport" Text="Xuất excel" runat="server" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="settingrow form-group">
                                                    <gb:SiteLabel ID="lblRelatedProduct" runat="server" ConfigKey="CouponSelectedProductsLabel"
                                                        ResourceFile="ProductResources" ForControl="txtProductRelatedKeyword" CssClass="settinglabel control-label col-sm-3" />
                                                    <div class="col-sm-9">
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtProductRelatedKeyword" runat="server" placeholder="Tìm sản phẩm" MaxLength="255" />
                                                            <div class="input-group-btn">
                                                                <asp:Button SkinID="DefaultButton" ID="btnProductRelatedKeyword" Text="<%$Resources:ProductResources, ProductSearchButton %>"
                                                                    runat="server" CausesValidation="false" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <telerik:RadGrid ID="gridRelated" SkinID="radGridSkin" runat="server">
                                                    <MasterTableView DataKeyNames="Guid,ItemId,UsePercentage,DiscountAmount,DealQty,SoldQty,ComboSaleQty,FromDate,ToDate,MaximumDiscount,DisplayOrder" PageSize="10">
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderStyle-Width="50" HeaderText="<%$Resources:ProductResources, ProductPictureHeading %>">
                                                                <ItemTemplate>
                                                                    <portal:MediaElement ID="ml" Title=" " runat="server" Width="40" />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Sản phẩm">
                                                                <ItemTemplate>
                                                                    <div><asp:Literal ID="litProductCode" runat="server" /></div>
                                                                    <asp:Literal ID="litProductName" runat="server" />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Giảm giá" UniqueName="DiscountAmount">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtDiscountAmount" SkinID="PriceTextBox" Style="display: inline-block" Text='<%# ProductHelper.FormatPrice(Convert.ToDecimal(Eval("DiscountAmount"))) %>' MaxLength="20" runat="server" />
                                                                    <asp:DropDownList ID="ddlDiscountType" CssClass="percentage" Style="width: auto; display: inline-block" runat="server">
                                                                        <asp:ListItem Value="1" Text="%"></asp:ListItem>
                                                                        <asp:ListItem Value="2" Text="VND"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Giảm tối đa" UniqueName="MaximumDiscount">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtMaximumDiscount" SkinID="PriceTextBox" Text='<%# ProductHelper.FormatPrice(Convert.ToDecimal(Eval("MaximumDiscount"))) %>' runat="server" />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn Visible="false" HeaderText="SL Deal/Đã bán" UniqueName="DealQty">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtDealQty" Style="display: inline-block" SkinID="NumericTextBox" Text='<%# Eval("DealQty") %>' runat="server" />
                                                                    <asp:TextBox ID="txtSoldQty" Style="display: inline-block" SkinID="NumericTextBox" Text='<%# Eval("SoldQty") %>' runat="server" />
                                                                    <%--<%# Eval("SoldQty") %>--%>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn Visible="false" HeaderText="SL" UniqueName="ComboSaleQty">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtComboSaleQty" SkinID="NumericTextBox" Text='<%# Eval("ComboSaleQty") %>' runat="server" />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn Visible="false" HeaderText="Ngày" UniqueName="DealDate">
                                                                <ItemTemplate>
                                                                    <telerik:RadDateTimePicker ID="dpFromDate" DateInput-EmptyMessage="Từ ngày" SelectedDate='<%# Eval("FromDate") %>' Culture="en-US" DateInput-DateFormat="dd/MM/yyyy HH:mm" TimeView-TimeFormat="HH:mm" runat="server" Skin="Simple" />
                                                                    <telerik:RadDateTimePicker ID="dpToDate" DateInput-EmptyMessage="Đến ngày" SelectedDate='<%# Eval("ToDate") %>' Culture="en-US" DateInput-DateFormat="dd/MM/yyyy HH:mm" TimeView-TimeFormat="HH:mm" runat="server" Skin="Simple" />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn Visible="false" HeaderText="Thứ tự" UniqueName="DisplayOrder">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtDisplayOrder" SkinID="NumericTextBox" Text='<%# Eval("DisplayOrder") %>' runat="server" />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="GiftHtml">
                                                                <ItemTemplate>
                                                                    <asp:HyperLink CssClass="popup-link" ID="ComboSale" runat="server"
                                                                        Font-Bold='<%# Eval("ComboSaleRules").ToString().Length > 0 %>'
                                                                        Visible='<%# discount.DiscountType == (int)DiscountType.ComboSale %>'
                                                                        Text="Combo sale" NavigateUrl='<%# SiteRoot + "/Product/AdminCP/Promotions/DiscountComboSaleDialog.aspx?Guid=" + Eval("Guid") %>'>
                                                                    </asp:HyperLink>
                                                                    <%--<asp:HyperLink CssClass="popup-link" ID="GiftProducts" runat="server"
                                                                Font-Bold='<%# Convert.ToInt32(Eval("GiftType")) > 0 %>'
                                                                Visible='<%# discount.DiscountType != (int)DiscountType.ComboSale %>'
                                                                Text="Quà tặng" NavigateUrl='<%# SiteRoot + "/Product/AdminCP/Promotions/DiscountChooseGiftDialog.aspx?Guid=" + Eval("Guid") %>'>
                                                            </asp:HyperLink>--%>
                                                                    <asp:HyperLink CssClass="popup-link" ID="GiftProducts" runat="server"
                                                                        Font-Bold='<%# !string.IsNullOrEmpty(Eval("GiftHtml").ToString()) %>'
                                                                        Text="Quà tặng" NavigateUrl='<%# SiteRoot + "/Product/AdminCP/Promotions/DiscountGiftDialog.aspx?Guid=" + Eval("Guid") %>'>
                                                                    </asp:HyperLink>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn Visible="false" UniqueName="ProductRange">
                                                                <ItemTemplate>
                                                                    <asp:HyperLink CssClass="popup-link" ID="ProductRange" runat="server"
                                                                        Text="Cấu hình" NavigateUrl='<%# SiteRoot + "/Product/AdminCP/Promotions/DiscountProductRangeDialog.aspx?ProductID=" + Eval("ItemID") + "&DiscountID=" + Eval("DiscountID") %>'>
                                                                    </asp:HyperLink>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridClientSelectColumn HeaderStyle-Width="35" />
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                                <div id="divExcludedZones" runat="server" visible="false" class="settingrow form-group">
                                                    <gb:SiteLabel ID="lblExcludedZones" runat="server" Text="Loại trừ DM"
                                                        ForControl="cobExcludedZones" CssClass="settinglabel control-label col-sm-3" />
                                                    <div class="col-sm-9">
                                                        <portal:ComboBox ID="cobExcludedZones" SelectionMode="Multiple" runat="server"></portal:ComboBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                    <div role="tabpanel" class="tab-pane fade" id="tabCoupon" visible="false" runat="server">
                        <asp:UpdatePanel ID="upCoupon" runat="server">
                            <ContentTemplate>
                                <div class="form-horizontal">
                                    <div class="settingrow form-group row align-items-center">
                                        <gb:SiteLabel ID="lblCouponNewType" runat="server"
                                            CssClass="settinglabel control-label col-sm-3 mb-0" Text="Thêm mã" />
                                        <div class="col-sm-9">
                                            <div class="input-group">
                                                <asp:DropDownList ID="ddlCouponNewType" AutoPostBack="true" runat="server">
                                                    <asp:ListItem Text="Thêm thủ công" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Phát sinh mã" Value="2"></asp:ListItem>
                                                    <%--<asp:ListItem Text="Import từ excel" Value="3"></asp:ListItem>--%>
                                                </asp:DropDownList>
                                                <div class="input-group-btn">
                                                    <asp:Button SkinID="DeleteButton" ID="btnDeleteCoupon" Visible="false" runat="server" Text="<%$Resources:Resource, DeleteSelectedButton %>" CausesValidation="false" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="divCouponManual" runat="server">
                                        <div class="settingrow form-group row align-items-center">
                                            <gb:SiteLabel ID="lblCouponCode" runat="server"
                                                CssClass="settinglabel control-label col-sm-3 mb-0" ShowRequired="true" Text="Nhập Mã" />
                                            <div class="col-sm-9">
                                                <asp:TextBox ID="txtCouponCode" runat="server" TextMode="MultiLine" Rows="2" placeholder="Mỗi mã 1 dòng" CssClass="form-control" />
                                                <asp:RequiredFieldValidator ID="reqCouponCode" runat="server"
                                                    ControlToValidate="txtCouponCode" Display="Dynamic" SetFocusOnError="true"
                                                    CssClass="txterror" ValidationGroup="CouponAddNew" />
                                            </div>
                                        </div>
                                        <div class="settingrow form-group row align-items-center">
                                            <gb:SiteLabel ID="lblLimitationTimes2" runat="server"
                                                CssClass="settinglabel control-label col-sm-3 mb-0" Text="Giới hạn" />
                                            <div class="col-sm-9">
                                                <asp:TextBox ID="txtLimitationTimes2" runat="server" Text="0" SkinID="PriceTextBox" CssClass="form-control" MaxLength="10" />
                                            </div>
                                        </div>
                                        <div class="settingrow form-group row align-items-center">
                                            <div class="col-sm-3"></div>
                                            <div class="col-sm-9">
                                                <asp:Button ID="btnInsertCoupon" SkinID="InsertButton" Text="<%$Resources:Resource, InsertButton %>" runat="server" ValidationGroup="CouponAddNew" />
                                                <asp:Button runat="server" SkinID="UpdateButton" ID="btnUpdateCoupon" Text="Cập nhật" />
                                            </div>
                                        </div>
                                    </div>
                                    <div id="divCouponGenerate" visible="false" runat="server">
                                        <div class="settingrow form-group row align-items-center">
                                            <gb:SiteLabel ID="lblCouponGenerate" runat="server" ShowRequired="true"
                                                CssClass="settinglabel control-label col-sm-3 mb-0" Text="Số ký tự" />
                                            <div class="col-sm-9">
                                                <asp:TextBox ID="txtCouponLength" MaxLength="2" runat="server" CssClass="form-control" />
                                                <asp:RequiredFieldValidator ID="reqCouponLength" runat="server"
                                                    ControlToValidate="txtCouponLength" Display="Dynamic" SetFocusOnError="true"
                                                    CssClass="txterror" ValidationGroup="CouponGenerate" />
                                            </div>
                                        </div>
                                        <div class="settingrow form-group row align-items-center">
                                            <gb:SiteLabel ID="lblCouponCount" runat="server"
                                                CssClass="settinglabel control-label col-sm-3 mb-0" ShowRequired="true" Text="Số lượng mã" />
                                            <div class="col-sm-9">
                                                <asp:TextBox ID="txtCouponCount" runat="server" CssClass="form-control" MaxLength="2" />
                                                <asp:RequiredFieldValidator ID="reqCouponCount" runat="server"
                                                    ControlToValidate="txtCouponCount" Display="Dynamic" SetFocusOnError="true"
                                                    CssClass="txterror" ValidationGroup="CouponGenerate" />
                                            </div>
                                        </div>
                                        <div class="settingrow form-group row align-items-center">
                                            <gb:SiteLabel ID="lblCouponPrefix" runat="server"
                                                CssClass="settinglabel control-label col-sm-3 mb-0" Text="Prefix" />
                                            <div class="col-sm-9">
                                                <asp:TextBox ID="txtCouponPrefix" runat="server" CssClass="form-control" MaxLength="10" />
                                            </div>
                                        </div>
                                        <div class="settingrow form-group row align-items-center">
                                            <gb:SiteLabel ID="lblLimitationTimes" runat="server"
                                                CssClass="settinglabel control-label col-sm-3 mb-0" Text="Giới hạn" />
                                            <div class="col-sm-9">
                                                <asp:TextBox ID="txtLimitationTimes" runat="server" Text="0" SkinID="PriceTextBox" CssClass="form-control" MaxLength="10" />
                                            </div>
                                        </div>
                                        <div class="settingrow form-group row align-items-center">
                                            <div class="col-sm-3"></div>
                                            <div class="col-sm-9">
                                                <asp:Button ID="btnCouponGenerate" SkinID="InsertButton" Text="Phát sinh" runat="server" ValidationGroup="CouponGenerate" />
                                            </div>
                                        </div>
                                    </div>
                                    <div id="divCouponImport" visible="false" runat="server">
                                        <div class="settingrow form-group">
                                            <gb:SiteLabel ID="lblCouponOverwrite" runat="server" ForControl="ddlCouponOverwrite" CssClass="settinglabel control-label col-sm-3" Text="Nhập mã khuyến mãi trùng" ResourceFile="ProductResources" />
                                            <div class="col-sm-9">
                                                <asp:DropDownList ID="ddlCouponOverwrite" runat="server">
                                                    <asp:ListItem Value="write" Text="Không ghi đè mã khuyến mãi"></asp:ListItem>
                                                    <asp:ListItem Value="overwrite" Text="Ghi đè mã khuyến mãi"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="settingrow form-group row align-items-center">
                                            <gb:SiteLabel ID="lblCouponImport" runat="server"
                                                CssClass="settinglabel control-label col-sm-3 mb-0" ShowRequired="true" Text="Chọn file" />
                                            <div class="col-sm-9">
                                                <telerik:RadAsyncUpload ID="fileUpload" SkinID="radAsyncUploadSkin" MultipleFileSelection="Disabled" AllowedFileExtensions="xls" runat="server" />
                                            </div>
                                        </div>
                                        <div class="settingrow form-group row align-items-center">
                                            <div class="col-sm-3"></div>
                                            <div class="col-sm-9">
                                                <asp:Button SkinID="DefaultButton" ID="btnCouponContinue" ValidationGroup="ImportCoupon" Text="<%$Resources:ProductResources, CouponContinueButton %>" runat="server" />
                                                <asp:Button SkinID="DefaultButton" ID="btnCouponImport" Visible="false" Text="<%$Resources:ProductResources, CouponContinueButton %>" runat="server" CausesValidation="false" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <telerik:RadGrid ID="gridCoupon" SkinID="radGridSkin" runat="server">
                                    <MasterTableView DataKeyNames="Guid,LimitationTimes" AllowSorting="false">
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderText="Mã" AllowFiltering="false">
                                                <ItemTemplate>
                                                    <%# Eval("CouponCode") %>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Đã sử dụng" AllowFiltering="false">
                                                <ItemTemplate>
                                                    <%# Eval("UseCount") %>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Giới hạn" AllowFiltering="false">
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtLimitTimes" Text='<%# Eval("LimitationTimes") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridClientSelectColumn HeaderStyle-Width="35" />
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div role="tabpanel" class="tab-pane fade" id="tabLandingPage" visible="false" runat="server">
                        <asp:UpdatePanel ID="updAttribute" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-3">
                                        <div class="input-group">
                                            <asp:ListBox ID="lbAttribute" Width="100%" Height="500" AutoPostBack="true" runat="server"
                                                AppendDataBoundItems="true" DataTextField="Title" DataValueField="ContentId" />
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
                                        <div class="settingrow form-group">
                                            <asp:Button SkinID="DefaultButton" ID="btnAttributeUpdate" Text="<%$Resources:ProductResources, AttributeUpdateButton %>" runat="server" />
                                            <asp:Button SkinID="DefaultButton" ID="btnDeleteAttributeLanguage" Visible="false" OnClick="btnDeleteAttributeLanguage_Click" Text="<%$Resources:Resource, DeleteLanguageButton %>" runat="server" CausesValidation="false" />
                                        </div>
                                        <div class="settingrow form-group">
                                            <gb:SiteLabel ID="lblAttribteZones" runat="server" Text="Danh mục" CssClass="settinglabel control-label col-sm-3" />
                                            <div class="col-sm-9">
                                                <portal:ComboBox ID="cobAttribteZones" SelectionMode="Multiple" runat="server" />
                                                <style>
                                                    .chosen-container {
                                                        width: 100% !important
                                                    }
                                                </style>
                                            </div>
                                        </div>
                                        <div class="settingrow form-group" id="divLoadType" runat="server">
                                            <gb:SiteLabel ID="lblLoadType" runat="server" Text="Template hiển thị SP" CssClass="settinglabel control-label col-sm-3" />
                                            <div class="col-sm-9">
                                                <asp:DropDownList ID="ddlLoadType" AppendDataBoundItems="true" runat="server">
                                                    <asp:ListItem Text="" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="settingrow form-group">
                                            <gb:SiteLabel ID="lblAttributeBannerFile" runat="server" Text="Hình ảnh" CssClass="settinglabel control-label col-sm-3" />
                                            <div class="col-sm-9">
                                                <img alt="" id="imgAttributeBannerFile" visible="false" runat="server" src="/Data/SiteImages/1x1.gif" style="max-width: 100px" />
                                                <asp:CheckBox ID="chkDeleteAttributeBannerFile" Visible="false" runat="server" Text="Xóa ảnh" />
                                                <telerik:RadAsyncUpload ID="fupAttributeBannerFile" SkinID="radAsyncUploadSkin" MultipleFileSelection="Automatic"
                                                    AllowedFileExtensions="jpg,jpeg,gif,png" runat="server" />
                                            </div>
                                        </div>
                                        <telerik:RadTabStrip ID="tabAttributeLanguage" OnTabClick="tabAttributeLanguage_TabClick"
                                            EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false"
                                            CssClass="subtabs" SkinID="SubTabs" Visible="false" SelectedIndex="0" runat="server" />
                                        <div class="settingrow form-group">
                                            <gb:SiteLabel ID="lblAttributeTitle" runat="server" CssClass="settinglabel control-label col-sm-3" ForControl="txtAttributeTitle" ConfigKey="AttributeTitle"
                                                ResourceFile="ProductResources" />
                                            <div class="col-sm-9">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtAttributeTitle" runat="server" MaxLength="255" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="settingrow form-group">
                                            <gb:SiteLabel ID="SiteLabel3" runat="server" ConfigKey="AttributeContent"
                                                ResourceFile="ProductResources" />
                                        </div>
                                        <div class="settingrow form-group">
                                            <gbe:EditorControl ID="edAttributeContent" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div id="divPages" visible="false" runat="server" class="settingrow form-group">
                            <gb:SiteLabel ID="lblPages" runat="server" ForControl="ddPages" CssClass="settinglabel control-label col-sm-3" Text="Landing page?" />
                            <div class="col-sm-9">
                                <asp:DropDownList ID="ddPages" runat="server" DataTextField="PageName" DataValueField="PageID" />
                                <%--<asp:CheckBox ID="chkPageID" runat="server" />--%>
                            </div>
                        </div>
                        <div class="settingrow form-group">
                            <gb:SiteLabel ID="lblImageFile" runat="server" Text="Ảnh chương trình" CssClass="settinglabel control-label col-sm-3" />
                            <div class="col-sm-9">
                                <img alt="" id="imgImageFile" visible="false" runat="server" src="/Data/SiteImages/1x1.gif" style="max-width: 100px" />
                                <asp:CheckBox ID="chkDeleteImageFile" Visible="false" runat="server" Text="Xóa ảnh" />
                                <telerik:RadAsyncUpload ID="fupImageFile" SkinID="radAsyncUploadSkin" MultipleFileSelection="Automatic"
                                    AllowedFileExtensions="jpg,jpeg,gif,png" runat="server" />
                            </div>
                        </div>
                        <div class="settingrow form-group">
                            <gb:SiteLabel ID="lblBannerFile" runat="server" Text="Banner chương trình" CssClass="settinglabel control-label col-sm-3" />
                            <div class="col-sm-9">
                                <img alt="" id="imgBannerFile" visible="false" runat="server" src="/Data/SiteImages/1x1.gif" style="max-width: 100px" />
                                <asp:CheckBox ID="chkDeleteBannerFile" Visible="false" runat="server" Text="Xóa ảnh" />
                                <telerik:RadAsyncUpload ID="fupBannerFile" SkinID="radAsyncUploadSkin" MultipleFileSelection="Automatic"
                                    AllowedFileExtensions="jpg,jpeg,gif,png" runat="server" />
                            </div>
                        </div>
                        <asp:UpdatePanel ID="up" runat="server">
                            <ContentTemplate>
                                <telerik:RadTabStrip ID="tabLanguage" OnTabClick="tabLanguage_TabClick"
                                    EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false"
                                    CssClass="subtabs" SkinID="SubTabs" Visible="false" SelectedIndex="0" runat="server" />
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
                                <div class="settingrow form-group">
                                    <gb:SiteLabel ID="lblBriefContent" runat="server" ForControl="edBriefContent" CssClass="settinglabel control-label col-sm-3"
                                        Text="Mô tả ngắn" ResourceFile="ProductResources" />
                                    <div class="col-sm-9">
                                        <gbe:EditorControl ID="edBriefContent" runat="server" />
                                    </div>
                                </div>
                                <div class="settingrow form-group">
                                    <gb:SiteLabel ID="lblFullContent" runat="server" ForControl="edFullContent" CssClass="settinglabel control-label col-sm-3"
                                        Text="Nội dung" ResourceFile="ProductResources" />
                                    <div class="col-sm-9">
                                        <gbe:EditorControl ID="edFullContent" runat="server" />
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div role="tabpanel" class="tab-pane fade" id="tabHistory" visible="false" runat="server">
                        <asp:UpdatePanel ID="upHistory" runat="server">
                            <ContentTemplate>
                                <telerik:RadGrid ID="gridOrder" SkinID="radGridSkin" runat="server">
                                    <MasterTableView DataKeyNames="OrderId,OrderStatus" AllowSorting="false" AllowFilteringByColumn="true">
                                        <Columns>
                                            <telerik:GridBoundColumn HeaderText="<%$Resources:ProductResources, OrderIdLabel %>" DataField="OrderCode" AllowFiltering="false" />
                                            <telerik:GridBoundColumn HeaderText="Mã Coupon" DataField="CouponCode" UniqueName="CouponCode"
                                                CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" ShowFilterIcon="false" FilterControlWidth="100%" Visible="false" />
                                            <telerik:GridTemplateColumn HeaderText="<%$Resources:ProductResources, OrderCreatedOnLabel %>" AllowFiltering="false">
                                                <ItemTemplate>
                                                    <%# DateTimeHelper.Format(Convert.ToDateTime(Eval("CreatedUtc")), timeZone, "dd/MM/yyyy HH:mm", timeOffset)%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn AllowFiltering="false">
                                                <ItemTemplate>
                                                    <asp:HyperLink CssClass="cp-link" ID="EditLink" runat="server"
                                                        Text="<%$Resources:ProductResources, OrderDetailLink %>" NavigateUrl='<%# SiteRoot + "/Product/AdminCP/OrderEdit.aspx?OrderID=" + Eval("OrderId") %>'>
                                                    </asp:HyperLink>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
        <style type="text/css">
            .settingrow input.rcbInput {
                border: none;
                padding: 0;
                width: 100%;
            }

            .settingrow select.percentage {
                min-width: inherit;
                width: 60px;
            }

            .separator {
                margin-top: 10px;
                border: solid 1px #ccc
            }

            .popup-link {
                display: block;
                white-space: nowrap;
                font-size: 12px;
                text-decoration: underline
            }
        </style>
        <portal:SessionKeepAliveControl ID="ka1" runat="server" />
    </div>
</asp:Content>
