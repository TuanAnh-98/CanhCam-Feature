<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="PurchaseHistory.aspx.cs" Inherits="CanhCam.Web.ProductUI.PurchaseHistoryPage" %>

<%@ Register TagPrefix="Site" TagName="AccountMenu" Src="~/Account/AccountMenu.ascx" %>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <div class="container">
        <div class="row flex flex-wrap">
            <Site:AccountMenu ID="accountMenu" runat="server" />
            <div class="col-xs-12 col-lg-9">
                <section class="user-page clearfix">
                    <h1 class="postname">
                        <asp:Literal ID="litTitle" Text="Đơn hàng của tôi" runat="server" />
                    </h1>
                    <div class="row flex flex-wrap">
                        <div class="col-xs-12">
                            <section class="cart-history clearfix">
                                <asp:Xml ID="xmlTransformer" runat="server"></asp:Xml>
                                <div id="divPager" runat="server" class="pages text-xs-center">
                                    <portal:gbCutePager ID="pgr" runat="server" />
                                </div>
                            </section>
                        </div>
                    </div>
                </section>
            </div>
        </div>
    </div>
</asp:Content>