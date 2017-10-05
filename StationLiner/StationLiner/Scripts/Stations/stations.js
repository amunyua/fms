var stations = {
    resetFields: function(formObj) {
        formObj.find('input[type="text"],input[type="radio"], input[type="email"], input[type="number"], select, textarea').val('').trigger("change");
    },
    showHide: function (action, target) {
        if (action == "show") {
            $(target).show();
        } else {
            $(target).hide();
        }
    }
}

/*var $tanks = $('#tanks').DataTable({
           "serverside":true,
            "ajax":({
                url: "/Stations/TankDetails",
                dataType: 'json',
                type: "GET"
               
            }),
            "columns": [
                { "data": "Id" },
                { "data": "TankName" },
                //{ "data": "Description" },
                { "data": "Length" }
           ]
           
});*/

$(document).ready(function () {
    /*var data = {
        "Id": 10
    }
    $('#pumps').DataTable({
        "destroy": true,
        "serverside": true,
        "order": [[0, "desc"]],
        "ajax": ({
            url: "/Stations/PumpDetails",
            data:data,
            dataType: 'json',
            type: "POST"

        }),
        "columns": [
            { "data": "Name" },
            { "data": "Description" },
            //{ "data": "Description" },
            { "data": "IsDoubleSided" }
        ]

    });*/
    $('#tan').dataTable({
        "bServerSide": true,
        "sAjaxSource": "/Stations/TankDetails",
        "bProcessing": true,
        "aoColumns": [
            {
                "sName": "ID",
                "bSearchable": false,
                "bSortable": false,
                "fnRender": function (oObj) {
                    return '<a href=\"Details/' +
                        oObj.aData[0] + '\">View</a>';
                }
            },
            { "sName": "COMPANY_NAME" },
            { "sName": "ADDRESS" },
            { "sName": "TOWN" }
        ]
    });
});

$(function() {
    //Bootstrap Wizard Validations

    var $validator = $("#basic-details-form").validate({
        highlight: function(element) {
            $(element).closest('.form-group').removeClass('has-success').addClass('has-error');
        },
        unhighlight: function(element) {
            $(element).closest('.form-group').removeClass('has-error').addClass('has-success');
        },
        errorElement: 'span',
        errorClass: 'help-block',
        errorPlacement: function(error, element) {
            if (element.parent('.input-group').length) {
                error.insertAfter(element.parent());
            } else {
                error.insertAfter(element);
            }
        }
    });

    $('#bootstrap-wizard-2').bootstrapWizard({
        'tabClass': 'form-wizard',
        'onNext': function(tab, navigation, index) {

            var $valid = $("#basic-details-form").valid();
            if (!$valid) {
                $validator.focusInvalid();
                return false;
            } else {
                //evaluate whether user is on the first page
                if (index === 1) {
                    $.ajax({
                        type: "POST",
                        url: "/Stations/StationBasicDetails",
                        data:$("#basic-details-form").serialize(), // serializes the form's elements.
                        success: function(data) {
                            if (data != "Failed") {
                                $(".StationId").val(data);
                                $(".save-btn").show();

                            } else {
                                return false;
                            }
                        }
                    });
                }
                $('#bootstrap-wizard-2').find('.form-wizard').children('li').eq(index - 1).addClass(
                    'complete');
                $('#bootstrap-wizard-2').find('.form-wizard').children('li').eq(index - 1).find('.step')
                    .html('<i class="fa fa-check"></i>');
            }
        }
    });

});
$('#save-tanks-btn').on("click",
    function(e) {
//        
        if ($('#add-tanks-form')[0].checkValidity()) {
            var id = $(".StationId").val();
            //submit the form at this point
            $.ajax({
                type: "POST",
                url: "/Stations/AddTank",
                data: $("#add-tanks-form").serialize(), // serializes the form's elements.
                success: function(data) {
                    if (data == "success") {
                        id = $(".StationId").val();
                        stations.resetFields($("#add-tanks-form"));
                        $(".StationId").val(id);
                        $("#tanks-modal .close").click();
                 }   
                }
            });
            //Added
            $('#tanks').DataTable({
                "destroy": true,
                "serverside": true,
                "order": [[0, "desc"]],
                "ajax": ({
                    url: "/Stations/TankDetails",
                    data : function ( d ) {
                        d.id= id;
                    },
                    dataType: 'json',
                    type: "GET"

                }),
                "columns": [
//                    { "data": "Id" },
                    { "data": "TankName" },
                    //{ "data": "Description" },
                    { "data": "Length" }
                ]

            });
            /////
            e.preventDefault();
        }
//        return false;
    });

$('#Sides').on('change',
    function() {
        var value = $(this).val();
        if (value == 2) {
            stations.showHide('show', '#side-b-div');
        } else {
            stations.showHide('hide', '#side-b-div');
        }
    });

$(".fuel-type").on("change",
    function() {
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
                    success: function(data) {
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
                            select.attr("required","required").html(html);
                            select.parent(".select-par-div").show();
                        }
                    }
                });
            } else {
                object.closest(".parent-div").find(".fuel-type-select").removeAttr("required").html("").parent(".select-par-div").hide();
            }
        }
    });

//code to save pump data
$("#save-pump-btn").on("click",
    function (e) {
//        alert("clicked");
        if ($('#add-pumps-form')[0].checkValidity()) {
            var id = $(".StationId").val();
            //submit the form at this point
            $.ajax({
                type: "POST",
                url: "/Stations/AddPumpDetails",
                data: $("#add-pumps-form").serialize(), // serializes the form's elements.
                success: function (data) {
                    if (data == "success") {
                        var id = $(".StationId").val();
                        stations.resetFields($("#add-pumps-form"));
                        $(".StationId").val(id);
                        $("#pumps-modal .close").click();
                        $('.fuel-type').attr('checked', false).closest(".parent-div").find(".fuel-type-select").removeAttr("required").html("").parent(".select-par-div").hide();

                        $(".fuel-type-select").hide();
//                        $("#pumps-modal").hide();
                    }
                }
            });
            //            //Added
//            alert(id);
            var data = {
                "Id": id
              }
                $('#pumps').DataTable({
                    "destroy": true,
                    "serverside": true,
                    "order": [[0, "desc"]],
                    "ajax": ({
                        url: "/Stations/PumpDetails",
                        data:data,
                        dataType: 'json',
                        type: "POST"

                    }),
                    "columns": [
                        { "data": "Name" },
                        { "data": "Description" },
                        //{ "data": "Description" },
//                        { "data": "IsDoubleSided" }
                    ]

                });
            e.preventDefault();

        }
    });

$(".edit-tank-btn").on("click",
    function() {
        var id = $(this).attr('edit-id');
        alert(id);
        var data = {
            "id":id
        }
        if (id != '') {
            $.ajax({
                url: "/Stations/GetEditTank",
                type: "POST",
                dataType: "json",
                data: data,
                success: function(data) {
                    
                }
            });
        }
    });

