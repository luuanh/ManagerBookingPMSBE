var urlGet = "/Admin/ConfigEmail/Get"
var urlPut = "/Admin/ConfigEmail/Put"

app.controller('controller', ['$scope', '$http', 'validate', 'notify',
    function ($scope, $http, validate, notify) {
        $scope.Put = function () {
            var v1 = validate.isEmail({ value: $scope.data.Email, key: 'Email' })
            var v2 = validate.isEmail({ value: $scope.data.EmailReceive, key: 'EmailReceive' })
            var v3 = validate.isNullOrEmptySingleShowError({ value: $scope.data.SubjectOffline, key: 'SubjectOffline' })
            var v4 = validate.isNullOrEmptySingleShowError({ value: $scope.data.Password, key: 'Password' })
            if (v1 || v2 || v3 || v4 ) {
                notify.error('Giá trị không hợp lệ!')
                return
            }
            $("#loader").css("display", "block")
            $http({
                method: "POST",
                url: urlPut,
                data: {
                    configEmail: $scope.data
                }
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                notify.success("Thành công")
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
                method: "GET"
            }).then(function success(response) {
                $("#loader").css("display", "none")
                $scope.data = response.data;
            }, function error(response) {
                $("#loader").css("display", "none")

            });
        }
        $scope.GetData()
    }])