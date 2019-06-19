var urlGet = "/Admin/Profile/Detail"
var urlPut = "/Admin/Profile/Put"
var urlChangePassword = "/Admin/Profile/ChangePassword"

app.controller('controller', ['$scope', '$http', 'validate', 'notify',
    '$timeout',
    function ($scope, $http, validate, notify, $timeout) {
        $scope.Put = function () {
            var v2 = validate.isNotNumberSingleShowError({ value: $scope.policy.Index, key: 'Index' })
            if (v2 || !v1) {
                notify.error('Giá trị không hợp lệ!')
                return
            }
            $("#loader").css("display", "block")
            $scope.GetAdditionInfoMultiLanguage()
            $http({
                method: "POST",
                url: urlPut,
                data: $scope.policy
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                $scope.GetData()
                notify.success("Thành công")
                $scope.CloseWindow()
            }, function error(respone) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra!")
            })
        }
        $scope.RefreshValidate = function () {
            $(".error").css("display", "none")
        }
        $scope.GetData = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlGet,
                method: "GET",
                params: $scope.filters
            }).then(function success(response) {
                $("#loader").css("display", "none")
                var data = response.data;
                $scope.user = data
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.Put = function () {
            $("#loader").css("display", "block")
            $scope.user.Photo = $("#Logo").val()
            $http({
                url: urlPut,
                method: "POST",
                data: {
                    user: $scope.user
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                notify.success("Thành công");
            }, function error(response) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra!");
            });
        }
        $scope.ChangePassword = function () {
            var v1 = validate.isNullOrEmptySingleShowError({ value: $scope.user.Password, key: 'Password' })
            var v2 = validate.isNullOrEmptySingleShowError({ value: $scope.user.NewPassword, key: 'NewPassword' })
            var v3 = validate.isNullOrEmptySingleShowError({ value: $scope.user.ConfirmPassword, key: 'ConfirmPassword' })
            if (v1 || v2 || v3) {
                notify.error('Giá trị không hợp lệ!')
                return
            }
            if ($scope.user.NewPassword != $scope.user.ConfirmPassword) {
                notify.error('Mật khẩu không khớp')
                return
            }
            $http({
                url: urlChangePassword,
                method: "POST",
                data: {
                    user: $scope.user
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                var result = response.data;
                if (result == -1)
                    notify.error('Mật khẩu không khớp')
                else if (result == -2)
                    notify.error('Mật khẩu không hợp lệ')
                else if (result == 1)
                    notify.success("Thành công");
                $scope.user.Password = '';
                $scope.user.NewPassword = '';
                $scope.user.ConfirmPassword = '';
            }, function error(response) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra");
            });
        }
        $scope.GetData()
    }]);