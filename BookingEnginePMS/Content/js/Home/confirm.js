var urlGetLanguage = '/Home/GetLanguage';
var urlGetCurrency = '/Home/GetCurrency';
var urlGetData = '/Home/GetDataScreenConfirm';
var urlGetAmountForVoucher = '/Home/GetAmountForVoucher';
var urlPost = '/Home/Post';
var urlSaveDataForPayPal = '/Home/SaveDataForPayPal';

app.controller("controller", ['$scope', '$http', '$timeout', 'helper', 'validate',
    function ($scope, $http, $timeout, helper, validate) {
        $scope.lang = 'vi';
        $scope.currency = 'VND';
        $scope.transitions = []
        $scope.voucher = {
            voucherCode: '',
            accept: -1,
            amountForVoucher: 0
        };
        $scope.params = {};
        $scope.data = []
        $scope.serviceChoose = [];
        $scope.extrabedChoose = [];
        $scope.roomTypeChoose = [];
        $scope.services = [];
        $scope.extrabeds = [];
        $scope.priceRoom = 0;
        $scope.priceService = 0;
        $scope.priceExtrabed = 0;
        $scope.priceDeposit = 0;
        // exchangeRate
        $scope.priceRoomExchangeRate = 0;
        $scope.priceServiceExchangeRate = 0;
        $scope.priceExtrabedExchangeRate = 0;
        $scope.priceDepositExchangeRate = 0;
        $scope.guest = {
            FirstName: '',
            SurName: '',
            Email: '',
            Phone: '',
            Address: '',
            ArrivalFlightDate: '',
            ArrivalFlightTime: '',
            Note: '',
            Country: 'N/A',
            ExpirationMonth: 1,
            ExpirationYear: 2018,
            Name: '',
            Number: '',
            Code: ''
        }
        $scope.paymentMethodChoose = {}
        $scope.acceptPolicy = true;
        $scope.changePaymentMethod = function () {
            $scope.paymentMethodChoose = $scope.paymentMethods.find(x => x.ConfigPaymentMethodId == $scope.guest.TypePaymentMethod);
            if ($scope.guest.TypePaymentMethod == 18) {
                var v1 = validate.isNullOrEmptySingleShowError({ value: $scope.guest.FirstName, key: 'FirstName' });
                var v2 = validate.isNullOrEmptySingleShowError({ value: $scope.guest.SurName, key: 'SurName' });
                var v3 = validate.isEmail({ value: $scope.guest.Email, key: 'Email' });
                if (v1 || v2 || v3) {
                    alert($scope.translateMessage(2354, 'Invalid customer information'));
                    $scope.guest.TypePaymentMethod = -1;
                }
                else {
                    $scope.saveDataForPayPal();
                }
            }
        }
        $scope.getData = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlGetData,
                method: "GET"
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                var data = JSON.parse(respone.data);
                $scope.data = data.data;
                $scope.params = data.param;
                $scope.services = data.services;
                $scope.extrabeds = data.extrabeds;
                $scope.vat = data.vat;
                $scope.serviceCharge = data.serviceCharge;
                $scope.paymentMethods = JSON.parse($("#firstPaymentMethod").val());
                $scope.paymentMethodChoose = $scope.paymentMethods[0];
                if ($scope.paymentMethods.length > 1 || ($scope.paymentMethods.length == 1 && $scope.paymentMethods[0].ConfigPaymentMethodId == 18)) {
                    $scope.guest.TypePaymentMethod = -1
                }
                else {
                    $scope.guest.TypePaymentMethod = $scope.paymentMethods[0].ConfigPaymentMethodId;
                }
                $scope.showRoomTypeChoose();
            }, function error(respone) {
                $("#loader").css("display", "none")
            })
        }
        $scope.showRoomTypeChoose = function () {
            var roomTypeChooseNotRepeat = []
            $scope.data.forEach(x => {
                if (roomTypeChooseNotRepeat.findIndex(y => y.RoomTypeId == x.RoomTypeId) < 0) {
                    roomTypeChooseNotRepeat.push(x);
                }
            });
            roomTypeChooseNotRepeat.forEach(x => {
                var allRoomSameX = $scope.data.filter(y => y.RoomTypeId == x.RoomTypeId)
                var total = 0;
                allRoomSameX.forEach(y => {
                    total += y.PromotionHome.ChooseRoom
                })
                x.TotalRoomChoose = total;
            })
            $scope.roomTypeChoose = roomTypeChooseNotRepeat;
            $scope.calPriceRoom();
            $scope.calDeposit();
        };
        $scope.calAllPriceForBooking = function () {
            $scope.calPriceService();
            $scope.calPriceExtrabed();
        }
        $scope.calPriceRoom = function () {
            // cal price room
            var priceRoom = 0;
            var priceRoomExchangeRate = 0;
            $scope.data.forEach(x => {
                if (x.PromotionHome.ChooseRoom > 0) {
                    if ($scope.currency == 'VND')
                        priceRoom += x.PromotionHome.ChooseRoom * x.PromotionHome.TotalPriceAfterPromotion;
                    else
                        priceRoomExchangeRate += x.PromotionHome.ChooseRoom * x.PromotionHome.TotalPriceAfterPromotionExchangeRate;

                }
            });
            $scope.priceRoom = priceRoom
            $scope.priceRoomExchangeRate = priceRoomExchangeRate
        }
        $scope.calPriceService = function () {
            var priceService = 0;
            var priceServiceExchangeRate = 0;
            $scope.services.forEach(x => {
                if (x.NumberChoose > 0) {
                    if ($scope.currency == 'VND')
                        priceService += x.Price * x.NumberChoose;
                    else
                        priceServiceExchangeRate += x.PriceExchangeRate * x.NumberChoose;
                }
            })
            $scope.priceService = priceService;
            $scope.priceServiceExchangeRate = priceServiceExchangeRate;
        }
        $scope.calPriceExtrabed = function () {
            var priceExtrabed = 0;
            var priceExtrabedExchangeRate = 0;

            $scope.extrabeds.forEach(x => {
                if (x.NumberChoose > 0) {
                    if ($scope.currency == 'VND')
                        priceExtrabed += x.Price * x.NumberChoose;
                    else
                        priceExtrabedExchangeRate += x.PriceExchangeRate * x.NumberChoose;
                }
            });
            $scope.priceExtrabed = priceExtrabed;
            $scope.priceExtrabedExchangeRate = priceExtrabedExchangeRate;
        }
        $scope.calDeposit = function () {
            var priceDeposit = 0;
            var priceDepositExchangeRate = 0;
            $scope.data.forEach(x => {
                if (x.PromotionHome.ChooseRoom > 0) {
                    if ($scope.currency == 'VND')
                        priceDeposit += x.PromotionHome.ChooseRoom * x.PromotionHome.TotalPriceAfterPromotion * x.PromotionHome.AmountRate / 100;
                    else
                        priceDepositExchangeRate += x.PromotionHome.ChooseRoom * x.PromotionHome.TotalPriceAfterPromotionExchangeRate * x.PromotionHome.AmountRate / 100;
                }
            });
            $scope.priceDeposit = priceDeposit * (100 + $scope.vat + $scope.serviceCharge + $scope.vat * $scope.serviceCharge / 100) / 100
            $scope.priceDepositExchangeRate = priceDepositExchangeRate * (100 + $scope.vat + $scope.serviceCharge + $scope.vat * $scope.serviceCharge / 100) / 100;
        }

        //#region get item choose for book
        $scope.getServiceChoose = function () {
            var serviceChoose = [];
            $scope.services.forEach(x => {
                if (x.NumberChoose > 0) {
                    serviceChoose.push(x);
                }
            })
            $scope.serviceChoose = serviceChoose
        }
        $scope.getExtrabedChoose = function () {
            var extrabedChoose = [];
            $scope.extrabeds.forEach(x => {
                if (x.NumberChoose > 0) {
                    extrabedChoose.push(x);
                }
            });
            $scope.extrabedChoose = extrabedChoose;
        }
        // #endregion

        $scope.initpage = function () {
            var lang = helper.findGetParameter("lang");
            var currency = helper.findGetParameter("currency");
            if (lang != null)
                $scope.lang = lang;
            if (currency != null)
                $scope.currency = currency;
            $scope.getData()
            $scope.transitions = JSON.parse($("#transition").val())
        };
        $scope.initpage();
        $scope.getLanguage = function () {
            $http({
                url: urlGetLanguage,
                method: 'GET'
            }).then(function success(respone) {
                $scope.languages = respone.data;
            }, function error(respone) {
            });
        };
        $scope.getCurrency = function () {
            $http({
                url: urlGetCurrency,
                method: 'GET'
            }).then(function success(respone) {
                $scope.currencies = respone.data;
            }, function error(respone) {
            });
        };
        $scope.getLanguage();
        $scope.getCurrency();
        $scope.changeSearch = function () {
            var newLink = "/Home/SelectRoom?hotelKey=" + $scope.params.hotelKey +
                "&hotelCode=" + $scope.params.hotelCode +
                "&fromDate=" + $scope.params.fromDate +
                "&toDate=" + $scope.params.toDate +
                "&adults=" + $scope.params.adults +
                "&child=" + $scope.params.child +
                "&lang=" + $scope.lang +
                "&currency=" + $scope.currency;
            location.href = newLink;
        }
        $scope.changeLanguage = function (key) {
            $scope.lang = key;
            $scope.loadChangePage();
        };
        $scope.changeCurrency = function (currencyCode) {
            $scope.currency = currencyCode;
            $scope.loadChangePage();
        };
        $scope.loadChangePage = function () {
            var newLink = "http://" + location.host + location.pathname +
                "?hotelKey=" + $scope.params.hotelKey +
                "&hotelCode=" + $scope.params.hotelCode +
                "&fromDate=" + $scope.params.fromDate +
                "&toDate=" + $scope.params.toDate +
                "&adults=" + $scope.params.adults +
                "&child=" + $scope.params.child +
                "&lang=" + $scope.lang +
                "&currency=" + $scope.currency;
            location.href = newLink;
        }
        $scope.rangeNumber = function (from, to) {
            var arr = [];
            for (var i = from; i <= to; i++) {
                arr.push(i);
            }
            return arr
        }

        // voucher
        $scope.roomIdForDiscountByVoucher = [];
        $scope.amountForRoom = 0;
        $scope.GetAmountForVoucher = function () {
            if ($scope.voucher.voucherCode == null || $scope.voucher.voucherCode == "")
                return;
            $scope.getServiceChoose();
            $http({
                url: urlGetAmountForVoucher,
                method: 'POST',
                data: {
                    voucher: $scope.voucher.voucherCode,
                    hotelCode: $scope.params.hotelCode,
                    services: $scope.serviceChoose
                }
            }).then(function success(respone) {
                $scope.voucher.accept = respone.data.accept;
                $scope.voucher.amountForVoucher = respone.data.amountForVoucher;
                $scope.roomIdForDiscountByVoucher = respone.data.roomIdForDiscountByVoucher
                $scope.amountForRoom = respone.data.amountForRoom;
                if ($scope.amountForRoom > 0 && $scope.roomIdForDiscountByVoucher.length > 0) {
                    $scope.reCalculatorDeposit();
                }
            }, function error(respone) {
            });
        };
        $scope.reCalculatorDeposit = function () {
            var priceDeposit = 0;
            var priceDepositExchangeRate = 0;
            $scope.data.forEach(x => {
                var discountForRoom = 0;
                if ($scope.roomIdForDiscountByVoucher.findIndex(y => y == x.RoomTypeId) >= 0) {
                    discountForRoom = $scope.amountForRoom;
                }
                if (x.PromotionHome.ChooseRoom > 0) {
                    if ($scope.currency == 'VND')
                        priceDeposit += x.PromotionHome.ChooseRoom * x.PromotionHome.TotalPriceAfterPromotion * (100 - discountForRoom) * x.PromotionHome.AmountRate / 10000;
                    else
                        priceDepositExchangeRate += x.PromotionHome.ChooseRoom * x.PromotionHome.TotalPriceAfterPromotionExchangeRate * (100 - discountForRoom) * x.PromotionHome.AmountRate / 10000;
                }
            });
            $scope.priceDeposit = priceDeposit * (100 + $scope.vat + $scope.serviceCharge + $scope.vat * $scope.serviceCharge / 100) / 100
            $scope.priceDepositExchangeRate = priceDepositExchangeRate * (100 + $scope.vat + $scope.serviceCharge + $scope.vat * $scope.serviceCharge / 100) / 100;
        }
        $scope.removeVoucher = function () {
            $scope.voucher = {
                voucherCode: '',
                accept: -1,
                amountForVoucher: 0
            };
            $scope.calDeposit();
            $scope.roomIdForDiscountByVoucher = [];
            $scope.amountForRoom = 0;
        }
        $scope.refreshErrorVoucher = function () {
            $scope.voucher.accept = -1
        }
        // book
        $scope.saveDataForPayPal = function () {
            $scope.getServiceChoose();
            $scope.getExtrabedChoose();
            $scope.guest.TypePaymentMethodName = $("#typePaymentMethod_" + $scope.guest.TypePaymentMethod).text()
            $http({
                url: urlSaveDataForPayPal,
                method: 'POST',
                data: {
                    guest: $scope.guest,
                    extrabeds: $scope.extrabedChoose,
                    services: $scope.serviceChoose,
                    voucher: $scope.voucher
                }
            }).then(function success(respone) {
            }, function error(respone) {
            });
        }
        $scope.Book = function () {
            if ($scope.guest.TypePaymentMethod < 0) {
                alert($scope.translateMessage(2434, 'Please choose a payment method'));
                return
            }
            if (!$scope.acceptPolicy) {
                $("#err_checkCertify").addClass('errorAccept');
                return;
            }
            else {
                $("#err_checkCertify").removeClass('errorAccept')
            }
            var v1 = validate.isNullOrEmptySingleShowError({ value: $scope.guest.FirstName, key: 'FirstName' })
            var v2 = validate.isNullOrEmptySingleShowError({ value: $scope.guest.SurName, key: 'SurName' })
            var v3 = validate.isEmail({ value: $scope.guest.Email, key: 'Email' })
            var v4 = false;
            if ($scope.paymentMethodChoose.RequireCard) {
                if (validate.checkVisa($scope.guest.Number) ||
                    validate.checkMasterCard($scope.guest.Number) ||
                    validate.checkJcb($scope.guest.Number) ||
                    validate.checkDiners($scope.guest.Number) ||
                    validate.checkAmericalExpress($scope.guest.Number) ||
                    validate.checkDiscover($scope.guest.Number)) {
                    $("#err_Number").css("display", "none");
                }
                else {
                    v4 = true
                    $("#err_Number").css("display", "block");
                }
                var v5 = validate.isNullOrEmptySingleShowError({ value: $scope.guest.Name, key: 'Name' })
                var v6 = validate.isNullOrEmptySingleShowError({ value: $scope.guest.Code, key: 'Code' })
                if (v5 || v6) {
                    v4 = true
                }
            }
            if (v1 || v2 || v3 || v4) {
                alert($scope.translateMessage(2354, 'Invalid customer information'));
                return
            }
            $("#loader").css("display", "block");
            $scope.getServiceChoose();
            $scope.getExtrabedChoose();
            $scope.guest.TypePaymentMethodName = $("#typePaymentMethod_" + $scope.guest.TypePaymentMethod).text()
            $http({
                url: urlPost,
                method: 'POST',
                data: {
                    guest: $scope.guest,
                    extrabeds: $scope.extrabedChoose,
                    services: $scope.serviceChoose,
                    voucher: $scope.voucher
                }
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                location.href = respone.data;
            }, function error(respone) {
                $("#loader").css("display", "none")
            });
        }
        $scope.RefreshValidate = function () {
            $(".error").css("display", "none");
            $("#err_checkCertify").removeClass('errorAccept')
        }
        $scope.translateMessage = function (id, messageDefault) {
            var item = $scope.transitions.find(x => x.TransitionId == id);
            return item == null ? messageDefault : item.Result
        }
    }]);