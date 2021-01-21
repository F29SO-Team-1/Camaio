 var toggleColorMode = function toggleColorMode(e) {
    // Switch to Light Mode
    if (e.currentTarget.classList.contains("light--hidden")) {
        // Sets the custom html attribute
        document.documentElement.setAttribute("color-mode", "light");
        // Sets the user's prefernce in local storage 
        localStorage.setItem("color-mode", "light");
        return; 
    }

    // Switch to Dark Mode
    // Sets the custom html attribute 
    document.documentElement.setAttribute("color-mode", "dark"); 
    // Set the users prefernce in local storage 
    localStorage.setItem("color-mode", "dark"); 
};

// Get the buttons in the DOM
var toggleColorButtons = document.querySelectorAll(".color-mode_btn");

// Set up even listeners
toggleColorButtons.forEach(function(btn) {
    btn.addEventListener("click", toggleColorMode); 
});

