<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="ImportProductsMeta.aspx.cs" Inherits="CanhCam.Web.ProductUI.ImportProductsMetaPage" %>

<%@ Register TagPrefix="Site" Assembly="CanhCam.Features.Product" Namespace="CanhCam.Web.ProductUI" %>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <Site:ProductDisplaySettings ID="displaySettings" runat="server" />
    <portal:BreadcrumbAdmin ID="breadcrumb" runat="server"
        ParentTitle="<%$Resources:ProductResources, ProductListTitle %>" ParentUrl="~/Product/AdminCP/ProductList.aspx"
        CurrentPageTitle="Import sản phẩm" CurrentPageUrl="~/Product/AdminCP/Import/ImportProducts.aspx" />
    <div class="admin-content col-md-12">
        <portal:HeadingPanel ID="heading" runat="server">
            <asp:HyperLink runat="server" SkinID="UpdateButton" Text="File mẫu" NavigateUrl="/data/sites/1/media/importproducts-meta.xls" />
        </portal:HeadingPanel>
        <asp:UpdatePanel ID="up" runat="server">
            <ContentTemplate>
                <portal:NotifyMessage ID="message" runat="server" />
                <div class="headInfo form-horizontal">
                    <div class="settingrow form-group">
                        <gb:SiteLabel ID="lblFileUpload" runat="server" ForControl="fileUpload" CssClass="settinglabel control-label col-sm-3" Text="Nhập tập tin (xls)" ResourceFile="ProductResources" />
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
                                <li>Dữ liệu ở Sheet đầu tiên & bắt đầu từ dòng 3</li>
                                <li>Ảnh đặt trong thư mục <b>/Data/Sites/1/media/dataimports/</b></li>
                                <li>Mã sản phẩm không được bỏ trống</li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div class="workplace">
                    <telerik:RadGrid ID="grid" SkinID="radGridSkin" runat="server">
                        <MasterTableView DataKeyNames="ItemNo,MetaTitle,MetaDescription,MetaKeywords,AdditionalMetaTags" AutoGenerateColumns="true" AllowSorting="false" AllowPaging="false">
                            <Columns>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
