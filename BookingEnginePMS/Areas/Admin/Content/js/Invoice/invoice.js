var urlGet = "/Admin/Invoice/Get"
var urlPost = "/Admin/Invoice/Post"
var urlPut = "/Admin/Invoice/Put"
var urlPaid = "/Admin/Invoice/Paid"
var urlClearCard = "/Admin/Invoice/ClearCard"
var urlDetail = "/Admin/Invoice/Detail"
var urlDelete = "/Admin/Invoice/Delete"
var urlGetCurrency = "/Admin/Currency/Get"

app.controller('controller', ['$scope', '$http', '$log', 'uiGridConstants', 'validate', 'notify',
    '$timeout',
    function ($scope, $http, $log, uiGridConstants, validate, notify, $timeout) {
        $scope.filters = {
            pageNumber: 1,
            pageSize: 100,
            keySearch: '',
            status: '-1',
            feedback: '-1',
            startDate: '',
            endDate: '',
        }
        $scope.isAdd = true
        $scope.invoice = {
            CustomerName: '',
            Phone: '',
            Email: '',
            ArrivalDate: '',
            TotalAmount: 0,
            CurrencyCode: 'đ',
            Description: '',
            Link: ''
        }
        $scope.showSercurityCode = true;
        $scope.ArrivalDate = ''

        var columnDef = [
            {
                displayName: "Mã",
                name: 'InvoiceCode',
                width: 120,
                pinnedLeft: true,
                enableFiltering: false,
                enableSorting: false
            },
            { displayName: "Khách hàng", name: 'CustomerName', width: '*', minWidth: 150 },
            {
                displayName: "Total Amount",
                name: 'TotalAmount',
                width: 150,
                cellTemplate: '<div class="ui-grid-cell-contents" ng-bind-html="grid.appScope.getTotalAmount(row.entity.TotalAmount,row.entity.CurrencyCode)"></div>'
            },
            { displayName: "Ngày đến", name: 'ArrivalDate', width: 150, cellFilter: 'date:"dd-MM-yyyy"' },
            { displayName: "Ngày tạo hóa đơn", name: 'DateInvoice', width: 150, cellFilter: 'date:"dd-MM-yyyy HH:mm"' },
            { displayName: "Người tạo", name: 'UserCreate', width: 120 },
            {
                displayName: "Trạng thái",
                name: 'Status',
                width: 120,
                cellTemplate: '<div class="ui-grid-cell-contents" ng-bind-html="grid.appScope.getStatusInvoice(row.entity.Status)"></div>'
            },
            {
                displayName: "Phản hồi",
                name: 'FeedBack',
                width: 120,
                cellTemplate: '<div class="ui-grid-cell-contents" ng-bind-html="grid.appScope.getFeedBack(row.entity.FeedBack)"></div>'
            },
            {
                displayName: "Thao tác", name: "#", width: 150, field: "#",
                cellTemplate: '<div class="ui-grid-cell-contents box-control" style="text-align:center"><a href="javascript:void(0);" class="link-control" title="Xem" ng-click="grid.appScope.Edit(row.entity.InvoiceId)">Xem</a><a href="javascript:void(0);" class="link-control" title="Xóa" ng-click="grid.appScope.ConfirmDelete(row.entity.InvoiceId)">Hủy</a></div>',
            }
        ];
        $scope.getTotalAmount = function (totalAmount, currencyCode) {
            return "<span>" + totalAmount + " " + currencyCode + "</span>";
        }
        $scope.getStatusInvoice = function (status) {
            return statusInvoice[(status - 0)]
        }
        $scope.getFeedBack = function (feedback) {
            return getFeedBack[(feedback - 0)]
        }
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
                $scope.invoice = JSON.parse(response.data)
                $("#ArrivalDate").val($scope.FormatDate($scope.invoice.ArrivalDate.split("T")[0]))
            }, function error(respone) {
                $("#loader").css("display", "none")
            })
        }
        $scope.showHideSercurityCode = function () {
            $scope.showSercurityCode = !$scope.showSercurityCode
            if ($scope.showSercurityCode)
                $("#sercurityCode").attr("type", "password")
            else
                $("#sercurityCode").attr("type", "text")
        }
        $scope.ShowAdd = function () {
            $scope.OpenWindow()
            $scope.isAdd = true
            $scope.invoice = {
                Title: 'Mr',
                CustomerName: '',
                Phone: '',
                Email: '',
                ArrivalDate: '',
                TotalAmount: 0,
                CurrencyCode: 'đ',
                Description: '',
                Link: ''
            }
            $scope.invoice.Link = location.origin + "/Home/Invoice?code=...&email="
        }
        $scope.Post = function () {
            var v1 = validate.isNullOrEmptySingleShowError({ value: $scope.invoice.CustomerName, key: 'CustomerName' })
            var v2 = validate.isEmail({ value: $scope.invoice.Email, key: 'Email' })
            var v3 = validate.isNullOrEmptySingleShowError({ value: $("#ArrivalDate").val(), key: 'ArrivalDate' })
            var v4 = validate.isNotNumberSingleShowError({ value: $scope.invoice.TotalAmount, key: 'TotalAmount' })
            if (v1 || v2 || v3 || v4) {
                notify.error('Giá trị không hợp lệ!')
                return
            }
            $("#loader").css("display", "block")
            $scope.invoice.ArrivalDate = $scope.FormatDate($("#ArrivalDate").val())
            $http({
                method: "POST",
                url: urlPost,
                data: $scope.invoice
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
            var v1 = validate.isNullOrEmptySingleShowError({ value: $scope.invoice.CustomerName, key: 'CustomerName' })
            var v2 = validate.isEmail({ value: $scope.invoice.Email, key: 'Email' })
            var v3 = validate.isNullOrEmptySingleShowError({ value: $("#ArrivalDate").val(), key: 'ArrivalDate' })
            var v4 = validate.isNotNumberSingleShowError({ value: $scope.invoice.TotalAmount, key: 'TotalAmount' })
            if (v1 || v2 || v3 || v4) {
                notify.error('Giá trị không hợp lệ!')
                return
            }
            $("#loader").css("display", "block")
            $scope.invoice.ArrivalDate = $scope.FormatDate($("#ArrivalDate").val())
            $http({
                method: "POST",
                url: urlPut,
                data: $scope.invoice
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
        $scope.Paid = function (invoiceId) {
            $http({
                method: "POST",
                url: urlPaid,
                data: {
                    id: invoiceId
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
        $scope.ClearCard = function (invoiceId) {
            $http({
                method: "POST",
                url: urlClearCard,
                data: {
                    id: invoiceId
                }
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                notify.success("Thành công")
                $scope.invoice.NameOnCard = ''
                $scope.invoice.CardNumber = ''
                $scope.invoice.SecurityCode = ''
                $scope.invoice.ExprireMonth = -1
                $scope.invoice.ExprireYear = -1
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
                notify.error("Có lỗi sảy ra")
            })
        }
        $scope.FormatDate = function (date) {
            date = date.trim()
            var dt = date.split('-');
            var d = dt[0];
            dt[0] = dt[2];
            dt[2] = d;
            return dt.join('-')
        }
        $scope.GetData = function () {
            var fromdate = $("#FromDate").val();
            var todate = $("#ToDate").val();
            if (fromdate == null || fromdate == '' || todate == null || todate == '') {
                alert('Ngày tháng không hợp lệ');
                return;
            }
            $("#loader").css("display", "block")
            $scope.filters.startDate = $scope.FormatDate(fromdate)
            $scope.filters.endDate = $scope.FormatDate(todate)
            $http({
                url: urlGet,
                method: "GET",
                params: $scope.filters
            }).then(function success(response) {
                $("#loader").css("display", "none")
                var data = JSON.parse(response.data);
                $scope.gridOptions.data = data.invoices;
                $scope.gridOptions.totalItems = data.totalRecord
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.GetCurrency = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlGetCurrency,
                method: "GET"
            }).then(function success(response) {
                $("#loader").css("display", "none")
                var data = response.data;
                $scope.currencies = data
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.Init = function () {
            var datenow = new Date()
            datenow.setDate(datenow.getDate() - 30);
            var fromdate = datenow.getDate() + "-" + (datenow.getMonth() + 1) + "-" + datenow.getFullYear();
            var newdate = new Date();
            newdate.setDate(datenow.getDate() + 45);
            var todate = newdate.getDate() + "-" + (newdate.getMonth() + 1) + "-" + newdate.getFullYear();
            $("#FromDate").val(fromdate)
            $("#ToDate").val(todate)
            $scope.FromDate = fromdate;
            $scope.ToDate = todate;
            $scope.invoice.Link = location.origin + "/Home/Invoice?code=...&email="
            $scope.GetData()
        }
        $scope.Init()
        $scope.GetCurrency()
    }]);