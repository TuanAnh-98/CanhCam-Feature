<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="ImportNews.aspx.cs" Inherits="CanhCam.Web.NewsUI.ImportNewsPage" %>

<%@ Register TagPrefix="Site" Assembly="CanhCam.Features.News" Namespace="CanhCam.Web.NewsUI" %>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <Site:NewsDisplaySettings ID="displaySettings" runat="server" />
    <portal:BreadcrumbAdmin ID="breadcrumb" runat="server"
        ParentTitle="<%$Resources:NewsResources, NewsList %>" ParentUrl="~/News/AdminCP/NewsList.aspx"
        CurrentPageTitle="Import bài viết" CurrentPageUrl="~/News/AdminCP/ImportNewss.aspx" />
    <div class="admin-content col-md-12">
        <portal:HeadingPanel ID="heading" runat="server"></portal:HeadingPanel>
        <asp:UpdatePanel ID="up" runat="server">
            <ContentTemplate>
                <portal:NotifyMessage ID="message" runat="server" />
                <div class="headInfo form-horizontal">
                    <div class="settingrow form-group">
                        <gb:SiteLabel ID="lblZone" runat="server" ForControl="ddlZone" CssClass="settinglabel control-label col-sm-3" Text="Chuyên mục" ResourceFile="NewsResources" />
                        <div class="col-sm-9">
                            <asp:DropDownList ID="ddZones" runat="server"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="settingrow form-group">
                        <gb:SiteLabel ID="lblFileUpload" runat="server" ForControl="fileUpload" CssClass="settinglabel control-label col-sm-3" Text="Nhập tập tin (xls)" ResourceFile="NewsResources" />
                        <div class="col-sm-9">
                            <telerik:RadAsyncUpload ID="fileUpload" SkinID="radAsyncUploadSkin" MultipleFileSelection="Disabled"
                                AllowedFileExtensions="xls" runat="server" />
                            <asp:CheckBox ID="chkOverride" Text="Cập nhật bài viết nếu đã tồn tại? (dựa vào Tên bài viết)" runat="server" />
                            <br />
                            <asp:CheckBox ID="chkOverrideImage" Text="Xóa những hình cũ khi cập nhật hình ảnh" runat="server" />
                            <br />
                            <asp:CheckBox ID="chkDowloadImageInContent" Text="Tải hình ảnh trong content về server" runat="server" />
                        </div>
                    </div>
                    <div class="settingrow form-group">
                        <div class="col-sm-9 col-sm-offset-3">
                            <asp:Button SkinID="DefaultButton" ID="btnImport" Text="Import" runat="server" />
                        </div>
                    </div>
                    <div class="settingrow form-group">
                        <div class="col-sm-offset-3 col-sm-9">
                            <h4>Lưu ý file dữ liệu</h4>
                            <ul class="import-tips">
                                <li>Dữ liệu ở Sheet đầu tiên & bắt đầu từ dòng 4</li>
                                <li>Ảnh đặt trong thư mục <b>/Data/Sites/1/media/dataimports/news/</b></li>
                                <li>Tên bài viết không được bỏ trống</li>
                            </ul>
                        </div>
                    </div>
                </div>
                <%--  <div class="workplace">
                    <telerik:RadGrid ID="grid" SkinID="radGridSkin" runat="server">
                        <MasterTableView DataKeyNames="Title,SubTitle,Url,BriefContent,FullContent,Picture,Pictures,StartDate" AutoGenerateColumns="true" AllowSorting="false" AllowPaging="false">
                            <Columns>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>--%>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>