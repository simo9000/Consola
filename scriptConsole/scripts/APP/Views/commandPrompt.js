/// <reference path="~/scripts/linq.js"/>
/// <reference path="~/scripts/linq-vsdoc.js"/>
/// <reference path="~/scripts/jquery-2.2.1.js"/>
/// <refernece path="~/scripts/jquery-2.1.0-vsdoc.js"/>

commandPrompt = Backbone.View.extend({

    tagName: 'tbody',

    createNewLine: function () {
        this.activeCommand = new commandLine({});
        this.listenTo(this.activeCommand, 'newLine', this.addNewLine);
    },

    initialize: function () {
        this.createNewLine();
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