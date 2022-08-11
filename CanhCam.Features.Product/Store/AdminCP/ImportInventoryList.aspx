<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="ImportInventoryList.aspx.cs" Inherits="CanhCam.Web.StoreUI.ImportInventoryListPage" %>

<%@ Register TagPrefix="Site" Assembly="CanhCam.Features.Product" Namespace="CanhCam.Web.ProductUI" %>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <Site:ProductDisplaySettings ID="displaySettings" runat="server" />
    <portal:BreadcrumbAdmin ID="breadcrumb" runat="server"
        CurrentPageTitle="<%$Resources:StoreResources, ImportInventoryListTitle %>" CurrentPageUrl="~Store/AdminCP/ImportInventoryList.aspx" />

    <div class="admin-content col-md-12 admin-content-bg-white">
        <portal:HeadingPanel ID="heading" runat="server"></portal:HeadingPanel>
        <asp:UpdatePanel ID="up" runat="server">
            <ContentTemplate>
                <portal:NotifyMessage ID="message" runat="server" />
                <div class="headInfo form-horizontal">
                    <div class="settingrow form-group row align-items-center">
                        <gb:SiteLabel ID="lblStoreFilter" runat="server" ConfigKey="InventoryStoreLabel"
                            ResourceFile="StoreResources" ForControl="ddStore"
                            CssClass="settinglabel control-label col-sm-3" />
                        <div class="col-sm-9">
                            <asp:DropDownList ID="ddStore" runat="server" DataTextField="Name" DataValueField="StoreID" AppendDataBoundItems="true" CssClass="form-control" />
                        </div>
                    </div>
                    <div class="settingrow form-group">
                        <gb:SiteLabel ID="lblFileUpload" runat="server" ForControl="fileUpload" CssClass="settinglabel control-label col-sm-3"
                            Text="Nhập tập tin (xls)" ResourceFile="ProductResources" />
                        <div class="col-sm-9">
                            <telerik:RadAsyncUpload ID="fileUpload" SkinID="radAsyncUploadSkin" MultipleFileSelection="Disabled"
                                AllowedFileExtensions="xls" runat="server" />
                        </div>
                    </div>
                    <div class="settingrow form-group">
                        <div class="col-sm-9 col-sm-offset-3">
                            <asp:Button SkinID="DefaultButton" ID="btnGetData" Text="Lấy dữ liệu" runat="server" />
                            <asp:Button SkinID="DefaultButton" ID="btnImport" Visible="false" Text="Import" runat="server" />
                        </div>
                    </div>
                    <div class="settingrow form-group">
                        <div class="col-sm-offset-3 col-sm-9">
                            <h4>Lưu ý file dữ liệu</h4>
                            <ul class="import-tips">
                                <li>Không được bỏ trống hàng</li>
                                <li>File mẫu có thể dùng file "export tồn kho" bên trang <a href="/Store/AdminCP/InventoryList.aspx">danh sách hàng tồn kho </a></li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div class="workplace">
                    <telerik:RadGrid ID="grid" SkinID="radGridSkin" runat="server">
                        <MasterTableView DataKeyNames="ProductID,ProductCode,ProductName,StoreID,StoreName,APICode,Quantity,Exists" AutoGenerateColumns="true" AllowSorting="false" AllowPaging="false">
                            <Columns>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>