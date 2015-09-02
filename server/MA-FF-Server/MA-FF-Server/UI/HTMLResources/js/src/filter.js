function filter(elementSelector, searchTerm) {
    if (searchTerm != '') {
        $(elementSelector).hide();

        var elements = $(elementSelector).filter(function () {
            return $(this).attr('filter').toLowerCase().indexOf(searchTerm.toLowerCase()) >= 0;
        });

        console.log("Elements: ", elements);

        $(elements).show();
    } else {
        $(elementSelector).show();
    }
}

//filter('#experiment-list .row', 'test');