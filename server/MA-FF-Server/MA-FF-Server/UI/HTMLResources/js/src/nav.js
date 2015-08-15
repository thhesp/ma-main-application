
$('ul li.active').removeClass('active');
$('#' + nav.getActivePage()).parent().addClass('active');

$('ul li a').click(function () {
    nav.showPage($(this)[0].id);
});

$('#experiment-name').click(function () {
    nav.showPage('overview');
});

$('#application-settings').click(function () {
    nav.showApplicationSettings();
});