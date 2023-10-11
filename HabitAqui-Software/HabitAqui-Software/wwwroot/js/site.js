// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const toggleButton = document.getElementById('toggleSidebar');
const sidebar = document.querySelector('.sidebar');
const toggleThemeButton = document.getElementById('toggleThemeButton');
let isSidebarVisible = true;
let darkMode = true;

setTimeout(function () {
    document.querySelector('.preloader').style.display = 'none';
    preloader.classList.add('hidden');
}, 700); 

toggleThemeButton.addEventListener('click', () => {
    var body = document.body;

    if (!darkMode) {
        body.style.backgroundColor = "#343a40";
        body.style.color = "#ffffff";
        darkMode = true;

    } else {
        body.style.backgroundColor = "white";
        body.style.color = "#000000";
        darkMode = false;

    }
});

toggleButton.addEventListener('click', () => {


    if (isSidebarVisible) {
        sidebar.style.display = 'none'; // Oculta a barra lateral
    } else {
        sidebar.style.display = 'block'; // Mostra a barra lateral
    }

    isSidebarVisible = !isSidebarVisible;
});

document.addEventListener("DOMContentLoaded", function () {

    const toggleFilterButton = document.getElementById("toggleFilter");
    const filterForm = document.getElementById("filterForm");

    toggleFilterButton.addEventListener("click", function () {

        if (filterForm.classList.contains("show-filter")) {
            filterForm.classList.remove("show-filter");
            toggleFilterButton.textContent = "Filtrar";
        } else {
            filterForm.classList.add("show-filter");
            toggleFilterButton.textContent = "Ocultar";
        }

    });

    const toggleOrderButton = document.getElementById("toggleOrder");
    const orderForm = document.getElementById("orderForm");

    toggleOrderButton.addEventListener("click", function () {

        if (orderForm.classList.contains("show-filter")) {
            orderForm.classList.remove("show-filter");
        } else {
            orderForm.classList.add("show-filter");
        }

    });

});
