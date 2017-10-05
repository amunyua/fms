$("#tank-id").on("change",
    function() {
        var id = $(this).val();
        if (id != "") {
//            alert(id);
            var data = {
                "id": id
            }
            $.ajax({
                url: "/StationHome/GetProduct",
                type: "POST",
                dataType: "json",
                data: data,
                success:function(data) {
                    if (data != "") {
//                        console.log();
                        $("#prod-id").val(data.Product["ProductName"]);
                    }
                }
            });
        }
    });

$('#supplier-id').on("change",function() {
    var id = $(this).val();
//    alert(id);
    var data= {
        "id":id
    }
    var drivers = "";
    var vehicles = "";
    $("#supplier-drivers").html(drivers);
    $("#supplier-vehicles").html(vehicles);
    if (id != "") {
        $.ajax({
            url: "/StationHome/GetFuelDetails",
            dataType: "json",
            type:"POST",
            data: data,
            success: function(data) {
                if (data) {
                    if (data.drivers.length > 0) {
                        for (i = 0; i < data.drivers.length; i++) {
                            drivers += '<option value="' +
                                data.drivers[i]["Id"] +
                                '">' +
                                data.drivers[i]["DriverName"] +
                                '</option>';
                        }
                        $("#supplier-drivers").html(drivers);
                    } else {
                        $("#supplier-drivers").html('<option value="">No Drivers found</option>');
                    }
                    if (data.vehicles.length > 0) {
                        for (i = 0; i < data.vehicles.length; i++) {
                            vehicles += '<option value="' +
                                data.vehicles[i]["Id"] +
                                '">' +
                                data.vehicles[i]["RegNumber"] +
                                '</option>';
                        }
                        $("#supplier-vehicles").html(vehicles);
                    } else {
                        $("#supplier-vehicles").html('<option value="">No vehicles found</option>');
                    }
                }
            }

        });
    }
})