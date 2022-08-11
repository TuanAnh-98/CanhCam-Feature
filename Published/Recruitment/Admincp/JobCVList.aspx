<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="JobCVList.aspx.cs" Inherits="CanhCam.Web.RecruitmentUI.JobCVList" %>

<%@ Register TagPrefix="Site" Assembly="CanhCam.Features.Recruitment" Namespace="CanhCam.Web.RecruitmentUI" %>
<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MainContent" runat="server">
    <portal:breadcrumbadmin id="breadcrumb" runat="server"
        currentpagetitle="<%$Resources:RecruitmentResources, JobViewCVTitle %>" currentpageurl="~/Recruitment/AdminCP/JobCVList.aspx" />
    <div class="admin-content col-md-12">
        <portal:headingpanel id="heading" runat="server">
            <asp:HyperLink SkinID="DefaultButton" ID="lnkRefresh" CssClass="cp-link" runat="server" />
            <asp:Button SkinID="DefaultButton" ID="btnExport" runat="server" />
            <asp:Button SkinID="DeleteButton" ID="btnDelete" Text="<%$Resources:Resource, DeleteSelectedButton %>"
                runat="server" CausesValidation="false" />
        </portal:headingpanel>
        <portal:notifymessage id="message" runat="server" />
        <div class="workplace">
            <telerik:RadGrid ID="grid" SkinID="radGridSkin" runat="server">
            <MasterTableView DataKeyNames="RecruitmentID,RecruitmentApplyId">
                <Columns>
                    <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Center" HeaderText="<%$Resources:Resource, RowNumber %>"
                        AllowFiltering="false">
                        <ItemTemplate>
                            <%# (grid.PageSize * grid.CurrentPageIndex) + Container.ItemIndex + 1%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Left" HeaderText="<%$Resources:RecruitmentResources, JobPositionLabel %>">
                        <ItemTemplate>
                            <asp:HyperLink CssClass="cp-link" ID="hplTitle" runat="server" EnableViewState="false" Visible='<%# (bool) (DataBinder.Eval(Container.DataItem, "URL").ToString().Length != 0) %>'
                                Text='<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem,"Title").ToString()) %>' Target="_blank"
                                NavigateUrl='<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem,"URL").ToString())%>'>
                            </asp:HyperLink>
                            <asp:Literal ID="litTitle" Visible='<%# (bool) (DataBinder.Eval(Container.DataItem, "URL").ToString().Length == 0) %>' Text='<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem,"Title").ToString()) %>' runat="server" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Left" HeaderText="<%$Resources:RecruitmentResources, JobCandidateInfoLabel %>">
                        <ItemTemplate>
                            <div>
                                <b>
                                    <asp:Literal ID="litFullName" Text="<%$Resources:RecruitmentResources, JobFullNameLabel %>" EnableViewState="false" runat="server" />:
                                </b>&nbsp;
                                <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem,"Name").ToString()) %>
                            </div>
                            <div>
                                <b>
                                    <asp:Literal ID="litAddress" Text="<%$Resources:RecruitmentResources, JobAddressLabel %>" EnableViewState="false" runat="server" />:
                                </b>&nbsp;
                                <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem,"Address").ToString()) %>
                            </div>
                            <div>
                                <b>
                                    <asp:Literal ID="litEmail" Text="<%$Resources:RecruitmentResources, JobEmailLabel %>" EnableViewState="false" runat="server" />:
                                </b>&nbsp;
                                <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem,"Email").ToString()) %>
                            </div>
                            <div>
                                <b>
                                    <asp:Literal ID="litPhone" Text="<%$Resources:RecruitmentResources, JobPhoneLabel %>" EnableViewState="false" runat="server" />:
                                </b>&nbsp;
                                <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem,"Phone").ToString()) %>
                            </div>
                            <%# FormatDate(Convert.ToDateTime(Eval("CreatedDate"))) %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Left" HeaderText="<%$Resources:RecruitmentResources, JobMessageLabel %>">
                        <ItemTemplate>
                            <NeatHtml:UntrustedContent ID="UntrustedContent2" runat="server" EnableViewState="false"
                                TrustedImageUrlPattern='<%# RegexRelativeImageUrlPatern %>' ClientScriptUrl="~/ClientScript/NeatHtml.js">
                                <%# DataBinder.Eval(Container.DataItem, "Comment").ToString() %>
                            </NeatHtml:UntrustedContent>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Left" HeaderText="<%$Resources:RecruitmentResources, JobAttachFileLabel %>">
                        <ItemTemplate>
                            <%# GetAttachFile(Convert.ToInt32(Eval("RecruitmentID")), Eval("AttachFile1"), Eval("AttachFile2")) %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridClientSelectColumn ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%" UniqueName="ClientSelectColumn" />
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
        </div>
    </div>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />