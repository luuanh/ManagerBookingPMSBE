var urlGetNumberFutureRoom = "/Admin/Statistic/GetNumberFutureRoom"

app.controller('controller', ['$scope', '$http', 'validate', 'notify', 'helper',
    function ($scope, $http, validate, notify, helper) {
        $scope.FormatDate = function (date) {
            date = date.trim()
            var dt = date.split('-');
            var d = dt[0];
            dt[0] = dt[2];
            dt[2] = d;
            return dt.join('-')
        }
        $scope.GetNumberFutureRoom = function () {
            var fromDate = $("#FromDate_Step1").val();
            var toDate = $("#ToDate_Step1").val();
            var v1 = validate.isNotDate({ value: fromDate, key: 'fromDate' })
            var v2 = validate.isNotDate({ value: toDate, key: 'toDate' })
            if (v1 || v2) {
                notify.error('Ngày tháng không hợp lệ!')
                return
            }
            $http({
                url: urlGetNumberFutureRoom,
                method: 'GET',
                params: {
                    fromDate: $scope.FormatDate(fromDate),
                    toDate: $scope.FormatDate(toDate)
                }
            }).then(function success(respone) {
                var data = JSON.parse(respone.data);
                $scope.dataNumberFutureRoom = data;
                console.log(data)
            }, function error(respone) {
            });
        }
        $scope.Init = function () {
            var datenow = new Date()
            datenow.setDate(datenow.getDate());
            var fromdate = datenow.getDate() + "-" + (datenow.getMonth() + 1) + "-" + datenow.getFullYear();
            var newdate = new Date();
            newdate.setDate(datenow.getDate() + 15);
            var todate = newdate.getDate() + "-" + (newdate.getMonth() + 1) + "-" + newdate.getFullYear();
            $("#FromDate_Step1").val(fromdate)
            $("#ToDate_Step1").val(todate)
            $scope.FromDate = fromdate;
            $scope.ToDate = todate;
            $scope.GetNumberFutureRoom()
        }
        $scope.Init()
    }])