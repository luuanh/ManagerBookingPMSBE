var urlGet = "/Admin/Policy/Get"
var urlPost = "/Admin/Policy/Post"
var urlPut = "/Admin/Policy/Put"
var urlDetail = "/Admin/Policy/Detail"
var urlDelete = "/Admin/Policy/Delete"
var urlGetLanguage = "/Admin/Language/GetActive"

app.controller('controller', ['$scope', '$http', '$log', 'uiGridConstants', 'validate', 'notify',
    '$timeout',
    function ($scope, $http, $log, uiGridConstants, validate, notify, $timeout) {
        $scope.filters = {
            pageNumber: 1,
            pageSize: 100,
            keySearch: ''
        }
        $scope.policy = {
            Index: 1,
            RequirePrice: true,
            PolicyLanguages: []
        }
        $scope.language = []
        $scope.languageChoose = 0;
        $scope.isAdd = true
        var columnDef = [
            { displayName: "Tên", name: 'PolicyName', width: '*', minWidth: 150 },
            { displayName: "Thứ tự", name: 'Index', width: 100 },
            {
                displayName: "Thao tác", name: "#", width: 150, field: "#",
                cellTemplate: '<div class="ui-grid-cell-contents box-control" style="text-align:center"><a href="javascript:void(0);" class="link-control" title="Sửa" ng-click="grid.appScope.Edit(row.entity.PolicyId)">Sửa</a><a href="javascript:void(0);" class="link-control" title="Xóa" ng-click="grid.appScope.ConfirmDelete(row.entity.PolicyId)">Xóa</a></div>',
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
        $scope.ConfirmDelete = function (value) {
            $scope.itemChoose = value;
            UIkit.modal("#ModalConfirm").show()
        };
        $scope.Edit = function (value) {
            $scope.OpenWindow()
            $scope.isAdd = false;
            $scope.languageChoose = -1;
            $("#loader").css("display", "block")
            $http({
                method: "POST",
                url: urlDetail,
                params: {
                    id: value
                }
            }).then(function success(response) {
                $("#loader").css("display", "none")
                $scope.policy = response.data
                if ($scope.language.length > 0) {
                    $scope.languageChoose = $scope.language[0].LanguageId
                    $timeout(function () {
                        $scope.SetAdditionInfoMultiLanguage($scope.languageChoose);
                    }, 500);
                }
            }, function error(respone) {
                $("#loader").css("display", "none")
            })
        }
        $scope.ShowAdd = function () {
            $scope.isAdd = true
            $scope.policy = {
                Index: 1,
                RequirePrice: true,
                PolicyLanguages: []
            }
            $scope.language.forEach(x => {
                $scope.policy.PolicyLanguages.push({
                    LanguageId: x.LanguageId,
                    PolicyName: '',
                    Content: ''
                })
            });
            if ($scope.language.length > 0) {
                $scope.languageChoose = $scope.language[0].LanguageId
            }
            $scope.ResetEditor();
            $scope.OpenWindow()
        }
        $scope.Post = function () {
            var v1 = false;
            $scope.policy.PolicyLanguages.forEach(x => {
                if (x.PolicyName != null && x.PolicyName != "") {
                    v1 = true;
                }
            })
            if (!v1) {
                $("#err_PolicyName").css("display", "block")
            }
            else
                $("#err_PolicyName").css("display", "none")
            var v2 = validate.isNotNumberSingleShowError({ value: $scope.policy.Index, key: 'Index' })
            if (v2 || !v1) {
                notify.error('Giá trị không hợp lệ!')
                return
            }

            $("#loader").css("display", "block")
            $scope.GetAdditionInfoMultiLanguage()
            $http({
                method: "POST",
                url: urlPost,
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
        $scope.Put = function () {
            var v1 = false;
            $scope.policy.PolicyLanguages.forEach(x => {
                if (x.PolicyName != null && x.PolicyName != "") {
                    v1 = true;
                }
            })
            if (!v1) {
                $("#err_PolicyName").css("display", "block")
            }
            else
                $("#err_PolicyName").css("display", "none")
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

        $scope.OpenWindow = function () {
            $("#kUI_window").data("kendoWindow").maximize().open();
        }
        $scope.CloseWindow = function () {
            $("#kUI_window").data("kendoWindow").close();
            $scope.RefreshValidate()
        }
        $scope.RefreshValidate = function () {
            $(".error").css("display", "none")
        }
        $scope.GetAdditionInfoMultiLanguage = function (index = -1) {
            if (index < 0)
                index = $scope.languageChoose
            var indexOld = $scope.policy.PolicyLanguages.findIndex(x => x.LanguageId == index);
            var objOld = $scope.policy.PolicyLanguages[indexOld]
            if (objOld != null) {
                objOld.PolicyName = $("#PolicyName").val()
                objOld.Content = CKEDITOR.instances.contentPolicy.getData()
            }
        }
        $scope.SetAdditionInfoMultiLanguage = function (index) {
            var indexNew = $scope.policy.PolicyLanguages.findIndex(x => x.LanguageId == index);
            var objNew = $scope.policy.PolicyLanguages[indexNew]
            if (objNew != null) {
                $("#PolicyName").val(objNew.PolicyName)
                CKEDITOR.instances.contentPolicy.setData(objNew.Content)
            }
        }
        $scope.ResetEditor = function () {
            CKEDITOR.instances.contentPolicy.setData('')
            $("#PolicyName").val('')
        }
        $scope.$watch('languageChoose', function (newValue, oldValue) {
            if (newValue < 0)
                return
            if (oldValue > 0)
                $scope.GetAdditionInfoMultiLanguage(oldValue)
            $scope.SetAdditionInfoMultiLanguage(newValue)
        });
        $scope.Delete = function () {
            $http({
                method: "GET",
                url: urlDelete,
                params: {
                    id: $scope.itemChoose
                }
            }).then(function success(respone) {
                $scope.GetData()
                UIkit.modal("#ModalConfirm").tryhide()
                notify.success("Thành công")
            }, function error(respone) {
                UIkit.modal("#ModalConfirm").tryhide()
                notify.error("Có lỗi sảy ra!")
            })
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
                $scope.gridOptions.data = data.policies;
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
    }]);