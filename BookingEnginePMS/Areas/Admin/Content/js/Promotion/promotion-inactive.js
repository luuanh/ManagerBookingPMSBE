var urlGet = "/Admin/Promotion/GetInActive"
var urlReactivate = "/Admin/Promotion/Reactivate"
var urlDelete = "/Admin/Promotion/Delete"

app.controller('controller', ['$scope', '$http', '$log', 'uiGridConstants', 'validate', 'notify',
    function ($scope, $http, $log, uiGridConstants, validate, notify) {
        $scope.data = [];
        $scope.GetData = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlGet,
                method: "GET"
            }).then(function success(response) {
                $("#loader").css("display", "none")
                $scope.data = JSON.parse(response.data);
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.ConfirmOpen = function (value) {
            $scope.itemChoose = value;
            UIkit.modal("#ModalConfirm").show()
        }
        $scope.ConfirmDelete = function (value) {
            $scope.itemChoose = value;
            UIkit.modal("#ModalConfirmDelete").show()
        }
        $scope.Reactivate = function () {
            $("#loader").css("display", "block")
            $http({
                method: "GET",
                url: urlReactivate,
                params: {
                    id: $scope.itemChoose
                }
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                $scope.GetData()
                UIkit.modal("#ModalConfirm").tryhide()
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
                UIkit.modal("#ModalConfirmDelete").tryhide()
                notify.success("Thành công")
            }, function error(respone) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra!")
            })
        }
        $scope.getNameTypePromotion = function (type) {
            switch (type) {
                case 1:
                    return "Basic deal"
                case 2:
                    return "Last minute"
                case 3:
                    return "Early booker"
                case 4:
                    return "Free nights"
            }
        }
        $scope.GetData()
    }])