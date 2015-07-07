
$('ul.nav li.active').removeClass('active');
$('#' + nav.getActivePage()).parent().addClass('active');

$('ul.nav li a').click(function () {
    nav.showPage($(this)[0].id);
});

$('#experiment-name').click(function () {
    nav.showPage('overview');
});