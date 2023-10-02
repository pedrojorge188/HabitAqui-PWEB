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