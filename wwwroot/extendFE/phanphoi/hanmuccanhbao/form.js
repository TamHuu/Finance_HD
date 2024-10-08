$(document).ready(function () {
    $('#btnSave').on('click', function (e) {
        e.preventDefault();
        var ma = $('#Ma').val();
        var HanMuc = $('#HanMuc').val();
        var Department = $('#Department').val();
        var Status = $('#Status').val()
        var formData = {
            Ma: ma,
            MaBoPhan: Department,
            HanMuc: HanMuc,
            Status: Status,
        }
        console.table(formData)
        var url = ma !== defaultUID ? '/WarningLimit/Edit' : '/WarningLimit/Add';
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
                        window.location.href = "/WarningLimit";
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
