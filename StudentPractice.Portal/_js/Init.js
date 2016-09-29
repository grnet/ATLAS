$(document).ready(function () {
    $(':input[title]').each(function () {
        $(this).attr("tip", $(this).attr("title"));
        $(this).removeAttr("title");
    });
})

$(':input[tip]').tipsy({
    title: 'tip',
    gravity: 'sw',
    trigger: "click"
});

$('textarea[tip]').tipsy({
    title: 'tip',
    gravity: 'sw',
    trigger: "click"
});

$('a[tip]').tipsy({
    title: 'tip',
    gravity: 'sw',
    className: 'simple-tipsy'
});

$('.tooltip').tipsy({
    title: 'tip',
    gravity: 'nw',
    className: 'info-tipsy'
});

$('.errortip').tipsy({
    gravity: 'w',
    className: 'error-tipsy'
});