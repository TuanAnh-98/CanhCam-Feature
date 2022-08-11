<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="DeleteProductsTool.aspx.cs" Inherits="CanhCam.Web.ProductUI.DeleteProductsToolPage" %>

<%@ Register TagPrefix="Site" Assembly="CanhCam.Features.Product" Namespace="CanhCam.Web.ProductUI" %>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <Site:ProductDisplaySettings ID="displaySettings" runat="server" />
    <portal:BreadcrumbAdmin ID="breadcrumb" runat="server"
        ParentTitle="Xóa sản phẩm hàng loạt" ParentUrl="~/Product/AdminCP/Import/DeleteProductsTool.aspx"
        CurrentPageTitle="Import sản phẩm" CurrentPageUrl="~/Product/AdminCP/Import/ImportProducts.aspx" />
    <div class="admin-content col-md-12">
        <portal:HeadingPanel ID="heading" runat="server">
        </portal:HeadingPanel>
        <portal:NotifyMessage ID="message" runat="server" />
        <div class="headInfo form-horizontal">
            <div class="settingrow form-group">
                <gb:SiteLabel ID="lblFileUpload" runat="server" ForControl="fileUpload" CssClass="settinglabel control-label col-sm-3" Text="Nhập tập tin (xls)" ResourceFile="ProductResources" />
                <div class="col-sm-9">
                    <telerik:RadAsyncUpload ID="fileUpload" SkinID="radAsyncUploadSkin" MultipleFileSelection="Disabled"
                        AllowedFileExtensions="xls" runat="server" />
                    <asp:CheckBox ID="chkDeleteOver" Text="Xóa sản phẩm khỏi hệ thống (xóa không lưu lại 'Dữ liệu đã xóa')" runat="server" />
                </div>
            </div>
            <div class="settingrow form-group">
                <div class="col-sm-9 col-sm-offset-3">
                    <asp:Button SkinID="DefaultButton" ID="btnGetData" Text="Delete" runat="server" />
                </div>
            </div>
        </div>
        <div class="workplace">
            <asp:Literal runat="server" ID="litResult" />
        </div>
    </div>
</asp:Content>
