<%@ Page Language="c#" CodeBehind="OrderAddNewProductDialog.aspx.cs" MasterPageFile="~/App_MasterPages/DialogMaster.Master"
    AutoEventWireup="false" Inherits="CanhCam.Web.ProductUI.OrderAddNewProductDialog" %>

<%@ Import Namespace="CanhCam.Web.ProductUI" %>
<asp:Content ContentPlaceHolderID="phMain" ID="MPContent" runat="server">
    <div class="admin-content col-md-12">
        <portal:HeadingControl ID="heading" Text="<%$Resources:ProductResources, OrderAddNewProductTitle %>" runat="server" />
        <asp:UpdatePanel ID="up" runat="server">
            <ContentTemplate>
                <portal:NotifyMessage ID="message" runat="server" />
                <asp:Panel ID="pnlSearch" DefaultButton="btnSearch" CssClass="headInfo form-horizontal" runat="server">
                    <div class="settingrow form-group">
                        <gb:SiteLabel ID="lblZones" runat="server" ConfigKey="ZoneLabel"
                            ForControl="ddZones" CssClass="settinglabel control-label col-sm-3" />
                        <div class="col-sm-9">
                            <asp:DropDownList ID="ddZones" Width="100%" AutoPostBack="false" runat="server" />
                        </div>
                    </div>
                    <div class="settingrow form-group">
                        <gb:SiteLabel ID="lblTitle" runat="server" ConfigKey="ProductNameLabel"
                            ResourceFile="ProductResources" ForControl="txtTitle" CssClass="settinglabel control-label col-sm-3" />
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtTitle" runat="server" MaxLength="255" />
                        </div>
                    </div>
                    <div class="settingrow form-group">
                        <div class="col-sm-9 col-sm-offset-3">
                            <asp:Button SkinID="DefaultButton" ID="btnSearch" Text="<%$Resources:ProductResources, ProductSearchButton %>"
                                runat="server" CausesValidation="false" />
                        </div>
                    </div>
                </asp:Panel>
                <telerik:RadGrid ID="grid" SkinID="radGridSkin" runat="server">
                    <MasterTableView DataKeyNames="ProductId" AllowPaging="false">
                        <Columns>
                            <telerik:GridTemplateColumn HeaderStyle-Width="50" HeaderText="<%$Resources:ProductResources, ProductPictureHeading %>">
                                <ItemTemplate>
                                    <portal:MediaElement ID="ml" runat="server" Width="40" FileUrl='<%# ProductHelper.GetImageFilePath(siteSettings.SiteId, Convert.ToInt32(Eval("ProductId")), Eval("ImageFile").ToString(), Eval("ThumbnailFile").ToString()) %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Sản phẩm">
                                <ItemTemplate>
                                    <%# Eval("Code") %>
                                    <div>
                                        <asp:HyperLink ID="Title" runat="server" Text='<%# Eval("Title").ToString() %>'
                                            NavigateUrl='<%# ProductHelper.FormatProductUrl(Eval("Url").ToString(), Convert.ToInt32(Eval("ProductId")), Convert.ToInt32(Eval("ZoneID")))  %>'>
                                        </asp:HyperLink>
                                    </div>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderStyle-Width="100" HeaderText="<%$Resources:ProductResources, ProductPriceLabel %>" UniqueName="Price">
                                <ItemTemplate>
                                    <%# ProductHelper.FormatPrice(Convert.ToDecimal(Eval("Price"))) %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="AddNew">
                                <ItemTemplate>
                                    <asp:Button ID="btnAddNew" SkinID="InsertButton" CommandName="AddNew" CommandArgument='<%#Eval("ProductId").ToString()%>' Text="<%$Resources:Resource, InsertButton %>" runat="server" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </ContentTemplate>
        </asp:UpdatePanel>
        <portal:SessionKeepAliveControl ID="ka1" runat="server" />
    </div>
</asp:Content>