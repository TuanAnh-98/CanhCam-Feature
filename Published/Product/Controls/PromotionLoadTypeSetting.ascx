<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PromotionLoadTypeSetting.ascx.cs" Inherits="CanhCam.Web.BannerUI.PromotionLoadTypeSetting" %>

<asp:DropDownList ID="ddlLoadType" runat="server">
    <asp:ListItem Text="<%$Resources:ProductResources, PromotionLoadProductSetting %>" Value="1" />
    <asp:ListItem Text="<%$Resources:ProductResources, PromotionLoadListSetting %>" Value="0" />
</asp:DropDownList>