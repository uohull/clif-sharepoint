<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
  exclude-result-prefixes="x d xsl msxsl cmswrt"
  xmlns:x="http://www.w3.org/2001/XMLSchema"
  xmlns:d="http://schemas.microsoft.com/sharepoint/dsp"
  xmlns:cmswrt="http://schemas.microsoft.com/WebParts/v3/Publishing/runtime"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt">

  <xsl:output method="html" indent="yes"/>

  <xsl:template match="/">
    <ul id="nav">
      <li class="KclNavLC"></li>
      <xsl:apply-templates select="/Items/Item/Item">
        <xsl:with-param name="navLev">1</xsl:with-param>
      </xsl:apply-templates>
      <li class="KclNavRC"></li>
    </ul>
  </xsl:template>

  <xsl:template match="Item">
    <xsl:param name="navLev"/>
    <xsl:variable name="CurrentItem">
      <xsl:choose>
        <xsl:when test="@Selected='True' or (@Current='True' and not(following-sibling::Item[@Selected='True']))">1</xsl:when>
        <xsl:otherwise>0</xsl:otherwise>
      </xsl:choose>
      
    </xsl:variable>
    <xsl:if test="$CurrentItem='1'"><li class="KclNavSelLC"></li></xsl:if>
    <li>
      <xsl:choose>
        <xsl:when test="$CurrentItem='1'">
          <xsl:attribute name="class">selected</xsl:attribute>
        </xsl:when>
      </xsl:choose>
      <a href="{@Url}">
        <xsl:if test="Item and not(parent::Items) and not(parent::Item/parent::Items) ">
          <xsl:attribute name="class">parent</xsl:attribute>
        </xsl:if>
        <xsl:value-of disable-output-escaping="yes" select="@Title"/>
      </a>
      <!-- KCL initially single level navigation only 
      <xsl:if test="Item and not(parent::Items)">
        <ul>
          <xsl:apply-templates select="Item">
            <xsl:with-param name="navLev">
              <xsl:value-of select="number($navLev) + number(1)"/>
            </xsl:with-param>
          </xsl:apply-templates>
        </ul>
      </xsl:if>
      -->
    </li>
    <xsl:if test="$CurrentItem='1'"><li class="KclNavSelRC"></li></xsl:if>
    <xsl:if test="following-sibling::Item and not($CurrentItem='1') and not(following-sibling::Item[1][@Current='True' or @Selected='True'])">
      <li class="KclNavSep"></li>
    </xsl:if>
  </xsl:template>

</xsl:stylesheet>
