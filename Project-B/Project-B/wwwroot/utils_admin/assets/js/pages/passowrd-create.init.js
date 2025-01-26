// Xử lý hiển thị mật khẩu
Array.from(document.querySelectorAll("form .auth-pass-inputgroup")).forEach(function (s) {
    Array.from(s.querySelectorAll(".password-addon")).forEach(function (t) {
        t.addEventListener("click", function () {
            var e = s.querySelector(".password-input, .confirm-password-input");
            e.type === "password" ? e.type = "text" : e.type = "password";
        });
    });
});

// Kiểm tra độ mạnh của mật khẩu
var myInput = document.getElementById("password-input");
var letter = document.getElementById("pass-lower");
var length = document.getElementById("pass-length");

myInput.onfocus = function () {
    document.getElementById("password-contain").style.display = "block";
};

myInput.onblur = function () {
    document.getElementById("password-contain").style.display = "none";
};

myInput.onkeyup = function () {
    // Kiểm tra chữ thường
    if (myInput.value.match(/[a-z]/g)) {
        letter.classList.remove("invalid");
        letter.classList.add("valid");
    } else {
        letter.classList.remove("valid");
        letter.classList.add("invalid");
    }

    // Kiểm tra độ dài
    if (myInput.value.length >= 8) {
        length.classList.remove("invalid");
        length.classList.add("valid");
    } else {
        length.classList.remove("valid");
        length.classList.add("invalid");
    }
};
