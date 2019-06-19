var urlGet = "/Admin/PlaneRate/Get"
var urlPost = "/Admin/PlaneRate/Post"
var urlPut = "/Admin/PlaneRate/Put"
var urlDetail = "/Admin/PlaneRate/Detail"
var urlDelete = "/Admin/PlaneRate/Delete"
var urlGetRoomType = "/Admin/RoomType/Get"

var urlGetLanguage = "/Admin/Language/GetActive"

app.controller('controller', ['$scope', '$http', '$log', 'uiGridConstants', 'validate', 'notify',
    '$timeout',
    function ($scope, $http, $log, uiGridConstants, validate, notify, $timeout) {
        $scope.filters = {
            pageNumber: 1,
            pageSize: 100,
            keySearch: ''
        }
        $scope.planerate = {
            Price: 0,
            Name: '',
            Breakfast: true,
            Lunch: true,
            Dinner: true,
            PlaneRateRoomTypes: []
        }
        $scope.isAdd = true

        var columnDef = [
            { displayName: "Tên hệ thống giá", name: 'Name', width: '*', minWidth: 150 },
            { displayName: "Giá (₫)", name: 'Price', width: 100, cellFilter: 'currency:"":"2"' },
            {
                displayName: "Kế hoạch bữa ăn",
                name: 'Note',
                width: '*',
                minwidth: 200,
                cellTemplate: '<div class="ui-grid-cell-contents" ng-bind-html="grid.appScope.planeMeal(row.entity.Breakfast,row.entity.Lunch,row.entity.Dinner)"></div>'
            },
            {
                displayName: "Thao tác", name: "#", width: 150, field: "#",
                cellTemplate: '<div class="ui-grid-cell-contents box-control" style="text-align:center"><a href="javascript:void(0);" class="link-control" title="Sửa" ng-click="grid.appScope.Edit(row.entity.PlaneRateId)">Sửa</a><a href="javascript:void(0);" class="link-control" title="Xóa" ng-click="grid.appScope.ConfirmDelete(row.entity.PlaneRateId)">Xóa</a></div>',
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
        $scope.planeMeal = function (Breakfast, Lunch, Dinner) {
            if (Breakfast && Lunch && Dinner)
                return '<span class="planeMealClass">Tất cả các bữa</span>'
            else {
                var arrMeal = [];
                if (Breakfast) {
                    arrMeal.push("Bữa sáng")
                }
                if (Lunch) {
                    arrMeal.push("Bữa trưa")
                }
                if (Dinner) {
                    arrMeal.push("Bữa tối")
                }
                var str = arrMeal.join(", ")
                return '<span class="planeMealClass">' + str + ' </span>'
            }
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
                $scope.planerate = response.data
                $scope.ModelForMultiLang = $scope.planerate.PlaneRateLanguages[0]
            }, function error(respone) {
                $("#loader").css("display", "none")
            })
        }
        $scope.ChangeLanguage = function () {
            var index = $scope.planerate.PlaneRateLanguages.findIndex(x => x.LanguageId == $scope.languageChoose);
            $scope.ModelForMultiLang = $scope.planerate.PlaneRateLanguages[index]
        }
        $scope.ShowAdd = function () {
            $scope.isAdd = true
            $scope.planerate = {
                Price: 0,
                Name: '',
                Breakfast: true,
                Lunch: true,
                Dinner: true,
                PlaneRateRoomTypes: [],
                PlaneRateLanguages: []
            }
            $scope.roomTypes.forEach(x => {
                $scope.planerate.PlaneRateRoomTypes.push({
                    RoomTypeId: x.RoomTypeId,
                    Checked: false,
                    RoomTypeName: x.RoomTypeName
                })
            });
            $scope.language.forEach(x => {
                $scope.planerate.PlaneRateLanguages.push({
                    LanguageId: x.LanguageId,
                    Name: ''
                })
            });
            $scope.ModelForMultiLang = $scope.planerate.PlaneRateLanguages[0]
            $scope.OpenWindow()
        }
        $scope.Post = function () {
            var v1 = true
            $scope.planerate.PlaneRateLanguages.forEach(x => {
                if (x.Name != null && x.Name != "") {
                    v1 = false;
                }
            })
            if (v1) {
                $("#err_Name").css("display", "block")
            }
            var v2 = validate.isNotNumberSingleShowError({ value: $scope.planerate.Price, key: 'Price' })
            if (v1 || v2) {
                notify.error('Giá trị không hợp lệ!')
                return
            }
            $("#loader").css("display", "block")

            $http({
                method: "POST",
                url: urlPost,
                data: $scope.planerate
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
            var v1 = true
            $scope.planerate.PlaneRateLanguages.forEach(x => {
                if (x.Name != null && x.Name != "") {
                    v1 = false;
                }
            })
            if (v1) {
                $("#err_Name").css("display", "block")
            }
            var v2 = validate.isNotNumberSingleShowError({ value: $scope.planerate.Price, key: 'Price' })
            if (v1 || v2) {
                notify.error('Giá trị không hợp lệ!')
                return
            }
            $("#loader").css("display", "block")

            $http({
                method: "POST",
                url: urlPut,
                data: $scope.planerate
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
                $scope.gridOptions.data = data.planeRates;
                $scope.gridOptions.totalItems = data.totalRecord
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.GetRoomType = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlGetRoomType,
                method: "GET"
            }).then(function success(response) {
                $("#loader").css("display", "none")
                $scope.roomTypes = response.data.roomTypes;
            }, function error(response) {
                $("#loader").css("display", "none")

            });
        }
        $scope.GetData()
        $scope.GetRoomType()
        $scope.GetLanguage = function () {
            $http({
                url: urlGetLanguage,
                method: "GET"
            }).then(function success(response) {
                $scope.language = response.data;
                $scope.languageChoose = $scope.language[0].LanguageId
            }, function error(response) {

            });
        }
        $scope.GetLanguage()
    }]);