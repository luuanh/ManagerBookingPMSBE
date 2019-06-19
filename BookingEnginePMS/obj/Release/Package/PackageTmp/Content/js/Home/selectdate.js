var urlGetLanguage = '/Home/GetLanguage';
var urlGetCurrency = '/Home/GetCurrency';
var urlGetHotels = '/Home/GetHotels';

app.controller("controller", ['$scope', '$http', '$timeout', 'helper',
    function ($scope, $http, $timeout, helper) {
        $scope.lang = 'vi';
        $scope.hotelId = 1;
        $scope.currency = 'VND';
        $scope.hotelKey = '';
        $scope.filter = {
            adult: 2,
            child: 0,
            fromDate: '',
            toDate: ''
        };
        $scope.languages = [];
        $scope.currencies = [];
        $scope.changeLanguage = function (key) {
            $scope.lang = key;
            $scope.loadChangePage();
        };
        $scope.changeCurrency = function (currencyCode) {
            $scope.currency = currencyCode;
            $scope.loadChangePage();
        };
        $scope.changeHotel = function () {
            $scope.loadChangePage();
        };
        $scope.loadChangePage = function () {
            var hotelCode = $scope.hotels.find(x => x.HotelId == $scope.hotelId).Code
            var newLink = "/Home/SelectDate?hotelKey=" + $scope.hotelKey +
                "&hotelCode=" + hotelCode +
                "&lang=" + $scope.lang +
                "&currency=" + $scope.currency;
            location.href = newLink;
        }
        $scope.getLanguage = function () {
            $http({
                url: urlGetLanguage,
                method: 'GET'
            }).then(function success(respone) {
                $scope.languages = respone.data;
            }, function error(respone) {
            });
        };
        $scope.getCurrency = function () {
            $http({
                url: urlGetCurrency,
                method: 'GET'
            }).then(function success(respone) {
                $scope.currencies = respone.data;
            }, function error(respone) {
            });
        };
        $scope.GetHotels = function () {
            $scope.hotelKey = helper.findGetParameter("hotelKey");
            $http({
                url: urlGetHotels,
                method: 'GET',
                params: {
                    hotelKey: $scope.hotelKey
                }
            }).then(function success(respone) {
                $scope.hotels = respone.data;
            }, function error(respone) {
            });
        };
        $scope.initpage = function () {
            $scope.hotelId = $("#hotelId").val();
            var lang = helper.findGetParameter("lang");
            var currency = helper.findGetParameter("currency");
            if (lang != null)
                $scope.lang = lang;
            if (currency != null)
                $scope.currency = currency;

            $timeout(function () {
                var dateNow = new Date();
                var todate = new Date();
                todate.setDate(dateNow.getDate() + 1);

                var day = $("#date-CalendarAvailability .first-specialDate").text();
                var month = $("#date-CalendarAvailability .first-specialDate").attr("data-month");
                var year = $("#date-CalendarAvailability .first-specialDate").attr("data-year");
                var date = new Date(dateNow.getFullYear(), dateNow.getMonth(), dateNow.getDate());
                var date2 = new Date(todate.getFullYear(), todate.getMonth(), todate.getDate());
                $("#Arrive").val($.datepicker.formatDate('D, M d yy', date));
                $("#Depart").val($.datepicker.formatDate('D, M d yy', date2));
            }, 100);
        };
        $scope.initpage();
        $scope.getLanguage();
        $scope.getCurrency();
        $scope.GetHotels();

        // select room
        $scope.formatDate = function (date) {
            return date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate();
        }
        $scope.selectRoom = function () {
            var hotelCode = $scope.hotels.find(x => x.HotelId == $scope.hotelId).Code
            var fromDate = $scope.formatDate(new Date($("#Arrive").val()))
            var toDate = $scope.formatDate(new Date($("#Depart").val()))
            var linkSelectRoom = "/Home/SelectRoom?hotelKey=" + $scope.hotelKey +
                "&hotelCode=" + hotelCode +
                "&fromDate=" + fromDate +
                "&toDate=" + toDate +
                "&adults=" + $scope.filter.adult +
                "&child=" + $scope.filter.child +
                "&lang=" + $scope.lang +
                "&currency=" + $scope.currency;
            location.href = linkSelectRoom;
        }
    }]);