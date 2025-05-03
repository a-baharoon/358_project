// wwwroot/js/script.js
/**
 * TeamSync - Main JavaScript File for ASP.NET MVC Version
 */

// Wait for the DOM to be fully loaded
document.addEventListener('DOMContentLoaded', function () {
    // Initialize any page-specific functionality
    initializePageFunctionality();

    // Add scroll animation effects
    addScrollAnimations();
});

/**
 * Initialize the page-specific functionality
 */
function initializePageFunctionality() {
 
    const dynamicTitle = document.getElementById('dynamicTitle');
    if (dynamicTitle) {
        initializeDynamicTitle(dynamicTitle);
    }

    const faqQuestions = document.querySelectorAll('.faq-question');
    if (faqQuestions.length > 0) {
        initializeFaqAccordion(faqQuestions);
    }
}

function initializeDynamicTitle(titleElement) {
    const titles = [
        "Revolutionize Your Team Collaboration",
        "Make Teamwork Fun and Productive",
        "Track Time, Boost Productivity",
        "The Smart Way to Work Together"
    ];

    let currentTitleIndex = 0;

    setInterval(() => {
        currentTitleIndex = (currentTitleIndex + 1) % titles.length;

        titleElement.style.opacity = 0;

        setTimeout(() => {
            titleElement.textContent = titles[currentTitleIndex];
            titleElement.style.opacity = 1;
        }, 500);
    }, 5000);
}

function initializeFaqAccordion(faqQuestions) {
    faqQuestions.forEach(question => {
        question.addEventListener('click', function () {
            const answer = this.nextElementSibling;
            const isOpen = answer.style.maxHeight !== "0px" && answer.style.maxHeight !== "";

            document.querySelectorAll('.faq-answer').forEach(item => {
                item.style.maxHeight = "0px";
            });

            if (!isOpen) {
                answer.style.maxHeight = answer.scrollHeight + "px";
            }
        });
    });

    const firstAnswer = document.querySelector('.faq-answer');
    if (firstAnswer) {
        firstAnswer.style.maxHeight = firstAnswer.scrollHeight + "px";
    }
}

function addScrollAnimations() {
    const animatedElements = document.querySelectorAll('.step, .feature-card, .team-member, .value-card');

    if ('IntersectionObserver' in window) {
        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    entry.target.classList.add('animated');
                    observer.unobserve(entry.target);
                }
            });
        }, { threshold: 0.1 });

        animatedElements.forEach(element => {
            observer.observe(element);
        });
    } else {
        animatedElements.forEach(element => {
            element.classList.add('animated');
        });
    }
}