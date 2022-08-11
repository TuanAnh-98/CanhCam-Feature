<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="ReviewList.aspx.cs" Inherits="CanhCam.Web.ProductUI.ReviewListPage" %>

<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:BreadcrumbAdmin ID="breadcrumb" runat="server" CurrentPageTitle="<%$Resources:ProductResources, ProductReviewTitle %>"
        CurrentPageUrl="~/Product/AdminCP/ReviewList.aspx" />
    <div class="admin-content col-md-12">
        <portal:HeadingPanel ID="heading" runat="server">
            <asp:Button SkinID="InsertButton" ID="btnApprove" Text="<%$Resources:ProductResources, ProductReviewApproveSelected %>"
                runat="server" />
            <asp:Button SkinID="InsertButton" ID="btnNotApproved" Text="<%$Resources:ProductResources, ProductReviewNotApprovedSelected %>"
                runat="server" />
            <asp:Button ID="btnDelete" SkinID="DeleteButton" Text="<%$Resources:Resource, DeleteSelectedButton %>"
                runat="server" CausesValidation="false" />
        </portal:HeadingPanel>
        <portal:NotifyMessage ID="message" runat="server" />
        <asp:UpdatePanel ID="up" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlSearch" DefaultButton="btnSearch" CssClass="headInfo form-horizontal"
                    runat="server">
                    <div class="settingrow form-group">
                        <gb:SiteLabel ID="lblFromDate" runat="server" ConfigKey="ProductReviewFromDate" ResourceFile="ProductResources"
                            ForControl="dpFromDate" CssClass="settinglabel control-label col-sm-3" />
                        <div class="col-sm-2">
                            <gb:DatePickerControl ID="dpFromDate" runat="server" />
                        </div>
                        <gb:SiteLabel ID="lblToDate" runat="server" ConfigKey="ProductReviewToDate" ResourceFile="ProductResources"
                            ForControl="dpToDate" CssClass="settinglabel control-label col-sm-1" />
                        <div class="col-sm-6">
                            <gb:DatePickerControl ID="dpToDate" runat="server" />
                        </div>
                    </div>
                    <div class="settingrow form-group">
                        <gb:SiteLabel ID="lblCommentType" runat="server" Text="Loại" ForControl="ddCommentType"
                            CssClass="settinglabel control-label col-sm-3" />
                        <div class="col-sm-9">
                            <asp:DropDownList ID="ddCommentType" runat="server">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="settingrow form-group">
                        <gb:SiteLabel ID="lblStatus" runat="server" ConfigKey="ProductReviewStatus" ResourceFile="ProductResources"
                            ForControl="ddStatus" CssClass="settinglabel control-label col-sm-3" />
                        <div class="col-sm-9">
                            <asp:DropDownList ID="ddStatus" runat="server">
                                <asp:ListItem Text="Tất cả" Value="-10"></asp:ListItem>
                                <asp:ListItem Text="Mới" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Không duyệt" Value="-1"></asp:ListItem>
                                <asp:ListItem Text="Đã duyệt" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Báo cáo vi phạm" Value="-2"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="settingrow form-group">
                        <gb:SiteLabel ID="lblKeyword" runat="server" ConfigKey="ProductReviewKeyword" ResourceFile="ProductResources"
                            ForControl="txtKeyword" CssClass="settinglabel control-label col-sm-3" />
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtKeyword" placeholder="<%$Resources:ProductResources, ProductReviewKeywordTip %>"
                                runat="server" MaxLength="255" />
                        </div>
                    </div>
                    <div class="settingrow form-group">
                        <gb:SiteLabel ID="lblOrderBy" runat="server" ConfigKey="ProductReviewOrderBy" ResourceFile="ProductResources"
                            ForControl="ddOrderBy" CssClass="settinglabel control-label col-sm-3" />
                        <div class="col-sm-7">
                            <asp:DropDownList ID="ddOrderBy" runat="server">
                                <asp:ListItem Text="Chọn" Value="-1"></asp:ListItem>
                                <asp:ListItem Text="Mới nhất" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Cũ nhất" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Thích nhiều nhất" Value="5"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:CheckBox ID="ckbPosition" Text="Vị trí yêu thích" runat="server" />
                        </div>
                        <div class="col-sm-2">
                            <asp:Button CssClass="btn btn-default" ID="btnSearch" Text="<%$Resources:ProductResources, OrderSearchButton %>"
                                runat="server" CausesValidation="false" />
                        </div>
                    </div>
                </asp:Panel>
                <div class="workplace">
                    <telerik:RadGrid ID="grid" SkinID="radGridSkin" runat="server">
                        <MasterTableView DataKeyNames="CommentId,ParentId,Status" AllowSorting="false" AllowPaging="false"
                            AllowFilteringByColumn="false">
                            <Columns>
                                <telerik:GridTemplateColumn HeaderStyle-Width="35" HeaderText="<%$Resources:Resource, RowNumber %>">
                                    <ItemTemplate>
                                        <%# Eval("RowNumber") %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="<%$Resources:ProductResources, ProductReviewProductTitle %>"
                                    UniqueName="ProductCode">
                                    <ItemTemplate>
                                        <div id="productInfo" runat="server" visible='<%# IsParent(Convert.ToInt32(Eval("ParentId"))) %>'>
                                            <%--<%# Eval("ProductCode") %>--%>
                                            <asp:HyperLink CssClass="cp-link" ID="hplProduct" Target="_blank" runat="server"
                                                Text='<%# Eval("ProductTitle").ToString() %>' NavigateUrl='<%# CanhCam.Web.ProductUI.ProductHelper.FormatProductUrl(Eval("ProductUrl").ToString(), Convert.ToInt32(Eval("ProductId")), Convert.ToInt32(Eval("ZoneId"))) %>'>
                                            </asp:HyperLink>
                                        </div>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn Visible="false" HeaderText="<%$Resources:ProductResources, ProductReviewTitleLabel %>"
                                    UniqueName="Title">
                                    <ItemTemplate>
                                        <%# Server.HtmlEncode(Eval("Title").ToString()) %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="<%$Resources:ProductResources, ProductReviewText %>"
                                    UniqueName="ContentText">
                                    <ItemTemplate>
                                        <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem,"ContentText").ToString()).Replace("\n", "<br />").Replace("\r\n", "<br />") %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="<%$Resources:ProductResources, ProductReviewFullName %>"
                                    UniqueName="FullName">
                                    <ItemTemplate>
                                        <%# Server.HtmlEncode(Eval("FullName").ToString()) %><br />
                                        <%# Server.HtmlEncode(Eval("Email").ToString()) %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="<%$Resources:ProductResources, ProductReviewCreatedOn %>">
                                    <ItemTemplate>
                                        <%# DateTimeHelper.Format(Convert.ToDateTime(Eval("CreatedUtc")), SiteUtils.GetUserTimeZone(), "dd/MM/yyyy HH:mm", SiteUtils.GetUserTimeOffset()) %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Thích">
                                    <ItemTemplate>
                                        <%# Eval("HelpfulYesTotal") %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Rating">
                                    <ItemTemplate>
                                        <%# Convert.ToInt32(Eval("CommentType")) == (int)ProductCommentType.Rating ? Eval("Rating").ToString() : "N/A" %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="<%$Resources:ProductResources, ProductReviewStatus %>">
                                    <ItemTemplate>
                                        <%# GetCommentStatus(Convert.ToInt32(Eval("Status"))) %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn AllowFiltering="false">
                                    <ItemTemplate>
                                        <asp:HyperLink CssClass="cp-link review-popup" ID="EditLink" Visible='<%# canUpdate %>'
                                            runat="server" Text="<%# Resources.ProductResources.ProductReviewEditLink %>"
                                            NavigateUrl='<%# this.SiteRoot + "/Product/AdminCP/ReviewEdit.aspx?CommentID=" + DataBinder.Eval(Container.DataItem,"CommentID") %>' /><br />
                                        <asp:HyperLink CssClass="cp-link review-popup" ID="ReplyLink" Visible='<%# canApprove && Eval("ParentId").ToString()=="-1" %>'
                                            runat="server" Text="<%# Resources.ProductResources.ProductReviewReplyLink %>"
                                            NavigateUrl='<%# this.SiteRoot + "/Product/AdminCP/ReviewEdit.aspx?CommentID=" + DataBinder.Eval(Container.DataItem,"CommentID") + "&reply=true" %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridClientSelectColumn HeaderStyle-Width="35" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                    <portal:gbCutePager ID="pgr" runat="server" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>