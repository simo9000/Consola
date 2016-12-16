var underscore = require('underscore');
var Backbone = require('backbone');
Backbone.$ = $;
require('../Global');

module.exports = Backbone.View.extend({

    template: _.template('<textarea spellcheck="false" maxlength="30000000" class="consolaCommand" style="height:30vh;border-style:solid;resize:vertical;width:99.5%"></textarea>'),

    render: function() {
        this.$el.html(this.template(({})));
        this.$el.addClass('active');
    }

});