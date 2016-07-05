/// <reference path="~/scripts/linq.js"/>
/// <reference path="~/scripts/linq-vsdoc.js"/>
/// <reference path="~/scripts/jquery-2.2.1.js"/>
/// <refernece path="~/scripts/jquery-2.1.0-vsdoc.js"/>

commandPrompt = Backbone.View.extend({

    tagName: 'tbody',

    initialize: function () {
        this.hub = $.connection.consoleHub;
        var console = this;
        $.connection.hub.start({ transport: 'longPolling' }).done(function () {
            console.hub.server.registerEngine();
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

    render: function (opt) {
        this.addNewLine();
    },

    addNewLine: function () {
        this.createNewLine();
        this.activeCommand.render();
        this.$el.append(this.activeCommand.$el);
        this.focus();
    },

    focus: function () {
        this.activeCommand.focus();
    }
});