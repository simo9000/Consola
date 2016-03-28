/// <reference path="~/scripts/linq.js"/>
/// <reference path="~/scripts/linq-vsdoc.js"/>
/// <reference path="~/scripts/jquery-2.2.1.js"/>
/// <refernece path="~/scripts/jquery-2.1.0-vsdoc.js"/>
var ENTER_KEY = 13;
var commandLine = Backbone.View.extend({

    tagName: 'tr',

    initialize: function() {
        this.prompt = "$>";
    },

    events: {
        "keyup .command": "CRLF"
    },

    CRLF: function(e){
        var view = this;
        if (e.which === ENTER_KEY) {
            var line = this.$el.find('.command').val();
            $.get('/templates/finishedCmdLine.html', function (template) {
                var renderedLine = Mustache.render(template, {
                    prompt: view.prompt,
                    content: line
                });
                view.$el.html(renderedLine);
                view.$el.find('.command').focus();
                view.trigger('newLine');
            });
        }
    },

    render: function () {
        var view = this;
        $.get('/templates/emptyCmdLine.html', function (template) {
            var renderedView = Mustache.render(template, {
                prompt: view.prompt
            });
            view.$el.html(renderedView);
            view.$el.find('.command').focus();
        });
        return this;
    }
});

var commandPrompt = Backbone.View.extend({

    tagName: 'tbody',

    createNewLine: function() {
        this.activeCommand = new commandLine({});
        this.listenTo(this.activeCommand, 'newLine', this.addNewLine);
    },

    initialize: function () {
        this.createNewLine();
    },

    render: function (opt) {
        this.activeCommand.render();
        this.$el.html(this.activeCommand.el);
    },

    addNewLine: function () {
        this.createNewLine();
        this.activeCommand.render();
        this.$el.append(this.activeCommand.el);
    }
});


$(document).ready(function () {
    var prompt = new commandPrompt({});
    prompt.render();
    $('#console').append(prompt.el);
});