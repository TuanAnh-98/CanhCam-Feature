<%@ Page Language="c#" CodeBehind="InventoryList.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="CanhCam.Web.StoreUI.InventoryListPage" %>

<%--<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />--%>
<%@ Import Namespace="CanhCam.Web.ProductUI" %>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:BreadcrumbAdmin ID="breadcrumb" runat="server"
        CurrentPageTitle="<%$Resources:StoreResources, InventoryListTitle %>" CurrentPageUrl="~/Store/AdminCP/InventoryList.aspx" />

    <div class="admin-content">
        <portal:NotifyMessage ID="message" runat="server" />
        <portal:HeadingPanel ID="heading" runat="server">
            <asp:Button ID="btnExport" SkinID="DefaultButton" Text="<%$Resources:ProductResources,OrderExportButton %>" runat="server" CausesValidation="false" />
            <asp:Button SkinID="UpdateButton" ID="btnUpdate" Text="<%$Resources:Resource, UpdateButton %>"
                runat="server" />
            <asp:Button SkinID="DeleteButton" ID="btnDelete" Text="<%$Resources:Resource, DeleteSelectedButton %>"
                runat="server" CausesValidation="false" Visible="false" />
        </portal:HeadingPanel>
        <asp:Panel ID="pnlSearch" CssClass="headInfo admin-content-bg-white" DefaultButton="btnSearch" runat="server">
            <div class="settingrow form-group row align-items-center">
                <gb:SiteLabel ID="lblKeyword" runat="server" ConfigKey="InventoryProductNameLabel"
                    ResourceFile="StoreResources" ForControl="txtKeyword"
                    CssClass="settinglabel control-label col-sm-3" />
                <div class="col-sm-9">
                    <asp:TextBox ID="txtKeyword" runat="server" MaxLength="255" CssClass="form-control" />
                </div>
            </div>
            <div class="settingrow form-group row align-items-center">
                <gb:SiteLabel ID="lblStoreFilter" runat="server" ConfigKey="InventoryStoreLabel"
                    ResourceFile="StoreResources" ForControl="ddStore"
                    CssClass="settinglabel control-label col-sm-3" />
                <div class="col-sm-9">
                    <asp:DropDownList ID="ddStore" runat="server" DataTextField="Name" DataValueField="StoreID" AppendDataBoundItems="true" CssClass="form-control" />
                </div>
            </div>
            <div class="settingrow form-group row align-items-center" id="divPriceFilter" runat="server">
                <gb:SiteLabel ID="lblFromPrice" runat="server" ConfigKey="InventoryFromPriceLabel"
                    ResourceFile="StoreResources" ForControl="txtFromPrice"
                    CssClass="settinglabel control-label col-sm-3" />
                <div class="col-sm-9">
                    <asp:TextBox ID="txtFromPrice" SkinID="PriceTextBox" MaxLength="50" runat="server" />
                    <asp:RegularExpressionValidator ID="rgxFromPrice"
                        ControlToValidate="txtFromPrice" runat="server"
                        ErrorMessage="<%$Resources:StoreResources, NumbersOnlyMessage %>"
                        ValidationExpression="\d+">
                    </asp:RegularExpressionValidator>
                    <gb:SiteLabel ID="lblToPrice" runat="server" ConfigKey="InventoryToLabel"
                        ResourceFile="StoreResources" ForControl="txtToPrice"
                        CssClass="settinglabel control-label mx-8" />
                    <asp:TextBox ID="txtToPrice" SkinID="PriceTextBox" MaxLength="50" runat="server" />
                    <asp:RegularExpressionValidator ID="rgxToPrice"
                        ControlToValidate="txtToPrice" runat="server"
                        ErrorMessage="<%$Resources:StoreResources, NumbersOnlyMessage %>"
                        ValidationExpression="\d+">
                    </asp:RegularExpressionValidator>
                </div>
            </div>
            <div class="settingrow form-group row align-items-center" id="divQuantityFilter" runat="server">
                <gb:SiteLabel ID="lblFromQuantity" runat="server" ConfigKey="InventoryFromQuantityLabel"
                    ResourceFile="StoreResources" ForControl="txtFromQuantity"
                    CssClass="settinglabel control-label col-sm-3" />
                <div class="col-sm-9">
                    <div class="form-inline justify-content-start align-items-center">
                        <asp:TextBox ID="txtFromQuantity" runat="server" />
                        <asp:RegularExpressionValidator ID="rgxFromQuantity"
                            ControlToValidate="txtFromQuantity" runat="server"
                            ErrorMessage="<%$Resources:StoreResources, NumbersOnlyMessage %>"
                            ValidationExpression="\d+">
                        </asp:RegularExpressionValidator>
                        <div style="margin: 0 8px;">
                            <gb:SiteLabel ID="lblToQuantity" runat="server" ConfigKey="InventoryToLabel"
                                ResourceFile="StoreResources" ForControl="txtToQuantity"
                                CssClass="settinglabel control-label mx-8" />
                        </div>
                        <asp:TextBox ID="txtToQuantity" runat="server" />
                        <asp:RegularExpressionValidator ID="rgxToQuantity"
                            ControlToValidate="txtToQuantity" runat="server"
                            ErrorMessage="<%$Resources:StoreResources, NumbersOnlyMessage %>"
                            ValidationExpression="\d+">
                        </asp:RegularExpressionValidator>
                    </div>
                </div>
            </div>
            <div class="settingrow form-group row align-items-center" id="divStatusFilter" runat="server">
                <gb:SiteLabel ID="lblStatusFilter" runat="server" ConfigKey="InventoryStatusLabel"
                    ResourceFile="StoreResources" ForControl="ddStore"
                    CssClass="settinglabel control-label col-sm-3" />
                <div class="col-sm-9">
                    <asp:DropDownList ID="ddStatus" runat="server" DataTextField="Name" DataValueField="StoreID" CssClass="form-control" />
                </div>
            </div>
            <div class="settingrow form-group row align-items-center">
                <gb:SiteLabel ID="lblDisplayFilter" runat="server" ConfigKey="InventoryDisplayLabel"
                    ResourceFile="StoreResources" ForControl="ddDisplay"
                    CssClass="settinglabel control-label col-sm-3" />
                <div class="col-sm-9">
                    <asp:DropDownList ID="ddDisplay" runat="server" DataTextField="Text" DataValueField="Value" CssClass="form-control" />
                </div>
            </div>
            <div class="settingrow form-group row align-items-center">
                <div class="col-lg-6"></div>
                <div class="col-lg-6">
                    <div class="form-inline">
                        <div class="form-group">
                            <asp:Button SkinID="DefaultButton" ID="btnSearch" Text="<%$Resources:ProductResources, OrderSearchButton %>" runat="server" CausesValidation="false" />
                        </div>
                        <div class="form-group">
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <div class="workplace admin-content-bg-white">
            <asp:UpdatePanel ID="upTreeList" runat="server">
                <ContentTemplate>
                    <telerik:RadTreeList RenderMode="Lightweight" Width="100%" ID="treeList" AllowLoadOnDemand="true" AllowSorting="true" AllowPaging="false" PageSize="15" AutoGenerateColumns="false"
                        OnChildItemsDataBind="treeList_ChildItemsDataBind" OnNeedDataSource="treeList_NeedDataSource" OnItemCreated="treeList_ItemCreated" OnItemDataBound="treeList_ItemDataBound"
                        DataKeyNames="InventoryID,ProductName,ProductID,ProductURL,ProductZoneID,StoreID,Price,ApiProductID,Quantity,IsPublished" ParentDataKeyNames="InventoryID,ProductName,ParentID,ProductURL,ProductZoneID,StoreID,Price,ApiProductID,Quantity,IsPublished"
                        AllowMultiItemSelection="true" runat="server" CssClass="table-responsive" Skin="Default">
                        <ClientSettings>
                            <Selecting AllowItemSelection="true" AllowToggleSelection="false" />
                        </ClientSettings>
                        <Columns>
                            <telerik:TreeListTemplateColumn HeaderStyle-Width="100" HeaderText="<%$Resources:ProductResources, ProductPictureHeading %>">
                                <ItemTemplate>
                                    <portal:MediaElement ID="me" runat="server" Width="90" Height="" FileUrl='<%# ProductHelper.GetImageFilePath(siteSettings.SiteId, Convert.ToInt32(Eval("ProductID")), Eval("ImageFile").ToString(), Eval("ThumbnailFile").ToString()) %>' />
                                </ItemTemplate>
                            </telerik:TreeListTemplateColumn>
                            <telerik:TreeListTemplateColumn HeaderText="<%$Resources:StoreResources, InventoryProductNameLabel %>" UniqueName="ProductName">
                                <ItemTemplate>
                                    <div>
                                        <asp:Literal ID="litProductName" runat="server" />
                                    </div>
                                </ItemTemplate>
                            </telerik:TreeListTemplateColumn>
                            <telerik:TreeListTemplateColumn HeaderText="<%$Resources:StoreResources, InventoryStoreNameLabel %>" UniqueName="StoreName">
                                <ItemTemplate>
                                    <div>
                                        <asp:Literal ID="litStoreName" runat="server" />
                                    </div>
                                </ItemTemplate>
                            </telerik:TreeListTemplateColumn>
                            <telerik:TreeListTemplateColumn HeaderText="<%$Resources:StoreResources, StorePublicLabel %>" UniqueName="IsPublished">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbPublish" runat="server" Checked='<%# Eval("IsPublished") %>' />
                                </ItemTemplate>
                            </telerik:TreeListTemplateColumn>
                            <telerik:TreeListTemplateColumn HeaderStyle-Width="115" HeaderStyle-Wrap="false" HeaderText="<%$Resources:StoreResources, InventoryAPIProductIDLabel %>" UniqueName="ApiProductID">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtApiProductID" Width="95"
                                        MaxLength="50" Text='<%# Eval("ApiProductID") %>' CssClass="form-control" runat="server" />
                                </ItemTemplate>
                            </telerik:TreeListTemplateColumn>
                            <telerik:TreeListTemplateColumn HeaderStyle-Width="115" HeaderStyle-Wrap="false" HeaderText="<%$Resources:StoreResources, InventoryQuantityLabel %>" UniqueName="Quantity">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtQuantity" Width="95"
                                        MaxLength="50" Text='<%# Eval("Quantity") %>' runat="server" />
                                </ItemTemplate>
                            </telerik:TreeListTemplateColumn>
                        </Columns>
                    </telerik:RadTreeList>
                    <telerik:RadDataPager RenderMode="Lightweight" ID="RadDataPager1" runat="server"
                        OnTotalRowCountRequest="RadDataPager1_TotalRowCountRequest" OnPageIndexChanged="RadDataPager1_PageIndexChanged" OnCommand="RadDataPager1_Command">
                        <Fields>
                            <telerik:RadDataPagerButtonField FieldType="FirstPrev" />
                            <telerik:RadDataPagerButtonField FieldType="Numeric" PageButtonCount="5" />
                            <telerik:RadDataPagerButtonField FieldType="NextLast" />
                            <telerik:RadDataPagerPageSizeField PageSizeComboWidth="60" PageSizeText="Page size: " />
                        </Fields>
                    </telerik:RadDataPager>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <style type="text/css">
        tr[data-class='stockout'], .stockout {
            background-color: lightblue !important;
        }

        tr[data-class='inputted'], .inputted {
            background-color: lightyellow !important;
        }
    </style>
</asp:Content>