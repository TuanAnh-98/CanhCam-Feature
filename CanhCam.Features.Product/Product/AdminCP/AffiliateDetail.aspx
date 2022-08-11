<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="AffiliateDetail.aspx.cs" Inherits="CanhCam.Web.ProductUI.AffiliateDetail" %>


<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:BreadcrumbAdmin ID="breadcrumb" runat="server" CurrentPageTitle="Nhan Vien Edit" CurrentPageUrl="~/Product/AdminCP/PaymentMethods.aspx" />
    <div class="admin-content col-md-12">
        <portal:HeadingPanel ID="heading" runat="server">
            <div>
                <asp:Button runat="server" ID="btnSubmit" Text="Save" SkinID="UpdateButton" />
            </div>
        </portal:HeadingPanel>
        <portal:NotifyMessage ID="message" runat="server" />
        <div class="workplace">
            <div>
                <div class="settingrow form-group">
                    <gb:SiteLabel ID="lblBillingEmail" runat="server" CssClass="settinglabel control-label col-sm-3" 
                        Text="Tổng số dư hiện tại: " />
                    <div class="col-sm-9" style="color: green; font-size: 30px;">
                        <gb:SiteLabel ID="slbTotalCommission" runat="server" />
                    </div>
                </div>
                <label style="color: red; font-style: italic;">Số tiền tối thiểu có thể rút là 200.000 đ</label>
                <div class="settingrow form-group">
                    <gb:SiteLabel ID="Sitelabel1" runat="server" CssClass="settinglabel control-label col-sm-3" 
                        Text="Tổng số tiền đã thanh toán: " />
                    <div class="col-sm-9">
                        <gb:SiteLabel ID="slbCommissionPay" runat="server" />
                    </div>
                </div>
                <div class="settingrow form-group">
                    <gb:SiteLabel ID="lblStartDate" runat="server"
                        Text="Tổng số tiền chờ thanh toán: " CssClass="settinglabel control-label col-sm-3" />
                    <div class="col-sm-9">
                        <gb:SiteLabel ID="slbCommissionWait" runat="server" />
                    </div>
                </div>
                <div class="settingrow form-group">
                    <gb:SiteLabel ID="Sitelabel3" runat="server" CssClass="settinglabel control-label col-sm-3" 
                        Text="Tổng số đơn hàng:  " />
                    <div class="col-sm-9">
                        <gb:SiteLabel ID="slbTotalOrder" runat="server" />
                    </div>
                </div>
                <asp:Label ID="Label1" CssClass="text-success" runat="server"></asp:Label>
            </div>
            <br />
            <br />
            <div>
                <asp:HyperLink runat="server" ID="hlnkPayment"  Text="Rút Tiền" CssClass="btnPaymentCss" />
            </div>
            
        </div>
        
    </div>
    <style>
        .btnPaymentCss{
            border-radius: 3px 4px;
            width: 200px;
            height: 50px;
            background-color: red;
            border: 1px solid black;
            color: white;
        }

        #slbTotalCommission {
            color: green;
            font-size: 100px;
        }
    </style>
</asp:Content>
