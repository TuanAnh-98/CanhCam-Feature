<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="Discounts.aspx.cs" Inherits="CanhCam.Web.ProductUI.DiscountsPage" %>

<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:BreadcrumbAdmin ID="breadcrumb" runat="server"
        CurrentPageTitle="<%$Resources:ProductResources, DiscountPageTitle %>" CurrentPageUrl="~/Product/AdminCP/Promotions/Discounts.aspx" />
    <div class="admin-content col-md-12">
        <portal:HeadingPanel ID="heading" runat="server">
            <asp:HyperLink SkinID="InsertButton" ID="lnkInsert" Text="<%$Resources:Resource, InsertButton %>" runat="server" />
            <asp:Button SkinID="DeleteButton" ID="btnDelete" Text="<%$Resources:Resource, DeleteSelectedButton %>" runat="server" CausesValidation="false" />
        </portal:HeadingPanel>
        <portal:NotifyMessage ID="message" runat="server" />
        <div class="workplace">
            <div class="form-horizontal">
                <div id="divDiscountType" runat="server" class="settingrow form-group">
                    <gb:SiteLabel ID="lblDiscountType" runat="server" ForControl="ddlDiscountType" CssClass="settinglabel control-label col-sm-3" Text="Loại khuyến mãi" />
                    <div class="col-sm-9">
                        <asp:DropDownList ID="ddlDiscountType" runat="server"/>
                    </div>
                </div>
                <div class="settingrow form-group">
                    <gb:SiteLabel ID="lblDiscountStatus" runat="server" ForControl="ddlDiscountStatus" CssClass="settinglabel control-label col-sm-3" Text="Trạng thái" />
                    <div class="col-sm-9">
                        <asp:DropDownList ID="ddlDiscountStatus" runat="server">
                            <asp:ListItem Text="Tất cả" Value="-1"></asp:ListItem>
                            <asp:ListItem Text="Sử dụng" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Ngừng sử dụng" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="settingrow form-group">
                    <gb:SiteLabel ID="lblProducts" runat="server" Text="Sản phẩm"
                        ResourceFile="ProductResources" ForControl="autProducts" CssClass="settinglabel control-label col-sm-3" />
                    <div class="col-sm-9">
                        <div class="input-group">
                            <telerik:RadAutoCompleteBox ID="autProducts" CssClass="form-control" TextSettings-SelectionMode="Single" Skin="Simple" runat="server" Width="100%" DropDownWidth="500" DropDownHeight="200"
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
                            <div class="input-group-btn">
                                <asp:Button SkinID="DefaultButton" ID="btnSearch" Text="<%$Resources:ProductResources, ProductSearchButton %>"
                                    runat="server" CausesValidation="false" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <telerik:RadGrid ID="grid" SkinID="radGridSkin" runat="server">
                <MasterTableView DataKeyNames="DiscountId" AllowSorting="false">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Chương trình">
                            <ItemTemplate>
                                <%# Eval("Name") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Mã">
                            <ItemTemplate>
                                <%# Eval("Code") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ngày tạo">
                            <ItemTemplate>
                                <%# FormatDate(Eval("CreatedOn")) %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Người tạo">
                            <ItemTemplate>
                                <%# Eval("CreatedBy") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ưu tiên">
                            <ItemTemplate>
                                <%# Eval("Priority") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="<%$Resources:ProductResources, CouponExpiryDateLabel %>">
                            <ItemTemplate>
                                <%# FormatDate(Eval("EndDate")) %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--<telerik:GridTemplateColumn HeaderText="Số HĐ đã sử dụng">
                            <ItemTemplate>
                                <%# Eval("OrderCount") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                        <telerik:GridTemplateColumn HeaderText="<%$Resources:ProductResources, CouponActiveStatusLabel %>">
                            <ItemTemplate>
                                <%# GetActive(Convert.ToBoolean(Eval("IsActive")), Eval("StartDate"), Eval("EndDate"))%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <ItemTemplate>
                                <asp:HyperLink CssClass="cp-link" ID="EditLink" runat="server"
                                    Text="<%$Resources:ProductResources, CouponEditLink %>" NavigateUrl='<%# SiteRoot + "/Product/AdminCP/Promotions/DiscountEdit.aspx?DiscountID=" + Eval("DiscountId") %>'>
                                </asp:HyperLink>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridClientSelectColumn HeaderStyle-Width="35" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>