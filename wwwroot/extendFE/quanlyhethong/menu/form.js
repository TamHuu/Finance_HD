$(document).ready(function () {
    $('#btnSave').click(function () {
        let formData = {
            Ma: $('#Ma').val(),
            UsingFor: $('#cboUsingFor').val(),
            Code: $('#Code').val(),
            MenuCha: $('#MenuCha').val(),
            Name: $('#Name').val(),
            STT: $('#STT').val(),
            Url: $('#Url').val(),
            Icon: $('#Icon').val(),
            MenuCon: $('#MenuCon').val(),
            Status: $('#Status').val()
        };
        var url = isEdit ? '/Menu/Edit' : '/Menu/Add';
        console.log("dữ liệu gửi đi", formData);

        $.ajax({
            url: url,
            type: 'POST',
            data: formData,
            success: function (response) {
                if (response.success) {
                    swal.fire({
                        title: 'thành công!',
                        text: response.message,
                        icon: 'success'
                    }).then(() => {
                        window.location.href = "/Menu";
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


});
