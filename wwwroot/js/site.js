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

// Smooth scroll for anchor links
document.addEventListener('DOMContentLoaded', function () {
    const smoothScrollLinks = document.querySelectorAll('a[href^="#"]');
    
    smoothScrollLinks.forEach(function (link) {
        link.addEventListener('click', function (e) {
            const targetId = this.getAttribute('href');
            if (targetId !== '#' && targetId !== '') {
                const targetElement = document.querySelector(targetId);
                if (targetElement) {
                    e.preventDefault();
                    targetElement.scrollIntoView({
                        behavior: 'smooth',
                        block: 'start'
                    });
                }
            }
        });
    });
});

// Form validation enhancement
document.addEventListener('DOMContentLoaded', function () {
    const forms = document.querySelectorAll('form');
    
    forms.forEach(function (form) {
        form.addEventListener('submit', function (e) {
            if (!form.checkValidity()) {
                e.preventDefault();
                e.stopPropagation();
                
                // Show validation feedback
                const invalidInputs = form.querySelectorAll(':invalid');
                invalidInputs.forEach(function (input) {
                    input.classList.add('is-invalid');
                });
            }
            form.classList.add('was-validated');
        });
        
        // Remove invalid class on input
        const inputs = form.querySelectorAll('input, select, textarea');
        inputs.forEach(function (input) {
            input.addEventListener('input', function () {
                if (this.checkValidity()) {
                    this.classList.remove('is-invalid');
                    this.classList.add('is-valid');
                } else {
                    this.classList.remove('is-valid');
                    this.classList.add('is-invalid');
                }
            });
        });
    });
});

// Back to top button
document.addEventListener('DOMContentLoaded', function () {
    // Create back to top button
    const backToTopBtn = document.createElement('button');
    backToTopBtn.innerHTML = '<i class="fas fa-arrow-up"></i>';
    backToTopBtn.className = 'btn btn-primary back-to-top';
    backToTopBtn.setAttribute('title', 'Към началото');
    document.body.appendChild(backToTopBtn);
    
    // Show/hide button based on scroll position
    window.addEventListener('scroll', function () {
        if (window.pageYOffset > 300) {
            backToTopBtn.style.display = 'block';
        } else {
            backToTopBtn.style.display = 'none';
        }
    });
    
    // Scroll to top on click
    backToTopBtn.addEventListener('click', function () {
        window.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
    });
});

// Animate elements on scroll
document.addEventListener('DOMContentLoaded', function () {
    const observerOptions = {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
    };
    
    const observer = new IntersectionObserver(function (entries) {
        entries.forEach(function (entry) {
            if (entry.isIntersecting) {
                entry.target.classList.add('animate-in');
                observer.unobserve(entry.target);
            }
        });
    }, observerOptions);
    
    // Observe cards and rows
    const animatedElements = document.querySelectorAll('.card, .row > div');
    animatedElements.forEach(function (element) {
        observer.observe(element);
    });
});

// Phone number formatting
document.addEventListener('DOMContentLoaded', function () {
    const phoneInputs = document.querySelectorAll('input[type="tel"]');
    
    phoneInputs.forEach(function (input) {
        input.addEventListener('input', function (e) {
            let value = e.target.value.replace(/\D/g, '');
            if (value.length > 10) {
                value = value.substring(0, 10);
            }
            e.target.value = value;
        });
    });
});

// Newsletter form submission
document.addEventListener('DOMContentLoaded', function () {
    const newsletterForm = document.getElementById('newsletterForm');
    
    if (newsletterForm) {
        newsletterForm.addEventListener('submit', function (e) {
            e.preventDefault();
            
            const emailInput = this.querySelector('input[type="email"]');
            const email = emailInput.value;
            
            // Simulate newsletter subscription
            if (email) {
                // Show success message
                const originalHTML = this.innerHTML;
                this.innerHTML = '<div class="alert alert-success mb-0"><i class="fas fa-check-circle"></i> Благодарим за абонамента!</div>';
                
                // Reset form after 3 seconds
                setTimeout(function () {
                    newsletterForm.innerHTML = originalHTML;
                    // Re-attach event listener
                    const newForm = document.getElementById('newsletterForm');
                    if (newForm) {
                        newForm.addEventListener('submit', arguments.callee);
                    }
                }, 3000);
            }
        });
    }
});
