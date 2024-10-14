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
            let messageTitle = response.success ? 'Thành công!' : 'Thất bại!';
            let messageIcon = response.success ? 'success' : 'error';

            swal.fire({
                title: messageTitle,
                text: response.message,
                icon: messageIcon
            }).then(() => {
                if (response.success) {
                    window.location.href = "/";
                }
            });
        },
        error: function (xhr, status, error) {
            swal.fire({
                title: 'Đã xảy ra lỗi!',
                text: 'Vui lòng thử lại.',
                icon: 'error'
            });
            console.error('Error: ', error);
        }
    });
});
