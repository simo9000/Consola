var underscore = require('underscore');
var Backbone = require('backbone');
Backbone.$ = $;
require('../Global');

module.exports = Backbone.View.extend({

    events: {
        "dblclick": "rename",
        "blur .rename": "save",
        "keydown .rename": "keyCheck",
        "click .delete" : "deleteTab"
    },

    tagName: 'li',

    initialize: function(e){
        this.name = e.name;
        this.isSaved = e.isSaved;
    },

    template: _.template('<span class="title" style="min-width:50px"><%= NAME %></span>\
                          <span class="ast"><%= SAVED ? "" : "*" %></span>\
                          <span class="delete" style="border: solid 1px red;color:red;display:none">X</span>'),

    render: function() {
        this.$el.html(this.template({
            NAME: this.name,
            SAVED: this.isSaved
        }));
        this.$el.addClass('active');
        return this;
    },

    keyCheck: function(e) {
        if (e.keyCode == ENTER_KEY)
            this.save(e);
    },

    rename: function() {
        var titleHolder = this.$el.find('.title');
        titleHolder.css('border', 'solid 1px white');
        titleHolder.attr('contenteditable', 'true');
        titleHolder.addClass('rename')
        this.trigger('freeze');
        titleHolder.focus();
    },

    save: function(e) {
        var reName = this.$el.find('.rename');
        var newName = reName.text();
        this.trigger('save', newName,this);
        if (this.isSaved) {
            var titleHolder = this.$el.find('.title');
            titleHolder.css('border', '');
            titleHolder.attr('contenteditable', 'false');
            titleHolder.removeClass('rename');
            this.$el.find('.ast').hide();
            this.trigger('unfreeze');
        }
        else {
            e.preventDefault();
            reName.focus();
        }
    },

    deleteTab: function() {
        if (window.confirm("Are you sure you want to delete?"))
            this.trigger('delete', this);
    }

});
