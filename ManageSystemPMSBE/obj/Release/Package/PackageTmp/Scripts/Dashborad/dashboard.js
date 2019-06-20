var urlGet = "/Home/GetDashboard"

app.controller('controller', ['$scope', '$http',
    function ($scope, $http) {
        $scope.GetData = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlGet,
                method: "GET"
            }).then(function success(response) {
                $("#loader").css("display", "none")
                $scope.data = response.data
            }, function error(response) {
                $("#loader").css("display", "none")

            });
        }
        $scope.GetData()
    }])