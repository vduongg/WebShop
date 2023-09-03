const list = document.querySelectorAll('.side__link');
sidebar = document.querySelector(".sidebar");

function acviteLink() {
    list.forEach((item) => item.classList.remove('active'));
    this.classList.add('active');
}
list.forEach((item) =>
    item.addEventListener('click', acviteLink));

