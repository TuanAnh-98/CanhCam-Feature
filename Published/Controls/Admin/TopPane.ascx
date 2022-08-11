<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="TopPane.ascx.cs" Inherits="CanhCam.Web.AdminUI.TopPane" %>

<asp:Repeater ID="rptQuickLink" Visible="false" runat="server">
    <ItemTemplate>
        <li>
            <a target='<%# Convert.ToBoolean(Eval("OpenInNewWindow")) ? "_blank" : "_self" %>'
                href='<%# BuildMenuLink(Eval("Url").ToString()) %>'>
                <%# ResourceHelper.GetResourceString(Eval("ResourceFile").ToString(), Eval("KeyName").ToString()) %>
            </a>
        </li>
    </ItemTemplate>
</asp:Repeater>
<nav class="main-header navbar navbar-expand navbar-white navbar-light">
    <ul class="navbar-nav">
        <li class="nav-item">
            <a class="nav-link" data-widget="pushmenu" href="#" role="button">
                <i class="fas fa-bars"></i>
            </a>
        </li>
        <li class="nav-item d-none d-sm-inline-block">
            <portal:HomeLink ID="HomeLink1" IconCssClass="fa fa-home" OverrideText=" " CssClass="nav-link" runat="server" />
        </li>
    </ul>
    <ul class="navbar-nav ml-auto">
        <asp:Xml ID="xmlTopMenu" runat="server"></asp:Xml>
        <asp:Repeater ID="rptLanguage" runat="server">
            <ItemTemplate>
                <li class="nav-item">
                    <asp:LinkButton ID="lkbLanguage" CssClass="language nav-link" style="text-transform:uppercase" runat="server" Text='<%#Eval("TwoLetterCode")%>'
                        CommandArgument='<%#Eval("LanguageCode")%>' CommandName="ChangeLanguage" />
                </li>
            </ItemTemplate>
        </asp:Repeater>
        <li class="nav-item dropdown author">
            <a href="#" class="nav-link" data-toggle="dropdown">
                <portal:Avatar ID="userAvatar" runat="server" />
            </a>
            <div class="dropdown-menu dropdown-menu-lg dropdown-menu-right">
                <span class="dropdown-item dropdown-header"><portal:WelcomeMessage ID="WelcomeMessage1" WrapInProfileLink="false" runat="server" /></span>
                <div class="dropdown-divider"></div>
                <portal:UserProfileLink ID="UserProfileLink" IconCssClass="fa fa-user" CssClass="dropdown-item" RenderAsListItem="false" runat="server" />
                <div class="dropdown-divider"></div>
                <asp:LinkButton ID="lnkClearCache" CssClass="dropdown-item" OnClick="lnkClearCache_Click" runat="server"><i class="fas fa-sync-alt" aria-hidden="true"></i> <asp:Literal ID="litClearCache" Text="Clear cache" runat="server"></asp:Literal></asp:LinkButton>
                <portal:LogoutLink ID="LogoutLink" IconCssClass="fa fa-sign-out" CssClass="dropdown-item dropdown-footer" RenderAsListItem="false" runat="server" />
            </div>
        </li>
    </ul>
</nav>