<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="StoreEdit.aspx.cs" Inherits="CanhCam.Web.StoreUI.StoreEditPage" %>

<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:BreadcrumbAdmin ID="breadcrumb" runat="server"
        CurrentPageTitle="<%$Resources:StoreResources, StoreEditTitle %>"
        CurrentPageUrl="~/Store/AdminCP/StoreEdit.aspx" />
    <div class="admin-content">
        <asp:UpdatePanel ID="upButton" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <portal:NotifyMessage ID="message" runat="server" />
                <portal:HeadingPanel ID="heading" runat="server">
                    <asp:Button SkinID="UpdateButton" ID="btnUpdate" Text="<%$Resources:Resource, UpdateButton %>"
                        ValidationGroup="Store" runat="server" />
                    <asp:Button SkinID="UpdateButton" ID="btnUpdateAndNew"
                        Text="<%$Resources:Resource, UpdateAndNewButton %>" ValidationGroup="Store"
                        runat="server" />
                    <asp:Button SkinID="UpdateButton" ID="btnUpdateAndClose"
                        Text="<%$Resources:Resource, UpdateAndCloseButton %>" ValidationGroup="Store"
                        runat="server" />
                    <asp:Button SkinID="InsertButton" ID="btnInsert" Text="<%$Resources:Resource, InsertButton %>"
                        ValidationGroup="Store" runat="server" />
                    <asp:Button SkinID="InsertButton" ID="btnInsertAndNew"
                        Text="<%$Resources:Resource, InsertAndNewButton %>" ValidationGroup="Store"
                        runat="server" />
                    <asp:Button SkinID="InsertButton" ID="btnInsertAndClose"
                        Text="<%$Resources:Resource, InsertAndCloseButton %>" ValidationGroup="Store"
                        runat="server" />
                    <asp:Button SkinID="DeleteButton" ID="btnDelete" runat="server" Text="<%$Resources:Resource, DeleteButton %>"
                        CausesValidation="false" />
                </portal:HeadingPanel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="workplace admin-content-bg-white">
            <div class="form-horizontal">
                <div class="settingrow form-group row align-items-center">
                    <gb:SiteLabel ID="SiteLabel1" runat="server" ForControl="txtPhone"
                        CssClass="settinglabel control-label col-sm-3 mb-0"
                        ConfigKey="StoreCodeLabel" ResourceFile="StoreResources" />
                    <div class="col-sm-9">
                        <asp:TextBox ID="txtCode" runat="server" MaxLength="100"
                            CssClass="forminput  form-control" />
                    </div>
                </div>
                <div class="settingrow form-group row align-items-center">
                    <gb:SiteLabel ID="lblPhone" runat="server" ForControl="txtPhone"
                        CssClass="settinglabel control-label col-sm-3 mb-0"
                        ConfigKey="StorePhoneLabel" ResourceFile="StoreResources" />
                    <div class="col-sm-9">
                        <asp:TextBox ID="txtPhone" runat="server" MaxLength="100"
                            CssClass="forminput verywidetextbox form-control" />
                    </div>
                </div>
                <div class="settingrow form-group row align-items-center">
                    <gb:SiteLabel ID="lblFax" runat="server" ForControl="txtFax"
                        CssClass="settinglabel control-label col-sm-3 mb-0"
                        ConfigKey="StoreFaxLabel" ResourceFile="StoreResources" />
                    <div class="col-sm-9">
                        <asp:TextBox ID="txtFax" runat="server" MaxLength="100"
                            CssClass="forminput verywidetextbox form-control" />
                    </div>
                </div>
                <div class="settingrow form-group row align-items-center">
                    <gb:SiteLabel ID="lblEmail" runat="server" ForControl="txtEmail"
                        CssClass="settinglabel control-label col-sm-3 mb-0"
                        ConfigKey="StoreEmailLabel" ResourceFile="StoreResources" />
                    <div class="col-sm-9">
                        <asp:TextBox ID="txtEmail" runat="server" MaxLength="100"
                            CssClass="forminput verywidetextbox form-control" />
                    </div>
                </div>
                <div class="settingrow form-group row align-items-center">
                    <gb:SiteLabel ID="lblContact" runat="server" ForControl="txtContact"
                        CssClass="settinglabel control-label col-sm-3 mb-0"
                        ConfigKey="StoreContactLabel" ResourceFile="StoreResources" />
                    <div class="col-sm-9">
                        <asp:TextBox ID="txtContact" runat="server" MaxLength="100"
                            CssClass="forminput verywidetextbox form-control" />
                    </div>
                </div>
                <div class="settingrow form-group row align-items-center">
                    <gb:SiteLabel ID="lblAPIDistrictCode" runat="server" ForControl="txtAPIDistrictCode"
                        CssClass="settinglabel control-label col-sm-3 mb-0"
                        ConfigKey="StoreAPIDistrictCodeLabel" ResourceFile="StoreResources" />
                    <div class="col-sm-9">
                        <asp:TextBox ID="txtDistrictCode" runat="server" MaxLength="100"
                            CssClass="forminput verywidetextbox form-control" />
                    </div>
                </div>
                <div class="settingrow form-group row align-items-center">
                    <gb:SiteLabel ID="lblAdmin" runat="server" ForControl="autAdmins"
                        CssClass="settinglabel control-label col-sm-3 mb-0"
                        ConfigKey="StoreAdminLabel" ResourceFile="StoreResources" />
                    <div class="col-sm-9">
                        <telerik:RadAutoCompleteBox ID="autAdmins" TextSettings-SelectionMode="Single" Skin="Simple" runat="server" Width="100%" DropDownWidth="250" DropDownHeight="200"
                            EmptyMessage="Tìm quản trị" OnClientEntryAdding="OnClientEntryAddingHandler">
                            <WebServiceSettings Method="GetUserNames" Path="/Store/AdminCP/StoreEdit.aspx" />
                            <ClientDropDownItemTemplate>
                                        <table cellpadding="0" cellspacing="3" width="100%">
                                            <tr>
                                                <td style="width:70%"><strong>#= Text #</strong></td>
                                            </tr>
                                        </table>
                            </ClientDropDownItemTemplate>
                        </telerik:RadAutoCompleteBox>
                    </div>
                </div>

                <div class="settingrow form-group row align-items-center">
                    <gb:SiteLabel ID="lblManagingArea" runat="server" ForControl="cobAreas"
                        CssClass="settinglabel control-label col-sm-3 mb-0"
                        ConfigKey="StoreManagingAreaLabel" ResourceFile="StoreResources" />
                    <div class="col-sm-9">
                        <portal:ComboBox ID="cobAreas" SelectionMode="Multiple" runat="server" DataTextField="Name" DataValueField="Guid" />
                    </div>
                </div>
                <div class="row col offset-sm-3 settingrow form-group">
                    <asp:CheckBox ID="chkTakeOnlineOrder" runat="server" />
                    <gb:SiteLabel ID="lblTakeOnlineOrder" runat="server"
                        ForControl="chkTakeOnlineOrder" ConfigKey="StoreTakeOnlineOrderLabel"
                        ResourceFile="StoreResources"
                        CssClass="settinglabel control-label mb-0" />
                </div>
                <div class="row col offset-sm-3 settingrow form-group">
                    <asp:CheckBox ID="chkPublic" runat="server" />
                    <gb:SiteLabel ID="lblIsPublic" runat="server"
                        ForControl="chkPublic" ConfigKey="StorePublicLabel"
                        ResourceFile="StoreResources"
                        CssClass="settinglabel control-label mb-0" />
                </div>
                <div class="row col offset-sm-3 settingrow form-group">
                    <asp:CheckBox ID="chkDefault" runat="server" />
                    <gb:SiteLabel ID="lblIsDefault" runat="server"
                        ForControl="chkDefault" ConfigKey="StoreDefaultLabel"
                        ResourceFile="StoreResources"
                        CssClass="settinglabel control-label mb-0" />
                </div>

                <asp:UpdatePanel ID="upLanguage" runat="server">
                    <ContentTemplate>
                        <telerik:RadTabStrip ID="tabLanguage" OnTabClick="tabLanguage_TabClick"
                            EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false"
                            CssClass="subtabs" SkinID="SubTabs" Visible="false" SelectedIndex="0" runat="server" />
                        <div class="settingrow form-group row align-items-center">
                            <gb:SiteLabel ID="lblName" runat="server" ForControl="txtName"
                                CssClass="settinglabel control-label col-sm-3 mb-0"
                                ConfigKey="StoreNameLabel" ResourceFile="StoreResources" />
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtName" runat="server" MaxLength="100"
                                    CssClass="forminput verywidetextbox form-control" />
                                <asp:RequiredFieldValidator ID="reqName" runat="server"
                                    ControlToValidate="txtName" Display="Dynamic" SetFocusOnError="true"
                                    CssClass="txterror" ValidationGroup="Store" />
                            </div>
                        </div>

                        <div class="settingrow form-group row align-items-center">
                            <gb:SiteLabel ID="lblDescription" runat="server" ForControl="edDescription"
                                CssClass="settinglabel control-label col-sm-3 mb-0"
                                ConfigKey="StoreDescriptionLabel" ResourceFile="StoreResources" />
                            <div class="col-sm-9">
                                <div class="input-group">
                                    <gbe:EditorControl ID="edDescription" runat="server"
                                        CssClass="form-control p-0" />
                                    <portal:gbHelpLink ID="GbHelpLinkStore" runat="server"
                                        HelpKey="productedit-briefcontent-help" />
                                </div>
                            </div>
                        </div>
                        <div class="settingrow form-group row align-items-center">
                            <gb:SiteLabel ID="lblAPIEndpoint" runat="server" ForControl="txtAPIEndpoint"
                                CssClass="settinglabel control-label col-sm-3 mb-0"
                                ConfigKey="APIEndpointLabel" ResourceFile="StoreResources" />
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtAPIEndpoint" runat="server" TextMode="MultiLine" Rows="10"
                                    CssClass="forminput verywidetextbox form-control" />
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="settingrow form-group row align-items-center">
                    <gb:SiteLabel ID="lblAddress" runat="server" ForControl="txtAddress"
                        CssClass="settinglabel control-label col-sm-3 mb-0"
                        ConfigKey="StoreAddressLabel" ResourceFile="StoreResources" />
                    <div class="col-sm-9">
                        <asp:TextBox ID="txtAddress" runat="server" MaxLength="255"
                            CssClass="forminput verywidetextbox form-control" />
                    </div>
                </div>
                <asp:UpdatePanel ID="up2" runat="server">
                    <ContentTemplate>
                        <div class="settingrow form-group row align-items-center">
                            <gb:SiteLabel ID="lblStoreProvince" runat="server" ForControl="ddProvince"
                                CssClass="settinglabel control-label col-sm-3 mb-0"
                                ConfigKey="StoreProvinceLabel" ResourceFile="StoreResources" />
                            <div class="col-sm-9">
                                <asp:DropDownList ID="ddProvince" AutoPostBack="true" AppendDataBoundItems="true" DataValueField="Guid" DataTextField="Name" runat="server"
                                    CssClass="form-control">
                                    <asp:ListItem Text="<%$Resources:StoreResources, StoreProvinceDropdown %>" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="settingrow form-group row align-items-center">
                            <gb:SiteLabel ID="lblStoreDistrict" runat="server" ForControl="ddDistrict"
                                CssClass="settinglabel control-label col-sm-3 mb-0"
                                ConfigKey="StoreDistrictLabel" ResourceFile="StoreResources" />
                            <div class="col-sm-9">
                                <asp:DropDownList ID="ddDistrict" AutoPostBack="false" AppendDataBoundItems="true" DataValueField="Guid" DataTextField="Name" runat="server"
                                    CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <%--	</ContentTemplate>
				</asp:UpdatePanel>--%>

                <%--Google Map--%>
                <%--Latitude--%>
                <div class="settingrow form-group row align-items-center">
                    <gb:SiteLabel ID="lblLatitude" runat="server" ForControl="txtLatitude" CssClass="settinglabel control-label col-sm-3 mb-0"
                        ConfigKey="EditPageLatitudeLabel" ResourceFile="DealerResources" />
                    <div class="col-sm-9">
                        <asp:TextBox ID="txtLatitude" runat="server" MaxLength="50" CssClass="forminput verywidetextbox form-control" />
                    </div>
                </div>

                <div class="settingrow form-group row align-items-center">
                    <gb:SiteLabel ID="lblLongitude" runat="server" ForControl="txtLongitude"
                        CssClass="settinglabel control-label col-sm-3 mb-0" ConfigKey="EditPageLongitudeLabel" ResourceFile="DealerResources" />
                    <div class="col-sm-9">
                        <asp:TextBox ID="txtLongitude" runat="server" MaxLength="50" CssClass="forminput verywidetextbox form-control" />
                        <%--<asp:RequiredFieldValidator ID="reqLongitude" runat="server" ControlToValidate="txtLongitude"
                                Display="Dynamic" ErrorMessage="<%$Resources:DealerResources, EditPageLongitudeRequiredWarning %>" CssClass="txterror" ValidationGroup="dealers" />--%>
                    </div>
                </div>

                <div class="settingrow form-group row align-items-center dealerlatitude">
                    <div class="col-sm-9 col-sm-offset-3">
                        <div id="map-canvas" style="width: 100%; height: 400px;"></div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>
        function OnClientEntryAddingHandler(sender, eventArgs) {
        }
    </script>
    <%--Google Map Script--%>
    <asp:Literal ID="litScripts" runat="server" />
    <script type="text/javascript">
        var geocoder = new google.maps.Geocoder();

        function geocodePosition(pos) {
            geocoder.geocode({
                latLng: pos
            }, function (responses) {
                if (responses && responses.length > 0) {
                    updateMarkerAddress(responses[0].formatted_address);
                } else {
                    updateMarkerAddress('');
                }
            });
        }

        function updateMarkerPosition(latLng) {
            document.getElementById('<%= txtLatitude.ClientID %>').value = latLng.lat();
            document.getElementById('<%= txtLongitude.ClientID %>').value = latLng.lng();
        }

        function updateMarkerAddress(str) {
            document.getElementById('<%= txtAddress.ClientID %>').value = str;
        }

        function setupMarker(latitude, longitude) {
            var latLng = new google.maps.LatLng(latitude, longitude);
            var map = new google.maps.Map(document.getElementById('map-canvas'), {
                zoom: 7,
                center: latLng,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            });
            var marker = new google.maps.Marker({
                position: latLng,
                title: 'Địa chỉ mạng lưới',
                map: map,
                draggable: true
            });

            // Update current position info.
            updateMarkerPosition(latLng);

            // Add dragging event listeners.
            google.maps.event.addListener(marker, 'drag', function () {
                updateMarkerPosition(marker.getPosition());
            });

            google.maps.event.addListener(marker, 'dragend', function () {
                geocodePosition(marker.getPosition());
            });
        }

        function GetLocation() {
            var geocoder = new google.maps.Geocoder();
            var address = document.getElementById('<%= txtAddress.ClientID %>').value;
            geocoder.geocode({ 'address': address }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    var latitude = results[0].geometry.location.lat();
                    var longitude = results[0].geometry.location.lng();

                    setupMarker(latitude, longitude);

                } else {
                    alert("Không thể hiển thị bản đồ! Vui lòng kiểm tra địa chỉ đã nhập!")
                }
            });
        };

        function initialize() {
            autocomplete = new google.maps.places.Autocomplete(
            /** @type {HTMLInputElement} */(document.getElementById('<%= txtAddress.ClientID %>')), { types: ['geocode'] });

            // When the user selects an address from the dropdown,
            // populate the address fields in the form.
            google.maps.event.addListener(autocomplete, 'place_changed', function () {
                GetLocation();
            });

            var latitude = document.getElementById('<%= txtLatitude.ClientID %>').value;
            var longitude = document.getElementById('<%= txtLongitude.ClientID %>').value;

            setupMarker(latitude, longitude);
        }

        // Onload handler to fire off the app.
        google.maps.event.addDomListener(window, 'load', initialize);
    </script>
    <%--Google Map Script--%>
</asp:Content>