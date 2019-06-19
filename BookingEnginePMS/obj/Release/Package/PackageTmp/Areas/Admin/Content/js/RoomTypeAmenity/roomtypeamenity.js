var urlGet = "/Admin/RoomTypeAmenity/Get"
var urlPut = "/Admin/RoomTypeAmenity/Put"

app.controller('controller', ['$scope', '$http', '$log', 'uiGridConstants', 'validate', 'notify',
    function ($scope, $http, $log, uiGridConstants, validate, notify) {
        $scope.data = [];
        $scope.Put = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlPut,
                method: "POST",
                data: $scope.data
            }).then(function success(response) {
                $("#loader").css("display", "none")
                notify.success("Thành công")
            }, function error(response) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra!")
            });
        }
        $scope.GetData = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlGet,
                method: "GET"
            }).then(function success(response) {
                $("#loader").css("display", "none")
                $scope.data = response.data;
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.GetData()
    }])