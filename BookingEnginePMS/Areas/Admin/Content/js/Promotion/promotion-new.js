var urlGetDataCofnig = "/Admin/Promotion/GetDataConfigForScreenEdit"
var urlGetLanguage = "/Admin/Language/GetActive"
var urlPost = "/Admin/Promotion/Post"

app.controller('controller', ['$scope', '$http', '$log', 'uiGridConstants', 'validate', 'notify',
    function ($scope, $http, $log, uiGridConstants, validate, notify) {
        $scope.data = {
            TypePromotion: 1,
            Deposit: 0,
            DayInHouse: 0,
            EarlyDay: 0,
            NightForFreeNight: 2,
            FromDate: '',
            ToDate: '',
            Monday: true,
            Tuesday: true,
            Wednesday: true,
            Thursday: true,
            Friday: true,
            Saturday: true,
            Sunday: true,
            PromotionName: '',
            PolicyId: 0,
            AmountRate: 0,
            PromotionRoomTypes: [],
            PromotionLanguages: []
        };
        $scope.languageChoose = 0;
        $scope.RefreahData = function () {
            $scope.data.Deposit = 0
            $scope.data.DayInHouse = 0
            $scope.data.EarlyDay = 0
            $scope.data.NightForFreeNight = 2
        }
        $scope.dataConfig = {}
        $scope.date = {
            startDate: '',
            endDate: ''
        };
        $scope.FormatDate = function (date) {
            date = date.trim()
            var dt = date.split('/');
            var d = dt[0];
            dt[0] = dt[2];
            dt[2] = d;
            return dt.join('/')
        }
        $scope.Post = function () {
            var v1 = validate.isNullOrEmptySingleShowError({ value: $scope.data.PromotionName, key: 'PromotionName' });
            var v2 = validate.isNotNumberSingleShowError({ value: $scope.data.NightForFreeNight, key: 'NightForFreeNight' })
            var v3 = validate.isNotNumberSingleShowError({ value: $scope.data.EarlyDay, key: 'EarlyDay' })
            var v4 = validate.isNotNumberSingleShowError({ value: $scope.data.Deposit, key: 'Deposit' })
            var v5 = validate.isNotNumberSingleShowError({ value: $scope.data.AmountRate, key: 'AmountRate' })
            if (v1 || v2 || v3 || v4 || v5) {
                notify.error('Giá trị không hợp lệ!')
                return
            }
            var datetime = $("#datetimeRange").val().split('-')
            $scope.data.FromDate = $scope.FormatDate(datetime[0])
            $scope.data.ToDate = $scope.FormatDate(datetime[1])
            $scope.data.isRequireRate = $scope.isRequireRate
            $("#loader").css("display", "block")
            $http({
                url: urlPost,
                method: "POST",
                data: $scope.data
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                notify.success("Thành công")

            }, function error(respone) {
                $("#loader").css("display", "none")
                notify.error("Có lỗi sảy ra!")

            });
        }
        

        $scope.ChooseTypePromotion = function (type) {
            $scope.data.TypePromotion = type
            $(".box-type-promotion").removeClass('box-promotion-choose')
            $(".type" + type).addClass('box-promotion-choose')
            $scope.RefreahData()
        }
        $scope.ChangePlaneRate = function () {
            $scope.data.PromotionRoomTypes = $scope.dataConfig.planeRates.
                find(x => x.PlaneRateId == $scope.data.PlaneRateId).PlaneRateRoomTypes
        }
        $scope.ChangePolicy = function () {
            $scope.isRequireRate = $scope.dataConfig.policies.
                find(x => x.PolicyId == $scope.data.PolicyId).RequirePrice
        }

        $scope.GetDataConfig = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlGetDataCofnig,
                method: "GET"
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                $scope.dataConfig = respone.data
                $scope.data.PromotionRoomTypes = $scope.dataConfig.planeRates[0].PlaneRateRoomTypes
                $scope.data.PlaneRateId = $scope.dataConfig.planeRates[0].PlaneRateId
                var policyFirst = $scope.dataConfig.policies[0]
                $scope.data.PolicyId = policyFirst.PolicyId
                $scope.isRequireRate = policyFirst.RequirePrice
                $scope.GetLanguage()
            }, function error(respone) {
                $("#loader").css("display", "none")
            })
        }
        $scope.GetLanguage = function () {
            $http({
                url: urlGetLanguage,
                method: "GET"
            }).then(function success(response) {
                $scope.language = response.data;
                $scope.languageChoose = $scope.language[0].LanguageId
                $scope.language.forEach(x => {
                    $scope.data.PromotionLanguages.push({
                        LanguageId: x.LanguageId,
                        Description: '',
                        Note: ''
                    })
                });
                $scope.ModelForMultiLang = $scope.data.PromotionLanguages[0]
            }, function error(response) {

            });
        }
        $scope.ChangeLanguage = function () {
            var index = $scope.data.PromotionLanguages.findIndex(x => x.LanguageId == $scope.languageChoose);
            $scope.ModelForMultiLang = $scope.data.PromotionLanguages[index]
        }
        $scope.GetDataConfig();
    }])