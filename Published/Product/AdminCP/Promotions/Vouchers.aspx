<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="Vouchers.aspx.cs" Inherits="CanhCam.Web.ProductUI.VouchersPage" %>

<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:BreadcrumbAdmin ID="breadcrumb" runat="server"
        CurrentPageTitle="<%$Resources:ProductResources, VouchersPageTitle %>" CurrentPageUrl="~/Product/AdminCP/Promotions/Vouchers.aspx" />
    <div class="admin-content col-md-12">
        <portal:HeadingPanel ID="heading" runat="server">
            <asp:Button SkinID="DeleteButton" ID="btnDelete" Text="<%$Resources:Resource, DeleteSelectedButton %>" runat="server" CausesValidation="false" />
        </portal:HeadingPanel>
        <portal:NotifyMessage ID="message" runat="server" />
        <div class="workplace">
            <div class="row">
                <div class="col-md-5 col-xs-12">
                    <div class="form-horizontal">
                        <div class="settingrow form-group">
                            <gb:SiteLabel ID="lblDiscountStatus" runat="server" ForControl="ddlDiscountStatus" CssClass="settinglabel control-label col-sm-3" Text="Trạng thái" />
                            <div class="col-sm-9">
                                <asp:DropDownList ID="ddlStatus" runat="server">
                                    <asp:ListItem Text="Tất cả" Value="-1"></asp:ListItem>
                                    <asp:ListItem Text="Còn hiệu lực" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Hết hiệu lực" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="settingrow form-group">
                            <gb:SiteLabel ID="lblTitle" runat="server" Text="Voucher code"
                                ResourceFile="ProductResources" ForControl="txtProduct" CssClass="settinglabel control-label col-sm-3" />
                            <div class="col-sm-9">
                                <div class="input-group">

                                    <asp:TextBox runat="server" ID="txtKeywords" />
                                    <div class="input-group-btn">
                                        <asp:Button SkinID="DefaultButton" ID="btnSearch" Text="<%$Resources:ProductResources, ProductSearchButton %>"
                                            runat="server" CausesValidation="false" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-7 col-xs-12">
                    <div class="form-horizontal">
                        <div class="settingrow form-group">
                            <gb:SiteLabel ID="lblFileUpload" runat="server" ForControl="fileUpload" CssClass="settinglabel control-label col-sm-3" Text="Nhập tập tin (xls)" ResourceFile="ProductResources" />
                            <div class="col-sm-9">
                                <telerik:RadAsyncUpload ID="fileUpload" SkinID="radAsyncUploadSkin" MultipleFileSelection="Disabled"
                                    AllowedFileExtensions="xls" runat="server" />
                                <asp:CheckBox ID="chkOverride" Text="Cập nhật voucher nếu đã tồn tại?" runat="server" />
                            </div>
                        </div>
                        <div class="settingrow form-group">
                            <div class="col-sm-9 col-sm-offset-3">
                                <asp:Button SkinID="DefaultButton" ID="btnImport" Text="Import" runat="server" />
                            </div>
                        </div>
                        <div class="settingrow form-group">
                            <div class="col-sm-offset-3 col-sm-9">
                                <h4>Lưu ý file dữ liệu</h4>
                                <ul class="import-tips">
                                    <li>Dữ liệu ở Sheet đầu tiên & bắt đầu từ dòng 2</li>
                                    <li>Mã voucher không được bỏ trống</li>
                                    <li><a href="/Data/Sites/1/media/import-vouchers-example.xls">File Mẫu</a></li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <telerik:RadGrid ID="grid" SkinID="radGridSkin" runat="server">
                <MasterTableView DataKeyNames="ItemGuid,VoucherCode,UseCount,LimitationTimes,MinimumOrderAmount,StartDate,EndDate" AllowSorting="false">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Mã">
                            <ItemTemplate>
                                <%# Eval("VoucherCode") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Đã sử dụng">
                            <ItemTemplate>
                                <%# Eval("UseCount") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Số lần sử dụng">
                            <ItemTemplate>
                                <%# Eval("LimitationTimes") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="áp dụng với đơn hàng tối thiểu">
                            <ItemTemplate>
                                <%# Eval("MinimumOrderAmount") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ngày bắt đầu">
                            <ItemTemplate>
                                <%# FormatDate(Eval("StartDate")) %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ngày hết hạn">
                            <ItemTemplate>
                                <%# FormatDate(Eval("EndDate")) %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Giá trị">
                            <ItemTemplate>
                                <%# CanhCam.Web.ProductUI.ProductHelper.FormatPrice(Convert.ToDecimal(Eval("Amount")),true) %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Đơn hàng đã sử dụng">
                            <ItemTemplate>
                                <%# CanhCam.Web.ProductUI.OrderHelper.FormatOrders(Eval("OrderCodesUsed").ToString(),true) %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridClientSelectColumn HeaderStyle-Width="35" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>
