var urlGetLanguage = '/Home/GetLanguage';
var urlGetCurrency = '/Home/GetCurrency';
var urlGetData = '/Home/GetRoomAvailable';
var urlDetailRoomType = '/Home/DetailRoomType';
var urlSaveDataBooking = '/Home/SaveInfoBooking'

app.controller("controller", ['$scope', '$http', '$timeout', 'helper',
    function ($scope, $http, $timeout, helper) {
        $scope.lang = 'vi';
        $scope.currency = 'VND';
        $scope.params = {};
        $scope.data = [];
        $scope.dataRoomBook = [];
        $scope.data1 = [];
        $scope.data2 = [];
        $scope.detailRoomChosen = [];
        $scope.totalRoomBook = 0;
        $scope.totalPriceRoom = [];

        $scope.rangeArr = function (number) {
            var arr = [];
            for (var i = 0; i < number; i++) {
                arr.push(i);
            }
            return arr;
        }
        $scope.rangeArr2 = function (number) {
            var arr = [];
            for (var i = 0; i <= number; i++) {
                arr.push(i);
            }
            return arr;
        }
        $scope.getData = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlGetData,
                method: 'GET',
                params: {
                    hotelCode: $scope.params.hotelCode,
                    fromDate: $scope.params.fromDate,
                    toDate: $scope.params.toDate,
                    lang: $scope.lang,
                    currency: $scope.currency
                }
            }).then(function success(respone) {
               
                $("#loader").css("display", "none");
                $scope.data = respone.data;
                $scope.data = JSON.parse(respone.data);

             
                for (var i = 0; i < $scope.data.length ; i++) {
                   
                    $scope.data1[i] = $scope.data[i];
                    for (var j = i + 1; j < $scope.data.length; j++) {
                        if ($scope.data[i].RoomTypeId == $scope.data[j].RoomTypeId) {
                            $scope.data2[i] = $scope.data[j];
                            i = j;
                        }
                        else {
                          
                            break;
                        }
                    }
                }
               

                console.log($scope.data);
                console.log($scope.data1);
                console.log($scope.data2);
                //var tem = $scope.data;
                //angular.forEach(respone.data, function (item, index) {
                //    console.log(item.RoomTypeId);
                //});

                

                
            }, function error(respone) {
                $("#loader").css("display", "none")
            });

        }

       

        $scope.initpage = function () {
            var lang = helper.findGetParameter("lang");
            var currency = helper.findGetParameter("currency");
            $scope.params.child = helper.findGetParameter("child");
            $scope.params.adults = helper.findGetParameter("adults");
            $scope.params.toDate = helper.findGetParameter("toDate");
            $scope.params.fromDate = helper.findGetParameter("fromDate");
            $scope.params.hotelCode = helper.findGetParameter("hotelCode");
            $scope.params.hotelKey = helper.findGetParameter("hotelKey");
            if (lang != null)
                $scope.lang = lang;
            $scope.params.lang = $scope.lang
            if (currency != null)
                $scope.currency = currency;
            $scope.getData()
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
            var newLink = "/?hotelKey=" + $scope.params.hotelKey +
                "&hotelCode=" + $scope.params.hotelCode +
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
        $scope.showDetailRoom = function (roomTypeId) {
            $("#loader").css("display", "block")
            $http({
                url: urlDetailRoomType,
                method: 'GET',
                params: {
                    id: roomTypeId
                }
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                $scope.detailRoom = respone.data;
                console.log($scope.detailRoom)
                $timeout(function () {
                    currentSlide(1)
                }, 100)
                $("#ui-id-2").html($scope.detailRoom.roomType.RoomTypeName)
                $('.detail-info-room').dialog('open');
            }, function error(respone) {
                $("#loader").css("display", "none")
            });
        }
        $scope.calRoomChoose = function () {
            var arrData = [];
            var totalRoomBook = 0;
            var detailRoomChosen = [];
            var totalPriceRoom = 0;
            var totalPriceRoomExchangeRate = 0;
            $scope.data.forEach(x => {
                var totalRoomChoose = 0;
                if (x.PromotionHome.ChooseRoom > 0) {
                    arrData.push(x);
                    totalRoomChoose += x.PromotionHome.ChooseRoom;
                    totalPriceRoom += x.PromotionHome.ChooseRoom * x.PromotionHome.TotalPriceAfterPromotion;
                    if ($scope.currency != 'VND')
                        totalPriceRoomExchangeRate += x.PromotionHome.ChooseRoom * x.PromotionHome.TotalPriceAfterPromotionExchangeRate;
                };
                x.TotalRoomChoose = totalRoomChoose;
                totalRoomBook += totalRoomChoose;
            });
            $scope.dataRoomBook = arrData;
            $scope.totalRoomBook = totalRoomBook;
            $scope.dataRoomBook.forEach(x => {
                var roomtype = detailRoomChosen.find(y => y.RoomTypeId == x.RoomTypeId);
                if (roomtype == null) {
                    detailRoomChosen.push(x);
                }
                else {
                    roomtype.TotalRoomChoose += x.PromotionHome.ChooseRoom
                }
            });
            $scope.detailRoomChosen = detailRoomChosen;
            //console.log(detailRoomChosen);
            $scope.totalPriceRoom = totalPriceRoom;
            $scope.totalPriceRoomExchangeRate = totalPriceRoomExchangeRate;
        };
        $scope.Confirm = function () {
       //$("#loader").css("display", "block")
            $scope.params.currency = $scope.currency;

            var x1 = parseFloat($scope.params.child);
            var x2 = parseFloat($scope.params.adults);
            var total = x1 + x2;
            var room = parseInt($scope.totalRoomBook);
            var detailRoomChosen = [];

            for (var i = 0; i < $scope.data.length; i++) {
                var people = $scope.data[i];            
            }

            var arrData = [];
            var totalRoomBook = 0;
            var totalPriceRoom = 0;
            var totalPriceRoomExchangeRate = 0;
            $scope.data.forEach(x => {
                var totalRoomChoose = 0;
                if (x.PromotionHome.ChooseRoom > 0) {
                    arrData.push(x);
                    totalRoomChoose += x.PromotionHome.ChooseRoom;
                    totalPriceRoom += x.PromotionHome.ChooseRoom * x.PromotionHome.TotalPriceAfterPromotion;
                    if ($scope.currency != 'VND')
                        totalPriceRoomExchangeRate += x.PromotionHome.ChooseRoom * x.PromotionHome.TotalPriceAfterPromotionExchangeRate;
                };
                x.TotalRoomChoose = totalRoomChoose;
                totalRoomBook += totalRoomChoose;
            });
            $scope.dataRoomBook = arrData;
            $scope.totalRoomBook = totalRoomBook;
            $scope.dataRoomBook.forEach(x => {
                var roomtype = detailRoomChosen.find(y => y.RoomTypeId == x.RoomTypeId);
                if (roomtype == null) {
                    detailRoomChosen.push(x);
                }
                else {
                    roomtype.TotalRoomChoose += x.PromotionHome.ChooseRoom
                }
            });

            for (var j = 0; j < $scope.detailRoomChosen.length; j++) {
                var roomtypes = detailRoomChosen[j];
                var roomtest = roomtypes.RoomTypeId;
            }

            var maxpeople = roomtypes.MaxPeople;
            var test = parseFloat(total / maxpeople);   
           
            if (test > room) {
                if ($scope.currency == 'VND') {
                    swal({
                        title: "Thông báo",
                        text: "Bạn chưa chọn đủ số lượng phòng tương ứng số người !",
                        icon: "warning",
                        buttons: true,
                        dangerMode: true,
                    })
                        .then((willDelete) => {

                            if (willDelete) {
                                swal("Yêu cầu đặt phòng của bạn đã được xác nhận", {
                                    icon: "success",
                                }).then((willOK) => {
                                    if (willOK) {
                                        $http({
                                            url: urlSaveDataBooking,
                                            method: 'POST',
                                            data: {
                                                data: $scope.dataRoomBook,
                                                param: $scope.params
                                            }
                                        }).then(function success(respone) {
                                            $("#loader").css("display", "none")

                                            location.href = "/Home/Confirm?lang=" + $scope.lang +
                                                "&currency=" + $scope.currency;
                                        }, function error(respone) {
                                            $("#loader").css("display", "none");
                                        });
                                    }
                                })
                            } else {
                                swal.close();
                                $("#loader").css("display", "none");
                            }
                        });
                } else {
                    swal({
                        title: "Notification",
                        text: "You have not selected a sufficient number of rooms corresponding to the number of people !",
                        icon: "warning",
                        buttons: true,
                        dangerMode: true,
                    })
                        .then((willDelete) => {

                            if (willDelete) {
                                swal("Your booking request has been confirmed", {
                                    icon: "success",
                                }).then((willOK) => {
                                    if (willOK) {
                                        $http({
                                            url: urlSaveDataBooking,
                                            method: 'POST',
                                            data: {
                                                data: $scope.dataRoomBook,
                                                param: $scope.params
                                            }
                                        }).then(function success(respone) {
                                            $("#loader").css("display", "none")

                                            location.href = "/Home/Confirm?lang=" + $scope.lang +
                                                "&currency=" + $scope.currency;
                                        }, function error(respone) {
                                            $("#loader").css("display", "none");
                                        });
                                    }
                                })
                            } else {
                                swal.close();
                                $("#loader").css("display", "none");
                            }
                        });
                }

                
                
                //alert("Bạn chưa chọn đủ số lượng phòng tương ứng số người !");
            } else  {
                $http({
                    url: urlSaveDataBooking,
                    method: 'POST',
                    data: {
                        data: $scope.dataRoomBook,
                        param: $scope.params
                    }
                }).then(function success(respone) {


                    $("#loader").css("display", "none")
                    location.href = "/Home/Confirm?lang=" + $scope.lang +
                        "&currency=" + $scope.currency;
                }, function error(respone) {
                    $("#loader").css("display", "none");
                });
            }
           
        };
    }]);