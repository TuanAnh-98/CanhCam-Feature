<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="html" indent="yes" />

  <xsl:template match="/">
    <xsl:if test="count(/ZoneList/Zone) > 0">
      <div class="container">
        <section class="product-subnav-wrap">
          <div class="product-subnav-slider position-relative">
            <div class="swiper-container">
              <div class="swiper-wrapper">
                <xsl:apply-templates select="/ZoneList/Zone"></xsl:apply-templates>
              </div>
            </div>
            <div class="swiper-navigation">
              <div class="swiper-btn swiper-btn-prev">
                <span class="ri-arrow-left-line"></span>
              </div>
              <div class="swiper-btn swiper-btn-next">
                <span class="ri-arrow-right-line"></span>
              </div>
            </div>
          </div>
        </section>
      </div>
    </xsl:if>
  </xsl:template>

  <xsl:template match="Zone">
    <div class="swiper-slide">
      <a class="product-subnav-item">
        <xsl:attribute name="href">
          <xsl:value-of select="Url"></xsl:value-of>
        </xsl:attribute>
        <xsl:attribute name="target">
          <xsl:value-of select="Target"></xsl:value-of>
        </xsl:attribute>
        <div class="img object-fit-img">
          <xsl:if test="ImageUrl != ''">
            <img src="data:image/gif;base64,R0lGODlhAQABAAD/ACwAAAAAAQABAAACADs=" class="swiper-lazy">
            <xsl:attribute name="data-src">
              <xsl:value-of select="ImageUrl"></xsl:value-of>
            </xsl:attribute>
            <xsl:attribute name="title">
              <xsl:value-of select="Title"></xsl:value-of>
            </xsl:attribute>
            </img>
          </xsl:if>
        </div>
        <div class="text">
          <xsl:value-of select="Title" disable-output-escaping="yes"></xsl:value-of>
        </div>
      </a>
    </div>
  </xsl:template>

  <xsl:template match="Zone" mode="Child">

  </xsl:template>

</xsl:stylesheet>