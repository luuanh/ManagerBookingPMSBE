var urlGet = "/Admin/ConfigPaymentMethod/GetConfigVTCPay";
var urlPut = "/Admin/ConfigPaymentMethod/PutConfigVTCPay";

app.controller('controller', ['$scope', '$http', 'validate', 'notify',
    function ($scope, $http, validate, notify) {
        $scope.data = {}
        $scope.GetData = function () {
            $("#loader").css("display", "block");
            $http({
                url: urlGet,
                method: "GET"
            }).then(function success(response) {
                $("#loader").css("display", "none");
                $scope.data = response.data;
                console.log($scope.data )
            }, function error(response) {
                $("#loader").css("display", "none");
            });
        }
        $scope.GetData()
        $scope.Put = function () {
            $("#loader").css("display", "block");
            $http({
                url: urlPut,
                method: "POST",
                data: $scope.data
            }).then(function success(response) {
                $("#loader").css("display", "none");
                notify.success("Thành công");
            }, function error(response) {
                $("#loader").css("display", "none");
                notify.error("Có lỗi sảy ra!");
            });
        }
    }])