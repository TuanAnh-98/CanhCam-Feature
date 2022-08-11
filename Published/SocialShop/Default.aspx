<%@ Page Language="c#" CodeBehind="Default.aspx.cs" MasterPageFile="~/SocialShop/layout.Master"
    AutoEventWireup="false" Inherits="CanhCam.Web.SocialShopUI.DefaulShopPage" MaintainScrollPositionOnPostback="true" %>

<%@ Register TagPrefix="Site" TagName="ProductList" Src="~/SocialShop/Controls/ProductListControl.ascx" %>

<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <Site:ProductList ID="ProductList1" runat="server"></Site:ProductList>
</asp:Content>