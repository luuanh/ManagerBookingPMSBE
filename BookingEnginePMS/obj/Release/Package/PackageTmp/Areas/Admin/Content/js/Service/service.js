var urlGet = "/Admin/Service/Get"
var urlPost = "/Admin/Service/Post"
var urlPut = "/Admin/Service/Put"
var urlDetail = "/Admin/Service/Detail"
var urlDelete = "/Admin/Service/Delete"
var urlGetLanguage = "/Admin/Language/GetActive"
var urlGetServiceCategory = "/Admin/ServiceCategory/GetFull"

app.controller('controller', ['$scope', '$http', '$log', 'uiGridConstants', 'validate', 'notify',
    function ($scope, $http, $log, uiGridConstants, validate, notify) {
        $scope.filters = {
            pageNumber: 1,
            pageSize: 100,
            keySearch: '',
            serviceCategoryId: -1
        }
        $scope.service = {
            Photo: '/FileDefault/image/img-default.gif',
            ServiceCode: '',
            Index: 1,
            NonPrice: false,
            Price: 0,
            ServiceLanguages: []
        }
        $scope.ServiceCategories = []
        $scope.isAdd = true;
        $scope.itemChoose = 0;
        var columnDef = [
            { displayName: "Tên dịch vụ", name: 'ServiceName', width: '*', minWidth: 150 },
            { displayName: "Mã", name: 'ServiceCode', width: 100 },
            {
                displayName: "Hình ảnh",
                name: "Photo",
                width: 100,
                cellTemplate: '<div class="ui-grid-cell-contents box-image" style="text-align:center"><img ng-src="{{grid.appScope.ShowImage(row.entity.Photo)}}" /></div>',
            },
            { displayName: "Giá (₫)", name: 'Price', width: 100, cellFilter:'currency:"":""' },
            { displayName: "Thứ tự", name: 'Index', width: 100 },
            {
                displayName: "Thao tác", name: "#", width: 150, field: "#",
                cellTemplate: '<div class="ui-grid-cell-contents box-control" style="text-align:center"><a href="javascript:void(0);" class="link-control" title="Sửa" ng-click="grid.appScope.Edit(row.entity.ServiceId)">Sửa</a><a href="javascript:void(0);" class="link-control" title="Xóa" ng-click="grid.appScope.ConfirmDelete(row.entity.ServiceId)">Xóa</a></div>',
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
        $scope.ShowImage = function (src) {
            return src
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
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                $scope.service = respone.data
                if ($scope.language.length > 0) {
                    $scope.languageChoose = $scope.language[0].LanguageId
                    $scope.ModelService = $scope.service.ServiceLanguages[0]
                }
            }, function error(respone) {
                $("#loader").css("display", "none")
            })
        }
        $scope.ShowAdd = function () {
            $scope.isAdd = true;
            $scope.service = {
                Photo: '/FileDefault/image/img-default.gif',
                ServiceCode: '',
                Index: 1,
                NonPrice: false,
                Price: 0,
                ServiceCategoryId: $scope.ServiceCategories[0].ServiceCategoryId,
                ServiceLanguages: []
            }
            $scope.language.forEach(x => {
                $scope.service.ServiceLanguages.push({
                    LanguageId: x.LanguageId,
                    ServiceName: '',
                    Description: '',
                    Policy: ''
                })
            });
            if ($scope.language.length > 0) {
                $scope.languageChoose = $scope.language[0].LanguageId
                $scope.ModelService = $scope.service.ServiceLanguages[0]
            }
            $scope.OpenWindow()

        }
        $scope.ChangeLanguage = function () {
            var index = $scope.service.ServiceLanguages.findIndex(x => x.LanguageId == $scope.languageChoose);
            $scope.ModelService = $scope.service.ServiceLanguages[index]
        }
        $scope.Post = function () {
            if ($scope.service.NonPrice)
                $scope.service.Price = 0;
            var v1 = true;
            $scope.service.ServiceLanguages.forEach(x => {
                if (x.ServiceName != null && x.ServiceName != "") {
                    v1 = false;
                }
            })
            if (v1) {
                $("#err_ServiceName").css("display", "block")
            }
            else
                $("#err_ServiceName").css("display", "none")

            var v2 = validate.isNullOrEmptySingleShowError({ value: $scope.service.ServiceCode, key: 'ServiceCode' })
            var v3 = validate.isNotNumberSingleShowError({ value: $scope.service.Index, key: 'Index' })
            var v4 = validate.isNotNumberSingleShowError({ value: $scope.service.Price, key: 'Price' })
            if (v1 || v2 || v3 || v4) {
                notify.error('Giá trị không hợp lệ!')
                return
            }
            $("#loader").css("display", "block")
            $scope.service.Photo = $("#Logo").val()
            $http({
                method: "POST",
                url: urlPost,
                data: $scope.service
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
            if ($scope.service.NonPrice)
                $scope.service.Price = 0;
            var v1 = true;
            $scope.service.ServiceLanguages.forEach(x => {
                if (x.ServiceName != null && x.ServiceName != "") {
                    v1 = false;
                }
            })
            if (v1) {
                $("#err_ServiceName").css("display", "block")
            }
            else
                $("#err_ServiceName").css("display", "none")

            var v2 = validate.isNullOrEmptySingleShowError({ value: $scope.service.ServiceCode, key: 'ServiceCode' })
            var v3 = validate.isNotNumberSingleShowError({ value: $scope.service.Index, key: 'Index' })
            var v4 = validate.isNotNumberSingleShowError({ value: $scope.service.Price, key: 'Price' })
            if (v1 || v2 || v3 || v4) {
                notify.error('Giá trị không hợp lệ!')
                return
            }
            $("#loader").css("display", "block")
            $scope.service.Photo = $("#Logo").val()
            $http({
                method: "POST",
                url: urlPut,
                data: $scope.service
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
        $scope.GetData = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlGet,
                method: "GET",
                params: $scope.filters
            }).then(function success(response) {
                $("#loader").css("display", "none")
                var data = response.data;
                $scope.gridOptions.data = data.services;
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
        $scope.GetServiceCategory = function () {
            $http({
                url: urlGetServiceCategory,
                method: "GET"
            }).then(function success(response) {
                $scope.ServiceCategories = response.data
            }, function error(response) {
            });
        }
        $scope.GetServiceCategory()
    }])