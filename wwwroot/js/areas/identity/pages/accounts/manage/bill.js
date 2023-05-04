function numberWithCommas(x) {
    var parts = x.toString().split(".");
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ".");
    return parts.join(",");
}
$(document).ready(function () {
    $(".tblcontent").each(function () {
        let totalPoints = 0;
        $(this).find("tr").each(function () {
            $(this).find(".total").each(function () {
                let splited = $(this).html().split('.').join("");
                totalPoints += parseInt(splited);
            });
        })
        $(this).find(".phivanchuyen").html(numberWithCommas(parseInt($(this).find(".tongthanhtoan").text() - totalPoints)));
        $(this).find(".tongthanhtoan").html(numberWithCommas(parseInt($(this).find(".tongthanhtoan").text())));

    })

    $("#history").addClass("chosen1");
    $(".tbl").each(function () {
        $(this).find("tr").each(function () {
            $(this).find(".price").html(numberWithCommas($(this).find(".price").text()));
            $(this).find(".total").html(numberWithCommas($(this).find(".total").text()));
        })
    })
})