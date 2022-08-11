<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="ImportNews.aspx.cs" Inherits="CanhCam.Web.CustomUI.ImportNewsPage" %>

<%@ Register TagPrefix="Site" Assembly="CanhCam.Features.News" Namespace="CanhCam.Web.NewsUI" %>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
      <div class="admin-content col-md-12">
        <portal:NotifyMessage ID="message" runat="server" />
        <div class="workplace">
            <div class="form-horizontal">
                <div class="settingrow form-group">
                    <gb:SiteLabel ID="lblZones" runat="server" Text="Import vào chuyên mục" CssClass="control-label col-sm-3" />
                    <div class="col-sm-3">
                        <asp:DropDownList ID="ddZones" AutoPostBack="false" runat="server" CssClass="form-control" />
                    </div>
                </div>
                <div class="settingrow form-group">
                    <gb:SiteLabel ID="SiteLabel1" runat="server" Text="Lưu hình ảnh trong content về server" CssClass="control-label col-sm-3" />
                    <div class="col-sm-3">
                        <asp:CheckBox ID="chkDowloadImageInContent" runat="server" CssClass="form-control" />
                    </div>
                </div>
                <div class="settingrow form-group">
                    <gb:SiteLabel ID="lblTitle" runat="server" Text="Url" ForControl="txtTitle" CssClass="control-label col-sm-3" />
                    <div class="col-sm-9">
                        <div class="form-inline">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <asp:TextBox ID="txtUrl" runat="server" MaxLength="255" />
                                </div>

                                <div class="form-group">
                                    <asp:Button SkinID="DefaultButton" ID="btnImport" Text="Import"
                                        runat="server" CausesValidation="false" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
