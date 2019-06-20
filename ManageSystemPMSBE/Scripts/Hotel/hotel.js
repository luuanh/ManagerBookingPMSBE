var urlGet = "/Hotel/GetListHotel"

app.controller('controller', ['$scope', '$http', 'uiGridConstants',
    function ($scope, $http, uiGridConstants) {
        $scope.filters = {
            pageNumber: 1,
            pageSize: 100,
            keySearch: '',
            status: '-1',
            startDate: '',
            endDate: ''
        }
        var columnDef = [
            {
                displayName: "Mã Nhóm", name: 'GroupCode', width: 100,
                pinnedLeft: true,
                enableFiltering: false,
                enableSorting: false
            },
            {
                displayName: "Mã KS", name: 'Code', width: 100,
                pinnedLeft: true,
                enableFiltering: false,
                enableSorting: false
            },
            {
                displayName: "Tên",
                name: 'Name',
                width: '*',
                minWidth: 200,
                pinnedLeft: true,
                enableFiltering: false,
                enableSorting: false,
                cellTemplate: '<div class="ui-grid-cell-contents box-control"><a href="/Hotel/Detail?id={{row.entity.HotelId}}" class="link-control" target="_blank">{{row.entity.Name}}</a></div>'
            },
            { displayName: "Ngày tạo", name: 'CreateDate', width: 150, cellFilter: 'date:"dd-MM-yyyy HH:mm"' },
            { displayName: "Ngày gia hạn", name: 'DayStartUse', width: 150, cellFilter: 'date:"dd-MM-yyyy HH:mm"' },
            { displayName: "TG gia hạn", name: 'TimeExtended', width: 100 },
            {
                displayName: "Hình thức TT",
                name: 'TypePaymentHotel',
                width: 120,
                cellTemplate: '<div class="ui-grid-cell-contents" ng-bind-html="grid.appScope.getTypePaymentHotel(row.entity.TypePaymentHotel)"></div>'
            },
            {
                displayName: "Trạng thái",
                name: 'Status',
                width: 120,
                cellTemplate: '<div class="ui-grid-cell-contents" ng-bind-html="grid.appScope.getStatusHotel(row.entity.Status)"></div>'
            }
        ];
        $scope.getStatusHotel = function (status) {
            return statusHotel[status - 1];
        }
        $scope.getTypePaymentHotel = function (type) {
            return typePaymentHotel[type];
        }
        $scope.gridOptions = {
            paginationPageSizes: [10, 20, 50, 100, 200, 500, 1000, 2000, 5000, 10000],
            paginationPageSize: $scope.filters.pageSize,
            useExternalPagination: true,
            exporterCsvFilename: 'file.csv',
            exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
            exporterExcelFilename: 'file.xlsx',
            exporterExcelSheetName: 'Sheet1',
            enableFiltering: false,
            useExternalFiltering: false,
            columnDefs: columnDef,
            rowHeight: 32,
            i18n: 'vi',
            showGridFooter: false,
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                $scope.gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                    $scope.filters.pageNumber = newPage;
                    $scope.filters.pageSize = pageSize;
                    $scope.GetData();
                });
            },
            exporterMenuCsv: true,
            enableGridMenu: true,
            autoResize: true
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
            $("#loader").css("display", "block")
            $scope.filters.startDate = $scope.FormatDate(fromdate)
            $scope.filters.endDate = $scope.FormatDate(todate)
            $http({
                url: urlGet,
                method: "GET",
                params: $scope.filters
            }).then(function success(response) {
                $("#loader").css("display", "none")
                var data = JSON.parse(response.data);
                $scope.gridOptions.data = data.hotels;
                $scope.gridOptions.totalItems = data.totalRecord
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.Init = function () {
            var datenowBase = new Date()
            var datenow = new Date()
            var todate = datenow.getDate() + "-" + (datenow.getMonth() + 1) + "-" + datenow.getFullYear();
            datenow.setDate(datenow.getDate() - 30);
            var fromdate = datenow.getDate() + "-" + (datenow.getMonth() + 1) + "-" + datenow.getFullYear();
            $("#FromDate").val(fromdate)
            $("#ToDate").val(todate)
            $scope.FromDate = fromdate;
            $scope.ToDate = todate;
            $scope.GetData()
        }
        $scope.Init()
    }]);