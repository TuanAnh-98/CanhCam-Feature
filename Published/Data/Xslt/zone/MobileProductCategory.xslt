<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
	<xsl:output method="html" indent="yes" />

	<xsl:template match="/ZoneList">
		<section class="section mobile-product-category">
			<div class="container">
				<div class="row">
					<xsl:apply-templates select="Zone"></xsl:apply-templates>
				</div>
			</div>
		</section>
	</xsl:template>

	<xsl:template match="Zone">
		<div class="col-sm-4 col-3">
			<a class="product-category-link flex items-center justify-between" href="#">
				<xsl:attribute name="href">
					<xsl:value-of select="Url"></xsl:value-of>
				</xsl:attribute>
				<xsl:attribute name="title">
					<xsl:value-of select="Title"></xsl:value-of>
				</xsl:attribute>
				<div class="text flex items-center">
					<div class="icon">
						<img class="lazyload" src="data:image/gif;base64,R0lGODlhAQABAAD/ACwAAAAAAQABAAACADs=">
						<xsl:attribute name="data-src">
							<xsl:value-of select="SecondImageUrl"></xsl:value-of>
						</xsl:attribute>
						<xsl:attribute name="alt">
							<xsl:value-of select="Title"></xsl:value-of>
						</xsl:attribute>
						</img>
					</div>
					<span>
						<xsl:value-of select="Title"></xsl:value-of>
					</span>
				</div>
			</a>
		</div>
	</xsl:template>

</xsl:stylesheet>