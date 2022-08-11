<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="html" indent="yes"/>

  <xsl:template match="/">

    <div class="footer-item footer-nav">
      <div class="footer-title">
        <xsl:value-of select="/ZoneList/ModuleTitle"></xsl:value-of>
      </div>
      <ul>
        <xsl:apply-templates select="/ZoneList/Zone"></xsl:apply-templates>
      </ul>
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
        <xsl:value-of select="Title" disable-output-escaping="yes"></xsl:value-of>
        <xsl:text> </xsl:text>
      </a>
    </li>
  </xsl:template>

</xsl:stylesheet>