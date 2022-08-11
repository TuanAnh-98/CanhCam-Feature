<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="AffiliateAchieveTargets.aspx.cs" Inherits="CanhCam.Web.ProductUI.AffiliateAchieveTargets" %>


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
                <MasterTableView DataKeyNames="productID" AllowPaging="false" AllowSorting="false">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="STT" HeaderStyle-CssClass="tableTBar">
                            <ItemTemplate>
                                <%# (grid.PageSize * grid.CurrentPageIndex) + Container.ItemIndex + 1%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Email" HeaderStyle-CssClass="tableTBar">
                            <ItemTemplate>
                                <%# Eval("Email") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Tên" HeaderStyle-CssClass="tableTBar">
                            <ItemTemplate>
                                <%# Eval("LastName") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Số đơn hàng" HeaderStyle-CssClass="tableTBar">
                            <ItemTemplate>
                                <%# Eval("TotalOrder") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Tổng tiền" HeaderStyle-CssClass="tableTBar">
                            <ItemTemplate>
                                <%# Eval("TotalMoney") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridClientSelectColumn HeaderStyle-Width="35" HeaderStyle-CssClass="tableTBar" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>
