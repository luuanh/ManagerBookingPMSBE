var urlgetSituation = "/Statistic/getSituation";
var urlgetUse = "/Statistic/getUse";
var urlgetRegistration = "/Statistic/getRegistration";
var urlgetRegistrationSituation = "/Statistic/getRegistrationSituation";
var urlgetAllRegistration = "/Statistic/getAllRegistration";

app.controller('controller', ['$scope', '$http',
    function ($scope, $http) {
        $scope.paramSituation = {
            type: 0,
            month: 1,
            year: 2018
        };
        $scope.paramUse = {
            type: 0,
            month: 1,
            year: 2018
        };
        $scope.paramRegistration = {
            type: 0,
            month: 1,
            year: 2018
        };
        $scope.paramRegistrationSituation = {
            month: 1,
            year: 2018
        };
        $scope.paramAllRegistration = {
            month: 1,
            year: 2018
        };

        var myChartSituation = null;
        var myChartUse = null;
        var myChartRegistration = null;
        var myChartRegistrationSituation = null;
        var myChartAllRegistration = null;
        var configSituation = {
            type: 'pie',
            data: {
                datasets: [{
                    data: [],
                    backgroundColor: [
                        '#36A2EB',
                        '#4BC0C0',
                        '#FFCD56',
                        '#FF6384'
                    ],
                    label: 'Tình hình khách sạn'
                }],
                labels: [
                    'Dùng thử',
                    'Sử dụng thật',
                    'Yêu cầu gia hạn',
                    'Đã khóa'
                ]
            },
            options: {
                responsive: true
            }
        };
        var configUse = {
            type: 'pie',
            data: {
                datasets: [{
                    data: [],
                    backgroundColor: [
                        '#36A2EB',
                        '#FFCD56',
                        '#4BC0C0'
                    ],
                    label: 'Khách hàng đăng ký sử dụng phần mềm'
                }],
                labels: [
                    'PMS',
                    'BookingEngine',
                    'PMS + BookingEngine'
                ]
            },
            options: {
                responsive: true
            }
        };
        var configRegistration = {
            type: 'pie',
            data: {
                datasets: [{
                    data: [],
                    backgroundColor: [
                        '#36A2EB',
                        '#FFCD56',
                        '#4BC0C0'
                    ],
                    label: 'Khách hàng đăng ký sử dụng phần mềm'
                }],
                labels: [
                    'PMS',
                    'BookingEngine',
                    'PMS + BookingEngine'
                ]
            },
            options: {
                responsive: true
            }
        };
        var configRegistrationSituation = {
            type: 'line',
            data: {
                labels: [],
                datasets: [{
                    label: 'PMS',
                    borderColor: '#36A2EB',
                    backgroundColor: '#36A2EB',
                    data: [],
                    fill: false
                }, {
                    label: 'BookingEngine',
                    borderColor: '#FFCD56',
                    backgroundColor: '#FFCD56',
                    data: [],
                    fill: false
                }, {
                    label: 'PMS + BookingEngine',
                    borderColor: '#4BC0C0',
                    backgroundColor: '#4BC0C0',
                    data: [],
                    fill: false
                }]
            },
            options: {
                responsive: true,
                tooltips: {
                    mode: 'index',
                    intersect: false
                },
                hover: {
                    mode: 'index',
                    intersect: false
                }
            }
        };
        var configAllRegistration = {
            type: 'line',
            data: {
                labels: [],
                datasets: [{
                    label: 'PMS, BookingEngine, PMS + BookingEngine',
                    borderColor: '#36A2EB',
                    backgroundColor: '#36A2EB',
                    data: [],
                    fill: false
                }]
            },
            options: {
                responsive: true,
                tooltips: {
                    mode: 'index',
                    intersect: false
                },
                hover: {
                    mode: 'index',
                    intersect: false
                }
            }
        };
        var ctxSituation = document.getElementById('chartSituation').getContext('2d');
        var ctxUse = document.getElementById('chartUse').getContext('2d');
        var ctxRegistration = document.getElementById('chartRegistration').getContext('2d');
        var ctxRegistrationSituation = document.getElementById('chartRegistrationSituation').getContext('2d');
        var ctxAllRegistration = document.getElementById('chartAllRegistration').getContext('2d');
        $scope.Init = function () {
            var datenow = new Date();
            var month = datenow.getMonth() + 1;
            var year = datenow.getFullYear();
            $scope.paramSituation.month = month;
            $scope.paramSituation.year = year;

            $scope.paramUse.month = month;
            $scope.paramUse.year = year;

            $scope.paramRegistration.month = month;
            $scope.paramRegistration.year = year;

            $scope.paramRegistrationSituation.month = month;
            $scope.paramRegistrationSituation.year = year;

            $scope.paramAllRegistration.month = month;
            $scope.paramAllRegistration.year = year;

            $scope.getSituation();
            $scope.getUse();
            $scope.getRegistration();
            $scope.getRegistrationSituation();
            $scope.getAllRegistration();
        };
        $scope.getSituation = function () {
            $http({
                url: urlgetSituation,
                method: 'GET',
                params: $scope.paramSituation
            }).then(function success(respone) {
                configSituation.data.datasets[0].data = respone.data;
                if (myChartSituation != null) {
                    myChartSituation.destroy();
                }
                myChartSituation = new Chart(ctxSituation, configSituation);
            }, function error(respone) {

            });
        };
        $scope.getUse = function () {
            $http({
                url: urlgetUse,
                method: 'GET',
                params: $scope.paramUse
            }).then(function success(respone) {
                configUse.data.datasets[0].data = respone.data;
                if (myChartUse != null) {
                    myChartUse.destroy();
                }
                myChartUse = new Chart(ctxUse, configUse);
            }, function error(respone) {

            });
        };
        $scope.getRegistration = function () {
            $http({
                url: urlgetRegistration,
                method: 'GET',
                params: $scope.paramRegistration
            }).then(function success(respone) {
                configRegistration.data.datasets[0].data = respone.data;
                if (myChartRegistration != null) {
                    myChartRegistration.destroy();
                }
                myChartRegistration = new Chart(ctxRegistration, configRegistration);
            }, function error(respone) {

            });
        };
        $scope.getRegistrationSituation = function () {
            $http({
                url: urlgetRegistrationSituation,
                method: 'GET',
                params: $scope.paramRegistrationSituation
            }).then(function success(respone) {
                var data = respone.data;
                configRegistrationSituation.data.labels = data.lables;
                configRegistrationSituation.data.datasets[0].data = data.pms;
                configRegistrationSituation.data.datasets[1].data = data.be;
                configRegistrationSituation.data.datasets[2].data = data.pmsbe;
                if (myChartRegistrationSituation != null) {
                    myChartRegistrationSituation.destroy();
                }
                myChartRegistrationSituation = new Chart(ctxRegistrationSituation, configRegistrationSituation);
            }, function error(respone) {

            });
        };
        $scope.getAllRegistration = function () {
            $http({
                url: urlgetAllRegistration,
                method: 'GET',
                params: $scope.paramAllRegistration
            }).then(function success(respone) {
                var data = respone.data;
                configAllRegistration.data.labels = data.lables;
                configAllRegistration.data.datasets[0].data = data.data;
                if (myChartAllRegistration != null) {
                    myChartAllRegistration.destroy();
                }
                myChartAllRegistration = new Chart(ctxAllRegistration, configAllRegistration);
            }, function error(respone) {

            });
        };
        $scope.Init();
    }]);