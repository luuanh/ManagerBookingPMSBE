var urlDetailGuest = "/Admin/Guest/Detail"
var urlDetail = "/Admin/Order/Detail"
var urlPut = "/Admin/Order/Put"
var urlCloseOrder = "/Admin/Order/CloseOrder"

var urlGetServiceCategory = "/Admin/ServiceCategory/GetFull"
var urlGetServices = "/Admin/Service/GetAll"
// action
var urlGetReservationForClusters = "/Admin/Reservation/GetReservationForClusters"
var urlAssignOrderBooking = "/Admin/Order/AssignOrderBooking"

app.controller('controller', ['$scope', '$http', 'validate', 'notify', 'helper', '$timeout',
    function ($scope, $http, validate, notify, helper, $timeout) {
        $scope.Voucher = ""
        $scope.status = true
        $scope.serviceChoose = [];
        $scope.services = [];
        $scope.guest = {
            GuestId: -1,
            FirstName: '',
            SurName: '',
            Country: 'Vietnam',
            Phone: '',
            Email: '',
            Note: ''
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
            }, function error(respone) {
                $("#loader").css("display", "none")
            })
        }
        $scope.refreshGuest = function () {
            $scope.guest = {
                GuestId: -1,
                FirstName: '',
                SurName: '',
                Country: 'Vietnam',
                Phone: '',
                Email: '',
                Note: ''
            }
        }
        $scope.RefreshValidate = function () {
            $(".error").css("display", "none")
        }
        $scope.GetServiceCategory = function () {
            $http({
                url: urlGetServiceCategory,
                method: "GET"
            }).then(function success(response) {
                $scope.ServiceCategories = response.data
                $timeout(function () {
                    if ($scope.ServiceCategories != null && $scope.ServiceCategories.length > 0) {
                        $scope.changeTypeServiceCategory($scope.ServiceCategories[0].ServiceCategoryId)
                    }
                }, 200);
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
                    voucher: $scope.Voucher
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                $scope.services = response.data
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.addService = function (item) {
            if ($scope.status) return
            var itemClone = JSON.parse(JSON.stringify(item));
            var index = $scope.serviceChoose.findIndex(x => x.ServiceId == itemClone.ServiceId);
            if (index < 0) {
                itemClone.Number = 1;
                itemClone.RoomTypeIdIndex = "-1";
                $scope.serviceChoose.unshift(itemClone)
            }
            else
                $scope.serviceChoose[index].Number = ($scope.serviceChoose[index].Number - 0) + 1
        }
        $scope.removeService = function (index) {
            if ($scope.status) return
            $scope.serviceChoose.splice(index, 1)
        }
        $scope.calService_footer = function () {
            var total = 0;
            $scope.serviceChoose.forEach(x => {
                total += x.Price * x.Number
            })
            $scope.ServicePrice = total;
        }
        $scope.AutoCheckAccept_Guest = function () {
            var v1 = validate.isNullOrEmptySingleShowError({ value: $scope.guest.FirstName, key: 'FirstName' })
            var v2 = validate.isNullOrEmptySingleShowError({ value: $scope.guest.SurName, key: 'SurName' })
            var v4 = validate.isNullOrEmptySingleShowError({ value: $scope.guest.Phone, key: 'Phone' })
            if (v1 || v2 || v4) {
                alert("Thông tin khách hàng không hợp lệ!")
                return true
            }
            return false
        }
        $scope.Put = function () {
            if ($scope.serviceChoose == null || $scope.serviceChoose.length == 0) {
                alert("Chọn ít nhất một dịch vụ");
                return
            };
            if ($scope.AutoCheckAccept_Guest()) return;

            $("#loader").css("display", "block")
            $http({
                url: urlPut,
                method: "POST",
                data: {
                    guest: $scope.guest,
                    services: $scope.serviceChoose,
                    guestOrderId: $scope.guestOrderChoose
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                notify.success("Thành công");
            }, function error(response) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra!");
            });
        }
        $scope.GetServiceCategory()
        $scope.GetData = function () {
            $("#loader").css("display", "block")
            var id = helper.findGetParameter('id')
            $scope.guestOrderChoose = id
            $http({
                url: urlDetail,
                method: "GET",
                params: {
                    id: id
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                var data = response.data
                $scope.status = data.status;
                $scope.guest = data.guest;
                $scope.serviceChoose = data.services
                $scope.GuestOrderCode = data.GuestOrderCode
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.GetData()

        //#region action
        $scope.confirmCloseOrder = function () {
            $scope.payCloseOrder = $scope.ServicePrice
            UIkit.modal("#ModalCloseOrder").show()
        }
        $scope.CloseOrder = function () {
            if (parseFloat($scope.ServicePrice) != parseFloat($scope.payCloseOrder)) {
                alert("Số tiền phải thanh toán phải bằng tổng tiền hóa đơn.")
                return
            }
            $("#loader").css("display", "block")
            $http({
                url: urlCloseOrder,
                method: "POST",
                data: {
                    id: $scope.guestOrderChoose
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                $scope.GetData()
                UIkit.modal("#ModalCloseOrder").tryhide()
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }

        $scope.getStatus = function (status) {
            return statusReservation[(status - 0) - 1]
        }
        $scope.getTypeReservation = function (type) {
            return typeReservation[type]
        }
        $scope.showAssignToReservation = function () {
            $scope.ClustersReservation = {}
            UIkit.modal("#ModalAssignReservation").show()
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
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.AssignOrderToBooking = function (bookingId) {
            $("#loader").css("display", "block")
            $http({
                url: urlAssignOrderBooking,
                method: "POST",
                data: {
                    bookingId: bookingId,
                    guestOrderId: $scope.guestOrderChoose
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                location.href = "/Admin/Order/Index"
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        //#endregion
    }])