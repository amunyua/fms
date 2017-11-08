$(".delete-b-btn").on("click",
    function() {
        var id = $(this).attr("delete-id");
        $("#bank-id").val(id);
    });
$(".edit-b-btn").on("click",
    function() {
        var id = $(this).attr('edit-id');
        $("#edit-bank-id").val(id);
        var data = { "id": id }
        if (id != "") {


            $.ajax({
                url: "/GeneralSettings/GetBankDetaiils",
                dataType: "json",
                type: "POST",
                data: data,
                success: function(data) {
                    if (data) {
                        $("#bank-name").val(data["Bank"]);
                        $("#account-number").val(data["AccountNumber"]);
                        $("#account-name").val(data["AccountName"]);
                        $("#branch").val(data["Branch"]);
                    }
                }
            });
        }
    });

$(".edit-setting").on("click",
    function() {
        var id = $(this).attr('edit-id');
        $("#edit-setting-id").val(id);
        var data = { "id": id }
        if (id != "") {
            $.ajax({
                url: "/GeneralSettings/GetSetting",
                dataType: "json",
                type: "POST",
                data: data,
                success: function(data) {
                    if (data) {
                        $("#company-name").val(data["CompanyName"]);
                        $("#pin").val(data["PinNumber"]);
                        $("#vat").val(data["VAT"]);
                        $("#location").val(data["Location"]);
                        $("#contact1").val(data["Contact1"]);
                        $("#contact2").val(data["Contact2"]);
                    }
                }
            });
        }

    });