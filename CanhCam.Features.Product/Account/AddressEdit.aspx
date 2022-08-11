<%@ Page Language="c#" MaintainScrollPositionOnPostback="true" CodeBehind="AddressEdit.aspx.cs"
    MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="CanhCam.Web.AccountUI.AddressEditPage" %>

<%@ Register TagPrefix="Site" TagName="AccountMenu" Src="~/Account/AccountMenu.ascx" %>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <div class="container">
        <div class="row flex flex-wrap">
            <Site:AccountMenu ID="accountMenu" runat="server" />
            <div class="col-xs-12 col-lg-9">
                <div class="user-page clearfix">
                    <h1 class="postname">
                        <asp:Literal ID="litHeading" Text="Thêm địa chỉ mới" runat="server" />
                    </h1>
                    <asp:UpdatePanel ID="up" runat="server">
                        <ContentTemplate>
                            <div class="row flex flex-wrap">
                                <div class="col-xs-12">
                                    <portal:NotifyMessage ID="message" runat="server" />
                                    <div class="address-info clearfix">
                                        <div class="address-form clearfix">
                                            <div class="form-group clearfix">
                                                <gb:SiteLabel ID="lblFirstName" runat="server" ForControl="txtFirstName" Text="Họ tên" ResourceFile="AccountResources" />
                                                <asp:TextBox ID="txtFirstName" MaxLength="255" runat="server" />
                                                <asp:RequiredFieldValidator ID="reqFirstName" ErrorMessage="Vui lòng nhập Họ tên" Display="Dynamic"
                                                    ControlToValidate="txtFirstName" runat="server" ValidationGroup="AddressEdit" SkinID="UserAddress" />
                                            </div>
                                            <div class="form-group clearfix">
                                                <gb:SiteLabel ID="lblCompany" runat="server" ForControl="txtCompany" Text="Công ty" ResourceFile="AccountResources" />
                                                <asp:TextBox ID="txtCompany" MaxLength="255" runat="server" />
                                            </div>
                                            <div class="form-group clearfix">
                                                <gb:SiteLabel ID="lblPhone" runat="server" ForControl="txtPhone" Text="Số điện thoại:" ResourceFile="AccountResources" />
                                                <asp:TextBox ID="txtPhone" MaxLength="255" runat="server" />
                                                <asp:RequiredFieldValidator ID="reqPhone" ErrorMessage="Vui lòng nhập Số điện thoại" Display="Dynamic"
                                                    ControlToValidate="txtPhone" runat="server" ValidationGroup="AddressEdit" SkinID="UserAddress" />
                                            </div>
                                            <div class="form-group clearfix">
                                                <gb:SiteLabel ID="lblProvince" runat="server" ForControl="ddlProvince" Text="Tỉnh/Thành phố:"
                                                    ResourceFile="AccountResources" />
                                                <asp:DropDownList ID="ddlProvince" AppendDataBoundItems="true" AutoPostBack="true" DataTextField="Name" DataValueField="Guid" runat="server" />
                                                <asp:RequiredFieldValidator ID="reqProvince" ErrorMessage="Vui lòng chọn Tỉnh/Thành phố" Display="Dynamic"
                                                    ControlToValidate="ddlProvince" runat="server" ValidationGroup="AddressEdit" SkinID="UserAddress" />
                                            </div>
                                            <div class="form-group clearfix">
                                                <gb:SiteLabel ID="lblDistrict" runat="server" ForControl="ddlDistrict" Text="Quận/Huyện:" ResourceFile="AccountResources" />
                                                <asp:DropDownList ID="ddlDistrict" AppendDataBoundItems="true" AutoPostBack="true" DataTextField="Name" DataValueField="Guid" runat="server" />
                                                <asp:RequiredFieldValidator ID="reqDistrict" ErrorMessage="Vui lòng chọn Quận/Huyện" Display="Dynamic"
                                                    ControlToValidate="ddlDistrict" runat="server" ValidationGroup="AddressEdit" SkinID="UserAddress" />
                                            </div>
                                            <div class="form-group clearfix" runat="server" visible="false">
                                                <gb:SiteLabel ID="lblWard" runat="server" ForControl="ddlWard" Text="Phường xã:" ResourceFile="AccountResources" />
                                                <asp:DropDownList ID="ddlWard" AppendDataBoundItems="true" DataTextField="Name" DataValueField="Guid" runat="server" />
                                            </div> 
                                            <div class="form-group clearfix">
                                                <gb:SiteLabel ID="lblAddress" runat="server" ForControl="txtAddress" Text="Địa chỉ:" ResourceFile="AccountResources" />
                                                <asp:TextBox ID="txtAddress" MaxLength="255" runat="server" />
                                                <asp:RequiredFieldValidator ID="reqAddress" ErrorMessage="Vui lòng nhập Địa chỉ" Display="Dynamic"
                                                    ControlToValidate="txtAddress" runat="server" ValidationGroup="AddressEdit" SkinID="UserAddress" />
                                            </div>
                                            <div class="form-group check-group clearfix">
                                                <asp:CheckBox ID="chkIsDefault" Visible="false" Text="Sửa dụng địa chỉ này làm mặc định" runat="server" />
                                            </div>
                                            <asp:Button ID="btnUpdate" ValidationGroup="AddressEdit" Text="Cập nhật" runat="server" />
                                            <portal:gbLabel ID="lblError" CssClass="alert alert-warning" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>