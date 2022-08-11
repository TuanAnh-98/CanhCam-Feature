<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="AdminRecruitmentControl.ascx.cs" Inherits="CanhCam.Web.RecruitmentUI.AdminRecruitmentControl" %>

<%@ Register TagPrefix="Site" Assembly="CanhCam.Features.Recruitment" Namespace="CanhCam.Web.RecruitmentUI" %>
<%@ Import Namespace="CanhCam.Web.RecruitmentUI" %>

<Site:RecruitmentDisplaySettings ID="displaySettings" runat="server" />
<portal:BreadcrumbAdmin ID="breadcrumb" runat="server"
    CurrentPageTitle="<%$Resources:RecruitmentResources, RecruitmentListPage %>" CurrentPageUrl="~/Recruitment/AdminCP/RecruitmentList.aspx" />
<div class="admin-content col-md-12">
    <portal:HeadingPanel ID="heading" runat="server">
        <asp:Button SkinID="UpdateButton" ID="btnUpdate" Text="<%$Resources:RecruitmentResources, RecruitmentUpdateButton %>"
            runat="server" />
        <asp:HyperLink SkinID="InsertButton" ID="lnkInsert" Text="<%$Resources:Resource, InsertButton %>" runat="server" />
        <asp:Button SkinID="DeleteButton" ID="btnDelete" Text="<%$Resources:RecruitmentResources, RecruitmentDeleteSelectedButton %>"
            runat="server" CausesValidation="false" />
    </portal:HeadingPanel>
    <portal:NotifyMessage ID="message" runat="server" />
    <asp:Panel ID="pnlSearch" CssClass="headInfo form-horizontal" DefaultButton="btnSearch" runat="server">
        <div class="settingrow form-group">
            <gb:SiteLabel ID="lblZones" runat="server" ConfigKey="ZoneLabel"
                ForControl="ddZones" CssClass="settinglabel control-label col-sm-3" />
            <div class="col-sm-9">
                <asp:DropDownList ID="ddZones" AutoPostBack="false" runat="server" />
            </div>
        </div>
        <div class="settingrow form-group">
            <gb:SiteLabel ID="lblTitle" runat="server" ConfigKey="RecruitmentNameLabel"
                ResourceFile="RecruitmentResources" ForControl="txtTitle" CssClass="settinglabel control-label col-sm-3" />
            <div class="col-sm-9">
                <div class="input-group">
                    <asp:TextBox ID="txtTitle" runat="server" MaxLength="255" />
                    <div class="input-group-btn">
                        <asp:Button SkinID="DefaultButton" ID="btnSearch" Text="<%$Resources:RecruitmentResources, RecruitmentSearchButton %>"
                            runat="server" CausesValidation="false" />
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <div class="workplace">
        <telerik:RadGrid ID="grid" SkinID="radGridSkin" runat="server">
            <MasterTableView DataKeyNames="RecruitmentId,ZoneID,DisplayOrder,ViewCount,Code">
                <Columns>
                    <telerik:GridTemplateColumn HeaderStyle-Width="35" HeaderText="<%$Resources:Resource, RowNumber %>">
                        <ItemTemplate>
                            <%# (grid.PageSize * grid.CurrentPageIndex) + Container.ItemIndex + 1%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="<%$Resources:RecruitmentResources, RecruitmentNameLabel %>">
                        <ItemTemplate>
                            <asp:HyperLink CssClass="cp-link" ID="Title" runat="server" Text='<%# Eval("Title").ToString() %>'
                                NavigateUrl='<%# RecruitmentHelper.FormatRecruitmentUrl(Eval("Url").ToString(), Convert.ToInt32(Eval("RecruitmentId")), Convert.ToInt32(Eval("ZoneID")))  %>'>
                            </asp:HyperLink>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderStyle-Width="115" HeaderStyle-Wrap="false" HeaderText="<%$Resources:RecruitmentResources, RecruitmentCodeLabel %>" UniqueName="RecruitmentCode">
                        <ItemTemplate>
                            <asp:TextBox ID="txtRecruitmentCode" Width="95"
                                MaxLength="50" Text='<%# Eval("Code") %>' runat="server" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="<%$Resources:RecruitmentResources, RecruitmentViewedLabel %>"
                            AllowFiltering="false">
                            <ItemTemplate>
                                <asp:TextBox ID="txtViewCount" SkinID="NumericTextBox"
                                    MaxLength="9" Text='<%# Eval("ViewCount") %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderStyle-Width="100" HeaderText="<%$Resources:RecruitmentResources, RecruitmentDisplayOrderLabel %>">
                        <ItemTemplate>
                            <asp:TextBox ID="txtDisplayOrder" SkinID="NumericTextBox"
                                MaxLength="4" Text='<%# Eval("DisplayOrder") %>' runat="server" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderStyle-Width="100">
                        <ItemTemplate>
                            <asp:HyperLink CssClass="cp-link" ID="EditLink" runat="server"
                                Text="<%$Resources:RecruitmentResources, RecruitmentEditLink %>" NavigateUrl='<%# siteRoot + EditPageUrl + "?RecruitmentID=" + Eval("RecruitmentId") %>'>
                            </asp:HyperLink>
                            <asp:HyperLink CssClass="cp-link" ID="ViewRecruitment" runat="server" Text="<%$Resources:RecruitmentResources, PreviewLabel %>"
                                NavigateUrl='<%# RecruitmentHelper.FormatRecruitmentUrl(Eval("Url").ToString(), Convert.ToInt32(Eval("RecruitmentId")), Convert.ToInt32(Eval("ZoneID"))) %>'>
                            </asp:HyperLink>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridClientSelectColumn HeaderStyle-Width="35" />
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
    </div>
</div>