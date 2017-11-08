//#region GeneralFunctions
function DisableButton(b) {
    if ($('form').valid()) {
        b.disabled = true;
        $(".overlay").show();
        b.form.submit();
    }
}
//#endregion

//#region Password Complexity
$.validator.addMethod("pwcheck", function (value) {
    return /^(?=[\w!@#$%^&*()+]{6,})(?:.*[!@#$%^&*()+]+.*)$/.test(value)
    //return /^[A-Za-z0-9\d=!\-@._*]*$/.test(value) // consists of only these
    //    && /[A-Z]/.test(value) // has a Uppercase letter
    //    && /\d/.test(value) // has a digit
    //    && /[!@#$%^&*]/.test(value)
});
//#endregion

//#region UniqueProdType
$.validator.addMethod("UniqueProdType",
function (value, element, params) {
    $("#loading").show();
    var v = new Object();
    v.ProductType_id = params[0];
    v.ProductType_Name = value;
    var data = JSON.stringify(v);
    var responseText = $.ajax({
        type: "POST",
        url: "/Admin/IsProductTypeExists",
        data: { Data: data },
        async: false,
        dataType: "json",
        success: function () { $("#loading").hide(); }
    }).responseText;
    var jsonResponse = JSON.parse(responseText, null);
    return !(jsonResponse.Result);
},
"This product type is already saved."
);
//#endregion
//#region UniqueProdProp
$.validator.addMethod("UniqueProdProp",
function (value, element, params) {
    $("#loading").show();
    var v = new Object();
    v.ProductProperty_id = params[0];
    v.ProductProperty_Name = value;
    var data = JSON.stringify(v);
    var responseText = $.ajax({
        type: "POST",
        url: "/Admin/IsProductPropExists",
        data: { Data: data },
        async: false,
        dataType: "json",
        success: function () { $("#loading").hide(); }
    }).responseText;
    var jsonResponse = JSON.parse(responseText, null);
    return !(jsonResponse.Result);
},
"This product property is already saved."
);
//#endregion
//#region UniqueProduct
$.validator.addMethod("UniqueProduct",
function (value, element, params) {
    $("#loading").show();
    var v = new Object();
    v.Productid = params[0];
    v.ProductName = value;
    var data = JSON.stringify(v);
    var responseText = $.ajax({
        type: "POST",
        url: "/Admin/IsProductExists",
        data: { Data: data },
        async: false,
        dataType: "json",
        success: function () { $("#loading").hide(); }
    }).responseText;
    var jsonResponse = JSON.parse(responseText, null);
    return !(jsonResponse.Result);
},
"This product is already saved."
);
//#endregion
//#region UniqueGadget
$.validator.addMethod("UniqueGadget",
function (value, element, params) {
    $("#loading").show();
    var v = new Object();
    v.Gadget_id = params[0];
    v.Gadget_Name = value;
    var data = JSON.stringify(v);
    var responseText = $.ajax({
        type: "POST",
        url: "/Admin/IsGadgetExists",
        data: { Data: data },
        async: false,
        dataType: "json",
        success: function () { $("#loading").hide(); }
    }).responseText;
    var jsonResponse = JSON.parse(responseText, null);
    return !(jsonResponse.Result);
},
"This gadget is already saved."
);
//#endregion
//#region UniqueChannelType
$.validator.addMethod("UniqueChannelType",
function (value, element, params) {
    $("#loading").show();
    var v = new Object();
    v.ChannelTypeId = params[0];
    v.ChannelTypeName = value;
    var data = JSON.stringify(v);
    var responseText = $.ajax({
        type: "POST",
        url: "/Admin/IsChannelTypeExists",
        data: { Data: data },
        async: false,
        dataType: "json",
        success: function () { $("#loading").hide(); }
    }).responseText;
    var jsonResponse = JSON.parse(responseText, null);
    return !(jsonResponse.Result);
},
"This channel type is already saved."
);
//#endregion
//#region UniqueChannel
$.validator.addMethod("UniqueChannel",
function (value, element, params) {
    $("#loading").show();
    var v = new Object();
    v.ChannelId = params[0];
    v.ChannelName = value;
    var data = JSON.stringify(v);
    var responseText = $.ajax({
        type: "POST",
        url: "/Admin/IsChannelExists",
        data: { Data: data },
        async: false,
        dataType: "json",
        success: function () { $("#loading").hide(); }
    }).responseText;
    var jsonResponse = JSON.parse(responseText, null);
    return !(jsonResponse.Result);
},
"This channel is already saved."
);
//#endregion
//#region UniqueRole
$.validator.addMethod("UniqueRole",
function (value, element, params) {
    $("#loading").show();
    var v = new Object();
    v.roleid = params[0];
    v.rolename = value;
    var data = JSON.stringify(v);
    var responseText = $.ajax({
        type: "POST",
        url: "/Admin/IsRoleExists",
        data: { Data: data },
        async: false,
        dataType: "json",
        success: function () { $("#loading").hide(); }
    }).responseText;
    var jsonResponse = JSON.parse(responseText, null);
    return !(jsonResponse.Result);
},
"This Role is already saved."
);
//#endregion
//#region UniqueCustomerType
$.validator.addMethod("UniqueCustomerType",
function (value, element, params) {
    $("#loading").show();
    var v = new Object();
    v.CustomerType_id = params[0];
    v.Customer_Type = value;
    var data = JSON.stringify(v);
    var responseText = $.ajax({
        type: "POST",
        url: "/Admin/IsCustomerTypeExists",
        data: { Data: data },
        async: false,
        dataType: "json",
        success: function () { $("#loading").hide(); }
    }).responseText;
    var jsonResponse = JSON.parse(responseText, null);
    return !(jsonResponse.Result);
},
"This customer type is already saved."
);
//#endregion
//#region Numbers only
function numbersOnly(e) {
    if ($.inArray(e.keyCode, [46, 8, 9, 27, 13]) !== -1) {
        return;
    }
    if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
        e.preventDefault();
    }
}
function numbersOnlywithNegative(e) {
    if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 173, 109]) !== -1) {
        return;
    }
    if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
        e.preventDefault();
    }
}
function decimalNumbersOnly(e) {
    if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 190, 110]) !== -1) {
        return;
    }
    if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
        e.preventDefault();
    }
}
function phoneNumbersOnly(e) {
    if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 107, 109, 48, 57]) !== -1) {
        return;
    }
    if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
        e.preventDefault();
    }
}
//#endregion
//#region ContactUsForm GeneralSettingsFormLoad();
function GeneralSettingsFormLoad() {
    var validator = false;
    validator = $('#GeneralSettingsForm').validate({
        errorClass: "error",
        rules: {
            "PostPaidExpValue": { required: true, number: true },
            "MinRedeemPoints": { required: true, number: true },
            "AdminEmail": { required: true, maxlength: 50, email: true },
            "MaxTopupPerDay": { required: true, number: true },
            "MaxPrepaidBalance": { required: true, number: true },
            "DefaultUsageLimit": { required: true, number: true },
            "CCEmail": { required: true, maxlength: 50, email: true }
        },
        messages: {
            'PostPaidExpValue': { required: "" },
            'MinRedeemPoints': { required: "" },
            'AdminEmail': { required: "" },
            'MaxTopupPerDay': { required: "" },
            'MaxPrepaidBalance': { required: "" },
            'DefaultUsageLimit': { required: "" },
            "CCEmail": { required: ""}
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
            $(element.form).find("label[for=" + element.id + "]")
            .addClass(errorClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
            $(element.form).find("label[for=" + element.id + "]")
            .removeClass(errorClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
}
function GeneralSettingsFormNum() {
    $("#PostPaidExpValue").keydown(function (e) {
        numbersOnlywithNegative(e);
    });
    $("#MinRedeemPoints").keydown(function (e) {
        numbersOnlywithNegative(e);
    });
    $("#MaxTopupPerDay").keydown(function (e) {
        numbersOnlywithNegative(e);
    });
    $("#MaxPrepaidBalance").keydown(function (e) {
        numbersOnlywithNegative(e);
    });
    $("#DefaultUsageLimit").keydown(function (e) {
        numbersOnlywithNegative(e);
    });
}
//#endregion
//#region CompanyDetailsFormFormLoad();
function CompanyDetailsFormLoad() {
    var validator = false;
    validator = $('#CompanyDetailsForm').validate({
        errorClass: "error",
        rules: {
            "Company_Name": { required: true, maxlength: 50 },
            "Company_PostalAddress": { maxlength: 500 },
            "Company_Email": { required: true, maxlength: 50, email: true },
            "Company_PostalCode": { maxlength: 50 },
            "Company_City": { maxlength: 50 },
            "Company_PIN": { maxlength: 50 }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
            $(element.form).find("label[for=" + element.id + "]")
            .addClass(errorClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
            $(element.form).find("label[for=" + element.id + "]")
            .removeClass(errorClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
}
//#endregion
//#region CurrencyForm
function CurrencyFormLoad() {
    var validator = false;
    validator = $('#CurrencyForm').validate({
        errorClass: "error",
        rules: {
            "Item1.RatioToBase": { required: true, number: true }
        },
        messages: {
            "Item1.RatioToBase": { required: "Required" }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
}
function CurrencyFormNum() {
    $("#Item1_RatioToBase").keydown(function (e) {
        decimalNumbersOnly(e);
    });
}
//#endregion

//#region AddProductTypeFormLoad()
function AddProductTypeFormLoad() {
    var validator = false;
    var id = $("#ProductType_id").val();
    validator = $('#AddProductTypeForm').validate({
        errorClass: "error",
        onkeyup: false,
        rules: {
            "ProductType_Name": { required: true, maxlength:50, UniqueProdType: [id] },
            "ProductType_Description": { required: true, maxlength: 250 }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
}
//#endregion

//#region AddProductPropertyFormLoad()
function AddProductPropertyFormLoad() {
    var validator = false;
    var id = $("#ProductProperty_id").val();
    validator = $('#AddProductPropertyForm').validate({
        errorClass: "error",
        onkeyup: false,
        rules: {
            "ProductProperty_Name": { required: true, maxlength: 50, UniqueProdProp: [id] },
            "ProductProperty_Description": { required: true, maxlength: 250 }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
}
//#endregion

//#region UniqueTaxCategory
$.validator.addMethod("UniqueTaxCategory",
function (value, element, params) {
    $("#loading").show();
    var v = new Object();
    v.CatID = params[0];
    v.CatName = value;
    var data = JSON.stringify(v);
    var responseText = $.ajax({
        type: "POST",
        url: "/Admin/IsTaxCategoryExists",
        data: { Data: data },
        async: false,
        dataType: "json",
        success: function () { $("#loading").hide(); }
    }).responseText;
    var jsonResponse = JSON.parse(responseText, null);
    return !(jsonResponse.Result);
},
"This tax category already exists."
);
//#endregion
//#region AddTaxCategoryFormLoad()
function AddTaxCategoryFormLoad() {
    var validator = false;
    var id = $("#CatID").val();
    validator = $('#AddTaxCategoryForm').validate({
        errorClass: "error",
        onkeyup: false,
        rules: {
            "CatName": { required: true, maxlength: 50, UniqueTaxCategory: [id] },
            "CatDescription": { required: true, maxlength: 250 }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
}
//#endregion

//#region UniqueTaxRateName
$.validator.addMethod("UniqueTaxRateName",
function (value, element, params) {
    $("#loading").show();
    var v = new Object();
    v.TaxRateID = params[0];
    v.TaxRateName = value;
    var data = JSON.stringify(v);
    var responseText = $.ajax({
        type: "POST",
        url: "/Admin/IsTaxRateNameExists",
        data: { Data: data },
        async: false,
        dataType: "json",
        success: function () { $("#loading").hide(); }
    }).responseText;
    var jsonResponse = JSON.parse(responseText, null);
    return !(jsonResponse.Result);
},
"This tax rate already exists."
);
//#endregion
//#region AddTaxRateFormLoad()
function AddTaxRateFormLoad() {
    var validator = false;
    var id = $("#TaxRateID").val();
    validator = $('#AddTaxRateForm').validate({
        errorClass: "error",
        onkeyup: false,
        rules: {
            "TaxRateName": { required: true, maxlength: 50, UniqueTaxRateName: [id] },
            "TaxRateDescription": { required: true, maxlength: 250 },
            "TaxRateValue": { required: true, number:true }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
    $("#TaxRateValue").keydown(function (e) {
        decimalNumbersOnly(e);
    });
}
//#endregion

//#region UniqueCustomerTaxCategory
$.validator.addMethod("UniqueCustomerTaxCategory",
function (value, element, params) {
    $("#loading").show();
    var v = new Object();
    v.CustCatID = params[0];
    v.CustCatName = value;
    var data = JSON.stringify(v);
    var responseText = $.ajax({
        type: "POST",
        url: "/Admin/IsCustomerTaxCategoryExists",
        data: { Data: data },
        async: false,
        dataType: "json",
        success: function () { $("#loading").hide(); }
    }).responseText;
    var jsonResponse = JSON.parse(responseText, null);
    return !(jsonResponse.Result);
},
"This tax category already exists."
);
//#endregion
//#region AddCustomerTaxCategoryFormLoad()
function AddCustomerTaxCategoryFormLoad() {
    var validator = false;
    var id = $("#CustCatID").val();
    validator = $('#AddCustomerTaxCategoryForm').validate({
        errorClass: "error",
        onkeyup: false,
        rules: {
            "CustCatName": { required: true, maxlength: 50, UniqueCustomerTaxCategory: [id] },
            "CustCatDescription": { required: true, maxlength: 250 }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
}
//#endregion

//#region UniqueTaxName
$.validator.addMethod("UniqueTaxName",
function (value, element, params) {
    $("#loading").show();
    var v = new Object();
    v.TaxID = params[0];
    v.TaxName = value;
    var data = JSON.stringify(v);
    var responseText = $.ajax({
        type: "POST",
        url: "/Admin/IsTaxNameExists",
        data: { Data: data },
        async: false,
        dataType: "json",
        success: function () { $("#loading").hide(); }
    }).responseText;
    var jsonResponse = JSON.parse(responseText, null);
    return !(jsonResponse.Result);
},
"This tax name already exists."
);
//#endregion
//#region AddCustomerTaxCategoryFormLoad()
function AddTaxFormLoad() {
    var validator = false;
    var id = $("#TaxID").val();
    validator = $('#AddTaxForm').validate({
        errorClass: "error",
        onkeyup: false,
        rules: {
            "TaxName": { required: true, maxlength: 50, UniqueTaxName: [id] },
            "TaxDescription": { required: true, maxlength: 250 }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
}
//#endregion

//#region AddProductFormLoad()
function AddProductFormLoad() {
    var validator = false;
    var id = $("#Item1_Product_id").val();
    validator = $('#AddProductForm').validate({
        errorClass: "error",
        onkeyup: false,
        rules: {
            "Item1.Product_Name": {
                required: true, maxlength: 50, UniqueProduct: [id] },
            "Item1.Product_Description": { required: true, maxlength: 250 },
            "Item1.Product_Price": { required: true, number:true }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
}
//#endregion

//#region UniquePriceList
$.validator.addMethod("UniquePriceList",
function (value, element, params) {
    $("#loading").show();
    var v = new Object();
    v.PriceList_id = params[0];
    v.PriceList_Name = value;
    var data = JSON.stringify(v);
    var responseText = $.ajax({
        type: "POST",
        url: "/Admin/IsPriceListExists",
        data: { Data: data },
        async: false,
        dataType: "json",
        success: function () { $("#loading").hide(); }
    }).responseText;
    var jsonResponse = JSON.parse(responseText, null);
    return !(jsonResponse.Result);
},
"This Price List is already saved."
);
//#endregion
//#region AddPriceListForm
function AddPriceListFormLoad() {
    var validator = false;
    var id = $("#PriceList_id").val();
    validator = $('#AddPriceListForm').validate({
        errorClass: "error",
        onkeyup: false,
        rules: {
            "PriceList_Name": { required: true, maxlength: 50, UniquePriceList: [id] },
            "PriceList_Description": { required: true, maxlength: 250 }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
}
//#endregion

//#region UniqueDiscountList
$.validator.addMethod("UniqueDiscountList",
function (value, element, params) {
    $("#loading").show();
    var v = new Object();
    v.DiscountList_id = params[0];
    v.DiscountList_Name = value;
    var data = JSON.stringify(v);
    var responseText = $.ajax({
        type: "POST",
        url: "/Admin/IsDiscountListExists",
        data: { Data: data },
        async: false,
        dataType: "json",
        success: function () { $("#loading").hide(); }
    }).responseText;
    var jsonResponse = JSON.parse(responseText, null);
    return !(jsonResponse.Result);
},
"This Discount is already saved."
);
//#endregion
//#region AddDiscountListForm
function AddDiscountListFormLoad() {
    var validator = false;
    var id = $("#DiscountList_id").val();
    validator = $('#AddDiscountListForm').validate({
        errorClass: "error",
        onkeyup: false,
        rules: {
            "DiscountList_Name": { required: true, maxlength: 50, UniqueDiscountList: [id] },
            "DiscountList_Description": { required: true, maxlength: 250 }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
}
//#endregion

//#region AddProductPriceListForm
function AddProductPriceListFormLoad() {
    var validator = false;
    validator = $('#AddProductPriceListForm').validate({
        errorClass: "error",
        rules: {
            "Item1.Product_id": { required: true },
            "Item1.Product_Price": { required: true, number: true },
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
    $("#Item1_Product_Price").keydown(function (e) {
        decimalNumbersOnly(e);
    });
}
//#endregion

//#region AddGadgetForm
function AddGadgetFormLoad() {
    var validator = false;
    var id = $("#Gadget_id").val();
    validator = $('#AddGadgetForm').validate({
        errorClass: "error",
        onkeyup: false,
        rules: {
            "Gadget_Name": { required: true, maxlength: 50, UniqueGadget: [id] },
            "Gadget_Description": { required: true, maxlength: 250 }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
}
//#endregion

//#region AddChannelTypeForm
function AddChannelTypeFormLoad() {
    var validator = false;
    var id = $("#ChannelType_id").val();
    validator = $('#AddChannelTypeForm').validate({
        errorClass: "error",
        onkeyup: false,
        rules: {
            "ChannelType_Name": { required: true, maxlength: 50, UniqueChannelType: [id] },
            "ChannelType_Description": { required: true, maxlength: 250 }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
}
//#endregion

//#region AddChannelForm
function AddChannelFormLoad() {
    var validator = false;
    var id = $("#Channel_id").val();
    validator = $('#AddChannelForm').validate({
        errorClass: "error",
        onkeyup: false,
        rules: {
            "Channel_Name": { required: true, maxlength: 50, UniqueChannel: [id] },
            "Channel_Description": { required: true, maxlength: 250 },
            "Channel_Address": { required: true, maxlength: 250 },
            "Channel_City": { required: true, maxlength: 50 }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
}
//#endregion

//#region AddChannelProductsForm
function AddChannelProductsFormLoad() {
    var validator = false;
    validator = $('#AddChannelProductsForm').validate({
        errorClass: "error",
        rules: {
            "chk": { required: true },
            "Item1.Product_id":{required:true},
            "Item1.Product_Price": { required: true, number: true }
        },
        messages:{
            "chk": { required: "" }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
}
function AddChannelProductsFormNum() {
    $("#Item1_Product_Price").keydown(function (e) {
        decimalNumbersOnly(e);
    });
}
//#endregion

//#region AddProductDiscountListForm
function AddProductDiscountListFormLoad() {
    var validator = false;
    validator = $('#AddProductDiscountListForm').validate({
        errorClass: "error",
        rules: {
            "chk": { required: true },
            "Item1.Product_id": { required: true },
            "Item1.Discount_Value": { required: true, number: true }
        },
        messages: {
            "chk": { required: "" }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
    $("#Item1_Discount_Value").keydown(function (e) {
        decimalNumbersOnly(e);
    });
}
//#endregion

//#region AddChannelGadgetsForm
function AddChannelGadgetsFormLoad() {
    var validator = false;
    validator = $('#AddChannelGadgetsForm').validate({
        errorClass: "error",
        rules: {
            "Channel_id": { required: true },
            "Gadget_id": { required: true }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
}
//#endregion

//#region AddRoleForm
function AddRoleFormLoad() {
    var validator = false;
    var id = $("#Item1_Role_id").val();
    validator = $('#AddRoleForm').validate({
        errorClass: "error",
        onkeyup: false,
        rules: {
            "Item1.RoleName": { required: true, maxlength:50, UniqueRole:[id] },
            "Item1.RoleDescription": { required: true, maxlength: 250 },
            "chk": {
                required: function (element) {
                    return $("#hdnPermission").val() == '';
                }
            }
        },
        messages: {
            "chk": { required: "" }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
}
//#endregion

//#region AddCustomerTypesFormLoad
function AddCustomerTypesFormLoad() {
    var validator = false;
    var id = $("#CustomerType_id").val();
    validator = $('#AddCustomerTypesForm').validate({
        errorClass: "error",
        onkeyup: false,
        rules: {
            "Customer_Type": { required: true, maxlength: 50, UniqueCustomerType: [id] },
            "CustomerTypeDescription": { required: true, maxlength: 250 }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
}
//#endregion AddCustomerTypesFormLoad

//#region ResetPasswordForm
function ResetPasswordFormLoad() {
    var validator = false;
    validator = $('#ResetPasswordForm').validate({
        errorClass: "error",
        rules: {
            "Customer_Password": { required: true, maxlength: 15, rangelength: [6, 15], pwcheck:true },
            "confPassword": { required: true, maxlength: 15, rangelength: [6, 15], equalTo: "#Customer_Password" }
        },
        messages:{
            "Customer_Password": { required: "", maxlength: "", rangelength: "Password is too short", pwcheck: "Password must have at least one upper-case character, at least one digit and at least one special character." },
            "confPassword": { required: "", maxlength: "", rangelength: "Password is too short", equalTo: "Confirm password is not same." }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
}
//#endregion

//#region UniqueCustomerEmail
$.validator.addMethod("UniqueCustomerEmail",
function (value, element, params) {
    $("#loading").show();
    var v = new Object();
    v.Customer_id = params[0];
    v.Customer_Email = value;
    var data = JSON.stringify(v);
    var responseText = $.ajax({
        type: "POST",
        url: "/Admin/IsCustomerExists",
        data: { Data: data },
        async: false,
        dataType: "json",
        success: function () { $("#loading").hide(); }
    }).responseText;
    var jsonResponse = JSON.parse(responseText, null);
    return !(jsonResponse.Result);
},
"This email is already exists."
);
//#endregion
//#region UniqueCustomerPhone IsCustomerPhoneNoExists
$.validator.addMethod("UniqueCustomerPhone",
function (value, element, params) {
    $("#loading1").show();
    var v = new Object();
    v.Customer_id = params[0];
    v.Customer_Phone = value;
    var data = JSON.stringify(v);
    var responseText = $.ajax({
        type: "POST",
        url: "/Admin/IsCustomerPhoneNoExists",
        data: { Data: data },
        async: false,
        dataType: "json",
        success: function () { $("#loading1").hide(); }
    }).responseText;
    var jsonResponse = JSON.parse(responseText, null);
    return !(jsonResponse.Result);
},
"This phone number is already exists."
);
//#endregion
//#region UniqueCustomerMobile IsCustomerMobileNoExists
$.validator.addMethod("UniqueCustomerMobile",
function (value, element, params) {
    $("#loading2").show();
    var v = new Object();
    v.Customer_id = params[0];
    v.Customer_Mobile = value;
    var data = JSON.stringify(v);
    var responseText = $.ajax({
        type: "POST",
        url: "/Admin/IsCustomerMobileNoExists",
        data: { Data: data },
        async: false,
        dataType: "json",
        success: function () { $("#loading2").hide(); }
    }).responseText;
    var jsonResponse = JSON.parse(responseText, null);
    return !(jsonResponse.Result);
},
"This mobile number is already exists."
);
//#endregion
//#region AddCustomerFormLoad
function AddCustomerFormLoad() {
    var validator = false;
    var id = $("#Item1_Customer_id").val();
    validator = $('#AddCustomerForm').validate({
        errorClass: "error",
        onkeyup: false,
        rules: {
            "Item1.Customer_FirstName": {required: true, maxlength: 50},
            "Item1.Customer_LastName": { required: true, maxlength: 50 },
            "Item1.Customer_OtherName": { maxlength: 50 },
            "Item1.Customer_Email": { maxlength: 50, UniqueCustomerEmail:[id], email: true},
            "Item1.Customer_Phone": {maxlength: 30, UniqueCustomerPhone: [id] },
            "Item1.Customer_Mobile": {maxlength: 30, UniqueCustomerMobile: [id] },
            "Item1.CompanyName": { required: true, maxlength: 50 },
            "Item1.CompanyRegistrationNo": {maxlength: 50 },
            "Item1.CompanyPIN": { maxlength: 50 },
            "Item1.NOK_Email": { maxlength: 50, email: true }
        },
        messages: {
            "chk": { required: ""}
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
}
function AddCorpCustomerFormLoad() {
    var validator = false;
    var id = $("#Item1_Customer_id").val();
    validator = $('#AddCustomerForm').validate({
        errorClass: "error",
        onkeyup: false,
        rules: {
            "Item1.Customer_FirstName": { required: true, maxlength: 50 },
            "Item1.Customer_LastName": { required: true, maxlength: 50 },
            "Item1.Customer_OtherName": { maxlength: 50 },
            "Item1.Customer_Email": { maxlength: 50, UniqueCustomerEmail: [id], email: true },
            "Item1.Customer_Phone": { required: true, maxlength: 30, UniqueCustomerPhone: [id] },
            "Item1.Customer_Mobile": { maxlength: 30, UniqueCustomerMobile: [id] },
            "Item1.CompanyName": { required: true, maxlength: 50 },
            "Item1.CompanyRegistrationNo": { maxlength: 50 },
            "Item1.CompanyPIN": { maxlength: 50 },
            "Item1.CompanyDirectorEmail1": { maxlength: 50, email: true },
            "Item1.CompanyDirectorEmail2": { maxlength: 50, email: true },
            "Item1.CompanyDirectorEmail3": { maxlength: 50, email: true },
            "Item1.CompanyContactPersonEmail1": { maxlength: 50, email: true },
            "Item1.CompanyContactPersonEmail2": { maxlength: 50, email: true }
        },
        messages: {
            "chk": { required: "" }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
}
function phone() {
    $("#Item1_Customer_Phone").keydown(function (e) {
        phoneNumbersOnly(e);
    });
}
//#endregion

//#region UniqueCustomerEmp
$.validator.addMethod("UniqueCustomerEmp",
function (value, element, params) {
    $("#loading").show();
    var v = new Object();
    v.Emp_id = params[0];
    v.Emp_Customer_id = params[1];
    v.Emp_Name = value;
    var data = JSON.stringify(v);
    var responseText = $.ajax({
        type: "POST",
        url: "/Admin/IsCustomerEmpExists",
        data: { Data: data },
        async: false,
        dataType: "json",
        success: function () { $("#loading").hide(); }
    }).responseText;
    var jsonResponse = JSON.parse(responseText, null);
    return !(jsonResponse.Result);
},
"This employee is already exists."
);
//#endregion
//#region UniqueEmpEmail
$.validator.addMethod("UniqueEmpEmail",
function (value, element, params) {
    $("#loading1").show();
    var v = new Object();
    v.Emp_id = params[0];
    v.Emp_Customer_id = params[1];
    v.Emp_Email = value;
    var data = JSON.stringify(v);
    var responseText = $.ajax({
        type: "POST",
        url: "/Admin/IsEmpEmailExists",
        data: { Data: data },
        async: false,
        dataType: "json",
        success: function () { $("#loading1").hide(); }
    }).responseText;
    var jsonResponse = JSON.parse(responseText, null);
    return !(jsonResponse.Result);
},
"This email is already exists."
);
//#endregion
//#region UniqueEmpPhone
$.validator.addMethod("UniqueEmpPhone",
function (value, element, params) {
    $("#loading2").show();
    var v = new Object();
    v.Emp_id = params[0];
    v.Emp_Customer_id = params[1];
    v.Emp_Phone = value;
    var data = JSON.stringify(v);
    var responseText = $.ajax({
        type: "POST",
        url: "/Admin/IsEmpPhoneExists",
        data: { Data: data },
        async: false,
        dataType: "json",
        success: function () { $("#loading2").hide(); }
    }).responseText;
    var jsonResponse = JSON.parse(responseText, null);
    return !(jsonResponse.Result);
},
"This phone number is already exists."
);
//#endregion
//#region AddEmpFormLoad
function AddEmpFormLoad() {
    var validator = false;
    var id = $("#Emp_id").val();
    var custid = $("#Emp_Customer_id").val();
    validator = $('#AddEmpForm').validate({
        errorClass: "error",
        onkeyup: false,
        rules: {
            "Emp_Name": { required: true, maxlength: 50, UniqueCustomerEmp:[id,custid] },
            "Emp_Description": { maxlength: 250 },
            "Emp_Email": { email: true, maxlength: 50, UniqueEmpEmail:[id,custid] },
            "Emp_Phone": { maxlength: 30, UniqueEmpPhone:[id,custid] }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
    $("#Emp_Phone").keydown(function (e) {
        phoneNumbersOnly(e);
    });
}
//#endregion

//#region UniqueStaffEmail
$.validator.addMethod("UniqueStaffEmail",
function (value, element, params) {
    $("#loading").show();
    var v = new Object();
    v.Staff_id = params[0];
    v.Staff_Email = value;
    var data = JSON.stringify(v);
    var responseText = $.ajax({
        type: "POST",
        url: "/Admin/IsStaffEmailExists",
        data: { Data: data },
        async: false,
        dataType: "json",
        success: function () { $("#loading").hide(); }
    }).responseText;
    var jsonResponse = JSON.parse(responseText, null);
    return !(jsonResponse.Result);
},
"This email already exists."
);
//#endregion
//#region UniqueStaffPhone
$.validator.addMethod("UniqueStaffPhone",
function (value, element, params) {
    $("#loading1").show();
    var v = new Object();
    v.Staff_id = params[0];
    v.Staff_Phone = value;
    var data = JSON.stringify(v);
    var responseText = $.ajax({
        type: "POST",
        url: "/Admin/IsStaffPhoneExists",
        data: { Data: data },
        async: false,
        dataType: "json",
        success: function () { $("#loading1").hide(); }
    }).responseText;
    var jsonResponse = JSON.parse(responseText, null);
    return !(jsonResponse.Result);
},
"This phone number is already exists."
);
//#endregion
//#region Add Staff Form
function AddStaffFormLoad() {
    var validator = false;
    var id = $("#Staff_id").val();
    validator = $('#AddStaffForm').validate({
        errorClass: "error",
        onkeyup: false,
        rules: {
            "Staff_Name": { required: true, maxlength: 50 },
            "Staff_Email": { required: true, maxlength: 50, UniqueStaffEmail: [id], email: true },
            "Staff_Phone": { required: true, UniqueStaffPhone: [id] },
            "Staff_TopupLimitValue": { required: true, number: true }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
}
function phone() {
    $("#Staff_Phone").keydown(function (e) {
        phoneNumbersOnly(e);
    });
    $("#Staff_TopupLimitValue").keydown(function (e) {
        decimalNumbersOnly(e);
    });
}
//#endregion
//#region StaffPasswordReset
function StaffPasswordResetFormLoad() {
    var validator = false;
    validator = $('#StaffPasswordResetForm').validate({
        errorClass: "error",
        rules: {
            "Staff_Password": { required: true, maxlength: 15, rangelength: [6, 15], pwcheck: true },
            "confPassword": { required: true, maxlength: 15, rangelength: [6, 15], equalTo: "#Staff_Password" }
        },
        messages: {
            "Staff_Password": { required: "", maxlength: "", rangelength: "Password is too short", pwcheck: "Password must have at least one upper-case character, at least one digit and at least one special character." },
            "confPassword": { required: "", maxlength: "", rangelength: "Password is too short", equalTo: "Confirm password is not same." }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
}
//#endregion
//#region Assign Cards to CustomerForm AssignCardstoCustomerForm
function AssignCardstoCustomerFormLoad() {
    var validator = false;
    validator = $('#AssignCardstoCustomerForm').validate({
        errorClass: "error",
        rules: {
            "Item1.Card_OverDraftValue": { required: true, number: true },
            "Item1.Card_PrepaidAmt": { required: true, number: true },
            "chk": { required: true }
        },
        messages: {
            "chk": { required: "" }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
}
function DecimalFormNum() {
    $("#Item1_Card_OverDraftValue").keydown(function (e) {
        decimalNumbersOnly(e);
    });
    $("#Item1_Card_PrepaidAmt").keydown(function (e) {
        decimalNumbersOnly(e);
    });
}
//#endregion

//#region EditCustomerCardForm
function EditCustomerCardFormLoad() {
    var validator = false;
    validator = $('#EditCustomerCardForm').validate({
        errorClass: "error",
        rules: {
            "Card_PIN": { required: true, maxlength: 20 },
            "Card_OverDraftValue": { required: true, number: true },
            "Card_PrepaidAmt": { required: true, number: true }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
}
function EditCustomerCardNum() {
    $("#Card_OverDraftValue").keydown(function (e) {
        decimalNumbersOnly(e);
    });
    $("#Card_PrepaidAmt").keydown(function (e) {
        decimalNumbersOnly(e);
    });
}
//#endregion

//#region AssignCardstoEmp
function AssignCardstoEmpFormLoad() {
    var validator = false;
    validator = $('#AssignCardstoEmpForm').validate({
        errorClass: "error",
        rules: {
            "Item1.Card_OverDraftValue": { required: true, number: true },
            "Item1.Card_PrepaidAmt": { required: true, number: true },
            "chk": { required: true }
        },
        messages: {
            "chk": { required: "" }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
}
//#endregion

//#region TopupCardFormLoad
function TopupCardFormLoad() {
    var validator = false;
    validator = $('#TopupCardForm').validate({
        errorClass: "error",
        rules: {
            "Card_Amount": { required: true, number: true }
        },
        messages: {
            "chk": { required: "" }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
    $("#Card_Amount").keydown(function (e) {
        decimalNumbersOnly(e);
    });
}
//#endregion

//#region TopupCustomerAccountForm
function TopupCustomerAccountFormLoad() {
    var validator = false;
    validator = $('#TopupCustomerAccountForm').validate({
        errorClass: "error",
        rules: {
            "Amount": { required: true, number: true }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
    $("#Amount").keydown(function (e) {
        decimalNumbersOnly(e);
    });
}
//#endregion

//#region SetUsageLimitForm
function SetUsageLimitFormLoad() {
    var validator = false;
    validator = $('#SetUsageLimitForm').validate({
        errorClass: "error",
        rules: {
            "Card_UsageLimit_Value": { required: true, number: true }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
    $("#Card_UsageLimit_Value").keydown(function (e) {
        decimalNumbersOnly(e);
    });
}
//#endregion

//#region SetAccUsageLimitForm
function SetupAccUsageLimitFormLoad() {
    var validator = false;
    validator = $('#SetupAccUsageLimitForm').validate({
        errorClass: "error",
        rules: {
            "UsageLimit_Value": { required: true, number: true }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
    $("#UsageLimit_Value").keydown(function (e) {
        decimalNumbersOnly(e);
    });
}
//#endregion

//#region UniqueProdType
$.validator.addMethod("UniqueActivityType",
function (value, element, params) {
    $("#loading").show();
    var v = new Object();
    v.ActivityType_id = params[0];
    v.ActivityType_Name = value;
    var data = JSON.stringify(v);
    var responseText = $.ajax({
        type: "POST",
        url: "/Admin/IsActivityTypeExists",
        data: { Data: data },
        async: false,
        dataType: "json",
        success: function () { $("#loading").hide(); }
    }).responseText;
    var jsonResponse = JSON.parse(responseText, null);
    return !(jsonResponse.Result);
},
"This activity type is already saved."
);
//#endregion
//#region AddActivityTypeFormLoad
function AddActivityTypeFormLoad() {
    var validator = false;
    var id = $("#ActivityType_id").val();
    validator = $('#AddActivityTypeForm').validate({
        errorClass: "error",
        onkeyup: false,
        rules: {
            "ActivityType_Name": { required: true, maxlength: 50, UniqueActivityType: [id] },
            "ActivityType_Description": { required: true, maxlength: 250 }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
}
//#endregion

//#region UniqueCustomerEmp
$.validator.addMethod("UniqueActivity",
function (value, element, params) {
    $("#loading").show();
    var v = new Object();
    v.Activity_id = params[0];
    v.Activity_Name = value;
    var data = JSON.stringify(v);
    var responseText = $.ajax({
        type: "POST",
        url: "/Admin/IsActivityExists",
        data: { Data: data },
        async: false,
        dataType: "json",
        success: function () { $("#loading").hide(); }
    }).responseText;
    var jsonResponse = JSON.parse(responseText, null);
    return !(jsonResponse.Result);
},
"This activity is already exists."
);
//#endregion
//#region AddActivityFormLoad
function AddActivityFormLoad() {
    var validator = false;
    var id = $("#Activity_id").val();
    validator = $('#AddActivityForm').validate({
        errorClass: "error",
        onkeyup: false,
        rules: {
            "Activity_Name": { required: true, maxlength: 50, UniqueActivity: [id] },
            "Activity_Description": { required: true, maxlength: 250 }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
}
//#endregion

//#region UniqueFormulaName
$.validator.addMethod("UniqueFormulaName",
function (value, element, params) {
    $("#loading1").show();
    var v = new Object();
    v.id = params[0];
    v.tierrule_name = value;
    var data = JSON.stringify(v);
    var responseText = $.ajax({
        type: "POST",
        url: "/Admin/IsFormulaNameExists",
        data: { Data: data },
        async: false,
        dataType: "json",
        success: function () { $("#loading1").hide(); }
    }).responseText;
    var jsonResponse = JSON.parse(responseText, null);
    return !(jsonResponse.Result);
},
"This Formula name is already exists."
);
//#endregion
//#region UniqueFormula
$.validator.addMethod("UniqueFormula",
function (value, element, params) {
    $("#loading2").show();
    var v = new Object();
    v.id = params[0];
    v.formula = value;
    var data = JSON.stringify(v);
    var responseText = $.ajax({
        type: "POST",
        url: "/Admin/IsFormulaExists",
        data: { Data: data },
        async: false,
        dataType: "json",
        success: function () { $("#loading2").hide(); }
    }).responseText;
    var jsonResponse = JSON.parse(responseText, null);
    return !(jsonResponse.Result);
},
"This Formula is already exists."
);
//#endregion
//#region AddFormulaForm
function AddFormulaFormLoad() {
    var validator = false;
    var id = $("#id").val();
    validator = $('#AddFormulaForm').validate({
        errorClass: "error",
        onkeyup: false,
        rules: {
            "tierrule_name": { required: true, maxlength: 250, UniqueFormulaName: [id] },
            "formula": { required: true, UniqueFormula: [id] }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
}
//#endregion

//#region UniqueAction
$.validator.addMethod("UniqueAction",
function (value, element, params) {
    $("#loading").show();
    var v = new Object();
    v.id = params[0];
    v.Action_Name = value;
    var data = JSON.stringify(v);
    var responseText = $.ajax({
        type: "POST",
        url: "/Admin/IsActionExists",
        data: { Data: data },
        async: false,
        dataType: "json",
        success: function () { $("#loading").hide(); }
    }).responseText;
    var jsonResponse = JSON.parse(responseText, null);
    return !(jsonResponse.Result);
},
"This Action is already saved."
);
//#endregion
//#region AddActionForm
function AddActionFormLoad() {
    var validator = false;
    var id = $("#id").val();
    validator = $('#AddActionForm').validate({
        errorClass: "error",
        onkeyup: false,
        rules: {
            "Action_Name": { required: true, maxlength: 50, UniqueAction: [id] },
            "Action_Description": { required: true, maxlength: 250 },
            "exclusive": { required: true },
            "actionitem_id": { required: true },
            "arithmetictype":{required: true}
        },
        messages: {
            "exclusive": { required: "" },
            "arithmetictype": { required: "" }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
}
//#endregion

//#region UniqueEvent
$.validator.addMethod("UniqueEvent",
function (value, element, params) {
    $("#loading").show();
    var v = new Object();
    v.Event_id = params[0];
    v.Event_Name = value;
    var data = JSON.stringify(v);
    var responseText = $.ajax({
        type: "POST",
        url: "/Admin/IsEventExists",
        data: { Data: data },
        async: false,
        dataType: "json",
        success: function () { $("#loading").hide(); }
    }).responseText;
    var jsonResponse = JSON.parse(responseText, null);
    return !(jsonResponse.Result);
},
"This Event is already saved."
);
//#endregion
//#region AddEventForm
function AddEventFormLoad() {
    var validator = false;
    var id = $("#Event_id").val();
    validator = $('#AddEventForm').validate({
        errorClass: "error",
        onkeyup: false,
        rules: {
            "Event_Name": { required: true, maxlength: 50, UniqueEvent: [id] },
            "Event_Description": { required: true, maxlength: 250 }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
}
//#endregion

//#region UniqueClass
$.validator.addMethod("UniqueClass",
function (value, element, params) {
    $("#loading").show();
    var v = new Object();
    v.id = params[0];
    v.tierlevel_name = value;
    var data = JSON.stringify(v);
    var responseText = $.ajax({
        type: "POST",
        url: "/Admin/IsClassificationExists",
        data: { Data: data },
        async: false,
        dataType: "json",
        success: function () { $("#loading").hide(); }
    }).responseText;
    var jsonResponse = JSON.parse(responseText, null);
    return !(jsonResponse.Result);
},
"This is already saved."
);
//#endregion
//#region AddEventForm
function AddLoyaltyClassificationFormLoad() {
    var validator = false;
    var id = $("#id").val();
    validator = $('#AddLoyaltyClassificationForm').validate({
        errorClass: "error",
        onkeyup: false,
        rules: {
            "tierlevel_name": { required: true, maxlength: 50, UniqueClass: [id] },
            "tierlevel_description": { required: true, maxlength: 250 }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
}
//#endregion

//#region UniqueProg
$.validator.addMethod("UniqueProg",
function (value, element, params) {
    $("#loading").show();
    var v = new Object();
    v.id = params[0];
    v.Loyalty_Program_Name = value;
    var data = JSON.stringify(v);
    var responseText = $.ajax({
        type: "POST",
        url: "/Admin/IsLoyaltyProgramExists",
        data: { Data: data },
        async: false,
        dataType: "json",
        success: function () { $("#loading").hide(); }
    }).responseText;
    var jsonResponse = JSON.parse(responseText, null);
    return !(jsonResponse.Result);
},
"This is already saved."
);
//#endregion
//#region AddLoyaltyProgramForm
function AddLoyaltyProgramFormLoad() {
    var validator = false;
    var id = $("#id").val();
    validator = $('#AddLoyaltyProgramForm').validate({
        errorClass: "error",
        onkeyup: false,
        rules: {
            "Loyalty_Program_Name": { required: true, maxlength: 50, UniqueProg: [id] }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
}
//#endregion

//#region AddComTemplateFormLoad
function AddComTemplateFormLoad() {
    var validator = false;
    validator = $('#AddComTemplateForm').validate({
        errorClass: "error",
        onkeyup: false,
        ignore: [],
        rules: {
            "Template_Name": { required: true, maxlength: 50 },
            "Template_Content": { required:  function() {
                CKEDITOR.instances.Template_Content.updateElement();
            } }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
}
//#endregion

//#region CreditLimitForm
function CreditLimitFormLoad() {
    var validator = false;
    validator = $('#CreditLimitForm').validate({
        errorClass: "error",
        onkeyup: false,
        ignore: [],
        rules: {
            "Item1.Card_CreditLimit": { required: true, number: true },
            "Item1.Card_CreditLimit_Expiration": { required: true, date: true }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
    $("#Item1_Card_CreditLimit").keydown(function (e) {
        decimalNumbersOnly(e);
    });
}
//#endregion

//#region CreditLimitForm
function CreditLimitPostpaidCustFormLoad() {
    var validator = false;
    validator = $('#CreditLimitPostpaidCustForm').validate({
        errorClass: "error",
        onkeyup: false,
        ignore: [],
        rules: {
            "Item1.CreditLimit": { required: true, number: true },
            "Item1.CreditLimit_Expiration": { required: true, date: true }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
    $("#Item1_CreditLimit").keydown(function (e) {
        decimalNumbersOnly(e);
    });
}
//#endregion

//#region ChangePassword - ChangePassword.cshtml
function ChangePasswordFormLoad() {
    var validator = $('#ChangePasswordForm').validate({
        errorClass: "error",
        rules: {
            'OldPassword': { required: true, maxlength: 15, rangelength: [6, 15] },
            'NewPassword': { required: true, maxlength: 15, rangelength: [6, 15], pwcheck: true },
            'ConfirmPassword': { required: true, maxlength: 15, rangelength: [6, 15], equalTo: "#NewPassword" },
        },
        messages: {
            'OldPassword': { required: "Required" },
            'NewPassword': { required: "Required", pwcheck: "Password must have at least one upper-case character, at least one digit and at least one special character." },
            'ConfirmPassword': { required: "Required" }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
}
//#endregion

//#region AddVehicleFormLoad
$.validator.addMethod("UniqueVehicle",
function (value, element, params) {
    $("#loading").show();
    var v = new Object();
    v.id = params[0];
    v.Vehicle_Make = value;
    var data = JSON.stringify(v);
    var responseText = $.ajax({
        type: "POST",
        url: "/Admin/IsVehicleExists",
        data: { Data: data },
        async: false,
        dataType: "json",
        success: function () { $("#loading").hide(); }
    }).responseText;
    var jsonResponse = JSON.parse(responseText, null);
    return !(jsonResponse.Result);
},
"This vehicle already exists"
);
function AddVehicleFormLoad() {
    var id = $("#Item1_Vehicle_Id").val();
    var validator = false;
    validator = $('#AddVehicleForm').validate({
        errorClass: "error",
        onkeyup: false,
        rules: {
            "Item1.Vehicle_Make": { required: true, maxlength: 50, UniqueVehicle:[id] },
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
    $("#Item1_CreditLimit").keydown(function (e) {
        decimalNumbersOnly(e);
    });
}
//#endregion
//#region AddVehicleModelForm
$.validator.addMethod("UniqueVehicleModel",
function (value, element, params) {
    $("#loading").show();
    var v = new Object();
    v.id = params[0];
    v.VehicleModel_Name = value;
    var data = JSON.stringify(v);
    var responseText = $.ajax({
        type: "POST",
        url: "/Admin/IsVehicleModelExists",
        data: { Data: data },
        async: false,
        dataType: "json",
        success: function () { $("#loading").hide(); }
    }).responseText;
    var jsonResponse = JSON.parse(responseText, null);
    return !(jsonResponse.Result);
},
"This vehicle model is already exists"
);
function AddVehicleModelFormLoad() {
    var id = $("#VehicleModel_Id").val();
    var validator = false;
    validator = $('#AddVehicleModelForm').validate({
        errorClass: "error",
        onkeyup: false,
        rules: {
            "VehicleModel_Name": { required: true, maxlength: 50, UniqueVehicleModel: [id] },
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
    $("#TankCapacity").keydown(function (e) {
        decimalNumbersOnly(e);
    });
}
//#endregion

//#region AddCustomerVehicleFormLoad
function AddCustomerVehicleFormLoad() {
    var validator = false;
    validator = $('#AddCustomerVehicleForm').validate({
        errorClass: "error",
        onkeyup: false,
        rules: {
            "Item1.Vehicle_Id": { required: true },
            "Item1.VehicleModel_Id": { required: true },
            "Item1.VehicleRegNo": { required: true, maxlength: 50 },
            "Item1.TankCapacity": { required: true, number: true },
            "chk": { required: true },
            "Item1.StartTime": { required: true },
            "Item1.EndTime": { required: true },
        },
        messages: {
            "Item1.Vehicle_Id": { required: "" },
            "Item1.VehicleModel_Id": { required: "" },
            "Item1.VehicleRegNo": { required: "" },
            "Item1.TankCapacity": { required: "" },
            "chk": { required: "" },
            "Item1.StartTime": { required: "" },
            "Item1.EndTime": { required: "" },
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
    $("#Item1_TankCapacity").keydown(function (e) {
        decimalNumbersOnly(e);
    });
}
//#endregion

//#region AddActionItemForm
$.validator.addMethod("UniqueActionItem",
function (value, element, params) {
    $("#loading").show();
    var v = new Object();
    v.id = params[0];
    v.actionitem_name = value;
    var data = JSON.stringify(v);
    var responseText = $.ajax({
        type: "POST",
        url: "/Admin/IsActionItemExists",
        data: { Data: data },
        async: false,
        dataType: "json",
        success: function () { $("#loading").hide(); }
    }).responseText;
    var jsonResponse = JSON.parse(responseText, null);
    return !(jsonResponse.Result);
},
"This item is already exists"
);
function AddActionItemFormLoad() {
    var validator = false;
    var id = $("#id").val();
    validator = $('#AddActionItemForm').validate({
        errorClass: "error",
        onkeyup: false,
        rules: {
            "actionitem_name": { required: true, maxlength: 50, UniqueActionItem: [id] },
            "ref_value": { required: true, number: true },
            "description": { maxlength: 250 },
        },
        messages: {
            "actionitem_name": { required: "" },
            "ref_value": { required: "" },
            "description": { required: "" },
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
    $("#Item1_TankCapacity").keydown(function (e) {
        decimalNumbersOnly(e);
    });
}
//#endregion 
//#region AddAccountTypeForm
$.validator.addMethod("UniqueAccountType",
function (value, element, params) {
    $("#loading").show();
    var v = new Object();
    v.id = params[0];
    v.AccType = value;
    var data = JSON.stringify(v);
    var responseText = $.ajax({
        type: "POST",
        url: "/Admin/IsAccountTypeExists",
        data: { Data: data },
        async: false,
        dataType: "json",
        success: function () { $("#loading").hide(); }
    }).responseText;
    var jsonResponse = JSON.parse(responseText, null);
    return !(jsonResponse.Result);
},
"This account type already exists"
);
function AddAccountTypeFormLoad() {
    var validator = false;
    var id = $("#Item1_id").val();
    validator = $('#AddAccountTypeForm').validate({
        errorClass: "error",
        onkeyup: false,
        rules: {
            "Item1.AccType": { required: true, maxlength: 50, UniqueAccountType: [id] },
            "Item1.AccDescription": { required: true, maxlength: 250 },
            "chk": { required: true },
        },
        messages: {
            "Item1.AccType": { required: "" },
            "Item1.AccDescription": { required: "" },
            "chk": { required: "" },
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
    $("#Item1_TankCapacity").keydown(function (e) {
        decimalNumbersOnly(e);
    });
}
//#endregion 

//#region AssignAccountsForm
$.validator.addMethod("UniqueMask",
function (value, element, params) {
    $("#loading").show();
    var v = new Object();
    v.AccountNumber = params[0];
    v.Customer_id = params[2];
    v.mask = value;
    var data = JSON.stringify(v);
    var URL = "";
    if (params[1]=="Card Number")
    {
        URL = "/Admin/IsCardNumberValid";
    }
    else if (params[1] == "Telephone Number")
    {
        URL = "/Admin/IsPhoneNumberValid";
    }
    var responseText = $.ajax({
        type: "POST",
        url: URL,
        data: { Data: data },
        async: false,
        dataType: "json",
        success: function () { $("#loading").hide(); }
    }).responseText;
    var jsonResponse = JSON.parse(responseText, null);
    return (jsonResponse.Result);
},
"Not Valid"
);
function AssignAccountsFormLoad() {
    var validator = false;
    var AccountNumber = $("#AccountNumber").val();
    var MaskType = $("#Mask_Type").val();
    var Customer_id = $("#Customer_id").val();
    validator = $('#AssignAccountsForm').validate({
        errorClass: "error",
        onkeyup: false,
        rules: {
            "Mask": { required: true, maxlength: 50, UniqueMask: [AccountNumber, MaskType, Customer_id] },
            "CreditType": { required: true }
        },
        messages: {
            "CreditType": { required: "" },
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
}
$('#Mask_Type').on('change', function () {
    $('input[name="Mask"]').rules('remove');
    var AccountNumber = $("#AccountNumber").val();
    var MaskType = $("#Mask_Type").val();
    var Customer_id = $("#Customer_id").val();
    $('input[name="Mask"]').rules('add', {
        UniqueMask: [AccountNumber, MaskType, Customer_id],
        required: true
    });
    //$('input[name="Mask"]').valid();  // trigger validation of the text field (optional)
    if (MaskType == "Telephone Number")
    {
        $("#Mask").inputmask("phone", {
            url: "/Content/plugins/input-mask/phone-codes/phone-codes.json"
        });
    }
    else
    {
        $("#Mask").inputmask('remove');
    }
});

var v = $("input:radio[name='CreditType']:checked").val();
if (v == "1") {
    $("#IsCollection").attr("disabled", true);
    $("#iscoltn").hide();
}
$("#CreditType1").on("click", function () {
    $("#IsCollection").removeAttr("disabled");
    $("#iscoltn").show();
});
$("#CreditType2").on("click", function () {
    $("#IsCollection").attr("disabled", true);
    $("#iscoltn").hide();
});
//#endregion 

//#region SetAccUsageLimitForm
function SetLimitCustomerAccountFormLoad() {
    var validator = false;
    validator = $('#SetLimitCustomerAccountForm').validate({
        errorClass: "error",
        rules: {
            "Item1.Account_CreditLimit": { required: true, number: true }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
    $("#Item1_Account_CreditLimit").keydown(function (e) {
        decimalNumbersOnly(e);
    });
}
//#endregion

//#region Transfer Account Form Load
function TransferAccountFormLoad() {
    var validator = false;
    validator = $('#TransferAccountForm').validate({
        errorClass: "error",
        rules: {
            "Item1.Balance": { required: true, number: true }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
    $("#Item1_Balance").keydown(function (e) {
        decimalNumbersOnly(e);
    });
}
//#endregion

//#region UniqueSchedule
$.validator.addMethod("UniqueSchedule",
function (value, element, params) {
    $("#loading").show();
    var v = new Object();
    v.Schedule_Id = params[0];
    v.Schedule_Name = value;
    var data = JSON.stringify(v);
    var responseText = $.ajax({
        type: "POST",
        url: "/Admin/IsScheduleExists",
        data: { Data: data },
        async: false,
        dataType: "json",
        success: function () { $("#loading").hide(); }
    }).responseText;
    var jsonResponse = JSON.parse(responseText, null);
    return !(jsonResponse.Result);
},
"Already exists"
);
//#endregion
//#region AddSchedule Form Load
function AddScheduleFormLoad() {
    var validator = false;
    var id = $("#Schedule_Id").val();
    validator = $('#AddScheduleForm').validate({
        errorClass: "error",
        onkeyup: false,
        rules: {
            "Schedule_Name": { required: true, maxlength: 50, UniqueSchedule:[id] },
            "Effective_Date": { required: true }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
}
//#endregion

//#region AssignPriceListForm
function AssignPriceListFormLoad() {
    var validator = false;
    validator = $('#AssignPriceListForm').validate({
        errorClass: "error",
        onkeyup: false,
        rules: {
            "chk": { required: true }
        },
        messages: {
            "chk": { required: "" }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
}
//#endregion

//#region AddCustomFieldsForm
function AddCustomFieldsFormLoad() {
    var validator = false;
    validator = $('#AddCustomFieldsForm').validate({
        errorClass: "error",
        rules: {
            "Item1.CustomFieldName": { required: true },
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
}
//#endregion

//#region SendMessageForm
function SendMessageFormLoad() {
    var validator = false;
    validator = $('#SendMessageForm').validate({
        errorClass: "error",
        onkeyup: false,
        ignore: [],
        rules: {
            "message_subject": { required: true, maxlength: 50 },
            "message_body": {
                required: function () {
                    CKEDITOR.instances.message_body.updateElement();
                }
            }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });
}
//#endregion

//Added by Adam
//#region Add Channel Products
function AddChannelPricesFormLoad() {
    var validator = false;
    validator = $('#AddChannelPricesForm').validate({
        errorClass: "error",
        rules: {
            // "chk": { required: true },
            "Price": { required: true, number: true }
        },
        /*messages: {
            "chk": { required: "" }
        },*/
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(errorClass).addClass(validClass);
        },
        submitHandler: function (form) {
            if (validator.valid()) {
                form.submit();
            }
        }
    });

}
//#endregion