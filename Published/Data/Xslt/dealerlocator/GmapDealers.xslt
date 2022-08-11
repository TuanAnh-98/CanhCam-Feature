<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="html" indent="yes" />
  <xsl:template match="/DealerList">

    <input type="hidden" id="hdnDealerLocatorSiteId">
    <xsl:attribute name="value">
      <xsl:value-of select="/DealerList/SiteId"></xsl:value-of>
    </xsl:attribute>
    </input>
    <input type="hidden" id="hdnDealerLocatorLanguageId">
    <xsl:attribute name="value">
      <xsl:value-of select="/DealerList/LanguageId"></xsl:value-of>
    </xsl:attribute>
    </input>
    <input type="hidden" id="hdnDealerLocatorZoneGuid" value="">
    <xsl:attribute name="value">
      <xsl:value-of select="/DealerList/ZoneGuid"></xsl:value-of>
    </xsl:attribute>
    </input>
    <input type="hidden" id="hdfDealerLocatorSelectCountry">
    <xsl:attribute name="value">
      <xsl:value-of select="/DealerList/SelectCountryText"></xsl:value-of>
    </xsl:attribute>
    </input>
    <input type="hidden" id="hdfDealerLocatorSelectProvince">
    <xsl:attribute name="value">
      <xsl:value-of select="/DealerList/SelectProvinceText"></xsl:value-of>
    </xsl:attribute>
    </input>
    <input type="hidden" id="hdfDealerLocatorSelectDistrict">
    <xsl:attribute name="value">
      <xsl:value-of select="/DealerList/SelectDistrictText"></xsl:value-of>
    </xsl:attribute>
    </input>
    <input type="hidden" id="hdfDealerLocatorSelectZone">
    <xsl:attribute name="value">
      <xsl:value-of select="/DealerList/SelectZoneText"></xsl:value-of>
    </xsl:attribute>
    </input>
    <input type="hidden" id="hdfDealerLocatorCulture">
    <xsl:attribute name="value">
      <xsl:value-of select="/DealerList/Culture"></xsl:value-of>
    </xsl:attribute>
    </input>

    <input type="hidden" id="latitude" value="0" />
    <input type="hidden" id="longitude"  value="0"/> 

    <section class="dealer-section section">
      <div class="container " id="add">
        <h1 class="section-title text-blue text-center mb-4">
          <xsl:value-of disable-output-escaping="yes" select="ModuleTitle"></xsl:value-of>
        </h1>
        <div class="row no-gutter">
          <div class="col-lg-8">
            <div class="map" > 
              <input type="hidden" id="distance" name="distance" value="1" />
              <div id="map_canvas" class="map_system">
              </div>
            </div>
          </div>
          <div class="col-lg-4">
            <div class="result-wrapper">
              <div class="filter-map">
                <h2 class="fz-18 font-bold text-uppercase text-blue">cửa hàng gần bạn </h2>
                <div class="form-group">
                  <div class="field form_desc" style="display:none;">
                    <!--<label for="ddlCountry">
                      <xsl:value-of select="/DealerList/CountryText"></xsl:value-of>
                    </label>-->
                    <select class="form-select" id="ddlCountry"></select>
                  </div>
                  <select class="form-select" id="ddlProvince">
                    <option value="">
                      <xsl:value-of select="/DealerList/SelectProvinceText"></xsl:value-of>
                    </option>
                  </select>
                </div>
                <div class="form-group">
                  <select class="form-select" id="ddlDistrict">
                    <option value="">
                      <xsl:value-of select="/DealerList/SelectDistrictText"></xsl:value-of>
                    </option>
                  </select>
                </div>
                <div class="mt-3">
                  <input type="submit" id="edit-submit" class="btn-searchmap">
                  <xsl:attribute name="value">
                    <xsl:value-of select="/DealerList/SearchText"></xsl:value-of>
                  </xsl:attribute>
                  </input>
                </div>
                <div class="hidden">
                  <div class="search-text">
                    <label>
                      <xsl:value-of select="/DealerList/OrText"></xsl:value-of>
                    </label>
                    <input type="text" maxlength="128" name="address" id="address" size="60" value="" class="form-text"
                      autocomplete="off">
                    <xsl:attribute name="placeholder">
                      <xsl:value-of select="/DealerList/EnterLocationText"></xsl:value-of>
                    </xsl:attribute>
                    </input>
                    <input type="submit" id="edit-submit" class="btn btn-large btn-primary">
                    <xsl:attribute name="value">
                      <xsl:value-of select="/DealerList/SearchText"></xsl:value-of>
                    </xsl:attribute>
                    </input>
                  </div>
                </div>

                <div id="direction" class="find_address" style="display:none;">
                  <div class="row mrb10">
                    <div class="col-sm-2">
                      <strong>
                        <xsl:value-of select="/DealerList/FromText"></xsl:value-of>
                      </strong>
                    </div>
                    <div class="col-sm-10">
                      <input id="origin-direction" name="origin-direction" class="orides-txt" type="text" />
                    </div>
                  </div>
                  <div class="row mrb20">
                    <div class="col-sm-2">
                      <strong>
                        <xsl:value-of select="/DealerList/ToText"></xsl:value-of>
                      </strong>
                    </div>
                    <div class="col-sm-10">
                      <input id="dest-direction" name="dest-direction" class="orides-txt" type="text" />
                    </div>
                  </div>
                  <div id="get-dir-button" class="get-dir-button">
                    <input type="submit" id="get-direction" class="btn">
                    <xsl:attribute name="value">
                      <xsl:value-of select="/DealerList/FindText"></xsl:value-of>
                    </xsl:attribute>
                    </input>
                    <a href="javascript:directionBack()">
                      <xsl:value-of select="/DealerList/BackText"></xsl:value-of>
                    </a>
                  </div>
                </div>
              </div>
              <div class="result-list">
                <div id="ajax_msg" class="locate mrb15 hidden">
                </div>
                <ul id="list"></ul>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  </xsl:template>
</xsl:stylesheet>