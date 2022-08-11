<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="UpdatePassword.aspx.cs" Inherits="CanhCam.Web.ComicUI.UpdatePassword" %>

<asp:Content ContentPlaceHolderID="phMain" ID="MPContent" runat="server">
    <div class="wrap-secure wrap-recover update-password">
        <asp:Panel ID="pnlResetPassword" runat="server" DefaultButton="btnChangePassword">
            <div class="panel panel-primary">
                <h3 class="mrb20">
                    <asp:Literal ID="litContinue" Text="Vui lòng hoàn thiện các thông tin dưới đây để tiếp tục" runat="server" />
                </h3>
                <div class="form-horizontal">
                    <div class="settingrow form-group">
                        <gb:SiteLabel ID="lblRegisterEmail1" runat="server" ForControl="Email" CssClass="settinglabel control-label col-sm-3"
                            ConfigKey="RegisterEmailLabel" />
                        <div class="col-sm-9">
                            <asp:TextBox ID="Email" runat="server" TabIndex="10" MaxLength="100" CssClass="form-control" />
                            <asp:RequiredFieldValidator ControlToValidate="Email" ID="EmailRequired" runat="server"
                                Display="Dynamic" SetFocusOnError="true" ValidationGroup="ChangePassword1" SkinID="Registration"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="EmailRegex" runat="server" ControlToValidate="Email"
                                Display="Dynamic" SetFocusOnError="true" ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@(([0-9a-zA-Z])+([-\w]*[0-9a-zA-Z])*\.)+[a-zA-Z]{2,9})$"
                                ValidationGroup="ChangePassword1" SkinID="Registration"></asp:RegularExpressionValidator>
                        </div>
                    </div>
                    <div class="settingrow form-group">
                        <gb:SiteLabel ID="lblNewPassword" runat="server" CssClass="settinglabel control-label col-sm-3" ForControl="txtNewPassword" ConfigKey="RegisterPasswordLabel" />
                        <div class="col-sm-9">
                            <telerik:RadTextBox runat="server" ID="txtNewPassword" CssClass="form-control" Width="100%" EnableViewState="false" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" TextMode="Password" EnableSingleInputRendering="false">
                                <PasswordStrengthSettings ShowIndicator="false"></PasswordStrengthSettings>
                            </telerik:RadTextBox>
                            <asp:RequiredFieldValidator ControlToValidate="txtNewPassword" ID="NewPasswordRequired"
                                runat="server" Display="Dynamic" SetFocusOnError="true" ValidationGroup="ChangePassword1">
                            </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="NewPasswordRegex" runat="server" ControlToValidate="txtNewPassword"
                                Display="Dynamic" SetFocusOnError="true" ValidationGroup="ChangePassword1">
                            </asp:RegularExpressionValidator>
                            <asp:CustomValidator ID="NewPasswordRulesValidator" runat="server" ControlToValidate="txtNewPassword"
                                Display="Dynamic" SetFocusOnError="true" ValidationGroup="ChangePassword1">
                            </asp:CustomValidator>
                        </div>
                    </div>
                    <div class="settingrow form-group">
                        <gb:SiteLabel ID="lblConfirmNewPassword" runat="server" CssClass="settinglabel control-label col-sm-3" ForControl="txtConfirmNewPassword" ConfigKey="RegisterConfirmPasswordLabel" />
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtConfirmNewPassword" CssClass="form-control" EnableViewState="false" runat="server" TextMode="password" />
                            <asp:RequiredFieldValidator ControlToValidate="txtConfirmNewPassword" ID="ConfirmNewPasswordRequired"
                                runat="server" Display="Dynamic" SetFocusOnError="true" ValidationGroup="ChangePassword1">
                            </asp:RequiredFieldValidator>
                            <asp:CompareValidator ControlToCompare="txtNewPassword" ControlToValidate="txtConfirmNewPassword"
                                ID="NewPasswordCompare" runat="server" Display="Dynamic" SetFocusOnError="true" ValidationGroup="ChangePassword1">
                            </asp:CompareValidator>
                        </div>
                    </div>
                    <div class="settingrow form-group">
                        <div class="offset-sm-3 col-sm-9">
                            <asp:Button SkinID="DefaultButton" ID="btnChangePassword" CommandName="ChangePassword"
                                Text="Hoàn thành" runat="server" ValidationGroup="ChangePassword1" />
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <portal:gbLabel ID="FailureText" runat="server" CssClass="error" EnableViewState="false" />
    </div>
</asp:Content>