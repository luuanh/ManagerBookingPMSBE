var urlGet = "/Admin/Company/Get"
var urlPost = "/Admin/Company/Post"
var urlPut = "/Admin/Company/Put"
var urlDetail = "/Admin/Company/Detail"

app.controller('controller', ['$scope', '$http', '$log', 'uiGridConstants', 'validate', 'notify',
    function ($scope, $http, $log, uiGridConstants, validate, notify) {
        $scope.filters = {
            pageNumber: 1,
            pageSize: 100,
            keySearch: ''
        }
        $scope.company = {
            GroupGuestId: -1,
            SourceId: -1,
            CompanyName: '',
            TaxCode: '',
            Phone: '',
            Fax: '',
            Email: '',
            Address: '',
            ContactUsName: '',
            ContactPhone: '',
            ContactEmail: ''
        }
        $scope.isAdd = true;
        $scope.itemChoose = 0;
        var columnDef = [
            {
                displayName: "Tên đối tác",
                name: 'CompanyName',
                width: '*',
                minWidth: 120,
                cellTemplate: '<div class="ui-grid-cell-contents box-control"><a href="javascript:void(0);" ng-click="grid.appScope.Edit(row.entity.CompanyId)" class="link-control">{{row.entity.CompanyName}}</a></div>'
            },
            { displayName: "Mã số thuế", name: 'TaxCode', width: 150 },
            { displayName: "Email", name: 'Email', width: '*', minWidth: 200 },
            { displayName: "Số điện thoại", name: 'Phone', width: 120 }
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
            $scope.isAdd = false
            $("#loader").css("display", "block")
            $http({
                method: "GET",
                url: urlDetail,
                params: {
                    id: value
                }
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                $scope.company = respone.data
            }, function error(respone) {
                $("#loader").css("display", "none")
            })
        }
        $scope.ShowAdd = function () {
            $scope.isAdd = true;
            $scope.company = {
                GroupGuestId: -1,
                SourceId: -1,
                CompanyName: '',
                CompanyCode: '',
                TaxCode: '',
                Phone: '',
                Fax: '',
                Email: '',
                Address: '',
                ContactUsName: '',
                ContactPhone: '',
                ContactEmail: ''
            }
            $scope.OpenWindow()

        }
        $scope.Post = function () {
            var v1 = validate.isNullOrEmptySingleShowError({ value: $scope.company.CompanyName, key: 'CompanyName' })
            if (v1) {
                notify.error('Giá trị không hợp lệ!')
                return
            }
            $("#loader").css("display", "block")
            $http({
                method: "POST",
                url: urlPost,
                data: $scope.company
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
            var v1 = validate.isNullOrEmptySingleShowError({ value: $scope.company.CompanyName, key: 'CompanyName' })
            if (v1) {
                notify.error('Giá trị không hợp lệ!')
                return
            }
            $("#loader").css("display", "block")
            $http({
                method: "POST",
                url: urlPut,
                data: $scope.company
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                $scope.GetData()
                $scope.CloseWindow()
                notify.success("Thành công")
            }, function error(respone) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra!")
            })
        }
        $scope.RefreshValidate = function () {
            $(".error").css("display", "none")
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
                $scope.gridOptions.data = data.companies;
                $scope.gridOptions.totalItems = data.totalRecord
            }, function error(response) {
                $("#loader").css("display", "none")

            });
        }

        $scope.OpenWindow = function () {
            $("#kUI_window").data("kendoWindow").maximize().open();
        }
        $scope.CloseWindow = function () {
            $("#kUI_window").data("kendoWindow").close();
            $scope.RefreshValidate()
        }
        $scope.GetData()
    }])