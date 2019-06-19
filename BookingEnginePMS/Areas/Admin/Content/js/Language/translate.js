var urlGetTranslate = "/Admin/Language/GetTranslate"
var urlGetScreen = "/Admin/Language/GetScreen"
var urlGetLanguageActive = "/Admin/Language/GetActive"
var urlPutCellTranlate = "/Admin/Language/PutCellTranlate"
var urlPutAllCellTranlate = "/Admin/Language/PutAllCellTranlate"

app.controller('controller', ['$scope', '$http', 'notify',
    function ($scope, $http, notify) {
        $scope.filters = {
            languageId: 1,
            screenId: 1,
            keySearch: ''
        }
        $scope.ShowAdd = function () {
        }
        $scope.GetLanguageActive = function () {
            $http({
                url: urlGetLanguageActive,
                method: "GET"
            }).then(function success(respone) {
                $scope.languages = respone.data
                $scope.filters.languageId = $scope.languages[0].LanguageId
                $scope.GetData()
            }, function error(respone) {

            })
        }
        $scope.GetScreen = function () {
            $http({
                url: urlGetScreen,
                method: "GET"
            }).then(function success(respone) {
                $scope.screens = respone.data
            }, function error(respone) {

            })
        }
        $scope.GetData = function () {
            $http({
                url: urlGetTranslate,
                method: "GET",
                params: $scope.filters
            }).then(function success(respone) {
                $scope.data = respone.data
            }, function error(respone) {

            })
        }
        $scope.PutCellTranslate = function (item) {
            item.LanguageId = $scope.filters.languageId
            $http({
                url: urlPutCellTranlate,
                method: "POST",
                data: {
                    transition: item
                }
            }).then(function success(respone) {
                notify.success("Thành công");
            }, function error(respone) {
                notify.error("Có lỗi sảy ra!")
            })
        }
        $scope.PutAllCellTranslate = function (item) {
            $http({
                url: urlPutAllCellTranlate,
                method: "POST",
                data: {
                    transitions: $scope.data
                }
            }).then(function success(respone) {
                notify.success("Thành công");
            }, function error(respone) {
                notify.error("Có lỗi sảy ra!")
            })
        }
        $scope.GetLanguageActive()
        $scope.GetScreen()
    }])