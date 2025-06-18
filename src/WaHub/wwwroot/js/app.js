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

        detectSystemPreference: function() {
            if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
                return 'dark';
            }
            return 'light';
        },
          getCurrentTheme: async function() {
            try {
                const response = await fetch('/api/theme/current');
                if (response.ok) {
                    const data = await response.json();
                    return data.theme;
                }
            } catch (error) {
                console.log('Error getting current theme:', error);
            }
            return this.detectSystemPreference();
        },
        
        setTheme: async function(theme) {
            try {
                const response = await fetch('/api/theme/set', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify({ theme: theme })
                });
                
                if (response.ok) {
                    this.current = theme;
                    // Forzar recarga para aplicar el nuevo theme
                    window.location.reload();
                    return theme;
                }
            } catch (error) {
                console.log('Error setting theme:', error);
            }
            return this.current;
        },
        
        toggle: async function() {
            const newTheme = this.current === 'light' ? 'dark' : 'light';
            return await this.setTheme(newTheme);
        },
        
        init: async function() {
            this.current = await this.getCurrentTheme();
            
            // Listen for system preference changes
            const mediaQuery = window.matchMedia('(prefers-color-scheme: dark)');
            mediaQuery.addEventListener('change', async (e) => {
                // Only update if user hasn't set a preference
                const currentFromServer = await this.getCurrentTheme();
                if (currentFromServer === this.detectSystemPreference()) {
                    const newSystemTheme = e.matches ? 'dark' : 'light';
                    await this.setTheme(newSystemTheme);
                }
            });
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
    },    // Initialize the app
    init: function() {
        this.theme.init();
        this.mobileNav.init();
        this.userMenu.init();
        
        // Add event listeners for common functionality
        document.addEventListener('DOMContentLoaded', () => {
            console.log('WaHub App initialized');
        });
        
        // Listen for Blazor navigation events to reapply theme
        this.setupBlazorNavigationListener();
    },
    
    // Setup Blazor navigation listener
    setupBlazorNavigationListener: function() {
        const self = this;
        
        // Listen for enhancedload event (Blazor enhanced navigation)
        document.addEventListener('enhancedload', function() {
            setTimeout(() => {
                self.theme.reinitialize();
            }, 100);
        });
        
        // Listen for locationchanged event (standard Blazor navigation)
        document.addEventListener('locationchanged', function() {
            setTimeout(() => {
                self.theme.reinitialize();
            }, 100);
        });
        
        // Fallback: Use MutationObserver to detect DOM changes
        if (typeof MutationObserver !== 'undefined') {
            const observer = new MutationObserver(function(mutations) {
                let shouldReapply = false;
                mutations.forEach(function(mutation) {
                    if (mutation.type === 'childList' && mutation.addedNodes.length > 0) {
                        for (let node of mutation.addedNodes) {
                            if (node.nodeType === 1 && (node.classList.contains('internal-layout') || node.tagName === 'MAIN')) {
                                shouldReapply = true;
                                break;
                            }
                        }
                    }
                });
                
                if (shouldReapply) {
                    setTimeout(() => {
                        self.theme.reinitialize();
                    }, 50);
                }
            });
            
            observer.observe(document.body, {
                childList: true,
                subtree: true
            });
        }
    }
};

// Auto-initialize when script loads
window.waHubApp.init();

// Apply theme immediately on script load to prevent flashing
(function() {
    const match = document.cookie.match(/(?:^|; )wahub-theme=([^;]*)/);
    const savedTheme = match ? decodeURIComponent(match[1]) : null;
    if (savedTheme && document && document.documentElement) {
        document.documentElement.setAttribute('data-theme', savedTheme);
    }
})();

// Add cookie change listener (not available natively, so we'll use a custom approach)
// Note: Cookies don't have a native change event like localStorage

// Expose functions for Blazor interop
window.toggleMobileNav = () => window.waHubApp.mobileNav.toggle();
window.closeMobileNav = () => window.waHubApp.mobileNav.close();
window.toggleTheme = () => window.waHubApp.theme.toggle();
window.reinitializeTheme = () => window.waHubApp.theme.reinitialize();
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
