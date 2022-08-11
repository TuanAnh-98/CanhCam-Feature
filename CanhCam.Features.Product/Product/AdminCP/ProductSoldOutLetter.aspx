<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="ProductSoldOutLetter.aspx.cs" Inherits="CanhCam.Web.ProductUI.ProductSoldOutLetterPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:BreadcrumbAdmin ID="breadcrumb" runat="server"
        CurrentPageTitle="Danh sách thông báo khi có hàng" CurrentPageUrl="~/Product/Admincp/ProductSoldOutLetter.aspx" />
    <div class="admin-content col-md-12">
        <portal:HeadingPanel ID="heading" runat="server">
            <asp:Button SkinID="UpdateButton" ID="btnUpdate" Text="Cập nhật"
                runat="server" />
            <asp:Button SkinID="UpdateButton" ID="btnSendMail" Text="Gửi mail có hàng cho khách"
                runat="server" />
            <asp:Button SkinID="UpdateButton" ID="btnExport" Text="Export Excel"
                runat="server" />
            <asp:Button SkinID="DeleteButton" ID="btnDetele" Text="Xóa"
                runat="server" />
        </portal:HeadingPanel>
        <portal:NotifyMessage ID="message" runat="server" />
        <asp:Panel ID="pnlSearch" DefaultButton="btnSearch" CssClass="headInfo" runat="server">
            <div class="form-horizontal">
                <div class="settingrow form-group">
                    <gb:SiteLabel ID="lblStartDate" runat="server" Text="Từ ngày" ForControl="dpStartDate" CssClass="control-label col-sm-3" />
                    <div class="col-sm-9">
                        <gb:DatePickerControl ID="dpStartDate" runat="server" SkinID="news"></gb:DatePickerControl>
                    </div>
                </div>
                <div class="settingrow form-group">
                    <gb:SiteLabel ID="lblEndDate" runat="server" Text="Đến ngày"
                        ForControl="dpEndDate" CssClass="control-label col-sm-3" />
                    <div class="col-sm-9">
                        <gb:DatePickerControl ID="dpEndDate" runat="server" SkinID="news"></gb:DatePickerControl>
                    </div>
                </div>
                <div class="settingrow form-group">
                    <gb:SiteLabel ID="lblTitle" runat="server" Text="Keywrod" ForControl="txtTitle" CssClass="control-label col-sm-3" />
                    <div class="col-sm-9">
                        <div class="form-inline">
                            <div class="col-md-12">
                                <div class="form-group" style="width: 50%;">
                                    <asp:TextBox ID="txtTitle" runat="server" MaxLength="255" placeholder="Tên hoặc cmnd hoặc số điện thoại" />
                                </div>
                                <div class="form-group">
                                    <asp:Button SkinID="DefaultButton" ID="btnSearch" Text="Tìm kiếm"
                                        runat="server" CausesValidation="false" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <div class="workplace">
            <div class="settingrow row align-items-center">
                <div class="col-lg-4">
                    <div class="order-processing"><span style="width: 40px; height: 10px; min-width: 20px; display: inline-block; min-height: 20px; background-color: #ffc10745;"></span>-  Đơn chưa đủ hàng khách yêu cầu</div>
                </div>
                <div class="col-lg-4">
                    <div class="order-processing"><span style="width: 40px; height: 10px; min-width: 20px; display: inline-block; min-height: 20px; background-color: #28a7459e;"></span>- Đơn đủ hàng khách yêu cầu</div>
                </div>
                <div class="col-lg-4">
                    <div class="order-processing"><span style="width: 40px; height: 10px; min-width: 20px; display: inline-block; min-height: 20px; background-color: #ff0707;"></span>- Sản phẩm đã xóa</div>
                </div>
            </div>
            <telerik:RadGrid ID="grid" SkinID="radGridSkin" runat="server">
                <MasterTableView DataKeyNames="RowId,ProductID,Quantity,IsContacted">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderStyle-Width="35" HeaderText="<%$Resources:Resource, RowNumber %>" AllowFiltering="false">
                            <ItemTemplate>
                                <%# (grid.PageSize * grid.CurrentPageIndex) + Container.ItemIndex + 1%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--  <telerik:GridTemplateColumn HeaderText="Họ và tên">
                            <ItemTemplate>
                                <%# Eval("FullName")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                        <telerik:GridTemplateColumn HeaderText="Số điện thoại">
                            <ItemTemplate>
                                <%# Eval("Phone")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Email">
                            <ItemTemplate>
                                <%# Eval("Email")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Tên Sản phẩm">
                            <ItemTemplate>
                                <%# Eval("ProductName") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Số lượng">
                            <ItemTemplate>
                                <%# Eval("Quantity") %>
                            </ItemTemplate> 
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Số lượng tồn của sản phẩm">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="litProductStockQuantity" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Đường dẫn đăng ký">
                            <ItemTemplate>
                                <a href='<%# Eval("Url") %>' title='<%# Eval("ProductName") %>' target="_blank">Xem</a>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ngày Đăng ký">
                            <ItemTemplate>
                                <%# Eval("CreateDate").ToString() %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Tình trạng">
                            <ItemTemplate>
                                <%# GetStatus(Convert.ToInt32(Eval("Status").ToString())) %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Đã liên hệ">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="cbIsContacted"  Checked='<%# Convert.ToBoolean(Eval("IsContacted")) %>'/> 
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridClientSelectColumn HeaderStyle-Width="35" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
    <style type="text/css">
        .RadGrid_Bootstrap .rgAltRow>td
        {
            background-color: none;
        }
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />