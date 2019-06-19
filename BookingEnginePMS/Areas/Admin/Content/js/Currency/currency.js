var urlGetCurrency = "/Admin/Currency/Get"
var urlGet = "/Admin/Currency/GetData"
var urlPut = "/Admin/Currency/Put"
var urlGetValueConvertCurrentFromBank = "/Admin/Currency/GetValueConvertCurrentFromBank"

app.controller('controller', ['$scope', '$http', 'validate', 'notify',
    function ($scope, $http, validate, notify) {
        $scope.convertStarted = false;
        $scope.filter = {
            typeCurrency: ''
        }

        $scope.GetData = function () {
            $scope.convertStarted = true
            $("#loader").css("display", "block")
            $http({
                url: urlGet,
                method: "GET",
                params: $scope.filter
            }).then(function success(response) {
                $("#loader").css("display", "none")
                $scope.data = response.data;
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.Put = function () {
            $scope.convertStarted = true
            $("#loader").css("display", "block")
            $http({
                url: urlPut,
                method: "POST",
                data: {
                    currency: $scope.data
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                notify.success("Thành công");
            }, function error(response) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra")
            });
        }
        $scope.GetCurrency = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlGetCurrency,
                method: "GET"
            }).then(function success(response) {
                $("#loader").css("display", "none")
                $scope.currencies = response.data;
                $scope.filter.typeCurrency = $scope.currencies[0].CurrencyCode
                $scope.GetData()
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.GetValueConvertCurrentFromBank = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlGetValueConvertCurrentFromBank,
                method: "GET",
                params: {
                    typeCurrency: $scope.filter.typeCurrency
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                $scope.data.Result = response.data
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.GetCurrency()
    }])