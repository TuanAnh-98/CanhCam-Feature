<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="html" indent="yes" />

  <xsl:template match="/NewsDetail">
    <div class="news-detail-wrap section-small section-grey">
      <div class="container">
        <div class="row justify-center no-gutter">
          <div class="col-lg-10">
            <div class="news-detail">
              <h1 class="news-detail-title">
                <xsl:value-of disable-output-escaping="yes" select="Title"></xsl:value-of>
                <xsl:value-of select="EditLink" disable-output-escaping="yes"></xsl:value-of>
              </h1>
              <div class="news-detail-meta">
                <div class="date">
                  <xsl:value-of disable-output-escaping="yes" select="CreatedDate"></xsl:value-of>
                </div>
                <div class="fb-share">
                  <div class="fb-like" data-href="https://developers.facebook.com/docs/plugins/" data-width="" data-layout="button_count" data-action="like" data-size="small" data-share="true">
                    <xsl:attribute name="data-href">
                      <xsl:value-of disable-output-escaping="yes" select="FullUrl"></xsl:value-of>
                    </xsl:attribute>
                  </div>
                </div>
              </div>
              <div class="news-detail-content article-content">
                <xsl:value-of disable-output-escaping="yes" select="FullContent"></xsl:value-of>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <xsl:if test="count(NewsOther)>0">
      <div class="news-detail-other">
        <div class="container">
          <h2 class="section-title text-center">Tin tức khác</h2>
          <div class="news-slider position-relative">
            <div class="swiper-container">
              <div class="swiper-wrapper">
                <xsl:apply-templates select="NewsOther"></xsl:apply-templates>
              </div>
            </div>
            <div class="swiper-navigation">
              <div class="swiper-btn swiper-prev"><span class="lnr lnr-chevron-left"></span></div>
              <div class="swiper-btn swiper-next"><span class="lnr lnr-chevron-right"></span></div>
            </div>
          </div>
        </div>
      </div>
    </xsl:if>
  </xsl:template>

  <xsl:template match="NewsOther">
    <div class="swiper-slide">
      <a class="news-item news-item-small" href="#">
        <xsl:attribute name="href">
          <xsl:value-of select="Url"></xsl:value-of>
        </xsl:attribute>
        <xsl:attribute name="target">
          <xsl:value-of select="Target"></xsl:value-of>
        </xsl:attribute>
        <xsl:attribute name="title">
          <xsl:value-of select="Title"></xsl:value-of>
        </xsl:attribute>
        <div class="news-img object-fit-img">
          <img>
          <xsl:attribute name="src">
            <xsl:value-of select="ImageUrl"></xsl:value-of>
          </xsl:attribute>
          <xsl:attribute name="alt">
            <xsl:value-of select="Title"></xsl:value-of>
          </xsl:attribute>
          </img>
        </div>
        <div class="news-caption">
          <div class="title">
            <xsl:value-of select="Title"></xsl:value-of>
          </div>
        </div>
      </a>
    </div>
  </xsl:template>


</xsl:stylesheet>