var urlGet = "/Admin/Promotion/GetActive"
var urlDeactivate = "/Admin/Promotion/Deactivate"

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
        $scope.ConfirmLook = function (value) {
            $scope.itemChoose = value;
            UIkit.modal("#ModalConfirm").show()
        }
        $scope.Deactivate = function () {
            $http({
                method: "GET",
                url: urlDeactivate,
                params: {
                    id: $scope.itemChoose
                }
            }).then(function success(respone) {
                $scope.GetData()
                UIkit.modal("#ModalConfirm").tryhide()
                notify.success("Thành công")
            }, function error(respone) {
                UIkit.modal("#ModalConfirm").tryhide()
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