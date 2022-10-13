

function openFullView() {
    
    document.querySelector(".bg-modal").style.display = 'flex';
}

function closeFullView() {
    document.querySelector(".bg-modal").style.display = 'none';
}

function addRemoveStock(ticker) {
    $.ajax({
        type: "POST",
        url: "/watchlistaddremove",
        data: {ticker: ticker},
        success: function (response) { console.log(response) },
        error: function (error) { console.log(error) }
    })
}


function searchTicker() {

    const ticker = $("#search-ticker-input").val().toUpperCase();

    $.ajax({
        type: "POST",
        url: "/searchstock",
        data: {ticker: ticker},
        success: function (response) {
            if (response.symbol === undefined){
                $("#search-error-comment").html("Ticker Not Found");
                return
            }

            $("#search-error-comment").html("");

            const openPrice = parseFloat(response.open);
            const closePrice = parseFloat(response.close);
            const percentage = ((closePrice - openPrice) / openPrice) * 100;

            $("#modal-ticker").html(` ${response.symbol}`);
            $("#modal-open").html(`Open: $${response.open}`);
            $("#modal-close").html(`Close: $${response.close}`);
            $("#modal-percentage").html(`${percentage}%`);
            $("#modal-high").html(`High: $${response.high}`);
            $("#modal-low").html(`Low: $${response.low}`);
            $("#modal-volume").html(`Volume: ${response.volume}`);

            $("#bg-modal").css("display", "flex");

        },
        error: function (error) {
            console.log(error);
        }
    })
}