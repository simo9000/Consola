/// <reference path="~/scripts/linq.js"/>
/// <reference path="~/scripts/linq-vsdoc.js"/>
/// <reference path="~/scripts/jquery-2.2.1.js"/>
/// <refernece path="~/scripts/jquery-2.1.0-vsdoc.js"/>
/// <reference path="~/scripts/APP/Views/commandPrompt.js"/>


$(document).ready(function () {
    var prompt = new commandPrompt({});
    prompt.render();
    $('#console').append(prompt.el);
    prompt.focus();
});