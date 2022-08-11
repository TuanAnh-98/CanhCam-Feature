<%@ Page Language="c#" CodeBehind="Login.aspx.cs" MasterPageFile="~/App_MasterPages/layoutSecure.Master"
    AutoEventWireup="false" Inherits="CanhCam.Web.UI.Pages.LoginPage" %>

<%@ Register TagPrefix="gb" TagName="Login" Src="~/Secure/Controls/LoginControl.ascx" %>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <asp:Panel ID="pnlLogin" CssClass="login-box" runat="server">
        <div class="login-logo">
            <gb:SiteLabel ID="lblSignIn" runat="server" ConfigKey="SignInLabel" UseLabelTag="false" />
        </div>
        <asp:Panel ID="pnlTopContent" runat="server" Visible="false" CssClass="logincontent logincontenttop"
            EnableViewState="false">
            <asp:Literal ID="litTopContent" runat="server" EnableViewState="false" />
        </asp:Panel>
        <asp:Panel ID="pnlStandardLogin" runat="server" CssClass="loginstandard">
            <gb:Login ID="login1" runat="server" />
        </asp:Panel>
        <div class="loginopenid">
            <asp:Panel ID="pnlWindowsLive" runat="server" Visible="false">
                <portal:WindowsLiveLoginControl ID="livelogin" runat="server" />
            </asp:Panel>
            <asp:Panel ID="divLiteralOr" runat="server" Visible="false">
                <asp:Literal ID="litOr" runat="server" />
            </asp:Panel>
            <asp:Panel ID="pnlOpenID" runat="server" Visible="false">
            </asp:Panel>
        </div>
        <asp:Panel ID="pnlBottomContent" runat="server" Visible="false" CssClass="logincontent logincontentbottom" EnableViewState="false">
            <asp:Literal ID="litBottomContent" runat="server" EnableViewState="false" />
        </asp:Panel>
    </asp:Panel>
</asp:Content>