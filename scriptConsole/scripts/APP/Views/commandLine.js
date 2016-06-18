/// <reference path="~/scripts/linq.js"/>
/// <reference path="~/scripts/linq-vsdoc.js"/>
/// <reference path="~/scripts/jquery-2.2.1.js"/>
/// <refernece path="~/scripts/jquery-2.1.0-vsdoc.js"/>


commandLine = Backbone.View.extend({

    tagName: 'tr',

    initialize: function () {
        this.prompt = "$>";
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
            var line = this.$el.find('.command').val();
            view.$el.find('textarea').replaceWith(this.finishedTemplate({
                content: line
            }));
            view.trigger('newLine');
        }
    },

    render: function () {
        var view = this;
        view.$el.html(this.emptyTemplate({
            prompt: this.prompt
        }));
        view.$el.find('.command').focus();

        return this;
    },

    focus: function () {
        this.$el.find('.command').focus();
    }
});