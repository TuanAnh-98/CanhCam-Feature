<%@ Page Language="c#" CodeBehind="MemberRankView.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master"
    AutoEventWireup="false" Inherits="CanhCam.Web.ProductUI.MemberRankViewPage" %>

<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:BreadcrumbAdmin ID="breadcrumb" runat="server"
        CurrentPageTitle="<%$Resources:ProductResources, MemberRankViewAdminTitle %>"
        CurrentPageUrl="~/Product/AdminCP/Member/MemberRankView.aspx" />
    <div class="admin-content">
        <portal:NotifyMessage ID="message" runat="server" />
        <portal:HeadingPanel ID="heading" runat="server">
        </portal:HeadingPanel>
        <div class="row">
            <div class="col-12">
                <asp:Panel ID="pnlSearch" CssClass="headInfo admin-content-bg-white" DefaultButton="btnSearch" runat="server">
                    <div class="settingrow form-group row align-items-center">
                        <gb:SiteLabel ID="SiteLabel1" runat="server" Text="Rank"
                            ForControl="ddlMemberRank" CssClass="control-label col-sm-3 mb-0" />
                        <div class="col-sm-9">
                            <asp:DropDownList ID="ddlMemberRank" AutoPostBack="false" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                    <div class="settingrow form-group row align-items-center">
                        <gb:SiteLabel ID="lblTitle" runat="server" Text="Keyword" ForControl="txtTitle" CssClass="control-label col-sm-3 mb-0" />
                        <div class="col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtTitle" runat="server" MaxLength="255" CssClass="form-control" />
                                <div class="input-group-btn">
                                    <asp:Button SkinID="DefaultButton" ID="btnSearch" Text="<%$Resources:ProductResources, ProductSearchButton %>" CssClass="btn btn-default"
                                        runat="server" CausesValidation="false" />
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>

        <div class="row">
            <div class="col-12">
                <div class="workplace admin-content-bg-white">
                    <telerik:RadGrid ID="grid" SkinID="radGridSkin" runat="server">
                        <MasterTableView AllowSorting="false" DataKeyNames="Id">
                            <Columns>
                                <telerik:GridTemplateColumn HeaderStyle-Width="35"
                                    HeaderText="<%$Resources:Resource, RowNumber %>">
                                    <ItemTemplate>
                                        <%# (grid.PageSize * grid.CurrentPageIndex) + Container.ItemIndex + 1%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <%--<telerik:GridTemplateColumn HeaderStyle-Width="35"
                                    HeaderText="Id">
                                    <ItemTemplate>
                                        <%#Eval("Id") %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>--%>

                                <telerik:GridTemplateColumn HeaderStyle-Width="35"
                                    HeaderText="<%$Resources:ProductResources, MemberRankViewNameLabel %>">
                                    <ItemTemplate>
                                        <%#Eval("Name") %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderStyle-Width="35"
                                    HeaderText="<%$Resources:ProductResources, MemberRankViewEmailLabel %>">
                                    <ItemTemplate>
                                        <%#Eval("Email") %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderStyle-Width="35"
                                    HeaderText="<%$Resources:ProductResources, MemberRankViewPointLabel %>">
                                    <ItemTemplate>
                                        <%#Eval("Point") %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderStyle-Width="35"
                                    HeaderText="<%$Resources:ProductResources, MemberRankViewRankLabel %>">
                                    <ItemTemplate>
                                        <%#Eval("Rank") %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
            </div>
        </div>
    </div>
</asp:Content>