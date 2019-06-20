var urlGetAllHotel = "/Hotel/GetAll";
var urlGet = "/Statistic/GetBookingCancel";

app.controller('controller', ['$scope', '$http', 'uiGridConstants',
    function ($scope, $http, uiGridConstants) {
        $scope.filters = {
            hotelId: -1,
            type: -1
        }
        var columnDef = [
            {
                displayName: "Tên khách sạn",
                name: 'Name',
                width: '*',
                minWidth: 180,
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
                displayName: "Mã đặt phòng", name: 'ReservationId', width: 100,
                pinnedLeft: true
            },
            {
                displayName: "Khách đặt phòng",
                name: 'GuestName',
                width: '220'
            },
            { displayName: "Check in", name: 'CreateDate', width: 150, cellFilter: 'date:"dd-MM-yyyy HH:mm"' },
            { displayName: "Check out", name: 'DepartureDate', width: 150, cellFilter: 'date:"dd-MM-yyyy HH:mm"' },
            {
                displayName: "Tổng tiền (₫)",
                name: 'Total',
                width: 150,
                cellTemplate: '<div class="ui-grid-cell-contents" style="text-align: right;">{{row.entity.Total | currency:"":""}}</div>'
            },
            {
                displayName: "Trạng thái",
                name: 'Status',
                width: 120,
                cellTemplate: '<div class="ui-grid-cell-contents" ng-bind-html="grid.appScope.getStatusBooking(row.entity.Status)"></div>'
            }
        ];
        $scope.getStatusHotel = function (status) {
            return statusHotel[status - 1];
        }
        $scope.getTypePaymentHotel = function (type) {
            return typePaymentHotel[type];
        }
        $scope.getStatusBooking = function (status) {
            return statusReservation[status - 1]
        }
        $scope.gridOptions = {
            paginationPageSizes: [10000],
            paginationPageSize: 10000,
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
                });
            },
            exporterMenuCsv: true,
            enableGridMenu: true,
            autoResize: true
        }
        $scope.GetAllHotel = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlGetAllHotel,
                method: "GET",
                params: $scope.filters
            }).then(function success(response) {
                $("#loader").css("display", "none")
                $scope.hotels = response.data;
                $scope.filters.hotelId = -1
                $scope.GetData()
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.GetData = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlGet,
                method: "GET",
                params: $scope.filters
            }).then(function success(response) {
                $("#loader").css("display", "none")
                var data = JSON.parse(response.data);
                $scope.gridOptions.data = data;
                $scope.gridOptions.totalItems = data.length
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.GetAllHotel()
    }]);