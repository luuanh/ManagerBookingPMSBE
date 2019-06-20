var urlGet = "/Setting/GetRenewalRegistration";
var urlPut = "/Setting/PutRenewalRegistration";
var urlGetLanguage = "/Language/GetActive";

app.controller('controller', ['$scope', '$http',
    function ($scope, $http) {
        $scope.GetData = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlGet,
                method: "GET",
                params: {
                    languageId: $scope.languageId
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                $scope.data = response.data;
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.GetLanguage = function () {
            $http({
                url: urlGetLanguage,
                method: "GET"
            }).then(function success(response) {
                $scope.language = response.data;
                $scope.languageId = $scope.language[0].LanguageId;
                $scope.GetData();
            }, function error(response) {

            });
        }
        $scope.Put = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlPut,
                method: "POST",
                data: $scope.data
            }).then(function success(response) {
                alert("Thành công")
                $("#loader").css("display", "none")
            }, function error(response) {
                $("#loader").css("display", "none")
                alert("Có lỗi sảy ra!")
            });
        }
        $scope.GetLanguage()
    }]);