<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="AffiliateList.aspx.cs" Inherits="CanhCam.Web.AffiliateUI.AffiliateList" %>

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
                <MasterTableView DataKeyNames="UserID" AllowPaging="false" AllowSorting="false">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Mã nhân viên" HeaderStyle-CssClass="tableTBar">
                            <ItemTemplate>
                                <%# Eval("Name")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="<%$Resources:ProductResources, OrderFirstNameLabel %>" HeaderStyle-CssClass="tableTBar">
                            <ItemTemplate>
                                <%# Eval("LoginName")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ngày sinh" HeaderStyle-CssClass="tableTBar">
                            <ItemTemplate>
                                <%# Convert.ToDateTime(Eval("DateCreated")).ToString("dd/MM/yyyy") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Nơi sinh" HeaderStyle-CssClass="tableTBar">
                            <ItemTemplate>
                                <%# Eval("Email")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridClientSelectColumn HeaderStyle-Width="35" HeaderStyle-CssClass="tableTBar" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>