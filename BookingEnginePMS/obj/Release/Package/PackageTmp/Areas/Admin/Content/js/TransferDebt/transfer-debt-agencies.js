var urlGet = "/Admin/Reservation/GetDebtTransferAgencies"
var urlGetReservationPayDebtByCompany = "/Admin/Reservation/GetReservationPayDebtByCompany"
var urlPayDebtBooking = "/Admin/Reservation/PayDebtBooking"

app.controller('controller', ['$scope', '$http', 'notify', 'helper', '$timeout',
    function ($scope, $http, notify, helper, $timeout) {
        $scope.filters = {
            fromDate: '',
            toDate: '',
            pageNumber: 1,
            pageSize: 100
        }
        $scope.totalPrice = 0;
        $scope.totalPaid = 0;
        $scope.companyChoose = {}

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
                var data = response.data;
                $scope.totalRecord = data.totalRecord
                $scope.data = data.companies
                console.log($scope.data)
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
        $scope.caculatorTotalPrice = function () {
            if ($scope.data == null) return;
            var total = 0;
            var paid = 0;
            $scope.data.forEach(x => {
                total = (total - 0) + (x.TotalAmount - 0)
                paid = (paid - 0) + (x.TotalPaid - 0)
            })
            $scope.totalPrice = total;
            $scope.totalPaid = paid;
        }
        $scope.showPayOnlyCompany = function (id) {
            $scope.companyChoose = $scope.data.find(x => x.CompanyId == id)
            $("#loader").css("display", "block")
            $http({
                url: urlGetReservationPayDebtByCompany,
                method: "GET",
                params: {
                    companyId: id
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                var data = JSON.parse(response.data);
                data.forEach(x => {
                    x.JsonBookingData = JSON.parse(x.JsonBookingData)
                    x.JsonBookingData.forEach(y => {
                        y.PayDebt = helper.round(y.Total - y.Paid)
                    })
                })
                $scope.reservationDebtOfCompany = data
                console.log(data)
                UIkit.modal("#ModalPayDeb").show()
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }

        $scope.PayDebtBooking = function () {
            var ok = true;
            $scope.reservationDebtOfCompany.forEach(x => {
                for (var i = 0; i < x.JsonBookingData.length; i++) {
                    var item = x.JsonBookingData[i];
                    if (item.PayDebt > item.Total - item.Paid) {
                        ok = false;
                        $("#inp_" + item.BookingId).css("border-color", "red")
                    }
                    else
                        $("#inp_" + item.BookingId).css("border-color", "#dedede")
                }
            })
            if (!ok) {
                $timeout(function () {
                    alert('Giá trị không hợp lệ!')
                }, 100)
                return;
            }
            var bookingOfReservationOfCompany = [];
            $scope.reservationDebtOfCompany.forEach(x => {
                x.JsonBookingData.forEach(y => {
                    bookingOfReservationOfCompany.push(y)
                })
            })
            $("#loader").css("display", "block")
            $http({
                url: urlPayDebtBooking,
                method: "POST",
                data: bookingOfReservationOfCompany
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
    }])