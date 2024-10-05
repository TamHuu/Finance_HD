let data
$(document).ready(function () {
    loadDanhSach();
});

function loadDanhSach() {
    $('#btnSave').on('click', function (e) {
        e.preventDefault();
       
        var ma = $('#Ma').val();
        var monetary = $('#Monetary').val();
        var giaTri = $('#GiaTri').val();
        var code = $('#Code').val();
        var status = $('#Status').val();
        
        var formData = {
            Ma: ma,
            MaTienTe: monetary,
            GiaTri: giaTri,
            Code: code,
            Status: status
        };
        var url = ma !== defaultUID ? '/Currency/Edit' : '/Currency/Add';
      
        console.table(formData)

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
                        window.location.href = "/Currency";
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

