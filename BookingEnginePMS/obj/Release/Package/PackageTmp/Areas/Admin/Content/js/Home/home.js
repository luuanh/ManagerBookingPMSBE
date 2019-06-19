var urlGet = "/Admin/Home/GetSituationOnDay"
var urlCheckOut = "/Admin/Reservation/CheckOut"
var urlCheckIn = "/Admin/Reservation/CheckIn"
var urlGetRoomAvailableForBooking = "/Admin/Reservation/GetRoomAvailableForBooking"
var urlAssignRoom = "/Admin/Reservation/AssignRoom"
var urlGetNumberFutureRoom = "/Admin/Statistic/GetNumberFutureRoom"


app.controller('controller', ['$scope', '$http', '$timeout', 'notify', 'validate',
    function ($scope, $http, $timeout, notify, validate) {
        $scope.filters = {
            rangeToday: 0
        }
        $scope.totalBookingArrival = 0;
        $scope.totalBookingDeparture = 0;
        $scope.getStatus = function (status) {
            return statusReservation[(status - 0) - 1]
        }
        $scope.getTypeReservation = function (type) {
            return typeReservation[type]
        }
        $scope.GetData = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlGet,
                method: "GET",
                params: {
                    rangeToday: $scope.filters.rangeToday
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                var data = JSON.parse(response.data);
                $scope.totalRoomInHotel = data.totalRoomInHotel
                $scope.numberRoomUsed = data.numberRoomUsed
                $scope.arrivals = data.arrivals
                $scope.departures = data.departures
                $scope.inhouses = data.inhouses
                $scope.stayovers = data.stayovers
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.calTotalBookingArrivalDeparture = function () {
            if ($scope.arrivals == null)
                $scope.totalBookingArrival = 0;
            else
                $scope.totalBookingArrival = $scope.arrivals.length;

            if ($scope.departures == null)
                $scope.totalBookingDeparture = 0;
            else
                $scope.totalBookingDeparture = $scope.departures.length;
        }
        $scope.checkBeforeCheckIn = function (booking) {
            if (booking.RoomId < 0) {
                $("#loader").css("display", "block")
                $http({
                    url: urlGetRoomAvailableForBooking,
                    method: 'GET',
                    params: {
                        roomtypeId: booking.RoomTypeId,
                        fromDate: booking.ArrivalDate,
                        toDate: booking.DepartureDate,
                        typeBooking: booking.TypeBooking
                    }
                }).then(function success(respone) {
                    $("#loader").css("display", "none")
                    var data = respone.data;
                    $scope.RoomAvailable = data
                    $scope.bookingdetail = {
                        BookingId: booking.BookingId,
                        ArrivalDate: booking.ArrivalDate,
                        DepartureDate: booking.DepartureDate,
                        RoomTypeId: booking.RoomTypeId,
                        TypeBooking: booking.TypeBooking
                    }
                    $scope.roomIdCurrent = -1
                    UIkit.modal("#ModalAssignRoom").show()
                }, function error(respone) {
                    $("#loader").css("display", "none")
                });
            }
            else {
                $scope.CheckIn(booking.BookingId)
            }
        }
        $scope.AssignRoom = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlAssignRoom,
                method: 'POST',
                data: {
                    bookingId: $scope.bookingdetail.BookingId,
                    roomtypeId: $scope.bookingdetail.RoomTypeId,
                    roomId: $scope.roomIdCurrent
                }
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                notify.success("Thành công")
                UIkit.modal("#ModalAssignRoom").tryhide()
                $scope.GetData()
            }, function error(respone) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra")
            });
        }
        $scope.CheckIn = function (bookingId) {
            $("#loader").css("display", "block")
            $http({
                url: urlCheckIn,
                method: 'POST',
                data: {
                    bookingId: bookingId
                }
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                notify.success("Thành công")
                $scope.GetData()
            }, function error(respone) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra")
            });
        }
        $scope.CheckOut = function (bookingId, totalAmount) {
            $("#loader").css("display", "block")
            $http({
                url: urlCheckOut,
                method: 'POST',
                data: {
                    bookingId: bookingId,
                    totalAmount: totalAmount
                }
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                notify.success("Thành công")
                $scope.GetData()
            }, function error(respone) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra")
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
        $scope.GetNumberFutureRoom = function () {
            var fromDate = $("#FromDate_Step1").val();
            var toDate = $("#ToDate_Step1").val();
            var v1 = validate.isNotDate({ value: fromDate, key: 'fromDate' })
            var v2 = validate.isNotDate({ value: toDate, key: 'toDate' })
            if (v1 || v2) {
                notify.error('Date value is invalid!')
                return
            }
            $http({
                url: urlGetNumberFutureRoom,
                method: 'GET',
                params: {
                    fromDate: $scope.FormatDate(fromDate),
                    toDate: $scope.FormatDate(toDate)
                }
            }).then(function success(respone) {
                var data = JSON.parse(respone.data);
                $scope.dataNumberFutureRoom = data;
                console.log(data)
            }, function error(respone) {
            });
        }
        $scope.RefreshValidate = function () {
            $(".error").css("display", "none")
        }
        $scope.init = function () {
            var datenow = new Date()
            datenow.setDate(datenow.getDate());
            var fromdate = datenow.getDate() + "-" + (datenow.getMonth() + 1) + "-" + datenow.getFullYear();
            var newdate = new Date();
            newdate.setDate(datenow.getDate() + 14);
            var todate = newdate.getDate() + "-" + (newdate.getMonth() + 1) + "-" + newdate.getFullYear();
            $("#FromDate_Step1").val(fromdate)
            $("#ToDate_Step1").val(todate)
            $scope.FromDate = fromdate;
            $scope.ToDate = todate;
            $scope.GetNumberFutureRoom();
        }
        $scope.init()
        $scope.GetData()
    }]);