var activeButton = document.querySelector('#bank-page');


function changeActiveButton(){
    activeButton.classList.replace("nav-button","nav-active-button");
}

document.addEventListener("DOMContentLoaded", changeActiveButton);