$(document).ready(function () {
    $('#Logout').on('click', function (e) {
        e.preventDefault();
        $.ajax({
            type: 'GET',
            url: '/Account/Logout',
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
});