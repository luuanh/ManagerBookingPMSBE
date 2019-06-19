var urlGet = "/Admin/AccomPriceHour/Get"
var urlPut = "/Admin/AccomPriceHour/Put"
var urlGetRoomType = "/Admin/RoomType/Get"

app.controller('controller', ['$scope', '$http', '$log', 'uiGridConstants', 'validate', 'notify',
    '$timeout',
    function ($scope, $http, $log, uiGridConstants, validate, notify, $timeout) {
        $scope.filters = {
            RoomTypeId: -1
        }
        $scope.Put = function () {
            $("#loader").css("display", "block")
            $http({
                method: "POST",
                url: urlPut,
                data: {
                    roomTypePriceHour: $scope.data
                }
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                $scope.GetData()
                notify.success("Thành công")
            }, function error(respone) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra!")
            })
        }
        $scope.GetData = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlGet,
                method: "GET",
                params: $scope.filters
            }).then(function success(response) {
                $("#loader").css("display", "none")
                $scope.data = response.data;
                $scope.data.RoomTypeId = $scope.filters.RoomTypeId
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.GetRoomType = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlGetRoomType,
                method: "GET"
            }).then(function success(response) {
                $("#loader").css("display", "none")
                $scope.roomTypes = response.data.roomTypes;
                $scope.filters.RoomTypeId = $scope.roomTypes[0].RoomTypeId
                $scope.GetData()
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.GetRoomType()
    }]);