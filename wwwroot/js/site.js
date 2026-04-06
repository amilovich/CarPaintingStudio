// ================================================================
// CarPainting Studio — Global JavaScript
// ================================================================

document.addEventListener('DOMContentLoaded', function () {

    // ── 1. Auto-dismiss alerts след 5 секунди ────────────────────
    document.querySelectorAll('.alert-dismissible').forEach(function (alert) {
        setTimeout(function () {
            try {
                bootstrap.Alert.getOrCreateInstance(alert).close();
            } catch (e) { /* ignore */ }
        }, 5000);
    });

    // ── 2. Back to Top бутон ─────────────────────────────────────
    const btn = document.createElement('button');
    btn.id        = 'backToTop';
    btn.className = 'btn btn-dark';
    btn.title     = 'Към началото';
    btn.innerHTML = '<i class="fas fa-arrow-up"></i>';
    document.body.appendChild(btn);

    window.addEventListener('scroll', function () {
        btn.style.display = window.scrollY > 300 ? 'block' : 'none';
    });

    btn.addEventListener('click', function () {
        window.scrollTo({ top: 0, behavior: 'smooth' });
    });

    // ── 3. Scroll fade-in анимация ───────────────────────────────
    const fadeEls = document.querySelectorAll('.card');
    const observer = new IntersectionObserver(function (entries) {
        entries.forEach(function (entry) {
            if (entry.isIntersecting) {
                entry.target.classList.add('visible');
                observer.unobserve(entry.target);
            }
        });
    }, { threshold: 0.08, rootMargin: '0px 0px -30px 0px' });

    fadeEls.forEach(function (el) {
        el.classList.add('fade-scroll');
        observer.observe(el);
    });

    // ── 4. Smooth scroll за anchor линкове ───────────────────────
    document.querySelectorAll('a[href^="#"]').forEach(function (link) {
        link.addEventListener('click', function (e) {
            const id = this.getAttribute('href');
            if (id && id !== '#') {
                const target = document.querySelector(id);
                if (target) {
                    e.preventDefault();
                    target.scrollIntoView({ behavior: 'smooth', block: 'start' });
                }
            }
        });
    });

    // ── 5. Минимална дата за date inputs ─────────────────────────
    document.querySelectorAll('input[type="date"]').forEach(function (input) {
        if (!input.min) {
            const tomorrow = new Date();
            tomorrow.setDate(tomorrow.getDate() + 1);
            input.min = tomorrow.toISOString().split('T')[0];
        }
    });

    // ── 6. Phone input — само цифри ──────────────────────────────
    document.querySelectorAll('input[type="tel"]').forEach(function (input) {
        input.addEventListener('input', function () {
            this.value = this.value.replace(/\D/g, '').slice(0, 10);
        });
    });

    // ── 7. Char counter за textarea ──────────────────────────────
    document.querySelectorAll('textarea[maxlength]').forEach(function (ta) {
        const counter = document.createElement('small');
        counter.className = 'text-muted float-end';
        ta.parentNode.appendChild(counter);

        function update() {
            const max = ta.getAttribute('maxlength');
            counter.textContent = ta.value.length + '/' + max;
        }

        ta.addEventListener('input', update);
        update();
    });

    // ── 8. Newsletter форма ───────────────────────────────────────
    const newsletterForm = document.getElementById('newsletterForm');
    if (newsletterForm) {
        newsletterForm.addEventListener('submit', function (e) {
            e.preventDefault();
            const email = this.querySelector('input[type="email"]').value;
            if (email) {
                this.innerHTML =
                    '<div class="alert alert-success py-2 mb-0">' +
                    '<i class="fas fa-check-circle me-2"></i>Благодарим за абонамента!</div>';
            }
        });
    }

    // ── 9. Active nav link highlight (допълнителен fallback) ─────
    const currentPath = window.location.pathname.toLowerCase();
    document.querySelectorAll('.navbar-nav .nav-link').forEach(function (link) {
        const href = link.getAttribute('href');
        if (href && currentPath.startsWith(href.toLowerCase()) && href !== '/') {
            link.classList.add('active');
        }
    });

});
