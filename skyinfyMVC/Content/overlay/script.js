jQuery(function ($) {

    $("<div id='overlay'><img src='Style/image/ajax-loader.gif' alt='loading image' /></div>").appendTo('body');

    $('body a').each(function () {
        $(this).addClass('is-overlay');
    });

    $('body input[type=submit]').each(function () {
        $(this).addClass('is-overlay');
    });

    $('.is-overlay').bind('click', function () {
        setTimeout(ShowOverlay, 1);
    });
});

function ShowOverlay() {
    $('#overlay').show();
}

