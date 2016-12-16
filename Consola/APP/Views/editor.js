var underscore = require('underscore');
var Backbone = require('backbone');
var codeFile = require('./codeFile');
Backbone.$ = $;
require('../Global');

module.exports = Backbone.View.extend({

    template: _.template('<ul class="tab-menu">\
                            <li>New</li>\
                          </ul>\
                          <div class="consolaClear"</div>'),

    initialize: function() {

    },

    render: function() {
        this.$el.html(this.template({}));
        this.$el.addClass('consolaTab-container');
        var testFile = new codeFile({});
        testFile.render();
        this.$el.append(testFile.$el);
        return this;
    }

});