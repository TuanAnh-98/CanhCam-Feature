<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="ShippingLocation.aspx.cs" Inherits="CanhCam.Web.ProductUI.ShippingLocation" %>

<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:BreadcrumbAdmin ID="breadcrumb" runat="server"
        CurrentPageTitle="ViettelPost Location" CurrentPageUrl="~/Product/AdminCP/Shipping/ShippingLocation.aspx" />
    <div class="admin-content col-md-12">
        <portal:HeadingPanel ID="heading" runat="server">
            <asp:Button SkinID="UpdateButton" ID="btnUpdate" Text="Syns" runat="server" />
            <asp:Button SkinID="UpdateButton" ID="btnUpdateLocation" Text="Cập nhật" runat="server" />
            <asp:Button SkinID="UpdateButton" ID="btnSyns" Text="Syns VittelPost" runat="server" />
        </portal:HeadingPanel>
        <portal:NotifyMessage ID="message" runat="server" />
        <asp:Panel ID="pnlSearch" CssClass="headInfo form-horizontal" runat="server">
            <div class="settingrow form-group">
                <gb:SiteLabel ID="lblZones" runat="server"
                    ForControl="ddZones" CssClass="settinglabel control-label col-sm-3" />
                <div class="col-sm-9">
                    <asp:DropDownList ID="ddlProvince" AutoPostBack="true" runat="server" DataTextField="Name" DataValueField="Guid" AppendDataBoundItems="true">
                        <asp:ListItem Value="-1">Tỉnh / Thành</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="settingrow form-group">
                <gb:SiteLabel ID="SiteLabel1" runat="server" Text=""
                    ForControl="ddZones" CssClass="settinglabel control-label col-sm-3" />
                <div class="col-sm-9">
                    <asp:HyperLink runat="server" ID="linkGetCode" Target="_blank" />
                    <br />
                    <a href="https://jsonformatter.curiousconcept.com/" target="_blank">Bạn có thể format json cho dễ nhìn vào bằng link này</a>
                </div>
            </div>
        </asp:Panel>
        <div class="workplace">

            <telerik:RadGrid ID="grid" SkinID="radGridSkin" runat="server">
                <MasterTableView DataKeyNames="RowID,ViettelPostCode2">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderStyle-Width="35" HeaderText="<%$Resources:Resource, RowNumber %>">
                            <ItemTemplate>
                                <%# (grid.PageSize * grid.CurrentPageIndex) + Container.ItemIndex + 1%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Địa điểm">
                            <ItemTemplate>
                                <%# Eval("GeoZoneName") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Viettel Post Code">
                            <ItemTemplate>
                                <asp:TextBox ID="txtProductCode" Text='<%# Eval("ViettelPostCode2") %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>
