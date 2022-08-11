<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="DiscountCoupons.aspx.cs" Inherits="CanhCam.Web.ProductUI.DiscountCouponsPage" %>

<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:BreadcrumbAdmin ID="breadcrumb" runat="server"
        ParentTitle="<%$Resources:ProductResources, DiscountPageTitle %>" ParentUrl="~/Product/AdminCP/Promotions/Discounts.aspx"
        CurrentPageTitle="<%$Resources:ProductResources, CouponsPageTitle %>" CurrentPageUrl="~/Product/AdminCP/Promotions/DiscountCoupons.aspx" />
    <div class="admin-content col-md-12">
        <portal:HeadingPanel ID="heading" Visible="false" runat="server">
            <%--<asp:HyperLink SkinID="InsertButton" ID="lnkInsert" Text="<%$Resources:Resource, InsertButton %>" runat="server" />--%>
            <asp:Button SkinID="DeleteButton" ID="btnDelete" Text="<%$Resources:Resource, DeleteSelectedButton %>" runat="server" CausesValidation="false" />
        </portal:HeadingPanel>
        <portal:NotifyMessage ID="message" runat="server" />
        <asp:UpdatePanel ID="up" runat="server">
            <ContentTemplate>
        <div class="workplace">
            <div class="form-horizontal">
                <div class="settingrow form-group">
                    <gb:SiteLabel ID="lblDiscounts" runat="server" Text="Chương trình KM"
                        ResourceFile="ProductResources" ForControl="autDiscounts" CssClass="settinglabel control-label col-sm-3" />
                    <div class="col-sm-9">
                        <telerik:RadAutoCompleteBox ID="autDiscounts" CssClass="form-control" TextSettings-SelectionMode="Single" Skin="Simple" runat="server" Width="100%" DropDownWidth="500" DropDownHeight="200"
                            EmptyMessage="Tìm chương trình KM">
                            <WebServiceSettings Method="GetDiscounts" Path="/Product/AdminCP/Promotions/DiscountCoupons.aspx" />
                            <ClientDropDownItemTemplate>
                                <table cellpadding="0" cellspacing="3" width="100%">
                                    <tr>
                                        <td style="width:70%"><strong>#= Text #</strong></td>
                                        <td style="width:20%;text-align:right;">#= Attributes.Code #</td>
                                    </tr>
                                </table>
                            </ClientDropDownItemTemplate>
                        </telerik:RadAutoCompleteBox>
                    </div>
                </div>
                <div class="settingrow form-group">
                    <gb:SiteLabel ID="lblCouponCode" runat="server" Text="Mã Coupon" ForControl="txtCouponCode" CssClass="settinglabel control-label col-sm-3" />
                    <div class="col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="txtCouponCode" runat="server" MaxLength="255" />
                            <div class="input-group-btn">
                                <asp:Button SkinID="DefaultButton" ID="btnSearch" Text="<%$Resources:ProductResources, ProductSearchButton %>"
                                    runat="server" CausesValidation="false" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <telerik:RadGrid ID="grid" SkinID="radGridSkin" runat="server">
                <MasterTableView DataKeyNames="Guid" AllowSorting="false">
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
                                <%# Eval("LimitationTimes") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Đơn hàng sử dụng" AllowFiltering="false">
                            <ItemTemplate>
                                <%# Eval("UseCount") %>
                                <asp:LinkButton ID="btnViewOrders" CommandName="ViewOrders" CommandArgument='<%# Eval("CouponCode") %>' Visible='<%# Convert.ToInt32(Eval("UseCount")) > 0 %>' runat="server" Text="Xem"></asp:LinkButton>
                                <asp:Literal ID="litViewOrders" runat="server"></asp:Literal>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Tên CTKM" AllowFiltering="false">
                            <ItemTemplate>
                                <asp:HyperLink CssClass="cp-link" ID="EditLink" runat="server" 
                                    Text='<%# GetDiscountName(Convert.ToInt32(Eval("DiscountId"))) %>' NavigateUrl='<%# SiteRoot + "/Product/AdminCP/Promotions/DiscountEdit.aspx?DiscountID=" + Eval("DiscountId") %>'></asp:HyperLink>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--<telerik:GridTemplateColumn HeaderText="Thời hạn CT" AllowFiltering="false">--%>
                        <telerik:GridTemplateColumn HeaderText="Tình trạng" AllowFiltering="false">
                            <ItemTemplate>
                                <%# GetActive(Convert.ToInt32(Eval("DiscountId")), Convert.ToInt32(Eval("UseCount")), Convert.ToInt32(Eval("LimitationTimes"))) %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridClientSelectColumn HeaderStyle-Width="35" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>