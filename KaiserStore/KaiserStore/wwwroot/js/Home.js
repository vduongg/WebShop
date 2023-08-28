const list = document.querySelectorAll(".item");

const next = document.getElementById("button-next");
const back = document.getElementById("button-back");
const numOfSlide = list.length;

var slideNum = 0;

function nextBtn() {
    list.forEach((item) => {
        item.classList.remove("active");
    });
    slideNum++;
    if (slideNum > (numOfSlide - 1)) {
        slideNum = 0;
    }
    list[slideNum].classList.add("active");
}

next.addEventListener("click", nextBtn);


back.addEventListener("click", () => {

    list.forEach((item) => {
        item.classList.remove("active");
    });
    slideNum--;
    if (slideNum < 0) {
        slideNum = numOfSlide - 1;
    }
    list[slideNum].classList.add("active");
})
var sliderShow = setInterval(sliderAuto, 4000);
function sliderAuto() {
    nextBtn();
}

