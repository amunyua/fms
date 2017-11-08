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
                if (!isValidTab) {
                    return false;
                }
                if (index === 1) {
                    
                    $('#start-shift-form').find('.form-wizard').children('li').eq(index - 1).addClass(
                    'complete');
                    $('#start-shift-form').find('.form-wizard').children('li').eq(index - 1).find('.step')
                    .html('<i class="fa fa-check"></i>');
                }

                if (index === numTabs) {
                    $('#start-shift-form').formValidation('defaultSubmit');
                }
                return true;
            },
            onPrevious: function (tab, navigation, index) {
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
//        $('#shift-overlay').removeAttr('style');

//        setTimeout(function() {
//        },1000);


        return true;

    }


    $("#start-shift-form").on("submit",
        function (e) {
            e.preventDefault();
            //        alert(e);

//            alert("submitting");

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
             $.ajax({
                 url: "/StationHome/OpenShiftDetails",
                 dataType: "json",
                 type: "POST",
                 data: data,
                 beforeSend: function () {
                     $("div .overlay").removeAttr("style");
                 },
                 success: function (data) {
                     if (data != "success") {
                         $("div .overlay").attr("style", "display:none");
                     } else {
                         setTimeout(function () {
                                 window.location.href = "/StationHome/StationHome";
                             },
                             500);
                     }
                 }
 
 
             });
        });

});