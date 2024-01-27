$(document).ready(function () {
    $('body').on('submit', 'form', function (event) {
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
                displayBootstrapAlert('success', response.message);
            },
            error: function (xhr, status, error) {
                displayBootstrapAlert('danger', "Error performing that action.");
                console.error('There has been a problem with the AJAX operation:', error);
            }
        });
    });

    function displayBootstrapAlert(type, message) {
        var alertType = type === 'success' ? 'alert-primary' : 'alert-danger';
        var alertHtml = '<div class="alert ' + alertType + ' alert-dismissible fade show" role="alert">' +
            message +
            '<button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close">' +
            '</button>' +
            '</div>';
        $('#alert-placeholder').html(alertHtml);

        setTimeout(function () {
            $('.alert').alert('close');
        }, 3000);
    }
});