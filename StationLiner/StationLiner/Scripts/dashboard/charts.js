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
            data:data,
            success: function (data) {
//                console.log(data.datasets);
                var ctx = document.getElementById('myChart').getContext('2d');
                var myChart = new Chart(ctx, {
                    type: 'line',
                    data: {
                        labels: data.labels,
                        datasets: data.datasets
                    },
                   /* options: {
                        scales: {
                            yAxes: [{
                                ticks: {
                                    beginAtZero: true
                                }
                            }]
                        }
                    }*/
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

            var dataq = $.ajax({
                url: "/Dashboard/StationFuelLevels",
                type: "POST",
                dataType: "json",
                data: data,
                success: function(data) {
                    if (data) {
                        $("#fuel-levels-row").show('slow');
                        var chart = new Chart(document.getElementById("tank-levels"),
                            {
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