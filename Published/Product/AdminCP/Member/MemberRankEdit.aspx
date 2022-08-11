<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="MemberRankEdit.aspx.cs" Inherits="CanhCam.Web.ProductUI.MemberRankEditPage" %>

<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:BreadcrumbAdmin ID="breadcrumb" runat="server"
        ParentTitle="<%$Resources:ProductResources, MemberRankAdminTitle %>" ParentUrl="~/Product/AdminCP/Member/MemberRank.aspx"
        CurrentPageTitle="<%$Resources:ProductResources, MemberRankEditAdminTitle %>" CurrentPageUrl="~/Product/AdminCP/Member/MemberRankEdit.aspx" />
    <div class="admin-content col-md-12">
        <asp:UpdatePanel ID="upButton" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <portal:HeadingPanel ID="heading" runat="server">
                    <asp:Button SkinID="UpdateButton" ID="btnUpdate" ValidationGroup="CustomFields" Text="<%$Resources:Resource, UpdateButton %>" runat="server" />
                    <asp:Button SkinID="InsertButton" ID="btnInsert" ValidationGroup="CustomFields" Text="<%$Resources:Resource, InsertButton %>" runat="server" />
                    <asp:Button SkinID="DeleteButton" ID="btnDelete" runat="server" Text="<%$Resources:Resource, DeleteButton %>" CausesValidation="false" />
                </portal:HeadingPanel>
                <portal:NotifyMessage ID="message" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="workplace">
            <div class="form-horizontal">

                <div class="settingrow form-group row align-items-center">
                    <gb:SiteLabel runat="server" Text="Thứ tự bậc"
                        CssClass="settinglabel control-label col-sm-3" />
                    <div class="col-sm-9">
                        <div class="input-group">
                            <asp:DropDownList ID="ddlRankOrder" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                </div>

                <div class="settingrow form-group row align-items-center">
                    <gb:SiteLabel runat="server" Text="Tên hiển thị(*)"
                        CssClass="settinglabel control-label col-sm-3" />
                    <div class="col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="settingrow form-group row align-items-center">
                    <gb:SiteLabel runat="server" Text="Mô tả"
                        CssClass="settinglabel control-label col-sm-3" />
                    <div class="col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="settingrow form-group row align-items-center">
                    <gb:SiteLabel runat="server" Text="Điểm thăng hạng(*)"
                        CssClass="settinglabel control-label col-sm-3" />
                    <div class="col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="txtPoint" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="settingrow form-group row align-items-center">
                    <gb:SiteLabel runat="server" Text="Giảm giá đơn hàng (phần trăm)(*)"
                        CssClass="settinglabel control-label col-sm-3" />
                    <div class="col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="txtDiscountPercent" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>