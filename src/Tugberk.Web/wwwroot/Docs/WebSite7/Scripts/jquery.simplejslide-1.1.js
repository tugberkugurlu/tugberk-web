/*!
 * jQuery simpleJSlide slideshow plugin v1.1
 * http://tugberkugurlu.com
 * 
 * This jQuery plugin has been created by Tugberk Ugurlu
 * Contact: http://tugberkugurlu.com/contact, http://twitter.com/tourismgeek
 * 
 * Nuget Package
 * PM> install-package jquery.simpleJSlide
 * http://nuget.org/List/Packages/jquery.simpleJSlide
 *
 * No Copyright but if it was, it would be like : Copyright 2011, Tugberk Ugurlu
 * 
 * Date : 2011, 05.24 04:25 AM in Turkey, Mugla
 */

/// <reference path="jquery-1.6.1-vsdoc.js" />

(function ($) {

    $.fn.simpleJSlide = function (settings) {

        var config = {
            actionactive: false,
            notificationactive: false,
            playbuttonid: "simpleJSlidePlayButton",
            stopbuttonid: "simpleJSlideStopButton",
            notificationcontainerid : "notificationContainer",
            playingtext : "Now playing...",
            stopedtext : "Stopped..."
        };

        var intervalInstance = null,
            interval = 2,
            current = 0,
            last = 0;

        var $container = this;
        var $thumbContainer = this.find("#thumbContainer");
        var $containerImage = this.find("#containerImage");
        var $totalImages = $thumbContainer.find("img").length;

        $.extend(config, settings);

        $containerImage.attr("src", $thumbContainer.find("img:first").attr("data-ui-simpleJSlide-imgSrc"));

        $thumbContainer.find("img").click(function () {

            var _attrsrc = $(this).attr("data-ui-simpleJSlide-imgSrc");

            $containerImage.fadeOut(function () {

                var instance = $(this);

                if (instance.attr("src") !== _attrsrc) {

                    instance.attr('src', _attrsrc);
                    instance.load(function () {
                        instance.fadeIn();
                    });

                } else {
                    instance.fadeIn();
                }

            });

        });

        $("#" + config.playbuttonid).click(function () {

            if (config.actionactive) {

                if (intervalInstance == null) {

                    if (config.notificationactive)
                        $("#" + config.notificationcontainerid).text(config.playingtext);

                    intervalInstance = window.setInterval(function () {
                        iterate(current + 1, $containerImage, $thumbContainer);
                    }, interval * 1000);

                }
            }

        });

        $("#" + config.stopbuttonid).click(function () {

            if (config.notificationactive)
                $("#" + config.notificationcontainerid).text(config.stopedtext);
            
            if (config.actionactive) {

                window.clearInterval(intervalInstance);
                intervalInstance = null;

            }
        });

        var iterate = function (index, mainImage, _thumbContainer) {

            if (index < 0) {
                index = $totalImages - 1;
            }

            if (index >= $totalImages) {
                index = 0;
            }

            if (index === current) return;

            mainImage.fadeOut(function () {
                var _instance = $(this);
                _instance.load(function () { _instance.fadeIn(); });
                _instance.attr('src', _thumbContainer.find('img:eq(' + index + ')').attr('data-ui-simpleJSlide-imgSrc'));

            });

            last = current;
            current = index;

        };

    };

})(jQuery)