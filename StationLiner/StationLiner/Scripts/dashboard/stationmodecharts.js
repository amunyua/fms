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
            data:data,
            success: function (data) {
                var ctx = document.getElementById('myChart').getContext('2d');
                var myChart = new Chart(ctx, {
                    type: 'line',
                    data: {
                        labels: data.labels,
                        datasets: data.datasets
                    }
                });

//                myChart.datasets[0].points[1].fillColor = "lightgreen";
//                console.log(myChart);
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
            success: function(data) {
                var ctx = document.getElementById('attendant-performance-chart').getContext('2d');
                var myChart = new Chart(ctx, {
                    type: 'bar',
                    data: {
                        labels: data.labels,
                        datasets: data.datasets,
//                        backgroundColor: color(window.chartColors.red).alpha(0.5).rgbString(),
//                        borderColor: window.chartColors.red,
                        borderWidth: 1,
                    }
                });
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
            url: "/Dashboard/StationFuelLevels",
            type: "POST",
            dataType: "json",
            success: function (data) {
                if (data) {
                    console.log(data.data);
                    var chart = new Chart(document.getElementById("tank-levels"), {
                        type: 'doughnut',
                        data: {
                            labels: data.labels,
                            datasets: [
                                {
                                    label: "Population (millions)",
//                                    backgroundColor: ["#3e95cd", "#8e5ea2", "#3cba9f", "#e8c3b9", "#c45850"],
                                    data: data.data
                                }
                            ]
                        },
                        options: {
                            title: {
                                display: true,
                                text: 'Tank Fuel Levels'
                            }
                        }
                    });

                    chart.update();
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