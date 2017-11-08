$(document).ready(function () {
    var id = $("#edit-id").val();
    
    $('#recieve-fuel-form').formValidation({
        framework: 'bootstrap',
        icon: {
                            valid: 'glyphicon glyphicon-ok',
                            invalid: 'glyphicon glyphicon-remove',
                            validating: 'glyphicon glyphicon-refresh'
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
            }, Height: {
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
            validateTab(index);
            return false;
        },
        'onNext': function (tab, navigation, index) {
            var numTabs = $('#recieve-fuel-form').find('.tab-pane').length,
                isValidTab = validateTab(index - 1);
            if (!isValidTab) {
                return false;
            }
            collectStepsData("RecieveFuelWizardStore", 1);
            $('#recieve-fuel-form').find('.form-wizard').children('li').eq(index - 1).addClass(
                'complete');
            $('#recieve-fuel-form').find('.form-wizard').children('li').eq(index - 1).find('.step')
                .html('<i class="fa fa-check"></i>');
            if (index === 1) {
                populateFormData();
            }
            if (index === 2) {
                
            }

            if (index === numTabs) {
//                alert(numTabs);
                //                $('#start-shift-form').formValidation('defaultSubmit');
                collectStepsData("CompleteFuelReceiptStore2", numTabs);
            }
            return true;
        },
        onPrevious: function (tab, navigation, index) {
            return validateTab(index + 1);
        }, onTabShow: function (tab, navigation, index) {
            populateFormData();
            $('#shift-overlay').attr('style', 'display:none');
            // Update the label of Next button when we are at the last tab
            var numTabs = $('#recieve-fuel-form').find('.tab-pane').length;
            $('#recieve-fuel-form')
                .find('.next')
                .removeClass('disabled')    // Enable the Next button
                .find('a')
                .html(index === numTabs - 1 ? 'Complete' : 'Next');

        }
    });

    function validateTab(index) {
        var fv = $('#recieve-fuel-form').data('formValidation'), // FormValidation instance
            // The current tab
            $tab = $('#recieve-fuel-form').find('.tab-pane').eq(index);

        // Validate the container
        fv.validateContainer($tab);

        var isValidStep = fv.isValidContainer($tab);
        if (isValidStep === false || isValidStep === null) {
            // Do not jump to the target tab
            return false;
        }


        return true;

    }

    $("#TankId").on("change",
        function () {
            var id = $(this).val();
            if (id != "") {
                //            alert(id);
                var data = {
                    "id": id
                }
                $.ajax({
                    url: "/StationHome/GetProduct",
                    type: "POST",
                    dataType: "json",
                    data: data,
                    success: function (data) {
                        if (data != "") {
                            //                        console.log();
                            $("#prod-id").val(data.Product["ProductName"]);
                        }
                    }
                });
            }
        });

    $('#SupplierId').on("change",
        function () {
            var id = $(this).val();
//                alert(id);
            var data = {
                "id": id
            }
            var drivers = "";
            var vehicles = "";
            $("#supplier-drivers").html(drivers);
            $("#supplier-vehicles").html(vehicles);
            if (id != null) {
                $.ajax({
                    url: "/StationHome/GetFuelDetails",
                    dataType: "json",
                    type: "POST",
                    data: data,
                    success: function(data) {
                        if (data) {
                            if (data.drivers.length > 0) {
                                for (i = 0; i < data.drivers.length; i++) {
                                    drivers += '<option value="' +
                                        data.drivers[i]["Id"] +
                                        '">' +
                                        data.drivers[i]["DriverName"] +
                                        '</option>';
                                }
                                $("#supplier-drivers").html(drivers).trigger("change");
                            } else {
                                $("#supplier-drivers").html('<option value="">No Drivers found</option>').trigger('change');
                            }
                            if (data.vehicles.length > 0) {
                                for (i = 0; i < data.vehicles.length; i++) {
                                    vehicles += '<option value="' +
                                        data.vehicles[i]["Id"] +
                                        '">' +
                                        data.vehicles[i]["RegNumber"] +
                                        '</option>';
                                }
                                $("#supplier-vehicles").html(vehicles).trigger("change");
                            } else {
                                $("#supplier-vehicles").html('<option value="">No vehicles found</option>').trigger('change');
                            }
                        }
                    }

                });
            }
        });

    function collectStepsData(url,step) {
       
        $.ajax({
            url: "/StationHome/"+url,
            type: "POST",
            dataType: "json",
            data: $("#recieve-fuel-form").serialize(),
            beforeSend: function() {
                $("#fuel-overlay").show();
            },
            success: function(data) {
                console.log(data);
                populateFormData();
                setTimeout(function() {
                    $("#fuel-overlay").hide();
                },500);

                if (step === 1) {
                    if (data) {
                        if (data.status == "no-shift") {
                            window.location.href = "/StationHome/GetFuelReciepts";
                        } else if (data.status == "success") {
                            $(".message").html().html(data.message);
                            $("#success-alert").show();
                        } else if (data.status == "failed") {
                            $(".message").html().html(data.message);
                            $("#warning-div").show();
                        }
                    }
                }
                if (step === 4) {
                    if (data.status == "success") {
                        setTimeout(function() {
                            window.location.href = "/StationHome/GetFuelReciepts";
                        },500);
                    }
                }
            }
        });
    }

    function populateFormData() {
        var data = {
            "id": ""
        }
        if (id != "") {
            data = {
                "id": id
            }
        } 
            $.ajax({
                url: "/StationHome/PopulateWizardData",
                type: "POST",
                dataType: "json",
                data:data,
                success: function (data) {
                    if (data) {
                        $("#recieve-fuel-form").find("input, select").each(
                            function (index, element) {
                                var name = $(element).attr('name');
                                if (data.details[name] !== null && data.details[name] !== 0) {
                                    $(element).val(data.details[name]).trigger("change");
                                }
                            });

                    }
                }
            });
        }

});
