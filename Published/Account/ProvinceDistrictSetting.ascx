<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ProvinceDistrictSetting.ascx.cs" Inherits="CanhCam.Web.AccountUI.ProvinceDistrictSetting" %>

<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <div class="row">
            <div class="col-xs-12">
                <div class="province-wrap">
                    <div class="row">
                        <div class="province col-xs-12 col-md-4">
                            <asp:DropDownList ID="ddProvince" runat="server" AppendDataBoundItems="true" AutoPostBack="true" DataValueField="Guid" DataTextField="Name" EnableTheming="false" />
                            <%--<asp:RequiredFieldValidator ControlToValidate="ddProvince"
									ValidationGroup="profile" SkinID="Registration" ErrorMessage="Vui lòng nhập Tỉnh/Thành"
									ID="reqProvince" Display="Dynamic" SetFocusOnError="true" runat="server" />--%>
                        </div>
                        <div class="district col-xs-12 col-md-4">
                            <asp:DropDownList ID="ddDistrict" runat="server" AppendDataBoundItems="true" AutoPostBack="true" DataValueField="Guid" DataTextField="Name" EnableTheming="false" />
                            <%--<asp:RequiredFieldValidator ControlToValidate="ddDistrict"
									ValidationGroup="profile" SkinID="Registration" ErrorMessage="Vui lòng nhập Quận/Huyện"
									ID="reqDistrict" Display="Dynamic" SetFocusOnError="true" runat="server" />--%>
                        </div>
                        <div class="ward col-xs-12 col-md-4">
                            <asp:DropDownList ID="ddWard" runat="server" AppendDataBoundItems="true" DataValueField="Guid" DataTextField="Name" EnableTheming="false" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <asp:RequiredFieldValidator ControlToValidate="ddWard"
            ValidationGroup="profile" SkinID="Registration" ErrorMessage="Vui lòng nhập Phường/Xã"
            ID="reqWard" Display="Dynamic" SetFocusOnError="true" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>