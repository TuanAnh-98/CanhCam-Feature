<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="LoginControl.ascx.cs"
    Inherits="CanhCam.Web.UI.LoginControl" %>

<div class="card">
    <div class="card-body login-card-body">
        <p class="login-box-msg">
            <gb:SiteLabel ID="lblEnterUsernamePassword" runat="server" UseLabelTag="false" />
        </p>
        <portal:SiteLogin ID="LoginCtrl" runat="server" CssClass="loginForm">
            <LayoutTemplate>
                <asp:Panel ID="pnlLContainer" CssClass="panel-body" runat="server" DefaultButton="Login">
                    <portal:gbLabel ID="FailureText" runat="server" CssClass="alert alert-danger" EnableViewState="false" />
                    <div class="input-group mb-3">
                        <gb:SiteLabel ID="lblEmail" runat="server" Visible="false" CssClass="control-label" ForControl="UserName" ConfigKey="SignInEmailLabel" />
                        <gb:SiteLabel ID="lblUserID" runat="server" Visible="false" CssClass="control-label" ForControl="UserName" ConfigKey="ManageUsersLoginNameLabel" />
                        <asp:TextBox ID="UserName" runat="server" CssClass="form-control" MaxLength="100" />
                        <div class="input-group-append">
                            <div class="input-group-text">
                                <span class="fas fa-envelope"></span>
                            </div>
                        </div>
                    </div>
                    <div class="input-group mb-3">
                        <gb:SiteLabel ID="lblPassword" runat="server" Visible="false" CssClass="control-label" ForControl="Password" ConfigKey="SignInPasswordLabel" />
                        <asp:TextBox ID="Password" runat="server" CssClass="form-control" TextMode="password" />
                        <div class="input-group-append">
                            <div class="input-group-text">
                                <span class="fas fa-lock"></span>
                            </div>
                        </div>
                    </div>
                    <asp:Panel class="captcha" ID="divCaptcha" runat="server">
                        <telerik:RadCaptcha ID="captcha" runat="server" EnableRefreshImage="true"
                            CaptchaTextBoxCssClass="form-control" CaptchaTextBoxLabel="" ErrorMessage="<%$Resources:Resource, CaptchaFailureMessage %>"
                            CaptchaTextBoxTitle="<%$Resources:Resource, CaptchaInstructions %>"
                            CaptchaLinkButtonText="<%$Resources:Resource, CaptchaRefreshImage %>" />
                    </asp:Panel>
                    <div class="row items-center">
                        <div class="col-12">
                            <div class="icheck-primary">
                                <asp:CheckBox ID="RememberMe" runat="server" />
                            </div>
                        </div>
                        <div class="col-12">
                            <asp:Button CssClass="btn btn-primary btn-block" ID="Login" CommandName="Login" runat="server" Text="Login" />
                        </div>
                    </div>

                    <div class="flex justify-between mt-3">
                        <p class="mb-0">
                            <asp:HyperLink ID="lnkPasswordRecovery" runat="server" />
                        </p>
                        <p class="mb-0">
                            <asp:HyperLink ID="lnkRegisterExtraLink" runat="server" />
                        </p>
                    </div>
                    <div class="flex justify-between mt-3"> 
                    <div class="cart-login" style="box-shadow:none;"> 
                       <div class="social-login">
                           <a class="btn facebook-login facebook" data-url="/Account/LoginFacebook.aspx" href="#"> 
                            <em class="ri-facebook-fill"></em><span>Đăng nhập bằng Facebook</span>
                           </a>
                           <a class="btn google-login google" data-url="/Account/LoginGooglePlus.aspx" href="#">  
                            <em class="ri-google-fill"></em><span>Đăng nhập bằng Google</span>
                           </a>
                       </div>  
                       </div>  
                   </div> 
                </asp:Panel>
            </LayoutTemplate>
        </portal:SiteLogin>
    </div>
</div>