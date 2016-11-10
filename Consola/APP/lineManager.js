var underscore = require('underscore');
var finishedLine = require('./Views/finishedLine');

module.exports = function lineManager() {

    var lines = 0;
    var commands = [];
    var currentCommand;

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
        currentCommand = commands.length;
    },

    this.getPreviousCommand = function() {
        if (commands.length > 0 && currentCommand > 0) {
            currentCommand--;
            return commands[currentCommand];
        }
        else if (currentCommand != 0)
            return '';
    },

    this.getNextCommand = function() {
        if (commands.length > 0) {
            if (currentCommand < commands.length - 1) {
                currentCommand++;
                return commands[currentCommand];
            }
            else if (currentCommand == commands.length - 1) {
                currentCommand++;
                return '';
            }
        }
    }
}

