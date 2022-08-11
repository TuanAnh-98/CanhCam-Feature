<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="html" indent="yes"/>

  <xsl:template match="/">
    <div class="menu-wrapper">
      <div class="menu flex items-center">
        <div class="icon-menu mr-1">
          <img src="/Data/Sites/1/skins/default/img/top-menu-icon.png" alt="Menu" />
        </div>
        <ul class="flex items-center">
          <xsl:apply-templates select="/ZoneList/Zone"></xsl:apply-templates>
        </ul>
      </div>
    </div>
  </xsl:template>

  <xsl:template match="Zone">
    <li>
      <a>
        <xsl:attribute name="href">
          <xsl:value-of select="Url"></xsl:value-of>
        </xsl:attribute>
        <xsl:attribute name="target">
          <xsl:value-of select="Target"></xsl:value-of>
        </xsl:attribute>
        <xsl:choose>
          <xsl:when test="position()=1">
            <em class="ri-price-tag-3-line"></em>
          </xsl:when>
          <xsl:when test="position()=2">
            <em class="ri-edit-line"></em>
          </xsl:when>
          <xsl:when test="position()=3">
            <em class="ri-file-list-3-line"></em>
          </xsl:when>
          <xsl:when test="position()=4">
            <em class="ri-customer-service-2-line"></em>
          </xsl:when>
          <xsl:when test="position()=5">
            <em class="ri-map-pin-line"></em>
          </xsl:when>
          <xsl:when test="position()=6">
            <em class="ri-chat-smile-line"></em>
          </xsl:when>
          <xsl:when test="position()=7">
            <em class="ri-community-line"></em>
          </xsl:when>
          <xsl:when test="position()=8">
            <em class="ri-service-line"></em>
          </xsl:when>
        </xsl:choose>
        <xsl:value-of select="Title" disable-output-escaping="yes"></xsl:value-of>
      </a>
    </li>
  </xsl:template>
  
</xsl:stylesheet>