var urlGet = "/Admin/Reservation/GetDebtTransferReservation"
var urlGetBookingPayDebt = "/Admin/Reservation/GetBookingPayDebt"
var urlPayDebtBooking = "/Admin/Reservation/PayDebtBooking"
var urlPayAllReservationChoose = "/Admin/Reservation/PayAllReservationChoose"

app.controller('controller', ['$scope', '$http', '$log', 'uiGridConstants', 'validate', 'notify', 'helper', '$timeout',
    function ($scope, $http, $log, uiGridConstants, validate, notify, helper, $timeout) {
        $scope.filters = {
            fromDate: '',
            toDate: '',
            pageNumber: 1,
            pageSize: 100
        }
        $scope.checkAllReservationPayDebt = false
        $scope.hasItemChoose = false
        $scope.totalPriceChosen = 0;
        $scope.totalPaidChosen = 0;
        
        $scope.changePage = function (count) {
            if ($scope.filters.pageNumber + count < 1 || $scope.filters.pageNumber + count > $scope.maxPage)
                return;
            $scope.filters.pageNumber += count;
            $scope.GetData()
        }
        $scope.fixPage = function (page) {
            if (page < 1 || page > $scope.maxPage)
                return;
            $scope.filters.pageNumber = page;
            $scope.GetData()
        }
        $scope.ChangePageSize = function () {
            $scope.filters.pageNumber = 1;
            $scope.GetData()
        }
        $scope.FormatDate = function (date) {
            date = date.trim()
            var dt = date.split('-');
            var d = dt[0];
            dt[0] = dt[2];
            dt[2] = d;
            return dt.join('-')
        }
        $scope.GetData = function () {
            var fromdate = $("#FromDate").val();
            var todate = $("#ToDate").val();
            if (fromdate == null || fromdate == '' || ((todate == null || todate == '') && $scope.filters.typeSearch == -1)) {
                alert('Ngày tháng không hợp lệ');
                return;
            }
            $scope.filters.fromDate = $scope.FormatDate(fromdate)
            $scope.filters.toDate = $scope.FormatDate(todate)
            $("#loader").css("display", "block")
            $http({
                url: urlGet,
                method: "GET",
                params: $scope.filters
            }).then(function success(response) {
                $("#loader").css("display", "none")
                var data = JSON.parse(response.data);
                var totalRecord = data.totalRecord
                $scope.data = data.reservations
                $scope.totalRecord = totalRecord
                $scope.totalPrice = data.totalPrice
                $scope.totalPaid = data.totalPaid
                $scope.maxPage = (totalRecord % $scope.filters.pageSize == 0) ? parseInt(totalRecord / $scope.filters.pageSize) : parseInt(totalRecord / $scope.filters.pageSize) + 1;
                $scope.data.forEach(x => x.Choose = false)
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.Init = function () {
            var datenow = new Date()
            var fromdate = 1 + "-" + (datenow.getMonth() + 1) + "-" + datenow.getFullYear();
            var newdate = new Date();
            newdate.setDate(datenow.getDate() + 30);
            var todate = 1 + "-" + (newdate.getMonth() + 1) + "-" + newdate.getFullYear();
            $("#FromDate").val(fromdate)
            $("#ToDate").val(todate)
            $scope.FromDate = fromdate;
            $scope.ToDate = todate;
            $scope.GetData()
        }
        $scope.Init()
        $scope.currentPage = function () {
            var page = ($scope.filters.pageNumber - 1) * $scope.filters.pageSize + 1
            return page > 0 ? page : 0
        }
        $scope.numberItemInPage = function () {
            var lengthdata = 0;
            if ($scope.data != null)
                lengthdata = $scope.data.length
            var number = ($scope.filters.pageNumber - 1) * $scope.filters.pageSize + lengthdata
            return number > 0 ? number : 0
        }

        // action
        $scope.reservationChooseToPayDebt = []
        $scope.payReservation = function (id) {
            $scope.reservationChoose = $scope.data.find(x => x.ReservationId == id)
            $("#loader").css("display", "block")
            $http({
                url: urlGetBookingPayDebt,
                method: "GET",
                params: {
                    reservationId: id
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                var data = JSON.parse(response.data);
                $scope.debtBookings = data
                $scope.debtBookings.forEach(x => {
                    x.PayDebt = helper.round(x.Total - x.Paid)
                })
                UIkit.modal("#ModalPayDeb").show()
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.PayDebtBooking = function () {
            var ok = true;
            for (var i = 0; i < $scope.debtBookings.length; i++) {
                var item = $scope.debtBookings[i];
                if (item.PayDebt > item.Total - item.Paid) {
                    ok = false;
                    $("#inp_" + i).css("border-color", "red")
                }
                else
                    $("#inp_" + i).css("border-color", "#dedede")
            }
            if (!ok) {
                $timeout(function () {
                    alert('Ngày tháng không hợp lệ!')
                }, 100)
                return;
            }
            $("#loader").css("display", "block")
            $http({
                url: urlPayDebtBooking,
                method: "POST",
                data: $scope.debtBookings
            }).then(function success(response) {
                $("#loader").css("display", "none")
                notify.success("Thành công")
                UIkit.modal("#ModalPayDeb").tryhide()
                $scope.GetData()
            }, function error(response) {
                notify.error("Có lỗi sảy ra!")
                $("#loader").css("display", "none")
            });
        }
        $scope.refreshErroInp = function () {
            $(".inpPaydebt").css("border-color", "#dedede")
        }

        $scope.changeCheckAll = function () {
            if ($scope.checkAllReservationPayDebt) {
                $scope.data.forEach(x => x.Choose = true)
            }
            else {
                $scope.data.forEach(x => x.Choose = false)
            }
        }
        $scope.checkHasItemChoose = function () {
            if ($scope.data == null)
                return
            var ok = true;
            for (var i = 0; i < $scope.data.length; i++) {
                if ($scope.data[i].Choose) {
                    $scope.hasItemChoose = true;
                    return;
                }
            }
            $scope.hasItemChoose = false;
            $scope.checkAllReservationPayDebt = false;
        }
        $scope.confirmPayAllReservationChoose = function () {
            $scope.reservationChooseToPayDebt = $scope.data.filter(x => x.Choose);
            UIkit.modal("#ModalPayAllReservationChoose").show()
        }
        $scope.PayAllReservationChoose = function () {
            $("#loader").css("display", "block")
            var arrReservationId = []
            $scope.reservationChooseToPayDebt.forEach(x => {
                arrReservationId.push(x.ReservationId)
            })
            $http({
                url: urlPayAllReservationChoose,
                method: "POST",
                data: arrReservationId
            }).then(function success(response) {
                $("#loader").css("display", "none")
                notify.success("Thành công")
                UIkit.modal("#ModalPayAllReservationChoose").tryhide()
                $scope.GetData()
            }, function error(response) {
                notify.error("Có lỗi sảy ra!")
                $("#loader").css("display", "none")
            });
        }
        $scope.calTotalPriceOfReservationChooseToPay = function () {
            var total = 0;
            var paid = 0;
            $scope.reservationChooseToPayDebt.forEach(x => {
                total = (total - 0) + (x.TotalAmount - 0);
                paid = (paid - 0) + (x.Paid - 0);
            })
            $scope.totalPriceChosen = total;
            $scope.totalPaidChosen = paid;
        }
    }])