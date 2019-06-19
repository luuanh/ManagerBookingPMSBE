var urlGet = "/Admin/Statistic/GetReportByUser"

app.controller('controller', ['$scope', '$http', '$log', 'uiGridConstants', 'validate', 'notify', 'helper',
    function ($scope, $http, $log, uiGridConstants, validate, notify, helper) {
        $scope.filters = {
            fromDate: '',
            toDate: ''
        }
        $scope.getStatus = function (status) {
            return statusReservation[(status - 0) - 1]
        }
        $scope.getSource = function (source) {
            return sourceReservation[(source - 0) - 1]
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
                var data = JSON.parse(response.data)
                $scope.data = data.bookings
                $scope.totalpriceAllBooking = data.totalpriceAllBooking
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.Init = function () {
            var datenow = new Date()
            datenow.setDate(datenow.getDate() - 7);
            var fromdate = datenow.getDate() + "-" + (datenow.getMonth() + 1) + "-" + datenow.getFullYear();
            var newdate = new Date();
            newdate.setDate(datenow.getDate() + 14);
            var todate = newdate.getDate() + "-" + (newdate.getMonth() + 1) + "-" + newdate.getFullYear();
            $("#FromDate").val(fromdate)
            $("#ToDate").val(todate)
            $scope.FromDate = fromdate;
            $scope.ToDate = todate;
            $scope.GetData()
        }
        $scope.Init()
    }])