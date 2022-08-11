<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="html" indent="yes"/>

  <xsl:template match="/">
    <xsl:if test="count(/MenuList/Menu)>0">
      <xsl:if test="/MenuList/LastPageVisitedUrl!=''">
        <li class="nav-item">
          <a class="viewwebsite nav-link">
            <xsl:attribute name="href">
              <xsl:value-of select="/MenuList/LastPageVisitedUrl"></xsl:value-of>
            </xsl:attribute>
            <i class="fa fa-desktop"></i>
            <xsl:text> </xsl:text>
            <xsl:value-of select="/MenuList/ViewWebsite"></xsl:value-of>
          </a>
        </li>
      </xsl:if>
      <xsl:apply-templates select="/MenuList/Menu"></xsl:apply-templates>
    </xsl:if>
  </xsl:template>

  <xsl:template match="Menu">
    <li>
      <xsl:attribute name="class">
        <xsl:text>nav-item</xsl:text>
        <xsl:if test="count(Menu)>0">
          <xsl:text> dropdown</xsl:text>
        </xsl:if>
        <xsl:if test="IsActive='true'">
          <xsl:text> active</xsl:text>
        </xsl:if>
      </xsl:attribute>
      <a href="#" class="nav-link">
        <xsl:if test="Url != ''">
          <xsl:attribute name="href">
            <xsl:value-of select="Url"></xsl:value-of>
          </xsl:attribute>
        </xsl:if>
        <xsl:if test="count(Menu)>0">
          <xsl:attribute name="data-toggle">
            <xsl:text>dropdown</xsl:text>
          </xsl:attribute>
        </xsl:if>
        <xsl:if test="CssClass!=''">
          <i>
            <xsl:attribute name="class">
              <xsl:value-of select="CssClass"></xsl:value-of>
            </xsl:attribute>
          </i>
          <xsl:text> </xsl:text>
        </xsl:if>
        <xsl:value-of select="Title" disable-output-escaping="yes"></xsl:value-of>
      </a>
      <xsl:if test="count(Menu)>0">
        <div class="dropdown-menu dropdown-menu-lg dropdown-menu-right">
          <ul class="nav nav-pills">
            <xsl:apply-templates select="Menu" mode="Sub"></xsl:apply-templates>
          </ul>
        </div>
      </xsl:if>
    </li>
  </xsl:template>

  <xsl:template match="Menu" mode="Sub">
    <li>
      <xsl:attribute name="class">
        <xsl:text>nav-item</xsl:text>
        <xsl:if test="count(Menu)>0">
          <xsl:text> dropdown</xsl:text>
        </xsl:if>
        <xsl:if test="IsActive='true'">
          <xsl:text> active</xsl:text>
        </xsl:if>
      </xsl:attribute>
      <a href="#" class="nav-link">
        <xsl:if test="Url != ''">
          <xsl:attribute name="href">
            <xsl:value-of select="Url"></xsl:value-of>
          </xsl:attribute>
        </xsl:if>
        <xsl:if test="count(Menu)>0">
          <xsl:attribute name="data-toggle">
            <xsl:text>dropdown</xsl:text>
          </xsl:attribute>
        </xsl:if>
        <xsl:if test="CssClass!=''">
          <i>
            <xsl:attribute name="class">
              <xsl:value-of select="CssClass"></xsl:value-of>
            </xsl:attribute>
          </i>
          <xsl:text> </xsl:text>
        </xsl:if>
        <xsl:value-of select="Title" disable-output-escaping="yes"></xsl:value-of>
      </a>
      <xsl:if test="count(Menu)>0">
        <ul class="dropdown-menu" role="menu">
          <xsl:apply-templates select="Menu" mode="Sub"></xsl:apply-templates>
        </ul>
      </xsl:if>
    </li>
  </xsl:template>
  
</xsl:stylesheet>