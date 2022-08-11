<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="RestrictedShippingList.aspx.cs" Inherits="CanhCam.Web.ProductUI.RestrictedShippingListPage" %>

<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:BreadcrumbAdmin ID="breadcrumb" runat="server"
        CurrentPageTitle="Địa điểm hạn chế" CurrentPageUrl="~/Product/AdminCP/RestrictedShippingList.aspx" />
    <div class="admin-content col-md-12">
        <portal:HeadingPanel ID="heading" runat="server">
            <asp:Button SkinID="DeleteButton" ID="btnDelete" Text="<%$Resources:Resource, DeleteSelectedButton %>" runat="server" CausesValidation="false" />
        </portal:HeadingPanel>
        <portal:NotifyMessage ID="message" runat="server" />
        <div class="workplace">
            <div class="row">
                <div class="col-md-12 col-xs-12">
                    <div class="form-horizontal">
                        <div class="settingrow form-group">
                            <gb:SiteLabel ID="lblFileUpload" runat="server" ForControl="fileUpload" CssClass="settinglabel control-label col-sm-3" Text="Nhập tập tin (xls)" ResourceFile="ProductResources" />
                            <div class="col-sm-9">
                                <telerik:RadAsyncUpload ID="fileUpload" SkinID="radAsyncUploadSkin" MultipleFileSelection="Disabled"
                                    AllowedFileExtensions="xls" runat="server" />
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
                                    <li>Dữ liệu ở Sheet đầu tiên & bắt đầu từ dòng 2</li>
                                    <li>Mã địa điểm không được bỏ trống</li> 
                                    <li>
                                        <asp:Button runat="server" ID="btnExportData" Text="Export" SkinID="DefaultButton" />
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <telerik:RadGrid ID="grid" SkinID="radGridSkin" runat="server">
                <MasterTableView DataKeyNames="RowId,GeoZoneGuid,Weight,OrderTotal,ShippingMethodIds,PaymentMethodIds,StoreId" AllowSorting="false">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Địa điểm">
                            <ItemTemplate>
                                <%# Eval("GeoZoneName") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Trọng lượng">
                            <ItemTemplate>
                                <%# Eval("Weight") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Phương thức vận chuyển khả dụng">
                            <ItemTemplate>
                                <%# Eval("ShippingMethodIds") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Phương thức thanh toán khả dụng">
                            <ItemTemplate>
                                <%# Eval("PaymentMethodIds") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Cửa hàng" UniqueName="Store">
                            <ItemTemplate>
                                <%# FormatStoreName(Convert.ToInt32(Eval("StoreId"))) %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridClientSelectColumn HeaderStyle-Width="35" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>
