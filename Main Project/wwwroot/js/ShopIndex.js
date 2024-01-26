$(document).ready(function () {
    // Event delegation for dynamically added forms
    $('#store-container').off('submit', 'form');

    $('#store-container').on('submit', 'form', function (event) {
        event.preventDefault();
        var $form = $(this);
        $.ajax({
            url: $form.attr('action'),
            type: 'POST',
            data: $form.serialize(),
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            },
            success: function (response) {
                $('#cart-items-container').html(response.html);
                alert(response.message);
            },
            error: function (xhr, status, error) {
                alert("Error preforming that action.");
                console.error('There has been a problem with the AJAX operation:', error);
            }
        });
    });
});