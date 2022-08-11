<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="ZonePosition.aspx.cs" Inherits="CanhCam.Web.AdminUI.ZonePositionPage" %>

<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <style>
        @media (min-width: 768px) {
            .buttons{margin-top:50px}
            .left-grid{margin-top:50px}
        }
    </style>
    <portal:BreadcrumbAdmin ID="breadcrumb" runat="server" 
        ParentTitle="<%$Resources:Resource, ZoneStructureLink %>" ParentUrl="~/AdminCP/ZoneTree.aspx"
        CurrentPageTitle="<%$Resources:Resource, ZonePositionHeading %>" CurrentPageUrl="~/AdminCP/ZonePermission.aspx" />
    <div class="admin-content col-md-12">
        <portal:HeadingPanel ID="heading" runat="server"></portal:HeadingPanel>
        <portal:NotifyMessage ID="message" runat="server" />
        <asp:UpdatePanel ID="upPages" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <div class="workplace">
                    <div class="row">
                        <div class="col-sm-5 left-grid">
                            <asp:ListBox ID="lstZones" runat="server" Width="100%" Height="300" SelectionMode="Multiple" />
                        </div>
                        <div class="col-sm-1 text-center buttons">
                            <asp:LinkButton ID="btnRemove" runat="server" CssClass="btn btn-default"><i class="fa fa-arrow-left"></i></asp:LinkButton>
                            <asp:LinkButton ID="btnAdd" runat="server" CssClass="btn btn-default"><i class="fa fa-arrow-right"></i></asp:LinkButton>
                        </div>
                        <div class="col-sm-6">
                            <div class="form-horizontal">
                                <div class="settingrow form-group">
                                    <gb:SiteLabel ID="lblPosition" runat="server" ForControl="ddlPosition" CssClass="settinglabel control-label col-sm-3"
                                        ConfigKey="ZoneSettingsPositionLabel" />
                                    <div class="col-sm-9">
                                        <asp:DropDownList ID="ddlPosition" AutoPostBack="true" runat="server" DataTextField="Name" DataValueField="Value" />
                                    </div>
                                </div>
                            </div>
                            <asp:ListBox ID="lstSelectedZones" runat="server" Width="100%" Height="300" SelectionMode="Multiple" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>