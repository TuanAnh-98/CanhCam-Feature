﻿<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="JobApplyControl.ascx.cs"
    Inherits="CanhCam.Web.RecruitmentUI.JobApplyControl" %>

<%@ Register TagPrefix="Site" Assembly="CanhCam.Features.Recruitment" Namespace="CanhCam.Web.RecruitmentUI" %>
<Site:RecruitmentDisplaySettings ID="displaySettings" runat="server" />

<div class="container-fluid">
    <div class="apply-frm row">
        <asp:UpdatePanel ID="upContact" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlNewComment" DefaultButton="btnPostComment" runat="server">
                    <h4>
                        <asp:Literal ID="litNote" Text="<%$Resources:RecruitmentResources, JobNoteLabel %>" EnableViewState="false"
                            runat="server" />
                        (<span class="required"><asp:Literal ID="litNoteStarRequired" Text="<%$Resources:RecruitmentResources, JobRequiredStarLabel %>"
                            EnableViewState="false" runat="server" /></span> :
                        <asp:Literal ID="litNoteRequired" Text="<%$Resources:RecruitmentResources, JobRequiredLabel %>"
                            EnableViewState="false" runat="server" />)
                    </h4>
                    <div class="col-left">
                        <div class="form-group">
                            <label class="label">
                                <asp:Literal ID="litPositionLabel" Text="<%$Resources:RecruitmentResources, JobPositionLabel %>"
                                    EnableViewState="false" runat="server" />
                                (<span class="required"><asp:Literal ID="litPositionRequired" Text="<%$Resources:RecruitmentResources, JobRequiredStarLabel %>"
                                    EnableViewState="false" runat="server" /></span>)
                            </label>
                            <asp:TextBox ID="txtPosition" CssClass="form-control" MaxLength="255" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqPosition" ToolTip="<%$Resources:RecruitmentResources, JobPositionRequiredLabel %>"
                                ValidationGroup="jobapply" runat="server" Display="Dynamic" ControlToValidate="txtPosition"
                                SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group">
                            <label class="label">
                                <asp:Literal ID="litFullName" Text="<%$Resources:RecruitmentResources, JobFullNameLabel %>"
                                    EnableViewState="false" runat="server" />
                                (<span class="required"><asp:Literal ID="litFullNameRequired" Text="<%$Resources:RecruitmentResources, JobRequiredStarLabel %>"
                                    EnableViewState="false" runat="server" /></span>)
                            </label>
                            <asp:TextBox ID="txtFullName" CssClass="form-control" MaxLength="255" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqFullName" ToolTip="<%$Resources:RecruitmentResources, JobFullNameRequiredLabel %>"
                                ValidationGroup="jobapply" runat="server" Display="Dynamic" ControlToValidate="txtFullName"
                                SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group">
                            <label class="label">
                                <asp:Literal ID="litAddress" Text="<%$Resources:RecruitmentResources, JobAddressLabel %>"
                                    EnableViewState="false" runat="server" />
                                (<span class="required"><asp:Literal ID="litAddressRequired" Text="<%$Resources:RecruitmentResources, JobRequiredStarLabel %>"
                                    EnableViewState="false" runat="server" /></span>)
                            </label>
                            <asp:TextBox ID="txtAddress" CssClass="form-control" MaxLength="255" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqAddress" ToolTip="<%$Resources:RecruitmentResources, JobAddressRequiredLabel %>"
                                ValidationGroup="jobapply" runat="server" Display="Dynamic" ControlToValidate="txtAddress"
                                SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group">
                            <label class="label">
                                <asp:Literal ID="litEmail" Text="<%$Resources:RecruitmentResources, JobEmailLabel %>" EnableViewState="false"
                                    runat="server" />
                                (<span class="required"><asp:Literal ID="litEmailRequired" Text="<%$Resources:RecruitmentResources, JobRequiredStarLabel %>"
                                    EnableViewState="false" runat="server" /></span>)
                            </label>
                            <asp:TextBox ID="txtEmail" CssClass="form-control" MaxLength="255" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqEmail" ToolTip="<%$Resources:RecruitmentResources, JobEmailRequiredLabel %>"
                                ValidationGroup="jobapply" runat="server" Display="Dynamic" ControlToValidate="txtEmail"
                                SetFocusOnError="true"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="regexEmail" ToolTip="<%$Resources:RecruitmentResources, JobValidEmailLabel %>"
                                runat="server" Display="Dynamic" ValidationGroup="jobapply" ControlToValidate="txtEmail"
                                ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@(([0-9a-zA-Z])+([-\w]*[0-9a-zA-Z])*\.)+[a-zA-Z]{2,9})$"
                                SetFocusOnError="true"></asp:RegularExpressionValidator>
                        </div>
                        <div class="form-group">
                            <label class="label">
                                <asp:Literal ID="litPhone" Text="<%$Resources:RecruitmentResources, JobPhoneLabel %>" EnableViewState="false"
                                    runat="server" />
                                (<span class="required"><asp:Literal ID="litPhoneRequired" Text="<%$Resources:RecruitmentResources, JobRequiredStarLabel %>"
                                    EnableViewState="false" runat="server" /></span>)
                            </label>
                            <asp:TextBox ID="txtPhone" CssClass="form-control" MaxLength="255" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqPhone" ToolTip="<%$Resources:RecruitmentResources, JobPhoneRequiredLabel %>"
                                ValidationGroup="jobapply" runat="server" Display="Dynamic" ControlToValidate="txtPhone"
                                SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-right">
                        <div class="form-group">
                            <label class="label">
                                <asp:Literal ID="litMessageLabel" Text="<%$Resources:RecruitmentResources, JobMessageLabel %>"
                                    EnableViewState="false" runat="server" />
                                (<span class="required"><asp:Literal ID="litMessageRequired" Text="<%$Resources:RecruitmentResources, JobRequiredStarLabel %>"
                                    EnableViewState="false" runat="server" /></span>)
                            </label>
                            <asp:TextBox ID="txtMessage" CssClass="form-control" TextMode="MultiLine" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqMessage" ToolTip="<%$Resources:RecruitmentResources, JobEmptyMessageWarning %>"
                                ValidationGroup="jobapply" runat="server" Display="Dynamic" ControlToValidate="txtMessage"
                                SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group attachfile1">
                            <label class="label">
                                <asp:Literal ID="litAttachFile1" Text="<%$Resources:RecruitmentResources, JobAttachFile1Label %>"
                                    EnableViewState="false" runat="server" />
                                (<span class="required"><asp:Literal ID="litAttachFileNote1" Text="rar,zip,doc,docx,pdf, <1MB" runat="server" /></span>)
                            </label>
                            <div class="left">
                                <telerik:RadAsyncUpload ID="uplAttachFile1" SkinID="radAsyncUploadSkin" MultipleFileSelection="Disabled"
                                    AllowedFileExtensions="rar,zip,doc,docx,pdf" runat="server" />
                            </div>
                            <div class="clear">
                            </div>
                        </div>
                        <div class="form-group attachfile2">
                            <label class="label">
                                <asp:Literal ID="litAttachFile2" Text="<%$Resources:RecruitmentResources, JobAttachFile2Label %>"
                                    EnableViewState="false" runat="server" />
                                (<span class="required"><asp:Literal ID="litAttachFileNote2" Text="rar,zip,doc,docx,pdf, <1MB" runat="server" /></span>)
                            </label>
                            <div class="left">
                                <telerik:RadAsyncUpload ID="uplAttachFile2" SkinID="radAsyncUploadSkin" MultipleFileSelection="Disabled"
                                    AllowedFileExtensions="rar,zip,doc,docx,pdf" runat="server" />
                            </div>
                            <div class="clear">
                            </div>
                        </div>
                    </div>
                    <div class="form-group frm-captcha" id="divCaptcha" runat="server">
                        <div class="frm-captcha-input">
                            <label class="label">
                                <asp:Literal ID="litCaptcha" Text="<%$Resources:Resource, CaptchaInstructions %>"
                                    EnableViewState="false" runat="server" />
                            </label>
                            <asp:TextBox ID="txtCaptcha" runat="server" MaxLength="5"></asp:TextBox>
                        </div>
                        <telerik:RadCaptcha ID="captcha" runat="server" ValidationGroup="jobapply" EnableRefreshImage="true" 
                            ValidatedTextBoxID="txtCaptcha" CaptchaImage-RenderImageOnly="true" Width="50%"
                            CaptchaTextBoxLabel="" ErrorMessage="<%$Resources:Resource, CaptchaFailureMessage %>" 
                            CaptchaTextBoxTitle="<%$Resources:Resource, CaptchaInstructions %>" 
                            CaptchaLinkButtonText="<%$Resources:Resource, CaptchaRefreshImage %>" />
                    </div>
                    <div class="form-group">
                        <div class="frm-btnwrap">
                            <div class="frm-btn">
                                <asp:Button CssClass="btn btn-default" ID="btnPostComment" runat="server" Text="Submit"
                                    ValidationGroup="jobapply" />
                            </div>
                        </div>
                    </div>
                    <div class="clear"></div>
                </asp:Panel>
                <portal:gbLabel ID="lblMessage" runat="server" CssClass="alert alert-success" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>