<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="html" indent="yes"/>

  <xsl:template match="/">
    <div class="header-util-item language-wrapper">
      <div class="language dropdown">
        <div class="current flex items-center">
          <span class="hidden-desktop">Ngôn ngữ</span>
          <div class="img">
            <img>
              <xsl:attribute name="src">
                <xsl:value-of select="/LanguageList/Language[IsActive='true']/ImageUrl"></xsl:value-of>
              </xsl:attribute>
              <xsl:attribute name="alt">
                <xsl:value-of select="/LanguageList/Language[IsActive='true']/Title"></xsl:value-of>
              </xsl:attribute>
            </img>
          </div>
          <span class="ri-arrow-drop-down-fill"></span>
        </div>
        <div class="dropdown-content">
          <ul>
            <xsl:apply-templates select="/LanguageList/Language[IsActive!='true']"></xsl:apply-templates>
          </ul>
        </div>
      </div>
    </div>
  </xsl:template>

  <xsl:template match="Language">
    <li>
      <!--<xsl:if test="IsActive='true'">
        <xsl:attribute name="class">
          <xsl:text>active</xsl:text>
        </xsl:attribute>
      </xsl:if>-->
      <a>
        <xsl:attribute name="href">
          <xsl:value-of select="Url"></xsl:value-of>
        </xsl:attribute>
        <xsl:attribute name="title">
          <xsl:value-of select="Title"></xsl:value-of>
        </xsl:attribute>
        <xsl:choose>
          <xsl:when test="ImageUrl!=''">
            <img>
              <xsl:attribute name="src">
                <xsl:value-of select="ImageUrl"></xsl:value-of>
              </xsl:attribute>
              <xsl:attribute name="alt">
                <xsl:value-of select="Title"></xsl:value-of>
              </xsl:attribute>
            </img>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="Title"></xsl:value-of>
          </xsl:otherwise>
        </xsl:choose>
      </a>
    </li>
  </xsl:template>
</xsl:stylesheet>