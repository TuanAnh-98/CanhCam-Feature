<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="AddProduct.aspx.cs" Inherits="CanhCam.Web.AffiliateUI.AddProduct" %>

<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <div class="admin-content col-md-12">
        <portal:HeadingPanel ID="heading" runat="server">
            <div>
                <asp:Button runat="server" ID="btnSubmit" Text="Add neww" SkinID="UpdateButton" />
            </div>
            <portal:NotifyMessage ID="message" runat="server" />
        </portal:HeadingPanel>
        <div class="workplace">
            <div class="settingrow form-group">
                <gb:SiteLabel ID="Sitelabel1" runat="server" CssClass="settinglabel control-label col-sm-3"
                    Text="Mã sản phẩm " />
                <div class="col-sm-9">
                    <asp:TextBox ID="txtCode" runat="server" />
                </div>
            </div>
            <div class="settingrow form-group">
                <gb:SiteLabel ID="Sitelabel2" runat="server" CssClass="settinglabel control-label col-sm-3"
                    Text="Giá " />
                <div class="col-sm-9">
                    <asp:TextBox ID="txtPrice" runat="server" />
                </div>
            </div>
            <div class="settingrow form-group">
                <gb:SiteLabel ID="Sitelabel3" runat="server" CssClass="settinglabel control-label col-sm-3"
                    Text="Giá thị trường " />
                <div class="col-sm-9">
                    <asp:TextBox ID="txtOldPrice" runat="server" />
                </div>
            </div>
            <div class="settingrow form-group">
                <gb:SiteLabel ID="Sitelabel4" runat="server" CssClass="settinglabel control-label col-sm-3"
                    Text="Số lượng " />
                <div class="col-sm-9">
                    <asp:TextBox ID="txtQuantity" runat="server" />
                </div>
            </div>
            <div class="settingrow form-group">
                <gb:SiteLabel ID="Sitelabel5" runat="server" CssClass="settinglabel control-label col-sm-3"
                    Text="Trọng lượng " />
                <div class="col-sm-9">
                    <asp:TextBox ID="txtWeight" runat="server" />
                </div>
            </div>
            <div class="settingrow form-group">
                <gb:SiteLabel ID="Sitelabel6" runat="server" CssClass="settinglabel control-label col-sm-3"
                    Text="Tên sản phẩm " />
                <div class="col-sm-9">
                    <asp:TextBox ID="txtName" runat="server" />
                </div>
            </div>
            <div class="settingrow form-group">
                <gb:SiteLabel ID="Sitelabel7" runat="server" CssClass="settinglabel control-label col-sm-3"
                    Text="Đường dẫn (URL) " />
                <div class="col-sm-9">
                    <asp:TextBox ID="txtUrl" runat="server" />
                </div>
            </div>
            <div class="settingrow form-group">
                <gb:SiteLabel ID="Sitelabel36" runat="server" ForControl="edBriefContent" CssClass="settinglabel control-label col-sm-3"
                    Text="Mô tả ngắn " />
                <div class="col-sm-9">
                    <gbe:EditorControl ID="edBriefContent" runat="server" />
                    <asp:TextBox ID="txtBriefContent" CssClass="form-control" Visible="false" runat="server" />
                </div>
            </div>
            <div class="settingrow form-group">
                <gb:SiteLabel ID="Sitelabel8" runat="server" ForControl="edFullContent" CssClass="settinglabel control-label col-sm-3"
                    Text="Nội dung " />
                <div class="col-sm-9">
                    <gbe:EditorControl ID="edFullContent" runat="server" />
                    <asp:TextBox ID="txtFullContent" CssClass="form-control" Visible="false" runat="server" />
                </div>
            </div>
            <div class="settingrow form-group">
                <gb:SiteLabel ID="Sitelabel9" runat="server" CssClass="settinglabel control-label col-sm-3"
                    Text="Hình ảnh " />
                <div class="col-sm-9">
                    <div>
                        <telerik:RadAsyncUpload ID="fileUpload1" SkinID="radAsyncUploadSkin" MultipleFileSelection="Disabled"
                            AllowedFileExtensions=".png,.jpg" runat="server" />
                    </div>
                </div>
            </div>
            <div class="settingrow form-group">
                <gb:SiteLabel ID="Sitelabel10" runat="server" CssClass="settinglabel control-label col-sm-3"
                    Text="Mở trang mới " />
                <div class="col-sm-9">
                    <asp:CheckBox ID="chbOpenNewWindow" runat="server" Checked="false" />
                </div>
            </div>
            <div class="settingrow form-group">
                <gb:SiteLabel ID="Sitelabel11" runat="server" CssClass="settinglabel control-label col-sm-3"
                    Text="Cho phép mua hàng " />
                <div class="col-sm-9">
                    <asp:CheckBox ID="chbDisableBuyButton" runat="server" Checked="true" />
                </div>
            </div>
            <div class="settingrow form-group">
                <gb:SiteLabel ID="Sitelabel12" runat="server" CssClass="settinglabel control-label col-sm-3"
                    Text="Duyệt hiển thị " />
                <div class="col-sm-9">
                    <asp:CheckBox ID="chbIsPublished" runat="server" Checked="true" />
                </div>
            </div>
            <div class="settingrow form-group">
                    <gb:SiteLabel ID="lblStartDate" runat="server"
                        Text="Ngày bắt đầu hiển thị" CssClass="settinglabel control-label col-sm-3" />
                    <div class="col-sm-9">
                        <gb:DatePickerControl ID="dpStartDate" runat="server" ShowTime="True" SkinID="news"
                            CssClass="forminput"></gb:DatePickerControl>
                        <%--<portal:gbHelpLink ID="GbHelpLink10" runat="server" RenderWrapper="false" HelpKey="newsedit-startdate-help" />--%>
                        <asp:RequiredFieldValidator ID="reqStartDate" runat="server" ControlToValidate="dpStartDate"
                            Display="Dynamic" SetFocusOnError="true" CssClass="txterror" ValidationGroup="news" />
                    </div>
                </div>
        </div>
    </div>
</asp:Content>
