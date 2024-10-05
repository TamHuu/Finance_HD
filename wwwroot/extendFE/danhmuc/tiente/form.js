let data
$(document).ready(function () {
    loadDanhSach();
});

function loadDanhSach() {
    $('#btnSave').on('click', function (e) {
        e.preventDefault();
       
        var ma = $('#Ma').val();
        var code = $('#Code').val();
        var ten = $('#Ten').val();
        var status = $('#Status').val();
        var url = ma !== defaultUID ? '/Monetary/Edit' : '/Monetary/Add';
        var formdata = {
            Ma: ma,
            Code: code,
            Ten: ten,
            Status: status
        };
        console.table(formdata)

        $.ajax({
            url: url,
            type: 'post',
            data: formdata,
            success: function (response) {
                if (response.success) {
                    swal.fire({
                        title: 'thành công!',
                        text: response.message,
                        icon: 'success'
                    }).then(() => {
                        window.location.href = "/Monetary";
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
    });


}

