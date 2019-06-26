var urlGet = "/Home/GetDashboard"

app.controller('controller', ['$scope', '$http','uiGridConstants',
    function ($scope, $http, uiGridConstants) {
      
       
        $scope.FormatDate = function (date) {
            date = date.trim()
            var dt = date.split('-');
            var d = dt[0];
            dt[0] = dt[2];
            dt[2] = d;
            return dt.join('-')
        }
        $scope.getTypePaymentHotel = function (type) {
            return typePaymentHotel[type];
        };
        $scope.getStatusHotel = function (status) {
            return statusHotel[status - 1];
        };
        //
        $scope.filters = {
            pageNumber: 1,
            pageSize: 20,
            keySearch: '',
            status: '-1'
          
        }
        var columnDef = [
            
            {
                displayName: "Mã KS", name: 'Code', width: 100,
                pinnedLeft: true,
                enableFiltering: false,
                enableSorting: false
            },
            {
                displayName: "Tên",
                name: 'Name',
                width: '*',
                minWidth: 200,
                pinnedLeft: true,
                enableFiltering: false,
                enableSorting: false,
                cellTemplate: '<div class="ui-grid-cell-contents box-control"><a href="/Hotel/Detail?id={{row.entity.HotelId}}" class="link-control" target="_blank">{{row.entity.Name}}</a></div>'
            },
            { displayName: "Ngày tạo", name: 'CreateDate', width: 150, cellFilter: 'date:"dd-MM-yyyy HH:mm"' },
            { displayName: "Ngày gia hạn", name: 'DayStartUse', width: 150, cellFilter: 'date:"dd-MM-yyyy HH:mm"' },
            
            { displayName: "TG gia hạn", name: 'TimeExtended', width: 100 },
            {
                displayName: "Hình thức TT",
                name: 'TypePaymentHotel',
                width: 120,
                cellTemplate: '<div class="ui-grid-cell-contents" ng-bind-html="grid.appScope.getTypePaymentHotel(row.entity.TypePaymentHotel)"></div>'
            },
            {
                displayName: "Trạng thái",
                name: 'Status',
                width: 120,
                cellTemplate: '<div class="ui-grid-cell-contents" ng-bind-html="grid.appScope.getStatusHotel(row.entity.Status)"></div>'
            },
            {
                displayName: "Thao tác",
                width: 150,
                name: 'ThaoTac',
               
                cellTemplate: '<div><a class="ui-grid-cell-contents box-control" style="margin-left:1em;margin-right:1em" href="/Home/GetDetailHotelById?id={{row.entity.HotelId}}">Sửa</a><a href="" ng-click="grid.appScope.DeleteHotel(row.entity.HotelId)">Xoa</a></div>'
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
                    $scope.GetAllHotel();
                  
                });

            },
            exporterMenuCsv: true,
            enableGridMenu: true,
            autoResize: true
        }

        //hotel use BE
        $scope.gridOptionsBE = {
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
                    $scope.GetAllHotelUseBE;

                });

            },
            exporterMenuCsv: true,
            enableGridMenu: true,
            autoResize: true
        }
        //
        //hotel use PMS
        $scope.gridOptionsPMS = {
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
                    $scope.GetAllHotelUsePMS;

                });

            },
            exporterMenuCsv: true,
            enableGridMenu: true,
            autoResize: true
        }
        //
        //hotel use PMSBE
        $scope.gridOptionsPMSBE = {
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
                    $scope.GetAllHotelUsePMSBE;

                });

            },
            exporterMenuCsv: true,
            enableGridMenu: true,
            autoResize: true
        }
        //
        $scope.GetData = function () {
           
            $("#loader").css("display", "block")
            $http({
                url: urlGet,
                method: "GET"
            }).then(function success(response) {
                $("#loader").css("display", "none")
                $scope.data = response.data;
                console.log($scope.data);
            }, function error(response) {
                $("#loader").css("display", "none")

            });
        };
      
        $scope.GetAllHotel = function () {
          
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
                url: '/Hotel/GetListHotel',
                method: "GET",
                params: $scope.filters
            }).then(function success(response) {
                $("#loader").css("display", "none")
                var data = JSON.parse(response.data);
                console.log(data);
                $scope.gridOptions.data = data.hotels;
                $scope.gridOptions.totalItems = data.totalRecord
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        };
       

        $scope.GetAllHotelUseBE = function () {
        
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
                url: '/Hotel/GetListHotelUseBE',
                method: "GET",
                params: $scope.filters
            }).then(function success(response) {
                $("#loader").css("display", "none")
                var data = JSON.parse(response.data);
                console.log(data);
                $scope.gridOptionsBE.data = data.hotels;
                $scope.gridOptionsBE.totalItems = data.totalRecord

            }, function error(response) {
                $("#loader").css("display", "none")
            });
        };

        $scope.GetAllHotelUsePMS = function () {

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
                url: '/Hotel/GetListHotelUsePMS',
                method: "GET",
                params: $scope.filters
            }).then(function success(response) {
                $("#loader").css("display", "none")
                var data = JSON.parse(response.data);
                console.log(data);
                $scope.gridOptionsPMS.data = data.hotels;
                $scope.gridOptionsPMS.totalItems = data.totalRecord

            }, function error(response) {
                $("#loader").css("display", "none")
            });
        };

        $scope.GetAllHotelUsePMSBE = function () {

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
                url: '/Hotel/GetListHotelUsePMSBE',
                method: "GET",
                params: $scope.filters
            }).then(function success(response) {
                $("#loader").css("display", "none")
                var data = JSON.parse(response.data);
                console.log(data);
                $scope.gridOptionsPMSBE.data = data.hotels;
                $scope.gridOptionsPMSBE.totalItems = data.totalRecord

            }, function error(response) {
                $("#loader").css("display", "none")
            });
        };

       
        // delete hotel
        $scope.DeleteHotel = function (id) {
            alert("Không được phép xóa");
            //$("#loader").css("display", "block")
            //$http({
            //    url: "/Home/DeleteHotel",
            //    method: "Put",
            //    params: { HotelId:id}
            //}).then(function success(response) {
            //    $("#loader").css("display", "none")
            //    alert("Thanh cong");
            //}, function error(response) {
            //    $("#loader").css("display", "none")
            //    alert("loi");
            //});
        };




        //
        $scope.Init = function () {
            var datenowBase = new Date()
            var datenow = new Date()
            var todate = datenow.getDate() + "-" + (datenow.getMonth() + 1) + "-" + datenow.getFullYear();
            datenow.setDate(datenow.getDate() - 200);
            var fromdate = datenow.getDate() + "-" + (datenow.getMonth() + 1) + "-" + datenow.getFullYear();
            $("#FromDate").val(fromdate)
            $("#ToDate").val(todate)
            $scope.FromDate = fromdate;
            $scope.ToDate = todate;
            $scope.GetData();
            $scope.GetAllHotel();
            $scope.GetAllHotelUseBE();
            $scope.GetAllHotelUsePMS();
            $scope.GetAllHotelUsePMSBE();
        
        }
        $scope.Init();
    }

   
      
   


])


