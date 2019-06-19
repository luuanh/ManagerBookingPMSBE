var urlGet = "/Admin/ServiceCategory/Get"
var urlPost = "/Admin/ServiceCategory/Post"
var urlPut = "/Admin/ServiceCategory/Put"
var urlDetail = "/Admin/ServiceCategory/Detail"
var urlDelete = "/Admin/ServiceCategory/Delete"
var urlGetLanguage = "/Admin/Language/GetActive"

app.controller('controller', ['$scope', '$http', '$log', 'uiGridConstants', 'validate', 'notify',
    function ($scope, $http, $log, uiGridConstants, validate, notify) {
        $scope.filters = {
            pageNumber: 1,
            pageSize: 100,
            keySearch: ''
        }
        $scope.servicecategory = {
            Image: '/FileDefault/image/img-default.gif',
            Code: '',
            ServiceCategoryLanguages: []
        }
        $scope.isAdd = true;
        $scope.itemChoose = 0;
        var columnDef = [
            { displayName: "Tên loại dịch vụ", name: 'ServiceCategoryName', width: '*', minWidth: 150 },
            { displayName: "Mã", name: 'Code', width: 100 },
            {
                displayName: "Hình ảnh",
                name: "Image",
                width: 100,
                cellTemplate: '<div class="ui-grid-cell-contents box-image" style="text-align:center"><img ng-src="{{grid.appScope.ShowImage(row.entity.Image)}}" /></div>',
            },
            { displayName: "Thứ tự", name: 'Index', width: 100 },
            {
                displayName: "Thao tác", name: "#", width: 150, field: "#",
                cellTemplate: '<div class="ui-grid-cell-contents box-control" style="text-align:center"><a href="javascript:void(0);" class="link-control" title="Sửa" ng-click="grid.appScope.Edit(row.entity.ServiceCategoryId)">Sửa</a><a href="javascript:void(0);" class="link-control" title="Xóa" ng-click="grid.appScope.ConfirmDelete(row.entity.ServiceCategoryId)">Xóa</a></div>',
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
                $scope.servicecategory = respone.data
                if ($scope.language.length > 0) {
                    $scope.languageChoose = $scope.language[0].LanguageId
                    $scope.ModelServiceCategory = $scope.servicecategory.ServiceCategoryLanguages[0]
                }
            }, function error(respone) {
                $("#loader").css("display", "none")
            })
        }
        $scope.ShowAdd = function () {
            $scope.isAdd = true;
            $scope.servicecategory = {
                Image: '/FileDefault/image/img-default.gif',
                Code: '',
                ServiceCategoryLanguages: []
            }
            $scope.language.forEach(x => {
                $scope.servicecategory.ServiceCategoryLanguages.push({
                    LanguageId: x.LanguageId,
                    ServiceCategoryName: ''
                })
            });
            if ($scope.language.length > 0) {
                $scope.languageChoose = $scope.language[0].LanguageId
                $scope.ModelServiceCategory = $scope.servicecategory.ServiceCategoryLanguages[0]
            }
            $scope.OpenWindow()

        }
        $scope.ChangeLanguage = function () {
            var index = $scope.servicecategory.ServiceCategoryLanguages.findIndex(x => x.LanguageId == $scope.languageChoose);
            $scope.ModelServiceCategory = $scope.servicecategory.ServiceCategoryLanguages[index]
        }
        $scope.Post = function () {
            var v1 = true;
            $scope.servicecategory.ServiceCategoryLanguages.forEach(x => {
                if (x.ServiceCategoryName != null && x.ServiceCategoryName != "") {
                    v1 = false;
                }
            })
            if (v1) {
                $("#err_ServiceCategoryName").css("display", "block")
            }
            else
                $("#err_ServiceCategoryName").css("display", "none")
            var v2 = validate.isNullOrEmptySingleShowError({ value: $scope.servicecategory.Code, key: 'Code' })
            if (v1 || v2) {
                notify.error('Value is invalid!')
                return
            }
            $("#loader").css("display", "block")
            $scope.servicecategory.Image = $("#Logo").val()
            $http({
                method: "POST",
                url: urlPost,
                data: $scope.servicecategory
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
            var v1 = true;
            $scope.servicecategory.ServiceCategoryLanguages.forEach(x => {
                if (x.ServiceCategoryName != null && x.ServiceCategoryName != "") {
                    v1 = false;
                }
            })
            if (v1) {
                $("#err_ServiceCategoryName").css("display", "block")
            }
            else
                $("#err_ServiceCategoryName").css("display", "none")
            var v2 = validate.isNullOrEmptySingleShowError({ value: $scope.servicecategory.Code, key: 'Code' })
            if (v1 || v2) {
                notify.error('Giá trị không hợp lệ!')
                return
            }
            $("#loader").css("display", "block")
            $scope.servicecategory.Image = $("#Logo").val()
            $http({
                method: "POST",
                url: urlPut,
                data: $scope.servicecategory
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
                $scope.gridOptions.data = data.serviceCategories;
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
    }])