const loadMoreBtn = document.getElementById("loadMoreBtn");
const productBox = document.getElementById("productBox");
const productCount = document.getElementById("productCount").value;


let skip = 8
loadMoreBtn.addEventListener("click", function () {
    let url = `/Shop/LoadMore?skip=${skip}`;
    
    fetch(url).then(response => response.text()).then(data => productBox.innerHTML += data )
    skip += 8;

    if (skip >= productCount) {
        loadMoreBtn.remove();
    }
})