$(document).ready(function () {

    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(function () {
        initCorePlugins();
    });

    initCorePlugins();
    function initCorePlugins() {
        //$('.header-buttons').scrollToFixed();
        if ($('.header-buttons').length && $('.header-buttons').html().length > 0) {
            $('.header-buttons').scrollToFixed({
            });
        }

        $('a.mhelp').fancybox({
            type: 'iframe', title: { type: 'outside' }
        });
        $('a.popup-link').fancybox({
            width: '80%', height: '80%', type: 'iframe', autoSize: false, title: { type: 'outside' }
        });
        $('a.mhelp').cluetip({
            attribute: 'href', showTitle: false, arrows: true, dropShadow: false, cluetipClass: 'help'
        });
    }

});