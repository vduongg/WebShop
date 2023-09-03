const btn_add = document.getElementById("add-category");
const btn_close = document.getElementById("close");
btn_add.addEventListener('click', () => {
    document.getElementById("popup").style.display = "flex";
});
btn_close.addEventListener('click', () => {
    document.getElementById("popup").style.display = "none";
})
