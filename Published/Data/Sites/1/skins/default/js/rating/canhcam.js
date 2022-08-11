;;
var wdH = 0,
    wdW = 0;


$(document).ready(function() {
    "use strict";
    winresize();


    if ($(window).width() < 1024) {
        $('.allmenu-button').click(function() {
            $(this).toggleClass('active');
            $('.list-menu').fadeToggle();
        });
    }
    $('.info-button').click(function() {
        $(this).toggleClass('active');
        $('.mobile-show').fadeToggle();
    });

    $('.icon-subleftmenu').click(function() {
        if ($(this).parent().parent().hasClass('active')) {
            $(this).parent().parent().removeClass('active');
        } else {
            $('.leftmenu ul > li').removeClass('active');
            $(this).parent().parent().addClass('active');
        }

    });

    $('.select-amount .btn-dec').on('click', function() {
        var input = $(this).parent().find('input');
        var v = input.val();
        v--;
        input.val(v);
        if ($('.table-wrap').length)
            input.trigger('change');
        return false;
    });
    $('.select-amount .btn-inc').on('click', function() {
        var input = $(this).parent().find('input');
        var v = input.val();
        v++;
        if (v > 0)
            input.val(v);
        if ($('.table-wrap').length)
            input.trigger('change');
        return false;
    });

    $('.single-item').slick({
		lazyLoad: 'ondemand',
		adaptiveHeight: true
	});

    $('.prd-slide').slick({
        dots: false,
		lazyLoad: 'ondemand',
        infinite: true,
        speed: 300,
        slidesToShow: 5,
        slidesToScroll: 1,
        responsive: [{
                breakpoint: 1028,
                settings: {
                    slidesToShow: 4
                }
            },
            {
                breakpoint: 800,
                settings: {
                    slidesToShow: 4
                }
            },
            {
                breakpoint: 650,
                settings: {
                    slidesToShow: 2
                }
            }
        ]
    });
    $('.prd-home a[data-toggle="tab"]').on('shown.bs.tab', function(e) {

        e.preventDefault()
        var id = $(this).attr('href');
        $(id + ' .prd-slide').slick('unslick');

        $(id + ' .prd-slide').slick({
            dots: false,
            infinite: true,
			lazyLoad: 'ondemand',
            speed: 300,
            slidesToShow: 5,
            slidesToScroll: 1,
            responsive: [{
                    breakpoint: 1028,
                    settings: {
                        slidesToShow: 4
                    }
                },
                {
                    breakpoint: 800,
                    settings: {
                        slidesToShow: 4
                    }
                },
                {
                    breakpoint: 650,
                    settings: {
                        slidesToShow: 2
                    }
                }
            ]
        });
        //  $(id + " .slick-slider").hide();      
        //  $(id + ' .slick-next').trigger('click');
        //  $(id + " .slick-slider").show();    
        //$(id + ' .prd-related , .prdzone-slide').resize();product-zone-list
    });

    $('.product-zone-list a[data-toggle="tab"]').on('shown.bs.tab', function(e) {

        e.preventDefault()
        var id = $(this).attr('href');
        $(id + ' .prdzone-slide').slick('unslick');

        $(id + ' .prdzone-slide').slick({
            dots: false,
			lazyLoad: 'ondemand',
            infinite: false,
            speed: 300,
            slidesToShow: 4,
            slidesToScroll: 1,
            responsive: [{
                    breakpoint: 1024,
                    settings: {
                        slidesToShow: 4
                    }
                },
                {
                    breakpoint: 800,
                    settings: {
                        slidesToShow: 3
                    }
                },
                {
                    breakpoint: 650,
                    settings: {
                        slidesToShow: 2
                    }
                }
            ]
        });

    });

    $('.prd-related').slick({
        dots: false,
        infinite: false,
		lazyLoad: 'ondemand',
        speed: 300,
        slidesToShow: 5,
        slidesToScroll: 1,
        responsive: [{
                breakpoint: 1028,
                settings: {
                    slidesToShow: 4
                }
            },
            {
                breakpoint: 800,
                settings: {
                    slidesToShow: 4
                }
            },
            {
                breakpoint: 650,
                settings: {
                    slidesToShow: 2
                }
            }
        ]
    });

    $('.banner-zone-slide').slick({
        dots: true,
		lazyLoad: 'ondemand',
        infinite: false,
        arrows: false,
        speed: 300,
        slidesToShow: 1,
        slidesToScroll: 1,
    });
    $('.prdzone-slide').slick({
        dots: false,
		lazyLoad: 'ondemand',
        infinite: false,
        speed: 300,
        slidesToShow: 4,
        slidesToScroll: 1,
        responsive: [{
                breakpoint: 1024,
                settings: {
                    slidesToShow: 4
                }
            },
            {
                breakpoint: 800,
                settings: {
                    slidesToShow: 3
                }
            },
            {
                breakpoint: 650,
                settings: {
                    slidesToShow: 2
                }
            }
        ]
    });
    $('.img-slide').slick({
        slidesToShow: 1,
		lazyLoad: 'ondemand',
        slidesToScroll: 1,
        arrows: false,
        fade: true,
        asNavFor: '.nav-slide',
        infinite: true
    });
    $('.nav-slide').slick({
		lazyLoad: 'ondemand',
        slidesToShow: 4,
        slidesToScroll: 1,
        responsive: [{
            breakpoint: 1024,
            settings: {
                slidesToShow: 3
            }
        }, {
            breakpoint: 991,
            settings: {
                slidesToShow: 2
            }
        }, {
            breakpoint: 480,
            settings: {
                slidesToShow: 2
            }
        }],
        prevArrow: $('.img-prev'),
        nextArrow: $('.img-next'),
        asNavFor: '.img-slide',
        focusOnSelect: true,
        infinite: true,
        vertical: true
            // verticalSwiping: true
    });

    if ($(window).width() > 767) {
        $('.event-content').responsiveEqualHeightGrid();
    }
    $('.faq-group').click(function() {
        $('.faq-group').removeClass('open');
        $('.faq-group .answer').slideUp(300);
        $(this).toggleClass('open');
        $(this).children('.answer').slideDown(300);
    });

    $('.filter-name').click(function() {
        $(this).parent('.filter-kind').children('.filter-list').slideToggle(300);
        $(this).toggleClass('minimize');
    });
    $('.filter').click(function() {
        $(this).toggleClass('active');
    });

    $('a.popup').fancybox({
        width: 1200,
        height: 400,
        type: 'iframe'
    });
    $(".main-img").fancybox({
        'padding': 0,
        helpers: {
            overlay: {
                locked: false
            }
        }
    });

    $('header').scrollToFixed();
    //custom menu home
    $(window).scroll(function() {
		 if ($('.altcontent2.cmszone').length) {
			var mainOffsetTop = $(".altcontent2.cmszone").offset().top;
			var menuOffsetTop = $("ul.list-menu").offset().top;
			var menuHeight = $("ul.list-menu").outerHeight(true);
			var headerHeight = $('header').outerHeight(true);

			var calOffset = mainOffsetTop - menuHeight - headerHeight;
			var _top = 0;
			if (menuOffsetTop >= calOffset) {
				$("header").css("position", "absolute");
				$("header").css("top", calOffset + "px");
				_top = calOffset;
			} else {

				$("header").css("position", "fixed");
				$("header").css("top", "0");

			}


			if ($(this).scrollTop() < _top && $("header").css("position") == "absolute") {
				$("header").css("position", "fixed");
				$("header").css("top", "0");
			}
		 }
        
    });

    $('.info-group .title').bind('click', function() {
        $('.bill-form').slideToggle();
    });
    $('.voucher .title').bind('click', function() {
        $('.voucher-form').slideToggle();
    });


    $(".facebook,.google").on('click', function() {
        var url = $(this).data('url');
        window.open(url, "popupWindow", "width=660,height=480,scrollbars=yes");
        return false;
    });

    $('.close-btn').click(function(event) {
        $(this).parent().addClass('hidden');
    });

    $('.comment-text a').click(function() {
        $('html,body').animate({
            scrollTop: $(this.hash).offset().top - 100
        }, 700);
        return false;
    });
	$("main img").lazyload({
        effect: "fadeIn",
		effectTime: 500
    });

});

;
$(function() {
    "use strict";

    $('html').scrollUp({
        scrollName: 'scrollUp-text', // Element ID
        topDistance: '300', // Distance from top before showing element (px)
        topSpeed: 300, // Speed back to top (ms)
        animation: 'fade', // Fade, slide, none
        animationInSpeed: 200, // Animation in speed (ms)
        animationOutSpeed: 200, // Animation out speed (ms)
        scrollText: '<i class="fa fa-angle-double-up" aria-hidden="true"></i>', // Text for element
        activeOverlay: false // Set CSS color to display scrollUp active point, e.g '#00FFFF'
    });
});

;
$(document).on('DOMNodeInserted', 'main img', function() {
	$(this).lazyload({
		effect: 'fadeIn'
	});
});


function winresize() {
    wdH = $(window).height();
    wdW = $(window).width();
};