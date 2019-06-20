
$(document).ready(function () {
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'get',
        url: '/api/search-item/get-cruise',

        success: function (data) {
            var dataCruiseType = data.cruiseTourType;
            var menuCruise = data.menu;
            var option = '<option value="41"></option>';
            for (var i = 0; i < menuCruise.length; i++) {
                $('.menuCruise').append('<option value="' + menuCruise[i].MenuId + '">' + menuCruise[i].MenuName + '</option>');
            }

            for (var y = 0; y < dataCruiseType.length; y++) {
                $('.dataCruiseType').append('<option value="' + dataCruiseType[y].CruiseTourTypeId + '">' + dataCruiseType[y].CruiseTourTypeName + '</option>');
            }
        },
        error: function () {
            console.log("Error");
        }
    });

    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'get',
        url: '/api/search-item/get-tour2',

        success: function (data) {
            var TourType = data.tourtype;
            var menuTour = data.menutour;
            var option = '<option value="41"></option>';
            for (var i = 0; i < menuTour.length; i++) {
                $('.menuTour').append('<option value="' + menuTour[i].MenuId + '">' + menuTour[i].MenuName + '</option>');

            }

            for (var y = 0; y < TourType.length; y++) {
                $('.TourType').append('<option value="' + TourType[y].TourId + '">' + TourType[y].TourName + '</option>');
            }
        },
        error: function () {
            console.log("Error");
        }
    });

    

});
