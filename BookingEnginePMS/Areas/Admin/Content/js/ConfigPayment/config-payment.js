var urlGet = "/Admin/ConfigPaymentMethod/Get"
var urlPut = "/Admin/ConfigPaymentMethod/Put"
var urlPutPolicy = "/Admin/ConfigPaymentMethod/PutPolicy"
var urlGetPolicy = "/Admin/ConfigPaymentMethod/GetPolicy"
var urlGetLanguage = "/Admin/Language/GetActive"

app.controller('controller', ['$scope', '$http', 'validate', 'notify',
    function ($scope, $http, validate, notify) {
        $scope.idChoose = 0;
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
        $scope.Put = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlPut,
                method: "POST",
                data: $scope.data
            }).then(function success(response) {
                $("#loader").css("display", "none")
                notify.success("Thành công ");
                $scope.GetData()
            }, function error(response) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra!");
            });
        }
        $scope.dataDetail = []
        $scope.languages = []

        $scope.ChangeLanguage = function () {
            var indexOld = $scope.dataDetail.findIndex(x => x.LanguageId == $scope.idLanguageChooseOld);
            if (indexOld >= 0)
                $scope.dataDetail[indexOld].Policy = CKEDITOR.instances.contentPolicy.getData()

            var index = $scope.dataDetail.findIndex(x => x.LanguageId == $scope.languageChoose);
            if (index >= 0)
                CKEDITOR.instances.contentPolicy.setData($scope.dataDetail[index].Policy)
            $scope.idLanguageChooseOld = $scope.languageChoose
        }

        $scope.EditPolicy = function (id) {
            $scope.idChoose = id;
            $scope.OpenWindow()
            $("#loader").css("display", "block")
            $http({
                url: urlGetPolicy,
                method: 'GET',
                params: {
                    id: id
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                $scope.dataDetail = response.data;
                $scope.languageChoose = $scope.languages[0].LanguageId
                $scope.idLanguageChooseOld = $scope.languageChoose;
                CKEDITOR.instances.contentPolicy.setData($scope.dataDetail[0].Policy)
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }
        $scope.PutPolicy = function () {
            $("#loader").css("display", "block")
            var index = $scope.dataDetail.findIndex(x => x.LanguageId == $scope.languageChoose);
            $scope.dataDetail[index].Policy = CKEDITOR.instances.contentPolicy.getData()
            $http({
                url: urlPutPolicy,
                method: 'POST',
                data: {
                    id: $scope.idChoose,
                    data: $scope.dataDetail
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                $scope.CloseWindow()
            }, function error(response) {
                $("#loader").css("display", "none")
            });
        }

        if (CKEDITOR.env.ie && CKEDITOR.env.version < 9)
            CKEDITOR.tools.enableHtml5Elements(document);
        CKEDITOR.config.height = 300;
        CKEDITOR.config.width = 'auto';
        $scope.initSample = (function () {
            var wysiwygareaAvailable = isWysiwygareaAvailable(),
                isBBCodeBuiltIn = !!CKEDITOR.plugins.get('bbcode');

            return function () {
                var editorElement = CKEDITOR.document.getById('contentPolicy');
                if (isBBCodeBuiltIn) {
                    editorElement.setHtml('');
                }
                if (wysiwygareaAvailable) {
                    CKEDITOR.replace('contentPolicy');
                } else {
                    editorElement.setAttribute('contenteditable', 'true');
                    CKEDITOR.inline('contentPolicy');
                }
            };

            function isWysiwygareaAvailable() {
                if (CKEDITOR.revision == ('%RE' + 'V%')) {
                    return true;
                }
                return !!CKEDITOR.plugins.get('wysiwygarea');
            }
        })();
        $scope.initSample()
        $scope.OpenWindow = function () {
            $("#kUI_window").data("kendoWindow").maximize().open();
        }
        $scope.CloseWindow = function () {
            $("#kUI_window").data("kendoWindow").close();
        }
        $scope.GetLanguage = function () {
            $http({
                url: urlGetLanguage,
                method: "GET"
            }).then(function success(response) {
                $scope.languages = response.data;
            }, function error(response) {

            });
        }
        $scope.GetLanguage()
        $scope.updateSelection = function (position, data) {
            angular.forEach(data, function (item, index) {
                if (item.ConfigPaymentMethodId == 13 || item.ConfigPaymentMethodId == 14) {
                    if (position != index)
                        item.Active = false;
                }
               
            });
        }

       
    }])
