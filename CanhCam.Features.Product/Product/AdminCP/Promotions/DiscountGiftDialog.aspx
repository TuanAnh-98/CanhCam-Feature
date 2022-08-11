<%@ Page Language="c#" CodeBehind="DiscountGiftDialog.aspx.cs" MasterPageFile="~/App_MasterPages/DialogMaster.Master"
    AutoEventWireup="false" Inherits="CanhCam.Web.ProductUI.DiscountGiftDialog" %>

<%@ Import Namespace="CanhCam.Web.ProductUI" %>
<asp:Content ContentPlaceHolderID="phMain" ID="MPContent" runat="server">
    <div class="container">
        <portal:HeadingControl ID="heading" Text="Chọn sản phẩm tặng kèm" Visible="false" runat="server" />
        <h3>Cập nhật Quà tặng</h3>
        <asp:UpdatePanel ID="up" runat="server">
            <ContentTemplate>
                <div class="settingrow form-group row align-items-center">
                    <div class="col-sm-3"></div>
                    <div class="col-sm-9">
                        <asp:Button SkinID="DeleteButton" ID="btnDeleteGift" Visible="false" runat="server" Text="<%$Resources:Resource, DeleteSelectedButton %>"
                            CausesValidation="false" />
                    </div>
                </div>
                <telerik:RadGrid ID="gridGifts" SkinID="radGridSkin" runat="server">
                    <MasterTableView DataKeyNames="GiftID,ProductID,DiscountID" AllowSorting="false" AllowPaging="false">
                        <Columns>
                            <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="50" HeaderText="<%$Resources:ProductResources, ProductPictureHeading %>">
                                <ItemTemplate>
                                    <portal:MediaElement ID="ml" runat="server" Width="40" Title=" " FileUrl='<%# GetImageFilePath(Eval("ImageFile").ToString()) %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Tên" AllowFiltering="false">
                                <ItemTemplate>
                                    <%# Eval("Name") %>
                                    <asp:Literal runat="server" ID="litTest" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Số lượng quà" AllowFiltering="false">
                                <ItemTemplate>
                                    <%# Eval("Quantity") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Loại" AllowFiltering="false">
                                <ItemTemplate>
                                    <%# Convert.ToInt32(Eval("ProductID")) > 0 ? "Hệ thống" : "Ngoài hệ thống" %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridClientSelectColumn HeaderStyle-Width="35" />
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
                <hr />
                <div class="form-horizontal">
                    <div class="settingrow form-group row align-items-center">
                        <gb:SiteLabel ID="lblGiftQuantity" runat="server"
                            CssClass="settinglabel control-label col-sm-3 mb-0" Text="Số lượng Quà" />
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtGiftQuantity" runat="server" Text="1" SkinID="NumericTextBox" CssClass="form-control" Style="width: 120px" />
                            <asp:RequiredFieldValidator ID="reqGiftQuantity" runat="server"
                                ControlToValidate="txtGiftQuantity" Display="Dynamic" SetFocusOnError="true"
                                CssClass="txterror" ValidationGroup="GiftEdit" />
                        </div>
                    </div>
                    <div class="settingrow form-group row align-items-center">
                        <gb:SiteLabel ID="lblGift" runat="server"
                            CssClass="settinglabel control-label col-sm-3 mb-0" Text="Thêm Quà tặng" />
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtGiftName" runat="server" CssClass="form-control" placeholder="Tên quà tặng" />
                            <asp:RequiredFieldValidator ID="reqGiftName" runat="server"
                                ControlToValidate="txtGiftName" Display="Dynamic" SetFocusOnError="true"
                                CssClass="txterror" ValidationGroup="GiftEdit" />
                            <%--<asp:TextBox ID="txtGiftMax" runat="server" SkinID="NumericTextBox" CssClass="form-control" Style="display: inline-block; width: 120px" placeholder="Số lượng tối đa" />--%>
                        </div>
                    </div>
                    <div class="settingrow form-group row align-items-center">
                        <gb:SiteLabel ID="lblGiftImage" runat="server"
                            CssClass="settinglabel control-label col-sm-3 mb-0" Text="Hình ảnh" />
                        <div class="col-sm-9">
                            <telerik:RadAsyncUpload ID="fupImageFile" SkinID="radAsyncUploadSkin"
                                MultipleFileSelection="Disabled" HideFileInput="true"
                                AllowedFileExtensions="jpg,jpeg,gif,png"
                                Localization-Select="<%$Resources:ProductResources, SelectFromComputerLabel %>"
                                MaxFileInputsCount="1" runat="server" />
                            <asp:Button ID="btnInsert" SkinID="InsertButton" Text="<%$Resources:Resource, InsertButton %>" runat="server" ValidationGroup="GiftEdit" />
                        </div>
                    </div>
                    <hr />
                    <h5>Thêm sản phẩm từ hệ thống</h5>
                    <asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server">
                        <div class="settingrow form-group">
                            <gb:SiteLabel ID="lblZones" runat="server" ConfigKey="ZoneLabel"
                                ForControl="ddZones" CssClass="settinglabel control-label col-sm-3" />
                            <div class="col-sm-9">
                                <asp:DropDownList ID="ddlZones" runat="server" CssClass="form-control" />
                            </div>
                        </div>
                        <div class="settingrow form-group">
                            <gb:SiteLabel ID="lblTitle" runat="server" ConfigKey="ProductNameLabel"
                                ResourceFile="ProductResources" ForControl="txtTitle" CssClass="settinglabel control-label col-sm-3" />
                            <div class="col-sm-9">
                                <div class="input-group">
                                    <asp:TextBox ID="txtTitle" runat="server" MaxLength="255" CssClass="form-control" />
                                    <div class="input-group-btn">
                                        <asp:Button SkinID="DefaultButton" ID="btnSearch" Text="<%$Resources:ProductResources, ProductSearchButton %>"
                                            runat="server" CausesValidation="false" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="settingrow form-group">
                            <div class="col-sm-3"></div>
                            <div class="col-sm-9">
                                <asp:Button ID="btnInsertFromSystem" SkinID="InsertButton" Text="Thêm sản phẩm được chọn" runat="server" />
                            </div>
                        </div>
                    </asp:Panel>
                    <telerik:RadGrid ID="grid" SkinID="radGridSkin" runat="server">
                        <MasterTableView DataKeyNames="ProductId" AllowPaging="true" PageSize="10">
                            <Columns>
                                <telerik:GridTemplateColumn HeaderStyle-Width="50" HeaderText="<%$Resources:ProductResources, ProductPictureHeading %>">
                                    <ItemTemplate>
                                        <portal:MediaElement ID="ml" runat="server" Width="40" Title=" " FileUrl='<%# ProductHelper.GetImageFilePath(siteSettings.SiteId, Convert.ToInt32(Eval("ProductId")), Eval("ImageFile").ToString(), Eval("ThumbnailFile").ToString()) %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Mã">
                                    <ItemTemplate>
                                        <%# Eval("Code") %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Sản phẩm">
                                    <ItemTemplate>
                                        <asp:HyperLink CssClass="cp-link" ID="Title" runat="server" Text='<%# Eval("Title").ToString() %>'
                                            NavigateUrl='<%# ProductHelper.FormatProductUrl(Eval("Url").ToString(), Convert.ToInt32(Eval("ProductId")), Convert.ToInt32(Eval("ZoneID")))  %>'>
                                        </asp:HyperLink>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderStyle-Width="75" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" HeaderText="<%$Resources:ProductResources, ProductPriceLabel %>" UniqueName="Price">
                                    <ItemTemplate>
                                        <%# ProductHelper.FormatPrice(Convert.ToDecimal(Eval("Price")), true) %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <%--<telerik:GridTemplateColumn Visible="false" HeaderStyle-Width="50" HeaderText="Hot?" UniqueName="SpecialProduct">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnSpecialProduct" runat="server"
                                            CommandName="SpecialProduct" CommandArgument='<%#Eval("ProductId")%>' ImageUrl='<%# GetSpecialProductImageUrl(Convert.ToInt32(Eval("ProductId")))%>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>--%>
                                <telerik:GridClientSelectColumn HeaderStyle-Width="35" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <style type="text/css">
        h1 {
            font-size: 20px
        }

        .RadGrid {
            width: 100% !important;
        }

        .RadEditor .reContentCell {
            height: 100% !important;
        }

        textarea {
            height: auto;
        }
    </style>
    <portal:SessionKeepAliveControl ID="ka1" runat="server" />
</asp:Content>