<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="html" indent="yes"/>

  <xsl:template match="/">
    <xsl:if test="count(/ZoneList/Zone) > 0">
      <div class="news-subnav-wrap">
        <div class="news-subnav">
          <div class="swiper-container">
            <div class="swiper-wrapper">
              <xsl:apply-templates select="/ZoneList/Zone"></xsl:apply-templates>
            </div>
          </div>
        </div>
        <div class="news-subnav-control flex hidden-desktop">
          <div class="btn-slide btn swiper-btn-prev">
            <em class="ri-arrow-left-s-line"></em>
          </div>
          <div class="btn-slide btn swiper-btn-next">
            <em class="ri-arrow-right-s-line"></em>
          </div>
        </div>
      </div>
    </xsl:if>
  </xsl:template>

  <xsl:template match="Zone">
    <div class="swiper-slide">
      <a class="subnav-link" href="#">
        <xsl:if test="IsActive='true'">
          <xsl:attribute name="class">
            <xsl:text>subnav-link active</xsl:text>
          </xsl:attribute>
        </xsl:if>
        <xsl:attribute name="href">
          <xsl:value-of select="Url"></xsl:value-of>
        </xsl:attribute>
        <xsl:attribute name="target">
          <xsl:value-of select="Target"></xsl:value-of>
        </xsl:attribute>
        <xsl:value-of select="Title" disable-output-escaping="yes"></xsl:value-of>
      </a>
    </div>
  </xsl:template>

</xsl:stylesheet>