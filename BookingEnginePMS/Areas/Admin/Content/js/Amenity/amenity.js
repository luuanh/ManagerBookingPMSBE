var urlGet = "/Admin/Amenity/Get"
var urlPost = "/Admin/Amenity/Post"
var urlPut = "/Admin/Amenity/Put"
var urlDetail = "/Admin/Amenity/Detail"
var urlDelete = "/Admin/Amenity/Delete"
var urlGetLanguage = "/Admin/Language/GetActive"

app.controller('controller', ['$scope', '$http', '$log', 'uiGridConstants', 'validate', 'notify',
    '$timeout',
    function ($scope, $http, $log, uiGridConstants, validate, notify, $timeout) {
        $scope.filters = {
            pageNumber: 1,
            pageSize: 100,
            keySearch: ''
        }
        $scope.amenity = {
            Index: 1,
            Note: '',
            AmenityLanguages: []
        }
        $scope.language = []
        $scope.languageChoose = 0;
        $scope.isAdd = true

        var columnDef = [
            { displayName: "Tên tiện ích", name: 'AmenityName', width: '*', minWidth: 150 },
            { displayName: "Thứ tự", name: 'Index', width: 100 },
            { displayName: "Ghi chú", name: 'Note', width: '*', minwidth: 200 },
            {
                displayName: "Thao tác", name: "#", width: 150, field: "#",
                cellTemplate: '<div class="ui-grid-cell-contents box-control" style="text-align:center"><a href="javascript:void(0);" class="link-control" title="Sửa" ng-click="grid.appScope.Edit(row.entity.AmenityId)">Sửa</a><a href="javascript:void(0);" class="link-control" title="Xóa" ng-click="grid.appScope.ConfirmDelete(row.entity.AmenityId)">Xóa</a></div>',
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
                $scope.amenity = response.data
                if ($scope.language.length > 0) {
                    $scope.languageChoose = $scope.language[0].LanguageId
                    $scope.ModelAmenityName = $scope.amenity.AmenityLanguages[0]
                }
            }, function error(respone) {
                $("#loader").css("display", "none")
            })
        }
        $scope.ShowAdd = function () {
            $scope.isAdd = true
            $scope.amenity = {
                Index: 1,
                Note: '',
                AmenityLanguages: []
            }
            $scope.language.forEach(x => {
                $scope.amenity.AmenityLanguages.push({
                    LanguageId: x.LanguageId,
                    AmenityName: ''
                })
            });
            if ($scope.language.length > 0) {
                $scope.languageChoose = $scope.language[0].LanguageId
                $scope.ModelAmenityName = $scope.amenity.AmenityLanguages[0]
            }
            $("#kUI_window").data("kendoWindow").maximize().open();
        }
        $scope.Post = function () {
            var v1 = false;
            $scope.amenity.AmenityLanguages.forEach(x => {
                if (x.AmenityName != null && x.AmenityName != "") {
                    v1 = true;
                }
            })
            if (!v1) {
                notify.error('Giá trị không hợp lệ!')
                $("#err_AmenityName").css("display", "block")
                return
            }
            else
                $("#err_AmenityName").css("display", "none")
            $("#loader").css("display", "block")

            $http({
                method: "POST",
                url: urlPost,
                data: $scope.amenity
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
            var v1 = false;
            $scope.amenity.AmenityLanguages.forEach(x => {
                if (x.AmenityName != null && x.AmenityName != "") {
                    v1 = true;
                }
            })
            if (!v1) {
                notify.error('Giá trị không hợp lệ!')
                $("#err_AmenityName").css("display", "block")
                return
            }
            else
                $("#err_AmenityName").css("display", "none")
            $("#loader").css("display", "block")

            $http({
                method: "POST",
                url: urlPut,
                data: $scope.amenity
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
            var index = $scope.amenity.AmenityLanguages.findIndex(x => x.LanguageId == $scope.languageChoose);
            $scope.ModelAmenityName = $scope.amenity.AmenityLanguages[index]
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
                var data = response.data;
                $scope.gridOptions.data = data.amenities;
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