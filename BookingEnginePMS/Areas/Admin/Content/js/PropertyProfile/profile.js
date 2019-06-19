var urlPut = "/Admin/PropertyProfile/Put"
var urlDetail = "/Admin/PropertyProfile/Detail"
var urlGetLanguage = "/Admin/Language/GetActive"

app.controller('controller', ['$scope', '$http', '$log', 'uiGridConstants', 'validate', 'notify',
    '$timeout',
    function ($scope, $http, $log, uiGridConstants, validate, notify, $timeout) {
        $scope.language = []
        $scope.languageChoose = 0;
        
        $scope.Put = function () {
            var v1 = validate.isNullOrEmptySingleShowError({ value: $scope.hotel.Name, key: 'Name' });
            var v2 = validate.isNullOrEmptySingleShowError({ value: $scope.hotel.Code, key: 'Code' })
            var v3 = validate.isNullOrEmptySingleShowError({ value: $scope.hotel.Email, key: 'Email' })
            var v4 = validate.isNullOrEmptySingleShowError({ value: $scope.hotel.Address, key: 'Address' })
            var v5 = validate.isNullOrEmptySingleShowError({ value: $scope.hotel.Phone, key: 'Phone' })
            if (v1 || v2 || v3 || v4 || v5) {
                notify.error('Giá trị không hợp lệ!')
                return
            }
            $("#loader").css("display", "block")
            $scope.hotel.Logo = $("#Logo").val();
            $scope.GetAdditionInfoMultiLanguage()
            $http({
                method: "POST",
                url: urlPut,
                data: $scope.hotel
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
        $scope.GetAdditionInfoMultiLanguage = function (index = -1) {
            if (index < 0)
                index = $scope.languageChoose
            var indexOld = $scope.hotel.hotelLanguages.findIndex(x => x.LanguageId == index);
            var objOld = $scope.hotel.hotelLanguages[indexOld]
            if (objOld != null) {
                objOld.Terms = CKEDITOR.instances.terms.getData()
                objOld.InforAccount = CKEDITOR.instances.infoAccountHotel.getData()
                objOld.Note = CKEDITOR.instances.noteHotel.getData()
            }
        }
        $scope.SetAdditionInfoMultiLanguage = function (index) {
            if ($scope.hotel == null)
                return
            var indexNew = $scope.hotel.hotelLanguages.findIndex(x => x.LanguageId == index);
            var objNew = $scope.hotel.hotelLanguages[indexNew]
            if (objNew != null) {
                CKEDITOR.instances.terms.setData(objNew.Terms)
                CKEDITOR.instances.infoAccountHotel.setData(objNew.InforAccount)
                CKEDITOR.instances.noteHotel.setData(objNew.Note)
            }
        }
        $scope.ResetEditor = function () {
            CKEDITOR.instances.terms.setData('')
            CKEDITOR.instances.infoAccountHotel.setData('')
            CKEDITOR.instances.noteHotel.setData('')
        }
        $scope.$watch('languageChoose', function (newValue, oldValue) {
            if (newValue < 0)
                return
            if (oldValue > 0)
                $scope.GetAdditionInfoMultiLanguage(oldValue)
            $scope.SetAdditionInfoMultiLanguage(newValue)
        });
        

        if (CKEDITOR.env.ie && CKEDITOR.env.version < 9)
            CKEDITOR.tools.enableHtml5Elements(document);
        CKEDITOR.config.height = 300;
        CKEDITOR.config.width = 'auto';
        $scope.initSample = (function () {
            var wysiwygareaAvailable = isWysiwygareaAvailable(),
                isBBCodeBuiltIn = !!CKEDITOR.plugins.get('bbcode');

            return function () {
                var editorElement = CKEDITOR.document.getById('infoAccountHotel');
                var editorElement2 = CKEDITOR.document.getById('noteHotel');
                var editorElement3 = CKEDITOR.document.getById('terms');
                if (isBBCodeBuiltIn) {
                    editorElement.setHtml('');
                    editorElement2.setHtml('');
                    editorElement3.setHtml('');
                }
                if (wysiwygareaAvailable) {
                    CKEDITOR.replace('infoAccountHotel');
                    CKEDITOR.replace('noteHotel');
                    CKEDITOR.replace('terms');
                } else {
                    editorElement.setAttribute('contenteditable', 'true');
                    CKEDITOR.inline('infoAccountHotel');
                    editorElement2.setAttribute('contenteditable', 'true');
                    CKEDITOR.inline('noteHotel');
                    editorElement3.setAttribute('contenteditable', 'true');
                    CKEDITOR.inline('terms');
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
        $scope.GetData = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlDetail,
                method: "GET"
            }).then(function success(response) {
                $("#loader").css("display", "none")
                $scope.hotel = response.data
                var idLanguageChoose = $scope.language[0].LanguageId
                $timeout(function () {
                    $scope.SetAdditionInfoMultiLanguage(idLanguageChoose);
                }, 500);
                $scope.languageChoose = idLanguageChoose
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
                $scope.GetData()
            }, function error(response) {

            });
        }
        $scope.GetLanguage()
    }]);