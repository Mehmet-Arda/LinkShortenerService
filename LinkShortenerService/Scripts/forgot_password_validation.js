
$(".forgot-password-done").hide();


function validateEmail() {


    var inputEmail = document.querySelector("#email");
    var emailSendBtn = document.querySelector("#emailSendBtn");
    var emailValidationMessage = document.querySelector("#emailValidationMessage");
    var check_email = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;



    if (inputEmail.value == "") {
        emailValidationMessage.innerText = "Email alanı boş geçilemez.";
        inputEmail.classList.add("error");
        return false;
    }
    else if (!check_email.test(inputEmail.value)) {

        emailValidationMessage.innerText = "Lütfen geçerli bir email adresi giriniz.";
        inputEmail.classList.add("error");
        return false;

    } else {
        emailValidationMessage.innerText = "";
        inputEmail.classList.remove("error");
        return true;
    }
}


function validateCode() {


    var inputValidationCode = document.querySelector("#validationCode");
    var validationCodeValidationMessage = document.querySelector("#validationCodeValidationMessage");
    var validationCodeSendBtn = document.querySelector("#validationCodeSendBtn");
    var check_validationCode = /[0-9A-Z]{6}/;



    if (inputValidationCode.value == "") {
        inputValidationCode.classList.add("error");
        validationCodeValidationMessage.innerText = "Doğrulama alanı boş geçilemez.";
        return false;

    } else if (inputValidationCode.value.length < 6) {
        inputValidationCode.classList.add("error");
        validationCodeValidationMessage.innerText = "Doğrulama kodu 6 haneli olmalıdır.";
        return false;

    } else if (!check_validationCode.test(inputValidationCode.value.toUpperCase())) {
        inputValidationCode.classList.add("error");
        validationCodeValidationMessage.innerText = "Lütfen geçerli bir doğrulama kodu giriniz.";
        return false;
    } else {
        inputValidationCode.classList.remove("error");
        validationCodeValidationMessage.innerText = "";
        return true;
    }



}



function validateNewPassword() {

    var inputNewPassword = document.querySelector("#newPassword");
    var newPasswordValidationMessage = document.querySelector("#newPasswordValidationMessage");
    var newPasswordSendBtn = document.querySelector("#newPasswordSendBtn");


    if (inputNewPassword.value == "") {

        inputNewPassword.classList.add("error");
        newPasswordValidationMessage.innerText = "Şifre alanı boş geçilemez.";
        return false;

    } else {
        inputNewPassword.classList.remove("error");
        newPasswordValidationMessage.innerText = "";
        return true;
    }




}


function toastShow(bg, message) {

    var toastResult = document.querySelector("#toast-result");
    var toastResultBody = document.querySelector("#toast-result .toast-body");

    if (bg == "bg-success") {
        toastResult.classList.remove("bg-danger");

    } else if (bg == "bg-danger") {
        toastResult.classList.remove("bg-success");

    }

    toastResult.classList.add(bg);
    toastResultBody.innerHTML = `<span>${message}</span>`;

    var toast = new bootstrap.Toast(toastResult, {
        animation: true,
        autohide: true,
        delay: 4000
    });
    toast.show();

}


emailSendOperations();








function emailSendOperations() {


    var inputEmail = document.querySelector("#email");
    var emailSendBtn = document.querySelector("#emailSendBtn");
    var emailValidationMessage = document.querySelector("#emailValidationMessage");
    var check_email = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;







    inputEmail.addEventListener("keyup", function (e) {
        validateEmail();

    });

    inputEmail.addEventListener("onfocus", function () {
        validateEmail();
    });


    var forgotPasswordForm = document.querySelector(".forgot-password-form");

    var loadingContainer = document.querySelector(".loading-container");

    var forgotPasswordFormInnerContainer = document.querySelector(".forgot-password-form-inner-container");

    var forgotPasswordProgressBar = document.querySelector(".forgot-password-progress-bar");






    emailSendBtn.addEventListener("click", function (e) {

        if (validateEmail()) {

            $.ajax({
                method: "POST",
                url: "/Home/SendEmail",
                data: { email: inputEmail.value.trim() },
                beforeSend: function () {

                    $(".forgot-password-form").hide();

                    loadingContainer.classList.remove("d-none");



                }
            }).done(function (validateEmailResult) {

                if (validateEmailResult[0] == "ok") {

                    $.ajax({
                        method: "POST",
                        url: "/Home/GetValidationForm",
                        beforeSend: function () {

                            toastShow("bg-success", validateEmailResult[1]);

                        }

                    }).done(function (getValidationFormResult) {

                        $(".forgot-password-form").remove();

                        loadingContainer.classList.add("d-none");

                        forgotPasswordFormInnerContainer.insertAdjacentHTML("afterbegin", getValidationFormResult);

                        $(".forgot-password-progress-bar").css("width", "50%");


                        validateCodeOperations();


                    });

                } else if (validateEmailResult[0] == "error") {
                    $(".forgot-password-form").show();

                    loadingContainer.classList.add("d-none");

                    toastShow("bg-danger", validateEmailResult[1]);

                }



            });


        }




    });




}


function validateCodeOperations() {

    var inputValidationCode = document.querySelector("#validationCode");
    var validationCodeValidationMessage = document.querySelector("#validationCodeValidationMessage");
    var validationCodeSendBtn = document.querySelector("#validationCodeSendBtn");


    var forgotPasswordForm = document.querySelector(".forgot-password-form");

    var loadingContainer = document.querySelector(".loading-container");

    var forgotPasswordFormInnerContainer = document.querySelector(".forgot-password-form-inner-container");

    var forgotPasswordProgressBar = document.querySelector(".forgot-password-progress-bar");



    inputValidationCode.addEventListener("onfocus", function () {
        validateCode();
    });

    inputValidationCode.addEventListener("keyup", function () {
        validateCode();
    });

    validationCodeSendBtn.addEventListener("click", function () {


        if (validateCode()) {

            $.ajax({
                method: "POST",
                url: "/Home/ValidateCode",
                data: { code: inputValidationCode.value.toUpperCase().trim() },
                beforeSend: function () {

                    $(".forgot-password-form").hide();

                    loadingContainer.classList.remove("d-none");

                }
            }).done(function (validateCodeResult) {

                if (validateCodeResult[0] == "ok") {

                    $.ajax({
                        method: "POST",
                        url: "/Home/GetNewPasswordForm",
                        beforeSend: function () {

                            toastShow("bg-success", validateCodeResult[1]);

                        }

                    }).done(function (getNewPasswordFormResult) {


                        $(".forgot-password-form").remove();

                        loadingContainer.classList.add("d-none");

                        forgotPasswordFormInnerContainer.insertAdjacentHTML("afterbegin", getNewPasswordFormResult);

                        $(".forgot-password-progress-bar").css("width", "83.3%");

                        newPasswordOperations();



                    });

                } else if (validateCodeResult[0] == "error") {

                    $(".forgot-password-form").show();

                    loadingContainer.classList.add("d-none");

                    toastShow("bg-danger", validateCodeResult[1]);

                }



            });


        }


    });



}



function newPasswordOperations() {


    var inputNewPassword = document.querySelector("#newPassword");
    var newPasswordValidationMessage = document.querySelector("#newPasswordValidationMessage");
    var newPasswordSendBtn = document.querySelector("#newPasswordSendBtn");




    if (inputNewPassword != null) {


        inputNewPassword.addEventListener("keyup", function () {
            validateNewPassword();

        });

        inputNewPassword.addEventListener("onfocus", function () {
            validateNewPassword();
        });


        var forgotPasswordForm = document.querySelector(".forgot-password-form");

        var loadingContainer = document.querySelector(".loading-container");

        var forgotPasswordFormInnerContainer = document.querySelector(".forgot-password-form-inner-container");

        var forgotPasswordProgressBar = document.querySelector(".forgot-password-progress-bar");




        newPasswordSendBtn.addEventListener("click", function (e) {

            if (validateNewPassword()) {

                $.ajax({
                    method: "POST",
                    url: "/Home/NewPassword",
                    data: { newPassword: inputNewPassword.value.trim() },
                    beforeSend: function () {

                        $(".forgot-password-form").hide();

                        loadingContainer.classList.remove("d-none");





                    }
                }).done(function (newPasswordResult) {

                    if (newPasswordResult[0] == "ok") {

                        toastShow("bg-success", newPasswordResult[1]);
                        $(".forgot-password-form").remove();

                        loadingContainer.classList.add("d-none");

                        $(".forgot-password-progress-bar").css("width", "100%");


                        $(".forgot-password-done").show("slow");

                        setTimeout(function () {
                            window.location.href = "/Home/Login/";
                        }, 3000);



                    } else if (validateEmailResult[0] == "error") {
                        $(".forgot-password-form").show();

                        loadingContainer.classList.add("d-none");

                        toastShow("bg-danger", newPasswordResult[1]);

                    }



                });


            }




        });

    }


}



