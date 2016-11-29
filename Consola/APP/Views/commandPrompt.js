global.underscore = require('underscore');
global._ = underscore;
global.jQuery = require('jquery');
global.$ = jQuery;
var Backbone = require('backbone');
Backbone.$ = $;
var signalR = require('ms-signalr-client');
var commandLine = require('./commandLine');
var lineManager = require('../lineManager');
require('jquery-file-download');
var Promise = require('promise');

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
        this.testDownload = false;
        this.lineManager = new lineManager();
        this.hubStatus = 'starting';
        var signalRLocation = prompt.createHostPath('/signalr/js');
        var asyncLoad = Promise.all([new Promise(function(resolve, reject) {
            $.ajax({
                url: signalRLocation,
                dataType: 'script',
                success: resolve
            });
        }), new Promise(function(resolve, reject){
            var cssLocation = prompt.createHostPath('/app/Consola.css');
            $.ajax({
                url: cssLocation,
                dataType: 'text',
                success: function(data) {
                    $('<style type="text/css">\n' + data + '</style>').appendTo("head");
                    resolve();
                }
            });
        })]).then( function() {
                console.hub = $.connection.consoleHub;
                console.hub.client.pushOutput = function(text) {
                    if (!console.activeCommand.active)
                        console.addNewLine();
                    console.activeCommand.appendText(text, true);
                };
                console.hub.client.pushHtmlOutput = function(html) {
                    if (!console.activeCommand.active)
                        console.addNewLine();
                    console.activeCommand.appendHTML(html);
                };
                console.hub.client.initiateDownload = function(key) {
                    var downloadPath = prompt.createHostPath('/Console/Download/' + key);
                    if (!prompt.testDownload)
                        $.fileDownload(downloadPath, {
                            successCallback: function(url) {
                                console.hub.server.confirmDownload(key);
                            }
                        });
                    else {
                        $('body').append('<iframe src="' + downloadPath + '"></iframe>');
                    }
                };
                console.hub.client.newLine = function() {
                    prompt.addNewLine();
                }
                $.connection.hub.url = signalRLocation;
                $.connection.hub.start({ transport: 'longPolling', jsonp:true }).done(function() {
                    console.addNewLine();
                    console.hubStatus = 'connected';
                    prompt.hub.server.consoleReady();
                }); 
       });
    },

    createNewLine: function () {
        var view = this;
        this.activeCommand = new commandLine({
            hub: this.hub,
            lineManager: this.lineManager
        });
    },

    render: function () {
        this.$el.html(this.template({}));
        this.$el.css('background-color', 'black');
        this.$el.css('height', '100vh');
        this.$el.css('width', '100vw');
    },

    addNewLine: function () {
        this.createNewLine();
        this.activeCommand.render();
        this.$el.find('tbody').append(this.activeCommand.$el);
        this.activeCommand.focus();
    },

    createHostPath: function (path) {
        var scriptLocation = '/app/output/bundle.js';
        var signalRLocation = document.querySelector('script[src*="' + scriptLocation + '"]');
        return signalRLocation.src.replace(scriptLocation,path);
    }
});