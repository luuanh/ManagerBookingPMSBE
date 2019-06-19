var urlGet = "/Admin/TypeGuest/Get"
var urlPost = "/Admin/TypeGuest/Post"
var urlPut = "/Admin/TypeGuest/Put"
var urlDetail = "/Admin/TypeGuest/Detail"
var urlDelete = "/Admin/TypeGuest/Delete"

app.controller('controller', ['$scope', '$http', '$log', 'uiGridConstants', 'validate', 'notify',
    '$timeout',
    function ($scope, $http, $log, uiGridConstants, validate, notify, $timeout) {
        $scope.filters = {
            pageNumber: 1,
            pageSize: 100,
            keySearch: ''
        }
        $scope.typeGuest = {
            TypeGuestName: '',
            MinAmount: 0,
            Discount: 0
        }
        $scope.language = []
        $scope.languageChoose = 0;
        $scope.isAdd = true

        var columnDef = [
            { displayName: "Tên loại khách", name: 'TypeGuestName', width: '*', minWidth: 150 },
            { displayName: "Số tiền tối thiều đạt mức (₫)", name: 'MinAmount', width: '*', minWidth: 150 },
            { displayName: "Giảm giá (%)", name: 'Discount', width: 150 },
            {
                displayName: "Thao tác", name: "#", width: 150, field: "#",
                cellTemplate: '<div class="ui-grid-cell-contents box-control" style="text-align:center"><a href="javascript:void(0);" class="link-control" title="Sửa" ng-click="grid.appScope.Edit(row.entity.TypeGuestId)">Sửa</a><a href="javascript:void(0);" class="link-control" title="Xóa" ng-click="grid.appScope.ConfirmDelete(row.entity.TypeGuestId)">Xóa</a></div>',
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
        }
        $scope.ConfirmDelete = function (value) {
            $scope.itemChoose = value;
            UIkit.modal("#ModalConfirm").show()
        };
        $scope.Edit = function (value) {
            $scope.OpenWindow()
            $scope.isAdd = false;
            $("#loader").css("display", "block")
            $http({
                method: "POST",
                url: urlDetail,
                params: {
                    id: value
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                $scope.typeGuest = response.data
            }, function error(respone) {
                $("#loader").css("display", "none")
            })
        }
        $scope.ShowAdd = function () {
            $scope.isAdd = true
            $scope.typeGuest = {
                TypeGuestName: '',
                MinAmount: 0,
                Discount: 0
            }
            $scope.OpenWindow()
        }
        $scope.Post = function () {
            var v1 = validate.isNullOrEmptySingleShowError({ value: $scope.typeGuest.TypeGuestName, key: 'TypeGuestName' })
            var v2 = validate.isNotNumberSingleShowError({ value: $scope.typeGuest.MinAmount, key: 'MinAmount' })
            var v3 = validate.isNotNumberSingleShowError({ value: $scope.typeGuest.Discount, key: 'Discount' })
            if (v1 || v2 || v3) {
                notify.error('Giá trị không hợp lệ!')
                return
            }
            $("#loader").css("display", "block")
            $http({
                method: "POST",
                url: urlPost,
                data: $scope.typeGuest
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
            var v1 = validate.isNullOrEmptySingleShowError({ value: $scope.typeGuest.TypeGuestName, key: 'TypeGuestName' })
            var v2 = validate.isNotNumberSingleShowError({ value: $scope.typeGuest.MinAmount, key: 'MinAmount' })
            var v3 = validate.isNotNumberSingleShowError({ value: $scope.typeGuest.Discount, key: 'Discount' })
            if (v1 || v2 || v3) {
                notify.error('Giá trị không hợp lệ!')
                return
            }
            $("#loader").css("display", "block")

            $http({
                method: "POST",
                url: urlPut,
                data: $scope.typeGuest
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
            $("#loader").css("display", "block")
            $http({
                method: "GET",
                url: urlDelete,
                params: {
                    id: $scope.itemChoose
                }
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                $scope.GetData()
                UIkit.modal("#ModalConfirm").tryhide()
                notify.success("Thành công")
            }, function error(respone) {
                $("#loader").css("display", "none")
                UIkit.modal("#ModalConfirm").tryhide()
                notify.error("Có lỗi sảy ra!")
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
                var data = response.data;
                $scope.gridOptions.data = data.typeGuests;
                $scope.gridOptions.totalItems = data.totalRecord
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.GetData()
    }]);