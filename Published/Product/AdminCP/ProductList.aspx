<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="ProductList.aspx.cs" Inherits="CanhCam.Web.ProductUI.ProductListPage" %>

<%@ Register TagPrefix="Site" TagName="AdminProduct" Src="~/Product/Controls/AdminProductControl.ascx" %>
<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <Site:AdminProduct ID="adminProduct" runat="server"
        ProductType="-1" EditPageUrl="/Product/AdminCP/ProductEdit.aspx"
        PageTitle="<%$Resources:ProductResources, ProductListTitle %>" PageUrl="/Product/AdminCP/ProductList.aspx" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />