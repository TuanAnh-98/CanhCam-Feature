<%@ Control Language="c#" AutoEventWireup="false" CodeBehind="PromotionModule.ascx.cs" Inherits="CanhCam.Web.ProductUI.PromotionModule" %>

<asp:Xml ID="xmlTransformer" runat="server"></asp:Xml>
<div id="divPager" visible="false" runat="server" class="pages discountpager">
    <portal:gbCutePager ID="pgr" runat="server" />
</div>