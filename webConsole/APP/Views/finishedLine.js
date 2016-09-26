var Backbone = require('backbone');
Backbone.$ = $;

module.exports = Backbone.View.extend({

    tagName: 'span',

    initialize: function (e) {
        this.line = e.contents;
    },

    render: function () {
        this.$el.css('font-size', '15px');
        this.$el.css('color', 'white');
        this.$el.css('white-space', 'pre');
        this.line = this.line.replace("/t", "&nbsp;&nbsp;&nbsp;&nbsp;");
        this.$el.append(document.createTextNode(this.line));
        this.$el.append('</br>');
        return this;
    }

});