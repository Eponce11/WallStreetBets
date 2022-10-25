
// opens the modal with all the stock info passed in
function openModalView(stock) {
    const openPrice = parseFloat(stock.open);
    const closePrice = parseFloat(stock.close);
    const percentage = ((closePrice - openPrice) / openPrice) * 100;

    $("#modal-ticker").html(` ${stock.symbol}`);
    $("#modal-open").html(`Open: $${stock.open}`);
    $("#modal-close").html(`Close: $${stock.close}`);
    $("#modal-percentage").html(`${percentage}%`);
    $("#modal-high").html(`High: $${stock.high}`);
    $("#modal-low").html(`Low: $${stock.low}`);
    $("#modal-volume").html(`Volume: ${stock.volume}`);

    $("#bg-modal").css("display", "flex");
}

// closes the modal view
function closeModalView() {
    document.querySelector(".bg-modal").style.display = 'none';
}

// api calls the backend route that adds or removes the stock from the users list
function addRemoveStock() {

    const ticker = $("#search-ticker-input").val().toUpperCase();
    $.ajax({
        type: "POST",
        url: "/watchlistaddremove",
        data: {ticker: ticker},
        success: function (response) { console.log(response) },
        error: function (error) { console.log(error) }
    })
}

// api calls the backend route that returns a stock if the ticker that was passed in was found
function searchTicker() {

    const ticker = $("#search-ticker-input").val().toUpperCase();

    $.ajax({
        type: "POST",
        url: "/searchstock",
        data: {ticker: ticker},
        success: function (response) {
            // if the stock was not found
            if (response.symbol === undefined){
                $("#search-error-comment").html("Ticker Not Found");
                return
            }

            // if the stock was found
            $("#search-error-comment").html("");
            openModalView(response);
        },
        error: function (error) {
            console.log(error);
        }
    })
}
