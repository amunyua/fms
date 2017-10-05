/**
 * Created by Alex on 5/8/2017.
 */
$('.assign-station-btn').on('click',
    function() {
        var userName = $(this).attr('user-name');
        var userId = $(this).attr('user-id');
        $('#user-name').html(userName);
        $('#UserId').val(userId);
        checkIfStationIsAssigned(userId);
    });

var actions = {
    attach: [],
    detach: []
}
$('.ChannelId').on('ifClicked',
    function () {
        var element = $(this);
        var channelId = $(this).attr('channel-id');
        var userId = $('#UserId').val();

        if (!$(this).is(":Checked")) {
            if ($.inArray(channelId, actions.attach) == -1) {
                actions.attach.push(channelId);
            }
            if ($.inArray(channelId, actions.detach) != -1) {
                var index = actions.detach.indexOf(channelId);
                actions.detach.splice(index, 1);
            }
        } else {
            if ($.inArray(channelId, actions.detach) == -1) {
                actions.detach.push(channelId);
            }
            //remove the element from the attach array if the element exists

            if ($.inArray(channelId, actions.attach) != -1) {
                var index1 = actions.attach.indexOf(channelId);
                actions.attach.splice(index1, 1);
            }
        }
//        if ($(this).is(":Checked")) {
//            assignRemoveChannel("assign", userId, channelId);
//        } else {
//            assignRemoveChannel("remove", userId, channelId);
        //        }

        
    });
$("#save-station-btn").on("click",
    function(e) {
        e.preventDefault();
        var userId = $('#UserId').val();
        if (actions.attach.length > 0) {

            for (var i = 0; i < actions.attach.length; i++) {
                assignRemoveChannel(actions.attach[i], userId, "assign");
                //            console.log(actions.attach[i],stationId);
            }
        }

        if (actions.detach.length > 0) {

            for (var i = 0; i < actions.detach.length; i++) {
                assignRemoveChannel(actions.detach[i], userId, "remove");
                //            console.log(actions.attach[i],stationId);
            }
        }
        window.location.href = "/Staffs";
    });

function assignRemoveChannel(channelId,userId,action) {
    var data = {
        UserId: userId,
        ChannelId: channelId,
        action: action
    };
    $.ajax({
        url: '/Manage/AllocateDeChannel',
        dataType: 'json',
        type:'POST',
        data:data,
        success: function(data) {

        }
});
}

function checkIfStationIsAssigned(userId) {
//    console.log($('ul#channels'));
    $.each($('.ChannelId'), function(index,element) {
        var channelId = $(element).attr('channel-id');
//        alert(channelId);
        var data = {
            UserId: userId,
            ChannelId: channelId
        };
        $.ajax({
            url: '/Manage/CheckAllocation',
            dataType: 'json',
            type: 'POST',
            data: data,
            success: function(data) {
                if (data == 'allocated') {
//                    $(element).prop('checked', true);
                    $(element).iCheck('check');
                }
            }
        });
    });

}