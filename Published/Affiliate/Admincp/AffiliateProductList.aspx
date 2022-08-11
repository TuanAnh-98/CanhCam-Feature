<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="AffiliateProductList.aspx.cs" Inherits="CanhCam.Web.AffiliateUI.AffiliateProductList" %>


<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:BreadcrumbAdmin ID="breadcrumb" runat="server"
        CurrentPageTitle="Nhan Vien" CurrentPageUrl="~/Product/AdminCP/PaymentMethods.aspx" />
    <div class="admin-content col-md-12">
        <portal:HeadingPanel ID="heading" runat="server">
            <asp:Button SkinID="UpdateButton" ID="btnExportExcel" Text="Export Excel" runat="server" />
            <asp:Button SkinID="UpdateButton" ID="btnUpdate" Text="Update" runat="server" />
            <asp:HyperLink SkinID="InsertButton" runat="server" ID="lnkInsert" Text="Insert" NavigateUrl='#' />
            <asp:Button SkinID="DeleteButton" ID="btnDelete" Text="Delete" runat="server" CausesValidation="false" />
        </portal:HeadingPanel>
        <portal:NotifyMessage ID="NotifyMessage1" runat="server" />
        <asp:Panel ID="pnlSearch" CssClass="headInfo form-horizontal" DefaultButton="btnSearch" runat="server">
            <div class="settingrow form-group">
                <gb:SiteLabel ID="lblTitle" runat="server" Text="Tên nhân viên" ForControl="txtTitle" CssClass="settinglabel control-label col-sm-3" />
                <div class="col-sm-9">
                    <div class="input-group">
                        <asp:TextBox ID="txtTitle" runat="server" Text="" />
                        <div class="input-group-btn">
                            <asp:Button SkinID="DefaultButton" ID="btnSearch" Text="Search"
                                runat="server" CausesValidation="false" />
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <div class="workplace">
            <telerik:RadGrid ID="grid" SkinID="radGridSkin" runat="server">
                <MasterTableView DataKeyNames="AffiliateID" AllowPaging="false" AllowSorting="false">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderStyle-Width="35" HeaderText="STT" HeaderStyle-CssClass="tableTBar">
                            <ItemTemplate>
                                <%# (grid.PageSize * grid.CurrentPageIndex) + Container.ItemIndex + 1%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Order Code" HeaderStyle-CssClass="tableTBar">
                            <ItemTemplate>
                                <%# Eval("OrderCode")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="User ID" HeaderStyle-CssClass="tableTBar">
                            <ItemTemplate>
                                <%# Eval("UserID")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Product ID" HeaderStyle-CssClass="tableTBar">
                            <ItemTemplate>
                                <%# Eval("ProductID")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date Buy" HeaderStyle-CssClass="tableTBar">
                            <ItemTemplate>
                                <%# Convert.ToDateTime(Eval("DateBuy")).ToString("dd/MM/yyyy") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-CssClass="tableTBar">
                            <ItemTemplate>
                               <asp:Label Text='<%# Convert.ToBoolean(Eval("Status")).ToString() == "True" ? "Done" : "New" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridClientSelectColumn HeaderStyle-Width="35" HeaderStyle-CssClass="tableTBar" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>
