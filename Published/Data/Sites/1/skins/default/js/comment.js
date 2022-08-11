/*!
* jQuery Raty - A Star Rating Plugin
*
* Licensed under The MIT License
*
* @version        2.4.5
* @author         Washington Botelho
* @documentation  wbotelhos.com/raty
* 
*/
; (function (b) { var a = { init: function (c) { return this.each(function () { var d = this, h = b(d).empty(); d.opt = b.extend(true, {}, b.fn.raty.defaults, c); h.data("settings", d.opt); d.opt.number = a.between(d.opt.number, 0, 20); if (d.opt.path.substring(d.opt.path.length - 1, d.opt.path.length) != "/") { d.opt.path += "/"; } if (typeof d.opt.score == "function") { d.opt.score = d.opt.score.call(d); } if (d.opt.score) { d.opt.score = a.between(d.opt.score, 0, d.opt.number); } for (var e = 1; e <= d.opt.number; e++) { b("<img />", { src: d.opt.path + ((!d.opt.score || d.opt.score < e) ? d.opt.starOff : d.opt.starOn), alt: e, title: (e <= d.opt.hints.length && d.opt.hints[e - 1] !== null) ? d.opt.hints[e - 1] : e }).appendTo(d); if (d.opt.space) { h.append((e < d.opt.number) ? "&#160;" : ""); } } d.stars = h.children('img:not(".raty-cancel")'); d.score = b("<input />", { type: "hidden", name: d.opt.scoreName }).appendTo(d); if (d.opt.score && d.opt.score > 0) { d.score.val(d.opt.score); a.roundStar.call(d, d.opt.score); } if (d.opt.iconRange) { a.fill.call(d, d.opt.score); } a.setTarget.call(d, d.opt.score, d.opt.targetKeep); var g = d.opt.space ? 4 : 0, f = d.opt.width || (d.opt.number * d.opt.size + d.opt.number * g); if (d.opt.cancel) { d.cancel = b("<img />", { src: d.opt.path + d.opt.cancelOff, alt: "x", title: d.opt.cancelHint, "class": "raty-cancel" }); if (d.opt.cancelPlace == "left") { h.prepend("&#160;").prepend(d.cancel); } else { h.append("&#160;").append(d.cancel); } f += (d.opt.size + g); } if (d.opt.readOnly) { a.fixHint.call(d); if (d.cancel) { d.cancel.hide(); } } else { h.css("cursor", "pointer"); a.bindAction.call(d); } }); }, between: function (e, d, c) { return Math.min(Math.max(parseFloat(e), d), c); }, bindAction: function () { var c = this, e = b(c); e.mouseleave(function () { var f = c.score.val() || undefined; a.initialize.call(c, f); a.setTarget.call(c, f, c.opt.targetKeep); if (c.opt.mouseover) { c.opt.mouseover.call(c, f); } }); var d = c.opt.half ? "mousemove" : "mouseover"; if (c.opt.cancel) { c.cancel.mouseenter(function () { b(this).attr("src", c.opt.path + c.opt.cancelOn); c.stars.attr("src", c.opt.path + c.opt.starOff); a.setTarget.call(c, null, true); if (c.opt.mouseover) { c.opt.mouseover.call(c, null); } }).mouseleave(function () { b(this).attr("src", c.opt.path + c.opt.cancelOff); if (c.opt.mouseover) { c.opt.mouseover.call(c, c.score.val() || null); } }).click(function (f) { c.score.removeAttr("value"); if (c.opt.click) { c.opt.click.call(c, null, f); } }); } c.stars.bind(d, function (g) { var h = parseInt(this.alt, 10); if (c.opt.half) { var f = parseFloat((g.pageX - b(this).offset().left) / c.opt.size), i = (f > 0.5) ? 1 : 0.5; h = parseFloat(this.alt) - 1 + i; a.fill.call(c, h); if (c.opt.precision) { h = h - i + f; } a.showHalf.call(c, h); } else { a.fill.call(c, h); } e.data("score", h); a.setTarget.call(c, h, true); if (c.opt.mouseover) { c.opt.mouseover.call(c, h, g); } }).click(function (f) { c.score.val((c.opt.half || c.opt.precision) ? e.data("score") : this.alt); if (c.opt.click) { c.opt.click.call(c, c.score.val(), f); } }); }, cancel: function (c) { return b(this).each(function () { var d = this, e = b(d); if (e.data("readonly") === true) { return this; } if (c) { a.click.call(d, null); } else { a.score.call(d, null); } d.score.removeAttr("value"); }); }, click: function (c) { return b(this).each(function () { if (b(this).data("readonly") === true) { return this; } a.initialize.call(this, c); if (this.opt.click) { this.opt.click.call(this, c); } else { a.error.call(this, 'you must add the "click: function(score, evt) { }" callback.'); } a.setTarget.call(this, c, true); }); }, error: function (c) { b(this).html(c); b.error(c); }, fill: function (k) { var c = this, h = c.stars.length, g = 0, d, j, f; for (var e = 1; e <= h; e++) { d = c.stars.eq(e - 1); if (c.opt.iconRange && c.opt.iconRange.length > g) { j = c.opt.iconRange[g]; if (c.opt.single) { f = (e == k) ? (j.on || c.opt.starOn) : (j.off || c.opt.starOff); } else { f = (e <= k) ? (j.on || c.opt.starOn) : (j.off || c.opt.starOff); } if (e <= j.range) { d.attr("src", c.opt.path + f); } if (e == j.range) { g++; } } else { if (c.opt.single) { f = (e == k) ? c.opt.starOn : c.opt.starOff; } else { f = (e <= k) ? c.opt.starOn : c.opt.starOff; } d.attr("src", c.opt.path + f); } } }, fixHint: function () { var c = b(this), e = parseInt(this.score.val(), 10), d = this.opt.noRatedMsg; if (!isNaN(e) && e > 0) { d = (e <= this.opt.hints.length && this.opt.hints[e - 1] !== null) ? this.opt.hints[e - 1] : e; } c.data("readonly", true).css("cursor", "default").attr("title", d); this.score.attr("readonly", "readonly"); this.stars.attr("title", d); }, getScore: function () { var d = [], c; b(this).each(function () { c = this.score.val(); d.push(c ? parseFloat(c) : undefined); }); return (d.length > 1) ? d : d[0]; }, readOnly: function (c) { return this.each(function () { var d = b(this); if (d.data("readonly") === c) { return this; } if (this.cancel) { if (c) { this.cancel.hide(); } else { this.cancel.show(); } } if (c) { d.unbind(); d.children("img").unbind(); a.fixHint.call(this); } else { a.bindAction.call(this); a.unfixHint.call(this); } d.data("readonly", c); }); }, reload: function () { return a.set.call(this, {}); }, roundStar: function (e) { var d = (e - Math.floor(e)).toFixed(2); if (d > this.opt.round.down) { var c = this.opt.starOn; if (d < this.opt.round.up && this.opt.halfShow) { c = this.opt.starHalf; } else { if (d < this.opt.round.full) { c = this.opt.starOff; } } this.stars.eq(Math.ceil(e) - 1).attr("src", this.opt.path + c); } }, score: function () { return arguments.length ? a.setScore.apply(this, arguments) : a.getScore.call(this); }, set: function (c) { this.each(function () { var d = b(this), f = d.data("settings"), e = d.clone().removeAttr("style").insertBefore(d); d.remove(); e.raty(b.extend(f, c)); }); return b(this.selector); }, setScore: function (c) { return b(this).each(function () { if (b(this).data("readonly") === true) { return this; } a.initialize.call(this, c); a.setTarget.call(this, c, true); }); }, setTarget: function (e, d) { if (this.opt.target) { var c = b(this.opt.target); if (c.length == 0) { a.error.call(this, "target selector invalid or missing!"); } var f = e; if (!d || f === undefined) { f = this.opt.targetText; } else { if (this.opt.targetType == "hint") { f = (f === null && this.opt.cancel) ? this.opt.cancelHint : this.opt.hints[Math.ceil(f - 1)]; } else { f = this.opt.precision ? parseFloat(f).toFixed(1) : parseInt(f, 10); } } if (this.opt.targetFormat.indexOf("{score}") < 0) { a.error.call(this, 'template "{score}" missing!'); } if (e !== null) { f = this.opt.targetFormat.toString().replace("{score}", f); } if (c.is(":input")) { c.val(f); } else { c.html(f); } } }, showHalf: function (d) { var c = (d - Math.floor(d)).toFixed(1); if (c > 0 && c < 0.6) { this.stars.eq(Math.ceil(d) - 1).attr("src", this.opt.path + this.opt.starHalf); } }, initialize: function (c) { c = !c ? 0 : a.between(c, 0, this.opt.number); a.fill.call(this, c); if (c > 0) { if (this.opt.halfShow) { a.roundStar.call(this, c); } this.score.val(c); } }, unfixHint: function () { for (var c = 0; c < this.opt.number; c++) { this.stars.eq(c).attr("title", (c < this.opt.hints.length && this.opt.hints[c] !== null) ? this.opt.hints[c] : c); } b(this).data("readonly", false).css("cursor", "pointer").removeAttr("title"); this.score.attr("readonly", "readonly"); } }; b.fn.raty = function (c) { if (a[c]) { return a[c].apply(this, Array.prototype.slice.call(arguments, 1)); } else { if (typeof c === "object" || !c) { return a.init.apply(this, arguments); } else { b.error("Method " + c + " does not exist!"); } } }; b.fn.raty.defaults = { cancel: false, cancelHint: "cancel this rating!", cancelOff: "cancel-off.png", cancelOn: "cancel-on.png", cancelPlace: "left", click: undefined, half: false, halfShow: true, hints: ["bad", "poor", "regular", "good", "gorgeous"], iconRange: undefined, mouseover: undefined, noRatedMsg: "not rated yet", number: 5, path: "img/", precision: false, round: { down: 0.25, full: 0.6, up: 0.76 }, readOnly: false, score: undefined, scoreName: "score", single: false, size: 16, space: true, starHalf: "star-half.png", starOff: "star-off.png", starOn: "star-on.png", target: undefined, targetFormat: "{score}", targetKeep: false, targetText: "", targetType: "hint" }; })(jQuery);

; var journalItem = {};
journalItem.Object = 'Product';
journalItem.JournalType = 'status';
journalItem.Title = '';
journalItem.Summary = '';
journalItem.Body = '';
journalItem.ItemData = null;
journalItem.Security = 'E';
var cancelRequest = false;

(function ($, window) {
    "use strict";
    if($('body').hasClass("news-detail-page"))
        journalItem.Object = 'News';
    function resetJournalItem() {
        journalItem = {};
        journalItem.JournalType = 'status';
        journalItem.Object = 'Product';
        journalItem.Title = '';
        journalItem.Summary = '';
        journalItem.Body = '';
        journalItem.ItemData = null;
        journalItem.Security = 'E';
        journalItem.Tags = [];
        journalItem.Users = [];
        if($('body').hasClass("news-detail-page"))
            journalItem.Object = 'News';
    } 
    $.fn.journalTools = function (options) {
        $.fn.journalTools.defaultOptions = {
            placeHolder: '.journalPlaceholder',
            shareButton: '.btnShare',
            closeButton: '.journalClose',
            info: '.journalInfo',
            fullName: '.txtFullName',
            email: '.txtEmail',
            place: '.txtPlace',
            content: '.journalContent',
            items: '.journalItems',
            pager: '.commentpager a',
            securityMenu: '.securityMenu',
            maxLength: 2000
        };
        var opts = $.extend({}, $.fn.journalTools.defaultOptions, options),
            $wrap = this,
            $placeHolder = $wrap.find(opts.placeHolder),
            $shareButton = $wrap.find(opts.shareButton),
            $closeButton = $wrap.find(opts.closeButton),
            $info = $wrap.find(opts.info),
            $fullName = $wrap.find(opts.fullName),
            $email = $wrap.find(opts.email),
            $content = $wrap.find(opts.content),
            $items = $wrap.find(opts.items),
            $pager = $wrap.find(opts.pager),
            $contentShow = false,
            $maxLength = opts.maxLength,
            $securityMenu = $wrap.find(opts.securityMenu)

        var isDirtyHandler = function (event) {
            $shareButton.removeClass('disabled');
            $shareButton.unbind('keypress', isDirtyHandler);
        };
        $securityMenu.find('.handle').click(function () {
            $securityMenu.toggle();
        });
        $(opts.securityMenu + ' input').click(function () {
            journalItem.Security = $(this).val();
        });

        $placeHolder.click(function () {
            showContent();
        });

        $closeButton.click(function () {
            closeEditor();
        });
        $content.bind('keypress', function (event) {
            if (event.keyCode == 8 || event.keyCode == 46) {
                return;
            }
            if ($content.val().length >= $maxLength) {
                return false;
            }
        });
        $content.bind('paste', function (e) {
            setTimeout(function () {
                $content.val($content.val());
                if ($content.val().length >= $maxLength) {
                    var txt = $content.val().substring(0, $maxLength);
                    $content.val(txt);
                }
            }, 100);

        });
        function closeEditor() {
            $closeButton.hide();
            $info.hide();
            $contentShow = false;
            //            $securityMenu.hide();
            $shareButton.addClass('disabled');
            //            $('#tbar span').removeClass('selected');

            //            var linkArea = $('#linkArea');
            //            $('#linkArea #imagePreviewer').hide();
            //            linkArea.find('#linkInfo b').text('');
            //            linkArea.find('#linkInfo p').text('');
            //            linkArea.hide();
            //            linkArea.data('url', '');
            $content.data('linkedUp', false);

            //            $(".filePreviewArea").empty();
            $content.unbind('keypress', isDirtyHandler);
            $content.animate({
                height: '0'
            }, 400, function () {
                $content.val('').hide();
                $placeHolder.show();
                resetJournalItem();
            });
        }
        var showContent = function () {
            $placeHolder.hide();
            $info.show();
            $shareButton.addClass('disabled');
            $content.show().animate({
                height: '+=65'
            }, 400, function () {
                $contentShow = true;
                $content.focus();
                $content.bind('keypress', isDirtyHandler);
                $closeButton.show();
            });
        };
        $shareButton.click(function (e) {
            cancelRequest = true;
            e.preventDefault();
            var data = {};
            var $formWrap = $shareButton.parents('.commentpanel');
            data.itemId = journalOptions.itemId;
            data.commentType = $formWrap.attr('data-type');
            data.fullName = encodeURIComponent($fullName.val());
            data.email = encodeURIComponent($email.val());
            data.content = encodeURIComponent($content.val());
            data.rating = 0;
            if (data.commentType == 0)
                data.title = $formWrap.find("input[name='commenttitle']").val();
            else {
                data.rating = 4;
                if ($('.rating input[name="score"]').length)
                    data.rating = $('.rating input[name="score"]').val();
                data.title = $formWrap.find('.txtPhone').val();
            }
            data.images = $formWrap.find(".hdfImg").val();

            //            data.journalType = journalItem.JournalType;
            //            data.securitySet = journalItem.Security;
            //            var ItemData = journalItem.ItemData;
            //            if (ItemData != null) {
            //                data.itemData = JSON.stringify(ItemData);
            //            } else {
            //                data.itemData = '';
            //            }
            if (data.content == '' || data.content == '%3Cbr%3E') {
                alert(journalOptions.contentRequiredText);
                $content.focus();
                return false;
            }
            if (data.fullName == '') {
                alert(journalOptions.fullNameRequiredText);
                $fullName.focus();
                return false;
            }
            //            if(data.email != '')
            //            {
            //                var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            //                if (!regex.test(data.email)) {
            //                    alert(journalOptions.emailInvalidText);
            //                    $email.focus();
            //                    return false;
            //                }
            //            }

            //Check for a value
            var tmpValue = $content.val();
            tmpValue = tmpValue.replace(/<(?:.|\n)*?>/gm, '').replace(/\s+/g, '').replace(/&nbsp;/g, '');
            if (tmpValue == '') {
                alert(journalOptions.contentInvalidText);
                $content.focus();
                return false;
            }

            $.ajax({
                type: "POST",
                //headers: { 'RequestVerificationToken': journalOptions.token },
                url: journalOptions.siteRoot + '/' + journalItem.Object + "/Services/CommentService.asmx/CreateRating",
                data: data, //{ itemId: data.itemId, fullName: data.fullName, email: data.email, content: data.content },
                //beforeSend: opts.servicesFramework.setModuleHeaders,
                success: function (res) {
                    //                    var req = {};
                    //                    req.RowIndex = 0;
                    //                    req.MaxRows = 1;
                    //                    closeEditor();
                    //                    getNewItem(req, insertNewItem);
                    if (res.success) {
                        $(res.response).hide().prependTo($items).fadeIn();
                        $content.val('');
                        $('.fileuploader-items-list li').remove();
                        $('.btnClose').trigger('click');
                        pluginInit();
                    }
                    else
                        alert(res.response);
                },
                error: function (xhr, status, error) {
                    alert(error);
                }
            });
        });
        //        function getNewItem(data, callback) {
        //            $.ajax({
        //                type: "POST",
        //                url: journalOptions.siteRoot + '/' + journalItem.Object + "/Services/CommentService.asmx/GetList",
        //                //beforeSend: opts.servicesFramework.setModuleHeaders,
        //                data: data,
        //                success: function (data) {
        //                    if (typeof (callback) != "undefined") {
        //                        callback(data);
        //                    }
        //                },
        //                error: function (xhr, status, error) {
        //                    alert(error);
        //                }
        //            });
        //        }
        function insertNewItem(html) {
            if (html != '' && html != ' ') {
                $(html).hide().prependTo($items).fadeIn();
                pluginInit();
                //.slideDown();
            }
        }

        $pager.bind('click', function (e) {
            e.preventDefault();

            var pg = $(this);
            var pageurl = pg.attr('href');
            var t = pageurl.split("cmtpg=");
            if (isNaN(parseInt(t[1])) == !1 && parseInt(t[1]) > 0) {
                $.ajax({
                    type: "POST",
                    url: journalOptions.siteRoot + '/' + journalItem.Object + '/Services/CommentService.asmx/GetList',
                    data: { itemId: journalOptions.itemId, commentType: $(this).parents('.commentpanel').attr('data-type'), parentId: -1, pageNumber: parseInt(t[1]), pageSize: journalOptions.pageSize },
                    success: function (data) {
                        if (data.length > 0) {
                            $items.html(data);

                            //to change the browser URL to 'pageurl'
                            //if (pageurl != window.location) {
                            //    window.history.pushState({ path: pageurl }, '', pageurl);
                            //}

                            pg.parents('ul').find('li').removeClass('active');
                            pg.parent().addClass('active');

                            pg.parent().find('a').removeClass('SelectedPage');
                            pg.addClass('SelectedPage');

                            pluginInit();
                            $("html, body").animate({ scrollTop: $items.offset().top }, 0, 'linear');
                        }
                        if (typeof (callback) != "undefined") {
                            callback(data);
                        }
                    },
                    error: function (xhr, status, error) {
                        alert(error);
                    }
                });
            }
        });
    }

}(jQuery, window));

function pluginInit() {
    if (journalOptions.fullName.length) {
        var fullNameDecode = $("<div />").html(journalOptions.fullName).text();
        $('.txtFullName').val(fullNameDecode);
    }
    if (journalOptions.email.length) {
        var emailDecode = $("<div />").html(journalOptions.email).text();
        $('.txtEmail').val(emailDecode);
    }
    //if (journalOptions.place.length) {
    //    var emailDecode = $("<div />").html(journalOptions.place).text();
    //    $('.txtPlace').val(emailDecode);
    //}

    $('.cmtname').val($('.txtFullName').val());
    $('.cmtemail').val($('.txtEmail').val());

    $('.jcmt').each(function (index) {
        $(this).journalComments(commentOpts);
    });
    //    var rows = $(".journalrow");
    //    if(rows.length == journalOptions.pageSize){
    //        $("#getMore").show();
    //    }

    // $('a[id^="cmtbtn-"]').bind('click', function (e) {
    //     e.preventDefault();
    //     var jid = $(this).attr('id').replace('cmtbtn-', '');
    //     var cmtarea = $("#jcmt-" + jid + " .cmteditarea");
    //     var cmtbtn = $("#jcmt-" + jid + " .cmtbtn");
    //     var cmtbtnlink = $("#jcmt-" + jid + " .cmtbtn a");
    //     var cmtinfo = $("#jcmt-" + jid + " .cmtbtn .cmtinfo");
    //     if (cmtarea.css('display') == 'none') {
    //         cmtarea.show();
    //         cmtbtnlink.addClass('disabled');
    //         cmtbtn.show();
    //         //$("html, body").animate({ scrollTop: cmtarea.offset().top }, 0, 'linear');
    //     }
    //     else {
    //         var cmtedit = $("#jcmt-" + jid + " .cmteditor");
    //         var plh = $("#jcmt-" + jid + " .editorPlaceholder");
    //         cmtedit.animate({
    //             height: '0'
    //         }, 400, function () {
    //             cmtbtn.hide();
    //             cmtbtnlink.addClass('disabled');
    //             cmtinfo.hide();
    //             cmtedit.text('').hide();
    //             cmtarea.hide();
    //             plh.show();
    //         });
    //     }
    // });

    $('a[id^="like-"]').bind('click', likeComment);
}

function buildLikes(data, commentId) {
    $('#like-' + commentId).fadeOut(function () {
        $(this).html(data).fadeIn();
    });
};

function likeComment() {
    var jid = $(this).attr('id').replace('like-', '');
    journalPost('Like', buildLikes, jid);
    return false;
}

function journalReport(obj) {
    var jid = obj.id.replace('report-', '');
    journalPost('Report', reportCallback, jid);
}
function reportCallback(data, jid) {
    $('#report-' + jid).html(journalOptions.reportSuccessfullyText);
}
function journalDelete(obj) {
    if (confirm(journalOptions.deleteConfirmText)) {
        var jid = obj.parentNode.parentNode.id.replace('jid-', '');
        journalPost('Delete', deleteCallback, jid);
    }
}
function deleteCallback(data, jid) {
    $('#jid-' + jid).slideUp(function () {
        $(this).remove();
    });
}
function journalPost(method, callback, commentId) {
    $.ajax({
        type: "POST",
        //headers: { 'RequestVerificationToken': journalOptions.token },
        url: journalOptions.siteRoot + '/' + journalItem.Object + "/Services/CommentService.asmx/" + method,
        data: { commentId: commentId },
        success: function (data) {
            if (typeof (callback) != "undefined") {
                callback(data, commentId);
            }
        },
        error: function (xhr, status, error) {
            alert(error);
        }
    });
}


(function ($, window) {
    $.fn.journalComments = function (options) {
        $.fn.journalComments.defaultOptions = {
            maxLength: 2000
        };
        var opts = $.extend({}, $.fn.journalComments.defaultOptions, options),
            $wrap = this,
            $id = $(this).attr('id'),
            $maxLength = opts.maxLength,
            $textarea = $('#' + $id + '-txt'),
            $placeHolder = $('#' + $id + ' .editorPlaceholder'),
            $button = $('#' + $id + ' .cmtbtn a'),
            $info = $('#' + $id + ' .cmtbtn .cmtinfo'),
            $fullName = $('#' + $id + ' .cmtbtn input.cmtname'),
            $email = $('#' + $id + ' .cmtbtn input.cmtemail'),
            $delete = $('#' + $id + ' .miniclose');

        $placeHolder.unbind('click');
        $button.unbind('click');
        $placeHolder.click(function () {
            $placeHolder.hide();
            $button.addClass('disabled');
            $info.show();
            $textarea.show().animate({
                height: '+=45'
            }, 400, function () {
                $textarea.focus();
                $textarea.bind('keypress', isDirtyHandler);
            });
        });
        $delete.bind('click', deleteComment);

        $textarea.bind('paste', function (e) {
            setTimeout(function () {
                $textarea.val($textarea.val());
                if ($textarea.val().length > $maxLength) {
                    var txt = $textarea.val().substring(0, $maxLength);
                    $textarea.val(txt);
                }
            }, 100);
            isDirtyHandler(e);

        });
        function deleteComment() {
            if (confirm(journalOptions.deleteConfirmText)) {
                jid = $(this).parent().attr('id').replace('cmt-', '');
                $.ajax({
                    type: "POST",
                    url: journalOptions.siteRoot + '/' + journalItem.Object + '/Services/CommentService.asmx/Delete',
                    //beforeSend: sf.setModuleHeaders,
                    data: { commentId: jid },
                    success: function (data) {
                    },
                    error: function (xhr, status, error) {
                        alert(error);
                    }
                });

                $(this).parent().fadeOut(function () {
                    var p = $(this).parent();
                    var id = $(this).attr('id');
                    $(this).animate({
                        height: '0'
                    }, 400, function () {
                        p.remove('#' + id);
                    });
                });
            }
        }
        var isDirtyHandler = function (event) {
            $button.removeClass('disabled');
            $textarea.unbind('keypress', isDirtyHandler);
        };

        $button.click(function (event) {
            event.preventDefault();
            var jid = $id.replace('jcmt-', '');
            var data = {};
            data.itemId = journalOptions.itemId;
            data.commentId = jid;
            data.Comment = encodeURIComponent($textarea.val());
            data.fullName = encodeURIComponent($fullName.val());
            data.email = encodeURIComponent($email.val());
            var tmpValue = $textarea.val();
            tmpValue = tmpValue.replace(/<(?:.|\n)*?>/gm, '').replace(/\s+/g, '').replace(/&nbsp;/g, '');
            if (tmpValue == '') {
                alert(journalOptions.contentInvalidText);
                $textarea.focus();
                return false;
            }
            if (data.Comment == '' || data.Comment == '%3Cbr%3E') {
                alert(journalOptions.contentRequiredText);
                $textarea.focus();
                return false;
            }
            if (data.fullName == '') {
                alert(journalOptions.fullNameRequiredText);
                $fullName.focus();
                return false;
            }
            //            if (data.email != '') {
            //                var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            //                if (!regex.test(data.email)) {
            //                    alert(journalOptions.emailInvalidText);
            //                    $email.focus();
            //                    return false;
            //                }
            //            }

            $.ajax({
                type: "POST",
                //headers: { 'RequestVerificationToken': journalOptions.token },
                url: journalOptions.siteRoot + '/' + journalItem.Object + '/Services/CommentService.asmx/CommentSave',
                //beforeSend: sf.setModuleHeaders,
                data: { itemId: data.itemId, commentType: $(this).parents('.commentpanel').attr('data-type'), commentId: data.commentId, fullName: data.fullName, email: data.email, content: data.Comment },
                success: function (res) {
                    if (res.success)
                        commentComplete(res.response);
                    else
                        alert(res.response); 
                },
                error: function (xhr, status, error) {
                    alert(error);
                }
            });
        });
        function commentComplete(data) {
            $textarea.animate({
                //height: '0'
            }, 400, function () {
                $button.addClass('disabled');
                //$info.hide();
                $textarea.val('');//.hide();
                $placeHolder.show();
            });

            if (data != '' && data != ' ') {
                var li = $(data);
                li.insertBefore('#' + $id + ' .cmteditarea');
                $(li).find('.miniclose').bind('click', deleteComment);
                $(li).find('.cmlike').bind('click', likeComment);
            }
        }
    }

    //    $("#getMore").bind('click', function (e) {
    //        getItems(sf);
    //        e.preventDefault();
    //    });
    //    function getItems(){
    //        var rows = $(".journalrow").get();
    //    
    //        $.ajax({
    //            type : "POST",
    //            url : journalOptions.siteRoot + '/' + journalItem.Object + '/Services/CommentService.asmx/GetList',
    //            data : {itemId: journalOptions.itemId, parentId: -1, pageNumber: rows.length + 1, pageSize: journalOptions.pageSize},
    //            success : function(data){
    //                if(data.length > 0){
    //                    $(".journalItems").append(data);
    //                    pluginInit();
    //                    var newRows = $(".journalrow").get();
    //                    var diff = (newRows.length - rows.length);
    //                    if(diff < journalOptions.pageSize){ 
    //                        $("#getMore").hide();
    //                    }

    //                    //to change the browser URL to 'pageurl'
    //                    if (pageurl != window.location) {
    //                        window.history.pushState({ path: pageurl }, '', pageurl);
    //                    }
    //                }
    //                if(typeof(callback) != "undefined"){
    //                    callback(data);
    //                }
    //            },
    //            error : function(xhr, status, error){
    //                alert(error);
    //            }
    //        });
    //    }

    if ($('.commentpanel').length) {
        pluginInit();
        $('body').journalTools(jopts);
        if ($('.rating-input').length) {
            $('.rating-input').raty({
                path: '/Data/Sites/1/skins/default/js/rating/',
                score: function () { return $(this).attr('data-score') }
            })
        }
    }

}(jQuery, window));

$(function () {
    $('body').on('click', '.cmreply', function (e) {
        e.preventDefault()
        $(this).parents('.cmt-item').find('.cmteditarea').slideDown()
        $(this).parents('.cmt-item').find('.cmtbtn').slideDown()
    })
});