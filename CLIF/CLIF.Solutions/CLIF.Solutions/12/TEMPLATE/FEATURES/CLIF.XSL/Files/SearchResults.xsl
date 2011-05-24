<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- xmlns:s="uuid:BDC6E3F0-6DA3-11d1-A2A3-00AA00C14882" xmlns:z="#RowsetSchema">
  <s:Schema id="RowsetSchema"/> -->
  <xsl:output method="xml" omit-xml-declaration="yes" />

  <xsl:template match="/">
    <!--<xsl:if test="Xml/@ErrorCode = ''">-->
    <xsl:variable name="RowCount" select="Xml/search_attributes/total_results"/>
    <xsl:variable name="StartNo" select ="Xml/search_attributes/start_row"/>
    <div >
      <h3>
        <xsl:text>You searched for </xsl:text>
        <xsl:value-of select="Xml/search_attributes/searched_text"/>
        <xsl:if test="Xml/AreaName">
          <xsl:text> in </xsl:text>
        </xsl:if>
        <xsl:value-of select="Xml/AreaName"/>
      </h3>
      <h5>
        <xsl:choose>
          <xsl:when test="$RowCount = 0">
            <xsl:text>No results matching your search were found.</xsl:text>
          </xsl:when>
          <xsl:otherwise>
            <xsl:text>There are </xsl:text>
            <strong>
              <xsl:value-of select="$RowCount"/>
            </strong>
            <xsl:text> search results</xsl:text>
          </xsl:otherwise>
        </xsl:choose>
      </h5>

      <xsl:apply-templates select="Xml/search_attributes"/>
      <ol class="search-results" start="{$StartNo}">
        <xsl:call-template name="result" />
      </ol>
      <xsl:apply-templates select="Xml/search_attributes"/>
    </div>
    <!--</xsl:if>-->
  </xsl:template>

  <xsl:template name="result">
    <xsl:if test="count(Xml/result) &gt; 0">
      <xsl:apply-templates select="Xml/result" />
    </xsl:if>
    <xsl:if test="count(Xml/error) &gt; 0">
      <div class="subbox">
        <xsl:value-of select="Xml/error/message"/>
      </div>
    </xsl:if>
  </xsl:template>

  <xsl:template match="result" name="srow">
    <div class="resultDiv">
      <li>
        <div class="searchresult">
          <div class="Documents">
            <div>
              <a href="{Path}" target="_blank">
                <xsl:value-of select="Title" disable-output-escaping="yes" />
              </a>
              <xsl:value-of select="Description" disable-output-escaping="yes" />
              <br></br>
              <table border="0"  class="MetaTable" >
                <tr>
                  <td>
                    <strong>PID:</strong>
                    <xsl:value-of select="PID" />
                  </td>
                  <td>
                    <strong>Author(s):</strong>
                  </td>
                  <td>
                    <strong>Last modified:</strong>
                    <xsl:value-of select="LastModifiedTime" />
                  </td>
                </tr>

              </table>

            </div>
          </div>
        </div>
      </li>
     
      <br/>
    </div>
    <br/>

  </xsl:template>

  <xsl:template match="search_attributes">
    <!--<xsl:if test="total_results != '0'">-->
    <xsl:choose>
      <xsl:when test="total_results != '0'">
        <div class="rounded">
          <!--<div class="greenbar">
          <div class="sortbox">-->
          <div class="top">
            <div class="left">
              <xsl:call-template name="space"/>
            </div>
            <div class="right">
              <xsl:call-template name="space"/>
            </div>
          </div>
          <div class="body">
            <div class="paging">
              <div class="pagingInner">
                <div class="pagelinks">
                  <span class="paginginfo">
                    Results <xsl:value-of select="start_row" /> to <xsl:value-of select="end_row" /> of
                    <strong>
                      <xsl:value-of select="total_results" />
                    </strong>
                  </span>
                  <!--<div class="pagenumber">
                    <div class="pagenumber">-->
                  <xsl:variable name="prev_no">
                    <xsl:value-of select="current_page - 2" />
                  </xsl:variable>
                  <xsl:choose>
                    <xsl:when test="prev_class = 'prevoff'">
                      <!--<span class="prevoff">
                          <a href="#">
                            <span class="hide">Previous</span>
                          </a>
                        </span>-->
                    </xsl:when>
                    <xsl:when test="prev_class = 'prev'">
                      <a href="{page_path}{query_string}&amp;p={$prev_no}" title="previous" class="btnPrevious">
                        <!--<span class="btnPrevious">
                            <a href="{page_path}{query_string}&amp;p={$prev_no}">-->
                        <span class="hide">Previous</span>
                        <!--</a>
                          </span>-->
                      </a>
                    </xsl:when>
                  </xsl:choose>

                  <xsl:call-template name="page_links">
                    <xsl:with-param name="i">
                      <xsl:value-of select="starting_no" />
                    </xsl:with-param>
                    <xsl:with-param name="count">
                      <xsl:value-of select="end_no" />
                    </xsl:with-param>
                    <xsl:with-param name="pageNo">
                      <xsl:value-of select="current_page" />
                    </xsl:with-param>
                  </xsl:call-template>

                  <xsl:choose>
                    <xsl:when test="next_class = 'nextoff'">
                      <!--<span class="nextoff">
                          <a href="#">
                            <span class="hide">Next</span>
                          </a>
                        </span>-->
                    </xsl:when>
                    <xsl:when test="next_class = 'next'">

                      <a class="btnNext" title="next" href="{page_path}{query_string}&amp;p={current_page}">
                        <!--<span class="btnNext">-->
                        <!--<a href="{page_path}{query_string}&amp;p={current_page}">-->
                        <span class="hide">Next</span>
                        <!--</a>-->
                        <!--</span>-->
                      </a>
                    </xsl:when>
                  </xsl:choose>
                  <!--</div>-->

                  <div class="clear">
                    <xsl:text xml:space="preserve"> </xsl:text>
                  </div>
                </div>
                <div class="clear"></div>
              </div>
            </div>
          </div>
          <div class="bottom">
            <div class="left">
              <xsl:call-template name="space"/>
            </div>
            <div class="right">
              <xsl:call-template name="space"/>
            </div>
          </div>
        </div>
      </xsl:when>
      <xsl:otherwise>
        <div class="clear">
          <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
        </div>
      </xsl:otherwise>
    </xsl:choose>
    <!--</xsl:if>-->
  </xsl:template>

  <xsl:template name="page_links">
    <xsl:param name="i"      />
    <xsl:param name="count"  />
    <xsl:param name="pageNo" />
    <!--begin_: Line_by_Line_Output -->
    <xsl:if test="$i &lt;= $count">
      <xsl:variable name="page_no">
        <xsl:value-of select="$i - 1" />
      </xsl:variable>
      <!--<a href="{page_path}{query_string}&amp;p={$page_no}" class="paginglink" title="Page {$i}">-->
      <xsl:choose>
        <xsl:when test="$i = $pageNo">
          <!--<strong>-->
          <a href="{page_path}{query_string}&amp;p={$page_no}" class="paginglink_active" title="Page {$i}">
            <xsl:value-of select="$i" />
          </a>
          <!--</strong>-->
        </xsl:when>
        <xsl:otherwise>
          <a href="{page_path}{query_string}&amp;p={$page_no}" class="paginglink" title="Page {$i}">
            <xsl:value-of select="$i" />
          </a>
        </xsl:otherwise>
      </xsl:choose>
      <!--</a>-->
      <span class="hide">|</span>
      <xsl:text xml:space="preserve"> </xsl:text>
    </xsl:if>
    <xsl:if test="$i &lt;= $count">
      <xsl:call-template name="page_links">
        <xsl:with-param name="i">
          <xsl:value-of select="$i + 1"/>
        </xsl:with-param>
        <xsl:with-param name="count">
          <xsl:value-of select="$count"/>
        </xsl:with-param>
        <xsl:with-param name="pageNo">
          <xsl:value-of select="$pageNo"/>
        </xsl:with-param>

      </xsl:call-template>
    </xsl:if>
  </xsl:template>

  <xsl:template name="space">
    <xsl:text xml:space="preserve"> </xsl:text>
  </xsl:template>

</xsl:stylesheet>

