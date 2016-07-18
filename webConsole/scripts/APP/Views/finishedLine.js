/// <reference path="~/scripts/jquery-2.2.1.js"/>
/// <refernece path="~/scripts/jquery-2.1.0-vsdoc.js"/>


finishedLine = Backbone.View.extend({

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