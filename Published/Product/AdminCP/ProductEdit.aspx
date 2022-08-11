<%@ Page ValidateRequest="false" Language="c#" CodeBehind="ProductEdit.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master"
    AutoEventWireup="false" Inherits="CanhCam.Web.ProductUI.ProductEditPage" %>

<%@ Register TagPrefix="Site" TagName="AdminProductEdit" Src="~/Product/Controls/AdminProductEditControl.ascx" %>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <Site:AdminProductEdit ID="adminProductEdit" runat="server"
        ProductType="0" EditPageUrl="/Product/AdminCP/ProductEdit.aspx" ListPageUrl="/Product/AdminCP/ProductList.aspx" />
</asp:Content>