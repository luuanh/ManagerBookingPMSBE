var urlGet = "/Admin/Manage/Get"
var urlPost = "/Admin/Manage/Post"
var urlPut = "/Admin/Manage/Put"
var urlDetail = "/Admin/Manage/Detail"
var urlDelete = "/Admin/Manage/Delete"

var urlGetScreen = "/Admin/Language/GetScreen"
var urlGetHotel = "/Admin/Manage/GetHotel"

app.controller('controller', ['$scope', '$http', '$log', 'uiGridConstants', 'validate', 'notify',
    function ($scope, $http, $log, uiGridConstants, validate, notify) {
        $scope.filters = {
            pageNumber: 1,
            pageSize: 100,
            keySearch: ''
        }
        $scope.isAdd = true;
        $scope.itemChoose = 0;
        $scope.user = {
            Photo: '/FileDefault/image/img-default.gif',
            UserName: '',
            Email: '',
            FullName: '',
            Roles: []
        }
        var columnDef = [
            { displayName: "Tên tài khoản", name: 'UserName', width: 150 },
            { displayName: "Họ tên", name: 'FullName', width: 200 },
            {
                displayName: "Ảnh",
                name: "Photo",
                width: 100,
                cellTemplate: '<div class="ui-grid-cell-contents box-image" style="text-align:center"><img ng-src="{{grid.appScope.ShowImage(row.entity.Photo)}}" /></div>',
            },
            { displayName: "Email", name: 'Email', width: '*' },
            { displayName: "Ngày tạo", name: 'CreateDate', width: 180, cellFilter: 'date:"dd-MM-yyyy HH:mm"' },
            {
                displayName: "Thao tác", name: "#", width: 150, field: "#",
                cellTemplate: '<div class="ui-grid-cell-contents box-control" style="text-align:center"><a href="javascript:void(0);" class="link-control" title="Sửa" ng-click="grid.appScope.Edit(row.entity.UserId)">Sửa</a><a href="javascript:void(0);" class="link-control" title="Xóa" ng-click="grid.appScope.ConfirmDelete(row.entity.UserId)">Xóa</a></div>',
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
                var data = respone.data
                $scope.user = data.user;
                $scope.hotels = data.hotels;
                $scope.screens = data.screens;
            }, function error(respone) {
                $("#loader").css("display", "none")
            })
        }
        $scope.ShowAdd = function () {
            $scope.isAdd = true;
            $scope.user = {
                Photo: '/FileDefault/image/img-default.gif',
                UserName: '',
                Email: '',
                FullName: '',
                Roles: []
            }
            $scope.hotels.forEach(x => x.Check = true)
            $scope.screens.forEach(x => x.Check = true)
            $scope.OpenWindow()
        }
        $scope.ChangeLanguage = function () {
            var index = $scope.servicecategory.ServiceCategoryLanguages.findIndex(x => x.LanguageId == $scope.languageChoose);
            $scope.ModelServiceCategory = $scope.servicecategory.ServiceCategoryLanguages[index]
        }
        $scope.Post = function () {
            var v1 = validate.isNullOrEmptySingleShowError({ value: $scope.user.UserName, key: 'UserName' })
            var v2 = validate.isNullOrEmptySingleShowError({ value: $scope.user.FullName, key: 'FullName' })
            var v3 = validate.isNullOrEmptySingleShowError({ value: $scope.user.Password, key: 'Password' })
            var v4 = validate.isEmail({ value: $scope.user.Email, key: 'Email' })
            if (v1 || v2 || v3 || v4) {
                notify.error('Giá trị không hợp lệ!')
                return
            }
            $("#loader").css("display", "block")
            $scope.user.Photo = $("#Logo").val()
            $http({
                method: "POST",
                url: urlPost,
                data: {
                    user: $scope.user,
                    hotels: $scope.hotels,
                    screens: $scope.screens
                }
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                if (respone.data == -1) {
                    alert("Account already exists")
                    return;
                }
                $scope.GetData()
                notify.success("Thành công")
                $scope.CloseWindow()
            }, function error(respone) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra!")
            })
        }
        $scope.Put = function () {
            var v1 = validate.isNullOrEmptySingleShowError({ value: $scope.user.UserName, key: 'UserName' })
            var v2 = validate.isNullOrEmptySingleShowError({ value: $scope.user.FullName, key: 'FullName' })
            var v3 = validate.isEmail({ value: $scope.user.Email, key: 'Email' })
            if (v1 || v2 || v3) {
                notify.error('Giá trị không hợp lệ!')
                return
            }
            $("#loader").css("display", "block")
            $scope.user.Photo = $("#Logo").val()
            $http({
                method: "POST",
                url: urlPut,
                data: {
                    user: $scope.user,
                    hotels: $scope.hotels,
                    screens: $scope.screens,
                }
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
                var data = JSON.parse(response.data);
                $scope.gridOptions.data = data.users;
                $scope.gridOptions.totalItems = data.totalRecord
            }, function error(response) {
                $("#loader").css("display", "none")

            });
        }
        $scope.GetScreen = function () {
            $http({
                url: urlGetScreen,
                method: "GET"
            }).then(function success(respone) {
                $scope.screens = respone.data
                $scope.screens.forEach(x => x.Check = true)
            }, function error(respone) {

            })
        }
        $scope.GetScreen()
        $scope.GetHotel = function () {
            $http({
                url: urlGetHotel,
                method: "GET"
            }).then(function success(respone) {
                $scope.hotels = respone.data
                $scope.hotels.forEach(x => x.Check = true)
            }, function error(respone) {

            })
        }
        $scope.GetScreen()
        $scope.GetHotel()
        $scope.GetData()
    }])