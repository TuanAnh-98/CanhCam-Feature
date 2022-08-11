<%@ Page CodeBehind="AccessDenied.aspx.cs" Language="c#" MasterPageFile="~/App_MasterPages/layout.Master"
    AutoEventWireup="false" Inherits="CanhCam.Web.UI.Pages.AccessDeniedPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <div class="error-page">
        <h2 class="headline">401</h2>
        <div class="error-content">
          <p>
              <gb:SiteLabel ID="lblAccessDeniedLabel" runat="server" UseLabelTag="false" ConfigKey="AccessDeniedLabel" />
              <asp:Literal ID="lblAccessDenied" runat="server" />
          </p>
          <p>
              <asp:HyperLink ID="lnkHome" runat="server" />
          </p>
        </div>
    </div>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
