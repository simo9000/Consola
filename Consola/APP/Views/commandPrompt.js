global.underscore = require('underscore');
global._ = underscore;
global.jQuery = require('jquery');
global.$ = jQuery;
var Backbone = require('backbone');
Backbone.$ = $;
var signalR = require('ms-signalr-client');
var commandLine = require('./commandLine');
var editor = require('./editor');
var lineManager = require('../lineManager');
var download = require('downloadjs');
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
                          </table>'),

    initialize: function(e) {
        this.showEditor = e.showEditor;
        this.editorVisible = false;
        this.guid = this.generateGUID();
        var prompt = this;
        this.editor = editor;
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
                    if (!prompt.testDownload) {
                        var x = new XMLHttpRequest();
                        x.open("GET", downloadPath, true);
                        x.responseType = 'blob';
                        x.onload = function(e, o) {
                            var fileName = x.getResponseHeader('Content-Disposition').replace('attachment; filename=', '');
                            download(x.response, fileName, e.target.response.type);
                            console.hub.server.confirmDownload(key);
                        }
                        x.send();
                    }
                    else {
                        $('body').append('<iframe src="' + downloadPath + '"></iframe>');
                    }
                };
                console.hub.client.newLine = function() {
                    if (!console.activeCommand.active)
                        prompt.addNewLine();
                    if (prompt.showEditor && !prompt.editorVisible)
                        prompt.renderEditor();
                }
                $.connection.hub.url = signalRLocation;
                $.connection.hub.start({ transport: 'longPolling', jsonp: true }).done(function() {
                    prompt.connectionID = $.connection.hub.id;
                    console.addNewLine();
                    console.hubStatus = 'connected';
                    prompt.hub.server.consoleReady();
                }); 
       });
    },

    createNewLine: function () {
        var view = this;
        this.activeCommand = new commandLine({
            lineManager: this.lineManager
        });
        this.listenTo(this.activeCommand, 'newLine', this.addNewLine);
        this.listenTo(this.activeCommand, 'submit', this.submit);
        this.listenTo(this.activeCommand, 'clearPrevious', this.clearConsole);
    },

    submit: function(code,guid,callback) {
        $.ajax({
            url: this.createHostPath("/Console/Command/" + guid || this.guid),
            method: 'POST',
            data: code,
            headers: {
                'CONNECTION_ID' : this.connectionID
            },
            success: function() {
                if (callback)
                    callback();
            }
        });
    },

    render: function () {
        this.$el.html(this.template({}));
        return this;
    },

    addNewLine: function () {
        this.createNewLine();
        this.activeCommand.render();
        this.$el.find('tbody').append(this.activeCommand.$el);
        this.activeCommand.el.scrollIntoView(true);
        this.activeCommand.focus();
    },

    createHostPath: function (path) {
        var scriptLocation = '/app/output/bundle.js';
        var signalRLocation = document.querySelector('script[src*="' + scriptLocation + '"]');
        return signalRLocation.src.replace(scriptLocation,path);
    },

    renderEditor: function() {
        var prompt = this;
        this.editor = new this.editor({
            cmd : this
        });
        this.listenTo(this.editor, 'submit', this.submit);
        this.listenTo(this.editor, 'resize', this.resize);
        this.editor.render();
        this.$el.prepend(this.editor.$el);
        this.windowed = true;
        this.resize(prompt);
        var resizeInt = null;
        this.editor.$el.on('mousedown', function(e) {
            resizeInt = setInterval(function() { prompt.resize(prompt); }, 1000 / 15);
        });
        this.editor.$el.on('mouseup', function(e) {
            if (resizeInt !== null) {
                clearInterval(resizeInt);
            }
            prompt.resize(prompt);
        });
        prompt.editorVisible = true;
    },

    resize: function(prompt) {
        var parentHeight = prompt.$el.parent().height();
        prompt.$el.find('.consola').height(parentHeight -prompt.editor.$el.height());
    },

    generateGUID: function() {
        function s4() {
            return Math.floor((1 + Math.random()) * 0x10000)
              .toString(16)
              .substring(1);
        }
        return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
          s4() + '-' + s4() + s4() + s4();
    },

    clearConsole: function() {
        this.$el.find('.finished').parent().parent().remove();
    }
});