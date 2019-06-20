var app = angular.module('app', ['ui.grid', 'ngSanitize', 'ui.grid.selection', 'ui.grid.cellNav',
    'ui.grid.resizeColumns', 'ui.grid.moveColumns', 'ngTouch', 'ui.grid.autoResize',
    'ui.grid.pinning', 'ui.grid.pagination', 'ui.grid.exporter']);
app.service('validate', function () {
    this.isNullOrEmptySingleShowError = function (data) {
        var ok = data.value == null || data.value == "";
        if (ok)
            $("#err_" + data.key).css("display", "block");
        else
            $("#err_" + data.key).css("display", "none");
        return ok
    }
    this.isNotNumberSingleShowError = function (data) {
        var ok = !(parseInt(data.value) % 1 == 0);
        if (ok)
            $("#err_" + data.key).css("display", "block");
        else
            $("#err_" + data.key).css("display", "none");
        return ok
    }
    this.isNotHour = function (data) {
        var ok = true;
        var result = data.value == null ||
            data.value == '' ||
            data.value.indexOf('_') >= 0
        if (!result) {
            var hourMinute = data.value.split(':');
            if (hourMinute[0] < 24 && hourMinute[1] <= 59)
                ok = false;
        }
        if (ok)
            $("#err_" + data.key).css("display", "block");
        else
            $("#err_" + data.key).css("display", "none");
        return ok
    }
    this.isNotDate = function (data) {
        var ok = true;
        var result = data.value == null ||
            data.value == ''
        if (!result) {
            ok = false;
        }
        if (ok)
            $("#err_" + data.key).css("display", "block");
        else
            $("#err_" + data.key).css("display", "none");
        return ok
    }
    this.isEmail = function (data) {
        if (this.isNullOrEmptySingleShowError(data))
            return true;
        var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        var ok = !regex.test(data.value);
        if (ok)
            $("#err_" + data.key).css("display", "block");
        else
            $("#err_" + data.key).css("display", "none");
        return ok;
    }
});
app.service('helper', function () {
    this.getAllValueSelect = function (id) {
        var assignedRoleId = new Array();
        $('#id option').each(function () {
            assignedRoleId.push(this.value);
        });
        return assignedRoleId;
    }
    this.unSign = function (str) {
        if (str === null || str === '') return ''
        str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
        str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
        str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
        str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
        str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
        str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
        str = str.replace(/đ/g, "d");
        str = str.replace(/À|Á|Ạ|Ả|Ã|Â|Ầ|Ấ|Ậ|Ẩ|Ẫ|Ă|Ằ|Ắ|Ặ|Ẳ|Ẵ/g, "A");
        str = str.replace(/È|É|Ẹ|Ẻ|Ẽ|Ê|Ề|Ế|Ệ|Ể|Ễ/g, "E");
        str = str.replace(/Ì|Í|Ị|Ỉ|Ĩ/g, "I");
        str = str.replace(/Ò|Ó|Ọ|Ỏ|Õ|Ô|Ồ|Ố|Ộ|Ổ|Ỗ|Ơ|Ờ|Ớ|Ợ|Ở|Ỡ/g, "O");
        str = str.replace(/Ù|Ú|Ụ|Ủ|Ũ|Ư|Ừ|Ứ|Ự|Ử|Ữ/g, "U");
        str = str.replace(/Ỳ|Ý|Ỵ|Ỷ|Ỹ/g, "Y");
        str = str.replace(/Đ/g, "D");
        return str;
    }
    this.findGetParameter = function (parameterName) {
        var result = null,
            tmp = [];
        location.search
            .substr(1)
            .split("&")
            .forEach(function (item) {
                tmp = item.split("=");
                if (tmp[0] === parameterName) result = decodeURIComponent(tmp[1]);
            });
        return result;
    }
    this.round = function (data, number = 0) {
        return parseFloat(data).toFixed(number)
    }
    this.getArrDate = function (date) {
        var arr = date.split('T');
        var dateArr = arr[0].split('-');
        var time = arr[1].split(':');
        return {
            day: dateArr[2],
            month: dateArr[1],
            year: dateArr[0],
            hour: time[0],
            minute: time[1],
            second: time[2]
        };
    };
});
app.directive('boolean', function () {
    return {
        priority: '50',
        require: 'ngModel',
        link: function (_, __, ___, ngModel) {
            ngModel.$parsers.push(function (value) {
                return value == 'true' || value == true;
            });

            ngModel.$formatters.push(function (value) {
                return value && value != 'false' ? 'true' : 'false';
            });
        }
    };
});
app.directive("datepicker", function () {

    function link(scope, element, attrs) {
        // CALL THE "datepicker()" METHOD USING THE "element" OBJECT.
        element.datepicker({
            dateFormat: "dd-mm-yy",
            changeMonth: true,
            changeYear: true,
            yearRange: "-100:+50",
        });
    }

    return {
        require: 'ngModel',
        link: link
    };
});
app.directive('convertToNumber', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ngModel) {
            ngModel.$parsers.push(function (val) {
                return val !== null ? parseInt(val, 10) : null;
            });
            ngModel.$formatters.push(function (val) {
                return val !== null ? '' + val : null;
            });
        }
    };
})
app.directive('tooltip', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            $(element).hover(function () {
                // on mouseenter
                $(element).tooltip('show');
            }, function () {
                // on mouseleave
                $(element).tooltip('hide');
            });
        }
    };
})
app.directive('onSizeChanged', ['$window', function ($window) {
    return {
        restrict: 'A',
        scope: {
            onSizeChanged: '&'
        },
        link: function (scope, $element, attr) {
            var element = $element[0];

            cacheElementSize(scope, element);
            $window.addEventListener('resize', onWindowResize);

            function cacheElementSize(scope, element) {
                scope.cachedElementWidth = element.offsetWidth;
                scope.cachedElementHeight = element.offsetHeight;
            }

            function onWindowResize() {
                var isSizeChanged = scope.cachedElementWidth != element.offsetWidth || scope.cachedElementHeight != element.offsetHeight;
                if (isSizeChanged) {
                    var expression = scope.onSizeChanged();
                    //expression();
                }
            };
        }
    }
}]);
app.directive('setGridHeight', function () {
    return {
        'scope': false,
        'link': function (scope, element, attrs) {
            /*Can Manipulate the height here*/
            attrs.$set("style", "height: 600px");
        }
    };
})
var statusMapping = [
    "<span class='uk-badge-active'>" + 'Kích hoạt' + "</span>",
    "<span class='uk-badge-looked'>" + 'Đã khóa' + "</span>",
    "<span class='uk-badge-used'>" + 'Đang sử dụng' + "</span>"
];
var statusHotel = [
    "<span class='text-green'>" + 'Dùng Thử' + "</span>",
    "<span class='text-light-blue'>" + 'Dùng Thật' + "</span>",
    "<span class='text-yellow'>" + 'Hết Hạn' + "</span>",
    "<span class='text-red'>" + 'Đã Khóa' + "</span>",
];
var typePaymentHotel = [
  
    "<span class='text-green'>" + 'Tháng' + "</span>",
    "<span class='text-green'>" + 'Tháng' + "</span>",
    "<span class='text-yellow'>" + 'Phần trăm' + "</span>",
    "<span class='text-green'>" + 'Tháng' + "</span>",
    "<span class='text-green'>" + 'Tháng' + "</span>"
];
var getFeedBack = [
    "<span class='uk-badge-looked'>" + 'Chưa phản hồi' + "</span>",
    "<span class='uk-badge-active'>" + 'Đã phản hồi' + "</span>"
];
var statusExtrabed = [
    "<span class='uk-icon-check icon-check'></span>",
    "<span class='icon-uncheck'>__</span>"
];
var typeTaxFee = [
    "VAT",
    "Service charge"
];
var statusReservation = [
    "<span class='uk-text-bold title-booking-new'>" + 'Mới' + "</span>",
    "<span class='uk-text-bold title-booking-inhouse'>" + 'Đã CheckIn' + "</span>",
    "<span class='uk-text-bold uk-text-end'>" + 'Đã CheckOut' + "</span>",
    "<span class='uk-text-bold uk-text-warning'>" + 'Khách không đến' + "</span>",
    "<span class='uk-text-bold uk-text-danger'>" + 'Đã hủy' + "</span>",
    "<span class='uk-text-bold uk-text-danger'>" + 'Chuyển công nợ' + "</span>"
];
var statusBooking = [
    "#3ABB79",
    "#358ec9",
    "#9F9F9F",
    "#FFFFB2",
    "linear-gradient(rgba(255, 26, 26, 0.68), rgba(148, 12, 12, 0.62))"
]
var sourceReservation = [
    "Walk-in / Telephone",
    "Online Booking Engine",
    "Booking.com",
    "Expedia",
    "Agoda",
    "Trip Connect",
    "AirBNB",
    "Hostelworld",
    "Myallocator",
    "Company",
    "Guest Member",
    "Owner",
    "Returning Guest",
    "Apartment",
    "Siteminder",
    "Other Travel Agency"
]
var typeReservation = [
    "Ngày",
    "Giờ"   
]
var statusCashDrawer = [
    "<span class='uk-text-bold uk-text-danger'>" + 'Đóng' + "</span>",
    "<span class='uk-text-bold title-booking-new'>" + 'Mở' + "</span>"
]