var urlGet = "/Admin/Statistic/GetSummaryReport"

app.controller('controller', ['$scope', '$http', 'validate', 'notify', 'helper',
    function ($scope, $http, validate, notify, helper) {
        $scope.filters = {
            fromDate: ''
        }
        $scope.data = {}
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
            if (fromdate == null || fromdate == '') {
                alert('Ngày tháng không hợp lệ');
                return;
            }
            $scope.filters.fromDate = $scope.FormatDate(fromdate)
            $("#loader").css("display", "block")
            $http({
                url: urlGet,
                method: "GET",
                params: $scope.filters
            }).then(function success(response) {
                $("#loader").css("display", "none")
                $scope.data = JSON.parse(response.data)
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.Init = function () {
            var datenow = new Date()
            datenow.setDate(datenow.getDate());
            var fromdate = datenow.getDate() + "-" + (datenow.getMonth() + 1) + "-" + datenow.getFullYear();
            $("#FromDate").val(fromdate)
            $scope.FromDate = fromdate;
            $scope.GetData()
        }
        $scope.Init()
    }])