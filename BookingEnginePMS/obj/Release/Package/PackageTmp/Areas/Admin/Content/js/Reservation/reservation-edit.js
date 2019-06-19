var urlDetail = "/Admin/Reservation/Detail"
var urlGetBooking = "/Admin/Reservation/GetBooking"
var urlPostNote = "/Admin/Reservation/PostNote"
var urlPutGeneralInfor = "/Admin/Reservation/PutGeneralInfor"
var urlDetailGuest = "/Admin/Guest/Detail"
var urlDetailAgency = "/Admin/Company/Detail"
var urlPutService = "/Admin/Reservation/PutService"
// service

var urlGetServiceCategory = "/Admin/ServiceCategory/GetFull"
var urlGetServices = "/Admin/Service/GetAll"

// Booking Detail
var urlGetBookingDetail = "/Admin/Reservation/GetBookingDetail"
var urlPutGuestBooking = "/Admin/Reservation/PutGuestBooking"
var urlPutVisaBooking = "/Admin/Reservation/PutVisaBooking"
var urlPutCardBooking = "/Admin/Reservation/PutCardBooking"
var urlDeleteCardBooking = "/Admin/Reservation/DeleteCardBooking"
var urlPutBooking = "/Admin/Reservation/PutBooking"
var urlGetRoomAvailableForBooking = "/Admin/Reservation/GetRoomAvailableForBooking"
var urlAssignRoom = "/Admin/Reservation/AssignRoom"
var urlCheckIn = "/Admin/Reservation/CheckIn"
var urlUndoCheckIn = "/Admin/Reservation/UndoCheckIn"
var urlNoShow = "/Admin/Reservation/NoShow"
var urlCancel = "/Admin/Reservation/Cancel"
var urlCheckOut = "/Admin/Reservation/CheckOut"
var urlTransferDebt = "/Admin/Reservation/TransferDebt"
var urlGetPriceRoomByRangeDate = "/Admin/Reservation/GetPriceRoomByRangeDate"
var urlChangeStay = "/Admin/Reservation/ChangeStay"
var urlPayBooking = "/Admin/Reservation/PayBooking"

// invoice

var urlOpenInvoice = "/Admin/Reservation/OpenInvoice"
var urlCalculatorRoomPrice = "/Admin/Reservation/CalculatorRoomPrice"
var urlGetRoomAvailability = "/Admin/Reservation/GetRoomAvailability"
var urlPostAddBooking = "/Admin/Reservation/PostAddBooking"

// group action
var urlAutoAssignRoom = "/Admin/Reservation/AutoAssignRoom"
var urlGetBookingNeedToCheckIn = "/Admin/Reservation/GetBookingNeedToCheckIn"
var urlPostBookingNeedToCheckIn = "/Admin/Reservation/PostBookingNeedToCheckIn"
var urlPostBookingNeedToCheckOut = "/Admin/Reservation/PostBookingNeedToCheckOut"
var urlGetReservationForClusters = "/Admin/Reservation/GetReservationForClusters"
var urlPostGroupClusters = "/Admin/Reservation/PostGroupClusters"

app.controller('controller', ['$scope', '$http', '$log', 'uiGridConstants', 'validate', 'notify', 'helper', '$timeout', '$interval',
    function ($scope, $http, $log, uiGridConstants, validate, notify, helper, $timeout, $interval) {
        //#region general
        $scope.filters = {
            status: -1
        }
        $scope.data = {}
        $scope.getStatus = function (status) {
            return statusReservation[(status - 0) - 1]
        }
        $scope.bookingdetail = {
            BookingServices: [],
            BookingExtrabeds: []
        }
        $scope.emailChoose = {}
        $scope.getSource = function (source) {
            return sourceReservation[(source - 0) - 1]
        }
        $scope.getTypeReservation = function (type) {
            return typeReservation[type]
        }
        $scope.getNumberBookingForFilter = function () {
            $scope.numberBookingNew = $scope.getNumberBookingByStatus(1)
            $scope.numberBookingInHouse = $scope.getNumberBookingByStatus(2)
            $scope.numberBookingCheckOut = $scope.getNumberBookingByStatus(3)
            $scope.numberBookingNoShow = $scope.getNumberBookingByStatus(4)
            $scope.numberBookingCancel = $scope.getNumberBookingByStatus(5)
            $scope.numberBookingAll = $scope.getNumberBookingByStatus(-1)
        }
        $scope.showMoreGuestInformation = false;
        $scope.showMoreAgencyInformationModel = false;
        $scope.showMoreInformation = function () {
            $scope.showMoreGuestInformation = !$scope.showMoreGuestInformation
        }
        $scope.showMoreAgencyInformation = function () {
            $scope.showMoreAgencyInformationModel = !$scope.showMoreAgencyInformationModel
        }
        $scope.getNumberBookingByStatus = function (status) {
            if ($scope.bookings != null && $scope.bookings.length > 0) {
                if (status < 0)
                    return $scope.bookings.length;
                else
                    return $scope.bookings.filter(x => x.Status == status).length
            }
            return 0;
        }
        $scope.GetInfoGuest_AfterAutoComplete = function () {
            var result = $("#result_SearchAuto_Guest").val().split('-')
            $("#loader").css("display", "block")
            $http({
                method: 'GET',
                url: urlDetailGuest,
                params: {
                    id: result[2].trim()
                }
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                $scope.guest = JSON.parse(respone.data).guest
                if ($scope.guest.Dob != null)
                    $scope.Dob = $scope.FormatDate($scope.guest.Dob.split('T')[0])
                else
                    $scope.Dob = ''
                $scope.data.Guest = $scope.guest;
            }, function error(respone) {
                $("#loader").css("display", "none")
            })
        }
        $scope.GetInfoAgency_AfterAutoComplete = function () {
            var result = $("#result_SearchAuto_Agency").val().split('-')
            $("#loader").css("display", "block")
            $http({
                method: 'GET',
                url: urlDetailAgency,
                params: {
                    id: result[2].trim()
                }
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                $scope.data.Company = respone.data
            }, function error(respone) {
                $("#loader").css("display", "none")
            })
        }
        $scope.showDetailEmailSent = function (reservationEmailSentId) {
            window.open('/Admin/Print/EmailSent?id=' + reservationEmailSentId, '_blank');
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
            var id = helper.findGetParameter('id')
            $http({
                url: urlDetail,
                method: "GET",
                params: {
                    id: id
                }
            }).then(function success(response) {
                var data = JSON.parse(response.data)
                $scope.superData = data;
                $scope.data = data.reservation
                $scope.reservationNote = data.reservationNote
                $scope.reservationEmailSents = data.reservationEmailSents
                $scope.GetServiceExist(data)
            }, function error(response) {
            });
        }
        $scope.PostNote = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlPostNote,
                method: "POST",
                data: {
                    id: $scope.data.ReservationId,
                    note: $("#txtNote").val()
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                notify.success("Thành công")
                $scope.GetData()
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.PutGeneralInfor = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlPutGeneralInfor,
                method: "POST",
                data: $scope.data
            }).then(function success(response) {
                $("#loader").css("display", "none")
                notify.success("Thành công")
                $scope.GetData()
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.PutService = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlPutService,
                method: "POST",
                data: {
                    id: $scope.data.ReservationId,
                    bookingServices: $scope.serviceChoose
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                notify.success("Thành công")
                $scope.GetData()
                $scope.GetBooking()
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.GetBooking = function () {
            $("#loader").css("display", "block")
            var id = helper.findGetParameter('id')
            $scope.ReservationId = id
            $http({
                url: urlGetBooking,
                method: "GET",
                params: {
                    id: id,
                    status: $scope.filters.status
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                var data = JSON.parse(response.data)
                $scope.bookings = data
                $scope.getNumberBookingForFilter()
                $scope.GetRoomTypeInRoomChoose(data)
            }, function error(response) {
                $("#loader").css("display", "none")

            });
        }
        $scope.GetData()
        $scope.GetBooking()
        //#endregion
        //#region service
        $scope.serviceChoose = [];
        $scope.services = [];
        $scope.roomtypeForService = []
        $scope.GetServiceExist = function (data) {
            $scope.serviceChoose = [];
            data.reservationServices.forEach(x => {
                $scope.serviceChoose.push({
                    ServiceId: x.ServiceId,
                    Number: x.Number,
                    ServiceName: x.ServiceName,
                    Price: x.Price,
                    BookingId: -1
                })
            })
            data.bookingServices.forEach(x => {
                $scope.serviceChoose.push({
                    ServiceId: x.ServiceId,
                    Number: x.Number,
                    ServiceName: x.ServiceName,
                    Price: x.Price,
                    BookingId: x.BookingId
                })
            })
        }
        $scope.GetRoomTypeInRoomChoose = function (data) {
            $scope.roomtypeForService = [];
            data.forEach(x => {
                $scope.roomtypeForService.push({
                    Text: x.BookingId + ' (' + x.RoomTypeName + ')',
                    BookingId: x.BookingId
                })
            })
        }
        $scope.sumServicePrice = function () {
            $scope.TotalService = 0;
            if ($scope.serviceChoose != null && $scope.serviceChoose.length > 0) {
                $scope.serviceChoose.forEach(x => {
                    $scope.TotalService = ($scope.TotalService - 0) + x.Number * (x.Price - 0)
                })
            }
        }

        $scope.changeTypeServiceCategory = function (serviceCategoryId) {
            $(".box-service-category > div").removeClass('selected-category')
            $(".box_serviceCategory_" + serviceCategoryId).addClass('selected-category')
            $scope.GetService(serviceCategoryId)
        }
        $scope.GetServiceCategory = function () {
            $http({
                url: urlGetServiceCategory,
                method: "GET"
            }).then(function success(response) {
                $scope.ServiceCategories = response.data
                if ($scope.ServiceCategories != null && $scope.ServiceCategories.length > 0) {
                    $scope.changeTypeServiceCategory($scope.ServiceCategories[0].ServiceCategoryId)
                }
            }, function error(response) {
            });
        }
        $scope.GetService = function (serviceCategoryId) {
            $("#loader").css("display", "block")
            $http({
                url: urlGetServices,
                method: "GET",
                params: {
                    serviceCategoryId: serviceCategoryId,
                    voucher: $scope.data.Voucher
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                $scope.services = response.data
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.addService = function (item) {
            var itemClone = JSON.parse(JSON.stringify(item));
            itemClone.Number = 1;
            itemClone.Paid = false;
            itemClone.BookingId = -1
            $scope.serviceChoose.unshift(itemClone)
        }
        $scope.removeService = function (index) {
            $scope.serviceChoose.splice(index, 1)
        }
        $scope.GetServiceCategory();
        //#endregion
        //#region Edit Booking
        $scope.IncludeVATAndServiceCharge = true
        $scope.masterData = {
            TotalDiscount: 0
        }
        $scope.showDetailPaid = false;
        $scope.showhideDetailPaid = function () {
            $scope.showDetailPaid = !$scope.showDetailPaid
        }
        $scope.EditBooking = function (id) {
            $scope.showInvoice = false;
            $("#loader").css("display", "block")
            $http({
                url: urlGetBookingDetail,
                method: 'GET',
                params: {
                    id: id
                }
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                $("#kUI_window").data("kendoWindow").maximize().open();
                $scope.masterData = JSON.parse(respone.data)
                $scope.bookingdetail = $scope.masterData.booking
                $scope.taxFees = ($scope.masterData.taxFees[0] - 0) + (($scope.masterData.taxFees[1] - 0))
                console.log('-------')
                console.log($scope.taxFees)
                console.log('-------')
                if ($scope.bookingdetail.BookingServices != null && $scope.bookingdetail.BookingServices.length > 0) {
                    for (var i = 0; i < $scope.bookingdetail.BookingServices.length; i++) {
                        var date = $scope.FormatDate($scope.bookingdetail.BookingServices[i].DateCreate.split('T')[0]);
                        $scope.bookingdetail.BookingServices[i].DateUser = date
                        $("#DateUse_Clone_" + i).val(date)
                    }
                }
                if ($scope.bookingdetail.BookingExtrabeds != null && $scope.bookingdetail.BookingExtrabeds.length > 0) {
                    for (var i = 0; i < $scope.bookingdetail.BookingExtrabeds.length; i++) {
                        var date2 = $scope.FormatDate($scope.bookingdetail.BookingExtrabeds[i].DateCreate.split('T')[0]);
                        $scope.bookingdetail.BookingExtrabeds[i].DateUser = date2
                        $("#DateUse_Extrabed_" + i).val(date)
                    }
                }
            }, function error(respone) {
                $("#loader").css("display", "none")

            });
        }
        $scope.PutVisaBooking = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlPutVisaBooking,
                method: 'POST',
                data: {
                    visa: $scope.bookingdetail.VisaBookings,
                    bookingId: $scope.bookingdetail.BookingId
                }
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                notify.success("Thành công")
            }, function error(respone) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra!")
            });
        }
        $scope.PutCardBooking = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlPutCardBooking,
                method: 'POST',
                data: {
                    card: $scope.bookingdetail.CardBooking,
                    bookingId: $scope.bookingdetail.BookingId
                }
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                notify.success("Thành công")
            }, function error(respone) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra!")
            });
        }
        $scope.ConfirmDeleteCardBooking = function () {
            UIkit.modal("#ModalDeleteInforCard").show()
        }
        $scope.DeleteCardBooking = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlDeleteCardBooking,
                method: 'POST',
                data: {
                    bookingId: $scope.bookingdetail.BookingId
                }
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                $scope.bookingdetail.CardBooking = {
                    ExpirationMonth: 1,
                    ExpirationYear: (new Date()).getFullYear()
                }
                notify.success("Thành công")
                UIkit.modal("#ModalDeleteInforCard").tryhide()
            }, function error(respone) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra!")
            });
        }
        $scope.PutGuestBooking = function () {
            var v1 = validate.isNullOrEmptySingleShowError({ value: $scope.bookingdetail.Guest.FirstName, key: 'FirstName_D' })
            var v2 = validate.isNullOrEmptySingleShowError({ value: $scope.bookingdetail.Guest.SurName, key: 'SurName_D' })
            var v4 = validate.isNullOrEmptySingleShowError({ value: $scope.bookingdetail.Guest.Phone, key: 'Phone_D' })
            if (v1 || v2 || v4) {
                alert("Guest is invalid")
                return true
            }
            $("#loader").css("display", "block")
            if ($("#DobBookingDetail").val() != '') {
                var datetime = $("#DobBookingDetail").val();
                $scope.bookingdetail.Guest.Dob = $scope.FormatDate(datetime)
            }
            $http({
                url: urlPutGuestBooking,
                method: 'POST',
                data: {
                    guest: $scope.bookingdetail.Guest,
                    bookingId: $scope.bookingdetail.BookingId
                }
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                notify.success("Thành công")
            }, function error(respone) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra!")

            });
        }
        $scope.PutBooking = function () {
            $("#loader").css("display", "block")
            $scope.dataPost = JSON.parse(JSON.stringify($scope.bookingdetail));
            $scope.dataPost.CardBooking = {}
            $scope.dataPost.Guest = {}
            $scope.dataPost.VisaBookings = {}
            $http({
                url: urlPutBooking,
                method: 'POST',
                data: $scope.dataPost
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                notify.success("Thành công")
                $scope.EditBooking($scope.bookingdetail.BookingId)
                $scope.GetBooking()
            }, function error(respone) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra!")
            });
        }
        $scope.showScheduleExtrabed = function () {
            $scope.scheduleExtrabed = $scope.masterData.Extrabeds;
            UIkit.modal("#ModalScheduleExtrabed").show()
        }
        $scope.showScheduleService = function () {
            UIkit.modal("#ModalScheduleService").show()
        }
        $scope.addExtrabedForBooking = function (item) {
            if (item.Number > 0) {
                if ($scope.bookingdetail.BookingExtrabeds != null && $scope.bookingdetail.BookingExtrabeds.length > 0) {
                    for (var i = 0; i < $scope.bookingdetail.BookingExtrabeds.length; i++) {
                        $scope.bookingdetail.BookingExtrabeds[i].DateUser = $("#DateUse_Extrabed_" + i).val()
                    }
                }
                var data = JSON.parse(JSON.stringify(item))
                data.Paid = false;
                data.DateUser = helper.getDateNow()
                if ($scope.bookingdetail.BookingExtrabeds == null)
                    $scope.bookingdetail.BookingExtrabeds = [];
                $scope.bookingdetail.BookingExtrabeds.unshift(data);
                if ($scope.bookingdetail.BookingExtrabeds != null && $scope.bookingdetail.BookingExtrabeds.length > 0) {
                    for (var i = 0; i < $scope.bookingdetail.BookingExtrabeds.length; i++) {
                        $("#DateUse_Extrabed_" + i).val($scope.bookingdetail.BookingExtrabeds[i].DateUser)
                    }
                }
            }
        }
        $scope.removeExtrabedForBooking = function (index) {
            $scope.bookingdetail.BookingExtrabeds.splice(index, 1)
        }
        $scope.addServiceForBooking = function (item) {
            if ($scope.bookingdetail.BookingServices != null && $scope.bookingdetail.BookingServices.length > 0) {
                for (var i = 0; i < $scope.bookingdetail.BookingServices.length; i++) {
                    $scope.bookingdetail.BookingServices[i].DateUser = $("#DateUse_Clone_" + i).val()
                }
            }
            var itemClone = JSON.parse(JSON.stringify(item));
            itemClone.Number = 1;
            itemClone.Paid = false;
            itemClone.DateUser = helper.getDateNow()
            $scope.bookingdetail.BookingServices.unshift(itemClone)
            if ($scope.bookingdetail.BookingServices != null && $scope.bookingdetail.BookingServices.length > 0) {
                for (var i = 0; i < $scope.bookingdetail.BookingServices.length; i++) {
                    $("#DateUse_Clone_" + i).val($scope.bookingdetail.BookingServices[i].DateUser)
                }
            }
        }
        $scope.ConfirmSelectService = function () {
            if ($scope.bookingdetail.BookingServices != null && $scope.bookingdetail.BookingServices.length > 0) {
                for (var i = 0; i < $scope.bookingdetail.BookingServices.length; i++) {
                    $scope.bookingdetail.BookingServices[i].DateUser = $("#DateUse_Clone_" + i).val()
                    $scope.bookingdetail.BookingServices[i].DateCreate = $scope.FormatDate($scope.bookingdetail.BookingServices[i].DateUser)
                }
            }
            UIkit.modal("#ModalScheduleService").tryhide()
        }
        $scope.ConfirmSelectExtrabed = function () {
            if ($scope.bookingdetail.BookingExtrabeds != null && $scope.bookingdetail.BookingExtrabeds.length > 0) {
                for (var i = 0; i < $scope.bookingdetail.BookingExtrabeds.length; i++) {
                    $scope.bookingdetail.BookingExtrabeds[i].DateUser = $("#DateUse_Extrabed_" + i).val()
                    $scope.bookingdetail.BookingExtrabeds[i].DateCreate = $scope.FormatDate($scope.bookingdetail.BookingExtrabeds[i].DateUser)
                }
            }
            UIkit.modal("#ModalScheduleExtrabed").tryhide()
        }
        $scope.removeServiceForBooking = function (index) {
            $scope.bookingdetail.BookingServices.splice(index, 1)
        }
        $scope.GetRoomAvailable = function () {
            $("#loader").css("display", "block")
            $scope.roomIdCurrent = -1
            $http({
                url: urlGetRoomAvailableForBooking,
                method: 'GET',
                params: {
                    roomtypeId: $scope.bookingdetail.RoomTypeId,
                    fromDate: $scope.bookingdetail.ArrivalDate,
                    toDate: $scope.bookingdetail.DepartureDate,
                    typeBooking: $scope.bookingdetail.TypeBooking
                }
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                var data = respone.data;
                $scope.RoomAvailable = data
                UIkit.modal("#ModalAssignRoom").show()
            }, function error(respone) {
                $("#loader").css("display", "none")
            });
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
                $scope.EditBooking($scope.bookingdetail.BookingId)
                $scope.GetBooking()
            }, function error(respone) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra!")
            });
        }
        $scope.CheckIn = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlCheckIn,
                method: 'POST',
                data: {
                    bookingId: $scope.bookingdetail.BookingId
                }
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                notify.success("Thành công")
                UIkit.modal("#ModalCheckIn").tryhide()
                $scope.EditBooking($scope.bookingdetail.BookingId)
                $scope.GetBooking()
            }, function error(respone) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra!")
            });
        }
        $scope.ConfirmCheckIn = function () {
            if ($scope.bookingdetail.RoomId < 0) {
                alert("Bạn chưa gán phòng");
                return
            }
            UIkit.modal("#ModalCheckIn").show()
        }
        $scope.ConfirmUndoCheckIn = function () {
            UIkit.modal("#ModalUndoCheckIn").show()
        }
        $scope.UndoCheckIn = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlUndoCheckIn,
                method: 'POST',
                data: {
                    bookingId: $scope.bookingdetail.BookingId
                }
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                notify.success("Thành công")
                UIkit.modal("#ModalUndoCheckIn").tryhide()
                $scope.EditBooking($scope.bookingdetail.BookingId)
                $scope.GetBooking()
            }, function error(respone) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra!")
            });
        }
        $scope.ConfirmNoShow = function () {
            UIkit.modal("#ModalNoShow").show()
        }
        $scope.NoShow = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlNoShow,
                method: 'POST',
                data: {
                    bookingId: $scope.bookingdetail.BookingId
                }
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                notify.success("Thành công")
                UIkit.modal("#ModalNoShow").tryhide()
                $scope.EditBooking($scope.bookingdetail.BookingId)
                $scope.GetBooking()
            }, function error(respone) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra!")
            });
        }
        $scope.ConfirmCancel = function () {
            UIkit.modal("#ModalCancel").show()
        }
        $scope.Cancel = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlCancel,
                method: 'POST',
                data: {
                    bookingId: $scope.bookingdetail.BookingId
                }
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                notify.success("Thành công")
                UIkit.modal("#ModalCancel").tryhide()
                $scope.EditBooking($scope.bookingdetail.BookingId)
                $scope.GetBooking()
            }, function error(respone) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra!")
            });
        }
        $scope.payBeforCheckout = 0;
        $scope.ConfirmCheckOut = function () {
            UIkit.modal("#ModalCheckOut").show()
        }
        $scope.CheckOut = function () {
            var v1 = validate.isNotNumberSingleShowError({ value: $scope.payBeforCheckout, key: 'payBeforCheckout' })
            if (v1) {
                notify.error('Giá trị không hợp lệ!')
                return
            }
            var TotalPriceBooking = $scope.masterData.TotalRoomCharges +
                $scope.masterData.TotalExtrabed +
                $scope.masterData.TotalFees +
                $scope.masterData.TotalServices -
                $scope.masterData.TotalDiscount

            var totalMustBePay = parseFloat($scope.masterData.TotalRoomCharges +
                $scope.masterData.TotalExtrabed +
                $scope.masterData.TotalFees +
                $scope.masterData.TotalServices -
                $scope.masterData.TotalDiscount -
                $scope.bookingdetail.Paid -
                $scope.bookingdetail.PrePaid
            ).toFixed(0)
            if (parseFloat($scope.payBeforCheckout) != parseFloat(totalMustBePay)) {
                alert("Số tiền thanh toán phải bằng số tiền hóa đơn trả phòng")
                return
            }
            $("#loader").css("display", "block")
            $http({
                url: urlCheckOut,
                method: 'POST',
                data: {
                    bookingId: $scope.bookingdetail.BookingId,
                    totalAmount: TotalPriceBooking
                }
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                notify.success("Thành công")
                UIkit.modal("#ModalCheckOut").tryhide()
                $scope.EditBooking($scope.bookingdetail.BookingId)
                $scope.GetBooking()
            }, function error(respone) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra!")
            });
        }
        $scope.TransferDebt = function () {
            var v1 = validate.isNotNumberSingleShowError({ value: $scope.payBeforCheckout, key: 'payBeforCheckout' })
            if (v1) {
                notify.error("Giá trị không hợp lệ!")
                return
            }
            var totalAmount = $scope.masterData.TotalRoomCharges +
                $scope.masterData.TotalExtrabed +
                $scope.masterData.TotalFees +
                $scope.masterData.TotalServices -
                $scope.masterData.TotalDiscount;

            var totalMustBePay = parseFloat(totalAmount -
                $scope.bookingdetail.Paid -
                $scope.bookingdetail.PrePaid
            ).toFixed(2)
            if (parseFloat($scope.payBeforCheckout) > parseFloat(totalMustBePay)) {
                alert("Số tiền không hợp lệ")
                return
            }
            $("#loader").css("display", "block")
            $http({
                url: urlTransferDebt,
                method: 'POST',
                data: {
                    bookingId: $scope.bookingdetail.BookingId,
                    totalAmount: totalAmount,
                    paid: $scope.bookingdetail.Paid + $scope.bookingdetail.PrePaid,
                    prepay: $scope.payBeforCheckout
                }
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                notify.success("Thành công")
                UIkit.modal("#ModalCheckOut").tryhide()
                $scope.EditBooking($scope.bookingdetail.BookingId)
                $scope.GetBooking()
            }, function error(respone) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra!")
            });
        }
        $scope.RefreshValidate = function () {
            $(".error").css("display", "none")
        }
        $scope.CloseWindow = function () {
            $("#kUI_window").data("kendoWindow").maximize().close();
            $("#kUI_window2").data("kendoWindow").maximize().close();
        }
        // #endregion
        //#region control
        $scope.Step = 1;
        $scope.scheduleRoomPrice = []
        $scope.TotalAmountPay = 0
        $scope.TotalPaymentAmount = 0
        $scope.TotalPayEnd = 0
        $scope.TotalServceChargeNotPaid = 0
        $scope.TotalExtrabedChargeNotPaid = 0
        $scope.CheckAllPayment = false
        $scope.roomChargeCheck = false
        $scope.servicesChargeCheck = false
        $scope.extrabedChargeCheck = false
        $scope.CalServiceNotPaid = function () {
            var total = 0;
            $scope.bookingdetail.BookingServices.forEach(x => {
                if (!x.Paid) {
                    total = (total - 0) + (x.Price * x.Number - 0)
                }
            });
            $scope.TotalServceChargeNotPaid = total
        }
        $scope.CalExtrabedNotPaid = function () {
            var total = 0;
            $scope.bookingdetail.BookingExtrabeds.forEach(x => {
                if (!x.Paid) {
                    total = (total - 0) + (x.Price * x.Number - 0)
                }
            });
            $scope.TotalExtrabedChargeNotPaid = total
        }
        $scope.showConfirmRoom = function () {
            UIkit.modal("#ModalConfirmRoom").show()
        }
        $scope.ChangeStay = function () {
            $scope.Step = 1;
            UIkit.modal("#ModalChangeStay").show()
        }
        $scope.PayBookingCharge = function () {
            $scope.RefreshCheckPayment()
            $timeout(function () {
                $scope.CheckAllPayment = true
            }, 200);
            UIkit.modal("#ModalPayBooking").show()
        }
        $scope.PayRoomCharge = function () {
            $scope.RefreshCheckPayment()
            $timeout(function () {
                if (!$scope.bookingdetail.BookingPrices[0].Paid)
                    $scope.roomChargeCheck = true;
            }, 200);
            UIkit.modal("#ModalPayBooking").show()
        }
        $scope.PayExtrabedCharge = function () {
            $scope.RefreshCheckPayment()
            $timeout(function () {
                $scope.extrabedChargeCheck = true
            }, 200);
            UIkit.modal("#ModalPayBooking").show()
        }
        $scope.PayServiceCharge = function () {
            $scope.RefreshCheckPayment()
            $timeout(function () {
                $scope.servicesChargeCheck = true
            }, 200);
            UIkit.modal("#ModalPayBooking").show()
        }
        $scope.PayOptionCharge = function () {
            $scope.RefreshCheckPayment()
            $timeout(function () {
                $scope.CheckAllPayment = false
            }, 200);
            UIkit.modal("#ModalPayBooking").show()
        }
        $scope.BackChooseDateChangeStay = function () {
            $scope.Step = 1;
        }
        $scope.ChangeStepChangeStay = function (step) {
            if (step == 1)
                $scope.BackChooseDateChangeStay()
            else
                $scope.ShowRoomPriceNewDateRange()
        }
        $scope.ShowRoomPriceNewDateRange = function () {
            var fromDate = $("#FromDate").val();
            var toDate = $("#ToDate").val();
            var hourFrom = $("#HourCheckin_Step1").val();
            var hourTo = $("#HourCheckout_Step1").val();

            var v1 = validate.isNotDate({ value: fromDate, key: 'FromDate' })
            var v2 = validate.isNotDate({ value: toDate, key: 'ToDate' })
            var v3 = validate.isNotHour({ value: hourFrom, key: 'hourFrom' })
            var v4 = validate.isNotHour({ value: hourTo, key: 'hourTo' })
            if (v1 || v2 || v3 || v4) {
                notify.error("Ngày tháng không hợp lệ!")
                return
            }
            $("#loader").css("display", "block")
            $http({
                method: "GET",
                url: urlGetPriceRoomByRangeDate,
                params: {
                    roomtypeId: $scope.bookingdetail.RoomTypeId,
                    fromDate: $scope.FormatDate(fromDate) + "T" + hourFrom + ":00",
                    toDate: $scope.FormatDate(toDate) + "T" + hourTo + ":00",
                    voucher: $scope.data.Voucher
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                $scope.Step = 2;
                $scope.scheduleRoomPrice = JSON.parse(response.data)
            }, function error(respone) {
                $("#loader").css("display", "none")
            })
        }
        $scope.ChangeStayDate = function () {
            var fromDate = $("#FromDate").val();
            var toDate = $("#ToDate").val();
            var hourFrom = $("#HourCheckin_Step1").val();
            var hourTo = $("#HourCheckout_Step1").val();

            var v1 = validate.isNotDate({ value: fromDate, key: 'FromDate' })
            var v2 = validate.isNotDate({ value: toDate, key: 'ToDate' })
            var v3 = validate.isNotHour({ value: hourFrom, key: 'hourFrom' })
            var v4 = validate.isNotHour({ value: hourTo, key: 'hourTo' })
            if (v1 || v2 || v3 || v4) {
                notify.error("Ngày tháng không hợp lệ!")
                return
            }
            $("#loader").css("display", "block")
            $http({
                method: "POST",
                url: urlChangeStay,
                data: {
                    bookingId: $scope.bookingdetail.BookingId,
                    fromDate: $scope.FormatDate(fromDate) + "T" + hourFrom + ":00",
                    toDate: $scope.FormatDate(toDate) + "T" + hourTo + ":00",
                    scheduleRoomPrice: $scope.scheduleRoomPrice
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                notify.success("Thành công")
                UIkit.modal("#ModalChangeStay").tryhide()
                $scope.EditBooking($scope.bookingdetail.BookingId)
                $scope.GetBooking()
            }, function error(respone) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra!")
            })
        }
        $scope.CalTotalAmountPay = function () {
            var total = 0;
            if ($scope.roomChargeCheck) {
                total += $scope.masterData.TotalRoomCharges
            }
            $scope.bookingdetail.BookingServices.forEach(x => {
                if (x.check) {
                    total = (total - 0) + (x.Price * x.Number - 0)
                }
            });
            $scope.bookingdetail.BookingExtrabeds.forEach(x => {
                if (x.check) {
                    total = (total - 0) + (x.Price * x.Number - 0)
                }
            });
            $scope.TotalAmountPay = total
            $scope.TotalFeeForPay = total * (100 - $scope.bookingdetail.Discount) * $scope.taxFees / 10000
        }
        $scope.CalTotalPay = function () {
            var total = 0;
            if ($scope.bookingdetail.PrePaid > $scope.TotalAmountPay * (100 - $scope.bookingdetail.Discount) / 100 + $scope.TotalFeeForPay) {
                total = 0
            }
            else {
                total = $scope.TotalAmountPay * (100 - $scope.bookingdetail.Discount) / 100 + $scope.TotalFeeForPay - $scope.bookingdetail.PrePaid
            }
            $scope.TotalPaymentAmount = total
        }
        $scope.$watch('CheckAllPayment', function () {
            if ($scope.CheckAllPayment) {
                if ($scope.bookingdetail.BookingPrices[0].Paid)
                    $scope.roomChargeCheck = false;
                else
                    $scope.roomChargeCheck = true
                $scope.servicesChargeCheck = true
                $scope.extrabedChargeCheck = true
            }
            else {
                $scope.roomChargeCheck = false
                $scope.servicesChargeCheck = false
                $scope.extrabedChargeCheck = false
            }
        });
        $scope.$watch('servicesChargeCheck', function () {
            if ($scope.servicesChargeCheck) {
                $scope.bookingdetail.BookingServices.forEach(x => {
                    if (!x.Paid) {
                        x.check = true;
                    }
                });
            }
            else {
                $scope.bookingdetail.BookingServices.forEach(x => {
                    x.check = false;
                });
            }
        });
        $scope.$watch('extrabedChargeCheck', function () {
            if ($scope.extrabedChargeCheck) {
                $scope.bookingdetail.BookingExtrabeds.forEach(x => {
                    if (!x.Paid) {
                        x.check = true;
                    }
                });
            }
            else {
                $scope.bookingdetail.BookingExtrabeds.forEach(x => {
                    x.check = false;
                });
            }
        });
        $scope.RefreshCheckPayment = function () {
            if ($scope.CheckAllPayment)
                $scope.CheckAllPayment = false;
            else {
                $scope.CheckAllPayment = true;
                $timeout(function () {
                    $scope.CheckAllPayment = false
                }, 100);
            }
        }
        $scope.PayBooking = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlPayBooking,
                method: 'POST',
                data: {
                    bookingId: $scope.bookingdetail.BookingId,
                    totalAmount: $scope.TotalAmountPay * (100 - $scope.bookingdetail.Discount) / 100 + $scope.TotalFeeForPay,
                    totalRooms: $scope.masterData.TotalRoomCharges,
                    totalFees: $scope.TotalFeeForPay,
                    roomCharge: $scope.roomChargeCheck,
                    bookingServices: $scope.bookingdetail.BookingServices,
                    bookingExtrabeds: $scope.bookingdetail.BookingExtrabeds,
                    totalDiscount: $scope.TotalAmountPay * $scope.bookingdetail.Discount / 100
                }
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                notify.success("Thành công")
                UIkit.modal("#ModalPayBooking").tryhide()
                $scope.EditBooking($scope.bookingdetail.BookingId)
                $scope.GetBooking()
            }, function error(respone) {
                $("#loader").css("display", "none")
            });

        }
        //#endregion        //#region invoice
        $scope.invoice = {}
        $scope.showInvoice = false;
        $scope.OpenInvoice = function (id) {
            $scope.showInvoice = true
            $("#loader").css("display", "block")
            $http({
                method: "GET",
                url: urlOpenInvoice,
                params: {
                    id: id
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                $scope.invoice = JSON.parse(response.data).invoiceBooking;
                $scope.dataVoice = JSON.parse($scope.invoice.JsonData)
            }, function error(respone) {
                $("#loader").css("display", "none")
            })
        }
        $scope.printInvoice = function () {
            var contents = $("#Invoice").html();
            var frame1 = $('<iframe />');
            frame1[0].name = "frame1";
            $("body").append(frame1);
            var frameDoc = frame1[0].contentWindow ? frame1[0].contentWindow : frame1[0].contentDocument.document ? frame1[0].contentDocument.document : frame1[0].contentDocument;
            frameDoc.document.open();
            frameDoc.document.write('<html><title>Booking Engine</title><head>');
            frameDoc.document.write('<link href="/bower_components/uikit/css/uikit.almost-flat.min.css" rel="stylesheet" type="text/css" />');
            frameDoc.document.write('<link href="/assets/css/main.min.css" rel="stylesheet" type="text/css" />');
            frameDoc.document.write('<link href="/Content/css/stylePrint.css" rel="stylesheet" type="text/css" />');
            frameDoc.document.write('</head><body>');
            frameDoc.document.write(contents);
            frameDoc.document.write('</body></html>');
            frameDoc.document.close();
            setTimeout(function () {
                window.frames["frame1"].focus();
                window.frames["frame1"].print();
                frame1.remove();
            }, 500);
        }
        //#endregion
        //#region add new booking
        $scope.ShowAddBooking = function () {
            $scope.bookingdetail = {
                PaymentType: 1,
                BookingServices: [],
                BookingExtrabeds: []
            }
            $scope.roomtypeAvailability = []
            $scope.bookingChoose = []
            $("#kUI_window2").data("kendoWindow").maximize().open();
        }
        //#endregion

        // #region special
        $scope.roomtypeAvailability = []
        $scope.bookingChoose = []
        $scope.calculatorRoomPrice = function () {
            $("#loader").css("display", "block")
            $http({
                method: "GET",
                url: urlCalculatorRoomPrice,
                params: {
                    id: $scope.bookingdetail.BookingId
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                $scope.EditBooking($scope.bookingdetail.BookingId)
                $scope.GetBooking()
            }, function error(respone) {
                $("#loader").css("display", "none")
            })
        }
        $interval(function () {
            if ($scope.bookingdetail.Status != 2) {
                $scope.countTimeRun = '0h:00:00'
                return
            }
            if ($scope.bookingdetail.CheckinDate == null || $scope.bookingdetail.CheckinDate == '')
                return;
            var startDate = new Date();
            var arrDateTimeArrival = $scope.bookingdetail.CheckinDate.split('T');
            var date = arrDateTimeArrival[0].split('-');
            var time = arrDateTimeArrival[1].split(':');
            var endDate = new Date(date[0], (date[1] - 0) - 1, date[2], time[0], time[1], time[2]);
            var seconds = (startDate.getTime() - endDate.getTime()) / 1000;
            var hour = parseInt(seconds / 3600);
            seconds = seconds - hour * 3600;
            var minute = parseInt(seconds / 60);
            seconds = seconds - minute * 60
            var sc = parseInt(seconds)
            $scope.countTimeRun = hour + "h:" + (minute < 10 ? "0" + minute : minute) + ":" + (sc < 10 ? "0" + sc : sc)
        }, 1000);
        $scope.checkRoomRypeChoose = function (arrayRoomTypeAvailable) {
            if ($scope.bookingChoose == null || $scope.bookingChoose.length == 0) {
                return arrayRoomTypeAvailable;
            }
            var arrayRoomType = new Array();
            arrayRoomTypeAvailable.forEach(x => {
                var numberRoomTypeExist = $scope.bookingChoose.filter(y => y.RoomTypeId == x.RoomTypeId).length
                x.NumberRoomAvailable -= numberRoomTypeExist;
                arrayRoomType.push(x);
            })
            return arrayRoomType;
        }
        $scope.SearchAvailability = function () {
            var fromDate = $("#FromDate_Step1").val();
            var toDate = $("#ToDate_Step1").val();
            var hourFrom = $("#HourCheckin_Step1").val();
            var hourTo = $("#HourCheckout_Step1").val();
            if ($scope.data.TypeReservation == 0) {
                var v1 = validate.isNotDate({ value: fromDate, key: 'fromDate' })
                var v2 = validate.isNotDate({ value: toDate, key: 'toDate' })
                var v3 = validate.isNotHour({ value: hourFrom, key: 'hourFrom' })
                var v4 = validate.isNotHour({ value: hourTo, key: 'hourTo' })
                if (v1 || v2 || v3 || v4) {
                    notify.error("Ngày tháng không hợp lệ!")
                    return
                }
            }
            else {
                var v1 = validate.isNotDate({ value: fromDate, key: 'fromDate' })
                var v3 = validate.isNotHour({ value: hourFrom, key: 'hourFrom' })
                if (v1 || v3) {
                    notify.error("Ngày tháng không hợp lệ!")
                    return
                }
                toDate = fromDate
                hourTo = hourFrom
            }

            $("#loader").css("display", "block")
            $http({
                method: "GET",
                url: urlGetRoomAvailability,
                params: {
                    type: $scope.data.TypeReservation,
                    fromDate: $scope.FormatDate(fromDate) + "T" + hourFrom + ":00",
                    toDate: $scope.FormatDate(toDate) + "T" + hourTo + ":00",
                    voucher: $scope.data.Voucher
                }
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                if (respone.data == -1) {
                    alert("Mã khuyễn mãi không có hiệu lực")
                    return
                }
                $scope.roomtypeAvailability = $scope.checkRoomRypeChoose(JSON.parse(respone.data))
                if ($scope.roomtypeAvailability != null && $scope.roomtypeAvailability.length > 0)
                    $scope.roomtypeAvailability.forEach(x => {
                        x.TypeBooking = $scope.data.TypeReservation
                    })
            }, function error(respone) {
                $("#loader").css("display", "none")
            })
        }
        $scope.addBooking = function (item) {
            if (item.NumberRoomAvailable == 0)
                return;
            for (var i = 0; i < item.NumberRoomChoose; i++) {
                var booking = JSON.parse(JSON.stringify(item));
                booking.RoomId = -1
                booking.BookingPrices = booking.RateAvailabilitys
                booking.BookingExtrabeds = booking.RoomTypeExtrabeds;
                $scope.bookingChoose.unshift(booking)
            }
            item.NumberRoomAvailable -= item.NumberRoomChoose;
            item.NumberRoomChoose = 1
        }
        $scope.removeBooking = function (index) {
            var roomtype = $scope.roomtypeAvailability.find(x => x.RoomTypeId == $scope.bookingChoose[index].RoomTypeId)
            roomtype.NumberRoomAvailable += 1
            roomtype.NumberRoomChoose = 1
            $scope.bookingChoose.splice(index, 1)
        }
        $scope.showSchedulePrice = function (model) {
            if (model.NumberRoomAvailable == 0)
                return;
            $scope.schedulePrice = model;
            UIkit.modal("#ModalSchedulePrice").show()
        }
        $scope.showScheduleExtrabedForAddBooking = function (model) {
            if (model.NumberRoomAvailable == 0)
                return;
            $scope.scheduleExtrabed = model;
            UIkit.modal("#ModalScheduleExtrabedAddBooking").show()
        }
        $scope.calculatorExtrabed = function (extrabed) {
            var total = 0;
            extrabed.forEach(x => {
                total += (x.Number - 0)
            })
            return total;
        }
        $scope.calculatorPrice = function (model) {
            var total = 0;
            model.RateAvailabilitys.forEach(x => {
                total += (x.Price - 0)
            })
            model.Total = total
        }
        $scope.GetArrayNumber = function (min = 0, max = 0) {
            var array = new Array();
            for (var i = min; i <= max; i++) {
                array.push(i)
            }
            return array;
        }
        $scope.PostAddBooking = function () {
            if ($scope.bookingChoose != null && $scope.bookingChoose.length > 0) {
                $http({
                    url: urlPostAddBooking,
                    method: "POST",
                    data: {
                        reservationId: $scope.ReservationId,
                        bookings: $scope.bookingChoose,
                        includeVATAndServiceCharge: $('#IncludeVATAndServiceCharge').is(":checked")
                    }
                }).then(function success(response) {
                    $("#loader").css("display", "none")
                    notify.success('Success')
                    $("#kUI_window2").data("kendoWindow").maximize().close();
                    $scope.GetData()
                    $scope.GetBooking()
                }, function error(response) {
                    $("#loader").css("display", "none")
                });
            }
        }
        //#endregion

        // #region group action
        $scope.AutoAssignRoom = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlAutoAssignRoom,
                method: "POST",
                data: {
                    reservationId: $scope.ReservationId
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                if (response.data.result == 1)
                    notify.success('Success')
                else {
                    alert("OverBooking - Không thể gán phòng cho các đặt phòng có mã : " + response.data.message)
                }
                $scope.GetBooking()
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.ChangeCheckAllCheckIn = function () {
            if ($scope.checkAllCheckIn) {
                if ($scope.bookingNeedToCheckIn != null) {
                    $scope.bookingNeedToCheckIn.forEach(x => {
                        x.Check = true
                    })
                }
            }
            else {
                if ($scope.bookingNeedToCheckIn != null) {
                    $scope.bookingNeedToCheckIn.forEach(x => {
                        x.Check = false
                    })
                }
            }
        }
        $scope.ChangeCheckAllCheckOut = function () {
            if ($scope.checkAllCheckOut) {
                if ($scope.bookingNeedToCheckOut != null) {
                    $scope.bookingNeedToCheckOut.forEach(x => {
                        x.Check = true
                    })
                }
            }
            else {
                if ($scope.bookingNeedToCheckOut != null) {
                    $scope.bookingNeedToCheckOut.forEach(x => {
                        x.Check = false
                    })
                }
            }
        }
        $scope.ShowGroupCheckIn = function () {
            $scope.bookingNeedToCheckIn = []
            $("#loader").css("display", "block")
            $http({
                url: urlGetBookingNeedToCheckIn,
                method: "GET",
                params: {
                    reservationId: $scope.ReservationId
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                $scope.bookingNeedToCheckIn = JSON.parse(response.data)
                $scope.checkAllCheckIn = true
                if ($scope.bookingNeedToCheckIn != null) {
                    $scope.bookingNeedToCheckIn.forEach(x => {
                        x.Check = true
                    })
                }
                UIkit.modal("#ModalGroupCheckIn").show()
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.PostBookingNeedToCheckIn = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlPostBookingNeedToCheckIn,
                method: "POST",
                data: $scope.bookingNeedToCheckIn.filter(x => x.Check)
            }).then(function success(response) {
                $("#loader").css("display", "none")
                notify.success("Thành công")
                UIkit.modal("#ModalGroupCheckIn").tryhide()
                $scope.GetBooking()
            }, function error(response) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra!")
            });
        }
        $scope.ShowGroupCheckOut = function () {
            $scope.bookingNeedToCheckOut = []
            $scope.checkAllCheckOut = true
            $("#loader").css("display", "block")
            var data = $scope.bookings.filter(x => x.Status == 2)
            $scope.bookingNeedToCheckOut = JSON.parse(JSON.stringify(data))
            $scope.bookingNeedToCheckOut.forEach(x => {
                x.Check = true;
                x.TotalAmount = x.Total
            })
            UIkit.modal("#ModalGroupCheckOut").show()
            $("#loader").css("display", "none")
        }
        $scope.PostBookingNeedToCheckOut = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlPostBookingNeedToCheckOut,
                method: "POST",
                data: $scope.bookingNeedToCheckOut.filter(x => x.Check)
            }).then(function success(response) {
                $("#loader").css("display", "none")
                notify.success("Thành công")
                UIkit.modal("#ModalGroupCheckOut").tryhide()
                $scope.GetData()
                $scope.GetBooking()
            }, function error(response) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra!")
            });
        }
        $scope.ShowGroupClusters = function () {
            $scope.ClustersReservation = {}
            UIkit.modal("#ModalClustersReservation").show()
        }
        $scope.ChangeClustersAllReservation = function () {
            if ($scope.clustersAllReservation) {
                if ($scope.ClustersReservation != null && $scope.ClustersReservation.bookings != null) {
                    $scope.ClustersReservation.bookings.forEach(x => {
                        x.Check = true
                    })
                }
            }
            else {
                if ($scope.ClustersReservation != null && $scope.ClustersReservation.bookings != null) {
                    $scope.ClustersReservation.bookings.forEach(x => {
                        x.Check = false
                    })
                }
            }
        }
        $scope.FindReservation = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlGetReservationForClusters,
                method: "GET",
                params: {
                    reservationId: $scope.findReservationId
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                $scope.ClustersReservation = JSON.parse(response.data)
                $scope.clustersAllReservation = false
                if ($scope.ClustersReservation != null && $scope.ClustersReservation.bookings != null) {
                    $scope.ClustersReservation.bookings.forEach(x => {
                        x.Check = false
                    })
                }
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.PostGroupClusters = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlPostGroupClusters,
                method: "POST",
                data: {
                    reservationId: $scope.ReservationId,
                    bookings: $scope.ClustersReservation.bookings.filter(x => x.Check),
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                notify.success("Thành công")
                UIkit.modal("#ModalClustersReservation").tryhide()
                $scope.GetData()
                $scope.GetBooking()
            }, function error(response) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra!")
            });
        }
        //#endregion
    }])