<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
	<xsl:output method="html" indent="yes" />

	<xsl:template match="/ZoneList">
		<xsl:if test="count(Zone[IsActive = 'true']/Zone[IsActive = 'true']/Zone) > 0">

			<div class="product-filter-item product-sidenav mb-2">
				<div class="product-filter-title flex items-center justify-between">
					<strong class="text">
						<xsl:value-of select="Zone/Zone[IsActive = 'true']/Title" />
					</strong>
				</div>
				<div class="product-filter-body" style="display: block">
					<div class="product-filter-group">
						<ul>
							<xsl:apply-templates select="Zone[IsActive = 'true']/Zone[IsActive = 'true']/Zone">
							</xsl:apply-templates>
						</ul>
					</div>
				</div>
			</div>
		</xsl:if>

	</xsl:template>

	<xsl:template match="Zone">
		<li>
			<xsl:if test="IsActive = 'true'">
				<xsl:attribute name="class">
					<xsl:text>active</xsl:text>
				</xsl:attribute>
			</xsl:if>
			<a class="filter-link">
				<xsl:if test="IsActive = 'true'">
					<xsl:attribute name="class">
						<xsl:text>filter-link text-main</xsl:text>
					</xsl:attribute>
				</xsl:if>
				<xsl:attribute name="href">
					<xsl:value-of select="Url" />
				</xsl:attribute>
				<div class="text">
					<xsl:value-of select="Title" />
				</div>
			</a>
			<xsl:if test="count(Zone) > 0">
				<div class="sub-menu pl-1">
					<xsl:apply-templates select="Zone" mode='Child'></xsl:apply-templates>
				</div>
			</xsl:if>
		</li>
	</xsl:template>

	<xsl:template match="Zone" mode='Child'>
		<a class="sub-link mt-1 fz-12 block">
			<xsl:if test="IsActive = 'true'">
				<xsl:attribute name="class">
					<xsl:text>sub-link mt-1 fz-12 block text-main</xsl:text>
				</xsl:attribute>
			</xsl:if>
			<xsl:attribute name="href">
				<xsl:value-of select="Url" />
			</xsl:attribute>
			<div class="text flex items-center">
				<i class="ri-arrow-drop-right-line mr-1 fz-14"></i>
				<xsl:value-of select="Title" />
			</div>
		</a>
	</xsl:template>

</xsl:stylesheet>