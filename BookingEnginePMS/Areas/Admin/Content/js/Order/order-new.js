var urlDetailGuest = "/Admin/Guest/Detail"
var urlPost = "/Admin/Order/Post"

var urlGetServiceCategory = "/Admin/ServiceCategory/GetFull"
var urlGetServices = "/Admin/Service/GetAll"

app.controller('controller', ['$scope', '$http', 'validate', 'notify', 'helper', '$timeout',
    function ($scope, $http, validate, notify, helper, $timeout) {
        $scope.Voucher = ""
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
        $scope.Post = function () {
            if ($scope.serviceChoose == null || $scope.serviceChoose.length == 0) {
                alert("Chọn ít nhất một dịch vụ");
                return
            };
            if ($scope.AutoCheckAccept_Guest()) return;

            $("#loader").css("display", "block")
            $http({
                url: urlPost,
                method: "POST",
                data: {
                    guest: $scope.guest,
                    services: $scope.serviceChoose
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                notify.success("Thành công");
                location.href = "/Admin/Order/Index"
            }, function error(response) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra!");
            });
        }
        $scope.GetServiceCategory()
    }])