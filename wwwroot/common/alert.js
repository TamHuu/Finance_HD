$.ajax({
    url: url,
    type: 'post',
    data: formData,
    success: function (response) {
        if (response.success) {
            swal.fire({
                title: 'thành công!',
                text: response.message,
                icon: 'success'
            }).then(() => {
                window.location.href = "/ExchangeRate";
            });
        } else {
            swal.fire({
                title: 'thất bại!',
                text: response.message,
                icon: 'error'
            });
        }
    },
    error: function (xhr, status, error) {
        swal.fire({
            title: 'đã xảy ra lỗi!',
            text: 'vui lòng thử lại.',
            icon: 'error'
        });
        console.error(error);
    }
});