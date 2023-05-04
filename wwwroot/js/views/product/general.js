function numberWithCommas(x) {
    var parts = x.toString().split(".");
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ".");
    return parts.join(",");
}
$(document).ready(function () {
    $(".highlight").ready(function () {
        $(this).find(".card-body").each(function () {
            $(this).find(".pricesale").html(numberWithCommas($(this).find(".pricesale").text()))
            $(this).find(".price").html(numberWithCommas($(this).find(".price").text()))
        })
    })
});