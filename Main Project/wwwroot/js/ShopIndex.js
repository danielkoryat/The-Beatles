$(document).ready(function () {
    $('#remove-from-cart-form').submit(function (event) {
        event.preventDefault();
        var $form = $(this);
        $.ajax({
            url: $form.attr('action'),
            type: 'POST',
            data: $form.serialize(),
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            },
            success: function (partialViewResult) {
                $('#cart-items-container').html(partialViewResult);
            },
            error: function (xhr, status, error) {
                alert("Error removing item from cart.");
                console.error('There has been a problem with the AJAX operation:', error);
            }
        });
    });
});

$(document).ready(function () {
    $('#add-to-cart').submit(function (event) {
        event.preventDefault();
        var $form = $(this);
        $.ajax({
            url: $form.attr('action'),
            type: 'POST',
            data: $form.serialize(), // serialize the form data for submission
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            },
            success: function (partialViewResult) {
                $('#cart-items-container').html(partialViewResult);
            },
            error: function (xhr, status, error) {
                alert("Error removing item from cart.");
                console.error('There has been a problem with the AJAX operation:', error);
            }
        });
    });
});