var urlGet = "/Admin/RateAvailability/Get"
var urlGetOnlyRoomType = "/Admin/RateAvailability/GetOnlyRoomType"
var urlPostNumberRoomSell = "/Admin/RateAvailability/PostNumberRoomSell"
var urlPostChangeStatus = "/Admin/RateAvailability/PostChangeStatus"
var urlPostPriceRoomSell = "/Admin/RateAvailability/PostPriceRoomSell"
var urlCloseMultiRoom = "/Admin/RateAvailability/CloseMultiRoom"
var urlOpenMultiRoom = "/Admin/RateAvailability/OpenMultiRoom"


app.controller('controller', ['$scope', '$http', '$log', 'uiGridConstants', 'validate', 'notify',
    function ($scope, $http, $log, uiGridConstants, validate, notify) {
        $scope.filters = {
            fromDate: '',
            toDate: '',
            typeSearch: -1
        }
        $scope.paramsChangeRoomSell = {
            AllDay: true,
            Monday: true,
            Tuesday: false,
            Wednesday: false,
            Thursday: false,
            Friday: false,
            Saturday: false,
            Sunday: false,
            Number: 0,
            RoomTypeId: 0
        }
        $scope.data = [];
        $scope.CheckSomeDayOfWeek = function () {
            if ($scope.paramsChangeRoomSell.Monday == true ||
                $scope.paramsChangeRoomSell.Tuesday == true ||
                $scope.paramsChangeRoomSell.Wednesday == true ||
                $scope.paramsChangeRoomSell.Thursday == true ||
                $scope.paramsChangeRoomSell.Friday == true ||
                $scope.paramsChangeRoomSell.Saturday == true ||
                $scope.paramsChangeRoomSell.Sunday == true
            ) {
                $scope.paramsChangeRoomSell.AllDay = false
            }
        }
        $scope.CheckAllDayOfWeek = function () {
            if ($scope.paramsChangeRoomSell.AllDay) {
                $scope.paramsChangeRoomSell.Monday = false;
                $scope.paramsChangeRoomSell.Tuesday = false;
                $scope.paramsChangeRoomSell.Wednesday = false;
                $scope.paramsChangeRoomSell.Thursday = false;
                $scope.paramsChangeRoomSell.Friday = false;
                $scope.paramsChangeRoomSell.Saturday = false;
                $scope.paramsChangeRoomSell.Sunday = false;
            }
        }
        $scope.ChangeStatus = function (roomtypeId, index, status) {
            $("#loader").css("display", "block")
            var roomtype = $scope.data.find(x => x.RoomTypeId == roomtypeId);
            var rateAvailability = roomtype.RateAvailabilities[index]
            var date = rateAvailability.Date;
            var number = rateAvailability.Number;
            var price = rateAvailability.Price;
            if (status == -1)
                rateAvailability.Status = -1
            else {
                if ((number == 0 || price == 0) && status == 1)
                    rateAvailability.Status = 0
                else if ((number == 0 || price == 0) && status == 2)
                    rateAvailability.Status = -1
                else
                    rateAvailability.Status = 1
            }
            $http({
                url: urlPostChangeStatus,
                method: "POST",
                data: {
                    roomTypeId: roomtypeId,
                    date: date,
                    status: rateAvailability.Status
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                notify.success("Thành công")
            }, function error(response) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra")
            });
        }
        $scope.ShowChangeNumberRoomSell = function (roomTypeId) {
            $scope.paramsChangeRoomSell = {
                AllDay: true,
                Tuesday: false,
                Wednesday: false,
                Thursday: false,
                Friday: false,
                Saturday: false,
                Sunday: false,
                Number: 0,
                RoomTypeId: roomTypeId,
                FromDate: '',
                ToDate: ''
            }
            UIkit.modal("#ModalSetNumberRoomSell").show()
        }
        $scope.ShowChangePriceRoomSell = function (roomTypeId) {
            $scope.paramsChangeRoomSell = {
                AllDay: true,
                Tuesday: false,
                Wednesday: false,
                Thursday: false,
                Friday: false,
                Saturday: false,
                Sunday: false,
                Price: 0,
                RoomTypeId: roomTypeId,
                FromDate: '',
                ToDate: ''
            }
            UIkit.modal("#ModalSetPriceRoomSell").show()
        }
        $scope.PostNumberRoomSell = function () {
            var fromdate = $("#FromDateChange").val();
            var todate = $("#ToDateChange").val();
            if (fromdate == null || fromdate == '' || todate == null || todate == '') {
                notify.error('Ngày tháng không hợp lệ');
                return;
            }
            $("#loader").css("display", "block")
            $scope.paramsChangeRoomSell.FromDate = $scope.FormatDate(fromdate)
            $scope.paramsChangeRoomSell.ToDate = $scope.FormatDate(todate)
            $http({
                url: urlPostNumberRoomSell,
                method: "POST",
                data: $scope.paramsChangeRoomSell
            }).then(function success(response) {
                $("#loader").css("display", "none")
                UIkit.modal("#ModalSetNumberRoomSell").tryhide()
                $scope.GetDataOnlyRoom($scope.paramsChangeRoomSell.RoomTypeId)
                notify.success("Thành công")
            }, function error(response) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra!")

            });
        }
        $scope.PostPriceRoomSell = function () {
            var fromdate = $("#FromDateChange2").val();
            var todate = $("#ToDateChange2").val();
            if (fromdate == null || fromdate == '' || todate == null || todate == '') {
                notify.error('Ngày tháng không hợp lệ');
                return;
            }
            $("#loader").css("display", "block")
            $scope.paramsChangeRoomSell.FromDate = $scope.FormatDate(fromdate)
            $scope.paramsChangeRoomSell.ToDate = $scope.FormatDate(todate)
            $http({
                url: urlPostPriceRoomSell,
                method: "POST",
                data: $scope.paramsChangeRoomSell
            }).then(function success(response) {
                $("#loader").css("display", "none")
                UIkit.modal("#ModalSetPriceRoomSell").tryhide()
                $scope.GetDataOnlyRoom($scope.paramsChangeRoomSell.RoomTypeId)
                notify.success("Thành công")
            }, function error(response) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra!")

            });
        }
        $scope.ChangeNumberSingle = function (event, roomTypeId, index) {
            if (event.keyCode == 13) {
                $("#loader").css("display", "block")
                var roomtype = $scope.data.find(x => x.RoomTypeId == roomTypeId);
                var rateAvailability = roomtype.RateAvailabilities[index]
                var date = rateAvailability.Date;
                var number = rateAvailability.Number;
                var price = rateAvailability.Price;
                if (status == -1)
                    rateAvailability.Status = -1
                else {
                    if ((number == 0 || price == 0) && status == 1)
                        rateAvailability.Status = 0
                    else if ((number == 0 || price == 0) && status == 2)
                        rateAvailability.Status = -1
                    else
                        rateAvailability.Status = 1
                }
                $scope.paramsChangeRoomSell.FromDate = date
                $scope.paramsChangeRoomSell.ToDate = date
                $scope.paramsChangeRoomSell.RoomTypeId = roomTypeId
                $scope.paramsChangeRoomSell.Number = number
                $http({
                    url: urlPostNumberRoomSell,
                    method: "POST",
                    data: $scope.paramsChangeRoomSell
                }).then(function success(response) {
                    $("#loader").css("display", "none")
                    $scope.GetDataOnlyRoom($scope.paramsChangeRoomSell.RoomTypeId)
                    notify.success("Thành công")
                }, function error(response) {
                    $("#loader").css("display", "none")
                    notify.error("Có lỗi sảy ra!")

                });
                return;
            }
            if (parseInt(event.key).toString() % 1 != 0) {
                event.preventDefault();
            }
        }
        $scope.ChangePriceSingle = function (event, roomTypeId, index) {
            if (event.keyCode == 13) {
                $("#loader").css("display", "block")
                var roomtype = $scope.data.find(x => x.RoomTypeId == roomTypeId);
                var rateAvailability = roomtype.RateAvailabilities[index]
                var date = rateAvailability.Date;
                var number = rateAvailability.Number;
                var price = rateAvailability.Price;
                if (status == -1)
                    rateAvailability.Status = -1
                else {
                    if ((number == 0 || price == 0) && status == 1)
                        rateAvailability.Status = 0
                    else if ((number == 0 || price == 0) && status == 2)
                        rateAvailability.Status = -1
                    else
                        rateAvailability.Status = 1
                }
                $scope.paramsChangeRoomSell.FromDate = date
                $scope.paramsChangeRoomSell.ToDate = date
                $scope.paramsChangeRoomSell.RoomTypeId = roomTypeId
                $scope.paramsChangeRoomSell.Price = price
                $http({
                    url: urlPostPriceRoomSell,
                    method: "POST",
                    data: $scope.paramsChangeRoomSell
                }).then(function success(response) {
                    $("#loader").css("display", "none")
                    $scope.GetData()
                    notify.success("Thành công")
                }, function error(response) {
                    $("#loader").css("display", "none")
                    notify.error("Có lỗi sảy ra")

                });
                return;
            }
            if (event.key != '.' && parseInt(event.key).toString() % 1 != 0) {
                event.preventDefault();
            }
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
            if (fromdate == null || fromdate == '' || ((todate == null || todate == '') && $scope.filters.typeSearch == -1)) {
                alert('Ngày tháng không hợp lệ');
                return;
            }
            $("#loader").css("display", "block")
            $scope.filters.fromDate = $scope.FormatDate(fromdate)
            $scope.filters.toDate = $scope.FormatDate(todate)
            $http({
                url: urlGet,
                method: "GET",
                params: $scope.filters
            }).then(function success(response) {
                $("#loader").css("display", "none")
                $scope.data = JSON.parse(response.data);
                $scope.data.forEach(x => x.checkClose = true);
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.GetDataOnlyRoom = function (id) {
            var fromdate = $("#FromDate").val();
            var todate = $("#ToDate").val();
            if (fromdate == null || fromdate == '' || ((todate == null || todate == '') && $scope.filters.typeSearch == -1)) {
                alert('Ngày tháng không hợp lệ');
                return;
            }
            $("#loader").css("display", "block")
            $http({
                url: urlGetOnlyRoomType,
                method: "GET",
                params: {
                    fromDate: $scope.FormatDate(fromdate),
                    toDate: $scope.FormatDate(todate),
                    roomtypeId: id
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                var data = JSON.parse(response.data);
                var index = $scope.data.findIndex(x => x.RoomTypeId == id);
                $scope.data[index] = data
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.showCloseMultiRoom = function () {
            UIkit.modal("#ModalCloseMultiRoom").show()
        }
        $scope.showOpenMultiRoom = function () {
            UIkit.modal("#ModalOpenMultiRoom").show()
        }
        $scope.CloseMultiRoom = function () {
            var roomIds = [];
            $scope.data.forEach(x => {
                if (x.checkClose) {
                    roomIds.push(x.RoomTypeId)
                }
            });
            var fromdate = $(".FromRangeDate").val();
            var todate = $(".ToRangeDate").val();
            if (fromdate == null || fromdate == '' || todate == null || todate == '') {
                alert('Ngày tháng không hợp lệ');
                return;
            }
            $("#loader").css("display", "block")
            $http({
                url: urlCloseMultiRoom,
                method: "POST",
                data: {
                    roomIds: roomIds,
                    fromDate: $scope.FormatDate(fromdate),
                    toDate: $scope.FormatDate(todate)
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                UIkit.modal("#ModalCloseMultiRoom").tryhide()
                $scope.GetData()
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.OpenMultiRoom = function () {
            var roomIds = [];
            $scope.data.forEach(x => {
                if (x.checkClose) {
                    roomIds.push(x.RoomTypeId)
                }
            });
            var fromdate = $(".FromRangeDate2").val();
            var todate = $(".ToRangeDate2").val();
            if (fromdate == null || fromdate == '' || todate == null || todate == '') {
                alert('Ngày tháng không hợp lệ');
                return;
            }
            $("#loader").css("display", "block")
            $http({
                url: urlOpenMultiRoom,
                method: "POST",
                data: {
                    roomIds: roomIds,
                    fromDate: $scope.FormatDate(fromdate),
                    toDate: $scope.FormatDate(todate)
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                UIkit.modal("#ModalOpenMultiRoom").tryhide()
                $scope.GetData()
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.Init = function () {
            var datenow = new Date()
            var fromdate = datenow.getDate() + "-" + (datenow.getMonth() + 1) + "-" + datenow.getFullYear();
            var newdate = new Date();
            newdate.setDate(datenow.getDate() + 30);
            var todate = newdate.getDate() + "-" + (newdate.getMonth() + 1) + "-" + newdate.getFullYear();
            $("#FromDate").val(fromdate)
            $("#ToDate").val(todate)
            $scope.FromDate = fromdate;
            $scope.ToDate = todate;
            $scope.GetData()
        }
        $scope.Init()
    }])