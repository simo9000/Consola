var underscore = require('underscore');
var Backbone = require('backbone');
Backbone.$ = $;
require('../Global');

module.exports = Backbone.View.extend({

    events: {
        "dblclick" : "rename",
    },

    tagName: 'li',

    initialize: function(e){
        this.name = e.name;
        this.isSaved = e.isSaved;
    },

    template: _.template('<span class="title"><%= NAME %></span>\
                          <span class="ast"><%= SAVED ? "*" : "" %></span>'),

    render: function() {
        this.$el.html(this.template({
            NAME: this.name,
            SAVED: this.isSaved
        }));
        this.$el.addClass('active');
        return this;
    },

    rename: function() {
        var titleHolder = this.$el.find('.title');
        var title = titleHolder.text();
        titleHolder.replaceWith('<input class="rename" type="text"></input>');
        titleHolder.val(title);
    }

});
