<%@ Page Language="c#" CodeBehind="ReviewEdit.aspx.cs" MasterPageFile="~/App_MasterPages/DialogMaster.Master"
    AutoEventWireup="false" Inherits="CanhCam.Web.ProductUI.ReviewEditPage" %>

<%@ Import Namespace="CanhCam.Web.ProductUI" %>
<asp:Content ContentPlaceHolderID="phMain" ID="MPContent" runat="server">
    <div class="admin-content col-md-12">
        <div class="heading">
            <portal:HeadingControl ID="heading" runat="server" />
        </div>
        <portal:NotifyMessage ID="message" runat="server" />
        <div class="workplace form-horizontal">
            <div id="divTitle" visible="false" runat="server" class="settingrow form-group">
                <gb:SiteLabel ID="lblCommentTitle" runat="server" ForControl="txtCommentTitle" CssClass="settinglabel control-label col-xs-5"
                    ConfigKey="CommentTitle" ResourceFile="ProductResources" EnableViewState="false"></gb:SiteLabel>
                <div class="col-xs-7">
                    <asp:TextBox ID="txtCommentTitle" runat="server" MaxLength="100" EnableViewState="false" />
                </div>
            </div>
            <div id="divEmail" runat="server" class="settingrow form-group">
                <gb:SiteLabel ID="lblEmail" runat="server" ForControl="txtEmail" CssClass="settinglabel control-label col-xs-5"
                    ConfigKey="CommentEmail" ResourceFile="ProductResources" EnableViewState="false"></gb:SiteLabel>
                <div class="col-xs-7">
                    <asp:TextBox ID="txtEmail" runat="server" MaxLength="100" EnableViewState="false" />
                    <%--<asp:RequiredFieldValidator ControlToValidate="txtEmail" ID="EmailRequired" runat="server"
                Display="Dynamic" SetFocusOnError="true" ValidationGroup="ProductComments"></asp:RequiredFieldValidator>--%>
                    <asp:RegularExpressionValidator ID="EmailRegex" runat="server" ControlToValidate="txtEmail"
                        ErrorMessage="Email sai định dạng." Display="Dynamic" SetFocusOnError="true"
                        ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@(([0-9a-zA-Z])+([-\w]*[0-9a-zA-Z])*\.)+[a-zA-Z]{2,9})$"
                        ValidationGroup="ProductComments"></asp:RegularExpressionValidator>
                </div>
            </div>
            <div id="divFullName" runat="server" class="settingrow form-group">
                <gb:SiteLabel ID="lblFullName" runat="server" ForControl="txtFullName" CssClass="settinglabel control-label col-xs-5"
                    ConfigKey="CommentFullName" ResourceFile="ProductResources" EnableViewState="false"></gb:SiteLabel>
                <div class="col-xs-7">
                    <asp:TextBox ID="txtFullName" runat="server" MaxLength="100" EnableViewState="false" />
                </div>
            </div>
            <div id="divQuestion" visible="false" runat="server" class="settingrow form-group">
                <gb:SiteLabel ID="lblQuestion" runat="server" CssClass="settinglabel control-label col-xs-5"
                    Text="Câu hỏi" ResourceFile="ProductResources" EnableViewState="false"></gb:SiteLabel>
                <div class="col-xs-7">
                    <asp:Literal ID="litQuestion" runat="server" />
                </div>
            </div>
            <div id="divComment" runat="server" class="settingrow form-group">
                <gb:SiteLabel ID="lblComment" runat="server" CssClass="settinglabel control-label col-xs-5"
                    Text="Nội dung" ResourceFile="ProductResources" EnableViewState="false" />
                <div class="col-xs-7">
                    <asp:TextBox ID="txtComment" TextMode="MultiLine" Rows="5" Width="300" runat="server" />
                    <asp:RequiredFieldValidator ID="reqMessage" ToolTip="<%$Resources:ContactFormResources, ContactFormEmptyMessageWarning %>"
                        ValidationGroup="ProductComments" runat="server" Display="Dynamic" ControlToValidate="txtComment"
                        SetFocusOnError="true"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div id="divRating" visible="false" runat="server" class="settingrow form-group">
                <gb:SiteLabel ID="lblRating" runat="server" ForControl="ratRating" CssClass="settinglabel control-label col-xs-5"
                    ConfigKey="CommentRating" ResourceFile="ProductResources" EnableViewState="false"></gb:SiteLabel>
                <div class="col-xs-7">
                    <telerik:RadRating ID="ratRating" Enabled="false" SkinID="radRatingSkin" runat="server" />
                </div>
            </div>
            <div id="divPosition" visible="false" runat="server" class="settingrow form-group">
                <gb:SiteLabel ID="lblPosition" runat="server" CssClass="settinglabel control-label col-xs-5"
                    Text="Vị trí đặc biệt" ResourceFile="ProductResources" EnableViewState="false"></gb:SiteLabel>
                <div class="col-xs-7">
                    <asp:CheckBox ID="ckbPosition" runat="server" />
                </div>
            </div>
            <div id="divCreatedDate" visible="false" runat="server" class="settingrow form-group">
                <gb:SiteLabel ID="lblCreatedDate" runat="server" CssClass="settinglabel control-label col-xs-5"
                    Text="Ngày" ResourceFile="ProductResources" EnableViewState="false"></gb:SiteLabel>
                <div class="col-xs-7">
                    <asp:Literal ID="litCreatedDate" runat="server" />
                </div>
            </div>
            <div id="divIpAddress" visible="false" runat="server" class="settingrow form-group">
                <gb:SiteLabel ID="lblIpAddress" runat="server" CssClass="settinglabel control-label col-xs-5"
                    Text="Địa chỉ IP" ResourceFile="ProductResources" EnableViewState="false"></gb:SiteLabel>
                <div class="col-xs-7">
                    <asp:Literal ID="litIpAddress" runat="server" />
                </div>
            </div>
            <div class="settingrow form-group">
                <label class="settinglabel control-label col-xs-5">
                    &nbsp;</label>
                <div class="col-xs-7">
                    <asp:Button SkinID="InsertButton" Visible="false" ID="btnInsert" ValidationGroup="ProductComments"
                        Text="<%$Resources:Resource, InsertButton %>" runat="server" />
                    <asp:Button SkinID="InsertButton" Visible="false" ID="btnInsertAndClose" ValidationGroup="ProductComments"
                        Text="<%$Resources:Resource, InsertAndCloseButton %>" runat="server" />
                    <asp:Button SkinID="UpdateButton" Visible="false" ID="btnUpdate" ValidationGroup="ProductComments"
                        Text="<%$Resources:Resource, UpdateButton %>" runat="server" />
                    <asp:Button SkinID="UpdateButton" Visible="false" ID="btnUpdateAndClose" ValidationGroup="ProductComments"
                        Text="<%$Resources:Resource, UpdateAndCloseButton %>" runat="server" />
                    <asp:Button SkinID="DeleteButton" Visible="false" ID="btnDelete" runat="server"
                        Text="<%$Resources:Resource, DeleteButton %>" CausesValidation="false" />
                </div>
            </div>
            <portal:SessionKeepAliveControl ID="ka1" runat="server" />
        </div>
    </div>
    <style type="text/css">
        .settingrow {
            display: block
        }
    </style>
</asp:Content>