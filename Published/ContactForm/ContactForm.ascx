<%@ Control Language="c#" AutoEventWireup="false" CodeBehind="ContactForm.ascx.cs"
    Inherits="CanhCam.Web.ContactUI.ContactForm" %>
    <div class="wrap-contact">
        <asp:UpdatePanel ID="upContact" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlSend" runat="server" DefaultButton="btnSend">
                    <div class="row">
                        <div class="form-group ct-name col-lg-6">
                            <gb:SiteLabel ID="lblName" runat="server" ForControl="txtName" ShowRequired="true"
                                CssClass="label" ConfigKey="ContactFormYourNameLabel"
                                ResourceFile="ContactFormResources" />
                            <asp:TextBox ID="txtName" CssClass="form-control" runat="server" MaxLength="100">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqName"
                                ToolTip="<%$Resources:ContactFormResources, ContactFormNameRequiredLabel %>"
                                ValidationGroup="Contact" runat="server" Display="Dynamic" ControlToValidate="txtName"
                                SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group ct-address col-lg-6" visible="false" runat="server">
                            <gb:SiteLabel ID="lblAddress" runat="server" ForControl="txtAddress" ShowRequired="false"
                                CssClass="label" ConfigKey="ContactFormYourAddressLabel"
                                ResourceFile="ContactFormResources" />
                            <asp:TextBox ID="txtAddress" CssClass="form-control" runat="server" MaxLength="255">
                            </asp:TextBox>
                            <%--<asp:RequiredFieldValidator ID="reqAddress"
                                ToolTip="<%$Resources:ContactFormResources, ContactFormAddressRequiredLabel %>"
                                ValidationGroup="Contact" runat="server" Display="Dynamic"
                                ControlToValidate="txtAddress" SetFocusOnError="true">
                                </asp:RequiredFieldValidator>--%>
                        </div>
                        <div class="form-group ct-email col-lg-6">
                            <gb:SiteLabel ID="lblEmail" runat="server" ForControl="txtEmail" ShowRequired="true"
                                CssClass="label" ConfigKey="ContactFormYourEmailLabel"
                                ResourceFile="ContactFormResources" />
                            <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server" MaxLength="255">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqEmail"
                                ToolTip="<%$Resources:ContactFormResources, ContactFormEmailRequiredLabel %>"
                                ValidationGroup="Contact" runat="server" Display="Dynamic" ControlToValidate="txtEmail"
                                SetFocusOnError="true"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="regexEmail"
                                ToolTip="<%$Resources:ContactFormResources, ContactFormValidEmailLabel %>"
                                runat="server" Display="Dynamic" ValidationGroup="Contact" ControlToValidate="txtEmail"
                                ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@(([0-9a-zA-Z])+([-\w]*[0-9a-zA-Z])*\.)+[a-zA-Z]{2,9})$"
                                SetFocusOnError="true"></asp:RegularExpressionValidator>
                        </div>
                        <asp:Panel ID="pnlToAddresses" runat="server" Visible="false"
                            CssClass="form-group ct-toaddress">
                            <gb:SiteLabel ID="lblToAddresses" runat="server" ForControl="ddToAddresses"
                                ShowRequired="false" CssClass="label" ConfigKey="ToLabel"
                                ResourceFile="ContactFormResources" />
                            <asp:DropDownList ID="ddToAddresses" CssClass="form-control" runat="server"
                                EnableViewState="true" EnableTheming="false" />
                        </asp:Panel>
                        <div class="form-group ct-phone col-lg-6">
                            <gb:SiteLabel ID="lblPhone" runat="server" ForControl="txtPhone" ShowRequired="false"
                                CssClass="label" ConfigKey="ContactFormYourPhoneLabel"
                                ResourceFile="ContactFormResources" />
                            <asp:TextBox ID="txtPhone" CssClass="form-control" runat="server" MaxLength="255">
                            </asp:TextBox>
                            <%--<asp:RequiredFieldValidator ID="reqPhone"
                                ToolTip="<%$Resources:ContactFormResources, ContactFormPhoneRequiredLabel %>"
                                ValidationGroup="Contact" runat="server" Display="Dynamic" ControlToValidate="txtPhone"
                                SetFocusOnError="true">
                                </asp:RequiredFieldValidator>--%>
                                <asp:RegularExpressionValidator ID="PhoneRegex" ToolTip="<%$Resources:ContactFormResources, ContactFormValidPhoneLabel %>" runat="server" ControlToValidate="txtPhone"
                                                    Display="Dynamic" SetFocusOnError="true" ValidationExpression="^\+?[0-9]{3}-?[0-9]{6,12}$"
                                                    ValidationGroup="Contact">
                            </asp:RegularExpressionValidator>
                        </div>
                        <div id="divFax" visible="false" class="form-group ct-fax" runat="server">
                            <gb:SiteLabel ID="lblFax" runat="server" ForControl="txtFax" ShowRequired="false"
                                CssClass="label" ConfigKey="ContactFormYourFaxLabel"
                                ResourceFile="ContactFormResources" />
                            <asp:TextBox ID="txtFax" runat="server" CssClass="form-control" MaxLength="255">
                            </asp:TextBox>
                        </div>
                        <div class="form-group ct-subject col-lg-6">
                            <gb:SiteLabel ID="lblSubject" runat="server" ForControl="txtSubject" ShowRequired="false"
                                CssClass="label" ConfigKey="ContactFormSubjectLabel"
                                ResourceFile="ContactFormResources" />
                            <asp:TextBox ID="txtSubject" CssClass="form-control" runat="server" MaxLength="255">
                            </asp:TextBox>
                        </div>
                        <div class="form-group ct-message col-12">
                            <gb:SiteLabel ID="lblMessageLabel" runat="server" ForControl="txtMessage"
                                ShowRequired="true" CssClass="label" ConfigKey="ContactFormMessageLabel"
                                ResourceFile="ContactFormResources" />
                            <asp:TextBox ID="txtMessage" CssClass="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqMessage"
                                ToolTip="<%$Resources:ContactFormResources, ContactFormEmptyMessageWarning %>"
                                ValidationGroup="Contact" runat="server" Display="Dynamic"
                                ControlToValidate="txtMessage" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group ct-captcha col-12" id="divCaptcha" runat="server">
                            <gb:SiteLabel ID="lblCaptcha" runat="server" ShowRequired="true" CssClass="label"
                                ConfigKey="CaptchaInstructions" ResourceFile="Resource" />
                            <div class="left">
                                <telerik:RadCaptcha ID="captcha" runat="server" ValidationGroup="Contact"
                                    EnableRefreshImage="true" CaptchaTextBoxLabel=""
                                    CaptchaTextBoxCssClass="form-control"
                                    ErrorMessage="<%$Resources:Resource, CaptchaFailureMessage %>"
                                    CaptchaTextBoxTitle="<%$Resources:Resource, CaptchaInstructions %>"
                                    CaptchaLinkButtonText="<%$Resources:Resource, CaptchaRefreshImage %>" />
                            </div>
                        </div>
                        <div class="form-group ct-button col-12">
                            <div class="frm-btnwrap">
                                <div class="frm-btn">
                                    <asp:Button CssClass="ct-button btn btn-default" ID="btnSend" runat="server"
                                        ValidationGroup="Contact" Text="Send" CausesValidation="true" />
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <portal:gbLabel ID="lblMessage" runat="server" CssClass="alert alert-success" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>