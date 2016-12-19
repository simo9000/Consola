var underscore = require('underscore');
var Backbone = require('backbone');
var codeFile = require('./codeFile');
var editorTab = require('./editorTab');
Backbone.$ = $;
require('../Global');

module.exports = Backbone.View.extend({

    events: {
        "click #addNew": "newTab",
        "click li": "switchTab",
        "click .consolaStart": "getScriptToExecute",
        "click .consolaStop" : "terminateScript"
    },

    tabTemplate: _.template('<li id="<%= GUID %>_tab" class="active"><%= NAME %></li>'),

    template: _.template('<button class="consolaStart">►</button>\
                          <button class="consolaStop" disabled>■</button>\
                          <ul class="tab-menu">\
                            <li id="addNew" style="font-size:large">+</li>\
                          </ul>\
                          <div class="consolaClear"</div>'),

    initialize: function(e) {
        this.cmd = e.cmd;
        this.codefiles = [];
    },

    render: function() {
        var prompt = this;
        this.$el.html(this.template({}));
        this.$el.addClass('consolaTab-container');
        this.newTab();
        return this;
    },

    newTab: function() {
        var prompt = this;
        var newGUID = this.cmd.generateGUID();
        this.activeTab = newGUID;
        this.$el.find('[id$=_tab]').removeClass('active');
        this.$el.find('ul').append(new editorTab({
            id: newGUID + '_tab',
            name: "untitled" + (this.tabCount || '')
        }).render().$el);
        this.$el.find('ul').append(this.$el.find('#addNew'));
        var codefile = new codeFile({
            id: newGUID
        });
        this.codefiles.push(codefile);
        this.listenTo(codefile, 'run', prompt.executeScript);
        codefile.render();
        this.$el.find('.codefile').hide();
        this.$el.append(codefile.$el);
        if (this.tabCount)
            this.tabCount++;
        else
            this.tabCount = 1;
        codefile.focus();
    },

    switchTab: function(e) {
        var li = e.target.tagName == 'LI' ? e.target : e.target.parentElement;
        if (li.id != "addNew" && li.id != this.activeTab + '_tab') {
            this.$el.find('[id$=_tab]').removeClass('active');
            $(li).addClass('active');
            var id = li.id.replace('_tab', '');
            this.activeTab = id;
            this.$el.find('.codefile').hide();
            this.$el.find("#" + id).show();
            var codefile = _.findWhere(this.codefiles, { GUID : id});
            if (codefile.running)
                this.switchControlButtons('running');
            else 
                this.switchControlButtons('idle');
            codefile.focus();
        }
    },

    getScriptToExecute: function(e) {
        var codefile = _.findWhere(this.codefiles, { GUID: this.activeTab });
        this.executeScript(codefile);
    },

    executeScript: function(codefile) {
        var editor = this;
        codefile.running = true;
        codefile.disableEdit();
        var scriptContents = codefile.getCode();
        this.switchControlButtons('running');
        var p = new Promise(function(resolve, reject) {
            editor.cmd.submit(scriptContents, codefile.GUID, resolve);
        });
        p.then(function() {
            codefile.running = false;
            codefile.enableEdit();
            if (editor.activeTab == codefile.GUID)
                editor.switchControlButtons('idle');
        });
    },

    terminateScript: function(e) {
        var codefile = _.findWhere(this.codefiles, { GUID: this.activeTab });
        this.cmd.hub.server.terminateScript(codefile.GUID);
        codefile.enableEdit();
        this.switchControlButtons('idle');
    },

    switchControlButtons: function(state) {
        if (state == 'idle') {
            this.$el.find('.consolaStop').prop('disabled', true);
            this.$el.find('.consolaStart').prop('disabled', false);
        }
        else if (state == 'running') {
            this.$el.find('.consolaStop').prop('disabled', false);
            this.$el.find('.consolaStart').prop('disabled', true);
        }
    }

});