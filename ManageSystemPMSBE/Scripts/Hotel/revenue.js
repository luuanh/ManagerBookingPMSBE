var urlGet = "/Hotel/GetRevenue";
//var urlDetail = "/Hotel/GetListUSerBooking";
app.controller('controller', ['$scope', '$http','uiGridConstants',
    function ($scope, $http, uiGridConstants) {
        $scope.filters = {
            pageNumber: 1,
            pageSize: 100,
            status: '-1',
            keySearch: '',
            month: 1,
            year: 2018
        };
        var columnDef = [
            {
                displayName: "Tên",
                name: 'Name',
                width: '*',
                minWidth: 200,
                cellTemplate: '<div class="ui-grid-cell-contents box-control"><a href="/Hotel/Detail?id={{row.entity.HotelId}}" class="link-control" target="_blank">{{row.entity.Name}}</a></div>'
            },
            { displayName: "Booking", name: 'NumberBooking', width: 100 },
            { displayName: "Hóa đơn", name: 'NumberInvoice', width: 100 },
            { displayName: "Order (*)", name: 'NumberOrder', width: 100 },
            { displayName: "Order (**)", name: 'NumberOrderMoved', width: 100 },
            { displayName: "Dịch vụ", name: 'NumberService', width: 100 },
            { displayName: "Giường phụ", name: 'NumberExtrabed', width: 100 },
            { displayName: "Doanh số (₫)", name: 'Total', width: 180, cellFilter: 'currency: "":""' },
            {
                displayName: "Thao tác", name: "#", width: 120, field: "#",
                cellTemplate: '<div  class="ui-grid-cell-contents box-control"><a href="/Hotel/GetListUSerBooking?id={{row.entity.HotelId}}&month={{row.entity.Month}}&year={{row.entity.Year}}" class="link-control" target="_blank">Chi tiết</a></div>'
            }
        ];
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
        };
        $scope.GetData = function () {
            $("#loader").css("display", "block");
            $http({
                url: urlGet,
                method: "GET",
                params: $scope.filters
            }).then(function success(response) {
                $("#loader").css("display", "none");
                var data = response.data;
                console.log(data);
                $scope.data = $scope.filters;
                console.log($scope.data);
                $scope.gridOptions.data = data.hotels;
                $scope.gridOptions.totalItems = data.totalRecord;
            }, function error(response) {
                $("#loader").css("display", "none");
            });
        };
        //$scope.showDetail = function ( _id, month, year ) {
        //    $http({
        //        url: urlDetail,
        //        method: 'GET',
                
        //        params: $scope.filters
        //    }).then(function success(response) {
        //        $scope.data = response.data;
        //        Console.log($scope.data);
        //    }, function error(response) {
        //        $("#loader").css("display", "none");
        //    });
        //};

        $scope.Init = function () {
            var datenow = new Date();
            $scope.filters.month = (datenow.getMonth() + 1);
            $scope.filters.year = datenow.getFullYear();
            $scope.GetData();
        };
        $scope.Init();
    }]);