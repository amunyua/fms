//on click take the station Id
$(".station-btn").on('click',
    function() {
        var stationId = $(this).attr('station-id');
        var name = $(this).attr('station-name');
        $('#stationIdentity').val(stationId);
        $("#put-name").text(name);
        $.each($(".shift-checkbox"),
            function(index, element) {
                var shiftId = $(element).val();
                attachDetach(shiftId, stationId, "check-state", element);
            });
    });

var actions = {
    attach: [],
    detach:[]
}
$(".shift-checkbox").on("ifClicked",
    function () {
       
        var elemnt = $(this);
        var shiftId = elemnt.val();
        var stationId = $("#stationIdentity").val();
        //
        if (!elemnt.is(":checked")) {
            if ($.inArray(shiftId,actions.attach) == -1) {
                actions.attach.push(shiftId);
            }
            //remove elements from the array if they exist
            if ($.inArray(shiftId, actions.detach) != -1) {
                var index = actions.detach.indexOf(shiftId);
                actions.detach.splice(index, 1);
            }
        } else {
            if ($.inArray(shiftId, actions.detach) == -1) {
                actions.detach.push(shiftId);
            }
            //remove the element from the attach array if the element exists

            if ($.inArray(shiftId, actions.attach) != -1) {
                var index = actions.attach.indexOf(shiftId);
                actions.attach.splice(index, 1);
            }
        }
        console.log(actions);

//        if (!elemnt.is(":checked")) {
//            attachDetach(shiftId, stationId, "attach");
//        } else {
//            attachDetach(shiftId, stationId, "detach");
//        }
    });

$("#save-shifts").on("click", function (e) {
    e.preventDefault();
    var stationId = $("#stationIdentity").val();
    //attach the one that the user has chosen
    if (actions.attach.length > 0) {
        
        for (var i = 0; i< actions.attach.length;i++){
            attachDetach(actions.attach[i], stationId, "attach");
//            console.log(actions.attach[i],stationId);
        }
    }

    if (actions.detach.length > 0) {

        for (var i = 0; i < actions.detach.length; i++) {
            attachDetach(actions.detach[i], stationId, "detach");
            //            console.log(actions.attach[i],stationId);
        }
    }
    window.location.href = "/Stations";

});

function attachDetach(shiftId,stationId,action,element) {
    var data =
    {
        "shiftId": shiftId,
        "stationId": stationId,
        "action": action
    };
    $.ajax({
        url: "/Stations/AssignStation",
        type:"POST",
        dataType: 'json',
        data: data,
        success: function(data) {
            if (data == "shift-attached") {
//                $(element).prop('checked', true);
                $(element).iCheck("check");

            }
        }
    });

}