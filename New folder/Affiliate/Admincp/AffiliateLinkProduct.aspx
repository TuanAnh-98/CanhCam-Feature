<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="AffiliateLinkProduct.aspx.cs" Inherits="CanhCam.Web.AffiliateUI.AffiliateLinkProduct" %>


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
                <MasterTableView DataKeyNames="Id" AllowPaging="false" AllowSorting="false">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Sản phẩm" HeaderStyle-CssClass="tableTBar">
                            <ItemTemplate>
                                <a class="nhanvien-img" href="/Data/SiteImages/imagePhongBan/<%# Eval("Image")%>">
                                    <img width="100" height="100" src="/Data/SiteImages/imagePhongBan/<%# Eval("Image")%>" class="CalloutRightPhoto" />
                                </a>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Liên kết (Url)" HeaderStyle-CssClass="tableTBar">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtUrl" Text='<%# SiteRoot + "/PhongBan/Admincp/NhanVienEdit.aspx?proid="  +  Eval("Id") + "&affid=" +  SiteUtils.GetCurrentSiteUser().UserId%>' ReadOnly="true" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridClientSelectColumn HeaderStyle-Width="35" HeaderStyle-CssClass="tableTBar" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>
