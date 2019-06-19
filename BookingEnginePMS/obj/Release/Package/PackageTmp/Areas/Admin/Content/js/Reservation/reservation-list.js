var urlGet = "/Admin/Reservation/Get"

app.controller('controller', ['$scope', '$http', '$log', 'uiGridConstants', 'validate', 'notify', 'helper',
    function ($scope, $http, $log, uiGridConstants, validate, notify, helper) {
        $scope.filters = {
            filter: -1,
            untreated: false,
            keySearch: '',
            fromDate: '',
            toDate: '',
            source: -1,
            status: [],
            pageNumber: 1,
            pageSize: 100
        }
        $scope.showAdvancedSearch = false

        $scope.getStatus = function (status) {
            return statusReservation[(status - 0) - 1]
        }
        $scope.getSource = function (source) {
            return sourceReservation[(source - 0) - 1]
        }
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
            $scope.filters.status = $("#kUI_multiselect_basic").data("kendoMultiSelect").value()
            $("#loader").css("display", "block")
            $http({
                url: urlGet,
                method: "GET",
                params: $scope.filters
            }).then(function success(response) {
                $("#loader").css("display", "none")
                var data = response.data;
                var totalRecord = data.totalRecord
                $scope.data = data.reservations
                $scope.totalRecord = totalRecord
                $scope.totalPrice = data.totalPrice
                $scope.totalPaid = data.totalPaid
                $scope.maxPage = (totalRecord % $scope.filters.pageSize == 0) ? parseInt(totalRecord / $scope.filters.pageSize) : parseInt(totalRecord / $scope.filters.pageSize) + 1;
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
        $scope.ShowAdvanceSearch = function () {
            $scope.showAdvancedSearch = !$scope.showAdvancedSearch
        }
    }])