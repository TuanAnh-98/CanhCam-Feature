<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="ImportProductByExcel.aspx.cs" Inherits="CanhCam.Web.CustomUI.ImportProductByExcelPages" %>

<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <div class="admin-content col-md-12">
        <portal:NotifyMessage ID="message" runat="server" />
        <div class="workplace">
            <div class="form-horizontal">
                <div class="settingrow form-group">
                    <gb:SiteLabel ID="lblTitle" runat="server" Text="Url" ForControl="txtTitle" CssClass="control-label col-sm-3" />
                    <div class="col-sm-9">
                        <div class="form-inline">
                            <div class="col-md-12">
                               <div class="settingrow form-group">
                                    <telerik:RadAsyncUpload ID="fileUpload" SkinID="radAsyncUploadSkin" MultipleFileSelection="Disabled"
                                        AllowedFileExtensions="xls" runat="server" />
                                </div>
                                <div class="settingrow form-group">
                                    <ul>
                                    <li>
                                        <asp:CheckBox ID="chkOverride" Text="Cập nhật sản phẩm nếu đã tồn tại?" runat="server" />
                                    </li>
                                    <li>
                                    <asp:CheckBox ID="chkDowloadImageInContent" runat="server"  Text="Lưu hình ảnh trong content về server" />
                                    </li>
                                    </ul>
                                </div>  
                              <div class="settingrow form-group">
                                    <asp:Button SkinID="DefaultButton" ID="btnImport" Text="Import"
                                        runat="server" CausesValidation="false" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div> 

            <div> 
                <asp:Literal runat="server" ID="litResult" />
            </div>
        </div>
    </div>
</asp:Content>
