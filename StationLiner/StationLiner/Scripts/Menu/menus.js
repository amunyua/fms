$(document).ready(function () {
    setTimeout(function () {
        $('body').find('.alert').fadeOut("slow");
    }, 3000);
    $('#menus').DataTable({
        "serverSide": true,
        "processing": true,
        "ajax": {
            "url": '/Menu/GetMenus',
            "dataType": 'json'
        },
        "columns": [
       { "data": "id" },
       { "data": "Parent_id" },
       { "data": "Controller" },
       { "data": "Action" },
       { "data": "Icon" },
       { "data": "Sequence" },
       { "data": "Menu_name" },
       { "data": "Status" }
        ],
    })
});

$('.delete-menu-btn').on('click', function () {
    var action = $(this).attr('action');
    $('#delete-menu-form').removeAttr('action').attr('action', action);
})