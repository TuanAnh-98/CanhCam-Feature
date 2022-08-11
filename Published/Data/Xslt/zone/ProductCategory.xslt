<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="html" indent="yes" />

  <xsl:template match="/ZoneList">
    <div class="product-category dropdown">
      <div class="product-category-toggle flex items-center">
        <em class="material-icons">menu</em>
        <strong>
          <xsl:value-of disable-output-escaping="yes" select="ModuleTitle"></xsl:value-of>
        </strong>
        <em class="material-icons">arrow_drop_down</em>
      </div>
      <div class="dropdown-content product-category-dropdown">
        <div class="product-category-list">
          <ul>
            <xsl:apply-templates select="Zone/Zone"></xsl:apply-templates>
          </ul>
        </div>
      </div>
    </div>
  </xsl:template>

  <xsl:template match="Zone">
    <li>
      <xsl:choose>
        <xsl:when test="count(Zone) > 0">
          <xsl:attribute name="class">
            <xsl:text disable-output-escaping="yes">has-sub</xsl:text>
          </xsl:attribute>
          <xsl:if test="IsActive='true'">
            <xsl:attribute name="class">
              <xsl:text>has-sub active</xsl:text>
            </xsl:attribute>
          </xsl:if>
        </xsl:when>
        <xsl:otherwise>
          <xsl:if test="IsActive='true'">
            <xsl:attribute name="class">
              <xsl:text>active</xsl:text>
            </xsl:attribute>
          </xsl:if>
        </xsl:otherwise>
      </xsl:choose>
      <a class="product-category-link" href="#">
          <xsl:attribute name="href">
            <xsl:value-of select="Url"></xsl:value-of>
          </xsl:attribute>
          <xsl:attribute name="target">
            <xsl:value-of select="Target"></xsl:value-of>
          </xsl:attribute>
          <xsl:attribute name="title">
            <xsl:value-of select="Title"></xsl:value-of>
          </xsl:attribute>
          <xsl:value-of select="Title"></xsl:value-of>
      </a>
      <xsl:if test="count(Zone) > 0">
        <span class="material-icons toggle">arrow_forward_ios</span>
        <div class="sub-menu">
          <ul>
            <xsl:apply-templates select="Zone" mode="Child"></xsl:apply-templates>
          </ul>
        </div>
        
      </xsl:if>
    </li>
  </xsl:template>

  <xsl:template match="Zone" mode="Child">
    <li>
      <xsl:if test="IsActive='true'">
        <xsl:attribute name="class">
          <xsl:text>active</xsl:text>
        </xsl:attribute>
      </xsl:if>
      
      <a>
        <xsl:attribute name="href">
          <xsl:value-of select="Url"></xsl:value-of>
        </xsl:attribute>
        <xsl:attribute name="target">
          <xsl:value-of select="Target"></xsl:value-of>
        </xsl:attribute>
        <xsl:value-of select="Title" disable-output-escaping="yes"></xsl:value-of>
      </a>
    </li>
  </xsl:template>

</xsl:stylesheet>