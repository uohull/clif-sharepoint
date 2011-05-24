<?xml version="1.0" encoding="utf-8"?>
<!-- XSL stylesheet for left navigation         -->
<!-- Version 2.0, 03/09/2009 - S. Nicholson     -->
<!-- - amended to handle showing the top level  -->
<!--   item as selected properly                -->

<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:str="http://exslt.org/strings"
                xmlns:date="http://exslt.org/dates-and-times"
                extension-element-prefixes="str date"
                exclude-result-prefixes="xsl str date">

  <xsl:output method="xml" omit-xml-declaration="yes" indent="yes" />

  <xsl:template match="/">
    <br></br>
    <xsl:variable name="absolutePath" select="Navigation/@AbsolutePath" />

    <xsl:if test="count(Navigation/Item) &gt; 0">
      <div class="Nav-y">
        <div class="Top">
          <xsl:call-template name="space" />
        </div>
        <div class="Body">
          <xsl:apply-templates select="Navigation/Item" mode="topLevel" />
        </div>
        <div class="Bottom">
          <xsl:call-template name="space" />
        </div>
      </div>
    </xsl:if>
  </xsl:template>

  <!-- this template handles the children and grandchildren etc of the top level current item -->
  <xsl:template match="Item">
    <xsl:param name="level" />
    <xsl:param name="sectionTitle" />

    <li>
      <xsl:if test="@Selected='True' or @Current='True'">
        <xsl:attribute name="class">Selected</xsl:attribute>
      </xsl:if>
      <a href="{@Url}" title="{@Title}">
        <xsl:value-of select="@Title" disable-output-escaping="yes"/>
      </a>
      <xsl:if test="count(Item) &gt; 0 and @Current = 'True'">
        <ul>
          <xsl:apply-templates select="Item">
            <xsl:with-param name="level" select="$level + 1" />
            <xsl:with-param select="@Title" name="sectionTitle" />
          </xsl:apply-templates>
        </ul>
      </xsl:if>
    </li>
  </xsl:template>
  <!-- This template handles the top level current item in the menu -->
  <xsl:template match="Item" mode="topLevel">

    <ul>
      <li>
        <!-- the top level site doesn't get set to selected by SharePoint, so to
             work out if it should be shown selected, see if any of its children
             are current - if so a lower level item must be selected so don't
             show the top level site as selected 
          -->

        <xsl:if test="not(descendant::Item[@Current='True']) and not(descendant::Item[@Selected='True'])">
          <xsl:attribute name="class">Selected</xsl:attribute>
        </xsl:if>

        <a href="{@Url}" title="{Title}">
          <xsl:value-of select="@Title" disable-output-escaping="yes"/>
        </a>
      </li>
      <xsl:apply-templates select="Item" >
        <xsl:with-param name="level" select="1" />
        <xsl:with-param select="@Title" name="sectionTitle" />
      </xsl:apply-templates>
    </ul>
  </xsl:template>

  <xsl:template name="space">
    <xsl:text xml:space="preserve"> </xsl:text>
  </xsl:template>

</xsl:stylesheet>

