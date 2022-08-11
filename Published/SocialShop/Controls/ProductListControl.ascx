<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ProductListControl.ascx.cs" Inherits="CanhCam.Web.SocialShopUI.ProductListControl" %>

<asp:Xml ID="xmlTransformer" runat="server"></asp:Xml>
<div id="divPager" runat="server" class="pages productpager">
    <portal:gbCutePager ID="pgr" runat="server" />
</div>