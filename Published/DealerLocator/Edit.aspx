<%@ Page Language="c#" CodeBehind="Edit.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master"
    AutoEventWireup="false" Inherits="CanhCam.Web.DealerUI.EditPage" %>

<%@ Register TagPrefix="Site" Assembly="CanhCam.Features.DealerLocator" Namespace="CanhCam.Web.DealerUI" %>
<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <Site:DealerDisplaySettings ID="displaySettings" runat="server" />
    <portal:BreadcrumbAdmin ID="breadcrumb" runat="server" 
        CurrentPageTitle="<%$Resources:DealerResources, EditPageLabel %>" CurrentPageUrl="~/DealerLocator/Edit.aspx" />
    <div class="admin-content col-md-12">
        <asp:UpdatePanel ID="upButton" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <portal:HeadingPanel ID="heading" runat="server">
                    <asp:Button SkinID="UpdateButton" ID="btnUpdate" Text="<%$Resources:Resource, UpdateButton %>" ValidationGroup="dealers" runat="server" />
                    <asp:Button SkinID="UpdateButton" ID="btnUpdateAndNew" Text="<%$Resources:Resource, UpdateAndNewButton %>" ValidationGroup="dealers" runat="server" />
                    <asp:Button SkinID="UpdateButton" ID="btnUpdateAndClose" Text="<%$Resources:Resource, UpdateAndCloseButton %>" ValidationGroup="dealers" runat="server" />
                    <asp:Button SkinID="InsertButton" ID="btnInsert" Text="<%$Resources:Resource, InsertButton %>" ValidationGroup="dealers" runat="server" />
                    <asp:Button SkinID="InsertButton" ID="btnInsertAndNew" Text="<%$Resources:Resource, InsertAndNewButton %>" ValidationGroup="dealers" runat="server" />
                    <asp:Button SkinID="InsertButton" ID="btnInsertAndClose" Text="<%$Resources:Resource, InsertAndCloseButton %>" ValidationGroup="dealers" runat="server" />
                    <asp:Button SkinID="DeleteButton" ID="btnDelete" runat="server" Text="<%$Resources:Resource, DeleteButton %>" CausesValidation="false" />
                    <asp:Button SkinID="DeleteButton" ID="btnDeleteLanguage" Visible="false" OnClick="btnDeleteLanguage_Click" Text="<%$Resources:Resource, DeleteLanguageButton %>" runat="server" CausesValidation="false" />
                </portal:HeadingPanel>
                <portal:NotifyMessage ID="message" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="workplace form-horizontal">
            <div class="settingrow form-group">
                <gb:SiteLabel ID="lblZones" runat="server" ConfigKey="ZoneLabel"
                    ForControl="ddZones" CssClass="settinglabel control-label col-sm-3" />
                <div class="col-sm-9">
                    <portal:ComboBox ID="cobZones" SelectionMode="Multiple" runat="server" />
                </div>
            </div>
            <asp:UpdatePanel ID="upGeoZone" runat="server">
                <ContentTemplate>
                    <div class="settingrow form-group">
                        <gb:SiteLabel ID="lblCountry" runat="server" ForControl="ddCountry" CssClass="settinglabel control-label col-sm-3"
                            ConfigKey="CountryLabel" ResourceFile="DealerResources" />
                        <div class="col-sm-9">
                            <asp:DropDownList ID="ddCountry" runat="server" DataTextField="Name" DataValueField="Guid"
                                AutoPostBack="true" />
                        </div>
                    </div>
                    <div class="settingrow form-group">
                        <gb:SiteLabel ID="lblProvince" runat="server" ForControl="ddProvince" CssClass="settinglabel control-label col-sm-3"
                            ConfigKey="ProvinceLabel" ResourceFile="DealerResources" />
                        <div class="col-sm-9">
                            <asp:DropDownList ID="ddProvince" runat="server" DataTextField="Name" DataValueField="Guid"
                                AutoPostBack="true" />
                        </div>
                    </div>
                    <div id="divDistrict" runat="server" class="settingrow form-group">
                        <gb:SiteLabel ID="lblDistrict" runat="server" ForControl="ddDistrict" CssClass="settinglabel control-label col-sm-3"
                            ConfigKey="DistrictLabel" ResourceFile="DealerResources" />
                        <div class="col-sm-9">
                            <asp:DropDownList ID="ddDistrict" runat="server" DataTextField="Name" DataValueField="Guid" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="settingrow form-group">
                <gb:SiteLabel ID="lblPhone" runat="server" ForControl="txtPhone" CssClass="settinglabel control-label col-sm-3"
                    ConfigKey="EditPagePhoneLabel" ResourceFile="DealerResources" />
                <div class="col-sm-9">
                    <asp:TextBox ID="txtPhone" runat="server" MaxLength="255" />
                </div>
            </div>
            <div id="divFax" runat="server" class="settingrow form-group dealerfax">
                <gb:SiteLabel ID="lblFax" runat="server" ForControl="txtFax" CssClass="settinglabel control-label col-sm-3"
                    ConfigKey="EditPageFaxLabel" ResourceFile="DealerResources" />
                <div class="col-sm-9">
                    <asp:TextBox ID="txtFax" runat="server" MaxLength="255" />
                </div>
            </div>
            <div id="divEmail" runat="server" class="settingrow form-group dealeremail">
                <gb:SiteLabel ID="lblEmail" runat="server" ForControl="txtEmail" CssClass="settinglabel control-label col-sm-3"
                    ConfigKey="EditPageEmailLabel" ResourceFile="DealerResources" />
                <div class="col-sm-9">
                    <asp:TextBox ID="txtEmail" runat="server" MaxLength="255" />
                </div>
            </div>
            <div id="divWebsite" runat="server" class="settingrow form-group dealerwebsite">
                <gb:SiteLabel ID="lblWebsite" runat="server" ForControl="txtWebsite" CssClass="settinglabel control-label col-sm-3"
                    ConfigKey="EditPageWebsiteLabel" ResourceFile="DealerResources" />
                <div class="col-sm-9">
                    <asp:TextBox ID="txtWebsite" runat="server" MaxLength="255" />
                </div>
            </div>
            <div id="divContactPerson" runat="server" class="settingrow form-group dealercontactpersion">
                <gb:SiteLabel ID="lblContactPerson" runat="server" ForControl="txtContactPerson" CssClass="settinglabel control-label col-sm-3"
                    ConfigKey="EditPageContactPersonLabel" ResourceFile="DealerResources" />
                <div class="col-sm-9">
                    <asp:TextBox ID="txtContactPerson" runat="server" MaxLength="255" />
                </div>
            </div>
            <div id="divPicture" runat="server" class="settingrow form-group dealerpicture">
                <gb:SiteLabel ID="lblPicture" runat="server" ForControl="txtPicture" ResourceFile="DealerResources"
                    ConfigKey="EditPagePictureLabel" CssClass="settinglabel control-label col-sm-3" />
                <div class="col-sm-9">
                    <div class="input-group">
                        <asp:TextBox ID="txtPicture" MaxLength="255" runat="server" />
                        <div class="input-group-addon">
                            <portal:FileBrowserTextBoxExtender ID="PictureBrowser" runat="server" BrowserType="image" />
                        </div>
                    </div>
                </div>
            </div>
            <asp:UpdatePanel ID="up" runat="server">
                <ContentTemplate>
                    <telerik:RadTabStrip ID="tabLanguage" OnTabClick="tabLanguage_TabClick" 
                        EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" 
                        CssClass="subtabs" SkinID="SubTabs" Visible="false" SelectedIndex="0" runat="server" />
                    <div class="settingrow form-group">
                        <gb:SiteLabel ID="lblName" runat="server" ForControl="txtName" CssClass="settinglabel control-label col-sm-3"
                            ConfigKey="EditPageNameLabel" ResourceFile="DealerResources" />
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtName" runat="server" MaxLength="255" />
                            <asp:RequiredFieldValidator ID="reqName" runat="server" ControlToValidate="txtName"
                                Display="Dynamic" ErrorMessage="<%$Resources:DealerResources, EditPageNameRequiredWarning %>" CssClass="txterror" ValidationGroup="dealers" />
                        </div>
                    </div>
                    <div class="settingrow form-group">
                        <gb:SiteLabel ID="lblAddress" runat="server" ForControl="txtAddress" CssClass="settinglabel control-label col-sm-3"
                            ConfigKey="EditPageAddressLabel" ResourceFile="DealerResources" />
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtAddress" runat="server" MaxLength="255" />
                        </div>
                    </div>
                    <div id="divMap" runat="server" visible="false" class="settingrow form-group dealermap">
                        <gb:SiteLabel ID="lblMap" runat="server" ForControl="txtMap" CssClass="settinglabel control-label col-sm-3"
                            ConfigKey="EditPageMapLabel" ResourceFile="DealerResources" />
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtMap" runat="server" CssClass="form-control" TextMode="MultiLine" />
                        </div>
                    </div>
                    <div id="divDescription" runat="server" class="settingrow form-group dealerdes">
                        <gb:SiteLabel ID="lblDescription" runat="server" ForControl="txtDescription" CssClass="settinglabel control-label col-sm-3"
                            ConfigKey="EditPageDescriptionLabel" ResourceFile="DealerResources" />
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtDescription" CssClass="form-control" runat="server" TextMode="MultiLine" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="settingrow form-group dealerlatitude">
                <gb:SiteLabel ID="lblLatitude" runat="server" ForControl="txtWebsite" CssClass="settinglabel control-label col-sm-3"
                    ConfigKey="EditPageLatitudeLabel" ResourceFile="DealerResources" />
                <div class="col-sm-9">
                    <asp:TextBox ID="txtLatitude" runat="server" Text="10.8230989" MaxLength="50" />
                    <%--<asp:RequiredFieldValidator ID="reqLatitude" runat="server" ControlToValidate="txtLatitude"
                        Display="Dynamic" ErrorMessage="<%$Resources:DealerResources, EditPageLatitudeRequiredWarning %>" CssClass="txterror" ValidationGroup="dealers" />--%>
                </div>
            </div>
            <div class="settingrow form-group dealerlongitude">
                <gb:SiteLabel ID="lblLongitude" runat="server" ForControl="txtLongitude"
                    CssClass="settinglabel control-label col-sm-3" ConfigKey="EditPageLongitudeLabel" ResourceFile="DealerResources" />
                <div class="col-sm-9">
                    <asp:TextBox ID="txtLongitude" runat="server" Text="106.6296638" MaxLength="50" />
                    <%--<asp:RequiredFieldValidator ID="reqLongitude" runat="server" ControlToValidate="txtLongitude"
                        Display="Dynamic" ErrorMessage="<%$Resources:DealerResources, EditPageLongitudeRequiredWarning %>" CssClass="txterror" ValidationGroup="dealers" />--%>
                </div>
            </div>
            <div class="settingrow form-group dealerlatitude">
                <div class="col-sm-9 col-sm-offset-3">
                    <div id="map-canvas" style="width: 100%; height: 400px;"></div>
                </div>
            </div>
        </div>
        
        <portal:SessionKeepAliveControl ID="ka1" runat="server" />
    </div>

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
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />