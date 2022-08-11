<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="ChangePassword.aspx.cs" Inherits="CanhCam.Web.AccountUI.ChangePasswordPage" %>

<%@ Register TagPrefix="Site" TagName="AccountMenu" Src="~/Account/AccountMenu.ascx" %>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <div class="container">
        <div class="row flex flex-wrap">
            <Site:AccountMenu ID="accountMenu" runat="server" />
            <div class="col-xs-12 col-lg-9">
                <asp:Panel ID="pnlPassword" CssClass="user-page clearfix" runat="server">
                    <h1 class="postname">
                        <asp:Literal ID="litHeading" Text="<%$Resources:Resource, ChangePasswordLabel %>" runat="server" />
                    </h1>
                    <div class="row flex flex-wrap">
                        <div class="col-xs-12">
                            <div class="account-info clearfix">
                                <asp:ChangePassword ID="ChangePassword1" Width="100%" runat="server">
                                    <ChangePasswordTemplate>
                                        <div class="account-form clearfix">
                                            <asp:Panel ID="divCurrentPassword" runat="server" class="form-group">
                                                <gb:SiteLabel ID="lblOldPassword" runat="server" ForControl="CurrentPassword" ConfigKey="ChangePasswordCurrentPasswordLabel" />
                                                <asp:TextBox ID="CurrentPassword" runat="server" CssClass="form-control" TextMode="password" />
                                                <asp:RequiredFieldValidator ControlToValidate="CurrentPassword" ID="CurrentPasswordRequired"
                                                    Display="Dynamic" runat="server" ValidationGroup="ChangePassword1" />
                                            </asp:Panel>
                                            <div class="form-group clearfix">
                                                <gb:SiteLabel ID="lblNewPassword" runat="server" ForControl="NewPassword" ConfigKey="ChangePasswordNewPasswordLabel" />
                                                <telerik:RadTextBox runat="server" ID="NewPassword" CssClass="form-control" Width="100%" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" TextMode="Password" EnableSingleInputRendering="false">
                                                    <PasswordStrengthSettings ShowIndicator="false"></PasswordStrengthSettings>
                                                </telerik:RadTextBox>
                                                <asp:RequiredFieldValidator ControlToValidate="NewPassword" ID="NewPasswordRequired"
                                                    runat="server" Display="Dynamic" ValidationGroup="ChangePassword1" />
                                                <asp:RegularExpressionValidator ID="NewPasswordRegex" runat="server" ControlToValidate="NewPassword"
                                                    Display="Dynamic" ValidationGroup="ChangePassword1" />
                                                <asp:CustomValidator ID="NewPasswordRulesValidator" runat="server" ControlToValidate="NewPassword"
                                                    Display="Dynamic" ValidationGroup="ChangePassword1" />
                                            </div>
                                            <div class="form-group clearfix">
                                                <gb:SiteLabel ID="lblConfirmNewPassword" runat="server" ForControl="ConfirmNewPassword" ConfigKey="ChangePasswordConfirmNewPasswordLabel" />
                                                <asp:TextBox ID="ConfirmNewPassword" runat="server" CssClass="form-control" TextMode="password" />
                                                <asp:RequiredFieldValidator ControlToValidate="ConfirmNewPassword" ID="ConfirmNewPasswordRequired"
                                                    runat="server" Display="Dynamic" ValidationGroup="ChangePassword1" />
                                                <asp:CompareValidator ControlToCompare="NewPassword" ControlToValidate="ConfirmNewPassword"
                                                    ID="NewPasswordCompare" runat="server" Display="Dynamic" ValidationGroup="ChangePassword1" />
                                            </div>
                                            <div class="form-group clearfix">
                                                <portal:gbLabel ID="FailureText" CssClass="alert alert-danger" runat="server" EnableViewState="false" />
                                            </div>
                                            <asp:Button SkinID="DefaultButton" ID="ChangePasswordPushButton" CommandName="ChangePassword"
                                                Text="Change Password" runat="server" ValidationGroup="ChangePassword1" />
                                            <asp:Button SkinID="DefaultButton" ID="CancelPushButton" Visible="false" CommandName="Cancel" Text="Cancel"
                                                runat="server" CausesValidation="false" />
                                        </div>
                                    </ChangePasswordTemplate>
                                </asp:ChangePassword>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
</asp:Content>