/**
 * TeamSync - Main JavaScript File
 * This file contains the main functionality for the TeamSync platform
 */

// Wait for the DOM to be fully loaded
document.addEventListener('DOMContentLoaded', function() {
    // Initialize the timer functionality if on the relevant page
    const timerElement = document.getElementById('timer');
    if (timerElement) {
        initializeTimer();
    }

    // Initialize form validation
    initializeFormValidation();
    
    // Add dynamic elements to the page
    addDynamicElements();
    
    // Add scroll animation effects
    addScrollAnimations();
});

/**
 * Initialize the timer functionality
 */
function initializeTimer() {
    const timerElement = document.getElementById('timer');
    const startButton = document.getElementById('start-timer');
    const stopButton = document.getElementById('stop-timer');
    const resetButton = document.getElementById('reset-timer');
    
    let seconds = 0;
    let minutes = 0;
    let hours = 0;
    let timerInterval;
    let isRunning = false;
    
    // Audio alerts array
    const audioAlerts = [
        {
            message: "Keep up the good work!",
            sound: "alert1.mp3"
        },
        {
            message: "Stay focused on your task!",
            sound: "alert2.mp3"
        },
        {
            message: "Great progress, keep going!",
            sound: "alert3.mp3"
        },
        {
            message: "Don't give up now, you're doing great!",
            sound: "alert4.mp3"
        }
    ];
    
    // Function to update the timer display
    function updateTimer() {
        seconds++;
        
        if (seconds >= 60) {
            seconds = 0;
            minutes++;
            
            if (minutes >= 60) {
                minutes = 0;
                hours++;
            }
        }
        
        // Format the time
        const formattedTime = 
            (hours < 10 ? "0" + hours : hours) + ":" +
            (minutes < 10 ? "0" + minutes : minutes) + ":" +
            (seconds < 10 ? "0" + seconds : seconds);
        
        timerElement.textContent = formattedTime;
        
        // Random alerts (approx every 5-15 minutes)
        if (minutes > 0 && seconds === 0 && Math.random() < 0.2) {
            triggerRandomAlert();
        }
    }
    
    // Function to trigger a random alert
    function triggerRandomAlert() {
        const randomIndex = Math.floor(Math.random() * audioAlerts.length);
        const alert = audioAlerts[randomIndex];
        
        // Display the alert message
        const alertElement = document.getElementById('alert-message');
        if (alertElement) {
            alertElement.textContent = alert.message;
            alertElement.classList.add('show');
            
            // Hide the alert after 5 seconds
            setTimeout(() => {
                alertElement.classList.remove('show');
            }, 5000);
        }
        
        // Play the alert sound (if available)
        // Note: In a real implementation, you would need the actual audio files
        console.log("Playing sound: " + alert.sound);
    }
    
    // Start timer function
    function startTimer() {
        if (!isRunning) {
            isRunning = true;
            timerInterval = setInterval(updateTimer, 1000);
            startButton.disabled = true;
            stopButton.disabled = false;
        }
    }
    
    // Stop timer function
    function stopTimer() {
        if (isRunning) {
            isRunning = false;
            clearInterval(timerInterval);
            startButton.disabled = false;
            stopButton.disabled = true;
            
            // Show reflection prompt when timer stops
            showReflectionPrompt();
        }
    }
    
    // Reset timer function
    function resetTimer() {
        stopTimer();
        seconds = 0;
        minutes = 0;
        hours = 0;
        timerElement.textContent = "00:00:00";
    }
    
    // Show reflection prompt
    function showReflectionPrompt() {
        const reflectionElement = document.getElementById('reflection-prompt');
        if (reflectionElement) {
            reflectionElement.classList.add('show');
        }
    }
    
    // Event listeners
    if (startButton) {
        startButton.addEventListener('click', startTimer);
    }
    
    if (stopButton) {
        stopButton.addEventListener('click', stopTimer);
    }
    
    if (resetButton) {
        resetButton.addEventListener('click', resetTimer);
    }
}

/**
 * Initialize form validation for login and signup forms
 */
function initializeFormValidation() {
    // Login form validation
    const loginForm = document.getElementById('login-form');
    if (loginForm) {
        loginForm.addEventListener('submit', function(e) {
            e.preventDefault();
            
            const email = document.getElementById('email').value;
            const password = document.getElementById('password').value;
            let isValid = true;
            
            // Reset error messages
            document.getElementById('email-error').textContent = '';
            document.getElementById('password-error').textContent = '';
            
            // Validate email
            if (!validateEmail(email)) {
                document.getElementById('email-error').textContent = 'Please enter a valid email address';
                isValid = false;
            }
            
            // Validate password
            if (!password || password.length < 6) {
                document.getElementById('password-error').textContent = 'Password must be at least 6 characters';
                isValid = false;
            }
            
            if (isValid) {
                // Show success message (in a real app, this would be a server request)
                const messageBox = document.getElementById('login-message');
                messageBox.textContent = 'Login successful! Redirecting...';
                messageBox.className = 'message-box success';
                
                // Simulate redirect after successful login
                setTimeout(() => {
                    window.location.href = 'index.html';
                }, 2000);
            }
        });
    }
    
    // Signup form validation
    const signupForm = document.getElementById('signup-form');
    if (signupForm) {
        signupForm.addEventListener('submit', function(e) {
            e.preventDefault();
            
            const fullname = document.getElementById('fullname').value;
            const email = document.getElementById('email').value;
            const password = document.getElementById('password').value;
            const confirmPassword = document.getElementById('confirm-password').value;
            const university = document.getElementById('university').value;
            const terms = document.getElementById('terms').checked;
            
            let isValid = true;
            
            // Reset error messages
            const errorElements = document.getElementsByClassName('error-message');
            for (let i = 0; i < errorElements.length; i++) {
                errorElements[i].textContent = '';
            }
            
            // Validate full name
            if (!fullname || fullname.length < 3) {
                document.getElementById('fullname-error').textContent = 'Please enter your full name (at least 3 characters)';
                isValid = false;
            }
            
            // Validate email
            if (!validateEmail(email)) {
                document.getElementById('email-error').textContent = 'Please enter a valid email address';
                isValid = false;
            }
            
            // Validate password
            if (!password || password.length < 8) {
                document.getElementById('password-error').textContent = 'Password must be at least 8 characters';
                isValid = false;
            }
            
            // Validate password confirmation
            if (password !== confirmPassword) {
                document.getElementById('confirm-password-error').textContent = 'Passwords do not match';
                isValid = false;
            }
            
            // Validate university selection
            if (!university) {
                document.getElementById('university-error').textContent = 'Please select your university';
                isValid = false;
            }
            
            // Validate terms agreement
            if (!terms) {
                document.getElementById('terms-error').textContent = 'You must agree to the terms and conditions';
                isValid = false;
            }
            
            if (isValid) {
                // Show success message
                const messageBox = document.getElementById('signup-message');
                messageBox.textContent = 'Account created successfully! Redirecting to login...';
                messageBox.className = 'message-box success';
                
                // Simulate redirect after successful signup
                setTimeout(() => {
                    window.location.href = 'login.html';
                }, 2000);
            }
        });
    }
}

/**
 * Email validation helper function
 */
function validateEmail(email) {
    const re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(email).toLowerCase());
}

/**
 * Add dynamic elements to the page
 */
function addDynamicElements() {
    // Dynamic hero title for the home page
    const heroTitle = document.querySelector('.hero-content h1');
    if (heroTitle) {
        const titles = [
            "Revolutionize Your Team Collaboration",
            "Make Teamwork Fun and Productive",
            "Track Time, Boost Productivity",
            "The Smart Way to Work Together"
        ];
        
        let currentTitleIndex = 0;
        
        setInterval(() => {
            currentTitleIndex = (currentTitleIndex + 1) % titles.length;
            
            // Fade out
            heroTitle.style.opacity = 0;
            
            // Change text and fade in after a short delay
            setTimeout(() => {
                heroTitle.textContent = titles[currentTitleIndex];
                heroTitle.style.opacity = 1;
            }, 500);
        }, 5000);
    }
    
    // Dynamic leaderboard data (if the element exists)
    const leaderboardElement = document.getElementById('leaderboard');
    if (leaderboardElement) {
        generateLeaderboardData();
    }
    
    // FAQ accordion functionality
    const faqQuestions = document.querySelectorAll('.faq-question');
    faqQuestions.forEach(question => {
        question.addEventListener('click', function() {
            const answer = this.nextElementSibling;
            const isOpen = answer.style.maxHeight;
            
            // Close all answers
            document.querySelectorAll('.faq-answer').forEach(item => {
                item.style.maxHeight = null;
            });
            
            // Toggle the clicked question
            if (!isOpen) {
                answer.style.maxHeight = answer.scrollHeight + 'px';
            }
        });
    });
    
    // Open the first FAQ item by default
    const firstAnswer = document.querySelector('.faq-answer');
    if (firstAnswer) {
        firstAnswer.style.maxHeight = firstAnswer.scrollHeight + 'px';
    }
}

/**
 * Generate dynamic leaderboard data
 */
function generateLeaderboardData() {
    const leaderboardElement = document.getElementById('leaderboard');
    const leaderboardTable = document.createElement('table');
    leaderboardTable.className = 'leaderboard-table';
    
    // Sample leaderboard data
    const leaderboardData = [
        { rank: 1, name: 'Ahmed A.', hours: 42, tasks: 15 },
        { rank: 2, name: 'Sara M.', hours: 38, tasks: 12 },
        { rank: 3, name: 'Khalid R.', hours: 35, tasks: 14 },
        { rank: 4, name: 'Noura K.', hours: 30, tasks: 11 },
        { rank: 5, name: 'Mohammed S.', hours: 28, tasks: 10 }
    ];
    
    // Create table header
    const thead = document.createElement('thead');
    const headerRow = document.createElement('tr');
    ['Rank', 'Team Member', 'Hours', 'Tasks Completed'].forEach(headerText => {
        const th = document.createElement('th');
        th.textContent = headerText;
        headerRow.appendChild(th);
    });
    thead.appendChild(headerRow);
    leaderboardTable.appendChild(thead);
    
    // Create table body
    const tbody = document.createElement('tbody');
    leaderboardData.forEach(member => {
        const row = document.createElement('tr');
        
        // Create cells
        const rankCell = document.createElement('td');
        rankCell.textContent = member.rank;
        
        const nameCell = document.createElement('td');
        nameCell.textContent = member.name;
        
        const hoursCell = document.createElement('td');
        hoursCell.textContent = member.hours;
        
        const tasksCell = document.createElement('td');
        tasksCell.textContent = member.tasks;
        
        // Highlight the top performer
        if (member.rank === 1) {
            row.className = 'top-performer';
        }
        
        // Append cells to row
        row.appendChild(rankCell);
        row.appendChild(nameCell);
        row.appendChild(hoursCell);
        row.appendChild(tasksCell);
        
        // Append row to tbody
        tbody.appendChild(row);
    });
    leaderboardTable.appendChild(tbody);
    
    // Add the table to the leaderboard element
    leaderboardElement.appendChild(leaderboardTable);
}

/**
 * Add scroll animations to elements
 */
function addScrollAnimations() {
    // Get all elements that should animate on scroll
    const animatedElements = document.querySelectorAll('.step, .feature-card, .team-member, .value-card');
    
    // Check if IntersectionObserver is supported
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
        // Fallback for browsers that don't support IntersectionObserver
        animatedElements.forEach(element => {
            element.classList.add('animated');
        });
    }
}