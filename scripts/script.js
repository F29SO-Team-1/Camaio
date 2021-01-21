const toggleColorMode = e => {
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
const toggleColorButtons = document.querySelectorAll(".color-mode_btn");

// Set up even listeners
toggleColorButtons.forEach(btn => {
    btn.addEventListener("click", toggleColorMode)
});













const toggleSwitch = document.querySelector('.theme-switch input[type="checkbox"]');

function switchTheme(e) {
    if (e.currenttarget.checked) {
        document.documentElement.setAttribute('data-theme', 'dark');
    }
    else {
        document.documentElement.setAttribute('data-theme', 'light');
    }    
}

toggleSwitch.addEventListener('change', switchTheme, false);



