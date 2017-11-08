//#region GeneralFunctions
function DisableButton(b) {
    if ($('form').valid()) {
        b.disabled = true;
        $(".overlay").show();
        b.form.submit();
    }
}
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

//#region ResetPasswordForm
function ResetPasswordFormLoad() {
    var validator = false;
    validator = $('#ResetPasswordForm').validate({
        errorClass: "error",
        rules: {
            "Customer_Password": { required: true, maxlength: 15, rangelength: [6, 15] },
            "confPassword": { required: true, maxlength: 15, rangelength: [6, 15], equalTo: "#Customer_Password" }
        },
        messages: {
            "Customer_Password": { required: "", maxlength: "", rangelength: "Password is too short" },
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
        url: "/Customer/IsCustomerExists",
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
        url: "/Customer/IsCustomerPhoneNoExists",
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
//#region AddCustomerFormLoad
function AddCustomerFormLoad() {
    var validator = false;
    var id = $("#Item1_Customer_id").val();
    validator = $('#AddCustomerForm').validate({
        errorClass: "error",
        onkeyup: false,
        rules: {
            "Item1.Customer_Name": { required: true, maxlength: 50 },
            "Item1.Customer_OtherName": { required: true, maxlength: 50 },
            "Item1.Customer_DoB": { required: true },
            "Item1.Customer_Email": { required: true, maxlength: 50, UniqueCustomerEmail: [id], email: true },
            "Item1.Customer_Phone": { required: true, maxlength: 30, UniqueCustomerPhone: [id] },
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
        url: "/Customer/IsCustomerEmpExists",
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
        url: "/Customer/IsEmpEmailExists",
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
        url: "/Customer/IsEmpPhoneExists",
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
            "Emp_Name": { required: true, maxlength: 50, UniqueCustomerEmp: [id, custid] },
            "Emp_Description": { required: true, maxlength: 250 },
            "Emp_Email": { required: true, email: true, maxlength: 50, UniqueEmpEmail: [id, custid] },
            "Emp_Phone": { required: true, maxlength: 30, UniqueEmpPhone: [id, custid] }
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

//#region SetAccUsageLimitForm
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