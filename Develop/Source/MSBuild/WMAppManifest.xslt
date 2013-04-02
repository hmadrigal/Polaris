<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:windowsphone="http://schemas.microsoft.com/windowsphone/2009/deployment"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                exclude-result-prefixes="msxsl">

  <xsl:output method="xml" indent="yes" omit-xml-declaration="no" encoding="utf-8"/>

  <xsl:param name="AssemblyVersion"></xsl:param>

  <xsl:template match="/windowsphone:Deployment/windowsphone:App/@windowsphone:version">
    <xsl:attribute name="version">
      <xsl:value-of select="$AssemblyVersion" />
    </xsl:attribute>
  </xsl:template>

  <!-- Copy all the remaining nodes -->
  <xsl:template match="node()|@*">
    <xsl:copy>
      <xsl:apply-templates select="node()|@*"/>
    </xsl:copy>
  </xsl:template>

</xsl:stylesheet>