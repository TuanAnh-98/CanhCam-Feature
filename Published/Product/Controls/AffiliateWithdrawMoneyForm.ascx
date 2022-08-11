<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="AffiliateWithdrawMoneyForm.ascx.cs" Inherits="CanhCam.Web.ProductUI.AffiliateWithdrawMoneyForm" %>


<div class="wave"></div>
<div id="form">

    <div class="fish" id="fish"></div>
    <div class="fish" id="fish2"></div>
    <div>
        <h1>THÔNG TIN TÀI KHOẢN</h1>
    </div>
    <br />
    <br />
    <div class="formgroup row">
        <gb:SiteLabel ID="Sitelabel2" runat="server" class="label" Text="Địa chỉ Email" />
        <asp:TextBox runat="server" class="textbox" ID="txtEmail">
        </asp:TextBox>
    </div>

    <div class="formgroup row">
        <gb:SiteLabel ID="Sitelabel1" runat="server" class="label" Text="Họ Tên*" />
        <asp:TextBox runat="server" class="textbox" ID="txtName"></asp:TextBox>
    </div>

    <div class="formgroup row">
        <gb:SiteLabel ID="Sitelabel3" runat="server" class="label" Text="Giới tính*" />
        <asp:TextBox runat="server" class="textbox" ID="txtSex"></asp:TextBox>
    </div>

    <div class="formgroup row">
        <gb:SiteLabel ID="Sitelabel4" runat="server" class="label" Text="Ngày sinh*" />
        <asp:TextBox runat="server" class="textbox" ID="txtDateBirth"></asp:TextBox>
    </div>
    <div class="formgroup row">
        <gb:SiteLabel ID="Sitelabel5" runat="server" class="label" Text="Địa chỉ*" />
        <asp:TextBox runat="server" class="textbox" ID="txtAddress"></asp:TextBox>
    </div>
    <div class="formgroup row">
        <gb:SiteLabel ID="Sitelabel6" runat="server" class="label" Text="Tỉnh/Thành Phố" />
        <asp:TextBox runat="server" class="textbox" ID="txtCity"></asp:TextBox>
    </div>
    <div class="formgroup row">
        <gb:SiteLabel ID="Sitelabel7" runat="server" class="label" Text="Công ty" />
        <asp:TextBox runat="server" class="textbox" ID="txtCompany"></asp:TextBox>
    </div>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <div>
        <h1>THÔNG TIN THANH TOÁN</h1>
    </div>
    <div class="formgroup row">
        <gb:SiteLabel ID="Sitelabel8" runat="server" class="label" Text="Tên người nhận thanh toán" />
        <asp:TextBox runat="server" class="textbox" ID="txtBankUserName"></asp:TextBox>
    </div>
    <div class="formgroup row">
        <gb:SiteLabel ID="Sitelabel9" runat="server" class="label" Text="Số tài khoản" />
        <asp:TextBox runat="server" class="textbox" ID="txtBankNumber"></asp:TextBox>
    </div>
    <div class="formgroup row">
        <gb:SiteLabel ID="Sitelabel10" runat="server" class="label" Text="Tên ngân hàng" />
        <asp:TextBox runat="server" class="textbox" ID="txtBankName"></asp:TextBox>
    </div>
    <div class="formgroup row">
        <gb:SiteLabel ID="Sitelabel11" runat="server" class="label" Text="Chi nhánh" />
        <asp:TextBox runat="server" class="textbox" ID="txtBankBranch"></asp:TextBox>
    </div>
    <div class="formgroup row">
        <gb:SiteLabel ID="Sitelabel12" runat="server" class="label" Text="Nhập số tiền muốn rút" />
        <asp:TextBox runat="server" class="textbox" ID="txtWithdrawMoney"></asp:TextBox>
    </div>

    <portal:NotifyMessage ID="message" runat="server" />
    <asp:Button runat="server" class="btnSubmit" ID="btnSubmit" Text="Điện Thoại" />

</div>

<style>
    #form {
        padding-left: 200px;
        background-color: #98D5F4;
        background-color: #98d4f3;
        overflow: hidden;
        position: relative;
    }

    .wave {
        background-image: url('http://www.geertjanhendriks.nl/codepen/form/golf.png');
        height: 160px;
        background-repeat: repeat-x;
        background-position: bottom;
    }
    /* Animation webkit */
    @-webkit-keyframes myfirst {
        0% {
            margin-left: -235px
        }

        90% {
            margin-left: 100%;
        }

        100% {
            margin-left: 100%;
        }
    }

    /* Animation */
    @keyframes myfirst {
        0% {
            margin-left: -235px
        }

        70% {
            margin-left: 100%;
        }

        100% {
            margin-left: 100%;
        }
    }

    .fish {
        background-image: url('http://www.geertjanhendriks.nl/codepen/form/fish.png');
        width: 235px;
        height: 104px;
        margin-left: -235px;
        position: absolute;
        animation: myfirst 24s;
        -webkit-animation: myfirst 24s;
        animation-iteration-count: infinite;
        -webkit-animation-iteration-count: infinite;
        animation-timing-function: linear;
        -webkit-animation-timing-function: linear;
    }

    #fish {
        top: 120px;
    }

    #fish2 {
        top: 260px;
        animation-delay: 12s;
        -webkit-animation-delay: 12s;
    }

    .formgroup {
        font-size: 25px;
    }

    h1 {
        text-align: center;
        font-size: 30px;
        font-weight: bold;
    }


    .textbox, .btnSubmit {
        width: 500px;
        margin: -20px 0  0 250px;
        padding-top: 20px;
        border: none;
        border-radius: 20px;
        outline: none;
        padding: 10px;
        font-family: 'Sniglet', cursive;
        font-size: 1em;
        color: #676767;
        transition: border 0.5s;
        -webkit-transition: border 0.5s;
        -moz-transition: border 0.5s;
        -o-transition: border 0.5s;
        border: solid 3px #98d4f3;
        -webkit-box-sizing: border-box;
        -moz-box-sizing: border-box;
        box-sizing: border-box;
    }

    .btnSubmit {
        margin-top: 50px;
        line-height: 40px;
        background-color: #e4af43;
    }

    .label {
        padding: 0 0 0 0;   
        
        
    }

    .formgroup-active {
        background-image: url('http://www.geertjanhendriks.nl/codepen/form/octo.png');
    }

    .formgroup-error {
        background-image: url('http://www.geertjanhendriks.nl/codepen/form/octo-error.png');
        color: red;
    }
</style>
