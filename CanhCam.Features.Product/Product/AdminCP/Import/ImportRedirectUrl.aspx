<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="ImportRedirectUrl.aspx.cs" Inherits="CanhCam.Web.ProductUI.ImportRedirectUrlPage" %>

<%@ Register TagPrefix="Site" Assembly="CanhCam.Features.Product" Namespace="CanhCam.Web.ProductUI" %>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <Site:ProductDisplaySettings ID="displaySettings" runat="server" />
    <portal:BreadcrumbAdmin ID="breadcrumb" runat="server"
        CurrentPageTitle="Import Redirect Urls" CurrentPageUrl="~/Product/AdminCP/Import/ImportRedirectUrl.aspx" />
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
                </div>
            </div>
            <div class="settingrow form-group">
                <div class="col-sm-9 col-sm-offset-3">
                    <asp:Button SkinID="DefaultButton" ID="btnGetData" Text="Import" runat="server" />
                </div>
            </div>
        </div>
        <div class="workplace">
            <asp:Literal runat="server" ID="litResult" />
        </div>
    </div>
</asp:Content>
