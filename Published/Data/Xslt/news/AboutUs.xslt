<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="html" indent="yes" />

  <xsl:template match="/ZoneList">
    <div class="section-grey">
      <section class="about-1 section-small">
        <div class="container">
          <div class="about-content-1">
            <div class="row no-gutter equal-height">
              <div class="col-lg-6">
                <div class="content">
                  <h1 class="section-title">
                    <xsl:value-of disable-output-escaping="yes" select="Zone[1]/News/Title"></xsl:value-of>
                    <xsl:value-of disable-output-escaping="yes" select="Zone[1]/News/EditLink"></xsl:value-of>
                  </h1>
                  <div class="article-content">
                    <xsl:value-of disable-output-escaping="yes" select="Zone[1]/News/BriefContent"></xsl:value-of>
                  </div>
                </div>
              </div>
              <div class="col-lg-6">
                <div class="img">
                  <img>
                  <xsl:attribute name="src">
                    <xsl:value-of select="Zone[1]/News/ImageUrl"></xsl:value-of>
                  </xsl:attribute>
                  <xsl:attribute name="alt">
                    <xsl:value-of select="Zone[1]/News/Title"></xsl:value-of>
                  </xsl:attribute>
                  </img>
                </div>
              </div>
            </div>
          </div>
          <div class="about-content-2">
            <div class="content-wrap">
              <div class="row equal-height no-gutter">
                <div class="col-lg-3">
                  <div class="img-wrap">
                    <div class="img">
                      <img>
                      <xsl:attribute name="src">
                        <xsl:value-of select="Zone[2]/News/ImageUrl"></xsl:value-of>
                      </xsl:attribute>
                      <xsl:attribute name="alt">
                        <xsl:value-of select="Zone[2]/News/Title"></xsl:value-of>
                      </xsl:attribute>
                      </img>
                    </div>
                  </div>
                </div>
                <div class="col-lg-9">
                  <div class="content article-content">
                    <xsl:value-of disable-output-escaping="yes" select="Zone[2]/News/BriefContent"></xsl:value-of>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>
      <section class="about-2 section">
        <div class="container">
          <h2 class="section-title text-center">
            <xsl:value-of disable-output-escaping="yes" select="Zone[3]/Title"></xsl:value-of>
          </h2>
          <div class="section-lead text-center font-bold">
            <xsl:value-of disable-output-escaping="yes" select="Zone[3]/Description"></xsl:value-of>
          </div>
          <div class="partner-slider position-relative">
            <div class="swiper-container">
              <div class="swiper-wrapper">
                <xsl:apply-templates select="Zone[3]/News" mode="ZoneNews-3"></xsl:apply-templates>
              </div>
            </div>
            <div class="swiper-navigation">
              <div class="swiper-btn swiper-prev"><span class="lnr lnr-chevron-left"></span></div>
              <div class="swiper-btn swiper-next"><span class="lnr lnr-chevron-right"></span></div>
            </div>
          </div>
        </div>
      </section>
      <section class="about-3 section">
        <div class="container">
          <h2 class="section-title text-center">
            <xsl:value-of disable-output-escaping="yes" select="Zone[4]/Title"></xsl:value-of>
          </h2>
          <div class="ship-place-slider position-relative">
            <div class="swiper-container">
              <div class="swiper-wrapper">
                <xsl:apply-templates select="Zone[4]/News" mode="ZoneNews-4"></xsl:apply-templates>
              </div>
            </div>
            <div class="swiper-navigation">
              <div class="swiper-btn swiper-prev"><span class="lnr lnr-chevron-left"></span></div>
              <div class="swiper-btn swiper-next"><span class="lnr lnr-chevron-right"></span></div>
            </div>
          </div>
        </div>
      </section>
    </div>
  </xsl:template>

  <xsl:template match="News" mode="ZoneNews-3">
    <div class="swiper-slide">
      <div class="img">
        <div class="logo">
          <a>
            <xsl:attribute name="href">
              <xsl:value-of select="SubTitle"></xsl:value-of>
            </xsl:attribute>
            <xsl:attribute name="target">
              <xsl:value-of select="Target"></xsl:value-of>
            </xsl:attribute>
            <xsl:attribute name="title">
              <xsl:value-of select="Title"></xsl:value-of>
            </xsl:attribute>
            <img>
            <xsl:attribute name="src">
              <xsl:value-of select="ImageUrl"></xsl:value-of>
            </xsl:attribute>
            <xsl:attribute name="alt">
              <xsl:value-of select="Title"></xsl:value-of>
            </xsl:attribute>
            </img>
          </a>
        </div>
        <div class="title">
          <a>
            <xsl:attribute name="href">
              <xsl:value-of select="SubTitle"></xsl:value-of>
            </xsl:attribute>
            <xsl:attribute name="target">
              <xsl:value-of select="Target"></xsl:value-of>
            </xsl:attribute>
            <xsl:attribute name="title">
              <xsl:value-of select="Title"></xsl:value-of>
            </xsl:attribute>
            <xsl:value-of select="Title"></xsl:value-of>
          </a>
          <xsl:value-of select="EditLink" disable-output-escaping="yes"></xsl:value-of>
        </div>
      </div>
    </div>
  </xsl:template>

  <xsl:template match="News" mode="ZoneNews-4">
    <div class="swiper-slide">
      <div class="item">
        <div class="img">
          <img>
          <xsl:attribute name="src">
            <xsl:value-of select="ImageUrl"></xsl:value-of>
          </xsl:attribute>
          <xsl:attribute name="alt">
            <xsl:value-of select="Title"></xsl:value-of>
          </xsl:attribute>
          </img>
        </div>
        <div class="content">
          <div class="title">
            <xsl:value-of disable-output-escaping="yes" select="Title"></xsl:value-of>
            <xsl:value-of select="EditLink" disable-output-escaping="yes"></xsl:value-of>
          </div>
          <p class="address">
            <xsl:value-of disable-output-escaping="yes" select="BriefContent"></xsl:value-of>
          </p>
          <p class="phone">
            <xsl:value-of disable-output-escaping="yes" select="SubTitle"></xsl:value-of>
          </p>
        </div>
      </div>
    </div>
  </xsl:template>


</xsl:stylesheet>