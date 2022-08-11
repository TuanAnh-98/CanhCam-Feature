﻿<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="Default.aspx.cs" Inherits="CanhCam.Web.AdminUI.DesignerToolsPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:BreadcrumbAdmin ID="breadcrumb" runat="server"
        CurrentPageTitle="<%$Resources:DevTools, DesignTools %>" CurrentPageUrl="~/DesignTools/Default.aspx"
        ParentTitle="<%$Resources:Resource, AdvancedToolsLink %>" ParentUrl="~/AdminCP/AdvancedTools.aspx" />
    <div class="admin-content col-md-12">
        <portal:HeadingPanel ID="heading" runat="server"></portal:HeadingPanel>
        <div class="workplace">
            <div class="metromini">
                <div class="row">
                    <div class="col-sm-12">
                        <div class="item">
                            <asp:Literal ID="litSkinList" runat="server" />
                        </div>
                        <div class="item">
                            <asp:Literal ID="litCacheTool" runat="server" />
                        </div>
                        <div id="divLessUtility" runat="server" class="item">
                            <asp:Literal ID="litLessUtility" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
