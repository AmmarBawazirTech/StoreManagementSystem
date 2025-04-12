// Initialize tooltips everywhere
$(function () {
    $('[data-bs-toggle="tooltip"]').tooltip();
});

// Global AJAX error handler
$(document).ajaxError(function (event, jqxhr, settings, thrownError) {
    if (jqxhr.status === 401) {
        Swal.fire({
            icon: 'error',
            title: 'Session Expired',
            text: 'Your session has expired. Please log in again.',
            confirmButtonText: 'OK'
        }).then(() => {
            window.location.href = '/Account/Login';
        });
    } else if (jqxhr.status !== 0) { // Ignore aborted requests
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'An error occurred while processing your request.',
            confirmButtonText: 'OK'
        });
    }
});

// Confirm before submitting forms with data-confirm attribute
$(document).on('submit', 'form[data-confirm]', function (e) {
    e.preventDefault();
    const form = this;
    const confirmMessage = $(form).data('confirm') || 'Are you sure you want to proceed?';

    Swal.fire({
        title: 'Confirm',
        text: confirmMessage,
        icon: 'question',
        showCancelButton: true,
        confirmButtonText: 'Yes, proceed',
        cancelButtonText: 'Cancel'
    }).then((result) => {
        if (result.isConfirmed) {
            form.submit();
        }
    });
});

// Auto-disable submit buttons on form submission to prevent double submits
$(document).on('submit', 'form', function () {
    const form = this;
    const submitButtons = $(form).find('button[type="submit"]');

    submitButtons.each(function () {
        const button = $(this);
        button.prop('disabled', true);

        // Add spinner if button has icon
        const icon = button.find('i');
        if (icon.length) {
            icon.removeClass().addClass('bi bi-arrow-repeat spinner');
        } else {
            button.append(' <span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>');
        }
    });
});


