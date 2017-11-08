$(document).ready(function () {
    var stationId = $("#StationChannelId").val();
    loadStationSalesSummary(stationId,"month-to-date");
    loadSalesByPaymentModes(stationId,"month-to-date");
    loadAttendantsCashDrops("month-to-date");
    attendantPerformance();
    loadPumpSummary();
    loadFuelStationLevel();
});



function loadStationSalesSummary(station,period) {
    $(function () {
        var data= {
            "Station": station,
            "period":period
        }
        var jsonData = $.ajax({
            url: "/Dashboard/LoadSalesData",
            type: "POST",
            dataType: "json",
            data: data,
            success: function (csv) {
                //                console.log(csv);
                //                var chartD = csv.chartData;
                var chart = Highcharts.chart('containerone', {
                    data: {
                        csv: csv
                    },
                    chart: {
                        type: "area"
                    },
                    title: {
                        text: csv.chartTitle
                    },

                    // subtitle: {
                    //     text: 'Source: thesolarfoundation.com'
                    // },

                    yAxis: {
                        title: {
                            text: 'Amount Sold'
                        }
                    },
                    xAxis: {
                        categories: csv.labels,
                        allowDecimals: false,
                        title: {
                            text: "Number of days"
                        },
                        labels: {
                            formatter: function () {
                                return this.value; // clean, unformatted number for year
                            }
                        }

                    },
                    legend: {
                        layout: 'vertical',
                        align: 'right',
                        verticalAlign: 'middle'
                    },

                    plotOptions: {
                        series: {
                            label: {
                                connectorAllowed: false
                            },
                            pointStart: 0
                        }
                    },

                    series: csv.datasets,

                    responsive: {
                        rules: [{
                            condition: {
                                maxWidth: 100
                            },
                            chartOptions: {
                                legend: {
                                    layout: 'horizontal',
                                    align: 'center',
                                    verticalAlign: 'bottom'
                                }
                            }
                        }]
                    }

                });
            }
           
        });
    });

}

function loadSalesByPaymentModes(station,period) {
    $(function () {
        var data = {
            "Station":station,
            "period":period
        }
        var datal = $.ajax({
            url: "/Dashboard/AdminStationPaymentModes",
            type: "POST",
            dataType: "json",
            data:data,
            success: function(data) {
                if (data) {
                    $("#below-payment-modes").html("");
                    for (var i = 0; i < data.modes.length; i++) {
                        var  percent = (parseInt(data.modes[i]["Amount"]) / parseInt(data.total)) * 100;
                       var html = '<div class="progress-group">' +
                           '<span class="progress-text">' + data.modes[i]["Mode"] + '</span>' +
                           '<span class="progress-number"><b>' + data.modes[i]["Amount"] + '/' + data.total + '</b></span>' +
                           '<div class="progress sm active">' +
                           '<div class="progress-bar progress-bar-aqua progress-bar-striped" style="width: '+percent+'%"></div>' +
                           '</div>' +
                           '</div>';
                        $("#below-payment-modes").append(html);
                    }
               }
            }
        });
    });
}

function loadAttendantsCashDrops(period) {
    $(function () {
        var data = {
           "period": period
        }
        var color = Chart.helpers.color;
        var data2 = $.ajax({
            url: "/Dashboard/LoadAttendantCashDrop",
            type: "POST",
            dataType: "json",
            data: data,
            success: function (data) {
                
                Highcharts.chart('attendant-performance-chart', {
                    chart: {
                        type: 'column'
                    },
                    title: {
                        text: 'Attendant Cash Drops Report'
                    },
                    subtitle: {
//                        text: 'Source: WorldClimate.com'
                    },
                    xAxis: {
                        categories: data.labels,
                        crosshair: true
                    },
                    yAxis: {
                        min: 0,
                        title: {
                            text: 'Amount'
                        }
                    },
                    tooltip: {
                        headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                        pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                            '<td style="padding:0"><b>{point.y:.1f} /=</b></td></tr>',
                        footerFormat: '</table>',
                        shared: true,
                        useHTML: true
                    },
                    plotOptions: {
                        column: {
                            pointPadding: 0.2,
                            borderWidth: 0
                        }
                    },
                    series: [{
                        name: (data.datasets.length >0)?  data.datasets[0].label: [],
                        data: (data.datasets.length >0)?  data.datasets[0].data: []

                    }
                    /*, {
                        name: 'New York',
                        data: [83.6, 78.8, 98.5, 93.4, 106.0, 84.5, 105.0, 104.3, 91.2, 83.5, 106.6, 92.3]

                    }, {
                        name: 'London',
                        data: [48.9, 38.8, 39.3, 41.4, 47.0, 48.3, 59.0, 59.6, 52.4, 65.2, 59.3, 51.2]

                    }, {
                        name: 'Berlin',
                        data: [42.4, 33.2, 34.5, 39.7, 52.6, 75.5, 57.4, 60.4, 47.6, 39.1, 46.8, 51.1]

                    }*/
                    ]
                });
                /*var ctx = document.getElementById('attendant-performance-chart').getContext('2d');
                var myChart = new Chart(ctx, {
                    type: 'bar',
                    data: {
                        labels: data.labels,
                        datasets: data.datasets,
//                        backgroundColor: color(window.chartColors.red).alpha(0.5).rgbString(),
//                        borderColor: window.chartColors.red,
                        borderWidth: 1,
                    }
                });*/
            }
        });
    });
}

function attendantPerformance() {
    $(function() {
        var data = $.ajax({
            url: "/Dashboard/LoadAttendantPerformance",
            type: "POST",
            dataType: "json",
            success: function (data) {
//                var ctx = document.getElementById('attendant-performance-chart').getContext('2d');
//                var myChart = new Chart(ctx, {
//                    type: 'bar',
//                    data: {
//                        labels: data.labels,
//                        datasets: data.datasets,
//                        //                        backgroundColor: color(window.chartColors.red).alpha(0.5).rgbString(),
//                        //                        borderColor: window.chartColors.red,
//                        borderWidth: 1,
//                    }
//                });
            }
        });
    });
}

function loadPumpSummary() {
    $(function () {

        var jsonData = $.ajax({
            url: "/Dashboard/PumpSummary",
            type: "POST",
            dataType: "json",
            success: function (data) {
                if (data) {
                    for (var i = 0; i < data.length; i++) {
                        var html = '<div class="info-box bg-aqua col-md-12"style="box-shadow: 0 0px 0px rgba(0,0,0,0.1);">' +
//                            '<span class="info-box-icon"><i class="ion-ios-chatbubble-outline"></i></span>' +
                            '<div class="info-box-content">' +
                            '<span class="info-box-text col-md-2">'+data[i].PumpName +'</span>' +
                            '<div class="col-md-10">';
                        for (var a = 0; a < data[i].nozzles.length; a++) {
                            var nozzleData = data[i].nozzles;
//                            console.log(nozzleData);
                            html += '<div class="row"><span class="info-box-text col-md-2">' + nozzleData[a].ProductName + ': <strong>' + nozzleData[a].LitersSold + 'l</strong></span>' +
                                '<span class="col-md-3"> Electronic Reading:  \t <strong>' + nozzleData[a].EndElectronicReading + '</strong></span> ' +
                                '<span class="col-md-3"> Manual Reading: \t <strong>' + nozzleData[a].EndManualReading + '</strong></span> ' +
                                '<span class="col-md-3"> Difference: \t <strong>' + nozzleData[a].DifferenceR + '</strong></span></div> ';
                        }
                        html += '</div></div></div>';

                        $("#after-summary").append(html);
                    }
                }
                
            }
        });
    });
}

function loadFuelStationLevel() {
    $(function () {
        var data = $.ajax({
            url: "/Dashboard/FuelLevel",
            type: "POST",
            dataType: "json",
            success: function (data) {

                if (data) {
                    for (var i = 0; i < data.length; i++) {
                        var containerId = "container" + i + 3;
                        $("#tank-levels-div").append('<div id="' + containerId + '" style="min-width: 310px; max-width: 400px; height: 300px; margin: 0 auto" class="col-md-4"></div>');


                        var gaugeOptions = {

                            chart: {
                                type: 'solidgauge'
                            },

                            title: data[i]["TankName"],

                            pane: {
                                center: ['50%', '85%'],
                                size: '80%',
                                startAngle: -90,
                                endAngle: 90,
                                background: {
                                    backgroundColor: (Highcharts.theme && Highcharts.theme.background2) || '#EEE',
                                    innerRadius: '60%',
                                    outerRadius: '100%',
                                    shape: 'arc'
                                }
                            },

                            tooltip: {
                                enabled: false
                            },

                            // the value axis
                            yAxis: {
                                stops: [
                                    [0.1, '#DF5353'], // red
                                    [0.5, '#DDDF0D'], // yellow
                                    [0.9, '#55BF3B'] // green
                                ],
                                lineWidth: 0,
                                minorTickInterval: null,
                                tickAmount: 2,
                                title: {
                                    y: -70
                                },
                                labels: {
                                    y: 16
                                }
                            },

                            plotOptions: {
                                solidgauge: {
                                    dataLabels: {
                                        y: 5,
                                        borderWidth: 0,
                                        useHTML: true
                                    }
                                }
                            }
                        };
                        // The speed gauge
                        var chartSpeed = Highcharts.chart(containerId, Highcharts.merge(gaugeOptions, {
                            yAxis: {
                                min: 0,
                                max: data[i]['MaxVolume'],
                                title: {
                                    text: data[i]['TankName']
                                }
                            },

                            credits: {
                                enabled: false
                            },

                            series: [{
                                name: 'Speed',
                                data: [data[i]['CurrentVolume']],
                                dataLabels: {
                                    format: '<div style="text-align:center"><span style="font-size:25px;color:' +
                                        ((Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black') + '">{y}</span><br/>' +
                                        '<span style="font-size:12px;color:silver">Liters</span></div>'
                                },
                                tooltip: {
                                    valueSuffix: 'Liters (v)'
                                }
                            }]

                        }));

                        // The RPM gauge
                        /*var chartRpm = Highcharts.chart(containerId, Highcharts.merge(gaugeOptions, {
                            yAxis: {
                                min: 0,
                                max: 5,
                                title: {
                                    text: 'RPM'
                                }
                            },

                            series: [{
                                name: 'RPM',
                                data: [1],
                                dataLabels: {
                                    format: '<div style="text-align:center"><span style="font-size:25px;color:' +
                                        ((Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black') + '">{y:.1f}</span><br/>' +
                                        '<span style="font-size:12px;color:silver">* 1000 / min</span></div>'
                                },
                                tooltip: {
                                    valueSuffix: ' revolutions/min'
                                }
                            }]

                        }));*/
                    }
                }

            }
        });
    });
}

$("#station-sales-range").on("change",
    function () {
        var stationId = $("#StationChannelId").val();
        var period = $(this).val();
        loadSalesByPaymentModes(stationId, period);
        loadStationSalesSummary(stationId, period);
    });

$("#cashdrops").on("change",
    function() {
        var period = $(this).val();
        loadAttendantsCashDrops(period);
    });

var dynamicColors = function () {
    var r = Math.floor(Math.random() * 255);
    var g = Math.floor(Math.random() * 255);
    var b = Math.floor(Math.random() * 255);
    return "rgb(" + r + "," + g + "," + b + ")";
}