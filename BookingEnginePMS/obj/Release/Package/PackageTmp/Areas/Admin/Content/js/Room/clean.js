var urlGet = "/Admin/Room/GetClean"
var urlCleanAll = "/Admin/Room/CleanAll"
var urlChangeStatusRoom = "/Admin/Room/ChangeStatusRoom"
var urlGetRoomType = "/Admin/RoomType/Get"

app.controller('controller', ['$scope', '$http', 'validate', 'notify',
    function ($scope, $http,validate, notify) {
        $scope.filters = {
            roomTypeId: -1,
            status: -1
        }
        $scope.GetData = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlGet,
                method: "GET",
                params: $scope.filters
            }).then(function success(response) {
                $("#loader").css("display", "none")
                $(".ellip-control > div").removeClass('uk-open')
                $(".ellip-control > div > div").removeClass('uk-dropdown-shown')
                $scope.data = response.data;
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
            }, function error(response) {
                $("#loader").css("display", "none")

            });
        }
        $scope.showCleanAll = function () {
            UIkit.modal("#ModalConfirm").show()
        }
        $scope.CleanAll = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlCleanAll,
                method: "POST"
            }).then(function success(response) {
                $("#loader").css("display", "none")
                notify.success("Thành công");
                UIkit.modal("#ModalConfirm").tryhide()
                $scope.GetData()
            }, function error(response) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra!");

            });
        }
        $scope.ChangeStatusRoom = function (roomId, status) {
            $("#loader").css("display", "block")
            $http({
                url: urlChangeStatusRoom,
                method: "POST",
                data: {
                    roomId: roomId,
                    status: status
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                notify.success("Thành công");
                UIkit.modal("#ModalConfirm").tryhide()
                $scope.GetData()
            }, function error(response) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra!");

            });
        }
        $scope.GetData()
        $scope.GetRoomType()
    }])