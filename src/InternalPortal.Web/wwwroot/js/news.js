var activeButton = document.querySelector('#news-page');


function changeActiveButton(){
    activeButton.classList.replace("nav-button","nav-active-button");
}

document.addEventListener("DOMContentLoaded", changeActiveButton);