$(document).ready(function () {
    $('body').on('submit', 'form', function (event) {
        event.preventDefault();
        var $form = $(this);
        var responseTarget = $form.data('response-target');

        $.ajax({
            url: $form.attr('action'),
            type: 'POST',
            data: $form.serialize(),
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            },
            success: function (response) {
                updateResponseTarget(response, responseTarget);
                displayBootstrapAlert(response.success, response.message);
            },
            error: function (xhr, status, error) {
                displayBootstrapAlert(false, "Error performing that action.");
                console.error('AJAX error:', status, error);
            }
        });
    });

    function updateResponseTarget(response, target) {
        if (response.html) {
            $(target).html(response.html);
        }
    }

    function displayBootstrapAlert(isSuccess, message) {
        var alertType = isSuccess ? 'alert-primary' : 'alert-danger';
        var alertHtml = `<div class="alert ${alertType} alert-dismissible fade show" role="alert">
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        </div>`;
        $('#alert-placeholder').html(alertHtml).alert();

        setTimeout(() => $('.alert').alert('close'), 3000);
    }
});