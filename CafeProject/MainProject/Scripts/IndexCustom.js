window.onload = function pageLoads() {
    var url = document.URL;
    var urlComponents = url.split("/");
    
    if (urlComponents.length <= 4)
        swapPic();
    else
    {
        switch (urlComponents[4])
        {
            case "AboutPage": setActiveUrlStyle("AboutPage");
                break;

            case "FaqPage": setActiveUrlStyle("FaqPage");
                break;

            case "HoursLocationPage": setActiveUrlStyle("HoursLocationPage");
                break;

            case "MenuPage": setActiveUrlStyle("MenuPage");
                break;

            case "OrderIndex": setActiveUrlStyle("OrderIndex");
                break;

        }
    }
}

function swapPic()
{
    document.getElementsByClassName("Menu")[0].style.paddingBottom = "20px";
    var imgSrc = [];
    imgSrc[0] = "Content/Resources/cafe1.jpg";
    imgSrc[1] = "Content/Resources/cafe2.jpg";
    imgSrc[2] = "Content/Resources/cafe3.jpg";
    imgSrc[3] = "Content/Resources/cafe4.jpg";
    imgSrc[4] = "Content/Resources/cafe5.jpg";
    imgSrc[5] = "Content/Resources/cafe6.jpg";

    var randomnumber = Math.floor(Math.random() * 6);
    var img = document.getElementById("imgContainer");
    img.style.clear = "both";
    img.style.minHeight = "470px";
    img.style.minWidth = "950px";
    img.style.backgroundImage = "url(" + imgSrc[randomnumber] + ")";
}

function setActiveUrlStyle(idContainer)
{
    var activeButton = document.getElementById(idContainer).style;
    var buttons = document.getElementsByClassName("IndexListButtons");
    
    var ctr = 0;
    for (ctr = 0; ctr < buttons.length; ctr++)
    {
        //buttons[ctr].removeAttribute("src");
        buttons[ctr].className = "IndexListButtons"
    }

    activeButton.fontWeight = "bold";
    activeButton.color="black";
    activeButton.borderTop = "5px solid #5F9EA0";
    activeButton.borderBottom = "20px solid #5F9EA0";
    activeButton.margin = "-5px auto";
    document.getElementsByClassName("Menu")[0].style.paddingBottom = "5px";
}