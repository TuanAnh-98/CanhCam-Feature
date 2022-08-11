<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ProductSoldOutletterControl.ascx.cs"
    Inherits="CanhCam.Web.ProductUI.ProductSoldOutletterControl" %>

<div style="display: none;" id="product-sold-out-letter">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Panel runat="server" ID="pnlfrmLetter" CssClass="form-wrap">
                <h3>
                    <asp:Literal runat="server" ID="litTitle" />
                </h3>
                <div class="form-group" runat="server" id="divFullName" visible="false">
                    <gb:SiteLabel runat="server" ForControl="txtFullName" ShowRequired="true" CssClass="label"
                        ConfigKey="FullNameLabel" ResourceFile="ProductResources" />
                    <asp:TextBox runat="server" ID="txtFullName" />
                </div>
                <div class="form-group" runat="server" id="divEmail">
                    <gb:SiteLabel runat="server" ForControl="txtEmail" ShowRequired="true" CssClass="label"
                        ConfigKey="EmailLabel" ResourceFile="ProductResources" />
                    <asp:TextBox runat="server" ID="txtEmail" />
                    <asp:RequiredFieldValidator ErrorMessage="<%$Resources:ProductResources, EmailRequiredLabel %>"
                        ValidationGroup="ProductSoldOutletter" runat="server" Display="Dynamic" ControlToValidate="txtEmail"
                        SetFocusOnError="true" />
                    <asp:RegularExpressionValidator ErrorMessage="<%$Resources:ProductResources, ValidEmailLabel %>"
                        runat="server" Display="Dynamic" ValidationGroup="ProductSoldOutletter" ControlToValidate="txtEmail"
                        ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@(([0-9a-zA-Z])+([-\w]*[0-9a-zA-Z])*\.)+[a-zA-Z]{2,9})$"
                        SetFocusOnError="true" />
                </div>
                <div class="form-group" runat="server" id="divPhone" >
                    <gb:SiteLabel runat="server" ForControl="txtPhoneNumber" ShowRequired="true" CssClass="label"
                        ConfigKey="PhoneNumberLabel" ResourceFile="ProductResources" />
                    <asp:TextBox runat="server" ID="txtPhoneNumber" />
                </div>
                <div class="form-group" runat="server" id="divQuantity" visible="false">
                    <gb:SiteLabel runat="server" ForControl="txtQuantity" ShowRequired="true" CssClass="label"
                        ConfigKey="QuantityLabel" ResourceFile="ProductResources" />
                    <asp:TextBox runat="server" ID="txtQuantity" Text="1" />
                </div>
                <div class="form-btn">
                    <asp:Button runat="server" ID="btnSubmit" Text="<%$Resources:ProductResources, SubmitButton %>" CausesValidation="true" ValidationGroup="ProductSoldOutletter" />
                </div>
            </asp:Panel>
            <portal:gbLabel ID="lblMessage" runat="server" CssClass="alert alert-success" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>