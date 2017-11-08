
//$("div .overlay").removeAttr("style");

$("#open-shift-form").on("submit",
    function (e) {
        e.preventDefault();
        //        alert("submitting");

        //here collect all the filled data
        var shiftDetails = []; var nozzleDetails = [];
        $('#shifts-tbl > tbody  > tr').each(function (index, element) {
            
            $.each($(element).find(".nozzle-div"),
                function (index, actualElement) {
                    nozzleDetails.push({
                        "NozzleId": $(actualElement).find(".nozzle-id").val(),
                        "ManualReading": $(actualElement).find(".manual-reading").val(),
                        "ElectronicReading": $(actualElement).find(".electronic-reading").val(),
                        "StaffId": $(element).find(".attendant").val(),
                        'PumpId': $(element).find(".pump-id-get").val()
                    });
                    
                });

//            shiftDetails.push(nozzleDetails);
        });
        console.log(nozzleDetails);
        var data = {
            "shiftDetails": nozzleDetails
        }
        $.ajax({
            url: "/StationHome/OpenShiftDetails",
            dataType: "json",
            type: "POST",
            data: data,
            beforeSend: function() {
                $("div .overlay").removeAttr("style");
            },
            success: function(data) {
                if (data != "success") {
                    $("div .overlay").attr("style", "display:none");
                } else {
                    setTimeout(function() {
                            window.location.href = "/StationHome/StationHome";
                    },
                    3000);
                }
            }

            
        });
    });


