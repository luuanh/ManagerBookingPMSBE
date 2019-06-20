var app = angular.module('app', []);
app.service('validation', function () {
    this.checkRequired = function () {
        var resultCheck = true;
        $("#form_data").each(function () {
            var inputs = $(this).find(':input[required]');
            var i = 0;
            for (i = 0; i < inputs.length; i++) {
                if (inputs[i].value === null || inputs[i].value === '') {
                    inputs[i].classList.add("border-warning");
                    var notify = $(inputs[i].parentElement).find('span.required-field');
                    if (notify.length > 0)
                        notify[0].textContent = "(*) Vui lòng điền vào trường này.";
                    resultCheck = false;
                }
            }
        });
        return resultCheck;
    };
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
    };
    this.checkVisa = function (value) {
        var cardno = /^(?:4[0-9]{12}(?:[0-9]{3})?)$/;
        return value.match(cardno)
    };
    this.checkMasterCard = function (value) {
        var cardno = /^(?:5[1-5][0-9]{14})$/;
        return value.match(cardno)
    };
    this.checkJcb = function (value) {
        var cardno = /^(?:(?:2131|1800|35\d{3})\d{11})$/;
        return value.match(cardno)
    };
    this.checkDiscover = function (value) {
        var cardno = /^(?:6(?:011|5[0-9][0-9])[0-9]{12})$/;
        return value.match(cardno)
    };
    this.checkDiners = function (value) {
        var cardno = /^(?:3(?:0[0-5]|[68][0-9])[0-9]{11})$/;
        return value.match(cardno)
    };
    this.checkAmericalExpress = function (value) {
        var cardno = /^(?:3[47][0-9]{13})$/;
        return value.match(cardno)
    };
});
app.service('helper', function () {
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
    };
    this.round = function (data, number = 2) {
        return parseFloat(data).toFixed(number);
    };
    this.ConvertToUnSign = function (value) {
        var str = value;
        str = str.toLowerCase();
        str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
        str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
        str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
        str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
        str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
        str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
        str = str.replace(/đ/g, "d");
        str = str.replace(/!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'| |\"|\&|\#|\[|\]|~|$|_/g, "-");// tìm và thay thế các kí tự đặc biệt trong chuỗi sang kí tự -
        str = str.replace(/-+-/g, "-");
        str = str.replace(/^\-+|\-+$/g, "");
        return str;
    };
    this.changeSwitchery = function (element, checked) {
        if ((element.is(':checked') && checked == false) || (!element.is(':checked') && checked == true)) {
            element.parent().find('.switchery').trigger('click');
        }
    };
    this.changeRadio = function (element, checked) {
        if ((element.is(':checked') && checked == false) || (!element.is(':checked') && checked == true)) {
            element.parent().find('.iCheck-helper').trigger('click');
        }
    };
});
app.service('notify', function () {
    this.success = function (message) {
        UIkit.notify({
            message: "<i class='icon-result uk-icon-check'></i> " + message + "<i class='icon-add-close uk-icon-close'></i>",
            status: 'success',
            timeout: 5000,
            pos: 'top-right'
        });
    };
    this.error = function (message) {
        UIkit.notify({
            message: "<i class='icon-result uk-icon-close'></i> " + message + "<i class='icon-add-close uk-icon-close'></i>",
            status: 'danger',
            timeout: 5000,
            pos: 'top-right'
        });
    };
    this.spinerShow = function () {
        $("#loader").css("display", "block");
    };
    this.spinerHide = function () {
        $("#loader").css("display", "none");
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
});

app.factory('httpRequestInterceptor', function () {
    return {
        request: function (config) {
            config.headers['Authorization'] = 'Bearer ' + getCookie('token');
            return config;
        }
    };
});
app.config(['$httpProvider', function ($httpProvider) {
    $httpProvider.interceptors.push('httpRequestInterceptor');
}]);
