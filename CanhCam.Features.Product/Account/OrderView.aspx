<%@ Page Language="c#" MaintainScrollPositionOnPostback="true" CodeBehind="OrderView.aspx.cs"
    MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="CanhCam.Web.AccountUI.OrderViewPage" %>

<%@ Register TagPrefix="Site" TagName="AccountMenu" Src="~/Account/AccountMenu.ascx" %>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">

    <div class="container user-sidemenu">
        <section class="row">
            <Site:AccountMenu ID="accountMenu" runat="server" />
            <div class="col-lg-9">
                <div class="user-page">
                    <h1 class="postname">Chi tiết đơn hàng
                    </h1>
                    <asp:Xml ID="xmlTransformer" runat="server"></asp:Xml>
                </div>
            </div>
        </section>
    </div>
</asp:Content>