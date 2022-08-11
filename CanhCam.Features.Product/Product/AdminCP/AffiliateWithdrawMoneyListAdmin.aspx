<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="AffiliateWithdrawMoneyListAdmin.aspx.cs" Inherits="CanhCam.Web.ProductUI.AffiliateWithdrawMoneyListAdmin" %>


<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:BreadcrumbAdmin ID="breadcrumb" runat="server"
        CurrentPageTitle="Nhan Vien" CurrentPageUrl="~/Product/AdminCP/PaymentMethods.aspx" />
    <div class="admin-content col-md-12">
        <portal:HeadingPanel ID="heading" runat="server">
            <asp:Button SkinID="UpdateButton" ID="btnExportExcel" Text="Export Excel" runat="server" />
            <asp:Button SkinID="UpdateButton" ID="btnUpdate" Text="Update" runat="server" />
            <asp:Button SkinID="DeleteButton" ID="btnDelete" Text="Delete" runat="server" CausesValidation="false" />
        </portal:HeadingPanel>
        <portal:NotifyMessage ID="NotifyMessage1" runat="server" />
        <div class="workplace">
            <telerik:RadGrid ID="grid" SkinID="radGridSkin" runat="server">
                <MasterTableView DataKeyNames="AffId, AffGuid, Status" AllowPaging="false" AllowSorting="false">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Ngày yêu cầu" HeaderStyle-CssClass="tableTBar">
                            <ItemTemplate>
                                <%# Convert.ToDateTime(Eval("DateRequire")).ToString("dd/MM/yyyy") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Số tiền" HeaderStyle-CssClass="tableTBar">
                            <ItemTemplate>
                                <%# Eval("WithdrawMoney") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Trạng thái" HeaderStyle-CssClass="tableTBar">
                            <ItemTemplate>
                                <asp:Label Text='<%# Convert.ToBoolean(Eval("Status")).ToString() == "True" ? "Đã thanh toán" : "Chưa thanh toán" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ngày thanh toán" HeaderStyle-CssClass="tableTBar">
                            <ItemTemplate>
                                <asp:Label Text='<%# Convert.ToDateTime(Eval("DateRespond")).ToString("dd/MM/yy") == "01/01/0001" ? "" :  CanhCam.Web.ProductUI.ProductHelper.FormatDate(Eval("DateRespond"),SiteUtils.GetUserTimeZone(), SiteUtils.GetUserTimeOffset(), "dd/MM/yyyy") %>' runat="server" />   
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Xác nhận thanh toán" HeaderStyle-CssClass="tableTBar">
                            <ItemTemplate>
                                <asp:Label runat="server" Visible="true" Text="Thanh toán đã được thực hiện" ID="lbPaymentStatus"/>
                                <asp:Button runat="server" Visible="false" CommandName="ProcessPayment" ID="btnPayment" Text="Xác nhận" SkinID="UpdateButton" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridClientSelectColumn HeaderStyle-Width="35" HeaderStyle-CssClass="tableTBar" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>