
chrome.runtime.onMessage.addListener(function(message, sender, sendResponse) {
    document.activeElement.value=message;
});