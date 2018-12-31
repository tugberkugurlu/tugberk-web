/// <reference path="jquery-ui-1.8.13.min.js" />
/// <reference path="jquery-1.6.1-vsdoc.js" />

(function ($) {

    $(function () {

        //Wire-up jQuery UI unobtrusively
        $("*[data-ui-fn]").each(function () {
            var el = this,
                $el = $(this);

            //loop through functions in data-ui-fn attribute
            $.each($el.attr("data-ui-fn").split(" "), function () {
                var fn = this,
                    options = {},
                    optionPrefix = "";

                optionPrefix = "data-ui-" + fn + "-";

                //Build options parameter from data-ui-* attributes
                $.each(el.attributes, function () {
                    var attr = this;

                    if (attr.name.toLowerCase().indexOf(optionPrefix.toLowerCase()) === 0) {
                        options[attr.name.substr(optionPrefix.length)] = attr.value;
                    }

                });

                //Call Jquery UI fn if it exists
                ($el[fn] || $.noop).call($el, options);

            });

        });

    });

} (jQuery))