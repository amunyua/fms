var stations = {
    resetFields: function (formObj) {
        formObj.find('input[type="text"], input[type="email"], input[type="number"], select, textarea').val('');
    },
    showHide: function (action, target) {
        if (action == "show") {
            $(target).show();
        } else {
            $(target).hide();
        }
    }
}

$('#Sides').on('change',
    function () {
        var value = $(this).val();
        if (value == 2) {
            stations.showHide('show', '#side-b-div');
        } else {
            stations.showHide('hide', '#side-b-div');
        }
    });

$(".fuel-type").on("change",
    function () {
        var fuelType = $(this).val();
        var object = $(this);
        var stationId = $(".StationId").val();
        if (stationId != "") {
            if ($(this).is(":checked")) {
                var data = {
                    "FuelType": fuelType,
                    "Station": stationId
                };
                $.ajax({
                    url: "/Stations/GetTanks",
                    type: "POST",
                    dataType: "json",
                    data: data,
                    success: function (data) {
                        var html = '<option value="">Select tank</option>';
                        if (data.length > 0) {
                            var select = object.closest(".parent-div").find(".fuel-type-select");
                            select.html('');
                            for (var i = 0; i < data.length; i++) {
                                //                                alert(i);
                                html += '<option value="' + data[i]['Id'] + '">' + data[i]['TankName'] + '</option>';
                                //                                alert(data[i]["TankName"]);
                            }
                            //                            console.log(html);
                            select.attr("required", "required").html(html);
                            select.parent(".select-par-div").show();
                        }
                    }
                });
            } else {
                object.closest(".parent-div").find(".fuel-type-select").removeAttr("required").html("").parent(".select-par-div").hide();
            }
        }
    });

$(".edit-pumb-btn").on("click",
    function() {
        var id = $(this).attr("edit-id");
        $("#PumpId").val(id);
//        alert(id);

        //get pump details
        var data = {
            "id":id
        }
        $.ajax({
            url: "/Stations/GetEditPumpDetails",
            type: "POST",
            dataType: "json",
            data: data,
            success: function(data) {
                if (data) {
                    console.log(data);
                    var sides = (data.pump["IsDoubleSided"]) ? 2 : 1;
//                    alert(sides);
//                    $("#edit-Sides").val(sides).change();
                    $("#description-edit").val(data.pump["Description"]);
                    $("#pump-name").val(data.pump["Name"]);
                    $("#edit-model").val(data.pump["ModelId"]).change();
//                    $("#edit-model").val(data["ModelId"]);
                }
            }
        });
    });


//functions for edit

$('#edit-Sides').on('change',
    function () {
//        alert("changed");
        var value = $(this).val();
        if (value == 2) {
            stations.showHide('show', '#edit-side-b-div');
        } else {
            stations.showHide('hide', '#edit-side-b-div');
        }
    });