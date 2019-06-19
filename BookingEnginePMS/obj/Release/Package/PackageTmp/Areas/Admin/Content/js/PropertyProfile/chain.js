var urlGet = "/Admin/PropertyProfile/Get"
var urlPost = "/Admin/PropertyProfile/Post"
var urlPut = "/Admin/PropertyProfile/Put"
var urlDetail = "/Admin/PropertyProfile/Detail"
var urlGetLanguage = "/Admin/Language/GetActive"

app.controller('controller', ['$scope', '$http', '$log', 'uiGridConstants', 'validate', 'notify',
    '$timeout',
    function ($scope, $http, $log, uiGridConstants, validate, notify, $timeout) {
        $scope.filters = {
            pageNumber: 1,
            pageSize: 100,
            keySearch: ''
        }
        $scope.hotel = {
            Logo: '/FileDefault/image/img-default.gif',
            Name: '',
            Code: '',
            Email: '',
            Address: '',
            Phone: '',
            Hotline: '',
            Fax: '',
            Website: '',
            Facebook: '',
            Skyper: '',
            Google: '',
            Youtobe: '',
            HighLight: false,
            hotelLanguages: []
        }
        $scope.hotellanguage = []
        $scope.language = []
        $scope.languageChoose = 0;
        $scope.isAdd = true

        var columnDef = [
            { displayName: "Tên", name: 'Name', width: '*', minWidth: 150 },
            { displayName: "Mã", name: 'Code', width: 100 },
            {
                displayName: "Logo",
                name: 'Logo',
                width: 100,
                cellTemplate: '<div class="ui-grid-cell-contents box-image" style="text-align:center"><img ng-src="{{grid.appScope.ShowImage(row.entity.Logo)}}" /></div>',
            },
            {
                displayName: "Thao tác", name: "#", width: 100, field: "#",
                cellTemplate: '<div class="ui-grid-cell-contents box-control" style="text-align:center"><a href="javascript:void(0);" class="link-control" title="Sửa" ng-click="grid.appScope.Edit(row.entity.HotelId)">Sửa</a></div>',
            }
        ];
        $scope.gridOptions = {
            paginationPageSizes: [10, 20, 50, 100, 200, 500, 1000, 2000, 5000, 10000],
            paginationPageSize: $scope.filters.pageSize,
            useExternalPagination: true,
            exporterCsvFilename: 'file.csv',
            exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
            exporterExcelFilename: 'file.xlsx',
            exporterExcelSheetName: 'Sheet1',
            enableFiltering: false,
            useExternalFiltering: false,
            columnDefs: columnDef,
            rowHeight: 32,
            i18n: 'vi',
            showGridFooter: false,
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                $scope.gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                    $scope.filters.pageNumber = newPage;
                    $scope.filters.pageSize = pageSize;
                    $scope.GetData();
                });
            },
            exporterMenuCsv: true,
            enableGridMenu: true,
        }

        $scope.ShowImage = function (src) {
            return src
        }
        $scope.Edit = function (value) {
            $scope.OpenWindow()
            $scope.isAdd = false;
            $scope.languageChoose = -1;
            $scope.ResetEditor();
            $("#loader").css("display", "block")
            $http({
                method: "POST",
                url: urlDetail,
                params: {
                    id: value
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                $scope.hotel = response.data
                var idLanguageChoose = $scope.language[0].LanguageId
                $timeout(function () {
                    $scope.SetAdditionInfoMultiLanguage(idLanguageChoose);
                }, 1000);
                $scope.languageChoose = idLanguageChoose
                }, function error(respone) {
                    $("#loader").css("display", "none")
            })
        }
        $scope.ShowAdd = function () {
            $scope.isAdd = true
            $("#kUI_window").css("display", "block")
            $scope.hotel = {
                Logo: '/FileDefault/image/img-default.gif',
                Name: '',
                Code: '',
                Email: '',
                Address: '',
                Phone: '',
                Hotline: '',
                Fax: '',
                Website: '',
                Facebook: '',
                Skyper: '',
                Google: '',
                Youtobe: '',
                HighLight: false,
                hotelLanguages: []
            }
            if ($scope.language.length > 0)
                $scope.languageChoose = $scope.language[0].LanguageId
            $scope.ResetEditor();
            $scope.language.forEach(x => {
                $scope.hotel.hotelLanguages.push({
                    LanguageId: x.LanguageId,
                    Terms: '',
                    InforAccount: '',
                    Note: ''
                })
            });
            $scope.OpenWindow()
        }
        $scope.Post = function () {
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
                url: urlPost,
                data: $scope.hotel
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

        $scope.OpenWindow = function () {
            $("#kUI_window").data("kendoWindow").maximize().open();
        }
        $scope.CloseWindow = function () {
            $("#kUI_window").data("kendoWindow").close();
            $scope.RefreshValidate()
        }


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
                url: urlGet,
                method: "GET",
                params: $scope.filters
            }).then(function success(response) {
                $("#loader").css("display", "none")
                var data = response.data;
                $scope.gridOptions.data = data.hotels;
                $scope.gridOptions.totalItems = data.totalRecord
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
            }, function error(response) {

            });
        }
        $scope.GetData()
        $scope.GetLanguage()
    }]);