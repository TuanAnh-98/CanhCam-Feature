<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="CustomerEdit.aspx.cs" Inherits="CanhCam.Web.AccountUI.CustomerEditPage" %>

<%--<%@ Register TagPrefix="Site" Assembly="CanhCam.Features.Product" Namespace="CanhCam.Web.ProductUI" %>--%>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <%--Breadcrumb--%>
    <portal:BreadcrumbAdmin ID="breadcrumb" runat="server"
        CurrentPageTitle="<%$Resources:AccountResources, CustomerEditTitle %>"
        CurrentPageUrl="~/Product/AdminCP/Member/CustomerEdit.aspx" />
    <div class="admin-content">
        <%--Buttons panel--%>
        <portal:NotifyMessage ID="message" runat="server" />

        <portal:HeadingPanel ID="heading" runat="server">

            <asp:Button SkinID="UpdateButton" ID="btnUpdate" Text="<%$Resources:Resource, UpdateButton %>"
                ValidationGroup="customer" runat="server" />
            <asp:Button SkinID="UpdateButton" ID="btnUpdateAndNew"
                Text="<%$Resources:Resource, UpdateAndNewButton %>" ValidationGroup="customer"
                runat="server" />
            <asp:Button SkinID="UpdateButton" ID="btnUpdateAndClose"
                Text="<%$Resources:Resource, UpdateAndCloseButton %>" ValidationGroup="customer"
                runat="server" />
            <asp:Button SkinID="InsertButton" ID="btnInsert" Text="<%$Resources:Resource, InsertButton %>"
                ValidationGroup="customer" runat="server" />
            <asp:Button SkinID="InsertButton" ID="btnInsertAndNew"
                Text="<%$Resources:Resource, InsertAndNewButton %>" ValidationGroup="customer"
                runat="server" />
            <asp:Button SkinID="InsertButton" ID="btnInsertAndClose"
                Text="<%$Resources:Resource, InsertAndCloseButton %>" ValidationGroup="customer"
                runat="server" />
            <asp:Button SkinID="DeleteButton" ID="btnDelete" runat="server" Text="<%$Resources:Resource, DeleteButton %>"
                CausesValidation="false" Visible="false" />
        </portal:HeadingPanel>

        <div class="workplace admin-content-bg-white">

            <div class="form-horizontal">
                <asp:UpdatePanel ID="up" runat="server">
                    <ContentTemplate>
                        <%--Name--%>
                        <div class="settingrow form-group row align-items-center">
                            <gb:SiteLabel ID="lblName" runat="server" ForControl="txtName"
                                CssClass="settinglabel control-label col-sm-3 mb-0"
                                ConfigKey="CustomerNameLabel" ResourceFile="AccountResources" />
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtName" runat="server" MaxLength="100"
                                    CssClass="forminput verywidetextbox form-control" />
                                <asp:RequiredFieldValidator ID="reqName" runat="server"
                                    ControlToValidate="txtName" Display="Dynamic" SetFocusOnError="true"
                                    CssClass="txterror" ValidationGroup="customer" />
                            </div>
                        </div>
                        <%--Login Name--%>
                        <div class="settingrow form-group row align-items-center">
                            <gb:SiteLabel ID="lblLoginName" runat="server" ForControl="txtLoginName"
                                CssClass="settinglabel control-label col-sm-3 mb-0"
                                ConfigKey="CustomerLoginNameLabel" ResourceFile="AccountResources" />
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtLoginName" runat="server" MaxLength="50"
                                    CssClass="forminput verywidetextbox form-control" />
                            </div>
                        </div>
                        <%--First Name--%>
                        <div class="settingrow form-group row align-items-center">
                            <gb:SiteLabel ID="lblFirstName" runat="server" ForControl="txtFirstName"
                                CssClass="settinglabel control-label col-sm-3 mb-0"
                                ConfigKey="CustomerFirstNameLabel" ResourceFile="AccountResources" />
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtFirstName" runat="server" MaxLength="100"
                                    CssClass="forminput verywidetextbox form-control" />
                            </div>
                        </div>
                        <%--Last Name--%>
                        <div class="settingrow form-group row align-items-center">
                            <gb:SiteLabel ID="lblLastName" runat="server" ForControl="txtLastName"
                                CssClass="settinglabel control-label col-sm-3 mb-0"
                                ConfigKey="CustomerLastNameLabel" ResourceFile="AccountResources" />
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtLastName" runat="server" MaxLength="100"
                                    CssClass="forminput verywidetextbox form-control" />
                            </div>
                        </div>  
                        <div class="settingrow form-group row align-items-center" runat="server" id="divERPCode" visible="false">
                            <gb:SiteLabel ID="SiteLabel4" runat="server" ForControl="txtLastName"
                                CssClass="settinglabel control-label col-sm-3 mb-0"
                                ConfigKey="CustomerCodeLabel" ResourceFile="AccountResources" />
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtCustomerCode" runat="server" MaxLength="100"
                                    CssClass="forminput verywidetextbox form-control" />
                            </div>
                        </div>
                        <%--Email--%>
                        <div class="settingrow form-group row align-items-center">
                            <gb:SiteLabel ID="lblEmail" runat="server" ForControl="txtEmail"
                                CssClass="settinglabel control-label col-sm-3 mb-0"
                                ConfigKey="CustomerEmailLabel" ResourceFile="AccountResources" />
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtEmail" runat="server" MaxLength="100"
                                    CssClass="forminput verywidetextbox form-control" />
                                <asp:RequiredFieldValidator ID="reqEmail" runat="server"
                                    ControlToValidate="txtEmail" Display="Dynamic" SetFocusOnError="true"
                                    CssClass="txterror" ValidationGroup="customer" />
                                <asp:RegularExpressionValidator ID="regexEmail" runat="server"
                                    ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                    ControlToValidate="txtEmail"
                                    Display="Dynamic" SetFocusOnError="true"
                                    ValidationGroup="customer" />
                            </div>
                        </div>
                        <%--Phone--%>
                        <div class="settingrow form-group row align-items-center">
                            <gb:SiteLabel ID="lblPhone" runat="server" ForControl="txtPhone"
                                CssClass="settinglabel control-label col-sm-3 mb-0"
                                ConfigKey="CustomerPhoneLabel" ResourceFile="AccountResources" />
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtPhone" runat="server" MaxLength="50"
                                    CssClass="forminput verywidetextbox form-control" />
                            </div>
                        </div>
                        <div class="settingrow form-group row align-items-center">
                            <gb:SiteLabel ID="SiteLabel1" runat="server" ForControl="txtTotalOrders"
                                CssClass="settinglabel control-label col-sm-3 mb-0" Text="Hạng thành viên" />
                            <div class="col-sm-9">
                                <div class="input-group">
                                    <asp:TextBox ID="txtRank" runat="server" MaxLength="100"
                                        CssClass="form-control" ReadOnly="true" />
                                    <div class="input-group-btn">
                                        <asp:Button SkinID="UpdateButton" ID="btnResetRank" Text="Tính lại hạng thành viên"
                                            ValidationGroup="customer" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="settingrow form-group row align-items-center">
                            <gb:SiteLabel ID="SiteLabel2" runat="server" ForControl="txtTotalOrders"
                                CssClass="settinglabel control-label col-sm-3 mb-0"
                                ConfigKey="MemberRankViewPointLabel" ResourceFile="ProductResources" />
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtTotalPoint" runat="server" MaxLength="100"
                                    CssClass="forminput verywidetextbox form-control" ReadOnly="true" />
                            </div>
                        </div>
                        <div class="settingrow form-group row align-items-center">
                            <gb:SiteLabel ID="SiteLabel3" runat="server" ForControl="txtTotalOrders"
                                CssClass="settinglabel control-label col-sm-3 mb-0" Text="Điểm tích lũy" />
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtTotalApprovedPoint" runat="server" MaxLength="100"
                                    CssClass="forminput verywidetextbox form-control" ReadOnly="true" />
                            </div>
                        </div>
                        <%--Total Orders Bought--%>
                        <div class="settingrow form-group row align-items-center">
                            <gb:SiteLabel ID="lblTotalOrders" runat="server" ForControl="txtTotalOrders"
                                CssClass="settinglabel control-label col-sm-3 mb-0"
                                ConfigKey="CustomerTotalOrdersBoughtLabel" ResourceFile="AccountResources" />
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtTotalOrders" runat="server" MaxLength="100"
                                    CssClass="forminput verywidetextbox form-control" ReadOnly="true" />
                            </div>
                        </div>
                        <%--Total Money--%>
                        <div class="settingrow form-group row align-items-center">
                            <gb:SiteLabel ID="lblTotalMoney" runat="server" ForControl="txtTotalMoney"
                                CssClass="settinglabel control-label col-sm-3 mb-0"
                                ConfigKey="CustomerTotalMoneyLabel" ResourceFile="AccountResources" />
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtTotalMoney" runat="server" MaxLength="100"
                                    CssClass="forminput verywidetextbox form-control" ReadOnly="true" />
                            </div>
                        </div>
                        <%--Confirmed Money--%>
                        <div class="settingrow form-group row align-items-center">
                            <gb:SiteLabel ID="lblConfirmedMoney" runat="server" ForControl="txtConfirmedMoney"
                                CssClass="settinglabel control-label col-sm-3 mb-0"
                                ConfigKey="CustomerTotalConfirmedMoneyLabel" ResourceFile="AccountResources" />
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtConfirmedMoney" runat="server" MaxLength="100"
                                    CssClass="forminput verywidetextbox form-control" ReadOnly="true" />
                            </div>
                        </div>
                        <%--UnconfirmedMoney--%>
                        <div class="settingrow form-group row align-items-center">
                            <gb:SiteLabel ID="lblUnconfirmedMoney" runat="server" ForControl="txtUnconfirmedMoney"
                                CssClass="settinglabel control-label col-sm-3 mb-0"
                                ConfigKey="CustomerTotalUnconfirmedMoneyLabel" ResourceFile="AccountResources" />
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtUnconfirmedMoney" runat="server" MaxLength="100"
                                    CssClass="forminput verywidetextbox form-control" ReadOnly="true" />
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <%--Order History--%>
                <div id="divOrderHistory" class="settingrow form-group row align-items-center" runat="server">
                    <gb:SiteLabel ID="lblOrderHistory" runat="server" ForControl="grid"
                        CssClass="settinglabel control-label col-sm-3 mb-0"
                        ConfigKey="CustomerOrderHistoryLabel" ResourceFile="AccountResources" />
                    <div class="col-sm-12">
                        <hr style="height: 1px; background: whitesmoke;" />
                    </div>
                    <div class="col-sm-12">
                        <telerik:RadGrid ID="grid" SkinID="radGridSkin" runat="server">
                            <MasterTableView DataKeyNames="OrderId,OrderStatus" AllowSorting="false">
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderStyle-Width="35"
                                        HeaderText="<%$Resources:Resource, RowNumber %>">
                                        <ItemTemplate>
                                            <%# (grid.PageSize * grid.CurrentPageIndex) + Container.ItemIndex + 1%>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn HeaderText="<%$Resources:ProductResources, OrderCodeLabel %>"
                                        DataField="OrderCode" />
                                    <telerik:GridTemplateColumn
                                        HeaderText="<%$Resources:ProductResources, OrderCustomerLabel %>">
                                        <ItemTemplate>
                                            <%# CanhCam.Web.ProductUI.OrderHelper.GetCustomer(Eval("BillingFirstName").ToString(), Eval("BillingLastName").ToString()) %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn
                                        HeaderText="<%$Resources:ProductResources, OrderCreatedDateLabel %>">
                                        <ItemTemplate>
                                            <%# DateTimeHelper.Format(Convert.ToDateTime(Eval("CreatedUtc")), timeZone, Resources.ProductResources.OrderCreatedDateFormat, timeOffset)%>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn
                                        HeaderText="<%$Resources:ProductResources, OrderStatusLabel %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOrderStatus"
                                                ForeColor='<%#CanhCam.Web.ProductUI.OrderHelper.GetForeColor(Convert.ToInt32(Eval("OrderStatus")))%>'
                                                Text='<%# CanhCam.Web.ProductUI.ProductHelper.GetOrderStatus(Convert.ToInt32(Eval("OrderStatus"))) %>'
                                                runat="server" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Right"
                                        ItemStyle-HorizontalAlign="Right"
                                        HeaderText="<%$Resources:ProductResources, OrderTotalLabel %>">
                                        <ItemTemplate>
                                            <%# CanhCam.Web.ProductUI.ProductHelper.FormatPrice(Convert.ToDecimal(Eval("OrderTotal")), true)%>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:HyperLink CssClass="cp-link btn btn-default bg-teal" ID="lnkQuickView"
                                                NavigateUrl="#" runat="server"
                                                Text="<%$Resources:ProductResources, OrderQuickViewLink %>">
													</asp:HyperLink>
                                            <asp:HyperLink CssClass="cp-link btn btn-default bg-teal" ID="EditLink"
                                                runat="server" Text="<%$Resources:ProductResources, OrderDetailLink %>"
                                                NavigateUrl='<%# SiteRoot + "/Product/AdminCP/OrderEdit.aspx?OrderID=" + Eval("OrderId") %>'>
													</asp:HyperLink>
                                            <a id="lnkPrint" style="display: inline-block; padding-left: 5px" runat="server" onclick='<%# "printOrder(" + Eval("OrderId").ToString() + ")" %>' href="javascript:;"><i class="fa fa-print"></i>In</a>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                        <telerik:RadToolTipManager ID="RadToolTipManager1" runat="server" Position="BottomCenter"
                            Width="960" RelativeTo="Element" OnAjaxUpdate="OnAjaxUpdate" HideEvent="ManualClose"
                            ShowEvent="OnClick" EnableDataCaching="true">
                        </telerik:RadToolTipManager>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <style type="text/css">
        .RadToolTip td.rtWrapperContent {
            background: none repeat scroll 0 0 #dbf4cd !important;
            border: 1px solid #dbf4cd !important;
        }

        .OrderPopup {
            color: #4e4f4f;
            font-family: Arial !important;
            margin: 10px 0;
            width: 100%;
        }

            .OrderPopup .BillingDetails,
            .OrderPopup .ShippingDetails {
                width: 32%;
            }

            .OrderPopup .OrderProducts {
                width: 36%;
            }

                .OrderPopup .Key,
                .OrderPopup .Value,
                .OrderPopup .OrderProducts .Key,
                .OrderPopup .OrderProducts .Price,
                .OrderPopup .OrderProducts td {
                    background: none repeat scroll 0 0 #dbf4cd !important;
                    border: medium none !important;
                    height: auto !important;
                    padding: 4px !important;
                }

            .OrderPopup .BillingDetails .Key,
            .OrderPopup .ShippingDetails .Key {
                background: none repeat scroll 0 0 #dbf4cd !important;
                border: medium none;
                vertical-align: top;
                width: 130px;
            }

            .OrderPopup .BillingDetails .Value,
            .OrderPopup .ShippingDetails .Value {
                background: none repeat scroll 0 0 #dbf4cd !important;
                border: medium none;
                vertical-align: top;
            }

            .OrderPopup .Seperator {
                background: #9fce8c none repeat scroll 0 0 !important;
                border: medium none !important;
                padding: 1px;
            }

            .OrderPopup .QuickViewPanel {
                background: none repeat scroll 0 0 #dbf4cd !important;
                border: medium none !important;
                vertical-align: top;
                padding-left: 5px;
            }

            .OrderPopup h5 {
                font-size: 13px;
                font-weight: bold;
                margin: 0 0 4px;
                padding-bottom: 5px;
            }

            .OrderPopup .OrderProducts .Key {
                text-align: right;
            }

            .OrderPopup .OrderProducts .Price {
                text-align: right;
                width: 100px;
            }

            .OrderPopup .OrderProducts .Seperator {
                margin: 10px 0;
            }
    </style>
    <script type="text/javascript">
        function printOrder(orderId) {
            $.ajax({
                url: "/Product/Services/OrderPrint.ashx",
                type: "GET",
                dataType: "html",
                data: {
                    id: orderId
                },
                cache: !0,
                success: function (data) {
                    var oldstr = document.body.innerHTML;
                    document.body.innerHTML = "<html><head><title></title></head><body>" + data + "</body>";
                    window.print();
                    document.body.innerHTML = oldstr;
                }
            })

            return false;
        }
    </script>
</asp:Content>