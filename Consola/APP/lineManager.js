var underscore = require('underscore');
var finishedLine = require('./Views/finishedLine');

module.exports = function lineManager() {

    var lines = 0;
    var commands = [];

    this.createFinishedLine = function(line) {
        lines++;
        return new finishedLine({
            contents: line,
            lineNumber: lines
        }).render().$el;
    },

    this.createFinishedHtmlLine = function(element) {
        lines++;
        element.addClass('LN' + lines);
        return element;
    },

    this.appendCommand = function(command) {
        command = command.slice(0, command.length - 1);
        commands.push(command);
    }
}

