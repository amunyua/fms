$(function() {
    var calibrationTable = $("#calibration-table").DataTable();
    
    var counter = 2,height= 1;
    $("#tank-cal-id").on("change", function() {
        
        var tankId = $(this).val();
        if (tankId != "") {
            $("#cal-table-overlay").show();
            $("#optimisation-box").fadeOut();
            $("#no-op-save-btn").html("Update");
            $("#no-op-save-btn").show();

            var data = {
                "Id": tankId
            }

            $('#calibration-table').DataTable({
                "destroy": true,
                "serverside": true,
                "paging": false,
                "ordering": false,
//                "order": [[0, "desc"]],
                "ajax": ({
                    url: "/Stations/CalibrationDetails",
                    data: function(d) {
                        d.id = tankId;
                    },
                    dataType: 'json',
                    type: "GET",
//                    success:function() {
//                        $("#cal-table-overlay").fadeOut();
                    //                    }
                    complete: function() {
                        $("#cal-table-overlay").fadeOut();
                    }

                }),
                "columns": [
//                    { "data": "Id" },
//                    { "data": "Height" },
                    {
                        mRender: function (data, type, row) {
//                            console.log(row);
                            return "";  
                        }
                    },
                    {
                        mRender: function(data, type, row) {
                            console.log(type);
                            return '<div class="form-group"><input type="number" class="form-control height" value="' +
                                row.Height +
                                '" data-id="' +
                                row[0] +
                                '"></div>';
                        }
                    },
                    {
                        mRender: function(data, type, row) {
                            return '<div class="form-group"><input type="number" class="form-control volume" value="' +
                                row.Volume +
                                '" data-id="' +
                                row[0] +
                                '"></div>';
                        }
                    },
                    {
                        mRender: function(data, type, row) {
                            return '<div class="btn-group">' +
                                '<a style="border:0px;background:0px;" class="btn add-row"><span data-toggle="tooltip" title="" class="badge bg-green" data-original-title="Add new row">+</span></a>' +
                                '</div>';
                        }
                    }
                ]


            });
        } else {
            $("#optimisation-box").show();
        }
    });

    $(".minimal").on('ifClicked', function() {
        var selected = $(this).val();
        if (selected == "no-optimisation") {
            showHideTriggers('hide',""
                );
        } else if (selected == 'fixed-heights') {
            showHideTriggers('show', "Heights");
        } else if (selected == 'fixed-volumes') {
            showHideTriggers('show',"Volume");
        }
    });
    function showHideTriggers(action,label) {
        if (action == 'show') {
            $("#opt-div").fadeIn();
            $("#dif-label").html(label);
        } else {
            $("#opt-div").fadeOut();
            $("#dif-label").html(label);
        }
    }

    $("#apply-optimization").on("click",
        function () {
            $("#no-op-save-btn").html("Save");
            counter = 2;
            height = $("#heights").val();
            $("#cal-table-overlay").show();
            var opt = $('input[name="optimisation"]:checked').val();
            if (opt == "no-optimisation") {
                var trows = [
                    { "Id": 1, "Height": '', Volume: '' }
                ];
                loadDatatable(trows,"add-row");
            } else if (opt == "fixed-heights") {
                
                var trows = [
                    { "Id": 1, "Height": 1, Volume: '' }
                ];
                loadDatatable(trows,"add-row-height");
            } else if (opt == "fixed-volumes") {

                var trows = [
                    { "Id": 1, "Height": '', Volume: 0 }
                ];
                //                loadDatatable(trows, "add-row-volume");
                volumeOptiomization();
            }
        });
    function loadDatatable(trows,addRowsButton) {
        $("#no-op-save-btn").show();
        //                $('#calibration-table').DataTable().destroy();
        $('#calibration-table').DataTable({
            "destroy": true,
            "serverside": true,
            "paging": false,
            //                    "ordering": false,
            "columns": [
                {
                    mRender: function (data, type, row) {
                        return '<div class="form-group"><label>' + row.Id + '</label></div>';
                    }
                }, {
                    mRender: function (data, type, row) {
                        //                            return '<a class="table-edit" data-id="' + row[0] + '">EDIT</a>';
                        //                            console.log(row.Height);
                        return '<div class="form-group"><input type="number" class="form-control height" value="' + row.Height + '"></div>';
                    }
                },
                {
                    mRender: function (data, type, row) {
                        return '<div class="form-group"><input type="number" class="form-control volume" value="' + row.Volume + '" ></div>';
                    }
                },
                {
                    mRender: function (data, type, row) {
                        return '<div class="btn-group">' +
                            '<a style="border:0px;background:0px;" class="btn '+addRowsButton+'"><span data-toggle="tooltip" title="" class="badge bg-green" data-original-title="Add new row">+</span></a>' +
                            '</div>';
                    }
                }
            ],
            "data": trows


        });
        $("#cal-table-overlay").fadeOut();
    }
    
    $(document).on('click','.add-row',
        function () {
            $("#cal-table-overlay").show();
            var ca = $("#calibration-table").DataTable();
            
            ca.row.add(
                { "Id": counter, "Height": '', Volume: '' }
            ).draw(false);
            counter++;
            $("#cal-table-overlay").fadeOut();
        }); 
    $(document).on('click', '.add-row-height',
        function () {
            $("#cal-table-overlay").show();
            var ca = $("#calibration-table").DataTable();
            
            ca.row.add(
                { "Id": counter, "Height": height, Volume: '' }
            ).draw(false);
            counter++;
           
            height = (parseFloat(height) + parseFloat($("#heights").val()));
            $("#cal-table-overlay").fadeOut();
        });
    $(document).on('click', '.add-row-volume',
        function () {
            $("#cal-table-overlay").show();
            var ca = $("#calibration-table").DataTable();
            
            ca.row.add(
                { "Id": counter, "Height": '', Volume: height }
            ).draw(false);
            counter++;
           
            height = (parseFloat(height) + parseFloat($("#heights").val()));
            $("#cal-table-overlay").fadeOut();
        });

    $(document).on('click',
        "#no-op-save-btn",
        function() {
            if (confirm("Are you sure you want to set new calibration values and discard the existing?")) {
                collectValues();
            }
        });

    function collectValues() {
        var calibrations = [];
        var values = [];
        var tankId = $("#tank-cal-id").val();
//        values.push({ "TankId": tankId });
        $('#calibration-table > tbody > tr').each(function (index, element) {
            var height = $(element).find('.height').val(),
                volume = $(element).find('.volume').val();
            if (height != '' && volume != '') {
                values.push({
                    "Height": height,
                    "Volume": volume
                });
            }
        });
//        calibrations.push({ "TankId": tankId, "Calibration": values });
        //        console.log(calibrations);
        var data = {
            calibration: {
                Calibration: values,
                TankId: tankId
            }
        }
        console.log(data);
        if (values.length > 0) {
            $.ajax({
                url: "/Stations/UpdateCalibtationDetails",
                beforeSend: function() {
                    $("#cal-table-overlay").show();
                },
                data: data,
                dataType: 'json',
                type: "POST",
                success: function(data) {
                    if (data.status == "Success") {
                        toastr.success(data.message, data.status);
                    } else {
                        toastr.error(data.message, data.status);
                    }
                },
                complete:function() {
                    $("#cal-table-overlay").fadeOut();
                }
        });
        }
       
    }

    function volumeOptiomization() {
        $("#no-op-save-btn").show();
        var stationId = $("#tank-cal-id").val();
        var data = {
            Id:stationId
        }
        var volume = $.ajax({
            url: "/Stations/getTankVolume",
            dataType: 'json',
            type: "POST",
            data: {id:data},
            success: function (data) {
                
                var diference = parseFloat($("#heights").val());
                var newDif = diference;
                var tableRows = [];
                if (data != "") {
                    var maxVolume = parseFloat(data.Volume);
                    var number = maxVolume / diference;
                    for (var i = 1; i <= number; i++) {
                        tableRows.push({ "Id": i, "Height": '', "Volume": newDif });
                        newDif = newDif + diference;
                    }
                    $('#calibration-table').DataTable({
                        "destroy": true,
                        "serverside": true,
                        "paging": true
                        ,
                        //                    "ordering": false,
                        "columns": [
                            {
                                mRender: function (data, type, row) {
                                    return '<div class="form-group"><label>' + row.Id + '</label></div>';
                                }
                            }, {
                                mRender: function (data, type, row) {
                                    //                            return '<a class="table-edit" data-id="' + row[0] + '">EDIT</a>';
                                    //                            console.log(row.Height);
                                    return '<div class="form-group"><input type="number" class="form-control height" value="' + row.Height + '"></div>';
                                }
                            },
                            {
                                mRender: function (data, type, row) {
                                    return '<div class="form-group"><input type="number" class="form-control volume" value="' + row.Volume + '" ></div>';
                                }
                            },
                            {
                                mRender: function (data, type, row) {
                                   /* return '<div class="btn-group">' +
                                        '<a style="border:0px;background:0px;" class="btn ' + addRowsButton + '"><span data-toggle="tooltip" title="" class="badge bg-green" data-original-title="Add new row">+</span></a>' +
                                        '</div>';*/
                                    return "";
                                }
                            }
                        ],
                        "data": tableRows


                    });

                    $("#cal-table-overlay").fadeOut();
                } else {
                    toastr.error("Failed to apply optimization, contact admin", "Failed");
                }
            }
        });
    }
});