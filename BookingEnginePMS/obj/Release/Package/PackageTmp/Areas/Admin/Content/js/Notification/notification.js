var urlGet = "/Admin/Notification/Get"
var urlDelete = "/Admin/Notification/Delete"
var urlPost = "/Admin/Notification/Post"
var urlDetail = "/Admin/Notification/Detail"
var urlPut = "/Admin/Notification/Put"
var urlGetAllUser = "/Admin/Notification/GetAllUser"

app.controller('controller', ['$rootScope','$scope', '$http', '$log', 'uiGridConstants', 'validate', 'notify',
    '$timeout',
    function ($rootScope,$scope, $http, $log, uiGridConstants, validate, notify, $timeout) {
        $scope.filters = {
            pageNumber: 1,
            pageSize: 100,
            keySearch: ''
        }
        $scope.data = {
            Title: '',
            Content: ''
        }
        var columnDef = [
            { displayName: "Tiêu đề", name: 'Title', width: '*', minWidth: 150 },
            { displayName: "Nội dung", name: 'Content', width: '*', minWidth: 100 },
            { displayName: "Ngày", name: 'Date', width: 180, cellFilter: 'date:"dd-MM-yyyy HH:mm"' },
            { displayName: "Người tạo", name: 'UserCreate', width: 150 },
            {
                displayName: "Thao tác", name: "#", width: 150, field: "#",
                cellTemplate: '<div class="ui-grid-cell-contents box-control" style="text-align:center" ng-if="row.entity.AllowDelete" ><a href="javascript:void(0);" class="link-control" title="Sửa" ng-click="grid.appScope.Edit(row.entity.NotificationId)">Sửa</a><a href="javascript:void(0);" class="link-control" title="Xóa" ng-click="grid.appScope.ConfirmDelete(row.entity.NotificationId)">Xóa</a></div>',
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
        $scope.Edit = function (value) {
            $scope.OpenWindow()
            $scope.isAdd = false;
            $("#loader").css("display", "block")
            $http({
                url: urlDetail,
                method: "GET",
                params: {
                    id: value
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                var data = JSON.parse(response.data)
                $scope.data = data.notification
                $scope.users = data.users
                var date = $scope.data.Date.split('T')
                $("#FromDate_Step1").val($scope.FormatDate(date[0]))
                var hour = date[1].split(':')
                $("#HourCheckin_Step1").val(hour[0] + ':' + hour[1])
                }, function error(response) {
                    $("#loader").css("display", "none")
            });
        }
        $scope.ShowAdd = function () {
            $scope.OpenWindow()
            $scope.isAdd = true
            $scope.data = {
                Title: '',
                Content: ''
            }
            $("#loader").css("display", "block")
            $http({
                url: urlGetAllUser,
                method: "GET",
                params: $scope.filters
            }).then(function success(response) {
                $("#loader").css("display", "none")
                $scope.users = response.data;
                $scope.users.forEach(x => {
                    x.Access = true
                })
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.ConfirmDelete = function (value) {
            $scope.itemChoose = value;
            UIkit.modal("#ModalConfirm").show()
        };
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
                $scope.gridOptions.data = data.notifications;
                $scope.gridOptions.totalItems = data.totalRecord
            }, function error(response) {
                $("#loader").css("display", "none")
            });
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
            var fromDate = $("#FromDate_Step1").val();
            var hourFrom = $("#HourCheckin_Step1").val();
            var v1 = validate.isNotDate({ value: fromDate, key: 'fromDate' })
            var v2 = validate.isNotHour({ value: hourFrom, key: 'hourFrom' })
            var v3 = validate.isNullOrEmptySingleShowError({ value: $scope.data.Title, key: 'Title' })
            var v4 = validate.isNullOrEmptySingleShowError({ value: $scope.data.Content, key: 'Content' })
            if (v1 || v2 || v3 || v4) {
                notify.error('Giá trị không hợp lệ!')
                return
            }
            $("#loader").css("display", "block")
            $scope.data.Date = $scope.FormatDate(fromDate) + "T" + hourFrom + ":00"
            $http({
                url: urlPost,
                method: "POST",
                data: {
                    notification: $scope.data,
                    users: $scope.users
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                $scope.GetData()
                $scope.CloseWindow()
                $scope.notifyToEmployee()
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.Put = function () {
            var fromDate = $("#FromDate_Step1").val();
            var hourFrom = $("#HourCheckin_Step1").val();
            var v1 = validate.isNotDate({ value: fromDate, key: 'fromDate' })
            var v2 = validate.isNotHour({ value: hourFrom, key: 'hourFrom' })
            var v3 = validate.isNullOrEmptySingleShowError({ value: $scope.data.Title, key: 'Title' })
            var v4 = validate.isNullOrEmptySingleShowError({ value: $scope.data.Content, key: 'Content' })
            if (v1 || v2 || v3 || v4) {
                notify.error('Giá trị không hơp lệ!')
                return
            }
            $("#loader").css("display", "block")
            $scope.data.Date = $scope.FormatDate(fromDate) + "T" + hourFrom + ":00"
            $http({
                url: urlPut,
                method: "POST",
                data: {
                    notification: $scope.data,
                    users: $scope.users
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                $scope.GetData()
                $scope.CloseWindow()
                $scope.notifyToEmployee()
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.notifyToEmployee = function () {
            $("#get_notify").click()
            otherUpdateNotify = false;
            $rootScope.GetAllMessageAndNotify()
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
        $scope.GetData()
    }]);