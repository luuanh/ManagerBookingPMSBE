var urlGet = "/Admin/TemplateEmail/Get"
var urlGetTypeTemplate = "/Admin/TemplateEmail/GetTypeTemplate"
var urlPut = "/Admin/TemplateEmail/Put"

app.controller('controller', ['$scope', '$http', 'validate', 'notify','$timeout',
    function ($scope, $http, validate, notify, $timeout) {
        $scope.filter = {
            typeEmailId: -1
        }
        $scope.GetData = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlGet,
                method: "GET",
                params: $scope.filter
            }).then(function success(response) {
                $("#loader").css("display", "none")
                $scope.data = response.data;
                CKEDITOR.instances.editor.setData($scope.data.Content)
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.GetTypeTemplate = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlGetTypeTemplate,
                method: "GET"
            }).then(function success(response) {
                $("#loader").css("display", "none")
                $scope.typeEmails = response.data;
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.Put = function () {
            $("#loader").css("display", "block")
            $scope.data.Content = CKEDITOR.instances.editor.getData()
            $http({
                url: urlPut,
                method: "POST",
                data: $scope.data
            }).then(function success(response) {
                $("#loader").css("display", "none")
                notify.success("Thành công");
            }, function error(response) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra!");
            });
        }
        $scope.GetTypeTemplate()
    }])