<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="html" indent="yes" />

  <xsl:template match="/ZoneList">
    <nav class="support-sidenav">
      <div class="root-title">
        <xsl:value-of disable-output-escaping="yes" select="RootTitle"></xsl:value-of>
      </div>
      <ul>
        <xsl:apply-templates select="Zone"></xsl:apply-templates>
      </ul>
    </nav>
  </xsl:template>

  <xsl:template match="Zone">
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
        <xsl:text> </xsl:text>
      </a>
    </li>
  </xsl:template>

</xsl:stylesheet>