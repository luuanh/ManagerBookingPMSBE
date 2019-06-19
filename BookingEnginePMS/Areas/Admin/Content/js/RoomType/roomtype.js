var urlGet = "/Admin/RoomType/Get"
var urlPost = "/Admin/RoomType/Post"
var urlPut = "/Admin/RoomType/Put"
var urlDetail = "/Admin/RoomType/Detail"
var urlDelete = "/Admin/RoomType/Delete"
var urlGetLanguage = "/Admin/Language/GetActive"

app.controller('controller', ['$scope', '$http', '$log', 'uiGridConstants', 'validate', 'notify',
    function ($scope, $http, $log, uiGridConstants, validate, notify) {
        $scope.filters = {
            pageNumber: 1,
            pageSize: 100,
            keySearch: ''
        }
        $scope.roomtype = {
            Image: '/FileDefault/image/img-default.gif',
            GalleryDefault: '/FileDefault/image/img-default.gif',
            RoomTypeName: '',
            MaxPeople: 2,
            Adult: 2,
            Children: 0,
            HasExtrabed: true,
            Index: 1,
            Size: '',
            RoomTypeGalleries: [],
            RoomTypeLanguages: [],
            RoomTypeExtrabedIds: []
        }
        $scope.isAdd = true;
        $scope.itemChoose = 0;
        $scope.extrabeds = []
        var columnDef = [
            { displayName: "Tên loại phòng", name: 'RoomTypeName', width: '*', minWidth: 150 },
            { displayName: "Mã", name: 'RoomTypeCode', width: 120 },
            {
                displayName: "Hình ảnh",
                name: "Image",
                width: 100,
                cellTemplate: '<div class="ui-grid-cell-contents box-image" style="text-align:center"><img ng-src="{{grid.appScope.ShowImage(row.entity.Image)}}" /></div>',
            },
            { displayName: "Số người tối đa", name: 'MaxPeople', width: 100 },
            {
                displayName: "Giường phụ", name: 'HasExtrabed', width: 100,
                cellTemplate: '<div class="ui-grid-cell-contents box-extrabed" style="text-align:center" ng-bind-html="grid.appScope.ShowExtrabedForList(row.entity.HasExtrabed)"></div>',
            },
            { displayName: "Thứ tự", name: 'Index', width: 100 },
            {
                displayName: "Thao tác", name: "#", width: 150, field: "#",
                cellTemplate: '<div class="ui-grid-cell-contents box-control" style="text-align:center"><a href="javascript:void(0);" class="link-control" title="Sửa" ng-click="grid.appScope.Edit(row.entity.RoomTypeId)">Sửa</a><a href="javascript:void(0);" class="link-control" title="Xóa" ng-click="grid.appScope.ConfirmDelete(row.entity.RoomTypeId)">Xóa</a></div>',
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
        $scope.ShowExtrabedForList = function (value) {
            if (value)
                return statusExtrabed[0];
            else
                return statusExtrabed[1];
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
                method: "GET",
                url: urlDetail,
                params: {
                    id: value
                }
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                $scope.roomtype = respone.data
                $("#kUI_multiselect_basic").data("kendoMultiSelect").value($scope.roomtype.RoomTypeExtrabedIds);
                if ($scope.language.length > 0) {
                    $scope.languageChoose = $scope.language[0].LanguageId
                    $scope.ModelRoomType = $scope.roomtype.RoomTypeLanguages[0]
                }
            }, function error(respone) {
                $("#loader").css("display", "none")
            })
        }
        $scope.ShowAdd = function () {
            $scope.isAdd = true;
            $scope.roomtype = {
                Image: '/FileDefault/image/img-default.gif',
                GalleryDefault: '/FileDefault/image/img-default.gif',
                RoomTypeName: '',
                MaxPeople: 2,
                Adult: 2,
                Children: 0,
                HasExtrabed: true,
                Index: 1,
                Size: '',
                RoomTypeGalleries: [],
                RoomTypeLanguages: [],
                RoomTypeExtrabedIds: []
            }
            $scope.language.forEach(x => {
                $scope.roomtype.RoomTypeLanguages.push({
                    LanguageId: x.LanguageId,
                    ExtrabedOption: '',
                    DescriptionBed: '',
                    DescriptionView: '',
                    Note: ''
                })
            });
            if ($scope.language.length > 0) {
                $scope.languageChoose = $scope.language[0].LanguageId
                $scope.ModelRoomType = $scope.roomtype.RoomTypeLanguages[0]
            }
            $("#kUI_multiselect_basic").data("kendoMultiSelect").value([]);
            $scope.OpenWindow()

        }
        $scope.ChangeLanguage = function () {
            var index = $scope.roomtype.RoomTypeLanguages.findIndex(x => x.LanguageId == $scope.languageChoose);
            $scope.ModelRoomType = $scope.roomtype.RoomTypeLanguages[index]
        }
        $scope.AddGallary = function () {
            var value = $("#Logo2").val()
            $scope.roomtype.RoomTypeGalleries.push({
                Image: value
            })
        }
        $scope.RemoveGallery = function (index) {
            $scope.roomtype.RoomTypeGalleries.splice(index, 1)
        }
        $scope.Post = function () {
            var v1 = validate.isNullOrEmptySingleShowError({ value: $scope.roomtype.RoomTypeName, key: 'RoomTypeName' })
            var v2 = validate.isNullOrEmptySingleShowError({ value: $scope.roomtype.RoomTypeCode, key: 'RoomTypeCode' })
            if (v1 || v2) {
                notify.error('Giá trị không hợp lệ!')
                return
            }
            $("#loader").css("display", "block")
            $scope.roomtype.Image = $("#Logo").val()
            $scope.roomtype.RoomTypeExtrabedIds = $("#kUI_multiselect_basic").data("kendoMultiSelect").value();
            $http({
                method: "POST",
                url: urlPost,
                data: $scope.roomtype
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
            var v1 = validate.isNullOrEmptySingleShowError({ value: $scope.roomtype.RoomTypeName, key: 'RoomTypeName' })
            var v2 = validate.isNullOrEmptySingleShowError({ value: $scope.roomtype.RoomTypeCode, key: 'RoomTypeCode' })
            if (v1 || v2) {
                notify.error('Giá trị không hợp lệ!')
                return
            }
            $("#loader").css("display", "block")
            $scope.roomtype.Image = $("#Logo").val()
            $scope.roomtype.RoomTypeExtrabedIds = $("#kUI_multiselect_basic").data("kendoMultiSelect").value();
            $http({
                method: "POST",
                url: urlPut,
                data: $scope.roomtype
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
                $scope.gridOptions.data = data.roomTypes;
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