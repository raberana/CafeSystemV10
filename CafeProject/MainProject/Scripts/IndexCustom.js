window.onload = function swapPic() {
    var imgSrc = [];
    imgSrc[0] = "/Content/Resources/cafe1.jpg";
    imgSrc[1] = "/Content/Resources/cafe2.jpg";
    imgSrc[2] = "/Content/Resources/cafe3.jpg";
    imgSrc[3] = "/Content/Resources/cafe4.jpg";
    imgSrc[4] = "/Content/Resources/cafe5.jpg";
    imgSrc[5] = "/Content/Resources/cafe6.jpg";

    var randomnumber = Math.floor(Math.random() * 5);
    var img = document.getElementById("imgContainer");
    img.setAttribute("src", imgSrc[randomnumber]);
}