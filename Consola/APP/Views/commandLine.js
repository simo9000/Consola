var underscore = require('underscore');
var Backbone = require('backbone');
Backbone.$ = $;
require('../Global');


module.exports = Backbone.View.extend({

    tagName: 'tr',

    initialize: function (e) {
        this.prompt = "$>";
        this.lineManager = e.lineManager;
        this.active = true;
        if (window.attachEvent) {
            this.observe = function(element, event, handler) {
                element.attachEvent('on' + event, handler);
            };
        }
        else {
            this.observe = function(element, event, handler) {
                element.addEventListener(event, handler, false);
            };
        }
    },

    events: {
        "keyup .consolaCommand": "keyUp",
        "keydown .consolaCommand": "keyDown"
    },

    emptyTemplate: _.template('<td class="consolaPromptSymbol" style="font-size:17px;color:white;"><%= prompt %></td>\
                          <td style="width:97%"><textarea spellcheck="false" maxlength="30000000" class="consolaCommand"></textarea></td>'),

    keyDown: function(e){
        if (e.keyCode == TAB_KEY) {
            e.preventDefault();
            //http://stackoverflow.com/questions/6637341/use-tab-to-indent-in-textarea
            //http://jsfiddle.net/jz6J5/
            //Adapted for Backbone.js
            var textArea = this.$el.find('.consolaCommand');
            var start = textArea.get(0).selectionStart;
            var end = textArea.get(0).selectionEnd;

            // set textarea value to: text before caret + tab + text after caret
            textArea.val(textArea.val().substring(0, start)
                        + "\t"
                        + textArea.val().substring(end));

            // put caret at right position again
            textArea.get(0).selectionStart =
            textArea.get(0).selectionEnd = start + 1;
        }
        else if (e.keyCode == ENTER_KEY && !e.shiftKey) {
            e.preventDefault();
        }
    },

    keyUp: function (e) {
        var view = this;
        var textArea = this.$el.find('.consolaCommand');
        var line = textArea.val();
        var historyFetch = function(fetchCallback) {
            e.preventDefault();
            var historyCommand = view.lineManager[fetchCallback]();
            if (historyCommand != undefined) {
                view.replaceText(historyCommand);
                view.focus();
            }
        }
        if (e.which === ENTER_KEY && !e.shiftKey) {
            view.addNewLine(true);
        }
        else if (e.ctrlKey && e.keyCode == S_KEY) {
            e.preventDefault();
        }
        else if (e.ctrlKey && e.keyCode == C_KEY) {
            this.trigger('clearPrevious');
        }
        else if (e.which === UP_KEY) {
            historyFetch('getPreviousCommand');
        }
        else if (e.which === DOWN_KEY) {
            historyFetch('getNextCommand');
        }
    },

    addNewLine: function (submit) {
        var view = this;
        var line = this.$el.find('.consolaCommand').val();
        var lines = line.split(/\n/g);
        if (lines[lines.length - 1] == '') lines.pop();
        lines = _.map(lines, function (l) {
            return view.lineManager.createFinishedLine(l);
        });
        if (!submit)
            view.$el.find('.consolaPromptSymbol').text('');
        view.$el.find('textarea').replaceWith(lines);
        if (submit) {
            view.lineManager.appendCommand(line);
            view.trigger('submit', line);
        }
        this.active = false;
    },

    render: function () {
        var view = this;
        view.$el.html(this.emptyTemplate({
            prompt: this.prompt
        }));
        this.focus();
        return this;
    },

    focus: function() {
        var view = this;
        var text = view.$el.find('.consolaCommand')[0];
        function resize() {
            text.style.height = 'auto';
            text.style.height = text.scrollHeight + 'px';
        }
        function delayedResize() {
            window.setTimeout(resize, 0);
        }
        view.observe(text, 'change', resize);
        underscore.each(['cut', 'paste', 'drop', 'keydown'], function(e) {
            view.observe(text, e, delayedResize);
        });
        text.focus();
        resize();
    },

    numberOfLines: function(line){
        //return (line.match(/\n/g) || []).length + 1;
        var textarea = this.$el.find('.consolaCommand');
        var lht = parseInt(textarea.css('lineHeight'), 10);
        var lines = textarea.attr('scrollHeight') / lht;
        return lines;
    },

    appendText: function (text, isExternal) {
        var crlf = "\r\n";
        if (text == crlf && isExternal)
            this.addNewLine(false);
        else
            this.$el.find('.consolaCommand').append(text);
    },

    appendHTML: function(html) {
        var view = this;
        var element = $('<div style="color:white;">' + html + '</div>');
        element = view.lineManager.createFinishedHtmlLine(element);
        view.$el.find('.consolaPromptSymbol').text('');
        view.$el.find('textarea').replaceWith(element);
        view.trigger('newLine', null);
    },

    replaceText: function (text) {
        this.$el.find('.consolaCommand').val(text);
    }
});