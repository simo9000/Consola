/// <reference path="~/scripts/linq.js"/>
/// <reference path="~/scripts/linq-vsdoc.js"/>
/// <reference path="~/scripts/jquery-2.2.1.js"/>
/// <refernece path="~/scripts/jquery-2.1.0-vsdoc.js"/>
var commandLine = Backbone.View.extend({

    tagName: 'li',

    initialize: function() {
        
    },

    render: function (opts) {
        var view = this;
        $.get('/templates/emptyCmdLine.html', function (template) {
            var renderedView = Mustache.render(template, {
                prompt: "$>"
            });
            view.$el.html(renderedView);
        });
        return this;
    }
});


$(document).ready(function () {
    var line = new commandLine({});
    line.render();
    $('#console').append(line.el);
});