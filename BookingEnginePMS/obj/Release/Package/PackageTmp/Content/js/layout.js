var app = angular.module('app',['ngSanitize', 'ngRoute']);
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
    this.checkVisa = function(value) {
        var cardno = /^(?:4[0-9]{12}(?:[0-9]{3})?)$/;
        return value.match(cardno)
    }
    this.checkMasterCard = function(value) {
        var cardno = /^(?:5[1-5][0-9]{14})$/;
        return value.match(cardno)
    }
    this.checkJcb = function(value) {
        var cardno = /^(?:(?:2131|1800|35\d{3})\d{11})$/;
        return value.match(cardno)
    }
    this.checkDiscover = function(value) {
        var cardno = /^(?:6(?:011|5[0-9][0-9])[0-9]{12})$/;
        return value.match(cardno)
    }
    this.checkDiners = function(value) {
        var cardno = /^(?:3(?:0[0-5]|[68][0-9])[0-9]{11})$/;
        return value.match(cardno)
    }
    this.checkAmericalExpress = function(value) {
        var cardno = /^(?:3[47][0-9]{13})$/;
        return value.match(cardno)
    }
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
    }
    this.round = function (data, number = 2) {
        return parseFloat(data).toFixed(number)
    }
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