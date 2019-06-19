var urlGetStatusDrawer = "/Admin/CashDrawer/GetStatusDrawer"
var urlGetCashDrawer = "/Admin/CashDrawer/GetCashDrawer"
var urlOpenDrawer = "/Admin/CashDrawer/OpenDrawer"
var urlGetDrawerLastest = "/Admin/CashDrawer/GetDrawerLastest"
var urlCloseCashDrawer = "/Admin/CashDrawer/CloseCashDrawer"


app.controller('cashDrawerController', ['$scope', '$http', 'notify', 'validate',
    function ($scope, $http, notify, validate) {
        $scope.statusCashDrawer = false
        $scope.cashDrawerIdChoose = -1
        $scope.stepCloseDrawer = 1
        $scope.getStatusDrawer = function () {
            $("#loader").css("display", "block")
            $http({
                method: "POST",
                url: urlGetStatusDrawer
            }).then(function success(response) {
                $("#loader").css("display", "none")
                $scope.statusCashDrawer = response.data
            }, function error(respone) {
                $("#loader").css("display", "none")
            })
        }
        $scope.openCashDrawer = function () {
            $scope.cashDrawerIdChoose = -1
            $("#loader").css("display", "block")
            $http({
                method: "POST",
                url: urlGetCashDrawer
            }).then(function success(response) {
                $("#loader").css("display", "none")
                $scope.cashdrawers = response.data
                UIkit.modal("#ModalOpenCashDrawer").show()
                console.log($scope.cashdrawers)
            }, function error(respone) {
                $("#loader").css("display", "none")
            })
        }
        $scope.selectOtherDrawer = function () {
            if ($scope.cashDrawerIdChoose > 0) {
                $scope.CashDrawer = $scope.cashdrawers.find(x => x.CashDrawerId == $scope.cashDrawerIdChoose)
            }
        }
        $scope.warningInputCashDrawer = function () {
            if (parseFloat($scope.CashDrawer.LastBalance) != parseFloat($("#StartBalance").val())) {
                $("#err_CashDrawer").css("display", "block")
            }
            else {
                $("#err_CashDrawer").css("display", "none")
            }
        }
        $scope.refreshErrDrawer = function () {
            $("#err_CashDrawer").css("display", "none")
            $("#err_StartBalance").css("display", "none")
        }
        $scope.OpenDrawer = function () {
            if ($scope.cashDrawerIdChoose < 0) {
                alert("Chọn một ngăn kéo")
                return
            }
            var v1 = validate.isNotNumberSingleShowError({ value: $("#StartBalance").val(), key: 'StartBalance' })
            if (v1) {
                notify.error('Giá trị không hợp lệ!')
                return
            }
            $("#loader").css("display", "block")
            $http({
                method: "POST",
                url: urlOpenDrawer,
                data: {
                    cashDrawerId: $scope.cashDrawerIdChoose,
                    startBalance: $("#StartBalance").val(),
                    noteOpen: $scope.NoteOpen
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                UIkit.modal("#ModalOpenCashDrawer").tryhide()
            }, function error(respone) {
                $("#loader").css("display", "none")
            })
        }

        // close
        $scope.closeCash = {
            DrawerBalance: 0,
            NoteClose: '',
            CashDrop: 0
        }
        $scope.BackCloseDrawer = function () {
            $scope.stepCloseDrawer -= 1
        }
        $scope.StepCashDrop = function () {
            $scope.stepCloseDrawer += 1
        }
        $scope.StepSummary = function () {
            $scope.stepCloseDrawer += 1
        }
        $scope.closeCashDrawer = function () {
            $scope.stepCloseDrawer = 1
            $scope.closeCash = {
                DrawerBalance: 0,
                NoteClose: '',
                CashDrop: 0
            }
            $("#loader").css("display", "block")
            $http({
                method: "GET",
                url: urlGetDrawerLastest
            }).then(function success(response) {
                $("#loader").css("display", "none")
                $scope.drawer = JSON.parse(response.data)
                console.log($scope.drawer)
                UIkit.modal("#ModalCloseCashDrawer").show()
            }, function error(respone) {
                $("#loader").css("display", "none")
            })
        }
        $scope.CloseDrawer = function () {
            $("#loader").css("display", "block")
            $scope.closeCash.CashHistoryId = $scope.drawer.CashHistoryId
            $http({
                method: "POST",
                url: urlCloseCashDrawer,
                data: $scope.closeCash
            }).then(function success(response) {
                $("#loader").css("display", "none")
                UIkit.modal("#ModalCloseCashDrawer").tryhide()
            }, function error(respone) {
                $("#loader").css("display", "none")
            })
        }
    }]);