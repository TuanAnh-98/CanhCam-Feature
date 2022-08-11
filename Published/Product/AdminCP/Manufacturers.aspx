<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="Manufacturers.aspx.cs" Inherits="CanhCam.Web.ProductUI.ManufacturersPage" %>

<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:BreadcrumbAdmin ID="breadcrumb" runat="server"
        CurrentPageTitle="<%$Resources:ProductResources, ManufacturersTitle %>" CurrentPageUrl="~/Product/AdminCP/Manufacturers.aspx" />
    <div class="admin-content col-md-12">
        <portal:HeadingPanel ID="heading" runat="server">
            <asp:Button SkinID="ExportButton" ID="btnExport" OnClick="btnExport_Click"
                Text="<%$Resources:ProductResources, OrderExportButton %>" runat="server" CausesValidation="false" />
            <asp:Button SkinID="UpdateButton" ID="btnUpdate" Text="<%$Resources:ProductResources, ProductUpdateButton %>" runat="server" />
            <asp:HyperLink SkinID="InsertButton" ID="lnkInsert" Text="<%$Resources:Resource, InsertButton %>" runat="server" />
            <asp:Button SkinID="DeleteButton" ID="btnDelete" Text="<%$Resources:Resource, DeleteSelectedButton %>" runat="server" CausesValidation="false" />
        </portal:HeadingPanel>
        <portal:NotifyMessage ID="message" runat="server" />
        <div class="workplace">
            <asp:Panel ID="pnlSearch" DefaultButton="btnSearch" CssClass="headInfo admin-content-bg-white form-horizontal"
                runat="server">
                <div class="settingrow form-group">
                    <gb:SiteLabel ID="lblZones" runat="server" ConfigKey="ZoneLabel"
                        ForControl="ddZones" CssClass="settinglabel control-label col-sm-3" />
                    <div class="col-sm-9">
                        <asp:DropDownList ID="ddZones" AutoPostBack="false" runat="server" />
                    </div>
                </div>
                <div class="settingrow form-group row align-items-center">
                    <gb:SiteLabel ID="lblKeyword" runat="server" ConfigKey="OrderKeywordLabel"
                        ResourceFile="ProductResources" ForControl="txtKeyword"
                        CssClass="settinglabel control-label col-sm-3" />
                    <div class="col-sm-9">
                        <asp:TextBox ID="txtKeyword" placeholder="Tên thương hiệu"
                            runat="server" MaxLength="255" CssClass="form-control" />
                    </div>
                </div>
                <div class="settingrow form-group row align-items-center">
                    <div class="offset-sm-3 col-sm-9">
                        <asp:Button SkinID="DefaultButton" ID="btnSearch"
                            Text="Tìm kiếm" runat="server"
                            CausesValidation="false" />
                    </div>
                </div>
            </asp:Panel>
            <telerik:RadGrid ID="grid" SkinID="radGridSkin" runat="server">
                <MasterTableView DataKeyNames="ManufacturerId,DisplayOrder" AllowSorting="false">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderStyle-Width="35" HeaderText="<%$Resources:Resource, RowNumber %>" AllowFiltering="false">
                            <ItemTemplate>
                                <%# (grid.PageSize * grid.CurrentPageIndex) + Container.ItemIndex + 1%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="<%$Resources:ProductResources, ManufacturerNameLabel %>">
                            <ItemTemplate>
                                <%# Eval("Name") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="100" HeaderText="<%$Resources:ProductResources, ManufacturerDisplayOrderLabel %>">
                            <ItemTemplate>
                                <asp:TextBox ID="txtDisplayOrder" SkinID="NumericTextBox" MaxLength="4" Text='<%# Eval("DisplayOrder") %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="50">
                            <ItemTemplate>
                                <asp:HyperLink CssClass="cp-link" ID="EditLink" runat="server"
                                    Text="<%$Resources:ProductResources, ProductEditLink %>" NavigateUrl='<%# SiteRoot + "/Product/AdminCP/ManufacturerEdit.aspx?ManufacturerID=" + Eval("ManufacturerId") %>'>
                                </asp:HyperLink>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridClientSelectColumn HeaderStyle-Width="35" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>
