<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="Countries.aspx.cs" Inherits="CanhCam.Web.ProductUI.CountriesPage" %>

<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:BreadcrumbAdmin ID="breadcrumb" runat="server"
        CurrentPageTitle="ViettelPost Province" CurrentPageUrl="~/Product/AdminCP/Location/Countries.aspx" />
    <div class="admin-content col-md-12">
        <portal:HeadingPanel ID="heading" runat="server">
            <asp:Button SkinID="UpdateButton" ID="btnUpdateLocation" Text="Cập nhật" runat="server" />
        </portal:HeadingPanel>
        <portal:NotifyMessage ID="message" runat="server" />
        <asp:Panel ID="pnlSearch" CssClass="headInfo admin-content-bg-white form-horizontal"
            runat="server">
            <div class="settingrow form-group row align-items-center">
                <gb:SiteLabel ID="lblKeyword" runat="server" Text="Tỉnh thành" ForControl="txtKeyword"
                    CssClass="settinglabel control-label col-sm-3" />
                <div class="col-sm-9">
                    <asp:DropDownList runat="server" ID="ddlPovince" AppendDataBoundItems="true" AutoPostBack="true" />
                </div>
            </div>
        </asp:Panel>
        <div class="workplace">
            <telerik:RadGrid ID="grid" SkinID="radGridSkin" runat="server">
                <MasterTableView DataKeyNames="Guid,ItemID">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderStyle-Width="35" HeaderText="<%$Resources:Resource, RowNumber %>">
                            <ItemTemplate>
                                <%# (grid.PageSize * grid.CurrentPageIndex) + Container.ItemIndex + 1%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Địa điểm">
                            <ItemTemplate>
                                <%# Eval("Name").ToString() %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Api Code">
                            <ItemTemplate>
                                <asp:TextBox ID="txtApiCode" Text='<%# Eval("ApiCode") %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>
