<%@ Page Language="c#" CodeBehind="ProductDetail.aspx.cs" MasterPageFile="~/SocialShop/layout.Master"
    AutoEventWireup="false" Inherits="CanhCam.Web.SocialShopUI.ProductDetailPage" MaintainScrollPositionOnPostback="true" %>

    <%@ Register TagPrefix="Site" Assembly="CanhCam.Features.Product" Namespace="CanhCam.Web.ProductUI" %>

<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:ContentExpiredLabel ID="expired" runat="server" EnableViewState="false" Visible="false" />
    <asp:Panel ID="pnlInnerWrap" runat="server">
        <Site:ProductDisplaySettings ID="displaySettings" runat="server" />
        <asp:Xml ID="xmlTransformer" runat="server"></asp:Xml>
        <div id="divPager" runat="server" class="pages productdetailpager">
            <portal:gbCutePager ID="pgr" runat="server" />
        </div>
    </asp:Panel>
</asp:Content>