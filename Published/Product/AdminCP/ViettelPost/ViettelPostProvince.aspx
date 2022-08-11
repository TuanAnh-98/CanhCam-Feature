<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="ViettelPostProvince.aspx.cs" Inherits="CanhCam.Web.ProductUI.ViettelPostProvincePage" %>

<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:BreadcrumbAdmin ID="breadcrumb" runat="server"
        CurrentPageTitle="ViettelPost Province" CurrentPageUrl="~/Product/AdminCP/Shipping/ViettelPostProvince.aspx" />
    <div class="admin-content col-md-12">
        <portal:HeadingPanel ID="heading" runat="server">
            <asp:Button SkinID="UpdateButton" ID="btnUpdate" Text="Syns" runat="server" />
            <asp:Button SkinID="UpdateButton" ID="btnUpdateLocation" Text="Cập nhật" runat="server" />
        </portal:HeadingPanel>
        <portal:NotifyMessage ID="message" runat="server" />

        <div class="workplace">
            <telerik:RadGrid ID="grid" SkinID="radGridSkin" runat="server">
                <MasterTableView DataKeyNames="RowID,ProvinceGuid,ViettelPostProvinceCode">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderStyle-Width="35" HeaderText="<%$Resources:Resource, RowNumber %>">
                            <ItemTemplate>
                                <%# (grid.PageSize * grid.CurrentPageIndex) + Container.ItemIndex + 1%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Địa điểm">
                            <ItemTemplate>
                                <%# GetName(new Guid(Eval("ProvinceGuid").ToString()))%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Viettel Post Code">
                            <ItemTemplate>
                                <asp:TextBox ID="txtProductCode" Text='<%# Eval("ViettelPostProvinceCode") %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>
