let data
$(document).ready(function () {
    loadDanhSach();
});

function loadDanhSach() {
    $('#btnSave').on('click', function (e) {
        e.preventDefault();
       
        var ma = $('#Ma').val();
        var ngayApDung = $('#NgayApDung').val();
        var monetary = $('#Monetary').val();
        var tyGia = $('#TyGia').val();
        var status = $('#Status').val();
        
        var formData = {
            Ma: ma,
            MaTienTe: monetary,
            TyGia: tyGia,
            Status: status,
            NgayApDung: ngayApDung,
        };
        var url = ma !== defaultUID ? '/ExchangeRate/Edit' : '/ExchangeRate/Add';
      
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
    });


}

