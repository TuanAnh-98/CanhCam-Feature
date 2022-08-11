<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="html" indent="yes" />

  <xsl:template match="/">
    <div class="search-result-wrap product-result">
      <div class="search-top">
        <div class="row">
          <div class="col-12">
            <div class="search-title section-title text-blue">
              Kết quả tìm kiếm “<span class="text-main">
                <xsl:value-of select="/SearchResults/Keyword"></xsl:value-of>
              </span>”
            </div>
            <div class="searchresultsummary">
              Tìm thấy <span>
                <xsl:value-of select="/SearchResults/TotalProducts"></xsl:value-of>
              </span> kết quả phù hợp với từ khóa “<xsl:value-of select="/SearchResults/Keyword"></xsl:value-of>”
            </div>
          </div>
          <!-- <div class="col-lg-4">
            <xsl:if test="count(/SearchResults/Product)>0">
              <div class="product-list-util flex items-center justify-end">
                <div class="product-list-sort">
                  <strong class="text-blue mr-1">Sắp xếp: </strong>
                  <select class="ajaxsort">
                    <option selected="" value="sort=0">Mới nhất</option>
                    <option value="sort=10">Giá: Thấp - Cao</option>
                    <option value="sort=11">Giá: Cao - Thấp</option>
                  </select>
                </div>
                <div class="product-list-display ml-3 hidden-mobile">
                  <strong class="text-blue mr-1">Hiển thị: </strong>
                  <select>
                    <option>20</option>
                    <option>40</option>
                    <option>60</option>
                  </select>
                </div>
                <div class="filter-mobile-toggle flex flex-center hidden-desktop text-blue">
                  <em class="ri-filter-fill"></em>
                  <strong class="ml-1">Bộ lọc</strong>
                </div>
              </div>
            </xsl:if>
          </div> -->
        </div>
      </div>
      <!-- <xsl:if test="count(/SearchResults/Product)>0">
        <div class="search-filter">
          <div class="product-filter-wrap">
            <div class="product-filter-heading hidden-desktop background-main text-white">
              <div class="flex items-center justify-between">
                <div class="filter-close">
                  <em class="ri-close-line"></em>
                </div>
                <div class="filter-title flex items-center">
                  <em class="ri-filter-fill"></em>
                  <strong>Bộ lọc sản phẩm</strong>
                </div>
                <div class="filter-reset">Bỏ chọn</div>
              </div>
            </div>
            <div class="product-filter-container">
              <div class="product-filter-item product-filter-price">
                <div class="product-filter-title flex items-center justify-between">
                  <strong class="text">Giá</strong>
                  <div class="arrow">
                    <em class="ri-arrow-down-s-fill"></em>
                  </div>
                </div>
                <div class="product-filter-body">
                  <div class="price-price-slider price-slider" data-max="10000000"></div>
                  <div class="price-filter-text-wrap flex justify-between">
                    <div class="price-filter-text-min-wrap">
                      <span>Giá từ </span>
                      <span class="price-filter-text-min"></span>
                    </div>
                    <div class="price-filter-text-max"></div>
                  </div>
                  <div class="price-filter-input-wrap">
                    <input class="price-filter-input-min" type="text" placeholder="Giá từ" />
                    <input class="price-filter-input-max" type="text" placeholder="Giá đến" />
                    <a class="price-filter-input-btn" href="#">Đồng ý</a>
                  </div>
                </div>
              </div>
              <xsl:if test="count(/SearchResults/Zone)>0">
                <div class="product-filter-item">
                  <div class="product-filter-title flex items-center justify-between">
                    <strong class="text">Danh mục sản phẩm</strong>
                    <div class="arrow">
                      <em class="ri-arrow-down-s-fill"></em>
                    </div>
                  </div>
                  <div class="product-filter-body">
                    <div class="product-filter-group">
                      <ul>
                        <xsl:apply-templates select="/SearchResults/Zone"></xsl:apply-templates>
                      </ul>
                    </div>
                  </div>
                </div>
              </xsl:if>
            </div>
            <div class="product-filter-apply hidden-desktop">
              <a class="btn filter-apply" href="#">Áp dụng bộ lọc</a>
            </div>
          </div>
        </div>
      </xsl:if> -->
      <div class="searchresults">
        <!-- <div class="section-title text-blue">Sản phẩm</div> -->
        <div class="product-list">
          <div class="row row-lg-5 row-md-3 row-2">
            <xsl:apply-templates select="/SearchResults/Product"></xsl:apply-templates>
          </div>
        </div>
      </div>
    </div>
    <xsl:if test="count(/SearchResults/News)>0">
      <div class="search-result-wrap news-result">
        <div class="searchresults">
          <div class="section-title text-blue">Tin tức</div>
          <div class="news-list">
            <div class="row row-lg-4 row-md-3 row-2">
              <xsl:apply-templates select="/SearchResults/News"></xsl:apply-templates>
            </div>
          </div>
          <!--<div class="view-more-wrap">
            <a class="btn btn-white" href="#">
              Xem thêm<em class="ri-arrow-down-s-fill"></em>
            </a>
          </div>-->
        </div>
      </div>
    </xsl:if>
  </xsl:template>

  <xsl:template match="Zone">
    <li>
      <a class="filter-link" href="#">
        <xsl:attribute name="data-id">
          <xsl:value-of select="ZoneId"></xsl:value-of>
        </xsl:attribute>
        <xsl:attribute name="data-url">
          <xsl:value-of select="Url"></xsl:value-of>
        </xsl:attribute>
        <div class="check-box"></div>
        <div class="text">
          <xsl:value-of select="Title"></xsl:value-of>
        </div>
      </a>
    </li>

    <!--<div class="col-xs-12 col-md-6 col-lg-6 col-xl-3 nopadding">
      <h3 class="title">
        <a>
          <xsl:attribute name="href">
            <xsl:value-of select="Url"></xsl:value-of>
          </xsl:attribute>
          <xsl:value-of select="Title" disable-output-escaping="yes"></xsl:value-of>
        </a>
      </h3>
      <div class="resultitem">
        <xsl:text>Có </xsl:text>
        <strong>
          <xsl:value-of select="TotalProducts"></xsl:value-of>
        </strong>
        <xsl:text> kết quả tìm được</xsl:text>
      </div>
      <div class="itemlist clearfix">
        <xsl:apply-templates select="Product"></xsl:apply-templates>
      </div>
      <xsl:if test="TotalPages>1">
        <a class="ajaxsearchviewmore" data-next="2">
          <xsl:attribute name="data-zoneid">
            <xsl:value-of select="ZoneId"></xsl:value-of>
          </xsl:attribute>
          <xsl:attribute name="data-totalpages">
            <xsl:value-of select="TotalPages"></xsl:value-of>
          </xsl:attribute>
          <xsl:attribute name="data-keyword">
            <xsl:value-of select="/SearchResults/Keyword"></xsl:value-of>
          </xsl:attribute>
          <xsl:text>Xem thêm</xsl:text>
        </a>
      </xsl:if>
    </div>-->
  </xsl:template>

  <xsl:template match="Product">
    <div class="w-100">
      <div class="product-item">
        <a class="product-img" href="#">
          <xsl:attribute name="href">
            <xsl:value-of select="Url"></xsl:value-of>
          </xsl:attribute>
          <xsl:attribute name="target">
            <xsl:value-of select="Target"></xsl:value-of>
          </xsl:attribute>
          <xsl:attribute name="title">
            <xsl:value-of select="Title"></xsl:value-of>
          </xsl:attribute>
          <img>
          <xsl:attribute name="src">
            <xsl:value-of select="ImageUrl"></xsl:value-of>
          </xsl:attribute>
          <xsl:attribute name="alt">
            <xsl:value-of select="Title"></xsl:value-of>
          </xsl:attribute>
          </img>
          <xsl:if test="DiscountPercentage!=''">
            <div class="product-discount">
              <xsl:text>Giảm </xsl:text>
              <xsl:value-of select="DiscountPercentage"></xsl:value-of>
              <xsl:text>%</xsl:text>
            </div>
          </xsl:if>
        </a>
        <xsl:if test="ShowOption>0">
          <div class="product-icon">
            <xsl:if test="floor(ShowOption div 1) mod 2 = 1">
              <span class="icon icon-new">new</span>
            </xsl:if>
          </div>
        </xsl:if>
        <div class="product-name leading-snug">
          <a>
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
        </div>
        <div class="product-price">
          <strong class="price text-main">
            <xsl:value-of disable-output-escaping="yes" select="Price"></xsl:value-of>
          </strong>
          <xsl:if test="OldPrice != ''">
            <del class="old-price">
              <xsl:value-of disable-output-escaping="yes" select="OldPrice"></xsl:value-of>
            </del>

          </xsl:if>
        </div>
      </div>
    </div>

  </xsl:template>

  <xsl:template match="Promotion">
    <xsl:if test="floor(Option div 1) mod 2 = 1">
      <xsl:if test="DiscountPrice!=''">
        <div class="product-vnpay-price">
          <strong>Giá VnPay: </strong>
          <strong class="text-main">
            <xsl:value-of select="DiscountPrice"></xsl:value-of>
          </strong>
        </div>
      </xsl:if>
    </xsl:if>
  </xsl:template>

  <xsl:template match="News">
    <div class="w-100">
      <div class="news-item small-news">
        <div class="news-image">
          <a>
            <xsl:attribute name="href">
              <xsl:value-of select="Url" />
            </xsl:attribute>
            <img class="lazyload" src="data:image/gif;base64,R0lGODlhAQABAAD/ACwAAAAAAQABAAACADs=">
            <xsl:attribute name="data-src">
              <xsl:value-of select="ImageUrl"></xsl:value-of>
            </xsl:attribute>
            <xsl:attribute name="alt">
              <xsl:value-of select="Title"></xsl:value-of>
            </xsl:attribute>
            </img>
          </a>
        </div>
        <div class="news-caption leading-snug">
          <div class="news-title fz-18 font-bold mb-1">
            <a>
              <xsl:attribute name="href">
                <xsl:value-of select="Url" />
              </xsl:attribute>
              <xsl:value-of select="Title"></xsl:value-of>
            </a>
          </div>
        </div>
      </div>
    </div>

  </xsl:template>
</xsl:stylesheet>