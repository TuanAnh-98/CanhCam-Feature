<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="Subscribe.ascx.cs"
    Inherits="CanhCam.Web.ELetterUI.Subscribe" %>

<portal:BasePanel ID="pnlSubscribeBody" runat="server" CssClass="subscribe">
    <asp:Literal ID="litSubcribeHeader" runat="server" />
    <portal:BasePanel ID="pnlSubscribe" CssClass="subscribefrm" runat="server" DefaultButton="btnSubscribe">
        <asp:TextBox ID="txtEmail" runat="server" CssClass="subscribeemail" />
        <asp:Literal ID="litSeparator" runat="server" EnableViewState="false" />
        <button id="btnSubscribe" runat="server" class="subscribebutton" validationgroup="subscribe">
            <asp:Literal ID="litSubscribe" EnableViewState="false" runat="server" />
        </button>
        <asp:RequiredFieldValidator ID="reqEmail" runat="server" ControlToValidate="txtEmail"
            ValidationGroup="subscribe" Display="Dynamic" SetFocusOnError="true" />
        <asp:RegularExpressionValidator ID="regexEmail" runat="server" ControlToValidate="txtEmail"
            ValidationGroup="subscribe" Display="Dynamic" SetFocusOnError="true" />
    </portal:BasePanel>
    <portal:BasePanel ID="pnlThanks" CssClass="subscribethanks alert alert-success" runat="server" Visible="false">
        <asp:Literal ID="litThankYou" runat="server" />
    </portal:BasePanel>
    <portal:BasePanel ID="pnlNoNewsletters" CssClass="subscribeerror alert alert-danger" runat="server" Visible="false">
        <gb:SiteLabel ID="lblWarning" runat="server" ConfigKey="NewslettersNotAvailable" />
    </portal:BasePanel>
</portal:BasePanel>