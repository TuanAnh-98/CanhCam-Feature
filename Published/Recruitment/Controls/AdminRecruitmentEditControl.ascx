<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="AdminRecruitmentEditControl.ascx.cs" Inherits="CanhCam.Web.RecruitmentUI.AdminRecruitmentEditControl" %>

<%@ Register TagPrefix="Site" Assembly="CanhCam.Features.Recruitment" Namespace="CanhCam.Web.RecruitmentUI" %>


<Site:RecruitmentDisplaySettings ID="displaySettings" runat="server" />
<portal:BreadcrumbAdmin ID="breadcrumb" runat="server"
CurrentPageTitle="<%$Resources:RecruitmentResources, RecruitmentAddNewTitle %>" CurrentPageUrl="~/Recruitment/AdminCP/RecruitmentEdit.aspx" />
<div class="admin-content col-md-12">
    <asp:UpdatePanel ID="upButton" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <portal:HeadingPanel ID="heading" runat="server">
                <asp:Button SkinID="UpdateButton" ID="btnUpdate" Text="<%$Resources:Resource, UpdateButton %>" ValidationGroup="recruitment" runat="server" />
                <asp:Button SkinID="UpdateButton" ID="btnUpdateAndNew" Text="<%$Resources:Resource, UpdateAndNewButton %>" ValidationGroup="recruitment" runat="server" />
                <asp:Button SkinID="UpdateButton" ID="btnUpdateAndClose" Text="<%$Resources:Resource, UpdateAndCloseButton %>" ValidationGroup="recruitment" runat="server" />
                <asp:Button SkinID="InsertButton" ID="btnInsert" Text="<%$Resources:Resource, InsertButton %>" ValidationGroup="recruitment" runat="server" />
                <asp:Button SkinID="InsertButton" ID="btnInsertAndNew" Text="<%$Resources:Resource, InsertAndNewButton %>" ValidationGroup="recruitment" runat="server" />
                <asp:Button SkinID="InsertButton" ID="btnInsertAndClose" Text="<%$Resources:Resource, InsertAndCloseButton %>" ValidationGroup="recruitment" runat="server" />
                <asp:Button SkinID="DefaultButton" ID="btnCopyModal" Visible="false" data-toggle="modal" data-target="#pnlModal" Text="Copy" runat="server" CausesValidation="false" />
                <asp:HyperLink SkinID="DefaultButton" ID="lnkPreview" Visible="false" runat="server" />
                <asp:Button SkinID="DeleteButton" ID="btnDelete" runat="server" Text="Delete this item" CausesValidation="false" />
                <asp:Button SkinID="DeleteButton" ID="btnDeleteLanguage" Visible="false" OnClick="btnDeleteLanguage_Click" Text="<%$Resources:Resource, DeleteLanguageButton %>" runat="server" CausesValidation="false" />
            </portal:HeadingPanel>
            <portal:NotifyMessage ID="message" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="workplace">
        <asp:Panel ID="pnlRecruitment" runat="server" DefaultButton="btnUpdate">
            <div id="divtabs" class="tabs">
                <ul class="nav nav-tabs">
                    <li role="presentation"><a aria-controls="tabContent" class="active" role="tab" data-toggle="tab" href="#tabContent">
                        <asp:Literal ID="litContentTab" runat="server" /></a></li>
                    <li role="presentation" id="liImages" runat="server">
                        <asp:Literal ID="litImagesTab" runat="server" /></li>
                     <li role="presentation" id="liAttribute" runat="server">
                        <asp:Literal ID="litAttributeTab" runat="server" /></li>
                    <li role="presentation"><a aria-controls="tabMeta" role="tab" data-toggle="tab" href="#tabMeta">
                        <asp:Literal ID="litMetaTab" runat="server" /></a></li>
                </ul>
                <div class="tab-content">
                    <div role="tabpanel" class="tab-pane fade active in" id="tabContent">
                        <div class="form-horizontal">
                            <div class="settingrow form-group">
                                <gb:SiteLabel ID="lblZones" runat="server" ConfigKey="ZoneLabel"
                                    ForControl="ddZones" CssClass="settinglabel control-label col-sm-3" />
                                <div class="col-sm-9">
                                    <div class="input-group">
                                        <asp:DropDownList ID="ddZones" runat="server" />
                                        <portal:gbHelpLink ID="GbHelpLink9" runat="server" HelpKey="recruitmentedit-selectzone-help" />
                                    </div>
                                </div>
                            </div>
                            <div class="settingrow form-group">
                                <gb:SiteLabel ID="SiteLabel1" runat="server" ConfigKey="RecruitmentDepartmentLabel" ResourceFile="RecruitmentResources"
                                    ForControl="ddZones" CssClass="settinglabel control-label col-sm-3" />
                                <div class="col-sm-9">
                                    <div class="input-group">
                                        <asp:DropDownList ID="ddlDepartment" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div id="divRecruitmentCode" runat="server" class="settingrow form-group recruitment-code">
                                <gb:SiteLabel ID="lblRecruitmentCode" runat="server" ConfigKey="RecruitmentCodeLabel" ResourceFile="RecruitmentResources"
                                    ForControl="txtRecruitmentCode" CssClass="settinglabel control-label col-sm-3" />
                                <div class="col-sm-9">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtRecruitmentCode" MaxLength="50" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <div class="settingrow form-group">
                                        <gb:SiteLabel ID="SiteLabel2" runat="server" ConfigKey="ProvinceLabel" ResourceFile="RecruitmentResources"
                                            ForControl="ddZones" CssClass="settinglabel control-label col-sm-3" />
                                        <div class="col-sm-9">
                                            <div class="input-group">
                                                <asp:DropDownList ID="ddlProvince" runat="server" AutoPostBack="true" DataValueField="Guid" DataTextField="Name" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlProvince_SelectedIndexChanged" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="settingrow form-group">
                                        <gb:SiteLabel ID="SiteLabel3" runat="server" ConfigKey="DisrtictLabel" ResourceFile="RecruitmentResources"
                                            ForControl="ddZones" CssClass="settinglabel control-label col-sm-3" />
                                        <div class="col-sm-9">
                                            <div class="input-group">
                                                <asp:DropDownList ID="ddlDistrict" runat="server" DataValueField="Guid" DataTextField="Name" AppendDataBoundItems="true" />
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div  runat="server" class="settingrow form-group">
                                <gb:SiteLabel ID="lblStockQuantity" runat="server" ConfigKey="RecruitmentQuantityLabel" ResourceFile="RecruitmentResources"
                                    ForControl="txtStockQuantity" CssClass="settinglabel control-label col-sm-3" />
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtQuantity" SkinID="NumericTextBox" MaxLength="10" runat="server" />
                                </div>
                            </div>
                            <div  runat="server" class="settingrow form-group">
                                <gb:SiteLabel ID="SiteLabel4" runat="server" ConfigKey="RecruitmentAppliedCountLabel" ResourceFile="RecruitmentResources"
                                    ForControl="txtStockQuantity" CssClass="settinglabel control-label col-sm-3" />
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtAppliedCount" SkinID="NumericTextBox" MaxLength="10" runat="server" />
                                </div>
                            </div>
                            <asp:UpdatePanel ID="up" runat="server">
                                <ContentTemplate>
                                    <telerik:RadTabStrip ID="tabLanguage" OnTabClick="tabLanguage_TabClick"
                                        EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false"
                                        CssClass="subtabs" SkinID="SubTabs" Visible="false" SelectedIndex="0" runat="server" />
                                    <div class="settingrow form-group">
                                        <gb:SiteLabel ID="lblTitle" runat="server" ForControl="txtTitle" CssClass="settinglabel control-label col-sm-3"
                                            ConfigKey="RecruitmentNameLabel" ResourceFile="RecruitmentResources" />
                                        <div class="col-sm-9">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtTitle" runat="server" MaxLength="255" CssClass="forminput verywidetextbox" />
                                                <portal:gbHelpLink ID="GbHelpLink1" runat="server" HelpKey="recruitmentedit-title-help" />
                                            </div>
                                            <asp:RequiredFieldValidator ID="reqTitle" runat="server" ControlToValidate="txtTitle"
                                                Display="Dynamic" SetFocusOnError="true" CssClass="txterror" ValidationGroup="recruitment" />
                                        </div>
                                    </div>
                                    <div id="divSubTitle" runat="server" class="settingrow form-group recruitment-subtitle">
                                        <gb:SiteLabel ID="lblSubTitle" runat="server" ForControl="txtSubTitle" CssClass="settinglabel control-label col-sm-3"
                                            ConfigKey="SubTitle" ResourceFile="RecruitmentResources" />
                                        <div class="col-sm-9">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtSubTitle" runat="server" MaxLength="255" CssClass="forminput verywidetextbox" />
                                                <portal:gbHelpLink ID="GbHelpLink2" runat="server" HelpKey="recruitmentedit-subtitle-help" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="settingrow form-group">
                                        <gb:SiteLabel ID="lblUrl" runat="server" ForControl="txtUrl" CssClass="settinglabel control-label col-sm-3"
                                            ConfigKey="RecruitmentEditItemUrlLabel" ResourceFile="RecruitmentResources" />
                                        <div class="col-sm-9">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtUrl" runat="server" MaxLength="255" CssClass="forminput verywidetextbox" />
                                                <portal:gbHelpLink ID="GbHelpLink3" runat="server" HelpKey="recruitmentedit-url-help" />
                                            </div>
                                            <span id="spnUrlWarning" runat="server" style="font-weight: normal; display: none;" class="txterror"></span>
                                            <asp:HiddenField ID="hdnTitle" runat="server" />
                                            <asp:RegularExpressionValidator ID="regexUrl" runat="server" ControlToValidate="txtUrl"
                                                ValidationExpression="((http\://|https\://|~/){1}(\S+){0,1})" Display="Dynamic" SetFocusOnError="true"
                                                ValidationGroup="recruitment" />
                                        </div>
                                    </div>
                                    <div class="settingrow form-group">
                                        <gb:SiteLabel ID="lblBriefContent" runat="server" CssClass="settinglabel control-label col-sm-3" ConfigKey="RecruitmentEditBriefContentLabel"
                                            ResourceFile="RecruitmentResources" />
                                        <div class="col-sm-9">
                                            <div class="input-group">
                                                <gbe:EditorControl ID="edBriefContent" runat="server" />
                                                <portal:gbHelpLink ID="GbHelpLink4" runat="server" HelpKey="recruitmentedit-briefcontent-help" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="settingrow form-group">
                                        <gb:SiteLabel ID="lblFullContent" runat="server" CssClass="settinglabel control-label col-sm-3" ConfigKey="RecruitmentEditContentLabel"
                                            ResourceFile="RecruitmentResources" />
                                        <div class="col-sm-9">
                                            <div class="input-group">
                                                <gbe:EditorControl ID="edFullContent" runat="server" />
                                                <portal:gbHelpLink ID="GbHelpLink5" runat="server" HelpKey="recruitmentedit-fullcontent-help" />
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div id="divFileAttachment" runat="server" class="settingrow form-group recruitmentedit-attachfile">
                                <gb:SiteLabel ID="FileAttachmentLabel" runat="server" ForControl="txtFileAttachment"
                                    ResourceFile="RecruitmentResources" ConfigKey="FileAttachmentLabel" CssClass="settinglabel control-label col-sm-3" />
                                <div class="col-sm-9">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtFileAttachment" MaxLength="255" runat="server" />
                                        <div class="input-group-addon">
                                            <portal:FileBrowserTextBoxExtender ID="FileAttachmentBrowser" runat="server" BrowserType="file" />
                                        </div>
                                        <portal:gbHelpLink ID="GbHelpLink7" runat="server" HelpKey="recruitmentedit-fileattachment-help" />
                                    </div>
                                </div>
                            </div>
                            <div class="settingrow form-group" runat="server" id="divUploadImage">
                                <gb:SiteLabel ID="lblImage" runat="server" ConfigKey="ImageFile" ResourceFile="RecruitmentResources" CssClass="settinglabel control-label col-sm-3" />
                                <div class="col-sm-9">
                                    <telerik:RadAsyncUpload ID="fileImage" SkinID="radAsyncUploadSkin" MultipleFileSelection="Automatic"
                                        AllowedFileExtensions="jpg,jpeg,gif,png" runat="server" />
                                </div>
                            </div>
                            <div id="divPosition" visible="false" runat="server" class="settingrow form-group">
                                <gb:SiteLabel ID="lblPosition" runat="server" ForControl="chlPosition" ConfigKey="RecruitmentPositionLabel"
                                    ResourceFile="RecruitmentResources" CssClass="settinglabel control-label col-sm-3" />
                                <div class="col-sm-9">
                                    <asp:CheckBoxList ID="chlPosition" SkinID="Enum" DataValueField="Value" DataTextField="Name" runat="server" />
                                </div>
                            </div>
                            <div id="divShowOption" visible="false" runat="server" class="settingrow form-group">
                                <gb:SiteLabel ID="lblShowOption" runat="server" ForControl="chlShowOption" ConfigKey="ShowOptionLabel"
                                    ResourceFile="RecruitmentResources" CssClass="settinglabel control-label col-sm-3" />
                                <div class="col-sm-9">
                                    <asp:CheckBoxList ID="chlShowOption" SkinID="Enum" runat="server" />
                                </div>
                            </div>
                            <div id="divIsPublished" class="settingrow form-group" runat="server">
                                <gb:SiteLabel ID="lblIsPublished" runat="server" ForControl="chkIsPublished" ConfigKey="IsPublishedLabel"
                                    ResourceFile="RecruitmentResources" CssClass="settinglabel control-label col-sm-3" />
                                <div class="col-sm-9">
                                    <asp:CheckBox ID="chkIsPublished" runat="server" Checked="true" />
                                </div>
                            </div>
                            <div class="settingrow form-group">
                                <gb:SiteLabel ID="lblStartDate" runat="server" ConfigKey="RecruitmentEditStartDateLabel"
                                    ResourceFile="RecruitmentResources" CssClass="settinglabel control-label col-sm-3" />
                                <div class="col-sm-9">
                                    <gb:DatePickerControl ID="dpBeginDate" runat="server" ShowTime="True" SkinID="recruitment"
                                        CssClass="forminput"></gb:DatePickerControl>
                                    <portal:gbHelpLink ID="GbHelpLink10" runat="server" RenderWrapper="false" HelpKey="recruitmentedit-startdate-help" />
                                    <asp:RequiredFieldValidator ID="reqStartDate" runat="server" ControlToValidate="dpBeginDate"
                                        Display="Dynamic" SetFocusOnError="true" CssClass="txterror" ValidationGroup="recruitment" />
                                </div>
                            </div>
                            <div  runat="server"  class="settingrow form-group">
                                <gb:SiteLabel ID="lblEndDate" runat="server" ConfigKey="RecruitmentEditEndDateLabel" ResourceFile="RecruitmentResources"
                                    CssClass="settinglabel control-label col-sm-3" />
                                <div class="col-sm-9">
                                    <gb:DatePickerControl ID="dpEndDate" runat="server" ShowTime="True" SkinID="recruitment"
                                        CssClass="forminput"></gb:DatePickerControl>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div role="tabpanel" class="tab-pane fade" id="tabImages" runat="server">
                        <asp:UpdatePanel ID="updImages" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="form-horizontal">
                                    <div class="settingrow form-group">
                                        <gb:SiteLabel ID="lblRecruitmentImages" runat="server" ConfigKey="RecruitmentImagesLabel" ResourceFile="RecruitmentResources"
                                            CssClass="settinglabel control-label col-sm-3" />
                                        <div class="col-sm-9">
                                            <telerik:RadAsyncUpload ID="uplImageFile" SkinID="radAsyncUploadSkin" MultipleFileSelection="Automatic" HideFileInput="true"
                                                AllowedFileExtensions="jpg,jpeg,gif,png" Localization-Select="<%$Resources:RecruitmentResources, SelectFromComputerLabel %>" runat="server" />
                                        </div>
                                    </div>
                                    <div id="divRecruitmentVideos" runat="server" class="settingrow form-group">
                                        <gb:SiteLabel ID="lblVideoPath" runat="server" ConfigKey="RecruitmentVideoUrlLabel" ResourceFile="RecruitmentResources"
                                            CssClass="settinglabel control-label col-sm-3" />
                                        <div class="col-sm-9">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtVideoPath" MaxLength="255" runat="server" />
                                                <div class="input-group-addon">
                                                    <portal:FileBrowserTextBoxExtender ID="VideoFileBrowser" Text='<%$Resources:RecruitmentResources, BrowserOnServerLink %>' runat="server" BrowserType="video" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="settingrow form-group">
                                        <div class="col-sm-offset-3 col-sm-9">
                                            <asp:Button SkinID="DefaultButton" ID="btnUpdateImage" OnClick="btnUpdateImage_Click"
                                                Text="<%$Resources:RecruitmentResources, RecruitmentUpdateButton %>" runat="server" CausesValidation="False" />
                                            <asp:Button SkinID="DefaultButton" ID="btnDeleteImage" Visible="false" OnClick="btnDeleteImage_Click"
                                                Text="<%$Resources:RecruitmentResources, RecruitmentDeleteSelectedButton %>" runat="server"
                                                CausesValidation="False" />
                                            <portal:gbHelpLink ID="GbHelpLink11" runat="server" RenderWrapper="false" HelpKey="recruitmentedit-mediaupload-help" />
                                        </div>
                                    </div>
                                </div>
                                <telerik:RadGrid ID="grid" SkinID="radGridSkin" runat="server"
                                    OnNeedDataSource="grid_NeedDataSource" OnItemDataBound="grid_ItemDataBound">
                                    <MasterTableView DataKeyNames="Guid,LanguageId,DisplayOrder,Title,MediaType,MediaFile,ThumbnailFile">
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderStyle-Width="35" HeaderText="<%$Resources:Resource, RowNumber %>" AllowFiltering="false">
                                                <ItemTemplate>
                                                    <%# (grid.PageSize * grid.CurrentPageIndex) + Container.ItemIndex + 1%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="<%$Resources:RecruitmentResources, RecruitmentThumbnailLabel %>">
                                                <ItemTemplate>
                                                    <portal:MediaElement ID="ml1" runat="server" Width="100" FileUrl='<%# CanhCam.Web.RecruitmentUI.RecruitmentHelper.GetMediaFilePath(thumbnailImageFolderPath, Eval("ThumbnailFile").ToString()) %>' />
                                                    <telerik:RadAsyncUpload ID="fupThumbnail" SkinID="radAsyncUploadSkin" Width="100%" HideFileInput="true" MultipleFileSelection="Disabled"
                                                        AllowedFileExtensions="jpg,jpeg,gif,png" Localization-Select="<%$Resources:RecruitmentResources, SelectFromComputerLabel %>" runat="server" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="<%$Resources:RecruitmentResources, RecruitmentImagesLabel %>" UniqueName="MediaFile">
                                                <ItemTemplate>
                                                    <portal:MediaElement ID="ml2" runat="server" Width="100" IncludeDownloadLinkForMedia="false" FileUrl='<%# CanhCam.Web.RecruitmentUI.RecruitmentHelper.GetMediaFilePath(imageFolderPath, Eval("MediaFile").ToString()) %>' />
                                                    <telerik:RadAsyncUpload ID="fupImageFile" SkinID="radAsyncUploadSkin" Width="100%" HideFileInput="true" MultipleFileSelection="Disabled"
                                                        AllowedFileExtensions="jpg,jpeg,gif,png" Localization-Select="<%$Resources:RecruitmentResources, SelectFromComputerLabel %>" runat="server" />
                                                    <asp:TextBox ID="txtVideoPath" MaxLength="255" Text='<%# Eval("MediaFile").ToString().Contains("/") ? Eval("MediaFile").ToString() : "" %>' runat="server" />
                                                    <portal:FileBrowserTextBoxExtender ID="VideoFileBrowser" Text='<%$Resources:VideoResources, FileBrowserLink %>' runat="server" BrowserType="video" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="<%$Resources:RecruitmentResources, RecruitmentImageTitleLabel %>">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtTitle" CssClass="input-grid" Width="97%" MaxLength="255" Text='<%# Eval("Title") %>'
                                                        runat="server" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn HeaderStyle-Width="150" HeaderText="<%$Resources:RecruitmentResources, RecruitmentEditLanguageLabel %>" UniqueName="LanguageID">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlLanguage" AppendDataBoundItems="true" DataValueField="LanguageID" DataTextField="Name" runat="server" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderStyle-Width="100" HeaderText="<%$Resources:RecruitmentResources, RecruitmentEditDisplayOrderLabel %>">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDisplayOrder" SkinID="NumericTextBox" MaxLength="4" Text='<%# Eval("DisplayOrder") %>'
                                                        runat="server" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridClientSelectColumn HeaderStyle-Width="35" />
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="tabAttribute" role="tabpanel" class="tab-pane fade" runat="server">
                        <asp:UpdatePanel ID="updAttribute" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-3">
                                        <div class="input-group">
                                            <asp:ListBox ID="lbAttribute" Width="100%" Height="500" AutoPostBack="true" runat="server"
                                                AppendDataBoundItems="true" DataTextField="Title" DataValueField="Guid" />
                                            <div class="input-group-addon">
                                                <ul class="nav sorter">
                                                    <li>
                                                        <asp:LinkButton ID="btnAttributeUp" CssClass="btn btn-default" CommandName="up" runat="server"><i class="fa fa-angle-up"></i></asp:LinkButton></li>
                                                    <li>
                                                        <asp:LinkButton ID="btnAttributeDown" CssClass="btn btn-default" CommandName="down" runat="server"><i class="fa fa-angle-down"></i></asp:LinkButton></li>
                                                    <li>
                                                        <asp:LinkButton ID="btnDeleteAttribute" CssClass="btn btn-default" runat="server" CausesValidation="False"><i class="fa fa-trash-o"></i></asp:LinkButton></li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-9">
                                        <telerik:RadTabStrip ID="tabAttributeLanguage" OnTabClick="tabAttributeLanguage_TabClick"
                                            EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false"
                                            CssClass="subtabs" SkinID="SubTabs" Visible="false" SelectedIndex="0" runat="server" />
                                        <div class="form-horizontal">
                                            <div class="settingrow form-group">
                                                <gb:SiteLabel ID="lblAttributeTitle" runat="server" CssClass="settinglabel control-label col-sm-3" ForControl="txtAttributeTitle" ConfigKey="AttributeTitle"
                                                    ResourceFile="RecruitmentResources" />
                                                <div class="col-sm-9">
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtAttributeTitle" runat="server" MaxLength="255" />
                                                        <asp:RequiredFieldValidator ID="reqAttributeTitle" ControlToValidate="txtAttributeTitle" ErrorMessage="<%$Resources:RecruitmentResources, AttributeTitleRequired %>"
                                                        ValidationGroup="Attribute" Display="Dynamic" CssClass="txterror" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="settingrow form-group">
                                                <div class="col-sm-offset-3 col-sm-9">
                                                    <asp:Button SkinID="DefaultButton" ID="btnAttributeUpdate" ValidationGroup="Attribute" Text="<%$Resources:RecruitmentResources, AttributeUpdateButton %>" runat="server" />
                                                    <asp:Button SkinID="DefaultButton" ID="btnDeleteAttributeLanguage" Visible="false" OnClick="btnDeleteAttributeLanguage_Click" Text="<%$Resources:Resource, DeleteLanguageButton %>" runat="server" CausesValidation="false" />
                                                </div>
                                            </div>
                                            <div class="settingrow form-group">
                                                <gb:SiteLabel ID="lblAttributeContent" runat="server" CssClass="settinglabel control-label" ConfigKey="AttributeContent"
                                                    ResourceFile="RecruitmentResources" />
                                                <div>
                                                    <gbe:EditorControl ID="edAttributeContent" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div role="tabpanel" class="tab-pane fade" id="tabMeta">
                        <asp:UpdatePanel ID="upSEO" runat="server">
                            <ContentTemplate>
                                <telerik:RadTabStrip ID="tabLanguageSEO" OnTabClick="tabLanguageSEO_TabClick"
                                    EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false"
                                    CssClass="subtabs" SkinID="SubTabs" Visible="false" SelectedIndex="0" runat="server" />
                                <div class="form-horizontal">
                                    <div class="settingrow form-group">
                                        <gb:SiteLabel ID="lblMetaTitle" runat="server" ForControl="txtMetaTitle" CssClass="settinglabel control-label col-sm-3"
                                            ConfigKey="MetaTitleLabel" />
                                        <div class="col-sm-9">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtMetaTitle" runat="server" MaxLength="65" />
                                                <portal:gbHelpLink ID="GbHelpLink6" runat="server" HelpKey="recruitmentedit-metatitle-help" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="settingrow form-group">
                                        <gb:SiteLabel ID="lblMetaDescription" runat="server" ForControl="txtMetaDescription" CssClass="settinglabel control-label col-sm-3"
                                            ConfigKey="MetaDescriptionLabel" ResourceFile="RecruitmentResources" />
                                        <div class="col-sm-9">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtMetaDescription" runat="server" MaxLength="156" />
                                                <portal:gbHelpLink ID="gbHelpLink22" runat="server" HelpKey="recruitmentedit-metakeywords-help" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="settingrow form-group">
                                        <gb:SiteLabel ID="lblMetaKeywords" runat="server" ForControl="txtMetaKeywords" CssClass="settinglabel control-label col-sm-3"
                                            ConfigKey="MetaKeywordsLabel" ResourceFile="RecruitmentResources" />
                                        <div class="col-sm-9">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtMetaKeywords" runat="server" MaxLength="156" />
                                                <portal:gbHelpLink ID="gbHelpLink23" runat="server" HelpKey="recruitmentedit-metadescription-help" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="settingrow form-group">
                                        <gb:SiteLabel ID="lblAdditionalMetaTags" ForControl="txtAdditionalMetaTags" CssClass="settinglabel control-label col-sm-3" runat="server" ConfigKey="MetaAdditionalLabel" />
                                        <div class="col-sm-9">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtAdditionalMetaTags" CssClass="form-control" TextMode="MultiLine" runat="server" />
                                                <portal:gbHelpLink ID="gbHelpLink25" runat="server" HelpKey="recruitmentedit-additionalmeta-help" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
        </asp:Panel>
    </div>
    <portal:SessionKeepAliveControl ID="ka1" runat="server" />
</div>
<asp:Panel CssClass="modal fade" ID="pnlModal" Visible="false" runat="server" TabIndex="-1" role="dialog" aria-labelledby="pnlModalLabel">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title" id="pnlModalLabel">
                    <gb:SiteLabel ID="lblCopyRecruitment" runat="server" UseLabelTag="false" ConfigKey="CopyRecruitmentLabel" ResourceFile="RecruitmentResources" />
                </h4>
            </div>
            <div class="modal-body">
                <div class="form-horizontal">
                    <div class="settingrow form-group">
                        <gb:SiteLabel ID="lblCopyRecruitmentTitle" runat="server" ConfigKey="CopyRecruitmentTitleLabel" ResourceFile="RecruitmentResources"
                            ForControl="txtCopyTitleName" CssClass="settinglabel control-label col-sm-3" />
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtCopyRecruitmentName" runat="server" MaxLength="255" />
                            <asp:RequiredFieldValidator ID="reqCopyRecruitmentTitle" runat="server" ControlToValidate="txtCopyRecruitmentName"
                                Display="Dynamic" SetFocusOnError="true" CssClass="txterror" ValidationGroup="recruitmentCopy" />
                        </div>
                    </div>
                    <div id="divCopyRecruitmentPublished" runat="server" class="settingrow form-group">
                        <gb:SiteLabel ID="lblCopyRecruitmentPublished" runat="server" ConfigKey="CopyRecruitmentPublishedLabel" ResourceFile="RecruitmentResources"
                            ForControl="chkCopyRecruitmentPublished" CssClass="settinglabel control-label col-sm-3" />
                        <div class="col-sm-9">
                            <asp:CheckBox ID="chkCopyRecruitmentPublished" runat="server" Checked="true" />
                        </div>
                    </div>
                    <div class="settingrow form-group">
                        <gb:SiteLabel ID="lblCopyRecruitmentCopyImages" runat="server" ConfigKey="CopyRecruitmentCopyImagesLabel" ResourceFile="RecruitmentResources"
                            ForControl="chkCopyRecruitmentCopyImages" CssClass="settinglabel control-label col-sm-3" />
                        <div class="col-sm-9">
                            <asp:CheckBox ID="chkCopyRecruitmentCopyImages" runat="server" Checked="true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <asp:Button ID="btnCopyRecruitment" CssClass="btn btn-primary" Text="Copy" ValidationGroup="recruitmentCopy" runat="server" />
            </div>
        </div>
    </div>
</asp:Panel>
