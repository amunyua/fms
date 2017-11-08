$(".edit-tank-btn").on("click",
    function () {
        var id = $(this).attr('edit-id');
        //        alert(id);
        $("#tank-id").val(id);
        var data = {
            "id": id
        }
        if (id != '') {
            $.ajax({
                url: "/Stations/GetEditTank",
                type: "POST",
                dataType: "json",
                data: data,
                success: function (data) {
                    if (data) {
//                        alert("good");
                        $("#edit-tank-name").val(data["TankName"]);
                        $("#edit-description").val(data["TankDesc"]);
                        $("#edit-length").val(data["Length"]);
                        $("#edit-diameter").val(data["Diameter"]);
                        $("#edit-volume").val(data["Volume"]);
                    }
                }
            });
        }
    });