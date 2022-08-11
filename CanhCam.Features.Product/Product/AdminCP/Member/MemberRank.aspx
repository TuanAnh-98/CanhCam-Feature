<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="MemberRank.aspx.cs" Inherits="CanhCam.Web.ProductUI.MemberRankPage" %>

<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:BreadcrumbAdmin ID="breadcrumb" runat="server"
        CurrentPageTitle="<%$Resources:ProductResources, MemberRankAdminTitle %>"
        CurrentPageUrl="~/Product/AdminCP/Member/MemberRank.aspx" />
    <div class="admin-content">
        <portal:NotifyMessage ID="message" runat="server" />
        <portal:HeadingPanel ID="heading" runat="server">
            <asp:HyperLink SkinID="InsertButton" ID="lnkInsert" Text="<%$Resources:Resource, InsertButton %>"
                runat="server" />
            <asp:Button SkinID="DeleteButton" ID="btnDelete" Text="<%$Resources:Resource, DeleteSelectedButton %>"
                runat="server" CausesValidation="false" />
        </portal:HeadingPanel>

        <div class="row">
            <div class="col-12">
                <div class="workplace admin-content-bg-white">
                    <telerik:RadGrid ID="grid" SkinID="radGridSkin" runat="server">
                        <MasterTableView DataKeyNames="Id, RankOrder" AllowSorting="false">
                            <Columns>
                                <telerik:GridTemplateColumn HeaderStyle-Width="35"
                                    HeaderText="<%$Resources:Resource, RowNumber %>">
                                    <ItemTemplate>
                                        <%# (grid.PageSize * grid.CurrentPageIndex) + Container.ItemIndex + 1%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn
                                    HeaderText="<%$Resources:ProductResources, MemberRankNameLabel %>">
                                    <ItemTemplate>
                                        <%# Eval("Name") %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="<%$Resources:ProductResources, MemberRankDescriptionLabel %>">
                                    <ItemTemplate>
                                        <%# Eval("Description") %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="<%$Resources:ProductResources, MemberRankPointLabel %>">
                                    <ItemTemplate>
                                        <%# Eval("Point") %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="<%$Resources:ProductResources, MemberRankDiscountPercentLabel %>">
                                    <ItemTemplate>
                                        <%# Eval("DiscountPercent") %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:HyperLink CssClass="cp-link btn btn-default bg-teal" ID="EditLink"
                                            runat="server" Text="<%$Resources:ProductResources, MemberRankEditLink %>"
                                            NavigateUrl='<%# SiteRoot + "/Product/AdminCP/Member/MemberRankEdit.aspx?ID=" + Eval("Id") %>'>
										</asp:HyperLink>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridClientSelectColumn HeaderStyle-Width="35" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
            </div>
        </div>
    </div>
</asp:Content>