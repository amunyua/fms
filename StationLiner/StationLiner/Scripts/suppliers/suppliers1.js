$(document).ready(function () {
//    alert("ok");
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
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            // This option will not ignore invisible fields which belong to inactive panels
//       
        })
        .bootstrapWizard({
            tabClass: 'form-wizard2',
            onTabClick: function (tab, navigation, index) {
                
                var formId = navigation.context.attributes["0"].value;
                return validateTab(index,formId);
            },
            onNext: function (tab, navigation, index) {
                console.log(tab);
                var formId = navigation.context.attributes["0"].value;
//                console.log(formId);
                var numTabs = $('#installationForm').find('.tab-pane').length,
                    isValidTab = validateTab(index - 1,formId);
                if (!isValidTab) {
                    return false;
                }
//                alert("next");
                $('#installationForm').find('.form-wizard').children('li').eq(index - 1).addClass(
                    'complete');
                $('#installationForm').find('.form-wizard').children('li').eq(index - 1).find('.step')
                    .html('<i class="fa fa-check"></i>');
                if (index == "1") {
                    //get all the filled values
                   
                }
                if (index == "2") {
                }
                
                if (index === numTabs) {
                    // We are at the last tab
//                    alert("last");

                    // Uncomment the following line to submit the form using the defaultSubmit() method
                    $('#installationForm').formValidation('defaultSubmit');

                    // For testing purpose
                }

                return true;
            },
            onPrevious: function (tab, navigation, index) {
                var formId = navigation.context.attributes["0"].value;
                var fv = $('#installationForm').data('formValidation'), // FormValidation instance
                    // The current tab
                    $tab = $('#installationForm').find('.tab-pane').eq(index + 1);

                // Validate the container
                fv.validateContainer($tab);

                var isValidStep = fv.isValidContainer($tab);
                return true;
            },
            onTabShow: function (tab, navigation, index) {
//                console.log(navigation);
//                alert('tab');
                // Update the label of Next button when we are at the last tab
                var formId = navigation.context.attributes["0"].value;
                var numTabs = $('#installationForm').find('.tab-pane').length;
                $('#installationForm')
                    .find('.next')
                    .removeClass('disabled')    // Enable the Next button
                    .find('a')
                    .html(index === numTabs - 1 ? 'Complete' : 'Proceed');
                
                // You don't need to care about it
                // It is for the specific demo
                adjustIframeHeight();
            }
        });
    

    function validateTab(index, formValue) {
        var fv = $('#installationForm').data('formValidation'), // FormValidation instance
            // The current tab
            $tab = $('#installationForm').find('.tab-pane').eq(index);
//        console.log($tab);

        // Validate the container
        fv.validateContainer($tab);

        var isValidStep = fv.isValidContainer($tab);
        if (isValidStep === false || isValidStep === null) {
            // Do not jump to the target tab
            return false;
        }

        return true;
    }

    $(".e-sup").on("click",
        function () {
            var id = $(this).attr('ed');
//            console.log(id);
            if (id != "") {
                $("#edit-id-holder").val(id);
                var data = {
                    "id": id
                }
                if (url != "") {


                    $.ajax({
                        url: url,
                        type: "POST",
                        dataType: "json",
                        data: data,
                        success: function (data) {
                            if (data) {
                                $.each(fields,
                                    function (elementId, elementName) {
                                        $("#" + elementName).val(data[elementName]).change();
                                    });
                            }
                        }
                    });
                }
                // here get the fields that we want to fetch data for

            }

        });
    
});

