var urlGet = "/Admin/Room/Get"
var urlPost = "/Admin/Room/Post"
var urlPut = "/Admin/Room/Put"
var urlDetail = "/Admin/Room/Detail"
var urlDelete = "/Admin/Room/Delete"
var urlGetRoomType = "/Admin/RoomType/Get"
app.controller('controller', ['$scope', '$http', '$log', 'uiGridConstants', 'validate', 'notify',
    function ($scope, $http, $log, uiGridConstants, validate, notify) {
        $scope.filters = {
            RoomTypeId: -1
        }
        $scope.data = []
        $scope.room = {
            RoomCode: '',
            Floor: 1,
            Status: 1,
            RoomTypeId: 1
        }
        $scope.roomTypes = []
        $scope.isAdd = true;
        $scope.itemChoose = 0;
        $scope.ConfirmDelete = function () {
            UIkit.modal("#ModalConfirm").show()
        };
        $scope.Edit = function (value) {
            $scope.OpenWindow()
            $scope.itemChoose = value;
            $scope.isAdd = false
            $("#loader").css("display", "block")
            $http({
                method: "GET",
                url: urlDetail,
                params: {
                    id: value
                }
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                $scope.room = respone.data
            }, function error(respone) {
                $("#loader").css("display", "none")
            })
        }
        $scope.ShowAdd = function () {
            $scope.isAdd = true;
            $scope.room = {
                RoomCode: '',
                Floor: 1,
                Status: 1,
                RoomTypeId: $scope.roomTypes[0].RoomTypeId
            }
            $scope.OpenWindow()

        }
        $scope.Post = function () {
            var v1 = validate.isNullOrEmptySingleShowError({ value: $scope.room.RoomCode, key: 'RoomCode' })
            var v2 = validate.isNotNumberSingleShowError({ value: $scope.room.Floor, key: 'Floor' })
            if (v1 || v2) {
                notify.error('Giá trị không hợp lệ!')
                return
            }
            $("#loader").css("display", "block")
            $http({
                method: "POST",
                url: urlPost,
                data: $scope.room
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                $scope.GetData()
                notify.success("Thành công")
                $scope.CloseWindow()
            }, function error(respone) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra!")
            })
        }
        $scope.Put = function () {
            var v1 = validate.isNullOrEmptySingleShowError({ value: $scope.room.RoomCode, key: 'RoomCode' })
            var v2 = validate.isNotNumberSingleShowError({ value: $scope.room.Floor, key: 'Floor' })
            if (v1 || v2) {
                notify.error('Giá trị không hợp lệ!')
                return
            }
            $("#loader").css("display", "block")
            $http({
                method: "POST",
                url: urlPut,
                data: $scope.room
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                $scope.GetData()
                $scope.CloseWindow()
                notify.success("Thành công")
            }, function error(respone) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra!")
            })
        }
        $scope.Delete = function () {
            $("#loader").css("display", "block")
            $http({
                method: "GET",
                url: urlDelete,
                params: {
                    id: $scope.itemChoose
                }
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                $scope.GetData()
                UIkit.modal("#ModalConfirm").tryhide()
                $scope.CloseWindow()
                notify.success("Thành công")
            }, function error(respone) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra!")
            })
        }
        $scope.RefreshValidate = function () {
            $(".error").css("display", "none")
        }
        $scope.OpenWindow = function () {
            $("#kUI_window").data("kendoWindow").maximize().open();
        }
        $scope.CloseWindow = function () {
            $("#kUI_window").data("kendoWindow").close();
            $scope.RefreshValidate()
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
        $scope.GetData()
        $scope.GetRoomType()
    }])