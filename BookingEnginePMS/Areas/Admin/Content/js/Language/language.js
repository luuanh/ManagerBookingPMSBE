var urlGet = "/Admin/Language/Get"
var urlPost = "/Admin/Language/Post"
var urlPut = "/Admin/Language/Put"
var urlDetail = "/Admin/Language/Detail"
var urlDelete = "/Admin/Language/Delete"

app.controller('controller', ['$scope', '$http', '$log', 'uiGridConstants', 'validate', 'notify',
    function ($scope, $http, $log, uiGridConstants, validate, notify) {
        $scope.filters = {
            pageNumber: 1,
            pageSize: 100,
            keySearch: ''
        }
        $scope.language = {
            Ensign: '/FileDefault/image/img-default.gif',
            Title: '',
            Key: '',
            Index: 1,
            Active: true
        }
        $scope.isAdd = true;
        $scope.itemChoose = 0;
        var columnDef = [
            { displayName: "Ngôn ngữ", name: 'Title', width: '*', minWidth: 150 },
            { displayName: "Khóa", name: 'Key', width: 100 },
            {
                displayName: "Quốc kỳ",
                name: "Ensign",
                width: 100,
                cellTemplate: '<div class="ui-grid-cell-contents box-image" style="text-align:center"><img ng-src="{{grid.appScope.ShowImage(row.entity.Ensign)}}" /></div>',
            },
            { displayName: "Thứ tự", name: 'Index', width: 100 },
            {
                displayName: "Trạng thái",
                name: "Active",
                width: 100,
                cellTemplate: '<div class="ui-grid-cell-contents box-control" style="text-align:center" ng-bind-html="grid.appScope.statusMapping(row.entity.Active)"></div>',
            },
            {
                displayName: "Thao tác", name: "#", width: 150, field: "#",
                cellTemplate: '<div class="ui-grid-cell-contents box-control" style="text-align:center"><a href="javascript:void(0);" class="link-control" title="Sửa" ng-click="grid.appScope.Edit(row.entity.LanguageId)">Sửa</a><a href="javascript:void(0);" class="link-control" title="Xóa" ng-click="grid.appScope.ConfirmDelete(row.entity.LanguageId)">Xóa</a></div>',
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
        $scope.statusMapping = function (value) {
            if (value)
                return statusMapping[0];
            else
                return statusMapping[1];
        };
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
                $scope.language = respone.data
                if ($scope.language.Active) {
                    $("#switch_demo_1").attr("checked", "true")
                    $(".box-wrapper span").attr("style", "background-color: rgba(0, 150, 136, 0.5);border-color: rgba(0, 150, 136, 0.5);box-shadow: rgba(0, 150, 136, 0.5) 0px 0px 0px 6.5px inset;transition: border 0.4s, box-shadow 0.4s, background-color 1.2s;")
                    $(".box-wrapper span small").attr("style", "left: 18px;background-color: rgb(0, 150, 136);transition: background-color 0.4s, left 0.2s;")
                }
                else {
                    $("#switch_demo_1").removeAttr("checked")
                    $(".box-wrapper span").attr("style", "background-color: rgba(0, 0, 0, 0.26);border-color: rgba(0, 0, 0, 0.26);box-shadow: rgba(0, 0, 0, 0.26) 0px 0px 0px 0px inset;transition: border 0.4s, box-shadow 0.4s;")
                    $(".box-wrapper span small").attr("style", "left: 0px;background-color: rgb(250, 250, 250);transition: background-color 0.4s, left 0.2s;")
                }
            }, function error(respone) {
                $("#loader").css("display", "none")
            })
        }
        $scope.ShowAdd = function () {
            $scope.isAdd = true;
            $scope.language = {
                Ensign: '/FileDefault/image/img-default.gif',
                Title: '',
                Key: '',
                Index: 1,
                Active: true
            }
            $scope.OpenWindow()
        }
        $scope.Post = function () {
            $scope.language.Ensign = $("#Logo").val()
            $scope.language.Active = $("#switch_demo_1")[0].checked
            var v1 = validate.isNullOrEmptySingleShowError({ value: $scope.language.Ensign, key: 'Ensign' });
            var v2 = validate.isNullOrEmptySingleShowError({ value: $scope.language.Title, key: 'Title' })
            var v3 = validate.isNullOrEmptySingleShowError({ value: $scope.language.Key, key: 'Key' })
            var v4 = validate.isNotNumberSingleShowError({ value: $scope.language.Index, key: 'Index' })
            if (v1 || v2 || v3 || v4) {
                notify.error('Giá trị không hợp lệ!')
                return
            }
            $("#loader").css("display", "block")
            $http({
                method: "POST",
                url: urlPost,
                data: $scope.language
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
        $scope.Put = function () {
            $scope.language.Ensign = $("#Logo").val()
            $scope.language.Active = $("#switch_demo_1")[0].checked
            var v1 = validate.isNullOrEmptySingleShowError({ value: $scope.language.Ensign, key: 'Ensign' });
            var v2 = validate.isNullOrEmptySingleShowError({ value: $scope.language.Title, key: 'Title' })
            var v3 = validate.isNullOrEmptySingleShowError({ value: $scope.language.Key, key: 'Key' })
            var v4 = validate.isNotNumberSingleShowError({ value: $scope.language.Index, key: 'Index' })
            if (v1 || v2 || v3 || v4)
                return
            $("#loader").css("display", "block")
            $http({
                method: "POST",
                url: urlPut,
                data: $scope.language
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                $scope.GetData()
                $scope.CloseWindow()
                notify.success("Thành công")
            }, function error(respone) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra")
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
                $scope.gridOptions.data = data.languages;
                $scope.gridOptions.totalItems = data.totalRecord
            }, function error(response) {
                $("#loader").css("display", "none")

            });
        }
        $scope.GetData()
    }])