var underscore = require('underscore');
//global.jQuery = require('jquery');
//global.$ = jQuery;
var Backbone = require('backbone');
Backbone.$ = $;
var signalR = require('ms-signalr-client');
var commandLine = require('./commandLine');

module.exports = Backbone.View.extend({

    events: {
        "click .spaceHolder" : "focus"
    },

    tagName: 'div',

    template: _.template('<table>\
                            <thead>\
                            </thead>\
                            <tbody>\
                            </tbody>\
                          </table>\
                          <div class="spaceHolder" style="height:100%"/>'),

    initialize: function () {
        this.hub = $.connection.consoleHub;
        this.history = [];
        var console = this;
        $.connection.hub.start({ transport: 'longPolling' }).done(function () {
            console.createNewLine({
                hub: console.hub
            });
        });
        this.hub.client.pushOutput = function (text) {
            console.activeCommand.appendText(text,true);
        };
    },

    createNewLine: function () {
        var view = this;
        this.activeCommand = new commandLine({
            getFromHistory: function(index){
                return view.history[index];
            },
            hub: this.hub
        });
        this.listenTo(this.activeCommand, 'newLine', this.addNewLine);
    },

    render: function () {
        this.$el.html(this.template({}));
        this.$el.css('height', '100vh');
        this.$el.css('width', '100vw');
        this.addNewLine();
    },

    addNewLine: function (previousLine) {
        if (previousLine != null)
            this.history.unshift(previousLine);
        this.createNewLine();
        this.activeCommand.render();
        this.$el.find('tbody').append(this.activeCommand.$el);
        this.focus();
    },

    focus: function () {
        this.activeCommand.focus();
    }
});