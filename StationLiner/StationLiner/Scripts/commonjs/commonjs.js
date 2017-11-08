$(document).ready(function () {
//    console.log(url);
    $(".edit-common-btn").on("click",
        function() {
            var id = $(this).attr('edit-common-id');
            console.log(id);
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
                    success: function(data) {
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

