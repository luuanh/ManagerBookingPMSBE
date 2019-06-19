var urlGetLanguage = '/Home/GetLanguage';
var urlPayInvoice = '/Home/PayInvoice';

app.controller("controller", ['$scope', '$http', '$timeout', 'helper', 'validate',
    function ($scope, $http, $timeout, helper, validate) {
        $scope.guest = {
            ExpirationMonth: -1,
            ExpirationYear: -1,
            Name: '',
            Number: '',
            Code: '',
            TypePaymentMethod: -1
        }
        $scope.paymentMethodChoose = {}
        $scope.acceptPolicy = false;
        $scope.changePaymentMethod = function () {
            $scope.paymentMethodChoose = $scope.paymentMethods.find(x => x.ConfigPaymentMethodId == $scope.guest.TypePaymentMethod);
        }
        $scope.getLanguage = function () {
            $http({
                url: urlGetLanguage,
                method: 'GET'
            }).then(function success(respone) {
                $scope.languages = respone.data;
            }, function error(respone) {
            });
        };
        $scope.initpage = function () {
            var lang = helper.findGetParameter("lang");
            if (lang == null || lang == '')
                $scope.lang = 'vi';
            else
                $scope.lang = lang
            $scope.code = helper.findGetParameter("code");
            $scope.email = helper.findGetParameter("email");
            $scope.transitions = JSON.parse($("#transition").val())
            $scope.paymentMethods = JSON.parse($("#firstPaymentMethod").val());
            $scope.paymentMethodChoose = $scope.paymentMethods[0];
            $scope.guest.TypePaymentMethod = $scope.paymentMethods[0].ConfigPaymentMethodId
            $scope.getLanguage()
        };
        $scope.initpage();
        $scope.rangeNumber = function (from, to) {
            var arr = [];
            for (var i = from; i <= to; i++) {
                arr.push(i);
            }
            return arr
        }
        $scope.changeLanguage = function (key) {
            $scope.lang = key;
            $scope.loadChangePage();
        };
        $scope.loadChangePage = function () {
            var newLink = "/Home/Invoice?code=" + $scope.code +
                "&email=" + $scope.email +
                "&lang=" + $scope.lang
            location.href = newLink;
        }
        $scope.Book = function () {
            if (!$scope.acceptPolicy) {
                $("#err_checkCertify").addClass('errorAccept');
                return;
            }
            else {
                $("#err_checkCertify").removeClass('errorAccept')
            }
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
                if (v5 || v6 || $scope.guest.ExpirationMonth < 0 || $scope.guest.ExpirationYear < 0) {
                    v4 = true
                }
            }
            if (v4) {
                alert($scope.translateMessage(2354, 'Invalid customer information'));
                return
            }
            $("#loader").css("display", "block");
            $scope.guest.TypePaymentMethodName = $("#typePaymentMethod_" + $scope.guest.TypePaymentMethod).text()
            $http({
                url: urlPayInvoice,
                method: 'POST',
                data: {
                    guest: $scope.guest
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