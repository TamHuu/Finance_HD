﻿let data
$(document).ready(function () {
    loadDanhSach();
});

function loadDanhSach() {
    $('#btnSave').on('click', function (e) {
        e.preventDefault();
       
        var ma = $('#Ma').val();
        var ten = $('#Ten').val();
        var code = $('#Code').val();
        var status = $('#Status').val();
        var loaithuchi = $('#LoaiThuChi').val();
        var noibo = $('#NoiBo').is(':checked');
        var formData = {
            Ma: ma,
            Ten: ten,
            Code: code,
            Status: status,
            MaLoaiThuChi: loaithuchi,
            NoiBo: noibo,

        };
        var url = ma !== defaultUID ? '/IncomeContent/Edit' : '/IncomeContent/Add';
      
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
                        window.location.href = "/IncomeContent";
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
