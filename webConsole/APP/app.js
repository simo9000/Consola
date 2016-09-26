global.jQuery = require('jquery');
global.$ = jQuery;
global.underscore = require('underscore');
global._ = underscore;
var commandPrompt = require('./Views/commandPrompt');
  
$(document).ready(function () {
    var prompt = new commandPrompt({});
    prompt.render();
    $('body').append(prompt.el);
    prompt.focus();
});