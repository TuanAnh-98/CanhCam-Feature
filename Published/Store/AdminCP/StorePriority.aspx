<%@ Page Language="c#" CodeBehind="StorePriority.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="CanhCam.Web.StoreUI.StorePriorityPage" %>

<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:BreadcrumbAdmin ID="breadcrumb" runat="server"
        ParentTitle="<%$Resources:StoreResources, StoreListTitle %>" ParentUrl="~/Store/AdminCP/StoreList.aspx"
        CurrentPageTitle="<%$Resources:StoreResources, UpdateStorePriorityTitle %>" CurrentPageUrl="~/Store/AdminCP/StorePriority.aspx" />

    <div class="admin-content">
        <%--Message Box--%>
        <portal:NotifyMessage ID="message" runat="server" />
        <%--Buttons panel--%>
        <asp:UpdatePanel ID="upButton" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <portal:NotifyMessage ID="NotifyMessage1" runat="server" />
                <portal:HeadingPanel ID="heading" runat="server">
                    <asp:Button SkinID="UpdateButton" ID="btnUpdate" Text="<%$Resources:Resource, UpdateButton %>"
                        ValidationGroup="Store" runat="server" />
                </portal:HeadingPanel>
            </ContentTemplate>
        </asp:UpdatePanel>

        <%--Filter Options--%>
        <div class="workplace admin-content-bg-white">
            <div class="form-horizontal">
                <div class="settingrow form-group row align-items-center">
                    <gb:SiteLabel ID="lblNameKeyword" runat="server" ConfigKey="StoreManagingAreaLabel"
                        ResourceFile="StoreResources" ForControl="txtNameKeyword"
                        CssClass="settinglabel control-label col-sm-3" />
                    <div class="col-sm-9">
                        <asp:DropDownList ID="ddlArea" runat="server" DataTextField="Name" DataValueField="StoreID" AutoPostBack="true" AppendDataBoundItems="true" CssClass="form-control" />
                    </div>
                </div>
            </div>
        </div>
        <div class="workplace admin-content-bg-white">
            <asp:UpdatePanel ID="up" runat="server">
                <ContentTemplate>
                    <telerik:RadGrid ID="grid" SkinID="radGridSkin" runat="server">
                        <MasterTableView DataKeyNames="StoreID,Name,Priority,ManagingArea" AllowSorting="false">
                            <Columns>
                                <telerik:GridTemplateColumn HeaderStyle-Width="35"
                                    HeaderText="<%$Resources:Resource, RowNumber %>" AllowFiltering="false">
                                    <ItemTemplate>
                                        <%# (grid.PageSize * grid.CurrentPageIndex) + Container.ItemIndex + 1%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="<%$Resources:StoreResources, StoreManagingAreaLabel %>" UniqueName="ManagingArea">
                                    <ItemTemplate>
                                        <div>
                                            <asp:Literal ID="litManagingArea" runat="server" />
                                        </div>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="<%$Resources:StoreResources, StoreNameLabel %>" UniqueName="StoreName">
                                    <ItemTemplate>
                                        <div>
                                            <%# Eval("Name") %>
                                        </div>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <%--<telerik:GridBoundColumn HeaderText="<%$Resources:StoreResources, StoreAddressLabel %>" DataField="Address" />--%>
                                <telerik:GridTemplateColumn HeaderText="<%$Resources:StoreResources, StorePriorityLabel %>" UniqueName="StorePriority">
                                    <ItemTemplate>
                                        <div>
                                            <asp:TextBox ID="txtPriority" SkinID="NumericTextBox" Style="text-align: right"
                                                MaxLength="50" Text='<%# Eval("Priority") %>' CssClass="form-control" runat="server" />
                                            <asp:RegularExpressionValidator ID="rgxPriority"
                                                ControlToValidate="txtPriority" runat="server"
                                                ErrorMessage="<%$Resources:StoreResources, NumbersOnlyMessage %>"
                                                ValidationExpression="\d+" />
                                        </div>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <%--<telerik:GridClientSelectColumn HeaderStyle-Width="35" />--%>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>