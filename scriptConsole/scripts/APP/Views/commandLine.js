﻿/// <reference path="~/scripts/linq.js"/>
/// <reference path="~/scripts/linq-vsdoc.js"/>
/// <reference path="~/scripts/jquery-2.2.1.js"/>
/// <refernece path="~/scripts/jquery-2.1.0-vsdoc.js"/>


commandLine = Backbone.View.extend({

    tagName: 'tr',

    initialize: function (e) {
        this.prompt = "$>";
        this.hub = e.hub
    },

    events: {
        "keyup .command": "CRLF"
    },

    emptyTemplate: _.template('<td style="font-size:17px;color:white;"><%= prompt %></td>\
                          <td><textarea spellcheck="false" class="command" style="font-size:15px;color:white;background-color:black;" rows="1"></textarea></td>'),

    finishedTemplate: _.template('<span style="font-size:15px;color:white;" rows="1"><%= content %></span>'),


    CRLF: function (e) {
        var view = this;
        if (e.which === ENTER_KEY) {
            view.addNewLine(true);
        }
    },

    addNewLine: function (submit) {
        var view = this;
        var line = this.$el.find('.command').val();
        view.$el.find('textarea').replaceWith(this.finishedTemplate({
            content: line
        }));
        if (submit) this.hub.server.submitCommand(line);
        view.trigger('newLine');
    },

    render: function () {
        var view = this;
        view.$el.html(this.emptyTemplate({
            prompt: this.prompt
        }));
        this.focus();
        return this;
    },

    focus: function () {
        this.$el.find('.command').focus();
    },

    appendText: function (text) {
        var crlf = "\r\0\n\0";
        if (text == crlf)
            this.addNewLine(false);
        else
            this.$el.find('.command').append(text);
    }
});