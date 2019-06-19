var urlGet = "/Admin/Order/Get"
var urlDetail = "/Admin/Order/Detail"

app.controller('controller', ['$scope', '$http', '$log', 'uiGridConstants', 'validate', 'notify',
    function ($scope, $http, $log, uiGridConstants, validate, notify) {
        $scope.filters = {
            status: -1,
            pageNumber: 1,
            pageSize: 100,
            fromDate: '',
            toDate: '',
            keySearch: ''
        }
        
        var columnDef = [
            {
                displayName: "Mã",
                name: 'GuestOrderCode',
                width: 120,
                cellTemplate: '<div class="ui-grid-cell-contents box-control"><a target="_blank" href="/Admin/Order/Edit?id={{row.entity.GuestOrderId}}" class="link-control">{{row.entity.GuestOrderCode}}</a></div>'
            },
            {
                displayName: "Tên khách hàng",
                name: 'GuestName',
                width: '*',
                minWidth: 120,
                cellTemplate: '<div class="ui-grid-cell-contents box-control"><a target="_blank" href="/Admin/Order/Edit?id={{row.entity.GuestOrderId}}" class="link-control">{{row.entity.GuestName}}</a></div>'
            },
            { displayName: "Tổng tiền ($)", name: 'Total', width: 120 },
            { displayName: "Ngày tạo", name: 'CreateDate', width: 180, cellFilter: 'date:"dd-MM-yyyy HH:mm"' },
            {
                displayName: "Tình trạng",
                name: 'Paid',
                width: 120,
                cellTemplate: '<div class="ui-grid-cell-contents" ng-bind-html="grid.appScope.getStatusOrder(row.entity.Paid)"></div>'
            },
            { displayName: "Người tạo", name: 'CreateBy', width: 120 },
            { displayName: "Thu ngân", name: 'Cashier', width: 120 }
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
        }
        $scope.getStatusOrder = function (status) {
            return statusInvoice[(status - 0)]
        }
        $scope.ConfirmDelete = function (value) {
            $scope.itemChoose = value;
            UIkit.modal("#ModalConfirm").show()
        };
        
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
                $scope.gridOptions.data = data.guestOrders;
                $scope.gridOptions.totalItems = data.totalRecord
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