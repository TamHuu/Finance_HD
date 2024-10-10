$(document).ready(function () {
    $('#loginForm').on('submit', function (e) {
        e.preventDefault(); // Ngăn form submit theo cách thông thường

        // Lấy giá trị từ các input
        var username = $('#username').val();
        var password = $('#password').val();
        var formData = { username, password }
        console.log(formData)
        $.ajax({
            url: '/Login/Login',
            type: 'POST',
            contentType: 'application/json', // Thêm header này để chỉ rõ định dạng dữ liệu
            data: JSON.stringify(formData), // Chuyển đổi đối tượng thành JSON
            success: function (response) {
                // Xử lý phản hồi thành công từ API
                if (response.success) {
                    // Nếu có token trong phản hồi, lưu nó vào localStorage
                    if (response.token) {
                        localStorage.setItem('jwtToken', response.token);
                    }
                    alert("Đăng nhập thành công!");
                    window.location.href = "/"; // Chuyển hướng về trang chính
                } else {
                    alert("Sai tên đăng nhập hoặc mật khẩu!");
                }
            },
            error: function (error) {
                // Xử lý khi có lỗi xảy ra
                console.log(error);
                alert("Đã xảy ra lỗi khi đăng nhập!");
            }
        });

    });
});
