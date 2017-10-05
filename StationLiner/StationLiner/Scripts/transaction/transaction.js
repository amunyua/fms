$(document).on('change', ':file', function () {
//    alert('changed');
    var input = $(this),
        numFiles = input.get(0).files ? input.get(0).files.length : 1,
        label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
    input.parent('.input-label').find('.message').text(label);
    input.trigger('fileselect', [numFiles, label]);
//    console.log(input.parent('.input-label').find('.message'));
});
//$(document).ready(function () {
//    $(':file').on('fileselect', function (event, numFiles, label) {
//        console.log(numFiles);
//        console.log(label);
//    });
//});

$('#WarehouseId').on('change',
    function() {
        var warehouseId = $(this).val();
        $('#ProductId').html("");
        $('#available-amount').val("");
        $('#MaximumCapacity').val("");
        if (warehouseId != '') {
            $('#product-div').show('slow');
            $('#product').attr('required', 'required');

            //here get all the products in that warehouse
            var data = {
                'warehouseId':warehouseId
            }
            $.ajax({
                url: "/Stock/WarehouseProducts",
                type: 'POST',
                dataType: 'json',
                data: data,
                success:function(data) {
                    if (data.length>0) {
                        var html ="";
                        for (i = 0; i < data.length; i++) {
                            html = '<option value="' + data[i]["Id"] + '">'+ data[i]["ProdId"]["Product_Name"]+'</option>';
                        }
                        $('#ProductId').html(html).trigger("change");

                        $('#available-amount').val(data[0]["AvailableStock"]);
                        $('#MaximumCapacity').val(data[0]["Warehouses"]["MaximumCapacity"]);

                    }
                }
        });
        }
    });

$('#transaction-type').on('change',
    function () {
        var type = $(this).val();
        if (type == 'PURCHASE') {
            showHide($('.purchase'), "show");
        } else {
            showHide($('.purchase'));
        }
    });

$('#transaction-category').on('change',
    function() {
        var cat = $(this).val();
//        alert(cat);
        if (cat == 'fuel') {
            showHide($('.fuel'), "show");
        } else {
            showHide($('.fuel'));
        }
    });

$('#supplier').on('change',
    function () {
        var empty = "";
        $('#vehicle').html(empty);
        $('#driver').html(empty);
        var supplierId = $(this).val();
        if (supplierId != '') {
            //            alert(supplierId);
           
            var data = {
                'supplierId': supplierId
            }
            $.ajax({
                url: "/Stock/SupplierVehicles",
                type: 'POST',
                dataType: 'json',
                data: data,
                success: function (data) {
                    if (data.length > 0) {
                        var html1 = '<option value="">--Select vehicle--</option>';
                        for (i = 0; i < data.length; i++) {
                            html1 += '<option value="' + data[i]['Id'] + '">' + data[i]["RegNumber"] + '</option>';
                            $('#vehicle').html(html1);
                        }
                    } else {
                        var dat = '<option value="">--Please Register atleast One vehicle--</option>';
                        $('#vehicle').html(dat);
                    }
                }
            });
            $.ajax({
                url: "/Stock/SupplierDrivers",
                type: 'POST',
                dataType: 'json',
                data: data,
                success: function (data) {
                    if (data.length > 0) {
                        var html1 = '<option value="">--Select Driver--</option>';
                        for (i = 0; i < data.length; i++) {
                            html1 += '<option value="' + data[i]['Id'] + '">' + data[i]["DriverName"] + '</option>';
                            $('#driver').html(html1);
                        }
                    }
                }
            });
        }
    });

function showHide(element,action) {
    if (action == 'show') {
        element.show('slow');
    } else {
        element.hide("slow");
    }
}

//submit wizard
$('#submit').on('click',
    function() {
        alert('clicked');
    });

$('#ProductId').on('change', function () {
    $('#price-per-liter').val('');
    var productId = $(this).val();
    var supplierId = $('#supplier').val();
    if (supplierId == '') {
        $('#ProductId').val('');
        alert('You must choose a supplier first');
    } else {
        if (productId != '') {
            var data = {
                'productId': productId,
                'supplierId': supplierId
            }
            $.ajax({
                url: "/Stock/SupplierPrices",
                type: 'POST',
                dataType: 'json',
                data: data,
                success: function(data) {
                    if (data.length > 0) {
                        $('#price-per-liter').val(data[0]["Price"]);
                    } else {
                        alert("Item does not belong to supplier, please attach it first");
                        $('#ProductId').val('');
                    }
                }
            });
        }
    }
});

