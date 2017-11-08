$(document).ready(function () {
    // You don't need to care about this function
    // It is for the specific demo
    function adjustIframeHeight() {
        var $body = $('body'),
            $iframe = $body.data('iframe.fv');
        if ($iframe) {
            // Adjust the height of iframe
            $iframe.height($body.height());
        }
    }

    $('#installationForm')
        .formValidation({
            framework: 'bootstrap',
            icon: {
               /* valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'*/
            },
            // This option will not ignore invisible fields which belong to inactive panels
//            excluded: ':disabled',
            fields: {
                manual: {
                    validators: {
                        notEmpty: {
                            message: 'Closing manual reading is required'
                        }
                    }
                },
                electronic: {
                    validators: {
                        notEmpty: {
                            message: 'Closing electronic reading is required'
                        }
                    }
                },Height: {
                    validators: {
                        notEmpty: {
                            message: 'Height reading is required'
                        }
                    }
                },
                Amount: {
                    validators: {
                        notEmpty: {
                            message: 'Amount is required'
                        }
                    }
                },
//                sold: {
//                    validators: {
//                        notEmpty: {
//                            message: 'The sold value is required'
//                        }
//                    }
//                }
            }
        })
        .bootstrapWizard({
            tabClass: 'form-wizard',
            onTabClick: function (tab, navigation, index) {
                return validateTab(index);
            },
            onNext: function (tab, navigation, index) {
                var numTabs = $('#installationForm').find('.tab-pane').length,
                    isValidTab = validateTab(index - 1);
                if (!isValidTab) {
                    return false;
                }
                $('#installationForm').find('.form-wizard').children('li').eq(index - 1).addClass(
                    'complete');
                $('#installationForm').find('.form-wizard').children('li').eq(index - 1).find('.step')
                    .html('<i class="fa fa-check"></i>');
                if (index == "1") {
                    //get all the filled values
                    setTimeout(function () {
                            $("div .overlay").attr("style", "display:none");
                        },
                        500);
                }
                if (index == "1") {
                    getTotalExpected();
                }
                if (index == "2") {
                    saveCashDrop();
                }
                
                if (index === numTabs) {
                    
                    saveDipReadings();
                }

                return true;
            },
            onPrevious: function (tab, navigation, index) {
                return validateTab(index + 1);
            },
            onTabShow: function (tab, navigation, index) {
                // Update the label of Next button when we are at the last tab
                var numTabs = $('#installationForm').find('.tab-pane').length;
                $('#installationForm')
                    .find('.next')
                    .removeClass('disabled')    // Enable the Next button
                    .find('a')
                    .html(index === numTabs - 1 ? 'Complete' : 'Save & Proceed');
                if (index === numTabs - 1) {
                    $(".save-readings").removeAttr("style");
                } else {
                    $(".save-readings").attr("style","display:none");
                }
                // You don't need to care about it
                // It is for the specific demo
                adjustIframeHeight();
            }
        });

    function validateTab(index) {
        var fv = $('#installationForm').data('formValidation'), // FormValidation instance
            // The current tab
            $tab = $('#installationForm').find('.tab-pane').eq(index);

        // Validate the container
        fv.validateContainer($tab);

        var isValidStep = fv.isValidContainer($tab);
        if (isValidStep === false || isValidStep === null) {
            // Do not jump to the target tab
            return false;
        }

        return true;
    }
});

$(".manual-reading").on("blur",
    function () {
        var element = $(this);
        var parentDiv = $(this).closest(".readings-div");
        var openingReading = $(this).closest(".readings-div").find(".opening-manual-reading").val();
        var closingReading = $(this).val();
        if (closingReading != "" && openingReading != "") {
            if (parseInt(closingReading) > parseInt(openingReading)) {
                var sold = parseInt(closingReading) - parseInt(openingReading);
                parentDiv.find(".manual-sold").val(sold);
            }
        }
    });

$(".electronic-reading").on("blur",
    function () {
        var element = $(this);
        var parentDiv = $(this).closest(".readings-div");
        var openingReading = $(this).closest(".readings-div").find(".opening-electronic-reading").val();
//        alert(openingReading);
        var closingReading = $(this).val();
//        alert(closingReading);
        if (closingReading != "" && openingReading != "") {
            if (parseInt(closingReading) > parseInt(openingReading)) {
                var sold = parseInt(closingReading) - parseInt(openingReading);
                parentDiv.find(".electronic-sold").val(sold).trigger("blur");
            } else {
                parentDiv.find(".electronic-sold").val("");
            }
        }
    });

function getAllFilledValues(divElement) {
    var closeShiftDetails = [];
    $.each(divElement, function (index, element) {
//        console.log();
        closeShiftDetails.push({
            "NozzleId": $(element).find($(".nozzle-id")).val(),
            "EndManualReading": $(element).find(".manual-reading").val(),
            "EndElectronicReading": $(element).find(".electronic-reading").val(),
            "TestData": $(element).find(".test-data").val(),
            "ReadingId": $(element).find(".reading-id").val()
        });
        
    });
    return closeShiftDetails;
}

$(".save-readings").on("click",
    function () {
        var id = $('#shift-id').val();
        $("div .overlay").removeAttr("style");
        setTimeout(function () {
            window.location.href = "/StationHome/GenerateCloseShiftReport/"+id;
            },
            500);

    });

function saveDipReadings() {
    var id = $('#shift-id').val();
    var readingDetails = [];
    $.each($(".tank-row-div"),
        function(index,element) {
            readingDetails.push({
                "TankId": $(element).find(".tank-id").val(),
                "Height": $(element).find(".height").val()
            });
           

        });

    console.log(readingDetails);
    var data = {
        "Readings":readingDetails
    }

    $.ajax({
        url: "/StationHome/RecordTankReadings",
        dataType: "json",
        type: "POST",
        data: data,
        beforeSend: function () {
            $("div .overlay").removeAttr("style");
        },
        success: function (data) {
            if (data != "success") {
                $("div .overlay").attr("style", "display:none");
                return false;
            } else {
                setTimeout(function () {
                    window.location.href = "/StationHome/GenerateCloseShiftReport/" + id;
                    },
                    500);
            }
        }


    });
}

function saveCashDrop() {
    
    var pumpDetails = getAllFilledValues($(".readings-div"));
    var cashDrops = [];
    $.each($(".cash-drop-div"),
        function (index, element) {
//            console.log();
            $.each($(element).find(".mode-of-payment"),
                function (index, element2) {
                    if ($(element2).val() != "0") {


                        cashDrops.push({
                            "StaffId": $(element).find(".staff-id").val(),
                            "PaymentMode": $(element2).attr("payment-mode"),
                            "Amount": $(element2).val()
                        });
                    }
                });
        });
    var data =  {
        "Cashdrop": cashDrops,
        "CloseDetails":pumpDetails
    }
//    console.log(data);
    $.ajax({
        url: "/StationHome/CloseShift",
        dataType: "json",
        type: "POST",
        data: data,
        beforeSend: function () {
            $("div .overlay").removeAttr("style");
        },
        success: function (data) {
//            console.log(data);
            if (data != "success") {
                $("div .overlay").attr("style", "display:none");
                return false;
            } else {
                setTimeout(function () {
                        $("div .overlay").attr("style", "display:none");
                    },
                    500);
            }
        }
    });
}

$(".p-mode").on("keyup",
    function () {
        var totalCashAtHand = 0;
        var currentContext = $(this).closest("div.cash-drop-div");
        var totalDrops = currentContext.find(".total-drops").val();
        var totalExpected = parseInt(currentContext.find(".total-expected").val());
        $.each(currentContext.find(".p-mode"),
            function (index, element) {
                if ($(element).val() != "") {
                    totalCashAtHand += parseInt($(element).val());
                }
            });
        //                var formatted = $.number(totalCashAtHand, 2);
        var totalcollected = parseInt(totalDrops) + totalCashAtHand;
        var short = totalExpected - totalcollected;

        currentContext.find(".total-at-hand").html("").html(totalCashAtHand.toLocaleString());
        currentContext.find(".total-cash-expected").html("").html(totalExpected.toLocaleString());
        currentContext.find(".total-cash-collected").html("").html(totalcollected.toLocaleString());
        currentContext.find(".short").html("").html(short.toLocaleString());

    });

function getTotalExpected() {
    var pumpDetails = getAllFilledValues($(".readings-div"));
//    console.log(pumpDetails);
    var data = {
        "ClosingPumpDetails": pumpDetails
    }
    $.ajax({
        url: "/StationHome/GetTotalExpectedForEachStaff",
        type: "POST",
        dataType: "json",
        data: data,
        success: function(data) {
            if (data.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    var sId = "#staff" + data[i].staffId;
//                    alert(sId);
                    $(sId).val(data[i].amount);
                }
            }
        }
    });
}


