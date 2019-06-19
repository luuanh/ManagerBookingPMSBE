var urlgetStatisticRevenue = "/Admin/Statistic/GetStatisticRevenue"

app.controller('controller', ['$scope', '$http', 'validate', 'notify', 'helper',
    function ($scope, $http, validate, notify, helper) {
        $scope.paramSituation = {
            month: 1,
            year: 2018
        };
        $scope.paramSituation2 = {
            month: 1,
            year: 2018
        };
        var myChartSituation = null;
        var myChartSituationAll = null;
        var myChartSituation2 = null;
        var myChartSituation2All = null;
        var configSituation = {
            type: 'line',
            data: {
                labels: [],
                datasets: [{
                    label: 'Booking',
                    borderColor: '#36A2EB',
                    backgroundColor: '#36A2EB',
                    data: [],
                    fill: false
                }, {
                    label: 'Dịch vụ',
                    borderColor: '#FFCD56',
                    backgroundColor: '#FFCD56',
                    data: [],
                    fill: false
                }, {
                    label: 'Giường phụ',
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
        var configSituationAll = {
            type: 'line',
            data: {
                labels: [],
                datasets: [{
                    label: 'Tổng',
                    borderColor: '#ff0000',
                    backgroundColor: '#ff0000',
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
        var configSituation2 = {
            type: 'line',
            data: {
                labels: [],
                datasets: [{
                    label: 'Booking',
                    borderColor: '#36A2EB',
                    backgroundColor: '#36A2EB',
                    data: [],
                    fill: false
                }, {
                    label: 'Dịch vụ',
                    borderColor: '#FFCD56',
                    backgroundColor: '#FFCD56',
                    data: [],
                    fill: false
                }, {
                    label: 'Giường phụ',
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
        var configSituation2All = {
            type: 'line',
            data: {
                labels: [],
                datasets: [{
                    label: 'Tổng',
                    borderColor: '#ff0000',
                    backgroundColor: '#ff0000',
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
        var ctxSituation = document.getElementById('chartStatisticRevenue').getContext('2d');
        var ctxSituationAll = document.getElementById('chartStatisticRevenueAll').getContext('2d');
        var ctxSituation2 = document.getElementById('chartStatisticRevenue2').getContext('2d');
        var ctxSituation2All = document.getElementById('chartStatisticRevenue2All').getContext('2d');
        $scope.getStatisticRevenue = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlgetStatisticRevenue,
                method: 'GET',
                params: $scope.paramSituation
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                var data = respone.data;
                configSituation.data.labels = data.lables;
                configSituationAll.data.labels = data.lables;
                configSituation.data.datasets[0].data = data.booking;
                configSituation.data.datasets[1].data = data.service;
                configSituation.data.datasets[2].data = data.extrabed;
                configSituationAll.data.datasets[0].data = data.all;
                if (myChartSituation != null) {
                    myChartSituation.destroy();
                }
                if (myChartSituationAll != null) {
                    myChartSituationAll.destroy();
                }
                myChartSituation = new Chart(ctxSituation, configSituation);
                myChartSituationAll = new Chart(ctxSituationAll, configSituationAll);
            }, function error(respone) {
                $("#loader").css("display", "none")

            });
        };
        $scope.getStatisticRevenue2 = function () {
            $("#loader").css("display", "block")
            $http({
                url: urlgetStatisticRevenue,
                method: 'GET',
                params: $scope.paramSituation2
            }).then(function success(respone) {
                $("#loader").css("display", "none")
                var data = respone.data;
                configSituation2.data.labels = data.lables;
                configSituation2All.data.labels = data.lables;
                configSituation2.data.datasets[0].data = data.booking;
                configSituation2.data.datasets[1].data = data.service;
                configSituation2.data.datasets[2].data = data.extrabed;
                configSituation2All.data.datasets[0].data = data.all;
                if (myChartSituation2 != null) {
                    myChartSituation2.destroy();
                }
                if (myChartSituation2All != null) {
                    myChartSituation2All.destroy();
                }
                myChartSituation2 = new Chart(ctxSituation2, configSituation2);
                myChartSituation2All = new Chart(ctxSituation2All, configSituation2All);
            }, function error(respone) {
                $("#loader").css("display", "none")

            });
        };
        $scope.Init = function () {
            var datenow = new Date();
            var month = datenow.getMonth() + 1;
            var year = datenow.getFullYear();
            $scope.paramSituation.month = month > 1 ? month - 1 : month;
            $scope.paramSituation.year = year;
            $scope.paramSituation2.month = month == 1 ? month + 1 : month;
            $scope.paramSituation2.year = year;
            $scope.getStatisticRevenue();
            $scope.getStatisticRevenue2();
        };
        $scope.Init();
    }])