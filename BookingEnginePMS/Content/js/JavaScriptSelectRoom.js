
function ShowLoading() {
    $("#wraploadding").show();
    $("#wraploadding").css("width", $(window).width());
    $("#wraploadding").css("height", $(window).height());
    $("#imgloadding ").css("top", ($(window).height() / 2) - 80);
}
function ShowGalleryRoom(param) {
    var detailPrice = $("#w-gallery-room-" + param);
    detailPrice.dialog("open");
}
function ChangePrice(param) {

    var numberroom = param.val();
    if (numberroom >= 1) {
        $("#text").css("display", "block");
    } else {
        $("#text").css("display", "none");
    }

    if (numberroom == 0) {
        numberroom = 1;

    }
    var priceId = param.data("priceid");
    var detailPriceSort = $("#price-item-room-" + priceId);
    var detailPriceLong = $("#w-detail-price-room-" + priceId);

    var price = detailPriceSort.find(".pzt-price-room").data("price") * numberroom;
    //Sort Detail
    detailPriceSort.find(".pzt-price-room").text(price.toFixed(2));

    var sale = detailPriceSort.find(".pzt-sale-room").data("sale");
    if (sale > 0) {
        var priceSale = sale * 0.01 * price;
        detailPriceSort.find(".pzt-sale-room").text(priceSale.toFixed(2));
        detailPriceSort.find(".pzt-left-price").text((price - priceSale).toFixed(2));
        // Long Detail
        detailPriceLong.find(".pxt-price-sale").text(priceSale.toFixed(2));
        detailPriceLong.find(".pxt-left-price").text((price - priceSale).toFixed(2));
    }
    // Long Detail
    if (numberroom > 1) {

        detailPriceLong.find(".more-number-room").show();
        detailPriceLong.find(".pxt-number-room").text(numberroom);
        detailPriceLong.find(".pxt-price-room").text(price.toFixed(2));
    } else {
        detailPriceLong.find(".more-number-room").hide();
        detailPriceLong.find(".pxt-number-room").text(numberroom);
        detailPriceLong.find(".pxt-price-room").text(price.toFixed(2));
    }

}
function ShowDetailPrice(param) {
    var detailPrice = $("#w-detail-price-room-" + param);
    detailPrice.dialog("open");
}

function ShowDetailInfoRoom(param) {
    var detailInfoRoom = $("#detail-info-room-" + param);
    detailInfoRoom.dialog("open");
}
function ChecSelectRoom(param) {
    var form = param.closest("form.fr-select-room");
    var selectRoom = form.find(".be-number-room option:checked").val();
    if (selectRoom <= 0) {
        form.find(".be-number-room").css("border", "1px solid red");
        var error = form.find(".be-error");
        error.text("Please select no. of rooms");
        error.show();
    } else {
        form.submit();
    }
}

//function ShowRoom(param) {
//    var code = $(param).attr("id");
//    var listRoom = $("#list-room-" + code);
//    if (listRoom.css('display') == 'none') {
//        listRoom.show(400);
//        $(param).text("Hide Rooms");
//    } else {
//        listRoom.hide(400);
//        $(param).text("See ALL Rooms");
//    }
//}

function readysearchroom() {
    if ($(".av-listhotel option").length == 1) {
        $("#av-wrap-list-hotel").hide();
    }

    $(".w-detail-price-room").dialog(
        {
            autoOpen: false,
            modal: true,
            width: 438,
            title: 'Price Details'
        });
    $(".detail-info-room").dialog(
        {
            autoOpen: false,
            modal: true,
            width: 600,
            title: 'ROOM DESCRIPTION & INCLUSIONS'
        });
    $(".w-room-gallery").dialog(
        {
            autoOpen: false,
            modal: true,
            width: 730,
            title: 'Room gallery',
        });
    $(".owl-demo").owlCarousel({
        navigation: true, // Show next and prev buttons
        slideSpeed: 300,
        paginationSpeed: 400,
        singleItem: true,
        autoHeight: true,
        autoWidth: true,
        items: 1,
    });
    if ($(window).height() < 767) {
        $(function () {
            $("#accordion").accordion({
                icons: { "header": "ui-icon-circle-triangle-n", "activeHeader": "ui-icon-circle-triangle-s" },
                collapsible: true,
                active: true,
            });
        });
    }
    //$("#HotelId").change(function () {
    //    ShowLoading();
    //    var url = "https://@company.Website/ChangeHotel?code=WHT8888&hotel=" + $("#HotelId").val();
    //    $(location).attr('href', url);
    //});
        }
$(document).ready(function () {
    if ($(".av-listhotel option").length == 1) {
        $("#av-wrap-list-hotel").hide();
    }

    $(".w-detail-price-room").dialog(
        {
            autoOpen: false,
            modal: true,
            width: 438,
            title: 'Price Details'
        });
    $(".detail-info-room").dialog(
        {
            autoOpen: false,
            modal: true,
            width: 600,
            title: 'ROOM DESCRIPTION & INCLUSIONS'
        });
    $(".w-room-gallery").dialog(
        {
            autoOpen: false,
            modal: true,
            width: 730,
            title: 'Room gallery',
        });
    $(".owl-demo").owlCarousel({
        navigation: true, // Show next and prev buttons
        slideSpeed: 300,
        paginationSpeed: 400,
        singleItem: true,
        autoHeight: true,
        autoWidth: true,
        items: 1,
    });
    if ($(window).height() < 767) {
        $(function () {
            //$("#accordion").accordion({
            //    icons: { "header": "ui-icon-circle-triangle-n", "activeHeader": "ui-icon-circle-triangle-s" },
            //    collapsible: true,
            //    active: true,
            //});
        });
    }
    //$("#HotelId").change(function () {
    //    ShowLoading();
    //    var url = "https://@company.Website/ChangeHotel?code=WHT8888&hotel=" + $("#HotelId").val();
    //    $(location).attr('href', url);
    //});
    });