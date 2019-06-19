var urlGet = "/Admin/Guest/Get"
var urlPost = "/Admin/Guest/Post"
var urlPut = "/Admin/Guest/Put"
var urlDetail = "/Admin/Guest/Detail"

app.controller('controller', ['$scope', '$http', '$log', 'uiGridConstants', 'validate', 'notify',
    function ($scope, $http, $log, uiGridConstants, validate, notify) {
        $scope.filters = {
            pageNumber: 1,
            pageSize: 100,
            keySearch: ''
        }
        $scope.guest = {
            FirstName: '',
            SurName: '',
            Photo: '/FileDefault/image/img-default.gif',
            TypeGuest: 0,
            TypeGuestName: 'New',
            Gender: -1,
            CompanyId: -1,
            ZIPCode: '',
            Dob: '',
            Region: '',
            Country: 'Vietnam',
            IdentityCart: '',
            DoIssueIdentity: '',
            AddressIssue: '',
            Passport: '',
            DoIssuePassport: '',
            ExpirationDate: '',
            CreditCard: '',
            DoIssueCreditCard: '',
            CVC: '',
            Phone: '',
            Fax: '',
            Email: '',
            Address: '',
            Note: ''
        }
        $scope.isAdd = true;
        $scope.itemChoose = 0;
        $scope.getSource = function (source) {
            return sourceReservation[(source - 0) - 1]
        }
        var columnDef = [
            {
                displayName: "Tên",
                name: 'FirstName',
                width: '*',
                minWidth: 120,
                cellTemplate: '<div class="ui-grid-cell-contents box-control"><a href="javascript:void(0);" ng-click="grid.appScope.Edit(row.entity.GuestId)" class="link-control">{{row.entity.FirstName}}</a></div>'
            },
            {
                displayName: "Họ",
                name: 'SurName',
                width: '*',
                minWidth: 120,
                cellTemplate: '<div class="ui-grid-cell-contents box-control"><a href="javascript:void(0);" ng-click="grid.appScope.Edit(row.entity.GuestId)" class="link-control">{{row.entity.SurName}}</a></div>'
            },
            { displayName: "Loại khách", name: 'TypeGuestName', width: 120 },
            { displayName: "Email", name: 'Email', width: '*', minWidth: 200 },
            { displayName: "Số điện thoại", name: 'Phone', width: 120 },
            { displayName: "Quốc gia", name: 'Country', width: 120 }
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
            $("#loader").css("display", "block")
            $scope.Dob = ""
            $scope.IdChoose = value
            $scope.isAdd = false
            $http({
                method: "GET",
                url: urlDetail,
                params: {
                    id: value
                }
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                var data = JSON.parse(respone.data)
                $scope.guest = data.guest
                $scope.history = data.history
                $scope.totalPrice = data.totalPrice;
                $scope.totalPaid = data.totalPaid;
                $scope.Totalaccumulated = data.Totalaccumulated;
                if ($scope.guest.Dob != null)
                    $scope.Dob = $scope.FormatDate($scope.guest.Dob.split('T')[0])
                }, function error(respone) {
                    $("#loader").css("display", "none")
            })
        }
        $scope.ShowAdd = function () {
            $(".uk-tab>li").removeClass('uk-active')
            $(".genenral-tab").addClass('uk-active')
            $scope.isAdd = true;
            $scope.guest = {
                FirstName: '',
                SurName: '',
                Photo: '/FileDefault/image/img-default.gif',
                TypeGuest: 0,
                TypeGuestName: 'New',
                Gender: -1,
                CompanyId: -1,
                ZIPCode: '',
                Dob: '',
                Region: '',
                Country: 'Vietnam',
                IdentityCart: '',
                DoIssueIdentity: '',
                AddressIssue: '',
                Passport: '',
                DoIssuePassport: '',
                ExpirationDate: '',
                CreditCard: '',
                DoIssueCreditCard: '',
                CVC: '',
                Phone: '',
                Fax: '',
                Email: '',
                Address: '',
                Note: ''
            }
            $("#Dob").val('');
            $scope.OpenWindow()

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
            var v1 = validate.isNullOrEmptySingleShowError({ value: $scope.guest.FirstName, key: 'FirstName' })
            var v2 = validate.isNullOrEmptySingleShowError({ value: $scope.guest.SurName, key: 'SurName' })
            var v4 = validate.isNullOrEmptySingleShowError({ value: $scope.guest.Phone, key: 'Phone' })
            if (v1 || v2 || v4) {
                notify.error('Giá trị không hợp lệ!')
                return
            }
            $("#loader").css("display", "block")
            if ($("#Dob").val() != '') {
                var datetime = $("#Dob").val();
                $scope.guest.Dob = $scope.FormatDate(datetime)
            }
            $http({
                method: "POST",
                url: urlPost,
                data: $scope.guest
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
            var v1 = validate.isNullOrEmptySingleShowError({ value: $scope.guest.FirstName, key: 'FirstName' })
            var v2 = validate.isNullOrEmptySingleShowError({ value: $scope.guest.SurName, key: 'SurName' })
            var v4 = validate.isNullOrEmptySingleShowError({ value: $scope.guest.Phone, key: 'Phone' })
            if (v1 || v2 || v4) {
                notify.error('Giá trị không hợp lệ!')
                return
            }
            $("#loader").css("display", "block")
            if ($("#Dob").val() != '') {
                var datetime = $("#Dob").val();
                $scope.guest.Dob = $scope.FormatDate(datetime)
            }
            $http({
                method: "POST",
                url: urlPut,
                data: $scope.guest
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
                $scope.gridOptions.data = data.guests;
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