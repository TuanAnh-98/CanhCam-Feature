<%@ Page Language="c#" CodeBehind="StoreList.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="CanhCam.Web.StoreUI.StoreListPage" %>

<%@ Import Namespace="CanhCam.Web.StoreUI" %>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:BreadcrumbAdmin ID="breadcrumb" runat="server"
        CurrentPageTitle="<%$Resources:StoreResources, StoreListTitle %>" CurrentPageUrl="~/Store/AdminCP/StoreList.aspx" />
    <div class="admin-content">
        <portal:NotifyMessage ID="message" runat="server" />
        <portal:HeadingPanel ID="heading" runat="server">
            <asp:HyperLink SkinID="InsertButton" ID="lnkInsert" Text="<%$Resources:Resource, InsertButton %>" runat="server" />
            <asp:Button SkinID="DeleteButton" ID="btnDelete" Text="<%$Resources:Resource, DeleteSelectedButton %>"
                runat="server" CausesValidation="false" />
        </portal:HeadingPanel>
        <asp:UpdatePanel ID="up" runat="server">
            <ContentTemplate>
                <div class="form-horizontal">
                    <div class="settingrow form-group row align-items-center">
                        <gb:SiteLabel ID="lblNameKeyword" runat="server" ConfigKey="StoreNameLabel"
                            ResourceFile="StoreResources" ForControl="txtNameKeyword"
                            CssClass="settinglabel control-label col-sm-3" />
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtNameKeyword" runat="server" MaxLength="100" CssClass="form-control" />
                        </div>
                    </div>
                    <div class="settingrow form-group row align-items-center">
                        <div class="col-sm-3 offset-sm-3">
                            <asp:DropDownList ID="ddProvince" runat="server" DataTextField="Name" DataValueField="Guid" AutoPostBack="true" AppendDataBoundItems="true" CssClass="form-control" />
                        </div>
                        <div class="col-sm-3">
                            <asp:DropDownList ID="ddDistrict" runat="server" DataTextField="Name" DataValueField="Guid" AutoPostBack="true" AppendDataBoundItems="true" CssClass="form-control" />
                        </div>
                    </div>
                    <div class="settingrow form-group row align-items-center">
                        <div class="col-sm-3"></div>
                        <div class="col-sm-9">
                            <asp:Button SkinID="DefaultButton" ID="btnSearch" Text="<%$Resources:ProductResources, OrderSearchButton %>" runat="server" CausesValidation="false" />
                        </div>
                    </div>
                </div>
                <div class="workplace admin-content-bg-white">
                    <telerik:RadGrid ID="grid" SkinID="radGridSkin" runat="server">
                        <MasterTableView DataKeyNames="StoreID,SiteID,Name,Options,IsPublished" AllowSorting="false">
                            <Columns>
                                <%--<telerik:GridTemplateColumn HeaderStyle-Width="35"
						            HeaderText="<%$Resources:Resource, RowNumber %>" AllowFiltering="false">
						            <ItemTemplate>
							            <%# (grid.PageSize * grid.CurrentPageIndex) + Container.ItemIndex + 1%>
						            </ItemTemplate>
					            </telerik:GridTemplateColumn>--%>
                                <telerik:GridTemplateColumn HeaderText="<%$Resources:StoreResources, StoreNameLabel %>" UniqueName="StoreName">
                                    <ItemTemplate>
                                        <div>
                                            <asp:Literal ID="litStoreName" runat="server" Text='<%# Eval("Name") %>' />
                                            <br />
                                            <%--<asp:Literal ID="litStoreDefault" runat="server" Text='<%# IsStoreDefault(Convert.ToInt32(Eval("StoreID"))) ? ResourceHelper.GetResourceString("StoreResources","StoreDefaultLabel") : "" %>' />--%>
                                        </div>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn HeaderText="<%$Resources:StoreResources, StoreAddressLabel %>" DataField="Address" />
                                <telerik:GridTemplateColumn HeaderText="<%$Resources:StoreResources, StoreTakeOnlineOrderLabel %>" AllowFiltering="false">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbOption" runat="server" Checked='<%# Convert.ToBoolean(Eval("Options")) %>' onclick="return false;" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="<%$Resources:StoreResources, StorePublicLabel %>" AllowFiltering="false">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbIsPublished" runat="server" Checked='<%# Eval("IsPublished") %>' onclick="return false;" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="<%$Resources:StoreResources, StoreDefaultLabel %>" AllowFiltering="false">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbDefault" Checked='<%# IsStoreDefault(Convert.ToInt32(Eval("StoreID"))) %>' runat="server" onclick="return false;" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemTemplate>
                                        <asp:HyperLink CssClass="cp-link" ID="EditLink"
                                            runat="server" Text="<%$Resources:StoreResources, StoreEditLink %>"
                                            NavigateUrl='<%# SiteRoot + "/Store/AdminCP/StoreEdit.aspx?id=" + Eval("StoreID") %>'>
                                        </asp:HyperLink>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridClientSelectColumn HeaderStyle-Width="35" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>