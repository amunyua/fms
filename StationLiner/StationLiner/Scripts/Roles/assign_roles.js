$('.assign-role-btn').on('click',
    function() {
        var roleName = $(this).attr('role-name');
        var roleId = $(this).attr('role-id');
        $('#role-id').val(roleId);
        $('#role-name').html(roleName);
        //check already assigned roles
        $.each($('.assigned-unassigned'),
           function (index, element) {
               var child_id = $(element).val();
//               console.log(child_id);
               var data = {
                   'roleId': roleId,
                   'child_id': child_id
               }

               $.ajax({
                   url: '/Roles/CheckAttachedMenus',
                   type: 'POST',
                   dataType: 'json',
                   data: data,
                   success: function (data) {
                       //console.log(data)
                       if (data == 'attached') {
                           //alert(data);
//                           $(element).prop('checked', true);
                           $(element).iCheck('check');

                           $.each($(element).closest('li.menu-item-li').find('input:checkbox.individual-permission'),
                               function (key, elmnt) {
                                   var permissionId = $(elmnt).val();
                                   //                                   alert(permissionId);
                                   checIfCrudActionAttached(roleId, child_id, permissionId, elmnt)
                                   //
                                   //
                               });
                           $(element).closest('li.menu-item-li').find('ul.permissions-list').show('slow');
                       } else {
//                           $(element).prop('checked', false);
                           $(element).iCheck('uncheck');
                           $(element).closest('li.menu-item-li').find('ul.permissions-list').hide('slow');
                       }
                   }
               });

           });
    });
$('body').on('ifClicked',
    'input:checkbox.assigned-unassigned',
    function() {
        var roleId = $('#role-id').val();
                var parenId = $(this).closest("li.parent").find('.parent-menu').attr('value');
        var childId = $(this).val();
        if (!$(this).is(":checked")) {
            allocateDisallocateMenu(parenId, childId, roleId, "allocate");

//            //check all boxes where permission is allocated
            $.each($(this).closest('li.menu-item-li').find('input:checkbox.individual-permission'),
                function (key, element) {
                    var permissionId = $(element).val();
//                    alert(permissionId);
                    console.log(checIfCrudActionAttached(roleId, childId, permissionId, element));
//
//
                });
//            //show parmissions
            $(this).closest('li.menu-item-li').find('ul.permissions-list').show('slow');
        } else {
            $.each($(this).closest('li.menu-item-li').find('input:checkbox.individual-permission'),
                function (key, element) {
                    if ($(element).is(':checked')) {

//                        $(element).prop('checked', false);
                        $(element).iCheck("uncheck");
                    }

                });
//            //hide the parmissions div
            $(this).closest('li.menu-item-li').find('ul.permissions-list').hide('slow');
            allocateDisallocateMenu(parenId, childId, roleId, "disallocate");
        }
    });

$('body').on('ifClicked', 'input:checkbox.individual-permission',
    function () {
        
        var roleId = $('#role-id').val();
        var actionId = $(this).val();
        var MenuId = $(this).closest('li.menu-item-li').find('input:checkbox.assigned-unassigned').val();
//        alert(MenuId);

        if (!$(this).is(':checked')) {
                    allocateDisallocateCrudAction(actionId, MenuId, "allocate", roleId);
        } else {
                    allocateDisallocateCrudAction(actionId, MenuId, "dislocate", roleId);
        }

    });


function allocateDisallocateMenu(parentId, childId, roleId, action) {
    var data = {
        'ParentId': parentId,
        'ChildId': childId,
        'RoleId': roleId,
        'Action': action
    }
    $.ajax({
        url: '/Roles/AttachDettachMenu',
        type: 'POST',
        dataType: 'json',
        data: data,
        success: function (data) {
            console.log(data);
            if (data == 'allocate-success') {
//                $('#alert-message').html('The menu has been allocated to this subscription')
//                $('.menu-assign-alert').show();
//                setTimeout(function () {
//                    $('.menu-assign-alert').hide();
//                }, 3000);
            } else {
                if (data == 'dassalocate-success') {

//                    $('#alert-message').html('The menu has been disallocated from this subscription')
//                    $('.menu-assign-alert').show();
//                    setTimeout(function () {
//                        $('.menu-assign-alert').hide();
//                    }, 3000);
                }
            }
        }
    })
}


function checIfCrudActionAttached(s, c, p, e) {
    var data = {
        roleId: s,
        menuId: c,
        permissionId: p
    }
    $.ajax({
        url: '/Roles/CheckAttachedCrudActionToMenu',
        data: data,
        dataType: 'json',
        type: "POST",
        success: function (data) {
            if (data == "true") {
                //                alert(data);
//                console.log(e);
//                $(e).prop('checked', true);
                $(e).iCheck('check');
            }
        }
    });
}

function allocateDisallocateCrudAction(actionId, menuId, action, roleId) {
    var data = {
        actionId: actionId,
        MenuId: menuId,
        roleId: roleId,
        action: action
    }
    $.ajax({
        url: '/Roles/AttachDettachCrudAction',
        dataType: 'json',
        type: "POST",
        data: data,
        success: function (data) {

        }
    });
}

$(".assign-station-role-btn").on("click",
    function() {
        var roleId = $(this).attr("role-id");
        $("#station-role-id-holder").val(roleId);
        checkAssignedStationRoles(roleId);
    });
//$(".station-role").iCheck('');

$(".station-role").on("ifClicked",
    function () {
//        alert('clicked');
        var roleId = $("#station-role-id-holder").val();
        var menuId = $(this).val();
      
        if (!$(this).is(":checked")) {
//            alert('checked');
            assignStationRoles("assign", roleId, menuId);
        } else {
//            alert('Unchecked');
            assignStationRoles("remove", roleId, menuId);
        }
    });

function assignStationRoles(action,roleId,menuId) {
    var data = {
        "Action": action,
        "RoleId": roleId,
        "MenuId":menuId
    }

    $.ajax({
        url: "/Roles/AssignStationRoles",
        dataType: "json",
        type: "POST",
        data: data,
        success: function(data) {

        }
    });
}

function checkAssignedStationRoles(roleId) {
    $.each($(".station-role"),function(index,element) {
        var menuId = $(element).val();
        var data = {
            "roleId": roleId,
            "menuId":menuId
        }
        $.ajax({
            url: "/Roles/CheckStationRoles",
            dataType: "json",
            type: "POST",
            data: data,
            success:function(data) {
                if (data == true) {
//                    $(element).prop('checked', true);
                    $(element).iCheck('check');
//                    $(element).parent('div').addClass('checked');
//                    console.log($(element).parent('div').attr('class'));

//                    alert("checjed");
                }
            }
        });


    });
}