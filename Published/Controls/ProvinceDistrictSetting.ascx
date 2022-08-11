<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ProvinceDistrictSetting.ascx.cs" Inherits="CanhCam.Web.UI.ProvinceDistrictSetting" %>

<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <asp:DropDownList ID="ddProvince" runat="server" AutoPostBack="true" DataValueField="Guid" DataTextField="Name" EnableTheming="false" />
        <asp:DropDownList ID="ddDistrict" runat="server" DataValueField="Guid" DataTextField="Name"
            EnableTheming="false">
        </asp:DropDownList>
    </ContentTemplate>
</asp:UpdatePanel>