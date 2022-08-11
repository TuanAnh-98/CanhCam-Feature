<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="UserPointHistoryList.aspx.cs"
    MasterPageFile="~/App_MasterPages/layout.Master" Inherits="CanhCam.Web.ProductUI.UserPointHistoryListPage" %>

<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:BreadcrumbAdmin ID="breadcrumb" runat="server"
        CurrentPageTitle="<%$Resources:ProductResources, UserPointHistoryListTitle %>"
        CurrentPageUrl="~/Product/AdminCP/RewardPoint/UserPointHistoryList.aspx" />

    <div class="admin-content">
        <portal:NotifyMessage ID="message" runat="server" />
        <asp:Panel ID="pnlSearch" DefaultButton="btnSearch" CssClass="headInfo admin-content-bg-white form-horizontal"
            runat="server">
            <div class="settingrow form-group row align-items-center">
                <gb:SiteLabel ID="lblStartDate" runat="server" Text="Từ ngày" ForControl="dpStartDate" CssClass="settinglabel control-label col-sm-3" />
                <div class="col-sm-9">
                    <gb:DatePickerControl ID="dpStartDate" runat="server" SkinID="news"></gb:DatePickerControl>
                </div>
            </div>
            <div class="settingrow form-group row align-items-center">
                <gb:SiteLabel ID="lblEndDate" runat="server" Text="Đến ngày" ForControl="dpEndDate" CssClass="settinglabel control-label col-sm-3" />
                <div class="col-sm-9">
                    <gb:DatePickerControl ID="dpEndDate" runat="server" SkinID="news"></gb:DatePickerControl>
                </div>
            </div>
            <div class="settingrow form-group row align-items-center">
                <gb:SiteLabel ID="lblTitle" runat="server" Text="Từ khóa" 
                    ForControl="txtTitle" CssClass="settinglabel control-label col-sm-3" />
                <div class="col-sm-9">
                    <asp:TextBox ID="txtTitle" runat="server" MaxLength="255" placeholder="Họ tên, mã đơn,mô tả......" CssClass="form-control" />
                </div>
            </div>
            <div class="settingrow form-group row align-items-center">
                <div class="col-sm-9 offset-sm-3">
                    <asp:Button SkinID="DefaultButton" ID="btnSearch" Text="Tìm kiếm"
                        runat="server" CausesValidation="false" />
                </div>
            </div>
        </asp:Panel>
        <div class="workplace">
            <telerik:RadGrid ID="grid" SkinID="radGridSkin" runat="server">
                <MasterTableView DataKeyNames="RowId">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Mã đơn hàng">
                            <ItemTemplate>
                                <%# Eval("OrderCode") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Khách hàng">
                            <ItemTemplate>
                                <%# Eval("CustomerName") %> (<%# Eval("CustomerEmail") %>)
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Điểm sử dụng" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <%# Eval("Points") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Điểm còn khả dụng tại thời điểm" 
                            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <%# Eval("PointsBalance") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Mô tả" >
                            <ItemTemplate>
                                <%# Eval("Message") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ngày đặt hàng">
                            <ItemTemplate>
                                <%# DateTimeHelper.Format(Convert.ToDateTime(Eval("CreatedOnUtc")), timeZone, Resources.ProductResources.OrderCreatedDateFormat, timeOffset)%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="" ItemStyle-HorizontalAlign="Center"
                            HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:HyperLink CssClass="cp-link btn btn-default bg-teal" ID="EditLink"
                                    runat="server" Text="Chi tiết đơn hàng"
                                    NavigateUrl='/Product/AdminCP/OrderEdit.aspx?OrderID=<%# Eval("UsedWithOrderId") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>
