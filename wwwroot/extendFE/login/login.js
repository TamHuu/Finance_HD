$('#loginForm').submit(function (e) {
    e.preventDefault();

    const username = $('#username').val();
    const password = $('#password').val();

    const data = {
        Username: username,
        Password: password
    };

    $.ajax({
        type: 'POST',
        url: '/Account/Login/Login',
        data: JSON.stringify(data),
        contentType: "application/json",
        success: function (response) {
            if (response.success) {
                alert(response.message);
                window.location.href = "/"; 
            } else {
                alert(response.message);
            }
        },
        error: function () {
            alert("Đã xảy ra lỗi trong quá trình đăng nhập.");
        }
    });
});
