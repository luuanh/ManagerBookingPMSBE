var urlGetRoomAvailability = "/Admin/Reservation/GetRoomAvailability"
var urlGetRoomAvailable = "/Admin/Reservation/GetRoomAvailable"
var urlPost = "/Admin/Reservation/Post"
var urlGetTaxFee = "/Admin/TaxFee/GetToalFee_Lastest"
var urlGetAllGuest = "/Admin/Guest/GetAllGuest"
var urlDetailGuest = "/Admin/Guest/Detail"
var urlDetailAgency = "/Admin/Company/Detail"
// service

var urlGetServiceCategory = "/Admin/ServiceCategory/GetFull"
var urlGetServices = "/Admin/Service/GetAll"
var urlGetDiscountForGuest = "/Admin/Guest/GetDiscountForGuest"

app.controller('controller', ['$scope', '$http', '$log', 'uiGridConstants', 'validate', 'notify', 'helper',
    function ($scope, $http, $log, uiGridConstants, validate, notify, helper) {
        //#region step 1
        $scope.Step = 1;
        $scope.IncludeVATAndServiceCharge = true
        $scope.data = {
            TypeReservation: 0,
            ReminiscentName: '',
            Color: '#80FF80',
            Adult: 2,
            Children: 0,
            Voucher: '',
            Note: '',
            BookingSource: 1,
            ArrivalFlightDate: '',
            ArrivalFlightTime: '',
            DepartureFlightDate: '',
            DepartureFlightTime: '',
            Guest: {
                GuestId: -1,
                FirstName: '',
                SurName: '',
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
                Note: '',
                TotalPaid: 0,
                Discount: 0
            },
            Company: {
                CompanyId: -1,
                GroupGuestId: -1,
                SourceId: -1,
                CompanyName: '',
                CompanyCode: '',
                TaxCode: '',
                Phone: '',
                Fax: '',
                Email: '',
                Address: '',
                ContactUsName: '',
                ContactPhone: '',
                ContactEmail: ''
            },
            Deposit: 0,
            Amount: 0,
            BalanceDue: 0,
            DepositPecent: 0,
            PaymentType: 1
        }
        $scope.roomtypeAvailability = []
        $scope.bookingChoose = []
        $scope.taxFee = [];
        $scope.taxFeeDefault = [];
        $scope.showSchedulePrice = function (model) {
            if (model.NumberRoomAvailable == 0)
                return;
            $scope.schedulePrice = model;
            UIkit.modal("#ModalSchedulePrice").show()
        }
        $scope.showScheduleExtrabed = function (model) {
            if (model.NumberRoomAvailable == 0)
                return;
            $scope.scheduleExtrabed = model;
            UIkit.modal("#ModalScheduleExtrabed").show()
        }
        $scope.calculatorPrice = function (model) {
            var total = 0;
            model.RateAvailabilitys.forEach(x => {
                total += (x.Price - 0)
            })
            model.Total = total
        }
        $scope.calculatorExtrabed = function (extrabed) {
            var total = 0;
            extrabed.forEach(x => {
                total += (x.Number - 0)
            })
            return total;
        }
        $scope.calSubTotal_footer = function () {
            var total = 0;
            if ($scope.bookingChoose != null || $scope.bookingChoose.length > 0) {
                $scope.bookingChoose.forEach(x => {
                    total += (x.Total - 0)
                })
            }
            $scope.SubTotalPrice = total
        }
        $scope.calExtrabed_footer = function () {
            var total = 0;
            if ($scope.bookingChoose != null || $scope.bookingChoose.length > 0) {
                $scope.bookingChoose.forEach(x => {
                    x.RoomTypeExtrabeds.forEach(y => {
                        total += (y.Price - 0) * y.Number
                    })
                })
            }
            $scope.ExtrabedPrice = total
        }
        $scope.calFee_footer = function () {
            var discountGuest = 0;
            if ($scope.data != null && $scope.data.Guest != null || $scope.data.Guest.Discount != null) {
                discountGuest = $scope.data.Guest.Discount;
            }
            $scope.FeePrice = ($scope.SubTotalPrice + $scope.ExtrabedPrice + $scope.ServicePrice) * (100 - discountGuest) * (($scope.taxFee[0] - 0) + ($scope.taxFee[1] - 0) + ($scope.taxFee[0] - 0) * ($scope.taxFee[1] - 0) / 100) / 10000;
        }
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
        $scope.addBooking = function (item) {
            if (item.NumberRoomAvailable == 0)
                return;
            for (var i = 0; i < item.NumberRoomChoose; i++) {
                var booking = JSON.parse(JSON.stringify(item));
                booking.RoomId = -1
                booking.BookingPrices = booking.RateAvailabilitys
                booking.BookingExtrabeds = booking.RoomTypeExtrabeds
                booking.Guest = {
                    GuestId: -1,
                    FirstName: '',
                    SurName: '',
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
                    Note: '',
                    TotalPaid: 0,
                    Discount: 0
                };
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
        $scope.StepReservation = function () {
            $scope.Step = 1
        }
        $scope.FormatDate = function (date) {
            date = date.trim()
            var dt = date.split('-');
            var d = dt[0];
            dt[0] = dt[2];
            dt[2] = d;
            return dt.join('-')
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
                    notify.error('Ngày tháng không hợp lệ!')
                    return
                }
            }
            else {
                var v1 = validate.isNotDate({ value: fromDate, key: 'fromDate' })
                var v3 = validate.isNotHour({ value: hourFrom, key: 'hourFrom' })
                if (v1 || v3) {
                    notify.error('Ngày tháng không hợp lệ!')
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
        $scope.GetArrayNumber = function (min = 0, max = 0) {
            var array = new Array();
            for (var i = min; i <= max; i++) {
                array.push(i)
            }
            return array;
        }
        $scope.RefreshValidate = function () {
            $(".error").css("display", "none")
        }
        $scope.GetTaxFee = function () {
            $http({
                url: urlGetTaxFee,
                method: "GET"
            }).then(function success(response) {
                $scope.taxFee = response.data;
                $scope.taxFeeDefault = response.data;
            }, function error(response) {
            });
        }
        $scope.GetTaxFee();
        //#endregion

        //#region Step 2 Guest
        $scope.guest = {}
        $scope.Dob = null
        $scope.Dob2 = null
        $scope.StepGuest = function () {
            if ($scope.AutoCheckAccept_Accommodation())
                return;
            $scope.Step = 2;
            $scope.addNewGuest()
            $scope.getRoomAvailable()
        }
        $scope.getRoomAvailable = function () {
            $("#loader").css("display", "block")
            var dataPost = new Array();
            $scope.bookingChoose.forEach(x => {
                dataPost.push({
                    RoomTypeId: x.RoomTypeId,
                    FromDate: x.ArrivalDate,
                    ToDate: x.DepartureDate,
                    TypeBooking: x.TypeBooking
                })
            });
            $http({
                method: 'POST',
                url: urlGetRoomAvailable,
                data: dataPost
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                var data = JSON.parse(respone.data);
                $scope.data.CheckIn = data.arrivalDateMin
                $scope.data.CheckOut = data.departureDateMax
                data.filterRoomAvailablesResult.forEach(x => {
                    var dataClone = JSON.parse(JSON.stringify(x));
                    var item = $scope.bookingChoose.find(y => y.RoomTypeId == dataClone.RoomTypeId && y.Rooms == null);
                    if (item != null) {
                        item.Rooms = dataClone.Rooms
                        item.RoomsBase = JSON.parse(JSON.stringify(dataClone.Rooms));
                        item.RoomId = -1;
                    }
                });
            }, function error(respone) {
                $("#loader").css("display", "none")
            })
        }
        $scope.setRoomForBooking = function (roomtypeId) {
            var arrayRoomIdChosen = new Array();
            $scope.bookingChoose.forEach(x => {
                if (x.RoomId > 0)
                    arrayRoomIdChosen.push(x.RoomId);
            })
            $scope.bookingChoose.forEach(x => {
                x.Rooms = JSON.parse(JSON.stringify(x.RoomsBase));
                arrayRoomIdChosen.forEach(y => {
                    if (y != x.RoomId) {
                        var index = x.Rooms.findIndex(z => z.RoomId == y);
                        x.Rooms.splice(index, 1);
                    }
                });
            });
        }
        $scope.addNewGuest = function () {
            if ($scope.data.Guest == null) {
                $scope.guest = {
                    GuestId: -1,
                    FirstName: '',
                    SurName: '',
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
                    Note: '',
                    TotalPaid: 0,
                    Discount: 0
                }
                $("#Dob").val('');
            }
            else {
                $scope.guest = $scope.data.Guest
                if ($scope.guest.Dob != null)
                    $scope.Dob = $scope.FormatDate($scope.guest.Dob.split('T')[0])
                else
                    $scope.Dob = null
            }
        }
        $scope.refreshGuest = function () {
            $scope.guest = {
                GuestId: -1,
                FirstName: '',
                SurName: '',
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
                Note: '',
                TotalPaid: 0,
                Discount: 0
            }
            $("#Dob").val('');
        }
        $scope.addNewGuestForBooking = function (item) {
            if (item.Guest == null) {
                $scope.guestForBooking = {
                    GuestId: -1,
                    FirstName: '',
                    SurName: '',
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
                    Note: '',
                    TotalPaid: 0,
                    Discount: 0
                }
                $("#Dob2").val('');
            }
            else {
                $scope.guestForBooking = item.Guest
                if ($scope.guestForBooking.Dob != null)
                    $scope.Dob2 = $scope.FormatDate($scope.guestForBooking.Dob.split('T')[0])
                else
                    $scope.Dob2 = null
            }

        }
        $scope.showMoreGuestInformation = false;
        $scope.showMoreAgencyInformationModel = false;
        $scope.showMoreInformation = function () {
            $scope.showMoreGuestInformation = !$scope.showMoreGuestInformation
        }
        $scope.showMoreAgencyInformation = function () {
            $scope.showMoreAgencyInformationModel = !$scope.showMoreAgencyInformationModel
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
                    $scope.Dob = null
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
        $scope.GetInfoGuest_AfterAutoComplete_ForBooking = function () {
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
                $scope.guestForBooking = JSON.parse(respone.data).guest
                if ($scope.guestForBooking.Dob != null)
                    $scope.Dob2 = $scope.FormatDate($scope.guestForBooking.Dob.split('T')[0])
                else
                    $scope.Dob2 = null
            }, function error(respone) {
                $("#loader").css("display", "none")
            })
        }
        $scope.addGuest_ForBooking = function (item) {
            $scope.booking_Start_Get_Guest = item;
            $scope.addNewGuestForBooking(item)
            $("#kUI_window").data("kendoWindow").maximize().open();
        }
        $scope.ApplyGuest_ForBooking = function () {
            var v1 = validate.isNullOrEmptySingleShowError({ value: $scope.guestForBooking.FirstName, key: 'FirstName' })
            var v2 = validate.isNullOrEmptySingleShowError({ value: $scope.guestForBooking.SurName, key: 'SurName' })
            var v3 = validate.isNullOrEmptySingleShowError({ value: $scope.guestForBooking.Phone, key: 'Phone' })
            if (v1 || v2 || v3) {
                notify.error('Giá trị không hợp lệ!')
                return
            }
            if ($("#Dob2").val() != '') {
                var datetime = $("#Dob2").val();
                $scope.guestForBooking.Dob = $scope.FormatDate(datetime)
            }
            $scope.booking_Start_Get_Guest.Guest = $scope.guestForBooking
            $("#kUI_window").data("kendoWindow").close();
        }
        $scope.closeWindowGuest_ForBooking = function () {
            $("#kUI_window").data("kendoWindow").close();
        }
        //#endregion

        //#region Step 3 Service
        $scope.serviceChoose = [];
        $scope.services = [];
        $scope.roomtypeForService = []
        $scope.StepService = function () {
            if ($scope.AutoCheckAccept_Accommodation() || $scope.AutoCheckAccept_Guest())
                return;
            $scope.Step = 3
            $scope.GetRoomTypeInRoomChoose()
            $scope.GetServiceCategory()
        }
        $scope.GetRoomTypeInRoomChoose = function () {
            $scope.roomtypeForService = []
            var index = 1;
            $scope.bookingChoose.forEach(x => {
                $scope.roomtypeForService.push({
                    RoomTypeId: x.RoomTypeId,
                    RoomTypeName: x.RoomTypeName,
                    Index: index
                })
                index += 1
            })
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
        $scope.changeTypeServiceCategory = function (serviceCategoryId) {
            $(".box-service-category > div").removeClass('selected-category')
            $("#box_serviceCategory_" + serviceCategoryId).addClass('selected-category')
            $scope.GetService(serviceCategoryId)
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
            if ($scope.serviceChoose != null && $scope.serviceChoose.length > 0) {
                for (var i = 0; i < $scope.serviceChoose.length; i++) {
                    $scope.serviceChoose[i].DateUser = $("#DateUse_" + i).val()
                }
            }
            var itemClone = JSON.parse(JSON.stringify(item));
            itemClone.Number = 1;
            itemClone.DateUser = helper.getDateNow()
            itemClone.RoomTypeIdIndex = $scope.roomtypeForService[0].RoomTypeId + "-1";
            $scope.serviceChoose.unshift(itemClone)
            if ($scope.serviceChoose != null && $scope.serviceChoose.length > 0) {
                for (var i = 0; i < $scope.serviceChoose.length; i++) {
                    $("#DateUse_" + i).val($scope.serviceChoose[i].DateUser)
                }
            }
        }
        $scope.removeService = function (index) {
            $scope.serviceChoose.splice(index, 1)
        }
        $scope.calService_footer = function () {
            var total = 0;
            $scope.serviceChoose.forEach(x => {
                total += x.Price * x.Number
            })
            $scope.ServicePrice = total;
        }
        //#endregion

        //#region Step 4 Confirm & Payment

        $scope.numberNightBook = function () {

        }
        $scope.AutoCheckAccept_Accommodation = function () {
            if ($scope.bookingChoose == null || $scope.bookingChoose.length == 0) {
                alert("Bạn chưa chọn phòng")
                return true;
            }
            return false
        }
        $scope.AutoCheckAccept_Guest = function () {
            var v1 = validate.isNullOrEmptySingleShowError({ value: $scope.guest.FirstName, key: 'FirstName' })
            var v2 = validate.isNullOrEmptySingleShowError({ value: $scope.guest.SurName, key: 'SurName' })
            var v4 = validate.isNullOrEmptySingleShowError({ value: $scope.guest.Phone, key: 'Phone' })
            if (v1 || v2 || v4) {
                alert("Thông tin khách hàng không hợp lệ")
                return true
            }
            return false
        }
        $scope.StepConfirm = function () {
            if ($scope.AutoCheckAccept_Accommodation() || $scope.AutoCheckAccept_Guest())
                return;
            $scope.Step = 4;
            if ($scope.serviceChoose != null && $scope.serviceChoose.length > 0) {
                for (var i = 0; i < $scope.serviceChoose.length; i++) {
                    $scope.serviceChoose[i].DateUser = $("#DateUse_" + i).val()
                }
            }
        }
        $scope.calculatorExpectedBalance = function () {
            var total = helper.round(($scope.ExtrabedPrice + $scope.SubTotalPrice + $scope.ServicePrice) * (100 - $scope.data.Guest.Discount) * (100 + ($scope.taxFee[0] - 0) + ($scope.taxFee[1] - 0) + ($scope.taxFee[0] - 0) * ($scope.taxFee[1] - 0) / 100) / 10000 - $scope.data.Amount)
            $scope.ExpectedBalance = total
        }
        $scope.calculatorBalanceDue = function () {
            $scope.data.BalanceDue = helper.round(($scope.ExtrabedPrice + $scope.SubTotalPrice + $scope.ServicePrice) * (100 - $scope.data.Guest.Discount) * (100 + ($scope.taxFee[0] - 0) + ($scope.taxFee[1] - 0) + ($scope.taxFee[0] - 0) * ($scope.taxFee[1] - 0) / 100) / 10000)
        }
        $scope.calculatorDepositPecent = function () {
            $scope.data.Amount = helper.round($scope.data.BalanceDue * $scope.data.DepositPecent / 100)
        }
        $scope.ConfirmReservation = function () {
            var v1 = validate.isNotNumberSingleShowError({ value: $scope.data.Deposit, key: 'Deposit' })
            if (v1) {
                notify.error('Giá trị không hợp lệ!')
                return
            }
            if ($scope.AutoCheckAccept_Accommodation() || $scope.AutoCheckAccept_Guest())
                return;
            if ($scope.ExpectedBalance < 0) {
                alert("Số tiền không hợp lệ!");
                return;
            }
            $("#loader").css("display", "block")
            $scope.data.BalanceDue = $scope.FeePrice + $scope.ExtrabedPrice + $scope.SubTotalPrice + $scope.ServicePrice - $scope.data.Deposit
            $scope.data.Bookings = $scope.bookingChoose;
            $scope.data.ReservationServices = $scope.serviceChoose;
            $scope.dataPost = JSON.parse(JSON.stringify($scope.data));
            $scope.refreshDataBeforePost();
            $scope.dataPost.IncludeTaxFee = $('#IncludeVATAndServiceCharge').is(":checked");
            if ($scope.data.ReservationServices != null && $scope.data.ReservationServices.length > 0) {
                for (var i = 0; i < $scope.data.ReservationServices.length; i++) {
                    $scope.data.ReservationServices[i].DateUser = $scope.FormatDate($scope.data.ReservationServices[i].DateUser)
                }
            }
            $http({
                url: urlPost,
                method: "POST",
                data: $scope.dataPost
            }).then(function success(response) {
                $("#loader").css("display", "none")
                notify.success('Success')
                location.href = "/Admin/Reservation/Index"
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.refreshDataBeforePost = function () {
            $scope.dataPost.Bookings.forEach(x => {
                x.RateAvailabilitys = [];
                x.RoomTypeExtrabeds = [];
                x.RoomsBase = [];
                x.Rooms = [];

            })
        }

        $scope.reCalculatorTaxFee = function () {
            var check = $('#IncludeVATAndServiceCharge').is(":checked");
            if (!check) {
                $scope.taxFee = [];
            }
            else {
                $scope.taxFee = $scope.taxFeeDefault
            }
        }
        //#endregion
    }])