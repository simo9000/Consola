/// <reference path="~/scripts/linq.js"/>
/// <reference path="~/scripts/linq-vsdoc.js"/>
/// <reference path="~/scripts/jquery-2.2.1.js"/>
/// <refernece path="~/scripts/jquery-2.1.0-vsdoc.js"/>

commandPrompt = Backbone.View.extend({

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
        var console = this;
        $.connection.hub.start({ transport: 'longPolling' }).done(function () {
            console.createNewLine({
                hub: console.hub
            });
        });
        this.hub.client.pushOutput = function (text) {
            console.activeCommand.appendText(text);
        };
    },

    createNewLine: function () {
        this.activeCommand = new commandLine({
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

    addNewLine: function () {
        this.createNewLine();
        this.activeCommand.render();
        this.$el.find('tbody').append(this.activeCommand.$el);
        this.focus();
    },

    focus: function () {
        this.activeCommand.focus();
    }
});