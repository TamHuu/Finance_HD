$(document).ready(function () {
    $('#btnSave').on('click', function (e) {
        e.preventDefault();

        var ma = $('#Ma').val();
        var name = $('#Name').val();
        var code = $('#Code').val();
        var menu = $('#Menu').val();
        var status = $('#Status').val();
        var url = ma !== defaultUID ? '/Report/Edit' : '/Report/Add';

        var data = {
            Ma: ma,
            Name: name,
            Code: code,
            Menu: menu,
            Status: status,
        };

        $.ajax({
            url: url,
            type: 'POST',
            data: data,
            success: function (response) {
                if (response.success) {
                    Swal.fire({
                        title: 'Thành công!',
                        text: response.message,
                        icon: 'success'
                    }).then(() => {
                        window.location.href = "/Role";
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
