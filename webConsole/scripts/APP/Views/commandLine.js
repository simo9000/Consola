/// <reference path="~/scripts/jquery-2.2.1.js"/>
/// <refernece path="~/scripts/jquery-2.1.0-vsdoc.js"/>


commandLine = Backbone.View.extend({

    tagName: 'tr',

    initialize: function (e) {
        this.prompt = "$>";
        this.hub = e.hub
    },

    events: {
        "keyup .command": "keyUp",
        "keydown .command": "keyDown"
    },

    emptyTemplate: _.template('<td style="font-size:17px;color:white;"><%= prompt %></td>\
                          <td><textarea spellcheck="false" class="command" style="font-size:15px;color:white;background-color:black;" rows="1"></textarea></td>'),

    finishedTemplate: _.template('<span style="font-size:15px;color:white;"><%= content %></span></br>'),

    keyDown: function(e){
        if (e.keyCode == 9) {
            e.preventDefault();
            //http://stackoverflow.com/questions/6637341/use-tab-to-indent-in-textarea
            //http://jsfiddle.net/jz6J5/
            //Adapted for Backbone.js
            var textArea = this.$el.find('.command');
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
    },

    keyUp: function (e) {
        var view = this;
        if (e.which === ENTER_KEY && !e.shiftKey) {
            view.addNewLine(true);
        }
        else {
            var textArea = this.$el.find('.command');
            var line = textArea.val();
            var numberOfLines = this.numberOfLines(line);
            textArea.attr('rows', numberOfLines);
        }
    },

    addNewLine: function (submit) {
        var view = this;
        var line = this.$el.find('.command').val();
        var lines = line.split(/\n/g);
        if (lines[lines.length - 1] == '') lines.pop();
        lines = _.map(lines, function (l) {
            return new finishedLine({
                contents: l
            }).render().$el;
        });
        view.$el.find('textarea').replaceWith(lines);
        if (submit) this.hub.server.submitCommand(line);
        view.trigger('newLine');
    },

    render: function () {
        var view = this;
        view.$el.html(this.emptyTemplate({
            prompt: this.prompt
        }));
        this.focus();
        return this;
    },

    focus: function () {
        this.$el.find('.command').focus();
    },

    numberOfLines: function(line){
        return (line.match(/\n/g) || []).length + 1;
    },

    appendText: function (text) {
        var crlf = "\r\n";
        if (text == crlf)
            this.addNewLine(false);
        else
            this.$el.find('.command').append(text);
    }
});