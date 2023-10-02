// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const toggleButton = document.getElementById('toggleSidebar');
const sidebar = document.querySelector('.sidebar');

let isSidebarVisible = true;

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

});
