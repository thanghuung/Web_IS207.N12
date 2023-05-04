function numberWithCommas(x) {
    var parts = x.toString().split(".");
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ".");
    return parts.join(",");
}
function updatetotal() {
    let totalPoints = 0;
    $("tr").find(".total-fld").each(function () {
        let splited = $(this).html().split('.').join("");
        totalPoints += parseInt(splited);
    });
    $("#m_total").html(numberWithCommas(totalPoints));
}
$(document).ready(function () {
    $("#pagetitle").html("Giỏ hàng");
    $(".tbl").ready(function () {
        $(this).find("tr").each(function () {
            $(this).find(".price-fld").html(numberWithCommas($(this).find(".price-fld").text()))
            $(this).find(".total-fld").html(numberWithCommas($(this).find(".total-fld").text()))
        })
    })
    updatetotal();
    $(".add").click(function () {
        let quan = parseInt($(this).siblings(".quantity_v").val()) + 1;
        let root = parseInt($(this).parents("tr").find(".root1").text());
        $(this).siblings("input[type=text].quantity_v").val(quan);
        $(this).parents("tr").find(".total-fld").html(numberWithCommas(quan * root));
        updatetotal();
    })
    $(".minus").click(function () {
        if ($(this).siblings("input[type=text].quantity_v").val() > 1) {
            let quan = parseInt($(this).siblings(".quantity_v").val()) - 1;
            let root = parseInt($(this).parents("tr").find(".root1").text());
            $(this).siblings("input[type=text].quantity_v").val(quan);
            $(this).parents("tr").find(".total-fld").html(numberWithCommas(quan * root));
            updatetotal();
        }
        else {
            $(location).attr('href', $(this).parents("tr").find("a.del-btn").attr("href"));
        }
    })
});