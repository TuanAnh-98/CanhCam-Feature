<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="html" indent="yes"/>

  <xsl:template match="/">
    <aside class="main-sidebar sidebar-dark-primary elevation-4">
      <a class="brand-link clearfix">
        <xsl:attribute name="href">
          <xsl:value-of select="/MenuList/DashboardUrl"></xsl:value-of>
        </xsl:attribute>
        <img src="/App_Themes/adminskin/img/logo.png" alt="Cánh Cam Logo" class="brand-image elevation-3" />
        <span class="brand-text font-weight-light"></span>
      </a>
      <div class="sidebar">
        <nav class="mt-2">
          <ul class="nav nav-pills nav-sidebar flex-column" data-widget="treeview" role="menu" data-accordion="false">
            <xsl:apply-templates select="/MenuList/Menu"></xsl:apply-templates>
          </ul>
        </nav>
      </div>
    </aside>
  </xsl:template>

  <xsl:template match="Menu">
    <li>
      <xsl:attribute name="class">
        <xsl:text>nav-item</xsl:text>
        <xsl:if test="count(Menu)>0">
          <xsl:text> has-treeview</xsl:text>
        </xsl:if>
        <xsl:if test="IsActive='true'">
            <xsl:text> menu-open</xsl:text>
        </xsl:if>
      </xsl:attribute>
      <a href="#" class="nav-link">
        <xsl:if test="Url != ''">
          <xsl:attribute name="href">
            <xsl:value-of select="Url"></xsl:value-of>
          </xsl:attribute>
        </xsl:if>
        <xsl:if test="IsActive='true'">
          <xsl:attribute name="class">
            <xsl:text>nav-link active</xsl:text>
          </xsl:attribute>
        </xsl:if>
        <xsl:if test="CssClass!=''">
          <i>
            <xsl:attribute name="class">
              <xsl:value-of select="CssClass"></xsl:value-of>
            </xsl:attribute>
          </i>
        </xsl:if>
        <p>
          <xsl:value-of select="Title" disable-output-escaping="yes"></xsl:value-of>
          <i class="right fas fa-angle-left"></i>
        </p>
      </a>
      <xsl:if test="count(Menu)>0">
        <ul class="nav nav-treeview">
          <xsl:apply-templates select="Menu" mode="Sub"></xsl:apply-templates>
        </ul>
      </xsl:if>
    </li>
  </xsl:template>
  <xsl:template match="Menu" mode="Sub">
    <li class="nav-item">
      <a href="#" class="nav-link">
        <xsl:if test="Url != ''">
          <xsl:attribute name="href">
            <xsl:value-of select="Url"></xsl:value-of>
          </xsl:attribute>
        </xsl:if>
        <xsl:if test="IsActive='true'">
          <xsl:attribute name="class">
            <xsl:text>nav-link active</xsl:text>
          </xsl:attribute>
        </xsl:if>
        <!--<i class="far fa-circle nav-icon"></i>-->
        <p>
          <xsl:value-of select="Title" disable-output-escaping="yes"></xsl:value-of>
        </p>
      </a>
    </li>
  </xsl:template>
</xsl:stylesheet>