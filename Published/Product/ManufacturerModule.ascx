<%@ Control Language="c#" AutoEventWireup="false" CodeBehind="ManufacturerModule.ascx.cs" Inherits="CanhCam.Web.ProductUI.ManufacturerModule" %>

<asp:Xml ID="xmlTransformer" runat="server"></asp:Xml>
<div id="divPager" visible="false" runat="server" class="pages">
    <portal:gbCutePager ID="pgr" runat="server" />
</div>