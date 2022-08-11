<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="Settings.aspx.cs"
    MasterPageFile="~/App_MasterPages/layout.Master" Inherits="CanhCam.Web.ProductUI.SettingsPage" %>

<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:BreadcrumbAdmin ID="breadcrumb" runat="server"
        CurrentPageTitle="<%$Resources:ProductResources, RewardPointSettingsTitle %>"
        CurrentPageUrl="~/Product/AdminCP/RewardPoint/Settings.aspx" />

    <div class="admin-content">
        <portal:NotifyMessage ID="message" runat="server" />
        <portal:HeadingPanel ID="heading" runat="server">
            <asp:Button ID="btnUpdate" SkinID="UpdateButton" Text="<%$Resources:ProductResources, ProductUpdateButton %>" runat="server" CausesValidation="true" />
        </portal:HeadingPanel>
        <div class="row">
            <div class="col-12">
                <div class="workplace admin-content-bg-white">

                    <div class="settingrow form-group row align-items-center">
                        <div class="col-sm-9 offset-sm-3">
                            <asp:CheckBox ID="chkRewardPoint" runat="server" />
                            <gb:SiteLabel ID="lblRewardPoint" runat="server" ForControl="chkRewardPoint" Text="Enable điểm thưởng"
                                CssClass="settinglabel control-label" />
                        </div>
                    </div>

                    <div class="settingrow form-group row align-items-center">
                        <gb:SiteLabel ID="lblAmountToPoint" runat="server" Text="Số tiền tương ứng với điểm"
                            CssClass="settinglabel control-label col-sm-3" />
                        <div class="col-sm-9">
                            <asp:Literal ID="litFromAmount" runat="server" Text="Số tiền "></asp:Literal>
                            <asp:TextBox ID="txtFromAmount" runat="server"></asp:TextBox>
                            <asp:Literal ID="litToPoint" runat="server" Text=" cho số điểm "></asp:Literal>
                            <asp:TextBox ID="txtToPoint" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <div class="settingrow form-group row align-items-center">
                        <gb:SiteLabel ID="lblPointForAmount" runat="server" Text="Số điểm quy đổi ra tiền"
                            CssClass="settinglabel control-label col-sm-3" />
                        <div class="col-sm-9">
                            <asp:Literal ID="litFromPoint" runat="server" Text="Số điểm "></asp:Literal>
                            <asp:TextBox ID="txtFromPoint" runat="server"></asp:TextBox>
                            <asp:Literal ID="litForAmount" runat="server" Text=" cho số tiền "></asp:Literal>
                            <asp:TextBox ID="txtForAmount" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>