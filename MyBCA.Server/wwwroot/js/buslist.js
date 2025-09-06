/**
 * @returns {boolean} Whether the current environment is the Instagram in-app browser.
 */
function isInstagramBrowser() {
    return /Instagram/.test(navigator.userAgent);
}

/**
 * @returns {boolean} Whether the current environment is a mobile browser.
 */
function isMobileBrowser() {
    return /Mobi|Android|iPhone|iPod/i.test(navigator.userAgent);
}

/**
 * @returns {boolean} Whether the current environment is a standalone PWA.
 */
function isPwa() {
    return window.matchMedia("(display-mode: standalone)").matches;
}

if (isMobileBrowser() && !isPwa()) {
    console.log("Not running in PWA environment, but mobile browser detected. Showing PWA install instructions.");
    document.getElementById("pwa-install-guide").style.display = "block";
}

if (isInstagramBrowser()) {
    console.log("Running in Instagram in-app browser. Showing warning.");
    document.getElementById("instagram-browser-warning").style.display = "block";
}