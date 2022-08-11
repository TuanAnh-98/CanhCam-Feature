<%@ Page Language="c#" MaintainScrollPositionOnPostback="true" CodeBehind="UserProfile.aspx.cs"
    MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="CanhCam.Web.AccountUI.UserProfilePage" %>

<%@ Register TagPrefix="Site" TagName="AccountMenu" Src="~/Account/AccountMenu.ascx" %>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <div class="container">
        <div class="row flex flex-wrap">
            <Site:AccountMenu ID="accountMenu" runat="server" />
            <div class="col-xs-12 col-lg-9">
                <asp:Panel ID="pnlUser" runat="server" CssClass="user-page clearfix" DefaultButton="btnUpdate">
                    <h1 class="postname">
                        <asp:Literal ID="litHeading" Text="<%$Resources:Resource, UserProfileMyProfileLabel %>" runat="server" />
                    </h1>
                    <portal:NotifyMessage ID="message" runat="server" />
                    <div id="divtabs" visible="false" runat="server" class="gb-tabs">
                        <div id="divOpenID" runat="server" class="form-group">
                            <gb:SiteLabel ID="SiteLabel4" runat="server" ForControl="OpenIdLogin1" CssClass="settinglabel"
                                ConfigKey="ManageUsersOpenIDURILabel" />
                            <div class="input">
                                <asp:Label ID="lblOpenID" runat="server" />
                                <asp:HyperLink ID="lnkOpenIDUpdate" runat="server" />
                                <portal:OpenIdRpxNowLink ID="rpxLink" runat="server" Embed="false" UseOverlay="true" Visible="false" />
                            </div>
                        </div>
                        <asp:Panel ID="pnlSecurityQuestion" runat="server">
                            <div class="form-group">
                                <gb:SiteLabel ID="SiteLabel2" runat="server" ForControl="txtPasswordQuestion" CssClass="settinglabel"
                                    ConfigKey="UsersSecurityQuestionLabel" />
                                <asp:TextBox ID="txtPasswordQuestion" runat="server" TabIndex="10" MaxLength="255"
                                    CssClass="widetextbox input"></asp:TextBox>
                                <asp:RequiredFieldValidator ControlToValidate="txtPasswordQuestion" ID="QuestionRequired"
                                    runat="server" Display="None" ValidationGroup="profile" SkinID="Profile"></asp:RequiredFieldValidator>
                            </div>
                            <div class="form-group">
                                <gb:SiteLabel ID="SiteLabel3" runat="server" ForControl="txtPasswordAnswer" CssClass="settinglabel"
                                    ConfigKey="UsersSecurityAnswerLabel" />
                                <asp:TextBox ID="txtPasswordAnswer" runat="server" TabIndex="10" MaxLength="255"
                                    CssClass="widetextbox input"></asp:TextBox>
                                <asp:RequiredFieldValidator ControlToValidate="txtPasswordAnswer" ID="AnswerRequired"
                                    runat="server" Display="None" ValidationGroup="profile" SkinID="Profile"></asp:RequiredFieldValidator>
                            </div>
                        </asp:Panel>
                        <div id="divEditorPreference" runat="server" visible="false" class="form-group">
                            <gb:SiteLabel ID="SiteLabel29" runat="server" ForControl="ddEditorProviders" CssClass="settinglabel"
                                ConfigKey="SiteSettingsEditorProviderLabel" EnableViewState="false" />
                            <asp:DropDownList ID="ddEditorProviders" DataTextField="name" DataValueField="name"
                                EnableViewState="true" EnableTheming="false" TabIndex="10" runat="server" CssClass="input">
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <gb:SiteLabel ID="lblCreatedDateLabel" runat="server" CssClass="settinglabel" ConfigKey="ManageUsersCreatedDateLabel" />
                            <asp:Label ID="lblCreatedDate" runat="server" CssClass="input"></asp:Label>
                        </div>
                        <div id="divAvatar" runat="server" class="form-group">
                            <gb:SiteLabel ID="lblAvatar" runat="server" CssClass="settinglabel" ConfigKey="UserProfileAvatarLabel" />
                            <div class="input">
                                <portal:Avatar ID="userAvatar" runat="server" CssClass="input" />
                                <asp:HyperLink ID="lnkAvatarUpld" CssClass="cp-link" runat="server" />
                                <asp:ImageButton ID="btnUpdateAvartar" runat="server" />
                            </div>
                        </div>
                        <div id="divLiveMessenger" runat="server" class="form-group">
                            <gb:SiteLabel ID="SiteLabel14" runat="server" ForControl="chkEnableLiveMessengerOnProfile"
                                CssClass="settinglabel" ConfigKey="EnableLiveMessengerLabel" />
                            <div class="input">
                                <asp:CheckBox ID="chkEnableLiveMessengerOnProfile" runat="server" />
                                <asp:HyperLink ID="lnkAllowLiveMessenger" runat="server" Text="Enable Live Messenger" />
                            </div>
                        </div>
                        <div id="divTimeZone" runat="server" visible="false" class="form-group">
                            <gb:SiteLabel ID="SiteLabel5" runat="server" CssClass="settinglabel" ConfigKey="TimeZone" />
                            <portal:TimeZoneIdSetting ID="timeZoneSetting" runat="server" />
                        </div>
                        <div class="form-group">
                            <asp:HyperLink ID="lnkPubProfile" Visible="false" runat="server" CssClass="settinglabel cp-link" />
                        </div>
                    </div>
                    <div class="row flex flex-wrap">
                        <div class="col-xs-12">
                            <div class="account-info clearfix">
                                <h2 class="posttitle">Thông tin tài khoản</h2>
                                <div class="account-form clearfix">
                                    <div id="divName" runat="server" visible="false">
                                        <gb:SiteLabel ID="lblUserName" runat="server" ForControl="txtName" CssClass="settinglabel control-label"
                                            ConfigKey="ManageUsersUserNameLabel" />
                                        <asp:TextBox ID="txtName" Enabled="false" runat="server" TabIndex="10" MaxLength="100" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="rfvName" runat="server" ValidationGroup="profile"
                                            Display="none" ErrorMessage="" ControlToValidate="txtName" SkinID="Profile" />
                                        <asp:RegularExpressionValidator ID="regexUserName" runat="server" ControlToValidate="txtName"
                                            Display="None" ValidationExpression="" ValidationGroup="profile" Enabled="false"
                                            SkinID="Profile" />
                                        <asp:CustomValidator ID="FailSafeUserNameValidator" runat="server" ControlToValidate="txtName"
                                            Display="None" ValidationGroup="profile" EnableClientScript="false" SkinID="Profile" />
                                    </div>
                                    <div id="divUserName" runat="server" visible="false" class="form-group clearfix">
                                        <gb:SiteLabel ID="lblLoginName" runat="server" CssClass="settinglabel control-label" ConfigKey="ManageUsersLoginNameLabel" />
                                        <asp:TextBox ID="txtLoginName" Enabled="false" runat="server" TabIndex="10" MaxLength="100" CssClass="form-control" />
                                    </div>
                                    <div class="form-group clearfix">
                                        <gb:SiteLabel ID="lblEmail" runat="server" ForControl="txtEmail" CssClass="settinglabel control-label"
                                            ConfigKey="ManageUsersEmailLabel" />
                                        <div class="">
                                            <asp:TextBox ID="txtEmail" Enabled="false" runat="server" TabIndex="10" CssClass="form-control" />
                                            <asp:RegularExpressionValidator ID="regexEmail" runat="server" ValidationGroup="profile"
                                                ErrorMessage="" ControlToValidate="txtEmail" Display="None" SkinID="Profile" />
                                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ValidationGroup="profile"
                                                ErrorMessage="" ControlToValidate="txtEmail" Display="none" SkinID="Profile" />
                                        </div>
                                    </div>
                                    <asp:Panel ID="pnlProfileProperties" runat="server">
                                    </asp:Panel>
                                    <div class="button-wrap">
                                        <div class="">
                                            <asp:Button SkinID="DefaultButton" ID="btnUpdate" runat="server" ValidationGroup="profile" />
                                            <asp:HyperLink ID="lnkChangePassword" CssClass="changepass-link" Visible="false" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
</asp:Content>