// Eliminar clave de hash de recursos Blazor al cargar la app por primera vez
try {
    localStorage.removeItem("blazor-resource-hash:WaHub.Client");
} catch (e) {
    // Silenciar errores de storage
}

// WaHub Mobile Navigation and App Functionality
window.waHubApp = {
    // Mobile Navigation
    mobileNav: {
        isOpen: false,
        
        toggle: function() {
            this.isOpen = !this.isOpen;
            const sidebar = document.querySelector('.sidebar');
            const overlay = document.querySelector('.sidebar-overlay');
            const toggle = document.querySelector('.mobile-nav-toggle');
            
            if (this.isOpen) {
                sidebar?.classList.add('mobile-open');
                overlay?.classList.add('active');
                toggle?.classList.add('active');
                document.body.style.overflow = 'hidden';
            } else {
                sidebar?.classList.remove('mobile-open');
                overlay?.classList.remove('active');
                toggle?.classList.remove('active');
                document.body.style.overflow = '';
            }
        },
        
        close: function() {
            if (this.isOpen) {
                this.toggle();
            }
        },
        
        init: function() {
            // Close sidebar when clicking overlay
            document.addEventListener('click', (e) => {
                if (e.target.classList.contains('sidebar-overlay')) {
                    this.close();
                }
            });
            
            // Close sidebar when clicking outside on mobile
            document.addEventListener('click', (e) => {
                const sidebar = document.querySelector('.sidebar');
                const toggle = document.querySelector('.mobile-nav-toggle');
                
                if (this.isOpen && 
                    !sidebar?.contains(e.target) && 
                    !toggle?.contains(e.target) &&
                    window.innerWidth <= 768) {
                    this.close();
                }
            });
            
            // Handle window resize
            window.addEventListener('resize', () => {
                if (window.innerWidth > 768 && this.isOpen) {
                    this.close();
                }
            });
        }
    },
    
    // Theme Management
    theme: {
        current: 'light',
        
        init: function() {
            // Load saved theme
            const savedTheme = localStorage.getItem('wahub-theme') || 'light';
            this.setTheme(savedTheme);
        },
        
        setTheme: function(theme) {
            this.current = theme;
            document.documentElement.setAttribute('data-theme', theme);
            localStorage.setItem('wahub-theme', theme);
        },
        
        toggle: function() {
            const newTheme = this.current === 'light' ? 'dark' : 'light';
            this.setTheme(newTheme);
            return newTheme;
        }
    },
    
    // Loading Management
    loading: {
        show: function(message = 'Cargando...') {
            const overlay = document.createElement('div');
            overlay.className = 'loading-overlay';
            overlay.id = 'loading-overlay';
            overlay.innerHTML = `
                <div class="loading-content">
                    <div class="loading-spinner"></div>
                    <div class="loading-text">${message}</div>
                </div>
            `;
            document.body.appendChild(overlay);
        },
        
        hide: function() {
            const overlay = document.getElementById('loading-overlay');
            if (overlay) {
                overlay.remove();
            }
        }
    },
    
    // Utility functions
    utils: {
        // Debounce function for performance
        debounce: function(func, wait) {
            let timeout;
            return function executedFunction(...args) {
                const later = () => {
                    clearTimeout(timeout);
                    func(...args);
                };
                clearTimeout(timeout);
                timeout = setTimeout(later, wait);
            };
        },
        
        // Format time ago
        timeAgo: function(date) {
            const now = new Date();
            const diff = now - date;
            const seconds = Math.floor(diff / 1000);
            const minutes = Math.floor(seconds / 60);
            const hours = Math.floor(minutes / 60);
            const days = Math.floor(hours / 24);
            
            if (days > 0) return `hace ${days} día${days > 1 ? 's' : ''}`;
            if (hours > 0) return `hace ${hours} hora${hours > 1 ? 's' : ''}`;
            if (minutes > 0) return `hace ${minutes} minuto${minutes > 1 ? 's' : ''}`;
            return 'hace un momento';
        },
        
        // Copy to clipboard
        copyToClipboard: async function(text) {
            try {
                await navigator.clipboard.writeText(text);
                return true;
            } catch (err) {
                // Fallback for older browsers
                const textArea = document.createElement('textarea');
                textArea.value = text;
                document.body.appendChild(textArea);
                textArea.select();
                try {
                    document.execCommand('copy');
                    return true;
                } catch (fallbackErr) {
                    return false;
                } finally {
                    document.body.removeChild(textArea);
                }
            }
        }
    },
    
    // Sidebar user menu functionality
    userMenu: {
        init: function() {
            // Close user dropdown when clicking outside
            document.addEventListener('click', function(event) {
                const userSection = document.querySelector('.user-section');
                const userMenuButton = document.querySelector('.user-menu-button');
                const userDropdown = document.querySelector('.user-dropdown');
                
                if (userSection && !userSection.contains(event.target)) {
                    userMenuButton?.classList.remove('open');
                    userDropdown?.classList.remove('open');
                }
            });
            
            // Handle keyboard navigation
            document.addEventListener('keydown', function(event) {
                if (event.key === 'Escape') {
                    const userMenuButton = document.querySelector('.user-menu-button');
                    const userDropdown = document.querySelector('.user-dropdown');
                    
                    userMenuButton?.classList.remove('open');
                    userDropdown?.classList.remove('open');
                }
            });
        }
    },

    // Initialize the app
    init: function() {
        this.theme.init();
        this.mobileNav.init();
        this.userMenu.init();
        
        // Add event listeners for common functionality
        document.addEventListener('DOMContentLoaded', () => {
            console.log('WaHub App initialized');
        });
    }
};

// Auto-initialize when script loads
window.waHubApp.init();

// Expose functions for Blazor interop
window.toggleMobileNav = () => window.waHubApp.mobileNav.toggle();
window.closeMobileNav = () => window.waHubApp.mobileNav.close();
window.toggleTheme = () => window.waHubApp.theme.toggle();
window.showLoading = (message) => window.waHubApp.loading.show(message);
window.hideLoading = () => window.waHubApp.loading.hide();
window.copyToClipboard = (text) => window.waHubApp.utils.copyToClipboard(text);

// Function to set language cookie (used by Blazor Interop)
window.setLanguageCookie = function(cookieName, language) {
    const date = new Date();
    date.setTime(date.getTime() + (365 * 24 * 60 * 60 * 1000)); // 1 year expiry
    const expires = "; expires=" + date.toUTCString();
    const cookieValue = `c=${language}|uic=${language}`;
    document.cookie = `${cookieName}=${cookieValue}${expires}; path=/`;
    // console.log(`Cookie '${cookieName}' set to: ${cookieValue}`); // For debug
};

// Function to get language cookie
window.getLanguageCookie = function(cookieName) {
    const name = cookieName + "=";
    const ca = document.cookie.split(';');
    for (let i = 0; i < ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) === ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) === 0) {
            const cookieValue = c.substring(name.length, c.length);
            // Parse the culture cookie format: c=language|uic=language
            const match = cookieValue.match(/c=([^|]+)/);
            return match ? match[1] : null;
        }
    }
    return null;
};
