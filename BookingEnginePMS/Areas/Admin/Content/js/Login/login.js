var app = angular.module('app', []);
app.service('validate', function () {
    this.isNullOrEmptySingleShowError = function (data) {
        var ok = data.value == null || data.value == "";
        if (ok)
            $("#err_" + data.key).css("display", "block");
        else
            $("#err_" + data.key).css("display", "none");
        return ok
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

var urlGetHotel = "/Admin/Login/GetHotel"
var urlPost = "/Admin/Login/Post"

app.controller('controller', ['$scope', '$http', 'validate',
    function ($scope, $http, validate) {
        $scope.error = ""
        $scope.HotelId = -1
        $scope.Code = ''
        $scope.data = {
            UserName: '',
            Password: ''
        }
        $scope.hotels = []
        $scope.validate = function (checkHotel = false) {
            var v1 = validate.isNullOrEmptySingleShowError({ value: $scope.data.UserName, key: 'UserName' })
            var v2 = validate.isNullOrEmptySingleShowError({ value: $scope.data.Password, key: 'Password' })
            var v3 = validate.isNullOrEmptySingleShowError({ value: $scope.Code, key: 'Code' })
            if (v1 || v2 || v3) {
                return false;
            }
            if (checkHotel) {
                if ($scope.HotelId < 0) {
                    alert('Chọn khách sạn để quản lý')
                    return false;
                }
            }
            return true;
        }
        $scope.getHotel = function (event) {
            if (!$scope.validate()) {
                return;
            }
            $scope.HotelId = -1
            $("#loader").css("display", "block")
            $http({
                url: urlGetHotel,
                method: 'POST',
                data: {
                    user: $scope.data,
                    code: $scope.Code
                }
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                var result = respone.data;
                if (result < 0) {
                    $scope.error = "Tài khoản hoặc mật khẩu hoặc mã khách sạn không đúng";
                    return;
                }
                $scope.error = ""
                $scope.hotels = result
            }, function error(respone) {
                $("#loader").css("display", "none")
                $scope.error = "Tài khoản hoặc mật khẩu hoặc mã khách sạn không đúng";
            })
        }
        $scope.login = function () {
            if (!$scope.validate(true)) {
                return;
            }
            $("#loader").css("display", "block")
            $http({
                url: urlPost,
                method: 'POST',
                data: {
                    user: $scope.data,
                    hotelId: $scope.HotelId,
                    languageId: $("select[name='LanguageId']").val(),
                    code: $scope.Code
                }
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                var result = respone.data;
                if (result < 0) {
                    $scope.error = "Tài khoản hoặc mật khẩu hoặc mã khách sạn không đúng";
                    return;
                }
                $scope.error = "";
             
                location.href = "/admin/home/Index"
            }, function error(respone) {
                $("#loader").css("display", "none")
                $scope.error = "Tài khoản hoặc mật khẩu hoặc mã khách sạn không đúng";
            })
        }
        $scope.RefreshValidate = function () {
            $(".error").css("display", "none")
            $scope.error = ""
        }
    }])