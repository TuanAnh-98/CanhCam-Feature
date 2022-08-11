<%@ Page Language="c#" CodeBehind="DiscountProductRangeDialog.aspx.cs" MasterPageFile="~/App_MasterPages/DialogMaster.Master"
    AutoEventWireup="false" Inherits="CanhCam.Web.ProductUI.DiscountProductRangeDialog" %>

<%@ Import Namespace="CanhCam.Web.ProductUI" %>
<asp:Content ContentPlaceHolderID="phMain" ID="MPContent" runat="server">
    <div class="container">
        <portal:HeadingControl ID="heading" Text="Chọn giá khuyến mãi theo số lượng sản phẩm" runat="server" />
        <asp:UpdatePanel ID="up" runat="server">
            <ContentTemplate>
                <portal:NotifyMessage ID="message" runat="server" />
                <div class="clearfix">
                    <asp:UpdatePanel ID="upOrderDiscountRange" runat="server">
                        <ContentTemplate>
                            <div class="mrb10 text-right">
                                <asp:Button ID="btnUpdate" SkinID="DefaultButton" Text="Cập nhật" runat="server" />
                            </div>
                            <telerik:RadGrid ID="gridOrderDiscountRange" SkinID="radGridSkin" runat="server">
                                <MasterTableView DataKeyNames="ItemID,DiscountType,FromPrice,ToPrice,DiscountAmount,MaximumDiscount">
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderText="Số lượng từ">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFromQuantity" Text='<%# ProductHelper.FormatPrice(Convert.ToDecimal(Eval("FromPrice"))) %>' runat="server" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Số lượng đến">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtToQuantity" Text='<%# ProductHelper.FormatPrice(Convert.ToDecimal(Eval("ToPrice"))) %>' runat="server" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Giảm giá">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtDiscountAmount" SkinID="PriceTextBox" Style="display: inline-block" Text='<%# ProductHelper.FormatPrice(Convert.ToDecimal(Eval("DiscountAmount"))) %>' runat="server" />
                                                <asp:DropDownList ID="ddlDiscountType" CssClass="percentage" Style="width: auto; display: inline-block" runat="server">
                                                    <asp:ListItem Value="1" Text="%"></asp:ListItem>
                                                    <asp:ListItem Value="0" Text="VND"></asp:ListItem>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Giảm tối đa" UniqueName="MaximumDiscount">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtMaximumDiscount" Text='<%# ProductHelper.FormatPrice(Convert.ToDecimal(Eval("MaximumDiscount"))) %>' runat="server" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn>
                                            <ItemTemplate>
                                                <asp:HyperLink CssClass="popup-link" ID="GiftProducts" runat="server"
                                                    Font-Bold='<%# !string.IsNullOrEmpty(Eval("GiftHtml").ToString()) %>'
                                                    Text="Quà tặng" NavigateUrl='<%# SiteRoot + "/Product/AdminCP/Promotions/DiscountGiftDialog.aspx?ItemID=" + Eval("ItemID") %>'>
                                                </asp:HyperLink>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn>
                                            <ItemTemplate>
                                                <asp:Button SkinID="DefaultButton" ID="btnOrderRangeDelete" Text="Xóa" runat="server" CausesValidation="false" CommandName="Delete" CommandArgument='<%#Eval("ItemID").ToString()%>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <%--<telerik:GridClientSelectColumn HeaderStyle-Width="35" />--%>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                            <div class="mrt10">
                                <asp:Button SkinID="DefaultButton" ID="btnOrderRangeNewRow" Text="Thêm dòng" runat="server" CausesValidation="false" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <style type="text/css">
        .RadGrid {
            width: 100% !important;
        }

        .RadEditor .reContentCell {
            height: 100% !important;
        }

        textarea {
            height: auto;
        }
    </style>
    <portal:SessionKeepAliveControl ID="ka1" runat="server" />
</asp:Content>