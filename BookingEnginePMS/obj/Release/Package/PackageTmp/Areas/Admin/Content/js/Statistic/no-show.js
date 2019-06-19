var urlGet = "/Admin/Statistic/GetNoShow"

app.controller('controller', ['$scope', '$http', 'validate', 'notify', 'helper',
    function ($scope, $http, validate, notify, helper) {
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
        $scope.getTypeReservation = function (type) {
            return typeReservation[type]
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
            if (fromdate == null || fromdate == '' || todate == null || todate == '') {
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
                $scope.data = data.bookings
                $scope.totalAmount = data.totalAmount
                $scope.totalPaid = data.totalPaid
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.Init = function () {
            var datenow = new Date()
            datenow.setDate(datenow.getDate());
            var fromdate = datenow.getDate() + "-" + (datenow.getMonth() + 1) + "-" + datenow.getFullYear();
            var newdate = new Date();
            newdate.setDate(datenow.getDate());
            var todate = newdate.getDate() + "-" + (newdate.getMonth() + 1) + "-" + newdate.getFullYear();
            $("#FromDate").val(fromdate)
            $("#ToDate").val(todate)
            $scope.FromDate = fromdate;
            $scope.ToDate = todate;
            $scope.GetData()
        }
        $scope.Init()
    }])