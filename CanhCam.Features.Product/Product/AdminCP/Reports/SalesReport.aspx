<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="SalesReport.aspx.cs" Inherits="CanhCam.Web.ProductUI.SalesReportPage" %>

<%@ Register TagPrefix="Site" Assembly="CanhCam.Features.Product" Namespace="CanhCam.Web.ProductUI" %>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <Site:ProductDisplaySettings ID="displaySettings" runat="server" />
    <portal:BreadcrumbAdmin ID="breadcrumb" runat="server"
        CurrentPageTitle="Thống kê bán hàng" CurrentPageUrl="~/Product/AdminCP/Reports/SalesReport.aspx" />
    <div class="admin-content col-md-12">
        <portal:HeadingPanel ID="heading" Text="<%$Resources:ProductResources,ReportSalesTitle %>" runat="server">
            <asp:Button ID="btnExport" SkinID="DefaultButton" Text="<%$Resources:ProductResources,OrderExportButton %>" OnClick="btnExport_Click" runat="server" CausesValidation="false" />
        </portal:HeadingPanel>
        <portal:NotifyMessage ID="message" runat="server" />
        <div class="workplace">
            <asp:Panel ID="pnlSearch" CssClass="form-horizontal" DefaultButton="btnSearch" runat="server">
                <div class="settingrow form-group">
                    <gb:SiteLabel ID="lblFromDate" runat="server" ConfigKey="OrderDateFromLabel"
                        ResourceFile="ProductResources" ForControl="dpFromDate" CssClass="settinglabel control-label col-sm-3" />
                    <div class="col-sm-9">
                        <div class="form-inline">
                            <telerik:RadDatePicker ID="dpFromDate" runat="server" />
                            <gb:SiteLabel ID="lblEndDate" runat="server" ConfigKey="OrderDateToLabel" ResourceFile="ProductResources"
                                ForControl="dpToDate" CssClass="settinglabel control-label" />
                            <telerik:RadDatePicker ID="dpToDate" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="settingrow form-group">
                    <gb:SiteLabel ID="lblOrderStatus" runat="server" ConfigKey="OrderStatusLabel" ResourceFile="ProductResources"
                        ForControl="ddOrderStatus" CssClass="settinglabel control-label col-sm-3" />
                    <div class="col-sm-9">
                        <asp:DropDownList ID="ddOrderStatus" Width="350" runat="server" />
                    </div>
                </div>
                <div class="settingrow form-group">
                    <gb:SiteLabel ID="lblReportBy" runat="server" Text="Thống kê theo"
                        ForControl="ddReportBy" CssClass="settinglabel control-label col-sm-3" />
                    <div class="col-sm-9">
                        <asp:DropDownList ID="ddReportBy" Width="350" runat="server">
                            <asp:ListItem Text="Ngày" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Tuần" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Tháng" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Năm" Value="4"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="settingrow form-group">
                    <gb:SiteLabel ID="lblReportType" runat="server" Text="Hiển thị"
                        ForControl="chkListReportType" CssClass="settinglabel control-label col-sm-3" />
                    <div class="col-sm-9">
                        <asp:CheckBoxList ID="chkListReportType" SkinID="ReportType" CssClass="nav" RepeatLayout="UnorderedList" runat="server">
                            <asp:ListItem Text="Lưới" Selected="True" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Biểu đồ doanh thu" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Biểu đồ số lượng" Value="3"></asp:ListItem>
                        </asp:CheckBoxList>
                    </div>
                </div>
                <div class="settingrow form-group">
                    <div class="col-sm-9 col-sm-offset-3">
                        <asp:Button SkinID="DefaultButton" ID="btnSearch" Text="<%$Resources:ProductResources,ReportButton %>" runat="server" CausesValidation="false" />
                    </div>
                </div>
            </asp:Panel>
            <telerik:RadGrid ID="grid" SkinID="radGridSkin" runat="server">
                <MasterTableView AllowSorting="false" AllowPaging="false">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="STT">
                            <ItemTemplate>
                                <%# Eval("Row")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Thời gian">
                            <ItemTemplate>
                                <%# Eval("Time")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderText="Số Đơn hàng">
                            <ItemTemplate>
                                <%# Eval("OrderCount") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderText="Tổng tiền">
                            <ItemTemplate>
                                <%# Eval("OrderTotalFormat")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
            <telerik:RadHtmlChart ID="chart" Height="500" runat="server">
                <PlotArea>
                    <Series>
                        <telerik:LineSeries DataFieldY="OrderTotal">
                            <LabelsAppearance DataFormatString="{0:#,##0}"></LabelsAppearance>
                            <TooltipsAppearance DataFormatString="{0:#,##0} VNĐ" Visible="true"></TooltipsAppearance>
                        </telerik:LineSeries>
                    </Series>
                    <XAxis DataLabelsField="Time">
                        <TitleAppearance Text="Thời gian"></TitleAppearance>
                        <MinorGridLines Visible="false"></MinorGridLines>
                        <MajorGridLines Visible="false"></MajorGridLines>
                        <LabelsAppearance RotationAngle="-90"></LabelsAppearance>
                    </XAxis>
                    <YAxis>
                        <TitleAppearance Text="Doanh thu"></TitleAppearance>
                        <MinorGridLines Visible="false"></MinorGridLines>
                        <LabelsAppearance DataFormatString="{0:#,##0}"></LabelsAppearance>
                    </YAxis>
                </PlotArea>
                <Legend>
                    <Appearance Position="Bottom"></Appearance>
                </Legend>
                <ChartTitle Text="Thống kê bán hàng"></ChartTitle>
            </telerik:RadHtmlChart>
            <telerik:RadHtmlChart ID="chart2" Height="500" runat="server">
                <PlotArea>
                    <Series>
                        <telerik:LineSeries DataFieldY="OrderCount">
                        </telerik:LineSeries>
                    </Series>
                    <XAxis DataLabelsField="Time">
                        <TitleAppearance Text="Thời gian"></TitleAppearance>
                        <MinorGridLines Visible="false"></MinorGridLines>
                        <MajorGridLines Visible="false"></MajorGridLines>
                        <LabelsAppearance RotationAngle="-90"></LabelsAppearance>
                    </XAxis>
                    <YAxis>
                        <TitleAppearance Text="Số lượng đơn hàng"></TitleAppearance>
                        <MinorGridLines Visible="false"></MinorGridLines>
                    </YAxis>
                </PlotArea>
                <Legend>
                    <Appearance Position="Bottom"></Appearance>
                </Legend>
                <ChartTitle Text="Thống kê đơn hàng theo ngày"></ChartTitle>
            </telerik:RadHtmlChart>
        </div>
    </div>
</asp:Content>