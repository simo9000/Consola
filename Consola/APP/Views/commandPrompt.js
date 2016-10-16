global.underscore = require('underscore');
global._ = underscore;
global.jQuery = require('jquery');
global.$ = jQuery;
var Backbone = require('backbone');
Backbone.$ = $;
var signalR = require('ms-signalr-client');
var commandLine = require('./commandLine');
var lineManager = require('../lineManager');

module.exports = Backbone.View.extend({

    events: {
        "click .spaceHolder" : "focus"
    },

    tagName: 'div',

    template: _.template('<table class="consola">\
                            <thead>\
                            </thead>\
                            <tbody>\
                            </tbody>\
                          </table>\
                          <div class="spaceHolder" style="height:100%"/>'),

    initialize: function() {
        var prompt = this;
        this.history = [];
        var console = this;
        this.lineManager = new lineManager();
        this.hubStatus = 'starting';
        var scriptLocation = '/APP/output/bundle.js';
        var signalRLocation = document.querySelector('script[src*="' + scriptLocation + '"]');
        signalRLocation = signalRLocation.src.replace(scriptLocation,'/signalr/js');
        $.ajax({
            url: signalRLocation,
            dataType: 'script',
            success: function() {
                console.hub = $.connection.consoleHub;
                console.hub.client.pushOutput = function(text) {
                    console.activeCommand.appendText(text, true);
                };
                $.connection.hub.start({ transport: 'longPolling' }).done(function() {
                    console.addNewLine();
                    console.hubStatus = 'connected';
                    prompt.hub.server.consoleReady();
                }); 
            }
        });
    },

    createNewLine: function () {
        var view = this;
        this.activeCommand = new commandLine({
            hub: this.hub,
            lineManager: this.lineManager
        });
        this.listenTo(this.activeCommand, 'newLine', this.addNewLine);
    },

    render: function () {
        this.$el.html(this.template({}));
        this.$el.css('height', '100vh');
        this.$el.css('width', '100vw');
    },

    addNewLine: function () {
        this.createNewLine();
        this.activeCommand.render();
        this.$el.find('tbody').append(this.activeCommand.$el);
        this.activeCommand.focus();
    }
});