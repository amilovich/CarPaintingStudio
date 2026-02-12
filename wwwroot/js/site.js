// Custom JavaScript for CarPainting Studio

// Auto-dismiss alerts after 5 seconds
document.addEventListener('DOMContentLoaded', function () {
    const alerts = document.querySelectorAll('.alert-dismissible');
    
    alerts.forEach(function (alert) {
        setTimeout(function () {
            const bsAlert = new bootstrap.Alert(alert);
            bsAlert.close();
        }, 5000);
    });
});

// Confirm delete actions
document.addEventListener('DOMContentLoaded', function () {
    const deleteButtons = document.querySelectorAll('a[href*="/Delete/"]');
    
    deleteButtons.forEach(function (button) {
        button.addEventListener('click', function (e) {
            if (!confirm('Сигурни ли сте, че искате да изтриете този запис?')) {
                e.preventDefault();
            }
        });
    });
});

// Date input validation - ensure future dates
document.addEventListener('DOMContentLoaded', function () {
    const dateInputs = document.querySelectorAll('input[type="date"]');
    
    dateInputs.forEach(function (input) {
        // Set minimum date to tomorrow
        const tomorrow = new Date();
        tomorrow.setDate(tomorrow.getDate() + 1);
        const minDate = tomorrow.toISOString().split('T')[0];
        input.setAttribute('min', minDate);
    });
});
