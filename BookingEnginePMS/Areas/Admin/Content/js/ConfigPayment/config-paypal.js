var urlConfirmConnection = "/Admin/ConfigPaymentMethod/GetConfirmConnection"

app.controller('controller', ['$scope', '$http',
    function ($scope, $http) {
        $scope.confirmConnection = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlConfirmConnection,
                method: 'GET'
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                if (respone.data == 1) {
                    alert("Connection successful");
                    location.reload();
                }
                else if(respone.data == 0) {
                    alert("Email not confirmed");
                }
                else {
                    alert("An error occurred");
                }
            }, function error(respone) {
                $("#loader").css("display", "none")
            })
        }
    }]);