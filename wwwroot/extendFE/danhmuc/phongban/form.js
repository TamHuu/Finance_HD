$(document).ready(function () {
    $('#btnSave').on('click', function (e) {
        e.preventDefault();

        // Lấy giá trị từ từng trường
        var ma = $('#Ma').val();
        var defaultUID = $('#DefaultUID').val();
        var code = $('#Code').val();
        var branch = $('#Branch').val();
        var tenBan = $('#TenBan').val();
        var tenPhong = $('#Ten').val();
        var ban = $('#Ban').is(':checked'); // true nếu checkbox được chọn
        var coSoQuy = $('#CoSoQuy').is(':checked'); // true nếu checkbox được chọn
        var status = $('#Status').val();
        var url = ma !== defaultUID ? '/Department/Edit' : '/Department/Add';
        // Tạo một đối tượng dữ liệu để gửi
        var formData = {
            Ma: ma,
            MaChiNhanh: branch,
            Code: code,
            Ten: tenPhong,
            TenPhong: tenPhong,
            CoSoQuy: coSoQuy,
            Ban: ban,
            Status: status
        };
        console.table(formData)

        $.ajax({
            url: url,
            type: 'POST',
            data: formData,
            success: function (response) {
                if (response.success) {
                    Swal.fire({
                        title: 'Thành công!',
                        text: response.message,
                        icon: 'success'
                    }).then(() => {
                        window.location.href = "/Department";
                    });
                } else {
                    Swal.fire({
                        title: 'Thất bại!',
                        text: response.message,
                        icon: 'error'
                    });
                }
            },
            error: function (xhr, status, error) {
                Swal.fire({
                    title: 'Đã xảy ra lỗi!',
                    text: 'Vui lòng thử lại.',
                    icon: 'error'
                });
                console.error(error);
            }
        });
    });


});
