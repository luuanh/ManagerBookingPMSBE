var urlGet = "/Admin/Extrabed/Get"
var urlPost = "/Admin/Extrabed/Post"
var urlPut = "/Admin/Extrabed/Put"
var urlDetail = "/Admin/Extrabed/Detail"
var urlDelete = "/Admin/Extrabed/Delete"
var urlGetLanguage = "/Admin/Language/GetActive"

app.controller('controller', ['$scope', '$http', '$log', 'uiGridConstants', 'validate', 'notify',
    function ($scope, $http, $log, uiGridConstants, validate, notify) {
        $scope.filters = {
            pageNumber: 1,
            pageSize: 100,
            keySearch: ''
        }
        $scope.extrabed = {
            Image: '/FileDefault/image/img-default.gif',
            Price: 0,
            ExtrabedLanguages: []
        }
        $scope.isAdd = true;
        $scope.itemChoose = 0;
        var columnDef = [
            { displayName: "Tên giường phụ", name: 'ExtrabedName', width: '*', minWidth: 150 },
            { displayName: "Giá (₫)", name: 'Price', width: 100 },
            {
                displayName: "Hình ảnh",
                name: "Image",
                width: 100,
                cellTemplate: '<div class="ui-grid-cell-contents box-image" style="text-align:center"><img ng-src="{{grid.appScope.ShowImage(row.entity.Image)}}" /></div>',
            },
            { displayName: "Mô tả", name: 'Description', width: '*', minWidth: 250 },
            {
                displayName: "Thao tác", name: "#", width: 150, field: "#",
                cellTemplate: '<div class="ui-grid-cell-contents box-control" style="text-align:center"><a href="javascript:void(0);" class="link-control" title="Sửa" ng-click="grid.appScope.Edit(row.entity.ExtrabedId)">Sửa</a><a href="javascript:void(0);" class="link-control" title="Xóa" ng-click="grid.appScope.ConfirmDelete(row.entity.ExtrabedId)">Xóa</a></div>',
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
                $scope.extrabed = respone.data
                if ($scope.language.length > 0) {
                    $scope.languageChoose = $scope.language[0].LanguageId
                    $scope.ModelExtrabed = $scope.extrabed.ExtrabedLanguages[0]
                }
            }, function error(respone) {
                $("#loader").css("display", "none")
            })
        }
        $scope.ShowAdd = function () {
            $scope.isAdd = true;
            $scope.extrabed = {
                Image: '/FileDefault/image/img-default.gif',
                Price: 0,
                ExtrabedLanguages: []
            }
            $scope.language.forEach(x => {
                $scope.extrabed.ExtrabedLanguages.push({
                    LanguageId: x.LanguageId,
                    ExtrabedName: '',
                    Description: ''
                })
            });
            if ($scope.language.length > 0) {
                $scope.languageChoose = $scope.language[0].LanguageId
                $scope.ModelExtrabed = $scope.extrabed.ExtrabedLanguages[0]
            }
            $scope.OpenWindow()

        }
        $scope.ChangeLanguage = function () {
            var index = $scope.extrabed.ExtrabedLanguages.findIndex(x => x.LanguageId == $scope.languageChoose);
            $scope.ModelExtrabed = $scope.extrabed.ExtrabedLanguages[index]
        }
        $scope.Post = function () {
            var v1 = true;
            $scope.extrabed.ExtrabedLanguages.forEach(x => {
                if (x.ExtrabedName != null && x.ExtrabedName != "") {
                    v1 = false;
                }
            })
            if (v1) {
                $("#err_ExtrabedName").css("display", "block")
            }
            else
                $("#err_ExtrabedName").css("display", "none")
            var v2 = validate.isNotNumberSingleShowError({ value: $scope.extrabed.Price, key: 'Price' })
            if (v1 || v2) {
                notify.error('The field is empty!')
                return
            }
            $("#loader").css("display", "block")
            $scope.extrabed.Image = $("#Logo").val()
            $http({
                method: "POST",
                url: urlPost,
                data: $scope.extrabed
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
            $scope.extrabed.ExtrabedLanguages.forEach(x => {
                if (x.ExtrabedName != null && x.ExtrabedName != "") {
                    v1 = false;
                }
            })
            if (v1) {
                $("#err_ExtrabedName").css("display", "block")
            }
            else
                $("#err_ExtrabedName").css("display", "none")
            var v2 = validate.isNotNumberSingleShowError({ value: $scope.extrabed.Price, key: 'Price' })
            if (v1 || v2) {
                notify.error('The field is empty!')
                return
            }

            $("#loader").css("display", "block")
            $scope.extrabed.Image = $("#Logo").val()
            $http({
                method: "POST",
                url: urlPut,
                data: $scope.extrabed
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
                $scope.gridOptions.data = data.extrabeds;
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