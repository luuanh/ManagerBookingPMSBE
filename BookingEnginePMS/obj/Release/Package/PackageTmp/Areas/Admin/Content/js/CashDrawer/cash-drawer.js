var urlGet = "/Admin/CashDrawer/Get"
var urlPost = "/Admin/CashDrawer/Post"
var urlPut = "/Admin/CashDrawer/Put"
var urlDetail = "/Admin/CashDrawer/Detail"
var urlDelete = "/Admin/CashDrawer/Delete"

app.controller('controller', ['$scope', '$http', '$log', 'uiGridConstants', 'validate', 'notify',
    '$timeout', 'helper',
    function ($scope, $http, $log, uiGridConstants, validate, notify, $timeout, helper) {
        $scope.filters = {
            pageNumber: 1,
            pageSize: 100,
            keySearch: ''
        }
        $scope.cashdrawer = {
            Name: '',
            LastBalance: 0
        }
        $scope.isAdd = true

        var columnDef = [
            { displayName: "Tên ngăn kéo", name: 'Name', width: '*', minWidth: 150 },
            { displayName: "Số dư cuối cùng (₫)", name: 'LastBalance', width: 180 },
            { displayName: "Số lần mở", name: 'TimesOpened', width: 120 },
            { displayName: "Thời gian mở gần nhất", name: 'LastOpen', width: 150, cellFilter: 'date:"dd-MM-yyyy HH:mm"' },
            { displayName: "Thời gian đóng gần nhất", name: 'LastClose', width: 150, cellFilter: 'date:"dd-MM-yyyy HH:mm"' },
            { displayName: "Lần cuối mở bởi", name: 'LastOpenedBy', width: 130 },
            {
                displayName: "Trạng thái",
                name: 'Active',
                width: 100,
                cellTemplate: '<div class="ui-grid-cell-contents box-control" style="text-align:center" ng-bind-html="grid.appScope.statusMapping(row.entity.Active)"></div>',
            },
            {
                displayName: "Thao tác", name: "#", width: 150, field: "#",
                cellTemplate: '<div class="ui-grid-cell-contents box-control" style="text-align:center"><a href="javascript:void(0);" class="link-control" title="Sửa" ng-click="grid.appScope.Edit(row.entity.CashDrawerId)">Sửa</a><a href="javascript:void(0);" class="link-control" title="Xóa" ng-click="grid.appScope.ConfirmDelete(row.entity.CashDrawerId)">Xóa</a></div>',
            }
        ];
        $scope.statusMapping = function (value) {
            return statusMapping[value - 1]
        };
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
        $scope.ConfirmDelete = function (value) {
            $scope.itemChoose = value;
            UIkit.modal("#ModalConfirm").show()
        };
        $scope.Edit = function (value) {
            $scope.OpenWindow()
            $scope.isAdd = false
            $("#loader").css("display", "block")
            $http({
                method: "POST",
                url: urlDetail,
                params: {
                    id: value
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                $scope.cashdrawer = JSON.parse(response.data)
            }, function error(respone) {
                $("#loader").css("display", "none")
            })
        }
        $scope.ShowAdd = function () {
            $scope.isAdd = true
            $scope.cashdrawer = {
                Name: '',
                LastBalance: 0
            }
            $("#kUI_window").data("kendoWindow").maximize().open();
        }
        $scope.Post = function () {
            var v1 = validate.isNullOrEmptySingleShowError({ value: $scope.cashdrawer.Name, key: 'Name' })
            var v2 = validate.isNotNumberSingleShowError({ value: $scope.cashdrawer.LastBalance, key: 'LastBalance' })
            if (v1 || v2) {
                notify.error('Value is invalid!')
                return
            }
            $("#loader").css("display", "block")
            $http({
                method: "POST",
                url: urlPost,
                data: $scope.cashdrawer
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                $scope.GetData()
                notify.success("Thành công")
                $scope.CloseWindow()
            }, function error(respone) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra!")
            })
        }
        $scope.Put = function () {
            var v1 = validate.isNullOrEmptySingleShowError({ value: $scope.cashdrawer.Name, key: 'Name' })
            if (v1) {
                notify.error('Value is invalid!')
                return
            }
            $("#loader").css("display", "block")
            $http({
                method: "POST",
                url: urlPut,
                data: $scope.cashdrawer
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                $scope.GetData()
                notify.success("Thành công")
                $scope.CloseWindow()
            }, function error(respone) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra!")
            })
        }

        $scope.OpenWindow = function () {
            $("#kUI_window").data("kendoWindow").maximize().open();
        }
        $scope.CloseWindow = function () {
            $("#kUI_window").data("kendoWindow").close();
            $scope.RefreshValidate()
        }
        $scope.RefreshValidate = function () {
            $(".error").css("display", "none")
        }
        $scope.Delete = function () {
            $http({
                method: "GET",
                url: urlDelete,
                params: {
                    id: $scope.itemChoose
                }
            }).then(function success(respone) {
                $scope.GetData()
                UIkit.modal("#ModalConfirm").tryhide()
                notify.success("Thành công")
            }, function error(respone) {
                UIkit.modal("#ModalConfirm").tryhide()
                notify.error("Có lỗi sảy ra")
            })
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
                $scope.gridOptions.data = data.cashDrawers;
                $scope.gridOptions.totalItems = data.totalRecord
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.GetData()
    }]);