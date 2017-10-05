$('.delete-btn').on('click', function () {
    var action = $(this).attr('action');
    $('#delete-modal-form').attr('action', '');
    $('#delete-modal-form').attr('action', action);

});