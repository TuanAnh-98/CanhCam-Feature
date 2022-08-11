<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="AffiliateOrderList.aspx.cs" Inherits="CanhCam.Web.ProductUI.AffiliateOrderList" %>


<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:BreadcrumbAdmin ID="breadcrumb" runat="server"
        CurrentPageTitle="Nhan Vien" CurrentPageUrl="~/Product/AdminCP/PaymentMethods.aspx" />
    <div class="admin-content col-md-12">
        <portal:HeadingPanel ID="heading" runat="server">
            <asp:Button SkinID="UpdateButton" ID="btnExportExcel" Text="Export Excel" runat="server" />
            <asp:Button SkinID="UpdateButton" ID="btnUpdate" Text="Update" runat="server" />
            <asp:HyperLink SkinID="InsertButton" runat="server" ID="lnkInsert" Text="Insert" NavigateUrl='<%# SiteRoot + "/PhongBan/Admincp/NhanVienEdit.aspx"%>' />
            <asp:Button SkinID="DeleteButton" ID="btnDelete" Text="Delete" runat="server" CausesValidation="false" />
        </portal:HeadingPanel>
        <portal:NotifyMessage ID="NotifyMessage1" runat="server" />
        <asp:Panel ID="pnlSearch" CssClass="headInfo form-horizontal" DefaultButton="btnSearch" runat="server">
            <div class="settingrow form-group">
                <gb:SiteLabel ID="lblTitle" runat="server" Text="Tên sản phẩm" ForControl="txtTitle" CssClass="settinglabel control-label col-sm-3" />
                <div class="col-sm-9">
                    <div class="input-group">
                        <asp:DropDownList ID="ddlAddProduct" AutoPostBack="true" runat="server">
                    </asp:DropDownList>
                        <div class="input-group-btn">
                            <asp:Button SkinID="InsertButton" ID="btnSearch" Text="Search"
                                runat="server" CausesValidation="false" />
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <div class="workplace">
            <telerik:RadGrid ID="grid" SkinID="radGridSkin" runat="server">
                <MasterTableView DataKeyNames="affiliateID" AllowPaging="false" AllowSorting="false">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Mã đơn hàng" HeaderStyle-CssClass="tableTBar">
                            <ItemTemplate>
                                <%# Eval("OrderCode") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sản phẩm" HeaderStyle-CssClass="tableTBar">
                            <ItemTemplate>
                                <%# Eval("ProductName") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ngày đặt hàng" HeaderStyle-CssClass="tableTBar">
                            <ItemTemplate>
                                <%# Convert.ToDateTime(Eval("DateCreate")).ToString("dd/MM/yyyy") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Tổng số tiền" HeaderStyle-CssClass="tableTBar">
                            <ItemTemplate>
                                <%# CanhCam.Web.ProductUI.ProductHelper.FormatPrice((Convert.ToDecimal(Eval("Quantity")) * Convert.ToDecimal(Eval("Price"))), true) %>
                                
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Hoa hồng" HeaderStyle-CssClass="tableTBar">
                            <ItemTemplate>
                                <%# CanhCam.Web.ProductUI.ProductHelper.FormatPrice((Convert.ToDecimal(Eval("Quantity")) * Convert.ToDecimal(Eval("Price"))) * Convert.ToDecimal("0,3"), true) %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridClientSelectColumn HeaderStyle-Width="35" HeaderStyle-CssClass="tableTBar" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>