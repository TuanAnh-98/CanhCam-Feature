<%@ Control Language="c#" AutoEventWireup="false" CodeBehind="ManufacturerDetailModule.ascx.cs" Inherits="CanhCam.Web.ProductUI.ManufacturerDetailModule" %>

<asp:Xml ID="xmlTransformer" runat="server"></asp:Xml>
<div id="divPager" visible="false" runat="server" class="pages">
    <portal:gbCutePager ID="pgr" runat="server" />
</div>