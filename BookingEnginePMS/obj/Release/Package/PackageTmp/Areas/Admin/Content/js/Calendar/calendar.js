var urlGet = "/Admin/Calendar/Get"
var urlGetOverBooking = "/Admin/Calendar/GetOverBooking"
var urlAssignRoom = "/Admin/Calendar/AssignRoom"
var urlNewReservation = "/Admin/Calendar/NewReservation"

var urlCheckIn = "/Admin/Reservation/CheckIn"
var urlCheckOut = "/Admin/Reservation/CheckOut"


app.controller('controller', ['$scope', '$http', 'notify', '$timeout', 'helper',
    function ($scope, $http, notify, $timeout, helper) {
        $scope.filters = {
            fromDate: '',
            numberDay: 20,
            changeStartDate: false
        }
        $scope.typeColorBooking = 1
        $scope.data = [];
        $scope.totalDay = []
        $scope.dateObjList = []
        $scope.FormatDate = function (date) {
            date = date.trim()
            var dt = date.split('-');
            var d = dt[0];
            dt[0] = dt[2];
            dt[2] = d;
            return dt.join('-')
        }
        $scope.setTotalDay = function () {
            $scope.totalDay = []
            for (var i = 0; i <= $scope.filters.numberDay; i++) {
                $scope.totalDay.push(i)
            }
        }
        $scope.GetData = function () {
            var fromdate = $("#FromDate").val();
            if (fromdate == null || fromdate == '') {
                alert('Ngày tháng không hợp lệ');
                return;
            }
            $("#loader").css("display", "block")
            $scope.setTotalDay()
            $scope.filters.fromDate = $scope.FormatDate(fromdate)
            $http({
                url: urlGet,
                method: "GET",
                params: $scope.filters
            }).then(function success(response) {
                $("#loader").css("display", "none")
                $scope.data = JSON.parse(response.data);
                $scope.getDateObjList($scope.data.dateTimes)
                $scope.data.modelDataCalendars.forEach(x => {
                    if (x.typeTitle == 2) {
                        var index = $scope.arrRoomTypeCLose.findIndex(y => y == x.roomType.RoomTypeId)
                        x.show = index < 0;
                    }
                })
                $scope.hidenBoxInfoReservation()
                if ($scope.data.bookings == null)
                    $scope.data.bookings = []
                $timeout(function () {
                    $scope.drawBookingExistController($scope.data.bookings)
                }, 100)
                //$scope.clearCellAddNewReservation()
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.getDateObjList = function (dates) {
            $scope.dateObjList = [];
            dates.forEach(x => {
                $scope.dateObjList.push(helper.getArrDate(x))
            })
        }
        $scope.Init = function () {
            var datenow = new Date()
            var fromdate = datenow.getDate() + "-" + (datenow.getMonth() + 1) + "-" + datenow.getFullYear();
            $("#FromDate").val(fromdate)
            $scope.FromDate = fromdate;
            $scope.GetData()
        }
        $scope.Init()
        $scope.GetDataToday = function () {
            $scope.Init()
        }
        $scope.ChangeRangeDay = function (type) {
            var fromdate = $("#FromDate").val();
            if (fromdate == null || fromdate == '') {
                alert('Ngày tháng không hợp lệ');
                return;
            }
            var arrd = fromdate.split('-');
            var dateCurrent = new Date(arrd[2], arrd[1] - 1, arrd[0])
            var dateNext = dateCurrent;
            if (type < 0)
                dateNext.setDate(dateCurrent.getDate() - $scope.filters.numberDay);
            else {
                dateNext.setDate((dateCurrent.getDate() - 0) + $scope.filters.numberDay);
            }
            var fromdateNew = dateNext.getDate() + "-" + (dateNext.getMonth() + 1) + "-" + dateNext.getFullYear();
            $("#FromDate").val(fromdateNew)
            $scope.GetData()
        }
        $scope.ChangeColorBooking = function () {
            $scope.drawBookingExistController($scope.data.bookings)
        }
        // draw booking exist
        $scope.drawBookingExistController = function (bookings) {
            $('.cell-booking-exist').detach();
            bookings.forEach(x => {
                $scope.drawBooking(x)
            })
        }
        $scope.checkDateAcceptShow = function (date) {
            var fromdate = $("#FromDate").val();
            var arrDate = fromdate.split('-');
            if (parseInt(date.year) < parseInt(arrDate[2]) ||
                parseInt(date.month) < parseInt(arrDate[1]) ||
                (
                    parseInt(date.day) < parseInt(arrDate[0]) &&
                    parseInt(date.month) == parseInt(arrDate[1])
                ))
                return true
            return false;
        }
        $scope.drawBooking = function (booking) {
            var dateArrival = helper.getArrDate(booking.ArrivalDate)
            var dateDeparture = helper.getArrDate(booking.DepartureDate)
            var indexStart = $scope.dateObjList.findIndex(x => x.day == dateArrival.day && x.month == dateArrival.month && x.year == dateArrival.year)
            var indexEnd = $scope.dateObjList.findIndex(x => x.day == dateDeparture.day && x.month == dateDeparture.month && x.year == dateDeparture.year)
            var minLeft = 121;
            var maxRight = $("#column_" + ($scope.dateObjList.length - 1))[0].offsetLeft + $("#column_0")[0].offsetWidth;
            var left = minLeft;
            if (indexStart >= 0) {
                left = $("#column_" + indexStart)[0].offsetLeft
                left += $("#column_0")[0].offsetWidth * dateArrival.hour / 24
            }
            else if (!$scope.checkDateAcceptShow(dateArrival)) {
                return;
            }
            if (left > maxRight)
                left = maxRight

            var right = maxRight
            if (indexEnd >= 0) {
                right = $("#column_" + indexEnd)[0].offsetLeft
                right += $("#column_0")[0].offsetWidth * dateDeparture.hour / 24
            }
            if (right > maxRight)
                right = maxRight
            var parentId = "row_" + booking.RoomId
            var color = $scope.getColorStatusBooking(booking.Status)
            if ($scope.typeColorBooking == 0) {
                color = booking.Color
            }
            var note = booking.GuestName;
            if (booking.ReminiscentName != null && booking.ReminiscentName != '')
                note = booking.ReminiscentName
            var fixWidth = booking.TypeBooking == 1 && booking.Status != 3
            var markNote = (booking.Note != null && booking.Note.length > 0) || (booking.reservationNotes != null && booking.reservationNotes.length > 0)
            $scope.addItemExistToParent(booking.BookingId, booking.TypeBooking, parentId, left, right - left, color,
                "#" + booking.ReservationId + " " + note,
                "#" + booking.ReservationId + " - " + booking.BookingId, markNote, fixWidth)
        }
        $scope.getColorStatusBooking = function (status) {
            return statusBooking[status - 1];
        }
        $scope.addItemExistToParent = function (bookingId, typeBooking, parentId, startPoint, width, color, note, title, markNote, fixWidth) {
            if (fixWidth || width < 0)
                width = $("#column_0")[0].offsetWidth / 4
            var addition = '';
            if (markNote)
                addition = addition + '<span class="mark-note"></span>'
            if (typeBooking)
                addition = addition + '<span class="mark-clock uk-icon-clock-o"></span>'
            $("#" + parentId).append('<td title="' + title + '" bookingId="' + bookingId + '" onmouseover="overCellBooking(this)" onmouseout="outCellBooking()" onclick="fixedItemLocation()" class="cell-booking-exist" style="background:' + color + ';width:' + width + 'px;left:' + startPoint + 'px;"><div style="overflow:hidden;height:100%;"><span class="note-in-cell">' + note + '</span></div>' + addition + '</td>');
        }
        // event draw calendar
        $scope.continuesDraw = false;
        $scope.allCellBooking = []
        $scope.pointStart = {}
        $scope.pointEnd = {}
        $scope.acceptBookByHour = 0
        $scope.arrivalDateBook = '';
        $scope.departurDateBook = ''
        $scope.roomTypeIdChooseBook = -1
        $scope.roomidChooseBook = -1
        $scope.roomCodeChooseBook = ''
        $scope.clickBook = function (roomid, index, roomCode = '') {
            $scope.hidenBoxInfoReservation()
            $scope.continuesDraw = !$scope.continuesDraw
            if ($scope.continuesDraw) {
                $scope.pointStart = {
                    roomid: roomid,
                    index: index
                }
                $scope.clearCellAddNewReservation()
            }
            $("#" + roomid + "_" + index).addClass("click-book-active")
            if (!$scope.continuesDraw) {
                $scope.pointEnd = {
                    roomid: roomid,
                    index: index
                }
                $scope.allCellBooking.push({
                    pointStart: JSON.parse(JSON.stringify($scope.pointStart)),
                    pointEnd: JSON.parse(JSON.stringify($scope.pointEnd))
                })
                $(".cell-room").removeClass("click-book-active")
                if ($scope.pointStart.roomid != $scope.pointEnd.roomid)
                    return;
                $scope.acceptBookByHour = $scope.pointStart.index == $scope.pointEnd.index ? 1 : 0
                if ($scope.pointStart.index < $scope.pointEnd.index) {
                    $scope.arrivalDateBook = $scope.data.dateTimes[$scope.pointStart.index]
                    $scope.departurDateBook = $scope.data.dateTimes[$scope.pointEnd.index]
                }
                else {
                    $scope.arrivalDateBook = $scope.data.dateTimes[$scope.pointEnd.index]
                    $scope.departurDateBook = $scope.data.dateTimes[$scope.pointStart.index]
                }
                $scope.roomidChooseBook = roomid
                $scope.roomTypeIdChooseBook = $("#row_" + roomid).attr("class").split('_').reverse()[0]
                $scope.roomCodeChooseBook = roomCode
                $scope.tryDrawToBook()
            }
        }
        $scope.tryDrawToBook = function () {
            var idStart = $scope.pointStart.roomid + "_" + $scope.pointStart.index;
            var idEnd = $scope.pointEnd.roomid + "_" + $scope.pointEnd.index;
            var idParent = $("#" + idStart).parent().attr('id')
            var widthStr = $("#" + idStart).width()
            var widthEnd = $("#" + idEnd).width()
            var pStr = $("#" + idStart)[0].offsetLeft
            var pEnd = $("#" + idEnd)[0].offsetLeft
            if (pEnd > pStr) {
                pStr += widthStr * 0.6
                pEnd += widthEnd / 2
                $scope.addItemToParent(idParent, pStr, pEnd - pStr)
            }
            else {
                pStr += widthStr / 2
                pEnd += widthEnd * 0.6
                $scope.addItemToParent(idParent, pEnd, pStr - pEnd)
            }
        }
        $scope.autoRawWhenResponesive = function () {
            $('.cell-booking').detach();
            $scope.drawBookingExistController($scope.data.bookings)
            $scope.allCellBooking.forEach(x => {
                var idStart = x.pointStart.roomid + "_" + x.pointStart.index;
                var idEnd = x.pointEnd.roomid + "_" + x.pointEnd.index;
                var idParent = $("#" + idStart).parent().attr('id')
                var widthStr = $("#" + idStart).width()
                var widthEnd = $("#" + idEnd).width()
                var pStr = $("#" + idStart)[0].offsetLeft
                var pEnd = $("#" + idEnd)[0].offsetLeft
                if (pEnd > pStr) {
                    pStr += widthStr * 0.6
                    pEnd += widthEnd / 2
                    $scope.addItemToParent(idParent, pStr, pEnd - pStr)
                }
                else {
                    pStr += widthStr / 2
                    pEnd += widthEnd * 0.6
                    $scope.addItemToParent(idParent, pEnd, pStr - pEnd)
                }
            })
        }
        $scope.addItemToParent = function (parentId, startPoint, width) {
            $("#" + parentId).append('<td class="cell-booking" style="width:' + width + 'px;left:' + startPoint + 'px;"></td>');
            $scope.addBoxAddNewBooking(parentId, startPoint + width / 2)
        }
        $scope.clearCellAddNewReservation = function () {
            $('.cell-booking').detach();
            $scope.allCellBooking = [];
            $("#box-add-new-booking").css("display", "none")
        }

        $scope.addBoxAddNewBooking = function (parentId, startPoint) {
            var indexCenterScreen = parseInt($scope.data.dateTimes.length / 2);
            var locationLeft = $("#column_" + indexCenterScreen)[0].offsetLeft
            var top = $("#" + parentId)[0].offsetTop
            $("#box-add-new-booking").css("top", top - 120)
            if (startPoint < locationLeft) {
                $("#box-add-new-booking").css("left", startPoint)
                $("#box-add-new-booking .point-bottom").css("right", "auto")
            }
            else {
                $("#box-add-new-booking").css("left", startPoint - 280)
                $("#box-add-new-booking .point-bottom").css("right", 20)
            }
            $("#box-add-new-booking").css("display", "block")
        }
        $scope.hidenBoxAddNewReservation = function () {
            $("#box-add-new-booking").css("display", "none")
            $scope.clearCellAddNewReservation();
            $scope.continuesDraw = false;
        }
        $scope.hidenBoxInfoReservation = function () {
            $("#box-info-booking").css("display", "none")
        }
        $scope.showBoxDetail = function () {
            $scope.hidenBoxAddNewReservation()
            var bookingId = $("#locationShowBoxInfor").val()
            $scope.booking = $scope.data.bookings.find(x => x.BookingId == bookingId);
            var indexCenterScreen = parseInt($scope.data.dateTimes.length / 2);
            var locationLeft = $("#column_" + indexCenterScreen)[0].offsetLeft
            var locationLeftOld = parseInt($("#box-info-booking").css("left"))
            var locationTop = parseInt($("#box-info-booking").css("top"))
            var width = $("#locationShowBoxInfor").attr("attWidth")
            if (locationLeft < locationLeftOld) {

                $("#box-info-booking").css("left", locationLeftOld - 380)
                $("#box-info-booking .point-bottom").css("right", 20)
                if ($("#locationTop").val() == 1) {
                    var left = $("#locationShowBoxInfor").attr("attLeft")
                    $("#box-info-booking").css("left", left - 410)
                    $("#point_bottom").addClass("point-bottom-right")
                    $("#point_bottom").removeClass("point-bottom-left")
                    $("#point_bottom").removeClass("point-bottom")
                    $("#point_bottom").removeAttr("style")
                }
            }
            else {
                $("#box-info-booking .point-bottom").css("right", "auto")
            }
            $("#box-info-booking").css("display", "block")
        }

        //#region control
        $scope.QuickCheckInBooking = function (id) {
            $("#loader").css("display", "block")
            $http({
                url: urlCheckIn,
                method: 'POST',
                data: {
                    bookingId: id
                }
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                notify.success("Thành công")
                $scope.GetData()
            }, function error(respone) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra!")
            });
        }
        $scope.confirmCheckOut = function (id) {
            $scope.bookingIdChoose = id
            UIkit.modal("#ModalConfirmCheckOut").show()
        }
        $scope.QuickCheckOutBooking = function () {
            $("#loader").css("display", "block")
            var booking = $scope.data.bookings.find(x => x.BookingId == $scope.bookingIdChoose);
            $http({
                url: urlCheckOut,
                method: 'POST',
                data: {
                    bookingId: $scope.bookingIdChoose,
                    totalAmount: booking.Total
                }
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                notify.success("Thành công")
                UIkit.modal("#ModalConfirmCheckOut").tryhide()
                $scope.GetData()
            }, function error(respone) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra")
            });
        }
        $scope.showOverBooking = function (index, number = 0) {
            if (number == 0)
                return;
            var date = $scope.data.dateTimes[index]
            $("#loader").css("display", "block")
            $http({
                url: urlGetOverBooking,
                method: "GET",
                params: {
                    date: date
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                var data = JSON.parse(response.data)
                $scope.dataOverBooking = data
                UIkit.modal("#ModalOverBooking").show()
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.changeRoomTypeForAssign = function (booking) {
            booking.roomCalendars = booking.roomTypeCalendars.find(x => x.RoomTypeId == booking.RoomTypeId).roomCalendars
            $scope.changeAssignRoom()
        }
        $scope.changeAssignRoom = function () {
            $scope.dataOverBooking.forEach(x => {
                var arrOldRooms = x.roomTypeCalendars.find(y => y.RoomTypeId == x.RoomTypeId).roomCalendars
                x.roomCalendars = JSON.parse(JSON.stringify(arrOldRooms))
            })
            $scope.dataOverBooking.forEach(x => {
                if (x.RoomId > 0) {
                    var otherBooking = $scope.dataOverBooking.filter(y => y.BookingId != x.BookingId);
                    otherBooking.forEach(y => {
                        var indexRoomExist = y.roomCalendars.findIndex(z => z.RoomId == x.RoomId);
                        if (indexRoomExist >= 0)
                            y.roomCalendars.splice(indexRoomExist, 1)
                    });
                }
            })
        }
        $scope.AssignRoom = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlAssignRoom,
                method: 'POST',
                data: $scope.dataOverBooking
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                notify.success("Thành công")
                UIkit.modal("#ModalOverBooking").tryhide()
                $scope.GetData()
            }, function error(respone) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra!")
            });
        }

        $scope.CreateReservationHour = function () {
            var params = 'checkin=' + $scope.arrivalDateBook.split('T')[0] +
                '&checkout=' + $scope.departurDateBook.split('T')[0] +
                '&typeBooking=' + $scope.acceptBookByHour +
                '&roomTypeId=' + parseInt($scope.roomTypeIdChooseBook) +
                '&roomId=' + $scope.roomidChooseBook
            location.href = urlNewReservation + "?" + params;
        }
        $scope.CreateReservationDay = function () {
            var params = 'checkin=' + $scope.arrivalDateBook.split('T')[0] +
                '&checkout=' + $scope.departurDateBook.split('T')[0] +
                '&typeBooking=' + $scope.acceptBookByHour +
                '&roomTypeId=' + parseInt($scope.roomTypeIdChooseBook) +
                '&roomId=' + $scope.roomidChooseBook
            location.href = urlNewReservation + "?" + params;
        }
        //#endregion        //#region control calendar
        $scope.arrRoomTypeCLose = []
        $scope.showHideGroupRoom = function (item) {
            item.show = !item.show
            if (item.show)
                $(".rowOfRoomType_3_" + item.roomType.RoomTypeId).css("display", "table-row")
            else {
                $(".rowOfRoomType_3_" + item.roomType.RoomTypeId).css("display", "none")
                var index = $scope.arrRoomTypeCLose.findIndex(x => x == item.roomType.RoomTypeId)
                if (index < 0)
                    $scope.arrRoomTypeCLose.push(item.roomType.RoomTypeId)
            }
        }
        //#endregion

    }])