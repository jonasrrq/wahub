// Language selector functions for client-side WebAssembly

// Function to set language cookie (used by Blazor Interop)
window.setLanguageCookie = function(cookieName, language) {
    const date = new Date();
    date.setTime(date.getTime() + (365 * 24 * 60 * 60 * 1000)); // 1 year expiry
    const expires = "; expires=" + date.toUTCString();
    const cookieValue = `c=${language}|uic=${language}`;
    document.cookie = `${cookieName}=${cookieValue}${expires}; path=/`;
    console.log(`Cookie '${cookieName}' set to: ${cookieValue}`); // For debug
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
