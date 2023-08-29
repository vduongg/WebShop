const list = document.querySelectorAll('.side__link');
sidebar = document.querySelector(".sidebar");
search = document.querySelector(".search");
love = document.querySelector(".love");
function acviteLink() {
    list.forEach((item) => item.classList.remove('active'));
    this.classList.add('active');
}
list.forEach((item) =>
    item.addEventListener('click', acviteLink));
function resetIcon() {
    document.getElementById('home-img').src = "image/home-img-white.svg";
    document.getElementById('search-img').src = "image/search-img-white.svg";
    document.getElementById('discover-img').src = "image/discover-img-white.svg";
    document.getElementById('reel-img').src = "image/reel-img-white.svg";
    document.getElementById('mess-img').src = "image/mess-img-white.svg";
    document.getElementById('love-img').src = "image/love-img-white.svg";
    document.getElementById('add-img').src = "image/add-img-white.svg";
    document.getElementById('more-img').src = "image/more-img-white.svg";
}
function clickAvatar() {
    resetIcon();
    document.getElementById('avatar').style.border = "2px solid black";
}
function ClickMenu(id) {

    resetIcon();
    document.getElementById('avatar').style.border = "none";
    document.getElementById(id).src = "image/" + id + "-black.svg";


    if (id === 'search-img') {

        sidebar.classList.add('close');
        search.classList.add('open');
        love.classList.remove('open');
    }
    else {
        search.classList.remove('open');
        sidebar.classList.remove('close');
    }
    if (id === 'love-img') {

        sidebar.classList.add('close');
        love.classList.add('open');
        search.classList.remove('open');
    }
    else {
        love.classList.remove('open');
    }
}
document.onclick = function (e) {
    if (e.target.id !== 'search' && e.target.id !== 'search-img' && e.target.id !== 'search-title' && e.target.id !== 'search-input' && e.target.id !== 'search-span' && e.target.id !== 'line-search' &&
        e.target.id !== 'popup-menu' && e.target.id !== 'title' && e.target.id !== "menu-link"
        && e.target.id !== 'love' && e.target.id !== 'love-img' && e.target.id !== 'love-span' && e.target.id !== 'love-title') {
        search.classList.remove('open');
        love.classList.remove('open');
        sidebar.classList.remove('close');

    }
}
