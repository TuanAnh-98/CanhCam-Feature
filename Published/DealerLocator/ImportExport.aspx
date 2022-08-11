<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="ImportExport.aspx.cs" Inherits="CanhCam.Web.DealerUI.ImportExportPage" %>

<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:BreadcrumbAdmin ID="breadcrumb" runat="server"
        ParentTitle="<%$Resources:DealerResources, DealersListHeading %>" ParentUrl="~/DealerLocator/List.aspx"
        CurrentPageTitle="<%$Resources:DealerResources, ExcelImportExportTitle %>" CurrentPageUrl="~/DealerLocator/ImportExport.aspx" />
    <div class="admin-content col-md-12">
        <portal:HeadingPanel ID="heading" runat="server"></portal:HeadingPanel>
        <portal:NotifyMessage ID="message" runat="server" />
        <div class="workplace">
            <div class="form-horizontal">
                <div class="settingrow form-group">
                    <gb:SiteLabel ID="lblZones" runat="server" ConfigKey="ZoneLabel"
                        ForControl="ddZones" CssClass="settinglabel control-label col-sm-3" />
                    <div class="col-sm-9">
                        <asp:DropDownList ID="ddZones" runat="server" />
                    </div>
                </div>
                <div class="settingrow form-group mrt20 mrb10">
                    <h4 class="col-sm-9 col-sm-offset-3 mrt0 mrb0">Export</h4>
                </div>
                <asp:UpdatePanel ID="up" runat="server">
                    <ContentTemplate>
                        <div class="settingrow form-group">
                            <div class="col-sm-3 col-sm-offset-3">
                                <asp:DropDownList ID="ddCountry" runat="server" DataTextField="Name" DataValueField="Guid"
                                    AutoPostBack="true" AppendDataBoundItems="true" />
                            </div>
                            <div class="col-sm-3">
                                <asp:DropDownList ID="ddProvince" runat="server" DataTextField="Name" DataValueField="Guid"
                                    AutoPostBack="true" AppendDataBoundItems="true" />
                            </div>
                            <div class="col-sm-3">
                                <asp:DropDownList ID="ddDistrict" runat="server" DataTextField="Name" DataValueField="Guid"
                                    AppendDataBoundItems="true" />
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="settingrow form-group">
                    <div class="col-sm-9 col-sm-offset-3">
                        <asp:Button SkinID="ExportButton" ID="btnExport" Text="<%$Resources:DealerResources, ExcelExportButton %>" runat="server" CausesValidation="false" />
                    </div>
                </div>
                <div class="settingrow form-group mrt20 mrb10">
                    <h4 class="col-sm-9 col-sm-offset-3 mrt0 mrb0">Import</h4>
                </div>
                <div class="settingrow form-group">
                    <gb:SiteLabel id="lblFileUpload" runat="server" ForControl="fileUpload" CssClass="settinglabel control-label col-sm-3" Text="Nhập tập tin (xsl)" ResourceFile="ProductResources" />
                    <div class="col-sm-9">
                        <telerik:RadAsyncUpload ID="fileUpload" SkinID="radAsyncUploadSkin" MultipleFileSelection="Disabled"
                            AllowedFileExtensions="xls" runat="server" />
                        <asp:Button SkinID="DefaultButton" ID="btnContinue" ValidationGroup="ImportCoupon" Text="<%$Resources:DealerResources, ExcelImportContinueButton %>" runat="server" />
                        <asp:Button SkinID="DefaultButton" ID="btnImport" Visible="false" Text="<%$Resources:DealerResources, ExcelImportButton %>" runat="server" CausesValidation="false" />
                    </div>
                </div>
            </div>
            <telerik:RadGrid ID="grid" Visible="false" SkinID="radGridSkin" runat="server">
                <MasterTableView AllowPaging="false" AllowSorting="false">
                    <Columns>
                        <telerik:GridBoundColumn HeaderText="ItemID" DataField="ItemID" UniqueName="ItemID" />
                        <telerik:GridBoundColumn HeaderText="Name" DataField="Name" UniqueName="Name" />
                        <telerik:GridBoundColumn HeaderText="Phone" DataField="Phone" UniqueName="Phone" />
                        <telerik:GridBoundColumn HeaderText="Address" DataField="Address" UniqueName="Address" />
                        <telerik:GridBoundColumn HeaderText="CountryGuid" DataField="CountryGuid" UniqueName="CountryGuid" />
                        <telerik:GridBoundColumn HeaderText="ProvinceGuid" DataField="ProvinceGuid" UniqueName="ProvinceGuid" />
                        <telerik:GridBoundColumn HeaderText="DistrictGuid" DataField="GeoZoneGuid" UniqueName="GeoZoneGuid" />
                        <telerik:GridBoundColumn HeaderText="Latitude" DataField="Latitude" UniqueName="Latitude" />
                        <telerik:GridBoundColumn HeaderText="Longitude" DataField="Longitude" UniqueName="Longitude" />
                        <telerik:GridClientSelectColumn HeaderStyle-Width="35" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>