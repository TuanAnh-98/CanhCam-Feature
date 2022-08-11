<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="RecruitmentDepartmentList.aspx.cs" Inherits="CanhCam.Web.RecruitmentUI.RecruitmentDepartmentList" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:breadcrumbadmin id="breadcrumb" runat="server"
        currentpagetitle="<%$Resources:RecruitmentResources, RecruitmentDepartmentTitle %>" currentpageurl="~/Recruitment/AdminCP/RecruitmentDepartmentList.aspx" />
    <div class="admin-content col-md-12">
        <portal:headingpanel id="heading" runat="server">
            <asp:Button SkinID="UpdateButton" ID="btnUpdate" Text="<%$Resources:Resource, UpdateButton %>"
                runat="server" />
            <asp:HyperLink SkinID="InsertButton" ID="lnkInsert" Text="<%$Resources:Resource, InsertButton %>" runat="server" />
            <asp:Button SkinID="DeleteButton" ID="btnDelete" Text="<%$Resources:Resource, DeleteSelectedButton %>" runat="server" CausesValidation="false" />
        </portal:headingpanel>
        <portal:notifymessage id="message" runat="server" />
        <asp:Panel ID="pnlSearch" CssClass="headInfo form-horizontal" DefaultButton="btnSearch" runat="server">
            <div class="settingrow form-group">
                <gb:SiteLabel ID="lblTitle" runat="server" ConfigKey="DepartmentNameLabel"
                    ResourceFile="RecruitmentResources" ForControl="txtTitle" CssClass="settinglabel control-label col-sm-3" />
                <div class="col-sm-9">
                    <div class="input-group">
                        <asp:TextBox ID="txtTitle" runat="server" MaxLength="255" />
                        <div class="input-group-btn">
                            <asp:Button SkinID="DefaultButton" ID="btnSearch" Text="<%$Resources:RecruitmentResources, RecruitmentSearchButton %>"
                                runat="server" CausesValidation="false" />
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <div class="workplace">
            <telerik:radgrid id="grid" skinid="radGridSkin" runat="server">
                <MasterTableView DataKeyNames="DepartmentId,Name,Description" AllowPaging="false" AllowSorting="false">
                    <Columns>
					    <telerik:GridTemplateColumn HeaderStyle-Width="35"
						    HeaderText="<%$Resources:Resource, RowNumber %>">
						    <ItemTemplate>
							    <%# (grid.PageSize * grid.CurrentPageIndex) + Container.ItemIndex + 1%>
						    </ItemTemplate>
					    </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="<%$Resources:RecruitmentResources, DepartmentNameLabel %>">
                            <ItemTemplate>
                                <%# Eval("Name") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="<%$Resources:RecruitmentResources, DisplayOrderLabel %>"
                            AllowFiltering="false">
                            <ItemTemplate>
                                <asp:TextBox ID="txtDisplayOrder" SkinID="NumericTextBox"
                                    MaxLength="4" Text='<%# Eval("DisplayOrder") %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <ItemTemplate>
                                <asp:HyperLink CssClass="cp-link" ID="EditLink" runat="server"
                                    Text="<%$Resources:RecruitmentResources, EditLink %>" NavigateUrl='<%# SiteRoot + "/Recruitment/AdminCP/RecruitmentDepartmentEdit.aspx?ItemID=" + Eval("DepartmentId") %>'>
                                </asp:HyperLink>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridClientSelectColumn HeaderStyle-Width="35" />
                    </Columns>
                </MasterTableView>
            </telerik:radgrid>
        </div>
    </div>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane"
    runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit"
    runat="server">
</asp:Content>