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
        this.frozen = false;
    },

    render: function() {
        var prompt = this;
        this.$el.html(this.template({}));
        this.$el.addClass('consolaTab-container');
        var i = 0, key, lastTab;
        for (; key = localStorage.key(i) ; i++)
            if (key.match(/ConsolaScripts\/*/)) {
                loadedScripts = true;
                lastTab = this.loadTab(key.replace('ConsolaScripts/', ''), localStorage.getItem(key));
            }
        if (!lastTab)
            lastTab = this.newTab();
        return this;
    },

    loadTab: function(name, code) {
        var file = this.newTab();
        file.tab.name = name;
        file.tab.isSaved = true;
        file.tab.render();
        file.code.name = name;
        file.code.setCode(code);
        return file.tab;
    },

    newTab: function() {
        if (this.frozen)
            return;
        var prompt = this;
        var newGUID = this.cmd.generateGUID();
        this.activeTab = newGUID;
        var allTabs = this.$el.find('[id$=_tab]');
        allTabs.removeClass('active');
        allTabs.find('.delete').hide();
        var newTab = new editorTab({
            id: newGUID + '_tab',
            name: "untitled" + (this.tabCount || '')
        }).render();
        this.listenTo(newTab, 'save', this.saveFile);
        this.listenTo(newTab, 'freeze', function() { prompt.frozen = true; });
        this.listenTo(newTab, 'unfreeze', function() { prompt.frozen = false; });
        this.listenTo(newTab, 'delete', this.deleteTab);
        this.$el.find('ul').append(newTab.$el);
        this.$el.find('ul').append(this.$el.find('#addNew'));
        var codefile = new codeFile({
            id: newGUID
        });
        this.codefiles.push(codefile);
        this.listenTo(codefile, 'run', prompt.executeScript);
        this.listenTo(codefile, 'change', function() {
            newTab.isSaved = false;
            newTab.render();
        });
        this.listenTo(codefile, 'save', function() {
            if (codefile.name)
                prompt.saveFile(codefile.name, newTab);
            else
                newTab.rename();
        });
        codefile.render();
        this.$el.find('.codefile').hide();
        this.$el.append(codefile.$el);
        if (this.tabCount)
            this.tabCount++;
        else
            this.tabCount = 1;
        codefile.focus();
        return {
            tab: newTab,
            code: codefile
        }
    },

    switchTab: function(e) {
        if (this.frozen)
            return;
        var li = e.target.tagName == 'LI' ? e.target : e.target.parentElement;
        if (li.id != "addNew" && li.id != this.activeTab + '_tab') {
            var allTabs = this.$el.find('[id$=_tab]');
            allTabs.removeClass('active');
            allTabs.find('.delete').hide();
            $(li).addClass('active');
            $(li).find('.delete').show();
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
        if (this.frozen)
            return;
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
        if (this.frozen)
            return;
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
    },

    saveFile: function(name, tab) {
        var codefile = _.findWhere(this.codefiles, { GUID: this.activeTab });
        tab.isSaved = false;
        if (name != '' && (codefile.name == name || !_.contains(_.pluck(this.codefiles, 'name'), name))){
            var scriptContents = codefile.getCode();
            codefile.name = name;
            localStorage.setItem('ConsolaScripts/' + name, scriptContents);
            tab.name = name;
            tab.isSaved = true;
            tab.render();
        }
    },

    deleteTab: function(tab) {
        this.frozen = false;
        var codefile = _.findWhere(this.codefiles, { GUID: this.activeTab });
        var index = _.findIndex(this.codefiles, function(c) { return c.GUID == this.activeTab });
        this.codefiles.splice(index, 1);
        if (codefile.name)
            localStorage.removeItem('ConsolaScripts/' + codefile.name);
        $('#' + codefile.GUID).remove();
        $('#' + codefile.GUID + '_tab').remove();
        var tab = this.$el.find('li')[0];
        if (tab)
            tab.click();
        else
            this.newTab();
    }


});