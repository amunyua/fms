$(document).ready(function () {

});
$(function () {
    //Bootstrap Wizard Validations


    $('#start-shift-form').formValidation({
            framework: 'bootstrap',
            icon: {
//                valid: 'glyphicon glyphicon-ok',
//                invalid: 'glyphicon glyphicon-remove',
//                validating: 'glyphicon glyphicon-refresh'
            },
            // This option will not ignore invisible fields which belong to inactive panels
            //            excluded: ':disabled',
            fields: {
                ShiftCategoryId: {
                    validators: {
                        notEmpty: {
                            message: 'Shift to start is required'
                        }
                    }
                },
                ShiftStartTIme: {
                    validators: {
                        notEmpty: {
                            message: 'Start Time is required'
                        }
                    }
                },Height: {
                    validators: {
                        notEmpty: {
                            message: 'Height reading is required'
                        }
                    }
                },
                manualReading: {
                    validators: {
                        notEmpty: {
                            message: 'Reading is required'
                        }
                    }
                },
                ElectronicReading: {
                    validators: {
                        notEmpty: {
                            message: 'Reading is required'
                        }
                    }
                }
            }
        }).bootstrapWizard({
            'tabClass': 'form-wizard',
            onTabClick: function (tab, navigation, index) {
//                alert(index);
                return validateTab(index);
            },
            'onNext': function (tab, navigation, index) {
                var numTabs = $('#start-shift-form').find('.tab-pane').length,
                    isValidTab = validateTab(index - 1);
                
                if (index === 1) {
                    
                    $('#start-shift-form').find('.form-wizard').children('li').eq(index - 1).addClass(
                    'complete');
                    $('#start-shift-form').find('.form-wizard').children('li').eq(index - 1).find('.step')
                    .html('<i class="fa fa-check"></i>');
                }
                if (index === 2) {
                    //                    alert(index);
                    if (isValidTab) {
                        $('#start-shift-form').find('.form-wizard').children('li').eq(index - 1).addClass(
                            'complete');
                        $('#start-shift-form').find('.form-wizard').children('li').eq(index - 1).find('.step')
                            .html('<i class="fa fa-check"></i>');
                        //                    saveShiftOpeningDetails();
                        $('#start-shift-form').formValidation('defaultSubmit');
                    }
                    

//                    isValidTab = false;
                }

                if (index === numTabs) {
                    if (isValidTab) {
                        saveDipReadings();
                    }
                }
                if (!isValidTab) {
                    return false;
                }
                return true;
            },
            onPrevious: function (tab, navigation, index) {
                alert(index);
                return validateTab(index + 1);
            }, onTabShow: function (tab, navigation, index) {
                $('#shift-overlay').attr('style','display:none');
                // Update the label of Next button when we are at the last tab
                var numTabs = $('#start-shift-form').find('.tab-pane').length;
                $('#start-shift-form')
                    .find('.next')
                    .removeClass('disabled')    // Enable the Next button
                    .find('a')
                    .html(index === numTabs - 1 ? 'Complete' : 'Next');
                
            }
        });

    function validateTab(index) {
        var fv = $('#start-shift-form').data('formValidation'), // FormValidation instance
            // The current tab
            $tab = $('#start-shift-form').find('.tab-pane').eq(index);

        // Validate the container
        fv.validateContainer($tab);

        var isValidStep = fv.isValidContainer($tab);
        if (isValidStep === false || isValidStep === null) {
            // Do not jump to the target tab
            return false;
        }

        return true;

    }

    $('#start-shift-form').on('submit', function(e) {
        e.preventDefault();
        saveShiftOpeningDetails();
    });

    function saveShiftOpeningDetails() {
        //here collect all the filled data
        var shiftDetails = []; var nozzleDetails = [];
        var shiftId = $("#shift-id").val();
        var startTime = $("#start-time").val();

        $('.outer-div').each(function (index, element) {

            $.each($(element).find(".nozzle-div"),
                function (index, actualElement) {
                    nozzleDetails.push({
                        "NozzleId": $(actualElement).find(".nozzle-id").val(),
                        "ManualReading": $(actualElement).find(".manual-reading").val(),
                        "ElectronicReading": $(actualElement).find(".electronic-reading").val(),
                        "StaffId": $(element).find(".attendant").val(),
                        'PumpId': $(element).find(".pump-id-get").val(),
                        "ShiftId": shiftId,
                        "StartTime": startTime
                    });

                });

            //            shiftDetails.push(nozzleDetails);
        });
        console.log(nozzleDetails);
        var data = {
            "shiftDetails": nozzleDetails
        }
        var isValid = false;
        $.ajax({
            url: "/StationHome/OpenShiftDetails",
            dataType: "json",
            type: "POST",
            data: data,
            beforeSend: function () {
                $("div .overlay").removeAttr("style");
            },
            success: function (data) {
                if (data.status != "success") {

                    $("div .overlay").attr("style", "display:none");
                    $("#alert-warning-message").html(data.message);
                    $('#start-shift-alert').fadeIn();
//                    return false;
                } else {
                    isValid = true;
                    toastr.success(data.message, "Success!");
                    setTimeout(function () {
                            //                                 window.location.href = "/StationHome/StationHome";
                            $("div .overlay").hide('slow');
                        },
                        500);
                }
            }
        });

        return isValid;
    }

    function saveDipReadings() {
//        var id = $('#shift-id').val();
        var readingDetails = [];
        $.each($(".tank-row-div"),
            function (index, element) {
                readingDetails.push({
                    "TankId": $(element).find(".tank-id").val(),
                    "Height": $(element).find(".height").val()
                });
            });
        //        console.log(readingDetails);
        var data = {
            "trModels":readingDetails
        }
        $.ajax({
            url: "/StationHome/UpdateFuelLevels",
            dataType: "json",
            type: "POST",
            data: data,
            beforeSend: function() {
                $("div .overlay").fadeIn('slow');
            },
            success: function(data) {
                if (data.status == 'success') {
                    toastr.success(data.message, 'Success!');
                    setTimeout(function () {
                        window.location.href = "/StationHome/StationHome";
                        },
                        1000);
                } else {
                    $("div .overlay").fadeOut('slow');
                    toastr.error(data.message, 'Failed!');
                }
            }
            
        });
    }
    

  

});
