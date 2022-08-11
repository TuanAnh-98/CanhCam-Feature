<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="SearchInputControl.ascx.cs" Inherits="CanhCam.Web.SocialShopUI.SearchInputControl" %>

<asp:Panel ID="pnlSearchInput" CssClass="nav navbar-nav navbar-right" runat="server" DefaultButton="btnSearch">
    <div class="navbar-form navbar-left" role="search">
        <div class="form-group">
            <asp:TextBox ID="txtKeyword" runat="server" CssClass="form-control" placeholder="Từ khóa" />
        </div>
        <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-default" Text="Tìm" />
    </div>
</asp:Panel>