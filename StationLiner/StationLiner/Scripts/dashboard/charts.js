$(document).ready(function() {
    loadSalesSummary("all","month-to-date");
//    loadTanks();
    loadBestPerfomingStations(3,"month-to-date");
    loadLeastPerfomingStations(3, "month-to-date");
    loadSalesByPaymentModes("all", "month-to-date");
});




function loadSalesSummary(station,period) {
    $(function () {
        
        var data = {
            "Station": station,
            "period": period
        }
//        console.log(data);
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
                        csv:csv
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
        //        console.log(labels);
        
        
    });
    
}

function loadTanks(station) {
    //here make an ajax request to get the total number of tanks in a certain station and their levels

    $(function () {
        if (station != "all") {
            var data = {
                "Station": station
            }
            $("#tank-levels-div").html('');
            var dataq = $.ajax({
                url: "/Dashboard/FuelLevel",
                type: "POST",
                dataType: "json",
                data: data,
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
                                    size: '100%',
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
                                    name: 'Volume',
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
                            $("#fuel-levels-row").slideDown('slow');
                        }
                    }
                   
                }
            });
        } else {
            $("#fuel-levels-row").slideUp('slow');
        }
    });
        
};

function loadBestPerfomingStations(show, range) {
    var data = {
        "filterBy": show,
        "range": range
    }
    $.ajax({
        url: "/Dashboard/BestPerfomingStation",
        type: "POST",
        dataType: "json",
        data:data,
        success: function(data) {
//            console.log(data);
//            data
            if (data) {
                $("#after-best-perfoming").html("");
                for (var i = 0; i < data.length; i++) {
//                    console.log(data);
//                    console.log(data[i].stationId);
                    var html = '<div class="col-sm-3 col-xs-6 col-md-6">' +
                        '<div class="description-block border-right">' +
                        '<span class="description-percentage text-green"><h3>' + data[i].StationName + '</h3></span>' +
                        '<h5 class="description-text">Total Revenue ' + data[i].TotalForSingleStationFormated + '</h5>' +
                        '<span class="description-percentage text-yellow">Contribution: ' + data[i].Contribution + '%</span>' +
                        '</div>' +
                        '</div>';
                    $("#after-best-perfoming").append(html);
                }
            }
            
           
            
        }
    });
};


function loadLeastPerfomingStations(show,range) {
    var data = {
        "filterBy": show,
        "range": range
    }
    $.ajax({
        url: "/Dashboard/LeastPerforming",
        type: "POST",
        dataType: "json",
        data:data,
        success: function (data) {
//            console.log(data);
            //            data
            if (data) {
                $("#after-worst-perfoming").html("");
                for (var i = 0; i < data.length; i++) {
                    //                    console.log(data);
                    //                    console.log(data[i].stationId);
                    var html = '<div class="col-sm-3 col-xs-6 col-md-6">' +
                        '<div class="description-block border-right">' +
                        '<span class="description-percentage text-green"><h3>' + data[i].StationName + '</h3></span>' +
                        '<h5 class="description-text">Total Revenue ' + data[i].TotalForSingleStationFormated + '</h5>' +
                        '<span class="description-percentage text-yellow">Contribution: ' + data[i].Contribution + '%</span>' +
                        '</div>' +
                        '</div>';
                    $("#after-worst-perfoming").append(html);
                }
            }



        }
    });
};


$("#station-btn,#period-range").on("change",
    function() {
        var station = $("#station-btn").val();
        var period = $("#period-range").val();

        loadSalesSummary(station, period);
        loadSalesByPaymentModes(station, period);
        loadTanks(station);
    });

$("#best-performing-range,#best-number").on("change", function () {
    var show = $("#best-number").val();
    var range = $("#best-performing-range").val();
//    console.log(show);
    loadBestPerfomingStations(show, range);
});

$("#least-performing-range,#least-number").on("change", function () {
    var show = $("#least-number").val();
    var range = $("#least-performing-range").val();
    //    console.log(show);
    loadLeastPerfomingStations(show, range);
});


function loadSalesByPaymentModes(station,period) {
    $(function () {
        var data = {
            "Station": station,
            "period": period
        }
        var dataobj = $.ajax({
            url: "/Dashboard/AdminStationPaymentModes",
            type: "POST",
            dataType: "json",
            data:data,
            success: function (data) {
                if (data) {
                    $("#below-payment-modes").html("");
                    for (var i = 0; i < data.modes.length; i++) {
                        var percent = (parseInt(data.modes[i]["Amount"]) / parseInt(data.total)) * 100;
//                        console.log(parseInt(data.total));
                        var html = '<div class="progress-group">' +
                            '<span class="progress-text">' + data.modes[i]["Mode"] + '</span>' +
                            '<span class="progress-number"><b>' + data.modes[i]["Amount"] + '/' + data.total + '</b></span>' +
                            '<div class="progress sm active">' +
                            '<div class="progress-bar progress-bar-aqua progress-bar-striped" style="width: ' + percent + '%"></div>' +
                            '</div>' +
                            '</div>';
                        $("#below-payment-modes").append(html);
                    }
                }
            }
        });
    });
}