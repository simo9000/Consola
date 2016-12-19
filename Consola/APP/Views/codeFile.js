var underscore = require('underscore');
var Backbone = require('backbone');
Backbone.$ = $;
require('../Global');

module.exports = Backbone.View.extend({

    events: {
        "keydown textarea" : "keyCheck"
    },

    template: _.template('<textarea spellcheck="false" maxlength="30000000" class="consolaCommand" style="height:30vh;border-style:solid;resize:vertical;width:99.5%"></textarea>'),

    initialize: function(e){
        this.GUID = e.id;
        this.running = false;
    },

    render: function() {
        this.$el.html(this.template(({})));
        this.$el.addClass('codefile');
    },

    keyCheck: function(e) {
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
        else if (e.ctrlKey && e.keyCode == S_KEY){

        }
        else if (e.keyCode == F5_KEY) {
            e.preventDefault();
            this.trigger('run', this);
        }
    },

    disableEdit: function() {
        var textarea = this.$el.find('textarea');
        textarea.css('color', 'grey');
        textarea.prop('disabled', true);
    },

    enableEdit: function() {
        var textarea = this.$el.find('textarea');
        textarea.css('color', '');
        textarea.prop('disabled',false);
    },

    getCode: function() {
        return this.$el.find('textarea').val();
    },

    focus: function() {
        this.$el.find('textarea').focus();
    }

});