        var urlGet = "/Admin/Voucher/Get"
var urlPost = "/Admin/Voucher/Post"
var urlPut = "/Admin/Voucher/Put"
var urlDetail = "/Admin/Voucher/Detail"
var urlDelete = "/Admin/Voucher/Delete"
var urlGetLanguage = "/Admin/Language/GetActive"

app.controller('controller', ['$scope', '$http', '$log', 'uiGridConstants', 'validate', 'notify',
    '$timeout','helper',
    function ($scope, $http, $log, uiGridConstants, validate, notify, $timeout, helper) {
        $scope.filters = {
            pageNumber: 1,
            pageSize: 100,
            keySearch: ''
        }
        $scope.voucher = {
            VoucherCode: '',
            VoucherName: '',
            FromDate: '',
            ToDate: '',
            DiscountForService: true,
            AmountForService: 0,
            DiscountForRoom: true,
            AmountForRoom: 0,
            Number: 1,
            VoucherLanguages: []
        }
        $scope.language = []
        $scope.languageChoose = 0;
        $scope.isAdd = true

        var columnDef = [
            { displayName: "Tên", name: 'VoucherName', width: '*', minWidth: 150 },
            { displayName: "Mã", name: 'VoucherCode', width: 100 },
            { displayName: "Ngày bắt đầu", name: 'FromDate', width: 100, cellFilter: 'date:"dd-MM-yyyy"' },
            { displayName: "Ngày kết thúc", name: 'ToDate', width: 100, cellFilter: 'date:"dd-MM-yyyy"' },
            { displayName: "Giảm giá dịch vụ (%)", name: 'AmountForService', width: 100 },
            { displayName: "Giá giá phòng (%)", name: 'AmountForRoom', width: 100 },
            { displayName: "Số lượng", name: 'Number', width: 100 },
            {
                displayName: "Thao tác", name: "#", width: 150, field: "#",
                cellTemplate: '<div class="ui-grid-cell-contents box-control" style="text-align:center"><a href="javascript:void(0);" class="link-control" title="Sửa" ng-click="grid.appScope.Edit(row.entity.VoucherId)">Sửa</a><a href="javascript:void(0);" class="link-control" title="Xóa" ng-click="grid.appScope.ConfirmDelete(row.entity.VoucherId)">Xóa</a></div>',
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
            $("#loader").css("display", "block")
            $scope.isAdd = false;
            $http({
                method: "POST",
                url: urlDetail,
                params: {
                    id: value
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                $scope.voucher = JSON.parse(response.data)
                if ($scope.language.length > 0) {
                    $scope.languageChoose = $scope.language[0].LanguageId
                    $scope.ModelVoucher = $scope.voucher.VoucherLanguages[0]
                }
                $("#FromDate").val($scope.FormatDate($scope.voucher.FromDate.split('T')[0]))
                $("#ToDate").val($scope.FormatDate($scope.voucher.ToDate.split('T')[0]))
                $("#kUI_multiselect_basic").data("kendoMultiSelect").value($scope.voucher.VoucherRoomTypes); 
                $("#kUI_multiselect_basic2").data("kendoMultiSelect").value($scope.voucher.VoucherServices); 
            }, function error(respone) {
                $("#loader").css("display", "none")
            })
        }
        $scope.ShowAdd = function () {
            $scope.isAdd = true
            $("#FromDate").val('')
            $("#ToDate").val('')
            $scope.voucher = {
                VoucherCode: '',
                VoucherName: '',
                FromDate: '',
                ToDate: '',
                DiscountForService: true,
                AmountForService: 0,
                DiscountForRoom: true,
                AmountForRoom: 0,
                Number: 1,
                VoucherLanguages: []
            }
            $scope.language.forEach(x => {
                $scope.voucher.VoucherLanguages.push({
                    LanguageId: x.LanguageId,
                    Description: ''
                })
            });
            if ($scope.language.length > 0) {
                $scope.languageChoose = $scope.language[0].LanguageId
                $scope.ModelVoucher = $scope.voucher.VoucherLanguages[0]
            }
            var arrayRoomType = JSON.parse($("#roomtypeHidden").val());
            $("#kUI_multiselect_basic").data("kendoMultiSelect").value(arrayRoomType);
            var arrayService = JSON.parse($("#serviceHidden").val());
            $("#kUI_multiselect_basic2").data("kendoMultiSelect").value(arrayService);
            $scope.OpenWindow()
        }
        $scope.FormatDate = function (date) {
            date = date.trim()
            var dt = date.split('-');
            var d = dt[0];
            dt[0] = dt[2];
            dt[2] = d;
            return dt.join('-')
        }
        $scope.Post = function () {
            var fromdate = $("#FromDate").val()
            var todate = $("#ToDate").val()
            var v1 = validate.isNullOrEmptySingleShowError({ value: $scope.voucher.VoucherName, key: 'VoucherName' })
            var v2 = validate.isNullOrEmptySingleShowError({ value: $scope.voucher.VoucherCode, key: 'VoucherCode' })
            var v3 = validate.isNullOrEmptySingleShowError({ value: fromdate, key: 'FromDate' })
            var v4 = validate.isNullOrEmptySingleShowError({ value: todate, key: 'ToDate' })
            if (v1 || v2 || v3 || v4) {
                notify.error('Giá trị không hợp lệ!')
                return
            }
            $("#loader").css("display", "block")
            $scope.voucher.FromDate = $scope.FormatDate(fromdate)
            $scope.voucher.ToDate = $scope.FormatDate(todate)
            $scope.voucher.VoucherRoomTypes = $("#kUI_multiselect_basic").data("kendoMultiSelect").value();
            $scope.voucher.VoucherServices = $("#kUI_multiselect_basic2").data("kendoMultiSelect").value();
            $http({
                method: "POST",
                url: urlPost,
                data: $scope.voucher
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
            var fromdate = $("#FromDate").val()
            var todate = $("#ToDate").val()
            var v1 = validate.isNullOrEmptySingleShowError({ value: $scope.voucher.VoucherName, key: 'VoucherName' })
            var v2 = validate.isNullOrEmptySingleShowError({ value: $scope.voucher.VoucherCode, key: 'VoucherCode' })
            var v3 = validate.isNullOrEmptySingleShowError({ value: fromdate, key: 'FromDate' })
            var v4 = validate.isNullOrEmptySingleShowError({ value: todate, key: 'ToDate' })
            if (v1 || v2 || v3 || v4) {
                notify.error('Giá trị không hợp lệ!')
                return
            }
            $("#loader").css("display", "block")
            $scope.voucher.FromDate = $scope.FormatDate(fromdate)
            $scope.voucher.ToDate = $scope.FormatDate(todate)
            $scope.voucher.VoucherRoomTypes = $("#kUI_multiselect_basic").data("kendoMultiSelect").value();
            $scope.voucher.VoucherServices = $("#kUI_multiselect_basic2").data("kendoMultiSelect").value();
            $http({
                method: "POST",
                url: urlPut,
                data: $scope.voucher
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
        $scope.ChangeLanguage = function () {
            var index = $scope.voucher.VoucherLanguages.findIndex(x => x.LanguageId == $scope.languageChoose);
            $scope.ModelVoucher = $scope.voucher.VoucherLanguages[index]
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
                var data = JSON.parse(response.data);
                $scope.gridOptions.data = data.vouchers;
                $scope.gridOptions.totalItems = data.totalRecord
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.GetLanguage = function () {
            $http({
                url: urlGetLanguage,
                method: "GET"
            }).then(function success(response) {
                $scope.language = response.data;
            }, function error(response) {

            });
        }
        $scope.GetData()
        $scope.GetLanguage()
    }]);